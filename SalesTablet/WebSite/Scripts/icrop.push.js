/**
 * @fileOverview Push通知中継機能(ブラウザ).
 * 
 * @author SKFC浅井
 * @version 1.00/GL
 */


if ((typeof icrop) == "undefined") {
	var icrop = {};
}


/**
 * 当機能を実装するオブジェクト
 */
icrop.push = {};


/**
 * WebSocketの接続先URL
 * @return {String}
 */
icrop.push.C_WEB_SOCKET_URL = "ws://127.0.0.1:51002";


/**
 * 接続タイプの定数
 * @return {Integer}
 */
icrop.push.C_CONNECTION_TYPE_NONE = 0;
icrop.push.C_CONNECTION_TYPE_WEB_SOCKET = 1;
icrop.push.C_CONNECTION_TYPE_URL_SCHEME = 2;


/**
 * URLスキーム名の定数(関連:icrop.push._getActionType)
 * @return {String}
 */
icrop.push.C_URL_SCHEME_ICROP = "icrop";


/**
 * URLスキームのログインコマンド名の定数
 * @return {String}
 */
icrop.push.C_COMMAND_LOGIN = "icrop:lgin:";


/**
 * URLスキームのログアウトコマンド名の定数
 * @return {String}
 */
icrop.push.C_COMMAND_LOGOUT = "icrop:lgot:";


/**
 * URLスキームに続く命令文字列の定数(関連:icrop.push._getActionType)
 * @return {String}
 */
icrop.push.C_ICROP_ACTION = "action";
icrop.push.C_ICROP_XML = "xml";


/**
 * 命令タイプ定数(関連:icrop.push._getActionType)
 * @return {Integer}
 */
icrop.push.C_ACTION_TYPE_ICROP_NONE = 0;
icrop.push.C_ACTION_TYPE_ICROP_ACTION = 1;
icrop.push.C_ACTION_TYPE_ICROP_XML = 2;


/**
 * URLスキームおよびそのデータのデリミタ
 * @return {String}
 */
icrop.push.C_COMMAND_DELIMITER = ":";

/**
 * URLスキームの初回実行間隔(ミリ秒)。iOSアプリで使用．
 * @return {Number}
 */
icrop.push.C_URLSCHEME_EXECUTION_INIT_INTERVAL_IN_MILLISECONDS = 1;

/**
 * URLスキームの実行間隔(ミリ秒)。iOSアプリで使用.
 * @return {Number}
 */
icrop.push.C_URLSCHEME_EXECUTION_INTERVAL_IN_MILLISECONDS = 500;

/**
 * Initializeの実行の遅延時間
 * @return {Number}
 */
icrop.push.C_INITIALIZE_DELAY_IN_MILLISECONDS = 500;

/**
 * WebSocket利用時 menuRedirectCallback のコールバック関数を呼び出す前の待ち時間
 * @return {Number}
 */
icrop.push.C_WAIT_TIME_BEFORE_MENUREDIRECTCALLBACK_IN_MILLISECONDS = 500;


/**
 * WebSocketのインスタンスを保持。PC端末アプリで使用.
 * @return {WebSocket}
 */
icrop.push._websocket = null;


/**
 * icrop.getUser関数から取得するユーザIDを保持する.
 * この値は _initializeUser 関数で設定する.
 * @return {String}
 */
icrop.push._iCropUserID = null;


/**
 * Pushサーバーにログイン要求を行ったユーザIDを保持する.
 * @return {Array}
 */
icrop.push._loginUsers = [];


/**
 * 実行待ちのコマンドを保持するキュー
 * @return {Array}
 */
icrop.push._commandQueue = [];

/**
 * URLスキーム実行キューを処理中かどうかのフラグ.
 * @return {Boolean}
 */
icrop.push._isInExecutingUrlSchemeQueue = false;


/**
 * Push通知中継機能に接続するための初期化
 * および getUser関数より取得したユーザIDでのログインコマンド実行.
 *
 * @return {Boolean} 初期化成功時に真を返す
 */
