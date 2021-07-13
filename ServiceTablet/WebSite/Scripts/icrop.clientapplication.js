/**
 * @fileOveriew ClientApplication.
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
if ((typeof icrop.clientapplication) == "undefined") {
	icrop.clientapplication = {};
}


/**
 * 対応ブラウザ起動機能に送信するクッキーの名前を配列で定義。
 * PC端末アプリのMC3A01015(対応ブラウザ起動機能)に関する設定.
 * @return {Array} クッキー名の配列
 */
icrop.clientapplication.C_TARGET_COOKIE_NAMES = [
	"iPlanetDirectoryPro",
	"icrop",
	"amlbcookie",
	"department",
	"displayname",
	"realm",
	"salt",
	"sam",
	"uid"
];

/**
 * 採用しているブラウザとそのブラウザでの表示対応情報の定義リスト。
 * PC端末アプリのMC3A01015(対応ブラウザ起動機能)に関する設定.
 *
 * @return {Array} 定義の連想配列を配列として保持している。
 * 定義の連想配列はパターン(正規表現)にマッチするUser-Agentを持つブラウザ
 * が対応しているページタイプが定義されている。
 * "regex"キーにパターン(正規表現)を定義する。
 * "supportedContentTypes"キーに対応ページタイプ(HTML4, HTML5)の配列を定義する。
 */
icrop.clientapplication.C_BROWSER_INFO_ARRAY = [
	// Safari
	{ "regex" : /AppleWebKit\/[^\/]+Version.+ Safari/, "supportedContentTypes" : ["HTML5"] },

	// Internet Explorer 8
	{ "regex" : /MSIE 8\.0/, "supportedContentTypes" : ["HTML4"] },

	// Internet Explorer 9
	{ "regex" : /MSIE 9\.0/, "supportedContentTypes" : ["HTML4"] },

	// Internet Explorer 10
	{ "regex" : /MSIE 10\.0/, "supportedContentTypes" : ["HTML4", "HTML5"] },

	// Internet Explorer 11以上
	{ "regex" : /\WTrident\/.+?\Wrv:?\s?(?:1[1-9]|[2-9][0-9])/, "supportedContentTypes" : ["HTML4", "HTML5"] }
];


/**
 * URLスキームの連続実行時の実行間隔(ミリ秒)
 * @return {Number}
 */
icrop.clientapplication.C_URLSCHEME_EXECUTION_INTERVAL = 1000;


/**
 * push通知の送信を実行する。
 * icrop.push.Executeメソッドのラッパーです。
 * icrop.push.js を読み込む前に実行すると何も処理を行いません.
 *
 * @param {String} aCommandString 送信する命令文字列
 *   文字列以外またはコロンを含まない文字列を指定した場合は処理を実行しない
 */
icrop.clientapplication.Execute = function(aCommandString) {
	if (icrop.push && (typeof icrop.push.Execute) === 'function') {
		icrop.push.Execute(aCommandString);
		return;
	}
	
	if (window.console) {
		console.log("icrop.push.Execute is not function. aCommandString = "
                    + aCommandString);
	}
};


/**
 * Push通知転送I/F形式のXMLをPush通知中継機能へ送信
 * icrop.push.SendXmlメソッドのラッパーです。
 * icrop.push.js を読み込む前に実行すると何も処理を行いません.
 * 
 * @param aXmlString XMLノードではなくXML文字列
 *   ここに含まれる改行は半角スペースに変換される
 *   この文字列はXMLとして不正な場合はXMLの送信は実行されない
 */
icrop.clientapplication.SendXml = function(aXmlString) {
	if (icrop.push && (typeof icrop.push.SendXml) === 'function') {
		icrop.push.SendXml(aXmlString);
		return;
	}
	
	if (window.console) {
		console.log("icrop.push.SendXml is not function. aXmlString = "
		                + aXmlString);
	}
};


/**
 * ブラウザのコンソールおよび端末アプリへのログ出力.
 *
 * @param {String} aMessage 出力する文字列
 */
icrop.clientapplication._log = function(aMessage) {
	// コンソール出力
	if (window.console && (typeof console.log) == 'function') {
		console.log("" + aMessage);
	}
	
	// 端末へのログ出力
	icrop.clientapplication.Execute("icrop:log:" + aMessage);
};


/**
 * 関数実行キューに関数を追加.
 *
 * @param aFunction 実行する関数
 * @param aNextScheduleTime 登録した関数実行後、次の関数を実行するまでの間隔を指定
 */
