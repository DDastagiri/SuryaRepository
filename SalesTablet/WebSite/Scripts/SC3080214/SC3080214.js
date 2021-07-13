/// <reference path="../jquery-1.4.4.js"/>
/// <reference path="../jquery.fingerscroll.js"/>
/// <reference path="Common.js"/>

//顧客メモポップアップ処理/////////////////////////
function setPopupCustomerMemoOpen() {
    //セッションを設定する
    $("#CustomerMemoEditOpenButton").click();

}

function commitCompleteOpenCustomerMemoEdit() {
//    //Iframe削除
//    $("#CustomerMemoIframe").remove();
//    //Iframe作成
//    var $iframe = $("<iframe id='CustomerMemoIframe' src='./SC3080204.aspx' width='1014px' height='645px' scrolling='no' frameborder='0' style='border:2px solid #666'></iframe>");
//    //タグ追加
//    $("#CustomerMemoEdit").append($iframe);
    
    //顧客メモをスライドインする
    $("#CustomerMemoEdit").fadeIn(300);
    
    //先頭のメモを選択状態にする
    SelectFirstMemo();
    
    //$("#CustomerMemoEdit").PageLoad();

}
////////////////////////////////////////////////////


//長谷川追加分
function setPopupOccupationPageOpen() {

    $("#CustomerRelatedOccupationOtherIdHiddenField").val("");
    $("#CustomerRelatedOccupationPopupArea").fadeIn(300);
}

//function setPopupOccupationPage(page, occupationId) {

//    $("#CustomerRelatedOccupationPageArea").removeClass("page1 page2").addClass(page);

