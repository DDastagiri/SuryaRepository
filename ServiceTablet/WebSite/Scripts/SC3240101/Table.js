//---------------------------------------------------------
//Table.js
//---------------------------------------------------------
//機能：メイン画面のJS
//作成：2012/12/22 TMEJ 張 タブレット版SMB機能開発(工程管理)
//更新：2013/11/29 TMEJ 張 タブレット版SMB チーフテクニシャン機能開発
//更新：2014/07/15 TMEJ 丁 タブレットSMB Job Dispatch機能開発
//更新：2014/07/15 TMEJ 張 タブレットSMB Job Dispatch機能開発
//更新：2014/09/12 TMEJ 張 BTS-390 「1ｽﾄｰﾙで2ﾁｯﾌﾟが作業中（表示のみ）」対応
//更新：2015/07/30 TMEJ 明瀬 アクティビティインジケータが消えない問題
//更新：2017/08/30 NSK 竹中(悠) REQ-SVT-TMT-20161109-001 SMB iPadにストールクローズ機能の追加
//更新：2018/02/20 NSK 小川 17PRJ01136-00 (トライ店システム評価)お客様受付における情報伝達の仕組み 適合性検証
//更新：2019/06/18 NSK 皆川 (トライ店システム評価)次世代サービスオペレーション効率化に向けた、業務適合性検証
//更新：2019/08/05 NSK 近藤 (トライ店システム評価)次世代サービスオペレーション効率化に向けた、業務適合性検証
//更新：2019/08/09 NSK 鈴木 [TKM]UAT-0488 ストールに配備する前に日付を切り替えると、リレーションチップが消える
//更新：2019/12/06 NSK 皆川 (トライ店システム評価)整備作業における休憩時間取得判定向上検証
//---------------------------------------------------------
/**
* テーブルの生成
* @return {なし}
*/
function CreateTable() {
    // 変数初期化
    gSelectedCellId = "";

    // 仕事時間
    var nWorkTime = gEndWorkTime.getHours() - gStartWorkTime.getHours();
    if (gEndWorkTime.getMinutes() > 0) {
        nWorkTime += 1;
    } 
    
    // 仕事時間によって、最大カラムを計算する
    gMaxCol = nWorkTime * 4;
    // 画面全体にタップイベントを登録する
    BindAllAreaClickEvent();

    /**********************************************************/
    /* 1．テーブルの左部分（ストール名、テクニク名）
    /**********************************************************/
    // ストールのタッチのイベントを登録する
    BindStallAreaClickEvent();

    /**********************************************************/
    /* 2．テーブルの時間エリアの部分
    /**********************************************************/
    // 時間の幅を再計算する
    $("#divScrollTime").css("width", gMaxCol * C_CELL_WIDTH + "px");

    /**********************************************************/
    /* 3．テーブルのチップエリアの部分
    /**********************************************************/
    // テーブルを生成する
    var strHtml = "";
    for (var nRow = 1; nRow <= gMaxRow; nRow++) {
        strHtml += "<div class='TbRow Row" + nRow + "'></div>";
    }
    document.getElementById("ulChipAreaBack_lineBox").innerHTML = "";
    document.getElementById("ulChipAreaBack_lineBox").innerHTML = strHtml;

    // 行の幅を設定する
    $(".TbRow").css("width", gMaxCol * C_CELL_WIDTH + "px");
    // 行毎に高さを設定する
    var nTop = 0;
    for (var nRow = 1; nRow <= gMaxRow; nRow++) {
        $(".Row" + nRow).css("top", nTop);
        $(".stallNo" + nRow).css("top", nTop);
        nTop += C_CELL_HEIGHT;
    }
    $(".NameList").css("visibility", "visible");

    // 行数と列数によって、含まれるdivの高さと幅を計算する
    $(".ChipArea").css("width", gMaxCol * C_CELL_WIDTH + C_CHIPAREA_OFFSET_LEFT + "px");
    $(".ChipArea").css("height", gMaxRow * C_CELL_HEIGHT + C_CHIPAREA_OFFSET_TOP + "px");
    $("#divScrollStall").css("height", gMaxRow * C_CELL_HEIGHT + C_CHIPAREA_OFFSET_TOP + "px");

    // 8行以下の場合、高さを調整する
    if (gMaxRow < 8) {
        var nChipAreaTrimmingHeight = gMaxRow * C_CELL_HEIGHT + C_CHIPAREA_OFFSET_TOP;
        $(".ChipArea_trimming").height(nChipAreaTrimmingHeight);
        $(".SMB_Main").height(nChipAreaTrimmingHeight);
    }

    // 画面スクロールできる
    $(".ChipArea_trimming").SmbFingerScroll({
        mergeLeft: $("#divScrollTime")[0],
        mergeTop: $("#divScrollStall")[0],
        minLeft: $(".ChipArea_trimming").width() - $(".ChipArea").width(),
        minTop: $(".ChipArea_trimming").height() - $(".ChipArea").height()
    });
    $(".ChipArea_trimming .scroll-inner").height(gMaxRow * C_CELL_HEIGHT + C_CHIPAREA_OFFSET_TOP);
    $(".ChipArea_trimming .scroll-inner").width(gMaxCol * C_CELL_WIDTH + C_CHIPAREA_OFFSET_LEFT);
    // 2013/11/29 TMEJ 張 タブレット版SMB チーフテクニシャン機能開発 START
    $("#divScrollStall").css({ "box-shadow": "2px 2px 10px #000" });
    // 2013/11/29 TMEJ 張 タブレット版SMB チーフテクニシャン機能開発 END
    // 中断理由ウィンドウの中断メモのスクロールイベントのbind
    $("#StopMemoScrollBox").SC3240101StopmemoFingerscroll();
    $("#StopMemoScrollBox .scroll-inner").height($("#StopMemoScrollBox .innerDataBox").height());

    // セルにタップするイベント
    BindCellTapEvent();

    // 中断理由ポップアップウィンドウのタップイベントを登録
    BindStopWndEvent();

	// 2013/11/29 TMEJ 張 タブレット版SMB チーフテクニシャン機能開発 START
    // テクニシャンエリアにタップするイベント
    BindTechniacianTapEvent();

    // 顧客詳細クリックイベント
    BindCustomerDetailEvent();
    // 2013/11/29 TMEJ 張 タブレット版SMB チーフテクニシャン機能開発 END

    // ポップアップボックスをスクロールとチップをリサイズする時、テーブルがスクロールできない
    // ポップアップボックスまたはチップから指を離せる時、テーブルのスクロールを再開する
    $("#MainArea").bind(C_TOUCH_END, function (e) {
        if (gStopFingerScrollFlg) {
            $(".ChipArea_trimming").SmbFingerScroll({
                action: "restart"
            });
            gStopFingerScrollFlg = false;
        }
    });
    
    // intervalを再開する(自動スクロールと定期リフレッシュ)
    if (gScrollTimerInterval == "") {
        gScrollTimerInterval = setInterval("SlideTimeLineInOneMinute()", C_INTERVAL_TIME);
    }
    if (gFuncRefreshTimerInterval == "") {
        gFuncRefreshTimerInterval = setInterval("RefreshSMB()", gRefreshTimerInterval * 1000);
    }

	// 2013/11/29 TMEJ 張 タブレット版SMB チーフテクニシャン機能開発 START
    // PullDownコントロールを追加する
    var template$ = $("#pullDownToRefreshTemplate .pullDownToRefresh");
    //重要事項/RSS
    $("#PullDownToRefreshDiv").append(template$);
    // 2013/11/29 TMEJ 張 タブレット版SMB チーフテクニシャン機能開発 END
}

/**
* 非稼働時間を表示する
* @param {String} strStallIdleInfo 非稼働時間情報のJsonString
* @return {なし}
*/
function ShowIdleArea(strStallIdleInfo) {
    // 元の非稼働時間全部削除
    $(".Idle").remove();
    // ストールの赤色をリセットする
    $("#ulStall li").removeClass("WhiteBack");

    var stallIdleList = $.parseJSON(strStallIdleInfo);
    for (var keyString in stallIdleList) {
        switch (stallIdleList[keyString].IDLE_TYPE) {
            case "0":
                // 非稼働日ストール
                SetIdleStallColorWhite(stallIdleList[keyString].STALL_ID);
                break;
            case "1":
                // 休憩時間を描画
                DrawRestArea(stallIdleList[keyString].STALL_IDLE_ID, stallIdleList[keyString].STALL_ID, stallIdleList[keyString].IDLE_START_TIME, stallIdleList[keyString].IDLE_END_TIME);
                break;
            case "2":
                // 使用不可エリアを描画
                //2017/08/30 NSK 竹中(悠) REQ-SVT-TMT-20161109-001 SMB iPadにストールクローズ機能の追加 START
                //DrawUnavailableArea(stallIdleList[keyString].STALL_IDLE_ID, stallIdleList[keyString].STALL_ID, stallIdleList[keyString].IDLE_START_DATETIME, stallIdleList[keyString].IDLE_END_DATETIME, stallIdleList[keyString].ROW_LOCK_VERSION);
                DrawUnavailableArea(stallIdleList[keyString].STALL_IDLE_ID, stallIdleList[keyString].STALL_ID, stallIdleList[keyString].IDLE_START_DATETIME, stallIdleList[keyString].IDLE_END_DATETIME, stallIdleList[keyString].ROW_LOCK_VERSION, stallIdleList[keyString].IDLE_MEMO);
                //2017/08/30 NSK 竹中(悠) REQ-SVT-TMT-20161109-001 SMB iPadにストールクローズ機能の追加 END
            break;
        }
    }
}

/**
* 使用不可エリアを更新する
* @param {String} strStallIdleInfo 非稼働時間情報のJsonString
* @return {なし}
*/
function UpdateIdleArea(strStallIdleInfo) {

    var stallIdleList = $.parseJSON(strStallIdleInfo);
    for (var keyString in stallIdleList) {
        // 選択されているフラグ
        var bSelectedFlg = false;
        switch (stallIdleList[keyString].IDLE_TYPE) {
            case "2":
                // 表示されてる場合削除する
                if ($("#" + C_UNAVALIABLECHIPID + stallIdleList[keyString].STALL_IDLE_ID).length == 1) {
                    // 選択された場合、選択フラグをtrueに設定する
                    if ($("#" + C_UNAVALIABLECHIPID + stallIdleList[keyString].STALL_IDLE_ID).hasClass("SelectedChipShadow")) {
                        bSelectedFlg = true;
                    }
                    // 削除
                    $("#" + C_UNAVALIABLECHIPID + stallIdleList[keyString].STALL_IDLE_ID).remove();
                }

                // 使用不可エリアを再描画
                //2017/08/30 NSK 竹中(悠) REQ-SVT-TMT-20161109-001 SMB iPadにストールクローズ機能の追加 START
                //DrawUnavailableArea(stallIdleList[keyString].STALL_IDLE_ID, stallIdleList[keyString].STALL_ID, stallIdleList[keyString].IDLE_START_DATETIME, stallIdleList[keyString].IDLE_END_DATETIME, stallIdleList[keyString].ROW_LOCK_VERSION);
                DrawUnavailableArea(stallIdleList[keyString].STALL_IDLE_ID, stallIdleList[keyString].STALL_ID, stallIdleList[keyString].IDLE_START_DATETIME, stallIdleList[keyString].IDLE_END_DATETIME, stallIdleList[keyString].ROW_LOCK_VERSION, stallIdleList[keyString].IDLE_MEMO);
                //2017/08/30 NSK 竹中(悠) REQ-SVT-TMT-20161109-001 SMB iPadにストールクローズ機能の追加 END

                // 選択フラグがtrue場合、選択状態用クラスを追加する
                if (bSelectedFlg) {
                    $("#" + C_UNAVALIABLECHIPID + stallIdleList[keyString].STALL_IDLE_ID).addClass("SelectedChipShadow SelectedChipZIndex");
                }
                break;
        }
    }
}

/**
* 休憩時間を描画
* @param {String} strStallIdleId 非稼働ストールID
* @param {String} strStallId ストールID
* @param {String} 休憩開始時間
* @param {String} 休憩終了時間
* @return {なし}
*/
function DrawRestArea(strStallIdleId, strStallId, strStartTime, strEndTime) {
    // 行数目を取得
    var nRowNum = parseInt($("#stallId_" + strStallId)[0].className.substr(7));

    var dtShow = new Date($("#hidShowDate").val());
    var dtStartTime = new Date(strStartTime);
    var dtEndTime = new Date(strEndTime);
    dtStartTime.setFullYear(dtShow.getFullYear());
    dtEndTime.setFullYear(dtShow.getFullYear());
    dtStartTime.setMonth(dtShow.getMonth());
    dtEndTime.setMonth(dtShow.getMonth());
    dtStartTime.setDate(dtShow.getDate());
    dtEndTime.setDate(dtShow.getDate());

    // エリアのleftと幅を計算する
    var nLeft = Math.round(GetXPosByTime(dtStartTime)) + 1;
    var nWidth = Math.round(GetXPosByTime(dtEndTime) - GetXPosByTime(dtStartTime)) - 1;

    var objRest = $("<div />").addClass("RestArea");
    objRest.attr("id", C_RESTCHIPID + strStallIdleId);
    // Rest文言を取得
    objRest.append(gSC3240101WordIni[13]);
    objRest.css("left", nLeft).css("width", nWidth);
    // 表示する
    $(".Row" + nRowNum).append(objRest);
    // 使用不可エリアのイベントを登録する
    BindRestAreaEvent(C_RESTCHIPID + strStallIdleId);
}

//2017/08/30 NSK 竹中(悠) REQ-SVT-TMT-20161109-001 SMB iPadにストールクローズ機能の追加 START
/**
* 使用不可エリアを描画
* @param {String} strStallIdleId 非稼働ストールID
* @param {String} strStallId ストールID
* @param {String} strStartTime 使用不可開始時間
* @param {String} strEndTime 使用不可終了時間
* @param {String} strIdleMemo 非稼働メモ
* @return {なし}
*/
//function DrawUnavailableArea(strStallIdleId, strStallId, strStartTime, strEndTime, nRowLockVersion) {
function DrawUnavailableArea(strStallIdleId, strStallId, strStartTime, strEndTime, nRowLockVersion, strIdleMemo) {
    //2017/08/30 NSK 竹中(悠) REQ-SVT-TMT-20161109-001 SMB iPadにストールクローズ機能の追加 END
    // 日跨ぎ移動チップあるフラグ
    var bOtherFlg = false;
    // 日跨ぎ移動チップが自分のページに戻る時、日跨ぎチップを削除し、自分のチップを選択中にする
    if ($("#" + C_UNAVALIABLECHIPID + strStallIdleId).length == 1) {
        $("#" + C_UNAVALIABLECHIPID + strStallIdleId).remove();
        // 選択したチップを解放する
        SetTableUnSelectedStatus();
        // チップ選択状態を解除する
        SetChipUnSelectedStatus();
        bOtherFlg = true;
    }

    // 行数目を取得
    var nRowNo = parseInt($("#stallId_" + strStallId)[0].className.substr(7));

    // 使用不可チップの開始時間と終了時間を取得する
    var dtStartTime = new Date(strStartTime);
    var dtEndTime = new Date(strEndTime);
    // 当ページの日付を取得する
    var dtShow = new Date($("#hidShowDate").val());

    // 表示用の開始時間と終了時間を取得する(当ページで表示される部分)
    var dtShowStartTime, dtShowEndTime;
    // 開始時間が当ページの営業時間内の場合
    if ((dtStartTime - gStartWorkTime >= 0) && (dtStartTime - gEndWorkTime < 0)) {
        dtShowStartTime = new Date(dtStartTime);
    } else {
        // 他のページにあれば、営業開始時間に設定する
        dtShowStartTime = new Date(gStartWorkTime);
    }

    // 終了時間が当ページの営業時間内の場合、dtShowStartTimeにそのまま設定する
    if ((dtEndTime - gStartWorkTime >= 0) && (dtEndTime - gEndWorkTime < 0)) {
        dtShowEndTime = new Date(dtEndTime);
    } else {
        // 他のページにあれば、営業終了時間に設定する
        dtShowEndTime = new Date(gEndWorkTime);
    }
    // 表示用の幅を計算する
    var nShowWidth = Math.round(GetXPosByTime(dtShowEndTime) - GetXPosByTime(dtShowStartTime)) - 1;

    // 表示用のleftと幅を計算する
    var nShowLeft = Math.round(GetXPosByTime(dtShowStartTime)) + 1;

    // 使用不可チップを生成する
    //2017/08/30 NSK 竹中(悠) REQ-SVT-TMT-20161109-001 SMB iPadにストールクローズ機能の追加 START
    //var objUnvaliable = CreateUnavailableChip(C_UNAVALIABLECHIPID + strStallIdleId);
    var objUnvaliable = CreateUnavailableChip(C_UNAVALIABLECHIPID + strStallIdleId, strIdleMemo);
    //2017/08/30 NSK 竹中(悠) REQ-SVT-TMT-20161109-001 SMB iPadにストールクローズ機能の追加 END

    // 表示用の左位置と幅を設定する
    objUnvaliable.css({ "left": nShowLeft, "width": nShowWidth });

    // 表示する
    $(".Row" + nRowNo).append(objUnvaliable);

    // 使用不可エリアのイベントを登録する
    BindUnavailableAreaEvent(C_UNAVALIABLECHIPID + strStallIdleId);
    $("#" + C_UNAVALIABLECHIPID + strStallIdleId).data("ROW_LOCK_VERSION", nRowLockVersion);

    // 本当の休憩時間(分)を記録する(休憩時間を除く)
    var nIdleTime = GetIdleChipMinutes(dtStartTime, dtEndTime);
    $("#" + C_UNAVALIABLECHIPID + strStallIdleId).data("IDLE_TIME", nIdleTime);
    // 開始時間を記録する
    $("#" + C_UNAVALIABLECHIPID + strStallIdleId).data("START_TIME", strStartTime);
    // 終了時間を記録する
    $("#" + C_UNAVALIABLECHIPID + strStallIdleId).data("END_TIME", strEndTime);

    // 日跨ぎ移動チップあれば、このチップが選択中にする
    if (bOtherFlg) {
        TapUnavailableArea(C_UNAVALIABLECHIPID + strStallIdleId);
        // 左座標を取得する
        var nLeft = Math.round(GetXPosByTime(dtShowStartTime)) + 1;
        // Movingチップを描画する
        drawUnavailableAreaAtPos(nLeft, nRowNo);
        // Movingチップを非表示にする
        $("#" + C_MOVINGUNAVALIABLECHIPID + " .ShadowBox").css("display", "none");
    }
}

/**
* 使用不可エリアの幅の時間を取得
* @param {Date} dtStartTime 開始時間
* @param {Date} dtEndTime 終了時間
* @return {なし}
*/
function GetIdleChipMinutes(dtStartTime, dtEndTime) {

    // 実際の幅を計算する
    var nOffsetdays = GetOffsetDays(dtStartTime, dtEndTime);

    // 3日間以上が日跨ぎの場合、
    // 開始の日の幅
    var nStartTime = 0;
    // 開始の日と終了の日の中の日の幅
    var nMidTime = 0;
    // 終了の日の幅
    var nEndTime = 0;
    // 全て時間
    var nIdleTime = 0;

    // 日跨ぎ場合
    if (nOffsetdays >= 1) {
        // 開始の日の幅(分)
        nStartTime = (gEndWorkTime.getHours() - dtStartTime.getHours()) * 60 + (gEndWorkTime.getMinutes() - dtStartTime.getMinutes());
        if (nStartTime < 0) {
            nStartTime = 0;
        }

        // 終了の日の幅(分)
        nEndTime = (dtEndTime.getHours() - gStartWorkTime.getHours()) * 60 + (dtEndTime.getMinutes() - gStartWorkTime.getMinutes());
        if (nEndTime < 0) {
            nEndTime = 0;
        }

        // 3日間以上が日跨ぎ
        if (nOffsetdays > 1) {
            // 開始の日と終了の日の中の日の時間(分)
            nMidTime = (gEndWorkTime - gStartWorkTime) / 60 / 1000 * (nOffsetdays - 1);
        }
        nIdleTime = nStartTime + nEndTime + nMidTime;
    } else {
        nIdleTime = (dtEndTime - dtStartTime) / 60 / 1000;
    }

    return nIdleTime;
}

//2017/08/30 NSK 竹中(悠) REQ-SVT-TMT-20161109-001 SMB iPadにストールクローズ機能の追加 START
/**
* 使用不可チップを生成する
* @param {String} strStallIdleId チップID
* @param {String} strIdleMemo 非稼働メモ
* @return {object} 使用不可チップのオブジェクト
*/
//function CreateUnavailableChip(strStallIdleId) {
function CreateUnavailableChip(strStallIdleId , strIdleMemo) {
    //2017/08/30 NSK 竹中(悠) REQ-SVT-TMT-20161109-001 SMB iPadにストールクローズ機能の追加 END
    var objUnvaliable = $("<div />").addClass(C_UNAVALIABLECHIPID);
    objUnvaliable.attr("id", strStallIdleId);
    //2017/08/30 NSK 竹中(悠) REQ-SVT-TMT-20161109-001 SMB iPadにストールクローズ機能の追加 START
    //var objShadowBox = $("<div />").addClass("ShadowBox");
    //Unavailable文言を取得
    //objShadowBox.append(gSC3240101WordIni[12]);

    var unavailableChipHtml = ""
    unavailableChipHtml += "<div id=UnavailableUpperText>" + gSC3240101WordIni[12] + "</div> "
    unavailableChipHtml += "<div id=UnavailableLowerText class=UnavailableEllipsis>" + strIdleMemo + "</div> "
    var objShadowBox = "<div class=ShadowBox>" + unavailableChipHtml + "</div>"
    //2017/08/30 NSK 竹中(悠) REQ-SVT-TMT-20161109-001 SMB iPadにストールクローズ機能の追加 END
    objUnvaliable.append(objShadowBox);
    return objUnvaliable;
}
/**
* 休憩エリアのタップイベントを登録する
* @param {String} strStallIdleId チップID
* @return {なし}
*/
function BindRestAreaEvent(strStallIdleId) {
    $("#" + strStallIdleId).bind("chipTap", function (e) {
        if (gSelectedChipId != "") {
            var nRowNo = GetRowNoByChipId(strStallIdleId);
            // 選択したのはも使用不可エリアの場合、休憩エリアに置けない
            if (IsUnavailableArea(gSelectedChipId)) {
                return false;
            } else if (gSelectedChipId == C_OTHERDTCHIPID) {
                // 日跨ぎ移動す時
                if (gOtherDtChipObj.stallIdleId) {
                    //使用不可エリアの場合、休憩エリアに置けない
                    return false;
                } else {
                    //普通のチップの場合、移動できる
                    return;
                }
            } else {
                // 選択したのはチップの場合
                $(".Row" + nRowNo).append($("#" + C_MOVINGCHIPID));
            }
        }
    });
}

