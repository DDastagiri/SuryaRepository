var icropBase = function() {

}

// WebSocket を使用してPC基盤へセッション確立
// 51002 はポート番号です。
icropBase.prototype = new WebSocket("ws://127.0.0.1:51002");
icropBase.prototype.constructor = icropBase;

// WebSocket のデータ受信処理
icropBase.prototype.onmessage = function (e) {
	try {
		var Recvcmd = e.data.split(":")
		var actionflg = false
		if (Recvcmd[0] == 'icrop') {
			if (Recvcmd[1] == "action") {
				if (Recvcmd.length == 3 ) {
					eval(Recvcmd[2]);
					actionflg = true;
				}
			}
		}
		if (actionflg == false) {
			icropBase.prototype.send('icropBase.Execute("icrop:log:JavaScriptCommandError[' + e.data + ']");');
		}
	} catch (e){
		icropBase.prototype.send('icropBase.Execute("icrop:log:' + Recvcmd[2] + '['+ e.data + ']" );');
	} finally{
	}

};
//WebSocket のセッション確立後処理
icropBase.prototype.onopen = function (e) {
	try {
//                var resultAreaObj = document.getElementById('result');
//                resultAreaObj.innerHTML += '<span class="log">onopen</span>' + '<br>'
		
	} catch(e){
		
	}
};

//WebSocket のセッション終了処理
icropBase.prototype.onclose = function (e) {
	try {
//                var resultAreaObj = document.getElementById('result');
//                resultAreaObj.innerHTML += '<span class="log">onclose</span>' + '<br>'
		
	} catch(e){
		
	}
};


// WebSocket のエラー処理
icropBase.prototype.onerror = function (e) {
	try {
//                var resultAreaObj = document.getElementById('result');
//                resultAreaObj.innerHTML += '<span class="log">onerror</span>' + '<br>'

	} catch(e){
		
	}

};

var ws_connect_timer
var ws_cmdstr

// ICrop の実行方法
icropBase.Execute = function(cmdstr){
	try {
		var SendData
		if (icropBase.prototype.readyState == 1 ) {
			SendData = icropBase.prototype.send(cmdstr);
			return SendData
		} else if (icropBase.prototype.readyState == 0 ) {
			ws_cmdstr = cmdstr
			ws_connect_timer = window.setInterval(function(){
			if(icropBase.prototype == null || icropBase.prototype.readyState == 1){
				SendData = icropBase.prototype.send(ws_cmdstr);
				clearInterval(ws_connect_timer)
			}
			}
			, 500);
		}

	} catch (e) {
		icropBase.prototype.send('icropBase.Execute("icrop:log:ExecuteError' + e.data + ' ");');
	}
};

// 音声ファイル再生関数
// 引数: 2:警告音1
// 　　  3:警告音2
icropBase.beep = function(kubun){
	if (kubun == 2) {
		icropBase.Execute('icrop:soundon:1');
	} else if (kubun == 3) {
		icropBase.Execute('icrop:soundon:2');
	}
};

/**
 * ユーザ初期化処理
 * UserIDを取得してログイン命令を送信
 */
icropBase._initializeUser = function () {
    if ((typeof icropBase.getUser) !== "function")
    {
        return;
    }

    var user = icropBase.getUser();
    if (user && (user != ""))
    {
        icropBase.Execute("icrop:lgin:" + user);
    }
};

/**
 * Initializeの実行イベント追加
 */
if ((typeof jQuery) != "undefined")
{
    jQuery(function ($) {
        icropBase._initializeUser();
    });
}
else
{
    document.addEventListener("DOMContentLoaded", icropBase._initializeUser, false);
};

/**
 * PC端末アプリのスクリーンセーバ操作機能有効化スイッチを切替えるリクエストを送信する。
 *
 * @param {boolean} enabled 有効化スイッチの設定値
 *  true:スイッチON false:スイッチOFF.
 */
icropBase.SwitchMC3A01013FunctionEnabled = function(enabled) {
    if ((typeof enabled) === 'boolean') {
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
        icropBase.Execute("icrop:xml:" + xml);
    }
};

