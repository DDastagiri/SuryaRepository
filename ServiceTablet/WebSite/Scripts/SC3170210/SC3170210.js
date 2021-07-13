/**
 * @fileOverview SC3170210　写真選択画面
 *
 * @author SKFC久代
 * @update SKFC二村 TR-V4-TKM-20190813-003横展
 * @version 1.0.1
 */

// デバッグフラグ
var gDebugFlag = false;

// URL Getパラメータ
var gUrlParamList = [];
// サムネイル情報
var gThumbnailList = {};
// カレント選択情報
var gCurrentThumbnail = 0;

var gCookieCurrentImageName = "iCROP.SC3170210.currentImage";

// 画像遅延ロードのオプション
var gLazyOption = { 
    skip_invisible : false,
    placeholder : "./Styles/Images/progress.gif"
};

// Getパラメータリスト作成
var getUrlVars = function () {
    var vars = [], hash;

    if (!gDebugFlag) {
        var hashes = window.location.href.slice(window.location.href.indexOf('?') + 1).split('&');
        for (var i = 0; i < hashes.length; i++) {
            hash = hashes[i].split('=');
            vars.push(hash[0]);
            vars[hash[0]] = hash[1];
        }
    } else {
        // デバッグ用仮データ
        vars["DealerCode"] = "44A10";
        vars["BranchCode"] = "01";
        vars["SAChipID"] = "14";
        vars["BASREZID"] = "00";
        vars["R_O"] = "0014";
        vars["SEQ_NO"] = "0";
        vars["VIN_NO"] = "100";
        vars["PictMode"] = "2";
        vars["ViewMode"] = "1";
        vars["LinkSysType"] = "1";
        vars["LoginUserID"] = "1";
    }

    // 写真モード(省略値チェック)
    if (null == vars["PictMode"]) {
        vars["PictMode"] = "1";
    }

    // 表示モード(省略値チェック)
    if (null == vars["ViewMode"]) {
        vars["ViewMode"] = "0";
    }

    // LinkSysType(省略値チェック)
    if (null == vars["LinkSysType"]) {
        vars["LinkSysType"] = "0";
    }

    // ログインユーザID(省略値チェック)
    if (null == vars["LoginUserID"]) {
        vars["LoginUserID"] = "NONE";
    }

    return vars;
};

// 全体再表示
var displayAll = function() {
    if (gThumbnailList.data.length === 0) {
        // 画像データが存在しない場合は表示されているものを全て取り除く
        $("#TargetImage").children().remove(); // メイン画像除去
        $("ul.ThumbnailPhotos").children().remove(); // サムネイル除去
        $(".MainView").fadeOut();
        $(".DeleteBtn").fadeOut();
    }
    else {
        // メイン画像表示
        $("#TargetImage").children().remove();
        $("<img class='lazy' />")
            .attr({ "data-original": gThumbnailList.data[gCurrentThumbnail].largeImgPath, "width": "640", "height": "480" })
            .appendTo($("#TargetImage"));

        // サムネイル表示
        $("ul.ThumbnailPhotos").children().remove();
        for (var i = 0; i < gThumbnailList.data.length; i++) {
            var li = $("<li />");
            $("<img class='lazy' />")
                .attr({ "data-original": gThumbnailList.data[i].smallImgPath, "width": "160", "height": "120" })
                .appendTo($(li));
            $(li).appendTo($("ul.ThumbnailPhotos"));
        }

        // 遅延ロード設定
        $("img.lazy").lazyload(gLazyOption);

        // サムネイル枠初期化
        initCurrentThumbnail();
        setCurrentThumbnail($(".ThumbnailPhotos > li").eq(gCurrentThumbnail));

        // サムネイル画像タップのイベント
        $(".ThumbnailPhotos > li")
            .unbind('click', actionThmbnailImages)
            .click(actionThmbnailImages);

    }
};

