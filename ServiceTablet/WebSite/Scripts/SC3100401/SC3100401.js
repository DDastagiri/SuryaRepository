//-------------------------------------------------------------------------
//SC3100401.js
//-------------------------------------------------------------------------
//機能：未振当て一覧画面
//補足：
//作成：2013/03/01 TMEJ 河原
//更新：2013/12/10 TMEJ 河原 次世代サービス 工程管理機能開発
//更新：2013/12/26 TMEJ 河原 次世代e-CRBタブレット(サービス) iOS7.0 VersionUp対応
//更新：2014/01/23 TMEJ 小澤 IT9611_次世代サービス 工程管理機能開発
//更新：2017/03/23 NSK  竹中 TR-SVT-TMT-20170117-001 SVR画面でランダムな車両でJOBオープンする
//更新：2018/02/19 NSK  山田 REQ-SVT-TMT-20170615-001 iPod Gatekeeperに顧客の車両登録番号を入力する機能を追加
//更新：2019/06/14 NSK  近藤 TKM PUAT-4066 TableNoに値を入力後、SAを振当てるとアラートメッセージが表示される

/****************************************
* グローバル変数宣言
****************************************/
//クライアントメッセージ保存用
var gClientMessage;

//来店一覧のチェックボックスの値保存用
var gButtomName;

//来店一覧のチェックボックスの値保存用
var gCheckBoxValue;

//変更前の値保存用
var gBeforeValue;

//SA振当用データ保存用
var gSAAssignValue = {
          VisitSeq: null
        , AssignStatus: null
        , BeforeAccount: null
        , AfterAccount: null
        , UpDateDate: null
        , EventKeyID: null
      };

// 2017/03/23 NSK  竹中 TR-SVT-TMT-20170117-001 SVR画面でランダムな車両でJOBオープンする START
//計算機表示中フラグ
var gNumericKeypadDispFlg;

//変更前の車両登録番号
var gBeforeRegNo;

//来店情報のリスト名
var gListName;
// 2017/03/23 NSK  竹中 TR-SVT-TMT-20170117-001 SVR画面でランダムな車両でJOBオープンする END

// 2019/06/14 NSK  近藤 TKM PUAT-4066 TableNoに値を入力後、SAを振当てるとアラートメッセージが表示される START
// コールバック実行中であることを表すフラグ
var gCallbackFunctionRunFlg = false;

// クリックしたフッターのボタンID
var gFooterButtonId;
// 2019/06/14 NSK  近藤 TKM PUAT-4066 TableNoに値を入力後、SAを振当てるとアラートメッセージが表示される END

/****************************************
* 定数宣言
****************************************/

//タッチイベント名
var C_TOUCH = "click";

//初期表示ボタン名
var C_MAIN_LOAD_BUTTOM = "MainLoadingButton";
//来店一覧の最大行数
var C_RECEPTION_LIST_MAX_ROW = 8;
//CSSCLASS名(チェック無し)
var C_CHECK = "Check";
//CSSCLASS名(チェック有り)
var C_CHECK_ON = "Check_on";
//CSSCLASS名(SAボタン選択状態)
var C_INACTIVE = "Inactive";
//CSSCLASS名(SA一覧のチェックボックス非活性)
var C_SA_CHECK = "SACheck";
//CSSCLASS名(SA一覧のチェックボックス活性)
var C_CHECKFREE = "CheckFree";
//CSSCLASS名(SA一覧の非選択状態)
var C_CASSETTE = "Cassette";
//CSSCLASS名(SA一覧の選択状態設定)
var C_CASSETTE_CCHECK = "Cassette C_Check";

//CSSCLASS名(SA一覧の選択状態設定Jクエリ用)
var C_UL_CASSETTE_CCHECK = "ul.Cassette.C_Check";

//AttributesNAME [来店実績連番]
var C_VISITSEQ = "visitseq";
//2014/01/23 TMEJ 小澤 IT9611_次世代サービス 工程管理機能開発 START
//AttributesNAME [予約ID]
var C_RESERVEID = "rezid";
//2014/01/23 TMEJ 小澤 IT9611_次世代サービス 工程管理機能開発 END
//AttributesNAME [呼出ステータス]
var C_CALL_STATUS = "callstatus";
//AttributesNAME [更新日時]
var C_UP_DATE_DATE = "updatedate";
//AttributesNAME [振当ステータス]
var C_ASSIGN_STATUS = "assignstatus";
//AttributesNAME チェックボックスイベントキー
var C_CHECK_EVENT = "eventclass";
//AttributesNAME [担当SAコード]
var C_SA_CODE = "sacode";
//AttributesNAME イベントキー
var C_ITEM_ID = "itemid";

//AttributesNAME [SAアカウント]
var C_ACCOUNT = "account";

//フッターボタンAttributesNAME
var C_BUTTON_ID = "buttonid";

//イベントキーフッターボタン「呼出」ID
var C_CALL_BUTTON = "100";
//イベントキーフッターボタン「呼出キャンセル」ID
var C_CALL_CANCEL_BUTTON = "200";
//イベントキーフッターボタン「チップ削除」ID
var C_TIP_DELETE_BUTTON = "300";

//イベントキー「受付番号計算機OKボタン」ID
var C_RECEIPT_NUMBER = "1100";
//イベントキー「車両登録NOフォーカスアウト」ID
var C_REGNO_TEXT = "1200";
//イベントキー「来店者フォーカスアウト」ID
var C_VISITOR_TEXT = "1300";
//イベントキー「電話番号フォーカスアウト」ID
var C_TELLNO_TEXT = "1400";
//イベントキー「テーブルNOフォーカスアウト」ID
var C_TABLENO_TEXT = "1500";

//イベントキー「SA振当」ID
var C_SA_ASSIGN = "3000";
//イベントキー「SA変更」ID
var C_SA_CHANGE = "3100";
//イベントキー「SA解除」ID
var C_SA_UNDO = "3200";

//非同期登録前チェック「0：変更無し」
var C_NO_CHANGE = "0";
//非同期登録前チェック「1：変更有り」
var C_CHANG = "1";
//非同期登録前チェック「2：エラー」
var C_CHANG_ERR = "2";

//呼出ステータス「0：未呼出」
var C_NO_CALL = "0";
//呼出ステータス「1：呼出中」
var C_CALLING = "1";

//振当てステータス「0：未振当」
var C_NO_ASSIGN = "0";
//振当てステータス「1：受付待ち」
var C_ASSIGN_WAIT = "1";
//振当てステータス「2：振当済」
var C_ASSIGN_FIN = "2";

// 2019/06/14 NSK  近藤 TKM PUAT-4066 TableNoに値を入力後、SAを振当てるとアラートメッセージが表示される START
// 定数値「""：空文字」
var C_EMPTY_STRING = "";
// 2019/06/14 NSK  近藤 TKM PUAT-4066 TableNoに値を入力後、SAを振当てるとアラートメッセージが表示される END

//コールバック処理結果「0：成功」
var C_RESULT_SUCCESS = 0;


//来店一覧の空行
var C_APPEND_LIST = '<li><ul class="ListCassette"><li></li><li></li><li></li><li></li><li></li><li></li><li></li><li></li><li></li><li></li></ul></li>'


// 2013/12/10 TMEJ 河原 次世代サービス 工程管理機能開発 START

//受付モニター使用フラグ("0"：使用しない)
var C_RECEPT_FALG = "0";

// 2013/12/10 TMEJ 河原 次世代サービス 工程管理機能開発 END


//アクティブインジケータ表示操作関数定義
var gActiveIndicator = {
    show: function () {
        $("#LoadingScreen").css({ "display": "table" });
    }
    ,
    hide: function () {
        $("#LoadingScreen").css({ "display": "none" });
    }
};

//来店一覧ダミー表示関数
var gLeftBoxListDammy = {
    show: function () {
        $("ul.LeftBoxListDammy").css({ "display": "block" });
    }
    ,
    hide: function () {
        $("ul.LeftBoxListDammy").css({ "display": "none" });
    }
};

//来店一覧表示関数
var gLeftBoxList = {
    show: function () {
        $("ul.LeftBoxListSet").css({ "display": "block" });
    }
    ,
    hide: function () {
        $("ul.LeftBoxListSet").css({ "display": "none" });
    }
};

//SA一覧表示関数
var gRightBoxList = {
    show: function () {
        $("ul.RightBoxListSet").css({ "display": "block" });
    }
    ,
    hide: function () {
        $("ul.RightBoxListSet").css({ "display": "none" });
    }
};

