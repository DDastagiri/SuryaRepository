//━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
//SC3080202.Common.js
//─────────────────────────────────────
//機能： 顧客詳細(商談情報)
//補足： 
//作成： 2011/11/24 TCS 小野
//更新： 2012/03/16 TCS 相田　【SALES_2】TCS_0315ao_03対応
//更新： 2013/06/30 TCS 徐 【A STEP2】i-CROP新DB適応に向けた機能開発（既存流用）
//更新： 2013/12/09 TCS 市川 Aカード情報相互連携開発
//更新： 2014/02/12 TCS 山口 受注後フォロー機能開発
//更新： 2014/04/21 TCS 市川 GTMCタブレット高速化対応（BTS-386）
//更新： 2015/12/08 TCS 中村 (ﾄﾗｲ店ｼｽﾃﾑ評価)新車ﾀﾌﾞﾚｯﾄ(受注後)の管理機能開発
//更新： 2017/11/16 TCS 河原 TKM独自機能開発
//更新： 2018/07/10 TCS 河原 TKM Next Gen e-CRB Project Application development Block B-1
//更新： 2018/08/07 TCS 中村 （FS）基幹連携を用いたセールス業務実績入力の検証
//更新： 2019/12/06 TS  三浦 （FS）e-CRBシステム各種作業効率化の品質検証
//更新： 2020/01/22 TCS 重松 TKM Change request development for Next Gen e-CRB (CR058,CR061)
//─────────────────────────────────────

/// <reference path="../jquery.js"/>
/// <reference path="../jquery.fingerscroll.js"/>
/// <reference path="../SC3080201/Common.js"/>


