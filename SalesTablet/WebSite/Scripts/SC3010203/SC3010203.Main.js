/**
* @fileOverview SC3010203　メイン処理関数
*
* @author TCS 寺本
* @version 1.0.0
* 
* @update TCS 2012/05/17 TCS 安田 クルクル対応
* @update TCS 2014/05/20 TCS 河原 マネージャー機能($01)  
* @update TS  2019/05/28 TS  舩橋 PostUAT-3098 カレンダーアプリ呼び出し変更
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

    $("div#parentDiv").css("width", "1600px");


    //縦幅の調整
    $("#VisitBoxIn .scroll-inner").css("padding-bottom", "10px")

    //表示対象日付の判定
    if ($("#isToDoBox").val() == "0") {
        $(".unComp").css("display", "");
        $(".slash").css("display", "");
        $("#ToDoDispSegmentedButton").css("display", "");
    } else {
        $(".unComp").css("display", "none");
        $(".slash").css("display", "none");
        $("#ToDoDispSegmentedButton").css("display", "none");
    }

    //$01 Update Start
    if (this_form.opeCD.value == 8) {
        //来店実績欄の読み込み
        $("#VisitSalesTrigger").click();
    }
    //$01 Update End
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
        dashboardFrameLoadEnd();
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

        if (item.createLocation == "1" && $("#isToDoChipDrop").val() == "0") {
            customerDetailTransfer(item.dlrCd, item.strCd, item.scheduleID, "")
        } else {
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

        // 2019/05/28 TS 舩橋 PostUAT-3098 カレンダーアプリ呼び出し変更 START 
        //アプリ起動
        CallToiOSSchedule();
        // 2019/05/28 TS 舩橋 PostUAT-3098 カレンダーアプリ呼び出し変更 END 
    });

    //終日イベント（チップ）
    $("#DateScheduleInner ul li").live("click", function (e) {
        var item = schedule.getDayEventFromUid($(this).attr("UID"));
        if (item === null) return;
        // 2019/05/28 TS 舩橋 PostUAT-3098 カレンダーアプリ呼び出し変更 START 
        //アプリ起動
        CallToiOSSchedule();
        // 2019/05/28 TS 舩橋 PostUAT-3098 カレンダーアプリ呼び出し変更 END 
    });

    //終日イベント（全体）
    $("#timeScheduleChipBox").live("click", function (e) {
        if ($(e.target).is("#timeScheduleChipBox div.timeScheduleChip") === false) {
            // 2019/05/28 TS 舩橋 PostUAT-3098 カレンダーアプリ呼び出し変更 START 
            //アプリ起動
            CallToiOSSchedule();
            // 2019/05/28 TS 舩橋 PostUAT-3098 カレンダーアプリ呼び出し変更 END 
        }
    });

    //終日スケジュール
    $("#DateScheduleBox").click(function () {
        if ($("#dayEventNotFound").css("display") === "block") {
            // 2019/05/28 TS 舩橋 PostUAT-3098 カレンダーアプリ呼び出し変更 START 
            //アプリ起動
            CallToiOSSchedule();
            // 2019/05/28 TS 舩橋 PostUAT-3098 カレンダーアプリ呼び出し変更 END 
        }
    });

});

$(function () {
    //スケジュール新規作成イベント
    $("#timeScheduleRightBox div.marginArea, #DateScheduleBox div.dateScheduleMarginArea").live("click", function (e) {
        // 2019/05/28 TS 舩橋 PostUAT-3098 カレンダーアプリ呼び出し変更 START 
        //アプリ起動
        CallToiOSSchedule();
        // 2019/05/28 TS 舩橋 PostUAT-3098 カレンダーアプリ呼び出し変更 END 
    });
    //スワイプの監視
    $(document.body).bind("mousedown touchstart", function (e) {
        if ($(e.target).is("#contentsLeftBox2 *") == false && $(e.target).is("#MessageListViewUpPanel  *") == false && $("#isSwipeLockHidden").val() == "0" && $("#isToDoChipDrop").val() == "0") {
            //スワイプ
            swipeSettingSchedule();
        } else if ($(e.target).is("#contentsLeftBox2 *") == false && $("#isSwipeLockHidden").val() == "1") {
            $("div#parentDiv").css({ "transform": "translate(-350px,0px)" });
            $("#isSwipeLockHidden").val("0");
            dashboardFrameReLoad();
            e.preventDefault();
        } else if ($(e.target).is("#contentsLeftBox2 *") == true) {
            //スワイプ
            dashboardFrameReLoad();
        };
    });
});

//ToDo一覧遷移
function todoTransfer() {
    if ($("#isSwipeLockHidden").val() == "0" && $("#isToDoChipDrop").val() == "0") {
        //オーバーレイ表示
        $("#serverProcessOverlayBlack").css("display", "block");
        //アニメーション
        setTimeout(function () {
            $("#serverProcessIcon").addClass("show");
            $("#serverProcessOverlayBlack").addClass("open");
        }, 0);
        //ToDo一覧遷移ダミーボタン
        $("#toDoTitleButton").click();
    }
}

function todoPrev() {
    if ($("#isSwipeLockHidden").val() == "0" && $("#isToDoChipDrop").val() == "0") {
        //オーバーレイ表示
        $("#serverProcessOverlayBlack").css("display", "block");
        //アニメーション
        setTimeout(function () {
            $("#serverProcessIcon").addClass("show");
            $("#serverProcessOverlayBlack").addClass("open");
        }, 0);
        $("#toDoButtom").val(1);
        //ToDo一覧遷移ダミーボタン
        $("#toDoPrevButtom").click();
    }
}

function todoToday() {
    if ($("#isSwipeLockHidden").val() == "0" && $("#isToDoChipDrop").val() == "0") {
        //オーバーレイ表示
        $("#serverProcessOverlayBlack").css("display", "block");
        //アニメーション
        setTimeout(function () {
            $("#serverProcessIcon").addClass("show");
            $("#serverProcessOverlayBlack").addClass("open");
        }, 0);
        $("#toDoButtom").val(1);
        //ToDo一覧遷移ダミーボタン
        $("#toDoTodayButtom").click();
    }
}

function todoNext() {
    if ($("#isSwipeLockHidden").val() == "0" && $("#isToDoChipDrop").val() == "0") {
        //オーバーレイ表示
        $("#serverProcessOverlayBlack").css("display", "block");
        //アニメーション
        setTimeout(function () {
            $("#serverProcessIcon").addClass("show");
            $("#serverProcessOverlayBlack").addClass("open");
        }, 0);
        $("#toDoButtom").val(1);
        //ToDo一覧遷移ダミーボタン
        $("#toDoNextButtom").click();
    }
}

//ダッシュボード再読込
function dashboardFrameReLoad() {
    document.getElementById("dashboardFrame").contentWindow.location.reload(true);
}

//ダッシュボード読込終了
function dashboardFrameLoadEnd() {
    $("#loadingDashboard").hide(0);
}

//顧客詳細遷移（ToDoチップ）
function customerDetailTransfer(dlrCd, strCd, followupboxSeqno, salesStatus) {

    $("#selectDLRCD").val(dlrCd);
    $("#selectSTRCD").val(strCd);
    $("#selectFOLLOWUPBOXSEQNO").val(followupboxSeqno);
    $("#selectSALESSTATUS").val(salesStatus);
    $("#isContactHistoryTransfer").val("0");

    //再表示判定Function
    function custDetailTimerFunc() {

        $("#refreshButton").click();

        //繰り返し処理をする
        return true;
    }

    //タイマーセット
    commonRefreshTimer(custDetailTimerFunc);

    //オーバーレイ表示
    $("#serverProcessOverlayBlack").css("display", "block");
    //アニメーション
    setTimeout(function () {
        $("#serverProcessIcon").addClass("show");
        $("#serverProcessOverlayBlack").addClass("open");
    }, 0);

    //遷移用のボタン
    $("#CustDetailDummyButton").click();
}

//顧客詳細遷移(来店実績一覧)
function contactHistoryCustomerDetailTransfer(cstKind, customerClass, crcustID, strCD, followupboxSeqno, salesStatus) {

    $("#selectCSTKIND").val(cstKind);
    $("#selectCUSTOMERCLASS").val(customerClass);
    $("#selectCRCUSTID").val(crcustID);
    $("#selectSTRCD").val(strCD);
    $("#selectFOLLOWUPBOXSEQNO").val(followupboxSeqno);
    $("#selectSALESSTATUS").val(salesStatus);
    $("#isContactHistoryTransfer").val("1");

    //オーバーレイ表示
    $("#serverProcessOverlayBlack").css("display", "block");
    //アニメーション
    setTimeout(function () {
        $("#serverProcessIcon").addClass("show");
        $("#serverProcessOverlayBlack").addClass("open");
    }, 0);

    //遷移用のボタン
    $("#CustDetailDummyButton").click();
}

//***********************************************************

//サーバー処理中のオーバーレイ位置変更
function loadingIconWidthChange() {
    $("#serverProcessOverlayBlack").css("width", "1650px");
    $("#serverProcessIcon").css("left", "850px");
}

//サーバー処理中の非表示
function loadingIconDisplayNone() {
    $("#loadingSchedule2").css("display", "none");
    $("#contactHistoryNowLoading").val("0");
}

// *** スワイプ用 ****************************************************************** //
var swipeOptionsSchedule =
{
    swipeStatus: swipeStatusSchedule,
    threshold: 100
}

var swipeOptionsContactHistory =
{
    swipeStatus: swipeStatusContactHistory,
    threshold: 100
}

/**
* スワイプ登録.
* 
* @param {-} - -
* @return {-} -
* 
* @example 
*  -
*/