/**
* 使用不可エリアのイベントを登録する
* @param {String} strStallIdleId チップID
* @return {なし}
*/
function BindUnavailableAreaEvent(strStallIdleId) {
    $("#" + strStallIdleId).bind("chipTap", function (e) {
        TapUnavailableArea(strStallIdleId);
    }).bind(C_TOUCH_START, function (e) {
        //2017/09/15 NSK 竹中(悠) REQ-SVT-TMT-20161109-001 SMB iPadにストールクローズ機能の追加 START
        var scrollMainObjPos = $(".ChipArea_trimming").find(".scroll-inner").position();
        gTranslateValStallX = scrollMainObjPos.left;
        gTranslateValStallY = scrollMainObjPos.top;
        //2017/09/15 NSK 竹中(悠) REQ-SVT-TMT-20161109-001 SMB iPadにストールクローズ機能の追加 END
        gOnTouchingFlg = true;
        // タップすると青色を表示する
        $("#" + strStallIdleId + " .ShadowBox").stop(true, true).addClass("TapBlueBack");
    })
    .bind(C_TOUCH_MOVE + " " + C_TOUCH_END, function (e) {
        gOnTouchingFlg = false;
        $("#" + strStallIdleId + " .ShadowBox").stop(true, true).removeClass("TapBlueBack");
    });
}
/**
* Movingチップのタップイベントのbind
* @return {なし}
*/
function BindUnavaliableMovingChipTapEvent() {
    $("#" + C_MOVINGUNAVALIABLECHIPID).bind("chipTap", function (e) {
        // ポップアップウィンドウが表示中、何もしない
        if (GetDisplayPopupWindow()) {
            return;
        }

        $("#" + gSelectedChipId + " .ShadowBox").removeClass("TapBlueBack");
        $("#" + C_MOVINGUNAVALIABLECHIPID + " .ShadowBox").removeClass("TapBlueBack");
        setTimeout(function () {
            // Movingチップのタップ関数を走る
            TapMovingUnavaliableChip();
        }, 0);

        // チップにタップすると、 $("#ulChipAreaBack_lineBox .TbRow").bind("chipTap")を走らないように
        gCanTbRowTapFlg = false;
    })
    .bind(C_TOUCH_START, function (e) {
        gOnTouchingFlg = true;
        // Movingチップがピッタリ元のチップの上にある時(非表示の時)
        if ($("#" + C_MOVINGUNAVALIABLECHIPID + " .ShadowBox").css("display") == "none") {
            // 元のチップに青色を表示する
            $("#" + gSelectedChipId + " .ShadowBox").addClass("TapBlueBack");
        } else {
            // Movingチップが表示される時、Movingチップが一瞬で青色を表示する
            $("#" + C_MOVINGUNAVALIABLECHIPID + " .ShadowBox").addClass("TapBlueBack");
        }
    })
    .bind(C_TOUCH_END, function (e) {
        $("#" + gSelectedChipId + " .ShadowBox").removeClass("TapBlueBack");
        $("#" + C_MOVINGUNAVALIABLECHIPID + " .ShadowBox").removeClass("TapBlueBack");
        gOnTouchingFlg = false;
    })
    .bind(C_TOUCH_MOVE, function (e) {
        $("#" + gSelectedChipId + " .ShadowBox").removeClass("TapBlueBack");
        $("#" + C_MOVINGUNAVALIABLECHIPID + " .ShadowBox").removeClass("TapBlueBack");
        gOnTouchingFlg = false;
    });
}
/**
* 移動不可チップにタップ
* @return {なし}
*/
function TapUnavailableArea(strStallIdleId) {
    // 選択したのはチップの場合
    if (gSelectedChipId != "") {
        var nRowNo = GetRowNoByChipId(strStallIdleId);
        // 選択したのはも使用不可エリア、なにもしない
        if (IsUnavailableArea(gSelectedChipId)) {
            return false;
        } else if (gSelectedChipId == C_OTHERDTCHIPID) {
            // 日跨ぎ移動す時
            if (gOtherDtChipObj.stallIdleId) {
                //使用不可エリアの場合、休憩エリアに置けない
                return false;
            } else {
                //普通のチップの場合、移動できる
                return;
            }
        } else {
            // 選択したのは予約チップの場合、行を変える
            if (gArrObjChip[gSelectedChipId]) {
                if (IsDefaultDate(gArrObjChip[gSelectedChipId].rsltStartDateTime)) {
                    $(".Row" + nRowNo).append($("#" + C_MOVINGCHIPID));
                }
            }
        }
        return;
    }

    SetSubChipBoxClose();

    gSelectedChipId = strStallIdleId;

    //遅れストール画面が表示中、もとに戻す
    if (gShowLaterStallFlg == true) {
        ShowAllStall(); //もとに戻す
        // タップしたチップが表示するようにスクロールする
        ScrollToShowChip(strStallIdleId);
    }

    SetTableSelectedStatus();

    //フッターボタンを変える
    CreateFooterButton(C_FT_DISPTP_SELECTED, C_FT_BTNTP_UNAVAILABLE);
}
/**
* 選択しているチップを描画する
* @param {Integer} nLeft 左の位置
* @param {Integer} nRowNo 何行目
* @return {Integer} 0:何もしない　1:新規 2:既存の移動
*/
function drawUnavailableAreaAtPos(nLeft, nRowNo) {
    // 選択チップがない場合、戻す。
    if (gSelectedChipId == "") {
        return 0;
    }

    var strChipId = gSelectedChipId;
    // 使用不可エリアの移動チップがないの場合、新規する
    if ($("#" + C_MOVINGUNAVALIABLECHIPID).length == 0) {
        var objUnavailableArea = $("<div />").addClass(C_UNAVALIABLECHIPID);    //チップの枠
        objUnavailableArea.attr("id", C_MOVINGUNAVALIABLECHIPID);
        var objShadowBox = $("<div />").addClass("ShadowBox");
        // Unavailable
        //2017/08/30 NSK 竹中(悠) REQ-SVT-TMT-20161109-001 SMB iPadにストールクローズ機能の追加 START
        // objShadowBox.append(gSC3240101WordIni[12]);
        
        var nWidth;
        var selectedChipLowerText = "";

        // 新規の場合、MovingChipの幅がC_CELL_WIDTHより小さいなので、MovingChipの幅を元のチップの幅に設定する
        if (gSelectedChipId == C_OTHERDTCHIPID) {
            nWidth = gOtherDtChipObj.width;
            gSelectedChipId = C_UNAVALIABLECHIPID + gOtherDtChipObj.stallIdleId;
            $("#" + gSelectedChipId).data("IDLE_TIME", Math.round((nWidth + 1) * 15 / C_CELL_WIDTH));

            if (gOtherDtChipObj.idleMemo != undefined) {
                selectedChipLowerText = gOtherDtChipObj.idleMemo;
            } else {
                selectedChipLowerText = "";
            }
        } else {
            nWidth = $("#" + gSelectedChipId).width();
            nLeft = $("#" + gSelectedChipId).position().left;
            selectedChipLowerText = document.getElementById(strChipId).getElementsByTagName("div").item(2).textContent; //選択元チップの下段メモ取得

            // 開始時間を記録する
            var dtStartTime = new Date($("#" + gSelectedChipId).data("START_TIME"));
            // 終了時間を記録する
            var dtEndTime = new Date($("#" + gSelectedChipId).data("END_TIME"));
            // 本当の休憩時間(分)を記録する(休憩時間を除く)
            var nIdleTime = GetIdleChipMinutes(dtStartTime, dtEndTime);
            $("#" + gSelectedChipId).data("IDLE_TIME", nIdleTime);
        }
         
        var unavailableChipHtml = ""
        unavailableChipHtml += "<div id=UnavailableUpperText>" + gSC3240101WordIni[12] + "</div> "
        unavailableChipHtml += "<div id=UnavailableLowerText class=UnavailableEllipsis>" + selectedChipLowerText + "</div> "
        var objShadowBox = "<div class=ShadowBox>" + unavailableChipHtml + "</div>"
        //2017/08/30 NSK 竹中(悠) REQ-SVT-TMT-20161109-001 SMB iPadにストールクローズ機能の追加 END

        objUnavailableArea.append(objShadowBox);
        var objTimeKnobPointL, objTimeKnobPointR;   //爪
        objTimeKnobPointL = $("<div />").addClass("TimeKnobPointL TimeKnobPoint_gray");
        objTimeKnobPointR = $("<div />").addClass("TimeKnobPointR TimeKnobPoint_gray");
        objUnavailableArea.append(objTimeKnobPointL);
        objUnavailableArea.append(objTimeKnobPointR);

        //一番前のdiv(タップ用)
        var objFrontFace = $("<div />").addClass("Front");
        objUnavailableArea.append(objFrontFace);

        // チップがストールに移動する
        $(".Row" + nRowNo).append(objUnavailableArea);

        // 新規の場合、MovingChipの幅がC_CELL_WIDTHより小さいなので、MovingChipの幅を元のチップの幅に設定する
//        var nWidth;
//        if (gSelectedChipId == C_OTHERDTCHIPID) {
//            nWidth = gOtherDtChipObj.width;
//            gSelectedChipId = C_UNAVALIABLECHIPID + gOtherDtChipObj.stallIdleId;
//            // 2013/11/29 TMEJ 張 タブレット版SMB チーフテクニシャン機能開発 START
//            $("#" + gSelectedChipId).data("IDLE_TIME", Math.round((nWidth + 1) * 15 / C_CELL_WIDTH));
//            // 2013/11/29 TMEJ 張 タブレット版SMB チーフテクニシャン機能開発 END
//        } else {
//            nWidth = $("#" + gSelectedChipId).width();
//            nLeft = $("#" + gSelectedChipId).position().left;

//            // 開始時間を記録する
//            var dtStartTime = new Date($("#" + gSelectedChipId).data("START_TIME"));
//            // 終了時間を記録する
//            var dtEndTime = new Date($("#" + gSelectedChipId).data("END_TIME"));
//            // 本当の休憩時間(分)を記録する(休憩時間を除く)
//            var nIdleTime = GetIdleChipMinutes(dtStartTime, dtEndTime);
//            $("#" + gSelectedChipId).data("IDLE_TIME", nIdleTime);
//        }
        $("#" + C_MOVINGUNAVALIABLECHIPID).css("width", nWidth);
        $("#" + C_MOVINGUNAVALIABLECHIPID).css("left", nLeft);

        // Movingチップのりサイズをbindする
        var nOverDaysType = GetUnavailableChipOverDaysType(gSelectedChipId);
        // 日跨ぎの場合
        if (nOverDaysType > C_OVERDAYS_NONE) {
            switch (nOverDaysType) {
                // 左端が日跨ぎ   
                case C_OVERDAYS_LEFT:
                    // 左の爪を削除する
                    $("#" + C_MOVINGUNAVALIABLECHIPID + " .TimeKnobPointL").remove();
                    // 右端しかリサイズできない
                    BindChipResize(C_MOVINGUNAVALIABLECHIPID, 0, 2);
                    break;
                // 右端が日跨ぎ    
                case C_OVERDAYS_RIGHT:
                    // 右の爪を削除する
                    $("#" + C_MOVINGUNAVALIABLECHIPID + " .TimeKnobPointR").remove();
                    // 左端しかリサイズできない
                    BindChipResize(C_MOVINGUNAVALIABLECHIPID, 0, 1);
                    break;
                // 両端が日跨ぎ    
                case C_OVERDAYS_BOTH:
                    // 左右の爪を削除する
                    $("#" + C_MOVINGUNAVALIABLECHIPID + " .TimeKnobPointL").remove();
                    $("#" + C_MOVINGUNAVALIABLECHIPID + " .TimeKnobPointR").remove();
                    break;
            }
        } else {
            // Movingチップのりサイズをbindする
            BindChipResize(C_MOVINGUNAVALIABLECHIPID, 0, 0);
        }
        // Movingチップの爪以外の部分が半透明、見えない
        $("#" + C_MOVINGUNAVALIABLECHIPID + " .CpInner").css("opacity", C_OPACITY_TRANSPARENT);
        $("#" + C_MOVINGUNAVALIABLECHIPID + " .CpInner").css("visibility", "hidden");
        // Movingチップのz-indexを追加する
        ChangeChipZIndex(C_MOVINGUNAVALIABLECHIPID, "MovingChipZIndex");
        // Movingチップのタップイベントのbind
        BindUnavaliableMovingChipTapEvent();
        return 1;
    } else {
        // まだ移動してない
        if ($("#" + C_MOVINGUNAVALIABLECHIPID + " .ShadowBox").css("display") == "none") {
            var nChipWidthTime = parseInt($("#" + gSelectedChipId).data("IDLE_TIME"));
            var nChipWidth = C_CELL_WIDTH / 15 * nChipWidthTime - 1;
            $("#" + C_MOVINGUNAVALIABLECHIPID).css("width", nChipWidth);
        }

        $("#" + C_MOVINGUNAVALIABLECHIPID).css("left", nLeft);
        $(".Row" + nRowNo).append($("#" + C_MOVINGUNAVALIABLECHIPID));
        return 2;
    }
}

/**
* Movingチップのタップ
* @return {なし}
*/
function TapMovingUnavaliableChip() {
    // MovingChipのleft、幅を取得する
    var nMovingChipLeft, nMovingChipWidth;
    var strChipId = gSelectedChipId;

    nMovingChipLeft = $("#" + C_MOVINGUNAVALIABLECHIPID).position().left;
    if ($("#" + gSelectedChipId).data("IDLE_TIME")) {
    	nMovingChipWidth = parseInt($("#" + gSelectedChipId).data("IDLE_TIME")) * C_CELL_WIDTH / 15 - 1;
    } else {
        nMovingChipWidth = $("#" + C_MOVINGUNAVALIABLECHIPID).width();
    }

    var nBackLeft, nBackRow, nBackWidth;
    // Movingチップある行目を取得
    var nRow = GetRowNoByChipId(C_MOVINGUNAVALIABLECHIPID);

    // 移動しない場合、選択状態を解除する
    if (gOtherDtChipObj == null) { 
        // バックアップ幅、左位置、行数目
        nBackWidth = $("#" + strChipId).width();
        nBackLeft = $("#" + strChipId).position().left;
        nBackRow = GetRowNoByChipId(strChipId);

        // 移動してない場合
        if ((nMovingChipLeft == nBackLeft)
        && (nMovingChipWidth == nBackWidth)
        && (nRow == nBackRow)) {
            // メインストールチップの場合
            // 選択したチップを解放する
            SetTableUnSelectedStatus();
            // チップ選択状態を解除する
            SetChipUnSelectedStatus();
            return;
        }
    }

    // 置いた位置をチェックする
    if (CheckChipPos(C_MOVINGUNAVALIABLECHIPID) == false) {
        //エラーメッセージ「使用不可チップを他のチップの上に重複させることができません。」を表示する
        ShowSC3240101Msg(914);
        $("#" + C_MOVINGUNAVALIABLECHIPID).remove();
        // 選択したチップを解放する
        SetTableUnSelectedStatus();
        // チップ選択状態を解除する
        SetChipUnSelectedStatus();

        return;
    }

    //2017/08/30 NSK 竹中(悠) REQ-SVT-TMT-20161109-001 SMB iPadにストールクローズ機能の追加 START
//    // 移動先が使用不可、休憩エリアがあるかどうかを判断する
//    var arrRestTime = GetRestTimeInServiceTime(C_MOVINGUNAVALIABLECHIPID);
//    // 重複休憩エリアがあれば、
//    if ((arrRestTime.length > 1) || ((arrRestTime.length == 1) && (arrRestTime[0][0] != strChipId))) {
//        //エラーメッセージ「使用不可チップを他のチップの上に重複させることができません。」を表示する
//        ShowSC3240101Msg(914);
//        $("#" + C_MOVINGUNAVALIABLECHIPID).remove();
//        // 選択したチップを解放する
//        SetTableUnSelectedStatus();
//        // チップ選択状態を解除する
//        SetChipUnSelectedStatus();
//        return;
//    }
    //2017/08/30 NSK 竹中(悠) REQ-SVT-TMT-20161109-001 SMB iPadにストールクローズ機能の追加 END

    var nRowLockVersion;

    // 別の日のチップの場合
    if (gOtherDtChipObj) {
        strChipId = C_UNAVALIABLECHIPID + gOtherDtChipObj.stallIdleId;
        // 普通のチップの大きさに戻す 
        $("#" + strChipId).css("visibility", "visible");
        $("#" + strChipId).css("top", "1px");
        $("#" + strChipId).css("height", "72px");
        // 非透明にする
        $("#" + strChipId).css("opacity", C_OPACITY);
        // IDLE_TIMEがない場合、
        if (($("#" + strChipId).data("IDLE_TIME") == undefined) 
            || ($("#" + strChipId).data("IDLE_TIME") == null)) {
            $("#" + strChipId).data("IDLE_TIME", 15 / C_CELL_WIDTH * (gOtherDtChipObj.width + 1));
        }
        // 日跨ぎ移動使用不可チップの場合
        nRowLockVersion = gOtherDtChipObj.rowLockVersion;
        // 別の日のチップをクリアする
        gOtherDtChipObj = null;
        // 元のchiptapイベントをunbindする
        $("#" + strChipId).unbind();
        // タップイベントをbindする
        BindUnavailableAreaEvent(strChipId);
    } else {
        nRowLockVersion = $("#" + strChipId).data("ROW_LOCK_VERSION");
    }

    // 移動元のちップを移動先へ移動する
    $("#" + strChipId).css("left", nMovingChipLeft);
    $("#" + strChipId).css("width", nMovingChipWidth);
    $(".Row" + nRow).append($("#" + strChipId));

    // 横座標で時間を取得する
    var tStartTime = GetTimeByXPos(nMovingChipLeft - 1);

    // 左側日跨ぎの場合、それで、ただリサイズの操作をして、開始時間が変わらない
    if ($("#" + C_MOVINGUNAVALIABLECHIPID + " .ui-resizable-w").length == 0) {
        tStartTime = new Date($("#" + strChipId).data("START_TIME"));
    }

    // テーブルの状態をチップの未選択状態に設定する
    SetTableUnSelectedStatus();
    // チップ選択状態を解除する
    SetChipUnSelectedStatus();

    // 休憩時間
    var nIdleTime = parseInt($("#" + strChipId).data("IDLE_TIME"));

    var strStallId = $(".stallNo" + nRow)[0].id;
    var jsonData = {
        Method: "UpdateStallUnavailable",
        StallIdleId: right(strChipId, strChipId.length - C_UNAVALIABLECHIPID.length),
        StallId: right(strStallId, strStallId.length - 8),
        ShowDate: $("#hidShowDate").val(),
        DisplayStartDate: tStartTime,
        ScheWorkTime: nIdleTime,
        RowLockVersion: nRowLockVersion
    };
    // 行ロックバージョンを1に加える
    $("#" + strChipId).data("ROW_LOCK_VERSION", parseInt(nRowLockVersion) + 1);
    DoCallBack(C_CALLBACK_WND101, jsonData, SC3240101AfterCallBack, jsonData.Method);
}
/**
* 使用不可エリアかどうか判断
* @param {String} strChipId チップID
* @return {Bool} true:使用不可エリア
*/
function IsUnavailableArea(strChipId) {

    if (strChipId === undefined) {
        return false;
    }

    if ((strChipId.toString().indexOf(C_UNAVALIABLECHIPID) > -1)
        && (strChipId != C_MOVINGUNAVALIABLECHIPID)) {
        return true;
    }
    return false;
}
/**
* 仮仮チップかどうか判断
* @param {String} strChipId チップID
* @return {Bool} true:仮仮チップ
*/
function IsKariKariChip(strChipId) {

    if (strChipId === undefined) {
        return false;
    }

    if (strChipId.toString().indexOf(C_KARIKARICHIPID) > -1) {
        return true;
    }
    return false;
}
/**
* 休憩エリアかどうか判断
* @param {String} strChipId チップID
* @return {Bool} true:使用不可エリア
*/
function IsRestArea(strChipId) {

    if (strChipId === undefined) {
        return false;
    }

    if (strChipId.toString().indexOf(C_RESTCHIPID) > -1) {
        return true;
    }
    return false;
}
/**
* テクニシャンエリアを更新する
* @return {なし}
*/
function AdjuestTechnicianNameArea() {

    var strHtml;
    // テクニクエリアの高さを取得する
    var nHeight = $(".stallNo1 .Technician").height();
    // ディフォルトが4名である
    var nCount = 4;

    // 行でループする
    for (var nRow = 1; nRow <= gMaxRow; nRow++) {
        // 最大テクニク数でループする
        for (var nLoop = 1; nLoop <= 4; nLoop++) {
            // テクニクは何名を取得する
            strHtml = document.getElementById("spanTechnician" + nRow + "_" + nLoop).innerHTML;
            if (strHtml.trim() == "") {
                nCount = nLoop - 1;
                break;
            }
        }
        // 高さを調整する
        if (nCount == 1) {
            $(".stallNo" + nRow + " .Technician .TechnicianName").css("line-height", "" + nHeight / nCount + "px");
            $(".stallNo" + nRow + " .Technician .TechnicianName:eq(0)").css({ "position": "", "top": "" });
            $(".stallNo" + nRow + " .Technician .TechnicianName:eq(2)").css({ "position": "", "top": "" });
        }
        else if (nCount == 3) {
            $(".stallNo" + nRow + " .Technician .TechnicianName").css("line-height", "" + nHeight / nCount + "px");
            $(".stallNo" + nRow + " .Technician .TechnicianName:eq(0)").css({ "position": "relative", "top": "7px" });
            $(".stallNo" + nRow + " .Technician .TechnicianName:eq(2)").css({ "position": "relative", "top": "-7px" });
        } else {
            $(".stallNo" + nRow + " .Technician .TechnicianName").css("line-height", "" + nHeight / 4 + "px");
            $(".stallNo" + nRow + " .Technician .TechnicianName:eq(0)").css({ "position": "", "top": "" });
            $(".stallNo" + nRow + " .Technician .TechnicianName:eq(2)").css({ "position": "", "top": "" });
        }
        nCount = 4;
    }
}