icrop.clientapplication._enqueueFunctionSchedule = (function() {
	// 関数を保持するキュー
	var _funcQueue = [];
	
	// キューの実行中フラグ
	var _isInRunningFunction = false;
	
	// キューに登録されたコマンドを実行
	var _runFunction = function() {
		if (_funcQueue.length != 0) {
			var info = _funcQueue.shift();
			
			try {
				info[0](); // 関数実行
			}
			catch (error) {
				icrop.clientapplication._log(
					"icrop.clientapplication._enqueueFunctionSchedule "
					+ "function = " + info[0]
					+ ", next = " + info[1]
					+ ", error = " + icrop.clientapplication._getDetailError(error));
			}
			
			if (info[1] == 0) {
				_runFunction();
			}
			else {
				setTimeout(_runFunction, info[1]);
			}
		}
		else {
			// キューにデータが存在しない場合は実行フラグをOFF
			_isInRunningFunction = false;
		}
	};

	// icrop.push._enqueueFunctionSchedule の関数本体
	return function(aFunction, aNextScheduleTime) {
		// パラメータチェック
		if (typeof aFunction != "function") {
			throw new Error("aFunction type must be function");
		}
		else if (typeof aNextScheduleTime != "number") {
			throw new Error("aNextScheduleTime type must be number");
		}
		else if (aNextScheduleTime < 0) {
			throw new Error("aNextScheduleTime cannot be minus");
		}
		
		// キューに追加
		_funcQueue.push([aFunction, aNextScheduleTime]);

		if ( ! _isInRunningFunction) {
			// キューが実行中でなければ実行状態にする
			_isInRunningFunction = true;
			_runFunction();
		}
	};
})();


/**
 * スクリーンセーバ操作機能の機能有効化スイッチを切替えるリクエストを送信する。
 *
 * @param {boolean} enabled 有効化スイッチの設定値
 *  true:スイッチON false:スイッチOFF.
 */
icrop.clientapplication.SwitchMC3A01013FunctionEnabled = function (enabled) {
	if (typeof enabled === 'boolean') {
		var xml = '';
		xml += '<?xml version="1.0" encoding="utf-8" ?>';
		xml += '<CommonCommunication>';
		xml += '  <Head>';
		xml += '    <Origin>';
		xml += '      <UserID></UserID>';
		xml += '      <ApplicationID>iCROP</ApplicationID>';
		xml += '    </Origin>';
		xml += '    <Destination>';
		xml += '      <UserID></UserID>';
		xml += '      <ApplicationID>MC3A01013</ApplicationID>';
		xml += '    </Destination>';
		xml += '  </Head>';
		xml += '  <Detail>';
		xml += '    <MC3A01013>';
		xml += '      <FunctionEnabled>' + (enabled ? 'True' : 'False') + '</FunctionEnabled>';
		xml += '    </MC3A01013>';
		xml += '  </Detail>';
		xml += '</CommonCommunication>';
		icrop.clientapplication.SendXml(xml);
	}
	else {
		icrop.clientapplication._log(
			"SwitchMC3A01013FunctionEnabled: invalid arg type = " + (typeof enabled));
	}
}


/**
 * MC3A01014(ウィンドウサイズ変更機能)へのリサイズ要求を実行する。
 * icrop:xml: のURLスキームによりPC端末アプリにXMLを送信する.
 *
 * @param {Integer} aWidth
 *   Width要素に指定する整数値。1以上の値を指定すること。
 *   リサイズ時に変更の対象としない場合は null を指定する。
 *   /^[1-9][0-9]*$/ のパターンにマッチする文字列の指定も可能。
 *   空文字 は null と等価な結果となる。
 * @param {Integer} aHeight
 *   Height要素に指定する整数値。1以上の値を指定すること。
 *   リサイズ時に変更の対象としない場合は null を指定する。
 *   /^[1-9][0-9]*$/ のパターンにマッチする文字列の指定も可能。
 *   空文字 は null と等価な結果となる。
 * @param {Integer} aPositionY
 *   PositionY要素に指定する整数値。
 *   リサイズ時に変更の対象としない場合は null を指定する。
 *   /^(?:-?[1-9][0-9]*|0)$/ のパターンにマッチする文字列の指定も可能。
 *   空文字 は null と等価な結果となる。
 * @param {Integer} aPositionX
 *   PositionX要素に指定する整数値。
 *   リサイズ時に変更の対象としない場合は null を指定する。
 *   /^(?:-?[1-9][0-9]*|0)$/ のパターンにマッチする文字列の指定も可能。
 *   空文字 は null と等価な結果となる。
 * @return {Boolean} 引数が不正な場合はfalse、正しい場合はtrueを返す。
 *   結果がfalseの場合はPC端末アプリへのリサイズ要求は実行されない。
 */
