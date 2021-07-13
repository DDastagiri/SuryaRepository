//---------------------------------------------------------
//SC3240701.js
//---------------------------------------------------------
//機能：ストール使用不可画面
//作成：2017/08/30 NSK 竹中(悠) REQ-SVT-TMT-20161109-001 SMB iPadにストールクローズ機能の追加
//更新：
//---------------------------------------------------------


/**	
* キャンセルボタン
* 	
*/
function UnavailableCancelButton() {
    //ポップアップを閉じる
    CloseUnavailableSetting();
    return false;
}

/**	
* ポップアップを閉じる
* 	
*/
function CloseUnavailableSetting() {

    $("#UnavailableRegisterBtn").unbind();
    $("#UnavailableSettingPopup").fadeOut(300);

    //フッターボタンを再表示する
    //モードによってフッターの描画を変える
    if (gSelectedChipId == C_NEWCHIPID) {
        //新規チップ選択時
        CreateFooterButton(C_FT_DISPTP_SELECTED, C_FT_BTNTP_REZ_NEW);
    } else if (IsUnavailableArea(gSelectedChipId)) {
        //使用不可チップ選択時
        CreateFooterButton(C_FT_DISPTP_SELECTED, C_FT_BTNTP_UNAVAILABLE);
    }

    //タイマーをクリア
    commonClearTimer();

    return false;
}

/**	
* 登録ボタン
* 	
*/
function UnavailableRegisterButton() {

    //アクティブインジケータ表示
    gUnavailableActiveIndicator.show();

    //オーバーレイ表示
    gUnavailableOverlay.show();

    //リフレッシュタイマーセット
    commonRefreshTimer(ReDisplayUnavailableChip);

    //サーバーに渡すパラメータを作成
    var prms = CreateUnavailableCallBackRegistParam(C_SC3240701CALLBACK_REGISTER, gSelectedChipId);

    //コールバック開始
    DoCallBack(C_CALLBACK_WND701, prms, SC3240701AfterCallBack, "UnavailableRegisterButton");

    return false;
}

/**
* ポップアップ表示(ストール使用不可ボタン、詳細ボタンクリック時にコール) 
*/
function ShowUnavailableSetting() {

    try {

        //アクティブインジケータ・オーバーレイ非表示
        gUnavailableActiveIndicator.hide();
        gUnavailableOverlay.hide();

        //ストール使用不可画面の登録ボタンを活性にする
        $("#UnavailableRegisterBtn").attr("disabled", false);

        //表示用コンテンツを削除
        $('#UnavailableSettingDetailContent>div').remove();

        //工程管理画面で選択されているチップID
        var baseCtrl = $("#" + gSelectedChipId);

        //ポップアップの表示位置を設定
        SetUnavailablePopoverPosition(CalcUnavailablePopoverPosition(baseCtrl));

        //ポップアップ表示
        $("#UnavailableSettingPopup").fadeIn(300);

        //アクティブインジケータ表示
        gUnavailableActiveIndicator.show();

        //リフレッシュタイマーセット
        commonRefreshTimer(ReDisplayUnavailableChip);

        //画面初期化情報を取得して作成する
        InitUnavailableSettingPage();

    }
    catch (e) {

        // gSelectedChipIdが空白の場合
        if (gSelectedChipId == "") {

            // ストール使用不可画面を閉じる
            HideChipDetailPopup();

            // 選択状態を解除する
            SetChipUnSelectedStatus();
            SetTableUnSelectedStatus();
        }
    }
};

/**
* 画面を作成する
* 
*/
function InitUnavailableSettingPage() {
    //サーバーに渡すパラメータを作成
    var prms = CreateUnavailableCallBackDisplayParam(C_SC3240701CALLBACK_CREATEDISP, gSelectedChipId);

    //コールバック開始
    DoCallBack(C_CALLBACK_WND701, prms, SC3240701AfterCallBack, "CreateUnavailableChipPage");
}



