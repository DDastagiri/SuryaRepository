//------------------------------------------------------------------------------
//SC3150102.js
//------------------------------------------------------------------------------
//機能：メインメニュー（TC）_R/O情報タブ_javascript
//補足：
//作成：2012/01/30 KN 渡辺
//更新：2012/03/02 KN 日比野【SERVICE_1】課題管理番号-BMTS_0229_YW_01の不具合修正
//更新：2012/03/02 KN 上田  【SERVICE_1】課題管理番号-BMTS_0229_YW_02の不具合修正(フッタボタン制御)
//------------------------------------------------------------------------------

//基本情報タブのCSSクラス名
var C_ROTAB_CLASS_BASE = "TabButton01";
//ご用命事項タブのCSSクラス名
var C_ROTAB_CLASS_ORDER = "TabButton02";
//作業内容タブのCSSクラス名
var C_ROTAB_CLASS_WORK = "TabButton03";

//基本情報タブのタブ番号
var C_ROTAB_CLASS_BASE_NUMBER = 1;
//ご用命事項タブのタブ番号
var C_ROTAB_CLASS_ORDER_NUMBER = 2;
//作業内容タブのタブ番号
var C_ROTAB_CLASS_WORK_NUMBER = 3;

//display属性の設定値
//表示しない
var C_DISPLAY_NONE = "none";
//表示する
var C_DISPLAY_BLOCK = "inline-block";

//基本情報のデータ定数
//燃料
var C_BASIC_FUEL_EMPTY = "0";
var C_BASIC_FUEL_QUARTER = "1";
var C_BASIC_FUEL_HALF = "2";
var C_BASIC_FUEL_THREE_QUARTER = "3";
var C_BASIC_FUEL_FULL = "4";
//オーディオ
var C_BASIC_AUDIO_OFF = "0";
var C_BASIC_AUDIO_CD = "1";
var C_BASIC_AUDIO_FM = "2";
//エアコン
var C_BASIC_AIR_CONDITIONER_OFF = "0";
var C_BASIC_AIR_CONDITIONER_ON = "1";
//付属品
var C_BASIC_ACCESSORY_CHECKED = "1";
var C_BASIC_ACCESSORY_MAX = 6;

//ご用命事項・確認事項のデータ定数
//交換部品
var C_ORDER_EXCHANGE_PARTS_TAKEOUT = "0";
var C_ORDER_EXCHANGE_PARTS_INSURANCE = "1";
var C_ORDER_EXCHANGE_PARTS_DISPOSE = "2";
//待ち方
var C_ORDER_WAITING_IN = "0";
var C_ORDER_WAITING_OUT = "1";
//洗車
var C_ORDER_WASHING_DO = "1";
var C_ORDER_WASHING_NONE = "0";
//支払方法
var C_ORDER_PAYMENT_CASH = "0";
var C_ORDER_PAYMENT_CARD = "1";
var C_ORDER_PAYMENT_OTHER = "2";
//CSI時間
var C_ORDER_CSI_AM = "1";
var C_ORDER_CSI_PM = "2";
var C_ORDER_CSI_ALWAYS = "0";

//ご用命事項・問診項目のデータ定数
//WNG
var C_ORDER_WNG_ALWAYS = "1";
var C_ORDER_WNG_OFTEN = "2";
var C_ORDER_WNG_NONE = "0";
//故障発生時間
var C_ORDER_OCCURRENCE_RECENTLY = "0";
var C_ORDER_OCCURRENCE_WEEK = "1";
var C_ORDER_OCCURRENCE_OTHER = "2";
//故障発生頻度
var C_ORDER_FREQUENCY_HIGH = "0";
var C_ORDER_FREQUENCY_OFTEN = "1";
var C_ORDER_FREQUENCY_ONCE = "2";
//再現可能
var C_ORDER_REAPPEAR_YES = "1";
var C_ORDER_REAPPEAR_NO = "0";
//水温
var C_ORDER_WATERT_LOW = "0";
var C_ORDER_WATERT_HIGH = "1";
//気温
var C_ORDER_TEMPERATURE_LOW = "0";
var C_ORDER_TEMPERATURE_HIGH = "1";
//発生場所
var C_ORDER_PLACE_PARKING = "0";
var C_ORDER_PLACE_ORDINARY = "1";
var C_ORDER_PLACE_MOTORWAY = "2";
var C_ORDER_PLACE_SLOPE = "3";
//渋滞状況
var C_ORDER_TRAFFICJAM_HAPPEN = "1";
var C_ORDER_TRAFFICJAM_NONE = "0";
//車両状態
var C_ORDER_CARSTATUS_ON = "1";
var C_ORDER_CARSTATUS_OFF = "0";
//var C_ORDER_CARSTATUS_STARTUP = "1";
//var C_ORDER_CARSTATUS_IDLLING = "2";
//var C_ORDER_CARSTATUS_COLD = "3";
//var C_ORDER_CARSTATUS_WARM = "4";
//走行時
var C_ORDER_TRAVELING_LOWSPEED = "0";
var C_ORDER_TRAVELING_ACCELERATION = "1";
var C_ORDER_TRAVELING_SLOWDOWN = "2";
////車両状態、操作状態1
//var C_ORDER_CARCONTROL_PARKING = "1";
//var C_ORDER_CARCONTROL_ADVANCE = "2";
//var C_ORDER_CARCONTROL_SHIFTCHANGE = "3";
////車両状態、操作状態2
//var C_ORDER_CARCONTROL_BACK = "1";
//var C_ORDER_CARCONTROL_BRAKE = "2";
//var C_ORDER_CARCONTROL_DETOUR = "3";
//非純正用品
var C_ORDER_NONGENUINE_YES = "1";
var C_ORDER_NONGENUINE_NO = "0";

//部品準備が完了していない状態
var C_PARTS_REPARE_UNPREPARED = "0";
//部品準備が完了している状態
var C_PARTS_REPARE_PREPARED = "1";

//左フリックをしたと判定する値
var C_LEFT_FLICK_THRESHOLD = -200;

//R/O情報欄のフィルタフラグ：フィルタをかける
var C_REPAIR_ORDER_FILTER_ON = "1";
//R/O情報欄のフィルタフラグ：フィルタをかけない
var C_REPAIR_ORDER_FILTER_OFF = "0";

//フリック移動距離
var gDiffX = 0;

//追加作業アイコンを一度に表示可能な数
var C_ICON_DISPLAY_LIMIT = 3;
//追加作業アイコンの1アイコンに必要な描画幅

//2012/03/05 日比野　作業追加アイコン制御対応 START
//var C_ICON_WIDTH = 31;
var C_ICON_WIDTH = 32;
//2012/03/05 日比野　作業追加アイコン制御対応 END

var C_ICON_SCROLLMAX = 92;
//追加作業アイコンを移動するときの動作時間
var C_ICON_MOVE_TIME = 1;

//追加作業アイコンを配置するBoxの幅
var gPagingDivMaxLen = 1;
var gScrollNowLen = 0;
//追加作業アイコンの移動変数
var gScrollNumber = 0;

//2012/03/05 日比野　作業追加アイコン制御対応 START
//追加作業アイコンの選択されているインデックス
var selectedAddWorkIndex = 0;
//追加作業アイコンの表示数
var maxAddWorkIndex = 0
//2012/03/05 日比野　作業追加アイコン制御対応 END

