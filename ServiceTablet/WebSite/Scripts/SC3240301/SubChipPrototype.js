//---------------------------------------------------------
//SC31504.SubChipPrototype.js
//---------------------------------------------------------
//機能：SMBサブチップクラス
//作成：2013/01/18 TMEJ 丁 タブレット版SMB機能開発(工程管理)
//更新：2013/11/29 TMEJ 丁 タブレット版SMB チーフテクニシャン機能開発
//更新：2014/06/18 TMEJ 丁 タブレット版SMB テレマ走行距離機能開発
//更新：2015/02/13 TMEJ 範 新プラットフォーム版i-CROP仕様変更対応 (SMBチップ検索及び作業中断機能強化)
//更新：2015/04/01 TMEJ 小澤 BTS-261対応 サービス名の表示制御の修正
//更新：2017/09/05 NSK 小川 REQ-SVT-TMT-20160906-003 計画済みチップをストールから簡単に外せるようにする
//更新：2017/09/25 NSK 竹中(悠) REQ-SVT-TMT-20170227-002 見込納車遅れ日時計算ロジック統一
//更新：2018/07/09 NSK 可児 TKM Next Gen e-CRB Project Application development Block B-1  Pマーク、Lマークなどを表示
//
//---------------------------------------------------------

//ストール予約チップクラス
//サブチップの生成まで、実際の配置は呼び出したクラスで行うものとする
/**
* サブチップ情報を格納し、チップを生成配置する.
*
* @class チップ情報の格納・生成クラス.
* サブチップの情報を所持し、それらを取り扱う機能を保有する.
*
* @param {String} aKEY チップID（プライマリー）
*/
function ReserveSubChip(aKEY) {
    /**
    * KEY
    * @return {String}
    */
    this.KEY = aKEY;

    /**
    * 開始日時（チップ表示用）
    * @return {Date}
    */
    this.displayStartDate = new Date(C_DATE_DEFAULT_VALUE);

    /**
    * 終了日時（チップ表示用)
    * @return {Date}
    */
    this.displayEndDate = new Date(C_DATE_DEFAULT_VALUE)

    /**
    * 追加作業起票申請状態
    * @param {String}
    */
    this.addWorkStatus = "";

    /**
    * サブチップエリアid
    * @return {String}
    */
    this.subChipAreaId = "";

    /**
    * 遅刻フラグ
    * @return {String}
    */
    this.delayStatus = C_NO_DELAY;
    /**
    * 枝番
    * @return {Integer}
    */
    this.srvAddSeq = 0;

    /**
    * 遅れ見込み時刻
    * @return {Date}
    */
    this.planDelayDate = new Date(C_DATE_DEFAULT_VALUE);

    //新DB対応　START
    /**
    * サービス入庫ID
    * @return {String}
    */
    this.svcInId = "";

    /**
    * 販売店コード
    * @return {String}
    */
    this.dlrCd = "";

    /**
    * 店舗コード
    * @return {String}
    */
    this.brnCd = "";

    /**
    * RO番号
    * @return {String}
    */
    this.roNum = "";

    /**
    * 顧客ID
    * @return {String}
    */
    this.cstId = "";

    /**
    * 車両ID
    * @return {String}
    */
    this.vclId = "";

    /**
    * 顧客車両区分
    * @return {String}
    */
    this.cstVclType = "";

    /**
    * テレマ契約フラグ
    * @return {String}
    */
    this.tlmContractFlg = "";

    /**
    * 受付区分
    * @return {String}
    */
    this.acceptanceTpye = "";

    /**
    * 引取納車区分
    * @return {String}
    */
    this.pickDeliType = "";

    /**
    * 洗車必要フラグ
    * @return {String}
    */
    this.carWashNeedFlg = "";

    /**
    * 予約ステータス
    * @return {String}
    */
    this.resvStatus = "";

    /**
    * サービスステータス
    * @return {String}
    */
    this.svcStatus = "";

    /**
    * 予定入庫日時
    * @return {Date}
    */
    this.scheSvcInDateTime = new Date(C_DATE_DEFAULT_VALUE);

    /**
    * 予定納車日時
    * @return {Date}
    */
    this.scheDeliDateTime = new Date(C_DATE_DEFAULT_VALUE);

    /**
    * 予定納車日時(親チップ)
    * @return {Date}
    */
    this.parentsScheDeliDateTime = new Date(C_DATE_DEFAULT_VALUE);

    /**
    * 実績入庫日時
    * @return {Date}
    */
    this.rsltSvcInDateTime = new Date(C_DATE_DEFAULT_VALUE);

    /**
    * 実績納車日時
    * @return {Date}
    */
    this.rsltDeliDateTime = new Date(C_DATE_DEFAULT_VALUE);

    /**
    * 行更新日時 
    * @return {Date}
    */
    this.rowUpdateDateTime = new Date(C_DATE_DEFAULT_VALUE);

    /**
    * 行ロックバージョン
    * @return {Integer}
    */
    this.rowLockVersion = 0;

    /**
    * 作業内容ID
    * @return {String}
    */
    this.jobDtlId = "";

    /**
    * 検査必要フラグ
    * @return {String}
    */
    this.inspectionNeedFlg = "";

    /**
    * 検査承認待ちフラグ
    * @return {String}
    */
    this.inspectionApprovalFlg = "";

    /**
    * キャンセルフラグ
    * @return {String}
    */
    this.cancelFlg = "";

    /**
    * ストール利用ID
    * @return {String}
    */
    this.stallUseId = "";

    /**
    * ストールID
    * @return {String}
    */
    this.stallId = "";

    /**
    * 仮置きフラグ
    * @return {String}
    */
    this.tempFlg = "";

    /**
    * ストール利用ステータス
    * @return {String}
    */
    this.stallUseStatus = "";

    /**
    * 予定開始日時
    * @return {Date}
    */
    this.scheStartDateTime = new Date(C_DATE_DEFAULT_VALUE);

    /**
    * 予定終了日時
    * @return {Date}
    */
    this.scheEndDateTime = new Date(C_DATE_DEFAULT_VALUE);

    /**
    * 予定作業時間
    * @return {Integer}
    */
    this.scheWorkTime = 0;

    /**
    * 休憩取得フラグ
    * @return {String}
    */
    this.restFlg = "";

    /**
    * 実績開始日時
    * @return {Date}
    */
    this.rsltStartDateTime = new Date(C_DATE_DEFAULT_VALUE);

    /**
    * 見込終了日時
    * @return {Date}
    */
    this.prmsEndDateTime = new Date(C_DATE_DEFAULT_VALUE);

    /**
    * 実績終了日時
    * @return {Date}
    */
    this.rsltEndDateTime = new Date(C_DATE_DEFAULT_VALUE);

    /**
    * 実績作業時間
    * @return {Integer}
    */
    this.rsltWorkTime = 0;

    /**
    * 中断理由区分
    * @return {String}
    */
    this.stopReasonType = "";

    /**
    * VIN 
    * @return {String}
    */
    this.vclVin = "";

    /**
    * モデル名
    * @return {String}
    */
    this.modelName = "";

    /**
    * 車両登録番号
    * @return {String}
    */
    this.regNum = "";

    /**
    * 洗車実績ID
    * @return {String}
    */
    this.carWashRsltId = "";

    /**
    * 実績開始日時
    * @return {Date}
    */
    this.cwRsltStartDateTime = new Date(C_DATE_DEFAULT_VALUE);

    /**
    * 実績終了日時
    * @return {Date}
    */
    this.cwRsltEndDateTime = new Date(C_DATE_DEFAULT_VALUE);

    /**
    * サービス分類名称
    * @return {String}
    */
    this.svcClassName = "";

    /**
    * サービス分類名称（英語）
    * @return {String}
    */
    this.svcClassNameEng = "";

    /**
    * 商品マーク上部表示文字列
    * @return {String}
    */
    this.upperDisp = "";

    /**
    * 商品マーク下部表示文字列
    * @return {String}
    */
    this.lowerDisp = "";

    /**
    *再配置所要時間
    *@return {Integer}
    */
    this.relocationWorkTime = 0;

    /**
    *作業連番
    *@return {Integer}
    */
    this.roJobSeq = -1;

    /**
    *顧客承認日時 
    *@return {Date}
    */
    this.custConfirmDate = new Date(C_DATE_DEFAULT_VALUE);
    /**
    *部品準備フラグ
    *@return {String}
    */
    this.partsFlg = "";

    /**
    *整備コード
    *@return {String}
    */
    this.mntnCd = "";

    /**
    *整備コード
    *@return {String}
    */
    this.mercId = "";

    /**
    *サービス分類ID
    *@return {String}
    */
    this.svcClassId = "";

    /**
    *受付チップの親チップのワークシーケンス 
    *@return {Integer}
    */
    this.parentsRoJobSeq = -1;

    //2013/11/29 TMEJ 丁 タブレット版SMB チーフテクニシャン機能開発 START
    /**
    *基幹作業内容ID
    *@return {String}
    */
    this.dmsJobDtlId = "";

    /**
    *訪問ID
    *@return {String}
    */
    this.visitId = "";
    //2013/11/29 TMEJ 丁 タブレット版SMB チーフテクニシャン機能開発 END

    //2015/02/13 TMEJ 範 新プラットフォーム版i-CROP仕様変更対応 (SMBチップ検索及び作業中断機能強化) START
    /**
    *未完了作業件数
    *@return {Integer}
    */
    this.notFinishedCount = 0;
    //2015/02/13 TMEJ 範 新プラットフォーム版i-CROP仕様変更対応 (SMBチップ検索及び作業中断機能強化) END

    // 2017/09/25 NSK 竹中(悠) REQ-SVT-TMT-20170227-002 見込納車遅れ日時計算ロジック統一 START
    /**
    *残完成検査区分
    *@return {Integer}
    */
    this.remainingInspectionType = "";
    // 2017/09/25 NSK 竹中(悠) REQ-SVT-TMT-20170227-002 見込納車遅れ日時計算ロジック統一 END

    //2018/07/09 NSK 可児 TKM Next Gen e-CRB Project Application development Block B-1  Pマーク、Lマークなどを表示 START
    /**
    *重要車両フラグ
    *@return {String}
    */
    this.impVclFlg = "";
    //2018/07/09 NSK 可児 TKM Next Gen e-CRB Project Application development Block B-1  Pマーク、Lマークなどを表示 END
}

