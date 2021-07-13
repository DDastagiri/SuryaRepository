/// <reference path="../jquery.js"/>
/// <reference path="../jquery.fingerscroll.js"/>
/// <reference path="../SC3080201/Common.js"/>


// onload時の設定
$(function () {

    $("#salesConditionCurrentMode").show(0);
    $("#salesConditionEditMode").hide(0);
    $("#normalSizeLinkButton").hide(0);
    if ($(".scNscCompetingCarAreaHidden").size() > 0) $("#bigSizeLinkButton").hide(0);
    selectedSeriesDisplay("", "", "");
    conditionEventStyleDisplay();
    activityEventStyleDisplay();
    processDisplay();
    humanIconDisplay();
    compCarEventSizeChange("normalMode");

    if ($.trim($("#dispWalkinnum").text()).length === 0) $(".scNscNewActionBox .ActionBoxMember").css("background-image", "none");
    if ($.trim($("#dispAccount").text()).length === 0) $(".scNscNewActionBox .ActionBoxHuman").css("background-image", "none");

    //if ($.trim($("#CrActResult").attr("src")).length === 0) $(".scNscStatusIconList .scNscStatusIcon").hide(0);
    if (SC3080201.newActivityFlg === true || $.trim($("#CrActResult").attr("src")).length === 0) $(".scNscStatusIconList .scNscStatusIcon").hide(0);

    if ($("#PageEnabledFlgHidden").val() == "False") {
        $("#scNscOneBoxContentsArea :text,#scNscOneBoxContentsArea textarea").attr("disabled", "true");
    }

    $("#ScNscCompeCarScrollPane").fingerScroll();

});

// 権限アイコンの表示
function humanIconDisplay() {
    // CCO
    if ($("#accountOperationHidden").val() == "2") {
        $(".scNscNewActionBox .ActionBoxHuman").css("background", "url(../Styles/Images/Authority/CCO.png) no-repeat 0 2px");
    // SC
    } else if ($("#accountOperationHidden").val() == "8") {
        $(".scNscNewActionBox .ActionBoxHuman").css("background", "url(../Styles/Images/Authority/SC.png) no-repeat 0 2px");
    // SA
    } else if ($("#accountOperationHidden").val() == "9") {
        $(".scNscNewActionBox .ActionBoxHuman").css("background", "url(../Styles/Images/Authority/SA.png) no-repeat 0 2px");
    // Manager
    } else {
        $(".scNscNewActionBox .ActionBoxHuman").css("background", "url(../Styles/Images/Authority/Manager.png) no-repeat 0 2px");
    }
}

// プロセス表示
function processDisplay() {
    //プロセス日付初期化
    $("#dispProcessCatalog").text("");
    $("#dispProcessTestdrive").text("");
    $("#dispProcessEvaluation").text("");
    $("#dispProcessQuotation").text("");
    //プロセス日付反映
    $(".ProcessHiddenField li").each(function () {
        if ($(this).children(":nth-child(1)").val() == $("#selSeqnoHidden").val()) {
            $("#dispProcessCatalog").text($(this).children(":nth-child(2)").val());
            $("#dispProcessTestdrive").text($(this).children(":nth-child(3)").val());
            $("#dispProcessEvaluation").text($(this).children(":nth-child(4)").val());
            $("#dispProcessQuotation").text($(this).children(":nth-child(5)").val());
        }
    });

    //アイコン状態のクリア
    $("#dispProcessCatalog, #dispProcessTestdrive, #dispProcessEvaluation, #dispProcessQuotation").each(function () {
        if ($(this).text() != "") {
            $(this).css("background", "url(" + $(this).attr("onIconPath") + ") no-repeat");
        } else {
            $(this).css("background", "url(" + $(this).attr("offIconPath") + ") no-repeat");
        }
    });

}

$(function () {

    //スクロール設定
    $(".scNsc50MemoBottom").fingerScroll();
//    $("#todayMemoTextBox").css("height", ($("#todayMemoTextBox").get(0).scrollHeight + 30) + "px");
//    $(".memoTextBoxInner").fingerScroll();

    //当日メモフォーカスアウト
    $("#todayMemoTextBox").live("focusout", function (e) {
//        var textValue = $("#todayMemoTextBox").val();
//        var html = $("#memoTextBoxInner2").html();
//        var textArea = $(html);
//        textArea.val(textValue);
//        $("#todayMemoTextBox").remove();
//        setTimeout(function () {
//            $("#memoTextBoxInner2").append(textArea);
//            setTimeout(function () {
//                $(".memoTextBoxInner").fingerScroll();

                //変更チェック
                if ($("#todayMemoTextBox").val() != $("#todayMemoTextBoxBefore").val()) {
                    //更新用ダミーボタンクリック
                    $("#commitTodayMemoButtonDummy").click();
                    $("#todayMemoTextBoxBefore").val($("#todayMemoTextBox").val());
                }

//            }, 0);
//        }, 0);
    });

    //テキストエリアタップ
    $("#todayMemoTextBox").live("mouseup touchend", function (e) {
        this.focus();
    });

//    //キーボード監視
//    $("#todayMemoTextBox").live("keyup keydown change", function (e) {
//        if ($("#todayMemoTextBox").get(0).scrollHeight > $("#todayMemoTextBox").height()) {
//            $("#todayMemoTextBox").scrollTop(0);
//            $("#todayMemoTextBox").css("height", ($("#todayMemoTextBox").get(0).scrollHeight + 30) + "px");
//        }
//    });


});


