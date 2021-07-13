//━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
//SC3080201Contact.js
//─────────────────────────────────────
//機能： 連絡方法PopUp
//補足： 連絡方法PopUpを開くタイミングにて遅延ロードする
//作成： 2014/04/21 TCS 市川 GTMCタブレット高速化対応（BTS-386）
//─────────────────────────────────────

//連絡方法ポップアップ表示処理(サーバー処理後)
function contactPopupOpen() {
    //ポップアップ表示
    $("#CustomerRelatedContactPopupArea").fadeIn(0);

    //共通読込みアニメーション戻し
    $("#processingServer").removeClass("contactPopupLoadingAnimation");
    $("#registOverlayBlack").removeClass("popupLoadingBackgroundColor");

}

//連絡方法ポップアップ非表示処理
function contactPopupClose() {
    //ポップアップ非表示
    $("#CustomerRelatedContactPopupArea").fadeOut(300);
    setTimeout(function () {
        //HTML削除
        $("#ContactVisiblePanel").empty();
    }, 300);
}
//2012/03/08 TCS 山口 【SALES_2】性能改善 END

function selectContactTool(tool) {

    var selectorLi = "";
    var selectorImage = "";
    var selectorHidden = "";
    switch (tool) {
        case 1:
            selectorLi = "#ContactToolMobileLi";
            selectorImage = "#ContactToolMobileImage";
            selectorHidden = "#ContactToolMobileHidden";
            break;
        case 2:
            selectorLi = "#ContactToolTelLi";
            selectorImage = "#ContactToolTelImage";
            selectorHidden = "#ContactToolTelHidden";
            break;
        case 3:
            selectorLi = "#ContactToolSMSLi";
            selectorImage = "#ContactToolSMSImage";
            selectorHidden = "#ContactToolSMSHidden";
            break;
        case 4:
            selectorLi = "#ContactToolEmailLi";
            selectorImage = "#ContactToolEmailImage";
            selectorHidden = "#ContactToolEmailHidden";
            break;
        case 5:
            selectorLi = "#ContactToolDMLi";
            selectorImage = "#ContactToolDMImage";
            selectorHidden = "#ContactToolDMHidden";
            break;
    }

    if ($(selectorHidden).val() == "1") {
        $(selectorLi).removeClass("scNscPopUpContactSelectBtnMiddleOn");
        $(selectorHidden).val("0");
        $(selectorImage).removeClass("selected");
    } else {
        $(selectorLi).addClass("scNscPopUpContactSelectBtnMiddleOn");
        $(selectorHidden).val("1");
        $(selectorImage).addClass("selected");
    }
}

function selectContactWeek(kind, days) {

    for (i = 0; i < days.length; i++) {
        var selector = GetWeekSelector(kind, days[i]);
        var selectorHidden = GetWeekSelectorHidden(kind, days[i]);

        if ($(selectorHidden).val() == "1") {
            $(selector).removeClass("scNscPopUpDaySelectBtnSmallOn");
            $(selectorHidden).val("0");
        } else {
            $(selector).addClass("scNscPopUpDaySelectBtnSmallOn");
            $(selectorHidden).val("1");
        }
    }
}


function selectContactWeekday(kind) {
    var delDays = [6, 7];
    for (i = 0; i < delDays.length; i++) {
        var selector = GetWeekSelector(kind, delDays[i])
        var selectorHidden = GetWeekSelectorHidden(kind, delDays[i]);
        $(selector).removeClass("scNscPopUpDaySelectBtnSmallOn");
        $(selectorHidden).val("0");
    }

    var selDays = [1, 2, 3, 4, 5];
    for (j = 0; j < selDays.length; j++) {
        var selectorHidden = GetWeekSelectorHidden(kind, selDays[j]);
        $(selectorHidden).val("0");
    }

    selectContactWeek(kind, selDays)
}

