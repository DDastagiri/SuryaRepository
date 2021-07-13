/**
* @fileOverview SC3170209　追加作業サムネイル
*
* @author SKFC橋本
* @version 1.0.0
*/


$(window).load(function () {
    // スライダー設定
    $('.flexslider').flexslider({
        animation: "slide",
        slideDirection: "horizontal",
        slideshow: false,
        controlNav: true,
        slideToStart: 0,
        animationLoop: false,
        controlsContainer: ".ControlNaviArea"
    });

    // カメラボタン設定
    $("#A_CameraButtom").click(function () {
        $.ajax({
            type: 'POST',
            datatype: 'json',
            url: 'SC3170209.aspx/Botton_Camera_Click',
            data: "{}",
            contentType: "application/json; charset=utf-8",
            success: function (result) {
                if (result) { onCamera(result.d); }
            }
        });
        // #リンクを無効（ページ先頭への移動を抑制）
        return false;
    });

    // 画像タップイベント設定
    $(".flexslider > ul > li > img").click(onImageClick);
});

/**
 * 画像タップイベント
 */
function onImageClick() {
    // 写真選択ポップアップ画面表示
    var strUrl = window.location.href;
    var target = strUrl.slice(0, strUrl.slice(0, strUrl.indexOf('?')).lastIndexOf('/')) + "/SC3170210.aspx";
    target += "?DealerCode=" + $("#Hidden_DlrCd").val();
    target += "&BranchCode=" + $("#Hidden_BrnCd").val();
    target += "&SAChipID=" + $("#Hidden_VisitSeq").val();
    target += "&BASREZID=" + $("#Hidden_BasrezId").val();
    target += "&R_O=" + $("#Hidden_RoNo").val();
    target += "&SEQ_NO=" + $("#Hidden_RoSeqNo").val();
    //2018/01/24 Mod Start【iOS11 課題№13】
    //target += "&VIN_NO=" + $("#Hidden_VinNo").val();
    target += "&VIN_NO=" + encodeURIComponent($("#Hidden_VinNo").val());
    //2018/01/24 Mod End
    target += "&PictMode=" + $("#Hidden_PictureGroup").val();
    target += "&ViewMode=2";
    target += "&LinkSysType=" + $("#Hidden_LinkSysType").val();
    target += "&LoginUserID=" + $("#Hidden_LoginUserId").val();

    // iframe越しのコールバック
    var cbFunc = "var cbTemp=function(){$('iframe').each(function(){$(this).contents().find('iframe').each(arguments.callee);if($(this).contents().find('body#RoThumbnailImage').length>0){this.contentWindow.cbThumbEditEnd();}});};cbTemp";

    // タイトルバー無しポップアップ
    //2018/01/24 Mod Start【iOS11 課題№13】
    //var scheme = "icrop:noTitlePopup?";
    var scheme = "icrop:///noTitlePopup?"; //タブレット端末アプリでのDecode回避
    //2018/01/24 Mod End
    scheme += "url=" + target;
    scheme += "::x=0";
    scheme += "::y=56";
    scheme += "::w=1024";
    scheme += "::h=656";
    scheme += "::endFunc=" + cbFunc;

    window.location.href = scheme;
}

/**
* タイトルバー無しポップアップコールバック
*/
function cbThumbEditEnd() {
    // 再表示
    window.location.reload(true);
}

/**
 * 日付文字列を取得する
 */
function getDateString() {
    // 今日の日付で Date オブジェクトを作成
    var now = new Date();

    // 「年」「月」「日」「曜日」を Date オブジェクトから取り出してそれぞれに代入
    var y = now.getFullYear();
    var m = now.getMonth() + 1;
    var d = now.getDate();

    // 「月」と「日」で1桁だったときに頭に 0 をつける
    if (m < 10) {
        m = '0' + m;
    }
    if (d < 10) {
        d = '0' + d;
    }

    // フォーマットを整形してコンソールに出力
    return (y + m + d);
}

/**
* カメラ機能起動
*/
function onCamera(fileName) {

    //クルクル表示
    dispLoadingScreen();

    //サムネイル画像IDとファイル名（拡張子付き）を保持する
    var path = $("#Hidden_DlrCd").val() + "/" + getDateString();

    document.getElementById("Hidden_RoThumbnailId").value = fileName.substring(0, (fileName.indexOf("_")));
    //document.getElementById("Hidden_RoThumbnailImgPath").value = path + "/" + fileName + ".png";

    document.getElementById("Hidden_RoThumbnailImgPath").value = path + "/" + fileName + document.getElementById("Hidden_PictureFormat").value;

    var posX = 80;
    var posY = 150;
    //カメラのコールバック関数に検索ロジックを付けて渡す
    var cbMethod = "var callbackCamera=" +
                        "function(rc){" +
                            "$('iframe').each(function(){" +
                                "$(this).contents().find('iframe').each(arguments.callee);" +
                                "if($(this).contents().find('body#RoThumbnailImage').length > 0){" +
                                    "this.contentWindow.CallBackThumbnailPhoto(rc);" +
                                "}" +
                            "});" +
                        "};callbackCamera";
    var mode = 0;
    var view = 0;
    var aspect = 1;

    //タブレット基盤のカメラ機能を起動
    var query = "";
    query += "icrop:came?";
    query += "x=" + posX + "&";
    query += "y=" + posY + "&";
    query += "file=" + fileName + "&";
    query += "func=" + cbMethod + "&";
    query += "mode=" + mode + "&";
    query += "view=" + view + "&";
    query += "aspect=" + aspect + "&";
    query += "path=" + path;

    location.href = query;
}

function takeFileNameError() { }

/**
* @カメラ機能からのコールバック関数
*/
function CallBackThumbnailPhoto(rc) {
    if (rc == 1) {
        if (0 < document.getElementById("Hidden_RoThumbnailImgPath").value.length) {

            //WebMethod を呼び出す
            PageMethods.TakePhotoAfter(document.getElementById("Hidden_RoThumbnailId").value,
                                   document.getElementById("Hidden_DlrCd").value,
                                   document.getElementById("Hidden_BrnCd").value,
                                   document.getElementById("Hidden_VisitSeq").value,
                                   document.getElementById("Hidden_BasrezId").value,
                                   document.getElementById("Hidden_RoNo").value,
                                   document.getElementById("Hidden_RoSeqNo").value,
                                   document.getElementById("Hidden_VinNo").value,
                                   document.getElementById("Hidden_CaptureGroup").value,
                                   document.getElementById("Hidden_LoginUserId").value,
                                   document.getElementById("Hidden_RoThumbnailImgPath").value,
                                   photoSaveSuccess,
                                   photoSaveError
                                   );
        }
    } else if (rc == 0) {
        photoSaveCancel();
    } else if (rc == -1) {
        //1秒後に非同期でアラート表示
        setTimeout("photoSaveError()", 1000);
    }
}

/**
* 画像保存成功
*/
function photoSaveSuccess() {
    // ファイルパス初期化
    document.getElementById("Hidden_RoThumbnailImgPath").value = "";

    //クルクル非表示
    hiddenLoadingScreen();

    //再表示
    location.reload(true);
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
    // ファイルパス初期化
    document.getElementById("Hidden_RoThumbnailImgPath").value = "";

    //クルクル非表示
    hiddenLoadingScreen();

    //画像の保存に失敗
    window.alert(document.getElementById("Hidden_MessageSaveImageFailure").value);
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
