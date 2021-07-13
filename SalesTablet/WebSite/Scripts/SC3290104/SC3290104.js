/** 
 * @fileOverview フォロー設定の処理を記述するファイル.
 * 
 * @author t.mizumoto
 * @version 1.0.0
 */
// ==============================================================
// 定数
// ==============================================================
// 処理タイプ
var C_SC3290104_ACTION_TYPE_NOTHING = 0;                // 処理なし
var C_SC3290104_ACTION_TYPE_REGISTER = 1;               // 登録ボタン押下
var C_SC3290104_ACTION_TYPE_CANCEL = 2;                 // キャンセルボタン押下
var C_SC3290104_ACTION_TYPE_CONCURRENCY = 3;            // 排他エラー発生

// フォロー完了フラグ
var C_SC3290104_FLLW_COMPLETE_FLG_COMPLETE = "1";       // 完了
var C_SC3290104_FLLW_COMPLETE_FLG_NOT_COMPLETE = "0";   // 未完了

// フォロー設定フラグ
var C_SC3290104_FLLW_FLG_ON = "1";                      // ON
var C_SC3290104_FLLW_FLG_OFF = "0";                     // OFF

// DB初期値
var C_SC3290104_DB_DEFAULT_VALUE_STRING = " ";          // 文字列


// ==============================================================
// DOMロード時処理
// ==============================================================
$(function () {

    // 登録ボタン押下
    $('#SC3290104_RegistButton').click(function () {

        // フォロー期日を受け渡し用のhiddenフィールドに設定
        $("#SC3290104_FllwExprDateDummy").val($("#SC3290104_FllwExprDate").val());

        // 読み込み中アイコン表示
        $("#SC3290104_ContentOverlayBlack").css("opacity", "0");
        showLodingSC3290104();

        $('#SC3290104_RegisterButton').click();
    });

    // キャンセルボタン押下
    $('#SC3290104_CancelButton').click(function () {

        $('#SC3290104_ActionType').val(C_SC3290104_ACTION_TYPE_CANCEL);
        closePopOverSC3290104();
    });

    // フォロー期日エリア
    $("#SC3290104_Panel .ListArrow").live("click", function () {
        // フォロー期日にフォーカスを設定する
        $("#SC3290104_FllwExprDate").focus();
    });

    // フォロー期日
    $("#SC3290104_FllwExprDate").live("change", function () {
        // フォロー期日の背景色を変更する
        changeFllwExprDateBackground();
    });

    // メモ
    $("#SC3290104_FllwMemo").live("change", function () {

        // メールボタンの活性・非活性を変更する
        changeMailBtnEnable();
    });

    // メモ消去ボタン押下
    $("#SC3290104_ClearMemoDiv").live("click", function () {

        // ボタンが有効な場合
        if ($(this).hasClass("ClearBtn02")) {
            $("#SC3290104_FllwMemo").val("");
            $("#SC3290104_Mail").removeClass("MemoBtnColor");
        }
    });

    // メールボタン押下
    $("#SC3290104_Mail").live("click", function () {

        // ボタンが有効な場合
        if ($(this).hasClass("MemoBtnColor")) {

            icrop.clientapplication.sendSimpleMail({
                Subject: $("#SC3290104_MailTitle").val(),
                Message: $("#SC3290104_FllwMemo").val()
            });

            $(this).removeClass("MemoBtnColor");
        }
    });

    // 完了ボタン押下
    $("#SC3290104_FllwCompleteFlgButtonDiv").live("click", function () {

        // フォロー完了の場合
        if ($("#SC3290104_FllwCompleteFlg").val() == C_SC3290104_FLLW_COMPLETE_FLG_COMPLETE) {
            $("#SC3290104_FllwCompleteFlg").val(C_SC3290104_FLLW_COMPLETE_FLG_NOT_COMPLETE);
        }
        else {
            $("#SC3290104_FllwCompleteFlg").val(C_SC3290104_FLLW_COMPLETE_FLG_COMPLETE);
        }

        // 各コントローラの活性・非活性を設定する
        setControlDisabled();
    });

});


