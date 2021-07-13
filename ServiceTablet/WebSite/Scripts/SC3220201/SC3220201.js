//------------------------------------------------------------------------------
//SC32202101.js
//------------------------------------------------------------------------------
//機能：全体管理_javascript
//作成：2012/02/28 TMEJ 小澤
//更新：2013/06/03 TMEJ 河原 IT9513_【A.STEP2】次世代e-CRBタブレット　新DB適応に向けた機能設計
//更新：2013/12/10 TMEJ 河原 TMEJ次世代サービス 工程管理機能開発
//更新：2014/12/10 TMEJ 三輪 DMS連携版サービスタブレット 納車予定時刻変更通知機能開発
//更新：2017/10/13 NSK 竹中(悠) REQ-SVT-TMT-20170227-002 見込納車遅れ日時計算ロジック統一
//------------------------------------------------------------------------------

// 定数

/* 行数 */
var C_ROW_COUNT = 7;
/* 赤枠 */
var C_BORDER_RED = "ColumnBoxBorderRed";
/* 黄枠 */
var C_BORDER_YELLOW = "ColumnBoxBorderYellow";
/* タッチイベント */
var C_TOUCH_EVENT = "mousedown touchstart";
/* クリックイベント */
var C_CLICK_EVENT = "click";
/**
* DOMロード直後の処理(重要事項).
* @return {void}
*/
$(function () {

    //クルクル表示
    SetLoadingStart();

    //タイマー設定
    commonRefreshTimer(function () { __doPostBack("", ""); });

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

            //スクロール設定
            $("#ReserveArea").fingerScroll();
            $("#ReceptionistArea").fingerScroll();
            $("#WorkArea").fingerScroll();
            $("#WashArea").fingerScroll();
            $("#DeliveryArea").fingerScroll();

            //三点文字の設定
            $(".Ellipsis").CustomLabel({ useEllipsis: true });

            //イベント設定
            SetEvent();

            //アプリケーションの設定を行う
            SetFutterApplication();

            //チップが1件以上ある場合は1分ごとにアイコンの色を確認・変更
            var chipCount = $("#ReserveArea").find("li").length +
                            $("#ReceptionistArea").find("li").length +
                            $("#WorkArea").find("li").length +
                            $("#WashArea").find("li").length +
                            $("#DeliveryArea").find("li").length;
            if (chipCount > 0) {
                setInterval("CheckChipColor()", (60 * 1000));
            }

            //クルクル非表示
            SetLoadingEnd();
        }
    });
});


/**
* イベント設定.
* @return {}
*/
function SetEvent() {

    //2013/12/10 TMEJ 河原 TMEJ次世代サービス 工程管理機能開発 START

//    //フッター「顧客詳細ボタン」クリック時の動作
//    $('#MstPG_FootItem_Main_200').bind(C_CLICK_EVENT, function (event) {
//        //ヘッダーの顧客検索にフォーカスを当てる
//        $('#MstPG_CustomerSearchTextBox').focus();
//        //ボタン背景点灯
//        $('#MstPG_FootItem_Main_200').addClass("icrop-pressed");
//        setTimeout(function () {
//            //ボタン背景を戻す
//            $('#MstPG_FootItem_Main_200').removeClass("icrop-pressed");
//        }, 500);
//        event.stopPropagation();
//    });

//    //フッター「来店管理」クリック時の動作
//    $('.InnerBox01').bind(C_TOUCH_EVENT, function (event) {
//        //ボタン背景点灯
//        $('.InnerBox01').addClass("icrop-pressed");
//        //クルクル表示
//        SetLoadingStart();
//        //チップ非表示
//        $(".chipsArea").attr("style", "display:none;");

//        setTimeout(function () {
//            //ボタン背景を戻す
//            $('.InnerBox01').removeClass("icrop-pressed");
//            //タイマーセット
//            commonRefreshTimer(function () { __doPostBack("", ""); });
//            //画面遷移イベント実行
//            $("#VisitManagementFooterButton").click();
//        }, 500);
//        event.stopPropagation();
//    });

//    //フッター「全体管理」クリック時の動作
//    $('.InnerBox02').bind(C_TOUCH_EVENT, function (event) {
//        //クルクル表示
//        SetLoadingStart();
//        //チップ非表示
//        $(".chipsArea").attr("style", "display:none;");

//        setTimeout(function () {
//            //タイマーセット
//            commonRefreshTimer(function () { __doPostBack("", ""); });
//            //再描画イベント実行
//            __doPostBack("", "");
//        }, 500);
//        event.stopPropagation();
//    });

    //フッター「SA」クリック時の動作
    $('.CustomFooterButtonSA').bind(C_TOUCH_EVENT, function (event) {

        //クルクル表示
        SetLoadingStart();
        //チップ非表示
        $(".chipsArea").attr("style", "display:none;");

        setTimeout(function () {
            //ボタン背景を戻す
            $('.InnerBox01').removeClass("icrop-pressed");
            //タイマーセット
            commonRefreshTimer(function () { __doPostBack("", ""); });
            //画面遷移イベント実行
            $("#FooterButtonSADummy").click();
        }, 500);
        event.stopPropagation();
    });

    //フッター「顧客詳細ボタン」クリック時の動作
    $('#MstPG_FootItem_Main_700').bind(C_CLICK_EVENT, function (event) {
        //ヘッダーの顧客検索にフォーカスを当てる
        $('#MstPG_CustomerSearchTextBox').focus();
        //ボタン背景点灯
        $('#MstPG_FootItem_Main_700').addClass("icrop-pressed");
        setTimeout(function () {
            //ボタン背景を戻す
            $('#MstPG_FootItem_Main_700').removeClass("icrop-pressed");
        }, 500);
        event.stopPropagation();
    });

    //2013/12/10 TMEJ 河原 TMEJ次世代サービス 工程管理機能開発 END
}