icrop.push.Initialize = function() {
	var success = false;
	
	try {
		if (icrop.push._connectionType === icrop.push.C_CONNECTION_TYPE_URL_SCHEME) {
			success = true;
		}
		else if (icrop.push._connectionType === icrop.push.C_CONNECTION_TYPE_WEB_SOCKET) {
			icrop.push._initializeWebSocket();
			success = true;
		}
		else {
			icrop.push._log("icrop.push.js does not support this browser");
		}
		icrop.push._initializeUser();
	}
	catch (e) {
		icrop.push._sendLog(
			"error at icrop.push.Initialize - error=" + icrop.push._getDetailError(e));
		success = false;
	}

	icrop.push._log("icrop.push.Initialize returns " + success);
	return success;
};


/**
 * Push通知中継機能へのコマンド送信.
 *
 * @param {String} aCommandString 送信する命令文字列
 *   文字列以外またはコロンを含まない文字列を指定した場合は処理を実行しない
 * @example icrop.push.Execute("icrop:log:HELLO");
 */
icrop.push.Execute = function(aCommandString){
	if ((typeof aCommandString) !== "string") {
		icrop.push._log("icrop.push.Executeに渡された引数が文字列では無いため処理を実行しません");
		return;
	}

	if (aCommandString.indexOf(icrop.push.C_COMMAND_DELIMITER) === -1) {
		icrop.push._log("icrop.push.Executeに渡された引数に :(コロン) が含まれないため処理を実行しません");
		return;
	}

	icrop.push._manageLoginUsers(aCommandString);

	switch(icrop.push._connectionType) {
		case icrop.push.C_CONNECTION_TYPE_URL_SCHEME:
			icrop.push._executeWithUrlScheme(aCommandString);
			break;
			
		case icrop.push.C_CONNECTION_TYPE_WEB_SOCKET:
			icrop.push._executeWithWebSocket(aCommandString);
			break;
		
		case icrop.push.C_CONNECTION_TYPE_NONE:
		default:
            icrop.push._log("Connection type is none");
			break;
	} // end of switch
};


/**
 * Push通知転送I/F形式のXMLをPush通知中継機能へ送信
 * 
 * @param aXmlString XMLノードではなくXML文字列
 *   ここに含まれる改行は半角スペースに変換される
 *   この文字列はXMLとして不正な場合はXMLの送信は実行されない
 */
icrop.push.SendXml = function(aXmlString) {
	if ( ! icrop.push._validateXml(aXmlString)) {
		icrop.push._sendLog("icrop.push.SendXml : aXmlString is invalid. aXmlString = " + aXmlString);
		return;
	}

	aXmlString = aXmlString.replace(/(?:\r\n|\r|\n)/g, " ");

	switch(icrop.push._connectionType) {
		case icrop.push.C_CONNECTION_TYPE_URL_SCHEME:
			icrop.push._xmlQueue.push(aXmlString);
			icrop.push.Execute("icrop:xmlforward?paramMethod=icrop.push.GetXml");
			break;

		case icrop.push.C_CONNECTION_TYPE_WEB_SOCKET:
			icrop.push.Execute("icrop:xml:" + aXmlString);
			break;

		default:
			break;
	} // end of switch
};

/**
 * URLスキームを実行キューに追加.
 *
 * URLスキームを短時間に連続実行すると最後のURLスキームしか
 * 実行されないため一定時間ごとにURLスキームを実行する.
 *
 * @param urlScheme 実行するURLスキーム
 */
icrop.push._registerUrlSchemeQueue = (function(){
	// キューに登録されたコマンドを実行
	var executeUrlScheme = function() {
		if (icrop.push._commandQueue.length != 0) {
			var command = icrop.push._commandQueue.shift();
			window.location = command;
			icrop.push._log("icrop.push._registerUrlSchemeQueue send aCommandString=" + command);
			setTimeout(executeUrlScheme, icrop.push.C_URLSCHEME_EXECUTION_INTERVAL_IN_MILLISECONDS);
		}
		else {
			// キューにデータが存在しない場合は実行フラグをOFF
			icrop.push._isInExecutingUrlSchemeQueue = false;
		}
	};

	// icrop.push._registerUrlSchemeQueue の関数本体
	return function(aUrlScheme) {
		icrop.push._commandQueue.push(aUrlScheme);

		if (!icrop.push._isInExecutingUrlSchemeQueue) {
			// URLスキーム実行中でなければ処理開始
			icrop.push._isInExecutingUrlSchemeQueue = true;
			setTimeout(executeUrlScheme, icrop.push.C_URLSCHEME_EXECUTION_INIT_INTERVAL_IN_MILLISECONDS);
		}
	};
})();


