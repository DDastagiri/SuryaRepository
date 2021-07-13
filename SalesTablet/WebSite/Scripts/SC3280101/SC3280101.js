/** 
* @fileOverview 納車時説明フレーム処理
* 
* @author NCN 跡部
* @version 1.0.0
*/

/**
* 定数
*/
var constants = {
	loadingAutoCloseDelay: 60000
};

var mLoadingTimerId = 0;

/**
* 読み込み中アイコンを表示する
*/
function showLoding() {
	//オーバーレイ表示
	$("#registOverlayBlack").css("display", "block");
	//アニメーション
	$("#processingServer").addClass("show");
	$("#registOverlayBlack").addClass("open");

	mLoadingTimerId = setTimeout(closeLoding, constants.loadingAutoCloseDelay);
}

/**
* 読み込み中アイコンを非表示にする
*/
function closeLoding() {
	if (mLoadingTimerId !== 0) {
		clearTimeout(mLoadingTimerId);
		mLoadingTimerId = 0;
	}

	$("#processingServer").removeClass("show");
	$("#registOverlayBlack").removeClass("open");
	$("#registOverlayBlack").css("display", "none");

}

/**
* フレームを初期化する
*/
function initFrame() {
    /// <summary>
    /// フレームを初期化する
    /// </summary>

	// iFrameの生成
    var frame = $('<iframe id="frameSC3B203" src="' + $('#DeliveryDescriptionUrl').val() + '?uid=' + $('#UrlParamAccount').val() + '&logintime=' + $('#UrlParamUpdateDate').val() + '&salesId=' + $('#UrlParamSalesId').val() + '&cstId=' + $('#UrlParamCstId').val() + '&cstType=' + $('#UrlParamCstType').val() + '&cstVclType=' + $('#UrlParamCstVclType').val() + '" width="1024px" height="655px" scrolling="no" id="DeliveryDescriptionFrame" seamless></iframe>');
	// iFrameの追加
	$('#Pages_SC3B203').append(frame);

	//ローディング開始
	showLoding();

}

/**
* window.onloadイベント処理
*/
function onLoad() {
	//ローディング終了
	closeLoding();

}

/**
* 非同期処理開始イベント処理
*/
function pageRequestManagerInitializeRequest(sender, args) {
	//ローディング開始
	showLoding();
}

/**
* 非同期処理終了イベント処理
*/
function pageRequestManagerEndRequest(sender, args) {

	//ローディング終了
	closeLoding();

}

/**
* icrop:came のコールバック
*/
var icropCameCallBack =
    function (rc) {
        //ローディング終了
        closeLoding();
        $('#frameSC3B203').each(
            function () {
                $(this).contents().find('#frameSC3B203').each(arguments.callee);
                this.contentWindow.CallBackThumbnailPhoto(rc);
            }
        );
    }


/**
* 画面表示が完了したとき
*/
function showed() {
	/// <summary>
	/// 画面表示が完了したとき
	/// </summary>

	//ローディング終了
	closeLoding();

}

/**
* document.readyイベント処理
*/
$(function () {

    $("li.prevButton").hide();  // 戻る
    $("li.nextButton").hide();  // 次へ
    $("#forumButtonPanel").hide();  //通知履歴
    $("#visitorButtonPanel").hide();    //来店一覧

	// ローディング開始
	showLoding();

	// ロード完了時イベント定義
	jQuery.event.add(window, "load", onLoad);

	// PageRequestManagerクラスをインスタンス化
	var mng = Sys.WebForms.PageRequestManager.getInstance();

	// 非同期処理開始イベント定義
	mng.add_initializeRequest(pageRequestManagerInitializeRequest);

	// 非同期処理終了イベント定義
	mng.add_endRequest(pageRequestManagerEndRequest);

	initFrame();

});