//DOMロード直後の処理(重要事項)
$(function () {

    //スモークフィルタの設置フラグを取得
    var str01Box03FilterFlag = $("#Hidden01Box03Filter").val();
    if (str01Box03FilterFlag == C_REPAIR_ORDER_FILTER_OFF) {
        $(".stc01Box03").css("opacity", 1);
    } else {
        $(".stc01Box03").css("opacity", 0.5);
    }
    //部品準備のスモークフィルタの設置処理
    var strPartsReadyFlag = $("#HiddenFieldPartsReady").val();
    if (strPartsReadyFlag == C_PARTS_REPARE_PREPARED) {
        //部品詳細の実体化
        $("#S-TC-01RightBody").css("opacity", "1");
        //部品詳細フィルタの透明化
        $(".S-TC-01RightScrollFilter").css("opacity", "0");
    } else {
        //部品詳細の半透明化
        $("#S-TC-01RightBody").css("opacity", "0.5");
        //部品詳細フィルタの実体化
        $(".S-TC-01RightScrollFilter").css("opacity", "1");
    }


    //基本情報・ご用命事項・作業内容パネルの選択処理をクリックイベントにバインドする.
    $(".Box03In > .TabButtonSet > ul > li").bind("touchstart click", function () {
        clickTabButtonSet(this);
    });

    //ご用命事項の確認事項・問診項目の選択処理をクリックイベントにバインドする.
    $(".TabBox02 > .S-TC-07TabWrap > .S-TC-07Right > .S-TC-07RightTab > ul > li").bind("click", function () {
        clickOrderTab(this);
    });

    //ご用命事項の問診項目の「走行時」をクリックしたときの動作をバインドする.
    $("#S-SA-07Tab02Right1-5-1").bind("touchstart click", function () {
        clickOrderTabTraveling();
    });

    //履歴情報をタップした時の処理をバインドする.
    $("#S-TC-05RightScroll > .S-TC-05Right1-1").bind("touchstart click", function () {
        clickHistory(this);
    });

    //基本情報のデータ表示設定
    initBasicInfo();
    //作業内容の追加作業アイコンの作成
    createChildChipIcon();
    //追加作業アイコンの表示
    scrollOnload();

    //基本情報の入庫履歴にフリックイベントを設定.
    //基本情報の入庫履歴は、最大5件ということなので、そもそもスクロール可能とならない.
    //少しでも負担を減らすため、flickableは設定しない.
    //$("#S-TC-05RightScroll").flickable();
    //ご用命事項のご用命事項欄にフリックイベントを設定.
    $('#S-TC-07LeftMemo2').flickable();
    //ご用命事項の各タブにフリックイベントを設定.
    $('#S-TC-07RightBody').flickable();
    $('#S-TC-07RightScroll').flickable();
    //作業内容の各テーブルにフリックイベントを設定.
    $("#S-TC-01LeftBody").flickable();
    $("#S-TC-01RightBody").flickable();

    //R/O情報パネル全体にフリックイベントを設定.
    $(".Box03In").bind("touchstart mousedown", function (event) {
        flickStart(event);
    });

    //フリックイベント設定時、display:noneに設定されているとその箇所はフリックできないため、
    //初期状態では表示しておき、フリックイベント設定後、デフォルト以外を非表示設定にする.
    $("#S-TC-07RightTab_01").click();           //ご用命事項タブ・確認事項タブ押下処理
    $("." + C_ROTAB_CLASS_WORK).click();  //作業内容タブ押下処理
    $("#S-SA-07Tab02Right1-5-1").click();       //走行時ボタン押下処理

    //親ページのR/Oステータスに値を格納する.
    parent.setOrderStatus($("#HiddenFieldOrderStatus").val())
    //親ページに担当SA名を投げる.
    parent.setSaName($("#HiddenFieldSAName").val())

    //親ページでポップアップの制御を実施する.
    $("*").bind("click.popover", function (event) {
        parent.ParentPopoverClose();
    });

    //2012/03/02 上田 フッタボタン制御 Start
    //リロードが終了したタイミングで親のリロードフラグをOFFにする
    parent.InitReloadFlag();
    //2012/03/02 上田 フッタボタン制御 End

    //2012/03/03 上田 ロード中アイコン制御 Start
    parent.StopLodingIcon('#loadingroInfomation');
    //2012/03/03 上田 ロード中アイコン制御 End
});


/**
 * R/O情報欄におけるフリック開始イベント処理
 * @param {event} event
 * @return {void}
 */
function flickStart(event) {

    //開始位置座標値を取得する.
    if (event.type === "touchstart") {
        startX = event.originalEvent.touches[0].pageX;
    } else {
        startX = event.pageX;
    }

    //開始イベントをアンバインドし、移動・終了イベントをバインドする.
    $(".Box03In").unbind("touchstart mousedown")
    .bind("touchmove mousemove", function (event) {
        flickMove(event);
    }).bind("touchend mouseup mouseleave", function (event) {
        flickEnd(event)
    });
}

/**
 * R/O情報欄における、フリック移動イベント処理
 * @param {event} event
 * @return {void}
 */
function flickMove(event) {

    //移動座標を取得し、開始座標からの差異を取得する.
    if (event.type === "touchmove") {
        pointX = event.originalEvent.touches[0].pageX;
    } else {
        pointX = event.pageX;
    }
    gDiffX = pointX - startX;
}

/**
 * R/O情報欄における、フリック終了イベント処理
 * @param {event} event
 * @return {void}
 */
function flickEnd(event) {
    
    //移動・終了イベントをアンバインドし、開始イベントをバインドする.
    $(".Box03In").unbind("touchmove mousemove touchend mouseup mouseleave")
    .bind("touchstart mousedown", function(event) {
        flickStart(event);
    });

    //左フリックを実施したと判定される値より、移動距離が大きい場合、左フリック処理を実施する.
    if (gDiffX < C_LEFT_FLICK_THRESHOLD) {
        //親画面のR/O情報左フリック処理を呼び出す.
        parent.flickRepairOrderInfomation();
    }
}


/**
 * 基本情報・ご用命事項・作業内容パネルを選択時イベント処理
 * @param {Object} tapObject 選択されたオブジェクト
 * @return {void}
 */
function clickTabButtonSet(tapObject) {
    var clickClassName = $(tapObject).attr("class");
    var clickTabNumber = 0;

    var display01 = C_DISPLAY_NONE;
    var display02 = C_DISPLAY_NONE;
    var display03 = C_DISPLAY_NONE;

    //取得したクラス名がTabButton01の場合、タブナンバーに1を返す
    if (clickClassName == (C_ROTAB_CLASS_BASE)) {
        display01 = C_DISPLAY_BLOCK;
        clickTabNumber = C_ROTAB_CLASS_BASE_NUMBER;
    } else if (clickClassName == (C_ROTAB_CLASS_ORDER)) {
        display02 = C_DISPLAY_BLOCK;
        clickTabNumber = C_ROTAB_CLASS_ORDER_NUMBER;
    } else if (clickClassName == (C_ROTAB_CLASS_WORK)) {
        display03 = C_DISPLAY_BLOCK;
        clickTabNumber = C_ROTAB_CLASS_WORK_NUMBER;
    }
    $(".TabBox01").css("display", display01);
    $(".TabBox02").css("display", display02);
    $(".TabBox03").css("display", display03);

    $("." + C_ROTAB_CLASS_BASE + " > div").toggleClass("Rollover", (clickClassName == C_ROTAB_CLASS_BASE));
    $("." + C_ROTAB_CLASS_BASE + " > div").toggleClass("Button", (clickClassName != C_ROTAB_CLASS_BASE));
    $("." + C_ROTAB_CLASS_ORDER + " > div").toggleClass("Rollover", (clickClassName == C_ROTAB_CLASS_ORDER));
    $("." + C_ROTAB_CLASS_ORDER + " > div").toggleClass("Button", (clickClassName != C_ROTAB_CLASS_ORDER));
    $("." + C_ROTAB_CLASS_WORK + " > div").toggleClass("Rollover", (clickClassName == C_ROTAB_CLASS_WORK));
    $("." + C_ROTAB_CLASS_WORK + " > div").toggleClass("Button", (clickClassName != C_ROTAB_CLASS_WORK));

    //親画面に渡すための部品準備完了フラグを取得する
    var partsRepareFlag = $("#HiddenFieldPartsReady").val();
    //親画面に渡すための部品情報数を取得する.
    var partsCount = $("#HiddenFieldPartsCount").val();
    //親画面に渡すためのB/O数を取得する.
    var backOrderCount = $("#HiddenFieldPartsBackOrderCount").val();
    //親画面のタブ変更メソッドを呼び出す
    parent.CheckChengeTab(clickTabNumber, partsRepareFlag, partsCount, backOrderCount);
}


