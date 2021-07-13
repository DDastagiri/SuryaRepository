//---------------------------------------------------------
//SC3240301.SubChip.js
//---------------------------------------------------------
//機能：サブチップ
//作成：2012/04/24 TMEJ 丁 タブレット版SMB機能開発(工程管理)
//更新：2013/11/29 TMEJ 丁 タブレット版SMB チーフテクニシャン機能開発
//更新：2013/11/29 TMEJ 張 タブレット版SMB チーフテクニシャン機能開発
//更新：2014/04/22 TMEJ 丁 【IT9669】サービスタブレットDMS連携作業追加機能開発
//更新：2014/06/18 TMEJ 丁 タブレット版SMB テレマ走行距離機能開発
//更新：2014/07/15 TMEJ 張 タブレットSMB Job Dispatch機能開発
//更新：2015/02/09 TMEJ 範 DMS連携版サービスタブレット SMB納車予定時刻通知機能開発
//更新：2015/02/13 TMEJ 範 新プラットフォーム版i-CROP仕様変更対応 (SMBチップ検索及び作業中断機能強化)
//更新：2015/04/23 TMEJ 明瀬  DMS連携版サービスタブレット強制納車機能追加開発
//更新：2015/07/30 TMEJ 明瀬 アクティビティインジケータが消えない問題
//更新：2015/09/26 NSK  秋田谷 開発TR-SVT-TMT-20151008-001 SMB画面の表示が遅い
//更新：2017/07/12 NSK  河谷 REQ-SVT-TMT-20160906-001 チップ検索の検索範囲をPC-SMBと同じにする
//更新：2017/09/05 NSK  小川 REQ-SVT-TMT-20160906-003 計画済みチップをストールから簡単に外せるようにする
//更新：2017/09/25 NSK 竹中(悠) REQ-SVT-TMT-20170227-002 見込納車遅れ日時計算ロジック統一
//更新：2017/11/12 NSK  荒川 REQ-SVT-TMT-20160906-003 計画済みチップをストールから簡単に外せるようにする
//更新：2019/06/18 NSK 皆川 (トライ店システム評価)次世代サービスオペレーション効率化に向けた、業務適合性検証
//更新：2019/07/29 NSK 近藤 (トライ店システム評価)次世代サービスオペレーション効率化に向けた、業務適合性検証
//更新：2019/08/09 NSK 鈴木 DLR-002 子チップを着工指示した時にエラー発生（親チップ着工指示でエラー発生を修正）
//更新：2019/12/06 NSK 皆川 (トライ店システム評価)整備作業における休憩時間取得判定向上検証
//更新：
//---------------------------------------------------------
/****************************************
* 定数宣言
****************************************/
//フッターボタンID
//受付
var C_RECEPTION = "100";
//追加作業
var C_ADDITIONALWORK = "200";
//追加作業承認遷移
var C_ADDWORKCONFIRM_REDIRECT = "202";
//完成検査
var C_COMPLETIONINSPECTION = "300";
//洗車
var C_CARWASH = "400";
//完成検査遷移
var C_COMPLETIONINSPECTION_REDIRECT = "402";
//納車待ち
var C_DELIVERDCAR = "500";
//NoShow
var C_NOSHOW = "600";
//NoShowチップ移動
var C_NOSHOW_MOVING = "602";
//中断
var C_STOP = "700";
//中断チップ移動
var C_STOP_MOVING = "702";
//受付ボタンの状態更新するため、件数を取得する
var C_UPDATECNT_RECEPTION = "101";
//追加作業ボタンの状態更新するため、件数を取得する
var C_UPDATECNT_ADDITIONALWORK = "201";
//完成検査ボタンの状態更新するため、件数を取得する
var C_UPDATECNT_COMPLETIONINSPECTION = "301";
//洗車ボタンの状態更新するため、件数を取得する
var C_UPDATECNT_CARWASH = "401";
//納車待ちボタンの状態更新するため、件数を取得する
var C_UPDATECNT_DELIVERDCAR = "501";
//NoShowボタンの状態更新するため、件数を取得する
var C_UPDATECNT_NOSHOW = "601";
//中断ボタンの状態更新するため、件数を取得する
var C_UPDATECNT_STOP = "701";
//全体サブエリア件数を取得する
var C_UPDATE_ALLCONUT = "1000";
//CallBack処理種類
//サブチップ表示
var C_OPERATIONTYPE_GETSUBBOXCHIP = 0;
//その他
var C_OPERATIONTYPE_OTHER = 1;

//サブチップ幅
var C_SubChipWidth = 84;

//受付(予約紐付け)
var C_RECEPTION_ATTACHMENT = "102";
//チップ更新パタンー
var C_UPDATA_STALLCHIP = 0;
var C_UPDATA_SUBCHIP = 1;
//2017/07/12 NSK  河谷 REQ-SVT-TMT-20160906-001 チップ検索の検索範囲をPC-SMBと同じにする START
//仮置きフラグ(0:仮置きでない)
var C_TEMP_FLG_OFF = "0";
//2017/07/12 NSK  河谷 REQ-SVT-TMT-20160906-001 チップ検索の検索範囲をPC-SMBと同じにする END

// 2019/08/09 NSK 鈴木 DLR-002 子チップを着工指示した時にエラー発生（親チップ着工指示でエラー発生を修正） START
// リレーションチップ配列でチップIDが格納されているインデックス
var INDEX_RELATION_CHIPS_CHIP_ID = 0;
// リレーションチップ配列でマッチングキーが格納されているインデックス
var INDEX_RELATION_CHIPS_MATCHING_KEY = 2;
// 2019/08/09 NSK 鈴木 DLR-002 子チップを着工指示した時にエラー発生（親チップ着工指示でエラー発生を修正） END

/****************************************
* グローバル変数宣言
****************************************/
// サブチップ幅
var gSubAreawidth = 1024;
//サブチップのmovingチップ初期left値
var gSubMovingChipLeft = "-10000px";
//サブチップのmovingチップ初期Top値
var gSubMovingChipTop = "-10000px";
//サブチップクラス配列の初期化
var gArrObjSubChip = new Array();
//遅れるフラグ
var gstrLateflg = 0;

//編集フラグ 0：編集可　固定
var C_EDIT_FLG = "0";

// 2017/09/05 NSK 小川 REQ-SVT-TMT-20160906-003 計画済みチップをストールから簡単に外せるようにする START
//仮置きフラグ 1：仮置き
var C_TEMP_FLG = "1";
// 2017/09/05 NSK 小川 REQ-SVT-TMT-20160906-003 計画済みチップをストールから簡単に外せるようにする END

/**
* コールバック関数定義
* 
* @param {String} argument サーバーに渡すパラメータ(JSON形式)
* @param {String} callbackFunction コールバック後に実行するメソッド
* 
*/
var gCallbackSC3240301 = {
    doCallback: function (argument, callbackFunction) {
        this.packedArgument = JSON.stringify(argument);
        this.endCallback = callbackFunction;
        this.beginCallback();
    }
};
/**
* 画面初期表示サブチップボタンの状態設定する
* @return {なし}
*/
function InitializationSubChip(jsonData) {
    // JSON形式のチップ情報読み込み
    var subchipjsonData = $("#hidJsonDatasubchip").val();
    var subchipinfoList;
    var subchipinfo;
    if (!subchipjsonData) {
        subchipinfoList = $.parseJSON(jsonData);
    } else {
        subchipinfoList = $.parseJSON(subchipjsonData);
    }
    $("#hidJsonDatasubchip").attr("value", "");
    // 取得したチップ情報をサブチップボタンに反映する
    for (var keyString in subchipinfoList) {
        subchipinfo = subchipinfoList[keyString];
        if (gOpenningSubBoxId == subchipinfo.AREAID) { 
            continue;
        }
        SetSubChipAreaStatus(subchipinfo.AREAID, subchipinfo.COUNT, subchipinfo.LATEFLG);
        if (subchipinfo.AREAID == C_RECEPTION) {
            var dtUpdateTime = new Date($("#hiddenupdatetime").val());
            var dtDBUpdateTime = new Date(subchipinfoList[keyString].CUST_CONFIRMDATE);
            if (dtUpdateTime < dtDBUpdateTime) {
                //受付フッターボタンを点滅させる
                BlinkReceprionButtonOn();
            }
        }
    }

}
/**
* 受付サブチップ表示
* @return {なし}
*/
function ShowReceptionchip() {
    if ($(".SubChipReception").css('display') === 'none') {
        var strButtonPosition = $("#FooterButtonIcon100").offset().left + 12
        $(".SubChipReception .Triangle").css("left", "" + strButtonPosition + "px");
        $(".SubChipAdditionalWork").fadeOut(300);
        $(".SubChipCompletionInspection").fadeOut(300);
        $(".SubChipCarWash").fadeOut(300);
        $(".SubChipWaitingDelivered").fadeOut(300);
        $(".SubChipNoShow").fadeOut(300);
        $(".SubChipStop").fadeOut(300);
        $(".SubChipReception").fadeIn(300);
        if (gOpenningSubBoxId != "") {
            gSearchedChipId = "";
        }
        gOpenningSubBoxId = C_RECEPTION;
        $("#SubChipAreaActiveIndicator").addClass("show");
        $("#SubChip_LoadingScreen").css({ "display": "block" });
        //リフレッシュタイマーセット
        commonRefreshTimerTabletSMB(ReceptionAreaReLoad);
        //$(".Reception").css("display", "block");
        $(".SubChipBox").SC3240301fingerScroll();
        $(".SCp").remove();
        //受付フッターボタンを点滅をやめる
        var now = GetServerTimeNow();
        $("#hiddenupdatetime").val(now);
        BlinkReceprionButtonOff();
        //サーバーに渡すパラメータを作成
        var prms = CreateCallBackSubChipParam(C_RECEPTION);
        //コールバック開始
        DoCallBack(C_CALLBACK_WND301, prms, ReceptionAfterCallBack, "ShowReceptionchip");
        //gCallbackSC3240301.doCallback(prms, ReceptionAfterCallBack);
    }
    else {
        $(".SubChipReception").fadeOut(300);
        gOpenningSubBoxId = "";
        gSearchedChipId = "";
        $(".SCp").remove();
        $("#SubChipAreaActiveIndicator").removeClass("show");
        $("#SubChip_LoadingScreen").css({ "display": "none" });
        //タイマーをクリア
        commonClearTimer();
        //メインエリアのスクロール範囲を元に戻す
        ChangeMainScrollHeight(false);
    }
}
/**
* 追加作業サブチップ表示
* @return {なし}
*/
function ShowAddWorkchip() {
    if ($(".SubChipAdditionalWork").css('display') === 'none') {
        var strButtonPosition = $("#FooterButtonIcon200").offset().left + 12
        $(".SubChipAdditionalWork .Triangle").css("left", "" + strButtonPosition + "px");
        $(".SubChipReception").fadeOut(300);
        $(".SubChipCompletionInspection").fadeOut(300);
        $(".SubChipCarWash").fadeOut(300);
        $(".SubChipWaitingDelivered").fadeOut(300);
        $(".SubChipNoShow").fadeOut(300);
        $(".SubChipStop").fadeOut(300);
        $(".SubChipAdditionalWork").fadeIn(300);
        if (gOpenningSubBoxId != "") {
            gSearchedChipId = "";
        }
        gOpenningSubBoxId = C_ADDITIONALWORK;
        $("#SubChipAreaActiveIndicator").addClass("show");
        $("#SubChip_LoadingScreen").css({ "display": "block" });
        //リフレッシュタイマーセット
        commonRefreshTimerTabletSMB(AddWorkAreaReLoad);
        $(".SubChipBox").SC3240301fingerScroll();
        $(".SCp").remove();
        //サーバーに渡すパラメータを作成
        var prms = CreateCallBackSubChipParam(C_ADDITIONALWORK);
        //コールバック開始
        DoCallBack(C_CALLBACK_WND301, prms, AddWorkAfterCallBack, "ShowAddWorkchip");
        //gCallbackSC3240301.doCallback(prms, AddWorkAfterCallBack);
    }
    else {
        $(".SubChipAdditionalWork").fadeOut(300);
        gOpenningSubBoxId = "";
        gSearchedChipId = "";
        $(".SCp").remove();
        $("#SubChipAreaActiveIndicator").removeClass("show");
        $("#SubChip_LoadingScreen").css({ "display": "none" });
        //タイマーをクリア
        commonClearTimer();
        //メインエリアのスクロール範囲を元に戻す
        ChangeMainScrollHeight(false);
    }
}
/**
* 完成検査サブチップ表示
* @return {なし}
*/
function ShowCompletionchip() {
    if ($(".SubChipCompletionInspection").css('display') === 'none') {
        var strButtonPosition = $("#FooterButtonIcon300").offset().left + 12
        $(".SubChipCompletionInspection .Triangle").css("left", "" + strButtonPosition + "px");
        $(".SubChipReception").fadeOut(300);
        $(".SubChipAdditionalWork").fadeOut(300);
        $(".SubChipCarWash").fadeOut(300);
        $(".SubChipWaitingDelivered").fadeOut(300);
        $(".SubChipNoShow").fadeOut(300);
        $(".SubChipStop").fadeOut(300);
        $(".SubChipCompletionInspection").fadeIn(300);
        if (gOpenningSubBoxId != "") {
            gSearchedChipId = "";
        }
        gOpenningSubBoxId = C_COMPLETIONINSPECTION;
        $("#SubChipAreaActiveIndicator").addClass("show");
        $("#SubChip_LoadingScreen").css({ "display": "block" });
        //リフレッシュタイマーセット
        commonRefreshTimerTabletSMB(ComInspectionAreaReLoad);
        $(".SubChipBox").SC3240301fingerScroll();
        $(".SCp").remove();
        //サーバーに渡すパラメータを作成
        var prms = CreateCallBackSubChipParam(C_COMPLETIONINSPECTION);
        //コールバック開始
        DoCallBack(C_CALLBACK_WND301, prms, InspectionComAfterCallBack, "ShowCompletionchip");
        //gCallbackSC3240301.doCallback(prms, InspectionComAfterCallBack);
    }
    else {
        $(".SubChipCompletionInspection").fadeOut(300);
        gOpenningSubBoxId = "";
        gSearchedChipId = "";
        $(".SCp").remove();
        $("#SubChipAreaActiveIndicator").removeClass("show");
        $("#SubChip_LoadingScreen").css({ "display": "none" });
        //タイマーをクリア
        commonClearTimer();
        //メインエリアのスクロール範囲を元に戻す
        ChangeMainScrollHeight(false);
    }
}
/**
* 洗車サブチップ表示
* @return {なし}
*/
function ShowCarWashchip() {
    if ($(".SubChipCarWash").css('display') === 'none') {
        var strButtonPosition = $("#FooterButtonIcon400").offset().left + 12
        $(".SubChipCarWash .Triangle").css("left", "" + strButtonPosition + "px");
        $(".SubChipReception").fadeOut(300);
        $(".SubChipAdditionalWork").fadeOut(300);
        $(".SubChipCompletionInspection").fadeOut(300);
        $(".SubChipWaitingDelivered").fadeOut(300);
        $(".SubChipNoShow").fadeOut(300);
        $(".SubChipStop").fadeOut(300);
        $(".SubChipCarWash").fadeIn(300);
        if (gOpenningSubBoxId != "") {
            gSearchedChipId = "";
        }
        gOpenningSubBoxId = C_CARWASH;
        $("#SubChipAreaActiveIndicator").addClass("show");
        $("#SubChip_LoadingScreen").css({ "display": "block" });
        //リフレッシュタイマーセット
        commonRefreshTimerTabletSMB(CarWashAreaReLoad);
        $(".SubChipBox").SC3240301fingerScroll();
        $(".SCp").remove();
        //サーバーに渡すパラメータを作成
        var prms = CreateCallBackSubChipParam(C_CARWASH);
        //コールバック開始
        DoCallBack(C_CALLBACK_WND301, prms, CarWashAfterCallBack, "ShowCarWashchip");
        //gCallbackSC3240301.doCallback(prms, CarWashAfterCallBack);
    }
    else {
        $(".SubChipCarWash").fadeOut(300);
        gOpenningSubBoxId = "";
        gSearchedChipId = "";
        $(".SCp").remove();
        $("#SubChipAreaActiveIndicator").removeClass("show");
        $("#SubChip_LoadingScreen").css({ "display": "none" });
        //タイマーをクリア
        commonClearTimer();
        //メインエリアのスクロール範囲を元に戻す
        ChangeMainScrollHeight(false);
    }
}
/**
* 納車待ちサブチップ表示
* @return {なし}
*/
function ShowDeliverdCarchip() {
    if ($(".SubChipWaitingDelivered").css('display') === 'none') {
        var strButtonPosition = $("#FooterButtonIcon500").offset().left + 12
        $(".SubChipWaitingDelivered .Triangle").css("left", "" + strButtonPosition + "px");
        $(".SubChipReception").fadeOut(300);
        $(".SubChipAdditionalWork").fadeOut(300);
        $(".SubChipCompletionInspection").fadeOut(300);
        $(".SubChipCarWash").fadeOut(300);
        $(".SubChipNoShow").fadeOut(300);
        $(".SubChipStop").fadeOut(300);
        $(".SubChipWaitingDelivered").fadeIn(300);
        if (gOpenningSubBoxId != "") {
            gSearchedChipId = "";
        }
        gOpenningSubBoxId = C_DELIVERDCAR;
        $("#SubChipAreaActiveIndicator").addClass("show");
        $("#SubChip_LoadingScreen").css({ "display": "block" });
        //リフレッシュタイマーセット
        commonRefreshTimerTabletSMB(DeliverdCarAreaReLoad);
        $(".SubChipBox").SC3240301fingerScroll();
        $(".SCp").remove();
        //サーバーに渡すパラメータを作成
        var prms = CreateCallBackSubChipParam(C_DELIVERDCAR);
        //コールバック開始
        DoCallBack(C_CALLBACK_WND301, prms, DeliWaitAfterCallBack, "ShowDeliverdCarchip");
        //gCallbackSC3240301.doCallback(prms, DeliWaitAfterCallBack);
    }
    else {
        $(".SubChipWaitingDelivered").fadeOut(300);
        gOpenningSubBoxId = "";
        gSearchedChipId = "";
        $(".SCp").remove();
        $("#SubChipAreaActiveIndicator").removeClass("show");
        $("#SubChip_LoadingScreen").css({ "display": "none" });
        //タイマーをクリア
        commonClearTimer();
        //メインエリアのスクロール範囲を元に戻す
        ChangeMainScrollHeight(false);
    }
}


