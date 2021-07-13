//------------------------------------------------------------------------------
//SC3220101.js
//------------------------------------------------------------------------------
//機能：メインメニュー（SM）_javascript
//作成：2012/05/16 TMEJ 日比野
//更新：2012/09/25 TMEJ 日比野 【SERVICE_2】受付待ち工程の追加対応
//更新：2012/09/21 TMEJ 小澤 【SERVICE_2】SAストール予約受付機能開発（仕分けNo.35）
//更新：2012/11/13 TMEJ 丁 【A.STEP2】次世代サービス アクティブインジゲータ対応 No.1
//更新：2013/03/12 TMEJ 岩城 【A.STEP1】来店受付管理オペレーション確立に向けた評価アプリ作成
//更新：2013/06/03 TMEJ 小澤 【A.STEP2】次世代e-CRBサービスタブレット 新DB適応に向けた機能開発
//更新：2013/12/10 TMEJ 河原 次世代サービス 工程管理機能開発
//更新：2014/07/18 TMEJ 小澤 UAT不具合対応 プロヴィンスの初期化処理追加
//更新：2017/10/13 NSK 竹中(悠) REQ-SVT-TMT-20170227-002 見込納車遅れ日時計算ロジック統一
//更新：2018/06/22 NSK 可児 TKM Next Gen e-CRB Project Application development Block B-1  Pマーク、Lマークなどを表示
//更新：
//------------------------------------------------------------------------------

// SAのオンラインステータス:オンライン
var C_SA_ONLINESTATE_ONLINE = "1";

// SAのオンラインステータス:退席中
var C_SA_ONLINESTATE_AWAY = "2";

// SAのオンラインステータス:オフライン
var C_SA_ONLINESTATE_OFFLINE = "3";

// チップの工程ステータス:受付中
var C_CHIP_PROGRESSSTATE_RECEPTION = 1;

// チップの工程ステータス:追加作業
var C_CHIP_PROGRESSSTATE_ADDITION_WORKING = 2;

// チップの工程ステータス:洗車・納車準備
var C_CHIP_PROGRESSSTATE_PREPARATION_DELIVERY = 3;

// チップの工程ステータス:納車作業
var C_CHIP_PROGRESSSTATE_DELIVERY = 4;

// チップの工程ステータス:作業中
var C_CHIP_PROGRESSSTATE_WORKING = 5;

// チップの工程ステータス:来店(受付待ち)
var C_CHIP_PROGRESS_STATE_RECEPTION_WAIT = 6

// チップの洗車有無:洗車なし
var C_CHIP_WASHING_NONE = "0"

// チップの洗車有無:洗車未完了
var C_CHIP_WASHING_IMPERFECT = "1"

// チップの洗車有無:洗車完了
var C_CHIP_WASHING_FINISH = "2"

// ページ取得時のサーバとクライアントの時間差
var gServerTimeDifference = 0;

// 表示しているチップリスト
var gChipList = new Array();

//チップ詳細スクロールフラグ
var gDetailPopUpSlideFLG = false;

var C_CHIP_COLOR_RED = "Red";
var C_CHIP_COLOR_BLUE = "Blue";
var C_CHIP_COLOR_ORANGE = "Orange";

// 遅れチップ有無：来店
var gReceptionWaitRedFLG = false;
// 遅れチップ有無：受付中
var gReceptionChipRedFLG = false;
// 遅れチップ有無：作業中
var gWorkingChipRedFLG = false;
// 遅れチップ有無：追加作業
var gAddWorkingChipRedFLG = false;
// 遅れチップ有無：納車準備
var gPreparationDeliveryChipRedFLG = false;
// 遅れチップ有無：納車作業
var gDeliveryChipRedFLG = false;

//更新：2018/06/22 NSK 可児 TKM Next Gen e-CRB Project Application development Block B-1  Pマーク、Lマークなどを表示 START
// アイコンの表示フラグ
var C_ICON_FLAG_1 = "1";
// アイコンの表示フラグ
var C_ICON_FLAG_2 = "2";
//更新：2018/06/22 NSK 可児 TKM Next Gen e-CRB Project Application development Block B-1  Pマーク、Lマークなどを表示 END

/**
* DOMロード直後の処理(重要事項).
* @return {void}
*/
$(function () {

    //サーバから非同期でデータを取得
    getServerData();

    //初期読み込み中表示
    setLoadingStart();
    // 更新：2012/11/13 TMEJ 丁 【A.STEP2】次世代サービス アクティブインジゲータ対応 No.1 START
    setTimeout(commonRefreshTimer(
                        function () {
                            //リロード処理
                            location.replace(location.href);
                        }
                    ), 0);
    // 更新：2012/11/13 TMEJ 丁 【A.STEP2】次世代サービス アクティブインジゲータ対応 No.1 END
    //フッターアプリの起動設定
    SetFutterApplication();

    var position = $("#headerSet01").position();

    $("#InsetHeaderShadowDiv").css({
        top: position.top + "px",
        left: position.left + "px"
    });

    //2013/12/10 TMEJ 河原 次世代サービス 工程管理機能開発 START

    //$("#scrollDiv").mainMenuFingerScroll({ scrollMode: "all" });

    //2013/12/10 TMEJ 河原 次世代サービス 工程管理機能開発 END

    //1分ごとにアイコンの色を確認・変更
    setInterval("checkChipColor()", (60 * 1000));

    $('.Coordinate').popoverEx({
        id: 'Service',
        header: $("#PopoverTitleBoxDiv"),
        content: $('#PopoverDataBoxDiv'),
        preventTop: true,
        preventBottom: true,
        live: true,
        openEvent: function (button, settings) {
            onOpenPopover(button, settings);
        }
    });

    //フッター「顧客詳細ボタン」クリック時の動作
    $('#MstPG_FootItem_Main_700').bind("click", function (event) {

        $('#MstPG_CustomerSearchTextBox').focus();

        //$.stopPropagation();
        //event.stopPropagation();
        event.stopPropagation();
    });

    //2013/12/10 TMEJ 河原 次世代サービス 工程管理機能開発 START

    //$("#scrollDiv2").mainMenuFingerScroll();

    //2013/12/10 TMEJ 河原 次世代サービス 工程管理機能開発 END

    Sys.WebForms.PageRequestManager.getInstance().add_endRequest(onEndRequestHandler);
});