function swipeSettingSchedule() {
    var swipeTargetSchedule = $("#parentDiv");
    swipeTargetSchedule.swipe(swipeOptionsSchedule);
}

// 初期表示左スワイプ (来店実績一覧) //
function swipeStatusSchedule(event, phase, direction, distance) {
    if ($(event.target).is("#MessageListViewUpPanel *") == true) {
        return;
    }

    //左スワイプにする
    if (phase == "end" && direction == "right") {
        $("div#parentDiv").css({ "transform": "translate(0px,0px)" });
        $("#isSwipeLockHidden").val("1");
    }
    //左スワイプにする
    if (phase == "end" && direction == "left" && $("#isSwipeLockHidden").val() == "1") {
        $("div#parentDiv").css({ "transform": "translate(0px,0px)" });
        $("#isSwipeLockHidden").val("0");
        dashboardFrameReLoad();
    }
}

//来店実績一覧右スワイプ
function swipeStatusContactHistory(event, phase, direction, distance) {
    //右スワイプにする
    if (phase == "end" && direction == "right" && $("#isSwipeLockHidden").val() == "1") {
        $("div#parentDiv").css({ "transform": "translate(0px,0px)" });
        $("#isSwipeLockHidden").val("0");
        dashboardFrameReLoad();
    }
}

