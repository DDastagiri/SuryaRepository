/**
* SC3320101.Event.js
* ---------------------------------------------------------
* 作成：2014/08/14 TMEJ 丁 メインメニュー(移動マン)
* ---------------------------------------------------------
*/

/****************************************
* 定数宣言
****************************************/
//Cellの高さデフォルト値
var C_SC3320101TA_DEFAULTHEIGHT = 82;

//スクロールの高さデフォルト値
var C_SC3320101SCR_DEFAULTHEIGHT = 691;
//スクロールの幅デフォルト値
var C_SC3320101SCR_DEFAULTWIDTH = 578;
//スクロールのTOPデフォルト値
var C_SC3320101SCR_DEFAULTTOP = -3;

// データフォーマット：YYYY/MM/dd HH:mm
var gDateFormat = "YYYY/MM/dd HH:mm";

//ロケーションテキストのID
var C_ID_NUM = "Num";
var C_ID_ALP = "Alp";

/**
* コールバック関数定義
* 
* @param {String} argument サーバーに渡すパラメータ(JSON形式)
* @param {String} callbackFunction コールバック後に実行するメソッド
* 
*/
var gCallbackSC3320101 = {
    doCallback: function (argument, callbackFunction) {
        this.packedArgument = JSON.stringify(argument);
        this.endCallback = callbackFunction;
        this.beginCallback();
    }
};

/****************************************
* グローバル変数宣言
****************************************/
//来店情報配列の初期化
var gArrObjVisitInfo = new Array();
//更新用来店情報配列の初期化
var gArrObjUpdVisitInfo = new Array();

// 定期リフレッシュ関数
var gFuncRefreshTimerInterval = "";

// 画面自動リフレッシュ時間単位(秒)
var gRefreshTimerInterval = 60;

//選択されたテキストのKey
var gSelectedVisitSeq = "";

//ページ取得時のサーバとクライアントの時間差
var gServerTimeDifference = 0;

// 編集モードフラグ
var gEditeFlg = false;

// 絞り込みモードフラグ
var gSearchFlg = false;