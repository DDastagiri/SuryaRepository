/**
* @fileOverview SC3040101_�A����������
*
* @author TCS ����
* @version 1.0.0
*/

/// <reference path="../jquery.js"/>

/**
* �\�������̏����l�ݒ�
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
* ���e�{�^���̊���,�񊈐��𐧌䂷��.
*/
function checkInput() {
    var txtMessageTitle = document.getElementById("messageCustomTitleTextBox");
    var txtMessage = document.getElementById("messagesCustomTextBox");
    var txtperiod = document.getElementById("displayPeriodCustomDateTimeSelector")

    var divActive = document.getElementById("divActive");   //������{�^��
    var divDisable = document.getElementById("divDisable"); //����

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
* Trim����
*/
String.prototype.trim = function () {
    return this.replace(/^[\s ]+|[\s ]+$/g, '');
}

/**
* ���e�{�^������������
*/
function check() {
    if ($("#serverProcessFlgHidden").val() == "1") {  //�T�[�o�[�T�C�h�����t���O���� (1:������)
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
    //�������t���O�𗧂Ă�
    $("#serverProcessFlgHidden").val("1");          //�T�[�o�[�T�C�h�����t���O (1:������)

}

/**
* �L�����Z���{�^������������
*/
function cancel() {
    $("#messageCustomTitleTextBox").CustomTextBox("updateText", "");
    $("#messagesCustomTextBox").val("");
    $("#displayPeriodCustomDateTimeSelector").val($("#displayPeriodHidden").val());
    divActive.style.display = "none";
    divDisable.style.display = "block";
    
    return false;
}