/**
 * ご用命事項の確認事項・問診項目の選択時のイベント処理
 * @param {Object} selectedOrderTab
 * @return {void}
 */
function clickOrderTab(selectedOrderTab) {

    //クリックされたIDを取得する
    var clickTabId = $(selectedOrderTab).attr("id");

    var rightTabDisplay01 = C_DISPLAY_NONE;
    var rightTabDisplay02 = C_DISPLAY_NONE;
    //確認事項タブである場合、確認事項エリアを表示可能にする
    if (clickTabId == "S-TC-07RightTab_01") {
        rightTabDisplay01 = C_DISPLAY_BLOCK;
    } else if (clickTabId == "S-TC-07RightTab_02") {
        rightTabDisplay02 = C_DISPLAY_BLOCK;
    }

    $(".S-TC-07RightBody").css("display", rightTabDisplay01);
    $(".S-TC-07RightScroll").css("display", rightTabDisplay02);

    //確認事項タブでない場合、選択されていないクラスを追加（確認事項タブが選択された場合は除去）
    $("#S-TC-07RightTab_01").toggleClass("S-TC-07RightTabNoSelected", (clickTabId != "S-TC-07RightTab_01"));
    //問診項目タブでない場合、選択されていないクラスを追加（問診項目タブが選択された場合は除去）
    $("#S-TC-07RightTab_02").toggleClass("S-TC-07RightTabNoSelected", (clickTabId != "S-TC-07RightTab_02"));
}


/**
 * ご用命事項の「走行時」のタップイベント処理
 * @return {void}
 */
function clickOrderTabTraveling() {
    //「走行時」のdisplay情報を取得する
    var _display = $("#S-SA-07Tab02Right1-5-1Display").css("display");
    var _nextDisplay = C_DISPLAY_NONE;

    //display情報に応じて、走行時以降の表示状態を設定する
    if (_display == C_DISPLAY_BLOCK) {
        _nextDisplay = C_DISPLAY_NONE;
    } else {
        _nextDisplay = C_DISPLAY_BLOCK;
    }
    //変更後のdisplay状態を設定する
    $("#S-SA-07Tab02Right1-5-1Display").css("display", _nextDisplay);

    //走行時の非活性状態クラスの設定
    $("#S-SA-07Tab02Right1-5-1").toggleClass("S-SA-07Tab02Right1-5-1Off", (_nextDisplay == C_DISPLAY_NONE));
    //走行時の活性状態クラスの設定
    $("#S-SA-07Tab02Right1-5-1").toggleClass("S-SA-07Tab02Right1-5-1", (_nextDisplay == C_DISPLAY_BLOCK));
}


/**
* 活性状態の追加作業アイコンにより、表示位置を調整する.
* @param {Integer} divScrollMargin 
*/
//function colorContorl() {
//    //活性状態のアイコン番号を取得する（ROアイコンは1からスタート）.
//    //colorIndex = $('#colorValue').attr("value");
//    var colorIndex = changeIntegerFromString($("#HiddenFieldSelectedAddWork").val()) + 1;
//    //追加作業アイコン数を取得する.（ROアイコン分を加算）
//    var buttonNum = changeIntegerFromString($("#HiddenFieldAddWorkCount").val()) + 1;

//    //アイコン数が表示限界を超えている場合、表示位置を調節する.
//    //if (buttonNum > NUMBER2) {
//    var divScrollMargin = 0;
//    if (buttonNum > C_ICON_DISPLAY_LIMIT) {
//        //これ以上、右へスライドできない場合の表示調整を行う.
//        if (colorIndex > buttonNum - (C_ICON_DISPLAY_LIMIT - 1)) {
//            divScrollMargin = (buttonNum - C_ICON_DISPLAY_LIMIT) * C_ICON_WIDTH
//            $("#divScroll").css({ "margin-left": -divScrollMargin });
//        }
//        //これ以上、左へスライドできない場合と右へスライドできない場合を除いた場合の表示調整を行う.
//        else if (colorIndex >= C_ICON_DISPLAY_LIMIT) {
//            divScrollMargin = (colorIndex - (C_ICON_DISPLAY_LIMIT - 1)) * C_ICON_WIDTH
//            $("#divScroll").css({ "margin-left": -divScrollMargin });
//        }
//    }
//    return divScrollMargin;
//    //$("#" + colorIndex).removeClass("SSA16Block5-5PagingOff")
//    //$("#" + colorIndex).addClass("SSA16Block5-5PagingOn")
//}

/**
 * 追加作業アイコンの初期表示位置を決定し、初期表示する.
 * @return {void}
 */
function scrollOnload() {

    //枝番（親チップを含まない数量）
    changeImageLen = changeIntegerFromString($("#HiddenFieldAddWorkCount").val());

    //枝番がアイコンの表示上限未満の場合、＜＞ボタンを表示しない.
    if (changeImageLen < C_ICON_DISPLAY_LIMIT) {
        $("#S-TC-01Paging").css({ "background": "url()" })
    }
    //追加作業アイコンを格納するBoxの幅を設定する.
    gPagingDivMaxLen = C_ICON_WIDTH * (changeImageLen + 1)
    
    //2012/03/05 日比野　作業追加アイコン制御対応 START
    //$("#divScroll").css({ "width": gPagingDivMaxLen })
    $("#divScroll ul").css({ "width": gPagingDivMaxLen })
	//2012/03/05 日比野　作業追加アイコン制御対応 END
	
	//2012/03/05 日比野　作業追加アイコン制御対応 START
	////????
    //gPagingDivMaxLen = gPagingDivMaxLen - C_ICON_SCROLLMAX
    ////IO
    //if (changeImageLen > 1) {
    //    $("#S-TC-01Paging li:last").bind("click", changeImageAdd);
    //    $("#S-TC-01Paging li:first").bind("click", changeImage);
    //}
    var element = $('#divScroll').flickable({
        section: 'li'
    });

    //活性状態のアイコン番号を取得する（0から）.
    selectedAddWorkIndex = changeIntegerFromString($("#HiddenFieldSelectedAddWork").val());
    //追加作業アイコン数を取得する.（ROアイコン分を加算）
    maxAddWorkIndex = changeIntegerFromString($("#HiddenFieldAddWorkCount").val()) + 1;

    //追加作業アイコンの表示位置初期設定.
    element.flickable('select', selectedAddWorkIndex);

    //＜＞ボタンのクリック処理.
    $("#S-TC-01Paging > li.liFast").click(function () {
        if (selectedAddWorkIndex <= maxAddWorkIndex - 4) {

            selectedAddWorkIndex++
            element.flickable('select', selectedAddWorkIndex);
            return false;
        }
    });

    $("#S-TC-01Paging > li.liLast").click(function () {
        if (selectedAddWorkIndex > 0) {

            selectedAddWorkIndex--
            element.flickable('select', selectedAddWorkIndex);
            return false;
        }
    });
    //2012/03/05 日比野　作業追加アイコン制御対応 END
}




