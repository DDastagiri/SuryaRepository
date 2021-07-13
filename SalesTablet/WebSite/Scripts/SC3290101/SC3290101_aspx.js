/** 
* @fileOverview 異常リストの処理を記述するファイル.
* 
* @author y.gotoh
* @version 1.0.0
*/
// ==============================================================
// 定数
// ==============================================================
// 担当未振当て処理タイプ
var C_SC3290101_FURIATE_STAFF_NOT_TYPE_NOTHING = 0;  // 処理なし
var C_SC3290101_FURIATE_STAFF_NOT_TYPE_CUST = 1;     // 顧客担当未振当てのみ
var C_SC3290101_FURIATE_STAFF_NOT_TYPE_ACTIVITY = 2; // 活動担当未振当てのみ
var C_SC3290101_FURIATE_STAFF_NOT_TYPE_BOTH = 3;     // 顧客担当未振当て、活動担当未振当て両方あり

// ==============================================================
// DOMロード時処理
// ==============================================================
$(function () {

    // 共通イベント定義
    $('#SC3290101_table02Body tr td #SC3290101_IrregularityItem').setCommonEvent();

    // 行タップ
    $('#SC3290101_table02Body tr td #SC3290101_IrregularityItem').live('tap', function (aEvent, aTarget) {

        var irregClassCd = $(this).parent().children("#IrregClassCd").val();
        var irregItemCd = $(this).parent().children("#IrregItemCd").val();

        if (irregClassCd == "00") {
            //目標未達の場合、異常詳細画面へ遷移
            window.parent.moveMainFrame("1", irregClassCd, irregItemCd);
        }
        else if (irregClassCd == "20" || irregClassCd == "30") {
            //計画異常または活動遅れの場合、SPMフレームへ遷移
            window.parent.moveMainFrame("2", irregClassCd, irregItemCd);
        }
    });

    // サーバサイド処理の呼び出し
    $('#SC3290101_LoadSpinButton').click();
});


// ==============================================================
// 関数定義
// ==============================================================
/**
* 読み込み中アイコンを非表示にする
*
* @param  {-} -
* @return {-} -
*/
function closeLoadingSC3290101() {

    // ロード時対応
    if ($("#SC3290101_ProgressPanel", parent.document).css('display') == 'none') {
        setTimeout('closeLoadingSC3290101()', 1000);
    } else {
        // オーバーレイ非表示
        $("#SC3290101_ProgressPanel", parent.document).hide();
        $("#SC3290101_LoadingAnimation", parent.document).hide();
    }
}

/**
 * 描画エリアの初期化処理を行う
 *
 * @param  {-} -
 * @return {-} -
 */
function initDisplaySC3290101(){

    // 読み込み中アイコンの非表示
    closeLoadingSC3290101();

    // 異常項目がない場合は、メッセージを表示
    if ($('#SC3290101_table02Body').find('tr').length == 0) {
        $('#SC3290101_ItemNothing', parent.document).show();
        return;
    }

    //iframeを表示
    $('#SC3290101_iframe', parent.document).show();

    // スクロールの設定
    $('#SC3290101_table02Body').fingerScroll();

    // 更新日時が取得できている場合、表示する
    if ($('#SC3290101_TempLastUpdateTime').val() != "") {
        $('#SC3290101_DateBox', parent.document).show();
        $('#SC3290101_LastUpdateTime', parent.document).text($('#SC3290101_TempLastUpdateTime').val())
    }

    //担当未振当て処理タイプ取得
    var furiateStaffNotType = getFuriateStaffNotType();

    //顧客担当未振当て、活動担当未振当ての両方が存在しない場合、サーバーサイドの処理は呼び出さない
    if (C_SC3290101_FURIATE_STAFF_NOT_TYPE_NOTHING == furiateStaffNotType) {
        $('.SC3290101_LoadingAnimation2').hide();
        return;
    }

    $('#SC3290101_FuriateStaffNotType').val(furiateStaffNotType)

    //サーバーサイド処理を呼び出し（担当未振当て件数更新）
    $('#SC3290101_FuriateStaffNotUpdateButton').click();
}

/**
* 担当未振当て処理タイプを取得する
*
* @param  {-} -
* @return {String} 担当未振当て処理タイプ
*/
function getFuriateStaffNotType() {

    var hasStaffAssignToCustCountPanel = false;
    var hasUnallocatedActivityCountPanel = false;

    //顧客担当未振当て件数のパネルが存在するか判定
    if (0 < $('#SC3290101_StaffAssignToCustCountPanel').length) {

        hasStaffAssignToCustCountPanel = true;
    }
    //活動担当未振当て件数のパネルが存在するか判定
    if (0 < $('#SC3290101_UnallocatedActivityCountPanel').length) {

        hasUnallocatedActivityCountPanel = true;
    }

    if (hasStaffAssignToCustCountPanel && !hasUnallocatedActivityCountPanel) {
        //顧客担当未振当てのみあり
        return C_SC3290101_FURIATE_STAFF_NOT_TYPE_CUST
    }
    else if (!hasStaffAssignToCustCountPanel && hasUnallocatedActivityCountPanel) {
        //活動担当未振当てのみあり
        return C_SC3290101_FURIATE_STAFF_NOT_TYPE_ACTIVITY
    }
    else if (hasStaffAssignToCustCountPanel && hasUnallocatedActivityCountPanel) {
        //顧客担当未振当て、活動担当未振当て両方あり
        return C_SC3290101_FURIATE_STAFF_NOT_TYPE_BOTH
    }
    else {
        //どちらも存在しない
        return C_SC3290101_FURIATE_STAFF_NOT_TYPE_NOTHING;
    }
}

/**
* 活動担当未割当件数をセットする
*
* @param  {-} -
* @return {-} -
*/
function setFuriateStaffNotCount() {

    $('#SC3290101_StaffAssignToCustCount').text($('#SC3290101_TempStaffAssignToCustCount').text());
    $('#SC3290101_UnallocatedActivityCount').text($('#SC3290101_TempUnallocatedActivityCount').text());
    $('.SC3290101_LoadingAnimation2').hide();
}

