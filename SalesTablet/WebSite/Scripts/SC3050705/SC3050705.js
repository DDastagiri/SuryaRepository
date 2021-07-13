/*━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
SC3050705.js
─────────────────────────────────────
機能： MOP/DOP詳細設定
補足： 
作成： 2012/11/27 TMEJ 玉置
更新： 
─────────────────────────────────────*/

/**
* 画面変更区分:変更なし
* @return {String}
*/
var MODIFY_OFF = "0";

/**
* 画面変更区分:変更あり
* @return {String}
*/
var MODIFY_ON = "1";

/**
* エラー区分 ON
* @return {String}
*/
var ERROR_DVS_ON = "1";

/**
* エラー区分 OFF
* @return {String}
*/
var ERROR_DVS_OFF = "0";

/**
* グレード適合:チェック
* @return {String}
*/
var GRADE_ON = "1";

/**
* 画像拡張子[png]
* @return {String}
*/
var IMAGE_PNG = "png";

/**
* 画像拡張子[PNG]
* @return {String}
*/
var IMAGE_PNG_BIG = "PNG";

/**
* 画像拡張子[jpg]
* @return {String}
*/
var IMAGE_JPG = "jpg";

/**
* 画像拡張子[JPG]
* @return {String}
*/
var IMAGE_JPG_BIG = "JPG";

/**
* 画像拡張子[jpeg]
* @return {String}
*/
var IMAGE_JPEG = "jpeg";

/**
* 画像拡張子[JPEG]
* @return {String}
*/
var IMAGE_JPEG_BIG = "JPEG";

/**
* 保存処理中かどうか
* @return {boolean}
*/
var isSaving = false;

/**
* 初期表示時の処理を行います。
*/
$(function () {

    //リフレッシュ判定
    if ($("#refleshDvsField").val() == ERROR_DVS_ON) {
        $("#refleshDvsField").val("")
        $("#RefleshButton").click();
    }

    //タイトル設定
    $("#MstPG_TitleLabel").text($("#HiddenTitle").val());
    $("#MstPG_WindowTitle").text($("#HiddenTitle").val());

    //ローディング開始
    showLoding();

    jQuery.event.add(window, "load", function () {
        //ローディング終了
        closeLoding();

    });

    //ポップアップ画像読み込み終了時イベント設定
    $(windowbBoxImage).bind('load', function () {
        var maxWidth = 520;
        var maxHeight = 403;
        var imgWidth = $("#windowbBoxImage").width();
        var imgHeight = $("#windowbBoxImage").height();

        //幅と高さの両方が基準値を超える場合
        if (maxWidth < imgWidth && maxHeight < imgHeight) {
            //基準値との比率を算出
            var widthRatio = imgWidth / maxWidth;
            var heightRatio = imgHeight / maxHeight;

            //高さの方が比率が大きい場合
            if (widthRatio < heightRatio) {
                //高さを基準にする
                $("#windowbBoxImage").height(maxHeight);
            } else {
                //それ以外は幅を基準にする
                $("#windowbBoxImage").width(maxWidth);
            }
        } else if (maxWidth < imgWidth) {
            //幅のみが基準値を超える場合は幅を基準にする
            $("#windowbBoxImage").width(maxWidth);
        } else if (maxHeight < imgHeight) {
            //高さのみが基準値を超える場合は高さを基準にする
            $("#windowbBoxImage").height(maxHeight);
        }

        //ローディング終了
        closeLoding();

        $("#windowbBox").css("z-index", "1");
    });

    //ポップアップ画像読み込み失敗時イベント設定
    $(windowbBoxImage).bind('error', function () {
        //ローディング終了
        closeLoding();

    });

    //PageRequestManagerクラスをインスタンス化
    var mng = Sys.WebForms.PageRequestManager.getInstance();

    //initializeRequestイベント・ハンドラを定義
    mng.add_initializeRequest(
        function (sender, args) {
            //ローディング開始
            showLoding();
        }
    );

    //非同期ポストバックの完了
    mng.add_endRequest(
        function (sender, args) {
            //非同期ポストバックの完了

            //保存イベント中の場合
            if (isSaving) {

                isSaving = false;
                //チェック結果が正常なら保存処理を実行
                if ($("#ajaxErrorField").val() != ERROR_DVS_ON) {

                    $("#SaveButton").click();

                } else {
                    //ローディング終了
                    closeLoding();
                }
            }
        }
    );

    //初期処理
    initialize()

});

