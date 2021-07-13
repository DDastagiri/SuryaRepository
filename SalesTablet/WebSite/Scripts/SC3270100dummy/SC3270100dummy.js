/** 
* @fileOverview 受注時説明フレーム処理
* 
* @author SKFC 下元武
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
	/// <summary>
	/// 読み込み中アイコンを表示する
	/// </summary>

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
	/// <summary>
	/// 読み込み中アイコンを非表示にする
	/// </summary>

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
	var frame = $('<iframe id="frameSC3B20201" src="' + $('#SalesbookingDescriptionUrl').val() + '?uid=' + $('#UrlParamAccount').val() + '&logintime=' + $('#UrlParamUpdateDate').val() + '&salesid=' + $('#UrlParamSalesId').val() + '&estimateid=' + $('#UrlParamEstimateId').val() + '&salesbkgnum=' + $('UrlParamSalesbkgNum').val() + '&viewmode=' + $('#UrlParamSalesbookingDescriptionViewMode').val() + '&contractaskchgflg=' + $('#UrlParamContractAskChgFlg').val() + '" width="1024px" height="655px" scrolling="no" id="SalesbookingDescriptionFrame" seamless></iframe>');

	// iFrameの追加
	$('#Pages_SC3B20201').append(frame);

	//ローディング開始
	showLoding();

}

/**
* window.onloadイベント処理
*/
function onLoad() {
	/// <summary>
	/// window.onloadイベント処理
	/// </summary>

	//ローディング終了
	closeLoding();

}

/**
* 非同期処理開始イベント処理
*/
function pageRequestManagerInitializeRequest(sender, args) {
	/// <summary>
	/// 非同期処理開始イベント処理
	/// </summary>

	//ローディング開始
	showLoding();

}

/**
* 非同期処理終了イベント処理
*/
function pageRequestManagerEndRequest(sender, args) {
	/// <summary>
	/// 非同期処理終了イベント処理
	/// </summary>

	//	//保存イベント中の場合
	//	if (isSaving) {

	//		isSaving = false;

	//		//入力エラー判定
	//		if (isInvalid()) {
	//			//ローディング終了
	//			closeLoding();
	//		} else {
	//			//入力エラーがなければ保存
	//			$("#SaveButton").click();
	//		}

	//	} else if (isRestoreSeries) {

	//		//選択車種の復元完了
	//		isRestoreSeries = false;

	//	} else {

	//		//初期処理
	//		initialize();

	//ローディング終了
	closeLoding();

	//	}

}

/**
* プレビューボタンをクリックしたとき
*/
function onPreviewButtonClick() {
	/// <summary>
	/// プレビューボタンをクリックしたとき
	/// </summary>

	$("#frameSC3B20201")[0].contentWindow.onPreviewButtonClick();

	return false;

}

/**
* 保存ボタンをクリックしたとき
*/
function onSaveButtonClick() {
	/// <summary>
	/// 保存ボタンをクリックしたとき
	/// </summary>

	$("#frameSC3B20201")[0].contentWindow.onSaveButtonClick();

	return false;

}

/**
* 画面データの登録を開始したとき
*/
function startSave() {
	/// <summary>
	/// 画面データの登録を開始したとき
	/// </summary>

	// ローディング開始
	showLoding();

}

/**
* 画面データの登録に成功したとき
*/
function endSave() {
	/// <summary>
	/// 画面データの登録に成功したとき
	/// </summary>

	////ローディング終了
	//closeLoding();

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