/**
* NoShowサブチップ表示
* @return {なし}
*/
function ShowNoShowchip() {
    if ($(".SubChipNoShow").css('display') === 'none') {
        var strButtonPosition = $("#FooterButtonIcon600").offset().left + 12
        $(".SubChipNoShow .Triangle").css("left", "" + strButtonPosition + "px");
        $(".SubChipReception").fadeOut(300);
        $(".SubChipAdditionalWork").fadeOut(300);
        $(".SubChipCompletionInspection").fadeOut(300);
        $(".SubChipCarWash").fadeOut(300);
        $(".SubChipWaitingDelivered").fadeOut(300);
        $(".SubChipStop").fadeOut(300);
        $(".SubChipNoShow").fadeIn(300);
        if (gOpenningSubBoxId != "") {
            gSearchedChipId = "";
        }
        gOpenningSubBoxId = C_NOSHOW;
        $("#SubChipAreaActiveIndicator").addClass("show");
        $("#SubChip_LoadingScreen").css({ "display": "block" });
        //リフレッシュタイマーセット
        commonRefreshTimerTabletSMB(NoShowAreaReLoad);
        $(".SubChipBox").SC3240301fingerScroll();
        $(".SCp").remove();
        //サーバーに渡すパラメータを作成
        var prms = CreateCallBackSubChipParam(C_NOSHOW);
        //コールバック開始
        DoCallBack(C_CALLBACK_WND301, prms, NoShowAfterCallBack, "ShowNoShowchip");
        //gCallbackSC3240301.doCallback(prms, NoShowAfterCallBack);
    }
    else {
        $(".SubChipNoShow").fadeOut(300);
        gOpenningSubBoxId = "";
        gSearchedChipId = "";
        $(".SCp").remove();
        $("#SubChipAreaActiveIndicator").removeClass("show");
        $("#SubChip_LoadingScreen").css({ "display": "none" });
        //タイマーをクリア
        commonClearTimer();
        //メインエリアのスクロール範囲を元に戻す
        ChangeMainScrollHeight(false);
    }
}

/**
* 中断サブチップ表示
* @return {なし}
*/
function ShowStopchip() {
    if ($(".SubChipStop").css('display') === 'none') {
        var strButtonPosition = $("#FooterButtonIcon700").offset().left + 12
        $(".SubChipStop .Triangle").css("left", "" + strButtonPosition + "px");
        $(".SubChipReception").fadeOut(300);
        $(".SubChipAdditionalWork").fadeOut(300);
        $(".SubChipCompletionInspection").fadeOut(300);
        $(".SubChipCarWash").fadeOut(300);
        $(".SubChipWaitingDelivered").fadeOut(300);
        $(".SubChipNoShow").fadeOut(300);
        $(".SubChipStop").fadeIn(300);
        if (gOpenningSubBoxId != "") {
            gSearchedChipId = "";
        }
        gOpenningSubBoxId = C_STOP;
        $("#SubChipAreaActiveIndicator").addClass("show");
        $("#SubChip_LoadingScreen").css({ "display": "block" });
        //リフレッシュタイマーセット
        commonRefreshTimerTabletSMB(StopAreaReLoad);
        $(".SubChipBox").SC3240301fingerScroll();
        $(".SCp").remove();
        //サーバーに渡すパラメータを作成
        var prms = CreateCallBackSubChipParam(C_STOP);
        //コールバック開始
        DoCallBack(C_CALLBACK_WND301, prms, StopAfterCallBack, "ShowStopchip");
        // gCallbackSC3240301.doCallback(prms, StopAfterCallBack);
    }
    else {
        $(".SubChipStop").fadeOut(300);
        gOpenningSubBoxId = "";
        gSearchedChipId = "";
        $(".SCp").remove();
        $("#SubChipAreaActiveIndicator").removeClass("show");
        $("#SubChip_LoadingScreen").css({ "display": "none" });
        //タイマーをクリア
        commonClearTimer();
        //メインエリアのスクロール範囲を元に戻す
        ChangeMainScrollHeight(false);
    }
}

// 2013/11/29 TMEJ 張 タブレット版SMB チーフテクニシャン機能開発 START
/**
* Undoボタンを押す
* @return {-} 無し
*/
function UndoWashingChip() {
    $("#SubChipAreaActiveIndicator").addClass("show");
    $("#SubChip_LoadingScreen").css({ "display": "block" });
    //リフレッシュタイマーセット
    commonRefreshTimerTabletSMB(CarWashAreaReLoad);

    //サーバーに渡すパラメータを作成
    var prms = CreateCallBackActionButtonParam(C_FT_BTNID_UNDO);
    //コールバック開始
    DoCallBack(C_CALLBACK_WND301, prms, AfterCallBackFooterActionButton, "UndoWashingChip");
}
// 2013/11/29 TMEJ 張 タブレット版SMB チーフテクニシャン機能開発 END

/**
* 洗車開始ボタンタップ
* @return {なし}
*/
function ClickBtnStartWashCar() {
    $("#SubChipAreaActiveIndicator").addClass("show");
    $("#SubChip_LoadingScreen").css({ "display": "block" });
    //リフレッシュタイマーセット
    commonRefreshTimerTabletSMB(ReDisplay);
    //サーバーに渡すパラメータを作成
    var prms = CreateCallBackActionButtonParam(C_FT_BTNID_WASHSTART);
    //コールバック開始
    DoCallBack(C_CALLBACK_WND301, prms, AfterCallBackFooterActionButton, "ClickBtnStartWashCar");
    //gCallbackSC3240301.doCallback(prms, AfterCallBackFooterActionButton);
}
/**
* 洗車終了ボタンタップ
* @return {なし}
*/
function ClickBtnEndWashCar() {
    $("#SubChipAreaActiveIndicator").addClass("show");
    $("#SubChip_LoadingScreen").css({ "display": "block" });
    //リフレッシュタイマーセット
    commonRefreshTimerTabletSMB(ReDisplay);
    //サーバーに渡すパラメータを作成
    var prms = CreateCallBackActionButtonParam(C_FT_BTNID_WASHEND);
    //コールバック開始
    DoCallBack(C_CALLBACK_WND301, prms, AfterCallBackFooterActionButton, "ClickBtnEndWashCar");
    //gCallbackSC3240301.doCallback(prms, AfterCallBackFooterActionButton);
}
/**
* 納車ボタンタップ
* @return {なし}
*/
function ClickBtnDeli() {
    $("#SubChipAreaActiveIndicator").addClass("show");
    $("#SubChip_LoadingScreen").css({ "display": "block" });
    //リフレッシュタイマーセット
    commonRefreshTimerTabletSMB(ReDisplay);
    //サーバーに渡すパラメータを作成
    var prms = CreateCallBackActionButtonParam(C_FT_BTNID_DELI);
    //コールバック開始
    DoCallBack(C_CALLBACK_WND301, prms, AfterCallBackFooterActionButton, "ClickBtnDeli");
    //gCallbackSC3240301.doCallback(prms, AfterCallBackFooterActionButton);
}
// 2013/11/29 TMEJ 丁 タブレット版SMB チーフテクニシャン機能開発 START
/**
* 洗車へ移動ボタンタップ
* @return {なし}
*/
function ClickMoveToWash() {
    $("#SubChipAreaActiveIndicator").addClass("show");
    $("#SubChip_LoadingScreen").css({ "display": "block" });
    //リフレッシュタイマーセット
    commonRefreshTimerTabletSMB(ReDisplay);
    //サーバーに渡すパラメータを作成
    var prms = CreateCallBackActionButtonParam(C_FT_BTNTP_MOVETOWASH);
    //コールバック開始
    DoCallBack(C_CALLBACK_WND301, prms, AfterCallBackFooterActionButton, "ClickMoveToWash");
}
/**
* 納車待ちへ移動ボタンタップ
* @return {なし}
*/
function ClickMoveToDeliWait() {
    $("#SubChipAreaActiveIndicator").addClass("show");
    $("#SubChip_LoadingScreen").css({ "display": "block" });
    //リフレッシュタイマーセット
    commonRefreshTimerTabletSMB(ReDisplay);
    //サーバーに渡すパラメータを作成
    var prms = CreateCallBackActionButtonParam(C_FT_BTNTP_MOVETODELI);
    //コールバック開始
    DoCallBack(C_CALLBACK_WND301, prms, AfterCallBackFooterActionButton, "ClickMoveToDeliWait");
}
// 2013/11/29 TMEJ 丁 タブレット版SMB チーフテクニシャン機能開発 END

//2015/02/13 TMEJ 範 新プラットフォーム版i-CROP仕様変更対応 (SMBチップ検索及び作業中断機能強化) START
/**
* 中断終了ボタンイベント
*/
function ClickFinishStopChip() {

    var dtShowDate = $("#hidShowDate").val();  //当ページの日付

    $("#SubChipAreaActiveIndicator").addClass("show");
    $("#SubChip_LoadingScreen").css({ "display": "block" });

    //リフレッシュタイマーセット
    commonRefreshTimerTabletSMB(ReDisplay);

    //サーバーに渡すパラメータを作成
    var prms = {
        ButtonID: C_FT_BTNID_FINISHSTOPCHIP
       , ServiceInId: gArrObjSubChip[gSelectedChipId].svcInId
       , JobDtlId: gArrObjSubChip[gSelectedChipId].jobDtlId
       , StalluseId: gArrObjSubChip[gSelectedChipId].stallUseId
       , RowLockVersion: gArrObjSubChip[gSelectedChipId].rowLockVersion
       , ShowDate: dtShowDate
    };

    //コールバック開始
    DoCallBack(C_CALLBACK_WND301, prms, AfterCallBackFooterActionButton, "ClickFinishStopChip");

}
//2015/02/13 TMEJ 範 新プラットフォーム版i-CROP仕様変更対応 (SMBチップ検索及び作業中断機能強化) END

/**
* 受付ボタンの最新状態表示
* @return {なし}
*/
function ReceptionButtonRefresh() {
    var prms = CreateCallBackSubChipParam(C_UPDATECNT_RECEPTION);
    DoCallBack(C_CALLBACK_WND301, prms, SC3240301AfterCallBackRefresh, "ReceptionButtonRefresh");
    //gCallbackSC3240301.doCallback(prms, SC3240301AfterCallBackRefresh);
}
/**
* 追加作業ボタンの最新状態表示
* @return {なし}
*/
function AddWorkButtonRefresh() {
    var prms = CreateCallBackSubChipParam(C_UPDATECNT_ADDITIONALWORK);
    DoCallBack(C_CALLBACK_WND301, prms, SC3240301AfterCallBackRefresh, "AddWorkButtonRefresh");
   // gCallbackSC3240301.doCallback(prms, SC3240301AfterCallBackRefresh);
}
/**
* 完成検査ボタンの最新状態表示
* @return {なし}
*/
function CompletionInspecButtonRefresh() {
    var prms = CreateCallBackSubChipParam(C_UPDATECNT_COMPLETIONINSPECTION);
    DoCallBack(C_CALLBACK_WND301, prms, SC3240301AfterCallBackRefresh, "CompletionInspecButtonRefresh");
    //gCallbackSC3240301.doCallback(prms, SC3240301AfterCallBackRefresh);
}
/**
* 洗車ボタンの最新状態表示
* @return {なし}
*/
function CarWashButtonRefresh() {
    var prms = CreateCallBackSubChipParam(C_UPDATECNT_CARWASH);
    DoCallBack(C_CALLBACK_WND301, prms, SC3240301AfterCallBackRefresh, "CarWashButtonRefresh");
    //gCallbackSC3240301.doCallback(prms, SC3240301AfterCallBackRefresh);
}
/**
* 納車待ちボタンの最新状態表示
* @return {なし}
*/
function DeliverdCarButtonRefresh() {
    var prms = CreateCallBackSubChipParam(C_UPDATECNT_DELIVERDCAR);
    DoCallBack(C_CALLBACK_WND301, prms, SC3240301AfterCallBackRefresh, "DeliverdCarButtonRefresh");
    //gCallbackSC3240301.doCallback(prms, SC3240301AfterCallBackRefresh);
}
/**
* NoShowボタンの最新状態表示
* @return {なし}
*/
function NoShowButtonRefresh() {
    var prms = CreateCallBackSubChipParam(C_UPDATECNT_NOSHOW);
    DoCallBack(C_CALLBACK_WND301, prms, SC3240301AfterCallBackRefresh, "NoShowButtonRefresh");
    //gCallbackSC3240301.doCallback(prms, SC3240301AfterCallBackRefresh);
}
/**
* 中断ボタンの最新状態表示
* @return {なし}
*/
function StopButtonRefresh() {
    var prms = CreateCallBackSubChipParam(C_UPDATECNT_STOP);
    DoCallBack(C_CALLBACK_WND301, prms, SC3240301AfterCallBackRefresh, "StopButtonRefresh");
    //gCallbackSC3240301.doCallback(prms, SC3240301AfterCallBackRefresh);
}
/**
* 全てのサブチップボタン最新状態表示（個別）
* @return {なし}
*/
function AllButtonRefresh() {
    //コールバック開始
    var prms = CreateCallBackSubChipParam(C_UPDATE_ALLCONUT);
    DoCallBack(C_CALLBACK_WND301, prms, SC3240301AfterCallBackRefresh, "AllButtonRefresh");
    //gCallbackSC3240301.doCallback(prms, SC3240301AfterCallBackRefresh);
}
/**
* コールバック後の処理関数(受付)
* 
* @param {String} result コールバック呼び出し結果
*
*/
function ReceptionAfterCallBack(result) {
    var jsonResult = $.parseJSON(result);
    if (jsonResult.Message) {
        //エラーメッセージの表示
        alert(htmlDecode(jsonResult.Message));
        //操作リストをクリアする
        ClearOperationList();
        return;
    }
    //相応なサブボックスが閉じられたら何もしない
    if (gOpenningSubBoxId == C_RECEPTION) {
        CreateSubChips(C_RECEPTION, result);
        //メインストールエリアスクロール範囲を拡大する
        ChangeMainScrollHeight(true);
    }
    //コールバック終了
    AfterCallBack();
}
/**
* コールバック後の処理関数(追加作業)
* 
* @param {String} result コールバック呼び出し結果
*
*/
function AddWorkAfterCallBack(result) {
    var jsonResult = $.parseJSON(result);
    if (jsonResult.Message) {
        //エラーメッセージの表示
        alert(htmlDecode(jsonResult.Message));
        //操作リストをクリアする
        ClearOperationList();
        return;
    }
    //相応なサブボックスが閉じられたら何もしない
    if (gOpenningSubBoxId == C_ADDITIONALWORK) {
        CreateSubChips(C_ADDITIONALWORK, result);
        //メインストールエリアスクロール範囲を拡大する
        ChangeMainScrollHeight(true);
    }
    //コールバック終了
    AfterCallBack();
}
/**
* コールバック後の処理関数(完成検査)
* 
* @param {String} result コールバック呼び出し結果
*
*/
function InspectionComAfterCallBack(result) {
    var jsonResult = $.parseJSON(result);
    if (jsonResult.Message) {
        //エラーメッセージの表示
        alert(htmlDecode(jsonResult.Message));
        //操作リストをクリアする
        ClearOperationList();
        return;
    }
    //相応なサブボックスが閉じられたら何もしない
    if (gOpenningSubBoxId == C_COMPLETIONINSPECTION) {
        CreateSubChips(C_COMPLETIONINSPECTION, result);
        //メインストールエリアスクロール範囲を拡大する
        ChangeMainScrollHeight(true);
    }
    //コールバック終了
    AfterCallBack();
}
/**
* コールバック後の処理関数(洗車)
* 
* @param {String} result コールバック呼び出し結果
*
*/
function CarWashAfterCallBack(result) {
    var jsonResult = $.parseJSON(result);
    if (jsonResult.Message) {
        //エラーメッセージの表示
        alert(htmlDecode(jsonResult.Message));
        //操作リストをクリアする
        ClearOperationList();
        return;
    }
    //相応なサブボックスが閉じられたら何もしない
    if (gOpenningSubBoxId == C_CARWASH) {
        CreateSubChips(C_CARWASH, result);
        //メインストールエリアスクロール範囲を拡大する
        ChangeMainScrollHeight(true);
    }
    //コールバック終了
    AfterCallBack();
}

