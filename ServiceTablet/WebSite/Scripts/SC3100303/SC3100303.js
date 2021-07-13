//------------------------------------------------------------------------------
//SC3100303.js
//------------------------------------------------------------------------------
//機能：来店管理_javascript
//補足：
//作成：2012/03/04 TMEJ 張
//更新：2012/04/25 TMEJ 張  ITxxxx_TSL自主研緊急対応（サービス）
//更新：2012/05/05 TMEJ 小澤  ITxxxx_TSL自主研緊急対応（サービス）3回目
//更新：2018/02/20 NSK  山田 REQ-SVT-TMT-20170615-001 来店管理に入庫予約を指定してROを発行する機能を追加
//更新：
//------------------------------------------------------------------------------

// -------------------------------------------------------------
// メイン処理
// -------------------------------------------------------------

// グロバール変数
var gArrObjChip;                // チップクラス配列の初期化
var gSelectedChipId = "";       // 選択したチップのId
var gSelectedObjChip = null;    // 選択したチップの構造体
var gServerTimeDifference = 0;  // ページ取得時のサーバとクライアントの時間差
var gStartWorkTime;             // 営業開始時間
var gEndWorkTime;               // 営業終了時間
var gDelayTime;                 // 遅刻時間(秒)
var gRefreshTime;               // リフレッシュ時間(秒)
var gColNums;                   // コラム数
var gTouchStartFlg = false;     // chipTapイベント用フラグ
var gTimerInterval;             // setIntervalの戻り値
var gSC3100303WordIni;          // 文言

// 定数
var C_SELECTEDCHIPID = "SelectedChipId"; // 選択したチップid
var C_HOUR_WIDTH = 322;         // 1時間の幅
var C_TB_WIDTH = 983;           // テーブルの幅
var C_TB_HEIGHT = 584;          // テーブルの高さ
var C_NO_ERROR = 0;             // エラー情報

var C_TOUCH_START = "touchstart mousedown"; // タッチ開始
var C_TOUCH_MOVE = "touchmove mousemove";   // タッチで移動
var C_TOUCH_END = "touchend mouseup";       // タッチ終わり

var C_CP_ST_RED = 1;        // 赤い色チップ
var C_CP_ST_BLUE = 2;       // 青い色チップ
var C_CP_ST_CALL = 4;       // コールマーク

var C_OPECD_SA = 9;         // SA権限
var C_OPECD_SM = 10;        // SM権限
var C_OPECD_SVR = 52;       // 受付係権限

var C_WORD_SEX_MAN = 7;    // 文言：様（男性向け）
var C_WORD_SEX_WOMEN = 8;  // 文言：様（女性向け）
var C_WORD_MON = 11;       // 文言：月
var C_WORD_SUN = 17;       // 文言：日
//2013/04/25 TMEJ 張 ITxxxx_TSL自主研緊急対応（サービス） START
var C_WORD_VST_CAR_CNT = 18;       // 文言：来店実績:N台
//2013/04/25 TMEJ 張 ITxxxx_TSL自主研緊急対応（サービス） END
// 2018/02/20 NSK  山田 REQ-SVT-TMT-20170615-001 来店管理に入庫予約を指定してROを発行する機能を追加 START
var C_CST_TYPE_1 = "1";            // 顧客種別1：自社客
var C_WORD_RO_MSG = 904;           // R/O作成確認メッセージ
var C_WORD_RO_NG_MSG = 905;        // R/O作成NGメッセージ
// 2018/02/20 NSK  山田 REQ-SVT-TMT-20170615-001 来店管理に入庫予約を指定してROを発行する機能を追加 END
//チップタップイベント
//・ドラッグ時は動作しない
//・ダブルタップ時間内に再度タップがあれば動作しない
jQuery.event.special.chipTap = {
    setup: (function () {
        return function () {
            var touchStart = false;
            var touchMove = false;
            var singleTap = false;

            $(this).bind(C_TOUCH_START, function (event) {
                if (event.type == 'touchstart') {
                    gTouchStartFlg = true;
                } else {
                    if (gTouchStartFlg) {
                        return;
                    }
                }
                touchStart = true;
                touchMove = false;
                singleTap = !singleTap;
            });

            $(this).bind(C_TOUCH_MOVE, function (event) {
                if (event.type == 'touchmove') {
                } else {
                    if (gTouchStartFlg) {
                        return;
                    }
                }
                if (!touchStart) {
                    return;
                }

                touchMove = true;
                singleTap = false;  //タッチムーブ後にダブルタップした際、chipTap処理をしないよう制御
            });

            $(this).bind(C_TOUCH_END, function (event) {
                if (event.type == 'touchend') {
                } else {
                    if (gTouchStartFlg) {
                        return;
                    }
                }
                if (!touchStart) {
                    return;
                }
                if (touchMove) {
                    return;
                }

                touchStart = false;
                touchMove = false;

                var obj = $(this);
                obj.trigger("chipTap");
            });
        }
    })()
}

