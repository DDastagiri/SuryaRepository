//------------------------------------------------------------------------------
//SC3150101.Main.js
//------------------------------------------------------------------------------
//機能：メインメニュー（TC）_javascript
//補足：
//作成：2012/01/30 KN 渡辺
//更新：2012/03/02 KN上田 【SERVICE_1】課題管理番号-BMTS_0229_YW_02の不具合修正(フッタボタン制御)
//------------------------------------------------------------------------------

//タイマーのインターバル時間（ミリ秒）
var C_INTERVAL_TIME = 1000 * 3;
//チップ情報の再読込時間
var C_CHIP_UPDATE_TIME = 1000 * 60;
//作業進捗バーの再読込時間
var C_METER_UPDATE_TIME = 1000 * 120;
//現在時刻表示の切り替え時間（ミリ秒）
var C_CURRENT_UPDATE_TIME = 1000 * 60;
//ページリロードまでの時間（ミリ秒）
var C_PAGE_RELOAD_TIME = 1000 * 60;
//チップ点滅時間（消えるまでor表示されるまでの時間）（ミリ秒）
var C_BLINK_TIME = 1000 * 1;

//チップ情報の再読込カウンタタイミング
var gChipUpdateTiming = C_CHIP_UPDATE_TIME / C_INTERVAL_TIME;
//現在時刻の表示切替タイミング
var gCurrentUpdateTiming = C_CURRENT_UPDATE_TIME / C_INTERVAL_TIME;
//作業進捗バーの表示切替タイミング
var gMeterUpdateTiming = C_METER_UPDATE_TIME / C_INTERVAL_TIME;
//ページリロードの発生タイミング
var gPageReloadTiming = C_PAGE_RELOAD_TIME / C_INTERVAL_TIME;
//チップ点滅発生タイミング
var gChipBlinkTiming = (C_BLINK_TIME * 2) / C_INTERVAL_TIME;

//チップ情報の再読込カウンタ
var gChipUpdateCount = 0;
//現在時刻の表示切替カウンタ
var gCurrentUpdateCount = 0;
//作業進捗バーの表示切替カウンタ
var gMeterUpdateCount = 0;
//ページリロード発生カウンタ
var gPageReloadCount = 0;
//チップ点滅処理発生カウンタ
var gChipBlinkCount = 0;

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
//Box01GraphCassetteの高さ（不要）
//var C_CASSETTE_HEIGHT = 62;
//Box01GraphCassetteを描画開始するY軸位置（不要）
//var C_DRAW_START_Y = 0;

//キャンバスの幅を初期化
var gCanvasWidth = 0;
//キャンバスの高さを初期化（不要）
//var gCanvasHeight = 68;
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

//端末操作時刻（放置処理のため）初期化する（不要）
//var dtmLastTime = new Date();
//作業対象チップID
//var gCandidateChipId = null;

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


/**
 * DOMロード直後の処理(重要事項).
 * @return {void}
 */
