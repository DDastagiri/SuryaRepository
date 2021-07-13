//SC3150101.Main.js
//------------------------------------------------------------------------------
//機能：メインメニュー（TC）_javascript
//更新：12/08/09 TMEJ 小澤   【SERVICE_2】矢印アイコン制御追加 START
//更新：12/11/05 TMEJ 彭健   問連修正(GMTC121029047)、ROステータス切り離し対応(TC作業開始ボタン制御変更)
//更新：12/11/14 TMEJ 彭健   アクティブインジゲータ対応（クルクルのタイムアウト対応）、サイズ削減の為に古い履歴を削除
//更新：12/11/30 TMEJ 小澤【SERVICE_2】SAストール予約受付機能開発（仕分けNo.75）
//更新：12/02/21 TMEJ 成澤   【A.STEP1】TC着工指示オペレーション確立に向けた評価アプリ作成(TCステータスモニターへの遷移機能）
//更新：13/06/17 TMEJ 成澤 【開発】IT9513_【A.STEP2】次世代e-CRBタブレット　新DB適応に向けた機能設計　
//更新：13/08/08 TMEJ 成澤 【A.STEP2】タブレット版SMB開発に向けた要件定義
//更新：13/11/12 TMEJ 小澤 【A.STEP2】次世代e-CRBサービスタブレット 新DB適応に向けた機能開発
//更新：2013/11/26 TMEJ 成澤【IT9573】次世代e-CRBサービス 店舗展開に向けた標準作業確立
//更新：2013/12/12 TMEJ 成澤 【開発】IT9611_次世代サービス 工程管理機能開発
//更新：2014/06/11 TMEJ 明瀬 【TMT_IT2】チップ削除後の更新でスクリプトエラー発生の対応
//更新：2014/07/23 TMEJ 成澤 【開発】IT9711_タブレットSMB Job Dispatch機能開発
//更新：2014/08/29 TMEJ 成澤 【開発】IT9737_NextSTEPサービス ロケ管理の効率化に向けた評価用アプリ作成
//更新：2014/09/12 TMEJ 成澤  自主研追加対応_ROプレビュー遷移
//更新：2014/12/05 TMEJ 岡田　IT9857_DMS連携版サービスタブレット JobDispatch完成検査入力制御開発
//更新：2016/03/30 NSK  小牟禮 アクティビティインジケータが消えない問題
//更新：2019/06/03 NSK 鈴木 18PRJ03142_(FS)業務検証:セールス受注前活動活動結果登録オペレーション[TKM]PUAT-4109 部品詳細にて、バックオーダー(B/O)が表示されない
//更新：2019/08/01 NSK 近藤 (トライ店システム評価)次世代サービスオペレーション効率化に向けた、業務適合性検証
//更新：2019/12/06 NSK 皆川 (トライ店システム評価)整備作業における休憩時間取得判定向上検証
//更新：
//------------------------------------------------------------------------------

var C_INTERVAL_TIME = 1000 * 3;             //タイマーのインターバル時間（ミリ秒）
var C_CHIP_UPDATE_TIME = 1000 * 60;         //チップ情報の再読込時間
var C_METER_UPDATE_TIME = 1000 * 120;       //作業進捗バーの再読込時間
var C_CURRENT_UPDATE_TIME = 1000 * 60;      //現在時刻表示の切り替え時間（ミリ秒）
var C_BLINK_TIME = 1000 * 1;                //チップ点滅時間（消えるまでor表示されるまでの時間）（ミリ秒）

var gChipUpdateTiming = C_CHIP_UPDATE_TIME / C_INTERVAL_TIME;           //チップ情報の再読込カウンタタイミング
var gCurrentUpdateTiming = C_CURRENT_UPDATE_TIME / C_INTERVAL_TIME;     //現在時刻の表示切替タイミング
var gMeterUpdateTiming = C_METER_UPDATE_TIME / C_INTERVAL_TIME;         //作業進捗バーの表示切替タイミング
var gChipBlinkTiming = (C_BLINK_TIME * 2) / C_INTERVAL_TIME;            //チップ点滅発生タイミング

var gChipUpdateCount = 0;           //チップ情報の再読込カウンタ
var gCurrentUpdateCount = 0;        //現在時刻の表示切替カウンタ
var gMeterUpdateCount = 0;          //作業進捗バーの表示切替カウンタ
var gChipBlinkCount = 0;            //チップ点滅処理発生カウンタ


//グレーフィルタをかけた際の透過度
var C_FILTER_TRANSLUCENT = 0.5;
//グレーフィルタをかけない場合の透過度
var C_FILTER_CLEAR = 0;
//チップが作業中につき点滅する際の、最大透過度
var C_BLINK_MAX_TRANSMITTANCE = 1;
//チップが作業中につき点滅する際の、最小透過度
var C_BLINK_MIN_TRANSMITTANCE = 0.1;

//R/O情報欄のフィルタフラグ：フィルタをかける
var C_REPAIR_ORDER_FILTER_ON = "1";
//R/O情報欄のフィルタフラグ：フィルタをかけない
var C_REPAIR_ORDER_FILTER_OFF = "0";

//Box01GraphCassetteの幅
var C_CASSETTE_WIDTH = 53;

//キャンバスの幅を初期化
var gCanvasWidth = 0;
//Box01GraphCassetteを作成する数を初期化する
var gGraphCassetteCount = 0;
//Box01GraphBoxの描画開始時間
var gGraphBoxStartTime = new Date();
//Box01GraphBoxの描画終了時間
var gGraphBoxEndTime = new Date();
//1つのBox01GraphCassetteの描画時間（ミリ秒）
var C_CASSETTE_PITCH = 60 * 60 * 1000;

//チップクラス配列の初期化
var gArrObjChip = Array();
//作業進捗メータークラス
var gWorkMeter;

//ポストバックされた状態を示す
var C_POSTBACK_TRUE = "1";
//ポストバックされていない状態を示す
var C_POSTBACK_FALSE = "0";

//チップ選択がなされてない状態を示す
var C_SELECTED_CHIP_OFF = "0";
//チップ選択がなされている状態を示す
var C_SELECTED_CHIP_ON = "1";

//基本情報タブのタブ番号
var C_ROTAB_CLASS_BASE_NUMBER = "1";
//ご用命事項タブのタブ番号
var C_ROTAB_CLASS_ORDER_NUMBER = "2";
//作業内容タブのタブ番号
var C_ROTAB_CLASS_WORK_NUMBER = "3";

//部品準備が完了していない状態
var C_PARTS_REPARE_UNPREPARED = "0";
//部品準備が完了している状態
var C_PARTS_REPARE_PREPARED = "1";

//ページ取得時のサーバとクライアントの時間差
var gServerTimeDifference = 0;

//休憩Popupの表示フラグ：表示
var C_BREAK_POPUP_DISPLAY = "1";
//休憩Popupの表示フラグ：非表示
var C_BREAK_POPUP_NONE = "0";

//完成検査承認前（承認済みを含む）
var C_INSPECTION_APPROVAL_BEFORE = "0";

//未着工指示の作業連番
var C_UNINSTRUCT = "0"

//12/02/21 TMEJ 成澤   【A.STEP1】TC着工指示オペレーション確立に向けた評価アプリ作成(TCステータスモニターへの遷移機能 START
//スクリーンセイバー画面遷移タイマー
var screenSeverTimer;
//スクリーンセイバー画面遷移秒数
var secondNum;
//12/02/21 TMEJ 成澤   【A.STEP1】TC着工指示オペレーション確立に向けた評価アプリ作成(TCステータスモニターへの遷移機能 END

//2014/07/23 TMEJ 成澤　【開発】IT9711_タブレットSMB Job Dispatch機能開発 START
var CAll_BY_SC3150101 = "0";
var CAll_BY_SC3150102 = "1";
var suspendWorkButtonFlg = 0;
var OPERATION_CODE_CHT = "CHT";
//2014/07/23 TMEJ 成澤　【開発】IT9711_タブレットSMB Job Dispatch機能開発 END

/**
 * DOMロード直後の処理(重要事項).
 * @return {void}
 */
