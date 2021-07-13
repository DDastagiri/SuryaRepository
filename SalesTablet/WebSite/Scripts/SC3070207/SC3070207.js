/*━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
SC3070207.js
─────────────────────────────────────
機能： 注文承認
補足： 
作成： 2013/12/09 TCS 山口  Aカード情報相互連携開発
更新： 2014/08/01 TCS 市川　Aカード切替BTS-114 対応
─────────────────────────────────────*/

/**
* 初期処理
*/
$(function () {
    // -------------------------------
    // 非同期ポストバック用処理
    // -------------------------------

    //2014/08/01 TCS 市川　Aカード切替BTS-114 対応 START
    //initializeRequestイベント登録(非同期通信初期化イベント)
    Sys.WebForms.PageRequestManager.getInstance().remove_initializeRequest(initRequest_SC3070207);
    Sys.WebForms.PageRequestManager.getInstance().add_initializeRequest(initRequest_SC3070207);
    //2014/08/01 TCS 市川　Aカード切替BTS-114 対応 END

    /**
    * 承認ボタン押下時
    */
    $('#SC3070207_ApprovalButtonArea').live('click', function (e) {
        //ロードアイコン表示
        dispLoading();

        $('#SC3070207_ApprovalButton').click();

    });

    /**
    * 否認ボタン押下時
    */
    $('#SC3070207_DenialButtonArea').live('click', function (e) {
        //ロードアイコン表示
        dispLoading();

        $('#SC3070207_DenialButton').click();

    });

});

//2014/08/01 TCS 市川　Aカード切替BTS-114 対応 START
//readyメソッド($();)が2度呼びされるため、readyメソッド外へ初期化処理配置
function initRequest_SC3070207(sender, args) {

    //承認・否認の要求発生時のみendRequestイベント登録する
    if (args.get_postBackElement() != undefined &&
        (   args.get_postBackElement().id == "SC3070207_ApprovalButton"
         || args.get_postBackElement().id == "SC3070207_DenialButton")) {

        var endRequest_SC3070207 = function (sender, args) {

            //エラーなしの場合、画面再表示
            if ($("#ErrorFlg").val() == "") {
                this_form.submit();
            } else {
                hideDispLoading();
            }
            //処理が加算され続けるため削除(endRequest)
            Sys.WebForms.PageRequestManager.getInstance().remove_endRequest(endRequest_SC3070207);
        };
        //endRequestイベント登録
        Sys.WebForms.PageRequestManager.getInstance().remove_endRequest(endRequest_SC3070207);
        Sys.WebForms.PageRequestManager.getInstance().add_endRequest(endRequest_SC3070207);

        //処理が加算され続けるため削除(initializeRequest)
        Sys.WebForms.PageRequestManager.getInstance().remove_initializeRequest(initRequest_SC3070207);
    }
}
//2014/08/01 TCS 市川　Aカード切替BTS-114 対応 END