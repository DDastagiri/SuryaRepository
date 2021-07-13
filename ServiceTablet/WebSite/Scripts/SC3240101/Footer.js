//---------------------------------------------------------
//Footer.js
//---------------------------------------------------------
//機能：フッター部
//作成：2012/12/28 TMEJ 小澤 タブレット版SMB機能開発(工程管理)
//更新：2013/11/29 TMEJ 丁 タブレット版SMB チーフテクニシャン機能開発
//更新：2013/11/29 TMEJ 張 タブレット版SMB チーフテクニシャン機能開発
//更新：2014/06/18 TMEJ 丁 タブレット版SMB テレマ走行距離機能開発
//更新：2014/07/14 TMEJ 張 タブレット版SMB 作業終了ボタン蓋締め
//更新：2014/07/15 TMEJ 張 タブレットSMB Job Dispatch機能開発
//更新：2014/12/08 TMEJ 丁 DMS連携版サービスタブレット JobDispatch完成検査入力制御開発
//更新：2015/02/13 TMEJ 範 新プラットフォーム版i-CROP仕様変更対応 (SMBチップ検索及び作業中断機能強化)
//更新：2017/08/30 NSK 竹中(悠) REQ-SVT-TMT-20161109-001 SMB iPadにストールクローズ機能の追加
//更新：2017/09/05 NSK 小川 REQ-SVT-TMT-20160906-003 計画済みチップをストールから簡単に外せるようにする
//更新：2017/10/21 NSK 小川 REQ-SVT-TMT-20160906-003 子チップがキャンセルできない
//更新：2019/06/18 NSK 皆川 (トライ店システム評価)次世代サービスオペレーション効率化に向けた、業務適合性検証
//更新：2019/12/06 NSK 皆川 (トライ店システム評価)整備作業における休憩時間取得判定向上検証
//---------------------------------------------------------
var bkButtonID = "";

//---------------------------------------------------------
//フッターボタンタップ時のイベント
//buttonID＝ボタンID
//---------------------------------------------------------
function FooterEvent(buttonID) {
    // 無効フラグがtrueの場合、クリックを無効にする
    if (gCancelFlg) {
        gCancelFlg = false;
        return;
    }
    // 初期メイン画面がグルグルの場合、ボタンを押すと、押すイベントの反応がない
    if ($("#MainAreaActiveIndicator").hasClass("show") == true) {
        return;
    }

    // ボタンを青色にする
    $("#FooterButton" + buttonID).addClass("btn-pressed");

    setTimeout(function () {
        // ボタンの青色を解除
        $("#FooterButton" + buttonID).removeClass("btn-pressed");
        // 受付～納車待ちボタンを押す時、
        if ((buttonID >= 100) && (buttonID <= 700)) {
            // ストールが遅れ絞り込みの場合、解除する
            if (gShowLaterStallFlg == true) {
                // 解除する
                ShowAllStall();
            }
        }
        //受付ボタン～遅れボタンまでボタンのon/off状態を切り替える
        if (buttonID <= C_FT_BTNID_LATER) {
            FooterIconReplace(buttonID);
        }
        //イベント用スクリプトをここに記述
        if (buttonID == 100) {
            //受付
            ShowReceptionchip();
        } else if (buttonID == 200) {
            //追加作業
            ShowAddWorkchip();
        } else if (buttonID == C_FT_BTNID_CONFIRMED) {
            //完成検査
            ShowCompletionchip();
        } else if (buttonID == 400) {
            //洗車
            ShowCarWashchip();
        } else if (buttonID == 500) {
            //納車待ち
            ShowDeliverdCarchip();
        } else if (buttonID == 600) {
            //No Show
            ShowNoShowchip();
        } else if (buttonID == 700) {
            //中断
            ShowStopchip();
        } else if (buttonID == C_FT_BTNID_LATER) {
            //遅れ
            // サブエリア全部非表示にする
            SetSubChipBoxClose();
            //count値があれば(遅れストールがあれば)、
            if ($("#FooterButtonCount800")[0].innerText != "") {
                //遅れストールだけ表示される
                ShowLaterStall();
            } else {
                //
                FooterIconReplace(C_FT_BTNID_LATER);
            }
        } else if (buttonID == C_FT_BTNID_DETAIL) {

            //フッターのボタンを全て非表示にする
            HideAllFooterButton();

            var scrollSubBoxObj = null;
            var translateSubBoxVal = null;

            //選択中チップのエリア判定
            var areaId = GetSubChipType(gSelectedChipId);
            SetForceScroll(areaId);

            // 2013/11/29 TMEJ 張 タブレット版SMB チーフテクニシャン機能開発 START
            //チップ詳細(小)画面を表示
            //ShowChipDetailSmall();
            // 選択されたチップが新規チップの場合
            if (gSelectedChipId == C_NEWCHIPID) {
                //チップ新規作成画面を表示
                ShowNewChip();
                return false;
            } //2017/08/30 NSK 竹中(悠) REQ-SVT-TMT-20161109-001 SMB iPadにストールクローズ機能の追加 START 
            else if (IsUnavailableArea(gSelectedChipId)) {
                // 使用不可画面(更新モード)
                ShowUnavailableSetting()
                return false;
            } //2017/08/30 NSK 竹中(悠) REQ-SVT-TMT-20161109-001 SMB iPadにストールクローズ機能の追加 END 
            else {
                //チップ詳細(小)画面を表示
                ShowChipDetailSmall();
                return false;
            }
            // 2013/11/29 TMEJ 張 タブレット版SMB チーフテクニシャン機能開発 END
            return false;

        } else if (buttonID == C_FT_BTNID_REZ_CONFIRMED) {
            //予約確定
            ClickRezConFirmed();
        } else if (buttonID == C_FT_BTNID_CARIN) {
            //入庫
            ClickBtnCarIn();
        } else if (buttonID == C_FT_BTNID_CANCEL_CARIN) {
            //入庫取消
            ClickBtnCancelCarIn();
        } else if (buttonID == C_FT_BTNID_TENTATIVE_REZ) {
            //予約確定取消
            ClickCancelRezConFirmed();
        } else if (buttonID == C_FT_BTNID_NOSHOW) {
            //No Show
            ClickNoShow();
        } else if (buttonID == C_FT_BTNID_START) {
            //開始
            ClickBtnStart();
        } else if (buttonID == C_FT_BTNID_END) {
            // 2014/07/14 TMEJ 張 タブレット版SMB 作業終了ボタン蓋締め START
            //終了
            //            ClickBtnFinish();
            // 2014/07/14 TMEJ 張 タブレット版SMB 作業終了ボタン蓋締め END

            // 2014/07/15 TMEJ 張 タブレットSMB Job Dispatch機能開発 START		
            //終了
            ClickBtnFinish();
            // 2014/07/15 TMEJ 張 タブレットSMB Job Dispatch機能開発 END		
        } else if (buttonID == C_FT_BTNID_STOPJOB) {
            //中断
            ClickBtnStopJob();
        } else if (buttonID == 1800) {
            //完検承認
            ClickBtnInspection();
        } else if (buttonID == 1900) {
            //追加承認
            ClickBtnAddWorkConfirm();
        } else if (buttonID == C_FT_BTNID_WASHSTART) {
            //洗車開始
            ClickBtnStartWashCar();
        } else if (buttonID == C_FT_BTNID_WASHEND) {
            //洗車終了
            ClickBtnEndWashCar();
        } else if (buttonID == C_FT_BTNID_DELI) {
            //納車
            ClickBtnDeli();
        } else if (buttonID == 2300) {
            //コピー
            ClickCopy();
        } else if (buttonID == 2400) {
            //再計画
        } else if (buttonID == C_FT_BTNID_DEL) {
            //削除
            if (gArrObjChip[gSelectedChipId]) {
                ClickBtnDel();
            } else if (gArrObjSubChip[gSelectedChipId]) {
                SubChipCancel();
            } else if (IsUnavailableArea(gSelectedChipId)) {
                ClickBtnDel();
            } else if (gSelectedChipId == C_COPYCHIPID) {
                // コピーチップを削除する
                DeleteCopyedChip();
            }

        } else if (buttonID == C_FT_BTNID_UNDO) {
            //Undo
            // 2013/11/29 TMEJ 丁 タブレット版SMB チーフテクニシャン機能開発 START
            if (gArrObjChip[gSelectedChipId]) {
                // メインストールチップが選択した場合、undoする
                UndoMainStallChip();
            } else {
                // 洗車中チップが選択した場合、undoして、洗車待ちにする
                UndoWashingChip();
            }
            //        } else if (buttonID == 2700) {
            //            //Redo
            //        } else if (buttonID == 2800) {
            //            //日跨ぎ終了
            //            ClickMidFinish();
            //        }
        } else if (buttonID == C_FT_BTNID_MIDFINISH) {
            //日跨ぎ終了
            ClickMidFinish();
        } else if (buttonID == C_FT_BTNTP_MOVETOWASH) {
            //洗車へ移動ボタン
            ClickMoveToWash();
        } else if (buttonID == C_FT_BTNTP_MOVETODELI) {
            //納車へ移動ボタン
            ClickMoveToDeliWait();
        }
        // 2013/11/29 TMEJ 丁 タブレット版SMB チーフテクニシャン機能開発 END

        //2014/06/18 TMEJ 丁 タブレット版SMB テレマ走行距離機能開発 START
        else if (buttonID == C_FT_BTNID_GBOOK) {
            //G-BOOKボタン
            ClickGBook();
        }
        //2014/06/18 TMEJ 丁 タブレット版SMB テレマ走行距離機能開発 END

        // 2017/09/05 NSK 小川 REQ-SVT-TMT-20160906-003 計画済みチップをストールから簡単に外せるようにする START
        else if (buttonID == C_FT_BTNID_TORECEPTION) {
            //計画取り消しボタン 
            ClickToReception();
        }
        // '2017/09/05 NSK 小川 REQ-SVT-TMT-20160906-003 計画済みチップをストールから簡単に外せるようにする END

        //2015/02/13 TMEJ 範 新プラットフォーム版i-CROP仕様変更対応 (SMBチップ検索及び作業中断機能強化) START
        else if (buttonID == C_FT_BTNID_FINISHSTOPCHIP) {
            //次工程へ移動ボタン

            //次工程へ移動ボタンタップ前に
            //一度ストールをタップした場合に表示される移動用チップを削除する
            //※次工程へ移動ボタンタップ後にチップの移動処理をできないようにするため
            $("#" + C_MOVINGCHIPID).remove();

            //中断終了ボタン
            ClickFinishStopChip();
        }
        //2015/02/13 TMEJ 範 新プラットフォーム版i-CROP仕様変更対応 (SMBチップ検索及び作業中断機能強化) END

        //2017/08/30 NSK 竹中(悠) REQ-SVT-TMT-20161109-001 SMB iPadにストールクローズ機能の追加 START
        else if (buttonID == C_FT_BTNID_UNAVAILABLESETTING) {

            //フッターのボタンを全て非表示にする
            HideAllFooterButton();
            var scrollSubBoxObj = null;
            var translateSubBoxVal = null;

            //選択中チップのエリア判定
            var areaId = GetSubChipType(gSelectedChipId);
            SetForceScroll(areaId);

            //ストール使用不可設定ポップアップを表示
            ShowUnavailableSetting()
        }
        //2017/08/30 NSK 竹中(悠) REQ-SVT-TMT-20161109-001 SMB iPadにストールクローズ機能の追加 END

        // 2019/06/18 NSK 皆川 (トライ店システム評価)次世代サービスオペレーション効率化に向けた、業務適合性検証 START
        else if (buttonID == C_FT_BTNTP_NOBREAK) {
            // 休憩なしボタン
            ClickBtnNoRest();
        }
        else if (buttonID == C_FT_BTNTP_BREAK) {
            // 休憩ありボタン
            ClickBtnRest();
        }
        // 2019/06/18 NSK 皆川 (トライ店システム評価)次世代サービスオペレーション効率化に向けた、業務適合性検証 END

    }, 300);
}

