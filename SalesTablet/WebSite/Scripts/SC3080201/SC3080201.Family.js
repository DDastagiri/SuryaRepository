//━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
//SC3080201.Family.js
//─────────────────────────────────────
//機能： 家族編集PopUp
//補足： 家族編集PopUpを開くタイミングにて遅延ロードする
//作成： 2014/04/21 TCS 市川 GTMCタブレット高速化対応（BTS-386）
//─────────────────────────────────────

function transitionFamilyCountBox(size) {

    $("#TriangulArrowDown").hide();
    $("#TriangulArrowUp").hide();

    if (size) {
        $("#FamilyCountBox").css({
            "-webkit-transition": "200ms linear",
            "height": "65px"
        }).one("webkitTransitionEnd", function () {
            $("#FamilyCountBox").css({ "-webkit-transition": "none" });
            $("#TriangulArrowUp").show();
        });
    } else {
        $("#FamilyCountBox").css({
            "-webkit-transition": "200ms linear",
            "height": "25px"
        }).one("webkitTransitionEnd", function () {
            $("#FamilyCountBox").css({ "-webkit-transition": "none" });
            $("#TriangulArrowDown").show();
        });
    }
}


//家族ポップアップ表示処理(サーバー処理後)
function familyPopupOpen() {
    //本人誕生日設定
    $("#familyBirthdayListBirthdayDate_Row_0").val($("#customerBirthday").val());

    //FingerScroll設定
    bindFingerScroll();

    //ポップアップ表示
    $("#CustomerRelatedFamilyPopupArea").fadeIn(0);

    //共通読込みアニメーション戻し
    $("#processingServer").removeClass("familyPopupLoadingAnimation");
    $("#registOverlayBlack").removeClass("popupLoadingBackgroundColor");

}
//家族ポップアップ非表示処理
function familyPopupClose() {
    //ポップアップ非表示
    $("#CustomerRelatedFamilyPopupArea").fadeOut(300);
    setTimeout(function () {
        //強制的に1ページ目に
        g_familyRow = -1;
        setPopupFamilyPage("page1", g_familyPage);
        //HTML削除
        $("#FamilyVisiblePanel").empty();
    }, 300);
}
//2012/03/08 TCS 山口 【SALES_2】性能改善 END

var g_familyPage = "page1";
var g_familyRow = -1;
function setPopupFamilyPage(page, prev, row) {
    //スライド処理
    var leftpoint = 0;
    $("#CustomerRelatedFamilyPageArea").css({ "-webkit-transition": "transform 500ms ease-in-out 0" });

    g_familyPage = page;

    if (page == "page1") {
        if (prev == "page3") {
            page = "page3";
        }
        $("#CustomerRelatedFamilyPopUpTitleLabel").text($("#FamilyPopuupTitlePage1").val());
        $("#CustomerRelatedFamilyPopupArea .btnR").show();
        leftpoint = 0;
    } else if (page == "page2") {
        if (prev == "page1") {
            page = "page2";
            g_familyRow = row;
            var relationNo = $("#familyBirthdayListRelationNoHidden_Row_" + row).val();
            $("#familyRelationship li").removeClass("familyRelationshipOn");
            $("#familyRelationshipList_No_" + relationNo).addClass("familyRelationshipOn")

            var other = $("#RelationOtherNoHidden").val();
            if (relationNo == other) {
                var word = $("#familyBirthdayListRelationLabel_Row_" + row).text();
                $("#familyRelationshipLabel_No_" + other).text(word);
                $("#familyOtherRelationshipTextBox").val(word);
            } else {
                $("#familyRelationshipLabel_No_" + other).text($("#RelationOtherWordHidden").val());
                $("#familyOtherRelationshipTextBox").val("");
                $("#familyOtherRelationshipTextBox").CustomTextBox("updateText", "");
            }
        } else {
            page = "page1";
        }
        $("#CustomerRelatedFamilyPopUpTitleLabel").text($("#FamilyPopuupTitlePage2").val());
        $("#CustomerRelatedFamilyPopupArea .btnR").hide();
        leftpoint = -320;
    } else if (page == "page3") {
        page = "page2";
        $("#familyOtherRelationshipNoHidden").val(row);
        $("#CustomerRelatedFamilyPopUpTitleLabel").text($("#FamilyPopuupTitlePage3").val());
        $("#CustomerRelatedFamilyPopupArea .btnR").show();
        leftpoint = -640;

    }

    $("#CustomerRelatedFamilyPageArea").removeClass("page1 page2 page3").addClass(page).one("webkitTransitionEnd", function () {
        $("#CustomerRelatedFamilyPageArea").css({ "-webkit-transition": "none" });
        $("#CustomerRelatedFamilyPageArea").removeClass(page);
        $("#CustomerRelatedFamilyPageArea").css({ "left": leftpoint });
    });
}

