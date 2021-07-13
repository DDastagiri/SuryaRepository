//---------------------------------------------------------
//ChipPrototype.js
//---------------------------------------------------------
//機能：SMBメイン画面_予約チップクラス
//作成：2012/12/22 TMEJ 張 タブレット版SMB機能開発(工程管理)
//更新：2013/11/29 TMEJ 張 タブレット版SMB チーフテクニシャン機能開発
//更新：2014/06/18 TMEJ 丁 タブレット版SMB テレマ走行距離機能開発
//更新：2014/07/15 TMEJ 張 タブレットSMB Job Dispatch機能開発
//更新：2014/09/24 TMEJ 張 ﾀﾌﾞﾚｯﾄSMBの遅れ見込み/遅れ表示変更
//更新：2014/12/04 TMEJ 丁 新プラットフォーム版i-CROP仕様変更対応（サービス入庫実績更新処理変更）
//更新：2015/04/01 TMEJ 小澤 BTS-261対応 サービス名の表示制御の修正
//更新：2015/10/02 TM 小牟禮 チップ描画でDOM操作処理を少なくするように修正（性能改善）
//更新：2017/09/25 NSK 竹中(悠) REQ-SVT-TMT-20170227-002 見込納車遅れ日時計算ロジック統一
//更新：2017/10/21 NSK 小川 REQ-SVT-TMT-20160906-003 子チップがキャンセルできない
//更新：2018/02/20 NSK 小川 17PRJ01136-00 (トライ店システム評価)お客様受付における情報伝達の仕組み 適合性検証
//更新：2018/07/06 NSK 可児 TKM Next Gen e-CRB Project Application development Block B-1  Pマーク、Lマークなどを表示
//更新：
//---------------------------------------------------------

//ストール予約チップクラス
//チップの生成まで、実際の配置は呼び出したクラスで行うものとする

//チップ情報を格納し、チップを生成配置
//@class チップ情報の格納・生成クラス
//       チップの情報を所持し、それらを取り扱う機能を保有
//@param {String} aRezId チップID（プライマリー）
var gStallArray = new Array();

function ReserveChip(aChipId) {
    this.stallUseId = aChipId;        //チップID
    this.svcInId = "";                                          //サービス入庫ID
    this.dlrCd = "";                                            //販売店コード
    this.brnCd = "";                                            //店舗コード
    this.cstId = "";                                            //顧客ID
    this.vclId = "";                                            //車両ID
    this.cstVclType = "";                                       //顧客車両区分
    this.tlmContractFlg = "";                                   //テレマ契約フラグ
    this.acceptanceTpye = "";                                   //受付区分
    this.pickDeliType = "";                                     //引取納車区分
    this.carWashNeedFlg = "";                                   //洗車必要フラグ
    this.resvStatus = "";                                       //予約ステータス
    this.svcStatus = "";                                        //サービスステータス
    this.scheSvcInDateTime = new Date(C_DATE_DEFAULT_VALUE);    //予定入庫日時
    this.scheDeliDateTime = new Date(C_DATE_DEFAULT_VALUE);     //予定納車日時
    this.rsltSvcInDateTime = new Date(C_DATE_DEFAULT_VALUE);    //実績入庫日時
    this.rsltDeliDateTime = new Date(C_DATE_DEFAULT_VALUE);     //実績納車日時
    this.rowUpdateDateTime = new Date(C_DATE_DEFAULT_VALUE);    //行更新日時
    this.roNum = "";                                            //RO番号
    this.rowLockVersion = 0;                                    //行ロックバージョン
    this.jobDtlId = "";                                         //作業内容ID
    this.inspectionNeedFlg = "";                                //検査必要フラグ
    this.inspectionApprovalFlg = "";                            //検査承認待ちフラグ
    this.cancelFlg = "";                                        //キャンセルフラグ
    this.stallId = "";                                          //ストールID
    this.tempFlg = "";                                          //仮置きフラグ
    this.partsFlg = "";                                         //部品準備完了フラグ
    this.stallUseStatus = "";                                   //ストール利用ステータス
    this.scheStartDateTime = new Date(C_DATE_DEFAULT_VALUE);    //予定開始日時
    this.scheEndDateTime = new Date(C_DATE_DEFAULT_VALUE);      //予定終了日時
    this.scheWorkTime = 0;                                      //予定作業時間
    this.restFlg = "";                                          //休憩取得フラグ
    this.rsltStartDateTime = new Date(C_DATE_DEFAULT_VALUE);    //実績開始日時
    this.prmsEndDateTime = new Date(C_DATE_DEFAULT_VALUE);      //見込終了日時
    this.rsltEndDateTime = new Date(C_DATE_DEFAULT_VALUE);      //実績終了日時
    this.rsltWorkTime = 0;                                      //実績作業時間
    this.stopReasonType = "";                                   //中断理由区分
    this.vclVin = "";                                           //VIN
    this.modelName = "";                                        //モデル名
    this.regNum = "";                                           //車両登録番号
    this.carWashRsltId = 0;                                     //洗車実績ID
    this.cwRsltStartDateTime = new Date(C_DATE_DEFAULT_VALUE);  //洗車実績開始日時
    this.cwRsltEndDateTime = new Date(C_DATE_DEFAULT_VALUE);    //洗車実績終了日時
    this.svcClassName = "";                                     //サービス分類名称
    this.svcClassNameEng = "";                                  //サービス分類名称（英語）
    this.upperDisp = "";                                        //商品マーク上部表示文字列
    this.lowerDisp = "";                                        //商品マーク下部表示文字列
    this.roJobSeq = -1;                                         //作業連番
    this.planDelayDate = new Date(C_DATE_DEFAULT_VALUE);        //遅れ見込み時刻
    this.addWorkStatus = "";                                    //追加作業起票申請状態
    this.displayStartDate = new Date(C_DATE_DEFAULT_VALUE);     //表示開始日時
    this.displayEndDate = new Date(C_DATE_DEFAULT_VALUE);       //表示終了日時
    this.stopFlg = false;                                       //中断フラグ
    this.delayStatus = C_NO_DELAY;                              //遅刻フラグ

    //2014/06/18 TMEJ 丁 タブレット版SMB テレマ走行距離機能開発 START
    this.tlmContractFlg = "";                                   //テレマフラグ
    //2014/06/18 TMEJ 丁 タブレット版SMB テレマ走行距離機能開発 END

    //2018/07/06 NSK 可児 TKM Next Gen e-CRB Project Application development Block B-1  Pマーク、Lマークなどを表示 START
    this.impVclFlg = "";                                        //P/Lマークフラグ
    //2018/07/06 NSK 可児 TKM Next Gen e-CRB Project Application development Block B-1  Pマーク、Lマークなどを表示 END
    // 2013/11/29 TMEJ 張 タブレット版SMB チーフテクニシャン機能開発 START
    // 履歴情報
    if (IsKariKariChip(this.stallUseId) == false) {
        if (HasChiphistory(this.stallUseId) == false) {
            this.chiphisScheStartDateTime = new Date(C_DATE_DEFAULT_VALUE);    //予定開始日時
            this.chiphisScheEndDateTime = new Date(C_DATE_DEFAULT_VALUE);      //予定終了日時
            this.chiphisStallId = "";                                          //ストールID
            this.chiphisSvcStatus = "";                                        //サービスステータス
            this.chiphisResvStatus = "";                                       //予約ステータス
            this.chiphisScheWorktime = 0;                                      //予定作業時間
        }
    }
    // 2013/11/29 TMEJ 張 タブレット版SMB チーフテクニシャン機能開発 END

    // 2017/10/19 NSK 竹中(悠) REQ-SVT-TMT-20170227-002 見込納車遅れ日時計算ロジック統一 START
    this.remainingInspectionType = "";                             //残完成検査区分
    // 2017/10/19 NSK 竹中(悠) REQ-SVT-TMT-20170227-002 見込納車遅れ日時計算ロジック統一 END
}

