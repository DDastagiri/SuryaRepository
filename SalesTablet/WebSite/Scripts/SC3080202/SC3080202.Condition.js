//━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
//SC3080202.Condition.js
//─────────────────────────────────────
//機能： 顧客詳細(商談情報)
//補足： 
//作成： 2011/11/24 TCS 小野
//更新： 2018/07/10 TCS 河原 TKM Next Gen e-CRB Project Application development Block B-1
//─────────────────────────────────────

/// <reference path="../jquery.js"/>
/// <reference path="../jquery.fingerscroll.js"/>


// 商談条件イベント
$(function () {
    // 編集ボタン押下イベント
    $("#salesConditionCurrentMode").live("click", function () {
        if ($("#PageEnabledFlgHidden").val() == "False") return;
        // 編集可
        $("#salesConditionCurrentMode").hide();
        $("#salesConditionEditMode").show();
        // 選択済みのものをブルー表示
        $("#conditionArea div ul li.OnGrey").removeClass("OnGrey").addClass("OnBlue");

        //2018/07/10 TCS 河原 TKM Next Gen e-CRB Project Application development Block B-1 START
        $("#demandStructureArea div ul li.OnGrey").removeClass("OnGrey").addClass("OnBlue");
        this_form.BeforeDemandStructureCd.value = this_form.DemandStructureCd.value
        this_form.BeforeTradeinEnabledFlg.value = this_form.TradeinEnabledFlg.value
        this_form.BeforeTrade_in_MakerName.value = this_form.Trade_in_MakerName.value
        this_form.BeforeTrade_in_MakerValue.value = this_form.Trade_in_MakerValue.value
        this_form.BeforeTrade_in_ModelName.value = this_form.Trade_in_ModelName.value
        this_form.BeforeTrade_in_ModelValue.value = this_form.Trade_in_ModelValue.value
        this_form.BeforeTrade_in_MileageValue.value = this_form.Trade_in_MileageValue.value
        this_form.BeforeTrade_in_ModelYearValue.value = this_form.Trade_in_ModelYearValue.value
        //2018/07/10 TCS 河原 TKM Next Gen e-CRB Project Application development Block B-1 END

    });

    // キャンセルボタン押下イベント
    $("#salesConditionCancel").live("click",
        function () {
            // 編集不可
            $("#salesConditionCurrentMode").show();
            $("#salesConditionEditMode").hide();
            // 全項目、初期表示
            $("#conditionArea div ul li").removeClass("OnBlue");
            $("#conditionArea div ul li").removeClass("OnGrey");
            conditionEventStyleDisplay();

            //2018/07/10 TCS 河原 TKM Next Gen e-CRB Project Application development Block B-1 START
            $("#demandStructureArea div ul li").removeClass("OnBlue");
            $("#demandStructureArea div ul li").removeClass("OnGrey");

            //編集前の値の復元
            this_form.DemandStructureCd.value = this_form.BeforeDemandStructureCd.value
            this_form.TradeinEnabledFlg.value = this_form.BeforeTradeinEnabledFlg.value
            this_form.Trade_in_MakerName.value = this_form.BeforeTrade_in_MakerName.value
            this_form.Trade_in_MakerValue.value = this_form.BeforeTrade_in_MakerValue.value
            this_form.Trade_in_ModelName.value = this_form.BeforeTrade_in_ModelName.value
            this_form.Trade_in_ModelValue.value = this_form.BeforeTrade_in_ModelValue.value
            this_form.Trade_in_MileageValue.value = this_form.BeforeTrade_in_MileageValue.value
            this_form.Trade_in_ModelYearValue.value = this_form.BeforeTrade_in_ModelYearValue.value

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

            // 項目名変更アイテム設定
            $("#conditionArea div ul li").each(function () {
                // 選択済の項目
                if ($(this).hasClass("OnGrey") && $(this).children(":nth-child(5)").val() == "2") {
                    // ItemTitleをEditボタン押下時点に戻す
                    // ItemTitleのマスタ値を取得
                    var masterItemTitle = $(this).children(":nth-child(7)").val();
                    // ItemTitleに、マスタ値の置換文字列("%1")を項目名の初期値で置換した結果を設定する
                    var defaultItemTitle = masterItemTitle.replace("%1", $(this).children(":nth-child(8)").val());
                    $(this).children(":nth-child(1)").val(defaultItemTitle);
                    $(this).children(":nth-child(1)")[0].innerText = defaultItemTitle;
                    $(this).children(":nth-child(6)").val($(this).children(":nth-child(8)").val());
                }
            });

            DemandStructureStyleDisplay();
            //2018/07/10 TCS 河原 TKM Next Gen e-CRB Project Application development Block B-1 END

        }
    );

    // 完了ボタン押下イベント
    $("#salesConditionCompleteButton").live("click",
        function () {

            //2018/07/10 TCS 河原 TKM Next Gen e-CRB Project Application development Block B-1 START
            //購入分類関連の必須入力チェック
            var checkRslt = checkDemandStructure();

            if (checkRslt != "00000") {
                alert(errMsg(checkRslt));
                return;
            }
            //2018/07/10 TCS 河原 TKM Next Gen e-CRB Project Application development Block B-1 END

            // Hidden項目設定
            $("#conditionArea div ul li").each(function () {
                if ($(this).hasClass("OnBlue")) {
                    $(this).children(":nth-child(4)").val("True");
                    //2018/07/10 TCS 河原 TKM Next Gen e-CRB Project Application development Block B-1 START
                    if ($(this).children(":nth-child(5)").val() == "2") {
                        $(this).children(":nth-child(8)").val($(this).children(":nth-child(6)").val());
                    }
                    //2018/07/10 TCS 河原 TKM Next Gen e-CRB Project Application development Block B-1 END
                } else {
                    $(this).children(":nth-child(4)").val("False");
                    if ($(this).children(":nth-child(5)").val() == "1") {
                        $(this).children(":nth-child(6)").val("");
                        //2018/07/10 TCS 河原 TKM Next Gen e-CRB Project Application development Block B-1 START
                    } else if ($(this).children(":nth-child(5)").val() == "2") {
                        $(this).children(":nth-child(6)").val("");
                        $(this).children(":nth-child(8)").val("");
                    }
                    //2018/07/10 TCS 河原 TKM Next Gen e-CRB Project Application development Block B-1 END
                }

            });
            // 編集不可
            $("#salesConditionCurrentMode").show();
            $("#salesConditionEditMode").hide();
            // 全項目、初期表示
            $("#conditionArea div ul li").removeClass("OnBlue");
            $("#conditionArea div ul li").removeClass("OnGrey");
            conditionEventStyleDisplay();

            //2018/07/10 TCS 河原 TKM Next Gen e-CRB Project Application development Block B-1 START
            $("#demandStructureArea div ul li").removeClass("OnBlue");
            $("#demandStructureArea div ul li").removeClass("OnGrey");

            DemandStructureStyleDisplay();
            //2018/07/10 TCS 河原 TKM Next Gen e-CRB Project Application development Block B-1 END

            $("#salesConditionCompleteButtonDummy").click();
        }
    );

    // 項目変更イベント
    $("#conditionArea div ul li").live("click", function () {

        if ($("#OtherConditionInputPopup").is(":visible") === true) return;

        //編集モードの場合
        // ' 2012/02/29 TCS 小野 【SALES_2】 START
        //if ($("#salesConditionCurrentMode").css("display") == "none"){
        if ($("#salesConditionCurrentMode").css("display") == "none" && $("#salesConditionEditMode").css("display") == "block") {
            // ' 2012/02/29 TCS 小野 【SALES_2】 END

            //以前の選択状態を保存
            beforeSelection = $(this).parent().children().filter(".OnBlue");

            if ($(this).parent().parent().children(":nth-child(3)").val() == "1") {

                //複数選択可の場合
                if ($(this).hasClass("OnBlue")) {
                    // 選択状態なら、未選択状態とする
                    $(this).removeClass("OnBlue");
                } else {
                    // 未選択状態なら、選択状態とする
                    $(this).addClass("OnBlue");
                    $("#selects").click();
                }
                singleSelection = false;
            } else {

                //複数選択不可の場合
                if ($(this).hasClass("OnBlue")) {
                    // 選択状態なら、未選択状態とする
                    $(this).removeClass("OnBlue");
                } else {
                    // 別の項目を全て未選択状態とする
                    $(this).parent().children().removeClass("OnBlue");
                    // 未選択状態なら、選択状態とする
                    $(this).addClass("OnBlue");
                    $("#selects").click();
                }
                singleSelection = true;
            }

            //その他ポップアップ
            if ($(this).is(".OnBlue") === true && $(this).children(":nth-child(5)").val() == "1") {
                //オープン
                openOtherPopup(this);
            }

            //2018/07/10 TCS 河原 TKM Next Gen e-CRB Project Application development Block B-1 START
            //項目名変更ポップアップ
            if ($(this).is(".OnBlue") === true && $(this).children(":nth-child(5)").val() == "2") {
                //オープン
                openCondItemLabelPopup(this);
            }
            //2018/07/10 TCS 河原 TKM Next Gen e-CRB Project Application development Block B-1 END
        }

    });

    //その他条件入力ポップアップへの受け渡しパラメータ
    var hiddenId = "";
    var targetLiTag;
    var beforeSelection;
    var singleSelection;

    //その他条件入力ポップアップ表示
    function openOtherPopup(target) {

        var top = $(target).offset().top - 170;

        //2012/03/07 TCS 平野  【SALES_2】(Sales1A ユーザテスト No.195) START
        //var left = $(target).offset().left - 130;
        var pop = $("#custDtlPage2 .scNsc51PopUpModelSelectArrowOther").width();
        var detail = 31;
        var left = $(target).offset().left + (target.offsetWidth / 2) - (pop / 2) - detail;
        //2012/03/07 TCS 平野  【SALES_2】(Sales1A ユーザテスト No.195) END

        hiddenId = $(target).children(":nth-child(6)").attr("id");

        targetLiTag = target;

        //テキスト更新
        $("#ScNsc51OtherConditionInputText").CustomTextBox("updateText", $("#" + hiddenId).val())

        //2018/07/10 TCS 河原 TKM Next Gen e-CRB Project Application development Block B-1 START
        var ArrowLeft = -10;
        var ArrowLeftOffset = left - 10 + ArrowLeft;

        if (left < 10) {
            //表示
            $("#OtherConditionInputPopup").css({ top: top, left: 10 }).fadeIn(300);
            $("#custDtlPage2 .scNsc51PopUpModelSelectArrowOther").css({ left: ArrowLeftOffset });
        } else {
            //表示
            $("#OtherConditionInputPopup").css({ top: top, left: left }).fadeIn(300);
            $("#custDtlPage2 .scNsc51PopUpModelSelectArrowOther").css({ left: -10 });
        }
        //2018/07/10 TCS 河原 TKM Next Gen e-CRB Project Application development Block B-1 END
    };

    //その他条件入力ポップアップのキャンセルボタンクリック
    $(".scNscOtherPopUpCancelButton").live("click", function (e) {

        calcelOtherPopup();
    });

    //その他条件入力ポップアップの完了ボタンクリック
    $(".scNscOtherPopUpCompleteButton").live("click", function (e) {

        //チェック
        if ($("#ScNsc51OtherConditionInputText").val().length <= 0) {
            icropScript.ShowMessageBox(0, $("#OtherConditionErrorMessage").val(), "");
            return;
        }

        $("#" + hiddenId).val($("#ScNsc51OtherConditionInputText").val());
        $("#OtherConditionInputPopup").fadeOut(300);
    });

    //その他条件入力ポップアップクローズの監視
    $(document.body).bind("mousedown touchstart", function (e) {
        if ($("#OtherConditionInputPopup").is(":visible") === false) return;
        if ($(e.target).is("#OtherConditionInputPopup, #OtherConditionInputPopup *") === false) {
            calcelOtherPopup();
            //event.preventDefault();
        }
    });

    //その他条件入力のキャンセル
    function calcelOtherPopup() {

        //選択状態をキャンセルする
        $(targetLiTag).removeClass("OnBlue");

        //ラジオ形式選択の場合は、もとの選択状態に戻る
        if (singleSelection === true) beforeSelection.addClass("OnBlue");

        //ポップアップを閉じる
        $("#OtherConditionInputPopup").fadeOut(300);
    }



    //2018/07/10 TCS 河原 TKM Next Gen e-CRB Project Application development Block B-1 START
    //項目名変更ポップアップ表示
    function openCondItemLabelPopup(target) {

        var top = $(target).offset().top - 170;

        var pop = $("#custDtlPage2 .scNsc51PopUpModelSelectArrowOther").width();
        var detail = 31;
        var left = $(target).offset().left + (target.offsetWidth / 2) - (pop / 2) - detail;

        hiddenId = $(target).children(":nth-child(6)").attr("id");

        targetLiTag = target;

        //タイトル設定：商談条件名を設定する
        $("#CondItemLabelInputPopupTitle").text(target.parentNode.parentNode.children[0].textContent);
        //テキスト更新
        $("#ScNsc51CondItemLabelInputText").CustomTextBox("updateText", $("#" + hiddenId).val())

        var ArrowLeft = -10;
        var ArrowLeftOffset = left - 10 + ArrowLeft;

        if (left < 10) {
            //表示
            $("#CondItemLabelInputPopup").css({ top: top, left: 10 }).fadeIn(300);
            $("#custDtlPage2 .scNsc51PopUpModelSelectArrowItmLbl").css({ left: ArrowLeftOffset });
        } else {
            //表示
            $("#CondItemLabelInputPopup").css({ top: top, left: left }).fadeIn(300);
            $("#custDtlPage2 .scNsc51PopUpModelSelectArrowItmLbl").css({ left: -10 });
        }
    };

    //項目名変更ポップアップのキャンセルボタンクリック
    $(".scNscCondItemLabelPopUpCancelButton").live("click", function (e) {

        cancelCondItemLabelPopup();
    });

    //項目名変更ポップアップの完了ボタンクリック
    $(".scNscCondItemLabelPopUpCompleteButton").live("click", function (e) {

        //チェック
        if ($("#ScNsc51CondItemLabelInputText").val().length <= 0) {
            icropScript.ShowMessageBox(0, $("#CondItemLabelErrorMessage").val(), "");
            return;
        }

        //ItemTitleのマスタ値を取得
        var defaultItemTitle = $(targetLiTag).children(":nth-child(7)")[0].value;

        //ItemTitleに、マスタ値の置換文字列("%1")を入力値で置換した結果を設定する
        $(targetLiTag).children(":nth-child(1)")[0].innerText = defaultItemTitle.replace("%1", $("#ScNsc51CondItemLabelInputText").val());

        //入力値の保存
        $("#" + hiddenId).val($("#ScNsc51CondItemLabelInputText").val());
        $("#CondItemLabelInputPopup").fadeOut(300);
    });

    //項目名変更ポップアップクローズの監視
    $(document.body).bind("mousedown touchstart", function (e) {
        if ($("#CondItemLabelInputPopup").is(":visible") === false) return;
        if ($(e.target).is("#CondItemLabelInputPopup, #CondItemLabelInputPopup *") === false) {
            cancelCondItemLabelPopup();
        }
    });

    //項目名変更のキャンセル
    function cancelCondItemLabelPopup() {

        //選択状態をキャンセルする
        $(targetLiTag).removeClass("OnBlue");

        //ラジオ形式選択の場合は、もとの選択状態に戻る
        if (singleSelection === true) beforeSelection.addClass("OnBlue");

        //ポップアップを閉じる
        $("#CondItemLabelInputPopup").fadeOut(300);
    }
    //2018/07/10 TCS 河原 TKM Next Gen e-CRB Project Application development Block B-1 END

});



