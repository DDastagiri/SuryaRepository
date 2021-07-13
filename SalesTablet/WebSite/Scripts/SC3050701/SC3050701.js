/*━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
SC3050701.js
─────────────────────────────────────
機能： コンテンツメニュー設定
補足： 
作成： 2012/12/18 TMEJ 宇野
更新： 
─────────────────────────────────────*/
/**
* 状態:初期
* @return {string}
*/
var STATE_INITIAL = "0";

/**
* 状態:編集済み
* @return {string}
*/
var STATE_EDITED = "1";

/**
* 状態:不正な編集
* @return {string}
*/
var STATE_INVALID = "2";

/**
* 状態:最新でない
* @return {string}
*/
var STATE_NOT_LATEST = "3"

/**
* 保存処理中かどうか
* @return {boolean}
*/
var isSaving = false;

/**
* 選択車種の復元処理かどうか
* @return {boolean}
*/
var isRestoreSeries = false;

/*
* 画像ファイルの拡張子
* @return {Array}
*/
var extensions = ["jpg", "JPG", "jpeg", "JPEG", "png", "PNG"];

/**
* 初期表示時の処理を行います。
*/
$(function () {

    //ローディング開始
    showLoding();

    //ロード完了時
    jQuery.event.add(window, "load", function () {
        //ローディング終了
        closeLoding();
    });

    //PageRequestManagerクラスをインスタンス化
    var mng = Sys.WebForms.PageRequestManager.getInstance();

    //非同期処理開始イベント定義
    mng.add_initializeRequest(
        function (sender, args) {
            //選択車種復元時はローディングを表示しない
            if (!isRestoreSeries) {
                //ローディング開始
                showLoding();
            }
        }
    );

    //非同期処理終了イベント定義
    mng.add_endRequest(
        function (sender, args) {

            //保存イベント中の場合
            if (isSaving) {

                isSaving = false;

                //入力エラー判定
                if (isInvalid()) {
                    //ローディング終了
                    closeLoding();
                } else {
                    //入力エラーがなければ保存
                    $("#SaveButton").click();
                }
            } else if (isRestoreSeries) {
                
                //選択車種の復元完了
                isRestoreSeries = false;

            } else {

                //初期処理
                initialize();

                //ローディング終了
                closeLoding();
            }

        }
    );

    //排他エラー発生時は再描画
    if (isNotLatest()) {

        //再描画イベント送出
        $("#RefreshButton").click();

    } else {

        //初期処理
        initialize();

    }
});

/*
* 初期処理を行います。
*/
function initialize() {

    //状態を初期化
    if (!isInvalid()) {
        $("#HiddenState").val(STATE_INITIAL);
    }

    //選択車種を保持
    $("#HiddenSeries").val($("#DropDownCarLineup").val());

    //スクロール設定
    setFingerScroll()
}

/**
* 読み込み中アイコンを表示します。
*/
function showLoding() {
    //オーバーレイ表示
    $("#registOverlayBlack").css("display", "block");
    //アニメーション
    $("#processingServer").addClass("show");
    $("#registOverlayBlack").addClass("open");

}

