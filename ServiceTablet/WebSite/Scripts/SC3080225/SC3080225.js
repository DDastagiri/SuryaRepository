//SC3080225.js
//------------------------------------------------------------------------------
//機能：顧客詳細（参照）_javascript
//作成：2014/02/14 TMEJ 小澤 IT9611_次世代サービス 工程管理機能開発
//更新：2014/09/22 SKFC 佐藤 e-Mail,Line送信機能
//更新：2016/06/30 NSK 皆川 TR-SVT-TMT-20150922-001 画面で画像プロフィールが表示しない
//更新：2018/02/27 NSK 山田 17PRJ01947-00_(トライ店システム評価)サービス重要顧客における、識別及び対処オペレーションの検証
//更新：2018/07/04 NSK 可児 TKM Next Gen e-CRB Project Application development Block B-1  Pマーク、L マークなどを表示
//更新：2020/02/28 NSK 坂本 (トライ店システム評価)大量車両保有顧客における、レスポンス向上検証
//------------------------------------------------------------------------------

/**
* 変数
*/
var gVehicleSelectRecordNumber = 0;
var gVehicleSelectRecordInfo = null;
var gEventKey = "";

//2018/02/27 NSK 山田 17PRJ01947-00_(トライ店システム評価)サービス重要顧客における、識別及び対処オペレーションの検証 START
//定数
var C_SSC_ON = "1";    //SSCフラグON
//2018/02/27 NSK 山田 17PRJ01947-00_(トライ店システム評価)サービス重要顧客における、識別及び対処オペレーションの検証 END

//2018/07/04 NSK 可児 TKM Next Gen e-CRB Project Application development Block B-1  Pマーク、L マークなどを表示 START
var C_ICON_ON_1 = "1"; //M/E/T/PフラグON
var C_ICON_ON_2 = "2"; //B/LフラグON
//2018/07/04 NSK 可児 TKM Next Gen e-CRB Project Application development Block B-1  Pマーク、L マークなどを表示 END

/********************************************************************
* 画面ロード時のイベント
*********************************************************************/
$(function () {

    //クルクル表示
    LoadProcess();

    //クルクルタイムアウト処理
    commonRefreshTimer(function () { __doPostBack("", ""); });

    //イベント設定
    gEventKey = "MAIN_RELOAD";

    //取得処理実行
    $("#MainPageReloadButton").click();

    // UpdatePanel処理前後イベント
    $(document).ready(function () {
        var prm = Sys.WebForms.PageRequestManager.getInstance();

        // 開始時のイベント
        prm.add_beginRequest(function () {
        });
        // 終了時のイベント
        prm.add_endRequest(EndRequest);
        function EndRequest(sender, args) {

            if (gEventKey == "MAIN_RELOAD") {
                //メインページリロードの場合
                //イベント処理設定
                SetEvent();

                //顧客詳細、車両詳細ポップアップのスクロール設定
                $(".innerDataBox").fingerScroll();

                //入庫履歴のスクロール設定
                $(".mainblockContentRightTabWrap").fingerScroll();

                //2020/02/28 NSK 坂本 (トライ店システム評価)大量車両保有顧客における、レスポンス向上検証 START
                //保有車両のスクロール設定
//                $(".PoPuPS-CM-07ContentBodyWrap1").fingerScroll();
//                //2020/02/28 NSK 坂本 (トライ店システム評価)大量車両保有顧客における、レスポンス向上検証 END

                //CustomLabelの適用
                $(".SC3080225Ellipsis").CustomLabel({ useEllipsis: true });

                //スケジュール、カレンダー設定
                SetFutterApplication();

                //2018/02/27 NSK 山田 17PRJ01947-00_(トライ店システム評価)サービス重要顧客における、識別及び対処オペレーションの検証 START
                //SSCアイコン表示
                DisplaySscIcon();
                //2018/02/27 NSK 山田 17PRJ01947-00_(トライ店システム評価)サービス重要顧客における、識別及び対処オペレーションの検証 END
                
                //2018/07/04 NSK 可児 TKM Next Gen e-CRB Project Application development Block B-1  Pマーク、L マークなどを表示 START
                //アイコン表示
                DisplayIcon();
                //2018/07/04 NSK 可児 TKM Next Gen e-CRB Project Application development Block B-1  Pマーク、L マークなどを表示 END


                //クルクルタイムアウト終了処理
                LoadProcessHide();

            } else if (gEventKey == "SERVICEIN_RELOAD") {
                //入庫履歴リロードの場合
                //入庫履歴のスクロール設定
                $(".mainblockContentRightTabWrap").fingerScroll();

                //CustomLabelの適用
                $(".SC3080225Ellipsis").CustomLabel({ useEllipsis: true });

                //2018/02/27 NSK 山田 17PRJ01947-00_(トライ店システム評価)サービス重要顧客における、識別及び対処オペレーションの検証 START
                //SSCアイコン表示
                DisplaySscIcon();
                //2018/02/27 NSK 山田 17PRJ01947-00_(トライ店システム評価)サービス重要顧客における、識別及び対処オペレーションの検証 END

                //2018/07/04 NSK 可児 TKM Next Gen e-CRB Project Application development Block B-1  Pマーク、L マークなどを表示 START
                //アイコン表示
                DisplayIcon();
                //2018/07/04 NSK 可児 TKM Next Gen e-CRB Project Application development Block B-1  Pマーク、L マークなどを表示 END

                //クルクルタイムアウト（右側）終了処理
                LoadProcessHideRight();

            }

        }

    });

});