$(function () {
    // window.onerror = function (desc, page, line, chr) { alert('[Error caught by SC3150101.Main.js]' + ' desc:' + desc + ', page:' + page + ', line:' + line + ', chr:' + chr); }

    //2016/03/30 NSK 小牟禮 アクティビティインジケータが消えない問題 START
    //クルクル表示
    //初期表示時には全体のクルクルアイコンは表示しないため、
    //クルクルアイコンは表示しない読み込み関数を利用する
    LoadingScreenNoIcon();
    //2016/03/30 NSK 小牟禮 アクティビティインジケータが消えない問題 END

    //クライアントで取得できる時間とサーバ取得時間との差を設定する.
    SetServerTimeDifference();

    //開始時間・終了時間を取得し、時間で丸める.
    var stallStart = new Date($("#HiddenStallStartTime").val().toString());
    gGraphBoxEndTime = new Date($("#HiddenStallEndTime").val().toString());
    gGraphBoxStartTime = new Date(stallStart.getFullYear(), stallStart.getMonth(), stallStart.getDate(), stallStart.getHours());

    //時間スケールを作成する.
    createScale();

    //現在時刻の配置イベント.
    setCurrentBoxPosition();

    //チップ情報をサーバより取得し、チップを配置する.
    createChipObject();

    //進捗作業クラスを新規作成.
    gWorkMeter = new workMeter();

    //初回選択フラグにより、チップの選択イベントの初期化処理.
    initSelectedChip();
    
    //フッターアプリの起動設定
    SetFutterApplication();
    
    //フリックが可能なように設定する.
    $('#Box01GraphBox').flickable();
    //スクロールの初期位置を設定する.
    initBox01GraphBoxScroll();
    
    //タイマー処理を設定する。
    (function pageRefreshLoop() {
        setTimeout(function () {    //処理が3秒超えてもQueueが溜まらないように、固定間隔のsetIntervalを使わずに、setTimeoutを使う
            controlTimer();
            pageRefreshLoop();
        }, C_INTERVAL_TIME);
    })();

    //フリックしてスクロールさせたときのイベント.
    //左からのスクロール位置をHiddenFieldに格納する.
    $("#Box01GraphBox").scroll(function () {
        $("#HiddenScrollLeft").val($(this).scrollLeft());
    });

    //休憩をとる・とらないを問うpopupを表示する処理.
    if ($("#HiddenBreakPopup").val() == C_BREAK_POPUP_DISPLAY) {
        //フラグを初期化する.
        $("#HiddenBreakPopup").val(C_BREAK_POPUP_NONE);

        //2014/07/23 TMEJ 成澤　【開発】IT9711_タブレットSMB Job Dispatch機能開発 START

        //ポップアップ表示処理
        //selectClass();
        selectClass(CAll_BY_SC3150101);

        //2014/07/23 TMEJ 成澤　【開発】IT9711_タブレットSMB Job Dispatch機能開発 END
    }

    //2016/03/30 NSK 小牟禮 アクティビティインジケータが消えない問題 START
    //リロードフラグをONに設定
    //$("#HiddenReloadFlag").val("1");
    //2016/03/30 NSK 小牟禮 アクティビティインジケータが消えない問題 END

    //2014/07/23 TMEJ 成澤　【開発】IT9711_タブレットSMB Job Dispatch機能開発 START

//    $("#ButtonSuspendWork").bind("click", function () {
//        HiddenButtonSuspendWork.click();
//    });

    //検査開始ボタン押下イベント
    $("#ButtonStartCheck").bind("click", function () {

        //2016/03/30 NSK 小牟禮 アクティビティインジケータが消えない問題 START
        //HiddenButtonStartCheck.click();

        if (!FooterButtonClick()) {
            return;
        }

        //クルクル表示
        LoadingScreen();
        
        setTimeout(function () {
            HiddenButtonStartCheck.click();
        }, 0);
        //2016/03/30 NSK 小牟禮 アクティビティインジケータが消えない問題 END
    });


    //開始ボタン押下イベント
//    $("#ButtonStartWork").bind("click", function () {
//        HiddenButtonStartWork.click();
//    });

    //開始ボタン押下イベント
    $("#ButtonStartWork").bind("click", function () {

        //2016/03/30 NSK 小牟禮 アクティビティインジケータが消えない問題 START
        if (!FooterButtonClick()) {
            return;
        }
        //2016/03/30 NSK 小牟禮 アクティビティインジケータが消えない問題 END

        var temp = $('#stc01Box03').contents().find("#HiddenHasStopJobValue").val()
        if (temp == "1") {
            RunConfirmation(true);
        } else {
            //2016/03/30 NSK 小牟禮 アクティビティインジケータが消えない問題 START
            //HiddenButtonStartWork.click();

            //クルクル表示
            LoadingScreen();
            
            setTimeout(function () {
                HiddenButtonStartWork.click();
            }, 0);
            //2016/03/30 NSK 小牟禮 アクティビティインジケータが消えない問題 END
        }
    });

    //チップが選択されてない場合処理しない
    if($("#HiddenSelectedId").val() != ""){

    //選択チップのID取得
    var _selectedChipId = $("#HiddenSelectedId").val().toString();
    //営業終了時刻より予定終了時刻が長い場合かつ、ログインユーザーがChTの場合
    if (gArrObjChip[_selectedChipId].chipResultEndTime > gGraphBoxEndTime && $("#HiddenOpretionCode").val() == OPERATION_CODE_CHT){

    //作業終了ボタン押下時に、ポップアップ表示
      $("#ButtonFinishWork").popover({
        id: "point",
        offsetX: 0,
        offsetY: 0,
        preventLeft: true,
        preventRight: true,
        preventTop: false,
        preventBottom: true,
        "header": "#CTConfirmPop_Header",
        "content": "#CTConfirmPop_content",
      });


 　}else{
    //13/08/08 TMEJ 成澤 【A.STEP2】タブレット版SMB開発に向けた要件定義 START
    //終了ボタン押下イベント
    $("#ButtonFinishWork").bind("click", function () {

        //2016/03/30 NSK 小牟禮 アクティビティインジケータが消えない問題 START
        if (!FooterButtonClick()) {
            return;
        }
        //2016/03/30 NSK 小牟禮 アクティビティインジケータが消えない問題 END

        var temp = $('#stc01Box03').contents().find("#HiddenHasStopJobValue").val()
        if (temp == "1") {
            RunConfirmation(false);
        } else {

            //2016/03/30 NSK 小牟禮 アクティビティインジケータが消えない問題 START
            //HiddenButtonFinishWork.click();

            //クルクル表示
            LoadingScreen();

            setTimeout(function () {
                HiddenButtonFinishWork.click();
            }, 0);
            //2016/03/30 NSK 小牟禮 アクティビティインジケータが消えない問題 END
        }

    });
    //13/08/08 TMEJ 成澤 【A.STEP2】タブレット版SMB開発に向けた要件定義 END

  }
  }
    //ポップアップの日跨ぎ処理ボタン押下イベント
    $("#PopUpButtonSuspendWork").bind("click", function () {

        //2016/03/30 NSK 小牟禮 アクティビティインジケータが消えない問題 START
        //HiddenButtonSuspendWork.click();

        //クルクル表示
        LoadingScreen();

        setTimeout(function () {
            HiddenButtonSuspendWork.click();
        }, 0);
        //2016/03/30 NSK 小牟禮 アクティビティインジケータが消えない問題 END
    });

    $("#PopUpButtonSuspendWork").bind("mouseenter", function () {
        $("#PopUpButtonSuspendWork").css("color", "BLACK");

    });
    
    //ポップアップの終了ボタン押下イベント
    $("#PopUpButtonFinishWork").bind("click", function () {
        var temp = $('#stc01Box03').contents().find("#HiddenHasStopJobValue").val()
        if (temp == "1") {
            RunConfirmation(false);
        } else {

            //2016/03/30 NSK 小牟禮 アクティビティインジケータが消えない問題 START
            //HiddenButtonFinishWork.click();

            //クルクル表示
            LoadingScreen();

            setTimeout(function () {
                HiddenButtonFinishWork.click();
            }, 0);
            //2016/03/30 NSK 小牟禮 アクティビティインジケータが消えない問題 END
        }
    });

    
    $("#PopUpButtonFinishWork").bind("mouseenter", function () {
        $("#paperPoint").css("color", "BLACK");

    });
     //2014/07/23 TMEJ 成澤　【開発】IT9711_タブレットSMB Job Dispatch機能開発 END

    //日跨ぎ処理ボタン押下イベント
    $("#ButtonSuspendWork").bind("click", function () {

        //2016/03/30 NSK 小牟禮 アクティビティインジケータが消えない問題 START
        //HiddenButtonSuspendWork.click();

        if (!FooterButtonClick()) {
            return;
        }

        LoadingScreen();

        setTimeout(function () {
            HiddenButtonSuspendWork.click();
        }, 0);
        //2016/03/30 NSK 小牟禮 アクティビティインジケータが消えない問題 END
    });




    //13/12/12 TMEJ 成澤 【開発】IT9611_次世代サービス 工程管理機能開発 START
    //追加作業ボタンを押下イベント
    $("#ButtonAddWork").bind("click", function () {

        //2016/03/30 NSK 小牟禮 アクティビティインジケータが消えない問題 START
        if (!FooterButtonClick()) {
            return;
        }
        
        //clearTimer();
        //2016/03/30 NSK 小牟禮 アクティビティインジケータが消えない問題 END

        //確認ポップアップ表示
        var result = confirm($("#HiddenAddWorkConfirmWord").val());
        if (result) {

            //2016/03/30 NSK 小牟禮 アクティビティインジケータが消えない問題 START
            //HiddenButtonAddWork.click();

            //クルクル表示
            LoadingScreen();

            setTimeout(function () {
                HiddenButtonAddWork.click();
            }, 0);
            //2016/03/30 NSK 小牟禮 アクティビティインジケータが消えない問題 END
        }
    });
    
    //13/12/12 TMEJ 成澤 【開発】IT9611_次世代サービス 工程管理機能開発 END

    //2014/07/23 TMEJ 成澤　【開発】IT9711_タブレットSMB Job Dispatch機能開発 START
    //中断ボタンを押下イベント
    $("#ButtonStopWork").bind("click", function () {

        //2016/03/30 NSK 小牟禮 アクティビティインジケータが消えない問題 START
        if (!FooterButtonClick()) {
            return;
        }
        
        //clearTimer();
        //2016/03/30 NSK 小牟禮 アクティビティインジケータが消えない問題 END

        //呼び出し元フラグを設定
        WindowCallByFlg(CAll_BY_SC3150101);
        //中断理由ポップアップ表示
        ChanselPopUp();

    });
    //2014/07/23 TMEJ 成澤　【開発】IT9711_タブレットSMB Job Dispatch機能開発 END 


    //2013/02/21 TMEJ 成澤【SERVICE_2】TCステータスモニターへの遷移機能 START

    //スクリーンセイバー画面遷移タイマースタート
    ScreenTimerStart();

    //画面タッチイベント(タブレット用)
    window.addEventListener('touchstart', function (e) {
        //スクリーンセイバータイマーリセット
        ScreenTimerRestart();
    });

    //マウスクリックイベント(PC用)
    $('html').mousedown(function (e) {
        //スクリーンセイバータイマーリセット
        ScreenTimerRestart();
    });

    //2013/02/21 TMEJ 成澤【SERVICE_2】TCステータスモニターへの遷移機能 END

    //13/06/17 TMEJ 成澤 【開発】IT9513_【A.STEP2】次世代e-CRBタブレット　新DB適応に向けた機能設計　START

    //2014/07/23 TMEJ 成澤　【開発】IT9711_タブレットSMB Job Dispatch機能開発 START
    $("#DummyBtnDoNotBreak").bind("click", function () {
        //    $("#ButtonDoNotBreak").bind("click", function () {

        if ($("#HiddenBreakPopUpFlg").val() == CAll_BY_SC3150101) {

            //2016/03/30 NSK 小牟禮 アクティビティインジケータが消えない問題 START
            //$("#ButtonDoNotBreak").click();

            //クルクル表示
            LoadingScreen();

            setTimeout(function () {
                $("#ButtonDoNotBreak").click();
            }, 0);
            //2016/03/30 NSK 小牟禮 アクティビティインジケータが消えない問題 END

            BreakPopupClose(true);
        } else if ($("#HiddenBreakPopUpFlg").val() == CAll_BY_SC3150102) {

            document.getElementById('stc01Box03').contentWindow.BreakBattonClick(false);
            BreakPopupClose(true);
        }
        //2014/07/23 TMEJ 成澤　【開発】IT9711_タブレットSMB Job Dispatch機能開発 END

    });
    //2014/07/23 TMEJ 成澤　【開発】IT9711_タブレットSMB Job Dispatch機能開発 START
    $("#DummyBtnTakeBreak").bind("click", function () {
        //    $("#ButtonTakeBreak").bind("click", function () {

        if ($("#HiddenBreakPopUpFlg").val() == CAll_BY_SC3150101) {

            //2016/03/30 NSK 小牟禮 アクティビティインジケータが消えない問題 START
            //$("#ButtonTakeBreak").click();
            
            //クルクル表示
            LoadingScreen();

            setTimeout(function () {
                $("#ButtonTakeBreak").click();
            }, 0);
            //2016/03/30 NSK 小牟禮 アクティビティインジケータが消えない問題 END

            BreakPopupClose(true);
        } else if ($("#HiddenBreakPopUpFlg").val() == CAll_BY_SC3150102) {

            document.getElementById('stc01Box03').contentWindow.BreakBattonClick(true);
            BreakPopupClose(true);
        }
        //2014/07/23 TMEJ 成澤　【開発】IT9711_タブレットSMB Job Dispatch機能開発 END

    });
   
    //13/06/17 TMEJ 成澤 【開発】IT9513_【A.STEP2】次世代e-CRBタブレット　新DB適応に向けた機能設計　END
});


/**
 * 部品連絡ポップオーバーのヘッダ部を生成し、返す.
 * @return {String}
 */