/**
* 読み込み中アイコンを非表示にします。
*/
function closeLoding() {
    $("#processingServer").removeClass("show");
    $("#registOverlayBlack").removeClass("open");
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
* 車種選択リスト変更イベント
* 非同期処理にて一覧情報を再取得します。
*/
function changeCarLineup() {
    //編集の破棄を確認
    if (isDiscard()) {

        //ファイル削除
        delFiles();

        //選択車種を保持
        $("#HiddenSeries").val($("#DropDownCarLineup").val());

        //再描画イベント送出
        $("#RefreshButton").click();

    } else {
        
        //破棄しない場合は選択車種を復元する
        isRestoreSeries = true;
        $("#RestoreButton").click();

    }
}

/**
* 削除イベント
* 同期処理にてコンテンツメニュー情報をクリアします。
* @param {number} index 行インデックス
*/
function clearRow(index) {
    //削除確認メッセージ
    if (confirm($("#HiddenMsgConfirmDelete").val())) {
        //編集状態にする
        edit();

        //行情報をクリア
        this_form.elements["SC3050701_ID"][index].value = "";
        this_form.elements["SC3050701_Menu"][index].value = "";
        this_form.elements["SC3050701_Url"][index].value = "";
        this_form.elements["SC3050701_Order"][index].value = "";
        this_form.elements["SC3050701_IconNameNew"][index].value = "";
        document.getElementsByName("SC3050701_Icon")[index].className = "hidden";
        document.getElementsByName("SC3050701_Frame")[index].className = "frameOn";
        var file = this_form.elements["SC3050701_File"][index];
        if (!file.disabled) {
            file.value = "";
        }
    }
}

/**
* 保存ボタン押下イベント
* 同期処理にて一覧情報を登録します。
*/
function validate() {
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
* 入力内容の妥当性を判定します。
* 入力内容に不備がある場合はエラーメッセージを表示します。
* @return {boolean} 妥当であればtrue、それ以外はfalse
*/
function isValid() {

    //画面情報を取得
    var count = document.getElementById("boxscrollTable").rows.length;
    var id = this_form.elements["SC3050701_ID"];
    var menu = this_form.elements["SC3050701_Menu"];
    var url = this_form.elements["SC3050701_Url"];
    var order = this_form.elements["SC3050701_Order"];
    var file = this_form.elements["SC3050701_File"];
    var maxSize = parseFloat($("#HiddenMaxFileSize").val());

    //一行ずつチェック
    for (var i = 0; i < count; i++) {

        //ファイル情報取得
        var files = file[i].files;
        var fileCount = files.length;

        //入力有無の判定
        var hasInput = false;
        if (!isEmpty(menu[i].value) || !isEmpty(url[i].value) || 0 < fileCount || !isEmpty(order[i].value)) {
            hasInput = true;
        }

        //メニュー名の必須チェック
        if (hasInput && isEmpty(menu[i].value)) {
            alert($("#HiddenErrRequiredMenu").val());
            return false;
        }

        //選択ファイルのチェック
        //※行あたりのファイルを全走査するが
        //┗0(未選択)か1(選択)しかありえない
        for (var j = 0; j < fileCount; j++) {
            //拡張子チェック
            if (!isAllowedImage(files[j].name)) {
                alert($("#HiddenErrFileKind").val());
                return false;
            }
            //サイズチェック(実サイズをKBに変換)
            if (maxSize < (files[j].size / 1024)) {
                alert($("#HiddenErrFileSize").val());
                return false;
            }
        }

        //遷移先URLの必須チェック
        if (hasInput && isEmpty(url[i].value)) {
            alert($("#HiddenErrRequiredURL").val());
            return false;
        }

        //遷移先URLの禁則文字チェック
        if (!isValidURL(url[i].value)) {
            alert($("#HiddenErrInvalidURL").val());
            return false;
        }

        //表示順の数値チェック
        if (!isEmpty(order[i].value) && !isNumeric(order[i].value)) {
            alert($("#HiddenErrNumericOrder").val());
            return false;
        }
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

    delFiles();

    //ローディング開始
    showLoding();

    //選択されたフッターの機能IDを保持
    $("#HiddenAppId").val(appId);

    //フッターボタンイベント送出
    $("#FooterButton").click();

    return true;
}

/**
* 状態を編集済みに設定します。
*/
function edit() {
    $("#HiddenState").val(STATE_EDITED);
}

/**
* 編集状態を破棄するかどうかを確認します。
* @return {boolean} 未編集または編集を破棄する場合はtrue、それ以外はfalse
*/
function isDiscard() {
    if (isEdited()) {
        //確認メッセージを表示
        if (confirm($("#HiddenMsgConfirmDiscard").val())) {
            //編集状態を破棄
            $("#HiddenState").val(STATE_INITIAL);
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
    if ($("#HiddenState").val() == STATE_INITIAL) {
        return false;
    }
    return true;
}

/**
* 状態を判定します。
* @return {boolean} 不正な編集があればtrue、それ以外はfalse
*/
function isInvalid() {
    if ($("#HiddenState").val() == STATE_INVALID) {
        return true;
    }
    return false;
}

/**
* 状態を判定します。
* @return {boolean} 最新でなければtrue、最新であればfalse
*/
function isNotLatest() {
    if ($("#HiddenState").val() == STATE_NOT_LATEST) {
        return true;
    }
    return false;
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
* 文字列が数値かどうかを判定します。
* @param {string} value 文字列
* @return {boolean} 数値であればtrue、それ以外はfalse
*/
function isNumeric(value) {
    if (value.match(/[^0-9]/g)) {
        return false;
    }
    return true;
}

/**
* URLに禁則文字が含まれていないかどうかを判定します。
* @param {string} value URL
* @return {boolean} 含まれていなければtrue、それ以外はfalse
*/
function isValidURL(value) {
    if (value.match(/[\\"'|*`^><\)\(}{\]\[]+/g)) {
        return false;
    }
    return true;
}

/**
* 画像ファイルが許可された拡張子かどうかを判定します。
* @param {string} value ファイル名
* @return {boolean} 許可されていればtrue、それ以外はfalse
*/
function isAllowedImage(value) {
    var extension = getExtension(value);
    for (var i = 0; i < extensions.length; i++) {
        if (extension == extensions[i]) {
            return true;
        }
    }
    return false;
}

/**
* 拡張子を取得します。
* @param {string} value ファイル名
* @return {string} 拡張子
*/
function getExtension(value) {
    var divided = value.split(".");
    var len = divided.length;
    if (0 < len) {
        return divided[len - 1];
    } else {
        return "";
    }
}

/**
* ファイル参照を削除します。
*/
function delFiles() {

    var file = this_form.elements["SC3050701_File"];
    var fileCount = file.length;

    for (var i = 0; i < fileCount; i++) {
        file[i].value = "";
    }
}