/********************************************************************
* ポップアップ表示非表示イベント
* @param  {popupFlg} 「0：ポップアップ非表示」
*                    「1：顧客詳細ポップアップ」
*                    「2：車両詳細ポップアップ」
*                    「3：保有車両ポップアップ」
* @return {void}
*********************************************************************/
function popupWindow(popupFlg) {

    //イベントチェック
    if (popupFlg == 0) {
        //「0：ポップアップ閉じる」の場合
        $('#PupupBackGroud').fadeOut(300);
        $('#CustomerInfo').fadeOut(300);
        $('#VehicleInfo').fadeOut(300);
        $("#VclSelectPop").fadeOut(300);

    } else if (popupFlg == 1) {
        //「1：顧客詳細ポップアップ表示」の場合
        $('#PupupBackGroud').fadeIn(300);
        $('#CustomerInfo').fadeIn(300);
        $('#VehicleInfo').fadeOut(300);
        $("#VclSelectPop").fadeOut(300);

    } else if (popupFlg == 2) {
        //「2：車両詳細ポップアップ表示」の場合
        $('#PupupBackGroud').fadeIn(300);
        $('#CustomerInfo').fadeOut(300);
        $('#VehicleInfo').fadeIn(300);
        $("#VclSelectPop").fadeOut(300);

    } else if (popupFlg == 3) {
        //「3：保有車両ポップアップ表示」の場合
        $('#PupupBackGroud').fadeIn(300);
        $('#CustomerInfo').fadeOut(300);
        $('#VehicleInfo').fadeOut(300);
        $("#VclSelectPop").fadeIn(300);

    }

}

/********************************************************************
* 地図表示処理
*********************************************************************/
function OpenGoogleMap() {

    var ArrowDir = 1;
    var posX = 490;
    var posY = 220;
    var width = 500;
    var height = 657;
    var address = $("#CstAddress").text();

    if (address == "") {
        return;
    }

    var query = "";
    query += "icrop:pmap";
    query += ":" + ArrowDir + ":";
    query += ":" + posX + ":";
    query += ":" + posY + ":";
    query += ":" + width + ":";
    query += ":" + height + ":";
    query += ":" + address;

    location.href = query;
}

/********************************************************************
* 写真表示処理
*********************************************************************/
function OpenPhotoRegister() {
    //2016/06/30 NSK 皆川 TR-SVT-TMT-20150922-001 画面で画像プロフィールが表示しない START
    ////基幹顧客IDのチェック
    //if ($("#DmsId").text() != "" && $("#DmsId").text() != null && $("#DmsId").text() != undefined) {
    //顧客IDのチェック
    if ($("#HiddenFieldCstId").val() != "" && $("#HiddenFieldCstId").val() != null && $("#HiddenFieldCstId").val() != undefined) {
        //2016/06/30 NSK 皆川 TR-SVT-TMT-20150922-001 画面で画像プロフィールが表示しない END
        //データが存在する場合
        //Photo
        var posX = 80;
        var posY = 150;
        var path = $("#HiddenFieldFileUpLoadPath").val();
        //2016/06/30 NSK 皆川 TR-SVT-TMT-20150922-001 画面で画像プロフィールが表示しない START
        //var file = $("#DmsId").text();
        var file = $("#HiddenFieldCstId").val();
        //2016/06/30 NSK 皆川 TR-SVT-TMT-20150922-001 画面で画像プロフィールが表示しない END
        var cbmethod = "CustomerPhotoRegister";
        var query = "";

        query += "icrop:came"
        query += ":" + posX + ":";
        query += ":" + posY + ":";
        query += ":" + path + file + ":";
        query += ":" + cbmethod;

        location.href = query;

    }
}

/********************************************************************
* 写真登録後に呼ばれるスクリプト処理
*********************************************************************/
function CustomerPhotoRegister(paramCode) {
    if (paramCode == 1) {
        $("#CustomerPhotoRegistButton").click();

    }
}

/********************************************************************
* クルクル非表示メソッド処理
*********************************************************************/
function LoadProcessHide() {
    var ele = document.getElementById("LoadingScreen");
    ele.style.display = "none";
    //再表示タイマーをリセット
    commonClearTimer();
}

/********************************************************************
* クルクル（全体）表示メソッド処理
*********************************************************************/
function LoadProcess() {
    var ele = document.getElementById("LoadingScreen");
    ele.style.display = "table";
}

/********************************************************************
* クルクル（右側）表示メソッド処理
*********************************************************************/
function LoadProcessRight() {
    var ele = document.getElementById("LoadingScreenRight");
    ele.style.display = "table";
}

/********************************************************************
* クルクル（右側）非表示メソッド処理
*********************************************************************/
function LoadProcessHideRight() {
    var ele = document.getElementById("LoadingScreenRight");
    ele.style.display = "none";
    //再表示タイマーをリセット
    commonClearTimer();
}

/********************************************************************
* 再表示タイマーリセット処理
*********************************************************************/
function commonClearTimer() {
    //現在時、以前のタイマーを無視する
    timerClearTime = new Date().getTime();
}