function getPopoverHeaderContents() {

    var popoverHeaderContents = "";

    popoverHeaderContents += "<div id='PoPuPBlockSTC0101'>";
    popoverHeaderContents += "<div class='PoPuPBlockSTC0101TitleBlock'>";
    popoverHeaderContents += "<div class='PoPuPBlockSTC0101TitleBlockButtonLeft'>";
    popoverHeaderContents += "<span id='ButtonConnectParts_cancel'>";
    popoverHeaderContents += $("#HiddenPopupPartsCancelWord").val();
    popoverHeaderContents += "</span>";
    popoverHeaderContents += "</div>";
    popoverHeaderContents += "<div class='PoPuPBlockSTC0101TitleBlockName'>";
    popoverHeaderContents += "<h3>";
    popoverHeaderContents += $("#HiddenPopupPartsTitleWord").val();
    popoverHeaderContents += "</h3>";
    popoverHeaderContents += "</div>";
    popoverHeaderContents += "</div>";
    popoverHeaderContents += "</div>";

    return popoverHeaderContents;
}


/**
 * サーバとの時間差を算出し、グローバル変数として格納する.
 * @return {void}
 */
function SetServerTimeDifference() {

    //ページ読込時のサーバ時間を取得する.
    var pageLoadServerTime = new Date($("#HiddenServerTime").val());
    //クライアントの現在時刻を取得する.
    var pageLoadClientTime = new Date();

    //サーバとの時間差を算出し、格納する（ミリ秒）.
    gServerTimeDifference = pageLoadServerTime - pageLoadClientTime;
}


/**
 * サーバの現在時刻を算出し、返す.
 * @return {Date}
 */
function getServerTimeNow() {

    //サーバの現在時刻を算出する.
    var serverTime = new Date();
    serverTime.setTime(serverTime.getTime() + gServerTimeDifference);

    return serverTime;
}

/**
 * 休憩をとる・とらないのPopupを表示する.
 * @return {void}
 */
function selectClass(CallByFlg)  {
    $("#tcvNsc31Black").css("display", "inline-block");
    $("#tcvNsc31Main").css("display", "inline-block");
    $("#popWind").fadeIn("slow");
    //2014/07/23 TMEJ 成澤　【開発】IT9711_タブレットSMB Job Dispatch機能開発 START
    $("#HiddenBreakPopUpFlg").val(CallByFlg);
    //2014/07/23 TMEJ 成澤　【開発】IT9711_タブレットSMB Job Dispatch機能開発 END
}

/**
 * 休憩をとる・とらないのPopupを閉じる.
 * @param {boolean} true値にて閉じる動作
 * @return {void}
 */
function BreakPopupConfirm(flag) {
    $("#tcvNsc31Black").css("display", "none");
    $("#tcvNsc31Main").css("display", "none");
    $("#popWind").fadeOut("slow");
  
}

function BreakPopupClose(flag) {
    $("#tcvNsc31Black").css("display", "none");
    $("#tcvNsc31Main").css("display", "none");

    //13/06/17 TMEJ 成澤 【開発】IT9513_【A.STEP2】次世代e-CRBタブレット　新DB適応に向けた機能設計　START
    // $("#popWind").fadeOut("slow");
    //ポップアップ非表示
    $("#popWind").css("display", "none");
    //2016/03/30 NSK 小牟禮 アクティビティインジケータが消えない問題 START
    ////クルクル表示
    //$.master.OpenLoadingScreen();
    //2016/03/30 NSK 小牟禮 アクティビティインジケータが消えない問題 END
    //13/06/17 TMEJ 成澤 【開発】IT9513_【A.STEP2】次世代e-CRBタブレット　新DB適応に向けた機能設計　END
} 

/**
 * フッタボタンの表示制御.
 * @return {void}
 */
function controlFooterButton() {

    var _selectedChipId = $("#HiddenSelectedId").val().toString();
    var AddWorkButtonFlg = $("#HiddenAddWorkButtonFlg").val()
    //すべてのボタンを非表示に設定.
    $("#ButtonConnectParts").css("display", "none");
    $("#ButtonStartWork").css("display", "none");
    $("#ButtonSuspendWork").css("display", "none");
    $("#ButtonStartCheck").css("display", "none");
    //13/08/08 TMEJ 成澤 【A.STEP2】タブレット版SMB開発に向けた要件定義 START
    $("#ButtonFinishWork").css("display", "none");
    //13/08/08 TMEJ 成澤 【A.STEP2】タブレット版SMB開発に向けた要件定義  END

    // 2013/12/12 TMEJ 成澤 【開発】IT9611_次世代サービス 工程管理機能開発 START
    $("#ButtonAddWork").css("display", "none");
    // 2013/12/12 TMEJ 成澤 【開発】IT9611_次世代サービス 工程管理機能開発 EMD

    //2014/07/23 TMEJ 成澤　【開発】IT9711_タブレットSMB Job Dispatch機能開発 START
    $("#ButtonStopWork").css("display", "none");
    //2014/07/23 TMEJ 成澤　【開発】IT9711_タブレットSMB Job Dispatch機能開発 END

    //フッタボタンが表示可能条件を満たす場合、各ボタンの表示判定を実施する.
    if (checkDisplayFooterButton()) {

        var partsDataCount = parseInt($("#HiddenPartsCount").val())

        // 2019/06/03 NSK 鈴木 18PRJ03142_(FS)業務検証:セールス受注前活動活動結果登録オペレーション[TKM]PUAT-4109 部品詳細にて、バックオーダー(B/O)が表示されない START
        // var partsBackOrderCount = parseInt($("#HiddenBackOrderCount").val())
        // 2019/06/03 NSK 鈴木 18PRJ03142_(FS)業務検証:セールス受注前活動活動結果登録オペレーション[TKM]PUAT-4109 部品詳細にて、バックオーダー(B/O)が表示されない END

        var partsDataComp = gArrObjChip[_selectedChipId].merchandiseFlag;
        //2014/07/23 TMEJ 成澤　【開発】IT9711_タブレットSMB Job Dispatch機能開発 START
         resultEndTime = gArrObjChip[_selectedChipId].chipResultEndTime;
         //2014/07/23 TMEJ 成澤　【開発】IT9711_タブレットSMB Job Dispatch機能開発 END

        // 2019/06/03 NSK 鈴木 18PRJ03142_(FS)業務検証:セールス受注前活動活動結果登録オペレーション[TKM]PUAT-4109 部品詳細にて、バックオーダー(B/O)が表示されない START
        // //タブが選択されたとき、部品連絡ボタンの表示処理を行う.
        // //作業内容タブが表示されている.
        // if (gArrObjChip[_selectedChipId].orderNumber != "" && $("#HiddenSelectedTabNumber").val() == C_ROTAB_CLASS_WORK_NUMBER) {
        //     //B/O項目を除く部品情報数を取得する.
        //     var partsDataCountBackOrderOut = 0;
        //     if (partsDataCount > 0) {
        //         partsDataCountBackOrderOut = partsDataCount;
        //     }
        //     if (partsBackOrderCount > 0) {
        //         partsDataCountBackOrderOut = partsDataCountBackOrderOut - partsBackOrderCount;
        //     }
        //     //B/O項目を除く部品情報数が1件以上ある場合、部品連絡ボタンを表示する可能性がある.
        //    /*  if (partsDataCountBackOrderOut > 0) {
        //         //部品準備完了している場合、部品連絡ボタンを表示する.
        //         if (partsDataComp == C_PARTS_REPARE_PREPARED) {
        //             $("#ButtonConnectParts").css("display", "inline-block");
        //         }
        //     } */
        // }
        // 2019/06/03 NSK 鈴木 18PRJ03142_(FS)業務検証:セールス受注前活動活動結果登録オペレーション[TKM]PUAT-4109 部品詳細にて、バックオーダー(B/O)が表示されない END

        var resultStatus = gArrObjChip[_selectedChipId].chipResultStatus;
        if (resultStatus == C_RESULT_STATUS_WAIT) {
            //実績ステータスが、作業待ちである場合、開始ボタンを表示とする.
            $("#ButtonStartWork").css("display", "inline-block");

            //13/08/08 TMEJ 成澤 【A.STEP2】タブレット版SMB開発に向けた要件定義 START
//        } else  if (gArrObjChip[_selectedChipId].orderNumber != "" && resultStatus == C_RESULT_STATUS_WORKING) {
//            //実績ステータスが、作業中である場合、完成検査ボタン表示
//            $("#ButtonStartCheck").css("display", "inline-block");

//             //終了時間（予定）がストール作業終了時間を越えている場合、当日処理ボタンを表示とする.
//             if (gArrObjChip[_selectedChipId].chipEndTime >= gGraphBoxEndTime) {
//                 var InspectionApproval = $("#HiddenFieldInspectionApprovalFlag").val()

//                 // 完成検査承認前の場合、当日処理を表示する。
//                 if (C_INSPECTION_APPROVAL_BEFORE == InspectionApproval) {
//                    $("#ButtonSuspendWork").css("display", "inline-block");
//                }
//            }
//        }
//    }
        } else if (resultStatus == C_RESULT_STATUS_WORKING) {
            // 2013/12/12 TMEJ 成澤 【開発】IT9611_次世代サービス 工程管理機能開発 START
//            //実績ステータスが、作業中である場合、作業終了ボタン表示
            $("#ButtonFinishWork").css("display", "inline-block");
            // 2013/12/12 TMEJ 成澤 【開発】IT9611_次世代サービス 工程管理機能開発 END

            //2014/07/23 TMEJ 成澤　【開発】IT9711_タブレットSMB Job Dispatch機能開発 START
            $("#ButtonStopWork").css("display", "inline-block");

            resultEndTime = gArrObjChip[_selectedChipId].chipResultEndTime;
            //2014/07/23 TMEJ 成澤　【開発】IT9711_タブレットSMB Job Dispatch機能開発 END

            //2015/04/08 TMEJ 明瀬 TMT２販社号口後フォロー BTS-318 START
            
            //終了時間（予定）がストール作業終了時間を越えている場合、当日処理ボタンを表示とする.
            //if (gArrObjChip[_selectedChipId].chipResultEndTime >= gGraphBoxEndTime) {
            if (gArrObjChip[_selectedChipId].chipResultEndTime > gGraphBoxEndTime) {

            //2015/04/08 TMEJ 明瀬 TMT２販社号口後フォロー BTS-318 END

                    var InspectionApproval = $("#HiddenFieldInspectionApprovalFlag").val();

                    
                    // 完成検査承認前の場合、当日処理を表示する。

                    //2014/12/05 TMEJ 岡田　IT9857_DMS連携版サービスタブレット JobDispatch完成検査入力制御開発 STRAT
//                    //2014/08/11 TMEJ 成澤 【開発】IT9711_タブレットSMB Job Dispatch機能開発 START
////                  if (C_INSPECTION_APPROVAL_BEFORE == InspectionApproval) {
//                    if (C_INSPECTION_APPROVAL_BEFORE == InspectionApproval && $("#HiddenOpretionCode").val() != OPERATION_CODE_CHT) {
//                    //2014/08/11 TMEJ 成澤 【開発】IT9711_タブレットSMB Job Dispatch機能開発 END
                    if (C_INSPECTION_APPROVAL_BEFORE == InspectionApproval) {
                    //2014/12/05 TMEJ 岡田　IT9857_DMS連携版サービスタブレット JobDispatch完成検査入力制御開発 STRAT

                            $("#ButtonSuspendWork").css("display", "inline-block");
                      
                    }
                } 

            }
            //2014/08/11 TMEJ 成澤 【開発】IT9711_タブレットSMB Job Dispatch機能開発 START

//            //RO番号が設定してある場合
//            if (gArrObjChip[_selectedChipId].orderNumber != ""
//            //ストール利用ステータスが"02","03"の場合
//                && (gArrObjChip[_selectedChipId].stallUseStatus == "02"
//                || gArrObjChip[_selectedChipId].stallUseStatus == "03")

            //RO番号が設定してある場合
            if (gArrObjChip[_selectedChipId].orderNumber != ""
            //ストール利用ステータスが"02","03","04"の場合
                && (gArrObjChip[_selectedChipId].stallUseStatus == "02"
                || gArrObjChip[_selectedChipId].stallUseStatus == "03"
                || gArrObjChip[_selectedChipId].stallUseStatus == "04")

            //2014/08/11 TMEJ 成澤 【開発】IT9711_タブレットSMB Job Dispatch機能開発 END
            //完成検査ステータスが0の場合
                && gArrObjChip[_selectedChipId].inspectionReqFlag == C_INSPECTION_APPROVAL_BEFORE
               ) {

                //完成検査ボタン表示
                $("#ButtonStartCheck").css("display", "inline-block");
            }
            //13/08/08 TMEJ 成澤 【A.STEP2】タブレット版SMB開発に向けた要件定義 END

        }
//        追加作業ボタン表示フラグがオンの場合
        if (AddWorkButtonFlg == 1) {
            //追加作業ボタン表示
            $("#ButtonAddWork").css("display", "inline-block");
        }
}