/**
* コールバック後の処理関数(納車待ち)
* 
* @param {String} result コールバック呼び出し結果
*
*/
function DeliWaitAfterCallBack(result) {

    var jsonResult = $.parseJSON(result);
    if (jsonResult.Message) {
        //エラーメッセージの表示
        alert(htmlDecode(jsonResult.Message));
        //操作リストをクリアする
        ClearOperationList();
        return;
    }
    //相応なサブボックスが閉じられたら何もしない
    if (gOpenningSubBoxId == C_DELIVERDCAR) {
        CreateSubChips(C_DELIVERDCAR, result);
        //メインストールエリアスクロール範囲を拡大する
        ChangeMainScrollHeight(true);
    }

    //コールバック終了
    AfterCallBack();
}
/**
* コールバック後の処理関数(NoShow)
* 
* @param {String} result コールバック呼び出し結果
*
*/
function NoShowAfterCallBack(result) {
    var jsonResult = $.parseJSON(result);
    if (jsonResult.Message) {
        //エラーメッセージの表示
        alert(htmlDecode(jsonResult.Message));
        //操作リストをクリアする
        ClearOperationList();
        return;
    }
    //相応なサブボックスが閉じられたら何もしない
    if (gOpenningSubBoxId == C_NOSHOW) {
        CreateSubChips(C_NOSHOW, result);
        //メインストールエリアスクロール範囲を拡大する
        ChangeMainScrollHeight(true);
    }
    //コールバック終了
    AfterCallBack();
}
/**
* コールバック後の処理関数(中断)
* 
* @param {String} result コールバック呼び出し結果
*
*/
function StopAfterCallBack(result) {
    var jsonResult = $.parseJSON(result);
    if (jsonResult.Message) {
        //エラーメッセージの表示
        alert(htmlDecode(jsonResult.Message));
        //操作リストをクリアする
        ClearOperationList();
        return;
    }
    //相応なサブボックスが閉じられたら何もしない
    if (gOpenningSubBoxId == C_STOP) {
        CreateSubChips(C_STOP, result);
        //メインストールエリアスクロール範囲を拡大する
        ChangeMainScrollHeight(true);
    }

    //コールバック終了
    AfterCallBack();
}
/**
* コールバック後の処理関数(洗車開始、洗車終了、納車、削除)
* @param {String} result コールバック呼び出し結果
*/
function AfterCallBackFooterActionButton(result) {
    //タイマーをクリア
    commonClearTimer();
    var jsonResult = JSON.parse(result);

    //2015/04/23 TMEJ 明瀬  DMS連携版サービスタブレット強制納車機能追加開発 START
    
    //if (jsonResult.ResultCode == 0) {
    if (jsonResult.ResultCode == 0 ||
        jsonResult.ResultCode == -9000) {
        //サーバでの処理結果が下記の場合
        //　　0(成功)、または
        //-9000(DMS除外エラーの警告)

        if (jsonResult.ResultCode == -9000) {
            //サーバでの処理結果が、-9000(DMS除外エラーの警告)の場合
            
            //メッセージを表示する
            icropScript.ShowMessageBox(jsonResult.ResultCode, htmlDecode(jsonResult.Message), "");

        }
    //2015/04/23 TMEJ 明瀬  DMS連携版サービスタブレット強制納車機能追加開発 END

        // 選択したチップを解放する
        SetTableUnSelectedStatus();
        // チップ選択状態を解除する
        SetChipUnSelectedStatus();
        if (jsonResult.SubButtonID != C_NOSHOW) {
            //最新のストールチップを表示する
            // 2013/11/29 TMEJ 張 タブレット版SMB チーフテクニシャン機能開発 START
            //ShowLatestChips(htmlDecode(jsonResult.StallChip));
            ShowLatestChips(htmlDecode(jsonResult.StallChip), false, false);
            // 2013/11/29 TMEJ 張 タブレット版SMB チーフテクニシャン機能開発 END
        }
        //サブボックスがとじられたらサブチップ再表示しない
        if (gOpenningSubBoxId == jsonResult.SubButtonID) {
//            //アクティブインジケータを表示
//            $("#SubChipAreaActiveIndicator").addClass("show");
//            $("#SubChip_LoadingScreen").css({ "display": "block" });
            //サブチップエリア再表示
            if (jsonResult.SubButtonID == C_CARWASH) {
                //洗車エリアリフレッシュ
                CreateSubChips(C_CARWASH, htmlDecode(jsonResult.CarwrashArea));
                if ((jsonResult.ButtonID == C_FT_BTNID_WASHEND) || (jsonResult.ButtonID == C_FT_BTNTP_MOVETODELI)) {
                    //洗車終了、納車待ちへ移動の場合は「納車待ち」ボタンの情報を更新する
                    InitializationSubChip(htmlDecode(jsonResult.DropofButtonInfo));
                }
            } else if (jsonResult.SubButtonID == C_DELIVERDCAR) {
                //納車エリアリフレッシュ
                CreateSubChips(C_DELIVERDCAR, htmlDecode(jsonResult.DropoffArea));
                // 2013/11/29 TMEJ 丁 タブレット版SMB チーフテクニシャン機能開発 START
                //洗車へ移動処理の場所
                if (jsonResult.ButtonID == C_FT_BTNTP_MOVETOWASH) {
                    InitializationSubChip(htmlDecode(jsonResult.CarWashButtonInfo));
                }
                // 2013/11/29 TMEJ 丁 タブレット版SMB チーフテクニシャン機能開発 END
            } else if (jsonResult.SubButtonID == C_NOSHOW) {
                //NoShowエリアを再描画
                CreateSubChips(C_NOSHOW, htmlDecode(jsonResult.NoShowArea));
                //2015/02/13 TMEJ 範 新プラットフォーム版i-CROP仕様変更対応 (SMBチップ検索及び作業中断機能強化) START
            } else if (jsonResult.SubButtonID == C_STOP) {
                //中断エリアを再描画
                CreateSubChips(C_STOP, htmlDecode(jsonResult.JobStopArea));

                //納車、洗車、完成検査ボタンの数字、色を最描画
                InitializationSubChip(htmlDecode(jsonResult.DropofButtonInfo));
                InitializationSubChip(htmlDecode(jsonResult.CarWashButtonInfo));
                InitializationSubChip(htmlDecode(jsonResult.ComplInsButtonInfo));

                //最新ストールチップを更新
                ShowLatestChips(htmlDecode(jsonResult.StallChip), false, false);

                //2015/02/13 TMEJ 範 新プラットフォーム版i-CROP仕様変更対応 (SMBチップ検索及び作業中断機能強化) END
            }
        } else {
            if (jsonResult.SubButtonID == C_CARWASH) {
                //洗車ボタン情報更新
                InitializationSubChip(htmlDecode(jsonResult.CarWashButtonInfo));
                // 2013/11/29 TMEJ 丁 タブレット版SMB チーフテクニシャン機能開発 START
                //if (jsonResult.ButtonID == C_FT_BTNID_WASHEND){
                if ((jsonResult.ButtonID == C_FT_BTNID_WASHEND) || (jsonResult.ButtonID == C_FT_BTNTP_MOVETODELI)) {
                    // 2013/11/29 TMEJ 丁 タブレット版SMB チーフテクニシャン機能開発 END
                    InitializationSubChip(htmlDecode(jsonResult.DropofButtonInfo));
                }
            } else if (jsonResult.SubButtonID == C_DELIVERDCAR) {
                //納車ボタン情報更新
                InitializationSubChip(htmlDecode(jsonResult.DropofButtonInfo));
                // 2013/11/29 TMEJ 丁 タブレット版SMB チーフテクニシャン機能開発 START
                //洗車へ移動処理の場所
                if (jsonResult.ButtonID == C_FT_BTNTP_MOVETOWASH) {
                    InitializationSubChip(htmlDecode(jsonResult.CarWashButtonInfo));
                }
                // 2013/11/29 TMEJ 丁 タブレット版SMB チーフテクニシャン機能開発 END
            } else if (jsonResult.SubButtonID == C_NOSHOW) {
                //NoShowボタン情報更新
                InitializationSubChip(htmlDecode(jsonResult.NoShowButtonInfo));

                //2015/02/13 TMEJ 範 新プラットフォーム版i-CROP仕様変更対応 (SMBチップ検索及び作業中断機能強化) START
            } else if (jsonResult.SubButtonID == C_STOP) {
                //中断エリアを再描画
                InitializationSubChip(htmlDecode(jsonResult.JobStopArea));

                //納車、洗車、完成検査ボタンの数字、色を最描画
                InitializationSubChip(htmlDecode(jsonResult.DropofButtonInfo));
                InitializationSubChip(htmlDecode(jsonResult.CarWashButtonInfo));
                InitializationSubChip(htmlDecode(jsonResult.ComplInsButtonInfo));

                //最新ストールチップを更新
                ShowLatestChips(htmlDecode(jsonResult.StallChip), false, false);
                //2015/02/13 TMEJ 範 新プラットフォーム版i-CROP仕様変更対応 (SMBチップ検索及び作業中断機能強化) END
            }
        }
        //コールバック終了
        AfterCallBack();
    } else {
        //2015/02/13 TMEJ 範 新プラットフォーム版i-CROP仕様変更対応 (SMBチップ検索及び作業中断機能強化) START

//        if ((jsonResult.ResultCode == 3) || (jsonResult.ResultCode == 23)) {
        if ((jsonResult.ResultCode == 3)  || 
            (jsonResult.ResultCode == 23) ||
            (jsonResult.ResultCode == 27)) {
            //3 ：追加作業の確認チェックエラー
            //23：清算準備完了チェックエラー
            //27：チップに紐づくJobの実績存在(作業終了時)チェックエラー

            //2015/02/13 TMEJ 範 新プラットフォーム版i-CROP仕様変更対応 (SMBチップ検索及び作業中断機能強化) END
            //エラーメッセージの表示
            alert(htmlDecode(jsonResult.Message));
            //操作リストをクリアする
            ClearOperationList();
            // 選択したチップを解放する
            SetTableUnSelectedStatus();
            // チップ選択状態を解除する
            SetChipUnSelectedStatus();
            //サブチップアイコンを選択状態にする
            if (gOpenningSubBoxId == jsonResult.SubButtonID) {
                FooterIconReplace(gOpenningSubBoxId);
            }
            // 記録のオブジェクトをクリアする
            gArrBackChipObj.length = 0;    
            $("#SubChipAreaActiveIndicator").removeClass("show");
            $("#SubChip_LoadingScreen").css({ "display": "none" });

            // 初期画面に表示されない場合、下へスクロールして、表示する
            $(".SubChipBox").SC3240301fingerScroll({
                action: "move",
                moveY: $(".SubChipBox .scroll-inner").position().top,
                moveX: $(".SubChipBox .scroll-inner").position().left
            });
        } else {
            //エラーメッセージの表示
            alert(htmlDecode(jsonResult.Message));
            //操作リストをクリアする
            ClearOperationList();
            // 選択したチップを解放する
            SetTableUnSelectedStatus();
            // チップ選択状態を解除する
            SetChipUnSelectedStatus();
            // 記録のオブジェクトをクリアする
            gArrBackChipObj.length = 0;
            //画面をリフレッシュ
            SetSubChipBoxClose();
            ClickChangeDate(0);
        }
    }
}

/**
* コールバック後の処理関数(受付ボタンの状態更新)
* 
* @param {String} result コールバック呼び出し結果
*
*/
function SC3240301AfterCallBackRefresh(result) {
    var jsonResult = $.parseJSON(result);
    if (jsonResult.Message) {
        //エラーメッセージの表示
        alert(htmlDecode(jsonResult.Message));
        //操作リストをクリアする
        ClearOperationList();
        return;
    }
    InitializationSubChip(result);
    //コールバック終了
    AfterCallBack();
}
/**
* サブチップの生成
* @return {なし}
*/
function CreateSubChips(buttonID, jsonData) {
    //サブボックスが閉じられたの場合何もしない
    if (gOpenningSubBoxId != buttonID){
        return;
    }

    // グロバール変数の初期化
    gArrObjSubChip = null;
    gArrObjSubChip = new Array();
    $(".SCp").remove();
    var strSubChipNo = 0;
    var strTargetArea;
    var strKey;

    //2013/11/29 TMEJ 丁 タブレット版SMB チーフテクニシャン機能開発 START
    //部品ステータス取得エラーId
    var strPartsErrorId = 0;
    //2013/11/29 TMEJ 丁 タブレット版SMB チーフテクニシャン機能開発 END

    //サーチチップId
    var strSearchedSubChipId;
    switch (buttonID) {
        case "100":
            strTargetArea = "SubChipReception";
            break;
        case "200":
            strTargetArea = "SubChipAdditionalWork";
            break;
        case "300":
            strTargetArea = "SubChipCompletionInspection";
            break;
        case "400":
            strTargetArea = "SubChipCarWash";
            break;
        case "500":
            strTargetArea = "SubChipWaitingDelivered";
            break;
        case "600":
            strTargetArea = "SubChipNoShow";
            break;
        case "700":
            strTargetArea = "SubChipStop";
            break;
    }

    // JSON形式のデータを変換し、処理する.
    var chipDataList = $.parseJSON(jsonData);
    // 取得したチップ情報をチップクラスに格納し、再描画.
    for (var keyString in chipDataList) {
        var chipData = chipDataList[keyString];
        switch (buttonID) {
            case "100":
                // 2017/09/05 NSK 小川 REQ-SVT-TMT-20160906-003 計画済みチップをストールから簡単に外せるようにする START
                //strKey = chipData.RO_NUM + "_" + chipData.RO_JOB_SEQ + "_" + C_RECEPTION;

                if (chipData.TEMP_FLG == C_TEMP_FLG) {
                    strKey = chipData.JOB_DTL_ID + "_" + C_RECEPTION;
                }else{
                    strKey = chipData.RO_NUM + "_" + chipData.RO_JOB_SEQ + "_" + C_RECEPTION;
                }
                // 2017/09/05 NSK 小川 REQ-SVT-TMT-20160906-003 計画済みチップをストールから簡単に外せるようにする END
                break;

            case "200":
                strKey = chipData.RO_NUM + "_" + chipData.RO_JOB_SEQ + "_" + C_ADDITIONALWORK;                
                break;
            case "300":
                strKey = chipData.JOB_DTL_ID + "_" + C_COMPLETIONINSPECTION;
                break;
            case "400":
                strKey = chipData.SVCIN_ID + "_" + C_CARWASH;
                break;
            case "500":
                strKey = chipData.SVCIN_ID + "_" + C_DELIVERDCAR;
                break;
            case "600":
                strKey = chipData.JOB_DTL_ID + "_" + C_NOSHOW;
                break;
            case "700":
                strKey = chipData.JOB_DTL_ID + "_" + C_STOP;
                break;
        }
        if (gArrObjSubChip[strKey]) {
            continue;
        }
        if (gArrObjSubChip[strKey] == undefined) {
            gArrObjSubChip[strKey] = new ReserveSubChip(strKey);
        }
        gArrObjSubChip[strKey].setSubChipParameter(chipData, buttonID);
        //2013/11/29 TMEJ 丁 タブレット版SMB チーフテクニシャン機能開発 START
        // if (gArrObjSubChip[strKey].roJobSeq > 0) {
        if ((gArrObjSubChip[strKey].roJobSeq > 0) && ((gArrObjSubChip[strKey].subChipAreaId == C_RECEPTION) || (gArrObjSubChip[strKey].subChipAreaId == C_ADDITIONALWORK))) {
            //2013/11/29 TMEJ 丁 タブレット版SMB チーフテクニシャン機能開発 END
            gArrObjSubChip[strKey].rsltStartDateTime = new Date(C_DATE_DEFAULT_VALUE);
            gArrObjSubChip[strKey].rsltEndDateTime = new Date(C_DATE_DEFAULT_VALUE);
            //2013/11/29 TMEJ 丁 タブレット版SMB チーフテクニシャン機能開発 START
            gArrObjSubChip[strKey].stallUseId = "0";
            //2013/11/29 TMEJ 丁 タブレット版SMB チーフテクニシャン機能開発 END
        }
        if (gSearchedChipId) {
            //2017/07/12 NSK 河谷 REQ-SVT-TMT-20160906-001 チップ検索の検索範囲をPC-SMBと同じにする START
            // if (gArrObjSubChip[strKey].stallUseId == gSearchedChipId) {
            //     strSearchedSubChipId = strKey;
            //     gSearchedChipId = "";
            // }
            if ((gSearchedSubChipAreaId == C_RECEPTION && gArrObjSubChip[strKey].tempFlg == C_TEMP_FLG_OFF) || (gSearchedSubChipAreaId == C_ADDITIONALWORK)) {
                // 仮置きでない受付エリアのチップ、もしくは追加作業エリアのチップはRO番号、RO連番で特定する
                if ((gArrObjSubChip[strKey].roNum == gSearchedChipRoNum) && (gArrObjSubChip[strKey].roJobSeq == gSearchedChipRoSeq)) {
                    strSearchedSubChipId = strKey;
                    gSearchedChipId = "";
                }
            }
            else if (gArrObjSubChip[strKey].stallUseId == gSearchedChipId) {
                strSearchedSubChipId = strKey;
                gSearchedChipId = "";
            }
            //2017/07/12 NSK 河谷 REQ-SVT-TMT-20160906-001 チップ検索の検索範囲をPC-SMBと同じにする END
        }

        //2013/11/29 TMEJ 丁 タブレット版SMB チーフテクニシャン機能開発 START
        //部品取得エラーの時メッセージを表示するため
        switch (gArrObjSubChip[strKey].partsFlg) {
            case "24":
                strPartsErrorId = 913;
                break;
            case "25":
                strPartsErrorId = 914;
                break;
            case "26":
                strPartsErrorId = 915;
                break;      
        }
        //2013/11/29 TMEJ 丁 タブレット版SMB チーフテクニシャン機能開発 END

        $("#" + strKey).remove();
        // チップ生成
        gArrObjSubChip[strKey].createSubChip(strTargetArea, strKey);
        // サブチップの位置を設定する
        SetSubChipPosition(strKey, strSubChipNo);
        // チップをタップする時のイベントを登録
        BindSubChipClickEvent(gArrObjSubChip[strKey]);
        strSubChipNo = strSubChipNo + 1;
    };
    $(".SubChipBox .scroll-inner").width("" + gSubAreawidth + "");
    $(".SubChipBox .scroll-inner").height("174px");
    //タイマーをクリア
    commonClearTimer();
    //アクティブインジケータ閉じる
    $("#SubChipAreaActiveIndicator").removeClass("show");
    $("#SubChip_LoadingScreen").css({ "display": "none" });
    gSubAreawidth = 1024;
    if ($("." + strTargetArea).css("display") === 'block') {
        if (strSubChipNo != 0) {
            var count
            if (buttonID == "500") {
                count = "";
            } else {
                count = strSubChipNo;
            }
            if (gstrLateflg > 0) {
                SetWarningIcon(buttonID, C_FT_BTNDISP_ON, count);
            } else {
                SetNormalIcon(buttonID, C_FT_BTNDISP_ON, count);
            }
        }
        else {
            SetVisibilityIcon(buttonID, C_FT_BTNDISP_ON);
        }
    } else {
        $(".SCp").remove();
    }
    //遅れるフラグを初期化
    gstrLateflg = 0;
    //タイマーをクリア
    commonClearTimer();

    if (strSearchedSubChipId) {
        if (gArrObjSubChip[strSearchedSubChipId]) {
            //2017/07/12 NSK 河谷 REQ-SVT-TMT-20160906-001 チップ検索の検索範囲をPC-SMBと同じにする START
//            // 選択中にする
//            SubChipTap(gArrObjSubChip[strSearchedSubChipId]);
            // サーチからに表示されない場合、スクロールして、表示する
            SearchChipScroll(strSearchedSubChipId);
            // 選択中にする
            SubChipTap(gArrObjSubChip[strSearchedSubChipId]);
            //2017/07/12 NSK 河谷 REQ-SVT-TMT-20160906-001 チップ検索の検索範囲をPC-SMBと同じにする END
        }
    }

    //2013/11/29 TMEJ 丁 タブレット版SMB チーフテクニシャン機能開発 START
    //部品取得エラーの時画面表示続行、エラーメッセージ表示
    if (strPartsErrorId) {
        ShowSC3240301Msg(strPartsErrorId);
    }
    //2013/11/29 TMEJ 丁 タブレット版SMB チーフテクニシャン機能開発 END
}
/**
* サブチップの位置を設定する
* @param {-} strChipId  チップid
* @param {-} strSubChipNo  チップ番号
* @return {なし}
*/
function SetSubChipPosition(strChipId, strSubChipNo) {
    var nTop, nLeft;
    var scrollwidth
    nTop = 64;
    nLeft = strSubChipNo * 140 + 50;
    $("#" + strChipId).css({ "top": nTop, "left": nLeft, "width": C_SubChipWidth });
    scrollwidth = nLeft + C_SubChipWidth + 50;
    if (scrollwidth > gSubAreawidth) {
        gSubAreawidth = scrollwidth;
    }

    AdjustSubChipItemByWidth(strChipId);
}
/**
* コールバックでサーバーに渡すパラメータを作成する
*
* @param {String}   buttonID:   コールバック時のメソッド分岐用
*
*/
function CreateCallBackSubChipParam(buttonID) {
    var rtnVal = {
         ButtonID: buttonID
       , DisplayDate: $("#hidShowDate").val()
    };

    return rtnVal;
}
/**
* コールバックでサーバーに渡すパラメータを作成する(洗車開始、洗車終了、納車、削除)
*
* @param {String}   buttonID:   コールバック時のメソッド分岐用
*
*/
function CreateCallBackActionButtonParam(buttonID) {
    var rtnVa;
    var dtShowDate = $("#hidShowDate").val();  //当ページの日付
    if (buttonID == C_FT_BTNID_DEL) {
        rtnVal ={
            ButtonID: buttonID
            //2013/11/29 TMEJ 丁 タブレット版SMB チーフテクニシャン機能開発 START
       , ServiceInId: gArrObjSubChip[gSelectedChipId].svcInId
       , JobDtlId: gArrObjSubChip[gSelectedChipId].jobDtlId
            //2013/11/29 TMEJ 丁 タブレット版SMB チーフテクニシャン機能開発 END
       , StalluseId: gArrObjSubChip[gSelectedChipId].stallUseId           
       , RowLockVersion: gArrObjSubChip[gSelectedChipId].rowLockVersion
       , ShowDate: dtShowDate
        };
    }
    else {
        rtnVal = {
            ButtonID: buttonID
       , ServiceInId: gArrObjSubChip[gSelectedChipId].svcInId
            //2013/11/29 TMEJ 丁 タブレット版SMB チーフテクニシャン機能開発 START
       , JobDtlId: gArrObjSubChip[gSelectedChipId].jobDtlId
       , StalluseId: gArrObjSubChip[gSelectedChipId].stallUseId
            //2013/11/29 TMEJ 丁 タブレット版SMB チーフテクニシャン機能開発 END
       , RoNum: gArrObjSubChip[gSelectedChipId].roNum
       , PickDeliType: gArrObjSubChip[gSelectedChipId].pickDeliType
       , RowLockVersion: gArrObjSubChip[gSelectedChipId].rowLockVersion
       , ShowDate: dtShowDate
        };
    }
    return rtnVal;
}
/**
* チップタップ時のイベントを登録
* @param {String} strChipId チップID
* @return {なし}
*/
function BindSubChipClickEvent(objChip) {

    //チップタップ時のイベントを登録
    $("#" + objChip.KEY).bind("chipTap", function (e) {
        // ポップアップウィンドウが表示中、何もしない
        if (GetDisplayPopupWindow()) {
            return;
        }
        // 影があるチップにタップする時、何もしない
        if ($("#" + objChip.KEY + " .Front").hasClass("BlackBack")) {
            return;
        }
        // サブチップが配置中の場合何もしない
        if ($("#" + objChip.KEY)[0].parentElement.className != "SubChipArea") {
            return;
        }

        $("#" + objChip.KEY + " .Front").removeClass("TapBlueBack");
        setTimeout(function () {
            // 青色表示した後で、サブチップのタップソースを走る
            SubChipTap(objChip);
        }, 0);
    })
    .bind(C_TOUCH_START, function (e) {
        // 影があるチップにタップする時、何もしない
        if ($("#" + objChip.KEY + " .Front").hasClass("BlackBack")) {
            return;
        }
        // タップすると青色を表示する
        $("#" + objChip.KEY + " .Front").addClass("TapBlueBack");
    })
    .bind(C_TOUCH_MOVE + " " + C_TOUCH_END, function (e) {
        $("#" + objChip.KEY + " .Front").removeClass("TapBlueBack");
    });
}

