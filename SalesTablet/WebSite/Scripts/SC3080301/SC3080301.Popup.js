/*━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
SC3080301.Popup.js
─────────────────────────────────────
機能： 査定依頼
補足： 査定依頼PopUpを開くタイミングにて遅延ロードする
作成： 2014/04/21 TCS 市川 GTMCタブレット高速化対応（BTS-386）
更新： 2014/11/04 TCS 藤井 iOS8 対応(i-CROP_V4_salesよりマージ)
─────────────────────────────────────*/

/***********************************************************
査定依頼
***********************************************************/	

//2012/05/23 TCS 鈴木(恭) ローディング中にフリーズする問題(号口課題No.83) START
/**
* 初期表示処理
* 
*/
function SC3080301displayAlert() {

    var prms = '';

    //メイン画面のコンテンツを削除	
    $('.scNsc412PopUpModelSelectListArea2>div').remove();

    //画面初期化情報を取得する
    callbackSC3080301.doCallback('AssessmentLoad', prms, function (result, context) {

        //タイマーをクリア
        commonClearTimer();

        SC3080301.endServerCallback();

        initializeWindow(result, context);

        var resArray = result.split(",");

        if (resArray[0] == "999") {	//異常終了時エラーメッセージ
            alert(resArray[1]);
            return;
        }
    });
};
//2012/05/23 TCS 鈴木(恭) ローディング中にフリーズする問題(号口課題No.83) END

function initializeWindow(result, context) {

    var contents = $('<Div>').html(result).text();
    var assessmentMain = $(contents).find('.scNsc412PopUpModelSelectListArea2');

    $('#scNsc412PopUpModelSelectListArea2>div').remove();
    assessmentMain.children('div').clone(true).appendTo('.scNsc412PopUpModelSelectListArea2');

    //2012/04/13 TCS 鈴木(恭) HTMLエンコード対応 START
    $("#SelectAssVinHidden").val(SC3080301HTMLEncode(SC3080301HTMLDecode($("#SelectAssVinHidden").val())));
    $("#SelectCarnoHidden").val(SC3080301HTMLEncode(SC3080301HTMLDecode($("#SelectCarnoHidden").val())));
    $("#SelectCarnameHidden").val(SC3080301HTMLEncode(SC3080301HTMLDecode($("#SelectCarnameHidden").val())));
    //2012/04/13 TCS 鈴木(恭) HTMLエンコード対応 END

    assessmentWindowScript();

};

//2012/04/13 TCS 鈴木(恭) HTMLエンコード対応 START
/**
* HTMLエンコードを行う
* 
* @param {String} value 
* 
*/
function SC3080301HTMLEncode(value) {
    return $("<Div>").text(value).html();
}

/**
* HTMLデコードを行う
* 
* @param {String} value 
* 
*/
function SC3080301HTMLDecode(value) {
    return $("<Div>").html(value).text();
}
//2012/04/13 TCS 鈴木(恭) HTMLエンコード対応 END

