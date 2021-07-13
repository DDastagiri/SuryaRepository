/**************************************************************
* DOMロード時の処理
**************************************************************/
$(function () {

    /* 親フレームのスクロール制御を妨げないようにする為、以下のハンドラを除外する */
    $("body")
    .unbind("touchstart.icropScript")
    .unbind("touchmove.icropScript");

    $("#tcvNcv50Main").VScroll().css("overflow:scroll", "touch");

    //印刷用ポップアップ作成
    //sc3070204Script.create("#PrintButton");

    //見積作成フレーム内のURLを更新
    document.getElementById('EstimateInfo').src = this_form.EstimateInfoURL.value;

    //ポップアップクローズ処理
    $("#EstimateInfo").load(function () {
        $(this).height(frames["EstimateInfo"].document.body.scrollHeight);

        //frames["EstimateInfo"].document.addEventListener('click', function () { top.$("#bodyFrame").trigger("hideOpenPopover"); }, true);
        //$("#bodyFrame").bind("click.EstimateInfoPopover", function (event) {
        //	if (HasSC3070205()) {
        //    	frames["EstimateInfo"].$("#bodyFrame").trigger("hideOpenPopover");
        //    }
        //});
    });
});
/**************************************************************
* 自画面用メソッド
**************************************************************/
function CheckAndSaveEstimate(checkMode, callback) {
    dispLoading();
    if ($('#ReferenceModeHiddenField').val() == "False") {
        //編集モード

        //入力チェック
        if (frames["EstimateInfo"].checkEstimateInfo(checkMode) == false) {
            hideDispLoading();
            return false;
        }

        //見積保存
        var saveResult = frames["EstimateInfo"].saveEstimateInfo(callback);
        if (saveResult != undefined) {
            if (saveResult == -1) {
                hideDispLoading();
                return false;
            }
            callback();
        }
    } else {
        //参照モード
        callback();
    }

}

/* 契約承認依頼ボタン押下時 */
function ContractButtonClick() {
    var saveEstimateCallback = function (success) {
        //パラメータ設定
        sc3070208Script.setParams({
            Estimateid: $('#lngEstimateIdHiddenField').val()
      , Customerid: $('#strCRCustIdHiddenField').val()
      , CustomerName: $('#cstNameHiddenField').val()
      , CustomerClass: $('#strCustomerClassHiddenField').val()
      , CustomerKind: $('#strCstKindHiddenField').val()
      , FollowUpBoxStoreCode: $('#strStrCdHiddenField').val()
      , FollowUpBoxNumber: $('#lngFollowupBoxSeqNoHiddenField').val()
      , VehicleSequenceNumber: $('').val()
      , SalesStaffCode: $('#staffCd').val()
        });

        var pageManager;
        try {
            pageManager = Sys.WebForms.PageRequestManager.getInstance();
        } catch (e) {

        }

        if (pageManager) {
            var handle = setInterval(function () {
                if (pageManager.get_isInAsyncPostBack() == false) {
                    clearInterval(handle);
                    setTimeout(function () {
                        if (success == undefined || success == true) {
                            $('#ContractButton').click();
                        }
                         hideDispLoading();
                    }, 2000);
                }
            }, 500)
        }
    };

    event.stopPropagation();

    CheckAndSaveEstimate('1', saveEstimateCallback);

    //ポップアップクローズ
    $("#bodyFrame").trigger("hideOpenPopover");
    return false;
}

/* 印刷ボタン押下 */
function printLinkClick() {
    var saveEstimateCallback = function (success) {
        //パラメータ設定
        sc3070204Script.setParams({
            EstimateId: $('#lngEstimateIdHiddenField').val(),
            PaymentKbn: (HasSC3070205() ? frames["EstimateInfo"].$('#payMethodSegmentedButton input:checked').val() : ''),
            MenuLockStatusFlg: $('#operationLockedHiddenField').val(),
            BusinessFlg: $('#businessFlgHiddenField').val(),
            CheckResult: "true",
            InputErrorMessage: ''
            // 2013/11/27 TCS 高橋 Aカード情報相互連携開発 START
            , ContractApprovalStatus: $('#contractApprovalSatus').val()
            // 2013/11/27 TCS 高橋 Aカード情報相互連携開発 END
        });

        var pageManager;
        try {
            pageManager = Sys.WebForms.PageRequestManager.getInstance();
        } catch (e) {

        }

        if (pageManager) {
            var handle = setInterval(function () {
                if (pageManager.get_isInAsyncPostBack() == false) {
                    clearInterval(handle);
                    setTimeout(function () {
                        if (success == undefined || success == true) {
                            $('#PrintButton').click();
                        }
                        hideDispLoading();
                    }, 2000);
                }
            }, 500)
        }
    };

    event.stopPropagation();

    CheckAndSaveEstimate('0', saveEstimateCallback);

    //ポップアップクローズ
    $("#bodyFrame").trigger("hideOpenPopover");
    return false;
}