//イベント
// リストのスタイル初期設定
function conditionEventStyleDisplay() {
    $("#conditionArea div ul li").each(function () {
        if ($(this).index() == 0) {
            $(this).addClass("Left");
        } else if ($(this).index() == $(this).parent().children().size() - 1) {
            $(this).addClass("Right");
        } else {
            $(this).addClass("Center");
        }
        // TODO:小数点以下要計算？
        $(this).width(((($(this).parent().width()) - ($(this).parent().children().size()) - 1) / ($(this).parent().children().size())) + "px");

        // 選択済みのものはグレー表示
        if ($(this).children(":nth-child(4)").val() == "True") {
            $(this).addClass("OnGrey");
        }
        //2018/07/10 TCS 河原 TKM Next Gen e-CRB Project Application development Block B-1 START
        // 未選択かつ項目名変更アイテムの場合：項目名を初期化する
        if ($(this).children(":nth-child(4)").val() != "True" && $(this).children(":nth-child(5)").val() == "2") {
            // ItemTitleのマスタ値を取得
            var defaultItemTitle = $(this).children(":nth-child(7)")[0].value;

            // ItemTitleに、マスタ値の置換文字列("%1")を画面表示用の文字列で置換した結果を設定する
            $(this).children(":nth-child(1)")[0].innerText = defaultItemTitle.replace("%1", $("#ReplaceTxtItemTitle").val());
            
            // OtherSalesConditionの値をクリア
            $(this).children(":nth-child(6)")[0].value = "";

        }
        //2018/07/10 TCS 河原 TKM Next Gen e-CRB Project Application development Block B-1 END
    });
    return false;
}

