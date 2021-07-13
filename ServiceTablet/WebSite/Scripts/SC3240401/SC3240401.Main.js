//------------------------------------------------------------------------------
//SC3240401.js
//------------------------------------------------------------------------------
//機能：チップ検索_javascript
//作成：2013/07/22 TMEJ 小澤 タブレット版SMB機能開発(工程管理)
//更新：2013/11/29 TMEJ 張 タブレット版SMB チーフテクニシャン機能開発
//更新：2017/07/12 NSK 河谷 REQ-SVT-TMT-20160906-001 チップ検索の検索範囲をPC-SMBと同じにする
//更新：2018/06/12 NSK 可児 TKM Next Gen e-CRB Project Application development Block B-1  Pマーク、L マークなどを表示
//更新：
//------------------------------------------------------------------------------

// 定数

/* クリックイベント */
var C_CLICK_EVENT = "click";

/* SA権限コード */
var C_OPARATION_SA = "9";

// 変数
var gTapEvent = "Init";

/**
* DOMロード直後の処理(重要事項).
* @return {void}
*/
$(function () {

    //リストクルクル表示
    SetListLoadingStart();

    //読み込み中のレコード削除、タイムアウトエラーの再描画時に出てくるのでここで消しておく
    $("#BackPage").attr("style", "display:none;");
    $("#BackPageLoad").attr("style", "display:none;");
    $("#NextPage").attr("style", "display:none;");
    $("#NextPageLoad").attr("style", "display:none;");

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

            //車両エリアタップ時の処理
            if (gTapEvent == "VehicleArea") {
                //RO一覧ポップアップフラグの確認
                if ($("#HiddenOrderListDisplayType").val() == "1") {
                    //RO一覧ポップアップの表示
                    $(".PopUpOrderListClass").attr("style", "");

                    //RO一覧のスクロール設定
                    $(".PopUpOrderListContentsClass").fingerScroll();

                    //RO一覧のイベント設定
                    SetOrderListEvent();

                    //全体クルクル非表示
                    SetLoadingEnd();

                    //リストクルクル非表示
                    SetListLoadingEnd();

                    $("#OrderListOverlayBlack").css("display", "block");

                } else {
                    //全体クルクル非表示
                    SetLoadingEnd();

                    //リストクルクル非表示
                    SetListLoadingEnd();
                }

                //顧客エリアタップ時の処理
            } else if (gTapEvent == "CustomerArea") {
                //新規顧客フラグがたっている場合
                if ($("#HiddenNewCustomerConfirmType").val() == "1") {
                    //新規顧客登録有無の確認
                    if (confirm($("#HiddenNewCustomerConfirmWord").val())) {
                        //新規登録画面に遷移する
                        $("#HiddenNewCustomerConfirmType").val("2");

                        //イベント実行
                        $("#CustomerAreaEventButton").click();

                    } else {
                        //値を戻してクルクルを非表示にする
                        $("#HiddenNewCustomerConfirmType").val("0");

                        //全体クルクル非表示
                        SetLoadingEnd();

                        //リストクルクル非表示
                        SetListLoadingEnd();
                    }
                } else {
                    //全体クルクル非表示
                    SetLoadingEnd();

                    //リストクルクル非表示
                    SetListLoadingEnd();
                }

                //予約エリアタップ時の処理
            } else if (gTapEvent == "ReserveArea") {
                //顧客一覧のスクロール設定
                $(".mainblockContentAreaNCM0201ResultScroll").fingerScroll();

                //三点文字の設定
                $(".Ellipsis").CustomLabel({ useEllipsis: true });

                //全体クルクル非表示
                SetLoadingEnd();

                //リストクルクル非表示
                SetListLoadingEnd();

                //上記以外のイベント時の処理
            } else {

                //初期表示用のイベント設定
                if (gTapEvent == "Init") {
                    SetInitEvent();
                }
                //顧客一覧イベント設定
                SetCustomerListEvent();

                //2018/06/12 NSK 可児 TKM Next Gen e-CRB Project Application development Block B-1  Pマーク、L マークなどを表示 START
                //アイコン設定
                AdjustIconArea();
                //2018/06/12 NSK 可児 TKM Next Gen e-CRB Project Application development Block B-1  Pマーク、L マークなどを表示 END
                //スクロール設定
                $(".mainblockContentAreaNCM0201ResultScroll").fingerScroll();

                //三点文字の設定
                $(".Ellipsis").CustomLabel({ useEllipsis: true });

                //全体クルクル非表示
                SetLoadingEnd();

                //リストクルクル非表示
                SetListLoadingEnd();
            }
        }
    });
});