/**
 * icrop:lgin: または icrop:lgot: のコマンドである場合に
 * ユーザIDを _loginUsers に登録または削除する.
 */
icrop.push._manageLoginUsers = function(aCommandString) {
	if (!aCommandString) return;

	if (icrop.push.C_COMMAND_LOGIN === aCommandString.substring(0, icrop.push.C_COMMAND_LOGIN.length)) {
		// ログインコマンドを実行する場合
		var userID = aCommandString.substring(icrop.push.C_COMMAND_LOGIN.length, aCommandString.length);
		if (userID.length != 0) {
			var isContainsUserID = false;
			for (var i = 0, l = icrop.push._loginUsers.length; i < l; i++) {
				if (icrop.push._loginUsers[i] === userID) {
					isContainsUserID = true;
					break;
				}
			}
			
			if (!isContainsUserID) {
				icrop.push._loginUsers.push(userID);
			}
		}
	}
	else if (icrop.push.C_COMMAND_LOGOUT === aCommandString.substring(0, icrop.push.C_COMMAND_LOGOUT.length)) {
		// ログアウトコマンドを実行する場合
		var userID = aCommandString.substring(icrop.push.C_COMMAND_LOGOUT.length, aCommandString.length);
		if (userID.length != 0) {
			for (var i = icrop.push._loginUsers.length - 1; i >= 0; i--) {
				if (userID === icrop.push._loginUsers[i]) {
					icrop.push._loginUsers.splice(i, 1);
				}
			}
		}
	}
};


/**
 * SMBからi-CROPのメインメニューに画面遷移する際のコールバック関数.
 * 基盤より呼び出される。
 * icrop.push ではなく icrop オブジェクトのプロパティである点に注意.
 *
 * @param {String} aScreenID 画面ID
 * @param {Function} aCallback 処理終了後に呼び出す必要のあるコールバック関数
 */
icrop.menuRedirectCallback = function (aScreenID, aCallback) {
	var queue = icrop.push._commandQueue;
	
	// 自動ログアウト
	var loginUsersCopy = icrop.push._loginUsers.slice(0);
	for (var i = 0, l = loginUsersCopy.length; i < l; i++) {
		var userID = loginUsersCopy[i];
		if (userID.length != 0 && userID != icrop.push._iCropUserID) {
			icrop.push.Execute(icrop.push.C_COMMAND_LOGOUT + userID);
		}
	}
	
	// コールバック関数の実行
	switch(icrop.push._connectionType) {
		case icrop.push.C_CONNECTION_TYPE_URL_SCHEME:
			// iOS端末の場合はURLスキームの実行停止状態まで待つ
			var timerID = setInterval(function(){
				if (!icrop.push._isInExecutingUrlSchemeQueue) {
					clearInterval(timerID);
					aCallback();
				}
			}, 8);
			break;

		case icrop.push.C_CONNECTION_TYPE_WEB_SOCKET:
			// PC端末の場合は少し待ってからコール
			setTimeout(function(){
				aCallback();
			}, icrop.push.C_WAIT_TIME_BEFORE_MENUREDIRECTCALLBACK_IN_MILLISECONDS);
			break;

		default:
			aCallback();
			break;
	} // end of switch
};


/**
 * icrop.push._xmlQueueから要素を1つ取り出す(iOS用)
 *
 * @return {String} 
 */
icrop.push.GetXml = function() {
	if (icrop.push._xmlQueue.length != 0) {
		var xml = icrop.push._xmlQueue.shift();
		return xml ? xml : null;
	}
	else {
		icrop.push._sendLog("icrop.push._xmlQueue is empty");
		icrop.push._log("icrop.push._xmlQueue is empty");
		return null;
	}
};


/**
 * SendXml で渡されたXML文字列を保持(iOS用)
 * @return {String}
 */
icrop.push._xmlQueue = [];


/**
 * 接続の種類
 * icrop.push.C_CONNECTION_TYPE_から始まる定数値を保持する
 * @return {String}
 */
