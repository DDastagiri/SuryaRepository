//---------------------------------------------------------
//Chip.js
//---------------------------------------------------------
//機能：チップ
//作成：2012/12/22 TMEJ 張 タブレット版SMB機能開発(工程管理)
//更新：2013/11/29 TMEJ 張 タブレット版SMB チーフテクニシャン機能開発
//更新：2014/06/18 TMEJ 丁 タブレット版SMB テレマ走行距離機能開発
//更新：2014/07/15 TMEJ 張 タブレットSMB Job Dispatch機能開発
//更新：2014/09/11 TMEJ 張 BTS-389 「SMBで異常ではないはずが、遅れ見込み表示になっている」対応
//更新：2014/09/12 TMEJ 張 BTS-390 「1ｽﾄｰﾙで2ﾁｯﾌﾟが作業中（表示のみ）」対応
//更新：2014/12/04 TMEJ 丁 新プラットフォーム版i-CROP仕様変更対応（サービス入庫実績更新処理変更）
//更新：2015/02/09 TMEJ 範 DMS連携版サービスタブレット SMB納車予定時刻通知機能開発
//更新：2015/05/14 TMEJ 小澤 DMS連携版サービスタブレット強制納車機能追加開発
//更新：2015/07/30 TMEJ 明瀬 アクティビティインジケータが消えない問題
//更新：2015/10/02 TM 小牟禮 サブエリアの情報取得処理を2回から1回に変更（性能改善）
//更新：2017/01/17 NSK 竹中 TR-SVT-TMT-20151019-002 TCメイン画面のエラーメッセージが理解不可能
//更新：2017/08/30 NSK 竹中(悠) REQ-SVT-TMT-20161109-001 SMB iPadにストールクローズ機能の追加
//更新：2017/09/05 NSK 小川 REQ-SVT-TMT-20160906-003 計画済みチップをストールから簡単に外せるようにする
//更新：2017/09/25 NSK 竹中(悠) REQ-SVT-TMT-20170227-002 見込納車遅れ日時計算ロジック統一
//更新：2017/10/21 NSK 小川 REQ-SVT-TMT-20160906-003 子チップがキャンセルできない
//更新：2018/07/06 NSK 可児 TKM Next Gen e-CRB Project Application development Block B-1  Pマーク、Lマークなどを表示
//更新：2019/06/18 NSK 皆川 (トライ店システム評価)次世代サービスオペレーション効率化に向けた、業務適合性検証
//更新：2019/08/09 NSK 鈴木 [TKM]UAT-0488 ストールに配備する前に日付を切り替えると、リレーションチップが消える
//更新：2019/07/22 NSK 近藤 (トライ店システム評価)次世代サービスオペレーション効率化に向けた、業務適合性検証
//更新：2019/12/06 NSK 皆川 (トライ店システム評価)整備作業における休憩時間取得判定向上検証
//更新：2020/01/07 NSK 坂本 TR-SVT-TMT-20190118-001 Stop start tagのXMLログが過去日19000101を表示している
//更新：
//---------------------------------------------------------

//チップの生成
//@return {なし}
function CreateChips() {

    gSelectedChipId = "";
    gMovingChipObj = null;
    // 初期化してない場合、初期化する
    if (!gArrObjChip) {
        gArrObjChip = new Array();
    }

    //JSON形式のチップ情報読み込み
    var jsonData = $("#hidJsonData").val();
    $("#hidJsonData").attr("value", "");
    var chipDataList = $.parseJSON(jsonData);

    // チップid
    var arrDuplicateChipsId = new Array();          // 作業中または作業完了チップid(重複可能性がある)
    var strPreJobDtlId = "";
    //取得したチップ情報をチップクラスに格納し、再描画
    for (var strKey in chipDataList) {
        var chipData = chipDataList[strKey];

        var objChip = new ReserveChip(strKey);
        objChip.setChipParameter(chipData);

        // Noshowの場合、表示しない
        if (objChip.stallUseStatus == C_STALLUSE_STATUS_NOSHOW) {
            continue;
        }
        // 2013/11/29 TMEJ 張 タブレット版SMB チーフテクニシャン機能開発 START
        // Tempエリアにある場合、表示しない
        if (objChip.tempFlg == C_TEMP_FLG_ON) {
            continue;
        }
        // 2013/11/29 TMEJ 張 タブレット版SMB チーフテクニシャン機能開発 END

        // このチップが当ページで表示するか
        if ((objChip.displayStartDate >= gEndWorkTime) || (objChip.displayEndDate <= gStartWorkTime)) {
            continue;
        }

        // 2013/11/29 TMEJ 張 タブレット版SMB チーフテクニシャン機能開発 START
        if ($("#stallId_" + objChip.stallId).length == 0) {
            continue;
        }
        // 2013/11/29 TMEJ 張 タブレット版SMB チーフテクニシャン機能開発 END

        strKey = chipData.STALL_USE_ID;
        // 新規の場合
        if (gArrObjChip[strKey] == undefined) {
            gArrObjChip[strKey] = objChip;
            gArrObjChip[strKey].createChip(C_CHIPTYPE_STALL_FASTER);   //チップ生成
            FasterSetChipPosition(gArrObjChip[strKey]);                      //チップ位置を設定
            BindChipClickEvent(strKey); //チップタップ時のイベントを登録
        } else {
            // 更新の場合
            gArrObjChip[strKey] = objChip;
            gArrObjChip[strKey].updateStallChip();
            // 位置をリセットする
            SetChipPosition(strKey, "", "", "");
        }

        // 作業中または作業完了の場合、該チップのidを記録
        if ((((IsDefaultDate(gArrObjChip[strKey].rsltStartDateTime) == false) && (IsDefaultDate(gArrObjChip[strKey].rsltEndDateTime)))
            || (IsDefaultDate(gArrObjChip[strKey].rsltEndDateTime)) == false)) {
            arrDuplicateChipsId.push(strKey);
        }
    }

    // 中断フラグの設定
    var nFirstLoop = true;
    var strPreJobId = "";
    var strPreStallUseId = "";
    for (var strKey in chipDataList) {
        if (nFirstLoop) {
            nFirstLoop = false;
            strPreJobId = chipDataList[strKey].JOB_DTL_ID;
            strPreStallUseId = chipDataList[strKey].STALL_USE_ID;
            continue;
        }
        // 同じ作業内容IDがあれば、中断フラグをtrueに設定
        if (chipDataList[strKey].JOB_DTL_ID == strPreJobId) {
            if (gArrObjChip[chipDataList[strKey].STALL_USE_ID]) {
                gArrObjChip[chipDataList[strKey].STALL_USE_ID].stopFlg = true;
            }
            if (gArrObjChip[strPreStallUseId]) {
                gArrObjChip[strPreStallUseId].stopFlg = true;
            }
        }
        strPreJobId = chipDataList[strKey].JOB_DTL_ID;
        strPreStallUseId = chipDataList[strKey].STALL_USE_ID;
    }

    //作業中または作業完了チップの重複チェック
    for (var strChipId in arrDuplicateChipsId) {
        // 有効のチップデータをチェック
        if (CheckgArrObjChip(arrDuplicateChipsId[strChipId]) == false) {
            continue;
        }
        //重複チップの枠を描画
        CreateWhiteBorders(arrDuplicateChipsId[strChipId]);
    }

    //選択しているチップを表示
    if ($("#hidSelectedChipId").val() != "") {
        jsonData = $("#hidSelectedChipId").val();   //前の日または後の日選択しているチップの情報を取得
        $("#hidSelectedChipId").val("");
        var chipData = $.parseJSON(jsonData);
        //自分のページを戻す
        if ((gArrObjChip[chipData.STALL_USE_ID]) && (!chipData.KEY)) {
            // 2013/11/29 TMEJ 張 タブレット版SMB チーフテクニシャン機能開発 START
//            TapStallChip(chipData.STALL_USE_ID);   //自分が選択される状態にする  
//            ScrollToShowChip(chipData.STALL_USE_ID);
            
            ScrollToShowChip(chipData.STALL_USE_ID);
            TapStallChip(chipData.STALL_USE_ID);   //自分が選択される状態にする  
            // 2013/11/29 TMEJ 張 タブレット版SMB チーフテクニシャン機能開発 END
        } else if ((chipData.STALLIDLEID) && ($("#" + C_UNAVALIABLECHIPID + chipData.STALLIDLEID).length == 1)) {
            TapUnavailableArea(C_UNAVALIABLECHIPID + chipData.STALLIDLEID); //自分が選択される状態にする
            ScrollToShowChip(C_UNAVALIABLECHIPID + chipData.STALLIDLEID);   //初期画面に表示されない場合、下へスクロールして、表示する
            // 左座標を取得する
            var nLeft = $("#" + C_UNAVALIABLECHIPID + chipData.STALLIDLEID).position().left;
            var nRow = GetRowNoByChipId(C_UNAVALIABLECHIPID + chipData.STALLIDLEID);
            // Movingチップを描画する
            drawUnavailableAreaAtPos(nLeft, nRow);
            // Movingチップを非表示にする
            $("#" + C_MOVINGUNAVALIABLECHIPID + " .ShadowBox").css("display", "none");
        } else {
            var nRowNum = 1;    //ディフォルトが1行目
            var strOtherDtChipId;
            // 日跨ぎ移動のは普通チップの場合、
            if (chipData.STALL_USE_ID) {
                gOtherDtChipObj = new ReserveChip(chipData.STALL_USE_ID);
                strOtherDtChipId = chipData.STALL_USE_ID;
            } else {
                // 使用不可エリアの場合
                gOtherDtChipObj = new UnavailableChip(chipData.STALLIDLEID);
                strOtherDtChipId = C_UNAVALIABLECHIPID + chipData.STALLIDLEID;
            }
            
            gOtherDtChipObj.setChipParameter(chipData);
            gOtherDtChipObj.createChip(C_CHIPTYPE_OTHER_DAY);   //チップを表示               
            nRowNum = $("#stallId_" + gOtherDtChipObj.stallId).position().top / C_CELL_HEIGHT + 1;  //チップのストールidより、行目の計算

            //チップの位置とサイズをストール名とテクニシャンのエリアにあわせる
            $("#" + strOtherDtChipId).css("top", $(".stallNo" + nRowNum).position().top + 2 + "px")
                                     .css("left", "7px")
                                     .css("width", "130px")
                                     .css("height", "70px")
                                     .css("opacity", C_OPACITY_TRANSPARENT);

            AdjustChipItemByWidth(strOtherDtChipId);

            //初期画面に表示されない場合、下へスクロールして、表示する
            var nOffsetRows = nRowNum - (($(".ChipArea_trimming").height() - C_CHIPAREA_OFFSET_TOP) / C_CELL_HEIGHT);
            if (nOffsetRows > 0) {
                var nTop = nOffsetRows * C_CELL_HEIGHT;
                $(".ChipArea_trimming").SmbFingerScroll({
                    action: "move", moveY: nTop, moveX: 0
                });
            }

            // 2019/08/09 NSK 鈴木 [TKM]UAT-0488 ストールに配備する前に日付を切り替えると、リレーションチップが消える START
            // //gSelectedChipIdを選択しているチップのIDに設定
            // gSelectedChipId = C_OTHERDTCHIPID;

            // 関連チップでストール未配置の場合
            if ((chipData != null) && (chipData.STALL_USE_ID == "-1")) {
                // 日付変更すると未配置リレーションチップ（透明チップ）が消えるので、チップIDをコピーチップIDとする
                // gSelectedChipIdをコピーチップのIDに設定
                gSelectedChipId = C_COPYCHIPID;

            } else {
            //gSelectedChipIdを選択しているチップのIDに設定
            gSelectedChipId = C_OTHERDTCHIPID;
            }
            // 2019/08/09 NSK 鈴木 [TKM]UAT-0488 ストールに配備する前に日付を切り替えると、リレーションチップが消える END

            //元のchiptapイベントをMovingチップのタップイベントにbind
            $("#" + strOtherDtChipId).unbind().bind("chipTap", function (e) {
                SetChipUnSelectedStatus();  //チップ選択状態を解除
                SetTableUnSelectedStatus();
            });
            SetTableSelectedStatus();
            //時刻線の表示制御
            setRedTimeLineLeftPos(false);
            // 納車予定日時がある場合、
            if ((gOtherDtChipObj.scheDeliDateTime) && (IsDefaultDate(gOtherDtChipObj.scheDeliDateTime) == false)) {

                //2015/03/11 TMEJ 明瀬 既存バグ修正 START
                //// 納車予定日時により、時刻線の位置を取得して、設定する
                //var setPosition = GetTimeLinePosByTime(gOtherDtChipObj.scheDeliDateTime);

                //納車遅れ見込み時間(黄色線の時間)取得
                // 2017/09/25 NSK 竹中(悠) REQ-SVT-TMT-20170227-002 見込納車遅れ日時計算ロジック統一 START
                // var dtDeliDelay = GetDeliDelayExpectedTimeLine(gOtherDtChipObj.carWashNeedFlg
                //                                              , gOtherDtChipObj.cwRsltStartDateTime
                //                                              , gOtherDtChipObj.scheDeliDateTime
                //                                              , gOtherDtChipObj.svcStatus);
                var dtDeliDelay = GetDeliDelayExpectedTimeLine(gOtherDtChipObj.carWashNeedFlg
                                                             , gOtherDtChipObj.scheDeliDateTime
                                                             , gOtherDtChipObj.svcStatus
                                                             , gOtherDtChipObj.remainingInspectionType);
                // 2017/09/25 NSK 竹中(悠) REQ-SVT-TMT-20170227-002 見込納車遅れ日時計算ロジック統一 END
                var setPosition = GetTimeLinePosByTime(dtDeliDelay);

                //2015/03/11 TMEJ 明瀬 既存バグ修正 END

                $(".TimingLineDeli").css("left", setPosition);
                // 表示にする
                $(".TimingLineDeli").css("visibility", "visible");
            }
            CreateFooterButton(C_FT_DISPTP_SELECTED, C_FT_BTNTP_CHANGING_DATE);
        }
    }
    // チップが遅れの場合、赤にする
    SetLaterStallColorRed();
    // 車両番号左寄せ、右寄せを調整する
    setTimeout(function () { ChangeAllStallChipCarNoTextAlign(); }, 0);
}
//情報により、チップを更新する
//@param {String} jsonData Json型のstring
//@param {Bool} bNotCloseDetailFlg 詳細ポップアップ閉じるかどうかフラグ True：閉じない
//@param {Bool} bRowLockVersionFlg リフレッシュ時、行ロックバージョン見る必要フラグ
//@return {なし}
// 2013/11/29 TMEJ 張 タブレット版SMB チーフテクニシャン機能開発 START
//function ShowLatestChips(jsonData) {
function ShowLatestChips(jsonData, bNotCloseDetailFlg, bRowLockVersionFlg) {
    // 2013/11/29 TMEJ 張 タブレット版SMB チーフテクニシャン機能開発 END

    // 2014/09/12 TMEJ 張 BTS-390 「1ｽﾄｰﾙで2ﾁｯﾌﾟが作業中（表示のみ）」対応 START
    // 今、全て操作をしたあと、差分チップ情報を取得するため、
    // bRefreshFlgにTrueをする
    bRefreshFlg = true;
    // 2014/09/12 TMEJ 張 BTS-390 「1ｽﾄｰﾙで2ﾁｯﾌﾟが作業中（表示のみ）」対応 END

    var chipDataList = $.parseJSON(jsonData);
    var arrDuplicateChipsId = new Array();          // 作業中または作業完了チップid(重複可能性がある)

    //取得したチップ情報をチップクラスに格納し、再描画
    for (var strKey in chipDataList) {
        var chipData = chipDataList[strKey];

        var objChip = new ReserveChip(strKey);
        objChip.setChipParameter(chipData);

        // 2013/11/29 TMEJ 張 タブレット版SMB チーフテクニシャン機能開発 START
        // 今の部品出庫ステータスが空白の場合、元の部品出庫ステータスを設定する
        if (objChip.partsFlg == "") {
            if (gArrObjChip[objChip.stallUseId]) {
                objChip.setPartsFlg(gArrObjChip[objChip.stallUseId].partsFlg);
            }
        }
        // 2013/11/29 TMEJ 張 タブレット版SMB チーフテクニシャン機能開発 END

        // 削除された場合
        if (objChip.cancelFlg == "1") {
            if ($("#" + objChip.stallUseId).length > 0) {
                // 今表示される場合、削除する
                RemoveChipFromStall(objChip.stallUseId);
                // 削除したチップが選択中、選択解除
                if (gSelectedChipId == objChip.stallUseId) {
                    //チップ選択状態を解除
                    SetChipUnSelectedStatus();
                    SetTableUnSelectedStatus();
                }
            }

            // 2017/10/21 NSK 小川 REQ-SVT-TMT-20160906-003 子チップがキャンセルできない START
            //チップの情報を関連チップ配列から削除
            DeleteChipFromRelationChipObj(objChip.stallUseId);
            // 2017/10/21 NSK 小川 REQ-SVT-TMT-20160906-003 子チップがキャンセルできない END

            continue;
        }

        // 2013/11/29 TMEJ 張 タブレット版SMB チーフテクニシャン機能開発 START
        // NoshowとTempの場合、表示しない
        //        if (objChip.stallUseStatus == C_STALLUSE_STATUS_NOSHOW) {
        if ((objChip.stallUseStatus == C_STALLUSE_STATUS_NOSHOW)
            || (objChip.tempFlg == C_TEMP_FLG_ON)) {
            // 今表示される場合、削除する
            if ($("#" + objChip.stallUseId).length > 0) {
                // 画面から削除する
                RemoveChipFromStall(objChip.stallUseId);
                // 削除したチップが選択中、選択解除
                if (gSelectedChipId == objChip.stallUseId) {
                    //チップ選択状態を解除
                    SetChipUnSelectedStatus();
                    SetTableUnSelectedStatus();
                }
            }
            // 2013/11/29 TMEJ 張 タブレット版SMB チーフテクニシャン機能開発 END
            continue;
        }
        // このチップが当ページで表示するか
        if ((objChip.displayStartDate >= gEndWorkTime) || (objChip.displayEndDate <= gStartWorkTime)) {
            // 当ページに表示される時、削除するする
            if ($("#" + chipData.STALL_USE_ID).length) {
                RemoveChipFromStall(chipData.STALL_USE_ID);
            }
            continue;
        }

        // 2013/11/29 TMEJ 張 タブレット版SMB チーフテクニシャン機能開発 START
        if ($("#stallId_" + objChip.stallId).length == 0) {
            // 今表示される場合、削除する
            if ($("#" + objChip.stallUseId).length > 0) {
                // 画面から削除する
                RemoveChipFromStall(objChip.stallUseId);
                // 削除したチップが選択中、選択解除
                if (gSelectedChipId == objChip.stallUseId) {
                    //チップ選択状態を解除
                    SetChipUnSelectedStatus();
                    SetTableUnSelectedStatus();
                }
            }
            continue;
        }
        // 2013/11/29 TMEJ 張 タブレット版SMB チーフテクニシャン機能開発 END

        // 2014/09/12 TMEJ 張 BTS-390 「1ｽﾄｰﾙで2ﾁｯﾌﾟが作業中（表示のみ）」対応 START


        // 画面上に該当チップがある場合
        if (gArrObjChip[objChip.stallUseId]) {

            // 元の状態が作業中
            if (gArrObjChip[objChip.stallUseId].stallUseStatus == C_STALLUSE_STATUS_START) {

                // 白い枠がある場合
                if ($("#WB" + objChip.stallUseId).length > 0) {

                    // 削除する
                    $("#WB" + objChip.stallUseId).remove();
                }

            }

        }
        // 2014/09/12 TMEJ 張 BTS-390 「1ｽﾄｰﾙで2ﾁｯﾌﾟが作業中（表示のみ）」対応 END

        strKey = chipData.STALL_USE_ID;
        // 画面にあれば
        if (gArrObjChip[strKey]) {
            // まだ更新してない状態で更新する
            // 2013/11/29 TMEJ 張 タブレット版SMB チーフテクニシャン機能開発 START
//            if (objChip.rowLockVersion >= gArrObjChip[strKey].rowLockVersion) {
            if ((objChip.rowLockVersion >= gArrObjChip[strKey].rowLockVersion)
                || (bRowLockVersionFlg)) {
                // 2013/11/29 TMEJ 張 タブレット版SMB チーフテクニシャン機能開発 END
                //---------------------------------------------------->
                // 下記のソース修正があれば、ShowCreatedRelationChipsに同じ場所もう修正が必要
                // 追加作業ステータスをバックする
                var strBackAddWorkStatus = gArrObjChip[strKey].addWorkStatus;
                gArrObjChip[strKey] = objChip;
                // 新しい追加作業ステータスが空白の場合、更新しない
                if (gArrObjChip[strKey].addWorkStatus.toString().Trim() == "") {
                    // バックした値を戻す
                    gArrObjChip[strKey].setAddWorkStatus(strBackAddWorkStatus);
                }

                var bHasBlackBackFlg = false;
                // 影があるチップにタップする時、バックする
                if ($("#" + strKey + " .Front").hasClass("BlackBack")) {
                    bHasBlackBackFlg = true;
                }
                //<----------------------------------------------------

                // チップを更新する
                gArrObjChip[strKey].updateStallChip();
                // もし影があれば、もう一回かげを追加する
                if (bHasBlackBackFlg) {
                    $("#" + strKey + " .Front").addClass("BlackBack");
                }

                // 位置をリセットする
                SetChipPosition(strKey, "", "", "");
            }
            // 2013/11/29 TMEJ 張 タブレット版SMB チーフテクニシャン機能開発 START
            //}
        } else {
            // リフレッシュの場合、存在してないチップが表示できる
            // 2014/09/12 TMEJ 張 BTS-390 「1ｽﾄｰﾙで2ﾁｯﾌﾟが作業中（表示のみ）」対応 START
//            if (bRefreshFlg) {
            // 2014/09/12 TMEJ 張 BTS-390 「1ｽﾄｰﾙで2ﾁｯﾌﾟが作業中（表示のみ）」対応 END

            gArrObjChip[strKey] = objChip;
            gArrObjChip[strKey].createChip(C_CHIPTYPE_STALL_FASTER);   //チップ生成
            FasterSetChipPosition(gArrObjChip[strKey]);                      //チップ位置を設定
            BindChipClickEvent(strKey); //チップタップ時のイベントを登録

            // 2014/09/12 TMEJ 張 BTS-390 「1ｽﾄｰﾙで2ﾁｯﾌﾟが作業中（表示のみ）」対応 START
//            }
            // 2014/09/12 TMEJ 張 BTS-390 「1ｽﾄｰﾙで2ﾁｯﾌﾟが作業中（表示のみ）」対応 END
        }
        // 2013/11/29 TMEJ 張 タブレット版SMB チーフテクニシャン機能開発 END
    }

    //画面中作業中、作業完了チップのidを全部登録する
    for (var nWork = 0; nWork < $(".StWork").length; nWork++) {
        arrDuplicateChipsId.push($(".StWork")[nWork].id);
    }
    for (var nWComplete = 0; nWComplete < $(".StWComplete").length; nWComplete++) {
        arrDuplicateChipsId.push($(".StWComplete")[nWComplete].id);
    }
    for (var nDComplete = 0; nDComplete < $(".StDComplete").length; nDComplete++) {
        arrDuplicateChipsId.push($(".StDComplete")[nDComplete].id);
    }

    //作業中または作業完了チップの重複チェックして、白い枠をつける
    for (var strChipId in arrDuplicateChipsId) {
        // 有効のチップデータをチェック
        if (CheckgArrObjChip(arrDuplicateChipsId[strChipId]) == false) {
            continue;
        }
        // 今選択中チップが更新したチップ、または被られたチップの場合、白い枠をつける必要がない
        if (gSelectedChipId) {
            var arrDuplChipIds = GetDuplicateChips(arrDuplicateChipsId[strChipId]);
            // 重複チップがある場合、元に戻す
            if (arrDuplChipIds.length > 1) {
                var bSelectedFlg = false;
                for (var nLoop = 0; nLoop < arrDuplChipIds.length; nLoop++) {
                    if (arrDuplChipIds[nLoop][0] == gSelectedChipId) {
                        bSelectedFlg = true;
                        break;
                    }
                }

                if (bSelectedFlg) {
                    continue;
                }
            }
        }
        //重複チップの枠を描画
        CreateWhiteBorders(arrDuplicateChipsId[strChipId]);
    }

    // 選択したチップがリレーションチップの場合、リレーション線を描画する
    if (gSelectedChipId != "") {
        ShowRelationLine(gSelectedChipId);
        // 白枠をグレーにする
        SetTableSelectedStatus();
    }
    // チップが遅れの場合、赤にする
    SetLaterStallColorRed();
    // 2013/11/29 TMEJ 張 タブレット版SMB チーフテクニシャン機能開発 START
    // 車両番号左寄せ、右寄せを調整する
    setTimeout(function () { ChangeAllStallChipCarNoTextAlign(); }, 0);
    // 2013/11/29 TMEJ 張 タブレット版SMB チーフテクニシャン機能開発 END
}

//遅れ見込み時間を更新
//@return {なし}
function RefreshPlanDeliTime(strJsonData) {
    var chipDataList = $.parseJSON(strJsonData);

    //取得したチップ情報をチップクラスに格納し、再描画
    for (var strKey in chipDataList) {
        var chipData = chipDataList[strKey];

        // 遅れ見込み時間を設定
        //2017/09/25 NSK 竹中(悠) REQ-SVT-TMT-20170227-002 見込納車遅れ日時計算ロジック統一 START
        //SetPlanDeliTime(chipData.SVCIN_ID, chipData.DELI_DELAY_DATETIME);
        SetPlanDeliTime(chipData.SVCIN_ID, chipData.DELI_DELAY_DATETIME, chipData.REMAINING_INSPECTION_TYPE);
        //2017/09/25 NSK 竹中(悠) REQ-SVT-TMT-20170227-002 見込納車遅れ日時計算ロジック統一 END
    }
    // チップが遅れの場合、赤にする
    SetLaterStallColorRed();
}

//遅れ見込み時間を設定
//@return {なし}
//2017/09/25 NSK 竹中(悠) REQ-SVT-TMT-20170227-002 見込納車遅れ日時計算ロジック統一 START
//function SetPlanDeliTime(nSvcinId, dtPlanDeli) {
function SetPlanDeliTime(nSvcinId, dtPlanDeli, strInspection) {
    //2017/09/25 NSK 竹中(悠) REQ-SVT-TMT-20170227-002 見込納車遅れ日時計算ロジック統一 END
    for (var strId in gArrObjChip) {
        // 2013/11/29 TMEJ 張 タブレット版SMB チーフテクニシャン機能開発 START
        if (CheckgArrObjChip(strId) == false) {
            continue;
        }
        // 2013/11/29 TMEJ 張 タブレット版SMB チーフテクニシャン機能開発 END
        if (gArrObjChip[strId].svcInId == nSvcinId) {
            gArrObjChip[strId].setPlanDelayDate(dtPlanDeli);

            //2017/09/25 NSK 竹中(悠) REQ-SVT-TMT-20170227-002 見込納車遅れ日時計算ロジック統一 START
            gArrObjChip[strId].setRemainingInspectionType(strInspection);
            //2017/09/25 NSK 竹中(悠) REQ-SVT-TMT-20170227-002 見込納車遅れ日時計算ロジック統一 END
        }
    }
}

//生成されたリレーションコピーチップを最新のデータで更新する
function ShowCreatedRelationChips(jsonData) {
    var chipDataList = $.parseJSON(jsonData);

    //取得したチップ情報をチップクラスに格納し、再描画
    for (var strKey in chipDataList) {
        var chipData = chipDataList[strKey];

        // 削除したチップの場合、次のループをする
        if (chipData.CANCEL_FLG == "1") {
            continue;
        }
        var objChip = new ReserveChip(strKey);
        objChip.setChipParameter(chipData);

        // Noshowの場合、表示しない
        if (objChip.stallUseStatus == C_STALLUSE_STATUS_NOSHOW) {
            continue;
        }

        // 2013/11/29 TMEJ 張 タブレット版SMB チーフテクニシャン機能開発 START
        // Tempエリアにある場合、表示しない
        if (objChip.tempFlg == C_TEMP_FLG_ON) {
            continue;
        }
        // 2013/11/29 TMEJ 張 タブレット版SMB チーフテクニシャン機能開発 END

        // このチップが当ページで表示するか
        if ((objChip.displayStartDate >= gEndWorkTime) || (objChip.displayEndDate <= gStartWorkTime)) {
            continue;
        }

        // 2013/11/29 TMEJ 張 タブレット版SMB チーフテクニシャン機能開発 START
        if ($("#stallId_" + objChip.stallId).length == 0) {
            continue;
        }
        // 2013/11/29 TMEJ 張 タブレット版SMB チーフテクニシャン機能開発 END

        strKey = chipData.STALL_USE_ID;
        // 画面にあれば
        if (gArrObjChip[strKey]) {
            // まだ更新してない状態で更新する
            if (objChip.rowLockVersion >= gArrObjChip[strKey].rowLockVersion) {

                //---------------------------------------------------->
                // 下記のソース修正があれば、ShowLatestChipsに同じ場所もう修正が必要
                // 追加作業ステータスをバックする
                var strBackAddWorkStatus = gArrObjChip[strKey].addWorkStatus;
                gArrObjChip[strKey] = objChip;
                // 新しい追加作業ステータスが空白の場合、更新しない
                if (gArrObjChip[strKey].addWorkStatus.toString().Trim() == "") {
                    // バックした値を戻す
                    gArrObjChip[strKey].setAddWorkStatus(strBackAddWorkStatus);
                }

                gArrObjChip[strKey] = objChip;
                // 元がグレーで表示中、updateした後もグレーにする
                var bBlackFlg = false;
                if ($("#" + strKey + " .Front").hasClass("BlackBack")) {
                    bBlackFlg = true;
                }
                //<----------------------------------------------------
                gArrObjChip[strKey].updateStallChip();
                // グレーにする
                if (bBlackFlg) {
                    $("#" + strKey + " .Front").addClass("BlackBack");
                }
                // 位置をリセットする
                SetChipPosition(strKey, "", "", "");
            }
        } else {
            // チップの位置により、生成されたリレーションコピーチップですかを判断する
            var strCopyedChipId = GetCopyedChipId(objChip.stallId, objChip.scheStartDateTime, objChip.scheWorkTime);
            if (strCopyedChipId != "") {
                // 最新のリレーションチップ構造体を生成する
                gArrObjChip[strKey] = new ReserveChip(strKey);
                objChip.copy(gArrObjChip[strKey]);
                // 白いプラスマークがあれば、
                if ($("#" + strCopyedChipId + " .ICAddWork").length == 1) {
                    gArrObjChip[strKey].setAddWorkStatus(C_AW_ADDINGWORK);
                } else if ($("#" + strCopyedChipId + " .ICWaitCommited").length == 1) {
                    gArrObjChip[strKey].setAddWorkStatus(C_AW_WAIT_COMMITTED);
                }

                // 画面にチップIDをC_COPYCHIPID→stalluseidに変える
                $("#" + strCopyedChipId).attr("id", strKey);

                // 元がグレーで表示中、updateした後もグレーにする
                var bBlackFlg = false;
                if ($("#" + strKey + " .Front").hasClass("BlackBack")) {
                    bBlackFlg = true;
                }

                // チップ更新すう
                gArrObjChip[strKey].updateStallChip();
                // グレーにする
                if (bBlackFlg) {
                    $("#" + strKey + " .Front").addClass("BlackBack");
                }
                // 位置をリセットする
                SetChipPosition(strKey, "", "", "");
                // 元のリレーションチップの構造体をクリア
                gArrObjChip[strCopyedChipId] = null;
                //チップタップ時のイベントを登録
                BindChipClickEvent(strKey); 
            }
        }
    }
    // 選択したチップがリレーションチップの場合、リレーション線を描画する
    if (gSelectedChipId != "") {
        ShowRelationLine(gSelectedChipId);
    }
    SetLaterStallColorRed();

    // サブエリアを更新する(ボタンの数字、色をリフレッシュ)
    AllButtonRefresh();
}

//ストールid、開始時間、終了時間により、コピーされたチップを探す
//@return {なし}
function GetCopyedChipId(nStallId, dtStartTime, nWorkTime) {
    for (var nLoop = 0; nLoop < C_ARR_COPYEDCHIPID.length; nLoop++) {
        // 一時コピー用チップがあれば、
        if (gArrObjChip[C_ARR_COPYEDCHIPID[nLoop]]) {
            // 一時コピーチップが探されば、
            if ((nStallId == gArrObjChip[C_ARR_COPYEDCHIPID[nLoop]].stallId)
                && (dtStartTime - gArrObjChip[C_ARR_COPYEDCHIPID[nLoop]].scheStartDateTime == 0)
                && (nWorkTime - gArrObjChip[C_ARR_COPYEDCHIPID[nLoop]].scheWorkTime == 0)) {
                // IDを戻す
                return C_ARR_COPYEDCHIPID[nLoop];
            }
        }
    }
    return "";
}

//全チップを削除
//@return {なし}
function RemoveAllStallChips() {
    gSelectedChipId = "";
    gSelectedCellId = "";
    gWBChipId = "";
    gMovingChipObj = null;
    gOtherDtChipObj = null;
    gArrBackChipObj.length = 0;
    gShowLaterStallFlg = false;
    gPopupBoxId = "";

    //メインエリアのチップ、白い枠、ポップアップボックス、リレーション線を削除
    $(".MCp").remove();
    $(".WhiteBorder").remove();
    $(".PopUpChipBoxBorder").remove();
    $(".RelationalLine").remove();
    //非稼働エリアのクリア
    $("." + C_UNAVALIABLECHIPID).remove();
    $(".RestArea").remove();

    gArrObjChip = new Array();  //gArrObjChipをクリア
}