//    if (page == "page1") {
//        $("#CustomerRelatedOccupationTitleLabel").text($("#OccupationPopupTitlePage1").val());
//        $("#CustomerRelatedOccupationOtherIdHiddenField").val("");
//        $("#CustomerRelatedOccupationPopupArea .btnL").hide(0);
//        $("#CustomerRelatedOccupationPopupArea .btnR").hide(0);
//    } else {
//        $("#CustomerRelatedOccupationTitleLabel").text($("#OccupationPopupTitlePage2").val());
//        $("#CustomerRelatedOccupationOtherIdHiddenField").val(occupationId);
//        $("#CustomerRelatedOccupationPopupArea .btnL").show(0);
//        $("#CustomerRelatedOccupationPopupArea .btnR").show(0);
//    }
//}
function setPopupOccupationPage(page, occupationId) {
    //スライド処理
    var leftpoint = 0;
    $("#CustomerRelatedOccupationPageArea").css({ "-webkit-transition": "transform 500ms ease-in-out 0" });
    if (page == "page1") {
        $("#CustomerRelatedOccupationOtherIdHiddenField").val("");
        $("#CustomerRelatedOccupationTitleLabel").text($("#OccupationPopupTitlePage1").val());
        $("#CustomerRelatedOccupationPopupArea .btnL").hide(0);
        $("#CustomerRelatedOccupationPopupArea .btnR").hide(0);
        leftpoint = 0;
    } else {
        $("#CustomerRelatedOccupationOtherIdHiddenField").val(occupationId);
        $("#CustomerRelatedOccupationTitleLabel").text($("#OccupationPopupTitlePage2").val());
        $("#CustomerRelatedOccupationPopupArea .btnL").show(0);
        $("#CustomerRelatedOccupationPopupArea .btnR").show(0);
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

function transitionFamilyCountBox(size) {

    $("#TriangulArrowDown").hide(0);
    $("#TriangulArrowUp").hide(0);

    if (size) {
        $("#FamilyCountBox").css({
            "-webkit-transition": "200ms linear",
            "height": "65px"
        }).one("webkitTransitionEnd", function () {
            $("#FamilyCountBox").css({ "-webkit-transition": "none" });
            $("#TriangulArrowUp").show(0);
        });
    } else {
        $("#FamilyCountBox").css({
            "-webkit-transition": "200ms linear",
            "height": "25px"
        }).one("webkitTransitionEnd", function () {
            $("#FamilyCountBox").css({ "-webkit-transition": "none" });
            $("#TriangulArrowDown").show(0);
        });
    }
}

function googleMapOpen() {
    //GoogleMap
//    var posX = 450;
//    var posY = 200;
//    var width = 520;
//    var height = 660;

    var ArrowDir = 1;
    var posX = 490;
    var posY = 220;
    var width = 500;
    var height = 657;
    //    var width = 500;
    //    var height = 640;
    var address = $("#customerAddressTextBox").val();
    
    if (address == "-") {
        return;
    }

    var query = "";
    query += "icrop:pmap";
    query += ":" + ArrowDir + ":";    
    query += ":" + posX + ":";
    query += ":" + posY + ":";
    query += ":" + width + ":";
    query += ":" + height + ":";
    query += ":" + address;

    location.href = query;
}

function photoSelectOpen() {
    //Photo
    var posX = 80;
    var posY = 150;
//    var file = $("#customerIdTextBox").val() + $("#faceFileNameTimeHiddenField").val();
    var file = $("#customerIdTextBox").val();
    var cbmethod = "CallBackCustomerPhoto";

    var query = "";
    query += "icrop:came"
    query += ":" + posX + ":";
    query += ":" + posY + ":";
    query += ":" + file + ":";
    query += ":" + cbmethod;

    location.href = query;
}

$(function () {

    bindFingerScroll();
    $(".scNscCurriculumListBox").fingerScroll();
    $(".scNscSelectionListBox").fingerScroll();

    //ポップアップクローズの監視
    $(document.body).bind("mousedown touchstart", function (e) {
        if ($("#CustomerRelatedOccupationPopupArea").is(":visible") === false) return;
        if ($(e.target).is("#CustomerRelatedOccupationPopupArea, #CustomerRelatedOccupationPopupArea *") === false) {
            $("#CustomerRelatedOccupationPopupArea").fadeOut(300);
            //画面初期化
            $("#CustomerRelatedOccupationCancelButton").click();
//            $("#CustomerRelatedOccupationPopupArea .btnL").hide(0);
//            $("#CustomerRelatedOccupationPopupArea .btnR").hide(0);

        }
    });

    $(document.body).bind("mousedown touchstart", function (e) {
        if ($("#CustomerRelatedFamilyPopupArea").is(":visible") === false) return;
        if ($(e.target).is("#CustomerRelatedFamilyPopupArea, #CustomerRelatedFamilyPopupArea *") === false) {
            g_familyPage = "page1"
            $("#CustomerRelatedFamilyPopupArea").fadeOut(300);
            //画面初期化
            $("#CustomerRelatedFamilyCancelButton").click();
        }
    });

    $(document.body).bind("mousedown touchstart", function (e) {
        if ($("#CustomerRelatedHobbyPopupArea").is(":visible") === false) return;
        if ($(e.target).is("#CustomerRelatedHobbyPopupArea, #CustomerRelatedHobbyPopupArea *") === false) {
            g_hobbyPage = "page1";
            $("#CustomerRelatedHobbyPopupArea").fadeOut(300);
            //画面初期化
            $("#CustomerRelatedHobbyPopupCancelButton").click();
        }
    });

    $(document.body).bind("mousedown touchstart", function (e) {
        if ($("#CustomerRelatedContactPopupArea").is(":visible") === false) return;
        if ($(e.target).is("#CustomerRelatedContactPopupArea, #CustomerRelatedContactPopupArea *") === false) {
            $("#CustomerRelatedContactPopupArea").fadeOut(300);
            //画面初期化
            $("#CustomerRelatedContactPopupCancelButton").click();
        }
    });

    $(document.body).bind("mousedown touchstart", function (e) {
        if ($("#CustomerMemoEdit").is(":visible") === false) return;
        if ($(e.target).is("#CustomerMemoEdit, #CustomerMemoEdit *") === false) {
            $("#CustomerMemoEdit").fadeOut(300);
        }
    });

    $(document.body).bind("mousedown touchstart", function (e) {
        if ($("#scNscSelectionWindownVehicleSelect").is(":visible") === false) return;
        if ($(e.target).is("#scNscSelectionWindownVehicleSelect, #scNscSelectionWindownVehicleSelect *") === false) {
            $("#scNscSelectionWindownVehicleSelect").fadeOut(300);
        }
    });
});

function bindFingerScroll() {
    $("#CustomerRelatedFamilyPageArea .popupScrollArea").fingerScroll();
}

function CallBackCustomerPhoto(iRc) {
    if (iRc == 1) {
        $("#customerInfoUpdateButton").click();
    }
}

function RefleshCustomerInfo() {
    $("#customerInfoUpdateButton").click();
}

//家族ポップアップ処理
function setPopupFamilyPageOpen() {
    $("#FamilyBirthdayListBirthdayDate_Row_0").val($("#birthdayTextBox").val());
    $("#CustomerRelatedFamilyPopupArea").fadeIn(300);
}

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
        $("#CustomerRelatedFamilyPopupTitleLabel").text($("#FamilyPopupTitlePage1").val());
        $("#CustomerRelatedFamilyPopupArea .btnR").show(0);
        leftpoint = 0;
    } else if (page == "page2") {
        if (prev == "page1") {
            page = "page2";
            g_familyRow = row;
            var relationNo = $("#FamilyBirthdayListRelationNoHidden_Row_" + row).val();
            $("#familyRelationship li").removeClass("familyRelationshipOn");
            $("#familyRelationshipList_No_" + relationNo).addClass("familyRelationshipOn")

            var other = $("#RelationOtherNoHidden").val();
            if (relationNo == other) {
                var word = $("#FamilyBirthdayListRelationLabel_Row_" + row).text();
                $("#familyRelationshipLabel_No_" + other).text(word);
                $("#FamilyOtherRelationshipTextBox").val(word);
            } else {
                $("#familyRelationshipLabel_No_" + other).text($("#RelationOtherWordHidden").val());
                $("#FamilyOtherRelationshipTextBox").val("");
                $("#FamilyOtherRelationshipTextBox").CustomTextBox("updateText", "");
            }
        } else {
            page = "page1";
        }
        $("#CustomerRelatedFamilyPopupTitleLabel").text($("#FamilyPopupTitlePage2").val());
        $("#CustomerRelatedFamilyPopupArea .btnR").hide(0);
        leftpoint = -320;
    } else if (page == "page3") {
        page = "page2";
        $("#familyOtherRelationshipNoHidden").val(row);
        $("#CustomerRelatedFamilyPopupTitleLabel").text($("#FamilyPopupTitlePage3").val());
        $("#CustomerRelatedFamilyPopupArea .btnR").show(0);
        leftpoint = -640;

    }

    $("#CustomerRelatedFamilyPageArea").removeClass("page1 page2 page3").addClass(page).one("webkitTransitionEnd", function () {
        $("#CustomerRelatedFamilyPageArea").css({ "-webkit-transition": "none" });
        $("#CustomerRelatedFamilyPageArea").removeClass(page);
        $("#CustomerRelatedFamilyPageArea").css({ "left": leftpoint });
    });
//    $("#CustomerRelatedFamilyPageArea").removeClass("page1 page2 page3").addClass(page);
//    $("#CustomerRelatedFamilyPageArea").css({ "-webkit-transition": "none" });
//    $("#CustomerRelatedFamilyPageArea").removeClass(page);
//    $("#CustomerRelatedFamilyPageArea").css({ "left": leftpoint });
    
}