/**
* ストールエリアを生成する
* @param {String} strStallContents ストールのデータのstring
* @return {なし}
*/
function UpdateStallArea(strStallContents) {
    // グロバール変数を初期化
    gArrObjStall.length = 0;
    // jsonデータのstringをリストに変える
    var listStall = $.parseJSON(strStallContents);
    // 全部ループする
    for (var keyString in listStall) {
        // データをグロバール変数に保存する
        gArrObjStall.push(listStall[keyString]);
    }

    // 全行ループする(ページを変えて、行数が変わらない)
    for (var nLoop = 1; nLoop <= gMaxRow; nLoop++) {
        // ストールidを取得する
        var strStallId = $(".stallNo" + nLoop)[0].id;
        // チップのidと分けるため、ストールidはstallId_1、1はdbからストールのid
        // stallId_を削除する
        strStallId = strStallId.substring(8, strStallId.length);
        // 1つストールのテクニシャンを取得する
        var arrStaffNames = GetTechnicianNumsByStallId(strStallId);
        // 1つストールのテクニシャンを更新する
        UpdateTechnicianNameAreaByStallNo(nLoop, arrStaffNames);
    }
    // テクニシャンエリアの行距を調整する
    AdjuestTechnicianNameArea();

    //spanタグにCustomLabelを適用する(これによって...をタップするとツールチップ表示)
    $("#ulStall .Technician span").CustomLabel({ useEllipsis: true });
}
/**
* ストールidにより、テクニシャンの人数を取得する
* @param {String} strStallId ストールID
* @return {Array} 1つストールのテクニシャン
*/
function GetTechnicianNumsByStallId(strStallId) {
    var arrStaffNames = new Array();

    // gArrObjStallでループする
    for (var nLoop = 0; nLoop < gArrObjStall.length; nLoop++) {
        if (gArrObjStall[nLoop].STALLID == strStallId) {
            arrStaffNames.push(gArrObjStall[nLoop].USERNAME);
        }
    }
    return arrStaffNames;
}
/**
* ストールidにより、テクニシャンの人数を取得する
* @param {Integer} nRowNo 行数目
* @param {String} arrStaffNames 1つストールのテクニシャンの名前
* @return {なし}
*/
function UpdateTechnicianNameAreaByStallNo(nRowNo, arrStaffNames) {

    // nRowNo行目のテクニシャン全部クリアする
    $("#spanTechnician" + nRowNo + "_1").text("");
    $("#spanTechnician" + nRowNo + "_2").text("");
    $("#spanTechnician" + nRowNo + "_3").text("");
    $("#spanTechnician" + nRowNo + "_4").text("");
    // 右寄せを解除する
    $("#spanTechnician" + nRowNo + "_4").removeClass("TextAlignRight");
    // 値が無い場合、戻す
    if (arrStaffNames == "") {
        return;
    }
    // 名前の順にソートする
    arrStaffNames.sort();
    // テクニシャンが二人の場合、真ん中で表示するために、中の2行に設定する
    if (arrStaffNames.length == 2) {
        $("#spanTechnician" + nRowNo + "_1").text("　");
        $("#spanTechnician" + nRowNo + "_2").text(arrStaffNames[0], 7);
        $("#spanTechnician" + nRowNo + "_3").text(arrStaffNames[1], 7);
        $("#spanTechnician" + nRowNo + "_4").text("　");
    } else {
        // 4名以下の場合、順に設定する
        if (arrStaffNames.length <= 4) {
            for (var nLoop = 1; nLoop <= arrStaffNames.length; nLoop++) {
                $("#spanTechnician" + nRowNo + "_" + nLoop).text(arrStaffNames[nLoop - 1], 7);
            }
        } else {
            // 前の3名がそのままで
            for (var nLoop = 1; nLoop <= 3; nLoop++) {
                $("#spanTechnician" + nRowNo + "_" + nLoop).text(arrStaffNames[nLoop - 1], 7);
            }
            // "他{0}名"を取得する
            var strString = gSC3240101WordIni[2];
            strString = strString.replace("{0}", arrStaffNames.length - 3);
            // 4行目は他N名と表示される
            $("#spanTechnician" + nRowNo + "_4").text(strString);
            // 右寄せ
            $("#spanTechnician" + nRowNo + "_4").addClass("TextAlignRight");
        }
    }
}
/**
* 遅刻ストールを赤に設定する
* @return {なし}
*/
function SetLaterStallColorRed() {
    var arrDelayStallId = new Array();
    var nDelayChipNums = 0;
    // すべてチップをループする
    for (var nChipNo in gArrObjChip) {
        // 有効のチップデータをチェックする
        if (CheckgArrObjChip(nChipNo) == false) {
            continue;
        }
        // チップ色とdelayStatusを更新する
        gArrObjChip[nChipNo].refleshChipRedColor();
        // 遅刻の場合
        if (gArrObjChip[nChipNo].delayStatus > C_NO_DELAY) {
            // 重複の値を削除する
            arrDelayStallId = $.grep(arrDelayStallId, function (n, i) {
                return n == gArrObjChip[nChipNo].stallId;
            }, true);
            // 遅刻チップがあるストールのidを記録する
            arrDelayStallId.push(gArrObjChip[nChipNo].stallId);
            nDelayChipNums++;
        }
    }

    // ストールの赤色をリセットする
    $("#ulStall li").removeClass("RedBack");

    // 遅刻ストールの色を赤に設定する
    for (var nStallIdLoop in arrDelayStallId) {
        $("#stallId_" + arrDelayStallId[nStallIdLoop]).addClass("RedBack");
    }

    // 遅れボタンの色を赤に設定する
    if (arrDelayStallId.length > 0) {

        SetFooter(C_FT_BTNID_LATER, C_FT_BTNCLR_RED, C_FT_BTNDISP_OFF, nDelayChipNums);
        // 遅れボタンの場合、空白にする
        if (bkButtonID == C_FT_BTNID_LATER) {
            bkButtonID = "";
        }
        // 遅れストールが絞り込んだ時、ボタンを押下状態にする
        if (gShowLaterStallFlg) {
            if (bkButtonID == "") {
                // ボタンを押下状態にする
                FooterIconReplace(C_FT_BTNID_LATER);
            }
        }
    } else {
        SetFooter(C_FT_BTNID_LATER, "", C_FT_BTNDISP_OFF, "");
    }
}

/**
* 非稼働時間ストールを赤に設定する
* @return {なし}
*/
function SetIdleStallColorWhite(strStallId) {
    $("#stallId_" + strStallId).addClass("WhiteBack");
}

/**
* 遅れストールだけ表示される
* @return {なし}
*/
function ShowLaterStall() {
    // 遅れストール画面が表示中、もとに戻す
    if (gShowLaterStallFlg == true) {
        // もとに戻す
        ShowAllStall();
    } else {
        gShowLaterStallFlg = true;
        // 一番上の行へスクロールする
        $(".ChipArea_trimming").SmbFingerScroll({
            action: "move",
            moveY: $(".scroll-inner").position().top,
            moveX: $(".scroll-inner").position().left
        });

        var nLaterRowNum = 0;
        // 全て行をループする
        for (var nLoop = 1; nLoop <= gMaxRow; nLoop++) {
            // 赤いストール以外の場合、非表示する
            if ($(".stallNo" + nLoop).hasClass("RedBack") == false) {
                $(".stallNo" + nLoop).css("visibility", "hidden");
                $(".Row" + nLoop).css("visibility", "hidden");
            } else {
                // 下の行が上へ移動する
                $(".Row" + nLoop).css("top", nLaterRowNum * C_CELL_HEIGHT);
                $(".stallNo" + nLoop).css("top", nLaterRowNum * C_CELL_HEIGHT);

                // 2013/11/29 TMEJ 張 タブレット版SMB チーフテクニシャン機能開発 START
                // 今の行数を記録する(テクニシャン表示の三角用)
                $(".stallNo" + nLoop).data("LATER_ROWNUM", nLaterRowNum + 1);
                // 2013/11/29 TMEJ 張 タブレット版SMB チーフテクニシャン機能開発 END

                nLaterRowNum++;
            }
        }
        $("#ulStall li").css("position", "absolute");

        // 表示される画面が8行以下の場合
        if (nLaterRowNum < 8) {
            // 高さを再計算する
            var nTableHeight = nLaterRowNum * C_CELL_HEIGHT + C_CHIPAREA_OFFSET_TOP;
            $(".SMB_Main").css("height", nTableHeight);
            // 画面のテーブルの高さにしたがって、時刻線の高さを設定する
            SetTimeLineHeight(nTableHeight);

            // 表示される画面が8行以下の場合、縦方向でスクロール禁止
            $(".ChipArea_trimming").SmbFingerScroll({
                minTop: 0
            });
 
        } else {
            // 縦方向でスクロールできる範囲を設定する
            $(".ChipArea_trimming").SmbFingerScroll({
                minTop:  $(".ChipArea_trimming").height() - nLaterRowNum * C_CELL_HEIGHT - $(".TimeLine").height()
            });
        }

    }
}
/**
* 全てストールだけ表示される
* @return {なし}
*/
function ShowAllStall() {
    gShowLaterStallFlg = false;
    // テーブルに全て行を表示する
    $(".TbRow").css("visibility", "visible");
    $("#ulStall li").css("visibility", "visible");
    
    // 全て行の高さをリセット
    var nTop = 0;
    for (var nRow = 1; nRow <= gMaxRow; nRow++) {
        $(".Row" + nRow).css("top", nTop);
        $(".stallNo" + nRow).css("top", nTop);
        nTop += C_CELL_HEIGHT;
    }

    // 高さを再計算する
    if (gMaxRow < 8) {
        $(".SMB_Main").css("height", gMaxRow * C_CELL_HEIGHT + C_CHIPAREA_OFFSET_TOP);
    } else {
        $(".SMB_Main").css("height", 8 * C_CELL_HEIGHT + C_CHIPAREA_OFFSET_TOP);
    }
    
    // 画面のテーブルの高さにしたがって、時刻線の高さを設定する
    var nHeight = $(".TimeLine").height();
    SetTimeLineHeight(gMaxRow * C_CELL_HEIGHT + nHeight);

    // 縦方向でスクロール再開して、1行目1列目から表示する
    $(".ChipArea_trimming").SmbFingerScroll({
        minTop: $(".ChipArea_trimming").height() - $(".ChipArea").height(),
        action: "move", moveY: $(".scroll-inner").position().top, moveX: $(".scroll-inner").position().left
    });

    // 遅れボタンの状態を設定する
    var nCount = GetButtonIconCount(C_FT_BTNID_LATER);

    // 遅れ件があれば
    if (nCount > 0) {
        // 赤
        SetFooter(C_FT_BTNID_LATER, C_FT_BTNCLR_RED, C_FT_BTNDISP_OFF, nCount);
    } else {
        SetFooter(C_FT_BTNID_LATER, "", C_FT_BTNDISP_OFF, nCount);
    }

    // 遅れボタンの押下フラグをクリア
    if (bkButtonID == C_FT_BTNID_LATER) {
        bkButtonID = "";
    }
}
/**
* 選択状態を解除する
* @return {なし}
*/
function CancelSelectedStatus() {

    // あるチップが選択されている場合、
    if (gSelectedChipId != "") {
        if (!gSelectedChipId) {
            return;
        }
        var strSelectedId = gSelectedChipId;
        // チップを選択している状態を解除する
        SetTableUnSelectedStatus();
        // チップ選択状態を解除する
        SetChipUnSelectedStatus();
        var strSubChipId = GetRelationSubChipId(strSelectedId)
        if (strSubChipId != "") {
            strSelectedId = strSubChipId;
        }
        if (gArrObjSubChip[strSelectedId]) {
            FooterIconReplace(gArrObjSubChip[strSelectedId].subChipAreaId);
            if ((gArrObjSubChip[strSelectedId].subChipAreaId != C_RECEPTION)&&(gArrObjSubChip[strSelectedId].subChipAreaId != C_NOSHOW)&&(gArrObjSubChip[strSelectedId].subChipAreaId != C_STOP)) {
                // チップ選択状態を解除する
                SetChipUnSelectedStatus();
                // 選択したチップを解放する
                SetTableUnSelectedStatus();
                // サブチップボックス閉じる
                SetSubChipBoxClose();
                CreateFooterButton(C_FT_DISPTP_UNSELECTED, 0);
                // フッター部にクリックイベントを0.5秒で無効する
                gCancelFlg = true;
                setTimeout(function () { gCancelFlg = false; }, 500);
                return;
            }
        }
        // フッター部にクリックイベントを0.5秒で無効する
        gCancelFlg = true;
        setTimeout(function () { gCancelFlg = false; }, 500);
    } else {
        // サブチップボックス閉じる
        SetSubChipBoxClose();
        CreateFooterButton(C_FT_DISPTP_UNSELECTED, 0);
    }
}

/**
* ストールタップ時のイベントを登録
* @return {なし}
*/
function BindStallAreaClickEvent() {
    // すべてストールをループする
    for (var nLoop = 1; nLoop <= gMaxRow; nLoop++) {
        // ストールのコントロールのidを作成する
        var strStallNameID = ".stallNo";
        strStallNameID += nLoop;
        // イベントの登録
        $(strStallNameID).bind("click touchstart", function (e) {

            // e.targetはストール中のコントロールなので、含まれるコントロールを取得する
            var objSelectedElement = e.target;
            var objChildNode = objSelectedElement;
            var objParentNode = objSelectedElement.parentNode;

            var nTimeOut = 0;
            // タッチしたstallのidを取得する
            while (objParentNode.tagName != "LI") {

                // 無限ループを防止する
                if (nTimeOut > 10) {
                    return;
                }
                objChildNode = objParentNode;
                objParentNode = objChildNode.parentNode;
                nTimeOut++;
            }

            // タッチしたストールにSelectedStoleクラスを追加する
            var strSelectedStallId = objParentNode.id;
            strSelectedStallId = "." + strSelectedStallId + " ." + objChildNode.className;
            $(strSelectedStallId).addClass("SelectedStole");

            //チップ選択解除イベント発行
            $(document.body).bind("mousedown touchend", function (e) {
                $(strSelectedStallId).removeClass("SelectedStole");
            });
        });
    }
}

/**
* 画面全体にタップイベントを登録する
* @return {なし}
*/
function BindAllAreaClickEvent() {
    // イベントの登録
    // 2013/11/29 TMEJ 張 タブレット版SMB チーフテクニシャン機能開発 START
    //    $("#bodyFrame").bind(C_TOUCH_START, function (e) {
    // C_TOUCH_STARTでBINDしたら、一回画面にタップして、２回このイベントに入る
    // bodyFrameが基盤でBINDイベントがもうあるので、その代わり、this_formを使う
    $("#this_form").unbind("touchstart");
    $("#this_form").bind("touchstart", function (e) {
        // 2013/11/29 TMEJ 張 タブレット版SMB チーフテクニシャン機能開発 END
        var nDispWindow = GetDisplayPopupWindow();
        if (nDispWindow == C_DISP_DETAIL) {
            //タップ領域がチップ詳細の領域内・オーバーレイ・ラベルのツールチップ以外の場合
            if ($(event.target).is("#ChipDetailPopup, #ChipDetailPopup *, #MstPG_registOverlayBlack, .icrop-CustomLabel-tooltip") === false) {
                //チップ詳細を閉じる
                CloseChipDetail();
                // フッター部にクリックイベントを0.5秒で無効する
                gCancelFlg = true;
                setTimeout(function () { gCancelFlg = false; }, 500);
                // 2013/11/29 TMEJ 張 タブレット版SMB チーフテクニシャン機能開発 START
                // IPADが2回この関数に入るので、2回めを止める
                return false;
                // 2013/11/29 TMEJ 張 タブレット版SMB チーフテクニシャン機能開発 END
            }
        } else if (nDispWindow == C_DISP_STOP) {
            if ($(event.target).is(".popStopWindowBase, .popStopWindowBase *, #MstPG_registOverlayBlack, .icrop-CustomLabel-tooltip") === false) {
                //中断理由ウィンドウを閉じる
                CancelStopWindow();
                // 2013/11/29 TMEJ 張 タブレット版SMB チーフテクニシャン機能開発 START
                return false;
                // 2013/11/29 TMEJ 張 タブレット版SMB チーフテクニシャン機能開発 END
            }
        } else if (nDispWindow == C_DISP_REST) {
            if ($(event.target).is(".popRestWindow, .popRestWindow *, #MstPG_registOverlayBlack, .icrop-CustomLabel-tooltip") === false) {
                //休憩ウィンドウを閉じる
                CancelRestWindow();
                // 2013/11/29 TMEJ 張 タブレット版SMB チーフテクニシャン機能開発 START
                return false;
                // 2013/11/29 TMEJ 張 タブレット版SMB チーフテクニシャン機能開発 END
            }
            // 2013/11/29 TMEJ 張 タブレット版SMB チーフテクニシャン機能開発 START
        } else if (nDispWindow == C_DISP_TECH) {
            if ($(event.target).is(".popTechnicianWindow, .popTechnicianWindow *, #MstPG_registOverlayBlack, .icrop-CustomLabel-tooltip, .#divScrollStall .Technician,.#divScrollStall .Technician * ") === false) {
                //休憩ウィンドウを閉じる
                CancelTechnicianWindow();
                return false;
            }
        } else if (nDispWindow == C_DISP_NEW) {
            if ($(event.target).is("#NewChipPopup, #NewChipPopup *, #MstPG_registOverlayBlack, .icrop-CustomLabel-tooltip") === false) {
                //チップ新規作成を閉じる
                CloseNewChip();
                // フッター部にクリックイベントを0.5秒で無効する
                gCancelFlg = true;
                setTimeout(function () { gCancelFlg = false; }, 500);
                return false;
            }
            // 2013/11/29 TMEJ 張 タブレット版SMB チーフテクニシャン機能開発 END
        } else if (nDispWindow == C_DISP_DUPL) {
            if ($(event.target).is(".PopUpChipBoxBorder, .PopUpChipBoxBorder *, #MstPG_registOverlayBlack, .icrop-CustomLabel-tooltip") === false) {
                // 2013/11/29 TMEJ 張 タブレット版SMB チーフテクニシャン機能開発 START
                //                if (gCanRemoveDuplWndFlg) {
                // 2013/11/29 TMEJ 張 タブレット版SMB チーフテクニシャン機能開発 END
                //重複ウィンドウを閉じる
                RemovePopUpBox();
                // 2013/11/29 TMEJ 張 タブレット版SMB チーフテクニシャン機能開発 START
                //                }
                // 2013/11/29 TMEJ 張 タブレット版SMB チーフテクニシャン機能開発 END
            }
            //2017/09/01 NSK 竹中(悠) REQ-SVT-TMT-20161109-001 SMB iPadにストールクローズ機能の追加 START
        }else if (nDispWindow == C_DISP_UNAVAILABLE) {
                
                if ($(event.target).is("#UnavailableSettingPopup, #UnavailableSettingPopup *, #MstPG_registOverlayBlack, .icrop-CustomLabel-tooltip") === false) {
                    //ストール使用不可画面を閉じる
                    CloseUnavailableSetting();
                    // フッター部にクリックイベントを0.5秒で無効する
                    gCancelFlg = true;
                    setTimeout(function () { gCancelFlg = false; }, 500);
                    return false;
                }
                //2017/09/01 NSK 竹中(悠) REQ-SVT-TMT-20161109-001 SMB iPadにストールクローズ機能の追加 END
        } else {
            // 2013/11/29 TMEJ 張 タブレット版SMB チーフテクニシャン機能開発 START
            // チップ新規、詳細ウィンドウ、サブエリア、時刻線、フッターボタン、ダミ画面
            //if ($(event.target).is(".PopUpChipBoxBorder, .PopUpChipBoxBorder *, .popRestWindow, .popRestWindow *,.popStopWindowBase, .popStopWindowBase *,#ChipDetailPopup, #ChipDetailPopup, #MstPG_registOverlayBlack, .icrop-CustomLabel-tooltip, #SubArea , #SubArea *, .TimingLineSet, .TimingLineSet *, .ChipArea_trimming, .ChipArea_trimming *, #DummyChipDetailPopup, #DummyChipDetailPopup *, .LeftButton_trimming, .LeftButton_trimming *, .RightButton_trimming, .RightButton_trimming *, .FooterButton, .FooterButton *, #BtnDummyRO, .Front, #MovingChip, #MovingChip *, .Date, .Date *") === false) {
            if ($(event.target).is(".PopUpChipBoxBorder, .PopUpChipBoxBorder *, .popRestWindow, .popRestWindow *,.popStopWindowBase, .popStopWindowBase *,#ChipDetailPopup, #ChipDetailPopup, #MstPG_registOverlayBlack, .icrop-CustomLabel-tooltip, #SubArea , #SubArea *, .TimingLineSet, .TimingLineSet *, .ChipArea_trimming, .ChipArea_trimming *, #DummyChipDetailPopup, #DummyChipDetailPopup *, .LeftButton_trimming, .LeftButton_trimming *, .RightButton_trimming, .RightButton_trimming *, .FooterButton, .FooterButton *, #BtnDummyRO, .Front, #MovingChip, #MovingChip *, #NewChipPopup, #NewChipPopup *, .Date, .Date *") === false) {
                // 2013/11/29 TMEJ 張 タブレット版SMB チーフテクニシャン機能開発 END
                // チップ選択時、フッター部にタップすれば、基盤のボタンタップイベントを止めるために
                if (gSelectedChipId) {
                    // タップした縦の位置を取得
                    var tapYPos = getTapYPosition();
                    // タップした位置がフッター部の場合
                    if (tapYPos >= 748 - $("#foot").height() - 1) {
                        e.preventDefault();
                    }
                }

                // 選択状態を解除する
                CancelSelectedStatus();
                // フッター部にクリックイベントを0.5秒で無効する
                gCancelFlg = true;
                setTimeout(function () { gCancelFlg = false; }, 500);
            }
        }
        // 2013/11/29 TMEJ 張 タブレット版SMB チーフテクニシャン機能開発 START
        //    });
    })
    .bind("gesturestart", function (e) {
        if ($(".scroll-inner").position().top <= 0) {
            gStopFingerScrollFlg = true;
            $(".ChipArea_trimming").SmbFingerScroll({
                action: "stop"
            });
        }
    })
    .bind("estureend", function (e) {
        if (gStopFingerScrollFlg) {
            $(".ChipArea_trimming").SmbFingerScroll({
                action: "restart"
            });
            gStopFingerScrollFlg = false;
        }
    });
    // 2013/11/29 TMEJ 張 タブレット版SMB チーフテクニシャン機能開発 END
}

/**
* ポップアップウィンドウ(新規、詳細)が表示中かどうか
* @return {Interger} C_DISP_NONE: 表示中ウィンドウが無い C_DISP_DETAIL:詳細画面 C_DISP_NEW:新規画面 C_DISP_UNAVAILABLE:ストール使用不可画面
*/
function GetDisplayPopupWindow() {
    var nRtValue = C_DISP_NONE;
    // 詳細ウィンドウが表示中
    if ($("#ChipDetailPopup").css("display") != "none") {
        nRtValue = C_DISP_DETAIL;
    }

    // 中断理由ウィンドウが表示中
    if ($(".popStopWindowBase").css("display") != "none") {
        nRtValue = C_DISP_STOP;
    }

    // 休憩ウィンドウが表示中
    if ($(".popRestWindow").css("display") != "none") {
        nRtValue = C_DISP_REST;
    }

    // 2013/11/29 TMEJ 張 タブレット版SMB チーフテクニシャン機能開発 START
    // テクニシャンウィンドウが表示中
    if ($(".popTechnicianWindow").css("display") != "none") {
        nRtValue = C_DISP_TECH;
    }

    // 新規ウィンドウが表示中
    if ($("#NewChipPopup").css("display") != "none") {
        nRtValue = C_DISP_NEW;
    }
    // 2013/11/29 TMEJ 張 タブレット版SMB チーフテクニシャン機能開発 END

    // 重複ポップアップウィンドウが表示中
    if ($(".PopUpChipBoxBorder").length == 1) {
        nRtValue = C_DISP_DUPL;
    }
    //2017/09/01 NSK 竹中(悠) REQ-SVT-TMT-20161109-001 SMB iPadにストールクローズ機能の追加 START
    // ストール使用不可画面が表示中
    if ($("#UnavailableSettingPopup").css("display") != "none") {
        nRtValue = C_DISP_UNAVAILABLE;
    }
    //2017/09/01 NSK 竹中(悠) REQ-SVT-TMT-20161109-001 SMB iPadにストールクローズ機能の追加 END
    return nRtValue;
}

// 2013/11/29 TMEJ 張 タブレット版SMB チーフテクニシャン機能開発 START
/**
* テクニシャンエリアにタップするイベント
* @return {なし}
*/
function BindTechniacianTapEvent() {

    var nTouchXPos;
    // チップ選択解除イベント発行
    $("#divScrollStall .Technician").bind("chipTap", function (e) {

        // 選択状態を解除する
        SetChipUnSelectedStatus();
        SetTableUnSelectedStatus();

        // サブエリアを閉じる
        SetSubChipBoxClose();

        // タップした行目を取得する
        var nRowNo = parseInt(e.target.parentNode.className.substr(7));;
        var nLaterRowNo = 0;
        // 遅れチップだけ表示される場合、LATER_ROWNUMで取得する
        if (gShowLaterStallFlg) {
            nLaterRowNo = $("#" + e.target.parentNode.id).data("LATER_ROWNUM");
        } 

        // テクニシャン選択ダイアログボックスが表示される
        ShowTechnicianDialog(nRowNo, nLaterRowNo);

        // 渡す引数
        jsonData = {
            Method: "GetTechnicians"
        };
        //コールバック開始
        DoCallBack(C_CALLBACK_WND101, jsonData, SC3240101AfterCallBack, jsonData.Method);
    }); 
}
// 2013/11/29 TMEJ 張 タブレット版SMB チーフテクニシャン機能開発 END

