/// <reference path="jquery.js"/>
/// <reference path="eCRB.js"/>
/// <reference path="eCRB.ui.js"/>

/****************************************************************************

マスターページに関する処理のjQUERY拡張

****************************************************************************/
(function (window) {
    $.extend({ master: {

        blinkIcropLogoTimer: null,

        //i-CROPアイコン点滅開始
        blinkStartIcropLogo: function() {
            this.blinkIcropLogoTimer = setInterval(function() {
                 $("#mstpg_icropLogo").is(":hidden") ? $("#mstpg_icropLogo").fadeIn(200) : $("#mstpg_icropLogo").fadeOut(200);
            }, 200);
        },

        //i-CROPアイコン点滅終了
        blinkEndIcropLogo: function() {
            if (this.blinkIcropLogoTimer) clearInterval(this.blinkIcropLogoTimer);
            $("#mstpg_icropLogo").show(0);
        },

        OpenLoadingScreen: function () {
            $("#MstPG_LoadingScreen").css({ "width": $(window).width() + "px", "height": $(window).height() + "px" });
            setTimeout(function () {
                $("#MstPG_LoadingScreen").css({ "display": "table"});
            }, 0);
        },

        CloseLoadingScreen: function() {
            $("#MstPG_LoadingScreen").css({ "display": "none" });
        }
    }});
})(window);


/****************************************************************************

キーボード制御

****************************************************************************/
$(function () {

    $(document).keydown(function (e) {
        
        if (e.which != 13) return true; //13:Enterキー(Goボタン)
        var tclass = (e.target.className).toUpperCase();

        if (tclass == "VALIDKBPROTECT") {
            e.target.blur();
            return false;
        }

        var tagName = (e.target.tagName).toUpperCase();
        if (tagName != "INPUT" && tagName != "SELECT") return true;
        if (tagName == "INPUT") {
            var type = (e.target.type).toUpperCase();
            if (type == "SEARCH" || type == "PASSWORD") return true;
        }

        if (tclass == "UNVALIDKBPROTECT") return true;
        e.target.blur();
        return false;
    });

});



/****************************************************************************

画面タイトル⇔スタッフ情報切り替え用

****************************************************************************/
$(function () {

    //画面タイトル、スタッフ情報切り替え処理
    function change_TitleLabel() {
        $("#MstPG_TitleLabel").toggle();
        $("#MstPG_StaffInfo").toggle();
    }
    //DOMロード時の初期処理
    $("#MstPG_TitleLabel").show(0);
    $("#MstPG_StaffInfo").hide(0);
    $("#MstPG_TitleLabel,#MstPG_StaffInfo").bind("click", change_TitleLabel);
});

/**
* 通知のアイコンのpopover変更(既読).
* 
* @param {-} - -
* @return {-} -
* 
* @example 
*  -
*/
$(function () {
    $("#myIMG").popover({
        id: "myIMG_popover",
        offsetX: 0,
        offsetY: 0,
        preventLeft: true,
        preventRight: true,
        preventTop: true,
        preventBottom: false,
        content: "<div id='dashboardFrame_base'/>",
        header: "<div id='MstPG_ForumHeader'><p>" + $("#MstPG_Title_Notice")[0].value + "</p></div>",
        openEvent: function () {
            var container = $('#dashboardFrame_base');
            var $iframe = $("<iframe frameborder='0' id='dashboardFrame' height='432px' width='398px' src='../Pages/SC3040801.aspx' />");
            container.empty().append($iframe);
        }
    });
});

/**
* 通知のアイコンのpopover変更(新着).
* 
* @param {-} - -
* @return {-} -
* 
* @example 
*  -
*/
$(function () {
    $("#myIMG_blink").popover({
        id: "myIMG_blink_popover",
        offsetX: 0,
        offsetY: 0,
        preventLeft: true,
        preventRight: true,
        preventTop: true,
        preventBottom: false,
        content: "<div id='dashboardFrameblink_base'/>",
        header: "<div id='MstPG_ForumHeader_blink'><p>" + $("#MstPG_Title_Notice")[0].value + "</p></div>",
        openEvent: function () {
            var container = $('#dashboardFrameblink_base');
            var $iframe = $("<iframe frameborder='0' id='dashboardFrame_blink' height='432px' width='398px' src='../Pages/SC3040801.aspx' />");
            container.empty().append($iframe);
        }
    });
});

