/// <reference path="jquery.js"/>
/// <reference path="eCRB.js"/>
/// <reference path="eCRB.ui.js"/>

/****************************************************************************

イベント用の変数設定
タッチイベントが使用できるかどうかを判断してイベントを切り替える変数を設定

****************************************************************************/
//タッチイベント可能有無
var supportTouch = 'ontouchend' in document;
//touchstart、mousedownのイベント
var EVENTNAME_TOUCHSTART = supportTouch ? 'touchstart' : 'mousedown';
//touchmove、mousemoveのイベント
var EVENTNAME_TOUCHMOVE = supportTouch ? 'touchmove' : 'mousemove';
//touchend、mouseupのイベント
var EVENTNAME_TOUCHEND = supportTouch ? 'touchend' : 'mouseup';
//clickのイベント
var EVENTNAME_CLICK = "click";

/****************************************************************************

マスターページに関する処理のjQUERY拡張

****************************************************************************/
(function (window) {
    $.extend({ master: {

        blinkIcropLogoTimer: null,

        //i-CROPアイコン点滅開始
        blinkStartIcropLogo: function () {
            this.blinkIcropLogoTimer = setInterval(function () {
                $("#mstpg_icropLogo").is(":hidden") ? $("#mstpg_icropLogo").fadeIn(200) : $("#mstpg_icropLogo").fadeOut(200);
            }, 200);
        },

        //i-CROPアイコン点滅終了
        blinkEndIcropLogo: function () {
            if (this.blinkIcropLogoTimer) clearInterval(this.blinkIcropLogoTimer);
            $("#mstpg_icropLogo").show(0);
        },

        OpenLoadingScreen: function () {
            $("#MstPG_LoadingScreen").css({ "width": $(window).width() + "px", "height": $(window).height() + "px" });
            setTimeout(function () {
                $("#MstPG_LoadingScreen").css({ "display": "table" });
            }, 0);
        },

        CloseLoadingScreen: function () {
            $("#MstPG_LoadingScreen").css({ "display": "none" });
        }
    }
    });
})(window);

function g_MstPGshowLoding() {
    $("#MstPG_registOverlayBlack")
        .css("display", "block")
        .offset({ top: $("#header").height(), left: 0 })
        .addClass("open");
    $("#MstPG_processingServer").addClass("show");
}

function g_MstPGcloseLoding() {
    $("#MstPG_processingServer").removeClass("show");
    $("#MstPG_registOverlayBlack")
        .removeClass("open")
        .one("webkitTransitionEnd", function (we) {
            $("#MstPG_registOverlayBlack").css("display", "none");
        });
}

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
            if (type == "SEARCH" || type == "PASSWORD") {
                return true;
            }
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

/**
* 未対応来店客のアイコンのpopover変更(既読).
* 
* @param {-} - -
* @return {-} -
* 
* @example 
*  -
*/
$(function () {
    $("#myVisit").popover({
        id: "myVisit_popover",
        offsetX: 0,
        offsetY: 0,
        preventLeft: true,
        preventRight: true,
        preventTop: true,
        preventBottom: false,
        content: "<div id='dashboardVisitFrame_base'/>",
        header: "<div id='MstPG_VisitorHeader'><p>" + $("#MstPG_Title_Visitor")[0].value + "</p></div>",
        openEvent: function () {
            var container = $('#dashboardVisitFrame_base');
            var $iframe = $("<iframe frameborder='0' id='dashboardVisitFrame' height='312px' width='633px' src='../Pages/SC3100201.aspx' />");
            container.empty().append($iframe);
        }
    });
});

/**
* 未対応来店客のアイコンのpopover変更(新着).
* 
* @param {-} - -
* @return {-} -
* 
* @example 
*  -
*/
$(function () {
    $("#myVisit_blink").popover({
        id: "myVisit_blink_popover",
        offsetX: 0,
        offsetY: 0,
        preventLeft: true,
        preventRight: true,
        preventTop: true,
        preventBottom: false,
        content: "<div id='dashboardVisitFrameblink_base'/>",
        header: "<div id='MstPG_VisitorHeader_blink'><p>" + $("#MstPG_Title_Visitor")[0].value + "</p></div>",
        openEvent: function () {
            var container = $('#dashboardVisitFrameblink_base');
            var $iframe = $("<iframe frameborder='0' id='dashboardVisitFrame_blink' height='312px' width='633px' src='../Pages/SC3100201.aspx' />");
            container.empty().append($iframe);
        }
    });
});