/**
* セルにタップするイベント
* @return {なし}
*/
function BindCellTapEvent() {

    var nTouchXPos;
    // チップ選択解除イベント発行
    $("#ulChipAreaBack_lineBox .TbRow").bind("chipTap", function (e) {

        // ポップアップウィンドウが表示中、何もしない
        if (GetDisplayPopupWindow()) {
            return;
        }
        //サブチップが処理中の場合、何もしない
        if ($("#SubChip_LoadingScreen").css("display") === 'block') {
            return;
        }
        // 今あるチップが選択中の場合、選択した位置に移動できる
        if (gSelectedChipId != "") {

            // タップしたクラス名は
            // チップの場合：「MCp …」の形式と表示される
            // テーブルの場合：「TbRow Row1」の形式と表示される
            var strClassName = e.target.className;

            // チップの場合、何もしない
            if (left(strClassName, 3) == "MCp") {
                return;
            }

            // TbRowにタップ無効フラグ
            if (!gCanTbRowTapFlg) {
                gCanTbRowTapFlg = true;
                return;
            }

            // 行数を取得
            var nRowNo;
            // タップイベント発生した所がTbRowのdivの場合
            if (strClassName.indexOf(" Row") > 0) {
                nRowNo = parseInt(strClassName.substring(strClassName.indexOf(" Row") + 4, strClassName.length));
            } else {
                // タップイベント発生した所がチップの場合
                nRowNo = GetRowNoByChipId(e.target.id);
            }

            // タップ行目が非稼働日の場合、何もしない
            if ($(".stallNo" + nRowNo).hasClass("WhiteBack")) {
                return;
            }

            // サブチップの場合
            if (gArrObjSubChip[gSelectedChipId]) {
                if ((gArrObjSubChip[gSelectedChipId].subChipAreaId != C_RECEPTION) && (gArrObjSubChip[gSelectedChipId].subChipAreaId != C_NOSHOW) && (gArrObjSubChip[gSelectedChipId].subChipAreaId != C_STOP)) {
                    // タップした所に他のチップがあれば、何もしない
                    if (IsChipInTapPos(nRowNo, nTouchXPos) == 1) {
                        return;
                    }
                    // チップ選択状態を解除する
                    SetChipUnSelectedStatus();
                    // 選択したチップを解放する
                    SetTableUnSelectedStatus();
                    // サブチップボックス閉じる
                    SetSubChipBoxClose();
                    CreateFooterButton(C_FT_DISPTP_UNSELECTED, 0);
                    return;
                }
            }

            // Movingチップの場合
            if (gMovingChipObj != null) {
                // Movingチップが緑とグレーチップの場合、移動不可
                // 作業中のチップの場合
                //2013/11/29 TMEJ 丁 タブレット版SMB チーフテクニシャン機能開発 START
                //if ((IsDefaultDate(gMovingChipObj.rsltStartDateTime) == false) && (IsDefaultDate(gMovingChipObj.rsltEndDateTime) == true)) {
                if ((IsDefaultDate(gMovingChipObj.rsltStartDateTime) == false) && (IsDefaultDate(gMovingChipObj.rsltEndDateTime) == true) &&
                    (((gOpenningSubBoxId != C_RECEPTION) || ((gOpenningSubBoxId == C_RECEPTION) && (CheckgArrObjSubChip(gSelectedChipId) == false))))) {
                    //2013/11/29 TMEJ 丁 タブレット版SMB チーフテクニシャン機能開発 END
                    // 移動不可
                    return;
                    //2013/11/29 TMEJ 丁 タブレット版SMB チーフテクニシャン機能開発 START
                    //} else if (IsDefaultDate(gMovingChipObj.rsltEndDateTime) == false) {
                } else if ((IsDefaultDate(gMovingChipObj.rsltEndDateTime) == false) &&
                            (((gOpenningSubBoxId != C_RECEPTION) || ((gOpenningSubBoxId == C_RECEPTION) && (CheckgArrObjSubChip(gSelectedChipId) == false))))) {
                    //2013/11/29 TMEJ 丁 タブレット版SMB チーフテクニシャン機能開発 END
                    // 移動不可
                    return;
                }
            }

            // タップした所は普通のチップの場合
            if ((IsChipInTapPos(nRowNo, nTouchXPos) == 1)
                || (IsChipInTapPos(nRowNo, nTouchXPos) == 3)) {
                return;
            } else if ((IsChipInTapPos(nRowNo, nTouchXPos) == 2)) {
                // タップしたのは休憩エリアの場合
                // 選択のは使用不可チップの場合、戻す
                //2017/08/30 NSK 竹中(悠) REQ-SVT-TMT-20161109-001 SMB iPadにストールクローズ機能の追加 START
//                if (IsUnavailableArea(gSelectedChipId)) {
//                    return;
                //                } else
                //2017/08/30 NSK 竹中(悠) REQ-SVT-TMT-20161109-001 SMB iPadにストールクローズ機能の追加 END
                if (gSelectedChipId == C_OTHERDTCHIPID) {
                    // 選択のは日跨ぎ使用不可チップの場合、戻す
                    if (gOtherDtChipObj.stallIdleId) {
                        return;
                    }
                }
            }

            // タップ位置により、列数目を取得
            var nColNo = GetColNoByTouchXPos(nTouchXPos - $(".scroll-inner").position().left);
            var bCheckFlg = false;
            var dtCheckStartTime = new Date();
            var dtCheckEndTime = new Date();
            dtCheckStartTime.setTime(gStartWorkTime.getTime());
            dtCheckEndTime.setTime(gEndWorkTime.getTime());
            // 正時の場合、1時間をマイナス
            if (dtCheckEndTime.getMinutes() == 0) {
                dtCheckEndTime.setTime(dtCheckEndTime.getTime() - 60 * 60 * 1000);
            }

            // 最初からの4列にタップする時、営業開始時間超えるかをチェックする
            if ((nColNo >= 0) && (nColNo <= 4)) {
                dtCheckStartTime.setMinutes((nColNo - 1) * 15);
                if (dtCheckStartTime - gStartWorkTime < 0) {
                    $("#BlueDiv").css("visibility", "hidden");
                    //「営業開始時間({0}:{1})以降に配置してください。」ってメッセージが表示される
                    ShowSC3240101Msg(911);
                    gShowBlueDivFlg = false;
                    SetChipUnSelectedStatus();
                    SetTableUnSelectedStatus();
                    return;
                }
            }

            // 最後までの4列にタップする
            if ((nColNo >= gMaxCol - 3) && (nColNo <= gMaxCol)) {
                dtCheckEndTime.setMinutes((3 - gMaxCol + nColNo) * 15);
                if (dtCheckEndTime - gEndWorkTime >= 0) {
                    $("#BlueDiv").css("visibility", "hidden");
                    //「営業終了時間({0}:{1})以内に配置してください。」ってメッセージが表示される
                    ShowSC3240101Msg(912)
                    gShowBlueDivFlg = false;
                    SetChipUnSelectedStatus();
                    SetTableUnSelectedStatus();
                    return;
                }
            }

            var nLeft = (nColNo - 1) * C_CELL_WIDTH + 1;
            // タップセルのidを取得
            gSelectedCellId = "C_" + nRowNo + "_" + nColNo;

            if (gSelectedChipId == C_OTHERDTCHIPID) {
                if (gOtherDtChipObj.stallUseId) {
                    $("#" + gOtherDtChipObj.stallUseId).css("visibility", "hidden");
                } else {
                    $("#" + C_UNAVALIABLECHIPID + gOtherDtChipObj.stallIdleId).css("visibility", "hidden");
                }
                // 2013/11/29 TMEJ 張 タブレット版SMB チーフテクニシャン機能開発 START
            } else if (gSelectedChipId == C_NEWCHIPID) {
                $("#" + gSelectedChipId).css("left", nLeft);
                $(".Row" + nRowNo).append($("#" + gSelectedChipId));
                // 新規チップのプロトタイプにストールIDを更新する
                SetChipPrototypeTimeAndStallIdData(C_NEWCHIPID);

                // 新規チップ移動する時、位置を記録する(新規詳細ポップアップ表示する位置)
                var scrollMainObjPos = $(".ChipArea_trimming").find(".scroll-inner").position();
                gTranslateValStallX = scrollMainObjPos.left;
                gTranslateValStallY = scrollMainObjPos.top;

                return;
                // 2013/11/29 TMEJ 張 タブレット版SMB チーフテクニシャン機能開発 END
            } else if (gSelectedChipId == C_COPYCHIPID) {
                $("#" + C_COPYCHIPID).css("visibility", "hidden");
                var strStallId = $(".stallNo" + nRowNo)[0].id;
                gCopyChipObj.setStallId(strStallId.replace("stallId_", ""));
            }
            // 使用不可エリアの場合
            if (IsUnavailableArea(gSelectedChipId)) {
                if (gCanMoveUnavailableFlg) {
                    // チップが指定位置に描画される
                    var nStatus = drawUnavailableAreaAtPos(nLeft, nRowNo);
                    // 新規の場合、部分が非表示
                    if (nStatus == 1) {
                        $("#" + C_MOVINGUNAVALIABLECHIPID + " .ShadowBox").css("display", "none");
                        $("#" + C_MOVINGUNAVALIABLECHIPID + " .Front").css("display", "none");
                        // 新規の場合、後の0.5秒で移動不可
                        gCanMoveUnavailableFlg = false;
                        setTimeout(function () { gCanMoveUnavailableFlg = true; }, 500);
                    } else {
                        // Movingチップを表示する
                        $("#" + C_MOVINGUNAVALIABLECHIPID + " .ShadowBox").css("display", "");
                        $("#" + C_MOVINGUNAVALIABLECHIPID + " .Front").css("display", "");
                        // 移動不可チップに爪がない場合
                        if (($("#" + C_MOVINGUNAVALIABLECHIPID + " .TimeKnobPointL").length == 0)
	                        || ($("#" + C_MOVINGUNAVALIABLECHIPID + " .TimeKnobPointR").length == 0)) {
                            // 実績の幅を設定する
                            var nChipWidthTime = parseInt($("#" + gSelectedChipId).data("IDLE_TIME"));
                            $("#" + C_MOVINGUNAVALIABLECHIPID).css("width", nChipWidthTime * C_CELL_WIDTH / 15);
                            // 爪を追加する
                            if ($("#" + C_MOVINGUNAVALIABLECHIPID + " .TimeKnobPointL").length == 0) {
                                var objTimeKnobPointL = $("<div />").addClass("TimeKnobPointL TimeKnobPoint_gray");
                                $("#" + C_MOVINGUNAVALIABLECHIPID).append(objTimeKnobPointL);
                            }
                            if ($("#" + C_MOVINGUNAVALIABLECHIPID + " .TimeKnobPointR").length == 0) {
                                var objTimeKnobPointR = $("<div />").addClass("TimeKnobPointR TimeKnobPoint_gray");
                                $("#" + C_MOVINGUNAVALIABLECHIPID).append(objTimeKnobPointR);
                            }

                            // 爪(右)の位置はチップの幅によって、座標を設定する
                            SetTimeKnobPointRPosbyChipWidth(C_MOVINGUNAVALIABLECHIPID);
                            // リサイズをもう一回bindする
                            BindChipResize(C_MOVINGUNAVALIABLECHIPID, 0, 0);
                        }
                    }
                }
            } else if ((gSelectedChipId == C_OTHERDTCHIPID) && gOtherDtChipObj.stallIdleId) {
                drawUnavailableAreaAtPos(nLeft, nRowNo);
            } else {
                //2013/11/29 TMEJ 丁 タブレット版SMB チーフテクニシャン機能開発 START
                if (gArrObjSubChip[gSelectedChipId]) {
                    if ((gMovingChipObj.roJobSeq == "0") && (gOpenningSubBoxId == C_RECEPTION) && (IsDefaultDate(gMovingChipObj.rsltStartDateTime) == false)) {
                        gMovingChipObj.inspectionRsltId = 0;
                        gMovingChipObj.cwRsltStartDateTime = new Date(C_DATE_DEFAULT_VALUE);
                        gMovingChipObj.cwRsltEndDateTime = new Date(C_DATE_DEFAULT_VALUE);
                        gMovingChipObj.carWashRsltId = "0";
                        gMovingChipObj.rsltStartDateTime = new Date(C_DATE_DEFAULT_VALUE);
                        gMovingChipObj.rsltEndDateTime = new Date(C_DATE_DEFAULT_VALUE);
                        gMovingChipObj.rsltDeliDateTime = new Date(C_DATE_DEFAULT_VALUE);
                        gMovingChipObj.stallUseStatus = "00";
                        gMovingChipObj.upperDisp = "";
                        gMovingChipObj.lowerDisp = "";
                        gMovingChipObj.svcClassName = "";
                        gMovingChipObj.svcClassNameEng = "";

                        //古いMovingチップを削除する
                        $("#" + C_MOVINGCHIPID).remove();
                        //新しくMovingチップを作成する
                        gMovingChipObj.createChip(C_CHIPTYPE_STALL_MOVING);
                        //サイズ調整
                        $("#" + C_MOVINGCHIPID).css("width", C_SubChipWidth - 1);

                        // 幅によって、チップに表示内容を調整する
                        AdjustChipItemByWidth(C_MOVINGCHIPID);
                        // Movingチップのりサイズをbindする
                        BindChipResize(C_MOVINGCHIPID, 0, 0);
                        // Movingチップの爪以外の部分が半透明、見えない
                        $("#" + C_MOVINGCHIPID + " .CpInner").css({ "opacity": C_OPACITY_TRANSPARENT, "visibility": "hidden" });
                        // Movingチップのz-indexを追加する
                        ChangeChipZIndex(C_MOVINGCHIPID, "MovingChipZIndex");
                        // Movingチップのタップイベントのbind
                        BindMovingSubChipTapEvent();
                    }
                }
                //2013/11/29 TMEJ 丁 タブレット版SMB チーフテクニシャン機能開発 END
                // チップが指定位置に描画される
                drawSelectedChipAtPos(nLeft, nRowNo);

                // サブエリアから画面に移動すれば、topが-10000pxである
                $("#" + C_MOVINGCHIPID).css("top", 1);

                // 日跨ぎで表示された時(左または右の爪がない)、無くなった爪を追加する
                var nOverDaysStatus = C_OVERDAYS_NONE;

                // 2018/02/20 NSK 小川 お客様受付における情報伝達の仕組み 適合性検証 START
                // 爪の色を設定する
                // var strColorClass = " TimeKnobPoint_skyblue";
                var strColorClass = " TimeKnobPoint_orange";
                // 2018/02/20 NSK 小川 お客様受付における情報伝達の仕組み 適合性検証 END

                // チップが青いの場合、爪も青いにする
                if ($("#" + C_MOVINGCHIPID + " .CpInner").hasClass("StRez")) {
                    strColorClass = " TimeKnobPoint_blue";
                }
                // 両方爪がない、追加する
                if (($("#" + C_MOVINGCHIPID + " .TimeKnobPointL").length == 0) && ($("#" + C_MOVINGCHIPID + " .TimeKnobPointR").length == 0)) {
                    nOverDaysStatus = C_OVERDAYS_BOTH;
                    var objTimeKnobPointL = $("<div />").addClass("TimeKnobPointL" + strColorClass);
                    $("#" + C_MOVINGCHIPID).append(objTimeKnobPointL);
                    var objTimeKnobPointR = $("<div />").addClass("TimeKnobPointR" + strColorClass);
                    $("#" + C_MOVINGCHIPID).append(objTimeKnobPointR);
                } else {
                    if ($("#" + C_MOVINGCHIPID + " .TimeKnobPointL").length == 0) {
                        nOverDaysStatus = C_OVERDAYS_RIGHT;
                        // 左の爪を追加する
                        var objTimeKnobPointL = $("<div />").addClass("TimeKnobPointL" + strColorClass);
                        $("#" + C_MOVINGCHIPID).append(objTimeKnobPointL);
                    }
                    if ($("#" + C_MOVINGCHIPID + " .TimeKnobPointR").length == 0) {
                        nOverDaysStatus = C_OVERDAYS_LEFT;
                        // 右の爪を追加する
                        var objTimeKnobPointR = $("<div />").addClass("TimeKnobPointR" + strColorClass);
                        $("#" + C_MOVINGCHIPID).append(objTimeKnobPointR);
                    }
                }
                // 日跨ぎで表示された場合、Movingチップの幅を設定し直す
                if (nOverDaysStatus > C_OVERDAYS_NONE) {
                    var nWorkTime = gMovingChipObj.scheWorkTime;
                    if (IsDefaultDate(gMovingChipObj.rsltEndDateTime) == false) {
                        nWorkTime = gMovingChipObj.rsltWorkTime;
                    }
                    var nWidth = nWorkTime * C_CELL_WIDTH / 15;
                    // 最小値(5分単位)より、小さいの場合、最小値で設定する
                    if (nWidth < C_CELL_WIDTH / 15 * gResizeInterval - 1) {
                        nWidth = C_CELL_WIDTH / 15 * gResizeInterval;
                    }
                    $("#" + C_MOVINGCHIPID).css("width", nWidth - 1);

                    // 幅によって、チップに表示内容を調整する
                    AdjustChipItemByWidth(C_MOVINGCHIPID);

                    // 爪(右)の位置はチップの幅によって、座標を設定する
                    SetTimeKnobPointRPosbyChipWidth(C_MOVINGCHIPID);

                    // リサイズをもう一回bindする
                    BindChipResize(C_MOVINGCHIPID, 0, 0);
                }
                // Movingチップを表示する
                $("#" + C_MOVINGCHIPID + " .CpInner").css("visibility", "visible");

            }
        } else {
            if (gPopupBoxId != "") {
                // チップ選択状態を解除する
                SetChipUnSelectedStatus();
            }
            // サブチップボックス閉じる
            SetSubChipBoxClose();
            CreateFooterButton(C_FT_DISPTP_UNSELECTED, 0);
        }
    })
    .bind(C_TOUCH_START, function (e) {

        // タップの横座標 
        nTouchXPos = getTapXPosition();

        // クラス名は「TbRow Row1」の形式と表示される
        var strClassName = e.target.className;
        var strId = e.target.id;
        var nIndex = strClassName.indexOf(" Row");
        // タップしたエリアが休憩エリア且つ普通のセルではない場合、何もしない
        if ((nIndex == -1) && (strId.indexOf(C_RESTCHIPID) != 0)) {
            return
        }

        if (gOnTouchingFlg) {
            return;
        }

        // タップ行目
        var nRowNo;
        // タップ場所により、行目を取得
        if (strId.indexOf(C_RESTCHIPID) == 0) {
            // 2014/07/15 TMEJ 張 タブレットSMB Job Dispatch機能開発 START
            //            nRowNo = GetRowNoByChipId(strId);

            // 親のDomのクラスを取得
            var strParentClassName = $("#" + strId).offsetParent()[0].className;

            // 行数を取得
            nRowNo = parseInt(strParentClassName.substring(9, strParentClassName.length));

            // 2014/07/15 TMEJ 張 タブレットSMB Job Dispatch機能開発 END
        } else {
            nRowNo = parseInt(strClassName.substring(nIndex + 4, strClassName.length));
        }

        // タップ位置により、列数目を取得
        var nColNo = GetColNoByTouchXPos(nTouchXPos - $(".scroll-inner").position().left);
        var nLeft = (nColNo - 1) * C_CELL_WIDTH;
        if (gShowBlueDivFlg) {
            $("#BlueDiv").css("left", nLeft).css("top", $(".Row" + nRowNo).position().top).css("visibility", "visible");
        } else {
            gShowBlueDivFlg = true;
        }

        // 2013/11/29 TMEJ 張 タブレット版SMB チーフテクニシャン機能開発 START
        // あるチップが選択されている場合、チップが新規できない
        if (gSelectedChipId != "") {
            return;
        }
        // 2本同時にタップするなので、2回目この関数を走る必要がない
        if ($(".ChipArea").data("tapHold")) {
            return;
        }

        $(".ChipArea").data("tapHold", setTimeout(function () {
            // タッチしたセルの位置にC_NEWCHIPIDチップが生成される
            CreateNewChip(nRowNo, nColNo);
            // 1秒後、ChipAreaのdivのdata(tapHold)をリセットする
            $(".ChipArea").data("tapHold", null);
        }, 1000));
        // 2013/11/29 TMEJ 張 タブレット版SMB チーフテクニシャン機能開発 END
    })
    .bind(C_TOUCH_MOVE, function (e) {
        $("#BlueDiv").css("visibility", "hidden");
        // 2013/11/29 TMEJ 張 タブレット版SMB チーフテクニシャン機能開発 START
        // 1秒内、touchendまたはtouchmoveをすれば、setTimeoutの関数をやめる
        if ($(".ChipArea").data("tapHold")) {
            // 1秒内、touchendまたはtouchmoveをすれば、setTimeoutの関数をやめる
            clearTimeout($(".ChipArea").data("tapHold"));
        }
        // ChipAreaのdivのdata(tapHold)をリセットする
        $(".ChipArea").data("tapHold", null);
        // 2013/11/29 TMEJ 張 タブレット版SMB チーフテクニシャン機能開発 END
    })
    .bind(C_TOUCH_END, function (e) {
        $("#BlueDiv").css("visibility", "hidden");
        // 2013/11/29 TMEJ 張 タブレット版SMB チーフテクニシャン機能開発 START
        // 1秒内、touchendまたはtouchmoveをすれば、setTimeoutの関数をやめる
        if ($(".ChipArea").data("tapHold")) {
            // 1秒内、touchendまたはtouchmoveをすれば、setTimeoutの関数をやめる
            clearTimeout($(".ChipArea").data("tapHold"));
        }
        // ChipAreaのdivのdata(tapHold)をリセットする
        $(".ChipArea").data("tapHold", null);
        // 2013/11/29 TMEJ 張 タブレット版SMB チーフテクニシャン機能開発 END
    });
}