icrop.push._connectionType = (function() {
    try {
		if (/like Mac OS X/i.test(navigator.userAgent)) {
			return icrop.push.C_CONNECTION_TYPE_URL_SCHEME;
		}
		else if ("WebSocket" in window) {
			return icrop.push.C_CONNECTION_TYPE_WEB_SOCKET;
		}
		else {
			icrop.push._log("icrop.push.js does not support this browser");
		}
    }
    catch(e) {
        icrop.push._log(icrop.push._getDetailError(e));
    }
    
    return icrop.push.C_CONNECTION_TYPE_NONE;
})();


/**
 * ユーザ初期化処理
 * UserIDを取得してログイン命令を送信
 */
icrop.push._initializeUser = function() {
	if ((typeof icrop.getUser) !== "function") {
		return;
	}
	
	var user = icrop.getUser();
	if (user && user != "") {
		icrop.push._iCropUserID = user;
		icrop.push.Execute("icrop:lgin:" + user);
	}
};


/**
 * WebSocket接続の初期化
 */
icrop.push._initializeWebSocket = function() {
	// WebSocket を使用してPC基盤へセッション確立
	var webSocket = new WebSocket(icrop.push.C_WEB_SOCKET_URL);
	
	webSocket.onmessage = icrop.push._onmessageCallback;

	webSocket.onopen = function(aEvent) {
		icrop.push._log("called WebSocket#onopen");

		// キューに溜まっているコマンドを全て実行しキューを空にする
		while(icrop.push._commandQueue.length != 0) {
			var command = icrop.push._commandQueue.shift();
			icrop.push._websocket.send(command);
			icrop.push._log("icrop.push._executeWithWebSoket send aCommandString=" + command);
		}
	};

	webSocket.onclose = function(aEvent) {
		icrop.push._log("called WebSocket#onclose");
        icrop.push._websocket = null;
        
        // 切断されたら1秒後に再接続
        setTimeout(icrop.push._initializeWebSocket, 1000);
	};

	webSocket.onerror = function(aEvent) {
		icrop.push._log("called WebSocket#onerror");
        icrop.push._log(aEvent);
	};

	icrop.push._websocket = webSocket;
};


/**
 * コマンド文字列に指定された命令の種類を取得する
 *
 * @param {String} aSchemeName 
 *   例えばコマンドが icrop:action:DUMMY の場合は icrop の部分を指定する.
 * @param {String} aActionName 
 *   例えばコマンドが icrop:action:DUMMY の場合は action の部分を指定する.
 * @return {String} icrop.push.C_ACTION_TYPE_から始まる定数を返す.
 *   存在しない場合は icrop.push.C_ACTION_TYPE_ICROP_NONE を返す.
 */
icrop.push._getActionType = function(aSchemeName, aActionName) {
	var type = icrop.push.C_ACTION_TYPE_ICROP_NONE;

	if (aSchemeName === icrop.push.C_URL_SCHEME_ICROP) {
		switch(aActionName) {
			case icrop.push.C_ICROP_ACTION:
				type = icrop.push.C_ACTION_TYPE_ICROP_ACTION;
				break;

			case icrop.push.C_ICROP_XML:
				type = icrop.push.C_ACTION_TYPE_ICROP_XML;
				break;
			
			default:
				break;
		}
	} 

	return type;
};


/**
 * icrop.push._initializeWebSocket実行時に
 * WebSocket#onmessage にセットする関数
 *
 * @param {Object} aEvent WebSocketで受信したデータ
 * aEvent.dataに icrop:action:alert('HELLO') や icrop:xml:[XML文字列] などの命令が含まれる
 */