/********************************************************************
* イベント設定処理
*********************************************************************/
function SetEvent() {

    //ポップアップ外領域タッチイベント、マウスクリックイベント
    $("#PupupBackGroud").bind('click', function (e) {
        popupWindow(0);
    });

    //フッター「顧客詳細ボタン」クリック時の動作
    $('#MstPG_FootItem_Main_700').bind('click', function (event) {
        //ヘッダーの顧客検索にフォーカスを当てる
        $('#MstPG_CustomerSearchTextBox').focus();

        event.stopPropagation();
    });

    //基幹顧客IDチェック
    if ($("#DmsId").text() != "" && $("#DmsId").text() != null && $("#DmsId").text() != undefined) {
        //データが存在する場合
        //顧客氏名エリアタッチイベント、マウスクリックイベント
        $("#CustomerNameArea").bind('click', function (e) {
            popupWindow(1);
        });

        //基幹顧客コードエリアタッチイベント、マウスクリックイベント
        $("#DmsIdArea").bind('click', function (e) {
            popupWindow(1);
        });

        //保有車両チェック
        if ($("#NumberOfVehicles").text() != "" && $("#NumberOfVehicles").text() != null && $("#NumberOfVehicles").text() != undefined) {
            //データが存在する場合
            //ロゴエリアタッチイベント、マウスクリックイベント
            $("#LogoArea").bind('click', function (e) {
                popupWindow(2);
            });
        }

        //ポップアップのキャンセルボタンクリックイベント
        $("#CustomerLeftBtn").bind('click', function (e) {
            popupWindow(0);
        });

        //ポップアップのキャンセルボタンクリックイベント
        $("#VehicleLeftBtn").bind('click', function (e) {
            popupWindow(0);

        });

        //保有車両アイコンタップイベント
        $("#NumberOfVehicles").bind('click', function (e) {
            //2020/02/28 NSK 坂本 (トライ店システム評価)大量車両保有顧客における、レスポンス向上検証 START

            //popupWindow(3);

            // アイコンタップが初回の場合
            popupWindow(3);
            //クルクル表示
            LoadProcessRight();
            //クルクルタイムアウト処理
            commonRefreshTimer(function () { __doPostBack("", ""); });

            var jsonData = {
                Method: "VehicleListDisp"
            };

            //保有車両情報取得処理
            callbackSC3080225.doCallback(jsonData, SC3080225AfterCallBack);

            //2020/02/28 NSK 坂本 (トライ店システム評価)大量車両保有顧客における、レスポンス向上検証 END
        });

        //2020/02/28 NSK 坂本 (トライ店システム評価)大量車両保有顧客における、レスポンス向上検証 START
        //保有車両一覧タップイベント
//        $(".PoPuPS-CM-07Block1").bind('click', function (e) {

//            //レコード番号取得
//            var recordIndex = $(this).attr("recordIndex");

//            //レコード番号チェック
//            if (recordIndex == gVehicleSelectRecordNumber) {
//                //同じ場合
//                //ポップアップを閉じる
//                popupWindow(0);

//            } else if (recordIndex == 0) {
//                //一番上のレコード場合
//                //選択済みレコードチェック
//                if (gVehicleSelectRecordInfo != null) {
//                    //選択済みのレコードが存在する場合
//                    //選択済みのレコードの背景色を戻す
//                    gVehicleSelectRecordInfo.removeClass("BGColorBlue");

//                    //グレードの★を灰色に変更
//                    gVehicleSelectRecordInfo.find(".mainblockContentLeftCustomerEditionSelect")
//                                        .addClass("mainblockContentLeftCustomerEdition")
//                                        .removeClass("mainblockContentLeftCustomerEditionSelect");
//                    //外装色のアイコンを灰色に変更
//                    gVehicleSelectRecordInfo.find(".mainblockContentLeftCustomerColorSelect")
//                                        .addClass("mainblockContentLeftCustomerColor")
//                                        .removeClass("mainblockContentLeftCustomerColorSelect");

//                    //Reg・VIN・Del・kmの背景を灰色、文字色を白色に変更
//                    gVehicleSelectRecordInfo.find(".PopBGWhiteWordLabel")
//                                        .addClass("PopBGGrayWordLabel")
//                                        .removeClass("PopBGWhiteWordLabel");

//                    //メーカーコード・モデルコード・グレード・外装色・
//                    //車両登録No.・VIN・納車日・最新走行距離・最新走行距離更新日(文言も含め)
//                    //元の色に戻す
//                    gVehicleSelectRecordInfo.find(".WhiteWord2").addClass("GrayWord2").removeClass("WhiteWord2");
//                    gVehicleSelectRecordInfo.find(".WhiteWord3").addClass("BlackWord").removeClass("WhiteWord3");
//                    gVehicleSelectRecordInfo.find(".WhiteWord1").addClass("GrayWord1").removeClass("WhiteWord1");
//                }

//                //選択レコードの情報格納
//                gVehicleSelectRecordNumber = recordIndex;
//                gVehicleSelectRecordInfo = $(this);

//                //固有情報をJSONから変換
//                var dataList = $.parseJSON($("#HiddenFieldVehicleListJsonData").val());
//                var selectRecordData = dataList[recordIndex];

//                //データ格納処理
//                SetVehicleInfo(selectRecordData);

//                //入庫履歴再描画
//                //VINの格納
//                $("#HiddenFieldServiceInVin").val(selectRecordData.Vin);

//                //車両登録番号の格納
//                $("#HiddenFieldServiceInRegisterNumber").val(selectRecordData.VehicleRegistrationNumber);

//                //ポップアップを閉じる
//                popupWindow(0);

//                //クルクル表示
//                LoadProcessRight();

//                //クルクルタイムアウト処理
//                commonRefreshTimer(function () { __doPostBack("", ""); });

//                //2018/02/27 NSK 山田 17PRJ01947-00_(トライ店システム評価)サービス重要顧客における、識別及び対処オペレーションの検証 START
//                //SSCアイコン初期化
//                $("#SSCIcon").css('display', 'none');
//                //2018/02/27 NSK 山田 17PRJ01947-00_(トライ店システム評価)サービス重要顧客における、識別及び対処オペレーションの検証 END

//                //入庫履歴再描画処理
//                $("#ServiceInResetButton").click();

//            } else {
//                //一番上のレコードでない場合
//                //レコードを青色にする
//                $(this).addClass("BGColorBlue");

//                //グレードの★を白色に変更
//                $(this).find(".mainblockContentLeftCustomerEdition")
//                   .addClass("mainblockContentLeftCustomerEditionSelect")
//                   .removeClass("mainblockContentLeftCustomerEdition");
//                //外装色のアイコンを白色に変更
//                $(this).find(".mainblockContentLeftCustomerColor")
//                   .addClass("mainblockContentLeftCustomerColorSelect")
//                   .removeClass("mainblockContentLeftCustomerColor");

//                //Reg・VIN・Del・kmの背景を白色、文字色を灰色に変更
//                $(this).find(".PopBGGrayWordLabel")
//                   .addClass("PopBGWhiteWordLabel")
//                   .removeClass("PopBGGrayWordLabel");

//                //メーカーコード・モデルコード・グレード・外装色・
//                //車両登録No.・VIN・納車日・最新走行距離・最新走行距離更新日(文言も含め)
//                //白色に変更
//                $(this).find(".GrayWord2").addClass("WhiteWord2").removeClass("GrayWord2");
//                $(this).find(".BlackWord").addClass("WhiteWord3").removeClass("BlackWord");
//                $(this).find(".GrayWord1").addClass("WhiteWord1").removeClass("GrayWord1");

//                //選択済みレコードチェック
//                if (gVehicleSelectRecordInfo != null) {
//                    //選択済みのレコードが存在する場合
//                    //選択済みのレコードの背景色を戻す
//                    gVehicleSelectRecordInfo.removeClass("BGColorBlue");

//                    //グレードの★を灰色に変更
//                    gVehicleSelectRecordInfo.find(".mainblockContentLeftCustomerEditionSelect")
//                                        .addClass("mainblockContentLeftCustomerEdition")
//                                        .removeClass("mainblockContentLeftCustomerEditionSelect");
//                    //外装色のアイコンを灰色に変更
//                    gVehicleSelectRecordInfo.find(".mainblockContentLeftCustomerColorSelect")
//                                        .addClass("mainblockContentLeftCustomerColor")
//                                        .removeClass("mainblockContentLeftCustomerColorSelect");

//                    //Reg・VIN・Del・kmの背景を灰色、文字色を白色に変更
//                    gVehicleSelectRecordInfo.find(".PopBGWhiteWordLabel")
//                                        .addClass("PopBGGrayWordLabel")
//                                        .removeClass("PopBGWhiteWordLabel");

//                    //メーカーコード・モデルコード・グレード・外装色・
//                    //車両登録No.・VIN・納車日・最新走行距離・最新走行距離更新日(文言も含め)
//                    //元の色に戻す
//                    gVehicleSelectRecordInfo.find(".WhiteWord2").addClass("GrayWord2").removeClass("WhiteWord2");
//                    gVehicleSelectRecordInfo.find(".WhiteWord3").addClass("BlackWord").removeClass("WhiteWord3");
//                    gVehicleSelectRecordInfo.find(".WhiteWord1").addClass("GrayWord1").removeClass("WhiteWord1");
//                }

//                //選択レコードの情報格納
//                gVehicleSelectRecordNumber = recordIndex;
//                gVehicleSelectRecordInfo = $(this);

//                //固有情報をJSONから変換
//                var dataList = $.parseJSON($("#HiddenFieldVehicleListJsonData").val());
//                var selectRecordData = dataList[recordIndex];

//                //データ格納処理
//                SetVehicleInfo(selectRecordData);

//                //入庫履歴再描画
//                //VINの格納
//                $("#HiddenFieldServiceInVin").val(selectRecordData.Vin);

//                //車両登録番号の格納
//                $("#HiddenFieldServiceInRegisterNumber").val(selectRecordData.VehicleRegistrationNumber);

//                //ポップアップを閉じる
//                popupWindow(0);

//                //右側クルクル表示
//                LoadProcessRight();

//                //イベントキー設定
//                gEventKey = "SERVICEIN_RELOAD";

//                //クルクルタイムアウト処理
//                commonRefreshTimer(function () { __doPostBack("", ""); });

//                //2018/02/27 NSK 山田 17PRJ01947-00_(トライ店システム評価)サービス重要顧客における、識別及び対処オペレーションの検証 START
//                //SSCアイコン初期化
//                $("#SSCIcon").css('display', 'none');
//                //2018/02/27 NSK 山田 17PRJ01947-00_(トライ店システム評価)サービス重要顧客における、識別及び対処オペレーションの検証 END
//                
//                //入庫履歴再描画処理
//                $("#ServiceInResetButton").click();

//            }

//        });
        //2020/02/28 NSK 坂本 (トライ店システム評価)大量車両保有顧客における、レスポンス向上検証 END

        //「全ての入庫履歴を表示する」タップイベント
        $("#AllDispLinkDiv").live('click', function (e) {
            $("#AllDispLinkDiv").css('display', 'none');
            $("#NextLodingDiv").css('display', 'block');

            //イベントキー設定
            gEventKey = "SERVICEIN_RELOAD";

            //クルクルタイムアウト処理
            commonRefreshTimer(function () { __doPostBack("", ""); });

            $("#AllDispLinkButton").click();

        });

        //「次のN件を表示する」タップイベント
        $("#NextDispLinkDiv").live('click', function (e) {
            $("#NextDispLinkDiv").css('display', 'none');
            $("#NextLodingDiv").css('display', 'block');

            //イベントキー設定
            gEventKey = "SERVICEIN_RELOAD";

            //クルクルタイムアウト処理
            commonRefreshTimer(function () { __doPostBack("", ""); });

            $("#AllDispLinkButton").click();

        });

        //住所タップイベント
        $(".mainblockContentLeftCustomerDetailRight").bind('click', function (e) {
            OpenGoogleMap();

        });

        //入庫履歴一覧タップイベント
        $("#mainblockContentRightTabAll01").live('click', function (e) {

            //レコード番号取得
            var serviceinValue = $(this).attr("serviceinValue").split(",");

            //RO番号チェック
            if (serviceinValue[1] != "" && serviceinValue[1] != null && serviceinValue[1] != undefined) {
                //存在する場合
                //データを格納
                $("#HiddenFieldDealerCode").val(serviceinValue[0]);
                $("#HiddenFieldOrderNumber").val(serviceinValue[1]);
                $("#HiddenFieldServiceInNumber").val(serviceinValue[2]);

                //クルクル表示
                LoadProcess();

                //クルクルタイムアウト処理
                commonRefreshTimer(function () { __doPostBack("", ""); });

                //ROプレビュー画面遷移処理
                $("#ServiceInEventButton").click();

            }

        });

        //2014/09/22 SKFC 佐藤 e-Mail,Line送信機能対応 START
        //メールアドレスチェック
        if ($("#CstEmail").text() != "" && $("#CstEmail").text() != null && $("#CstEmail").text() != undefined) {
            //データが存在する場合
            //メールアドレスタッチイベント、マウスクリックイベント
            $("#CstEmail").live('click', function (e) {
                var url = window.location.href.replace("SC3080225.aspx", "SC3180203.aspx");
                // e-Mail,Line送信機能を起動
                var ret = icrop.clientapplication.sendMessage({
                    DealerCode: $("#HiddenFieldDealerCode").val(),
                    StoreCode: $("#HiddenFieldStoreCode").val(),
                    TemplateClass: 1,
                    DisplayId: "SC3080225",
                    CstId: $("#HiddenFieldCstId").val(),
                    FileName: "PDF",
                    Source: url,
                    WaitTimeIntervalAfterLoad: 1,
                    Margin: "0/0/0/0",
                    DebugParam: $("#HiddenFieldOrderNumber").val()
                });
            });
        }
        //2014/09/22 SKFC 佐藤 e-Mail,Line送信機能対応 END

    }

}

