/**
* @fileOverview SC3160219　RO損傷登録
*
* @author SKFC橋本
* @update SKFC二村 TR-V4-TKM-20190813-003横展
* @version 1.0.1
*/


/**
* @コールバック関数定義
*/
var callBack = {
    doCallback: function (argument, callbackFunction) {
        this.packedArgument = argument;
        this.endCallback = callbackFunction;
        this.beginCallback();
    }
};

/**
* @初期表示後のポップアップリサイズコールバック関数
*/
function SetResizePopUp() {
    //タイトルを取得
    var strPopUpTitle = document.getElementById("Hidden_Title").value;

    //画面の幅と高さを取得
    var strPopUpHeight = document.getElementById("PopUpBlock").offsetHeight;
    var strPopUpWidth = document.getElementById("PopUpBlock").offsetWidth;

    //左ボタンタイトルを取得
    var strLeftBottunTitle = document.getElementById("Hidden_CancelTitle").value;
    //右ボタンタイトルを取得
    var strRightBottunTitle = document.getElementById("Hidden_DoneTitle").value;

    //ポップアップ外タップ設定
    if (strRightBottunTitle == "") {
        var outOfPopupTap = "YES";
    } else {
        var outOfPopupTap = "NO";
    }
    var strRet = ""
    strRet += "title=" + strPopUpTitle;
    strRet += "&w=" + strPopUpWidth;
    strRet += "&h=" + strPopUpHeight;
    strRet += "&lbuttonword=" + strLeftBottunTitle;
    strRet += "&rbuttonword=" + strRightBottunTitle;
    strRet += "&lbuttoncb=" + "OnCloseBottunClick";
    strRet += "&rbuttoncb=" + "OnDoneBottunClick";
    strRet += "&outofpopuptap=" + outOfPopupTap;

    return strRet;
}

/**
* キャンセルボタン押下時コールバック関数
*/
function OnCloseBottunClick() {

    //非画像アップロード時はポップアップを閉じる
    if (document.getElementById("Hidden_UploadFlag").value == "0") {
        //ポップアップを閉じる
        var query = "icrop:titlebarPopup?close=YES";
        location.href = query;

        return "OK";
    }
    else {
        //画像アップロード中はポップアップを閉じない
        return "OK";
    }
}

/**
* 追加ボタン押下時コールバック関数
*/
function OnDoneBottunClick() {

    //非画像アップロード時はポップアップを閉じる
    if (document.getElementById("Hidden_UploadFlag").value == "0") {
        //一回しか起動しない（非同期の為、連打防止）
        if (document.getElementById("Hidden_DoneClickFlg").value != "1") {

            document.getElementById("Hidden_DoneClickFlg").value = "1"

            var RoThumbnailIdOrg = ""
            RoThumbnailIdOrg += document.getElementById("Hidden_RoThumbnailIdOrg1").value;
            RoThumbnailIdOrg += ',' + document.getElementById("Hidden_RoThumbnailIdOrg2").value;
            RoThumbnailIdOrg += ',' + document.getElementById("Hidden_RoThumbnailIdOrg3").value;
            RoThumbnailIdOrg += ',' + document.getElementById("Hidden_RoThumbnailIdOrg4").value;
            RoThumbnailIdOrg += ',' + document.getElementById("Hidden_RoThumbnailIdOrg5").value;

            var RoThumbnailIdDel = ""
            RoThumbnailIdDel += document.getElementById("Hidden_RoThumbnailIdDel1").value;
            RoThumbnailIdDel += ',' + document.getElementById("Hidden_RoThumbnailIdDel2").value;
            RoThumbnailIdDel += ',' + document.getElementById("Hidden_RoThumbnailIdDel3").value;
            RoThumbnailIdDel += ',' + document.getElementById("Hidden_RoThumbnailIdDel4").value;
            RoThumbnailIdDel += ',' + document.getElementById("Hidden_RoThumbnailIdDel5").value;

            var Hidden_RoThumbnailImgPath = ""
            Hidden_RoThumbnailImgPath += document.getElementById("Hidden_RoThumbnailImgPath1").value;
            Hidden_RoThumbnailImgPath += ',' + document.getElementById("Hidden_RoThumbnailImgPath2").value;
            Hidden_RoThumbnailImgPath += ',' + document.getElementById("Hidden_RoThumbnailImgPath3").value;
            Hidden_RoThumbnailImgPath += ',' + document.getElementById("Hidden_RoThumbnailImgPath4").value;
            Hidden_RoThumbnailImgPath += ',' + document.getElementById("Hidden_RoThumbnailImgPath5").value;

            var Hidden_RoThumbnailImgOrg = ""
            Hidden_RoThumbnailImgOrg += document.getElementById("Hidden_RoThumbnailImgOrg1").value;
            Hidden_RoThumbnailImgOrg += ',' + document.getElementById("Hidden_RoThumbnailImgOrg2").value;
            Hidden_RoThumbnailImgOrg += ',' + document.getElementById("Hidden_RoThumbnailImgOrg3").value;
            Hidden_RoThumbnailImgOrg += ',' + document.getElementById("Hidden_RoThumbnailImgOrg4").value;
            Hidden_RoThumbnailImgOrg += ',' + document.getElementById("Hidden_RoThumbnailImgOrg5").value;

            PageMethods.Botton_Done_Click(document.getElementById("Hidden_RoExteriorId").value,
                                document.getElementById("Hidden_PartsType").value,
                                document.getElementById("TextBox_Memo").value,
                                RoThumbnailIdOrg,
                                Hidden_RoThumbnailImgPath,
                                Hidden_RoThumbnailImgOrg,
                                document.getElementById("Hidden_LoginUserId").value,
                                document.getElementById("Hidden_DamageTypeCount").value,
                                OnCloseBottunClick, doneError);



        };

        return "OK";
    }
    else {
        //画像アップロード中はポップアップを閉じない
        return "OK";
    }
}