icrop.clientapplication.resizeWindow = function(aWidth, aHeight, aPositionY, aPositionX) {
	try {
		// 引数の数のチェック
		if (arguments.length !== 4) {
			throw new Error("arguments length must be 4. arguments.length = "
				+ arguments.length);
		}
		
		// パラメータの事前変換
		var positionX = aPositionX === "" ? null : aPositionX;
		var positionY = aPositionY === "" ? null : aPositionY;
		var width = aWidth === "" ? null : aWidth;
		var height = aHeight === "" ? null : aHeight;

		// パラメータチェック
		if (positionX !== null && ! /^(?:-?[1-9][0-9]*|0)$/.test(positionX)) {
			throw new Error("aPositionX must match /^(?:-?[1-9][0-9]*|0)$/");
		}

		if (positionY !== null && ! /^(?:-?[1-9][0-9]*|0)$/.test(positionY)) {
			throw new Error("aPositionY must match /^(?:-?[1-9][0-9]*|0)$/");
		}

		if (width !== null && ! /^[1-9][0-9]*$/.test(width)) {
			throw new Error("aWidth must match /^[1-9][0-9]*$/");
		}
		
		if (width !== null && width <= 0) {
			throw new Error("aWidth must be over 1");
		}

		if (height !== null && ! /^[1-9][0-9]*$/.test(height)) {
			throw new Error("aHeight must match /^[1-9][0-9]*$/");
		}
		
		if (height !== null && height <= 0) {
			throw new Error("aHeight must be over 1");
		}
		
		var xml = icrop.clientapplication._createResizePushXml(
			navigator.userAgent, positionX, positionY, width, height);
		icrop.clientapplication.SendXml(xml);
		return true; // 処理途中で例外が発生しなければtrueを返す
	}
	catch (error) {
		icrop.clientapplication._log("icrop.clientapplication.resizeWindow "
			+ "aPositionX = " + aPositionX
			+ ", aPositionY = " + aPositionY
			+ ", aWidth = " + aWidth
			+ ", aHeight = " + aHeight
			+ ", error = " + icrop.clientapplication._getDetailError(error));

		return false;
	}
};


/**
 * e-Mail,LINE送信機能(タブレット端末アプリ)の実行要求を行う。
 *
 * @param {Object} aHash icrop:sendMessage? のパラメータの連想配列
 * @return {String} 実行したURLスキーム
 */
icrop.clientapplication.sendMessage = function(aHash) {
    try {
        var url = "icrop:///sendMessage?";
        url += icrop.clientapplication._buildQuery(aHash);
        icrop.clientapplication.Execute(url);
        return url;
    }
    catch (error) {
        icrop.clientapplication._log(
            "icrop.clientapplication.sendMessage "
            + "aHash = " + JSON.stringify(aHash)
			+ ", Error = " + icrop.clientapplication._getDetailError(error));
        return null;
	}
};


/**
 * スタッフ間e-Mail送信機能(タブレット端末アプリ)の実行要求を行う。
 *
 * @param {Object} aHash icrop:sendSimpleMail? のパラメータの連想配列
 * @return {String} 実行したURLスキーム
 */
icrop.clientapplication.sendSimpleMail = function(aHash) {
  try {
				var url = "icrop:///sendSimpleMail?";
				url += icrop.clientapplication._buildQuery(aHash);
				icrop.clientapplication.Execute(url);

				return url;
  }
  catch (error) {
				icrop.clientapplication._log(
                   "icrop.clientapplication.sendSimpleMail "
                   + "aHash = " + JSON.stringify(aHash)
                   + ", Error = " + icrop.clientapplication._getDetailError(error));
				return null;
  }
};


/**
 * AirPrint機能(タブレット端末アプリ)の実行要求を行う。
 *
 * @param {Object} aHash icrop:prtr? のパラメータの連想配列
 * @return {String} 実行したURLスキーム
 */