///**
//* 追加作業アイコンを右に移動.
//* @param {void}
//*/
//function changeImageAdd() {
//    // 2012/03/02 KN 日比野【SERVICE_1】START
//    //    gScrollNowLen++
//    //    if (gScrollNumber < C_ICON_WIDTH && gScrollNowLen < 1) {
//    //        gScrollNumber++
//    //        $("#divScroll").css({ "margin-left": gScrollNowLen });
//    //        setTimeout("changeImageAdd()", C_ICON_MOVE_TIME)
//    //    }
//    //    else {
//    //        gScrollNowLen--
//    //        gScrollNumber = 0
//    //    }

////    gScrollNowLen = gScrollNowLen + 4
////    if (gScrollNumber < C_ICON_WIDTH && gScrollNowLen < 1) {
////        gScrollNumber = gScrollNumber + 4
////        $("#divScroll").css({ "margin-left": gScrollNowLen });
////        setTimeout("changeImageAdd()", C_ICON_MOVE_TIME)
////    }
////    else {
////        if (gScrollNumber >= C_ICON_WIDTH) {
////            var wkNum = gScrollNumber + 4 - C_ICON_WIDTH
////            $("#divScroll").css({ "margin-left": gScrollNowLen - wkNum });
////        } else if (gScrollNowLen >= 1) {
////            $("#divScroll").css({ "margin-left": 1 });
////        }

////        gScrollNowLen = gScrollNowLen - 4
////        gScrollNumber = 0
////    }

//    if (selectedAddWorkIndex > 0) {
//        selectedAddWorkIndex--

//        $('#divScroll').flickable('select', index);

//    }

//    // 2012/03/02 KN 日比野【SERVICE_1】END
//}


///**
//* 追加作業アイコンを左に移動.
//* @param {void}
//*/
//function changeImage() {
//    // 2012/03/02 KN 日比野【SERVICE_1】START
//    //    gScrollNowLen--
//    //    if (gScrollNumber < C_ICON_WIDTH && -gScrollNowLen < gPagingDivMaxLen) {
//    //        gScrollNumber++
//    //        $("#divScroll").css({ "margin-left": gScrollNowLen });
//    //        setTimeout("changeImage()", C_ICON_MOVE_TIME)
//    //    }
//    //    else {
//    //        gScrollNowLen++
//    //        gScrollNumber = 0
//    //    }

////    gScrollNowLen = gScrollNowLen - 4
////    if (gScrollNumber < C_ICON_WIDTH && -gScrollNowLen < gPagingDivMaxLen) {
////        gScrollNumber = gScrollNumber + 4
////        $("#divScroll").css({ "margin-left": gScrollNowLen });
////        setTimeout("changeImage()", C_ICON_MOVE_TIME)
////    }
////    else {
////        if (gScrollNumber >= C_ICON_WIDTH) {
////            var wkNum = gScrollNumber + 4 - C_ICON_WIDTH
////            $("#divScroll").css({ "margin-left": gScrollNowLen + wkNum });
////            gScrollNowLen = gScrollNowLen + wkNum
////        } else if (-gScrollNowLen >= gPagingDivMaxLen) {
////            $("#divScroll").css({ "margin-left": -gPagingDivMaxLen });
////            gScrollNowLen = -gPagingDivMaxLen
////        } else {
////            gScrollNowLen = gScrollNowLen + 4
////        }

////        gScrollNumber = 0
//    //    }

//    if (selectedAddWorkIndex < maxAddWorkIndex) {
//        selectedAddWorkIndex++

//        $('#divScroll').flickable('select', index);

//    }
//    // 2012/03/02 KN 日比野【SERVICE_1】END
//}


/**
 * 作業内容タブの、R/O追加作業チップを作成する.
 * @return {void}
 */
function createChildChipIcon() {

    //追加作業の数量を取得する.
    //親チップを含まない数量とする.即ち、最小値0とする.
    var addWorkCount = changeIntegerFromString($("#HiddenFieldAddWorkCount").val());
    //現在、表示する作業のchildNumberを取得する.
    //chileNumberは、1orNull値で親チップ（R）、2以降で（childNumber-1）番目の追加作業とする.
    var childNumber = changeIntegerFromString($("#HiddenFieldSelectedAddWork").val());
    //chileNumberと追加作業の数量インデックスと同期させるため、取得した値を調整する.
    if (childNumber <= 0) {
        childNumber = 0;
    //} else {
    //    childNumber = childNumber - 1;
    }
    //アイコンのページ番号を取得する.
    var iconPage = 0;
    //追加作業の数量が0以下の場合、アイコンページ番号を0のままとする。
    //それ以外の場合、個番号を3で割った整数値がページ番号とする.
    //if (addWorkCount > 0) {
    //    iconPage = Math.floor(childNumber / C_ICON_DISPLAY_LIMIT);
    //}

    //作業内容タブのR/O追加作業アイコンを生成し、紐付ける.
    appendChildChipIcon(addWorkCount, childNumber, iconPage);

    //タップイベントをバインドする.
    //2012/03/05 日比野　作業追加アイコン制御対応 START
    //$("#divScroll > div").bind("touchstart click", function () {
    //    var tapIconNumber = $("#divScroll > div").index(this);
    //    parent.tapRepairOrderIcon(tapIconNumber, childNumber);
    //});
    $("#divScroll li").bind("touchstart click", function () {
    	var tapIconNumber = $("#divScroll li").index(this);
    	parent.tapRepairOrderIcon(tapIconNumber, childNumber);
    });
    //2012/03/05 日比野　作業追加アイコン制御対応 END 
}


/**
 * 作業内容タブのR/O追加作業アイコンを生成し、紐付ける.
 * @param {Integer} childChipCount 追加作業総数
 * @param {Integer} selectedChipNumber 選択されている子番号
 * @param {Integer} iconPage アイコンの表示ページ番号（0スタート）
 * @return {void}
 */
function appendChildChipIcon(childChipCount, selectedChipNumber, iconPage) {

    //R/O追加作業チップを配置する親要素を取得する.
    //var elementParent = $("#S-TC-01Paging");
    var elementParent = $("#divScroll");

    //ループ上限を設定する.
    //var limitPageCount = ((iconPage + 1) * C_ICON_DISPLAY_LIMIT) - 1;
    //if (childChipCount < limitPageCount) {
    //    limitPageCount = childChipCount;
    //}
    var limitPageCount = childChipCount;
    
	//2012/03/05 日比野　作業追加アイコン制御対応 START
    var elementUl = $("<ul />");
	//2012/03/05 日比野　作業追加アイコン制御対応 END
	
    //追加作業の数量分処理をループさせる.
    for (var i = (iconPage * C_ICON_DISPLAY_LIMIT); i <= limitPageCount; i++) {
    
    	//2012/03/05 日比野　作業追加アイコン制御対応 START
        //<li>要素のオブジェクトを作成する.
        var elementList = $("<li />");
        //2012/03/05 日比野　作業追加アイコン制御対応 END
        
        //チップに付与するCSSクラス名を定義する.
        //ループインデックスとchildNumberが同値となる場合、CSSの活性クラスを指定する.それ以外は、非活性クラスを指定する.
        var appendCssClass = "S-TC-01PagingOff";
        if (i == selectedChipNumber) {
            appendCssClass = "S-TC-01PagingOn";
        }
        //<div>要素のオブジェクトを作成し、作成したオブジェクトに、CSSのクラスを付与する
        var elementDiv = $("<div />").addClass(appendCssClass);

        if (i == 0) {
            //ループインデックスが0の場合、リペアオーダーのイニシャル文字をDivタグにテキストとして格納する.
            elementDiv.text($("#HiddenFieldRepairOrderInitialWord").val());
        }
        else {
            //追加作業の場合、現在のループインデックスを<span>タグのテキストとし、<div>要素の子要素として追加する.
            var elementSpan = $("<span />");
            elementSpan.text(i.toString());
            elementDiv.append(elementSpan);
        }
        //2012/03/05 日比野　作業追加アイコン制御対応 START
        elementList.append(elementDiv);
        //親要素の<ul>タグに、生成された<li>要素を紐付ける.
        elementUl.append(elementList);
        //2012/03/05 日比野　作業追加アイコン制御対応 END
    }
    //2012/03/05 日比野　作業追加アイコン制御対応 START
    elementParent.append(elementUl);
    //2012/03/05 日比野　作業追加アイコン制御対応 END
}