//チップタップ
//@param {String} strChipId チップID
//@return {なし}
function TapStallChip(strChipId) {
    var strSelectedChipId;
    var strsubchipFlg = 0;

    var scrollMainObjPos = $(".ChipArea_trimming").find(".scroll-inner").position();
    gTranslateValStallX = scrollMainObjPos.left;
    gTranslateValStallY = scrollMainObjPos.top;

    // リレーションコピー中、返事も帰らない時、このチップをタップすると、無視する
    if (IsCopyedChip(strChipId) && (!gCopyChipObj)) {
        return;
    }

    // 2014/09/12 TMEJ 張 BTS-390 「1ｽﾄｰﾙで2ﾁｯﾌﾟが作業中（表示のみ）」対応 START
    // タップ不可の場合、戻す
    if ($("#" + strChipId).data(C_DATA_CHIPTAP_FLG) == false) {

        return;

    }
    // 2014/09/12 TMEJ 張 BTS-390 「1ｽﾄｰﾙで2ﾁｯﾌﾟが作業中（表示のみ）」対応 END


    //2014/07/15 TMEJ 張 タブレットSMB Job Dispatch機能開発 START
    //    //配置不可チップをタップすると、反応がない
    //    if (($("#" + strChipId + " .Front").hasClass("BlackBack") == true)
    //        && (!gArrObjSubChip[gSelectedChipId])) {

    //修正原因：受付エリアの追加作業を選択して、ストール上のチップ(暗いの子チップ)をタップして、Movingチップが生成された
    //配置不可チップをタップすると、反応がない
    if ($("#" + strChipId + " .Front").hasClass("BlackBack") == true) {
        //2014/07/15 TMEJ 張 タブレットSMB Job Dispatch機能開発 END

        return;
    }

    //選択したチップがあれば
    if (gSelectedChipId != "") {
        //前タップチップはサブチップの場合
        if (gArrObjSubChip[gSelectedChipId]) {
            //ストール×時間の空き時間をタップしてから予約をタップしても何も反応しない
            if (($("#MovingChip").css("left")) && ($("#MovingChip").css("left") != gSubMovingChipLeft)) {
                return;
            }
            //関連あるストールチップをタップの場合（受付チップのみ）
            if ((gArrObjSubChip[gSelectedChipId].svcInId == gArrObjChip[strChipId].svcInId)
                && (gArrObjSubChip[gSelectedChipId].subChipAreaId == C_RECEPTION)) {
                //ROの場合
                strsubchipFlg = 1;
            } else if ((gArrObjSubChip[gSelectedChipId].subChipAreaId == C_RECEPTION) || (gArrObjSubChip[gSelectedChipId].subChipAreaId == C_NOSHOW) || (gArrObjSubChip[gSelectedChipId].subChipAreaId == C_STOP)) {
                //受付、NOSHOW、中断エリアの場合何もしない
                return;
            } else {
                //受付以外のサブチップ
                // チップ選択状態を解除する
                SetChipUnSelectedStatus();
                // 選択したチップを解放する
                SetTableUnSelectedStatus();
                // サブチップボックス閉じる
                SetSubChipBoxClose();
                CreateFooterButton(C_FT_DISPTP_UNSELECTED, 0);
                return;
            }
        } else {
            var strUpDataType = GetChipUpdatetype(strChipId);
            if (strUpDataType == C_UPDATA_SUBCHIP) {
                return;
            }
        }
        // 2013/11/29 TMEJ 張 タブレット版SMB チーフテクニシャン機能開発 START
        //今回タッチしたのは新規チップの場合、何も変更しない
        if (strChipId == C_NEWCHIPID) {
            return;
        }
        // 2013/11/29 TMEJ 張 タブレット版SMB チーフテクニシャン機能開発 END

        strSelectedChipId = gSelectedChipId;    //(SetTableUnSelectedStatusでクリアされるため)gSelectedChipIdをロカール変数に保存       
        SetTableUnSelectedStatus(); //選択したチップを解放
        SetChipUnSelectedStatus();  //チップ選択状態を解除
        //チップに2回目でタッチして、選択状態を解放して、何もしない
        if (strSelectedChipId == strChipId) {
            return;
        }
    }

    //前タップチップはサブチップ、関連あるストールチップをタップ以外の場合
    if (!strsubchipFlg) {        
        SetSubChipBoxClose();   
    }
   
    gSelectedChipId = strChipId;    //gSelectedChipIdに選択したチップのidを保存

    //遅れストール画面が表示中、もとに戻す
    if (gShowLaterStallFlg == true) {        
        ShowAllStall(); //もとに戻す
        // タップしたチップが表示するようにスクロールする
        ScrollToShowChip(strChipId);
        // 詳細画面開く初期位置を再設定する
        var scrollMainObjPos = $(".ChipArea_trimming").find(".scroll-inner").position();
        gTranslateValStallX = scrollMainObjPos.left;
        gTranslateValStallY = scrollMainObjPos.top;
    }

    // 2013/11/29 TMEJ 張 タブレット版SMB チーフテクニシャン機能開発 START
    //普通のチップの場合
    if (strChipId != C_NEWCHIPID) {
        // 2013/11/29 TMEJ 張 タブレット版SMB チーフテクニシャン機能開発 END
        //Movingチップを生成
        drawSelectedChipAtPos($("#" + strChipId).position().left, GetRowNoByChipId(strChipId));
        //左右の影、矢印とか隠れないため、選択したチップを最後に移動する
        $("#" + strChipId).appendTo($("#" + strChipId).offsetParent());
        // 2013/11/29 TMEJ 張 タブレット版SMB チーフテクニシャン機能開発 START
    }
    // 2013/11/29 TMEJ 張 タブレット版SMB チーフテクニシャン機能開発 END
    SetTableSelectedStatus();

    // 納車予定日時の線を表示する
    if (gArrObjChip[strChipId]) {
        // 納車予定日時がある且つ納車前チップの場合、納車遅れ見込み時刻線の位置を計算する
        //2014/12/04 TMEJ 丁 新プラットフォーム版i-CROP仕様変更対応（サービス入庫実績更新処理変更）START
        //        if ((IsDefaultDate(gArrObjChip[strChipId].scheDeliDateTime) == false)
        //            && (IsDefaultDate(gArrObjChip[strChipId].rsltDeliDateTime))) {
        if ((IsDefaultDate(gArrObjChip[strChipId].scheDeliDateTime) == false)
            && (gArrObjChip[strChipId].svcStatus != C_SVCSTATUS_DELIVERY)) {
            //2014/12/04 TMEJ 丁 新プラットフォーム版i-CROP仕様変更対応（サービス入庫実績更新処理変更）END

            // 2014/09/11 TMEJ 張 BTS-389 「SMBで異常ではないはずが、遅れ見込み表示になっている」対応 START
            //            // 納車作業時間
            //            var nDeliWorkTime = parseInt($("#hidStandardDeliWrTime").val(), 10);
            //            // 洗車前の場合、洗車時間または納車準備時間を加える
            //            if ((IsDefaultDate(gArrObjChip[strChipId].cwRsltStartDateTime))
            //                && (gArrObjChip[strChipId].carWashNeedFlg == "1")) {
            //                var nDeliPreTime = parseInt($("#hidStandardDeliPreTime").val(), 10);
            //                var nWashTime = parseInt($("#hidStandardWashTime").val(), 10);
            //                // 洗車と納車準備時間が並列行うので、長いほうを使用する
            //                if (nDeliPreTime > nWashTime) {
            //                    nWashTime = nDeliPreTime;
            //                }
            //                nDeliWorkTime = nDeliWorkTime + nWashTime;
            //            }

            //            var dtScheDeliLater = new Date();
            //            dtScheDeliLater.setTime(gArrObjChip[strChipId].scheDeliDateTime.getTime() - nDeliWorkTime * 60000);

            //                            // 納車作業標準時間
            //                            var nDeliWorkTime = parseInt($("#hidStandardDeliWrTime").val(), 10);

            //                            // 納車準備時間
            //                            var nDeliPreTime = parseInt($("#hidStandardDeliPreTime").val(), 10);

            //                            // 洗車標準時間
            //                            var nWashTime = parseInt($("#hidStandardWashTime").val(), 10);

            //                            // 納車見込み線の日時
            //                            var dtScheDeliLater = new Date();

            //                            if (gArrObjChip[strChipId].carWashNeedFlg == "1") {
            //                                // 洗車ありの場合

            //                                if (IsDefaultDate(gArrObjChip[strChipId].cwRsltStartDateTime)) {
            //                                    // 洗車未開始

            //                                    // 洗車標準時間と納車準備時間の長いほうを使う
            //                                if (nDeliPreTime > nWashTime) {

            //                                    // 納車準備時間 + 納車作業標準時間
            //                                    nDeliWorkTime = nDeliWorkTime + nDeliPreTime;
            //                                    
            //                                } else {

            //                                        // 洗車準備時間 + 納車作業標準時間
            //                                        nDeliWorkTime = nDeliWorkTime + nWashTime;

            //                                }

            //                                // 納車予定日時 - 納車作業標準時間 - 標準洗車時間(納車準備時間)
            //                                dtScheDeliLater.setTime(gArrObjChip[strChipId].scheDeliDateTime.getTime() - nDeliWorkTime * 60000);

            //                            } else {
            //                                // 洗車開始した

            //                                // 納車予定日時 - 納車作業標準時間
            //                                dtScheDeliLater.setTime(gArrObjChip[strChipId].scheDeliDateTime.getTime() - nDeliWorkTime * 60000);

            //                            }

            //                        } else {
            //                            // 洗車なしの場合

            //                            if ((gArrObjChip[strChipId].svcStatus == C_SVCSTATUS_DROPOFFCUSTOMER)
            //                                || (gArrObjChip[strChipId].svcStatus == C_SVCSTATUS_WAITINGCUSTOMER)) {
            //                                //納車準備の場合(サービスステータスが預かり中または納車準備)

            //                                // 納車予定日時 - 納車作業標準時間
            //                                dtScheDeliLater.setTime(gArrObjChip[strChipId].scheDeliDateTime.getTime() - nDeliWorkTime * 60000);

            //                            } else {
            //                                // 作業中

            //                                // 納車予定日時 - 納車作業標準時間 - 納車準備時間
            //                                dtScheDeliLater.setTime(gArrObjChip[strChipId].scheDeliDateTime.getTime() - (nDeliWorkTime + nDeliPreTime) * 60000);
            //                                                
            //                            }

            //                        }
            // 納車遅れ見込み時間取得
            // 2017/09/25 NSK 竹中(悠) REQ-SVT-TMT-20170227-002 見込納車遅れ日時計算ロジック統一 START
            // var dtScheDeliLater = GetDeliDelayExpectedTimeLine(gArrObjChip[strChipId].carWashNeedFlg, gArrObjChip[strChipId].cwRsltStartDateTime, gArrObjChip[strChipId].scheDeliDateTime, gArrObjChip[strChipId].svcStatus, gArrObjChip[strChipId].svcinId);
            var dtScheDeliLater = GetDeliDelayExpectedTimeLine(gArrObjChip[strChipId].carWashNeedFlg
                                                             , gArrObjChip[strChipId].scheDeliDateTime
                                                             , gArrObjChip[strChipId].svcStatus
                                                             , gArrObjChip[strChipId].remainingInspectionType);
            // 2017/09/25 NSK 竹中(悠) REQ-SVT-TMT-20170227-002 見込納車遅れ日時計算ロジック統一 END
            // 2014/09/11 TMEJ 張 BTS-389 「SMBで異常ではないはずが、遅れ見込み表示になっている」対応 END

            // 納車遅れ見込み時間により、時刻線の位置を取得して、設定する
            setPosition = GetTimeLinePosByTime(dtScheDeliLater);
            $(".TimingLineDeli").css("left", setPosition);
            // 表示にする
            $(".TimingLineDeli").css("visibility", "visible");
        }
    }

    ShowRelationLine(strChipId);

    //関連あるサブチップをハイライト
    if (strsubchipFlg) {       
        $("#" + strSelectedChipId + " .Front").removeClass("BlackBack");    //BlackBack色を解除        
        $("#" + strSelectedChipId).addClass("SelectedChipShadow");  //影を追加
    }

    // 2019/12/06 NSK 皆川 (トライ店システム評価)整備作業における休憩時間取得判定向上検証 START

//    //2014/06/18 TMEJ 丁 タブレット版SMB テレマ走行距離機能開発 START
//    //CreateFooterButton(C_FT_DISPTP_SELECTED, GetChipType(strChipId)); //フッターボタンを変える
//    CreateFooterButton(C_FT_DISPTP_SELECTED, GetChipType(strChipId), gArrObjChip[strChipId].tlmContractFlg); //フッターボタンを変える
//    //2014/06/18 TMEJ 丁 タブレット版SMB テレマ走行距離機能開発 END

    // 休憩を自動判定する場合
    if ($("#hidRestAutoJudgeFlg").val() == "1") {
        // 2019/12/06 NSK 皆川 (トライ店システム評価)整備作業における休憩時間取得判定向上検証 END
        // 日跨ぎタイプを取得する
        var nOverDaysType = GetChipOverDaysType(strChipId);
        // 日跨ぎの場合
        if (C_OVERDAYS_NONE < nOverDaysType) {
            // コールバックに渡す引数
            var jsonData = {
                Method: "GetCanRestChange",
                ShowDate: $("#hidShowDate").val(),
                StallUseId: gArrObjChip[strChipId].stallUseId
            };

            //コールバック開始
            DoCallBack(C_CALLBACK_WND101, jsonData, SC3240101AfterCallBack, jsonData.Method);
        } else {
            // 休憩変更可能か判定
            var canRestChange = CanRestChange(strChipId)
            CreateFooterButton(C_FT_DISPTP_SELECTED, GetChipType(strChipId), gArrObjChip[strChipId].tlmContractFlg, canRestChange); //フッターボタンを変える
}
    } else {
        CreateFooterButton(C_FT_DISPTP_SELECTED, GetChipType(strChipId), gArrObjChip[strChipId].tlmContractFlg, false); //フッターボタンを変える
    }
    // 2019/12/06 NSK 皆川 (トライ店システム評価)整備作業における休憩時間取得判定向上検証 END
}

/**
* 右爪のTiming_Balloonのdivのを表示する
*/
function ShowRightTimingBalloon() {

    // Movingチップが表示中
    if ($("#" + C_MOVINGCHIPID).length == 1) {
        strChipId = C_MOVINGCHIPID;
    } else if ($("#" + C_MOVINGUNAVALIABLECHIPID).length == 1) {
        // 移動不可チップが表示中
        strChipId = C_MOVINGUNAVALIABLECHIPID;
        // 2013/11/29 TMEJ 張 タブレット版SMB チーフテクニシャン機能開発 START
    } else if ($("#" + C_NEWCHIPID).length == 1) {
        // 移動不可チップが表示中
        strChipId = C_NEWCHIPID;
        // 2013/11/29 TMEJ 張 タブレット版SMB チーフテクニシャン機能開発 END
    } else {
        return;
    }
    var nChipWidth = $("#" + strChipId).width();
    var nChipLeft = $("#" + strChipId).position().left;
    // チップのtopを取得
    var nRow = GetRowNoByChipId(strChipId);

    var nChipTop = $(".stallNo" + nRow).position().top;

    var nTimingBalloonLeft = nChipLeft + nChipWidth + $(".ChipAreaBkGround").position().left - 3 + $(".scroll-inner").position().left;
    var nTimingBalloonTop = nChipTop + 8 + $(".scroll-inner").position().top;

    var dtShowDate = GetTimeByXPos(nChipLeft + nChipWidth);

    // 時間設定
    $(".Timing_Balloon .Time").html(add_zero(dtShowDate.getHours()) + "：" + add_zero(dtShowDate.getMinutes()))
    // 座標設定
    $(".Timing_Balloon").css({ "top": nTimingBalloonTop + "px", "left": nTimingBalloonLeft + "px" });
}
/**
* 左爪のTiming_Balloonのdivのを表示する
*/
function ShowLeftTimingBalloon() {

    // チップのleftを取得
    var strChipId;

    // Movingチップが表示中
    if ($("#" + C_MOVINGCHIPID).length == 1) {
        strChipId = C_MOVINGCHIPID;
    } else if ($("#" + C_MOVINGUNAVALIABLECHIPID).length == 1) {
        // 移動不可チップが表示中
        strChipId = C_MOVINGUNAVALIABLECHIPID;
        // 2013/11/29 TMEJ 張 タブレット版SMB チーフテクニシャン機能開発 START
    } else if ($("#" + C_NEWCHIPID).length == 1) {
        // 移動不可チップが表示中
        strChipId = C_NEWCHIPID;
        // 2013/11/29 TMEJ 張 タブレット版SMB チーフテクニシャン機能開発 END
    } else {
        return;
    }
    var nChipWidth = $("#" + strChipId).width();
    var nChipLeft = $("#" + strChipId).position().left;
    // チップのtopを取得
    var nRow = GetRowNoByChipId(strChipId);

    var nChipTop = $(".stallNo" + nRow).position().top;

    var nTimingBalloonLeft = nChipLeft + $(".ChipAreaBkGround").position().left - 3 + $(".scroll-inner").position().left;
    var nTimingBalloonTop = nChipTop + 8 + $(".scroll-inner").position().top;

    dtShowDate = GetTimeByXPos(nChipLeft - 1);

    // 時間設定
    $(".Timing_Balloon .Time").html(add_zero(dtShowDate.getHours()) + "：" + add_zero(dtShowDate.getMinutes()))
    // 座標設定
    $(".Timing_Balloon").css({ "top": nTimingBalloonTop + "px", "left": nTimingBalloonLeft + "px" });
}
/**
* 右爪のTiming_Balloonのdivのを表示する
*/
function HideTimingBalloon() {
    // スクリーン以外の座標で設定する
    $(".Timing_Balloon").css({ "top": -999 + "px", "left": -999 + "px" });
}


/**
* アクティブストール画面のチップ(サブエリアのチップと紐付け)
* @param {String} strSvcInId svcInId
* @return {Array} リレーションチップのstallUseId
*/
function ActiveStallChipWithLinkSubChip(strSvcInId) {

    // 戻る用リレーションチップのstallUseId
    var arrRtChipIds = new Array();

    // 遅れストール画面が表示中、もとに戻す
    if (gShowLaterStallFlg == true) {
        // もとに戻す
        ShowAllStall();
    }

    // テーブルの状態をチップの選択状態に設定する
    SetTableSelectedStatus();

    // リレーションチップのarrayを取得する
    var arrRelationChipIds = FindRelationChips("", strSvcInId);
    // メイン画面でリレーションチップがあれば
    if (arrRelationChipIds.length > 0) {
        // リレーションチップの線を表示する
        ShowRelationLine(arrRelationChipIds[0][0]);

        for (var nLoop = 0; nLoop < arrRelationChipIds.length; nLoop++) {
            arrRtChipIds.push(arrRelationChipIds[nLoop][0]);
        }
    }
    // リレーションチップのstallUseIdを戻す
    return arrRtChipIds;
}

/**
* 見込み遅刻時刻により、チップの色を更新する
* @param {String} strChipId チップID
* @return {なし}
*/
function UpdateChipColorByDelayDate(strChipId) {

    // 当画面にリレーションチップを取得する
    var arrRelationChips = FindRelationChips(strChipId, "");
    if (arrRelationChips.length == 0) {
        // チップを更新する
        gArrObjChip[strChipId].refleshChipRedColor();
    } else {
        // リレーションの点と線の表示
        for (var nLoop = 0; nLoop < arrRelationChips.length; nLoop++) {
            // 画面に表示される時
            if (gArrObjChip[arrRelationChips[nLoop][0]]) {
                // チップを更新する
                gArrObjChip[arrRelationChips[nLoop][0]].refleshChipRedColor();
            }
        }
    }
    // 遅刻ストールを赤に設定する
    SetLaterStallColorRed();

}

/**
* チップタップ時のイベントを登録
* @param {String} strChipId チップID
* @return {なし}
*/
function BindChipClickEvent(strChipId) {

    //チップタップ時のイベントを登録
    $("#" + strChipId).bind("chipTap", function (e) {
        // ポップアップウィンドウが表示中、何もしない
        if (GetDisplayPopupWindow()) {
            return;
        }
        $("#" + strChipId + " .Front").stop(true, true).removeClass("TapBlueBack");
        setTimeout(function () {
            // 青色表示した後で、チップのタップソースを走る
            TapStallChip(strChipId);
        }, 0);
    })
    .bind(C_TOUCH_START, function (e) {
        gOnTouchingFlg = true;
        // 影があるチップにタップする時、何もしない
        if ($("#" + strChipId + " .Front").hasClass("BlackBack")) {
            return;
        }
        // タップすると青色を表示する
        $("#" + strChipId + " .Front").stop(true, true).addClass("TapBlueBack");
    })
    .bind(C_TOUCH_MOVE + " " + C_TOUCH_END, function (e) {
        gOnTouchingFlg = false;
        $("#" + strChipId + " .Front").stop(true, true).removeClass("TapBlueBack");
    });
}

/**
* Movingチップのタップイベントのbind
* @return {なし}
*/
function BindMovingChipTapEvent() {
    $("#" + C_MOVINGCHIPID).bind("chipTap", function (e) {
        // ポップアップウィンドウが表示中、何もしない
        if (GetDisplayPopupWindow()) {
            return;
        }

        $("#" + gSelectedChipId + " .Front").removeClass("TapBlueBack");
        $("#" + C_MOVINGCHIPID + " .Front").removeClass("TapBlueBack").addClass("BlackBack");
        setTimeout(function () {
            // Movingチップのタップ関数を走る
            TapMovingChip();
        }, 0);

        // チップにタップすると、 $("#ulChipAreaBack_lineBox .TbRow").bind("chipTap")を走らないように
        gCanTbRowTapFlg = false;
    })
    .bind(C_TOUCH_START, function (e) {
        gOnTouchingFlg = true;
        // Movingチップがピッタリ元のチップの上にある時(非表示の時)
        if ($("#" + C_MOVINGCHIPID + " .CpInner").css("visibility") == "hidden") {
            // 元のチップに青色を表示する
            $("#" + gSelectedChipId + " .Front").addClass("TapBlueBack");
        } else {
            // Movingチップが表示される時、Movingチップが一瞬で青色を表示する
            $("#" + C_MOVINGCHIPID + " .Front").removeClass("BlackBack").addClass("TapBlueBack");
        }
    })
    .bind(C_TOUCH_END, function (e) {
        $("#" + gSelectedChipId + " .Front").removeClass("TapBlueBack");
        $("#" + C_MOVINGCHIPID + " .Front").removeClass("TapBlueBack").addClass("BlackBack");
        gOnTouchingFlg = false;
    })
    .bind(C_TOUCH_MOVE, function (e) {
        $("#" + gSelectedChipId + " .Front").removeClass("TapBlueBack");
        $("#" + C_MOVINGCHIPID + " .Front").removeClass("TapBlueBack").addClass("BlackBack");
        gOnTouchingFlg = false;
    });
}
/**
* Movingチップのタップ
* @return {なし}
*/
function TapMovingChip() {
    // MovingChipのleft、幅を取得する
    var nMovingChipLeft, nMovingChipWidth;
    var nBackWidth;
    var strChipId = gSelectedChipId;
    var strRelationSubChip = GetRelationSubChipId(strChipId);

    //2013/11/29 TMEJ 張 タブレット版SMB チーフテクニシャン機能開発 START
    if ($("#" + C_MOVINGCHIPID).length == 0) {
        return;
    }
    //2013/11/29 TMEJ 張 タブレット版SMB チーフテクニシャン機能開発 END
    nMovingChipLeft = $("#" + C_MOVINGCHIPID).position().left;
    nMovingChipWidth = $("#" + C_MOVINGCHIPID).width();

    var nBackLeft, nBackRow;
    // Movingチップある行目を取得
    var nRow = GetRowNoByChipId(C_MOVINGCHIPID);
    //2013/11/29 TMEJ 丁 タブレット版SMB チーフテクニシャン機能開発 START
//    if (GetChipUpdatetype(strChipId) != C_UPDATA_STALLCHIP) {
//        //既にRO紐付いているチップは入れ子にできません
//        if ((gArrObjChip[strChipId].roJobSeq != -1) && (gArrObjSubChip[strRelationSubChip].roJobSeq != gArrObjChip[strChipId].roJobSeq)) {
//            // 「非推奨エリアに配置することできません。元に戻します。」メッセージを表示する
//            ShowSC3240301Msg(910);
//            // 選択したチップを解放する
//            SetTableUnSelectedStatus();
//            // チップ選択状態を解除する
//            SetChipUnSelectedStatus();
//            return;
//        }
//    }
    //2013/11/29 TMEJ 丁 タブレット版SMB チーフテクニシャン機能開発 END
    // 普通のチップの場合、移動したかを判断する
    if ((gSelectedChipId != C_OTHERDTCHIPID) 
        && (gSelectedChipId != C_COPYCHIPID)) {
        // バックアップ幅、左位置、行数目
        if (nBackWidth == null) {
            nBackWidth = $("#" + strChipId).width();
        }
        nCompareWidth = gArrObjChip[strChipId].scheWorkTime * C_CELL_WIDTH / 15 - 1;
        nBackLeft = $("#" + strChipId).position().left;
        nBackRow = GetRowNoByChipId(strChipId);

        // 日跨ぎタイプを取得する
        var nOverDaysType = GetChipOverDaysType(strChipId);
        // 日跨ぎチップ移動したかを判断する
        if (nOverDaysType > C_OVERDAYS_NONE) {
            switch (nOverDaysType) {
                // 左端が日跨ぎ 
                case C_OVERDAYS_LEFT:
                    // 左の爪がない場合
                    if ($("#" + C_MOVINGCHIPID + " .TimeKnobPointL").length == 0) {
                        // 右端が変化してない場合、戻る
                        if ((Math.abs(nMovingChipWidth - nBackWidth) < 1) 
                            && (GetChipUpdatetype(strChipId) == C_UPDATA_STALLCHIP)) {
                            // 選択したチップを解放する
                            SetTableUnSelectedStatus();
                            // チップ選択状態を解除する
                            SetChipUnSelectedStatus();
                            return;
                        }
                    }
                    break;
                // 右端が日跨ぎ     
                case C_OVERDAYS_RIGHT:
                    // 右の爪がない場合
                    if (($("#" + C_MOVINGCHIPID + " .TimeKnobPointR").length == 0)
                        && (GetChipUpdatetype(strChipId) == C_UPDATA_STALLCHIP)) {
                        // 左端が変化してない場合、戻る
                        if (Math.abs(nMovingChipWidth - nBackWidth) < 1) {
                            // 選択したチップを解放する
                            SetTableUnSelectedStatus();
                            // チップ選択状態を解除する
                            SetChipUnSelectedStatus();
                            return;
                        }
                    }
                    break;
                // 両端が日跨ぎ     
                case C_OVERDAYS_BOTH:
                    // 左右の爪がない場合
                    if (($("#" + C_MOVINGCHIPID + " .TimeKnobPointR").length == 0)
                        && ($("#" + C_MOVINGCHIPID + " .TimeKnobPointL").length == 0)) {
                        // 両端が変化してない場合、戻る
                        if ((Math.abs(nMovingChipWidth - nBackWidth) < 1)
                            && (GetChipUpdatetype(strChipId) == C_UPDATA_STALLCHIP)) {
                            // 選択したチップを解放する
                            SetTableUnSelectedStatus();
                            // チップ選択状態を解除する
                            SetChipUnSelectedStatus();
                            return;
                        }
                    }
                    break;
            }
        } else {
            // 移動してない場合
            if (((nMovingChipLeft == nBackLeft) || (nMovingChipLeft < 1))
                && (Math.abs(nCompareWidth - nMovingChipWidth) < 1)
                && (nRow == nBackRow)
                && (GetChipUpdatetype(strChipId) == C_UPDATA_STALLCHIP)) {
                // メインストールチップの場合
                // 選択したチップを解放する
                SetTableUnSelectedStatus();
                // チップ選択状態を解除する
                SetChipUnSelectedStatus();
                return;
            } else if (IsDefaultDate(gArrObjChip[strChipId].rsltEndDateTime) == false) {
                // サブチップ且つ実績チップと紐付く場合
                if (GetChipUpdatetype(strChipId) != C_UPDATA_STALLCHIP) {
                    return;
                } else {
                    // 選択したチップを解放する
                    SetTableUnSelectedStatus();
                    // チップ選択状態を解除する
                    SetChipUnSelectedStatus();
                    return;
                }
            }
        }
    }

    // 置いた位置をチェックする
    if (CheckChipPos(C_MOVINGCHIPID) == false) {
        // エラーメッセージ「他のチップと配置時間が重複します。」を表示する
        ShowSC3240101Msg(906);

        // 他の日付から移動したチップ、コピーの場合
        if (gSelectedChipId == C_OTHERDTCHIPID) {
            // Movingチップを削除する
            $("#" + C_MOVINGCHIPID).remove();
            gMovingChipObj = null;
            // テクニシャンエリアにチップを表示する
            $("#" + gOtherDtChipObj.stallUseId).css("visibility", "visible");
        } else if (gSelectedChipId == C_COPYCHIPID) {
            // 選択したチップを解放する
            SetTableUnSelectedStatus();
            // チップ選択状態を解除する
            SetChipUnSelectedStatus();
        } else {
            // メインチップの場合
            if (!gArrObjSubChip[strChipId]) {
                // 元に戻す
                $("#" + strChipId).css("left", nBackLeft);
                $("#" + strChipId).css("width", nBackWidth);
                AdjustChipItemByWidth(strChipId);
            }
            // 選択したチップを解放する
            SetTableUnSelectedStatus();
            // チップ選択状態を解除する
            SetChipUnSelectedStatus();
        }

        return;
    }

    // 2019/12/06 NSK 皆川 (トライ店システム評価)整備作業における休憩時間取得判定向上検証 START
    // 休憩を自動判定しない場合
    if ($("#hidRestAutoJudgeFlg").val() != "1") {
        // 2019/12/06 NSK 皆川 (トライ店システム評価)整備作業における休憩時間取得判定向上検証 END

        // 移動先が休憩エリアがあるかどうかを判断する
    var arrRestTime = GetRestTimeInServiceTime(C_MOVINGCHIPID);
    // 重複休憩エリアがあれば、
    if (arrRestTime.length > 0) {
        // 一番左の休憩エリアの真中で休憩ウィンドウを表示する
        ShowRestTimeDialog(arrRestTime[0][0], C_ACTION_MOVE);
        return;
    }

        // 2019/12/06 NSK 皆川 (トライ店システム評価)整備作業における休憩時間取得判定向上検証 START
    }
    // 2019/12/06 NSK 皆川 (トライ店システム評価)整備作業における休憩時間取得判定向上検証 END

    // チップがMovingチップにある所に移動
    MoveChip(null);
}
/**
* チップ移動
* @return {なし}
*/
function MoveChip(nRestFlg) {

    var strChipId = gSelectedChipId;
    var nMovingChipLeft = $("#" + C_MOVINGCHIPID).position().left;

    // Movingチップが休憩エリアの後に移動可能ですから、最初タップする位置がサーバに渡し用の開始時間である
    var nTapXPos = nMovingChipLeft;
    var nMovingChipWidth = $("#" + C_MOVINGCHIPID).width();
    var bOtherDtChipFlg = false;
    // エラーの時、元に戻す用
    var nBackWidth;
    var nBackLeft;
    var nBackRow;

    //2015/02/09 TMEJ 範 DMS連携版サービスタブレット SMB納車予定時刻通知機能開発 START

    //一時保存用チップオブジェクト
    var tempChipObject = new ReserveChip(strChipId);

    //状況に合ったチップオブジェクトを一時保存用チップオブジェクトにコピーする
    if (0 < strChipId) gArrObjChip[strChipId].copy(tempChipObject); //当日内の移動チップ
    else if (strChipId == C_OTHERDTCHIPID) gOtherDtChipObj.copy(tempChipObject);        //当日外に移動したチップ
    else gCopyChipObj.copy(tempChipObject);           //確定前コピーチップ

    //RO番号がある場合(RO顧客承認後の場合)
    if (tempChipObject.roNum.Trim() != "") {

        //移動後の終了日時を取得
        var afterMovingEndTime = GetTimeByXPos(nMovingChipLeft + nMovingChipWidth);

        //表示終了日時を一時保存用チップオブジェクトに設定
        tempChipObject.setDisplayEndDate(afterMovingEndTime);

        //移動後のチップが遅れ、または遅れ見込の場合
        if (IsDelayDelivery(tempChipObject)) {

            // 確認ボックスを表示
            if (!ConfirmSC3240101Msg(935)) {

                //一時保存用チップオブジェクト削除
                tempChipObject = null;

                //チップの移動を確定しないために、以降の処理を行わない
                return;

            }
        }
    }

    //一時保存用チップオブジェクト削除
    tempChipObject = null;

    //2015/02/09 TMEJ 範 DMS連携版サービスタブレット SMB納車予定時刻通知機能開発 END

    // 重複チップIDを保存する
    var arrBackDuplicateChipsId = GetDuplicateChips(strChipId);

    // 別の日のチップの場合
    if (gSelectedChipId == C_OTHERDTCHIPID) {
        bOtherDtChipFlg = true;
        strChipId = gOtherDtChipObj.stallUseId;
        // 普通のチップの大きさに戻す 
        $("#" + strChipId).css("visibility", "visible").css("top", "1px").css("height", "72px").css("opacity", C_OPACITY);
        // 普通のチップで表示される
        gArrObjChip[strChipId] = new ReserveChip(strChipId);
        gOtherDtChipObj.copy(gArrObjChip[strChipId]);
        nBackWidth = GetWidthByDisplayTime(gOtherDtChipObj.displayStartDate, gOtherDtChipObj.displayEndDate);
        // 別の日のチップをクリアする
        gOtherDtChipObj = null;
        gSelectedChipId = gArrObjChip[strChipId].stallUseId;
        // 元のchiptapイベントをunbindする
        $("#" + gSelectedChipId).unbind();
        // 普通のチップのタップイベントをbindする
        BindChipClickEvent(gSelectedChipId);
    } else if (gSelectedChipId == C_COPYCHIPID) {
        bOtherDtChipFlg = true;
        strChipId = C_COPYCHIPID;
        // 普通のチップの大きさに戻す 
        $("#" + strChipId).css("visibility", "visible");
        $("#" + strChipId).css("top", "1px");
        $("#" + strChipId).css("height", "72px");
        // 非透明にする
        $("#" + strChipId).css("opacity", C_OPACITY);

        // 一時用コピーされたチップIDを取得する
        strChipId = GetNextCopyedId();
        // チップIDを一時用コピーされたチップIDに設定する
        $("#" + C_COPYCHIPID).attr("id", strChipId);

        // 普通のチップで表示される
        gArrObjChip[strChipId] = new ReserveChip(strChipId);
        gCopyChipObj.copy(gArrObjChip[strChipId]);
        gArrObjChip[strChipId].stallUseId = strChipId;
        nBackWidth = GetWidthByDisplayTime(gCopyChipObj.displayStartDate, gCopyChipObj.displayEndDate);
        
        // 別の日のチップをクリアする
        gCopyChipObj = null;
        gSelectedChipId = gArrObjChip[strChipId].stallUseId;
        // 元のchiptapイベントをunbindする
        $("#" + gSelectedChipId).unbind();
        // 普通のチップのタップイベントをbindする
        BindChipClickEvent(gSelectedChipId);
    } else {
        nBackWidth = $("#" + strChipId).width();
        nBackLeft = $("#" + strChipId).position().left;
        nBackRow = GetRowNoByChipId(strChipId);
    }

    var nRestTimeWidth = 0;

    // 2019/12/06 NSK 皆川 (トライ店システム評価)整備作業における休憩時間取得判定向上検証 START
    // 休憩を自動判定しない場合
    if ($("#hidRestAutoJudgeFlg").val() != "1") {
        // 2019/12/06 NSK 皆川 (トライ店システム評価)整備作業における休憩時間取得判定向上検証 END

    // 休憩フラグを設定した場合
    if (nRestFlg != null) {
        // 休憩を取得する場合
        if (nRestFlg == 1) {
            // 移動チップと重複の休憩エリアIDを取得する
            var arrRestTime = GetRestTimeInServiceTime(C_MOVINGCHIPID);

            // 休憩エリアがチップの左にある且つ重複時、チップが休憩エリアの右に移動する
            var nChipLeft = GetLeftByRestTime(C_MOVINGCHIPID, arrRestTime)
            if (nChipLeft != nMovingChipLeft) {
                nMovingChipLeft = nChipLeft;
                $("#" + C_MOVINGCHIPID).css("left", nMovingChipLeft);
            }

            // チップの幅が休憩エリアの幅をプラスする
            nRestTimeWidth = GetWidthByRestTime(C_MOVINGCHIPID);

            var dtMovedDate = GetTimeByXPos(nChipLeft - 1);
            if (dtMovedDate - gEndWorkTime >= 0) {
                // コピー、他のページからきたチップを削除する
                if (bOtherDtChipFlg) {
                    // バックアップした位置に戻す
                    $("#" + gSelectedChipId).remove();
                }

                //「営業終了時間(%1:%2)以内に配置してください。」ってメッセージが表示される
                ShowSC3240101Msg(912);
                // 選択したチップを解放する
                SetTableUnSelectedStatus();
                // チップ選択状態を解除する
                SetChipUnSelectedStatus();
                return;
            }
            }
        }
        // 2019/12/06 NSK 皆川 (トライ店システム評価)整備作業における休憩時間取得判定向上検証 START
    } else {
        // 移動先のチップの開始日時が当日である場合、作業時間に休憩時間を足す処理を行う
        // ※日跨ぎの翌日チップを選択し休憩変更ボタンをタップするとチップ幅の表示が一瞬おかしくなる不具合の対応で分岐を追加
        //   チップが日跨ぎの場合はクライアント側ではチップ幅の計算をしない（コールバック後サーバー側で計算した幅で再描画）
        if (IsTodayStartDate(strChipId)) {
            // チップの幅が休憩エリアの幅をプラスする
            nRestTimeWidth = GetWidthByRestTime(C_MOVINGCHIPID);
        }
    }
    // 2019/12/06 NSK 皆川 (トライ店システム評価)整備作業における休憩時間取得判定向上検証 END
    
    // 元のちップをMovingChipの所へ移動する
    $("#" + strChipId).css("left", nMovingChipLeft);
    $("#" + strChipId).css("width", nMovingChipWidth + nRestTimeWidth);

    if (strChipId == gSelectedChipId) {
        $("#" + gSelectedChipId).css("top", 1);
    }

    // 移動前と移動後の行目
    nBackRow = GetRowNoByChipId(strChipId);
    var nRow = GetRowNoByChipId(C_MOVINGCHIPID);
    // 行数が変わる時
    if ((bOtherDtChipFlg) || (nBackRow != nRow)) {
        $(".Row" + nRow).append($("#" + strChipId));
    }
    // 幅によって、チップに表示内容を調整する　(ストール上)
    AdjustChipItemByWidth(strChipId);
    // 重複チップがあれば、リサイズを失敗する
    var arrDuplChipId = GetDuplicateChips(strChipId);
    // 重複の場合、
    if (arrDuplChipId.length > 1) {
        // エラーメッセージ「他のチップと配置時間が重複します。」を表示する
        ShowSC3240101Msg(906);

        // nBackLeftがundefinedの場合、移動されたチップが他の日から移動するチップまたはコピーされたチップ
        // 元の位置が当ページではない、それで、削除する
        if (nBackLeft == undefined) {
            $("#" + strChipId).remove();
        } else {
            // 元に戻す
            $("#" + strChipId).width(nBackWidth);
            $("#" + strChipId).css("left", nBackLeft);
            $(".Row" + nBackRow).append($("#" + strChipId));
        }

        // 選択したチップを解放する
        SetTableUnSelectedStatus();
        // チップ選択状態を解除する
        SetChipUnSelectedStatus();

        // ワイトボードを最表示する
        CreateWhiteBorders(strChipId);
        return;
    }

    // 差分値をバックする
    var nBackMinutes;
    // 元のチップ終了表示時間をバックアップする
    var dtBackdispEndDate;
    if (gArrObjChip[strChipId]) {
        nBackMinutes = Math.ceil((gArrObjChip[strChipId].displayEndDate - gArrObjChip[strChipId].displayStartDate) / 1000 / 60);
        dtBackdispEndDate = gArrObjChip[strChipId].displayEndDate;
    }
    else {
        nBackMinutes = Math.ceil((gArrObjSubChip[strChipId].displayEndDate - gArrObjSubChip[strChipId].displayStartDate) / 1000 / 60);
        dtBackdispEndDate = gArrObjSubChip[strChipId].displayEndDate;
    }
    
    // chipの属性を設定する
    SetChipPrototypeTimeAndStallIdData(strChipId);

    // 2019/12/06 NSK 皆川 (トライ店システム評価)整備作業における休憩時間取得判定向上検証 START
    // 休憩を自動判定する場合
    if ($("#hidRestAutoJudgeFlg").val() == "1") {
        // 2019/12/06 NSK 皆川 (トライ店システム評価)整備作業における休憩時間取得判定向上検証 END

        // 2019/06/18 NSK 皆川 (トライ店システム評価)次世代サービスオペレーション効率化に向けた、業務適合性検証 START
        // 日跨ぎタイプを取得する
        var nOverDaysType = GetChipOverDaysType(strChipId);
        // 日跨ぎでない場合
        if (nOverDaysType == C_OVERDAYS_NONE) {
            gArrObjChip[gSelectedChipId].restFlg = JudgeRestFlg(strChipId);
        }
        // 2019/06/18 NSK 皆川 (トライ店システム評価)次世代サービスオペレーション効率化に向けた、業務適合性検証 END

        // 2019/12/06 NSK 皆川 (トライ店システム評価)整備作業における休憩時間取得判定向上検証 END
    }
    // 2019/12/06 NSK 皆川 (トライ店システム評価)整備作業における休憩時間取得判定向上検証 END

    // 左爪がある場合
    if ($("#" + C_MOVINGCHIPID + " .TimeKnobPointL").length == 1) {
        // 予約開始時間を設定する
        gArrObjChip[strChipId].setScheStartDateTime(gArrObjChip[strChipId].displayStartDate);
        // 関連チップの場合、関連チップのstartdatetimeも設定する
        if (gArrObjRelationChip[strChipId]) {
            gArrObjRelationChip[strChipId].setStartDateTime(gArrObjChip[strChipId].displayStartDate);
        }
    }

    // 見込み遅刻時刻により、チップの色を更新する
    UpdateChipColorByDelayDate(strChipId);

    if (GetChipUpdatetype(strChipId) == C_UPDATA_STALLCHIP) {
        var nTapTime = GetTimeByXPos(nTapXPos - 1);
        var nWorkTime = (gArrObjChip[strChipId].displayEndDate.getTime() - gArrObjChip[strChipId].displayStartDate.getTime()) / 60 / 1000;
        // 左爪がない場合、左端が日跨ぎなので、開始時間が変わらない
        if ($("#" + C_MOVINGCHIPID + " .TimeKnobPointL").length == 0) {
            // 予約チップの場合、予定開始時間に設定する
            if (IsDefaultDate(gArrObjChip[strChipId].rsltStartDateTime)) {
                nTapTime = new Date(gArrObjChip[strChipId].scheStartDateTime);
            } else {
                nTapTime = new Date(gArrObjChip[strChipId].rsltStartDateTime);
            }
            // 日跨ぎチップ、移動してない状態でリサイズするとgArrObjChip[strChipId].scheWorkTimeを作業時間にする
            nWorkTime = gArrObjChip[strChipId].scheWorkTime;
        }
        // 日跨ぎチップ、移動してない状態でリサイズするとgArrObjChip[strChipId].scheWorkTimeを作業時間にする
        if ($("#" + C_MOVINGCHIPID + " .TimeKnobPointR").length == 0) {
            nWorkTime = gArrObjChip[strChipId].scheWorkTime;
        }

        // サーバへチップの開始時間と終了時間を設定する
        setChipDisplayTimeAndStallIdToServer(strChipId, nRestFlg, nTapTime, nWorkTime);
        
        // テーブルの状態をチップの未選択状態に設定する
        SetTableUnSelectedStatus();
        // チップ選択状態を解除する
        SetChipUnSelectedStatus();
    } else {
        //MOVINGチップにパラメタを退避
        //ストールID
        var strStallId = $(".stallNo" + nRow)[0].id;
        //ストールID
        strStallId = strStallId.substring(8, strStallId.length);
        gMovingSubChipObj = new MovingSubChip();
        gMovingSubChipObj.setStallId(strStallId);             //ストールID
        gMovingSubChipObj.setScheWorkTime(nWorkTime);        // 予定開始日時
        // 予定作業時間
        var startTime = GetTimeByXPos(nMovingChipLeft - 1);
        gMovingSubChipObj.setStartDateTime(startTime);     
        gMovingSubChipObj.selectedChipId = gSelectedChipId; //選択チップ
        GetReadyMovingUpdate(strChipId, nRestFlg);
        // テーブルの状態をチップの未選択状態に設定する
        SetTableUnSelectedStatus();
        // チップ選択状態を解除する
        SetChipUnSelectedStatus();
        //サブボックスが開いているなら、フーターボタンを凹ませる
        if (gOpenningSubBoxId != "") {
            FooterIconReplace(gOpenningSubBoxId);
        }
    }
    // チップの右のボードーラインが画面のボーダーラインを超える場合
    var nMaxBorder = $(".Row1").width();
    if (nMovingChipLeft + nMovingChipWidth + nRestTimeWidth > nMaxBorder) {
        // チップの表示が画面の右のボーダーラインまで終わる
        nMovingChipWidth = nMovingChipWidth - (nMovingChipLeft + nMovingChipWidth + nRestTimeWidth - nMaxBorder)
        $("#" + strChipId).css("width", nMovingChipWidth + nRestTimeWidth);
    }

    if (!bOtherDtChipFlg) {
        // 元のチップが重複の場合、
        if (arrBackDuplicateChipsId.length > 1) {
            for (var nLoop = 0; nLoop < arrBackDuplicateChipsId.length; nLoop++) {
                // 他の元の重複チップの場合
                if (arrBackDuplicateChipsId[nLoop][0] != strChipId) {
                    // 今重複かをはんだんする
                    $("#WB" + arrBackDuplicateChipsId[nLoop][0]).remove();
                    CreateWhiteBorders(arrBackDuplicateChipsId[nLoop][0]);
                } else {
                    // 今重複かをはんだんする
                    $("#WB" + arrBackDuplicateChipsId[nLoop][0]).remove();
                }
            }
        }
    }
}