//2014/01/23 TMEJ 小澤 IT9611_次世代サービス 工程管理機能開発 START
////メインフッター表示関数
//var gMainFooter = {
//    show: function () {
//        $("div.MainFooter").css({ "display": "block" });
//    }
//    ,
//    hide: function () {
//        $("div.MainFooter").css({ "display": "none" });
//    }
//};
//2014/01/23 TMEJ 小澤 IT9611_次世代サービス 工程管理機能開発 END

//サブフッター表示関数
var gCheckFooter = {
    show: function () {
        $("div.CheckFooter").css({ "display": "block" });
    }
    ,
    hide: function () {
        $("div.CheckFooter").css({ "display": "none" });
    }
};

//ブラックアウト表示関数
var gBlackBack = {
    show: function () {
        $("div.BlackBack").css({ "display": "block" });
    }
    ,
    hide: function () {
        $("div.BlackBack").css({ "display": "none" });
    }
};

//SAリストシャドウ表示関数
var gRightPopBox = {
    show: function () {
        $("div.RightPopBox").css({ "display": "block" });
    }
    ,
    hide: function () {
        $("div.RightPopBox").css({ "display": "none" });
    }
};

//SAリストクリックDiv表示関数
var gSAListClick = {
    show: function () {
        $("div.SAListClick").css({ "display": "block" });
    }
    ,
    hide: function () {
        $("div.SAListClick").css({ "display": "none" });
    }
};

//SA振当て登録ボタン表示関数
var gRightBoxSARegisterButton = {
    show: function () {
        $("div.RightBoxSARegisterButton").css({ "display": "block" });
    }
    ,
    hide: function () {
        $("div.RightBoxSARegisterButton").css({ "display": "none" });
    }
};

//メインフッターアクティブ表示関数
var gMainFooterActive = {
    on: function () {

        $("div.InnerBox01").addClass("icrop-pressed");

    }
    ,
    off: function () {

        $("div.InnerBox01").removeClass("icrop-pressed");
    }
};

//トリム関数
String.prototype.trim = function () {

    //文字列の先頭および末尾の連続する「半角空白・タブ文字・全角空白」を削除します
    return this.replace(/^[\s　]+|[\s　]+$/g, "");

};

/*************************************************************************************************************
* コールバック関数定義
* 
* @param {String} argument サーバーに渡すパラメータ
* @param {String} callbackFunction コールバック後に実行するメソッド
* 
**************************************************************************************************************/
var gCallbackSC3100401 = {
    doSC3100401Callback: function (argument, callbackFunction) {

        // 2019/06/19 NSK  近藤 TKM PUAT-4066 TableNoに値を入力後、SAを振当てるとアラートメッセージが表示される START
        // フラグを「true:コールバック実行中」にする
        gCallbackFunctionRunFlg = true;
        // 2019/06/19 NSK  近藤 TKM PUAT-4066 TableNoに値を入力後、SAを振当てるとアラートメッセージが表示される END

        this.packedArgument = JSON.stringify(argument);
        this.endCallback = callbackFunction;
        this.beginCallback();
    }
};


/****************************************
* メイン処理
****************************************/

//DOMロード時の処理
$(function () {

    //アクティブインジケータ
    gActiveIndicator.show();

    //･･･の設定(タイトル)
    $(".Ellipsis").CustomLabelEx({ useEllipsis: true });

    //フッターアプリの起動設定
    SetFutterApplication();

    //タイマーセット
    commonRefreshTimer(RefreshDisplay);

    //初期表示ボタンイベント
    setTimeout(function () { PageLoad(); }, 50);

});

//初期表示イベント
function PageLoad() {

    //初期表示ボタンクリック
    $("#MainLoadingButton").click();

    //初期表示のみボタン名をセット
    gButtomName = C_MAIN_LOAD_BUTTOM;

    // UpdatePanel処理前後イベント
    $(document).ready(function () {
        var prm = Sys.WebForms.PageRequestManager.getInstance();

        // 開始時のイベント
        prm.add_beginRequest(function () {

        });

        // 終了時のイベント
        prm.add_endRequest(EndRequest);
        function EndRequest(sender, args) {

            //初期表示のイベントか確認
            if (gButtomName == "MainLoadingButton") {
                //初期表示ボタンイベント終了後

                // 2019/06/19 NSK  近藤 TKM PUAT-4066 TableNoに値を入力後、SAを振当てるとアラートメッセージが表示される START
                // フッターのクリックしたボタンの初期化 (呼出ボタン、呼出キャンセルボタン、チップ削除ボタン)
                gFooterButtonId = C_EMPTY_STRING;
                // 2019/06/19 NSK  近藤 TKM PUAT-4066 TableNoに値を入力後、SAを振当てるとアラートメッセージが表示される END

                //サーバー側よりクライアントメッセージ群を取得
                gClientMessage = JSON.parse($("#HiddenClientMessage").val());


                //2013/12/10 TMEJ 河原 次世代サービス 工程管理機能開発 START

                //受付持ちモニター使用フラグチェック
                if ($("#HiddenReceptFlag").val() === C_RECEPT_FALG) {
                    //受付待ちモニターを使用しない場合は呼出・呼出キャンセルボタンは表示しない

                    //呼出ボタン
                    $("dd.BtnCall").css({ "display": "none" });
                    //呼出キャンセルボタン
                    $("dd.BtnCancel").css({ "display": "none" });

                };

                //2013/12/10 TMEJ 河原 次世代サービス 工程管理機能開発 END

                var listCount = 0;

                if ($("#HiddenReceptionListCount").val()) {
                    //来店一覧追加行数カウント
                    listCount = Number($("#HiddenReceptionListCount").val());

                } else {
                    //来店一覧追加行数カウントがない場合
                    listCount = C_RECEPTION_LIST_MAX_ROW
                };

                //来店データの確認
                if (listCount == C_RECEPTION_LIST_MAX_ROW) {
                    //表示データ無し
                    //行の追加
                    AppendReceptionList(listCount);

                    //来店一覧の表示
                    gLeftBoxList.show();

                    //初期表示用のデザインを非表示
                    gLeftBoxListDammy.hide();
                };

                //SA一覧のフィンガースクロール設定
                $("ul.RightBoxListSet").fingerScroll();

                //･･･の設定(リスト)
                $(".Ellipsis").CustomLabelEx({ useEllipsis: true });

                //SA一覧の表示
                gRightBoxList.show();

                //フッターボタン「来店管理」(クリックイベント追加)
                $("div.BtnManageImage").bind(C_TOUCH, ReceptionManageButtonClick);

                //来店データの確認
                if (listCount != C_RECEPTION_LIST_MAX_ROW) {
                    //表示データ有り                
                    //来店一覧の表示
                    gLeftBoxList.show();

                    //イベント定義
                    AddEvent();

                    //行の追加
                    AppendReceptionList(listCount);

                    //初期表示用のデザインを非表示
                    gLeftBoxListDammy.hide();

                };

                //来店データの表示件数確認
                if (listCount < 0) {
                    //8件以上有る場合

                    //来店リストのフィンガースクロール設定
                    $("ul.LeftBoxListSet").fingerScroll();
                };

                //タイマークリア
                commonClearTimer();

                //アクティブインジケータ終了
                gActiveIndicator.hide();

                gButtomName = "";

                // 2017/03/23 NSK  竹中 TR-SVT-TMT-20170117-001 SVR画面でランダムな車両でJOBオープンする START
            } else if ($("#HiddenVehicleListDisplayType").val() == "1") {

                $(".PopUpVehicleListClass").attr("style", "");

                //車両選択一覧のスクロール設定
                $(".PopUpVehicleListContentsClass").fingerScroll();

                //車両選択一覧のイベント設定
                SetVehicleListEvent();

                $(".Ellipsis").CustomLabelEx({ useEllipsis: true });

                //全体クルクル非表示
                gActiveIndicator.hide();

                $("#VehicleListOverlayBlack").css("display", "block");

                commonClearTimer();

                // 2017/03/23 NSK  竹中 TR-SVT-TMT-20170117-001 SVR画面でランダムな車両でJOBオープンする END
            } else {
                //初期表示ボタンイベント以外のイベント後

                //アクティブインジケータ
                gActiveIndicator.show();

                // タイマーセット
                commonRefreshTimer(RefreshDisplay);

                //再描画イベント                
                setTimeout(function () { window.location.reload(); }, 50);

            };
        };
    });
};