/****************************************************************************

検索ボックス制御用

****************************************************************************/
$(function () {

    //検索条件テキストボックスの幅を保管しておく
    var size = "S";
    var flg = false;

    var custSearchTextWidth = $("#MstPG_CustomerSearchTextBox").width();
    var custSearchAreaWidth = $("#MstPG_CustomerSearchArea").width();

    // 顧客検索ボックスサイズ変更
    function changeCustomerSearchSize() {
        //二重起動チェック
        if (flg === true) return;
        $("#MstPG_CustomerSearchTextBox").CustomTextBox("hideClearButton");
        flg = true;
        if (size === "S") {
            //スモールサイズ時の処理
            $("#header > h1").fadeOut(300);
            $("#MstPG_LeftButtonsGroup > li").fadeOut(300);
            $("#MstPG_SearchType").fadeIn(300);

            $("#MstPG_CustomerSearchArea").css({
                "-webkit-transition": "400ms linear",
                "width": "333px"
            });
            $("#MstPG_CustomerSearchTextBox").css({
                "-webkit-transition": "400ms linear",
                "width": "294px"
            }).one("webkitTransitionEnd", function () {
                //アニメーション終了
                size = "L";
                flg = false;
                $("#MstPG_CustomerSearchTextBox").CustomTextBox("showClearButton");
                $("#MstPG_CustomerSearchArea").css({ "-webkit-transition": "none" });
                $("#MstPG_CustomerSearchTextBox").css({ "-webkit-transition": "none" });
            });
        } else {
            size = "S";
            //ビッグサイズ時の処理
            $("#MstPG_CustomerSearchTextBox").css("width", custSearchTextWidth);
            $("#MstPG_SearchType").fadeOut(300);
            $("#header > h1").fadeIn(300);
            $("#MstPG_LeftButtonsGroup > li").fadeIn(300);
            $("#MstPG_CustomerSearchArea").css({
                "-webkit-transition": "400ms linear",
                "width": custSearchAreaWidth
            }).one("webkitTransitionEnd", function () {
                flg = false;
                $("#MstPG_CustomerSearchArea").css({ "-webkit-transition": "none" });
                $("#MstPG_CustomerSearchTextBox").blur();
            });
        }
    }

    //検索ボックスタップ
    $("#MstPG_CustomerSearchButton").bind("mousedown touchstart", function (e) {
        e.stopImmediatePropagation();
    });

    $("#MstPG_SearchType").bind("mousedown touchstart", function (e) {
        e.stopImmediatePropagation();
    });

    $("#MstPG_CustomerSearchTextBox").bind("focus", function (e) {
        if (size === "S") changeCustomerSearchSize();
    });

    $("#MstPG_CustomerSearchTextBox").bind("focusin", function (e) {
        e.stopImmediatePropagation();
    });

    //ドキュメントクリックの監視
    $("#container").bind("mousedown touchstart", function (e) {
        if ($(e.target).is("#MstPG_RightButtonsGroup #MstPG_CustomerSearchArea, #MstPG_RightButtonsGroup #MstPG_CustomerSearchArea *") === false && size === "L") {
            changeCustomerSearchSize();
        }
    });

});

function clickMstPGCustomerSearch() {
    if ($("#MstPG_CustomerSearchTextBox").val() == "") {
        return false;
    }
    $("#MstPG_CustomerSearchTextBox").blur();
    $.master.OpenLoadingScreen();
}

$(function () {
    $("#MstPG_LoadingScreen").bind("mousedown touchstart", function (e) {
        e.stopImmediatePropagation();
    });
});


/****************************************************************************

顧客検索用ラジオボタン変更時

****************************************************************************/
var g_MstPGIniLoad = false;
function MstPG_SearchTypeSegmenteButton_select(value) {
    if (value == '1') {
        $("#MstPG_CustomerSearchTextBox")[0].placeholder = $("#MstPG_CustomerSearchTypeWordRegNoTextBox")[0].value;
    }
    else if (value == '2') {
        $("#MstPG_CustomerSearchTextBox")[0].placeholder = $("#MstPG_CustomerSearchTypeWordNameTextBox")[0].value;
    }
    else if (value == '3') {
        $("#MstPG_CustomerSearchTextBox")[0].placeholder = $("#MstPG_CustomerSearchTypeWordVinTextBox")[0].value;
    }
    else if (value == '4') {
        $("#MstPG_CustomerSearchTextBox")[0].placeholder = $("#MstPG_CustomerSearchTypeWordTelTextBox")[0].value;
    }
    else if (value == '5') {
        $("#MstPG_CustomerSearchTextBox")[0].placeholder = $("#MstPG_CustomerSearchTypeWordROTextBox")[0].value;
    }

    if (g_MstPGIniLoad) {
        $("#MstPG_FocusinDummyButton").click();
    } else {
        g_MstPGIniLoad = true;
    }
}

function FocusInCustomerSearchTextBox() {
    $('#MstPG_CustomerSearchTextBox').focus();
    setTimeout(function () {
        $("#MstPG_CustomerSearchTextBox").CustomTextBox("showClearButton");
    }, 150);
}

function footerOpen() {
    $("#MstPG_FootItem_Space_Center").css({ "display": "none" });
    var _width = (88 + 5) * $("#MstPG_FootItem_Space_Center > div[id^='MstPG_FootItem_Sub']").length;
    $("#MstPG_FootItem_Space_Right").css({
        "-webkit-transition": "400ms linear",
        "left": _width
    }).one("webkitTransitionEnd", function () {
        $("#MstPG_FootItem_Space_Right").css({ "-webkit-transition": "none", "left": 0 });
        $("#MstPG_FootItem_Space_Center").fadeIn(300);
        $("#MstPG_FootItem_Space_Center").css({ "display": "block" });
    });
}

