//------------------------------------------------------------------------------
//SC3230101.js
//------------------------------------------------------------------------------
//機能：メインメニュー(FM)画面_javascript
//
//作成：2014/02/XX NEC 桜井	初版作成
//更新：
//------------------------------------------------------------------------------

$(function () {

    //チップ領域のスクロール機能設定
    $("#AddJobApprChips").fingerScroll();
    $("#InsRltApprChips").fingerScroll();

    //フッターアプリの起動設定
    SetFooterApplication();

});

/**
* フッター部のアプリ
* @return {void}
*/
function SetFooterApplication() {

    //スケジュールオブジェクトを拡張
    $.extend(window, { schedule: {} });

    //スケジュールオブジェクトに関数追加
    $.extend(schedule, {

        /**
        * @class アプリ起動クラス
        */
        appExecute: {

            /**
            * カレンダーアプリ起動(単体)
            */
            executeCaleNew: function () {
                window.location = "icrop:cale:";
                return false;
            },
            /**
            * 電話帳アプリ起動(単体)
            */
            executeCont: function () {
                window.location = "icrop:cont:";
                return false;
            }
        }

    });
}

/**
* フッターボタンのクリックイベント.
* @return {}
*/
function FooterButtonClick(Id) {

    //全体クルクル表示
    ActiveDisplayOn();

//    //タイマーセット
//    commonRefreshTimer(function () { __doPostBack("", ""); });

    //各イベント処理実行
    switch (Id) {
        case 100:
            //メインメニュー
            __doPostBack('ctl00$MstPG_FootItem_Main_100', '');
            break;
        case 200:
            //TCメインメニュー
            __doPostBack('ctl00$MstPG_FootItem_Main_200', '');
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

// 通知リフレッシュ処理
function MainRefresh() {

    //全体クルクル表示
    ActiveDisplayOn();

    //リフレッシュ処理用隠しボタンのクリックイベントを実行
    __doPostBack('ctl00$content$hdnBtnRefreshPage', '');

    return "TRUE";
};
// 通知リフレッシュ処理
function refreshInspectionResultArea() {

    //全体クルクル表示
    ActiveDisplayOn();

    //リフレッシュ処理用隠しボタンのクリックイベントを実行
    __doPostBack('ctl00$content$hdnBtnRefreshPage', '');

    return "TRUE";
};
// 通知リフレッシュ処理
function refreshAdditionalJobArea() {

    //全体クルクル表示
    ActiveDisplayOn();

    //リフレッシュ処理用隠しボタンのクリックイベントを実行
    __doPostBack('ctl00$content$hdnBtnRefreshPage', '');

    return "TRUE";
};

function FncRedirectNextScreen(url) {

    //全体クルクル表示
    ActiveDisplayOn();

    //パラメータのUrlを隠し項目にセット
    $("#hdnUrl").val(url);

    //画面遷移処理用隠しボタンのクリックイベントを実行
    $("#hdnBtnNextPage").click();

};