/*
来店実績用処理
*/
VisitSalesScript = function () {
    /**
    * @class 定数
    */
    var constants = {
        init: "CreatePriceConsultationWindow",
        request: "InsertPriceConsultationInfo",
        cancel: "CancelPriceConsultationInfo"
    }

    //来店実績更新中フラグ
    var isInUpdate = false;

    function update() {
        $("#VisitSalesTrigger").click();
    }


    return {
        update: update,
        isInUpdate: isInUpdate
    }
} ();

$(function () {
    //チップをタップした時のイベント処理
    $("#VisitActualRow").live("click", function () {
        if (VisitSalesScript.isInUpdate) {
            //更新中の場合は、処理終了
            return;
        }
        var visitActualRow;
        if (this.id == "VisitActualRow") {
            visitActualRow = $(this);
        } else {
            visitActualRow = $(this).parents("#VisitActualRow");
        }

        //顧客詳細（商談メモ）へ遷移
        contactHistoryCustomerDetailTransfer(visitActualRow.find(".CustomerSegment").val(),
		visitActualRow.find(".CustomerClass").val(),
		visitActualRow.find(".CustomerId").val(),
		visitActualRow.find(".Strcd").val(),
		visitActualRow.find(".FllwupBoxSeqno").val(),
		visitActualRow.find(".SalesStatus").val());
    });
})