/**
* 初期表示
* @return {void}
*/
$(function () {

    // 営業開始終了時間を取得
    var strStartWorkTime = $("#hidShowDate").val() + " " + $("#hidStallStartTime").val();
    gStartWorkTime = new Date(strStartWorkTime);
    var strEndWorkTime = $("#hidShowDate").val() + " " + $("#hidStallEndTime").val();
    gEndWorkTime = new Date(strEndWorkTime);
    if (gEndWorkTime - gStartWorkTime < 0) {
        gEndWorkTime.setDate(gEndWorkTime.getDate() + 1);
    }
    // 遅刻とリフレッシュ時間を取得
    gDelayTime = parseInt($("#hidDelayTime").val());
    gRefreshTime = parseInt($("#hidRefreshTime").val());

    // 受付係の場合、右上の検索ボックスが表示されない
    if ($("#hidOpeCD").val() == C_OPECD_SVR) {
        $("#MstPG_CustomerSearchArea").css("display", "none");
    }

    // クライアントで取得できる時間とサーバ取得時間との差を設定する.
    SetServerTimeDifference();

    // 文言を取得する
    GetSC3100303WordIni();

    //フッターアプリの起動設定
    SetFooterApplication();

    // グルグルを表示
    gMainAreaActiveIndicator.show();
    //リフレッシュタイマーセット
    commonRefreshTimer(RefreshWindow);

    // 列数を取得
    if (gEndWorkTime.getMinutes() > 0) {
        // 終了時間が20:01の場合、21:00まで表示される
        gColNums = gEndWorkTime.getHours() + 1 - gStartWorkTime.getHours();
    } else {
        gColNums = gEndWorkTime.getHours() - gStartWorkTime.getHours();
    }

    if (gColNums <= 0) {
        gColNums += 24;
    }

    // テーブルを生成する
    CreateTable();


    //フッター「顧客詳細ボタン」クリック時の動作
    $('#MstPG_FootItem_Main_700').bind("click", function (event) {

        $('#MstPG_CustomerSearchTextBox').focus();

        event.stopPropagation();
    });

    // チップを取得
    GetVstChips();
});