/*
* 初期処理を行います。
*/
function initialize() {

    $("#HiddenState").val(MODIFY_OFF);

    //ポップアップイベントの初期処理
    initEvent();

    //スクロール設定
    setFingerScroll()
}

/**
* 読み込み中アイコン表示
*/
function showLoding() {
    //オーバーレイ表示
    $("#registOverlayBlack").css("display", "block");
    //アニメーション
    $("#processingServer").addClass("show");
    $("#registOverlayBlack").addClass("open");

}

/**
* 読み込み中アイコン非表示
*/
function closeLoding() {
    $("#processingServer").removeClass("show");
    $("#registOverlayBlack").removeClass("open");
    //オーバーレイ表示
    $("#registOverlayBlack").css("display", "none");

}

/**
* スクロールを設定します。
*/
function setFingerScroll() {

    if ($("#boxscrollTable").height() > $("#boxscroll").height()) {
        $("#boxscroll").fingerScroll();
    }
}

/**
* 保存ボタン押下イベント
* 非同期処理にて一覧情報を登録します。
*/
function save() {

    //クライアント入力チェック
    if (isValid()) {

        isSaving = true;

        //ローディング開始
        showLoding();

        //サーバ入力チェックイベント送出
        $("#ValidationButton").click();
    }

    return false;
}

/**
* ファイル選択変更イベント
*/
function editUploadFile() {

    //編集状態にする
    edit();
    //オプション名をクリアする
    $("#OptionImageName").text("");

    return false;
}

/**
* 削除ボタン押下イベント
* 非同期処理にて一覧情報を登録します。
*/
function del() {
    var message = $("#HiddenDeleteMessage").val();

    if (confirm(message)) {
        //ファイル削除
        delFiles();

        //ローディング開始
        showLoding();

        //削除ボタンイベント送出
        $("#DeleteButton").click();
    }
    return false;
}

/**
* オプション画像の削除
* 
*/
function delImage() {
    var message = $("#HiddenImageDeleteMessage").val();

    if (confirm(message)) {
        edit();
        $("#OptionImageName").text("");
        $("#HiddenFileName").val("");
        $("#reference").val("");
    }
}

