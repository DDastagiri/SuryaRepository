//-------------------------------------------------------------------------
//SC3040801.js
//-------------------------------------------------------------------------
//機能：通知履歴
//補足：
//作成：2012/02/3 KN 河原 【servive_1】
//更新：

// -------------------------------------------------------------
// メイン処理
// -------------------------------------------------------------
//ロードイベント
$(window).load(function () {
    $("div.Datas").fingerScroll();
});
//ロードスクリーン
function LoadingScreen() {
    document.getElementById('LoadButton').click();    
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
    document.getElementById('LinkButton').click();

    return false;
};
//キャンセルボタンボタンが押されたときの処理
function CancelBtnClick(id) {
    //$('div.DisabledDiv').show();
    $('div#LoadPanel').show();
    document.getElementById('CancelField').value = id;
    document.getElementById('HideCancelButton').click();
};
//次の6件の表示
function NextBtnClick() {
    $('div.NextButton').hide();
    document.getElementById('HideNextButton').click();
    $('div.NextLoad').show();
    $('div.DisabledDiv').show();    
};                                                            