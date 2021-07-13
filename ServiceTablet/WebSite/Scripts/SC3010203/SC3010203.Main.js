/**
* @fileOverview SC3010203　メイン処理関数
*
* @author TCS 寺本
* @version 1.0.0
*/
/// <reference path="../jquery.js"/>
/// <reference path="../jquery.fingerscroll.js"/>
/// <reference path="../icropScript.js"/>
/// <reference path="SC3010203.Data.js"/>
/// <reference path="SC3010203.Layout.js"/>
/// <reference path="SC3010203.Drag.js"/>
/// <reference path="SC3010203.Ajax.js"/>

/**
 * DOMロード時の処理(スケジュール時間)
 */
$(function () {
    
    //時間スケジュール・TODOスクロール
    $("#timeScheduleBoxOut, #todoChipBox").fingerScroll();
    //初期化
    schedule.init();
    //CalDav情報取得
    schedule.loadCalDav(function () {
        //チップの配置開始
        schedule.layoutChip.allLayout();
        $("#loadingSchedule").hide(0);
        $("#contentsRightBox .LodingInnerBox01").removeClass("LodingInnerBox01");
    });

});

/**
* RSSサイズ変更
*/
function dayEventSizeChange() {

    $("#DateScheduleInner").fingerScroll();

    if ($("#DateScheduleBox ul").is(".normalMode") === true) {
        //通常サイズから大きいサイズ
        $("#DateScheduleBox").addClass("bigMessageWindow");
        $("#DateScheduleBox ul").removeClass("normalMode");
        $("#DayEventBigSizeLink").hide(0);
        $("#DayEventNormalSizeLink").show(0);
    } else {
        //大きいサイズから通常サイズ
        $("#DateScheduleBox").removeClass("bigMessageWindow").one("webkitTransitionEnd", function (e) {
            //枠を縮めてから、４件目以降の連絡事項を消す
            $("#DateScheduleBox ul").addClass("normalMode");
        });
        $("#DayEventBigSizeLink").show(0);
        $("#DayEventNormalSizeLink").hide(0);
    }
    return false;
}

/**
* DOMロード時の処理(終日イベント)
*/
$(function () {

    //イベントバインド
    $("#DayEventBigSizeLink, #DayEventNormalSizeLink").bind("mousedown touchstart", dayEventSizeChange);

});

/**
* ダッシュボード読み込み処理
*/
$(function () {
    $("#dashboardFrame").bind("load", function () {
        $("#dashboardBox").removeClass("loading");
    });
});

/**
* Todoチップ選択の開始
*/
$(function () {

    //選択監視
    $("#todoChipBox .todoChip").live("click", function (e) {
        var selectChip;

        //対象チップを特定
        if ($(e.target).is("#todoChipBox .todoChip") === true) {
            //チップ自体をタップ
            selectChip = $(e.target);
        } else {
            //チップ内の子タグをタップ
            selectChip = $(e.target).parents("#todoChipBox .todoChip");
            if (selectChip.length !== 1) return;
        }

        var item = schedule.getTodoFromUid(selectChip.attr("UID"));
        if (item === null) return;

        if (item.createLocation == "1") {
            //iCROP系チップ
            //遷移パラメータ
            $("#selectDLRCD").val(item.dlrCd);
            $("#selectSTRCD").val(item.strCd);
            $("#selectFOLLOWUPBOXSEQNO").val(item.scheduleID);

            //オーバーレイ表示
            $("#serverProcessOverlayBlack").css("display", "block");
            //アニメーション
            setTimeout(function () {
                $("#serverProcessIcon").addClass("show");
                $("#serverProcessOverlayBlack").addClass("open");
            }, 0);

            //遷移用のボタンクリック
            $("#CustDetailDummyButton").click();
        } else {
            //ネイティブチップ
            //window.location = "icrop:cale:";
        }

    });

});

/**
* スケジュールチップ選択処理
*/
$(function () {

    //時間スケジュールのチップ
    $("#timeScheduleChipBox div.timeScheduleChip").live("click", function (e) {
        var selectChip;

        //対象チップを特定
        if ($(e.target).is("#timeScheduleChipBox div.timeScheduleChip") === true) {
            //チップ自体をタップ
            selectChip = $(e.target);
        } else {
            //チップ内の子タグをタップ
            selectChip = $(e.target).parents("#timeScheduleChipBox div.timeScheduleChip");
            if (selectChip.length !== 1) return;
        }

        var item = schedule.getEventFromUid(selectChip.attr("UID"));
        if (item === null) return;

        //アプリ起動
        schedule.appExecute.executeCaleEdit(item.eventId);
    });

    //終日イベント
    $("#DateScheduleInner ul li").live("click", function (e) {
        var item = schedule.getDayEventFromUid($(this).attr("UID"));
        if (item === null) return;
        //アプリ起動
        schedule.appExecute.executeCaleEdit(item.eventId);
    });
});


$(function () {

    //スケジュール新規作成イベント
    $("#timeScheduleRightBox div.marginArea, #DateScheduleBox div.dateScheduleMarginArea").live("click", function (e) {
        //アプリ起動
        schedule.appExecute.executeCaleNew();
    });

});