/**
* @fileOverview SC3070204　見積書・契約書印刷画面処理
*
* @author TCS 坪根
* @version 1.0.0
* 更新： 2013/01/16 TCS 上田     GTMC121228118対応
* 更新： 2013/03/08 TCS 山田    【A.STEP2】新車タブレット受付画面の管理指標変更対応
* 更新： 2013/07/11 TCS 坪根     GL0895対応
* 更新： 2013/11/27 TCS 河原     Aカード情報相互連携開発
* 更新： 2014/11/04 TCS 藤井     iOS8 対応(i-CROP_V4_salesよりマージ) 
*/

var sc3070204Script;

sc3070204Script = function () {
    /******************************************************************************
    定数定義
    ******************************************************************************/
    /**
    * @コールバック要求処理名
    */
    var constants = {
        initialize: "Initialize",
        updateEstimatePrintDate: "UpdateEstimatePrintDate",
        updateContractPrintFlg: "UpdateContractPrintFlg",
        decideContractInfo: "DecideContractInfo",
        cancelContractInfo: "CancelContractInfo",
        errorProc: "ErrorProc",
        // 2013/03/08 TCS 山田    【A.STEP2】新車タブレット受付画面の管理指標変更対応 START
        orderUpdateContractPrintFlg: "OrderUpdateContractPrintFlg",
        // 2013/03/08 TCS 山田    【A.STEP2】新車タブレット受付画面の管理指標変更対応 END
        // 2013/07/11 TCS 坪根 GL0895対応 START
        successContractPrint: "SuccessContractPrint",
        successOrderPrint: "SuccessOrderPrint"
        // 2013/07/11 TCS 坪根 GL0895対応 END
    }

    /**
    * @コールバック関数定義
    */
    var callBack = {
        doCallback: function (argument, callbackFunction) {
            this.packedArgument = JSON.stringify(argument);
            this.endCallback = callbackFunction;
            this.beginCallback();
        }
    };

    /**
    * @ボタン押下インデックス
    *  ※リロード時に見積作成画面で、当画面で押下したボタンを判別するために使用
    */
    var buttonIndex = {
        EstPrintButton: "4",
        ConPrintButton: "5",
        ConDecideButton: "6",
        ConCancelButton: "7",
        // 2013/03/08 TCS 山田    【A.STEP2】新車タブレット受付画面の管理指標変更対応 START
        OrdPrintButton: "8"
        // 2013/03/08 TCS 山田    【A.STEP2】新車タブレット受付画面の管理指標変更対応 END
    }

    /**
    * @契約書印刷フラグ
    *  (印刷済)
    */
    var C_CONTRACT_PRINT_FLG_ON = "1";

    /**
    * @契約状況フラグ
    *  (契約済)
    */
    var C_CONTRACT_STATUS_FLG_ON = "1";

    /**
    * @印刷処理の結果値
    *  (正常値)
    *  Remakes:正常値は、204で返ってくる
    */
    var C_RESULT_PRINT_SUCCESS = 204;

    /**
    * @メッセージID
    *  (メッセージID:901)
    */
    var C_MESSAGE_ID_901 = 901;

    /**
    * @メッセージID
    *  (メッセージID:902)
    */
    var C_MESSAGE_ID_902 = 902;

    /**
    * @メッセージID
    *  (メッセージID:903)
    */
    var C_MESSAGE_ID_903 = 903;

    // 2013/03/08 TCS 山田    【A.STEP2】新車タブレット受付画面の管理指標変更対応 START
    /**
    * @メッセージID
    *  (メッセージID:906)
    */
    var C_MESSAGE_ID_906 = 906;
    // 2013/03/08 TCS 山田    【A.STEP2】新車タブレット受付画面の管理指標変更対応 END
    
    // 2013/07/11 TCS 坪根 GL0895対応 START
    /**
    * @印刷処理の正常時のログメッセージ内容
    *  Remakes:
    */
    var C_RESULT_PRINT_SUCCESS_MESSAGE = "SUCCESS";
    // 2013/07/11 TCS 坪根 GL0895対応 END

    /******************************************************************************
    パラメータ
    ******************************************************************************/
    /**
    * @呼出元のインプット項目
    */
    var parentParam = {
        EstimateId: ""
		, PaymentKbn: ""
		, MenuLockStatusFlg: ""
        , BusinessFlg: ""
        , CheckResult: ""
        , InputErrorMessage: ""
        // 2013/11/27 TCS 高橋 Aカード情報相互連携開発 START
        , ContractApprovalStatus: ""
        // 2013/11/27 TCS 高橋 Aカード情報相互連携開発 END
    };

    /******************************************************************************
    Private変数
    ******************************************************************************/
    /**
    * @見積書・契約書印刷画面
    */
    var _popForm;

    /**
    * @見積書印刷情報(XML)
    */
    var _xmlPrintEstimateInfo;

    // 2013/03/08 TCS 山田    【A.STEP2】新車タブレット受付画面の管理指標変更対応 START
    /**
    * @注文書印刷情報(XML)
    */
    var _xmlPrintOrderInfo;
    // 2013/03/08 TCS 山田    【A.STEP2】新車タブレット受付画面の管理指標変更対応 END

    /**
    * @契約書印刷情報(XML)
    */
    var _xmlprintContractInfo;

    /******************************************************************************
    処理中アニメーション
    ******************************************************************************/
    /**
    * 読み込み中アイコン表示
    */
    function showLoading() {

        //オーバーレイ表示
        $("#registOverlayBlackSC3070204").css("display", "block");
        //アニメーション
        setTimeout(function () {
            $("#processingServerSC3070204").addClass("show");
            $("#registOverlayBlackSC3070204").addClass("open");
        }, 0);

    }

    /**
    * 読み込み中アイコンを非表示にする
    */
    function closeLoading() {
        $("#processingServerSC3070204").removeClass("show");
        //2014/11/04 TCS 藤井 iOS8 対応(i-CROP_V4_salesよりマージ)  Start
        $("#registOverlayBlackSC3070204").removeClass("open");
        setTimeout(function () {
            $("#registOverlayBlackSC3070204").css("display", "none");
        }, 300);
        //2014/11/04 TCS 藤井 iOS8 対応(i-CROP_V4_salesよりマージ)  End
    }

    /******************************************************************************
    サーバーコールバック
    (クライアント → サーバー)
    ******************************************************************************/
    /**
    * @初期表示時
    */
    function initialize() {
        // 画面内の表示を一時的に非表示
        $("#SC3070204MainFrameContent").css("display", "none");

        var strMenuLockStatusFlg = parentParam.MenuLockStatusFlg.toLowerCase();
        var strBusinessFlg = parentParam.BusinessFlg.toLowerCase();
        var strCheckResult = parentParam.CheckResult.toLowerCase();

        // 大文字を小文字に変換
        parentParam.MenuLockStatusFlg = strMenuLockStatusFlg;
        parentParam.BusinessFlg = strBusinessFlg;
        parentParam.CheckResult = strCheckResult;

        // クルクルをここで開始
        showLoading();

        // サーバーへコールバック
        callBack.doCallback({ EstimateId: parentParam.EstimateId,
            PaymentKbn: parentParam.PaymentKbn,
            Method: constants.initialize,
            ShowDialogErrorId: 0,
            ShowDialogErrorMessage: "",
            ErrorLogValue: ""
        }, clientCallBackEnd);
    }

    /**
    * @見積書ボタンクリック時
    */
    function estimatePrintClick() {
        // クルクルをここで開始
        showLoading();

        // タブレット基盤へメソッド名を送信
        var urlTabret;
        try {
            urlTabret = getUrlSchemeMethod("sendEstimatePrintInfo", "getResEstimatePrint");

            window.location = urlTabret;
        } catch (e) {
            var errMessage;

            // エラー情報を設定
            errMessage = "ErrorId=" + C_MESSAGE_ID_902 + ", " +
                         "ErrorMessage=" + $("#HdnMessage902").val() + ", " +
                         "Exception ErrorNumber=" + e.ErrorNumber + ", " +
                         "Exception Description=" + e.Description + ", " +
                         "urlTabret=" + urlTabret;

            // エラー情報をログ出力
            callBack.doCallback({ EstimateId: parentParam.EstimateId,
                PaymentKbn: parentParam.PaymentKbn,
                Method: constants.errorProc,
                ShowDialogErrorId: C_MESSAGE_ID_902,
                ShowDialogErrorMessage: $("#HdnMessage902").val(),
                ErrorLogValue: errMessage
            }, clientCallBackEnd);
        }
    }

    // 2013/03/08 TCS 山田    【A.STEP2】新車タブレット受付画面の管理指標変更対応 START
    /**
    * @注文書ボタンクリック時
    */
    function orderPrintClick() {
        // 見積作成画面で入力エラーがあったか
        if (parentParam.CheckResult == "false") {
            window.alert(parentParam.InputErrorMessage);

            // 当画面を閉じる
            $(".SC3070204header-left").click();
            return;
        }

        // クルクルをここで開始
        showLoading();

        // 契約書印刷フラグ更新
        callBack.doCallback({ EstimateId: parentParam.EstimateId,
            PaymentKbn: parentParam.PaymentKbn,
            Method: constants.orderUpdateContractPrintFlg,
            ShowDialogErrorId: 0,
            ShowDialogErrorMessage: "",
            ErrorLogValue: ""
        }, sendTabretOrder);

    }

    /**
    * @タブレット送信を行う
    */
    function sendTabretOrder(result, context) {

        // 契約書印刷フラグ更新結果確認
        var jsonResult = JSON.parse(result);
        if (jsonResult.ResultCode != 0) {
            // エラー処理
            errorProc(jsonResult.ResultCode, jsonResult.Message);

            return;
        }

        // タブレット基盤へメソッド名を送信
        var urlTabret;
        try {
            urlTabret = getUrlSchemeMethod("sendOrderPrintInfo", "getResOrderPrint");
            window.location = urlTabret;
        } catch (e) {
            var errMessage;

            // エラー情報を設定
            errMessage = "ErrorId= " + C_MESSAGE_ID_906 + ", " +
                         "ErrorMessage= " + $("#HdnMessage906").val() + ", " +
                         "Exception ErrorNumber= " + e.ErrorNumber + ", " +
                         "Exception Description= " + e.Description + ", " +
                         "urlTabret= " + urlTabret;

            // エラー情報をログ出力
            callBack.doCallback({ EstimateId: parentParam.EstimateId,
                PaymentKbn: parentParam.PaymentKbn,
                Method: constants.errorProc,
                ShowDialogErrorId: C_MESSAGE_ID_906,
                ShowDialogErrorMessage: $("#HdnMessage906").val(),
                ErrorLogValue: errMessage
            }, clientCallBackEnd);
        }
    }
    // 2013/03/08 TCS 山田    【A.STEP2】新車タブレット受付画面の管理指標変更対応 END

    /**
    * @契約書ボタンクリック時
    */
    function contectPrintClick() {
        // 見積作成画面で入力エラーがあったか
        if (parentParam.CheckResult == "false") {
            window.alert(parentParam.InputErrorMessage);

            // 当画面を閉じる
            $(".SC3070204header-left").click();
            return;
        }

        // クルクルをここで開始
        showLoading();

        // 2013/01/16 TCS 上田 GTMC121228118対応 START
        //// タブレット基盤へメソッド名を送信
        //var urlTabret;
        //try {
        //    urlTabret = getUrlSchemeMethod("sendContractPrintInfo", "getResContractPrint");
        //
        //    window.location = urlTabret;
        //} catch (e) {
        //    var errMessage;
        //
        //    // エラー情報を設定
        //    errMessage = "ErrorId= " + C_MESSAGE_ID_903 + ", " +
        //                 "ErrorMessage= " + $("#HdnMessage903").val() + ", " +
        //                 "Exception ErrorNumber= " + e.ErrorNumber + ", " +
        //                 "Exception Description= " + e.Description + ", " +
        //                 "urlTabret= " + urlTabret;
        //
        //    // エラー情報をログ出力
        //    callBack.doCallback({ EstimateId: parentParam.EstimateId,
        //        PaymentKbn: parentParam.PaymentKbn,
        //        Method: constants.errorProc,
        //        ShowDialogErrorId: C_MESSAGE_ID_903,
        //        ShowDialogErrorMessage: $("#HdnMessage903").val(),
        //        ErrorLogValue: errMessage
        //    }, clientCallBackEnd);
        //}

        // 契約書印刷フラグ更新
        callBack.doCallback({ EstimateId: parentParam.EstimateId,
            PaymentKbn: parentParam.PaymentKbn,
            Method: constants.updateContractPrintFlg,
            ShowDialogErrorId: 0,
            ShowDialogErrorMessage: "",
            ErrorLogValue: ""
        }, sendTabret);
        // 2013/01/16 TCS 上田 GTMC121228118対応 END

    }

    // 2013/01/16 TCS 上田 GTMC121228118対応 START
    /**
    * @タブレット送信を行う
    */
    function sendTabret(result, context) {

        // 契約書印刷フラグ更新結果確認
        var jsonResult = JSON.parse(result);
        if (jsonResult.ResultCode != 0) {
            // エラー処理
            errorProc(jsonResult.ResultCode, jsonResult.Message);

            return;
        }

        // タブレット基盤へメソッド名を送信
        var urlTabret;
        try {
            urlTabret = getUrlSchemeMethod("sendContractPrintInfo", "getResContractPrint");
            window.location = urlTabret;
        } catch (e) {
            var errMessage;

            // エラー情報を設定
            errMessage = "ErrorId= " + C_MESSAGE_ID_903 + ", " +
                         "ErrorMessage= " + $("#HdnMessage903").val() + ", " +
                         "Exception ErrorNumber= " + e.ErrorNumber + ", " +
                         "Exception Description= " + e.Description + ", " +
                         "urlTabret= " + urlTabret;

            // エラー情報をログ出力
            callBack.doCallback({ EstimateId: parentParam.EstimateId,
                PaymentKbn: parentParam.PaymentKbn,
                Method: constants.errorProc,
                ShowDialogErrorId: C_MESSAGE_ID_903,
                ShowDialogErrorMessage: $("#HdnMessage903").val(),
                ErrorLogValue: errMessage
            }, clientCallBackEnd);
        }
    }
    // 2013/01/16 TCS 上田 GTMC121228118対応 END

    /**
    * @確定ボタン
    */
    function contectDecideClick() {
        // 見積作成画面で入力エラーがあったか
        if (parentParam.CheckResult == "false") {
            window.alert(parentParam.InputErrorMessage);

            // 当画面を閉じる
            $(".SC3070204header-left").click();
            return;
        }

        // 確認メッセージを表示
        var strMessage = SC3070204HtmlDecode($("#HdnMessage904").val());
        if (!window.confirm(strMessage)) {
            return;
        }

        // クルクルをここで開始
        showLoading();

        callBack.doCallback({ EstimateId: parentParam.EstimateId,
            PaymentKbn: parentParam.PaymentKbn,
            Method: constants.decideContractInfo,
            ShowDialogErrorId: 0,
            ShowDialogErrorMessage: "",
            ErrorLogValue: ""
        }, clientCallBackEnd);
    }

    /**
    * @キャンセルクリック時
    */
    function contectCancelClick() {
        // 確認メッセージを表示
        var strMessage = SC3070204HtmlDecode($("#HdnMessage905").val());
        if (!window.confirm(strMessage)) {
            return;
        }

        // クルクルをここで開始
        showLoading();

        callBack.doCallback({ EstimateId: parentParam.EstimateId,
            PaymentKbn: parentParam.PaymentKbn,
            Method: constants.cancelContractInfo,
            ShowDialogErrorId: 0,
            ShowDialogErrorMessage: "",
            ErrorLogValue: ""
        }, clientCallBackEnd);
    }

    /**********************a********************************************************
    クライアントコールバック
    (サーバー → クライアント)
    ******************************************************************************/
    /**
    * @コールバック後、終了する
    * @(正常用)
    */
    function clientCallBackEnd(result, context) {
        var jsonResult = JSON.parse(result);

        if (jsonResult.ResultCode != 0) {
            // エラー処理
            errorProc(jsonResult.ResultCode, jsonResult.Message);

            return;
        }

        // ボタン押下インデックスを設定
        var retButtonIndex = "";
        switch (jsonResult.Caller) {
            case constants.initialize:                  // 初期表示時
                // 見積書の印刷情報をメモリ上で保管
                _xmlPrintEstimateInfo = jsonResult.PrintEstimateInfo;
                // 2013/03/08 TCS 山田    【A.STEP2】新車タブレット受付画面の管理指標変更対応 START
                // 注文書の印刷情報をメモリ上で保管
                _xmlPrintOrderInfo = jsonResult.PrintOrderInfo;
                // 2013/03/08 TCS 山田    【A.STEP2】新車タブレット受付画面の管理指標変更対応 END
                // 契約書の印刷情報をメモリ上で保管
                _xmlprintContractInfo = jsonResult.PrintContractInfo;

                // コントロール属性設定
                setButtonVisible(jsonResult.ContractPrintFlg,
                                 jsonResult.ContractStatusFlg,
                                 jsonResult.FllwupBoxSeqNo);

                // 画面内を表示
                $("#SC3070204MainFrameContent").css("display", "block");

                // ボタン押下インデックスを設定(リロードはしないので、空白を設定)
                retButtonIndex = ""

                break;
            case constants.updateEstimatePrintDate:     // 見積印刷日更新             ※見積書印刷ボタン押下時
                // ボタン押下インデックスを設定
                retButtonIndex = buttonIndex.EstPrintButton;

                break;
            // 2013/03/08 TCS 山田    【A.STEP2】新車タブレット受付画面の管理指標変更対応 START  
            case constants.orderUpdateContractPrintFlg: // 契約書印刷フラグ更新       ※注文書印刷ボタン押下時
                // ボタン押下インデックスを設定
                retButtonIndex = buttonIndex.OrdPrintButton;

                break;
            // 2013/03/08 TCS 山田    【A.STEP2】新車タブレット受付画面の管理指標変更対応 END  
            case constants.updateContractPrintFlg:     // 契約書印刷フラグ更新       ※契約書印刷ボタン押下時
                // ボタン押下インデックスを設定
                retButtonIndex = buttonIndex.ConPrintButton;

                break;
            case constants.decideContractInfo:          // 契約情報更新(確定時)       ※確定ボタン押下時
                // ボタン押下インデックスを設定
                retButtonIndex = buttonIndex.ConDecideButton;

                break;
            case constants.cancelContractInfo:          // 契約情報更新(キャンセル時)  ※キャンセルボタン押下時
                // ボタン押下インデックスを設定
                retButtonIndex = buttonIndex.ConCancelButton;

                break;                
            // 2013/07/11 TCS 坪根 GL0895対応 START 
            case constants.successContractPrint:         // 契約書印刷処理正常ログ出力時
                // ボタン押下インデックスを設定
                retButtonIndex = buttonIndex.ConPrintButton;

                break;
            case constants.successOrderPrint:            // 注文書印刷処理正常ログ出力時
                // ボタン押下インデックスを設定
                retButtonIndex = buttonIndex.OrdPrintButton;

                break;
            // 2013/07/11 TCS 坪根 GL0895対応 END  
            default:                                    // 上記以外
                // 処理不要の為、未処理
                break;
        }

        // 2013/01/16 TCS 上田 GTMC121228118対応 START
        //// クルクルを閉じる
        //closeLoading();
        //// 見積作成画面のリロードを使って、画面リロード
        //if (retButtonIndex != "") {
        //    // リロード
        //    refreshPage(retButtonIndex);
        //
        //    // リロード後は、当画面は不要になるので、手動で当画面をクローズ
        //    $(".SC3070204header-left").click();
        //}

        // 見積作成画面をリロード
        refreshParentPage(retButtonIndex);
        // 2013/01/16 TCS 上田 GTMC121228118対応 END
    }

    /**
    * @見積書印刷情報取得処理
    * @remarks
    * @タブレットからは、外部関数経由でこのメソッドをコール
    */
    function getEstimatePrint() {
        return _xmlPrintEstimateInfo;
    }

    // 2013/03/08 TCS 山田    【A.STEP2】新車タブレット受付画面の管理指標変更対応 START
    /**
    * @注文書印刷情報取得処理
    * @remarks
    * @タブレットからは、外部関数経由でこのメソッドをコール
    */
    function getOrderPrint() {
        return _xmlPrintOrderInfo;
    }
    // 2013/03/08 TCS 山田    【A.STEP2】新車タブレット受付画面の管理指標変更対応 END

    /**
    * @契約書印刷情報取得処理
    * @remarks
    * @タブレットからは、外部関数経由でこのメソッドをコール
    */
    function getContractPrint() {
        return _xmlprintContractInfo;
    }

    /**
    * @見積印刷更新処理
    *
    * @param {Integer} printResult 見積書印刷結果
    * @remarks
    * @タブレットからは、外部関数経由でこのメソッドをコール
    */
    function updateEstimatePrint(printResult) {
        //印刷結果を判別
        if (C_RESULT_PRINT_SUCCESS == printResult) {

            // 2013/07/11 TCS 坪根 GL0895対応 START
//            //見積印刷日更新の為、サーバーへコールバック
//            callBack.doCallback({ EstimateId: parentParam.EstimateId,
//                PaymentKbn: parentParam.PaymentKbn,
//                Method: constants.updateEstimatePrintDate,
//                ShowDialogErrorId: 0,
//                ShowDialogErrorMessage: "",
//                ErrorLogValue: ""
//            }, clientCallBackEnd);

            // 正常情報を設定
            errMessage = "ErrorId= , " +
                         "ErrorMessage=" + C_RESULT_PRINT_SUCCESS_MESSAGE + ", " +
                         "printResult=" + printResult;

            //見積印刷日更新の為、サーバーへコールバック ※正常時のログも出力する
            callBack.doCallback({ EstimateId: parentParam.EstimateId,
                PaymentKbn: parentParam.PaymentKbn,
                Method: constants.updateEstimatePrintDate,
                ShowDialogErrorId: 0,
                ShowDialogErrorMessage: "",
                ErrorLogValue: errMessage
            }, clientCallBackEnd);
            // 2013/07/11 TCS 坪根 GL0895対応 END
        }
        else {
            var errMessage;

            // エラー情報を設定
            errMessage = "ErrorId=" + C_MESSAGE_ID_902 + ", " +
                         "ErrorMessage=" + $("#HdnMessage902").val() + ", " +
                         "printResult=" + printResult;

            // エラー情報をログ出力
            callBack.doCallback({ EstimateId: parentParam.EstimateId,
                PaymentKbn: parentParam.PaymentKbn,
                Method: constants.errorProc,
                ShowDialogErrorId: C_MESSAGE_ID_902,
                ShowDialogErrorMessage: $("#HdnMessage902").val(),
                ErrorLogValue: errMessage
            }, clientCallBackEnd);
        }
    }

    // 2013/03/08 TCS 山田    【A.STEP2】新車タブレット受付画面の管理指標変更対応 START
    /**
    * @注文印刷更新処理
    *
    * @param {Integer} printResult 注文書印刷結果
    * @remarks
    * @タブレットからは、外部関数経由でこのメソッドをコール
    */
    function updateOrderPrint(printResult) {
        //印刷結果を判別
        if (C_RESULT_PRINT_SUCCESS == printResult) {
            // 2013/07/11 TCS 坪根 GL0895対応 START
//            // 見積作成画面をリロード
//            refreshParentPage(constants.orderUpdateContractPrintFlg);

            // 正常情報を設定
            errMessage = "ErrorId= , " +
                         "ErrorMessage=" + C_RESULT_PRINT_SUCCESS_MESSAGE + ", " +
                         "printResult=" + printResult;

            // エラー情報をログ出力
            callBack.doCallback({ EstimateId: parentParam.EstimateId,
                PaymentKbn: parentParam.PaymentKbn,
                Method: constants.successOrderPrint,
                ShowDialogErrorId: 0,
                ShowDialogErrorMessage: "",
                ErrorLogValue: errMessage
            }, clientCallBackEnd);
            // 2013/07/11 TCS 坪根 GL0895対応 END
        }
        else {
            var errMessage;

            // エラー内容を設定
            errMessage = "ErrorId=" + C_MESSAGE_ID_906 + ", " +
                         "ErrorMessage=" + $("#HdnMessage906").val() + ", " +
                         "printResult=" + printResult;

            // エラー情報をログ出力
            callBack.doCallback({ EstimateId: parentParam.EstimateId,
                PaymentKbn: parentParam.PaymentKbn,
                Method: constants.errorProc,
                ShowDialogErrorId: C_MESSAGE_ID_906,
                ShowDialogErrorMessage: $("#HdnMessage906").val(),
                ErrorLogValue: errMessage
            }, clientCallBackEnd);
        }
    }
    // 2013/03/08 TCS 山田    【A.STEP2】新車タブレット受付画面の管理指標変更対応 END

    /**
    * @契約印刷更新処理
    *
    * @param {Integer} printResult 契約書印刷結果
    * @remarks
    * @タブレットからは、外部関数経由でこのメソッドをコール
    */
    function updateContractPrint(printResult) {
        //印刷結果を判別
        if (C_RESULT_PRINT_SUCCESS == printResult) {
            // 2013/01/16 TCS 上田 GTMC121228118対応 START
            ////契約書印刷フラグ更新の為、サーバーへコールバック
            //callBack.doCallback({ EstimateId: parentParam.EstimateId,
            //    PaymentKbn: parentParam.PaymentKbn,
            //    Method: constants.updateContractPrintFlg,
            //    ShowDialogErrorId: 0,
            //    ShowDialogErrorMessage: "",
            //    ErrorLogValue: ""
            //}, clientCallBackEnd);
            
            // 2013/07/11 TCS 坪根 GL0895対応 START
            //// 見積作成画面をリロード
            //refreshParentPage(constants.updateContractPrintFlg);
            
            // 正常情報を設定
            errMessage = "ErrorId= , " +
                         "ErrorMessage=" + C_RESULT_PRINT_SUCCESS_MESSAGE + ", " +
                         "printResult=" + printResult;

            // エラー情報をログ出力
            callBack.doCallback({ EstimateId: parentParam.EstimateId,
                PaymentKbn: parentParam.PaymentKbn,
                Method: constants.successContractPrint,
                ShowDialogErrorId: 0,
                ShowDialogErrorMessage: "",
                ErrorLogValue: errMessage
            }, clientCallBackEnd);
            // 2013/07/11 TCS 坪根 GL0895対応 END

            // 2013/01/16 TCS 上田 GTMC121228118対応 END
        }
        else {
            var errMessage;

            // エラー内容を設定
            errMessage = "ErrorId=" + C_MESSAGE_ID_903 + ", " +
                         "ErrorMessage=" + $("#HdnMessage903").val() + ", " +
                         "printResult=" + printResult;

            // エラー情報をログ出力
            callBack.doCallback({ EstimateId: parentParam.EstimateId,
                PaymentKbn: parentParam.PaymentKbn,
                Method: constants.errorProc,
                ShowDialogErrorId: C_MESSAGE_ID_903,
                ShowDialogErrorMessage: $("#HdnMessage903").val(),
                ErrorLogValue: errMessage
            }, clientCallBackEnd);
        }
    }

    // 2013/01/16 TCS 上田 GTMC121228118対応 START
    /**
    * @見積作成画面をリロードする
    */
    function refreshParentPage(retButtonIndex) {
        // クルクルを閉じる
        closeLoading();

        // 見積作成画面のリロードを使って、画面リロード
        if (retButtonIndex != "") {
            // リロード
            refreshPage(retButtonIndex);

            // リロード後は、当画面は不要になるので、手動で当画面をクローズ
            $(".SC3070204header-left").click();
        }
    }
    // 2013/01/16 TCS 上田 GTMC121228118対応 END

    // 2013/11/27 TCS 高橋 Aカード情報相互連携開発 START
    /******************************************************************************
    ボタン制御処理
    ******************************************************************************/
    /**
    * @ボタン表示設定
    *
    * @param {String} aContractPrintFlg 契約書印刷フラグ
    * @param {String} aContractStatusFlg 契約状況フラグ
    * @param {String} aFllwupBoxSeqNo Follow-up Box内連番
    */
    function setButtonVisible(aContractPrintFlg, aContractStatusFlg, aFllwupBoxSeqNo) {

        // 見積書印刷ボタン表示フラグ
        var estimatePrintButtonViewFlg = false;
        // 2013/03/08 TCS 山田    【A.STEP2】新車タブレット受付画面の管理指標変更対応 START
        // 注文書印刷ボタン表示フラグ
        var orderPrintButtonViewFlg = false;
        // 2013/03/08 TCS 山田    【A.STEP2】新車タブレット受付画面の管理指標変更対応 END
        // 契約書印刷ボタン表示フラグ
        var contractPrintButtonViewFlg = false;

        // ▼見積書印刷ボタン▼
        estimatePrintButtonViewFlg = true;

        // 2013/03/08 TCS 山田    【A.STEP2】新車タブレット受付画面の管理指標変更対応 START
        // ▼注文書印刷ボタン▼
        // 活動があるか判別
        // 2013/11/27 TCS 高橋 Aカード情報相互連携開発 START
        if (aFllwupBoxSeqNo != "" && parentParam.ContractApprovalStatus == "2") {
            // 2013/11/27 TCS 高橋 Aカード情報相互連携開発 END
            orderPrintButtonViewFlg = true;
        }
        // 2013/03/08 TCS 山田    【A.STEP2】新車タブレット受付画面の管理指標変更対応 END

        // ▼契約書印刷ボタン▼
        // 活動があるか判別
        // 2013/11/27 TCS 高橋 Aカード情報相互連携開発 START
        if (aFllwupBoxSeqNo != "" && parentParam.ContractApprovalStatus == "2") {
            // 2013/11/27 TCS 高橋 Aカード情報相互連携開発 END
            contractPrintButtonViewFlg = true;
        }

        // 見積書印刷ボタン
        if (estimatePrintButtonViewFlg) {
            // ボタン表示
            $("#EstimatePrintButton").css("display", "block");
            // クリックイベント追加
            $("#EstimatePrintButton").unbind("click", estimatePrintClick);
            $("#EstimatePrintButton").bind("click", estimatePrintClick);
        }
        else {
            // ボタン非表示
            $("#EstimatePrintButton").css("display", "none");
        }

        // 2013/03/08 TCS 山田    【A.STEP2】新車タブレット受付画面の管理指標変更対応 START
        // 注文書印刷ボタン
        if (orderPrintButtonViewFlg) {
            // ボタン表示
            $("#OrderPrintButton").css("display", "block");
            // クリックイベント追加
            $("#OrderPrintButton").unbind("click", orderPrintClick);
            $("#OrderPrintButton").bind("click", orderPrintClick);
        }
        else {
            // ボタン非表示
            $("#OrderPrintButton").css("display", "none");
        }
        // 2013/03/08 TCS 山田    【A.STEP2】新車タブレット受付画面の管理指標変更対応 END

        // 契約書印刷ボタン
        if (contractPrintButtonViewFlg) {
            // ボタン表示
            $("#ContractPrintButton").css("display", "block");
            // クリックイベント追加
            $("#ContractPrintButton").unbind("click", contectPrintClick);
            $("#ContractPrintButton").bind("click", contectPrintClick);
        }
        else {
            // ボタン非表示
            $("#ContractPrintButton").css("display", "none");
        }
    }

    /**
    * @エラー処理
    *
    * @param {Integer} aErrMsgId エラーメッセージID
    * @param {String} aErrMsg エラー内容
    */
    function errorProc(aErrMsgId, aErrMsg) {
        var strMessage = SC3070204HtmlDecode(aErrMsg);

        // エラーが発生した場合は、エラー表示
        icropScript.ShowMessageBox(aErrMsgId, strMessage, "");
        // クルクルを閉じる
        closeLoading();
        // 当画面を閉じる
        $(".SC3070204header-left").click();
    }

    /******************************************************************************
    初回時の処理
    ※見積入力画面からコールされて、ポップオーバーの属性を付加
    ******************************************************************************/
    function create(triggerButton) {
        /**
        * @ポップオーバー初期化
        */
        $(triggerButton).TCSpopover({
            openEvent: function (pop, elem) {
                _popForm = pop;
                initialize();
            },
            header: '#SC3070204PopOverHeader',
            content: '#SC3070204PopOverContent',
            preventLeft: true,
            preventRight: true,
            preventTop: false,
            preventBottom: true,
            id: "SC3070204PopOver"
        });

        $(".SC3070204header-left").bind("click", function () {
            $(triggerButton).trigger('hidePopover');
        });
    }

    return {
        /**
        * @コールバック関数定義
        */
        callBack: callBack,

        /**
        * @画面初期化処理
        */
        create: create,

        /**
        * @パラメータ設定
        */
        setParams: function (params) {
            $.extend(parentParam, params);
        },

        /**
        * @見積書印刷情報取得処理
        */
        getEstimatePrint: getEstimatePrint,

        // 2013/03/08 TCS 山田    【A.STEP2】新車タブレット受付画面の管理指標変更対応 START
        /**
        * @注文書印刷情報取得処理
        */
        getOrderPrint: getOrderPrint,
        // 2013/03/08 TCS 山田    【A.STEP2】新車タブレット受付画面の管理指標変更対応 END

        /**
        * @契約書印刷情報取得処理
        */
        getContractPrint: getContractPrint,

        /**
        * @見積印刷更新処理
        */
        updateEstimatePrint: updateEstimatePrint,

        // 2013/03/08 TCS 山田    【A.STEP2】新車タブレット受付画面の管理指標変更対応 START
        /**
        * @注文印刷更新処理
        */
        updateOrderPrint: updateOrderPrint,
        // 2013/03/08 TCS 山田    【A.STEP2】新車タブレット受付画面の管理指標変更対応 END

        /**
        * @契約印刷更新処理
        */
        updateContractPrint: updateContractPrint
    }
} ();

