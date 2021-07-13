/** 
* @fileOverview SC3240201.js(チップ詳細)
* 
* @author TMEJ 岩城
* @version 1.0.0
* 更新： 2013/11/29 TMEJ 張 タブレット版SMB チーフテクニシャン機能開発
* 更新： 2013/12/04 TMEJ 下村 タブレット版SMB チーフテクニシャン機能開発
* 更新： 2013/12/26 TMEJ 下村 次世代e-CRBタブレット(サービス) iOS7.0 VersionUp対応
* 更新： 2014/01/16 TMEJ 明瀬 タブレット版SMB チーフテクニシャン機能開発
* 更新： 2014/06/05 TMEJ 丁 タブレット版SMB チーフテクニシャン機能開発
* 更新： 2014/06/18 TMEJ 丁 タブレット版SMB テレマ走行距離機能開発
* 更新： 2014/07/07 TMEJ 張 UAT-114 不具合対応
* 更新： 2014/07/18 TMEJ 明瀬 休憩取得ポップアップ文言文字化け対応
* 更新： 2014/07/15 TMEJ 丁 タブレットSMB Job Dispatch機能開発
* 更新： 2014/07/15 TMEJ 張 タブレットSMB Job Dispatch機能開発
* 更新： 2014/09/12 TMEJ 張 BTS-390 「1ｽﾄｰﾙで2ﾁｯﾌﾟが作業中（表示のみ）」対応
* 更新： 2014/12/02 TMEJ 丁 DMS連携版サービスタブレットSMB来店予約時間外計画機能開発
* 更新： 2014/12/08 TMEJ 丁 DMS連携版サービスタブレット JobDispatch完成検査入力制御開発
* 更新： 2015/04/23 TMEJ 明瀬  DMS連携版サービスタブレット強制納車機能追加開発
* 更新： 2015/06/15 TMEJ 河原  サービス入庫予約情報のバリデーション実行タイミング変更に向けた評価用アプリ作成
* 更新： 2016/10/04 NSK  秋田谷 チップ詳細の実績時間を変更できなくする対応
* 更新： 2017/09/05 NSK 小川 REQ-SVT-TMT-20160906-003 計画済みチップをストールから簡単に外せるようにする
* 更新： 2018/06/26 NSK 可児 TKM Next Gen e-CRB Project Application development Block B-1  Pマーク、L マークなどを表示
* 更新： 2019/08/02 NSK 近藤 (トライ店システム評価)次世代サービスオペレーション効率化に向けた、業務適合性検証
* 更新： 2019/12/06 NSK 皆川 (トライ店システム評価)整備作業における休憩時間取得判定向上検証
* 更新： 
*/

var chipDetailSlideDownFlag;

/**
* ポップアップ表示(SC3240101から詳細ボタンクリック時にコールされる)
*
*/
function ShowChipDetailSmall() {


    // 2014/09/12 TMEJ 張 BTS-390 「1ｽﾄｰﾙで2ﾁｯﾌﾟが作業中（表示のみ）」対応 START
    try {

        // 2014/09/12 TMEJ 張 BTS-390 「1ｽﾄｰﾙで2ﾁｯﾌﾟが作業中（表示のみ）」対応 END

        //チップ詳細(小)の拡大ボタンを非表示
        $("#ExpansionButton").css("display", "none");

        //登録ボタンを非活性にしておく
        $("#DetailRegisterBtn").attr("disabled", true);

        //チップ詳細(小)・チップ詳細(大)のコンテンツを削除
        $('#ChipDetailSContent>div').remove();
        $('#ChipDetailLContent>div').remove();

        //工程管理画面で選択されているチップID
        var baseCtrl = $("#" + gSelectedChipId);

        //ポップアップの表示位置を設定
        SetDetailSPopoverPosition(CalcDetailSPopoverPosition(baseCtrl));

        //ポップアップ表示
        $("#ChipDetailPopup").fadeIn(300);

        //アクティブインジケータ表示
        gDetailSActiveIndicator.show();

        //リフレッシュタイマーセット
        commonRefreshTimer(ReDisplayChipDetail);

        //画面初期化情報を取得して作成する
        CreateChipDetailPage();

        //チップ詳細画面のクローズ監視イベント設定
        //SetEventChipDetailClose();

        // 2014/09/12 TMEJ 張 BTS-390 「1ｽﾄｰﾙで2ﾁｯﾌﾟが作業中（表示のみ）」対応 START
    }
    catch (e) {

        // チップ移動した直後に、詳細ボタンを押す
        // 詳細画面がfadein中、サーバーから該当チップの最新の情報を戻って、
        // チップを更新し、詳細画面をfadeoutし、チップ選択を解除したあとで、
        // fadeinが300ミリ秒を設定するから、チップ詳細関数を走る
        // gSelectedChipIdが空白なので、エラーが発生した

        // gSelectedChipIdが空白の場合
        if (gSelectedChipId == "") {

            // 詳細画面を閉じる
            HideChipDetailPopup();

            // 選択状態を解除する
            SetChipUnSelectedStatus();
            SetTableUnSelectedStatus();

        }
    }
    // 2014/09/12 TMEJ 張 BTS-390 「1ｽﾄｰﾙで2ﾁｯﾌﾟが作業中（表示のみ）」対応 END

};

/**
* 画面を作成する
* 
*/
function CreateChipDetailPage() {

    if ($("#MstPG_registOverlayBlack").css("display") == "block") {
        //オーバーレイ非表示(リロード対応)        
        gDetailOverlay.hide();
    }

    //サーバーに渡すパラメータを作成
    var prms = CreateCallBackDisplayParam(C_SC3240201CALLBACK_CREATEDISP, gSelectedChipId);

    //コールバック開始
    DoCallBack(C_CALLBACK_WND201, prms, SC3240201AfterCallBack, "ShowDetail");
}

/**
* 画面内容を登録する
* 
*/
function RegisterChipDetail() {

    //ボタンを青色にする
    $("#DetailRightBtnDiv").addClass("icrop-pressed");

    //背景色をクリア
    $("#DetailRegisterBtn").css("background-image", "none");

    setTimeout(function () {
        //ボタンの青色を解除
        $("#DetailRightBtnDiv").removeClass("icrop-pressed");

        //背景色を戻す
        $("#DetailRegisterBtn").css("background-image", "-webkit-gradient(linear, left top, left bottom, from(#6485cc),color-stop(0.01, #6485cc),color-stop(0.01, #6f96e8),color-stop(0.5, #376fe0),color-stop(0.5, #2361dd),to(#2463de))");
    }, 300);

    //アクティブインジケータ表示
    gDetailSActiveIndicator.show();
    //オーバーレイ表示
    gDetailOverlay.show();

    //リフレッシュタイマーセット
    commonRefreshTimer(ReDisplayChipDetail);

    //サーバーに渡すパラメータを作成
    var prms = CreateCallBackRegistParam(C_SC3240201CALLBACK_REGISTER, gSelectedChipId, -1);

    //コールバック開始
    DoCallBack(C_CALLBACK_WND201, prms, SC3240201AfterCallBack, "ClickDetailOKButton");
    return false;
}

/**
* コールバックでサーバーに渡すパラメータを作成する
*
* @param {String}   method:   コールバック時のメソッド分岐用
* @param {Integer}  chipId:   チップのID
*
*/
function CreateCallBackDisplayParam(method, chipId) {

    //サブチップボックスならば数値が返却される
    var subAreaId = GetSubChipType(chipId);

    //共通パラメータ
    var rtnVal = {
          Method: method                                    //サーバー処理分岐用
        , DlrCD: $("#hidDlrCD").val()                       //販売店コード
        , StrCD: $("#hidBrnCD").val()                       //店舗コード
        , ShowDate: $("#hidShowDate").val()                 //表示日時(yyyy/MM/dd)
        , SubAreaId: subAreaId                              //サブチップボックスのID(5:受付/15:追加作業/14:完成検査/16,17:洗車/18:納車/20:NoShow/21:中断/Empty:ストール)
        , StallStartTime: $("#hidStallStartTime").val()     //営業開始時間(HH:mm)
        , StallEndTime: $("#hidStallEndTime").val()         //営業終了時間(HH:mm)
    }

    //選択中チップが所属するエリアによってサーバーに渡すパラメータを変更する
      switch (subAreaId) {

          case C_FT_BTNTP_CONFIRMED_RO:
              //更新：2017/09/05 NSK 小川 REQ-SVT-TMT-20160906-003 計画済みチップをストールから簡単に外せるようにする START
              if (gArrObjSubChip[chipId].tempFlg == "1") {
                  //Tempサブチップ
                  rtnVal.subAreaId = ""     //ROが採番されていない仮置きチップでも受付サブエリア上からチップ詳細を開くための処理
                  rtnVal.RONum = gArrObjSubChip[chipId].roNum;                  //RO番号
                  rtnVal.SvcInId = gArrObjSubChip[chipId].svcInId;              //サービス入庫ID
                  rtnVal.JobDtlId = gArrObjSubChip[chipId].jobDtlId;            //作業内容ID
                  rtnVal.StallUseId = gArrObjSubChip[chipId].stallUseId;        //ストール利用ID
              } else {
                  //受付チップ
                  rtnVal.RONum = gArrObjSubChip[chipId].roNum;              //RO番号
                  rtnVal.SvcInId = gArrObjSubChip[chipId].svcInId;          //サービス入庫ID
                  rtnVal.ROJobSeq = gArrObjSubChip[chipId].roJobSeq;        //作業連番
                  rtnVal.SrvAddSeq = gArrObjSubChip[chipId].srvAddSeq;      //枝番
              }
              break;
             //更新：2017/09/05 NSK 小川 REQ-SVT-TMT-20160906-003 計画済みチップをストールから簡単に外せるようにする END

        case C_FT_BTNTP_WAIT_CONFIRMEDADDWORK:
            //追加作業サブチップボックス
            rtnVal.RONum = gArrObjSubChip[chipId].roNum;              //RO番号
            rtnVal.SvcInId = gArrObjSubChip[chipId].svcInId;          //サービス入庫ID
            rtnVal.ROJobSeq = gArrObjSubChip[chipId].roJobSeq;        //作業連番
            rtnVal.SrvAddSeq = gArrObjSubChip[chipId].srvAddSeq;      //枝番
            break;

        case C_FT_BTNTP_CONFIRMED_INSPECTION:
        case C_FT_BTNTP_WAITING_WASH:
        case C_FT_BTNTP_WASHING:
        case C_FT_BTNTP_WAIT_DELIVERY:
        case C_FT_BTNTP_NOSHOW:
        case C_FT_BTNTP_STOP:
            //完成検査・洗車・納車・NoShow・中断サブチップボックス
            rtnVal.RONum = gArrObjSubChip[chipId].roNum;              //RO番号
            rtnVal.SvcInId = gArrObjSubChip[chipId].svcInId;          //サービス入庫ID
            rtnVal.JobDtlId = gArrObjSubChip[chipId].jobDtlId;        //作業内容ID
            rtnVal.StallUseId = gArrObjSubChip[chipId].stallUseId;    //ストール利用ID
            break;

        default:
            //ストール内
            rtnVal.RONum = gArrObjChip[chipId].roNum;                  //RO番号
            rtnVal.SvcInId = gArrObjChip[chipId].svcInId;              //サービス入庫ID
            rtnVal.JobDtlId = gArrObjChip[chipId].jobDtlId;            //作業内容ID
            rtnVal.StallUseId = gArrObjChip[chipId].stallUseId;        //ストール利用ID
            break;
    }

    return rtnVal;
}

/**
* コールバックでサーバーに渡すパラメータを作成する（整備名の取得用）
*
* @param {String}   method:   コールバック時のメソッド分岐用
* @param {Integer}  aftValue: 選択した変更後のサービス分類ID（整備種類）
*
*/
function CreateCallBackGetMercParam(method, aftValue) {

    //パラメータ
    var rtnVal = {
          Method: method                                    //サーバー処理分岐用
        , DlrCD: $("#hidDlrCD").val()                       //販売店コード
        , StrCD: $("#hidBrnCD").val()                       //店舗コード
        , SvcClassId: aftValue                              //選択した変更後のサービス分類ID（整備種類）
    };

    return rtnVal;
}

