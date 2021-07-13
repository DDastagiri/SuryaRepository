

var sc3070208Script;

sc3070208Script = function () {
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

    /**
    * @依頼ボタンの活性非活性制御
    */
    function changeRequestButtonStatus() {
        if (isEnableRequestButton()) {
            $('#SC3070208_RequestButton').removeClass('disabled');
        } else {
            $('#SC3070208_RequestButton').addClass('disabled');
        }
    }

    /**
    * @依頼ボタンの活性非活性判定
    */
    function isEnableRequestButton() {
        if ($("#SC3070208_SelectedManagerOnlineStatus").val() == "4") {
            return false;
        } else {
            return true;
        }
    }

    function setStaffMemo(memo) {
        $("#SC3070208_StaffMemo").val(memo);
        if (memo == undefined || memo == "") {
            if ($('#SC3070208_IsUnderRequest').val() != "True") {
                //コメントのプレースホルダ―を設定
                $('#SC3070208_StaffMemoDisplayArea').html("<span style='color:#BBB'>" + $('#SC3070208_CommentPlaceHolderWord').text() + "</span>");
            }
        } else {
            //コメントを改行付きで表示
            var memoHtml = $("<div />").text(memo).html().replace(/\n/g, "<br>");
            $('#SC3070208_StaffMemoDisplayArea').html(memoHtml);
        }
    }

    function initializeWindow(result, context) {
        //コールバックによって取得した価格相談のHTMLを格納
        var jsonResult = JSON.parse(result);
        var contents = $('<Div>').html(jsonResult.Contents).text();
        var SC3070208_Main = $(contents).find('#SC3070208_Main');
        var salesManagerList = $(contents).find('#SC3070208_SalesManagerList');
        var noSendAccountArea = $(contents).find('#SC3070208_NoSendAccountArea');
        $('#SC3070208_NoSendAccountArea>div').remove();
        noSendAccountArea.children('div').clone(true).appendTo('#SC3070208_NoSendAccountArea');

        //メイン画面のコンテンツを削除
        //1ページ目のコンテンツを設定
        $('#SC3070208_Main>div').remove();
        if ($('#SC3070208_IsExistManager').val() == "True") {
            SC3070208_Main.children('div').clone(true).appendTo('#SC3070208_Main');
            $("#SC3070208_RequestButton").appendTo("#SC3070208_ButtonArea");
            $("#SC3070208_CancelButton").appendTo("#SC3070208_ButtonArea");
        } else {
            //対応者いない画面を表示
            noSendAccountArea.children('div').clone(true).appendTo('#SC3070208_Main');
        }

        //相談先一覧のコンテンツを削除
        $('#SC3070208_SalesManagerList>div').remove();
        //相談先一覧のコンテンツを設定
        salesManagerList.children('div').clone(true).appendTo('#SC3070208_SalesManagerList');

        //コメント欄のコンテンツを設定
        setStaffMemo($("#SC3070208_StaffMemo").val());

        $('#SC3070208_Main').fingerScroll();

        if ($('#SC3070208_IsUnderRequest').val() == "True") {
            return;
        }

        //コメント欄をクリックした時の動作を定義
        $('#SC3070208_StaffMemoArea').bind('click', function (e) {
            //２ページ目表示領域をクリアする
            $('#SC3070208_DisplayPage').find('div').remove();

            var staffMemoAreaTextFrame = $("<div id='SC3070208_StaffMemoAreaTextFrame'><textarea id='SC3070208_StaffMemoAreaText'></textarea></div>");
            var staffMemoAreaText = staffMemoAreaTextFrame.children('textarea');
            staffMemoAreaText
                .attr("maxlength", 128)
                .val($("#SC3070208_StaffMemo").val())
                .focusout(function () { $('#SC3070208_PopOverFormHeader .rewindButton').click(); });

            staffMemoAreaTextFrame.appendTo('#SC3070208_DisplayPage');

            //２ページ目に移動する
            popForm.pageIndex = 0;
            popForm.pushPage("SC3070208_StaffMemo");

            setTimeout(function () {
                staffMemoAreaText.focus();
            }, 1000);

            //ヘッダーの左ボタン（価格相談へ）の定義
            $('#SC3070208_PopOverFormHeader .icrop-PopOverForm-header-left')
                .unbind('click')
                .removeClass("icrop-PopOverForm-header-back")
                .empty()
                .html('<div class="rewindButton"><a href="#" class="useCut"></a><span class="tgLeft">&nbsp;</span></div>');
            $("#SC3070208_PopOverFormHeader .rewindButton>a").text($("#SC3070208_HeaderBack1").text());
            $('#SC3070208_HeaderTitle').text($("#SC3070208_HeaderTitleWord4").text());
            $('#SC3070208_PopOverFormHeader .rewindButton').click(function (e) {
                $('#SC3070208_PopOverFormHeader .icrop-PopOverForm-header-left').remove();
                $('#SC3070208_PopOverFormHeader .icrop-PopOverForm-header-right').remove();
                $('#SC3070208_HeaderTitle').text($("#SC3070208_HeaderTitleWord1").text());

                setStaffMemo(staffMemoAreaText.val());
                staffMemoAreaTextFrame.remove();

                popForm.popPage();
            });
        });

        //セールスマネージャー欄をクリックした時の動作を定義
        $('#SC3070208_SelectedSalesMangerArea').bind('click', function (e) {
            //２ページ目表示領域をクリアする
            $('#SC3070208_DisplayPage').find('div').remove();
            //相談先一覧のコンテンツを２ページ目にコピー
            $('#SC3070208_SalesManagerList>div').clone(true).appendTo('#SC3070208_DisplayPage');

            $('#SC3070208_DisplayPage').fingerScroll();

            //２ページ目に移動する
            popForm.pageIndex = 0;
            popForm.pushPage();

            //ヘッダーの左ボタン（価格相談へ）の定義
            $('#SC3070208_PopOverFormHeader .icrop-PopOverForm-header-left')
                .unbind('click')
                .removeClass("icrop-PopOverForm-header-back")
                .empty()
                .html('<div class="rewindButton"><a href="#" class="useCut"></a><span class="tgLeft">&nbsp;</span></div>');
            $("#SC3070208_PopOverFormHeader .rewindButton>a").text($("#SC3070208_HeaderBack1").text());
            $('#SC3070208_HeaderTitle').text($("#SC3070208_HeaderTitleWord2").text());
            $('#SC3070208_PopOverFormHeader .rewindButton').click(function (e) {
                $('#SC3070208_PopOverFormHeader .icrop-PopOverForm-header-left').remove();
                $('#SC3070208_PopOverFormHeader .icrop-PopOverForm-header-right').remove();
                $('#SC3070208_HeaderTitle').text($("#SC3070208_HeaderTitleWord1").text());
                popForm.popPage();
            });

            //セールスマネージャーを選択した時の動作を定義
            $('#SC3070208_ApprovalStaffRow.Online').unbind('click');
            $('#SC3070208_ApprovalStaffRow.Online').bind('click', function (e) {
                //すべてのチェックマークを削除する
                $("#SC3070208_DisplayPage #SC3070208_ApprovalStaffRow").removeClass('Check');
                //選択された行にチェックマークを設定する
                $(this).addClass('Check');
                //価格相談画面メインに選択されたセールスマネージャーを反映
                //2013/10/03 TCS 市川【次世代e-CRBタブレット iOS7.0 VersionUp対応（セールス）】MOD START
                //iOS7対応：同一IDが複数ある場合の.FINDメソッド不具合回避のためコントロールIDによる制御からclass名による制御へ変更
                if ($(this).find('.SalesMangerAccount').val() == $("#SC3070208_SelfAccount").text()) {
                    $('#SC3070208_SelectedSalesMangerNameArea').text($("#SC3070208_SelfWord").text());
                } else {
                    $('#SC3070208_SelectedSalesMangerNameArea').text($(this).find('.SalesMangerName').val());
                }
                $('#SC3070208_SelectedSalesMangerName').val($(this).find('.SalesMangerName').val());
                $('#SC3070208_SelectedManagerAccount').val($(this).find('.SalesMangerAccount').val());
                $('#SC3070208_SelectedManagerOnlineStatus').val($(this).find('.OnlineStatus').val());
                //2013/10/03 TCS 市川【次世代e-CRBタブレット iOS7.0 VersionUp対応（セールス）】MOD END

                //ヘッダーボタンの削除（POPする度にボタンが追加されてしまうため）
                $('#SC3070208_PopOverFormHeader .icrop-PopOverForm-header-left').remove();
                $('#SC3070208_PopOverFormHeader .icrop-PopOverForm-header-right').remove();
                $('#SC3070208_HeaderTitle').text($("#SC3070208_HeaderTitleWord1").text());
                //依頼ボタンの活性非活性制御
                changeRequestButtonStatus();
                popForm.popPage();

                //相談先一覧のコンテンツを削除
                $('#SC3070208_SalesManagerList').find('div').remove();
                //表示領域の内容を相談先一覧にコピー
                $('#SC3070208_DisplayPage>div').clone(true).appendTo('#SC3070208_SalesManagerList');
            });
        });
    }

    function createCallBackParam(options) {
        var prms = $.extend(parentScreenInfo, createScreenParam(), options)

        return prms;
    }

    function createScreenParam() {
        return {
            ManagerAccount: $("#SC3070208_SelectedManagerAccount").val()
		    , ManagerName: $("#SC3070208_SelectedSalesMangerName").val()
		    , NoticeRequestid: parseInt($("#SC3070208_NoticeRequestid").val())
            , RequestStaffMemo: $('#SC3070208_StaffMemo').val()
        }
    }

    function createWindow(options) {
        var prms = createCallBackParam($.extend({ Method: constants.init }, options));

        //メイン画面のコンテンツを削除
        $('#SC3070208_Main>div').remove();

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
        if (jsonResult.ResultCode < 0) {
            alert(jsonResult.Message);
            closeLoading();
            $("#SC3070208_RequestButton").removeClass("disabled");
            $("#SC3070208_CancelButton").removeClass("disabled");
            return;
        }

        $("#SC3070208_RequestButton").remove();
        $("#SC3070208_CancelButton").remove();

        if (jsonResult.Caller == constants.init) {
            initializeWindow(result, context);
            closeLoading();
        } else {
            //追加： 2014/05/28 TCS 安田 受注時説明機能開発（受注後工程スケジュール） START
            if (jsonResult.Caller == constants.request && jsonResult.ResultCode == 100) {
                $('#OrderAfterFlgHiddenField').val(jsonResult.Info);
                $('#OrderAfterButton').click();
                alert(jsonResult.Message);
                closeLoading();
            }
            //追加： 2014/05/28 TCS 安田 受注時説明機能開発（受注後工程スケジュール） END

            //画面再表示
            this_form.submit();
        }
    }


    /******************************************************************************
    処理中アニメーション
    ******************************************************************************/
    /**
    * 読み込み中アイコン表示
    */
    function showLoading() {
        $("#SC3070208_registOverlayBlack").css("display", "block");
        setTimeout(function () {
            $("#SC3070208_processingServer").addClass("show");
            $("#SC3070208_registOverlayBlack").addClass("open");
        }, 0);
    }

    /**
    * 読み込み中アイコンを非表示にする
    */
    function closeLoading() {
        $("#SC3070208_registOverlayBlack").removeClass("open");
        setTimeout(function () {
            $("#SC3070208_processingServer").removeClass("show");
            $("#SC3070208_registOverlayBlack").css("display", "none");
        }, 300);
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
            $("#SC3070208_PopOverForm").TCSPopOverForm({
                open: function (pop, elem) {
                    popForm = pop;
                    $('#SC3070208_Main>div').remove();
                    createWindow({ RequestStaffMemo: parentScreenInfo.RequestStaffMemo });
                },
                render: function (pop, index, args, container, header) {

                    if (index == 0) {
                        //ヘッダーのキャンセルボタンを定義
                        $("#SC3070208_PopOverFormHeader .icrop-PopOverForm-header-left").empty().html('<a href="#" class="nscPopUpCancelButton useCut"></a>')
                        $("#SC3070208_PopOverFormHeader .icrop-PopOverForm-header-left>a").text($("#SC3070208_HeaderCancelWord").text());
                        $("#SC3070208_HeaderTitle").text($("#SC3070208_HeaderTitleWord1").text());
                        $("#SC3070208_PopOverFormHeader .nscPopUpCancelButton").bind("click", function (e) {
                            pop.closePopOver();
                        });
                    }
                },
                preventLeft: true,
                preventRight: true,
                preventTop: false,
                preventBottom: true,
                elasticConstant: 0.3,
                id: "SC3070208_PopOver"
            });

            /**
            * @依頼ボタン押下
            */
            $('#SC3070208_RequestButton').live("click", function () {
                if (isEnableRequestButton() == false) return;
                $('#SC3070208_RequestButton').addClass('disabled');
                InsertInfo();
            });

            /**
            * @キャンセルボタン押下
            */
            $('#SC3070208_CancelButton').live("click", function () {
                $('#SC3070208_CancelButton').addClass('disabled');
                CancelInfo();
            });
        },


        setParams: function (params) {
            $.extend(parentScreenInfo, params)
        }

    }
} ();

$(function () {
    sc3070208Script.init();
});