//TKMローカル
//2018/07/10 TCS 河原 TKM Next Gen e-CRB Project Application development Block B-1 START

// 購入分類変更イベント
$("#demandStructureArea div ul li").live("click", function () {

    //編集モードの場合
    if ($("#salesConditionCurrentMode").css("display") == "none" && $("#salesConditionEditMode").css("display") == "block") {

        if ($(this).hasClass("OnBlue")) {
            // 選択状態なら、未選択状態とする
            $(this).removeClass("OnBlue");

            //選択情報を保存
            this_form.DemandStructureCd.value = "";
            this_form.TradeinEnabledFlg.value = "0";

        } else {
            // 別の項目を全て未選択状態とする
            $(this).parent().children().removeClass("OnBlue");
            // 未選択状態なら、選択状態とする
            $(this).addClass("OnBlue");
            $("#selects").click();

            //選択情報を保存
            this_form.DemandStructureCd.value = $(this).children("input:nth-child(2)").attr("value");
            this_form.TradeinEnabledFlg.value = $(this).children("input:nth-child(3)").attr("value");

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
    }
});


//下取車両メーカー
$(function () {
    //ポップアップオープン時のイベント
    $("#Trade_in_MakerTrigger").click(function () {
        if ($("#salesConditionCurrentMode").css("display") == "none" && $("#salesConditionEditMode").css("display") == "block" && this_form.TradeinEnabledFlg.value == "1") {
            $("#Trade_in_MakerUpdatePanel").empty();

            $("#processingServer").css("z-index", 1000000);
            $("#processingServer").css("top", ($("#Trade_in_MakerTrigger").attr("offsetHeight") / 2 + $("#Trade_in_MakerTrigger").offset().top) - 10 + "px");
            $("#processingServer").css("left", ($("#Trade_in_MakerTrigger").attr("offsetWidth") / 2 + $("#Trade_in_MakerTrigger").offset().left) + 200 + "px");

            $("#registOverlayBlack").addClass("BGColor");
            $("#Trade_in_MakerButton").click();

            $(".nscListBoxSetIn li:last-child").addClass("end");
        } else {
            $("#Trade_in_MakerPopOver_popover").css("visibility", "hidden");
        }
    });

    //選択時のイベント
    $(".Trade_in_Makerlist").live("click", function (e) {
        $("#Trade_in_Maker").html($(this).html());
        $(".Trade_in_Makerlist").removeClass("Selection");
        $(this).addClass("Selection");

        //異なるメーカーを選択した場合
        if (this_form.Trade_in_MakerValue.value != $(this).attr("id")) {
            this_form.Trade_in_ModelValue.value = "";
            $("#Trade_in_Model").html("");
        }

        this_form.Trade_in_MakerValue.value = $(this).attr("id");
        //2018/07/10 TCS 河原 TKM Next Gen e-CRB Project Application development Block B-1 START
        //編集→編集完了→編集→キャンセル の場合に、編集完了時点の状態に戻せるよう退避しておく
        this_form.Trade_in_MakerName.value = $(this).attr("title");
        //2018/07/10 TCS 河原 TKM Next Gen e-CRB Project Application development Block B-1 END

        $("#bodyFrame").trigger("click.popover");
    });

    $(".scNscStaffCancellButton").click(function () {
        $("#bodyFrame").trigger("click.popover");
    });
});


//下取車両モデル
$(function () {
    //ポップアップオープン時のイベント
    $("#Trade_in_ModelTrigger").click(function () {
        if ($("#salesConditionCurrentMode").css("display") == "none" && $("#salesConditionEditMode").css("display") == "block" && this_form.TradeinEnabledFlg.value == "1" && this_form.Trade_in_MakerValue.value != "") {
            $("#Trade_in_ModelUpdatePanel").empty();

            $("#processingServer").css("z-index", 1000000);
            $("#processingServer").css("top", ($("#Trade_in_ModelTrigger").attr("offsetHeight") / 2 + $("#Trade_in_ModelTrigger").offset().top) - 10 + "px");
            $("#processingServer").css("left", ($("#Trade_in_ModelTrigger").attr("offsetWidth") / 2 + $("#Trade_in_ModelTrigger").offset().left) + 200 + "px");

            $("#registOverlayBlack").addClass("BGColor");
            $("#Trade_in_ModelButton").click();

            $(".nscListBoxSetIn li:last-child").addClass("end");
        } else {
            $("#Trade_in_ModelPopOver_popover").css("visibility", "hidden");
        }
    });

    //選択時のイベント
    $(".Trade_in_Modellist").live("click", function (e) {
        $("#Trade_in_Model").html($(this).html());
        $(".Trade_in_Modellist").removeClass("Selection");
        $(this).addClass("Selection");

        this_form.Trade_in_ModelValue.value = $(this).attr("id");
        //2018/07/10 TCS 河原 TKM Next Gen e-CRB Project Application development Block B-1 START
        //編集→編集完了→編集→キャンセル の場合に、編集完了時点の状態に戻せるよう退避しておく
        this_form.Trade_in_ModelName.value = $(this).attr("title");
        //2018/07/10 TCS 河原 TKM Next Gen e-CRB Project Application development Block B-1 END

        $("#bodyFrame").trigger("click.popover");
    });

    $(".scNscStaffCancellButton").click(function () {
        $("#bodyFrame").trigger("click.popover");
    });
});

//下取車両走行距離
$(function () {
    //ポップアップオープン時のイベント
    $("#Trade_in_MileageTrigger").click(function () {
        if ($("#salesConditionCurrentMode").css("display") == "none" && $("#salesConditionEditMode").css("display") == "block" && this_form.TradeinEnabledFlg.value == "1") {
            $("#Trade_in_MileageInputText").val($("#Trade_in_MileageValue").val());
            openMileagePopup(this);
        }
    });
});

//下取車両走行距離入力ポップアップ表示
function openMileagePopup(target) {

    var top = $(target).offset().top - 170;

    var pop = $("#custDtlPage2 .scNsc51PopUpModelSelectArrowOther").width();
    var detail = 31;
    var left = $(target).offset().left + (target.offsetWidth / 2) - (pop / 2) - detail;

    hiddenId = $(target).children(":nth-child(6)").attr("id");

    targetLiTag = target;

    //テキスト更新
    $("#custDtlPage2 .scNsc51PopUpModelSelectArrowOther").css({ left: -10 });
    $("#ScNsc51OtherConditionInputText").CustomTextBox("updateText", $("#" + hiddenId).val())

    //表示
    $("#Trade_in_MileageInputPopup").css({ top: top, left: left }).fadeIn(300);
};

//下取車両年式
$(function () {
    //ポップアップオープン時のイベント
    $("#Trade_in_ModelYearTrigger").click(function () {
        if ($("#salesConditionCurrentMode").css("display") == "none" && $("#salesConditionEditMode").css("display") == "block" && this_form.TradeinEnabledFlg.value == "1") {
            $("#Trade_in_ModelYearUpdatePanel").empty();

            $("#processingServer").css("z-index", 1000000);
            $("#processingServer").css("top", ($("#Trade_in_ModelYearTrigger").attr("offsetHeight") / 2 + $("#Trade_in_ModelYearTrigger").offset().top) - 10 + "px");
            $("#processingServer").css("left", ($("#Trade_in_ModelYearTrigger").attr("offsetWidth") / 2 + $("#Trade_in_ModelYearTrigger").offset().left) + 200 + "px");

            $("#registOverlayBlack").addClass("BGColor");
            $("#Trade_in_ModelYearButton").click();

            $(".nscListBoxSetIn li:last-child").addClass("end");
        } else {
            $("#Trade_in_ModelYearPopOver_popover").css("visibility", "hidden");
        }
    });

    //選択時のイベント
    $(".Trade_in_ModelYearlist").live("click", function (e) {
        $("#Trade_in_ModelYear").html($(this).html());
        $(".Trade_in_ModelYearlist").removeClass("Selection");
        $(this).addClass("Selection");

        this_form.Trade_in_ModelYearValue.value = $(this).attr("id");

        $("#bodyFrame").trigger("click.popover");
    });
    $(".scNscStaffCancellButton").click(function () {
        $("#bodyFrame").trigger("click.popover");
    });
});

//下取車両走行距離ポップアップクローズの監視
$(document.body).bind("mousedown touchstart", function (e) {
    if ($("#Trade_in_MileageInputPopup").is(":visible") === false) return;
    if ($(e.target).is("#Trade_in_MileageInputPopup, #Trade_in_MileageInputPopup *") === false) {
        $("#Trade_in_MileageInputPopup").fadeOut(300);
    }
});

//下取車両走行距離ポップアップ閉じるボタン処理
$(".Trade_in_MileageCancelButton").live("click", function (e) {
    $("#Trade_in_MileageInputPopup").fadeOut(300);
});

//下取車両走行距離ポップアップの完了ボタンクリック
$(".Trade_in_MileageCompleteButton").live("click", function (e) {
    $("#Trade_in_Mileage").html($("#Trade_in_MileageInputText").val());
    $("#Trade_in_MileageValue").val($("#Trade_in_MileageInputText").val());
    $("#Trade_in_MileageInputPopup").fadeOut(300);
});


//購入分類関連の必須入力チェック
function checkDemandStructure() {

    var checkResult = ""

    //購入分類が入力済みかどうか
    if (this_form.DemandStructureCd.value == "") {
        checkResult = "1";
    } else {
        checkResult = "0";
    }

    //下取車両が入力可能な場合
    if (this_form.TradeinEnabledFlg.value == "1") {
        if (this_form.Trade_in_MakerValue.value == "") {
            checkResult = checkResult + "1"
        } else {
            checkResult = checkResult + "0"
        }
        if (this_form.Trade_in_ModelValue.value == "") {
            checkResult = checkResult + "1"
        } else {
            checkResult = checkResult + "0"
        }
        if (this_form.Trade_in_MileageValue.value == "") {
            checkResult = checkResult + "1"
        } else {
            checkResult = checkResult + "0"
        }
        if (this_form.Trade_in_ModelYearValue.value == "") {
            checkResult = checkResult + "1"
        } else {
            checkResult = checkResult + "0"
        }
    } else {
        checkResult = checkResult + "0000"
    }

    //チェックOK
    return checkResult;
}
//2018/07/10 TCS 河原 TKM Next Gen e-CRB Project Application development Block B-1 END
