//------------------------------------------------------------------------------
//SC3180205.js
//------------------------------------------------------------------------------
//機能：承認者選択 javascript
//
//作成：2014/01/21 TMEJ小澤	初版作成
//更新：
//------------------------------------------------------------------------------

// 定数

/* クリックイベント */
var C_CLICK_EVENT = "click";

//変数

/* 選択アカウント */
var gSelectAccount = "";

/* 選択レコード情報 */
var gSelectObject;

/**
* DOMロード直後の処理(重要事項).
* @return {void}
*/
$(function () {
    //タイマー設定
    //window.parent.commonRefreshTimer(function () { __doPostBack("", ""); });

    //クルクル表示
    SetLoadingStart();

    //情報取得
    $("#MainAreaReload").click();

    // UpdatePanel処理前後イベント
    $(document).ready(function () {
        var prm = Sys.WebForms.PageRequestManager.getInstance();

        // 開始時のイベント
        prm.add_beginRequest(function () {
        });
        // 終了時のイベント
        prm.add_endRequest(EndRequest);
        function EndRequest(sender, args) {
            //タイマー初期化
            //window.parent.commonClearTimer();

            //クルクル非表示
            SetLoadingEnd();

            //キャンセルボタンのイベント設定
            $("#CancelButtonDiv").bind(C_CLICK_EVENT, function (event) {
                //ポップアップを閉じる
                window.parent.OpenPopupClose();

            });

            //登録ボタンのイベント設定
            $("#RegisterButtonDiv").bind(C_CLICK_EVENT, function (event) {
                //選択アカウントチェック
                if (gSelectAccount != "") {
                    //存在する場合
                    //ポップアップを閉じる
                    window.parent.OpenPopupClose();
                    //アカウントを親に渡す
                    window.parent.SelectAccount(gSelectAccount);

                }

            });

            //レコードタップのイベント設定
            $(".AccountRecord").bind(C_CLICK_EVENT, function (event) {
                //データ取得
                var selectData = $(this).attr("name").split(",");

                //ログイン状態チェック
                if (selectData[1] != "4") {
                    //ログイン中の場合
                    //選択中のアカウントチェック
                    if (gSelectAccount == "") {
                        //未選択の場合
                        //チェックをする
                        $($(this).children("div")[2]).addClass("PopOverCtConfirmListChecked");
                        //登録ボタン活性
                        $("#RegisterButtonDiv").attr("class", "CTConfirmPopTitleBlockButtonRightOn");
                        //アカウントとオブジェクトを保持
                        gSelectAccount = selectData[0];
                        gSelectObject = $($(this).children("div")[2]);

                    } else if (gSelectAccount == selectData[0]) {
                        //同じものを選択した場合
                        //チェックを外す
                        $($(this).children("div")[2]).removeClass("PopOverCtConfirmListChecked");
                        //登録ボタンを非活性にする
                        $("#RegisterButtonDiv").attr("class", "CTConfirmPopTitleBlockButtonRightOff");
                        //保持情報を初期化
                        gSelectAccount = "";
                        gSelectObject = null;

                    } else {
                        //違うものを選択した場合
                        //選択中のチェックを外す
                        $(gSelectObject).removeClass("PopOverCtConfirmListChecked");
                        //選択したレコードをチェックする
                        $($(this).children("div")[2]).addClass("PopOverCtConfirmListChecked");
                        //アカウントとオブジェクトを保持
                        gSelectAccount = selectData[0];
                        gSelectObject = $($(this).children("div")[2]);

                    }

                }

            });

            //スクロール設定
            $(".CTConfirmPopList").fingerScroll();

            //三点文字の設定
            $(".Ellipsis").CustomLabel({ useEllipsis: true });

        }
    });

});

/**
* 全体読み込み中画面を表示する.
* @return {}
*/
function SetLoadingStart() {
    $("#ServerProcessOverlayBlack").css("display", "block");
    $("#ServerProcessIcon").css("display", "block");
}

/**
* 全体読み込み中画面を非表示にする.
* @return {}
*/
function SetLoadingEnd() {
    $("#ServerProcessOverlayBlack").css("display", "none");
    $("#ServerProcessIcon").css("display", "none");
}