function SubChipTap(objChip) {
    //2013/04/24 myose add start    
    var areaId = GetSubChipType(objChip.KEY);
    SetTranslateValSubBox(areaId);
    //2013/04/24 myose add end
    // 選択したチップがあれば
    if (gSelectedChipId != "") {
        // チップに2回目でタッチして、選択状態を解放して、何もしない
        var strChipPattern = GetChipUpdatetype(gSelectedChipId)
        if (gSelectedChipId == objChip.KEY) {
            // 選択したチップを解放する
            SetTableUnSelectedStatus();
            // チップ選択状態を解除する
            SetChipUnSelectedStatus();
            //サブチップアイコンを選択状態にする
            if (gOpenningSubBoxId != "") {
                FooterIconReplace(gOpenningSubBoxId);
            }
            return;
        } else if (!$("#" + objChip.KEY + " .Front").hasClass("BlackBack") && $("#" + objChip.KEY).length > 0) {
            if (strChipPattern == C_UPDATA_SUBCHIP) {
                // 選択したチップを解放する
                SetTableUnSelectedStatus();
                // チップ選択状態を解除する
                SetChipUnSelectedStatus();
                if (gOpenningSubBoxId != "") {
                    FooterIconReplace(gOpenningSubBoxId);
                }
            }
        }
        return;
    }
    // gSelectedChipIdに選択したチップのidを保存する
    gSelectedChipId = objChip.KEY;
    var strRezSEQ = GetSubChipSVCINID(gSelectedChipId);
    // 遅れストール画面が表示中、もとに戻す
    if (gShowLaterStallFlg == true) {
        // もとに戻す
        ShowAllStall();
    }

    if (((objChip.subChipAreaId == C_RECEPTION) || (objChip.subChipAreaId == C_NOSHOW) || (objChip.subChipAreaId == C_STOP))) {
        //Movingチップを生成する
        drawSelectedSubChip(objChip.KEY);
    } else {
        //ストール上のリレーションチップを探す
        var arrRelationChipId = FindRelationChipsFromSubChip("", objChip.svcInId);
        // テーブルの状態をチップの選択状態に設定する
        SetSubChipAreaSelectedStatus(arrRelationChipId);
    }

    //2014/06/18 TMEJ 丁 タブレット版SMB テレマ走行距離機能開発 START
    // フッターボタンを変える
    //CreateFooterButton(C_FT_DISPTP_SELECTED, GetSubChipFootButtonType(objChip.KEY));
    CreateFooterButton(C_FT_DISPTP_SELECTED, GetSubChipFootButtonType(objChip.KEY), gArrObjSubChip[objChip.KEY].tlmContractFlg);
    //2014/06/18 TMEJ 丁 タブレット版SMB テレマ走行距離機能開発 END

    //2015/03/11 TMEJ 明瀬 既存バグ修正 START
    //// 納車予定日時の線を表示する
    //if (IsDefaultDate(objChip.scheDeliDateTime) == true) {
    //    // 納車予定日時により、時刻線の位置を取得して、設定する
    //    var setPosition = GetTimeLinePosByTime(objChip.scheDeliDateTime);
    //    // 表示にする
    //    $(".TimingLineDeli").css({"left": setPosition,"visibility": "visible"});
    //  
    //    }

    // 納車予定日時がある場合、
    if ((objChip.scheDeliDateTime) && (IsDefaultDate(objChip.scheDeliDateTime) == false)) {
        // 納車予定日時により、時刻線の位置を取得して、設定する
        // 納車遅れ見込み時間取得
        // 2017/09/25 NSK 竹中(悠) REQ-SVT-TMT-20170227-002 見込納車遅れ日時計算ロジック統一 START
        // var dtScheDeliLater = GetDeliDelayExpectedTimeLine(objChip.carWashNeedFlg, objChip.cwRsltStartDateTime, objChip.scheDeliDateTime, objChip.svcStatus);
        var dtScheDeliLater = GetDeliDelayExpectedTimeLine(objChip.carWashNeedFlg
                                                         , objChip.scheDeliDateTime
                                                         , objChip.svcStatus
                                                         , objChip.remainingInspectionType);
        // 2017/09/25 NSK 竹中(悠) REQ-SVT-TMT-20170227-002 見込納車遅れ日時計算ロジック統一 END
        var setPosition = GetTimeLinePosByTime(dtScheDeliLater);
        $(".TimingLineDeli").css("left", setPosition);
        // 表示にする
        $(".TimingLineDeli").css("visibility", "visible");
    }
    //2015/03/11 TMEJ 明瀬 既存バグ修正 END

}

/**
* サブチップエリアの状態をチップの選択状態に設定する
* @param {String} arrRelationChipId ChipIdlist
*
* @return {なし}
*
*/
function SetSubChipAreaSelectedStatus(arrRelationChipId) {

    SetTableSelectedStatus();
    // 選択中チップがBlackBack色を解除する
    $("#" + gSelectedChipId + " .Front").removeClass("BlackBack");
    // メイン画面でリレーションチップがあれば
    if (arrRelationChipId.length > 0) {
        // リレーションチップの線を表示する
        ShowRelationLine(arrRelationChipId[0][0]);
    }

    // 2017/09/05 NSK 小川 REQ-SVT-TMT-20160906-003 計画済みチップをストールから簡単に外せるようにする START
    // 仮置きチップを選択した場合、関係チップをハイライトしない
    if (gArrObjSubChip[gSelectedChipId].tempFlg == "1" && gArrObjSubChip[gSelectedChipId].subChipAreaId == "100") {
        $(".MCp .Front").addClass("BlackBack");
    }
    // 2017/09/05 NSK 小川 REQ-SVT-TMT-20160906-003 計画済みチップをストールから簡単に外せるようにする END

    // 定期リフレッシュと自動スクロールをやめる
    clearInterval(gFuncRefreshTimerInterval);
    gFuncRefreshTimerInterval = "";
    gScrollTimerInterval = "";
}

/**
* サブチップタイプを取得する
*
* @param {String} strChipId チップid
* @return {なし}
*
*/
function GetSubChipType(strChipId) {
    if (strChipId == "") {
        return "";
    }

    //2014/07/15 TMEJ 張 タブレットSMB Job Dispatch機能開発 START
    if ($("#" + strChipId).length == 0) {
        // 画面にいない場合、空白を戻す

        return "";
    }
    //2014/07/15 TMEJ 張 タブレットSMB Job Dispatch機能開発 END

    switch ($("#" + strChipId).offsetParent().offsetParent().offsetParent()[0].className) {
        case "SubChipReception":
            return C_FT_BTNTP_CONFIRMED_RO;
            break;
        case "SubChipAdditionalWork":
            return C_FT_BTNTP_WAIT_CONFIRMEDADDWORK;
            break;
        case "SubChipCompletionInspection":
            return C_FT_BTNTP_CONFIRMED_INSPECTION;
            break;
        case "SubChipCarWash":
            //洗車開始日時のチェック
            if (gArrObjSubChip[strChipId].svcStatus == "07") {
                //洗車開始待ち
                return C_FT_BTNTP_WAITING_WASH;
            } else if(gArrObjSubChip[strChipId].svcStatus == "08"){
                //洗車中
                return C_FT_BTNTP_WASHING;
            }
            break;
        case "SubChipWaitingDelivered":
            return C_FT_BTNTP_WAIT_DELIVERY;
            break;
        case "SubChipNoShow":
            return C_FT_BTNTP_NOSHOW;
            break;
        case "SubChipStop":
            return C_FT_BTNTP_STOP;
            break;
        default:
            return "";
            break;
    }
}

/**
* サブチップタイプを取得する
*
* @param {String} strChipId チップid
* @return {なし}
*
*/
function GetSubChipFootButtonType(strChipId) {
    switch ($("#" + strChipId).offsetParent().offsetParent().offsetParent()[0].className) {
        case "SubChipReception":
            if ((gArrObjSubChip[strChipId].stallId == "0") || (gArrObjSubChip[strChipId].stallId == "")) {
                return C_FT_BTNTP_CONFIRMED_RO_AVOIDCOPY;         
            } else {
                return C_FT_BTNTP_CONFIRMED_RO;
            }
            break;
        case "SubChipAdditionalWork":
            if ((gArrObjSubChip[strChipId].stallId == "0") || (gArrObjSubChip[strChipId].stallId == "")) {
                return C_FT_BTNTP_WAIT_CONFIRMEDADDWORK_AVOIDCOPY        
            } else {
                return C_FT_BTNTP_WAIT_CONFIRMEDADDWORK;
            }
            break;
        case "SubChipCompletionInspection":
            return C_FT_BTNTP_CONFIRMED_INSPECTION;
            break;
        case "SubChipCarWash":
            //洗車開始日時のチェック
            if (gArrObjSubChip[strChipId].svcStatus == "07") {
                //洗車開始待ち
                return C_FT_BTNTP_WAITING_WASH;
            } else if (gArrObjSubChip[strChipId].svcStatus == "08") {
                //洗車中
                return C_FT_BTNTP_WASHING;
            }
            break;
        case "SubChipWaitingDelivered":
            // 2013/11/29 TMEJ 丁 タブレット版SMB チーフテクニシャン機能開発 START
            //return C_FT_BTNTP_WAIT_DELIVERY;
            if (gArrObjSubChip[strChipId].carWashNeedFlg == "1") {
                return C_FT_BTNTP_WAIT_DELIVERY_WASH;
            } else {
                return C_FT_BTNTP_WAIT_DELIVERY;
            }
            // 2013/11/29 TMEJ 丁 タブレット版SMB チーフテクニシャン機能開発 END
            break;
        case "SubChipNoShow":
            return C_FT_BTNTP_NOSHOW;
            break;
        case "SubChipStop":
            return C_FT_BTNTP_STOP;
            break;
        default:
            return "";
            break;
    }
}
/**
* サブチップエリアの状態をボタンに設定する
*@param {String} buttonId ボタンID
*@param {String} Count チップ数
*@param {String} lateFlg 遅れるフラグ
* @return {なし}
*
*/
function SetSubChipAreaStatus(buttonId,Count,lateFlg) {
    if (Count != 0) {
        if (buttonId == "500") {
            Count = "";
        }
        if (lateFlg > 0) {
            SetWarningIcon(buttonId, C_FT_BTNDISP_OFF, Count);
        } else {
            SetNormalIcon(buttonId, C_FT_BTNDISP_OFF, Count);
        }
    } else {
        SetVisibilityIcon(buttonId, C_FT_BTNDISP_OFF)
    }

}
function SetSubChipBoxClose() {
    $(".SubChipAdditionalWork").fadeOut(300);
    $(".SubChipCompletionInspection").fadeOut(300);
    $(".SubChipCarWash").fadeOut(300);
    $(".SubChipWaitingDelivered").fadeOut(300);
    $(".SubChipReception").fadeOut(300);
    $(".SubChipNoShow").fadeOut(300);
    $(".SubChipStop").fadeOut(300);
    gOpenningSubBoxId = "";
    gSearchedChipId = "";
    $(".SCp").remove();
    $("#SubChipAreaActiveIndicator").removeClass("show");
    $("#SubChip_LoadingScreen").css({ "display": "none" });
    //タイマーをクリア
    commonClearTimer();
    //メインエリアのスクロール範囲を元に戻す
    ChangeMainScrollHeight(false);
}

/**
* 透明チップを生成する（サブチップ用）
*
* @return {なし}
*
*/
function drawSelectedSubChip(strSubChipId) {

    // 選択チップがない場合、戻す。
    if (gSelectedChipId == "") {
        return;
    }
    //ストール上のリレーションチップを探す
    var arrRelationChipId = FindRelationChipsFromSubChip("", gArrObjSubChip[strSubChipId].svcInId);
    // テーブルの状態をチップの選択状態に設定する
    SetSubChipAreaSelectedStatus(arrRelationChipId);
   
    if ((gArrObjSubChip[gSelectedChipId].subChipAreaId == C_NOSHOW) || (gArrObjSubChip[gSelectedChipId].subChipAreaId == C_STOP)) {
        //NoShowエリア、中断エリアチップの場合
        drawSelectedNoShowStopChip(gSelectedChipId);
    }
    // C_MOVINGCHIPIDがないの場合、新規する
    if (gMovingChipObj == null) {
        //ストール上に関係チップがない場合
        if (arrRelationChipId.length == 0) {
            // 選択したチップのデータが全部gMovingChipObjにコピーする
            gMovingChipObj = new ReserveChip(C_MOVINGCHIPID);
            gArrObjSubChip[strSubChipId].copy(gMovingChipObj);
            // gMovingChipObjのチップidをMovingChipに設定する
            gMovingChipObj.stallUseId = C_MOVINGCHIPID;
            // gMovingChipObjの予定納車日時をMovingChipに設定する
            gMovingChipObj.scheDeliDateTime = gArrObjSubChip[strSubChipId].scheDeliDateTime;
            //追加作業 開始日時、終了日時、着工指示日時をNULLに設定する
            if (gArrObjSubChip[strSubChipId].roJobSeq > 0) {
                gMovingChipObj.inspectionRsltId = 0;
                gMovingChipObj.cwRsltStartDateTime = new Date(C_DATE_DEFAULT_VALUE);
                gMovingChipObj.cwRsltEndDateTime = new Date(C_DATE_DEFAULT_VALUE);
                gMovingChipObj.carWashRsltId = "0";
                gMovingChipObj.rsltStartDateTime = new Date(C_DATE_DEFAULT_VALUE);
                gMovingChipObj.rsltEndDateTime = new Date(C_DATE_DEFAULT_VALUE);
                gMovingChipObj.rsltDeliDateTime = new Date(C_DATE_DEFAULT_VALUE);
                gMovingChipObj.stallUseStatus = "00";

                // 2014/04/22 TMEJ 丁 【IT9669】サービスタブレットDMS連携作業追加機能開発 START
                gMovingChipObj.upperDisp = "";
                gMovingChipObj.lowerDisp = "";
                gMovingChipObj.svcClassName = "";
                gMovingChipObj.svcClassNameEng = "";
                // 2014/04/22 TMEJ 丁 【IT9669】サービスタブレットDMS連携作業追加機能開発 END
            }
            gMovingChipObj.createChip(C_CHIPTYPE_STALL_MOVING);
            // 新規の場合、MovingChipの幅がC_CELL_WIDTHより小さいなので、MovingChipの幅を元のチップの幅に設定する
            $("#" + C_MOVINGCHIPID).css({ "left": gSubMovingChipLeft, "top": gSubMovingChipTop});
            if (gArrObjSubChip[strSubChipId].roJobSeq > 0) {
                $("#" + C_MOVINGCHIPID).css("width", C_SubChipWidth - 1);
            } else if (gArrObjSubChip[strSubChipId].scheWorkTime != 0) {
                if (gArrObjSubChip[strSubChipId].scheWorkTime < 5) {
                    $("#" + C_MOVINGCHIPID).css("width", C_CELL_WIDTH / 15 * gResizeInterval - 1);
                } else {
                    $("#" + C_MOVINGCHIPID).css("width", Math.round((gArrObjSubChip[strSubChipId].scheWorkTime * C_CELL_WIDTH / 15))-1);
                }
            } else {
                $("#" + C_MOVINGCHIPID).css("width", (C_SubChipWidth*3/2) - 1);
            }
            $("#" + strSubChipId).css("z-index", 5);
        } else {
            //ストール上に関係チップがある場合
            // 選択したチップのデータをgMovingChipObjにコピーする
            gMovingChipObj = new ReserveChip(C_MOVINGCHIPID);
            gArrObjSubChip[strSubChipId].copy(gMovingChipObj);
            
            //追加作業 開始日時、終了日時、着工指示日時をNULLに設定する
           if (gArrObjSubChip[strSubChipId].roJobSeq > 0) {
                gMovingChipObj.inspectionRsltId = 0;
                gMovingChipObj.cwRsltStartDateTime = new Date(C_DATE_DEFAULT_VALUE);
                gMovingChipObj.cwRsltEndDateTime = new Date(C_DATE_DEFAULT_VALUE);
                gMovingChipObj.carWashRsltId = "0";
                gMovingChipObj.rsltStartDateTime = new Date(C_DATE_DEFAULT_VALUE);
                gMovingChipObj.rsltEndDateTime = new Date(C_DATE_DEFAULT_VALUE);
                gMovingChipObj.rsltDeliDateTime = new Date(C_DATE_DEFAULT_VALUE);
                gMovingChipObj.stallUseStatus = "00";

                // 2014/04/22 TMEJ 丁 【IT9669】サービスタブレットDMS連携作業追加機能開発 START
                gMovingChipObj.upperDisp = "";
                gMovingChipObj.lowerDisp = "";
                gMovingChipObj.svcClassName = "";
                gMovingChipObj.svcClassNameEng = "";
                // 2014/04/22 TMEJ 丁 【IT9669】サービスタブレットDMS連携作業追加機能開発 END
            }
            // gMovingChipObjのチップid、開始日時、終了日時をMovingChipに設定する
            gMovingChipObj.stallUseId = C_MOVINGCHIPID;
            // gMovingChipObjの予定納車日時をMovingChipに設定する
            gMovingChipObj.scheDeliDateTime = gArrObjSubChip[strSubChipId].scheDeliDateTime;
            gMovingChipObj.createChip(C_CHIPTYPE_STALL_MOVING);
            // 新規の場合、MovingChipの幅がC_CELL_WIDTHより小さいなので、MovingChipの幅を元のチップの幅に設定する
            //var nWidth = parseInt((gMovingChipObj.displayEndDate.getTime() - gMovingChipObj.displayStartDate.getTime()) / 1000 / 60 * (C_CELL_WIDTH / 15)) - 1;
            $("#" + C_MOVINGCHIPID).css({ "left": gSubMovingChipLeft, "top": gSubMovingChipTop });
            if (gArrObjSubChip[strSubChipId].roJobSeq != 0) {
                $("#" + C_MOVINGCHIPID).css("width", C_SubChipWidth - 1);
            } else {
                $("#" + C_MOVINGCHIPID).css("width", Math.round((gArrObjSubChip[strSubChipId].scheWorkTime * C_CELL_WIDTH / 15))-1);
            }
        }
        // 幅によって、チップに表示内容を調整する
        AdjustChipItemByWidth(C_MOVINGCHIPID);
        // Movingチップのりサイズをbindする
        BindChipResize(C_MOVINGCHIPID, 0, 0);
        // Movingチップの爪以外の部分が半透明、見えない
        $("#" + C_MOVINGCHIPID + " .CpInner").css({"opacity": C_OPACITY_TRANSPARENT,"visibility":"hidden"});
        // Movingチップのz-indexを追加する
        ChangeChipZIndex(C_MOVINGCHIPID, "MovingChipZIndex");
        // Movingチップのタップイベントのbind
        BindMovingSubChipTapEvent();
    }

}

