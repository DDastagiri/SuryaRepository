//---------------------------------------------------------
//SC3240101.js
//---------------------------------------------------------
//機能：メイン画面処理
//作成：2012/12/22 TMEJ 張 タブレット版SMB機能開発(工程管理)
//更新：2013/11/29 TMEJ 張 タブレット版SMB チーフテクニシャン機能開発
//更新：2014/06/18 TMEJ 丁 タブレット版SMB テレマ走行距離機能開発
//更新：2014/07/17 TMEJ 張 文言エンコード対応
//更新：2014/09/11 TMEJ 張 BTS-389 「SMBで異常ではないはずが、遅れ見込み表示になっている」対応
//更新：2015/07/30 TMEJ 明瀬 アクティビティインジケータが消えない問題
//更新：2017/01/17 NSK 竹中 TR-SVT-TMT-20151019-002 TCメイン画面のエラーメッセージが理解不可能
//更新：2017/07/12 NSK 河谷 REQ-SVT-TMT-20160906-001 チップ検索の検索範囲をPC-SMBと同じにする
//更新：2017/09/25 NSK 竹中(悠) REQ-SVT-TMT-20170227-002 見込納車遅れ日時計算ロジック統一
//更新：2019/08/07 NSK 鈴木 【TKM】チップ検索からSMBに遷移した時、タイムアウトの文言が表示される
//---------------------------------------------------------

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