/**
* 次の一時用のコピーチップIDを取得する
* @return {なし}
*/
function GetNextCopyedId() {
    if (gLoopCopyedId == 9) {
        gLoopCopyedId = 0;
    } else {
        gLoopCopyedId++;
    }
    return C_ARR_COPYEDCHIPID[gLoopCopyedId];
}
/**
* Movingチップのりサイズの設定
* @param {String} strChipId 選択中のチップのid
* @param {Integer} nMinWidth divの最小幅
* @param {Integer} nHandles リサイズできる端(0:両方 1:左端のみ 2:右端のみ)
* @return {なし}
*/
function BindChipResize(strChipId, nMinWidth, nHandles) {

    if (strChipId == C_MOVINGCHIPID) {
        // Movingチップがあるか
        if (gMovingChipObj == null) {
            return;
        }
    }

    var strHandles = 'e, w';
    // 左端のみ
    if (nHandles == 1) {
        strHandles = 'w';
    } else if (nHandles == 2) {
        // 右端のみ
        strHandles = 'e';
    }

    // Movingチップの場合、
    if (strChipId == C_MOVINGCHIPID) {
        // 作業中のチップの場合
        if ((IsDefaultDate(gMovingChipObj.rsltStartDateTime) == false) && (IsDefaultDate(gMovingChipObj.rsltEndDateTime) == true)) {
            // 作業中チップ且つ左端しかリサイズできない場合、チップがリサイズ不可
            if (nHandles == 1) {
                $("#" + C_MOVINGCHIPID).resizable("disable");
                return;
            } else {
                // 右方向だけで移動できる
                strHandles = 'e';
            }

        } else if (IsDefaultDate(gMovingChipObj.rsltEndDateTime) == false) {
            // 作業終了と納車終了の場合、りサイズできない
            $("#" + C_MOVINGCHIPID).resizable("disable");
            return;
        }
    }

    // nMinWidthがない場合、最小単位を設定する
    if (nMinWidth == 0) {
        nMinWidth = C_CELL_WIDTH / 15 * gResizeInterval;
    }

    var nMaxRight, nMinLeft;
    var nMovingChipLeft, nMovingChipRight;
    var nBackMovingChipLeft, nBackWidth;
    var bEastFlg = true;

    // 前のresizableイベントを削除する
    $("#" + strChipId).resizable('destroy');
    // チップのresizableを設定する
    $("#" + strChipId).resizable({
        handles: strHandles,                                //ドラッグ位置を右と左のみに限定
        minWidth: nMinWidth,                                //リサイズできる最小の幅(ストールの間隔に合わせる必要あり)
        grid: C_CELL_WIDTH / 15 * gResizeInterval,          //リサイズの単位(px)[x座標, y座標](ストールの間隔に合わせる必要あり)

        start: function (e, ui) {
            //  移動できる左と右の初期位置
            nMaxRight = GetXPosByTime(gEndWorkTime) + 1;
            nMinLeft = GetXPosByTime(gStartWorkTime);

            var arrLeftPos = new Array();
            var arrRightPos = new Array();
            arrLeftPos.push(nMinLeft);
            arrRightPos.push(nMaxRight);

            // Movingチップ左座標と右座標を記録する
            nMovingChipLeft = $("#" + strChipId).position().left;
            nMovingChipRight = nMovingChipLeft + $("#" + strChipId).width();
            // 移動する前バックアップ
            nBackMovingChipLeft = nMovingChipLeft;
            nBackWidth = $("#" + strChipId).width();

            // この行に全てチップを取得する
            $("#" + strChipId).offsetParent().children("div").each(function (index, e) {
                // Movingチップと選択したチップ以外のチップの場合、左座標と右座標を記録する
                if ((e.id != strChipId) && (e.id != gSelectedChipId)
                    && (!IsUnavailableArea(e.id)) && (!IsRestArea(e.id))) {
                    // 移動できる左の座標を計算する
                    if ((e.offsetLeft + e.offsetWidth) < nMovingChipLeft) {
                        arrLeftPos.push(e.offsetLeft + e.offsetWidth);
                    }
                    // 移動できる右の座標を計算する
                    if (e.offsetLeft > nMovingChipRight) {
                        arrRightPos.push(e.offsetLeft);
                    }
                }
            });

            // ソートする
            arrLeftPos.sort(function (x1, x2) { return x1 - x2 });
            arrRightPos.sort(function (x1, x2) { return x1 - x2 });
            // 移動できる左の座標を変数に記録する
            nMinLeft = arrLeftPos[arrLeftPos.length - 1];
            nMaxRight = arrRightPos[0];

            // Movingチップを表示する
            if ($("#" + C_MOVINGCHIPID).length > 0) {
                $("#" + C_MOVINGCHIPID + " .CpInner").css("visibility", "visible");
            }
            // C_MOVINGUNAVALIABLECHIPIDチップを表示する
            if ($("#" + C_MOVINGUNAVALIABLECHIPID).length > 0) {
                $("#" + C_MOVINGUNAVALIABLECHIPID + " .ShadowBox").css("display", "");
                $("#" + C_MOVINGUNAVALIABLECHIPID + " .Front").css("display", "");
            }

            // 選択のは左の爪ですか、右の爪ですか
            if (e.srcElement.className.indexOf("ui-resizable-w") >= 0) {
                // 右の爪
                bEastFlg = false;
            } else {
                // 左の爪
                bEastFlg = true;
            }
        },
        resize: function (e, ui) {                     //リサイズ中に呼び出される関数
            gStopFingerScrollFlg = true;
            $(".ChipArea_trimming").SmbFingerScroll({
                action: "stop"
            });
            nMovingChipLeft = $("#" + strChipId).position().left;
            nMovingChipRight = nMovingChipLeft + $("#" + strChipId).width();
         
            // 5分倍数未満の場合、5分倍数に設定する(予約チップ)
            // 予約チップのMovingチップが表示中
            if ($("#" + C_MOVINGCHIPID).length == 1) {
                var dtEndDateTime = GetTimeByXPos(nMovingChipRight);
                var nOffsetMinutes = dtEndDateTime.getMinutes() % gResizeInterval;
                if ((IsDefaultDate(gMovingChipObj.rsltStartDateTime)) && (nOffsetMinutes != 0)) {
                    // 右端が左へ移動する時、07分のチップが05分にする
                    if (nMovingChipRight < nBackMovingChipLeft + nBackWidth) {
                        // 5分単位を減らして、02分になる。それで、3分をプラスして、05分になる。
                        nMovingChipRight += Math.floor((gResizeInterval - nOffsetMinutes) * C_CELL_WIDTH / 15);
                        $("#" + strChipId).css("width", nMovingChipRight - nMovingChipLeft);
                    }
                    // 右端が右へ移動する時、07分のチップが10にする
                    if (nMovingChipRight > nBackMovingChipLeft + nBackWidth) {
                        // 5分単位を加えて、12分になる。それで、2分をプラスして、10分になる。
                        nMovingChipRight -= Math.ceil(nOffsetMinutes * C_CELL_WIDTH / 15);
                        $("#" + strChipId).css("width", nMovingChipRight - nMovingChipLeft);
                    }
                }
            }

            // 左の方向へ移動する時、左のボードが越える場合、左のボードはnMinLeftの右に設定する
            if ((nMovingChipLeft <= nMinLeft) && (nMovingChipLeft < nBackMovingChipLeft)) {
                var nLeftPos = nMovingChipRight - (parseInt((nMovingChipRight - nMinLeft) / C_CELL_WIDTH) * C_CELL_WIDTH) + 1;
                var nWidth = nMovingChipRight - nLeftPos;
                $("#" + strChipId).css("left", nLeftPos).css("width", nWidth);
            }
            // 右の方向へ移動する時、右が越える場合、左のボードはnMaxRightの左に設定する
            if ((nMovingChipRight >= nMaxRight) && (nMovingChipRight > nBackMovingChipLeft + nBackWidth)) {
                // チップの開始座標は2なので、+2をする
                var nWidth = parseInt((nMaxRight + 2 - nMovingChipLeft) / C_CELL_WIDTH) * C_CELL_WIDTH - 1;
                $("#" + strChipId).css("width", nWidth);
            }

            // 幅によって、チップに表示内容を調整する
            AdjustChipItemByWidth(strChipId);

            // 爪(右)の位置はチップの幅によって、座標を設定する
            SetTimeKnobPointRPosbyChipWidth(strChipId);

            // bEastFlgにより、ポップアップ時間が左で表示するか、右で表示するか
            if (bEastFlg) {
                ShowRightTimingBalloon();
            } else {
                ShowLeftTimingBalloon();
            }
        },
        stop: function (e, ui) {
            // C_MOVINGUNAVALIABLECHIPIDチップが表示中
            if ($("#" + C_MOVINGUNAVALIABLECHIPID).length > 0) {
                var nAfterMovingWidth = $("#" + C_MOVINGUNAVALIABLECHIPID).width();
                // 移動する前の幅と比べる
                var nChangeWidth = nAfterMovingWidth - nBackWidth;
                var nChangeMinutes = 15 / C_CELL_WIDTH * nChangeWidth;
                var nIdleTime = parseInt($("#" + gSelectedChipId).data("IDLE_TIME"));
                nIdleTime += nChangeMinutes;
                // 新たしい時間を設定する
                $("#" + gSelectedChipId).data("IDLE_TIME", nIdleTime);
            }
            HideTimingBalloon();

            // 2013/11/29 TMEJ 張 タブレット版SMB チーフテクニシャン機能開発 START
            // 新しい開始時間と終了時間がNEWチップに設定する
            if (strChipId == C_NEWCHIPID) {
                // chipの属性を設定する
                SetChipPrototypeTimeAndStallIdData(C_NEWCHIPID);
            }
            // 2013/11/29 TMEJ 張 タブレット版SMB チーフテクニシャン機能開発 END
        }
    });

    $(".ui-resizable-e").bind(C_TOUCH_START, function (e) {
        ShowRightTimingBalloon();
    })
    .bind(C_TOUCH_END, function (event) {
        HideTimingBalloon();
    });

    $(".ui-resizable-w").bind(C_TOUCH_START, function (e) {
        ShowLeftTimingBalloon();
    })
    .bind(C_TOUCH_END, function (event) {
        HideTimingBalloon();
    });
}
/**
* ポップアップボックスにチップをタップイベント
* @param {String} strChipId ポップアップボックスにチップID
* @return {なし}
*/
function BindPopUpChipTapEvent(strChipId) {
    // チップタップ時のイベントを登録
    $("#" + strChipId).bind("chipTap", function (e) {

        // グレーの時タップすると、反応がない
        if ($("#" + strChipId + " .Front").hasClass("BlackBack") == true) {
            return;
        }

        // 他のポップアップウィンドウが表示中、何もしない
        var nDisplayPopWnd = GetDisplayPopupWindow();
        if ((nDisplayPopWnd > C_DISP_NONE) && (nDisplayPopWnd != C_DISP_DUPL)) {
            return;
        }

        // ストールに対応するチップのidを取得
        strChipId = strChipId.substring(5, strChipId.length);

        // チップが選択されている場合、選択を解除する
        if (gSelectedChipId != "") {
            // 選択したチップを解放する
            SetTableUnSelectedStatus();
            // チップ選択状態を解除する
            SetChipUnSelectedStatus();
        }

        // 白い枠のチップを選択されている場合、白い枠が消しる
        if (gWBChipId.substring(2, gWBChipId.length) == strChipId) {
            // 白い枠を削除する
            $("#" + gWBChipId).remove();
        }

        // ポップアップボックスが消える
        RemovePopUpBox();

        // チップをタップする
        TapStallChip(strChipId);
    });
}

/**
* リレーションチップを取得する(order by ChildNo)
* @param {String} strStallUseId チップStallUseId
* @param {String} strSvcinId チップサービス入庫ID
* @return {Array} strChipIdのリレーションチップの配列
*/
function FindRelationChips(strStallUseId, strSvcinId) {

    var arrRelationChipId = new Array();
    // サービス入庫IDが空白の場合、
    if (strSvcinId == "") {
        // 関連チップにあれば、
        if (gArrObjRelationChip[strStallUseId]) {
            // サービス入庫IDを取得
            strSvcinId = gArrObjRelationChip[strStallUseId].svcinId;
        } else {
            // 関連チップがない場合
            return arrRelationChipId;
        }
    }

    // すべて関連チップをループして、同じsvcInIdを持つチップを探す
    var nLoop = 0;
    for (var strId in gArrObjRelationChip) {
        if (gArrObjRelationChip[strId]) {
            if (gArrObjRelationChip[strId].svcinId == strSvcinId) {
                arrRelationChipId[nLoop] = new Array(strId, gArrObjRelationChip[strId].startDateTime);
                nLoop++;
            }
        }
    }
    // startDatetimeよりsortする
    arrRelationChipId.sort(function (x, y) { return x[1] - y[1] });
    return arrRelationChipId;
}

/**
* 当ページに同じサービス入庫IDを持ってのチップを探す
* @param {String} strSvcinId チップサービス入庫ID
* @return {Array} strChipIdのリレーションチップの配列
*/
function FindChipsBySvcinId(strSvcinId) {

    var arrRelationChipId = new Array();
    // すべて関連チップをループして、同じsvcInIdを持つチップを探す
    for (var strId in gArrObjChip) {
        // 2013/11/29 TMEJ 張 タブレット版SMB チーフテクニシャン機能開発 START
        if (CheckgArrObjChip(strId) == false) {
            continue;
        }
        // 2013/11/29 TMEJ 張 タブレット版SMB チーフテクニシャン機能開発 END
        if (gArrObjChip[strId] && (IsCopyedChip(strId) == false)) {
            if (gArrObjChip[strId].svcInId == strSvcinId) {
                arrRelationChipId.push(strId);
            }
        }
    }
    // startDatetimeよりsortする
    return arrRelationChipId;
}

/**
* リレーションチップに作業中チップがあるか
* @param {String} strChipId チップid
* @return {Bool} true: ある
*/
function HasWorkingChipInRelation(strChipId) {
    var strSvcInId = gArrObjChip[strChipId].svcInId;
    // すべてチップをループして、同じsvcInIdを持つチップを探す
    for (var strId in gArrObjChip) {
        // 有効のチップデータをチェックする
        if (CheckgArrObjChip(strId) == false) {
            continue;
        }
        if (gArrObjChip[strId].svcInId == strSvcInId) {
            // 作業中の場合
            if ((IsDefaultDate(gArrObjChip[strId].rsltStartDateTime) == false) && (IsDefaultDate(gArrObjChip[strId].rsltEndDateTime) == true)) {
                return true
            }
        }
    }
    return false;
}

//2017/01/17 NSK 竹中 TR-SVT-TMT-20151019-002 TCメイン画面のエラーメッセージが理解不可能 START

/**
* 作業中のリレーションチップが存在するストール名称
* @param {String} strChipId チップid
* @return {String} stallId ストール名称
*/
function GetStallIdOfRelationChip(strChipId) {
    var strSvcInId = gArrObjChip[strChipId].svcInId;
    // すべてチップをループして、同じsvcInIdを持つチップを探す
    for (var strId in gArrObjChip) {
        // 有効のチップデータをチェックする
        if (CheckgArrObjChip(strId) == false) {
            continue;
        }
        if (gArrObjChip[strId].svcInId == strSvcInId) {
            // 作業中の場合
            if ((IsDefaultDate(gArrObjChip[strId].rsltStartDateTime) == false) && (IsDefaultDate(gArrObjChip[strId].rsltEndDateTime) == true)) {
                return gArrObjChip[strId].stallId
            }
        }
    }
    return undefined;
}

//2017/01/17 NSK 竹中 TR-SVT-TMT-20151019-002 TCメイン画面のエラーメッセージが理解不可能 END