/**
* コールバックでサーバーに渡すパラメータを作成する(初期表示)
*
* @param {String}   method:   コールバック時のメソッド分岐用
* @param {Integer}  chipId:   チップのID
*
*/
function CreateUnavailableCallBackDisplayParam(method, chipId) {

    //新規作成の場合
    if (chipId == C_NEWCHIPID) {
        var rtnVal = {
            Method: method
                , StartIdleTime: gArrObjChip[C_NEWCHIPID].displayStartDate    //チップ表示用開始時間
                , FinishIdleTime: gArrObjChip[C_NEWCHIPID].displayEndDate     //チップ表示用終了時間
                , StallIdleId: 0
        };
    } else {
        //更新の場合
        //ストール非稼働ID取得
        var stallIdleId = right(chipId, chipId.length - C_UNAVALIABLECHIPID.length)
        var rtnVal = {
            Method: method
                , StallIdleId: stallIdleId
        };
    }
    return rtnVal;
}

/**
* コールバックでサーバーに渡すパラメータを作成する(登録ボタン)
*
* @param {String}   method:   コールバック時のメソッド分岐用
* @param {Integer}  chipId:   チップのID
*
*/
function CreateUnavailableCallBackRegistParam(method, chipId) {

    var strStallId = getStallId(chipId)
    //共通
    var rtnVal = {
        Method: method
        , DlrCD: $("#hidDlrCD").val()                                    //販売店コード
        , StrCD: $("#hidBrnCD").val()                                    //店舗コード
        , ShowDate: $("#hidShowDate").val()                              //表示日時(yyyy/MM/dd)
        , Account: icropScript.ui.account                                //ログインアカウント
        , StartIdleTime: smbScript.ConvertDateToString2(smbScript.changeStringToDateIcrop($("#StartIdleDateTimeSelector").get(0).value))           ////使用不可開始日時
        , FinishIdleTime: smbScript.ConvertDateToString2(smbScript.changeStringToDateIcrop($("#FinishIdleDateTimeSelector").get(0).value))         //使用不可終了日時
        , IdleTime: $("#IdleTimeHidden").val()                           //非稼働時間
        , IdleMemo: $("#IdleMemoTxt").val()                               //メモ
        , ValidateCode: CheckUnavailableSettingInputValue()                     //入力項目チェック結果コード
    }

     if (chipId == C_NEWCHIPID) {
        //新規作成モード
        rtnVal.StallId = gArrObjChip[C_NEWCHIPID].stallId                      //ストールID
        rtnVal.RowLockVersion = gArrObjChip[C_NEWCHIPID].rowLockVersion        //行ロックバージョン(0)

    } else {
        //更新モード
        rtnVal.RowLockVersion = $("#" + chipId).data("ROW_LOCK_VERSION")       //行ロックバージョン
        var stallIdleId = right(chipId, chipId.length - C_UNAVALIABLECHIPID.length)
        rtnVal.StallIdleId = stallIdleId                                       //ストール非稼働ID
        rtnVal.StallId = right(strStallId, strStallId.length - 8)              //ストールID
    }

    return rtnVal;
}

/**	
* StallIdを取得
* 	
*/
function getStallId(chipId) {

    var nRow = GetRowNoByChipId(chipId);

    return $(".stallNo" + nRow)[0].id;
}

/**	
* 登録時、入力項目値のチェックを行う
* 	
*/
function CheckUnavailableSettingInputValue() {

    var rtnVal = 0;
    var startIdle = smbScript.changeStringToDateIcrop($("#StartIdleDateTimeSelector").get(0).value);      //使用不可開始日時
    var endIdle = smbScript.changeStringToDateIcrop($("#FinishIdleDateTimeSelector").get(0).value);       //使用不可終了日時

    //使用不可開始、終了時間の前後関係が不正な場合
    if (!smbScript.CheckContextOfPlan(null, startIdle, endIdle, null)) {
        rtnVal = 903;   //非稼働日時の大小関係が不正です。
    }


    return rtnVal;
}

/**	
* 必須項目の空チェックを行う
* 	
*/
function IsMandatoryUnavailableSettingEmpty() {

    var rtnVal = false;
    var checkItem1 = $("#StartIdleDateTimeLabel").text();     //開始予定
    var checkItem2 = $("#FinishIdleDateTimeLabel").text();    //終了予定

    //「開始予定」が空 または「終了予定」が空の場合
    if (checkItem1 == "" || checkItem2 == "") {
        rtnVal = true;
    }

    return rtnVal;
}