function selectContactWeekend(kind) {

    var delDays = [1, 2, 3, 4, 5];
    for (i = 0; i < delDays.length; i++) {
        var selector = GetWeekSelector(kind, delDays[i])
        var selectorHidden = GetWeekSelectorHidden(kind, delDays[i])
        $(selector).removeClass("scNscPopUpDaySelectBtnSmallOn");
        $(selectorHidden).val("0");
    }

    var selDays = [6, 7];
    for (j = 0; j < selDays.length; j++) {
        var selectorHidden = GetWeekSelectorHidden(kind, selDays[j])
        $(selectorHidden).val("0");
    }

    selectContactWeek(kind, selDays)
}

function GetWeekSelector(kind, day) {

    var selector = "";
    switch (day) {
        case 1:
            selector = "#ContactWeek" + kind + "MonLi";
            break;
        case 2:
            selector = "#ContactWeek" + kind + "TueLi";
            break;
        case 3:
            selector = "#ContactWeek" + kind + "WedLi";
            break;
        case 4:
            selector = "#ContactWeek" + kind + "TurLi";
            break;
        case 5:
            selector = "#ContactWeek" + kind + "FriLi";
            break;
        case 6:
            selector = "#ContactWeek" + kind + "SatLi";
            break;
        case 7:
            selector = "#ContactWeek" + kind + "SunLi";
            break;
    }
    return selector;
}

function GetWeekSelectorHidden(kind, day) {

    var selector = "";
    switch (day) {
        case 1:
            selector = "#ContactWeek" + kind + "MonHidden";
            break;
        case 2:
            selector = "#ContactWeek" + kind + "TueHidden";
            break;
        case 3:
            selector = "#ContactWeek" + kind + "WedHidden";
            break;
        case 4:
            selector = "#ContactWeek" + kind + "TurHidden";
            break;
        case 5:
            selector = "#ContactWeek" + kind + "FriHidden";
            break;
        case 6:
            selector = "#ContactWeek" + kind + "SatHidden";
            break;
        case 7:
            selector = "#ContactWeek" + kind + "SunHidden";
            break;
    }
    return selector;
}

function selectContactTime(kind, row) {

    var li = $("#ContactTime" + kind + "Li_Row_" + row);
    var hidden = $("#ContactTime" + kind + "Hidden_Row_" + row);
    if (hidden.val() == "1") {
        li.removeClass("scNscPopUpContactSelectBtnMiddleOn");
        hidden.val("0");
    } else {
        li.addClass("scNscPopUpContactSelectBtnMiddleOn");
        hidden.val("1");
    }
}

function cancelContact() {
    //2012/03/08 TCS 山口 【SALES_2】性能改善 START
    contactPopupClose();
    //$("#CustomerRelatedContactPopupArea").fadeOut(300);
    ////画面初期化
    //$("#CustomerRelatedContactPopupCancelButton").click();
    //2012/03/08 TCS 山口 【SALES_2】性能改善 END
}

function registContact() {

    var days = [1, 2, 3, 4, 5, 6, 7];

    for (i = 0; i < days.length; i++) {
        if ($(GetWeekSelectorHidden(1, days[i])).val() == "1" && $(GetWeekSelectorHidden(2, days[i])).val() == "1") {
            alert($("#ContactErrMsg").val());
            return false;
        }
    }
    return true;
}

//初期設定
$(function () {
    //ポップアップクローズの監視
    //連絡方法ポップアップ
    $(document.body).bind("mousedown touchstart", function (e) {
        if ($("#CustomerRelatedContactPopupArea").is(":visible") === false) return;
        //2012/03/08 TCS 山口 【SALES_2】性能改善 START
        if ($("#registOverlayBlack").hasClass("popupLoadingBackgroundColor") === true) return;
        //2012/03/08 TCS 山口 【SALES_2】性能改善 END
        if ($(e.target).is("#CustomerRelatedContactPopupArea, #CustomerRelatedContactPopupArea *") === false) {
            //2012/03/08 TCS 山口 【SALES_2】性能改善 START
            //画面初期化
            contactPopupClose();
            //$("#CustomerRelatedContactPopupArea").fadeOut(300);
            ////画面初期化
            //$("#CustomerRelatedContactPopupCancelButton").click();
            //2012/03/08 TCS 山口 【SALES_2】性能改善 END
        }
    });
});