/**
* フッタボタンの表示制御.
* @return {void}
*/
function MainButtonClick() {
    $.master.OpenLoadingScreen();
    return true;
}

//2013/11/29 TMEJ 張 タブレット版SMB チーフテクニシャン機能開発 START
/**
* 顧客詳細クリックイベント.
* @return {}
*/
function BindCustomerDetailEvent() {
    $('#MstPG_FootItem_Main_700').bind("click", function (event) {
        //ヘッダーの顧客検索にフォーカスを当てる
        $('#MstPG_CustomerSearchTextBox').focus();

        //ボタン背景点灯
        $('#MstPG_FootItem_Main_700').addClass("icrop-pressed");
        setTimeout(function () {
            //ボタン背景を戻す
            $('#MstPG_FootItem_Main_700').removeClass("icrop-pressed");
        }, 500);
        event.stopPropagation();
    });
}

/**
* フッターボタンのクリックイベント.
* @return {}
*/
function FooterButtonClick(Id) {

    // 2013/11/29 TMEJ 張 タブレット版SMB チーフテクニシャン機能開発 START 
//    //TCボタンの場合は何もしない
//    if  (Id == 200) {
//        return false;
//    }

    //顧客詳細ボタン、TCボタンの場合は何もしない
    if ((Id == 700) || (Id == 200)) {
        return false;
    }
    // 2013/11/29 TMEJ 張 タブレット版SMB チーフテクニシャン機能開発 END 

    //全体クルクル表示
    $.master.OpenLoadingScreen();

    //各イベント処理実行
    switch (Id) {
        case 1100:
            //SMBボタン
            __doPostBack('ctl00$MstPG_FootItem_Main_1100', '');
            break;
        case 300:
            //FM
            __doPostBack('ctl00$MstPG_FootItem_Main_300', '');
            break;
        case 400:
            //予約ボタン
            __doPostBack('ctl00$MstPG_FootItem_Main_400', '');
            break;
        case 500:
            //R/Oボタン
            __doPostBack('ctl00$MstPG_FootItem_Main_500', '');
            break; 
        case 700:
            //顧客ボタン
             __doPostBack('ctl00$MstPG_FootItem_Main_700', '');
            break;
        case 800:
            //商品訴求
            __doPostBack('ctl00$MstPG_FootItem_Main_800', '');
            break;
        case 900:
            //キャンペーンボタン
            __doPostBack('ctl00$MstPG_FootItem_Main_900', '');
            break;
        case 1200:
            //追加作業ボタン
            __doPostBack('ctl00$MstPG_FootItem_Main_1200', '');
            break;
    }
}

/**
* フッタボタンの切替える
* @return {void}
*/
function ChangeButtonEvent() {
    if (gButtonStatus == 1) {
        gButtonStatus = 0;
        // ボタンの初期状態に戻る
        $("#InitFooterArea").attr("style", "");
        // アプリボタンの初期状態に戻る
        HideAplicationButton();
    } else {
        gButtonStatus = 1;
        // ボタンの初期状態を切替える
        $("#InitFooterArea").attr("style", "display:none;");
        ShowAplicationButton();
    }
}
/**
* 基盤のボタンを初期化する
* @return {void}
*/
function HideAplicationButton() {
    // 100がメインボタン、メインボタン以外の基盤のボタンを非表示にする
    for (var index = 2; index <= 20; index++) {
        if ($("#" + C_APPLICATION_BUTTON + (index * 100)).length == 1) {
            // SMBボタンの場合
            if (index == 11) {
                // CTとCHTの場合、表示されない
                if ((gOpeCode == C_OPECODE_CT) || (gOpeCode == C_OPECODE_CHT)) {
                    $("#" + C_APPLICATION_BUTTON + (index * 100)).css("display", "none");
                }
            } else {
                $("#" + C_APPLICATION_BUTTON + (index * 100)).css("display", "none");
            }
        }
    }
}
/**
* 基盤のボタンの表示ように
* @return {void}
*/
function ShowAplicationButton() {
    // 100がメインボタン、メインボタン以外の基盤のボタンを非表示にする
    for (var index = 2; index <= 20; index++) {
        if ($("#" + C_APPLICATION_BUTTON + (index * 100)).length == 1) {
            $("#" + C_APPLICATION_BUTTON + (index * 100)).css("display", "block");
        }
    }
}

/**
* スケジュールボタンと電話帳ボタンの設定する.
* @return {}
*/
function SetFooterApplication() {
    //スケジュールオブジェクトを拡張
    $.extend(window, { schedule: {} });
    //スケジュールオブジェクトに関数追加
    $.extend(schedule, {
        //アプリ起動クラス
        appExecute: {
            //カレンダーアプリ起動(単体)
            executeCaleNew: function () {
                window.location = "icrop:cale:";
                return false;
            },
            //電話帳アプリ起動(単体)
            executeCont: function () {
                window.location = "icrop:cont:";
                return false;
            }
        }
    });
}
//2013/11/29 TMEJ 張 タブレット版SMB チーフテクニシャン機能開発 END

//---------------------------------------------------------
//ボタンOffOnの切り替え
//buttonID＝ボタンID
//---------------------------------------------------------
function FooterIconReplace(buttonID) {
    //新規ボタン情報
    var newFooterButton = "FooterButton" + buttonID;
    var newFooterButtonBox = "FooterButtonBox" + buttonID;
    var newFooterButtonType1 = document.getElementById(newFooterButton).className;
    var newFooterButtonType2 = newFooterButtonType1.indexOf("_");
    var newFooterButtonType3 = newFooterButtonType1.substring(0, newFooterButtonType2);

    //前回と同じボタン情報の場合
    if (bkButtonID == buttonID) {
        document.getElementById(newFooterButton).className = newFooterButtonType3 + "_Off FooterButton";
        if (newFooterButtonType3 != "VisibilityIcon") {
            document.getElementById(newFooterButtonBox).className = "FooterLightBox";
        }
        bkButtonID = "";
    //前回と違うボタン情報の場合
    } else if (bkButtonID != "") {
        var oldFooterButton = "FooterButton" + bkButtonID;
        var oldFooterButtonBox = "FooterButtonBox" + bkButtonID;
        var oldFooterButtonType1 = document.getElementById(oldFooterButton).className;
        var oldFooterButtonType2 = oldFooterButtonType1.indexOf("_");
        var oldFooterButtonType3 = oldFooterButtonType1.substring(0, oldFooterButtonType2);

        document.getElementById(newFooterButton).className = newFooterButtonType3 + "_On FooterButton";
        if (newFooterButtonType3 != "VisibilityIcon") {
            document.getElementById(newFooterButtonBox).className = "";
        }

        document.getElementById(oldFooterButton).className = oldFooterButtonType3 + "_Off FooterButton";
        if (oldFooterButtonType3 != "VisibilityIcon") {
            document.getElementById(oldFooterButtonBox).className = "FooterLightBox";
        }
        bkButtonID = buttonID;

    //前回の情報がない場合
    } else {
        document.getElementById(newFooterButton).className = newFooterButtonType3 + "_On FooterButton";
        document.getElementById(newFooterButtonBox).className = "";
        bkButtonID = buttonID;
    }
}