//2020/02/28 NSK 坂本 (トライ店システム評価)大量車両保有顧客における、レスポンス向上検証 START

/********************************************************************
* 保有車両一覧タップイベント設定処理
*********************************************************************/
function SetVehidleListEvent() {
    //保有車両一覧タップイベント
    $(".PoPuPS-CM-07Block1").bind('click', function (e) {

        //レコード番号取得
        var recordIndex = $(this).attr("recordIndex");

        //レコード番号チェック
        if (recordIndex == gVehicleSelectRecordNumber) {
            //同じ場合
            //ポップアップを閉じる
            popupWindow(0);

        } else if (recordIndex == 0) {
            //一番上のレコード場合
            //選択済みレコードチェック
            if (gVehicleSelectRecordInfo != null) {
                //選択済みのレコードが存在する場合
                //選択済みのレコードの背景色を戻す
                gVehicleSelectRecordInfo.removeClass("BGColorBlue");

                //グレードの★を灰色に変更
                gVehicleSelectRecordInfo.find(".mainblockContentLeftCustomerEditionSelect")
                                        .addClass("mainblockContentLeftCustomerEdition")
                                        .removeClass("mainblockContentLeftCustomerEditionSelect");
                //外装色のアイコンを灰色に変更
                gVehicleSelectRecordInfo.find(".mainblockContentLeftCustomerColorSelect")
                                        .addClass("mainblockContentLeftCustomerColor")
                                        .removeClass("mainblockContentLeftCustomerColorSelect");

                //Reg・VIN・Del・kmの背景を灰色、文字色を白色に変更
                gVehicleSelectRecordInfo.find(".PopBGWhiteWordLabel")
                                        .addClass("PopBGGrayWordLabel")
                                        .removeClass("PopBGWhiteWordLabel");

                //メーカーコード・モデルコード・グレード・外装色・
                //車両登録No.・VIN・納車日・最新走行距離・最新走行距離更新日(文言も含め)
                //元の色に戻す
                gVehicleSelectRecordInfo.find(".WhiteWord2").addClass("GrayWord2").removeClass("WhiteWord2");
                gVehicleSelectRecordInfo.find(".WhiteWord3").addClass("BlackWord").removeClass("WhiteWord3");
                gVehicleSelectRecordInfo.find(".WhiteWord1").addClass("GrayWord1").removeClass("WhiteWord1");
            }

            //選択レコードの情報格納
            gVehicleSelectRecordNumber = recordIndex;
            gVehicleSelectRecordInfo = $(this);

            //固有情報をJSONから変換
            var dataList = $.parseJSON($("#HiddenFieldVehicleListJsonData").val());
            var selectRecordData = dataList[recordIndex];

            //データ格納処理
            SetVehicleInfo(selectRecordData);

            //入庫履歴再描画
            //VINの格納
            $("#HiddenFieldServiceInVin").val(selectRecordData.Vin);

            //車両登録番号の格納
            $("#HiddenFieldServiceInRegisterNumber").val(selectRecordData.VehicleRegistrationNumber);

            //ポップアップを閉じる
            popupWindow(0);

            //クルクル表示
            LoadProcessRight();

            //クルクルタイムアウト処理
            commonRefreshTimer(function () { __doPostBack("", ""); });

            //2018/02/27 NSK 山田 17PRJ01947-00_(トライ店システム評価)サービス重要顧客における、識別及び対処オペレーションの検証 START
            //SSCアイコン初期化
            $("#SSCIcon").css('display', 'none');
            //2018/02/27 NSK 山田 17PRJ01947-00_(トライ店システム評価)サービス重要顧客における、識別及び対処オペレーションの検証 END

            //入庫履歴再描画処理
            $("#ServiceInResetButton").click();

        } else {
            //一番上のレコードでない場合
            //レコードを青色にする
            $(this).addClass("BGColorBlue");

            //グレードの★を白色に変更
            $(this).find(".mainblockContentLeftCustomerEdition")
                   .addClass("mainblockContentLeftCustomerEditionSelect")
                   .removeClass("mainblockContentLeftCustomerEdition");
            //外装色のアイコンを白色に変更
            $(this).find(".mainblockContentLeftCustomerColor")
                   .addClass("mainblockContentLeftCustomerColorSelect")
                   .removeClass("mainblockContentLeftCustomerColor");

            //Reg・VIN・Del・kmの背景を白色、文字色を灰色に変更
            $(this).find(".PopBGGrayWordLabel")
                   .addClass("PopBGWhiteWordLabel")
                   .removeClass("PopBGGrayWordLabel");

            //メーカーコード・モデルコード・グレード・外装色・
            //車両登録No.・VIN・納車日・最新走行距離・最新走行距離更新日(文言も含め)
            //白色に変更
            $(this).find(".GrayWord2").addClass("WhiteWord2").removeClass("GrayWord2");
            $(this).find(".BlackWord").addClass("WhiteWord3").removeClass("BlackWord");
            $(this).find(".GrayWord1").addClass("WhiteWord1").removeClass("GrayWord1");

            //選択済みレコードチェック
            if (gVehicleSelectRecordInfo != null) {
                //選択済みのレコードが存在する場合
                //選択済みのレコードの背景色を戻す
                gVehicleSelectRecordInfo.removeClass("BGColorBlue");

                //グレードの★を灰色に変更
                gVehicleSelectRecordInfo.find(".mainblockContentLeftCustomerEditionSelect")
                                        .addClass("mainblockContentLeftCustomerEdition")
                                        .removeClass("mainblockContentLeftCustomerEditionSelect");
                //外装色のアイコンを灰色に変更
                gVehicleSelectRecordInfo.find(".mainblockContentLeftCustomerColorSelect")
                                        .addClass("mainblockContentLeftCustomerColor")
                                        .removeClass("mainblockContentLeftCustomerColorSelect");

                //Reg・VIN・Del・kmの背景を灰色、文字色を白色に変更
                gVehicleSelectRecordInfo.find(".PopBGWhiteWordLabel")
                                        .addClass("PopBGGrayWordLabel")
                                        .removeClass("PopBGWhiteWordLabel");

                //メーカーコード・モデルコード・グレード・外装色・
                //車両登録No.・VIN・納車日・最新走行距離・最新走行距離更新日(文言も含め)
                //元の色に戻す
                gVehicleSelectRecordInfo.find(".WhiteWord2").addClass("GrayWord2").removeClass("WhiteWord2");
                gVehicleSelectRecordInfo.find(".WhiteWord3").addClass("BlackWord").removeClass("WhiteWord3");
                gVehicleSelectRecordInfo.find(".WhiteWord1").addClass("GrayWord1").removeClass("WhiteWord1");
            }

            //選択レコードの情報格納
            gVehicleSelectRecordNumber = recordIndex;
            gVehicleSelectRecordInfo = $(this);

            //固有情報をJSONから変換
            var dataList = $.parseJSON($("#HiddenFieldVehicleListJsonData").val());
            var selectRecordData = dataList[recordIndex];

            //データ格納処理
            SetVehicleInfo(selectRecordData);

            //入庫履歴再描画
            //VINの格納
            $("#HiddenFieldServiceInVin").val(selectRecordData.Vin);

            //車両登録番号の格納
            $("#HiddenFieldServiceInRegisterNumber").val(selectRecordData.VehicleRegistrationNumber);

            //ポップアップを閉じる
            popupWindow(0);

            //右側クルクル表示
            LoadProcessRight();

            //イベントキー設定
            gEventKey = "SERVICEIN_RELOAD";

            //クルクルタイムアウト処理
            commonRefreshTimer(function () { __doPostBack("", ""); });

            //2018/02/27 NSK 山田 17PRJ01947-00_(トライ店システム評価)サービス重要顧客における、識別及び対処オペレーションの検証 START
            //SSCアイコン初期化
            $("#SSCIcon").css('display', 'none');
            //2018/02/27 NSK 山田 17PRJ01947-00_(トライ店システム評価)サービス重要顧客における、識別及び対処オペレーションの検証 END

            //入庫履歴再描画処理
            $("#ServiceInResetButton").click();

        }

    });
}