function footerClose() {
    $("#MstPG_FootItem_Space_Center").css({ "width": $("#MstPG_FootItem_Space_Center").width() });
    setTimeout(function () {
        $("#MstPG_FootItem_Space_Center").css({
            "-webkit-transition": "100ms linear",
            "opacity": 0
        }).one("webkitTransitionEnd", function () {
            $("#MstPG_FootItem_Space_Center").css({
                "-webkit-transition": "100ms linear",
                "width": 0
            }).one("webkitTransitionEnd", function () {
                $("#MstPG_FootItem_Space_Center").hide();
            });
        });
    }, 0);
    return true;
}

function freezeHeaderOperation() {
    var blocker = $("<div style='position:absolute; left:0; top:0; z-index:10000;'></div>");
    var header = $("#header");
    blocker
        .width(header.width())
        .height(header.height());
    header
        .css("position", "relative")
        .append(blocker);

    header.find(".leftButtonsGroup .prevButton").empty();
    header.find(".leftButtonsGroup .nextButton").empty();
    $("#MstPG_CustomerSearchArea").css("opacity", 0.3);
}

/****************************************************************************

ロック状態切り替え

****************************************************************************/
$(function () {
    var data = {};

    $('#MstPG_OperationLockedImage').click(function (e) {
        var locked = $("#MstPG_OperationLocked");
        if (locked.val() != "1") {
            locked.val("1");
            MstPG_doPostBack();
        }
    });

    $('#foot').bind("mousedown touchstart", data, function (e) {
        var now = new Date();
        var prevTouchTime = $(this).data("prevTouchTime");
        if (prevTouchTime) {
            var currentTouchTime = now.getTime();
            if ((currentTouchTime - prevTouchTime) < 1000) {
                //フッタをダブルタップすると上にスクロールしてしまう現象を防止する
                e.preventDefault();
                return;
            }
        }
        $(this).data("prevTouchTime", now.getTime());

        var locked = $("#MstPG_OperationLocked");
        if (locked.val() == "1" && 600 < e.pageX) {
            e.data.minY = e.pageY - 20;
            e.data.maxY = e.pageY + 20;
            e.data.minX = 0;
            e.data.maxX = 50;
            e.data.unlocking = false;

            $("#bodyFrame")
                .bind("mousemove.CommonMasterPage touchmove.CommonMasterPage", e.data, function (e) {
                    e.data.unlocking = (e.data.minY <= e.pageY && e.pageY <= e.data.maxY && e.data.minX <= e.pageX && e.pageX <= e.data.maxX);
                })
                .bind("mouseup.CommonMasterPage touchend.CommonMasterPage", e.data, function (e) {
                    if (e.data.unlocking) {
                        $("#MstPG_OperationLocked").val("0");
                        MstPG_doPostBack();
                    }
                    $("#bodyFrame").unbind(".CommonMasterPage");
                    return false;
                });
        }
    });

});

/**
* 通知のアイコンを点滅させる.
* 
* @param {-} - -
* @return {-} -
* 
* @example 
*  -
*/
function blinkImage() {
    var targetElement = document.getElementById('myIMG_blink');

    setTimeout(function () { if (targetElement != null) { targetElement.style.opacity = 1.0 }; }, 0);
    setTimeout(function () { if (targetElement != null) { targetElement.style.opacity = 0.9 }; }, 100);
    setTimeout(function () { if (targetElement != null) { targetElement.style.opacity = 0.8 }; }, 200);
    setTimeout(function () { if (targetElement != null) { targetElement.style.opacity = 0.7 }; }, 300);
    setTimeout(function () { if (targetElement != null) { targetElement.style.opacity = 0.6 }; }, 400);
    setTimeout(function () { if (targetElement != null) { targetElement.style.opacity = 0.5 }; }, 500);
    setTimeout(function () { if (targetElement != null) { targetElement.style.opacity = 0.4 }; }, 600);
    setTimeout(function () { if (targetElement != null) { targetElement.style.opacity = 0.3 }; }, 700);
    setTimeout(function () { if (targetElement != null) { targetElement.style.opacity = 0.4 }; }, 1400);
    setTimeout(function () { if (targetElement != null) { targetElement.style.opacity = 0.5 }; }, 1500);
    setTimeout(function () { if (targetElement != null) { targetElement.style.opacity = 0.6 }; }, 1600);
    setTimeout(function () { if (targetElement != null) { targetElement.style.opacity = 0.7 }; }, 1700);
    setTimeout(function () { if (targetElement != null) { targetElement.style.opacity = 0.8 }; }, 1800);
    setTimeout(function () { if (targetElement != null) { targetElement.style.opacity = 0.9 }; }, 1900);
    setTimeout(function () { if (targetElement != null) { targetElement.style.opacity = 1.0 }; }, 2000);

    setTimeout("blinkImage()", 3000);
}