//DOMロード直後の処理(重要事項).
//@return {void}
$(function () {

    //クライアントとサーバとの時間の差を設定
    SetServerTimeDifference();

    GetSC3240101WordIni();

    GetSC3240301WordIni();

    $("#MainAreaActiveIndicator").addClass("show"); //グルグルを表示

    //2015/07/30 TMEJ 明瀬 アクティビティインジケータが消えない問題 START

    gArrTimeoutValue = new Array();

    //リフレッシュタイマーセット
    RefreshMainWndTimer(ReDisplay);

    //画面更新中フラグをtrue(更新中)に設定
    gUpdatingDisplayFlg = true;

    //2015/07/30 TMEJ 明瀬 アクティビティインジケータが消えない問題 END

    //グロバール変数の初期化
    gMaxRow = parseInt(document.getElementById("hidMaxRow").value);
    //2013/11/29 TMEJ 張 タブレット版SMB チーフテクニシャン機能開発 START
    if (document.getElementById("hidDateFormatMMdd").value != "") {
        gDateFormatMMdd = document.getElementById("hidDateFormatMMdd").value
    }

    if (document.getElementById("hidDateFormatHHmm").value != "") {
        gDateFormatHHmm = document.getElementById("hidDateFormatHHmm").value
    }

    if (document.getElementById("hidDateFormatYYYYMMddHHmm").value != "") {
        gDateFormatYYYYMMddHHmm = document.getElementById("hidDateFormatYYYYMMddHHmm").value
    }

    gOpeCode = parseInt(document.getElementById("hidOpeCode").value);

    //CT、CHTの場合、SMBボタンがないので、ボタンがずれた。正しい表示ように調整する
    if ((gOpeCode == C_OPECODE_CT) || (gOpeCode == C_OPECODE_CHT)) {
        if ($(".InitFooterButton_Space").length == 2) {
            $(".InitFooterButton_Space").width(90);
        }
    } else {
        if ($(".InitFooterButton_Space").length == 2) {
            $(".InitFooterButton_Space").width(48);
        }
    }

    //スケジュールボタンと電話帳ボタンの設定初期化
    SetFooterApplication();
    //2013/11/29 TMEJ 張 タブレット版SMB チーフテクニシャン機能開発 END

    //営業開始時間、終了時間を設定
    SetStallDate();

    CreateTable();

    InitializationSubChip("");

    //2013/11/29 TMEJ 張 タブレット版SMB チーフテクニシャン機能開発 START
    // 基盤のボタンを初期化する
    HideAplicationButton();
    //z2013/11/29 TMEJ 張 タブレット版SMB チーフテクニシャン機能開発 END

    //2015/07/30 TMEJ 明瀬 アクティビティインジケータが消えない問題 START

    //gArrTimeoutValue = new Array();

    //2015/07/30 TMEJ 明瀬 アクティビティインジケータが消えない問題 END

    var jsonData = $("#hidSessionValue").val();
    if (jsonData != "") {
        var objSelectedChip = $.parseJSON(htmlDecode(jsonData));
        $("#hidSessionValue").val("");
        // チップ表示される日付
        var dtShowDate = new Date(objSelectedChip.DATE);
        dtShowDate.setHours(0);
        dtShowDate.setMinutes(0);
        dtShowDate.setSeconds(0);
        dtShowDate.setMilliseconds(0);
        // 今の日付
        var dtNow = GetServerTimeNow();
        dtNow.setHours(0);
        dtNow.setMinutes(0);
        dtNow.setSeconds(0);
        dtNow.setMilliseconds(0);

        // サブエリア
        if (objSelectedChip.SUB_CHIP_TYPE != 0) {
            //2017/07/12 NSK 河谷 REQ-SVT-TMT-20160906-001 チップ検索の検索範囲をPC-SMBと同じにする START
            //if ((objSelectedChip.SVC_STATUS == C_SVCSTATUS_DROPOFFCUSTOMER) || (objSelectedChip.SVC_STATUS == C_SVCSTATUS_WAITINGCUSTOMER)) {
            //    //チップが納車エリアにいるの場合
            //    gSearchedSubChipAreaId = C_DELIVERDCAR;
            if ((objSelectedChip.RO_STATUS == C_RO_STATUS_STARTWAIT) || (objSelectedChip.RO_STATUS == C_RO_STATUS_WORKING) || (objSelectedChip.TEMP_FLG == C_TEMP_FLAG_ON)) {
                //チップが受付エリアにいるの場合
                gSearchedSubChipAreaId = C_RECEPTION
            } else if (objSelectedChip.RO_STATUS == C_RO_STATUS_WAITING_FM_APPROVAL) {
                //チップが追加作業エリアにいるの場合
                gSearchedSubChipAreaId = C_ADDITIONALWORK
            } else if ((objSelectedChip.SVC_STATUS == C_SVCSTATUS_DROPOFFCUSTOMER) || (objSelectedChip.SVC_STATUS == C_SVCSTATUS_WAITINGCUSTOMER)) {
                //チップが納車エリアにいるの場合
                gSearchedSubChipAreaId = C_DELIVERDCAR;
            //2017/07/12 NSK 河谷 REQ-SVT-TMT-20160906-001 チップ検索の検索範囲をPC-SMBと同じにする END
            } else if ((objSelectedChip.SVC_STATUS == C_SVCSTATUS_CARWASHWAIT) || (objSelectedChip.SVC_STATUS == C_SVCSTATUS_CARWASHSTART)) {
                //チップが洗車エリアにいるの場合
                gSearchedSubChipAreaId = C_CARWASH;
            } else if ((objSelectedChip.SVC_STATUS == C_SVCSTATUS_NOSHOW) && (objSelectedChip.STALL_USE_STATUS == C_STALLUSE_STATUS_NOSHOW)) {
                //チップがNoShowエリアにいるの場合
                gSearchedSubChipAreaId = C_NOSHOW;
            } else if (objSelectedChip.STALL_USE_STATUS == C_STALLUSE_STATUS_STOP) {
                //チップが中断エリアにいるの場合
                gSearchedSubChipAreaId = C_STOP;
            } else if (objSelectedChip.INSPECTION_STATUS == C_INSPECTION_APPROVAL) {
                //チップが完成検査エリアにいるの場合
                gSearchedSubChipAreaId = C_COMPLETIONINSPECTION
            }
            gSearchedChipId = objSelectedChip.STALL_USE_ID;
            //2017/07/12 NSK 河谷 REQ-SVT-TMT-20160906-001 チップ検索の検索範囲をPC-SMBと同じにする START
            gSearchedChipRoNum = objSelectedChip.RO_NUM;
            gSearchedChipRoSeq = objSelectedChip.RO_SEQ;
            //2017/07/12 NSK 河谷 REQ-SVT-TMT-20160906-001 チップ検索の検索範囲をPC-SMBと同じにする END
        } else {
            //2017/07/12 NSK 河谷 REQ-SVT-TMT-20160906-001 チップ検索の検索範囲をPC-SMBと同じにする START
            //// チップ表示日付が今日以後の場合、画面を遷移する
            //if (dtShowDate.getTime() - dtNow.getTime() >= 0) {
            //    gSearchedChipId = objSelectedChip.STALL_USE_ID;
            //    var nChangeDays = (dtShowDate.getTime() - dtNow.getTime()) / (24 * 60 * 60 * 1000);
            //    ClickChangeDate(nChangeDays);
            //    return;
            //}

            //チップ表示日付の工程管理画面に遷移する
            gSearchedChipId = objSelectedChip.STALL_USE_ID;
            var nChangeDays = (dtShowDate.getTime() - dtNow.getTime()) / (24 * 60 * 60 * 1000);
            ClickChangeDate(nChangeDays);
            // 2019/08/07 NSK 鈴木 【TKM】チップ検索からSMBに遷移した時、タイムアウトの文言が表示される START
            // return;
            // 2019/08/07 NSK 鈴木 【TKM】チップ検索からSMBに遷移した時、タイムアウトの文言が表示される END
            //2017/07/12 NSK 河谷 REQ-SVT-TMT-20160906-001 チップ検索の検索範囲をPC-SMBと同じにする END
        }
    }

    var jsonData = {
        Method: "ShowMainArea",
        ShowDate: $("#hidShowDate").val()
    };
    gCallbackSC3240101.doCallback(jsonData, SC3240101AfterCallBack);
});