/**
 * 全てのフッタボタンを表示する最低条件の検証
 * @return {Boolean} true:表示可能,false:表示不可能
 */
function checkDisplayFooterButton() {

    var checkResult = false;
    
    var _selectedChipId = $("#HiddenSelectedId").val().toString();
    var _selectedChipStatus = $("#HiddenSelectedChip").val();

    //チップが選択状態、且つ、選択中のチップが存在する場合処理を実施する.
    if ((_selectedChipStatus == C_SELECTED_CHIP_ON) && (gArrObjChip[_selectedChipId])) {

        //13/08/08 TMEJ 成澤 【A.STEP2】タブレット版SMB開発に向けた要件定義 START
        //選択中のチップの実績ステータスが、作業待ち・作業中の場合、ボタンの表示制御を行う.
//        var resultStatus = gArrObjChip[_selectedChipId].chipResultStatus;
//        if ((resultStatus == C_RESULT_STATUS_WAIT) || (resultStatus == C_RESULT_STATUS_WORKING)) {
        //13/08/08 TMEJ 成澤 【A.STEP2】タブレット版SMB開発に向けた要件定義 END

            //作業対象チップの、REZ_REZRECEPTIONが0,4の場合のみ、ボタンの表示制御を行う.
            var reception = gArrObjChip[_selectedChipId].chipRezReception;
            if ((reception == C_REZ_RECEPTION_WAIT) || (reception == C_REZ_RECEPTION_DROPOFF)) {
                checkResult = true;
            }
//        }
    }

    return checkResult;
}


/**
 * タイマーの制御.
 * @return {void}
 */
function controlTimer() {
    //チップの更新処理.
    updateChip();
    //現在時刻の更新処理.
    updateCurrentBar();
    //作業進捗バーの更新処理.
    updateMeter();
    //チップの点滅処理.
    blinkChip();
}


/**
 * チップ情報の更新処理.
 * @return {void}
 */
function updateChip() {

    //チップ情報のカウンタを更新する.
    gChipUpdateCount++;

    //更新後のチップ情報カウンタが更新タイミングを超過する場合、
    //チップの更新処理をし、カウンタを初期化する.
    if (gChipUpdateTiming <= gChipUpdateCount) {

        //新規チップオブジェクトを生成する.
        createChipObject();
        //初回選択フラグにより、チップの選択イベントの初期化処理.
        initSelectedChip();
        //alert("チップ情報を更新しました。");

        gChipUpdateCount = 0;
    }
}


/**
 * 現在時間の更新処理
 * @return {void}
 */
function updateCurrentBar() {

    //現在時刻のカウンタを更新する.
    gCurrentUpdateCount++;

    //更新後の現在時間のカウンタが更新タイミングを超過する場合、
    //現在時刻の更新を行い、現在時刻のカウンタを初期化する.
    if (gCurrentUpdateTiming <= gCurrentUpdateCount) {

    //現在時刻の配置を実施する.
    setCurrentBoxPosition();
    //alert("現在時刻を更新しました。");
    }
}


/**
 * 作業進捗バーの更新処理.
 * @return {void}
 */
function updateMeter() {

    //作業進捗バーのカウンタを更新する.
    gMeterUpdateCount++;

    //更新後の現在時間のカウンタが更新タイミングを超過する場合、
    //現在時刻の更新を行い、現在時刻のカウンタを初期化する.
    if (gMeterUpdateTiming <= gMeterUpdateCount) {

        //作業進捗バーを更新処理する.
        gWorkMeter.refreshMeter();
        //alert("作業進捗バーを更新しました。");
    }
}


//画面全体のリロード処理
function reloadPage() {
    $("#HiddenButtonRefresh").click();
    return true;
}


//一時的なNetwork障害などによって発生したクルクル現象の対応としての画面全体リロード処理のタイマー設定
function reloadPageIfNoResponse() {
    timerClearTime = (new Date().getTime()) - 1;
    commonRefreshTimer(reloadPage);
}


//再表示タイマーをリセット
function clearTimer() {
    commonClearTimer();
}


