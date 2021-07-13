/// <reference path="../jquery-1.5.2.js"/>
/// <reference path="../jquery.fingerscroll.js"/>
/**
* @fileOverview SC3010201_メインメニュー
*　１B対応
* @author TCS 寺本
* @version 1.0.0
*/

//プルダウンリフレッシュのレイアウトテンプレートをコピー
$(function () {
    var template$ = $("#pullDownToRefreshTemplate .pullDownToRefresh");
    //重要事項/RSS
    $("#messagePullDownToRefreshTemplateIn, #rssPullDownToRefreshTemplateIn").append(template$);

});

$(function () {

    //スクロールイベントを監視
    $("#messageInner02, #newsInner02").bind("move.fingerscroll", function (e, position) {


        $(".pullDownToRefresh", this).removeClass("step0 step1 step2");

        if (position.top >= 37) {
            $(".pullDownToRefresh", this).addClass("step1");
        } else {
            $(".pullDownToRefresh", this).addClass("step0");
        }
    });

    //スクロール終了イベントを監視
    $("#messageInner02, #newsInner02").bind("end.fingerscroll", function (e, position) {

        $(".pullDownToRefresh", this).removeClass("step0 step1 step2");

        if (position.top >= 37) {

            //更新中にする
            $(".pullDownToRefresh", this).addClass("step2");

            if ($(this).is("#messageInner02") === true) {
                //スクロール停止
                $("#messageInner02").mainMenuFingerScroll({ action: "stop", scrollMode: "all" });
                //更新処理
                $("#MessageListViewUpdateButton").click();
            } else {
                //スクロール停止
                $("#newsInner02").mainMenuFingerScroll({ action: "stop", scrollMode: "all" });
                //更新処理
                $("#RssListViewUpdateButton").click();
            }
        }
    });
});

//
function endRefreshMessage() {
    $("#messageInner02 .pullDownToRefresh").removeClass("step0 step1 step2").addClass("step0");
    $("#messageInner02").mainMenuFingerScroll({ action: "restart", scrollMode: "all" });
    //スワイプの設定
    swipeSetting();
}

function endRefreshRss() {
    $("#newsInner02 .pullDownToRefresh").removeClass("step0 step1 step2").addClass("step0");
    $("#newsInner02").mainMenuFingerScroll({ action: "restart", scrollMode: "all" });
    //スワイプの設定
    swipeSetting();
}