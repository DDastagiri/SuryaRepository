//------------------------------------------------------------------------------
//SC3190402.js
//------------------------------------------------------------------------------
//機能：部品庫モニター_javascript
//
//作成：2014/03/XX NEC村瀬	初版作成
//更新：2014/09/09 TMEJ部品庫B／O管理に向けた評価用アプリ作成 $01
//更新：2017/07/10 NSK H.Kawatani REQ-SVT-TMT-20160825-001 部品監視画面に通知音を追加 $02
//------------------------------------------------------------------------------

// 通知リフレッシュ処理
function MainRefresh() {

    //$01 部品庫B／O管理に向けた評価用アプリ作成 START
    // 読み込み中の場合はリフレッシュ処理を行わない
    if (0 < $('#SC3190402_LoadingScreen:visible').length) {
        return "TRUE";
    }
    //$01 部品庫B／O管理に向けた評価用アプリ作成 END

    //$02 REQ-SVT-TMT-20160825-001 部品監視画面に通知音を追加 START

    // 各エリアの初期状態の件数を取得し、数値型に変換
    var area01 = parseInt($('#pnlArea01 #lblAreaCount01').html());
    var area03 = parseInt($('#pnlArea03 #lblAreaCount03').html());
    
    // 取得した各エリアの件数をJSON形式に変換し、Session Storageに保存
    sessionStorage.setItem("SC3190402_Area01", JSON.stringify(area01));
    sessionStorage.setItem("SC3190402_Area03", JSON.stringify(area03));

    //$02 REQ-SVT-TMT-20160825-001 部品監視画面に通知音を追加 END

    // 顧客承認時予約登録よりも先に画面が再描画されないように、
    // イベントを1.0秒(1000ミリ秒)遅らせる
    setTimeout(function () {
        //リフレッシュ処理用隠しボタンクリックイベントを実行
        $("#hdnBtnRefreshPage").click();
    }, 1000);

    return "TRUE";
};

//$02 REQ-SVT-TMT-20160825-001 部品監視画面に通知音を追加 START

// チップ数増加時に通知音を再生する処理
function beep() {

    // 見積もり待ち情報が初期表示で無い場合、通知音を再生するかの判定処理
    if (sessionStorage.getItem("SC3190402_Area01") != null) {

        // 1つ前の状態の件数をSession Storageから取得
        var beforeArea01 = JSON.parse(sessionStorage.getItem("SC3190402_Area01"));
        
        // 現在の件数を取得し、数値型に変換
        var afterArea01 = parseInt($('#pnlArea01 #lblAreaCount01').html());

        // 件数が増加している場合、通知音を再生
        if (beforeArea01 < afterArea01) {
            icrop.clientapplication.Execute('icrop:soundon:2');
            return;
        }
    }

    // 出庫待ち情報が初期表示で無い場合、通知音を再生するかの判定処理
    if (sessionStorage.getItem("SC3190402_Area03") != null) {

        // 1つ前の状態の件数をSession Storageから取得
        var beforeArea03 = JSON.parse(sessionStorage.getItem("SC3190402_Area03"));

        // 現在の件数を取得し、数値型に変換
        var afterArea03 = parseInt($('#pnlArea03 #lblAreaCount03').html());

        // 件数が増加している場合、通知音を再生
        if (beforeArea03 < afterArea03) {
            icrop.clientapplication.Execute('icrop:soundon:2');
            return;
        }
    }

}
//$02 REQ-SVT-TMT-20160825-001 部品監視画面に通知音を追加 END

//$01 部品庫B／O管理に向けた評価用アプリ作成 START
// ==============================================================
// DOMロード時処理
// ==============================================================
$(function () {

    //$02 REQ-SVT-TMT-20160825-001 部品監視画面に通知音を追加 START
    try {
        //通知音の再生判定
        beep();
    }
    catch (e) {
        //例外は無視する
    }
    finally {
        //session storageに保存した件数を削除する
        sessionStorage.removeItem("SC3190402_Area01");
        sessionStorage.removeItem("SC3190402_Area03");
    }
    //$02 REQ-SVT-TMT-20160825-001 部品監視画面に通知音を追加 END

    // タイトル押下
    $('.divMainHeaderTitleLeft').click(function () {

        // 読み込み中ウィンドウ表示
        $('#SC3190402_LoadingScreen').show();

        $('#hdnBtnMovePage').click();
    });
});
//$01 部品庫B／O管理に向けた評価用アプリ作成 END