// サムネイル情報取得処理
var getThumbnailData = function () {
    var argData = "{ ";
    argData += " 'DealerCode': '" + gUrlParamList["DealerCode"] + "', ";
    argData += " 'BranchCode': '" + gUrlParamList["BranchCode"] + "', ";
    argData += " 'SAChipID': '" + gUrlParamList["SAChipID"] + "', ";
    argData += " 'R_O': '" + gUrlParamList["R_O"] + "', ";
    argData += " 'SEQ_NO': '" + gUrlParamList["SEQ_NO"] + "', ";
    argData += " 'VIN_NO': '" + gUrlParamList["VIN_NO"] + "', ";
    argData += " 'PictMode': '" + gUrlParamList["PictMode"] + "', ";
    argData += " 'ViewMode': '" + gUrlParamList["ViewMode"] + "', ";
    argData += " 'BASREZID': '" + gUrlParamList["BASREZID"] + "', ";
    argData += " 'LinkSysType': '" + gUrlParamList["LinkSysType"] + "', ";
    argData += " 'LoginUserID': '" + gUrlParamList["LoginUserID"] + "' ";
    argData += " }";

    // ajax(非同期)サムネイル情報取得
    $.ajax({
        type: 'POST',
        datatype: 'json',
        url: 'SC3170210.aspx/GetRoThumbnail',
        data: argData,
        contentType: "application/json; charset=utf-8",
        success: function (result) {
            if (result) {
                // json配列生成
                gThumbnailList = {};
                gThumbnailList = $.parseJSON(result.d);
                // カレント位置更新
                var tmpIndex = getCurrentIndex();
                if (0 <= tmpIndex) {
                    gCurrentThumbnail = tmpIndex;
                }
                // サムネイル表示更新
                displayAll();
            }
        },
        error: function () {
            $("#debugArea").text("Ajax error GetRoThumbnail!");
        }
    });
};

// キャンセルボタンタップ時の処理
var cancelBtnClick = function () {

//    if (0 < gThumbnailList.data.length) {
//        // キャンセル対象の画像パスを取得(どのサイズの画像を通知する?)
//        var cancelImgPath = gThumbnailList.data[0].largeImgPath;

//        // URLスキームでキャンセルボタンタップを通知
//        var urlScheme = "icrop:noTitlePopup?imagePath=" + cancelImgPath;
//        window.location.href = urlScheme;
    //    }

    //キャンセルボタン押下時は、imagePathに「cancelled」をセットする
    var urlScheme = "icrop:noTitlePopup?imagePath=cancelled";
    window.location.href = urlScheme;

    // タイムアウトで閉じる
    setTimeout("closeBtnClick()", 100);
}

// 登録ボタンタップ時の処理
var registBtnClick = function () {
    if (0 < gThumbnailList.data.length) {
        // 登録対象の画像パスを取得(どのサイズの画像を通知する?)
        var registImgPath = $("#TargetImage img").attr("data-original");

        // URLスキームで登録ボタンタップを通知
        var urlScheme = "icrop:noTitlePopup?imagePath=" + registImgPath;
        window.location.href = urlScheme;
    }

    // タイムアウトで閉じる
    setTimeout("closeBtnClick()", 100);
}

// 閉じるボタンタップ時の処理
var closeBtnClick = function () {
    // URLスキームで閉じるボタンタップを通知
    var urlScheme = "icrop:noTitlePopup?close=YES";
    window.location.href = urlScheme;
}

// 削除ボタンタップ時の処理
var deleteBtnClick = function () {
    if (0 < gThumbnailList.data.length) {
        var argData = "{ ";
        argData += " 'id': '" + gThumbnailList.data[gCurrentThumbnail].id + "', ";
        argData += " 'LoginUserID': '" + gUrlParamList["LoginUserID"] + "' ";
        argData += " }";

        // ajax(非同期)サムネイル情報取得
        $.ajax({
            type: 'POST',
            datatype: 'json',
            url: 'SC3170210.aspx/DeleteRoThumbnail',
            data: argData,
            contentType: "application/json; charset=utf-8",
            success: function (result) {
                if (result) {
                    // データ再取得
                    gCurrentThumbnail = 0;
                    getThumbnailData();
                }
            },
            error: function (result) {
                $("#debugArea").text("Ajax error DeleteRoThumbnail : " + result);
            }
        });
    }
}

// メイン画像タップ時の処理
var actionMainImage = function () {

    if (0 < gThumbnailList.data.length) {
        // 写真表示ポップアップURLスキーム呼び出し
        // 登録対象の画像パスを取得(どのサイズの画像を通知する?)
        var registImgPath = $("#TargetImage img").attr("data-original");

        // URLスキームでキャンセルボタンタップを通知
        var strUrl = window.location.href;
        var baseUrl = strUrl.slice(0, strUrl.slice(0, strUrl.indexOf('?')).lastIndexOf('/')) + "/SC3170211.aspx";
        var targetUrl = baseUrl + "?";
        targetUrl += "PictureURL=" + gThumbnailList.data[gCurrentThumbnail].orignalImgPath;
        targetUrl += "&TitleString=" + gThumbnailList.data[gCurrentThumbnail].title;
        // 参照モード:0 編集モード:1
        targetUrl += "&Mode=" + (("2" == gUrlParamList["ViewMode"]) ? "1" : "0");

        var urlScheme = "icrop:popupPhoto?";
        urlScheme += "url=" + targetUrl;
        urlScheme += "::x=0";
        urlScheme += "::y=0";
        urlScheme += "::w=1024";
        urlScheme += "::h=768";
        urlScheme += "::callback=callbackMainImage";
        urlScheme += "::view=0";
        window.location.href = urlScheme;
    }
}

