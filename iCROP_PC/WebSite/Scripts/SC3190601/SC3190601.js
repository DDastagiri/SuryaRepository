/** 
* @fileOverview B/O管理ボードの処理を記述するファイル.
* 
* @author M.Asano
* @version 1.0.0
* 更新： 
*/

// ==============================================================
// 定数
// ==============================================================
// 処理タイプ
var C_SC3190601_ACTION_TYPE_NONE = 0;       // 処理なし
var C_SC3190601_ACTION_TYPE_RELOAD = 1;     // 画面再描画

// ==============================================================
// 変数
// ==============================================================
// オートページング判定時間①
var gAutoPagingSpanFirst = 0;

// オートページング判定時間②
var gAutoPagingSpanSecond = 0;

// オートページングカウンタ
var gAutoPagingCounter = 0;

// ロックフラグ
var gLockFlag = false;

// 最大ページ番号
var gMaxPageCount = 0;

// 現在ページ番号
var gNowPageCount = 0;

// ==============================================================
// DOMロード時処理
// ==============================================================
$(function () {

    // オートページング判定時間取得
    gAutoPagingSpanFirst = parseInt($('#AutoPagingTimeFirstField').val());
    gAutoPagingSpanSecond = parseInt($('#AutoPagingTimeSecondField').val());

    // タイトル押下
    $('div.Title01').click(function () {

        // ローディング表示
        showLodingSC3190601();

        // 画面遷移
        $("#ScreenTransitionButton").click();
    });

    // ページャー押下
    $('div.PageIcon').click(function () {
        // 押下されたページャーのページ番号取得
        var pageIndex = parseInt($(this).attr("PageIndex"));

        // 現在表示されているページと違う場合
        if (gNowPageCount != pageIndex) {
            // 対象ページへ遷移
            SC3190601_Paging(pageIndex);
        }
    });

    // 次ページボタン押下
    $('div.NextPageIcon').click(function () {
        RedirectNextPage();
    });

    // 部品追加ボタン押下
    $('div.PlusBtn').live('click', function () {
        showPartsInputScreen(0);
    });

    // 部品リスト押下
    $('div.PartsListRowDiv').live('click', function () {
        // 対象のBO Idを取得
        var boid = $("#SC3190601_PartsList tr").eq(parseInt($(this).attr("RowIndex"))).find("#BoIdField").val();
        if (boid) {
            showPartsInputScreen(boid);
        }
    });

    // カウンタ処理
    setInterval(counter, 1000);
});

// ==============================================================
// 関数定義
// ==============================================================
/**
 * 初期表示処理を行う.
 * 
 * @return {-} -
 */
function showSC3190601() {
    SC3190601_Paging(1);
}

/**
* 初期表示後の画面初期化処理を行う.
* 
* @return {-} -
*/
function initSC3190601() {

    // 現在ページ番号及び最大ページ番号を取得
    gMaxPageCount = parseInt($('#MaxPageCount').val());
    gNowPageCount = parseInt($('#NowPageCount').val());

    // ページインジケータの設定
    setIndicator();

    // Current Status更新
    $("#SC3190601_Label_POTotal_Val").text($('#POTotalValField').val());
    $("#SC3190601_Label_PODelay").text($('#PODelayField').val());
    $("#SC3190601_Label_PSTotal_Val").text($('#PSTotalValField').val());
    $("#SC3190601_Label_PSDelay").text($('#PSDelayField').val());

    // カウンタのリセット
    resetCounter();

    // ローディング非表示
    closeLodingSC3190601();
}

/**
* インジケーターの設定を行う
*
* @param  {-} -
* @return {-} -
*/
function setIndicator() {

    // インジケーターの設定
    for (var i = 1, cnt = 4; i <= cnt; ++i) {

        //現在位置のインジケーターはONとする
        if (i <= gMaxPageCount) {

            //インジケーターは表示する
            $('div.Icon0' + i).css("display", "block");

            //全体ページ数以下のインジケーターは現在位置により分岐してセットする
            if (i == gNowPageCount) {
                //現在位置のインジケーターはONとする
                $('div.Icon0' + i).addClass("Active");
            }
            else {
                //インジケーターはOFFとする
                $('div.Icon0' + i).removeClass("Active");
            }
        } else {
            //全体ページ数より大きいインジケーターは非表示とする
            $('div.Icon0' + i).css("display", "none");
        }
    }

    // インジケーターの位置調整
    if (gMaxPageCount == 1) {
        $('div.PageIconbox').css("margin-left", "110px");
    }
    else if (gMaxPageCount == 2) {
        $('div.PageIconbox').css("margin-left", "70px");
    }
    else if (gMaxPageCount == 3) {
        $('div.PageIconbox').css("margin-left", "40px");
    }
    else {
        $('div.PageIconbox').css("margin-left", "0px");
    }

    // 次ページボタンの表示
    if (gMaxPageCount > 4) {
        // 最大ページ数が4ページより多い場合は、次ページボタンを表示する。
        $('div.PChangeRight').css("display", "block");
    }
    else {
        $('div.PChangeRight').css("display", "none");
    }
}