/**
* 中断ポップアップウィンドウのイベントをバインドする
* @return {なし}
*/
function BindStopWndEvent() {

    // 中断時間
    $("#StopTimeTxt")
        .click(function () {
            //編集可能な場合のみ、ラベルとテキストを切り替える
            if ($(this).hasClass(C_SC3240201CLASS_TEXTBLACK) == false) {
                $("#StopTimeTxt").css("display", "block");
                // 2014/07/15 TMEJ 張 タブレットSMB Job Dispatch機能開発 START
                $(".popStopWindowBase #StopTimeTxt").parent().css("display", "inline-block"); 
                // 2014/07/15 TMEJ 張 タブレットSMB Job Dispatch機能開発 END
                $(".StopTimeLabel").css("display", "none");
            }
        })
        .blur(function () {
            //編集可能な場合のみ、ラベルとテキストを切り替える
            if ($(this).hasClass(C_SC3240201CLASS_TEXTBLACK) == false) {
                
                //5分単位で切り上げる
                var nMinutes = smbScript.RoundUpToNumUnits($("#StopTimeTxt").val(), gResizeInterval, 0, C_MAXSTOPTIME);
                //分文言を取得
                $(".popStopWindowBase .StopTimeLabel").html(nMinutes + gSC3240101WordIni[27])
                $("#StopTimeTxt").css("display", "none");

                // 2014/07/15 TMEJ 張 タブレットSMB Job Dispatch機能開発 START
                $(".popStopWindowBase #StopTimeTxt").parent().css("display", "inline-block");
                // 2014/07/15 TMEJ 張 タブレットSMB Job Dispatch機能開発 END

                $(".StopTimeLabel").css("display", "block");
            }
        });

    // 中断メモの選択ボックス
        $("#dpDetailStopMemo")
        .click(function () {
            $("#dpDetailStopMemo").focus();
        })
        .blur(function () {
            // 中断メモに選択した内容を追加する
            var e = document.getElementById("dpDetailStopMemo");
            if (e.selectedIndex != 0) {
                $("#lblDetailStopMemo").text(e.options[e.selectedIndex].text);
                var strText = $("#txtStopMemo").val();
                strText += e.options[e.selectedIndex].text;
                $("#txtStopMemo").val(strText);
                // 中断メモに文字が追加されるため、高さ、文字最大数などを再計算
                DeleteOverString($("#txtStopMemo"));
                AdjusterStopTextArea();
            }
        });

    // 中断メモ
        $("#txtStopMemo")
        .blur(function () {
            DeleteOverString($("#txtStopMemo"));
            AdjusterStopTextArea();
            $("#btnJobStopDummy").focus();
        })
        .click(function () {
            DeleteOverString($("#txtStopMemo"));
            $("#txtStopMemo").focus();
        })
        .bind("paste", function (e) {
            setTimeout(function () {
                DeleteOverString($("#txtStopMemo"));
                AdjusterStopTextArea();
            }, 0);
        })
        .bind("keyup", function () {
            DeleteOverString($("#txtStopMemo"));
            AdjusterStopTextArea();
        })
        .bind("keydown", function () {
            DeleteOverString($("#txtStopMemo"));
        });

}
/**	
* テキストエリア内の文字列長制御を行う 	
* @param {$(textarea)} ta
*/
function DeleteOverString(ta) {
    //許容する最大バイト数
    var maxLen = ta.attr("maxlen");
    if (ta.val().length > maxLen) {
        ta.val(ta.val().substring(0, maxLen));
    }
}
/**	
* テキストエリア内の文字列長制御を行う 	
*/
function AdjusterStopTextArea() {
    var textArea = $("#txtStopMemo");
    // 2013/11/29 TMEJ 張 タブレット版SMB チーフテクニシャン機能開発 START
    //    if (textArea.attr("scrollHeight") == textArea.height()) {
    //        return;
    //    }
    // 2013/11/29 TMEJ 張 タブレット版SMB チーフテクニシャン機能開発 END

    textArea.height(100);

    var tmp_sh = textArea.attr("scrollHeight");

    while (tmp_sh > textArea.attr("scrollHeight")) {
        tmp_sh = textArea.attr("scrollHeight");
        textarea[0].scrollHeight++;
    }

    if (textArea.attr("scrollHeight") >= textArea.attr("offsetHeight")) {
        textArea.height(textArea.attr("scrollHeight"));
        $("#StopMemoScrollBox .scroll-inner").height(437 + textArea.attr("scrollHeight") - 100);
        $("#StopMemoScrollBox .scroll-inner").css({ "transform": "translate3d(0px, 0px, 0px)" });
    }
}

/**
* タップした所にチップがあるかどうか
* @param {Integer} nRowNo 行数目
* @param {Integer} nTouchXPos タップxPos
* @return {bool} 0：重複チップがない　1:重複のは普通のチップ　2:重複のは休憩エリア 3:重複のは普通のチップ+休憩エリア
*/
function IsChipInTapPos(nRowNo, nTouchXPos) {
    // タップした所に他のチップがあれば、何もしない
    var nRtFlg = 0;
    var bRestFlg = false;
    var bChipFlg = false;
    $(".Row" + nRowNo).children("div").each(function (index, e) {
        if ((e.id != C_MOVINGCHIPID) && (e.id != gSelectedChipId)) {
            // タップしたxPosにチップがあれば、bRtFlgをtrueに設定
            if (((e.offsetLeft + e.offsetWidth) >= nTouchXPos - C_SCREEN_CHIPAREA_OFFSET_LEFT - $(".scroll-inner").position().left)
                        && (e.offsetLeft <= nTouchXPos - C_SCREEN_CHIPAREA_OFFSET_LEFT - $(".scroll-inner").position().left)) {
                if (IsUnavailableArea(e.id) || IsRestArea(e.id)) {
                    bRestFlg = true;
                } else {
                    bChipFlg = true;
                }
            }
        }
    });

    // 戻り値の計算
    if (bChipFlg) {
        nRtFlg += 1;
    }

    if (bRestFlg) {
        nRtFlg += 2;
    }
    return nRtFlg;
}

/**
* テーブルの状態をチップの選択状態に設定する
* @return {なし}
*/
function SetTableSelectedStatus() {

    var strChipId = gSelectedChipId;

    // すべてチップがBlackBack色を追加する
    $(".MCp .Front").addClass("BlackBack");
    $(".SCp .Front").addClass("BlackBack");

    // 白枠をグレーにする
    $(".WhiteBorder p").css("border", "gray 3px solid");

    // サブチップの場合
    if (IsChipOrSubChip(gSelectedChipId) == C_CHIPTYPE_SUBCHIP) {
        var arrChipIds = FindRelationChipsFromSubChip("", gArrObjSubChip[gSelectedChipId].svcInId);
        if (arrChipIds.length > 0) {
            strChipId = arrChipIds[0][0];
        }
    }

    // リレーションチップの場合、リレーションチップのBlackBack色を解除する
    // 2013/11/29 TMEJ 張 タブレット版SMB チーフテクニシャン機能開発 START
    //if (IsRelationChip(strChipId) == true) {
    if ((strChipId != C_NEWCHIPID) && (IsRelationChip(strChipId) == true)) {
        // 2013/11/29 TMEJ 張 タブレット版SMB チーフテクニシャン機能開発 END
        // すべてチップをループして、リレーションチップを探す
        for (var strId in gArrObjChip) {
            // 有効のチップデータをチェックする
            if (CheckgArrObjChip(strId) == false) {
                continue;
            }
            if (gArrObjChip[strId].svcInId == gArrObjChip[strChipId].svcInId) {
                    $("#" + strId + " .Front").removeClass("BlackBack");
                if ((IsDefaultDate(gArrObjChip[strId].rsltStartDateTime) == false) && (IsDefaultDate(gArrObjChip[strId].rsltEndDateTime) == true)) {
                    // zindexを調整する(作業中リレーションチップ)
                    ChangeChipZIndex(strId, "SelectedRelationWorkingChipZIndex");
                } else if (IsDefaultDate(gArrObjChip[strId].rsltEndDateTime) == false) {
                    // zindexを調整する(作業終了リレーションチップ)
                    ChangeChipZIndex(strId, "SelectedRelationWorkOverChipZIndex");
                } else {
                    // zindexを調整する(作業前リレーションチップ)
                    ChangeChipZIndex(strId, "SelectedRelationRezChipZIndex");
                }
            }
        }
    } else {
        // 別の日のチップの場合、別の日のチップにBlackBack色を解除する
        if (strChipId == C_OTHERDTCHIPID) {
            if (gOtherDtChipObj.KEY) {
                $("#" + gOtherDtChipObj.KEY + " .Front").removeClass("BlackBack");
            } else {
                $("#" + gOtherDtChipObj.stallUseId + " .Front").removeClass("BlackBack");
            }
            if (gOtherDtChipObj.svcInId) {
                var strRelationChipId = "";
                // すべてチップをループして、リレーションチップを探す
                for (var strId in gArrObjChip) {
                    // 有効のチップデータをチェックする
                    if (CheckgArrObjChip(strId) == false) {
                        continue;
                    }
                    if (gOtherDtChipObj.svcInId == gArrObjChip[strId].svcInId) {
                        $("#" + strId + " .Front").removeClass("BlackBack");
                        // zindexを調整する
                        ChangeChipZIndex(strId, "SelectedRelationRezChipZIndex");
                        strRelationChipId = strId;
                    }
                }
                // リレーション線を描画する
                if (strRelationChipId) {
                    // リレーションチップの線を表示する
                    ShowRelationLine(strRelationChipId);
                }
            }
        } else {
            // 選択中チップがBlackBack色を解除する
            $("#" + strChipId + " .Front").removeClass("BlackBack");
        }
    }
    // 選択チップがstrChipIdの場合、
    if (gSelectedChipId == strChipId) {
        // 選択したチップの影を追加する
        $("#" + strChipId).addClass("SelectedChipShadow");

        // zindexを調整する(選択したチップだけSelectedChipZIndexクラスを追加)
        $(".MCp").removeClass("SelectedChipZIndex");
        ChangeChipZIndex(strChipId, "SelectedChipZIndex");
    }

    // 定期リフレッシュと自動スクロールをやめる
    clearInterval(gFuncRefreshTimerInterval);
    gScrollTimerInterval = "";
    gFuncRefreshTimerInterval = "";
}

/**
* テーブルの状態をチップの選択状態に解除する
* @return {なし}
*/
function SetTableUnSelectedStatus() {

    // すべてチップの上にBlackBack色を解除する
    $(".MCp .Front").removeClass("BlackBack");
    $(".SCp .Front").removeClass("BlackBack");
    // 白枠を白色にする
    $(".WhiteBorder p").css("border", "#FFF 3px solid");

    // intervalを再開する(自動スクロールと定期リフレッシュ)
    if (gFuncRefreshTimerInterval == "") {
        gFuncRefreshTimerInterval = setInterval("RefreshSMB()", gRefreshTimerInterval * 1000);
    }
}

/**
* リレーション線を描画する
* @param {Integer} x1 開始点の左座標
* @param {Integer} y1 開始点の右座標
* @param {Integer} x2 終了点の左座標
* @param {Integer} y2 終了点の右座標
* @return {なし}
*/
function DrawRelationLine(strLineId, x1, y1, x2, y2) {
    // 同じIDのリレーション線があれば、削除する
    if ($("#" + strLineId).length > 0) {
        $("#" + strLineId).remove();
    }
    var nAngle, nWidth, nTop, nLeft, nDeg;
    x1 += C_CHIPAREA_OFFSET_LEFT;
    x2 += C_CHIPAREA_OFFSET_LEFT;
    var lenX = x2 - x1;
    var lenY = y2 - y1;
    var temp;
    if (x2 < x1) {
        temp = x1;
        x1 = x2;
        x2 = temp;
    }
    if (y2 < y1) {
        temp = y1;
        y1 = y2;
        y2 = temp;
    }
    // 線の角度を計算する
    nDeg = Math.atan((y2 - y1) / (x2 - x1));

    if (((lenX >= 0) && (lenY >= 0))
        || ((lenX < 0) && (lenY < 0))) {
        nAngle = nDeg * 180 / Math.PI;
    } else {
        nAngle = 0 - nDeg * 180 / Math.PI;
    }

    // 線の長さを計算する
    nWidth = Math.sqrt((x1 - x2) * (x1 - x2) + (y1 - y2) * (y1 - y2));

    nTop = y1 + ((nWidth / 2) * Math.sin(nDeg));
    nLeft = (x2 - x1) / 2 + x1 - (nWidth / 2);

    // 線のオブジェクト
    var objLine = $("<div />").addClass("RelationalLine RelationLineZIndex");
    objLine.attr("id", strLineId);
    // 画面に表示する
    $(".ChipArea").append(objLine);
    $("#" + strLineId).css("left", nLeft);
    $("#" + strLineId).css("top", nTop);
    $("#" + strLineId).css("width", nWidth);
    $("#" + strLineId).css("height", 2);
    $("#" + strLineId).css("transform", "rotate(" + nAngle + "deg)");
}
/**
* セルのidにより、行数目を取得する
* @param {String}  strCellId セルのid
* @return {Integer} 行数目
*/
function GetRowNoByCellId(strCellId) {
    // セルidはC_1_1の形式なので、2つ"_"中のは行数目
    return parseInt(strCellId.substring(strCellId.indexOf("_") + 1, strCellId.lastIndexOf("_")));
}
/**
* 時刻線の初期化
* @return {-}
*/
function InitRedTimeLinePropty() {
    // 赤線の高さを設定する
    var nHeight = $(".ChipArea").height();
    $(".TimingLineSet").css("height", nHeight);
    $(".TimingLineSet .TimingLine01").css("height", nHeight);
    $(".TimingLineSet .TimingLine02").css("height", nHeight);
    $(".TimingLineSet .TimingLine03").css("height", nHeight);

    $(".TimingLineDeli").css("height", nHeight);
    $(".TimingLineDeli .TimingLine01").css("height", nHeight);
    $(".TimingLineDeli .TimingLine02").css("height", nHeight);
    $(".TimingLineDeli .TimingLine03").css("height", nHeight);

    // 時刻線位置を設定する(横軸)
    setRedTimeLineLeftPos(false);

    // クリックイベントのbind
    BindTimeLineClickEvent();
}
/**
* 時刻線のクリックイベントのbind
* @return {無し}
*/
function BindTimeLineClickEvent() {
    $(".TimingLineSet").bind("chipTap ", function (e) {
        // 行目を取得する
        var nRowNo = GetRowNoByYPos(event.offsetY);
        // 今の時間を取得する
        var dtNow = GetServerTimeNow();

        // チップarrayを作成
        var arrChipIds = new Array();
        var bMovingChipFlg = false;
        // 当行のチップ全部ループする
        $(".Row" + nRowNo).children("div").each(function (index, eChildChip) {
            // メインストールのチップの場合
            if (IsChipOrSubChip(eChildChip.id) == C_CHIPTYPE_STALL) {
                // Movingチップの場合
                if (eChildChip.id == C_MOVINGCHIPID) {
                    bMovingChipFlg = true;
                } else {
                    if (!bMovingChipFlg) {
                        if (gArrObjChip[eChildChip.id]) {
                            // 当座標にあるチップを探す
                            if ((gArrObjChip[eChildChip.id].displayStartDate - dtNow <= 0) && (gArrObjChip[eChildChip.id].displayEndDate - dtNow >= 0)) {
                                // idを記録
                                arrChipIds.push(eChildChip.id);
                            }
                        } else {
                            //Todo白い枠など

                        }
                    }
                }

            } else {
                //TODO: サブチップなど

            }
        });

        // Movingチップがあれば、必ずmovingチップのchiptapイベントを発生する
        if (bMovingChipFlg) {
            TapMovingChip();
        } else {
            // 時刻線の裏にチップがあれば
            if (arrChipIds.length == 1) {
                TapStallChip(arrChipIds[0]);
            } else if (arrChipIds.length > 1) {
                //TODO: 重複の場合、

            }
        }
        //TODO: ポップアップボックスとか

    });
}

/**
* 時刻線1分単位で移動
* @return {無し}
*/
function SlideTimeLineInOneMinute() {
    var dtNow = GetServerTimeNow();
    // 毎回正時の時、画面をスクロール
    if (dtNow.getMinutes() == 0) {
        setRedTimeLineLeftPos(true);
    } else {
        // 他の場合、画面をスクロールしなくて、時刻線を移動する
        setRedTimeLineLeftPos(false);
    }
}

/**
* 時刻線位置を設定する(横軸)
* @param {Bool} True:画面をスクロール False:画面をスクロールしない
* @return {無し}
*/
function setRedTimeLineLeftPos(bScrollFlag) {

    // 当ページは今日以外の日付の場合、時刻線を表示しない
    if (IsTodayPage() == false) {
        $(".TimingLineSet").css("visibility", "hidden");
    } else {
        // 今の時刻を取得する
        var dtNow = GetServerTimeNow();

        // 時間により、時刻線の位置を取得する
        var setPosition = GetTimeLinePosByTime(dtNow);

        // 位置を設定して、表示する
        $(".TimingLineSet").css("left", setPosition);
        $(".TimingLineSet").css("visibility", "visible");
    }

    var strDelayStallNum = $("#FooterButtonCount800")[0].innerText;
    // 遅刻ストールを赤に設定する
    SetLaterStallColorRed();

    // スクロールかどうかフラグ
    if (bScrollFlag) {
        // 選択状態ではない場合、画面をスクロールする
        if (gSelectedChipId != "") {
            ScrollWindowToNowTime(false);
        }
    }
    // 遅れストール数が変わる
    if (strDelayStallNum != $("#FooterButtonCount800")[0].innerText) {
        // 遅れ絞り込んだ時
        if (gShowLaterStallFlg) {
            // 再度全表示
            ShowAllStall();
            // 遅れストールだけを表示する
            ShowLaterStall();
        }
    }
}
/**
* 中心に表示されるようにスクロールする
* @param {integer} bScrollFlg true:日付を変えると、スクロールする false:定期リフレッシュ、スクロールしない
* @return {void}
*/
function ScrollWindowToNowTime(bScrollFlg) {
    // 定期リフレッシュ中
    if (gCanScrollFlg == false) {
        return;
    }
    // 今日以外の場合、最初の位置に移動
    if (IsTodayPage() == false) {
        if (bScrollFlg) {
            $(".ChipArea_trimming").SmbFingerScroll({
                action: "move",
                moveY: $(".scroll-inner").position().top,
                moveX: $(".scroll-inner").position().left
            });
        }
        return;
    }

    var nMoveY = 0;
    // 日付を変えると、必ず1行目に戻す
    if (bScrollFlg) {
        nMoveY = $(".scroll-inner").position().top;
    }
    // 今の時間
    var dtNow = new GetServerTimeNow();
    var hour = dtNow.getHours();

    // スクロールはじめの列目を計算する
    var nhours = hour - gStartWorkTime.getHours() - 2;
    var nStartColIndex = 4 * nhours; ;
    // スクロール
    $(".ChipArea_trimming").SmbFingerScroll({
        action: "move", moveY: nMoveY, moveX: nStartColIndex * C_CELL_WIDTH + $(".scroll-inner").position().left
    });
}
/**
* 時刻線の高さを設定する
* @param {Integer} nHeight 時刻線の高さ
* @return {-} 無し
*/
function SetTimeLineHeight(nHeight) {
    $(".TimingLineSet").css("height", nHeight);
    $(".TimingLine01").css("height", nHeight);
    $(".TimingLine02").css("height", nHeight);
    $(".TimingLine03").css("height", nHeight);
}
/**
* カレンダーに今日の日付を設定する
* @return {void}
*/
function ShowCalendar() {
    // ハイドンコントロールの日付を取得する
    var dtShowDate = new Date($("#hidShowDate").val());
    // 表示する(MM/DD(曜日))
    // 2013/11/29 TMEJ 張 タブレット版SMB チーフテクニシャン機能開発 START
    //    $("#pCalendar").text(add_zero(dtShowDate.getMonth() + 1) + "/" + add_zero(dtShowDate.getDate()) + GetDay(dtShowDate.getDay()));
    $("#pCalendar").text(DateFormat(dtShowDate, gDateFormatMMdd) + GetDay(dtShowDate.getDay()));
    // 2013/11/29 TMEJ 張 タブレット版SMB チーフテクニシャン機能開発 END
    $("#dtCalendar")[0].value = dtShowDate.getFullYear() + "-" + add_zero(dtShowDate.getMonth() + 1) + "-" + add_zero(dtShowDate.getDate());
}
/**
* カレンダーバリューが変わるイベント
* @param {Int} 日付を変える設定フラグ
* @return {void}
*/
function ChangeDateValue(nSetFlg) {
    var dtShowDate = $("#dtCalendar")[0].valueAsDate;

    // カレンダーに「消去」ボタンを押すと、$("#dtCalendar")[0].valueAsDateがnull
    if ($("#dtCalendar")[0].valueAsDate == null) {
        // 今日の日付を設定する
        var dtToday = GetServerTimeNow();
        dtToday.setHours(0);
        dtToday.setMinutes(0);
        dtToday.setSeconds(0);
        dtToday.setMilliseconds(0);
        dtShowDate = new Date();
        dtShowDate.setDate(dtToday.getDate());
        // 2013/11/29 TMEJ 張 タブレット版SMB チーフテクニシャン機能開発 START
        //        $("#pCalendar").text(add_zero(dtShowDate.getMonth() + 1) + "/" + add_zero(dtShowDate.getDate()) + GetDay(dtShowDate.getDay()));
        $("#pCalendar").text(DateFormat(dtShowDate, gDateFormatMMdd) + GetDay(dtShowDate.getDay()));
        // 2013/11/29 TMEJ 張 タブレット版SMB チーフテクニシャン機能開発 END
        $("#dtCalendar")[0].valueAsDate = dtToday.getFullYear() + "-" + add_zero(dtToday.getMonth() + 1) + "-" + add_zero(dtToday.getDate());
    } else {
        // 表示用の値を変更する
        // 2013/11/29 TMEJ 張 タブレット版SMB チーフテクニシャン機能開発 START
        //        $("#pCalendar").text(add_zero(dtShowDate.getMonth() + 1) + "/" + add_zero(dtShowDate.getDate()) + GetDay(dtShowDate.getDay()));
        $("#pCalendar").text(DateFormat(dtShowDate, gDateFormatMMdd) + GetDay(dtShowDate.getDay()));
        // 2013/11/29 TMEJ 張 タブレット版SMB チーフテクニシャン機能開発 END
    }
    
    // 設定フラグが1の場合、画面が日付の日に遷移する
    if (nSetFlg == 1) {
        // hiddenコントロールの値を修正
        var dtPreDate = new Date($("#hidShowDate").val());
        dtPreDate.setHours(0);
        dtPreDate.setMinutes(0);
        dtPreDate.setSeconds(0);
        dtPreDate.setMilliseconds(0);
        dtShowDate.setHours(0);
        dtShowDate.setMinutes(0);
        dtShowDate.setSeconds(0);
        dtShowDate.setMilliseconds(0);
        // 画面遷移
        var nChangeDt = (dtShowDate - dtPreDate) / 24 / 60 / 60 / 1000;
        // 日付が変わった時
        if (nChangeDt != 0) {
            ClickChangeDate(nChangeDt);
        }
    }
}
/**
* 前の日付の設定
* @return {なし}
*/
function imgbtnPrevDate_onClick() {
    // カレンダーの矢印ボタンクリック
    ClickChangeDate(-1);
    return false;
}

/**
* 前の日付の設定
* @return {なし}
*/
function imgbtnNextDate_onClick() {
    // カレンダーの矢印ボタンクリック
    ClickChangeDate(1);
    return false;
}
/**
* なにもしてない関数(サブと接続することを防止するため、無くなった場合は翌日、前日ボタンを押すと、画面のリフレッシュ可能性がある)
* @return {なし}
*/
function imgbtnDate_onClick() {
    return false;
}