//2020/02/28 NSK 坂本 (トライ店システム評価)大量車両保有顧客における、レスポンス向上検証 END


/********************************************************************
* 車両詳細データ設定処理
*********************************************************************/
function SetVehicleInfo(VehicleObject) {
    //車両エリアの設定

    if (VehicleObject.ModelLogoOffURL != "") {
        //車両ロゴ
        $("#VehicleLogoIcon").attr("src", VehicleObject.ModelLogoOffURL);

        //ロゴの表示
        $("#VehicleLogoIcon").attr("style", "display:block;");
        $("#VehicleMakerModelTable").attr("style", "display:none;");

    } else {
        //メーカー名
        $("#VehicleMakerName").text(VehicleObject.MakerCode);

        //モデルコード
        $("#VehicleModelName").text(VehicleObject.SERIESCD);

        //メーカー名とモデルコードの表示
        $("#VehicleLogoIcon").attr("style", "display:none;");
        $("#VehicleMakerModelTable").attr("style", "display:block;");
    }

    //グレード名
    $("#VehicleGrade").text(VehicleObject.Grade);

    //外装色
    $("#VehicleBodyColor").text(VehicleObject.BodyColorName);

    //車両登録番号
    $("#VehicleRegNo").text(VehicleObject.VehicleRegistrationNumber);

    //車両登録エリア名称
    $("#VehicleProvince").text(VehicleObject.VehicleAreaName);

    //VIN
    $("#VehicleVin").text(VehicleObject.Vin);

    //納車日（YYYY/MM/DD）
    $("#VehicleDeliveryDate").text(VehicleObject.VehicleDeliveryDate);

    //最新走行距離
    $("#LatestMileage").text(VehicleObject.Mileage);

    //最新走行距離更新日（MM/DD）
    $("#LatestMileageUpdateDate").text(VehicleObject.LastUpdateDate);

    //セールス担当者名
    $("#SalesStaffName").text(VehicleObject.SalesStaffName);

    //サービス担当者名
    $("#ServiceStaffName").text(VehicleObject.ServiceAdviserName);


    //車両ポップアップエリアの設定
    //メーカー名
    $("#VclPopMakerName").text(VehicleObject.MakerCode);

    //モデルコード
    $("#VclPopModelName").text(VehicleObject.ModelCode);

    //車両登録No
    $("#VclPopRegNo").text(VehicleObject.VehicleRegistrationNumber);

    //車両登録エリア名称
    $("#VclPopProvince").text(VehicleObject.VehicleAreaName);

    //VIN
    $("#VclPopVin").text(VehicleObject.Vin);

    //基本型式
    $("#VclPopKatashiki").text(VehicleObject.BaseType);

    //燃料
    $("#VclPopFuel").text(VehicleObject.FuelDivisionName);

    //外板色名称
    $("#VclPopBodyColor").text(VehicleObject.BodyColorName);

    //エンジンNo
    $("#VclPopEngineNo").text(VehicleObject.EngineNumber);

    //トランスミッション
    $("#VclPopTransmission").text(VehicleObject.Transmission);

    //登録日
    $("#VclPopRegDate").text(VehicleObject.VehicleRegistrationDate);

    //納車日
    $("#VclPopDeliDate").text(VehicleObject.VehicleDeliveryDate);

    //車両区分
    $("#VclPopVehicleType").text(VehicleObject.NewVehicleDivisionName);

    //最終整備完了日
    $("#VclPopServiceCompletedDate").text(VehicleObject.RegistDate);

    //最新走行距離
    $("#VclPopMileage").text(VehicleObject.Mileage);

    //保険会社名
    $("#VclPopInsuranceCompany").text(VehicleObject.CompanyName);

    //保険証券番号
    $("#VclPopInsurancePolicyNo").text(VehicleObject.InsNo);

    //保険満期日
    $("#VclPopInsuranceExpiryDate").text(VehicleObject.EndDate);

}