/**
* カウンタ処理を行う
*
* @param  {-} -
* @return {-} -
*/
function counter() {

    // ロック中の場合は処理を行わない。
    if (gLockFlag){
        return;
    }

    // 表示データが無い場合は処理を行わない。
    if (gNowPageCount == 0 || gMaxPageCount == 1) {
        return;
    }

    // 自動ページング時間が0の場合は処理を行わない。
    if (gAutoPagingSpanFirst == 0 || gAutoPagingSpanSecond == 0) {
        return;
    }

    // カウンタを加算
    gAutoPagingCounter++;

    // 現在のページが1ページ目の場合
    if (gNowPageCount == 1) {
        // カウンタが自動ページング時間①を超えていなければ何もしない
        if (gAutoPagingCounter <= gAutoPagingSpanFirst)
        {
            return;
        }

        // 超えている場合は、2ページ目へ遷移
        SC3190601_Paging(2);
    }
    else {
        // 現在のページが2ページ目以降の場合
        // カウンタが自動ページング時間②を超えていなければ何もしない
        if (gAutoPagingCounter <= gAutoPagingSpanSecond) {
            return;
        }

        // 超えている場合、次ページへの遷移
        RedirectNextPage();
    }
}

/**
* カウンタのクリア処理を行う
*
* @param  {-} -
* @return {-} -
*/
function resetCounter(){
    gAutoPagingCounter = 0;
}

/**
* 次ページへの遷移処理を行う
*
* @param  {-} -
* @return {-} -
*/
function RedirectNextPage() {
    if (gMaxPageCount == gNowPageCount) {
    　　// 現在のページが最終ページの場合、1ページ目へ
        SC3190601_Paging(1);
    }
    else {
        // 現在のページが最終ページ以外の場合、次ページへ
        SC3190601_Paging(gNowPageCount + 1);
    }
}

/**
* ページング処理を行う
*
* @param  {Integer} aPageNumber ページ番号
* @return {-} -
*/
function SC3190601_Paging(aPageNumber) {
    // ページ番号更新
    $('#NowPageCount').val(aPageNumber);

    // ローディング表示
    showLodingSC3190601();

    // ページング処理
    $("#PagingButton").click();
}

/**
* 読み込み中アイコンを表示する
*
* @param  {-} -
* @return {-} -
*/
function showLodingSC3190601() {
    $("#SC3190601_LoadingScreen").show();
}

/**
* 読み込み中アイコンを非表示にする
*
* @param  {-} -
* @return {-} -
*/
function closeLodingSC3190601() {
    $("#SC3190601_LoadingScreen").hide();
}

/**
* オーバーレイを表示する
*
* @param  {-} -
* @return {-} -
*/
function showOverlaySC3190601() {
    $("#SC3190601_Overlay").show();
}

/**
* オーバーレイを非表示にする
*
* @param  {-} -
* @return {-} -
*/
function closeOverlaySC3190601() {
    $("#SC3190601_Overlay").hide();
}

/**
* B/O部品入力画面を開く処理.
* 
* @param {Integer} aBoId B/O ID
* @return {-} -
*/
function showPartsInputScreen(aBoId) {

    // 画面ロック
    gLockFlag = true;

    // オーバーレイを表示
    showOverlaySC3190601();

    // B/O部品入力画面表示
    showSC3190602(aBoId);
}

/**
* B/O部品入力画面を閉じた場合の処理.
* 
* @param {String} aActionType 処理タイプ（0:処理なし、1:画面再描画）
* @return {-} -
*/
function closeSC3190602(aActionType) {

    // オーバーレイを消す
    closeOverlaySC3190601();

    // ロックを解除する
    gLockFlag = false;

    if (aActionType == C_SC3190601_ACTION_TYPE_RELOAD) {
 
        // 画面再描画の場合、再描画し1ページ目を表示
        SC3190601_Paging(1);
    }
}