icrop.clientapplication.print = function(aHash) {
	try {
		// iOS8で印刷結果が縮小されるバグ(TMT全販社BTS-361)の暫定対応
		if ( ! /OS [1-7](?:_\d+_\d+)? like Mac OS X/i.test(navigator.userAgent)) {
			for (var key in aHash) {
				if (key.toLowerCase() === "htmlwidth") {
					aHash[key] = "700px";
					break;
				}
			}
		}

		// URLを生成して実行
		var url = "icrop:///prtr?";
		url += icrop.clientapplication._buildQuery(aHash);
		icrop.clientapplication.Execute(url);
		return url;
	}
	catch (error) {
		icrop.clientapplication._log(
			"icrop.clientapplication.print "
			+ "aHash = " + JSON.stringify(aHash)
			+ ", Error = " + icrop.clientapplication._getDetailError(error));
		return null;
	}
};


/**
 * e-Mail,LINE送信機能(タブレット端末アプリ)に読込完了を通知する。
 * icrop:messageSendのパラメータSourceにURLが指定された場合に、
 * そのURLのHTML内のJavascriptからHTMLの読込完了時に通知すること.
 */
icrop.clientapplication.sourceLoaded = function() {
    try {
        icrop.clientapplication.Execute("icrop:sourceLoaded");  
    }
    catch (error) {
        icrop.clientapplication._log(
            "icrop.clientapplication.sourceLoaded "
			+ "Error = " + icrop.clientapplication._getDetailError(error));
	}
};


/**
 * 新しくウィンドウを開く(対応ブラウザ起動機能を呼び出す).
 *
 * @param {String} aPageType ページタイプ(必須)
 *   "HTML4"（既存流用等ie8で表示させるページ）
 *   "HTML5"（再構築アプリ等Safariで表示させるページ）
 * @param {String} aTargetUrl 開くURL(必須)
 * @param {Integer} aViewType 表示モード(任意)
 *   0 (i-CROP既定表示モード、新規ウィンドウ表示後上にずらす:デフォルト値)
 *   1（単純に新しいウィンドウで表示）
 * @param {Integer} aCloseFlag 呼び出し元のクローズフラグ(任意)
 *   0 (閉じない:デフォルト値）
 *   1 (閉じる）
 * @param {String} aViewOption オプション(任意)
 *   window.openメソッドのオプションパラメータ
 *   未指定の場合i-CROP既定の値をセット(aViewType = 0 の場合と等価)
 * @return {Boolean} 引数が不正な場合は0を返す。引数が正しい場合で、
 *   UserAgentが /like Mac OS X/i の正規表現に一致する場合は 1 そうでない場合は 2
 */