// ==============================================================
// 関数定義
// ==============================================================
/**
 * フォロー設定ポップアップの設定を行う.
 * 
 * @param  {Object} aElement イベント設定対象のオブジェクト
 * @param  {String} aOffsetX X座標の表示位置の調整px
 * @param  {String} aOffsetY Y座標の表示位置の調整px
 * @param  {String} aPreventLeft 表示位置（左）
 * @param  {String} aPreventRight 表示位置（右）
 * @param  {String} aPreventTop 表示位置（上）
 * @param  {String} aPreventBottom 表示位置（下）
 * @return {-} -
 */
function setPopOverSC3290104(aElement, aOffsetX, aOffsetY, aPreventLeft, aPreventRight, aPreventTop, aPreventBottom) {
    
    // ポップオーバー
    aElement.popoverEx({
        contentId: $("#SC3290104_Panel")
        , openEvent: function (button) {
            var irregFllwId = C_SC3290104_DB_DEFAULT_VALUE_STRING;
            var irregClassCd = C_SC3290104_DB_DEFAULT_VALUE_STRING;
            var irregItemCd = C_SC3290104_DB_DEFAULT_VALUE_STRING;
            var stfCd = C_SC3290104_DB_DEFAULT_VALUE_STRING;

            if (0 < button.find('#IrregFllwId').length) {
                irregFllwId = button.find('#IrregFllwId').val();
            }
            else {

                if (0 < button.find('#IrregClassCd').length) {
                    irregClassCd = button.find('#IrregClassCd').val();
                }
                if (0 < button.find('#IrregItemCd').length) {
                    irregItemCd = button.find('#IrregItemCd').val();
                }
                if (0 < button.find('#StfCd').length) {
                    stfCd = button.find('#StfCd').val();
                }
            }
            // フォロー設定ポップアップの表示を行う
            showSC3290104(irregFllwId, irregClassCd, irregItemCd, stfCd);
        }
        , padding: 70
        , offsetX: aOffsetX
        , offsetY: aOffsetY
        , preventLeft: aPreventLeft
        , preventRight: aPreventRight
        , preventTop: aPreventTop
        , preventBottom: aPreventBottom
    });

}

/**
 * フォロー設定ポップアップの表示を行う.
 * 
 * @param  {String} aIrregFllwId 異常フォローID
 * @param  {String} aIrregClassCd 異常分類コード
 * @param  {String} aIrregItemCd 異常項目コード
 * @param  {String} stfCd スタッフコード
 * @return {-} -
 */
function showSC3290104(aIrregFllwId, aIrregClassCd, aIrregItemCd, stfCd) {

    // 読み込み中アイコンの表示
    $("#SC3290104_ContentOverlayBlack").css("opacity", "1");
    showLodingSC3290104();

    // 処理タイプの初期化
    $('#SC3290104_ActionType').val(C_SC3290104_ACTION_TYPE_NOTHING);

    $('#SC3290104_IrregFllwId').val(aIrregFllwId);
    $('#SC3290104_IrregClassCd').val(aIrregClassCd);
    $('#SC3290104_IrregItemCd').val(aIrregItemCd);
    $('#SC3290104_StfCd').val(stfCd);

    // サーバサイド処理の呼び出し
    $('#SC3290104_LoadSpinButton').click();
}

/**
 * フォロー設定登録完了処理.
 * 
 * @param  {-} -
 * @return {-} -
 */
function registerCompleteSC3290104() {

    // フォロー期日を受け渡し用のhiddenフィールドから復元する
    $("#SC3290104_FllwExprDate").val($("#SC3290104_FllwExprDateDummy").val());

    $('#SC3290104_ActionType').val(C_SC3290104_ACTION_TYPE_REGISTER);
    closePopOverSC3290104();
}

/**
 * フォロー設定ポップアップを閉じる処理.
 * 
 * @param  {-} -
 * @return {-} -
 */