// onload時の設定
$(function () {
    //2013/12/09 TCS 市川 Aカード情報相互連携開発 START
    var mng = Sys.WebForms.PageRequestManager.getInstance();
    mng.add_initializeRequest(function (sender, args) {
        //2020/01/22 TCS 重松 TKM Change request development for Next Gen e-CRB (CR058,CR061)変更 start
        if (args.get_postBackElement().id == "CommitSource1ButtonDummy") {
            //用件ソース1st
            $("#Source1SelectedNameCover").css("visibility", "visible");
            var handler = function (sender, args) {
                $("#Source1SelectedNameCover").css("visibility", "hidden");
                mng.remove_endRequest(handler);
            };
            mng.add_endRequest(handler);
        } else if (args.get_postBackElement().id == "CommitSource2ButtonDummy") {
            //用件ソース2nd
            $("#Source2SelectedNameCover").css("visibility", "visible");
            var handler = function (sender, args) {
                $("#Source2SelectedNameCover").css("visibility", "hidden");
                mng.remove_endRequest(handler);
            };
            mng.add_endRequest(handler);
        }
        //2020/01/22 TCS 重松 TKM Change request development for Next Gen e-CRB (CR058,CR061)変更 end
    });
    //2013/12/09 TCS 市川 Aカード情報相互連携開発 END

    $("#salesConditionCurrentMode").show();
    $("#salesConditionEditMode").hide();
    $("#normalSizeLinkButton").hide();
    if ($(".scNscCompetingCarAreaHidden").size() > 0) $("#bigSizeLinkButton").hide();
    selectedSeriesDisplay("", "", "");
    conditionEventStyleDisplay();
    
    //2018/07/10 TCS 河原 TKM Next Gen e-CRB Project Application development Block B-1 START
    DemandStructureStyleDisplay();
    //2018/07/10 TCS 河原 TKM Next Gen e-CRB Project Application development Block B-1 END

    // 2012/03/27 TCS 松野 【SALES_2】 START
    //activityEventStyleDisplay();
    // 2012/03/27 TCS 松野 【SALES_2】 END

    processDisplay();
    humanIconDisplay();
    compCarEventSizeChange("normalMode");

    if ($.trim($("#dispWalkinnum").text()).length === 0) $(".scNscNewActionBox .ActionBoxMember").css("background-image", "none");
    if ($.trim($("#dispAccount").text()).length === 0) $(".scNscNewActionBox .ActionBoxHuman").css("background-image", "none");

    //if ($.trim($("#CrActResult").attr("src")).length === 0) $(".scNscStatusIconList .scNscStatusIcon").hide();
    //2012/03/16 TCS 相田　【SALES_2】TCS_0315ao_03対応 START
    //if (SC3080201.newActivityFlg === true || $.trim($("#CrActResult").attr("src")) === "dummy.jpg") $(".scNscStatusIconList .scNscStatusIcon").hide();
    //'2013/06/30 TCS 宋 2013/10対応版　既存流用 START
    if ($.trim($("#CrActResult").attr("src")) === "") $(".scNscStatusIconList .scNscStatusIcon").hide();
    //'2013/06/30 TCS 宋 2013/10対応版　既存流用 END
    //2012/03/16 TCS 相田　【SALES_2】TCS_0315ao_03対応 END

    // ' 2012/02/29 TCS 小野 【SALES_2】 START
    // if ($("#PageEnabledFlgHidden").val() == "False") {
    //     $("#scNscOneBoxContentsArea :text,#scNscOneBoxContentsArea textarea").attr("disabled", "true");
    // }
    if ($("#selFllwupboxSalesBkgno").val().length <= 0) {
        if ($("#PageEnabledFlgHidden").val() == "False") {
            $("#scNscOneBoxContentsArea :text,#scNscOneBoxContentsArea textarea").attr("disabled", "true");
        }
    } else {
        // 受注後で、商談可能な場合、メモ欄入力可能
        if ($("#PageEnabledFlgHidden").val() == "False") {
            if ($("#MemoOnlyFlgHidden").val() != "True") {
                $("#scNscOneBoxContentsArea :text,#scNscOneBoxContentsArea textarea").attr("disabled", "true");
            }
        }
        if ($("#selFllwupboxSalesAfterFlg").val() == "1") {
            // 商談条件編集ボタン非表示
            $("#salesConditionCurrentMode").hide();
        }
    }
    // ' 2012/02/29 TCS 小野 【SALES_2】 END

    $("#ScNscCompeCarScrollPane").fingerScroll();

    //2012/04/06 TCS 河原 受注後で商談開始→キャンセル時に商談メモが入力可能になるバグ対応 START
    //// ' 2012/02/29 TCS 安田 【SALES_2】 START
    //if ($("#PageMoveFlgHidden").val() == "False") {
    //    $("#todayMemoTextBox").attr("readonly", "readonly");
    //} else {
    //    $("#todayMemoTextBox").removeAttr("readonly");
    //}
    //// ' 2012/02/29 TCS 安田 【SALES_2】 END
    //2012/04/06 TCS 河原 受注後で商談開始→キャンセル時に商談メモが入力可能になるバグ対応 END

    //2014/04/21 TCS市川 GTMCタブレット高速化対応 START
    //【最新活動】ポップアップ表示処理
    $("#NewActivityLabel").bind("click", function (e) {
        if ($("#PageActivityPopEnabledFlgHidden").val() == "False") return;
        //スクリプトの遅延読み込み
        SC3080201.requirePartialScript("../Scripts/SC3080202/SC3080202.LastCr.js", function () {

            //$01-STEP-1B-ここから-----
            if ($("#PageActivityPopEnabledFlgHidden").val() == "True") {
                //ポップアップフェードイン
                $("#activityPop_content").fadeIn(300);
            }
            //$01-STEP-1B-ここまで-----

            //2012/03/27 TCS 松野 【SALES_2】 START
            //共通読込みアニメーション変更
            $("#processingServer").addClass("activityPopupLoadingAnimation");
            $("#registOverlayBlack").addClass("popupLoadingBackgroundColor");
            //パネルの内容をクリア
            $("#activityPopPanel").empty();

            $("#activityPopUpdateDummyButton").click();
            //2012/03/27 TCS 松野 【SALES_2】 END
        });
    });
    //2014/04/21 TCS市川 GTMCタブレット高速化対応 END
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

//2014/02/12 TCS 山口 受注後フォロー機能開発 START
// プロセス表示
function processDisplay() {
    //プロセス日付初期化
    $("#dispProcessCatalogLabel").text("");
    $("#dispProcessTestdriveLabel").text("");
    $("#dispProcessEvaluationLabel").text("");
    $("#dispProcessQuotationLabel").text("");
    //プロセス日付反映
    $(".ProcessHiddenField li").each(function () {
        if ($(this).children(":nth-child(1)").val() == $("#selSeqnoHidden").val()) {
            $("#dispProcessCatalogLabel").text($(this).children(":nth-child(2)").val());
            $("#dispProcessTestdriveLabel").text($(this).children(":nth-child(3)").val());
            $("#dispProcessEvaluationLabel").text($(this).children(":nth-child(4)").val());
            $("#dispProcessQuotationLabel").text($(this).children(":nth-child(5)").val());
        }
    });

    //アイコン状態のクリア
    $("#dispProcessCatalogLabel, #dispProcessTestdriveLabel, #dispProcessEvaluationLabel, #dispProcessQuotationLabel").each(function () {
        if ($(this).text() != "") {
            $(this).parent().css("background", "url(" + $(this).parent().attr("onIconPath") + ") ");
            $(this).parent().css("background-position", "center top");
            $(this).parent().css("background-repeat", "no-repeat");
            if ($(this).attr("id") == "dispProcessCatalogLabel") {
                $(this).parent().removeClass("On").addClass("Off");
            }
            if ($(this).attr("id") == "dispProcessTestdriveLabel") {
                $(this).parent().removeClass("On").addClass("Off");
            }
            if ($(this).attr("id") == "dispProcessEvaluationLabel") {
                $(this).parent().removeClass("On").addClass("Off");
            }
            if ($(this).attr("id") == "dispProcessQuotationLabel") {
                $(this).parent().removeClass("On").addClass("Off");
            }
        } else {
            $(this).parent().css("background", "url(" + $(this).parent().attr("offIconPath") + ") ");
            $(this).parent().css("background-position", "center top");
            $(this).parent().css("background-repeat", "no-repeat");

            // ' 2012/02/29 TCS 小野 【SALES_2】 START
            if ($(this).attr("id") == "dispProcessCatalogLabel") {
                $("#dispProcessCatalogLabel").text($("#ProcessCatalogHiddenDefalutName").val());
                $(this).parent().removeClass("Off").addClass("On");
            }
            if ($(this).attr("id") == "dispProcessTestdriveLabel") {
                $("#dispProcessTestdriveLabel").text($("#ProcessTestdriveHiddenDefalutName").val());
                $(this).parent().removeClass("Off").addClass("On");
            }
            if ($(this).attr("id") == "dispProcessEvaluationLabel") {
                $("#dispProcessEvaluationLabel").text($("#ProcessEvaluationHiddenDefalutName").val());
                $(this).parent().removeClass("Off").addClass("On");
            }
            if ($(this).attr("id") == "dispProcessQuotationLabel") {
                $("#dispProcessQuotationLabel").text($("#ProcessQuotationHiddenDefalutName").val());
                $(this).parent().removeClass("Off").addClass("On");
            }
            // ' 2012/02/29 TCS 小野 【SALES_2】 END
        }
    });

}
//2014/02/12 TCS 山口 受注後フォロー機能開発 END

$(function () {

    //スクロール設定
    $(".scNsc50MemoBottom").fingerScroll();
    //    $("#todayMemoTextBox").css("height", ($("#todayMemoTextBox").get(0).scrollHeight + 30) + "px");
    //    $(".memoTextBoxInner").fingerScroll();
    //2018/07/10 TCS 河原 TKM Next Gen e-CRB Project Application development Block B-1 START
    $("#conditionAreaFrame").fingerScroll();
    //2018/07/10 TCS 河原 TKM Next Gen e-CRB Project Application development Block B-1 END

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

    //2012/03/02 TCS 平野  【SALES_1A】号口(課題No.66)対応 START
    //テキストエリアタップ
    //$("#todayMemoTextBox").live("mouseup touchend", function (e) {
    //    this.focus();
    //});
    //2012/03/02 TCS 平野  【SALES_1A】号口(課題No.66)対応 END

    //    //キーボード監視
    //    $("#todayMemoTextBox").live("keyup keydown change", function (e) {
    //        if ($("#todayMemoTextBox").get(0).scrollHeight > $("#todayMemoTextBox").height()) {
    //            $("#todayMemoTextBox").scrollTop(0);
    //            $("#todayMemoTextBox").css("height", ($("#todayMemoTextBox").get(0).scrollHeight + 30) + "px");
    //        }
    //    });


});

//2013/12/09 TCS 市川 Aカード情報相互連携開発 START
//2020/01/22 TCS 重松 TKM Change request development for Next Gen e-CRB (CR058,CR061)削除・変更 start
/********** Source1 PopOver **********/
//2020/01/22 TCS 重松 TKM Change request development for Next Gen e-CRB (CR058,CR061)削除・変更 end
//画面描画イベント(基盤 PopOverFormControl使用)
function renderPopOver_Source1(pop, index, args, container, header) {
    var page;
    var header;

    headerPage = header.children("#popOverSource1ListHeader");
    if (headerPage.size() == 0) {
        headerPage = $("#popOverSource1ListHeader").css("display", "block");
        headerPage.children(".cancelButton").unbind("click").bind("click", function () {
            pop.closePopOver(false);
        });
        headerPage.children(".commitButton").unbind("click").bind("click", function () {
            pop.closePopOver(true);
        });
        header.empty().append(headerPage);
    }
    page = container.children("#popOverSource1ListBody");
    if (page.size() == 0) {
        page = $("#popOverSource1ListBody").css("display", "block");
        container.empty().append(page);
        page.fingerScroll();
        page.find("ul > .itemRow").unbind("click").bind("click", function () {
            $(this).parent(0).children(".selected").removeClass("selected");
            $(this).addClass("selected");
            $("#popOverSource1ListHeader .commitButton").attr("selectingItemId", $(this).attr("value")).attr("selectingItemName", $(this).text());
        });
    }
}

//ポップアップ表示イベント(基盤 PopOverFormControl使用)
function openPopOver_Source1() {

    if ($("#PageEnabledFlgHidden").val() == "False") return false;
    //2020/01/22 TCS 重松 TKM Change request development for Next Gen e-CRB (CR058,CR061)追加 start
    if ($("#hdnSource1PossibleFlg").val() == "0") return false;

    //値を変えずにDoneボタンを押下した場合は処理を抜けるようにするため開いたときの選択値を保持する
    $("#hdnLastSource1").val() == $("#Source1SelectedCodeHidden").val()
    //2020/01/22 TCS 重松 TKM Change request development for Next Gen e-CRB (CR058,CR061)追加 end

    $("#popOverSource1ListBody").find(".itemRow").removeClass("selected")
      .each(function (index, Element) {
          if ($(this).attr("value") == $("#Source1SelectedCodeHidden").val()) {
              $(this).addClass("selected");
          }
      });
}

//ポップアップ閉じる時のイベント(基盤 PopOverFormControl使用)
function closePopOver_Source1(pop, result) {

    if (result) {
        $("#Source1SelectedCodeHidden").val($("#popOverSource1ListHeader .commitButton").attr("selectingItemId"));

        window.SC3080201.asyncAnimationEnable = false;
        $("#CommitSource1ButtonDummy").click();
        window.SC3080201.asyncAnimationEnable = true;
    }
    return false;
}
//2013/12/09 TCS 市川 Aカード情報相互連携開発 END
//2020/01/22 TCS 重松 TKM Change request development for Next Gen e-CRB (CR058,CR061)変更 start
/********** Source2 PopOver **********/
//画面描画イベント(基盤 PopOverFormControl使用)
function renderPopOver_Source2(pop, index, args, container, header) {
    var page;
    var header;
    headerPage = header.children("#popOverSource2ListHeader");
    if (headerPage.size() == 0) {
        headerPage = $("#popOverSource2ListHeader").css("display", "block");
        headerPage.children(".cancelButton").unbind("click").bind("click", function () {
            pop.closePopOver(false);
        });
        headerPage.children(".commitButton").unbind("click").bind("click", function () {
            pop.closePopOver(true);
        });
        header.empty().append(headerPage);
    }
    page = container.children("#popOverSource2ListBody");

    page = $("#popOverSource2ListBody").css("display", "block");
    container.empty().append(page);
    page.fingerScroll();
    page.find("ul > div > .itemRow").unbind("click").bind("click", function () {
        $(this).parent(0).children(".selected").removeClass("selected");
        $(this).addClass("selected");
        $("#popOverSource2ListHeader .commitButton").attr("selectingItemId", $(this).attr("value")).attr("selectingItemName", $(this).text());
    });
}

//ポップアップ表示イベント(基盤 PopOverFormControl使用)
function openPopOver_Source2() {
    if ($("#PageEnabledFlgHidden").val() == "False") return false;
    //値を変えずにDoneボタンを押下した場合は処理を抜けるようにするため開いたときの選択値を保持する
    if ($("#hdnSource2PossibleFlg").val() == "0") return false;

    $("#popOverSource2ListBody").find(".itemRow").removeClass("selected")
        .each(function (index, Element) {
            if ($(this).attr("value") == $("#Source2SelectedCodeHidden").val()) {
                $(this).addClass("selected");
            }
        });
}

//ポップアップ閉じる時のイベント(基盤 PopOverFormControl使用)
function closePopOver_Source2(pop, result) {

    if (result) {
        $("#Source2SelectedCodeHidden").val($("#popOverSource2ListHeader .commitButton").attr("selectingItemId"));
        window.SC3080201.asyncAnimationEnable = false;
        $("#CommitSource2ButtonDummy").click();
        window.SC3080201.asyncAnimationEnable = true;
    }
    return false;
}
//2020/01/22 TCS 重松 TKM Change request development for Next Gen e-CRB (CR058,CR061)変更 end

//2014/02/12 TCS 山口 受注後フォロー機能開発 START
$(function () {

    //メモ履歴欄設定
    BookedAfterMemoHis();

    //注文番号表示中の場合の場合
    if ($("#ContractNoFlgHidden").val() == "1") {
        //TCVボタン活性時のみ注文番号設定を行う
        if ($("#TcvRedirectFlgHidden").val() == "1") {
            //注文番号設定
            BookedAfterContractNum()
        }
    }

    //受注後の場合
    if ($("#selFllwupboxSalesAfterFlg").val() == "1") {

        //契約車種詳細情報初期設定
        AfterOdrPrcsCVDI();

        //プロセス欄初期設定

        //2015/12/08 TCS 中村 MOD (ﾄﾗｲ店ｼｽﾃﾑ評価)新車ﾀﾌﾞﾚｯﾄ(受注後)の管理機能開発 START
        if ( $("#UseAfterOdrProcFlgHidden").val() == "1") {
            BookedAfterSetProssess();
        }
        //2015/12/08 TCS 中村 MOD (ﾄﾗｲ店ｼｽﾃﾑ評価)新車ﾀﾌﾞﾚｯﾄ(受注後)の管理機能開発 END

    }

    //メモ履歴欄設定
    function BookedAfterMemoHis() {
        $("#custDtlPage2 .scNsc50MemoInBox li").each(function () {
            //メモスタッフアイコン設定
            var fileName = $(this).children("[id*=MemoHisStaffIconFileName]").val();
            $(this).children(".MemoHisStaffIcon").css("background", "url(../Styles/Images/Authority/" + fileName + ") no-repeat");
        });
        //Ellipsis設定
        $("#custDtlPage2 ul.DottedBoder").find(".ellipsis").CustomLabel({ 'useEllipsis': 'true' });

        //受注後の場合
        //2015/12/08 TCS 中村 MOD (ﾄﾗｲ店ｼｽﾃﾑ評価)新車ﾀﾌﾞﾚｯﾄ(受注後)の管理機能開発 START
        //if ($("#selFllwupboxSalesAfterFlg").val() == "1") {
        if ($("#selFllwupboxSalesAfterFlg").val() == "1" && $("#UseAfterOdrProcFlgHidden").val() == "1") {
            //2015/12/08 TCS 中村 MOD (ﾄﾗｲ店ｼｽﾃﾑ評価)新車ﾀﾌﾞﾚｯﾄ(受注後)の管理機能開発 END
            //商談メモ初期設定(高さ変更)
            var memo = $(".scNsc50MemoArea");
            memo.find("div.scNsc50MemoTop").css("height", "190px");
            memo.find("div.memoTextBoxInner").css("height", "170px");
            memo.find("#todayMemoTextBox").css("height", "170px");
        }
    }

    //注文番号設定
    function BookedAfterContractNum() {
        //注文番号設定
        $("#AcardNumOrContractNumValue").css("color", "#375388");
        //注文番号イベント定義
        $("#AcardNumOrContractNumValue").bind("click", function (e) {
            SC3080201.showLoding();
            //ダミーボタン押下で見積画面遷移
            $("#AcardNumOrContractNumDummyButton").click();
        });
    }

    //契約車種詳細情報設定
    function AfterOdrPrcsCVDI() {
        var afterOdrPrcs = $("#AfterOdrPrcsCVDIPanel ul.ContractVehicleDetailInfoBox");

        //2019/12/06 TS  三浦 （FS）e-CRBシステム各種作業効率化の品質検証 START
        if("1" == $("#DispFlgActStatus").val()){
            //スクロール設定
            afterOdrPrcs.VScroll().css({
                "overflow-x": "hidden",
                "overflow-y": "scroll",
                "overflow:scroll": "touch"
            });
        }
        //2018/08/07 TCS 中村 （FS）基幹連携を用いたセールス業務実績入力の検証 START DEL
        //2018/08/07 TCS 中村 （FS）基幹連携を用いたセールス業務実績入力の検証 END
        //2019/12/06 TS  三浦 （FS）e-CRBシステム各種作業効率化の品質検証 END

        //Ellipsis設定
        afterOdrPrcs.find(".ellipsis").CustomLabel({ 'useEllipsis': 'true' });
    }

    //プロセス欄初期設定
    function BookedAfterSetProssess() {
        //受注前プロセス非表示
        $(".scNscProcessIconListUl li:lt(4)").css("display", "none");
        //プロセス欄拡大
        $("#dispProcessArea").css({
            "width": "408px",
            "border-right": "0",
            "padding-left": "16px",
            "padding-right": "16px"
        });
        $("#dispProcessAreaInner").css({
            "width": "408px",
            "position": "relative"
        });
        $("#custDtlPage2 .scNscProcessIconList").css("width", parseInt($("#AfterOdrPrcsIconMaxPageHidden").val()) * 430 + "px");
        $("#custDtlPage2 .scNscProcessIconList ul.scNscProcessIconListUl li").css("width", "68px");
        //ステータス非表示
        $(".scNscStatusIconList").css("display", "none");
        $(".scNscProcessAndStatusArea .scNscTitleStatus").css("display", "none");

        //プロセススクロールボタン初期設定
        ProcessArrow();

        //プロセスアイコン設定
        $(".scNscProcessIconListUl li:gt(3)").each(function () {
            //アイコン
            $(this).css({
                "background": "url(" + $(this).attr("IconPath") + ") ",
                "background-position": "center top",
                "background-repeat": "no-repeat"
            });
            //フラグにより駐車禁止マーク
            if ($(this).children("[id*=ProcessBookedAfterCheckFlg]").val() == "0") {
                $(this).children(".ProcessBookedAfterNoUse").addClass("On");
            }
        });

        //受注前/受注後切り替えボタンイベント定義
        var leftButton = $("#BeforeAfterOdrPrcsSwitchButtonPanel .LeftOdrPrcsButton");
        var rightButton = $("#BeforeAfterOdrPrcsSwitchButtonPanel .RightOdrPrcsButton");
        leftButton.bind("click", function (e) {
            if (leftButton.hasClass("PrcsButtonOff")) {
                //受注後→受注前
                leftButton.removeClass("PrcsButtonOff").addClass("PrcsButtonOn");
                rightButton.removeClass("PrcsButtonOn").addClass("PrcsButtonOff");

                //プロセススクロールボタン非表示
                $("#ProcessLeftArrow").hide();
                $("#ProcessRightArrow").hide();
                //スクロール位置初期化
                $("#dispProcessAreaInner").scrollLeft(5);

                //プロセス切り替え
                $(".scNscProcessIconListUl li:gt(3)").css("display", "none");
                $(".scNscProcessIconListUl li:lt(4)").show();

            }
        });
        rightButton.bind("click", function (e) {
            if (rightButton.hasClass("PrcsButtonOff")) {
                //受注前→受注後
                leftButton.removeClass("PrcsButtonOn").addClass("PrcsButtonOff");
                rightButton.removeClass("PrcsButtonOff").addClass("PrcsButtonOn");

                //表示中ページ初期化
                $("#AfterOdrPrcsIconPageHidden").val("1");
                //プロセススクロールボタン初期設定
                ProcessArrow();

                //プロセス切り替え
                $(".scNscProcessIconListUl li:lt(4)").css("display", "none");
                $(".scNscProcessIconListUl li:gt(3)").show();

            }
        });
    }

    //プロセススクロールボタン初期設定
    function ProcessArrow() {
        if (parseInt($("#AfterOdrPrcsIconMaxPageHidden").val()) > 1) {
            //最大ページ数 > 1
            $("#ProcessLeftArrow").hide();
            $("#ProcessRightArrow").show();
            //スクロール位置初期化
            $("#dispProcessAreaInner").scrollLeft(5);

            //プロセススクロールボタンイベント定義
            $("#ProcessRightArrow").unbind("click").bind("click", function (e) {
                //スクロール範囲保持
                var scroll = parseInt($("#AfterOdrPrcsIconPageHidden").val()) * 408 + 6;
                //表示中ページ更新
                $("#AfterOdrPrcsIconPageHidden").val(parseInt($("#AfterOdrPrcsIconPageHidden").val()) + 1);

                //プロセススクロールボタン制御
                ProcessArrowDisplay($("#ProcessLeftArrow"));
                //スクロール
                $("#dispProcessAreaInner").animate({ scrollLeft: scroll });
            });
            $("#ProcessLeftArrow").unbind("click").bind("click", function (e) {
                //スクロール範囲保持
                var scroll = (parseInt($("#AfterOdrPrcsIconPageHidden").val()) - 2) * 408 + 6;
                //表示中ページ更新
                $("#AfterOdrPrcsIconPageHidden").val(parseInt($("#AfterOdrPrcsIconPageHidden").val()) - 1);

                //プロセススクロールボタン制御
                ProcessArrowDisplay($("#ProcessRightArrow"));
                //スクロール
                $("#dispProcessAreaInner").animate({ scrollLeft: scroll });
            });

        } else {
            //最大ページ数 = 1
            $("#ProcessLeftArrow").hide();
            $("#ProcessRightArrow").hide();
        }
    }

    //プロセススクロールボタン制御
    function ProcessArrowDisplay(Arrow) {
        //現在のページ数により処理分岐
        if ($("#AfterOdrPrcsIconPageHidden").val() == $("#AfterOdrPrcsIconMaxPageHidden").val()) {
            //表示中ページ = 最大ページ数
            $("#ProcessLeftArrow").show();
            $("#ProcessRightArrow").hide();
        } else if ($("#AfterOdrPrcsIconPageHidden").val() == "1") {
            //表示中ページ = 1
            $("#ProcessLeftArrow").hide();
            $("#ProcessRightArrow").show();
        } else {
            //表示中ページ < 最大ページ数
            Arrow.show();
        }
    }
});

//2014/02/12 TCS 山口 受注後フォロー機能開発 END

//2014/04/21 TCS市川 GTMCタブレット高速化対応 START
/**
* 競合車種ポップアップ処理
*/
function showCompCarSelect() {

    //読み取り専用の場合は無視
    if ($("#PageEnabledFlgHidden").val() == "False") return;

    //スクリプトの遅延読み込み
    SC3080201.requirePartialScript("../Scripts/SC3080202/SC3080202.Compcar.js", function () {

        //１ページ目を表示
        setPopupPageComp("page1");

        //ポップアップ初期化
        initCompPopup();

        //ポップアップ表示
        $("#CompCarSelectPopup").fadeIn(300);

        //共通読込みアニメーション戻し
        $("#processingServer").removeClass("carSelectPopupCompLoadingAnimation");
        $("#registOverlayBlack").removeClass("popupLoadingBackgroundColor");
    });
}
/**
* 競合車種の拡大縮小
*/
function compCarEventSizeChange(modeClass) {

    if ($(".titleCompeMaker").size() <= 0) {
        $("#dispCompeCarCountFlg").hide();
        $("#dispCompeCarCountNoFlg").show();
    } else {
        $("#dispCompeCarCountFlg").show();
        $("#dispCompeCarCountNoFlg").hide();
    }

    if (modeClass !== undefined && typeof modeClass === "string") {
        //指定モードで設定
        $("#scNscCompetingCarAreaInner").removeClass("normalMode bigMode").addClass(modeClass);
    } else {

        //切り替え処理
        if ($("#scNscCompetingCarAreaInner").hasClass("normalMode") === true) {
            //拡大サイズ
            $("#scNscCompetingCarAreaInner").removeClass("normalMode").addClass("bigMode");
        } else {
            //縮小サイズ
            $("#scNscCompetingCarAreaInner").removeClass("bigMode").addClass("normalMode");
        }

    }

    //拡大縮小ボタン
    if ($("#scNscCompetingCarAreaInner").hasClass("normalMode") === true) {
        //縮小サイズ
        if ($("#otherCountHidden").val() == "0") {
            $("#bigSizeLinkButton").hide();
        } else {
            $("#bigSizeLinkButton").show();
        }
        $("#normalSizeLinkButton").hide();
    } else {
        //拡大サイズ
        $("#bigSizeLinkButton").hide();
        $("#normalSizeLinkButton").show();
    }
    $("#ScNscCompeCarScrollPane").fingerScroll();
    return false;
}

$(function () {

    //競合車種の拡大縮小イベントバインド
    $("#scNscCompeCarArea .scNscCompetingCarArea .moreCarEvent").live("mousedown touchstart", compCarEventSizeChange);

    //競合車種エリア押下時の処理 (ポップアップ表示)
    $("#scNscCompetingCarAreaInner,#dispCompeCarCountNoFlg").live("click", function (e) {
        if ($(e.target).is("#scNscCompeCarArea .scNscCompetingCarArea .moreCarEvent, #scNscCompeCarArea .scNscCompetingCarArea .moreCarEvent *") === true) return;

        //2012/03/16 TCS 藤井 【SALES_2】性能改善 Delete 
        //2012/03/16 TCS 藤井 【SALES_2】性能改善 Add Start

        if ($("#PageEnabledFlgHidden").val() == "False") return;

        //スクリプトの遅延読み込み
        SC3080201.requirePartialScript("../Scripts/SC3080202/SC3080202.Compcar.js", function () {

            //HTML削除
            $(".scNsc51PopUpScrollWrapComp").empty();

            //タイトル表示(メーカー)
            $("#CompCarPopupMakerTitle").show();

            //タイトル非表示(車種)
            $("#CompCarPopupModelTitle").css("display", "none");

            //取り消し、完成非表示
            $(".scNscCompPopUpCancelButton").css("display", "none");
            $(".scNscCompPopUpCompleteButton").css("display", "none");

            //ポップアップ表示
            $("#CompCarSelectPopup").fadeIn(300);

            //共通読込みアニメーション変更
            $("#processingServer").addClass("carSelectPopupCompLoadingAnimation");
            $("#registOverlayBlack").addClass("popupLoadingBackgroundColor");

            //サーバー処理実行
            $("#CompCarSelectPopupButtonDummy").click();
            //2012/03/16 TCS 藤井 【SALES_2】性能改善 Add End
        });

    });
});
//2014/04/21 TCS市川 GTMCタブレット高速化対応 END

//2017/11/16 TCS 河原 TKM独自機能開発 START 
$(function () {

    if ($("#Use_Direct_Billing_Flg").val() != "1") {
        $("#dispSelectedDirectBilling").css("display", "none");
    }

    $("#SelectedDirectBilling").click(function () {
        $("#CommitdDirectBillingDummy").click();
    }
    );
});
//2017/11/16 TCS 河原 TKM独自機能開発 END

//TKMローカル処理
//下取車両メーカーマスタポップアップ読み込み後
function Trade_in_MakerPageOpenEnd() {

    //スクロール
    $(".Trade_in_MakerListBox").fingerScroll();

    if ($("#Trade_in_MakerValue").attr("value") != "") {
        //選択済みのものがあれば選択済みスタイル適用
        var targetClass = "#Trade_in_MakerPanel .nscListBoxSetIn #" + $("#Trade_in_MakerValue").attr("value");
        $(targetClass).addClass("Selection");
    }
    $("#processingServer").css("z-index", "");
    $("#processingServer").css("top", "");
    $("#processingServer").css("left", "");
}

//下取車両モデルマスタポップアップ読み込み後
function Trade_in_ModelPageOpenEnd() {

    //スクロール
    $(".Trade_in_ModelListBox").fingerScroll();

    if ($("#Trade_in_ModelValue").attr("value") != "") {
        //選択済みのものがあれば選択済みスタイル適用
        var targetClass = "#Trade_in_ModelPanel .nscListBoxSetIn #" + $("#Trade_in_ModelValue").attr("value");
        $(targetClass).addClass("Selection");
    }
    $("#processingServer").css("z-index", "");
    $("#processingServer").css("top", "");
    $("#processingServer").css("left", "");
}

//下取車両年式ポップアップ読み込み後
function Trade_in_ModelYearPageOpenEnd() {

    //スクロール
    $(".Trade_in_ModelYearListBox").fingerScroll();

    if ($("#Trade_in_ModelYearValue").attr("value") != "") {
        //選択済みのものがあれば選択済みスタイル適用
        var targetClass = "#Trade_in_ModelYearPanel .nscListBoxSetIn #" + $("#Trade_in_ModelYearValue").attr("value");
        $(targetClass).addClass("Selection");
    }
    $("#processingServer").css("z-index", "");
    $("#processingServer").css("top", "");
    $("#processingServer").css("left", "");
}

$(function () {
    if ($("#DemandStructureCd").val() != "") {
        //購入分類の復元
        $("#demandStructureArea div ul li").each(function () {
            // 選択済の場合
            if ($("#DemandStructureCd").val() == $(this).children(":nth-child(2)").val()) {
                $(this).addClass("OnGrey");
            }
        });
    }

    $("#Trade_in_Maker").html($("#Trade_in_MakerName").val());
    $("#Trade_in_Model").html($("#Trade_in_ModelName").val());
    $("#Trade_in_Mileage").html($("#Trade_in_MileageValue").val());
    $("#Trade_in_ModelYear").html($("#Trade_in_ModelYearValue").val());

    //各ポップアップ呼び出し時のデザインの調整
    $("#Trade_in_MakerTrigger").click(function () {
        setPopupIniit2($("#Trade_in_MakerPopOver_content"));
    });
    $("#Trade_in_ModelTrigger").click(function () {
        setPopupIniit2($("#Trade_in_ModelPopOver_content"));
    });
    $("#Trade_in_ModelYearTrigger").click(function () {
        setPopupIniit2($("#Trade_in_ModelYearPopOver_content"));
    });
});

function DemandStructureStyleDisplay() {
    $("#demandStructureArea div ul li").each(function () {
        if ($(this).index() == 0) {
            $(this).addClass("Left");
        } else if ($(this).index() == $(this).parent().children().size() - 1) {
            $(this).addClass("Right");
        } else {
            $(this).addClass("Center");
        }
        // TODO:小数点以下要計算？
        $(this).width(((($(this).parent().width()) - ($(this).parent().children().size()) - 1) / ($(this).parent().children().size())) + "px");

        if ($("#DemandStructureCd").val() != "") {
            // 選択済みのものはグレー表示
            if ($("#DemandStructureCd").val() == $(this).children(":nth-child(2)").val()) {
                $(this).addClass("OnGrey");
            }
        }

    });

    if ($("#DemandStructureCd").val() != "") {
        //選択中の購入分類の下取車両入力可否フラグ取得
        this_form.TradeinEnabledFlg.value = "0";
        $("#demandStructureArea div ul li").each(function () {
            if ($("#DemandStructureCd").val() == $(this).children(":nth-child(2)").val()) {
                this_form.TradeinEnabledFlg.value = $(this).children("input:nth-child(3)").attr("value");
            }
        });
    } else {
        this_form.TradeinEnabledFlg.value = "0";
    }

    //下取車両が入力不可の購入分類を選択した場合
    if (this_form.TradeinEnabledFlg.value == "0") {
        this_form.Trade_in_MakerValue.value = "";
        $("#Trade_in_Maker").html("");

        this_form.Trade_in_ModelValue.value = "";
        $("#Trade_in_Model").html("");

        this_form.Trade_in_MileageValue.value = "";
        $("#Trade_in_Mileage").html("");

        this_form.Trade_in_ModelYearValue.value = "";
        $("#Trade_in_ModelYear").html("");

        //下取車両欄のデザイン変更
        $("#demandStructureArea .ColorWhite").removeClass("ColorWhite").addClass("ColorGary");

    } else {
        //下取車両欄のデザイン変更
        $("#demandStructureArea .ColorGary").removeClass("ColorGary").addClass("ColorWhite");
    }

    return false;
}

//ポップアップ表示時にスタイルを調整する
function setPopupIniit2(contentTag) {
    contentTag.parents(".popover").css("border", "0px solid black");
    contentTag.parents(".popover").css("background", "Transparent");
    contentTag.parents(".popover").css("box-shadow", "None");
    contentTag.parents(".popover").find(".content").css("padding", "0px");
    contentTag.parents(".popover").find(".content").css("margin", "0px");
    contentTag.parents(".popover").find(".content").css("background", "Transparent");
    contentTag.parents(".popover").find(".content").css("border", "none");

    setTimeout(function () {
        //サーバー処理実行
        contentTag.parents(".popover").css("border-width", "0px");
        contentTag.parents(".popover").css("background-image", "initial");
        contentTag.parents(".popover").css("box-shadow", "none");
    }, 0);
}

//購入分類エラー時のメッセージ作成
function errMsg(checkResult) {
    var msg = $("#msg2020913").text();

    if (checkResult.substr(0, 1) == "0") {
    } else {
        msg = msg + "\r\n    " + $("#DemandStructureLabel").text();
    }

    if (checkResult.substr(1, 1) == "0") {
    } else {
        msg = msg + "\r\n    " + $("#Trade_in_MakerLabel").text();
    }

    if (checkResult.substr(2, 1) == "0") {
    } else {
        msg = msg + "\r\n    " + $("#Trade_in_ModelLabel").text();
    }

    if (checkResult.substr(3, 1) == "0") {
    } else {
        msg = msg + "\r\n    " + $("#Trade_in_MileageLabel").text();
    }

    if (checkResult.substr(4, 1) == "0") {
    } else {
        msg = msg + "\r\n    " + $("#Trade_in_ModelYearLabel").text();
    }

    return msg
}