/**
* コールバック後の処理関数
* 
* @param {String} result コールバック呼び出し結果
* @param {String} context
*
*/
function SC3240701AfterCallBack(result, context) {

    //JSON解析
    var jsonResult = JSON.parse(result);
    jsonResult.Contents = htmlDecode(jsonResult.Contents);
    jsonResult.Message = htmlDecode(jsonResult.Message);
    jsonResult.UnavailableChipJson = htmlDecode(jsonResult.UnavailableChipJson);

    //タイマーをクリア
    commonClearTimer();

    //画面作成
    if (jsonResult.Caller == C_SC3240701CALLBACK_CREATEDISP && jsonResult.ResultCode == 0) {

        //画面の初期化 
        InitUnavailablePage(result, context);

        //アクティブインジケータ・オーバーレイ非表示
        gUnavailableActiveIndicator.hide();
        gUnavailableOverlay.hide();


        //次の操作を実行する
        AfterCallBack();

    } else if(jsonResult.ResultCode == 8) {
        //存在チェックエラー
        //ダイアログ
        icropScript.ShowMessageBox(jsonResult.ResultCode, jsonResult.Message, "");

        //ストール使用不可画面を閉じる
        CloseUnavailableSetting();

        //選択したチップを解放する(SetTableUnSelectedStatusは~/SC3240101/Table.js内のメソッド)
        SetTableUnSelectedStatus();

        //チップ選択状態を解除する
        SetChipUnSelectedStatus();

        //操作リストをクリアする
        ClearOperationList();

        //工程管理画面のチップを再描画する(ClickChangeDate~/SC3240101/Table.js内のメソッド)
        ClickChangeDate(0);
    } else if (jsonResult.ResultCode != 0) {

        //ダイアログ
        icropScript.ShowMessageBox(jsonResult.ResultCode, jsonResult.Message, "");

        //アクティブインジケータ・オーバーレイ非表示
        gUnavailableActiveIndicator.hide();
        gUnavailableOverlay.hide();

        AfterCallBack();

    } else {
        //登録ボタンクリック後 

        //アクティブインジケータ・オーバーレイ非表示
        gUnavailableActiveIndicator.hide();
        gUnavailableOverlay.hide();

        //ストール使用不可画面を閉じる
        CloseUnavailableSetting();

        var dtStartTime;
        var dtEndTime;

        // 当ページの日付を取得する
        var dtShow = new Date($("#hidShowDate").val());

        var stallIdleList = $.parseJSON(jsonResult.UnavailableChipJson);
        for (var keyString in stallIdleList) {
            // 使用不可チップの開始時間と終了時間を取得する
            dtStartTime = new Date(stallIdleList[keyString].IDLE_START_DATETIME);
            dtEndTime = new Date(stallIdleList[keyString].IDLE_END_DATETIME);


            if (gSelectedChipId == C_NEWCHIPID) {
                //新規チップ削除
                $("#" + C_NEWCHIPID).remove();

            } else if (gSelectedChipId == C_UNAVALIABLECHIPID + stallIdleList[keyString].STALL_IDLE_ID) {

                //更新前使用不可チップ削除
                $("#" + C_UNAVALIABLECHIPID + stallIdleList[keyString].STALL_IDLE_ID).remove();
            }
            //当ページの営業時間内の場合
            if ((dtStartTime < gEndWorkTime) && (dtEndTime > gStartWorkTime)) {

                //チップ描画
                DrawUnavailableArea(stallIdleList[keyString].STALL_IDLE_ID, stallIdleList[keyString].STALL_ID, stallIdleList[keyString].IDLE_START_DATETIME, stallIdleList[keyString].IDLE_END_DATETIME, stallIdleList[keyString].ROW_LOCK_VERSION, stallIdleList[keyString].IDLE_MEMO);
            }

            //選択したチップを解放する(SetTableUnSelectedStatusは~/SC3240101/Table.js内のメソッド)
            SetTableUnSelectedStatus();

            //チップ選択状態を解除する
            SetChipUnSelectedStatus();
        }
        //次の操作を実行する
        AfterCallBack();
    }
}

/**
* 画面を初期化する
*
*/
function InitUnavailablePage(result, context) {

    //コールバックによって取得したHTMLを設定
    var jsonResult = JSON.parse(result);
    SetUnavailableContents(jsonResult.Contents);

    //タイマーをクリア
    commonClearTimer();

    //CustomLabelの適用
    $("#UnavailableSettingPopup .UnavailableEllipsis").CustomLabel({ useEllipsis: true });

    //縦スクロールの設定
    //初期では縦スクロールさせない
    $("#UnavailableSettingDetailContent").fingerScroll({ action: "stop" });

    //メモエリア初期化
    InitUnavailableTextArea($("#IdleMemoTxt"), $("#IdleMemoDt"));

    //使用不可画面のテキストイベント設定
    SetUnavailableSettingTextEvent();

    var timeSpan = smbScript.CalcTimeSpan(smbScript.changeStringToDateIcrop($("#StartIdleDateTimeSelector").get(0).value), smbScript.changeStringToDateIcrop($("#FinishIdleDateTimeSelector").get(0).value));
    if (timeSpan != null) {
        $("#UnavailableWorkTimeHidden").val(timeSpan);
    }
}

