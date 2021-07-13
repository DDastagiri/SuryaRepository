//---------------------------------------------------------
//SC3240701.Difine.js
//---------------------------------------------------------
//機能：ストール使用不可画面のグローバル変数と定義
//作成：2017/08/30 NSK 竹中(悠) REQ-SVT-TMT-20161109-001 SMB iPadにストールクローズ機能の追加
//更新：
//---------------------------------------------------------

/****************************************
* 定数宣言
****************************************/
//テキストエリアの高さデフォルト値
var C_UNAVAILABLE_TA_DEFAULTHEIGHT = 90;

//ポップアップをチップ左に表示時の吹出し三角の相対left値
var C_SC3240701POP_DISPLEFT_ARROW_X = 362;

//ポップアップをチップ右に表示時の吹出し三角の相対left値
var C_SC3240701POP_DISPRIGHT_ARROW_X = -17;

//ポップアップを画面右端に表示時のポップアップの相対left値
var C_SC3240701POP_DISPRIGHT_DEFAULT_X = 635;

//タッチイベント名
var C_SC3240701_TOUCH = "mousedown touchstart";

// 使用不可エリアの移動チップ
var C_MOVINGUNAVALIABLECHIPID = "MovingUnavaliableChip";

//コールバック時にサーバー側で処理分岐用メソッド名
//画面の作成
var C_SC3240701CALLBACK_CREATEDISP = "UnavailableChip";

//登録ボタンクリック
var C_SC3240701CALLBACK_REGISTER = "RegisterUnavailableSetting";

//作業時間最大値（分）
var C_SC3240701_MAXWORKTIME = 9995;

//日付の最小値
var C_SC3240701_DATE_MIN_VALUE = Date.parse("1900/01/01 0:00:00");


/****************************************
* グローバル変数宣言
****************************************/
//チップ表示時のleft値保存用
var gUnavailablePopX;

//チップ表示用開始時間
var gSC3240701DisplayChipStartTime;

//チップ表示用終了時間
var gSC3240701DisplayChipEndTime;


/**
* コールバック関数定義
* 
* @param {String} argument サーバーに渡すパラメータ(JSON形式)
* @param {String} callbackFunction コールバック後に実行するメソッド
* 
*/
var gCallbackSC3240701 = {
    doCallback: function (argument, callbackFunction) {
        this.packedArgument = JSON.stringify(argument);
        this.endCallback = callbackFunction;
        this.beginCallback();
    }
};

/**
* オーバーレイ表示操作関数定義
* 
*/
var gUnavailableOverlay = {
    show: function () {
        $("#MstPG_registOverlayBlack").css({
            "display": "block"
            , "z-index": "10002"
            , "opacity": "0.1"
            , "height": "768px"
        });
    }
    ,
    hide: function () {
        $("#MstPG_registOverlayBlack").css({
            "display": "none"
            , "z-index": "4"
            , "opacity": "0"
            , "height": "703px"
        });
    }
};

/**
* アクティブインジケータ表示操作関数定義
* 
*/
var gUnavailableActiveIndicator = {
    show: function () {
        $("#UnavailableActiveIndicator").addClass("show");
    }
    ,
    hide: function () {
        $("#UnavailableActiveIndicator").removeClass("show");
    }
};

/**	
* 画面を再表示する(commonRefreshTimerにセットする関数)
* 	
*/
function ReDisplayUnavailableChip() {
    //ストール使用不可画面を閉じる
    CloseUnavailableSetting();

    //操作リストをクリアする
    ClearOperationList();

    //20秒経っても表示されない場合
    setTimeout(function () {

        //フッターイベント(3300)を呼び出し
        FooterEvent(C_FT_BTNID_UNAVAILABLESETTING);
    }, 200);
}