//ToDoチップの表示対象の切り替え
function ToDoDispChange() {

    //表示対象日付の判定
    if ($("#isToDoBox").val() == "0") {
        if ($("#ToDoDispSegmentedButton").size() > 0) {
            var strToDo;
            strToDo = document.getElementById("ToDoDispSegmentedButton_0").checked;

            if (strToDo == true) {
                $(".completion").css("display", "none");
            } else {
                $(".completion").css("display", "block");
            }
            //受注前
            if ($("#toDoBoxOut").height() + 10 > $("#toDoBoxIn .todoChipBox").children("div .scroll-inner").height()) {
            //if ($("#toDoBoxIn .todoChipBox").height() > $("#toDoBoxIn .todoChipBox").children("div .scroll-inner").height()) {
                //表示対象が減って、スクロールしなくなった
                todoScrollPositionReset($("#toDoBoxIn .todoChipBox"));
            }
            //受注後
            if ($("#BookedAftertoDoBoxOut").height() + 10 > $("#BookedAftertoDoBoxIn .BookedAftertodoChipBox").children("div .scroll-inner").height()) {
            //if ($("#BookedAftertoDoBoxIn .BookedAftertodoChipBox").height() > $("#BookedAftertoDoBoxIn .BookedAftertodoChipBox").children("div .scroll-inner").height()) {
                //表示対象が減って、スクロールしなくなった
                todoScrollPositionReset($("#BookedAftertoDoBoxIn .BookedAftertodoChipBox"));
            }
            //納車後
            if ($("#DeliAftertoDoBoxOut").height() + 10 > $("#DeliAftertoDoBoxIn .DeliAftertodoChipBox").children("div .scroll-inner").height()) {
            //if ($("#DeliAftertoDoBoxIn .DeliAftertodoChipBox").height() > $("#DeliAftertoDoBoxIn .DeliAftertodoChipBox").children("div .scroll-inner").height()) {
                //表示対象が減って、スクロールしなくなった
                todoScrollPositionReset($("#DeliAftertoDoBoxIn .DeliAftertodoChipBox"));
            }
            //来店実績
            if ($("#VisitBoxOut").height() + 10 > $("#VisitBoxOut .todoBoxIn").children("div .scroll-inner").height()) {
            //if ($("#VisitBoxOut .todoBoxIn").height() > $("#VisitBoxOut .todoBoxIn").children("div .scroll-inner").height()) {
                //表示対象が減って、スクロールしなくなった
                todoScrollPositionReset($("#VisitBoxOut .todoBoxIn"));
            }

        }
    } else {
        $(".completion").css("display", "block");
    }

}

function todoScrollPositionReset(obj) {
    obj.children("div .scroll-inner").css({ "transform": "translate(0px, 0px)" });
}


//来店実績チップカラー取得
function getVisitSalesTipColor() {
    var rgba = $("#visitSalesTipColor").val();
    var rgbaArry = $("#visitSalesTipColor").val().split(",");
    cssText = "";
    cssText += rgba;
    return cssText;
}

//SCメイン(KPI)読み込み処理
$(function () {
    $("#processKpiFrame").bind("load", function () {
        processKpiFrameLoadEnd();
    });
});

//SCメイン(KPI)再読込
function processKpiFrameReLoad() {
    document.getElementById("processKpiFrame").contentWindow.location.reload(true);
}

//SCメイン(KPI)読込終了
function processKpiFrameLoadEnd() {
    $("#loadingProcessKpi").hide(0);
}

//$01 Add Start
//画面遷移用処理
function moveMainFrame(transitionsDiv, abnormalClassCD, abnormalItemCD) {

    //オーバーレイ表示
    $("#serverProcessOverlayBlack").css("display", "block");
    //アニメーション
    setTimeout(function () {
        $("#serverProcessIcon").addClass("show");
        $("#serverProcessOverlayBlack").addClass("open");
    }, 0);

    //引数をHidden項目に設定
    this_form.transitionsDiv.value = transitionsDiv;
    this_form.abnormalClassCD.value = abnormalClassCD;
    this_form.abnormalItemCD.value = abnormalItemCD;

    //画面遷移用ダミーボタン押下
    $("#moveMainFrameDummyButton").click();

}
//$01 Add End

// 2019/05/28 TS 舩橋 PostUAT-3098 カレンダーアプリ呼び出し変更 START 
function CallToiOSSchedule() {

    // 世界標準時(UTC) 1970-01-01 00:00:00 からのミリ秒（ms）を取得
    // month は 0-11 の範囲なので -1 をする
    nowUTCNum = Date.UTC($("#Yearhidden").val(), $("#Monthhidden").val() - 1, $("#Dayhidden").val());
    baseUTCNum = Date.UTC(2001, 0, 1);

    // ms を ｓ に変換 
    nowUTCNum = parseInt(nowUTCNum / 1000);
    baseUTCNum = parseInt(baseUTCNum / 1000);

    // iOSカレンダー標準時 2001-01-01 00:00:00 からの秒（s）を取得
    // 現在の日付（世界標準時からの（s））- iOSカレンダー標準時（世界標準時からの（s））
    selectUTCNum = nowUTCNum - baseUTCNum;

    // iOSカレンダーを起動 引数: iOSカレンダー標準時からの(s)
    location.href = "calshow:" + selectUTCNum;
}
// 2019/05/28 TS 舩橋 PostUAT-3098 カレンダーアプリ呼び出し変更 END 