ReserveChip.prototype = {
    //予約チップクラスのメンバ変数にデータベースから取得した値を格納
    //@param {DataSet} aDataSet データベースより取得した値
    //@return {void}
    setChipParameter: function setChipParameter(aDataSet) {

        this.setSvcInId(aDataSet.SVCIN_ID);                         // サービス入庫ID
        this.setDlrCd(aDataSet.DLR_CD);                             // 販売店コード
        this.setBrnCd(aDataSet.BRN_CD);                             // 店舗コード
        this.setCstId(aDataSet.CST_ID);                             // 顧客ID
        this.setVclId(aDataSet.VCL_ID);                             // 車両ID
        this.setCstVclType(aDataSet.CST_VCL_TYPE);                  // 顧客車両区分
        this.setTlmContractFlg(aDataSet.TLM_CONTRACT_FLG);          // テレマ契約フラグ
        this.setAcceptanceTpye(aDataSet.ACCEPTANCE_TYPE);           // 受付区分
        this.setPickDeliType(aDataSet.PICK_DELI_TYPE);              // 引取納車区分
        this.setCarWashNeedFlg(aDataSet.CARWASH_NEED_FLG);          // 洗車必要フラグ
        this.setResvStatus(aDataSet.RESV_STATUS);                   // 予約ステータス
        this.setSvcStatus(aDataSet.SVC_STATUS);                     // サービスステータス
        this.setScheSvcInDateTime(aDataSet.SCHE_SVCIN_DATETIME);    // 予定入庫日時
        this.setScheDeliDateTime(aDataSet.SCHE_DELI_DATETIME);      // 予定納車日時
        this.setRsltSvcInDateTime(aDataSet.RSLT_SVCIN_DATETIME);    // 実績入庫日時
        this.setRsltDeliDateTime(aDataSet.RSLT_DELI_DATETIME);      // 実績納車日時
        this.setRowUpdateDateTime(aDataSet.ROW_UPDATE_DATETIME);    // 行更新日時
        this.setRowLockVersion(aDataSet.ROW_LOCK_VERSION);          // 行ロックバージョン
        this.setRoNum(aDataSet.RO_NUM);                             // RO番号
        this.setJobDtlId(aDataSet.JOB_DTL_ID);                      // 作業内容ID
        this.setInspectionNeedFlg(aDataSet.INSPECTION_NEED_FLG);    // 検査必要フラグ
        this.setInspectionApprovalFlg(aDataSet.INSPECTION_STATUS);  // 検査承認待ちフラグ
        this.setCancelFlg(aDataSet.CANCEL_FLG);                     // キャンセルフラグ
        this.setStallUseId(aDataSet.STALL_USE_ID);                  // ストール利用ID
        this.setStallId(aDataSet.STALL_ID);                         // ストールID
        this.setTempFlg(aDataSet.TEMP_FLG);                         // 仮置きフラグ
        this.setPartsFlg(aDataSet.PARTS_FLG);                       // 部品準備完了フラグ
        this.setStallUseStatus(aDataSet.STALL_USE_STATUS);          // ストール利用ステータス
        this.setScheStartDateTime(aDataSet.SCHE_START_DATETIME);    // 予定開始日時
        this.setScheEndDateTime(aDataSet.SCHE_END_DATETIME);        // 予定終了日時
        this.setScheWorkTime(aDataSet.SCHE_WORKTIME);               // 予定作業時間
        this.setRestFlg(aDataSet.REST_FLG);                         // 休憩取得フラグ
        this.setRsltStartDateTime(aDataSet.RSLT_START_DATETIME);    // 実績開始日時
        this.setPrmsEndDateTime(aDataSet.PRMS_END_DATETIME);        // 見込終了日時
        this.setRsltEndDateTime(aDataSet.RSLT_END_DATETIME);        // 実績終了日時
        this.setRsltWorkTime(aDataSet.RSLT_WORKTIME);               // 実績作業時間
        this.setStopReasonType(aDataSet.STOP_REASON_TYPE);          // 中断理由区分
        this.setVclVin(aDataSet.VCL_VIN);                           // VIN 
        this.setModelName(aDataSet.MODEL_NAME);                     // モデル名 
        this.setRegNum(aDataSet.REG_NUM);                           // 車両登録番号
        this.setCarWashRsltId(aDataSet.CARWASH_RSLT_ID);            // 洗車実績ID
        this.setCwRsltStartDateTime(aDataSet.CW_RSLT_START_DATETIME); // 洗車実績開始日時
        this.setCwRsltEndDateTime(aDataSet.CW_RSLT_END_DATETIME);   // 洗車実績終了日時
        this.setSvcClassName(aDataSet.SVC_CLASS_NAME);              // サービス分類名称
        this.setSvcClassNameEng(aDataSet.SVC_CLASS_NAME_ENG);       // サービス分類名称（英語）
        this.setUpperDisp(aDataSet.UPPER_DISP);                     // 商品マーク上部表示文字列
        this.setLowerDisp(aDataSet.LOWER_DISP);                     // 商品マーク下部表示文字列
        this.setRoJobSeq(aDataSet.RO_JOB_SEQ)                       // 顧客承認連番(WORKSEQ)
        this.setPlanDelayDate(aDataSet.PLAN_DELAYDATE);             // 遅れ見込み時刻
        this.setAddWorkStatus(aDataSet.ADDWORK_STATUS);             // 追加作業起票申請状態
        // 2017/10/19 NSK 竹中(悠) REQ-SVT-TMT-20170227-002 見込納車遅れ日時計算ロジック統一 START
        this.setRemainingInspectionType(aDataSet.REMAINING_INSPECTION_TYPE);       // 残完成検査区分
        // 2017/10/19 NSK 竹中(悠) REQ-SVT-TMT-20170227-002 見込納車遅れ日時計算ロジック統一 END

        //2018/07/06 NSK 可児 TKM Next Gen e-CRB Project Application development Block B-1  Pマーク、Lマークなどを表示 START
        this.setImpVclFlg(aDataSet.IMP_VCL_FLG);                    // P/Lマーク表示
        //2018/07/06 NSK 可児 TKM Next Gen e-CRB Project Application development Block B-1  Pマーク、Lマークなどを表示 END
        // DISPLAY_STARTDATEがあれば、直接データを設定する
        if (aDataSet.DISPLAY_STARTDATE) {
            this.setDisplayStartDate(aDataSet.DISPLAY_STARTDATE);   // 表示開始日時
            this.setDisplayEndDate(aDataSet.DISPLAY_ENDDATE);       // 表示終了日時
        } else {
            this.setDisplayDate();                                  // チップ表示用の開始、終了日時
        }
    },

    // 2013/11/29 TMEJ 張 タブレット版SMB チーフテクニシャン機能開発 START
    //作業中チップの履歴情報を設定する
    //@return {void}
    SetWorkingChipHis: function SetWorkingChipHis(aDataSet) {
        this.chiphisScheStartDateTime = new Date(aDataSet.SCHE_START_DATETIME);    // 予定開始日時
        this.chiphisScheEndDateTime = new Date(aDataSet.SCHE_END_DATETIME);        // 予定終了日時
        this.chiphisStallId = aDataSet.STALL_ID;                         // ストールID
        this.chiphisSvcStatus = aDataSet.SVC_STATUS;                     // サービスステータス
        this.chiphisResvStatus = aDataSet.RESV_STATUS;                   // 予約ステータス
        this.chiphisScheWorktime = aDataSet.SCHE_WORKTIME;               // 予定作業時間
    },

    //作業中チップの履歴情報をクリアする
    //@return {void}
    clearWorkingChipHisInfo: function clearWorkingChipHisInfo() {
        this.chiphisScheStartDateTime = new Date(C_DATE_DEFAULT_VALUE);    //予定開始日時
        this.chiphisScheEndDateTime = new Date(C_DATE_DEFAULT_VALUE);      //予定終了日時
        this.chiphisStallId = "";                                          //ストールID
        this.chiphisSvcStatus = "";                                        //サービスステータス
        this.chiphisResvStatus = "";                                       //予約ステータス
        this.chiphisScheWorktime = 0;                                      // 予定作業時間
    },
    // 2013/11/29 TMEJ 張 タブレット版SMB チーフテクニシャン機能開発 END

    //仮仮チップ情報の設定
    //@return {void}
    setKariKariChipParameter: function setKariKariChipParameter(aDataSet) {
        this.setSvcInId(aDataSet.SVCIN_ID);                         // サービス入庫ID
        this.setJobDtlId(aDataSet.JOB_DTL_ID);                      // 作業内容ID
        this.setStallUseId(aDataSet.SVCIN_TEMP_RESV_ID);            // ストール利用ID
        this.setStallId(aDataSet.STALL_ID);                         // ストールID
        this.setDisplayStartDate(aDataSet.START_DATETIME);          // 表示開始日時
        this.setDisplayEndDate(aDataSet.END_DATETIME);              // 表示終了日時
        this.setModelName(aDataSet.STF_NAME);                       // スタッフ名前を表示する
    },

    //チップのコピー
    //@return {void}
    copy: function copy(objDestChip) {
        objDestChip.svcInId = this.svcInId;
        objDestChip.dlrCd = this.dlrCd;
        objDestChip.brnCd = this.brnCd;
        objDestChip.cstId = this.cstId;
        objDestChip.vclId = this.vclId;
        objDestChip.cstVclType = this.cstVclType;
        objDestChip.tlmContractFlg = this.tlmContractFlg;
        objDestChip.acceptanceTpye = this.acceptanceTpye;
        objDestChip.pickDeliType = this.pickDeliType;
        objDestChip.carWashNeedFlg = this.carWashNeedFlg;
        objDestChip.resvStatus = this.resvStatus;
        objDestChip.svcStatus = this.svcStatus;
        objDestChip.scheSvcInDateTime = new Date(this.scheSvcInDateTime);
        objDestChip.scheDeliDateTime = new Date(this.scheDeliDateTime);
        objDestChip.rsltSvcInDateTime = new Date(this.rsltSvcInDateTime);
        objDestChip.rsltDeliDateTime = new Date(this.rsltDeliDateTime);
        objDestChip.rowUpdateDateTime = new Date(this.rowUpdateDateTime);
        objDestChip.rowLockVersion = this.rowLockVersion;
        objDestChip.jobDtlId = this.jobDtlId;
        objDestChip.inspectionNeedFlg = this.inspectionNeedFlg;
        objDestChip.inspectionApprovalFlg = this.inspectionApprovalFlg;
        objDestChip.cancelFlg = this.cancelFlg;
        objDestChip.stallUseId = this.stallUseId;
        objDestChip.stallId = this.stallId;
        objDestChip.tempFlg = this.tempFlg;
        objDestChip.partsFlg = this.partsFlg;
        objDestChip.stallUseStatus = this.stallUseStatus;
        objDestChip.scheStartDateTime = new Date(this.scheStartDateTime);
        objDestChip.scheEndDateTime = new Date(this.scheEndDateTime);
        objDestChip.scheWorkTime = this.scheWorkTime;
        objDestChip.restFlg = this.restFlg;
        objDestChip.rsltStartDateTime = new Date(this.rsltStartDateTime);
        objDestChip.prmsEndDateTime = new Date(this.prmsEndDateTime);
        objDestChip.rsltEndDateTime = new Date(this.rsltEndDateTime);
        objDestChip.rsltWorkTime = this.rsltWorkTime;
        objDestChip.stopReasonType = this.stopReasonType;
        objDestChip.vclVin = this.vclVin;
        objDestChip.modelName = this.modelName;
        objDestChip.regNum = this.regNum;
        objDestChip.carWashRsltId = this.carWashRsltId;
        objDestChip.cwRsltStartDateTime = new Date(this.cwRsltStartDateTime);
        objDestChip.cwRsltEndDateTime = new Date(this.cwRsltEndDateTime);
        objDestChip.svcClassName = this.svcClassName;
        objDestChip.svcClassNameEng = this.svcClassNameEng;
        objDestChip.upperDisp = this.upperDisp;
        objDestChip.lowerDisp = this.lowerDisp;
        objDestChip.roJobSeq = this.roJobSeq;
        objDestChip.stopFlg = this.stopFlg;
        objDestChip.addWorkStatus = this.addWorkStatus;
        objDestChip.roNum = this.roNum;
        objDestChip.scheDeliDateTime = new Date(this.scheDeliDateTime);
        objDestChip.planDelayDate = new Date(this.planDelayDate);
        objDestChip.displayStartDate = new Date(this.displayStartDate);
        objDestChip.displayEndDate = new Date(this.displayEndDate);
        objDestChip.delayStatus = this.delayStatus;

        // 2017/10/19 NSK 竹中(悠) REQ-SVT-TMT-20170227-002 見込納車遅れ日時計算ロジック統一 START
        objDestChip.remainingInspectionType = this.remainingInspectionType;
        // 2017/10/19 NSK 竹中(悠) REQ-SVT-TMT-20170227-002 見込納車遅れ日時計算ロジック統一 END
        //2018/07/06 NSK 可児 TKM Next Gen e-CRB Project Application development Block B-1  Pマーク、Lマークなどを表示 START
        objDestChip.impVclFlg = this.impVclFlg;
        //2018/07/06 NSK 可児 TKM Next Gen e-CRB Project Application development Block B-1  Pマーク、Lマークなどを表示 END
    },

    //全てデータをjsonのstringを出力
    //@param {-}
    //@return {String}
    toJsonSting: function toJsonSting() {
        var jsonData = '{'
        jsonData += '"SVCIN_ID":"' + transferNullToBlank(this.svcInId) + '"';
        jsonData += ',"DLR_CD":"' + transferNullToBlank(this.dlrCd) + '"';
        jsonData += ',"BRN_CD":"' + transferNullToBlank(this.brnCd) + '"';
        jsonData += ',"CST_ID":"' + transferNullToBlank(this.cstId) + '"';
        jsonData += ',"VCL_ID":"' + transferNullToBlank(this.vclId) + '"';
        jsonData += ',"CST_VCL_TYPE":"' + transferNullToBlank(this.cstVclType) + '"';
        jsonData += ',"TLM_CONTRACT_FLG":"' + transferNullToBlank(this.tlmContractFlg) + '"';
        jsonData += ',"ACCEPTANCE_TYPE":"' + transferNullToBlank(this.acceptanceTpye) + '"';
        jsonData += ',"PICK_DELI_TYPE":"' + transferNullToBlank(this.pickDeliType) + '"';
        jsonData += ',"CARWASH_NEED_FLG":"' + transferNullToBlank(this.carWashNeedFlg) + '"';
        jsonData += ',"RESV_STATUS":"' + transferNullToBlank(this.resvStatus) + '"';
        jsonData += ',"SVC_STATUS":"' + transferNullToBlank(this.svcStatus) + '"';
        jsonData += ',"SCHE_SVCIN_DATETIME":"' + transferNullToBlank(this.scheSvcInDateTime) + '"';
        jsonData += ',"SCHE_DELI_DATETIME":"' + transferNullToBlank(this.scheDeliDateTime) + '"';
        jsonData += ',"RSLT_SVCIN_DATETIME":"' + transferNullToBlank(this.rsltSvcInDateTime) + '"';
        jsonData += ',"RSLT_DELI_DATETIME":"' + transferNullToBlank(this.rsltDeliDateTime) + '"';
        jsonData += ',"ROW_UPDATE_DATETIME":"' + transferNullToBlank(this.rowUpdateDateTime) + '"';
        jsonData += ',"ROW_LOCK_VERSION":"' + transferNullToBlank(this.rowLockVersion) + '"';
        jsonData += ',"RO_NUM":"' + transferNullToBlank(this.roNum) + '"';
        jsonData += ',"JOB_DTL_ID":"' + transferNullToBlank(this.jobDtlId) + '"';
        jsonData += ',"INSPECTION_NEED_FLG":"' + transferNullToBlank(this.inspectionNeedFlg) + '"';
        jsonData += ',"INSPECTION_STATUS":"' + transferNullToBlank(this.inspectionApprovalFlg) + '"';
        jsonData += ',"CANCEL_FLG":"' + transferNullToBlank(this.cancelFlg) + '"';
        jsonData += ',"STALL_USE_ID":"' + transferNullToBlank(this.stallUseId) + '"';
        jsonData += ',"STALL_ID":"' + transferNullToBlank(this.stallId) + '"';
        jsonData += ',"TEMP_FLG":"' + transferNullToBlank(this.tempFlg) + '"';
        jsonData += ',"PARTS_FLG":"' + transferNullToBlank(this.partsFlg) + '"';
        jsonData += ',"STALL_USE_STATUS":"' + transferNullToBlank(this.stallUseStatus) + '"';
        jsonData += ',"SCHE_START_DATETIME":"' + transferNullToBlank(this.scheStartDateTime) + '"';
        jsonData += ',"SCHE_END_DATETIME":"' + transferNullToBlank(this.scheEndDateTime) + '"';
        jsonData += ',"SCHE_WORKTIME":"' + transferNullToBlank(this.scheWorkTime) + '"';
        jsonData += ',"REST_FLG":"' + transferNullToBlank(this.restFlg) + '"';
        jsonData += ',"RSLT_START_DATETIME":"' + transferNullToBlank(this.rsltStartDateTime) + '"';
        jsonData += ',"PRMS_END_DATETIME":"' + transferNullToBlank(this.prmsEndDateTime) + '"';
        jsonData += ',"RSLT_END_DATETIME":"' + transferNullToBlank(this.rsltEndDateTime) + '"';
        jsonData += ',"RSLT_WORKTIME":"' + transferNullToBlank(this.rsltWorkTime) + '"';
        jsonData += ',"STOP_REASON_TYPE":"' + transferNullToBlank(this.stopReasonType) + '"';
        jsonData += ',"VCL_VIN":"' + transferNullToBlank(this.vclVin) + '"';
        jsonData += ',"MODEL_NAME":"' + transferNullToBlank(this.modelName) + '"';
        jsonData += ',"REG_NUM":"' + transferNullToBlank(this.regNum) + '"';
        jsonData += ',"CARWASH_RSLT_ID":"' + transferNullToBlank(this.carWashRsltId) + '"';
        jsonData += ',"CW_RSLT_START_DATETIME":"' + transferNullToBlank(this.cwRsltStartDateTime) + '"';
        jsonData += ',"CW_RSLT_END_DATETIME":"' + transferNullToBlank(this.cwRsltEndDateTime) + '"';
        jsonData += ',"SVC_CLASS_NAME":"' + transferNullToBlank(this.svcClassName) + '"';
        jsonData += ',"SVC_CLASS_NAME_ENG":"' + transferNullToBlank(this.svcClassNameEng) + '"';
        jsonData += ',"UPPER_DISP":"' + transferNullToBlank(this.upperDisp) + '"';
        jsonData += ',"LOWER_DISP":"' + transferNullToBlank(this.lowerDisp) + '"';
        jsonData += ',"RO_JOB_SEQ":"' + transferNullToBlank(this.roJobSeq) + '"';
        jsonData += ',"STOP_FLG":"' + transferNullToBlank(this.stopFlg) + '"';
        jsonData += ',"PLAN_DELAYDATE":"' + transferNullToBlank(this.planDelayDate) + '"';
        jsonData += ',"ADDWORK_STATUS":"' + transferNullToBlank(this.addWorkStatus) + '"';
        jsonData += ',"DISPLAY_STARTDATE":"' + transferNullToBlank(this.displayStartDate) + '"';
        jsonData += ',"DISPLAY_ENDDATE":"' + transferNullToBlank(this.displayEndDate) + '"';
        // 2017/10/19 NSK 竹中(悠) REQ-SVT-TMT-20170227-002 見込納車遅れ日時計算ロジック統一 START
        jsonData += ',"REMAINING_INSPECTION_TYPE":"' + transferNullToBlank(this.remainingInspectionType) + '"';
        // 2017/10/19 NSK 竹中(悠) REQ-SVT-TMT-20170227-002 見込納車遅れ日時計算ロジック統一 END
        //2018/07/06 NSK 可児 TKM Next Gen e-CRB Project Application development Block B-1  Pマーク、Lマークなどを表示 START
        jsonData += ',"IMP_VCL_FLG":"' + transferNullToBlank(this.impVclFlg) + '"';
        //2018/07/06 NSK 可児 TKM Next Gen e-CRB Project Application development Block B-1  Pマーク、Lマークなどを表示 END
        jsonData += '}';
        return jsonData;
    },

    //全チップを生成
    //@param {String} nChipType チップタイプ
    //@return {void}
    createChip: function createChip(nChipType) {
        switch (nChipType) {
            case C_CHIPTYPE_STALL:
            case C_CHIPTYPE_OTHER_DAY:
            case C_CHIPTYPE_STALL_FASTER:
                this.createStallChip(nChipType);
                break;
            case C_CHIPTYPE_STALL_MOVING:
                this.createMovingChip();
                break;
            case C_CHIPTYPE_STALL_COPYMOVING:
                this.createCopyMovingChip();
                break;
            case C_CHIPTYPE_POPUP:
                this.createPopUpChip();
                break;
            case C_CHIPTYPE_STALL_NEW:
                this.createStallNewChip();
                break;
            case C_CHIPTYPE_COPY:
                this.createCopyChip();
                break;
            case C_CHIPTYPE_STALL_KARIKARI:
                this.createKariKariChip();
                break;
        }
        return true;
    },

    //ストールチップを生成する
    //@param {String} nChipType チップタイプ
    //@return {void}
    createStallChip: function createStallChip(nChipType) {

        //2015/10/02 TM 小牟禮 チップ描画でDOM操作処理を少なくするように修正（性能改善） START
        /*
        var strData = "";   //一時データ

        var objChip = $("<div />").addClass("MCp");    //チップの枠
        objChip.attr("id", this.stallUseId);
        var strColor = this.getChipColor(); //チップの色クラス名を取得
        objChip.addClass(strColor);

        //2014/07/15 TMEJ 張 タブレットSMB Job Dispatch機能開発 START

        //作業計画の一部の作業が中断かつ該当日付が実績開始日時と同じ日付の場合(日跨ぎチップの翌日部分がピンク色付けない)
        var dtShowDate = new Date($("#hidShowDate").val());
        if ((C_STALLUSE_STATUS_STARTINCLUDESTOPJOB == this.stallUseStatus)
        && (CompareDate(this.rsltStartDateTime, dtShowDate) == 0)) {

        //赤色を追加する
        objChip.addClass("StoppingJobColor");

        }

        //2014/07/15 TMEJ 張 タブレットSMB Job Dispatch機能開発 END

        //横の枠
        var objBand = $("<div />").addClass("Band");
        objBand.append(this.getBandIcons());
        objChip.append(objBand);

        //タイトル文字
        //車名
        strData = "";
        if (this.modelName) {
        strData = this.modelName;
        }
        objChip.append("<h3>" + strData + "</h3>");

        //車番号
        strData = "";
        if (this.regNum) {
        strData = this.regNum;
        }
        var objCarNo = $("<div />").addClass("CarNoL");
        objCarNo.append("<span>" + strData + "</span>");
        objChip.append(objCarNo);

        //2014/06/18 TMEJ 丁 タブレット版SMB テレマ走行距離機能開発 START 
        //Gアイコン
        if (this.tlmContractFlg == "1") {
        var objGicon = $("<div />").addClass("GIcon");
        objChip.append(objGicon);
        }
        //2014/06/18 TMEJ 丁 タブレット版SMB テレマ走行距離機能開発 END

        //時間
        var strDateTime = this.getDateOrTime();
        if (strDateTime != "") {
        var objTime = $("<div />").addClass("time");
        objTime.append(strDateTime);
        objChip.append(objTime);
        }

        //下のアイコン
        objChip.append(this.getStatusIcons());

        //整備内容
        strData = "";
        strData = this.getSvcName();

        var objInfo = $("<div />").addClass("infoBox");
        if (strData != "") {
        objInfo.append(strData);
        }
        objChip.append(objInfo);
        //一番前のdiv(タップ用)
        var objFrontFace = $("<div />").addClass("Front");
        objChip.append(objFrontFace);
        */

        var strData = "";   //一時データ
        var html = "";

        var strColor = this.getChipColor(); //チップの色クラス名を取得

        //作業計画の一部の作業が中断かつ該当日付が実績開始日時と同じ日付の場合(日跨ぎチップの翌日部分がピンク色付けない)
        var dtShowDate = new Date($("#hidShowDate").val());
        if ((C_STALLUSE_STATUS_STARTINCLUDESTOPJOB == this.stallUseStatus)
            && (CompareDate(this.rsltStartDateTime, dtShowDate) == 0)) {

            html += "<div id='" + this.stallUseId + "' class='MCp " + strColor + " StoppingJobColor'>";

        }
        else {
            html += "<div id='" + this.stallUseId + "' class='MCp " + strColor + "'>";
        }

        //横の枠
        html += "<div class='Band'>" + this.getBandIcons() + "</div>";

        //タイトル文字
        //車名
        strData = "";
        if (this.modelName) {
            strData = this.modelName;
        }
        html += "<h3>" + strData + "</h3>";

        //車番号
        strData = "";
        if (this.regNum) {
            strData = this.regNum;
        }
        html += "<div class='CarNoL'><span>" + strData + "</span></div>";

        //Gアイコン
        if (this.tlmContractFlg == "1") {
            html += "<div class='GIcon'></div>";
        }

        //時間
        var strDateTime = this.getDateOrTime();
        if (strDateTime != "") {
            html += "<div class='time'>" + strDateTime + "</div>";
        }

        //下のアイコン
        html += this.getStatusIcons();

        //整備内容
        strData = "";
        strData = this.getSvcName();

        if (strData != "") {

            html += "<div class='infoBox'>" + strData + "</div>";
        }
        else {
            html += "<div class='infoBox'></div>";
        }

        //一番前のdiv(タップ用)
        html += "<div class='Front'></div>";

        html += "</div>";

        var objChip = $(html);
        //2015/10/02 TM 小牟禮 チップ描画でDOM操作処理を少なくするように修正（性能改善） END

        if ((this.stallId != "0") && ($("#stallId_" + this.stallId).length > 0)) {
            var nRowNum;
            if (C_CHIPTYPE_STALL_FASTER == nChipType) {
                nRowNum = parseInt($("#stallId_" + this.stallId)[0].className.substr(7));
            } else {
                nRowNum = $("#stallId_" + this.stallId).position().top / C_CELL_HEIGHT + 1;
            }
            if (nChipType != C_CHIPTYPE_OTHER_DAY) {
                $(".Row" + nRowNum).append(objChip);    //チップをメインストールに追加
            } else {
                $("#ulStall").append(objChip);  //チップをストール名称に追加
            }
        } else {
            $(".ChipArea").append(objChip);
        }
    },

    //ストール上仮仮チップを生成する
    //@param {String} nChipType チップタイプ
    //@return {void}
    createKariKariChip: function createKariKariChip(nChipType) {
        // 2013/11/29 TMEJ 張 タブレット版SMB チーフテクニシャン機能開発 START
        //        var objChip = $("<div />").addClass("MCp StTRez ");    //水青いチップの枠
        var objChip = $("<div />").addClass("MCp StTRez KARIKARI");    //水青いチップの枠
        // 2013/11/29 TMEJ 張 タブレット版SMB チーフテクニシャン機能開発 END

        objChip.attr("id", C_KARIKARICHIPID + this.stallUseId); // 他のチップと衝突しなくように、C_KARIKARICHIPIDを追加

        //横の枠
        var objBand = $("<div />").addClass("Band");
        objBand.append(this.getBandIcons());
        objChip.append(objBand);

        //タイトル文字
        //車名
        var strData = "";
        if (this.modelName) {
            strData = this.modelName;
        }
        objChip.append("<h3>" + strData + "</h3>");

        //整備内容
        var objInfo = $("<div />").addClass("infoBox");
        objChip.append(objInfo);

        //一番前のdiv(タップ用)
        var objFrontFace = $("<div />").addClass("Front");
        objChip.append(objFrontFace);

        // 2013/11/29 TMEJ 張 タブレット版SMB チーフテクニシャン機能開発 START
        if ($("#stallId_" + this.stallId).length > 0) {
            // 2013/11/29 TMEJ 張 タブレット版SMB チーフテクニシャン機能開発 END
            var nRowNum = parseInt($("#stallId_" + this.stallId)[0].className.substr(7));
            $(".Row" + nRowNum).append(objChip);    //チップをチップアリアに追加
            // 2013/11/29 TMEJ 張 タブレット版SMB チーフテクニシャン機能開発 START
        }
        // 2013/11/29 TMEJ 張 タブレット版SMB チーフテクニシャン機能開発 END
    },

    //Movingチップを生成する
    //@return {void}
    createMovingChip: function createMovingChip() {
        var strData = "";   //一時データ
        var objTimeKnobPointL, objTimeKnobPointR;   //爪

        var objChip = $("<div />").addClass("MCp");    //チップの枠
        objChip.attr("id", this.stallUseId);
        var strColor = this.getChipColor(); //チップの色クラス名を取得

        //Movingチップの場合、内枠、爪を追加
        var objChipColor = $("<div />").addClass("CpInner").addClass(strColor);

        //2014/06/18 TMEJ 丁 タブレット版SMB テレマ走行距離機能開発 START 
        //車番号
        strData = "";
        if (this.regNum) {
            strData = this.regNum;
        }
        var objCarNo = $("<div />").addClass("CarNoL");
        objCarNo.append("<span>" + strData + "</span>")
        objChipColor.append(objCarNo);
        //Gアイコン
        if (this.tlmContractFlg == "1") {
            var objGicon = $("<div />").addClass("GIcon");
            objChipColor.append(objGicon);
        }
        //2014/06/18 TMEJ 丁 タブレット版SMB テレマ走行距離機能開発 END
        objChip.append(objChipColor);
        objTimeKnobPointL = $("<div />").addClass("TimeKnobPointL");
        objTimeKnobPointR = $("<div />").addClass("TimeKnobPointR");

        //objChipColorのクラスにより、爪の色を設定
        var strChipColorClassName = objChipColor[0].className;
        if (strChipColorClassName.indexOf("StTRez") > -1) {

            // 2018/02/20 NSK 小川 お客様受付における情報伝達の仕組み 適合性検証 START
            // //水青
            // objTimeKnobPointL.addClass("TimeKnobPoint_skyblue");
            // objTimeKnobPointR.addClass("TimeKnobPoint_skyblue");

            //オレンジ
            objTimeKnobPointL.addClass("TimeKnobPoint_orange");
            objTimeKnobPointR.addClass("TimeKnobPoint_orange");
            // 2018/02/20 NSK 小川 お客様受付における情報伝達の仕組み 適合性検証 END

            objChip.append(objTimeKnobPointL);
            objChip.append(objTimeKnobPointR);
        } else if (strChipColorClassName.indexOf("StRez") > -1) {
            if (strChipColorClassName.indexOf("StDelay") > -1) {
                //赤
                objTimeKnobPointL.addClass("TimeKnobPoint_red");
                objTimeKnobPointR.addClass("TimeKnobPoint_red");
                objChip.append(objTimeKnobPointL);
                objChip.append(objTimeKnobPointR);
            } else {
                //青
                objTimeKnobPointL.addClass("TimeKnobPoint_blue");
                objTimeKnobPointR.addClass("TimeKnobPoint_blue");
                objChip.append(objTimeKnobPointL);
                objChip.append(objTimeKnobPointR);
            }
        } else if (strChipColorClassName.indexOf("StWork") > -1) {
            //緑
            objTimeKnobPointR.addClass("TimeKnobPoint_green");
            objChip.append(objTimeKnobPointR);
        }

        //横の枠
        var objBand = $("<div />").addClass("Band");
        objBand.append(this.getBandIcons());
        objChipColor.append(objBand);

        //タイトル文字
        //車名
        strData = "";
        if (this.modelName) {
            strData = this.modelName;
        }
        objChipColor.append("<h3>" + strData + "</h3>");
        //2014/06/18 TMEJ 丁 タブレット版SMB テレマ走行距離機能開発 START
        //車番号
        //strData = "";
        //if (this.regNum) {
        //    strData = this.regNum;
        //}
        //var objCarNo = $("<div />").addClass("CarNoL");
        //objCarNo.append("<span>" + strData + "</span>");
        //objChip.append(objCarNo);
        //2014/06/18 TMEJ 丁 タブレット版SMB テレマ走行距離機能開発 END

        //時間
        var strDateTime = this.getDateOrTime();
        if (strDateTime != "") {
            var objTime = $("<div />").addClass("time");
            objTime.append(strDateTime);
            objChipColor.append(objTime);
        }

        //下のアイコン
        objChipColor.append(this.getStatusIcons());

        //整備内容
        strData = "";
        strData = this.getSvcName();
        var objInfo = $("<div />").addClass("infoBox");
        if (strData != "") {
            objInfo.append(strData);
        }
        objChipColor.append(objInfo);
        //一番前のdiv(タップ用)
        var objFrontFace = $("<div />").addClass("Front");
        objChipColor.append(objFrontFace);
        // 該当ストール画面にあれば
        if ($("#stallId_" + this.stallId).length > 0) {
            var nRowNum = $("#stallId_" + this.stallId).position().top / C_CELL_HEIGHT + 1;
            $(".Row" + nRowNum).append(objChip);    //チップをチップアリアに追加
        }
        // サブエリアのチップタップする時
        else {
            $(".ChipArea").append(objChip);
        }
    },

    //CopyMovingチップを生成する
    //@return {void}
    createCopyMovingChip: function createCopyMovingChip() {
        var strData = "";   //一時データ
        var objTimeKnobPointL, objTimeKnobPointR;   //爪

        var objChip = $("<div />").addClass("MCp");    //チップの枠
        objChip.attr("id", this.stallUseId);
        var strColor = this.getChipColor(); //チップの色クラス名を取得

        //Movingチップの場合、内枠、爪を追加
        var objChipColor = $("<div />").addClass("CpInner").addClass(strColor);

        //2014/06/18 TMEJ 丁 タブレット版SMB テレマ走行距離機能開発 START
        //車番号
        strData = "";
        if (this.regNum) {
            strData = this.regNum;
        }
        var objCarNo = $("<div />").addClass("CarNoL");
        objCarNo.append("<span>" + strData + "</span>");
        objChipColor.append(objCarNo);
        //Gアイコン
        if (this.tlmContractFlg == "1") {
            var objGicon = $("<div />").addClass("GIcon");
            objChipColor.append(objGicon);
        }
        //2014/06/18 TMEJ 丁 タブレット版SMB テレマ走行距離機能開発 END

        objChip.append(objChipColor);
        objTimeKnobPointL = $("<div />").addClass("TimeKnobPointL");
        objTimeKnobPointR = $("<div />").addClass("TimeKnobPointR");

        //objChipColorのクラスにより、爪の色を設定
        var strChipColorClassName = objChipColor[0].className;
        if (strChipColorClassName.indexOf("StTRez") > -1) {

            // 2018/02/20 NSK 小川 お客様受付における情報伝達の仕組み 適合性検証 START
            // //水青
            // objTimeKnobPointL.addClass("TimeKnobPoint_skyblue");
            // objTimeKnobPointR.addClass("TimeKnobPoint_skyblue");

            //オレンジ
            objTimeKnobPointL.addClass("TimeKnobPoint_orange");
            objTimeKnobPointR.addClass("TimeKnobPoint_orange");
            // 2018/02/20 NSK 小川 お客様受付における情報伝達の仕組み 適合性検証 END

            objChip.append(objTimeKnobPointL);
            objChip.append(objTimeKnobPointR);
        } else if (strChipColorClassName.indexOf("StRez") > -1) {
            if (strChipColorClassName.indexOf("StDelay") > -1) {
                //赤
                objTimeKnobPointL.addClass("TimeKnobPoint_red");
                objTimeKnobPointR.addClass("TimeKnobPoint_red");
                objChip.append(objTimeKnobPointL);
                objChip.append(objTimeKnobPointR);
            } else {
                //青
                objTimeKnobPointL.addClass("TimeKnobPoint_blue");
                objTimeKnobPointR.addClass("TimeKnobPoint_blue");
                objChip.append(objTimeKnobPointL);
                objChip.append(objTimeKnobPointR);
            }
        } else if (strChipColorClassName.indexOf("StWork") > -1) {
            //緑
            objTimeKnobPointR.addClass("TimeKnobPoint_green");
            objChip.append(objTimeKnobPointR);
        }

        //横の枠
        var objBand = $("<div />").addClass("Band");
        objBand.append(this.getBandIcons());
        objChipColor.append(objBand);

        //タイトル文字
        //車名
        strData = "";
        if (this.modelName) {
            strData = this.modelName;
        }
        objChipColor.append("<h3>" + strData + "</h3>");
        //2014/06/18 TMEJ 丁 タブレット版SMB テレマ走行距離機能開発  START
        //        //車番号
        //        strData = "";
        //        if (this.regNum) {
        //            strData = this.regNum;
        //        }
        //        var objCarNo = $("<div />").addClass("CarNoL");
        //        objCarNo.append("<span>" + strData + "</span>");
        //        objChip.append(objCarNo);
        //2014/06/18 TMEJ 丁 タブレット版SMB テレマ走行距離機能開発  END
        //時間
        var strDateTime = this.getDateOrTime();
        if (strDateTime != "") {
            var objTime = $("<div />").addClass("time");
            objTime.append(strDateTime);
            objChipColor.append(objTime);
        }

        //下のアイコン
        objChipColor.append(this.getCopyStatusIcons());

        //整備内容
        strData = "";
        strData = this.getSvcName();
        var objInfo = $("<div />").addClass("infoBox");
        if (strData != "") {
            objInfo.append(strData);
        }
        objChipColor.append(objInfo);
        //一番前のdiv(タップ用)
        var objFrontFace = $("<div />").addClass("Front");
        objChipColor.append(objFrontFace);
        if ($("#stallId_" + this.stallId).length > 0) {
            var nRowNum = $("#stallId_" + this.stallId).position().top / C_CELL_HEIGHT + 1;
            $(".Row" + nRowNum).append(objChip);    //チップをチップアリアに追加
        } else {
            $(".ChipArea").append(objChip);
        }
    },

    //ストールチップを生成
    //@return {void}
    createPopUpChip: function createPopUpChip() {
        var strData = "";   // 一時データ
        var objChip = $("<div />").addClass("MCp");    //チップの枠
        objChip.attr("id", this.stallUseId);
        var strColor = this.getChipColor(); //チップの色クラス名を取得
        objChip.addClass(strColor);

        //2014/07/15 TMEJ 張 タブレットSMB Job Dispatch機能開発 START

        //作業計画の一部の作業が中断かつ該当日付が実績開始日時と同じ日付の場合(日跨ぎチップの翌日部分がピンク色付けない)
        var dtShowDate = new Date($("#hidShowDate").val());
        if ((C_STALLUSE_STATUS_STARTINCLUDESTOPJOB == this.stallUseStatus)
            && (CompareDate(this.rsltStartDateTime, dtShowDate) == 0)) {

            //赤色を追加する
            objChip.addClass("StoppingJobColor");

        }

        //2014/07/15 TMEJ 張 タブレットSMB Job Dispatch機能開発 END

        var objBand = $("<div />").addClass("Band");    //横の枠
        objBand.append(this.getBandIcons());
        objChip.append(objBand);

        //タイトル文字
        //車名
        strData = "";
        if (this.modelName) {
            strData = this.modelName;
        }
        objChip.append("<h3>" + strData + "</h3>");

        //車番号
        strData = "";
        if (this.regNum) {
            strData = this.regNum;
        }
        var objCarNo = $("<div />").addClass("CarNoL");
        objCarNo.append("<span>" + strData + "</span>");
        objChip.append(objCarNo);

        //2014/06/18 TMEJ 丁 タブレット版SMB テレマ走行距離機能開発 START 
        //Gアイコン
        if (this.tlmContractFlg == "1") {
            var objGicon = $("<div />").addClass("GIcon");
            objChip.append(objGicon);
        }
        //2014/06/18 TMEJ 丁 タブレット版SMB テレマ走行距離機能開発 END

        //時間
        var strDateTime = this.getDateOrTime();
        if (strDateTime != "") {
            var objTime = $("<div />").addClass("time");
            objTime.append(strDateTime);
            objChip.append(objTime);
        }

        //下のアイコン
        objChip.append(this.getStatusIcons());

        //整備内容
        strData = "";
        strData = this.getSvcName();

        var objInfo = $("<div />").addClass("infoBox");
        if (strData != "") {
            objInfo.append(strData);
        }
        objChip.append(objInfo);
        //一番前のdiv(タップ用)
        var objFrontFace = $("<div />").addClass("Front");
        objChip.append(objFrontFace);

        $(".PopUpChipInnerBox").append(objChip);    //ポップアップボックスにチップを生成
        this.refleshChipRedColor(); //遅刻色を更新
    },

    // 2013/11/29 TMEJ 張 タブレット版SMB チーフテクニシャン機能開発 START
    //空白のnewチップを生成
    createStallNewChip: function createStallNewChip() {
        //チップの枠
        var objChip = $("<div />").addClass("MCp StNew");
        objChip.attr("id", C_NEWCHIPID);

        //爪
        var objTimeKnobPointL = $("<div />").addClass("TimeKnobPointL TimeKnobPoint_white");
        var objTimeKnobPointR = $("<div />").addClass("TimeKnobPointR TimeKnobPoint_white");
        objChip.append(objTimeKnobPointL);
        objChip.append(objTimeKnobPointR);

        //横の枠
        var objBand = $("<div />").addClass("Band");
        objChip.append(objBand);

        //時間
        var objTime = $("<div />").addClass("time");
        objChip.append(objTime);

        //整備内容
        var objInfo = $("<div />").addClass("infoBox");
        objChip.append(objInfo);
        //チップをチップアリアに追加
        $(".ChipArea").append(objChip);
    },
    // 2013/11/29 TMEJ 張 タブレット版SMB チーフテクニシャン機能開発 END

    //Movingチップを生成する
    //@param {String} nChipType チップタイプ
    //@return {void}
    createCopyChip: function createCopyChip() {

        var strData = "";   //一時データ

        var objChip = $("<div />").addClass("MCp");    //チップの枠
        objChip.attr("id", C_COPYCHIPID);
        var strColor = this.getCopyChipColor(); //チップの色クラス名を取得
        objChip.addClass(strColor);

        //横の枠
        var objBand = $("<div />").addClass("Band");
        objBand.append(this.getBandIcons());
        objChip.append(objBand);

        //タイトル文字
        //車名
        strData = "";
        if (this.modelName) {
            strData = this.modelName;
        }
        objChip.append("<h3>" + strData + "</h3>");

        //車番号
        strData = "";
        if (this.regNum) {
            strData = this.regNum;
        }
        var objCarNo = $("<div />").addClass("CarNoL");
        objCarNo.append("<span>" + strData + "</span>");
        objChip.append(objCarNo);

        //2014/06/18 TMEJ 丁 タブレット版SMB テレマ走行距離機能開発 START 
        //Gアイコン
        if (this.tlmContractFlg == "1") {
            var objGicon = $("<div />").addClass("GIcon");
            objChip.append(objGicon);
        }
        //2014/06/18 TMEJ 丁 タブレット版SMB テレマ走行距離機能開発 END

        //時間
        var strDateTime = this.getDateOrTime();
        if (strDateTime != "") {
            var objTime = $("<div />").addClass("time");
            objTime.append(strDateTime);
            objChip.append(objTime);
        }

        //下のアイコン
        objChip.append(this.getCopyStatusIcons());

        //整備内容
        strData = "";
        strData = this.getSvcName();

        var objInfo = $("<div />").addClass("infoBox");
        if (strData != "") {
            objInfo.append(strData);
        }
        objChip.append(objInfo);
        //一番前のdiv(タップ用)
        var objFrontFace = $("<div />").addClass("Front");
        objChip.append(objFrontFace);

        $("#ulStall").append(objChip);  //チップをチップアリアに追加(liに入れないの原因はliが背景色がある)

    },

    //ストールチップを更新
    updateStallChip: function updateStallChip() {
        var strData = "";   //一時データ
        var objChip = $("#" + this.stallUseId);  //チップの枠

        objChip.children().remove();    //子Div且つMCp以外のクラスは全部削除

        //2014/07/15 TMEJ 張 タブレットSMB Job Dispatch機能開発 START
        //        objChip.removeClass("StRez StWork StTRez StWComplete StDComplete StNew");  //色のクラスは全部削除
        objChip.removeClass("StRez StWork StTRez StWComplete StDComplete StNew StoppingJobColor");  //色のクラスは全部削除
        //2014/07/15 TMEJ 張 タブレットSMB Job Dispatch機能開発 END

        //チップの色を決定するdiv
        var strColor = this.getChipColor(); //チップの色クラス名を取得
        objChip.addClass(strColor);

        //2014/07/15 TMEJ 張 タブレットSMB Job Dispatch機能開発 START
        //作業計画の一部の作業が中断かつ該当日付が実績開始日時と同じ日付の場合(日跨ぎチップの翌日部分がピンク色付けない)
        var dtShowDate = new Date($("#hidShowDate").val());
        if ((C_STALLUSE_STATUS_STARTINCLUDESTOPJOB == this.stallUseStatus)
            && (CompareDate(this.rsltStartDateTime, dtShowDate) == 0)) {

            //赤色を追加する
            objChip.addClass("StoppingJobColor");

        }
        //2014/07/15 TMEJ 張 タブレットSMB Job Dispatch機能開発 END

        //横の枠
        var objBand = $("<div />").addClass("Band");
        objBand.append(this.getBandIcons());
        objChip.append(objBand);

        //タイトル文字
        //車名
        strData = "";
        if (this.modelName) {
            strData = this.modelName;
        }
        objChip.append("<h3>" + strData + "</h3>");

        //車番号
        strData = "";
        if (this.regNum) {
            strData = this.regNum;
        }
        var objCarNo = $("<div />").addClass("CarNoL");
        objCarNo.append("<span>" + strData + "</span>");
        objChip.append(objCarNo);

        //2014/06/18 TMEJ 丁 タブレット版SMB テレマ走行距離機能開発 START 
        //Gアイコン
        if (this.tlmContractFlg == "1") {
            var objGicon = $("<div />").addClass("GIcon");
            objChip.append(objGicon);
        }
        //2014/06/18 TMEJ 丁 タブレット版SMB テレマ走行距離機能開発 END

        //時間
        var strDateTime = this.getDateOrTime();
        if (strDateTime != "") {
            var objTime = $("<div />").addClass("time");
            objTime.append(strDateTime);
            objChip.append(objTime);
        }

        //下のアイコン
        objChip.append(this.getStatusIcons());

        //整備内容
        strData = "";
        strData = this.getSvcName();

        var objInfo = $("<div />").addClass("infoBox");
        if (strData != "") {
            objInfo.append(strData);
        }

        objChip.append(objInfo);
        //一番前のdiv(タップ用)
        var objFrontFace = $("<div />").addClass("Front");
        objChip.append(objFrontFace);

        this.refleshChipRedColor(); //遅刻色を更新
        //幅により、文字を調整
        if ((this.stallUseId != "") && (this.stallUseId != "0")) {
            AdjustChipItemByWidth(this.stallUseId);
        }
    },
    //チップが遅刻するかどうかをチェック
    checkChipLater: function checkChipLater() {
        this.delayStatus = C_NO_DELAY;  //初期化

        //2014/09/24 TMEJ 張 ﾀﾌﾞﾚｯﾄSMBの遅れ見込み/遅れ表示変更 START
        //        //納車時間がないまたは納車完了チップのdelayStatusがC_NO_DELAY
        //        if ((IsDefaultDate(this.scheDeliDateTime) == false)
        //            && (IsDefaultDate(this.rsltDeliDateTime) == true)) {

        // 実績チップ(グレーチップ)の遅れ表示がいらない
        if ((IsDefaultDate(this.scheDeliDateTime) == false)
            && (IsDefaultDate(this.rsltEndDateTime) == true)) {
            // 予定納車日時がある、かつ実績終了日時がない場合

            // 遅れステータスを計算する
            //2014/09/24 TMEJ 張 ﾀﾌﾞﾚｯﾄSMBの遅れ見込み/遅れ表示変更 END

            // 秒を切り捨てる
            var dtNow = GetServerTimeNow();
            dtNow.setSeconds(0);
            dtNow.setMilliseconds(0);
            var dtDeliTime = new Date(this.scheDeliDateTime);
            dtDeliTime.setSeconds(0);
            dtDeliTime.setMilliseconds(0);
            var dtPlanDelayDate = new Date(this.planDelayDate);
            dtPlanDelayDate.setSeconds(0);
            dtPlanDelayDate.setMilliseconds(0);
            //今の時間見込み遅刻時間、納車予定時間を取得
            var nNow = Date.parse(dtNow);
            var nDeliTime = Date.parse(dtDeliTime);
            var nPlanDelayDate = Date.parse(dtPlanDelayDate);

            //見込み遅刻の場合、nNow == nDeliTimeの場合、サーバ側が遅れ見込み計算してない
            if (((nNow >= nPlanDelayDate) && (IsDefaultDate(nPlanDelayDate) == false))
                || (nNow == nDeliTime)) {
                this.delayStatus = C_DELAY_PROSPECTS;
            }
            //実際遅刻の場合
            if ((nNow > nDeliTime) && (IsDefaultDate(nDeliTime) == false)) {
                this.delayStatus = C_DELAY;
            }
        }
    },

    //生成するチップ種別を設定（併せて、タップの有効フラグも設定）
    getChipColor: function getChipColor() {
        var strRt = "";

        //実際開始してない(作業前)
        if (IsDefaultDate(this.rsltStartDateTime) == true) {
            //仮予約：水青
            if (this.resvStatus == C_RTYPE_TEMP) {
                strRt = "StTRez";
            } else {
                //本予約：青
                strRt = "StRez";
            }
        } else {
            //作業中(実際開始して、未だ終わってない)
            if (IsDefaultDate(this.rsltEndDateTime) == true) {
                var dtNow = GetServerTimeNow();
                var dtShowDate = new Date($("#hidShowDate").val());
                // 作業中チップが実績開始日時の日の場合、表示されるチップが緑
                if (CompareDate(this.rsltStartDateTime, dtShowDate) == 0) {
                    // 緑
                    strRt = "StWork";
                } else {
                    //本予約：青
                    strRt = "StRez";
                }

            } else {
                //作業完了(薄いグレー)  
                //2014/12/04 TMEJ 丁 新プラットフォーム版i-CROP仕様変更対応（サービス入庫実績更新処理変更）START
                //if (IsDefaultDate(this.rsltDeliDateTime) == true) {
                if (this.svcStatus != C_SVCSTATUS_DELIVERY) {
                    //2014/12/04 TMEJ 丁 新プラットフォーム版i-CROP仕様変更対応（サービス入庫実績更新処理変更）END
                    strRt = "StWComplete";
                } else {
                    //納車完了(濃いグレー)
                    strRt = "StDComplete";
                }
            }
        }
        return strRt;
    },

    //コピーされたチップの色設定
    getCopyChipColor: function getCopyChipColor() {
        var strRt = "";
        //仮予約：水青
        if (this.resvStatus == C_RTYPE_TEMP) {
            strRt = "StTRez";
        } else {
            //本予約：青
            strRt = "StRez";
        }
        return strRt;
    },

    //チップの赤色を更新
    refleshChipRedColor: function refleshChipRedColor() {
        //赤いがあるdivクラス
        var strChipColorClass = "#" + this.stallUseId;
        //レセット
        $(strChipColorClass).removeClass("StDelay StPDelay")

        //遅刻を判断
        this.checkChipLater();
        //遅刻のクラス
        var strDelayClass = "";

        if (this.delayStatus == C_DELAY) {  //終了時間（実績）を超える
            strDelayClass = " StDelay";
        } else if (this.delayStatus == C_DELAY_PROSPECTS) { //終了時間（予定）を超える           
            strDelayClass = " StPDelay";
        }

        //納車完了以外のチップの場合赤色を追加
        //2014/12/04 TMEJ 丁 新プラットフォーム版i-CROP仕様変更対応（サービス入庫実績更新処理変更）START
        //if (IsDefaultDate(this.rsltDeliDateTime) == true) {
        //   $(strChipColorClass).addClass(strDelayClass);
        //        }

        $(strChipColorClass).addClass(strDelayClass);
        //2014/12/04 TMEJ 丁 新プラットフォーム版i-CROP仕様変更対応（サービス入庫実績更新処理変更）END
    },

    //Bandエリアにアイコンを決定する
    getBandIcons: function getBandIcons() {
        var strBandHtml = "";
        //VIPマーク Demoでは対象外
        var bVip = false;
        if (bVip) {
            strBandHtml += '<div class="IC01"><p>V</p></div>';
        }
        //店内の場合
        if (this.pickDeliType == C_WAIT_IN) {
            strBandHtml += '<div class="IC02"></div>';
        }
        //予約客の場合
        if (this.acceptanceTpye == C_RFLG_RESERVE) {
            strBandHtml += '<div class="IC03"></div>';
        }

        //2018/07/06 NSK 可児 TKM Next Gen e-CRB Project Application development Block B-1  Pマーク、Lマークなどを表示 START
        //フラグが1の場合
        if (this.impVclFlg == C_ICON_FLAG_P) {
            //Pマーク追加
            strBandHtml += '<div class="IconP"></div>';
            //フラグが2の場合
        } else if (this.impVclFlg == C_ICON_FLAG_L) {
            //Lマーク追加
            strBandHtml += '<div class="IconL"></div>';
        }
        //2018/07/06 NSK 可児 TKM Next Gen e-CRB Project Application development Block B-1  Pマーク、Lマークなどを表示 END


        return strBandHtml;
    },

    //ステータスアイコンを取得する
    getStatusIcons: function getStatusIcons() {
        //追加作業起票中+中断
        if ((this.addWorkStatus == C_AW_ADDINGWORK)
            && (this.stallUseStatus == C_STALLUSE_STATUS_STOP)) {
            return '<div class="ICAddInterrupt"></div>';
        }
        //追加作業承認待ち+中断
        if ((this.addWorkStatus == C_AW_WAIT_COMMITTED)
            && (this.stallUseStatus == C_STALLUSE_STATUS_STOP)) {
            return '<div class="ICWaitInterrupt"></div>';
        }

        //追加作業起票中+完成検査承認待ち
        if ((this.addWorkStatus == C_AW_ADDINGWORK) && (this.inspectionApprovalFlg == C_INSPECTION_APPROVAL)) {
            return '<div class="ICAddInspect"></div>';
        }
        //追加作業承認待ち+完成検査承認待ち
        if ((this.addWorkStatus == C_AW_WAIT_COMMITTED) && (this.inspectionApprovalFlg == C_INSPECTION_APPROVAL)) {
            return '<div class="ICWaitInspect"></div>';
        }

        //追加作業起票中+洗車アイコン(洗車中または洗車待ち)
        if ((this.addWorkStatus == C_AW_ADDINGWORK)
            && ((this.svcStatus == C_SVCSTATUS_CARWASHWAIT) || (this.svcStatus == C_SVCSTATUS_CARWASHSTART))) {
            return '<div class="ICAddWash"></div>';
        }
        //追加作業承認待ち+洗車アイコン(洗車中または洗車待ち)
        if ((this.addWorkStatus == C_AW_WAIT_COMMITTED)
            && ((this.svcStatus == C_SVCSTATUS_CARWASHWAIT) || (this.svcStatus == C_SVCSTATUS_CARWASHSTART))) {
            return '<div class="ICWaitWash"></div>';
        }

        //追加作業起票中
        if (this.addWorkStatus == C_AW_ADDINGWORK) {
            return '<div class="ICAddWork"></div>';
        } else if (this.addWorkStatus == C_AW_WAIT_COMMITTED) {
            //追加作業承認待ち
            return '<div class="ICWaitCommited"></div>';
        }

        //中断
        if (this.stallUseStatus == C_STALLUSE_STATUS_STOP) {
            return '<div class="ICInterrupt"></div>';
        }
        //完成検査承認待ち
        if (this.inspectionApprovalFlg == C_INSPECTION_APPROVAL) {
            return '<div class="ICInspect"></div>';
        }
        //洗車アイコン(洗車中または洗車待ち)
        if ((this.svcStatus == C_SVCSTATUS_CARWASHWAIT) || (this.svcStatus == C_SVCSTATUS_CARWASHSTART)) {
            return '<div class="ICWashCar"></div>';
        }

        //追加作業起票中、追加作業承認待ち、完成検査待ち、洗車アイコンが表示されない場合、下記のアイコンを表示できる
        return this.getIcon();
    },

    //コピーされたチップのステータスアイコンを取得する
    getCopyStatusIcons: function getCopyStatusIcons() {
        //追加作業起票中
        if (this.addWorkStatus == C_AW_ADDINGWORK) {
            return '<div class="ICAddWork"></div>';
        } else if (this.addWorkStatus == C_AW_WAIT_COMMITTED) {
            //追加作業承認待ち
            return '<div class="ICWaitCommited"></div>';
        }

        //入庫の場合
        if (IsDefaultDate(this.rsltSvcInDateTime) == false) {
            return '<div class="ICCarIn"></div>';
        }
        return "";
    },

    getIcon: function getIcon() {
        var nCount = 0;
        var strRt = "";
        //入庫の場合
        if (IsDefaultDate(this.rsltSvcInDateTime) == false) {
            nCount += 1;
        }
        //着工指示の場合(C_STALLUSE_STATUS_WORKORDERWAITが着工指示待ち、それで、着工指示待ち以後のステータスは全部着工済と認識する)
        if ((this.stallUseStatus >= C_STALLUSE_STATUS_STARTWAIT) && (this.stallUseStatus <= C_STALLUSE_STATUS_MIDFINISH)) {
            nCount += 2;
        }
        //部品準備完了の場合
        if (this.partsFlg == "1") {
            nCount += 4;
        }

        switch (nCount) {
            case 1:
                strRt = '<div class="ICCarIn"></div>';
                break;
            case 2:
                strRt = '<div class="ICRo"></div>';
                break;
            case 3:
                strRt = '<div class="ICCarinRo"></div>';
                break;
            case 4:
                strRt = '<div class="ICIssue"></div>';
                break;
            case 5:
                strRt = '<div class="ICCarinIssue"></div>';
                break;
            case 6:
                strRt = '<div class="ICRoIssue"></div>';
                break;
            case 7:
                strRt = '<div class="ICAll"></div>';
                break;
        }
        return strRt;
    },

    //時間または日付を取得
    getDateOrTime: function getDateOrTime() {
        var strBandHtml = "";
        var dtShowDate = new Date($("#hidShowDate").val());
        //実績納車時間が入っている場合は実績納車時間を表示する
        //2014/12/04 TMEJ 丁 新プラットフォーム版i-CROP仕様変更対応（サービス入庫実績更新処理変更）START
        //if (IsDefaultDate(this.rsltDeliDateTime) == false) {
        if (this.svcStatus == C_SVCSTATUS_DELIVERY) {
            //2014/12/04 TMEJ 丁 新プラットフォーム版i-CROP仕様変更対応（サービス入庫実績更新処理変更）END

            //実績納車時間が当日の場合は「HH:MM」、当日以外の場合は「MM/DD」
            if ((this.rsltDeliDateTime.getFullYear() == dtShowDate.getFullYear())
                && (this.rsltDeliDateTime.getMonth() == dtShowDate.getMonth())
                && (this.rsltDeliDateTime.getDate() == dtShowDate.getDate())) {
                // 2013/11/29 TMEJ 張 タブレット版SMB チーフテクニシャン機能開発 START
                //                strBandHtml = add_zero(this.rsltDeliDateTime.getHours()) + ":" + add_zero(this.rsltDeliDateTime.getMinutes());
                strBandHtml = DateFormat(this.rsltDeliDateTime, gDateFormatHHmm);
                // 2013/11/29 TMEJ 張 タブレット版SMB チーフテクニシャン機能開発 END
            } else {
                // 2013/11/29 TMEJ 張 タブレット版SMB チーフテクニシャン機能開発 START
                //                strBandHtml = add_zero(this.rsltDeliDateTime.getMonth() + 1) + "/" + add_zero(this.rsltDeliDateTime.getDate());
                strBandHtml = DateFormat(this.rsltDeliDateTime, gDateFormatMMdd);
                // 2013/11/29 TMEJ 張 タブレット版SMB チーフテクニシャン機能開発 END
            }
        } else if (IsDefaultDate(this.scheDeliDateTime) == false) {
            //納車予定が当日の場合は「HH:MM」、当日以外の場合は「MM/DD」
            if ((this.scheDeliDateTime.getFullYear() == dtShowDate.getFullYear())
                && (this.scheDeliDateTime.getMonth() == dtShowDate.getMonth())
                && (this.scheDeliDateTime.getDate() == dtShowDate.getDate())) {
                // 2013/11/29 TMEJ 張 タブレット版SMB チーフテクニシャン機能開発 START
                //                strBandHtml = add_zero(this.scheDeliDateTime.getHours()) + ":" + add_zero(this.scheDeliDateTime.getMinutes());
                strBandHtml = DateFormat(this.scheDeliDateTime, gDateFormatHHmm);
                // 2013/11/29 TMEJ 張 タブレット版SMB チーフテクニシャン機能開発 END
            } else {
                // 2013/11/29 TMEJ 張 タブレット版SMB チーフテクニシャン機能開発 START
                //                strBandHtml = add_zero(this.scheDeliDateTime.getMonth() + 1) + "/" + add_zero(this.scheDeliDateTime.getDate());
                strBandHtml = DateFormat(this.scheDeliDateTime, gDateFormatMMdd);
                // 2013/11/29 TMEJ 張 タブレット版SMB チーフテクニシャン機能開発 END
            }
        }
        return strBandHtml;
    },

    //整備内容の表示取得
    getSvcName: function getSvcName() {
        //商品マーク上部表示文字列と商品マーク下部表示文字列があれば
        var strSvcName = "";

        //2015/04/01 TMEJ 小澤 BTS-261対応 サービス名の表示制御の修正 START
        //        if ((this.upperDisp) && (this.lowerDisp)) {
        //            if ((this.upperDisp != "") && (this.lowerDisp != "")) {
        //                strSvcName = this.upperDisp + this.lowerDisp;
        //            }
        //        }

        //上部文言、下部文言どちらかのデータが存在する場合
        if ((this.upperDisp) || (this.lowerDisp)) {
            if ((this.upperDisp != "") || (this.lowerDisp != "")) {
                strSvcName = this.upperDisp + this.lowerDisp;
            }
        }

        //2015/04/01 TMEJ 小澤 BTS-261対応 サービス名の表示制御の修正 END

        //設定してない場合、svcClassNameで設定する
        if (strSvcName == "") {
            if ((this.svcClassName) && (this.svcClassName != "")) {
                strSvcName = this.svcClassName;
            } else {
                if (this.svcClassNameEng) {
                    strSvcName = this.svcClassNameEng;
                }
            }
        }
        return strSvcName;
    },

    //遅れ見込み時刻
    setPlanDelayDate: function setPlanDelayDate(aPlanDelayDate) {
        try {
            if (aPlanDelayDate) {
                this.planDelayDate = new Date(aPlanDelayDate);
            } else {
                this.planDelayDate = new Date(C_DATE_DEFAULT_VALUE);
            }
        }
        catch (e) {
            this.planDelayDate = new Date(C_DATE_DEFAULT_VALUE);
        }
    },

    /**
    * 表示開始日時に値を格納する.
    *
    * @param {String} aCwRsltStartDateTime 表示開始日時
    * @return {void}
    *
    */
    setDisplayStartDate: function setDisplayStartDate(aDisplayStartDate) {
        try {
            if (aDisplayStartDate) {
                this.displayStartDate = new Date(aDisplayStartDate);
            }
        }
        catch (e) {
            this.displayStartDate = new Date(C_DATE_DEFAULT_VALUE);
        }
    },
    /**
    * 表示終了日時に値を格納する.
    *
    * @param {String} aDisplayEndDate 表示終了日時
    * @return {void}
    *
    */
    setDisplayEndDate: function setDisplayEndDate(aDisplayEndDate) {
        try {
            if (aDisplayEndDate) {
                this.displayEndDate = new Date(aDisplayEndDate);
            }
        }
        catch (e) {
            this.displayEndDate = new Date(C_DATE_DEFAULT_VALUE);
        }
    },

    //チップ表示用の開始、終了日時
    setDisplayDate: function setDisplayDate() {
        try {
            // 実績開始時間があれば、チップの表示時間が実績開始時間に設定
            if (IsDefaultDate(this.rsltStartDateTime) == false) {
                this.displayStartDate.setTime(this.rsltStartDateTime.getTime());
                if (IsDefaultDate(this.rsltEndDateTime) == false) {
                    this.displayEndDate.setTime(this.rsltEndDateTime.getTime());
                } else {
                    if (IsDefaultDate(this.prmsEndDateTime) == false) {
                        this.displayEndDate.setTime(this.prmsEndDateTime.getTime());
                    } else {
                        this.displayEndDate = new Date();
                        this.displayEndDate.setTime(this.rsltStartDateTime.getTime() + (this.scheDeliDateTime * 60 * 1000));
                    }
                }
            } else {
                // 実績時間がない場合、予定時間が表示時間
                this.displayStartDate.setTime(this.scheStartDateTime.getTime());
                this.displayEndDate.setTime(this.scheEndDateTime.getTime());
            }

            // 見込み終了時間が今日終われない場合、displayEndDateが今日の営業終了時間で設定する
            var dtShowDate = new Date($("#hidShowDate").val());
            // 終了日付が当ページの日付と違う場合
            if ((dtShowDate.getFullYear() != this.displayEndDate.getFullYear())
                    || (dtShowDate.getMonth() != this.displayEndDate.getMonth())
                    || (dtShowDate.getDate() != this.displayEndDate.getDate())) {
                if (dtShowDate - this.displayEndDate > 0) {
                    return;
                }
                // 終了日付が当ページの後の場合、当画面にこのチップが営業終了時間まで表示する
                this.displayEndDate.setTime(gEndWorkTime.getTime());
            }

            // 開始日付が当ページの日付と違う場合
            if ((dtShowDate.getFullYear() != this.displayStartDate.getFullYear())
                    || (dtShowDate.getMonth() != this.displayStartDate.getMonth())
                    || (dtShowDate.getDate() != this.displayStartDate.getDate())) {
                if (dtShowDate - this.displayStartDate < 0) {
                    return;
                }
                // 開始日付が当ページの前場合、当画面にこのチップが営業開始時間から表示する
                this.displayStartDate.setTime(gStartWorkTime.getTime());
            }
        }
        catch (e) {
            this.displayStartDate = new Date(C_DATE_DEFAULT_VALUE);
            this.displayEndDate = new Date(C_DATE_DEFAULT_VALUE);
        }
    },

    //追加作業起票申請状態
    setAddWorkStatus: function setAddWorkStatus(aAddWorkStatus) {
        if (aAddWorkStatus.toString().Trim() != "") {
            this.addWorkStatus = aAddWorkStatus;
        }
    },

    //RO番号
    setRoNum: function setRoNum(aRoNum) {
        try {
            if (aRoNum) {
                this.roNum = aRoNum.toString().Trim();
            }
        }
        catch (e) {
            this.roNum = "";
        }
    },


    //新DB対応 START
    /**
    * サービス入庫IDに値を格納する.
    *
    * @param {String} aSvcInId サービス入庫ID
    * @return {void}
    *
    */
    setSvcInId: function setSvcInId(aSvcInId) {
        try {
            if (aSvcInId) {
                this.svcInId = aSvcInId.toString().Trim();
            }
        }
        catch (e) {
            this.svcInId = "";
        }
    },
    /**
    * 販売店コードに値を格納する.
    *
    * @param {String} aDlrCd 販売店コード
    * @return {void}
    *
    */
    setDlrCd: function setDlrCd(aDlrCd) {
        try {
            if (aDlrCd) {
                this.dlrCd = aDlrCd.toString().Trim();
            }
        }
        catch (e) {
            this.dlrCd = "";
        }
    },
    /**
    * 店舗コードに値を格納する.
    *
    * @param {String} aBrnCd 店舗コード
    * @return {void}
    *
    */
    setBrnCd: function setBrnCd(aBrnCd) {
        try {
            if (aBrnCd) {
                this.brnCd = aBrnCd.toString().Trim();
            }
        }
        catch (e) {
            this.brnCd = "";
        }
    },
    /**
    * 顧客IDに値を格納する.
    *
    * @param {Integer} aCstId 顧客ID
    * @return {void}
    *
    */
    setCstId: function setCstId(aCstId) {
        try {
            if (aCstId) {
                this.cstId = aCstId.toString().Trim();
            }
        }
        catch (e) {
            this.cstId = "";
        }
    },
    /**
    * 車両IDに値を格納する.
    *
    * @param {Integer} aVclId 車両ID
    * @return {void}
    *
    */
    setVclId: function setVclId(aVclId) {
        try {
            if (aVclId) {
                this.vclId = aVclId.toString().Trim();
            }
        }
        catch (e) {
            this.vclId = "";
        }
    },
    /**
    * 顧客車両区分に値を格納する.
    *
    * @param {{String}} aCstVclType 顧客車両区分
    * @return {void}
    *
    */
    setCstVclType: function setCstVclType(aCstVclType) {
        try {
            if (aCstVclType) {
                this.cstVclType = aCstVclType.toString().Trim();
            }
        }
        catch (e) {
            this.cstVclType = "";
        }
    },
    /**
    * テレマ契約フラグに値を格納する.
    *
    * @param {String} aTlmContractFlg テレマ契約フラグ
    * @return {void}
    *
    */
    setTlmContractFlg: function setTlmContractFlg(aTlmContractFlg) {
        try {
            if (aTlmContractFlg) {
                this.tlmContractFlg = aTlmContractFlg.toString().Trim();
            }
        }
        catch (e) {
            this.tlmContractFlg = "";
        }
    },
    /**
    * 受付区分に値を格納する.
    *
    * @param {String} aAcceptanceTpye 受付区分
    * @return {void}
    *
    */
    setAcceptanceTpye: function setAcceptanceTpye(aAcceptanceTpye) {
        try {
            if (aAcceptanceTpye) {
                this.acceptanceTpye = aAcceptanceTpye.toString().Trim();
            }
        }
        catch (e) {
            this.acceptanceTpye = "";
        }
    },
    /**
    * 引取納車区分に値を格納する.
    *
    * @param {String} aPickDeliType 引取納車区分
    * @return {void}
    *
    */
    setPickDeliType: function setPickDeliType(aPickDeliType) {
        try {
            if (aPickDeliType) {
                this.pickDeliType = aPickDeliType.toString().Trim();
            }
        }
        catch (e) {
            this.pickDeliType = "";
        }
    },
    /**
    * 洗車必要フラグに値を格納する.
    *
    * @param {String} aCarWashNeedFlg 洗車必要フラグ
    * @return {void}
    *
    */
    setCarWashNeedFlg: function setCarWashNeedFlg(aCarWashNeedFlg) {
        try {
            if (aCarWashNeedFlg) {
                this.carWashNeedFlg = aCarWashNeedFlg.toString().Trim();
            }
        }
        catch (e) {
            this.carWashNeedFlg = "";
        }
    },
    /**
    * 予約ステータスに値を格納する.
    *
    * @param {String} aResvStatus 予約ステータス
    * @return {void}
    *
    */
    setResvStatus: function setResvStatus(aResvStatus) {
        try {
            if (aResvStatus) {
                this.resvStatus = aResvStatus.toString().Trim();
            }
        }
        catch (e) {
            this.resvStatus = "";
        }
    },
    /**
    * サービスステータスに値を格納する.
    *
    * @param {String} aSvcStatus サービスステータス
    * @return {void}
    *
    */
    setSvcStatus: function setSvcStatus(aSvcStatus) {
        try {
            if (aSvcStatus) {
                this.svcStatus = aSvcStatus.toString().Trim();
            }
        }
        catch (e) {
            this.svcStatus = "";
        }
    },
    /**
    * 予定入庫日時に値を格納する.
    *
    * @param {String} aScheSvcInDateTime 予定入庫日時
    * @return {void}
    *
    */
    setScheSvcInDateTime: function setScheSvcInDateTime(aScheSvcInDateTime) {
        try {
            if (aScheSvcInDateTime) {
                this.scheSvcInDateTime = new Date(aScheSvcInDateTime);
            }
        }
        catch (e) {
            this.scheSvcInDateTime = new Date(C_DATE_DEFAULT_VALUE);
        }
    },
    /**
    * 予定納車日時に値を格納する.
    *
    * @param {String} aScheDeliDateTime 予定納車日時
    * @return {void}
    *
    */
    setScheDeliDateTime: function setScheDeliDateTime(aScheDeliDateTime) {
        try {
            if (aScheDeliDateTime) {
                this.scheDeliDateTime = new Date(aScheDeliDateTime);
            }
        }
        catch (e) {
            this.scheDeliDateTime = new Date(C_DATE_DEFAULT_VALUE);
        }
    },
    /**
    * 実績入庫日時に値を格納する.
    *
    * @param {String} aRsltSvcInDateTime 実績入庫日時
    * @return {void}
    *
    */
    setRsltSvcInDateTime: function setRsltSvcInDateTime(aRsltSvcInDateTime) {
        try {
            if (aRsltSvcInDateTime) {
                this.rsltSvcInDateTime = new Date(aRsltSvcInDateTime);
            }
        }
        catch (e) {
            this.rsltSvcInDateTime = new Date(C_DATE_DEFAULT_VALUE);
        }
    },
    /**
    * 実績納車日時に値を格納する.
    *
    * @param {String} aRsltDeliDateTime 実績納車日時
    * @return {void}
    *
    */
    setRsltDeliDateTime: function setRsltDeliDateTime(aRsltDeliDateTime) {
        try {
            if (aRsltDeliDateTime) {
                this.rsltDeliDateTime = new Date(aRsltDeliDateTime);
            }
        }
        catch (e) {
            this.rsltDeliDateTime = new Date(C_DATE_DEFAULT_VALUE);
        }
    },
    /**
    * 行更新日時に値を格納する.
    *
    * @param {String} aRowUpdateDateTime 行更新日時
    * @return {void}
    *
    */
    setRowUpdateDateTime: function setRowUpdateDateTime(aRowUpdateDateTime) {
        try {
            if (aRowUpdateDateTime) {
                this.rowUpdateDateTime = new Date(aRowUpdateDateTime);
            }
        }
        catch (e) {
            this.rowUpdateDateTime = new Date(C_DATE_DEFAULT_VALUE);
        }
    },
    /**
    * 行ロックバージョンに値を格納する.
    *
    * @param {String} aRowLockVersion 行ロックバージョン
    * @return {void}
    *
    */
    setRowLockVersion: function setRowLockVersion(aRowLockVersion) {
        try {
            if (aRowLockVersion) {
                this.rowLockVersion = parseInt(aRowLockVersion);
            }
        }
        catch (e) {
            this.rowLockVersion = 0;
        }
    },
    /**
    * 作業内容IDに値を格納する.
    *
    * @param {String} aJobDtlId 作業内容ID
    * @return {void}
    *
    */
    setJobDtlId: function setJobDtlId(aJobDtlId) {
        try {
            if (aJobDtlId) {
                this.jobDtlId = aJobDtlId.toString().Trim();
            }
        }
        catch (e) {
            this.jobDtlId = "";
        }
    },
    /**
    * 検査必要フラグに値を格納する.
    *
    * @param {String} aInspectionNeedFlg 検査必要フラグ
    * @return {void}
    *
    */
    setInspectionNeedFlg: function setInspectionNeedFlg(aInspectionNeedFlg) {
        try {
            if (aInspectionNeedFlg) {
                this.inspectionNeedFlg = aInspectionNeedFlg.toString().Trim();
            }
        }
        catch (e) {
            this.inspectionNeedFlg = "";
        }
    },
    /**
    * 検査承認待ちフラグに値を格納する.
    *
    * @param {String} aInspectionApprovalFlg 検査必要フラグ
    * @return {void}
    *
    */
    setInspectionApprovalFlg: function setInspectionApprovalFlg(aInspectionApprovalFlg) {
        try {
            if (aInspectionApprovalFlg) {
                this.inspectionApprovalFlg = aInspectionApprovalFlg.toString().Trim();
            }
        }
        catch (e) {
            this.inspectionApprovalFlg = "";
        }
    },
    /**
    * キャンセルフラグに値を格納する.
    *
    * @param {String} aCancelFlg キャンセルフラグ
    * @return {void}
    *
    */
    setCancelFlg: function setCancelFlg(aCancelFlg) {
        try {
            if (aCancelFlg) {
                this.cancelFlg = aCancelFlg.toString().Trim();
            }
        }
        catch (e) {
            this.cancelFlg = "";
        }
    },
    /**
    * ストール利用IDに値を格納する.
    *
    * @param {String} aStallUseId ストール利用ID
    * @return {void}
    *
    */
    setStallUseId: function setStallUseId(aStallUseId) {
        try {
            if (aStallUseId) {
                this.stallUseId = aStallUseId.toString().Trim();
            }
        }
        catch (e) {
            this.stallUseId = "";
        }
    },
    /**
    * ストールIDに値を格納する.
    *
    * @param {String} aStallId ストールID
    * @return {void}
    *
    */
    setStallId: function setStallId(aStallId) {
        try {
            if (aStallId) {
                this.stallId = aStallId.toString().Trim();
            }
        }
        catch (e) {
            this.stallId = "";
        }
    },
    /**
    * 仮置きフラグに値を格納する.
    *
    * @param {String} aTempFlg 仮置きフラグ
    * @return {void}
    *
    */
    setTempFlg: function setTempFlg(aTempFlg) {
        try {
            if (aTempFlg) {
                this.tempFlg = aTempFlg.toString().Trim();
            }
        }
        catch (e) {
            this.tempFlg = "";
        }
    },
    /**
    * 部品準備完了フラグに値を格納する.
    *
    * @param {String} aTempFlg 仮置きフラグ
    * @return {void}
    *
    */
    setPartsFlg: function setPartsFlg(aPartsFlg) {
        try {
            // 2013/11/29 TMEJ 張 タブレット版SMB チーフテクニシャン機能開発 START
            //            if (aPartsFlg) {
            // 部品ステータスが初期表示の時取得して、後操作する時、取得してない
            // ほかの操作もう部品準備ステータスを空白値で更新する
            if (aPartsFlg.toString().Trim() != "") {
                // 2013/11/29 TMEJ 張 タブレット版SMB チーフテクニシャン機能開発 END
                this.partsFlg = aPartsFlg.toString().Trim();
            }
        }
        catch (e) {
            this.partsFlg = "";
        }
    },
    /**
    * ストール利用ステータスに値を格納する.
    *
    * @param {String} aStallUseStatus ストール利用ステータス
    * @return {void}
    *
    */
    setStallUseStatus: function setStallUseStatus(aStallUseStatus) {
        try {
            if (aStallUseStatus) {
                this.stallUseStatus = aStallUseStatus.toString().Trim();
            }
        }
        catch (e) {
            this.stallUseStatus = "";
        }
    },
    //2018/07/06 NSK 可児 TKM Next Gen e-CRB Project Application development Block B-1  Pマーク、Lマークなどを表示 START
    /**
    * P/Lマークのフラグを格納する.
    *
    * @param {String} aImpVclFlg IMP_VCLフラグ 
    * @return {void}
    *
    */
    setImpVclFlg: function setImpVclFlg(aImpVclFlg) {
        try {
            if (aImpVclFlg) {
                this.impVclFlg = aImpVclFlg.toString().Trim();
            }
        }
        catch (e) {
            this.impVclFlg = "";
        }
    },
    //2018/07/06 NSK 可児 TKM Next Gen e-CRB Project Application development Block B-1  Pマーク、Lマークなどを表示 END
    /**
    * 予定開始日時に値を格納する.
    *
    * @param {String} aScheStartDateTime 予定開始日時
    * @return {void}
    *
    */
    setScheStartDateTime: function setScheStartDateTime(aScheStartDateTime) {
        try {
            if (aScheStartDateTime) {
                this.scheStartDateTime = new Date(aScheStartDateTime);
            }
        }
        catch (e) {
            this.scheStartDateTime = new Date(C_DATE_DEFAULT_VALUE);
        }
    },
    /**
    * 予定終了日時に値を格納する.
    *
    * @param {String} aScheEndDateTime 予定終了日時
    * @return {void}
    *
    */
    setScheEndDateTime: function setScheEndDateTime(aScheEndDateTime) {
        try {
            if (aScheEndDateTime) {
                this.scheEndDateTime = new Date(aScheEndDateTime);
            }
        }
        catch (e) {
            this.scheEndDateTime = new Date(C_DATE_DEFAULT_VALUE);
        }
    },
    /**
    * 予定作業時間に値を格納する.
    *
    * @param {String} aScheWorkTime 予定作業時間
    * @return {void}
    *
    */
    setScheWorkTime: function setScheWorkTime(aScheWorkTime) {
        try {
            if (aScheWorkTime) {
                this.scheWorkTime = parseInt(aScheWorkTime);
            }
        }
        catch (e) {
            this.scheWorkTime = 0;
        }
    },
    /**
    * 休憩取得フラグに値を格納する.
    *
    * @param {String} aRestFlg 休憩取得フラグ
    * @return {void}
    *
    */
    setRestFlg: function setRestFlg(aRestFlg) {
        try {
            if (aRestFlg) {
                this.restFlg = aRestFlg.toString().Trim();
            }
        }
        catch (e) {
            this.restFlg = "";
        }
    },
    /**
    * 実績開始日時に値を格納する.
    *
    * @param {String} aRsltStartDateTime 実績開始日時
    * @return {void}
    *
    */
    setRsltStartDateTime: function setRsltStartDateTime(aRsltStartDateTime) {
        try {
            if (aRsltStartDateTime) {
                this.rsltStartDateTime = new Date(aRsltStartDateTime);
            }
        }
        catch (e) {
            this.rsltStartDateTime = new Date(C_DATE_DEFAULT_VALUE);
        }
    },
    /**
    * 見込終了日時に値を格納する.
    *
    * @param {String} aPrmsEndDateTime 見込終了日時
    * @return {void}
    *
    */
    setPrmsEndDateTime: function setPrmsEndDateTime(aPrmsEndDateTime) {
        try {
            if (aPrmsEndDateTime) {
                this.prmsEndDateTime = new Date(aPrmsEndDateTime);
            }
        }
        catch (e) {
            this.prmsEndDateTime = new Date(C_DATE_DEFAULT_VALUE);
        }
    },
    /**
    * 実績終了日時に値を格納する.
    *
    * @param {String} aRsltEndDateTime 実績終了日時
    * @return {void}
    *
    */
    setRsltEndDateTime: function setRsltEndDateTime(aRsltEndDateTime) {
        try {
            if (aRsltEndDateTime) {
                this.rsltEndDateTime = new Date(aRsltEndDateTime);
            }
        }
        catch (e) {
            this.rsltEndDateTime = new Date(C_DATE_DEFAULT_VALUE);
        }
    },
    /**
    * 実績作業時間に値を格納する.
    *
    * @param {String} aRsltWorkTime 実績作業時間
    * @return {void}
    *
    */
    setRsltWorkTime: function setRsltWorkTime(aRsltWorkTime) {
        try {
            if (aRsltWorkTime) {
                this.rsltWorkTime = parseInt(aRsltWorkTime);
                if (this.rsltWorkTime < 0 || isNaN(this.rsltWorkTime)) {
                    this.rsltWorkTime = 0;
                }
            } else {
                this.rsltWorkTime = 0;
            }
        }
        catch (e) {
            this.rsltWorkTime = 0;
        }
    },
    /**
    * 中断理由区分に値を格納する.
    *
    * @param {String} aStopReasonType 中断理由区分
    * @return {void}
    *
    */
    setStopReasonType: function setStopReasonType(aStopReasonType) {
        try {
            if (aStopReasonType) {
                this.stopReasonType = aStopReasonType.toString().Trim();
            }
        }
        catch (e) {
            this.stopReasonType = "";
        }
    },
    /**
    * VINに値を格納する.
    *
    * @param {String} aVclVin VIN
    * @return {void}
    *
    */
    setVclVin: function setVclVin(aVclVin) {
        try {
            if (aVclVin) {
                this.vclVin = aVclVin.toString().Trim();
            }
        }
        catch (e) {
            this.vclVin = "";
        }
    },
    /**
    * モデル名に値を格納する.
    *
    * @param {String} aModelName モデル名
    * @return {void}
    *
    */
    setModelName: function setModelName(aModelName) {
        try {
            if (aModelName) {
                this.modelName = aModelName.toString().Trim();
            }
        }
        catch (e) {
            this.modelName = "";
        }
    },
    /**
    * 車両登録番号に値を格納する.
    *
    * @param {String} aRegNum 車両登録番号
    * @return {void}
    *
    */
    setRegNum: function setRegNum(aRegNum) {
        try {
            if (aRegNum) {
                this.regNum = aRegNum.toString().Trim();
            }
        }
        catch (e) {
            this.regNum = "";
        }
    },
    /**
    * 洗車実績IDに値を格納する.
    *
    * @param {String} aCarWashRsltId 洗車実績ID
    * @return {void}
    *
    */
    setCarWashRsltId: function setCarWashRsltId(aCarWashRsltId) {
        try {
            if (aCarWashRsltId) {
                this.carWashRsltId = aCarWashRsltId.toString().Trim();
            }
        }
        catch (e) {
            this.carWashRsltId = "";
        }
    },
    /**
    * 実績開始日時に値を格納する.
    *
    * @param {String} aCwRsltStartDateTime 実績開始日時
    * @return {void}
    *
    */
    setCwRsltStartDateTime: function setCwRsltStartDateTime(aCwRsltStartDateTime) {
        try {
            if (aCwRsltStartDateTime) {
                this.cwRsltStartDateTime = new Date(aCwRsltStartDateTime);
            }
        }
        catch (e) {
            this.cwRsltStartDateTime = new Date(C_DATE_DEFAULT_VALUE);
        }
    },
    /**
    * 実績終了日時に値を格納する.
    *
    * @param {String} aCwRsltEndDateTime 実績終了日時
    * @return {void}
    *
    */
    setCwRsltEndDateTime: function setCwRsltEndDateTime(aCwRsltEndDateTime) {
        try {
            if (aCwRsltEndDateTime) {
                this.cwRsltEndDateTime = new Date(aCwRsltEndDateTime);
            }
        }
        catch (e) {
            this.cwRsltEndDateTime = new Date(C_DATE_DEFAULT_VALUE);
        }
    },
    /**
    * サービス分類名称に値を格納する.
    *
    * @param {String} aSvcClassName サービス分類名称
    * @return {void}
    *
    */
    setSvcClassName: function setSvcClassName(aSvcClassName) {
        try {
            if (aSvcClassName) {
                this.svcClassName = aSvcClassName.toString().Trim();
            }
        }
        catch (e) {
            this.svcClassName = "";
        }
    },
    /**
    * サービス分類名称（英語）に値を格納する.
    *
    * @param {String} aSvcClassNameEng サービス分類名称（英語）
    * @return {void}
    *
    */
    setSvcClassNameEng: function setSvcClassNameEng(aSvcClassNameEng) {
        try {
            if (aSvcClassNameEng) {
                this.svcClassNameEng = aSvcClassNameEng.toString().Trim();
            }
        }
        catch (e) {
            this.svcClassNameEng = "";
        }
    },
    /**
    * 商品マーク上部表示文字列に値を格納する.
    *
    * @param {String} aUpperDisp 商品マーク上部表示文字列
    * @return {void}
    *
    */
    setUpperDisp: function setUpperDisp(aUpperDisp) {
        try {
            if (aUpperDisp) {
                this.upperDisp = aUpperDisp.toString().Trim();
            }
        }
        catch (e) {
            this.upperDisp = "";
        }
    },
    /**
    * 商品マーク下部表示文字列に値を格納する.
    *
    * @param {String} aLowerDisp 商品マーク下部表示文字列
    * @return {void}
    *
    */
    setLowerDisp: function setLowerDisp(aLowerDisp) {
        try {
            if (aLowerDisp) {
                this.lowerDisp = aLowerDisp.toString().Trim();
            }
        }
        catch (e) {
            this.lowerDisp = "";
        }
    },
    /**
    * 顧客承認連番に値を格納する.
    *
    * @param {Integer} aRoJobSeq 顧客承認連番
    * @return {void}
    *
    */
    setRoJobSeq: function setRoJobSeq(aRoJobSeq) {
        try {
            if (aRoJobSeq) {
                this.roJobSeq = parseInt(aRoJobSeq);
            }
        }
        catch (e) {
            this.roJobSeq = -1;
        }
    },
    /**
    * 中断フラグに値を格納する.
    *
    * @param {String} aStopFlg 中断フラグ
    * @return {void}
    *
    */
    setStopFlg: function setStopFlg(aStopFlg) {
        this.stopFlg = aStopFlg;
    },

    // 2017/10/19 NSK 竹中(悠) REQ-SVT-TMT-20170227-002 見込納車遅れ日時計算ロジック統一 START
    /**
    * 残完成検査区分を格納する.
    *
    * @param {String} aRemainingInspectionType 残完成検査区分
    * @return {void}
    *
    */
    setRemainingInspectionType: function setRemainingInspectionType(aRemainingInspectionType) {
        try {
            if (aRemainingInspectionType) {
                this.remainingInspectionType = aRemainingInspectionType.toString();
            }
        }
        catch (e) {
            this.remainingInspectionType = "";
        }
    }
    // 2017/10/19 NSK 竹中(悠) REQ-SVT-TMT-20170227-002 見込納車遅れ日時計算ロジック統一 END
}