icrop.clientapplication.openNewWindow = function(
	aPageType,
	aTargetUrl,
	aViewType,
	aCloseFlag,
	aViewOption) {
	try {
		// 引数の数チェック
		if (arguments.length < 2 || arguments.length > 5) {
			throw new Error(
				"arguments length must be range 2-6. arguments.length = "
				+ arguments.length);
		}
		
		// 引数チェック
		if ((typeof aPageType) != "string") {
			throw new Error("aPageType must be string");
		}
		
		if (aPageType.length == 0) {
			throw new Error("aPageType cannot be empty");
		}
		
		if ((typeof aTargetUrl) != "string") {
			throw new Error("aTargetUrl must be string");
		}
		
		if (aTargetUrl.length == 0) {
			throw new Error("aTargetUrl cannot be empty");
		}
		
		if (typeof aViewType != "undefined") {
			if ((typeof aViewType) != "number") {
				throw new Error("aViewType must be Integer");
			}
			
			if (aViewType !== 0 && aViewType !== 1) {
				throw new Error("aViewType must be 0 or 1");
			}
		}
		
		if (typeof aCloseFlag != "undefined") {
			if ((typeof aCloseFlag) != "number") {
				throw new Error("aCloseFlag must be Integer");
			}
			
			if (aCloseFlag !== 0 && aCloseFlag !== 1) {
				throw new Error("aCloseFlag must be 0 or 1");
			}
		}

		var option = (typeof aViewOption) != "undefined" ? aViewOption : "";
		if (option != null && (typeof option) != "string") {
			throw new Error("aViewOption must be string or null or undefined");
		}
		
		if (option == null || option.length === 0) {
			viewType = 0; // optionが未入力の場合はviewType==0の場合と同じ効果
		}
		
		// 処理開始(iOS端末)
		if (/like Mac OS X/i.test(navigator.userAgent)) {
			// iOS端末の場合はただ遷移すればOK
			location.href = aTargetUrl;
			return 1;
		}
		
		// 処理開始(PC)
		if (icrop.clientapplication.checkBrowserSupport(navigator.userAgent,
		                                                 aPageType)) {
			// ブラウザがpageTypeに対応している場合は新しくウィンドウを開く
			if ((typeof aViewType) == "undefined" || aViewType === 0) {
				// 未定義 または 0 はフルスクリーン
				icrop.clientapplication._openFullScreenWindow(aTargetUrl);
			}
			else if (aViewType === 1) {
				// 1 はそのまま開く
				if (window.open(aTargetUrl, null, option)) {
					window.location = aTargetUrl;
				}
			}
			else {
				throw new Error("invalid viewType");
			}
		}
		else {
			// 非対応の場合はブラウザ間遷移機能へ
			var schemes = icrop.clientapplication._createBrowserCallUrlSchemes(
				aPageType,
				icrop.clientapplication._convertToAbsoluteUrl(aTargetUrl, location.href),
				aViewType,
				option);

			var openInIframe = function(aUrlScheme) {
				var iframe = window.document.createElement('iframe');
				iframe.style.display = 'none';
				window.document.body.appendChild(iframe);
				setTimeout(function() {
					iframe.parentNode.removeChild(iframe);
				}, 10000);

				if (iframe.src) {
					iframe.src = aUrlScheme;
				}
				else if (iframe.contentWindow || iframe.contentWindow.location) {
					iframe.contentWindow.location = aUrlScheme;
				}
				else {
					iframe.setAttribute("src", aUrlScheme);
				}
			};
			
			// IFRAMEにより実行するかどうかの判定
			// ※Safari, IE8~11, FireFox はIFRAMEにより実行可能であることを確認済み
			var shouldOpenInIframe = navigator.userAgent.indexOf('Chrome') === -1; 
			
			for (var i = 0; i < schemes.length; i++) {
				if (shouldOpenInIframe) {
						openInIframe(schemes[i]);
				}
				else {
					icrop.clientapplication._enqueueFunctionSchedule((function(aUrlScheme) {
						return function() {
							location.href = aUrlScheme;
						};
					})(schemes[i]), icrop.clientapplication.C_URLSCHEME_EXECUTION_INTERVAL);
				}
			}
		}
		
		if (aCloseFlag == 1) {
			icrop.clientapplication._enqueueFunctionSchedule(function() {
				setTimeout(function() {
					icrop.clientapplication._closeWindow();
				}, 500);
			}, 0);
		}
		
		return 2;
	}
	catch (error) {
		icrop.clientapplication._log("icrop.clientapplication.openNewWindow "
			+ "aPageType = " + aPageType
			+ ", aTargetUrl = " + aTargetUrl
			+ ", aViewType = " + aViewType
			+ ", aViewOption = " + aViewOption
			+ ", User-Agent = " + navigator.userAgent
			+ ", error = " + icrop.clientapplication._getDetailError(error));

		return 0;
	}
};


/**
 * MC3A01014(ウィンドウリサイズ機能)へ送信するXML文字列
 * を作成する。受け取った引数が不正な場合は例外を送出する。
 * icrop.clientapplication.resizeWindowの補助関数.
 *
 * @param {String} aResizeTarget
 *   ResizeTarget要素に指定するリサイズ対象を識別するための文字列。
 *   nullまたは空文字は指定してはならない。
 * @param {Integer} aPositionX icrop.clientapplication.resizeWindowを参照
 * @param {Integer} aPositionY icrop.clientapplication.resizeWindowを参照
 * @param {Integer} aWidth icrop.clientapplication.resizeWindowを参照
 * @param {Integer} aHeight icrop.clientapplication.resizeWindowを参照
 * @return {String} XML形式(Push通知I/F形式)の文字列
 */
icrop.clientapplication._createResizePushXml = function(
	aResizeTarget,
	aPositionX,
	aPositionY,
	aWidth,
	aHeight) {
	if ((typeof aResizeTarget) != "string") {
		throw new Error("aResizeTarget must be string");
	}
	
	if (aResizeTarget.length == 0) {
		throw new Error("aResizeTarget cannot be empty");
	}
	
	// Detail要素を組み立て
	var buildXmlElement = icrop.clientapplication._buildXmlElement;
	var detailElement
	= '<MC3A01014>'
	+   buildXmlElement("ResizeTarget", aResizeTarget)
	+   buildXmlElement("PositionX", aPositionX)
	+   buildXmlElement("PositionY", aPositionY)
	+   buildXmlElement("Width", aWidth)
	+   buildXmlElement("Height", aHeight)
	+ '</MC3A01014>'

	// Push通知I/F形式にして返す
	return icrop.clientapplication._createPushXml(
		"MC3A01014", detailElement);
};