/**
* 初期表示イベント設定.
* @return {}
*/
function SetInitEvent() {
    // 2013/11/29 TMEJ 張 タブレット版SMB チーフテクニシャン機能開発 START
    //    //SA以外の権限は顧客ボタンの制御を行う
    //    if ($('#HiddenOperationCode').val() != C_OPARATION_SA) {
    //フッター「顧客詳細ボタン」クリック時の動作
    //        $('#MstPG_FootItem_Main_200').bind(C_CLICK_EVENT, function (event) {
    //            //ヘッダーの顧客検索にフォーカスを当てる
    //            $('#MstPG_CustomerSearchTextBox').focus();

    //            //ボタン背景点灯
    //            $('#MstPG_FootItem_Main_200').addClass("icrop-pressed");
    //            setTimeout(function () {
    //                //ボタン背景を戻す
    //                $('#MstPG_FootItem_Main_200').removeClass("icrop-pressed");
    //            }, 500);
    //            event.stopPropagation();
    //        });
    //    }
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
    // 2013/11/29 TMEJ 張 タブレット版SMB チーフテクニシャン機能開発 END

    //RO一覧エリア以外のタップ時のイベント
    $('#OrderListOverlayBlack').bind(C_CLICK_EVENT, function (event) {

        //RO一覧を非表示にする
        $(".PopUpOrderListClass").attr("style", "display:none");
        $("#OrderListOverlayBlack").css("display", "none");

        //RO一覧ポップアップフラグを0に設定
        $("#HiddenOrderListDisplayType").val("0");

        event.stopPropagation();
    });

    //スケジューラーと電話帳のアプリケーション設定
    SetFutterApplication();

}

/**
* RO一覧を閉じる
* @return {}
*/
function CloseOrderList() {
    //ボタン背景点灯
    $("#PopUpOrderListFooterButton").attr("class", "PopUpOrderListFooterButtonOn");
    setTimeout(function (event) {
        //ボタン背景色を戻す
        $("#PopUpOrderListFooterButton").attr("class", "PopUpOrderListFooterButtonOff");

        //ポップアップ画面を閉じる処理
        $('#OrderListOverlayBlack').click();
    }, 300);

    return false;
}