/**
* 非同期通信でサーバに接続.
* @return {void}
*/
function getServerData() {

    $.ajax({
        type: "POST",
        url: "SC3220101.aspx",
        data: "method=GetDataAjax",
        async: true,
        success: function (html) {

            //クライアントで取得できる時間とサーバ取得時間との差を設定する.
            SetServerTimeDifference($("#HiddenServerTime", html).val());

            //サーバーから取得したデータを設定する.
            OnSuccessed($("#HiddenChipData", html).val());

            endRefresh();
            setLoadingEnd();

            //2013/12/10 TMEJ 河原 次世代サービス 工程管理機能開発 START

            $("#scrollDiv").mainMenuFingerScroll({ scrollMode: "all" });

            $("#scrollDiv2").mainMenuFingerScroll();

            //2013/12/10 TMEJ 河原 次世代サービス 工程管理機能開発 END

        }
    });
}

/**
* 非同期通信でのサーバ接続に成功した場合の処理.
* @return {void}
*/
function OnSuccessed(str) {
    // JSON形式のデータを変換
    var dataList = $.parseJSON(str);

    //更新：2018/06/22 NSK 可児 TKM Next Gen e-CRB Project Application development Block B-1  Pマーク、Lマークなどを表示 START
    var iconWords = $("#HiddenIconWord").val();
    var iconWord = $.parseJSON(iconWords);
    //更新：2018/06/22 NSK 可児 TKM Next Gen e-CRB Project Application development Block B-1  Pマーク、Lマークなどを表示 END

    var leftDataRowDiv = $("#LeftDataRowDiv");
    var rightDataRowDiv = $("#RightDataRowDiv");
    var visitChipArea = $("#ssvm01VisitTable div.DatasSet ");

    leftDataRowDiv.empty();
    rightDataRowDiv.empty();
    visitChipArea.empty();

    gChipList = new Array();

    //工程ごとのチップ数
    var receptionWaitChipCount = 0;
    var receptionChipCount = 0;
    var workingChipCount = 0;
    var addWorkingChipCount = 0;
    var preparationDeliveryChipCount = 0;
    var deliveryChipCount = 0;

    for (var i = 0; i < dataList.length; i++) {

        var data = dataList[i];

        if (data.Visit == "1") {
            //来店(受付待ち)の場合
            var InsBox6Div = $("<div />").addClass("InsBoxType6");

            // アイコン情報を保持
            gChipList = gChipList.concat(data.ChipList);

            // アイコンの設定
            for (var chipKey in data.ChipList) {

                var chip = data.ChipList[chipKey];
                receptionWaitChipCount++;

                //来店実績連番
                var chipDivId = chip.Id;

                var chipColor = getChipColor(chip);

                var chipDiv = $("<div id='" + chipDivId + "' />").addClass("Coordinate")
                                                                 .addClass(getChipColorClassName(chip, chipColor));

                //2018/06/22 NSK 可児 TKM Next Gen e-CRB Project Application development Block B-1  Pマーク、Lマークなどを表示 START
                //M/B/E/T/P/Lアイコン
                var iconDiv = SetIcon(chip, iconWord);

                chipDiv.append(iconDiv)
                //2018/06/22 NSK 可児 TKM Next Gen e-CRB Project Application development Block B-1  Pマーク、Lマークなどを表示 END               

                // 車両登録No
                chipDiv.append($("<span>").text(chip.VehiclesRegNo));

                InsBox6Div.append(chipDiv);
            }

            visitChipArea.append(InsBox6Div);
            continue;
        }

        var leftDataBoxDiv = $("<div />").addClass("LeftDataDiv");
        var SANameDiv = $("<div />");

        //オンライン状態
        switch (data.Stats) {
            case C_SA_ONLINESTATE_ONLINE:
                SANameDiv.addClass("SANameSet1");
                break;
            case C_SA_ONLINESTATE_AWAY:
                SANameDiv.addClass("SANameSet2");
                break;
            case C_SA_ONLINESTATE_OFFLINE:
                SANameDiv.addClass("SANameSet3");
                break;
        }

        // SA名を設定
        var SAName = $("<span class='SANameSpan'>").text(data.Name)
                                                   .css({ display: "inline-block", width: "115px" });

        SANameDiv.append(SAName);

        leftDataBoxDiv.append(SANameDiv);
        leftDataRowDiv.append(leftDataBoxDiv);

        var ChipRowDiv = $("<div />").addClass("RightRowDataBox");

        // 行を縞々に設定
        if (i % 2 == 0) {
            ChipRowDiv.addClass("BgSetWhiteG");
        } else {
            ChipRowDiv.addClass("BgSetGrayG");
        }

        var InsBox1Div = $("<div />").addClass("InsBoxType1");
        var InsBox2Div = $("<div />").addClass("InsBoxType2");
        var InsBox3Div = $("<div />").addClass("InsBoxType3");
        var InsBox4Div = $("<div />").addClass("InsBoxType4");
        var InsBox5Div = $("<div />").addClass("InsBoxType5");

        // アイコン情報を保持
        gChipList = gChipList.concat(data.ChipList);

        // アイコンの設定
        for (var chipKey in data.ChipList) {
            var chip = data.ChipList[chipKey];

            var chipDivId = "";

            //追加作業の場合、IDに「+」を追加
            if (chip.Stats == C_CHIP_PROGRESSSTATE_ADDITION_WORKING) {
                chipDivId = chip.Id + "+";
            } else {
                chipDivId = chip.Id;
            }

            var chipColor = getChipColor(chip);
            var chipDiv = $("<div id='" + chipDivId + "' />").addClass("Coordinate")
                                                             .addClass(getChipColorClassName(chip, chipColor));

            //2018/06/22 NSK 可児 TKM Next Gen e-CRB Project Application development Block B-1  Pマーク、Lマークなどを表示 START
            //M/B/E/T/P/Lアイコン
            var iconDiv = SetIcon(chip, iconWord);
            chipDiv.append(iconDiv)
            //2018/06/22 NSK 可児 TKM Next Gen e-CRB Project Application development Block B-1  Pマーク、Lマークなどを表示 END

            // 車両登録No
            chipDiv.append($("<span>").text(chip.VehiclesRegNo));

            //工程ごとのDivに配置
            switch (chip.Stats) {
                case C_CHIP_PROGRESSSTATE_RECEPTION:
                    receptionChipCount++;
                    InsBox1Div.append(chipDiv);
                    break;
                case C_CHIP_PROGRESSSTATE_WORKING:
                    workingChipCount++;
                    InsBox2Div.append(chipDiv);
                    break;
                case C_CHIP_PROGRESSSTATE_ADDITION_WORKING:
                    addWorkingChipCount++;
                    InsBox3Div.append(chipDiv);
                    break;
                case C_CHIP_PROGRESSSTATE_PREPARATION_DELIVERY:
                    preparationDeliveryChipCount++;
                    InsBox4Div.append(chipDiv);
                    break;
                case C_CHIP_PROGRESSSTATE_DELIVERY:
                    deliveryChipCount++;
                    InsBox5Div.append(chipDiv);
                    break;
            }
        }

        ChipRowDiv.append(InsBox1Div)
                  .append(InsBox2Div)
                  .append(InsBox3Div)
                  .append(InsBox4Div)
                  .append(InsBox5Div);

        rightDataRowDiv.append(ChipRowDiv);

        // 工程毎のアイコン数の最大値
        var maxChipCount = Math.max.apply(null, [$(InsBox1Div).children().size(),
                                                 $(InsBox2Div).children().size(),
                                                 $(InsBox3Div).children().size(),
                                                 $(InsBox4Div).children().size(),
                                                 $(InsBox5Div).children().size()]);

        // 最大アイコン数が3以上の場合
        //2018/06/22 NSK 可児 TKM Next Gen e-CRB Project Application development Block B-1  Pマーク、Lマークなどを表示 START
        //if (maxChipCount > 3) {
        // 最大アイコン数が2以上の場合
        if (maxChipCount > 2) {
            //2018/06/22 NSK 可児 TKM Next Gen e-CRB Project Application development Block B-1  Pマーク、Lマークなどを表示 END
            // SA名前列に空白Divを設置
            var leftDataBlankLine = $("<div />").addClass("LeftDataBlankLine");

            var rowHeight = 0;
            //2018/06/22 NSK 可児 TKM Next Gen e-CRB Project Application development Block B-1  Pマーク、Lマークなどを表示 START
            //rowHeight = Math.ceil(maxChipCount / 3) * 54 - 5;
            rowHeight = Math.ceil(maxChipCount / 2) * 54;
            //2018/06/22 NSK 可児 TKM Next Gen e-CRB Project Application development Block B-1  Pマーク、Lマークなどを表示 END


            ChipRowDiv.css("height", rowHeight + "px");
            leftDataBoxDiv.css("height", rowHeight + "px");

            leftDataBlankLine.css("height", rowHeight - 54 + "px");
            leftDataBoxDiv.append(leftDataBlankLine);
        }
    }

    //工程ごとのチップ数を表示
    $("#InnerText1CounterLabel").text(receptionChipCount);
    $("#InnerText2CounterLabel").text(workingChipCount);
    $("#InnerText3CounterLabel").text(addWorkingChipCount);
    $("#InnerText4CounterLabel").text(preparationDeliveryChipCount);
    $("#InnerText5CounterLabel").text(deliveryChipCount);
    $("#InnerText6CounterLabel").text(receptionWaitChipCount);


    rightDataRowDiv.append($("<div />").attr("id", "InsetRightShadowDiv"));

    // 行数が少なくてもスクロールできるように設定
    if (leftDataRowDiv.outerHeight() > 550) {
        $("#ssvm01MaineTable .DatasSet").css("height", leftDataRowDiv.outerHeight() + "px");
    } else {
        $("#ssvm01MaineTable .DatasSet").css("height", "550px");
    }
    $(".SANameSpan").CustomLabelEx({ useEllipsis: true });

    //チップカウンターの色を設定
    changeChipCounterColor();

    //更新時間を設定する
    $("#MessageUpdateTime").text(getUpdateTime());
}