function closePopOverSC3290104() {

    $("#bodyFrame").trigger("click.popover");

    var actionType = $('#SC3290104_ActionType').val();
    var irregFllwId = $('#SC3290104_IrregFllwId').val();
    var irregClassCd = $('#SC3290104_IrregClassCd').val();
    var irregItemCd = $('#SC3290104_IrregItemCd').val();
    var stfCd = $('#SC3290104_StfCd').val();

    var fllwFlg = C_SC3290104_FLLW_FLG_OFF;
    if ($("#SC3290104_FllwFlg").is(':checked')) {
        fllwFlg = C_SC3290104_FLLW_FLG_ON;
    }

    var fllwCompleteFlg = $('#SC3290104_FllwCompleteFlg').val();
    var fllwExprDate = changeStringToDateIcrop($('#SC3290104_FllwExprDate').val())
    var fllwExprDateYYYYMMDD = dateToStringYYYYMMDD(fllwExprDate);


    // 呼び出し元画面に結果を通知する
    closeSC3290104(actionType, irregFllwId, irregClassCd, irregItemCd, stfCd, fllwFlg, fllwCompleteFlg, fllwExprDateYYYYMMDD);
}

/**
 * 読み込み中アイコンを表示する
 *
 * @param  {-} -
 * @return {-} -
 */
function showLodingSC3290104() {

    // オーバーレイ表示
    $("#SC3290104_RegistOverlayBlack").show();
    $("#SC3290104_ContentOverlayBlack").show();
    $("#SC3290104_ProcessingServer").show();
}

/**
 * 読み込み中アイコンを非表示にする
 *
 * @param  {-} -
 * @return {-} -
 */
function closeLodingSC3290104() {

    // オーバーレイ非表示
    $("#SC3290104_RegistOverlayBlack").hide();
    $("#SC3290104_ContentOverlayBlack").hide();
    $("#SC3290104_ProcessingServer").hide();
}

/**
 * 描画エリアの初期化処理を行う
 *
 * @param  {-} -
 * @return {-} -
 */
function initDisplaySC3290104() {

    // 各コントローラの活性・非活性を設定する
    setControlDisabled();
}

/**
 * 各コントローラの活性・非活性を設定する
 *
 * @param  {-} -
 * @return {-} -
 */
function setControlDisabled() {

    // フォロー設定の初期化
    $("#SC3290104_FllwFlg").SwitchButton({
        onLabel: "ON",
        offLabel: "OFF",
        check: function (value) {
            // 各コントローラの活性・非活性を設定する
            setControlDisabled();
        }
    });

    $('#SC3290104_Panel .icrop-SwitchButton').addClass("FollowFlg");

    // フォロー設定がOnの場合
    if ($("#SC3290104_FllwFlg").is(':checked')) {

        $("#SC3290104_FllwFlg").SwitchButton("disabled", true);

        // フォロー完了の場合
        if ($("#SC3290104_FllwCompleteFlg").val() == C_SC3290104_FLLW_COMPLETE_FLG_COMPLETE) {

            $("#SC3290104_FllwExprDate").attr("disabled", "disabled");
            $("#SC3290104_FllwMemo").attr("disabled", "disabled");
            $("#SC3290104_ClearMemoDiv").removeClass("ClearBtn02").addClass("ClearBtn01");
            $("#SC3290104_Mail").removeClass("MemoBtnColor");
        }
        else {

            $("#SC3290104_FllwExprDate").removeAttr("disabled");
            $("#SC3290104_FllwMemo").removeAttr("disabled");
            $("#SC3290104_ClearMemoDiv").removeClass("ClearBtn01").addClass("ClearBtn02");

            // フォロー期日の背景色を変更する
            changeFllwExprDateBackground();

            // メールボタンの活性・非活性を変更する
            changeMailBtnEnable();
        }
    }
    // フォロー設定がOffの場合
    else {

        $("#SC3290104_FllwFlg").SwitchButton("disabled", false);

        $("#SC3290104_FllwExprDate").attr("disabled", "disabled");
        $("#SC3290104_FllwMemo").attr("disabled", "disabled");
        $("#SC3290104_ClearMemoDiv").removeClass("ClearBtn02").addClass("ClearBtn01");
        $("#SC3290104_Mail").removeClass("MemoBtnColor");
    }

    // フォロー完了ボタンの文言を変更する
    changeFllwCompleteFlgButton();
}