//チップのタップ処理
function tapChip(obj) {

    //チップ選択状態を初期化する.
    var chipSelectedStatus = true;

    //直前の選択されたREZID, ORDERNO, CHILDNOを格納する.
    var lastSelectedRezId = $("#HiddenSelectedReserveId").val();
    var lastSelectedOrderNo = $("#HiddenFieldOrderNo").val();
    var lastSelectedChildNo = $("#HiddenFieldChildNo").val();
    // 作業連番、着工指示区分
    var lastSelectedWorkSeq = $("#HiddenSelectedWorkSeq").val();
    var lastSelectedInstruct = $("#HiddenFieldInstruct").val();
    //2014/07/23 TMEJ 成澤　【開発】IT9711_タブレットSMB Job Dispatch機能開発 START

    var lastSelectedUpdateCount = $("#HiddenSelectedUpdateCount").val();
    //2014/07/23 TMEJ 成澤　【開発】IT9711_タブレットSMB Job Dispatch機能開発 END

    $("#HiddenSelectedReserveId").val("");
    //13/11/12 TMEJ 小澤 【A.STEP2】次世代e-CRBサービスタブレット 新DB適応に向けた機能開発 START
    $("#HiddenSelectedJobDetailId").val("");
    //13/11/12 TMEJ 小澤 【A.STEP2】次世代e-CRBサービスタブレット 新DB適応に向けた機能開発 END
    $("#HiddenFieldOrderNo").val("");
    $("#HiddenFieldChildNo").val("");
    $("#HiddenSelectedWorkSeq").val("");
    $("#HiddenFieldInstruct").val("");
    $("#HiddenSelectedVclRegNo").val("");
    $("#HiddenSelectedUpdateCount").val("");
    $("#HiddenChipResultStatus").val("");
    
    //対象となるオブジェクトが存在する場合のみ、処理を実施する
    if (obj) {
        //選択されたチップのIDを取得する
        var strSelectedChipId = $(obj).attr("id");

        //チップ選択状態を取得し、Hidden格納値を更新する.
        chipSelectedStatus = judgeSelectedChip(strSelectedChipId);
        //選択されたチップIDをHiddenフィールドに格納し、更新する.
        $("#HiddenSelectedId").val(strSelectedChipId);

        //スモークフィルタの表示用に、実績ステータス値を取得する.
        var resultStatus = "0";
        //R/O情報に渡すためのR/O番号を初期化する.
        var _orderNo = "";
        //所持しているチップオブジェクトをループ処理.
        for (var key in gArrObjChip) {

            //チップ選択状態がtrue、すなわち、チップを選択している状態である場合の処理を実施する.
            if (chipSelectedStatus) {
                //選択されたチップのIDと同値のチップIDの場合.
                if (gArrObjChip[key].chipId == strSelectedChipId) {
                    //チップ選択フラグをtrueにする.
                    gArrObjChip[key].setChipFilter(true, gArrObjChip[key].chipResultStatus);
                    //選択されたチップのREZIDをHiddenに格納する.
                    $("#HiddenSelectedReserveId").val(gArrObjChip[key].rezId);
                    //13/11/12 TMEJ 小澤 【A.STEP2】次世代e-CRBサービスタブレット 新DB適応に向けた機能開発 START
                    $("#HiddenSelectedJobDetailId").val(gArrObjChip[key].seqNo);
                    //13/11/12 TMEJ 小澤 【A.STEP2】次世代e-CRBサービスタブレット 新DB適応に向けた機能開発 END

                    //2014/07/23 TMEJ 成澤　【開発】IT9711_タブレットSMB Job Dispatch機能開発 START
                    $("#HiddenSelectedStallUseStatus").val(gArrObjChip[key].stallUseStatus);
                     //2014/07/23 TMEJ 成澤　【開発】IT9711_タブレットSMB Job Dispatch機能開発 END

                    //R/O番号を設定する.
                    _orderNo = gArrObjChip[key].orderNumber;
                    $("#HiddenFieldOrderNo").val(_orderNo);

                    //2013/11/26 TMEJ 成澤【IT9573】次世代e-CRBサービス 店舗展開に向けた標準作業確立 START
                    $("#HiddenSelectdealerCode").val(gArrObjChip[key].dealerCode);          //販売店コード
                    //2013/11/26 TMEJ 成澤【IT9573】次世代e-CRBサービス 店舗展開に向けた標準作業確立 END

                    $("#HiddenFieldChildNo").val(gArrObjChip[key].childNumber);             // 子番号
                    $("#HiddenSelectedWorkSeq").val(gArrObjChip[key].workSeq);              // 作業連番
                    $("#HiddenFieldInstruct").val(gArrObjChip[key].instruct);               // 着工指示区分
                    $("#HiddenSelectedVclRegNo").val(gArrObjChip[key].chipVclRegNo);        // 車輌登録番号
                    $("#HiddenSelectedUpdateCount").val(gArrObjChip[key].chipUpdateCount);  // 更新カウント

                    //実績ステータスを取得する.
                    resultStatus = gArrObjChip[key].chipResultStatus;

                    //2012/08/09 TMEJ 小澤【SERVICE_2】矢印アイコン制御追加 START
                    $("#HiddenChipResultStatus").val(gArrObjChip[key].chipResultStatus);
                    //2012/08/09 TMEJ 小澤【SERVICE_2】矢印アイコン制御追加 END

                    //2014/08/29 TMEJ 成澤 【開発】IT9737_NextSTEPサービス ロケ管理の効率化に向けた評価用アプリ作成　START
                    //選択されたチップ情報を作業進捗メータークラスに渡す.
                    //gWorkMeter.setMeterParameter(gArrObjChip[key].chipDrawStartTime, gArrObjChip[key].chipDrawEndTime,
                    //_orderNo, gArrObjChip[key].chipDrawStartTime, gArrObjChip[key].chipDrawEndTime, resultStatus);
                    gWorkMeter.setMeterParameter(gArrObjChip[key].chipDrawStartTime, gArrObjChip[key].chipDrawEndTime,
                    _orderNo, gArrObjChip[key].chipDrawStartTime, gArrObjChip[key].chipDrawEndTime, resultStatus, gArrObjChip[key].parkingCode);
                    //2014/08/29 TMEJ 成澤 【開発】IT9737_NextSTEPサービス ロケ管理の効率化に向けた評価用アプリ作成　END

                    //渡したチップ情報を元に、作業進捗メーターを再描画する.
                    gWorkMeter.refreshMeter();
                }
                //選択されたチップのIDと同値のチップでない場合.
                else {
                    //チップ選択フラグをfalseにする.
                    gArrObjChip[key].setChipFilter(false, gArrObjChip[key].chipResultStatus);
                   
                }
                //Box01GraphLineにグレーフィルターをかける.
                $("#Box01GraphLineFilter").css("opacity", C_FILTER_TRANSLUCENT);
               
            }
            //チップ選択状態がfalse、すなわち、チップを選択している状態でない場合、チップ選択を解除する.
            else {
                //チップ選択フラグをtrueにする.
                gArrObjChip[key].setChipFilter(true, gArrObjChip[key].chipResultStatus);
               
                //Box01GraphLineのグレーフィルターを解除する.
                $("#Box01GraphLineFilter").css("opacity", C_FILTER_CLEAR);

            }
        } //End for
    }
    else {
        //2014/06/11 TMEJ 明瀬 【TMT_IT2】チップ削除後の更新でスクリプトエラー発生の対応 START
        $("#HiddenSelectedId").val("");
        $("#HiddenSelectedChip").val("");
        //2014/06/11 TMEJ 明瀬 【TMT_IT2】チップ削除後の更新でスクリプトエラー発生の対応 END

        //Box01GraphLineのグレーフィルターを解除する.
        $("#Box01GraphLineFilter").css("opacity", C_FILTER_CLEAR);
        //渡したチップ情報を元に、作業進捗メーターを再描画する.
        gWorkMeter.refreshMeter();

        //2013/06/17 TMEJ 成澤 【開発】IT9513_【A.STEP2】次世代e-CRBタブレット　新DB適応に向けた機能設計 START
        //実績チップのフィルターを実績チップより前面に配置する
        $(".ChipsBaseFilter").css("z-index", "3");
        //2013/06/17 TMEJ 成澤 【開発】IT9513_【A.STEP2】次世代e-CRBタブレット　新DB適応に向けた機能設計 END
    }

    //R/O情報欄のフィルターフラグを設定する.
    var repairOrderFilterFlag = C_REPAIR_ORDER_FILTER_ON;
    if (obj) {

        //チップの実績ステータスが、作業待ち・作業中の場合、且つ、REZ_REZRECEPTIONが0,4の場合のみスモークフィルタを除去する.
        //2013/06/17 TMEJ 成澤 【開発】IT9513_【A.STEP2】次世代e-CRBタブレット　新DB適応に向けた機能設計 START
        if ((resultStatus == C_RESULT_STATUS_WAIT) || (resultStatus == C_RESULT_STATUS_WORKING)) {
//            var reception = gArrObjChip[strSelectedChipId].chipRezReception;
//            if ((reception == C_REZ_RECEPTION_WAIT) || (reception == C_REZ_RECEPTION_DROPOFF)) {
                repairOrderFilterFlag = C_REPAIR_ORDER_FILTER_OFF;
//            }

            }
        //2013/06/17 TMEJ 成澤 【開発】IT9513_【A.STEP2】次世代e-CRBタブレット　新DB適応に向けた機能設計 END
    }
    else {
        // チップの選択が無い場合は、グレーフィルターをかけない
        repairOrderFilterFlag = C_REPAIR_ORDER_FILTER_OFF;
    }
    $("#HiddenFieldRepairOrderFilter").val(repairOrderFilterFlag);

    // 前回と値が違えばセッションに値を入れなおし
    if ((lastSelectedRezId != $("#HiddenSelectedReserveId").val())
     || (lastSelectedOrderNo != $("#HiddenFieldOrderNo").val())
     || (lastSelectedChildNo != $("#HiddenFieldChildNo").val())
     || (lastSelectedWorkSeq != $("#HiddenSelectedWorkSeq").val())
     || (lastSelectedInstruct != $("#HiddenFieldInstruct").val())
     || (lastSelectedUpdateCount != $("#HiddenSelectedUpdateCount").val())) {

        //部品情報数を初期化する.
        $("#HiddenPartsCount").val("");

        // 2019/06/03 NSK 鈴木 18PRJ03142_(FS)業務検証:セールス受注前活動活動結果登録オペレーション[TKM]PUAT-4109 部品詳細にて、バックオーダー(B/O)が表示されない START
        // //B/O項目数を初期化する.
        // $("#HiddenBackOrderCount").val("");
        // 2019/06/03 NSK 鈴木 18PRJ03142_(FS)業務検証:セールス受注前活動活動結果登録オペレーション[TKM]PUAT-4109 部品詳細にて、バックオーダー(B/O)が表示されない END

        //2016/03/30 NSK 小牟禮 アクティビティインジケータが消えない問題 START
        ////リロードフラグをONに設定（フッタボタン制御）
        //$("#HiddenReloadFlag").val("1");

        //setTimeout(function () {
		//	reloadPageIfNoResponse();
        //    HiddenButtonChipTap.click();
        //}, 0);

        //クルクル表示
        //初期表示時にも呼び出され、初期表示時には全体のクルクルアイコンは表示しないため、
        //クルクルアイコンは表示しない読み込み関数を利用する
        LoadingScreenNoIcon();

        setTimeout(function () {
            HiddenButtonChipTap.click();
        }, 0);
        //2016/03/30 NSK 小牟禮 アクティビティインジケータが消えない問題 END
    }
    else {
        //フッタボタンの表示制御を行う
        controlFooterButton();
    }
}

//2013/12/12 TMEJ 成澤 【開発】IT9611_次世代サービス 工程管理機能開発　START

//2012/08/09 TMEJ 小澤【SERVICE_2】矢印アイコン制御追加 START
/**
* 選択されているチップの状況を判断し、フッターに▼を表示する.
*/
//function setArrowImage() {
//    //矢印アイコン表示制御を行う前に矢印アイコンを全て非表示にする
//    setTimeout(function () { icropScript.ui.arrowImageOff() }, 1000);
//    //チップが選択されている場合(グレーフィルター制御で判断)
//    if ($("#HiddenFieldRepairOrderFilter").val() == C_REPAIR_ORDER_FILTER_OFF) {
//        //選択チップのR/O作業ステータスが「4:部品待ち」「2:整備中」「7:検査完了」の場合
//        if ($("#HiddenOrderStatus").val() == "4" || $("#HiddenOrderStatus").val() == "2" || $("#HiddenOrderStatus").val() == "7") {
//            //「追加作業」フッターボタンに矢印アイコンを表示する
//            setTimeout(function () { icropScript.ui.arrowImageOn("1100") }, 1000);
//        }
//        //選択チップが「20:作業中」の場合
//        if ($("#HiddenChipResultStatus").val() == C_RESULT_STATUS_WORKING) {
//            //「完成検査」フッターボタンに矢印アイコンを表示する
//            setTimeout(function () { icropScript.ui.arrowImageOn("1000") }, 1000);
//        }
//    }
//}

//2012/08/09 TMEJ 小澤【SERVICE_2】矢印アイコン制御追加 END

//2013/12/12 TMEJ 成澤 【開発】IT9611_次世代サービス 工程管理機能開発　END


/**
 * 選択されているチップを選択状態にするかを判定し、現在のチップ選択状態を格納する.
 *
 * @param {String} aSelectedChipId 選択されているチップのID
 * @return {boolean} true:チップが選択状態になっている、false:チップが選択状態になっていない
 */
function judgeSelectedChip(aSelectedChipId) {

    var selectedChipStatus = false;
    //前回選択されたチップIDをHiddenフィールドより取得する.
    var lastSelectedChipId = $("#HiddenSelectedId").val();

    //前回選択されたチップと現在選択されたチップが同値であるかを判定し、
    //同値であった場合、チップ選択状態を反転させる.
    if (aSelectedChipId == lastSelectedChipId) {
        toggleSelectedChip();
        //反転させて、Hiddenに格納したチップの状態に応じて、チップの選択状態を設定する.
        if ($("#HiddenSelectedChip").val() == C_SELECTED_CHIP_ON) {
            selectedChipStatus = true;

        }
        else {
            selectedChipStatus = false;
           
        }
    } else {
        //異なる値であった場合、チップ選択状態をONとする.
        $("#HiddenSelectedChip").val(C_SELECTED_CHIP_ON);
        //チップの選択状態をtrue値に設定する.
        selectedChipStatus = true;
    }

    return selectedChipStatus;
}


//チップの選択状態（ON・OFF）を切り替える
function toggleSelectedChip() {

    //現在のチップの状態をHiddenフィールドより取得する
    var _chipSelectedStatus = $("#HiddenSelectedChip").val();
    //チップの選択状態がOFFの場合、ONに切り替える
    if (_chipSelectedStatus == C_SELECTED_CHIP_OFF) {
        $("#HiddenSelectedChip").val(C_SELECTED_CHIP_ON);
    }
    //その他の場合は、OFF状態に切り替える
    else {
        $("#HiddenSelectedChip").val(C_SELECTED_CHIP_OFF);
    }
}


//チップを点滅させる条件に合致しているかをチェックし、合致していた場合点滅処理させる
function checkBlinkChip(obj) {

    //チップのアニメーションを停止させ、透過度をなしにして表示する.
    $(obj.objChipsBase).stop(true, true).fadeTo("fast", C_BLINK_MAX_TRANSMITTANCE);

    if (obj.chipResultStatus == C_RESULT_STATUS_WORKING) {
        blinkChip(obj.objChipsBase);
    }
}


/**
 * チップの点滅処理.
 * @return {void}
 */
function blinkChip() {
    //チップの点滅カウンタを更新
    gChipBlinkCount++;
    //チップの点滅カウンタが、更新値に達している場合、点滅処理を実施しカウンタを初期化する
    if (gChipBlinkTiming <= gChipBlinkCount) {
        for (var key in gArrObjChip) {
            if (gArrObjChip[key].chipResultStatus == C_RESULT_STATUS_WORKING) {
                $(gArrObjChip[key].objChipsBase).stop(true, true)
                                                .fadeTo(C_BLINK_TIME, C_BLINK_MIN_TRANSMITTANCE)
                                                .fadeTo(C_BLINK_TIME, C_BLINK_MAX_TRANSMITTANCE);
            }
        }
    }
}