/**
* カレンダーの矢印ボタンクリック
* @param {int} 変更日付数
* @return {-} 
*/
function ClickChangeDate(nDays) {

    // 日付を変える時、全てストールを表示する
    if (gShowLaterStallFlg == true) {
        // もとに戻す
        ShowAllStall();
    }

    // 2013/11/29 TMEJ 張 タブレット版SMB チーフテクニシャン機能開発 START
//    // 詳細画面を閉じる
//    // CloseChipDetailは使えない、使えば、エラーになって、部分の操作をここに移行する
//    //CloseChipDetail-->
//    if ($("#ChipDetailPopup").css("display") == "block") {
//        $("#DetailSMaintenanceTypeList").remove();
//        $("#DetailSMercList").remove();
//        $("#DetailLMaintenanceTypeList").remove();
//        $("#DetailLMercList").remove();
//        $("#ChipDetailPopup").fadeOut(300);
//        //ポップアップが閉じるときは縮小表示にしておく
//        ShrinkDisplay(0);
//        //タイマーをクリア(画面が表示し終わる前に閉じられた場合の対応)
//        commonClearTimer();
//    }
//    //<--CloseChipDetail

//    // 休憩エリアポップアップウィンドウまたは中断ポップアップウィンドウが表示される場合、閉じる
//    if ($(".popRestWindow").css("display") == "block") {
//        $(".popRestWindow").css("display", "none");
//    }
//    if ($(".popStopWindowBase").css("display") == "block") {
//        CancelStopWindow();
//    }

    // 全てポップアップを閉じる
    HideAllPopup();
    // 2013/11/29 TMEJ 張 タブレット版SMB チーフテクニシャン機能開発 END

    // グルグルを表示する
    gMainAreaActiveIndicator.show();

    // 選択したチップを記録する
    // Movingチップの場合
    if (gMovingChipObj != null) {
        // Movingチップが緑とグレーチップの場合、移動不可
        // 実績のチップ以外の場合
        if (IsDefaultDate(gMovingChipObj.rsltStartDateTime)) {
            // 2013/11/29 TMEJ 張 タブレット版SMB チーフテクニシャン機能開発 START
            // 選択チップがある場合
            //if (gSelectedChipId) {
            // 選択チップがある場合(新規チップではなく)
            if ((gSelectedChipId != "") && (gSelectedChipId != C_NEWCHIPID)) {
                // 2013/11/29 TMEJ 張 タブレット版SMB チーフテクニシャン機能開発 END
                if (gSelectedChipId == C_OTHERDTCHIPID) {
                    // 2013/11/29 TMEJ 張 タブレット版SMB チーフテクニシャン機能開発 START
                    // 当日リフレッシュの場合、hidSelectedChipIdコントロールをクリアする
                    if (nDays == 0) {
                        $("#hidSelectedChipId").val("");
                    }
                    else {
                        // 2013/11/29 TMEJ 張 タブレット版SMB チーフテクニシャン機能開発 END
                        $("#hidSelectedChipId").val(gOtherDtChipObj.toJsonSting());
                        // 2013/11/29 TMEJ 張 タブレット版SMB チーフテクニシャン機能開発 START
                    }
                    // 2013/11/29 TMEJ 張 タブレット版SMB チーフテクニシャン機能開発 END
                } else {
                    // 選択したチップのデータがあれば
                    if (gArrObjChip[gSelectedChipId]) {
                        // メインストールチップの場合
                        $("#hidSelectedChipId").val(gArrObjChip[gSelectedChipId].toJsonSting());
                    }
                }
            }
        }
    } else if (gSelectedChipId == C_OTHERDTCHIPID) {
        // 2013/11/29 TMEJ 張 タブレット版SMB チーフテクニシャン機能開発 START
        // 当日リフレッシュの場合、hidSelectedChipIdコントロールをクリアする
        if (nDays == 0) {
            $("#hidSelectedChipId").val("");
        }
        else {
            // 2013/11/29 TMEJ 張 タブレット版SMB チーフテクニシャン機能開発 END
            $("#hidSelectedChipId").val(gOtherDtChipObj.toJsonSting());
            // 2013/11/29 TMEJ 張 タブレット版SMB チーフテクニシャン機能開発 START
        }
        // 2013/11/29 TMEJ 張 タブレット版SMB チーフテクニシャン機能開発 END
        
    } else if ($("#" + C_MOVINGUNAVALIABLECHIPID).length == 1) {

        // 2013/11/29 TMEJ 張 タブレット版SMB チーフテクニシャン機能開発 START
        // 当日リフレッシュの場合、hidSelectedChipIdコントロールをクリアする
        if (nDays == 0) {
            $("#hidSelectedChipId").val("");
        }
        else {
            // 2013/11/29 TMEJ 張 タブレット版SMB チーフテクニシャン機能開発 END
            // 移動不可エリアが選択中
            var nRow = GetRowNoByChipId(C_MOVINGUNAVALIABLECHIPID);
            var strStallId = $(".stallNo" + nRow)[0].id;

            var objUnavailableChip = new UnavailableChip();
            objUnavailableChip.setStallIdleId(right(gSelectedChipId, gSelectedChipId.length - C_UNAVALIABLECHIPID.length));
            objUnavailableChip.setStallId(right(strStallId, strStallId.length - 8));
            var nSelectedChipWidth;
            if ($("#" + gSelectedChipId).data("IDLE_TIME")) { 
                nSelectedChipWidth = Math.round(C_CELL_WIDTH / 15 * parseInt($("#" + gSelectedChipId).data("IDLE_TIME")) - 1);
            } else {
                nSelectedChipWidth = $("#" + C_MOVINGUNAVALIABLECHIPID).width();
            }
            objUnavailableChip.setWidth(nSelectedChipWidth);
            objUnavailableChip.setRowLockVersion($("#" + gSelectedChipId).data("ROW_LOCK_VERSION"));
            //2017/09/16 NSK 竹中(悠) REQ-SVT-TMT-20161109-001 SMB iPadにストールクローズ機能の追加 START
            objUnavailableChip.setIdleMemo(document.getElementById(gSelectedChipId).getElementsByTagName("div").item(2).textContent);
            //2017/09/16 NSK 竹中(悠) REQ-SVT-TMT-20161109-001 SMB iPadにストールクローズ機能の追加 END
            // 使用不可エリアの属性がJsonStringでhidSelectedChipIdに保存する
            $("#hidSelectedChipId").val(objUnavailableChip.toJsonSting());
            // 2013/11/29 TMEJ 張 タブレット版SMB チーフテクニシャン機能開発 START
        }
        // 2013/11/29 TMEJ 張 タブレット版SMB チーフテクニシャン機能開発 END
    }

    // 日付を変更する時、遷移チップが無い場合、選択テーブルの選択状態を解除する
    if (gOtherDtChipObj == null) {
        // 受付ボックスのチップを選択した場合、テーブルの選択状態を解除しない
        if (!((gArrObjSubChip[gSelectedChipId])
            && (gArrObjSubChip[gSelectedChipId].subChipAreaId == C_RECEPTION))) {
            // テーブルの選択状態を解除する
            SetTableUnSelectedStatus();
        }
    }

    var dtShowDate = new Date($("#hidShowDate").val());
    // クリックした日付を計算する
    dtShowDate.setDate(dtShowDate.getDate() + nDays);
    // ハイドンコントロールに最新の日付を設定する
    $("#hidShowDate").val(dtShowDate.getFullYear() + "/" + add_zero((dtShowDate.getMonth() + 1)) + "/" + add_zero(dtShowDate.getDate()));
    // フッターボタンが未選択に戻すか
    // 2013/11/29 TMEJ 張 タブレット版SMB チーフテクニシャン機能開発 START
    var backSelectedChipId = "";
    //if (gSelectedChipId) {
    if ((gSelectedChipId) && (nDays != 0)) {
        // 2013/11/29 TMEJ 張 タブレット版SMB チーフテクニシャン機能開発 END
        //サブチップの場合
        if (gArrObjSubChip[gSelectedChipId]) {
            //受付以外のサブエリアのチップの場合、
            if ((gArrObjSubChip[gSelectedChipId].subChipAreaId != C_RECEPTION) && (gArrObjSubChip[gSelectedChipId].subChipAreaId != C_NOSHOW) && (gArrObjSubChip[gSelectedChipId].subChipAreaId != C_STOP)) {
                CreateFooterButton(C_FT_DISPTP_UNSELECTED, 0);    //フッター部を未選択状態にする
                // 2013/11/29 TMEJ 張 タブレット版SMB チーフテクニシャン機能開発 START
            } else {
                // RemoveAllStallChipsでgSelectedChipIdをクリアしないように
                backSelectedChipId = gSelectedChipId;
                // 2013/11/29 TMEJ 張 タブレット版SMB チーフテクニシャン機能開発 END
            }
        }

        //メインストールチップの場合
        if (gArrObjChip[gSelectedChipId]) {
            //作業中チップとグレーチップの場合
            if (((IsDefaultDate(gArrObjChip[gSelectedChipId].rsltStartDateTime) == false) && (IsDefaultDate(gArrObjChip[gSelectedChipId].rsltEndDateTime) == true))
                || (IsDefaultDate(gArrObjChip[gSelectedChipId].rsltEndDateTime) == false)) {
                CreateFooterButton(C_FT_DISPTP_UNSELECTED, 0);    //フッター部を未選択状態にする
            }
        }

        //コピーされたチップの場合、
        if (gSelectedChipId == C_COPYCHIPID) {

            // 2019/08/09 NSK 鈴木 [TKM]UAT-0488 ストールに配備する前に日付を切り替えると、リレーションチップが消える START
            // 日付変更すると未配置リレーションチップ（透明チップ）が消えるので、現在の未配置リレーションチップ（透明チップ）情報を退避
            $("#hidSelectedChipId").val(gCopyChipObj.toJsonSting());
            // 2019/08/09 NSK 鈴木 [TKM]UAT-0488 ストールに配備する前に日付を切り替えると、リレーションチップが消える END

            CreateFooterButton(C_FT_DISPTP_UNSELECTED, 0);    //フッター部を未選択状態にする
        }
    } else {
        CreateFooterButton(C_FT_DISPTP_UNSELECTED, 0);
    }

    // 渡す引数
    var jsonData = {
            Method: "ReShowMainArea",
            ShowDate: $("#hidShowDate").val()
        };

    setTimeout(function () {
        DoCallBack(C_CALLBACK_WND101, jsonData, SC3240101AfterCallBack, jsonData.Method);
    }, 0);

    //メインストールのチップを削除
    RemoveAllStallChips();

	// 2013/11/29 TMEJ 張 タブレット版SMB チーフテクニシャン機能開発 START
    // 日付を変える時、受付、NOSHOW、中断エリアのチップ選択した場合、gSelectedChipIdをクリアしない
    if (backSelectedChipId != "") {
        gSelectedChipId = backSelectedChipId;
    }
    // 2013/11/29 TMEJ 張 タブレット版SMB チーフテクニシャン機能開発 END
}

// 2013/11/29 TMEJ 張 タブレット版SMB チーフテクニシャン機能開発 START
/**
* 差分リフレッシュ（PUSHがくると画面ドラッグでリフレッシュ）
* @return {なし}
*/
function DiffRefresh() {

    // 日付を変える時、全てストールを表示する
    if (gShowLaterStallFlg == true) {
        // もとに戻す
        ShowAllStall();
        }       

    // 全てポップアップを閉じる
    HideAllPopup();

    // グルグルを表示する
    gMainAreaActiveIndicator.show();

    //2015/07/30 TMEJ 明瀬 アクティビティインジケータが消えない問題 START

    //リフレッシュタイマーセット
    RefreshMainWndTimer(ReDisplay);

    //画面更新中フラグをtrue(更新中)に設定
    gUpdatingDisplayFlg = true;

    //2015/07/30 TMEJ 明瀬 アクティビティインジケータが消えない問題 END

    // テーブルの選択状態を解除する
    SetTableUnSelectedStatus();
    // チップ選択状態を解除する
            SetChipUnSelectedStatus();
    // 日跨ぎ移動用のhiddenコントロールをクリアする
    $("#hidSelectedChipId").val("");

    //フッター部を未選択状態にする
    CreateFooterButton(C_FT_DISPTP_UNSELECTED, 0);

    // 2014/09/12 TMEJ 張 BTS-390 「1ｽﾄｰﾙで2ﾁｯﾌﾟが作業中（表示のみ）」対応 START
    //統一で全部DoCallBackの中でやる
//    // リフレッシュ用更新日時を取得
//    var dtPreUpdateDatetime = GetPreRefreshDate();
    // 2014/09/12 TMEJ 張 BTS-390 「1ｽﾄｰﾙで2ﾁｯﾌﾟが作業中（表示のみ）」対応 END

    jsonData = {
        Method: "ReShowMainAreaFromTheTime",
        ShowDate: $("#hidShowDate").val()
        // 2014/09/12 TMEJ 張 BTS-390 「1ｽﾄｰﾙで2ﾁｯﾌﾟが作業中（表示のみ）」対応 START
//        PreRefreshDateTime: dtPreUpdateDatetime
        // 2014/09/12 TMEJ 張 BTS-390 「1ｽﾄｰﾙで2ﾁｯﾌﾟが作業中（表示のみ）」対応 END
    };

    setTimeout(function () {
        DoCallBack(C_CALLBACK_WND101, jsonData, SC3240101AfterCallBack, jsonData.Method);
    }, 0);

        //非稼働エリアのクリア
        $("." + C_UNAVALIABLECHIPID).remove();
        $(".RestArea").remove();
        //仮仮チップのクリア
        $(".KARIKARI").remove();
        //白い枠を削除
        $(".WhiteBorder").remove();
    }                                       

/**
* 全てポップアップを閉じる
* @return {なし}
*/
function HideAllPopup() {

    // 2014/09/12 TMEJ 張 BTS-390 「1ｽﾄｰﾙで2ﾁｯﾌﾟが作業中（表示のみ）」対応 START
    //    // 詳細画面を閉じる
    //    // CloseChipDetailは使えない、使えば、エラーになって、部分の操作をここに移行する
    //    if ($("#ChipDetailPopup").css("display") == "block") {
    //        $("#DetailSMaintenanceTypeList").remove();
    //        $("#DetailSMercList").remove();
    //        $("#DetailLMaintenanceTypeList").remove();
    //        $("#DetailLMercList").remove();
    //        $("#ChipDetailPopup").fadeOut(300);
    //        //ポップアップが閉じるときは縮小表示にしておく
    //        ShrinkDisplay(0);
    //        //タイマーをクリア(画面が表示し終わる前に閉じられた場合の対応)
    //        commonClearTimer();
    //    }

    // 詳細画面を閉じる
    HideChipDetailPopup();
    // 2014/09/12 TMEJ 張 BTS-390 「1ｽﾄｰﾙで2ﾁｯﾌﾟが作業中（表示のみ）」対応 END

    // 休憩エリアポップアップウィンドウまたは中断ポップアップウィンドウが表示される場合、閉じる
    if ($(".popRestWindow").css("display") == "block") {
        $(".popRestWindow").css("display", "none");
    }
    if ($(".popStopWindowBase").css("display") == "block") {
        CancelStopWindow();
    }

    // テクニシャンウィンドウが表示される時、閉じる
    if ($(".popTechnicianWindow").css("display") == "block") {
        $(".popTechnicianWindow .DataListTable")[0].innerHTML = "";
        $(".popTechnicianWindow").fadeOut(300);
    }
}

// 2013/11/29 TMEJ 張 タブレット版SMB チーフテクニシャン機能開発 END

/**
* 詳細ポップアップを閉じる
* @return {なし}
*/
function HideChipDetailPopup() {

    // 詳細画面を閉じる
    // CloseChipDetailは使えない、使えば、エラーになって、部分の操作をここに移行する
    if ($("#ChipDetailPopup").css("display") == "block") {
        $("#DetailSMaintenanceTypeList").remove();
        $("#DetailSMercList").remove();
        $("#DetailLMaintenanceTypeList").remove();
        $("#DetailLMercList").remove();
        $("#ChipDetailPopup").fadeOut(300);
        //ポップアップが閉じるときは縮小表示にしておく
        ShrinkDisplay(0);
        //タイマーをクリア(画面が表示し終わる前に閉じられた場合の対応)
        commonClearTimer();
    }
}


/**
* 曜日の取得
* @return {曜日}
*/
function GetDay(temp) {

    var day;
    switch (temp) {
        case 0:
            //日曜日  
            day = gSC3240101WordIni[3];
            break;
        case 1:
            //月曜日  
            day = gSC3240101WordIni[4];
            break;
        case 2:
            day = gSC3240101WordIni[5];
            break;
        case 3:
            day = gSC3240101WordIni[6];
            break;
        case 4:
            day = gSC3240101WordIni[7];
            break;
        case 5:
            day = gSC3240101WordIni[8];
            break;
        case 6:
        default:
            //土曜日
            day = gSC3240101WordIni[9];
            break;
    }
    return day;
}

/**
* タップイベント発生時のX座標のポジション取得
*/
function getTapXPosition() {

    var xPos = 0;
    if (event.changedTouches !== undefined && event.changedTouches) {
        //iPad
        xPos = event.touches[0].pageX;
    } else {
        //PC
        xPos = event.pageX;
    }
    return xPos;
}

// 2013/11/29 TMEJ 張 タブレット版SMB チーフテクニシャン機能開発 START
/**
* タップイベント発生時のY座標のポジション取得
*/
function getTapYPosition() {

    var yPos = 0;
    if (event.changedTouches !== undefined && event.changedTouches) {
        //iPad
        yPos = event.touches[0].pageY;
    } else {
        //PC
        yPos = event.pageY;
    }
    return yPos;
}
// 2013/11/29 TMEJ 張 タブレット版SMB チーフテクニシャン機能開発 END

/**
* 時間より、コラム番目を取得する
* @param {Date} 時間
* @return {Integer} コラム番目
*/
function GetColNoByTime(dtTime) {

    var dtShowDate = new Date($("#hidShowDate").val());
    var nCompareValue = CompareDate(dtShowDate, dtTime);
    // 当ページ以外の場合0を戻す
    if (nCompareValue < 0) {
        return 0;
    }
    if (nCompareValue > 0) {
        return gMaxCol + 1;
    }
    var nHours = dtTime.getHours();
    var nMinutes = dtTime.getMinutes();
    nHours = nHours + (nMinutes / 60);
    // 仕事未だ開始してない場合
    if (nHours < gStartWorkTime.getHours()) {
        return 1;
    }

    // 仕事終わる時間場合
    var nEndHour = gEndWorkTime.getHours();
    if (gEndWorkTime.getMinutes() > 0) {
        nEndHour += 1;
    }
    if (nHours > nEndHour) {
        return gMaxCol;
    }
    // 今の時間より、時刻線のxposを取得する
    var nXPos = GetXPosByTime(dtTime);
    // xposより
    return Math.floor(nXPos / C_CELL_WIDTH) + 1;
}
/**
* 時刻線がある列番目を取得する
* @param {} 
* @return {なし}
*/
function GetTimeLineColNo() {
    // 今の時間を取得する
    var dtNow = GetServerTimeNow();
    var dtShowDate = new Date($("#hidShowDate").val());
    // nStartColの値を設定する
    // 当ページの日付と今の日付と比べる
    var nCompareValue = CompareDate(dtShowDate, dtNow);
    // 明日以降の場合
    if (nCompareValue > 0) {
        return C_DATE_AFTER_TOMMORROW;
    }
    // 昨日以前の場合、
    if (nCompareValue < 0) {
        return C_DATE_BEFORE_YESTDAY;
    }

    return GetColNoByTime(dtNow);
}

/**
* 2つ日付の差を取得
* @param {Date} 比べる時間1
* @param {Date} 比べる時間2
* @return {Integer} 2つ日付の差
*/
function GetOffsetDays(dtStartTime, dtEndTime) {

    var dt1 = new Date(dtStartTime);
    var dt2 = new Date(dtEndTime);

    dt1.setHours(0);
    dt1.setMinutes(0);
    dt1.setSeconds(0);
    dt1.setMilliseconds(0);

    dt2.setHours(0);
    dt2.setMinutes(0);
    dt2.setSeconds(0);
    dt2.setMilliseconds(0);

    return Math.abs(dt2 - dt1) / (1000 * 60 * 60 * 24);
}
/**
* 背景色を初期化
* @return {なし}
*/
function ResetBkColor() {
    $("#ColorDiv1").removeClass(C_BK_COLOR_GRAY + " " + C_BK_COLOR_WHITE).css("display", "none");
    $("#ColorDiv2").removeClass(C_BK_COLOR_GRAY + " " + C_BK_COLOR_WHITE).css("display", "none");
    $("#ColorDiv3").removeClass(C_BK_COLOR_GRAY + " " + C_BK_COLOR_WHITE).css("display", "none");
}

/**
* 画面リフレッシュ
* @return {なし}
*/
function RefreshSMB() {

    // チップが選択中、画面定期リフレッシュしない
    if (gSelectedChipId) {
        return;
    }

    // サブエリアが開いている時、画面定期リフレッシュしない
    if (gOpenningSubBoxId != "") {
        return;
    }

    // 2013/11/29 TMEJ 張 タブレット版SMB チーフテクニシャン機能開発 START
    // サブエリアが開いている時、画面定期リフレッシュしない
    if ($(".popTechnicianWindow").css("display") != "none") {
        return;
    }

    //2015/07/30 TMEJ 明瀬 アクティビティインジケータが消えない問題 START

    //画面更新中の場合
    // ・初期表示中
    // ・手動更新中
    // ・Push更新中
    // ・定期更新中
    if (gUpdatingDisplayFlg == true) {
        return;
    }

    //2015/07/30 TMEJ 明瀬 アクティビティインジケータが消えない問題 END

    // 2013/11/29 TMEJ 張 タブレット版SMB チーフテクニシャン機能開発 END
    // 画面スクロールできない
    gCanScrollFlg = false;

    // 2013/11/29 TMEJ 張 タブレット版SMB チーフテクニシャン機能開発 START
    // リフレッシュ
//    ClickChangeDate(0);
    DiffRefresh();
    // 2013/11/29 TMEJ 張 タブレット版SMB チーフテクニシャン機能開発 END
}

// 2013/11/29 TMEJ 張 タブレット版SMB チーフテクニシャン機能開発 START
/**
* テクニシャン選択ダイアログボックスを表示する
* @param {Integer} 行数目
* @param {Integer} 遅れチップだけ表示される場合、タップした行数目
* @return {なし}
*/
function ShowTechnicianDialog(nRow, nLaterRow) {
    // 休憩エリアの日付ウィンドウ
    $(".popTechnicianWindow").fadeIn(300);
    $(".popTechnicianWindow .gradationBox .ArrowMaskUpper").css("display", "block");
    $(".popTechnicianWindow .gradationBox .ArrowMaskBelow").css("display", "none");
    // グルグルを表示する
    gTechnicianActiveIndicator.show();

    // 表示されてる1行目の行数目を取得する
    var nScrollTop = $("#divScrollStall").position().top;

    // 三角の位置を計算する
    // 相対の行数目を取得する
    var nRelativeRowNo;

    if (gShowLaterStallFlg) {
        nRelativeRowNo = nLaterRow - Math.floor(Math.abs(nScrollTop) / C_CELL_HEIGHT);
    } else {
        nRelativeRowNo = nRow - Math.floor(Math.abs(nScrollTop) / C_CELL_HEIGHT);
    }

    // 行数目より、矢印のtop位置を設定する
    var nArrowMaskTop = (nRelativeRowNo - 1) * C_CELL_HEIGHT + 20;

    var nScrollHeight = nScrollTop % C_CELL_HEIGHT;
    if (nScrollHeight != 0) {

        // 三角位置が一番下行を超える場合(表示される一番下行にタップする場合)
        if (nArrowMaskTop > 531) {
            nArrowMaskTop -= C_CELL_HEIGHT;
            nScrollHeight += C_CELL_HEIGHT;
        }

        // 1行目を全部表示するようにスクロールする
        $(".ChipArea_trimming").SmbFingerScroll({
            action: "move", moveY: nScrollHeight, moveX: 0
        });
    }


    $(".popTechnicianWindow .ArrowMask").css("top", nArrowMaskTop).css("left", "-17px");

    // タップ行番号を記録する
    $(".popTechnicianWindow").data("ROWNO", nRow);

    // 0.5秒で無効にする
    gCancelFlg = true;
    setTimeout(function () { gCancelFlg = false; }, 500);
}

