/** 
* @fileOverview SC3100104 リマインダー画面内の処理
* 
* @author t.nagata
* @version 1.0.0
*/

// ==============================================================
// 定数
// ==============================================================
// 処理タイプ
var C_SC3290104_ACTION_TYPE_REGISTER = 1;               // 登録ボタン押下
var C_SC3290104_ACTION_TYPE_CONCURRENCY = 3;            // 排他エラー発生


// ==============================================================
// DOMロード時処理
// ==============================================================
$(function () {

    // 読み込み中アイコンの表示
    showLoadingSC3290102();

    // 共通イベント定義
    $('#SC3290102_PreButtonRow').setCommonEvent();
    $('#SC3290102_NextButtonRow').setCommonEvent();
    $('.SC3290102_table02 tbody tr.SC3290102_FollwPopup').setCommonEvent();

    // 前のN件ボタンタップ
    $('#SC3290102_PreButtonRow').live('tap', function (aEvent, aTarget) {

        // 読み込み中アイコンの表示
        $('#SC3290102_Panel div.PreButton').hide();
        $('#SC3290102_Panel div.PreLoad').show();
        $('#SC3290102_Panel div.DisabledDiv').show();

        // サーバサイド処理の呼び出し
        $('#SC3290102_HidePreButton').click();
    });

    // 次のN件ボタンタップ
    $('#SC3290102_NextButtonRow').live('tap', function (aEvent, aTarget) {

        // 読み込み中アイコンの表示
        $('#SC3290102_Panel div.NextButton').hide();
        $('#SC3290102_Panel div.NextLoad').show();
        $('#SC3290102_Panel div.DisabledDiv').show();

        // サーバサイド処理の呼び出し
        $('#SC3290102_HideNextButton').click();
    });

    // 行タップ
    $('.SC3290102_table02 tbody tr.SC3290102_FollwPopup').live('tap', function (aEvent, aTarget) {
        $(this).trigger('showPopover');
    });

    // サーバサイド処理の呼び出し
    $('#SC3290102_LoadSpinButton').click();
});


// ==============================================================
// 関数定義
// ==============================================================

/**
 * 読み込み中アイコンを表示する
 *
 * @param  {-} -
 * @return {-} -
 */
function showLoadingSC3290102() {

    // オーバーレイ表示
    $("#SC3290102_ProgressPanel").show();
    $("#SC3290102_LoadingAnimation2").show();
}

/**
 * 読み込み中アイコンを非表示にする
 *
 * @param  {-} -
 * @return {-} -
 */
function closeLoadingSC3290102() {

    // オーバーレイ非表示
    $("#SC3290102_ProgressPanel").hide();
    $("#SC3290102_LoadingAnimation2").hide();
}

/**
 * 描画エリアの初期化処理を行う
 *
 * @param  {-} -
 * @return {-} -
 */
function initDisplaySC3290102() {

    // フォロー中の件数
    var items = parseInt($('#SC3290102_ItemsField').val());

    // 最大表示件数
    var maxItems = parseInt($('#SC3290102_MaxItemsField').val());

    // 取得開始行番号
    var getBeginLine = parseInt($('#SC3290102_GetBeginLineField').val());

    // 取得終了行番号
    var getEndLine = parseInt($('#SC3290102_GetEndLineField').val());

    // ページングボタンの表示／非表示
    $('#SC3290102_Panel div.PreLoad').hide();
    $('#SC3290102_Panel div.NextLoad').hide();
    $('#SC3290102_PreButtonRow').hide();
    $('#SC3290102_NextButtonRow').hide();
    $('#SC3290102_FollwListUpdatePanel').show();
    closeLoadingSC3290102();

    if (items <= 0) {

        $('#SC3290102_ItemNothing').show();

    } else {

        $('#SC3290102_table02Body').show();
        $('#SC3290102_ItemNoting').hide();

        // 前のN件ボタン
    	if (1 < getBeginLine) {
    	    $('#SC3290102_PreButtonRow').show();
    	}
    	
    	// 次のN件ボタン
    	if (getEndLine < items) {
    	    $('#SC3290102_NextButtonRow').show();
    	}

        // スクロールの設定
    	$('#SC3290102_table02Body').fingerScroll();

    	// フォロー設定ポップアップのイベント設定
    	setPopOverSC3290104($('.SC3290102_table02 tbody tr.SC3290102_FollwPopup'), -50, -45, false, true, true, true);
    }
    

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
    
    // 登録ボタン押下、又は排他エラー発生の場合
    if (aActionType == C_SC3290104_ACTION_TYPE_REGISTER || aActionType == C_SC3290104_ACTION_TYPE_CONCURRENCY) {

        // 読み込み中アイコンの表示
        showLoadingSC3290102();

        // サーバサイド処理の呼び出し
        $('#SC3290102_LoadSpinButton').click();
    }
}