//イベント定義
function AddEvent() {

    //受付番号エリア計算機イベントの追加
    $("div.No").NumericKeypad({ maxDigits: 3,
                                acceptDecimalPoint: false,
                                defaultValue: "",
                                completionLabel: gClientMessage.id030,
                                cancelLabel: gClientMessage.id031,
                                valueChanged: function (num) { NumericKeypadOKButtonClick(this, num); },
                                parentPopover: null,
                                open: function () {
                                    $(this).NumericKeypad("setValue", this.innerText);
                                    // 2017/03/23 NSK  竹中 TR-SVT-TMT-20170117-001 SVR画面でランダムな車両でJOBオープンする START
                                    gNumericKeypadDispFlg = "1";
                                    // 2017/03/23 NSK  竹中 TR-SVT-TMT-20170117-001 SVR画面でランダムな車両でJOBオープンする END
                                 },
                                close: function () {
                                    $(".icrop-NumericKeypad-content-TextArea").innerText = "";
                                    // 2017/03/23 NSK  竹中 TR-SVT-TMT-20170117-001 SVR画面でランダムな車両でJOBオープンする START
                                    gNumericKeypadDispFlg = "0";
                                    // 2017/03/23 NSK  竹中 TR-SVT-TMT-20170117-001 SVR画面でランダムな車両でJOBオープンする END
                                 }
    });    

    //フッターボタン「呼出」(クリックイベント追加)
    $("dd.BtnCall").bind(C_TOUCH, SubFooterButtonClick);
    //フッターボタン「呼出」(ボタンIDの追加)
    $("dd.BtnCall").attr(C_BUTTON_ID, C_CALL_BUTTON);

    //フッターボタン「呼出キャンセル」(クリックイベント追加)
    $("dd.BtnCancel").bind(C_TOUCH, SubFooterButtonClick);
    //フッターボタン「呼出キャンセル」(ボタンIDの追加)
    $("dd.BtnCancel").attr(C_BUTTON_ID, C_CALL_CANCEL_BUTTON);

    //フッターボタン「チップ削除」(クリックイベント追加)
    $("dd.BtnDelete").bind(C_TOUCH, SubFooterButtonClick);
    //フッターボタン「チップ削除」(ボタンIDの追加)
    $("dd.BtnDelete").attr(C_BUTTON_ID, C_TIP_DELETE_BUTTON);

    //チェックボックス(イベントクラス名追加)
    $("div.CheckClick").attr(C_CHECK_EVENT, C_CHECK);

    //テキストボックス「車両登録No」(ボタンIDの追加)
    $("div.No").attr(C_ITEM_ID, C_RECEIPT_NUMBER);
    //テキストボックス「車両登録No」(ボタンIDの追加)
    $("input.CarTypeTextBoxItem").attr(C_ITEM_ID, C_REGNO_TEXT);
    //テキストボックス「来店者」(ボタンIDの追加)
    $("input.VisitorTextBoxItem").attr(C_ITEM_ID, C_VISITOR_TEXT);
    //テキストボックス「電話番号」(ボタンIDの追加)
    $("input.TellNoTextBoxItem").attr(C_ITEM_ID, C_TELLNO_TEXT);
    //テキストボックス「テーブルNo」(ボタンIDの追加)
    $("input.TableNoTextBoxItem").attr(C_ITEM_ID, C_TABLENO_TEXT);

    //来店一覧チェックボックス(クリックイベント追加)
    $("div.CheckClick").bind(C_TOUCH, ReceptionCheckBox);

    //SAボタン(クリックイベント追加)
    $("div#SAButton").bind(C_TOUCH, SAButtonClick);

    //ブラックアウト(クリックイベント追加)
    $("div.BlackBack").bind(C_TOUCH, BlackBackClick);

    //SA登録ボタン「振当てボタン」(クリックイベント追加)
    $("div.RightBoxSARegisterButton").bind(C_TOUCH, RightBoxSARegisterButtonClick);

    //テキストボックス(フォーカスINイベント追加)
    $(".TextArea").bind('focusin', TextBoxFocusIn);
    //テキストボックス(フォーカスOutイベント追加)    
    $(".TextArea").bind('focusout', TextBoxFocusOut);

};

//再描画イベント
function RefreshDisplay() {

    //再描画イベント                
    window.location.reload();
};

//通知受信時再描画
function Send_Visit() {

    // 2017/03/23 NSK  竹中 TR-SVT-TMT-20170117-001 SVR画面でランダムな車両でJOBオープンする START
    //SA一覧が表示されている場合リフレッシュしない
    if ($("div.SAListClick").css('display') != 'none') {
        return;
    }

    //車両選択ポップアップが表示されている場合、リフレッシュしない
    if ($("#HiddenVehicleListDisplayType").val() == '1') {
        return;
    }

    //計算機表示中の場合、リフレッシュしない
    if (gNumericKeypadDispFlg == "1") {
        return;
    }
    // 2017/03/23 NSK  竹中 TR-SVT-TMT-20170117-001 SVR画面でランダムな車両でJOBオープンする END
    
    //アクティブインジケータ
    gActiveIndicator.show();

    //タイマーセット
    commonRefreshTimer(RefreshDisplay);

    //来店者一覧ダミー表示
    gLeftBoxListDammy.show();

    //来店者一覧非表示
    gLeftBoxList.hide();

    //SA一覧の非表示
    gRightBoxList.hide();

    //再描画イベント                
    setTimeout(function () { window.location.reload(); }, 50);
    
};

//クリアタイマー
function commonClearTimer() {

    //タイマークリア
    timerClearTime = (new Date()).getTime() - 1;
};

//来店一覧に空行の追加
function AppendReceptionList(count) {

    //足りない行数分ループ
    for (var i = 1; i <= count; i++) {

        //リストの追加
        $("ul.LeftBoxListSet #AjaxListPanelReception").append(C_APPEND_LIST);
    };
};

//来店一覧のチェックボックス処理
function ReceptionCheckBox() {

    //クラス名の取得
    var className = $(this).attr(C_CHECK_EVENT);
    //親リストの取得
    var parentList = $(this).parents("li#ReceptionList");

    //タップしたListがすでにチェックされているか確認
    if (className == C_CHECK) {
        //チェックされていない

        //すでに他にチェックがあるか検索し、チェックがあればチェックをはずす
        $("div." + C_CHECK_ON).removeClass(C_CHECK_ON).addClass(C_CHECK);
        //背景色を戻す
        $("div.WhiteBack").css({ "display": "none" });

        //チェックボックス(イベントクラス名変更チェック無し)
        $("div.CheckClick").attr(C_CHECK_EVENT, C_CHECK);        

        //背景色の変更
        parentList.find("div.WhiteBack").css({ "display": "block" });

        //フッターボタンの変更
        //2014/01/23 TMEJ 小澤 IT9611_次世代サービス 工程管理機能開発 START
        //gMainFooter.hide();
        //2014/01/23 TMEJ 小澤 IT9611_次世代サービス 工程管理機能開発 END
        gCheckFooter.show();

        //チェックを入れる
        $(this).prev("div#RecCheckBox").removeClass(C_CHECK).addClass(C_CHECK_ON);

        //チェックボックス(イベントクラス名変更チェック有り)
        $(this).attr(C_CHECK_EVENT, C_CHECK_ON);

        //チェックしたList情報をグローバル変数に格納
        gCheckBoxValue = parentList

    } else {
        //チェックされている

        //背景色を戻す
        parentList.find("div.WhiteBack").css({ "display": "none" });

        //チェックをはずす
        $(this).prev("div#RecCheckBox").removeClass(C_CHECK_ON).addClass(C_CHECK);

        //チェックボックス(イベントクラス名変更チェック無し)
        $("div.CheckClick").attr(C_CHECK_EVENT, C_CHECK);

        //フッターボタンの変更
        //2014/01/23 TMEJ 小澤 IT9611_次世代サービス 工程管理機能開発 START
        //gMainFooter.show();
        //2014/01/23 TMEJ 小澤 IT9611_次世代サービス 工程管理機能開発 END
        gCheckFooter.hide();

        //チェックしたList情報をグローバル変数からさ削除
        gCheckBoxValue = null;
    };
};