/**
* 詳細ポップアップを開いたときの処理
* @return {void}
*/
function onOpenPopover(button, settings) {
    //詳細ポップアップの初期化
    initDetailsPopover();

    // 読み込み中アイコン表示
    $('#loadingroInfomation').attr("style", "visibility: visible");

    var parentDiv = button.parents("[class^='InsBoxType']");

    // 詳細ポップアップの表示方向を設定
    if ($(parentDiv).hasClass("InsBoxType1")) {
        //受付
        $("#HiddenSelectedDisplayArea").val(C_CHIP_PROGRESSSTATE_RECEPTION);
        settings.preventLeft = true;
        settings.preventRight = false;
    } else if ($(parentDiv).hasClass("InsBoxType2")) {
        //作業中
        $("#HiddenSelectedDisplayArea").val(C_CHIP_PROGRESSSTATE_WORKING);
        settings.preventLeft = true;
        settings.preventRight = false;
    } else if ($(parentDiv).hasClass("InsBoxType3")) {
        //追加作業
        $("#HiddenSelectedDisplayArea").val(C_CHIP_PROGRESSSTATE_ADDITION_WORKING);
        settings.preventLeft = false;
        settings.preventRight = true;
    } else if ($(parentDiv).hasClass("InsBoxType4")) {
        //納車準備
        $("#HiddenSelectedDisplayArea").val(C_CHIP_PROGRESSSTATE_PREPARATION_DELIVERY);
        settings.preventLeft = false;
        settings.preventRight = true;
    } else if ($(parentDiv).hasClass("InsBoxType5")) {
        //納車作業
        $("#HiddenSelectedDisplayArea").val(C_CHIP_PROGRESSSTATE_DELIVERY);
        settings.preventLeft = false;
        settings.preventRight = true;
    } else if ($(parentDiv).hasClass("InsBoxType6")) {
        //来店
        $("#HiddenSelectedDisplayArea").val(C_CHIP_PROGRESS_STATE_RECEPTION_WAIT);
        settings.preventLeft = true;
        settings.preventRight = false;
    }

    // 選択された来店実績連番を設定
    var buttonId = button.attr("id");

    if (buttonId.slice(-1) == "+") {
        buttonId = buttonId.substring(0, buttonId.length - 1)
    }

    $("#HiddenSelectedVisitSeq").val(buttonId);

    // 詳細ポップアップ内の更新
    // UpdatePanel内のボタンをクリック
    $("#HiddenButtonDetailPopup").click();
}