/**
 * メッセージ表示処理を行う
 *
 * @param  {String} aMessage メッセージ
 * @return {-} -
 */
function showMessageBoxSC3290104(aMessage) {

    // フォロー期日を受け渡し用のhiddenフィールドから復元する
    $("#SC3290104_FllwExprDate").val($("#SC3290104_FllwExprDateDummy").val());

    alert(aMessage);

    // 各コントローラの活性・非活性を設定する
    setControlDisabled();
}

/**
 * エラーメッセージを出力し排他エラー処理を行う
 *
 * @param  {String} aMessage メッセージ
 * @return {-} -
 */
function showMessageBoxAndConcurrencySC3290104(aMessage) {

    // フォロー期日を受け渡し用のhiddenフィールドから復元する
    $("#SC3290104_FllwExprDate").val($("#SC3290104_FllwExprDateDummy").val());

    alert(aMessage);

    $('#SC3290104_ActionType').val(C_SC3290104_ACTION_TYPE_CONCURRENCY);
    closePopOverSC3290104();
}

/**
 * フォロー期日の背景色を変更する
 *
 * @param  {-} -
 * @return {-} -
 */
function changeFllwExprDateBackground() {

    // フォロー期日が入力されている場合
    if ($("#SC3290104_FllwExprDate").val() != "") {

        var fllwExprDate = changeStringToDateIcrop($('#SC3290104_FllwExprDate').val())
        var nowDate = changeStringToDateIcrop($('#SC3290104_NowDate').val())

        // フォロー期日が過去の場合
        if (fllwExprDate < nowDate) {
            $("#SC3290104_FllwExprDate").addClass("required-error-background");
        }
        else {
            $("#SC3290104_FllwExprDate").removeClass("required-error-background");
        }
    }
    else {
        $("#SC3290104_FllwExprDate").removeClass("required-error-background");
    }

}

/**
 * メールボタンの活性・非活性を変更する
 *
 * @param  {-} -
 * @return {-} -
 */
function changeMailBtnEnable() {
    if (0 < $("#SC3290104_FllwMemo").val().length) {
        $("#SC3290104_Mail").addClass("MemoBtnColor");
    }
    else {
        $("#SC3290104_Mail").removeClass("MemoBtnColor");
    }
}

/**
 * フォロー完了ボタンの文言を変更する
 *
 * @param  {-} -
 * @return {-} -
 */
function changeFllwCompleteFlgButton() {

    // フォロー完了の場合
    if ($("#SC3290104_FllwCompleteFlg").val() == C_SC3290104_FLLW_COMPLETE_FLG_COMPLETE) {
        $("#SC3290104_FllwCompleteFlgButton").text($("#SC3290104_FllwCompleteWord").val());
    }
    else {
        $("#SC3290104_FllwCompleteFlgButton").text($("#SC3290104_FllwNotCompleteWord").val());
    }
}

/**
 * DateTimeSelectorのValue値を日付型に変換する
 *
 * @param  {String} dateValue DateTimeSelectorのValue値
 * @return {Date} 変換後の値
 */
function changeStringToDateIcrop(dateValue) {

    if (dateValue == null || dateValue == "") {
        return null;
    }

    var strDate = String(dateValue);
    strDate = strDate.replace(/-/g, '/');
    strDate = strDate.replace('T', ' ');

    return new Date(Date.parse(strDate));
}

/**
 * Date型の値をYYYYMMDD形式に変換する
 *
 * @param  {Date} value Date型の値
 * @return {String} 変換後の値
 */
function dateToStringYYYYMMDD(value) {

    if (value == null) {
        return "";
    }

    var year = value.getFullYear();
    var month = value.getMonth() + 1;
    var day = value.getDate();

    if (month < 10) {
        month = '0' + month;
    }
    if (day < 10) {
        day = '0' + day;
    }

    return (year + '') + month + day;  
    
}