//画面表示用
function assessmentWindowScript() {

    $("#AssessmentCarSelectPopup").css("display", "block");

    //ポップアップの初期化
    $(".scNsc412PopUpScrollWrapAssessment").fingerScroll();

    //一旦全部のチェック状態削除
    $(".scNsc412AssessmentPopUpList02 li.scNsc412AssessmentListLi2").removeClass("On");

    /**
    * ページ切り替え関数
    */
    function setAssessmentPopupPage(pageClass) {
        //モード毎のラベル・ボタンを一旦全部非表示にする
        //        $("#AssessmentCarPopupMakerTitle,#AssessmentCarPopupModelTitle,#AssessmentCarPopupCancelLabel,#AssessmentCarPopupMakerBkLabel,#tgLeft").css("display", "none");
        $("#AssessmentCarPopupMakerTitle,#AssessmentCarPopupModelTitle,#scNscPopUpClosePanel,#scNscPopUpReturnPanel").css("display", "none");

        //ページ１
        if (pageClass === "page1") {
            //文言ラベルの制御
            $("#AssessmentCarPopupMakerTitle").show(0);
            $("#scNscPopUpClosePanel").css("display", "block");
        }

        //ページ２
        if (pageClass === "page2") {
            //文言ラベルの制御
            $("#AssessmentCarPopupModelTitle").show(0);
            $("#scNscPopUpReturnPanel").css("display", "block");
        }
        //ページクラス設定
        $("#AssessmentCarSelectPopupListWrap").removeClass("page1 page2").addClass(pageClass);
        //スクロール初期化
        $(".scNsc412PopUpScrollWrapAssessment").fingerScroll();
    };

    //ヘッダーのキャンセルボタン押下時の処理
    $(".scNscAssessmentPopUpCancelButton").bind("click", function (e) {
        //ポップアップクローズ
        $("#AssessmentCarSelectPopup").fadeOut(300);
    });
    //ヘッダーの戻るボタン押下時の処理
    $(".scNscAssessmentPopUpReturnButton").bind("click", function (e) {
        setAssessmentPopupPage("page1");
    });

    //車両を選択した時のイベント
    $(".scNsc412AssessmentPopUpList01 li.scNsc412AssessmentListLi1").bind("click", function (e) {

        var retention = $("#SelectRetentionHidden").val();
        var seqno = $("#SelectAssSeqnoHidden").val();

        if (retention === "0") {
            $(".scNsc412AssessmentPopUpList02 li.scNsc412AssessmentListLi2[retention='" + retention + "']").addClass("On");
        } else {
            $(".scNsc412AssessmentPopUpList02 li.scNsc412AssessmentListLi2[retention='" + retention + "'][seqno='" + seqno + "']").addClass("On");
        }
        //ページ１→ページ２
        setAssessmentPopupPage("page2");
    });
    
    //車両を選択した時のイベント
    $(".scNsc412AssessmentPopUpList02 li.scNsc412AssessmentListLi2").bind("click", function (e) {

        //一旦全部のチェック状態削除
        $(".scNsc412AssessmentPopUpList02 li.scNsc412AssessmentListLi2").removeClass("On");

        //キー取得
        //2012/04/13 TCS 鈴木(恭) HTMLエンコード対応 START
        var seqno = SC3080301HTMLEncode($(this).attr("seqno"));
        var vin = SC3080301HTMLEncode($(this).attr("vin"));
        var carno = SC3080301HTMLEncode($(this).attr("carno"));
        var carname = SC3080301HTMLEncode($(this).attr("carname"));
        var assessno = SC3080301HTMLEncode($(this).attr("assessno"));
        var noticeid = SC3080301HTMLEncode($(this).attr("noticeid"));
        var insdate = SC3080301HTMLEncode($(this).attr("insdate"));
        var price = SC3080301HTMLEncode($(this).attr("price"));
        var retention = SC3080301HTMLEncode($(this).attr("retention"));
        var status = SC3080301HTMLEncode($(this).attr("status"));
        //2012/04/13 TCS 鈴木(恭) HTMLエンコード対応 END
        $(this).toggleClass("On");
        if (seqno == '0') {
            //その他車両の場合
            status = $("#SelectOtherStatusHidden").val();
            insdate = $("#SelectOtherDateHidden").val();
            price = $("#SelectOtherPriceHidden").val();
            $("#SelectAssVinHidden").val(vin);
            $("#SelectAssSeqnoHidden").val(seqno);
            $("#SelectInspectionDateHidden").val(insdate);
            $("#SelectApprisalPriceHidden").val(price);
            $("#SelectAssessmentNoHidden").val($("#SelectOtherAssessmentNoHidden").val());
            $("#SelectNoticeReqIdHidden").val($("#SelectOtherNoticeReqIdHidden").val());
            $("#SelectStatusHidden").val(status);
            $("#SelectRetentionHidden").val(retention);
        } else {
            $("#SelectAssSeqnoHidden").val(seqno);
            $("#SelectAssVinHidden").val(vin);
            $("#SelectInspectionDateHidden").val(insdate);
            $("#SelectApprisalPriceHidden").val(price);
            $("#SelectAssessmentNoHidden").val(assessno);
            $("#SelectNoticeReqIdHidden").val(noticeid);
            $("#SelectStatusHidden").val(status);
            $("#SelectRetentionHidden").val(retention);
        }
        $("#SelectRetentionHidden").val(retention);
        //メーカーリストのチェック状態を最新化
/*  2012/03/01 鈴木(健) 呼出し元の顧客詳細(商談情報)画面：SC3080202とfunction名が重複している為、別名にリネーム。 START
        setMekerCheckState(seqno, carno, carname, status, insdate, price); */
        //2012/04/13 TCS 鈴木(恭) HTMLエンコード対応 START
        setAssessmentMekerCheckState(SC3080301HTMLDecode(seqno), SC3080301HTMLDecode(carno), SC3080301HTMLDecode(carname), SC3080301HTMLDecode(status), SC3080301HTMLDecode(insdate), SC3080301HTMLDecode(price));
        //2012/04/13 TCS 鈴木(恭) HTMLエンコード対応 END
/*  2012/03/01 鈴木(健) 呼出し元の顧客詳細(商談情報)画面：SC3080202とfunction名が重複している為、別名にリネーム。 END */
        setAssessmentPopupPage("page1");
    });

    //車両リストのチェック状態を最新化
/*  2012/03/01 鈴木(健) 呼出し元の顧客詳細(商談情報)画面：SC3080202とfunction名が重複している為、別名にリネーム。 START
    function setMekerCheckState(seqno, carno, carname, status, insdate, price) { */
    function setAssessmentMekerCheckState(seqno, carno, carname, status, insdate, price) {
/*  2012/03/01 鈴木(健) 呼出し元の顧客詳細(商談情報)画面：SC3080202とfunction名が重複している為、別名にリネーム。 END */

        if (seqno == '') {
            $("#RequestStatusPanel").css("display", "none");
            $("#EndStatusPanel").css("display", "none");
        } else {
            if (status == '1' || status == '3') {
                $("#RequestStatusPanel").css("display", "block");
                $("#EndStatusPanel").css("display", "none");
                if (seqno == '0') {
                    $("#OtherRequestLabel").css("display", "block");
                    $("#RequestRegLabel").css("display", "none");
                    $("#RequestCarLabel").css("display", "none");
                } else {
                    $("#OtherRequestLabel").css("display", "none");
                    $("#RequestRegLabel").css("display", "block");
                    $("#RequestCarLabel").css("display", "block");
                    $("#RequestRegLabel").text(carno);
                    $("#RequestCarLabel").text(carname);
                }
            } else {
                $("#RequestStatusPanel").css("display", "none");
                $("#EndStatusPanel").css("display", "block");
                if ($("#SelectAccountStatusHidden").val() == '1') {
                    $("#AssessmentEnableButtonPanel").css("display", "block");
                    $("#AssessmentDisableButtonPanel").css("display", "none");
                } else {
                    $("#AssessmentEnableButtonPanel").css("display", "none");
                    $("#AssessmentDisableButtonPanel").css("display", "block");
                }

                if (status == '4') {
                    $("#AssessmentResultPanel").css("display", "block");
                    $("#AssessmentDateLabel").text(insdate);
                    $("#AssessmentPriceLabel").text(price);
                } else {
                    $("#AssessmentResultPanel").css("display", "none");
                }
                if (seqno == '0') {
                    $("#OtherAssessmentLabel").css("display", "block");
                    $("#AssessmentRegLabel").css("display", "none");
                    $("#AssessmentCarLabel").css("display", "none");
                } else {
                    $("#OtherAssessmentLabel").css("display", "none");
                    $("#AssessmentRegLabel").css("display", "block");
                    $("#AssessmentCarLabel").css("display", "block");
                    $("#AssessmentRegLabel").text(carno);
                    $("#AssessmentCarLabel").text(carname);
                }
            }
        }
    }

    //査定の依頼ボタン押下時の処理
    $(".nscPopUpAssessmentButton").bind("click", function (e) {
        //隠し項目に一括反映
        //チェックOFFを全体に反映
        $(".scNsc412AssessmentPopUpList02 li .scNsc412AssessmentPopUpList02Hidden input[type='hidden']:nth-child(3)").val("False");
        $(".scNsc412AssessmentPopUpList02 li.On .scNsc412AssessmentPopUpList02Hidden input[type='hidden']:nth-child(3)").val("True");

        //ボタンを2重で押せなくする
        $("#AssessmentEnableButtonPanel").css("display", "none");
        $("#AssessmentDisableButtonPanel").css("display", "block");

        var prms = '';
        //2012/04/13 TCS 鈴木(恭) HTMLエンコード対応 START
        //2012/03/09 TCS 鈴木(恭) 【SALES_2】コールバック時の文字列のエンコード処理追加 START
        prms = prms + encodeURIComponent(SC3080301HTMLDecode($('#SelectAssessmentNoHidden').val())) + ',';
        prms = prms + encodeURIComponent(SC3080301HTMLDecode($('#SelectNoticeReqIdHidden').val())) + ',';
        prms = prms + encodeURIComponent(SC3080301HTMLDecode($('#SelectRetentionHidden').val())) + ',';
        prms = prms + encodeURIComponent(SC3080301HTMLDecode($('#SelectAssVinHidden').val())) + ',';
        prms = prms + encodeURIComponent(SC3080301HTMLDecode($('#SelectAssSeqnoHidden').val()));
        //2012/03/09 TCS 鈴木(恭) 【SALES_2】コールバック時の文字列のエンコード処理追加 END
        //2012/04/13 TCS 鈴木(恭) HTMLエンコード対応 END

        SC3080301.startServerCallback();
        //2012/05/23 TCS 鈴木(恭) ローディング中にフリーズする問題(号口課題No.83) START
        commonRefreshTimer(SC3080301displayAlert);

        callbackSC3080301.doCallback('AssessmentRegister', prms, function (result, context) {
            SC3080301displayAlert();
        });
        //2012/05/23 TCS 鈴木(恭) ローディング中にフリーズする問題(号口課題No.83) END
    });

    //査定のキャンセルボタン押下時の処理
    $(".nscPopUpAssessmentCancelButton").bind("click", function (e) {

        //ボタンを2重で押せなくする
        $("#AssessmentEnableCancelButtonPanel").css("display", "none");
        $("#AssessmentDisableCancelButtonPanel").css("display", "block");

        var prms = '';
        //2012/04/13 TCS 鈴木(恭) HTMLエンコード対応 START
        //2012/03/09 TCS 鈴木(恭) 【SALES_2】コールバック時の文字列のエンコード処理追加 START
        prms = prms + encodeURIComponent(SC3080301HTMLDecode($('#SelectAssessmentNoHidden').val())) + ',';
        prms = prms + encodeURIComponent(SC3080301HTMLDecode($('#SelectNoticeReqIdHidden').val())) + ',';
        prms = prms + encodeURIComponent(SC3080301HTMLDecode($('#SelectRetentionHidden').val())) + ',';
        prms = prms + encodeURIComponent(SC3080301HTMLDecode($('#SelectAssVinHidden').val())) + ',';
        prms = prms + encodeURIComponent(SC3080301HTMLDecode($('#SelectAssSeqnoHidden').val()));
        //2012/03/09 TCS 鈴木(恭) 【SALES_2】コールバック時の文字列のエンコード処理追加 END
        //2012/04/13 TCS 鈴木(恭) HTMLエンコード対応 END

        SC3080301.startServerCallback();

        //2012/05/23 TCS 鈴木(恭) ローディング中にフリーズする問題(号口課題No.83) START
        commonRefreshTimer(SC3080301displayAlert);

        callbackSC3080301.doCallback('AssessmentCancel', prms, function (result, context) {
            SC3080301displayAlert();
        });
        //2012/05/23 TCS 鈴木(恭) ローディング中にフリーズする問題(号口課題No.83) END

    });

    //カーチェックシート遷移ボタン押下時の処理
    $(".nscPopUpContactCarCheckButton01").bind("click", function (e) {

        //2012/02/21 TCS 鈴木(健) 【SALES_1B】カーチェックシート画面への遷移時の処理中表示追加
        $('#IsCarCheckSheetOpenHidden').val("True");
        //カーチェックシート画面の主要配置エリアのZ-INDEXを0に設定する
        //（登録時のオーバーレイのZ-INDEXが効かないことによる暫定対応）
        var targetElement = document.getElementById("optList");
        if (targetElement != null) {
            targetElement.style["z-index"] = "0";
        }
        SC3080301.startServerCallback();
        //2012/02/21 TCS 鈴木(健) 【SALES_1B】カーチェックシート画面への遷移時の処理中表示追加
        //2012/05/23 TCS 鈴木(恭) ローディング中にフリーズする問題(号口課題No.83) START
        commonRefreshTimer(SC3080301displayAlert);
        //2012/05/23 TCS 鈴木(恭) ローディング中にフリーズする問題(号口課題No.83) END

        //ＤＢ反映用ダミーボタン押下
        $("#linkCarCheckSheetButtonDummy").click();

        //2012/02/21 TCS 鈴木(健) 【SALES_1B】カーチェックシート画面への遷移時の処理中表示追加 START
        ////ポップアップクローズ
        //$("#AssessmentCarSelectPopup").fadeOut(300);
        //2012/02/21 TCS 鈴木(健) 【SALES_1B】カーチェックシート画面への遷移時の処理中表示追加 END
    });

    //ポップアップクローズの監視
    $(document.body).bind("mousedown touchstart", function (e) {
        if ($("#AssessmentCarSelectPopup").is(":visible") === false) return;
        if ($(e.target).is("#AssessmentCarSelectPopup, #AssessmentCarSelectPopup *") === false) {
            $("#AssessmentCarSelectPopup").fadeOut(300);
            //SC3080301.endServerCallback();
        }
    });
};

