//-------------------------------------------------------------------------
//SC3040801.js
//-------------------------------------------------------------------------
//機能：通知履歴
//補足：
//作成：2012/02/3 KN 河原 【servive_1】
//更新：2012/05/25 KN 長谷 【SERVICE_1】対応ステータスの追加
//更新：2012/11/13 TMEJ 河原 クルクルリトライ対応
//更新：2014/04/08 TMEJ 小澤 BTS-370対応
//更新：
// -------------------------------------------------------------
// メイン処理
// -------------------------------------------------------------

var index = -1;

//ロードイベント
$(window).load(function () {
    $("div.Datas").fingerScroll();
    window.parent.commonClearTimer();
});
//ロードスクリーン
function LoadingScreen() {

    //2012/11/13 TMEJ 河原 クルクルリトライ対応 START
    window.parent.commonRefreshTimer(ReLoadfunction);
    //2012/11/13 TMEJ 河原 クルクルリトライ対応 END 

    document.getElementById('LoadButton').click();

    //2014/04/08 TMEJ 小澤 BTS-370対応 START
    //    //2012/11/13 TMEJ 河原 クルクルリトライ対応 START
    //    RequestCheck();
    //    //2012/11/13 TMEJ 河原 クルクルリトライ対応 END
    //2014/04/08 TMEJ 小澤 BTS-370対応 END
};
//セールスリンククリック
function SalesLinkClick(event,id) {

    var ConstWord = "No";

    var selfDiv;
    var parent;
    var listId;
    var idIndex;
    //Disable制御
    //$('div.DisabledDiv').show();
    $('div#LoadPanel').show();
    selfDiv = event.target;
    parent = $(selfDiv).parents('li');
    listId = parent[0].getAttribute("name");
    idIndex = listId.replace(ConstWord, "");
    document.getElementById('LinkIdField').value = id;
    document.getElementById('LinkValueField').value = document.getElementById('SessionValue' + idIndex).value;
    $('#this_form').attr('target', '_parent');

    //2012/11/13 TMEJ 河原 クルクルリトライ対応 START
    window.parent.commonRefreshTimer(WindowReLoad);
    //2012/11/13 TMEJ 河原 クルクルリトライ対応 END

    document.getElementById('LinkButton').click();

    return false;
};
//サービスリンククリック
function ServiceLinkClick(event) {

    var ConstWord = "No";

    var selfDiv;
    var pageId;
    var linlId;
    var parent;
    var listId;
    var idIndex;

    //Disable制御
    //$('div.DisabledDiv').show();
    $('div#LoadPanel').show();
    selfDiv = event.target;
    pageId = selfDiv.className;
    linkId = selfDiv.id;
    parent = $(selfDiv).parents('li');
    listId = parent[0].getAttribute("name");
    idIndex = listId.replace(ConstWord, "");
    document.getElementById('PageIdField').value = pageId;
    document.getElementById('LinkIdField').value = linkId;
    document.getElementById('LinkValueField').value = document.getElementById('SessionValue' + idIndex).value;
    $('#this_form').attr('target', '_parent');

    //2012/11/13 TMEJ 河原 クルクルリトライ対応 START
    window.parent.commonRefreshTimer(WindowReLoad);
    //2012/11/13 TMEJ 河原 クルクルリトライ対応 END

    document.getElementById('LinkButton').click();

    return false;
};
//キャンセルボタンボタンが押されたときの処理
function CancelBtnClick(id) {
    //$('div.DisabledDiv').show();
    $('div#LoadPanel').show();
    document.getElementById('CancelField').value = id;

    //2012/11/13 TMEJ 河原 クルクルリトライ対応 START
    window.parent.commonRefreshTimer(WindowReLoad);
    //2012/11/13 TMEJ 河原 クルクルリトライ対応 END

    document.getElementById('HideCancelButton').click();

    //2014/04/08 TMEJ 小澤 BTS-370対応 START
    //    //2012/11/13 TMEJ 河原 クルクルリトライ対応 START
    //    RequestCheck();
    //    //2012/11/13 TMEJ 河原 クルクルリトライ対応 END
    //2014/04/08 TMEJ 小澤 BTS-370対応 END
};
//次の6件の表示
function NextBtnClick() {
    $('div.NextButton').hide();

    //2012/11/13 TMEJ 河原 クルクルリトライ対応 START
    window.parent.commonRefreshTimer(WindowReLoad);
    //2012/11/13 TMEJ 河原 クルクルリトライ対応 END 

    document.getElementById('HideNextButton').click();
    $('div.NextLoad').show();
    $('div.DisabledDiv').show();

    //2014/04/08 TMEJ 小澤 BTS-370対応 START
    //    //2012/11/13 TMEJ 河原 クルクルリトライ対応 START
    //    RequestCheck();
    //    //2012/11/13 TMEJ 河原 クルクルリトライ対応 END
    //2014/04/08 TMEJ 小澤 BTS-370対応 END
};