function CancelCustomerRelatedFamily() {

    if (g_familyPage == "page1") {
        $("#CustomerRelatedFamilyPopupArea").fadeOut(300);
        //画面初期化
        $("#CustomerRelatedFamilyCancelButton").click();
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
            $("#FamilyBirthdayHidden_Row_" + i).val($("#FamilyBirthdayListBirthdayDate_Row_" + i).val());
        }
        return true;
    } else if (g_familyPage == "page3") {

        if ($("#FamilyOtherRelationshipTextBox").val() == "") {
            alert($("#RelationOtherErrMsgHidden").val());
            return false;
        }

        $("#FamilyBirthdayListRelationLabel_Row_" + g_familyRow).text($("#FamilyOtherRelationshipTextBox").val());
        $("#FamilyBirthdayListRelationNoHidden_Row_" + g_familyRow).val($("#RelationOtherNoHidden").val());
        $("#FamilyBirthdayListRelationOtherHidden_Row_" + g_familyRow).val($("#FamilyOtherRelationshipTextBox").val());

        setPopupFamilyPage("page1", g_familyPage);
        return false;
    }
}

function selectFamilyRelationship(relationNo) {

    $("#FamilyBirthdayListRelationLabel_Row_" + g_familyRow).text($("#familyRelationshipLabel_No_" + relationNo).text());
    $("#FamilyBirthdayListRelationNoHidden_Row_" + g_familyRow).val(relationNo);
    $("#FamilyBirthdayListRelationOtherHidden_Row_" + g_familyRow).val("");
    $("#FamilyOtherRelationshipTextBox").CustomTextBox("updateText", "");

    setPopupFamilyPage("page1", "page2");

}