/****************************************************************************

検索ボックス制御用

****************************************************************************/
//検索条件テキストボックスの幅を保管しておく
var custSearchSize = "S";
var custSearchFlg = false;

// 顧客検索ボックスサイズ変更
function changeCustomerSearchSize() {
    //二重起動チェック
    if (custSearchFlg === true) return;

    $("#MstPG_CustomerSearchTextBox").CustomTextBox("hideClearButton");
    custSearchFlg = true;
    if (custSearchSize === "S") {
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
            "width": "310px"
        }).one("webkitTransitionEnd", function () {
            //アニメーション終了
            $("#MstPG_CustomerSearchTextBox").CustomTextBox("showClearButton");
            custSearchSize = "L";
            custSearchFlg = false;
            $("#MstPG_CustomerSearchArea").css({ "-webkit-transition": "none" });
            $("#MstPG_CustomerSearchTextBox").css({ "-webkit-transition": "none" });
        });
    } else {
        custSearchSize = "S";
        //ビッグサイズ時の処理
        $("#MstPG_CustomerSearchTextBox").css("width", "133px");
        $("#MstPG_SearchType").fadeOut(300);
        $("#header > h1").fadeIn(300);
        $("#MstPG_LeftButtonsGroup > li").fadeIn(300);
        $("#MstPG_CustomerSearchArea").css({
            "-webkit-transition": "400ms linear",
            "width": "165px"
        }).one("webkitTransitionEnd", function () {
            custSearchFlg = false;
            $("#MstPG_CustomerSearchArea").css({ "-webkit-transition": "none" });
            $("#MstPG_CustomerSearchTextBox").blur();
        });
    }
}

//顧客検索バーフォーカスフラグ
var custSearchfouusFlg = false;