icrop.push._onmessageCallback = function(aEvent) {
	try {
		var receivedCommand = aEvent.data;
		var commands = receivedCommand.split(icrop.push.C_COMMAND_DELIMITER);
		var actionType = icrop.push._getActionType(commands[0], commands[1]);
		var commandParameter = commands.slice(2).join(icrop.push.C_COMMAND_DELIMITER);

		var executed = false;
		switch (actionType) {
			case icrop.push.C_ACTION_TYPE_ICROP_ACTION:
				eval(commandParameter);
				executed = true;
				break;
			
			case icrop.push.C_ACTION_TYPE_ICROP_XML:
				if ((typeof ReceiveXML) === "function") {
				    ReceiveXML(commandParameter);
					executed = true;
				} else {
					icrop.push._sendLog("ReceiveXML is not function");
					icrop.push._log("ReceiveXMLは関数ではありません");
				}
				break;
			
			case icrop.push.C_ACTION_TYPE_ICROP_NONE:
				break;
				
			default:
				break;
		} // end of switch

		if (!executed) {
			icrop.push._sendLog("command error at icrop.push._onmessageCallback aEvent.data=" + aEvent.data); 
		}
	}
	catch (e) {
		icrop.push._sendLog("error at icrop.push._onmessageCallback aEvent.data="
			+ aEvent.data + " - error=" + icrop.push._getDetailError(e));
	}
};


/**
 * push通知の送信をURLスキームにより実行する(iOS)
 * icrop.push.Executeから呼び出す
 *
 * @param {String} aCommandString
 */
icrop.push._executeWithUrlScheme = function(aCommandString) {
	try {
		icrop.push._registerUrlSchemeQueue(aCommandString);
	}
	catch (e) {
		icrop.push._sendLog("error at icrop.push._executeWithUrlScheme aCommandString="
			+ aCommandString + " - error=" + icrop.push._getDetailError(e));
	}
};


/**
 * push通知の送信をWebSocketに経由で実行する(iOS以外)
 * icrop.push.Executeから呼び出す
 *
 * @param {String} aCommandString 
 */
icrop.push._executeWithWebSocket = function(aCommandString) {
	try {
		if (icrop.push._websocket &&
            icrop.push._websocket.readyState == 1) {
			icrop.push._websocket.send(aCommandString);
			icrop.push._log("icrop.push._executeWithWebSoket send aCommandString=" + aCommandString);
		}
		else if (icrop.push._websocket == null ||
                 icrop.push._websocket.readyState != 1) {
			icrop.push._commandQueue.push(aCommandString);
			icrop.push._log("icrop.push._executeWithWebSoket wait to send aCommandString=" + aCommandString);
		}
	}
	catch (e) {
		icrop.push._sendLog("error at icrop.push._executeWithWebSocket aCommandString="
			+ aCommandString + " - error=" + icrop.push._getDetailError(e));
	}
};


/**
 * XML文字列を解析して正しいXMLであれば真を返す
 *
 * @param {String} aXmlString 文字列以外を渡すと偽を返す
 * @return {Boolean}
 */
icrop.push._validateXml = function(aXmlString) {
	if ( ! aXmlString || (typeof aXmlString) !== "string") {
		return false;
	}

	var xml;
	try {
		if (window.DOMParser) {
			xml = (new DOMParser).parseFromString(aXmlString, "text/xml");
		}
		else {
			xml = new ActiveXObject("Microsoft.XMLDOM");
			xml.async = "false";
			xml.loadXML(aXmlString);
		}
	}
	catch (e) {
		return false;
	}
	
	if ( ! xml || ! xml.documentElement || xml.getElementsByTagName("parsererror").length) {
		return false;
	}
	
	return true;
};


/**
 * ログコマンドをPush通知中継機能へ送信
 *
 * @param {String} 
 */
icrop.push._sendLog = function(aLogString) {
	icrop.push.Execute("icrop:log:" + aLogString);
};


/**
 * ブラウザへコンソール出力
 */
icrop.push._log = function(aLogString) {
	if (window.console && (typeof console.log) == 'function') {
		console.log(aLogString);
	}
};


/**
 * エラーオブジェクトからエラー詳細を取得.
 * @param {Error} aError エラーオブジェクト
 * @return {String} エラー詳細
 */
icrop.push._getDetailError = function(aError) {
	return /Firefox/i.test(navigator.userAgent)
		? (aError.message + "\n" + aError.stack.replace(/(.+)/g, "\tat $1"))
		: (aError.stack || aError.message)
};


//
// Initializeの実行イベント追加
//
(function(){
    var delayedInitialize = function() {
            setTimeout(icrop.push.Initialize, icrop.push.C_INITIALIZE_DELAY_IN_MILLISECONDS);    
    };

    if (window.addEventListener) {
        window.addEventListener("load", delayedInitialize, false);
    }
    else { // for IE
        window.attachEvent("onload", delayedInitialize);
    }
})();