//2015/07/30 TMEJ 明瀬 アクティビティインジケータが消えない問題 START

////メイン画面の表示
////@param {bScrollFlg} 最初の位置に移動するかどうか
////@return {無し}
//function ShowMainArea() {

//メイン画面の表示
//@param {method} イベントの発生元を示す文字列
//@return {void}
function ShowMainArea(method) {

//2015/07/30 TMEJ 明瀬 アクティビティインジケータが消えない問題 END

    // 最初の位置に移動するかどうか
    if (gCanScrollFlg) {
        // ウィンドウをスクロール
        ScrollWindowToNowTime(true);
    } else {
        gCanScrollFlg = true;
    }

    var strBackSubChipId = "";
    var objBackSubChip = null;    
    
    // 納車予定時刻線を非表示にする
    $(".TimingLineDeli").css("visibility", "hidden");

    //受付ボックスのチップを選択した場合、テーブルの選択状態を解除しない
    if ((gArrObjSubChip[gSelectedChipId])
        && ((gArrObjSubChip[gSelectedChipId].subChipAreaId == C_RECEPTION)
        || (gArrObjSubChip[gSelectedChipId].subChipAreaId == C_NOSHOW)
        || (gArrObjSubChip[gSelectedChipId].subChipAreaId == C_STOP))) {
        strBackSubChipId = gSelectedChipId;
        CreateChips();          //全チップを生成
        SubChipTap(gArrObjSubChip[strBackSubChipId]);
        objBackSubChip = gMovingChipObj;
    } else {
        if (gOpenningSubBoxId != "") {
            SetSubChipBoxClose();   //表示中のサブエリアを非表示にする
        }
        CreateChips();          //全チップを生成
        // 2013/11/29 TMEJ 張 タブレット版SMB チーフテクニシャン機能開発 START
        //日付切替時、選択したチップが新規チップの場合
        if (gSelectedChipId == "") {
            CreateFooterButton(C_FT_DISPTP_UNSELECTED, 0);    //フッター部を未選択状態にする
        }
        // 2013/11/29 TMEJ 張 タブレット版SMB チーフテクニシャン機能開発 END
    }

    ShowCalendar();
    InitRedTimeLinePropty();

    gMainAreaActiveIndicator.hide();    //グルグルを非表示

    //2015/07/30 TMEJ 明瀬 アクティビティインジケータが消えない問題 START

    //イベント発生元が初期表示の場合
    if (method == "ShowMainArea") {

        //リフレッシュタイマーをクリア
        ClearMainWndTimer();

        //画面更新中フラグをFalse(更新中でない)に設定
        gUpdatingDisplayFlg = false;
    }

    //2015/07/30 TMEJ 明瀬 アクティビティインジケータが消えない問題 END

    if (strBackSubChipId != "") {
        gSelectedChipId = strBackSubChipId;
        gMovingChipObj = objBackSubChip;
    }
}

