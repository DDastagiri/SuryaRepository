//------------------------------------------------------------------------------
//SC3320101.PullDownRefresh.js
//---------------------------------------------------------
//機能：メイン画面処理
//作成：2014/08/14 TMEJ 丁 メインメニュー(移動マン)
//---------------------------------------------------------


//プルダウンリフレッシュのレイアウトテンプレートをコピー
$(function () {
    var template$ = $("#pullDownToRefreshTemplate .pullDownToRefresh");
    //重要事項/RSS
    $("#PullDownToRefreshDiv").append(template$);

});

$(function () {

    //スクロールイベントを監視
    /*$("#VisitInfoContents").bind("move.fingerscroll", function (e, position) {

        if (position.top >= 60) {
            $(".pullDownToRefresh").removeClass("step0").addClass("step1");
        } else {
            $(".pullDownToRefresh").removeClass("step1").addClass("step0");
        }
    });*/

    //スクロール終了イベントを監視
    $("#VisitInfoContents").bind("end.fingerscroll", function (e, position) {

        if ((position.top >= 60)&&(!gEditeFlg)&&(!gSearchFlg)) {
            //更新中にする
            //            $(".pullDownToRefresh").removeClass("step1").addClass("step2");
            $(".pullDownToRefresh").removeClass("step0").addClass("step2");

            //スクロール停止
            $("#VisitInfoContents").SC3320101fingerScroll({ action: "stop" });

            //更新処理
            PullDownRefresh();
        }
    });
});

/**
* 画面更新後の処理.
* @return {void}
*/
function endRefresh() {
    $(".pullDownToRefresh").removeClass("step2").addClass("step0");
    $("#VisitInfoContents").SC3320101fingerScroll({ action: "restart" });
}
