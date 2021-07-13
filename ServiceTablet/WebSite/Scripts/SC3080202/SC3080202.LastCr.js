/// <reference path="../jquery.js"/>
/// <reference path="../jquery.fingerscroll.js"/>


/**
* DOMロード時の処理
*/
$(function () {

    //スクロール設定
    $(".activityPopScrollWrap").fingerScroll();

    //ポップアップ表示処理
    $("#NewActivityLabel").bind("click", function (e) {

        //ポップアップフェードイン
        $("#activityPop_content").fadeIn(300);

    });

    //ポップアップクローズの監視
    $(document.body).bind("mousedown touchstart", function (e) {
        if ($("#activityPop_content").is(":visible") === false) return;
        if ($(e.target).is("#activityPop_content, #activityPop_content *, #NewActivityLabel") === false) {
            $("#activityPop_content").fadeOut(300);
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

    });
    return false;
}