$(function () {
    //検索ボックスタップ
    $("#MstPG_CustomerSearchButton").bind("mousedown touchstart", function (e) {
        e.stopImmediatePropagation();
    });

    $("#MstPG_SearchType").bind("mousedown touchstart", function (e) {
        e.stopImmediatePropagation();
    });

    $("#MstPG_CustomerSearchTextBox").bind("focus", function (e) {
        if (custSearchSize === "S") changeCustomerSearchSize();
    });

    $("#MstPG_CustomerSearchTextBox").bind("focusin", function (e) {
        e.stopImmediatePropagation();
    });

    $("#MstPG_CustomerSearchTextBox").bind("focusout", function (e) {
        setTimeout(function () {
            //顧客検索バーの表示幅を小さくする
            if (custSearchfouusFlg === false) {
                if (custSearchSize === "L") {
                    changeCustomerSearchSize();
                }
            }
            custSearchfouusFlg = false;
        }, 300);
    });

    //ドキュメントクリックの監視
    $("#container").bind("mousedown touchstart", function (e) {
        if ($(e.target).is("#MstPG_RightButtonsGroup #MstPG_CustomerSearchArea, #MstPG_RightButtonsGroup #MstPG_CustomerSearchArea *") === false && custSearchSize === "L") {
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

// 2013/06/03 TMEJ 小澤 【A.STEP2】i-CROP業務機能要件定義（既存流用機能） START
var g_MstSMBPGIniLoad = false;
function MstPG_SearchTypeSMBSegmenteButton_select(value) {

    if (g_MstSMBPGIniLoad) {
        $("#MstPG_FocusinDummyButton").click();
    } else {
        g_MstSMBPGIniLoad = true;
    }
}
// 2013/06/03 TMEJ 小澤 【A.STEP2】i-CROP業務機能要件定義（既存流用機能） END

function FocusInCustomerSearchTextBox() {
    $('#MstPG_CustomerSearchTextBox').focus();
    custSearchfouusFlg = true;
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
    var blocker = $("<div style='position:absolute; left:0; top:0; z-index:10000;'></div>"),
        header = $("#header");

    blocker
        .width(header.width())
        .height(header.height());
    header
        .css("position", "relative")
        .append(blocker);

    header.find(".leftButtonsGroup .prevButton").empty();
    header.find(".leftButtonsGroup .nextButton").empty();
    $("#MstPG_CustomerSearchTextBoxPanel").addClass("disabled");
    $("#visitorButtonPanel").addClass("disabled");
    $("#forumButtonPanel").addClass("disabled");

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
    var targetVisitElement = document.getElementById('myVisit_blink');

    setTimeout(function () { if (targetElement != null) { targetElement.style.opacity = 1.0 }; if (targetVisitElement != null) { targetVisitElement.style.opacity = 1.0 }; }, 0);
    setTimeout(function () { if (targetElement != null) { targetElement.style.opacity = 0.9 }; if (targetVisitElement != null) { targetVisitElement.style.opacity = 0.9 }; }, 100);
    setTimeout(function () { if (targetElement != null) { targetElement.style.opacity = 0.8 }; if (targetVisitElement != null) { targetVisitElement.style.opacity = 0.8 }; }, 200);
    setTimeout(function () { if (targetElement != null) { targetElement.style.opacity = 0.7 }; if (targetVisitElement != null) { targetVisitElement.style.opacity = 0.7 }; }, 300);
    setTimeout(function () { if (targetElement != null) { targetElement.style.opacity = 0.6 }; if (targetVisitElement != null) { targetVisitElement.style.opacity = 0.6 }; }, 400);
    setTimeout(function () { if (targetElement != null) { targetElement.style.opacity = 0.5 }; if (targetVisitElement != null) { targetVisitElement.style.opacity = 0.5 }; }, 500);
    setTimeout(function () { if (targetElement != null) { targetElement.style.opacity = 0.4 }; if (targetVisitElement != null) { targetVisitElement.style.opacity = 0.4 }; }, 600);
    setTimeout(function () { if (targetElement != null) { targetElement.style.opacity = 0.3 }; if (targetVisitElement != null) { targetVisitElement.style.opacity = 0.3 }; }, 700);
    setTimeout(function () { if (targetElement != null) { targetElement.style.opacity = 0.4 }; if (targetVisitElement != null) { targetVisitElement.style.opacity = 0.4 }; }, 1400);
    setTimeout(function () { if (targetElement != null) { targetElement.style.opacity = 0.5 }; if (targetVisitElement != null) { targetVisitElement.style.opacity = 0.5 }; }, 1500);
    setTimeout(function () { if (targetElement != null) { targetElement.style.opacity = 0.6 }; if (targetVisitElement != null) { targetVisitElement.style.opacity = 0.6 }; }, 1600);
    setTimeout(function () { if (targetElement != null) { targetElement.style.opacity = 0.7 }; if (targetVisitElement != null) { targetVisitElement.style.opacity = 0.7 }; }, 1700);
    setTimeout(function () { if (targetElement != null) { targetElement.style.opacity = 0.8 }; if (targetVisitElement != null) { targetVisitElement.style.opacity = 0.8 }; }, 1800);
    setTimeout(function () { if (targetElement != null) { targetElement.style.opacity = 0.9 }; if (targetVisitElement != null) { targetVisitElement.style.opacity = 0.9 }; }, 1900);
    setTimeout(function () { if (targetElement != null) { targetElement.style.opacity = 1.0 }; if (targetVisitElement != null) { targetVisitElement.style.opacity = 1.0 }; }, 2000);

    setTimeout("blinkImage()", 3000);
}

var timerClearTime = 0;

/**
* 再表示タイマーをセットする.
* 
* @param {refreshFunc} 再表示用のJavaScrep関数 -
* @return {-} -
* 
* @example 
*  -
*/
function commonRefreshTimer(refreshFunc) {

    //タイマー間隔の取得
    var refreshTime = Number($("#MstPG_RefreshTimerTime").val());

    //開始時間を保持する
    var startTime = new Date().getTime();

    setTimeout(function () {

        //clearTimer()がされている場合は処理しない
        if (startTime <= timerClearTime) {
            return;
        }

        //出力メッセージを選択する
        var messageString = $("#MstPG_RefreshTimerMessage1").val();

        //警告メッセージ出力
        alert(messageString);

        //各画面でリフレッシュ処理をする
        if (refreshFunc() === false) {
            //falseが帰ってきたら再読み込み処理をしない
            return;
        }

        //再度、タイマーをセットする
        commonRefreshTimer(refreshFunc)

    }, refreshTime);
}

/**
* 再表示タイマーをリセットする.
* 
* @param {-} - -
* @return {-} -
* 
* @example 
*  -
*/
function commonClearTimer() {
    //現在時、以前のタイマーを無視する
    timerClearTime = new Date().getTime();
}


/* 2013/12/16 TMEJ 小澤 次世代サービス 工程管理機能開発 START */

/**
* フッターボタンバインド設定.
* @buttonId {フッターボタンID}
* @callScript {設定するScript}
* 
* @return {}
*/
function SetFooterScript(buttonId, callScript) {

    //ボタンIDの設定
    var buttonInfo = "#MstPG_FootItem_Main_" + buttonId;

    //設定されていたバインド情報削除
    $(buttonInfo).unbind("click");

    //フッターボタンのバインド情報作成
    $(buttonInfo).bind("click", function (event) {
        //背景を青くする
        $(buttonInfo).addClass("icrop-pressed");

        //タイマーで背景を元に戻す
        setTimeout(function () {
            $(buttonInfo).removeClass("icrop-pressed");
        }, 300);

        //スクリプトをコールする
        return callScript();

    });

    //サーバーで設定したスクリプトを操作できないように非表示にする
    var buttonInfoMain = "#sample" + buttonId;
    $(buttonInfoMain).attr("style", "display:none;");

}

/**
* フッターボタンバインド削除.
* 
* @return {}
*/
function DeleteFooterScript() {
    //左側フッターボタンのバインドを削除
    DeleteFooterScriptList($("#MstPG_FootItem_Space_Left").children("div"));

    //真ん中側フッターボタンのバインドを削除
    DeleteFooterScriptList($("#MstPG_FootItem_Space_Center").children("div"));

    //右側フッターボタンのバインドを削除
    DeleteFooterScriptList($("#MstPG_FootItem_Space_Right").children("div"));
}

/**
* 各エリアのフッターボタンバインド削除.
* @buttonObject {フッターボタンエリア情報}
* 
* @return {}
*/
function DeleteFooterScriptList(buttonObject) {
    for (i = 0; i < $(buttonObject).length; i++) {
        //ボタンID取得
        var buttonId = $(buttonObject[i]).attr("id").substring($(buttonObject[i]).attr("id").lastIndexOf("_") + 1, $(buttonObject[i]).attr("id").length);

        //削除する情報を取得
        var buttonInfo = "#MstPG_FootItem_Main_" + buttonId;

        //設定されていたバインド情報削除
        $(buttonInfo).unbind("click");

        //サーバーで設定したスクリプトを操作できるように表示する
        var buttonInfoMain = "#sample" + buttonId;
        $(buttonInfoMain).attr("style", "display:block;");

    }
}

//ハイライトボタン情報
var HighlightFooterButtonId = "";

/**
* フッターボタンハイライト設定.
* @buttonId {フッターボタンID}
* 
* @return {}
*/
function HighlightFooterButton(buttonId) {
    //すでにハイライトしているボタンがあるかを確認
    if (HighlightFooterButtonId == "") {
        //ハイライトしているボタンがない場合
        //指定ボタンをハイライト設定する
        HighlightOnFooterButton(buttonId);

    } else {
        //ハイライトしているボタンがある場合
        //過去のハイライトを削除する
        HighlightOffFooterButton(HighlightFooterButtonId);
        //指定ボタンをハイライト設定する
        HighlightOnFooterButton(buttonId);

    }
    //指定ボタンIDをハイライトボタン情報に格納する
    HighlightFooterButtonId = buttonId;
}

/**
* フッターボタンハイライトOn設定.
* @buttonId {フッターボタンID}
* 
* @return {}
*/
function HighlightOnFooterButton(buttonId) {
    //ボタンIDの設定
    var buttonInfo = "#MstPG_FootItem_Main_" + buttonId;

    //イメージ画像URLの設定
    var buttonImageUrl = "url(../Styles/Images/FooterButtons/" + buttonId + "_on.png)";

    //画像を設定
    $(buttonInfo).css("background-image", buttonImageUrl);

    //文字色設定
    $(buttonInfo).addClass("mstpg-selected");
}

/**
* フッターボタンハイライトOff設定.
* @buttonId {フッターボタンID}
* 
* @return {}
*/
function HighlightOffFooterButton(buttonId) {
    if (buttonId == null) {
        buttonId = HighlightFooterButtonId;
    }
    //ボタンIDの設定
    var buttonInfo = "#MstPG_FootItem_Main_" + buttonId;

    //イメージ画像URLの設定
    var buttonImageUrl = "url(../Styles/Images/FooterButtons/" + buttonId + ".png)";

    //画像を設定
    $(buttonInfo).css("background-image", buttonImageUrl);

    //文字色設定
    $(buttonInfo).removeClass("mstpg-selected")

    //ハイライトボタン情報初期化
    HighlightFooterButtonId = "";
}

/**
* 固有ボタン作成.
* @PeculiarButtonWord {固有ボタン文言}
* @PeculiarButtonColor {固有ボタンカラー「0：青色」「1：赤色」「2：灰色」「3：空白」}
* @PeculiarButtonClick {スクリプト}
* 
* @return {}
*/
function CreatePeculiarButton(PeculiarButtonWord, PeculiarButtonColor, PeculiarButtonClick) {

    //固有ボタン枠のチェック
    if ($($(".footerNavi").children("div")[2]).children("div").length == 0) {
        //枠の設定がなければ作成
        $($(".footerNavi").children("div")[2]).append("<div id='FooterCustomButton' style='float:right; margin-right:12px;'>");

    }

    /* 空白の場合は文言を初期化しておく */
    if (PeculiarButtonColor == 3) {
        PeculiarButtonWord = "";
    }

    //固有ボタン配置場所設定
    var PeculiarButtonArea = $($(".footerNavi").children("div")[2]).children("div");
    //固有ボタンID
    var CreatePeculiarButtonId = "PeculiarButton" + ($(PeculiarButtonArea).children("p").length + 1);
    //固有文言ID
    var CreatePeculiarButtonWordId = "PeculiarButtonWord" + ($(PeculiarButtonArea).children("p").length + 1);
    //固有ボタン色クラス
    var CreatePeculiarButtonColor = GetPeculiarButtonColor(PeculiarButtonColor);
    //固有ボタン文言クラス
    var CreatePeculiarButtonWordColor = GetPeculiarButtonWord(PeculiarButtonWord);

    //固有ボタンレイアウト作成
    $(PeculiarButtonArea).append("<p id='" + CreatePeculiarButtonId + "' class='" + CreatePeculiarButtonColor + "'>");
    //固有ボタン文言設定
    $("#" + CreatePeculiarButtonId).append("<div id='" + CreatePeculiarButtonWordId + "' class=" + CreatePeculiarButtonWordColor + ">" + PeculiarButtonWord);

    //青色、赤色ボタンの場合は固有ボタンバインド情報作成
    if (PeculiarButtonColor == 0 || PeculiarButtonColor == 1) {
        if (PeculiarButtonClick != undefined && PeculiarButtonClick != null) {
            $("#" + CreatePeculiarButtonId).bind("click", function (event) {
                return PeculiarButtonClick();
            });
        }
    }
}

/**
* 固有ボタン色クラス取得.
* @PeculiarButtonColor {固有ボタンカラー「0：青色」「1：赤色」「2：灰色」「3：空白」}
* 
* @return {}
*/
function GetPeculiarButtonColor(PeculiarButtonColor) {
    //固有ボタン色チェック
    if (PeculiarButtonColor == 0) {
        //青色のCLASSを設定
        return "PeculiarButtonBlue";

    } else if (PeculiarButtonColor == 1) {
        //赤色のクラスを設定
        return "PeculiarButtonRed";

    } else if (PeculiarButtonColor == 2) {
        //灰色のクラスを設定
        return "PeculiarButtonGray";

    } else {
        //空白のクラスを設定
        return "PeculiarButtonNone";

    }
}

/**
* 固有ボタン文言クラス取得.
* @PeculiarButtonWord {固有ボタン文言}
* 
* @return {}
*/
function GetPeculiarButtonWord(PeculiarButtonWord) {
    //改行タグチェック
    if (0 < PeculiarButtonWord.indexOf("<br>") || 0 < PeculiarButtonWord.indexOf("<BR>")) {
        //改行タグが存在する場合は2ライン用のCSSを返す
        return "PeculiarButtonWord_2Line";

    } else {
        //改行タグが存在しない場合は1ライン用のCSSを返す
        return "PeculiarButtonWord";

    }
}
/**
* 固有ボタン削除.
* 
* @return {}
*/
function DeletePeculiarButton() {
    //固有ボタン配置場所設定
    var PeculiarButtonArea = $($(".footerNavi").children("div")[2]).children("div");

    //固有ボタンの存在チェック
    if ($(PeculiarButtonArea).length != 0) {
        //固有ボタンが存在している場合
        //設定した固有ボタンのバインド情報削除
        for (i = 0; i <= $(PeculiarButtonArea).length; i++) {
            $("#PeculiarButton" + i).unbind("click");

        }

        //レイアウト削除
        $(PeculiarButtonArea).remove();
    }

}

/**
* クルクル表示.
* @return {}
*/
function ActiveDisplayOn() {
    $(".ActiveOverlayBlack").css("display", "block");
    $(".ActiveIcon").css("display", "block");
}

/**
* クルクル非表示.
* @return {}
*/
function ActiveDisplayOff() {
    $(".ActiveOverlayBlack").css("display", "none");
    $(".ActiveIcon").css("display", "none");
}

/**
* ポップアップ呼び出し.
* @ProgramId {画面ID}
* @ArrangementButtonNo {ボタン位置}
* @RequestParam {引数二次元配列リスト：キーとデータ}
* 
* @return {}
*/
function OpenPopup(ProgramId, ArrangementButtonNo, RequestParam) {

    //ポップアップの親IDを設定
    var popupDivId = "PopupDiv" + ArrangementButtonNo;
    var popupJqueryDivId = "#" + popupDivId;

    //ポップアップの作成確認
    if ($(popupJqueryDivId) == null || $(popupJqueryDivId) == undefined || $(popupJqueryDivId).length == 0) {
        //ポップアップが作成されていない場合
        //ポップアップのID作成
        var popupId = "Popup" + ArrangementButtonNo;
        var popupJqueryId = "#" + popupId;

        //ポップアップの中身のID作成
        var popupFrameId = popupId + "_Main";
        var popupJqueryFrameId = "#" + popupFrameId;

        /* 0個目「position: absolute;display: block;bottom: 60px;right: 12px;z-index: 10000;」「right: 30px;」 */
        /* 1個目「position: absolute;display: block;bottom: 60px;right: 12px;z-index: 10000;」「right: 128px;」 */
        /* 2個目「position: absolute;display: block;bottom: 60px;right: 105px;z-index: 10000;」「right: 130px;」 */
        /* 3個目「position: absolute;display: block;bottom: 60px;right: 200px;z-index: 10000;」「right: 130px;」 */
        /* 4個目「position: absolute;display: block;bottom: 60px;right: 295px;z-index: 10000;」「right: 130px;」 */

        var popupPosition = "";
        var trianglePosition = "";
        //ボタンNoの確認して位置データを格納
        if (ArrangementButtonNo == 1) {
            //一番右のボタンの場合
            trianglePosition = "right: 30px;";
            popupPosition = "position: absolute;display: block;bottom: 60px;right: 12px;z-index: 10000;"

        } else if (ArrangementButtonNo == 2) {
            //右から2番目のボタンの場合
            trianglePosition = "right: 128px;";
            popupPosition = "position: absolute;display: block;bottom: 60px;right: 12px;z-index: 10000;"

        } else {
            //右から3番目以降のボタンの場合
            popupPosition = "position: absolute;display: block;bottom: 60px;right: " + (105 + (95 * (ArrangementButtonNo - 3))) + "px;z-index: 10000;";
            trianglePosition = "right: 130px;";

        }

        //親DIVの作成
        $("body").append("<div id='" + popupDivId + "' class='OpenPopupClass' style='position:absolute;display:block;top:0px;left:0px;width:1024px;height:748px;z-index: 9999;'>");

        //ポップアップの作成
        $(popupJqueryDivId).append("<div class='popover2' id='" + popupId + "' style='" + popupPosition + "'>");

        //IFlameの作成
        $(popupJqueryId).append("<iframe frameborder='0' id='" + popupFrameId + "' height='302px' width='289px' src='../Pages/" + ProgramId + ".aspx' />");

        //三角アイコンの作成
        $(popupJqueryId).append("<div class='triangle top' style='" + trianglePosition + "'><div class='triangleBorder'><div class='triangleInner'>");

        //ポップアップ以外がタップされたらポップアップを非表示にする処理を作成
        $(popupJqueryDivId).bind("click", function (event) {
            $(popupJqueryDivId).css("display", "none");
        });

    } else {
        //ポップアップがすでに存在している場合
        //IFrameの再描画
        var popupIframeId = "Popup" + ArrangementButtonNo + "_Main";

        // 2019/08/06 NSK 鈴木 PUAT-1054 FM承認のポップアップが表示されない START
        // frames[popupIframeId].location.reload();
        try {
            frames[popupIframeId].contentWindow.location.reload();
        } catch (e) {
            frames[popupIframeId].location.reload();
        }
        // 2019/08/06 NSK 鈴木 PUAT-1054 FM承認のポップアップが表示されない END

        //ポップアップの表示
        setTimeout(function () { $(popupJqueryDivId).css("display", "block"); }, 100);

    }
}

/**
* ポップアップクローズ.
* 
* @return {}
*/
function OpenPopupClose() {
    //OpenPopupを全て閉じる
    $(".OpenPopupClass").each(function () {
        $(this).click();
    });
}

/**
* 画面遷移.
* @ProgramId {画面ID}
* @SessionData {画面間引数のデータ}
* 
* @return {}
*/
function OpenScreen(ProgramId, SessionData) {
    $("#MstPG_RedirectProgramId").val(ProgramId);
    if (SessionData.length != 0) {
        var sessionKey = "";
        var sessionData = "";
        for (i = 0; i < SessionData.length; i++) {
            if (i != 0) {
                sessionKey += ",";
                sessionData += ",";
            }
            sessionKey += SessionData[i][0];
            sessionData += SessionData[i][1];
        }
        $("#MstPG_RedirectSessionKey").val(sessionKey);
        $("#MstPG_RedirectSessionData").val(sessionData);
    }
    $("#MstPG_RedirectNextScreenButton").click();
}

/**
* タイトル変更.
* @TitleWord {タイトル文言}
* 
* @return {}
*/
function SetDisplayTitle(TitleWord) {
    //タイトル文言を変更する
    $("#MstPG_TitleLabel").text(TitleWord);
}

var historyBackCount = 0;
/**
* 戻るボタン回数加算.
* 
* @return {}
*/
function HistoryBackCountUp() {
    //カウントを1増やす
    historyBackCount += 1;

}

/**
* 戻るボタンタップ処理.
* 
* @return {}
*/
function iframeHistoryBack() {
    //カウントチェック
    if (historyBackCount == 0) {
        //画面内での戻るが必要ない場合
        //従来の戻るボタン処理実行
        //2014/09/03 TMEJ 小澤 IT9745_NextSTEPサービス サービス業務向け評価用アプリのシステムテスト START
        $.master.OpenLoadingScreen();
        //2014/09/03 TMEJ 小澤 IT9745_NextSTEPサービス サービス業務向け評価用アプリのシステムテスト END
        return true;

    } else {
        //画面内の戻るが必要な場合
        //iFrameに対しHitoryBackを実行
        document.getElementById("iFramePage").contentWindow.history.back();

        //カウントを1減らす
        historyBackCount -= 1;
        return false;

    }
}

$(function () {
    //ドメインデータチェック
    if ($("#MstPG_Domain").val() != "" && $("#MstPG_Domain").val() != undefined && $("#MstPG_Domain").val() != null) {
        try {
            //クロスドメインの設定
            document.domain = $("#MstPG_Domain").val();
        } catch (e) {
            //alert("The crossing domain went wrong. ");
        }
    }
});

/* 2013/12/16 TMEJ 小澤 次世代サービス 工程管理機能開発 END */

