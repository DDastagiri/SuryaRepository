/*━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
SC3050702.js
─────────────────────────────────────
機能： セールスポイント設定
補足： 
作成： 2012/11/23 TMEJ 三和
更新： 
─────────────────────────────────────*/

/**
* 外装定数
* @return {String}
*/
var C_EXTERIOR = "exterior";

/**
* 内装定数
* @return {Integer}
*/
var C_INTERIOR = "interior";

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
* 非同期ポストバックコントロール格納
* @return {String}
*/
var gAjaxControlId = "";

/**
* 外装/内装区分
* @return {Boolean}
*/
var isExterior = true;

/**
* セールスポイント情報格納
* @return {String}
*/
var gSalesPointInfo = null;

/**
* 選択車種の復元処理かどうか
* @return {boolean}
*/
var isRestoreSeries = false;

/**
* 初期表示設定
*/
$(function () {

    //ローディング開始
    showLoding();

    jQuery.event.add(window, "load", function () {
        //ローディング終了
        closeLoding();

    });

    //スクロール設定
    setFingerScroll();

    //コントロール制御
    initControl();

    //PageRequestManagerクラスをインスタンス化
    var mng = Sys.WebForms.PageRequestManager.getInstance();

    //initializeRequestイベント・ハンドラを定義
    mng.add_initializeRequest(
    //非同期ポストバックの開始前にイベント発生元の要素を格納
        function (sender, args) {
            gAjaxControlId = args.get_postBackElement().id;
        }
    );

    //非同期ポストバックの完了
    mng.add_endRequest(
        function (sender, args) {

            if (isRestoreSeries) {
                //選択車種の復元完了
                isRestoreSeries = false;

            } else {
                //スクロール設定
                setFingerScroll();

                //コントロール制御
                initControl();

                //ローディング終了
                closeLoding();
            }

        }
    );

});

/**
* 外装/内装変更処理
*/
function switchOrnament(switcher) {
    //選択値が同じ場合処理しない
    if ((switcher.id == C_EXTERIOR && isExterior) || (switcher.id == C_INTERIOR && !isExterior)) {
        return;
    }

    if ($("#modifyDvsField").val() == MODIFY_ON) {
        if (!confirm($("#modifyMessageField").val())) {
            return;
        }
    }

    isExterior = !isExterior;
    if (isExterior) {
        //外装の場合
        $("#interior").removeClass("switcher_in_on");
        $("#exterior").removeClass("switcher_ex_off");
        $("#interior").addClass("switcher_in_off");
        $("#exterior").addClass("switcher_ex_on");

        //ローディング開始
        showLoding();

        $("#exInField").val("exterior");
        // Refreshダミーボタンをクリック
        $("#RefreshButton").click();

    } else {
        //内装の場合
        $("#interior").removeClass("switcher_in_off");
        $("#exterior").removeClass("switcher_ex_on");
        $("#interior").addClass("switcher_in_on");
        $("#exterior").addClass("switcher_ex_off");

        //ローディング開始
        showLoding();

        $("#exInField").val("interior");
        // refreshダミーボタンをクリック
        $("#RefreshButton").click();
    }

}

/**
* 車種選択変更イベント
*/
function changeCarLineUp() {
    if ($("#modifyDvsField").val() == MODIFY_ON) {
        if (!confirm($("#modifyMessageField").val())) {
            //破棄しない場合は選択車種を復元する
            isRestoreSeries = true;
            $("#RestoreButton").click();
            return;
        }
    }

    //ローディング開始
    showLoding();

    //選択値を保持
    $("#carSelectField").val($("#DropDownList_Vehicle").val())

    // refreshダミーボタンをクリック
    $("#RefreshButton").click();

}

/**
* セールスポイントアンカー押下イベント
*/
function editSalesPoint(salesPointId) {
    if ($("#modifyDvsField").val() == MODIFY_ON) {
        if (!confirm($("#modifyMessageField").val())) {
            return false;
        }
    }

    //ローディング開始
    showLoding();

    //セールスポイントIDを設定
    $("#salesPointIdField").val(salesPointId);

    //ダミーボタンをクリック
    $("#EditButton").click();

}

/**
* 追加ボタン押下イベント
*/
function addSalesPointInfo() {
    if ($("#modifyDvsField").val() == MODIFY_ON) {
        if (!confirm($("#modifyMessageField").val())) {
            return false;
        }
    }

    //セールスポイント最大登録件数チェック
    //Hiddenの値をjavaScriptの変数にセット
    gSalesPointInfo = eval('(' + $("#salesPointJsonField").val() + ')');

    if (gSalesPointInfo.sales_point.length >= $("#maxCountField").val()) {
        //メッセージ表示
        alert($("#maxCountMessageField").val());
        return false;
    }

    //ローディング開始
    showLoding();

    //ダミーボタンをクリック
    $("#AddButton").click();
}

/**
* 保存ボタン押下イベント
*/
function sendSalesPointInfo() {

    //No.の数値チェック
    var objControl = document.all;
    var flg = 0;

    for (var i = 0; i < objControl['sortNo'].length; i++) {
        if (isNaN(objControl['sortNo'][i].value)) {
            flg = 1;
        }
    }

    if (flg == 1) {
        //メッセージ表示
        alert($("#sortNoMessageField").val());
        return false;

    }

    //ローディング開始
    showLoding();

    //ダミーボタンをクリック
    $("#SendButton").click();
    
    return false;

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
    $("#registOverlayBlack").removeClass("open").one("webkitTransitionEnd", function (e) {
        $("#registOverlayBlack").css("display", "none");
    });

    //オーバーレイ非表示
    $("#serverProcessOverlayBlack").css("display", "none");

}

/**
* 初期コントロール制御
*/
function initControl() {

    //外装/内装の選択値設定
    if ($("#exInField").val() == C_EXTERIOR) {
        isExterior = true;

        //外装の場合
        $("#interior").removeClass("switcher_in_on");
        $("#exterior").removeClass("switcher_ex_off");
        $("#interior").addClass("switcher_in_off");
        $("#exterior").addClass("switcher_ex_on");

    } else {
        isExterior = false;

        //内装の場合
        $("#interior").removeClass("switcher_in_off");
        $("#exterior").removeClass("switcher_ex_on");
        $("#interior").addClass("switcher_in_on");
        $("#exterior").addClass("switcher_ex_off");

    }
}

/**
* スクロール設定
*/
function setFingerScroll() {
    if ($("#boxscrollTable").height() > $("#boxscroll").height()) {
        $("#boxscroll").fingerScroll();
    }

}

/**
* 画面変更フラグON
*/
function onChangeDisplay() {
    //画面変更フラグON
    $("#modifyDvsField").val(MODIFY_ON);

}

/**
* 画面変更チェック
*/
function onChangeDisplayCheck() {
    if ($("#modifyDvsField").val() == MODIFY_ON) {
        //画面変更確認
        if (!confirm($("#modifyMessageField").val())) {
            //キャンセル
            return false;
        }
    }

    //ローディング開始
    showLoding();

    return true;

}

