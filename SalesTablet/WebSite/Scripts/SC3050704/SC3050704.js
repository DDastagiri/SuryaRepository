/*━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
SC3050704.js
─────────────────────────────────────
機能： MOP/DOP設定
補足： 
作成： 2012/11/27 TMEJ 宇野
更新： 
─────────────────────────────────────*/
/**
* 権限:DIST権限
* @return {String}
*/
var OPERATION_DIST = "1"

/**
* 権限:DIST権限以外
* @return {String}
*/
var OPERATION_OTHER = "0"

/**
* 状態:初期
* @return {String}
*/
var STATE_INITIAL = "0";

/**
* 状態:編集済み
* @return {String}
*/
var STATE_EDITED = "1";

/**
* 状態:不正な編集
* @return {String}
*/
var STATE_INVALID = "2";

/**
* 状態:最新でない
* @return {String}
*/
var STATE_NOT_LATEST = "3"

/**
* 選択車種の復元処理かどうか
* @return {boolean}
*/
var isRestoreSeries = false;

/**
* 初期表示時の処理を行います。
*/
$(function () {

    //タイトル設定
    $("#MstPG_TitleLabel").text($("#HiddenHeaderTitle").val());
    $("#MstPG_WindowTitle").text($("#HiddenHeaderTitle").val());

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

            if (isRestoreSeries) {
                //選択車種の復元完了
                isRestoreSeries = false;

            } else {

                //初期処理
                initialize()

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
* オプション追加ボタン押下イベント
* 同期処理にてMOP/DOP詳細設定画面に遷移します。
*/
function addOption() {
    //編集の破棄を確認
    if (!isDiscard()) {
        return false;
    }

    //ローディング開始
    showLoding();

    //オプション追加イベント送出
    $("#AddButton").click();
}

/**
* オプション選択イベント
* 同期処理にてMOP/DOP詳細設定画面に遷移します。
* @param {number} 行インデックス
*/
function selectOption(index) {
    //編集の破棄を確認
    if (!isDiscard()) {
        return false;
    }

    //ローディング開始
    showLoding();

    //対象行インデックス設定
    $("#HiddenIndex").val(index);

    //オプション選択イベント送出
    $("#OptionAnchor").click();

}

/**
* 保存ボタン押下イベント
* 同期処理にて一覧情報を登録します。
*/
function save() {
    //クライアント入力チェック
    if (isValid()) {

        //ローディング開始
        showLoding();

        //保存ボタンイベント送出
        $("#SaveButton").click();
    }
    return false;
}

/*
* ヘッダー押下イベント
* @return {boolean} 処理を続行する場合はtrue、中断する場合はfalseを返します。
*/
function onHeaderHandler() {
    if (!isDiscard()) {
        return false;
    }

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

    //ローディング開始
    showLoding();

    //選択されたフッターの機能IDを保持
    $("#HiddenAppId").val(appId);

    //フッターボタンイベント送出
    $("#FooterButton").click();

    return true;
}

/**
* 入力内容の妥当性を判定します。
* 入力内容に不備がある場合はエラーメッセージを表示します。
* @return {boolean} 妥当であればtrue、それ以外はfalse
*/
function isValid() {

    //画面情報を取得
    var order = this_form.elements["Order"];
    
    //一行ずつチェック
    for (var i = 0; i < order.length; i++) {

        //表示順の数値チェック
        if (!isEmpty(order[i].value) && !isNumeric(order[i].value)) {
            alert($("#HiddenMsgInvalid").val());
            return false;
        }
    }
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