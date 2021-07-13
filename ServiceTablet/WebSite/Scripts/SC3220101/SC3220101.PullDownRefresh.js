//------------------------------------------------------------------------------
//SC3220101.PullDownRefresh.js
//------------------------------------------------------------------------------
//機能：メインメニュー（SAステータスマネジメント）[フリック＆リリース更新処理]
//補足：
//作成：2012/05/30 日比野
//更新：2012/11/13 TMEJ 丁 【A.STEP2】次世代サービス アクティブインジゲータ対応 No.1
//------------------------------------------------------------------------------


//プルダウンリフレッシュのレイアウトテンプレートをコピー
$(function () {
    var template$ = $("#pullDownToRefreshTemplate .pullDownToRefresh");
    //重要事項/RSS
    $("#PullDownToRefreshDiv").append(template$);

});

$(function () {

    //スクロールイベントを監視
    $("#scrollDiv").bind("move.fingerscroll", function (e, position) {

        if (position.top >= 60) {
            $(".pullDownToRefresh").removeClass("step0").addClass("step1");
        } else {
            $(".pullDownToRefresh").removeClass("step1").addClass("step0");
        }
    });

    //スクロール終了イベントを監視
    $("#scrollDiv").bind("end.fingerscroll", function (e, position) {

        if (position.top >= 60) {
            //更新中にする
            $(".pullDownToRefresh").removeClass("step1").addClass("step2");

            //スクロール停止
            $("#scrollDiv").mainMenuFingerScroll({ action: "stop" });

            // 更新：2012/11/13 TMEJ 丁 【A.STEP2】次世代サービス アクティブインジゲータ対応 No.1 START
            commonRefreshTimer(
                        function () {
                            //リロード処理
                            location.replace(location.href);
                        }
                    );
            // 更新：2012/11/13 TMEJ 丁 【A.STEP2】次世代サービス アクティブインジゲータ対応 No.1 END

            //更新処理
            getServerData();
        }
    });
});

/**
 * 画面更新後の処理.
 * @return {void}
 */
function endRefresh() {

    $(".pullDownToRefresh").removeClass("step2").addClass("step0");
    $("#scrollDiv").mainMenuFingerScroll({ action: "restart"});
    // 更新：2012/11/13 TMEJ 丁 【A.STEP2】次世代サービス アクティブインジゲータ対応 No.1 START
    //現在時、以前のタイマーを無視する
    commonClearTimer();
    // 更新：2012/11/13 TMEJ 丁 【A.STEP2】次世代サービス アクティブインジゲータ対応 No.1 END
}