/**
* 顧客一覧イベント設定.
* @return {}
*/
function SetCustomerListEvent() {

    //取得件数が1件以上あれば設定する
    if (0 < $('#HiddenSearchListCount').val()) {
        //車両情報エリアのイベント設定
        $('.VehicleRecordClass').bind(C_CLICK_EVENT, function (event) {
            //車両情報エリア背景点灯
            var selectedRow = $(this);
            selectedRow.addClass("icrop-pressed");

            //全体クルクル表示
            SetLoadingStart();

            //選択した顧客IDと車両IDを保持
            var selecttedInfo = $(this).attr("name").split(",");
            $("#HiddenSelectCustomerId").val(selecttedInfo[0]);
            $("#HiddenSelectVehicleId").val(selecttedInfo[1]);

            //車両情報エリア背景を戻して処理実行
            setTimeout(function (event) {
                //背景色を戻す
                selectedRow.removeClass("icrop-pressed");

                //タイマー設定
                commonRefreshTimer(function () { __doPostBack("", ""); });

                //イベント変数設定
                gTapEvent = "VehicleArea";

                //イベント実行
                $("#VehicleAreaEventButton").click();
            }, 300);

            event.stopPropagation();
        });

        //顧客情報エリアのイベント設定
        $('.CustomerRecordClass').bind(C_CLICK_EVENT, function (event) {
            //顧客情報エリアと電話情報エリア背景点灯
            var selectedRow = $(this);
            var telArea = $(this).next("li");
            selectedRow.addClass("icrop-pressed");
            telArea.addClass("icrop-pressed");

            //全体クルクル表示
            SetLoadingStart();

            //選択した顧客IDと車両IDを保持
            var selecttedInfo = $(this).attr("name").split(",");
            $("#HiddenSelectCustomerId").val(selecttedInfo[0]);
            $("#HiddenSelectVehicleId").val(selecttedInfo[1]);

            // 2013/11/29 TMEJ 張 タブレット版SMB チーフテクニシャン機能開発 START
            // タップしたLIの隣のストール利用IDを取得する
            //2017/07/12 NSK 河谷 REQ-SVT-TMT-20160906-001 チップ検索の検索範囲をPC-SMBと同じにする START
            //var selectedResvInfo = $(this).parent().children().last().children().children().first().attr("name").split(",");
            var selectedResvInfo = eval($(this).parent().children().last().children().children().first().attr("name"));
            //2017/07/12 NSK 河谷 REQ-SVT-TMT-20160906-001 チップ検索の検索範囲をPC-SMBと同じにする END
            $("#HiddenSelectSvcinId").val(selectedResvInfo[2]);
            // 2013/11/29 TMEJ 張 タブレット版SMB チーフテクニシャン機能開発 END

            //顧客情報エリアと電話情報エリア背景を戻して処理実行
            setTimeout(function (event) {
                //背景色を戻す
                selectedRow.removeClass("icrop-pressed");
                telArea.removeClass("icrop-pressed");

                //タイマー設定
                commonRefreshTimer(function () { __doPostBack("", ""); });

                //イベント変数設定
                gTapEvent = "CustomerArea";

                //イベント実行
                $("#CustomerAreaEventButton").click();
            }, 300);

            event.stopPropagation();
        });

        //電話情報エリアのイベント設定
        $('.CustomerTelClass').bind(C_CLICK_EVENT, function (event) {
            //顧客情報エリアと電話情報エリア背景点灯
            var selectedRow = $(this);
            var custArea = $(this).prev("li");
            selectedRow.addClass("icrop-pressed");
            custArea.addClass("icrop-pressed");

            //全体クルクル表示
            SetLoadingStart();

            //選択した顧客IDと車両IDを保持
            var selecttedInfo = $(this).attr("name").split(",");
            $("#HiddenSelectCustomerId").val(selecttedInfo[0]);
            $("#HiddenSelectVehicleId").val(selecttedInfo[1]);

            //顧客情報エリアと電話情報エリア背景を戻して処理実行
            setTimeout(function (event) {
                //背景色を戻す
                selectedRow.removeClass("icrop-pressed");
                custArea.removeClass("icrop-pressed");

                //タイマー設定
                commonRefreshTimer(function () { __doPostBack("", ""); });

                //イベント変数設定
                gTapEvent = "CustomerArea";

                //イベント実行
                $("#CustomerAreaEventButton").click();
            }, 300);

            event.stopPropagation();
        });

        //予約情報エリアのイベント設定
        $('.AppointmentDate').bind(C_CLICK_EVENT, function (event) {
            //ボタン背景点灯
            var selectedRow = $(this);
            selectedRow.addClass("icrop-pressed");

            //全体クルクル表示
            SetLoadingStart();

            //選択した予約のストール利用IDを保持
            //2017/07/12 NSK 河谷 REQ-SVT-TMT-20160906-001 チップ検索の検索範囲をPC-SMBと同じにする START
            //var selecttedInfo = $(this).attr("name").split(",");
            var selecttedInfo = eval($(this).attr("name"));
            //2017/07/12 NSK 河谷 REQ-SVT-TMT-20160906-001 チップ検索の検索範囲をPC-SMBと同じにする END
            $("#HiddenSelectStallUseId").val(selecttedInfo[0]);
            $("#HiddenSelectAddType").val(selecttedInfo[1]);
            //2017/07/12 NSK 河谷 REQ-SVT-TMT-20160906-001 チップ検索の検索範囲をPC-SMBと同じにする START
            $("#HiddenSelectRoNum").val(selecttedInfo[3]);
            $("#HiddenSelectRoSeq").val(selecttedInfo[4]);
            $("#HiddenSelectTempFlag").val(selecttedInfo[5]);
            //2017/07/12 NSK 河谷 REQ-SVT-TMT-20160906-001 チップ検索の検索範囲をPC-SMBと同じにする END

            //ボタン背景を戻して画面遷移する
            setTimeout(function (event) {
                //背景色を戻す
                selectedRow.removeClass("icrop-pressed");

                //タイマー設定
                commonRefreshTimer(function () { __doPostBack("", ""); });

                //イベント変数設定
                gTapEvent = "ReserveArea";

                //イベント実行
                $("#ReserveAreaEventButton").click();
            }, 300);

            event.stopPropagation();
        });

        //車両ヘッダーのイベント設定
        $('.RegisterSort').bind(C_CLICK_EVENT, function (event) {
            //リストクルクル表示
            SetListLoadingStart();

            //タイマー設定
            commonRefreshTimer(function () { __doPostBack("", ""); });

            //イベント変数設定
            gTapEvent = "VehicleSort";

            //ソートイベント実行
            $("#RegisterSortButton").click();
            event.stopPropagation();
        });

        //顧客ヘッダーのイベント設定
        $('.CustomerSort').bind(C_CLICK_EVENT, function (event) {
            //リストクルクル表示
            SetListLoadingStart();

            //タイマー設定
            commonRefreshTimer(function () { __doPostBack("", ""); });

            //イベント変数設定
            gTapEvent = "CustomerSort";

            //ソートイベント実行
            $("#CustomerSortButton").click();
            event.stopPropagation();
        });

        //前の50件のイベント設定
        $('#BackPage').bind(C_CLICK_EVENT, function (event) {
            //読み込み中を表示
            $("#BackPage").css("display", "none");
            $("#BackPageLoad").css("display", "block");

            //クルクル表示
            //SetLoadingStart();

            //タイマー設定
            commonRefreshTimer(function () { __doPostBack("", ""); });

            //イベント変数設定
            gTapEvent = "BackPage";

            //表示
            $("#BackPageButton").click();
            event.stopPropagation();
        });

        //次の50件のイベント設定
        $('#NextPage').bind(C_CLICK_EVENT, function (event) {
            //読み込み中を表示
            $("#NextPage").css("display", "none");
            $("#NextPageLoad").css("display", "block");

            //クルクル表示
            //SetLoadingStart();

            //タイマー設定
            commonRefreshTimer(function () { __doPostBack("", ""); });

            //イベント変数設定
            gTapEvent = "NextPage";

            //表示
            $("#NextPageButton").click();
            event.stopPropagation();
        });

    }
}