/**
* データ登録のコールバックでサーバーに渡すパラメータを作成する
*
* @param {String}   method:   コールバック時のメソッド分岐用
* @param {Integer}  chipId:   チップのID
* @param {Integer}  chipId:   休憩取得フラグ（-1:省略値／0:休憩を取得しない／1:休憩を取得する）
*
*/
function CreateCallBackRegistParam(method, chipId, restFlg) {

    //チップ表示用開始時間と終了時間をグローバル変数に設定する
    gSC3240201DisplayChipStartTime = GetDisplayStartTime();
    gSC3240201DisplayChipEndTime = GetDisplayEndTime();

    //整備コード、整備連番、枝番、予約IDのリストを作成する
    CreateMainteInfoList();

    //チップ詳細のチップエリアに表示した予約IDのリストを作成する
    CreateRezIdList();

    //整備種類のDropDownListで選択されているIDを取得
    var e = document.getElementById("DetailSMaintenanceTypeList");
    var regSvcClassId;
    if (e.selectedIndex < 0) {
    	regSvcClassId = 0;
    } else {
        //「サービス分類ID,標準作業時間」の文字列を分解
        var svcClassInfo = e.options[e.selectedIndex].value.split(",");
        regSvcClassId = svcClassInfo[0];
    }

    //整備名のDropDownListで選択されているIDを取得
    var f = document.getElementById("DetailSMercList");
    var regMercId;
    if (f.selectedIndex < 0) {
        regMercId = 0;
    } else {
        var mercInfo = f.options[f.selectedIndex].value.split(",");
        regMercId = mercInfo[0];
    }

    //サブチップボックスならば数値が返却される
    var subAreaId = GetSubChipType(gSelectedChipId);
    var objSubChip;

    //登録時の共通パラメータ
    var rtnVal = {
          Method: method                                                //サーバー処理分岐用
        , SvcClassId: regSvcClassId                                     //表示サービス分類ID
        , MercId: regMercId                                             //表示商品ID

        //2013/12/26 TMEJ 下村 次世代e-CRBタブレット(サービス) iOS7.0 VersionUp対応 START
        //, VisitPlanTime: smbScript.ConvertDateToString2($("#DetailSPlanVisitDateTimeSelector").get(0).valueAsDate)           //来店予定日時
        //, StartPlanTime: smbScript.ConvertDateToString2($("#DetailSPlanStartDateTimeSelector").get(0).valueAsDate)           //作業開始予定日時
        //, FinishPlanTime: smbScript.ConvertDateToString2($("#DetailSPlanFinishDateTimeSelector").get(0).valueAsDate)         //作業終了予定日時
        //, DeriveredPlanTime: smbScript.ConvertDateToString2($("#DetailSPlanDeriveredDateTimeSelector").get(0).valueAsDate)   //納車予定日時
        //, StartProcessTime: smbScript.ConvertDateToString2($("#DetailSProcessStartDateTimeSelector").get(0).valueAsDate)     //作業開始実績日時
        //, FinishProcessTime: smbScript.ConvertDateToString2($("#DetailSProcessFinishDateTimeSelector").get(0).valueAsDate)   //作業終了実績日時
        , VisitPlanTime: smbScript.ConvertDateToString2(smbScript.changeStringToDateIcrop($("#DetailSPlanVisitDateTimeSelector").get(0).value))           //来店予定日時
        , StartPlanTime: smbScript.ConvertDateToString2(smbScript.changeStringToDateIcrop($("#DetailSPlanStartDateTimeSelector").get(0).value))           //作業開始予定日時
        , FinishPlanTime: smbScript.ConvertDateToString2(smbScript.changeStringToDateIcrop($("#DetailSPlanFinishDateTimeSelector").get(0).value))         //作業終了予定日時
        , DeriveredPlanTime: smbScript.ConvertDateToString2(smbScript.changeStringToDateIcrop($("#DetailSPlanDeriveredDateTimeSelector").get(0).value))   //納車予定日時
        , StartProcessTime: smbScript.ConvertDateToString2(smbScript.changeStringToDateIcrop($("#DetailSProcessStartDateTimeSelector").get(0).value))     //作業開始実績日時
        , FinishProcessTime: smbScript.ConvertDateToString2(smbScript.changeStringToDateIcrop($("#DetailSProcessFinishDateTimeSelector").get(0).value))   //作業終了実績日時
        //2013/12/26 TMEJ 下村 次世代e-CRBタブレット(サービス) iOS7.0 VersionUp対応 END

        , PlanWorkTime: $("#DetailSWorkTimeTxt").val()                  //予定作業時間
        , ProcWorkTime: CalcSC3240201ProcWorkTime()                     //実績作業時間
        , RezFlg: $("#RezFlgHidden").val()                              //予約フラグ
        , CarWashFlg: $("#CarWashFlgHidden").val()                      //洗車フラグ
        , WaitingFlg: $("#WaitingFlgHidden").val()                      //待ち方フラグ
        , Order: $("#DetailSOrderTxt").val()                            //ご用命
        //2013/12/04 TMEJ 下村 タブレット版SMB チーフテクニシャン機能開発 START
        //, Failure: $("#DetailSFailureTxt").val()                        //故障原因
        //, Result: $("#DetailSResultTxt").val()                          //診断結果
        //, Advice: $("#DetailSAdviceTxt").val()                          //アドバイス
        //2013/12/04 TMEJ 下村 タブレット版SMB チーフテクニシャン機能開発 END
        , ROInfoChangeFlg: SetSC3240201ROInfo()                         //RO情報変更フラグ（0:変更なし／1:変更あり）
        , DlrCD: $("#hidDlrCD").val()                                   //販売店コード
        , StrCD: $("#hidBrnCD").val()                                   //店舗コード
        , Account: icropScript.ui.account                               //ログインアカウント
        , ShowDate: $("#hidShowDate").val()                             //表示日時(yyyy/MM/dd)
        , StallStartTime: $("#hidStallStartTime").val()                 //営業開始時間(HH:mm)
        , StallEndTime: $("#hidStallEndTime").val()                     //営業終了時間(HH:mm)
        , FixItemCodeList: gSC3240201FixItemCodeList                    //チップ詳細に表示されている整備の整備コードリスト
    	//2013/12/04 TMEJ 下村 タブレット版SMB チーフテクニシャン機能開発 START
        //, FixItemSeqList: gSC3240201FixItemSeqList                      //チップ詳細に表示されている整備の整備連番リスト
        , JobinstrucDtlIdList: gSC3240201JobinstrucDtlIdList            //チップ詳細に表示されている整備の作業内容IDリスト
        , JobInstructSeqList: gSC3240201JobInstructSeqList              //チップ詳細に表示されている整備の作業指示枝番リスト
        , JobInstructIdList: gSC3240201JobInstructIdList                //チップ詳細に表示されている整備の作業指示IDリスト
    	//2013/12/04 TMEJ 下村 タブレット版SMB チーフテクニシャン機能開発 END
        , ROJobSeqList: gSC3240201RoJobSeqList                          //チップ詳細に表示されている整備の作業連番リスト
    	//2013/12/04 TMEJ 下村 タブレット版SMB チーフテクニシャン機能開発 START
        //, SrvAddSeqList: gSC3240201SrvAddSeqList                        //チップ詳細に表示されている整備の枝番リスト
    	//2013/12/04 TMEJ 下村 タブレット版SMB チーフテクニシャン機能開発 END
        , MatchingRezIdList: gSC3240201MatchingRezIdList                //整備に紐付いた予約IDのリスト
        , BeforeMatchingRezIdList: gSC3240201Before_MatchingRezIdList   //整備に紐付いた予約IDのリスト(チップ詳細表示時点)
        , RezIdList: gSC3240201RezIdList                                //チップエリアに表示した予約IDのリスト
        , RezIdStallUseStatusList: gSC3240201RezId_StallUseStatusList   //チップエリアに表示した予約のストール利用ステータスリスト
    	//2013/12/04 TMEJ 下村 タブレット版SMB チーフテクニシャン機能開発 START
        //, RoJobSeq2List: gSC3240201RoJobSeq2List                        //チップエリアに表示した予約の作業連番リスト
    	, InvisibleInstructFlgList: gSC3240201RezId_InvisibleInstructFlgList //チップエリアに表示した予約の着工指示フラグリスト
    	//2013/12/04 TMEJ 下村 タブレット版SMB チーフテクニシャン機能開発 END
        , SubAreaId: subAreaId                                          //サブチップボックスのID(5:受付/15:追加作業/14:完成検査/16,17:洗車/18:納車/20:NoShow/21:中断/Empty:ストール)
        , RestFlg: restFlg                                              //休憩取得フラグ
        , InputStallStartTime: CalcInputStallStartTime()                //チップ詳細画面で指定した日時の営業開始時間(date)
        , InputStallEndTime: CalcInputStallEndTime()                    //チップ詳細画面で指定した日時の営業終了時間(date)
        , StallUseStatus: $("#ChipDetailStallUseStatusHidden").val()    //ストール利用.ストール利用ステータス
        , PrmsEndTime: CalcSC3240201PrmsEndTime()                       //見込終了日時
        , CstName: $("#DetailSCstNameLabel").text()                     //顧客名
        , Mobile: $("#DetailSMobileLabel").text()                       //携帯電話番号
        , Home: $("#DetailSHomeLabel").text()                           //電話番号
        , RegNo: $("#DetailSRegNoLabel").text()                         //登録No.
        , Vin: $("#DetailSVinLabel").text()                             //VIN
        , Vehicle: $("#DetailSVehicleLabel").text()                     //車種
        //2013/12/04 TMEJ 下村 タブレット版SMB チーフテクニシャン機能開発 START
        , CompleteExaminationFlg: $("#CompleteExaminationFlgHidden").val()   //完成検査フラグ
        , Memo: $("#DetailSMemoTxt").val()                              //メモ
        , CstAddress: $("#DetailLCstAddressLabel").val()                //顧客住所
        , FleetFlg: $("#FleetFlgHidden").val()                          //法人フラグ
        , DmsCstCD: $("#DmsCstCdHidden").val()                          //基幹顧客コード
        , NameTitleName: $("#NameTitleNameHidden").val()                //敬称
        , PositionType: $("#PositionTypeHidden").val()                  //配置区分
        , CstType: $("#CstTypeHidden").val()                 			//顧客種別
        , DmsJobDtlId: $("#DmsJobDtlIdHidden").val()                 	//基幹作業内容ID
        , VisitSeq: $("#Visit_VisitSeqHidden").val()                 	//来店者実績連番
        , VisitVin: getSC3240201VinName()                 				//来店者Vin
        , InvoiceDateTime: $("#InvoiceDateTimeHidden").val()            //清算準備完了日時
        //2013/12/04 TMEJ 下村 タブレット版SMB チーフテクニシャン機能開発 END
    };

    //選択中チップが所属するエリアによってサーバーに渡すパラメータを変更する
    switch (subAreaId) {
        case C_FT_BTNTP_CONFIRMED_RO:
        case C_FT_BTNTP_WAIT_CONFIRMEDADDWORK:
            //受付・追加作業サブチップボックス
            objSubChip = gArrObjSubChip[chipId];

            rtnVal.RowLockVersion = objSubChip.rowLockVersion;                                  //行ロックバージョン
            rtnVal.SvcInId = objSubChip.svcInId;                                                //サービス入庫ID
            rtnVal.JobDtlId = objSubChip.jobDtlId;                                              //作業内容ID
            rtnVal.StallUseId = objSubChip.stallUseId;                                          //ストール利用ID
            rtnVal.CstId = objSubChip.cstId;                                                    //顧客ID
            rtnVal.VclId = objSubChip.vclId;                                                    //車両ID
            rtnVal.ResvStatus = objSubChip.resvStatus;                                          //予約ステータス
            rtnVal.RONum = objSubChip.roNum;                                                    //RO番号
    	    //2013/12/04 TMEJ 下村 タブレット版SMB チーフテクニシャン機能開発 START
            //rtnVal.ROJobSeq = objSubChip.roJobSeq;                                              //RO作業連番
    	    //2013/12/04 TMEJ 下村 タブレット版SMB チーフテクニシャン機能開発 END
            rtnVal.SrvAddSeq = objSubChip.srvAddSeq;                                            //枝番
            rtnVal.DispStartTime = $("#hidShowDate").val();                                     //チップ表示用開始時間（メイン画面の表示日時を渡す）
            rtnVal.DispEndTime = $("#hidShowDate").val();                                       //チップ表示用終了時間（メイン画面の表示日時を渡す）
            rtnVal.ValidateCode = 0;                                                            //入力項目チェック結果コード
            rtnVal.SubPlanWorkTime = objSubChip.scheWorkTime;                                   //予定作業時間(RO番号→サービス入庫ID→MIN(作業内容ID)の予定作業時間)
            rtnVal.SubStartPlanTime = smbScript.ConvertDateToString2(objSubChip.scheStartDateTime);    //予定作業時間(RO番号→サービス入庫ID→MIN(作業内容ID)の予定開始日時)
            rtnVal.SubFinishPlanTime = smbScript.ConvertDateToString2(objSubChip.scheEndDateTime);     //予定作業時間(RO番号→サービス入庫ID→MIN(作業内容ID)の予定終了日時)
            rtnVal.SubStallId = objSubChip.stallId;                                             //ストールID
            rtnVal.SubRestFlg = objSubChip.restFlg;                                             //休憩取得フラグ
            break;

        case C_FT_BTNTP_CONFIRMED_INSPECTION:
        case C_FT_BTNTP_WAITING_WASH:
        case C_FT_BTNTP_WASHING:
        case C_FT_BTNTP_WAIT_DELIVERY:
        case C_FT_BTNTP_NOSHOW:
        case C_FT_BTNTP_STOP:
            //完成検査・洗車・納車・NoShow・中断サブチップボックス
            objSubChip = gArrObjSubChip[chipId];

            rtnVal.RowLockVersion = objSubChip.rowLockVersion;                                  //行ロックバージョン
            rtnVal.RONum = objSubChip.roNum;                                                    //RO番号
            rtnVal.SvcInId = objSubChip.svcInId;                                                //サービス入庫ID
            rtnVal.JobDtlId = objSubChip.jobDtlId;                                              //作業内容ID
            rtnVal.StallUseId = objSubChip.stallUseId;                                          //ストール利用ID
            rtnVal.CstId = objSubChip.cstId;                                                    //顧客ID
            rtnVal.VclId = objSubChip.vclId;                                                    //車両ID
            rtnVal.ResvStatus = objSubChip.resvStatus;                                          //予約ステータス
    	    //2013/12/04 TMEJ 下村 タブレット版SMB チーフテクニシャン機能開発 START
            //rtnVal.ROJobSeq = $("#ChipDetailRoJobSeqHidden").val();                             //RO作業連番
    	    //2013/12/04 TMEJ 下村 タブレット版SMB チーフテクニシャン機能開発 END
            //rtnVal.StallId = objSubChip.stallId;                                                //ストールID
            rtnVal.StallId = objSubChip.stallId;                                                //ストールID
            rtnVal.ChipDispStartDate = GetSC3240201ChipDispStartDate();                         //チップ表示開始日時
            rtnVal.DispStartTime = gSC3240201DisplayChipStartTime;                              //チップ表示用開始時間
            rtnVal.DispEndTime = gSC3240201DisplayChipEndTime;                                  //チップ表示用終了時間
            rtnVal.ValidateCode = CheckChipDetailInputValue();                                  //入力項目チェック結果コード
            break;

        default:
            //ストール内
            var objChip = gArrObjChip[chipId];

            rtnVal.RowLockVersion = objChip.rowLockVersion;                                     //行ロックバージョン
            rtnVal.RONum = objChip.roNum;                                                       //RO番号
            rtnVal.SvcInId = objChip.svcInId;                                                   //サービス入庫ID
            rtnVal.JobDtlId = objChip.jobDtlId;                                                 //作業内容ID
            rtnVal.StallUseId = objChip.stallUseId;                                             //ストール利用ID
            rtnVal.CstId = objChip.cstId;                                                       //顧客ID
            rtnVal.VclId = objChip.vclId;                                                       //車両ID
            rtnVal.ResvStatus = objChip.resvStatus;                                             //予約ステータス
    	    //2013/12/04 TMEJ 下村 タブレット版SMB チーフテクニシャン機能開発 START
            //rtnVal.ROJobSeq = $("#ChipDetailRoJobSeqHidden").val();                             //RO作業連番
    	    //2013/12/04 TMEJ 下村 タブレット版SMB チーフテクニシャン機能開発 END
            rtnVal.StallId = objChip.stallId;                                                   //ストールID
            rtnVal.ChipDispStartDate = GetSC3240201ChipDispStartDate();                         //チップ表示開始日時
            rtnVal.DispStartTime = gSC3240201DisplayChipStartTime;                              //チップ表示用開始時間
            rtnVal.DispEndTime = gSC3240201DisplayChipEndTime;                                  //チップ表示用終了時間
            rtnVal.ValidateCode = CheckChipDetailInputValue();                                  //入力項目チェック結果コード
            break;
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
function SC3240201AfterCallBack(result, context) {

    var jsonResult = JSON.parse(result);

    //タイマーをクリア
    commonClearTimer();

    //コールバック結果コードの取得
    var resultCD = jsonResult.ResultCode;

    //商品コンボボックス用データが存在しない場合
    if (resultCD == 5) {

        //アクティブインジケータ・オーバーレイ非表示
        gDetailSActiveIndicator.hide();
        gDetailOverlay.hide();

        //チップ詳細(小)　商品コンボボックスを初期化
        var e = document.getElementById("DetailSMercList");
        e.options.length = 0; //コンボボックス内のデータをクリア
        $("#DetailSMercLabel").text("");

        //チップ詳細(大)　商品コンボボックスを初期化
        var f = document.getElementById("DetailLMercList");
        f.options.length = 0; //コンボボックス内のデータをクリア
        $("#DetailLMercLabel").text("");

        //商品コンボボックスを非活性にする
        $("#DetailSMercList").attr("disabled", true);
        $("#DetailLMercList").attr("disabled", true);

        //必須項目がEmptyなら登録ボタンを非活性にする
        $("#DetailRegisterBtn").attr("disabled", IsMandatoryChipDetailEmpty());

        //次の操作を実行する
        AfterCallBack();

        // 2015/06/15 TMEJ 河原  サービス入庫予約情報のバリデーション実行タイミング変更に向けた評価用アプリ作成 STRAT

        //商品データなし("0"：商品データ無し)
        $("#DetailSMercList").attr("MERCITEM", 0);

        // 2015/06/15 TMEJ 河原  サービス入庫予約情報のバリデーション実行タイミング変更に向けた評価用アプリ作成 END

        return false;
    }
    //2015/04/23 TMEJ 明瀬  DMS連携版サービスタブレット強制納車機能追加開発 START

    //else if (resultCD != 0) {
    else if (resultCD != 0 &&
             resultCD != -9000) {

    //2015/04/23 TMEJ 明瀬  DMS連携版サービスタブレット強制納車機能追加開発 END

        //サーバー処理でエラー発生の場合

        //休憩・使用不可チップとの重複以外は、ここでエラーメッセージダイアログを表示
        //  (登録時、更新チップが休憩・使用不可チップと重複した場合は、別途confirmで出す)
        if (resultCD != 8) {
            icropScript.ShowMessageBox(resultCD, jsonResult.Message, "");
        }

        //アクティブインジケータ・オーバーレイ非表示
        gDetailSActiveIndicator.hide();
        gDetailOverlay.hide();

        //Newで生成したArrayを削除
        gSC3240201RezIdList = null;
        gSC3240201RezId_StallUseStatusList = null;
    	//2013/12/04 TMEJ 下村 タブレット版SMB チーフテクニシャン機能開発 START
        //gSC3240201RoJobSeq2List = null;
    	gSC3240201RezId_InvisibleInstructFlgList = null;
    	//2013/12/04 TMEJ 下村 タブレット版SMB チーフテクニシャン機能開発 END
        //gSC3240201Before_MatchingRezIdList = null;
        gSC3240201MatchingRezIdList = null;
        gSC3240201FixItemCodeList = null;
    	//2013/12/04 TMEJ 下村 タブレット版SMB チーフテクニシャン機能開発 START
        //gSC3240201FixItemSeqList = null;
    	gSC3240201JobinstrucDtlIdList = null;
    	gSC3240201JobInstructSeqList = null;
    	gSC3240201JobInstructIdList = null;
    	//2013/12/04 TMEJ 下村 タブレット版SMB チーフテクニシャン機能開発 END
        gSC3240201RoJobSeqList = null;
    	//2013/12/04 TMEJ 下村 タブレット版SMB チーフテクニシャン機能開発 START
        //gSC3240201SrvAddSeqList = null;
    	//2013/12/04 TMEJ 下村 タブレット版SMB チーフテクニシャン機能開発 END

        //登録時、更新チップが休憩と重複した場合
        if (resultCD == 8) {

            //『休憩を取得しますか？(取得する場合はOKを選択、取得しない場合はキャンセルを選択)』

            //2014/07/18 TMEJ 明瀬 休憩取得ポップアップ文言文字化け対応 START
            //var result = confirm($("#WordDuplicateRestOrUnavailableHidden").val());
            var result = confirm(htmlDecode($("#WordDuplicateRestOrUnavailableHidden").val()));
            //2014/07/18 TMEJ 明瀬 休憩取得ポップアップ文言文字化け対応 END

            //休憩取得フラグ（0:休憩を取得しない／1:休憩を取得する）
            var restFlg = 0;

            //OK(1:休憩を取得する)をタップした場合
            if (result) {
                restFlg = 1;
            }
            else {
                //作業終了予定時間が読取専用でない場合
                if (!$("#DetailSPlanFinishDateTimeSelector").attr("readonly")) {

                    //作業終了予定を再設定する
                    //開始日時と加算時間を元に、終了日時を計算する（営業時間外の時間帯は除外して計算）

                    //2013/12/26 TMEJ 下村 次世代e-CRBタブレット(サービス) iOS7.0 VersionUp対応 START
                    //var newEndDateTime = smbScript.CalcEndDate_ExcludeOutOfTime($("#DetailSPlanStartDateTimeSelector").get(0).valueAsDate, $("#DetailSWorkTimeTxt").val());
                    //
                    //$("#DetailSPlanFinishDateTimeSelector").get(0).valueAsDate = newEndDateTime;
                    //$("#DetailSPlanFinishLabel").text(smbScript.ConvertDateOrTime(newEndDateTime, $("#hidShowDate").val()));
                    //
                    ////チップ詳細(大)に反映
                    //$("#DetailLPlanFinishDateTimeSelector").get(0).valueAsDate = newEndDateTime;
                    //$("#DetailLPlanFinishLabel").text($("#DetailSPlanFinishLabel").text());

                    var newEndDateTime = smbScript.CalcEndDate_ExcludeOutOfTime(smbScript.changeStringToDateIcrop($("#DetailSPlanStartDateTimeSelector").get(0).value), $("#DetailSWorkTimeTxt").val());

                    $("#DetailSPlanFinishDateTimeSelector").get(0).value = smbScript.getDateTimelocalDate(newEndDateTime);
                    $("#DetailSPlanFinishLabel").text(smbScript.ConvertDateOrTime(newEndDateTime, $("#hidShowDate").val()));

                    //チップ詳細(大)に反映
                    $("#DetailLPlanFinishDateTimeSelector").get(0).value = smbScript.getDateTimelocalDate(newEndDateTime);
                    $("#DetailLPlanFinishLabel").text($("#DetailSPlanFinishLabel").text());
                    //2013/12/26 TMEJ 下村 次世代e-CRBタブレット(サービス) iOS7.0 VersionUp対応 END
                }
            }

            //アクティブインジケータ表示
            gDetailSActiveIndicator.show();
            //オーバーレイ表示
            gDetailOverlay.show();

            //リフレッシュタイマーセット
            commonRefreshTimer(ReDisplayChipDetail);

            //サーバーに渡すパラメータを作成
            var prms = CreateCallBackRegistParam(C_SC3240201CALLBACK_REGISTER, gSelectedChipId, restFlg);

            //次の操作を実行する
            AfterCallBack();

            //コールバック開始
            DoCallBack(C_CALLBACK_WND201, prms, SC3240201AfterCallBack, "ClickDetailOKButton");

        }
        //チップをタップし、詳細ボタンをタップしたときに他のユーザーがチップを削除していた場合
        else if (resultCD == 4) {
            //チップ詳細を閉じる
            CloseChipDetail(0);

            //サブチップボックスを閉じる
            HiddenSubChipBox();

            //選択したチップを解放する(SetTableUnSelectedStatusは~/SC3240101/Table.js内のメソッド)
            SetTableUnSelectedStatus(gSelectedChipId);

            //チップ選択状態を解除する(SetChipUnSelectedStatusは~/SC3240101/Chip.js内のメソッド)
            SetChipUnSelectedStatus();

            //操作リストをクリアする
            ClearOperationList();

            //工程管理画面のチップを再描画する(ClickChangeDate~/SC3240101/Table.js内のメソッド)
            ClickChangeDate(0);
        } 
        else {
            //次の操作を実行する
            AfterCallBack();
            return false;
        }

    }
    else {
        //画面作成
        if (jsonResult.Caller == C_SC3240201CALLBACK_CREATEDISP) {

            //2014/07/15 TMEJ 張 タブレットSMB Job Dispatch機能開発 START
            //選択されたチップが無くなった場合
            if ((!CheckgArrObjChip(gSelectedChipId)) && (!CheckgArrObjSubChip(gSelectedChipId))) {

                //アクティブインジケータ・オーバーレイ非表示
                gDetailSActiveIndicator.hide();
                gDetailOverlay.hide();
                gMainAreaActiveIndicator.hide();

                //詳細画面を閉じる
                HideChipDetailPopup();
                //選択したチップを解放する
                SetTableUnSelectedStatus();
                //チップ選択状態を解除する
                SetChipUnSelectedStatus();

                //次の操作を実行する
                AfterCallBack();
                return;
            }
            //2014/07/15 TMEJ 張 タブレットSMB Job Dispatch機能開発 END

            //画面の初期化 
            InitPage(result, context);

            //アクティブインジケータ非表示
            gDetailSActiveIndicator.hide();

            //固定文言をチップ詳細(小)から(大)へコピーする
            CopyToDetailLWord();
            //リピーター系以外の動的データをチップ詳細(小)から(大)へコピーする
            CopyToDetailLData();

            //チップ詳細(小)の拡大ボタンを表示
            $("#ExpansionButton").css("display", "block");

            //登録ボタンを活性にする
            $("#DetailRegisterBtn").attr("disabled", false);

            //次の操作を実行する
            AfterCallBack();

            //2014/01/16 TMEJ 明瀬 タブレット版SMB チーフテクニシャン機能開発 START
            //部品詳細取得APIでエラーが発生した場合、メッセージを表示する(部品以外は正常表示)
            var errorMsg = $("#PartsDtlErrMsgHidden").val();
            if (errorMsg != "") {
                icropScript.ShowMessageBox("", errorMsg, "");
            }
            //2014/01/16 TMEJ 明瀬 タブレット版SMB チーフテクニシャン機能開発 END
        }
        //整備名の取得後
        else if (jsonResult.Caller == C_SC3240201CALLBACK_GETMERC) {

            //アクティブインジケータ、オーバーレイ非表示
            gDetailSActiveIndicator.hide();
            gDetailOverlay.hide();

            //JSON形式の商品情報を変換する
            var mercDataList = $.parseJSON(htmlDecode(jsonResult.MercJson));

            //チップ詳細(小)　商品コンボボックスを初期化
            var e = document.getElementById("DetailSMercList");
            e.options.length = 0; //コンボボックス内のデータをクリア
            $("#DetailSMercLabel").text("");

            //チップ詳細(大)　商品コンボボックスを初期化
            var f = document.getElementById("DetailLMercList");
            f.options.length = 0; //コンボボックス内のデータをクリア
            $("#DetailLMercLabel").text("");

            //空行を追加
            e.options[0] = new Option("", 0);    //チップ詳細(小)
            f.options[0] = new Option("", 0);    //チップ詳細(大)
            var i = 1;

            //商品コンボボックスにDBから取得した値をセット
            for (var keyString in mercDataList) {
                var mercData = mercDataList[keyString];
                e.options[i] = new Option(mercData.MERC_NAME, mercData.MERCID_TIME);    //チップ詳細(小)
                f.options[i] = new Option(mercData.MERC_NAME, mercData.MERCID_TIME);    //チップ詳細(大)
                i = i + 1;
            }

            //商品コンボボックスを活性にする
            $("#DetailSMercList").attr("disabled", false);
            $("#DetailLMercList").attr("disabled", false);

            // 2015/06/15 TMEJ 河原  サービス入庫予約情報のバリデーション実行タイミング変更に向けた評価用アプリ作成 STRAT

            //商品データが存在する("1"：商品データ有り)
            $("#DetailSMercList").attr("MERCITEM", 1);

            // 2015/06/15 TMEJ 河原  サービス入庫予約情報のバリデーション実行タイミング変更に向けた評価用アプリ作成 END


            //必須項目がEmptyなら登録ボタンを非活性にする
            $("#DetailRegisterBtn").attr("disabled", IsMandatoryChipDetailEmpty());

            //次の操作を実行する
            AfterCallBack();

        }
        //登録ボタンクリック後
        else {
            //アクティブインジケータ、オーバーレイ非表示
            gDetailSActiveIndicator.hide();
            gDetailOverlay.hide();

            //チップ詳細画面を閉じる
            CloseChipDetail(0);

            //どのサブエリアから開いたチップかを特定する
            var subAreaId = GetSubChipType(gSelectedChipId);

            //チップ詳細がサブエリアから開かれた場合
            if (subAreaId != "") {

                //更新したチップが所属するエリアによって、呼び出すリロード用関数を変更する
                switch (subAreaId) {
                    case C_FT_BTNTP_CONFIRMED_RO:
                        //受付サブチップボックス

                        ReceptionAreaReLoad();
                        break;

                    case C_FT_BTNTP_WAIT_CONFIRMEDADDWORK:
                        //追加作業サブチップボックス

                        AddWorkAreaReLoad();
                        break;

                    case C_FT_BTNTP_CONFIRMED_INSPECTION:
                        //完成検査サブチップボックス

                        ComInspectionAreaReLoad();
                        break;

                    case C_FT_BTNTP_WAITING_WASH:
                    case C_FT_BTNTP_WASHING:
                        //洗車サブチップボックス

                        CarWashAreaReLoad();
                        break;

                    case C_FT_BTNTP_WAIT_DELIVERY:
                        //納車サブチップボックス

                        DeliverdCarAreaReLoad();
                        break;

                    case C_FT_BTNTP_NOSHOW:
                        //NoShowサブチップボックス

                        NoShowAreaReLoad();
                        break;

                    case C_FT_BTNTP_STOP:
                        //中断サブチップボックス

                        StopAreaReLoad();
                        break;

                    default:
                        break;
                }
            }
            else {

                //受付エリアのカウントを更新する
                ReceptionButtonRefresh();
            }

            //2015/04/23 TMEJ 明瀬  DMS連携版サービスタブレット強制納車機能追加開発 START

            //DMS除外エラーの警告が発生した場合
            if (resultCD == -9000) {

                //メッセージを表示する
                icropScript.ShowMessageBox(resultCD, jsonResult.Message, "");

            }

            //2015/04/23 TMEJ 明瀬  DMS連携版サービスタブレット強制納車機能追加開発 END

            //更新した後で、更新されたチップの情報を取得する
            var jsonData = htmlDecode(jsonResult.Contents);

            //工程管理メインの表示を最新化する
            // 2013/11/29 TMEJ 張 タブレット版SMB チーフテクニシャン機能開発 START
            //ShowLatestChips(jsonData);
            ShowLatestChips(jsonData, false, false);
            // 2013/11/29 TMEJ 張 タブレット版SMB チーフテクニシャン機能開発 END

            //チップ選択状態を解除
            SetChipUnSelectedStatus();
            SetTableUnSelectedStatus();

            //チップの表示を削除する場合
            //　→更新したチップの表示開始日時or表示終了日時が、メイン画面で表示されている日に含まれない場合
            if (jsonResult.DelDispChip == 1) {

                //チップの表示を削除
                $("#" + String(jsonResult.StallUseId)).remove();
            }

            //リレーションチップの場合、リレーションチップ用の構造体へ開始日時をセット
            if (IsRelationChip(jsonResult.StallUseId) == true) {
                SetChipDetailForRelationChip(jsonData);
            }

            //Newで生成したArrayを削除
            gSC3240201RezIdList = null;
            gSC3240201RezId_StallUseStatusList = null;
        	//2013/12/04 TMEJ 下村 タブレット版SMB チーフテクニシャン機能開発 START
            //gSC3240201RoJobSeq2List = null;
        	gSC3240201RezId_InvisibleInstructFlgList = null;
        	//2013/12/04 TMEJ 下村 タブレット版SMB チーフテクニシャン機能開発 END
            gSC3240201Before_MatchingRezIdList = null;
            gSC3240201MatchingRezIdList = null;
            gSC3240201FixItemCodeList = null;
        	//2013/12/04 TMEJ 下村 タブレット版SMB チーフテクニシャン機能開発 START
            //gSC3240201FixItemSeqList = null;
        	gSC3240201JobinstrucDtlIdList = null;
	    	gSC3240201JobInstructSeqList = null;
	    	gSC3240201JobInstructIdList = null;
        	//2013/12/04 TMEJ 下村 タブレット版SMB チーフテクニシャン機能開発 END
            gSC3240201RoJobSeqList = null;
    		//2013/12/04 TMEJ 下村 タブレット版SMB チーフテクニシャン機能開発 START
            //gSC3240201SrvAddSeqList = null;
    		//2013/12/04 TMEJ 下村 タブレット版SMB チーフテクニシャン機能開発 END

            //次の操作を実行する
            AfterCallBack();
        }
    }
}

/**
* リレーションチップの構造体へ開始日時をセットする
*/
function SetChipDetailForRelationChip(jsonData) {
    var chipDataList = $.parseJSON(jsonData);

    //取得したチップ情報をリレーション分Loop
    for (var strKey in chipDataList) {
        var chipData = chipDataList[strKey];

        var stallUseId = chipData.STALL_USE_ID;

    	//2013/12/04 TMEJ 下村 タブレット版SMB チーフテクニシャン機能開発 START
        ////作業開始実績日時が最小日付の場合、作業開始予定日時をセットする
        //if (IsDefaultDate(new Date(chipData.RSLT_START_DATETIME))) {
        //
        //    gArrObjRelationChip[stallUseId].setStartDateTime(chipData.SCHE_START_DATETIME);
        //}
        //else {
        //    gArrObjRelationChip[stallUseId].setStartDateTime(chipData.RSLT_START_DATETIME);
        //}        
    	
    	// リレーションチップのみ(中断したチップは更新しない)
    	if (gArrObjRelationChip[stallUseId]) {
	        //作業開始実績日時が最小日付の場合、作業開始予定日時をセットする
	        if (IsDefaultDate(new Date(chipData.RSLT_START_DATETIME))) {

	            gArrObjRelationChip[stallUseId].setStartDateTime(chipData.SCHE_START_DATETIME);
	        }
	        else {
	            gArrObjRelationChip[stallUseId].setStartDateTime(chipData.RSLT_START_DATETIME);
	        }
    	}
    	//2013/12/04 TMEJ 下村 タブレット版SMB チーフテクニシャン機能開発 END
    }
}

/**
* 画面を初期化する
*
*/
function InitPage(result, context) {

    //コールバックによって取得したHTMLを設定
    var jsonResult = JSON.parse(result);
    SetChipDetailContents(jsonResult.Contents);

    //チップ詳細(小)に縦スクロールの設定
    $("#ChipDetailSContent").fingerScroll();

    //チップ詳細(大)に縦スクロールの設定
    $("#ChipDetailLContent").fingerScroll();

    //チップ詳細(大)のチップエリア横スクロールの設定
    $("#scrollChip").SC3240201fingerScroll();

    //CustomLabelの適用
    $("#ChipDetailPopup .ChipDetailEllipsis").CustomLabel({ useEllipsis: true });

    //見た目やイベントの設定を行う
    SetChipDetailDisplayAndEvent();

}//InitPage End

/**
* コールバックで取得したHTMLを画面に設定する
* 
* @param {String} cbResult コールバック呼び出し結果
* 
*/
function SetChipDetailContents(cbResult) {

    //コールバックによって取得したHTMLを格納
    var contents = $('<Div>').html(cbResult).text();

    //チップ詳細(小)のコンテンツを取得
    var detailS = $(contents).find('#ChipDetailSContent');

    //チップ詳細(大)のコンテンツを取得
    var detailL = $(contents).find('#ChipDetailLContent');

    //チップ詳細のHiddenコンテンツを取得
    var detailHidden = $(contents).find('#SC3240201HiddenArea');

    //チップ詳細(小)のコンテンツを削除
    $('#ChipDetailSContent>div').remove();

    //チップ詳細(小)のコンテンツを設定
    detailS.children('div').clone(true).appendTo('#ChipDetailSContent');

    //チップ詳細(大)のコンテンツを削除
    $('#ChipDetailLContent>div').remove();

    //チップ詳細(大)のコンテンツを設定
    detailL.children('div').clone(true).appendTo('#ChipDetailLContent');

    //チップ詳細のHiddenコンテンツを削除
    $('#SC3240201HiddenArea>div').remove();

    //チップ詳細のHiddenコンテンツを設定
    detailHidden.children('div').clone(true).appendTo('#SC3240201HiddenArea');

} //SetChipDetailContents End

///**	
//* 工程管理画面に表示されているチップを更新する
//* 	
//*/
//function UpdateSmbChipByChipDetail(updDate, instructedRezIdList) {
//
//    //クライアントに保持している情報を上書きする(DBに設定した値と同期を取るため)
//
//    var planDeli = $("#DetailSPlanDeriveredDateTimeSelector").get(0).valueAsDate;   //納車予定日時(convertHHMMToDtは~/SC3240101/Common.js内のメソッド)
//    var waitType = $("#WaitingFlgHidden").val();                                    //待ち方
//    var rezFlg = $("#RezFlgHidden").val();                                          //予約有無
//    var carWash = $("#CarWashFlgHidden").val();                                     //洗車有無
//
//    var objStallChip;           //選択チップに該当するストール上のチップオブジェクト
//    var selectStallChipId;      //選択中チップのストール上でのID
//    var orderNo;                //整備受注番号
//
//    //サブチップボックスならば数値が返却される
//    var subAreaId = GetSubChipType(gSelectedChipId);
//
//    //チップ詳細がサブチップボックスから開かれている場合
//    if (subAreaId != "") {
//        //現在選択中のサブチップが保持している予約ID・整備受注番号
//        selectStallChipId = gArrObjSubChip[gSelectedChipId].rezId;
//        orderNo = gArrObjSubChip[gSelectedChipId].orderNo;
//        //ストール上のチップオブジェクトを取得
//        objStallChip = gArrObjChip[selectStallChipId];
//    }
//    //チップ詳細がストール上のチップから開かれている場合
//    else {
//        //現在選択中のチップID
//        selectStallChipId = gSelectedChipId;
//        orderNo = gArrObjChip[selectStallChipId].orderNo;
//        //ストール上のチップオブジェクトを取得
//        objStallChip = gArrObjChip[selectStallChipId];
//    }
//
//    //チップ詳細初期表示時点でのチップ表示用日時の差分(分)
//    var initDiffTime = null;
//
//    //ストール上のチップオブジェクトが存在する場合
//    if (!$.isEmptyObject(objStallChip)) {
//        initDiffTime = smbScript.CalcTimeSpan(objStallChip.displayStartDate, objStallChip.displayEndDate);
//    }
//
//    //***** プロパティ設定 Start *****
//
//    //リレーションのチップIDリスト
//    var relationChipIdList;
//
//    //選択中チップが所属するエリアによって下記のように分岐
//    switch (subAreaId) {
//        case C_FT_BTNTP_CONFIRMED_RO:
//        case C_FT_BTNTP_WAIT_CONFIRMEDADDWORK:
//            //受付・追加作業サブチップボックス
//
//            //現在表示中のストールにあるチップIDリストを予約管理連番をキーとして取得する
//            relationChipIdList = FindRelationChips("", gArrObjSubChip[gSelectedChipId].prezSeq);
//            break;
//
//        default:
//            //ストール上・完成検査・洗車・納車サブチップボックス
//
//            //ストール上のチップオブジェクトが存在する場合
//            if (!$.isEmptyObject(objStallChip)) {
//                //選択中チップのみ更新するプロパティ
//                objStallChip.setPlanStartDate($("#DetailSPlanStartDateTimeSelector").get(0).valueAsDate);      //開始予定日時
//                objStallChip.setPlanEndDate($("#DetailSPlanFinishDateTimeSelector").get(0).valueAsDate);       //終了予定日時
//                objStallChip.setRtStartDate($("#DetailSProcessStartDateTimeSelector").get(0).valueAsDate);     //開始実績日時
//                objStallChip.setRtEndDate($("#DetailSProcessFinishDateTimeSelector").get(0).valueAsDate);      //終了実績日時
//                objStallChip.setDisplayStartDate(gSC3240201DisplayChipStartTime);                              //表示用の開始日時
//                objStallChip.setDisplayEndDate(gSC3240201DisplayChipEndTime);                                  //表示用の終了日時
//                //***********************************************************************************************
//                //* $01 整備種類をDropDownListで選択できるようにする対応 Start
//                //***********************************************************************************************
//                //                objStallChip.setFixType($("#DetailSMaintenanceTypeTxt").val());                                //整備種類
//                objStallChip.setFixType($("#DetailSMaintenanceTypeList").val());                                //整備種類
//                //***********************************************************************************************
//                //* $01 整備種類をDropDownListで選択できるようにする対応 End
//                //***********************************************************************************************
//            }
//
//            //チップ詳細で整備に紐付けられるチップ情報がない場合
//            if (gSC3240201RezIdList.length == 0) {
//                //現在表示中のストールにあるチップIDリストを予約IDをキーとして取得する
//                relationChipIdList = FindRelationChips(selectStallChipId, "");
//            }
//            else {
//                //チップ詳細で表示されているリレーションチップも含めた予約IDのリスト
//                relationChipIdList = gSC3240201RezIdList;
//            }
//            break;
//    }
//
//    $.each(relationChipIdList, function (i, val) {
//
//        //予約IDの取得
//        var rezId;
//        if (typeof val == "string") {
//            //relationChipIdListがgSC3240201RezIdList
//            rezId = val;
//        }
//        else {
//            //relationChipIdListがFindRelationChipsメソッドから取得したリレーションチップリスト
//            rezId = val[0];
//        }
//
//        //チップオブジェクトの取得
//        var objChip = gArrObjChip[rezId];
//
//        //チップオブジェクトが表示営業日のストール上にいる場合
//        if (!$.isEmptyObject(objChip)) {
//
//            //追加作業エリア以外から開かれている場合
//            if (subAreaId != C_FT_BTNTP_WAIT_CONFIRMEDADDWORK) {
//                objChip.setUpdateDateSR(updDate);   //T_STALLREZテーブルの更新日時
//            }
//            objChip.setOrderNo(orderNo);        //整備受注番号
//            objChip.setPlanDeliDate(planDeli);  //納車予定日時
//            objChip.setUpdateDateSW(updDate);   //T_SERVICEWORKテーブルの更新日時            
//            objChip.setWaitType(waitType);      //待ち方
//            objChip.setRezFlag(rezFlg);         //予約有無  [ToDo 1or0]
//            objChip.setWashFlag(carWash);       //洗車有無
//
//            //予約IDが整備と紐付く予約IDリスト内に存在しない場合
//            if ($.inArray(String(rezId), instructedRezIdList) < 0) {
//                //着工指示日時をnullにする
//                objChip.setRtInstructDate(null);
//            }
//            //予約IDが整備と紐付く予約IDリスト内に存在する場合
//            else {
//                //そのチップがまだ着工指示日時を保持していない場合
//                if (objChip.rtInstructDate == null) {
//                    //着工指示日時を更新日時にする
//                    objChip.setRtInstructDate(updDate);
//                }
//            }
//            //チップの再描画(updateStallChipは~/SC3240101/ChipPrototype.js内のメソッド)
//            objChip.updateStallChip();
//        }
//    });
//
//    //チップオブジェクトが表示営業日のストール上にいる場合
//    if (!$.isEmptyObject(objStallChip)) {
//        //チップのプロトタイプに残作業時間を設定する(SetChipPrototypeRemainWorkTimeは~/SC3240101/Chip.js内のメソッド)
//        SetChipPrototypeRemainWorkTime(selectStallChipId, initDiffTime);
//        //チップのプロトタイプに作業終了予定日時を設定する(SetChipPrototypeMaxEndDateは~/SC3240101/Chip.js内のメソッド)
//        SetChipPrototypeMaxEndDate(selectStallChipId, new Date(gSC3240201InitDisplayChipEndTime), false);
//        //見込み遅刻時刻により、チップの色を更新する(UpdateChipColorByDelayDateは~/SC3240101/Chip.js内のメソッド)
//        UpdateChipColorByDelayDate(selectStallChipId);
//    }
//
//    //***** プロパティ設定 End *****
//
//    //受付・追加作業サブチップボックス以外からチップ詳細が開かれている場合
//    if (subAreaId != C_FT_BTNTP_CONFIRMED_RO && subAreaId != C_FT_BTNTP_WAIT_CONFIRMEDADDWORK) {
//        //ストール上のチップオブジェクトが存在する場合
//        if (!$.isEmptyObject(objStallChip)) {
//            //チップを移動する(SetChipPositionは~/SC3240101/Chip.js内のメソッド)
//            SetChipPosition(selectStallChipId, "", "", "");
//        }
//    }
//
//    //チップ詳細がサブチップボックスから開かれている場合
//    if (subAreaId != "") {
//        //サブチップボックスをリロードする(SubChipAreaReLoadは~/SC3500400/SubChip.js内のメソッド)
//        SubChipAreaReLoad(gSelectedChipId);
//        //選択したチップを解放する(SetTableUnSelectedStatusは~/SC3240101/Table.js内のメソッド)
//        SetTableUnSelectedStatus(gSelectedChipId);
//        //チップ選択状態を解除する(SetChipUnSelectedStatusは~/SC3240101/Chip.js内のメソッド)
//        SetChipUnSelectedStatus();
//    }
//    //ストール上のチップから開かれている場合
//    else {
//        //選択したチップを解放する(SetTableUnSelectedStatusは~/SC3240101/Table.js内のメソッド)
//        SetTableUnSelectedStatus(gSelectedChipId);
//        //チップ選択状態を解除する(SetChipUnSelectedStatusは~/SC3240101/Chip.js内のメソッド)
//        SetChipUnSelectedStatus();
//        //受付ボタンのカウントをリフレッシュする
//        ReceptionButtonRefresh();
//    }
//}

/**	
* チップ詳細内のマーク表示設定を行う
* 	
*/
function SetMarkChipDetail() {

    //予約マーク（0:予約客／1:Walk-in）
    if ($("#RezFlgHidden").val() != "0") {
        $("#DetailSIcnD").css("display", "none");
        $("#DetailLIcnD").css("display", "none");
    }

    //JDP調査対象客マーク
    //2018/06/26 NSK 可児 TKM Next Gen e-CRB Project Application development Block B-1  Pマーク、L マークなどを表示 START
    //if ($("#JDPMarkFlgHidden").val() == "0") {
    //    $("#DetailSIcnI").css("display", "none");
    //    $("#DetailLIcnI").css("display", "none");
    //P/Lマーク（1:Pマーク／2:Lマーク）
    if ($("#JDPMarkFlgHidden").val() == "1") {
        $("#DetailSIcnL").css("display", "none");
        $("#DetailLIcnL").css("display", "none");
    } else if ($("#JDPMarkFlgHidden").val() == "2") {
        $("#DetailSIcnP").css("display", "none");
        $("#DetailLIcnP").css("display", "none");
    } else {
        $("#DetailSIcnP").css("display", "none");
        $("#DetailLIcnP").css("display", "none");
        $("#DetailSIcnL").css("display", "none");
        $("#DetailLIcnL").css("display", "none");
    }
    //2018/06/26 NSK 可児 TKM Next Gen e-CRB Project Application development Block B-1  Pマーク、L マークなどを表示 END

    //SSCマーク
    if ($("#SSCMarkFlgHidden").val() == "0") {
        $("#DetailSIcnS").css("display", "none");
        $("#DetailLIcnS").css("display", "none");
    }
}

/**	
* 納車予定変更履歴エリアの初期設定を行う
* 	
*/
function InitDeliHisArea() {

    chipDetailSlideDownFlag = false;

    // 納車時刻の変更回数が0以外の場合
    if ($("#DeliveryPlanUpdateCountHidden").val() != "0") {

        // チップ詳細(小)ステータスエリアをタップした場合
        $("#DetailSChipStatusDiv").click(function () {
            if (chipDetailSlideDownFlag) {
                // スライドアップ（チップ詳細(大)のスライドアップは効かない為、ExpandDisplay()で別途フェードアウト）
                SlideUpDetailS();
                chipDetailSlideDownFlag = false;
            } else {
                // スライドダウン
                $("#DetailSHeadInfomationPullDiv").slideDown();
                $("#DetailLHeadInfomationPullDiv").slideDown();
                chipDetailSlideDownFlag = true;
            }
        });

        // チップ詳細(小)履歴エリアをタップした場合
        $("#DetailSHeadInfomationPullDiv").click(function () {
            // スライドアップ（チップ詳細(大)のスライドアップは効かない為、ExpandDisplay()で別途フェードアウト）
            SlideUpDetailS();
            chipDetailSlideDownFlag = false;
        });


        // チップ詳細(大)ステータスエリアをタップした場合
        $("#DetailLChipStatusDiv").click(function () {
            if (chipDetailSlideDownFlag) {
                // スライドアップ（チップ詳細(小)のスライドアップは効かない為、ShrinkDisplay()で別途フェードアウト）
                SlideUpDetailL();
                chipDetailSlideDownFlag = false;
            } else {
                // スライドダウン
                $("#DetailLHeadInfomationPullDiv").slideDown();
                $("#DetailSHeadInfomationPullDiv").slideDown();
                chipDetailSlideDownFlag = true;
            }
        });

        // チップ詳細(大)履歴エリアをタップした場合
        $("#DetailLHeadInfomationPullDiv").click(function () {
            // スライドアップ（チップ詳細(小)のスライドアップは効かない為、ShrinkDisplay()で別途フェードアウト）
            SlideUpDetailL();
            chipDetailSlideDownFlag = false;
        });
    } else {

        // ステータスエリアの▼マークを非表示にする
        $("#DetailSTriangleLabel").css("display", "none");
        $("#DetailLTriangleLabel").css("display", "none");
    }
}


/**	
* チップ詳細(小)slideUp処理
* 	
*/
function SlideUpDetailS() {

    $("#DetailSHeadInfomationPullDiv").slideUp();
    $('.detailSInnerDataBox02 .scroll-inner').css({
        'transform': 'translate3d(0px, 0px, 0px)',
        '-webkit-transition': 'transform 400ms'
    });
}

/**	
* チップ詳細(大)slideUp処理
* 	
*/
function SlideUpDetailL() {

    $("#DetailLHeadInfomationPullDiv").slideUp();
    $('.detailLInnerDataBox .scroll-inner').css({
        'transform': 'translate3d(0px, 0px, 0px)',
        '-webkit-transition': 'transform 400ms'
    });
}


/**	
* チップ詳細(小)のチップエリアの初期設定を行う
* 	
*/
function InitDetailSChipArea() {

    var detailSTableStallDl = $("#detailSTableChipUl dl");
    var copyChipInfo = $("#detailSTableChipUl .Cassette")[0].innerHTML;
    var chipInfoDivList;
    var selectChipText = "";

    detailSTableStallDl.each(function (i, elem) {

        //No.が1の行にしかチップ情報が描画されていないため、他の行にもコピーする
        if (1 < i) {
            $(this).find(".Cassette").children("span").remove();
            $(this).find(".Cassette").append(copyChipInfo);
        }

        //それぞれの整備がどのチップに紐付いているかを確認し、チェックマークを付ける
        chipInfoDivList = $(this).find(".Cassette div");

        chipInfoDivList.each(function (j, divElem) {

            //整備の行に保持している予約ID(整備に紐付いている予約ID)と
            //チップリスト内の予約IDが等しい場合、チップリスト内の該当する予約IDのdivにチェックを付ける
            if ($(divElem).attr("rezid") == $(elem).attr("selectrezid")) {
                $(divElem).addClass(C_SC3240201CLASS_CHECKBLUE);
            }

            //各チップリストの最終行には未選択行クラスを追加
            if ($(chipInfoDivList).length - 1 == j) {
                $(divElem).addClass("Unselected");
            }
        });

        //複数行エリア表示時の高さを属性として保存しておく
        $(this).attr("maxH", this.clientHeight - 39);

        //開閉状態のステータスを属性として作成
        $(this).attr("openFlg", "0");

        //選択中のチップのインデックスを属性として保存しておく
        $(this).attr("selectchipindex", $(this).find(".CheckBlue").attr("chipindex"));

        //１行表示時の高さを設定
        $(this).height(29);

        //選択されているチップ情報を１行表示用ラベルに設定
        if ($(this).attr("selectrezid") == "-1") {
            //未選択
            selectChipText = $("#WordChipUnselectedHidden").val();

            //太字
            $(this).find("dd").children(".SingleLine").css("font-weight", "bold");
        }
        else {
            selectChipText = $(this).find(".CheckBlue").children("span").text();
        }
        $(this).find("dd").children(".SingleLine").text(selectChipText);
    });
}

/**	
* チップ詳細(大)のチップエリアの初期設定を行う	
* 	
*/
function InitDetailLChipArea() {

    var detailLChipTr = $("#ChipDetailPopupContent .detailLTableChip2 #chipInfoTable .chipCheck");
    var copyCheckArea = detailLChipTr[0].innerHTML;
    var chipInfoDivList;

    detailLChipTr.each(function (i, elem) {

        //チップ情報が取得できている場合
        if (0 < $(detailLChipTr[0]).children("td").length) {

            //No.が1の行にしかチップをチェックできるエリアが描画されていないため、他の行にもコピーする
            if (1 < $(this).index()) {
                $(this).append(copyCheckArea);
            }

            //それぞれの整備がどのチップに紐付いているかを確認し、チェックマークを付ける
            chipInfoDivList = $(this).find("div");

            chipInfoDivList.each(function (j, divElem) {

                //整備の行に保持している予約ID(整備に紐付いている予約ID)と
                //チップリスト内の予約IDが等しい場合、チップリスト内の該当する予約IDのdivにチェックを付ける
                if ($(divElem).attr("rezid") == $(elem).attr("selectrezid")) {
                    $(divElem).addClass(C_SC3240201CLASS_CHECKBLUE);
                }
            });

            //青チェックがある場合
            if (0 < $(this).find(".CheckBlue").length) {
                $(this).attr("selectChipIndex", $(this).find(".CheckBlue").attr("chipindex"));
            }
            else {
                $(this).attr("selectChipIndex", "0");
            }
        }
        //チップ情報が取得できていない場合
        else {
            //高さの調整(これがないと線がずれる)
            $(this).height(30);
        }
    });
}

//2014/07/15 TMEJ 丁 タブレットSMB Job Dispatch機能開発 START
/**	
* チップ詳細(大)のアクションボタン制御
* 	
*/
function InitDetailLActionButton() {

    gBeforeEndWorkCount = 0;
    var detailSTableStallDl = $(".detailLTableChip dl");
    //開始前Jobフラグ
    gBeforeStartFlg = false;
    //開始中Jobフラグ
    var blWorkingFlg = false;
    //サービスステータス
    var strSvcStatus;
    //ストール使用ステータス
    var strStalluseStatus;
    //仮置きフラグ
    var strTemFlg;
    //着工指示フラグ
    var blInstructFlg = false;
    //予定開始日時
    var scheStartDateTime;
    //実績終了日時
    var rsltEndDateTime;
    //選択されたチップのJobDtlId
    var selectrezid;

    //2014/12/08 TMEJ 丁 DMS連携版サービスタブレット JobDispatch完成検査入力制御開発 START
    //中断Jobのカウント
    var nStopJobCount = 0;
    //2014/12/08 TMEJ 丁 DMS連携版サービスタブレット JobDispatch完成検査入力制御開発 END

    //ボタンのイベントバイト
    //チップ詳細(大)のAllStartボタンクリックイベント登録
    $("#AllStart").unbind().bind("click", function (e) {
        gSC3240201JobInstructSeqList = new Array();
        gSC3240201JobInstructIdList = new Array();
        ClickBtnStartByDetail("AllStart");
    });

    //チップ詳細(大)のSingleStartボタンクリックイベント登録
    $(".BtnBoxSingle .SingleStart").unbind().bind("click", function (e) {
        gSC3240201JobInstructSeqList = new Array();
        gSC3240201JobInstructIdList = new Array();
        gSC3240201JobInstructIdList.push($(e.target.parentElement.parentElement.parentElement).attr("jobinstructid"));
        gSC3240201JobInstructSeqList.push($(e.target.parentElement.parentElement.parentElement).attr("jobinstructseq"));
        ClickBtnStartByDetail("SingleStart");
    });
    //チップ詳細(大)のSingleStartボタンクリックイベント登録
    $(".BtnBoxSingle .SingleReStart").unbind().bind("click", function (e) {
        gSC3240201JobInstructSeqList = new Array();
        gSC3240201JobInstructIdList = new Array();
        gSC3240201JobInstructIdList.push($(e.target.parentElement.parentElement.parentElement).attr("jobinstructid"));
        gSC3240201JobInstructSeqList.push($(e.target.parentElement.parentElement.parentElement).attr("jobinstructseq"));
        ClickBtnStartByDetail("ReStart");
    });

    //2014/12/08 TMEJ 丁 DMS連携版サービスタブレット JobDispatch完成検査入力制御開発 START
    //チップ詳細(大)のAllFinishボタンクリックイベント登録
//    $("#AllFinish").unbind().bind("click", function (e) {
//        gFinishChipFig = "1";
//        ClickBtnFinishByDetail("AllFinish");
//    });
    //2014/12/08 TMEJ 丁 DMS連携版サービスタブレット JobDispatch完成検査入力制御開発 END

    //チップ詳細(大)のSingleFinishボタンクリックイベント登録
    $(".BtnBoxSingle .SingleFinish").unbind().bind("click", function (e) {
        var nJobInstructId = $(e.target.parentElement.parentElement.parentElement).attr("jobinstructid");
        var nJobInstructSeq = $(e.target.parentElement.parentElement.parentElement).attr("jobinstructseq");
        if (gBeforeEndWorkCount > 1) {
            gFinishChipFig = "0";
        } else {
            gFinishChipFig = "1";
        }
        ClickBtnFinishByDetail("SingleFinish", nJobInstructId, nJobInstructSeq);
    });

    //チップ詳細(大)のAllStopボタンクリックイベント登録
    $("#AllStop").unbind().bind("click", function (e) {
        gSC3240201JobInstructSeqList = new Array();
        gSC3240201JobInstructIdList = new Array();
        if (gBeforeStartFlg) {
            gStopChipFig = "0";
        } else {
            gStopChipFig = "1";
        }
        ClickBtnStopJobByDetail();
    });

    //チップ詳細(大)のSingleStopボタンクリックイベント登録
    $(".BtnBoxSingle .SingleStop").unbind().bind("click", function (e) {

        gSC3240201JobInstructSeqList = new Array();
        gSC3240201JobInstructIdList = new Array();
        gSC3240201JobInstructIdList.push($(e.target.parentElement.parentElement.parentElement).attr("jobinstructid"));
        gSC3240201JobInstructSeqList.push($(e.target.parentElement.parentElement.parentElement).attr("jobinstructseq"));
        if (gBeforeEndWorkCount > 1) {
            gStopChipFig = "0";
        } else {
            gStopChipFig = "1";
        }
        ClickBtnStopJobByDetail();
    });
    

    if (CheckgArrObjChip(gSelectedChipId)) {
        strSvcStatus = gArrObjChip[gSelectedChipId].svcStatus;
        strTemFlg = gArrObjChip[gSelectedChipId].tempFlg;
        rsltEndDateTime = gArrObjChip[gSelectedChipId].rsltEndDateTime;
        scheStartDateTime = gArrObjChip[gSelectedChipId].scheStartDateTime;
        strStalluseStatus = gArrObjChip[gSelectedChipId].stallUseStatus;
        selectrezid = gArrObjChip[gSelectedChipId].jobDtlId;
    } else {
        strSvcStatus = gArrObjSubChip[gSelectedChipId].svcStatus;
        strTemFlg = gArrObjSubChip[gSelectedChipId].tempFlg;
        rsltEndDateTime = gArrObjSubChip[gSelectedChipId].rsltEndDateTime;
        scheStartDateTime = gArrObjSubChip[gSelectedChipId].scheStartDateTime;
        strStalluseStatus = gArrObjSubChip[gSelectedChipId].stallUseStatus;
        selectrezid = gArrObjSubChip[gSelectedChipId].jobDtlId;
    }
    //チップ着工指示フラグ
    if ((strStalluseStatus >= C_STALLUSE_STATUS_STARTWAIT) && (strStalluseStatus <= C_STALLUSE_STATUS_MIDFINISH)) {
        blInstructFlg = true;
    }

    //開始中かどうか
    if ((strStalluseStatus == "02") || (strStalluseStatus == "04")) {
        //既に開始中の場合、SingleStartはチップ状態へんこうしない
        gStartChipFig = "0";
    } else {
        //既に開始中の場合、SingleStartはチップ状態へんこうしない
        gStartChipFig = "1";
    }

    //2014/12/08 TMEJ 丁 DMS連携版サービスタブレット JobDispatch完成検査入力制御開発 START
    //All Finishボタンを常時非活性にする
    $("#AllFinish").addClass("BtnOff");
    //2014/12/08 TMEJ 丁 DMS連携版サービスタブレット JobDispatch完成検査入力制御開発 END

    if ((strSvcStatus == "00") ||
    (strSvcStatus == "01") ||
    (strTemFlg == "1") ||
    (!blInstructFlg) ||
    (strStalluseStatus == "05") ||
    (!IsDefaultDate(rsltEndDateTime)) ||
    (IsDefaultDate(scheStartDateTime))) {
        //全アクションボタンを非活性化
        $("#AllStart").addClass("BtnOff");

        //2014/12/08 TMEJ 丁 DMS連携版サービスタブレット JobDispatch完成検査入力制御開発 START
        //$("#AllFinish").addClass("BtnOff");
        //2014/12/08 TMEJ 丁 DMS連携版サービスタブレット JobDispatch完成検査入力制御開発END

        $("#AllStop").addClass("BtnOff");

        $(".BtnBoxSingle .SingleStart").addClass("BtnOff");
        $(".BtnBoxSingle .SingleFinish").addClass("BtnOff");
        $(".BtnBoxSingle .SingleStop").addClass("BtnOff");
        $(".BtnBoxSingle .SingleReStart").addClass("BtnOff");

        //Clickイベント解除する
        $("#AllStart").unbind();

        //2014/12/08 TMEJ 丁 DMS連携版サービスタブレット JobDispatch完成検査入力制御開発 START
        //$("#AllFinish").unbind();
        //2014/12/08 TMEJ 丁 DMS連携版サービスタブレット JobDispatch完成検査入力制御開発 END

        $("#AllStop").unbind();
        $(".BtnBoxSingle .SingleStart").unbind();
        $(".BtnBoxSingle .SingleFinish").unbind();
        $(".BtnBoxSingle .SingleStop").unbind();
        $(".BtnBoxSingle .SingleReStart").unbind();

        //チップ列の背景色をグレーに変更
        detailSTableStallDl.each(function (i, elem) {
            if ($(this).attr("jobstatus") == "2") {
                $(this).find(".BtnBoxSingle .SingleStop")[0].parentNode.outerHTML = "";
                $(this).find(".BtnBoxSingle .SingleReStart").css("display", "block");

            }
            if ($(this).attr("jobstatus") != "-1") {
                if ($(this).find(".BtnBoxSingle .SingleStart").length > 0) {
                    //チップ列の背景色をグレーに変更
                    $(this).find("p").addClass(C_SC3240201CLASS_BACKGROUNDGRAY);
                }
            }
        });
    } else {
        //シングルボタンの制御
    detailSTableStallDl.each(function (i, elem) {
        if (($(this).attr("jobstatus") == "-1") && ($(this).attr("selectrezid") == selectrezid)) {
            $(this).find(".BtnBoxSingle .SingleStart").addClass("BtnOn");
            $(this).find(".BtnBoxSingle .SingleFinish").addClass("BtnOff");
            $(this).find(".BtnBoxSingle .SingleStop").addClass("BtnOff");
            $(this).find(".BtnBoxSingle .SingleReStart").addClass("BtnOff");

            //Clickイベント解除する
            $(this).find(".BtnBoxSingle .SingleReStart").unbind();
            $(this).find(".BtnBoxSingle .SingleFinish").unbind();
            $(this).find(".BtnBoxSingle .SingleStop").unbind();

            gBeforeStartFlg = true;
            gBeforeEndWorkCount = gBeforeEndWorkCount + 1;
        } else if (($(this).attr("jobstatus") == "0") && ($(this).attr("selectrezid") == selectrezid)) {
            $(this).find(".BtnBoxSingle .SingleFinish").addClass("BtnOn");
            $(this).find(".BtnBoxSingle .SingleStart").addClass("BtnOff");
            $(this).find(".BtnBoxSingle .SingleStop").addClass("BtnOn");
            $(this).find(".BtnBoxSingle .SingleReStart").addClass("BtnOff");
            if ($(this).find(".BtnBoxSingle .SingleStart").length > 0) {
                //チップ列の背景色をグレーに変更
                $(this).find("p").addClass(C_SC3240201CLASS_BACKGROUNDGRAY);

                //Clickイベント解除する
                $(this).find(".BtnBoxSingle .SingleReStart").unbind();
                $(this).find(".BtnBoxSingle .SingleStart").unbind();
            }
            blWorkingFlg = true;
            gBeforeEndWorkCount = gBeforeEndWorkCount + 1;
        } else if (($(this).attr("jobstatus") == "2") && ($(this).attr("selectrezid") == selectrezid)) {
            $(this).find(".BtnBoxSingle .SingleStop")[0].parentNode.outerHTML = "";
            $(this).find(".BtnBoxSingle .SingleReStart").css("display", "block");
            $(this).find(".BtnBoxSingle .SingleReStart").addClass("BtnOn");
            $(this).find(".BtnBoxSingle .SingleFinish").addClass("BtnOff");
            $(this).find(".BtnBoxSingle .SingleStart").addClass("BtnOff");

            //2014/12/08 TMEJ 丁 DMS連携版サービスタブレット JobDispatch完成検査入力制御開発 START
            nStopJobCount = nStopJobCount + 1;
            //2014/12/08 TMEJ 丁 DMS連携版サービスタブレット JobDispatch完成検査入力制御開発 END

            if ($(this).find(".BtnBoxSingle .SingleStart").length > 0) {
                //チップ列の背景色をグレーに変更
                $(this).find("p").addClass(C_SC3240201CLASS_BACKGROUNDGRAY);

                //Clickイベント解除する
                $(this).find(".BtnBoxSingle .SingleFinish").unbind();
                $(this).find(".BtnBoxSingle .SingleStart").unbind();
            }
        } else {
            if ($(this).find(".BtnBoxSingle .SingleStart").length > 0) {
                $(this).find(".BtnBoxSingle .SingleReStart").addClass("BtnOff");
                $(this).find(".BtnBoxSingle .SingleFinish").addClass("BtnOff");
                $(this).find(".BtnBoxSingle .SingleStart").addClass("BtnOff");
                $(this).find(".BtnBoxSingle .SingleStop").addClass("BtnOff");
                //チップ列の背景色をグレーに変更
                $(this).find("p").addClass(C_SC3240201CLASS_BACKGROUNDGRAY);

                $(this).find(".BtnBoxSingle .SingleStart").unbind();
                $(this).find(".BtnBoxSingle .SingleFinish").unbind();
                $(this).find(".BtnBoxSingle .SingleStop").unbind();
                $(this).find(".BtnBoxSingle .SingleReStart").unbind();
            }
        }
    });

        if (gBeforeStartFlg) {
            $("#AllStart").addClass("BtnOn");
        } else {
            $("#AllStart").addClass("BtnOff");
            $("#AllStart").unbind();
        }

        //2014/12/08 TMEJ 丁 DMS連携版サービスタブレット JobDispatch完成検査入力制御開発 START
        //if ((blWorkingFlg) && (!gBeforeStartFlg)) {
        //    $("#AllFinish").addClass("BtnOn");
        //} else {
        //    $("#AllFinish").addClass("BtnOff");
        //    $("#AllFinish").unbind();
        //}

        //単独JOB終了によるチップ終了をできなくなる
        if ((gBeforeEndWorkCount == 1) && (nStopJobCount == 0)) {
            $(".BtnBoxSingle .SingleFinish").addClass("BtnOff");
            $(".BtnBoxSingle .SingleFinish").unbind();
        }
        //2014/12/08 TMEJ 丁 DMS連携版サービスタブレット JobDispatch完成検査入力制御開発 END

        if (blWorkingFlg) {
            $("#AllStop").addClass("BtnOn");
        } else {
            $("#AllStop").addClass("BtnOff");
            $("#AllStop").unbind();
        }
    }

}

/**	
* 追加作業チップ詳細(大)JobDisPatchのレイアウト調整を行う
* 	
*/
function SetChipDetialLayoutForAddWorkArea() {
    $("#detailLMaintenanceNoCstApproveLi").css("height", "29px");
    $(".ChipDetailPopStyle .detailLTableChip dt, .ChipDetailPopStyle .detailLTableChip dd").css("line-height", "29px");
    $(".ChipDetailPopStyle .detailLTableChip2 .detailLTitleCassette").height(28);
    $("#detailLMaintenanceNoCstApproveLi2").css("height", "29px");
    $("#DetailLMaintenanceItemsWordLabel").css("line-height", "61px");
    $("#DetailLMaintenanceNoWordLabel").css("line-height", "61px");    
}

//2014/07/15 TMEJ 丁 タブレットSMB Job Dispatch機能開発 END

/**	
* チップ詳細(小)チップエリアの太字・細字・背景色の設定を行う
* 	
*/
function SetOtherDetailSChipArea() {

    var detailSTableStallDl = $("#detailSTableChipUl dl");

    detailSTableStallDl.each(function (i, elem) {

        //自チップの作業内容ID　＝　整備に紐付いている作業内容ID　の場合
        if (($(elem).attr("selectrezid") == undefined) || ($("#MyJobDtlIdHidden").val() == $(elem).attr("selectrezid"))) {
            //自チップは太字
            $(elem).find("dd").addClass(C_SC3240201CLASS_FONTBOLD);
        }
        else {
            //他チップは細字
            $(elem).find("dd").addClass(C_SC3240201CLASS_FONTNORMAL);
        }

        //それぞれの整備がどのチップに紐付いているかを確認し、書式をセットする
        chipInfoDivList = $(this).find(".Cassette div");

        chipInfoDivList.each(function (j, divElem) {

            //自チップの作業内容ID　＝　チップリスト内の作業内容ID、もしくは未選択　の場合
            if (($("#MyJobDtlIdHidden").val() == $(divElem).attr("rezid")) || ($(divElem).attr("rezid") == "-1") || $(divElem).hasClass("Unselected")) {
                //自チップは太字
                $(divElem).addClass(C_SC3240201CLASS_FONTBOLD);
            }
            else {
                //他チップは細字
                $(divElem).addClass(C_SC3240201CLASS_FONTNORMAL);
            }

        	//2013/12/04 TMEJ 下村 タブレット版SMB チーフテクニシャン機能開発 START
            ////チップリスト内のストール利用ステータス　＝　03：完了　の場合
            //if ($(divElem).attr("stallusestatus") == "03") { 
            //チップリスト内のストール利用ステータス = 03：完了 の場合、または、整備の作業ステータス = 0：作業中、1:完了、2:中断 の場合
            if (($(divElem).attr("stallusestatus") == "03") || 
            	($(elem).attr("jobstatus") == "0") || 
            	($(elem).attr("jobstatus") == "1") || 
            	($(elem).attr("jobstatus") == "2")) {
            //2013/12/04 TMEJ 下村 タブレット版SMB チーフテクニシャン機能開発 END

                //チップ列の背景色をグレーに変更
                $(divElem).addClass(C_SC3240201CLASS_BACKGROUNDGRAY);

                //チップ列の文字を黒に変更
                $(divElem).addClass(C_SC3240201CLASS_TEXTBLACK);
                //$(divElem).find("dd").children(".SingleLine").css("color", "#000");

                //$(divElem).removeClass(C_SC3240201CLASS_TEXTBLUE).addClass(C_SC3240201CLASS_TEXTBLACK);
                //$("#DetailSStallSingleLineLabel").removeClass(C_SC3240201CLASS_TEXTBLUE).addClass(C_SC3240201CLASS_TEXTBLACK);
                //$(divElem).find("dd").children(".SingleLine").css("color", "#000");

                if ($(divElem).attr("rezid") == $(elem).attr("selectrezid")) {
                    //整備列の背景色をグレーに変更
                    //$(elem).find("dd").addClass(C_SC3240201CLASS_BACKGROUNDGRAY); //children
                    $(elem).children("dd").addClass(C_SC3240201CLASS_BACKGROUNDGRAY); //children
                }
            }
            else {
                //チップ列の文字を青に変更
                $(divElem).addClass(C_SC3240201CLASS_TEXTBLUE);
                //$(divElem).find("dd").children(".SingleLine").css("color", "#324f85");
            }
        });

        //１行表示用ラベルにフォント色を設定
        if ($(this).find("dd").hasClass(C_SC3240201CLASS_BACKGROUNDGRAY)) {
            //完了チップは黒字
            $(this).find("dd").children(".SingleLine").css("color", "#000");
        }
        else {
            //未完了チップは青字
            $(this).find("dd").children(".SingleLine").css("color", "#324f85");
        }

    });
}

/**	
* チップ詳細(大)チップエリアの太字・細字・背景色の設定を行う
* 	
*/
function SetOtherDetailLChipArea() {

    var detailLTableStallDl = $("#detailLTableChipUl dl");

    detailLTableStallDl.each(function (i, elem) {

        //自チップの作業内容ID　＝　整備に紐付いている作業内容ID　の場合
        if (($(elem).attr("selectrezid") == undefined) || ($("#MyJobDtlIdHidden").val() == $(elem).attr("selectrezid"))) {
            //自チップは太字
            $(elem).find("dd").addClass(C_SC3240201CLASS_FONTBOLD);
        }
        else {
            //他チップは細字
            $(elem).find("dd").addClass(C_SC3240201CLASS_FONTNORMAL);
        }
        //2013/12/04 TMEJ 下村 タブレット版SMB チーフテクニシャン機能開発 START
        ////整備に紐付いている予約のストール利用ステータス　＝　03：完了　の場合
        //if ($(elem).attr("stallusestatus") == "03") {
        //整備に紐付いている予約のストール利用ステータス = 03：完了 の場合、または、整備の作業ステータス = 0：作業中、1:完了、2:中断 の場合
        if (($(elem).attr("stallusestatus") == "03") || 
        	($(elem).attr("jobstatus") == "0") || 
        	($(elem).attr("jobstatus") == "1") || 
        	($(elem).attr("jobstatus") == "2")) {
        //2013/12/04 TMEJ 下村 タブレット版SMB チーフテクニシャン機能開発 END

            //整備行の背景色をグレーに変更
            $(elem).find("dd").addClass(C_SC3240201CLASS_BACKGROUNDGRAY);

            //チップエリアの全ての列の該当行を背景灰色に変更
            var chipInfoTr = $("#ChipDetailPopupContent .detailLTableChip2").find("tr");
            chipInfoTr.each(function (j, elem2) {

                if (($(elem2).attr("rowindex") != undefined) && ($(elem2).attr("rowindex") == i)) {
                    $(elem2).addClass(C_SC3240201CLASS_BACKGROUNDGRAY);
                	//2013/12/04 TMEJ 下村 タブレット版SMB チーフテクニシャン機能開発 START
                	var chipInfoDivList2 = $(elem2).find("div")
                	chipInfoDivList2.each(function (k, elem3) {
	                	//青色のチェックマークが付いていた場合
	                	if ($(elem3).hasClass(C_SC3240201CLASS_CHECKBLUE)) {
			                //チェックマークを灰色に変更
			                $(elem3).removeClass(C_SC3240201CLASS_CHECKBLUE).addClass(C_SC3240201CLASS_CHECKBLACK);
			            }
    				});
                	//2013/12/04 TMEJ 下村 タブレット版SMB チーフテクニシャン機能開発 END
                }        
            });
        }
    });

    var chipInfoDivList = $("#ChipDetailPopupContent .detailLTableChip2").find("td");
    chipInfoDivList.each(function (j, divElem) {

        //チップリスト内のストール利用ステータス　＝　03：完了　の場合
        if ($(divElem).find("div").attr("stallusestatus") == "03") { 

            //チップエリアの該当フィールドを背景灰色に変更（縦列）
            $(divElem).addClass(C_SC3240201CLASS_BACKGROUNDGRAY);

            //青色のチェックマークが付いていた場合
            if ($(divElem).find("div").hasClass(C_SC3240201CLASS_CHECKBLUE)) {
                //チェックマークを灰色に変更
                $(divElem).find("div").removeClass(C_SC3240201CLASS_CHECKBLUE).addClass(C_SC3240201CLASS_CHECKBLACK);
            }
        }
    });
}

/**	
* チップ詳細(小・大)の予約有無エリアの設定を行う
*
* @param {Boolean} isEditable(True:編集可能/False:編集不可)
* @return {-} -
*
*/
function SetDetailReservationArea(isEditable) {

    //予約有無が変更可能の場合
    if (isEditable) {
        //選択エリアを青文字テキストにする
        $("#DetailSReserveLi dd").addClass(C_SC3240201CLASS_TEXTBLUE);
        $("#DetailLReserveLi dd").addClass(C_SC3240201CLASS_TEXTBLUE);

        //予約有無の判断
        if ($("#RezFlgHidden").val() != "1") {
            //「予約」に青チェック
            $("#DetailSReserveLi dd:first").addClass(C_SC3240201CLASS_CHECKBLUE);
            $("#DetailLReserveLi dd:first").addClass(C_SC3240201CLASS_CHECKBLUE);
        }
        else {
            //「飛び込み」に青チェック
            $("#DetailSReserveLi dd:last").addClass(C_SC3240201CLASS_CHECKBLUE);
            $("#DetailLReserveLi dd:last").addClass(C_SC3240201CLASS_CHECKBLUE);
        }

        //予約有無エリアにイベントの登録
        SetEventDetailReservationArea();

    }
    //予約有無が変更不可の場合
    else {
        //選択エリアを黒文字テキストにする
        $("#DetailSReserveLi dd").addClass(C_SC3240201CLASS_TEXTBLACK);
        $("#DetailLReserveLi dd").addClass(C_SC3240201CLASS_TEXTBLACK);

        //予約有無の判断
        if ($("#RezFlgHidden").val() != "1") {
            //「予約」に黒チェック
            $("#DetailSReserveLi dd:first").addClass(C_SC3240201CLASS_CHECKBLACK);
            $("#DetailLReserveLi dd:first").addClass(C_SC3240201CLASS_CHECKBLACK);
        }
        else {
            //「飛び込み」に黒チェック
            $("#DetailSReserveLi dd:last").addClass(C_SC3240201CLASS_CHECKBLACK);
            $("#DetailLReserveLi dd:last").addClass(C_SC3240201CLASS_CHECKBLACK);
        }
    }
} //SetDetailReservationArea End

/**	
* チップ詳細(小・大)の洗車有無エリアの設定を行う
* 	
* @param {Boolean} isEditable(True:編集可能/False:編集不可)
* @return {-} -
*
*/
function SetDetailCarWashArea(isEditable) {

    //洗車有無が変更可能の場合
    if (isEditable) {
        //選択エリアを青文字テキストにする
        $("#DetailSCarWashLi dd").addClass(C_SC3240201CLASS_TEXTBLUE);
        $("#DetailLCarWashLi dd").addClass(C_SC3240201CLASS_TEXTBLUE);

        //洗車有無の判断
        if ($("#CarWashFlgHidden").val() == "1") {
            //「有り」に青チェック
            $("#DetailSCarWashLi dd:first").addClass(C_SC3240201CLASS_CHECKBLUE);
            $("#DetailLCarWashLi dd:first").addClass(C_SC3240201CLASS_CHECKBLUE);
        }
        else {
            //「無し」に青チェック
            $("#DetailSCarWashLi dd:last").addClass(C_SC3240201CLASS_CHECKBLUE);
            $("#DetailLCarWashLi dd:last").addClass(C_SC3240201CLASS_CHECKBLUE);
        }

        //洗車有無エリアにイベントの設定
        SetEventDetailCarWashArea();

    }
    //洗車有無が変更不可の場合
    else {
        //選択エリアを黒文字テキストにする
        $("#DetailSCarWashLi dd").addClass(C_SC3240201CLASS_TEXTBLACK);
        $("#DetailLCarWashLi dd").addClass(C_SC3240201CLASS_TEXTBLACK);

        //洗車有無の判断
        if ($("#CarWashFlgHidden").val() == "1") {
            //「有り」に黒チェック
            $("#DetailSCarWashLi dd:first").addClass(C_SC3240201CLASS_CHECKBLACK);
            $("#DetailLCarWashLi dd:first").addClass(C_SC3240201CLASS_CHECKBLACK);
        }
        else {
            //「無し」に黒チェック
            $("#DetailSCarWashLi dd:last").addClass(C_SC3240201CLASS_CHECKBLACK);
            $("#DetailLCarWashLi dd:last").addClass(C_SC3240201CLASS_CHECKBLACK);
        }
    }
} //SetDetailCarWashArea End

/**	
* チップ詳細(小・大)の待ち方エリアの設定を行う
*
* @param {Boolean} isEditable(True:編集可能/False:編集不可)
* @return {-} -
*
*/
function SetDetailWaitingArea(isEditable) {

    //待ち方が変更可能の場合
    if (isEditable) {
        //選択エリアを青文字テキストにする
        $("#DetailSWaitingLi dd").addClass(C_SC3240201CLASS_TEXTBLUE);
        $("#DetailLWaitingLi dd").addClass(C_SC3240201CLASS_TEXTBLUE);

        //待ち方の判断（0:Waiting／4:Drop off）
        if ($("#WaitingFlgHidden").val() != "4") {
            //「店内」に青チェック
            $("#DetailSWaitingLi dd:first").addClass(C_SC3240201CLASS_CHECKBLUE);
            $("#DetailLWaitingLi dd:first").addClass(C_SC3240201CLASS_CHECKBLUE);
        }
        else {
            //「店外」に青チェック
            $("#DetailSWaitingLi dd:last").addClass(C_SC3240201CLASS_CHECKBLUE);
            $("#DetailLWaitingLi dd:last").addClass(C_SC3240201CLASS_CHECKBLUE);
        }

        //待ち方エリアにイベント設定
        SetEventDetailWaitingArea();

    }
    //待ち方が変更不可の場合
    else {
        //選択エリアを黒文字テキストにする
        $("#DetailSWaitingLi dd").addClass(C_SC3240201CLASS_TEXTBLACK);
        $("#DetailLWaitingLi dd").addClass(C_SC3240201CLASS_TEXTBLACK);

        //待ち方の判断（0:Waiting／4:Drop off）
        if ($("#WaitingFlgHidden").val() != "4") {
            //「店内」に黒チェック
            $("#DetailSWaitingLi dd:first").addClass(C_SC3240201CLASS_CHECKBLACK);
            $("#DetailLWaitingLi dd:first").addClass(C_SC3240201CLASS_CHECKBLACK);
        }
        else {
            //店外に黒チェック
            $("#DetailSWaitingLi dd:last").addClass(C_SC3240201CLASS_CHECKBLACK);
            $("#DetailLWaitingLi dd:last").addClass(C_SC3240201CLASS_CHECKBLACK);
        }
    }
} //SetDetailWaitingArea End

//2013/12/04 TMEJ 下村 タブレット版SMB チーフテクニシャン機能開発 START
/**	
* チップ詳細(小・大)の完成検査有無エリアの設定を行う
* 	
* @param {Boolean} isEditable(True:編集可能/False:編集不可)
* @return {-} -
*
*/
function SetDetailCompleteExaminationArea(isEditable) {

    //完成検査有無が変更可能の場合
    if (isEditable) {
        //選択エリアを青文字テキストにする
        $("#DetailSCompleteExaminationLi dd").addClass(C_SC3240201CLASS_TEXTBLUE);
        $("#DetailLCompleteExaminationLi dd").addClass(C_SC3240201CLASS_TEXTBLUE);

        //完成検査有無の判断
        if ($("#CompleteExaminationFlgHidden").val() == "1") {
            //「有り」に青チェック
            $("#DetailSCompleteExaminationLi dd:first").addClass(C_SC3240201CLASS_CHECKBLUE);
            $("#DetailLCompleteExaminationLi dd:first").addClass(C_SC3240201CLASS_CHECKBLUE);
        }
        else {
            //「無し」に青チェック
            $("#DetailSCompleteExaminationLi dd:last").addClass(C_SC3240201CLASS_CHECKBLUE);
            $("#DetailLCompleteExaminationLi dd:last").addClass(C_SC3240201CLASS_CHECKBLUE);
        }

        //完成検査有無エリアにイベントの設定
        SetEventDetailCompleteExaminationArea();

    }
    //完成検査有無が変更不可の場合
    else {
        //選択エリアを黒文字テキストにする
        $("#DetailSCompleteExaminationLi dd").addClass(C_SC3240201CLASS_TEXTBLACK);
        $("#DetailLCompleteExaminationLi dd").addClass(C_SC3240201CLASS_TEXTBLACK);

        //完成検査有無の判断
        if ($("#CompleteExaminationFlgHidden").val() == "1") {
            //「有り」に黒チェック
            $("#DetailSCompleteExaminationLi dd:first").addClass(C_SC3240201CLASS_CHECKBLACK);
            $("#DetailLCompleteExaminationLi dd:first").addClass(C_SC3240201CLASS_CHECKBLACK);
        }
        else {
            //「無し」に黒チェック
            $("#DetailSCompleteExaminationLi dd:last").addClass(C_SC3240201CLASS_CHECKBLACK);
            $("#DetailLCompleteExaminationLi dd:last").addClass(C_SC3240201CLASS_CHECKBLACK);
        }
    }
} //SetDetailCompleteExaminationArea End

/**	
* チップ詳細(大)の個人・法人エリアの設定を行う
* 	
* @return {-} -
*
*/
function SetDetailLIndividualOrCorporationArea() {

    //完成検査有無の判断
    if ($("#FleetFlgHidden").val() == "1") {
        //「法人:1」に黒チェック
        $(".detailLIndividualOrCorporation dd:last").addClass(C_SC3240201CLASS_CHECKBLACK);
    }
    else if ($("#FleetFlgHidden").val() == "0") {
        //「個人:0」に黒チェック
        $(".detailLIndividualOrCorporation dd:first").addClass(C_SC3240201CLASS_CHECKBLACK);
    }
} //SetDetailLIndividualOrCorporationArea End
//2013/12/04 TMEJ 下村 タブレット版SMB チーフテクニシャン機能開発 END

/**	
* テキストエリアの初期化を行う
* 	
* @param {$(textarea)} ctrlTa
* @param {$(dt)} ctrlDt
* @return {-} -
*
*/
function InitDetailTextArea(ctrlTa, ctrlDt) {

    var settingHeight = 0;
    var textArea = ctrlTa;
    var headerDt = ctrlDt;

    //初期表示データが5行以上ある場合、設定値はscrollHeight
    if (C_SC3240201TA_DEFAULTHEIGHT < textArea.attr("scrollHeight")) {
        settingHeight = textArea.attr("scrollHeight");
    }
    //5行未満はデフォルト値
    else {
        settingHeight = C_SC3240201TA_DEFAULTHEIGHT;
    }

    //テキストエリアとヘッダーに高さ設定
    textArea.height(settingHeight);
    headerDt.css("line-height", settingHeight + 12 + "px");
} //InitDetailTextArea End

/**	
* テキストエリアの高さ調整を行う
* 	
* @param {$(textarea)} ctrlTa
* @param {$(dt)} ctrlDt
* @return {-} -
*
*/
function AdjusterDetailTextArea(ctrlTa, ctrlDt) {

    var textArea = ctrlTa;
    var headerDt = ctrlDt;

    textArea.height(C_SC3240201TA_DEFAULTHEIGHT);
    //headerDt.css("line-height", (C_SC3240201TA_DEFAULTHEIGHT + 12) + 'px');

    var tmp_sh = textArea.attr("scrollHeight");

    while (tmp_sh > textArea.attr("scrollHeight")) {
        tmp_sh = textArea.attr("scrollHeight");
        textarea[0].scrollHeight++;
    }

    if (textArea.attr("scrollHeight") >= textArea.attr("offsetHeight")) {
        textArea.height(textArea.attr("scrollHeight"));
        headerDt.css("line-height", (textArea.attr("scrollHeight") + 12) + 'px');
    }
} //AdjusterDetailTextArea End

/**	
* テキストエリア内の文字列長制御を行う
* 	
* @param {$(textarea)} ta
*
*/
function ControlLengthTextarea(ta) {

//2013/12/04 TMEJ 下村 タブレット版SMB チーフテクニシャン機能開発 START
//    //許容する最大バイト数
//    var maxLen = ta.attr("maxlen");

//    var inputTotalLen = 0;
//    var afterTotalLen = 0;
//    var v = ta.val();

//    //入力（もしくはペースト）された合計バイト数を算出する
//    for (var i = 0; i < v.length; i++) {

//        //※SC3170203.js isByteLength()と同様の正規表現によるチェック
//        if (v[i].match(/[^\x00-\xff]/ig) != null) {
//            inputTotalLen += 2;
//        }
//        else {
//            inputTotalLen += 1;
//        }
//    }

//    //許容する最大バイト数を超える場合、フラグをたてる
//    var overFlg = 0;
//    if (inputTotalLen > maxLen) {
//        var overFlg = 1;
//    }

//    //超過した文字を切り捨てた後の、合計バイト数を算出
//    var k = 0;
//    while (inputTotalLen > maxLen) {
//        var lastChar = v.charAt(v.length - 1 - k);

//        if (lastChar.match(/[^\x00-\xff]/ig) != null) {
//            inputTotalLen -= 2;
//        }
//        else {
//            inputTotalLen -= 1;
//        }
//        k += 1;
//    }
//    afterTotalLen = inputTotalLen;

//    //許容する最大バイト数を超えていた場合のみ、切り出し処理を実施してセットしなおす
//    if (overFlg == "1") {
//        var AfterStr = smbScript.trimStr(ta.val(), afterTotalLen);
//        ta.val(AfterStr);
//    }
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
//2013/12/04 TMEJ 下村 タブレット版SMB チーフテクニシャン機能開発 END
}

/**	
* チップ詳細のポップアップを閉じる
* 	
*/
function CloseChipDetail(closeFlg) {

    //DropDownListのコントロールを削除
    //  ※「System.ArgumentException: 無効なポストバックまたはコールバック引数です。」の対応
    $("#DetailSMaintenanceTypeList").remove();
    $("#DetailSMercList").remove();
    $("#DetailLMaintenanceTypeList").remove();
    $("#DetailLMercList").remove();

    if (closeFlg == "1") {
        //ボタンを青色にする
        $("#DetailLeftBtnDiv").addClass("icrop-pressed");

        //背景色をクリア
        $("#DetailCancelBtn").css("background-image", "none");

        setTimeout(function () {
            //ボタンの青色を解除
            $("#DetailLeftBtnDiv").removeClass("icrop-pressed");

            //背景色を戻す
            $("#DetailCancelBtn").css("background-image", "-webkit-gradient(linear, left top, left bottom, from(#4f5769),color-stop(0.01, #4f5769),color-stop(0.01, #576174),color-stop(0.5, #1a233e),color-stop(0.5, #000b29),to(#000b29))");
        }, 300);
    }

    $("#ChipDetailPopup").fadeOut(300);

    //ポップアップが閉じるときは縮小表示にしておく
    ShrinkDisplay(0);

    //タイマーをクリア(画面が表示し終わる前に閉じられた場合の対応)
    commonClearTimer();

    //フッターボタンを再表示する
    RedisplayFooterBtnByChipDetail();

    //ポップアップ監視イベントを取り除く(画面を表示するたびにbindするため)
    //$(document.body).unbind(C_SC3240201_TOUCH, ObserveChipDetailClose);

    return false;
}

/**	
* チップの所属位置によってフッターボタンを再表示する
* 	
*/
function RedisplayFooterBtnByChipDetail() {
    //サブチップボックスならば数値が返却される
    //2014/06/05 TMEJ 丁 タブレット版SMB チーフテクニシャン機能開発 START
    //var subAreaId = GetSubChipType(gSelectedChipId);
    var subAreaId = GetSubChipFootButtonType(gSelectedChipId); 
    //2014/06/05 TMEJ 丁 タブレット版SMB チーフテクニシャン機能開発 END

    if (subAreaId == "") {
        //ストール上のチップ
        //2019/08/02 NSK 近藤 (トライ店システム評価)次世代サービスオペレーション効率化に向けた、業務適合性検証 START
        //2014/06/18 TMEJ 丁 タブレット版SMB テレマ走行距離機能開発 START
        //CreateFooterButton(C_FT_DISPTP_SELECTED, GetChipType(gSelectedChipId));
        //CreateFooterButton(C_FT_DISPTP_SELECTED, GetChipType(gSelectedChipId), gArrObjChip[gSelectedChipId].tlmContractFlg);
        //2014/06/18 TMEJ 丁 タブレット版SMB テレマ走行距離機能開発 END  
        //2019/08/02 NSK 近藤 (トライ店システム評価)次世代サービスオペレーション効率化に向けた、業務適合性検証 END

        // 2019/12/06 NSK 皆川 (トライ店システム評価)整備作業における休憩時間取得判定向上検証 START
        // 休憩を自動判定する場合
        if ($("#hidRestAutoJudgeFlg").val() == "1") {
            // 2019/12/06 NSK 皆川 (トライ店システム評価)整備作業における休憩時間取得判定向上検証 END

            //2019/08/02 NSK 近藤 (トライ店システム評価)次世代サービスオペレーション効率化に向けた、業務適合性検証 START
            // 日跨ぎタイプを取得する
            var nOverDaysType = GetChipOverDaysType(gSelectedChipId);
            // 日跨ぎの場合
            if (C_OVERDAYS_NONE < nOverDaysType) {
                // コールバックに渡す引数
                var jsonData = {
                    Method: "GetCanRestChange",
                    ShowDate: $("#hidShowDate").val(),
                    StallUseId: gArrObjChip[gSelectedChipId].stallUseId
                };

                //コールバック開始
                DoCallBack(C_CALLBACK_WND101, jsonData, SC3240101AfterCallBack, jsonData.Method);
            } else {
                var canRestChange = CanRestChange(gSelectedChipId);
                CreateFooterButton(C_FT_DISPTP_SELECTED, GetChipType(gSelectedChipId), gArrObjChip[gSelectedChipId].tlmContractFlg, canRestChange);
            }
            //2019/08/02 NSK 近藤 (トライ店システム評価)次世代サービスオペレーション効率化に向けた、業務適合性検証 END

            // 2019/12/06 NSK 皆川 (トライ店システム評価)整備作業における休憩時間取得判定向上検証 START
        } else {
            CreateFooterButton(C_FT_DISPTP_SELECTED, GetChipType(gSelectedChipId), gArrObjChip[gSelectedChipId].tlmContractFlg, false);
        }
        // 2019/12/06 NSK 皆川 (トライ店システム評価)整備作業における休憩時間取得判定向上検証 END
        
    }
    else {
        //2014/06/18 TMEJ 丁 タブレット版SMB テレマ走行距離機能開発 START
        //サブチップボックスのチップ
        //CreateFooterButton(C_FT_DISPTP_SELECTED, subAreaId);
        CreateFooterButton(C_FT_DISPTP_SELECTED, subAreaId, gArrObjSubChip[gSelectedChipId].tlmContractFlg);
        //2014/06/18 TMEJ 丁 タブレット版SMB テレマ走行距離機能開発 END  
    }
}

/**	
* チップ詳細(小)から(大)へ共通の固定文言をコピーする
* 	
*/
function CopyToDetailLWord() {

    //オレンジ枠エリア
    $("#DetailLDeriveredPlanWordLabel").text($("#DetailSDeriveredPlanWordLabel").text());
    $("#DetailLDeriveredProspectWordLabel").text($("#DetailSDeriveredProspectWordLabel").text());

    //顧客車両エリア
    $("#DetailLRegNoWordLabel").text($("#DetailSRegNoWordLabel").text());
    $("#DetailLVinWordLabel").text($("#DetailSVinWordLabel").text());
    $("#DetailLVehicleWordLabel").text($("#DetailSVehicleWordLabel").text());
    $("#DetailLCstNameWordLabel").text($("#DetailSCstNameWordLabel").text());
    $("#DetailLMobileWordLabel").text($("#DetailSMobileWordLabel").text());
    $("#DetailLHomeWordLabel").text($("#DetailSHomeWordLabel").text());
    $("#DetailLSAWordLabel").text($("#DetailSSAWordLabel").text());

    //アイコン
    $("#DetailLIcnD").text($("#DetailSIcnD").text());
    $("#DetailLIcnD").text($("#DetailSIcnD").text());
    //2018/06/26 NSK 可児 TKM Next Gen e-CRB Project Application development Block B-1  Pマーク、L マークなどを表示 START
    //$("#DetailLIcnI").text($("#DetailSIcnI").text());
    $("#DetailLIcnP").text($("#DetailSIcnP").text());
    $("#DetailLIcnL").text($("#DetailSIcnL").text());
    //2018/06/26 NSK 可児 TKM Next Gen e-CRB Project Application development Block B-1  Pマーク、L マークなどを表示 END
    $("#DetailLIcnS").text($("#DetailSIcnS").text());

    //時間エリア
    $("#DetailLVisitTimeWordLabel").text($("#DetailSVisitTimeWordLabel").text());
    $("#DetailLStartTimeWordLabel").text($("#DetailSStartTimeWordLabel").text());
    $("#DetailLFinishTimeWordLabel").text($("#DetailSFinishTimeWordLabel").text());
    $("#DetailLDeliveredTimeWordLabel").text($("#DetailSDeliveredTimeWordLabel").text());
    $("#DetailLPlanTimeWordLabel").text($("#DetailSPlanTimeWordLabel").text());
    $("#DetailLProcessTimeWordLabel").text($("#DetailSProcessTimeWordLabel").text());

    //整備種類エリア
    $("#DetailLMaintenanceTypeWordLabel").text($("#DetailSMaintenanceTypeWordLabel").text());
    $("#DetailLWorkTimeWordLabel").text($("#DetailSWorkTimeWordLabel").text());

    //整備名
    $("#DetailLMercWordLabel").text($("#DetailSMercWordLabel").text());

    //整備内容エリア
    $("#DetailLMaintenanceNoWordLabel").text($("#DetailSMaintenanceNoWordLabel").text());
    $("#DetailLMaintenanceWordLabel").text($("#DetailSMaintenanceWordLabel").text());
    $("#DetailLStallWordLabel").text($("#DetailSStallWordLabel").text());
    //2013/12/04 TMEJ 下村 タブレット版SMB チーフテクニシャン機能開発 START
    $("#DetailLMaintenanceNoCstApproveLabel").text($("#DetailSMaintenanceNoCstApproveLabel").text());
    //2013/12/04 TMEJ 下村 タブレット版SMB チーフテクニシャン機能開発 END

    //パーツエリア
    $("#DetailLPartsNoWordLabel").text($("#DetailSPartsNoWordLabel").text());
    $("#DetailLPartsWordLabel").text($("#DetailSPartsWordLabel").text());
    //2013/12/04 TMEJ 下村 タブレット版SMB チーフテクニシャン機能開発 START
    $("#DetailLTablePartsNoCstApproveLabel").text($("#DetailSTablePartsNoCstApproveLabel").text());
    //2013/12/04 TMEJ 下村 タブレット版SMB チーフテクニシャン機能開発 END

    //ご用命エリア
    $("#DetailLOrderWordLabel").text($("#DetailSOrderWordLabel").text());

    //2013/12/04 TMEJ 下村 タブレット版SMB チーフテクニシャン機能開発 START
    ////故障原因エリア
    //$("#DetailLFailureWord1Label").text($("#DetailSFailureWord1Label").text());

    ////診断結果エリア
    //$("#DetailLResultWord1Label").text($("#DetailSResultWord1Label").text());

    ////アドバイスエリア
    //$("#DetailLAdviceWord1Label").text($("#DetailSAdviceWord1Label").text());

    //メモエリア
    $("#DetailLMemoWord1Label").text($("#DetailSMemoWord1Label").text());
    //2013/12/04 TMEJ 下村 タブレット版SMB チーフテクニシャン機能開発 END

    //チェックエリア
    $("#DetailLReservationCheckWordLabel").text($("#DetailSReservationCheckWordLabel").text());
    $("#DetailLCarWashCheckWordLabel").text($("#DetailSCarWashCheckWordLabel").text());
    $("#DetailLWaitingCheckWordLabel").text($("#DetailSWaitingCheckWordLabel").text());
    $("#DetailLReservationYesWordLabel").text($("#DetailSReservationYesWordLabel").text());
    $("#DetailLWalkInWordLabel").text($("#DetailSWalkInWordLabel").text());
    $("#DetailLCarWashYesWordLabel").text($("#DetailSCarWashYesWordLabel").text());
    $("#DetailLCarWashNoWordLabel").text($("#DetailSCarWashNoWordLabel").text());
    $("#DetailLWaitingInsideWordLabel").text($("#DetailSWaitingInsideWordLabel").text());
    $("#DetailLWaitingOutsideWordLabel").text($("#DetailSWaitingOutsideWordLabel").text());
    //2013/12/04 TMEJ 下村 タブレット版SMB チーフテクニシャン機能開発 START
    $("#DetailLCompleteExaminationCheckWordLabel").text($("#DetailSCompleteExaminationCheckWordLabel").text());
    $("#DetailLCompleteExaminationYesWordLabel").text($("#DetailSCompleteExaminationYesWordLabel").text());
    $("#DetailLCompleteExaminationNoWordLabel").text($("#DetailSCompleteExaminationNoWordLabel").text());
    //2013/12/04 TMEJ 下村 タブレット版SMB チーフテクニシャン機能開発 END
}

/**	
* チップ詳細(小)から(大)へリピーター系以外の共通動的データをコピーする
* 	
*/
function CopyToDetailLData() {

    //オレンジ枠内のチップステータス
    $("#DetailLDeriveredPlanTimeLabel").text($("#DetailSDeriveredPlanTimeLabel").text());
    $("#DetailLChipStatusLabel").text($("#DetailSChipStatusLabel").text());
    $("#DetailLChangeNumberLabel").text($("#DetailSChangeNumberLabel").text());
    $("#DetailLTriangleLabel").text($("#DetailSTriangleLabel").text());
    $("#DetailLDeriveredProspectTimeLabel").text($("#DetailSDeriveredProspectTimeLabel").text());
    $("#DetailLFixUpArrow").text($("#DetailSFixUpArrow").text());

    //顧客・車両情報
    $("#DetailLRegNoLabel").text($("#DetailSRegNoLabel").text());
    $("#DetailLVinLabel").text($("#DetailSVinLabel").text());
    $("#DetailLVehicleLabel").text($("#DetailSVehicleLabel").text());
    $("#DetailLCstNameLabel").text($("#DetailSCstNameLabel").text());
    $("#DetailLMobileLabel").text($("#DetailSMobileLabel").text());
    $("#DetailLHomeLabel").text($("#DetailSHomeLabel").text());
    $("#DetailLSALabel").text($("#DetailSSALabel").text());

    //時間エリア
    //実際に表示しているラベル
    $("#DetailLPlanVisitLabel").text($("#DetailSPlanVisitLabel").text());
    $("#DetailLPlanStartLabel").text($("#DetailSPlanStartLabel").text());
    $("#DetailLPlanFinishLabel").text($("#DetailSPlanFinishLabel").text());
    $("#DetailLPlanDeriveredLabel").text($("#DetailSPlanDeriveredLabel").text());
    $("#DetailLProcessVisitTimeLabel").text($("#DetailSProcessVisitTimeLabel").text());
    $("#DetailLProcessStartLabel").text($("#DetailSProcessStartLabel").text());
    $("#DetailLProcessFinishLabel").text($("#DetailSProcessFinishLabel").text());
    $("#DetailLProcessDeriveredTimeLabel").text($("#DetailSProcessDeriveredTimeLabel").text());

    //DateTimeSelectorに設定しているパラメータ
    //2013/12/26 TMEJ 下村 次世代e-CRBタブレット(サービス) iOS7.0 VersionUp対応 START
    //$("#DetailLPlanVisitDateTimeSelector").get(0).valueAsDate = $("#DetailSPlanVisitDateTimeSelector").get(0).valueAsDate;
    //$("#DetailLPlanStartDateTimeSelector").get(0).valueAsDate = $("#DetailSPlanStartDateTimeSelector").get(0).valueAsDate;
    //$("#DetailLPlanFinishDateTimeSelector").get(0).valueAsDate = $("#DetailSPlanFinishDateTimeSelector").get(0).valueAsDate;
    //$("#DetailLPlanDeriveredDateTimeSelector").get(0).valueAsDate = $("#DetailSPlanDeriveredDateTimeSelector").get(0).valueAsDate;
    //$("#DetailLProcessStartDateTimeSelector").get(0).valueAsDate = $("#DetailSProcessStartDateTimeSelector").get(0).valueAsDate;
    //$("#DetailLProcessFinishDateTimeSelector").get(0).valueAsDate = $("#DetailSProcessFinishDateTimeSelector").get(0).valueAsDate;
    $("#DetailLPlanVisitDateTimeSelector").get(0).value = $("#DetailSPlanVisitDateTimeSelector").get(0).value;
    $("#DetailLPlanStartDateTimeSelector").get(0).value = $("#DetailSPlanStartDateTimeSelector").get(0).value;
    $("#DetailLPlanFinishDateTimeSelector").get(0).value = $("#DetailSPlanFinishDateTimeSelector").get(0).value;
    $("#DetailLPlanDeriveredDateTimeSelector").get(0).value = $("#DetailSPlanDeriveredDateTimeSelector").get(0).value;
    $("#DetailLProcessStartDateTimeSelector").get(0).value = $("#DetailSProcessStartDateTimeSelector").get(0).value;
    $("#DetailLProcessFinishDateTimeSelector").get(0).value = $("#DetailSProcessFinishDateTimeSelector").get(0).value;
    //2013/12/26 TMEJ 下村 次世代e-CRBタブレット(サービス) iOS7.0 VersionUp対応 END

    //来店実績と納車実績はDateTimeSelectorがないため、属性で日時を保持している
    $("#DetailLProcessVisitTimeLabel").attr("datetime", $("#DetailSProcessVisitTimeLabel").attr("datetime"));
    $("#DetailLProcessDeriveredTimeLabel").attr("datetime", $("#DetailSProcessDeriveredTimeLabel").attr("datetime"));

    //作業時間
    $("#DetailLWorkTimeTxt").val($("#DetailSWorkTimeTxt").val());
    $("#DetailLWorkTimeLabel").text($("#DetailSWorkTimeLabel").text());

    //ご用命
    $("#DetailLOrderTxt").val($("#DetailSOrderTxt").val());

    //2013/12/04 TMEJ 下村 タブレット版SMB チーフテクニシャン機能開発 START
    ////故障原因
    //$("#DetailLFailureTxt").val($("#DetailSFailureTxt").val());

    ////診断結果
    //$("#DetailLResultTxt").val($("#DetailSResultTxt").val());

    ////アドバイス
    //$("#DetailLAdviceTxt").val($("#DetailSAdviceTxt").val());

    //メモ
    $("#DetailLMemoTxt").val($("#DetailSMemoTxt").val());
    //2013/12/04 TMEJ 下村 タブレット版SMB チーフテクニシャン機能開発 END

    //リレーションチップ情報
    var largeChipTableRow = $("#chipInfoTable tr:eq(0)");
    var rezIdListDiv = $("#DetailSMaintenanceDl").find("div");
    if (!$.isEmptyObject(rezIdListDiv)) {

        //チップ詳細に表示したチップ分ループ
        rezIdListDiv.each(function (i, elem) {

            //「未選択」のチップは除く
            if ($(elem).attr("rezid") != "-1") {

                //チップリスト内のストール利用ステータス　＝　03：完了　の場合
                if ($(elem).attr("stallusestatus") == "03") {

                    //チップ詳細(大)のチップ欄のヘッダ部分にコピー（背景色を灰色にする）
                    largeChipTableRow.append("<td style='background-color: #F2F2F2; '><span class='ChipDetailEllipsis icrop-CustomLabel' style='display: inline-block; width: 90px; overflow: hidden; white-space: nowrap; text-overflow: ellipsis; '>" + elem.children[0].innerHTML + "</td></span>");
                }
                else {

                    //チップ詳細(大)のチップ欄のヘッダ部分にコピー（背景色は指定しない）
                    largeChipTableRow.append("<td><span class='ChipDetailEllipsis icrop-CustomLabel' style='display: inline-block; width: 90px; overflow: hidden; white-space: nowrap; text-overflow: ellipsis; '>" + elem.children[0].innerHTML + "</td></span>");
                }
            }
        });
    } 
}

/**	
* イベントや表示の設定を行う
* 	
*/
function SetChipDetailDisplayAndEvent() {

    //チップ詳細(小)の拡大ボタンクリックイベント登録
    $("#ExpansionButton").bind("click", ExpandDisplay);

    //チップ詳細(大)の縮小ボタンクリックイベント登録
    $("#ShrinkingButton").bind("click", ShrinkDisplay);

    //納車予定変更履歴エリアの初期設定
    InitDeliHisArea();

    //予約マーク・JDP調査対象客マーク・SSCマークの表示設定
    SetMarkChipDetail();

	//2013/12/04 TMEJ 下村 タブレット版SMB チーフテクニシャン機能開発 START
	//if ($("#detailSTableChipUl").css("display") != "none")
    //    //チップ詳細(小)チップエリアの初期化
    //    InitDetailSChipArea();
    //    //チップ詳細(大)チップエリアの初期化
    //    InitDetailLChipArea();
    //    //チップ詳細(小)チップエリアの太字・細字・背景色の設定
    //    SetOtherDetailSChipArea();
    //    //チップ詳細(大)チップエリアの太字・細字・背景色の設定
    //    SetOtherDetailLChipArea();
    //}
    
//    //整備種類の選択されているリストを設定
//    //整備種類のDropDownListで選択されているIDを取得
//    var e = document.getElementById("DetailSMaintenanceTypeList");
//    var g = document.getElementById("DetailLMaintenanceTypeList");
//    var svcClassName = $("#DetailSMaintenanceTypeLabel").text().Trim();
//    var i = 1;
//    if(svcClassName != ""){
//        for (i = 1; i < e.options.length; i++) {
//            if (svcClassName ==  e.options[i].text.Trim()){
//                break;
//            }
//        }
//        e.options[i].selected = true;
//    	g.options[i].selected = true;
//    }
//    //整備名のDropDownListで選択されているIDを取得
//    var f = document.getElementById("DetailSMercList");
//    var h = document.getElementById("DetailLMercList");
//	var regMercName = $("#DetailSMercLabel").text().Trim();
//	i = 1;
//	if(regMercName != ""){
//		for (i = 1; i < f.options.length; i++) {
//			if (regMercName ==  f.options[i].text.Trim()){
//				break;
//			}
//        }
//        f.options[i].selected = true;
//        h.options[i].selected = true;
//	}
//	
    //サブチップボックスならば数値が返却される
    var subAreaId = GetSubChipType(gSelectedChipId);
	
	//追加作業エリア以外の場合
    if (subAreaId != C_FT_BTNTP_WAIT_CONFIRMEDADDWORK) {
        //整備エリアが表示されている場合
        if ($("#detailSTableChipUl").css("display") != "none") {
            //チップ詳細(小)チップエリアの初期化
            InitDetailSChipArea();
            //チップ詳細(大)チップエリアの初期化
            InitDetailLChipArea();
            //チップ詳細(小)チップエリアの太字・細字・背景色の設定
            SetOtherDetailSChipArea();
            //チップ詳細(大)チップエリアの太字・細字・背景色の設定
            SetOtherDetailLChipArea();
        }
    } else {
        //2014/07/15 TMEJ 丁 タブレットSMB Job Dispatch機能開発 START
        //追加作業チップ詳細のレイアウト調整
        SetChipDetialLayoutForAddWorkArea();
        //2014/07/15 TMEJ 丁 タブレットSMB Job Dispatch機能開発 END
    } 

    //2014/07/15 TMEJ 丁 タブレットSMB Job Dispatch機能開発 START
    //Jobボタン制御
    InitDetailLActionButton();
    //2014/07/15 TMEJ 丁 タブレットSMB Job Dispatch機能開発 END

	//2013/12/04 TMEJ 下村 タブレット版SMB チーフテクニシャン機能開発 END

    //2013/12/04 TMEJ 下村 タブレット版SMB チーフテクニシャン機能開発 START
    ////ご用命エリア、故障原因、診断結果、アドバイスエリアの初期化
    //InitDetailTextArea($("#DetailSOrderTxt"), $("#DetailSOrderDt"));
    //InitDetailTextArea($("#DetailSFailureTxt"), $("#DetailSFailureDt"));
    //InitDetailTextArea($("#DetailSResultTxt"), $("#DetailSResultDt"));
    //InitDetailTextArea($("#DetailSAdviceTxt"), $("#DetailSAdviceDt"));
    //InitDetailTextArea($("#DetailLOrderTxt"), $("#DetailLOrderDt"));
    //InitDetailTextArea($("#DetailLFailureTxt"), $("#DetailLFailureDt"));
    //InitDetailTextArea($("#DetailLResultTxt"), $("#DetailLResultDt"));
    //InitDetailTextArea($("#DetailLAdviceTxt"), $("#DetailLAdviceDt"));

    //ご用命エリア、メモの初期化
    InitDetailTextArea($("#DetailSOrderTxt"), $("#DetailSOrderDt"));
    InitDetailTextArea($("#DetailSMemoTxt"), $("#DetailSMemoDt"));
    InitDetailTextArea($("#DetailLOrderTxt"), $("#DetailLOrderDt"));
    InitDetailTextArea($("#DetailLMemoTxt"), $("#DetailLMemoDt"));

	//チップ詳細(大)の個人・法人のチェックリストの設定
	SetDetailLIndividualOrCorporationArea();
	//2013/12/04 TMEJ 下村 タブレット版SMB チーフテクニシャン機能開発 END

    //チップ詳細(小)のテキストイベント設定
    SetDetailSTextEvent();
    //チップ詳細(大)のテキストイベント設定
    SetDetailLTextEvent();

    //サブチップボックスならば数値が返却される
    var subAreaId = GetSubChipType(gSelectedChipId);

    //チップ詳細がサブチップボックスから開かれた場合
    if (subAreaId != "") {
        //サブチップボックスIDによって設定を行う
        SetChipDetailBySubAreaId(subAreaId);
    }
    else {
        //ステータス等によって設定を行う
        SetChipDetailByStatusEtc();
    }

    //チップ詳細を開いた時点のデータを保持する（更新時の比較用）
    CreateChipDetailBeforeData();
}

/**	
* サブチップボックスのチップからチップ詳細が開かれた場合、
* サブチップボックスのIDによって編集できる項目を変更する
* 	
* @param {String} subAreaId サブチップボックスID
* @return {-} -
*
*/
function SetChipDetailBySubAreaId(subAreaId) {

    //ストール利用.ストール利用ステータス
    var stallUseStatus = $("#ChipDetailStallUseStatusHidden").val().Trim();

    //RO番号
    var orderNo = $("#ChipDetailOrderNoHidden").val().Trim();

    //来店実績日時
    var processVisit = $("#DetailSProcessVisitTimeLabel").attr("datetime");

    //選択中チップが所属するエリアによって編集できる内容を変更する
    switch (subAreaId) {

        //受付サブチップボックス
        case C_FT_BTNTP_CONFIRMED_RO:

            //来店予定日時
            $("#DetailSPlanVisitLabel").addClass(C_SC3240201CLASS_TEXTBLACK);
            $("#DetailSPlanVisitDateTimeSelector").attr("readonly", "true").attr("disabled", "true");
            $("#DetailLPlanVisitLabel").addClass(C_SC3240201CLASS_TEXTBLACK);
            $("#DetailLPlanVisitDateTimeSelector").attr("readonly", "true").attr("disabled", "true");

            //作業開始予定日時
            $("#DetailSPlanStartLabel").addClass(C_SC3240201CLASS_TEXTBLACK);
            $("#DetailSPlanStartDateTimeSelector").attr("readonly", "true").attr("disabled", "true");
            $("#DetailLPlanStartLabel").addClass(C_SC3240201CLASS_TEXTBLACK);
            $("#DetailLPlanStartDateTimeSelector").attr("readonly", "true").attr("disabled", "true");

            //作業終了予定日時
            $("#DetailSPlanFinishLabel").addClass(C_SC3240201CLASS_TEXTBLACK);
            $("#DetailSPlanFinishDateTimeSelector").attr("readonly", "true").attr("disabled", "true");
            $("#DetailLPlanFinishLabel").addClass(C_SC3240201CLASS_TEXTBLACK);
            $("#DetailLPlanFinishDateTimeSelector").attr("readonly", "true").attr("disabled", "true");

            //納車予定日時
            //サービス入庫．実績入庫日時がある場合、編集不可　※'1900/01/01 00:00:00'でない場合
            if (IsMinDate(processVisit) == false) {
                $("#DetailSPlanDeriveredLabel").addClass(C_SC3240201CLASS_TEXTBLACK);
                $("#DetailSPlanDeriveredDateTimeSelector").attr("readonly", "true").attr("disabled", "true");
                $("#DetailLPlanDeriveredLabel").addClass(C_SC3240201CLASS_TEXTBLACK);
                $("#DetailLPlanDeriveredDateTimeSelector").attr("readonly", "true").attr("disabled", "true");
            }

            // 2016/10/04 NSK 秋田谷 チップ詳細の実績時間を変更できなくする対応 START
            //作業開始実績日時
            // $("#DetailSProcessStartLabel").addClass(C_SC3240201CLASS_TEXTBLACK);
            // $("#DetailSProcessStartDateTimeSelector").attr("readonly", "true").attr("disabled", "true");
            // $("#DetailLProcessStartLabel").addClass(C_SC3240201CLASS_TEXTBLACK);
            // $("#DetailLProcessStartDateTimeSelector").attr("readonly", "true").attr("disabled", "true");

            //作業終了実績日時
            // $("#DetailSProcessFinishLabel").addClass(C_SC3240201CLASS_TEXTBLACK);
            // $("#DetailSProcessFinishDateTimeSelector").attr("readonly", "true").attr("disabled", "true");
            // $("#DetailLProcessFinishLabel").addClass(C_SC3240201CLASS_TEXTBLACK);
            // $("#DetailLProcessFinishDateTimeSelector").attr("readonly", "true").attr("disabled", "true");
            // 2016/10/04 NSK 秋田谷 チップ詳細の実績時間を変更できなくする対応 END

            //整備種類
            $("#DetailSMaintenanceTypeLabel").addClass(C_SC3240201CLASS_TEXTBLACK);
            $("#DetailLMaintenanceTypeLabel").addClass(C_SC3240201CLASS_TEXTBLACK);
            $("#DetailSMaintenanceTypeList").attr("disabled", "true");
            $("#DetailLMaintenanceTypeList").attr("disabled", "true");

            //整備名
            $("#DetailSMercLabel").addClass(C_SC3240201CLASS_TEXTBLACK);
            $("#DetailLMercLabel").addClass(C_SC3240201CLASS_TEXTBLACK);
            $("#DetailSMercList").attr("disabled", "true");
            $("#DetailLMercList").attr("disabled", "true");

            //作業時間
            SetReadOnlyDetailWorkTime();

            //予約有無・洗車有無・待ち方を変更不可に設定
            SetDetailReservationArea(false);
            SetDetailCarWashArea(false);
            SetDetailWaitingArea(false);
            //2013/12/04 TMEJ 下村 タブレット版SMB チーフテクニシャン機能開発 START
            SetDetailCompleteExaminationArea(false);
            //2013/12/04 TMEJ 下村 タブレット版SMB チーフテクニシャン機能開発 END

			//作業完了チップは編集不可
            if (stallUseStatus == "03") {
                //チップエリアの見た目を読取専用(文字を青→黒、チェックを青→グレー)にする
                SetReadOnlyDetailChipArea();
            }

            //選択できるチップが存在する場合
            if (0 < $("#DetailSMaintenanceDl").find("div").length) {
            	//2013/12/04 TMEJ 下村 タブレット版SMB チーフテクニシャン機能開発 START
                ////チップエリアのイベントを設定
                //SetEventDetailSChipArea();
                //SetEventDetailLChipArea();
                if (stallUseStatus != "03") {
	                //チップエリアのイベントを設定
	                SetEventDetailSChipArea();
	                SetEventDetailLChipArea();
            	}
            	//2013/12/04 TMEJ 下村 タブレット版SMB チーフテクニシャン機能開発 END
            }
            else {
                //チップ詳細(小)整備エリアの1行表示用チップ情報をEmptyにする
                SetDetailSSingleLineTextEmpty();
            }

            //ご用命
            $("#DetailSOrderTxt").addClass(C_SC3240201CLASS_TEXTBLACK).attr("readonly", "true");
            $("#DetailLOrderTxt").addClass(C_SC3240201CLASS_TEXTBLACK).attr("readonly", "true");

            //2013/12/04 TMEJ 下村 タブレット版SMB チーフテクニシャン機能開発 START
            ////ご用命(親R/O)・故障原因(追加作業)・診断結果(追加作業)・アドバイス(親R/O)の活性／非活性制御
            //SetReadOnlyTextArea();

            //メモ
            $("#DetailSMemoTxt").addClass(C_SC3240201CLASS_TEXTBLACK).attr("readonly", "true");
            $("#DetailLMemoTxt").addClass(C_SC3240201CLASS_TEXTBLACK).attr("readonly", "true");
            //2013/12/04 TMEJ 下村 タブレット版SMB チーフテクニシャン機能開発 END

            break;

        //追加作業サブチップボックス
        case C_FT_BTNTP_WAIT_CONFIRMEDADDWORK:

            //来店予定日時
            $("#DetailSPlanVisitLabel").addClass(C_SC3240201CLASS_TEXTBLACK);
            $("#DetailSPlanVisitDateTimeSelector").attr("readonly", "true").attr("disabled", "true");
            $("#DetailLPlanVisitLabel").addClass(C_SC3240201CLASS_TEXTBLACK);
            $("#DetailLPlanVisitDateTimeSelector").attr("readonly", "true").attr("disabled", "true");

            //作業開始予定日時
            $("#DetailSPlanStartLabel").addClass(C_SC3240201CLASS_TEXTBLACK);
            $("#DetailSPlanStartDateTimeSelector").attr("readonly", "true").attr("disabled", "true");
            $("#DetailLPlanStartLabel").addClass(C_SC3240201CLASS_TEXTBLACK);
            $("#DetailLPlanStartDateTimeSelector").attr("readonly", "true").attr("disabled", "true");

            //作業終了予定日時
            $("#DetailSPlanFinishLabel").addClass(C_SC3240201CLASS_TEXTBLACK);
            $("#DetailSPlanFinishDateTimeSelector").attr("readonly", "true").attr("disabled", "true");
            $("#DetailLPlanFinishLabel").addClass(C_SC3240201CLASS_TEXTBLACK);
            $("#DetailLPlanFinishDateTimeSelector").attr("readonly", "true").attr("disabled", "true");

            //納車予定日時
            //サービス入庫．実績入庫日時がある場合、編集不可　※'1900/01/01 00:00:00'でない場合
            if (IsMinDate(processVisit) == false) {
                $("#DetailSPlanDeriveredLabel").addClass(C_SC3240201CLASS_TEXTBLACK);
                $("#DetailSPlanDeriveredDateTimeSelector").attr("readonly", "true").attr("disabled", "true");
                $("#DetailLPlanDeriveredLabel").addClass(C_SC3240201CLASS_TEXTBLACK);
                $("#DetailLPlanDeriveredDateTimeSelector").attr("readonly", "true").attr("disabled", "true");
            }

            // 2016/10/04 NSK 秋田谷 チップ詳細の実績時間を変更できなくする対応 START
            //作業開始実績日時
            // $("#DetailSProcessStartLabel").addClass(C_SC3240201CLASS_TEXTBLACK);
            // $("#DetailSProcessStartDateTimeSelector").attr("readonly", "true").attr("disabled", "true");
            // $("#DetailLProcessStartLabel").addClass(C_SC3240201CLASS_TEXTBLACK);
            // $("#DetailLProcessStartDateTimeSelector").attr("readonly", "true").attr("disabled", "true");

            //作業終了実績日時
            // $("#DetailSProcessFinishLabel").addClass(C_SC3240201CLASS_TEXTBLACK);
            // $("#DetailSProcessFinishDateTimeSelector").attr("readonly", "true").attr("disabled", "true");
            // $("#DetailLProcessFinishLabel").addClass(C_SC3240201CLASS_TEXTBLACK);
            // $("#DetailLProcessFinishDateTimeSelector").attr("readonly", "true").attr("disabled", "true");

            // 2016/10/04 NSK 秋田谷 チップ詳細の実績時間を変更できなくする対応 END

            //整備種類
            $("#DetailSMaintenanceTypeLabel").addClass(C_SC3240201CLASS_TEXTBLACK);
            $("#DetailLMaintenanceTypeLabel").addClass(C_SC3240201CLASS_TEXTBLACK);
            $("#DetailSMaintenanceTypeList").attr("disabled", "true");
            $("#DetailLMaintenanceTypeList").attr("disabled", "true");

            //整備名
            $("#DetailSMercLabel").addClass(C_SC3240201CLASS_TEXTBLACK);
            $("#DetailLMercLabel").addClass(C_SC3240201CLASS_TEXTBLACK);
            $("#DetailSMercList").attr("disabled", "true");
            $("#DetailLMercList").attr("disabled", "true");

            //作業時間
            SetReadOnlyDetailWorkTime();

            //予約有無・洗車有無・待ち方を変更不可に設定
            SetDetailReservationArea(false);
            SetDetailCarWashArea(false);
            SetDetailWaitingArea(false);
            //2013/12/04 TMEJ 下村 タブレット版SMB チーフテクニシャン機能開発 START
            SetDetailCompleteExaminationArea(false);
            //2013/12/04 TMEJ 下村 タブレット版SMB チーフテクニシャン機能開発 END

            //作業完了チップは編集不可
            //if (stallUseStatus == "03") {
            //    //チップエリアの見た目を読取専用(文字を青→黒、チェックを青→グレー)にする
            //    SetReadOnlyDetailChipArea();
            //}

            //チップ詳細(小)整備エリアの1行表示用チップ情報をEmptyにする
            SetDetailSSingleLineTextEmpty();

            //ご用命
            $("#DetailSOrderTxt").addClass(C_SC3240201CLASS_TEXTBLACK).attr("readonly", "true");
            $("#DetailLOrderTxt").addClass(C_SC3240201CLASS_TEXTBLACK).attr("readonly", "true");

            //2013/12/04 TMEJ 下村 タブレット版SMB チーフテクニシャン機能開発 START
            ////ご用命(親R/O)・故障原因(追加作業)・診断結果(追加作業)・アドバイス(親R/O)の活性／非活性制御
            //SetReadOnlyTextArea();

            //メモ
            $("#DetailSMemoTxt").addClass(C_SC3240201CLASS_TEXTBLACK).attr("readonly", "true");
            $("#DetailLMemoTxt").addClass(C_SC3240201CLASS_TEXTBLACK).attr("readonly", "true");
            //2013/12/04 TMEJ 下村 タブレット版SMB チーフテクニシャン機能開発 END

            break;

        //完成検査サブチップボックス 
        case C_FT_BTNTP_CONFIRMED_INSPECTION:

            //来店予定日時
            $("#DetailSPlanVisitLabel").addClass(C_SC3240201CLASS_TEXTBLACK);
            $("#DetailSPlanVisitDateTimeSelector").attr("readonly", "true").attr("disabled", "true");
            $("#DetailLPlanVisitLabel").addClass(C_SC3240201CLASS_TEXTBLACK);
            $("#DetailLPlanVisitDateTimeSelector").attr("readonly", "true").attr("disabled", "true");

            //作業開始予定日時
            $("#DetailSPlanStartLabel").addClass(C_SC3240201CLASS_TEXTBLACK);
            $("#DetailSPlanStartDateTimeSelector").attr("readonly", "true").attr("disabled", "true");
            $("#DetailLPlanStartLabel").addClass(C_SC3240201CLASS_TEXTBLACK);
            $("#DetailLPlanStartDateTimeSelector").attr("readonly", "true").attr("disabled", "true");

            //作業終了予定日時
            $("#DetailSPlanFinishLabel").addClass(C_SC3240201CLASS_TEXTBLACK);
            $("#DetailSPlanFinishDateTimeSelector").attr("readonly", "true").attr("disabled", "true");
            $("#DetailLPlanFinishLabel").addClass(C_SC3240201CLASS_TEXTBLACK);
            $("#DetailLPlanFinishDateTimeSelector").attr("readonly", "true").attr("disabled", "true");

            //納車予定日時
            $("#DetailSPlanDeriveredLabel").addClass(C_SC3240201CLASS_TEXTBLACK);
            $("#DetailSPlanDeriveredDateTimeSelector").attr("readonly", "true").attr("disabled", "true");
            $("#DetailLPlanDeriveredLabel").addClass(C_SC3240201CLASS_TEXTBLACK);
            $("#DetailLPlanDeriveredDateTimeSelector").attr("readonly", "true").attr("disabled", "true");

            //整備種類
            $("#DetailSMaintenanceTypeLabel").addClass(C_SC3240201CLASS_TEXTBLACK);
            $("#DetailLMaintenanceTypeLabel").addClass(C_SC3240201CLASS_TEXTBLACK);
            $("#DetailSMaintenanceTypeList").attr("disabled", "true");
            $("#DetailLMaintenanceTypeList").attr("disabled", "true");

            //整備名
            $("#DetailSMercLabel").addClass(C_SC3240201CLASS_TEXTBLACK);
            $("#DetailLMercLabel").addClass(C_SC3240201CLASS_TEXTBLACK);
            $("#DetailSMercList").attr("disabled", "true");
            $("#DetailLMercList").attr("disabled", "true");

            // 2016/10/04 NSK 秋田谷 チップ詳細の実績時間を変更できなくする対応 START
            // 処理はSetReadOnlyProcessStartEndに集約する。
            //作業終了実績日時
            // $("#DetailSProcessFinishLabel").addClass(C_SC3240201CLASS_TEXTBLACK);
            // $("#DetailSProcessFinishDateTimeSelector").attr("readonly", "true").attr("disabled", "true");
            // $("#DetailLProcessFinishLabel").addClass(C_SC3240201CLASS_TEXTBLACK);
            // $("#DetailLProcessFinishDateTimeSelector").attr("readonly", "true").attr("disabled", "true");
            // 2016/10/04 NSK 秋田谷 チップ詳細の実績時間を変更できなくする対応 END

            //作業時間
            SetReadOnlyDetailWorkTime();

            //予約有無・洗車有無・待ち方を変更不可に設定
            SetDetailReservationArea(false);
            SetDetailCarWashArea(false);
            SetDetailWaitingArea(false);
            //2013/12/04 TMEJ 下村 タブレット版SMB チーフテクニシャン機能開発 START
            SetDetailCompleteExaminationArea(false);
            //2013/12/04 TMEJ 下村 タブレット版SMB チーフテクニシャン機能開発 END

            //作業完了チップは編集不可
            //if (stallUseStatus == "03") {
            //    //チップエリアの見た目を読取専用(文字を青→黒、チェックを青→グレー)にする
            //    SetReadOnlyDetailChipArea();
            //}

            ////チップ詳細(小)整備エリアの1行表示用チップ情報をEmptyにする
            //SetDetailSSingleLineTextEmpty();

            //チップエリアの見た目を読取専用(文字を青→黒、チェックを青→グレー)にする
            SetReadOnlyDetailChipArea();

            //ご用命
            $("#DetailSOrderTxt").addClass(C_SC3240201CLASS_TEXTBLACK).attr("readonly", "true");
            $("#DetailLOrderTxt").addClass(C_SC3240201CLASS_TEXTBLACK).attr("readonly", "true");

            //2013/12/04 TMEJ 下村 タブレット版SMB チーフテクニシャン機能開発 START
            ////ご用命(親R/O)・故障原因(追加作業)・診断結果(追加作業)・アドバイス(親R/O)の活性／非活性制御
            //SetReadOnlyTextArea();

            //メモ
            $("#DetailSMemoTxt").addClass(C_SC3240201CLASS_TEXTBLACK).attr("readonly", "true");
            $("#DetailLMemoTxt").addClass(C_SC3240201CLASS_TEXTBLACK).attr("readonly", "true");
            //2013/12/04 TMEJ 下村 タブレット版SMB チーフテクニシャン機能開発 END

            break;

        //洗車・納車待ちサブチップボックス 
        case C_FT_BTNTP_WAITING_WASH:
        case C_FT_BTNTP_WASHING:
        case C_FT_BTNTP_WAIT_DELIVERY:

            //来店予定日時
            $("#DetailSPlanVisitLabel").addClass(C_SC3240201CLASS_TEXTBLACK);
            $("#DetailSPlanVisitDateTimeSelector").attr("readonly", "true").attr("disabled", "true");
            $("#DetailLPlanVisitLabel").addClass(C_SC3240201CLASS_TEXTBLACK);
            $("#DetailLPlanVisitDateTimeSelector").attr("readonly", "true").attr("disabled", "true");

            //作業開始予定日時
            $("#DetailSPlanStartLabel").addClass(C_SC3240201CLASS_TEXTBLACK);
            $("#DetailSPlanStartDateTimeSelector").attr("readonly", "true").attr("disabled", "true");
            $("#DetailLPlanStartLabel").addClass(C_SC3240201CLASS_TEXTBLACK);
            $("#DetailLPlanStartDateTimeSelector").attr("readonly", "true").attr("disabled", "true");

            //作業終了予定日時
            $("#DetailSPlanFinishLabel").addClass(C_SC3240201CLASS_TEXTBLACK);
            $("#DetailSPlanFinishDateTimeSelector").attr("readonly", "true").attr("disabled", "true");
            $("#DetailLPlanFinishLabel").addClass(C_SC3240201CLASS_TEXTBLACK);
            $("#DetailLPlanFinishDateTimeSelector").attr("readonly", "true").attr("disabled", "true");

            //納車予定日時
            $("#DetailSPlanDeriveredLabel").addClass(C_SC3240201CLASS_TEXTBLACK);
            $("#DetailSPlanDeriveredDateTimeSelector").attr("readonly", "true").attr("disabled", "true");
            $("#DetailLPlanDeriveredLabel").addClass(C_SC3240201CLASS_TEXTBLACK);
            $("#DetailLPlanDeriveredDateTimeSelector").attr("readonly", "true").attr("disabled", "true");

            //整備種類
            $("#DetailSMaintenanceTypeLabel").addClass(C_SC3240201CLASS_TEXTBLACK);
            $("#DetailLMaintenanceTypeLabel").addClass(C_SC3240201CLASS_TEXTBLACK);
            $("#DetailSMaintenanceTypeList").attr("disabled", "true");
            $("#DetailLMaintenanceTypeList").attr("disabled", "true");

            //整備名
            $("#DetailSMercLabel").addClass(C_SC3240201CLASS_TEXTBLACK);
            $("#DetailLMercLabel").addClass(C_SC3240201CLASS_TEXTBLACK);
            $("#DetailSMercList").attr("disabled", "true");
            $("#DetailLMercList").attr("disabled", "true");

            //作業時間
            SetReadOnlyDetailWorkTime();

            //予約有無・洗車有無・待ち方を変更不可に設定
            SetDetailReservationArea(false);
            SetDetailCarWashArea(false);
            SetDetailWaitingArea(false);
            //2013/12/04 TMEJ 下村 タブレット版SMB チーフテクニシャン機能開発 START
            SetDetailCompleteExaminationArea(false);
            //2013/12/04 TMEJ 下村 タブレット版SMB チーフテクニシャン機能開発 END

            //作業完了チップは編集不可
            //if (stallUseStatus == "03") {
            //    //チップエリアの見た目を読取専用(文字を青→黒、チェックを青→グレー)にする
            //    SetReadOnlyDetailChipArea();
            //}

            ////チップ詳細(小)整備エリアの1行表示用チップ情報をEmptyにする
            //SetDetailSSingleLineTextEmpty();

            //チップエリアの見た目を読取専用(文字を青→黒、チェックを青→グレー)にする
            SetReadOnlyDetailChipArea();

            //ご用命
            $("#DetailSOrderTxt").addClass(C_SC3240201CLASS_TEXTBLACK).attr("readonly", "true");
            $("#DetailLOrderTxt").addClass(C_SC3240201CLASS_TEXTBLACK).attr("readonly", "true");

            //2013/12/04 TMEJ 下村 タブレット版SMB チーフテクニシャン機能開発 START
            ////ご用命(親R/O)・故障原因(追加作業)・診断結果(追加作業)・アドバイス(親R/O)の活性／非活性制御
            //SetReadOnlyTextArea();

            //メモ
            $("#DetailSMemoTxt").addClass(C_SC3240201CLASS_TEXTBLACK).attr("readonly", "true");
            $("#DetailLMemoTxt").addClass(C_SC3240201CLASS_TEXTBLACK).attr("readonly", "true");
            //2013/12/04 TMEJ 下村 タブレット版SMB チーフテクニシャン機能開発 END

            break;

        //No Showサブチップボックス 
        case C_FT_BTNTP_NOSHOW:

            //作業開始予定日時
            $("#DetailSPlanStartLabel").addClass(C_SC3240201CLASS_TEXTBLACK);
            $("#DetailSPlanStartDateTimeSelector").attr("readonly", "true").attr("disabled", "true");
            $("#DetailLPlanStartLabel").addClass(C_SC3240201CLASS_TEXTBLACK);
            $("#DetailLPlanStartDateTimeSelector").attr("readonly", "true").attr("disabled", "true");

            //作業終了予定日時
            $("#DetailSPlanFinishLabel").addClass(C_SC3240201CLASS_TEXTBLACK);
            $("#DetailSPlanFinishDateTimeSelector").attr("readonly", "true").attr("disabled", "true");
            $("#DetailLPlanFinishLabel").addClass(C_SC3240201CLASS_TEXTBLACK);
            $("#DetailLPlanFinishDateTimeSelector").attr("readonly", "true").attr("disabled", "true");

            // 2016/10/04 NSK 秋田谷 チップ詳細の実績時間を変更できなくする対応 START
            //作業開始実績日時
            // $("#DetailSProcessStartLabel").addClass(C_SC3240201CLASS_TEXTBLACK);
            // $("#DetailSProcessStartDateTimeSelector").attr("readonly", "true").attr("disabled", "true");
            // $("#DetailLProcessStartLabel").addClass(C_SC3240201CLASS_TEXTBLACK);
            // $("#DetailLProcessStartDateTimeSelector").attr("readonly", "true").attr("disabled", "true");

            //作業終了実績日時
            // $("#DetailSProcessFinishLabel").addClass(C_SC3240201CLASS_TEXTBLACK);
            // $("#DetailSProcessFinishDateTimeSelector").attr("readonly", "true").attr("disabled", "true");
            // $("#DetailLProcessFinishLabel").addClass(C_SC3240201CLASS_TEXTBLACK);
            // $("#DetailLProcessFinishDateTimeSelector").attr("readonly", "true").attr("disabled", "true");
            // 2016/10/04 NSK 秋田谷 チップ詳細の実績時間を変更できなくする対応 END

            //作業時間
            SetReadOnlyDetailWorkTime();

            //予約有無・洗車有無・待ち方を変更不可に設定
            SetDetailReservationArea(false);
            SetDetailCarWashArea(false);
            SetDetailWaitingArea(false);
            //2013/12/04 TMEJ 下村 タブレット版SMB チーフテクニシャン機能開発 START
            SetDetailCompleteExaminationArea(false);
            //2013/12/04 TMEJ 下村 タブレット版SMB チーフテクニシャン機能開発 END

            //作業完了チップは編集不可
            //if (stallUseStatus == "03") {
            //    //チップエリアの見た目を読取専用(文字を青→黒、チェックを青→グレー)にする
            //    SetReadOnlyDetailChipArea();
            //}

            ////チップ詳細(小)整備エリアの1行表示用チップ情報をEmptyにする
            //SetDetailSSingleLineTextEmpty();

            //チップエリアの見た目を読取専用(文字を青→黒、チェックを青→グレー)にする
            SetReadOnlyDetailChipArea();

            //ご用命
            $("#DetailSOrderTxt").addClass(C_SC3240201CLASS_TEXTBLACK).attr("readonly", "true");
            $("#DetailLOrderTxt").addClass(C_SC3240201CLASS_TEXTBLACK).attr("readonly", "true");

            //2013/12/04 TMEJ 下村 タブレット版SMB チーフテクニシャン機能開発 START
            ////RO番号がない場合（RO発行前）、編集不可
            //if (orderNo == "") {
                ////故障原因
                //$("#DetailSFailureTxt").addClass(C_SC3240201CLASS_TEXTBLACK).attr("readonly", "true");
                //$("#DetailLFailureTxt").addClass(C_SC3240201CLASS_TEXTBLACK).attr("readonly", "true");

                ////診断結果
                //$("#DetailSResultTxt").addClass(C_SC3240201CLASS_TEXTBLACK).attr("readonly", "true");
                //$("#DetailLResultTxt").addClass(C_SC3240201CLASS_TEXTBLACK).attr("readonly", "true");

                ////アドバイス
                //$("#DetailSAdviceTxt").addClass(C_SC3240201CLASS_TEXTBLACK).attr("readonly", "true");
                //$("#DetailLAdviceTxt").addClass(C_SC3240201CLASS_TEXTBLACK).attr("readonly", "true");
            //}
    	
            ////ご用命(親R/O)・故障原因(追加作業)・診断結果(追加作業)・アドバイス(親R/O)の活性／非活性制御
            //SetReadOnlyTextArea();

            //メモ
            $("#DetailSMemoTxt").addClass(C_SC3240201CLASS_TEXTBLACK).attr("readonly", "true");
            $("#DetailLMemoTxt").addClass(C_SC3240201CLASS_TEXTBLACK).attr("readonly", "true");
            //2013/12/04 TMEJ 下村 タブレット版SMB チーフテクニシャン機能開発 END

            break;

        //中断サブチップボックス 
        case C_FT_BTNTP_STOP:

            //来店予定日時
            $("#DetailSPlanVisitLabel").addClass(C_SC3240201CLASS_TEXTBLACK);
            $("#DetailSPlanVisitDateTimeSelector").attr("readonly", "true").attr("disabled", "true");
            $("#DetailLPlanVisitLabel").addClass(C_SC3240201CLASS_TEXTBLACK);
            $("#DetailLPlanVisitDateTimeSelector").attr("readonly", "true").attr("disabled", "true");

            //作業開始予定日時
            $("#DetailSPlanStartLabel").addClass(C_SC3240201CLASS_TEXTBLACK);
            $("#DetailSPlanStartDateTimeSelector").attr("readonly", "true").attr("disabled", "true");
            $("#DetailLPlanStartLabel").addClass(C_SC3240201CLASS_TEXTBLACK);
            $("#DetailLPlanStartDateTimeSelector").attr("readonly", "true").attr("disabled", "true");

            //作業終了予定日時
            $("#DetailSPlanFinishLabel").addClass(C_SC3240201CLASS_TEXTBLACK);
            $("#DetailSPlanFinishDateTimeSelector").attr("readonly", "true").attr("disabled", "true");
            $("#DetailLPlanFinishLabel").addClass(C_SC3240201CLASS_TEXTBLACK);
            $("#DetailLPlanFinishDateTimeSelector").attr("readonly", "true").attr("disabled", "true");

            //納車予定日時
            $("#DetailSPlanDeriveredLabel").addClass(C_SC3240201CLASS_TEXTBLACK);
            $("#DetailSPlanDeriveredDateTimeSelector").attr("readonly", "true").attr("disabled", "true");
            $("#DetailLPlanDeriveredLabel").addClass(C_SC3240201CLASS_TEXTBLACK);
            $("#DetailLPlanDeriveredDateTimeSelector").attr("readonly", "true").attr("disabled", "true");

            // 2016/10/04 NSK 秋田谷 チップ詳細の実績時間を変更できなくする対応 START
            //作業開始実績日時
            // $("#DetailSProcessStartLabel").addClass(C_SC3240201CLASS_TEXTBLACK);
            // $("#DetailSProcessStartDateTimeSelector").attr("readonly", "true").attr("disabled", "true");
            // $("#DetailLProcessStartLabel").addClass(C_SC3240201CLASS_TEXTBLACK);
            // $("#DetailLProcessStartDateTimeSelector").attr("readonly", "true").attr("disabled", "true");

            //作業終了実績日時
            // $("#DetailSProcessFinishLabel").addClass(C_SC3240201CLASS_TEXTBLACK);
            // $("#DetailSProcessFinishDateTimeSelector").attr("readonly", "true").attr("disabled", "true");
            // $("#DetailLProcessFinishLabel").addClass(C_SC3240201CLASS_TEXTBLACK);
            // $("#DetailLProcessFinishDateTimeSelector").attr("readonly", "true").attr("disabled", "true");
            // 2016/10/04 NSK 秋田谷 チップ詳細の実績時間を変更できなくする対応 END

            //作業時間
            SetReadOnlyDetailWorkTime();

            //予約有無・洗車有無・待ち方を変更不可に設定
            SetDetailReservationArea(false);
            SetDetailCarWashArea(false);
            SetDetailWaitingArea(false);
            //2013/12/04 TMEJ 下村 タブレット版SMB チーフテクニシャン機能開発 START
            SetDetailCompleteExaminationArea(false);
            //2013/12/04 TMEJ 下村 タブレット版SMB チーフテクニシャン機能開発 END

			//作業完了チップは編集不可
            if (stallUseStatus == "03") {
                //チップエリアの見た目を読取専用(文字を青→黒、チェックを青→グレー)にする
                SetReadOnlyDetailChipArea();
            }

            //選択できるチップが存在する場合
            if (0 < $("#DetailSMaintenanceDl").find("div").length) {
            	//2013/12/04 TMEJ 下村 タブレット版SMB チーフテクニシャン機能開発 START
                ////チップエリアのイベントを設定
                //SetEventDetailSChipArea();
                //SetEventDetailLChipArea();
                if (stallUseStatus != "03") {
	                //チップエリアのイベントを設定
	                SetEventDetailSChipArea();
	                SetEventDetailLChipArea();
            	}
            	//2013/12/04 TMEJ 下村 タブレット版SMB チーフテクニシャン機能開発 END
            }
            else {
                //チップ詳細(小)整備エリアの1行表示用チップ情報をEmptyにする
                SetDetailSSingleLineTextEmpty();
            }

            //ご用命
            $("#DetailSOrderTxt").addClass(C_SC3240201CLASS_TEXTBLACK).attr("readonly", "true");
            $("#DetailLOrderTxt").addClass(C_SC3240201CLASS_TEXTBLACK).attr("readonly", "true");

            //2013/12/04 TMEJ 下村 タブレット版SMB チーフテクニシャン機能開発 START
            ////ご用命(親R/O)・故障原因(追加作業)・診断結果(追加作業)・アドバイス(親R/O)の活性／非活性制御
            //SetReadOnlyTextArea();

            //メモ
            $("#DetailSMemoTxt").addClass(C_SC3240201CLASS_TEXTBLACK).attr("readonly", "true");
            $("#DetailLMemoTxt").addClass(C_SC3240201CLASS_TEXTBLACK).attr("readonly", "true");
            //2013/12/04 TMEJ 下村 タブレット版SMB チーフテクニシャン機能開発 END

            break;

        default:
            break;
    }
}

/**	
* チップ詳細(小)整備エリアの1行表示用チップ情報をEmptyにする
* 	
*/
function SetDetailSSingleLineTextEmpty() {
    var detailSTableStallDl = $("#detailSTableChipUl dl");
    detailSTableStallDl.each(function () {
        $(this).find("dd").children(".SingleLine").text("");
    });
}

/**	
* ストール上のチップからチップ詳細が開かれた場合、
* ステータス等によって編集できる項目を変更する
* 	
*/
function SetChipDetailByStatusEtc() {

    var wkStatus; 

    //サービス入庫.サービスステータス
    var svcStatus = $("#ChipDetailSvcStatusHidden").val().Trim();

    //ストール利用.ストール利用ステータス
    var stallUseStatus = $("#ChipDetailStallUseStatusHidden").val().Trim();

    //サービス入庫.予約ステータス
    var resvStatus = $("#ChipDetailResvStatusHidden").val().Trim();

    //RO番号
    var orderNo = $("#ChipDetailOrderNoHidden").val().Trim();

    //来店実績日時
    var processVisit = $("#DetailSProcessVisitTimeLabel").attr("datetime");


    //チップの状態を決定する
    //サービス入庫．サービスステータス＝"13：納車済み"の場合
    if (svcStatus == "13") {

        wkStatus = C_SC3240201_CP_DELI;                                //『納車済み』
    }
    //サービス入庫．サービスステータス＝"11：預かり中(Drop Off)"、もしくは"12：納車待ち(Waiting)"の場合
    else if (svcStatus == "11" || svcStatus == "12") {

        wkStatus = C_SC3240201_WAIT_DELI;                              //『納車待ち』
    }
    //サービス入庫．サービスステータス＝"08：洗車中"の場合
    else if (svcStatus == "08") {

        wkStatus = C_SC3240201_WASHING;                                //『洗車中』
    }
    //サービス入庫．サービスステータス＝"07：洗車待ち"の場合
    else if (svcStatus == "07") {

        wkStatus = C_SC3240201_WAIT_WASH;                              //『洗車待ち』
    }
    //サービス入庫．サービスステータス＝"09：検査待ち"の場合
    else if (svcStatus == "09") {

        wkStatus = C_SC3240201_WAIT_INSPECTION;                        //『検査待ち』 ※『洗車待ち(C_SC3240201_WAIT_WASH)』と同じ読取り設定
    }
    //サービス入庫．サービスステータス＝"10：検査中"の場合
    else if (svcStatus == "10") {

        wkStatus = C_SC3240201_INSPECTION;                             //『検査中』 ※『洗車待ち(C_SC3240201_WAIT_WASH)』と同じ読取り設定
    }
    //ストール利用．ストール利用ステータス＝"03：完了"の場合
    else if (stallUseStatus == "03") {

        wkStatus = C_SC3240201_FINISH_WORK;                            //『作業完了』 ※『洗車待ち(C_SC3240201_WAIT_WASH)』と同じ読取り設定
    }
    //ストール利用．ストール利用ステータス＝"06：日跨ぎ終了"の場合
    else if (stallUseStatus == "06") {

        wkStatus = C_SC3240201_MIDFINISH;                              //『日跨ぎ終了』 ※『洗車待ち(C_SC3240201_WAIT_WASH)』と同じ読取り設定
    }
    //ストール利用．ストール利用ステータス＝"01：作業開始待ち"の場合
    else if (stallUseStatus == "01") {

        wkStatus = C_SC3240201_WORK_ORDER;                             //『着工指示済み』 ※『中断再配置(C_SC3240201_REPOST)』と同じ読取り設定
    }
    //ストール利用．ストール利用ステータス＝"05：中断"の場合
    else if (stallUseStatus == "05") {

        wkStatus = C_SC3240201_STOP;                                   //『中断実績』
    }
    //ストール利用．ストール利用ステータス＝"02：作業中"の場合
    else if (stallUseStatus == "02") {

        wkStatus = C_SC3240201_WORKING;                                //『作業中』
    }
    //サービス入庫．実績入庫日時がある場合　※'1900/01/01 00:00:00'でない場合
    else if (IsMinDate(processVisit) == false) {

        wkStatus = C_SC3240201_CARIN;                                  //『入庫済み』
    }
    //サービス入庫．実績入庫日時がない場合　※'1900/01/01 00:00:00'の場合
    //且つ、予約ステータス＝"1：本予約"の場合
    else if (IsMinDate(processVisit) == true && resvStatus == "1") {

        wkStatus = C_SC3240201_REZ_COMMITTED;                          //『本予約』
    }
    //サービス入庫．実績入庫日時がない場合　※'1900/01/01 00:00:00'の場合
    //且つ、予約ステータス＝"0：仮予約"の場合
    else if (IsMinDate(processVisit) == true && resvStatus == "0") {

        wkStatus = C_SC3240201_REZ_TEMP;                               //『仮予約』
    }
    //上記以外
    else {

        wkStatus = C_SC3240201_REZ_TEMP;                               //『仮予約』と同じ読取り設定
    }

    //チップの状態（ステータス）によって処理を分岐
    switch (wkStatus) {

        //仮予約、本予約
        case C_SC3240201_REZ_TEMP:
        case C_SC3240201_REZ_COMMITTED:

            // 2016/10/04 NSK 秋田谷 チップ詳細の実績時間を変更できなくする対応 START
            //作業開始実績日時
            // $("#DetailSProcessStartLabel").addClass(C_SC3240201CLASS_TEXTBLACK);
            // $("#DetailSProcessStartDateTimeSelector").attr("readonly", "true").attr("disabled", "true");
            // $("#DetailLProcessStartLabel").addClass(C_SC3240201CLASS_TEXTBLACK);
            // $("#DetailLProcessStartDateTimeSelector").attr("readonly", "true").attr("disabled", "true");

            //作業完了実績日時
            // $("#DetailSProcessFinishLabel").addClass(C_SC3240201CLASS_TEXTBLACK);
            // $("#DetailSProcessFinishDateTimeSelector").attr("readonly", "true").attr("disabled", "true");
            // $("#DetailLProcessFinishLabel").addClass(C_SC3240201CLASS_TEXTBLACK);
            // $("#DetailLProcessFinishDateTimeSelector").attr("readonly", "true").attr("disabled", "true");
            // 2016/10/04 NSK 秋田谷 チップ詳細の実績時間を変更できなくする対応 END

            //予約有無・洗車有無・待ち方を変更可能に設定
            SetDetailReservationArea(true);
            SetDetailCarWashArea(true);
            SetDetailWaitingArea(true);
            //2013/12/04 TMEJ 下村 タブレット版SMB チーフテクニシャン機能開発 START
            SetDetailCompleteExaminationArea(true);
            //2013/12/04 TMEJ 下村 タブレット版SMB チーフテクニシャン機能開発 END

            //チップエリアの見た目を読取専用(文字を青→黒、チェックを青→グレー)にする
            SetReadOnlyDetailChipArea();

            //2013/12/04 TMEJ 下村 タブレット版SMB チーフテクニシャン機能開発 START
            ////ご用命(親R/O)・故障原因(追加作業)・診断結果(追加作業)・アドバイス(親R/O)の活性／非活性制御
            //SetReadOnlyTextArea();

            //サブチップボックスならば数値が返却される
            var subAreaId = GetSubChipType(gSelectedChipId);

            //受付・追加作業エリアの場合
            if (subAreaId != "") {
	            //ご用命
	            $("#DetailSOrderTxt").addClass(C_SC3240201CLASS_TEXTBLACK).attr("readonly", "true");
	            $("#DetailLOrderTxt").addClass(C_SC3240201CLASS_TEXTBLACK).attr("readonly", "true");
            }
    	    
            //メモを非活性にする
            $("#DetailSMemoTxt").addClass(C_SC3240201CLASS_TEXTBLACK).attr("readonly", "true");
            $("#DetailLMemoTxt").addClass(C_SC3240201CLASS_TEXTBLACK).attr("readonly", "true");
            //2013/12/04 TMEJ 下村 タブレット版SMB チーフテクニシャン機能開発 END

            break;

        //入庫済み
        case C_SC3240201_CARIN:

            //来店予定日時
            $("#DetailSPlanVisitLabel").addClass(C_SC3240201CLASS_TEXTBLACK);
            $("#DetailSPlanVisitDateTimeSelector").attr("readonly", "true").attr("disabled", "true");
            $("#DetailLPlanVisitLabel").addClass(C_SC3240201CLASS_TEXTBLACK);
            $("#DetailLPlanVisitDateTimeSelector").attr("readonly", "true").attr("disabled", "true");

            //納車予定日時
            $("#DetailSPlanDeriveredLabel").addClass(C_SC3240201CLASS_TEXTBLACK);
            $("#DetailSPlanDeriveredDateTimeSelector").attr("readonly", "true").attr("disabled", "true");
            $("#DetailLPlanDeriveredLabel").addClass(C_SC3240201CLASS_TEXTBLACK);
            $("#DetailLPlanDeriveredDateTimeSelector").attr("readonly", "true").attr("disabled", "true");

            // 2016/10/04 NSK 秋田谷 チップ詳細の実績時間を変更できなくする対応 START
            //作業開始実績日時
            // $("#DetailSProcessStartLabel").addClass(C_SC3240201CLASS_TEXTBLACK);
            // $("#DetailSProcessStartDateTimeSelector").attr("readonly", "true").attr("disabled", "true");
            // $("#DetailLProcessStartLabel").addClass(C_SC3240201CLASS_TEXTBLACK);
            // $("#DetailLProcessStartDateTimeSelector").attr("readonly", "true").attr("disabled", "true");

            //作業完了実績日時
            // $("#DetailSProcessFinishLabel").addClass(C_SC3240201CLASS_TEXTBLACK);
            // $("#DetailSProcessFinishDateTimeSelector").attr("readonly", "true").attr("disabled", "true");
            // $("#DetailLProcessFinishLabel").addClass(C_SC3240201CLASS_TEXTBLACK);
            // $("#DetailLProcessFinishDateTimeSelector").attr("readonly", "true").attr("disabled", "true");
            // 2016/10/04 NSK 秋田谷 チップ詳細の実績時間を変更できなくする対応 END

            //予約有無・洗車有無・待ち方を変更不可に設定
            SetDetailReservationArea(false);
            SetDetailCarWashArea(false);
            SetDetailWaitingArea(false);
            //2013/12/04 TMEJ 下村 タブレット版SMB チーフテクニシャン機能開発 START
            //2014/07/07 TMEJ 張 UAT-114 不具合対応 START
            //            SetDetailCompleteExaminationArea(false);
            SetDetailCompleteExaminationArea(true);
            //2014/07/07 TMEJ 張 UAT-114 不具合対応 END
            //2013/12/04 TMEJ 下村 タブレット版SMB チーフテクニシャン機能開発 END

            ////RO番号がない場合（RO発行前）、もしくは、作業完了チップは編集不可
            //if (orderNo == "" || stallUseStatus == "03") {

            //RO番号がない場合（RO発行前）は編集不可
            if (orderNo == "") {

                //チップエリアの見た目を読取専用(文字を青→黒、チェックを青→グレー)にする
                SetReadOnlyDetailChipArea();
            }

            //選択できるチップが存在する場合
            if (0 < $("#DetailSMaintenanceDl").find("div").length) {
                //チップエリアのイベントを設定
                SetEventDetailSChipArea();
                SetEventDetailLChipArea();
            }

            //ご用命
            $("#DetailSOrderTxt").addClass(C_SC3240201CLASS_TEXTBLACK).attr("readonly", "true");
            $("#DetailLOrderTxt").addClass(C_SC3240201CLASS_TEXTBLACK).attr("readonly", "true");

            //2013/12/04 TMEJ 下村 タブレット版SMB チーフテクニシャン機能開発 START
            ////ご用命(親R/O)・故障原因(追加作業)・診断結果(追加作業)・アドバイス(親R/O)の活性／非活性制御
            //SetReadOnlyTextArea();

            //メモを非活性にする
            $("#DetailSMemoTxt").addClass(C_SC3240201CLASS_TEXTBLACK).attr("readonly", "true");
            $("#DetailLMemoTxt").addClass(C_SC3240201CLASS_TEXTBLACK).attr("readonly", "true");
            //2013/12/04 TMEJ 下村 タブレット版SMB チーフテクニシャン機能開発 END

            break;

        //着工指示済み、中断再配置
        case C_SC3240201_WORK_ORDER:
        case C_SC3240201_REPOST:
            //来店予定日時
            $("#DetailSPlanVisitLabel").addClass(C_SC3240201CLASS_TEXTBLACK);
            $("#DetailSPlanVisitDateTimeSelector").attr("readonly", "true").attr("disabled", "true");
            $("#DetailLPlanVisitLabel").addClass(C_SC3240201CLASS_TEXTBLACK);
            $("#DetailLPlanVisitDateTimeSelector").attr("readonly", "true").attr("disabled", "true");

            //納車予定日時
            $("#DetailSPlanDeriveredLabel").addClass(C_SC3240201CLASS_TEXTBLACK);
            $("#DetailSPlanDeriveredDateTimeSelector").attr("readonly", "true").attr("disabled", "true");
            $("#DetailLPlanDeriveredLabel").addClass(C_SC3240201CLASS_TEXTBLACK);
            $("#DetailLPlanDeriveredDateTimeSelector").attr("readonly", "true").attr("disabled", "true");

            // 2016/10/04 NSK 秋田谷 チップ詳細の実績時間を変更できなくする対応 START
            //作業開始実績日時
            // $("#DetailSProcessStartLabel").addClass(C_SC3240201CLASS_TEXTBLACK);
            // $("#DetailSProcessStartDateTimeSelector").attr("readonly", "true").attr("disabled", "true");
            // $("#DetailLProcessStartLabel").addClass(C_SC3240201CLASS_TEXTBLACK);
            // $("#DetailLProcessStartDateTimeSelector").attr("readonly", "true").attr("disabled", "true");

            //作業完了実績日時
            // $("#DetailSProcessFinishLabel").addClass(C_SC3240201CLASS_TEXTBLACK);
            // $("#DetailSProcessFinishDateTimeSelector").attr("readonly", "true").attr("disabled", "true");
            // $("#DetailLProcessFinishLabel").addClass(C_SC3240201CLASS_TEXTBLACK);
            // $("#DetailLProcessFinishDateTimeSelector").attr("readonly", "true").attr("disabled", "true");
            // 2016/10/04 NSK 秋田谷 チップ詳細の実績時間を変更できなくする対応 END


            //予約有無・洗車有無・待ち方を変更不可に設定
            SetDetailReservationArea(false);
            SetDetailCarWashArea(false);
            SetDetailWaitingArea(false);
            //2013/12/04 TMEJ 下村 タブレット版SMB チーフテクニシャン機能開発 START
            //2014/07/07 TMEJ 張 UAT-114 不具合対応 START
            //            SetDetailCompleteExaminationArea(false);
            SetDetailCompleteExaminationArea(true);
            //2014/07/07 TMEJ 張 UAT-114 不具合対応 END
            //2013/12/04 TMEJ 下村 タブレット版SMB チーフテクニシャン機能開発 END

            //作業完了チップは編集不可
            //if (stallUseStatus == "03") {
            //
            //    //チップエリアの見た目を読取専用(文字を青→黒、チェックを青→グレー)にする
            //    SetReadOnlyDetailChipArea();
            //}

            //選択できるチップが存在する場合
            if (0 < $("#DetailSMaintenanceDl").find("div").length) {
                //チップエリアのイベントを設定
                SetEventDetailSChipArea();
                SetEventDetailLChipArea();
            }

            //ご用命
            $("#DetailSOrderTxt").addClass(C_SC3240201CLASS_TEXTBLACK).attr("readonly", "true");
            $("#DetailLOrderTxt").addClass(C_SC3240201CLASS_TEXTBLACK).attr("readonly", "true");

            //2013/12/04 TMEJ 下村 タブレット版SMB チーフテクニシャン機能開発 START
            ////ご用命(親R/O)・故障原因(追加作業)・診断結果(追加作業)・アドバイス(親R/O)の活性／非活性制御
            //SetReadOnlyTextArea();

            //メモを非活性にする
            $("#DetailSMemoTxt").addClass(C_SC3240201CLASS_TEXTBLACK).attr("readonly", "true");
            $("#DetailLMemoTxt").addClass(C_SC3240201CLASS_TEXTBLACK).attr("readonly", "true");
            //2013/12/04 TMEJ 下村 タブレット版SMB チーフテクニシャン機能開発 END

            break;

        //作業中
        case C_SC3240201_WORKING:
            //来店予定日時
            $("#DetailSPlanVisitLabel").addClass(C_SC3240201CLASS_TEXTBLACK);
            $("#DetailSPlanVisitDateTimeSelector").attr("readonly", "true").attr("disabled", "true");
            $("#DetailLPlanVisitLabel").addClass(C_SC3240201CLASS_TEXTBLACK);
            $("#DetailLPlanVisitDateTimeSelector").attr("readonly", "true").attr("disabled", "true");

            //作業開始予定日時
            $("#DetailSPlanStartLabel").addClass(C_SC3240201CLASS_TEXTBLACK);
            $("#DetailSPlanStartDateTimeSelector").attr("readonly", "true").attr("disabled", "true");
            $("#DetailLPlanStartLabel").addClass(C_SC3240201CLASS_TEXTBLACK);
            $("#DetailLPlanStartDateTimeSelector").attr("readonly", "true").attr("disabled", "true");

            //納車予定日時
            $("#DetailSPlanDeriveredLabel").addClass(C_SC3240201CLASS_TEXTBLACK);
            $("#DetailSPlanDeriveredDateTimeSelector").attr("readonly", "true").attr("disabled", "true");
            $("#DetailLPlanDeriveredLabel").addClass(C_SC3240201CLASS_TEXTBLACK);
            $("#DetailLPlanDeriveredDateTimeSelector").attr("readonly", "true").attr("disabled", "true");

            // 2016/10/04 NSK 秋田谷 チップ詳細の実績時間を変更できなくする対応 START
            // 作業完了実績日時
            // $("#DetailSProcessFinishLabel").addClass(C_SC3240201CLASS_TEXTBLACK);
            // $("#DetailSProcessFinishDateTimeSelector").attr("readonly", "true").attr("disabled", "true");
            // $("#DetailLProcessFinishLabel").addClass(C_SC3240201CLASS_TEXTBLACK);
            // $("#DetailLProcessFinishDateTimeSelector").attr("readonly", "true").attr("disabled", "true");
            // 2016/10/04 NSK 秋田谷 チップ詳細の実績時間を変更できなくする対応 END

            //作業完了予定日時
            $("#DetailSPlanFinishLabel").addClass(C_SC3240201CLASS_TEXTBLACK);
            $("#DetailSPlanFinishDateTimeSelector").attr("readonly", "true").attr("disabled", "true");
            $("#DetailLPlanFinishLabel").addClass(C_SC3240201CLASS_TEXTBLACK);
            $("#DetailLPlanFinishDateTimeSelector").attr("readonly", "true").attr("disabled", "true");

            //予約有無・洗車有無・待ち方を変更不可に設定
            SetDetailReservationArea(false);
            SetDetailCarWashArea(false);
            SetDetailWaitingArea(false);
            //2013/12/04 TMEJ 下村 タブレット版SMB チーフテクニシャン機能開発 START
            //2014/07/07 TMEJ 張 UAT-114 不具合対応 START
            //            SetDetailCompleteExaminationArea(false);
            SetDetailCompleteExaminationArea(true);
            //2014/07/07 TMEJ 張 UAT-114 不具合対応 END
            //2013/12/04 TMEJ 下村 タブレット版SMB チーフテクニシャン機能開発 END

            //作業完了チップは編集不可
            //if (stallUseStatus == "03") {
            //
            //    //チップエリアの見た目を読取専用(文字を青→黒、チェックを青→グレー)にする
            //    SetReadOnlyDetailChipArea();
            //}

            //選択できるチップが存在する場合
            if (0 < $("#DetailSMaintenanceDl").find("div").length) {
                //チップエリアのイベントを設定
                SetEventDetailSChipArea();
                SetEventDetailLChipArea();
            }

            //ご用命
            $("#DetailSOrderTxt").addClass(C_SC3240201CLASS_TEXTBLACK).attr("readonly", "true");
            $("#DetailLOrderTxt").addClass(C_SC3240201CLASS_TEXTBLACK).attr("readonly", "true");

            //2013/12/04 TMEJ 下村 タブレット版SMB チーフテクニシャン機能開発 START
            ////ご用命(親R/O)・故障原因(追加作業)・診断結果(追加作業)・アドバイス(親R/O)の活性／非活性制御
            //SetReadOnlyTextArea();

            //メモを非活性にする
            $("#DetailSMemoTxt").addClass(C_SC3240201CLASS_TEXTBLACK).attr("readonly", "true");
            $("#DetailLMemoTxt").addClass(C_SC3240201CLASS_TEXTBLACK).attr("readonly", "true");
            //2013/12/04 TMEJ 下村 タブレット版SMB チーフテクニシャン機能開発 END

            break;

        //中断実績、洗車待ち、洗車中、検査待ち、検査中、作業完了、日跨ぎ終了
        case C_SC3240201_STOP:
        case C_SC3240201_WAIT_WASH:
        case C_SC3240201_WASHING:
        case C_SC3240201_WAIT_INSPECTION:
        case C_SC3240201_INSPECTION:
        case C_SC3240201_FINISH_WORK:
        case C_SC3240201_MIDFINISH:

            //来店予定日時
            $("#DetailSPlanVisitLabel").addClass(C_SC3240201CLASS_TEXTBLACK);
            $("#DetailSPlanVisitDateTimeSelector").attr("readonly", "true").attr("disabled", "true");
            $("#DetailLPlanVisitLabel").addClass(C_SC3240201CLASS_TEXTBLACK);
            $("#DetailLPlanVisitDateTimeSelector").attr("readonly", "true").attr("disabled", "true");

            //作業開始予定日時
            $("#DetailSPlanStartLabel").addClass(C_SC3240201CLASS_TEXTBLACK);
            $("#DetailSPlanStartDateTimeSelector").attr("readonly", "true").attr("disabled", "true");
            $("#DetailLPlanStartLabel").addClass(C_SC3240201CLASS_TEXTBLACK);
            $("#DetailLPlanStartDateTimeSelector").attr("readonly", "true").attr("disabled", "true");

            //作業終了予定日時
            $("#DetailSPlanFinishLabel").addClass(C_SC3240201CLASS_TEXTBLACK);
            $("#DetailSPlanFinishDateTimeSelector").attr("readonly", "true").attr("disabled", "true");
            $("#DetailLPlanFinishLabel").addClass(C_SC3240201CLASS_TEXTBLACK);
            $("#DetailLPlanFinishDateTimeSelector").attr("readonly", "true").attr("disabled", "true");

            //納車予定日時
            $("#DetailSPlanDeriveredLabel").addClass(C_SC3240201CLASS_TEXTBLACK);
            $("#DetailSPlanDeriveredDateTimeSelector").attr("readonly", "true").attr("disabled", "true");
            $("#DetailLPlanDeriveredLabel").addClass(C_SC3240201CLASS_TEXTBLACK);
            $("#DetailLPlanDeriveredDateTimeSelector").attr("readonly", "true").attr("disabled", "true");

            //整備種類
            $("#DetailSMaintenanceTypeLabel").addClass(C_SC3240201CLASS_TEXTBLACK);
            $("#DetailLMaintenanceTypeLabel").addClass(C_SC3240201CLASS_TEXTBLACK);
            $("#DetailSMaintenanceTypeList").attr("disabled", "true");
            $("#DetailLMaintenanceTypeList").attr("disabled", "true");

            //整備名
            $("#DetailSMercLabel").addClass(C_SC3240201CLASS_TEXTBLACK);
            $("#DetailLMercLabel").addClass(C_SC3240201CLASS_TEXTBLACK);
            $("#DetailSMercList").attr("disabled", "true");
            $("#DetailLMercList").attr("disabled", "true");

            //作業時間
            SetReadOnlyDetailWorkTime();

            //予約有無・洗車有無・待ち方を変更不可に設定
            SetDetailReservationArea(false);
            SetDetailCarWashArea(false);
            SetDetailWaitingArea(false);
            //2013/12/04 TMEJ 下村 タブレット版SMB チーフテクニシャン機能開発 START
            SetDetailCompleteExaminationArea(false);
            //2013/12/04 TMEJ 下村 タブレット版SMB チーフテクニシャン機能開発 END

            //作業完了チップは編集不可
            //if (stallUseStatus == "03") {
            //
            //    //チップエリアの見た目を読取専用(文字を青→黒、チェックを青→グレー)にする
            //    SetReadOnlyDetailChipArea();
            //}

            //選択できるチップが存在する場合
            if (0 < $("#DetailSMaintenanceDl").find("div").length) {
                //チップエリアのイベントを設定
                SetEventDetailSChipArea();
                SetEventDetailLChipArea();
            }

            //ご用命
            $("#DetailSOrderTxt").addClass(C_SC3240201CLASS_TEXTBLACK).attr("readonly", "true");
            $("#DetailLOrderTxt").addClass(C_SC3240201CLASS_TEXTBLACK).attr("readonly", "true");

            //2013/12/04 TMEJ 下村 タブレット版SMB チーフテクニシャン機能開発 START
            ////ご用命(親R/O)・故障原因(追加作業)・診断結果(追加作業)・アドバイス(親R/O)の活性／非活性制御
            //SetReadOnlyTextArea();

            //メモを非活性にする
            $("#DetailSMemoTxt").addClass(C_SC3240201CLASS_TEXTBLACK).attr("readonly", "true");
            $("#DetailLMemoTxt").addClass(C_SC3240201CLASS_TEXTBLACK).attr("readonly", "true");
            //2013/12/04 TMEJ 下村 タブレット版SMB チーフテクニシャン機能開発 END

            break;

        //納車待ち
        case C_SC3240201_WAIT_DELI:
            //来店予定日時
            $("#DetailSPlanVisitLabel").addClass(C_SC3240201CLASS_TEXTBLACK);
            $("#DetailSPlanVisitDateTimeSelector").attr("readonly", "true").attr("disabled", "true");
            $("#DetailLPlanVisitLabel").addClass(C_SC3240201CLASS_TEXTBLACK);
            $("#DetailLPlanVisitDateTimeSelector").attr("readonly", "true").attr("disabled", "true");

            //作業開始予定日時
            $("#DetailSPlanStartLabel").addClass(C_SC3240201CLASS_TEXTBLACK);
            $("#DetailSPlanStartDateTimeSelector").attr("readonly", "true").attr("disabled", "true");
            $("#DetailLPlanStartLabel").addClass(C_SC3240201CLASS_TEXTBLACK);
            $("#DetailLPlanStartDateTimeSelector").attr("readonly", "true").attr("disabled", "true");

            //作業終了予定日時
            $("#DetailSPlanFinishLabel").addClass(C_SC3240201CLASS_TEXTBLACK);
            $("#DetailSPlanFinishDateTimeSelector").attr("readonly", "true").attr("disabled", "true");
            $("#DetailLPlanFinishLabel").addClass(C_SC3240201CLASS_TEXTBLACK);
            $("#DetailLPlanFinishDateTimeSelector").attr("readonly", "true").attr("disabled", "true");

            //納車予定日時
            $("#DetailSPlanDeriveredLabel").addClass(C_SC3240201CLASS_TEXTBLACK);
            $("#DetailSPlanDeriveredDateTimeSelector").attr("readonly", "true").attr("disabled", "true");
            $("#DetailLPlanDeriveredLabel").addClass(C_SC3240201CLASS_TEXTBLACK);
            $("#DetailLPlanDeriveredDateTimeSelector").attr("readonly", "true").attr("disabled", "true");

            //整備種類
            $("#DetailSMaintenanceTypeLabel").addClass(C_SC3240201CLASS_TEXTBLACK);
            $("#DetailLMaintenanceTypeLabel").addClass(C_SC3240201CLASS_TEXTBLACK);
            $("#DetailSMaintenanceTypeList").attr("disabled", "true");
            $("#DetailLMaintenanceTypeList").attr("disabled", "true");

            //整備名
            $("#DetailSMercLabel").addClass(C_SC3240201CLASS_TEXTBLACK);
            $("#DetailLMercLabel").addClass(C_SC3240201CLASS_TEXTBLACK);
            $("#DetailSMercList").attr("disabled", "true");
            $("#DetailLMercList").attr("disabled", "true");

            //作業時間
            SetReadOnlyDetailWorkTime();

            //予約有無・洗車有無・待ち方を変更不可に設定
            SetDetailReservationArea(false);
            SetDetailCarWashArea(false);
            SetDetailWaitingArea(false);
            //2013/12/04 TMEJ 下村 タブレット版SMB チーフテクニシャン機能開発 START
            SetDetailCompleteExaminationArea(false);
            //2013/12/04 TMEJ 下村 タブレット版SMB チーフテクニシャン機能開発 END

            //チップエリアの見た目を読取専用(文字を青→黒、チェックを青→グレー)にする
            SetReadOnlyDetailChipArea();

            //ご用命
            $("#DetailSOrderTxt").addClass(C_SC3240201CLASS_TEXTBLACK).attr("readonly", "true");
            $("#DetailLOrderTxt").addClass(C_SC3240201CLASS_TEXTBLACK).attr("readonly", "true");

            //2013/12/04 TMEJ 下村 タブレット版SMB チーフテクニシャン機能開発 START
            ////ご用命(親R/O)・故障原因(追加作業)・診断結果(追加作業)・アドバイス(親R/O)の活性／非活性制御
            //SetReadOnlyTextArea();

            //メモを非活性にする
            $("#DetailSMemoTxt").addClass(C_SC3240201CLASS_TEXTBLACK).attr("readonly", "true");
            $("#DetailLMemoTxt").addClass(C_SC3240201CLASS_TEXTBLACK).attr("readonly", "true");
            //2013/12/04 TMEJ 下村 タブレット版SMB チーフテクニシャン機能開発 END

            break;

        //納車済み
        case C_SC3240201_CP_DELI:
            //来店予定日時
            $("#DetailSPlanVisitLabel").addClass(C_SC3240201CLASS_TEXTBLACK);
            $("#DetailSPlanVisitDateTimeSelector").attr("readonly", "true").attr("disabled", "true");
            $("#DetailLPlanVisitLabel").addClass(C_SC3240201CLASS_TEXTBLACK);
            $("#DetailLPlanVisitDateTimeSelector").attr("readonly", "true").attr("disabled", "true");

            //作業開始予定日時
            $("#DetailSPlanStartLabel").addClass(C_SC3240201CLASS_TEXTBLACK);
            $("#DetailSPlanStartDateTimeSelector").attr("readonly", "true").attr("disabled", "true");
            $("#DetailLPlanStartLabel").addClass(C_SC3240201CLASS_TEXTBLACK);
            $("#DetailLPlanStartDateTimeSelector").attr("readonly", "true").attr("disabled", "true");

            //作業終了予定日時
            $("#DetailSPlanFinishLabel").addClass(C_SC3240201CLASS_TEXTBLACK);
            $("#DetailSPlanFinishDateTimeSelector").attr("readonly", "true").attr("disabled", "true");
            $("#DetailLPlanFinishLabel").addClass(C_SC3240201CLASS_TEXTBLACK);
            $("#DetailLPlanFinishDateTimeSelector").attr("readonly", "true").attr("disabled", "true");

            //納車予定日時
            $("#DetailSPlanDeriveredLabel").addClass(C_SC3240201CLASS_TEXTBLACK);
            $("#DetailSPlanDeriveredDateTimeSelector").attr("readonly", "true").attr("disabled", "true");
            $("#DetailLPlanDeriveredLabel").addClass(C_SC3240201CLASS_TEXTBLACK);
            $("#DetailLPlanDeriveredDateTimeSelector").attr("readonly", "true").attr("disabled", "true");
            
            //整備種類
            $("#DetailSMaintenanceTypeLabel").addClass(C_SC3240201CLASS_TEXTBLACK);
            $("#DetailLMaintenanceTypeLabel").addClass(C_SC3240201CLASS_TEXTBLACK);
            $("#DetailSMaintenanceTypeList").attr("disabled", "true");
            $("#DetailLMaintenanceTypeList").attr("disabled", "true");

            //整備名
            $("#DetailSMercLabel").addClass(C_SC3240201CLASS_TEXTBLACK);
            $("#DetailLMercLabel").addClass(C_SC3240201CLASS_TEXTBLACK);
            $("#DetailSMercList").attr("disabled", "true");
            $("#DetailLMercList").attr("disabled", "true");

            //作業時間
            SetReadOnlyDetailWorkTime();

            //予約有無・洗車有無・待ち方を変更不可に設定
            SetDetailReservationArea(false);
            SetDetailCarWashArea(false);
            SetDetailWaitingArea(false);
            //2013/12/04 TMEJ 下村 タブレット版SMB チーフテクニシャン機能開発 START
            SetDetailCompleteExaminationArea(false);
            //2013/12/04 TMEJ 下村 タブレット版SMB チーフテクニシャン機能開発 END

            //チップエリアの見た目を読取専用(文字を青→黒、チェックを青→グレー)にする
            SetReadOnlyDetailChipArea();

            //ご用命
            $("#DetailSOrderTxt").addClass(C_SC3240201CLASS_TEXTBLACK).attr("readonly", "true");
            $("#DetailLOrderTxt").addClass(C_SC3240201CLASS_TEXTBLACK).attr("readonly", "true");

            //2013/12/04 TMEJ 下村 タブレット版SMB チーフテクニシャン機能開発 START
            ////故障原因
            //$("#DetailSFailureTxt").addClass(C_SC3240201CLASS_TEXTBLACK).attr("readonly", "true");
            //$("#DetailLFailureTxt").addClass(C_SC3240201CLASS_TEXTBLACK).attr("readonly", "true");

            ////診断結果
            //$("#DetailSResultTxt").addClass(C_SC3240201CLASS_TEXTBLACK).attr("readonly", "true");
            //$("#DetailLResultTxt").addClass(C_SC3240201CLASS_TEXTBLACK).attr("readonly", "true");

            ////アドバイス
            //$("#DetailSAdviceTxt").addClass(C_SC3240201CLASS_TEXTBLACK).attr("readonly", "true");
            //$("#DetailLAdviceTxt").addClass(C_SC3240201CLASS_TEXTBLACK).attr("readonly", "true");

            //メモ
            $("#DetailSMemoTxt").addClass(C_SC3240201CLASS_TEXTBLACK).attr("readonly", "true");
            $("#DetailLMemoTxt").addClass(C_SC3240201CLASS_TEXTBLACK).attr("readonly", "true");
            //2013/12/04 TMEJ 下村 タブレット版SMB チーフテクニシャン機能開発 END

            break;

        //上記以外
        default:

            break;
    }
} //SetChipDetailByStatusEtc End

/**	
* チップエリアの読取専用の見た目設定を行う
* 	
*/
function SetReadOnlyDetailChipArea() {

    //チップ詳細(小)
    var detailSTableStallDl = $("#detailSTableChipUl dl");
    detailSTableStallDl.each(function () {
        //青文字を黒文字にする
        //$(this).find("dd").children(".SingleLine").removeClass(C_SC3240201CLASS_TEXTBLUE).addClass(C_SC3240201CLASS_TEXTBLACK);
        $(this).find("dd").children(".SingleLine").css("color", "#000");
    });

    //チップ詳細(大)
    var detailLChipTableDiv = $(".ChipDetailPopStyle .detailLTableChip2 #chipInfoTable").find("div");
    detailLChipTableDiv.each(function () {
        //青チェックが入っていたら黒チェックに変更
        if ($(this).hasClass(C_SC3240201CLASS_CHECKBLUE)) {
            $(this).removeClass(C_SC3240201CLASS_CHECKBLUE).addClass(C_SC3240201CLASS_CHECKBLACK);
        }
    });
}

/**	
* 作業時間の読取専用の見た目設定を行う
* 	
*/
function SetReadOnlyDetailWorkTime() {

    //作業時間
    $("#DetailSWorkTimeTxt").addClass(C_SC3240201CLASS_TEXTBLACK).attr("readonly", "true");
    $("#DetailSWorkTimeLabel").addClass(C_SC3240201CLASS_TEXTBLACK);
    $("#DetailLWorkTimeTxt").addClass(C_SC3240201CLASS_TEXTBLACK).attr("readonly", "true");
    $("#DetailLWorkTimeLabel").addClass(C_SC3240201CLASS_TEXTBLACK);

//    //作業時間の▲マーク
//    $(".detailSWorkTime dd .detailSLeftArrow span").css("background-color", "#000");
//    $(".detailSWorkTime dd .detailSLeftArrow span").css("border-color", "#000");
//    $(".detailSWorkTime dd .detailSRightArrow span").css("background-color", "#000");
//    $(".detailSWorkTime dd .detailSRightArrow span").css("border-color", "#000");
//
//    $(".detailLTableWorkTime dd .detailLLeftArrow span").css("background-color", "#000");
//    $(".detailLTableWorkTime dd .detailLLeftArrow span").css("border-color", "#000");
//    $(".detailLTableWorkTime dd .detailLRightArrow span").css("background-color", "#000");
//    $(".detailLTableWorkTime dd .detailLRightArrow span").css("border-color", "#000");

    //作業時間の▲マーク
    $(".detailSWorkTime dd .detailSLeftArrow span").css("display", "none");
    $(".detailSWorkTime dd .detailSRightArrow span").css("display", "none");

    $(".detailLTableWorkTime dd .detailLLeftArrow span").css("display", "none");
    $(".detailLTableWorkTime dd .detailLRightArrow span").css("display", "none");
}

//2013/12/04 TMEJ 下村 タブレット版SMB チーフテクニシャン機能開発 START
///**	
//* テキストエリアの読取専用の見た目設定を行う
//* （ご用命(親R/O)・故障原因(追加作業)・診断結果(追加作業)・アドバイス(親R/O)の活性／非活性制）
//* 	
//*/
//function SetReadOnlyTextArea() {

//    //作業連番
//    var roJobSeq = -1;

//    //サブチップボックスならば数値が返却される
//    var subAreaId = GetSubChipType(gSelectedChipId);

//    //受付・追加作業エリアの場合
//    if (subAreaId == C_FT_BTNTP_CONFIRMED_RO || subAreaId == C_FT_BTNTP_WAIT_CONFIRMEDADDWORK) {

//        //サブチップから作業連番を取得する
//        roJobSeq = gArrObjSubChip[gSelectedChipId].roJobSeq;
//    }
//    else {
//        //受付・追加作業エリア以外の場合

//        //DB(作業内容．作業連番)から作業連番を取得する
//        roJobSeq = $("#ChipDetailRoJobSeqHidden").val();
//    }

//    //親RO、もしくは紐付いていない場合
//    if (roJobSeq == 0 || roJobSeq == -1) {

//        //2013/12/04 TMEJ 下村 タブレット版SMB チーフテクニシャン機能開発 START
//        ////故障原因を非活性にする
//        //$("#DetailSFailureTxt").addClass(C_SC3240201CLASS_TEXTBLACK).attr("readonly", "true");
//        //$("#DetailLFailureTxt").addClass(C_SC3240201CLASS_TEXTBLACK).attr("readonly", "true");

//        ////診断結果を非活性にする
//        //$("#DetailSResultTxt").addClass(C_SC3240201CLASS_TEXTBLACK).attr("readonly", "true");
//        //$("#DetailLResultTxt").addClass(C_SC3240201CLASS_TEXTBLACK).attr("readonly", "true");

//        //メモを非活性にする
//        $("#DetailSMemoTxt").addClass(C_SC3240201CLASS_TEXTBLACK).attr("readonly", "true");
//        $("#DetailLMemoTxt").addClass(C_SC3240201CLASS_TEXTBLACK).attr("readonly", "true");
//        //2013/12/04 TMEJ 下村 タブレット版SMB チーフテクニシャン機能開発 END
//    }

////追加作業の場合、親ROの情報も一緒に表示するため、コメント化
////    //追加作業の場合
////    else if (roJobSeq >= 1) {
////
////        //ご用命を非活性にする
////        $("#DetailSOrderTxt").addClass(C_SC3240201CLASS_TEXTBLACK).attr("readonly", "true");
////        $("#DetailLOrderTxt").addClass(C_SC3240201CLASS_TEXTBLACK).attr("readonly", "true");
////
////        //アドバイスを非活性にする
////        $("#DetailSAdviceTxt").addClass(C_SC3240201CLASS_TEXTBLACK).attr("readonly", "true");
////        $("#DetailLAdviceTxt").addClass(C_SC3240201CLASS_TEXTBLACK).attr("readonly", "true");
////    }
//}
////2013/12/04 TMEJ 下村 タブレット版SMB チーフテクニシャン機能開発 END
	
/**	
* チップ詳細の入力項目値のチェックを行う
* 	
*/
function CheckChipDetailInputValue() {

    var rtnVal = 0;

    //2013/12/26 TMEJ 下村 次世代e-CRBタブレット(サービス) iOS7.0 VersionUp対応 START
    //var planVisit = $("#DetailSPlanVisitDateTimeSelector").get(0).valueAsDate;      //来店予定日時
    //var planStart = $("#DetailSPlanStartDateTimeSelector").get(0).valueAsDate;      //作業開始予定日時
    //var planEnd = $("#DetailSPlanFinishDateTimeSelector").get(0).valueAsDate;       //作業終了予定日時
    //var planDeli = $("#DetailSPlanDeriveredDateTimeSelector").get(0).valueAsDate;   //納車予定日時
	
    var planVisit = smbScript.changeStringToDateIcrop($("#DetailSPlanVisitDateTimeSelector").get(0).value);      //来店予定日時
    var planStart = smbScript.changeStringToDateIcrop($("#DetailSPlanStartDateTimeSelector").get(0).value);      //作業開始予定日時
    var planEnd = smbScript.changeStringToDateIcrop($("#DetailSPlanFinishDateTimeSelector").get(0).value);       //作業終了予定日時
    var planDeli = smbScript.changeStringToDateIcrop($("#DetailSPlanDeriveredDateTimeSelector").get(0).value);   //納車予定日時
    //2013/12/26 TMEJ 下村 次世代e-CRBタブレット(サービス) iOS7.0 VersionUp対応 END

    var procVisit = $("#DetailSProcessVisitTimeLabel").attr("datetime");            //来店実績日時

    //2013/12/26 TMEJ 下村 次世代e-CRBタブレット(サービス) iOS7.0 VersionUp対応 START
    //var procStart = $("#DetailSProcessStartDateTimeSelector").get(0).valueAsDate;   //作業開始実績日時
    //var procEnd = $("#DetailSProcessFinishDateTimeSelector").get(0).valueAsDate;    //作業終了実績日時
    var procStart = smbScript.changeStringToDateIcrop($("#DetailSProcessStartDateTimeSelector").get(0).value);   //作業開始実績日時
    var procEnd = smbScript.changeStringToDateIcrop($("#DetailSProcessFinishDateTimeSelector").get(0).value);    //作業終了実績日時
    //2013/12/26 TMEJ 下村 次世代e-CRBタブレット(サービス) iOS7.0 VersionUp対応 END

    var procDeli = $("#DetailSProcessDeriveredTimeLabel").attr("datetime");         //納車実績日時

    var procStartInit = $("#DetailSProcessStartDateTimeSelector").attr("date");     //初期表示時の作業開始実績日時

    if (procVisit == "") procVisit = null;
    if (procDeli == "") procDeli = null;

    // 2015/06/15 TMEJ 河原  サービス入庫予約情報のバリデーション実行タイミング変更に向けた評価用アプリ作成 STRAT

    //サービス・商品項目必須区分の取得
    //サービス・商品項目必須区分
    //　 0：サービス分類、商品を必須入力としない
    //　 1：サービス分類、商品を必須入力とする
    //　 2：サービス分類を必須入力とする
    var mercMandType = $("#MercMandatoryTypeHidden").val();

    // 2015/06/15 TMEJ 河原  サービス入庫予約情報のバリデーション実行タイミング変更に向けた評価用アプリ作成 END



    //2014/12/02 TMEJ 丁 DMS連携版サービスタブレットSMB来店予約時間外計画機能開発 START
//    //作業開始実績日時が読取専用でない、かつ表示用ラベルがEmptyの場合
//    if (!$("#DetailSProcessStartDateTimeSelector").attr("readonly") && !$("#DetailSProcessStartLabel").text()) {
//        rtnVal = 906;
//    }
//    //作業終了実績時間が読取専用でない、かつ表示用ラベルがEmptyの場合
//    else if (!$("#DetailSProcessFinishDateTimeSelector").attr("readonly") && !$("#DetailSProcessFinishLabel").text()) {
//        rtnVal = 907;
//    }
//    //チップの開始実績日時と終了実績日時が同じ日付でない場合
//    else if (procStart != null && procEnd != null && !smbScript.CheckSameTwoDates(procStart, procEnd)) {
//        rtnVal = 903;
//    }
//    //チップの実績日時が日を跨いで変更されている場合(日付を変更した場合)
//    //else if (procStart != null && procEnd != null && !smbScript.CheckSameTwoDates(procStart, procStartInit)) {
//    else if (procStart != null && !smbScript.CheckSameTwoDates(procStart, procStartInit)) {
//        rtnVal = 905;
//    }
//    //チップの配置時間が営業時間外の場合　※予定時間は営業時間内に補正されるため、実績用のチェック
//    else if (!smbScript.CheckChipInStallTime(gSC3240201DisplayChipStartTime, gSC3240201DisplayChipEndTime)) {
//        rtnVal = 911;
//    }
//    //予定日時の前後関係が不正な場合(開始予定日時と完了予定日時の前後だけ見る)
//    else if (!smbScript.CheckContextOfPlan(null, planStart, planEnd, null)) {
//        rtnVal = 908;
//    }
//    //実績日時の前後関係が不正な場合
//    else if (!smbScript.CheckContextOfProcess(procVisit, procStart, procEnd, procDeli)) {
//        rtnVal = 909;
//    }
//    //2013/12/04 TMEJ 下村 タブレット版SMB チーフテクニシャン機能開発 START
//    //入庫日時・納車日時必須フラグ (1:必須)
//    else if ($("#MandatoryFlgHidden").val() == "1") {
//    	if (planVisit == null) {
//        	rtnVal = 924;		//入庫予定日時が入力されていません。
//    	}
//    	else if (planDeli == null){
//    		rtnVal = 925;		//納車予定日時が入力されていません。
//    	}
//    	else{
//			if (!smbScript.CheckContextOfPlan(planVisit, planStart, planEnd, planDeli)) {
//		        rtnVal = 908;	//予定日時の大小関係が不正です。
//		    }
//    	}
//    }
//    //2013/12/04 TMEJ 下村 タブレット版SMB チーフテクニシャン機能開発 END


    //入庫日時・納車日時必須フラグ (1:必須)
    if ($("#MandatoryFlgHidden").val() == "1") {
        if (planVisit == null) {
            return 924; 	//入庫予定日時が入力されていません。
        }
        else if (planDeli == null) {
            return 925; 	//納車予定日時が入力されていません。
        }
    };

    // 2015/06/15 TMEJ 河原  サービス入庫予約情報のバリデーション実行タイミング変更に向けた評価用アプリ作成 STRAT
    //サービス・商品項目必須区分の確認
    if (mercMandType != C_SC3240201_NOCHECK && rtnVal == 0) {
        //サービス・商品項目必須区分"0"以外の場合は必須チェックが必要
        //1,2以外はスルー

        //画面から各値の取得
        var maintenanceType = $("#DetailSMaintenanceTypeList").val();  //整備種類(サービス分類)
        var mercType = $("#DetailSMercList").val();                    //整備名(商品)


        switch (mercMandType) {

            case C_SC3240201_CHECK_MAINTE_AND_MERC:
                //"1"の場合はサービス分類、商品を必須入力とする

                if (maintenanceType == "0" || maintenanceType == null) {
                    //整備種類(サービス分類)に値無し

                    rtnVal = 942; 	//整備種類が入力されていません。

                }
                else if (mercType == "0" || mercType == null) {
                    //整備名(商品)に値無し

                    ////商品データが存在しているかチェック(商品選択不可かのチェック)
                    if ($("#DetailSMercList").attr("MERCITEM") == 1) {
                        //商品が存在している場合(商品選択可能)

                        //サービス分類に紐つく商品があるが選択されていない場合
                        rtnVal = 943; 	//整備名が入力されていません。

                    }

                }
                break;

            case C_SC3240201_CHECK_MAINTE:
                //"2"の場合はサービス分類を必須入力とする

                if (maintenanceType == "0" || maintenanceType == null) {
                    //整備種類(サービス分類)に値無し

                    rtnVal = 942; 	//整備種類が入力されていません。

                }
                break;

            default:
                //"1","2"以外場合はスルー

                break;

        }
    }
    // 2015/06/15 TMEJ 河原  サービス入庫予約情報のバリデーション実行タイミング変更に向けた評価用アプリ作成 END

    //作業開始実績日時が読取専用でない、かつ表示用ラベルがEmptyの場合
    if (!$("#DetailSProcessStartDateTimeSelector").attr("readonly") && !$("#DetailSProcessStartLabel").text()) {
        return 906;
    }

    //作業終了実績時間が読取専用でない、かつ表示用ラベルがEmptyの場合
    if (!$("#DetailSProcessFinishDateTimeSelector").attr("readonly") && !$("#DetailSProcessFinishLabel").text()) {
        return 907;
    }

    //チップの開始実績日時と終了実績日時が同じ日付でない場合
    if (procStart != null && procEnd != null && !smbScript.CheckSameTwoDates(procStart, procEnd)) {
        return 903;
    }

    //チップの実績日時が日を跨いで変更されている場合(日付を変更した場合)
    if (procStart != null && !smbScript.CheckSameTwoDates(procStart, procStartInit)) {
        return 905;
    }

    //チップの配置時間が営業時間外の場合　※予定時間は営業時間内に補正されるため、実績用のチェック
    if (!smbScript.CheckChipInStallTime(gSC3240201DisplayChipStartTime, gSC3240201DisplayChipEndTime)) {
        return 911;
    }

    //作業中以降の場合
    if (procStart != null) {
        if (!smbScript.CheckContextOfProcess(procVisit, procStart, procEnd, procDeli)) {
            return 909; //実績日時の大小関係が不正です。
        }

    } else {
    //作業前の場合
        if (procVisit == null) {
        //未入庫の場合
            if (!smbScript.CheckContextOfPlan(null, planStart, planEnd, planDeli)) {
                return 908; //予定日時の大小関係が不正です。
            }

        }
        else {
        //入庫済みの場合
            if (!smbScript.CheckContextOfPlan(procVisit, planStart, planEnd, planDeli)) {
                return 939; //実績入庫日時と予定日時の大小関係が不正です。
            }
        }
    }

//2014/12/02 TMEJ 丁 DMS連携版サービスタブレットSMB来店予約時間外計画機能開発 END

//    //チップ表示日時の前後関係が不正な場合(サーバー側で自分以外のリレーションのチェックをする)
//    else {
//        if (procVisit != null) {
//            if (!smbScript.CheckContextOfProcess(procVisit, gSC3240201DisplayChipStartTime, gSC3240201DisplayChipEndTime, planDeli)) {
//                rtnVal = 904;
//            }
//        }
//        else {
//            if (!smbScript.CheckContextOfProcess(planVisit, gSC3240201DisplayChipStartTime, gSC3240201DisplayChipEndTime, planDeli)) {
//                rtnVal = 904;
//            }
//        }
//    }
    

    return rtnVal;
}

/**	
* チップ詳細を開いた時点のデータを保持する（更新時の比較用）
* 	
*/
function CreateChipDetailBeforeData() {

    //整備エリアの予約IDリスト（各整備がどの予約IDに紐付いているかのリスト(-1は紐付きなし)）
    var detailSTableStallDl = $("#detailSTableChipUl dl");
    gSC3240201Before_MatchingRezIdList = new Array();

    if (!$.isEmptyObject(detailSTableStallDl)) {
        detailSTableStallDl.each(function (i, elem) {
            if (0 < i) {
                gSC3240201Before_MatchingRezIdList.push($(this).attr("selectrezid"));
            }
        });
    }

    //飛び込みフラグ
    gSC3240201Before_RezFlg = $("#RezFlgHidden").val();           //0:予約／1:飛び込み

    //洗車フラグ
    gSC3240201Before_CarWashFlg = $("#CarWashFlgHidden").val();   //0:無し／1:有り

    //待ち方フラグ
    gSC3240201Before_WaitingFlg = $("#WaitingFlgHidden").val();   //0:店内／4:店外

    //ご用命
    gSC3240201Before_Order = $("#DetailSOrderTxt").val();

    //2013/12/04 TMEJ 下村 タブレット版SMB チーフテクニシャン機能開発 START
    ////故障原因
    //gSC3240201Before_Failure = $("#DetailSFailureTxt").val();

    ////診断結果
    //gSC3240201Before_Result = $("#DetailSResultTxt").val();

    ////アドバイス
    //gSC3240201Before_Advice = $("#DetailSAdviceTxt").val();

    //メモ
    gSC3240201Before_Memo = $("#DetailSMemoTxt").val();

    //完成検査フラグ
    gSC3240201Before_CompleteExaminationFlg = $("#CompleteExaminationFlgHidden").val();   //0:無し／1:有り
    //2013/12/04 TMEJ 下村 タブレット版SMB チーフテクニシャン機能開発 END
}

/**	
* 整備コード、整備連番、作業連番、枝番、予約ID(未選択(未紐付き)行は-1)のリストを作成する
* 	
*/
function CreateMainteInfoList() {

    var detailSTableStallDl = $("#detailSTableChipUl dl");
    gSC3240201FixItemCodeList = new Array();
	//2013/12/04 TMEJ 下村 タブレット版SMB チーフテクニシャン機能開発 START
    //gSC3240201FixItemSeqList = new Array();
    gSC3240201JobinstrucDtlIdList = new Array();
	gSC3240201JobInstructSeqList = new Array();
	gSC3240201JobInstructIdList = new Array();
	//2013/12/04 TMEJ 下村 タブレット版SMB チーフテクニシャン機能開発 END
    gSC3240201RoJobSeqList = new Array();
	//2013/12/04 TMEJ 下村 タブレット版SMB チーフテクニシャン機能開発 START
    //gSC3240201SrvAddSeqList = new Array();
	//2013/12/04 TMEJ 下村 タブレット版SMB チーフテクニシャン機能開発 END
    gSC3240201MatchingRezIdList = new Array();

    if (!$.isEmptyObject(detailSTableStallDl)) {
        detailSTableStallDl.each(function (i, elem) {
            if (0 < i) {
                gSC3240201FixItemCodeList.push($(this).attr("fixitemcode"));
            	//2013/12/04 TMEJ 下村 タブレット版SMB チーフテクニシャン機能開発 START
                //gSC3240201FixItemSeqList.push($(this).attr("fixitemseq"));
				gSC3240201JobinstrucDtlIdList.push($(this).attr("jobinstrucdtlid"));
				gSC3240201JobInstructSeqList.push($(this).attr("jobinstructseq"));
				gSC3240201JobInstructIdList.push($(this).attr("jobinstructid"));
            	//2013/12/04 TMEJ 下村 タブレット版SMB チーフテクニシャン機能開発 END
                gSC3240201RoJobSeqList.push($(this).attr("rojobseq"));
            	//2013/12/04 TMEJ 下村 タブレット版SMB チーフテクニシャン機能開発 START
                //gSC3240201SrvAddSeqList.push($(this).attr("srvaddseq"));
            	//2013/12/04 TMEJ 下村 タブレット版SMB チーフテクニシャン機能開発 START
                gSC3240201MatchingRezIdList.push($(this).attr("selectrezid"));
            }
        });
    }
}

/**	
* チップ詳細のチップエリアに表示した予約IDのリストを作成する
* 	
*/
function CreateRezIdList() {

    var rezIdListDiv = $("#DetailSMaintenanceDl").find("div");

    gSC3240201RezIdList = new Array();
    gSC3240201RezId_StallUseStatusList = new Array();
	//2013/12/04 TMEJ 下村 タブレット版SMB チーフテクニシャン機能開発 START
    //gSC3240201RoJobSeq2List = new Array();
	gSC3240201RezId_InvisibleInstructFlgList = new Array();
	//2013/12/04 TMEJ 下村 タブレット版SMB チーフテクニシャン機能開発 END

    if (!$.isEmptyObject(rezIdListDiv)) {

        //チップ詳細に表示したチップ分ループ
        rezIdListDiv.each(function (i, elem) {
            
            //属性から予約IDを取得
            var elemRezId = $(elem).attr("rezid");

            //属性から予約のストール利用ステータスを取得
            var elemRezId_stallUseStatus = $(elem).attr("stallusestatus");

            //属性から予約のRO作業連番を取得
        	//2013/12/04 TMEJ 下村 タブレット版SMB チーフテクニシャン機能開発 START
            //var elemRoJobSeq2 = $(elem).attr("rojobseq2");
            var elemInvisibleInstructFlg = $(elem).attr("InvisibleInstructFlg");
        	//2013/12/04 TMEJ 下村 タブレット版SMB チーフテクニシャン機能開発 END

            //「未選択」のチップは除く
            if (elemRezId != "-1") {
                gSC3240201RezIdList.push(elemRezId);
                gSC3240201RezId_StallUseStatusList.push(elemRezId_stallUseStatus);
            	//2013/12/04 TMEJ 下村 タブレット版SMB チーフテクニシャン機能開発 START
                //gSC3240201RezId_InvisibleInstructFlgList.push(elemRoJobSeq2);
                gSC3240201RezId_InvisibleInstructFlgList.push(elemInvisibleInstructFlg);
            	//2013/12/04 TMEJ 下村 タブレット版SMB チーフテクニシャン機能開発 END
            }
        });
    }
}

/**	
* null値の場合、-1に変換する
* 	
*/
function ConvertNullToMinusNum(src) {

    var rtnVal = -1;

    if (src != null) {
        rtnVal = src;
    }

    return rtnVal;
}

/**	
* 表示用開始時間を取得する
* 	
*/
function GetDisplayStartTime() {

    var rtnVal;

    //2013/12/26 TMEJ 下村 次世代e-CRBタブレット(サービス) iOS7.0 VersionUp対応 START
    //var planStart = $("#DetailSPlanStartDateTimeSelector").get(0).valueAsDate;        //作業開始予定日時
    //var procStart = $("#DetailSProcessStartDateTimeSelector").get(0).valueAsDate;     //作業開始実績日時    
    var planStart = smbScript.changeStringToDateIcrop($("#DetailSPlanStartDateTimeSelector").get(0).value);        //作業開始予定日時
    var procStart = smbScript.changeStringToDateIcrop($("#DetailSProcessStartDateTimeSelector").get(0).value);     //作業開始実績日時
    //2013/12/26 TMEJ 下村 次世代e-CRBタブレット(サービス) iOS7.0 VersionUp対応 END

    if (procStart != null) {
        //作業開始実績時間
        rtnVal = smbScript.ConvertDateToString(procStart);
    }
    else {
        //作業開始予定時間
        rtnVal = smbScript.ConvertDateToString(planStart);
    }

    return rtnVal;
}

/**	
* 表示用終了時間を取得する
* 	
*/
function GetDisplayEndTime() {

    var rtnVal;

    //2013/12/26 TMEJ 下村 次世代e-CRBタブレット(サービス) iOS7.0 VersionUp対応 START
    //var planEnd = $("#DetailSPlanFinishDateTimeSelector").get(0).valueAsDate;         //作業終了予定時間
    //var procStart = $("#DetailSProcessStartDateTimeSelector").get(0).valueAsDate;     //作業開始実績時間
    //var procEnd = $("#DetailSProcessFinishDateTimeSelector").get(0).valueAsDate;      //作業終了実績時間        
    var planEnd = smbScript.changeStringToDateIcrop($("#DetailSPlanFinishDateTimeSelector").get(0).value);         //作業終了予定時間
    var procStart = smbScript.changeStringToDateIcrop($("#DetailSProcessStartDateTimeSelector").get(0).value);     //作業開始実績時間
    var procEnd = smbScript.changeStringToDateIcrop($("#DetailSProcessFinishDateTimeSelector").get(0).value);      //作業終了実績時間
    //2013/12/26 TMEJ 下村 次世代e-CRBタブレット(サービス) iOS7.0 VersionUp対応 END

    var workTime = $("#DetailSWorkTimeTxt").val();                                    //予定作業時間
    //var workTime = $("#DetailSWorkTimeLabel").text();                                    //予定作業時間

    if (procEnd) {
        //作業終了実績時間
        rtnVal = smbScript.ConvertDateToString(procEnd);
    }
    else if (procStart) {
        //作業開始実績時間 + 予定作業時間
        rtnVal = smbScript.ConvertDateToString(smbScript.CalcEndDateTime(procStart, workTime));
    }
    else {
        //作業終了予定時間
        rtnVal = smbScript.ConvertDateToString(planEnd);
    }

    return rtnVal;
}

/**	
* DB登録用に渡す実績作業時間を計算する
* 	
*/
function CalcSC3240201ProcWorkTime() {
    
    var rtnVal = 0;

    //2013/12/26 TMEJ 下村 次世代e-CRBタブレット(サービス) iOS7.0 VersionUp対応 START
    //var procStart = $("#DetailSProcessStartDateTimeSelector").get(0).valueAsDate;     //作業開始実績時間
    //var procEnd = $("#DetailSProcessFinishDateTimeSelector").get(0).valueAsDate;      //作業終了実績時間    
    var procStart = smbScript.changeStringToDateIcrop($("#DetailSProcessStartDateTimeSelector").get(0).value);     //作業開始実績時間
    var procEnd = smbScript.changeStringToDateIcrop($("#DetailSProcessFinishDateTimeSelector").get(0).value);      //作業終了実績時間
    //2013/12/26 TMEJ 下村 次世代e-CRBタブレット(サービス) iOS7.0 VersionUp対応 END

    var workTime = $("#DetailSWorkTimeTxt").val();                                    //予定作業時間
    //var workTime = $("#DetailSWorkTimeLabel").text();                                    //予定作業時間

    //作業終了実績時間がある場合
    if (procStart && procEnd) {
        rtnVal = smbScript.CalcTimeSpan(procStart, procEnd);
    }

    return rtnVal;
}

/**	
* DB登録用に渡す見込終了日時を計算する
* 	
*/
function CalcSC3240201PrmsEndTime() {

    //var rtnVal = Date.parse("1900/01/01 0:00:00");
    var rtnVal = "";
    var wkVal = "";

    //2013/12/26 TMEJ 下村 次世代e-CRBタブレット(サービス) iOS7.0 VersionUp対応 START
    //var procStart = $("#DetailSProcessStartDateTimeSelector").get(0).valueAsDate;     //作業開始実績時間
    var procStart = smbScript.changeStringToDateIcrop($("#DetailSProcessStartDateTimeSelector").get(0).value);     //作業開始実績時間
    //2013/12/26 TMEJ 下村 次世代e-CRBタブレット(サービス) iOS7.0 VersionUp対応 END

    var workTime = $("#DetailSWorkTimeTxt").val();                                    //予定作業時間

    //ストール利用ステータス=02:作業中の場合
    if ($("#ChipDetailStallUseStatusHidden").val() == "02") {
        if (procStart && workTime) {
            //開始実績日時＋作業予定時間の日時を求める
            wkVal = smbScript.CalcEndDate_ExcludeOutOfTime(procStart, $("#DetailSWorkTimeTxt").val());
            rtnVal = smbScript.ConvertDateToString2(wkVal);
        }
    }

    return rtnVal;
}

/**	
* DB登録用に渡すチップ表示開始日時を取得する
* 	
*/
function GetSC3240201ChipDispStartDate() {
    
    var rtnVal = 0;

    //2013/12/26 TMEJ 下村 次世代e-CRBタブレット(サービス) iOS7.0 VersionUp対応 START
    //var planStart = $("#DetailSPlanStartDateTimeSelector").get(0).valueAsDate;        //作業開始予定日時
    //var procStart = $("#DetailSProcessStartDateTimeSelector").get(0).valueAsDate;     //作業開始実績日時
    var planStart = smbScript.changeStringToDateIcrop($("#DetailSPlanStartDateTimeSelector").get(0).value);        //作業開始予定日時
    var procStart = smbScript.changeStringToDateIcrop($("#DetailSProcessStartDateTimeSelector").get(0).value);     //作業開始実績日時
    //2013/12/26 TMEJ 下村 次世代e-CRBタブレット(サービス) iOS7.0 VersionUp対応 END

    //作業開始実績日時がある場合、作業開始実績日時を返却する
    if (IsMinDate(procStart) == false) {
        rtnVal = procStart;
    } else {
        //作業開始実績日時がない場合、作業開始予定日時を返却する
        rtnVal = planStart;
    }

    return rtnVal;
}

/**	
* DB登録用に渡すRO情報変更フラグをセットする（0:変更なし／1:変更あり）
* 	
*/
function SetSC3240201ROInfo() {
    
    var rtnVal = "0";

    if ((gSC3240201Before_RezFlg != $("#RezFlgHidden").val()) ||
       (gSC3240201Before_CarWashFlg != $("#CarWashFlgHidden").val()) ||
       (gSC3240201Before_WaitingFlg != $("#WaitingFlgHidden").val()) ||
       (gSC3240201Before_Order != $("#DetailSOrderTxt").val()) ||
       //2013/12/04 TMEJ 下村 タブレット版SMB チーフテクニシャン機能開発 START
       //(gSC3240201Before_Failure != $("#DetailSFailureTxt").val()) ||
       //(gSC3240201Before_Result != $("#DetailSResultTxt").val()) ||
       //(gSC3240201Before_Advice != $("#DetailSAdviceTxt").val())) {
       (gSC3240201Before_CompleteExaminationFlg != $("#CompleteExaminationFlgHidden").val()) ||
       (gSC3240201Before_Memo != $("#DetailSMemoTxt").val())) {
       //2013/12/04 TMEJ 下村 タブレット版SMB チーフテクニシャン機能開発 END
        rtnVal = "1";
    }

    return rtnVal;
}

//2013/12/04 TMEJ 下村 タブレット版SMB チーフテクニシャン機能開発 START
/**	
* Vinを取得する
* 	
*/
function getSC3240201VinName() {
    
    var rtnVal = "";
	
	if ($("#Visit_VINHidden").val() != "") {
        rtnVal = $("#Visit_VINHidden").val();
    } else {
		rtnVal = $("#ChipDetail_VinHidden").val();
	}

    return rtnVal;
}
//2013/12/04 TMEJ 下村 タブレット版SMB チーフテクニシャン機能開発 END
	
/**	
* チップ詳細画面で指定した日時の営業開始時間(date)を取得する
* 	
*/
function CalcInputStallStartTime() {
    
    var rtnVal = 0;

    //2013/12/26 TMEJ 下村 次世代e-CRBタブレット(サービス) iOS7.0 VersionUp対応 START
    //var planStart = $("#DetailSPlanStartDateTimeSelector").get(0).valueAsDate;        //作業開始予定日時
    //var procStart = $("#DetailSProcessStartDateTimeSelector").get(0).valueAsDate;     //作業開始実績日時
    var planStart = smbScript.changeStringToDateIcrop($("#DetailSPlanStartDateTimeSelector").get(0).value);        //作業開始予定日時
    var procStart = smbScript.changeStringToDateIcrop($("#DetailSProcessStartDateTimeSelector").get(0).value);     //作業開始実績日時
    //2013/12/26 TMEJ 下村 次世代e-CRBタブレット(サービス) iOS7.0 VersionUp対応 END

    var wkStart;

    //作業開始実績日時がある場合、「作業開始実績日 営業開始時刻」を返却する
    if (IsMinDate(procStart) == false) {
        //2013/12/26 TMEJ 下村 次世代e-CRBタブレット(サービス) iOS7.0 VersionUp対応 START
        //wkStart = new Date(procStart);
        wkStart = procStart;
        //2013/12/26 TMEJ 下村 次世代e-CRBタブレット(サービス) iOS7.0 VersionUp対応 END

    } else {
        //作業開始実績日時がない場合、「作業開始予定日 営業開始時刻」を返却する
        //2013/12/26 TMEJ 下村 次世代e-CRBタブレット(サービス) iOS7.0 VersionUp対応 START
        //wkStart = new Date(planStart);
        wkStart = planStart;
        //2013/12/26 TMEJ 下村 次世代e-CRBタブレット(サービス) iOS7.0 VersionUp対応 END
    }
    //2013/12/26 TMEJ 下村 次世代e-CRBタブレット(サービス) iOS7.0 VersionUp対応 START
    //rtnVal = new Date(wkStart.getFullYear() + "/" + (wkStart.getMonth() + 1) + "/" + wkStart.getDate() + " " + $("#hidStallStartTime").val() + ":00");
    if (wkStart != null) {
    	rtnVal = new Date(wkStart.getFullYear() + "/" + (wkStart.getMonth() + 1) + "/" + wkStart.getDate() + " " + $("#hidStallStartTime").val() + ":00");
	} else {
		rtnVal = new Date()
	}
    //2013/12/26 TMEJ 下村 次世代e-CRBタブレット(サービス) iOS7.0 VersionUp対応 END

    return rtnVal;
}

/**	
* チップ詳細画面で指定した日時の営業終了時間(date)を取得する
* 	
*/
function CalcInputStallEndTime() {
    
    var rtnVal = 0;

    //2013/12/26 TMEJ 下村 次世代e-CRBタブレット(サービス) iOS7.0 VersionUp対応 START
    //var planEnd = $("#DetailSPlanFinishDateTimeSelector").get(0).valueAsDate;        //作業終了予定日時
    //var procEnd = $("#DetailSProcessFinishDateTimeSelector").get(0).valueAsDate;     //作業終了実績日時
    var planEnd = smbScript.changeStringToDateIcrop($("#DetailSPlanFinishDateTimeSelector").get(0).value);        //作業終了予定日時
    var procEnd = smbScript.changeStringToDateIcrop($("#DetailSProcessFinishDateTimeSelector").get(0).value);     //作業終了実績日時
    //2013/12/26 TMEJ 下村 次世代e-CRBタブレット(サービス) iOS7.0 VersionUp対応 END

    var wkEnd;

    //作業終了実績日時がある場合、「作業終了実績日 営業終了時刻」を返却する
    if (IsMinDate(procEnd) == false) {
        //2013/12/26 TMEJ 下村 次世代e-CRBタブレット(サービス) iOS7.0 VersionUp対応 START
        //wkEnd = new Date(procEnd);
        wkEnd = procEnd;
        //2013/12/26 TMEJ 下村 次世代e-CRBタブレット(サービス) iOS7.0 VersionUp対応 END

    } else {
        //作業終了実績日時がない場合、「作業終了予定日 営業終了時刻」を返却する
        //2013/12/26 TMEJ 下村 次世代e-CRBタブレット(サービス) iOS7.0 VersionUp対応 START
        //wkEnd = new Date(planEnd);
        wkEnd = planEnd;
        //2013/12/26 TMEJ 下村 次世代e-CRBタブレット(サービス) iOS7.0 VersionUp対応 END
    }
    //2013/12/26 TMEJ 下村 次世代e-CRBタブレット(サービス) iOS7.0 VersionUp対応 START
    //rtnVal = new Date(wkEnd.getFullYear() + "/" + (wkEnd.getMonth() + 1) + "/" + wkEnd.getDate() + " " + $("#hidStallEndTime").val() + ":00");
    if (wkEnd != null) {
    	rtnVal = new Date(wkEnd.getFullYear() + "/" + (wkEnd.getMonth() + 1) + "/" + wkEnd.getDate() + " " + $("#hidStallEndTime").val() + ":00");
	} else {
		rtnVal = new Date()
	}
    //2013/12/26 TMEJ 下村 次世代e-CRBタブレット(サービス) iOS7.0 VersionUp対応 END

    return rtnVal;
}

/**
* ポップアップを表示する方向を計算し、設定値を決定する
*
* @param {$(div)} baseCtrl
*
*/
function CalcDetailSPopoverPosition(baseCtrl) {

    var possibleDir = {
        left: false,
        right: false
    }

    var ctrlPosition = {
        popX: 0,    //ポップアップのleft値
        arrowX: 0,  //吹出し三角のleft値
        arrowY: 0   //吹出し三角のtop値
    }

    //ポップアップをチップの左に表示する場合のleft値
    var popDispLeftX = baseCtrl.offset().left - (($("#ChipDetailArrowMask").width() / 2) - 3) - $("#ChipDetailPopupContent").width();

    //ポップアップをチップの右に表示する場合のleft値
    var popDispRightX = baseCtrl.offset().left + baseCtrl.width() + (($("#ChipDetailArrowMask").width() / 2) - 3);

    //チップのleft + チップのwidth + (吹出し三角のwidth / 2 - 3(微調整値)) + ポップアップのwidth <= 画面全体のwidth
    if (popDispRightX + $("#ChipDetailPopupContent").width() <= $(document.body).width()) {
        possibleDir.right = true;
    }
    //0 <= チップのleft - (吹出し三角のwidth / 2 - 3(微調整値)) - ポップアップのwidth
    else if (0 <= popDispLeftX) {
        possibleDir.left = true;
    }

    if (possibleDir.right) {
        ctrlPosition.popX = popDispRightX;
        ctrlPosition.arrowX = C_SC3240201POP_DISPRIGHT_ARROW_X;
    }
    else if (possibleDir.left) {
        ctrlPosition.popX = popDispLeftX;
        ctrlPosition.arrowX = C_SC3240201POP_DISPLEFT_ARROW_X;
    }
    else {  //チップが長すぎてどちらも適切でない場合
        ctrlPosition.popX = C_SC3240201POP_DISPRIGHT_DEFAULT_X;
        ctrlPosition.arrowX = C_SC3240201POP_DISPRIGHT_ARROW_X;
    }

    //チップのtop + (チップのheight / 2) - 79(調整値)
    ctrlPosition.arrowY = baseCtrl.offset().top + (baseCtrl.height() / 2) - 79;

    return ctrlPosition;
} //CalcDetailSPopoverPosition End

/**
* ポップアップを表示する位置を設定する
*
* @param {class} ctrlPosition
*                   popX:   ポップアップのleft値
*                   arrowX: 吹出しのleft値
*                   arrowY: 吹出しのtop値
*
*/
function SetDetailSPopoverPosition(ctrlPosition) {
    $("#ChipDetailPopupContent").css("left", ctrlPosition.popX);
    $("#ChipDetailArrowMask").css({ "left": ctrlPosition.arrowX, "top": ctrlPosition.arrowY });
    //チップ詳細(小)表示時のleft値を保持しておく
    gDetailSPopX = ctrlPosition.popX;
}

/**	
* 画面を再表示する(commonRefreshTimerにセットする関数)
* 	
*/
function ReDisplayChipDetail() {
    CloseChipDetail(0);
    ClearOperationList();
    setTimeout(function () {
        FooterEvent(C_FT_BTNID_DETAIL);
    }, 200);
}

/**	
* サブチップボックスを非表示にする
* 	
*/
function HiddenSubChipBox() {
    $(".SubChipReception").fadeOut(300);
    $(".SubChipAdditionalWork").fadeOut(300);
    $(".SubChipCompletionInspection").fadeOut(300);
    $(".SubChipCarWash").fadeOut(300);
    $(".SubChipWaitingDelivered").fadeOut(300);
    $(".SubChipNoShow").fadeOut(300);
    $(".SubChipStop").fadeOut(300);
}

/**
* チップ詳細(小)　作業時間の左▲クリック時
* 
*/
function DetailSWorkTimeLeft() {

//    // ボタンを青色にする
//    $("#DetailSWorkTimeLeftArrow").addClass("icrop-pressed");
//
//    setTimeout(function () {
//        // ボタンの青色を解除
//        $("#DetailSWorkTimeLeftArrow").removeClass("icrop-pressed");
//    }, 300);

    //作業時間を設定
    SetDetailWorkTime(-gResizeInterval);

    return false;
}

/**
* チップ詳細(小)　作業時間の右▲クリック時
* 
*/
function DetailSWorkTimeRight() {

//    // ボタンを青色にする
//    $("#DetailSWorkTimeRightArrow").addClass("icrop-pressed");
//
//    setTimeout(function () {
//        // ボタンの青色を解除
//        $("#DetailSWorkTimeRightArrow").removeClass("icrop-pressed");
//    }, 300);

    //作業時間を設定
    SetDetailWorkTime(gResizeInterval);

    return false;
}

/**
* チップ詳細(大)　作業時間の左▲クリック時
* 
*/
function DetailLWorkTimeLeft() {

//    // ボタンを青色にする
//    $("#DetailLWorkTimeLeftArrow").addClass("icrop-pressed");
//
//    setTimeout(function () {
//        // ボタンの青色を解除
//        $("#DetailLWorkTimeLeftArrow").removeClass("icrop-pressed");
//    }, 300);

    //作業時間を設定
    SetDetailWorkTime(-gResizeInterval);

    return false;
}

/**
* チップ詳細(大)　作業時間の右▲クリック時
* 
*/
function DetailLWorkTimeRight() {

//    // ボタンを青色にする
//    $("#DetailLWorkTimeRightArrow").addClass("icrop-pressed");
//
//    setTimeout(function () {
//        // ボタンの青色を解除
//        $("#DetailLWorkTimeRightArrow").removeClass("icrop-pressed");
//    }, 300);

    //作業時間を設定
    SetDetailWorkTime(gResizeInterval);

    return false;
}

/**
* 作業時間の設定
* 
*/
function SetDetailWorkTime(num) {

    //編集不可の場合は何もしない
    if ($("#DetailSWorkTimeTxt").hasClass(C_SC3240201CLASS_TEXTBLACK) == true) {
        return false;
    }

    //現在の作業時間の値を取得
    var min = parseInt($("#DetailSWorkTimeTxt").val(), C_RADIX);

    //現在の作業時間の値が5分以下、且つ、左▲により作業時間をマイナスさせようとした場合、
    //  もしくは、現在の作業時間の値が9995分以上、且つ、右▲により作業時間をプラスさせようとした場合
    if ((min <= gResizeInterval) && (num < 0) || (min >= C_SC3240201_MAXWORKTIME) && (num > 0)) {
        //何もせず終了
        return false;
    }

    //パラメータにより、＋－させる
    min = min + num;

    $("#DetailSWorkTimeTxt").val(min);
    $("#DetailLWorkTimeTxt").val(min);
    $("#DetailSWorkTimeLabel").text(min + $("#WordWorkTimeUnitHidden").val());
    $("#DetailLWorkTimeLabel").text(min + $("#WordWorkTimeUnitHidden").val());

    //作業開始日時がnullでない場合、設定した作業時間に合わせて作業終了予定時間を変更する
    //2013/12/26 TMEJ 下村 次世代e-CRBタブレット(サービス) iOS7.0 VersionUp対応 START
    //var startDateTime = $("#DetailSPlanStartDateTimeSelector").get(0).valueAsDate;
    var startDateTime = smbScript.changeStringToDateIcrop($("#DetailSPlanStartDateTimeSelector").get(0).value);
    //2013/12/26 TMEJ 下村 次世代e-CRBタブレット(サービス) iOS7.0 VersionUp対応 END

    //if (startDateTime != null) {
    var startLabel = $("#DetailSPlanStartLabel").text();

    //2014/04/01 TMEJ 丁 次世代e-CRBタブレット(サービス) チーフテクニシャン機能開発 START
    //if (startLabel != ""){
    if ((startLabel != "") && (!$("#DetailSPlanFinishDateTimeSelector")[0].disabled)) {
    //2014/04/01 TMEJ 丁 次世代e-CRBタブレット(サービス) チーフテクニシャン機能開発 START
        //var newEndDateTime = smbScript.CalcEndDateTime(startDateTime, min);
        //開始日時と加算時間を元に、終了日時を計算する（営業時間外の時間帯は除外して計算）
        var newEndDateTime = smbScript.CalcEndDate_ExcludeOutOfTime(startDateTime, min);

        //2013/12/26 TMEJ 下村 次世代e-CRBタブレット(サービス) iOS7.0 VersionUp対応 START
        //$("#DetailSPlanFinishDateTimeSelector").get(0).valueAsDate = newEndDateTime;
        $("#DetailSPlanFinishDateTimeSelector").get(0).value = smbScript.getDateTimelocalDate(newEndDateTime);
        //2013/12/26 TMEJ 下村 次世代e-CRBタブレット(サービス) iOS7.0 VersionUp対応 END
        $("#DetailSPlanFinishLabel").text(smbScript.ConvertDateOrTime(newEndDateTime, $("#hidShowDate").val()));

        //チップ詳細(大)に反映
        //2013/12/26 TMEJ 下村 次世代e-CRBタブレット(サービス) iOS7.0 VersionUp対応 START
        //$("#DetailLPlanFinishDateTimeSelector").get(0).valueAsDate = newEndDateTime;
        $("#DetailLPlanFinishDateTimeSelector").get(0).value = smbScript.getDateTimelocalDate(newEndDateTime);
        //2013/12/26 TMEJ 下村 次世代e-CRBタブレット(サービス) iOS7.0 VersionUp対応 END
        $("#DetailLPlanFinishLabel").text($("#DetailSPlanFinishLabel").text());

        //必須項目がEmptyなら登録ボタンを非活性にする
        $("#DetailRegisterBtn").attr("disabled", IsMandatoryChipDetailEmpty());
    }

    return false;
}

/**
* チップ詳細(小)　顧客詳細画面へ遷移するためのダミーボタンClick処理
* 
*/
function DetailSCustButton() {

    // ボタンを青色にする
    $("#DetailSCustBtnDiv").addClass("icrop-pressed");

    //2013/12/04 TMEJ 下村 タブレット版SMB チーフテクニシャン機能開発 START
    //setTimeout(function () {
    //    //アクティブインジケータ表示
    //    gDetailSActiveIndicator.show();
    //
    //    //オーバーレイ表示
    //    gDetailOverlay.show();
    //
    //    // ボタンの青色を解除
    //    $("#DetailSCustBtnDiv").removeClass("icrop-pressed");
    //}, 300);
	//
    ////リフレッシュタイマーセット
    //commonRefreshTimer(ReDisplayChipDetail);
    //
    //$('#DetailCustButtonDummy').click();

    if ($("#DmsCstCdHidden").val() == "") {
		//未取引客の場合
	    icropScript.ShowMessageBox(920, $("#DetailCstBtnErrMsgHidden").val(), "");	//新規顧客登録をしてください
        // ボタンの青色を解除
        $("#DetailSCustBtnDiv").removeClass("icrop-pressed");
	} else {
	    setTimeout(function () {
	        //アクティブインジケータ表示
	        gDetailSActiveIndicator.show();

	        //オーバーレイ表示
	        gDetailOverlay.show();

	        // ボタンの青色を解除
	        $("#DetailSCustBtnDiv").removeClass("icrop-pressed");
	    }, 300);

	    //リフレッシュタイマーセット
	    commonRefreshTimer(ReDisplayChipDetail);

	    $('#DetailCustButtonDummy').click();
	}
	//2013/12/04 TMEJ 下村 タブレット版SMB チーフテクニシャン機能開発 END

    return false;
}

/**
* チップ詳細(小)　R/O参照画面へ遷移するためのダミーボタンClick処理
* 
*/
function DetailSROButton() {

    // ボタンを青色にする
    $("#DetailSRORefBtnDiv").addClass("icrop-pressed");

    setTimeout(function () {
        //アクティブインジケータ表示
        gDetailSActiveIndicator.show();

        //オーバーレイ表示
        gDetailOverlay.show();

        // ボタンの青色を解除
        $("#DetailSRORefBtnDiv").removeClass("icrop-pressed");
    }, 300);

    //リフレッシュタイマーセット
    commonRefreshTimer(ReDisplayChipDetail);

    $('#DetailROButtonDummy').click();

    return false;
}


/**
* チップ詳細(大)　顧客詳細画面へ遷移するためのダミーボタンClick処理
* 
*/
function DetailLCustButton() {

    // ボタンを青色にする
    $("#DetailLCustBtnDiv").addClass("icrop-pressed");

    //2013/12/04 TMEJ 下村 タブレット版SMB チーフテクニシャン機能開発 START
    //setTimeout(function () {
    //    //アクティブインジケータ表示
    //    gDetailSActiveIndicator.show();
    //
    //    //オーバーレイ表示
    //    gDetailOverlay.show();
    //
    //    // ボタンの青色を解除
    //    $("#DetailLCustBtnDiv").removeClass("icrop-pressed");
    //}, 300);
    //
    ////リフレッシュタイマーセット
    //commonRefreshTimer(ReDisplayChipDetail);
    //
    //$('#DetailCustButtonDummy').click();

    if ($("#DmsCstCdHidden").val() == "") {
		//未取引客の場合
	    icropScript.ShowMessageBox(920, $("#DetailCstBtnErrMsgHidden").val(), "");	//新規顧客登録をしてください
        // ボタンの青色を解除
        $("#DetailLCustBtnDiv").removeClass("icrop-pressed");
	} else {
	    setTimeout(function () {
	        //アクティブインジケータ表示
	        gDetailSActiveIndicator.show();

	        //オーバーレイ表示
	        gDetailOverlay.show();

	        // ボタンの青色を解除
	        $("#DetailLCustBtnDiv").removeClass("icrop-pressed");
	    }, 300);

	    //リフレッシュタイマーセット
	    commonRefreshTimer(ReDisplayChipDetail);

	    $('#DetailCustButtonDummy').click();
	}
	//2013/12/04 TMEJ 下村 タブレット版SMB チーフテクニシャン機能開発 END

    return false;
}

/**
* チップ詳細(大)　R/O参照画面へ遷移するためのダミーボタンClick処理
* 
*/
function DetailLROButton() {

    // ボタンを青色にする
    $("#DetailLRORefBtnDiv").addClass("icrop-pressed");

    setTimeout(function () {
        //アクティブインジケータ表示
        gDetailSActiveIndicator.show();

        //オーバーレイ表示
        gDetailOverlay.show();

        // ボタンの青色を解除
        $("#DetailLRORefBtnDiv").removeClass("icrop-pressed");
    }, 300);

    //リフレッシュタイマーセット
    commonRefreshTimer(ReDisplayChipDetail);

    $('#DetailROButtonDummy').click();

    return false;
}

/**
* 最小日付をチェック
* @param {String} dtDate チェック日付
* @return {Bool} true：最小日付
*/
function IsMinDate(dtDate) {
    var dtMin = new Date(C_SC3240201_DATE_MIN_VALUE);

    if (dtDate == null || dtDate == "") {
        return true;
    } else if ((dtDate - dtMin) == 0) {
        return true;
    } else {
        return false;
    }
}


//2014/07/15 TMEJ 丁 タブレットSMB Job Dispatch機能開発 START
/**
* チップ詳細開始
* @param {String} nType 開始類別:AllStart、SingleStart
* @return {-} 無し
*/
function ClickBtnStartByDetail(nType) {
    //開始する日付は当日ではない時は開始できない
    if (IsTodayPage() == false) {
        alert(htmlDecode($("#JobStartDtlErrMsgHidden").val()));
        //アクティブインジケータ・オーバーレイ非表示
        gDetailSActiveIndicator.hide();
        gDetailOverlay.hide();
        return false;
    }

    //アクティブインジケータ表示
    gDetailSActiveIndicator.show();
    //オーバーレイ表示
    gDetailOverlay.show();

    //リフレッシュタイマーセット
    commonRefreshTimer(ReDisplayChipDetail);

    var strChipId = gSelectedChipId;

    var bStartChipFlg = false;
    //既に開始中の場合はチェックしない
    if (gArrObjChip[strChipId].svcStatus != "05") {
        // ストールに既に開始チップがあるかどうかをチック
        $("#" + strChipId).offsetParent().children("div").each(function (index, e) {
            if (gArrObjChip[e.id]) {
                // 作業中
                if ((IsDefaultDate(gArrObjChip[e.id].rsltStartDateTime) == false) && (IsDefaultDate(gArrObjChip[e.id].rsltEndDateTime) == true)) {
                    bStartChipFlg = true;
                }
            }
        });
    }
    // 同じストールに、二つの作業を開始はできない
    if (bStartChipFlg) {
        ShowSC3240101Msg(905); //後で詳細の文言を書き換える
        //アクティブインジケータ・オーバーレイ非表示
        gDetailSActiveIndicator.hide();
        gDetailOverlay.hide();
        //タイマーをクリア
        commonClearTimer();
        return;
    }

    // 今の時間を取得して、プロトタイプに設定する
    var dtStartNow = GetServerTimeNow();
    dtStartNow.setSeconds(0);
    dtStartNow.setMilliseconds(0);

    // 整備種類が選択されていないため、作業開始できない。
    if ($("#DetailLMaintenanceTypeLabel")[0].textContent.toString().Trim() == "") {
        ShowSC3240101Msg(904); //後で詳細の文言を書き換える
        //アクティブインジケータ・オーバーレイ非表示
        gDetailSActiveIndicator.hide();
        gDetailOverlay.hide();
        //タイマーをクリア
        commonClearTimer();
        return;
    }

    StartByChipDetail(null, null, nType);
}

/**
* チップ詳細終了
* @param {String} nType 終了類別:AllFinish、SingleFinish
* @param {String} nJobInstructId 作業指示ID
* @param {String} nJobInstructSeq 作業指示SEQ
* @return {-} 無し
*/
function ClickBtnFinishByDetail(nType, nJobInstructId, nJobInstructSeq) {

    //アクティブインジケータ表示
    gDetailSActiveIndicator.show();
    //オーバーレイ表示
    gDetailOverlay.show();

    //リフレッシュタイマーセット
    commonRefreshTimer(ReDisplayChipDetail);

    // 終了
    FinishByChipDetail(nType, nJobInstructId, nJobInstructSeq);
}


/**
* 中断ボタンを押す
* @return {-} 無し
*/
function ClickBtnStopJobByDetail() {

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

    //中断ウィンドウを表示する
    ShowStopDialog(true);
}

/**
* チップ詳細開始
* @param {String} nRestFlg 休憩フラグ
* @param {String} nReStartJobFlg 再開フラグ
* @param {String} nType 開始類別:AllStart、SingleStart、ReStart
* @return {-} 無し
*/
function StartByChipDetail(nRestFlg, nReStartJobFlg, nType) {
    var jonInstructId;
    var jonInstructSeq;
    var strChipId = gSelectedChipId;
    // 開始時刻を取得する
    var dtStartNow = GetServerTimeNow();
    dtStartNow.setSeconds(0);
    dtStartNow.setMilliseconds(0);

    var dtNowDateTime = new Date();
    dtNowDateTime.setTime(dtStartNow.getTime());

    if (gSC3240201JobInstructIdList.length > 0) {
        jonInstructId = gSC3240201JobInstructIdList[0];
    }
    if (gSC3240201JobInstructSeqList.length > 0) {
        jonInstructSeq = gSC3240201JobInstructSeqList[0];
    }

    // dbを更新する
    // 渡す引数
    var jsonData;
    //既に開始中の場合
    if (gArrObjChip[strChipId].svcStatus == "05") {
        jsonData = {
            Method: nType,
            DlrCD: $("#hidDlrCD").val(),                       //販売店コード
            StrCD: $("#hidBrnCD").val(),                       //店舗コード
            ShowDate: $("#hidShowDate").val(),
            SvcInId: gArrObjChip[strChipId].svcInId,
            JobDtlId: gArrObjChip[strChipId].jobDtlId,
            ReStartJobFlg: nReStartJobFlg,
            StallUseId: gArrObjChip[strChipId].stallUseId,
            StallUseStatus: gArrObjChip[strChipId].stallUseStatus,
            StallId: gArrObjChip[strChipId].stallId,
            ChipDispStartDate: gArrObjChip[strChipId].rsltStartDateTime,
            StartProcessTime: dtNowDateTime,
            ProcWorkTime: gArrObjChip[strChipId].scheWorkTime,
            RowLockVersion: gArrObjChip[strChipId].rowLockVersion,
            SubAreaId: "",
            RONum: gArrObjChip[strChipId].roNum,                  //RO番号
            StallStartTime: $("#hidStallStartTime").val(),     //営業開始時間(HH:mm)
            StallEndTime: $("#hidStallEndTime").val(),         //営業終了時間(HH:mm)
            ChipStartFlg: gStartChipFig
        };
    } else {
        jsonData = {
            Method: nType,
            DlrCD: $("#hidDlrCD").val(),                       //販売店コード
            StrCD: $("#hidBrnCD").val(),                       //店舗コード
            ShowDate: $("#hidShowDate").val(),
            SvcInId: gArrObjChip[strChipId].svcInId,
            JobDtlId: gArrObjChip[strChipId].jobDtlId,
            ReStartJobFlg: nReStartJobFlg,
            StallUseId: gArrObjChip[strChipId].stallUseId,
            StallUseStatus: gArrObjChip[strChipId].stallUseStatus,
            StallId: gArrObjChip[strChipId].stallId,
            ChipDispStartDate: dtNowDateTime,
            StartProcessTime: dtNowDateTime,
            PlanWorkTime: gArrObjChip[strChipId].scheWorkTime,
            RowLockVersion: gArrObjChip[strChipId].rowLockVersion,
            SubAreaId: "",
            RONum: gArrObjChip[strChipId].roNum,                  //RO番号
            StallStartTime: $("#hidStallStartTime").val(),     //営業開始時間(HH:mm)
            StallEndTime: $("#hidStallEndTime").val(),         //営業終了時間(HH:mm)
            ChipStartFlg: gStartChipFig
        };
    }
    if (jonInstructId) {
        jsonData.JobInstructId = jonInstructId;
        jsonData.JobInstructSeq = jonInstructSeq;
    }
    if (nRestFlg) {
        jsonData.RestFlg = nRestFlg;
    }

    // JsonDataのバックアップを取る
    gBackupStartJson[gArrObjChip[strChipId].stallUseId] = jsonData;
    //コールバック開始
    DoCallBack(C_CALLBACK_WND201, jsonData, AfterCallBackActionOperation, jsonData.Method);
}

/**
* チップ詳細終了
* @param {String} nRestFlg 休憩フラグ
* @param {String} nStopJobFinishFlg 中断Job終了フラグ
* @param {String} nType 終了類別:AllFinish、SingleFinish
* @return {-} 無し
*/
function FinishByChipDetail(nType, nJobInstructId, nJobInstructSeq) {
    var strChipId = gSelectedChipId;

    var dtEndNow = GetServerTimeNow();

    // dbを更新する
    // 渡す引数
    var jsonData;
    jsonData = {
        Method: nType,
        DlrCD: $("#hidDlrCD").val(),                       //販売店コード
        StrCD: $("#hidBrnCD").val(),                       //店舗コード
        ShowDate: $("#hidShowDate").val(),
        SvcInId: gArrObjChip[strChipId].svcInId,
        JobDtlId: gArrObjChip[strChipId].jobDtlId,
        JobInstructId: nJobInstructId,
        JobInstructSeq: nJobInstructSeq,
        StallUseId: gArrObjChip[strChipId].stallUseId,
        StallUseStatus: gArrObjChip[strChipId].stallUseStatus,
        StallId: gArrObjChip[strChipId].stallId,
        PlanWorkTime: gArrObjChip[strChipId].scheWorkTime,
        FinishProcessTime: dtEndNow,
        ChipDispStartDate: gArrObjChip[strChipId].rsltStartDateTime,
        RowLockVersion: gArrObjChip[strChipId].rowLockVersion,
        SubAreaId: "",
        RONum: gArrObjChip[strChipId].roNum,                  //RO番号
        StallStartTime: $("#hidStallStartTime").val(),     //営業開始時間(HH:mm)
        StallEndTime: $("#hidStallEndTime").val(),         //営業終了時間(HH:mm)
        ChipFinishFlg: gFinishChipFig
    };
 
    // JsonDataのバックアップを取る
    gBackupStartJson[gArrObjChip[strChipId].stallUseId] = jsonData;
    //コールバック開始
    DoCallBack(C_CALLBACK_WND201, jsonData, AfterCallBackActionOperation, jsonData.Method);
}

/**
* チップ詳細中断
* @param {String} strStopReasonType 中断理由
* @param {String} nStopTime 中断時間
* @param {String} strStopMemo 中断メモ
* @param {String} nRestFlg 休憩フラグ
* @return {-} 無し
*/
function StopByChipDetail(strStopReasonType, nStopTime, strStopMemo, nRestFlg) {
    var dtEndNow = GetServerTimeNow();
    var strChipId = gSelectedChipId;
    var jonInstructId;
    var jonInstructSeq;
    var nStoptype = "AllStop";
    // 開始時刻を取得する
    var dtStartNow = GetServerTimeNow();
    dtStartNow.setSeconds(0);
    dtStartNow.setMilliseconds(0);

    var dtNowDateTime = new Date();
    dtNowDateTime.setTime(dtStartNow.getTime());
    //アクティブインジケータ表示
    gDetailSActiveIndicator.show();
    //オーバーレイ表示
    gDetailOverlay.show();

    //リフレッシュタイマーセット
    commonRefreshTimer(ReDisplayChipDetail);

    if (gSC3240201JobInstructIdList.length > 0) {
        jonInstructId = gSC3240201JobInstructIdList[0];
        nStoptype = "SingleStop"
    }
    if (gSC3240201JobInstructSeqList.length > 0) {
        jonInstructSeq = gSC3240201JobInstructSeqList[0];
    }

    // dbを更新する
    // 渡す引数
    var jsonData;
    if (jonInstructId) {
        jsonData = {
            Method: nStoptype,
            DlrCD: $("#hidDlrCD").val(),                       //販売店コード
            StrCD: $("#hidBrnCD").val(),                       //店舗コード
            ShowDate: $("#hidShowDate").val(),
            SvcInId: gArrObjChip[strChipId].svcInId,
            JobDtlId: gArrObjChip[strChipId].jobDtlId,
            StallUseId: gArrObjChip[strChipId].stallUseId,
            ChipDispStartDate: gArrObjChip[strChipId].rsltStartDateTime,
            PlanWorkTime: gArrObjChip[strChipId].scheWorkTime,
            FinishProcessTime: dtEndNow,
            JobInstructId: jonInstructId,
            JobInstructSeq: jonInstructSeq,
            StallUseStatus: gArrObjChip[strChipId].stallUseStatus,
            StallId: gArrObjChip[strChipId].stallId,
            SubAreaId: "",
            RONum: gArrObjChip[strChipId].roNum,                  //RO番号
            StallWaitTime: nStopTime,
            StopReasonType: strStopReasonType,
            StopMemo: strStopMemo,
            RowLockVersion: gArrObjChip[strChipId].rowLockVersion,
            StallStartTime: $("#hidStallStartTime").val(),     //営業開始時間(HH:mm)
            StallEndTime: $("#hidStallEndTime").val(),         //営業終了時間(HH:mm)
            ChipStopFlg: gStopChipFig
        };
    } else {
        jsonData = {
            Method: nStoptype,
            DlrCD: $("#hidDlrCD").val(),                       //販売店コード
            StrCD: $("#hidBrnCD").val(),                       //店舗コード
            ShowDate: $("#hidShowDate").val(),
            SvcInId: gArrObjChip[strChipId].svcInId,
            JobDtlId: gArrObjChip[strChipId].jobDtlId,
            StallUseId: gArrObjChip[strChipId].stallUseId,
            ChipDispStartDate: gArrObjChip[strChipId].rsltStartDateTime,
            FinishProcessTime: dtEndNow,
            PlanWorkTime: gArrObjChip[strChipId].scheWorkTime,
            StallUseStatus: gArrObjChip[strChipId].stallUseStatus,
            StallId: gArrObjChip[strChipId].stallId,
            SubAreaId: "",
            RONum: gArrObjChip[strChipId].roNum,                  //RO番号
            StallWaitTime: nStopTime,
            StopReasonType: strStopReasonType,
            StopMemo: strStopMemo,
            RowLockVersion: gArrObjChip[strChipId].rowLockVersion,
            StallStartTime: $("#hidStallStartTime").val(),     //営業開始時間(HH:mm)
            StallEndTime: $("#hidStallEndTime").val(),         //営業終了時間(HH:mm)
            ChipStopFlg: gStopChipFig
        };
    }
    if (nRestFlg) {
        jsonData.RestFlg = nRestFlg;
     }
    
    // JsonDataのバックアップを取る
    gBackupStartJson[gArrObjChip[strChipId].stallUseId] = jsonData;
    //コールバック開始
    DoCallBack(C_CALLBACK_WND201, jsonData, AfterCallBackActionOperation, jsonData.Method);

}

/**
* チップ詳細アクションボタンコールバック後の処理関数(AllStart AllFinish AllStop等)
* 
* @param {String} result コールバック呼び出し結果
*
*/
function AfterCallBackActionOperation(result) {

    var jsonResult = JSON.parse(result);

    //タイマーをクリア
    commonClearTimer();

    //コールバック結果コードの取得
    var resultCD = jsonResult.ResultCode;

    //2015/04/23 TMEJ 明瀬  DMS連携版サービスタブレット強制納車機能追加開発 START

    //if (resultCD != 0) {
    if (resultCD != 0 &&
        resultCD != -9000) {

    //2015/04/23 TMEJ 明瀬  DMS連携版サービスタブレット強制納車機能追加開発 END
    
        //休憩・使用不可チップとの重複以外は、ここでエラーメッセージダイアログを表示
        //  (登録時、更新チップが休憩・使用不可チップと重複した場合は、別途confirmで出す)
        if (resultCD == 8) {
            //登録時、更新チップが休憩・使用不可チップと重複した場合
            //『休憩を取得しますか？(取得する場合はOKを選択、取得しない場合はキャンセルを選択)』
            var result = confirm(htmlDecode($("#WordDuplicateRestOrUnavailableHidden").val()));

            //休憩取得フラグ（0:休憩を取得しない／1:休憩を取得する）
            var restFlg = 0;

            //OK(1:休憩を取得する)をタップした場合
            if (result) {
                restFlg = 1;
            }
            if ((jsonResult.Caller == "AllStart") ||
            (jsonResult.Caller == "SingleStart") ||
            (jsonResult.Caller == "ReStart")) {
                gBackupStartJson[gSelectedChipId].ReStartJobFlg = C_RESTARTJOB_NOTSET;
            } else if ((jsonResult.Caller == "AllFinish") || (jsonResult.Caller == "SingleFinish")) {
                gBackupStartJson[gSelectedChipId].FinishStopJobFlg = C_RESTARTJOB_NOTSET;
            } else if ((jsonResult.Caller == "AllStop") || (jsonResult.Caller == "SingleStop")) {
                gBackupStartJson[gSelectedChipId].ChipStopFlg="0"
            }
            gBackupStartJson[gSelectedChipId].RestFlg = restFlg;
            //リフレッシュタイマーセット
            commonRefreshTimer(ReDisplayChipDetail);

            //次の操作を実行する
            AfterCallBack();

            //コールバック開始
            DoCallBack(C_CALLBACK_WND201, gBackupStartJson[gSelectedChipId], AfterCallBackActionOperation, "ClickAllStartButton");

        }
        else if (resultCD == 14) {


            //再開フラグ（1:再開する／2:再開しない）
            var reStartFlg = 2;
            if (confirm(htmlDecode(jsonResult.Message))) {
                reStartFlg = 1;
            }

            if ((jsonResult.Caller == "AllStart") ||
            (jsonResult.Caller == "SingleStart") ||
            (jsonResult.Caller == "ReStart")) {
                gBackupStartJson[gSelectedChipId].ReStartJobFlg = reStartFlg;
                //リフレッシュタイマーセット
                commonRefreshTimer(ReDisplayChipDetail);

                //次の操作を実行する
                AfterCallBack();

                //コールバック開始
                DoCallBack(C_CALLBACK_WND201, gBackupStartJson[gSelectedChipId], AfterCallBackActionOperation, jsonResult.Caller);
            } else if ((jsonResult.Caller == "AllFinish") || (jsonResult.Caller == "SingleFinish")) {
                if (reStartFlg == 2) {
                    //終了をキャンセル

                    //次の操作を実行する
                    AfterCallBack();
                    //アクティブインジケータ・オーバーレイ非表示
                    gDetailSActiveIndicator.hide();
                    gDetailOverlay.hide();
                    gMainAreaActiveIndicator.hide();

                    return false;
                } else {
                    //終了続行する
                    gBackupStartJson[gSelectedChipId].FinishStopJobFlg = reStartFlg;

                    //リフレッシュタイマーセット
                    commonRefreshTimer(ReDisplayChipDetail);

                    //次の操作を実行する
                    AfterCallBack();

                    //コールバック開始
                    DoCallBack(C_CALLBACK_WND201, gBackupStartJson[gSelectedChipId], AfterCallBackActionOperation, jsonResult.Caller);
                }
            }
        }
        else if (resultCD == 15) {
            //登録時、更新チップが休憩・使用不可チップと重複した場合
            //『休憩を取得しますか？(取得する場合はOKを選択、取得しない場合はキャンセルを選択)』
            var result = confirm(htmlDecode($("#WordDuplicateRestOrUnavailableHidden").val()));

            //休憩取得フラグ（0:休憩を取得しない／1:休憩を取得する）
            var restFlg = 0;

            //OK(1:休憩を取得する)をタップした場合
            if (result) {
                restFlg = 1;
            }
            //再開フラグ（1:再開する／2:再開しない）
            var reStartFlg = 2;
            if (confirm(htmlDecode(jsonResult.Message))) {
                reStartFlg = 1;
            }

            if ((jsonResult.Caller == "AllStart") ||
            (jsonResult.Caller == "SingleStart") ||
            (jsonResult.Caller == "ReStart")) {
                gBackupStartJson[gSelectedChipId].RestFlg = restFlg;
                gBackupStartJson[gSelectedChipId].ReStartJobFlg = reStartFlg;

                //リフレッシュタイマーセット
                commonRefreshTimer(ReDisplayChipDetail);

                //次の操作を実行する
                AfterCallBack();

                //コールバック開始
                DoCallBack(C_CALLBACK_WND201, gBackupStartJson[gSelectedChipId], AfterCallBackActionOperation, "ClickAllStartButton");
            } else if ((jsonResult.Caller == "AllFinish") || (jsonResult.Caller == "SingleFinish")) {
                if (reStartFlg == 2) {
                    //終了をキャンセル

                    //次の操作を実行する
                    AfterCallBack();
                    //アクティブインジケータ・オーバーレイ非表示
                    gDetailSActiveIndicator.hide();
                    gDetailOverlay.hide();
                    gMainAreaActiveIndicator.hide();

                    return false;
                } else {
                    //終了続行する
                    gBackupStartJson[gSelectedChipId].RestFlg = restFlg;
                    gBackupStartJson[gSelectedChipId].FinishStopJobFlg = reStartFlg;

                    //リフレッシュタイマーセット
                    commonRefreshTimer(ReDisplayChipDetail);

                    //次の操作を実行する
                    AfterCallBack();

                    //コールバック開始
                    DoCallBack(C_CALLBACK_WND201, gBackupStartJson[gSelectedChipId], AfterCallBackActionOperation, jsonResult.Caller);
                }
            }
        } else {
            icropScript.ShowMessageBox(resultCD, htmlDecode(jsonResult.Message), "");
            //アクティブインジケータ・オーバーレイ非表示
            gDetailSActiveIndicator.hide();
            gDetailOverlay.hide();
            gMainAreaActiveIndicator.hide();

            //次の操作を実行する
            AfterCallBack();
        }
    } else {

        //初期化
        //Chip中断フラグ
        gStopChipFig = "1";
        //Chip開始フラグ
        gStartChipFig = "1";
        //Chip終了フラグ
        gFinishChipFig = "1";

        //工程管理メインの表示を最新化する
        ShowLatestChips(htmlDecode(jsonResult.StallChipInfo), true, false);

        //2015/04/23 TMEJ 明瀬  DMS連携版サービスタブレット強制納車機能追加開発 START

        //DMS除外エラーの警告が発生した場合
        if (jsonResult.ResultCode == -9000) {

            //メッセージを表示する
            icropScript.ShowMessageBox(jsonResult.ResultCode, jsonResult.Message, "");

        }

        //2015/04/23 TMEJ 明瀬  DMS連携版サービスタブレット強制納車機能追加開発 END

        // 新規した使用不可エリアのIDを取得する
        var strStallIdleId = jsonResult.NewStallIdleId;
        // 新規した使用不可エリアチップがあれば、
        if ((strStallIdleId != "0") && (strStallIdleId != "")) {

            // 該行移動不可チップ全部リフレッシュ
            UpdateIdleArea(htmlDecode(jsonResult.StallIdleInfo));

            // 使用不可エリアのイベントを登録する
            BindUnavailableAreaEvent(C_UNAVALIABLECHIPID + strStallIdleId);
        }
        if ((jsonResult.Caller == "AllStop") || (jsonResult.Caller == "SingleStop") || (jsonResult.Caller == "SingleFinish") || (jsonResult.Caller == "AllFinish")) {
            gMainAreaActiveIndicator.hide();
            // 中断ボタン数値を更新する
            StopButtonRefresh();

        } else if ((jsonResult.Caller == "AllStart") || (jsonResult.Caller == "SingleStart") || (jsonResult.Caller == "ReStart")) {
            // 作業中チップの履歴情報の設定
            SetWorkingChipHis(htmlDecode(jsonResult.WorkingChipHisInfo));
        }

        // 選択されたチップが無くなった場合
        if ((!CheckgArrObjChip(gSelectedChipId)) && (!CheckgArrObjSubChip(gSelectedChipId))) {

            //アクティブインジケータ・オーバーレイ非表示
            gDetailSActiveIndicator.hide();
            gDetailOverlay.hide();
            gMainAreaActiveIndicator.hide();

            // 詳細画面を閉じる
            HideChipDetailPopup();
            // 選択したチップを解放する
            SetTableUnSelectedStatus();
            // チップ選択状態を解除する
            SetChipUnSelectedStatus();

            //次の操作を実行する
            AfterCallBack();
            return;
        }

        //詳細画面を最新化
        InitPageForJobDispatch(result, "");

        //選択チップのID保存する
        var strSelectedChipId = gSelectedChipId;

        SetChipUnSelectedStatus();  //チップ選択状態を解除
        SetTableUnSelectedStatus();

        TapStallChip(strSelectedChipId);

        //フッターのボタンを全て非表示にする
        HideAllFooterButton();

        //工程管理画面で選択されているチップID
        var baseCtrl = $("#" + strSelectedChipId);

        //ポップアップの表示位置を設定
        SetDetailSPopoverPosition(CalcDetailSPopoverPosition(baseCtrl));

        ExpandDisplay();

        //アクティブインジケータ・オーバーレイ非表示
        gDetailSActiveIndicator.hide();
        gDetailOverlay.hide();
        gMainAreaActiveIndicator.hide();

        //固定文言をチップ詳細(小)から(大)へコピーする
        CopyToDetailLWord();
        //リピーター系以外の動的データをチップ詳細(小)から(大)へコピーする
        CopyToDetailLData();

        //次の操作を実行する
        AfterCallBack();

        //部品詳細取得APIでエラーが発生した場合、メッセージを表示する(部品以外は正常表示)
        var errorMsg = $("#PartsDtlErrMsgHidden").val();
        if (errorMsg != "") {
            icropScript.ShowMessageBox("", errorMsg, "");
        }
    }
}


/**
* 画面を最新化する(JobDispatch)
*
*/
function InitPageForJobDispatch(result, context) {

    //スクロール退避
    var scrollInner = $("#ChipDetailLContent .scroll-inner").attr("style");

    //コールバックによって取得したHTMLを設定
    var jsonResult = JSON.parse(result);
    SetChipDetailContents(jsonResult.Contents);

    //チップ詳細(小)に縦スクロールの設定
    $("#ChipDetailSContent").fingerScroll();

    //チップ詳細(大)に縦スクロールの設定
    $("#ChipDetailLContent").fingerScroll();

    //スクロールを保持する
    $("#ChipDetailLContent .scroll-inner").attr("style", scrollInner);

    //チップ詳細(大)のチップエリア横スクロールの設定
    $("#scrollChip").SC3240201fingerScroll();

    //CustomLabelの適用
    $("#ChipDetailPopup .ChipDetailEllipsis").CustomLabel({ useEllipsis: true });

    //見た目やイベントの設定を行う
    SetChipDetailDisplayAndEvent();

}

/**
* コールバックで取得したHTMLを画面に設定する(JobDispatch)
* 
* @param {String} cbResult コールバック呼び出し結果
* 
*/
function SetChipDetailContentsForJobDispatch(cbResult) {

    //コールバックによって取得したHTMLを格納
    var contents = $('<Div>').html(cbResult).text();

    //チップ詳細(小)のコンテンツを取得
    var detailS = $(contents).find('#ChipDetailSContent');

    //チップ詳細(大)のコンテンツを取得
    var detailL = $(contents).find('#ChipDetailLContent');

    //チップ詳細のHiddenコンテンツを取得
    var detailHidden = $(contents).find('#SC3240201HiddenArea');

    //チップ詳細(小)のコンテンツを削除
    $('#ChipDetailSContent>div').remove();

    //チップ詳細(小)のコンテンツを設定
    detailS.children('div').clone(true).appendTo('#ChipDetailSContent');

    //チップ詳細(大)のコンテンツを削除
    $('#ChipDetailLContent>div').remove();

    //チップ詳細(大)のコンテンツを設定
    detailL.children('div').clone(true).appendTo('#ChipDetailLContent');

    //チップ詳細のHiddenコンテンツを削除
    $('#SC3240201HiddenArea>div').remove();

    //チップ詳細のHiddenコンテンツを設定
    detailHidden.children('div').clone(true).appendTo('#SC3240201HiddenArea');
}
//2014/07/15 TMEJ 丁 タブレットSMB Job Dispatch機能開発 END