/**
* 詳細ポップアップの初期化
* @return {void}
*/
function initDetailsPopover() {
    $("#AiconStatsLabel").empty();
    $("#DeliveryTimeLabel").empty();
    $("#ChangeCountLabel").empty();
    $("#DeliveryEstimateLabel").empty();
    $("#HeadInfomationPullDiv").empty();
    $("#VclregNoLabel").text("");
    //2014/07/18 TMEJ 小澤 UAT不具合対応 プロヴィンスの初期化処理追加 START
    $("#DetailsProvince").text("");
    //2014/07/18 TMEJ 小澤 UAT不具合対応 プロヴィンスの初期化処理追加 END
    $("#CarModelLabel").text("");
    $("#CarGradeLabel").text("");
    $("#CustomerNameLabel").text("");
    $("#TelNoLable").text("");
    $("#PortableTelNoLable").text("");
    $("#ServiceContentsLable").text("");
    $("#WaitPlanLabel").text("");
    $("#InterruptionCauseDiv").empty();
    $("#ChangeTimeRepeaterDiv").empty();
    //2012/09/21 TMEJ 小澤 【SERVICE_2】SAストール予約受付機能開発（仕分けNo.35）START
    $("#DrawerLabel").text("");
    //2012/09/21 TMEJ 小澤 【SERVICE_2】SAストール予約受付機能開発（仕分けNo.35）END

    $("#DetailsRightIconD").css("display", "none");
    //2018/06/22 NSK 可児 TKM Next Gen e-CRB Project Application development Block B-1  Pマーク、Lマークなどを表示 START
    //$("#DetailsRightIconI").css("display", "none");
    $("#DetailsRightIconP").css("display", "none");
    $("#DetailsRightIconL").css("display", "none");
    //2018/06/22 NSK 可児 TKM Next Gen e-CRB Project Application development Block B-1  Pマーク、Lマークなどを表示 END
    $("#DetailsRightIconS").css("display", "none");
    $("#DetailsRightIconV").css("display", "none");

    $("#FixDeliveryTimeLabel").css("display", "none");
    $("#FixSlashLabel").css("display", "none");
    $("#FixDownArrow").css("display", "none");
    $("#FixDeliveryEstimateLabel").css("display", "none");
    //2012/09/21 TMEJ 小澤 【SERVICE_2】SAストール予約受付機能開発（仕分けNo.35）START
    $("#DrawerTable").css("display", "none");
    //2012/09/21 TMEJ 小澤 【SERVICE_2】SAストール予約受付機能開発（仕分けNo.35）END

    $("#FooterButton01").unbind("click");
    $("#FooterButton01").removeClass("FooterButton01_on").addClass("FooterButton01_off");
    $("#FooterButton02").unbind("click");
    $("#FooterButton02").removeClass("FooterButton02_on").addClass("FooterButton02_off");
}