/**
* 透明チップを生成する（NoShow、中断チップ）
*
* @return {なし}
*
*/
function drawSelectedNoShowStopChip(strSubChipId) {
    // C_MOVINGCHIPIDがないの場合、新規する
    if (gMovingChipObj == null) {
            // 選択したチップのデータが全部gMovingChipObjにコピーする
        gMovingChipObj = new ReserveChip(C_MOVINGCHIPID);
            gArrObjSubChip[strSubChipId].copy(gMovingChipObj);
            // gMovingChipObjのチップidをMovingChipに設定する
            gMovingChipObj.stallUseId = C_MOVINGCHIPID;
            if (gArrObjSubChip[strSubChipId].subChipAreaId == C_STOP) {
                gMovingChipObj.stallUseStatus = C_STALLUSE_STATUS_STARTWAIT;
                gMovingChipObj.rsltStartDateTime = C_DATE_DEFAULT_VALUE;
                gMovingChipObj.prmsEndDateTime = C_DATE_DEFAULT_VALUE;
                gMovingChipObj.rsltEndDateTime = C_DATE_DEFAULT_VALUE;
                gMovingChipObj.rsltWorkTime = C_NUM_DEFAULT_VALUE;
                gMovingChipObj.stopReasonType = C_STR_DEFAULT_VALUE;
                var svcStatus = C_SVCSTATUS_STARTWAIT;
                for (var keyString in gArrObjSubChip) {
                    if (CheckgArrObjSubChip(keyString) == false) { continue };
                    if (gArrObjSubChip[keyString].KEY == strSubChipId) { continue };
                    if ((gArrObjSubChip[keyString].svcInId == gArrObjSubChip[strSubChipId].svcInId) && ((gArrObjSubChip[keyString].stallUseStatus == C_STALLUSE_STATUS_FINISH) || (gArrObjSubChip[keyString].stallUseStatus == C_STALLUSE_STATUS_STOP))) {
                        svcStatus = C_SVCSTATUS_NEXTSTARTWAIT;
                        break;
                    }
                }
                gMovingChipObj.svcStatus = svcStatus;
            } 
            gMovingChipObj.createChip(C_CHIPTYPE_STALL_MOVING);
            // 新規の場合、MovingChipの幅がC_CELL_WIDTHより小さいなので、MovingChipの幅を元のチップの幅に設定する
            var dtScheWrokTmie = gArrObjSubChip[strSubChipId].scheWorkTime;
            $("#" + C_MOVINGCHIPID).css({ "left": gSubMovingChipLeft, "top": gSubMovingChipTop });
            if (gArrObjSubChip[strSubChipId].subChipAreaId == C_STOP) {
                var relocationWorkTime=gArrObjSubChip[strSubChipId].relocationWorkTime;
                $("#" + C_MOVINGCHIPID).css({"width": GetSubChipWidth(relocationWorkTime * C_CELL_WIDTH / 15) - 1,"z-index":100});
            } else {
                $("#" + C_MOVINGCHIPID).css({ "width": GetSubChipWidth(dtScheWrokTmie * C_CELL_WIDTH / 15) - 1, "z-index": 100 });
            }
        }
        // 幅によって、チップに表示内容を調整する
        AdjustChipItemByWidth(C_MOVINGCHIPID);
        // Movingチップのりサイズをbindする
        BindChipResize(C_MOVINGCHIPID, 0, 0);
        // Movingチップの爪以外の部分が半透明、見えない
        $("#" + C_MOVINGCHIPID + " .CpInner").css({"opacity": C_OPACITY_TRANSPARENT,"visibility": "hidden"});
        // Movingチップのz-indexを追加する
        ChangeChipZIndex(C_MOVINGCHIPID, "MovingChipZIndex");
        // Movingチップのタップイベントのbind
        BindMovingSubChipTapEvent();
    }

    /**
    * Movingチップのタップイベントのbind(サブチップ)
    * @return {なし}
    */
    function BindMovingSubChipTapEvent() {
        $("#" + C_MOVINGCHIPID).bind("chipTap", function (e) {
            // ポップアップウィンドウが表示中、何もしない
            if (GetDisplayPopupWindow()) {
                return;
            }

            $("#" + gSelectedChipId + " .Front").removeClass("TapBlueBack");
            $("#" + C_MOVINGCHIPID + " .Front").removeClass("TapBlueBack").addClass("BlackBack");
            setTimeout(function () {
                // Movingチップのタップ関数を走る(サブチップ)
                MovingSubChipTap();
            }, 0);

            // チップにタップすると、 $("#ulChipAreaBack_lineBox .TbRow").bind("chipTap")を走らないように
            gCanTbRowTapFlg = false;
        })
   .bind(C_TOUCH_START, function (e) {
       gOnTouchingFlg = true;
       // Movingチップがピッタリ元のチップの上にある時(非表示の時)
       if ($("#" + C_MOVINGCHIPID + " .CpInner").css("visibility") == "hidden") {
           // 元のチップに青色を表示する
           $("#" + gSelectedChipId + " .Front").addClass("TapBlueBack");
       } else {
           // Movingチップが表示される時、Movingチップが一瞬で青色を表示する
           $("#" + C_MOVINGCHIPID + " .Front").removeClass("BlackBack").addClass("TapBlueBack");
       }
   })
    .bind(C_TOUCH_END, function (e) {
        $("#" + gSelectedChipId + " .Front").removeClass("TapBlueBack");
        $("#" + C_MOVINGCHIPID + " .Front").removeClass("TapBlueBack").addClass("BlackBack");
        gOnTouchingFlg = false;
    })
    .bind(C_TOUCH_MOVE, function (e) {
        $("#" + gSelectedChipId + " .Front").removeClass("TapBlueBack");
        $("#" + C_MOVINGCHIPID + " .Front").removeClass("TapBlueBack").addClass("BlackBack");
        gOnTouchingFlg = false;
    });
}

/**
* Movingチップのタップ
* @return {なし}
*/
function MovingSubChipTap() {
    var strSubChipId;
    if (gSelectedChipId != C_OTHERDTCHIPID) {
        // 移動する前の重複チップid全部バックする
        arrBackDuplicateChipsId = GetMovingCpDuplicateSubChips(gSelectedChipId);
        //2013/11/29 TMEJ 丁 タブレット版SMB チーフテクニシャン機能開発 START                         
//        // 受チップ且つ実績チップ
//        if ((IsDefaultDate(gArrObjSubChip[gSelectedChipId].rsltEndDateTime) == false)&& (gArrObjSubChip[gSelectedChipId].subChipAreaId== C_RECEPTION)){
//            strSubChipId = gSelectedChipId;
//            // 選択したチップを解放する
//            SetTableUnSelectedStatus();
//            // チップ選択状態を解除する
//            SetChipUnSelectedStatus();
//            if (gOpenningSubBoxId != "") {
//                FooterIconReplace(gOpenningSubBoxId);
//            }
//            return;
//        }
        //2013/11/29 TMEJ 丁 タブレット版SMB チーフテクニシャン機能開発 END
    }
    // 置いた位置をチェックする
    if (CheckChipPos(C_MOVINGCHIPID) == false) {
        var strSubChipId = gSelectedChipId;
        //エラーメッセージ「他のチップと配置時間が重複します。」を表示する
        ShowSC3240301Msg(903);
        // 選択したチップを解放する
        SetTableUnSelectedStatus();
        // チップ選択状態を解除する
        SetChipUnSelectedStatus();
        //サブチップアイコンを選択状態にする
        if (gOpenningSubBoxId != "") {
            FooterIconReplace(gOpenningSubBoxId);
        }
        return;
    }
    // 重複チップがあれば、リサイズを失敗する
    var arrDuplChipId = GetMovingCpDuplicateSubChips();
    // 重複の場合
    if (arrDuplChipId.length > 0) {
        strSubChipId = gSelectedChipId;
        // エラーメッセージを表示する
        ShowSC3240301Msg(903);
        // 選択したチップを解放する
        SetTableUnSelectedStatus();
        // チップ選択状態を解除する
        SetChipUnSelectedStatus();
        //サブチップアイコンを選択状態にする
        if (gOpenningSubBoxId != "") {
            FooterIconReplace(gOpenningSubBoxId);
        }
        return;
    }
    //2013/11/29 TMEJ 丁 タブレット版SMB チーフテクニシャン機能開発 START
//    //既にRO紐付いているチップは入れ子にできません
//    if ((gArrObjSubChip[gSelectedChipId].parentsRoJobSeq != -1) && (gArrObjSubChip[gSelectedChipId].parentsRoJobSeq != gArrObjSubChip[gSelectedChipId].roJobSeq) && (gArrObjSubChip[gSelectedChipId].roJobSeq==0)) {
//        strSubChipId = gSelectedChipId;
//        ShowSC3240301Msg(910);
//        // 選択したチップを解放する
//        SetTableUnSelectedStatus();
//        // チップ選択状態を解除する
//        SetChipUnSelectedStatus();
//        //サブチップアイコンを選択状態にする
//        if (gOpenningSubBoxId != "") {
//            FooterIconReplace(gOpenningSubBoxId);
//        }
//        return;
//    }
    //2013/11/29 TMEJ 丁 タブレット版SMB チーフテクニシャン機能開発 END

    // 2019/12/06 NSK 皆川 (トライ店システム評価)整備作業における休憩時間取得判定向上検証 START
    // 休憩を自動判定しない場合
    if ($("#hidRestAutoJudgeFlg").val() != "1") {
        // 2019/12/06 NSK 皆川 (トライ店システム評価)整備作業における休憩時間取得判定向上検証 END

        // 移動先が使用不可、休憩エリアがあるかどうかを判断する
        var arrRestTime = GetRestTimeInServiceTime(C_MOVINGCHIPID);
        // 重複休憩エリアがあれば
        if (arrRestTime.length > 0) {
            // 一番左の休憩エリアの真中で休憩ウィンドウを表示する
            ShowRestTimeDialog(arrRestTime[0][0], C_ACTION_SUBCHIPMOVE);
            return;
        }

        // 2019/12/06 NSK 皆川 (トライ店システム評価)整備作業における休憩時間取得判定向上検証 START
    }
    // 2019/12/06 NSK 皆川 (トライ店システム評価)整備作業における休憩時間取得判定向上検証 END

    MoveSubChip(null);
}