/**
* 入力内容の妥当性を判定します。
* 入力内容に不備がある場合はエラーメッセージを表示します。
* @return {boolean} 妥当であればtrue、それ以外はfalse
*/
function isValid() {

    var displayMode = $("#HiddenDisplayMode").val();
    var imageMaxSize = parseFloat($("#HiddenImageMaxFileSizeField").val());
    var decimaLength = parseFloat($("#HiddenDecimalPoint").val());

    if (displayMode == "1") {
        // オプション名必須チェック
        if (isEmpty($("#OptionName").val())) {
            alert($("#HiddenRequiredMessage").val());

            return false;
        }

        // 価格の数値チェック
        if (!isEmpty($("#Price").val())) {
            if (!isDecimal($("#Price").val())) {
                //メッセージ表示
                alert($("#HiddenPriceMessage").val());

                return false;
            } else {
                // 価格の少数点桁数チェック
                if (!isDecimalLength($("#Price").val(), decimaLength)) {
                    //メッセージ表示
                    alert($("#HiddenDecimalMessage").val());
                    return false;
                }
            }
        }

        // 画像ファイル
        var uploadFileList = document.getElementById("reference").files;
        for (var i = 0; i < uploadFileList.length; i++) {
            //アップロード拡張子チェック
            if (getExtention(uploadFileList[i].name) != IMAGE_PNG &&
            getExtention(uploadFileList[i].name) != IMAGE_JPG &&
            getExtention(uploadFileList[i].name) != IMAGE_JPEG &&
            getExtention(uploadFileList[i].name) != IMAGE_PNG_BIG &&
            getExtention(uploadFileList[i].name) != IMAGE_JPG_BIG &&
            getExtention(uploadFileList[i].name) != IMAGE_JPEG_BIG) {
                //メッセージ表示
                alert($("#HiddenUploadMessage").val());

                return false;
            }

            //アップロードサイズチェック(実サイズをKBに変換)
            if (imageMaxSize < (uploadFileList[i].size / 1024)) {
                alert($("#HiddenUploadFileSizeMessage").val());
                return false;
            }

        }
        //グレード必須チェック
        var objControl = document.all;
        var flg = 0;

        for (var i = 0; i < objControl['Grade'].length; i++) {
            if (objControl['Grade'].length == 1) {
                if (objControl['Grade'].checked == true) {
                    flg = 1;
                }
            } else {
                if (objControl['Grade'][i].checked == true) {
                    flg = 1;
                }
            }
        }

        if (flg == 0) {
            //メッセージ表示
            alert($("#HiddenGreadMessage").val());
            return false;

        }
    }
    
    return true;
}

/**
* ファイル名から拡張子を取得
*/
function getExtention(fileName) {
    var ret;
    if (!fileName) {
        return ret;
    }
    var fileTypes = fileName.split(".");
    var len = fileTypes.length;
    if (len === 0) {
        return ret;
    }
    ret = fileTypes[len - 1];
    return ret;
}

/**
* 数値チェック
*/
function isDecimal(argValue) {
    if (argValue.match(/[^0-9|^.]/g)) {
        // パターンマッチ 0～9以外はＮＧ
        return false;
    }

    // 小数点の数を取得する
    var count = 0;
    for (var i = 0; i < argValue.length; i++) {
        if (argValue.charAt(i) == ".") {
            count++;
        }
    }
    if (2 <= count) {
        // "."が２つ以上入力されている場合はＮＧ
        return false;
    }
    if (argValue.charAt(0) == ".") {
        // 先頭に小数点が入力された場合はＮＧ
        return false;
    }

    // 小数点以下のチェック
    if (count == 1) {
        // 小数点が入力された場合のみチェック
        // 小数点以下の桁数チェック
        var idx = argValue.lastIndexOf(".");
        var decimalPart = argValue.substring(idx);
        // 小数点以下の桁数を取得する
        var length = decimalPart.length - 1;
        if (length == 0) {
            // 小数点以下の入力がない場合はＮＧ
            return false;
        }
    }

    return true;
}

/**
* 少数桁数チェック
*/
function isDecimalLength(argValue, decimalLength) {

    // 小数点の数を取得する
    var count = 0;
    for (var i = 0; i < argValue.length; i++) {
        if (argValue.charAt(i) == ".") {
            count++;
        }
    }
    
    // 小数点以下のチェック
    if (count == 1) {
        // 小数点が入力された場合のみチェック
        // 小数点以下の桁数チェック
        var idx = argValue.lastIndexOf(".");
        var decimalPart = argValue.substring(idx);
        // 小数点以下の桁数を取得する
        var length = decimalPart.length - 1;
        if (decimalLength < length) {
            // 小数点以下の桁数がオーバーしている場合はＮＧ
            return false;
        }
    }

    return true;
}
/**
* 文字列が空かどうかを判定します。
* @param {string} value 文字列
* @return {boolean} nullまたは空であればtrue、それ以外はfalse
*/
function isEmpty(value) {
    if (value == null || value.length == 0) {
        return true;
    }
    return false;
}

/**
* 状態を編集済みに設定します。
*/
function edit() {
    $("#HiddenState").val(MODIFY_ON);
}