//計算機OKボタン処理
function NumericKeypadOKButtonClick(selfDiv, num) {

    //ボタンIDを取得
    var itemNo = $(selfDiv).attr(C_ITEM_ID);

    //ラベル名の取得
    var textLabelID = $(selfDiv).children()[0].id;

    //OKボタンが呼出される前の値を変数に格納

    //2013/12/26 TMEJ 河原 次世代e-CRBタブレット(サービス) iOS7.0 VersionUp対応 START

    //var beforeValue = $(selfDiv).find("#" + textLabelID)[0].textContent;    
    var beforeValue = $(selfDiv).children("#" + textLabelID)[0].textContent;

    //2013/12/26 TMEJ 河原 次世代e-CRBタブレット(サービス) iOS7.0 VersionUp対応 END

    //ボタンIDとラベル名の取得確認
    if ((!itemNo) || (!textLabelID)) {
        //取得できなかった場合

        //エラー
        alert(gClientMessage.id922);

        return;
    };

    //2013/12/26 TMEJ 河原 次世代e-CRBタブレット(サービス) iOS7.0 VersionUp対応 START

    //変更後の値を発券番号テキストに表示
    //$(selfDiv).find("#" + textLabelID)[0].textContent = num;
    $(selfDiv).children("#" + textLabelID)[0].textContent = num;

    //2013/12/26 TMEJ 河原 次世代e-CRBタブレット(サービス) iOS7.0 VersionUp対応 END
    
    //変更後の値を発券番号をグローバル変数に格納
    var afterValue = num;
   
    //登録前値チェック
    var result = RegisterCheck(beforeValue, afterValue);

    //結果確認
    if (result == C_NO_CHANGE) {
        //値に変更無しのため処理無し        

    } else if (result == C_CHANG) {
        //値に変更あり正常

        //親のリスト情報の取得
        var listInfo = $(selfDiv).parents("li#ReceptionList");

        //来店実績連番の取得
        var visitSeq = listInfo.attr(C_VISITSEQ);

        //LISTのクラス名の取得
        var listName = listInfo.attr("class");

        //更新日時(排他制御用)の取得
        var upDateTime = listInfo.attr(C_UP_DATE_DATE);

        //登録キー来店実績連番とList名の存在確認
        if (visitSeq && listName && upDateTime) {
            //存在する            

            //パラメータの作成
            var prams = CreatetParam(itemNo, visitSeq, listName, beforeValue, afterValue, upDateTime);

            //非同期更新処理
            //コールバック開始
            gCallbackSC3100401.doSC3100401Callback(prams, ReturnScript);

            //タイマーセット
            commonRefreshTimer(RefreshDisplay);

        } else {
            //存在しない

            //予期せぬエラーのためロールバック

            //元にもどす
            setTimeout(function () {

                //2013/12/26 TMEJ 河原 次世代e-CRBタブレット(サービス) iOS7.0 VersionUp対応 START

                //変更前の値にもどす
                //$(selfDiv).find("#" + textLabelID)[0].textContent = beforeValue;                
                $(selfDiv).children("#" + textLabelID)[0].textContent = beforeValue;

                //2013/12/26 TMEJ 河原 次世代e-CRBタブレット(サービス) iOS7.0 VersionUp対応 END

            }, 50); 
        };  
    };
};

//テキストボックスフォーカスInイベント
function TextBoxFocusIn() {

    //テキストボックスフォーカスINされた時の値をグローバル変数に格納
    gBeforeValue = $(this).val();

};

//テキストボックスフォーカスOutイベント
function TextBoxFocusOut() {    

    //テキストボックスフォーカスINされた時の値を変数
    var beforeValue = gBeforeValue;

    //次のイベント用に初期化
    gBeforeValue = null;

    //テキストボックスフォーカスOUTされた時の値をグローバル変数に格納
    var afterValue = $(this).val();

    //タップ要素の取得
    var selfDiv = $(this);

    //ボタンIDの取得
    var itemNo = selfDiv.attr(C_ITEM_ID);

    //登録前値チェック
    var result

    //ボタンIDの取得確認
    if (!itemNo) {
        //ボタンID取得できず

        //予期せぬエラーのためロールバック

        //元にもどす
        setTimeout(function () {

            alert(gClientMessage.id922);

            //変更前の値にもどす
            $(selfDiv).val(beforeValue);

        }, 50);

        return;

    } else {
        ////ボタンID取得成功

        //値の変更確認
        result = RegisterCheck(beforeValue, afterValue, itemNo);  

    };         

    //結果確認
    // 2018/02/19 NSK  山田 REQ-SVT-TMT-20170615-001 iPod Gatekeeperに顧客の車両登録番号を入力する機能を追加 START
    //if (result == C_NO_CHANGE) {
    //    //値に変更無しのため処理無し        
    //
    //} else if (result == C_CHANG) {

    //値が変化した、または登録番号フォーカスアウト時に登録番号値有り且つ来店者名、来店者電話番号が共に未入力場合処理を実行する
    //親リストの取得
    var parentList = $(this).parents("li#ReceptionList");
    //該当行の来店者名取得
    var customerName = parentList.find("div.VisitorEdit").children("#LeftBoxListTextBox02").val().trim();
    //該当行の来店者電話番号取得
    var customerTelNo = parentList.find("div.TellNoEdit").children("#LeftBoxListTextBox03").val().trim();
    if ((result == C_CHANG) || 
        ((itemNo == C_REGNO_TEXT) && 
         (afterValue.trim() != "") &&
         (customerName == "") && 
         (customerTelNo == ""))) {
    // 2018/02/19 NSK  山田 REQ-SVT-TMT-20170615-001 iPod Gatekeeperに顧客の車両登録番号を入力する機能を追加 END
        //値に変更あり正常

        //親のリスト情報の取得
        var listInfo = $(selfDiv).parents("li#ReceptionList");

        //来店実績連番の取得
        var visitSeq = listInfo.attr(C_VISITSEQ);

        //LISTのクラス名の取得
        var listName = listInfo.attr("class");

        //更新日時(排他制御用)の取得
        var upDateTime = listInfo.attr(C_UP_DATE_DATE);

        //登録キー来店実績連番とList名の存在確認
        if (visitSeq && listName &&  upDateTime) {
            //存在する

            //どこのテキストか確認
            if (itemNo == C_REGNO_TEXT) {
                //車両登録Noテキスト

                //スペースのトリム
                afterValue = afterValue.trim();

                //車両登録Noの変更後の値確認
                if (!afterValue) {
                    //入力されている値が存在しない

                    //元にもどす
                    setTimeout(function () {

                        alert(gClientMessage.id909);

                        //変更前の値にもどす
                        $(selfDiv).val(beforeValue);

                    }, 50);

                    return;

                };

                //アクティブインジケータ
                gActiveIndicator.show();

                // 2017/03/23 NSK  竹中 TR-SVT-TMT-20170117-001 SVR画面でランダムな車両でJOBオープンする START
                //車両登録番号変更時にコールバックは行わないよう修正

                //必要パラメータの設定
                $("#HiddenVisitSeq").val(visitSeq);          //来店実績連番
                $("#HiddenRegNo").val(afterValue);           //車両登録No
                $("#HiddenUpDateDate").val(upDateTime);      //更新日時

                //ロールバック用に値を保持
                gBeforeRegNo = beforeValue;                  //変更前の車両登録番号
                gListName = listName;                        //変更する来店情報のクラス名

                //タイマーセット
                commonRefreshTimer(RefreshDisplay);

                //POSTBACK処理(DB登録)
                setTimeout(function () { $("#RegisterRegNoButton").click(); }, 50);
                event.stopPropagation();

                return;
                // 2017/03/23 NSK  竹中 TR-SVT-TMT-20170117-001 SVR画面でランダムな車両でJOBオープンする END 

            } else {
                //上記以外

                //スペースのトリム
                afterValue = afterValue.trim();

            };

            //パラメータの作成
            var prams = CreatetParam(itemNo, visitSeq, listName, beforeValue, afterValue, upDateTime);

            //非同期更新処理
            //コールバック開始
            gCallbackSC3100401.doSC3100401Callback(prams, ReturnScript);

            //タイマーセット
            commonRefreshTimer(RefreshDisplay);

        } else {
            //存在しない

            //予期せぬエラーのためロールバック

            //元にもどす
            setTimeout(function () {

                alert(gClientMessage.id922);

                //変更前の値にもどす
                $(selfDiv).val(beforeValue);

            }, 50);
        };
    };
};

//非同期登録前チェック
function RegisterCheck(bifValue, aftValue, itemNo) {

    //値が変更されたか確認
    if (bifValue == aftValue) {
        //変更されていない


        //更新登録しない
        return C_NO_CHANGE;

    } else {
        //変更された

        //更新登録する
        return C_CHANG;
    };

};

