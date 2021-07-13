/**
* @fileOverview SC3070201　見積作成画面処理
*
* @author TCS 葛西
* @version 1.0.0
* 更新： 2012/02/03 TCS 葛西  【SALES_1A】号口(課題No.47)対応
*/



/**************************************************************
* DOMロード時の処理
**************************************************************/
$(function () {

    //全体スクロール
    // $99 Ken-Suzuki Modify Start
    //  $("#tcvNcv50Main").fingerScroll();
    $("#tcvNcv50Main").VScroll();
    // $99 Ken-Suzuki Modify End

    //販売店オプション合計欄書式設定
    $(".TableOptionSum").CustomLabel({ useEllipsis: true });

    //下取り合計額欄の書式設定
    $("#TradeInCarTotalPriceTotalLabel").CustomLabel({ useEllipsis: true });



    //動的行追加、削除
    $.fn.observeValue = function (callback, options) {
        var default_options = {
            interval: 100
        };

        options = $.extend(default_options, options || {});
        return this.each(function () {
            if (typeof this.value == 'undefined') return;
            var tid;
            var self = this;
            var elm = $(self);
            elm.focus(function () {
                tid = setInterval(function () {
                    callback.call(self, elm.val())
                }, options.interval);
            });
            elm.blur(function () {
                clearInterval(tid);
            });
        })
    }



    //金額フォーマット修正

    //車両価格
    // $99 Modify Start
//  var basePriceValue = $("#basePriceHiddenField").val();
    var basePriceValue = parseFloat($("#basePriceHiddenField").val());
    var extPriceValue = parseFloat($("#extOptionPriceHiddenField").val());
    var intPriceValue = parseFloat($("#intOptionPriceHiddenField").val());
    // $99 Modify End

    var lblBasePrice = document.getElementById("basePriceLabel");
    // $99 Modify Start
//  lblBasePrice.innerText = formatNumber(basePriceValue);
    lblBasePrice.innerText = formatNumber(Math.round(basePriceValue + extPriceValue + intPriceValue));
    // $99 Modify End

    //内装、外装オプション、メーカーオプション価格
    var tblOption = document.getElementById("tblOption");
    var rowOpt = tblOption.rows.length;
    for (i = 1; i < rowOpt; i++) {

        var tdOptionPriceValue = tblOption.rows[i].cells[1].innerText;
        var tdOptionMoneyTalValue = tblOption.rows[i].cells[3].innerText;

        if (tdOptionPriceValue != "") {
            tdOptionPriceValue = parseFloat(tdOptionPriceValue);
            tblOption.rows[i].cells[1].innerText = formatNumber(tdOptionPriceValue);
        }
        if (tdOptionMoneyTalValue != "") {
            tdOptionMoneyTalValue = parseFloat(tdOptionMoneyTalValue);
            tblOption.rows[i].cells[3].innerText = formatNumber(tdOptionMoneyTalValue);
        }

    }

    //販売店オプション価格
    var tblDlr = document.getElementById("tblDlrOption");
    var rowDlr = tblDlr.rows.length;
    for (i = 0; i < rowDlr - 2; i++) {

        var tdOptionPriceValue = tblDlr.rows[i].cells[1].getElementsByTagName("input")[0].value;
        var tdOptionMoneyValue = tblDlr.rows[i].cells[2].getElementsByTagName("input")[0].value;

        var totalData = 0.00;

        if (tdOptionPriceValue != "") {
            totalData = Math.round((parseFloat(totalData) + parseFloat(tdOptionPriceValue)) * 100) / 100;
        }
        if (tdOptionMoneyValue != "") {
            totalData = Math.round((parseFloat(totalData) + parseFloat(tdOptionMoneyValue)) * 100) / 100;
        }


        tblDlr.rows[i].cells[1].getElementsByTagName("input")[0].value = formatNumber(tdOptionPriceValue);
        tblDlr.rows[i].cells[2].getElementsByTagName("input")[0].value = formatNumber(tdOptionMoneyValue);

        tblDlr.rows[i].cells[3].innerText = formatNumber(totalData);

    }



    //オプション合計額計算
    totalOption();



    //諸費用金額フォーマット修正
    //車両購入税
    // $99 Ken-Suzuki Modify Start
//  var tdCarBuyTax = document.getElementById("CarBuyTaxCustomLabel").innerHTML;
    var tdCarBuyTax = $("#carBuyTaxHiddenField").val();
    // $99 Ken-Suzuki Modify End
    if (tdCarBuyTax != "") {
        tdCarBuyTax = parseFloat(tdCarBuyTax);
        document.getElementById("CarBuyTaxCustomLabel").innerHTML = formatNumber(tdCarBuyTax);
    }
    //登録費用
    var tdRegCostValue;
    tdRegCostValue = $("#regCostValueHiddenField").val();
    if ((this_form.ReferenceModeHiddenField.value).toUpperCase() == "TRUE") {

        var lblRegPrice = document.getElementById("regPriceLabel");
        lblRegPrice.innerText = formatNumber(tdRegCostValue);

    } else {

        $("#regPriceTextBox").val(formatNumber(tdRegCostValue));
    }



    //諸費用合計額計算
    chargeTotal();

    //保険金額フォーマット修正
    //年額
    var tdInsAmountValue;

    tdInsAmountValue = $("#insuAmountValueHiddenField").val();
    if ((this_form.ReferenceModeHiddenField.value).toUpperCase() == "TRUE") {

        var lblInsuAmount = document.getElementById("insuranceAmountLabel");
        lblInsuAmount.innerText = formatNumber(tdInsAmountValue);

    } else {

        $("#insuranceAmountTextBox").val(formatNumber(tdInsAmountValue));

    }

    //お支払い方法金額フォーマット修正
    //□現金
    //頭金
    var tdCashDeposit;
    tdCashDeposit = $("#cashDepositValueHiddenField").val();
    if ((this_form.ReferenceModeHiddenField.value).toUpperCase() == "TRUE") {

        var lblCashDeposit = document.getElementById("cashDepositLabel");
        lblCashDeposit.innerText = formatNumber(tdCashDeposit);
    } else {

        $("#cashDepositTextBox").val(formatNumber(tdCashDeposit));
    }
    //□ローン
    //月額
    //頭金
    //ボーナス
    var tdLoanMonthlyPay;
    var tdLoanDeposit;
    var tdLoanBonusPay;
    tdLoanMonthlyPay = $("#loanMonthlyValueHiddenField").val();
    tdLoanDeposit = $("#loanDepositValueHiddenField").val();
    tdLoanBonusPay = $("#loanBonusValueHiddenField").val();

    if ((this_form.ReferenceModeHiddenField.value).toUpperCase() == "TRUE") {

        var lblLoanMonthPay = document.getElementById("loanMonthlyPayLabel");
        lblLoanMonthPay.innerText = formatNumber(tdLoanMonthlyPay);
        var lblLoanDeposit = document.getElementById("loanDepositLabel");
        lblLoanDeposit.innerText = formatNumber(tdLoanDeposit);
        var lblLoanBonus = document.getElementById("loanBonusPayLabel");
        lblLoanBonus.innerText = formatNumber(tdLoanBonusPay);

    } else {

        $("#loanMonthlyPayTextBox").val(formatNumber(tdLoanMonthlyPay));
        $("#loanDepositTextBox").val(formatNumber(tdLoanDeposit));
        $("#loanBonusPayTextBox").val(formatNumber(tdLoanBonusPay));

    }




    //金額フォーマット修正
    //下取り車両価格
    var tblTradInCar = document.getElementById("tblTradeInCar");
    var rowCar = tblTradInCar.rows.length;

    for (i = 1; i < rowCar - 2; i++) {
        var tdCarPrice = tblTradInCar.rows[i].cells[1].getElementsByTagName("input")[0].value;
        tblTradInCar.rows[i].cells[1].getElementsByTagName("input")[0].value = formatNumber(tdCarPrice);

    }

    //下取り合計額計算
    tradeInCarSum();


    //値引き額金額フォーマット修正
    var tdDiscountPrice;

    tdDiscountPrice = $("#discountPriceValueHiddenField").val();
    if ((this_form.ReferenceModeHiddenField.value).toUpperCase() == "TRUE") {

        var lblDiscountPrice = document.getElementById("discountPriceLabel");
        lblDiscountPrice.innerText = formatNumber(tdDiscountPrice);
    } else {

        $("#discountPriceTextBox").val(formatNumber(tdDiscountPrice));

    }



    //値引き額非表示
    if (this_form.discountPriceValueHiddenField.value == "") {
        $("#divDiscountPriceArea").hide(0);
    }

    //支払い総額計算
    totalPrice();



    //販売店オプション欄(行追加、削除)
    $(".TableTextArea1")
	.CustomTextBox({
	    clear: function () {
	        inputChangedClient();
	    }
	})
	    .observeValue(function () {
	        setTextOption($(this));
	    });

    //販売店オプション価格
    $(".TableTextArea2")
	.observeValue(function () {
	    setTextOption($(this));
	})
    //キーボートを追加する
    .NumericKeypad({
        acceptDecimalPoint: true,
        defaultValue: 0,
        completionLabel: $("#numericKeyPadDoneHiddenField").val(),
        cancelLabel: $("#numericKeyPadCancelHiddenField").val(),
        valueChanged: function (num) {
            if (num.match(/^[0-9]{1,9}(\.[0-9]{1,2})?$/) || (num == "")) {

                var numberFormat = num;
                var numberFormatReturn = formatNumber(numberFormat);
                $(this).val(numberFormatReturn);

                //販売店オプション行、オプション合計額計算
                totalDlrOptionSum($(this));

                //支払い総額計算
                totalPrice();

                //入力値変更フラグ設定
                inputChangedClient();

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
            //監視関数とセルのフォーマート設定に合わせるため、下記処理が必要
            var strValue = $.data(this, "strFlg");

            if (strValue == "0") {

                $.data(this, "strFlg", "");
                setTextOption($(this));
                return true;
            } else if (strValue == "1") {
                $.data(this, "strFlg", "");
                alert(this_form.optionPriceMsgHiddenField.value);
                return false;
            } else {

                setTextOption($(this));
            }
        }
    });


    //販売店オプション取り付け額
    $(".TableTextArea3")
	.observeValue(function () {
	    setTextOption($(this));
	})
    //キーボートを追加する
    .NumericKeypad({
        acceptDecimalPoint: true,
        defaultValue: 0,
        completionLabel: $("#numericKeyPadDoneHiddenField").val(),
        cancelLabel: $("#numericKeyPadCancelHiddenField").val(),
        valueChanged: function (num) {
            if (num.match(/^[0-9]{1,9}(\.[0-9]{1,2})?$/) || (num == "")) {

                var numberFormat = num;
                var numberFormatReturn = formatNumber(numberFormat);
                $(this).val(numberFormatReturn);

                //販売店オプション行、オプション合計額計算
                totalDlrOptionSum($(this));

                //支払い総額計算
                totalPrice();

                //入力値変更フラグ設定
                inputChangedClient();

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
            //監視関数とセルのフォーマート設定に合わせるため、下記処理が必要
            var strValue = $.data(this, "strFlg");

            if (strValue == "0") {

                $.data(this, "strFlg", "");
                setTextOption($(this));
                return true;
            } else if (strValue == "1") {
                $.data(this, "strFlg", "");
                alert(this_form.optionInstallFeeMsgHiddenField.value);
                return false;
            } else {

                setTextOption($(this));
            }
        }
    });


    //下取り車両欄(行追加、削除)
    $(".TradeInCarTextArea1")
        .CustomTextBox({
            clear: function () {
                inputChangedClient();
            }
        })
        .observeValue(function () {
            tradeInCarSet($(this));
        });

    $(".TradeInCarTextArea2")
        .observeValue(function () {
            tradeInCarSet($(this));
        })
    //キーボードを追加する
        .NumericKeypad({
            acceptDecimalPoint: true,
            defaultValue: 0,
            completionLabel: $("#numericKeyPadDoneHiddenField").val(),
            cancelLabel: $("#numericKeyPadCancelHiddenField").val(),
            valueChanged: function (num) {
                if (num.match(/^[0-9]{1,9}(\.[0-9]{1,2})?$/) || (num == "")) {
                    var numberFormat = num;
                    var numberFormatReturn = formatNumber(numberFormat);
                    $(this).val(numberFormatReturn);
                    //下取り車両合計計算
                    tradeInCarSum();
                    //支払い総額計算
                    totalPrice();
                    //入力値変更フラグ設定
                    inputChangedClient();
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
                //監視関数のため、focus,focusout処理が必要
                var strValue = $.data(this, "strFlg");
                if (strValue == "0") {
                    $.data(this, "strFlg", "");
                    tradeInCarSet($(this));
                    return true;
                } else if (strValue == "1") {
                    $.data(this, "strFlg", "");
                    alert(this_form.tradeInPriceMsgHiddenField.value);
                    return false;
                } else {
                    tradeInCarSet($(this));
                    return true;
                }
            }
        });


    //登録費用
    $(".regCost").NumericKeypad({
        acceptDecimalPoint: true,
        defaultValue: 0,
        completionLabel: $("#numericKeyPadDoneHiddenField").val(),
        cancelLabel: $("#numericKeyPadCancelHiddenField").val(),
        valueChanged: function (num) {
            if (num.match(/^[0-9]{1,9}(\.[0-9]{1,2})?$/) || (num == "")) {
                var numberFormat = num;
                var numberFormatReturn = formatNumber(numberFormat);
                $(this).val(numberFormatReturn);
                $("#regCostValueHiddenField").val(numberFormatReturn);
                //諸費用合計計算
                chargeTotal();
                //支払い総額計算
                totalPrice();
                //入力値変更フラグ設定
                inputChangedClient();
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
            var strRslt = $.data(this, "strFlg");
            if (strRslt == "0") {
                $.data(this, "strFlg", "");
                return true;
            } else if (strRslt == "1") {
                $.data(this, "strFlg", "");
                alert(this_form.regFeeMsgHiddenField.value);
                return false;
            } else {
                return true;
            }
        }

    });

    //保険年額
    $(".insuAmount").NumericKeypad({
        acceptDecimalPoint: true,
        defaultValue: 0,
        completionLabel: $("#numericKeyPadDoneHiddenField").val(),
        cancelLabel: $("#numericKeyPadCancelHiddenField").val(),
        valueChanged: function (num) {
            if (num.match(/^[0-9]{1,9}(\.[0-9]{1,2})?$/) || (num == "")) {
                var numberFormat = num;
                var numberFormatReturn = formatNumber(numberFormat);
                $(this).val(numberFormatReturn);
                $("#insuAmountValueHiddenField").val(numberFormatReturn);
                //支払い総額計算
                totalPrice();
                //入力値変更フラグ設定
                inputChangedClient();
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
            var strRslt = $.data(this, "strFlg");
            if (strRslt == "0") {
                $.data(this, "strFlg", "");
                return true;
            } else if (strRslt == "1") {
                $.data(this, "strFlg", "");
                alert(this_form.insuranceFeeMsgHiddenField.value);
                return false;
            } else {
                return true;
            }
        }
    });

    //頭金（現金）
    $(".cashDeposit").NumericKeypad({
        acceptDecimalPoint: true,
        defaultValue: 0,
        completionLabel: $("#numericKeyPadDoneHiddenField").val(),
        cancelLabel: $("#numericKeyPadCancelHiddenField").val(),
        valueChanged: function (num) {
            if (num.match(/^[0-9]{1,9}(\.[0-9]{1,2})?$/) || (num == "")) {
                var numberFormat = num;
                var numberFormatReturn = formatNumber(numberFormat);
                $(this).val(numberFormatReturn);
                $("#cashDepositValueHiddenField").val(numberFormatReturn);
                //入力値変更フラグ設定
                inputChangedClient();
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
            var strRslt = $.data(this, "strFlg");
            if (strRslt == "0") {
                $.data(this, "strFlg", "");
                return true;
            } else if (strRslt == "1") {
                $.data(this, "strFlg", "");
                alert(this_form.cashDownMsgHiddenField.value);
                return false;
            } else {
                return true;
            }
        }
    });


    //月額（ローン）
    $(".loanMonthlyPay").NumericKeypad({
        acceptDecimalPoint: true,
        defaultValue: 0,
        completionLabel: $("#numericKeyPadDoneHiddenField").val(),
        cancelLabel: $("#numericKeyPadCancelHiddenField").val(),
        valueChanged: function (num) {
            if (num.match(/^[0-9]{1,9}(\.[0-9]{1,2})?$/) || (num == "")) {
                var numberFormat = num;
                var numberFormatReturn = formatNumber(numberFormat);
                $(this).val(numberFormatReturn);
                $("#loanMonthlyValueHiddenField").val(numberFormatReturn);
                //入力値変更フラグ設定
                inputChangedClient();
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
            var strRslt = $.data(this, "strFlg");
            if (strRslt == "0") {
                $.data(this, "strFlg", "");
                return true;
            } else if (strRslt == "1") {
                $.data(this, "strFlg", "");
                alert(this_form.loanMonthlyPayMsgHiddenField.value);
                return false;
            } else {
                return true;
            }
        }

    });


    //頭金（ローン）
    $(".loanDeposit").NumericKeypad({
        acceptDecimalPoint: true,
        defaultValue: 0,
        completionLabel: $("#numericKeyPadDoneHiddenField").val(),
        cancelLabel: $("#numericKeyPadCancelHiddenField").val(),
        valueChanged: function (num) {
            if (num.match(/^[0-9]{1,9}(\.[0-9]{1,2})?$/) || (num == "")) {
                var numberFormat = num;
                var numberFormatReturn = formatNumber(numberFormat);
                $(this).val(numberFormatReturn);
                $("#loanDepositValueHiddenField").val(numberFormatReturn);
                //入力値変更フラグ設定
                inputChangedClient();
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
                alert(this_form.loanDownMsgHiddenField.value);
                return false;
            } else {
                return true;
            }
        }
    });


    //ボーナス（ローン）
    $(".loanBonus").NumericKeypad({
        acceptDecimalPoint: true,
        defaultValue: 0,
        completionLabel: $("#numericKeyPadDoneHiddenField").val(),
        cancelLabel: $("#numericKeyPadCancelHiddenField").val(),
        valueChanged: function (num) {
            if (num.match(/^[0-9]{1,9}(\.[0-9]{1,2})?$/) || (num == "")) {
                var numberFormat = num;
                var numberFormatReturn = formatNumber(numberFormat);
                $(this).val(numberFormatReturn);
                $("#loanBonusValueHiddenField").val(numberFormatReturn);
                //入力値変更フラグ設定
                inputChangedClient();
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
                alert(this_form.loanBonusMsgHiddenField.value);
                return false;
            } else {
                return true;
            }
        }
    });

    //値引き額
    $(".discountPrice").NumericKeypad({
        acceptDecimalPoint: true,
        defaultValue: 0,
        completionLabel: $("#numericKeyPadDoneHiddenField").val(),
        cancelLabel: $("#numericKeyPadCancelHiddenField").val(),
        valueChanged: function (num) {
            if (num.match(/^[0-9]{1,9}(\.[0-9]{1,2})?$/) || (num == "")) {
                var numberFormat = num;
                var numberFormatReturn = formatNumber(numberFormat);
                $(this).val(numberFormatReturn);
                $("#discountPriceValueHiddenField").val(numberFormatReturn);
                // $99 Ken-Suzuki Add Start
                //車両購入税再計算
                setCarBuyTax();
                // 諸費用合計計算
                chargeTotal();
                // $99 Ken-Suzuki Add End
                //支払い総額計算
                totalPrice();
                //入力値変更フラグ設定
                inputChangedClient();
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
                alert(this_form.discountMsgHiddenField.value);
                return false;
            } else {
                return true;
            }
        }

    });


    /*  車両画像のファイルパスを設定 */
    var carImgSrc = this_form.carImgFileHidden.value;
    $("#carImg").attr("src", carImgSrc);


    //セグメントボタン初期表示
    $("#custClassSegmentedButton_0").click();
    $("#payMethodSegmentedButton_0").click();

    //契約完了後、現金／ローン表示設定
    if ($("#contractAfterFlgHiddenField").val() == "1") {

        $("#payMethodSegmentedButton_0").css("display", "none");
        $("#payMethodSegmentedButton_1").css("display", "none");

        if ($("#payMethodHiddenField").val() == "1") {

            $("#cash").css("display", "block");
            $("#loan").css("display", "none");
            $("#payMethodSegmentedButton_0").parent().parent().addClass("icrop-selected");
        } else {

            $("#cash").css("display", "none");
            $("#loan").css("display", "block");
            $("#payMethodSegmentedButton_1").parent().parent().addClass("icrop-selected");

        }
        $("#payMethodSegmentedButton").addClass("SwitchButton1");

    }

    //保険会社名表示
    var insucomHidden = this_form.InsComInsuComCdHidden.value;
    var insdvsHidden = this_form.InsComInsuKubunHidden.value;
    var inscomnameHidden = this_form.InsComInsuComNameHidden.value;
    var selectinscomCdHidden = this_form.SelectInsuComCdHidden.value;
    var insucom = insucomHidden.split(",");
    var insudvs = insdvsHidden.split(",");
    var insucomName = inscomnameHidden.split(",");


    for (var i = 0; i < insucom.length; i++) {
        if (insucom[i] == selectinscomCdHidden) {
            //保険会社名をラベルに表示 
            this_form.SelectInsuComNmHidden.value = insucomName[i];
        }
    }

    //保険種別表示
    //保険種類リスト作成
    var insKindinsucomHidden = this_form.InsKindInsuComCdHidden.value;
    var insKindinskindHidden = this_form.InsKindInsuKindCdHidden.value;
    var insKindinskindnameHidden = this_form.InsKindInsuKindNmHidden.value;
    var selectinskindCdHidden = this_form.SelectInsuKindCdHidden.value;
    var insKindinsucom = insKindinsucomHidden.split(",");
    var insKindinsukind = insKindinskindHidden.split(",");
    var insKindinsukindName = insKindinskindnameHidden.split(",");

    for (var i = 0; i < insKindinsucom.length; i++) {
        if (insKindinsucom[i] == selectinscomCdHidden) {
            //保険種別をラベルに表示
            if (insKindinsukind[i] == selectinskindCdHidden) {
                this_form.SelectInsuKindNmHidden.value = insKindinsukindName[i];
            }
        }
    }



    if ((this_form.ReferenceModeHiddenField.value).toUpperCase() == "TRUE") {
        //参照モード時

        setDlrOptionDesible();
        setTradeInCarDesible();

        //コピーボタン非活性
        this_form.copyButton.disabled = true;

        //メモ欄大きさ固定
        $("#memoTextBox").addClass("TextAreaSet");


        //所有者顧客区分表示
        var strLabelKojin = document.getElementById("CustomLabelShoyusyaKojinLock");
        var strLabelHojin = document.getElementById("CustomLabelShoyusyaHojinLock");
        var imgKojinCheck = document.getElementById("imgChkKojinLock");
        var imgHojinCheck = document.getElementById("imgChkHojinLock");

        if (this_form.shoyusyaKojinCheckMark.value == "TRUE") {
            strLabelKojin.style.color = "#324F85";
            imgKojinCheck.style.display = "block";
            strLabelHojin.style.display = "none";
            imgHojinCheck.style.display = "none";


            //敬称表示
            $("#shoyusyaKeisyoMaeLabel").show(0);
            $("#shoyusyaKeisyoAtoLabel").show(0);

        }
        else {
            strLabelHojin.style.color = "#324F85";
            imgHojinCheck.style.display = "block";
            strLabelKojin.style.display = "none";
            imgKojinCheck.style.display = "none";


            //敬称非表示
            $("#shoyusyaKeisyoMaeLabel").hide();
            $("#shoyusyaKeisyoAtoLabel").hide();
        }

        //使用者顧客区分表示
        var strLabelshiyosyaKojin = document.getElementById("CustomLabelshiyosyaKojinLock");
        var strLabelshiyosyaHojin = document.getElementById("CustomLabelshiyosyaHojinLock");
        var imgshiyosyaKojinCheck = document.getElementById("imgChkShiyosyaKojinLock");
        var imgshiyosyaHojinCheck = document.getElementById("imgChkShiyosyaHojinLock");

        if (this_form.shiyosyaKojinCheckMark.value == "TRUE") {
            strLabelshiyosyaKojin.style.color = "#324F85";
            imgshiyosyaKojinCheck.style.display = "block";
            strLabelshiyosyaHojin.style.display = "none";
            imgshiyosyaHojinCheck.style.display = "none";

            //敬称表示
            $('#shiyosyaKeisyoMaeLabel').show(0);
            $('#shiyosyaKeisyoAtoLabel').show(0);
        }
        else {
            strLabelshiyosyaHojin.style.color = "#324F85";
            imgshiyosyaHojinCheck.style.display = "block";
            strLabelshiyosyaKojin.style.display = "none";
            imgshiyosyaKojinCheck.style.display = "none";

            //敬称非表示
            $('#shiyosyaKeisyoMaeLabel').hide();
            $('#shiyosyaKeisyoAtoLabel').hide();
        }

        //保険会社区分(自社他社)表示 lock
        var strLabelJisya = document.getElementById("CustomLabelJisyaLock");
        var strLabelTasya = document.getElementById("CustomLabelTasyaLock");
        var imgJisyaCheck = document.getElementById("imgChkJisyaLock");
        var imgTasyaCheck = document.getElementById("imgChkTasyaLock");
        if (this_form.jisyaCheckMark.value == "TRUE") {
            strLabelJisya.style.color = "#324F85";
            imgJisyaCheck.style.display = "block";
            strLabelTasya.style.display = "none";
            imgTasyaCheck.style.display = "none";
        }
        else {
            strLabelTasya.style.color = "#324F85";
            imgTasyaCheck.style.display = "block";
            strLabelJisya.style.display = "none";
            imgJisyaCheck.style.display = "none";
        }

        //保険会社名をラベルに表示
        $("#insuComLabel").text(this_form.SelectInsuComNmHidden.value);
        //保険種別をラベルに表示
        $("#insuComKindLabel").text(this_form.SelectInsuKindNmHidden.value);
        //融資会社ＤＥＢＵＧ表示(ロック時)
        listname = "#loanFinanceComList";
        listvalue = this_form.SelectFinanceComHiddenField.value;
		$("#loanFinanceComLabel").text($(listname + listvalue).attr("title"));
		$("#NebikiHideButton").css("display", "none");

        // $99 Ken-Suzuki Add Start
        $("#chargeSegmentedButton_0").css("display", "none");
        $("#chargeSegmentedButton_1").css("display", "none");

        if (document.getElementById("chargeSegmentedButton_0").checked) {
            $("#chargeSegmentedButton_0").parent().parent().addClass("icrop-selected");
        } else {
            $("#chargeSegmentedButton_1").parent().parent().addClass("icrop-selected");
        }
        $("#chargeSegmentedButton").addClass("SwitchButton1");
        // $99 Ken-Suzuki Add End

    } else {
        //通常時


        //所有者顧客区分表示
        var strLabelKojin = document.getElementById("CustomLabelShoyusyaKojin");
        var strLabelKojinSelected = document.getElementById("CustomLabelShoyusyaKojinSelected");
        var strLabelHojin = document.getElementById("CustomLabelShoyusyaHojin");
        var strLabelHojinSelected = document.getElementById("CustomLabelShoyusyaHojinSelected");
        var imgKojinCheck = document.getElementById("imgChkKojin");
        var imgHojinCheck = document.getElementById("imgChkHojin");
        if (this_form.shoyusyaKojinCheckMark.value == "TRUE") {
            strLabelKojin.style.color = "#324F85";
            imgKojinCheck.style.display = "block";
            strLabelHojin.style.color = "#A6A6A6";
            imgHojinCheck.style.display = "none";

            //敬称表示
            $("#shoyusyaKeisyoMaeLabel").show(0);
            $("#shoyusyaKeisyoAtoLabel").show(0);
        }
        else {
            strLabelKojin.style.color = "#A6A6A6";
            imgKojinCheck.style.display = "none";
            strLabelHojin.style.color = "#324F85";
            imgHojinCheck.style.display = "block";

            //敬称非表示
            $("#shoyusyaKeisyoMaeLabel").hide();
            $("#shoyusyaKeisyoAtoLabel").hide();
        }

        //使用者顧客区分表示
        var strLabelshiyosyaKojin = document.getElementById("CustomLabelshiyosyaKojin");
        var strLabelshiyosyaKojinSelected = document.getElementById("CustomLabelshiyosyaKojinSelected");
        var strLabelshiyosyaHojin = document.getElementById("CustomLabelshiyosyaHojin");
        var strLabelshiyosyaHojinSelected = document.getElementById("CustomLabelshiyosyaHojinSelected");
        var imgshiyosyaKojinCheck = document.getElementById("imgChkShiyosyaKojin");
        var imgshiyosyaHojinCheck = document.getElementById("imgChkShiyosyaHojin");

        if (this_form.shiyosyaKojinCheckMark.value == "TRUE") {
            strLabelshiyosyaKojin.style.color = "#324F85";
            imgshiyosyaKojinCheck.style.display = "block";
            strLabelshiyosyaHojin.style.color = "#A6A6A6";
            imgshiyosyaHojinCheck.style.display = "none";

            //敬称表示
            $('#shiyosyaKeisyoMaeLabel').show(0);
            $('#shiyosyaKeisyoAtoLabel').show(0);
        }
        else {
            strLabelshiyosyaKojin.style.color = "#A6A6A6";
            imgshiyosyaKojinCheck.style.display = "none";
            strLabelshiyosyaHojin.style.color = "#324F85";
            imgshiyosyaHojinCheck.style.display = "block";
            //1224
            //敬称非表示
            $('#shiyosyaKeisyoMaeLabel').hide();
            $('#shiyosyaKeisyoAtoLabel').hide();
        }

        //保険会社区分(自社他社)表示
        var strLabelJisya = document.getElementById("CustomLabelJisya");
        var strLabelJisyaSelected = document.getElementById("CustomLabelJisyaSelected");
        var strLabelTasya = document.getElementById("CustomLabelTasya");
        var strLabelTasyaSelected = document.getElementById("CustomLabelTasyaSelected");
        var imgJisyaCheck = document.getElementById("imgChkJisya");
        var imgTasyaCheck = document.getElementById("imgChkTasya");

        //自社
        if (this_form.jisyaCheckMark.value == "TRUE") {
            strLabelJisya.style.color = "#324F85";
            imgJisyaCheck.style.display = "block";
            strLabelTasya.style.color = "#A6A6A6";
            imgTasyaCheck.style.display = "none";

        }
        //他社
        else {
            strLabelTasya.style.color = "#324F85";
            imgTasyaCheck.style.display = "block";
            strLabelJisya.style.color = "#A6A6A6";
            imgJisyaCheck.style.display = "none";

        }

        //保険会社名をラベルに表示
        $("#dispSelectedInsCom").text(this_form.SelectInsuComNmHidden.value);
        //保険種別をラベルに表示
        $("#dispSelectedInsKind").text(this_form.SelectInsuKindNmHidden.value);
        //融資会社初期表示(通常時)
        listname = "#loanFinanceComList";
        listvalue = this_form.SelectFinanceComHiddenField.value;
        $(listname + listvalue).addClass("Selection");
        $(".loanFinanceComdiv").text($(listname + listvalue).attr("title"));

    }

    //納車予定日初期値設定
    if (this_form.initialFlgHiddenField.value == "") {

        if ((this_form.ReferenceModeHiddenField.value).toUpperCase() != "TRUE") {
            this_form.deliDateInitialValueHiddenField.value = deliDateDateTimeSelector.value;
            this_form.initialFlgHiddenField.value = "1";
        }

    }

});
/**************************************************************
* DOMロード時の処理　終わり
**************************************************************/


/**************************************************************
* イベント処理
**************************************************************/
$(function () {

    //長押しで値引き額表示
    $("#divTotalPriceArea")
		.bind("mousedown touchstart", function (event) {
		    if ((this_form.ReferenceModeHiddenField.value).toUpperCase() == "FALSE") {
		        $(this).css('opacity', 0.35);
		        var taskTarget = $(this);
		        taskTarget.data("tapHold", setTimeout(function () {
		            taskTarget.data("tapHold", null);
		            taskTarget.css('opacity', 1.0);
		            $("#divDiscountPriceArea").show(100);
		        }, 2000));
		    }
		})
	    .bind("mouseup mouseout touchend", function (event) {
	        $(this).css('opacity', 1.0);
	        if ($(this).data("tapHold")) {
	            clearTimeout($(this).data("tapHold"));
	            $(this).data("tapHold", null);
	        }
	    });

    //値引き額非表示ボタン押下
    $("#NebikiHideButton").bind("click", function () {
        if ((this_form.ReferenceModeHiddenField.value).toUpperCase() == "FALSE") {

            $("#discountPriceTextBox").val("");
            $("#discountPriceValueHiddenField").val("")
            //支払い総額計算
            totalPrice();
            //入力値変更フラグ設定
            inputChangedClient();
            $("#divDiscountPriceArea").hide(0);
        }
    });

});



//保険会社リスト表示
$(function () {
	$("#InsComSelector").PopOverForm({
	    render: function (pop, index, args, container, header) {

			//閉じるボタンをポップアップのヘッダに追加(非表示)
            commitButton = $("<button id='BtInsComPopClose' type='button' style='display:none' >Close</button>");

            commitButton
                .click(function (e) {
                    pop.closePopOver();
                })
                header.append(commitButton);

			//保険会社リストをポップアップに追加
		    var insucomHidden = this_form.InsComInsuComCdHidden.value;
		    var insdvsHidden = this_form.InsComInsuKubunHidden.value;
		    var inscomnameHidden = this_form.InsComInsuComNameHidden.value;
		    var insucom = insucomHidden.split(",");
		    var insudvs = insdvsHidden.split(",");
		    var insucomName = inscomnameHidden.split(",");
			var Chkinsudvs;
			if(this_form.jisyaCheckMark.value == "TRUE"){
				Chkinsudvs = "1"
			}
			else{
				Chkinsudvs = "2"
			}
	
			var InsComList = $('<div class="nscListBoxSetIn" >');
			if(this_form.SelectInsuComCdHidden.value == ""){
				InsComList.append(('<li class="InsComListLi Selection" insucomcd="" insucomname="" >&nbsp</li>'));
			}else{
				InsComList.append(('<li class="InsComListLi" insucomcd="" insucomname="" >&nbsp</li>'));
			}
	
		    for (var i = 0; i < insucom.length; i++) {
		    	if (insudvs[i] == Chkinsudvs) {
					if(insucom[i] == this_form.SelectInsuComCdHidden.value ){
						InsComList.append(('<li class="InsComListLi Selection" insucomcd="' + insucom[i] + '" insucomname=' + insucomName[i] + '>' + insucomName[i] + '</li>'));
					}
					else{
						InsComList.append(('<li class="InsComListLi" insucomcd="' + insucom[i] + '" insucomname=' + insucomName[i] + '>' + insucomName[i] + '</li>'));
					}
				}
		    }
			pop.resize(330, 235);
		    InsComList.append(('</div>'));
			container.empty().append(InsComList);                    
		}
	});
});

//保険会社を選択した時のイベント
$(".icrop-PopOverForm-sheet li.InsComListLi").live("click", function (e) {
	//保険会社コード取得
	var insucomcd = $(this).attr("insucomcd");
	var insucomname = $(this).attr("insucomname");


	//選択された保険会社コード,名称を格納
	this_form.SelectInsuComCdHidden.value=insucomcd;
	this_form.SelectInsuComNmHidden.value = insucomname;

	//ラベルに表示
    $("#dispSelectedInsCom").text(insucomname);
    
    //選択している保険種別をクリア
	$("#dispSelectedInsKind").text("");
	this_form.SelectInsuKindCdHidden.value = "";
	this_form.SelectInsuKindNmHidden.value = "";
	
	//閉じるボタン(非表示)を押下し、ポップアップ終了
	$("#BtInsComPopClose").click();

    //入力値変更フラグ設定
    inputChangedClient();

});

//保険種類リスト表示
$(function () {
	$("#InsKindSelector").PopOverForm({
		//保険会社が選択されていない場合はリストを表示しない
		open: function () {
			if(this_form.SelectInsuComCdHidden.value == ""){
				return false;
			}
		},
		render: function (pop, index, args, container, header) {
		//閉じるボタンをポップアップのヘッダに追加(非表示)
		commitButton = $("<button id='BtInsKindPopClose' type='button' style='display:none' >Close</button>");
		commitButton
		    .click(function (e) {
		        pop.closePopOver();
		    })
                header.append(commitButton);

		var insucomcd = this_form.SelectInsuComCdHidden.value;

		//保険種類リスト作成
		var insucomHidden = this_form.InsKindInsuComCdHidden.value;
		var inskindHidden = this_form.InsKindInsuKindCdHidden.value;
		var inskindnameHidden = this_form.InsKindInsuKindNmHidden.value;
		var insucom = insucomHidden.split(",");
		var insukind = inskindHidden.split(",");
		var insukindName = inskindnameHidden.split(",");
	
		var InsKindList = $('<div class="nscListBoxSetIn" >');
		if(this_form.SelectInsuKindCdHidden.value == ""){
			InsKindList.append(('<li class="InsKindListLi Selection" insukindcd="" insukindname="" >&nbsp</li>'));
		}else{
			InsKindList.append(('<li class="InsKindListLi" insukindcd="" insukindname="" >&nbsp</li>'));
		}
		
	    for (var i = 0; i < insucom.length; i++) {
	    	if (insucom[i] == insucomcd) {
	    		if(insukind[i] == this_form.SelectInsuKindCdHidden.value){
					InsKindList.append(('<li class="InsKindListLi Selection" insukindcd="' + insukind[i] + '" insukindname=' + insukindName[i] + '>' + insukindName[i] + '</li>'));
				}
				else{
					InsKindList.append(('<li class="InsKindListLi" insukindcd="' + insukind[i] + '" insukindname=' + insukindName[i] + '>' + insukindName[i] + '</li>'));
				}
			}
	    }
		    
		pop.resize(330, 235);
		InsKindList.append(('</div>'));
		container.empty().append(InsKindList);

		}
	});
});

//保険種類を選択した時のイベント
$(".icrop-PopOverForm-sheet li.InsKindListLi").live("click", function (e) {
	//保険会社コード取得
	var insukindcd = $(this).attr("insukindcd");
	var insukindname = $(this).attr("insukindname");
	
	//選択された保険種類コード,名称を格納
	this_form.SelectInsuKindCdHidden.value=insukindcd;
	this_form.SelectInsuKindNmHidden.value=insukindname;
	
	//ラベルに表示
    $("#dispSelectedInsKind").text(insukindname);
    
	//閉じるボタン(非表示)を押下し、ポップアップ終了
	$("#BtInsKindPopClose").click();

    //入力値変更フラグ設定
    inputChangedClient();
});


//融資会社セレクトリストスクロール化
$(function () {
    // $99 Ken-Suzuki Modify Start
    //  $(".loanFinanceComListBox").fingerScroll();
    $(".loanFinanceComListBox").VScroll();
    // $99 Ken-Suzuki Modify End
});

//融資会社を選択した時のイベント
$(function () {
    $(".loanFinanceComlist").click(function (e) {
        $(".loanFinanceComdiv").html($(this).html());
        $(".loanFinanceComlist").removeClass("Selection");
        $(this).addClass("Selection");
        this_form.SelectFinanceComHiddenField.value = $(this).children("span").attr("value");
        $("#bodyFrame").trigger("click.popover");
        //入力値変更フラグ設定
        inputChangedClient();
    });
});
//テキストエリアフォーカス取得時には納車予定日を入力不可にする
$(function () {
    //テキストエリアフォーカス取得時
    $("input,textarea").live("focusin.icropScript", function (e) {
	    if ($(e.target).is("#deliDateDateTimeSelector") === false) {
	        $("#deliDateDateTimeSelector").DateTimeSelector("disabled", true);
		}
    });
    //テキストエリアフォーカスアウト時
    $("input,textarea").live("focusout.icropScript", function (e) {
        $("#deliDateDateTimeSelector").DateTimeSelector("disabled", false);
    });

    //ポップアップクローズの監視
    $(document.body).bind("mousedown touchstart", function (e) {
    if ($(e.target).is(".popover *") === false) {
        $("#BtInsComPopClose").click();
        $("#bodyFrame").trigger("click.popover");
	}
});

});
/**************************************************************
* 関数
**************************************************************/

//小数以下２桁を0埋め
function formatNumber(num) {

    // $99 Ken-Suzuki Add Start
    if (num.toString() == "") {
        return num.toString();
    } else {
        return Math.ceil(num).toString() + '.00';
    }
    // $99 Ken-Suzuki Add End

    //var dataIn = num;
    var dataIn = num.toString();

    if (dataIn == "") {
        return dataIn.toString();
    }
    var dataPut;

    var pointLocation = dataIn.toString();
    pointLocation = pointLocation.indexOf(".");
    if (pointLocation == -1) {
        //整数の場合
        dataPut = dataIn + '.00';
        dataPut = dataPut.toString();

    } else {
        if (pointLocation == dataIn.length - 2) {
            //小数以下1桁の場合'0'を補足する
            dataPut = dataIn + '0';
            dataPut = dataPut.toString();

        } else if (pointLocation == dataIn.length - 1) {
            //小数以下0桁の場合'00'を補足する
            dataPut = dataIn + '00';
            dataPut = dataPut.toString();

        } else {
            //小数以下2桁の場合
            dataPut = dataIn.toString();
        }
    }
    return dataPut;
}

//販売店オプション欄（追加、削除）
function setTextOption(element) {

    var textVal = element.val();

    if (element.hasClass("TableTextArea1") == true) {
        var trTargetSeq = element.parent().parent().parent().parent().parent().find("tr").index(element.parent().parent().parent().parent()[0]);
        var tdTargetSeq = element.parent().parent().parent().parent().find("td").index(element.parent().parent().parent()[0]);
    } else {
        var trTargetSeq = element.parent().parent().parent().parent().find("tr").index(element.parent().parent().parent()[0]);
        var tdTargetSeq = element.parent().parent().parent().find("td").index(element.parent().parent()[0]);
    }

    var tbl = document.getElementById("tblDlrOption");
    var row = tbl.rows.length;
    var tdTempvalue1;
    var tdTempvalue2;

    if (textVal.length != 0 && trTargetSeq == (row - 2)) {
        var insertTarget = row - 1;
        var insertRow = tbl.insertRow(insertTarget);

        var rowCell1 = $(insertRow.insertCell(0));
        var rowCell2 = $(insertRow.insertCell(1));
        var rowCell3 = $(insertRow.insertCell(2));
        var rowCell4 = $(insertRow.insertCell(3));

        rowCell1.attr("class", "TableText3");
        rowCell2.attr("class", "TableText3");
        rowCell3.attr("class", "TableText3");
        rowCell4.attr("class", "TableText2");
        rowCell4.attr("style", "width:97px;");
        var HTML1 = $('<div><input class="TableTextArea1" name="optionNameText' + row + '" type = "text" style = "width:171px;color:#666d74;background-color:#FFF;" onchange="inputChangedClient();" tabindex="23" /></div>');
        var HTML2 = $('<div><input class="TableTextArea2" name="optionPriceText' + row + '" type="text" style = "width:85px;color:#666d74;background-color:#FFF;" ReadOnly="True" /></div>');
        var HTML3 = $('<div><input class="TableTextArea3" name="optionMoneyText' + row + '" type="text" style = "width:85px;color:#666d74;background-color:#FFF;" ReadOnly="True" /></div>');
        var HTML4 = $('<label class = "TableOptionSum"/>');

        var inputTarget = HTML1.children("input");
        inputTarget
		   .CustomTextBox({
		       clear: function () {
		           inputChangedClient();
		       }
		   })
		   .observeValue(function () {
		       setTextOption($(this));
		   });

		   HTML2.children("input")
	           .observeValue(function () {
	               setTextOption($(this));
	           })
	       //キーボートを追加する
           .NumericKeypad({
		       acceptDecimalPoint: true,
		       defaultValue: 0,
		       completionLabel: $("#numericKeyPadDoneHiddenField").val(),
		       cancelLabel: $("#numericKeyPadCancelHiddenField").val(),
		       valueChanged: function (num) {
                   if (num.match(/^[0-9]{1,9}(\.[0-9]{1,2})?$/) || (num == "")) {
		               var numberFormat = num;
		               var numberFormatReturn = formatNumber(numberFormat);
		               $(this).val(numberFormatReturn);
                       //販売店オプション合計計算
		               totalDlrOptionSum($(this));
		               //支払い総額計算
		               totalPrice();
		               //入力値変更フラグ設定
		               inputChangedClient();
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
		               setTextOption($(this));
		               return true;
		           } else if (strValue == "1") {
		               $.data(this, "strFlg", "");
		               alert(this_form.optionPriceMsgHiddenField.value);
		               return false;
		           } else {
		               setTextOption($(this));
		           }
		       }

		   });

		   HTML3.children("input")
		   .observeValue(function () {
		       setTextOption($(this));
		   })
		   //キーボートを追加する
           .NumericKeypad({
		       acceptDecimalPoint: true,
		       defaultValue: 0,
		       completionLabel: $("#numericKeyPadDoneHiddenField").val(),
		       cancelLabel: $("#numericKeyPadCancelHiddenField").val(),
		       valueChanged: function (num) {
                    if (num.match(/^[0-9]{1,9}(\.[0-9]{1,2})?$/) || (num == "")) {
		               var numberFormat = num;
		               var numberFormatReturn = formatNumber(numberFormat);
		               $(this).val(numberFormatReturn);
                       //販売店オプション合計計算
		               totalDlrOptionSum($(this));
		               //支払い総額計算
		               totalPrice();
		               //入力値変更フラグ設定
		               inputChangedClient();
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
		               setTextOption($(this));
		               return true;
		           } else if (strValue == "1") {
		               $.data(this, "strFlg", "");
		               alert(this_form.optionInstallFeeMsgHiddenField.value);
		               return false;
		           } else {
		               setTextOption($(this));
		           }
		       }

		   });

		HTML4.CustomLabel({ useEllipsis: true });

        rowCell1.append(HTML1);
        rowCell2.append(HTML2);
        rowCell3.append(HTML3);
        rowCell4.append(HTML4);

        //変更後行数設定
        $("#dlrOptionCountHiddenField").val(row - 1);
    }
    if (textVal.length == 0 && trTargetSeq != (row - 2)) {
        if (tdTargetSeq == 0) {
            tdTempvalue1 = tbl.rows[trTargetSeq].cells[tdTargetSeq + 1].getElementsByTagName("input")[0].value;
            tdTempvalue2 = tbl.rows[trTargetSeq].cells[tdTargetSeq + 2].getElementsByTagName("input")[0].value;
        } else if (tdTargetSeq == 1) {
            tdTempvalue1 = tbl.rows[trTargetSeq].cells[tdTargetSeq - 1].getElementsByTagName("input")[0].value;
            tdTempvalue2 = tbl.rows[trTargetSeq].cells[tdTargetSeq + 1].getElementsByTagName("input")[0].value;
        } else if (tdTargetSeq == 2) {
            tdTempvalue1 = tbl.rows[trTargetSeq].cells[tdTargetSeq - 2].getElementsByTagName("input")[0].value;
            tdTempvalue2 = tbl.rows[trTargetSeq].cells[tdTargetSeq - 1].getElementsByTagName("input")[0].value;
        }

        if (tdTempvalue1 == "" && tdTempvalue2 == "") {

            if (element.hasClass("TableTextArea1") == true) {
                var trDelete = element.parent().parent().parent().parent();
            } else {
                var trDelete = element.parent().parent().parent();
            }
            //name再設定
            var trTempstart = trTargetSeq + 1;
            for (i = trTempstart; i < row - 1; i++) {
                var nextTrtd1 = $(tbl.rows[i].cells[0].getElementsByTagName("input")[0]);
                var nextTrtd2 = $(tbl.rows[i].cells[1].getElementsByTagName("input")[0]);
                var nextTrtd3 = $(tbl.rows[i].cells[2].getElementsByTagName("input")[0]);
                nextTrtd1.attr("name", "optionNameText" + i);
                nextTrtd2.attr("name", "optionPriceText" + i);
                nextTrtd3.attr("name", "optionMoneyText" + i);
            }

            //変更後行数設定
            $("#dlrOptionCountHiddenField").val(row - 3);

            trDelete.remove();
            //販売店オプション合計計算
            totalOption();
		    //支払い総額計算
		    totalPrice();
        }
    }

}


//オプション合計額計算
function totalOption() {

    var i = 0;

    var tblDlrOption = document.getElementById("tblDlrOption");
    var tblElseOption = document.getElementById("tblOption");
    var dlrRows = tblDlrOption.rows.length;
    var elseOptionRows = tblElseOption.rows.length;
    var totalDlrOption = 0.0;
    var totalElseOption = 0.0;

    //販売店オプション合計計算
    for (i = 0; i < dlrRows - 1; i++) {
        var dlrOptionValue = tblDlrOption.rows[i].cells[3].innerText;
        if (dlrOptionValue == "") {
            dlrOptionValue = 0.0;
        }
        totalDlrOption = Math.round((parseFloat(dlrOptionValue) + parseFloat(totalDlrOption)) * 100) / 100;
    }
    //内装、外装オプション、メーカーオプション合計計算
    for (i = 1; i < elseOptionRows; i++) {
        var elseOptionValue = tblElseOption.rows[i].cells[3].innerText;
        if (elseOptionValue == "") {
            elseOptionValue = 0.0;
        }
        totalElseOption = Math.round((parseFloat(elseOptionValue) + parseFloat(totalElseOption)) * 100) / 100;
    }


    var fltDispVal = Math.round((totalDlrOption + totalElseOption) * 100) / 100;

    tblDlrOption.rows[dlrRows - 1].cells[1].innerText = formatNumber(fltDispVal);

}

//オプション合計額計算（１行）
function totalDlrOptionSum(elemnt) {

    var tbl = document.getElementById("tblDlrOption");
    var rows = tbl.rows.length;
    var cellValue = 0.0;

    if (elemnt.val() != "") {
        cellValue = parseFloat(elemnt.val());
    }
    var trTargetSeq = elemnt.parent().parent().parent().parent().find("tr").index(elemnt.parent().parent().parent()[0]);
    var tdTargetSeq = elemnt.parent().parent().parent().find("td").index(elemnt.parent().parent()[0]);

    var tempCellValue;
    var goukeiSum = 0.0;

    if (tdTargetSeq == 1) {
        if (tbl.rows[trTargetSeq].cells[tdTargetSeq + 1].getElementsByTagName("input")[0].value == "") {
            tempCellValue = 0.0;
        } else {
            tempCellValue = parseFloat(tbl.rows[trTargetSeq].cells[tdTargetSeq + 1].getElementsByTagName("input")[0].value);
        }
    }

    if (tdTargetSeq == 2) {
        if (tbl.rows[trTargetSeq].cells[tdTargetSeq - 1].getElementsByTagName("input")[0].value == "") {
            tempCellValue = 0.0;
        } else {
            tempCellValue = parseFloat(tbl.rows[trTargetSeq].cells[tdTargetSeq - 1].getElementsByTagName("input")[0].value);
        }
    }

    goukeiSum = Math.round((cellValue + tempCellValue) * 100) / 100

    //オプション合計額（１行）表示
    tbl.rows[trTargetSeq].cells[3].innerText = formatNumber(goukeiSum);

    //オプション合計額計算
    totalOption();

}


//下取り車両欄（追加・削除）
function tradeInCarSet(element) {

    var i = 0;
    var textVal = element.val();

    if (element.hasClass("TradeInCarTextArea1") == true) {
        var trTargetSeq = element.parent().parent().parent().parent().parent().find("tr").index(element.parent().parent().parent().parent()[0]);
        var tdTargetSeq = element.parent().parent().parent().parent().find("td").index(element.parent().parent().parent()[0]);
    } else {
        var trTargetSeq = element.parent().parent().parent().parent().find("tr").index(element.parent().parent().parent()[0]);
        var tdTargetSeq = element.parent().parent().parent().find("td").index(element.parent().parent()[0]);
    }
    var tbl = document.getElementById("tblTradeInCar");
    var row = tbl.rows.length;

    var tdTempvalue;
    if (tdTargetSeq == 0) {
        tdTempvalue = tbl.rows[trTargetSeq].cells[tdTargetSeq + 1].getElementsByTagName("input")[0].value;

    } else if (tdTargetSeq == 1) {
        tdTempvalue = tbl.rows[trTargetSeq].cells[tdTargetSeq - 1].getElementsByTagName("input")[0].value;
    }

    if (textVal.length != 0 && trTargetSeq == (row - 2)) {
        var insertTarget = row - 1;
        var insertRow = tbl.insertRow(insertTarget);

        var rowCell1 = $(insertRow.insertCell(0));
        var rowCell2 = $(insertRow.insertCell(1));

        rowCell1.attr("class", "TableText1");
        rowCell2.attr("class", "TableText2");

        var minus = this_form.minusLabelHiddenField.value;
        var HTML1 = $('<div><input class="TradeInCarTextArea1" name="tradeInCarText' + insertTarget + '" type = "text" style = "width:327px;color:#666d74;background-color:#FFF;" onchange="inputChangedClient();" tabindex="39" /></div>');
        var HTML2 = $('<div><p class= "TradeInCarLabel">' + minus + '<p/><input class="TradeInCarTextArea2" name="tradeInCarPrice' + insertTarget + '" type="text" style = "width:80px;color:#666d74;background-color:#FFF;" ReadOnly="True" /></div>');

        HTML1.children("input")
            .CustomTextBox({
                clear: function () {
                    inputChangedClient();
                }
            })
            .observeValue(function () {
                tradeInCarSet($(this));
            });
            HTML2.children("input")
           .observeValue(function () {
               tradeInCarSet($(this));
           })
           //キーボートを追加する
        　　.NumericKeypad({
            acceptDecimalPoint: true,
            defaultValue: 0,
            completionLabel: $("#numericKeyPadDoneHiddenField").val(),
            cancelLabel: $("#numericKeyPadCancelHiddenField").val(),
            valueChanged: function (num) {
                if (num.match(/^[0-9]{1,9}(\.[0-9]{1,2})?$/) || (num == "")) {
                    var numberFormat = num;
                    var numberFormatReturn = formatNumber(numberFormat);
                    $(this).val(numberFormatReturn);
                    //下取り額合計計算
                    tradeInCarSum();
                    //支払い総額計算
                    totalPrice();
                    //入力値変更フラグ設定
                    inputChangedClient();
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
                //監視関数とセルのフォーマート設定に合わせるため、下記処理が必要
                var strValue = $.data(this, "strFlg");
                if (strValue == "0") {
                    $.data(this, "strFlg", "");

                    tradeInCarSet($(this));
                    return true;
                } else if (strValue == "1") {
                    $.data(this, "strFlg", "");
                    alert(this_form.tradeInPriceMsgHiddenField.value);
                    return false;
                } else {

                    tradeInCarSet($(this));
                }
            }
           
           });
        rowCell1.append(HTML1);
        rowCell2.append(HTML2);

        //変更後行数設定
        $("#tradeInCarCountHiddenField").val(insertTarget - 1);

    }

    if (textVal.length == 0 && trTargetSeq != (row - 2) && tdTempvalue == 0) {

        if (element.hasClass("TradeInCarTextArea1") == true) {
            var trDelete = element.parent().parent().parent().parent();
        } else {
            var trDelete = element.parent().parent().parent();
        }
        //name再設定
        var trTempstart = trTargetSeq;

        for (i = trTempstart; i < row - 2; i++) {
            var nextTrtd1 = $(tbl.rows[i + 1].cells[0].getElementsByTagName("input")[0]);
            var nextTrtd2 = $(tbl.rows[i + 1].cells[1].getElementsByTagName("input")[0]);
            nextTrtd1.attr("name", "tradeInCarText" + i);
            nextTrtd2.attr("name", "tradeInCarPrice" + i);

        }
        //変更後行数設定
        $("#tradeInCarCountHiddenField").val(row - 4);

        trDelete.remove();
        tradeInCarSum();

    }

    //支払い総額
    totalPrice();
}

//下取り合計額計算
function tradeInCarSum() {

    var i = 0;

    var tradeInCarValue = 0.0;
    var tradeInCartotal = 0.0;

    var tblTradeInCar = document.getElementById("tblTradeInCar");
    var tradeInCarRows = tblTradeInCar.rows.length;


    for (i = 1; i < tradeInCarRows - 1; i++) {
        tradeInCarValue = 0.0;

        if (tblTradeInCar.rows[i].cells[1].getElementsByTagName("input")[0].value != "") {
            tradeInCarValue = parseFloat(tblTradeInCar.rows[i].cells[1].getElementsByTagName("input")[0].value);
        }
        tradeInCartotal = Math.round((tradeInCartotal + tradeInCarValue) * 100) / 100;
    }

    $(tblTradeInCar.rows[tradeInCarRows - 1].cells[1]).find("label").text(formatNumber(tradeInCartotal));
}


//参照モード時　販売店オプションテーブルの処理
function setDlrOptionDesible() {

    var tblDlr = document.getElementById("tblDlrOption");
    var rowDlr = tblDlr.rows.length;
    for (i = 0; i < rowDlr - 2; i++) {
        var tdOptionName = $(tblDlr.rows[i].cells[0]);
        var tdOptionPrice = $(tblDlr.rows[i].cells[1]);
        var tdOptionMoney = $(tblDlr.rows[i].cells[2]);

        var tdOptionNameValue = tblDlr.rows[i].cells[0].getElementsByTagName("input")[0].value;
        var tdOptionPriceValue = tblDlr.rows[i].cells[1].getElementsByTagName("input")[0].value;
        var tdOptionMoneyValue = tblDlr.rows[i].cells[2].getElementsByTagName("input")[0].value;

        var tdOptionNameText = $('<lable style="display:inline-block; width:171px;">' + tdOptionNameValue + '</lable>');
        var tdOptionPriceText = $('<lable style="display:inline-block; width:85px;">' + tdOptionPriceValue + '</lable>');
        var tdOptionMoneyText = $('<lable style="display:inline-block; width:85px;">' + tdOptionMoneyValue + '</lable>');

        tdOptionName.find("div").css({ "display": "none" });
        tdOptionPrice.find("div").css({ "display": "none" });
        tdOptionMoney.find("div").css({ "display": "none" });
        tdOptionNameText.CustomLabel({ useEllipsis: true }).appendTo(tdOptionName);
        tdOptionPriceText.CustomLabel({ useEllipsis: true }).appendTo(tdOptionPrice);
        tdOptionMoneyText.CustomLabel({ useEllipsis: true }).appendTo(tdOptionMoney);

        tdOptionName.attr("class", "TableText1");
        tdOptionPrice.attr("class", "TableText4");
        tdOptionMoney.attr("class", "TableText4");
    }
    $(tblDlr.rows[rowDlr - 2]).css({ "display": "none" });
}


//参照モード時　下取り車両テーブルの処理
function setTradeInCarDesible() {
    var minus = this_form.minusLabelHiddenField.value;
    var tblCar = document.getElementById("tblTradeInCar");
    var rowCar = tblCar.rows.length;
    for (i = 1; i < rowCar - 2; i++) {
        var tdCarName = $(tblCar.rows[i].cells[0]);
        var tdCarPrice = $(tblCar.rows[i].cells[1]);

        var tdCarNameValue = tblCar.rows[i].cells[0].getElementsByTagName("input")[0].value;
        var tdCarPriceValue = tblCar.rows[i].cells[1].getElementsByTagName("input")[0].value;

        var tdCarNameText = $('<label style="display:inline-block; width:327px;">' + tdCarNameValue + '</lable>');

        if (tdCarPriceValue == "") {
            var tdCarPriceText = $('<label style="display:inline-block; width:80px;">' + tdCarPriceValue + '</lable>');
        } else {
            var tdCarPriceText = $('<label style="display:inline-block; width:80px;">' + minus + tdCarPriceValue + '</lable>');
        }


        tdCarName.find("div").css({ "display": "none" });
        tdCarPrice.find("div").css({ "display": "none" });
        tdCarNameText.CustomLabel({ useEllipsis: true }).appendTo(tdCarName);
        tdCarPriceText.CustomLabel({ useEllipsis: true }).appendTo(tdCarPrice);

        tdCarName.attr("class", "TableText1");
        tdCarPrice.attr("class", "TableText2");
    }
    $(tblCar.rows[rowCar - 2]).css({ "display": "none" });

}


//所有者情報を使用者情報へコピー
function customerInfoCopy() {

    //□氏名
    this_form.shiyosyaNameTextBox.value = this_form.shoyusyaNameTextBox.value;
    //□住所
    this_form.shiyosyaZipCodeTextBox.value = this_form.shoyusyaZipCodeTextBox.value;
    this_form.shiyosyaAddressTextBox.value = this_form.shoyusyaAddressTextBox.value;
    //□連絡先
    this_form.shiyosyaMobileTextBox.value = this_form.shoyusyaMobileTextBox.value;
    this_form.shiyosyaTelTextBox.value = this_form.shoyusyaTelTextBox.value;
    //□E-Mail
    this_form.shiyosyaEmailTextBox.value = this_form.shoyusyaEmailTextBox.value;
    //□国民ID
    this_form.shiyosyaIDTextBox.value = this_form.shoyusyaIDTextBox.value;


    $("#shiyosyaNameTextBox").CustomTextBox("updateText", this_form.shiyosyaNameTextBox.value);
    $("#shiyosyaZipCodeTextBox").CustomTextBox("updateText", this_form.shiyosyaZipCodeTextBox.value);
    $("#shiyosyaAddressTextBox").CustomTextBox("updateText", this_form.shiyosyaAddressTextBox.value);
    $("#shiyosyaMobileTextBox").CustomTextBox("updateText", this_form.shiyosyaMobileTextBox.value);
    $("#shiyosyaTelTextBox").CustomTextBox("updateText", this_form.shiyosyaTelTextBox.value);
    $("#shiyosyaEmailTextBox").CustomTextBox("updateText", this_form.shiyosyaEmailTextBox.value);
    $("#shiyosyaIDTextBox").CustomTextBox("updateText", this_form.shiyosyaIDTextBox.value);


    //□顧客区分
    var strLabelKojin = document.getElementById("CustomLabelshiyosyaKojin");
	var strLabelHojin = document.getElementById("CustomLabelshiyosyaHojin");
	var imgKojinCheck = document.getElementById("imgChkShiyosyaKojin");
	var imgHojinCheck = document.getElementById("imgChkShiyosyaHojin");
	if (this_form.shoyusyaHojinCheckMark.value == "TRUE") {

	    strLabelHojin.style.color = "#324F85";
	    imgHojinCheck.style.display = "block";
	    strLabelKojin.style.color = "#A6A6A6";
	    imgKojinCheck.style.display = "none";

	    this_form.shiyosyaHojinCheckMark.value = "TRUE"
	    this_form.shiyosyaKojinCheckMark.value = "FALSE"

	    //敬称非表示
	    $('#shiyosyaKeisyoMaeLabel').hide();
	    $('#shiyosyaKeisyoAtoLabel').hide();
	}
	else {
	    strLabelHojin.style.color = "#A6A6A6";
	    imgHojinCheck.style.display = "none";
	    strLabelKojin.style.color = "#324F85";
	    imgKojinCheck.style.display = "block";

	    this_form.shiyosyaHojinCheckMark.value = "FALSE"
	    this_form.shiyosyaKojinCheckMark.value = "TRUE"

	    //敬称表示
	    $('#shiyosyaKeisyoMaeLabel').show(0);
	    $('#shiyosyaKeisyoAtoLabel').show(0);
	}

	//入力値変更フラグ設定
	inputChangedClient();
}

//所有者、使用者表示切替え
function custChange() {

    var strCstKind;
    var strSyoyusya;
    var strShiyosya;

    strCstKind = document.getElementById("custClassSegmentedButton_0").checked;
    strSyoyusya = document.getElementById("syoyusya");
    strShiyosya = document.getElementById("shiyosya");

    //表示切替え
    if (strCstKind == true) {
        strSyoyusya.style.display = "block";
        strShiyosya.style.display = "none";

    } else {

        strSyoyusya.style.display = "none";
        strShiyosya.style.display = "block";

    }

}

//現金、ローン表示切替え
function payMethodChange() {

    //契約完了後でない場合
    if ($("#contractAfterFlgHiddenField").val() != "1") {
        var strPayMethod;
        var strCash;
        var strLoan;

        strPayMethod = document.getElementById("payMethodSegmentedButton_0").checked;
        strCash = document.getElementById("cash");
        strLoan = document.getElementById("loan");

        //表示切替え
        if (strPayMethod == true) {
            strCash.style.display = "block";
            strLoan.style.display = "none";

        } else {

            strCash.style.display = "none";
            strLoan.style.display = "block";

            //表示設定
            //月額
            //頭金
            //ボーナス
            var tdLoanMonthlyPay;
            var tdLoanDeposit;
            var tdLoanBonusPay;

            tdLoanMonthlyPay = $("#loanMonthlyValueHiddenField").val();
            tdLoanDeposit = $("#loanDepositValueHiddenField").val();
            tdLoanBonusPay = $("#loanBonusValueHiddenField").val();


            if ((this_form.ReferenceModeHiddenField.value).toUpperCase() == "TRUE") {

                $("#loanMonthlyPayLabel").val(formatNumber(tdLoanMonthlyPay));
                $("#loanDepositLabel").val(formatNumber(tdLoanDeposit));
                $("#loanBonusPayLabel").val(formatNumber(tdLoanBonusPay));
            } else {

                $("#loanMonthlyPayTextBox").val(formatNumber(tdLoanMonthlyPay));
                $("#loanDepositTextBox").val(formatNumber(tdLoanDeposit));
                $("#loanBonusPayTextBox").val(formatNumber(tdLoanBonusPay));
                
            }

        }
    }

}


//諸費用合計額計算
function chargeTotal() {

    //諸費用合計額
    var fltChargeTotal;
    var fltCarBuyTax;
    var fltRegCost;
    fltChargeTotal = 0.0;
    fltCarBuyTax = 0.0;
    fltRegCost = 0.0;

    //車両購入税
    // $99 Ken-Suzuki Modify Start
//    if (document.getElementById("CarBuyTaxCustomLabel").innerHTML == "") {
//        fltCarBuyTax = 0.0;
//    } else {
//        fltCarBuyTax = parseFloat(document.getElementById("CarBuyTaxCustomLabel").innerHTML);
//    }
    if (this_form.carBuyTaxHiddenField.value != "") {
        fltCarBuyTax = parseFloat(this_form.carBuyTaxHiddenField.value);
    } else {
        fltCarBuyTax = 0.0;
    }
    // $99 Ken-Suzuki Modify End
    //登録費用

    if (this_form.regCostValueHiddenField.value != "") {
        fltRegCost = parseFloat(this_form.regCostValueHiddenField.value);
    } else {
        fltRegCost = 0.0;
    }


    fltChargeTotal = Math.round((fltCarBuyTax + fltRegCost) * 100) / 100;
    //表示
    document.getElementById("chargeInfoTotalCustomLabel").innerHTML = formatNumber(fltChargeTotal);


}



//支払い総額計算
function totalPrice() {

    //オプション合計額取得用
    var tblOption = document.getElementById("tblDlrOption");
    var OptionRows = tblOption.rows.length;

    //下取り合計額取得用
    var tblTradeInCar = document.getElementById("tblTradeInCar");
    var CarRows = tblTradeInCar.rows.length;

    var lngPayTotal;
    lngPayTotal = 0.0;

    //車両価格
    // $99 Ken-Suzuki Modify Start
//  lngPayTotal = Math.round((lngPayTotal + parseFloat(document.getElementById("basePriceHiddenField").value)) * 100) / 100;
    var basePriceValue = parseFloat($("#basePriceHiddenField").val());
    var extPriceValue = parseFloat($("#extOptionPriceHiddenField").val());
    var intPriceValue = parseFloat($("#intOptionPriceHiddenField").val());
    lngPayTotal = Math.round((lngPayTotal + basePriceValue + extPriceValue + intPriceValue) * 100) / 100;
    // $99 Ken-Suzuki Modify End

    //オプション合計額
    lngPayTotal = Math.round((lngPayTotal + parseFloat(tblOption.rows[OptionRows - 1].cells[1].innerText)) * 100) / 100;

    //諸費用合計
    lngPayTotal = Math.round((lngPayTotal + parseFloat(document.getElementById("chargeInfoTotalCustomLabel").innerHTML)) * 100) / 100;

    //保険年額

    if (this_form.insuAmountValueHiddenField.value != "") {
        lngPayTotal = Math.round((lngPayTotal + parseFloat(this_form.insuAmountValueHiddenField.value)) * 100) / 100;
    }

    //下取り合計額（-）
    lngPayTotal = Math.round((lngPayTotal - parseFloat(tblTradeInCar.rows[CarRows - 1].cells[1].innerText)) * 100) / 100;

    //値引き額（-）
    if (this_form.discountPriceValueHiddenField.value != "") {
        lngPayTotal = Math.round((lngPayTotal - parseFloat(this_form.discountPriceValueHiddenField.value)) * 100) / 100;
    }

    //支払い総額金額表示
    document.getElementById("PayTotalLabel").innerHTML = formatNumber(lngPayTotal);

}

//必須入力チェック
function inputMandatryCheck() {

    //契約済みのときはチェックしない
    //DEBUG(定数でない)
    if (this_form.contractFlgHiddenField.value != "1") {
    
        //氏名（所有者）
        if (inputTrim(this_form.shoyusyaNameTextBox.value) == "") {
            alert(this_form.shoyusyaNameMsgHiddenField.value);
            return false;
        }
        //郵便番号（所有者）
        if (inputTrim(this_form.shoyusyaZipCodeTextBox.value) == "") {
            alert(this_form.shoyusyaZipcodeMsgHiddenField.value);
            return false;
        }
        //住所（所有者）
        if (inputTrim(this_form.shoyusyaAddressTextBox.value) == "") {
            alert(this_form.shoyusyaAddressMsgHiddenField.value);
            return false;
        }
        //国民ID（所有者）
        if (inputTrim(this_form.shoyusyaIDTextBox.value) == "") {
            alert(this_form.shoyusyaIdMsgHiddenField.value);
            return false;
        }
        //氏名（使用者）
        if (inputTrim(this_form.shiyosyaNameTextBox.value) == "") {
            alert(this_form.shiyosyaNameMsgHiddenField.value);
            return false;
        }
        //郵便番号（使用者）
        if (inputTrim(this_form.shiyosyaZipCodeTextBox.value) == "") {
            alert(this_form.shiyosyaZipcodeMsgHiddenField.value);
            return false;
        }
        //住所（使用者）
        if (inputTrim(this_form.shiyosyaAddressTextBox.value) == "") {
            alert(this_form.shiyosyaAddressMsgHiddenField.value);
            return false;
        }
        //国民ID（使用者）
        if (inputTrim(this_form.shiyosyaIDTextBox.value) == "") {
            alert(this_form.shiyosyaIdMsgHiddenField.value);
            return false;
        }
        //諸費用　登録費用
        if (inputTrim(this_form.regPriceTextBox.value) == "") {
            alert(this_form.regPriceHiddenField.value);
            return false;
        }
    }

    //オーバーレイ、ロード中表示

    dispLoading();

    //入力変更チェックを実施する(inputChangedClient以外のもの)
    inputChangeCheck();

    //実行モード設定（1:契約）
    this_form.actionModeHiddenField.value = "1";

    //サーバ処理へ
    this_form.submit();

}
//文字列トリム
function inputTrim(strVal) {
    var strWk;

    strWk = strVal.replace(/^[\s]+/g, "");
    strWk = strWk.replace(/[\s]+$/g, "");
    return strWk;
}

//見積書印刷ボタン押下（クライアント）
function estPreviewClientClick() {
    inputChangeCheck();
    dispLoading();
}

//入力変更フラグを立てる（クライアント側）
function inputChangedClient() {
    $("#blnInputChangedClientHiddenField").val("TRUE");

}

//入力変更チェックを実施する(inputChangedClient以外のもの)
function inputChangeCheck() {

    //期間（ローン）
    if ((this_form.ReferenceModeHiddenField.value).toUpperCase() == "TRUE") {
        if (this_form.periodInitialValueHiddenField.value != loanPayPeriodLabel.innerHTML) {

            //入力値変更フラグ設定
            inputChangedClient();
        }
    } else {
        if (this_form.periodInitialValueHiddenField.value != loanPayPeriodNumericBox.innerHTML) {

            //入力値変更フラグ設定
            inputChangedClient();
        }
    }
    //初回支払い（日）
    if ((this_form.ReferenceModeHiddenField.value).toUpperCase() == "TRUE") {
        if (this_form.firstPayInitialValueHiddenField.value != loanDueDateLabel.innerHTML) {
            //入力値変更フラグ設定
            inputChangedClient();
        }
    } else {
        if (this_form.firstPayInitialValueHiddenField.value != loanDueDateNumericBox.innerHTML) {
            //入力値変更フラグ設定
            inputChangedClient();
        }
    }
    //納車予定日
    if ((this_form.ReferenceModeHiddenField.value).toUpperCase() == "TRUE") {


        if (this_form.deliDateInitialValueHiddenField.value != this_form.deliDateAfterValueHiddenField.value) {
            //入力値変更フラグ設定
            inputChangedClient();
        }

    } else {
        if (this_form.deliDateInitialValueHiddenField.value != deliDateDateTimeSelector.value) {
            //入力値変更フラグ設定
            inputChangedClient();
        }
    }
}


////商談情報破棄メッセージ
function cancellationCheck () {

    if ((this_form.blnNewActFlagHiddenField.value).toUpperCase() == "TRUE") {
        return confirm(this_form.customerDeleteMsgHiddenField.value);

    }
    return true;

}

////入力内容破棄メッセージ
function inputUpdateCheck() {


    //入力変更チェックを実施する(inputChangedClient以外のもの)
    inputChangeCheck();

    if ((this_form.blnInputChangedClientHiddenField.value).toUpperCase() == "TRUE") {
        //2012/02/03 TCS 葛西 【SALES_1A】号口(課題No.47)対応 MOD START
        //return confirm(this_form.inputDataDeleteMsgHiddenField.value);

        if (confirm(this_form.inputDataDeleteMsgHiddenField.value)) {

            dispClearLoading();
            return true;
        } else {
            return false;
        }
        //2012/02/03 TCS 葛西 【SALES_1A】号口(課題No.47)対応 MOD END

    }

    //2012/02/03 TCS 葛西 【SALES_1A】号口(課題No.47)対応 ADD START
    dispClearLoading();
    //2012/02/03 TCS 葛西 【SALES_1A】号口(課題No.47)対応 ADD END

    return true;

}

////入力内容、商談情報破棄メッセージ
function deleteCheck() {

    //入力変更チェックを実施する(inputChangedClient以外のもの)
    inputChangeCheck();

    //商談情報破棄メッセージ
    if ((this_form.blnNewActFlagHiddenField.value).toUpperCase() == "TRUE") {
        //2012/02/03 TCS 葛西 【SALES_1A】号口(課題No.47)対応 MOD START
        //return confirm(this_form.customerDeleteMsgHiddenField.value);
        if (confirm(this_form.customerDeleteMsgHiddenField.value)) {

            dispClearLoading();
            return true;
        } else {
            return false;
        }
        //2012/02/03 TCS 葛西 【SALES_1A】号口(課題No.47)対応 MOD END

    } else if ((this_form.blnInputChangedClientHiddenField.value).toUpperCase() == "TRUE") {

    //2012/02/03 TCS 葛西 【SALES_1A】号口(課題No.47)対応 MOD START
        //return confirm(this_form.inputDataDeleteMsgHiddenField.value);
        if (confirm(this_form.customerDeleteMsgHiddenField.value)) {

            dispClearLoading();
            return true;
        } else {
            return false;
        }
    //2012/02/03 TCS 葛西 【SALES_1A】号口(課題No.47)対応 MOD END
    }

    //2012/02/03 TCS 葛西 【SALES_1A】号口(課題No.47)対応 ADD START
    dispClearLoading();
    //2012/02/03 TCS 葛西 【SALES_1A】号口(課題No.47)対応 ADD END

    return true;
    
    
}


//所有者個人を選択した時のイベント
function onClickShoyushaKojin() {
        var strLabelKojin = document.getElementById("CustomLabelShoyusyaKojin");
        var strLabelHojin = document.getElementById("CustomLabelShoyusyaHojin");
        var imgKojinCheck = document.getElementById("imgChkKojin");
        var imgHojinCheck = document.getElementById("imgChkHojin");

        if (this_form.shoyusyaKojinCheckMark.value != "TRUE") {
            strLabelKojin.style.color = "#324F85";
            imgKojinCheck.style.display = "block";
            strLabelHojin.style.color = "#A6A6A6";
            imgHojinCheck.style.display = "none";

            this_form.shoyusyaKojinCheckMark.value = "TRUE"
            this_form.shoyusyaHojinCheckMark.value = "FALSE"

            //敬称表示
            $("#shoyusyaKeisyoMaeLabel").show(0);
            $("#shoyusyaKeisyoAtoLabel").show(0);


            //入力値変更フラグ設定
            inputChangedClient();

        }
}

//所有者法人を選択した時のイベント
function onClickShoyushaHojin() {
        var strLabelKojin = document.getElementById("CustomLabelShoyusyaKojin");
        var strLabelHojin = document.getElementById("CustomLabelShoyusyaHojin");
        var imgKojinCheck = document.getElementById("imgChkKojin");
        var imgHojinCheck = document.getElementById("imgChkHojin");

        if (this_form.shoyusyaHojinCheckMark.value != "TRUE") {
            strLabelHojin.style.color = "#324F85";
            imgHojinCheck.style.display = "block";
            strLabelKojin.style.color = "#A6A6A6";
            imgKojinCheck.style.display = "none";

            this_form.shoyusyaHojinCheckMark.value = "TRUE"

            this_form.shoyusyaKojinCheckMark.value = "FALSE"

            //敬称非表示
            $('#shoyusyaKeisyoMaeLabel').hide();
            $('#shoyusyaKeisyoAtoLabel').hide();

            //入力値変更フラグ設定
            inputChangedClient();
        }
}

//使用者個人を選択した時のイベント
function onClickShiyosyaKojin(){
        var strLabelKojin = document.getElementById("CustomLabelshiyosyaKojin");
        var strLabelHojin = document.getElementById("CustomLabelshiyosyaHojin");
        var imgKojinCheck = document.getElementById("imgChkShiyosyaKojin");
        var imgHojinCheck = document.getElementById("imgChkShiyosyaHojin");

        if (this_form.shiyosyaKojinCheckMark.value != "TRUE") {
            strLabelKojin.style.color = "#324F85";
            imgKojinCheck.style.display = "block";
            strLabelHojin.style.color = "#A6A6A6";
            imgHojinCheck.style.display = "none";

            this_form.shiyosyaKojinCheckMark.value = "TRUE"

            this_form.shiyosyaHojinCheckMark.value = "FALSE"

            //敬称表示
            $('#shiyosyaKeisyoMaeLabel').show(0);
            $('#shiyosyaKeisyoAtoLabel').show(0);

            //入力値変更フラグ設定
            inputChangedClient();

        }
}

//使用者法人を選択した時のイベント
function onClickShiyosyaHojin(){
        var strLabelKojin = document.getElementById("CustomLabelshiyosyaKojin");
        var strLabelHojin = document.getElementById("CustomLabelshiyosyaHojin");
        var imgKojinCheck = document.getElementById("imgChkShiyosyaKojin");
        var imgHojinCheck = document.getElementById("imgChkShiyosyaHojin");

        if (this_form.shiyosyaHojinCheckMark.value != "TRUE") {
            strLabelHojin.style.color = "#324F85";
            imgHojinCheck.style.display = "block";
            strLabelKojin.style.color = "#A6A6A6";
            imgKojinCheck.style.display = "none";

            this_form.shiyosyaHojinCheckMark.value = "TRUE"
            this_form.shiyosyaKojinCheckMark.value = "FALSE"
            //敬称非表示
            $('#shiyosyaKeisyoMaeLabel').hide();
            $('#shiyosyaKeisyoAtoLabel').hide();

            //入力値変更フラグ設定
            inputChangedClient();

        }
}

//自社を選択した時のイベント
function onClickJisya(){
    var strLabelJisya = document.getElementById("CustomLabelJisya");
    var strLabelJisyaSelected = document.getElementById("CustomLabelJisyaSelected");
    var strLabelTasya = document.getElementById("CustomLabelTasya");
    var imgJisyaCheck = document.getElementById("imgChkJisya");
    var imgTasyaCheck = document.getElementById("imgChkTasya");

    if (this_form.jisyaCheckMark.value != "TRUE") {
        strLabelJisya.style.color = "#324F85";
        imgJisyaCheck.style.display = "block";
        strLabelTasya.style.color = "#A6A6A6";
        imgTasyaCheck.style.display = "none";

        this_form.jisyaCheckMark.value = "TRUE"
        this_form.tasyaCheckMark.value = "FALSE"

        $("#dispSelectedInsCom").text("");
        $("#dispSelectedInsKind").text("");

        this_form.SelectInsuComCdHidden.value = "";
        this_form.SelectInsuKindCdHidden.value = "";

        //入力値変更フラグ設定
        inputChangedClient();

    }
}

//他社を選択した時のイベント
function onClickTasya(){
    var strLabelJisya = document.getElementById("CustomLabelJisya");
    var strLabelJisyaSelected = document.getElementById("CustomLabelJisyaSelected");
    var strLabelTasya = document.getElementById("CustomLabelTasya");
    var imgJisyaCheck = document.getElementById("imgChkJisya");
    var imgTasyaCheck = document.getElementById("imgChkTasya");

    if (this_form.tasyaCheckMark.value != "TRUE") {
        strLabelTasya.style.color = "#324F85";
        imgTasyaCheck.style.display = "block";
        strLabelJisya.style.color = "#A6A6A6";
        imgJisyaCheck.style.display = "none";

        this_form.tasyaCheckMark.value = "TRUE"
        this_form.jisyaCheckMark.value = "FALSE"

        //選択していた保険会社、種類を消す
        $("#dispSelectedInsCom").text("");
        $("#dispSelectedInsKind").text("");

        this_form.SelectInsuComCdHidden.value = "";
        this_form.SelectInsuKindCdHidden.value = "";

        //入力値変更フラグ設定
        inputChangedClient();
    }
}

//オーバーレイ、ロード中表示
function dispLoading() {

    //オーバーレイ表示
    $("#serverProcessOverlayBlack").css("display", "block");
    //アニメーション(ロード中)
    setTimeout(function () {
        $("#serverProcessIcon").addClass("show");
        $("#serverProcessOverlayBlack").addClass("open");
    }, 0);
}

//2012/02/03 TCS 葛西 【SALES_1A】号口(課題No.47)対応 ADD START
//透明オーバーレイ表示
function dispClearLoading() {
    //オーバーレイ表示
    $("#serverProcessOverlayBlack").css("display", "block");

}
//2012/02/03 TCS 葛西 【SALES_1A】号口(課題No.47)対応 ADD END

// $99 Ken-Suzuki Add Start
// 販売店、個人表示切替え
function chargeChange() {

    var priceZero = 0.00;

    if (document.getElementById("chargeSegmentedButton_0").checked) {
        // 「販売店」が選択された場合
        document.getElementById("CarBuyTaxCustomLabel").innerHTML = formatNumber($("#carBuyDefaultTaxHiddenField").val());
        $("#CarBuyTaxCustomLabel").val(formatNumber($("#carBuyDefaultTaxHiddenField").val()));
        $("#carBuyTaxHiddenField").val(formatNumber($("#carBuyDefaultTaxHiddenField").val()));
    } else {
        // 「個人」が選択された場合
        document.getElementById("CarBuyTaxCustomLabel").innerHTML = formatNumber(priceZero);
        $("#carBuyTaxHiddenField").val(formatNumber(priceZero));
        $("#regPriceTextBox").val(formatNumber(priceZero));
        $("#regCostValueHiddenField").val(formatNumber(priceZero));
    }

    // 諸費用合計計算
    chargeTotal();

    // 支払い総額計算
    totalPrice();

    // 入力値変更フラグ設定
    inputChangedClient();
}

// 車両購入税の再計算
function setCarBuyTax() {
    
    // 車両本体価格
    var basePriceValue = parseFloat($("#basePriceHiddenField").val());
    // 外装色追加費用
    var extPriceValue = parseFloat($("#extOptionPriceHiddenField").val());
    // 内装色追加費用
    var intPriceValue = parseFloat($("#intOptionPriceHiddenField").val());
    // 値引き額
    var discountPriceValue = 0.0;
    if ($("#discountPriceValueHiddenField").val() != "") {
        discountPriceValue = parseFloat($("#discountPriceValueHiddenField").val());
    }
    // 車両購入税率
    var estVclTaxRatio = parseFloat($("#estVclTaxRatioHiddenField").val());
    // 車両購入税最低額
    var carBuyTaxMast = parseFloat($("#carBuyTaxMastHiddenField").val());

    // 車両購入税（計算結果）
    var carBuyTax = (basePriceValue + extPriceValue + intPriceValue - discountPriceValue) * estVclTaxRatio;

    // 車両購入税（計算結果） ＜ 車両購入税（最低価格） の場合は車両購入税（最低価格）を採用
    if (carBuyTax < carBuyTaxMast) {
        carBuyTax = carBuyTaxMast;
    }

    // 小数点以下の切り捨て
    carBuyTax = Math.floor(carBuyTax);

    // 車両購入税（初期値）の設定
    $("#carBuyDefaultTaxHiddenField").val(formatNumber(carBuyTax));

    if (document.getElementById("chargeSegmentedButton_1").checked) {
        carBuyTax = 0.0;
    }

    // 車両購入税の再計算結果を格納
    document.getElementById("CarBuyTaxCustomLabel").innerHTML = formatNumber(carBuyTax);
    $("#CarBuyTaxCustomLabel").val(formatNumber(carBuyTax));
    $("#carBuyTaxHiddenField").val(formatNumber(carBuyTax));
    $("#CarBuyTaxCustomLabel").val(formatNumber(carBuyTax));

}
// $99 Ken-Suzuki Add End
