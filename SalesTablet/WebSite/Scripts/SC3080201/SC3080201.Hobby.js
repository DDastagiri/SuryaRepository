//━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
//SC3080201.Hobby.js
//─────────────────────────────────────
//機能： 趣味PopUp
//補足： 趣味PopUpを開くタイミングにて遅延ロードする
//作成： 2014/04/21 TCS 市川 GTMCタブレット高速化対応（BTS-386）
//─────────────────────────────────────

//趣味ポップアップ表示処理(サーバー処理後)
function hobbyPopupOpen() {
    
    //--2013/11/29 TCS 市川 Aカード情報相互連携開発 START
    //スクロール設定
    $("#CustomerRelatedHobbyPopupPage1").fingerScroll();
    //--2013/11/29 TCS 市川 Aカード情報相互連携開発 END

    //ポップアップ表示
    $("#CustomerRelatedHobbyPopupArea").fadeIn(0);

    //共通読込みアニメーション戻し
    //--2013/11/29 TCS 市川 Aカード情報相互連携開発 START
    $("#processingServer").removeClass("hobbyPopupLoadingAnimation").css("top", "");
    //--2013/11/29 TCS 市川 Aカード情報相互連携開発 END
    $("#registOverlayBlack").removeClass("popupLoadingBackgroundColor");
}

//趣味ポップアップ非表示処理
function hobbyPopupClose() {
    //ポップアップ非表示
    $("#CustomerRelatedHobbyPopupArea").fadeOut(300);
    setTimeout(function () {
        //強制的に1ページ目に
        setCustomerRelatedHobbyPopupPage("page1");
        //HTML削除
        $("#HobbyVisiblePanel").empty();
    }, 300);
}

function setCustomerRelatedHobbyPopupPage(page, row) {
    //スライド処理
    g_hobbyPage = page;
    g_hobyRow = row;

    if ($("#CustomerRelatedHobbyPopupSelectButtonCheck_Row_" + row).val() == "1") {
        g_hobbyPage = "page1";
        selectCustomerRelatedHobbyPopupButton(row);
        $("#CustomerRelatedHobbyPopupSelectButtonTitleLabel_Row_" + row).text($("#CustomerRelatedHobbyPopupOtherHobbyDefaultText").val());
        $("#CustomerRelatedHobbyPopupOtherText").val("");
        $("#CustomerRelatedHobbyPopupOtherText").CustomTextBox("updateText", "");
        return;
    }

    // 2013/11/27 TCS 市川 Aカード情報相互連携開発 START
    var leftpoint = 0;
    $("#CustomerRelatedHobbyPopupPageArea").css({ "-webkit-transition": "transform 500ms ease-in-out 0" });
    if (page == "page1") {
        $("#CustomerRelatedHobbyPopupTitleLabel").text($("#CustomerRelatedHobbyPopupTitlePage1").val());
        g_hobyRow = -1;
        leftpoint = 0;
    } else if (page == "page2") {
        $("#CustomerRelatedHobbyPopupTitleLabel").text($("#CustomerRelatedHobbyPopupTitlePage2").val());
        leftpoint = -370;
    }

    $("#CustomerRelatedHobbyPopupPageArea").removeClass("page1 page2").addClass(page).one("webkitTransitionEnd", function () {
        $("#CustomerRelatedHobbyPopupPageArea").css({ "-webkit-transition": "none" });
        $("#CustomerRelatedHobbyPopupPageArea").removeClass(page);
        $("#CustomerRelatedHobbyPopupPageArea").css({ "left": leftpoint });
    });
    // 2013/11/27 TCS 市川 Aカード情報相互連携開発 END
}

function cancelCustomerRelatedHobby() {

    if (g_hobbyPage == "page1") {
        //2012/03/08 TCS 山口 【SALES_2】性能改善 START
        hobbyPopupClose();
        //$("#CustomerRelatedHobbyPopupArea").fadeOut(300);
        ////画面初期化
        //$("#CustomerRelatedHobbyPopupCancelButton").click();
        //2012/03/08 TCS 山口 【SALES_2】性能改善 END
    } else if (g_hobbyPage == "page2") {
        setCustomerRelatedHobbyPopupPage("page1");
    }
}

function registCustomerRelatedHobby() {

    if (g_hobbyPage == "page1") {
        return true;
    } else if (g_hobbyPage == "page2") {

        if ($("#CustomerRelatedHobbyPopupOtherText").val() == "") {
            alert($("#HobbyOthererrMsg").val());
            return false;
        }

        $("#CustomerRelatedHobbyPopupSelectButtonTitleLabel_Row_" + g_hobyRow).text($("#CustomerRelatedHobbyPopupOtherText").val());
        $("#CustomerRelatedHobbyPopupOtherHiddenField").val($("#CustomerRelatedHobbyPopupOtherText").val());
        selectCustomerRelatedHobbyPopupButton(g_hobyRow);
        setCustomerRelatedHobbyPopupPage("page1");
        return false;
    }
}

function selectCustomerRelatedHobbyPopupButton(row) {

    if ($("#CustomerRelatedHobbyPopupSelectButtonCheck_Row_" + row).val() == "1") {
        $("#CustomerRelatedHobbyPopupSelectButtonPanel_Row_" + row).css({ "background-image": "url(" + $("#CustomerRelatedHobbyPopupNotSelectedButtonPath_Row_" + row).val() + ")" })
        $("#CustomerRelatedHobbyPopupSelectButtonTitleLabel_Row_" + row).removeClass("selectedButton");
        $("#CustomerRelatedHobbyPopupSelectButtonCheck_Row_" + row).val("0");
    } else {
        $("#CustomerRelatedHobbyPopupSelectButtonPanel_Row_" + row).css({ "background-image": "url(" + $("#CustomerRelatedHobbyPopupSelectedButtonPath_Row_" + row).val() + ")" })
        $("#CustomerRelatedHobbyPopupSelectButtonTitleLabel_Row_" + row).addClass("selectedButton");
        $("#CustomerRelatedHobbyPopupSelectButtonCheck_Row_" + row).val("1");
    }

}


//初期設定
$(function () {
    //ポップアップクローズの監視
    //趣味ポップアップ
    $(document.body).bind("mousedown touchstart", function (e) {
        if ($("#CustomerRelatedHobbyPopupArea").is(":visible") === false) return;
        //2012/03/08 TCS 山口 【SALES_2】性能改善 START
        if ($("#registOverlayBlack").hasClass("popupLoadingBackgroundColor") === true) return;
        //2012/03/08 TCS 山口 【SALES_2】性能改善 END
        if ($(e.target).is("#CustomerRelatedHobbyPopupArea, #CustomerRelatedHobbyPopupArea *") === false) {
            //2012/03/08 TCS 山口 【SALES_2】性能改善 START
            g_hobbyPage = "page1";
            //画面初期化
            hobbyPopupClose();
            //$("#CustomerRelatedHobbyPopupArea").fadeOut(300);
            ////画面初期化
            //$("#CustomerRelatedHobbyPopupCancelButton").click();
            //2012/03/08 TCS 山口 【SALES_2】性能改善 END
        }
    });
});