function changeIntegerFromString(stringData) {
    var integerValue;
    try {
        integerValue = Number(stringData);
        if (integerValue == NaN) {
            integerValue = 0;
        }
        return integerValue;
    }
    catch (e) {
        integerValue = 0;
        return integerValue;
    }
}


/**
 * 履歴情報をタップした際の処理.
 * @return {void}
 */
function clickHistory(selectedHistory) {
    var orderNumber = $(selectedHistory).children("#HiddenFieldHOrderNo").val()
    if (orderNumber != "") {
        parent.tapHistory(orderNumber);
    }
}


/**
 * 基本情報における情報の表示をする
 * @return {void}
 */
function initBasicInfo() {

    //燃料情報の設定
    setBasicFuelInfo();
    //オーディオ情報の設定
    setBasicAudioInfo();
    //エアコン情報の設定
    setBasicAirConditionerInfo();
    //付属品情報の設定
    setBasicAccessoryInfo();

    //交換部品情報の設定
    setOrderExchangePartsInfo();
    //待ち方の設定
    setOrderWaitingInfo();
    //洗車の設定
    setOrderWashingInfo();
    //支払方法の設定
    setOrderPaymentInfo();
    //CSI時間の設定
    setOrderCSIInfo();

    //WNGの設定
    setOrderWNGInfo();
    //故障発生時間の設定
    setOrderOccurrenceInfo();
    //故障発生頻度の設定
    setOrderFrequencyInfo();
    //再現可能
    setOrderReappearInfo();
    //水温
    setOrderWaterTemperatureInfo();
    //気温
    setOrderTemperatureInfo();
    //発生場所
    setOrderPlaceInfo();
    //渋滞状況
    setOrderTrafficjamInfo();
    //車両状態
    setOrderCarStatusInfo();
    //車両状態、走行時
    setOrderTravelingInfo();
//    //車両状態、操作状況1
//    setOrderCarControl1Info();
//    //車両状態、操作状況2
//    setOrderCarControl2Info();
    //非純正用品
    setOrderNonGenuineInfo();
}


/**
 * 基本情報・初期状態の燃料情報を表示する.
 * @return {void}
 */
function setBasicFuelInfo() {

    //燃料情報の取得.
    var _fuelValue = $("#HiddenField05_Fuel").val();

    //燃料の1メモリ目のCSSを設定する.
    $("#TC05_Fuel01").toggleClass("S-TC-05Left2-3-1On2", (_fuelValue == C_BASIC_FUEL_QUARTER));
    $("#TC05_Fuel01").toggleClass("S-TC-05Left2-3-1On", ((_fuelValue == C_BASIC_FUEL_HALF) || (_fuelValue == C_BASIC_FUEL_THREE_QUARTER) || (_fuelValue == C_BASIC_FUEL_FULL)));
    $("#TC05_Fuel01").toggleClass("S-TC-05Left2-3-1Off", ((_fuelValue != C_BASIC_FUEL_QUARTER) && (_fuelValue != C_BASIC_FUEL_HALF) && (_fuelValue != C_BASIC_FUEL_THREE_QUARTER) && (_fuelValue != C_BASIC_FUEL_FULL)));

    //燃料の2メモリ目のCSSを設定する.
    $("#TC05_Fuel02").toggleClass("S-TC-05Left2-3-2On2", (_fuelValue == C_BASIC_FUEL_HALF));
    $("#TC05_Fuel02").toggleClass("S-TC-05Left2-3-2On", ((_fuelValue == C_BASIC_FUEL_THREE_QUARTER) || (_fuelValue == C_BASIC_FUEL_FULL)));
    $("#TC05_Fuel02").toggleClass("S-TC-05Left2-3-2Off", ((_fuelValue != C_BASIC_FUEL_HALF) && (_fuelValue != C_BASIC_FUEL_THREE_QUARTER) && (_fuelValue != C_BASIC_FUEL_FULL)));

    //燃料の3メモリ目のCSSを設定する.
    $("#TC05_Fuel03").toggleClass("S-TC-05Left2-3-3On2", (_fuelValue == C_BASIC_FUEL_THREE_QUARTER));
    $("#TC05_Fuel03").toggleClass("S-TC-05Left2-3-3On", (_fuelValue == C_BASIC_FUEL_FULL));
    $("#TC05_Fuel03").toggleClass("S-TC-05Left2-3-3Off", ((_fuelValue != C_BASIC_FUEL_THREE_QUARTER) && (_fuelValue != C_BASIC_FUEL_FULL)));

    //燃料の4メモリ目のCSSを設定する.
    $("#TC05_Fuel04").toggleClass("S-TC-05Left2-3-4On", (_fuelValue == C_BASIC_FUEL_FULL));
    $("#TC05_Fuel04").toggleClass("S-TC-05Left2-3-4Off", (_fuelValue != C_BASIC_FUEL_FULL));
}

/**
 * 基本情報・初期状態のオーディオ情報を表示する
 * @return {void}
 */
function setBasicAudioInfo() {
    
    //オーディオ情報の取得
    var _audio = $("#HiddenField05_Audio").val();

    //オーディオ「オフ」の設定
    $("#TC05_AudioOff").toggleClass("S-TC-05Left2-6-1On", (_audio == C_BASIC_AUDIO_OFF));
    $("#TC05_AudioOff").toggleClass("S-TC-05Left2-6-1Off", (_audio != C_BASIC_AUDIO_OFF));

    //オーディオ「CD」の設定
    $("#TC05_AudioCD").toggleClass("S-TC-05Left2-6-2On", (_audio == C_BASIC_AUDIO_CD));
    $("#TC05_AudioCD").toggleClass("S-TC-05Left2-6-2Off", (_audio != C_BASIC_AUDIO_CD));

    //オーディオ「FM」の設定
    $("#TC05_AudioFM").toggleClass("S-TC-05Left2-6-3On", (_audio == C_BASIC_AUDIO_FM));
    $("#TC05_AudioFM").toggleClass("S-TC-05Left2-6-3Off", (_audio != C_BASIC_AUDIO_FM));
}


/**
* 基本情報・初期状態のエアコン情報を表示する
* @return {void}
*/
function setBasicAirConditionerInfo() {
    
    //エアコン情報の取得
    var _air = $("#HiddenField05_AirConditioner").val();

    //エアコン「オフ」の設定
    $("#TC05_AirConditionerOff").toggleClass("S-TC-05Left2-8-1On", (_air == C_BASIC_AIR_CONDITIONER_OFF));
    $("#TC05_AirConditionerOff").toggleClass("S-TC-05Left2-8-1Off", (_air != C_BASIC_AIR_CONDITIONER_OFF));

    //エアコン「オン」の設定
    $("#TC05_AirConditionerOn").toggleClass("S-TC-05Left2-8-2On", (_air == C_BASIC_AIR_CONDITIONER_ON));
    $("#TC05_AirConditionerOn").toggleClass("S-TC-05Left2-8-2Off", (_air != C_BASIC_AIR_CONDITIONER_ON));
}