/**
 * XML形式(Push通知I/F形式)の文字列を生成.
 *
 * @param {String}
 * aDestinationApplicaitonID 送信先アプリケーションのIDを指定
 * @param {String} aDetailContentString
 * Detail要素のインナーテキストを指定する。この引数はエスケープなどの
 * 処理はされずにそのままDetail要素のコンテンツとして利用される。
 * @return {String} XML形式(Push通知I/F形式)の文字列
 */
icrop.clientapplication._createPushXml = function(
	aDestinationApplicaitonID,
	aDetailContentString) {
	var buildXmlElement = icrop.clientapplication._buildXmlElement;
	return '<?xml version="1.0" encoding="utf-8" ?>'
	+ '<CommonCommunication>'
	+   '<Head>'
	+     '<Origin>'
	+       '<ApplicationID>iCROP</ApplicationID>'
	+     '</Origin>'
	+     '<Destination>'
	+       buildXmlElement("ApplicationID", aDestinationApplicaitonID)
	+     '</Destination>'
	+   '</Head>'
	+   '<Detail>' + aDetailContentString + '</Detail>'
	+ '</CommonCommunication>';
};


/**
 * XML要素形式の文字列を生成.
 *
 * @param {String} aElementName 要素名
 * @param {Object} aContent 要素の値。
 * @return {String}
 * XML要素形式の文字列を返す。aContentが空の場合は空文字を返す。
 */
icrop.clientapplication._buildXmlElement = function(aElementName, aContent) {
	aContent = "" + (aContent ? aContent : "");
	if (aContent.length != 0) {
		return '<' + aElementName + '>'
			+ icrop.clientapplication._xmlEscape(aContent)
			+ '</' + aElementName + '>';
	}
	else {
		return '';
	}
};


/**
 * XMLエスケープ.
 *
 * @param {String} aTargetString XMLエスケープする文字列
 * @return {String} XMLエスケープ後の文字列
 */