// 2013/11/29 TMEJ 張 タブレット版SMB チーフテクニシャン機能開発 START
//当画面リフレッシュ
//@param {String} チップ情報
//@return {無し}
function RefreshMainArea(strChipData) {
    // 最初の位置に移動するかどうか
    if (gCanScrollFlg) {
        // ウィンドウをスクロール
        ScrollWindowToNowTime(true);
    } else {
        gCanScrollFlg = true;
    }

    SetSubChipBoxClose();   //表示中のサブエリアを非表示にする
    ShowLatestChips(strChipData, false, true);      //情報により、チップを更新する

    InitRedTimeLinePropty();

    gMainAreaActiveIndicator.hide();    //グルグルを非表示

    //2015/07/30 TMEJ 明瀬 アクティビティインジケータが消えない問題 START

    //リフレッシュタイマーをクリア
    ClearMainWndTimer();

    //画面更新中フラグをFalse(更新中でない)に設定
    gUpdatingDisplayFlg = false;

    //2015/07/30 TMEJ 明瀬 アクティビティインジケータが消えない問題 END

}
// 2013/11/29 TMEJ 張 タブレット版SMB チーフテクニシャン機能開発 END

//サーバとの時間差を算出し、グローバル変数に格納する.
//@return {void}
function SetServerTimeDifference() {
    var pageLoadServerTime = new Date($("#hidServerTime").val());   //ページ読込時のサーバ時間を取得
    var pageLoadClientTime = new Date();    //クライアントの現在時刻を取得
    gServerTimeDifference = pageLoadServerTime - pageLoadClientTime;    //サーバとの時間差を算出し、格納（ミリ秒）
}

//サーバの現在時刻を算出し、返す
//@return {Date}
function GetServerTimeNow() {
    var serverTime = new Date();    //サーバの現在時刻を算出  
    serverTime.setTime(serverTime.getTime() + gServerTimeDifference);
    return serverTime;
}

//文言取得
//@return {Date}
function GetSC3240101WordIni() {
    var strWordIni = $("#hidMsgData").val();
    if (gSC3240101WordIni == null) {
        gSC3240101WordIni = $.parseJSON(strWordIni);

        // 2014/07/17 TMEJ 張 文言エンコード対応 START
        for (var strWordNo in gSC3240101WordIni) {
            gSC3240101WordIni[strWordNo] = htmlDecode(gSC3240101WordIni[strWordNo]);
        }
        // 2014/07/17 TMEJ 張 文言エンコード対応 END

        $("#hidMsgData").attr("value", "");
    }
}

