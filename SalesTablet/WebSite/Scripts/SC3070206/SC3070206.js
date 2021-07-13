/*━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
SC3070206.js
─────────────────────────────────────
機能： 価格相談回答
補足： 
作成： 2013/12/09 TCS 外崎  Aカード情報相互連携開発
─────────────────────────────────────*/
/**************************************************************
* 関数
**************************************************************/

//支払い総額計算（スタッフ）
function ApprovalTotalPriceStaff() {
    if ((this_form.approvalFieldFlgHiddenField.value).toUpperCase() == "TRUE") {
        if (typeof frames["EstimateInfo"].ApprovalTotalPrice == "function") {
            var lngPayTotal = frames["EstimateInfo"].ApprovalTotalPrice();

            //値引き額（-）
            if (this_form.approvalPriceStaffHiddenField.value != "") {
                lngPayTotal = Math.round((lngPayTotal - parseFloat(this_form.approvalPriceStaffHiddenField.value)) * 100) / 100;
            }

            //支払い総額金額表示
            $("#RequestTotalPriceLabel").show().html(formatNumber(lngPayTotal));
        } else {
            //支払い総額金額表示
            $("#RequestTotalPriceLabel").hide();
        }
    }
}

//支払い総額計算（マネージャー）
function ApprovalTotalPriceManager() {
    if ((this_form.approvalFieldFlgHiddenField.value).toUpperCase() == "TRUE") {
        if (typeof frames["EstimateInfo"].ApprovalTotalPrice == "function") {
            var lngPayTotal = frames["EstimateInfo"].ApprovalTotalPrice();

            //値引き額（-）
            if (this_form.approvalPriceHiddenField.value != "") {
                lngPayTotal = Math.round((lngPayTotal - parseFloat(this_form.approvalPriceHiddenField.value)) * 100) / 100;
            }

            //支払い総額金額表示
            $("#ApprovalTotalPriceLabel").show().html(formatNumber(lngPayTotal));
        } else {
            //支払い総額金額表示
            $("#ApprovalTotalPriceLabel").hide();
        }
    }
}

//送信クリック時（二重押し対応）
function sendButtonClick() {
    dispLoading();
    return true;
}

$(function () {
    $("#EstimateInfo").load(function () {
        //回答入力スタッフ値引後お支払い金額
        ApprovalTotalPriceStaff();

        //回答入力マネージャー値引後お支払い金額
        ApprovalTotalPriceManager();

        //マネージャー値引き額
        $(".approvalPrice").attr('readonly','readonly').NumericKeypad({
            acceptDecimalPoint: true,
            defaultValue: 0,
            completionLabel: $("#numericKeyPadDoneHiddenField").val(),
            cancelLabel: $("#numericKeyPadCancelHiddenField").val(),
            valueChanged: function (num) {
                if (num.match(/^[0-9]{1,9}(\.[0-9]{1,2})?$/) || (num == "")) {
                    var numberFormat = num;
                    var numberFormatReturn = formatNumber(numberFormat);
                    $(this).val(numberFormatReturn);
                    $("#approvalPriceHiddenField").val(numberFormatReturn);
                    $("#ApprovalDiscountPriceTextBox").val(numberFormatReturn);
                    //価格相談総額計算
                    ApprovalTotalPriceManager();

                    //入力値変更フラグ設定
                    $.data(this, "strFlg", "0");
                } else {
                    $.data(this, "strFlg", "1");
                }
            },
            open: function () {
                var strDefValue = $(this).val();
                $(this).NumericKeypad("setValue", strDefValue);
            },
            close: function () {
                var strValue = $.data(this, "strFlg");
                if (strValue == "0") {
                    $.data(this, "strFlg", "");
                    return true;
                } else if (strValue == "1") {
                    $.data(this, "strFlg", "");
                    alert(SC3070201HTMLDecode(this_form.approvalDiscountMsgHiddenField.value));
                    return false;
                } else {
                    return true;
                }
            }
        });
    });
});

$(function () {
    //回答入力非表示ボタン押下
    $("#CloseButton").bind("click", function () {
        $("#tcvNcv206Main").hide(0);
    });
});