/**
* @メソッド名を送信する為のURLスキーム取得
*
* @param {String} sendPrintMethodName 印刷情報送信処理メソッド名
* @param {String} getPrintMethodName 印刷結果取得処理メソッド名
* @return {String} URLスキーム
*/
function getUrlSchemeMethod(sendPrintMethodName, getPrintMethodName) {
    var urlTabret;

    urlTabret = "icrop:httpreq?paramMethod=" + sendPrintMethodName + "&resultMethod=" + getPrintMethodName;
    return urlTabret;
}

/**
* @デコード処理
*
* @param {String} strValue デコードする値
* @return {String} デコードした値
*/
function SC3070204HtmlDecode(strValue) {
    var strDecodeValue;

    strDecodeValue = decodeURIComponent(strValue);
    return strDecodeValue;
}

/******************************************************************************
タブレット基盤呼び出し用関数
******************************************************************************/
/**
* @見積書印刷情報送信処理
*/
function sendEstimatePrintInfo() {
    //見積書印刷情報取得
    var xmlEstimatePrint = sc3070204Script.getEstimatePrint();
    //見積書印刷情報をタブレット基盤へ返却
    return xmlEstimatePrint;
}

// 2013/03/08 TCS 山田    【A.STEP2】新車タブレット受付画面の管理指標変更対応 START
/**
* @注文書印刷情報送信処理
*/
function sendOrderPrintInfo() {
    //注文書印刷情報取得
    var xmlEstimatePrint = sc3070204Script.getOrderPrint();
    //注文書印刷情報をタブレット基盤へ返却
    return xmlEstimatePrint;
}
// 2013/03/08 TCS 山田    【A.STEP2】新車タブレット受付画面の管理指標変更対応 END

/**
* @契約書印刷情報送信処理
*/
function sendContractPrintInfo() {
    //契約書印刷情報取得
    var xmlEstimatePrint = sc3070204Script.getContractPrint();
    //契約書印刷情報をタブレット基盤へ返却
    return xmlEstimatePrint;
}

/**
* @見積書印刷結果取得処理
*
* @param {Integer} printResult 見積書印刷結果
*/
function getResEstimatePrint(printResult) {
    //見積更新処理
    sc3070204Script.updateEstimatePrint(printResult);
}

// 2013/03/08 TCS 山田    【A.STEP2】新車タブレット受付画面の管理指標変更対応 START
/**
* @注文書印刷結果取得処理
*
* @param {Integer} printResult 注文書印刷結果
*/
function getResOrderPrint(printResult) {
    //注文更新処理
    sc3070204Script.updateOrderPrint(printResult);
}
// 2013/03/08 TCS 山田    【A.STEP2】新車タブレット受付画面の管理指標変更対応 END

/**
* @契約書印刷結果取得処理
*
* @param {Integer} printResult 契約書印刷結果
*/
function getResContractPrint(printResult) {
    //契約更新処理
    sc3070204Script.updateContractPrint(printResult);
}