//SC3150201.js
//------------------------------------------------------------------------------
//機能：TCステータスモニター_javascript
//作成：2013/02/21 TMEJ 成澤
//更新：2016/03/30 NSK  小牟禮 アクティビティインジケータが消えない問題
//------------------------------------------------------------------------------


//スクリーンセイバー画面遷移タイマー
var refreshTimer;

//スクリーンセイバー画面遷移秒数
var secondNum;

var timerClearTime = 0;


//2016/03/30 NSK 小牟禮 アクティビティインジケータが消えない問題 START
//画面更新中フラグ
//true:更新中 / false:更新中でない
//※初期表示終了後、必ずfalseが設定される
var gUpdatingDisplayFlg;
//2016/03/30 NSK 小牟禮 アクティビティインジケータが消えない問題 END

//画面ロード時のイベント
$(function () {

    //リフレッシュタイマースタート
    refreshTimerStart();

    //画面タッチイベント、マウスクリックイベント
    $(".screensaver").bind('mouseup touchstart', function (e) {
        mainMenurLoad();
    });

    //2016/03/30 NSK 小牟禮 アクティビティインジケータが消えない問題 START
    ////クルクルタイムアウト処理
    //reloadPageIfNoResponse();
    //クルクル表示
    LoadingScreenNoIcon();
    //2016/03/30 NSK 小牟禮 アクティビティインジケータが消えない問題 END
});

$(window).load(function () {
    //画面の読み込み完了後クルクル非表示
    setTimeout(function () { LoadProcessHide() }, 100);
});


//ページ更新メソッド
function refresh() {

    //2016/03/30 NSK 小牟禮 アクティビティインジケータが消えない問題 START
    ////クルクル表示
    //LoadProcess();
    ////隠しボタンをクリック
    //$('#HiddenButtonRefreshtSC3150201').click();
    ////クルクルタイムアウト処理
    //reloadPageIfNoResponse();

    //画面更新中の場合は処理を行わない
    if (gUpdatingDisplayFlg) {
        return;
    }
    
    //クルクル表示
    LoadProcess();

    //隠しボタンをクリック
    setTimeout(function () {
        $('#HiddenButtonRefreshtSC3150201').click();
    }, 0);
    //2016/03/30 NSK 小牟禮 アクティビティインジケータが消えない問題 END
}

//スクリーンセイバー画面遷移メソッド
function mainMenurLoad() {
    //クルクル表示
    LoadProcess();
    //画面遷移
    setTimeout(function () { $('#HiddenButtonRedirectSC3150101').click() }, 100);

    //2016/03/30 NSK 小牟禮 アクティビティインジケータが消えない問題 START
    ////クルクルタイムアウト処理
    //reloadPageIfNoResponse();
    //2016/03/30 NSK 小牟禮 アクティビティインジケータが消えない問題 END
}

//スクリーンセイバータイマースタートメソッド
function refreshTimerStart() {
    //DBから取得した秒数を格納
    secondNum = document.getElementById("HiddenRefreshTime").value;
    //タイマースタート
    refreshTimer = setInterval("refresh()", secondNum * 1000);
}

//クルクル非表示メソッド
function LoadProcessHide() {
    var ele = document.getElementById("LoadingScreen");
    ele.style.display = "none";
    //再表示タイマーをリセット
    commonClearTimer();

    //2016/03/30 NSK 小牟禮 アクティビティインジケータが消えない問題 START
    //画面更新中フラグをfalse(更新中でない)に設定
    gUpdatingDisplayFlg = false;
    //2016/03/30 NSK 小牟禮 アクティビティインジケータが消えない問題 END
}

//クルクル表示メソッド
function LoadProcess() {
    var ele = document.getElementById("LoadingScreen");
    ele.style.display = "table";

    //2016/03/30 NSK 小牟禮 アクティビティインジケータが消えない問題 START
    //クルクルタイムアウト処理
    reloadPageIfNoResponse();

    //画面更新中フラグをtrue(更新中)に設定
    gUpdatingDisplayFlg = true;
    //2016/03/30 NSK 小牟禮 アクティビティインジケータが消えない問題 END
}

//2016/03/30 NSK 小牟禮 アクティビティインジケータが消えない問題 START
//読み込みを行うが、くるくるアイコンを表示しない場合、こちらの関数を利用する
function LoadingScreenNoIcon() {
    //クルクルタイムアウト処理
    reloadPageIfNoResponse();
    //画面更新中フラグをtrue(更新中)に設定
    gUpdatingDisplayFlg = true;
}
//2016/03/30 NSK 小牟禮 アクティビティインジケータが消えない問題 END

//画面全体のリロード処理
function reloadPage() {
    $("#HiddenButtonRefresh").click();
    return true;
}

//一時的なNetwork障害などによって発生したクルクル現象の対応としての画面全体リロード処理のタイマー設定
function reloadPageIfNoResponse() {
    timerClearTime = (new Date().getTime()) - 1;
    commonRefreshTimer(reloadPage);
}

/**
* 再表示タイマーをセットする.
* 
* @param {refreshFunc} 再表示用のJavaScrep関数 -
* @return {-} -
* 
* @example 
*  -
*/
function commonRefreshTimer(refreshFunc) {

    //タイマー間隔の取得
    var refreshTime = Number($("#MstPG_RefreshTimerTime").val());

    //開始時間を保持する
    var startTime = new Date().getTime();

    setTimeout(function () {

        //clearTimer()がされている場合は処理しない
        if (startTime <= timerClearTime) {
            return;
        }

        //出力メッセージを選択する
        var messageString = $("#MstPG_RefreshTimerMessage1").val();

        //警告メッセージ出力
        alert(messageString);

        //各画面でリフレッシュ処理をする
        if (refreshFunc() === false) {
            //falseが帰ってきたら再読み込み処理をしない
            return;
        }

        //再度、タイマーをセットする
        commonRefreshTimer(refreshFunc);

    }, refreshTime);
}

//再表示タイマーをリセットする.
function commonClearTimer() {
    //現在時、以前のタイマーを無視する
    timerClearTime = new Date().getTime();
}