/**
* 追加失敗時
*/
function doneError() {

    OnCloseBottunClick();
}

/**
* ポップアップリサイズ
*/
function resizePopUp() {

    //ポップアップのサイズを取得
    var height = document.getElementById("PopUpBlock").offsetHeight;
    var width = document.getElementById("PopUpBlock").offsetWidth;

    //ポップアップをリサイズ
    var query = "";
    query += "icrop:titlebarPopup?";
    query += "w=" + width;
    query += "::h=" + height;

    location.href = query;
}

/**
* カメラ機能起動
*/
function onCamera(fileName, freeNo) {

    var posX = 80;
    var posY = 150;

    var cbMethod = "CallBackCustomerPhoto" + freeNo;

    var mode = 0;
    var path = "";

    if (0 <= fileName.lastIndexOf('/')) {
        var tmpFileName = fileName;
        // ファイル名抽出
        fileName = tmpFileName.substring(tmpFileName.lastIndexOf('/') + 1, tmpFileName.length);
        // パス名抽出
        path = tmpFileName.substring(0, tmpFileName.lastIndexOf('/'));
    }

    //タブレット基盤のカメラ機能を起動
    var query = "";
    query += "icrop:came?";
    query += "x=" + posX + "&";
    query += "y=" + posY + "&";
    query += "file=" + fileName + "&";
    query += "func=" + cbMethod + "&";
    query += "mode=" + mode + "&";
    query += "view=1&";
    query += "aspect=1&";
    query += "path=" + path;

    location.href = query;
}

/**
* @カメラ機能からのコールバック関数
*/
function CallBackCustomerPhoto1(rc) {
    if (rc == 1) {
        //WebMethod を呼び出す
        PageMethods.TakePhotoAfter(document.getElementById("Hidden_RoExteriorId").value,
                                    document.getElementById("Hidden_PartsType").value,
                                    document.getElementById("Hidden_RoThumbnailImgSeq1").value,
                                    photoSaveSuccess1,
                                    photoSaveError
                                    );
    } else if (rc == 0) {
        photoSaveCancel();
    } else if (rc == -1) {

        photoSaveError();
    }

}