/**
* RO一覧イベント設定.
* @return {}
*/
function SetOrderListEvent() {

    //ROレコードタップ時のイベント設定
    $('.OrderListItemClass').bind(C_CLICK_EVENT, function (event) {
        //背景点灯
        var selectedRow = $(this);
        selectedRow.addClass("icrop-pressed");

        //全体クルクル表示
        SetLoadingStart();

        //選択したRO番号を保持
        $("#HiddenSelectOrderNumber").val($(this).attr("name"));
        //2013/11/29 TMEJ 張 タブレット版SMB チーフテクニシャン機能開発 START
        //タップした行数目をhiddenコントロールに設定する
        $("#HiddenDmsJobDtlId").val($(this).attr("dmsJobDtlId"));
        $("#HiddenVisitId").val($(this).attr("visitSeq"));
        //2013/11/29 TMEJ 張 タブレット版SMB チーフテクニシャン機能開発 END

        //ボタン背景を戻して画面遷移する
        setTimeout(function (event) {
            //背景色を戻す
            selectedRow.removeClass("icrop-pressed");

            //タイマー設定
            commonRefreshTimer(function () { __doPostBack("", ""); });

            //イベント変数設定
            gTapEvent = "OrderArea";

            //イベント実行
            $("#OrderAreaEventButton").click();
        }, 300);

        event.stopPropagation();
    });
}

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
* リスト読み込み中画面を表示する.
* @return {}
*/
function SetListLoadingStart() {
    $("#ServerProcessListOverlay").css("display", "block");
    $("#ServerProcessListIcon").css("display", "block");
}

/**
* リスト読み込み中画面を非表示にする.
* @return {}
*/
function SetListLoadingEnd() {
    $("#ServerProcessListOverlay").css("display", "none");
    $("#ServerProcessListIcon").css("display", "none");
}