//メッセージを表示する
//@return {String}
//2017/01/17 NSK 竹中 TR-SVT-TMT-20151019-002 TCメイン画面のエラーメッセージが理解不可能 START
//function ShowSC3240101Msg(strWordNo) {
function ShowSC3240101Msg(strWordNo, stallId) {
//2017/01/17 NSK 竹中 TR-SVT-TMT-20151019-002 TCメイン画面のエラーメッセージが理解不可能 END
    //gSC3240101WordIni[strWordNo]があれば、メッセージボックスで表示
    if (gSC3240101WordIni != null) {
        if (gSC3240101WordIni[strWordNo] != null) {
            var strMsg = gSC3240101WordIni[strWordNo];
            // 「営業開始時間({0}:{1})以降に配置してください。」の場合
            if (strWordNo == 911) {
                strMsg = strMsg.replace("{0}", add_zero(gStartWorkTime.getHours()));
                strMsg = strMsg.replace("{1}", add_zero(gStartWorkTime.getMinutes()));
            } else if (strWordNo == 912) {
                //「営業終了時間({0}:{1})以内に配置してください。」の場合
                strMsg = strMsg.replace("{0}", add_zero(gEndWorkTime.getHours()));
                strMsg = strMsg.replace("{1}", add_zero(gEndWorkTime.getMinutes()));
            //2017/01/17 NSK 竹中 TR-SVT-TMT-20151019-002 TCメイン画面のエラーメッセージが理解不可能 START
            } else if (strWordNo == 937) {
                var stallName = $('#ulStall #stallId_' + stallId + ' #lblStallName').text();
                if (stallName != " " && stallName != C_STR_DEFAULT_VALUE) {
                    strMsg = strMsg.replace("{0}", stallName);
                } else {
                    strMsg = gSC3240101WordIni[910];
                }
            //2017/01/17 NSK 竹中 TR-SVT-TMT-20151019-002 TCメイン画面のエラーメッセージが理解不可能 END
            }
            alert(strMsg);
        }
    }
}

//確認メッセージを表示
//@return {String}
function ConfirmSC3240101Msg(strWordNo) {
    // gSC3240101WordIni[strWordNo]があれば、メッセージボックスで表示
    if (gSC3240101WordIni != null) {
        if (gSC3240101WordIni[strWordNo] != null) {
            return confirm(gSC3240101WordIni[strWordNo]);
        }
    }
}

/**
* 再表示タイマーをセットする.
* @param {refreshFunc} 再表示用のJavaScrep関数 -
* @return {-} -
*/
function RefreshMainWndTimer(refreshFunc) {

    //タイマー間隔の取得
    var refreshTime = Number($("#MstPG_RefreshTimerTime").val());

    var rtValue = setTimeout(function () {

        //出力メッセージを選択する
        var messageString = $("#MstPG_RefreshTimerMessage1").val();
        alert(messageString);
        //各画面でリフレッシュ処理をする
        ClearAllMainWndTimer();
        refreshFunc();
    }, refreshTime);
    gArrTimeoutValue.push(rtValue);
}

/**
* 再表示タイマーをリセットする.
*/
function ClearMainWndTimer() {
    if (gArrTimeoutValue.length > 0) {
        // arrayに1番目のtimeoutをクリア
        clearTimeout(gArrTimeoutValue[0]);
        gArrTimeoutValue.splice(0, 1);
    }
}
/**
* 再表示タイマーをリセットする.
*/
function ClearAllMainWndTimer() {
    for (var nLoop = 0; nLoop < gArrTimeoutValue.length; nLoop++){
        clearTimeout(gArrTimeoutValue[nLoop]);
    }
    gArrTimeoutValue.length = 0;
    // 操作リストをクリアする
    ClearOperationList();
}
/**
* 営業開始、終了時間を設定
*/
function SetStallDate() {
    var strStartWorkTime = $("#hidShowDate").val() + " " + document.getElementById("hidStallStartTime").value;
    var strEndWorkTime = $("#hidShowDate").val() + " " + document.getElementById("hidStallEndTime").value;

    gStartWorkTime = new Date(strStartWorkTime);
    gEndWorkTime = new Date(strEndWorkTime);
    gResizeInterval = parseInt(document.getElementById("hidIntervlTime").value);
    gRefreshTimerInterval = parseInt(document.getElementById("hidTabletSmbRefreshInterval").value);

    // 終了時間が開始時間より小さいの場合、24時間をプラスする
    if (gEndWorkTime - gStartWorkTime < 0) {
        gEndWorkTime.setTime(gEndWorkTime.getTime() + 24 * 60 * 60 * 1000);
    }
}

