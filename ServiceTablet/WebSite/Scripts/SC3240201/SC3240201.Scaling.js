/** 
* @fileOverview SC3240201.Scaling.js
* 
* @author TMEJ 岩城
* @version 1.0.0
* 更新：2013/12/04 TMEJ 下村 タブレット版SMB チーフテクニシャン機能開発
*/

/**
* チップ詳細(小)からチップ詳細(大)に画面を拡大表示するため、拡大表示用のCSSプロパティをセットする
*
*/
function SetExpandDisplay() {
    $("#ChipDetailDataBox").css("width", "972px");
    $("#ChipDetailPopupContent").css("left", "19px");
    $("#ChipDetailPopUpHeaderDiv").css("width", "983px");
    $("#ChipDetailOverShadowDiv").css("width", "973px");
    $("#ChipDetailBorderBoxDiv").css("width", "983px");
    $("#ChipDetailMyDataBoxDiv").css("width", "983px");
    $("#ChipDetailGradationBoxDiv").css("width", "983px");
    $("#ChipDetailNscPopUpHeaderBgDiv").css("width", "983px");
    $("#ChipDetailNscPopUpDataBgDiv").css("width", "983px");
    $("#ExpansionButton").css("display", "none");
    $("#ShrinkingButton").css("display", "block");
    $("#ChipDetailArrowMask").css("display", "none");
    $("#DetailSActiveIndicator").css("left", "476px");
}

/**
* チップ詳細(大)からチップ詳細(小)に画面を縮小表示するため、縮小表示用のCSSプロパティをセットする
*
*/
function SetShrinkDisplay() {
    $("#ChipDetailDataBox").css("width", "375px");
    $("#ChipDetailPopupContent").css("left", gDetailSPopX);
    $("#ChipDetailPopUpHeaderDiv").css("width", "385px");
    $("#ChipDetailOverShadowDiv").css("width", "375px");
    $("#ChipDetailBorderBoxDiv").css("width", "385px");
    $("#ChipDetailMyDataBoxDiv").css("width", "385px");
    $("#ChipDetailGradationBoxDiv").css("width", "385px");
    $("#ChipDetailNscPopUpHeaderBgDiv").css("width", "385px");
    $("#ChipDetailNscPopUpDataBgDiv").css("width", "385px");
    $("#ShrinkingButton").css("display", "none");
    $("#ExpansionButton").css("display", "block");
    $("#ChipDetailArrowMask").css("display", "block");
    $("#DetailSActiveIndicator").css("left", "174px");
}

/**
* チップ詳細(小)からチップ詳細(大)に画面を拡大表示する
*
*/
function ExpandDisplay() {

    //登録ボタンを非活性にしておく
    $("#DetailRegisterBtn").attr("disabled", true);

    // ボタンを青色にする
    //$("#DetailExpandDiv").addClass("icrop-pressed");
    $("#DetailShrinkDiv").addClass("icrop-pressed");

    //背景色をクリア
    //$("#ExpansionButton").css("background-image", "none");
    $("#ShrinkingButton").css("background-image", "none");

    setTimeout(function () {

        // ボタンの青色を解除
        //$("#DetailExpandDiv").removeClass("icrop-pressed");
        $("#DetailShrinkDiv").removeClass("icrop-pressed");

        //背景色を戻す
        //$("#ExpansionButton").css("background-image", "-webkit-gradient(linear, left top, left bottom, from(#576174), color-stop(0.5, #19233e),to(#000b29))");
        $("#ShrinkingButton").css("background-image", "-webkit-gradient(linear, left top, left bottom, from(#576174), color-stop(0.5, #19233e),to(#000b29))");
    }, 200);

    //画面コントロールのプロパティを拡大表示用にする
    SetExpandDisplay();

    //チップ詳細(小)を非表示
    $("#ChipDetailSContent").fadeOut(100);

    setTimeout(function () {
        //チップ詳細(小)のスクロール位置を初期位置に戻す
        $("#ChipDetailSContent .scroll-inner").css("transform", "translate3d(0px, 0px, 0px)");

        //チップ詳細(大)を表示
        $("#ChipDetailLContent").fadeIn(500);

        //チップ詳細(大)の横スクロール領域調整
        $("#stallArea .scroll-inner-SC3240201").width($("#chipInfoTable")[0].clientWidth);

        //チップ詳細(大)のテキストエリア領域をチップ詳細(小)のテキストエリア領域に合わせる
        AdjusterDetailTextArea($("#DetailLOrderTxt"), $("#DetailLOrderDt"));
        //2013/12/04 TMEJ 下村 タブレット版SMB チーフテクニシャン機能開発 START
        //AdjusterDetailTextArea($("#DetailLFailureTxt"), $("#DetailLFailureDt"));
        //AdjusterDetailTextArea($("#DetailLResultTxt"), $("#DetailLResultDt"));
        //AdjusterDetailTextArea($("#DetailLAdviceTxt"), $("#DetailLAdviceDt"));
        AdjusterDetailTextArea($("#DetailLMemoTxt"), $("#DetailLMemoDt"));
        //2013/12/04 TMEJ 下村 タブレット版SMB チーフテクニシャン機能開発 END

        //チップ詳細(小)で、納車予定変更履歴エリアを閉じている場合
        if (!chipDetailSlideDownFlag) {
            // チップ詳細(大)の納車予定変更履歴エリアをフェードアウト　※スライドアップが効かない為、フェードアウトを使用
            $("#DetailLHeadInfomationPullDiv").fadeOut(100);
        }
    }, 500);

    //必須項目がEmptyなら登録ボタンを非活性にする
    $("#DetailRegisterBtn").attr("disabled", IsMandatoryChipDetailEmpty());

    return false;
}