/**
 * R/O情報をクリックした際に、POPOVERを制御する.
 * @return {void}
 */
function ParentPopoverClose() {
    $("#bodyFrame").click();
}

/**
* R/O情報からR/O枝番を引数で受け、進捗バーを更新する.
* @param {String} R/O枝番
* @return {void}
*/
function setSrvAddSeq(srvAddSeq) {
    gWorkMeter.setMaterParameterSrvAddSeq(srvAddSeq);
}

/**
 * R/O情報から担当SA名を引数で受け、進捗バーを更新する.
 * @param {String} 担当SA名
 * @return {void}
 */
function setSaName(saName) {
    //選択されたチップ情報を作業進捗メータークラスに渡す.
    gWorkMeter.setMeterParameterSaName(saName);
    //渡したチップ情報を元に、作業進捗メーターを再描画する.
    gWorkMeter.refreshMeter();
}

// 2019/06/03 NSK 鈴木 18PRJ03142_(FS)業務検証:セールス受注前活動活動結果登録オペレーション[TKM]PUAT-4109 部品詳細にて、バックオーダー(B/O)が表示されない START
// /**
//  * R/O情報からタブ選択時に、選択したタブ情報と、部品準備完了情報を取得し、フッターの表示制御を行う.
//  *
//  * @param {Integer} intTabNumber 選択されたタブ番号
//  * @param {String} strPartsRepareFlag 部品準備完了フラグ
//  * @param {String} strPartsCount 部品情報数
//  * @param {String} strBackOrderCount B/O項目数
//  * @return {void}
//  */
// function CheckChengeTab(intTabNumber, strPartsCount, strBackOrderCount, strInspectionApproval) {
/**
 * R/O情報からタブ選択時に、選択したタブ情報と、部品準備完了情報を取得し、フッターの表示制御を行う.
 *
 * @param {Integer} intTabNumber 選択されたタブ番号
 * @param {String} strPartsRepareFlag 部品準備完了フラグ
 * @param {String} strPartsCount 部品情報数
 * @return {void}
 */
function CheckChengeTab(intTabNumber, strPartsCount, strInspectionApproval) {
// 2019/06/03 NSK 鈴木 18PRJ03142_(FS)業務検証:セールス受注前活動活動結果登録オペレーション[TKM]PUAT-4109 部品詳細にて、バックオーダー(B/O)が表示されない END

    //選択されているタブ番号を格納するフィールドに、取得したタブ番号を格納する.
    $("#HiddenSelectedTabNumber").val(intTabNumber);
    //部品情報数を格納する.
    $("#HiddenPartsCount").val(strPartsCount);

    // 2019/06/03 NSK 鈴木 18PRJ03142_(FS)業務検証:セールス受注前活動活動結果登録オペレーション[TKM]PUAT-4109 部品詳細にて、バックオーダー(B/O)が表示されない START
    // //B/O項目数を格納する.
    // $("#HiddenBackOrderCount").val(strBackOrderCount);
    // 2019/06/03 NSK 鈴木 18PRJ03142_(FS)業務検証:セールス受注前活動活動結果登録オペレーション[TKM]PUAT-4109 部品詳細にて、バックオーダー(B/O)が表示されない END

    //完成検査承認フラグを格納する。
    $("#HiddenFieldInspectionApprovalFlag").val(strInspectionApproval);

    controlFooterButton();
}

/**
 * R/O情報にて左フリックを実施した際に、呼び出されるメソッド.
 * HiddenにR/O情報にて左クリックされたフラグを格納し、ポストバックする.
 *
 * @return {void}
 */
function flickRepairOrderInfomation() {

    var selectedId = $("#HiddenSelectedId").val();
    var candidateId = $("#HiddenCandidateId").val();

    var status = "";
    var instruct = C_UNINSTRUCT;

    if (selectedId != "") {
        status = gArrObjChip[selectedId].chipResultStatus;
        instruct = gArrObjChip[selectedId].instruct;
    }

    //着工未指示の場合、イベントを発生させない
    if (instruct == undefined
     || C_UNINSTRUCT == instruct) {
        return false;
    }

    // 2013/12/12 TMEJ 成澤 【開発】IT9611_次世代サービス 工程管理機能開発 START
    //チップの選択状態を取得
    var _selectedChipStatus = $("#HiddenSelectedChip").val();
    //チップの選択状態が未選択なら、イベントを発生させない
      if (_selectedChipStatus == C_SELECTED_CHIP_OFF) {
          return false;
      }
      // 2013/12/12 TMEJ 成澤 【開発】IT9611_次世代サービス 工程管理機能開発 END

    // 初期選択チップ又は、実績チップ以外はフリックで遷移できない
    if (candidateId == ""
     || status == C_RESULT_STATUS_COMPLETION
     || selectedId == candidateId){
        //隠しボタンを押下し、画面遷移処理を開始する.
        //2016/03/30 NSK 小牟禮 アクティビティインジケータが消えない問題 START
		//reloadPageIfNoResponse();
        //$("#HiddenButtonFlickRepairOrder").click();

        //クルクル表示
        LoadingScreen();

        setTimeout(function () {
            $("#HiddenButtonFlickRepairOrder").click();
        }, 0);
        //2016/03/30 NSK 小牟禮 アクティビティインジケータが消えない問題 END
        
    }
}


/**
 * R/O情報呼び出し時に、R/O作業ステータスを設定するために呼び出されるメソッド
 * @param {string} aOrderStatus
 * @return {void}
*/
function setOrderStatus(aOrderStatus) {
    $("#HiddenOrderStatus").val(aOrderStatus);
}


/**
 * R/O情報にて、追加作業アイコンをタップした際に呼び出されるメソッド.
 * @param {Integer} tapIconNumber
 * @param {Integer} workSeq         作業連番
 * @return {void}
 */
function tapRepairOrderIcon(tapIconNumber, workSeq) {
    $("#HiddenFieldRepairOrderIcon").val(tapIconNumber.toString());
    //orderNumberを取得する.
    var orderNumber = $("#HiddenFieldOrderNo").val();
    //空文字を除去
    orderNumber = trimString(orderNumber);

    //orderNumberがない場合、処理を実施しない.
    if (orderNumber != "") {
        //追加作業アイコン番号と活性状態にある追加作業番号が一致する場合も遷移処理しない.
        if (tapIconNumber != workSeq) {
            //隠しボタンを押下し、画面遷移処理を開始する.
            //2016/03/30 NSK 小牟禮 アクティビティインジケータが消えない問題 START
		    //reloadPageIfNoResponse();
            //$("#HiddenButtonRepairOrderIcon").click();

            //クルクル表示
            LoadingScreen();

            setTimeout(function () {
                $("#HiddenButtonRepairOrderIcon").click();
            }, 0);
            //2016/03/30 NSK 小牟禮 アクティビティインジケータが消えない問題 END
        }
    }
}


/**
 * R/O情報にて、履歴情報をタップした際の処理.
 * @param {String} selectedOrderNumber
 * @param {String} selectedDealerCode
 * @return {void}
*/
//2012/11/30 TMEJ 小澤【SERVICE_2】SAストール予約受付機能開発（仕分けNo.75）START
//function tapHistory(selectedOrderNumber) {
function tapHistory(selectedOrderNumber, selectedDealerCode,  selectedServiceInNumder) {
//2012/11/30 TMEJ 小澤【SERVICE_2】SAストール予約受付機能開発（仕分けNo.75）END

    //取得したR/O番号がブランクの場合、処理を実施しない.
    if (selectedOrderNumber != "") {
        //履歴情報がタップされた行のR/O番号と販売店CDを格納する.
        $("#HiddenHistoryOrderNumber").val(selectedOrderNumber);
        //2012/11/30 TMEJ 小澤【SERVICE_2】SAストール予約受付機能開発（仕分けNo.75）START
        $("#HiddenHistoryDealerCode").val(selectedDealerCode);
        //2012/11/30 TMEJ 小澤【SERVICE_2】SAストール予約受付機能開発（仕分けNo.75）END

        //2013/12/12 TMEJ 成澤 【開発】IT9611_次世代サービス 工程管理機能開発 START
        //履歴情報がタップされた行の入庫管理番号を格納
        $("#HiddenServiceInNumber").val(selectedServiceInNumder);
        //2013/12/12 TMEJ 成澤 【開発】IT9611_次世代サービス 工程管理機能開発 END
        //隠しボタンを押下し、画面遷移処理を開始する.
        
        //2016/03/30 NSK 小牟禮 アクティビティインジケータが消えない問題 START
		//reloadPageIfNoResponse();
        //$("#HiddenButtonHistory").click();

        //クルクル表示
        LoadingScreen();

        setTimeout(function () {
            $("#HiddenButtonHistory").click();
        }, 0);
        //2016/03/30 NSK 小牟禮 アクティビティインジケータが消えない問題 END
    }
}


/**
 * 文字列より空白文字を除去
 *
 * @param {String} 対象文字列
 * @return {String} 空白文字を除去した対象文字列
*/
function trimString(targetString) {

    var trimAfter = "";
    trimAfter = targetString.replace(/^[\s　]+|[\s　]+$/g, "");

    return trimAfter;
}


/**
 * スクロールの初期位置を設定する
 *
 * @return {void}
 */
function initBox01GraphBoxScroll() {

    var drawPointX = 0;

    if ($("#HiddenScrollLeft").val() == "") {
        //↓これでスクロールの初期位置を指定可能
        //あとは、スクロールしておく適正値を算出して渡すようにすればOK
        var dtmScrollTime = getServerTimeNow();
        dtmScrollTime.setHours(dtmScrollTime.getHours() - 1, 0, 0);

        var _candidateStartTime = dtmScrollTime;
        //所持しているチップオブジェクトをループ処理
        for (var key in gArrObjChip) {
            //作業対象チップである場合、その描画開始日時を取得
            if (gArrObjChip[key].chipId == $("#HiddenCandidateId").val()) {
                _candidateStartTime = gArrObjChip[key].chipDrawStartTime;
                break;
            }
        }

        //スクロール補正日時と作業対象チップの描画開始日時を比較して、小さい方でスクロール位置を算出する
        if (dtmScrollTime > _candidateStartTime) {
            drawPointX = getDrawPositionX(_candidateStartTime);
        } else {
            drawPointX = getDrawPositionX(dtmScrollTime);
        }
    }
    else {
        drawPointX = $("#HiddenScrollLeft").val();
    }

    //初期位置をスクロールして決定する
    $('#Box01GraphBox').scrollLeft(drawPointX);
}


/**
 * チップ情報を解析し、配置する.
 * @return {void}
 */