/**
* 基本情報・初期状態の付属品情報を表示する
* @return {void}
*/
function setBasicAccessoryInfo() {

    //付属品の数だけループ処理をする    
    for (var i=1; i<=C_BASIC_ACCESSORY_MAX; i++) {
        //付属品情報の取得
        var _accessory = $("#HiddenField05_Accessory" + i.toString()).val();

        //付属品情報の設定
        $("#TC05_Accessory" + i.toString()).toggleClass("S-TC-05Left2-9Checked", (_accessory == C_BASIC_ACCESSORY_CHECKED));
    }
}


/**
* ご用命事項・確認事項の交換部品情報を表示する
* @return {void}
*/
function setOrderExchangePartsInfo() {
    
    //交換部品情報を取得
    var _parts = $("#HiddenField07_ExchangeParts").val();

    //交換部品「持帰り」の設定
    $("#TC07_ExchangeParts1").toggleClass("S-TC-07Right01-1", (_parts == C_ORDER_EXCHANGE_PARTS_TAKEOUT));
    $("#TC07_ExchangeParts1").toggleClass("S-TC-07Right01-1Off", (_parts != C_ORDER_EXCHANGE_PARTS_TAKEOUT));

    //交換部品「保険提出」の設定
    $("#TC07_ExchangeParts2").toggleClass("S-TC-07Right01-2", (_parts == C_ORDER_EXCHANGE_PARTS_INSURANCE));
    $("#TC07_ExchangeParts2").toggleClass("S-TC-07Right01-2Off", (_parts != C_ORDER_EXCHANGE_PARTS_INSURANCE));

    //交換部品「店内処分」の設定
    $("#TC07_ExchangeParts3").toggleClass("S-TC-07Right01-3", (_parts == C_ORDER_EXCHANGE_PARTS_DISPOSE));
    $("#TC07_ExchangeParts3").toggleClass("S-TC-07Right01-3Off", (_parts != C_ORDER_EXCHANGE_PARTS_DISPOSE));
}


/**
* ご用命事項・確認事項の待ち方情報を表示する
* @return {void}
*/
function setOrderWaitingInfo() {
    
    //待ち方情報を取得
    var _waiting = $("#HiddenField07_Waiting").val();

    //待ち方「店内」の設定
    $("#TC07_WaitingIn").toggleClass("S-TC-07Right02-1", (_waiting == C_ORDER_WAITING_IN));
    $("#TC07_WaitingIn").toggleClass("S-TC-07Right02-1Off", (_waiting != C_ORDER_WAITING_IN));

    //持ち方「店外」の設定
    $("#TC07_WaitingOut").toggleClass("S-TC-07Right02-2", (_waiting == C_ORDER_WAITING_OUT));
    $("#TC07_WaitingOut").toggleClass("S-TC-07Right02-2Off", (_waiting != C_ORDER_WAITING_OUT));
}


/**
* ご用命事項・確認事項の洗車情報を表示する
* @return {void}
*/
function setOrderWashingInfo() {
    
    //洗車情報を取得
    var _washing = $("#HiddenField07_Washing").val();

    //洗車「する」の設定
    $("#TC07_WashingDo").toggleClass("S-TC-07Right02-1", (_washing == C_ORDER_WASHING_DO));
    $("#TC07_WashingDo").toggleClass("S-TC-07Right02-1Off", (_washing != C_ORDER_WASHING_DO));

    //洗車「しない」の設定
    $("#TC07_WashingNone").toggleClass("S-TC-07Right02-2", (_washing == C_ORDER_WASHING_NONE));
    $("#TC07_WashingNone").toggleClass("S-TC-07Right02-2Off", (_washing != C_ORDER_WASHING_NONE));
}


/**
* ご用命事項・確認事項の支払方法情報を表示する
* @return {void}
*/
function setOrderPaymentInfo() {
    
    //支払方法情報を取得
    var _payment = $("#HiddenField07_Payment").val();

    //支払方法「現金」の設定
    $("#TC07_PaymentCash").toggleClass("S-TC-07Right01-1", (_payment == C_ORDER_PAYMENT_CASH));
    $("#TC07_PaymentCash").toggleClass("S-TC-07Right01-1Off", (_payment != C_ORDER_PAYMENT_CASH));

    //支払方法「カード」の設定
    $("#TC07_PaymentCard").toggleClass("S-TC-07Right01-2", (_payment == C_ORDER_PAYMENT_CARD));
    $("#TC07_PaymentCard").toggleClass("S-TC-07Right01-2Off", (_payment != C_ORDER_PAYMENT_CARD));

    //支払方法「その他」の設定
    $("#TC07_PaymentOther").toggleClass("S-TC-07Right01-3", (_payment == C_ORDER_PAYMENT_OTHER));
    $("#TC07_PaymentOther").toggleClass("S-TC-07Right01-3Off", (_payment != C_ORDER_PAYMENT_OTHER));
}


/**
* ご用命事項・確認事項のCSI時間情報を表示する
* @return {void}
*/
function setOrderCSIInfo() {

    //CSI時間情報の取得
    var _csi = $("#HiddenField07_Csi").val();

    //CSI時間「午前」の設定
    $("#TC07_CSI_AM").toggleClass("S-TC-07Right01-1", (_csi == C_ORDER_CSI_AM));
    $("#TC07_CSI_AM").toggleClass("S-TC-07Right01-1Off", (_csi != C_ORDER_CSI_AM));

    //CSI時間「午後」の設定
    $("#TC07_CSI_PM").toggleClass("S-TC-07Right01-2", (_csi == C_ORDER_CSI_PM));
    $("#TC07_CSI_PM").toggleClass("S-TC-07Right01-2Off", (_csi != C_ORDER_CSI_PM));

    //CSI時間「指定なし」の設定
    $("#TC07_CSI_Always").toggleClass("S-TC-07Right01-3", (_csi == C_ORDER_CSI_ALWAYS));
    $("#TC07_CSI_Always").toggleClass("S-TC-07Right01-3Off", (_csi != C_ORDER_CSI_ALWAYS));
}


/**
* ご用命事項・問診項目のWNG情報を表示する.
* @return {void}
*/
function setOrderWNGInfo() {

    //WNG情報の取得.
    var _wng = $("#HiddenField07_Warning").val();

    //WNG「常時点灯」の設定.
    $("#TC07_WNG_Always").toggleClass("S-TC-07Right01-1", (_wng == C_ORDER_WNG_ALWAYS));
    $("#TC07_WNG_Always").toggleClass("S-TC-07Right01-1Off", (_wng != C_ORDER_WNG_ALWAYS));

    //WNG「頻繁に点灯」の設定.
    $("#TC07_WNG_Often").toggleClass("S-TC-07Right01-2", (_wng == C_ORDER_WNG_OFTEN));
    $("#TC07_WNG_Often").toggleClass("S-TC-07Right01-2Off", (_wng != C_ORDER_WNG_OFTEN));

    //WNG「表示なし」の設定.
    $("#TC07_WNG_None").toggleClass("S-TC-07Right01-3", (_wng == C_ORDER_WNG_NONE));
    $("#TC07_WNG_None").toggleClass("S-TC-07Right01-3Off", (_wng != C_ORDER_WNG_NONE));
}