function CallBackCustomerPhoto2(rc) {

    if (rc == 1) {
        //WebMethod を呼び出す
        PageMethods.TakePhotoAfter(document.getElementById("Hidden_RoExteriorId").value,
                                    document.getElementById("Hidden_PartsType").value,
                                    document.getElementById("Hidden_RoThumbnailImgSeq2").value,
                                    photoSaveSuccess2,
                                    photoSaveError
                                    );
    } else if (rc == 0) {
        photoSaveCancel();
    } else if (rc == -1) {

        photoSaveError();
    }

}

function CallBackCustomerPhoto3(rc) {

    if (rc == 1) {
        //WebMethod を呼び出す
        PageMethods.TakePhotoAfter(document.getElementById("Hidden_RoExteriorId").value,
                                    document.getElementById("Hidden_PartsType").value,
                                    document.getElementById("Hidden_RoThumbnailImgSeq3").value,
                                    photoSaveSuccess3,
                                    photoSaveError
                                    );
    } else if (rc == 0) {
        photoSaveCancel();
    } else if (rc == -1) {

        photoSaveError();
    }

}

function CallBackCustomerPhoto4(rc) {

    if (rc == 1) {
        //WebMethod を呼び出す
        PageMethods.TakePhotoAfter(document.getElementById("Hidden_RoExteriorId").value,
                                    document.getElementById("Hidden_PartsType").value,
                                    document.getElementById("Hidden_RoThumbnailImgSeq4").value,
                                    photoSaveSuccess4,
                                    photoSaveError
                                    );
    } else if (rc == 0) {
        photoSaveCancel();
    } else if (rc == -1) {

        photoSaveError();
    }

}

function CallBackCustomerPhoto5(rc) {

    if (rc == 1) {
        //WebMethod を呼び出す
        PageMethods.TakePhotoAfter(document.getElementById("Hidden_RoExteriorId").value,
                                    document.getElementById("Hidden_PartsType").value,
                                    document.getElementById("Hidden_RoThumbnailImgSeq5").value,
                                    photoSaveSuccess5,
                                    photoSaveError
                                    );
    } else if (rc == 0) {
        photoSaveCancel();
    } else if (rc == -1) {

        photoSaveError();
    }

}

/**
* 画像保存成功
*/
function photoSaveSuccess1(result) {

    //新ファイルパスを保持
    document.getElementById("Hidden_RoThumbnailImgPath1").value = result;

    //クルクル非表示
    hiddenLoadingScreen();

    //再表示
    document.getElementById("PopUpForm").submit();
}

function photoSaveSuccess2(result) {

    //新ファイルパスを保持
    document.getElementById("Hidden_RoThumbnailImgPath2").value = result;

    //クルクル非表示
    hiddenLoadingScreen();

    //再表示
    document.getElementById("PopUpForm").submit();
}

function photoSaveSuccess3(result) {

    //新ファイルパスを保持
    document.getElementById("Hidden_RoThumbnailImgPath3").value = result;

    //クルクル非表示
    hiddenLoadingScreen();

    //再表示
    document.getElementById("PopUpForm").submit();
}

function photoSaveSuccess4(result) {

    //新ファイルパスを保持
    document.getElementById("Hidden_RoThumbnailImgPath4").value = result;

    //クルクル非表示
    hiddenLoadingScreen();

    //再表示
    document.getElementById("PopUpForm").submit();
}

function photoSaveSuccess5(result) {

    //新ファイルパスを保持
    document.getElementById("Hidden_RoThumbnailImgPath5").value = result;

    //クルクル非表示
    hiddenLoadingScreen();

    //再表示
    document.getElementById("PopUpForm").submit();
}

/**
* 画像保存キャンセル
*/
function photoSaveCancel() {

    //クルクル非表示
    hiddenLoadingScreen();
}

/**
* 画像保存失敗
*/
function photoSaveError() {

    //クルクル非表示
    hiddenLoadingScreen();

    //画像の保存に失敗
    window.alert(document.getElementById("Hidden_MessageSaveImageFailure").value);
}

/**
 * 損傷写真押下処理
 */