function SelectFamilyCount(row) {

    $("#FamilyCount").val(row + 1);

    $("#FamilyCountBox li a").removeClass("selectedButton");
    $("#FamilyCountBox li a:eq(" + row + ")").addClass("selectedButton");

    $("#FamilyBirthdayListArea li").removeClass("displaynone FamilyBirthdayListAreaNoBorder");
    $("#FamilyBirthdayListArea li:eq(" + row + ")").addClass("FamilyBirthdayListAreaNoBorder");
    $("#FamilyBirthdayListArea li:gt(" + (row) + ")").addClass("displaynone");

}

//function editFamilyBirthday(row) {

//    $("#FamilyBirthdayHidden_Row_" + row).val($("#FamilyBirthdayListBirthdayDate_Row_" + row).val());
//}



//趣味関連
var g_hobbyPage = "page1";
var g_hobyRow = -1;

function setPopupHobbyPageOpen() {
    $("#CustomerRelatedHobbyPopupArea").fadeIn(300);
}

//function setCustomerRelatedHobbyPopupPage(page, row) {

//    g_hobbyPage = page;
//    g_hobyRow = row;

//    if ($("#CustomerRelatedHobbyPopupSelectButtonCheck_Row_" + row).val() == "1") {
//        g_hobbyPage = "page1";
//        selectCustomerRelatedHobbyPopupButton(row);
//        $("#CustomerRelatedHobbyPopupSelectButtonTitleLabel_Row_" + row).text($("#CustomerRelatedHobbyPopupOtherHobbyDefaultText").val());
//        $("#CustomerRelatedHobbyPopupOtherText").val("");
//        $("#CustomerRelatedHobbyPopupOtherText").CustomTextBox("updateText", "");
//        return;
//    }

//    if (page == "page1") {
//        $("#CustomerRelatedHobbyPopupTitleLabel").text($("#CustomerRelatedHobbyPopupTitlePage1").val());
//        g_hobyRow = -1;
//    } else if (page == "page2") {
//        $("#CustomerRelatedHobbyPopupTitleLabel").text($("#CustomerRelatedHobbyPopupTitlePage2").val());
//    }

//    $("#CustomerRelatedHobbyPopupPageWrap").removeClass("page1 page2").addClass(page);