/* 価格相談ボタン押下時 */
function goUpdateData() {
    var saveEstimateCallback = function (success) {
        //パラメータ設定
        sc3070203Script.setParams({
            Estimateid: $('#lngEstimateIdHiddenField').val()
      , RequestPrice: $('').val()
      , Customerid: $('#strCRCustIdHiddenField').val()
      , CustomerName: $('#cstNameHiddenField').val()
      , CustomerClass: $('#strCustomerClassHiddenField').val()
      , CustomerKind: $('#strCstKindHiddenField').val()
      , FollowUpBoxStoreCode: $('#strStrCdHiddenField').val()
      , FollowUpBoxNumber: $('#lngFollowupBoxSeqNoHiddenField').val()
      , VehicleSequenceNumber: $('').val()
      , SalesStaffCode: $('#staffCdHiddenField').val()
      , SeriesCode: $('#seriesCdHiddenField').val()
      , SeriesName: $('#seriesNameHiddenField').val()
      , ModelCode: $('#modelCdHiddenField').val()
      , ModelName: $('#modelNameHiddenField').val()
        });

        var pageManager;
        try {
            pageManager = Sys.WebForms.PageRequestManager.getInstance();
        } catch (e) {

        }

        if (pageManager) {
            var handle = setInterval(function () {
                if (pageManager.get_isInAsyncPostBack() == false) {
                    clearInterval(handle);
                    setTimeout(function () {
                        if (success == undefined || success == true) {
                            $('#ApprovalButton').click();
                        }
                        hideDispLoading();
                    }, 2000);
                }
            }, 500)
        }
    };

    event.stopPropagation();

    CheckAndSaveEstimate('0', saveEstimateCallback);

    //ポップアップクローズ
    $("#bodyFrame").trigger("hideOpenPopover");
    return false;
}


/* 入力破棄チェック */
function inputUpdateCheck() {

    if (typeof frames["EstimateInfo"].inputUpdateCheck == "function") {
        //入力破棄チェック
        return frames["EstimateInfo"].inputUpdateCheck();
    }

    return true;
}

/* 新車納車システムリンクメニュー押下時の処理 */
function linkMenu(url) {
    location.href = url;
    return false;
}


//小数以下２桁を0埋め
function formatNumber(num) {
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



/**************************************************************
* 読み込み用メソッド
**************************************************************/
//オーバーレイ、ロード中表示
function dispLoading() {

    //オーバーレイ表示
    $("#serverProcessOverlayBlack").css("display", "block");
    //アニメーション(ロード中)
    $("#serverProcessIcon").addClass("show");
    $("#serverProcessOverlayBlack").addClass("open");
}

//透明オーバーレイ表示
function dispClearLoading() {
    //オーバーレイ表示
    $("#serverProcessOverlayBlack").css("display", "block");

}

function hideDispLoading() {
    //アニメーション
    $("#serverProcessIcon").removeClass("show");
    $("#serverProcessOverlayBlack").removeClass("open");
    //オーバーレイ表示
    $("#serverProcessOverlayBlack").css("display", "none");

}


/**************************************************************
* 子画面に提供するメソッド
**************************************************************/

/* 価格相談ボタン表示 */
function showPriceApprovalButton(dispmode) {
    if (dispmode == 0) {
        //価格相談ボタンを非表示にする
        $("#DiscountApprovalButton").hide(0);
        $("#ApprovalButton").css("display", "none");
    } else if (dispmode == 1) {
        //価格相談ボタンを表示にする
        $("#DiscountApprovalButton").show(0);
        $("#ApprovalButton").css("display", "block");
    }
}

/* 見積情報切り替え */
function changeEstimateInfo(selectEstimateId) {
    var saveEstimateCallback = function (success) {
        var EstimateId = this_form.estimateIdHiddenField.value;
        var EstimateIdList = EstimateId.split(",");

        for (i = 0; i < EstimateIdList.length; i++) {
            if (EstimateIdList[i] == selectEstimateId) {
                //選択している見積IDのIndexを変更
                this_form.selectedEstimateIndexHiddenField.value = i;
            }
        }

        //処理モード設定(見積切り替え)
        this_form.actionModeHiddenField.value = "3";

        //再表示へ
        this_form.submit();
    };

    CheckAndSaveEstimate('0', saveEstimateCallback);

    return;
}


/* 見積登録 */
function registerEstimateInfo() {
    var saveEstimateCallback = function (success) {
        hideDispLoading();
    };

    var rslt = CheckAndSaveEstimate('0', saveEstimateCallback);

    if (rslt == false) {
        return rslt
    }

    return  true; 
}

/* HTMLデコードを行う */
function SC3070201HTMLDecode(value) {
    return $("<Div>").html(value).text();
}

/* 子画面がGL版見積りかどうか判断する */
function HasSC3070205() {
	return (0 <= frames["EstimateInfo"].location.href.search(/SC3070205\.aspx/));
}