//2014/06/18 TMEJ 丁 タブレット版SMB テレマ走行距離機能開発 START
/**
* G-BOOKボタンイベント
*/
function ClickGBook() {

    // グルグルを表示する
    gMainAreaActiveIndicator.show();

    var strParam;
    //サブチップの場合
    if (gArrObjSubChip[gSelectedChipId]) {
        strParam = '{'
        strParam += '"OperationCode":"' + C_FT_BTNID_GBOOK + '"';
        strParam += ',"VclID":"' + gArrObjSubChip[gSelectedChipId].vclId + '"';
        strParam += '}';
    } else {
    //ストールチップの場合
        strParam = '{'
        strParam += '"OperationCode":"' + C_FT_BTNID_GBOOK + '"';
        strParam += ',"VclID":"' + gArrObjChip[gSelectedChipId].vclId + '"';
        strParam += '}';
    }
    //画面遷移のためポストバック
    __doPostBack("", strParam);
    return false;
}
//2014/06/18 TMEJ 丁 タブレット版SMB テレマ走行距離機能開発 END


// 2014/09/11 TMEJ 張 BTS-389 「SMBで異常ではないはずが、遅れ見込み表示になっている」対応 START

// 2017/09/25 NSK 竹中(悠) REQ-SVT-TMT-20170227-002 見込納車遅れ日時計算ロジック統一 START
// /**
// * 遅れ見込み時間を取得
// * @param {carWashNeedFlg} 洗車必要フラグ -
// * @param {cwRsltStartDateTime} 洗車終了実績日時 -
// * @param {scheDeliDateTime} 納車予定日時 -
// * @param {svcStatus} サービスステータス -
// * @return {dtScheDeliLater} {遅れ見込み時間}-
// */
// function GetDeliDelayExpectedTimeLine(carWashNeedFlg, cwRsltEndDateTime, scheDeliDateTime, svcStatus) {