function onClickDamagePhoto1() {
    // 写真表示ポップアップURLスキーム呼び出し

    var strUrl = window.location.href;
    var baseUrl = strUrl.slice(0, strUrl.slice(0, strUrl.indexOf('?')).lastIndexOf('/')) + "/SC3170211.aspx";
    var targetUrl = baseUrl + "?";

    targetUrl += "PictureURL=" + $("#Hidden_OriginalImageFilePath1").val();
    targetUrl += "&TitleString=" + $("#Hidden_Title").val();
    // 参照モード:0 編集モード:1
    targetUrl += "&Mode=" + (("0" == $("#Hidden_DispMode").val()) ? "1" : "0");

    var urlScheme = "icrop:popupPhoto?";

    urlScheme += "url=" + targetUrl;
    urlScheme += "::x=0";
    urlScheme += "::y=0";
    urlScheme += "::w=1024";
    urlScheme += "::h=768";
    urlScheme += "::callback=callbackDisplayPhoto" + 1;

    urlScheme += "::view=1";


    window.location.href = urlScheme;
}

function onClickDamagePhoto2() {
    // 写真表示ポップアップURLスキーム呼び出し
    var strUrl = window.location.href;
    var baseUrl = strUrl.slice(0, strUrl.slice(0, strUrl.indexOf('?')).lastIndexOf('/')) + "/SC3170211.aspx";
    var targetUrl = baseUrl + "?";

    targetUrl += "PictureURL=" + $("#Hidden_OriginalImageFilePath2").val();
    targetUrl += "&TitleString=" + $("#Hidden_Title").val();
    // 参照モード:0 編集モード:1
    targetUrl += "&Mode=" + (("0" == $("#Hidden_DispMode").val()) ? "1" : "0");

    var urlScheme = "icrop:popupPhoto?";

    urlScheme += "url=" + targetUrl;
    urlScheme += "::x=0";
    urlScheme += "::y=0";
    urlScheme += "::w=1024";
    urlScheme += "::h=768";
    urlScheme += "::callback=callbackDisplayPhoto" + 2;
    urlScheme += "::view=1";

    window.location.href = urlScheme;
}

function onClickDamagePhoto3() {
    // 写真表示ポップアップURLスキーム呼び出し
    var strUrl = window.location.href;
    var baseUrl = strUrl.slice(0, strUrl.slice(0, strUrl.indexOf('?')).lastIndexOf('/')) + "/SC3170211.aspx";
    var targetUrl = baseUrl + "?";

    targetUrl += "PictureURL=" + $("#Hidden_OriginalImageFilePath3").val();
    targetUrl += "&TitleString=" + $("#Hidden_Title").val();
    // 参照モード:0 編集モード:1
    targetUrl += "&Mode=" + (("0" == $("#Hidden_DispMode").val()) ? "1" : "0");

    var urlScheme = "icrop:popupPhoto?";

    urlScheme += "url=" + targetUrl;
    urlScheme += "::x=0";
    urlScheme += "::y=0";
    urlScheme += "::w=1024";
    urlScheme += "::h=768";
    urlScheme += "::callback=callbackDisplayPhoto" + 3;
    urlScheme += "::view=1";

    window.location.href = urlScheme;
}

function onClickDamagePhoto4() {
    // 写真表示ポップアップURLスキーム呼び出し
    var strUrl = window.location.href;
    var baseUrl = strUrl.slice(0, strUrl.slice(0, strUrl.indexOf('?')).lastIndexOf('/')) + "/SC3170211.aspx";
    var targetUrl = baseUrl + "?";

    targetUrl += "PictureURL=" + $("#Hidden_OriginalImageFilePath4").val();
    targetUrl += "&TitleString=" + $("#Hidden_Title").val();
    // 参照モード:0 編集モード:1
    targetUrl += "&Mode=" + (("0" == $("#Hidden_DispMode").val()) ? "1" : "0");

    var urlScheme = "icrop:popupPhoto?";


    urlScheme += "url=" + targetUrl;
    urlScheme += "::x=0";
    urlScheme += "::y=0";
    urlScheme += "::w=1024";
    urlScheme += "::h=768";
    urlScheme += "::callback=callbackDisplayPhoto" + 4;
    urlScheme += "::view=1";



    window.location.href = urlScheme;
}