/**	
* テキストエリアの初期化を行う
* 	
* @param {$(textarea)} ctrlTa
* @param {$(dt)} ctrlDt
* @return {-} -
*
*/
function InitUnavailableTextArea(ctrlTa, ctrlDt) {
    var settingHeight = 0;
    var textArea = ctrlTa;
    var headerDt = ctrlDt;

    //初期表示データが5行以上ある場合、設定値はscrollHeight
    if (C_UNAVAILABLE_TA_DEFAULTHEIGHT < textArea.attr("scrollHeight")) {
        settingHeight = textArea.attr("scrollHeight");

        $("#UnavailableSettingDetailContent").fingerScroll();
    }
    //5行未満はデフォルト値
    else {
        settingHeight = C_UNAVAILABLE_TA_DEFAULTHEIGHT;

    }

    //テキストエリアとヘッダーに高さ設定
    textArea.height(settingHeight);
    headerDt.css("line-height", settingHeight + 12 + "px");
}

/**	
* テキストエリアの高さ調整を行う
* 	
* @param {$(textarea)} ctrlTa
* @param {$(dt)} ctrlDt
* @return {-} -
*
*/
function UnavailableTextArea(ctrlTa, ctrlDt) {

    var textArea = ctrlTa;
    var headerDt = ctrlDt;

    textArea.height(C_UNAVAILABLE_TA_DEFAULTHEIGHT);

    var tmp_sh = textArea.attr("scrollHeight");

    while (tmp_sh > textArea.attr("scrollHeight")) {
        tmp_sh = textArea.attr("scrollHeight");
        textarea[0].scrollHeight++;
    }

    //スクロール有無の設定
    if (textArea.attr("scrollHeight") > textArea.attr("offsetHeight")) {
        //デフォルトを超えたとき
        //縦スクロール設定
        $("#UnavailableSettingDetailContent").fingerScroll();
    } else if (textArea.attr("scrollHeight") == textArea.attr("offsetHeight")) {
        //縦スクロール停止
        $("#UnavailableSettingDetailContent").fingerScroll({ action: "stop" });
    }

    // 高さ調整
    if (textArea.attr("scrollHeight") > textArea.attr("offsetHeight")) {
        textArea.height(textArea.attr("scrollHeight"));
        headerDt.css("line-height", (textArea.attr("scrollHeight") + 12) + 'px');
    } else {

        // 初期値
        headerDt.css("line-height", C_UNAVAILABLE_TA_DEFAULTHEIGHT + 12 + "px");
    }
}

/**
* コールバックで取得したHTMLを画面に設定する
* 
* @param {String} cbResult コールバック呼び出し結果
* 
*/
function SetUnavailableContents(cbResult) {

    //コールバックによって取得したHTMLを格納
    var contents = $('<Div>').html(cbResult).text();

    //使用不可画面のコンテンツを取得
    var UnavailableChip = $(contents).find('#UnavailableSettingDetailContent');

    //使用不可画面のHiddenコンテンツを取得
    var UnavailableChipHidden = $(contents).find('#SC3240701HiddenArea');

    //使用不可画面のコンテンツを削除
    $('#UnavailableSettingDetailContent>div').remove();

    //使用不可画面のコンテンツを設定
    UnavailableChip.children('div').clone(true).appendTo('#UnavailableSettingDetailContent');

    //使用不可画面のHiddenコンテンツを削除
    $('#SC3240701HiddenArea>div').remove();

    //使用不可画面のHiddenコンテンツを設定
    UnavailableChipHidden.children('div').clone(true).appendTo('#SC3240701HiddenArea');

}