/** 車両先頭行、最終行判定 **/
/*  2012/03/01 鈴木(健) 呼出し元の顧客詳細(商談情報)画面：SC3080202とfunction名が重複している為、別名にリネーム。 START
function modelMasterStyle(id) { */
function assessmentModelMasterStyle(id) {
/*  2012/03/01 鈴木(健) 呼出し元の顧客詳細(商談情報)画面：SC3080202とfunction名が重複している為、別名にリネーム。 END */
    var index = 0
    var count = $(".scNsc412AssessmentPopUpList02 li.scNsc412AssessmentListLi2").parent().children("[seqno='" + id + "']").size()
    //表示対象のスタイル設定
    $(".scNsc412AssessmentPopUpList02 li.scNsc412AssessmentListLi2").each(function () {
        if ($(this).attr("seqno") == id) {
            //先頭行のスタイル
            if (index == 0) {
                $(this).addClass("top");
            }
            //最終行のスタイル
            if (index == count - 1) {
                $(this).addClass("bottom");
            }
            index++;
        }
    });
}

/**
* 初期処理
*/
(function (window) {

    $.extend(window, { SC3080301: {} });
    $.extend(SC3080301, {

        /**
        * コールバック開始
        */
        startServerCallback: function () {
            SC3080301.showLoding();
        },

        /**
        * コールバック終了
        */
        endServerCallback: function () {
            SC3080301.closeLoding();
        },

        /******************************************************************************
        読み込み中表示
        ******************************************************************************/
        
        /**
        * 読み込み中アイコン表示
        */
        showLoding: function () {

            //オーバーレイ表示
            //2012/02/21 TCS 鈴木(健) 【SALES_1B】カーチェックシート画面への遷移時の処理中表示追加 START
            //$("#registOverlayBlackSC3080301").css("display", "block");
            if ($('#IsCarCheckSheetOpenHidden').val() == "True") {
                $("#registOverlayBlackSC3080301_Redirect").css("display", "block");
            } else {
                $("#registOverlayBlackSC3080301").css("display", "block");
            }
            //2012/02/21 TCS 鈴木(健) 【SALES_1B】カーチェックシート画面への遷移時の処理中表示追加 END
            //アニメーション
            setTimeout(function () {
                //2012/02/21 TCS 鈴木(健) 【SALES_1B】カーチェックシート画面への遷移時の処理中表示追加 START
                //$("#processingServerSC3080301").addClass("show");
                //$("#registOverlayBlackSC3080301").addClass("open");
                if ($('#IsCarCheckSheetOpenHidden').val() == "True") {
                    $("#processingServerSC3080301_Redirect").addClass("show");
                    $("#registOverlayBlackSC3080301_Redirect").addClass("open");
                } else {
                    $("#processingServerSC3080301").addClass("show");
                    $("#registOverlayBlackSC3080301").addClass("open");
                }
                //2012/02/21 TCS 鈴木(健) 【SALES_1B】カーチェックシート画面への遷移時の処理中表示追加 END
            }, 0);

        },

        /**
        * 読み込み中アイコンを非表示にする
        */
        closeLoding: function () {
        //2012/05/23 TCS 鈴木(恭) ローディング中にフリーズする問題(号口課題No.83) START
        //2012/02/21 TCS 鈴木(健) 【SALES_1B】カーチェックシート画面への遷移時の処理中表示追加 START
                if ($('#IsCarCheckSheetOpenHidden').val() == "True") {
                    $("#processingServerSC3080301_Redirect").removeClass("show");
                    //2014/11/04 TCS 藤井 iOS8 対応(i-CROP_V4_salesよりマージ)  Start
                    $("#registOverlayBlackSC3080301_Redirect").removeClass("open");
                    setTimeout(function () {
                        $("#registOverlayBlackSC3080301_Redirect").css("display", "none");
                    }, 300);
                    //2014/11/04 TCS 藤井 iOS8 対応(i-CROP_V4_salesよりマージ)  End
                    $('#IsCarCheckSheetOpenHidden').val("False");
                } else {
                    $("#processingServerSC3080301").removeClass("show");
                    //2014/11/04 TCS 藤井 iOS8 対応(i-CROP_V4_salesよりマージ)  Start
                    $("#registOverlayBlackSC3080301").removeClass("open");
                    setTimeout(function () {
                        $("#registOverlayBlackSC3080301").css("display", "none");
                    }, 300);
                    //2014/11/04 TCS 藤井 iOS8 対応(i-CROP_V4_salesよりマージ)  End
                }
        //2012/02/21 TCS 鈴木(健) 【SALES_1B】カーチェックシート画面への遷移時の処理中表示追加 END
        //2012/05/23 TCS 鈴木(恭) ローディング中にフリーズする問題(号口課題No.83) END
        }
    });

})(window);