function onClickDamagePhoto5() {
    // 写真表示ポップアップURLスキーム呼び出し
    var strUrl = window.location.href;
    var baseUrl = strUrl.slice(0, strUrl.slice(0, strUrl.indexOf('?')).lastIndexOf('/')) + "/SC3170211.aspx";
    var targetUrl = baseUrl + "?";

    targetUrl += "PictureURL=" + $("#Hidden_OriginalImageFilePath5").val();
    targetUrl += "&TitleString=" + $("#Hidden_Title").val();
    // 参照モード:0 編集モード:1
    targetUrl += "&Mode=" + (("0" == $("#Hidden_DispMode").val()) ? "1" : "0");

    var urlScheme = "icrop:popupPhoto?";

    urlScheme += "url=" + targetUrl;
    urlScheme += "::x=0";
    urlScheme += "::y=0";
    urlScheme += "::w=1024";
    urlScheme += "::h=768";
    urlScheme += "::callback=callbackDisplayPhoto" + 5;
    urlScheme += "::view=1";

    window.location.href = urlScheme;
}
/**
 * 写真表示ポップアップからのコールバック処理
 */
function callbackDisplayPhoto1(msg) {
    // カメラで新規撮影かつ編集モードの場合
    if ("1" == msg) {
        // 再表示
        var imagePath = $("#Img_DmagePhoto1").attr("src");
        $("#Img_DmagePhoto1").attr("src", "");
        $("#Img_DmagePhoto1").attr("src", imagePath);
    }
}

/**
 * 写真表示ポップアップからのコールバック処理
 */
function callbackDisplayPhoto2(msg) {
    // カメラで新規撮影かつ編集モードの場合
    if ("1" == msg) {
        // 再表示
        var imagePath = $("#Img_DmagePhoto2").attr("src");
        $("#Img_DmagePhoto2").attr("src", "");
        $("#Img_DmagePhoto2").attr("src", imagePath);
    }
}

/**
 * 写真表示ポップアップからのコールバック処理
 */
function callbackDisplayPhoto3(msg) {
    // カメラで新規撮影かつ編集モードの場合
    if ("1" == msg) {
        // 再表示
        var imagePath = $("#Img_DmagePhoto3").attr("src");
        $("#Img_DmagePhoto3").attr("src", "");
        $("#Img_DmagePhoto3").attr("src", imagePath);
    }
}

/**
 * 写真表示ポップアップからのコールバック処理
 */
function callbackDisplayPhoto4(msg) {
    // カメラで新規撮影かつ編集モードの場合
    if ("1" == msg) {
        // 再表示
        var imagePath = $("#Img_DmagePhoto4").attr("src");
        $("#Img_DmagePhoto4").attr("src", "");
        $("#Img_DmagePhoto4").attr("src", imagePath);
    }
}

/**
 * 写真表示ポップアップからのコールバック処理
 */
function callbackDisplayPhoto5(msg) {
    // カメラで新規撮影かつ編集モードの場合
    if ("1" == msg) {
        // 再表示
        var imagePath = $("#Img_DmagePhoto5").attr("src");
        $("#Img_DmagePhoto5").attr("src", "");
        $("#Img_DmagePhoto5").attr("src", imagePath);
    }
}

/**
 * クルクル表示処理
 */
function dispLoadingScreen() {
    //画像アップロードフラグを更新(アップロード中)
    document.getElementById("Hidden_UploadFlag").value = "1";
    //クルクル表示
    document.getElementById("LoadingScreen").style.display = "table";
}

/**
 * クルクル非表示処理
 */
function hiddenLoadingScreen() {
    //画像アップロードフラグを更新(非アップロード)
    document.getElementById("Hidden_UploadFlag").value = "0";
    //クルクル非表示
    document.getElementById("LoadingScreen").style.display = "none";
}

/**
* readyイベント
*/
$(function () {
    // 損傷写真押下イベント設定
    $("#Img_DmagePhoto1").click(onClickDamagePhoto1);
    $("#Img_DmagePhoto2").click(onClickDamagePhoto2);
    $("#Img_DmagePhoto3").click(onClickDamagePhoto3);
    $("#Img_DmagePhoto4").click(onClickDamagePhoto4);
    $("#Img_DmagePhoto5").click(onClickDamagePhoto5);
});