/** 
* @fileOverview SC3070202.aspクラスを記述するファイル.
* 
* @author TCS myose
* @version 1.0.0
*/

/**
* 見積印刷の初期表示制御を行う。
* 
* @param {-} - -
* @return {-} -
* 
* @example 
*  -
*/
window.onload = function () {
    document.getElementById("contractHcv5001PaperFrame").style.height = $("#DisplayHeightValueHiddenField").val();
    document.getElementById("scrollInner").style.height = $("#ScrollHeightValueHiddenField").val();
}

/**
* 見積印刷の画面制御を行う。
* 
* @param {-} - -
* @return {-} -
* 
* @example 
*  -
*/
$(function () {
    $("#scrollInner").fingerScroll();

    //印刷処理時にボタン色変更
    $("#printButton").bind("click", changeColor);

    //2012/01/17 myose add start
    //閉じるボタン押下で非活性
    $("#closeButton").bind("click", disabledButton);
    //2012/01/17 myose add end
    
});

/**
* 印刷を行う。
* 
* @param {-} - -
* @return {-} -
* 
* @example 
*  -
*/
function printDialog() {
    window.location = "icrop:prtr:";
    $("#printButton").removeClass("buttonGlay2");
    $("#printButton").addClass("buttonGlay1");
    document.getElementById("contractHcv5001PaperFrame").style.height = $("#DisplayHeightValueHiddenField").val();
    document.getElementById("scrollInner").style.height = $("#ScrollHeightValueHiddenField").val();
}

/**
* 印刷ボタンのcssクラスを制御する。
* 
* @param {-} - -
* @return {-} -
* 
* @example 
*  -
*/
function changeColor() {
    $("#printButton").removeClass("buttonGlay1");
    $("#printButton").addClass("buttonGlay2");
}

//2012/01/17 myose add start
/**
* 閉じるボタンの非活性を制御する。
* 
* @param {-} - -
* @return {-} -
* 
* @example 
*  -
*/
function disabledButton() {
    $("#closeButton").attr("disabled", "disabled");
}
//2012/01/17 myose add end