//コールバックでサーバーに渡すパラメータを作成する
function CreatetParam(caller, visitSeq, className, bifValue, aftValue, upDateTime) {

    //コールバック用パラメータの作成
    var rtnVal = {
          Caller: caller            //呼出元
        , ClassName: className      //クラス名
        , VisitSeq: visitSeq        //来店実績連番      
        , BeforeValue: bifValue     //変更前の値
        , AfterValue: aftValue      //変更後の値
        , UpDateDate: upDateTime    //更新日時
    };

    return rtnVal;
};

//コールバック終了メソッド
function ReturnScript(result) {
    // 2019/06/19 NSK  近藤 TKM PUAT-4066 TableNoに値を入力後、SAを振当てるとアラートメッセージが表示される START
    // コールバック実行中のフラグをOffにする
    gCallbackFunctionRunFlg = false;
    // 2019/06/19 NSK  近藤 TKM PUAT-4066 TableNoに値を入力後、SAを振当てるとアラートメッセージが表示される END
 
    //タイマーをクリア
    commonClearTimer();

    //サーバー側より返却値を取得
    var jsonResult = JSON.parse(result);

    //サーバー側からの値取得結果
    if (!jsonResult) {
        //取得失敗

        alert(gClientMessage.id922);

        return;
    };

    //処理結果確認
    if (jsonResult.ResultCode == C_RESULT_SUCCESS) {
        //コールバック登録処理成功

        //呼出元の確認
        if (jsonResult.Caller == C_REGNO_TEXT) {
            //車両登録No

            // 2017/03/23 NSK  竹中 TR-SVT-TMT-20170117-001 SVR画面でランダムな車両でJOBオープンする START
            ////必要パラメータの設定
            //$("#HiddenVisitSeq").val(jsonResult.VisitSeq);          //来店実績連番
            //$("#HiddenRegNo").val(jsonResult.AfterValue);           //車両登録No
            //$("#HiddenUpDateDate").val(jsonResult.UpDateDate);      //更新日時

            ////チェック成功後POSTBACK処理(DB登録)
            //setTimeout(function () { $("#RegisterRegNoButton").click(); }, 50);

            return;
            // 2017/03/23 NSK  竹中 TR-SVT-TMT-20170117-001 SVR画面でランダムな車両でJOBオープンする START

        } else {
            //上記以外

            //更新日時を最新
            $("li." + jsonResult.ClassName).attr(C_UP_DATE_DATE, jsonResult.UpDateDate);

            // 2019/06/14 NSK  近藤 TKM PUAT-4066 TableNoに値を入力後、SAを振当てるとアラートメッセージが表示される START
            // SA一覧の表示をしている場合
            // かつ、来店実績連番がSAの選択した行と一致した場合 (排他制御を行う行と一致した場合)
            if ($(".RightPopBox").css("display") == "block" && gSAAssignValue.VisitSeq == jsonResult.VisitSeq) {

                // SA振当の更新用日時を更新
                gSAAssignValue.UpDateDate = jsonResult.UpDateDate;
            }

            // フッターの表示をしている場合 (呼出ボタン、呼出キャンセルボタン、チップ削除ボタンを表示している場合)
            // かつ、来店実績連番がチェックした行と一致した場合 (排他制御を行う行と一致した場合)
            if ($(".CheckFooter").css("display") == "block" && gCheckBoxValue.attr(C_VISITSEQ) == jsonResult.VisitSeq) {

                // 来店一覧のチェックボックスの更新用日時を更新
                gCheckBoxValue.attr(C_UP_DATE_DATE, jsonResult.UpDateDate);
            }

            // フッターのボタン (呼出ボタン・呼出キャンセルボタン・チップ削除ボタン) をクリックした場合はイベントを発火
            if (gFooterButtonId !== C_EMPTY_STRING) {

                CallFooterButtonClickEvent(gFooterButtonId);

                // フッターの選択したボタンIDを初期化する
                gFooterButtonId = C_EMPTY_STRING;
            }
            // 2019/06/14 NSK  近藤 TKM PUAT-4066 TableNoに値を入力後、SAを振当てるとアラートメッセージが表示される END
        };

    } else {
        //コールバック処理失敗

        //呼出元で処理分岐
        switch (jsonResult.Caller) {
            case C_RECEIPT_NUMBER:
                //受付番号計算機

                //エラーメッセージの表示
                alert(jsonResult.Message);

                //2013/12/26 TMEJ 河原 次世代e-CRBタブレット(サービス) iOS7.0 VersionUp対応 START

                //変更する前の値に戻す
                //$("li." + jsonResult.ClassName).find("#LeftBoxListLabel02")[0].textContent = jsonResult.BeforeValue;                
                $("li." + jsonResult.ClassName).find("div.No").children("#LeftBoxListLabel02")[0].textContent = jsonResult.BeforeValue;

                //2013/12/26 TMEJ 河原 次世代e-CRBタブレット(サービス) iOS7.0 VersionUp対応 END

                break;

                // 2017/03/23 NSK  竹中 TR-SVT-TMT-20170117-001 SVR画面でランダムな車両でJOBオープンする START
                /*
            case C_REGNO_TEXT:
                //車両登録No

                //エラーメッセージの表示
                alert(jsonResult.Message);

                //アクティブインジケータ終了
                gActiveIndicator.hide();

                //2013/12/26 TMEJ 河原 次世代e-CRBタブレット(サービス) iOS7.0 VersionUp対応 START

                //変更する前の値に戻す
                //$("li." + jsonResult.ClassName).find("#LeftBoxListTextBox01").val(jsonResult.BeforeValue);
                $("li." + jsonResult.ClassName).find("div.CarTypeText").children("#LeftBoxListTextBox01").val(jsonResult.BeforeValue);

                //2013/12/26 TMEJ 河原 次世代e-CRBタブレット(サービス) iOS7.0 VersionUp対応 END

                break;
                */
                // 2017/03/23 NSK  竹中 TR-SVT-TMT-20170117-001 SVR画面でランダムな車両でJOBオープンする END

            case C_VISITOR_TEXT:
                //来店者テキスト

                //エラーメッセージの表示
                alert(jsonResult.Message);

                //2013/12/26 TMEJ 河原 次世代e-CRBタブレット(サービス) iOS7.0 VersionUp対応 START

                //変更する前の値に戻す
                //$("li." + jsonResult.ClassName).find("#LeftBoxListTextBox02").val(jsonResult.BeforeValue);
                $("li." + jsonResult.ClassName).find("div.VisitorEdit").children("#LeftBoxListTextBox02").val(jsonResult.BeforeValue);

                //2013/12/26 TMEJ 河原 次世代e-CRBタブレット(サービス) iOS7.0 VersionUp対応 END

                break;

            case C_TELLNO_TEXT:
                //電話番号テキスト

                //エラーメッセージの表示
                alert(jsonResult.Message);

                //2013/12/26 TMEJ 河原 次世代e-CRBタブレット(サービス) iOS7.0 VersionUp対応 START

                //変更する前の値に戻す
                //$("li." + jsonResult.ClassName).find("#LeftBoxListTextBox03").val(jsonResult.BeforeValue);
                $("li." + jsonResult.ClassName).find("div.TellNoEdit").children("#LeftBoxListTextBox03").val(jsonResult.BeforeValue);

                //2013/12/26 TMEJ 河原 次世代e-CRBタブレット(サービス) iOS7.0 VersionUp対応 END

                break;

            case C_TABLENO_TEXT:
                //テーブルNoテキスト

                //エラーメッセージの表示
                alert(jsonResult.Message);

                //2013/12/26 TMEJ 河原 次世代e-CRBタブレット(サービス) iOS7.0 VersionUp対応 START

                //変更する前の値に戻す
                //$("li." + jsonResult.ClassName).find("#LeftBoxListTextBox04").val(jsonResult.BeforeValue);
                $("li." + jsonResult.ClassName).find("div.TableNo").children("#LeftBoxListTextBox04").val(jsonResult.BeforeValue);

                //2013/12/26 TMEJ 河原 次世代e-CRBタブレット(サービス) iOS7.0 VersionUp対応 END

                break;

            default:

                //エラーメッセージの表示
                alert(jsonResult.Message);

                break;
        };

    };
    
};

