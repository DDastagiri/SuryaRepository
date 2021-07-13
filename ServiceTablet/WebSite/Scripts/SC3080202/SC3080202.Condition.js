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
        }
    );

    // 完了ボタン押下イベント
    $("#salesConditionCompleteButton").live("click",
        function () {
            // Hidden項目設定
            $("#conditionArea div ul li").each(function () {
                if ($(this).hasClass("OnBlue")) {
                    $(this).children(":nth-child(4)").val("True");
                } else {
                    $(this).children(":nth-child(4)").val("False");
                    if ($(this).children(":nth-child(5)").val() == "1") {
                        $(this).children(":nth-child(6)").val("");
                    }
                }

            });
            // 編集不可
            $("#salesConditionCurrentMode").show();
            $("#salesConditionEditMode").hide();
            // 全項目、初期表示
            $("#conditionArea div ul li").removeClass("OnBlue");
            $("#conditionArea div ul li").removeClass("OnGrey");
            conditionEventStyleDisplay();

            $("#salesConditionCompleteButtonDummy").click();
        }
    );

    // 項目変更イベント
    $("#conditionArea div ul li").live("click", function () {

        if ($("#OtherConditionInputPopup").is(":visible") === true) return;

        //編集モードの場合
        if ($("#salesConditionCurrentMode").css("display") == "none") {

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
        var left = $(target).offset().left - 130;
        hiddenId = $(target).children(":nth-child(6)").attr("id");

        targetLiTag = target;

        //テキスト更新
        $("#ScNsc51OtherConditionInputText").CustomTextBox("updateText", $("#" + hiddenId).val())
        //表示
        $("#OtherConditionInputPopup").css({ top: top, left: left }).fadeIn(300);
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
    });
    return false;
}