/**
* 編集状態を破棄するかどうかを確認します。
* @return {boolean} 未編集または編集を破棄する場合はtrue、それ以外はfalse
*/
function isDiscard() {
    if (isEdited()) {
        //確認メッセージを設定
        var message = $("#HiddenConfirmMessage").val();

        if (confirm(message)) {
            //編集状態を破棄
            $("#HiddenState").val(MODIFY_OFF);
            return true;
        } else {
            //キャンセル
            return false;
        }
    } else {
        //未編集
        return true;
    }
}

/**
* 状態を判定します。
* @return {boolean} 編集があればtrue、それ以外はfalse
*/
function isEdited() {
    if ($("#HiddenState").val() == MODIFY_OFF) {
        return false;
    }
    return true;
}

/*
* ヘッダー押下イベント
* @return {boolean} 処理を続行する場合はtrue、中断する場合はfalseを返します。
*/
function onHeaderHandler() {
    if (!isDiscard()) {
        return false;
    }

    //ファイル削除
    delFiles();

    //ローディング開始
    showLoding();

    return true;
}

/*
* フッター押下イベント
* @param {string} appId 機能ID
* @return {boolean} 処理を続行する場合はtrue、中断する場合はfalseを返します。
*/
function onFooterHandler(appId) {
    if (!isDiscard()) {
        return false;
    }

    //ファイル削除
    delFiles();

    //ローディング開始
    showLoding();

    //選択されたフッターの機能IDを保持
    $("#HiddenAppId").val(appId);

    //フッターボタンイベント送出
    $("#FooterButton").click();

    return true;
}


function initEvent() {
    // ポップアップ閉じるボタン
    $("#windowBackFrameCloseBtn").click(function (e) {
        e.stopPropagation();
        popupClose();
    });

    // ポップアップ背景
    $("#windowbBox").click(function (e) {
        e.stopPropagation();

        var pos = {};
        if (e.originalEvent.x) pos = { x: e.originalEvent.pageX, y: e.originalEvent.pageY };
        else pos = { x: e.originalEvent.touches[0].pageX, y: e.originalEvent.touches[0].pageY };

        var offset = $("#windowBackFrame").offset();
        var width = $("#windowBackFrame").width();
        var height = $("#windowBackFrame").height();

        if (pos.x < offset.left || offset.left + width < pos.x || pos.y < offset.top || offset.top + height < pos.y) {
            popupClose();
        }
    });
}

function popupClose() {
    $("#windowbBox").css("display", "none");
    $("#windowbBoxImage").attr("src", "");
};

/**
* 画像ポップアップ表示
*/
function onClickImage() {

    //ローディング開始
    showLoding();

    var popWindow = this;
    var displayMode = $("#HiddenDisplayMode").val();

    if (displayMode == "1") {
        $("#windowbBoxName").text($("#OptionName").val());
        $("#windowbBoxPrice").text(insertComma($("#Price").val()));
    } else {
        $("#windowbBoxName").text($("#LabelOptionName").text());
        $("#windowbBoxPrice").text($("#LabelPrice").text());
    }
    $("#windowBackFrame").css({ "width": "574px", "left": "225px" });

    // コンテンツ
    $("#windowbBoxImage").parent().css({ "-webkit-box-align": "center", "-webkit-box-pack": "center" });

    var filePath = $("#HiddenFilePath").val();

    $("#windowbBoxImage").attr("src", filePath);
    $("#windowbBoxImage").parent().css({ "display": "-webkit-box", "width": "534px" });

    $("#windowbBox").css("display", "block");
    $("#windowbBox").css("z-index", "-100");
}

/**
* カンマ編集
*/
function insertComma(str) {
    var num = "";
    if (!isEmpty(str) && isDecimal(str)) {
        num = new String(str).replace(/,/g, "");
        while (num != (num = num.replace(/^(-?\d+)(\d{3})/, "$1,$2")));
    }
    return num;
}

/**
* ファイル参照を削除します。
*/
function delFiles() {
    // ファイル削除
    $("#reference").val("");
}