//関連チップ構造体
function RelationChip(aStallUseId) {
    /**
    * サービス入庫ID
    * @return {Long}
    */
    this.svcinId = "";

    // 2017/10/21 NSK 小川 REQ-SVT-TMT-20160906-003 子チップがキャンセルできない STRAT
    /**
    * 作業内容ID
    * @return {Long}
    */
    this.jobDtlId = "";
    // 2017/10/21 NSK 小川 REQ-SVT-TMT-20160906-003 子チップがキャンセルできない END

    /**
    * ストール利用ID
    * @return {Long}
    */
    this.stallUseId = aStallUseId;

    /**
    * 開始日時
    * @return {Date}
    */
    this.startDateTime;

}
RelationChip.prototype = {

    //予約チップクラスのメンバ変数にデータベースから取得した値を格納
    //@param {DataSet} aDataSet データベースより取得した値
    //@return {void}
    setChipParameter: function setChipParameter(aDataSet) {
        this.setSvcinId(aDataSet.SVCIN_ID);                         // サービス入庫ID

        // 2017/10/21 NSK 小川 REQ-SVT-TMT-20160906-003 子チップがキャンセルできない STRAT
        this.setJobDtlId(aDataSet.JOB_DTL_ID);                      //作業内容ID
        // 2017/10/21 NSK 小川 REQ-SVT-TMT-20160906-003 子チップがキャンセルできない END

        this.setStallUseId(aDataSet.STALL_USE_ID);                  // ストール利用ID
        this.setStartDateTime(aDataSet.START_DATETIME);             // 開始時間
    },
    /**
    * サービス入庫IDに値を格納する.
    *
    * @param {String} aSvcinId サービス入庫ID
    * @return {void}
    *
    */
    setSvcinId: function setSvcinId(aSvcinId) {
        try {
            if (aSvcinId) {
                this.svcinId = aSvcinId.toString().Trim();
            }
        }
        catch (e) {
            this.svcinId = "";
        }
    },

    // 2017/10/21 NSK 小川 REQ-SVT-TMT-20160906-003 子チップがキャンセルできない STRAT
    /**
    * 作業内容Idに値を格納する.
    *
    * @param {String} aJobDtlId 開始日時
    * @return {void}
    *
    */
    setJobDtlId: function setJobDtlId(aJobDtlId) {
        try {
            if (aJobDtlId) {
                this.jobDtlId = aJobDtlId.toString().Trim();
            }
        }
        catch (e) {
            this.jobDtlId = "";
        }
    },
    // 2017/10/21 NSK 小川 REQ-SVT-TMT-20160906-003 子チップがキャンセルできない END

    /**
    * ストール利用IDに値を格納する.
    * @param {String} aStallUseId ストール利用ID
    * @return {void}
    */
    setStallUseId: function setStallUseId(aStallUseId) {
        try {
            if (aStallUseId) {
                this.stallUseId = aStallUseId.toString().Trim();
            }
        }
        catch (e) {
            this.stallUseId = "";
        }
    },
    /**
    * 開始日時に値を格納する.
    *
    * @param {String} aStartDateTime 開始日時
    * @return {void}
    *
    */
    setStartDateTime: function setStartDateTime(aStartDateTime) {
        try {
            if (aStartDateTime) {
                this.startDateTime = new Date(aStartDateTime);
            }
        }
        catch (e) {
            this.startDateTime = new Date(C_DATE_DEFAULT_VALUE);
        }
    }
}