/**
* Movingチップのタップ(サブチップ)
* @return {なし}
*/
function MoveSubChip(nRestFlg) {
    // MovingChipのleft、幅を取得する
    var nMovingChipLeft = $("#" + C_MOVINGCHIPID).position().left;

    //2013/11/29 TMEJ 丁 タブレット版SMB チーフテクニシャン機能開発 START
    //var nTapLeft = nMovingChipLeft;
    //2013/11/29 TMEJ 丁 タブレット版SMB チーフテクニシャン機能開発 END
    var nMovingChipWidth = $("#" + C_MOVINGCHIPID).width();

    //2015/02/09 TMEJ 範 DMS連携版サービスタブレット SMB納車予定時刻通知機能開発 START
    //一時保存用チップオブジェクト
    var tempChipObject = new ReserveSubChip(gSelectedChipId);
    gArrObjSubChip[gSelectedChipId].copy(tempChipObject);

    //RO番号がある場合(RO顧客承認後の場合)
    if (tempChipObject.roNum.Trim() != "") {

        //移動後の終了日時を取得
        var afterMovingEndTime = GetTimeByXPos(nMovingChipLeft + nMovingChipWidth);

        //表示終了日時を一時保存用チップオブジェクトに設定
        tempChipObject.setDisplayEndDate(afterMovingEndTime);

        //移動後のチップが遅れ、または遅れ見込の場合
        if (IsDelayDelivery(tempChipObject)) {

            // 確認ボックスを表示
            if (!ConfirmSC3240101Msg(935)) {

                //一時保存用チップオブジェクト削除
                tempChipObject = null;

                //チップの移動を確定しないために、以降の処理を行わない
                return;

            }
        }
    }

    //一時保存用チップオブジェクト削除
    tempChipObject = null;

    //2015/02/09 TMEJ 範 DMS連携版サービスタブレット SMB納車予定時刻通知機能開発 END

    var strSubChipId;
    var dtShowDate = new Date($("#hidShowDate").val());  //当ページの日付
    //ストール上のリレーションチップを探す
    var arrRelationChipId = FindRelationChipsFromSubChip("", gArrObjSubChip[gSelectedChipId].svcInId);
    if (gArrObjSubChip[gSelectedChipId].subChipAreaId != C_RECEPTION) {
        strSubChipId = gSelectedChipId;
        //2013/11/29 TMEJ 丁 タブレット版SMB チーフテクニシャン機能開発 START
        //} else if ((arrRelationChipId.length > 0) && (gArrObjSubChip[gSelectedChipId].roJobSeq == 0)) {

    // 2019/08/09 NSK 鈴木 DLR-002 子チップを着工指示した時にエラー発生（親チップ着工指示でエラー発生を修正） START
    //    //実績がある親ROは追加作業扱い
    // } else if ((arrRelationChipId.length > 0) && (gArrObjSubChip[gSelectedChipId].roJobSeq == 0) &&
    // ((IsDefaultDate(gArrObjSubChip[gSelectedChipId].rsltStartDateTime) == true) && (IsDefaultDate(gArrObjSubChip[gSelectedChipId].rsltEndDateTime) == true))) {
    //    //2013/11/29 TMEJ 丁 タブレット版SMB チーフテクニシャン機能開発 END
    // //予約ありのROの場合尚且予約がストール上に
    //    strSubChipId = arrRelationChipId[0][0];

        // サブチップが受付サブボックス上にある場合、
        // かつ、ストール上に関連チップがある場合、
        // かつ、サブチップが親チップ（RO連番が0）の場合、
        // かつ、サブチップが作業未開始である（実績開始日時がデフォルト値ではない）場合、
        // リレーションチップ配列から作業内容IDをマッチングキーとしてチップIDを取得する。
        // キーマッチングしなかった場合は、サブチップIDを使用する。
    } else if ((0 < arrRelationChipId.length) && (gArrObjSubChip[gSelectedChipId].roJobSeq == 0)
        && (IsDefaultDate(gArrObjSubChip[gSelectedChipId].rsltStartDateTime))) {
        strSubChipId =
            GetChipIdFromRelationChips(
                arrRelationChipId, gArrObjSubChip[gSelectedChipId].jobDtlId, gSelectedChipId);
    // 2019/08/09 NSK 鈴木 DLR-002 子チップを着工指示した時にエラー発生（親チップ着工指示でエラー発生を修正） END

    } else {
        strSubChipId = gSelectedChipId;
    }

    // 2013/11/29 TMEJ 張 タブレット版SMB チーフテクニシャン機能開発 START
    // 部品準備完了フラグをバックアップ
    var strPartFlg = gArrObjSubChip[gSelectedChipId].partsFlg;
    // 2013/11/29 TMEJ 張 タブレット版SMB チーフテクニシャン機能開発 END

    var nRestTimeWidth = 0;

    // 2019/12/06 NSK 皆川 (トライ店システム評価)整備作業における休憩時間取得判定向上検証 START
    // 休憩を自動判定しない場合
    if ($("#hidRestAutoJudgeFlg").val() != "1") {
        // 2019/12/06 NSK 皆川 (トライ店システム評価)整備作業における休憩時間取得判定向上検証 END

        // 休憩フラグを設定した場合
        if (nRestFlg != null) {
            // 休憩を取得する場合
            if (nRestFlg == 1) {
                // 移動チップと重複の休憩エリアIDを取得する
                var arrRestTime = GetRestTimeInServiceTime(C_MOVINGCHIPID);

                // 休憩エリアがチップの左にある且つ重複時、チップが休憩エリアの右に移動する
                var nChipLeft = GetLeftByRestTime(C_MOVINGCHIPID, arrRestTime)
                if (nChipLeft != nMovingChipLeft) {
                    nMovingChipLeft = nChipLeft;
                    $("#" + C_MOVINGCHIPID).css("left", nMovingChipLeft);
                }

                // チップの幅が休憩エリアの幅をプラスする
                nRestTimeWidth = GetWidthByRestTime(C_MOVINGCHIPID);

                var dtMovedDate = GetTimeByXPos(nChipLeft - 1);
                if (dtMovedDate - gEndWorkTime >= 0) {
                    //「営業終了時間({0}:{1})以内に配置してください。」ってメッセージが表示される
                    if (gSC3240301WordIni != null) {
                        if (gSC3240301WordIni[908] != null) {
                            var strMsg = gSC3240301WordIni[908];
                            strMsg = strMsg.replace("{0}", add_zero(gEndWorkTime.getHours()));
                            strMsg = strMsg.replace("{1}", add_zero(gEndWorkTime.getMinutes()));
                            alert(strMsg);
                        }
                    }
                    strSubChipId = gSelectedChipId;
                    // 選択したチップを解放する
                    SetTableUnSelectedStatus();
                    // チップ選択状態を解除する
                    SetChipUnSelectedStatus();
                    //サブチップアイコンを選択状態にする
                    if (gOpenningSubBoxId != "") {
                        FooterIconReplace(gOpenningSubBoxId);
                    }
                    return;
                }
            }
        }

        // 2019/12/06 NSK 皆川 (トライ店システム評価)整備作業における休憩時間取得判定向上検証 START
    } else {
        // 移動先のチップの開始日時が当日である場合、作業時間に休憩時間を足す処理を行う
        // ※日跨ぎの翌日チップを選択し休憩変更ボタンをタップするとチップ幅の表示が一瞬おかしくなる不具合の対応で分岐を追加
        //   チップが日跨ぎの場合はクライアント側ではチップ幅の計算をしない（コールバック後サーバー側で計算した幅で再描画）
        if (IsTodayStartDate(strSubChipId)) {
            // チップの幅が休憩エリアの幅をプラスする
            nRestTimeWidth = GetWidthByRestTime(C_MOVINGCHIPID);
        }

        var dtMovedDate = GetTimeByXPos(nMovingChipLeft - 1);

        if (dtMovedDate - gEndWorkTime >= 0) {
            //「営業終了時間({0}:{1})以内に配置してください。」ってメッセージが表示される
            if (gSC3240301WordIni != null) {
                if (gSC3240301WordIni[908] != null) {
                    var strMsg = gSC3240301WordIni[908];
                    strMsg = strMsg.replace("{0}", add_zero(gEndWorkTime.getHours()));
                    strMsg = strMsg.replace("{1}", add_zero(gEndWorkTime.getMinutes()));
                    alert(strMsg);
                }
            }
            strSubChipId = gSelectedChipId;
            // 選択したチップを解放する
            SetTableUnSelectedStatus();
            // チップ選択状態を解除する
            SetChipUnSelectedStatus();
            //サブチップアイコンを選択状態にする
            if (gOpenningSubBoxId != "") {
                FooterIconReplace(gOpenningSubBoxId);
            }
            return;
        }
    }
    // 2019/12/06 NSK 皆川 (トライ店システム評価)整備作業における休憩時間取得判定向上検証 END

    // 行移動
    var nRow = GetRowNoByChipId(C_MOVINGCHIPID);
    $(".Row" + nRow).append($("#" + strSubChipId));

    //移動チップのプロパティを設定する
    //予定作業時間
    var workTimeMinutes = (GetTimeByXPos((nMovingChipLeft+ nMovingChipWidth)) - GetTimeByXPos(nMovingChipLeft-1)) / 1000 / 60;
    //ストールID
    var strStallId = $(".stallNo" + nRow)[0].id
    //ストールID
    strStallId = strStallId.substring(8, strStallId.length);
    //予定開始時間
    //2013/11/29 TMEJ 丁 タブレット版SMB チーフテクニシャン機能開発 START
    //var startTime = GetTimeByXPos(nTapLeft - 1);
    var startTime = GetTimeByXPos(nMovingChipLeft - 1);
    //2013/11/29 TMEJ 丁 タブレット版SMB チーフテクニシャン機能開発 END

    //MOVINGチップにパラメタを退避
    gMovingSubChipObj = new MovingSubChip();
    gMovingSubChipObj.setStallId(strStallId);             //ストールID
    gMovingSubChipObj.setScheWorkTime(workTimeMinutes);        // 予定開始日時
    gMovingSubChipObj.setStartDateTime(startTime);     // 予定作業時間

    // 2015/09/26 NSK  秋田谷 開発TR-SVT-TMT-20151008-001 SMB画面の表示が遅い START
    // ";"とコメントの間に全角スペースが存在したため削除
    gMovingSubChipObj.selectedChipId = gSelectedChipId; //選択チップ
    // 2015/09/26 NSK  秋田谷 開発TR-SVT-TMT-20151008-001 SMB画面の表示が遅い END

    // 2013/11/29 TMEJ 張 タブレット版SMB チーフテクニシャン機能開発 START
    // バックアップした部品準備完了フラグをMovingチップに設定する
    if (strPartFlg == "1") {
        gMovingSubChipObj.partsFlg = strPartFlg;
    }
    // 2013/11/29 TMEJ 張 タブレット版SMB チーフテクニシャン機能開発 END

    if (strSubChipId == gSelectedChipId) {
        gArrObjSubChip[strSubChipId].setScheStartDateTime(startTime);           // 予定開始日時
        gArrObjSubChip[strSubChipId].setScheEndDateTime(GetTimeByXPos(nMovingChipLeft + nMovingChipWidth + nRestTimeWidth));           // 予定終了日時
        gArrObjSubChip[strSubChipId].setScheWorkTime(workTimeMinutes);           // 予定作業時間
        gArrObjSubChip[strSubChipId].setStallId(strStallId);   //ストールID

        // 2013/11/29 TMEJ 張 タブレット版SMB チーフテクニシャン機能開発 START
        // バックアップした部品準備完了フラグをMovingチップに設定する
        if (strPartFlg == "1") {
            gArrObjSubChip[strSubChipId].setPartsFlg(strPartFlg);
        }
        // 2013/11/29 TMEJ 張 タブレット版SMB チーフテクニシャン機能開発 END

    } else if (strSubChipId != gSelectedChipId) {

        //2013/11/29 TMEJ 丁 タブレット版SMB チーフテクニシャン機能開発 START
        //gArrObjChip[strSubChipId].setScheStartDateTime(GetTimeByXPos(nTapLeft - 1));           // 予定開始日時
        gArrObjChip[strSubChipId].setScheStartDateTime(GetTimeByXPos(nMovingChipLeft - 1));           // 予定開始日時      
        //2013/11/29 TMEJ 丁 タブレット版SMB チーフテクニシャン機能開発 END

        gArrObjChip[strSubChipId].setScheEndDateTime(GetTimeByXPos(nMovingChipLeft + nMovingChipWidth + nRestTimeWidth));           // 予定終了日時
        gArrObjChip[strSubChipId].setScheWorkTime(workTimeMinutes);           // 予定作業時間
        gArrObjChip[strSubChipId].setStallId(strStallId);   //ストールID

        // 2013/11/29 TMEJ 張 タブレット版SMB チーフテクニシャン機能開発 START
        // バックアップした部品準備完了フラグをMovingチップに設定する
        if (strPartFlg == "1") {
            gArrObjChip[strSubChipId].setPartsFlg(strPartFlg);
        }
        // 2013/11/29 TMEJ 張 タブレット版SMB チーフテクニシャン機能開発 END
    }
    //movingチップの代わりに本チップを目的座標に移動する
    $("#" + strSubChipId).css({ "left": nMovingChipLeft, "width": nMovingChipWidth + nRestTimeWidth, "top": 1 });

    // 2014/04/22 TMEJ 丁 【IT9669】サービスタブレットDMS連携作業追加機能開発 START
    // 追加作業の場合
    //2013/11/29 TMEJ 丁 タブレット版SMB チーフテクニシャン機能開発 START 
    //  if ((gArrObjSubChip[gSelectedChipId].roJobSeq > 0) 
    // && (gArrObjSubChip[gSelectedChipId].subChipAreaId == C_RECEPTION)){
    if (((gArrObjSubChip[gSelectedChipId].roJobSeq > 0) || ((IsDefaultDate(gArrObjSubChip[gSelectedChipId].rsltStartDateTime) == false) || (IsDefaultDate(gArrObjSubChip[gSelectedChipId].rsltEndDateTime) == false)))
        && (gArrObjSubChip[gSelectedChipId].subChipAreaId == C_RECEPTION)) {
        //2013/11/29 TMEJ 丁 タブレット版SMB チーフテクニシャン機能開発 END
        $("#" + strSubChipId + " .infoBox")[0].innerHTML = "";
        //2013/11/29 TMEJ 丁 タブレット版SMB チーフテクニシャン機能開発 START
        gArrObjSubChip[strSubChipId].stallUseId = 0;
        //2013/11/29 TMEJ 丁 タブレット版SMB チーフテクニシャン機能開発 END
    }
    // 2014/04/22 TMEJ 丁 【IT9669】サービスタブレットDMS連携作業追加機能開発 END

    //本チップのSCpクラスを削除する
    $("#" + strSubChipId).removeClass("SCp");

    //本チップにMCpクラスを追加する
    if ($("#" + strSubChipId).hasClass("MCp") == false) {
        $("#" + strSubChipId).addClass("MCp");
    }
    // テーブルの状態をチップの未選択状態に設定する
    SetTableUnSelectedStatus();
    // チップ選択状態を解除する
    SetChipUnSelectedStatus();
    if (gOpenningSubBoxId != "") {
        FooterIconReplace(gOpenningSubBoxId);
    }

    // 幅によって、チップに表示内容を調整する
    AdjustChipItemByWidth(strSubChipId);

    //サブチップ更新準備
    GetReadyMovingUpdate(strSubChipId, nRestFlg);

    //サブチップをストールに移動する場合
    if (CheckgArrObjSubChip(strSubChipId)) {
        //サブチップの影を消す
        $("#" + strSubChipId).removeClass("SelectedChipShadow");
        //サブチップをストールチップに変身する
        if (gArrObjSubChip[strSubChipId].subChipAreaId == C_NOSHOW) {
            var strStallChipId = gArrObjSubChip[strSubChipId].stallUseId;
            $("#" + strSubChipId).attr("id", strStallChipId);
            gArrObjChip[strStallChipId] = new ReserveChip(strStallChipId);
            gArrObjSubChip[strSubChipId].copy(gArrObjChip[strStallChipId]);
            // チップをタップする時のイベントを解除する
            $("#" + strStallChipId).unbind();
            // チップをタップする時のイベントを登録
            BindChipClickEvent(strStallChipId);
            //チップ連続移動するためRowロックバージョンプラス1
            AddRowLockVersionOne(strStallChipId);
        } else {
            //サブチップの影を消す
            $("#" + strSubChipId).removeClass("SelectedChipShadow");
            // チップをタップする時のイベントを解除する
            $("#" + strSubChipId).unbind();
            //新規チップの場合は仮チップIDを設定する
            $("#" + strSubChipId).attr("id", C_TEMPCHIPID);
            //チップ連続移動するためRowロックバージョンプラス1
            AddRowLockVersionOneBySvcInId(gArrObjSubChip[strSubChipId].svcInId);
        }

    } else {
        //ストールチップの場合
        AddRowLockVersionOne(strSubChipId);
    }
}

/**
* チップ移動DB更新準備
* @param {String} strSubChipId チップID
* 
*/
function GetReadyMovingUpdate(strSubChipId, nRestFlg) {
    $("#SubChipAreaActiveIndicator").addClass("show");
    $("#SubChip_LoadingScreen").css({ "display": "block" });

    //2015/07/30 TMEJ 明瀬 アクティビティインジケータが消えない問題 START

    //リフレッシュタイマーセット
    commonRefreshTimerTabletSMB(ReDisplay);

    //2015/07/30 TMEJ 明瀬 アクティビティインジケータが消えない問題 END

    var subChipAreaId;
    if (CheckgArrObjSubChip(gMovingSubChipObj.selectedChipId) == true) {
        subChipAreaId = gArrObjSubChip[gMovingSubChipObj.selectedChipId].subChipAreaId;
    } else {
        var strRelationSubChip = GetRelationSubChipId(strSubChipId);
        if (CheckgArrObjSubChip(strRelationSubChip) == true) {
            subChipAreaId = gArrObjSubChip[strRelationSubChip].subChipAreaId;
         }
    }

    //2015/07/30 TMEJ 明瀬 アクティビティインジケータが消えない問題 START

    //commonRefreshTimerTabletSMB(ReDisplay);

    //2015/07/30 TMEJ 明瀬 アクティビティインジケータが消えない問題 END

    // 2019/06/18 NSK 皆川 (トライ店システム評価)次世代サービスオペレーション効率化に向けた、業務適合性検証 START
    // ※NoShow以外のサブエリアは休憩自動判定処理を入れる前からタップ不可にしているため
    // 　休憩自動判定しない場合でもここのタップ不可処理だけは残しておく
    // NoShowから再配置の場合
    if (CheckgArrObjSubChip(strSubChipId) && gArrObjSubChip[strSubChipId].subChipAreaId == C_NOSHOW) {
    // 最新の情報取得までタップ不可にする
        $("#" + strSubChipId).data(C_DATA_CHIPTAP_FLG, false);
    }
    // 2019/06/18 NSK 皆川 (トライ店システム評価)次世代サービスオペレーション効率化に向けた、業務適合性検証 END

    //コールバックパラメータ作成
    var prms = CreateCallBackMovingParam(strSubChipId, nRestFlg);
    //コールバック開始
    DoCallBack(C_CALLBACK_WND301, prms, MovingAfterCallBackUpData, "GetReadyMovingUpdate");
    //gCallbackSC3240301.doCallback(prms, MovingAfterCallBackUpData);
}

/**
* NoShow、中断、受付チップ配置のDB更新準備
* @param {String} strChipId チップID
* 
*/
function CreateCallBackMovingParam(strSubChipId, nRestFlg) {
    var strButtonID;
    var strServiceInId;
    var strJobDtlId;
    var strStalluseId;
    var strStallId;
    var strScheStartDatetime;
    var strScheEndDatetime;
    var strScheWorkTime;
    var strRestFlg;
    var strRowLockVersion;
    var dtShowDate = $("#hidShowDate").val();  //当ページの日付
    var strMainteCd;
    var strRoJobSeq;
    var strRelationSubChip;
    var strSubChipKey;
    var strScheDeliDatetime;
    var strPickDeliType;
    var strScheSvcinDateTime;
    var strRoNum;
    // 2013/11/29 TMEJ 張 タブレット版SMB チーフテクニシャン機能開発 START
    var strInspectionNeedFlg;
    // 2013/11/29 TMEJ 張 タブレット版SMB チーフテクニシャン機能開発 END
    //2017/11/12 NSK  荒川 REQ-SVT-TMT-20160906-003 計画済みチップをストールから簡単に外せるようにする START
    var strTempFlg;
    //2017/11/12 NSK  荒川 REQ-SVT-TMT-20160906-003 計画済みチップをストールから簡単に外せるようにする END

    if (nRestFlg==null) {
        strRestFlg = "1";
    } else {
        strRestFlg = nRestFlg;
    }

    if (CheckgArrObjSubChip(gMovingSubChipObj.selectedChipId) == true) {
        strSubChipKey = gArrObjSubChip[gMovingSubChipObj.selectedChipId].KEY;
        if (gArrObjSubChip[gMovingSubChipObj.selectedChipId].subChipAreaId == C_NOSHOW) {
            strButtonID = C_NOSHOW_MOVING;
        } else if (gArrObjSubChip[gMovingSubChipObj.selectedChipId].subChipAreaId == C_STOP) {
            strButtonID = C_STOP_MOVING;
        } else if (gArrObjSubChip[gMovingSubChipObj.selectedChipId].subChipAreaId == C_RECEPTION) {
            strButtonID = C_RECEPTION_ATTACHMENT;
        }
    } else {
            strRelationSubChip = GetRelationSubChipId(strSubChipId);
            strButtonID = C_RECEPTION_ATTACHMENT;
            strSubChipKey = gArrObjSubChip[strRelationSubChip].KEY;
        }
        if (strButtonID != C_RECEPTION_ATTACHMENT) {
            strServiceInId = gArrObjSubChip[strSubChipId].svcInId;
            strJobDtlId = gArrObjSubChip[strSubChipId].jobDtlId;
            strStalluseId = gArrObjSubChip[strSubChipId].stallUseId;
            strStallId = gArrObjSubChip[strSubChipId].stallId;
            strScheStartDatetime = gArrObjSubChip[strSubChipId].scheStartDateTime;
            strScheEndDatetime = gArrObjSubChip[strSubChipId].scheEndDateTime;
            strScheWorkTime = gArrObjSubChip[strSubChipId].scheWorkTime;
            strRowLockVersion = gArrObjSubChip[strSubChipId].rowLockVersion;
            strPickDeliType = gArrObjSubChip[strSubChipId].pickDeliType;
            strScheSvcinDateTime = gArrObjSubChip[strSubChipId].scheSvcInDateTime;
            strRoNum = gArrObjSubChip[strSubChipId].roNum;
        } else if (CheckgArrObjSubChip(strSubChipId) == false) {
            //目標チップの情報
            strServiceInId = gArrObjChip[strSubChipId].svcInId;
            strJobDtlId = gArrObjChip[strSubChipId].jobDtlId;
            strStalluseId = gArrObjChip[strSubChipId].stallUseId;
            strStallId = gArrObjChip[strSubChipId].stallId;
            strScheStartDatetime = gArrObjChip[strSubChipId].scheStartDateTime;
            strScheEndDatetime = gArrObjChip[strSubChipId].scheEndDateTime;
            strScheWorkTime = gArrObjChip[strSubChipId].scheWorkTime;
            strRowLockVersion = gArrObjChip[strSubChipId].rowLockVersion;
            strPickDeliType = gArrObjChip[strSubChipId].pickDeliType;
            strScheSvcinDateTime = gArrObjChip[strSubChipId].scheSvcInDateTime;
            strRoNum = gArrObjChip[strSubChipId].roNum;
            // 2013/11/29 TMEJ 張 タブレット版SMB チーフテクニシャン機能開発 START
            strInspectionNeedFlg = gArrObjChip[strSubChipId].inspectionNeedFlg;
            // 2013/11/29 TMEJ 張 タブレット版SMB チーフテクニシャン機能開発 END
            if (CheckgArrObjSubChip(gMovingSubChipObj.selectedChipId) == true) {
                //選択チップの情報
                strMainteCd = gArrObjSubChip[gMovingSubChipObj.selectedChipId].mntnCd;
                strRoJobSeq = gArrObjSubChip[gMovingSubChipObj.selectedChipId].roJobSeq;
                strScheDeliDatetime = gArrObjSubChip[gMovingSubChipObj.selectedChipId].scheDeliDateTime;
                //2017/11/12 NSK  荒川 REQ-SVT-TMT-20160906-003 計画済みチップをストールから簡単に外せるようにする START
                strTempFlg = gArrObjSubChip[gMovingSubChipObj.selectedChipId].tempFlg;
                //2017/11/12 NSK  荒川 REQ-SVT-TMT-20160906-003 計画済みチップをストールから簡単に外せるようにする END
            } else {
                if (CheckgArrObjSubChip(strRelationSubChip) == true) {
                    strMainteCd = gArrObjSubChip[strRelationSubChip].mntnCd;
                    strRoJobSeq = gArrObjSubChip[strRelationSubChip].roJobSeq;
                    strScheDeliDatetime = gArrObjSubChip[strRelationSubChip].scheDeliDateTime;
                    //2017/11/12 NSK  荒川 REQ-SVT-TMT-20160906-003 計画済みチップをストールから簡単に外せるようにする START
                    strTempFlg = gArrObjSubChip[strRelationSubChip].tempFlg;
                    //2017/11/12 NSK  荒川 REQ-SVT-TMT-20160906-003 計画済みチップをストールから簡単に外せるようにする END
                } else {
                    //予期せぬエラー
                    ShowSC3240301Msg(912);
                }
            }
        } else {
            strServiceInId = gArrObjSubChip[strSubChipId].svcInId;
            strJobDtlId = gArrObjSubChip[strSubChipId].jobDtlId;
            strStalluseId = gArrObjSubChip[strSubChipId].stallUseId;
            strStallId = gArrObjSubChip[strSubChipId].stallId;
            strScheStartDatetime = gArrObjSubChip[strSubChipId].scheStartDateTime;
            strScheEndDatetime = gArrObjSubChip[strSubChipId].scheEndDateTime;
            strScheWorkTime = gArrObjSubChip[strSubChipId].scheWorkTime;
            strRowLockVersion = gArrObjSubChip[strSubChipId].rowLockVersion;
            strMainteCd = gArrObjSubChip[strSubChipId].mntnCd;
            strRoJobSeq = gArrObjSubChip[strSubChipId].roJobSeq;
            strScheDeliDatetime = gArrObjSubChip[strSubChipId].scheDeliDateTime;
            strPickDeliType = gArrObjSubChip[strSubChipId].pickDeliType;
            strScheSvcinDateTime = gArrObjSubChip[strSubChipId].scheSvcInDateTime;
            strRoNum = gArrObjSubChip[strSubChipId].roNum;
            // 2013/11/29 TMEJ 張 タブレット版SMB チーフテクニシャン機能開発 START
            strInspectionNeedFlg = gArrObjSubChip[strSubChipId].inspectionNeedFlg;
            // 2013/11/29 TMEJ 張 タブレット版SMB チーフテクニシャン機能開発 END
            //2017/11/12 NSK  荒川 REQ-SVT-TMT-20160906-003 計画済みチップをストールから簡単に外せるようにする START
            strTempFlg = gArrObjSubChip[strSubChipId].tempFlg;
            //2017/11/12 NSK  荒川 REQ-SVT-TMT-20160906-003 計画済みチップをストールから簡単に外せるようにする END
        }
    var rtnVal = {
        ButtonID: strButtonID
    , ServiceInId: strServiceInId
    , JobDtlId: strJobDtlId
    , StalluseId: strStalluseId
    , StallId: strStallId
    , ScheStartDatetime: strScheStartDatetime
    , ScheEndDatetime: strScheEndDatetime
    , ScheWorkTime: strScheWorkTime
    , RestFlg: strRestFlg
    , RowLockVersion: strRowLockVersion
    , SubChipKey: strSubChipKey
    , ShowDate: dtShowDate
    , MainteCd: strMainteCd
    , WorkSeq: strRoJobSeq
    , ScheDeliDatetime: strScheDeliDatetime
    , PickDeliType: strPickDeliType
    , ScheSvcinDateTime: strScheSvcinDateTime
    , RoNum: strRoNum
    // 2013/11/29 TMEJ 張 タブレット版SMB チーフテクニシャン機能開発 START
    , InspectionNeedFlg : strInspectionNeedFlg
    // 2013/11/29 TMEJ 張 タブレット版SMB チーフテクニシャン機能開発 END
    //2017/11/12 NSK  荒川 REQ-SVT-TMT-20160906-003 計画済みチップをストールから簡単に外せるようにする START
    , tempFlg : strTempFlg
    //2017/11/12 NSK  荒川 REQ-SVT-TMT-20160906-003 計画済みチップをストールから簡単に外せるようにする END
    };
    return rtnVal;
}