/**
* ご用命事項・問診項目の故障発生時間情報を表示する.
* @return {void}
*/
function setOrderOccurrenceInfo() {

    //故障発生時間の取得.
    var _occurrence = $("#HiddenField07_Occurrence").val();

    //故障発生時間「最近」の設定.
    $("#TC07_Occurrence_Recently").toggleClass("S-TC-07Right01-1", (_occurrence == C_ORDER_OCCURRENCE_RECENTLY));
    $("#TC07_Occurrence_Recently").toggleClass("S-TC-07Right01-1Off", (_occurrence != C_ORDER_OCCURRENCE_RECENTLY));

    //故障発生時間「一週間前」の設定.
    $("#TC07_Occurrence_Week").toggleClass("S-TC-07Right01-2", (_occurrence == C_ORDER_OCCURRENCE_WEEK));
    $("#TC07_Occurrence_Week").toggleClass("S-TC-07Right01-2Off", (_occurrence != C_ORDER_OCCURRENCE_WEEK));

    //故障発生時間「その他」の設定.
    $("#TC07_Occurrence_Other").toggleClass("S-TC-07Right01-3", (_occurrence == C_ORDER_OCCURRENCE_OTHER));
    $("#TC07_Occurrence_Other").toggleClass("S-TC-07Right01-3Off", (_occurrence != C_ORDER_OCCURRENCE_OTHER));
}


/**
* ご用命事項・問診項目の故障発生頻度情報を表示する.
* @return {void}
*/
function setOrderFrequencyInfo() {

    //故障発生頻度の取得.
    var _frequency = $("#HiddenField07_Frequency").val();

    //故障発生頻度「頻繁に」の設定.
    $("#TC07_Frequency_High").toggleClass("S-TC-07Right01-1", (_frequency == C_ORDER_FREQUENCY_HIGH));
    $("#TC07_Frequency_High").toggleClass("S-TC-07Right01-1Off", (_frequency != C_ORDER_FREQUENCY_HIGH));

    //故障発生頻度「時々」の設定.
    $("#TC07_Frequency_Often").toggleClass("S-TC-07Right01-2", (_frequency == C_ORDER_FREQUENCY_OFTEN));
    $("#TC07_Frequency_Often").toggleClass("S-TC-07Right01-2Off", (_frequency != C_ORDER_FREQUENCY_OFTEN));

    //故障発生頻度「一回だけ」の設定
    $("#TC07_Frequency_Once").toggleClass("S-TC-07Right01-3", (_frequency == C_ORDER_FREQUENCY_ONCE));
    $("#TC07_Frequency_Once").toggleClass("S-TC-07Right01-3Off", (_frequency != C_ORDER_FREQUENCY_ONCE));
}


/**
* ご用命事項・問診項目の再現可能情報を表示する.
* @return {void}
*/
function setOrderReappearInfo() {

    //再現可能情報の取得.
    var _reappear = $("#HiddenField07_Reappear").val();

    //再現可能「はい」の設定.
    $("#TC07_Reappear_Yes").toggleClass("S-TC-07Right02-1", (_reappear == C_ORDER_REAPPEAR_YES));
    $("#TC07_Reappear_Yes").toggleClass("S-TC-07Right02-1Off", (_reappear != C_ORDER_REAPPEAR_YES));

    //再現可能「いいえ」の設定
    $("#TC07_Reappear_No").toggleClass("S-TC-07Right02-2", (_reappear == C_ORDER_REAPPEAR_NO));
    $("#TC07_Reappear_No").toggleClass("S-TC-07Right02-2Off", (_reappear != C_ORDER_REAPPEAR_NO));
}


/**
* ご用命事項・問診項目の水温情報を表示する.
* @return {void}
*/
function setOrderWaterTemperatureInfo() {

    //水温情報の取得.
    var _water = $("#HiddenField07_WaterT").val();

    //水温「冷」の設定.
    $("#TC07_WaterT_Low").toggleClass("S-TC-07Right03-1", (_water == C_ORDER_WATERT_LOW));
    $("#TC07_WaterT_Low").toggleClass("S-TC-07Right03-1Off", (_water != C_ORDER_WATERT_LOW));

    //水温「熱」の設定.
    $("#TC07_WaterT_High").toggleClass("S-TC-07Right03-2", (_water == C_ORDER_WATERT_HIGH));
    $("#TC07_WaterT_High").toggleClass("S-TC-07Right03-2Off", (_water != C_ORDER_WATERT_HIGH));
}


/**
* ご用命事項・問診項目の気温情報を表示する.
* @return {void}
*/
function setOrderTemperatureInfo() {

    //気温情報の取得.
    var _temperature = $("#HiddenField07_Temperature").val();

    //気温「寒」の設定.
    $("#TC07_Temperature_Low").toggleClass("S-TC-07Right03-1", (_temperature == C_ORDER_TEMPERATURE_LOW));
    $("#TC07_Temperature_Low").toggleClass("S-TC-07Right03-1Off", (_temperature != C_ORDER_TEMPERATURE_LOW));

    //気温「暑」の設定.
    $("#TC07_Temperature_High").toggleClass("S-TC-07Right03-2", (_temperature == C_ORDER_TEMPERATURE_HIGH));
    $("#TC07_Temperature_High").toggleClass("S-TC-07Right03-2Off", (_temperature != C_ORDER_TEMPERATURE_HIGH));
}


/**
* ご用命事項・問診項目の発生場所情報を表示する.
* @return {void}
*/
function setOrderPlaceInfo() {

    //発生場所情報の取得.
    var _place = $("#HiddenField07_Place").val();

    //発生場所「駐車場」の設定.
    $("#TC07_Place_Parking").toggleClass("S-TC-07Right04-1", (_place == C_ORDER_PLACE_PARKING));
    $("#TC07_Place_Parking").toggleClass("S-TC-07Right04-1Off", (_place != C_ORDER_PLACE_PARKING));

    //発生場所「一般道路」の設定.
    $("#TC07_Place_Ordinary").toggleClass("S-TC-07Right04-2", (_place == C_ORDER_PLACE_ORDINARY));
    $("#TC07_Place_Ordinary").toggleClass("S-TC-07Right04-2Off", (_place != C_ORDER_PLACE_ORDINARY));

    //発生場所「高速道路」の設定.
    $("#TC07_Place_Motorway").toggleClass("S-TC-07Right04-3", (_place == C_ORDER_PLACE_MOTORWAY));
    $("#TC07_Place_Motorway").toggleClass("S-TC-07Right04-3Off", (_place != C_ORDER_PLACE_MOTORWAY));

    //発生場所「坂道」の設定.
    $("#TC07_Place_Slope").toggleClass("S-TC-07Right04-4", (_place == C_ORDER_PLACE_SLOPE));
    $("#TC07_Place_Slope").toggleClass("S-TC-07Right04-4Off", (_place != C_ORDER_PLACE_SLOPE));
}


/**
* ご用命事項・問診項目の渋滞状況情報を表示する.
* @return {void}
*/
function setOrderTrafficjamInfo() {

    //渋滞状況の取得.
    var _traffic = $("#HiddenField07_TrafficJam").val();

    //渋滞状況「あり」の設定.
    $("#TC07_Trafficjam_Happen").toggleClass("S-TC-07Right02-1", (_traffic == C_ORDER_TRAFFICJAM_HAPPEN));
    $("#TC07_Trafficjam_Happen").toggleClass("S-TC-07Right02-1Off", (_traffic != C_ORDER_TRAFFICJAM_HAPPEN));

    //渋滞状況「なし」の設定.
    $("#TC07_Trafficjam_None").toggleClass("S-TC-07Right02-2", (_traffic == C_ORDER_TRAFFICJAM_NONE));
    $("#TC07_Trafficjam_None").toggleClass("S-TC-07Right02-2Off", (_traffic != C_ORDER_TRAFFICJAM_NONE));
}


