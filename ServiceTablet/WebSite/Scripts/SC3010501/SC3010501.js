//------------------------------------------------------------------------------
//SC3010501.js
//------------------------------------------------------------------------------
//機能：他システム連携画面_javascript
//
//作成：2013/12/16 TMEJ小澤	初版作成
//更新：
//------------------------------------------------------------------------------

// 定数

/* クリックイベント */
var C_CLICK_EVENT = "click";
var gTapEvent = "Init";

/**
* DOMロード直後の処理(重要事項).
* @return {void}
*/
$(function () {
    //タイマー設定
    commonRefreshTimer(function () { __doPostBack("", ""); });

    //クルクル表示
    ActiveDisplayOn();

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
            commonClearTimer();

            //クルクル非表示
            ActiveDisplayOff();

            //IFRAMEのURLを設定
            $("#iFramePage").attr("src", $("#HiddenFieldIFrameURL").val());

            //フッター「顧客詳細ボタン」クリック時の動作
            $('#MstPG_FootItem_Main_700').bind(C_CLICK_EVENT, function (event) {
                //ヘッダーの顧客検索にフォーカスを当てる
                $('#MstPG_CustomerSearchTextBox').focus();

                event.stopPropagation();
            });

            //スケジューラーと電話帳のアプリケーション設定
            SetFutterApplication();

        }
    });

});

/**
* スケジュールボタンと電話帳ボタンの設定する.
* @return {}
*/
function SetFutterApplication() {
    //スケジュールオブジェクトを拡張
    $.extend(window, { schedule: {} });
    //スケジュールオブジェクトに関数追加
    $.extend(schedule, {
        //アプリ起動クラス
        appExecute: {
            //カレンダーアプリ起動(単体)
            executeCaleNew: function () {
                window.location = "icrop:cale:";
                return false;
            },
            //電話帳アプリ起動(単体)
            executeCont: function () {
                window.location = "icrop:cont:";
                return false;
            }
        }
    });
}


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

/**
* フッターボタンのクリックイベント.
* @return {}
*/
function FooterButtonClick(Id) {
    //顧客ボタン、TCメインボタンの場合は何もしない
    if (Id == 700 || Id == 200) {
        return false;
    }

    //全体クルクル表示
    ActiveDisplayOn();

    //タイマーセット
    commonRefreshTimer(function () { __doPostBack("", ""); });
    //各イベント処理実行
    switch (Id) {
        case 100:
            //メインメニュー
            __doPostBack('ctl00$MstPG_FootItem_Main_100', '');
            break;
        case 300:
            //FMメインメニュー
            __doPostBack('ctl00$MstPG_FootItem_Main_300', '');
            break;
        case 400:
            //予約管理
            __doPostBack('ctl00$MstPG_FootItem_Main_400', '');
            break;
        case 500:
            //R/O
            __doPostBack('ctl00$MstPG_FootItem_Main_500', '');
            break;
        case 600:
            //連絡先
            __doPostBack('ctl00$MstPG_FootItem_Main_600', '');
            break;
        case 700:
            //顧客
            __doPostBack('ctl00$MstPG_FootItem_Main_700', '');
            break;
        case 800:
            //商品訴求コンテンツ
            __doPostBack('ctl00$MstPG_FootItem_Main_800', '');
            break;
        case 900:
            //キャンペーン
            __doPostBack('ctl00$MstPG_FootItem_Main_900', '');
            break;
        case 1000:
            //全体管理
            __doPostBack('ctl00$MstPG_FootItem_Main_1000', '');
            break;
        case 1100:
            //SMB
            __doPostBack('ctl00$MstPG_FootItem_Main_1100', '');
            break;
        case 1200:
            //追加作業
            __doPostBack('ctl00$MstPG_FootItem_Main_1200', '');
            break;
    }
}

/**
* 承認者選択ポップアップの受口.
* @return {}
*/
function SelectAccount(account) {
    //iframeに「@」以降を削除したアカウントを返す
    document.getElementById("iFramePage").contentWindow.SetSelectAccount(account.substr(0, account.indexOf("@")));

}