function CancelCustomerRelatedFamily() {

    if (g_familyPage == "page1") {
        familyPopupClose();
    } else if (g_familyPage == "page2") {
        g_familyRow = -1;
        setPopupFamilyPage("page1", g_familyPage);
    } else if (g_familyPage == "page3") {
        $("#familyOtherRelationshipNoHidden").val("");
        setPopupFamilyPage("page2", g_familyPage);
    }
}

function RegistCustomerRelatedFamily() {
    if (g_familyPage == "page1") {
        for (i = 0; i < 10; i++) {
            $("#familyBirthdayHidden_Row_" + i).val($("#familyBirthdayListBirthdayDate_Row_" + i).val());
        }
        return true;
    } else if (g_familyPage == "page3") {

        if ($("#familyOtherRelationshipTextBox").val() == "") {
            alert($("#RelationOtherErrMsgHidden").val());
            return false;
        }

        $("#familyBirthdayListRelationLabel_Row_" + g_familyRow).text($("#familyOtherRelationshipTextBox").val());
        $("#familyBirthdayListRelationNoHidden_Row_" + g_familyRow).val($("#RelationOtherNoHidden").val());
        $("#familyBirthdayListRelationOtherHidden_Row_" + g_familyRow).val($("#familyOtherRelationshipTextBox").val());

        setPopupFamilyPage("page1", g_familyPage);
        return false;
    }
}

function selectFamilyRelationship(relationNo) {

    $("#familyBirthdayListRelationLabel_Row_" + g_familyRow).text($("#familyRelationshipLabel_No_" + relationNo).text());
    $("#familyBirthdayListRelationNoHidden_Row_" + g_familyRow).val(relationNo);
    $("#familyBirthdayListRelationOtherHidden_Row_" + g_familyRow).val("");
    $("#familyOtherRelationshipTextBox").CustomTextBox("updateText", "");

    setPopupFamilyPage("page1", "page2");

}

function SelectFamilyCount(row) {

    $("#FamilyCount").val(row + 1);

    $("#FamilyCountBox li a").removeClass("selectedButton");
    $("#FamilyCountBox li a:eq(" + row + ")").addClass("selectedButton");

    $("#familyBirthdayListArea li").removeClass("displaynone familyBirthdayListAreaNoBorder");
    $("#familyBirthdayListArea li:eq(" + row + ")").addClass("familyBirthdayListAreaNoBorder");
    $("#familyBirthdayListArea li:gt(" + (row) + ")").addClass("displaynone");

}


//初期設定
$(function () {
    //ポップアップクローズの監視
    //家族ポップアップ
    $(document.body).bind("mousedown touchstart", function (e) {
        if ($("#CustomerRelatedFamilyPopupArea").is(":visible") === false) return;
        //2012/03/08 TCS 山口 【SALES_2】性能改善 START
        if ($("#registOverlayBlack").hasClass("popupLoadingBackgroundColor") === true) return;
        //2012/03/08 TCS 山口 【SALES_2】性能改善 END
        if ($(e.target).is("#CustomerRelatedFamilyPopupArea, #CustomerRelatedFamilyPopupArea *") === false) {
            //2012/03/08 TCS 山口 【SALES_2】性能改善 START
            g_familyPage = "page1"
            //画面初期化
            familyPopupClose();
            //$("#CustomerRelatedFamilyPopupArea").fadeOut(300);
            ////画面初期化
            //$("#CustomerRelatedFamilyCancelButton").click();
            //2012/03/08 TCS 山口 【SALES_2】性能改善 END
        }
    });
});