icrop.clientapplication._xmlEscape = function(aTargetString) {
	return aTargetString
		.replace(/&/g, "&amp;")
		.replace(/</g, "&lt;")
		.replace(/>/g, "&gt;")
		.replace(/"/g, "&quot;")
		.replace(/'/g, "&apos;");
};


/**
 * 連想配列をHTTPのクエリ形式(a=b&c=d&...)に組み立てる.
 *
 * @param {Object} aHash 連想配列
 * @return {String} クエリ文字列
 */
icrop.clientapplication._buildQuery = function(aHash) {
    var ret = '';
    for (var key in aHash) {
        ret += encodeURIComponent(key)
            + "="
            + encodeURIComponent(aHash[key])
            + "&";
    }

    // 末尾の&を1文字を消去して返す
    return ret.replace(/&$/, "");
};


/**
 * リンクのURLを絶対URLに変換する。
 * リンクが絶対URLの場合はそのまま返す.
 *
 * @param {String} aUrl URLを指定
 * @param {String} aLocationHref 現在のページのURL(location.href)を指定
 * @return {String} 絶対URL
 */
icrop.clientapplication._convertToAbsoluteUrl = function(aUrl, aLocationHref) {
	if (aUrl.charAt(0) === "/") {
		// 先頭がスラッシュの場合 scheme://***** の部分を付加して返す
		var matches = aLocationHref.match(/^[^:]+:\/\/[^\/]+/);
		if (matches !== null) {
			return matches[0] + aUrl;
		}
		else {
			throw new Error(aLocationHref + " does not match /^[^:]+:\\/\\/[^\\/]+/");
		}
	}
	else if (aUrl.match(/^[^:]+:\/\/[^\/]+/) === null) {
		// スキームの無いURLの場合は相対パス
		if ( ! aLocationHref.match(/^[^:]+:\/\/[^\/]+\//)) {
			// location が scheme://domain でdomainの後ろに / が無い場合は / を付加
			aLocationHref += "/";
		}
		
		var matches1 = aLocationHref.match(/^[^?#]+/);
		if (matches1 !== null) {
			var url = matches1[0]; // ひとまず ? や # 以前の部分を切り出す
			var matches2 = url.match(/^.+\//);
			if (matches2 !== null) {
				return matches2[0] + aUrl;
			}
			else {
				throw new Error(url + " does not match /^.+\//");
			}
		}
		else {
			throw new Error(aLocationHref + " does not match /^[^?#]+/");
		}
	}
	else if ( ! aUrl || aUrl.length == 0) {
		// 引数がnullだったり空文字の場合はエラー
		throw new Error("aUrl cannot be null or undefined or empty");
	}
	else {
		// この場合は普通のURL(例:http://localhost/foobar.htm)なのでそのまま返す
		return aUrl;
	}
};


/**
 * 現在のブラウザがpageTypeをサポートしているかどうかの判定.
 *
 * @param {String} userAgent
 * @param {String} pageType
 */
icrop.clientapplication.checkBrowserSupport = function(
	aUserAgent, aPageType) {
	// 配列に文字列が含まれているか(大文字小文字無視)を調べる関数
	var contains = function(aArray, aString) {
		for (var i = 0, l = aArray.length; i < l; i++) {
			if (aArray[i].toLowerCase() === aString.toLowerCase()) {
				return true;
			}
		}
		return false;
	};
	
	var info = icrop.clientapplication.C_BROWSER_INFO_ARRAY; // short hand
	for (var i = 0, l = info.length; i < l; i++) {
		if (info[i].regex.test(aUserAgent) &&
		    contains(info[i].supportedContentTypes, aPageType)) {
			return true;
		}
	}
	return false;
};

// for backword
icrop.clientapplication._checkBrowserSupport
  = icrop.clientapplication.checkBrowserSupport;


/**
 * ブラウザ遷移機能のURLスキーム文字列配列を生成.
 *
 * @param {String} aPageType icrop.clientapplication.openNewWindow を参照
 * @param {String} aTargetUrl icrop.clientapplication.openNewWindow を参照
 * @param {Integer} aViewType icrop.clientapplication.openNewWindow を参照
 * @param {String} aViewOption icrop.clientapplication.openNewWindowを参照
 * @return {String} icrop-browser-callのパラメータ付きURLスキームの配列
 */
icrop.clientapplication._createBrowserCallUrlSchemes = function(
	aPageType, aTargetUrl, aViewType, aViewOption) {
	// URLスキームに含めるクッキーを取得
	var cookies = {};
	document.cookie.replace(/([^ ;=]+)=([^ ;]+);?/g, function(m, m1, m2) {
		cookies[m1] = m2;
	});
	
	// クッキーをJSON形式に変換, パラメータ形式に組み立て
	var cookie = "";
	var targetCookieNames = icrop.clientapplication.C_TARGET_COOKIE_NAMES;
	for (var i = 0; i < targetCookieNames.length; i++) {
		var targetName = targetCookieNames[i];
		if (cookies[targetName]) {
			// ダブルクオートで囲む
			cookie += '"' + targetName.replace(/"/g, '\\"') + '":'
				+ '"' + cookies[targetName].replace(/"/g, '\\"') + '",';
		}
	}
	
	if (cookie.length) {
		cookie = "{" + cookie.substr(0, cookie.length - 1) + "}";
		cookie = "&cookie=" + encodeURIComponent(cookie);
	}
	
	// URLスキームを組み立てて返す
	if ((typeof aViewOption) == "undefined" || aViewOption === "") {
		aViewOption == "";
	}
	else {
		aViewOption = "&viewoption=" + encodeURIComponent(aViewOption);
	}

	// GUIDの生成
	var guid = (function() {
		var d = new Date().getTime();
		var guid = 'xxxxxxxx-xxxx-4xxx-yxxx-xxxxxxxxxxxx'.replace(/[xy]/g, function(c) {
			var r = (d + Math.random() * 16) % 16 | 0;
			d = Math.floor(d / 16);
			return (c=='x' ? r : (r&0x7 | 0x8)).toString(16);
		});
		return guid;
	})();

	var params = "pagetype=" + encodeURIComponent(aPageType)
		+ "&targeturl=" + encodeURIComponent(aTargetUrl)
		+ "&viewtype=" + (typeof aViewType != "undefined" ?aViewType : "0")
		+ aViewOption
		+ cookie;

	// パラメータを440文字以下に分割する
	var list = []; // 分割パラメータの格納配列
	var sequence = ""; // パラメータのバッファ
	var encoded; // 文字をURLエンコードした文字列のバッファ

	for(var pos = 0, paramsLength = params.length; pos < paramsLength; pos++) {
		encoded = encodeURIComponent(params.substr(pos, 1)); // IE対策のURLエンコード
		if (sequence.length + encoded.length <= 440) {
			sequence += encoded;
		}
		else {
			list.push(sequence);
			sequence = encoded;
		}
		
		if (pos == paramsLength - 1) { // paramsの最後の文字列処理の場合
			list.push(sequence); 
		}
	}

	// URLスキームを結合して返す
	for (var i = 0, l = list.length; i < l; i++) {
		list[i] = "icrop-browser-call:" + guid + "/" + i + "/" + l + "?" + list[i];
	}
	return list;
};


/**
 * ブラウザのウィンドウを閉じる.
 */
icrop.clientapplication._closeWindow = function() {
	setTimeout (function() { window.open("", "_self").close(); }, 1);
	setInterval(function() { window.open("", "_self").close(); }, 20);
};


/**
 * ブラウザのウィンドウを新しくフルスクリーンで開く。
 * 開くのに失敗した場合はそのまま開く。
 * ※信頼済サイトに登録する（していないと、アドレスバーが表示される）
 * ※ポップアップを解除する（していないと、新しいウインドウを出さない）.
 *
 * @param {String} aUrl 開くURL
 */
icrop.clientapplication._openFullScreenWindow = function(aUrl) {
	var option = "WIDTH=" + screen.availWidth
		+ ",HEIGHT=" + screen.availHeight
		+ ",scrollbars=0,resizable=1,"
		+ "toolbar=0,menubar=0,location=0,status=1";
	
	var objw = window.open("about:blank", "_blank", option);
	if (objw) {
		var taskbarAndTitlebar = screen.height - screen.availHeight;
		var titlebar = objw.screenTop;
		var taskbar = taskbarAndTitlebar - titlebar;
		var windowMargin = objw.screenLeft;
		objw.moveTo(-windowMargin / 2, -objw.screenTop);
		objw.resizeTo(screen.availWidth + windowMargin * 2,
			screen.height + titlebar + windowMargin - taskbar * 0.2);

		objw.moveTo(0, 0);
		// objw.moveTo(-3, -26);

		objw.location = aUrl;
	}
	else {
		// popupブロック(ipad)
		window.location = aUrl;
	}
};


/**
 * エラーオブジェクトからエラー詳細を取得.
 * @param {Error} aError エラーオブジェクト
 * @return {String} エラー詳細
 */
icrop.clientapplication._getDetailError = function(aError) {
	return /Firefox/i.test(navigator.userAgent)
		? (aError.message + "\n" + aError.stack.replace(/(.+)/g, "\tat $1"))
		: (aError.stack || aError.message)
};


/**
 * MC3A01016(OSK表示機能)へのOSK表示/非表示を要求する。
 * @param {boolean} bool
 *   VISIBLE要素に指定するOSKの表示/非表示を識別するためのブール値。
 *   nullまたは空文字は指定してはならない。
 * @return {Boolean} 引数が不正な場合はfalse、正しい場合はtrueを返す。
 *   結果がfalseの場合はPC端末アプリへのOSK表示/非表示要求は実行されない。
 */
icrop.clientapplication.showScreenKeyboard = function(bool){
	var result = false;

	try {
		// パラメータ数のチェック
		if (arguments.length != 1) {
			throw new Error("arguments length must be 1. arguments.length = "
				+ arguments.length);
		}
		var boolString = "";
		
		// パラメータチェック
		if ((typeof bool) != "boolean") {
			throw new Error("bool must be boolean");
		}
		else {
			boolString = new Boolean(bool).toString();
		}
	
		var xml = icrop.clientapplication._createOskPushXml(boolString);
		icrop.clientapplication.SendXml(xml);
		result = true; // 処理途中で例外が発生しなければtrueを返す
	}
	catch (error) {
		icrop.clientapplication._log("icrop.clientapplication.showScreenKeyboard "
			+ "bool = " + bool
			+ ", Error = " + icrop.clientapplication._getDetailError(error));
	}
	
	return result;
};


/**
 * MC3A01016(OSK表示機能)へ送信するXML文字列
 * を作成する。受け取った引数が不正な場合は例外を送出する。
 * icrop.clientapplication.showScreenKeyboardの補助関数.
 *
 * @param {string} bool
 *   VISIBLE要素に指定するOSKの表示/非表示を識別するための文字列。
 *   nullまたは空文字は指定してはならない。
 */
icrop.clientapplication._createOskPushXml = function(
	bool) {
	bool = bool === "" ? null : bool;

	// パラメータチェック
	if ((typeof bool) != "string") {
		throw new Error("bool must be string");
	}

	// 組み立てて返す
	var detailElement
		= '<MC3A01016>'
		+   icrop.clientapplication._buildXmlElement("VISIBLE", bool)
		+ '</MC3A01016>';
	return icrop.clientapplication._createPushXml("MC3A01016", detailElement);
};