/**
* コールバック後の処理関数(NoShow、中断移動、受付チップ移動)
* @param {String} result コールバック呼び出し結果
*/
function MovingAfterCallBackUpData(result) {

    //2015/07/30 TMEJ 明瀬 アクティビティインジケータが消えない問題 START

    ////タイマーをクリア
    //commonClearTimer();

    //2015/07/30 TMEJ 明瀬 アクティビティインジケータが消えない問題 END

    var jsonResult = $.parseJSON(result);

    //2015/07/30 TMEJ 明瀬 アクティビティインジケータが消えない問題 START

    //受付チップ移動でない場合、ここでタイマークリアを実施
    //※ただし、将来的にはNoShow、中断も含めて同様の改修が必要になる
    if (jsonResult.SubButtonID != C_RECEPTION) {

        //タイマーをクリア
        commonClearTimer();

    }

    //2015/07/30 TMEJ 明瀬 アクティビティインジケータが消えない問題 END

    //ストールチップ更新
    var jsonStallChip = $.parseJSON(htmlDecode(jsonResult.StallChip));
    var strChipId=0;
    // 関連チップの設定
    //中断エリアから移動の場合
    if (jsonResult.SubButtonID == C_STOP) {
        //サービス入庫idを取得する
        var chipDataList = $.parseJSON(htmlDecode(jsonResult.RelationChipInfo));
        var strSvcinId = "";
        for (var keyString in chipDataList) {
            var chipData = chipDataList[keyString];
            strSvcinId = chipData.SVCIN_ID;
            break;
        }
        // サービス入庫id取得できた場合、リレーションチップ構造体にこのサービスidをループして、該サービス入庫の全てチップ削除する
        if (strSvcinId != "") {
            for (var strId in gArrObjRelationChip) {
                if (gArrObjRelationChip[strId]) {
                    if (gArrObjRelationChip[strId].svcinId == strSvcinId) {
                        gArrObjRelationChip[strId] = null;
                    }
                }
            }
        }
    }
    AddRelationChipInfo(htmlDecode(jsonResult.RelationChipInfo));

    var strSubChipId = jsonResult.SubChipKey;

    //2015/04/23 TMEJ 明瀬  DMS連携版サービスタブレット強制納車機能追加開発 START

    //if (jsonResult.ResultCode == 0) {
    if (jsonResult.ResultCode == 0 ||
        jsonResult.ResultCode == -9000) {
        //サーバでの処理結果が下記の場合
        //　　0(成功)、または
        //-9000(DMS除外エラーの警告)

        if (jsonResult.ResultCode == -9000) {
            //サーバでの処理結果が、-9000(DMS除外エラーの警告)の場合

            //メッセージを表示する
            icropScript.ShowMessageBox(jsonResult.ResultCode, htmlDecode(jsonResult.Message), "");

        }

    //2015/04/23 TMEJ 明瀬  DMS連携版サービスタブレット強制納車機能追加開発 END

        if (gMovingSubChipObj!=null) {
            //新規チップのIDを探す
            for (var strKey in jsonStallChip) {
                var chipData = jsonStallChip[strKey];
                //2014/04/22 TMEJ 丁 【IT9669】サービスタブレットDMS連携作業追加機能開発 START
//                if ((chipData.STALL_ID == gMovingSubChipObj.stallId) && (new Date(chipData.SCHE_START_DATETIME) - gMovingSubChipObj.startDateTime == 0) && (chipData.SCHE_WORKTIME - gMovingSubChipObj.scheWorkTime == 0) && (CheckgArrObjChip(chipData.STALL_USE_ID) == false)) {
                if ((chipData.STALL_ID == gMovingSubChipObj.stallId) 
                    && (new Date(chipData.SCHE_START_DATETIME) - gMovingSubChipObj.startDateTime == 0)
                    && (chipData.SCHE_WORKTIME - gMovingSubChipObj.scheWorkTime == 0) 
                    && (CheckgArrObjChip(chipData.STALL_USE_ID) == false)
                    && (chipData.CANCEL_FLG == "0")) {
                    //2014/04/22 TMEJ 丁 【IT9669】サービスタブレットDMS連携作業追加機能開発 END
                    strChipId = chipData.STALL_USE_ID;
                    break;
                }
            }
            if (strChipId) {
                // 追加作業ステータスをバックする
                var strBackAddWorkStatus = "";
                if (gArrObjChip[strChipId]) {
                    strBackAddWorkStatus = gArrObjChip[strChipId].addWorkStatus;
                }
                // ストールチップとして生成する
                gArrObjChip[strChipId] = new ReserveChip(strChipId);
                // 新しい追加作業ステータスが空白の場合、更新しない
                if (gArrObjChip[strChipId].addWorkStatus.toString().Trim() == "") {
                    // バックした値を戻す
                    gArrObjChip[strChipId].setAddWorkStatus(strBackAddWorkStatus);
                }

                // 2013/11/29 TMEJ 張 タブレット版SMB チーフテクニシャン機能開発 START
                // Movingチップに部品出庫アイコンが表示されてる場合、新規チップのデータに設定する
                if (gMovingSubChipObj.partsFlg) {
                    if (gMovingSubChipObj.partsFlg == "1") {
                        gArrObjChip[strChipId].setPartsFlg("1");
                    }
                }
                // 2013/11/29 TMEJ 張 タブレット版SMB チーフテクニシャン機能開発 END

                // サブエリアの選択中のz-indexを削除する
                $("#" + C_TEMPCHIPID).css("z-index", "");

                // チップの枠
                // 画面にチップIDをC_TEMPCHIPID→stalluseidに変える
                $("#" + C_TEMPCHIPID).attr("id", strChipId);
            }
        }
        //最新のストールチップを表示する
        // 2013/11/29 TMEJ 張 タブレット版SMB チーフテクニシャン機能開発 START
        //ShowLatestChips(htmlDecode(jsonResult.StallChip));
        ShowLatestChips(htmlDecode(jsonResult.StallChip), false, false);
        // 2013/11/29 TMEJ 張 タブレット版SMB チーフテクニシャン機能開発 END
        if ((strChipId)&&(jsonResult.SubButtonID == C_STOP)) {
            // 中断フラグをtrueに設定
            gArrObjChip[strChipId].stopFlg = true;
        }
        // チップをタップする時のイベントを解除する
        $("#" + strChipId).unbind();
        // チップをタップする時のイベントを登録
        BindChipClickEvent(strChipId);

        // 2019/06/18 NSK 皆川 (トライ店システム評価)次世代サービスオペレーション効率化に向けた、業務適合性検証 START
        // 元チップ削除前に、対応するストール上チップのIDを取得する（タップ不可解除のため）
        var strStallChipId = gArrObjSubChip[jsonResult.SubChipKey].stallUseId;
        // 2019/06/18 NSK 皆川 (トライ店システム評価)次世代サービスオペレーション効率化に向けた、業務適合性検証 END

        // 元のチップを削除する
        $("#" + strSubChipId).remove();
        //サブボックスがとじられたらサブチップ再表示しない
        if (gOpenningSubBoxId == jsonResult.SubButtonID) {
//            //アクティブインジケータを表示
//            $("#SubChipAreaActiveIndicator").addClass("show");
//            $("#SubChip_LoadingScreen").css({ "display": "block" });
//            //リフレッシュタイマーセット
//            commonRefreshTimerTabletSMB(ReDisplay);
            if (jsonResult.SubButtonID == C_NOSHOW) {
                //NOShowエリアリフレッシュ
                CreateSubChips(C_NOSHOW, htmlDecode(jsonResult.NoShowArea));
            } else if (jsonResult.SubButtonID == C_STOP) {
                //中断エリアリフレッシュ
                CreateSubChips(C_STOP, htmlDecode(jsonResult.JobStopArea));
            } else if (jsonResult.SubButtonID == C_RECEPTION) {
                //受付エリアリフレッシュ
                CreateSubChips(C_RECEPTION, htmlDecode(jsonResult.ReceptionArea));

                // 2013/11/29 TMEJ 丁 タブレット版SMB チーフテクニシャン機能開発 START
                //NoShowエリアリフレッシュ
                //(着工時、チップがNoshowエリアにあれば、ストールに移動させる)
                InitializationSubChip(htmlDecode(jsonResult.NoShowButtonInfo));
                // 2013/11/29 TMEJ 丁 タブレット版SMB チーフテクニシャン機能開発 END
                
            }

            // 初期画面に表示されない場合、下へスクロールして、表示する
            $(".SubChipBox").SC3240301fingerScroll({
                action: "move",
                moveY: $(".SubChipBox .scroll-inner").position().top,
                moveX: $(".SubChipBox .scroll-inner").position().left
            });
        } else {
            if (jsonResult.SubButtonID == C_NOSHOW) {
                //NOShowボタンの情報更新
                InitializationSubChip(htmlDecode(jsonResult.NoShowButtonInfo));
            } else if (jsonResult.SubButtonID == C_STOP) {
                //中断ボタンの情報更新
                InitializationSubChip(htmlDecode(jsonResult.JobStopButtonInfo));
            } else if (jsonResult.SubButtonID == C_RECEPTION) {
                //受付ボタンの情報更新
                InitializationSubChip(htmlDecode(jsonResult.ReceptionButtonInfo));
            }
        }

        // 2019/06/18 NSK 皆川 (トライ店システム評価)次世代サービスオペレーション効率化に向けた、業務適合性検証 START
        // NoShowから再配置の場合
        if (jsonResult.SubButtonID == C_NOSHOW) {
            // タップ不可の場合、タップ可能にする
            if ($("#" + strStallChipId).length > 0) {
                if ($("#" + strStallChipId).data(C_DATA_CHIPTAP_FLG) == false) {
                    // タップ可能
                    $("#" + strStallChipId).data(C_DATA_CHIPTAP_FLG, true);
                }
                // 白い枠がある場合
                if ($("#WB" + strStallChipId).length > 0) {
                    // 削除する
                    $("#WB" + strStallChipId).remove();
                }
            }
        }
        // 2019/06/18 NSK 皆川 (トライ店システム評価)次世代サービスオペレーション効率化に向けた、業務適合性検証 END

        // 記録のオブジェクトをクリアする
        gArrBackChipObj.length = 0;
        //コールバック終了
        AfterCallBack();
    } else {
        //エラーメッセージの表示
        alert(htmlDecode(jsonResult.Message));
        //操作リストをクリアする
        ClearOperationList();
        // 記録のオブジェクトをクリアする
        gArrBackChipObj.length = 0;
        //画面をリフレッシュ
        SetSubChipBoxClose();
        ClickChangeDate(0);
    }
    //MovingSubChipをリセットする
    gMovingSubChipObj = null;
}

///**
//* サブチップ最新情報表示（個別）
//* @param {String} strchip チップID
//* @return {なし}
//*/
//function SubChipAreaReLoad(strChip) {
////サブボックスが閉じる状態の場合何もしない
//    if (gOpenningSubBoxId == "") {
//        return;
//    }
//    var strareaid = GetSubChipType(strChip)
//    $("#SubChipAreaActiveIndicator").addClass("show");
//    $("#SubChip_LoadingScreen").css({ "display": "block" });
//    //サーバーに渡すパラメータを作成
//    //コールバック開始
//    var prms;
//    switch (strareaid) {
//        case C_FT_BTNTP_CONFIRMED_RO:
//            prms = CreateCallBackSubChipParam(C_RECEPTION);
//            DoCallBack(C_CALLBACK_WND301, prms, ReceptionAfterCallBack, "SubChipAreaReLoad");
//            //gCallbackSC3240301.doCallback(prms, ReceptionAfterCallBack);
//            break;
//        case C_FT_BTNTP_WAIT_CONFIRMEDADDWORK:
//            prms = CreateCallBackSubChipParam(C_ADDITIONALWORK);
//            DoCallBack(C_CALLBACK_WND301, prms, AddWorkAfterCallBack, "SubChipAreaReLoad");
//            //gCallbackSC3240301.doCallback(prms, AddWorkAfterCallBack);
//            break;
//        case C_FT_BTNTP_CONFIRMED_INSPECTION:
//            prms = CreateCallBackSubChipParam(C_COMPLETIONINSPECTION);
//            DoCallBack(C_CALLBACK_WND301, prms, InspectionComAfterCallBack, "SubChipAreaReLoad");
//            //gCallbackSC3240301.doCallback(prms, InspectionComAfterCallBack);
//            break;
//        case C_FT_BTNTP_WAITING_WASH:
//        case C_FT_BTNTP_WASHING:
//            prms = CreateCallBackSubChipParam(C_CARWASH);
//            DoCallBack(C_CALLBACK_WND301, prms, CarWashAfterCallBack, "SubChipAreaReLoad");
//            //gCallbackSC3240301.doCallback(prms, CarWashAfterCallBack);
//            break;
//        case C_FT_BTNTP_WAIT_DELIVERY:
//            prms = CreateCallBackSubChipParam(C_DELIVERDCAR);
//            DoCallBack(C_CALLBACK_WND301, prms, DeliWaitAfterCallBack, "SubChipAreaReLoad");
//            gCallbackSC3240301.doCallback(prms, DeliWaitAfterCallBack);
//        break;
//            return false;
//    }
//}
/**
* 受付サブチップ最新情報表示（個別）
* @param {String} 
* @return {なし}
*/
function ReceptionAreaReLoad() {
    //相応なサブボックスではないの場合何もしない
    if (gOpenningSubBoxId != C_RECEPTION) {
        return;
    }
    $("#SubChipAreaActiveIndicator").addClass("show");
    $("#SubChip_LoadingScreen").css({ "display": "block" });
    //コールバック開始
    var prms = CreateCallBackSubChipParam(C_RECEPTION);
    DoCallBack(C_CALLBACK_WND301, prms, ReceptionAfterCallBack, "ReceptionAreaReLoad");
    //gCallbackSC3240301.doCallback(prms, ReceptionAfterCallBack);
}
/**
* 追加作業サブチップ最新情報表示（個別）
* @param {String} 
* @return {なし}
*/
function AddWorkAreaReLoad() {
    //相応なサブボックスではないの場合何もしない
    if (gOpenningSubBoxId != C_ADDITIONALWORK) {
        return;
    }
    $("#SubChipAreaActiveIndicator").addClass("show");
    $("#SubChip_LoadingScreen").css({ "display": "block" });
    //コールバック開始
    var prms = CreateCallBackSubChipParam(C_ADDITIONALWORK);
    DoCallBack(C_CALLBACK_WND301, prms, AddWorkAfterCallBack, "AddWorkAreaReLoad");
    //gCallbackSC3240301.doCallback(prms, AddWorkAfterCallBack);
}
/**
* 完成検査サブチップ最新情報表示（個別）
* @param {String} 
* @return {なし}
*/
function ComInspectionAreaReLoad() {
    //相応なサブボックスではないの場合何もしない
    if (gOpenningSubBoxId != C_COMPLETIONINSPECTION) {
        return;
    }
    $("#SubChipAreaActiveIndicator").addClass("show");
    $("#SubChip_LoadingScreen").css({ "display": "block" });
    //コールバック開始
    var prms = CreateCallBackSubChipParam(C_COMPLETIONINSPECTION);
    DoCallBack(C_CALLBACK_WND301, prms, InspectionComAfterCallBack, "ComInspectionAreaReLoad");
    //gCallbackSC3240301.doCallback(prms, InspectionComAfterCallBack);
}
/**
* 洗車サブチップ最新情報表示（個別）
* @param {String} 
* @return {なし}
*/
function CarWashAreaReLoad() {
    //相応なサブボックスではないの場合何もしない
    if (gOpenningSubBoxId != C_CARWASH) {
        return;
    }
    $("#SubChipAreaActiveIndicator").addClass("show");
    $("#SubChip_LoadingScreen").css({ "display": "block" });
    //コールバック開始
    var prms = CreateCallBackSubChipParam(C_CARWASH);
    DoCallBack(C_CALLBACK_WND301, prms, CarWashAfterCallBack, "CarWashAreaReLoad");
    //gCallbackSC3240301.doCallback(prms, CarWashAfterCallBack);
}
/**
* 納車待ちサブチップ最新情報表示（個別）
* @param {String} 
* @return {なし}
*/
function DeliverdCarAreaReLoad() {
    //相応なサブボックスではないの場合何もしない
    if (gOpenningSubBoxId != C_DELIVERDCAR) {
        return;
    }
    $("#SubChipAreaActiveIndicator").addClass("show");
    $("#SubChip_LoadingScreen").css({ "display": "block" });
    //コールバック開始
    var prms = CreateCallBackSubChipParam(C_DELIVERDCAR);
    DoCallBack(C_CALLBACK_WND301, prms, DeliWaitAfterCallBack, "DeliverdCarAreaReLoad");
    //gCallbackSC3240301.doCallback(prms, DeliWaitAfterCallBack);

}
/**
* NoShowサブチップ最新情報表示（個別）
* @param {String} 
* @return {なし}
*/
function NoShowAreaReLoad() {
    //相応なサブボックスではないの場合何もしない
    if (gOpenningSubBoxId != C_NOSHOW) {
        return;
    }
    $("#SubChipAreaActiveIndicator").addClass("show");
    $("#SubChip_LoadingScreen").css({ "display": "block" });
    //コールバック開始
    var prms = CreateCallBackSubChipParam(C_NOSHOW);
    DoCallBack(C_CALLBACK_WND301, prms, NoShowAfterCallBack, "NoShowAreaReLoad");
    //gCallbackSC3240301.doCallback(prms, NoShowAfterCallBack);
}
/**
* 中断サブチップ最新情報表示（個別）
* @param {String} 
* @return {なし}
*/
function StopAreaReLoad() {
    //相応なサブボックスではないの場合何もしない
    if (gOpenningSubBoxId != C_STOP) {
        return;
    }
    $("#SubChipAreaActiveIndicator").addClass("show");
    $("#SubChip_LoadingScreen").css({ "display": "block" });
    //コールバック開始
    var prms = CreateCallBackSubChipParam(C_STOP);
    DoCallBack(C_CALLBACK_WND301, prms, StopAfterCallBack, "StopAreaReLoad");
    //gCallbackSC3240301.doCallback(prms, StopAfterCallBack);
}
/**
* サブチップとストール上チップ関連性チェック
* @param {String} strchip チップID
* @return {String}　strSvcInId
*/
function GetSubChipSVCINID(strChipId) {
    var strSvcInId = "";
    if (gArrObjSubChip[strChipId]) {       
        strSvcInId=gArrObjSubChip[strChipId].svcInId;             
    }
    return strSvcInId;
}
/**
* サブチップとストールチップのチェック
* @param {String} strchip チップID
* @return {String}　strChipFlg　C_CHIPTYPE_STALL 1:ストールチップ  C_CHIPTYPE_SUBCHIP 3:サブチップチップ
*/
function IsChipOrSubChip(strChipId) {
    var strChipFlg = C_CHIPTYPE_STALL;
    if (gArrObjSubChip[strChipId]) {
        strChipFlg = C_CHIPTYPE_SUBCHIP;
        return strChipFlg;
    }
    return strChipFlg;
}