// 2012/05/25 KN 長谷 【SERVICE_1】対応ステータスの追加 Start
// チェックボックスクリック
function SupportStatusCheckBoxClick(noticeId, idx) {

    //2012/11/13 TMEJ 河原 クルクルリトライ対応 START
    //$('div#LoadPanel').show();
    $('div#LoadPanel').show();
    //2012/11/13 TMEJ 河原 クルクルリトライ対応 END

    index = idx;
    $("#SupportStatusNoticeId").val(noticeId);
    $("#ListIndex").val(idx);

    //2012/11/13 TMEJ 河原 クルクルリトライ対応 START
    //$("#DetailCheckBoxButton").click();
    //クルクルを表示させるため遅くする
    setTimeout('DetailCheckBoxButtonClick();', 200);
    //2012/11/13 TMEJ 河原 クルクルリトライ対応 END
};
//2012/05/25 KN 長谷 【SERVICE_1】対応ステータスの追加 End


//2012/11/13 TMEJ 河原 クルクルリトライ対応 START
// チェックボックスクリックイベント用
function DetailCheckBoxButtonClick() {
    //リトライ対応
    window.parent.commonRefreshTimer(WindowReLoad);
    //イベント
    $("#DetailCheckBoxButton").click();
    //2014/04/08 TMEJ 小澤 BTS-370対応 START
    //    //リクエストチェック
    //    RequestCheck();
    //2014/04/08 TMEJ 小澤 BTS-370対応 END
};

//初期表示用
function ReLoadfunction() {
    //Loading表示
    $('div#LoadPanel').show();
    //DisabledDiv削除
    $('div.DisabledDiv').hide();
    //リロード処理
    document.getElementById('LoadButton').click();
    //リトライクリア
    return false;
};
//次の6件用
function WindowReLoad() {
    //Loading表示
    $('div#LoadPanel').show();
    //DisabledDiv削除
    $('div.DisabledDiv').hide();
    //画面リフレッシュ
    window.location.reload();
    //リトライ終了
    return false;
};
//2014/04/08 TMEJ 小澤 BTS-370対応 START
////リクエストチェック
//function RequestCheck() {
//    $(document).ready(function () {
//        var prm = Sys.WebForms.PageRequestManager.getInstance();

//        // 終了時のイベント
//        prm.add_endRequest(EndRequest);
//        function EndRequest() {
//            window.parent.commonClearTimer();
//        };
//    });
//};
//2014/04/08 TMEJ 小澤 BTS-370対応 END

//2012/11/13 TMEJ 河原 クルクルリトライ対応 END

//2014/04/08 TMEJ 小澤 BTS-370対応 START
/********************************************************************
* タイマーリセット終了の処理
*********************************************************************/
function clearTimer() {
    setTimeout('window.parent.commonClearTimer();', 200);

}
//2014/04/08 TMEJ 小澤 BTS-370対応 END

/********************************************************************
* 通知アイコンの再描画を行う処理
*********************************************************************/
function updateNoticeIcon() {
    setTimeout('window.parent.icropScript.ui.setNotice();', 200);

}