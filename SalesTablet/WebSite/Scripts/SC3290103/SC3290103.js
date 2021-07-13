/** 
* @fileOverview 異常詳細画面の処理を記述するファイル.
* 
* @author y.gotoh
* @version 1.0.0
*/
// ==============================================================
// 定数
// ==============================================================
// 処理タイプ
var C_SC3290103_ACTION_TYPE_REGISTER = 1;          // 登録ボタン押下
var C_SC3290103_ACTION_TYPE_CONCURRENCY = 3;       // 排他エラー発生

//フォローフラグ
var C_SC3290103_FLLW_FLG_OFF = 0;                  // OFF
var C_SC3290103_FLLW_FLG_ON = 1;                   // ON

//フォロー完了フラグ
var C_SC3290103_FLLW_COMPLETE_FLG_NOTCOMPLETE = 0; // 未完了
var C_SC3290103_FLLW_COMPLETE_FLG_COMPLETE = 1;    // 完了


// ==============================================================
// 変数
// ==============================================================
// 選択した行のIndex
var gSelectIndex;


// ==============================================================
// DOMロード時処理
// ==============================================================
$(function () {
    showSC3290103();
});


// ==============================================================
// 関数定義
// ==============================================================
/**
 * 異常詳細一覧の表示を行う.
 * 
 * @return {-} -
 */
function showSC3290103() {

    // 読み込み中アイコンの表示
    showLodingSC3290103();

    // 共通イベント定義
    $('#table02 tbody tr td.Confirmation').setCommonEvent();

    // タップ
    $('#table02 tbody tr td.Confirmation').live('tap', function (aEvent, aTarget) {

        //選択された行のインデックスを保持
        gSelectIndex = $('#table02 tbody tr td.Confirmation').index(this);

        var loginStaffCode = $('#LoginStaffCode').val();
        var fllwPicStfCd = $('#table02 tbody tr td.Confirmation #FllwPicStfCd').eq(gSelectIndex).val();

        //他のMGRがフォロー中の場合、アラートを表示
        if (fllwPicStfCd != "" && loginStaffCode != fllwPicStfCd) {

            var alertMessage = $('#AlertMessage').val();

            setTimeout(function () { alert(alertMessage) }, 100);
        }
        else {
            // フォロー設定ポップアップを開く
            $(this).trigger('showPopover');
        }
    });

    // サーバサイド処理の呼び出し
    $('#SC3290103_Panel #SC3290103_LoadSpinButton').click();
}

/**
 * 読み込み中アイコンを表示する
 *
 * @param  {-} -
 * @return {-} -
 */
function showLodingSC3290103() {
    $(".MstPG_LoadingScreen").show();
}

/**
 * 読み込み中アイコンを非表示にする
 *
 * @param  {-} -
 * @return {-} -
 */
function closeLodingSC3290103() {
    $(".MstPG_LoadingScreen").hide();
}

/**
 * 異常詳細画面表示完了後処理
 * 
 * @param  {-} -
 * @return {-} -
 */
function showCompleteSC3290103() {

    // スクロールの設定
    $('#TBL_Box02').fingerScroll();

    // フォロー設定ポップアップのイベント設定
    setPopOverSC3290104($('#table02 tbody tr td.Confirmation'), 0, -47, true, false, true, true);

    // 読み込み中アイコンを非表示
    closeLodingSC3290103();
}

/**
 * フォロー設定ポップアップを閉じた場合の処理.
 * 
 * @param {String} aActionType 処理タイプ（0:処理なし、1:登録ボタン押下、2:キャンセルボタン押下、3:排他エラー発生）
 * @param {String} aIrregFllwId 異常フォローID
 * @param {String} aIrregClassCd 異常分類コード
 * @param {String} aIrregItemCd 異常項目コード
 * @param {String} aStfCd スタッフコード
 * @param {String} aFllwFlg フォローフラグ（0:OFF、1:ON）
 * @param {String} aFllwCompleteFlg フォロー完了フラグ（0:未完了、1:完了）
 * @param {String} aFllwExprDate フォロー期日（YYYYMMDD形式）
 * @return {-} -
 */
function closeSC3290104(aActionType, aIrregFllwId, aIrregClassCd, aIrregItemCd, aStfCd, aFllwFlg, aFllwCompleteFlg, aFllwExprDate) {

    //フォロー設定画面で変更があった場合
    if (aActionType == C_SC3290103_ACTION_TYPE_REGISTER) {
        var selectTr = $('#table02 tbody tr').eq(gSelectIndex);
        var selectTd = $('#table02 tbody tr td.Confirmation div#ConfirmationDiv').eq(gSelectIndex);

        //フォロー完了した場合
        if (aFllwCompleteFlg == C_SC3290103_FLLW_COMPLETE_FLG_COMPLETE) {
            selectTr.removeClass("WhiteBKDT");
            selectTr.addClass("GrayBKDT");
            selectTd.html("<div id=\"MgrCheck\" class=\"MGR_Check Cheked\"></div>");

            //フォロー設定をONにした場合
        }
        else if (aFllwFlg == C_SC3290103_FLLW_FLG_ON) {
            selectTr.removeClass("GrayBKDT");
            selectTr.addClass("WhiteBKDT");
            var displayFllwExprDate = aFllwExprDate.substr(6, 2) + "/" + aFllwExprDate.substr(4, 2);
            selectTd.html("<div id=\"MgrButton01\" class=\"MGR_Button01\"><span id=\"FollowDate\">" + displayFllwExprDate + "</span></div>");

            //フォロー設定をOFFにした場合
        }
        else if (aFllwFlg == C_SC3290103_FLLW_FLG_OFF) {
            selectTr.removeClass("GrayBKDT");
            selectTr.addClass("WhiteBKDT");
            selectTd.html("<div id=\"MgrCheck\" class=\"MGR_Check\"></div>");
        }

    //排他制御が発生した場合
    }
    else if (aActionType == C_SC3290103_ACTION_TYPE_CONCURRENCY) {

        // 読み込み中アイコンの表示
        showLodingSC3290103();

        // サーバサイド処理の呼び出し
        $('#SC3290103_Panel #SC3290103_LoadSpinButton').click();
    };
}