/**
* ご用命事項・問診項目の車両状態情報を表示する.
* @return {void}
*/
function setOrderCarStatusInfo() {

    //車両状態の取得.
    var _status = $("#HiddenField07_CarStatus").val();

    //車両状態「起動時」の設定.
    setOrderCarStatus("HiddenField07_CarStatus_Startup", "TC07_CarStatus_Startup", "S-TC-07Right04-1", "S-TC-07Right04-1Off");

    //車両状態「アイドル時」の設定.
    setOrderCarStatus("HiddenField07_CarStatus_Idling", "TC07_CarStatus_Idlling", "S-TC-07Right04-2", "S-TC-07Right04-2Off");
    
    //車両状態「冷間時」の設定.
    setOrderCarStatus("HiddenField07_CarStatus_Cold", "TC07_CarStatus_Cold", "S-TC-07Right04-3", "S-TC-07Right04-3Off");

    //車両状態「温間時」の設定.
    setOrderCarStatus("HiddenField07_CarStatus_Warm", "TC07_CarStatus_Warm", "S-TC-07Right04-4", "S-TC-07Right04-4Off");

    //「駐車時」の設定.
    setOrderCarStatus("HiddenField07_CarStatus_Parking", "TC07_CarControl1_Parking", "S-TC-07Right01-1", "S-TC-07Right01-1Off");

    //「前進時」の設定.
    setOrderCarStatus("HiddenField07_CarStatus_Advance", "TC07_CarControl1_Advance", "S-TC-07Right01-2", "S-TC-07Right01-2Off");

    //「変速時」の設定.
    setOrderCarStatus("HiddenField07_CarStatus_ShiftChange", "TC07_CarControl1_ShiftChange", "S-TC-07Right01-3", "S-TC-07Right01-3Off");

    //「後退時」の設定.
    setOrderCarStatus("HiddenField07_CarStatus_Back", "TC07_CarControl2_Back", "S-TC-07Right01-1", "S-TC-07Right01-1Off");

    //「ブレーキ時」の設定.
    setOrderCarStatus("HiddenField07_CarStatus_Brake", "TC07_CarControl2_Brake", "S-TC-07Right01-2", "S-TC-07Right01-2Off");

    //「迂回時」の設定.
    setOrderCarStatus("HiddenField07_CarStatus_Detour", "TC07_CarControl2_Detour", "S-TC-07Right01-3", "S-TC-07Right01-3Off");
}


/**
* ご用命事項・問診項目の車両状態を表示する.
* @param {String} aHiddenFieldId
* @param {String} aTargetId
* @param {String} aCssClassNameOn
* @param {String} aCssClassNameOff
* @return {void}
*/
function setOrderCarStatus(aHiddenFieldId, aTargetId, aCssClassNameOn, aCssClassNameOff) {

    //車両状態の情報取得.
    var _idlling = $("#" + aHiddenFieldId).val();

    //車両状態の情報を表示する.
    $("#" + aTargetId).toggleClass(aCssClassNameOn, (_idlling == C_ORDER_CARSTATUS_ON));
    $("#" + aTargetId).toggleClass(aCssClassNameOff, (_idlling != C_ORDER_CARSTATUS_ON));
}


/**
* ご用命事項・問診項目の走行時情報を表示する.
* @return {void}
*/
function setOrderTravelingInfo() {

    //走行時の情報取得.
    var _traveling = $("#HiddenField07_Traveling").val();

    //走行時「穏速」の設定.
    $("#TC07_Traveling_Lowspeed").toggleClass("S-TC-07RightListChecked", (_traveling == C_ORDER_TRAVELING_LOWSPEED));

    //走行時「加速」の設定.
    $("#TC07_Traveling_Acceleration").toggleClass("S-TC-07RightListChecked", (_traveling == C_ORDER_TRAVELING_ACCELERATION));

    //走行時「減速」の設定.
    $("#TC07_Traveling_Slowdown").toggleClass("S-TC-07RightListChecked", (_traveling == C_ORDER_TRAVELING_SLOWDOWN));
}


///**
//* ご用命事項・問診項目の車両状態（操作状況1）を表示する.
//* @return {void}
//*/
//function setOrderCarControl1Info() {

//    //車両状態（操作状況1）の情報取得.
//    var _control = $("#HiddenField07_CarControl1").val();

//    //「駐車時」の設定.
//    $("#TC07_CarControl1_Parking").toggleClass("S-TC-07Right01-1", (_control == C_ORDER_CARCONTROL_PARKING));
//    $("#TC07_CarControl1_Parking").toggleClass("S-TC-07Right01-1Off", (_control != C_ORDER_CARCONTROL_PARKING));

//    //「前進時」の設定.
//    $("#TC07_CarControl1_Advance").toggleClass("S-TC-07Right01-2", (_control == C_ORDER_CARCONTROL_ADVANCE));
//    $("#TC07_CarControl1_Advance").toggleClass("S-TC-07Right01-2Off", (_control != C_ORDER_CARCONTROL_ADVANCE));

//    //「変速時」の設定.
//    $("#TC07_CarControl1_ShiftChange").toggleClass("S-TC-07Right01-3", (_control == C_ORDER_CARCONTROL_SHIFTCHANGE));
//    $("#TC07_CarControl1_ShiftChange").toggleClass("S-TC-07Right01-3Off", (_control != C_ORDER_CARCONTROL_SHIFTCHANGE));
//}


///**
//* ご用命事項・問診項目の車両状態（捜査状況2）を表示する.
//* @return {void}
//*/
//function setOrderCarControl2Info() {

//    //車両状態（捜査状況2）の情報取得.
//    var _control = $("#HiddenField07_CarControl2").val();

//    //「後退時」の設定.
//    $("#TC07_CarControl2_Back").toggleClass("S-TC-07Right01-1", (_control == C_ORDER_CARCONTROL_BACK));
//    $("#TC07_CarControl2_Back").toggleClass("S-TC-07Right01-1Off", (_control != C_ORDER_CARCONTROL_BACK));

//    //「ブレーキ時」の設定.
//    $("#TC07_CarControl2_Brake").toggleClass("S-TC-07Right01-2", (_control == C_ORDER_CARCONTROL_BRAKE));
//    $("#TC07_CarControl2_Brake").toggleClass("S-TC-07Right01-2Off", (_control != C_ORDER_CARCONTROL_BRAKE));

//    //「迂回時」の設定.
//    $("#TC07_CarControl2_Detour").toggleClass("S-TC-07Right01-3", (_control == C_ORDER_CARCONTROL_DETOUR));
//    $("#TC07_CarControl2_Detour").toggleClass("S-TC-07Right01-3Off", (_control != C_ORDER_CARCONTROL_DETOUR));
//}


/**
* ご用命事項・問診項目の非純正用品を表示する.
* @return {void}
*/
function setOrderNonGenuineInfo() {

    //非純正用品情報の取得.
    var _genuine = $("#HiddenField07_NonGenuine").val();

    //非純正用品「あり」の設定.
    $("#TC07_NonGenuine_Yes").toggleClass("S-TC-07Right02-1", (_genuine == C_ORDER_NONGENUINE_YES));
    $("#TC07_NonGenuine_Yes").toggleClass("S-TC-07Right02-1Off", (_genuine != C_ORDER_NONGENUINE_YES));

    //非純正用品「なし」の設定.
    $("#TC07_NonGenuine_No").toggleClass("S-TC-07Right02-2", (_genuine == C_ORDER_NONGENUINE_NO));
    $("#TC07_NonGenuine_No").toggleClass("S-TC-07Right02-2Off", (_genuine != C_ORDER_NONGENUINE_NO));
}