/**
* 遅れ見込み時間を取得
* @param {carWashNeedFlg} 洗車必要フラグ -
* @param {scheDeliDateTime} 納車予定日時 -
* @param {svcStatus} サービスステータス -
* @param {inspectionType} 残完成検査区分 -
* @return {dtScheDeliLater} {遅れ見込み時間}-
*/
function GetDeliDelayExpectedTimeLine(carWashNeedFlg, scheDeliDateTime, svcStatus, inspectionType) {
// 2017/09/25 NSK 竹中(悠) REQ-SVT-TMT-20170227-002 見込納車遅れ日時計算ロジック統一 END

    // 納車作業標準時間
    var nDeliWorkTime = parseInt($("#hidStandardDeliWrTime").val(), 10);

    // 納車準備時間
    var nDeliPreTime = parseInt($("#hidStandardDeliPreTime").val(), 10);

    // 洗車標準時間
    var nWashTime = parseInt($("#hidStandardWashTime").val(), 10);

    // 2017/09/25 NSK 竹中(悠) REQ-SVT-TMT-20170227-002 見込納車遅れ日時計算ロジック統一 START
    
    // 検査標準時間
    var nInspectionTime = parseInt($("#hidStandardInspectionTime").val(), 10);

    // 2017/09/25 NSK 竹中(悠) REQ-SVT-TMT-20170227-002 見込納車遅れ日時計算ロジック統一 END

    // 納車見込み線の日時
    var dtScheDeliLater = new Date();

    // 2017/09/25 NSK 竹中(悠) REQ-SVT-TMT-20170227-002 見込納車遅れ日時計算ロジック統一 START
//    if (carWashNeedFlg == "1") {
//        // 洗車ありの場合

//        if (IsDefaultDate(cwRsltStartDateTime)) {
//            // 洗車未開始

//            // 洗車標準時間と納車準備時間の長いほうを使う
//            if (nDeliPreTime > nWashTime) {

//                // 納車準備時間 + 納車作業標準時間
//                nDeliWorkTime = nDeliWorkTime + nDeliPreTime;

//            } else {

//                // 洗車準備時間 + 納車作業標準時間
//                nDeliWorkTime = nDeliWorkTime + nWashTime;

//            }

//            // 納車予定日時 - 納車作業標準時間 - 標準洗車時間(納車準備時間)
//            dtScheDeliLater.setTime(scheDeliDateTime.getTime() - nDeliWorkTime * 60000);

//        } else {
//            // 洗車開始した

//            // 納車予定日時 - 納車作業標準時間
//            dtScheDeliLater.setTime(scheDeliDateTime.getTime() - nDeliWorkTime * 60000);

//        }

//    } else {
//        // 洗車なしの場合

//        if ((svcStatus == C_SVCSTATUS_DROPOFFCUSTOMER) ||
//            (svcStatus == C_SVCSTATUS_WAITINGCUSTOMER)) {
//            //納車準備の場合(サービスステータスが預かり中または納車待ち)

//            // 納車予定日時 - 納車作業標準時間
//            dtScheDeliLater.setTime(scheDeliDateTime.getTime() - nDeliWorkTime * 60000);

//        } else {
//            // 作業中

//            // 納車予定日時 - 納車作業標準時間 - 納車準備時間
//            dtScheDeliLater.setTime(scheDeliDateTime.getTime() - (nDeliWorkTime + nDeliPreTime) * 60000);

//        }

//    }

    //洗車標準時間と納車準備標準時間の長いほうを使う
    var addLongTime = 0;
    if (nDeliPreTime > nWashTime) {

        //納車準備標準時間が長い場合            
        addLongTime = nDeliPreTime;
    }else {
                    
        //洗車標準時間のが長い場合
        addLongTime = nWashTime;
    }

    if ((svcStatus == C_SVCSTATUS_CARWASHSTART) ||
        (svcStatus == C_SVCSTATUS_DROPOFFCUSTOMER) ||
        (svcStatus == C_SVCSTATUS_WAITINGCUSTOMER)) {
        // サービスステータスが洗車中、預かり中、納車待ちの場合

        // 納車予定日時 - 納車作業標準時間
        dtScheDeliLater.setTime(scheDeliDateTime.getTime() - nDeliWorkTime * 60000);

    } else if (svcStatus == C_SVCSTATUS_CARWASHWAIT) {
        // サービスステータスが洗車待ちの場合

        // 納車予定日時 - 洗車標準時間 - 納車作業標準時間
        dtScheDeliLater.setTime(scheDeliDateTime.getTime() - (nWashTime + nDeliWorkTime) * 60000);

    } else {
        // 上記以外
        
        if (inspectionType == C_NOTFINISH_FINAL_INSPECTION) {
            //残完成検査区分が完成検査入力未完了の場合

            //納車予定日時 - 検査標準時間 - 納車作業標準時間
            dtScheDeliLater.setTime(scheDeliDateTime.getTime() - (nInspectionTime + nDeliWorkTime) * 60000);

        } else {
            //上記以外の場合

            //納車予定日時 - 納車作業標準時間
            dtScheDeliLater.setTime(scheDeliDateTime.getTime() - nDeliWorkTime * 60000);

        }

        //洗車ありの場合
        if (carWashNeedFlg == "1") {

            //遅れ見込み時間 - (洗車標準時間or納車準備標準時間)
            dtScheDeliLater.setTime(dtScheDeliLater.getTime() - addLongTime * 60000);
        } else {
            //上記以外の場合

            //遅れ見込み時間 - 納車準備標準時間
            dtScheDeliLater.setTime(dtScheDeliLater.getTime() - nDeliPreTime * 60000);
        }
    }

    // 2017/09/25 NSK 竹中(悠) REQ-SVT-TMT-20170227-002 見込納車遅れ日時計算ロジック統一 END
    return dtScheDeliLater;
}
// 2014/09/11 TMEJ 張 BTS-389 「SMBで異常ではないはずが、遅れ見込み表示になっている」対応 END