/**
* サブチップから、ストール上に関連あるストール親チップIDを取得
* @param {String} strChipId サブチップID
* @return {String}　strRelationChipId　関連あるストールチップID
*/
function GetRelationChipId(strSubChipId) {
    var strRelationChipId = "";
    var strSvcInId  = GetSubChipSVCINID(strSubChipId);
    if (strSvcInId) {
        for (var keyString in gArrObjChip) {
            if (CheckgArrObjChip(keyString) == false) {
                continue;
            }
            if ((gArrObjChip[keyString].svcInId == strSvcInId) &&(gArrObjChip[keyString].childNo == "1")){
                strRelationChipId = gArrObjChip[keyString].stallUseId;
                return strRelationChipId;
            }  
        }
    }
    return strRelationChipId;
}
/**
* ストールチップから、サブチップに関連あるサブチップIDを取得
* @param {String} strChipId チップID
* @return {String}　strRelationSubChipId　関連あるサブチップID
*/
function GetRelationSubChipId(strChipId) {
    var strRelationSubChipId = "";
    var strSvcInId;
    if (gArrObjChip[strChipId]) {
        strSvcInId = gArrObjChip[strChipId].svcInId;
    }
    if (strSvcInId) {
        for (var keyString in gArrObjSubChip) {
            if (CheckgArrObjSubChip(keyString) == true) {
                if (gArrObjSubChip[keyString].svcInId == strSvcInId) {
                    if (!$("#" + keyString + " .Front").hasClass("BlackBack") && $("#" + keyString).length > 0) {
                        strRelationSubChipId = gArrObjSubChip[keyString].KEY;
                        return strRelationSubChipId;
                    }
                }
            }
        }
    }
    return strRelationSubChipId;
}

/**
* チップ移動する時更新パタンー取得
* @param {String} strChipId チップID
* @return {String}　strChipUpdatatype 　C_UPDATA_STALLCHIP 0: ストールチップ
*C_UPDATA_SUBCHIP 1:サブチップ
*                                   
*/
function GetChipUpdatetype(strChipId) {
    var strChipUpdatatype = C_UPDATA_STALLCHIP;
    var strRelationSubChipId;
    //サブチップボックスが閉じられた場合（非同期のため）
    if ($(".SubChipBox").css("display") === 'none') {
        return strChipUpdatatype;
    }
    //サブチップオブジェクトに探す
    if (gArrObjSubChip[strChipId]) {
        strChipUpdatatype = C_UPDATA_SUBCHIP;
        return strChipUpdatatype;
    }
    //ストールチップ関連性から判定
    for (var keyString in gArrObjSubChip) {
        if (CheckgArrObjSubChip(keyString) == false) {
            continue;
        }
        if (!$("#" + keyString + " .Front").hasClass("BlackBack") && $("#" + keyString).length>0) {
            strChipUpdatatype = C_UPDATA_SUBCHIP;
            return strChipUpdatatype;
        }
    }

    return strChipUpdatatype;
}

/**
* 配列gArrObjSubChipをチェックする
* @param {String} strChipId  チップid
* @return {bool} true:gArrObjChip[strChipId]が有効のチップデータ
*/
function CheckgArrObjSubChip(strSubChipId) {
    if ((gArrObjSubChip[strSubChipId] == null) || (typeof gArrObjSubChip[strSubChipId] != "object")) {
        return false;
    } else {
        return true;
    }
}
//2013/04/24 myose add start
/**
* サブチップボックスのスクロール位置を記録する.
* @param {String} areaId  ストールの配置エリアID
* @return {-} -
*/
function SetTranslateValSubBox(areaId) {

    switch (areaId) {
        case C_FT_BTNTP_CONFIRMED_RO:
            //受付サブチップボックス
            gTranslateValSubBoxX = $(".SubChipReception .SubChipBox").find(".scroll-inner").position().left;
            break;

        case C_FT_BTNTP_WAIT_CONFIRMEDADDWORK:
            //追加作業サブチップボックス
            gTranslateValSubBoxX = $(".SubChipAdditionalWork .SubChipBox").find(".scroll-inner").position().left;
            break;

        case C_FT_BTNTP_CONFIRMED_INSPECTION:
            //完成検査サブチップボックス
            gTranslateValSubBoxX = $(".SubChipCompletionInspection .SubChipBox").find(".scroll-inner").position().left;
            break;

        case C_FT_BTNTP_WAITING_WASH:
        case C_FT_BTNTP_WASHING:
            //洗車サブチップボックス
            gTranslateValSubBoxX = $(".SubChipCarWash .SubChipBox").find(".scroll-inner").position().left;
            break;

        case C_FT_BTNTP_WAIT_DELIVERY:
            //納車待ちサブチップボックス
            gTranslateValSubBoxX = $(".SubChipWaitingDelivered .SubChipBox").find(".scroll-inner").position().left;
            break;

        case C_FT_BTNTP_NOSHOW:
            //NoShowサブチップボックス
            gTranslateValSubBoxX = $(".SubChipNoShow .SubChipBox").find(".scroll-inner").position().left;
            break;

        case C_FT_BTNTP_STOP:
            //中断サブチップボックス
            gTranslateValSubBoxX = $(".SubChipStop .SubChipBox").find(".scroll-inner").position().left;
            break;        
    }

}
//2013/04/24 myose add end

function SubChipCancel() {
    var rtValue = confirm(gSC3240301WordIni[909]);
    // 削除の場合
    if (rtValue) {
        $("#SubChipAreaActiveIndicator").addClass("show");
        $("#SubChip_LoadingScreen").css({ "display": "block" });
        //リフレッシュタイマーセット
        commonRefreshTimerTabletSMB(NoShowAreaReLoad);
        //サーバーに渡すパラメータを作成
        var prms = CreateCallBackActionButtonParam(C_FT_BTNID_DEL);
        //コールバック開始
        DoCallBack(C_CALLBACK_WND301, prms, AfterCallBackFooterActionButton, "SubChipCancel");
        //gCallbackSC3240301.doCallback(prms, AfterCallBackFooterActionButton);
    }
}
/**
* チップの幅を取得する.
* @param {Integer} aWidth  表示開始時間
* @return {Integer}  nWidth 幅
*/
function GetSubChipWidth(aWidth) {
    var nWidth = aWidth;
    // 5分より小さい場合、見えない
    if (nWidth < C_CELL_WIDTH / 15 * gResizeInterval - 1) {
        nWidth = C_CELL_WIDTH / 15 * gResizeInterval - 1;
    }
    return Math.round(nWidth);
}
/**
* 完成検査ボタンイベント
*/
function ClickBtnInspection() {

    if (!gArrObjSubChip[gSelectedChipId]) {
        return;
    }
    // グルグルを表示する
    gMainAreaActiveIndicator.show();

    var strParam = '{'
    strParam += '"OperationCode":"' + C_COMPLETIONINSPECTION_REDIRECT + '"';
    strParam += ',"OrderNo":"' + gArrObjSubChip[gSelectedChipId].roNum + '"';
    strParam += ',"VisitId":"' + gArrObjSubChip[gSelectedChipId].visitId + '"';
    strParam += ',"DmsJobDtlId":"' + gArrObjSubChip[gSelectedChipId].dmsJobDtlId + '"';
    strParam += ',"Vin":"' + gArrObjSubChip[gSelectedChipId].vclVin + '"';
    //2014/04/22 TMEJ 丁 【IT9669】サービスタブレットDMS連携作業追加機能開発 START
    strParam += ',"JobDtlId":"' + gArrObjSubChip[gSelectedChipId].jobDtlId + '"';
    //2014/04/22 TMEJ 丁 【IT9669】サービスタブレットDMS連携作業追加機能開発 END
    strParam += '}';
    //画面遷移のためポストバック
    __doPostBack("", strParam);
    return false;
}

/**
* 追加作業承認ボタンイベント
*/
function ClickBtnAddWorkConfirm() {

    if (!gArrObjSubChip[gSelectedChipId]) {
        return;
    }
    // グルグルを表示する
    gMainAreaActiveIndicator.show();

    var strParam = '{'
    strParam += '"OperationCode":"' + C_ADDWORKCONFIRM_REDIRECT + '"';
    strParam += ',"OrderNo":"' + gArrObjSubChip[gSelectedChipId].roNum + '"';
    strParam += ',"RoJobSeq":"' + gArrObjSubChip[gSelectedChipId].roJobSeq + '"';
    strParam += ',"VisitId":"' + gArrObjSubChip[gSelectedChipId].visitId + '"';
    strParam += ',"DmsJobDtlId":"' + gArrObjSubChip[gSelectedChipId].dmsJobDtlId + '"';
    strParam += ',"Vin":"' + gArrObjSubChip[gSelectedChipId].vclVin + '"';
    strParam += '}';
    //画面遷移のためポストバック
    __doPostBack("", strParam);
    return false;
}
/**
* リレーションチップを取得する(order by ChildNo)
* @param {String} strStallUseId チップStallUseId
* @param {String} strSvcinId チップサービス入庫ID
* @return {Array} strChipIdのリレーションチップの配列
*/
function FindRelationChipsFromSubChip(strStallUseId, strSvcinId) {

    var arrRelationChipId = new Array();
    // サービス入庫IDが空白の場合、
    if (strSvcinId == "") {
        // 関連チップにあれば、
        if (gArrObjChip[strStallUseId]) {
            // サービス入庫IDを取得
            strSvcinId = gArrObjChip[strStallUseId].svcInId;
        } else {
            // 関連チップがない場合
            return arrRelationChipId;
        }
    }

    // すべて関連チップをループして、同じsvcInIdを持つチップを探す
    var nLoop = 0;
    for (var strId in gArrObjChip) {
        // 2013/11/29 TMEJ 張 タブレット版SMB チーフテクニシャン機能開発 START
        if (CheckgArrObjChip(strId) == false) {
            continue;
        }
        // 2013/11/29 TMEJ 張 タブレット版SMB チーフテクニシャン機能開発 END
        
        if (gArrObjChip[strId]) {

            //2015/04/01 TMEJ 明瀬 TMT２販社号口後フォロー BTS-281 START

            //if ((gArrObjChip[strId].svcInId == strSvcinId) && (gArrObjChip[strId].stallUseStatus != C_STALLUSE_STATUS_STOP)) {
            //    arrRelationChipId[nLoop] = new Array(strId, gArrObjChip[strId].startDateTime);
            //    nLoop++;
            //}

            //サービス入庫IDが同じ(関連チップである)
            if (gArrObjChip[strId].svcInId == strSvcinId) {

                //ストール利用ステータスが05(中断)でない、かつ
                //ストール利用ステータスが06(日跨ぎ終了)でない
                //※中断実績チップと日跨ぎ終了実績チップは関連チップとしてカウントしない
                if (gArrObjChip[strId].stallUseStatus != C_STALLUSE_STATUS_STOP &&
				    gArrObjChip[strId].stallUseStatus != C_STALLUSE_STATUS_MIDFINISH) {

                    // 2019/08/09 NSK 鈴木 DLR-002 子チップを着工指示した時にエラー発生（親チップ着工指示でエラー発生を修正） START
                    // arrRelationChipId[nLoop] = new Array(strId, gArrObjChip[strId].startDateTime);

                    // 作業内容IDをマッチングキーとして追加で設定する。
                    arrRelationChipId[nLoop] =
                        new Array(strId, gArrObjChip[strId].startDateTime, gArrObjChip[strId].jobDtlId);
                    // 2019/08/09 NSK 鈴木 DLR-002 子チップを着工指示した時にエラー発生（親チップ着工指示でエラー発生を修正） END

	                nLoop++;
	            }
            }

            //2015/04/01 TMEJ 明瀬 TMT２販社号口後フォロー BTS-281 END
        }
    }
    // startDatetimeよりsortする
    arrRelationChipId.sort(function (x, y) { return x[0] - y[0] });
    return arrRelationChipId;
}

//文言取得
//@return {Date}
function GetSC3240301WordIni() {
    var strWordIni = $("#hidSubMsgData").val();
    if (gSC3240301WordIni == null) {
        gSC3240301WordIni = $.parseJSON(strWordIni);
        $("#hidSubMsgData").attr("value", "");
    }
}

//メッセージを表示する
//@return {String}
function ShowSC3240301Msg(strWordNo) {
    //gSC3240101WordIni[strWordNo]があれば、メッセージボックスで表示
    if (gSC3240301WordIni != null) {
        if (gSC3240301WordIni[strWordNo] != null) {
            alert(gSC3240301WordIni[strWordNo]);
        }
    }
}
//文字位置調整
//@return {String}
function AdjustSubChipItemByWidth(strChipId) {
    // 車両番号が左寄せか、右寄せかを判断する
    if ($("#" + strChipId + " .CarNoL").length > 0) {
        if ($("#" + strChipId + " .CarNoL").width() <= $("#" + strChipId + " .CarNoL span").width()) {
            $("#" + strChipId + " .CarNoL").addClass("CarNoR").removeClass("CarNoL");
        }
    } else {
        if ($("#" + strChipId + " .CarNoR").width() > $("#" + strChipId + " .CarNoR span").width()) {
            $("#" + strChipId + " .CarNoR").addClass("CarNoL").removeClass("CarNoR");
        }
    }
}
//サブチップサーチ
function SubChipSearch() {
    // 検索画面から、検索したチップを選択中にする
    if (gSearchedSubChipAreaId) {
        switch (gSearchedSubChipAreaId) {
            //2017/07/12 NSK 河谷 REQ-SVT-TMT-20160906-001 チップ検索の検索範囲をPC-SMBと同じにする START 
            case C_RECEPTION:
                ShowReceptionchip();
                break;
            case C_ADDITIONALWORK:
                ShowAddWorkchip();
                break;
            //2017/07/12 NSK 河谷 REQ-SVT-TMT-20160906-001 チップ検索の検索範囲をPC-SMBと同じにする END 
            case C_STOP:
                ShowStopchip();
                break;
            case C_NOSHOW:
                ShowNoShowchip();
                break;
            case C_DELIVERDCAR:
                ShowDeliverdCarchip();
                break;
            case C_CARWASH:
                ShowCarWashchip();
                break;
            case C_COMPLETIONINSPECTION:
                ShowCompletionchip();
                break;
        }
        if (gOpenningSubBoxId != "") {
            FooterIconReplace(gOpenningSubBoxId);
        }
    }
}
//サブチップサーチでサブボックスをスクロール
function SearchChipScroll(strSubChipId) {
    var subChipLeft = $("#" + strSubChipId).position().left;
    var diffTime = subChipLeft - gSubAreawidth;
    var moveX;
    //サブチップの位置が初期表示時に表示できるの場合何もしない
    if (diffTime <= 0) {
        return;
    }
    var count = Math.floor(subChipLeft / gSubAreawidth);
    var surplus = subChipLeft % gSubAreawidth;
    moveX = (count - 1) * gSubAreawidth + surplus + C_SubChipWidth + 50;
    // サーチからに表示されない場合、スクロールして、表示する
    $(".SubChipBox").SC3240301fingerScroll({
        action: "move",
        moveY: $(".SubChipBox .scroll-inner").position().top,
        moveX: moveX
    });
}

/**
* Movingチップと重複チップのidを取得する
* @return {Array} 重複チップid とポップアップに表示順番
*/
function GetMovingCpDuplicateSubChips() {

    // Movingチップ左座標と右座標を記録する
    var nMovingChipLeft = $("#" + C_MOVINGCHIPID).position().left;
    var nMovingChipRight = nMovingChipLeft + $("#" + C_MOVINGCHIPID).width();
    var nLoop = 0;
    var arrDuplChipId = new Array();

    // この行に全てチップを取得する
    $("#" + C_MOVINGCHIPID).offsetParent().children("div").each(function (index, e) {
        // Movingチップ以外のチップの場合、左座標と右座標を記録する
        if ((e.id != gSelectedChipId) && (e.id != C_MOVINGCHIPID) && (!IsUnavailableArea(e.id)) && (!IsRestArea(e.id)) && ((gArrObjSubChip[gSelectedChipId].RO_JOB_SEQ != 0) && (e.id != gArrObjSubChip[gSelectedChipId].stallUseId))) {

            // 仮仮チップと重複する時
            var nChipLeft = e.offsetLeft;
            var nChipRight = e.offsetLeft + e.offsetWidth;
            // 重複の場合、重複チップのidを記録する
            if ((!((nChipRight < nMovingChipLeft) || (nChipLeft > nMovingChipRight))
                            && (left(e.id.toString(), 2) != "WB"))) {
                arrDuplChipId[nLoop] = e.id;
                nLoop++;
            }

        }
    });

    return arrDuplChipId;
}

/**
* サブボックスCallBack分類
* @param {string} buttonID　コールバックのButtonId
* @return {Integer} CallBack種類　　0:サブボックスを開く
*                                   1:それ以外
*/
function GetOperationType(buttonID) {
    switch (buttonID) {
        case C_RECEPTION:
        case C_ADDITIONALWORK:
        case C_COMPLETIONINSPECTION:
        case C_CARWASH:
        case C_DELIVERDCAR:
        case C_NOSHOW:
        case C_STOP:
            return C_OPERATIONTYPE_GETSUBBOXCHIP;
            break;
        default:
            return C_OPERATIONTYPE_OTHER;

    }

}

// 2019/08/09 NSK 鈴木 DLR-002 子チップを着工指示した時にエラー発生（親チップ着工指示でエラー発生を修正） START
/**
 * リレーションチップからマッチングキーに対するチップIDを取得する。
 * @param {Array} arrRelationChipId リレーションチップ配列
 * @param {String} matchingKey マッチングキー
 * @param {String} selectedChipId サブチップID
 * @return {String} チップID
 */
function GetChipIdFromRelationChips(arrRelationChipId, matchingKey, selectedChipId) {
    // 初期化
    // キーマッチングしなかった場合は、サブチップIDを返却する。
    var strChipId = selectedChipId;

    // キーマッチングを行い、マッチングしたキーのチップIDを返却する。
    if (0 < arrRelationChipId.length) {
        var relationChipId;
        for (var count = 0; count < arrRelationChipId.length; count++) {
            relationChipId = arrRelationChipId[count];
            if (relationChipId[INDEX_RELATION_CHIPS_MATCHING_KEY] == matchingKey) {
                strChipId = relationChipId[INDEX_RELATION_CHIPS_CHIP_ID];
                break;
            }
        }
    }

    return strChipId;
}
// 2019/08/09 NSK 鈴木 DLR-002 子チップを着工指示した時にエラー発生（親チップ着工指示でエラー発生を修正） END