function createChipObject() {
    //JSON形式のチップ情報読み込み.
    var jsonData = $("#HiddenJsonData").val();

    //チップ情報の更新時刻に設定する現在時刻を取得する.
    var dtmUpdateTime = getServerTimeNow();
    //JSON形式のデータを変換し、処理する.
    chipDataList = $.parseJSON(jsonData);

    //取得したチップ情報をチップクラスに格納し、再描画.
    var lngIndex = 0;
    for (var keyString in chipDataList) {
        var chipData = chipDataList[keyString];

        var strKey = chipData.REZID + "_" + chipData.SEQNO + "_" + chipData.DSEQNO;
        //var strKey = value.REZID + "_" + value.DSEQNO;
        if (gArrObjChip[strKey] == undefined) {
            gArrObjChip[strKey] = new ReserveChip(strKey, gGraphBoxStartTime, gGraphBoxEndTime);
        }
        gArrObjChip[strKey].setChipParameter(chipData);
        //チップ生成に成功する場合、更新日時を設定する.
        if (gArrObjChip[strKey].createChip()) {
            gArrObjChip[strKey].setUpdateTime(dtmUpdateTime);
        }
        checkBlinkChip(gArrObjChip[strKey]);
    }
    //所持しているチップオブジェクトをループ処理.
    for (var key in gArrObjChip) {
        //更新時刻が今回の更新時刻に一致しないチップオブジェクトを破棄する.
        if (gArrObjChip[key].dtmUpdateTime != dtmUpdateTime) {
            $("#" + gArrObjChip[key].chipId + "_BASE").remove();
            $("#" + gArrObjChip[key].chipId).remove();
            delete gArrObjChip[key];
        }
    }
    //チップのタップイベントを再バインドする.
    $(".ChipsBaseFilter").unbind("touchstart click");
    $(".ChipsBaseFilter").bind("touchstart click", function () {
        //タップ処理を行う.
        tapChip(this);
    });
}


/**
 * チップ選択フラグをHiddenより取得し
 * チップ選択されていない場合は、ストール情報欄を初期化する
 *
 * @return {void}
 */
function initSelectedChip() {

    //現在選択されているチップIDを取得する.
    var strSelectedId = $("#HiddenSelectedId").val().toString();

    // 選択されているチップがあるかチェック
    var isExists = false;
    var strChipId = "";

    for (var key in gArrObjChip) {
        strChipId = gArrObjChip[key].chipId;
        if(strSelectedId == strChipId) {
            isExists = true;
            break;
        }
    }

    //選択されているチップIDが空白でない場合、チップのタップイベントを実施する.
    if (strSelectedId && isExists) {
        //タップイベントにて、チップの選択状態が切り替えられるため、
        //前と同様の状態が表示されるように事前にチップの選択状態を切り替えておく
        toggleSelectedChip();
        tapChip($("#" + strSelectedId));
    }
    //作業対象のチップIDが指定されている場合のみ処理を実施する
    else if ($("#HiddenCandidateId").val()) {
        //作業対象チップIDの情報を表示した状態で、チップ選択状態にするため、タップイベントを実施する.
        tapChip($("#" + $("#HiddenCandidateId").val()));
    }
    //作業対象となるチップIDが存在しない場合、作業進捗メータの再描画とフッタボタンの表示制御を行う
    //本来はタップイベントにて実施していたが、処理を通らないのでここで実施する
    else {
        tapChip(null);
    }
}


/**
 * 開始時間からBox01GraphCassetteを生成する数を取得する.
 *
 * @return {void}
 */
function getScaleCount() {

    //ストールの開始時間と終了時間の時間差を算出する.
    var lngStallWorkTime = gGraphBoxEndTime - gGraphBoxStartTime;
    //ストールの稼働時間より、Box01GraphCassetteの描画数を設定する.
    gGraphCassetteCount = Math.ceil(lngStallWorkTime / C_CASSETTE_PITCH);
    //キャンバスの幅を設定する.
    gCanvasWidth = gGraphCassetteCount * C_CASSETTE_WIDTH;
}


/**
 * Box01GraphLineの初期化を行う.
 *
 * @return {void}
 */
function initBox01GraphLine() {

    //Box01GraphLineの幅を再設定する.
    $("#Box01GraphLine").css("width", gCanvasWidth.toString() + "px");
    //併せてBox01GraphLineFilterの幅も再設定する.
    $("#Box01GraphLineFilter").css("width", gCanvasWidth.toString() + "px");
}


/**
 * 所持しているチップ、Box01GraphLineのスモークフィルタの透過度を変更する.
 *
 * @param {Integer} intValue スモークフィルタの透過度（0～1）
 * @return {void}
 */
function setChipLineGrayFilter(intValue) {
    //所持しているチップオブジェクトをループ処理.
    for (var key in gArrObjChip) {
        gArrObjChip[key].setChipFilter(true);
    }
    //Box01GraphLineのグレーフィルターの透過度を設定.
    $("#Box01GraphLineFilter").css("opacity", intValue);
}



/**
 * 時間スケールの生成処理.
 *
 * @return {void}
 */
function createScale() {

    //開始・終了時間からClassGraph01BoxCassetteを作成する総数を取得する.
    getScaleCount();
    //Box01GraphLineを初期設定する（幅を設定する）.
    initBox01GraphLine();

    //時間スケールを描画する親要素を取得する.
    var objBox01GraphLine = $("#Box01GraphLine");

    //Box01GraphBoxの描画開始時間.
    var dtmDrawTime = getServerTimeNow();

    //Box01GraphCassetteの生成総数だけループ処理を実施する.
    for (var lngCount = 0; lngCount < gGraphCassetteCount; lngCount++) {
        //描画時刻を取得する.
        dtmDrawTime.setTime(gGraphBoxStartTime.getTime() + (C_CASSETTE_PITCH * lngCount));
        //<div>要素を新規作成し、時刻カセットのCSSクラス（Box01GraphCassette）を与える.
        var objCassette = $("<div />").addClass("Box01GraphCassette");
        //作成したオブジェクトにメモリ表示用の時間を設定する.
        objCassette.text(dtmDrawTime.getHours().toString() + ":00");
        //作成したオブジェクトの描画位置を設定する.
        //objCassette.css("left", getDrawPositionX(dtmDrawTime).toString() + "px");
        var drawX = getDrawPositionX(dtmDrawTime);
        objCassette.css("left", getDrawPositionX(dtmDrawTime).toString() + "px");
        objCassette.css("top", "-15px");
        //作成したオブジェクトを子要素として親要素に追加する.
        objBox01GraphLine.append(objCassette);
    } //End for

}


/**
 * 現在時刻のCSSを再配置する.
 *
 * @return {void}
 */
function setCurrentBoxPosition() {

    //現在時刻をあらわす線の要素を取得する.
    var element = document.getElementById("CurrentBox");

    //現在時刻を取得する.
    var dtmNow = getServerTimeNow();
    //取得した現在時刻が、ストール稼働時間の範囲外である場合、現在時刻線を表示しない.
    if ((dtmNow < gGraphBoxStartTime) || (gGraphBoxEndTime < dtmNow)) {

        //現在時刻場所を表示しないように設定する.
        element.style.visibility = 'hidden';
    }
    else {
        //現在時刻場所を表示するように設定する.
        element.style.visibility = 'visible';
        //現在時刻場所を絶対座標に設定.
        element.style.position = 'absolute';

        //13/08/08 TMEJ 成澤 【A.STEP2】タブレット版SMB開発に向けた要件定義  START
        //チップより上に描画する
        $("#CurrentBox").css("z-index", "9");
        //13/08/08 TMEJ 成澤 【A.STEP2】タブレット版SMB開発に向けた要件定義  END

        //現在時刻バーを現在時刻に合致した箇所に設置.
        element.style.left = getDrawPositionX(dtmNow).toString() + "px";
        //jQueryにて現在時刻を変更.
        //$("#CurrentBoxTime").text(formatTime(dtmNow.getHours()) + ":" + formatTime(dtmNow.getMinutes()));
        //時間強調を現在の時刻帯に設定.
        //var dtmStrongTime = new Date;
        var dtmStrongTime = getServerTimeNow();
        dtmStrongTime.setMinutes(0);
        dtmStrongTime.setSeconds(0);
        var positionX = getDrawPositionX(dtmStrongTime).toString() + "px";
        $("#CurrentBoxTime").css("left", getDrawPositionX(dtmStrongTime, dtmNow).toString() + "px");
        //2012/05/16 KN 森下【SERVICE_1】号口課題管理表 No.25 ストールエリアの時間帯表示を現在時刻に変更 START
        //現在時刻バーを更新
        //$("#CurrentBoxTime").text(formatTime(dtmStrongTime.getHours()) + ":" + formatTime(dtmStrongTime.getMinutes()));
        $("#CurrentBoxTime").text(formatTime(dtmNow.getHours()) + ":" + formatTime(dtmNow.getMinutes()));
        //2012/05/16 KN 森下【SERVICE_1】号口課題管理表 No.25 ストールエリアの時間帯表示を現在時刻に変更 END
    }
}


/**
 * 描画対象の時刻を引数として、描画開始X座標値を返す
 *
 * @param {Date} dtmTime 描画開始時刻（ストール開始時刻）
 * @param {Date} dtmSTime 描画対象の描画開始時刻
 * @return {Integer} 描画開始X座標値
 */
function getDrawPositionX(dtmTime, dtmSTime) {

    var dtmDrawStartTime = dtmSTime;
    if (dtmSTime == undefined) {
        dtmDrawStartTime = gGraphBoxStartTime;
    }
    //描画開始時刻と描画対象の時刻をの分差を取得する
    var lngDiffMinutes = compareMinutes(dtmTime, dtmDrawStartTime);
    //描画開始X座標は、分差に1分ごとのpx数を乗算して算出する
    var lngDrawX = (lngDiffMinutes * (C_CASSETTE_WIDTH / 60));

    return lngDrawX;
}


/**
 * 描画開始時刻と、格納された時刻の分差を算出する.
 *
 * @param {Date} dtmTime
 * @param {Date} dtmDrawStartTime
 * @return {Integer} パラメータ1、2の時間差を分単位で返す.
 */
function compareMinutes(dtmTime, dtmDrawStartTime) {

    //秒数を0にして、描画開始時刻を取得する
    var dtmTime1 = dtmDrawStartTime;
    dtmTime1.setSeconds(0);
    //秒数を0として、格納された時刻を取得する
    var dtmTime2 = dtmTime;
    dtmTime2.setSeconds(0);
    //互いの差分を求める
    var dtmDiff = dtmTime2 - dtmTime1;
    //取得した値を経過分数に変換する
    var lngDiffMinutes = Math.floor(dtmDiff / (60 * 1000))

    return lngDiffMinutes;
}


//1桁の数値を2桁の文字列として返す
function formatTime(lngValue) {

    var strResult = lngValue.toString();

    if (lngValue < 10) {
        strResult = "0" + strResult;
    }

    return strResult;
}


/**
 * Push通信の際に呼び出されるイベント
 *
 * @return {void}
 */
function CallPushEvent() {

    //2016/03/30 NSK 小牟禮 アクティビティインジケータが消えない問題 START
    //画面をリフレッシュするイベントを実施する.
	//reloadPageIfNoResponse();
    //$("#HiddenButtonRefresh").click();

    if ($("#HiddenReloadFlag").val() == "1") {
        //リロード中の場合は、以後の処理を行わない
        return;
    }

    //クルクル表示
    LoadingScreen();

    //画面をリフレッシュするイベントを実施する.
    setTimeout(function () {
        $("#HiddenButtonRefresh").click();
    }, 0);
    //2016/03/30 NSK 小牟禮 アクティビティインジケータが消えない問題 END
}


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