//(来店一覧エリア)SA選択ボタンクリック処理
function SAButtonClick(e) {
    
    //選択されたSAのポジションの取得
    var currentHeight = $(this).offset().top - ($(this).height() / 2) - 56;

    //ボタンを選択状態にする
    $(this).addClass(C_INACTIVE);

    //SA一覧以外のグレーアウト表示
    gBlackBack.show();

    //ボックスシャドウ表示
    gRightPopBox.show();

    //SAリストイベント用Divの表示
    gSAListClick.show();

    //SA振当て登録ボタンの表示
    gRightBoxSARegisterButton.show();

    //吹きだしの▲表示
    $("div.PopBoxArrow").css({ "display": "block", "top": currentHeight });
    $("div.PopBoxArrow_shadow").css({ "display": "block", "top": currentHeight });

    //SA一覧のチェックボックス活性
    $("div.SACheck").removeClass(C_SA_CHECK).addClass(C_CHECKFREE);

    //選択したSACodeの取得
    var saAccount = $(this).attr(C_SA_CODE);

    //親のリスト情報の取得
    var listInfo = $(this).parents("li#ReceptionList");

    //来店実績連番の取得
    var visitSeq = listInfo.attr(C_VISITSEQ);

    //更新日時(排他制御用)の取得
    var upDateTime = listInfo.attr(C_UP_DATE_DATE);

    //選択した行の振当てステータスを取得
    var assignStatus = $(this).attr(C_ASSIGN_STATUS);

    //SAリスト(クリックイベント追加)
    $("div.SAListClick").bind(C_TOUCH, SAListClick);

    //取得確認
    if (visitSeq && upDateTime && assignStatus) {
        //取得成功

        //グローバル変数に来店実績連番を保存
        gSAAssignValue.VisitSeq = visitSeq;

        //グローバル変数に振当てステータスを保存
        gSAAssignValue.AssignStatus = assignStatus;        

        //グローバル変数に更新日時を保存
        gSAAssignValue.UpDateDate = upDateTime;


        //振当てステータスの確認
        if (assignStatus == C_ASSIGN_FIN) {
            //振当済み

            //SAAccountの取得確認
            if (!saAccount) {
                //取得失敗

                //エラー
                alert(gClientMessage.id922);

                //元に戻す
                BlackBackClick();

                return;
            };

            //グローバル変数に変更前のSAを保存
            gSAAssignValue.BeforeAccount = saAccount;

            //SA一覧から振当てされているSAを検索
            $("div.SAListClick").each(function (i) {

                if ($($("div.SAListClick")[i]).attr(C_ACCOUNT) == saAccount) {
                    //検索結果SAが存在していたら背景を青色・チェックボックスにチェック
                    $("div.SAListClick")[i].nextElementSibling.className = C_CASSETTE_CCHECK;

                    //グローバル変数に変更後SAを保存
                    gSAAssignValue.AfterAccount = saAccount; 

                };
            });
        };        

    } else {
        //取得失敗

        //エラー
        alert(gClientMessage.id922);

        //元に戻す
        BlackBackClick();
    };
};

//グレーアウトのクリックイベント
function BlackBackClick() {
    
    //ブラックアウト削除
    gBlackBack.hide();

    //ボックスシャドウ削除
    gRightPopBox.hide();

    //SAリストイベント用Divの削除
    gSAListClick.hide();

    //SA振当て登録ボタン削除
    gRightBoxSARegisterButton.hide();

    //SAボタンの選択状態解除
    $("div.Inactive").removeClass(C_INACTIVE)

    //SAリストの背景色を全て下にもどす
    $("ul.Cassette.C_Check").removeClass(C_CASSETTE_CCHECK).addClass(C_CASSETTE);

    //SA一覧チェックボックスの非活性
    $("div.CheckFree").removeClass(C_CHECKFREE).addClass(C_SA_CHECK);

    //SAリスト(クリックイベント削除)
    $("div.SAListClick").unbind(C_TOUCH, SAListClick);

    //グローバル変数の初期化
    gSAAssignValue.VisitSeq = null;
    gSAAssignValue.AssignStatus = null;
    gSAAssignValue.BeforeAccount = null;
    gSAAssignValue.AfterAccount = null;
    gSAAssignValue.UpDateDate = null;

};

//(SA一覧エリア)
//SAリストクリックイベント
function SAListClick() {

    //現在選択中のリスト情報の取得
    var bifCheckDiv = $(C_UL_CASSETTE_CCHECK)

    //選択しているリストを再度選択した場合
    if ($(this).next().attr("class") == C_CASSETTE_CCHECK) {

        //グローバル変数を初期化
        gSAAssignValue.AfterAccount = null;

        //既選択解除
        bifCheckDiv.removeClass(C_CASSETTE_CCHECK).addClass(C_CASSETTE);

        return;
    };

    //既に選択されているのもがあれば選択解除
    bifCheckDiv.removeClass(C_CASSETTE_CCHECK).addClass(C_CASSETTE);

    //タップしたSAリストの背景色・チェックボックスの設定
    $(this).next("ul.Cassette").removeClass(C_CASSETTE).addClass(C_CASSETTE_CCHECK);

    //選択したSAAccountを取得
    var saAccount = $(this).attr(C_ACCOUNT);

    //選択したSAAccountの取得確認
    if (saAccount) {
        //取得成功

        //グローバル変数に変更後SAAccountを格納
        gSAAssignValue.AfterAccount = saAccount;

    }else{
        //取得失敗

        //現在のDivを格納
        var thisDiv = $(this);

        //エラー表示
        setTimeout(function () {
            //エラー
            alert(gClientMessage.id922);

            //元の選択状態に戻す
            bifCheckDiv.removeClass(C_CASSETTE).addClass(C_CASSETTE_CCHECK);
            thisDiv.next("ul.Cassette").removeClass(C_CASSETTE_CCHECK).addClass(C_CASSETTE);

        }, 50);
    };
};

//SA振当て登録ボタンイベント
function RightBoxSARegisterButtonClick() {

    //振当ステータスで処理の分岐
    switch (gSAAssignValue.AssignStatus) {
        case C_NO_ASSIGN:
        case C_ASSIGN_WAIT:
            //SA振当前

            //SA振当登録処理
            SARegisterAssign();

            break;
        case C_ASSIGN_FIN:
            //SA振当済

            //SA登録変更・Undo処理
            ChangeSARegister();

            break;
        default:
            //上記以外のエラー

            alert(gClientMessage.id922);
            break;
    };

};

//SA振当登録処理
function SARegisterAssign() {

    //SA確認
    if (!gSAAssignValue.AfterAccount) {
        //SA選択無し

        //処理無し
        alert(gClientMessage.id913);

        return;

    };

    //振当処理
    gSAAssignValue.EventKeyID = C_SA_ASSIGN

    //登録ポストバック処理
    SARegisterPostBack();

};

//SA変更・Undo処理
function ChangeSARegister() {

    //表示メッセージ
    var messageWord;

    //処理確認
    if (!gSAAssignValue.AfterAccount) {
        //Undo処理

        //SA解除処理
        gSAAssignValue.EventKeyID = C_SA_UNDO

        //表示メッセージ格納
        messageWord = gClientMessage.id906;

    } else if (gSAAssignValue.BeforeAccount != gSAAssignValue.AfterAccount) {
        //担当SA変更

        //SA変更処理
        gSAAssignValue.EventKeyID = C_SA_CHANGE

        //表示メッセージ格納
        messageWord = gClientMessage.id905;

    } else {
        //上記以外

        //処理無し
        alert(gClientMessage.id923);

       return;

    };

    //メッセージボックスの表示
   if (window.confirm(messageWord)) {
       //OKボタン押下

       //登録ポストバック処理
       SARegisterPostBack();

   };

};

//SA登録ボタン PostBack処理
function SARegisterPostBack() {

    //必要パラメータの設定
    $("#HiddenVisitSeq").val(gSAAssignValue.VisitSeq);          //来店実績連番
    $("#HiddenSAAccount").val(gSAAssignValue.AfterAccount);     //SAアカウント
    $("#HiddenUpDateDate").val(gSAAssignValue.UpDateDate);      //更新日時
    $("#HiddenEventKeyID").val(gSAAssignValue.EventKeyID);      //イベントID

    //アクティブインジケータ
    gActiveIndicator.show();

    //タイマーセット
    commonRefreshTimer(RefreshDisplay);

    //初期表示ボタンイベント
    setTimeout(function () { $("#RegisterAssignButton").click(); }, 50);

};

//フッター
//フッターボタン「メインメニュー」イベント
function FooterButtonControlMainMenu() {

    //アクティブインジケータ
    gActiveIndicator.show();

    //タイマーセット
    commonRefreshTimer(RefreshDisplay);

    //来店者一覧ダミー表示
    gLeftBoxListDammy.show();

    //来店者一覧非表示
    gLeftBoxList.hide();

    //SA一覧の非表示
    gRightBoxList.hide();

    //再描画イベント                
    setTimeout(function () { window.location.reload(); }, 50);

};