/**
* フッターボタンのクリックイベント.
* @return {}
*/
function FooterButtonClick(Id) {

    //2013/12/10 TMEJ 河原 TMEJ次世代サービス 工程管理機能開発 START

//    //顧客詳細ボタンの場合は何もしない
//    if (Id == 200) {
//        return false;
    //    }

    //顧客詳細ボタンの場合は何もしない
    if (Id == 700) {
        return false;
    }

    //2013/12/10 TMEJ 河原 TMEJ次世代サービス 工程管理機能開発 END

    //クルクル表示
    SetLoadingStart();
    //チップ非表示
    $("#ReserveArea").attr("style", "display:none;");
    $("#ReceptionistArea").attr("style", "display:none;");
    $("#WorkArea").attr("style", "display:none;");
    $("#WashArea").attr("style", "display:none;");
    $("#DeliveryArea").attr("style", "display:none;");

    //タイマーセット
    commonRefreshTimer(function () { __doPostBack("", ""); });

    //2013/12/10 TMEJ 河原 TMEJ次世代サービス 工程管理機能開発 START

//    //各イベント処理実行
//    switch (Id) {
//        case 100:
//            //メインメニューボタン
//            __doPostBack('ctl00$MstPG_FootItem_Main_100', '');
//            break;
//        case 600:
//            //R/Oボタン
//            __doPostBack('ctl00$MstPG_FootItem_Main_600', '');
//            break;
//        case 1100:
//            //追加作業ボタン
//            __doPostBack('ctl00$MstPG_FootItem_Main_1100', '');
//            break;
//        //2013/06/03 TMEJ 河原 IT9513_【A.STEP2】次世代e-CRBタブレット　新DB適応に向けた機能設計 START   
//        case 800:
//            __doPostBack('ctl00$MstPG_FootItem_Main_800', '');
//            break;
//        //2013/06/03 TMEJ 河原 IT9513_【A.STEP2】次世代e-CRBタブレット　新DB適応に向けた機能設計 END   
//    };

    switch (Id) {
        case 100:
            __doPostBack('ctl00$MstPG_FootItem_Main_100', '');
            break;
        case 500:
            __doPostBack('ctl00$MstPG_FootItem_Main_500', '');
            break;
        case 800:
            __doPostBack('ctl00$MstPG_FootItem_Main_800', '');
            break;
        case 900:
            __doPostBack('ctl00$MstPG_FootItem_Main_900', '');
            break;
        case 400:
            __doPostBack('ctl00$MstPG_FootItem_Main_400', '');
            break;
        case 1000:
            __doPostBack('ctl00$MstPG_FootItem_Main_1000', '');
            break;
        case 1100:
            __doPostBack('ctl00$MstPG_FootItem_Main_1100', '');
            break;

    };

    //2013/12/10 TMEJ 河原 TMEJ次世代サービス 工程管理機能開発 END

};

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
* 読み込み中画面を表示する.
* @return {}
*/
function SetLoadingStart() {
    $("#ServerProcessOverlayBlack").css("display", "block");
    $("#ServerProcessIcon").css("display", "block");
}

/**
* 読み込み中画面を非表示にする.
* @return {}
*/
function SetLoadingEnd() {
    $("#ServerProcessOverlayBlack").css("display", "none");
    $("#ServerProcessIcon").css("display", "none");
}

/**
* 遅れチップへの更新.
* @return {}
*/
function CheckChipColor() {
    //現在日付取得
    var nowDate = new Date(new Date().getTime());
    //チップの赤枠一覧を取得する
    var chipList = $("[id=ChipBorder]")
    for (i = 0; i < chipList.length; i++) {
        //基準日時、遅見込日時、設定されているクラス名を取得
        var chipDate = new Date(chipList[i].getAttribute("chipDate"));
        var delayDate = new Date(chipList[i].getAttribute("delayDate"));
        var chipClassName = $(chipList[i]).attr("class");

        //2017/10/13 NSK 竹中(悠) REQ-SVT-TMT-20170227-002 見込納車遅れ日時計算ロジック統一 START
        //日付の最小値
        var minValue = new Date("0001/01/01 00:00"); ;
        //基準日時が日付の最小値の場合
        if ((chipDate - minValue) == 0) {
            //枠の色を設定しない（遅れ管理しない）
            continue;
        }
        //2017/10/13 NSK 竹中(悠) REQ-SVT-TMT-20170227-002 見込納車遅れ日時計算ロジック統一 START

        //クラスが設定されいる場合、且つ基準日時が現在日時を超えていた場合は赤枠を設定する
        if (chipDate < nowDate) {
            $(chipList[i]).attr("class", C_BORDER_RED);

            //クラスが設定されいる場合、且つ遅見込日時が現在日時を超えていた場合は黄枠を設定する
        } else if (chipClassName == "" && delayDate < nowDate) {
            $(chipList[i]).attr("class", C_BORDER_YELLOW);
        }
    }
}

//2014/12/10 TMEJ 三輪 DMS連携版サービスタブレット 納車予定時刻変更通知機能開発 START
//Push用リフレッシュ関数
function RefreshSM() {
    //クルクル表示
    SetLoadingStart();

    //タイマー設定
    commonRefreshTimer(function () { __doPostBack("", ""); });

    //情報取得
    $("#MainAreaReload").click();
}
//2014/12/10 TMEJ 三輪 DMS連携版サービスタブレット 納車予定時刻変更通知機能開発 END

