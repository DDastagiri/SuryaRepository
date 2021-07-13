

var sc3070203Script;

sc3070203Script = function () {
    /**
    * @class 定数
    */
    var constants = {
        init: "CreateWindow",
        request: "InsertInfo",
        cancel: "CancelInfo"
    }

    /**
    * @価格相談画面
    */
    var popForm;

    /**
    * @パラメータ情報(見積作成画面の情報)
    */
    var parentScreenInfo = {
        Estimateid: ""
		, RequestPrice: ""
		, Customerid: ""
		, CustomerName: ""
		, CustomerClass: ""
		, CustomerKind: ""
		, FollowUpBoxStoreCode: ""
		, FollowUpBoxNumber: ""
		, VehicleSequenceNumber: ""
		, SalesStaffCode: ""
		, SeriesCode: ""
		, SeriesName: ""
		, ModelCode: ""
		, ModelName: ""
    };



    /**
    * @クライアントコールバック関数定義
    */
    var callBack = {
        doCallback: function (argument, callbackFunction) {
            showLoading();
            this.packedArgument = JSON.stringify(argument);
            this.endCallback = callbackFunction;
            this.beginCallback();
        }
    };

    function onChangeRequestPrice(num) {
        if (num == undefined) return;

        if (num.match(/^[0-9]{1,9}(\.[0-9]{1,2})?$/) || (num == "")) {
            var numberFormat = num;
            var numberFormatReturn = formatNumber(numberFormat);
            $(this).val(numberFormatReturn);
            $("#RequestPriceNewArea").text(numberFormatReturn);
            $("#RequestPriceNew").val(numberFormatReturn);
            changeRequestButtonStatus();
            $("#RequestPriceNewArea").data("isValid", true);
        } else {
            $("#RequestPriceNewArea").data("isValid", false);
        }
    }

    /**
    * @依頼ボタンの活性非活性制御
    */
    function changeRequestButtonStatus() {
        if (isEnableRequestButton()) {
            $('#SC3070203_RequestButton').removeClass('disabled');
        } else {
            $('#SC3070203_RequestButton').addClass('disabled');
        }
    }

    /**
    * @依頼ボタンの活性非活性判定
    */
    function isEnableRequestButton() {
        if ($("#SC3070203_SelectedManagerOnlineStatus").val() == "4") {
            return false;
        } else {
            return true;
        }
    }

    function setStaffMemo(memo) {
        $("#SC3070203_StaffMemo").val(memo);
        if (memo == undefined || memo == "") {
            if ($('#SC3070203_IsUnderRequest').val() != "True") {
                //コメントのプレースホルダ―を設定
                $('#SC3070203_StaffMemoDisplayArea').html("<span style='color:#BBB'>" + $('#SC3070203_CommentPlaceHolderWord').text() + "</span>");
            }
        } else {
            //コメントを改行付きで表示
            var memoHtml = $("<div />").text(memo).html().replace(/\n/g, "<br>");
            $('#SC3070203_StaffMemoDisplayArea').html(memoHtml);
        }
    }

    function initializeWindow(result, context) {
        //コールバックによって取得した価格相談のHTMLを格納
        var jsonResult = JSON.parse(result);
        var contents = $('<Div>').html(jsonResult.Contents).text();
        var SC3070203_Main = $(contents).find('#SC3070203_Main');
        var salesManagerList = $(contents).find('#SC3070203_SalesManagerList');
        var priceConsultationResonList = $(contents).find('#PriceConsultationResonList');
        var noSendAccountArea = $(contents).find('#SC3070203_NoSendAccountArea');
        $('#SC3070203_NoSendAccountArea>div').remove();
        noSendAccountArea.children('div').clone(true).appendTo('#SC3070203_NoSendAccountArea');

        //メイン画面のコンテンツを削除
        //1ページ目のコンテンツを設定
        $('#SC3070203_Main>div').remove();
        if ($('#SC3070203_IsExistManager').val() == "True") {
            SC3070203_Main.children('div').clone(true).appendTo('#SC3070203_Main');
            $("#SC3070203_RequestButton").appendTo("#SC3070203_ButtonArea");
            $("#SC3070203_CancelButton").appendTo("#SC3070203_ButtonArea");
        } else {
            //対応者いない画面を表示
            noSendAccountArea.children('div').clone(true).appendTo('#SC3070203_Main');
        }

        //相談先一覧のコンテンツを削除
        $('#SC3070203_SalesManagerList>div').remove();
        //相談先一覧のコンテンツを設定
        salesManagerList.children('div').clone(true).appendTo('#SC3070203_SalesManagerList');
        //値引き理由一覧のコンテンツを削除
        $('#PriceConsultationResonList>div').remove();
        //値引き理由一覧のコンテンツを設定
        priceConsultationResonList.children('div').clone(true).appendTo('#PriceConsultationResonList');

        //コメント欄のコンテンツを設定
        setStaffMemo($("#SC3070203_StaffMemo").val());

        $('#SC3070203_Main').fingerScroll();

        if ($('#SC3070203_IsUnderRequest').val() == "True") {

            return;
        }
        //テンキーコントロールの設定
        $("#RequestPriceNewArea").TCSNumericKeypad({
            maxDigits: 12,
            acceptDecimalPoint: true,
            defaultValue: "0",
            completionLabel: $("#SC3070203_NumericPadOk").text(),
            cancelLabel: $("#SC3070203_NumericPadCancel").text(),
            valueChanged: function (num) { onChangeRequestPrice(num); },
            parentPopover: $("#" + $("#SC3070203PopOverForm").data("TriggerClientID")),
            open: function () {
                var strDefValue = $("#RequestPriceNew").val();
                $(this).TCSNumericKeypad("setValue", strDefValue);
                $("#RequestPriceNewArea").data("isValid", true);
            },
            close: function () {
                if ($("#RequestPriceNewArea").data("isValid") == true) {
                    $("#RequestPriceNewArea").data("isValid", true);
                    return true;
                } else {
                    alert($("#ErrorMessageWord2").text());
                    $("#RequestPriceNewArea").data("isValid", true);
                    return false;
                }
            }
        });

        //コメント欄をクリックした時の動作を定義
        $('#SC3070203_StaffMemoArea').bind('click', function (e) {
            //２ページ目表示領域をクリアする
            $('#SC3070203_DisplayPage').find('div').remove();

            var staffMemoAreaTextFrame = $("<div id='SC3070203_StaffMemoAreaTextFrame'><textarea id='SC3070203_StaffMemoAreaText'></textarea></div>");
            var staffMemoAreaText = staffMemoAreaTextFrame.children('textarea');
            staffMemoAreaText
                .attr("maxlength", 128)
                .val($("#SC3070203_StaffMemo").val())
                .focusout(function () { $('#SC3070203_PopOverFormHeader .rewindButton').click(); });

            staffMemoAreaTextFrame.appendTo('#SC3070203_DisplayPage');

            //２ページ目に移動する
            popForm.pageIndex = 0;
            popForm.pushPage("SC3070203_StaffMemo");

            setTimeout(function () {
                staffMemoAreaText.focus();
            }, 1000);

            //ヘッダーの左ボタン（価格相談へ）の定義
            $('#SC3070203_PopOverFormHeader .icrop-PopOverForm-header-left')
                .unbind('click')
                .removeClass("icrop-PopOverForm-header-back")
                .empty()
                .html('<div class="rewindButton"><a href="#" class="useCut"></a><span class="tgLeft">&nbsp;</span></div>');
            $("#SC3070203_PopOverFormHeader .rewindButton>a").text($("#SC3070203_HeaderBack1").text());
            $('#SC3070203_HeaderTitle').text($("#SC3070203_HeaderTitleWord4").text());
            $('#SC3070203_PopOverFormHeader .rewindButton').click(function (e) {
                $('#SC3070203_PopOverFormHeader .icrop-PopOverForm-header-left').remove();
                $('#SC3070203_PopOverFormHeader .icrop-PopOverForm-header-right').remove();
                $('#SC3070203_HeaderTitle').text($("#SC3070203_HeaderTitleWord1").text());

                setStaffMemo(staffMemoAreaText.val());
                staffMemoAreaTextFrame.remove();

                popForm.popPage();
            });
        });

        //セールスマネージャー欄をクリックした時の動作を定義
        $('#SC3070203_SelectedSalesMangerArea').bind('click', function (e) {
            //２ページ目表示領域をクリアする
            $('#SC3070203_DisplayPage').find('div').remove();
            //相談先一覧のコンテンツを２ページ目にコピー
            $('#SC3070203_SalesManagerList>div').clone(true).appendTo('#SC3070203_DisplayPage');

            $('#SC3070203_DisplayPage').fingerScroll();

            //２ページ目に移動する
            popForm.pageIndex = 0;
            popForm.pushPage();

            //ヘッダーの左ボタン（価格相談へ）の定義
            $('#SC3070203_PopOverFormHeader .icrop-PopOverForm-header-left')
                .unbind('click')
                .removeClass("icrop-PopOverForm-header-back")
                .empty()
                .html('<div class="rewindButton"><a href="#" class="useCut"></a><span class="tgLeft">&nbsp;</span></div>');
            $("#SC3070203_PopOverFormHeader .rewindButton>a").text($("#SC3070203_HeaderBack1").text());
            $('#SC3070203_HeaderTitle').text($("#SC3070203_HeaderTitleWord2").text());
            $('#SC3070203_PopOverFormHeader .rewindButton').click(function (e) {
                $('#SC3070203_PopOverFormHeader .icrop-PopOverForm-header-left').remove();
                $('#SC3070203_PopOverFormHeader .icrop-PopOverForm-header-right').remove();
                $('#SC3070203_HeaderTitle').text($("#SC3070203_HeaderTitleWord1").text());
                popForm.popPage();
            });

            //セールスマネージャーを選択した時の動作を定義
            $('#SC3070203_SalesMangerRow.Online').unbind('click');
            $('#SC3070203_SalesMangerRow.Online').bind('click', function (e) {
                //すべてのチェックマークを削除する
                $("#SC3070203_DisplayPage #SC3070203_SalesMangerRow").removeClass('Check');
                //選択された行にチェックマークを設定する
                $(this).addClass('Check');
                //価格相談画面メインに選択されたセールスマネージャーを反映
                //2013/10/03 TCS 市川【次世代e-CRBタブレット iOS7.0 VersionUp対応（セールス）】MOD START
                //iOS7対応：同一IDが複数ある場合の.FINDメソッド不具合回避のためコントロールIDによる制御からclass名による制御へ変更
                $('#SC3070203_SelectedSalesMangerNameArea').text($(this).find('.SalesMangerName').val());
                $('#SC3070203_SelectedSalesMangerName').val($(this).find('.SalesMangerName').val());
                $('#SC3070203_SelectedManagerAccount').val($(this).find('.SalesMangerAccount').val());
                $('#SC3070203_SelectedManagerOnlineStatus').val($(this).find('.OnlineStatus').val());
                //2013/10/03 TCS 市川【次世代e-CRBタブレット iOS7.0 VersionUp対応（セールス）】MOD END

                //ヘッダーボタンの削除（POPする度にボタンが追加されてしまうため）
                $('#SC3070203_PopOverFormHeader .icrop-PopOverForm-header-left').remove();
                $('#SC3070203_PopOverFormHeader .icrop-PopOverForm-header-right').remove();
                $('#SC3070203_HeaderTitle').text($("#SC3070203_HeaderTitleWord1").text());
                //依頼ボタンの活性非活性制御
                changeRequestButtonStatus();
                popForm.popPage();

                //相談先一覧のコンテンツを削除
                $('#SC3070203_SalesManagerList').find('div').remove();
                //表示領域の内容を相談先一覧にコピー
                $('#SC3070203_DisplayPage>div').clone(true).appendTo('#SC3070203_SalesManagerList');
            });
        });

        //値引き理由欄をクリックした時の動作を定義
        $('#SC3070203_SelectedReasonArea')
            .unbind('click')
            .bind('click', function (e) {
                $('#SC3070203_DisplayPage>div').remove();
                $('#PriceConsultationResonList>div').clone(true).appendTo('#SC3070203_DisplayPage');

                popForm.pageIndex = 0;
                popForm.pushPage();

                $('#SC3070203_DisplayPage').fingerScroll();
                $('#SC3070203_PopOverFormHeader .icrop-PopOverForm-header-left')
                    .unbind('click')
                    .removeClass("icrop-PopOverForm-header-back")
                    .empty()
                    .html('<div class="rewindButton"><a href="#"  class="useCut"></a><span class="tgLeft">&nbsp;</span></div>');
                $("#SC3070203_PopOverFormHeader .rewindButton>a").text($("#SC3070203_HeaderBack1").text());
                $('#SC3070203_HeaderTitle').text($("#SC3070203_HeaderTitleWord3").text());
                $('#SC3070203_PopOverFormHeader .rewindButton').click(function (e) {
                    $('#SC3070203_PopOverFormHeader .icrop-PopOverForm-header-left').remove();
                    $('#SC3070203_PopOverFormHeader .icrop-PopOverForm-header-right').remove();
                    $('#SC3070203_HeaderTitle').text($("#SC3070203_HeaderTitleWord1").text());
                    popForm.popPage();
                });

                $('#SC3070203_DisplayPage #PriceConsultationResonRow')
                    .unbind('click')
                    .bind('click', function (e) {
                        $('#DisplayPage #PriceConsultationResonRow').removeClass('Check');
                        $(this).addClass('Check');
                        //2013/10/02 TCS 市川【次世代e-CRBタブレット iOS7.0 VersionUp対応（セールス）】MOD START
                        //iOS7対応：同一IDが複数ある場合の.FINDメソッド不具合回避のためコントロールIDによる制御からclass名による制御へ変更
                        $('#SC3070203_SelectedResonNameArea').text($(this).find('.ResonName').val());
                        $('#SC3070203_SelectedResonName').val($(this).find('.ResonName').val());
                        $('#SC3070203_SelectedResonid').val($(this).find('.Resonid').val());
                        //2013/10/02 TCS 市川【次世代e-CRBタブレット iOS7.0 VersionUp対応（セールス）】MOD END
                        $('#SC3070203_PopOverFormHeader .icrop-PopOverForm-header-left').remove();
                        $('#SC3070203_PopOverFormHeader .icrop-PopOverForm-header-right').remove();
                        $('#SC3070203_HeaderTitle').text($("#SC3070203_HeaderTitleWord1").text());
                        popForm.popPage();

                        $('#PriceConsultationResonList').find('div').remove();
                        $('#SC3070203_DisplayPage>div').clone(true).appendTo('#PriceConsultationResonList');
                    });
            });


    }

    function createCallBackParam(options) {
        var prms = $.extend(parentScreenInfo, createScreenParam(), options)

        return prms;
    }

    function createScreenParam() {
        return {
            ManagerAccount: $("#SC3070203_SelectedManagerAccount").val()
		    , ManagerName: $("#SC3070203_SelectedSalesMangerName").val()
		    , Reasonid: parseInt($("#SC3070203_SelectedResonid").val())
		    , NoticeRequestid: parseInt($("#SC3070203_NoticeRequestid").val())
            , RequestPrice: parseFloat($('#RequestPriceNew').val())
            , RequestStaffMemo: $('#SC3070203_StaffMemo').val()
        }
    }

    function createWindow(options) {
        var prms = createCallBackParam($.extend({ Method: constants.init }, options));

        //メイン画面のコンテンツを削除
        $('#SC3070203_Main>div').remove();

        //画面初期化情報を取得する
        callBack.doCallback(prms, clientCallBack);
    }

    function InsertInfo() {
        var prms = createCallBackParam({ Method: constants.request });
        callBack.doCallback(prms, clientCallBack);
    }

    function CancelInfo() {
        var prms = createCallBackParam({ Method: constants.cancel });
        callBack.doCallback(prms, clientCallBack);
    }

    /**
    * @クライアントコールバック
    */
    function clientCallBack(result, context) {
        var jsonResult = JSON.parse(result);
        if (jsonResult.ResultCode != 0) {
            alert(jsonResult.Message);
            closeLoading();
            $("#SC3070203_RequestButton").removeClass("disabled");
            $("#SC3070203_CancelButton").removeClass("disabled");
            return;
        }

        $("#SC3070203_RequestButton").remove();
        $("#SC3070203_CancelButton").remove();

        if (jsonResult.Caller == constants.init) {
            initializeWindow(result, context);
        } else {
            SC3070210.reload();
            $("#bodyFrame").trigger("hideOpenPopover");
        }

        closeLoading();
    }


    /******************************************************************************
    処理中アニメーション
    ******************************************************************************/
    /**
    * 読み込み中アイコン表示
    */
    function showLoading() {

        //オーバーレイ表示
        $("#SC3070203_registOverlayBlack").css("display", "block");
        //アニメーション
        setTimeout(function () {
            $("#SC3070203_processingServer").addClass("show");
            $("#SC3070203_registOverlayBlack").addClass("open");
        }, 0);

    }

    /**
    * 読み込み中アイコンを非表示にする
    */
    function closeLoading() {
        $("#SC3070203_processingServer").removeClass("show");
        //2014/11/04 TCS 藤井 iOS8 対応(i-CROP_V4_salesよりマージ)  Start
        $("#SC3070203_registOverlayBlack").removeClass("open");
        setTimeout(function () {
            $("#SC3070203_registOverlayBlack").css("display", "none");
        }, 300);
        //2014/11/04 TCS 藤井 iOS8 対応(i-CROP_V4_salesよりマージ)  End
    }

    return {
        /**
        * @コールバック関数定義
        */
        callBack: callBack,

        /**
        * @画面初期化処理
        */
        init: function () {
            /**
            * @ポップオーバー初期化
            */
            $("#SC3070203PopOverForm").TCSPopOverForm({
                open: function (pop, elem) {
                    popForm = pop;
                    $('#SC3070203_Main>div').remove();
                    createWindow({ RequestPrice: parseFloat(parentScreenInfo.RequestPrice),
                        //2015/03/11 TCS 鈴木【TMT課題対応(#72 セールスタブレットの価格相談履歴表示)】ADD START
                        RequestStaffMemo: parentScreenInfo.RequestStaffMemo
                        //2015/03/11 TCS 鈴木【TMT課題対応(#72 セールスタブレットの価格相談履歴表示)】ADD END
                    });
                },
                render: function (pop, index, args, container, header) {

                    if (index == 0) {
                        //ヘッダーのキャンセルボタンを定義
                        $("#SC3070203_PopOverFormHeader .icrop-PopOverForm-header-left").empty().html('<a href="#" class="nscPopUpCancelButton useCut"></a>')
                        $("#SC3070203_PopOverFormHeader .icrop-PopOverForm-header-left>a").text($("#SC3070203_HeaderCancelWord").text());
                        $("#SC3070203_HeaderTitle").text($("#SC3070203_HeaderTitleWord1").text());
                        $("#SC3070203_PopOverFormHeader .nscPopUpCancelButton").bind("click", function (e) {
                            pop.closePopOver();
                        });
                    }
                },
                preventLeft: true,
                preventRight: true,
                preventTop: false,
                preventBottom: true,
                elasticConstant: 0.3,
                id: "SC3070203PopOver"
            });

            /**
            * @依頼ボタン押下
            */
            $('#SC3070203_RequestButton').live("click", function () {
                if (isEnableRequestButton() == false) return;
                $('#SC3070203_RequestButton').addClass('disabled');
                InsertInfo();
            });

            /**
            * @キャンセルボタン押下
            */
            $('#SC3070203_CancelButton').live("click", function () {
                $('#SC3070203_CancelButton').addClass('disabled');
                CancelInfo();
            });
        },


        setParams: function (params) {
            $.extend(parentScreenInfo, params)
        },

        onChangeRequestPrice: onChangeRequestPrice()

    }
} ();







$(function () {
	sc3070203Script.init();
});