/**
* strChipIdチップと関連チップのリレーションチップの線を表示する
* @param {String} strChipId チップid
* @return {なし}
*/
function ShowRelationLine(strChipId) {

    // リレーションチップではない場合、何もしない
    if (IsRelationChip(strChipId) == false) {
        return;
    }

    // リレーションチップを取得する
    var arrRelationChipId = FindRelationChips(strChipId, "");

    // リレーション数を取得する
    var nRelationCnt = arrRelationChipId.length;
    // Noshowにあるリレーションチップを絞り込む
    for (var nLoop = nRelationCnt - 1; nLoop >= 0; nLoop--) {
        // 画面に表示されない場合
        if (!gArrObjChip[arrRelationChipId[nLoop][0]]) {
            // 当ページにあれば、noshowチップであること
            if ((arrRelationChipId[nLoop][1] - gStartWorkTime >= 0)
                && (arrRelationChipId[nLoop][1] - gEndWorkTime < 0)) {
                arrRelationChipId.splice(nLoop, 1); 
                nRelationCnt--;
            }
        }
    }

    // 1枚チップが画面で表示される時、他のはNoShowエリアにある場合、リレーション点線が表示しない
    if (nRelationCnt == 1) {
        return;
    }

    // １番目チップの点の描画
    if (gArrObjChip[arrRelationChipId[0][0]]) {
        // 画面に表示される場合、右の緑点が表示される
        var objRelationalR1 = $("<div />").addClass("PointR");
        $("#" + arrRelationChipId[0][0]).append(objRelationalR1);
        $("#" + arrRelationChipId[0][0] + " .PointR").removeClass("ArrowR").css("visibility", "visible");
    }
    // 最後チップの点の描画
    if (gArrObjChip[arrRelationChipId[nRelationCnt - 1][0]]) {
        // 画面に表示される場合、左の緑点が表示される
        var objRelationalL1 = $("<div />").addClass("PointL");
        $("#" + arrRelationChipId[nRelationCnt - 1][0]).append(objRelationalL1);
        $("#" + arrRelationChipId[nRelationCnt - 1][0] + " .PointL").removeClass("ArrowL").css("visibility", "visible");
    }

    // リレーションの点と線の表示
    for (var nLoop = 0; nLoop < nRelationCnt; nLoop++) {
        // 点のdiv
        var objRelationalL = $("<div />").addClass("PointL");
        var objRelationalR = $("<div />").addClass("PointR");

        var strRelationChipId = arrRelationChipId[nLoop][0];
        // 表示されていない場合、
        if (!gArrObjChip[strRelationChipId]) {
            continue;
        }

        // 点の表示
        // 最初の関連チップの場合、
        if ((nLoop != 0) && (nLoop != nRelationCnt - 1)) {
            $("#" + strRelationChipId).append(objRelationalL).append(objRelationalR);
            // 中のチップの場合、左右2点全部表示される
            $("#" + strRelationChipId + " .PointR").removeClass("ArrowR").css("visibility", "visible");
            $("#" + strRelationChipId + " .PointL").removeClass("ArrowL").css("visibility", "visible");
        }
 
        // 左矢印の表示、
        if (nLoop > 0) {
        	// 前のチップがない場合、左矢印を表示する
            if (!gArrObjChip[arrRelationChipId[nLoop - 1][0]]) {
                $("#" + arrRelationChipId[nLoop][0] + " .PointL").addClass("ArrowL RelationLineZIndex");
            }
        }

        // 次のチップがあれば
        if ((nLoop + 1) < nRelationCnt) {
            // 次のチップが画面に表示される場合、線を描画する
            if (gArrObjChip[arrRelationChipId[nLoop + 1][0]]) {
                // 行目を取得する
                var nStartRowNum = GetRowNoByChipId(arrRelationChipId[nLoop][0]);
                var nEndRowNum = GetRowNoByChipId(arrRelationChipId[nLoop + 1][0]);
                DrawRelationLine("Line" + arrRelationChipId[nLoop][0],
                             $("#" + arrRelationChipId[nLoop][0]).position().left + $("#" + arrRelationChipId[nLoop][0]).width(),
                             $(".Row" + nStartRowNum).position().top + ((C_CELL_HEIGHT + 1) / 2 - 1),
                             $("#" + arrRelationChipId[nLoop + 1][0]).position().left,
                             $(".Row" + nEndRowNum).position().top + ((C_CELL_HEIGHT + 1) / 2) - 1);
            } else {
                // 連続の子チップではなく場合、右矢印を表示する
                $("#" + arrRelationChipId[nLoop][0] + " .PointR").addClass("ArrowR RelationLineZIndex");
            }
        }
    }
}
/**
* リレーションチップかどうかの判断
* @param {String} strChipId チップid
* @return {bool} true:リレーションチップ
*/
function IsRelationChip(strChipId) {

    // 2013/11/29 TMEJ 張 タブレット版SMB チーフテクニシャン機能開発 START
    if ((strChipId == undefined)
        || (gArrObjRelationChip == undefined)
        || (gArrObjRelationChip == null)) {
        return false;
    }
    // 2013/11/29 TMEJ 張 タブレット版SMB チーフテクニシャン機能開発 END

    // 別の日のチップの場合
    if (strChipId == C_OTHERDTCHIPID) {
        return false;
    }
    if (gArrObjRelationChip[strChipId]) {
        return true;
    } else {
        return false;
    }
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
* チェック日付が今日仕事時間以内、前、後の判断
* @param {Date} dtDate:チェック日付
* @return {bool} 0:今日仕事時間以内　-1:今日仕事時間前 1:開始時間後
*/
function IsStartWorkDate(dtDate) {
    // 今日の仕事開始時間を取得する
    var dtTodayStartDate = GetServerTimeNow();
    dtTodayStartDate.setHours(gStartWorkTime.getHours());
    dtTodayStartDate.setMinutes(gStartWorkTime.getMinutes());
    dtTodayStartDate.setSeconds(0);
    dtTodayStartDate.setMilliseconds(0);
    // 今日の仕事終了時間を取得する
    var dtTodayEndDate = GetServerTimeNow();
    dtTodayEndDate.setHours(gEndWorkTime.getHours());
    dtTodayEndDate.setMinutes(gEndWorkTime.getMinutes());
    dtTodayEndDate.setSeconds(0);
    dtTodayEndDate.setMilliseconds(0);

    if (dtTodayStartDate - dtDate > 0) {
        return -1;
    } else if (dtDate - dtTodayEndDate > 0) {
        return 1;
    } else {
        return 0;
    }
}
/**
* チェック日付が今日仕事時間以内、前、後の判断
* @param {Date} dtCompare1:比べる日付1
* @param {Date} dtCompare2:比べる日付2
* @return {bool} 0:日付は一緒内　-1:dt1<dt2 1:d1t>dt2
*/
function CompareDate(dtCompare1, dtCompare2) {
    var dt1 = new Date(dtCompare1);
    var dt2 = new Date(dtCompare2);
    // dt1の時間は0に設定する
    dt1.setHours(0);
    dt1.setMinutes(0);
    dt1.setSeconds(0);
    dt1.setMilliseconds(0);

    // dt1の時間は0に設定する
    dt2.setHours(0);
    dt2.setMinutes(0);
    dt2.setSeconds(0);
    dt2.setMilliseconds(0);

    if (dt1 - dt2 > 0) {
        return 1;
    } else if (dt1 - dt2 < 0) {
        return -1;
    } else {
        return 0;
    }
}
/**
* チップタイプを取得する
*
* @param {String} strChipId チップid
* @return {なし}
*
*/
function GetChipType(strChipId) {
    // 2013/11/29 TMEJ 張 タブレット版SMB チーフテクニシャン機能開発 START
    // 新規予約
    if (gSelectedChipId == C_NEWCHIPID) {
        return C_FT_BTNTP_REZ_NEW;
    }
    // 2013/11/29 TMEJ 張 タブレット版SMB チーフテクニシャン機能開発 END

    var objChip;
    // Movingチップの場合、
    if (strChipId == C_MOVINGCHIPID) {
        objChip = gMovingChipObj;
    } else {
        objChip = gArrObjChip[strChipId];
    }

    // 仮仮チップの場合
    if (objChip.cstId == 0) {
        return C_FT_BTNTP_REZ_KARIKARIREZ;
    }

    // 仮予約
    if (objChip.resvStatus == C_RTYPE_TEMP) {
        // 入庫してない場合
        if (IsDefaultDate(objChip.rsltSvcInDateTime)) {
            return C_FT_BTNTP_REZ_TEMP;
        } else {
            return C_FT_BTNTP_REZ_TEMP_CARIN;
        }
    }

    // 入庫してない場合
    if (IsDefaultDate(objChip.rsltSvcInDateTime) == true) {
        if (objChip.acceptanceTpye == C_RFLG_RESERVE) {
            // 確定予約(予約客)
        	return C_FT_BTNTP_REZ_DECIDED;
        } else {
            // 確定予約(飛び込む客)
            return C_FT_BTNTP_WALKIN_DECIDED;
        }
    }

    // 来店済
    var nType = C_FT_BTNTP_WALKIN;

    // 作業開始待ちまたは作業計画の一部の作業が中断
    if ((objChip.stallUseStatus == C_STALLUSE_STATUS_STARTWAIT)
        || (objChip.stallUseStatus == C_STALLUSE_STATUS_STARTINCLUDESTOPJOB)) {
        // 中断中(ストール中断再配置)
        if (objChip.stopFlg == true) {
            nType = C_FT_BTNTP_INTERRRUPT_STALL;
        } else {
            // R/Oお客様承認(ストール)
            if (objChip.roNum != "") {
                nType = C_FT_BTNTP_RO_PUBLISHED;
            }
            //2013/11/29 TMEJ 張 タブレット版SMB チーフテクニシャン機能開発 START
//            // 作業計画確定(ストール)
//            if ((objChip.roJobSeq >= 0) && (objChip.roNum != "")) {
//                nType = C_FT_BTNTP_DECIDED_WORKPLAN;
//            }
            // 作業計画確定(ストール)
            if ((objChip.stallUseStatus == C_STALLUSE_STATUS_STARTWAIT)) {
                nType = C_FT_BTNTP_DECIDED_WORKPLAN;
            }
            //2013/11/29 TMEJ 張 タブレット版SMB チーフテクニシャン機能開発 END
        }
    }

    // 作業中
    if (IsDefaultDate(objChip.rsltStartDateTime) == false) {
        var dtShowDate = new Date($("#hidShowDate").val());
        if (CompareDate(objChip.rsltStartDateTime, dtShowDate) == 0) {
        	nType = C_FT_BTNTP_WORKING;
        } else {
            // 作業中チップ、実績開始日が表示される日と違う場合、詳細ボタンしか表示されない
            nType = C_FT_BTNTP_WORKING_NOTSTARTDAY;
        }        
    }

    // 作業完了
    if (IsDefaultDate(objChip.rsltEndDateTime) == false) {
        nType = C_FT_BTNTP_END_WORK;
    }

    // 納車済
    //2014/12/04 TMEJ 丁 新プラットフォーム版i-CROP仕様変更対応（サービス入庫実績更新処理変更）START
    //if (IsDefaultDate(objChip.rsltDeliDateTime) == false) {
    if (objChip.svcStatus == C_SVCSTATUS_DELIVERY) {
        //2014/12/04 TMEJ 丁 新プラットフォーム版i-CROP仕様変更対応（サービス入庫実績更新処理変更）END

        nType = C_FT_BTNTP_END_DELIVERY;
    }

    return nType;
}

/**
* チップの日跨ぎタイプを取得する
* @param {String} チップID
* @return {Integer} 0:日跨ぎじゃない 1:左端が日跨ぎ 2:右端が日跨ぎ 3:両端が日跨ぎ
*/
function GetChipOverDaysType(strChipId) {

    // 構造体があれば
    if (gArrObjChip[strChipId]) {
        var dtStartTime;
        var dtEndTime;
        // 予約チップの場合
        if (IsDefaultDate(gArrObjChip[strChipId].rsltStartDateTime)) {
            dtStartTime = new Date(gArrObjChip[strChipId].scheStartDateTime);
            dtEndTime = new Date(gArrObjChip[strChipId].scheEndDateTime);
        } else {
            // 作業中チップ
            if (IsDefaultDate(gArrObjChip[strChipId].rsltEndDateTime)) {
                dtStartTime = new Date(gArrObjChip[strChipId].rsltStartDateTime);

                // 見込終了日時が入っていない場合があるため、その場合は予定終了日時を利用する
                if (IsDefaultDate(gArrObjChip[strChipId].prmsEndDateTime)) {
                    dtEndTime = new Date(gArrObjChip[strChipId].scheEndDateTime);
                } else {
                dtEndTime = new Date(gArrObjChip[strChipId].prmsEndDateTime);
                }
                
            } else {
                // 作業完了チップ
                dtStartTime = new Date(gArrObjChip[strChipId].rsltStartDateTime);
                dtEndTime = new Date(gArrObjChip[strChipId].rsltEndDateTime);
            }
        }
        // 当ページの日付を取得する
        var dtShowDate = new Date($("#hidShowDate").val());
        // 開始時間と比べる
        var nStartTimeCompareResult = CompareDate(dtStartTime, dtShowDate);
        // 終了時間と比べる
        var nEndTimeCompareResult = CompareDate(dtEndTime, dtShowDate);

        // 両端が日跨ぎ
        if ((nStartTimeCompareResult < 0) && (nEndTimeCompareResult > 0)) {
            return C_OVERDAYS_BOTH;
        } else {
            // 左端が日跨ぎ
            if (nStartTimeCompareResult < 0) {
                return C_OVERDAYS_LEFT;
            }
            // 右端が日跨ぎ
            if (nEndTimeCompareResult > 0) {
                return C_OVERDAYS_RIGHT;
            }
        }
    }

    return C_OVERDAYS_NONE;
}

/**
* 使用不可チップの日跨ぎタイプを取得する
* @param {String} 使用不可チップID
* @return {Integer} 0:日跨ぎじゃない 1:左端が日跨ぎ 2:右端が日跨ぎ 3:両端が日跨ぎ
*/
function GetUnavailableChipOverDaysType(strChipId) {
    // 開始時間と終了時間を取得
    var dtStartTime = new Date($("#" + strChipId).data("START_TIME"));
    var dtEndTime = new Date($("#" + strChipId).data("END_TIME"));

    // 当ページの日付を取得する
    var dtShowDate = new Date($("#hidShowDate").val());
    // 開始時間と比べる
    var nStartTimeCompareResult = CompareDate(dtStartTime, dtShowDate);
    // 終了時間と比べる
    var nEndTimeCompareResult = CompareDate(dtEndTime, dtShowDate);
    // 両端が日跨ぎ
    if ((nStartTimeCompareResult < 0) && (nEndTimeCompareResult > 0)) {
        return C_OVERDAYS_BOTH;
    } else {
        // 左端が日跨ぎ
        if (nStartTimeCompareResult < 0) {
            return C_OVERDAYS_LEFT;
        }
        // 右端が日跨ぎ
        if (nEndTimeCompareResult > 0) {
            return C_OVERDAYS_RIGHT;
        }
    }

    return C_OVERDAYS_NONE;
}
/**
* 選択しているチップを描画する
*
* @param {Integer} nLeft 左の位置
* @param {Integer} nRowNo 何行目
* @return {なし}
*
*/
function drawSelectedChipAtPos(nLeft, nRowNo) {
    // 選択チップがない場合、戻す。
    if (gSelectedChipId == "") {
        return;
    }
    
    var strChipId = gSelectedChipId;
    // C_MOVINGCHIPIDがないの場合、新規する
    if (gMovingChipObj == null) {
        var nBackWidth = -1;
        // 選択したチップのデータが全部gMovingChipObjにコピーする
        gMovingChipObj = new ReserveChip(C_MOVINGCHIPID);
        if (strChipId == C_OTHERDTCHIPID) {
            gOtherDtChipObj.copy(gMovingChipObj);
        } else if (strChipId == C_COPYCHIPID) {
            //選択したチップがコピーされたチップの場合、
            gCopyChipObj.copy(gMovingChipObj);
        } else {
            gArrObjChip[strChipId].copy(gMovingChipObj);
            nBackWidth = $("#" + strChipId).width();
        }
        // gMovingChipObjのチップidをMovingChipに設定する
        gMovingChipObj.stallUseId = C_MOVINGCHIPID;
        if (strChipId == C_COPYCHIPID) {
            gMovingChipObj.createChip(C_CHIPTYPE_STALL_COPYMOVING);
        } else {
        	gMovingChipObj.createChip(C_CHIPTYPE_STALL_MOVING);
        }

        // 新規の場合、MovingChipの幅がC_CELL_WIDTHより小さいなので、MovingChipの幅を元のチップの幅に設定する
        var nWorkTime = gMovingChipObj.scheWorkTime;
        if (IsDefaultDate(gMovingChipObj.rsltEndDateTime) == false) {
            nWorkTime = gMovingChipObj.rsltWorkTime;
        }
        var nWidth = nWorkTime * C_CELL_WIDTH / 15;
        // 最小値(5分単位)より、小さいの場合、最小値で設定する
        if (nWidth < C_CELL_WIDTH / 15 * gResizeInterval - 1) {
            nWidth = C_CELL_WIDTH / 15 * gResizeInterval;
        }

        // 2013/11/29 TMEJ 張 タブレット版SMB チーフテクニシャン機能開発 START
        // ストール上の終了チップの場合、半透明チップが表示しないように、幅が裏のチップと同じ幅を設定する
        if ((strChipId != C_COPYCHIPID)
            && (strChipId != C_OTHERDTCHIPID)
            && (IsDefaultDate(gMovingChipObj.rsltEndDateTime) == false)
            && (gArrObjChip[strChipId])) {
            nWidth = nBackWidth + 1;
        }
        // 2013/11/29 TMEJ 張 タブレット版SMB チーフテクニシャン機能開発 END

        // 日跨ぎタイプを取得する
        var nOverDaysType = GetChipOverDaysType(strChipId);
        // 日跨ぎの場合
        if (nOverDaysType > C_OVERDAYS_NONE) {
            // 幅が選択チップの表示てる部分と同じ用にする
            nWidth = $("#" + strChipId).width() + 1;

            switch (nOverDaysType) {
                // 左端が日跨ぎ  
                case C_OVERDAYS_LEFT:
                    // 左の爪を削除する
                    $("#" + C_MOVINGCHIPID + " .TimeKnobPointL").remove();
                    // 右端しかリサイズできない
                    BindChipResize(C_MOVINGCHIPID, 0, 2);
                    break;
                // 右端が日跨ぎ   
                case C_OVERDAYS_RIGHT:
                    // 右の爪を削除する
                    $("#" + C_MOVINGCHIPID + " .TimeKnobPointR").remove();
                    // 左端しかリサイズできない
                    BindChipResize(C_MOVINGCHIPID, 0, 1);
                    break;
                // 両端が日跨ぎ   
                case C_OVERDAYS_BOTH:
                    // 左右の爪を削除する
                    $("#" + C_MOVINGCHIPID + " .TimeKnobPointL").remove();
                    $("#" + C_MOVINGCHIPID + " .TimeKnobPointR").remove();
                    break;
            }
        } else {
            // Movingチップのりサイズをbindする
            BindChipResize(C_MOVINGCHIPID, 0, 0);
        }

        // widthを設定する
        $("#" + C_MOVINGCHIPID).css("width", nWidth - 1);

        // 幅によって、チップに表示内容を調整する
        AdjustChipItemByWidth(C_MOVINGCHIPID);

        // Movingチップの爪以外の部分が半透明
        $("#" + C_MOVINGCHIPID + " .CpInner").css("opacity", C_OPACITY_TRANSPARENT);

        // 幅が同じ場合、爪以外の部分が見えないようにする
        if ((nBackWidth != -1) && (Math.abs(nBackWidth - (nWidth - 1)) < 1)) { 
            $("#" + C_MOVINGCHIPID + " .CpInner").css("visibility", "hidden");
        } 

        // Movingチップのz-indexを追加する
        ChangeChipZIndex(C_MOVINGCHIPID, "MovingChipZIndex");
        // Movingチップのタップイベントのbind
        BindMovingChipTapEvent();

        // コピー、日跨ぎ移動の場合、フッターボタンが変更されない
        if ((gSelectedChipId != C_COPYCHIPID) && (gSelectedChipId != C_OTHERDTCHIPID)) {
            // フッターボタンを変える
            //2014/06/18 TMEJ 丁 タブレット版SMB テレマ走行距離機能開発 START
            //CreateFooterButton(C_FT_DISPTP_SELECTED, GetChipType(C_MOVINGCHIPID));
            CreateFooterButton(C_FT_DISPTP_SELECTED, GetChipType(C_MOVINGCHIPID), gArrObjChip[strChipId].tlmContractFlg);
            //2014/06/18 TMEJ 丁 タブレット版SMB テレマ走行距離機能開発 END                
        }
    }

    $("#" + C_MOVINGCHIPID).css("left", nLeft);
    $(".Row" + nRowNo).append($("#" + C_MOVINGCHIPID));

}

/**
* チップの選択状態を解除する
*
* @return {なし}
*
*/
function SetChipUnSelectedStatus() {

    switch (gSelectedChipId) {
        case "":
            break;
        // 2013/11/29 TMEJ 張 タブレット版SMB チーフテクニシャン機能開発 START 
        case C_NEWCHIPID:
            // 新規チップが消える
            $("#" + C_NEWCHIPID).remove();
            gArrObjChip[C_NEWCHIPID] = null;
            break;
        // 2013/11/29 TMEJ 張 タブレット版SMB チーフテクニシャン機能開発 END 
        case C_OTHERDTCHIPID:
            // MovingChipがある場合
            if (gOtherDtChipObj != null) {
                if (gOtherDtChipObj.stallUseId) {
                    $("#" + gOtherDtChipObj.stallUseId).remove();
                } else {
                    $("#" + C_UNAVALIABLECHIPID + gOtherDtChipObj.stallIdleId).remove();
                }
                // MovingChipのオブジェクトをnullに設定する
                gOtherDtChipObj = null;
            }
            // MovingChipがある場合
            if (gMovingChipObj != null) {
                // MovingChipを削除する
                $("#" + C_MOVINGCHIPID).remove();
                // MovingChipのオブジェクトをnullに設定する
                gMovingChipObj = null;
            }
            break;
        case C_COPYCHIPID:
            // リレーションコピーチップ
            if (gCopyChipObj != null) {
                $("#" + C_COPYCHIPID).remove();
                // MovingChipのオブジェクトをnullに設定する
                gCopyChipObj = null;
            }
            // MovingChipがある場合
            if (gMovingChipObj != null) {
                // MovingChipを削除する
                $("#" + C_MOVINGCHIPID).remove();
                // MovingChipのオブジェクトをnullに設定する
                gMovingChipObj = null;
            }
            break;
        default:
            //使用不可エリアの移動チップの場合、削除
            if (IsUnavailableArea(gSelectedChipId)) {
                $("#" + C_MOVINGUNAVALIABLECHIPID).remove();
                ChangeChipZIndex(gSelectedChipId, "");
            }
            // MovingChipがある場合、削除
            if (gMovingChipObj != null) {
                // MovingChipを削除する
                $("#" + C_MOVINGCHIPID).remove();
                // MovingChipのオブジェクトをnullに設定する
                gMovingChipObj = null;
            }
            // リレーションチップ全体
            var arrRelationChipId;
            if (gArrObjSubChip[gSelectedChipId]) {
                // リレーションチップを取得する(サブチップ)
                var strSvcInId = GetSubChipSVCINID(gSelectedChipId)
                arrRelationChipId = FindRelationChips("", strSvcInId);
            } else {
                // リレーションチップを取得する（ストールチップ）
                arrRelationChipId = FindRelationChips(gSelectedChipId, "");
            }
            if (arrRelationChipId.length == 0) {
                // 影を削除する
                $("#" + gSelectedChipId).removeClass("SelectedChipShadow");
                // メインストールのチップだけ、
                if ((gArrObjChip[gSelectedChipId]) && ($("#" + gSelectedChipId).length > 0)) {
                    // 選択状態を解除する時、白い枠とz-indexを再計算する
                    CreateWhiteBorders(gSelectedChipId);
                }
            } else {
                // 選択したチップの影を削除する(メインストールのチップも、サブエリアチップも)
                for (var nLoop = 0; nLoop < arrRelationChipId.length; nLoop++) {
                    // 影を削除する
                    $("#" + arrRelationChipId[nLoop][0]).removeClass("SelectedChipShadow");
                    // メインストールのチップだけ、
                    if ((gArrObjChip[arrRelationChipId[nLoop][0]]) 
                        && ($("#" + arrRelationChipId[nLoop][0]).length > 0)) {
                        // 選択状態を解除する時、白い枠とz-indexを再計算する
                        CreateWhiteBorders(arrRelationChipId[nLoop][0]);
                    }
                }
            }
            // リレーションチップのZ-INDEXを削除
            $(".SelectedRelationWorkOverChipZIndex").removeClass("SelectedRelationWorkOverChipZIndex");
            break;
    }
    // チップを選択中ステータスに設定する
    gSelectedChipId = "";
    gSelectedCellId = "";

    // フッターボタンを変える
    CreateFooterButton(C_FT_DISPTP_UNSELECTED, 0);

    // リレーションの点を非表示する
    $(".PointR").remove();
    $(".PointL").remove();
    // リレーションの線を非表示する
    $(".RelationalLine").remove();
    // 納車予定時刻線を非表示にする
    $(".TimingLineDeli").css("visibility", "hidden");
    // ポップアップ時間を非表示
    HideTimingBalloon();
}

/**
* 爪(右)の位置はチップの幅によって、座標を設定する
*
* @param {String} strChipId 選択中のチップのid
* @return {なし}
*
*/
function SetTimeKnobPointRPosbyChipWidth(strChipId) {
    //チップの右側に表示する●のleft位置を動的に変更(-5は●の横幅に合わせる必要あり)
    var rCircleLeftPos = $("#" + strChipId).width() - 5;
    $(".TimeKnobPointR").css("left", rCircleLeftPos);
}

// 2013/11/29 TMEJ 張 タブレット版SMB チーフテクニシャン機能開発 START
/**
* 指定位置にC_NEWCHIPIDチップが生成される
* @param {Integer} nRowNo 行目
* @param {Integer} nColNo 列目
* @return {なし}
*/
function CreateNewChip(nRowNo, nColNo) {
    
    // 遅れストールだけ表示する場合
    if (gShowLaterStallFlg == true) {

        // 2014/07/15 TMEJ 張 タブレットSMB Job Dispatch機能開発 START
        // 今のXのスクロール位置を記録する
        var nBackupXPos = $(".scroll-inner").position().left;
        // 2014/07/15 TMEJ 張 タブレットSMB Job Dispatch機能開発 END

        // 全ストール表示する
        ShowAllStall();

        // 2014/07/15 TMEJ 張 タブレットSMB Job Dispatch機能開発 START
        // 全ストール表示時、xPosが一番左にスクロールしたので、バックアップのXPosに戻る
        $(".ChipArea_trimming").SmbFingerScroll({
            action: "move", moveY: 0, moveX: -nBackupXPos
        });
        // 2014/07/15 TMEJ 張 タブレットSMB Job Dispatch機能開発 END
    }

    var strBackSelectedChipId = gSelectedChipId;
    // gSelectedChipIdに選択したチップのidを保存する
    gSelectedChipId = C_NEWCHIPID;
    if (gArrObjChip[C_NEWCHIPID] == undefined) {
        gArrObjChip[C_NEWCHIPID] = new ReserveChip(C_NEWCHIPID);
    }
    gArrObjChip[C_NEWCHIPID].createChip(C_CHIPTYPE_STALL_NEW);

    var nLeft = (nColNo - 1) * C_CELL_WIDTH + 1;
    // 新規チップの幅はなんセル分を計算
    var nWidth = C_NEWCHIPID_COLUM_NUM;
    //最後列目の場合、新規チップの幅が1セル
    if (gMaxCol == nColNo) {
        nWidth = 1;
    } else if ((gMaxCol - 1) == nColNo) {
        //最後2列目の場合、新規チップの幅が2セル
        nWidth = 2;
    }

    // 新規した前のセルの青色を消しる
    $("#BlueDiv").css("visibility", "hidden");

    $("#" + C_NEWCHIPID).css("left", nLeft)
                        .css("width", C_CELL_WIDTH * nWidth - 1)
                        .css("visibility", "visible");
    $(".Row" + nRowNo).append($("#" + C_NEWCHIPID));

    SetSubChipBoxClose();

    // ポップアップボックスが表示される時、削除する
    if (gPopupBoxId != "") {
        RemovePopUpBox();
    }

    setTimeout(function () {
        // タッチしたセルにチップが生成できるかどうかのをチェックする
        if (CheckChipPos(C_NEWCHIPID) == false) {
            // エラーメッセージ「他のチップと配置時間が重複します。」を表示する
            ShowSC3240101Msg(906);
            SetTableUnSelectedStatus();
            SetChipUnSelectedStatus();
            return;
        }
        // 新規チップをタップする時のイベントを登録
        BindChipClickEvent(C_NEWCHIPID);

        // C_NEWCHIPIDのresizableを設定する
        BindChipResize(C_NEWCHIPID);

        // NEWチップの爪の位置を設定する
        SetTimeKnobPointRPosbyChipWidth(C_NEWCHIPID);

        // フッターボタンを変える
        CreateFooterButton(C_FT_DISPTP_SELECTED, C_FT_BTNTP_REZ_NEW);
        // 新規チップのプロトタイプにデータを更新する
        SetChipPrototypeTimeAndStallIdData(C_NEWCHIPID);
        // テーブルの状態をチップの選択状態に設定する
        SetTableSelectedStatus();
    }, 100);
}
// 2013/11/29 TMEJ 張 タブレット版SMB チーフテクニシャン機能開発 END

/**
* チップの幅を設定する
* @return {なし}
*/
function GetCellNumByWidth(nWidth) {
    return Math.ceil((nWidth + 1) / C_CELL_WIDTH);
}
/**
* チップのIDにより、行数を取得する
* @param {String} strChipId 
* @return {Integer} 行数目
*/
function GetRowNoByChipId(strChipId) {
    // 2013/11/29 TMEJ 張 タブレット版SMB チーフテクニシャン機能開発 START
    //    return $("#" + strChipId).offsetParent().position().top / C_CELL_HEIGHT + 1;
    return Math.round($("#" + strChipId).offsetParent().position().top / C_CELL_HEIGHT + 1);
    // 2013/11/29 TMEJ 張 タブレット版SMB チーフテクニシャン機能開発 END
}

/**
* YPosにより、行目を取得する
* @param {Integer} nYPos
* @return {Integer} 行数目
*/
function GetRowNoByYPos(nYPos) {
    return Math.ceil(nYPos / C_CELL_HEIGHT);
}

/**
* xPosにより、行目を取得する
* @param {Integer} nXPos
* @return {Integer} 列数目
*/
function GetColNoByXPos(nXPos) {
    return Math.floor(nXPos / C_CELL_WIDTH + 1);
}
/**
* タッチしたxPosにより、行目を取得する
* @param {Integer} nXPos
* @return {Integer} 列数目
*/
function GetColNoByTouchXPos(nXPos) {
    return Math.floor((nXPos - C_SCREEN_CHIPAREA_OFFSET_LEFT) / C_CELL_WIDTH + 1);
}
/**
* ストールid、開始と終了時間によって、チップの位置を設定する
* @param {String} strChipId  チップid
* @param {Date/String} strStallId 指定ストールid Stringの場合 HH:MMの形式
* @param {Date/String} strStartTime 指定開始時間 Stringの場合 HH:MMの形式
* @param {Date/String} strEndTime   指定終了時間 Stringの場合 HH:MMの形式
* @return {なし}
*/
function SetChipPosition(strChipId, stallId, startTime, endTime) {

    var nTop, nLeft, nWidth;
    var nRowNo;
    var hour, minutes;
    var tStartTime, tEndTime;

    // startTimeは指定してない場合、チップのdisplayStartDateでstarttimeにする
    if (startTime == "") {
        tStartTime = gArrObjChip[strChipId].displayStartDate;
    } else {
        if (typeof startTime == "string") {
            strStartTime = $("#hidShowDate").val() + " " + startTime + ":00";
            tStartTime = new Date(strStartTime)
        } else {
            tStartTime = startTime;
        }
    }

    // endTimeは指定してない場合、チップのdisplayStartDateでendTimeにする
    if (endTime == "") {
        tEndTime = gArrObjChip[strChipId].displayEndDate;
    } else {
        if (typeof endTime == "string") {
            strEndTime = $("#hidShowDate").val() + " " + endTime + ":00";
            tEndTime = new Date(strEndTime)
        } else {
            tEndTime = endTime;
        }
    }

    // stallIdは指定してない場合、チップのdisplayStartDateでstallIdにする
    if (stallId == "") {
        nStallId = gArrObjChip[strChipId].stallId;
    } else {
        nStallId = stallId;
    }

    var dtShowDate = new Date($("#hidShowDate").val());
    // 今の日付と違う場合、
    if ((dtShowDate.getFullYear() != tStartTime.getFullYear())
        || (dtShowDate.getMonth() != tStartTime.getMonth())
        || (dtShowDate.getDate() != tStartTime.getDate())) {
        // 削除する
        if (strChipId == gSelectedChipId) {
            $("#" + strChipId).remove();
            gSelectedChipId = "";
            if (gMovingChipObj) {
                // MovingChipを削除する
                $("#" + C_MOVINGCHIPID).remove();
                // MovingChipのオブジェクトをnullに設定する
                gMovingChipObj = null;
            }
        }        
        return;    
    }
    // 左の位置と幅を計算する
    nLeft = Math.round(GetXPosByTime(tStartTime)) + 1;
    nWidth = GetWidthByDisplayTime(tStartTime, tEndTime);

    // ストールidで行数目を取得する
    nRowNo = $("#stallId_" + nStallId).position().top / C_CELL_HEIGHT + 1;

    // 設定
    $("#" + strChipId).css("left", nLeft);
    $("#" + strChipId).css("width", nWidth);
    // 幅により文字を調整する
    AdjustChipItemByWidth(strChipId);

    $(".Row" + nRowNo).append($("#" + strChipId));
}
/**
* チップの位置を設定する(速く版)
* @param {Object} objChip  チップ構造体
* @return {なし}
*/
function FasterSetChipPosition(objChip) {

    // 左の位置と幅を計算する
    var nLeft = Math.round(GetXPosByTime(objChip.displayStartDate)) + 1;
    nWidth = GetWidthByDisplayTime(objChip.displayStartDate, objChip.displayEndDate)

    // 設定
    $("#" + objChip.stallUseId).css("left", nLeft).css("width", nWidth);

    // 幅により文字を調整する
    FasterAdjustChipItemByWidth(objChip.stallUseId, nWidth);
}
/**
* 幅によって、チップに表示内容を調整する
* @param {String} strChipId  チップid
* @param {String} strVclRegNo  車両番号
* @param {Integer} nChipWidth  車両番号
* @return {なし}
*/
function FasterAdjustChipItemByWidth(strChipId, nChipWidth) {

    // チップの幅は1つセルの幅以下の場合
    if (nChipWidth <= C_CELL_WIDTH) {
        // 車種名エリアを表示しない
        $("#" + strChipId + " h3").css("display", "none");

        // 左下側のアイコン部分表示しない
        if (($("#" + strChipId + " .infoBox").prev().hasClass("time") == false)
            && $("#" + strChipId + " .infoBox").prev().html() == "") {
            $("#" + strChipId + " .infoBox").prev().css("display", "none");
        }

        // 右下の情報部
        if (nChipWidth < 40) {
            $("#" + strChipId + " .infoBox").css("width", nChipWidth - 5);
        } else {
            $("#" + strChipId + " .infoBox").css("width", 36);
        }

        // 時間エリア
        if (nChipWidth < 35) {
            $("#" + strChipId + " .time").css("width", nChipWidth - 3);
        } else {
            $("#" + strChipId + " .time").css("width", 33);
        }

        // 店内店外アイコン
        if (nChipWidth < 26) {
            $("#" + strChipId + " .IC02").css("left", nChipWidth - 12);
        } else {
            $("#" + strChipId + " .IC02").css("left", 13);
        }

        // 予約アイコン
        if (nChipWidth < 18) {
            $("#" + strChipId + " .IC03").css("width", nChipWidth - 3);
            //2014/06/18 TMEJ 丁 タブレット版SMB テレマ走行距離機能開発 START
            $("#" + strChipId + " .GIcon").css("right", 0);
            //2014/06/18 TMEJ 丁 タブレット版SMB テレマ走行距離機能開発 END
        } else {
            $("#" + strChipId + " .IC03").css("width", 15);
            //2014/06/18 TMEJ 丁 タブレット版SMB テレマ走行距離機能開発 START
            $("#" + strChipId + " .GIcon").css("right", 4);
            //2014/06/18 TMEJ 丁 タブレット版SMB テレマ走行距離機能開発 END
        }
        //2018/07/06 NSK 可児 TKM Next Gen e-CRB Project Application development Block B-1  Pマーク、Lマークなどを表示 START
        // P/Lアイコン 
        if (nChipWidth < 18) { //予約アイコンと同じサイズにする 
            $("#" + strChipId + " .IconL").css("right", 2);
            $("#" + strChipId + " .IconL").css("width", nChipWidth - 3);
            $("#" + strChipId + " .IconP").css("right", 2);
            $("#" + strChipId + " .IconP").css("width", nChipWidth - 3);
            $("#" + strChipId + " .IC03").css("right", 2);
        } else if (nChipWidth < 35) { //PLマークがはみ出す場合は予約アイコンに被せる 
            $("#" + strChipId + " .IconL").css("right", 2);
            $("#" + strChipId + " .IconP").css("right", 2);
            $("#" + strChipId + " .IC03").css("right", 2);
            $("#" + strChipId + " .IconL").css("width", 15);
            $("#" + strChipId + " .IconP").css("width", 15);
        } else {
            $("#" + strChipId + " .IconP").css("width", 15);
            $("#" + strChipId + " .IconL").css("width", 15);
            $("#" + strChipId + " .IC03").css("right", 4);
            if ($("#" + strChipId).find(".IC03").length) {
                $("#" + strChipId + " .IconL").css("right", 20);
                $("#" + strChipId + " .IconP").css("right", 20);
            } else {
                $("#" + strChipId + " .IconL").css("right", 4);
                $("#" + strChipId + " .IconP").css("right", 4);
            }
        }
        //2018/07/06 NSK 可児 TKM Next Gen e-CRB Project Application development Block B-1  Pマーク、Lマークなどを表示 END
    } else {
        // 全部表示すして、アイコンのサイズ、座標が元に戻す
        $("#" + strChipId + " h3").css("display", "block");
        $("#" + strChipId + " .infoBox").prev().css("display", "block");
        $("#" + strChipId + " .infoBox").css("width", 36);
        $("#" + strChipId + " .time").css("width", 33);
        $("#" + strChipId + " .IC02").css("left", 13);
        $("#" + strChipId + " .IC03").css("width", 15);
        //2014/06/18 TMEJ 丁 タブレット版SMB テレマ走行距離機能開発 START
        $("#" + strChipId + " .GIcon").css("right", 4);
        //2014/06/18 TMEJ 丁 タブレット版SMB テレマ走行距離機能開発 END
        //2018/07/06 NSK 可児 TKM Next Gen e-CRB Project Application development Block B-1  Pマーク、Lマークなどを表示 START
        if ($("#" + strChipId).find(".IC03").length) {
            $("#" + strChipId + " .IconL").css("right", 20);
            $("#" + strChipId + " .IconP").css("right", 20);
        } else {
            $("#" + strChipId + " .IconL").css("right", 4);
            $("#" + strChipId + " .IconP").css("right", 4);
        }
        $("#" + strChipId + " .IconL").css("width", 15);
        $("#" + strChipId + " .IconP").css("width", 15);
        // 店内店外アイコン
        if (nChipWidth < 60) {
            $("#" + strChipId + " .IC02").css("left", nChipWidth - 50);
        }
        //2018/07/06 NSK 可児 TKM Next Gen e-CRB Project Application development Block B-1  Pマーク、Lマークなどを表示 END
    }
}
/**
* 車両番号文字が左寄せ、右よせを設定する
* @param {String} strChipId  チップid
* @param {String} strVclRegNo  車両番号
* @param {Integer} nChipWidth  車両番号
* @return {なし}
*/
function ChangeCarNoTextAlign(strChipId) {
    //車両番号が左寄せか、右寄せかを判断する
    if ($("#" + strChipId + " .CarNoL").length > 0) {
        if ($("#" + strChipId + " .CarNoL").width() <= $("#" + strChipId + " .CarNoL span").width()) {
            $("#" + strChipId + " .CarNoL").addClass("CarNoR").removeClass("CarNoL");
        }
    } else {
        if ($("#" + strChipId + " .CarNoR").width() > $("#" + strChipId + " .CarNoR span").width()) {
            $("#" + strChipId + " .CarNoR").addClass("CarNoL").removeClass("CarNoR");
        } 
    }
}
/**
* 車両番号文字が左寄せ、右よせを設定する
* @param {String} strChipId  チップid
* @param {String} strVclRegNo  車両番号
* @param {Integer} nChipWidth  車両番号
* @return {なし}
*/
function ChangeAllStallChipCarNoTextAlign() {
    for (var strId in gArrObjChip) {
        // 2013/11/29 TMEJ 張 タブレット版SMB チーフテクニシャン機能開発 START
        if (CheckgArrObjChip(strId) == false) {
            continue;
        }
        // 2013/11/29 TMEJ 張 タブレット版SMB チーフテクニシャン機能開発 END
        ChangeCarNoTextAlign(strId);
    }
}
/**
* 幅によって、チップに表示内容を調整する
* @param {String} strChipId  チップid
* @param {String} strVclRegNo  車両番号
* @return {なし}
*/
function AdjustChipItemByWidth(strChipId) {
    var nChipWidth = $("#" + strChipId).width();
    ChangeCarNoTextAlign(strChipId);
    FasterAdjustChipItemByWidth(strChipId, nChipWidth);
}
/**
* 配列gArrObjChipをチェックする
* @param {String} strChipId  チップid
* @return {bool} true:gArrObjChip[strChipId]が有効のチップデータ
*/
function CheckgArrObjChip(strChipId) {
    if ((gArrObjChip[strChipId] == null) || (typeof gArrObjChip[strChipId] != "object")) {
        return false;
    } else {
        return true;
    }
}

/**
* x軸の位置により、時間を取得する
* @param {Integer} nXPos  x軸の位置
* @return {Date} 時間
*/
function GetTimeByXPos(nXPos) {
    var dtDay = new Date($("#hidShowDate").val());
    // ストール開始時間
    dtDay.setHours(gStartWorkTime.getHours());
    dtDay.setMinutes(0);
    dtDay.setSeconds(0);
    dtDay.setMilliseconds(0);

    var nStartHour = gStartWorkTime.getHours();
    var nEndHour = gEndWorkTime.getHours();
    if (gEndWorkTime.getMinutes() > 0) {
        nEndHour += 1;
    }

    var time = nXPos / $(".TbRow").width() * ((nEndHour - nStartHour) * 60 * 60 * 1000) + dtDay.getTime();
    dtDay.setTime(time);
    var nSeconds = dtDay.getSeconds();
    // 分以後の単位が切る
    dtDay.setSeconds(0);
    dtDay.setMilliseconds(0);
    // ３０秒以上の場合、１分をプラスする
    if (nSeconds >= 30) {
        dtDay.setTime(dtDay.getTime() + 60000);
    }
    return dtDay;
}
/**
* チップの表示時間により、幅を計算する
* @param {Date} dtStartTime  表示開始時間
* @param {Date} dtEndTime    表示終了時間
* @return {Integer}  nWidth 幅
*/
function GetWidthByDisplayTime(dtStartTime, dtEndTime) {
    var nWidth = Math.round(GetXPosByTime(dtEndTime) - GetXPosByTime(dtStartTime)) - 1;
    // 5分より小さい場合、見えない
    if (nWidth < C_CELL_WIDTH / 15 * gResizeInterval - 1) {
        nWidth = C_CELL_WIDTH / 15 * gResizeInterval - 1;
    }
    return nWidth;
}
/**
* 時間により、xPosを取得する
* @param {Date} dtDate  年月日 
* @return {Integer}  nXPos  x軸の位置
*/
function GetXPosByTime(dtDate) {
    // 今日の仕事開始時間を取得する
    var dtTodayStartDate = new Date($("#hidShowDate").val());
    dtTodayStartDate.setHours(gStartWorkTime.getHours());
    dtTodayStartDate.setMinutes(0);
    dtTodayStartDate.setSeconds(0);
    dtTodayStartDate.setMilliseconds(0);
    // 今日の仕事終了時間を取得する
    var dtTodayEndDate = new Date($("#hidShowDate").val());
    var nEndTime = gEndWorkTime.getHours();
    if (gEndWorkTime.getMinutes() > 0) {
        nEndTime += 1;
    }
    dtTodayEndDate.setHours(nEndTime);
    dtTodayEndDate.setMinutes(0);
    dtTodayEndDate.setSeconds(0);
    dtTodayEndDate.setMilliseconds(0);
    var rtValue = ((dtDate.getTime() - dtTodayStartDate.getTime()) / (dtTodayEndDate.getTime() - dtTodayStartDate.getTime()) * (gMaxCol * C_CELL_WIDTH));
    return rtValue;
}

/**
* 時間により、時刻線の位置を取得する
* @param {Date} dtDate  年月日 
* @return {Integer}  nXPos  x軸の位置
*/
function GetTimeLinePosByTime(dtDate) {

    var dt = new Date();
    dt.setTime(dtDate.getTime());

    var dtShowDate = new Date($("#hidShowDate").val());
    var nCompareValue = CompareDate(dtShowDate, dt);
    // 当ページ以外の場合、-1を戻す
    if (nCompareValue != 0) {
        return -1;
    }

    // 今日の仕事開始時間を取得する
    var setPosition;
    var hour = dt.getHours();
    var minutes = dt.getMinutes();
    var nowtime = hour * 60 + minutes;
    var starttime = gStartWorkTime.getHours() * 60;
    var endtime = gEndWorkTime.getHours() * 60;
    if (gEndWorkTime.getMinutes() > 0) {
        endtime += 60;
    } 

    // スクロールDivの幅
    var nScrollDivWidth = $(".ChipArea").width() - C_CHIPAREA_OFFSET_LEFT;
    setPosition = (nowtime - starttime) * nScrollDivWidth / (endtime - starttime) + C_CHIPAREA_OFFSET_LEFT;
    var nTimeOffset = $(".TimingLine01").offset().left - $(".TimingLineSet").offset().left;
    setPosition -= nTimeOffset;

    return setPosition;
}

/**
* チップのプロトタイプにデータを設定する
* @param {String} strChipId  チップid
* @return {Bool} true:変化がある 
*/
function SetChipPrototypeTimeAndStallIdData(strChipId) {
    var rtChangedFlg;
    var updateType = GetChipUpdatetype(strChipId);
    if (updateType == C_UPDATA_STALLCHIP) {
        rtChangedFlg = false;
    } else {
        rtChangedFlg = true;
    }
    if ((strChipId == "")
        || (strChipId == null)
        || ((gArrObjChip[strChipId] == null) && (gArrObjSubChip[strChipId] == null))) {
        return rtChangedFlg;
    }

    // 作業完了の場合、戻る
    if (IsDefaultDate(gArrObjChip[strChipId].rsltEndDateTime) == false) {
        return false;
    }

    // チップidより、チップの左と右の座標を取得する
    // 2013/11/29 TMEJ 張 タブレット版SMB チーフテクニシャン機能開発 STAR
    var strMovingChip;
    if (strChipId == C_NEWCHIPID) {
        strMovingChip = C_NEWCHIPID;
    } else {
        strMovingChip = C_MOVINGCHIPID;
    }

//    var nMovingChipLeft = $("#" + C_MOVINGCHIPID).position().left;
//    var nMovingChipWidth = $("#" + C_MOVINGCHIPID).width();
    var nMovingChipLeft = $("#" + strMovingChip).position().left;
    var nMovingChipWidth = $("#" + strMovingChip).width();
    // 2013/11/29 TMEJ 張 タブレット版SMB チーフテクニシャン機能開発 END
    // 座標により、時間を取得する
    var tStartTime = GetTimeByXPos(nMovingChipLeft - 1);
    var tEndTime = GetTimeByXPos(nMovingChipLeft + nMovingChipWidth);

    // 行数目を取得する
    var nRowNo = GetRowNoByChipId(strChipId);
    var strStallId = $(".stallNo" + nRowNo)[0].id;
    if (strStallId.length > 7) {
        strStallId = strStallId.substring(8, strStallId.length);
    }
    // 変更する前のデータをバックアップする
    BackupRelationChips(strChipId);

    var nChangeStartTime = 0;
    var nChangeEndTime = 0;

    // 予約チップの場合、開始時間、ストールを変えれる
    if (IsDefaultDate(gArrObjChip[strChipId].rsltStartDateTime)) {

        // ストールidを設定する
        if ((strStallId != "") && (strStallId != gArrObjChip[strChipId].stallId)) {
            gArrObjChip[strChipId].setStallId(strStallId);
            rtChangedFlg = true;
        }

        // 左端の爪がある場合
        // 2013/11/29 TMEJ 張 タブレット版SMB チーフテクニシャン機能開発 STAR
//        if ($("#" + C_MOVINGCHIPID + " .TimeKnobPointL").length == 1) {
        if ($("#" + strMovingChip + " .TimeKnobPointL").length == 1) {
            // 2013/11/29 TMEJ 張 タブレット版SMB チーフテクニシャン機能開発 END
            // 開始時間を設定する
            nChangeStartTime = gArrObjChip[strChipId].displayStartDate - tStartTime;
            if (nChangeStartTime != 0) {
                gArrObjChip[strChipId].setScheStartDateTime(tStartTime);
                gArrObjChip[strChipId].setDisplayStartDate(tStartTime);
                rtChangedFlg = true;
            }
        }

        // 右端の爪がある場合
        // 2013/11/29 TMEJ 張 タブレット版SMB チーフテクニシャン機能開発 STAR
//        if ($("#" + C_MOVINGCHIPID + " .TimeKnobPointR").length == 1) {
        if ($("#" + strMovingChip + " .TimeKnobPointR").length == 1) {
            // 2013/11/29 TMEJ 張 タブレット版SMB チーフテクニシャン機能開発 END
            // 作業完了時間を設定する
            nChangeEndTime = gArrObjChip[strChipId].displayEndDate - tEndTime;
            if (nChangeEndTime != 0) {
                // 終了時間を設定する
                gArrObjChip[strChipId].setScheEndDateTime(tEndTime);
                gArrObjChip[strChipId].setDisplayEndDate(tEndTime);
                rtChangedFlg = true;
            }
        }

    } else {

        // 作業中場合
        // 右端の爪がある場合
        if ($("#" + C_MOVINGCHIPID + " .TimeKnobPointR").length == 1) {
            // 作業完了時間を設定する
            nChangeEndTime = gArrObjChip[strChipId].displayEndDate - tEndTime;
            if (nChangeEndTime != 0) {
                // 終了時間を設定する
                gArrObjChip[strChipId].setPrmsEndDateTime(tEndTime);
                gArrObjChip[strChipId].setDisplayEndDate(tEndTime);
                rtChangedFlg = true;
            }
        }
    }

    // 分にする
    nChangeStartTime = nChangeStartTime / 60 / 1000;
    nChangeEndTime = nChangeEndTime / 60 / 1000;

    //2015/03/16 TMEJ 明瀬 既存バグ修正(関連チップをコピー元と同じ時間で作成するとタップできない) START
//    // 作業時間を設定する
//    if (rtChangedFlg) {
//        var nOverDaysType = GetChipOverDaysType(strChipId);
//        var nScheWorkTime;
//        // 日跨ぎではないチップの場合、Movingチップの幅が実績の幅
//        if ((nOverDaysType == C_OVERDAYS_NONE)
//        // 日跨ぎのに、左右爪があるチップも、Movingチップの幅が実績の幅
//            || (($("#" + C_MOVINGCHIPID + " .TimeKnobPointL").length == 1) && ($("#" + C_MOVINGCHIPID + " .TimeKnobPointR").length == 1))) {
//            nScheWorkTime = (gArrObjChip[strChipId].displayEndDate - gArrObjChip[strChipId].displayStartDate) / 60 / 1000;
//        } else {
//            // 変更時間差を計算する
//            nScheWorkTime = gArrObjChip[strChipId].scheWorkTime + nChangeStartTime - nChangeEndTime;
//            if (nScheWorkTime <= gResizeInterval) {
//                nScheWorkTime = gResizeInterval;
//            }
//        }
//        gArrObjChip[strChipId].setScheWorkTime(nScheWorkTime);
//    }

    // 作業時間を設定する
    var nOverDaysType = GetChipOverDaysType(strChipId);
    var nScheWorkTime;
    // 日跨ぎではないチップの場合、Movingチップの幅が実績の幅
    if ((nOverDaysType == C_OVERDAYS_NONE)
    // 日跨ぎのに、左右爪があるチップも、Movingチップの幅が実績の幅
        || (($("#" + C_MOVINGCHIPID + " .TimeKnobPointL").length == 1) && ($("#" + C_MOVINGCHIPID + " .TimeKnobPointR").length == 1))) {
        nScheWorkTime = (gArrObjChip[strChipId].displayEndDate - gArrObjChip[strChipId].displayStartDate) / 60 / 1000;
    } else {
        // 変更時間差を計算する
        nScheWorkTime = gArrObjChip[strChipId].scheWorkTime + nChangeStartTime - nChangeEndTime;
        if (nScheWorkTime <= gResizeInterval) {
            nScheWorkTime = gResizeInterval;
        }
    }
    gArrObjChip[strChipId].setScheWorkTime(nScheWorkTime);
    //2015/03/16 TMEJ 明瀬 既存バグ修正(関連チップをコピー元と同じ時間で作成するとタップできない) END

    return rtChangedFlg;
}
/**
* 予約確定ボタンを押す
* @return {-} 無し
*/
function ClickRezConFirmed() {
    var strChipId = gSelectedChipId;
    // 変更する前のデータをバックアップする
    BackupRelationChips(strChipId);

    // 当画面にリレーションチップを取得する
    var arrRelationChips = FindRelationChips(strChipId, "");

    if (arrRelationChips.length == 0) {
        // 仮予約→本予約
        gArrObjChip[strChipId].setResvStatus(C_RTYPE_COMMITTED);
        // チップを更新する
        gArrObjChip[strChipId].updateStallChip();
    } else {
        // リレーションチップ全体仮予約→本予約に変更する
        for (var nLoop = 0; nLoop < arrRelationChips.length; nLoop++) {
            // 画面に表示される時
            if (gArrObjChip[arrRelationChips[nLoop][0]]) {
                // 仮予約→本予約
                gArrObjChip[arrRelationChips[nLoop][0]].setResvStatus(C_RTYPE_COMMITTED);
                // チップを更新する
                gArrObjChip[arrRelationChips[nLoop][0]].updateStallChip();
            }
        }
    }

    // 選択したチップを解放する
    SetTableUnSelectedStatus();
    // チップ選択状態を解除する
    SetChipUnSelectedStatus();
    // dbを更新する
    // 渡す引数
    var jsonData = {
        Method: "ClickRezConFirmed",
        SvcInId: gArrObjChip[strChipId].svcInId,
        StallUseId: gArrObjChip[strChipId].stallUseId,
        RowLockVersion: gArrObjChip[strChipId].rowLockVersion
    };
    //rowlockversion+1
    AddRowLockVersionOne(strChipId);
    //コールバック開始
    DoCallBack(C_CALLBACK_WND101, jsonData, SC3240101AfterCallBack, jsonData.Method);
}

/**
* 予約確定取り消しを押す
* @return {-} 無し
*/
function ClickCancelRezConFirmed() {
    var strChipId = gSelectedChipId;
    // 変更する前のデータをバックアップする
    BackupRelationChips(strChipId);

    // 当画面にリレーションチップを取得する
    var arrRelationChips = FindRelationChips(strChipId, "");

    if (arrRelationChips.length == 0) {
        // 仮予約→本予約
        gArrObjChip[strChipId].setResvStatus(C_RTYPE_TEMP);
        // チップを更新する
        gArrObjChip[strChipId].updateStallChip();
    } else {
        // リレーションチップ全体本予約→仮予約に変更する
        for (var nLoop = 0; nLoop < arrRelationChips.length; nLoop++) {
            // 画面に表示される時
            if (gArrObjChip[arrRelationChips[nLoop][0]]) {
                // 仮予約→本予約
                gArrObjChip[arrRelationChips[nLoop][0]].setResvStatus(C_RTYPE_TEMP);
                // チップを更新する
                gArrObjChip[arrRelationChips[nLoop][0]].updateStallChip();
            }
        }
    }

    // 選択したチップを解放する
    SetTableUnSelectedStatus();
    // チップ選択状態を解除する
    SetChipUnSelectedStatus();
    // dbを更新する
    // 渡す引数
    var jsonData = {
        Method: "ClickCancelRezConFirmed",
        SvcInId: gArrObjChip[strChipId].svcInId,
        StallUseId: gArrObjChip[strChipId].stallUseId,
        RowLockVersion: gArrObjChip[strChipId].rowLockVersion
    };
    //rowlockversion+1
    AddRowLockVersionOne(strChipId);
    //コールバック開始
    DoCallBack(C_CALLBACK_WND101, jsonData, SC3240101AfterCallBack, jsonData.Method);
}

/**
* NOSHOWボタンを押す
* @return {-} 無し
*/
function ClickNoShow() {
    // 「選択したチップをNo Showエリアに移動しますか？」確認メッセージが表示される
    var rtValue = ConfirmSC3240101Msg(916);

    // Noshowの場合
    if (rtValue) {
        var strChipId = gSelectedChipId;
        // 変更する前のデータをバックアップする
        BackupRelationChips(strChipId);
        // 来店時間が今の時間を設定する
        var dtNow = GetServerTimeNow();

        // 重複チップを取得する
        var arrDuplChipIds = GetDuplicateChips(strChipId);
        // 当画面にリレーションチップを取得する
        var arrRelationChips = FindRelationChips(strChipId, "");

        // 2013/11/29 TMEJ 張 タブレット版SMB チーフテクニシャン機能開発 START
//        // 関連チップ全部削除
//        if (arrRelationChips.length == 0) {
//            $("#" + strChipId).remove();
//        } else {
//            for (var nLoop = 0; nLoop < arrRelationChips.length; nLoop++) {
//                // 画面に表示される時
//                if (gArrObjChip[arrRelationChips[nLoop][0]]) {
//                    $("#" + arrRelationChips[nLoop][0]).remove();
//                }
//            }
//        }
        // 2013/11/29 TMEJ 張 タブレット版SMB チーフテクニシャン機能開発 END

        // 選択したチップを解放する
        SetTableUnSelectedStatus(arrRelationChips);
        // チップ選択状態を解除する
        SetChipUnSelectedStatus();

        // 2013/11/29 TMEJ 張 タブレット版SMB チーフテクニシャン機能開発 START
//        // 白い枠を削除
//        DeleteWhiteBoarder(arrDuplChipIds, strChipId);
        
//        // 遅刻ストールを赤に設定する
//        SetLaterStallColorRed();
        // 2013/11/29 TMEJ 張 タブレット版SMB チーフテクニシャン機能開発 END

        // dbを更新する
        // 渡す引数
        var jsonData = {
            Method: "ClickBtnNoshow",
            SvcInId: gArrObjChip[strChipId].svcInId,
            StallUseId: gArrObjChip[strChipId].stallUseId,
            RowLockVersion: gArrObjChip[strChipId].rowLockVersion
        };

        //rowlockversion+1
        AddRowLockVersionOne(strChipId);

        // 2013/11/29 TMEJ 張 タブレット版SMB チーフテクニシャン機能開発 START
//        // 構造体に値をnullにする
//        for (var nLoop = 0; nLoop < arrRelationChips.length; nLoop++) {
//            // 画面に表示される時
//            if (gArrObjChip[arrRelationChips[nLoop][0]]) {
//                gArrObjChip[arrRelationChips[nLoop][0]] = null;
//            }
//        }

        // 関連チップ全部削除
        if (arrRelationChips.length == 0) {

            RemoveChipFromStall(strChipId);

        } else {

            for (var nLoop = 0; nLoop < arrRelationChips.length; nLoop++) {

                // 画面に表示される時
                if (gArrObjChip[arrRelationChips[nLoop][0]]) {

                    RemoveChipFromStall(arrRelationChips[nLoop][0]);
                }

            }

        }

        // 遅刻ストールを赤に設定する
        SetLaterStallColorRed();
        // 2013/11/29 TMEJ 張 タブレット版SMB チーフテクニシャン機能開発 END

        //コールバック開始
        DoCallBack(C_CALLBACK_WND101, jsonData, SC3240101AfterCallBack, jsonData.Method);
    }

}
/**
* 中断ボタンを押す
* @return {-} 無し
*/
function ClickBtnStopJob() {

    var strChipId = gSelectedChipId;
    var dtEndNow;
    // 今の日付は画面の日付と違う場合、
    if (IsTodayPage() == false) {
        // チップの終了時間はストールの最後営業時間にする
        dtEndNow = new Date($("#hidShowDate").val() + " " + $("#hidStallEndTime").val() + ":00");
    } else {
        dtEndNow = GetServerTimeNow();
        dtEndNow.setSeconds(0);
        dtEndNow.setMilliseconds(0);
    }

    var dtStart = gArrObjChip[strChipId].rsltStartDateTime;

    // 2019/12/06 NSK 皆川 (トライ店システム評価)整備作業における休憩時間取得判定向上検証 START
    // 休憩を自動判定しない場合
    if ($("#hidRestAutoJudgeFlg").val() != "1") {
        // 2019/12/06 NSK 皆川 (トライ店システム評価)整備作業における休憩時間取得判定向上検証 END

        // 開始先が休憩と重複する場合、休憩ウィンドウを表示する
    var nRowNo = GetRowNoByChipId(strChipId);
    var nLeft = GetXPosByTime(dtStart) + 1;
    var nRight = GetXPosByTime(dtEndNow);

    // 2014/07/15 TMEJ 張 タブレットSMB Job Dispatch機能開発 START
    // 休憩フラグを初期化
    $(".popStopWindowBase").data("RESTFLG", C_RESTTIMEGETFLG_NOSET);
    // 2014/07/15 TMEJ 張 タブレットSMB Job Dispatch機能開発 END


    var arrRestTimeId = GetRestTimeInRange(nRowNo, nLeft, nRight);
    if (arrRestTimeId.length > 0) {
        // 休憩エリアを表示する
        ShowRestTimeDialog(arrRestTimeId[0][0], C_ACTION_STOP);
        return;
    }

        // 2019/12/06 NSK 皆川 (トライ店システム評価)整備作業における休憩時間取得判定向上検証 START
    }
    // 2019/12/06 NSK 皆川 (トライ店システム評価)整備作業における休憩時間取得判定向上検証 END

    // 2014/07/15 TMEJ 張 タブレットSMB Job Dispatch機能開発 START
    //    //中断ウィンドウを表示する
    //    ShowStopDialog();

    // dbを更新する
    // 渡す引数
    var jsonData = {
            Method: "HasBeforeStartJob",
            JobDtlId: gArrObjChip[strChipId].jobDtlId
        };

    // グルグルを表示する
    gMainAreaActiveIndicator.show();

    //コールバック開始
    DoCallBack(C_CALLBACK_WND101, jsonData, SC3240101AfterCallBack, jsonData.Method);
    // 2014/07/15 TMEJ 張 タブレットSMB Job Dispatch機能開発 END
}

// 2014/07/15 TMEJ 張 タブレットSMB Job Dispatch機能開発 START
/**
* 未開始のJobがあるので、中断後、赤作業中チップになる
* @return {-} 無し
*/
function StopJobExceptBeforeJob(strStopReasonType, strStopMemo, nRestFlg) {

    var strChipId = gSelectedChipId;
    // 変更する前のデータをバックアップする
    BackupRelationChips(strChipId);

    // 今の日時取得
    var dtEndNow = GetServerTimeNow();
    dtEndNow.setSeconds(0);
    dtEndNow.setMilliseconds(0);

    // ストール利用ステータスに作業計画の一部の作業が中断を設定する
    gArrObjChip[strChipId].setStallUseStatus(C_STALLUSE_STATUS_STARTINCLUDESTOPJOB);

    // 作業計画の一部の作業が中断の赤色を追加する
    $("#" + strChipId).addClass("StoppingJobColor");

    // 重複チップを表示する
    ShowDuplicateChips(strChipId);

    // 選択したチップを解放する
    SetTableUnSelectedStatus();

    // チップ選択状態を解除する
    SetChipUnSelectedStatus();

    // 見込み遅刻時刻により、チップの色を更新する
    UpdateChipColorByDelayDate(strChipId);

    // dbを更新する
    // 渡す引数
    var jsonData;
    if (nRestFlg == null) {
        jsonData = {
            Method: "ClickBtnStopJob",
            ShowDate: $("#hidShowDate").val(),
            SvcInId: gArrObjChip[strChipId].svcInId,
            StallId: gArrObjChip[strChipId].stallId,
            StallUseId: gArrObjChip[strChipId].stallUseId,
            RsltStartDateTime: gArrObjChip[strChipId].rsltStartDateTime,
            RsltEndDateTime: dtEndNow,
            RowLockVersion: gArrObjChip[strChipId].rowLockVersion,
            StopTime: 0,
            StopMemo: strStopMemo,
            StopReasonType: strStopReasonType
        };
    } else {
        jsonData = {
            Method: "ClickBtnStopJob",
            ShowDate: $("#hidShowDate").val(),
            SvcInId: gArrObjChip[strChipId].svcInId,
            StallId: gArrObjChip[strChipId].stallId,
            StallUseId: gArrObjChip[strChipId].stallUseId,
            RsltStartDateTime: gArrObjChip[strChipId].rsltStartDateTime,
            RsltEndDateTime: dtEndNow,
            RowLockVersion: gArrObjChip[strChipId].rowLockVersion,
            StopTime: 0,
            StopMemo: strStopMemo,
            RestFlg: nRestFlg,
            StopReasonType: strStopReasonType
        };
    }

    //rowlockversion+1
    AddRowLockVersionOne(strChipId);
    //コールバック開始
    DoCallBack(C_CALLBACK_WND101, jsonData, SC3240101AfterCallBack, jsonData.Method);

}
// 2014/07/15 TMEJ 張 タブレットSMB Job Dispatch機能開発 END

/**
* 中断ボタンを押す
* @return {-} 無し
*/
function StopJob(strStopReasonType, nStopTime, strStopMemo, nRestFlg) {

    var strChipId = gSelectedChipId;
    // 変更する前のデータをバックアップする
    BackupRelationChips(strChipId);

    var dtEndNow;
    // 今の日付は画面の日付と違う場合、
    if (IsTodayPage() == false) {
        // チップの終了時間はストールの最後営業時間にする
        dtEndNow = new Date($("#hidShowDate").val() + " " + $("#hidStallEndTime").val() + ":00");
    } else {
        dtEndNow = GetServerTimeNow();
        dtEndNow.setSeconds(0);
        dtEndNow.setMilliseconds(0);
    }

    // 中断時間があれば、使用不可エリアの開始、終了時間を取得
    var dtUnavailableStartTime, dtUnavailableEndTime;
    if (nStopTime > 0) {
        dtUnavailableStartTime = GetServerTimeNow();
        dtUnavailableStartTime.setMilliseconds(0);
        dtUnavailableStartTime.setSeconds(0);

        // 5分単位で切り上げる
        var nMinutes = smbScript.RoundUpToNumUnits(dtUnavailableStartTime.getMinutes().toString(), gResizeInterval, 0, 60);
        if (nMinutes < 60) {
            dtUnavailableStartTime.setMinutes(nMinutes);
        } else {
            // Minutesをクリアした後で、１時間をプラスする
            dtUnavailableStartTime.setMinutes(0);
            dtUnavailableStartTime.setTime(dtUnavailableStartTime.getTime() + 60 * 60 * 1000);
        }

        // 終了時間
        dtUnavailableEndTime = new Date();
        dtUnavailableEndTime.setTime(dtUnavailableStartTime.getTime() + nStopTime * 60 * 1000);

        var nRowNo = GetRowNoByChipId(strChipId);
        var nLeft = GetXPosByTime(dtUnavailableStartTime) + 1;
        var nRight = GetXPosByTime(dtUnavailableEndTime);
        // 重複チェック
        //2017/09/13 NSK 竹中(悠) REQ-SVT-TMT-20161109-001 SMB iPadにストールクローズ機能の追加 
        var arrRestTimeId = GetRestTimeInRange(nRowNo, nLeft, nRight);
        // 生成する中断エリアが既存の休憩エリアと重複チェック
//        if (arrRestTimeId.length > 0) {
//            ShowSC3240101Msg(914);
//            return;
//        }
        //2017/09/13 NSK 竹中(悠) REQ-SVT-TMT-20161109-001 SMB iPadにストールクローズ機能の追加 
        // 普通のチップと重複チェック
        var arrDuplChipId = GetDuplChipIdInRange(nRowNo, nLeft, nRight);
        if (arrDuplChipId.length > 0) {
            // 重複のは自分ではない
            if ((arrDuplChipId.length != 1) || (arrDuplChipId[0][0] != gSelectedChipId)) {
                ShowSC3240101Msg(914);
                return;
            }
        }
        // 使用不可チップを生成する
        //2017/08/30 NSK 竹中(悠) REQ-SVT-TMT-20161109-001 SMB iPadにストールクローズ機能の追加 START
        //var objUnvaliable = CreateUnavailableChip(C_UNAVALIABLENEWCHIPID);
        var objUnvaliable = CreateUnavailableChip(C_UNAVALIABLENEWCHIPID, "");
        //2017/08/30 NSK 竹中(悠) REQ-SVT-TMT-20161109-001 SMB iPadにストールクローズ機能の追加 END
        objUnvaliable.css({ "left": nLeft, "width": nRight - nLeft });
        $(".Row" + nRowNo).append(objUnvaliable);
    }

    // チップの終了時間をバックアップする
    var dtBackDispEndDate = gArrObjChip[strChipId].DisplayEndDate;
    // 実績終了時間の設定
    gArrObjChip[strChipId].setRsltEndDateTime(dtEndNow.toString());
    // 表示終了時間の設定
    gArrObjChip[strChipId].setDisplayDate();
    // ストール利用ステータスを中断に設定すう
    gArrObjChip[strChipId].setStallUseStatus(C_STALLUSE_STATUS_STOP);
    // チップを更新する
    gArrObjChip[strChipId].updateStallChip();
    // 位置をリセットする
    SetChipPosition(strChipId, "", "", "");
    // 作業中のz-indexを削除する
    $("#" + strChipId).removeClass("WorkingChipZIndex");
    // 重複チップを表示する
    ShowDuplicateChips(strChipId);
    // 選択したチップを解放する
    SetTableUnSelectedStatus();
    // チップ選択状態を解除する
    SetChipUnSelectedStatus();
    // 見込み遅刻時刻により、チップの色を更新する
    UpdateChipColorByDelayDate(strChipId);

    // dbを更新する
    // 渡す引数
    var jsonData;
    if (nRestFlg == null) {
        jsonData = {
            Method: "ClickBtnStopJob",
            ShowDate: $("#hidShowDate").val(),
            SvcInId: gArrObjChip[strChipId].svcInId,
            StallId: gArrObjChip[strChipId].stallId,
            StallUseId: gArrObjChip[strChipId].stallUseId,
            RsltStartDateTime: gArrObjChip[strChipId].rsltStartDateTime,
            RsltEndDateTime: gArrObjChip[strChipId].rsltEndDateTime,
            RowLockVersion: gArrObjChip[strChipId].rowLockVersion,
            StopTime: nStopTime,
            StopMemo: strStopMemo,
            StopReasonType: strStopReasonType
        };
    } else {
        jsonData = {
            Method: "ClickBtnStopJob",
            ShowDate: $("#hidShowDate").val(),
            SvcInId: gArrObjChip[strChipId].svcInId,
            StallId: gArrObjChip[strChipId].stallId,
            StallUseId: gArrObjChip[strChipId].stallUseId,
            RsltStartDateTime: gArrObjChip[strChipId].rsltStartDateTime,
            RsltEndDateTime: gArrObjChip[strChipId].rsltEndDateTime,
            RowLockVersion: gArrObjChip[strChipId].rowLockVersion,
            StopTime: nStopTime,
            StopMemo: strStopMemo,
            RestFlg: nRestFlg,
            StopReasonType: strStopReasonType
        };
    }

    //rowlockversion+1
    AddRowLockVersionOne(strChipId);
    //コールバック開始
    DoCallBack(C_CALLBACK_WND101, jsonData, SC3240101AfterCallBack, jsonData.Method);

}
/**
* 入庫ボタンを押す
* @return {-} 無し
*/
function ClickBtnCarIn() {
    var strChipId = gSelectedChipId;
    // 変更する前のデータをバックアップする
    BackupRelationChips(strChipId);
    // 来店時間が今の時間を設定する
    var dtNow = GetServerTimeNow();
    // 当画面にリレーションチップを取得する
    var arrRelationChips = FindRelationChips(strChipId, "");
    if (arrRelationChips.length == 0) {
        gArrObjChip[strChipId].setRsltSvcInDateTime(dtNow);
        // チップを更新する
        gArrObjChip[strChipId].updateStallChip();
    } else {
        // リレーションの点と線の表示
        for (var nLoop = 0; nLoop < arrRelationChips.length; nLoop++) {
            // 画面に表示される時
            if (gArrObjChip[arrRelationChips[nLoop][0]]) {
                gArrObjChip[arrRelationChips[nLoop][0]].setRsltSvcInDateTime(dtNow);
                // チップを更新する
                gArrObjChip[arrRelationChips[nLoop][0]].updateStallChip();
            }
        }
    }

    // 選択したチップを解放する
    SetTableUnSelectedStatus();
    // チップ選択状態を解除する
    SetChipUnSelectedStatus();
    // dbを更新する
    // 渡す引数
    var jsonData = {
        Method: "ClickBtnCarIn",
        SvcInId: gArrObjChip[strChipId].svcInId,
        StallUseId: gArrObjChip[strChipId].stallUseId,
        RsltServiceInDate: gArrObjChip[strChipId].rsltSvcInDateTime,
        RowLockVersion: gArrObjChip[strChipId].rowLockVersion
    };
    //rowlockversion+1
    AddRowLockVersionOne(strChipId);
    //コールバック開始
    DoCallBack(C_CALLBACK_WND101, jsonData, SC3240101AfterCallBack, jsonData.Method);
}
/**
* 入庫取消ボタンを押す
* @return {-} 無し
*/
function ClickBtnCancelCarIn() {
    var strChipId = gSelectedChipId;
    // 変更する前のデータをバックアップする
    BackupRelationChips(strChipId);
    // 当画面にリレーションチップを取得する
    var arrRelationChips = FindRelationChips(strChipId, "");
    if (arrRelationChips.length == 0) {
        gArrObjChip[strChipId].setRsltSvcInDateTime(new Date(C_DATE_DEFAULT_VALUE));
        // チップを更新する
        gArrObjChip[strChipId].updateStallChip();
    } else {
        // リレーションの点と線の表示
        for (var nLoop = 0; nLoop < arrRelationChips.length; nLoop++) {
            // 画面に表示される時
            if (gArrObjChip[arrRelationChips[nLoop][0]]) {
                gArrObjChip[arrRelationChips[nLoop][0]].setRsltSvcInDateTime(new Date(C_DATE_DEFAULT_VALUE));
                // チップを更新する
                gArrObjChip[arrRelationChips[nLoop][0]].updateStallChip();
            }
        }
    }

    // 選択したチップを解放する
    SetTableUnSelectedStatus();
    // チップ選択状態を解除する
    SetChipUnSelectedStatus();

    // dbを更新する
    // 渡す引数
    var jsonData = {
        Method: "ClickBtnCancelCarIn",
        SvcInId: gArrObjChip[strChipId].svcInId,
        StallUseId: gArrObjChip[strChipId].stallUseId,
        RowLockVersion: gArrObjChip[strChipId].rowLockVersion
    };
    //rowlockversion+1
    AddRowLockVersionOne(strChipId);
    //コールバック開始
    DoCallBack(C_CALLBACK_WND101, jsonData, SC3240101AfterCallBack, jsonData.Method);
}
/**
* 開始ボタンを押す
* @return {-} 無し
*/
function ClickBtnStart() {
    var strChipId = gSelectedChipId;

    var bStartChipFlg = false;
    // 当行に開始チップがあるチック
    $("#" + strChipId).offsetParent().children("div").each(function (index, e) {
        if (gArrObjChip[e.id]) {
            // 作業中
            if ((IsDefaultDate(gArrObjChip[e.id].rsltStartDateTime) == false) && (IsDefaultDate(gArrObjChip[e.id].rsltEndDateTime) == true)) {
                bStartChipFlg = true;
            }
        }
    });
    // あるチップが作業中、開始できない
    if (bStartChipFlg) {
        ShowSC3240101Msg(905);
        return;
    }

    // 今の時間を取得して、プロトタイプに設定する
    var dtStartNow = GetServerTimeNow();
    dtStartNow.setSeconds(0);
    dtStartNow.setMilliseconds(0);
    // リレーションチップの場合、
    if (IsRelationChip(strChipId) == true) {
        // リレーションチップに作業中チップがある場合、開始できない
        if (HasWorkingChipInRelation(strChipId)) {
            //2017/01/17 NSK 竹中 TR-SVT-TMT-20151019-002 TCメイン画面のエラーメッセージが理解不可能 START
            var stallId = GetStallIdOfRelationChip(strChipId);
            if (stallId != undefined) {
                ShowSC3240101Msg(937, stallId);
                return;
            }
            //2017/01/17 NSK 竹中 TR-SVT-TMT-20151019-002 TCメイン画面のエラーメッセージが理解不可能 END
            ShowSC3240101Msg(910);
            return;
        }
    }

    // 開始先が使用不可チップと重複する場合、休憩ウィンドウを表示する
    var nRowNo = GetRowNoByChipId(strChipId);
    // 2013/11/29 TMEJ 張 タブレット版SMB チーフテクニシャン機能開発 START
//    // 該ストールにテクニシャンがいるかを判断する
//    if ($("#spanTechnician" + nRowNo + "_1").text() == "") {
//        //「ストールに作業者が設定されていません。作業者を設定し再度処理を行ってください。」メッセージが表示される
//        ShowSC3240101Msg(907);
//        return;
//    }
    // 2013/11/29 TMEJ 張 タブレット版SMB チーフテクニシャン機能開発 END

    // 整備種類が選択されていないため、作業開始できない。
    if ($("#" + strChipId + " .infoBox")[0].innerHTML.toString().Trim() == "") {
        //「ストールに作業者が設定されていません。作業者を設定し再度処理を行ってください。」メッセージが表示される
        ShowSC3240101Msg(904);
        return;
    }

    // 2019/12/06 NSK 皆川 (トライ店システム評価)整備作業における休憩時間取得判定向上検証 START
    // 休憩を自動判定しない場合
    if ($("#hidRestAutoJudgeFlg").val() != "1") {
        // 2019/12/06 NSK 皆川 (トライ店システム評価)整備作業における休憩時間取得判定向上検証 END

    var nLeft = GetXPosByTime(dtStartNow) + 1;
    var nRight = nLeft + $("#" + strChipId).width();
    var arrRestTimeId = GetRestTimeInRange(nRowNo, nLeft, nRight);
    if (arrRestTimeId.length > 0) {
        // 休憩エリアを表示する
        ShowRestTimeDialog(arrRestTimeId[0][0], C_ACTION_START);
        return;
    }

        // 2019/12/06 NSK 皆川 (トライ店システム評価)整備作業における休憩時間取得判定向上検証 START
    }
    // 2019/12/06 NSK 皆川 (トライ店システム評価)整備作業における休憩時間取得判定向上検証 END

    StartChip(null);
}

/**
* チップ開始
* @return {-} 無し
*/
function StartChip(nRestFlg) {

    var strChipId = gSelectedChipId;
    // 開始時刻を取得する
    var dtStartNow = GetServerTimeNow();
    dtStartNow.setSeconds(0);
    dtStartNow.setMilliseconds(0);

    var dtTapDateTime = new Date();
    dtTapDateTime.setTime(dtStartNow.getTime());

    var nLeft = GetXPosByTime(dtStartNow) + 1;
    // 2019/07/22 NSK 近藤 (トライ店システム評価)次世代サービスオペレーション効率化に向けた、業務適合性検証 START
    // var nWidth = $("#" + strChipId).width();
    var nWidth = $("#" + C_MOVINGCHIPID).width();
    // 2019/07/22 NSK 近藤 (トライ店システム評価)次世代サービスオペレーション効率化に向けた、業務適合性検証 END
    var nRestTimeWidth = 0;

    // 2019/12/06 NSK 皆川 (トライ店システム評価)整備作業における休憩時間取得判定向上検証 START
    // 休憩を自動判定しない場合
    if ($("#hidRestAutoJudgeFlg").val() != "1") {
        // 2019/12/06 NSK 皆川 (トライ店システム評価)整備作業における休憩時間取得判定向上検証 END

    // 休憩フラグを設定した場合
    if (nRestFlg != null) {
        // 休憩を取得する場合、移動チップが今の時刻線から表示ではない可能性がある
        if (nRestFlg == 1) {
            // Movingチップがまず移動先に移動する
            $("#" + C_MOVINGCHIPID).css({ "left": nLeft, "width": nWidth });

            // チップの幅が休憩エリアの幅をプラスする
            var arrRestTime = GetRestTimeInServiceTime(C_MOVINGCHIPID);
            // 休憩エリアがチップの左にある且つ重複時、チップが休憩エリアの右に移動する
            var nChipLeft = GetLeftByRestTime(C_MOVINGCHIPID, arrRestTime)

            if (nChipLeft != nLeft) {
                nLeft = nChipLeft;
                $("#" + C_MOVINGCHIPID).css("left", nLeft);
            }
            nRestTimeWidth = GetWidthByRestTime(C_MOVINGCHIPID);
        }
        }

        // 2019/12/06 NSK 皆川 (トライ店システム評価)整備作業における休憩時間取得判定向上検証 START
    } else {
        // Movingチップがまず移動先に移動する
        $("#" + C_MOVINGCHIPID).css({ "left": nLeft, "width": nWidth });

        // チップの幅が休憩エリアの幅をプラスする
        // ※チップの開始日時は当日となるため、常に休憩時間を作業時間に追加する（MoveChipのような日跨ぎの翌日チップの考慮が不要）
        nRestTimeWidth = GetWidthByRestTime(C_MOVINGCHIPID);
    }
    // 2019/12/06 NSK 皆川 (トライ店システム評価)整備作業における休憩時間取得判定向上検証 END

    // 終了時間設定
    dtEndTime = GetTimeByXPos(nLeft + nWidth + nRestTimeWidth);

    // 変更する前のデータをバックアップする
    BackupRelationChips(strChipId);
    // 実績開始時間の設定
    gArrObjChip[strChipId].setRsltStartDateTime(dtTapDateTime.toString());
    // リレーションチップの構造体にも更新する
    if (IsRelationChip(strChipId) == true) {
        gArrObjRelationChip[strChipId].setStartDateTime(dtTapDateTime);
    }

    // 表示開始時間の設定
    gArrObjChip[strChipId].setDisplayStartDate(dtTapDateTime.toString());
    gArrObjChip[strChipId].setDisplayEndDate(dtEndTime.toString());

    // チップを更新する
    gArrObjChip[strChipId].updateStallChip();
    // 位置をリセットする
    SetChipPosition(strChipId, "", "", "");

    // 重複チップを表示する
    ShowDuplicateChips(strChipId);

    // 選択したチップを解放する
    SetTableUnSelectedStatus();
    // チップ選択状態を解除する
    SetChipUnSelectedStatus();
    var nMinutes = Math.ceil((gArrObjChip[strChipId].displayEndDate - gArrObjChip[strChipId].displayStartDate) / 1000 / 60);

    // 見込み遅刻時刻により、チップの色を更新する
    UpdateChipColorByDelayDate(strChipId);

    // dbを更新する
    // 渡す引数
    var jsonData;
    if (nRestFlg == null) {
        jsonData = {
            Method: "ClickBtnStart",
            ShowDate: $("#hidShowDate").val(),
            SvcInId: gArrObjChip[strChipId].svcInId,
            // 2014/07/15 TMEJ 張 タブレットSMB Job Dispatch機能開発 START
            JobDtlId: gArrObjChip[strChipId].jobDtlId,
            ReStartJobFlg: C_RESTARTJOB_NOTSET,
            // 2014/07/15 TMEJ 張 タブレットSMB Job Dispatch機能開発 END
            StallUseId: gArrObjChip[strChipId].stallUseId,
            StallId: gArrObjChip[strChipId].stallId,
            RsltStartDateTime: dtTapDateTime,
            ScheWorkTime: gArrObjChip[strChipId].scheWorkTime,
            RowLockVersion: gArrObjChip[strChipId].rowLockVersion
        };
    } else {
        jsonData = {
            Method: "ClickBtnStart",
            ShowDate: $("#hidShowDate").val(),
            SvcInId: gArrObjChip[strChipId].svcInId,
            // 2014/07/15 TMEJ 張 タブレットSMB Job Dispatch機能開発 START
            JobDtlId: gArrObjChip[strChipId].jobDtlId,
            ReStartJobFlg: C_RESTARTJOB_NOTSET,
            // 2014/07/15 TMEJ 張 タブレットSMB Job Dispatch機能開発 END
            StallUseId: gArrObjChip[strChipId].stallUseId,
            StallId: gArrObjChip[strChipId].stallId,
            RsltStartDateTime: dtTapDateTime,
            ScheWorkTime: gArrObjChip[strChipId].scheWorkTime,
            RestFlg: nRestFlg,
            RowLockVersion: gArrObjChip[strChipId].rowLockVersion
        };
    }

    //2014/07/15 TMEJ 張 タブレットSMB Job Dispatch機能開発 START
    // 開始のJsonデータをバックアップする
    // 中断Jobがある場合、ポップアップ出て、任意ボタンをタップすると、
    // もう一回開始のJsonをサーバに渡す
    gBackupStartJson[gArrObjChip[strChipId].stallUseId] = jsonData;
    //2014/07/15 TMEJ 張 タブレットSMB Job Dispatch機能開発 END

    //2020/01/07 NSK 坂本 TR-SVT-TMT-20190118-001 Stop start tagのXMLログが過去日19000101を表示している START
    //最新の情報取得までタップ不可にする
    $("#" + strChipId).data(C_DATA_CHIPTAP_FLG, false);
    //2020/01/07 NSK 坂本 TR-SVT-TMT-20190118-001 Stop start tagのXMLログが過去日19000101を表示している END

    //rowlockversion+1
    AddRowLockVersionOne(strChipId);
    //コールバック開始
    DoCallBack(C_CALLBACK_WND101, jsonData, SC3240101AfterCallBack, jsonData.Method);
}
/**
* 終了ボタンを押す
* @return {-} 無し
*/
function ClickBtnFinish() {
    var strChipId = gSelectedChipId;
    var dtEndNow;
    // 今の日付は画面の日付と違う場合、
    if (IsTodayPage() == false) {
        // チップの終了時間はストールの最後営業時間にする
        dtEndNow = gEndWorkTime;
    } else {
        dtEndNow = GetServerTimeNow();
    }
    dtEndNow.setSeconds(0);
    dtEndNow.setMilliseconds(0);

    // 2019/12/06 NSK 皆川 (トライ店システム評価)整備作業における休憩時間取得判定向上検証 START
    // 休憩を自動判定しない場合
    if ($("#hidRestAutoJudgeFlg").val() != "1") {
        // 2019/12/06 NSK 皆川 (トライ店システム評価)整備作業における休憩時間取得判定向上検証 END

    // 開始先が使用不可チップと重複する場合、休憩ウィンドウを表示する
    var nRowNo = GetRowNoByChipId(strChipId);
    var nLeft = $("#" + strChipId).position().left;
    var nRight = GetXPosByTime(dtEndNow);
    var arrRestTimeId = GetRestTimeInRange(nRowNo, nLeft, nRight);
    if (arrRestTimeId.length > 0) {
        // 休憩確認ウィンドウをポップアップする
        ShowRestTimeDialog(arrRestTimeId[0][0], C_ACTION_FINISH);
        return;
    }

        // 2019/12/06 NSK 皆川 (トライ店システム評価)整備作業における休憩時間取得判定向上検証 START
    }
    // 2019/12/06 NSK 皆川 (トライ店システム評価)整備作業における休憩時間取得判定向上検証 END

    // 終了
    FinishChip(null);
}

/**
* チップ終了
* @param {Integer} 休憩フラグ
* @return {-} 無し
*/
function FinishChip(nRestFlg) {

    var strChipId = gSelectedChipId;

    //2014/07/15 TMEJ 張 タブレットSMB Job Dispatch機能開発 START

    // 中断Jobがある場合(ストール利用ステータスが04作業計画の一部の作業が中断)
    if (C_STALLUSE_STATUS_STARTINCLUDESTOPJOB == gArrObjChip[strChipId].stallUseStatus) {

        // 中断Job以外のJobを終了するかといい確認メッセージを出す
        var retConfirm = ConfirmSC3240101Msg(930);

        // キャンセルボタンを押すと、戻る
        if (!retConfirm) {

            return;
        }

    }

    //2014/07/15 TMEJ 張 タブレットSMB Job Dispatch機能開発 END		

    // 変更する前のデータをバックアップする
    BackupRelationChips(strChipId);
    var dtEndNow;
    // 今の日付は画面の日付と違う場合、
    if (IsTodayPage() == false) {
        // チップの終了時間はストールの最後営業時間にする
        dtEndNow = gEndWorkTime;
    } else {
        dtEndNow = GetServerTimeNow();
    }
    dtEndNow.setSeconds(0);
    dtEndNow.setMilliseconds(0);

    // 実績終了時間の設定
    gArrObjChip[strChipId].setRsltEndDateTime(dtEndNow.toString());
    // 表示終了時間の設定
    gArrObjChip[strChipId].setDisplayDate();
    // チップを更新する
    gArrObjChip[strChipId].updateStallChip();
    // 位置をリセットする
    SetChipPosition(strChipId, "", "", "");
    // 作業中のz-indexを削除する
    $("#" + strChipId).removeClass("WorkingChipZIndex");

    // 2014/07/15 TMEJ 張 タブレットSMB Job Dispatch機能開発 START
    // 作業一部中断の赤いろを消す
    $("#" + strChipId).removeClass("StoppingJobColor");
    // 2014/07/15 TMEJ 張 タブレットSMB Job Dispatch機能開発 END

    // 重複チップを表示する
    ShowDuplicateChips(strChipId);
    // 選択したチップを解放する
    SetTableUnSelectedStatus();
    // チップ選択状態を解除する
    SetChipUnSelectedStatus();
    // 見込み遅刻時刻により、チップの色を更新する
    UpdateChipColorByDelayDate(strChipId);
    // dbを更新する
    // 渡す引数
    var jsonData;

    if (nRestFlg == null) {
        jsonData = {
            Method: "ClickBtnFinish",
            ShowDate: $("#hidShowDate").val(),
            SvcInId: gArrObjChip[strChipId].svcInId,
            StallUseId: gArrObjChip[strChipId].stallUseId,
            StallId: gArrObjChip[strChipId].stallId,
            RsltStartDateTime: gArrObjChip[strChipId].rsltStartDateTime,
            RsltEndDateTime: gArrObjChip[strChipId].rsltEndDateTime,
            RowLockVersion: gArrObjChip[strChipId].rowLockVersion
        };
    } else {
        jsonData = {
            Method: "ClickBtnFinish",
            ShowDate: $("#hidShowDate").val(),
            SvcInId: gArrObjChip[strChipId].svcInId,
            StallUseId: gArrObjChip[strChipId].stallUseId,
            StallId: gArrObjChip[strChipId].stallId,
            RsltStartDateTime: gArrObjChip[strChipId].rsltStartDateTime,
            RsltEndDateTime: gArrObjChip[strChipId].rsltEndDateTime,
            RestFlg: nRestFlg,
            RowLockVersion: gArrObjChip[strChipId].rowLockVersion
        };
    }
    
    //rowlockversion+1
    AddRowLockVersionOne(strChipId);
    //コールバック開始
    DoCallBack(C_CALLBACK_WND101, jsonData, SC3240101AfterCallBack, jsonData.Method);
}

/**
* 日跨ぎ終了ボタンを押す
* @return {-} 無し
*/
function ClickMidFinish() {
    var strChipId = gSelectedChipId;
    // 変更する前のデータをバックアップする
    BackupRelationChips(strChipId);

    var dtEndNow;
    // 今の日付は画面の日付と違う場合、
    if (IsTodayPage() == false) {
        // チップの終了時間はストールの最後営業時間にする
        dtEndNow = gEndWorkTime;
    } else {
        dtEndNow = GetServerTimeNow();
    }
    dtEndNow.setSeconds(0);
    dtEndNow.setMilliseconds(0);

    // 2019/12/06 NSK 皆川 (トライ店システム評価)整備作業における休憩時間取得判定向上検証 START
    // 休憩を自動判定しない場合
    if ($("#hidRestAutoJudgeFlg").val() != "1") {
        // 2019/12/06 NSK 皆川 (トライ店システム評価)整備作業における休憩時間取得判定向上検証 END

    // 開始先が使用不可チップと重複する場合、休憩ウィンドウを表示する
    var nRowNo = GetRowNoByChipId(strChipId);
    var nLeft = $("#" + strChipId).position().left;
    var nRight = GetXPosByTime(dtEndNow);
    var arrRestTimeId = GetRestTimeInRange(nRowNo, nLeft, nRight);
    if (arrRestTimeId.length > 0) {
        // 休憩確認ウィンドウをポップアップする
        ShowRestTimeDialog(arrRestTimeId[0][0], C_ACTION_MIDFINISH);
        return;
    }

        // 2019/12/06 NSK 皆川 (トライ店システム評価)整備作業における休憩時間取得判定向上検証 START
    }
    // 2019/12/06 NSK 皆川 (トライ店システム評価)整備作業における休憩時間取得判定向上検証 END

    // 終了
    MidfinishChip(null);
}

/**
* チップを日跨ぎ終了
* @param {Integer} 休憩フラグ
* @return {-} 無し
*/
function MidfinishChip(nRestFlg) {

    var strChipId = gSelectedChipId;
    var dtEndNow;
    // 今の日付は画面の日付と違う場合、
    if (IsTodayPage() == false) {
        // チップの終了時間はストールの最後営業時間にする
        dtEndNow = gEndWorkTime;
    } else {
        dtEndNow = GetServerTimeNow();
    }
    dtEndNow.setSeconds(0);
    dtEndNow.setMilliseconds(0);

    // 実績終了時間の設定
    gArrObjChip[strChipId].setRsltEndDateTime(dtEndNow.toString());
    // 表示終了時間の設定
    gArrObjChip[strChipId].setDisplayDate();
    // チップを更新する
    gArrObjChip[strChipId].updateStallChip();
    // 位置をリセットする
    SetChipPosition(strChipId, "", "", "");
    // 作業中のz-indexを削除する
    $("#" + strChipId).removeClass("WorkingChipZIndex");
    // 重複チップを表示する
    ShowDuplicateChips(strChipId);
    // 選択したチップを解放する
    SetTableUnSelectedStatus();
    // チップ選択状態を解除する
    SetChipUnSelectedStatus();
    // 見込み遅刻時刻により、チップの色を更新する
    UpdateChipColorByDelayDate(strChipId);

    // 2014/07/15 TMEJ 張 タブレットSMB Job Dispatch機能開発 START
    // 中断Job含むピンク色を消す
    $("#" + strChipId).removeClass("StoppingJobColor");
    // 2014/07/15 TMEJ 張 タブレットSMB Job Dispatch機能開発 END

    // dbを更新する
    // 渡す引数
    var jsonData;
    if (nRestFlg == null) {
        jsonData = {
            Method: "ClickMidFinish",
            ShowDate: $("#hidShowDate").val(),
            SvcInId: gArrObjChip[strChipId].svcInId,
            StallUseId: gArrObjChip[strChipId].stallUseId,
            StallId: gArrObjChip[strChipId].stallId,
            RsltStartDateTime: gArrObjChip[strChipId].rsltStartDateTime,
            RsltEndDateTime: gArrObjChip[strChipId].rsltEndDateTime,
            RowLockVersion: gArrObjChip[strChipId].rowLockVersion
        };
    } else {
        jsonData = {
            Method: "ClickMidFinish",
            ShowDate: $("#hidShowDate").val(),
            SvcInId: gArrObjChip[strChipId].svcInId,
            StallUseId: gArrObjChip[strChipId].stallUseId,
            StallId: gArrObjChip[strChipId].stallId,
            RsltStartDateTime: gArrObjChip[strChipId].rsltStartDateTime,
            RsltEndDateTime: gArrObjChip[strChipId].rsltEndDateTime,
            RestFlg: nRestFlg,
            RowLockVersion: gArrObjChip[strChipId].rowLockVersion
        };
    }

    //コールバック開始
    DoCallBack(C_CALLBACK_WND101, jsonData, SC3240101AfterCallBack, jsonData.Method);
}
/**
* コピーボタンを押す
* @return {-} 無し
*/
function ClickCopy() {
    var strChipId = gSelectedChipId;

    SetChipUnSelectedStatus();  //チップ選択状態を解除
    SetTableUnSelectedStatus();

    // コピーされたチップの構造体に値の設定
    gCopyChipObj = new ReserveChip(C_COPYCHIPID);
    // ストールのチップの場合
    if (gArrObjChip[strChipId]) {
        gArrObjChip[strChipId].copy(gCopyChipObj);
    } else if (gArrObjSubChip[strChipId]) {
        // サブチップの場合
        gArrObjSubChip[strChipId].copy(gCopyChipObj);
        if (gArrObjSubChip[strChipId].subChipAreaId == C_RECEPTION) {
            //2013/11/29 TMEJ 張 タブレット版SMB チーフテクニシャン機能開発 START
            //gCopyChipObj.setScheDeliDateTime(gArrObjSubChip[strChipId].parentsScheDeliDateTime);
            //2013/11/29 TMEJ 張 タブレット版SMB チーフテクニシャン機能開発 END
            gCopyChipObj.setPartsFlg("0");
        }
    } else {
        return;
    }
    gCopyChipObj.setStallUseId(C_COPYCHIPID);
    // 実績チップの場合、コピーで生成されたチップが実績ではない
    gCopyChipObj.setRsltStartDateTime(C_DATE_DEFAULT_VALUE);
    gCopyChipObj.setPrmsEndDateTime(C_DATE_DEFAULT_VALUE);
    gCopyChipObj.setRsltEndDateTime(C_DATE_DEFAULT_VALUE);
    gCopyChipObj.setCwRsltStartDateTime(C_DATE_DEFAULT_VALUE);
    gCopyChipObj.setCwRsltEndDateTime(C_DATE_DEFAULT_VALUE);
    gCopyChipObj.setScheWorkTime(45);
    // 右下のinfoボックスを空白にする
    gCopyChipObj.setUpperDisp(" ");
    gCopyChipObj.setLowerDisp(" ");
    gCopyChipObj.setSvcClassName(" ");
    gCopyChipObj.setSvcClassNameEng(" ");

    // 左上にチップを生成する
    gCopyChipObj.createChip(C_CHIPTYPE_COPY);

    //チップの位置とサイズをストール名とテクニシャンのエリアにあわせる
    $("#" + C_COPYCHIPID).css("top", $(".stallNo1").position().top + 2 + "px")
                                     .css("left", "7px")
                                     .css("width", "130px")
                                     .css("height", "70px")
                                     .css("opacity", C_OPACITY_TRANSPARENT);

    AdjustChipItemByWidth(C_COPYCHIPID);
    //縦方向で1行目にスクロール
    $(".ChipArea_trimming").SmbFingerScroll({
        action: "move", moveY: $(".scroll-inner").position().top, moveX: 0
    });
    //gSelectedChipIdを選択しているチップのIDに設定
    gSelectedChipId = C_COPYCHIPID;
    //元のchiptapイベントをMovingチップのタップイベントにbind
    $("#" + C_COPYCHIPID).unbind().bind("chipTap", function (e) {
        SetChipUnSelectedStatus();  //チップ選択状態を解除
        SetTableUnSelectedStatus();
    });
    SetTableSelectedStatus();
    //時刻線の表示制御
    setRedTimeLineLeftPos(false);
    // 納車予定日時がある場合、
    if (IsDefaultDate(gCopyChipObj.scheDeliDateTime) == false) {
        // 納車予定日時により、時刻線の位置を取得して、設定する
        // 納車遅れ見込み時間取得
        // 2017/09/25 NSK 竹中(悠) REQ-SVT-TMT-20170227-002 見込納車遅れ日時計算ロジック統一 START
        // var dtScheDeliLater = GetDeliDelayExpectedTimeLine(gCopyChipObj.carWashNeedFlg, gCopyChipObj.cwRsltStartDateTime, gCopyChipObj.scheDeliDateTime, gCopyChipObj.svcStatus);
        var dtScheDeliLater = GetDeliDelayExpectedTimeLine(gCopyChipObj.carWashNeedFlg
                                                         , gCopyChipObj.scheDeliDateTime
                                                         , gCopyChipObj.svcStatus
                                                         , gCopyChipObj.remainingInspectionType);
        // 2017/09/25 NSK 竹中(悠) REQ-SVT-TMT-20170227-002 見込納車遅れ日時計算ロジック統一 END
        //var setPosition = GetTimeLinePosByTime(gCopyChipObj.scheDeliDateTime);
        var setPosition = GetTimeLinePosByTime(dtScheDeliLater);
        // 2014/09/11 TMEJ 張 BTS-389 「SMBで異常ではないはずが、遅れ見込み表示になっている」対応 END
        $(".TimingLineDeli").css("left", setPosition);
        // 表示にする
        $(".TimingLineDeli").css("visibility", "visible");
    }

    // サブチップエリアを閉じる
    SetSubChipBoxClose();
    CreateFooterButton(C_FT_DISPTP_SELECTED, C_FT_BTNTP_COPYCHIP);

}
/**
* コピーチップを削除する
* @return {-} 無し
*/
function DeleteCopyedChip() {
    // 削除確認メッセージを表示する
    var rtValue = ConfirmSC3240101Msg(913);
    // 削除の場合
    if (rtValue) {
        // 選択したチップを解放する
        SetTableUnSelectedStatus();
        // チップ選択状態を解除する
        SetChipUnSelectedStatus();
    }
}

// 2013/11/29 TMEJ 張 タブレット版SMB チーフテクニシャン機能開発 START
/**
* 履歴があるかどうか
* @return {-} True:ある
*/
function HasChiphistory(strChipId) {
    var bReturn = false;
    // 仮仮チップの場合、戻す
    if (IsKariKariChip(strChipId)) {
        return bReturn;
    }

    if (gArrObjChip[strChipId]) {
        if (IsDefaultDate(gArrObjChip[strChipId].chiphisScheStartDateTime) == false
            && IsDefaultDate(gArrObjChip[strChipId].chiphisScheEndDateTime) == false
            && gArrObjChip[strChipId].chiphisStallId != "") {
            bReturn = true;
        }
    }

    return bReturn;
}

/**
* Undoボタンを押す
* @return {-} 無し
*/
function UndoMainStallChip() {
    var strChipId = gSelectedChipId;

    if (!HasChiphistory(strChipId)) {

        // 2014/09/12 TMEJ 張 BTS-390 「1ｽﾄｰﾙで2ﾁｯﾌﾟが作業中（表示のみ）」対応 START
        // 最新の情報取得までタップ不可にする
        $("#" + gSelectedChipId).data(C_DATA_CHIPTAP_FLG, false);
        // 2014/09/12 TMEJ 張 BTS-390 「1ｽﾄｰﾙで2ﾁｯﾌﾟが作業中（表示のみ）」対応 END

        // 履歴がない場合、画面にチップの変化がない
        // 選択したチップを解放する
        SetTableUnSelectedStatus();
        // チップ選択状態を解除する
        SetChipUnSelectedStatus();


        // dbを更新する
        // 渡す引数
        jsonData = {
            Method: "UndoMainStallChip",
            StallUseId: gArrObjChip[strChipId].stallUseId,
            SvcInId: gArrObjChip[strChipId].svcInId,
            StallId: gArrObjChip[strChipId].stallId,
            RowLockVersion: gArrObjChip[strChipId].rowLockVersion
        };

        //rowlockversion+1
        AddRowLockVersionOne(strChipId);

        //コールバック開始
        DoCallBack(C_CALLBACK_WND101, jsonData, SC3240101AfterCallBack, jsonData.Method);
        return;
    }
    // 履歴の情報を戻す
    gArrObjChip[strChipId].setScheStartDateTime(gArrObjChip[strChipId].chiphisScheStartDateTime);
    gArrObjChip[strChipId].setScheEndDateTime(gArrObjChip[strChipId].chiphisScheEndDateTime);
    gArrObjChip[strChipId].setScheWorkTime(gArrObjChip[strChipId].chiphisScheWorktime);
    gArrObjChip[strChipId].setDisplayStartDate(gArrObjChip[strChipId].chiphisScheStartDateTime);
    gArrObjChip[strChipId].setDisplayEndDate(gArrObjChip[strChipId].chiphisScheEndDateTime);
    gArrObjChip[strChipId].setStallId(gArrObjChip[strChipId].chiphisStallId);

    //TMEJ 丁　リレションラインの不具合修正　START
    //リレションオブジェクトを更新
    if (gArrObjRelationChip[strChipId]) {
        gArrObjRelationChip[strChipId].setStartDateTime(gArrObjChip[strChipId].chiphisScheStartDateTime);
    }
    //TMEJ 丁　リレションラインの不具合修正　END

    // サービス全体の情報は全て関連チップを更新する
    var arrRelationChips = FindRelationChips(strChipId, "");
    if (arrRelationChips.length == 0) {
        gArrObjChip[strChipId].setSvcStatus(gArrObjChip[strChipId].chiphisSvcStatus);
        gArrObjChip[strChipId].setResvStatus(gArrObjChip[strChipId].chiphisResvStatus);
    } else {
        for (var nLoop = 0; nLoop < arrRelationChips.length; nLoop++) {
            if (gArrObjChip[arrRelationChips[nLoop][0]]) {
                gArrObjChip[arrRelationChips[nLoop][0]].setSvcStatus(gArrObjChip[strChipId].chiphisSvcStatus);
                gArrObjChip[arrRelationChips[nLoop][0]].setResvStatus(gArrObjChip[strChipId].chiphisResvStatus);
            }
        }
    }
    // ストール利用ステータスの更新
    var bIsWorkOrderWait = (gArrObjChip[strChipId].svcStatus == C_SVCSTATUS_WORKORDERWAIT);
    var bIsStartWait = (gArrObjChip[strChipId].svcStatus == C_SVCSTATUS_STARTWAIT);
    if (bIsWorkOrderWait || bIsStartWait) {
        var strStallUseStatus = bIsWorkOrderWait ? C_STALLUSE_STATUS_WORKORDERWAIT : C_STALLUSE_STATUS_STARTWAIT;
        gArrObjChip[strChipId].setStallUseStatus();
    }

    // 作業中に関して情報をクリアする
    gArrObjChip[strChipId].setRsltStartDateTime(C_DATE_DEFAULT_VALUE);
    gArrObjChip[strChipId].setPrmsEndDateTime(C_DATE_DEFAULT_VALUE);

    // 履歴の情報をクリアする
    gArrObjChip[strChipId].clearWorkingChipHisInfo();

    // 更新する
    if (arrRelationChips.length == 0) {
        gArrObjChip[strChipId].updateStallChip();
    } else {
        for (var nLoop = 0; nLoop < arrRelationChips.length; nLoop++) {
            if (gArrObjChip[arrRelationChips[nLoop][0]]) {
                gArrObjChip[arrRelationChips[nLoop][0]].updateStallChip();
            }
        }
    }

    // 位置をリセットする
    SetChipPosition(strChipId, "", "", "");

    // 2019/12/06 NSK 皆川 (トライ店システム評価)整備作業における休憩時間取得判定向上検証 START
    // 休憩を自動判定する場合
    if ($("#hidRestAutoJudgeFlg").val() == "1") {
        // 2019/12/06 NSK 皆川 (トライ店システム評価)整備作業における休憩時間取得判定向上検証 END

        // 2019/06/18 NSK 皆川 (トライ店システム評価)次世代サービスオペレーション効率化に向けた、業務適合性検証 START
        // 最新の情報取得までタップ不可にする
        $("#" + gSelectedChipId).data(C_DATA_CHIPTAP_FLG, false);
        // 2019/06/18 NSK 皆川 (トライ店システム評価)次世代サービスオペレーション効率化に向けた、業務適合性検証 END

        // 2019/12/06 NSK 皆川 (トライ店システム評価)整備作業における休憩時間取得判定向上検証 START
    }
    // 2019/12/06 NSK 皆川 (トライ店システム評価)整備作業における休憩時間取得判定向上検証 END

    // 選択したチップを解放する
    SetTableUnSelectedStatus();
    // チップ選択状態を解除する
    SetChipUnSelectedStatus();

    // dbを更新する
    // 渡す引数
    jsonData = {
        Method: "UndoMainStallChip",
        StallUseId: gArrObjChip[strChipId].stallUseId,
        SvcInId: gArrObjChip[strChipId].svcInId,
        StallId: gArrObjChip[strChipId].stallId,
        RowLockVersion: gArrObjChip[strChipId].rowLockVersion
    };

    //rowlockversion+1
    AddRowLockVersionOne(strChipId);

    //コールバック開始
    DoCallBack(C_CALLBACK_WND101, jsonData, SC3240101AfterCallBack, jsonData.Method);
}

// 2013/11/29 TMEJ 張 タブレット版SMB チーフテクニシャン機能開発 END

// 2017/09/05 NSK 小川 REQ-SVT-TMT-20160906-003 計画済みチップをストールから簡単に外せるようにする START
/**
* 計画取り消しボタンを押す
*@return {-}無し
*/
function ClickToReception() {

    // 計画取消メッセージを表示する
    var rtValue = ConfirmSC3240101Msg(938);

    // 計画取り消しを実行する場合
    if (rtValue) {
        var jsonData;
        var strChipId = gSelectedChipId;

        // 変更する前のデータをバックアップする
        BackupRelationChips(strChipId);

        // 行ロックバージョン+1
        AddRowLockVersionOne(strChipId);

        // 選択したチップを解放する
        SetTableUnSelectedStatus();

        // チップ選択状態を解除する
        SetChipUnSelectedStatus();

        // チップのプロトタイプに残作業時間、作業終了予定日時を設定する
        var nBackMinutes = Math.ceil((gArrObjChip[strChipId].displayEndDate - gArrObjChip[strChipId].displayStartDate) / 1000 / 60);
        // dbを更新する
        // 渡す引数
        jsonData = {
            Method: "ClickBtnToReception",
            SvcinId: gArrObjChip[strChipId].svcInId,
            StallUseId: gArrObjChip[strChipId].stallUseId,
            StallId: gArrObjChip[strChipId].stallId,
            JobDtlId: gArrObjChip[strChipId].jobDtlId,
            StaffId: gArrObjChip[strChipId].StaffId,
            FunctionId: gArrObjChip[strChipId].FunctionId,
            RowLockVersion: gArrObjChip[strChipId].rowLockVersion - 1
        };

        // 本体を削除
        RemoveChipFromStall(strChipId);

        // 削除したチップが赤チップの場合、ストール色を更新する
        if (gArrBackChipObj[0].delayStatus != C_NO_DELAY) {
            // 遅刻ストールを赤に設定する
            SetLaterStallColorRed();
        }

        //コールバック開始
        DoCallBack(C_CALLBACK_WND101, jsonData, SC3240101AfterCallBack, jsonData.Method);
    }
    return;
}
// 2017/09/05 NSK 小川 REQ-SVT-TMT-20160906-003 計画済みチップをストールから簡単に外せるようにする END

// 2019/06/18 NSK 皆川 (トライ店システム評価)次世代サービスオペレーション効率化に向けた、業務適合性検証 START
/**
* 休憩なしボタンを押す
* @return {-} 無し
*/
function ClickBtnNoRest() {
    var strChipId = gSelectedChipId;

    // 日跨ぎタイプを取得する
    var nOverDaysType = GetChipOverDaysType(strChipId);
    // 日跨ぎの場合
    if (C_OVERDAYS_NONE < nOverDaysType) {
        // 最新の情報取得までタップ不可にする
        $("#" + gSelectedChipId).data(C_DATA_CHIPTAP_FLG, false);
    }
        // 日跨ぎでない場合
    else {
    // チップの幅を予定作業時間から再設定する
    $("#" + strChipId).css("width", gArrObjChip[strChipId].scheWorkTime * C_CELL_WIDTH / 15 - 1);
    }

    // JSONの休憩取得フラグを取得しないに変更する
    gArrObjChip[strChipId].restFlg = C_RESTTIMEGETFLG_NOGETREST;

    // 選択したチップを解放する
    SetTableUnSelectedStatus();
    // チップ選択状態を解除する
    SetChipUnSelectedStatus();
    // dbを更新する
    // 渡す引数
    var jsonData = {
        Method: "ClickBtnNoRest",
        ShowDate: $("#hidShowDate").val(),
        SvcInId: gArrObjChip[strChipId].svcInId,
        StallUseId: gArrObjChip[strChipId].stallUseId,
        RowLockVersion: gArrObjChip[strChipId].rowLockVersion
    };
    //rowlockversion+1
    AddRowLockVersionOne(strChipId);
    //コールバック開始
    DoCallBack(C_CALLBACK_WND101, jsonData, SC3240101AfterCallBack, jsonData.Method);
}

/**
* 休憩ありボタンを押す
* @return {-} 無し
*/
function ClickBtnRest() {
    var strChipId = gSelectedChipId;

    // 日跨ぎタイプを取得する
    var nOverDaysType = GetChipOverDaysType(strChipId);
    // 日跨ぎの場合
    if (C_OVERDAYS_NONE < nOverDaysType) {
        // 最新の情報取得までタップ不可にする
        $("#" + gSelectedChipId).data(C_DATA_CHIPTAP_FLG, false);
    }
        // 日跨ぎでない場合
    else {
    // チップの幅に休憩エリアの幅をプラスする
    nRestTimeWidth = GetWidthByRestTime(strChipId);
    $("#" + strChipId).css("width", $("#" + strChipId).width() + nRestTimeWidth);
    }

    // JSONの休憩取得フラグを取得するに変更する
    gArrObjChip[strChipId].restFlg = C_RESTTIMEGETFLG_GETREST;

    var nTapTime = GetTimeByXPos($("#" + strChipId).position().left);

    // 選択したチップを解放する
    SetTableUnSelectedStatus();
    // チップ選択状態を解除する
    SetChipUnSelectedStatus();
    // dbを更新する
    // 渡す引数
    var jsonData = {
        Method: "ClickBtnRest",
        ShowDate: $("#hidShowDate").val(),
        SvcInId: gArrObjChip[strChipId].svcInId,
        StallUseId: gArrObjChip[strChipId].stallUseId,
        RowLockVersion: gArrObjChip[strChipId].rowLockVersion
    };
    //rowlockversion+1
    AddRowLockVersionOne(strChipId);
    //コールバック開始
    DoCallBack(C_CALLBACK_WND101, jsonData, SC3240101AfterCallBack, jsonData.Method);
}
// 2019/06/18 NSK 皆川 (トライ店システム評価)次世代サービスオペレーション効率化に向けた、業務適合性検証 END

/**
* 削除ボタンを押す
* @return {-} 無し
*/
function ClickBtnDel() {
    // 削除確認メッセージを表示する
    var rtValue = ConfirmSC3240101Msg(913);
    // 削除の場合
    if (rtValue) {
        var jsonData;
        var strChipId = gSelectedChipId;
        // 2013/11/29 TMEJ 張 タブレット版SMB チーフテクニシャン機能開発 START
        // 新規チップの場合
        if (strChipId == C_NEWCHIPID) {
            // 選択したチップを解放する
            SetTableUnSelectedStatus();
            // チップ選択状態を解除する
            SetChipUnSelectedStatus();
            return;
        }
        // 2013/11/29 TMEJ 張 タブレット版SMB チーフテクニシャン機能開発 END
        // 重複チップを取得する
        var arrDuplChipIds = GetDuplicateChips(strChipId);

        // 削除したチップが使用不可エリアの場合
        if (IsUnavailableArea(strChipId)) {
            // 選択したチップを解放する
            SetTableUnSelectedStatus();
            // チップ選択状態を解除する
            SetChipUnSelectedStatus();
            var strRowLockVersion = $("#" + strChipId).data("ROW_LOCK_VERSION");
            // 本体を削除する
            $("#" + strChipId).remove();
            jsonData = {
                Method: "ClickBtnDeleteStallUnavailable",
                StallIdleId: right(strChipId, strChipId.length - C_UNAVALIABLECHIPID.length),
                RowLockVersion: strRowLockVersion
            };
            //コールバック開始
            DoCallBack(C_CALLBACK_WND101, jsonData, SC3240101AfterCallBack, jsonData.Method);
            return;
        }

        // 変更する前のデータをバックアップする
        BackupRelationChips(strChipId);

        // 行ロックバージョン+1
        AddRowLockVersionOne(strChipId);

        // リレーションオブジェクト構造体から該チップを削除する
        DeleteChipFromRelationChipObj(strChipId);

        // 選択したチップを解放する
        SetTableUnSelectedStatus();
        // チップ選択状態を解除する
        SetChipUnSelectedStatus();

        // チップのプロトタイプに残作業時間、作業終了予定日時を設定する
        var nBackMinutes = Math.ceil((gArrObjChip[strChipId].displayEndDate - gArrObjChip[strChipId].displayStartDate) / 1000 / 60);
        // dbを更新する
        // 渡す引数
        jsonData = {
            Method: "ClickBtnDeleteStallChip",
            ShowDate: $("#hidShowDate").val(),
            SvcInId: gArrObjChip[strChipId].svcInId,
            StallUseId: gArrObjChip[strChipId].stallUseId,
            RowLockVersion: gArrObjChip[strChipId].rowLockVersion - 1
        };

        // 本体を削除
        RemoveChipFromStall(strChipId);

        // 削除したチップが赤チップの場合、ストール色を更新する
        if (gArrBackChipObj[0].delayStatus != C_NO_DELAY) {
            // 遅刻ストールを赤に設定する
            SetLaterStallColorRed();
        }
        //コールバック開始
        DoCallBack(C_CALLBACK_WND101, jsonData, SC3240101AfterCallBack, jsonData.Method);
    }
}

/**
* 画面から指定チップを削除する
* @param {String} strChipId  指定チップid
* @return {なし} 
*/
function RemoveChipFromStall(strChipId) {
    // 重複チップを取得
    var arrDuplChipIds = GetDuplicateChips(strChipId);

    $("#" + strChipId).remove();
    gArrObjChip[strChipId] = null;

    // 白い枠を削除する
    DeleteWhiteBoarder(arrDuplChipIds, strChipId);
}

/**
* リレーションオブジェクト構造体から該チップを削除する
* @param {String} strChipId  指定チップid
* @return {なし} 
*/
function DeleteChipFromRelationChipObj(strChipId) {
    // リレーションチップを取得する
    var arrRelationChipId = FindRelationChips(strChipId, "");

    // リレーションチップの場合、
    if (IsRelationChip(strChipId) == true) {
        // 削除したあと、ただ2つの場合
        if (arrRelationChipId.length == 2) {
            // 全部削除する
            gArrObjRelationChip[arrRelationChipId[0][0]] = null;
            gArrObjRelationChip[arrRelationChipId[1][0]] = null;
        } else {
            // 関連チップ配列に自分のチップを削除する
            gArrObjRelationChip[strChipId] = null;
        }
    }
}

/**
* 指定チップが他のチップと重複するかどうか
* @param {String} strChipId  指定チップid
* @return {Integer} ワイトボード個数 
*/
function ShowDuplicateChips(strChipId) {
    
    // ポップアップボックスがひょうじされたあとで、白い枠を含めるチップのidにより、チップ重複かどうかを判断する
    if (gWBChipId != "") {
        // 白い枠を削除する
        $("#" + gWBChipId).remove();
        if (strChipId != "") {
            // z-index属性を削除する
            $("#" + strChipId).removeClass("SelectedChipZIndex");
        }
        // 白い枠を含めるチップのidを取得する
        strChipId = gWBChipId.substring(2, gWBChipId.length);
        gWBChipId = "";
    } 

    // 開始と終了により、別々チップの上で白い枠を描画する
    return CreateWhiteBorders(strChipId);
}

/**
* ポップアップボックスにを削除
* @return {なし}
*/
function RemovePopUpBox() {
    // ポップアップボックスが消える
    $(".PopUpChipBoxBorder").remove();
    gPopupBoxId = "";
    // フッター部にクリックイベントを0.5秒で無効する
    gCancelFlg = true;
    setTimeout(function () { gCancelFlg = false; }, 500);
}

/**
* 指定チップが他のチップと重複の時、全部取得する
* @param {String} strChipId  指定チップid
* @return {Array} 重複チップid とポップアップに表示順番
*/
function GetDuplicateChips(strChipId) {

    var arrDuplChipId = new Array();
    // メインストールチップ以外の場合、戻る
    if (!gArrObjChip[strChipId]) {
        return arrDuplChipId;
    }
    // Movingチップ左座標と右座標を記録する
    var nMovingChipLeft = $("#" + strChipId).position().left;
    var nMovingChipRight = nMovingChipLeft + $("#" + strChipId).width();
    // 作業中と作業終了チップのobj
    var objWorkingChip;
    var arrOverWordChip = new Array();
    var nLoop = 0;
    var nLoopOverWork = 0;
    var arrTemp = new Array();
    // この行に全てチップを取得する
    $("#" + strChipId).offsetParent().children("div").each(function (index, e) {
        // Movingチップ以外のチップの場合、左座標と右座標を記録する
        if ((e.id != C_MOVINGCHIPID) && (!IsUnavailableArea(e.id)) && (!IsRestArea(e.id)) && (e.id != "")
            && (!IsKariKariChip(e.id))) {
            var nChipLeft = e.offsetLeft;
            var nChipRight = e.offsetLeft + e.offsetWidth;
            // 重複の場合、重複チップのidを記録する
            if ((!((nChipRight < nMovingChipLeft) || (nChipLeft > nMovingChipRight))
                && (left(e.id.toString(), 2) != "WB") && (gArrObjChip[e.id]))) {
                // 作業中
                if ((IsDefaultDate(gArrObjChip[e.id].rsltStartDateTime) == false) && (IsDefaultDate(gArrObjChip[e.id].rsltEndDateTime) == true)) {
                    // 作業完了チップと作業中チップと重複なれば、無視する（ありえないので）
                    // 作業完了チップではないの場合
                    if ((IsDefaultDate(gArrObjChip[strChipId].rsltEndDateTime)) || (strChipId == e.id)) {
                        objWorkingChip = new Array(e.id, nChipLeft);
                    }
                } else if (IsDefaultDate(gArrObjChip[e.id].rsltEndDateTime) == false) {
                    // 作業終了の場合
                    // 作業完了チップが作業中、作業完了チップと重複なれば、無視する（ありえないので）
                    // 作業中チップではないの場合
                    if ((IsDefaultDate(gArrObjChip[strChipId].rsltStartDateTime)) || (strChipId == e.id)) {
                        arrOverWordChip[nLoopOverWork] = new Array(e.id, nChipLeft);
                        nLoopOverWork++;
                    }
                } else {
                    arrTemp[nLoop] = new Array(e.id, nChipLeft);
                    nLoop++;
                }
            }
        }
    });

    // 最初は作業中チップ
    if (objWorkingChip != null) {
        arrDuplChipId[0] = objWorkingChip;
    }

    // 後は普通チップ
    if (arrTemp.length > 0) {
        // left座標よりソートする
        arrTemp.sort(function (x, y) { return x[1] - y[1] });
        arrDuplChipId = arrDuplChipId.concat(arrTemp);
    }

    // 最後は作業終了チップ
    if (arrOverWordChip.length > 0) {
        arrOverWordChip.sort(function (x, y) { return x[1] - y[1] });
        arrDuplChipId = arrDuplChipId.concat(arrOverWordChip);
    }

    return arrDuplChipId;
}

/**
* Movingチップと重複チップのidを取得する
* @return {Array} 重複チップid とポップアップに表示順番
*/
function GetMovingCpDuplicateChips() {

    // Movingチップ左座標と右座標を記録する
    var nMovingChipLeft = $("#" + C_MOVINGCHIPID).position().left;
    var nMovingChipRight = nMovingChipLeft + $("#" + C_MOVINGCHIPID).width();
    var nLoop = 0;
    var arrDuplChipId = new Array();
    //2013/11/29 TMEJ 張 タブレット版SMB チーフテクニシャン機能開発 START
    var compareChipId;
    //2013/11/29 TMEJ 張 タブレット版SMB チーフテクニシャン機能開発 START

    // 選択したチップが作業中チップかフラグ
    var bWorkingChipFlg = false;
    if (gArrObjChip[gSelectedChipId]) {
        if ((IsDefaultDate(gArrObjChip[gSelectedChipId].rsltStartDateTime) == false) && (IsDefaultDate(gArrObjChip[gSelectedChipId].rsltEndDateTime))) {
            bWorkingChipFlg = true;
        }

        //2013/11/29 TMEJ 張 タブレット版SMB チーフテクニシャン機能開発 START
        compareChipId = gSelectedChipId;       
    } else {
        if (gArrObjSubChip[gSelectedChipId]) {
            compareChipId = gArrObjSubChip[gSelectedChipId].stallUseId;
        }
    }
        //2013/11/29 TMEJ 張 タブレット版SMB チーフテクニシャン機能開発 END

    // この行に全てチップを取得する
    $("#" + C_MOVINGCHIPID).offsetParent().children("div").each(function (index, e) {
        // Movingチップ以外のチップの場合、左座標と右座標を記録する

        //2013/11/29 TMEJ 張 タブレット版SMB チーフテクニシャン機能開発 START
        //if ((e.id != gSelectedChipId) && (e.id != C_MOVINGCHIPID) && (!IsUnavailableArea(e.id)) && (!IsRestArea(e.id))) {
        if ((e.id != compareChipId) && (e.id != C_MOVINGCHIPID) && (!IsUnavailableArea(e.id)) && (!IsRestArea(e.id))) {
        //2013/11/29 TMEJ 張 タブレット版SMB チーフテクニシャン機能開発 END

            if (gArrObjChip[e.id]) {
                // リサイズしたチップが作業中チップ、重複のは作業完了のチップの場合、無視する
                if (!((IsDefaultDate(gArrObjChip[e.id].rsltEndDateTime) == false) && bWorkingChipFlg)) {
                    var nChipLeft = e.offsetLeft;
                    var nChipRight = e.offsetLeft + e.offsetWidth;
                    // 重複の場合、重複チップのidを記録する
                    if ((!((nChipRight < nMovingChipLeft) || (nChipLeft > nMovingChipRight))
                        && (left(e.id.toString(), 2) != "WB"))) {
                        arrDuplChipId[nLoop] = e.id;
                        nLoop++;
                    }
                }
            } else {
                // 仮仮チップと重複する時
                var nChipLeft = e.offsetLeft;
                var nChipRight = e.offsetLeft + e.offsetWidth;
                // 重複の場合、重複チップのidを記録する
                if ((!((nChipRight < nMovingChipLeft) || (nChipLeft > nMovingChipRight))
                            && (left(e.id.toString(), 2) != "WB"))) {
                    arrDuplChipId[nLoop] = e.id;
                    nLoop++;
                }
            }

        }
    });

    return arrDuplChipId;
}
/**
* Movingチップと重複チップのidを取得する
* @return {Array} 重複チップid とポップアップに表示順番
*/
function GetUnavaliableMovingCpDuplicateChips() {

    // Movingチップ左座標と右座標を記録する
    var nMovingChipLeft = $("#" + C_MOVINGUNAVALIABLECHIPID).position().left;
    var nMovingChipRight = nMovingChipLeft + $("#" + C_MOVINGUNAVALIABLECHIPID).width();
    var nLoop = 0;
    var arrDuplChipId = new Array();
    // この行に全てチップを取得する
    $("#" + C_MOVINGUNAVALIABLECHIPID).offsetParent().children("div").each(function (index, e) {
        // Movingチップ以外のチップの場合、左座標と右座標を記録する
        if ((e.id != gSelectedChipId) && (e.id != C_MOVINGUNAVALIABLECHIPID) && (!IsUnavailableArea(e.id)) && (!IsRestArea(e.id))) {
            var nChipLeft = e.offsetLeft;
            var nChipRight = e.offsetLeft + e.offsetWidth;
            // 重複の場合、重複チップのidを記録する
            if ((!((nChipRight < nMovingChipLeft) || (nChipLeft > nMovingChipRight))
                && (left(e.id.toString(), 2) != "WB"))) {
                arrDuplChipId[nLoop] = e.id;
                nLoop++;
            }
        }
    });

    return arrDuplChipId;
}
/**
* 開始と終了により、別々チップの上で白い枠を描画する
* @param {String} strChipId  指定チップid
* @return {無し}
*/
function CreateWhiteBorders(strChipId) {

    // 別のチップと重複するかどうかのチック
    var arrDuplChipId = GetDuplicateChips(strChipId);
    // 重複チップがない場合、元に戻す
    if (arrDuplChipId.length == 1) {
        return;
    }

    // 重複チップに作業中、作業終了チップを全部取得して、arrDuplicateChipsIdに保存する
    var arrDuplicateChipsId = new Array();
    for (var nLoop = 0; nLoop < arrDuplChipId.length; nLoop++) {
        if (((IsDefaultDate(gArrObjChip[arrDuplChipId[nLoop][0]].rsltStartDateTime) == false) && (IsDefaultDate(gArrObjChip[arrDuplChipId[nLoop][0]].rsltEndDateTime) == true))
            || (IsDefaultDate(gArrObjChip[arrDuplChipId[nLoop][0]].rsltEndDateTime) == false)) {
            arrDuplicateChipsId.push(arrDuplChipId[nLoop][0]);
        }
    }

    // 作業中、作業終了チップをループする
    for (var nWorkLoop = 0; nWorkLoop < arrDuplicateChipsId.length; nWorkLoop++) {
        // 作業中、作業終了のチップと重複のチップを取得する
        arrDuplChipId = new Array();
        arrDuplChipId = GetDuplicateChips(arrDuplicateChipsId[nWorkLoop]);

        // 重複の場合、開始しているチップが一番上で表示される
        if (arrDuplChipId.length > 1) {
            for (var nLoop = 0; nLoop < arrDuplChipId.length; nLoop++) {

                // 終了チップがあれば、終了チップと重複の普通チップが全部白い枠を追加する
                if (IsDefaultDate(gArrObjChip[arrDuplChipId[arrDuplChipId.length - 1][0]].rsltEndDateTime) == false) {
                    // 終了チップの場合
                    if (nLoop == arrDuplChipId.length - 1) {
                        // 終了チップのzindexを設定する
                        ChangeChipZIndex(arrDuplChipId[arrDuplChipId.length - 1][0], "WorkOverChipZIndex");
                    } else {
                        // 普通チップ
                        if (!((IsDefaultDate(gArrObjChip[arrDuplChipId[0][0]].rsltStartDateTime) == false) && (IsDefaultDate(gArrObjChip[arrDuplChipId[0][0]].rsltEndDateTime) == true))) {
                            // 白い枠と影を追加する
                            CreateWhiteBorder(arrDuplChipId[nLoop][0]);
                            // もう一回重複チップを探す
                            var arrRezDuplChip = GetDuplicateChips(arrDuplChipId[nLoop][0]);
                            var arrDuplOverChip = new Array();
                            for (var i = 0; i < arrRezDuplChip.length; i++) {
                                arrDuplOverChip[i] = new Array(arrRezDuplChip[i][0], 1);
                            }
                            // 白い枠にタップするイベントを登録する
                            BindWhiteBorderTapEvent(arrDuplOverChip[0][0], arrDuplOverChip);
                            // 普通のチップのzindexを設定する
                            ChangeChipZIndex(arrDuplChipId[nLoop][0], "WorkRezChipZIndex");
                        }
                    }
                }

                // 開始中チップと重複の場合
                if ((IsDefaultDate(gArrObjChip[arrDuplChipId[0][0]].rsltStartDateTime) == false) && (IsDefaultDate(gArrObjChip[arrDuplChipId[0][0]].rsltEndDateTime) == true)) {
                    // 開始中チップの場合
                    if (nLoop == 0) {
                        // 白い枠と影を追加する
                        CreateWhiteBorder(arrDuplChipId[0][0]);
                        // 重複の最後のは終了チップの場合、作業中チップのポップアップボックスに終了チップが無いように
                        if (IsDefaultDate(gArrObjChip[arrDuplChipId[arrDuplChipId.length - 1][0]].rsltEndDateTime) == false) {
                            var arrDuplWorkingChip = new Array();
                            for (var i = 0; i < arrDuplChipId.length - 1; i++) {
                                arrDuplWorkingChip[i] = new Array(arrDuplChipId[i][0], 1);
                            }
                            // 白い枠にタップするイベントを登録する
                            BindWhiteBorderTapEvent(arrDuplChipId[0][0], arrDuplWorkingChip);
                        } else {
                            // 白い枠にタップするイベントを登録する
                            BindWhiteBorderTapEvent(arrDuplChipId[0][0], arrDuplChipId);
                        }
                        // 開始中チップのzindexを設定する
                        ChangeChipZIndex(arrDuplChipId[0][0], "WorkingChipZIndex");
                    } else {
                        // 他のは全部普通のチップのzindexを設定する
                        ChangeChipZIndex(arrDuplChipId[nLoop][0], "WorkRezChipZIndex");
                    }
                }
            }
        }
    }
}
/**
* 指定チップに上のチップで白い枠を生成する(重複のチップの場合)
* @param {String} strChipId  指定チップid
* @return {-} 無し
*/
function CreateWhiteBorder(strChipId) {
    
    // 画面にその枠があれば、削除する
    $("#WB" + strChipId).remove();
    // Movingチップ左座標と右座標を記録する
    var nMovingChipLeft = $("#" + strChipId).position().left;
    var nMovingChipRight = nMovingChipLeft + $("#" + strChipId).width();

    var strClassZIndex = "";
    // チップ種類により、白い枠のzindexを設定する
    if ((IsDefaultDate(gArrObjChip[strChipId].rsltStartDateTime) == false) && (IsDefaultDate(gArrObjChip[strChipId].rsltEndDateTime) == true)) {
        // 作業中
        strClassZIndex = " WorkingChipZIndex";
    } else if (IsDefaultDate(gArrObjChip[strChipId].rsltStartDateTime) == true) {
        // 作業前
        strClassZIndex = " WorkRezChipZIndex";
    }

    var objWhiteBorder = $("<div />").addClass("WhiteBorder selection" + strClassZIndex).append("<p></p>");
    objWhiteBorder.attr("id", "WB" + strChipId);
    var nRowNo = GetRowNoByChipId(strChipId);
    $(".Row" + nRowNo).append(objWhiteBorder);
    $("#WB" + strChipId).css("left", nMovingChipLeft);
    $("#WB" + strChipId).css("width", nMovingChipRight - nMovingChipLeft);
}
/**
* 白いボードをタップイベントを登録する
* @param {String} strChipId チップID
* @param {Array} arrDuplChipId 重複チップID
* @return {なし}
*/
function BindWhiteBorderTapEvent(strChipId, arrDuplChipId) {

    $("#WB" + strChipId).unbind();
    //チップタップ時のイベントを登録
    $("#WB" + strChipId).bind("chipTap", function (e) {

        // チップが選択中
        if (gSelectedChipId) {
            // falseをしなくて、チップが横でタップ座標に移動する
            return false;
        }

        // ポップアップウィンドウが表示中、何もしない
        var nDisplayWnd = GetDisplayPopupWindow();
        if (nDisplayWnd > C_DISP_NONE) {
            if (nDisplayWnd == C_DISP_DUPL) {
                RemovePopUpBox();
            }
            return;
        }

        // 連続で白い枠をタップすると、ウィンドウが表示不正
        if (gCancelFlg) {
            return;
        }

        var bReceptionChipFlg = false;
        // 選択のは受付のチップの場合、bReceptionChipFlgをtrueにする
        if (gArrObjSubChip[gSelectedChipId]) {
            // 受付サブエリアのチップ
            if (gArrObjSubChip[gSelectedChipId].subChipAreaId == C_RECEPTION) {
                bReceptionChipFlg = true;
            }
        }

        // 選択のは受付のチップ以外の情況で、サブエリアを全部閉じる
        if (!bReceptionChipFlg) {
            // サブチップエリアを閉じる
            SetSubChipBoxClose();
            CreateFooterButton(C_FT_DISPTP_UNSELECTED, 0);
            // チップ選択状態を解除する
            SetChipUnSelectedStatus();
            // 選択したチップを解放する
            SetTableUnSelectedStatus();
        }

        // 遅れストール画面が表示中、もとに戻す
        if (gShowLaterStallFlg == true) {
            // もとに戻す
            ShowAllStall();
        }
        // ポップアップボックスが表示している場合、消しる
        if (gPopupBoxId != "") {
            var strPopupBoxId = gPopupBoxId;
            // 他の白い枠にタップする時、新しいポップアップボックスを表示する
            if (strPopupBoxId != ("POPUPBOX_" + strChipId)) {
                // ポップアップボックスを表示する
                CreatePopUpChipBox(strChipId, arrDuplChipId);

            }
        } else {
            // ポップアップボックスを表示する
            CreatePopUpChipBox(strChipId, arrDuplChipId);
        }
        return false;
    });
}
/**
* チップのz-indexを変更する
* @param {Array} strChipId  チップid
* @param {Array} 変更したいZ-Index
* @return {無し} 
*/
function ChangeChipZIndex(strChipId, strZIndex) {

    // 昔のz-indexを削除する
    if ($("#" + strChipId).hasClass("SelectedChipZIndex")) {
        $("#" + strChipId).removeClass("SelectedChipZIndex");
    }
    if ($("#" + strChipId).hasClass("SelectedRelationWorkingChipZIndex")) {
        $("#" + strChipId).removeClass("SelectedRelationWorkingChipZIndex");
    }
    if ($("#" + strChipId).hasClass("SelectedRelationWorkOverChipZIndex")) {
        $("#" + strChipId).removeClass("SelectedRelationWorkOverChipZIndex");
    }
    if ($("#" + strChipId).hasClass("SelectedRelationRezChipZIndex")) {
        $("#" + strChipId).removeClass("SelectedRelationRezChipZIndex");
    }

    if ($("#" + strChipId).hasClass("MovingChipZIndex")) {
        $("#" + strChipId).removeClass("MovingChipZIndex");
    }
    if ($("#" + strChipId).hasClass("WorkOverChipZIndex")) {
        $("#" + strChipId).removeClass("WorkOverChipZIndex");
    }
    if ($("#" + strChipId).hasClass("WorkRezChipZIndex")) {
        $("#" + strChipId).removeClass("WorkRezChipZIndex");
    }
    if ($("#" + strChipId).hasClass("WorkingChipZIndex")) {
        $("#" + strChipId).removeClass("WorkingChipZIndex");
    }
    // 最新のzindexを設定する
    if (strZIndex != "") {
        $("#" + strChipId).addClass(strZIndex);
    }
}
/**
* ディフォルトz-indexを設定する
* @param {Array} strChipId  チップid
* @return {無し} 
*/
function SetDefaultZIndex(strChipId) {

    // 別のチップと重複するかどうかのチック
    var arrDuplChipId = GetDuplicateChips(strChipId);
    // 重複の場合、
    if (arrDuplChipId.length > 1) {
        // 重複チップをループする
        for (var nLoop = 0; nLoop < arrDuplChipId.length; nLoop++) {
            // 作業終了の場合
            if (IsDefaultDate(gArrObjChip[arrDuplChipId[nLoop][0]].rsltEndDateTime) == false) {
                ChangeChipZIndex(arrDuplChipId[nLoop][0], "WorkOverChipZIndex");
            } else if ((IsDefaultDate(gArrObjChip[arrDuplChipId[nLoop][0]].rsltStartDateTime) == false) && (IsDefaultDate(gArrObjChip[arrDuplChipId[nLoop][0]].rsltEndDateTime) == true)) {
                // 作業中の場合
                ChangeChipZIndex(strChipId, "WorkingChipZIndex")
            } else {
                // 普通のチップの場合
                ChangeChipZIndex(arrDuplChipId[nLoop][0], "WorkRezChipZIndex");
            }
        }
    }
}
/**
* 重複チップに上のチップをタップして、ポップアップボクスが表示される
* @param {Array} strChipId  白い枠があるチップid
* @param {Array} arrDuplChipId  重複チップid
* @return {無し} 
*/
function CreatePopUpChipBox(strChipId, arrDuplChipId) {

    // 選択したポップアップボックスをgPopupBoxIdに保存する
    gPopupBoxId = "POPUPBOX_" + strChipId;

    gWBChipId = "WB" + strChipId;
    // ポップアップボクスの枠を生成する
    var objPopUpChipBoxBorder = $("<div />").addClass("PopUpChipBoxBorder");
    var objTriangleBorder = $("<div />").addClass("TriangleBorderDown");
    var objPopUpChipBox = $("<div />").addClass("PopUpChipBox");
    var objPopUpScrollBox = $("<div />").addClass("PopUpScrollBox");
    var objPopUpChipInnerBox = $("<div />").addClass("PopUpChipInnerBox");
    objPopUpChipBoxBorder.append(objTriangleBorder);
    objPopUpChipBoxBorder.append(objPopUpChipBox);
    objPopUpChipBox.append(objPopUpScrollBox);
    objPopUpScrollBox.append(objPopUpChipInnerBox);

    // ポップアップボクスを画面に生成する
    $("#MainArea").append(objPopUpChipBoxBorder);

    // ポップアップボクスの位置と大きさを調整する
    var nWidth, nTop, nLeft;

    // ポップアップボックスが最大8つチップが表示される
    if (arrDuplChipId.length > C_POPUP_MAX_CHIP_NUM) {
        nWidth = C_POPUP_MAX_CHIP_NUM;
    } else {
        nWidth = arrDuplChipId.length;
    }
    // 幅を計算する
    nWidth = (nWidth + 1) * 10 + (nWidth * (C_CELL_WIDTH * 2 - 1));
    // 幅を設定する
    $(".PopUpChipBoxBorder").css("width", nWidth);
    $(".PopUpChipBox").css("width", nWidth - 2);
    $(".PopUpScrollBox").css("width", nWidth - 2);
    // スクロールできるウィンドウ
    var nScrollWidth = arrDuplChipId.length;
    nScrollWidth = (nScrollWidth + 1) * 10 + (nScrollWidth * (C_CELL_WIDTH * 2 - 1));
    $(".PopUpChipInnerBox").css("width", nScrollWidth - 2);

    // 今チップの行数目を取得する
    var nRowNo = GetRowNoByChipId(arrDuplChipId[0][0]);
    var strTriangleBorder = "TriangleBorderDown";
    var offset;
    var nTop;
    // チップの上で2行があれば(ポップアップボクスが2行の高さが必要)
    if (nRowNo - 2 > 0) {
        nRowNo -= 2;
        // 上の2行目から42pxの所はポップアップボックスのtop、18はinnerのpadding: 18px
        nTop = $(".Row" + nRowNo).position().top + 42 + 18 + C_CHIPAREA_OFFSET_TOP + $(".scroll-inner").position().top;
        // top座標を設定する
        $(".PopUpChipBoxBorder").css("top", nTop);
        offset = 13;
    } else {
        nRowNo += 1;
        // 1の2行目から42pxの所はポップアップボックスのtop、18はinnerのpadding: 18px
        nTop = $(".Row" + nRowNo).position().top + 5 + 18 + C_CHIPAREA_OFFSET_TOP + $(".scroll-inner").position().top;
        // top座標を設定する
        $(".PopUpChipBoxBorder").css("top", nTop);
        // 下の△が要らない、上の△を追加する
        $(".TriangleBorderDown").addClass("TriangleBorderUp").removeClass("TriangleBorderDown");
        strTriangleBorder = "TriangleBorderUp";
        offset = 3;
    }

    // 選択したチップの中心のleftを取得する
    var nChipCenter = $("#" + strChipId).position().left + C_CHIPAREA_OFFSET_LEFT + ($("#" + strChipId).width() / 2);
    var nTriangleBorderLeft = nChipCenter - offset - (nChipCenter - (nWidth / 2));
    // ポップアップのleftを計算する
    nLeft = nChipCenter - (nWidth / 2);
    // leftが1より、小さい場合、1に設定する
    if (nLeft - C_CHIPAREA_OFFSET_LEFT < 1) {
        // 左越える
        nTriangleBorderLeft -= 1 + C_CHIPAREA_OFFSET_LEFT - nLeft;
        nLeft = 1 + C_CHIPAREA_OFFSET_LEFT;
    } else if (nLeft + nWidth >= $(".ChipArea").width()) {
        // 右越える
        var nNewLeft = $(".ChipArea").width() - nWidth - 1;
        nTriangleBorderLeft += nLeft - nNewLeft;
        nLeft = nNewLeft;
    }
    // 20はinnerのpadding: 20px
    nLeft += 20 + $(".scroll-inner").position().left;
    // left座標を設定する
    $(".PopUpChipBoxBorder").css("left", nLeft);
    $("." + strTriangleBorder).css("left", nTriangleBorderLeft);

    // ポップアップにチップを生成する
    for (var nLoop = 0; nLoop < arrDuplChipId.length; nLoop++) {
        var objPopUpChip = new ReserveChip("POPUP" + arrDuplChipId[nLoop][0]);
        gArrObjChip[arrDuplChipId[nLoop][0]].copy(objPopUpChip);
        objPopUpChip.stallUseId = "POPUP" + arrDuplChipId[nLoop][0];
        // チップを表示する
        objPopUpChip.createChip(C_CHIPTYPE_POPUP);

        // 選択したチップがあれば、
        if (gSelectedChipId) {
            var strSvcInId = "";
            // 選択したチップの親連番を取得する
            if (gArrObjChip[gSelectedChipId]) {
                strSvcInId = gArrObjChip[gSelectedChipId].svcInId;
            } else if (gArrObjSubChip[gSelectedChipId]) {
                strSvcInId = gArrObjSubChip[gSelectedChipId].svcInId;
            } else if (gSelectedChipId == C_OTHERDTCHIPID) {
                strSvcInId = gOtherDtChipObj.svcInId;
            }
            // 選択したチップとリレーションチップではない場合、BlackBack色を追加する
            if (strSvcInId != objPopUpChip.svcInId) {
                $("#" + objPopUpChip.stallUseId + " .Front").addClass("BlackBack");
            }
        }
        BindPopUpChipTapEvent(objPopUpChip.stallUseId);
    }

    // チップの位置を調整する
    for (var nLoop = 0; nLoop < arrDuplChipId.length; nLoop++) {
        var strPopUpChipId = "POPUP" + arrDuplChipId[nLoop][0];
        $("#" + strPopUpChipId).css("width", (C_CELL_WIDTH - 1) * 2);
        $("#" + strPopUpChipId).css("left", (nLoop + 1) * 10 + (nLoop * (C_CELL_WIDTH * 2 - 1)));
        $("#" + strPopUpChipId).css("top", 10);
        AdjustChipItemByWidth(strPopUpChipId);
    }

    // ポップアップボックスにチップが8枚以上の場合、ポップアップボックスにスクロールできる
    if (arrDuplChipId.length > C_POPUP_MAX_CHIP_NUM) {
        // ポップアップウィンドウにタップする時、画面のスクロールを停止する
        BindPopBoxClickEvent();
        $(".PopUpScrollBox").SmbMainFlickable();
        $(".ui-flickable-wrapper").css("background-color", "");
    }

    // 2013/11/29 TMEJ 張 タブレット版SMB チーフテクニシャン機能開発 START
    //    // 0.5秒後、ポップアップウィンドウ閉じれる
    //    gCanRemoveDuplWndFlg = false;
    //    setTimeout(function () { gCanRemoveDuplWndFlg = true; }, 500);
    // 2013/11/29 TMEJ 張 タブレット版SMB チーフテクニシャン機能開発 END
}

/**
* ポップアップボックスにタップしたイベントを登録
* @return {なし}
*/
function BindPopBoxClickEvent() {

    // チップをリサイズする時、ウィンドウがスクロールできない
    // touch start(スクロールできない)→touch move(リサイズ)→touch end(スクロールできる)
    $(".PopUpChipInnerBox").bind(C_TOUCH_START, function (e) {
        gStopFingerScrollFlg = true;
        $(".ChipArea_trimming").SmbFingerScroll({
            action: "stop"
        });
    });
}

/**
* サーバへチップの開始時間、終了時間、ストールidを設定する
* @param {String} strChipId  チップid
* @param {Date}   nTapTime   表示開始日時
* @param {Number} nWorkTime  作業時間
* @return {なし} 
*/
function setChipDisplayTimeAndStallIdToServer(strChipId, nRestFlg, nTapTime, nWorkTime) {

    var jsonData = "";
    if (nRestFlg != null) {
        //リレーションピーチップの位置、サイズを確定する場合
        if (IsCopyedChip(strChipId)) {
            // 渡す引数
            jsonData = {
                Method: "CreateRelationChip",
                ShowDate: $("#hidShowDate").val(),
                SvcInId: gArrObjChip[strChipId].svcInId,
                StallUseId: gArrObjChip[strChipId].stallUseId,
                JobDtlId: gArrObjChip[strChipId].jobDtlId,
                StallId: gArrObjChip[strChipId].stallId,
                DisplayStartDate: nTapTime,
                ScheWorkTime: (gArrObjChip[strChipId].displayEndDate.getTime() - gArrObjChip[strChipId].displayStartDate.getTime()) / 60 / 1000,
                RestFlg: nRestFlg,
                InspectionNeedFlg:gArrObjChip[strChipId].inspectionNeedFlg,
                RowLockVersion: gArrObjChip[strChipId].rowLockVersion,
                PickDeliType: gArrObjChip[strChipId].pickDeliType,
                ScheSvcinDateTime: gArrObjChip[strChipId].scheSvcInDateTime,
                ScheDeliDateTime:  gArrObjChip[strChipId].scheDeliDateTime
            };
        } else {
            // 渡す引数
            jsonData = {
                Method: "DisplayTimeAndStallId",
                ShowDate: $("#hidShowDate").val(),
                SvcInId: gArrObjChip[strChipId].svcInId,
                StallUseId: gArrObjChip[strChipId].stallUseId,
                StallId: gArrObjChip[strChipId].stallId,
                DisplayStartDate: nTapTime,
                ScheWorkTime: nWorkTime,
                RestFlg: nRestFlg,
                RowLockVersion: gArrObjChip[strChipId].rowLockVersion
            };
        }
    } else {
        //リレーションピーチップの位置、サイズを確定する場合
        if (IsCopyedChip(strChipId)) {
            // 渡す引数
            jsonData = {
                Method: "CreateRelationChip",
                ShowDate: $("#hidShowDate").val(),
                SvcInId: gArrObjChip[strChipId].svcInId,
                StallUseId: gArrObjChip[strChipId].stallUseId,
                JobDtlId: gArrObjChip[strChipId].jobDtlId,
                StallId: gArrObjChip[strChipId].stallId,
                DisplayStartDate: nTapTime,
                ScheWorkTime: (gArrObjChip[strChipId].displayEndDate.getTime() - gArrObjChip[strChipId].displayStartDate.getTime()) / 60 / 1000,
                InspectionNeedFlg: gArrObjChip[strChipId].inspectionNeedFlg,
                RowLockVersion: gArrObjChip[strChipId].rowLockVersion,
                PickDeliType: gArrObjChip[strChipId].pickDeliType,
                ScheSvcinDateTime: gArrObjChip[strChipId].scheSvcInDateTime,
                ScheDeliDateTime: gArrObjChip[strChipId].scheDeliDateTime
            };
        } else {
            // 渡す引数
            jsonData = {
                Method: "DisplayTimeAndStallId",
                ShowDate: $("#hidShowDate").val(),
                SvcInId: gArrObjChip[strChipId].svcInId,
                StallUseId: gArrObjChip[strChipId].stallUseId,
                StallId: gArrObjChip[strChipId].stallId,
                DisplayStartDate: nTapTime,
                ScheWorkTime: nWorkTime,
                RowLockVersion: gArrObjChip[strChipId].rowLockVersion
            };
        }
    }

    // 行ロックバージョン+1
    if (IsCopyedChip(strChipId)) {
        AddRowLockVersionOneBySvcInId(gArrObjChip[strChipId].svcInId);
    } else {
    	AddRowLockVersionOne(strChipId);
    }

    // 2019/12/06 NSK 皆川 (トライ店システム評価)整備作業における休憩時間取得判定向上検証 START
    // 休憩を自動判定する場合
    if ($("#hidRestAutoJudgeFlg").val() == "1") {

        // 関連チップコピー以外の場合
        if (!IsCopyedChip(strChipId)) {

            // 2019/12/06 NSK 皆川 (トライ店システム評価)整備作業における休憩時間取得判定向上検証 END

            // 2019/06/18 NSK 皆川 (トライ店システム評価)次世代サービスオペレーション効率化に向けた、業務適合性検証 START
            // 日跨ぎタイプを取得する
            var nOverDaysType = GetChipOverDaysType(strChipId);
            // 日跨ぎの場合
            if (C_OVERDAYS_NONE < nOverDaysType) {
                // 最新の情報取得までタップ不可にする
                $("#" + gSelectedChipId).data(C_DATA_CHIPTAP_FLG, false);
            }
            // 2019/06/18 NSK 皆川 (トライ店システム評価)次世代サービスオペレーション効率化に向けた、業務適合性検証 END

            // 2019/12/06 NSK 皆川 (トライ店システム評価)整備作業における休憩時間取得判定向上検証 START
        }
    }
    // 2019/12/06 NSK 皆川 (トライ店システム評価)整備作業における休憩時間取得判定向上検証 END

    //コールバック開始
    DoCallBack(C_CALLBACK_WND101, jsonData, SC3240101AfterCallBack, jsonData.Method);
}
/**
* 一時コピーされたチップですか
* @return {なし}
*/
function IsCopyedChip(strChipId) {
    var nChipId = parseInt(strChipId);
    if ((nChipId <= C_ARR_COPYEDCHIPID[0]) && (nChipId >= C_ARR_COPYEDCHIPID[9])) {
        return true;
    } else {
        return false;
    }
}
/**
* ストール利用IDにより、全てリレーションチップの行ロックバージョン+1
* @return {なし}
*/
function AddRowLockVersionOne(strStallUseId) {

    var strNewRowLockVersion = gArrObjChip[strStallUseId].rowLockVersion + 1;
    var arrRelationChipId = FindRelationChips(strStallUseId, "");
    if (arrRelationChipId.length == 0) {
        gArrObjChip[strStallUseId].setRowLockVersion(strNewRowLockVersion);
    } else {
        for (var nLoop = 0; nLoop < arrRelationChipId.length; nLoop++) {
            // チップid
            var strRelationChipId = arrRelationChipId[nLoop][0];
            if (gArrObjChip[strRelationChipId]) {
                gArrObjChip[strRelationChipId].setRowLockVersion(strNewRowLockVersion);
            }
        }
    }
}
/**
* サービス入庫IDにより、全てリレーションチップの行ロックバージョン+1
* @return {なし}
*/
function AddRowLockVersionOneBySvcInId(strSvcinId) {

    var arrRelationChipId = FindRelationChips("", strSvcinId);
    // コピーする前は単独のチップの場合
    if (arrRelationChipId.length == 0) {
        // 単独チップのidを探す
        var arrChipId = FindChipsBySvcinId(strSvcinId);
        // あれば、行ロックバージョン+1
        if (arrChipId.length > 0) {
            gArrObjChip[arrChipId[0]].setRowLockVersion(gArrObjChip[arrChipId[0]].rowLockVersion + 1);
        }
    } else {
        // コピーする前はリレーションチップの場合
    	for (var nLoop = 0; nLoop < arrRelationChipId.length; nLoop++) {
        	// チップid
        	var strRelationChipId = arrRelationChipId[nLoop][0];
        	if (gArrObjChip[strRelationChipId]) {
            	gArrObjChip[strRelationChipId].setRowLockVersion(gArrObjChip[strRelationChipId].rowLockVersion + 1);
            }
        }
    }
}
/**
* メインストールに全てチップをリロードし、再表示する
* @return {なし}
*/
// 2015/10/02 TM 小牟禮 サブエリアの情報取得処理を2回から1回に変更（性能改善） START
// function GetPLanLaterTime() {
function GetPLanLaterTime(isFirst) {
// 2015/10/02 TM 小牟禮 サブエリアの情報取得処理を2回から1回に変更（性能改善） END

    // 全てのストールチップのサービス入庫idを取得する
    var arrSvcinId = new Array();
    for (var strId in gArrObjChip) {
        // 2013/11/29 TMEJ 張 タブレット版SMB チーフテクニシャン機能開発 START
        if (CheckgArrObjChip(strId) == false) {
            continue;
        }
        // 2013/11/29 TMEJ 張 タブレット版SMB チーフテクニシャン機能開発 END
        arrSvcinId.push(gArrObjChip[strId].svcInId);
    }
    // ソートする
    arrSvcinId.sort();
    // サービス入庫IDより、遅れ見込み時間を取得する
    // 2015/10/02 TM 小牟禮 サブエリアの情報取得処理を2回から1回に変更（性能改善） START
    //jsonData = {
    //    Method: "GetPLanLaterTime",
    //    SvcInIdLst: arrSvcinId.toString()
    //};
    var methodName = "GetPLanLaterTime";
    if (isFirst) {
        methodName = "GetPLanLaterTimeForFirstDisplay";
    }
    
    jsonData = {
        Method: methodName,
        SvcInIdLst: arrSvcinId.toString()
    };
    // 2015/10/02 TM 小牟禮 サブエリアの情報取得処理を2回から1回に変更（性能改善） END

    //コールバック開始
    gCallbackSC3240101.doCallback(jsonData, SC3240101AfterCallBack);
}

// 2013/11/29 TMEJ 張 タブレット版SMB チーフテクニシャン機能開発 START
/**
* 画面の更新時間を返す.
* @return {Date}
*/
function getUpdateTime() {
    var dtPreRefreshDatetime = GetServerTimeNow();
    return DateFormat(dtPreRefreshDatetime, gDateFormatYYYYMMddHHmm);
}
// 2013/11/29 TMEJ 張 タブレット版SMB チーフテクニシャン機能開発 END

/**
* コールバック後の処理関数(受付)
* @param {String} result コールバック呼び出し結果
* @param {String} context
*/
function SC3240101AfterCallBack(result) {

    //2015/07/30 TMEJ 明瀬 アクティビティインジケータが消えない問題 START

    ////タイマーをクリア
    //ClearMainWndTimer();

    //2015/07/30 TMEJ 明瀬 アクティビティインジケータが消えない問題 END

    // JSON形式のデータを変換し、処理する.
    var rtList = $.parseJSON(result);

    //2015/07/30 TMEJ 明瀬 アクティビティインジケータが消えない問題 START

    if (rtList.Method != "ShowMainArea" &&
        rtList.Method != "ReShowMainAreaFromTheTime") {
        //Callback処理の発生元が以下以外の場合
        //※以下の場合はAI非表示直後にリフレッシュタイマーをクリアするように改修
        // ・初期表示
        // ・手動更新
        // ・Push更新
        // ・定期更新

        //タイマーをクリア
        ClearMainWndTimer();
    }

    //2015/07/30 TMEJ 明瀬 アクティビティインジケータが消えない問題 END

    // 2014/07/15 TMEJ 張 タブレットSMB Job Dispatch機能開発 START
    if (rtList.Method == "HasBeforeStartJob") {

        //グルグル非表示
        gMainAreaActiveIndicator.hide();

        // 画面の選択項目より、休憩フラグを取得する
        var nRestFlg = C_RESTTIMEGETFLG_NOGETREST;

        if (rtList.ResultCode == 1) {
            // 未開始Jobがある

            // Stall Wait Timeエリア表示しなくて中断ポップアップを表示する
            ShowStallStopDialog(false);

        } else {
            // 未開始Jobがない

            // Stall Wait Timeエリア表示して中断ポップアップを表示する
            ShowStallStopDialog(true);

        }

        // 前回送信内容がリストからクリアする
        AfterCallBack();

        return;
    }
    
    // 2014/07/15 TMEJ 張 タブレットSMB Job Dispatch機能開発 END

    // 2019/06/18 NSK 皆川 (トライ店システム評価)次世代サービスオペレーション効率化に向けた、業務適合性検証 START
    if (rtList.Method == "GetCanRestChange") {

        var canRestChange;
        if (rtList.ResultCode == 1) {
            // 休憩変更可能
            canRestChange = true;
        } else {
            // 休憩変更不可
            canRestChange = false;
        }

        // 前回送信内容をリストからクリアする
        AfterCallBack();

        //フッターボタンを変える
        CreateFooterButton(C_FT_DISPTP_SELECTED, GetChipType(gSelectedChipId), gArrObjChip[gSelectedChipId].tlmContractFlg, canRestChange);

        return;
    }
    // 2019/06/18 NSK 皆川 (トライ店システム評価)次世代サービスオペレーション効率化に向けた、業務適合性検証 END
    // 2013/11/29 TMEJ 張 タブレット版SMB チーフテクニシャン機能開発 START
//    if (rtList.ResultCode == C_NO_ERROR) {
    if ((rtList.ResultCode == C_NO_ERROR)
        || (rtList.MessageId == C_CALLBACK_JUST_ALERT)) {

        // 2013/11/29 TMEJ 張 タブレット版SMB チーフテクニシャン機能開発 END
        switch (rtList.Method) {

            // 初期画面表示★★★
            case "ShowMainArea":
                $("#hidJsonData").val(htmlDecode(rtList.ChipInfo));

                // タブレットの表示が速い為、
                setTimeout(function () {
                    // 関連チップ情報をリセットする
                    ResetRelationChipInfo(htmlDecode(rtList.RelationChipInfo));

                    //2015/07/30 TMEJ 明瀬 アクティビティインジケータが消えない問題 START

                    // メイン画面表示(リレション線描画が可能なので、リレションチップ情報リセットをまずやる)
                    //ShowMainArea();

                    ShowMainArea(rtList.Method);

                    //2015/07/30 TMEJ 明瀬 アクティビティインジケータが消えない問題 END

                    // 2013/11/29 TMEJ 張 タブレット版SMB チーフテクニシャン機能開発 START
                    // 作業中チップの履歴情報の設定
                    SetWorkingChipHis(htmlDecode(rtList.WorkingChipHisInfo));
                    // 2013/11/29 TMEJ 張 タブレット版SMB チーフテクニシャン機能開発 END

                    // 遅れ見込み時間を取得する
                    setTimeout(function () {
                        // 2015/10/02 TM 小牟禮 サブエリアの情報取得処理を2回から1回に変更（性能改善） START
                        //GetPLanLaterTime();
                        GetPLanLaterTime(true);
                        // 2015/10/02 TM 小牟禮 サブエリアの情報取得処理を2回から1回に変更（性能改善） END

                        setTimeout(function () {
                            SubChipSearch();
                        }, 0);
                    }, 0);
                }, 0);

                // ストールエリアの社員を更新する
                UpdateStallArea(htmlDecode(rtList.TechnicianInfo));

                // 非稼働時間エリアを表示
                ShowIdleArea(htmlDecode(rtList.StallIdleInfo));

                // 仮仮チップを表示する
                ShowKariKariChip(htmlDecode(rtList.KariKariChipInfo));

                // 2013/11/29 TMEJ 張 タブレット版SMB チーフテクニシャン機能開発 START
                //更新時間を設定する
                $("#MessageUpdateTime").text(getUpdateTime());

                if (rtList.MessageId == C_CALLBACK_JUST_ALERT) {
                    alert(rtList.Message);
                }

                // 2013/11/29 TMEJ 張 タブレット版SMB チーフテクニシャン機能開発 END
                break;

            //画面再描画★★★
            case "ReShowMainArea":

                // 日付を変える時、受付、noshow、中断エリアのチップが選択された場合、サブエリアが閉じられない(gSelectedChipIdのデータを保持する)
                var strSubChipId = "";
                if ((gArrObjSubChip[gSelectedChipId])
                    && ((gArrObjSubChip[gSelectedChipId].subChipAreaId == C_RECEPTION)
                    || (gArrObjSubChip[gSelectedChipId].subChipAreaId == C_NOSHOW)
                    || (gArrObjSubChip[gSelectedChipId].subChipAreaId == C_STOP))) {
                    strSubChipId = gSelectedChipId;
                }       
                // 全てチップを削除する
                RemoveAllStallChips();

                gSelectedChipId = strSubChipId;

                // 営業時間をリセット
                SetStallDate();
                $("#hidJsonData").val(htmlDecode(rtList.ChipInfo));
                setTimeout(function () {
                    // 関連チップ情報をリセットする
                    ResetRelationChipInfo(htmlDecode(rtList.RelationChipInfo));
                    // メインエリアを再表示する(リレション線描画が可能なので、リレションチップ情報リセットをまずやる)
                    ShowMainArea();
                    // 2013/11/29 TMEJ 張 タブレット版SMB チーフテクニシャン機能開発 START
                    // 作業中チップの履歴情報の設定
                    SetWorkingChipHis(htmlDecode(rtList.WorkingChipHisInfo));
                    // 2013/11/29 TMEJ 張 タブレット版SMB チーフテクニシャン機能開発 END

                    if (gSearchedChipId) {
                        if (gArrObjChip[gSearchedChipId]) {
                            // チップ表示するために、スクロールする
                            ScrollToShowChip(gSearchedChipId);
                            // 選択中にする
                            TapStallChip(gSearchedChipId);
                            gSearchedChipId = "";
                        }
                    }
                    // 遅れ見込み時間を取得する
                    setTimeout(function () { GetPLanLaterTime(); }, 0);
                }, 0);
                // ストールエリアの社員を更新する
                UpdateStallArea(htmlDecode(rtList.TechnicianInfo));

                // 非稼働時間エリアを表示
                ShowIdleArea(htmlDecode(rtList.StallIdleInfo));

                // 仮仮チップを表示する
                ShowKariKariChip(htmlDecode(rtList.KariKariChipInfo));

				// 2013/11/29 TMEJ 張 タブレット版SMB チーフテクニシャン機能開発 START
                //更新時間を設定する
                $("#MessageUpdateTime").text(getUpdateTime());

                if (rtList.MessageId == C_CALLBACK_JUST_ALERT) {
                    alert(rtList.Message);
                }

                // 2013/11/29 TMEJ 張 タブレット版SMB チーフテクニシャン機能開発 END
                break;

                // 2013/11/29 TMEJ 張 タブレット版SMB チーフテクニシャン機能開発 START 

            //画面再描画（日時変更）★★★ 
            case "ReShowMainAreaFromTheTime":
                // 営業時間をリセット
                SetStallDate();
                $("#hidJsonData").val(htmlDecode(rtList.ChipInfo));
                setTimeout(function () {
                    // 当画面リフレッシュ
                    RefreshMainArea(htmlDecode(rtList.ChipInfo));
                    // 関連チップ情報を切替える
                    SetRelationChipInfo(htmlDecode(rtList.RelationChipInfo));
                    // 作業中チップの履歴情報の設定
                    SetWorkingChipHis(htmlDecode(rtList.WorkingChipHisInfo));

                    if (gSearchedChipId) {
                        if (gArrObjChip[gSearchedChipId]) {
                            // チップ表示するために、スクロールする
                            ScrollToShowChip(gSearchedChipId);
                            // 選択中にする
                            TapStallChip(gSearchedChipId);
                            gSearchedChipId = "";
                        }
                    }
                    // 遅れ見込み時間を取得する
                    setTimeout(function () { GetPLanLaterTime(); }, 0);
                }, 0);

                // ストールエリアの社員を更新する
                UpdateStallArea(htmlDecode(rtList.TechnicianInfo));

                // 非稼働時間エリアを表示
                ShowIdleArea(htmlDecode(rtList.StallIdleInfo));

                // 仮仮チップを表示する
                ShowKariKariChip(htmlDecode(rtList.KariKariChipInfo));

                //更新時間を設定する
                $("#MessageUpdateTime").text(getUpdateTime());

                if (rtList.MessageId == C_CALLBACK_JUST_ALERT) {
                    alert(rtList.Message);
                }

                break;
                // 2013/11/29 TMEJ 張 タブレット版SMB チーフテクニシャン機能開発 END

            //遅れ見込み時間を計算して取得する★★★   
            case "GetPLanLaterTime":
                // 更新した後で、更新されたチップの情報を取得する
                var strJsonData = htmlDecode(rtList.Contents);
                // 遅れ見込み時間をストール上のチップに更新する
                RefreshPlanDeliTime(strJsonData);

                setTimeout(function () {
                    // 全てボタンの数字、色をリフレッシュ
                    AllButtonRefresh(); 
                    // 次のcallbackを送信する
                    AfterCallBack();
                }, 0);
                break;

            // 2015/10/02 TM 小牟禮 サブエリアの情報取得処理を2回から1回に変更（性能改善） START   
            //遅れ見込み時間を計算して取得する★★★    
            case "GetPLanLaterTimeForFirstDisplay":
                // 更新した後で、更新されたチップの情報を取得する
                var strJsonData = htmlDecode(rtList.Contents);
                // 遅れ見込み時間をストール上のチップに更新する
                RefreshPlanDeliTime(strJsonData);

                break;
            // 2015/10/02 TM 小牟禮 サブエリアの情報取得処理を2回から1回に変更（性能改善）END    
             
            //中断成功した後の処理★★★   
            case "ClickBtnStopJob":
                // 更新した後で、更新されたチップの情報を取得する
                var jsonData = htmlDecode(rtList.Contents);
                // 表示する
                // 2013/11/29 TMEJ 張 タブレット版SMB チーフテクニシャン機能開発 START
                //ShowLatestChips(jsonData);
                ShowLatestChips(jsonData, false, false);
                // 2013/11/29 TMEJ 張 タブレット版SMB チーフテクニシャン機能開発 END

                // 新規した使用不可エリアのIDを取得する
                var strStallIdleId = htmlDecode(rtList.NewStallIdleId);
                // 新規した使用不可エリアチップがあれば、
                if ((strStallIdleId != "0") && (strStallIdleId != "")) {
                    // 画面に新規移動不可チップがあれば
                    if ($("#" + C_UNAVALIABLENEWCHIPID).length == 1) {
                        // 新規した、idが一時用の使用不可チップを削除する
                        $("#" + C_UNAVALIABLENEWCHIPID).remove();
                        // 該行移動不可チップ全部リフレッシュ
                        UpdateIdleArea(htmlDecode(rtList.StallIdleInfo));
                    }
                    // 使用不可エリアのイベントを登録する
                    BindUnavailableAreaEvent(C_UNAVALIABLECHIPID + strStallIdleId);
                }
                // 中断ボタン数値を更新する
                StopButtonRefresh();

                //2015/05/14 TMEJ 小澤 DMS連携版サービスタブレット強制納車機能追加開発 START
                //MessageIdのチェック
                if (rtList.MessageId == C_CALLBACK_JUST_ALERT) {
                    //「2：エラーを出した後で、画面がそのままで表示される」の場合
                    //メッセージを表示する
                    alert(rtList.Message);

                }
                //2015/05/14 TMEJ 小澤 DMS連携版サービスタブレット強制納車機能追加開発 END

                // 次のcallbackを送信する
                AfterCallBack();
                break;
            // 2013/11/29 TMEJ 張 タブレット版SMB チーフテクニシャン機能開発 START
            case "GetTechnicians":
            case "SetStallTechnicians":
                var strTechnicianInfo = htmlDecode(rtList.TechnicianInfo);
                // ストールエリアの社員を更新する
                UpdateStallArea(strTechnicianInfo);
                // テクニシャンウィンドウにデータを表示する
                SetTechnicianWndData(strTechnicianInfo);

                // 次のcallbackを送信する
                AfterCallBack();
                break; 
            // 2013/11/29 TMEJ 張 タブレット版SMB チーフテクニシャン機能開発 END
                // NoShow後の処理
            case "ClickBtnNoshow":
                // 更新した後で、更新されたチップの情報を取得する
                var strJsonData = htmlDecode(rtList.Contents);
                // 表示する
                // 2013/11/29 TMEJ 張 タブレット版SMB チーフテクニシャン機能開発 START
                //ShowLatestChips(strJsonData);
                ShowLatestChips(strJsonData, false, false);
                // 2013/11/29 TMEJ 張 タブレット版SMB チーフテクニシャン機能開発 END
                // NoShowボタン数値を更新する
                NoShowButtonRefresh();

                //2015/05/14 TMEJ 小澤 DMS連携版サービスタブレット強制納車機能追加開発 START
                //MessageIdのチェック
                if (rtList.MessageId == C_CALLBACK_JUST_ALERT) {
                    //「2：エラーを出した後で、画面がそのままで表示される」の場合
                    //メッセージを表示する
                    alert(rtList.Message);

                }
                //2015/05/14 TMEJ 小澤 DMS連携版サービスタブレット強制納車機能追加開発 END

                // 次のcallbackを送信する
                AfterCallBack();
                break;
                //リレーションコピーした後の処理
            case "CreateRelationChip":
                // 更新した後で、更新されたチップの情報を取得する
                var strJsonData = htmlDecode(rtList.Contents);
                // 生成されたリレーションコピーチップを最新のデータで更新する
                ShowCreatedRelationChips(strJsonData);
                // 関連チップの設定
                AddRelationChipInfo(htmlDecode(rtList.RelationChipInfo));

                //2015/05/14 TMEJ 小澤 DMS連携版サービスタブレット強制納車機能追加開発 START
                //MessageIdのチェック
                if (rtList.MessageId == C_CALLBACK_JUST_ALERT) {
                    //「2：エラーを出した後で、画面がそのままで表示される」の場合
                    //メッセージを表示する
                    alert(rtList.Message);

                }
                //2015/05/14 TMEJ 小澤 DMS連携版サービスタブレット強制納車機能追加開発 END

                // 次のcallbackを送信する
                AfterCallBack();
                break;
                //ストールチップを削除する時
            case "ClickBtnDeleteStallChip":
                // 更新した後で、更新されたチップの情報を取得する
                var strJsonData = htmlDecode(rtList.Contents);
                // 表示する
                // 2013/11/29 TMEJ 張 タブレット版SMB チーフテクニシャン機能開発 START
                //ShowLatestChips(strJsonData);
                ShowLatestChips(strJsonData, false, false);
                // 2013/11/29 TMEJ 張 タブレット版SMB チーフテクニシャン機能開発 END
                // 全てボタンの数字、色をリフレッシュ
                AllButtonRefresh();

                //2015/05/14 TMEJ 小澤 DMS連携版サービスタブレット強制納車機能追加開発 START
                //MessageIdのチェック
                if (rtList.MessageId == C_CALLBACK_JUST_ALERT) {
                    //「2：エラーを出した後で、画面がそのままで表示される」の場合
                    //メッセージを表示する
                    alert(rtList.Message);

                }
                //2015/05/14 TMEJ 小澤 DMS連携版サービスタブレット強制納車機能追加開発 END

                // 次のcallbackを送信する
                AfterCallBack();
                break;
                // 終了ボタンを押すと
            case "ClickBtnFinish":

                // 更新した後で、更新されたチップの情報を取得する
                var strJsonData = htmlDecode(rtList.Contents);
                // 表示する
                // 2013/11/29 TMEJ 張 タブレット版SMB チーフテクニシャン機能開発 START
                //ShowLatestChips(strJsonData);
                ShowLatestChips(strJsonData, false, false);
                // 2013/11/29 TMEJ 張 タブレット版SMB チーフテクニシャン機能開発 END
                // 洗車、完成検査ボタン数値を更新する
                CompletionInspecButtonRefresh();
                CarWashButtonRefresh();

                //2014/07/15 TMEJ 張 タブレットSMB Job Dispatch機能開発 START
                // 中断ボタン数値更新(終了操作でチップが中断エリアに移動可能性がある)
                StopButtonRefresh();
                //2014/07/15 TMEJ 張 タブレットSMB Job Dispatch機能開発 END

                //2015/05/14 TMEJ 小澤 DMS連携版サービスタブレット強制納車機能追加開発 START
                //MessageIdのチェック
                if (rtList.MessageId == C_CALLBACK_JUST_ALERT) {
                    //「2：エラーを出した後で、画面がそのままで表示される」の場合
                    //メッセージを表示する
                    alert(rtList.Message);

                }
                //2015/05/14 TMEJ 小澤 DMS連携版サービスタブレット強制納車機能追加開発 END

                // 次のcallbackを送信する
                AfterCallBack();
                break;
            case "UpdateStallUnavailable":
                // 非稼働時間エリアを更新
                UpdateIdleArea(htmlDecode(rtList.StallIdleInfo));
                // 次のcallbackを送信する
                AfterCallBack();
                break;
                // 2013/11/29 TMEJ 張 タブレット版SMB チーフテクニシャン機能開発 START
            case "ClickBtnStart":

                //2014/07/15 TMEJ 張 タブレットSMB Job Dispatch機能開発 START

                // 渡す用のJsonのID
                var backupJsonId = rtList.StallUseId;

                // MessageIdを取得して、3(確認ボックスを出す)の場合
                if (C_CALLBACK_CONFIRM == rtList.MessageId) {

                    // 中断含むJob開始確認ボックスを出す
                    var result = confirm(htmlDecode(rtList.Message));

                    if (result) {
                        // OK選択時

                        // 中断されてる作業再開する
                        gBackupStartJson[backupJsonId].ReStartJobFlg = C_RESTARTJOB_YES;

                    } else {
                        // Cancel選択時

                        // 中断されてる作業再開しない
                        gBackupStartJson[backupJsonId].ReStartJobFlg = C_RESTARTJOB_NO;

                        // 中断Job含むピンク色を付ける
                        $("#" + backupJsonId).addClass("StoppingJobColor");
                    }

                    // CallBack列挙体に追加する
                    DoCallBack(C_CALLBACK_WND101, gBackupStartJson[backupJsonId], SC3240101AfterCallBack, gBackupStartJson[backupJsonId].Method);

                    // 次のcallbackを送信する
                    AfterCallBack();

                    // バックアップの構造体をクリアする
                    gBackupStartJson[backupJsonId] = null;

                    return;
                }

                // バックアップの構造体をクリアする
                gBackupStartJson[backupJsonId] = null;

                //2014/07/15 TMEJ 張 タブレットSMB Job Dispatch機能開発 END

                // 更新した後で、更新されたチップの情報を取得する
                var strJsonData = htmlDecode(rtList.Contents);
                // 表示する
                // 2013/11/29 TMEJ 張 タブレット版SMB チーフテクニシャン機能開発 START
                //ShowLatestChips(strJsonData);
                ShowLatestChips(strJsonData, false, false);
                // 2013/11/29 TMEJ 張 タブレット版SMB チーフテクニシャン機能開発 END

                // 作業中チップの履歴情報の設定
                SetWorkingChipHis(htmlDecode(rtList.WorkingChipHisInfo));

                //2020/01/07 NSK 坂本 TR-SVT-TMT-20190118-001 Stop start tagのXMLログが過去日19000101を表示している START
                // タップ不可の場合、タップ可能にする
                if ($("#" + rtList.StallUseId).length > 0) {

                    if ($("#" + rtList.StallUseId).data(C_DATA_CHIPTAP_FLG) == false) {

                        // タップ可能
                        $("#" + rtList.StallUseId).data(C_DATA_CHIPTAP_FLG, true);

                    }

                    // 白い枠がある場合
                    if ($("#WB" + rtList.StallUseId).length > 0) {

                        // 削除する
                        $("#WB" + rtList.StallUseId).remove();
                    }

                }
                //2020/01/07 NSK 坂本 TR-SVT-TMT-20190118-001 Stop start tagのXMLログが過去日19000101を表示している END

                //2015/05/14 TMEJ 小澤 DMS連携版サービスタブレット強制納車機能追加開発 START
                //MessageIdのチェック
                if (rtList.MessageId == C_CALLBACK_JUST_ALERT) {
                    //「2：エラーを出した後で、画面がそのままで表示される」の場合
                    //メッセージを表示する
                    alert(rtList.Message);

                }
                //2015/05/14 TMEJ 小澤 DMS連携版サービスタブレット強制納車機能追加開発 END

                // 次のcallbackを送信する
                AfterCallBack();
                break;
            // 初期画面表示 
            case "DisplayTimeAndStallId":
                // 2019/06/18 NSK 皆川 (トライ店システム評価)次世代サービスオペレーション効率化に向けた、業務適合性検証 START
            case "ClickBtnNoRest":
            case "ClickBtnRest":
                // 2019/06/18 NSK 皆川 (トライ店システム評価)次世代サービスオペレーション効率化に向けた、業務適合性検証 END
                // 更新した後で、更新されたチップの情報を取得する
                var strJsonData = htmlDecode(rtList.Contents);
                // 表示する
                ShowLatestChips(strJsonData, false, false);
                // 関連チップ情報を切替える
                SetRelationChipInfo(htmlDecode(rtList.RelationChipInfo));

                // 2019/06/18 NSK 皆川 (トライ店システム評価)次世代サービスオペレーション効率化に向けた、業務適合性検証 START
                // タップ不可の場合、タップ可能にする
                if ($("#" + rtList.StallUseId).length > 0) {

                    if ($("#" + rtList.StallUseId).data(C_DATA_CHIPTAP_FLG) == false) {

                        // タップ可能
                        $("#" + rtList.StallUseId).data(C_DATA_CHIPTAP_FLG, true);

                    }

                    // 白い枠がある場合
                    if ($("#WB" + rtList.StallUseId).length > 0) {

                        // 削除する
                        $("#WB" + rtList.StallUseId).remove();
                    }

                }
                // 2019/06/18 NSK 皆川 (トライ店システム評価)次世代サービスオペレーション効率化に向けた、業務適合性検証 END

                //2015/05/14 TMEJ 小澤 DMS連携版サービスタブレット強制納車機能追加開発 START
                //MessageIdのチェック
                if (rtList.MessageId == C_CALLBACK_JUST_ALERT) {
                    //「2：エラーを出した後で、画面がそのままで表示される」の場合
                    //メッセージを表示する
                    alert(rtList.Message);

                }
                //2015/05/14 TMEJ 小澤 DMS連携版サービスタブレット強制納車機能追加開発 END

                // 次のcallbackを送信する
                AfterCallBack();
                // 2013/11/29 TMEJ 張 タブレット版SMB チーフテクニシャン機能開発 END

                //2015/05/14 TMEJ 小澤 DMS連携版サービスタブレット強制納車機能追加開発 START

                break;

                //2015/05/14 TMEJ 小澤 DMS連携版サービスタブレット強制納車機能追加開発 END 

                //UNDO
            case "UndoMainStallChip":
                // 更新した後で、更新されたチップの情報を取得する
                var strJsonData = htmlDecode(rtList.Contents);
                // 表示する
                // 2013/11/29 TMEJ 張 タブレット版SMB チーフテクニシャン機能開発 START
                //ShowLatestChips(strJsonData);
                ShowLatestChips(strJsonData, false, false);

                //TMEJ 丁　リレションラインの不具合修正　START
                if (rtList.RelationChipInfo != null) {
                    // 関連チップ情報を追加、変更する
                    AddRelationChipInfo(htmlDecode(rtList.RelationChipInfo));
                }

                // タップ不可の場合、タップ可能にする
                if ($("#" + rtList.StallUseId).length > 0) {

                    if ($("#" + rtList.StallUseId).data(C_DATA_CHIPTAP_FLG) == false) {

                        // タップ可能
                        $("#" + rtList.StallUseId).data(C_DATA_CHIPTAP_FLG, true);

                    }

                    // 白い枠がある場合
                    if ($("#WB" + rtList.StallUseId).length > 0) {

                        // 削除する
                        $("#WB" + rtList.StallUseId).remove();
                    }

                }
                //TMEJ 丁　リレションラインの不具合修正　END

                //2015/05/14 TMEJ 小澤 DMS連携版サービスタブレット強制納車機能追加開発 START
                //MessageIdのチェック
                if (rtList.MessageId == C_CALLBACK_JUST_ALERT) {
                    //「2：エラーを出した後で、画面がそのままで表示される」の場合
                    //メッセージを表示する
                    alert(rtList.Message);

                }
                //2015/05/14 TMEJ 小澤 DMS連携版サービスタブレット強制納車機能追加開発 END


                // 2013/11/29 TMEJ 張 タブレット版SMB チーフテクニシャン機能開発 END
                // 次のcallbackを送信する
                AfterCallBack();
                break;

            // 2017/09/05 NSK 小川 REQ-SVT-TMT-20160906-003 計画済みチップをストールから簡単に外せるようにする START 
            case "ClickBtnToReception":
                // 更新した後で、更新されたチップの情報を取得する
                var strJsonData = htmlDecode(rtList.Contents);
                // 表示する
                ShowLatestChips(strJsonData, false, false);
                // 受付ボタン数値を更新する
                ReceptionButtonRefresh();

                //MessageIdのチェック
                if (rtList.MessageId == C_CALLBACK_JUST_ALERT) {
                    //「2：エラーを出した後で、画面がそのままで表示される」の場合
                    //メッセージを表示する
                    alert(rtList.Message);

                }
                // 次のcallbackを送信する
                AfterCallBack();
                break;
            // 2017/09/05 NSK 小川 REQ-SVT-TMT-20160906-003 計画済みチップをストールから簡単に外せるようにする END  

            default:
                // 更新した後で、更新されたチップの情報を取得する
                var strJsonData = htmlDecode(rtList.Contents);
                // 表示する
                // 2013/11/29 TMEJ 張 タブレット版SMB チーフテクニシャン機能開発 START
                //ShowLatestChips(strJsonData);
                ShowLatestChips(strJsonData, false, false);

                //TMEJ 丁　リレションラインの不具合修正　START
                if (rtList.RelationChipInfo != null) {
                    // 関連チップ情報を追加、変更する
                    AddRelationChipInfo(htmlDecode(rtList.RelationChipInfo));
                } //TMEJ 丁　リレションラインの不具合修正　END

                //2015/05/14 TMEJ 小澤 DMS連携版サービスタブレット強制納車機能追加開発 START
                //MessageIdのチェック
                if (rtList.MessageId == C_CALLBACK_JUST_ALERT) {
                    //「2：エラーを出した後で、画面がそのままで表示される」の場合
                    //メッセージを表示する
                    alert(rtList.Message);

                }
                //2015/05/14 TMEJ 小澤 DMS連携版サービスタブレット強制納車機能追加開発 END

                // 2013/11/29 TMEJ 張 タブレット版SMB チーフテクニシャン機能開発 END
                // 次のcallbackを送信する
                AfterCallBack();
                break;
        }
    } else {
        // エラーメッセージが表示される
        alert(rtList.Message);
        // 選択状態を解除する
        SetTableUnSelectedStatus();
        SetChipUnSelectedStatus();
        // あとの操作の返信がいらない
        ClearAllMainWndTimer();
        // 2013/11/29 TMEJ 張 タブレット版SMB チーフテクニシャン機能開発 START
        ShowLatestChips(htmlDecode(rtList.Contents), false, true);
        // 作業中チップの履歴情報の設定
        SetWorkingChipHis(htmlDecode(rtList.WorkingChipHisInfo));
        if (rtList.RelationChipInfo != null) {
            // 関連チップ情報を追加、変更する
            AddRelationChipInfo(htmlDecode(rtList.RelationChipInfo));
        }
        // 2013/11/29 TMEJ 張 タブレット版SMB チーフテクニシャン機能開発 END
        // リフレッシュ
        ClickChangeDate(0);
    }
    // 記録のオブジェクトをクリアする
    gArrBackChipObj.length = 0;
}

/**
* 仮仮チップを表示する
* @param {string} 仮チップ情報のstring
* @return {-} 
*/
function ShowKariKariChip(strKariKariChipJson) {
    //JSON形式のチップ情報読み込み
    var chipDataList = $.parseJSON(strKariKariChipJson);
    //取得した関連チップ情報をgArrObjRelationChipクラスに格納
    for (var keyString in chipDataList) {
        var chipData = chipDataList[keyString];

        var strKey = C_KARIKARICHIPID + chipData.SVCIN_TEMP_RESV_ID;
        var objKariChip = new ReserveChip(strKey);
        //データを設定する
        objKariChip.setKariKariChipParameter(chipData);
        // 開始時間、終了時間が全部当ページにする
        if (objKariChip.displayStartDate - gStartWorkTime < 0) {
            objKariChip.setDisplayStartDate(gStartWorkTime);
        }

        if (objKariChip.displayEndDate - gEndWorkTime > 0) {
            objKariChip.setDisplayEndDate(gEndWorkTime);
        }

        //生成する
        objKariChip.createChip(C_CHIPTYPE_STALL_KARIKARI);
        SetChipPosition(strKey, objKariChip.stallId, objKariChip.displayStartDate, objKariChip.displayEndDate);
    }
}

// 2013/11/29 TMEJ 張 タブレット版SMB チーフテクニシャン機能開発 START
/**
* 作業中チップの履歴情報の設定
* @return {-} 
*/
function SetWorkingChipHis(strChipHisJson) {

    //JSON形式のチップ情報読み込み
    var chipDataList = $.parseJSON(strChipHisJson);
    //取得した関連チップ情報をgArrObjRelationChipクラスに格納
    for (var keyString in chipDataList) {
        var chipHisData = chipDataList[keyString];

        var strKey = chipHisData.STALL_USE_ID;
        // 画面に存在の場合、履歴情報を追加する
        if (gArrObjChip[strKey]) {
            gArrObjChip[strKey].SetWorkingChipHis(chipHisData);
        }
    }
}
// 2013/11/29 TMEJ 張 タブレット版SMB チーフテクニシャン機能開発 END

/**
* 関連チップ情報をリセット
* @return {-} 
*/
function ResetRelationChipInfo(strRelationChipJson) {
    // gArrObjRelationChipをクリア
    gArrObjRelationChip = new Array();

    //JSON形式のチップ情報読み込み
    var chipDataList = $.parseJSON(strRelationChipJson);
    //取得した関連チップ情報をgArrObjRelationChipクラスに格納
    for (var keyString in chipDataList) {
        var chipData = chipDataList[keyString];

        var strKey = chipData.STALL_USE_ID;
        if (gArrObjRelationChip[strKey] == undefined) {
            gArrObjRelationChip[strKey] = new RelationChip(strKey);
        }
        gArrObjRelationChip[strKey].setChipParameter(chipData);
    }
}
/**
* 関連チップ情報を追加
* @return {-} 
*/
function AddRelationChipInfo(strRelationChipJson) {
    //JSON形式のチップ情報読み込み
    var chipDataList = $.parseJSON(strRelationChipJson);

    // undefinedの場合、arrayをnewする
    if (!gArrObjRelationChip) {
        gArrObjRelationChip = new Array();
    }
    //取得した関連チップ情報をgArrObjRelationChipクラスに格納
    for (var keyString in chipDataList) {
        var chipData = chipDataList[keyString];

        var strKey = chipData.STALL_USE_ID;
        if (!gArrObjRelationChip[strKey]) {
            gArrObjRelationChip[strKey] = new RelationChip(strKey);
        }
        gArrObjRelationChip[strKey].setChipParameter(chipData);
    }
}

// 2013/11/29 TMEJ 張 タブレット版SMB チーフテクニシャン機能開発 START
/**
* 関連チップ情報を部分切替える
* @return {-} 
*/
function SetRelationChipInfo(strRelationChipJson) {
    //JSON形式のチップ情報読み込み
    var chipDataList = $.parseJSON(strRelationChipJson);

    // undefinedの場合、arrayをnewする
    if (!gArrObjRelationChip) {
        gArrObjRelationChip = new Array();
    }
    //取得した関連チップ情報をgArrObjRelationChipクラスに格納
    for (var keyString in chipDataList) {
        var chipData = chipDataList[keyString];

        var strKey = chipData.STALL_USE_ID;
        gArrObjRelationChip[strKey] = new RelationChip(strKey);
        gArrObjRelationChip[strKey].setChipParameter(chipData);
    }
}
// 2013/11/29 TMEJ 張 タブレット版SMB チーフテクニシャン機能開発 END

/**
* バックアップチップが全部ロールバック
* @return {-} 
*/
function BackChipRollBack() {
    for (var nLoop = 0; nLoop < gArrBackChipObj.length; nLoop++) {
        // 1枚チップをロールバックする
        RollBackOneChip(gArrBackChipObj[nLoop]);
    }
    // 白い枠を再表示する
    ReshowAllWhiteBoards();
    // 赤ストールを生成する
    SetLaterStallColorRed();
}

/**
* 全画面の白い枠を再表示
* @return {-} 
*/
function ReshowAllWhiteBoards() {
    // 白い枠全部削除する
    $(".WhiteBorder").remove();
    var arrDuplicateChipsId = new Array();
    // 取得したチップ情報をチップクラスに格納し、再描画.
    for (var strId in gArrObjChip) {
        // 有効のチップデータをチェックする
        if (CheckgArrObjChip(strId) == false) {
            continue;
        }

        // 作業中または作業完了の場合、該チップのidを記録する
        if ((((IsDefaultDate(gArrObjChip[strId].rsltStartDateTime) == false) && (IsDefaultDate(gArrObjChip[strId].rsltEndDateTime) == true))
            || (IsDefaultDate(gArrObjChip[strId].rsltEndDateTime) == false))) {
            arrDuplicateChipsId.push(strId);
        }
    }

    // 重複チップの枠を描画
    // 作業中または作業完了チップの重複チェック
    for (var strChipId in arrDuplicateChipsId) {
        // 有効のチップデータをチェックする
        if (CheckgArrObjChip(arrDuplicateChipsId[strChipId]) == false) {
            continue;
        }
        // 重複チップarrayを取得する
        CreateWhiteBorders(arrDuplicateChipsId[strChipId]);
    }
}
/**
* 1枚チップをロールバックする
* @return {-} 
*/
function RollBackOneChip(objBackChip) {
    // objBackChipがあれば、
    if (objBackChip) {
        var strChipId = objBackChip.stallUseId;
        // 削除ボタン押した場合、gArrObjChip[strChipId]はnull
        if (gArrObjChip[strChipId] == null) {
            gArrObjChip[strChipId] = new ReserveChip(strChipId);
            objBackChip.copy(gArrObjChip[strChipId]);
            // チップ生成
            gArrObjChip[strChipId].createChip(C_CHIPTYPE_STALL);
            // チップをタップする時のイベントを登録
            BindChipClickEvent(strChipId);
        } else {
            objBackChip.copy(gArrObjChip[strChipId]);
        }

        // チップを更新する
        gArrObjChip[strChipId].updateStallChip();
        SetChipPosition(strChipId, "", "", "");
    }
}

/**
* リレーションチップを全部バックアップする
* @return {-} 
*/
function BackupRelationChips(strChipId) {
    var objBackChip;
    // 記録のオブジェクトをクリアする
    gArrBackChipObj.length = 0;
    // リレーションチップ探す
    var arrRelationChipId = FindRelationChips(strChipId, "");
    // 関連チップがあれば、関連チップ全部バックする
    if (arrRelationChipId.length > 0) {
        for (var nLoop = 0; nLoop < arrRelationChipId.length; nLoop++) {
            // チップid
            var strRelationChipId = arrRelationChipId[nLoop][0];
            if (gArrObjChip[strRelationChipId]) {
                objBackChip = new ReserveChip(strRelationChipId);
                gArrObjChip[strRelationChipId].copy(objBackChip);
                gArrBackChipObj.push(objBackChip);
            }
        }
    } else {
        // 自分1つの場合、自分をバックする
        objBackChip = new ReserveChip(strChipId);
        gArrObjChip[strChipId].copy(objBackChip);
        gArrBackChipObj.push(objBackChip);
    }

}
/**
* 画面を再表示する(commonRefreshTimerにセットする関数)
* @return {-} 
*/
function ReDisplay() {
    //画面にリフレッシュする
    __doPostBack("", "");
    return false;
}
/**
* チップ置く位置チェック
* @param {String} strChipId チップID
* @return {Bool} false：置けない位置に置けした
*/
function CheckChipPos(strChipId) {
    var arrDuplChipId;
    // 重複チェック
    if (strChipId == C_MOVINGCHIPID) {
        arrDuplChipId = GetMovingCpDuplicateChips();
        // 重複の場合、
        if (arrDuplChipId.length > 0) {
            return false;
        }
    } else if (strChipId == C_MOVINGUNAVALIABLECHIPID) {
        arrDuplChipId = GetUnavaliableMovingCpDuplicateChips();
        // 重複の場合、
        if (arrDuplChipId.length > 0) {
            return false;
        } 
    
    } else {
        arrDuplChipId = GetDuplicateChips(strChipId);
        // 重複の場合、
        if (arrDuplChipId.length > 1) {
            return false;
        }
    }

    return true;
}

/**
* ディフォルト日付をチェック
* @param {String} strChipId チェック日付
* @return {Bool} true：ディフォルト日付
*/
function IsDefaultDate(dtDate) {
    var dtDefault = new Date(C_DATE_DEFAULT_VALUE);

    if ((dtDate - dtDefault) == 0) {
        return true;
    } else {
        return false;
    }
}

/**
* チップIDリストにあるチップの白い枠を全て削除する
* @param {Array} arrDuplChipId 重複のチェックID
* @param {String} strChipId チェックID
* @return {なし} 
*/
function DeleteWhiteBoarder(arrDuplChipId, strChipId) {

    if (arrDuplChipId.length > 0) {
        for (var nLoop = 0; nLoop < arrDuplChipId.length; nLoop++) {
            $("#WB" + arrDuplChipId[nLoop][0]).remove();
        }
    } else {
        $("#WB" + strChipId).remove();
    }

}

// 2013/11/29 TMEJ 張 タブレット版SMB チーフテクニシャン機能開発 START
/**
* 画面ストールに表示されたチップの最大のROW_UPDATE_DATETIMEを取得する
* @return {なし} 
*/
function GetPreRefreshDate() {

    var dtMaxUpdateDate = new Date();
    // チップがない場合、今日0時0分0秒のデータを戻す
    dtMaxUpdateDate.setHours(0);
    dtMaxUpdateDate.setMinutes(0);
    dtMaxUpdateDate.setSeconds(0);
    dtMaxUpdateDate.setMilliseconds(0);

    // すべてチップをループして、一番大きいのROW_UPDATE_DATETIMEを取得する
    for (var nChipNo in gArrObjChip) {
        // 有効のチップデータをチェックする
        if (CheckgArrObjChip(nChipNo) == false) {
            continue;
        }

        if (dtMaxUpdateDate - gArrObjChip[nChipNo].rowUpdateDateTime < 0) {
            dtMaxUpdateDate.setTime(gArrObjChip[nChipNo].rowUpdateDateTime.getTime());
        }
    }

    // 秒から切り捨てる
    dtMaxUpdateDate.setSeconds(0);
    dtMaxUpdateDate.setMilliseconds(0);

    // 2014/09/12 TMEJ 張 BTS-390 「1ｽﾄｰﾙで2ﾁｯﾌﾟが作業中（表示のみ）」対応 StartChip

    // クライアントからサーバーにcallback時、待ってる時間
    var offsetMilliSecond = Number($("#MstPG_RefreshTimerTime").val());

    // 更新用の基準日時は最新のチップ更新日時 - 非同期処理待ってる時間
    dtMaxUpdateDate.setTime(dtMaxUpdateDate.getTime() - offsetMilliSecond);

    // 2014/09/12 TMEJ 張 BTS-390 「1ｽﾄｰﾙで2ﾁｯﾌﾟが作業中（表示のみ）」対応 END

    return dtMaxUpdateDate;
}
// 2013/11/29 TMEJ 張 タブレット版SMB チーフテクニシャン機能開発 END

//2015/02/09 TMEJ 範 DMS連携版サービスタブレット SMB納車予定時刻通知機能開発 START
/**
* チップの納車遅れ(遅れ見込も含む)を確認
* @param {Object} chipObject チップクラスオブジェクト
* @return {Boolean} true：納車遅れ/False：遅れなし
* 
*/
function IsDelayDelivery(chipObject) {

    //予定納車日時がデフォルト値(未設定)の場合は、遅れ無し
    if (IsDefaultDate(chipObject.scheDeliDateTime)) {

        //遅れなし
        return false;

    }

    //現在日時の取得
    var nowDateTime = GetServerTimeNow();

    //秒とミリ秒の切り捨て
    nowDateTime.setSeconds(0);
    nowDateTime.setMilliseconds(0);

    //予定納車日時 < 現在日時の場合
    if (chipObject.scheDeliDateTime < nowDateTime) {
        
        //納車遅れ
        return true;
    
    }

    //納車遅れ見込み時間(黄色線の時間)取得
    // 2017/09/25 NSK 竹中(悠) REQ-SVT-TMT-20170227-002 見込納車遅れ日時計算ロジック統一 START
    // var dtDeliDelay = GetDeliDelayExpectedTimeLine(chipObject.carWashNeedFlg
    //                                              , chipObject.cwRsltStartDateTime
    //                                              , chipObject.scheDeliDateTime
    //                                              , chipObject.svcStatus);
    var dtDeliDelay = GetDeliDelayExpectedTimeLine(chipObject.carWashNeedFlg
                                                 , chipObject.scheDeliDateTime
                                                 , chipObject.svcStatus
                                                 , chipObject.remainingInspectionType);
    // 2017/09/25 NSK 竹中(悠) REQ-SVT-TMT-20170227-002 見込納車遅れ日時計算ロジック統一 END
    //チップの終了日時が黄色線を超えた場合
    if (dtDeliDelay < chipObject.displayEndDate) {
        
        //納車遅れ見込
        return true;

    }

    //TODO：現在時刻 > 黄色線時刻 - 残作業時間の場合も納車遅れ見込
    //※ただし、処理が複雑となるため「DMS連携版サービスタブレット SMB納車予定時刻通知機能開発」ではこのケースを考えない

    //遅れなし
    return false;
}

//2015/02/09 TMEJ 範 DMS連携版サービスタブレット SMB納車予定時刻通知機能開発 END

// 2019/07/22 NSK 近藤 (トライ店システム評価)次世代サービスオペレーション効率化に向けた、業務適合性検証 START
/**
* 移動先のチップの開始日時が当日が当日かどうか調べる
* @param {String} strChipId  チップid
* @return {Bool} true:チップの開始日時が当日
*/
function IsTodayStartDate(strChipId) {

    // 引数の値が不正な場合、判定不可能なためfalseを返却する
    if (strChipId == "" || strChipId == null) {
        return false;
    }

    var strMovingChip;
    if (strChipId == C_NEWCHIPID) {
        strMovingChip = C_NEWCHIPID;
    } else {
        strMovingChip = C_MOVINGCHIPID;
    }

    // 移動先のチップに左端の爪がある場合、当日の開始日時
    if ($("#" + strMovingChip + " .TimeKnobPointL").length == 1) {
        return true;
    }

    // ストール上のチップのオブジェクトがある場合
    if (gArrObjChip[strChipId]) {

        // 予定チップの場合
        if (IsDefaultDate(gArrObjChip[strChipId].rsltStartDateTime)) {
            return false;

            // 実績チップの場合
        } else {
            // 実績開始日時を取得する
            var dtRsltStartDateTime = new Date(gArrObjChip[strChipId].rsltStartDateTime);

            // 当ページの日付を取得する
            var dtShowDate = new Date($("#hidShowDate").val());

            // 実績開始日時が当ページの日付より前の場合
            if (CompareDate(dtRsltStartDateTime, dtShowDate) < 0) {
                return false;
            }
            else {
                return true;
            }
        }
    }
        // ストール上のチップのオブジェクトがない場合
    else {
        // 開始日時が当日となるためtrueを返却する
        return true;
    }
}
// 2019/07/22 NSK 近藤 (トライ店システム評価)次世代サービスオペレーション効率化に向けた、業務適合性検証 END