/**
* ポップアップを表示する位置を設定します
*/
function SetUnavailablePopoverPosition(ctrlPosition) {
    //ポップアップ
    $("#UnavailableSettingPopupContent").css("left", ctrlPosition.popX);
    $("#UnavailableSettingPopupContent").css("top", ctrlPosition.popY);

    //矢印
    $(".ArrowMask").css("left", ctrlPosition.arrowX);
    $(".ArrowMask").css("top", ctrlPosition.arrowY);

    //インジケータ
    $("#UnavailableActiveIndicator").css({ "left": ($("#UnavailableMyDataBoxDiv").outerWidth() / 2) });
    $("#UnavailableActiveIndicator").css({ "top": ($("#UnavailableMyDataBoxDiv").outerHeight() / 2) });


    //使用不可画面表示時のleft値を保持しておく
    gUnavailablePopX = ctrlPosition.popX;
}

/**
* ポップアップを表示する方向を計算し、設定値を決定します
*/
function CalcUnavailablePopoverPosition(baseCtrl) {

    var possibleDir = {
        left: false,
        right: false
    }

    var ctrlPosition = {
        popX: 0,    //ポップアップのleft値
        popY: 0,
        arrowX: 0,  //吹出し三角のleft値
        arrowY: 0   //吹出し三角のtop値
    }

    //ポップアップをチップの左に表示する場合のleft値
    var popDispLeftX = baseCtrl.offset().left - (($("#UnavailableArrowMask").width() / 2) - 3) - $("#UnavailableSettingPopupContent").width();

    //ポップアップをチップの右に表示する場合のleft値
    var popDispRightX = baseCtrl.offset().left + baseCtrl.width() + (($("#UnavailableArrowMask").width() / 2) - 3);

    var temp = $("#UnavailableSettingPopupContent").width();
    //チップのleft + チップのwidth + (吹出し三角のwidth / 2 - 3(微調整値)) + ポップアップのwidth <= 画面全体のwidth
    if (popDispRightX + $("#UnavailableSettingPopupContent").width() <= $(document.body).width()) {
        possibleDir.right = true;
    }
    // 0 <= チップのleft - (吹出し三角のwidth / 2 - 3(微調整値)) - ポップアップのwidth
    else if (0 <= popDispLeftX) {
        possibleDir.left = true;
    }

    if (possibleDir.right) {
        ctrlPosition.popX = popDispRightX;
        ctrlPosition.arrowX = C_SC3240701POP_DISPRIGHT_ARROW_X;
    }
    else if (possibleDir.left) {
        ctrlPosition.popX = popDispLeftX;
        ctrlPosition.arrowX = C_SC3240701POP_DISPLEFT_ARROW_X;
    }
    else {  //チップが長すぎてどちらも適切でない場合
        ctrlPosition.popX = C_SC3240701POP_DISPRIGHT_DEFAULT_X;
        ctrlPosition.arrowX = C_SC3240701POP_DISPRIGHT_ARROW_X;
    }

    // 矢印のtop(調整値)
    ctrlPosition.arrowY = 55

    //ポップアップのtop = チップのtop + (チップのheight / 2) - 115(調整値)
    ctrlPosition.popY = baseCtrl.offset().top + (baseCtrl.height() / 2) - 115;

    //ポップアップがヘッダーに隠れていないか
    if (ctrlPosition.popY <= 0) {
        ctrlPosition.popY = 0;
    }

    //ポップアップがフッターに隠れていないか
    var mainArea = document.getElementById("MainArea").offsetHeight;
    var popupHegiht = $("#UnavailableSettingPopupContent").height();
    if ((ctrlPosition.popY + popupHegiht) - mainArea >= 0) {

        ctrlPosition.popY = mainArea - popupHegiht;
        ctrlPosition.arrowY = (baseCtrl.offset().top) - ctrlPosition.popY - 15;

        // 矢印のtop
        if (ctrlPosition.arrowY < 0) {
            ctrlPosition.arrowY = 0;
        }
    }

    return ctrlPosition;
}

/**	
* テキストエリア内の文字列長制御を行う
* 	
* @param {$(textarea)} ta
*
*/
function ControlLengthTextarea(ta) {

    //許容する最大バイト数
    var maxLen = ta.attr("maxlen");
    var overFlg = 0;
    var v = ta.val();

    if (v.length > maxLen) {
        var overFlg = 1;
    }

    //許容する最大バイト数を超えていた場合のみ、切り出し処理を実施してセットしなおす
    if (overFlg == "1") {
        var AfterStr = v.substr(0, maxLen);
        ta.val(AfterStr);
    }
}