//2019/06/18 NSK 皆川 (トライ店システム評価)次世代サービスオペレーション効率化に向けた、業務適合性検証 START
////---------------------------------------------------------
////フッターボタン表示制御
////displayType＝C_FT_DISPTP_UNSELECTED：チップ未選択時、C_FT_DISPTP_SELECTED：チップ選択時
////chipDisplayType＝ボタン表示パターン(1～17)
////---------------------------------------------------------
////2014/06/18 TMEJ 丁 タブレット版SMB テレマ走行距離機能開発 START
////function CreateFooterButton(displayType, chipDisplayType) {
//function CreateFooterButton(displayType, chipDisplayType,gMarkFlg) {
//    //2014/06/18 TMEJ 丁 タブレット版SMB テレマ走行距離機能開発 END

//---------------------------------------------------------
//フッターボタン表示制御
//displayType＝C_FT_DISPTP_UNSELECTED：チップ未選択時、C_FT_DISPTP_SELECTED：チップ選択時
//chipDisplayType＝ボタン表示パターン(1～17)
//gMarkFlg＝Gマークボタン表示フラグ(1：表示、1以外：非表示)
//canRestChangeFlg＝休憩変更ボタン表示フラグ（true：表示、false：非表示）
//---------------------------------------------------------
function CreateFooterButton(displayType, chipDisplayType, gMarkFlg, canRestChangeFlg) {
//2019/06/18 NSK 皆川 (トライ店システム評価)次世代サービスオペレーション効率化に向けた、業務適合性検証 END
    
    //ボタンが押されていたら解除する
    if (bkButtonID != "") {
        if (!gShowLaterStallFlg) {
            FooterIconReplace(bkButtonID)
        }
    }
    if (displayType == C_FT_DISPTP_UNSELECTED) {
        $("#MstPG_FootItem_Space_Left").attr("style", "");
        //2013/11/29 TMEJ 張 タブレット版SMB チーフテクニシャン機能開発 START
//        $("#InitFooterArea").attr("style", "");
        if (gButtonStatus == 0) {
            $("#InitFooterArea").attr("style", "");
        } else {
            ShowAplicationButton();
        }
        //2013/11/29 TMEJ 張 タブレット版SMB チーフテクニシャン機能開発 END
        $("#ChipFooterArea").attr("style", "display:none;");
    } else if (displayType == C_FT_DISPTP_SELECTED) {
        var buttonList;
        $("#MstPG_FootItem_Space_Left").attr("style", "");
        //2013/11/29 TMEJ 張 タブレット版SMB チーフテクニシャン機能開発 START
//        $("#InitFooterArea").attr("style", "display:none;");
        if (gButtonStatus == 0) {
            $("#InitFooterArea").attr("style", "display:none;");
        } else {
            HideAplicationButton();
        }
        //2013/11/29 TMEJ 張 タブレット版SMB チーフテクニシャン機能開発 END
        $("#ChipFooterArea").attr("style", "");
        //ボタン情報格納
        // 2013/11/29 TMEJ 丁 タブレット版SMB チーフテクニシャン機能開発 START
//        if (chipDisplayType == C_FT_BTNTP_REZ_KARIKARIREZ) {
//            buttonList = new Array("1", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "1", "0", "0", "0")
//        } else if (chipDisplayType == C_FT_BTNTP_REZ_TEMP) {
//            buttonList = new Array("1", "1", "0", "0", "0", "1", "0", "0", "0", "0", "0", "0", "0", "0", "1", "1", "1", "1", "1", "0")
//        } else if (chipDisplayType == C_FT_BTNTP_REZ_TEMP_CARIN) {
//            buttonList = new Array("1", "1", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "1", "1", "1", "1", "1", "0")
//        } else if (chipDisplayType == C_FT_BTNTP_REZ_DECIDED) {
//            buttonList = new Array("1", "0", "1", "0", "1", "1", "0", "0", "0", "0", "0", "0", "0", "0", "1", "1", "1", "1", "1", "0")
//        } else if (chipDisplayType == C_FT_BTNTP_WALKIN_DECIDED) {
//            // 確定予約(飛び込む客)：予約取消ボタンが表示できない
//            buttonList = new Array("1", "0", "1", "0", "0", "1", "0", "0", "0", "0", "0", "0", "0", "0", "1", "1", "1", "1", "1", "0")
//        } else if (chipDisplayType == C_FT_BTNTP_WALKIN) {
//            buttonList = new Array("1", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "1", "1", "1", "1", "1", "0")
//        } else if (chipDisplayType == C_FT_BTNTP_CONFIRMED_RO || chipDisplayType == C_FT_BTNTP_CONFIRMED_ADDWORK || chipDisplayType == C_FT_BTNTP_INTERRRUPT_BOX) {
//            buttonList = new Array("1", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "1", "1", "0", "1", "1", "0")
//        } else if (chipDisplayType == C_FT_BTNTP_RO_PUBLISHED) {
//            buttonList = new Array("1", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "1", "1", "1", "1", "1", "0")
//        } else if (chipDisplayType == C_FT_BTNTP_DECIDED_WORKPLAN) {
//            buttonList = new Array("1", "0", "0", "0", "0", "0", "1", "0", "0", "0", "0", "0", "0", "0", "1", "1", "1", "1", "1", "0")
//        } else if (chipDisplayType == C_FT_BTNTP_WORKING) {
//            buttonList = new Array("1", "0", "0", "0", "0", "0", "0", "1", "1", "0", "0", "0", "0", "0", "1", "1", "0", "1", "1", "1")
//        } else if (chipDisplayType == C_FT_BTNTP_WORKING_NOTSTARTDAY) {
//            buttonList = new Array("1", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "1", "0", "0", "0", "0", "0")
//        } else if (chipDisplayType == C_FT_BTNTP_INTERRRUPT_STALL) {
//            buttonList = new Array("1", "0", "0", "0", "0", "0", "1", "0", "0", "0", "0", "0", "0", "0", "1", "1", "0", "1", "1", "0")
//        } else if (chipDisplayType == C_FT_BTNTP_END_WORK) {
//            buttonList = new Array("1", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "1", "0", "0", "1", "1", "0")
//        } else if (chipDisplayType == C_FT_BTNTP_END_DELIVERY) {
//            buttonList = new Array("1", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0")
//        } else if (chipDisplayType == C_FT_BTNTP_CONFIRMED_INSPECTION) {
//            buttonList = new Array("1", "0", "0", "0", "0", "0", "0", "0", "0", "1", "0", "0", "0", "0", "1", "0", "0", "1", "1", "0")
//        } else if (chipDisplayType == C_FT_BTNTP_WAIT_CONFIRMEDADDWORK) {
//            buttonList = new Array("1", "0", "0", "0", "0", "0", "0", "0", "0", "0", "1", "0", "0", "0", "1", "0", "0", "1", "1", "0")
//        } else if (chipDisplayType == C_FT_BTNTP_WAITING_WASH) {
//            buttonList = new Array("1", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "1", "0", "0", "1", "0", "0", "1", "1", "0")
//        } else if (chipDisplayType == C_FT_BTNTP_WASHING) {
//            buttonList = new Array("1", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "1", "0", "1", "0", "0", "1", "1", "0")
//        } else if (chipDisplayType == C_FT_BTNTP_WAIT_DELIVERY) {
//            buttonList = new Array("1", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "1", "1", "0", "0", "0", "0", "0")
//        } else if (chipDisplayType == C_FT_BTNTP_CHANGING_DATE) {
//            buttonList = new Array("0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0")
//        } else if (chipDisplayType == C_FT_BTNTP_NOSHOW) {
//            buttonList = new Array("1", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "1", "0", "1", "0", "0", "0")
//        } else if (chipDisplayType == C_FT_BTNTP_STOP) {
//            buttonList = new Array("1", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "1", "0", "0", "0", "0", "0")
//        } else if (chipDisplayType == C_FT_BTNTP_UNAVAILABLE) {
//            buttonList = new Array("0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "1", "0", "0", "0")
//        } else if (chipDisplayType == C_FT_BTNTP_CONFIRMED_RO_AVOIDCOPY) {
//            buttonList = new Array("1", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "1", "0", "1", "1", "0")
//        } else if (chipDisplayType == C_FT_BTNTP_WAIT_CONFIRMEDADDWORK_AVOIDCOPY) {
//            buttonList = new Array("1", "0", "0", "0", "0", "0", "0", "0", "0", "0", "1", "0", "0", "0", "0", "0", "0", "1", "1", "0")
//        } else if (chipDisplayType == C_FT_BTNTP_COPYCHIP) {
//            buttonList = new Array("0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "1", "0", "0", "0")
//        }
//2017/08/30 NSK 竹中(悠) REQ-SVT-TMT-20161109-001 SMB iPadにストールクローズ機能の追加 
//        if (chipDisplayType == C_FT_BTNTP_REZ_KARIKARIREZ) {
//            buttonList = new Array("1", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "1", "0", "0", "0", "0", "0", "0")
//        } else if (chipDisplayType == C_FT_BTNTP_REZ_TEMP) {
//            buttonList = new Array("1", "1", "0", "0", "0", "1", "0", "0", "0", "0", "0", "0", "0", "0", "1", "1", "1", "0", "0", "0", "0", "0", "0")
//        } else if (chipDisplayType == C_FT_BTNTP_REZ_TEMP_CARIN) {
//            buttonList = new Array("1", "1", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "1", "1", "1", "0", "0", "0", "0", "0", "0")
//        } else if (chipDisplayType == C_FT_BTNTP_REZ_DECIDED) {
//            buttonList = new Array("1", "0", "1", "0", "1", "1", "0", "0", "0", "0", "0", "0", "0", "0", "1", "1", "1", "0", "0", "0", "0", "0", "0")
//        } else if (chipDisplayType == C_FT_BTNTP_WALKIN_DECIDED) {
//            // 確定予約(飛び込む客)：予約取消ボタンが表示できない
//            buttonList = new Array("1", "0", "1", "0", "0", "1", "0", "0", "0", "0", "0", "0", "0", "0", "1", "1", "1", "0", "0", "0", "0", "0", "0")
//        } else if (chipDisplayType == C_FT_BTNTP_WALKIN) {
//            buttonList = new Array("1", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "1", "1", "1", "0", "0", "0", "0", "0", "0")
//        } else if (chipDisplayType == C_FT_BTNTP_CONFIRMED_RO || chipDisplayType == C_FT_BTNTP_CONFIRMED_ADDWORK || chipDisplayType == C_FT_BTNTP_INTERRRUPT_BOX) {
//            buttonList = new Array("1", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "1", "1", "0", "0", "0", "0", "0", "0", "0")
//        } else if (chipDisplayType == C_FT_BTNTP_RO_PUBLISHED) {
//            buttonList = new Array("1", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "1", "1", "1", "0", "0", "0", "0", "0", "0")
//        } else if (chipDisplayType == C_FT_BTNTP_DECIDED_WORKPLAN) {
//            buttonList = new Array("1", "0", "0", "0", "0", "0", "1", "0", "0", "0", "0", "0", "0", "0", "1", "1", "1", "0", "0", "0", "0", "0", "0")
//        } else if (chipDisplayType == C_FT_BTNTP_WORKING) {
//            //2014/12/08 TMEJ 丁 DMS連携版サービスタブレット JobDispatch完成検査入力制御開発 START
//            //buttonList = new Array("1", "0", "0", "0", "0", "0", "0", "1", "1", "0", "0", "0", "0", "0", "1", "1", "0", "1", "1", "0", "0", "0", "0")
//            buttonList = new Array("1", "0", "0", "0", "0", "0", "0", "0", "1", "0", "0", "0", "0", "0", "1", "1", "0", "1", "1", "0", "0", "0", "0")
//            //2014/12/08 TMEJ 丁 DMS連携版サービスタブレット JobDispatch完成検査入力制御開発 END
//        } else if (chipDisplayType == C_FT_BTNTP_WORKING_NOTSTARTDAY) {
//            buttonList = new Array("1", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "1", "0", "0", "0", "0", "0", "0", "0", "0")
//        } else if (chipDisplayType == C_FT_BTNTP_INTERRRUPT_STALL) {
//            
//            //2015/03/17 TMEJ 明瀬 既存バグ修正(中断再配置チップタップ時に削除ボタンが表示されない) START
//            //buttonList = new Array("1", "0", "0", "0", "0", "0", "1", "0", "0", "0", "0", "0", "0", "0", "1", "1", "0", "0", "0", "0", "0", "0", "0")
//            buttonList = new Array("1", "0", "0", "0", "0", "0", "1", "0", "0", "0", "0", "0", "0", "0", "1", "1", "1", "0", "0", "0", "0", "0", "0")
//            //2015/03/17 TMEJ 明瀬 既存バグ修正(中断再配置チップタップ時に削除ボタンが表示されない) END

//        } else if (chipDisplayType == C_FT_BTNTP_END_WORK) {
//            buttonList = new Array("1", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "1", "0", "0", "0", "0", "0", "0", "0", "0")
//        } else if (chipDisplayType == C_FT_BTNTP_END_DELIVERY) {
//            buttonList = new Array("1", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0")
//        } else if (chipDisplayType == C_FT_BTNTP_CONFIRMED_INSPECTION) {
//            buttonList = new Array("1", "0", "0", "0", "0", "0", "0", "0", "0", "1", "0", "0", "0", "0", "1", "0", "0", "0", "0", "0", "0", "0", "0")
//        } else if (chipDisplayType == C_FT_BTNTP_WAIT_CONFIRMEDADDWORK) {
//            buttonList = new Array("1", "0", "0", "0", "0", "0", "0", "0", "0", "0", "1", "0", "0", "0", "1", "0", "0", "0", "0", "0", "0", "0", "0")
//        } else if (chipDisplayType == C_FT_BTNTP_WAITING_WASH) {
//            buttonList = new Array("1", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "1", "0", "0", "1", "0", "0", "0", "0", "0", "0", "0", "1")
//        } else if (chipDisplayType == C_FT_BTNTP_WASHING) {
//            buttonList = new Array("1", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "1", "0", "1", "0", "0", "1", "0", "0", "0", "0", "0")
//        } else if (chipDisplayType == C_FT_BTNTP_WAIT_DELIVERY) {
//            buttonList = new Array("1", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "1", "1", "0", "0", "0", "0", "0", "0", "0", "0")
//        } else if (chipDisplayType == C_FT_BTNTP_WAIT_DELIVERY_WASH) {
//            buttonList = new Array("1", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "1", "1", "0", "0", "0", "0", "0", "0", "1", "0")
//        } else if (chipDisplayType == C_FT_BTNTP_CHANGING_DATE) {
//            buttonList = new Array("0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0")
//        } else if (chipDisplayType == C_FT_BTNTP_NOSHOW) {
//            buttonList = new Array("1", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "1", "0", "1", "0", "0", "0", "0", "0", "0")
//        } else if (chipDisplayType == C_FT_BTNTP_STOP) {
//            buttonList = new Array("1", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "1", "0", "0", "0", "0", "0", "0", "0", "0")
//        } else if (chipDisplayType == C_FT_BTNTP_UNAVAILABLE) {
//            buttonList = new Array("0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "1", "0", "0", "0", "0", "0", "0")
//        } else if (chipDisplayType == C_FT_BTNTP_CONFIRMED_RO_AVOIDCOPY) {
//            buttonList = new Array("1", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "1", "0", "0", "0", "0", "0", "0", "0")
//        } else if (chipDisplayType == C_FT_BTNTP_WAIT_CONFIRMEDADDWORK_AVOIDCOPY) {
//            buttonList = new Array("1", "0", "0", "0", "0", "0", "0", "0", "0", "0", "1", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0")
//        } else if (chipDisplayType == C_FT_BTNTP_COPYCHIP) {
//            buttonList = new Array("0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "1", "0", "0", "0", "0", "0", "0")
//        } else if (chipDisplayType == C_FT_BTNTP_REZ_NEW) {
//            buttonList = new Array("1", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "1", "0", "0", "0", "0", "0", "0")
        //        }
        // 2013/11/29 TMEJ 丁 タブレット版SMB チーフテクニシャン機能開発 END

// 2017/09/05 NSK 小川 REQ-SVT-TMT-20160906-003 計画済みチップをストールから簡単に外せるようにする START
//        if (chipDisplayType == C_FT_BTNTP_REZ_KARIKARIREZ) {
//            buttonList = new Array("1", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "1", "0", "0", "0", "0", "0", "0", "0")
//        } else if (chipDisplayType == C_FT_BTNTP_REZ_TEMP) {
//            buttonList = new Array("1", "1", "0", "0", "0", "1", "0", "0", "0", "0", "0", "0", "0", "0", "1", "1", "1", "0", "0", "0", "0", "0", "0", "0")
//        } else if (chipDisplayType == C_FT_BTNTP_REZ_TEMP_CARIN) {
//            buttonList = new Array("1", "1", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "1", "1", "1", "0", "0", "0", "0", "0", "0", "0")
//        } else if (chipDisplayType == C_FT_BTNTP_REZ_DECIDED) {
//            buttonList = new Array("1", "0", "1", "0", "1", "1", "0", "0", "0", "0", "0", "0", "0", "0", "1", "1", "1", "0", "0", "0", "0", "0", "0", "0")
//        } else if (chipDisplayType == C_FT_BTNTP_WALKIN_DECIDED) {
//            // 確定予約(飛び込む客)：予約取消ボタンが表示できない
//            buttonList = new Array("1", "0", "1", "0", "0", "1", "0", "0", "0", "0", "0", "0", "0", "0", "1", "1", "1", "0", "0", "0", "0", "0", "0", "0")
//        } else if (chipDisplayType == C_FT_BTNTP_WALKIN) {
//            buttonList = new Array("1", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "1", "1", "1", "0", "0", "0", "0", "0", "0", "0")
//        } else if (chipDisplayType == C_FT_BTNTP_CONFIRMED_RO || chipDisplayType == C_FT_BTNTP_CONFIRMED_ADDWORK || chipDisplayType == C_FT_BTNTP_INTERRRUPT_BOX) {
//            buttonList = new Array("1", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "1", "1", "0", "0", "0", "0", "0", "0", "0", "0")
//        } else if (chipDisplayType == C_FT_BTNTP_RO_PUBLISHED) {
//            buttonList = new Array("1", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "1", "1", "1", "0", "0", "0", "0", "0", "0", "0")
//        } else if (chipDisplayType == C_FT_BTNTP_DECIDED_WORKPLAN) {
//            buttonList = new Array("1", "0", "0", "0", "0", "0", "1", "0", "0", "0", "0", "0", "0", "0", "1", "1", "1", "0", "0", "0", "0", "0", "0", "0")
//        } else if (chipDisplayType == C_FT_BTNTP_WORKING) {
//            buttonList = new Array("1", "0", "0", "0", "0", "0", "0", "0", "1", "0", "0", "0", "0", "0", "1", "1", "0", "1", "1", "0", "0", "0", "0", "0")
//        } else if (chipDisplayType == C_FT_BTNTP_WORKING_NOTSTARTDAY) {
//            buttonList = new Array("1", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "1", "0", "0", "0", "0", "0", "0", "0", "0", "0")
//        } else if (chipDisplayType == C_FT_BTNTP_INTERRRUPT_STALL) {
//            buttonList = new Array("1", "0", "0", "0", "0", "0", "1", "0", "0", "0", "0", "0", "0", "0", "1", "1", "1", "0", "0", "0", "0", "0", "0", "0")
//        } else if (chipDisplayType == C_FT_BTNTP_END_WORK) {
//            buttonList = new Array("1", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "1", "0", "0", "0", "0", "0", "0", "0", "0", "0")
//        } else if (chipDisplayType == C_FT_BTNTP_END_DELIVERY) {
//            buttonList = new Array("1", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0")
//        } else if (chipDisplayType == C_FT_BTNTP_CONFIRMED_INSPECTION) {
//            buttonList = new Array("1", "0", "0", "0", "0", "0", "0", "0", "0", "1", "0", "0", "0", "0", "1", "0", "0", "0", "0", "0", "0", "0", "0", "0")
//        } else if (chipDisplayType == C_FT_BTNTP_WAIT_CONFIRMEDADDWORK) {
//            buttonList = new Array("1", "0", "0", "0", "0", "0", "0", "0", "0", "0", "1", "0", "0", "0", "1", "0", "0", "0", "0", "0", "0", "0", "0", "0")
//        } else if (chipDisplayType == C_FT_BTNTP_WAITING_WASH) {
//            buttonList = new Array("1", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "1", "0", "0", "1", "0", "0", "0", "0", "0", "0", "0", "1", "0")
//        } else if (chipDisplayType == C_FT_BTNTP_WASHING) {
//            buttonList = new Array("1", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "1", "0", "1", "0", "0", "1", "0", "0", "0", "0", "0", "0")
//        } else if (chipDisplayType == C_FT_BTNTP_WAIT_DELIVERY) {
//            buttonList = new Array("1", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "1", "1", "0", "0", "0", "0", "0", "0", "0", "0", "0")
//        } else if (chipDisplayType == C_FT_BTNTP_WAIT_DELIVERY_WASH) {
//            buttonList = new Array("1", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "1", "1", "0", "0", "0", "0", "0", "0", "1", "0", "0")
//        } else if (chipDisplayType == C_FT_BTNTP_CHANGING_DATE) {
//            buttonList = new Array("0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0")
//        } else if (chipDisplayType == C_FT_BTNTP_NOSHOW) {
//            buttonList = new Array("1", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "1", "0", "1", "0", "0", "0", "0", "0", "0", "0")
//        } else if (chipDisplayType == C_FT_BTNTP_STOP) {
//            buttonList = new Array("1", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "1", "0", "0", "0", "0", "0", "0", "0", "0", "0")
//        } else if (chipDisplayType == C_FT_BTNTP_UNAVAILABLE) {
//            buttonList = new Array("1", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "1", "0", "0", "0", "0", "0", "0", "0")
//        } else if (chipDisplayType == C_FT_BTNTP_CONFIRMED_RO_AVOIDCOPY) {
//            buttonList = new Array("1", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "1", "0", "0", "0", "0", "0", "0", "0", "0")
//        } else if (chipDisplayType == C_FT_BTNTP_WAIT_CONFIRMEDADDWORK_AVOIDCOPY) {
//            buttonList = new Array("1", "0", "0", "0", "0", "0", "0", "0", "0", "0", "1", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0")
//        } else if (chipDisplayType == C_FT_BTNTP_COPYCHIP) {
//            buttonList = new Array("0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "1", "0", "0", "0", "0", "0", "0", "0")
//        } else if (chipDisplayType == C_FT_BTNTP_REZ_NEW) {
//            buttonList = new Array("1", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "1", "0", "0", "0", "0", "0", "0", "1")
//        }
//        //2017/08/30 NSK 竹中(悠) REQ-SVT-TMT-20161109-001 SMB iPadにストールクローズ機能の追加 END      

        // 2019/06/18 NSK 皆川 (トライ店システム評価)次世代サービスオペレーション効率化に向けた、業務適合性検証 START
//        if (chipDisplayType == C_FT_BTNTP_REZ_KARIKARIREZ) {
//            buttonList = new Array("1", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "1", "0", "0", "0", "0", "0", "0", "0", "0")
//        } else if (chipDisplayType == C_FT_BTNTP_REZ_TEMP) {
//            buttonList = new Array("1", "1", "0", "0", "0", "1", "0", "0", "0", "0", "0", "0", "0", "0", "1", "1", "1", "0", "0", "0", "0", "0", "0", "0", "0")
//        } else if (chipDisplayType == C_FT_BTNTP_REZ_TEMP_CARIN) {
//            buttonList = new Array("1", "1", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "1", "1", "0", "0", "0", "0", "0", "0", "0", "0", "1")
//        } else if (chipDisplayType == C_FT_BTNTP_REZ_DECIDED) {
//            buttonList = new Array("1", "0", "1", "0", "1", "1", "0", "0", "0", "0", "0", "0", "0", "0", "1", "1", "1", "0", "0", "0", "0", "0", "0", "0", "0")
//        } else if (chipDisplayType == C_FT_BTNTP_WALKIN_DECIDED) {
//            // 確定予約(飛び込む客)：予約取消ボタンが表示できない
//            buttonList = new Array("1", "0", "1", "0", "0", "1", "0", "0", "0", "0", "0", "0", "0", "0", "1", "1", "1", "0", "0", "0", "0", "0", "0", "0", "0")
//        } else if (chipDisplayType == C_FT_BTNTP_WALKIN) {
//            buttonList = new Array("1", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "1", "1", "0", "0", "0", "0", "0", "0", "0", "0", "1")
//        } else if (chipDisplayType == C_FT_BTNTP_CONFIRMED_RO || chipDisplayType == C_FT_BTNTP_CONFIRMED_ADDWORK || chipDisplayType == C_FT_BTNTP_INTERRRUPT_BOX) {
//            buttonList = new Array("1", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "1", "1", "0", "0", "0", "0", "0", "0", "0", "0", "0")
//        } else if (chipDisplayType == C_FT_BTNTP_RO_PUBLISHED) {
//            buttonList = new Array("1", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "1", "1", "0", "0", "0", "0", "0", "0", "0", "0", "1")
//        } else if (chipDisplayType == C_FT_BTNTP_DECIDED_WORKPLAN) {
//            buttonList = new Array("1", "0", "0", "0", "0", "0", "1", "0", "0", "0", "0", "0", "0", "0", "1", "1", "0", "0", "0", "0", "0", "0", "0", "0", "1")
//        } else if (chipDisplayType == C_FT_BTNTP_WORKING) {
//            buttonList = new Array("1", "0", "0", "0", "0", "0", "0", "0", "1", "0", "0", "0", "0", "0", "1", "1", "0", "1", "1", "0", "0", "0", "0", "0", "0")
//        } else if (chipDisplayType == C_FT_BTNTP_WORKING_NOTSTARTDAY) {
//            buttonList = new Array("1", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "1", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0")
//        } else if (chipDisplayType == C_FT_BTNTP_INTERRRUPT_STALL) {
//            buttonList = new Array("1", "0", "0", "0", "0", "0", "1", "0", "0", "0", "0", "0", "0", "0", "1", "1", "0", "0", "0", "0", "0", "0", "0", "0", "1")
//        } else if (chipDisplayType == C_FT_BTNTP_END_WORK) {
//            buttonList = new Array("1", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "1", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0")
//        } else if (chipDisplayType == C_FT_BTNTP_END_DELIVERY) {
//            buttonList = new Array("1", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0")
//        } else if (chipDisplayType == C_FT_BTNTP_CONFIRMED_INSPECTION) {
//            buttonList = new Array("1", "0", "0", "0", "0", "0", "0", "0", "0", "1", "0", "0", "0", "0", "1", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0")
//        } else if (chipDisplayType == C_FT_BTNTP_WAIT_CONFIRMEDADDWORK) {
//            buttonList = new Array("1", "0", "0", "0", "0", "0", "0", "0", "0", "0", "1", "0", "0", "0", "1", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0")
//        } else if (chipDisplayType == C_FT_BTNTP_WAITING_WASH) {
//            buttonList = new Array("1", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "1", "0", "0", "1", "0", "0", "0", "0", "0", "0", "0", "1", "0", "0")
//        } else if (chipDisplayType == C_FT_BTNTP_WASHING) {
//            buttonList = new Array("1", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "1", "0", "1", "0", "0", "1", "0", "0", "0", "0", "0", "0", "0")
//        } else if (chipDisplayType == C_FT_BTNTP_WAIT_DELIVERY) {
//            buttonList = new Array("1", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "1", "1", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0")
//        } else if (chipDisplayType == C_FT_BTNTP_WAIT_DELIVERY_WASH) {
//            buttonList = new Array("1", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "1", "1", "0", "0", "0", "0", "0", "0", "1", "0", "0", "0")
//        } else if (chipDisplayType == C_FT_BTNTP_CHANGING_DATE) {
//            buttonList = new Array("0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0")
//        } else if (chipDisplayType == C_FT_BTNTP_NOSHOW) {
//            buttonList = new Array("1", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "1", "0", "1", "0", "0", "0", "0", "0", "0", "0", "0")
//        } else if (chipDisplayType == C_FT_BTNTP_STOP) {
//            buttonList = new Array("1", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "1", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0")
//        } else if (chipDisplayType == C_FT_BTNTP_UNAVAILABLE) {
//            buttonList = new Array("1", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "1", "0", "0", "0", "0", "0", "0", "0", "0")
//        } else if (chipDisplayType == C_FT_BTNTP_CONFIRMED_RO_AVOIDCOPY) {
//            buttonList = new Array("1", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "1", "0", "0", "0", "0", "0", "0", "0", "0", "0")
//        } else if (chipDisplayType == C_FT_BTNTP_WAIT_CONFIRMEDADDWORK_AVOIDCOPY) {
//            buttonList = new Array("1", "0", "0", "0", "0", "0", "0", "0", "0", "0", "1", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0")
//        } else if (chipDisplayType == C_FT_BTNTP_COPYCHIP) {
//            buttonList = new Array("0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "1", "0", "0", "0", "0", "0", "0", "0", "0")
//        } else if (chipDisplayType == C_FT_BTNTP_REZ_NEW) {
//            buttonList = new Array("1", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "1", "0", "0", "0", "0", "0", "0", "1", "0")
//        }
//        // 2017/09/05 NSK 小川 REQ-SVT-TMT-20160906-003 計画済みチップをストールから簡単に外せるようにする END

        if (chipDisplayType == C_FT_BTNTP_REZ_KARIKARIREZ) {
            buttonList = new Array("1", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "1", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0")
        } else if (chipDisplayType == C_FT_BTNTP_REZ_TEMP) {
            buttonList = new Array("1", "1", "0", "0", "0", "1", "0", "0", "0", "0", "0", "0", "0", "0", "1", "1", "1", "0", "0", "0", "0", "0", "0", "0", "0", "1", "1")
        } else if (chipDisplayType == C_FT_BTNTP_REZ_TEMP_CARIN) {
            buttonList = new Array("1", "1", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "1", "1", "0", "0", "0", "0", "0", "0", "0", "0", "1", "1", "1")
        } else if (chipDisplayType == C_FT_BTNTP_REZ_DECIDED) {
            buttonList = new Array("1", "0", "1", "0", "1", "1", "0", "0", "0", "0", "0", "0", "0", "0", "1", "1", "1", "0", "0", "0", "0", "0", "0", "0", "0", "1", "1")
        } else if (chipDisplayType == C_FT_BTNTP_WALKIN_DECIDED) {
            // 確定予約(飛び込む客)：予約取消ボタンが表示できない
            buttonList = new Array("1", "0", "1", "0", "0", "1", "0", "0", "0", "0", "0", "0", "0", "0", "1", "1", "1", "0", "0", "0", "0", "0", "0", "0", "0", "1", "1")
        } else if (chipDisplayType == C_FT_BTNTP_WALKIN) {
            buttonList = new Array("1", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "1", "1", "0", "0", "0", "0", "0", "0", "0", "0", "1", "1", "1")
        } else if (chipDisplayType == C_FT_BTNTP_CONFIRMED_RO || chipDisplayType == C_FT_BTNTP_CONFIRMED_ADDWORK || chipDisplayType == C_FT_BTNTP_INTERRRUPT_BOX) {
            buttonList = new Array("1", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "1", "1", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0")
        } else if (chipDisplayType == C_FT_BTNTP_RO_PUBLISHED) {
            buttonList = new Array("1", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "1", "1", "0", "0", "0", "0", "0", "0", "0", "0", "1", "1", "1")
        } else if (chipDisplayType == C_FT_BTNTP_DECIDED_WORKPLAN) {
            buttonList = new Array("1", "0", "0", "0", "0", "0", "1", "0", "0", "0", "0", "0", "0", "0", "1", "1", "0", "0", "0", "0", "0", "0", "0", "0", "1", "1", "1")
        } else if (chipDisplayType == C_FT_BTNTP_WORKING) {
            buttonList = new Array("1", "0", "0", "0", "0", "0", "0", "0", "1", "0", "0", "0", "0", "0", "1", "1", "0", "1", "1", "0", "0", "0", "0", "0", "0", "1", "1")
        } else if (chipDisplayType == C_FT_BTNTP_WORKING_NOTSTARTDAY) {
            buttonList = new Array("1", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "1", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0")
        } else if (chipDisplayType == C_FT_BTNTP_INTERRRUPT_STALL) {
            buttonList = new Array("1", "0", "0", "0", "0", "0", "1", "0", "0", "0", "0", "0", "0", "0", "1", "1", "0", "0", "0", "0", "0", "0", "0", "0", "1", "1", "1")
        } else if (chipDisplayType == C_FT_BTNTP_END_WORK) {
            buttonList = new Array("1", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "1", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0")
        } else if (chipDisplayType == C_FT_BTNTP_END_DELIVERY) {
            buttonList = new Array("1", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0")
        } else if (chipDisplayType == C_FT_BTNTP_CONFIRMED_INSPECTION) {
            buttonList = new Array("1", "0", "0", "0", "0", "0", "0", "0", "0", "1", "0", "0", "0", "0", "1", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0")
        } else if (chipDisplayType == C_FT_BTNTP_WAIT_CONFIRMEDADDWORK) {
            buttonList = new Array("1", "0", "0", "0", "0", "0", "0", "0", "0", "0", "1", "0", "0", "0", "1", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0")
        } else if (chipDisplayType == C_FT_BTNTP_WAITING_WASH) {
            buttonList = new Array("1", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "1", "0", "0", "1", "0", "0", "0", "0", "0", "0", "0", "1", "0", "0", "0", "0")
        } else if (chipDisplayType == C_FT_BTNTP_WASHING) {
            buttonList = new Array("1", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "1", "0", "1", "0", "0", "1", "0", "0", "0", "0", "0", "0", "0", "0", "0")
        } else if (chipDisplayType == C_FT_BTNTP_WAIT_DELIVERY) {
            buttonList = new Array("1", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "1", "1", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0")
        } else if (chipDisplayType == C_FT_BTNTP_WAIT_DELIVERY_WASH) {
            buttonList = new Array("1", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "1", "1", "0", "0", "0", "0", "0", "0", "1", "0", "0", "0", "0", "0")
        } else if (chipDisplayType == C_FT_BTNTP_CHANGING_DATE) {
            buttonList = new Array("0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0")
        } else if (chipDisplayType == C_FT_BTNTP_NOSHOW) {
            buttonList = new Array("1", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "1", "0", "1", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0")
        } else if (chipDisplayType == C_FT_BTNTP_STOP) {
            buttonList = new Array("1", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "1", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0")
        } else if (chipDisplayType == C_FT_BTNTP_UNAVAILABLE) {
            buttonList = new Array("1", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "1", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0")
        } else if (chipDisplayType == C_FT_BTNTP_CONFIRMED_RO_AVOIDCOPY) {
            buttonList = new Array("1", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "1", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0")
        } else if (chipDisplayType == C_FT_BTNTP_WAIT_CONFIRMEDADDWORK_AVOIDCOPY) {
            buttonList = new Array("1", "0", "0", "0", "0", "0", "0", "0", "0", "0", "1", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0")
        } else if (chipDisplayType == C_FT_BTNTP_COPYCHIP) {
            buttonList = new Array("0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "1", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0")
        } else if (chipDisplayType == C_FT_BTNTP_REZ_NEW) {
            buttonList = new Array("1", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "1", "0", "0", "0", "0", "0", "0", "1", "0", "0", "0")
        }

        // 2019/06/18 NSK 皆川 (トライ店システム評価)次世代サービスオペレーション効率化に向けた、業務適合性検証 END

        // 入庫ボタンが表示される時
        if (buttonList[2] == "1") {
            // 未来店客の場合(リレーションチップがNOSHOWエリアにあれば、入庫ボタンが表示されない)
            if (gArrObjChip[gSelectedChipId].svcStatus == "01") {
                buttonList[2] = "0";
            }
        }

        // コピーボタンが表示される時
        if (buttonList[14] == "1") {
            // 洗車中の場合、コピーできないので、表示しない
            if (gArrObjChip[gSelectedChipId]) {
                if (gArrObjChip[gSelectedChipId].svcStatus == C_SVCSTATUS_CARWASHSTART) {
                    buttonList[14] = "0";
                }
            } else if (gArrObjSubChip[gSelectedChipId]) {
                if (gArrObjSubChip[gSelectedChipId].svcStatus == C_SVCSTATUS_CARWASHSTART) {
                    buttonList[14] = "0";
                }
            }
        }

        // 2013/11/29 TMEJ 張 タブレット版SMB チーフテクニシャン機能開発 START
        if (buttonList[18] == "1") {
            // 緑チップの予定終了時刻が今日営業時刻を超えない場合、
            if (gArrObjChip[gSelectedChipId].prmsEndDateTime - gEndWorkTime <= 0) {
                // 日跨ぎ終了ボタンを表示しない
                buttonList[18] = "0";
            }
        }
        // 2013/11/29 TMEJ 張 タブレット版SMB チーフテクニシャン機能開発 END

        // 2017/09/05 NSK 小川 REQ-SVT-TMT-20160906-003 計画済みチップをストールから簡単に外せるようにする START
        // 計画取り消しボタンが表示されるとき
        if (buttonList[24] == "1") {
            // チップにROが発行されていない場合
            if (gArrObjChip[gSelectedChipId].roNum == "") {
                // 計画取消ボタンを表示しない
                buttonList[24] = "0";
            }
        }
        // 2017/09/05 NSK 小川 REQ-SVT-TMT-20160906-003 計画済みチップをストールから簡単に外せるようにする END


        // 2017/10/21 NSK 小川 REQ-SVT-TMT-20160906-003 子チップがキャンセルできない START
        // 関連チップがあり、入庫済の場合
        if (gArrObjRelationChip[gSelectedChipId] && (chipDisplayType == C_FT_BTNTP_REZ_TEMP_CARIN || chipDisplayType == C_FT_BTNTP_WALKIN || chipDisplayType == C_FT_BTNTP_RO_PUBLISHED || chipDisplayType == C_FT_BTNTP_DECIDED_WORKPLAN || chipDisplayType == C_FT_BTNTP_INTERRRUPT_STALL)) {
            //対象チップjobDtlId
            var selectedChipId = gArrObjChip[gSelectedChipId].jobDtlId
            // すべての関連チップをループして、チップを探す
            for (var chipId in gArrObjRelationChip) {
                if (gArrObjRelationChip[chipId]) {
                    //サービス入庫単位チップを検索
                    if (gArrObjRelationChip[chipId].svcinId == gArrObjChip[gSelectedChipId].svcInId) {
                        // 対象チップの作業内容IDが最小でない場合、削除ボタンを表示
                        if (Number(gArrObjRelationChip[chipId].jobDtlId) < Number(selectedChipId)) {
                            buttonList[16] = "1";
                            break;
                        }
                    }
                }
            }
        }
        // 2017/10/21 NSK 小川 REQ-SVT-TMT-20160906-003 子チップがキャンセルできない END

        // 2019/06/18 NSK 皆川 (トライ店システム評価)次世代サービスオペレーション効率化に向けた、業務適合性検証 START

        // 2019/12/06 NSK 皆川 (トライ店システム評価)整備作業における休憩時間取得判定向上検証 START
//        // 休憩取得変更ボタンを表示する場合
//        if (canRestChangeFlg) {
        // 休憩を自動判定するかつ休憩取得変更ボタンを表示する場合
        if ($("#hidRestAutoJudgeFlg").val() == "1" && canRestChangeFlg) {
            // 2019/12/06 NSK 皆川 (トライ店システム評価)整備作業における休憩時間取得判定向上検証 END

            // 休憩なしボタンの表示判定
            if (buttonList[25] == "1") {
                // 対象チップの休憩取得フラグが休憩なしの場合ボタンを非表示
                if (gArrObjChip[gSelectedChipId].restFlg == C_RESTTIMEGETFLG_NOGETREST) {
                    buttonList[25] = "0";
                }
            }

            // 休憩ありボタンの表示判定
            if (buttonList[26] == "1") {
                // 対象チップの休憩取得フラグが休憩ありの場合ボタンを非表示
                if (gArrObjChip[gSelectedChipId].restFlg == C_RESTTIMEGETFLG_GETREST) {
                    buttonList[26] = "0";
                }
            }
        } else {
            buttonList[25] = "0";
            buttonList[26] = "0";
        }
        // 2019/06/18 NSK 皆川 (トライ店システム評価)次世代サービスオペレーション効率化に向けた、業務適合性検証 END

        //ボタンの表示文字列格納
        for (var index = 0; index < buttonList.length; index++) {
            if (buttonList[index] == 0) {
                buttonList[index] = "display:none;";
            } else {
                buttonList[index] = "";
            }
        }

        // 2013/11/29 TMEJ 張 タブレット版SMB チーフテクニシャン機能開発 START
        // demo版：redoメニューを非表示にする
//        buttonList[18] = "display:none;";
        // 2013/11/29 TMEJ 張 タブレット版SMB チーフテクニシャン機能開発 END

        //ボタン表示設定
        $("#FooterButton900").attr("style", buttonList[0]);     //詳細ボタン
        $("#FooterButton1000").attr("style", buttonList[1]);    //予約確定ボタン
        $("#FooterButton1100").attr("style", buttonList[2]);    //入庫ボタン
        $("#FooterButton1200").attr("style", buttonList[3]);    //入庫取消ボタン
        $("#FooterButton1300").attr("style", buttonList[4]);    //予約確定取消ボタン
        $("#FooterButton1400").attr("style", buttonList[5]);    //NO SHOWボタン
        $("#FooterButton1500").attr("style", buttonList[6]);    //開始ボタン
        $("#FooterButton1600").attr("style", buttonList[7]);    //終了ボタン
        $("#FooterButton1700").attr("style", buttonList[8]);    //中断ボタン
        $("#FooterButton1800").attr("style", buttonList[9]);    //完成承認ボタン
        $("#FooterButton1900").attr("style", buttonList[10]);   //追加承認ボタン
        $("#FooterButton2000").attr("style", buttonList[11]);   //洗車開始ボタン
        $("#FooterButton2100").attr("style", buttonList[12]);   //洗車終了ボタン
        $("#FooterButton2200").attr("style", buttonList[13]);   //納車ボタン
        $("#FooterButton2300").attr("style", buttonList[14]);   //コピーボタン
        $("#FooterButton2400").attr("style", buttonList[15]);   //再計画ボタン
        $("#FooterButton2500").attr("style", buttonList[16]);   //削除ボタン
        $("#FooterButton2600").attr("style", buttonList[17]);   //UNDOボタン
        // 2013/11/29 TMEJ 丁 タブレット版SMB チーフテクニシャン機能開発 START
//        $("#FooterButton2700").attr("style", buttonList[18]);   //REDOボタン
//        $("#FooterButton2800").attr("style", buttonList[19]);   //日跨ぎ終了ボタン
        $("#FooterButton2700").attr("style", buttonList[18]);   //日跨ぎ終了ボタン
        $("#FooterButton2800").attr("style", buttonList[19]);   //REDOボタン
        $("#FooterButton2900").attr("style", buttonList[21]);   //洗車へ移動ボタン
        $("#FooterButton3000").attr("style", buttonList[22]);   //納車へ移動ボタン
        // 2013/11/29 TMEJ 丁 タブレット版SMB チーフテクニシャン機能開発 END

        //2017/08/30 NSK 竹中(悠) REQ-SVT-TMT-20161109-001 SMB iPadにストールクローズ機能の追加 START
        $("#FooterButton3300").attr("style", buttonList[23]);   //ストール使用不可ボタン
        //2017/08/30 NSK 竹中(悠) REQ-SVT-TMT-20161109-001 SMB iPadにストールクローズ機能の追加 END

        // 2017/09/05 NSK 小川 REQ-SVT-TMT-20160906-003 計画済みチップをストールから簡単に外せるようにする START
        $("#FooterButton3400").attr("style", buttonList[24]);   //計画取り消しボタン
        // 2017/09/05 NSK 小川 REQ-SVT-TMT-20160906-003 計画済みチップをストールから簡単に外せるようにする END

        //2014/06/18 TMEJ 丁 タブレット版SMB テレマ走行距離機能開発 START
        if (gMarkFlg == "1") {
            $("#FooterButton3100").attr("style", "display:block;");   //Gマークボタン
        } else {
            $("#FooterButton3100").attr("style", "display:none;");
        }
        //2014/06/18 TMEJ 丁 タブレット版SMB テレマ走行距離機能開発 START

        // 2019/06/18 NSK 皆川 (トライ店システム評価)次世代サービスオペレーション効率化に向けた、業務適合性検証 START
        $("#FooterButton3500").attr("style", buttonList[25]);
        $("#FooterButton3600").attr("style", buttonList[26]);
        // 2019/06/18 NSK 皆川 (トライ店システム評価)次世代サービスオペレーション効率化に向けた、業務適合性検証 END

        //2015/04/16 TMEJ 明瀬 次工程へ移動ボタン蓋締め START

//        //2015/02/13 TMEJ 範 新プラットフォーム版i-CROP仕様変更対応 (SMBチップ検索及び作業中断機能強化) START
//        
//        //中断終了ボタン非表示
//        $("#FooterButton3200").attr("style", "display:none;");
//                
//        // 中断サブエリアにチップを選択する時
//        if (chipDisplayType == C_FT_BTNTP_STOP) {

//            // サブチップが選択中の場合
//            if (gArrObjSubChip[gSelectedChipId]) {

//                // 残り未完了作業が1つの場合
//                if (gArrObjSubChip[gSelectedChipId].notFinishedCount == 1) {

//                    //中断終了ボタン表示
//                    $("#FooterButton3200").attr("style", "display:block;");

//                }
//            }
//        }

//        //2015/02/13 TMEJ 範 新プラットフォーム版i-CROP仕様変更対応 (SMBチップ検索及び作業中断機能強化) END

        //2015/04/16 TMEJ 明瀬 次工程へ移動ボタン蓋締め END

        //2014/07/15 TMEJ 張 タブレットSMB Job Dispatch機能開発 START
//        // 2014/07/14 TMEJ 張 タブレット版SMB 作業終了ボタン蓋締め START
//        $("#FooterButton1600").attr("style", "display:none;");
//        // 2014/07/14 TMEJ 張 タブレット版SMB 作業終了ボタン蓋締め END
        //2014/07/15 TMEJ 張 タブレットSMB Job Dispatch機能開発 END
    } else {
        //指定しなかった場合は表示しない
        $("#MstPG_FootItem_Space_Left").attr("style", "display:none;");
        $("#InitFooterArea").attr("style", "display:none;");
        $("#ChipFooterArea").attr("style", "display:none;");
    }

    // 今日以外のページの場合
    if (IsTodayPage() == false) {
        // 開始ボタンを表示しない
        FooterIconDisplay(C_FT_BTNID_START, C_FT_BTNDISP_OFF);
    }

    // 選択したチップが他の日付からのチップの場合、詳細ボタンが表示されない
    if (gSelectedChipId == C_OTHERDTCHIPID) {
        if (gOtherDtChipObj.KEY) {
            // 詳細ボタンが表示されない(サブチップの場合)
            FooterIconDisplay(C_FT_BTNID_DETAIL, C_FT_BTNDISP_OFF);
        } else {
            // 表示される日付が当ページではない場合、詳細ボタンが表示されない
            var dtShowDate = new Date($("#hidShowDate").val());
            var nCompareValue = CompareDate(gOtherDtChipObj.displayStartDate, dtShowDate);
            if (nCompareValue != 0) {
                FooterIconDisplay(C_FT_BTNID_DETAIL, C_FT_BTNDISP_OFF);
            }
        }
    }

}

//---------------------------------------------------------
//フッターアイコン表示制御
//buttonID＝100～2700
//diplayOnOff＝C_FT_BTNDISP_OFF：表示しない、C_FT_BTNDISP_ON：表示する
//---------------------------------------------------------
function FooterIconDisplay(buttonID, diplayOnOff) {
    if (diplayOnOff == C_FT_BTNDISP_ON) {
        $("#FooterButton" + buttonID).css("display", "");
    } else if (diplayOnOff == C_FT_BTNDISP_OFF) {
        $("#FooterButton" + buttonID).css("display", "none");
    }
}
//---------------------------------------------------------
//フッターアイコン変更
//buttonID＝100～2700
//buttonType＝C_FT_BTNCLR_NOCOLOR：色無し、C_FT_BTNCLR_BLUE：青色、C_FT_BTNCLR_RED：赤色
//buttonOnOff＝C_FT_BTNDISP_OFF：Off、C_FT_BTNDISP_ON：On
//iconCount＝表示件数
//---------------------------------------------------------
function SetFooter(buttonID, buttonType, buttonOnOff, iconCount) {
    //色無しアイコン
    if (buttonType == C_FT_BTNCLR_NOCOLOR) {
        SetVisibilityIcon(buttonID, buttonOnOff)

        //青色アイコン
    } else if (buttonType == C_FT_BTNCLR_BLUE) {
        SetNormalIcon(buttonID, buttonOnOff, iconCount)

        //赤色アイコン
    } else if (buttonType == C_FT_BTNCLR_RED) {
        SetWarningIcon(buttonID, buttonOnOff, iconCount)
    }
}

//---------------------------------------------------------
//色無しアイコン変更
//buttonID＝100～2700
//buttonOnOff＝0：Off、1：On
//---------------------------------------------------------
function SetVisibilityIcon(buttonID, buttonOnOff) {
    var normalButton = "VisibilityIcon_Off";
    var iconURL = "url(../Styles/Images/SC3240101/Footer" + buttonID + ".png)";
    if (buttonOnOff == 1) {
        normalButton = "VisibilityIcon_On";
        bkButtonID = buttonID;
    }
    setFooterIcon(buttonID, normalButton, "", "", iconURL, "FooterName_Off");
}

//---------------------------------------------------------
//青色アイコン変更
//buttonID＝100～2700
//buttonOnOff＝0：Off、1：On
//iconCount＝表示件数
//---------------------------------------------------------
function SetNormalIcon(buttonID, buttonOnOff, iconCount) {
    var normalButton = "NormalIcon_Off";
    var lightBoxButton = "FooterLightBox";
    var iconURL = "url(../Styles/Images/SC3240101/Footer" + buttonID + "_Active.png)";
    if (buttonOnOff == 1) {
        normalButton = "NormalIcon_On";
        lightBoxButton = "";
        bkButtonID = buttonID;
    }
    setFooterIcon(buttonID, normalButton, lightBoxButton, iconCount, iconURL, "FooterName_On");
}

//---------------------------------------------------------
//赤色アイコン変更
//buttonID＝100～2700
//buttonOnOff＝0：Off、1：On
//iconCount＝表示件数
//---------------------------------------------------------
function SetWarningIcon(buttonID, buttonOnOff, iconCount) {
    var normalButton = "WarningIcon_Off";
    var lightBoxButton = "FooterLightBox";
    var iconURL = "url(../Styles/Images/SC3240101/Footer" + buttonID + "_Active.png)";
    if (buttonOnOff == 1) {
        normalButton = "WarningIcon_On";
        lightBoxButton = "";
        bkButtonID = buttonID;
    }
    setFooterIcon(buttonID, normalButton, lightBoxButton, iconCount, iconURL, "FooterName_On");
}

//---------------------------------------------------------
//アイコン設定
//buttonID＝100～2700
//buttonType＝0：Off、1：On
//lightBoxButton＝色有り
//iconCount＝表示件数
//iconURL＝画像URL
//nameType＝文字色
//---------------------------------------------------------
function setFooterIcon(buttonID, buttonType, lightBoxButton, iconCount, iconURL, nameType) {
    $("#FooterButton" + buttonID).removeClass().addClass(buttonType).addClass("FooterButton");
    $("#FooterButtonBox" + buttonID).removeClass().addClass(lightBoxButton);
    $("#FooterButtonCount" + buttonID).html(iconCount);
    document.getElementById("FooterButtonIcon" + buttonID).style.backgroundImage = iconURL;
    $("#FooterButtonName" + buttonID).removeClass().addClass(nameType);
}

//---------------------------------------------------------
//全てのフッターボタンを非表示にする
//---------------------------------------------------------
function HideAllFooterButton() {
    $("#MstPG_FootItem_Space_Left").attr("style", "display:none;");
    $("#InitFooterArea").attr("style", "display:none;");
    $("#ChipFooterArea").attr("style", "display:none;");
}

//---------------------------------------------------------
//受付フッターボタンを点滅させる
//---------------------------------------------------------
function BlinkReceprionButtonOn() {
    $("#FooterButton100").addClass("FotterBlink");
}
//---------------------------------------------------------
//受付フッターボタンを点滅をやめる
//---------------------------------------------------------
function BlinkReceprionButtonOff() {
    $("#FooterButton100").removeClass("FotterBlink");
}
//---------------------------------------------------------
//フッターボタンの表示件数を取得する
//---------------------------------------------------------
function GetButtonIconCount(buttonID) {
    var returnIconCount = $("#FooterButtonCount" + buttonID).html();
    //件数がない場合は「0」を設定
    if (returnIconCount == "") {
        returnIconCount = "0";
    }
    return parseInt(returnIconCount);
}
//---------------------------------------------------------
//SMBボタンを押すイベント
//---------------------------------------------------------
//2013/11/29 TMEJ 張 タブレット版SMB チーフテクニシャン機能開発 START
//function SMBButtonEvent() {
//    //画面にリフレッシュする
//    ClickChangeDate(0);
//}
//2013/11/29 TMEJ 張 タブレット版SMB チーフテクニシャン機能開発 END


/**
* サブチップボックス、またはストール上のスクロール位置を設定する.
* @param {String} areaId  ストールの配置エリアID
* @return {-} -
*/
function SetForceScroll(areaId) {
    switch (areaId) {
        case C_FT_BTNTP_CONFIRMED_RO:
            //受付サブチップボックス
            scrollSubBoxObj = $(".SubChipReception .SubChipBox").find(".scroll-inner");
            ForceScrollSubBox(scrollSubBoxObj, gTranslateValSubBoxX);
            break;

        case C_FT_BTNTP_WAIT_CONFIRMEDADDWORK:
            //追加作業サブチップボックス
            scrollSubBoxObj = $(".SubChipAdditionalWork .SubChipBox").find(".scroll-inner");
            ForceScrollSubBox(scrollSubBoxObj, gTranslateValSubBoxX);
            break;

        case C_FT_BTNTP_CONFIRMED_INSPECTION:
            //完成検査サブチップボックス
            scrollSubBoxObj = $(".SubChipCompletionInspection .SubChipBox").find(".scroll-inner");
            ForceScrollSubBox(scrollSubBoxObj, gTranslateValSubBoxX);
            break;

        case C_FT_BTNTP_WAITING_WASH:
        case C_FT_BTNTP_WASHING:
            //洗車サブチップボックス
            scrollSubBoxObj = $(".SubChipCarWash .SubChipBox").find(".scroll-inner");
            ForceScrollSubBox(scrollSubBoxObj, gTranslateValSubBoxX);
            break;

        case C_FT_BTNTP_WAIT_DELIVERY:
            //納車待ちサブチップボックス
            scrollSubBoxObj = $(".SubChipWaitingDelivered .SubChipBox").find(".scroll-inner");
            ForceScrollSubBox(scrollSubBoxObj, gTranslateValSubBoxX);
            break;

        case C_FT_BTNTP_NOSHOW:
            //NoShowサブチップボックス
            scrollSubBoxObj = $(".SubChipNoShow .SubChipBox").find(".scroll-inner");
            ForceScrollSubBox(scrollSubBoxObj, gTranslateValSubBoxX);                       
            break;

        case C_FT_BTNTP_STOP:
            //中断サブチップボックス
            scrollSubBoxObj = $(".SubChipStop .SubChipBox").find(".scroll-inner");
            ForceScrollSubBox(scrollSubBoxObj, gTranslateValSubBoxX);
            break;
            
        default:
            //ストール内
            ForceScrollMain(gTranslateValStallX, gTranslateValStallY);
            break;
    }
}

/**
* メインエリアを強制スクロールする.
* @param {Integer} translateX  水平スクロール値
* @param {Integer} translateY  垂直スクロール値
* @return {-} -
*/
function ForceScrollMain(translateX, translateY) {

    //メインエリア
    var scrollMainObj = $(".ChipArea_trimming").find(".scroll-inner");
    var translateMainVal = "translate3d(" + translateX + "px," + translateY + "px, 0px)";

    //時間エリア(横軸)
    var scrollTimeObj = $("#divScrollTime");
    var translateTimeVal = "translate3d(" + translateX + "px, 0px, 0px)";

    //ストールエリア(縦軸)
    var scrollStallObj = $("#divScrollStall");
    var translateStallVal = "translate3d(0px, " + translateY + "px, 0px)";

    //処理を段階的にしたいので、メソッドチェーンはしていない
    scrollMainObj.css({ "-webkit-transition": "none" });
    scrollTimeObj.css({ "-webkit-transition": "none" });
    scrollStallObj.css({ "-webkit-transition": "none" });

    scrollMainObj.css({ "transform": translateMainVal });
    scrollTimeObj.css({ "transform": translateTimeVal });
    scrollStallObj.css({ "transform": translateStallVal });
}

/**
* サブチップボックスを強制スクロールする.
* @param {String} scrollObj  スクロールさせるオブジェクト
* @param {Integer} translateX  水平スクロール値
* @return {-} -
*/
function ForceScrollSubBox(scrollObj, translateX) {
    var translateSubBoxVal = "translate3d(" + translateX + "px, 0px, 0px)";

    //処理を段階的にしたいので、メソッドチェーンはしていない
    scrollObj.css({ "-webkit-transition": "none" });
    scrollObj.css({ "transform": translateSubBoxVal });
}