//フッターボタン「来店管理」イベント
function ReceptionManageButtonClick() {

       
    //タイマーセット
    commonRefreshTimer(RefreshDisplay);

    //フッターボタンアクティブ
    gMainFooterActive.on();

    //フッターボタンを600ミリ秒後に元にもどす 
    setTimeout(function () {
        //アクティブインジケータ
        gActiveIndicator.show();

        //来店管理画面遷移ボタンクリック
        $("#VisitManageButton").click();

        //フッターボタンオフ
        gMainFooterActive.off();

        //来店者一覧ダミー表示
        gLeftBoxListDammy.show();

        //来店者一覧非表示
        gLeftBoxList.hide();

        //SA一覧の非表示
        gRightBoxList.hide();
        
        }, 300);
};

//2014/01/23 TMEJ 小澤 IT9611_次世代サービス 工程管理機能開発 START
//フッターボタンイベント
function FooterButtonClick(buttonId) {

    //アクティブインジケータ
    gActiveIndicator.show();

    //タイマーセット
    commonRefreshTimer(RefreshDisplay);

    //来店者一覧ダミー表示
    gLeftBoxListDammy.show();

    //来店者一覧非表示
    gLeftBoxList.hide();

    //SA一覧の非表示
    gRightBoxList.hide();

    //各イベント処理実行
    switch (buttonId) {
        case 400:
            //予約管理ボタン
            setTimeout(function () { $("#ReserveManagementButton").click(); }, 50);
            break;
        case 500:
            //RO一覧ボタン
            setTimeout(function () { $("#RepairOrderListButton").click(); }, 50);
            break;
        case 1000:
            //全体管理ボタン
            setTimeout(function () { $("#WholeManagementButton").click(); }, 50);
            break;
    }

    return false;

};
//2014/01/23 TMEJ 小澤 IT9611_次世代サービス 工程管理機能開発 END