$(function () {

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

    //タイマー処理を設定する.
    setInterval("controlTimer()", C_INTERVAL_TIME);

    //フリックしてスクロールさせたときのイベント.
    //左からのスクロール位置をHiddenFieldに格納する.
    $("#Box01GraphBox").scroll(function () {
        $("#HiddenScrollLeft").val($(this).scrollLeft());
    });

    //休憩をとる・とらないを問うpopupを表示する処理.
    if ($("#HiddenBreakPopup").val() == C_BREAK_POPUP_DISPLAY) {
        //フラグを初期化する.
        $("#HiddenBreakPopup").val(C_BREAK_POPUP_NONE);
        selectClass();
    }

    //2012/03/02 上田 フッタボタン制御 Start
//    //部品連絡ポップアップ処理.
//    $("#ButtonConnectParts").popover({
//        id: "ButtonConnectParts",
//        offsetX: 0,
//        offsetY: 0,
//        preventLeft: true,
//        preventRight: true,
//        preventTop: false,
//        preventBottom: true,
//        content: "<div id='ButtonConnectParts_content' />",
//        //header: "<div id='ButtonConnectParts_header'><p>" + "部品連絡" + "</p></div>",
//        header: getPopoverHeaderContents(),
//        openEvent: function () {
//            var container = $('#ButtonConnectParts_content');
//            //var $iframe = $("<iframe frameborder='0' id='ButtonConnectParts_Frame' width='407px' height='329px' scrolling='no' src='../Pages/SC3190303.aspx' />");
//            var $iframe = $("<iframe frameborder='0' id='ButtonConnectParts_Frame' width='395px' height='280px' scrolling='no' src='../Pages/SC3190303.aspx' />");
//            container.empty().append($iframe);
//        }
//    });

//    //部品連絡ポップアップのキャンセル押下時処理.
//    $("#ButtonConnectParts_cancel").bind("click.popover", function (event) {
//        ParentPopoverClose();
//    });

    //リロードフラグをONに設定
    $("#HiddenReloadFlag").val("1");
    //2012/03/02 上田 フッタボタン制御 End
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
function selectClass() {
    $("#tcvNsc31Black").css("display", "inline-block");
    $("#tcvNsc31Main").css("display", "inline-block");
    //$("#popWind").animate({ top: '210px' }, "slow");
    $("#popWind").fadeIn("slow");
}


/**
 * 休憩をとる・とらないのPopupを閉じる.
 * @param {boolean} true値にて閉じる動作
 * @return {void}
 */
function confirm(flag) {
    $("#tcvNsc31Black").css("display", "none");
    $("#tcvNsc31Main").css("display", "none");
    //$("#popWind").animate({ top: '700px' }, "slow");
    $("#popWind").fadeOut("slow");
} 


/**
 * フッタボタンの表示制御.
 * @return {void}
 */
function controlFooterButton() {

    var _selectedChipId = $("#HiddenSelectedId").val().toString();

    //すべてのボタンを非表示に設定.
    $("#ButtonConnectParts").css("display", "none");
    $("#ButtonStartWork").css("display", "none");
    $("#ButtonSuspendWork").css("display", "none");
    $("#ButtonStartCheck").css("display", "none");

    //フッタボタンが表示可能条件を満たす場合、各ボタンの表示判定を実施する.
    if (checkDisplayFooterButton()) {

        var partsDataCount = parseInt($("#HiddenPartsCount").val())
        var partsBackOrderCount = parseInt($("#HiddenBackOrderCount").val())
        var partsDataComp = $("#HiddenPartsComp").val()

        //タブが選択されたとき、部品連絡ボタンの表示処理を行う.
        //作業内容タブが表示されている.
        if ($("#HiddenSelectedTabNumber").val() == C_ROTAB_CLASS_WORK_NUMBER) {
            //B/O項目を除く部品情報数を取得する.
            var partsDataCountBackOrderOut = 0;
            if (partsDataCount > 0) {
                partsDataCountBackOrderOut = partsDataCount;
            }
            if (partsBackOrderCount > 0) {
                partsDataCountBackOrderOut = partsDataCountBackOrderOut - partsBackOrderCount;
            }
            //B/O項目を除く部品情報数が1件以上ある場合、部品連絡ボタンを表示する可能性がある.
            if (partsDataCountBackOrderOut > 0) {
                //部品準備完了している場合、部品連絡ボタンを表示する.
                if (partsDataComp == C_PARTS_REPARE_PREPARED) {
                    $("#ButtonConnectParts").css("display", "inline-block");
                }
            }
        }

        var resultStatus = gArrObjChip[_selectedChipId].chipResultStatus;
        //実績ステータスが、作業待ちである場合、当日処理ボタンと完了検査ボタンを非表示にする.
        if (resultStatus == C_RESULT_STATUS_WAIT) {
            //部品情報数が0値の場合、作業開始ボタンを表示する.
            if (partsDataCount == 0) {
                $("#ButtonStartWork").css("display", "inline-block");
            }
            //部品情報数がブランク（0値を含む）以外、且つ、部品準備が完了している場合、作業開始ボタンを表示する.
            else if ((partsDataCount > 0) && (partsDataComp == C_PARTS_REPARE_PREPARED)) {
                $("#ButtonStartWork").css("display", "inline-block");
            }

        } else if (resultStatus == C_RESULT_STATUS_WORKING) {
            //ButtonStartCheck 完了検査ボタン.
            $("#ButtonStartCheck").css("display", "inline-block");

            //終了時間（予定）がストール作業終了時間を越えている場合、当日処理ボタンを表示とする.
            if (gArrObjChip[_selectedChipId].chipEndTime > gGraphBoxEndTime) {
                $("#ButtonSuspendWork").css("display", "inline-block");
            }
        }
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

        //選択中のチップの実績ステータスが、作業待ち・作業中の場合、ボタンの表示制御を行う.
        var resultStatus = gArrObjChip[_selectedChipId].chipResultStatus;
        if ((resultStatus == C_RESULT_STATUS_WAIT) || (resultStatus == C_RESULT_STATUS_WORKING)) {

            //作業対象チップの、REZ_REZRECEPTIONが0,4の場合のみ、ボタンの表示制御を行う.
            var reception = gArrObjChip[_selectedChipId].chipRezReception;
            if ((reception == C_REZ_RECEPTION_WAIT) || (reception == C_REZ_RECEPTION_DROPOFF)) {

                //R/O番号がブランクでない場合、ボタンの表示制御を行う.
                if (gArrObjChip[_selectedChipId].orderNumber != "") {

                    checkResult = true;
                }
            }
        }
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
    //ページリロード処理.
    //reloadPage();
    //現在時刻とストール終了時刻による画面更新の連絡処理.
    //warnNextDate();
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
 *
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


/**
 * リロード処理
 * @return {void}
 */
function reloadPage() {
    
    //リロードカウンタを更新する
    gPageReloadCount++;
    //更新後のリロードカウンタが更新タイミングを超過する場合、ページリロードを行う.
    //カウンタはリロード時に更新されるため、ここでは行わない.
    if (gPageReloadTiming <= gPageReloadCount) {
        //alert("ページリロードを行います。リロードカウンタ：" + gPageReloadCount.toString());
        window.location.reload();
    }
}


//ストール終了時刻による画面更新処理
function warnNextDate() {

    //現在時刻を取得して、その時刻がストール終了時刻より新しい時間である場合、
    //ログアウトして、再度ログインしてもらうためのメッセージを表示する.
    if (gGraphBoxEndTime < getServerTimeNow()) {
        alert($("#HiddenWarnNextDate").val());
        //icropScript.ShowMessageBox(101, "Name field is empty. Name field is required.", "");
    }
}


//チップのタップ処理
function tapChip(obj) {

    //チップ選択状態を初期化する.
    var chipSelectedStatus = true;

    //対象となるオブジェクトが存在する場合のみ、処理を実施する
    if (obj) {
        //選択されたチップのIDを取得する
        var strSelectedChipId = $(obj).attr("id");

        //チップ選択状態を取得し、Hidden格納値を更新する.
        chipSelectedStatus = judgeSelectedChip(strSelectedChipId);
        //選択されたチップIDをHiddenフィールドに格納し、更新する.
        $("#HiddenSelectedId").val(strSelectedChipId);

        //直前の選択されたREZID, ORDERNO, CHILDNOを格納する.
        var lastSelectedRezId = $("#HiddenSelectedReserveId").val();
        var lastSelectedOrderNo = $("#HiddenFieldOrderNo").val();
        var lastSelectedChildNo = $("#HiddenFieldChildNo").val();

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
                    gArrObjChip[key].setChipFilter(true);
                    //選択されたチップのREZIDをHiddenに格納する.
                    $("#HiddenSelectedReserveId").val(gArrObjChip[key].rezId);
                    //R/O番号を設定する.
                    _orderNo = gArrObjChip[key].orderNumber;
                    $("#HiddenFieldOrderNo").val(_orderNo);
                    //子番号を設定する.
                    $("#HiddenFieldChildNo").val(gArrObjChip[key].childNumber);
                    //実績ステータスを取得する.
                    resultStatus = gArrObjChip[key].chipResultStatus;
                    //選択されたチップ情報を作業進捗メータークラスに渡す.
                    gWorkMeter.setMeterParameter(gArrObjChip[key].chipDrawStartTime, gArrObjChip[key].chipDrawEndTime,
                    _orderNo, gArrObjChip[key].chipDrawStartTime, gArrObjChip[key].chipDrawEndTime, resultStatus);
                    //渡したチップ情報を元に、作業進捗メーターを再描画する.
                    gWorkMeter.refreshMeter();
                }
                //選択されたチップのIDと同値のチップでない場合.
                else {
                    //チップ選択フラグをfalseにする.
                    gArrObjChip[key].setChipFilter(false);
                }
                //Box01GraphLineにグレーフィルターをかける.
                $("#Box01GraphLineFilter").css("opacity", C_FILTER_TRANSLUCENT);
            }
            //チップ選択状態がfalse、すなわち、チップを選択している状態でない場合、チップ選択を解除する.
            else {
                //チップ選択フラグをfalseにする.
                gArrObjChip[key].setChipFilter(true);
                //Box01GraphLineのグレーフィルターを解除する.
                $("#Box01GraphLineFilter").css("opacity", C_FILTER_CLEAR);
            }
        } //End for
    }

    //R/O情報欄のフィルターフラグを設定する.
    var repairOrderFilterFlag = C_REPAIR_ORDER_FILTER_ON;
    //if (this.checkSelectedIsCandidateId()) {
    //チップの実績ステータスが、作業待ち・作業中の場合、且つ、REZ_REZRECEPTIONが0,4の場合のみスモークフィルタを除去する.
    if ((resultStatus == C_RESULT_STATUS_WAIT) || (resultStatus == C_RESULT_STATUS_WORKING)) {
        var reception = gArrObjChip[strSelectedChipId].chipRezReception;
        if ((reception == C_REZ_RECEPTION_WAIT) || (reception == C_REZ_RECEPTION_DROPOFF)) {
            repairOrderFilterFlag = C_REPAIR_ORDER_FILTER_OFF;
        }
    }
    $("#HiddenFieldRepairOrderFilter").val(repairOrderFilterFlag);

    //************************************
    //インラインフレームの内容を再読込
    //************************************
    //チップが選択されている状態である場合、且つ、直前の選択されたREZID, ORDERNO, CHILDNOが同値でない場合、
    //インラインフレームの内容の再読込を実施する.
    if (chipSelectedStatus) {
        if ((lastSelectedRezId != $("#HiddenSelectedReserveId").val()) || (lastSelectedOrderNo != $("#HiddenFieldOrderNo").val()) || (lastSelectedChildNo != $("#HiddenFieldChildNo").val())) {
            //部品準備フラグを初期化する.
            $("#HiddenPartsComp").val(C_PARTS_REPARE_UNPREPARED);
            //部品情報数を初期化する.
            $("#HiddenPartsCount").val("");
            //B/O項目数を初期化する.
            $("#HiddenBackOrderCount").val("");

            //2012/03/02 上田 フッタボタン制御 Start
            //リロードフラグをONに設定
            $("#HiddenReloadFlag").val("1");
            //2012/03/02 上田 フッタボタン制御 End

            HiddenButtonChipTap.click();
        }
        else {
            //フッタボタンの表示制御を行う
            controlFooterButton();
        }
    } else {
        //フッタボタンの表示制御を行う
        controlFooterButton();
    }
}


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
    $(obj.objChipsBase).stop();
    $(obj.objChipsBase).fadeTo("fast", C_BLINK_MAX_TRANSMITTANCE);

    if (obj.chipResultStatus == C_RESULT_STATUS_WORKING) {
        //結局下記の条件も実績開始時間と実績終了時間なので、実績に登録されていないとNULL値しか取らない
        //if ((obj.chipResultStartTime) && !(obj.chipResultEndTime)) {
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
                //$(gArrObjChip[key].objChipsBase).fadeOut(C_BLINK_TIME).fadeIn(C_BLINK_TIME);
                $(gArrObjChip[key].objChipsBase).fadeTo(C_BLINK_TIME, C_BLINK_MIN_TRANSMITTANCE)
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


/**
 * R/O情報からタブ選択時に、選択したタブ情報と、部品準備完了情報を取得し、フッターの表示制御を行う.
 *
 * @param {Integer} intTabNumber 選択されたタブ番号
 * @param {String} strPartsRepareFlag 部品準備完了フラグ
 * @param {String} strPartsCount 部品情報数
 * @param {String} strBackOrderCount B/O項目数
 * @return {void}
 */
function CheckChengeTab(intTabNumber, strPartsRepareFlag, strPartsCount, strBackOrderCount) {

    //選択されているタブ番号を格納するフィールドに、取得したタブ番号を格納する.
    $("#HiddenSelectedTabNumber").val(intTabNumber);
    //選択されている部品準備完了フラグを格納するフィールドに、取得した部品準備完了フラグを格納する.
    $("#HiddenPartsComp").val(strPartsRepareFlag);
    //部品情報数を格納する.
    $("#HiddenPartsCount").val(strPartsCount);
    //B/O項目数を格納する.
    $("#HiddenBackOrderCount").val(strBackOrderCount);

    controlFooterButton();
}


/**
 * R/O情報にて左フリックを実施した際に、呼び出されるメソッド.
 * HiddenにR/O情報にて左クリックされたフラグを格納し、ポストバックする.
 *
 * @return {void}
 */
function flickRepairOrderInfomation() {
    //隠しボタンを押下し、画面遷移処理を開始する.
    $("#HiddenButtonFlickRepairOrder").click();
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
 * @param {Integer} selectedChildNumber
 * @return {void}
 */
function tapRepairOrderIcon(tapIconNumber, selectedChildNumber) {
    $("#HiddenFieldRepairOrderIcon").val(tapIconNumber.toString());
    //orderNumberを取得する.
    var orderNumber = $("#HiddenFieldOrderNo").val();
    //空文字を除去
    orderNumber = trimString(orderNumber);

    //orderNumberがない場合、処理を実施しない.
    if (orderNumber != "") {
        //追加作業アイコン番号と活性状態にある追加作業番号が一致する場合も遷移処理しない.
        if (tapIconNumber != selectedChildNumber) {
            //隠しボタンを押下し、画面遷移処理を開始する.
            $("#HiddenButtonRepairOrderIcon").click();
        }
    }
}


/**
 * R/O情報にて、履歴情報をタップした際の処理.
 * @param {String} selectedOrderNumber
 * @return {void}
*/
function tapHistory(selectedOrderNumber) {

    //取得したR/O番号がブランクの場合、処理を実施しない.
    if (selectedOrderNumber != "") {
        //R/O番号を履歴情報の選択RO番号フィールドに格納する.
        $("#HiddenHistoryOrderNumber").val(selectedOrderNumber);
        //隠しボタンを押下し、画面遷移処理を開始する.
        $("#HiddenButtonHistory").click();
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
    //作業対象チップIDの設定を行う.
    //setCandidateChipId();
    //チップのタップイベントを再バインドする.
    $(".ChipsBaseFilter").unbind("touchstart click");
    $(".ChipsBaseFilter").bind("touchstart click", function () {
        //タップ処理を行う.
        tapChip(this);
    });
}
///**
//* チップ情報をサーバより取得し、配置する.
//* @return {void}
//*/
//function createChipObject() {
//    //JSON形式のチップ情報読み込み.
//    try {
//        http = new ActiveXObjext("Microsoft.XMLHTTP");
//    } catch (e) {
//        http = new XMLHttpRequest();
//    }
//    //受信時のコールバック関数を登録.
//    http.onreadystatechange = function () {
//        //チップ情報の更新時刻に設定する現在時刻を取得する.
//        var dtmUpdateTime = getServerTimeNow();
//        //データの受信に成功した場合、受信データを取得.
//        if (http.readyState == 4 && http.status == 200) {
//            var data = http.responseText;
//            res = $.parseJSON(data);

//            //取得したチップ情報をチップクラスに格納し、再描画.
//            var lngIndex = 0;
//            for (var keyString in res) {
//                var value = res[keyString];

//                var strKey = value.REZID + "_" + value.SEQNO + "_" + value.DSEQNO;
//                //var strKey = value.REZID + "_" + value.DSEQNO;
//                if (gArrObjChip[strKey] == undefined) {
//                    gArrObjChip[strKey] = new ReserveChip(strKey, gGraphBoxStartTime, gGraphBoxEndTime);
//                }
//                gArrObjChip[strKey].setChipParameter(value);
//                //チップ生成に成功する場合、更新日時を設定する.
//                if (gArrObjChip[strKey].createChip()) {
//                    gArrObjChip[strKey].setUpdateTime(dtmUpdateTime);
//                }
//                checkBlinkChip(gArrObjChip[strKey]);
//            }
//            //所持しているチップオブジェクトをループ処理.
//            for (var key in gArrObjChip) {
//                //更新時刻が今回の更新時刻に一致しないチップオブジェクトを破棄する.
//                if (gArrObjChip[key].dtmUpdateTime != dtmUpdateTime) {
//                    $("#" + gArrObjChip[key].chipId + "_BASE").remove();
//                    $("#" + gArrObjChip[key].chipId).remove();
//                    delete gArrObjChip[key];
//                }
//            }
//        }
//        //作業対象チップIDの設定を行う.
//        setCandidateChipId();
//        //チップタップイベントハンドラの削除.
//        $(".ChipsBaseFilter").unbind("click");
//        //チップタップイベントハンドラの追加.
//        $(".ChipsBaseFilter").bind("click", function () {
//            //タップ処理を行う.
//            tapChip(this);
//        });
//    }
//    //HTTP GETメソッドでSC3150101.aspxにデータを送信.
//    http.open("GET", "SC3150101.aspx?read=1", false);
//    http.send(null);
//}


/**
 * チップ選択フラグをHiddenより取得し
 * チップ選択されていない場合は、ストール情報欄を初期化する
 *
 * @return {void}
 */
function initSelectedChip() {

    //現在選択されているチップIDを取得する.
    var strSelectedId = $("#HiddenSelectedId").val().toString();
//    //現在選択されているチップIDがない場合、作業対象チップのIDを割り当てる.
//    if (!(strSelectedId)) {
//        strSelectedId = $("#HiddenCandidateId").val();
//        $("#HiddenSelectedId").val(strSelectedId);
//    }

    //選択されているチップIDが空白でない場合、チップのタップイベントを実施する.
    if (strSelectedId) {
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
        //作業進捗メーターを再描画する
        gWorkMeter.refreshMeter();
        //フッタボタンの表示制御を行う
        controlFooterButton();
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
        $("#CurrentBoxTime").text(formatTime(dtmStrongTime.getHours()) + ":" + formatTime(dtmStrongTime.getMinutes()));
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


//選択中のチップIDと作業対象チップIDが一致するかを比較し、返す
function checkSelectedIsCandidateId() {

    var checkResult = false;
    var selectedId = $("#HiddenSelectedId").val();

    //選択中チップIDと作業対象チップIDが存在する場合、比較処理を実施
    if ((selectedId) && ($("#HiddenCandidateId").val())) {
        if (selectedId == $("#HiddenCandidateId").val()) {
            checkResult = true;
        }
    }

    return checkResult;
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
    //画面をリフレッシュするイベントを実施する.
    $("#HiddenButtonReflesh").click();
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

//2012/03/02 上田 フッタボタン制御 Start
/**
* フッターボタン制御
*/
function FooterButtonClick(btnId) {

    if ($("#HiddenReloadFlag").val() == "1") {
        //リロード中の場合は、以後の処理を行わない
        $("#bodyFrame").unbind(".popover");
        return false;
    }

    if (btnId != "ButtonConnectParts") {
        $("#HiddenReloadFlag").val("1");
    }

    return true;
}

/**
* リロードフラグ初期化
*/
function InitReloadFlag() {
    $("#HiddenReloadFlag").val("");

    if ($("#ButtonConnectParts")[0].style.display != "none") {
        //部品連絡ポップアップ処理.
        $("#ButtonConnectParts").popover({
            id: "ButtonConnectParts",
            offsetX: 0,
            offsetY: 0,
            preventLeft: true,
            preventRight: true,
            preventTop: false,
            preventBottom: true,
            content: "<div id='ButtonConnectParts_content' />",
            //header: "<div id='ButtonConnectParts_header'><p>" + "部品連絡" + "</p></div>",
            header: getPopoverHeaderContents(),
            openEvent: function () {
                var container = $('#ButtonConnectParts_content');
                //var $iframe = $("<iframe frameborder='0' id='ButtonConnectParts_Frame' width='407px' height='329px' scrolling='no' src='../Pages/SC3190303.aspx' />");
                var $iframe = $("<iframe frameborder='0' id='ButtonConnectParts_Frame' width='395px' height='280px' scrolling='no' src='../Pages/SC3190303.aspx' />");
                container.empty().append($iframe);
            }
        });

        $("#ButtonConnectParts_cancel").bind("click.popover", function (event) {
            ParentPopoverClose();
        });
    }
}
//2012/03/02 上田 フッタボタン制御 End

//2012/03/03 上田 ロード中アイコン制御 Start
function StopLodingIcon(itemName) {
    $(itemName).hide(0);
}
//2012/03/03 上田 ロード中アイコン制御 End
