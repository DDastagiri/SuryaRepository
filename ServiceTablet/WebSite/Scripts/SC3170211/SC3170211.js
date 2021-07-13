/**
 * @fileOverview SC3170211　写真画像ポップアップ
 *
 * @author SKFC橋本
 * @version 1.0.0
 */
// URL Getパラメータ
var gUrlParamList = [];

/**
 * Getパラメータリスト作成
 */
var getUrlVars = function () {
    var vars = [], hash;

    var hashes = window.location.href.slice(window.location.href.indexOf('?') + 1).split('&');
    for (var i = 0; i < hashes.length; i++) {
        hash = hashes[i].split('=');
        vars.push(hash[0]);
        vars[hash[0]] = hash[1];
    }

    // モード(省略値チェック)
    if (null == vars["Mode"]) {
        vars["Mode"] = "0";
    }

    // タイトル文字列(省略値チェック)
    if (null == vars["TitleString"]) {
        vars["TitleString"] = "";
    }

    return vars;
};

/**
 * 完了ボタン押下時コールバック関数
 */
function OnCloseBottunClick() {
    //ポップアップを閉じる
    var query = "icrop:popupPhoto?close=YES";
    location.href = query;

    return "OK";
}

/**
 * 編集ボタン押下時コールバック関数
 */
function OnEditBottunClick() {
    //カメラ機能起動
    onCamera($("#Hidden_FileName").val(), $("#Hidden_CameraFilePath").val());

    return "OK";
}

/**
 * カメラ機能起動
 */
function onCamera(fileName, path) {

    var posX = 80;
    var posY = 150;
    var cbMethod = "CallBackCustomerPhoto";
    var mode = 0;
    var view = 3;
    var aspect = 1;

    //タブレット基盤のカメラ機能(拡張)を起動
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

    //window.alert("タブレット基盤カメラ機能起動：" + query);
    location.href = query;

}

/**
 * @カメラ機能からのコールバック関数
 */
function CallBackCustomerPhoto(rc) {
        
    // カメラ撮影後
    if (rc == 1) {
        //ポップアップを閉じる
        var query = "icrop:popupPhoto?close=CEND";
        location.href = query;
    //ユーザーキャンセル
    } else if (rc == 0) {
        //何もしない
    //アップロード失敗
    } else if (rc == -1) {
        //何もしない
    }
}

/**
 * READYイベント
 */
$(function () {
    // URLパラメータ取得
    gUrlParamList = getUrlVars();

    // ボタンの表示/非表示とタップイベント設定
    if ("0" == gUrlParamList["Mode"]) {
        $(".PhotoButton").remove();
    } else {
        $(".PhotoButton").click(OnEditBottunClick);
    }
    $(".DoneButton").click(OnCloseBottunClick);
});
