//━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
//SC3080201.Occupation.js
//─────────────────────────────────────
//機能： 職業PopUp
//補足： 職業PopUpを開くタイミングにて遅延ロードする
//作成： 2014/04/21 TCS 市川 GTMCタブレット高速化対応（BTS-386）
//─────────────────────────────────────

//職業ポップアップ表示処理(サーバー処理後)
function occupationPopupOpen() {

    //--2013/11/29 TCS 市川 Aカード情報相互連携開発 START
    //スクロール設定
    $("#occupationPopOverForm_1").fingerScroll();
    //--2013/11/29 TCS 市川 Aカード情報相互連携開発 END

    $("#CustomerRelatedOccupationOtherIdHiddenField").val("");
    //ポップアップ表示
    $("#CustomerRelatedOccupationPopupArea").fadeIn(0);

    //共通読込みアニメーション戻し
    //--2013/11/29 TCS 市川 Aカード情報相互連携開発 START
    $("#processingServer").removeClass("occupationPopupLoadingAnimation").css("top", "");
    //--2013/11/29 TCS 市川 Aカード情報相互連携開発 END
    $("#registOverlayBlack").removeClass("popupLoadingBackgroundColor");
}
//職業ポップアップ非表示処理
function occupationPopupClose() {
    //ポップアップ非表示
    $("#CustomerRelatedOccupationPopupArea").fadeOut(300);
    setTimeout(function () {
        //強制的に1ページ目に
        setPopupOccupationPage('page1');
        //HTML削除
        $("#OccupationVisiblePanel").empty();
    }, 300);
}
//2012/03/08 TCS 山口 【SALES_2】性能改善 END

function setPopupOccupationPage(page, occupationId) {
    //スライド処理
    var leftpoint = 0;
    $("#CustomerRelatedOccupationPageArea").css({ "-webkit-transition": "transform 500ms ease-in-out 0" });
    if (page == "page1") {
        $("#CustomerRelatedOccupationOtherIdHiddenField").val("");
        $("#CustomerRelatedOccupationTitleLabel").text($("#OccupationPopuupTitlePage1").val());
        $("#CustomerRelatedOccupationPopupArea .btnL").hide();
        $("#CustomerRelatedOccupationPopupArea .btnR").hide();
        leftpoint = 0;
    } else {
        $("#CustomerRelatedOccupationOtherIdHiddenField").val(occupationId);
        $("#CustomerRelatedOccupationTitleLabel").text($("#OccupationPopuupTitlePage2").val());
        $("#CustomerRelatedOccupationPopupArea .btnL").show();
        $("#CustomerRelatedOccupationPopupArea .btnR").show();
        leftpoint = -370;
    }
    $("#CustomerRelatedOccupationPageArea").removeClass("page1 page2").addClass(page).one("webkitTransitionEnd", function () {
        $("#CustomerRelatedOccupationPageArea").css({ "-webkit-transition": "none" });
        $("#CustomerRelatedOccupationPageArea").removeClass(page);
        $("#CustomerRelatedOccupationPageArea").css({ "left": leftpoint });
    });
}

function checkOtherOccupation() {

    if ($("#CustomerRelatedOccupationOtherCustomTextBox").val() == "") {
        alert($("#OccupationOtherErrMsg").val());
        return false;
    }
    return true;
}

//初期設定
$(function () {
    //ポップアップクローズの監視
    //職業ポップアップ
    $(document.body).bind("mousedown touchstart", function (e) {
        if ($("#CustomerRelatedOccupationPopupArea").is(":visible") === false) return;
        //2012/03/08 TCS 山口 【SALES_2】性能改善 START
        if ($("#registOverlayBlack").hasClass("popupLoadingBackgroundColor") === true) return;
        //2012/03/08 TCS 山口 【SALES_2】性能改善 END
        if ($(e.target).is("#CustomerRelatedOccupationPopupArea, #CustomerRelatedOccupationPopupArea *") === false) {
            //2012/03/08 TCS 山口 【SALES_2】性能改善 START
            //画面初期化
            occupationPopupClose();
        }
    });
});