/**
* テクニシャンウィンドウに「キャンセル」ボタンを押す
* @return {なし}
*/
function CancelTechnicianWindow() {
    // 無効フラグがtrueの場合、クリックを無効にする
    if (gCancelFlg) {
        gCancelFlg = false;
        return;
    }
    // テクニシャン内容をクリアする
    $(".popTechnicianWindow .DataListTable")[0].innerHTML = "";

    // 非表示
    $(".popTechnicianWindow").fadeOut(300);
}
/**
* テクニシャンウィンドウに「登録」ボタンを押す
* @return {なし}
*/
function ConfirmTechnicianWindow() {
    // 無効フラグがtrueの場合、クリックを無効にする
    if (gCancelFlg) {
        gCancelFlg = false;
        return;
    }

    // 選択されたストールidを取得する
    var nSelectedRowNo = $(".popTechnicianWindow").data("ROWNO");
    var strStallId = GetStallIdByRowNo(nSelectedRowNo);

    // チェックされたテクニシャンのアカウントを取得する
    var objChecked = $(".popTechnicianWindow .Check");
    // テクニシャンが4名以上の場合、エラーメッセージを表示する
    if (objChecked.length > 4) {
        ShowSC3240101Msg(924);
        return;
    }
     
    var arrCheckedStaffCode = new Array();
    for (var nLoop = 0; nLoop < objChecked.length; nLoop++) {
        arrCheckedStaffCode.push(objChecked[nLoop].firstChild.id);
    }

    // 2つarrayの同じ部分を削除する
    for(var i = gArrTapStallTechnician.length - 1; i >= 0; i--){
        for(var j = arrCheckedStaffCode.length - 1; j >= 0; j--){
            if(arrCheckedStaffCode[j] == gArrTapStallTechnician[i]) {
                gArrTapStallTechnician.splice(i, 1); 
                arrCheckedStaffCode.splice(j, 1); 
            }
        }
    }

    var strDeletedStaffCode = "";
    var strAddStaffCode = "";
    var strAddRowlockVersion = "";
    var strDeletedRowlockVersion = "";
    // arrCheckedStaffCodeに残ったのは新しいチェックしたスタッフ
    for (var nLoop = 0; nLoop < arrCheckedStaffCode.length; nLoop++) {
        strAddStaffCode += arrCheckedStaffCode[nLoop] + ",";
        strAddRowlockVersion += FindRowLockVersionInArray(arrCheckedStaffCode[nLoop]) + ",";
    }
    // gArrTapStallTechnicianに残ったのはチェックを外したスタッフ
    for (var nLoop = 0; nLoop < gArrTapStallTechnician.length; nLoop++) {
        strDeletedStaffCode += gArrTapStallTechnician[nLoop] + ",";
        strDeletedRowlockVersion += FindRowLockVersionInArray(gArrTapStallTechnician[nLoop]) + ",";
    }

    // 最後のコンマを削除する
    if (strAddStaffCode.length > 0) {
        strAddStaffCode = strAddStaffCode.substring(0, strAddStaffCode.length - 1);
        strAddRowlockVersion = strAddRowlockVersion.substring(0, strAddRowlockVersion.length - 1);
    }
    if (strDeletedStaffCode.length > 0) {
        strDeletedStaffCode = strDeletedStaffCode.substring(0, strDeletedStaffCode.length - 1);
        strDeletedRowlockVersion = strDeletedRowlockVersion.substring(0, strDeletedRowlockVersion.length - 1);
    }

    // テクニシャン内容をクリアする
    $(".popTechnicianWindow .DataListTable")[0].innerHTML = "";
    // 非表示
    $(".popTechnicianWindow").fadeOut(300);

    // 変更された項目があれば、サーバに発信する
    if ((strAddStaffCode.length > 0) || (strDeletedStaffCode.length > 0)) {
        var jsonData = {
            Method: "SetStallTechnicians",
            StallId: strStallId,
            AddTechnicianAccount: strAddStaffCode,
            DeleteTechnicianAccount: strDeletedStaffCode,
            AddStaffRowLockVersion: strAddRowlockVersion,
            DeleteStaffRowLockVersion: strDeletedRowlockVersion
        };

        // コールバック開始
        DoCallBack(C_CALLBACK_WND101, jsonData, SC3240101AfterCallBack, jsonData.Method);
    }
}

/**
* gArrObjStallリストからスタッフコードにより、行ロックバージョンを取得する
* @param {String} スタッフコード
* @return {String} 行ロックバージョン
*/
function FindRowLockVersionInArray(strStaffCode) {
    for (var nLoop = 0; nLoop < gArrObjStall.length; nLoop++) {
        if (strStaffCode == gArrObjStall[nLoop].STF_CD) {
            return gArrObjStall[nLoop].ROW_LOCK_VERSION;
        }
    }
    return "";
}

/**
* テクニシャンウィンドウにデータを表示するように
* @param {String} テクニシャン情報
* @return {なし}
*/
function SetTechnicianWndData(strTechnicianInfo) {
    // テクニシャンが非表示場合、何もしない
    if ($(".popTechnicianWindow").css("display") == "none") {
        return;
    }

    // グルグルを隠す
    gTechnicianActiveIndicator.hide();

    // jsonデータのstringをリストに変える
    var listStall = $.parseJSON(strTechnicianInfo);

    // クリア
    var objStartDiv = $(".popTechnicianWindow .DataListTable");
    objStartDiv[0].innerHTML = "";

    // タップした行番号を取得する
    var nRowNo = $(".popTechnicianWindow").data("ROWNO");
    var strStallId = GetStallIdByRowNo(nRowNo);

    // リセット
    var nIndex = 1;
    gArrTapStallTechnician = new Array();
    // 選択ストールのテクニシャンまず表示する
    for (var nLoop = 0; nLoop < gArrObjStall.length; nLoop++) {
        // チェックマークが表示されるか
        if (strStallId == gArrObjStall[nLoop].STALLID) {
            objStartDiv.append(CreateLi(gArrObjStall[nLoop].STF_CD, gArrObjStall[nLoop].USERNAME, true, false, "", nIndex++));
            gArrTapStallTechnician.push(gArrObjStall[nLoop].STF_CD);
        }
    }
    // ストールに振り当てないテクニシャンを表示する
    for (var nLoop = 0; nLoop < gArrObjStall.length; nLoop++) {
        // チェックマークが表示されるか
        if ((gArrObjStall[nLoop].STALLID == "0")
            || (gArrObjStall[nLoop].STALLID == "")) {
            objStartDiv.append(CreateLi(gArrObjStall[nLoop].STF_CD, gArrObjStall[nLoop].USERNAME, false, false, "", nIndex++));
        }
    }
    // 振当てったテクニシャンを表示する
    for (var nLoop = 0; nLoop < gArrObjStall.length; nLoop++) {
        // チェックマークが表示されるか
        if ((strStallId != gArrObjStall[nLoop].STALLID)
            && (gArrObjStall[nLoop].STALLID != "0")
            && (gArrObjStall[nLoop].STALLID != "")) {
            objStartDiv.append(CreateLi(gArrObjStall[nLoop].STF_CD, gArrObjStall[nLoop].USERNAME, false, true, gArrObjStall[nLoop].STALLNAME_S, nIndex));
        } 
    }

    // 12個以上の場合、スクロールできる
    if (gArrObjStall.length > 12) {
        $(".popTechnicianWindow .DataListTable ").SC3240101StopmemoFingerscroll();
        $(".popTechnicianWindow .DataListTable  .scroll-inner").height(44 * gArrObjStall.length);
    }
}

/**
* Liを作成
* @param {String} ストールID
* @param {String} テクニシャン名前
* @param {Bool}　チェックマークつけるフラグ
* @param {Bool} Grey色で表示するフラグ
* @return {string} Liのhtml
*/
function CreateLi(strStaffId, strStaffName, bCheckMark, bGreyMark, strStallName, nIndex) {
    var strClass = "onclick='SelectTechnicianArea(" + nIndex + ")'";

    if (bCheckMark) {
        strClass = "class='Check' onclick='SelectTechnicianArea(" + nIndex + ")'";
    } else {
        if (bGreyMark) {
            strClass = "class='Grey'";
            strStallName = "<span class='RightPad'>" + strStallName + "</span>";
        }
    }

    return "<li " + strClass + "><span id='" + strStaffId + "'>" + strStaffName + "</span>" + strStallName + "</li>";
}
/**
* 行番号により、行idを取得する(stallId)
* @return {stallId}
*/
function GetStallIdByRowNo(nRowNo) {
    var strStallId = $(".stallNo" + nRowNo)[0].id;
    return strStallId.substring(8, strStallId.length);
}

/**
* 休憩するかどうかの選択項目にタップする
* @param {Integer} タップの項目順番
* @return {なし}
*/
function SelectTechnicianArea(nIndex) {
    // 色が変更される
    if ($(".popTechnicianWindow .dataBox .innerDataBox .DataListTable li:eq(" + (nIndex - 1) + ")").hasClass("Check")) {
        $(".popTechnicianWindow .dataBox .innerDataBox .DataListTable li:eq(" + (nIndex - 1) + ")").removeClass("Check");
    } else {
        $(".popTechnicianWindow .dataBox .innerDataBox .DataListTable li:eq(" + (nIndex - 1) + ")").addClass("Check");
    }
}
// 2013/11/29 TMEJ 張 タブレット版SMB チーフテクニシャン機能開発 END

/**
* 休憩エリアを表示する
* @param {String}　休憩エリアのID(ポップアップウィンドウの矢印が指すエリア)
* @param {Integer} どの操作タイプで休憩ウィンドウ表示される
* @return {なし}
*/
function ShowRestTimeDialog(strRestTimeId, nType) {
    // 休憩エリアの日付ウィンドウ
    $(".popRestWindow").css("display", "block");
    $(".popRestWindow .gradationBox .ArrowMaskUpper").css("display", "block");
    $(".popRestWindow .gradationBox .ArrowMaskBelow").css("display", "none");
 
    // 元に戻す
    SelectRestArea(0);
    // 休憩エリアの表示位置を設定
    SetRestTimeDlgPosition(strRestTimeId);
    // ポップアップタイプを設定する
    gPopupRestType = nType;
    // 0.5秒で無効にする
    gCancelFlg = true;
    setTimeout(function () { gCancelFlg = false; }, 500);
}

/**
* 休憩エリアの表示位置を設定
* @return {なし}
*/
function SetRestTimeDlgPosition(strRestTimeId) {
    var nMaxTop = 507;
    var nMaxRight = 674;
    var nMinLeft = 0;
    // 休憩エリアの行目を取得する
    var nRowNo = GetRowNoByChipId(strRestTimeId);
    // 休憩エリアの中点を取得する
    var nXPos = $("#" + strRestTimeId).position().left + ($("#" + strRestTimeId).width() / 2);

    // ウィンドウ縦位置の計算
    var nTop = 69 + C_CELL_HEIGHT * nRowNo + $(".scroll-inner").position().top;
    // 最大値を超える場合
    if (nTop > nMaxTop) {
        // 最後の2行ではない場合
        if (nRowNo < gMaxRow - 1) {
            // 最大値にスクロール
            var nScroll = nTop - nMaxTop;
            // 縦方向でスクロール
            $(".ChipArea_trimming").SmbFingerScroll({
                action: "move", moveY: nScroll, moveX: 0
            });
            nTop = nMaxTop;
        } else {
            // 最後の2行の場合、ウィンドウがチップの上に表示する
            nTop -= 273;
            // 矢印もウィンドウの下で表示される
            $(".popRestWindow .gradationBox .ArrowMaskUpper").css("display", "none");
            $(".popRestWindow .gradationBox .ArrowMaskBelow").css("display", "block");
            // 矢印がディフォルト位置に設定する
            $(".popRestWindow .gradationBox .ArrowMaskBelow").css("left", 151);
        }
    }

    var nLeft = nXPos - ($(".popRestWindow").width() / 2) + $(".scroll-inner").position().left + 156;

    if (nLeft < nMinLeft) {
        // スクリーンの左を超える場合、右へスクロールする
        var nScroll = nMinLeft - nLeft;
        // 横方向でスクロール
        $(".ChipArea_trimming").SmbFingerScroll({
            action: "move", moveY: 0, moveX: -nScroll
        });
        nLeft = nMinLeft;
    } else if (nLeft > nMaxRight) {
        // スクリーンの左を超える場合、右へスクロールする
        var nScroll = nLeft - nMaxRight;
        // 横方向でスクロール
        $(".ChipArea_trimming").SmbFingerScroll({
            action: "move", moveY: 0, moveX: nScroll
        });
        nLeft = nMaxRight;

        // もう一回休憩エリアの中心座標を取得する
        nXPos = $("#" + strRestTimeId).offset().left + ($("#" + strRestTimeId).width() / 2);
        // 矢印の座標を設定する
        var nArrowLeft = nXPos - nLeft - 19;
        $(".popRestWindow .gradationBox .ArrowMaskBelow").css("left", nArrowLeft);
    }

    // 値の設定
    $(".popRestWindow").css({ "top": nTop, "left": nLeft });
}

/**
* 休憩ウィンドウに「登録」ボタンを押す
* @return {なし}
*/
function ConfirmRestWindow() {
    // 当ページの日付
    $(".popRestWindow").css("display", "none");

    // 画面の選択項目より、休憩フラグを取得する
    var nRestFlg = C_RESTTIMEGETFLG_NOGETREST;
    // 休憩をする場合
    if ($(".popRestWindow .dataBox .innerDataBox .DataListTable li:first").hasClass("Check")) {
        nRestFlg = C_RESTTIMEGETFLG_GETREST;
    }

    switch (gPopupRestType) {
        case C_ACTION_MOVE:
            // チップ移動
            MoveChip(nRestFlg);
            break;
        case C_ACTION_START:
            // チップ開始
            StartChip(nRestFlg);
            break;
        case C_ACTION_STOP:
            // 2014/07/15 TMEJ 張 タブレットSMB Job Dispatch機能開発 START
            //            // チップ中断
            //            ShowStopDialog(); 
            //            $(".popStopWindowBase").data("RESTFLG", nRestFlg);

            $(".popStopWindowBase").data("RESTFLG", nRestFlg);

            // dbを更新する
            // 渡す引数
            var jsonData = {
                Method: "HasBeforeStartJob",
                JobDtlId: gArrObjChip[gSelectedChipId].jobDtlId
            };

            // グルグルを表示する
            gMainAreaActiveIndicator.show();

            //コールバック開始
            DoCallBack(C_CALLBACK_WND101, jsonData, SC3240101AfterCallBack, jsonData.Method);
            // 2014/07/15 TMEJ 張 タブレットSMB Job Dispatch機能開発 END
           
            break;
        case C_ACTION_FINISH:
            // チップ終了
            FinishChip(nRestFlg);
            break;
        case C_ACTION_MIDFINISH:
            // チップ日跨ぎ終了
            MidfinishChip(nRestFlg);
            break;
        case C_ACTION_SUBCHIPMOVE:
            // サブチップ移動
            MoveSubChip(nRestFlg);
            break;
    }
}
/**
* 休憩ウィンドウに「キャンセル」ボタンを押す
* @return {なし}
*/
function CancelRestWindow() {
    // 無効フラグがtrueの場合、クリックを無効にする
    if (gCancelFlg) {
        gCancelFlg = false;
        return;
    }
    // 非表示
    $(".popRestWindow").css("display", "none");
}
/**
* 休憩するかどうかの選択項目にタップする
* @param {Integer} タップの項目順番
* @return {なし}
*/
function SelectRestArea(nIndex) {
    // 色が変更される
    $(".popRestWindow .dataBox .innerDataBox .DataListTable li").removeClass("Check");
    if (nIndex == 0) {
        $(".popRestWindow .dataBox .innerDataBox .DataListTable li:first").addClass("Check");
    } else {
        $(".popRestWindow .dataBox .innerDataBox .DataListTable li:eq(1)").addClass("Check");
    }
}

// 2014/07/15 TMEJ 張 タブレットSMB Job Dispatch機能開発 START
/**
* 中断エリアを表示する(工程管理画面から)
* @param {Integer} ストール待ち時間エリア表示フラグ
* @return {なし}
*/
function ShowStallStopDialog(bShowTime) {

    // 当ページの日付
    $(".popStopWindowBase").fadeIn(300);

    // ディフォルト値の設定
    // 時間の初期化
    $(".popStopWindowBase .StopTimeLabel").html("0" + gSC3240101WordIni[27]);
    // メモエリアとウィンドウスクロールの初期化
    $(".popStopWindowBase #txtStopMemo").val("").css("height", 100);
    $("#StopMemoScrollBox .scroll-inner").height($("#StopMemoScrollBox .innerDataBox").height());
    $("#StopMemoScrollBox .scroll-inner").css({ "transform": "translate3d(0px, 0px, 0px)" });
    // 選択ボックスの初期化
    document.getElementById("dpDetailStopMemo").selectedIndex = 0;
    $("#lblDetailStopMemo").text($("#dpDetailStopMemo")[0].options[0].text);
    // 中断理由のディフォルト値
    SelectStopArea(0);

    // 中断理由ウィンドウの位置を調整する
    SetStopDlgPosition(gSelectedChipId, false);

    // 表示しない場合
    if (!bShowTime) {
        $(".popStopWindowBase .TableWorkingHours").css({ "display": "none" });
        $("#CustomLabel32").css({ "display": "none" });
        $("#lblDetailStopMemo").css({ "top": "198px" });
    } else {
        $(".popStopWindowBase .TableWorkingHours").css({ "display": "block" });
        $("#CustomLabel32").css({ "display": "block" });
        $("#lblDetailStopMemo").css({ "top": "273px" });
    }
}
// 2014/07/15 TMEJ 張 タブレットSMB Job Dispatch機能開発 END

/**
* 中断エリアを表示する
* @param {boolean} 詳細画面呼び出しフラグ
* @return {なし}
*/
function ShowStopDialog(blDetailFlg) {

    // 当ページの日付
    //2014/07/15 TMEJ 丁 タブレットSMB Job Dispatch機能開発 START
    //$(".popStopWindowBase").css("display", "block");
    $(".popStopWindowBase").fadeIn(300);
    //2014/07/15 TMEJ 丁 タブレットSMB Job Dispatch機能開発 END

    // ディフォルト値の設定
    // 時間の初期化
    $(".popStopWindowBase .StopTimeLabel").html("0" + gSC3240101WordIni[27]);
    // メモエリアとウィンドウスクロールの初期化
    $(".popStopWindowBase #txtStopMemo").val("").css("height", 100);
    $("#StopMemoScrollBox .scroll-inner").height($("#StopMemoScrollBox .innerDataBox").height());
    $("#StopMemoScrollBox .scroll-inner").css({ "transform": "translate3d(0px, 0px, 0px)" });
    // 選択ボックスの初期化
    document.getElementById("dpDetailStopMemo").selectedIndex = 0;
    $("#lblDetailStopMemo").text($("#dpDetailStopMemo")[0].options[0].text);
    // 中断理由のディフォルト値
    SelectStopArea(0);

    // 中断理由ウィンドウの位置を調整する
    SetStopDlgPosition(gSelectedChipId, blDetailFlg);

    // 休憩フラグを設定してない
    $(".popStopWindowBase").data("RESTFLG", C_RESTTIMEGETFLG_NOSET);
}
/**
* 中断理由ウィンドウの位置を調整する
* @param {Integer} 選択したチップID
* @param {boolean} 詳細画面呼び出しフラグ
* @return {なし}
*/
//2014/07/15 TMEJ 丁 タブレットSMB Job Dispatch機能開発 START
//function SetStopDlgPosition(strChipId) {
function SetStopDlgPosition(strChipId, blDetailFlg) {
//2014/07/15 TMEJ 丁 タブレットSMB Job Dispatch機能開発 END

    // ウィンドウの左座標の最小値
    var nMinLeft = 156;
    var nMaxLeft = 656;
    // 選択したチップの左座標
    var nChipLeft = $("#" + strChipId).offset().left;
    var nChipWidth = $("#" + strChipId).width();
    var nChipRight = nChipLeft + nChipWidth;

    var nDlgWidth = $(".popStopWindowBase").width();

    var nLeft = -1;

    //2014/07/15 TMEJ 丁 タブレットSMB Job Dispatch機能開発 START
    if (blDetailFlg) {
    //チップ詳細用中断ボップアップ
        SetStopDlgPositionByChipDetail();
        
   } else {
    //2014/07/15 TMEJ 丁 タブレットSMB Job Dispatch機能開発 END
        // 横座標設定
        // 左部分で直接表示できる場合
        if (nChipLeft - nDlgWidth - 17 >= nMinLeft) {
            nLeft = nChipLeft - nDlgWidth - 17;
            $(".popStopWindowBase .gradationBox .ArrowMaskL").css("display", "none");
            $(".popStopWindowBase .gradationBox .ArrowMaskR").css("display", "block");
        }
        // 未設定且つ右部分で表示できる場合
        if ((nChipRight <= nMaxLeft) && (nLeft == -1)) {
            nLeft = nChipRight + 17;
            $(".popStopWindowBase .gradationBox .ArrowMaskL").css("display", "block");
            $(".popStopWindowBase .gradationBox .ArrowMaskR").css("display", "none");
        }

        // 直接表示できない場合、ディフォルト位置で表示する
        if (nLeft == -1) {
            $(".popStopWindowBase .gradationBox .ArrowMaskL").css("display", "none");
            $(".popStopWindowBase .gradationBox .ArrowMaskR").css("display", "block");
            nLeft = 276;
        }

        // 縦標設定
        var nTop = -1;
        // ウィンドウの上と下辺の座標
        var nMinTop = 63;
        var nMaxTop = 648;
        // チップの中心座標
        var nRowNo = GetRowNoByChipId(strChipId);
        var nChipTop = $(".Row" + nRowNo).offset().top - C_CHIPAREA_OFFSET_TOP;
        var nChipBottom = nChipTop + $(".Row" + nRowNo).height();
        var nArrowTop;
        // 中心点が中断理由の縦の座標の外の場合、ウィンドウのtop座標を調整する
        if (nChipTop < 95) {
            nTop = 40;
            // 半分チップしか表示されない、全部表示するようにスクロール
            if (nChipTop < nMinTop) {
                // 最大値にスクロール
                var nScroll = nMinTop - nChipTop;
                // 縦方向でスクロール
                $(".ChipArea_trimming").SmbFingerScroll({
                    action: "move", moveY: -nScroll, moveX: 0
                });
                // 矢印の座標を調整する
                nChipTop = $(".Row" + nRowNo).offset().top - C_CHIPAREA_OFFSET_TOP; ;
            }
        } else if (nChipBottom > 600) {
            nTop = 145;
            // 半分チップしか表示されない、全部表示するようにスクロール
            if (nChipBottom > nMaxTop) {
                // 最大値にスクロール
                var nScroll = nChipBottom - nMaxTop;
                // 縦方向でスクロール
                $(".ChipArea_trimming").SmbFingerScroll({
                    action: "move", moveY: nScroll, moveX: 0
                });
                // 矢印の座標を調整する
                nChipTop = $(".Row" + nRowNo).offset().top - C_CHIPAREA_OFFSET_TOP; ;
            }
        } else {
            // ディフォルトtop座標
            nTop = 95;
        }
        // 矢印の座標を調整する
        nArrowTop = nChipTop - nTop + 6;
        $(".popStopWindowBase .gradationBox .ArrowMaskL").css("top", nArrowTop);
        $(".popStopWindowBase .gradationBox .ArrowMaskR").css("top", nArrowTop);

        //2014/07/15 TMEJ 丁 タブレットSMB Job Dispatch機能開発 START
        //$(".popStopWindowBase").css({ "top": nTop, "left": nLeft });
        $(".popStopWindowBase").css({ "top": nTop, "left": nLeft, "z-index": 1001 });
        //2014/07/15 TMEJ 丁 タブレットSMB Job Dispatch機能開発 END
    }
}