/**
* アイコン色の確認・更新
* @return {void}
*/
function checkChipColor() {

    for (var chipKey in gChipList) {

        var chip = gChipList[chipKey];

        var chipColor = getChipColor(chip);
        var className = getChipColorClassName(chip, chipColor);

        var chipDivId = "";

        //追加作業の場合、IDに「+」を追加
        if (chip.Stats == C_CHIP_PROGRESSSTATE_ADDITION_WORKING) {
            chipDivId = "[id='" + chip.Id + "+']";
        } else {
            chipDivId = "#" + chip.Id;
        }

        if ($(chipDivId).hasClass(className) == false) {
            $(chipDivId).removeClass();
            $(chipDivId).addClass("Coordinate");
            $(chipDivId).addClass(className);
        }

    }

    //チップカウンターの色を設定
    changeChipCounterColor();
}

/**
* チップの色判定
* @return {チップの色}
*/
function getChipColor(chip) {

    var chipColor = "";

    var serverTime = getServerTimeNow();

    if (chip.Stats == C_CHIP_PROGRESSSTATE_RECEPTION || chip.Stats == C_CHIP_PROGRESS_STATE_RECEPTION_WAIT) {
        //来店・受付中の場合
        var deliveryEstimateTime = new Date(chip.DelayTime);

        if (deliveryEstimateTime < serverTime) {
            //納車見込時間より遅れている場合
            chipColor = C_CHIP_COLOR_RED;
        } else {
            //予定通りの場合
            chipColor = C_CHIP_COLOR_BLUE;
        }
    } else {
        //作業中～納車作業
        var deliveryPlanTime = new Date(chip.DeliTime);
        var deliveryEstimateTime = new Date(chip.DelayTime);
        // 2017/10/13 NSK 竹中(悠) REQ-SVT-TMT-20170227-002 見込納車遅れ日時計算ロジック統一 START
        var minValue = new Date("0001/01/01 00:00");
        if ((deliveryPlanTime - minValue) == 0) {
            // 予定納車日時が日付の最小値の場合、遅れ管理しない
            // 常に青色アイコン
            return chipColor = C_CHIP_COLOR_BLUE;
        }
        // 2017/10/13 NSK 竹中(悠) REQ-SVT-TMT-20170227-002 見込納車遅れ日時計算ロジック統一 START
        if (deliveryPlanTime < serverTime) {
            //納車予定時刻より遅れている場合
            chipColor = C_CHIP_COLOR_RED;
        } else if (deliveryEstimateTime < serverTime) {
            //納車見込時間より遅れている場合
            chipColor = C_CHIP_COLOR_ORANGE;
        } else {
            //予定通りの場合
            chipColor = C_CHIP_COLOR_BLUE;
        }
    }

    return chipColor;
}

/**
* チップの色判定
* @return {チップDivのCSSクラス名}
*/
function getChipColorClassName(chip, chipColor) {

    var chipColorClassName = "";

    //工程ごとのDivに配置
    switch (chip.Stats) {
        case C_CHIP_PROGRESSSTATE_RECEPTION:
            //受付
            chipColorClassName = getCarTypeClassName(chipColor);
            if (chipColor == C_CHIP_COLOR_RED) {
                gReceptionChipRedFLG = true;
            }
            break;

        case C_CHIP_PROGRESSSTATE_WORKING:
            //作業中
            chipColorClassName = getCarTypeClassName(chipColor);
            if (chipColor == C_CHIP_COLOR_RED || chipColor == C_CHIP_COLOR_ORANGE) {
                gWorkingChipRedFLG = true;
            }
            break;

        case C_CHIP_PROGRESSSTATE_ADDITION_WORKING:
            //追加作業
            chipColorClassName = getAddworkTypeClassName(chipColor);
            if (chipColor == C_CHIP_COLOR_RED || chipColor == C_CHIP_COLOR_ORANGE) {
                gAddWorkingChipRedFLG = true;
            }
            break;

        case C_CHIP_PROGRESSSTATE_PREPARATION_DELIVERY:
            //納車準備
            chipColorClassName = getPreparationDeliveryTypeClassName(chipColor, chip.Wash);
            if (chipColor == C_CHIP_COLOR_RED || chipColor == C_CHIP_COLOR_ORANGE) {
                gPreparationDeliveryChipRedFLG = true;
            }
            break;

        case C_CHIP_PROGRESSSTATE_DELIVERY:
            //納車作業
            chipColorClassName = getCarTypeClassName(chipColor);
            if (chipColor == C_CHIP_COLOR_RED || chipColor == C_CHIP_COLOR_ORANGE) {
                gDeliveryChipRedFLG = true;
            }
            break;

        case C_CHIP_PROGRESS_STATE_RECEPTION_WAIT:
            //来店(受付待ち)
            chipColorClassName = getCarTypeClassName(chipColor);
            if (chipColor == C_CHIP_COLOR_RED) {
                gReceptionWaitRedFLG = true;
            }
            break;
    }

    return chipColorClassName;
}