/**
* フッターボタン制御
*/
function FooterButtonClick() {

    if ($("#HiddenReloadFlag").val() == "1") {
        //リロード中の場合は、以後の処理を行わない
        $("#bodyFrame").unbind(".popover");
        return false;
    }

    //2016/03/30 NSK 小牟禮 アクティビティインジケータが消えない問題 START
    //$("#HiddenReloadFlag").val("1");
    //$.master.OpenLoadingScreen();
    //2016/03/30 NSK 小牟禮 アクティビティインジケータが消えない問題 END

    return true;
}

//2016/03/30 NSK 小牟禮 アクティビティインジケータが消えない問題 START
//フッターボタン押下時の事前処理
//くるくるアイコンも表示する場合、こちらの関数を利用する
function FooterButtonClickAndLoadingScreen() {

    if ($("#HiddenReloadFlag").val() == "1") {
        //リロード中の場合は、以後の処理を行わない
        $("#bodyFrame").unbind(".popover");
        return false;
    }

    //クルクル表示
    LoadingScreen();
    
    return true;
}

///**
//* リロードフラグ初期化
//*/
//function InitReloadFlag() {
//    $("#HiddenReloadFlag").val("");
//
////    if ($("#ButtonConnectParts")[0].style.display != "none") {
////        //部品連絡ポップアップ処理.
////        //$("#ButtonConnectParts").popover({
////        $("#ButtonConnectParts").popover({
////            id: "ButtonConnectParts",
////            offsetX: 0,
////            offsetY: 0,
////            preventLeft: true,
////            preventRight: true,
////            preventTop: false,
////            preventBottom: true,
////            content: "<div id='ButtonConnectParts_content' />",
////            //header: "<div id='ButtonConnectParts_header'><p>" + "部品連絡" + "</p></div>",
////            header: getPopoverHeaderContents(),
////            openEvent: function () {
////                var container = $('#ButtonConnectParts_content');
////                //var $iframe = $("<iframe frameborder='0' id='ButtonConnectParts_Frame' width='407px' height='329px' scrolling='no' src='../Pages/SC3190303.aspx' />");
////                var $iframe = $("<iframe frameborder='0' id='ButtonConnectParts_Frame' width='395px' height='280px' scrolling='no' src='../Pages/SC3190303.aspx' />");
////                container.empty().append($iframe);
////            }
////        });
//
////        $("#ButtonConnectParts_cancel").bind("click.popover", function (event) {
////            ParentPopoverClose();
////        });
////    }
//}
//
//function StopLodingIcon(itemName) {
//    $(itemName).hide(0);
//}
//2016/03/30 NSK 小牟禮 アクティビティインジケータが消えない問題 END

function LoadingScreen() {
    $.master.OpenLoadingScreen();

    //2016/03/30 NSK 小牟禮 アクティビティインジケータが消えない問題 START
    //クルクルタイムアウト処理
    reloadPageIfNoResponse();
    //リロードフラグをONに設定
    $("#HiddenReloadFlag").val("1");
    //2016/03/30 NSK 小牟禮 アクティビティインジケータが消えない問題 END
}

//2016/03/30 NSK 小牟禮 アクティビティインジケータが消えない問題 START
//読み込みを行うが、くるくるアイコンを表示しない場合、こちらの関数を利用する
function LoadingScreenNoIcon() {
    //クルクルタイムアウト処理
    reloadPageIfNoResponse();
    //リロードフラグをONに設定
    $("#HiddenReloadFlag").val("1");
}
//2016/03/30 NSK 小牟禮 アクティビティインジケータが消えない問題 END

function UnloadingScreen() {
    $.master.CloseLoadingScreen();

    //クルクル非表示（R/O情報）
    $("#loadingroInfomation").hide(0);

    //2016/03/30 NSK 小牟禮 アクティビティインジケータが消えない問題 START
    //再表示タイマーをリセット
    clearTimer();
    //リロードフラグをOFFに設定
    $("#HiddenReloadFlag").val("");
    //2016/03/30 NSK 小牟禮 アクティビティインジケータが消えない問題 END
}

function getPartsReadyFlag() {

    // 選択中のチップ取得
    var selectedId = $("#HiddenSelectedId").val();

    // 部品準備完了フラグ取得
    var partsReadyFlag = "";

    if (selectedId != "") {
        partsReadyFlag = gArrObjChip[selectedId].merchandiseFlag;
    }

    return partsReadyFlag;
}

/*
* 作業終了フラグ取得
* 作業終了直後、チップの選択が変わらないためR/O情報欄が再描画されなく、グレーフィルターがかからないため追加
*/
function getEndWorkFlg() {

    var endWorkFlg = $("#HiddenFieldEndWorkFlg").val();

    // フラグ初期化
    $("#HiddenFieldEndWorkFlg").val("");

    return endWorkFlg;
}

function confirmBeforeRedirectSC3170203() {

	//2016/03/30 NSK 小牟禮 アクティビティインジケータが消えない問題 START
	//clearTimer();
	//2016/03/30 NSK 小牟禮 アクティビティインジケータが消えない問題 END
	
    var result = confirm($("#HiddenAddWorkConfirmWord").val());
    if (result) {
        //2016/03/30 NSK 小牟禮 アクティビティインジケータが消えない問題 START
		//reloadPageIfNoResponse();
        //HiddenButtonRedirectSC3170203.click();

        LoadingScreen();

        setTimeout(function () {
            HiddenButtonRedirectSC3170203.click();
        }, 0);
        //2016/03/30 NSK 小牟禮 アクティビティインジケータが消えない問題 END
    }
}

function confirmBeforeRedirectSC3170201() {

	//2016/03/30 NSK 小牟禮 アクティビティインジケータが消えない問題 START
	//clearTimer();
	//2016/03/30 NSK 小牟禮 アクティビティインジケータが消えない問題 END
	
    var result = confirm($("#HiddenAddWorkConfirmWord").val());
    if (result) {
        //2016/03/30 NSK 小牟禮 アクティビティインジケータが消えない問題 START
		//reloadPageIfNoResponse();
        //HiddenButtonRedirectSC3170201.click();

        LoadingScreen();

        setTimeout(function () {
            HiddenButtonRedirectSC3170201.click();
        }, 0);
        //2016/03/30 NSK 小牟禮 アクティビティインジケータが消えない問題 END
    }
}

//2013/02/21 TMEJ 成澤【A.STEP1】TC着工指示オペレーション確立に向けた評価アプリ作成 START


//スクリーンセイバー画面遷移
function ScreenSeverLoad() {

    //2016/03/30 NSK 小牟禮 アクティビティインジケータが消えない問題 START
    //クルクル表示
    //$.master.OpenLoadingScreen();
    //TCステータスモニターへ遷移
    //setTimeout(function () { HiddenButtonRedirectSC3150201.click() }, 100);
    //reloadPageIfNoResponse();

    if ($("#HiddenReloadFlag").val() == "1") {
        //リロード中の場合は、以後の処理を行わない
        return;
    }
    
    //クルクル表示
    LoadingScreen();
    //TCステータスモニターへ遷移
    setTimeout(function () { HiddenButtonRedirectSC3150201.click() }, 100);
    //2016/03/30 NSK 小牟禮 アクティビティインジケータが消えない問題 END
}

//スクリーンセイバータイマースタートメソッド
function ScreenTimerStart() {
    //DBから取得した秒数を格納
    secondNum = document.getElementById("HiddenTcStatusStandTime").value
    //タイマースタート
    screenSeverTimer = setInterval("ScreenSeverLoad()", secondNum * 1000);
}

//スクリーンセイバータイマーリスタートメソッド
function ScreenTimerRestart() {
    
    //遷移タイマーリセット
    clearInterval(screenSeverTimer);
    ScreenTimerStart();
}
//2013/02/21 TMEJ 成澤【A.STEP1】TC着工指示オペレーション確立に向けた評価アプリ作成 END


//2014/07/23 TMEJ 成澤　【開発】IT9711_タブレットSMB Job Dispatch機能開発 START
/**
*ALLSTART、ALLFINISHタップ時の確認用ポップアップ
* @return {void}
*/
function RunConfirmation(buttonFlg) {

    //変数宣言
    var button;
    var text;

    //ALLSTARTタップ時の処理
    if (buttonFlg) {
        text = $("#HiddenConfirmStartWording").val();
        button = $("#HiddenButtonStartWork");
    //ALLFINISHタップ時の処理
    } else {
        text = $("#HiddenConfirmFinishWording").val();
        button = $("#HiddenButtonFinishWork");
    }

    
    // 確認ダイアログの表示
    if (window.confirm(text)) {
        //中断作業の開始フラグを設定
        $("#HiddenRestartStopJobFlg").val("0");

        //2016/03/30 NSK 小牟禮 アクティビティインジケータが消えない問題 START
        // 「OK」時の処理終了
        //button.click();

        //クルクル表示
        LoadingScreen();

        // 「OK」時の処理終了
        setTimeout(function () {
            button.click();
        }, 0);
        //2016/03/30 NSK 小牟禮 アクティビティインジケータが消えない問題 END
    } else {
        //「キャンセル」時の処理

        //ALLSTARTタップ時の処理
        if (buttonFlg) {
             //中断作業の開始フラグを設定
             $("#HiddenRestartStopJobFlg").val("1");

            //2016/03/30 NSK 小牟禮 アクティビティインジケータが消えない問題 START
            //button.click();

            //クルクル表示
            LoadingScreen();

            setTimeout(function () {
                button.click();
            }, 0);
            //2016/03/30 NSK 小牟禮 アクティビティインジケータが消えない問題 END
        }else{

        }
    }

}
//2014/07/23 TMEJ 成澤　【開発】IT9711_タブレットSMB Job Dispatch機能開発 END

//2014/09/12 TMEJ 成澤  自主研追加対応_ROプレビュー遷移 START
/**
* R/O情報にて、R/Oアイコンをタップした際に呼び出されるメソッド.
* @param {Integer} selectedOrderNumber　　　　RO番号
* @param {Integer} selectedOrderNumberSeq     RO枝番
* @return {void}
*/
function newTapRepairOrderIcon(selectedOrderNumber, selectedOrderNumberSeq) {
    
    //取得したR/O番号がブランクの場合、処理を実施しない.
    if (selectedOrderNumber != "") {
        //アイコンがタップされた行のR/O番号とR/O枝番
        $("#HiddenHistoryOrderNumber").val(selectedOrderNumber);
        $("#HiddenHistoryOrderNumberSeq").val(selectedOrderNumberSeq);

        //2016/03/30 NSK 小牟禮 アクティビティインジケータが消えない問題 START
        ////隠しボタンを押下し、画面遷移処理を開始する.
        //reloadPageIfNoResponse();
        //$("#HiddenButtonRepairOrderIcon").click();

        //クルクル表示
        LoadingScreen();

        //隠しボタンを押下し、画面遷移処理を開始する.
        setTimeout(function () {
            $("#HiddenButtonRepairOrderIcon").click();
        }, 0);
        //2016/03/30 NSK 小牟禮 アクティビティインジケータが消えない問題 END
    }
}
//2014/09/12 TMEJ 成澤  自主研追加対応_ROプレビュー遷移 END