//2014/07/15 TMEJ 丁 タブレットSMB Job Dispatch機能開発 START
/**
* グレーフィルターにタップするイベント
* @return {なし}
*/
function BindLoadingScreenTapEvent() {
    // チップ選択解除イベント発行
    $("#MstPG_LoadingScreen").unbind().bind("chipTap", function (e) {
        // 非表示
        $(".popStopWindowBase").fadeOut(300);
        gMainAreaActiveIndicator.hide();
        $("#MstPG_LoadingScreen").unbind();
    });
}

/**
* 中断ボップアップの位置調整(チップ詳細から)
* @return {なし}
*/
function SetStopDlgPositionByChipDetail() {

    var nDlgWidth = $(".popStopWindowBase").width();
    var nChipAreaWidth = $(".ChipArea_OutFrame").width();
    var nSetDlgLeft = nChipAreaWidth / 2 - nDlgWidth / 2 + $(".NameList").width() / 2;
    var nChipAreaHeight = $(".ChipArea_OutFrame").height();
    var nDlgHeight = $(".popStopWindowBase").height();
    var nSetDlgTop = nChipAreaHeight / 2 - nDlgHeight / 2;
    $(".popStopWindowBase").css({ "top": nSetDlgTop, "left": nSetDlgLeft, "z-index": 1001 });
    $(".popStopWindowBase .gradationBox .ArrowMaskL").css("display", "none");
    $(".popStopWindowBase .gradationBox .ArrowMaskR").css("display", "none");
    $("#MstPG_LoadingScreen").css({ "width": $(window).width() + "px", "height": $(window).height() + "px" });
    $("#MstPG_LoadingScreen").css({ "display": "table" });
    $("#MstPG_LoadingScreen .loadingIcn").css({ "display": "none" });

    if (gStopChipFig=="0") {
        $(".popStopWindowBase .TableWorkingHours").css({ "display": "none" });
        $("#CustomLabel32").css({ "display": "none" });
        $("#lblDetailStopMemo").css({ "top": "198px" });
    } else {
        $(".popStopWindowBase .TableWorkingHours").css({ "display": "block" });
        $("#CustomLabel32").css({ "display": "block" });
        $("#lblDetailStopMemo").css({ "top": "273px" });
    }
    BindLoadingScreenTapEvent();
}

//2014/07/15 TMEJ 丁 タブレットSMB Job Dispatch機能開発 END

/**
* 中断理由ウィンドウに「キャンセル」ボタンを押す
* @return {なし}
*/
function CancelStopWindow() {
    // 非表示
    //2014/07/15 TMEJ 丁 タブレットSMB Job Dispatch機能開発 START
    //$(".popStopWindowBase").css("display", "none");
    $(".popStopWindowBase").fadeOut(300);
    gMainAreaActiveIndicator.hide();
    $("#MstPG_LoadingScreen").unbind();
    //2014/07/15 TMEJ 丁 タブレットSMB Job Dispatch機能開発 END
}
/**
* 中断理由ウィンドウに「登録」ボタンを押す
* @return {なし}
*/
function ConfirmStopWindow() {

    var strStopReasonType;
    if ($(".popStopWindowBase .dataBox .innerDataBox .DataListTable li:first").hasClass("Check")) {
        // 部品欠品
        strStopReasonType = C_STOPREASONTYPE_STOCKOUT;
    } else if ($(".popStopWindowBase .dataBox .innerDataBox .DataListTable li:eq(1)").hasClass("Check")) {
        // 顧客承認待ち
        strStopReasonType = C_STOPREASONTYPE_WAITCONFIRMED;
    } else {
        // その他
        strStopReasonType = C_STOPREASONTYPE_OTHER; 
    }

    // 中断時間
    var nStopTime = parseInt($(".popStopWindowBase .StopTimeLabel").html());
    // 中断メモ
    var strStopMemo = $("#txtStopMemo").val();
    // 非表示
    $(".popStopWindowBase").css("display", "none");

    // 休憩フラグを取得する
    var nRestFlg = $(".popStopWindowBase").data("RESTFLG");

    //2014/07/15 TMEJ 丁 タブレットSMB Job Dispatch機能開発 START   

//    // 中断操作を行う
//    if (nRestFlg == C_RESTTIMEGETFLG_NOSET) {
//        StopJob(strStopReasonType, nStopTime, strStopMemo, null);
//    } else {
//        StopJob(strStopReasonType, nStopTime, strStopMemo, nRestFlg);
//    }

    var nDispWindow = GetDisplayPopupWindow();
    if (nDispWindow == C_DISP_DETAIL) {
        StopByChipDetail(strStopReasonType, nStopTime, strStopMemo, null)
    } else {
        
        // StallWaitTimeエリア非表示場合(未開始のJobがあるので、中断後、赤作業中チップになる)
        if ($("#CustomLabel32").css("display") == "none") {
            // 赤作業中チップになる
            if (nRestFlg == C_RESTTIMEGETFLG_NOSET) {
                StopJobExceptBeforeJob(strStopReasonType, strStopMemo, null);
            } else {
                StopJobExceptBeforeJob(strStopReasonType, strStopMemo, nRestFlg);
            }
        } else {
            // グレー中断チップになる
            if (nRestFlg == C_RESTTIMEGETFLG_NOSET) {
                StopJob(strStopReasonType, nStopTime, strStopMemo, null);
            } else {
                StopJob(strStopReasonType, nStopTime, strStopMemo, nRestFlg);
            }
        }

    }
    //2014/07/15 TMEJ 丁 タブレットSMB Job Dispatch機能開発 END

}

/**
* 中断理由選択項目にタップする
* @param {Integer} タップの項目順番
* @return {なし}
*/
function SelectStopArea(nIndex) {
    // 色が変更される
    $(".popStopWindowBase .dataBox .innerDataBox .DataListTable li").removeClass("Check");
    if (nIndex == 0) {
        $(".popStopWindowBase .dataBox .innerDataBox .DataListTable li:first").addClass("Check");
    } else if (nIndex == 1) {
        $(".popStopWindowBase .dataBox .innerDataBox .DataListTable li:eq(1)").addClass("Check");
    } else {
        $(".popStopWindowBase .dataBox .innerDataBox .DataListTable li:eq(2)").addClass("Check");
    }
}

/**
* 中断時間を変更する
* @param {Integer} 変える時間
* @return {なし}
*/
function ChangeStopMinutes(nChangeMinutes) {
    var nMinutes = parseInt($(".popStopWindowBase .StopTimeLabel").html());
    // 最小値、最大値を超える場合、
    if ((nMinutes + nChangeMinutes < 0)
        || (nMinutes + nChangeMinutes > C_MAXSTOPTIME)) {
        return;
    }
    nMinutes += nChangeMinutes;
    $(".popStopWindowBase .StopTimeLabel").text(nMinutes + gSC3240101WordIni[27]);
}
/**
* 中断時間エリアにタップして、テクストボックスを表示する
* @return {なし}
*/
function ClickStopTime() {

    var nMinutes = parseInt($(".popStopWindowBase .StopTimeLabel").html());
    $(".popStopWindowBase .StopTimeLabel").css("display", "none");
    // 2014/07/15 TMEJ 張 タブレットSMB Job Dispatch機能開発 START
    $(".popStopWindowBase #StopTimeTxt").parent().css("display", "inline-block");
    // 2014/07/15 TMEJ 張 タブレットSMB Job Dispatch機能開発 END
    $(".popStopWindowBase #StopTimeTxt").css({ "display": "block" ,
                                               "font-size": 14,
                                               "font-weight": "bold",
                                               "line-height": "22px"})
                                        .val(nMinutes)
                                        .focus();


                                           
}

/**
* 作業チップが重複している休憩チップのIDを取得
* @param {String} strChipId  指定チップid
* @return {array} 重複している休憩、使用不可エリアのID
*/
function GetRestTimeInServiceTime(strChipId) {

    // Movingチップ左座標と右座標を記録する
    var nMovingChipLeft = $("#" + strChipId).position().left;
    var nMovingChipRight = nMovingChipLeft + $("#" + strChipId).width();
    var nRowNo = GetRowNoByChipId(strChipId);
    // return用array
    var arrDuplChipId = GetRestTimeInRange(nRowNo, nMovingChipLeft, nMovingChipRight);

    return arrDuplChipId;
}

/**
* 固定範囲に休憩チップのIDを取得
* @param {Integer} 行目
* @param {Integer} 開始座標
* @param {Integer} 終了座標
* @return {array} 範囲内の休憩、使用不可エリアのID
*/
function GetRestTimeInRange(nRowNo, nLeft, nRight) {

    // return用array
    var arrDuplChipId = new Array();
    var nLoop = 0;

    // この行に全てチップを取得する
    $(".Row" + nRowNo).children("div").each(function (index, e) {
        // 2019/06/18 NSK 皆川 (トライ店システム評価)次世代サービスオペレーション効率化に向けた、業務適合性検証 START
//        // 重複のは休憩、使用不可エリアの場合
//        if ((IsUnavailableArea(e.id)) || (IsRestArea(e.id))) {
//            var nChipLeft = e.offsetLeft;
//            var nChipRight = e.offsetLeft + e.offsetWidth;
//            // 重複のチップのIDと左座標を記録する
//            if (!((nChipRight < nLeft) || (nChipLeft > nRight))) {
//                arrDuplChipId[nLoop] = new Array(e.id, nChipLeft);
//                nLoop++;
//            }
//        }

        // 重複するチップが休憩の場合
        if (IsRestArea(e.id)) {
            var nChipLeft = e.offsetLeft;
            var nChipRight = e.offsetLeft + e.offsetWidth;
            // 重複のチップのIDと左座標を記録する
            if (!((nChipRight < nLeft) || (nChipLeft > nRight))) {
                arrDuplChipId[nLoop] = new Array(e.id, nChipLeft);
                nLoop++;
            }
        }
        // 2019/06/18 NSK 皆川 (トライ店システム評価)次世代サービスオペレーション効率化に向けた、業務適合性検証 END
    });

    // left座標よりソートする
    arrDuplChipId.sort(function (x, y) { return x[1] - y[1] });

    return arrDuplChipId;
}

/**
* 固定範囲に普通のチップのIDを取得
* @param {Integer} 行目
* @param {Integer} 開始座標
* @param {Integer} 終了座標
* @return {array} 範囲内の休憩、使用不可エリアのID
*/
function GetDuplChipIdInRange(nRowNo, nLeft, nRight) {

    // return用array
    var arrDuplChipId = new Array();
    var nLoop = 0;

    // この行に全てチップを取得する
    $(".Row" + nRowNo).children("div").each(function (index, e) {
        // 重複のは休憩、使用不可エリアの場合
        if (gArrObjChip[e.id]) {
            var nChipLeft = e.offsetLeft;
            var nChipRight = e.offsetLeft + e.offsetWidth;
            // 重複のチップのIDと左座標を記録する
            if (!((nChipRight < nLeft) || (nChipLeft > nRight))) {
                arrDuplChipId[nLoop] = new Array(e.id, nChipLeft);
                nLoop++;
            }
        }
    });

    // left座標よりソートする
    arrDuplChipId.sort(function (x, y) { return x[1] - y[1] });

    return arrDuplChipId;
}

/**
* 休憩エリアと重複したチップの幅を取得する
* @param {String} チップID
* @return {Integer} 幅
*/
function GetWidthByRestTime(strChipId) {
    var nChipWidth = $("#" + strChipId).width();
    var nNewChipWidth = nChipWidth;
    var nCompareRestTimeWidth = 0;
    while (1) {
        $("#" + strChipId).css("width", nNewChipWidth);
        // 移動チップと重複の休憩エリアIDを取得する
        var arrRestTime = GetRestTimeInServiceTime(strChipId);
        var nNewRestTimeWidth = GetRestTimeAreaWidth(strChipId, arrRestTime);
        if (nCompareRestTimeWidth == nNewRestTimeWidth) {
            break;
        } else {
            nCompareRestTimeWidth = nNewRestTimeWidth;
            nNewChipWidth = nChipWidth + nNewRestTimeWidth;
        }
    }
    // チップの幅を元に戻す
    $("#" + strChipId).css("width", nChipWidth);
    return nNewChipWidth - nChipWidth;
}
/**
* 休憩エリアと重複した幅を取得する
* @param {array} 休憩エリアのチップIDアレイ
* @param {array} 休憩エリアのチップIDアレイ
* @return {Integer} 幅
*/
function GetRestTimeAreaWidth(strChipId, arrRestTime) {
    var nPlusWidth = 0;
    var nChipLeft = $("#" + strChipId).position().left;
    var nChipWidth = $("#" + strChipId).width();
    var nChipRight = nChipWidth + nChipLeft;

    for (var nLoop = 0; nLoop < arrRestTime.length; nLoop++) {
        var nRestLeft = $("#" + arrRestTime[nLoop][0]).position().left;
        var nRestWidth = $("#" + arrRestTime[nLoop][0]).width();
        var nRestRight = nRestWidth + nRestLeft;

        // 2019/12/06 NSK 皆川 (トライ店システム評価)整備作業における休憩時間取得判定向上検証 START
//        // 休憩エリアがチップに含まれる
//        // またはチップが休憩エリア左にある時
//        if (nChipLeft <= nRestLeft) {
//            // 休憩エリアの幅を追加する
//            nPlusWidth += nRestWidth + 1;
//        } else if ((nRestLeft <= nChipLeft) && (nChipRight <= nRestRight)) {
//            
//            nPlusWidth += nRestRight - nChipLeft;
//        }

        // チップが休憩エリア左にある場合
        if (nChipLeft < nRestLeft) {
            // 休憩エリアの幅を追加する
            nPlusWidth += nRestWidth + 1;
        }

        // 休憩を自動判定しない場合
        if ($("#hidRestAutoJudgeFlg").val() != "1") {
            // チップが休憩チップに含まれる
            if ((nRestLeft <= nChipLeft) && (nChipRight <= nRestRight)) {
            nPlusWidth += nRestRight - nChipLeft;
        }
    }
        // 2019/12/06 NSK 皆川 (トライ店システム評価)整備作業における休憩時間取得判定向上検証 END
    }
    
    return nPlusWidth;
}
/**
* 休憩エリアと重複した場合（休憩エリアが左のほうにある）、チップの新しい左座標を取得する
* @param {array} 休憩エリアのチップIDアレイ
* @param {array} 休憩エリアのチップIDアレイ
* @return {Integer} 幅
*/
function GetLeftByRestTime(strChipId, arrRestTime) {
    // 移動チップと重複の休憩エリアIDを取得する
    var arrRestTime = GetRestTimeInServiceTime(strChipId);
    // 休憩エリアがチップの左にある且つ重複時、チップが休憩エリアの右に移動する
    var nChipLeft = GetLeftByOneRestTime(strChipId, arrRestTime)

    while (1) {
        $("#" + strChipId).css("left", nChipLeft);
        // 移動チップと重複の休憩エリアIDを取得する
        var arrNewRestTime = GetRestTimeInServiceTime(strChipId);
        // 休憩エリアがチップの左にある且つ重複時、チップが休憩エリアの右に移動する
        var nNewChipLeft = GetLeftByOneRestTime(strChipId, arrNewRestTime)
        if (nNewChipLeft == nChipLeft) {
            break;
        } else {
            nChipLeft = nNewChipLeft;
        }
    }
    return nChipLeft;
}

/**
* 休憩エリアと重複した場合（休憩エリアが左のほうにある）、チップの新しい左座標を取得する
* @param {array} 休憩エリアのチップIDアレイ
* @param {array} 休憩エリアのチップIDアレイ
* @return {Integer} 幅
*/
function GetLeftByOneRestTime(strChipId, arrRestTime) {

    var nChipLeft = $("#" + strChipId).position().left;
    if (arrRestTime.length > 0) { 
        var nRestLeft = $("#" + arrRestTime[0][0]).position().left;
        var nRestRight = $("#" + arrRestTime[0][0]).width() + nRestLeft;
        // 休憩エリアとチップの一部を重複してる(休憩エリアが左方にある)
        if (nRestLeft <= nChipLeft) {
            return nRestRight + 1;
        }
    }
    return nChipLeft;
}

/**
* チップを表示ため、スクロールする
* @param {String} チップID
* @return {-} なし
*/
function ScrollToShowChip(strChipId) {
    // 行番号を取得
    var nRowNo = GetRowNoByChipId(strChipId);
    // チップのtop、leftを取得
    var nChipLeft, nChipTop;
    nChipTop = $(".Row" + nRowNo).position().top;
    nChipLeft = $("#" + strChipId).position().left + $(".scroll-inner").position().left;

    // 選択したチップが一番上左上で表示される
    $(".ChipArea_trimming").SmbFingerScroll({
        action: "move",
        moveY: nChipTop,
        moveX: nChipLeft
    });
}
/**
* メインストールスクロール範囲を拡大する(サブエリアが表示される時、メインストールが全部表示できるように)
* @param {bool} bEnlargeFlag 拡大フラグ　true:拡大
* @return {なし}
*/
function ChangeMainScrollHeight(bEnlargeFlag) {

    // 拡大したい時、画面がもう拡大している場合、
    // 縮みたい時、画面がもう縮みている場合、何もしない
    if (bEnlargeFlag == gEnlargeScrollHeight) {
        return;
    }

    // スクロール範囲広く/縮む前の位置を記録する
    var nXPos = $(".scroll-inner").position().left;
    var nYPos = $(".scroll-inner").position().top;
//    nYPos += $(".pullDownToRefresh").position().top;

    var nOffsetHeight = 0;
    // 拡大したい時
    if (bEnlargeFlag) {
        // 遅れチップストールしか表示されない場合
        if (gShowLaterStallFlg) {
            return;
        }

        // 行数により、拡大分が違う
        if (gMaxRow >= 8) {
            nOffsetHeight = C_CHIPAREA_OFFSET_HEIGHT;
        } else if (gMaxRow == 7) {
            nOffsetHeight = C_CHIPAREA_OFFSET_HEIGHT - C_CELL_HEIGHT;
        } else if (gMaxRow == 6) {
            nOffsetHeight = C_CHIPAREA_OFFSET_HEIGHT - C_CELL_HEIGHT * 2;
        }
        
        // スクロール範囲が拡大中フラグ
        gEnlargeScrollHeight = true;
    } else {
        gEnlargeScrollHeight = false;
    }

    // 画面スクロールできる
    $(".ChipArea_trimming").SmbFingerScroll({
        minTop: $(".ChipArea_trimming").height() - $(".ChipArea").height() - nOffsetHeight
    });

    // 画面スクロールできる
    var nHeight = gMaxRow * C_CELL_HEIGHT + C_CHIPAREA_OFFSET_TOP + nOffsetHeight;
    $(".ChipArea_trimming .scroll-inner").height(nHeight);


    // 1行目、1列目にスクロールする
    // 2013/11/29 TMEJ 張 タブレット版SMB チーフテクニシャン機能開発 START
//    $(".ChipArea_trimming").SmbFingerScroll({
//        action: "move",
//        moveX: nXPos,
//        moveY: nYPos
//    });
    $(".ChipArea_trimming").SmbFingerScroll({
        action: "move",
        moveX: 0,
        moveY: 0
    });
    // 2013/11/29 TMEJ 張 タブレット版SMB チーフテクニシャン機能開発 END

    // 記録した位置にスクロールする
    $(".ChipArea_trimming").SmbFingerScroll({
        action: "move",
        moveX: -nXPos,
        moveY: -nYPos
    });
}

// タブレット版SMB チーフテクニシャン機能開発 START
/**
* 使用権限がChTの場合、ストール名をタップすると該当ストールのTCメイン画面に遷移する
* @param {Integer} nStallId ストールID
* @return {なし}
*/
function ClickStall(nStallId) {
    // チーフテクニシャン以外の場合、何も変化しない
    if (gOpeCode != C_OPECODE_CHT) {
        return;
    }
    var strParam = '{'
    strParam += '"StallId":"' + nStallId + '"';
    strParam += '}';

    // グルグルを表示する
    gMainAreaActiveIndicator.show();

    //画面遷移のためポストバック
    __doPostBack("", strParam);
}
// タブレット版SMB チーフテクニシャン機能開発 END

// 2019/06/18 NSK 皆川 (トライ店システム評価)次世代サービスオペレーション効率化に向けた、業務適合性検証 START
/**
* 日跨ぎでないチップの休憩取得フラグを自動判別する
* @param {String} strChipId  指定チップid
* @return {Integer} 休憩取得フラグの自動判別結果
*/
function JudgeRestFlg(strChipId) {
    // 休憩取得フラグ（自動判別）
    var nAutoJudgeRestFlg = C_RESTTIMEGETFLG_GETREST

    // チップに重なるすべての休憩を取得
    var arrRestTime = GetRestTimeInServiceTime(strChipId);

    // 取得結果が存在する場合
    if (1 <= arrRestTime.length) {
        var nAutoJudgeRestFlg = C_RESTTIMEGETFLG_NOGETREST
        var nMovingChipLeft = $("#" + strChipId).position().left;

        // 取得した休憩がすべて「休憩開始時間≦チップの開始時間」の場合、0（休憩取得しない）を返す
        // 1件でも「チップの開始時間<休憩開始時間」の休憩がある場合、1（休憩取得する）を返す
        for (var nLoop = 0; nLoop < arrRestTime.length; nLoop++) {
            var nRestLeft = $("#" + arrRestTime[nLoop][0]).position().left;

            // チップの開始時間 < 休憩開始時間 のデータがある場合
            if (nMovingChipLeft < nRestLeft) {
                nAutoJudgeRestFlg = C_RESTTIMEGETFLG_GETREST
            }
        }
    }

    return nAutoJudgeRestFlg;
}

/**
* 日跨ぎ出ないチップの休憩取得変更ボタン表示有無を判定する
* @param {String} strChipId  指定チップid
* @return {Bool} true:ボタン表示、false:ボタン非表示
*/
function CanRestChange(strChipId) {
    // チップに重なるすべての休憩を取得
    var arrRestTime = GetRestTimeInServiceTime(strChipId);

    // 取得結果が存在する場合
    if (1 <= arrRestTime.length) {
        var nMovingChipLeft = $("#" + strChipId).position().left;

        for (var nLoop = 0; nLoop < arrRestTime.length; nLoop++) {
            var nRestLeft = $("#" + arrRestTime[nLoop][0]).position().left;

            // チップの開始時間 < 休憩開始時間 のデータがある場合
            if (nMovingChipLeft < nRestLeft) {
                return true;
            }
        }
    }

    return false;
}
// 2019/06/18 NSK 皆川 (トライ店システム評価)次世代サービスオペレーション効率化に向けた、業務適合性検証 END