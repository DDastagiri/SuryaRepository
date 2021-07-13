/** 
* @fileOverview SC3070301.aspクラスを記述するファイル.
* 
* @author TCS aida
* @version 1.0.0
* 更新： 2012/02/03 TCS 藤井  【SALES_1A】号口(課題No.46)対応
*/

/**
* 契約書印刷の画面制御を行う.
* 
* @param {-} - -
* @return {-} -
* 
* @example 
*  -
*/
$(function () {

    $("#scrollInner").fingerScroll();

    //印刷処理
    $("#PrintButton").bind("click", changeColor);

});

/**
* 印刷を行う.
* 
* @param {-} - -
* @return {-} -
* 
* @example 
*  -
*/
function printDialog() {

    window.location = "icrop:prtr:";
    $("#PrintButton").removeClass("buttonGlay3");
    $("#PrintButton").addClass("buttonGlay1");
}

/**
* 印刷ボタンのcssクラスを制御する.
* 
* @param {-} - -
* @return {-} -
* 
* @example 
*  -
*/
function changeColor() {

    $("#PrintButton").removeClass("buttonGlay1");
    $("#PrintButton").addClass("buttonGlay3");

}

/**
* 閉じるボタンのcssクラスを制御する.
* 
* @param {-} - -
* @return {-} -
* 
* @example 
*  -
*/
function closeButtonClick() {

    this_form.CloseButton.disabled = true;
}

//2012/02/03 TCS 藤井 【SALES_1A】号口(課題No.46)対応 ADD START
/**
* 実行ボタン押下時の送信確認をする.
* 
* @param {-} - -
* @return {-} -
* 
* @example 
*  -
*/
function sendButtonClick() {
    var sendCheckConfirm = confirm(sendCheckMsg.value);
    if (sendCheckConfirm == false) {
        return false;
    } else {
        dispLoading();
        return true;
    }
}

/**
* キャンセルボタン押下時の確認をする.
* 
* @param {-} - -
* @return {-} -
* 
* @example 
*  -
*/
function cancelButtonClick() {
    return cancelCheckConfirm = confirm(cancelCheckMsg.value);
}

/**
* オーバーレイ、ロード中処理.
* 
* @param {-} - -
* @return {-} -
* 
* @example 
*  -
*/
function dispLoading() {

    //オーバーレイ表示
    $("#serverProcessOverlayBlack").css("display", "block");
    //アニメーション(ロード中)
    setTimeout(function () {
        $("#serverProcessIcon").addClass("show");
        $("#serverProcessOverlayBlack").addClass("open");
    }, 0);
}
//2012/02/03 TCS 藤井 【SALES_1A】号口(課題No.46)対応 ADD END