//}
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

    var leftpoint = 0;
    $("#CustomerRelatedHobbyPopupPageWrap").css({ "-webkit-transition": "transform 500ms ease-in-out 0" });
    if (page == "page1") {
        $("#CustomerRelatedHobbyPopupTitleLabel").text($("#CustomerRelatedHobbyPopupTitlePage1").val());
        g_hobyRow = -1;
        leftpoint = 0;
    } else if (page == "page2") {
        $("#CustomerRelatedHobbyPopupTitleLabel").text($("#CustomerRelatedHobbyPopupTitlePage2").val());
        leftpoint = -370;
    }

    $("#CustomerRelatedHobbyPopupPageWrap").removeClass("page1 page2").addClass(page).one("webkitTransitionEnd", function () {
        $("#CustomerRelatedHobbyPopupPageWrap").css({ "-webkit-transition": "none" });
        $("#CustomerRelatedHobbyPopupPageWrap").removeClass(page);
        $("#CustomerRelatedHobbyPopupPageWrap").css({ "left": leftpoint });
    });
}

function cancelCustomerRelatedHobby() {

    if (g_hobbyPage == "page1") {
        $("#CustomerRelatedHobbyPopupArea").fadeOut(300);
        //画面初期化
        $("#CustomerRelatedHobbyPopupCancelButton").click();
    } else if (g_hobbyPage == "page2") {
        setCustomerRelatedHobbyPopupPage("page1");
    }
}

function registCustomerRelatedHobby() {

    if (g_hobbyPage == "page1") {
        return true;
    } else if (g_hobbyPage == "page2") {

        if ($("#CustomerRelatedHobbyPopupOtherText").val() == "") {
            alert($("#HobbyOtherErrorMessage").val());
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
    }else{
        $("#CustomerRelatedHobbyPopupSelectButtonPanel_Row_" + row).css({ "background-image": "url(" + $("#CustomerRelatedHobbyPopupSelectedButtonPath_Row_" + row).val() + ")" })
        $("#CustomerRelatedHobbyPopupSelectButtonTitleLabel_Row_" + row).addClass("selectedButton");
        $("#CustomerRelatedHobbyPopupSelectButtonCheck_Row_" + row).val("1");
    }

}

//連絡方法関連
function setPopupContactPageOpen() {
    $("#CustomerRelatedContactPopupArea").fadeIn(300);
}

function selectContactTool(tool) {

    var selectorLi = "";
    var selectorImage = "";
    var selectorHidden = "";
    switch (tool) {
        case 1:
            selectorLi = "#ContactToolMobileLI";
            selectorImage = "#ContactToolMobileImage";
            selectorHidden = "#ContactToolMobileHidden";
            break;
        case 2:
            selectorLi = "#ContactToolTelLI";
            selectorImage = "#ContactToolTelImage";
            selectorHidden = "#ContactToolTelHidden";
            break;
        case 3:
            selectorLi = "#ContactToolShortMessageServiceLI";
            selectorImage = "#ContactToolShortMessageServiceImage";
            selectorHidden = "#ContactToolShortMessageServiceHidden";
            break;
        case 4:
            selectorLi = "#ContactToolEmailLI";
            selectorImage = "#ContactToolEmailImage";
            selectorHidden = "#ContactToolEmailHidden";
            break;
        case 5:
            selectorLi = "#ContactToolDirectMailLI";
            selectorImage = "#ContactToolDirectMailImage";
            selectorHidden = "#ContactToolDirectMailHidden";
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
            selector = "#ContactWeek" + kind + "MonLI";
            break;
        case 2:
            selector = "#ContactWeek" + kind + "TueLI";
            break;
        case 3:
            selector = "#ContactWeek" + kind + "WedLI";
            break;
        case 4:
            selector = "#ContactWeek" + kind + "ThuLI";
            break;
        case 5:
            selector = "#ContactWeek" + kind + "FriLI";
            break;
        case 6:
            selector = "#ContactWeek" + kind + "SatLI";
            break;
        case 7:
            selector = "#ContactWeek" + kind + "SunLI";
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
            selector = "#ContactWeek" + kind + "ThuHidden";
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
    $("#CustomerRelatedContactPopupArea").fadeOut(300);
    //画面初期化
    $("#CustomerRelatedContactPopupCancelButton").click();
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

function reloadMemo() {
    $("#CustomerMemoEdit").fadeOut(300);
    $("#CustomerMemoEditCloseButton").click();
}