/********************************************************************
* スケジュールボタンと電話帳ボタンの設定する.
*********************************************************************/
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

//2018/02/27 NSK 山田 17PRJ01947-00_(トライ店システム評価)サービス重要顧客における、識別及び対処オペレーションの検証 START
/********************************************************************
* SSCアイコンの表示/非表示切替
*********************************************************************/
function DisplaySscIcon() {
    if ($("#HiddenFieldSscFlag").val() == C_SSC_ON) {
        $("#SSCIcon").css('display', 'block');
    } else {
        $("#SSCIcon").css('display', 'none');
    }
}
//2018/02/27 NSK 山田 17PRJ01947-00_(トライ店システム評価)サービス重要顧客における、識別及び対処オペレーションの検証 END

//2018/07/04 NSK 可児 TKM Next Gen e-CRB Project Application development Block B-1  Pマーク、L マークなどを表示 START
/********************************************************************
* アイコンの表示/非表示切替
*********************************************************************/
function DisplayIcon() {
    //M/Bアイコン
    if ($("#HiddenFieldSmlAmcFlg").val() == C_ICON_ON_1) {
        $("#MIcon").css('display', 'block');
        $("#BIcon").css('display', 'none');
    } else if ($("#HiddenFieldSmlAmcFlg").val() == C_ICON_ON_2) {
        $("#BIcon").css('display', 'block')
        $("#MIcon").css('display', 'none');
    } else {
        $("#MIcon").css('display', 'none');
        $("#BIcon").css('display', 'none');
    }
    //Eアイコン
    if ($("#HiddenFieldEwFlg").val() == C_ICON_ON_1) {
        $("#EIcon").css('display', 'block');
    } else {
        $("#EIcon").css('display', 'none');
    }
    //Tアイコン
    if ($("#HiddenFieldTlmMbrFlg").val() == C_ICON_ON_1) {
        $("#TIcon").css('display', 'block');
    } else {
        $("#TIcon").css('display', 'none');
    }
    //P/Lアイコン
    if ($("#HiddenFieldImpFlg").val() == C_ICON_ON_1) {
        $("#PIcon").css('display', 'block');
        $("#LIcon").css('display', 'none');
    } else if ($("#HiddenFieldImpFlg").val() == C_ICON_ON_2) {
        $("#LIcon").css('display', 'block')
        $("#PIcon").css('display', 'none');
    } else {
        $("#PIcon").css('display', 'none');
        $("#LIcon").css('display', 'none');
    }
}
//2018/07/04 NSK 可児 TKM Next Gen e-CRB Project Application development Block B-1  Pマーク、L マークなどを表示 END