/**
* フッターボタンのクリックイベント.
* @return {}
*/
function FooterButtonClick(Id) {

    //2013/11/29 TMEJ 張 タブレット版SMB チーフテクニシャン機能開発 START 
    //    //SA権限以外、顧客詳細ボタンの場合は何もしない
    //    if ($('#HiddenOperationCode').val() != C_OPARATION_SA && Id == 200) {
    //        return false;
    //    }

    //顧客詳細ボタン、TCボタンの場合は何もしない
    if ((Id == 700) || (Id == 200)) {
        return false;
    }
    //2013/11/29 TMEJ 張 タブレット版SMB チーフテクニシャン機能開発 END

    //全体クルクル表示
    SetLoadingStart();

    //タイマーセット
    commonRefreshTimer(function () { __doPostBack("", ""); });
    //各イベント処理実行
    switch (Id) {
        //2013/11/29 TMEJ 張 タブレット版SMB チーフテクニシャン機能開発 START  
        //        case 100: 
        //            //メインメニューボタン 
        //            __doPostBack('ctl00$MstPG_FootItem_Main_100', ''); 
        //            break; 
        //        case 800: 
        //            //SMBボタン 
        //            __doPostBack('ctl00$MstPG_FootItem_Main_800', ''); 
        //            break; 
        //        case 200: 
        //            //顧客ボタン 
        //            __doPostBack('ctl00$MstPG_FootItem_Main_200', ''); 
        //            break; 
        //        case 600: 
        //            //R/Oボタン 
        //            __doPostBack('ctl00$MstPG_FootItem_Main_600', ''); 
        //            break; 
        //        case 1100: 
        //            //追加作業ボタン 
        //            __doPostBack('ctl00$MstPG_FootItem_Main_1100', ''); 
        //            break; 
        //        case 1000: 
        //            //完成検査ボタン 
        //            __doPostBack('ctl00$MstPG_FootItem_Main_1000', ''); 
        //            break;  
        case 100:
            //メインメニューボタン
            __doPostBack('ctl00$MstPG_FootItem_Main_100', '');
            break;
        case 1100:
            //SMBボタン
            __doPostBack('ctl00$MstPG_FootItem_Main_1100', '');
            break;
        case 300:
            //FM
            __doPostBack('ctl00$MstPG_FootItem_Main_300', '');
            break;
        case 400:
            //予約ボタン
            __doPostBack('ctl00$MstPG_FootItem_Main_400', '');
            break;
        case 500:
            //R/Oボタン
            __doPostBack('ctl00$MstPG_FootItem_Main_500', '');
            break;
        case 700:
            //顧客ボタン
            __doPostBack('ctl00$MstPG_FootItem_Main_700', '');
            break;
        case 800:
            //商品訴求
            __doPostBack('ctl00$MstPG_FootItem_Main_800', '');
            break;
        case 900:
            //キャンペーンボタン
            __doPostBack('ctl00$MstPG_FootItem_Main_900', '');
            break;
        case 1200:
            //追加作業ボタン
            __doPostBack('ctl00$MstPG_FootItem_Main_1200', '');
            break;
            //2013/11/29 TMEJ 張 タブレット版SMB チーフテクニシャン機能開発 END  
    }
}
//2018/06/12 NSK 可児 TKM Next Gen e-CRB Project Application development Block B-1  Pマーク、L マークなどを表示 START
function AdjustIconArea() {
    //表示アイコンの数
    var resultList = $('ul#chipReserveRow');

    resultList.each(function (index) {
        var list = $('ul#chipReserveRow').eq(index);
        //表示されているアイコンの数
        var count = 0;
        if (list.find('#SSCIcon').css('display') == 'block') {
            count++;
        }
        if (list.find('#MIcon').css('display') == 'block') {
            count++;
        }
        if (list.find('#BIcon').css('display') == 'block') {
            count++;
        }
        if (list.find('#EIcon').css('display') == 'block') {
            count++;
        }
        if (list.find('#TIcon').css('display') == 'block') {
            count++;
        }
        if (list.find('#PIcon').css('display') == 'block') {
            count++;
        }
        //アイコンエリアの表示幅
        var iconAreaSize = count * 18;
        //アイコンエリアの幅を調節
        list.find('.IconArea').width(iconAreaSize);
        //VINの表示幅を調節
        list.find('#Vin').width(240 - iconAreaSize);
    });
}
//2018/06/12 NSK 可児 TKM Next Gen e-CRB Project Application development Block B-1  Pマーク、L マークなどを表示 END