/**
* チップ詳細(大)からチップ詳細(小)に画面を縮小表示する
*
*/
function ShrinkDisplay(closeFlg) {

    //登録ボタンを非活性にしておく
    $("#DetailRegisterBtn").attr("disabled", true);

    if (closeFlg != "0") {
        // ボタンを青色にする
        $("#DetailExpandDiv").addClass("icrop-pressed");
        //$("#DetailShrinkDiv").addClass("icrop-pressed");
    
        //背景色をクリア
        $("#ExpansionButton").css("background-image", "none");
        //$("#ShrinkingButton").css("background-image", "none");
    
        setTimeout(function () {
    
            // ボタンの青色を解除
            $("#DetailExpandDiv").removeClass("icrop-pressed");
            //$("#DetailShrinkDiv").removeClass("icrop-pressed");
    
            //背景色を戻す
            $("#ExpansionButton").css("background-image", "-webkit-gradient(linear, left top, left bottom, from(#576174), color-stop(0.5, #19233e),to(#000b29))");
            //$("#ShrinkingButton").css("background-image", "-webkit-gradient(linear, left top, left bottom, from(#576174), color-stop(0.5, #19233e),to(#000b29))");
        }, 200);
    }

    //チップ詳細(大)を非表示
    $("#ChipDetailLContent").fadeOut(100);

    //画面コントロールのプロパティを縮小表示用にする
    SetShrinkDisplay();

    setTimeout(function () {
        //チップ詳細(大)のスクロール位置を初期位置に戻す
        $("#ChipDetailLContent .scroll-inner").css("transform", "translate3d(0px, 0px, 0px)");

        //チップ詳細(小)を表示
        $("#ChipDetailSContent").fadeIn(500);

        //チップ詳細(小)のテキストエリア領域をチップ詳細(大)のテキストエリア領域に合わせる
        AdjusterDetailTextArea($("#DetailSOrderTxt"), $("#DetailSOrderDt"));
        //2013/12/04 TMEJ 下村 タブレット版SMB チーフテクニシャン機能開発 START
        //AdjusterDetailTextArea($("#DetailSFailureTxt"), $("#DetailSFailureDt"));
        //AdjusterDetailTextArea($("#DetailSResultTxt"), $("#DetailSResultDt"));
        //AdjusterDetailTextArea($("#DetailSAdviceTxt"), $("#DetailSAdviceDt"));
        AdjusterDetailTextArea($("#DetailSMemoTxt"), $("#DetailSMemoDt"));
        //2013/12/04 TMEJ 下村 タブレット版SMB チーフテクニシャン機能開発 END

        //チップ詳細(大)で、納車予定変更履歴エリアを閉じている場合
        if (!chipDetailSlideDownFlag) {
            // チップ詳細(小)の納車予定変更履歴エリアをフェードアウト　※スライドアップが効かない為、フェードアウトを使用
            $("#DetailSHeadInfomationPullDiv").fadeOut(100);
        }
    }, 500);

    //必須項目がEmptyなら登録ボタンを非活性にする
    $("#DetailRegisterBtn").attr("disabled", IsMandatoryChipDetailEmpty());

    return false;
}