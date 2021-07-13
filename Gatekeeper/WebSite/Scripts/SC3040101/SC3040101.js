/**
* @fileOverview SC3040101_連絡事項入力
*
* @author TCS 藤井
* @version 1.0.0
*/

/// <reference path="../jquery.js"/>

/**
* 表示期限の初期値設定
*/
$(function () {
    $("#displayPeriodCustomDateTimeSelector").val($("#displayPeriodHidden").val());

    if ($("#postButton").is("[data-errorflg='yes']") === true) {
        $("#displayPeriodCustomDateTimeSelector").val($("#displayPerioderrHidden").val());
    }

    $("#messageCustomTitleTextBox").CustomTextBox({
        clear: function () {
            checkInput();
        }
    });
});


/**
* 投稿ボタンの活性,非活性を制御する.
*/
function checkInput() {
    var txtMessageTitle = document.getElementById("messageCustomTitleTextBox");
    var txtMessage = document.getElementById("messagesCustomTextBox");
    var txtperiod = document.getElementById("displayPeriodCustomDateTimeSelector")

    var divActive = document.getElementById("divActive");   //押せるボタン
    var divDisable = document.getElementById("divDisable"); //文字

    if (txtMessageTitle && txtMessage && txtperiod) {
        var strTitle = txtMessageTitle.value.trim();
        var strMessage = txtMessage.value.trim();
        var strPeriod = txtperiod.value.trim();

        if (strTitle == "" || strMessage == "" || strPeriod == "") {
            divActive.style.display = "none";
            divDisable.style.display = "block";
        }
        else {
            divActive.style.display = "block";
            divDisable.style.display = "none";
        }
    }
}

/**
* Trim処理
*/
String.prototype.trim = function () {
    return this.replace(/^[\s ]+|[\s ]+$/g, '');
}

/**
* 投稿ボタン押下時処理
*/
function check() {
    if ($("#serverProcessFlgHidden").val() == "1") {  //サーバーサイド処理フラグ判定 (1:処理中)
        return false;
    }

    var title = $("#messageCustomTitleTextBox").attr("value");
    var message = $("#messagesCustomTextBox").attr("value");
    var selectdate = $("#displayPeriodCustomDateTimeSelector").attr("value");

    if (title == "") {
        icropScript.ShowMessageBox(901, $("#errMsg1Hidden").val(), "");
        return false;
    }

    if (message == "") {
        icropScript.ShowMessageBox(902, $("#errMsg2Hidden").val(), "");
        return false;
    }
          
    if (selectdate == "") {
        icropScript.ShowMessageBox(903, $("#errMsg3Hidden").val(), "");
        return false;
    }
    //処理中フラグを立てる
    $("#serverProcessFlgHidden").val("1");          //サーバーサイド処理フラグ (1:処理中)

}

/**
* キャンセルボタン押下時処理
*/
function cancel() {
    $("#messageCustomTitleTextBox").CustomTextBox("updateText", "");
    $("#messagesCustomTextBox").val("");
    $("#displayPeriodCustomDateTimeSelector").val($("#displayPeriodHidden").val());
    divActive.style.display = "none";
    divDisable.style.display = "block";
    
    return false;
}