/**
* 来店・受付・作業中・納車作業のチップのクラスを取得
*
* @param {String} チップ色
* @return {チップDivのCSSクラス名}
*/
function getCarTypeClassName(chipColor) {

    if (chipColor == C_CHIP_COLOR_RED) {
        //納車予定時刻より遅れている場合
        return "IconTypeCarRed01";
    } else if (chipColor == C_CHIP_COLOR_ORANGE) {
        //納車見込時間より遅れている場合
        return "IconTypeCarOrange01";
    } else {
        //予定通りの場合
        return "IconTypeCarBlue01";
    }
}

/**
* 追加作業のチップの色判定
*
* @param {String} チップ色
* @return {チップDivのクラス名}
*/
function getAddworkTypeClassName(chipColor) {

    if (chipColor == C_CHIP_COLOR_RED) {
        return "IconTypeMainteRed01";
    } else if (chipColor == C_CHIP_COLOR_ORANGE) {
        return "IconTypeMainteOrange01";
    } else {
        return "IconTypeMainteBlue01";
    }
}

/**
* 納車準備工程のチップの色判定
*
* @param {String} チップ色
* @return {チップDivのクラス名}
*/
function getPreparationDeliveryTypeClassName(chipColor, Wash) {

    if (Wash == C_CHIP_WASHING_IMPERFECT) {
        // 洗車未完了の場合
        if (chipColor == C_CHIP_COLOR_RED) {
            return "IconTypeDeliveredRed01";
        } else if (chipColor == C_CHIP_COLOR_ORANGE) {
            return "IconTypeDeliveredOrange01";
        } else {
            return "IconTypeDeliveredBlue01";
        }
    } else {
        // 洗車なし・完了の場合
        if (chipColor == C_CHIP_COLOR_RED) {
            return "IconTypeDeliveredRed03";
        } else if (chipColor == C_CHIP_COLOR_ORANGE) {
            return "IconTypeDeliveredOrange03";
        } else {
            //予定通りの場合
            return "IconTypeDeliveredBlue03";
        }
    }
}

/**
* チップカウンターの色を設定
*/
function changeChipCounterColor() {

    if (gReceptionChipRedFLG) {
        $("#InnerText1CounterLabel").css("color", "#FF0000");
    } else {
        $("#InnerText1CounterLabel").css("color", "#FFFFFF");
    }
    if (gWorkingChipRedFLG) {
        $("#InnerText2CounterLabel").css("color", "#FF0000");
    } else {
        $("#InnerText2CounterLabel").css("color", "#FFFFFF");
    }
    if (gAddWorkingChipRedFLG) {
        $("#InnerText3CounterLabel").css("color", "#FF0000");
    } else {
        $("#InnerText3CounterLabel").css("color", "#FFFFFF");
    }
    if (gPreparationDeliveryChipRedFLG) {
        $("#InnerText4CounterLabel").css("color", "#FF0000");
    } else {
        $("#InnerText4CounterLabel").css("color", "#FFFFFF");
    }
    if (gDeliveryChipRedFLG) {
        $("#InnerText5CounterLabel").css("color", "#FF0000");
    } else {
        $("#InnerText5CounterLabel").css("color", "#FFFFFF");
    }
    if (gReceptionWaitRedFLG) {
        $("#InnerText6CounterLabel").css("color", "#FF0000");
    } else {
        $("#InnerText6CounterLabel").css("color", "#FFFFFF");
    }

    gReceptionChipRedFLG = false;
    gWorkingChipRedFLG = false;
    gAddWorkingChipRedFLG = false;
    gPreparationDeliveryChipRedFLG = false;
    gDeliveryChipRedFLG = false;
    gReceptionWaitRedFLG = false;
}

/**
* 詳細ポップアップ表示後の設定
*
* @param {} 
* @param {} 
* @return {void}
*/
function onEndRequestHandler(sender, args) {

    $("#PopoverScrollDiv").fingerScroll();
    $("#HeadInfomationPullDiv").css("display", "none");

    // 納車時刻の変更回数が0以外の場合
    if ($("#HiddenDeliveryPlanUpdateCount").val() != "0") {

        // ステータスエリアをクリックで変更履歴を表示・非表示
        $("#StatsInfoInnaerDataBoxDiv").click(function () {

            if (gDetailPopUpSlideFLG) {
                gDetailPopUpSlideFLG = false;
                $("#PopoverScrollDiv .scroll-inner").css({
                    '-webkit-transform': 'translate3d(0px, 0px, 0px)',
                    '-webkit-transition': '-webkit-transform 400ms'
                });
            } else {
                gDetailPopUpSlideFLG = true;
            }
            $("#HeadInfomationPullDiv").slideToggle();
        });
    } else {
        $("#FixDownArrow").css("display", "none");
    }

    // 顧客情報サブボタンの活性・非活性を設定
    if ($("#HiddenDetailsCustomerButtonStatus").val() == "0") {
        //非活性
        $("#FooterButton01").unbind("click");
        $("#FooterButton01").removeClass("FooterButton01_on").addClass("FooterButton01_off");
    } else {
        //活性
        $("#FooterButton01").removeClass("FooterButton01_off").addClass("FooterButton01_on");
        $("#FooterButton01").bind("click", function () {
            $("#HiddenButtonDetailCustomer").click();
        });
    }

    // R/Oサブボタンの活性・非活性を設定
    if ($("#HiddenDetailsROButtonStatus").val() == "0") {
        //非活性
        $("#FooterButton02").unbind("click");
        $("#FooterButton02").removeClass("FooterButton02_on").addClass("FooterButton02_off");
    } else {
        //活性
        $("#FooterButton02").removeClass("FooterButton02_off").addClass("FooterButton02_on");
        $("#FooterButton02").bind("click", function () {
            $("#HiddenButtonDetailRo").click();
        });
    }

    $("#FixDeliveryTimeLabel").css("display", "inline-block");
    $("#FixSlashLabel").css("display", "inline-block");
    $("#FixDeliveryEstimateLabel").css("display", "inline-block");


    openPopovetSetEllipsis();

    //ローディングアイコン非表示
    $("#loadingroInfomation").hide(0);
}