//2020/02/28 NSK 坂本 (トライ店システム評価)大量車両保有顧客における、レスポンス向上検証 START

/********************************************************************
* コールバック関数定義
*********************************************************************/
var callbackSC3080225 = {
    doCallback: function (argument, callbackFunction) {
        this.packedArgument = JSON.stringify(argument);
        this.endCallback = callbackFunction;
        this.beginCallback();
    }
};

/********************************************************************
* コールバック後の処理関数
*********************************************************************/
function SC3080225AfterCallBack(result, context) {

    var result = $.parseJSON(result);

    //コールバック結果コードの取得
    var resultCd = result.ResultCode;

    if (resultCd == 0) {
        //JSON形式のコールバック結果を変換する
        var vehicleList = $.parseJSON(result.Contents);
        var wordDict = result.WordDict;
        var html = "";

        //前の件数表示
        if (result.BeforeFlg == 1) {
            html += GetBeforeButton(wordDict);
        }

        //HTMLを生成
        html += vehicleListHtml = GetVehicleListItem(vehicleList, wordDict);

        $("#HiddenFieldVehicleListJsonData").val(result.Contents);

        //次の件数表示
        if (result.NextFlg == 1) {
            html += GetNextButton(wordDict);
        }

        $(".PoPuPS-CM-07ContentBodyWrap1").empty();
        $(html).appendTo(".PoPuPS-CM-07ContentBodyWrap1");

        //車両一覧タップ時のイベントを設定
        SetVehidleListEvent();

        //前のN件タップイベント
        $("#NumberOfVehiclesBefore").bind('click', function (e) {

            $(".FrontList")[0].textContent = wordDict["210"];

            //クルクル表示
            LoadProcessRight();
            //クルクルタイムアウト処理
            commonRefreshTimer(function () { __doPostBack("", ""); });

            var jsonData = {
                Method: "VehicleListDispBefore",
                Start: result.NowStart,
                Count: result.NowCount
            };

            //保有車両情報取得処理
            callbackSC3080225.doCallback(jsonData, SC3080225AfterCallBack);

        });

        //次のN件タップイベント
        $("#NumberOfVehiclesAfter").bind('click', function (e) {

            $(".NextList")[0].textContent = wordDict["212"];

            //クルクル表示
            LoadProcessRight();
            //クルクルタイムアウト処理
            commonRefreshTimer(function () { __doPostBack("", ""); });

            var jsonData = {
                Method: "VehicleListDispAfter",
                Start: result.NowStart,
                Count: result.NowCount
            };

            //保有車両情報取得処理
            callbackSC3080225.doCallback(jsonData, SC3080225AfterCallBack);
        });

        //保有車両のスクロール設定
        $(".PoPuPS-CM-07ContentBodyWrap1").fingerScroll();

        //クルクルタイムアウト（右側）終了処理
        LoadProcessHideRight();
    }
    else {
        //クルクルタイムアウト（右側）終了処理
        LoadProcessHideRight();

        //エラーメッセージを表示する
        icropScript.ShowMessageBox(resultCd, result.Message, "");

        //ポップアップを閉じる
        popupWindow(0);

    }
}