/**
* フッターボタンのクリックイベント.
* @return {}
*/
function FooterButtonClick(Id) {
    //顧客詳細ボタンの場合は何もしない
    if (Id == 700) {
        return false;
    }
}

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
* HTMLのencode
* @param {String} encode前の値
* @return {String} encode後の値
*/
function htmlEncode(text) {
    return text.replace(/&/g, '&amp').replace(/\"/g, '&quot;').replace(/</g, '&lt;').replace(/>/g, '&gt;');
}
/**
* HTMLのdecode
* @param {String} decode前の値
* @return {String} decode後の値
*/
function htmlDecode(text) {
    return text.replace(/&amp;/g, '&').replace(/&quot;/g, '"').replace(/&lt;/g, '<').replace(/&gt;/g, '>');
}
/**
* Stringのleft関数
* @param {Date} mainStr 元のstr
* @param {Date} lngLen 左から何桁目
* @return {Integer} format後の値
*/
function left(mainStr, lngLen) {
    if (mainStr == null) {
        return null;
    }
    if (lngLen > 0) {
        return mainStr.substring(0, lngLen);
    } else {
        return null;
    }
}

/**
* Stringのright関数
* @param {Date} mainStr 元のstr
* @param {Date} lngLen 右から何桁目
* @return {Integer} format後の値
*/
function right(mainStr, lngLen) {
    if (mainStr == null) {
        return null;
    }
    if (mainStr.length - lngLen >= 0 && mainStr.length >= 0 && mainStr.length - lngLen <= mainStr.length) {
        return mainStr.substring(mainStr.length - lngLen, mainStr.length);
    } else {
        return null;
    }
}

/**
* サーバとの時間差を算出し、グローバル変数として格納する.
* @return {void}
*/
function SetServerTimeDifference() {

    //ページ読込時のサーバ時間を取得する.
    var dtServerTime = new Date($("#hidServerTime").val());
    //クライアントの現在時刻を取得する.
    var dtClientTime = new Date();

    //サーバとの時間差を算出し、格納する（ミリ秒）.
    gServerTimeDifference = dtServerTime - dtClientTime;
}

/**
* サーバの現在時刻を算出し、返す.
* @return {Date} 現在時刻
*/
function GetServerTimeNow() {

    //サーバの現在時刻を算出する.
    var dtServerTime = new Date();

    dtServerTime.setTime(dtServerTime.getTime() + gServerTimeDifference);
    return dtServerTime;
}

/**
* カレンダーに今日の日付を設定する
* @return {void}
*/
function ShowCalendar() {
    // 営業開始終了時間を取得
    var strStartWorkTime = $("#hidShowDate").val() + " " + $("#hidStallStartTime").val();
    gStartWorkTime = new Date(strStartWorkTime);
    var strEndWorkTime = $("#hidShowDate").val() + " " + $("#hidStallEndTime").val();
    gEndWorkTime = new Date(strEndWorkTime);
    if (gEndWorkTime - gStartWorkTime < 0) {
        gEndWorkTime.setDate(gEndWorkTime.getDate() + 1);
    }
}

/**
* 前日ボタンを押すイベント
* @return {なし}
*/
function imgbtnPrevDate_onClick() {
    // 今日の場合、前日に遷移不可
    if (IsTodayPage() == true) {
        return false;
    }
    // カレンダーの矢印ボタンクリック
    ClickChangeDate(-1);
    return false;
}

/**
* 翌日ボタンを押すイベント
* @return {なし}
*/
function imgbtnNextDate_onClick() {
    // カレンダーの矢印ボタンクリック
    ClickChangeDate(1);
    return false;
}

/**
* カレンダーの矢印ボタンクリック
* @param {int} 変更日付数
* @return {-} 
*/
function ClickChangeDate(nDays) {

    // チップが選択した場合、解除する
    if (gSelectedChipId != "") {
        TapChip("");
    }

    // グルグルを表示する
    gMainAreaActiveIndicator.show();
    //リフレッシュタイマーセット
    commonRefreshTimer(RefreshWindow);

    var dtShowDate = new Date($("#hidShowDate").val());
    // クリックした日付を計算する
    dtShowDate.setDate(dtShowDate.getDate() + nDays);
    // ハイドンコントロールに最新の日付を設定する
    $("#hidShowDate").val(dtShowDate.getFullYear() + "/" + add_zero((dtShowDate.getMonth() + 1)) + "/" + add_zero(dtShowDate.getDate()));

    // チップを取得する
    GetVstChips();
}

/**
* 画面をリフレッシュ(チップを削除して、再表示する)
* @return {-} 
*/
function RefreshVstChips() {
    // 画面をリフレッシュ
    ClickChangeDate(0);
}

/**
* 画面をリフレッシュ(F5キーを押すように)
* @return {-} 
*/
function RefreshWindow() {
    // 画面をリフレッシュ
    window.location.reload(); 
}

/**
* 1桁の数値を2桁の文字列として返す
*
* @param {Date} lngValue format前の値
* @return {String} format後の値
*/
function add_zero(lngValue) {

    var strResult = lngValue.toString();

    if (lngValue < 10) {
        strResult = "0" + strResult;
    }

    return strResult;
}

/**
* 今日のページチェック
* @return {bool} true:今日のページ
*/
function IsTodayPage() {
    var dtShowDate = new Date($("#hidShowDate").val());
    var dtTodayDate = GetServerTimeNow();
    // 今の日付と違う場合
    if ((dtShowDate.getFullYear() != dtTodayDate.getFullYear())
        || (dtShowDate.getMonth() != dtTodayDate.getMonth())
        || (dtShowDate.getDate() != dtTodayDate.getDate())) {
        return false;
    } else {
        return true;
    }
}

/**
* メイン画面の表示
* @param {Bool} bScrollFlg 画面スクロールかどうかフラグ　true:スクロール
* @return {なし}
*/
function ShowMainArea(bScrollFlg) {

    // メインス画面のチップを削除
    RemoveAllChips();

    // 日付を表示する
    ShowCalendar();

    // 全チップを生成
    CreateAllChips();

    // 全て列に台数を表示
    ShowAllCarNums();

    if (bScrollFlg) {
        // ウィンドウをスクロール
        ScrollWndToNow();
    }

    // ボタンの表示を切り替える
    ChangeFooterBtn();

    //グルグルを非表示
    gMainAreaActiveIndicator.hide();
}

/**
* 点滅チップを表示する
* @return {なし}
*/
function ShowSwitchChips() {

    //JSON形式のチップ情報読み込み
    var jsonData = $("#hidJsonData").val();
    $("#hidJsonData").attr("value", "");
    var chipDataList = $.parseJSON(jsonData);

    var arrAllChips = new Array();
    // 取得したチップ情報をチップクラスに格納
    for (var keyString in chipDataList) {
        var chipData = chipDataList[keyString];
        var strKey = chipData.REZID;
        // 取得した点滅チップidが画面にある場合、
        if (gArrObjChip[strKey]) {
            // 点滅してないチップを点滅する
            if (gArrObjChip[strKey].vstFlg != 1) {
                gArrObjChip[strKey].vstFlg = 1;
                $("#" + strKey).addClass("FotterBlink");
            }
        }
    }
    //グルグルを非表示
    gMainAreaActiveIndicator.hide();
}
/**
* テーブルの生成
* @return {なし}
*/
function CreateTable() {

    // 画面スクロールできる
    $(".tsl02-01_bodyBox").SC3100303FingerScroll({
        minLeft: C_TB_WIDTH - (gColNums * C_HOUR_WIDTH),
        minTop: 0
    });
    // スクロールDIVの高さと幅を設定する
    $(".tsl02-01_bodyBox .scroll-inner").height(C_TB_HEIGHT);
    $(".tsl02-01_bodyBox .scroll-inner").width(gColNums * C_HOUR_WIDTH);

    // 固定時間で画面をリフレッシュ
    setInterval("RefreshVstChips()", gRefreshTime * 1000);
}


/**
* ウィンドウを今の時刻にスクロール
* @return {なし}
*/
function ScrollWndToNow() {
    // 今日の場合
    if (IsTodayPage() == true) {
        var dtNow = GetServerTimeNow();
        if (dtNow - gStartWorkTime < 0) {
            // 営業開始時間前の場合、最初からスクロール
            ScrollToColNo(1);
        } else if (dtNow - gEndWorkTime > 0) {
            // 営業終了時間後の場合、最後までスクロール
            ScrollToColNo(gColNums);
        } else {
            // 営業中
            ScrollToColNo(dtNow.getHours() - gStartWorkTime.getHours());
        }
    } else {
        // 今日以外の場合、最初から表示される
        ScrollToColNo(1);
    }
}

/**
* N番目列にスクロールする
* @param {Integer} nColNo N番目列
* @return {なし}
*/
function ScrollToColNo(nColNo) {

    // N番目列により、左座標を計算
    var nMoveX;
    if (nColNo < 1) {
        nMoveX = $(".scroll-inner").position().left;
    } else if (nColNo >= gColNums - 2) {
        nMoveX = $(".scroll-inner").position().left + C_HOUR_WIDTH * (gColNums - 3) - 17;
    } else {
        nMoveX = $(".scroll-inner").position().left + C_HOUR_WIDTH * (nColNo - 1);
    }

    // スクロール
    $(".tsl02-01_bodyBox").SC3100303FingerScroll({
        action: "move",
        moveY: $(".scroll-inner").position().top,
        moveX: nMoveX
    });
}

/**
* 全てチップの生成
* @return {なし}
*/
function CreateAllChips() {
    gSelectedChipId = "";

    //JSON形式のチップ情報読み込み
    var jsonData = $("#hidJsonData").val();
    $("#hidJsonData").attr("value", "");
    var chipDataList = $.parseJSON(jsonData);

    var arrAllChips = new Array();
    // 取得したチップ情報をチップクラスに格納
    for (var keyString in chipDataList) {
        var chipData = chipDataList[keyString];

        var strKey = chipData.REZID;
        if (gArrObjChip[strKey] == undefined) {
            gArrObjChip[strKey] = new ReserveChip(strKey);
        }

        gArrObjChip[strKey].setChipParameter(chipData);
        arrAllChips.push(gArrObjChip[strKey]);
    }

    // 開始時間を設定
    var dtStartTime = new Date();
    dtStartTime.setTime(gStartWorkTime.getTime());
    // 9:30の場合、9:00にする
    dtStartTime.setMinutes(0);
    // 開始時間後の30分
    var dtEndTime = new Date();
    dtEndTime.setTime(dtStartTime.getTime() + 30*60*1000);

    for (var nCol = 0; nCol < gColNums * 2; nCol++) {
        // 全てチップの中から、一列にあるチップを絞り込む
        var arrColChips = SelectChipsInTime(arrAllChips, dtStartTime, dtEndTime);
        // statusにより、ソートする
        arrColChips.sort(function (x, y) { return x.chipStatus - y.chipStatus });
        var nMaxLoop = arrColChips.length;
        if (nMaxLoop > 20) {
            nMaxLoop = 20;
        }
        for (var nLoop = 0; nLoop < nMaxLoop; nLoop++) {
            // チップ生成
            arrColChips[nLoop].createChip("");    
            // チップタップ時のイベントを登録     
            BindChipClickEvent(arrColChips[nLoop].rezId);
        }

        // 時間範囲が後の30分に設定
        dtStartTime.setTime(dtStartTime.getTime() + 30 * 60 * 1000);
        dtEndTime.setTime(dtStartTime.getTime() + 30 * 60 * 1000);
    }
}

/**
* 全て列に台数を表示
* @param {Integer} nCol 開始時間
* @return {なし}
*/
function ShowAllCarNums() {
    // 全画面の台数を表示する
    for (var nCol = 0; nCol < gColNums; nCol++) {
        ShowChipNumbers(nCol);
    }
    //spanタグにCustomLabelを適用する(これによって...をタップするとツールチップ表示)
    $(".hourSet .DivNumberWord .SpanNumberWord").CustomLabel({ useEllipsis: true });
}

/**
* 1時間範囲の台数を表示する
* @param {Integer} nCol 開始時間
* @return {なし}
*/
function ShowChipNumbers(nCol) {
    
    // 一時間の2つ列の台数を加える
    var nCarNums = $(".tsl02-01_innerBox .TimesBox .wbChipsArea:eq(" + (nCol*2) + ") .Inner .wbChips").length;
    nCarNums += $(".tsl02-01_innerBox .TimesBox .wbChipsArea:eq(" + (nCol*2+1) + ") .Inner .wbChips").length;

    // 台数を表示する
    var objCarNums = $(".tsl02-01_innerBox .TimesBox .hourSet:eq(" + nCol + ") .Number");
    if (objCarNums[0]) {
        objCarNums[0].innerHTML = nCarNums;
    }
}

/**
* 全てチップの中から、時間範囲のチップを絞り込む
* @param {Array} arrAllChips 全てチップ(order by 時間)
* @param {Date} dtStartTime 開始時間
* @param {Date} dtEndTime 終了時間
* @return {なし}
*/
function SelectChipsInTime(arrAllChips, dtStartTime, dtEndTime) {
    var arrRt = new Array();
    for (var nLoop = 0; nLoop < arrAllChips.length; nLoop++) {
        // 範囲外の場合、戻す
        if ((arrAllChips[nLoop].planVstDate < dtStartTime) || (arrAllChips[nLoop].planVstDate >= dtEndTime)) {
            // arrRtに移動したデータは削除する
            arrAllChips.splice(0, arrRt.length);
            return arrRt;
        }
        arrRt.push(arrAllChips[nLoop]);
    }
    return arrRt;
}

/**
* チップタップ時のイベントを登録
* @param {String} strChipId チップID
* @return {なし}
*/
function BindChipClickEvent(strChipId) {

    //チップタップ時のイベントを登録
    $("#" + strChipId).bind("chipTap", function (e) {
        TapChip(strChipId);
    })
}

/**
* チップタップ
* @param {String} strChipId チップID
* @return {なし}
*/
function TapChip(strChipId) {
    // 選択中→未選択にする
    if (gSelectedChipId != "") {
        // 元のチップを表示する
        $("#" + gSelectedChipId).css("visibility", "visible");

        setTimeout(function () {
            // 選択した影を表示しない
            $(".blackBackGround").css("display", "none");

            // 選択したチップ構造体をクリア
            gSelectedObjChip = null;
            $("#" + C_SELECTEDCHIPID).remove();
            gSelectedChipId = "";
            // ボタンを切り替える
            ChangeFooterBtn();
        }, 100);
    } else {
        // 未選択→選択中にする
        var objChipId = $("#" + strChipId);
        // 影を表示する
        $(".blackBackGround").addClass("BackGroundZIndex").css("display", "block");

        // 選択したチップのデータをgSelectedObjChipにコピーする
        gSelectedObjChip = new ReserveChip(C_SELECTEDCHIPID);
        gArrObjChip[strChipId].copy(gSelectedObjChip);
        gSelectedObjChip.rezId = C_SELECTEDCHIPID;

        // 選択したチップのidをバックする
        gSelectedChipId = strChipId;

        // 選択したチップが影の上で新規する
        gSelectedObjChip.createChip(C_SELECTEDCHIPID);
        // チップタップ時のイベントを登録     
        BindChipClickEvent(C_SELECTEDCHIPID);

        // 位置の設定
        var nLeft, nTop;
        // 相対位置を取得
        nLeft = objChipId.offset().left - parseInt($("#Inner").css("left"));
        nTop = objChipId.position().top;
        $("#" + C_SELECTEDCHIPID).css({"left":nLeft, "top":nTop});

        // 元のチップを非表示する
        objChipId.css("visibility", "hidden");
        // ボタンを切り替える
        ChangeFooterBtn();
    }
}

/**
* 全チップを削除
* @return {なし}
*/
function RemoveAllChips() {

    gSelectedChipId = "";

    // チップを削除
    $(".wbChips").remove();

    // gArrObjChipをクリア
    gArrObjChip = new Array();
}

/**
* コールバック関数定義
* @param {String} argument サーバーに渡すパラメータ(JSON形式)
* @param {String} callbackFunction コールバック後に実行するメソッド
*/
var gCallbackSC3100303 = {
    doCallback: function (argument, callbackFunction) {
        this.packedArgument = JSON.stringify(argument);
        this.endCallback = callbackFunction;
        this.beginCallback();
    }
};

/**
* アクティブインジケータ表示操作関数定義
*/
var gMainAreaActiveIndicator = {
    show: function () {
        $.master.OpenLoadingScreen();
    }
    ,
    hide: function () {
        $.master.CloseLoadingScreen();
    }
};

/**
* 来店管理のチップを取得
* @return {なし} 
*/
function GetVstChips() {
    // 渡す引数
    var jsonData = {
        MEDTHOD: "GetVstChips",
        SHOWDATE: $("#hidShowDate").val()
    };
    //コールバック開始
    gCallbackSC3100303.doCallback(jsonData, SC3100303AfterCallBack);
}

/**
* GKから来店通知で画面を更新
* @return {なし}
*/
function Send_Visit() {

    //2012/05/05 TMEJ 小澤  ITxxxx_TSL自主研緊急対応（サービス）3回目 START
    //通知音を出す
    icropScript.ui.beep(2);
    //2012/05/05 TMEJ 小澤  ITxxxx_TSL自主研緊急対応（サービス）3回目 END

    // 画面の表示が今日ではない場合、何もしない
    if (IsTodayPage() == false) {
        return;
    }

    // チップが選択した場合、解除する
    if (gSelectedChipId != "") {
        TapChip("");
    }

    // グルグルを表示する
    gMainAreaActiveIndicator.show();
    //リフレッシュタイマーセット
    commonRefreshTimer(RefreshWindow);

    // ボタンの表示を切り替える
    ChangeFooterBtn();

    // 渡す引数
    var jsonData = {
        MEDTHOD: "GetSwitchChipId",
        SHOWDATE: $("#hidShowDate").val()
    };
    //コールバック開始
    gCallbackSC3100303.doCallback(jsonData, SC3100303AfterCallBack);
}

/**
* フォローボタンを押す
* @return {なし} 
*/
function ClickBtnFollow() {
    // 渡す引数
    var jsonData = {
        MEDTHOD: "ClickBtnFollow",
        REZID: gSelectedChipId,
        SHOWDATE: $("#hidShowDate").val(),
        UPDATECNT: gSelectedObjChip.updateCnt
    };

    // チップが選択した場合、解除する
    if (gSelectedChipId != "") {
        TapChip("");
    }
    // グルグルを表示する
    gMainAreaActiveIndicator.show();
    //リフレッシュタイマーセット
    commonRefreshTimer(RefreshWindow);

    //コールバック開始
    gCallbackSC3100303.doCallback(jsonData, SC3100303AfterCallBack);
}

/**
* フォロー解除ボタンを押す 
* @return {なし} 
*/
function ClickBtnClearFollow() {

    // 渡す引数
    var jsonData = {
        MEDTHOD: "ClickBtnClearFollow",
        REZID: gSelectedChipId,
        SHOWDATE: $("#hidShowDate").val(),
        UPDATECNT: gSelectedObjChip.updateCnt
    };

    // チップが選択した場合、解除する
    if (gSelectedChipId != "") {
        TapChip("");
    }
    // グルグルを表示する
    gMainAreaActiveIndicator.show();
    // リフレッシュタイマーセット
    commonRefreshTimer(RefreshWindow);

    // コールバック開始
    gCallbackSC3100303.doCallback(jsonData, SC3100303AfterCallBack);
}

/**
* コールバック後の処理関数(受付)
* @param {String} result コールバック呼び出し結果
* @param {String} context
*/
function SC3100303AfterCallBack(result) {

    //タイマーをクリア
    commonClearTimer();
    // JSON形式のデータを変換し、処理する
    var rtList = $.parseJSON(result);
    if (rtList.RESULTCODE == C_NO_ERROR) {
        switch (rtList.MEDTHOD) {
            // 追加した点滅ボタンを点滅する
            case "GetSwitchChipId":
                $("#hidJsonData").val(htmlDecode(rtList.CONTENTS));
                //2013/04/25 TMEJ 張 ITxxxx_TSL自主研緊急対応（サービス） START
                // 来店実績台数を表示する
                ShowVstCarCnt(rtList.VSTCARCNT);
                //2013/04/25 TMEJ 張 ITxxxx_TSL自主研緊急対応（サービス） END
                // 点滅チップを表示する
                ShowSwitchChips();
                break;
            // チップを再表示する(画面自動スクロール)
            case "GetVstChips":
                $("#hidJsonData").val(htmlDecode(rtList.CONTENTS));
                var dtShowDate = new Date($("#hidShowDate").val());
                $("#pCalendar").text(htmlDecode(rtList.SHOWDATE));
                //2013/04/25 TMEJ 張 ITxxxx_TSL自主研緊急対応（サービス） START
                // 来店実績台数を表示する
                ShowVstCarCnt(rtList.VSTCARCNT);
                //2013/04/25 TMEJ 張 ITxxxx_TSL自主研緊急対応（サービス） END
                ShowMainArea(true);
                break;
            // チップを再表示する(画面自動スクロールしない) 
            default:
                $("#hidJsonData").val(htmlDecode(rtList.CONTENTS));
                ShowMainArea(false);
                break; 
        }
    } else { 
        // メッセージidがあれば、メッセージidにより、エラーを表示する
        if ((rtList.MESSAGEID) 
            && ((rtList.MESSAGEID >= 901) && (rtList.MESSAGEID <= 903))) {
            // エラーメッセージが表示される
            ShowSC3100303Msg(rtList.MESSAGEID);
        } else {
            // 予想以外のエラーの場合
            alert(rtList.MESSAGE);
        }
        // データを再表示する
        GetVstChips();
    }
}

//2013/04/25 TMEJ 張 ITxxxx_TSL自主研緊急対応（サービス） START
/**
* 来店実績台数を表示する
* @param {Integer} nVstCarCnt 来店実績台数
*/
function ShowVstCarCnt(nVstCarCnt) {

    // 文言を取得
    var strWord = gSC3100303WordIni[C_WORD_VST_CAR_CNT];
    strWord = strWord.replace("{0}", nVstCarCnt);
    $("#lblVstCarCnt").text(strWord);
}
//2013/04/25 TMEJ 張 ITxxxx_TSL自主研緊急対応（サービス） END

/**
* フッター部にボタンをクリックイベント
* @param {Integer} nButtonID 1:全体管理ボタンを押す　2:来店管理ボタンを押す
* @param {String} context
*/
function FooterEvent(nButtonID) {

    switch (nButtonID) {
        // 全体管理ボタンを押す
        case 1:
            //ボタン背景点灯
            $('#FooterButton100').addClass("icrop-pressed");
            // グルグルを表示
            gMainAreaActiveIndicator.show();
            // 遷移する
            setTimeout(function () {
                // ボタン背景を戻す
                $('#FooterButton100').removeClass("icrop-pressed");
                //タイマーセット
                commonRefreshTimer(function () { __doPostBack("", ""); });
                $("#GeneralMngButton").click();
            }, 300);
            break;
        // 来店管理ボタンを押す 
        case 2:
            //ボタン背景点灯
            $('#FooterButton200').addClass("icrop-pressed");
            // グルグルを表示
            gMainAreaActiveIndicator.show();
            // 遷移する
            setTimeout(function () {
                // ボタン背景を戻す
                $('#FooterButton200').removeClass("icrop-pressed");
                //タイマーセット
                commonRefreshTimer(function () { __doPostBack("", ""); });
                //再描画イベント実行
                __doPostBack("", "");
            }, 500);
            break;
        // フォローボタンを押す
        case 3:
            ClickBtnFollow();
            break;
        // フォロー解除ボタンを押す 
        case 4:
            ClickBtnClearFollow();
            break;
        // 2018/02/20 NSK  山田 REQ-SVT-TMT-20170615-001 来店管理に入庫予約を指定してROを発行する機能を追加 START
        //R/Oボタンを押す
        case 5:
            if (gSelectedObjChip.cstType != C_CST_TYPE_1) {
                //メッセージ文言取得
                var strWordNg = gSC3100303WordIni[C_WORD_RO_NG_MSG];
                alert(strWordNg);
            } else {
                //メッセージ文言取得
                var strWord = gSC3100303WordIni[C_WORD_RO_MSG];
                if (window.confirm(strWord)) {
                    //「Yes」押下
                    //選択中のREZIDをサーバー送信用hiddenにセット
                    $("#hidSelectedRezId").val(gSelectedChipId);
                    // グルグルを表示
                    gMainAreaActiveIndicator.show();
                    // 遷移する
                    setTimeout(function () {
                        //タイマーセット
                        commonRefreshTimer(RefreshWindow);
                        $("#ROCreateButton").click();
                    }, 300);
                }
            }
            break;
        // 2018/02/20 NSK  山田 REQ-SVT-TMT-20170615-001 来店管理に入庫予約を指定してROを発行する機能を追加 END
    }
}

/**
* フッター部にボタンの表示を切り替える
* @param {なし}
*/
function ChangeFooterBtn() {
    
    // 未選択状態のボタンを表示する
    if (gSelectedChipId == "") {
        $("#ChipFooterArea").css("display", "none");
        $("#InitFooterArea").css("display", "block");

        // SM以外の場合だけ、全体管理ボタンが表示される
        if ($("#hidOpeCD").val() == C_OPECD_SM) {
            $("#FooterButton100").css("display", "block");
        } else {
            $("#FooterButton100").css("display", "none");
        }
    } else {
        // 選択状態のボタンを表示する
        $("#ChipFooterArea").css("display", "block");
        $("#InitFooterArea").css("display", "none");

        // 選択したチップに電話マークが表示されて、フォロー解除ボタンを表示する
        if (gSelectedObjChip.noShowFollowFlg == 1) {
            $("#FooterButton300").css("display", "none");
            $("#FooterButton400").css("display", "block");
        } else {
            // フォローボタンを表示する
            $("#FooterButton300").css("display", "block");
            $("#FooterButton400").css("display", "none");
        }

        // 2018/02/20 NSK  山田 REQ-SVT-TMT-20170615-001 来店管理に入庫予約を指定してROを発行する機能を追加 START
        //ログイン権限がSAの場合のみR/Oボタン表示
        if ($("#hidOpeCD").val() == C_OPECD_SA) {
            $("#FooterButton500").css("display", "block");
        } else {
            $("#FooterButton500").css("display", "none");
        }
        // 2018/02/20 NSK  山田 REQ-SVT-TMT-20170615-001 来店管理に入庫予約を指定してROを発行する機能を追加 END
    }
}

/**
* 文言取得
* @param {なし}
*/
function GetSC3100303WordIni() {
    var strWordIni = $("#hidMsgData").val();
    if (gSC3100303WordIni == null) {
        gSC3100303WordIni = $.parseJSON(strWordIni);
        $("#hidMsgData").attr("value", "");
    }
}

/**
* メッセージを表示する
* @param {なし}
*/
function ShowSC3100303Msg(strWordNo) {
    //gSC3100303WordIni[strWordNo]があれば、メッセージボックスで表示
    if (gSC3100303WordIni != null) {
        if (gSC3100303WordIni[strWordNo] != null) {
            alert(gSC3100303WordIni[strWordNo]);
        }
    }
}

// フッターボタンの2度押し制御
function FooterButtonControl() {
    $.master.OpenLoadingScreen();
    return true;
}