/**
* 詳細ポップアップのToolTipを設定.
* @return {void}
*/
function openPopovetSetEllipsis() {
    $("#FixDeliveryTimeLabel").CustomLabelEx({ useEllipsis: true });
    $("#ChangeCountLabel").CustomLabelEx({ useEllipsis: true });
    $("#FixDeliveryEstimateLabel").CustomLabelEx({ useEllipsis: true });
    $("#FixVclregNoLabel").CustomLabelEx({ useEllipsis: true });
    $("#VclregNoLabel").CustomLabelEx({ useEllipsis: true });
    //2014/07/18 TMEJ 小澤 UAT不具合対応 プロヴィンスの初期化処理追加 START
    $("#DetailsProvince").CustomLabelEx({ useEllipsis: true });
    //2014/07/18 TMEJ 小澤 UAT不具合対応 プロヴィンスの初期化処理追加 END
    $("#FixCarModelLabel").CustomLabelEx({ useEllipsis: true });
    $("#CarModelLabel").CustomLabelEx({ useEllipsis: true });
    $("#CarGradeLabel").CustomLabelEx({ useEllipsis: true });
    $("#FixCustomerNameLabel").CustomLabelEx({ useEllipsis: true });
    $("#CustomerNameLabel").CustomLabelEx({ useEllipsis: true });
    $("#FixTelNoLable").CustomLabelEx({ useEllipsis: true });
    $("#TelNoLable").CustomLabelEx({ useEllipsis: true });
    $("#FixPortableTelNoLable").CustomLabelEx({ useEllipsis: true });
    $("#PortableTelNoLable").CustomLabelEx({ useEllipsis: true });
    $("#FixServiceContentsLable").CustomLabelEx({ useEllipsis: true });
    $("#ServiceContentsLable").CustomLabelEx({ useEllipsis: true });
    $("#FixWaitPlanLabel").CustomLabelEx({ useEllipsis: true });
    $("#WaitPlanLabel").CustomLabelEx({ useEllipsis: true });
    $("#AiconStatsLabel").CustomLabelEx({ useEllipsis: true });
}


/**
* 詳細ポップアップを消す.
* @return {void}
*/
function hideDetailsPopover() {
    var selectedId = $("#HiddenSelectedVisitSeq").val();

    if (selectedId != "") {
        if ($("#" + selectedId).length == 0) {
            var chipDivId = "[id='" + selectedId + "+']";
            $(chipDivId).trigger('hidePopover');
        } else {
            $("#" + selectedId).trigger('hidePopover');
        }
    }
}