/********************************************************************
* 保有車両リスト作成処理
*********************************************************************/
function GetVehicleListItem(objVehicleListItems, objWordDict) {

    var html = "";

    for (var index in objVehicleListItems) {

        var item = objVehicleListItems[index];

        if (index == "0") {
            html += "<div class='PoPuPS-CM-07Block1 BGColorGray' RecordIndex='" + index + "'>";
        }
        else {
            html += "<div class='PoPuPS-CM-07Block1' RecordIndex='" + index + "'>";
        }

        html += "<div class='mainblockContentLeftCustomerCarName'>";
        html += "<dl>";
        //ロゴ"
        html += "<dt id='VclSelPopLogoArea'>";

        if (item["ModelLogoOffURL"] != '' && item["ModelLogoOnURL"] != '') {
            html += "<img id='VclSelPopVehicleLogoIcon' src='" + item["ModelLogoOffURL"] + "' style='display:block;' />";
            html += "<table id='VclSelPopVehicleMakerModelTable' class='NoBorderTable' style='display:none'>";
            html += "<tr>";
        }
        else {
            html += "<img id='VclSelPopVehicleLogoIcon' src='" + item["ModelLogoOffURL"] + "' style='display:none;' />";
            html += "<table id='VclSelPopVehicleMakerModelTable' class='NoBorderTable' style='display:block'>";
        }

        html += "<td>";
        html += "<span id='VclSelPopMakerName' class='SC3080225Ellipsis GrayWord2'>" + item["MakerCode"] + "</span>";
        html += "</td>";
        html += "<td class='CarTypeBoldText'>";
        html += "<span id='VclSelPopModelName' class='SC3080225Ellipsis BlackWord'>" + item["SERIESCD"] + "</span>";
        html += "</td>";
        html += "</tr>";
        html += "</table>";
        html += "</dt>";
        //グレード
        html += "<dd class='mainblockContentLeftCustomerEdition'>";
        html += "<span id='VclSelPopGrade' class='SC3080225Ellipsis GrayWord1'>" + item["Grade"] + "</span>";
        html += "</dd>";
        //外装色
        html += "<dd class='mainblockContentLeftCustomerColor'>";
        html += "<span id='VclSelPopBodyColor' class='SC3080225Ellipsis GrayWord1'>" + item["BodyColorName"] + "</span>";
        html += "</dd>";
        html += "</dl>";
        html += "<div class='mainblockContentLeftCustomerCarDetail'>";
        html += "<ul>";
        //車両登録番号
        html += "<li class='mainblockContentLeftCustomerCarDetail1'>";
        html += "<span id='VclSelPopRegNoWord' class='PopBGGrayWordLabel SC3080225Ellipsis'>" + objWordDict["8"] + "</span>";
        html += "<span id='VclSelPopRegNo' class='SC3080225Ellipsis GrayWord1'>" + item["VehicleRegistrationNumber"] + "</span>";
        html += "</li>";
        //VIN
        html += "<li class='mainblockContentLeftCustomerCarDetail2'>";
        html += "<span id='VclSelPopVinWord' class='PopBGGrayWordLabel SC3080225Ellipsis'>" + objWordDict["9"] + "</span>";
        html += "<span id='VclSelPopVin' class='SC3080225Ellipsis GrayWord1'>" + item["Vin"] + "</span>";
        html += "</li>";
        //納車日
        html += "<li class='mainblockContentLeftCustomerCarDetail3'>";
        html += "<span id='VclSelPopDeliveryDateWord' class='PopBGGrayWordLabel SC3080225Ellipsis'>" + objWordDict["10"] + "</span>";
        html += "<span id='VclSelPopDeliveryDate' class='SC3080225Ellipsis GrayWord1'>" + item["VehicleDeliveryDate"] + "</span>";
        html += "</li>";
        //最新走行距離
        html += "<li class='mainblockContentLeftCustomerCarDetail4'>";
        html += "<span id='VclSelPopLatestMileageWord' class='PopBGGrayWordLabel SC3080225Ellipsis'>" + objWordDict["11"] + "</span>";
        html += "<span id='VclSelPopLatestMileage' class='SC3080225Ellipsis GrayWord1'>" + item["Mileage"] + "</span>";
        //最新走行距離更新日
        html += "<span id='VclSelPopLatestMileageUpdateDateWord' class='SC3080225Ellipsis GrayWord2'>" + objWordDict["170"] + "</span>";
        html += "<span id='VclSelPopLatestMileageUpdateDate' class='SC3080225Ellipsis GrayWord2'>" + item["LastUpdateDate"] + "</span>";
        html += "</li>";
        html += "</ul>";
        html += "</div>";
        html += "</div>";
        html += "</div>";

    }

    return html;

}

function GetBeforeButton(objWordDict) {

    var html = "";

    html += "<li class='FrontLink' ID='NumberOfVehiclesBefore' runat='server'>";
    html += "<div class='FrontList' ID='FrontList' runat='server'>" + objWordDict["209"] + "</div>";
    html += "</li>";

    return html
}
function GetNextButton(objWordDict) {

    var html = "";

    html += "<li class='NextLink' id='NumberOfVehiclesAfter' runat='server'>";
    html += "<div class='NextList' ID='NextList' runat='server'>" + objWordDict["211"] + "</div>";
	html += "</li>";

    return html
}

//2020/02/28 NSK 坂本 (トライ店システム評価)大量車両保有顧客における、レスポンス向上検証 END