//関連チップ構造体
function UnavailableChip(aStallUseId) {

    // 非稼働ストールID
    this.stallIdleId = "";
    // ストールID
    this.stallId = "";
    // 幅
    this.width = 0;
    // 行ロックバージョン
    this.rowLockVersion = 0;
}

UnavailableChip.prototype = {
    //予約チップクラスのメンバ変数にデータベースから取得した値を格納
    //@param {DataSet} aDataSet データベースより取得した値
    //@return {void}
    setChipParameter: function setChipParameter(aDataSet) {
        this.setStallIdleId(aDataSet.STALLIDLEID);          // 非稼働ストールID
        this.setStallId(aDataSet.STALLID);                  // ストールID
        this.setWidth(aDataSet.WIDTH);                      // 幅
        this.setRowLockVersion(aDataSet.ROWLOCKVERSION);    // 行ロックバージョン
        //2017/09/16 NSK 竹中(悠) REQ-SVT-TMT-20161109-001 SMB iPadにストールクローズ機能の追加 START
        this.setIdleMemo(aDataSet.IDLEMEMO);                // 非稼働メモ
        //2017/09/16 NSK 竹中(悠) REQ-SVT-TMT-20161109-001 SMB iPadにストールクローズ機能の追加 END
    },
    /**
    * 非稼働ストールIDに値を格納する.
    *
    * @param {String} aSvcinId サービス入庫ID
    * @return {void}
    *
    */
    setStallIdleId: function setStallIdleId(aStallIdleId) {
        try {
            if (aStallIdleId) {
                this.stallIdleId = aStallIdleId.toString().Trim();
            }
        }
        catch (e) {
            this.stallIdleId = "";
        }
    },
    /**
    * ストールIDに値を格納する.
    * @param {String} aStallId ストールID
    * @return {void}
    */
    setStallId: function setStallId(aStallId) {
        try {
            if (aStallId) {
                this.stallId = aStallId.toString().Trim();
            }
        }
        catch (e) {
            this.stallId = "";
        }
    },
    /**
    * 幅
    * @param {String} aWidth 幅
    * @return {void}
    */
    setWidth: function setWidth(aWidth) {
        try {
            if (aWidth) {
                this.width = parseInt(aWidth);
            }
        }
        catch (e) {
            this.width = 0;
        }
    },
    /**
    * 行ロックバージョン
    * @param {String} aRowLockVersion 行ロックバージョン
    * @return {void}
    */
    setRowLockVersion: function setRowLockVersion(aRowLockVersion) {
        try {
            if (aRowLockVersion) {
                this.rowLockVersion = parseInt(aRowLockVersion);
            }
        }
        catch (e) {
            this.rowLockVersion = 0;
        }
    },

    //2017/09/16 NSK 竹中(悠) REQ-SVT-TMT-20161109-001 SMB iPadにストールクローズ機能の追加 START
    /**
    * 非稼働メモ
    * @param {String} aIdleMemo 非稼働メモ
    * @return {void}
    */
    setIdleMemo: function setIdleMemo(aIdleMemo) {
        try {
            if (aIdleMemo) {
                this.idleMemo = aIdleMemo.toString().Trim();
            }
        }
        catch (e) {
            this.rowLockVersion = "";
        }
    },
    //2017/09/16 NSK 竹中(悠) REQ-SVT-TMT-20161109-001 SMB iPadにストールクローズ機能の追加 END

    //ストールチップを生成する
    //@param {String} nChipType チップタイプ
    //@return {void}
    createChip: function createChip(nChipType) {
        //2017/09/16 NSK 竹中(悠) REQ-SVT-TMT-20161109-001 SMB iPadにストールクローズ機能の追加 START
        //        var objUnvaliable = CreateUnavailableChip(C_UNAVALIABLECHIPID + this.stallIdleId);
        var idleMemo = " ";
        if (this.idleMemo != undefined) {
            idleMemo = this.idleMemo;
        }
        var objUnvaliable = CreateUnavailableChip(C_UNAVALIABLECHIPID + this.stallIdleId, idleMemo);
        //2017/09/16 NSK 竹中(悠) REQ-SVT-TMT-20161109-001 SMB iPadにストールクローズ機能の追加 END
        // 日跨ぎ移動以外の場合位置を設定する
        if (nChipType == C_CHIPTYPE_OTHER_DAY) {
            $("#ulStall").append(objUnvaliable);
        }
    },

    //全てデータをjsonのstringを出力
    //@param {-}
    //@return {String}
    toJsonSting: function toJsonSting() {
        var jsonData = '{'
        jsonData += '"STALLIDLEID":"' + transferNullToBlank(this.stallIdleId) + '"';
        jsonData += ',"STALLID":"' + transferNullToBlank(this.stallId) + '"';
        jsonData += ',"WIDTH":"' + transferNullToBlank(this.width) + '"';
        jsonData += ',"ROWLOCKVERSION":"' + transferNullToBlank(this.rowLockVersion) + '"';

        //2017/09/16 NSK 竹中(悠) REQ-SVT-TMT-20161109-001 SMB iPadにストールクローズ機能の追加 START
        jsonData += ',"IDLEMEMO":"' + transferNullToBlank(this.idleMemo) + '"';
        //2017/09/16 NSK 竹中(悠) REQ-SVT-TMT-20161109-001 SMB iPadにストールクローズ機能の追加 END

        jsonData += '}';
        return jsonData;
    }
}