ReserveSubChip.prototype = {
    /**
    * 予約チップクラスのメンバ変数にデータベースから取得した値を格納する.
    *
    * @param {DataSet} aDataSet データベースより取得した値
    * @return {void}
    *
    */
    setSubChipParameter: function setSubChipParameter(aDataSet, SubChipAreaId) {

        this.setPlanDelayDate(aDataSet.PLAN_DELAYDATE);             // 遅れ見込み時刻
        this.setSubChipAreaId(SubChipAreaId);                   // サブチップエリアid

        this.setSvcInId(aDataSet.SVCIN_ID);                          // サービス入庫ID
        this.setDlrCd(aDataSet.DLR_CD);                          // 販売店コード
        this.setBrnCd(aDataSet.BRN_CD);                          // 店舗コード
        this.setRoNum(aDataSet.RO_NUM);                          // RO番号
        this.setCstId(aDataSet.CST_ID);                          // 顧客ID
        this.setVclId(aDataSet.VCL_ID);                          // 車両ID
        this.setCstVclType(aDataSet.CST_VCL_TYPE);                          // 顧客車両区分
        this.setTlmContractFlg(aDataSet.TLM_CONTRACT_FLG);                          // テレマ契約フラグ
        this.setAcceptanceTpye(aDataSet.ACCEPTANCE_TYPE);                          // 受付区分
        this.setPickDeliType(aDataSet.PICK_DELI_TYPE);                          // 引取納車区分
        this.setCarWashNeedFlg(aDataSet.CARWASH_NEED_FLG);                          // 洗車必要フラグ
        this.setResvStatus(aDataSet.RESV_STATUS);                          // 予約ステータス
        this.setSvcStatus(aDataSet.SVC_STATUS);                          // サービスステータス
        this.setScheSvcInDateTime(aDataSet.SCHE_SVCIN_DATETIME);                          // 予定入庫日時
        this.setScheDeliDateTime(aDataSet.SCHE_DELI_DATETIME);                          // 予定納車日時  
        this.setRsltSvcInDateTime(aDataSet.RSLT_SVCIN_DATETIME);                          // 実績入庫日時
        this.setRsltDeliDateTime(aDataSet.RSLT_DELI_DATETIME);                          // 実績納車日時
        this.setRowUpdateDateTime(aDataSet.ROW_UPDATE_DATETIME);                          // 行更新日時
        this.setRowLockVersion(aDataSet.ROW_LOCK_VERSION);                          // 行ロックバージョン
        this.setJobDtlId(aDataSet.JOB_DTL_ID);                          // 作業内容ID
        this.setInspectionNeedFlg(aDataSet.INSPECTION_NEED_FLG);                          // 検査必要フラグ
        this.setInspectionApprovalFlg(aDataSet.INSPECTION_STATUS);    // 検査承認待ちフラグ
        this.setCancelFlg(aDataSet.CANCEL_FLG);                          // キャンセルフラグ
        this.setStallUseId(aDataSet.STALL_USE_ID);                          // ストール利用ID
        this.setStallId(aDataSet.STALL_ID);                          // ストールID
        this.setTempFlg(aDataSet.TEMP_FLG);                          // 仮置きフラグ
        this.setStallUseStatus(aDataSet.STALL_USE_STATUS);                          // ストール利用ステータス
        this.setScheStartDateTime(aDataSet.SCHE_START_DATETIME);                          // 予定開始日時
        this.setScheEndDateTime(aDataSet.SCHE_END_DATETIME);                          // 予定終了日時
        this.setScheWorkTime(aDataSet.SCHE_WORKTIME);                          // 予定作業時間
        this.setRestFlg(aDataSet.REST_FLG);                          //休憩取得フラグ
        this.setRsltStartDateTime(aDataSet.RSLT_START_DATETIME);                          // 実績開始日時
        this.setPrmsEndDateTime(aDataSet.PRMS_END_DATETIME);                          // 見込終了日時
        this.setRsltEndDateTime(aDataSet.RSLT_END_DATETIME);                          // 実績終了日時
        this.setRsltWorkTime(aDataSet.RSLT_WORKTIME);                          // 実績作業時間
        this.setStopReasonType(aDataSet.STOP_REASON_TYPE);                          // 中断理由区分
        this.setVclVin(aDataSet.VCL_VIN);                          // VIN 
        this.setModelName(aDataSet.MODEL_NAME);                          // モデル名 
        this.setRegNum(aDataSet.REG_NUM);                          // 車両登録番号
        this.setCarWashRsltId(aDataSet.CARWASH_RSLT_ID);                          // 洗車実績ID
        this.setCwRsltStartDateTime(aDataSet.CW_RSLT_START_DATETIME);                          // 実績開始日時
        this.setCwRsltEndDateTime(aDataSet.CW_RSLT_END_DATETIME);                          // 実績終了日時
        this.setSvcClassName(aDataSet.SVC_CLASS_NAME);                          // サービス分類名称
        this.setSvcClassNameEng(aDataSet.SVC_CLASS_NAME_ENG);                          // サービス分類名称（英語）
        this.setUpperDisp(aDataSet.UPPER_DISP);                          // 商品マーク上部表示文字列
        this.setLowerDisp(aDataSet.LOWER_DISP);                          // 商品マーク下部表示文字列
        this.setRoJobSeq(aDataSet.RO_JOB_SEQ)           //顧客承認連番(WORKSEQ)
        this.setSrvAddSeq(aDataSet.SRVADDSEQ);                  // 枝番
        this.setCustConfirmDate(aDataSet.CUST_CONFIRMDATE)      //顧客承認日時 
        this.setPartsFlg(aDataSet.PARTS_FLG)      //部品準備フラグ 
        this.setAddWorkStatus(aDataSet.ADD_WORKSTATUS);          // 追加作業起票申請状態
        this.setMntnCd(aDataSet.MNTNCD);          // 整備コード
        this.setMercId(aDataSet.MERC_ID);          // 商品ID
        this.setSvcClassId(aDataSet.SVC_CLASS_ID);          // サービス分類ID
        //2018/07/09 NSK 可児 TKM Next Gen e-CRB Project Application development Block B-1  Pマーク、Lマークなどを表示 START
        this.setImpVclFlg(aDataSet.IMP_VCL_FLG);
        //2018/07/09 NSK 可児 TKM Next Gen e-CRB Project Application development Block B-1  Pマーク、Lマークなどを表示 END
        if (SubChipAreaId == C_RECEPTION) {
            this.setParentsJobSeq(aDataSet.PARENTS_RO_JOB_SEQ);          // 親チップのワークシーケンス（受付エリア専用）
            this.setParentsScheDeliDateTime(aDataSet.PARENTS_SCHE_DELI_DATETIME);
        }

        if (SubChipAreaId == C_STOP) {
            this.setRelocationWorkTime(this.scheWorkTime, this.rsltWorkTime, this.stopReasonType);     //再配置時間
        }
        this.setDisplayDate();     // チップ表示用の開始、終了日時

        //2013/11/29 TMEJ 丁 タブレット版SMB チーフテクニシャン機能開発 START
        this.setDmsJobDtlId(aDataSet.DMS_JOB_DTL_ID);     // 基幹作業内容ID
        this.setVisitId(aDataSet.VISIT_ID);     // 訪問ID
        //2013/11/29 TMEJ 丁 タブレット版SMB チーフテクニシャン機能開発 END

        //2015/02/13 TMEJ 範 新プラットフォーム版i-CROP仕様変更対応 (SMBチップ検索及び作業中断機能強化) START
        this.setNotFinishedCount(aDataSet.NOT_FINISHED_COUNT);  // 未完了作業件数
        //2015/02/13 TMEJ 範 新プラットフォーム版i-CROP仕様変更対応 (SMBチップ検索及び作業中断機能強化) END

        // 2017/09/25 NSK 竹中(悠) REQ-SVT-TMT-20170227-002 見込納車遅れ日時計算ロジック統一 START
        this.setRemainingInspectionType(aDataSet.REMAINING_INSPECTION_TYPE);  // 残完成検査区分
        // 2017/09/25 NSK 竹中(悠) REQ-SVT-TMT-20170227-002 見込納車遅れ日時計算ロジック統一 END

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
        // 2017/09/25 NSK 竹中(悠) REQ-SVT-TMT-20170227-002 見込納車遅れ日時計算ロジック統一 START
        objDestChip.remainingInspectionType = this.remainingInspectionType;
        // 2017/09/25 NSK 竹中(悠) REQ-SVT-TMT-20170227-002 見込納車遅れ日時計算ロジック統一 END
        //2018/07/09 NSK 可児 TKM Next Gen e-CRB Project Application development Block B-1  Pマーク、Lマークなどを表示 START
        objDestChip.impVclFlg = this.impVclFlg;
        //2018/07/09 NSK 可児 TKM Next Gen e-CRB Project Application development Block B-1  Pマーク、Lマークなどを表示 END
    },
    /**
    * 生成するチップ種別を設定（併せて、タップの有効フラグを設定する）
    *
    * @return {void}
    *
    */
    getChipColor: function getChipColor() {
        //色ノード
        var strColor = "";

        switch (this.subChipAreaId) {
            case "100":

                //2017/09/05 NSK 小川 REQ-SVT-TMT-20160906-003 計画済みチップをストールから簡単に外せるようにする START
                if (this.resvStatus == C_RTYPE_TEMP) {
                    //仮予約：水青
                    strColor = "StTRez";
                } else {
                    //本予約：青
                    strColor = "StRez";
                }
                break;
            //2017/09/05 NSK 小川 REQ-SVT-TMT-20160906-003 計画済みチップをストールから簡単に外せるようにする END        

            case "500":
            case "700":
                //受付 納車待ちエリア：青い
                strColor = "StRez";
                break;
            case "400":
                if (this.svcStatus == "07") {
                    //洗車エリア　洗車待ち：青い
                    strColor = "StRez";
                } else if (this.svcStatus == "08") {
                    //洗車エリア　洗車中：緑
                    strColor = "StWork";
                }
                break;
            case "200":
            case "300":
                // 追加作業　完成検査エリア ：緑
                strColor = "StWork";
                break;
            case "600":
                if (this.resvStatus == C_RTYPE_TEMP) {
                    //仮予約：水青
                    strColor = "StTRez";
                } else {
                    //本予約：青
                    strColor = "StRez";
                }
                break;
            default:
                strColor = "StTRez";
                break;
        }
        return strColor;
    },
    /**
    * チップの赤色を更新する
    * @return {void}
    */
    refleshChipRedColor: function refleshChipRedColor() {
        // 赤いがあるdivクラス
        var strChipColorClass = "#" + this.KEY;
        // リセットする
        $(strChipColorClass).removeClass("StDelay StPDelay")

        // 遅刻を判断する
        this.checkSubChipLater();
        //遅刻のクラス
        var strDelayClass = "";

        if (this.delayStatus == C_DELAY) {  //終了時間（実績）を超える
            strDelayClass = " StDelay";
        } else if (this.delayStatus == C_DELAY_PROSPECTS) { //終了時間（予定）を超える           
            strDelayClass = " StPDelay";
        }
        $(strChipColorClass).addClass(strDelayClass);
    },
    /**
    * チップが遅刻するかどうかをチェックする
    *
    * @return {void}
    *
    */
    checkSubChipLater: function checkSubChipLater() {
        this.delayStatus = C_NO_DELAY;  //初期化

        //納車時間がないまたは納車完了チップのdelayStatusがC_NO_DELAY
        if (IsDefaultDate(this.scheDeliDateTime) == false) {

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
            if (nNow > nDeliTime) {
                // 実際遅刻の場合
                this.delayStatus = C_DELAY;
                gstrLateflg = gstrLateflg + 1;
            } else if ((nNow >= nPlanDelayDate) && (IsDefaultDate(this.planDelayDate) == false)) {
                // 見込み遅刻の場合
                this.delayStatus = C_DELAY_PROSPECTS;
                gstrLateflg = gstrLateflg + 1;
            } else if (nNow == nDeliTime) {
                // nNow == nDeliTimeの場合、サーバ側が遅れ見込み計算してない
                this.delayStatus = C_DELAY_PROSPECTS;
                gstrLateflg = gstrLateflg + 1;
            }
        }
    },
    /**
    * Bandエリアにアイコンを決定する
    *
    * @return {void}
    *
    */
    getBandIcons: function getBandIcons() {
        var strBandHtml = "";
        // VIPマーク Demoでは対象外
        var bVip = false;        //TODO
        if (bVip) {
            strBandHtml += '<div class="IC01"><p>V</p></div>';
        }
        // 店内の場合
        if (this.pickDeliType == C_WAIT_IN) {
            strBandHtml += '<div class="IC02"></div>';
        }
        // 予約客の場合
        if (this.acceptanceTpye == C_RFLG_RESERVE) {
            strBandHtml += '<div class="IC03"></div>';
        }
        //2018/07/09 NSK 可児 TKM Next Gen e-CRB Project Application development Block B-1  Pマーク、Lマークなどを表示 START
        if (this.impVclFlg == C_ICON_FLAG_P) {
            if (this.acceptanceTpye == C_RFLG_RESERVE) {
                strBandHtml += '<div class="IconP"></div>';
            } else {
                strBandHtml += '<div class="IconP" style="right:4px;"></div>';
            }
        } else if (this.impVclFlg == C_ICON_FLAG_L) {
            if (this.acceptanceTpye == C_RFLG_RESERVE) {
                strBandHtml += '<div class="IconL"></div>';
            } else {
                strBandHtml += '<div class="IconL" style="right:4px;"></div>';
            }
        }

        //2018/07/09 NSK 可児 TKM Next Gen e-CRB Project Application development Block B-1  Pマーク、Lマークなどを表示 END

        return strBandHtml;
    },

    /**
    * サブチップステータスアイコンを取得する
    *
    * @return {void}
    *
    */
    getSubChipStatusIcons: function getSubChipStatusIcons() {
        //追加作業起票中
        if (this.addWorkStatus == C_AW_ADDINGWORK) {
            return '<div class="ICAddWork"></div>';
        } else if (this.addWorkStatus == C_AW_WAIT_COMMITTED) {
            //追加作業承認待ち
            return '<div class="ICWaitCommited"></div>';
        }
        //追加作業起票中、追加作業承認待ち 下記のアイコンを表示できる
        return this.getIcon();
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
        if (IsDefaultDate(this.scheDeliDateTime) == false) {
            //納車予定が当日の場合は「HH:MM」、当日以外の場合は「MM/DD」
            if ((this.scheDeliDateTime.getFullYear() == dtShowDate.getFullYear())
                && (this.scheDeliDateTime.getMonth() == dtShowDate.getMonth())
                && (this.scheDeliDateTime.getDate() == dtShowDate.getDate())) {
                // 2013/11/29 TMEJ 丁 タブレット版SMB チーフテクニシャン機能開発 START
                //strBandHtml = add_zero(this.scheDeliDateTime.getHours()) + ":" + add_zero(this.scheDeliDateTime.getMinutes());
                strBandHtml = DateFormat(this.scheDeliDateTime, gDateFormatHHmm);
                // 2013/11/29 TMEJ 丁 タブレット版SMB チーフテクニシャン機能開発 END
            } else {
                // 2013/11/29 TMEJ 丁 タブレット版SMB チーフテクニシャン機能開発 START
                //strBandHtml = add_zero(this.scheDeliDateTime.getMonth() + 1) + "/" + add_zero(this.scheDeliDateTime.getDate());
                strBandHtml = DateFormat(this.scheDeliDateTime, gDateFormatMMdd);
                // 2013/11/29 TMEJ 丁 タブレット版SMB チーフテクニシャン機能開発 END
            }
        }
        return strBandHtml;
    },

    /**
    * チップ表示用の開始日時
    *
    * @param {String} aDisplayStartDate チップ表示用の開始日時
    */
    setDisplayStartDate: function setDisplayStartDate(aDisplayStartDate) {
        try {
            if (aDisplayStartDate) {
                this.displayStartDate = new Date(aDisplayStartDate);
            }
        }
        catch (e) {
            this.displayStartDate = null;
        }
    },
    /**
    * チップ表示用の終了日時
    *
    * @param {String} aDisplayEndDate チップ表示用の終了日時
    */
    setDisplayEndDate: function setDisplayEndDate(aDisplayEndDate) {
        try {
            if (aDisplayEndDate) {
                this.displayEndDate = new Date(aDisplayEndDate);
            }
        }
        catch (e) {
            this.displayEndDate = null;
        }
    },
    /**
    * 追加作業起票申請状態
    * @param {String} aAddWorkStatus
    */
    setAddWorkStatus: function setAddWorkStatus(aAddWorkStatus) {
        if (aAddWorkStatus) {
            this.addWorkStatus = aAddWorkStatus;
        }
    },
    /*
    * サブチップエリアid
    * @param {Integer} setSubChipAreaId サブチップエリアid
    */
    setSubChipAreaId: function setSubChipAreaId(aSubChipAreaId) {
        if (aSubChipAreaId) {
            this.subChipAreaId = aSubChipAreaId;
        }
    },
    /*
    * 枝番
    * @param {Integer} setSRVADDSEQ 枝番
    */
    setSrvAddSeq: function setSrvAddSeq(aSrvAddSeq) {
        if (aSrvAddSeq) {
            this.srvAddSeq = aSrvAddSeq;
        }
    },
    /*
    * キー
    * @param {string} setKEY キー
    */
    setKEY: function setKEY(aKEY) {
        if (aKEY) {
            this.KEY = aKEY;
        }
    },

    /* 遅れ見込み時刻
    * @param {String} aPlanDelayDate 遅れ見込み時刻
    *
    */
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
                this.svcInId = aSvcInId;
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
    *  RO番号に値を格納する.
    *
    * @param {String} aRoNum RO番号
    * @return {void}
    *
    */
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
    /**
    *  整備コードに値を格納する.
    *
    * @param {String} aMntnCd 整備コード
    * @return {void}
    *
    */
    setMntnCd: function setMntnCd(aMntnCd) {
        try {
            if (aMntnCd) {
                this.mntnCd = aMntnCd.toString().Trim();
            }
        }
        catch (e) {
            this.mntnCd = "";
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
                this.cstId = aCstId;
            }
        }
        catch (e) {
            this.cstId = "";
        }
    },
    /**
    * 商品IDに値を格納する.
    *
    * @param {Integer} aMercId 商品ID
    * @return {void}
    *
    */
    setMercId: function setMercId(aMercId) {
        try {
            if (aMercId) {
                this.mercId = aMercId;
            }
        }
        catch (e) {
            this.mercId = "";
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
                this.vclId = aVclId;
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
    * 予定納車日時(親チップ)に値を格納する.
    *
    * @param {String} aParentsScheDeliDateTime 予定納車日時(親チップ)
    * @return {void}
    *
    */
    setParentsScheDeliDateTime: function setParentsScheDeliDateTime(aParentsScheDeliDateTime) {
        try {
            if (aParentsScheDeliDateTime) {
                this.parentsScheDeliDateTime = new Date(aParentsScheDeliDateTime);
            }
        }
        catch (e) {
            this.parentsScheDeliDateTime = new Date(C_DATE_DEFAULT_VALUE);
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
                this.jobDtlId = aJobDtlId;
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
    * 顧客承認日時 に値を格納する.
    *
    * @param {String} aCustConfirmDate 顧客承認日時 
    * @return {void}
    *
    */
    setCustConfirmDate: function setCustConfirmDate(aCustConfirmDate) {
        try {
            if (aCustConfirmDate) {
                this.custConfirmDate = new Date(aCustConfirmDate);
            }
        }
        catch (e) {
            this.custConfirmDate = new Date(C_DATE_DEFAULT_VALUE);
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
                this.stallUseId = aStallUseId;
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
                this.stallId = aStallId;
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
                this.carWashRsltId = aCarWashRsltId;
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
    * 検査実績IDに値を格納する.
    *
    * @param {Integer} aSvcClassId サービス分類ID
    * @return {void}
    *
    */
    setSvcClassId: function setSvcClassId(aSvcClassId) {
        try {
            if (aSvcClassId) {
                this.svcClassId = aSvcClassId;
            }
        }
        catch (e) {
            this.svcClassId = "";
        }
    },
    /**
    * スタッフコードに値を格納する.
    *
    * @param {String} aStfCd スタッフコード
    * @return {void}
    *
    */
    setStfCd: function setStfCd(aStfCd) {
        try {
            if (aStfCd) {
                this.stfCd = aStfCd.toString().Trim();
            }
        }
        catch (e) {
            this.stfCd = "";
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
                this.roJobSeq = aRoJobSeq;
            }
        }
        catch (e) {
            this.roJobSeq = -1;
        }
    },

    /**
    * 親チップ顧客承認連番に値を格納する.
    *
    * @param {Integer} aParentsJobSeq 親チップ顧客承認連番
    * @return {void}
    *
    */
    setParentsJobSeq: function setParentsJobSeq(aParentsJobSeq) {
        try {
            if (aParentsJobSeq) {
                this.parentsRoJobSeq = aParentsJobSeq;
            }
        }
        catch (e) {
            this.parentsRoJobSeq = -1;
        }
    },

    /*
    * 部品準備フラグ
    * @param {String} aPartsFlg
    */
    setPartsFlg: function setPartsFlg(aPartsFlg) {
        try {
            if (aPartsFlg) {
                this.partsFlg = aPartsFlg;
            }
        }
        catch (e) {
            this.partsFlg = "";
        }
    },
    //2013/11/29 TMEJ 丁 タブレット版SMB チーフテクニシャン機能開発 START

    /*
    * 基幹作業内容ID
    * @param {String} aDmsJobDtlId
    */
    setDmsJobDtlId: function setDmsJobDtlId(aDmsJobDtlId) {
        try {
            if (aDmsJobDtlId) {
                this.dmsJobDtlId = aDmsJobDtlId;
            }
        }
        catch (e) {
            this.dmsJobDtlId = "";
        }
    },

    /*
    * 訪問ID
    * @param {Long} aVisitId
    */
    setVisitId: function setVisitId(aVisitId) {
        try {
            if (aVisitId) {
                this.visitId = aVisitId;
            }
        }
        catch (e) {
            this.visitId = "";
        }
    },
    //2013/11/29 TMEJ 丁 タブレット版SMB チーフテクニシャン機能開発 END

    //2018/07/09 NSK 可児 TKM Next Gen e-CRB Project Application development Block B-1  Pマーク、Lマークなどを表示 START
    /**
    * 重要車両フラグに値を格納する.
    *
    * @param {Integer} aImpVclFlg 重要車両フラグ
    * @return {void}
    *
    */
    setImpVclFlg: function setImpVclFlg(aImpVclFlg) {
        try {
            if (aImpVclFlg) {
                this.impVclFlg = aImpVclFlg;
            }
        }
        catch (e) {
            this.impVclFlg = "";
        }
    },
    //2018/07/09 NSK 可児 TKM Next Gen e-CRB Project Application development Block B-1  Pマーク、Lマークなどを表示 END
    //2015/02/13 TMEJ 範 新プラットフォーム版i-CROP仕様変更対応 (SMBチップ検索及び作業中断機能強化) START
    /*
    * 未完了作業件数
    * @param {Integer} aNotFinishedCount
    */
    setNotFinishedCount: function setNotFinishedCount(aNotFinishedCount) {
        try {
            if (aNotFinishedCount) {
                this.notFinishedCount = aNotFinishedCount;
            }
        }
        catch (e) {
            this.notFinishedCount = -1;
        }
    },
    //2015/02/13 TMEJ 範 新プラットフォーム版i-CROP仕様変更対応 (SMBチップ検索及び作業中断機能強化) END

    // 2017/09/25 NSK 竹中(悠) REQ-SVT-TMT-20170227-002 見込納車遅れ日時計算ロジック統一 START
    /*
    * 残完成検査区分
    * @param {Integer} aRemainingInspectionType
    */
    setRemainingInspectionType: function setRemainingInspectionType(aRemainingInspectionType) {
        try {
            if (aRemainingInspectionType) {
                this.remainingInspectionType = aRemainingInspectionType;
            }
        }
        catch (e) {
            this.remainingInspectionType = "";
        }
    },
    // 2017/09/25 NSK 竹中(悠) REQ-SVT-TMT-20170227-002 見込納車遅れ日時計算ロジック統一 END


    /**
    * 予定作業時間と実績作業時間から再配置所要時間を算出します。
    * @param {Integer} scheWorkTime 予定作業時間
    * @param {Integer} rsltWorkTime 実績作業時間
    * @param {String} stopReasonType 中断理由区分
    * @return {void}
    */
    setRelocationWorkTime: function setRelocationWorkTime(scheWorkTime, rsltWorkTime, stopReasonType) {
        try {
            if (stopReasonType == "") {
                this.relocationWorkTime = 0;
                return;
            }
            if (stopReasonType == C_STOPREASON_INSPECTIONFAILURE) {
                // 予定作業時間を返却する。
                this.relocationWorkTime = scheWorkTime;
                return;
            }
            if (rsltWorkTime < scheWorkTime) {
                var diffTime = scheWorkTime - rsltWorkTime;
                var count = Math.floor(diffTime / gResizeInterval);
                var surplus = diffTime % gResizeInterval;
                if (surplus == 0) {
                    this.relocationWorkTime = diffTime;
                } else {
                    this.relocationWorkTime = count * gResizeInterval + gResizeInterval;
                }
            } else {
                this.relocationWorkTime = scheWorkTime;
            }
        }
        catch (e) {
            this.relocationWorkTime = 0;
        }
    },

    /** 
    * サブチップを生成する
    *
    * @return {void}
    *
    */
    createSubChip: function createSubChip(strTargetArea, strKey) {

        // 一時データ
        var strData = "";
        // サブチップID
        var strSubchipId = strKey;

        // チップの枠
        var objChip = $("<div />").addClass("SCp");
        objChip.attr("id", strSubchipId);
        // チップの色を決定するdiv
        var strColor = this.getChipColor();
        objChip.addClass(strColor);
        // 横の枠
        var objBand = $("<div />").addClass("Band");
        objBand.append(this.getBandIcons());
        objChip.append(objBand);

        // タイトル文字
        // 車名
        strData = "";
        if (this.modelName) {
            strData = this.modelName;
        }

        objChip.append("<h3>" + strData + "</h3>");

        // 車番号
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

        // 時間
        var strDateTime = this.getDateOrTime();
        if (strDateTime != "") {
            var objTime = $("<div />").addClass("time");
            objTime.append(strDateTime);
            objChip.append(objTime);
        }

        // 下のアイコン
        objChip.append(this.getSubChipStatusIcons());

        //整備内容
        strData = "";
        strData = this.getSvcName();
        if (strData.Trim() != "") {
            var objInfo = $("<div />").addClass("infoBox");
            if (strData != "") {
                objInfo.append(strData);
            }
            objChip.append(objInfo);
        }
        // 一番前のdiv(タップ用)
        var objFrontFace = $("<div />").addClass("Front");
        objChip.append(objFrontFace);
        if (strTargetArea == C_CHIPTYPE_OTHER_DAY) {
            // C_CHIPTYPE_OTHER_DAYのチップはストール名称エリアに作成する
            $("#ulStall").append(objChip);
        } else {
            // チップをチップアリアに追加する
            $("." + strTargetArea + " .SubChipArea").append(objChip);
        }

        // 遅刻色を更新する
        this.refleshChipRedColor();
    },
    //整備内容の表示取得
    getSvcName: function getSvcName() {
        //商品マーク上部表示文字列と商品マーク下部表示文字列があれば
        var strSvcName = "";
        if (this.subChipAreaId == C_ADDITIONALWORK) {
            strSvcName = "";
        }
        else {
            //2015/04/01 TMEJ 小澤 BTS-261対応 サービス名の表示制御の修正 START
            //            if ((this.upperDisp) && (this.lowerDisp)) {
            //                if ((this.upperDisp != "") && (this.lowerDisp != "")) {
            //                    strSvcName = this.upperDisp + this.lowerDisp;
            //                }
            //            }

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
        }
        return strSvcName;
    },
    /** 
    * サブチップを更新する（個別チップ）
    *
    * @return {void}
    *
    */
    updateSubChip: function updateSubChip() {
        var strData = "";   //一時データ
        var objChip = $("#" + this.KEY);  //チップの枠

        objChip.children().remove();    //子Div且つMCp以外のクラスは全部削除
        objChip.removeClass("StRez StWork StTRez StWComplete StDComplete StNew");  //色のクラスは全部削除
        //チップの色を決定するdiv
        var strColor = this.getChipColor(); //チップの色クラス名を取得
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
        objChip.append(this.getStatusIcons());

        //整備内容
        strData = "";
        if (this.svcClassNameEng) {
            strData = this.svcClassNameEng;
        }
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
        AdjustSubChipItemByWidth(this.KEY);
    }
}

//操作中のサブチップ構造体
function MovingSubChip() {
    /**
    * ストールID
    * @return {Long}
    */
    this.stallId = "";

    /**
    * 開始日時
    * @return {Date}
    */
    this.startDateTime = new Date(C_DATE_DEFAULT_VALUE);

    /**
    * 予定作業時間
    * @return {Long}
    */
    this.scheWorkTime = 0;

    /**
    * 選択チップID/サブチップID
    * @return {Long}
    */
    this.selectedChipId = 0;
}
MovingSubChip.prototype = {
    /**
    * ストールIDを格納する.
    *
    * @param {Long} aStallId ストールID
    * @return {void}
    *
    */
    setStallId: function setStallId(aStallId) {
        try {
            if (aStallId) {
                this.stallId = aStallId;
            }
        }
        catch (e) {
            this.stallId = "";
        }
    },
    /**
    * 予定作業時間を格納する.
    * @param {Long} aScheWorkTime 予定作業時間
    * @return {void}
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

