//━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
//SC3080202.LastCr.js
//─────────────────────────────────────
//機能： 顧客詳細(商談情報)
//補足： 
//作成： 2011/11/24 TCS 小野
//更新： 2012/01/26 TCS 山口 【SALES_1B】
//更新： 2014/02/12 TCS 山口 受注後フォロー機能開発
//更新： 2014/04/21 TCS 市川 GTMCタブレット高速化対応（BTS-386）
//─────────────────────────────────────

/// <reference path="../jquery.js"/>
/// <reference path="../jquery.fingerscroll.js"/>


/**
* DOMロード時の処理
*/
$(function () {

    //2012/03/27 TCS 松野 【SALES_2】 START
    //スクロール設定
    //$(".activityPopScrollWrap").fingerScroll();
    //2012/03/27 TCS 松野 【SALES_2】 END

    //2014/04/21 TCS市川 GTMCタブレット高速化対応 DELETE

    //ポップアップクローズの監視
    $(document.body).bind("mousedown touchstart", function (e) {
        if ($("#activityPop_content").is(":visible") === false) return;

        //2012/03/27 TCS 松野 【SALES_2】 START
        if ($("#registOverlayBlack").hasClass("popupLoadingBackgroundColor") === true) return;
        //2012/03/27 TCS 松野 【SALES_2】 END

        if ($(e.target).is("#activityPop_content, #activityPop_content *, #NewActivityLabel") === false) {
            $("#activityPop_content").fadeOut(300);

            //2012/03/27 TCS 松野 【SALES_2】 START
            $("#activityPopPanel").empty();
            //2012/03/27 TCS 松野 【SALES_2】 END

        }
    });

    // 最新活動リストクリック処理
    $("#activityPop_content .popWind .dataWind1 ul li.activityListItem a").live("click", function (e) {
        $("#selFllwupboxSeqnoHidden").val($(this).parent("li.activityListItem").eq(0).children(":nth-child(3)").val());
        $("#selFllwupboxStrcdHidden").val($(this).parent("li.activityListItem").eq(0).children(":nth-child(4)").val());
        SC3080201.showLoding();
        $("#commitActivityButtonDummy").click();
    });

});

function newActivityChange(seqno) {
    $("#activityPop_content .popWind .dataWind1 ul li.activityListItem a").parent().parent().children(":nth-child(1)").children(":nth-child(3)").val(seqno);
    $("#selFllwupboxSeqnoHidden").val(seqno)
}


//イベント
// 最新活動のスタイル初期設定
function activityEventStyleDisplay() {
    $("#activityPop_content .popWind .dataWind1 ul li.activityListItem").each(function () {

        //選択活動はチェックボックスありのblue
        if ($(this).children(":nth-child(3)").val() == $("#selFllwupboxSeqnoHidden").val()) {
            //選択状態
            $(this).addClass("TebleBoxCheck");
        } else if ($(this).children(":nth-child(2)").val() == "True") {
            //それ以外
            $(this).addClass("FontBlack");
        }

        //2014/02/12 TCS 山口 受注後フォロー機能開発 START
        //Ellipsis設定
        $(this).find(".ellipsis").CustomLabel({ 'useEllipsis': 'true' });
        //2014/02/12 TCS 山口 受注後フォロー機能開発 END
    });
    return false;
}

//2012/03/27 TCS 松野 【SALES_2】 START
//活動一覧ポップアップ終了処理
function showPopUpActivityEnd() {
    //スクロール設定
    $(".activityPopScrollWrap").fingerScroll();
    //最新活動のスタイル初期設定
    activityEventStyleDisplay();
    //共通読込みアニメーション変更
    $("#processingServer").removeClass("activityPopupLoadingAnimation");
    $("#registOverlayBlack").removeClass("popupLoadingBackgroundColor");
}
//2012/03/27 TCS 松野 【SALES_2】 END