//フッターボタン「スケジュール」イベント
function SetFutterApplication() {

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

//サブフッターボタンクリックイベント
function SubFooterButtonClick() {

    var dataString;

    //フッターサブボタンのIDを取得
    var subButtonID = $(this).attr(C_BUTTON_ID);

    //必要な値とボタンIDの確認
    if ((gCheckBoxValue) && (subButtonID)) {
        //値あり

        // 2019/06/14 NSK  近藤 TKM PUAT-4066 TableNoに値を入力後、SAを振当てるとアラートメッセージが表示される START
        //ボタンIDで処理の分岐
        //switch (subButtonID) {
        //    case C_CALL_BUTTON:
        //        //呼出ボタン
        //        CallButton();
        //        break;
        //    case C_CALL_CANCEL_BUTTON:
        //        //呼出キャンセルボタン
        //        CallCancelButton();
        //        break;
        //    case C_TIP_DELETE_BUTTON:
        //        //チップ削除ボタン
        //        TipDelete();
        //        break;
        //    default:
        //        alert(gClientMessage.id922);
        //        break;
        //};

        // CallBack (DB更新) が動いている場合は、クリックイベントを発火させずCallBackの最後に処理する。
        if (gCallbackFunctionRunFlg) {
            // クリックしたフッターのボタンIDを格納
            gFooterButtonId = subButtonID;
        } else {
            // フッターボタンのクリックイベントを発火
            CallFooterButtonClickEvent(subButtonID);
        }
        // 2019/06/14 NSK  近藤 TKM PUAT-4066 TableNoに値を入力後、SAを振当てるとアラートメッセージが表示される END

    } else {
        //値なし
        alert(gClientMessage.id922);
    };
};

// 2019/06/14 NSK  近藤 TKM PUAT-4066 TableNoに値を入力後、SAを振当てるとアラートメッセージが表示される START
/**
 * フッターボタンのクリックを行うイベント
 * @param {String} subButtonID クリックしたボタンのID
 * @return {-} -
 */
function CallFooterButtonClickEvent(subButtonId) {

    //ボタンIDで処理の分岐
    switch (subButtonId) {
        case C_CALL_BUTTON:
            //呼出ボタン
            CallButton();
            break;
        case C_CALL_CANCEL_BUTTON:
            //呼出キャンセルボタン
            CallCancelButton();
            break;
        case C_TIP_DELETE_BUTTON:
            //チップ削除ボタン
            TipDelete();
            break;
        default:
            alert(gClientMessage.id922);
            break;
    };
}
// 2019/06/14 NSK  近藤 TKM PUAT-4066 TableNoに値を入力後、SAを振当てるとアラートメッセージが表示される END

//フッターボタン「呼出」処理
function CallButton() {

    //呼出ステータスの取得
    var callStatus = gCheckBoxValue.attr(C_CALL_STATUS);

    //呼出ステータスが取得できなかった場合処理終了
    if (!callStatus) { alert(gClientMessage.id922); return; };

    //呼出中か確認
    if (C_CALLING == callStatus) { alert(gClientMessage.id917); return; };

    //2013/12/26 TMEJ 河原 次世代e-CRBタブレット(サービス) iOS7.0 VersionUp対応 START

    //振当てステータスの取得
    //var assignStatus = gCheckBoxValue.find("div#SAButton").attr(C_ASSIGN_STATUS);
    var assignStatus = gCheckBoxValue.find("div.ClassSAButton").attr(C_ASSIGN_STATUS);

    //2013/12/26 TMEJ 河原 次世代e-CRBタブレット(サービス) iOS7.0 VersionUp対応 END

    //振当てステータスが取得できなかった場合処理終了
    if (!assignStatus) { alert(gClientMessage.id922); return; };

    //SA振当済か確認
    if (C_ASSIGN_FIN == assignStatus) {
        //SA振当済み

        //呼出可能

        //2013/12/26 TMEJ 河原 次世代e-CRBタブレット(サービス) iOS7.0 VersionUp対応 START

        //受付番号の取得
        //var receiptNumber = gCheckBoxValue.find("#LeftBoxListLabel02")[0].textContent;
        var receiptNumber = gCheckBoxValue.find("div.No").children("#LeftBoxListLabel02")[0].textContent;

        //2013/12/26 TMEJ 河原 次世代e-CRBタブレット(サービス) iOS7.0 VersionUp対応 END

        //受付番号が存在しないなかった場合処理終了
        if (!receiptNumber) { alert(gClientMessage.id907); return; };

        //2013/12/26 TMEJ 河原 次世代e-CRBタブレット(サービス) iOS7.0 VersionUp対応 START

        //テーブルNoの取得
        //var tableNumber = gCheckBoxValue.find("#LeftBoxListTextBox04").val();        
        var tableNumber = gCheckBoxValue.find("div.TableNo").children("#LeftBoxListTextBox04").val();

        //2013/12/26 TMEJ 河原 次世代e-CRBタブレット(サービス) iOS7.0 VersionUp対応 END

        //トリム
        tableNumber = tableNumber.trim();

        //テーブルNoが存在しないなかった場合処理終了
        if (!tableNumber) { alert(gClientMessage.id911); return; };

        //来店実績連番の取得
        var visitSeq = gCheckBoxValue.attr(C_VISITSEQ);

        //2014/01/23 TMEJ 小澤 IT9611_次世代サービス 工程管理機能開発 START
        //予約IDの取得
        var reserveId = gCheckBoxValue.attr(C_RESERVEID);
        //2014/01/23 TMEJ 小澤 IT9611_次世代サービス 工程管理機能開発 END

        //更新日時の取得
        var upDateTime = gCheckBoxValue.attr(C_UP_DATE_DATE);

        //来店実績連番・更新日時が取得できなかった場合処理終了
        if ((!visitSeq) || (!upDateTime)) { alert(gClientMessage.id922); return; };

        //メッセージボックスの表示
        if (window.confirm(gClientMessage.id914)) {

            ///ポストバック処理
            //2014/01/23 TMEJ 小澤 IT9611_次世代サービス 工程管理機能開発 START
            SubFooterPostBack(visitSeq, reserveId, upDateTime, C_CALL_BUTTON);
            //2014/01/23 TMEJ 小澤 IT9611_次世代サービス 工程管理機能開発 END

        };

    } else {
        //SA未振当

        alert(gClientMessage.id913);

    };
};

//フッターボタン「呼出キャンセル」処理
function CallCancelButton() {

    //呼出ステータスの取得
    var callStatus = gCheckBoxValue.attr(C_CALL_STATUS);

    //呼出ステータスが取得できなかった場合処理終了
    if (!callStatus) { alert(gClientMessage.id922); return; };

    //呼出中か確認
    if (C_CALLING == callStatus) {
        //呼出中

        //来店実績連番の取得
        var visitSeq = gCheckBoxValue.attr(C_VISITSEQ);

        //2014/01/23 TMEJ 小澤 IT9611_次世代サービス 工程管理機能開発 START
        //予約IDの取得
        var reserveId = gCheckBoxValue.attr(C_RESERVEID);
        //2014/01/23 TMEJ 小澤 IT9611_次世代サービス 工程管理機能開発 END

        //更新日時の取得
        var upDateTime = gCheckBoxValue.attr(C_UP_DATE_DATE);

        //来店実績連番・更新日時が取得できなかった場合処理終了
        if ((!visitSeq) || (!upDateTime)) { alert(gClientMessage.id922); return; };

        //メッセージボックスの表示
        if (window.confirm(gClientMessage.id915)) {

            ///ポストバック処理
            //2014/01/23 TMEJ 小澤 IT9611_次世代サービス 工程管理機能開発 START
            //SubFooterPostBack(visitSeq, upDateTime, C_CALL_CANCEL_BUTTON);
            SubFooterPostBack(visitSeq, reserveId, upDateTime, C_CALL_CANCEL_BUTTON);
            //2014/01/23 TMEJ 小澤 IT9611_次世代サービス 工程管理機能開発 END

        };

    } else {
        //上記以外

        alert(gClientMessage.id919);

    };
};

//フッターボタン「チップ削除」イベント処理
function TipDelete() {

    //呼出ステータスの取得
    var callStatus = gCheckBoxValue.attr(C_CALL_STATUS);

    //呼出ステータスが取得できなかった場合処理終了
    if (!callStatus) { alert(gClientMessage.id922); return; };

    //呼出中の確認
    if (C_CALLING == callStatus) {
        //呼出中

        alert(gClientMessage.id924);

    } else {
        //上記以外

        //2013/12/26 TMEJ 河原 次世代e-CRBタブレット(サービス) iOS7.0 VersionUp対応 START

        //振当てステータスの取得        
        var assignStatus = gCheckBoxValue.find("div.ClassSAButton").attr(C_ASSIGN_STATUS);

        //2013/12/26 TMEJ 河原 次世代e-CRBタブレット(サービス) iOS7.0 VersionUp対応 END

        //振当てステータスが取得できなかった場合処理終了
        if (!assignStatus) { alert(gClientMessage.id922); return; };

        //SA振当済か確認
        if (C_ASSIGN_FIN == assignStatus) {
            //SA振当済み

            alert(gClientMessage.id924);

        } else {
            //上記以外
            //削除可能

            //来店実績連番の取得
            var visitSeq = gCheckBoxValue.attr(C_VISITSEQ);

            //2014/01/23 TMEJ 小澤 IT9611_次世代サービス 工程管理機能開発 START
            //予約IDの取得
            var reserveId = gCheckBoxValue.attr(C_RESERVEID);
            //2014/01/23 TMEJ 小澤 IT9611_次世代サービス 工程管理機能開発 END

            //更新日時の取得
            var upDateTime = gCheckBoxValue.attr(C_UP_DATE_DATE);

            //来店実績連番・更新日時が取得できなかった場合処理終了
            if ((!visitSeq) || (!upDateTime)) { alert(gClientMessage.id922); return; };

            //メッセージボックスの表示
            if (window.confirm(gClientMessage.id916)) {

                //ポストバック処理
                //2014/01/23 TMEJ 小澤 IT9611_次世代サービス 工程管理機能開発 START
                //SubFooterPostBack(visitSeq, upDateTime, C_TIP_DELETE_BUTTON);
                SubFooterPostBack(visitSeq, reserveId, upDateTime, C_TIP_DELETE_BUTTON);
                //2014/01/23 TMEJ 小澤 IT9611_次世代サービス 工程管理機能開発 END

            };
        };
    };
};

//サブフッターボタン PostBack処理
function SubFooterPostBack(visitSeq, reserveId, upDateTime, buttonId) {
    //2014/01/23 TMEJ 小澤 IT9611_次世代サービス 工程管理機能開発 START
    //function SubFooterPostBack(visitSeq, upDateTime, buttonId) {
    //2014/01/23 TMEJ 小澤 IT9611_次世代サービス 工程管理機能開発 END

    //必要パラメータの設定
    $("#HiddenVisitSeq").val(visitSeq);           //来店実績連番
    //2014/01/23 TMEJ 小澤 IT9611_次世代サービス 工程管理機能開発 START
    $("#HiddenReserveId").val(reserveId);         //予約ID
    //2014/01/23 TMEJ 小澤 IT9611_次世代サービス 工程管理機能開発 END
    $("#HiddenUpDateDate").val(upDateTime);       //更新日時
    $("#HiddenEventKeyID").val(buttonId);     　　//イベントID

    //アクティブインジケータ
    gActiveIndicator.show();

    //タイマーセット
    commonRefreshTimer(RefreshDisplay);

    //PostBack用ボタンクリック
    setTimeout(function () { $("#CustomFooterButton").click(); }, 50);
};

// 2017/03/23 NSK  竹中 TR-SVT-TMT-20170117-001 SVR画面でランダムな車両でJOBオープンする START

//車両選択一覧イベント設定.
function SetVehicleListEvent() {

    //ROレコードタップ時のイベント設定
    $('.VehicleListItemClass').bind(C_TOUCH, function (event) {
        //背景点灯
        var selectedRow = $(this);
        selectedRow.addClass("icrop-pressed");

        //アクティブインジケータ
        gActiveIndicator.show();

        //選択したデータを保持
        $("#HiddenSelectCstId").val($(this).attr("CSTID"));
        $("#HiddenSelectVclId").val($(this).attr("VCLID"));
        $("#HiddenSelectRezId").val($(this).attr("REZID"));

        //ボタン背景を戻して登録処理をする
        setTimeout(function (event) {
            //背景色を戻す
            selectedRow.removeClass("icrop-pressed");

            //タイマーセット
            commonRefreshTimer(RefreshDisplay);

            //イベント実行
            $("#PopupVehicleListEventButton").click();

            //車両選択一覧を非表示にする
            $(".PopUpVehicleListClass").attr("style", "display:none");
            $("#VehicleListOverlayBlack").css("display", "none");

            //車両選択一覧ポップアップフラグを0に設定
            $("#HiddenVehicleListDisplayType").val("0");

            //グローバル変数の初期化
            gBeforeRegNo = null;
            gListName = null;

        }, 50);

        event.stopPropagation();
    });

    // ポップアップ外押下時のロールバックイベント設定
    $('#VehicleListOverlayBlack').bind(C_TOUCH, function (event) {

        //車両選択一覧を非表示にする
        $(".PopUpVehicleListClass").attr("style", "display:none");
        $("#VehicleListOverlayBlack").css("display", "none");

        //車両選択一覧ポップアップフラグを0に設定
        $("#HiddenVehicleListDisplayType").val("0");

        //変更する前の値に戻す
        $("li." + gListName).find("div.CarTypeText").children("#LeftBoxListTextBox01").val(gBeforeRegNo);
        
        //グローバル変数の初期化
        gBeforeRegNo = null;
        gListName = null;

    });
}

//新規顧客登録
function RegistNewCustomer() {

    //ボタン背景点灯
    $("#PopUpVehicleListFooterButton").attr("class", "PopUpVehicleListFooterButtonOn");

    //アクティブインジケータ
    gActiveIndicator.show();

    //データを初期化
    $("#HiddenSelectCstId").val("");
    $("#HiddenSelectVclId").val("");
    $("#HiddenSelectRezId").val("");

    //ボタン背景を戻して登録処理をする
    setTimeout(function (event) {
        //ボタン背景色を戻す
        $("#PopUpVehicleListFooterButton").attr("class", "PopUpVehicleListFooterButtonOff");

        //タイマーセット
        commonRefreshTimer(RefreshDisplay);

        //イベント実行
        $("#PopupVehicleListEventButton").click();

        //車両選択一覧を非表示にする
        $(".PopUpVehicleListClass").attr("style", "display:none");
        $("#VehicleListOverlayBlack").css("display", "none");

        //車両選択一覧ポップアップフラグを0に設定
        $("#HiddenVehicleListDisplayType").val("0");

        //グローバル変数の初期化
        gBeforeRegNo = null;
        gListName = null;

    }, 50);

    return false;
}

// 2017/03/23 NSK  竹中 TR-SVT-TMT-20170117-001 SVR画面でランダムな車両でJOBオープンする END
