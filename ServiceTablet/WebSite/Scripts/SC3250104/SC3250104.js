//------------------------------------------------------------------------------
//SC3250104.js
//------------------------------------------------------------------------------
//機能：部品説明(新旧コンテンツ表示)画面_javascript
//
//作成：2014/02/XX NEC 上野	初版作成
//更新：
//------------------------------------------------------------------------------

// 写真選択ポップアップ画面表示
function ShowUrlSchemeNoTitlePopup(targetparam) {

    //写真選択画面のURL+パラメータ作成
    var strUrl = window.location.href;
    var target = strUrl.slice(0, strUrl.slice(0, strUrl.slice(0, strUrl.indexOf('?')).lastIndexOf('/')).lastIndexOf('/')) + "/SC3170210.aspx" + targetparam;

    //写真選択のイベント通知コールバック関数に検索ロジックを付けて渡す
    var cbMethod = "var callbackCamera=" +
                        "function(rc){" +
                            "$('iframe').each(function(){" +
                                //"$(this).contents().find('iframe').each(arguments.callee);" +
                                //"if($(this).contents().find('div#SC3250104OldNew').length > 0){" +
                                //    "this.contentWindow.CallBackThumbnailPhoto(rc);" +
                                //"}" +
                                "if($(this).attr('id') == 'iFrame2'){ this.contentWindow.CallBackThumbnailPhoto(rc); };" + 
                            "});" +
                        "};callbackCamera";

    //写真選択の終了通知コールバック関数に検索ロジックを付けて渡す
    var cbCloseMethod = "var callbackCloseCamera=" +
                        "function(){" +
                            "$('iframe').each(function(){" +
                                //"$(this).contents().find('iframe').each(arguments.callee);" +
                                //"if($(this).contents().find('div#SC3250104OldNew').length > 0){" +
                                //    "this.contentWindow.CallBackClose();" +
                                //"}" +
                                "if($(this).attr('id') == 'iFrame2'){this.contentWindow.CallBackClose();};" + 
                            "});" +
                        "};callbackCloseCamera";

    // タイトルバー無しポップアップ表示
    var scheme = "icrop:noTitlePopup?";
    scheme += "url=" + target;
    scheme += "::x=0";
    scheme += "::y=56";
    scheme += "::w=1024";
    scheme += "::h=656";
    scheme += "::eventFunc=" + cbMethod;
    scheme += "::endFunc=" + cbCloseMethod;

    window.location.href = scheme;
}

//写真選択画面からのイベント通知コールバック
function CallBackThumbnailPhoto(rc) {
    //alert(rc);
    document.getElementById("hdnRegisterFile").value = rc;
}

//写真選択画面からの終了通知コールバック
function CallBackClose() {
    //ポストバック
    this_form.submit();

}