/**
* サーバとの時間差を算出し、グローバル変数として格納する.
* @return {String}
*/
function SetServerTimeDifference(strDate) {

    $("#HiddenServerTime").val(strDate);

    //ページ読込時のサーバ時間を取得する.
    var pageLoadServerTime = new Date(strDate);
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
 * 画面の更新時間を返す.
 * @return {Date}
 */
function getUpdateTime() {
    var srverTime = new Date($("#HiddenServerTime").val());

    var mm = srverTime.getMonth() + 1;
    if (mm < 10) { mm = "0" + mm; }

    var dd = srverTime.getDate();
    if (dd < 10) { dd = "0" + dd; }

    var hh = srverTime.getHours();
    if (hh < 10) { hh = "0" + hh; }

    var MM = srverTime.getMinutes();
    if (MM < 10) { MM = "0" + MM; }

    return mm + "/" + dd + " " + hh + ":" + MM;
}

/**
 * 初期読み込み中画面を表示する.
 * @return {Date}
 */
function setLoadingStart() {
    $("#serverProcessOverlayBlack").css("display", "block");
    $("#serverProcessIcon").css("display", "block");
}

/**
 * 初期読み込み中画面を非表示にする.
 * @return {Date}
 */
function setLoadingEnd() {
    $("#serverProcessOverlayBlack").css("display", "none");
    $("#serverProcessIcon").css("display", "none");

    //MainMenuFingerscrollのクリックイベント抑制解除
    $("#scrollDiv").click();
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
// 更新：2012/11/13 TMEJ 丁 【A.STEP2】次世代サービス アクティブインジゲータ対応 No.1 START
function FooterButtonclick(Id) {

    //2013/12/10 TMEJ 河原 次世代サービス 工程管理機能開発 START
    //顧客詳細ボタンなら処理無し
    if (Id == 700) {

        return false;
    };
    //2013/12/10 TMEJ 河原 次世代サービス 工程管理機能開発 START

    if (Id == 1200 || Id == 1300) {
        $("#serverProcessOverlayBlack").css("z-index", "10000");
        $("#serverProcessIcon").css("z-index", "10000");
    }
    setLoadingStart();
    setTimeout(commonRefreshTimer(
                        function () {
                            //リロード処理
                            location.replace(location.href);
                        }
                    ), 0);

    //2013/12/10 TMEJ 河原 次世代サービス 工程管理機能開発 END
    //	switch (Id){
    //	case 600 :	
    //		__doPostBack('ctl00$MstPG_FootItem_Main_600','');
    //		break;
    //	case 100 :
    //		__doPostBack('ctl00$MstPG_FootItem_Main_100','');
    //		break;		
    //	case 1100 :
    //		__doPostBack('ctl00$MstPG_FootItem_Main_1100','');
    //		break;
    //	case 1200 :
    //		$("#HiddenButtonDetailCustomer").removeAttr('OnClick','');
    //		$("#HiddenButtonDetailCustomer").click();
    //		break;
    //    case 1300:
    //        $("#HiddenButtonDetailRo").removeAttr('OnClick', '');
    //        $("#HiddenButtonDetailRo").click();
    //        break;
    //    //2013/06/03 TMEJ 小澤 【A.STEP2】次世代e-CRBサービスタブレット 新DB適応に向けた機能開発 START 
    //    case 800:
    //        __doPostBack('ctl00$MstPG_FootItem_Main_800', '');
    //        break;
    //    //2013/06/03 TMEJ 小澤 【A.STEP2】次世代e-CRBサービスタブレット 新DB適応に向けた機能開発 END 

    //	}
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
        case 1200:
            $("#HiddenButtonDetailCustomer").removeAttr('OnClick', '');
            $("#HiddenButtonDetailCustomer").click();
            break;
        case 1300:
            $("#HiddenButtonDetailRo").removeAttr('OnClick', '');
            $("#HiddenButtonDetailRo").click();
            break;

    };
    //2013/12/10 TMEJ 河原 次世代サービス 工程管理機能開発 END

}
// 更新：2012/11/13 TMEJ 丁 【A.STEP2】次世代サービス アクティブインジゲータ対応 No.1 END

// 2013/03/12 TMEJ 岩城 【A.STEP1】来店受付管理オペレーション確立に向けた評価アプリ作成 START
/**
* フッターボタンタップ時のイベント
*
* @param {buttonID} ボタンID
*/
function FooterEvent(buttonID) {

    // ボタンを青色にする
    $("#FooterButton" + buttonID).addClass("icrop-pressed");

    setTimeout(function () {
        // クルクルを表示
        setLoadingStart();
        // ボタンの青色を解除
        $("#FooterButton" + buttonID).removeClass("icrop-pressed");
    }, 300);

    // タイマーセット
    setTimeout(commonRefreshTimer(
                        function () {
                            //リロード処理
                            location.replace(location.href);
                        }
                    ), 0);

    if (buttonID == 100) {
        //来店管理画面へ遷移
        $("#FooterButtonDummy100").click();
    } else if (buttonID == 200) {
        //全体管理画面へ遷移
        $("#FooterButtonDummy200").click();
    }
    return false;
}
// 2013/03/12 TMEJ 岩城 【A.STEP1】来店受付管理オペレーション確立に向けた評価アプリ作成 END


//2013/12/10 TMEJ 河原 次世代サービス 工程管理機能開発 START

/**
* フッターSAボタンタップ時のイベント
*
* @param {buttonID} ボタンID
*/
function FooterSABtnEvent() {

    // クルクルを表示
    setLoadingStart();

    //再描画
    $("#FooterButtonDummy300").click();

};


//2013/12/10 TMEJ 河原 次世代サービス 工程管理機能開発 END

//2018/06/22 NSK 可児 TKM Next Gen e-CRB Project Application development Block B-1  Pマーク、Lマークなどを表示 START
/**
* M/B/E/T/P/Lアイコンを追加する
*
* @param {chip} チップ情報
* @return {} アイコン表示エリア
*/
function SetIcon(chip, iconWord) {

    var iconDiv = $("<div />").addClass("MarkArea");

    // 各アイコンのdivを設定
    var iconM = $("<div />").addClass("IconM").text(iconWord.WordM);
    var iconB = $("<div />").addClass("IconB").text(iconWord.WordB);
    var iconE = $("<div />").addClass("IconE").text(iconWord.WordE);
    var iconT = $("<div />").addClass("IconT").text(iconWord.WordT);
    var iconP = $("<div />").addClass("IconP").text(iconWord.WordP);
    var iconL = $("<div />").addClass("IconL").text(iconWord.WordL);

    if (chip.SmlAmcFlg == C_ICON_FLAG_1) {
        // Mマーク
        iconDiv.append(iconM);
    }
    else if (chip.SmlAmcFlg == C_ICON_FLAG_2) {
        // Bマーク
        iconDiv.append(iconB);
    }

    if (chip.EwFlg == C_ICON_FLAG_1) {
        // Eマーク
        iconDiv.append(iconE);
    }

    if (chip.TlmMbrFlg == C_ICON_FLAG_1) {
        // Tマーク
        iconDiv.append(iconT);
    }

    if (chip.ImpVclFlg == C_ICON_FLAG_1) {
        // Pマーク
        iconDiv.append(iconP);
    }
    else if (chip.ImpVclFlg == C_ICON_FLAG_2) {
        // Lマーク
        iconDiv.append(iconL);
    }

    return iconDiv;
}
//2018/06/22 NSK 可児 TKM Next Gen e-CRB Project Application development Block B-1  Pマーク、Lマークなどを表示 END