// メイン画像タップ時の処理のコールバック
var callbackMainImage = function (msg) {
    // カメラで新規撮影かつ編集モードの場合
    if (("1" == msg) && ("2" == gUrlParamList["ViewMode"])) {
        // 閉じる
        closeBtnClick();
    } else {
        // 再表示
        getThumbnailData();
    }
}

// サムネイル画像タップ時の処理
var actionThmbnailImages = function()
{
    var curIndex = -1;
    // 管理データからタップした該当データを探す
    var thmbSrc = $(this).find('img').attr("data-original");
    for(var i = 0; i < gThumbnailList.data.length; i++){
        if(thmbSrc == gThumbnailList.data[i].smallImgPath){
            curIndex = i;
            $("#TargetImage").children().remove();
            $("<img class='lazy' />")
                .attr({ "data-original": gThumbnailList.data[i].largeImgPath, "width": "640", "height": "480" })
                .appendTo($("#TargetImage"));
            $("img.lazy").lazyload(gLazyOption);
            // Cookieに保存
            document.cookie = gCookieCurrentImageName + "=" + gThumbnailList.data[i].largeImgPath + ";  expires=3600";
            break;
        }
    }

    // 探索判定
    if(0 > curIndex) {
        gCurrentThumbnail = 0;
    } else {
        gCurrentThumbnail = curIndex;
    }

    // サムネイル枠初期化
    initCurrentThumbnail();
    setCurrentThumbnail(this);
}

// サムネイルのカレント枠を設定する
var setCurrentThumbnail = function (target)
{
    $(target).append($("<p class='SelectedPhoto'></p>"));
    var moveTop = 133 * gCurrentThumbnail;
    $(".ThumbnailPhotos > li > p").css({ top : moveTop });
}

// サムネイル画像のカレント枠を初期化(消す)する
var initCurrentThumbnail = function()
{
    $(".ThumbnailPhotos > li > p").each(function () {
        $(this).remove();
    });
}


// Cookieから読み込む
var getCookie = function (key) {
    // Cookieから値を取得する
    var cookieString = document.cookie;
    // 要素ごとに ";" で区切られているので、";" で切り出しを行う
    var cookieKeyArray = cookieString.split(";");
    // 要素分ループを行う
    for (var i = 0; i < cookieKeyArray.length; i++) {
        var targetCookie = cookieKeyArray[i];
        // 前後のスペースをカットする
        targetCookie = targetCookie.replace(/^\s+|\s+$/g, "");
        var valueIndex = targetCookie.indexOf("=");
        if (targetCookie.substring(0, valueIndex) == key) {
            // キーが引数と一致した場合、値を返す
            return unescape(targetCookie.slice(valueIndex + 1));
        }
    }
    return "";
}

// カレント位置を取得
var getCurrentIndex = function () {
    var tmpIndex = -1;
    var imageSrc = getCookie(gCookieCurrentImageName);
    if (("" != imageSrc) && (0 < gThumbnailList.data.length)) {
        for (var i = 0; i < gThumbnailList.data.length; i++) {
            if (imageSrc == gThumbnailList.data[i].largeImgPath) {
                tmpIndex = i;
                break;
            }
        }
    }

    return tmpIndex;
}

// readyイベント
$(function () {
    // URLパラメータ保持
    gUrlParamList = getUrlVars();
    gCurrentThumbnail = 0;

    // 表示/非表示とイベント設定
    if("1" == gUrlParamList["ViewMode"]){
        // 選択モード
        $(".pointBtn03").remove();
        $(".pointBtn01").click(cancelBtnClick);
        $(".pointBtn02").click(registBtnClick);
    } else if("2" == gUrlParamList["ViewMode"]){
        // 編集モード
        $(".pointBtn01").remove();
        $(".pointBtn02").remove();
        $(".pointBtn03").click(deleteBtnClick);
    } else {
        // 参照モード
        $(".pointBtn01").remove();
        $(".pointBtn02").remove();
        $(".pointBtn03").remove();
    }

    // 閉じるボタンのイベント設定
    $(".mainblockDialogBoxClose").click(closeBtnClick);

    // メイン画像タップのイベント
    $("#TargetImage").click(actionMainImage);

    // サムネイル画像タップのイベント
    $(".ThumbnailPhotos > li").click(actionThmbnailImages);

    // サムネイルのタッチスクロール設定
    $(".ThumbnailView").fingerScroll();

    // サムネイル情報取得
    getThumbnailData();
});
