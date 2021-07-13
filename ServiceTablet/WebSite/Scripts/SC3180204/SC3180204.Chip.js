//---------------------------------------------------------
//SC3180204.Chip.js
//---------------------------------------------------------
//機能：完成検査入力画面ー_予約チップクラス
//作成：2014/02/14 AZ宮澤
//更新：
//---------------------------------------------------------

//ステータス
//2013/06/17 TMEJ 成澤 【開発】IT9513_【A.STEP2】次世代e-CRBタブレット　新DB適応に向けた機能設計 START
//var C_STATUS_NULL = 0;   //ステータスなし
//var C_STATUS_REZ_FIX = 1; //ストール本予約
//var C_STATUS_REZ_TEMP = 2;    //ストール仮予約
//var C_STATUS_UNAVAILABLE = 3; //使用不可
//var C_STATUS_DELIVERY = 4;        //引取・納車
//var C_STATUS_REST = 99;       //休憩

var C_STATUS_NULL = 5;   //ステータスなし
var C_STATUS_REZ_FIX = 1; //ストール本予約
var C_STATUS_REZ_TEMP = 0;    //ストール仮予約
var C_STATUS_UNAVAILABLE = 3; //使用不可
var C_STATUS_DELIVERY = 4;        //引取・納車
var C_STATUS_REST = 99;       //休憩
//2013/06/17 TMEJ 成澤 【開発】IT9513_【A.STEP2】次世代e-CRBタブレット　新DB適応に向けた機能設計 END

//実績ステータス
var C_RESULT_STATUS_WAIT = "1";    //作業待ち
var C_RESULT_STATUS_WORKING = "2"; //作業中
var C_RESULT_STATUS_COMPLETION = "3";    //作業完了

//予約_受付納車区分
var C_REZ_RECEPTION_WAIT = "0";   //店舗待ち客
var C_REZ_RECEPTION_PICKUP_DELIVARY = "1";     //PickUp & DelivaryService（預け客）
var C_REZ_RECEPTION_PICKUP = "2";     //PickUpService（預け客）
var C_REZ_RECEPTION_BRINGIN_DELIVERY = "3";     //BringIn & DeliveryService（預け客）
var C_REZ_RECEPTION_DROPOFF = "4";   //DropOff（預け客）

//来店フラグ
var C_WALKIN_RESERVE = "0";       //予約客
var C_WALKIN_WALKIN = "1";        //来店客（飛び込み）

////サービスコード
var C_SERVICECODE_INSPECTION = "10";  //車検
var C_SERVICECODE_PERIODIC = "20";    //定期点検
var C_SERVICECODE_GENERAL = "30";     //一般点検
var C_SERVICECODE_NEWCAR = "40";      //新車点検

//チップ種別（CSSのクラス名の一部としても使用する）
var C_CHIP_REZ_FIX = "RezFix";   //本予約
var C_CHIP_REZ_TEMP = "RezTemp";  //仮予約
var C_CHIP_DELIVERY = "Deli";      //引取・納車
var C_CHIP_DELAY = "Delay";     //遅れ
var C_CHIP_COMPLETION = "Comp";      //完了
var C_CHIP_REST = "Rest";      //休憩
var C_CHIP_UNAVAILABLE = "Unavailable";   //使用不可
var C_CHIP_OTHER = "Other";    //その他（エラー）

//日付の最小値を取得する.
var C_DATE_MIN_VALUE = Date.parse("0001/01/01 0:00:00");


// 2012/06/01 KN 西田 STEP1 重要課題対応 START
// 着工指示完了
var C_INSTRUCT_COMPLETE = "2";

// 部品準備完了
var C_MERCHANDISE_FLAG_COMPLETE = "1";
//merchandiseFlag 
// 2012/06/01 KN 西田 STEP1 重要課題対応 END

// 2012/11/29 TMEJ 河原 【A. STEP2】SA ストール予約受付機能開発 仕分けNO.74対応
// 完成検査承認待ち
var C_INSPECTIONREQ_FLAG_WAIT = "1";
// 2012/11/29 TMEJ 河原 【A. STEP2】SA ストール予約受付機能開発 仕分けNO.74対応

//ストール予約チップクラス
//チップの生成まで、実際の配置は呼び出したクラスで行うものとする
/**
* チップ情報を格納し、チップを生成配置する.
*
* @class チップ情報の格納・生成クラス.
* チップの情報を所持し、それらを取り扱う機能を保有する.
*
* @param {String} aChipId チップID（プライマリー）
* @param {Date} aStallStartTime ストール作業開始時刻
* @param {Date} aStallEndTime ストール作業終了時刻
*/
function ReserveChip(aChipId, aStallStartTime, aStallEndTime) {
    /**
    * チップタグのID
    * （チップID="chip_" + 予約ID + "_" + シーケンス番号 + "_" + 日跨ぎシーケンス番号
    * @return {String}
    */
    this.chipId = aChipId;

    /**
    * ストール作業開始時刻
    * @return {Date}
    */
    this.stallStartTime = aStallStartTime;

    /**
    * ストール作業終了時刻
    * @return {Date}
    */
    this.stallEndTime = aStallEndTime;

    /**
    * 開始時間（予定）
    * @return {Date}
    */
    this.chipStartTime = null;

    /**
    * 終了時間（予定）
    * @return {Date}
    */
    this.chipEndTime = null;

    /**
    * 開始時間（作業開始に基づいた時間）
    * @return {Date}
    */
    this.chipRezStartTime = null;

    /**
    * 終了時間（RezStartTimeに応じた終了予定時間)
    * @return {Date}
    */
    this.chipRezEndTime = null;

    /**
    * 開始時間（実績）
    * @return {Date}
    */
    this.chipResultStartTime = null;

    /**
    * 終了時間（実績）
    * @return {Date}
    */
    this.chipResultEndTime = null;

    /**
    * チップ描画開始時間
    * @return {Date}
    */
    this.chipDrawStartTime = null;

    /**
    * チップ描画終了時間
    * @return {Date}
    */
    this.chipDrawEndTime = null;

    /**
    * 予約ID
    * @return {Integer}
    */
    this.rezId = null;

    /**
    * 日跨ぎシーケンス番号
    * @return {Integer}
    */
    this.dSeqNo = 0;

    /**
    * シーケンス番号
    * @return {Integer}
    */
    this.intSeqNo = 0;

    /**
    * サービスコード（車検・定期点検・新車点検・一般点検）
    * @return {String}
    */
    this.chipServiceCode = null;

    /**
    * 来店フラグ
    * @param {String}
    */
    this.chipWalkIn = C_WALKIN_WALKIN;

    /**
    * 車両番号
    * @param {String}
    */
    this.chipVclRegNo = null;

    /**
    * ステータス（仮予約・本予約・使用禁止・休憩）
    * @return {Integer}
    */
    this.chipStatus = C_STATUS_NULL;

    /**
    * 予約_受付納車区分
    * @return {String}
    */
    this.chipRezReception = C_REZ_RECEPTION_WAIT;

    /**
    * 入庫日時（入庫されている場合、日時が格納されている）
    * @return {Date}
    */
    this.chipStrDate = null;

    /**
    * 実績ステータス（作業待ち・作業中・完了）
    * @return {String}
    */
    this.chipResultStatus = C_RESULT_STATUS_WAIT;

    /**
    * OrderNumber
    * @return {String}
    */
    this.orderNumber = "";

    /**
    * 子番号
    * @return {Integer}
    */
    this.childNumber = 0;

    /**
    * 更新カウント
    * @return {Integer}
    */
    this.chipUpdateCount = 0;

    /**
    * 定期点検の距離
    * @return {String}
    */
    this.distance = "";

    /**
    * 定期点検の距離の単位
    * @return {String}
    */
    this.distanceUnit = "";

    //キャンセルフラグ
    //this.cancelFlag = null;
    //作業時間（予定）
    //this.dtmRezWorkTime = null;
    //作業時間（実績）
    //this.dtmResultWorkTime = null;

    //担当SA名
    //this.strSAName = "";

    /**
    * チップ種別
    * @return {String}
    */
    this.chipColor = C_CHIP_OTHER;

    /**
    * タップ有効フラグ
    * @return {Boolean}
    */
    this.chipTapFlag = false;

    /**
    *チップ情報更新時間
    * @return {Date}
    */
    this.dtmUpdateTime = null;

    //2012/02/27 KN 渡辺 KN_0225_HH_2_結合テスト（現地）課題票の修正 Start
    /**
    * 入庫予定時間
    * @return {Date}
    */
    this.chipCrryInTime = null;
    //2012/02/27 KN 渡辺 KN_0225_HH_2_結合テスト（現地）課題票の修正 End

    //SA名
    //サービスコメント
    //入庫予定日時
    //ROナンバー
    //納車区分
    //CSSで生成されているチップオブジェクト
    this.objChipsBase;
    this.objChipsFilter;
    // 2012/06/01 KN 西田 STEP1 重要課題対応 START
    /**
    * 着工指示区分
    * @return {String}
    */
    this.instruct;

    /**
    * 作業連番
    * @return {Integer}
    */
    this.workSeq;

    /**
    * 部品準備完了フラグ
    * @return {String}
    */
    this.merchandiseFlag;

    // 2012/06/01 KN 西田 STEP1 重要課題対応 END

    // 2012/11/29 TMEJ 河原 【A. STEP2】SA ストール予約受付機能開発 仕分けNO.74対応 START
    /**
    * 完成検査承認待ちフラグ
    * @return {String}
    */
    this.inspectionReqFlag;

    // 2012/11/29 TMEJ 河原 【A. STEP2】SA ストール予約受付機能開発 仕分けNO.74対応 END

    /**
    * ストール利用ステータス
    * @return {Integer}
    */
    this.stallUseStatus;
    //13/08/08 TMEJ 成澤 【A.STEP2】タブレット版SMB開発に向けた要件定義 END

    //2013/11/26 TMEJ 成澤【IT9573】次世代e-CRBサービス 店舗展開に向けた標準作業確立 START
    /**
    * 販売店コード
    * @return {String}
    */
    this.dealerCode;
    //2013/11/26 TMEJ 成澤【IT9573】次世代e-CRBサービス 店舗展開に向けた標準作業確立 END
}

ReserveChip.prototype = {
    /**
    * 予約チップクラスのメンバ変数にデータベースから取得した値を格納する.
    *
    * @param {DataSet} aDataSet データベースより取得した値
    * @return {void}
    *
    */
    setChipParameter: function setChipParameter(aDataSet) {

        this.setRezId(aDataSet.REZID);      //予約ID
        this.setDSeqNo(aDataSet.DSEQNO);   //日跨ぎシーケンス番号
        this.setSeqNo(aDataSet.SEQNO);  //中断シーケンス番号
        this.setStartTime(aDataSet.STARTTIME);  //開始時間（予定）
        this.setEndTime(aDataSet.ENDTIME);  //終了時間（予定）
        this.setStatus(aDataSet.STATUS);    //ステータス
        this.setResultStatus(aDataSet.RESULT_STATUS);   //作業実績ステータス
        this.setRezReception(aDataSet.REZ_RECEPTION);   //予約_受付納車区分
        this.setResultStartTime(aDataSet.RESULT_START_TIME);    //開始時間（実績）
        this.setResultEndTime(aDataSet.RESULT_END_TIME);    //終了時間（実績）
        this.setRezStartTime(aDataSet.REZ_START_TIME);  //予定_ストール開始日時時刻
        this.setRezEndTime(aDataSet.REZ_END_TIME);  //予定_ストール終了日時時刻
        this.setDrawStartTime();    //チップを配置する開始時間
        this.setDrawEndTime();      //チップを配置する終了時間
        this.setStrDate(aDataSet.STRDATE);  //入庫日時
        this.setServiceCode(aDataSet.SERVICECODE);  //サービスコード
        this.setWalkIn(aDataSet.WALKIN);    //来店フラグ
        this.setVclRegNo(aDataSet.VCLREGNO);    //車両番号
        this.setUpdateCount(aDataSet.UPDATE_COUNT);  //チップの更新カウンタ
        this.setOrderNumber(aDataSet.ORDERNO);  //OrderNumber
        this.setChildNumber(aDataSet.REZCHILDNO);   //子番号
        this.setDistance(aDataSet.SVCORGNMCT); //定期点検距離
        this.setDistanceUnit(aDataSet.SVCORGNMCB); //定期点検距離の単位
        //2012/02/27 KN 渡辺 KN_0225_HH_2_結合テスト（現地）課題票の修正 Start
        this.setChipCrryInTime(aDataSet.CRRYINTIME);    //入庫予定時間
        //2012/02/27 KN 渡辺 KN_0225_HH_2_結合テスト（現地）課題票の修正 End
        // 2012/06/01 KN 西田 STEP1 重要課題対応 START
        this.setInstruct(aDataSet.INSTRUCT);                // 着工指示区分
        this.setWorkSeq(aDataSet.WORKSEQ);                  // 作業連番
        this.setMerchandiseFlag(aDataSet.MERCHANDISEFLAG);  // 部品準備完了フラグ
        // 2012/06/01 KN 西田 STEP1 重要課題対応 END

        // 2012/11/29 TMEJ 河原 【A. STEP2】SA ストール予約受付機能開発 仕分けNO.74対応 START
        this.setinspectionReqFlag(aDataSet.INSPECTIONREQFLG);  // 完成検査承認待ちフラグ
        // 2012/11/29 TMEJ 河原 【A. STEP2】SA ストール予約受付機能開発 仕分けNO.74対応 END

        //13/08/08 TMEJ 成澤 【A.STEP2】タブレット版SMB開発に向けた要件定義 START
        this.setStallUseStatus(aDataSet.STALL_USE_STATUS);
        //13/08/08 TMEJ 成澤 【A.STEP2】タブレット版SMB開発に向けた要件定義 END

         //2013/11/26 TMEJ 成澤【IT9573】次世代e-CRBサービス 店舗展開に向けた標準作業確立 START
        this.setDealerCode(aDataSet.DLRCD);
        //2013/11/26 TMEJ 成澤【IT9573】次世代e-CRBサービス 店舗展開に向けた標準作業確立  END
    },


    //チップ情報の更新時間を設定する
    setUpdateTime: function setUpdateTime(dtmUPTime) {
        this.dtmUpdateTime = dtmUPTime;
    },


    //生成するチップ種別を設定（併せて、タップの有効フラグを設定する）
    setChipColor: function setChipColor() {

        //ステータスに応じて、チップ種別を決定する
        if (this.chipStatus == C_STATUS_REZ_TEMP) {
            //2012/03/09 上田 仮予約の遅れ考慮 START
            if (this.chipEndTime < getServerTimeNow()) {
                //現在時間が終了時刻(予定)を超えている場合、遅れチップ
                this.chipColor = C_CHIP_DELAY;
                this.chipTapFlag = true;
            } else {
                //上記以外は仮予約チップ
                this.chipColor = C_CHIP_REZ_TEMP;
                this.chipTapFlag = true;
            }
            //this.chipColor = C_CHIP_REZ_TEMP;
            //this.chipTapFlag = true;
            //2012/03/09 上田 仮予約の遅れ考慮 END
        } else if (this.chipStatus == C_STATUS_UNAVAILABLE) {
            this.chipColor = C_CHIP_UNAVAILABLE;
        } else if (this.chipStatus == C_STATUS_DELIVERY) {
            //2012/03/09 上田 引取・納車チップの実績チップ考慮対応 START
            if (this.chipResultStatus == C_RESULT_STATUS_COMPLETION) {
                //作業完了している場合は実績チップとする
                this.chipColor = C_CHIP_COMPLETION;
                this.chipTapFlag = true;
            }
            else {
                //作業前、作業中は規定値(緑)を設定
                this.chipColor = C_CHIP_DELIVERY;
                this.chipTapFlag = true;
            }
            //this.chipColor = C_CHIP_DELIVERY;
            //this.chipTapFlag = true;
            //2012/03/09 上田 引取・納車チップの実績チップ考慮対応 END
        } else if (this.chipStatus == C_STATUS_REST) {
            this.chipColor = C_CHIP_REST;
        } else if (this.chipStatus == C_STATUS_REZ_FIX) {
            //ストール本予約の場合、遅れ検出、実績ステータスのチェックを行い、チップ種別を確定する
            //まずは、実績チップかを判定する（作業完了しているか）
            if (this.chipResultStatus == C_RESULT_STATUS_COMPLETION) {
                this.chipColor = C_CHIP_COMPLETION;
                this.chipTapFlag = true;
            }
            // 2012/06/21 KN 西田 STEP1 重要課題対応 START
            else {
                // 遅れ比較時間
                var endTime = this.chipEndTime;


                if (this.chipResultStatus == C_RESULT_STATUS_WORKING) {
                    // 作業中の場合は、実開始時間から求めた終了時間

                    //更新：2013/06/17 TMEJ 成澤 【開発】IT9513_【A.STEP2】次世代e-CRBタブレット　新DB適応に向けた機能設計 START
                    //endTime = this.chipResultEndTime;
                    var endTime = this.chipRezEndTime;
                    //更新：2013/06/17 TMEJ 成澤 【開発】IT9513_【A.STEP2】次世代e-CRBタブレット　新DB適応に向けた機能設計 END
                }

                //遅れチップなのか否かを現在時間が、終了時間（予定）を超えているかを判定する
                if (endTime < getServerTimeNow()) {
                    this.chipColor = C_CHIP_DELAY;
                    this.chipTapFlag = true;
                } else {
                    this.chipColor = C_CHIP_REZ_FIX;
                    this.chipTapFlag = true;
                }
            }
            ////遅れチップなのか否かを現在時間が、終了時間（予定）を超えているかを判定する
            //else if (this.chipEndTime < getServerTimeNow()) {
            //    this.chipColor = C_CHIP_DELAY;
            //    this.chipTapFlag = true;
            //} else {
            //    this.chipColor = C_CHIP_REZ_FIX;
            //    this.chipTapFlag = true;
            //}
            // 2012/06/21 KN 西田 STEP1 重要課題対応 END
        } else {
            this.chipColor = C_CHIP_OTHER;
        }
    },


    //休憩チップを作成する
    createChipRest: function createChipRest() {

        //休憩チップのオブジェクトを作成する
        var objChipFrame = $("<div />").addClass("ChipRest");
        this.objChipsBase.append(objChipFrame);

        var objChipNoData = $("<div />").addClass("NoData");
        objChipFrame.append(objChipNoData);

        var objChipAddText = $("<div />").addClass("addText");
        objChipAddText.text($("#HiddenRestText").val().toString());
        objChipNoData.append(objChipAddText);
        this.objChipsBase.css("z-index", "1");
    },


    //使用不可チップを作成する
    createChipUnavailable: function createChipUnavailable() {

        //使用不可チップのオブジェクトを作成する
        var objChipFrame = $("<div />").addClass("ChipUnavailable");
        this.objChipsBase.append(objChipFrame);

        var objChipUnavilable = $("<div />").addClass("Unavailable");
        objChipFrame.append(objChipUnavilable);

        var objChipAddText = $("<div />").addClass("addText");
        objChipAddText.text($("#HiddenUnavailableText").val().toString());
        objChipUnavilable.append(objChipAddText);
    },


    //作業チップを作成する
    createWorkChip: function createWorkChip() {

        //チップの枠クラスの設定
        objChipFrame = this.createChipFrame();

        var objChipUpperArea = $("<div />").addClass("UpperArea");
        var objChipLowerArea = $("<div />").addClass("lowerArea");
        var objChipLast2IconArea = $("<div />").addClass("Last2IconArea");

        //作成した上部エリアをチップ枠タブの子要素として追加する
        objChipFrame.append(objChipUpperArea);
        //作成した下部エリアをチップ枠タブの子要素として追加する
        objChipFrame.append(objChipLowerArea);

        //上部エリアの詳細を生成
        this.createUpperArea(objChipUpperArea);

        //チップ種別と、予約_受付納車区分により、CSSクラス名を設定し
        //下部エリアを塗りつぶすか白抜きにするかを決めるCSSクラスを追加する
        strLAaddClassName = "LA" + this.chipColor;
        if (this.chipRezReception == C_REZ_RECEPTION_WAIT) {
            strLAaddClassName += "0";
        }
        objChipLowerArea.addClass(strLAaddClassName);

        //各下部エリアの要素を生成し、追加する
        objChipLowerArea.append(this.createWalkInFlag());
        objChipLowerArea.append(objChipLast2IconArea);

        objChipLast2IconArea.append(this.createLowerArea02());
        objChipLast2IconArea.append(this.createLowerArea03());
    },


    //チップを描画する枠を生成し、生成したオブジェクトを返す
    createChipFrame: function createChipFrame() {

        //作成したオブジェクトに、チップ枠を付与する
        var objChipFrame = $("<div />").addClass("ChipFrame");
        //チップ種別によってチップ枠によってCSSクラスを追加する
        objChipFrame.addClass("CF" + this.chipColor);
        //作成したチップ枠を子要素として追加する
        this.objChipsBase.append(objChipFrame);

        return objChipFrame;
    },


    //上部エリアの子要素を生成し、子要素を追加する
    createUpperArea: function createUpperArea(objChipUpperArea) {

        /* 顧客名の表示は不要
        //顧客名の表示
        var objChipUpper01 = $("<div />").addClass("titleLine01");
        objChipUpper01.text(this.strCustName);
        objChipUpperArea.append(objChipUpper01);
        */

        //車両番号の表示
        var objChipUpper02 = $("<div />").addClass("titleLine02");
        objChipUpper02.text(this.chipVclRegNo);
        objChipUpperArea.append(objChipUpper02);

        /* 入庫予定時間の表示は不要
        //入庫予定時間の表示
        var objChipUpper03 = $("<div />").addClass("time");
        objChipUpper03.text(formatTime(this.dtmCrryInTime.getHours()) + ":" + formatTime(this.dtmCrryInTime.getMinutes()));
        objChipUpperArea.append(objChipUpper03);
        */
    },


    //来店フラグにより、ピンを描画して、その要素を返す
    createWalkInFlag: function createWalkInFlag() {

        //ピンを描画するタブを生成
        var objChipAddIcon01 = $("<div />").addClass("AddIcon01");
        //来店フラグを参照し、予約客である場合Pinを描画する
        if (this.chipWalkIn == C_WALKIN_RESERVE) {
            objChipAddIcon01.addClass("WalkIn" + this.chipColor);
        }

        return objChipAddIcon01;
    },


    //入庫状態部分を描画するタグ要素を生成し、返す
    createLowerArea02: function createLowerArea02() {

        var objChipAddIcon02;
        var objChipAddIcon02_li03;

        //実績ステータスが作業待ちの場合のみ入庫状態を描画する
        if (this.chipResultStatus == C_RESULT_STATUS_WAIT) {
            // 2012/06/01 KN 西田 STEP1 重要課題対応 START
            // 作業待ち、且つ着工指示がある場合
            if (C_INSTRUCT_COMPLETE == this.instruct) {

                objChipAddIcon02 = $("<div />").addClass("AddIconInstruct02");

                if (this.chipCrryInTime != null && this.chipStrDate > this.chipCrryInTime) {
                    //objChipAddIcon02.addClass("InstructDelay" + this.chipRezReception);
                    objChipAddIcon02.addClass("InstructDelay1");
                }
                else {
                    objChipAddIcon02.addClass("Instruct" + this.chipColor + this.chipRezReception);
                }
            }
            // 2012/06/01 KN 西田 STEP1 重要課題対応 END

            //入庫日時が指定されている場合、入庫済みアイコンを配置する
            else if (this.chipStrDate) {
                //チップ種別、予約_受付納車区分より、入庫済みアイコンとするCSSクラスを追加する
                objChipAddIcon02 = $("<div />").addClass("AddIcon02");
                // 2012/03/09 上田 入庫遅れ発生時のアイコン修正 START
                if (this.chipCrryInTime != null && this.chipStrDate > this.chipCrryInTime) {
                    objChipAddIcon02.addClass("CrryInDelay1");
                }
                else {
                    objChipAddIcon02.addClass("CrryIn" + this.chipColor + this.chipRezReception);
                }
                //objChipAddIcon02.addClass("CrryIn" + this.chipColor + this.chipRezReception);
                // 2012/03/09 上田 入庫遅れ発生時のアイコン修正 END
            }

            //2012/02/27 KN 渡辺 KN_0225_HH_2_結合テスト（現地）課題票の修正 Start
            //入庫日時が指定されていない場合、入庫前のクラスを設定する
            else {
                objChipAddIcon02 = $("<div />").addClass("AddIcon02Text");

                //2012/03/08 上田 引取・納車チップの入庫予定日時削除 START
                if (this.chipStatus == C_STATUS_DELIVERY && (this.childNumber == 0 || this.childNumber == 999)) {
                    //引取・納車チップの場合は、入庫予定日時は設定不要のため、何もしない
                    return;
                }
                //2012/03/08 上田 引取・納車チップの入庫予定日時削除 END

                var crryInTime = new Date();
                if (this.chipCrryInTime) {
                    crryInTime = this.chipCrryInTime;
                }
                //入庫予定時間が存在しない場合、開始予定時間を入庫予定時間とする.
                else {
                    // 2012/03/09 上田 日跨ぎを考慮するように修正 START
                    crryInTime = this.chipDrawStartTime;
                    //crryInTime = this.chipStartTime;
                    // 2012/03/09 上田 日跨ぎを考慮するように修正 END
                }

                //サーバ現在時刻を取得
                var nowTime = getServerTimeNow();
                //現在時刻が入庫予定時間を経過していた場合、赤字で表示するクラスを追加する.
                if (crryInTime < nowTime) {
                    objChipAddIcon02.addClass("TextColorDelay");
                }

                var crryInTimeText = "";
                //入庫予定時刻が当日の場合、入庫予定時刻を表示する
                if ((crryInTime.getYear() == nowTime.getYear()) && (crryInTime.getMonth() == nowTime.getMonth()) && (crryInTime.getDate() == nowTime.getDate())) {
                    crryInTimeText += formatTime(crryInTime.getHours());
                    crryInTimeText += ":";
                    crryInTimeText += formatTime(crryInTime.getMinutes());
                }
                //入庫予定時刻が当日でない場合、日付を描画する
                else {
                    //crryInTimeText += formatTime(crryInTime.getMonth());
                    crryInTimeText += formatTime(crryInTime.getMonth() + 1);
                    crryInTimeText += "/";
                    crryInTimeText += formatTime(crryInTime.getDate());
                }
                objChipAddIcon02.text(crryInTimeText);
            }
            //2012/02/27 KN 渡辺 KN_0225_HH_2_結合テスト（現地）課題票の修正 End
        }
        //実績ステータスが、作業中or作業完了の場合、作業時間を配置する
        else {
            objChipAddIcon02 = $("<ul />").addClass("AddIcon02Working");

            //開始時間（実績）が存在する場合、開始時間を表示する
            if ((this.chipResultStartTime != null) && (!isNaN(this.chipResultStartTime)) && (this.chipResultStartTime != "Invalid Date")) {
                objChipAddIcon02_li01 = $("<li />").addClass("time");
                //開始時間（実績）が開始時間（予定）よりも大きい場合、文字色を赤に設定する
                if (this.chipResultStartTime > this.chipRezStartTime) {
                    objChipAddIcon02_li01.addClass("TextColorDelay");
                }
                objChipAddIcon02_li01.text(formatTime(this.chipResultStartTime.getHours()) + ":" + formatTime(this.chipResultStartTime.getMinutes()));
                objChipAddIcon02.append(objChipAddIcon02_li01);

                objChipAddIcon02_li02 = $("<li />").addClass("Down");
                objChipAddIcon02.append(objChipAddIcon02_li02);
            }

            //2012/03/09 上田 作業中チップと作業完了チップの作業時間の文字色設定方法修正 START
            if (this.chipResultStatus == C_RESULT_STATUS_WORKING) {
                //作業中チップの場合
                objChipAddIcon02_li03 = $("<li />").addClass("time");
                //作業終了予定時刻と現在時刻の比較を行う
                var nowTime = getServerTimeNow();
                if (this.chipRezEndTime < nowTime) {
                    //現在時刻が作業終了予定時刻より大きい場合、文字色を赤に設定する
                    objChipAddIcon02_li03.addClass("TextColorDelay");
                }
                objChipAddIcon02_li03.text(formatTime(this.chipResultEndTime.getHours()) + ":" + formatTime(this.chipResultEndTime.getMinutes()));
                objChipAddIcon02.append(objChipAddIcon02_li03);
            }
            else {
                //作業完了チップの場合
                //終了時間（実績）が存在する場合、終了時間を表示する
                if ((this.chipResultEndTime != null) && (!isNaN(this.chipResultEndTime)) && (this.chipResultEndTime != "Invalid Date")) {
                    objChipAddIcon02_li03 = $("<li />").addClass("time");

                    if (this.chipResultEndTime > this.chipRezEndTime) {
                        //終了時間（実績）が終了時間（予定）よりも大きい場合、文字色を赤に設定する
                        objChipAddIcon02_li03.addClass("TextColorDelay");
                    }
                    else {
                        var rezTime = this.chipRezEndTime - this.chipRezStartTime;              //予定の総時間
                        var resultTime = this.chipResultEndTime - this.chipResultStartTime;     //実績の総時間
                        if (rezTime < resultTime) {
                            //実績の総時間が予定の総時間より大きい場合、文字色を赤に設定する
                            objChipAddIcon02_li03.addClass("TextColorDelay");
                        }
                    }
                    objChipAddIcon02_li03.text(formatTime(this.chipResultEndTime.getHours()) + ":" + formatTime(this.chipResultEndTime.getMinutes()));
                    objChipAddIcon02.append(objChipAddIcon02_li03);
                }
            }
            //2012/03/09 上田 作業中チップと作業完了チップの作業時間の文字色設定方法修正 END
        }

        return objChipAddIcon02;
    },


    //作業種別部分を描画するタグ要素を生成し、返す
    createLowerArea03: function createLowerArea03() {
        var objChipAddIcon03;

        // 2012/06/01 KN 西田 STEP1 重要課題対応 START

        // 2012/11/29 TMEJ 河原 【A. STEP2】SA ストール予約受付機能開発 仕分けNO.74対応 START
        //if (C_MERCHANDISE_FLAG_COMPLETE == this.merchandiseFlag) {
        // 完成検査承認待ちの場合、完成検査承認待ちアイコンを表示
        if (this.inspectionReqFlag == C_INSPECTIONREQ_FLAG_WAIT) {
            objChipAddIcon03 = $("<div />").addClass("AddIcon03");
            addIcon03ImageClass = "InspectionReq";
            objChipAddIcon03.addClass(addIcon03ImageClass);
        }
        // 部品準備完了の場合、部品準備完了アイコンを表示
        else if (C_MERCHANDISE_FLAG_COMPLETE == this.merchandiseFlag) {
            // 2012/11/29 TMEJ 河原 【A. STEP2】SA ストール予約受付機能開発 仕分けNO.74対応 END

            objChipAddIcon03 = $("<div />").addClass("AddIcon03");
            addIcon03ImageClass = "Merchandise";
            // 待ち/預かり区別無く白枠＋色つきのアイコン表示
            addIcon03ImageClass += this.chipColor;

            ////予約_受付納車区分が店舗待ち客の場合のみ、各チップ種別毎のクラスを設定する
            ////預け客の場合は、白抜きのイメージを配置する
            //if (this.chipRezReception == C_REZ_RECEPTION_WAIT) {
            //    addIcon03ImageClass += this.chipColor;
            //}
            objChipAddIcon03.addClass(addIcon03ImageClass);
        }
        // 2012/06/01 KN 西田 STEP1 重要課題対応 END
        //定期点検距離がブランクでない場合、定期点検を表示する.
        else if (this.distance != "") {
            objChipAddIcon03 = $("<ul />").addClass("AddIcon03Periodic");
            //定期点検の距離を表示する
            var objChipAppIcon03UpText = $("<li />").addClass("UpText");
            objChipAppIcon03UpText.text(this.distance);
            objChipAddIcon03.append(objChipAppIcon03UpText);
            //定期点検の距離の単位を表示する
            var objChipAppIcon03UnderText = $("<li />").addClass("UnderText");
            objChipAppIcon03UnderText.text(this.distanceUnit);
            objChipAddIcon03.append(objChipAppIcon03UnderText);
            //予約_納車区分が店舗待ち客の場合のみ、文字色を各チップ種別毎に設定する
            strTextColorClass = "TextColor";
            if (this.chipRezReception == C_REZ_RECEPTION_WAIT) {
                strTextColorClass += this.chipColor;
            }
            objChipAddIcon03.addClass(strTextColorClass);
        }
        //2012/03/08 上田 引取チップと納車チップのアイコン修正 START
        //引取チップの場合
        else if (this.chipStatus == C_STATUS_DELIVERY && this.childNumber == 0) {
            objChipAddIcon03 = $("<div />").addClass("AddIcon03");
            objChipAddIcon03.addClass("PickUp");
        }
        //納車チップの場合
        else if (this.chipStatus == C_STATUS_DELIVERY && this.childNumber == 999) {
            objChipAddIcon03 = $("<div />").addClass("AddIcon03");
            objChipAddIcon03.addClass("Delivery");
        }
        //        //ステータスに引取納車が設定されている場合、引取or納車のアイコンを表示する
        //        else if (this.chipStatus == C_STATUS_DELIVERY) {
        //            objChipAddIcon03 = $("<div />").addClass("AddIcon03");
        //        }
        //2012/03/08 上田 引取チップと納車チップのアイコン修正 END
        //サービスコードが車検の場合、車検アイコンを表示する
        else if (this.chipServiceCode == C_SERVICECODE_INSPECTION) {
            objChipAddIcon03 = $("<div />").addClass("AddIcon03");
            addIcon03ImageClass = "Inspection";
            //予約_受付納車区分が店舗待ち客の場合のみ、各チップ種別毎のクラスを設定する
            //預け客の場合は、白抜きのイメージを配置する
            if (this.chipRezReception == C_REZ_RECEPTION_WAIT) {
                addIcon03ImageClass += this.chipColor;
            }
            objChipAddIcon03.addClass(addIcon03ImageClass);
        }
        //サービスコードが一般点検の場合、一般点検のアイコンを表示する
        else if (this.chipServiceCode == C_SERVICECODE_GENERAL) {
            objChipAddIcon03 = $("<div />").addClass("AddIcon03");
            addIcon03ImageClass = "General";
            //予約_受付納車区分が店舗待ち客の場合のみ、各チップ種別毎のクラスを設定する
            //預け客の場合は、白抜きのイメージを配置する
            if (this.chipRezReception == C_REZ_RECEPTION_WAIT) {
                addIcon03ImageClass += this.chipColor;
            }
            objChipAddIcon03.addClass(addIcon03ImageClass);
        }
        //サービスコードが新車点検の場合、新車点検のアイコンを表示する
        else if (this.chipServiceCode == C_SERVICECODE_NEWCAR) {
            objChipAddIcon03 = $("<div />").addClass("AddIcon03");
            addIcon03ImageClass = "Newcar";
            //予約_受付納車区分が店舗待ち客の場合のみ、各チップ種別毎のクラスを設定する
            //預け客の場合は、白抜きのイメージを配置する
            if (this.chipRezReception == C_REZ_RECEPTION_WAIT) {
                addIcon03ImageClass += this.chipColor;
            }
            objChipAddIcon03.addClass(addIcon03ImageClass);
        }

        return objChipAddIcon03;
    },


    //チップを生成する（
    createChipContents: function createChipContents() {
        //チップ内の子要素をすべて削除する
        this.objChipsBase.empty();


        //その他チップの場合何もしない
        if (this.chipColor == C_CHIP_OTHER) {
        }
        //休憩チップを生成する
        else if (this.chipColor == C_CHIP_REST) {
            this.createChipRest();
            //更新：13/08/08 TMEJ 成澤 【A.STEP2】タブレット版SMB開発に向けた要件定義 START
            //休憩チップの表示順序変更
            this.objChipsBase.css("z-index", "0");
            //更新：13/08/08 TMEJ 成澤 【A.STEP2】タブレット版SMB開発に向けた要件定義 END
        }
        //使用不可チップを生成する
        else if (this.chipColor == C_CHIP_UNAVAILABLE) {
            this.createChipUnavailable();
            //更新：13/08/08 TMEJ 成澤 【A.STEP2】タブレット版SMB開発に向けた要件定義 START
            //使用不可チップの表示順序変更
            this.objChipsBase.css("z-index", "1");
            //更新：13/08/08 TMEJ 成澤 【A.STEP2】タブレット版SMB開発に向けた要件定義 END
        }
        //その他・休憩・使用不可以外のチップを生成する
        else {
            this.createWorkChip();
            //更新：13/08/08 TMEJ 成澤 【A.STEP2】タブレット版SMB開発に向けた要件定義 START
            //作業中チップの表示順序変更
            if (this.chipResultStatus == C_RESULT_STATUS_WORKING) {
                this.objChipsBase.css("z-index", "6");
                //作業完了チップの表示順序変更
            } else if (this.chipResultStatus == C_RESULT_STATUS_COMPLETION) {
                this.objChipsBase.css("z-index", "2");
                //作業待ちチップの表示順序変更
            } else if (this.chipResultStatus == C_RESULT_STATUS_WAIT) {
                this.objChipsBase.css("z-index", "4");
            } else if (this.chipColor == C_CHIP_UNAVAILABLE) {
            }
            //更新：13/08/08 TMEJ 成澤 【A.STEP2】タブレット版SMB開発に向けた要件定義 END
        }

        var lngStartPosition = getDrawPositionX(this.chipDrawStartTime);
        var lngEndPosition = getDrawPositionX(this.chipDrawEndTime);
        //チップの描画幅を設定する
        this.objChipsBase.css("width", (lngEndPosition - lngStartPosition).toString() + "px");
        //チップを配置する
        this.objChipsBase.css("left", lngStartPosition.toString() + "px");

        

        //フィルターチップが存在する場合、フィルターチップの配置なども設定する
        if (this.chipTapFlag) {
            //フィルターチップの描画幅を設定する
            this.objChipsFilter.css("width", (lngEndPosition - lngStartPosition).toString() + "px");
            //フィルターチップを配置する
            this.objChipsFilter.css("left", lngStartPosition.toString() + "px");
        }
    },


    //チップ生成可能かを判定して、チップを生成する
    createChip: function createChip() {

        //チップ種別を設定する
        this.setChipColor();

        //チップを生成するに値するかのチェック
        var chipCreationFlag = this.checkChipCreation();
        if (chipCreationFlag) {

            //該当するIDタグが存在しない場合、そのオブジェクトを生成する
            if ((this.objChipsBase == undefined) || (this.objChipsBase.size == 0)) {
                //予約チップを配置する親要素を取得する
                var objParent = $("#Box01GraphLine");
                //<div>要素のオブジェクトを作成し、
                //作成したオブジェクトに、CSSのカセットクラスを付与する
                this.objChipsBase = $("<div />").addClass("ChipsBase");
                //チップのIDを付与する
                this.objChipsBase.attr("id", this.chipId + "_BASE");
                //生成したオブジェクトを子要素として追加する
                objParent.append(this.objChipsBase);
            }
            if ((this.objChipsFilter == undefined) || (this.objChipsFilter.size() == 0)) {
                //タップ可能の場合、フィルターを生成する
                if (this.chipTapFlag) {
                    this.objChipsFilter = ($("<div />").addClass("ChipsBaseFilter"));
                    //フィルターチップのIDを付与する
                    this.objChipsFilter.attr("id", this.chipId);
                    objParent.append(this.objChipsFilter);
                }
            }
            //チップの実体を生成する
            this.createChipContents();
        }

        return chipCreationFlag;
    },


    //チップを生成するに足る情報があるかのチェック
    checkChipCreation: function checkChipCreation() {

        var checkResult = false;

        //チップ種別がその他である場合、チップを生成しない
        if (this.chipColor != C_CHIP_OTHER) {
            //開始時間（予定）、もしくは、終了時間（予定）がNull値である場合もチップを生成しない
            if ((this.chipStartTime) && (this.chipEndTime)) {
                checkResult = true;
            }
        }

        return checkResult;
    },


    //チップのフィルターを設定する
    //this.setChipFilter = function (blnSelected) {
    setChipFilter: function setChipFilter(blnSelected, chipStatus) {

        //チップのCSSクラスをフィールド値にしたがって描画し、チップを再配置する
        //選択中フラグがtrueの場合、背景にライム色を指定
        if (this.chipTapFlag) {
            if (blnSelected) {
                this.objChipsFilter.css("opacity", C_FILTER_CLEAR);

            } else {
                this.objChipsFilter.css("opacity", C_FILTER_TRANSLUCENT);
            }
            //更新：13/08/08 TMEJ 成澤 【A.STEP2】タブレット版SMB開発に向けた要件定義 START
            //グレイフィルターを各チップに合わせた階層に配置
            if (chipStatus == "2") {
                this.objChipsFilter.css("z-index", "7");
            } else if (chipStatus == "1") {
                this.objChipsFilter.css("z-index", "5");
            } else {
                this.objChipsFilter.css("z-index", "3");
            }
            //更新：13/08/08 TMEJ 成澤 【A.STEP2】タブレット版SMB開発に向けた要件定義 END
        }
    },



    /**
    * 予約IDに値を格納する.
    *
    * @param {String} aRezId 予約ID情報
    * @return {void}
    *
    */
    setRezId: function setRezId(aRezId) {
        try {
            if (aRezId) {
                //更新：2013/12/12 TMEJ 成澤 【開発】IT9611_次世代サービス 工程管理機能開発 START
                //this.rezId = parseInt(aRezId);
                this.rezId = aRezId;
                //更新：2013/12/12 TMEJ 成澤 【開発】IT9611_次世代サービス 工程管理機能開発 END
            }
        }
        catch (e) {
            this.rezId = null;
        }
    },
    /**
    * 日跨ぎシーケンス番号に値を格納する.
    *
    * @param {String} aDSeqNo 日跨ぎシーケンス番号
    * @return {void}
    *
    */
    setDSeqNo: function setDSeqNo(aDSeqNo) {
        try {
            if (aDSeqNo) {
                //更新：2013/12/12 TMEJ 成澤 【開発】IT9611_次世代サービス 工程管理機能開発 START
                //this.dSeqNo = parseInt(aDSeqNo);
                this.dSeqNo = aDSeqNo;
                //更新：2013/12/12 TMEJ 成澤 【開発】IT9611_次世代サービス 工程管理機能開発 END
                
            }
        }
        catch (e) {
            this.dSeqNo = 0;
        }
    },
    /**
    * 中断シーケンス番号に値を格納する.
    *
    * @param {String} aSeqNo 中断シーケンス番号
    * @return {void}
    *
    */
    setSeqNo: function setSeqNo(aSeqNo) {
        try {
            if (aSeqNo) {
                //更新：2013/12/12 TMEJ 成澤 【開発】IT9611_次世代サービス 工程管理機能開発 START
                //this.seqNo = parseInt(aSeqNo);
                this.seqNo = aSeqNo;
                //更新：2013/12/12 TMEJ 成澤 【開発】IT9611_次世代サービス 工程管理機能開発 END
               
            }
        }
        catch (e) {
            this.seqNo = 0;
        }
    },
    /** 
    * 開始時間（予定）
    * @param {String} aStartTime 開始時刻（予定）
    *
    */
    setStartTime: function setStartTime(aStartTime) {
        try {
            if (aStartTime) {
                this.chipStartTime = new Date(aStartTime);
            }
        }
        catch (e) {
            this.chipStartTime = null;
        }
    },
    /**
    * 終了時間（予定）
    * @param {String} aEndTime 終了時刻（予定）
    *
    */
    setEndTime: function setEndTime(aEndTime) {
        try {
            if (aEndTime) {
                this.chipEndTime = new Date(aEndTime);
            }
        }
        catch (e) {
            this.chipEndTime = null;
        }
    },
    /**
    * ステータス
    * @param {String} aStatus ステータス
    *
    */
    setStatus: function setStatus(aStatus) {
        try {
            if (aStatus) {
                this.chipStatus = parseInt(aStatus);
            }
        }
        catch (e) {
            this.chipStatus = C_STATUS_NULL;
        }
    },
    /**
    * 実績ステータス
    * @param {String} aResultStatus 実績ステータス
    *
    */
    setResultStatus: function setResultStatus(aResultStatus) {
        if (aResultStatus) {
            this.chipResultStatus = aResultStatus;
        }
    },
    /**
    * 予約_受付納車区分
    *
    * @param {String} aRezReception 予約_受付納車区分
    */
    setRezReception: function setRezReception(aRezReception) {
        if (aRezReception) {
            this.chipRezReception = aRezReception;
        }
    },
    /**
    * 開始時間（実績）
    *
    * @param {String} aResultStartTime 開始時間（実績）
    */
    setResultStartTime: function setResultStartTime(aResultStartTime) {
        try {
            if (aResultStartTime) {
                //this.chipResultStartTime = new Date(exchangeTimeString(aResultStartTime));
                this.chipResultStartTime = new Date(aResultStartTime);
            }
        }
        catch (e) {
            this.chipResultStartTime = null;
        }
    },
    /**
    * 終了時間（実績）
    *
    * @param {String} aResultEndTime 終了時間（実績）
    */
    setResultEndTime: function serResultEndTime(aResultEndTime) {
        try {
            if (aResultEndTime) {
                //this.chipResultEndTime = new Date(exchangeTimeString(aResultEndTime));
                this.chipResultEndTime = new Date(aResultEndTime);
            }
        }
        catch (e) {
            this.chipResultEndTime = null;
        }
    },
    /**
    * 開始時間（実績に基づいた開始予定時間）
    *
    * @param {String} aRezStartTime 開始時間（実績）
    */
    setRezStartTime: function setRezStartTime(aRezStartTime) {
        try {
            if (aRezStartTime) {
                //this.chipResultStartTime = new Date(exchangeTimeString(aResultStartTime));
                this.chipRezStartTime = new Date(aRezStartTime);
            }
        }
        catch (e) {
            this.chipRezStartTime = null;
        }
    },
    /**
    * 終了時間（作業開始時間に基づいた終了予定時間）
    *
    * @param {String} aRezEndTime 終了時間（実績）
    */
    setRezEndTime: function serRezEndTime(aRezEndTime) {
        try {
            if (aRezEndTime) {
                //this.chipResultEndTime = new Date(exchangeTimeString(aResultEndTime));
                this.chipRezEndTime = new Date(aRezEndTime);
            }
        }
        catch (e) {
            this.chipRezEndTime = null;
        }
    },
    /**
    * チップを配置する際の開始時間を設定する.
    *
    */
    setDrawStartTime: function setDrawStartTime() {
        try {
            if (this.chipResultStartTime) {
                this.chipDrawStartTime = this.chipResultStartTime;
            } else if (this.chipStartTime) {
                this.chipDrawStartTime = this.chipStartTime;
            }
            //チップを配置する際の開始時間がストールの作業開始時間より前の場合、ストールの作業開始時間を格納する.
            if ((this.chipDrawStartTime) && (this.stallStartTime)) {
                if (this.chipDrawStartTime < this.stallStartTime) {
                    this.chipDrawStartTime = this.stallStartTime;
                }
            }
        }
        catch (e) {
            this.chipDrawStartTime = null;
        }
    },
    /**
    * チップを配置する際の終了時間を設定する.
    *
    */
    setDrawEndTime: function setDrawEndTime() {
        try {
            if (this.chipResultEndTime) {
                this.chipDrawEndTime = this.chipResultEndTime;
            } else if (this.chipEndTime) {
                this.chipDrawEndTime = this.chipEndTime;
            }
            //チップを配置する際の終了時間が、ストールの作業終了時間より遅くなる場合、ストールの作業終了時間を格納する.
            if ((this.chipDrawEndTime) && (this.stallEndTime)) {
                if (this.chipDrawEndTime > this.stallEndTime) {
                    this.chipDrawEndTime = this.stallEndTime;
                }
            }
        }
        catch (e) {
            this.chipDrawEndTime = null;
        }
    },
    /**
    * 入庫日時
    * @param {String} aStrDate
    */
    setStrDate: function setStrDate(aStrDate) {
        try {
            var strDateParseValue = Date.parse(aStrDate);
            if (strDateParseValue != C_DATE_MIN_VALUE) {
                this.chipStrDate = new Date(aStrDate);
            }
            else {
                this.chipStrDate = null;
            }
        }
        catch (e) {
            this.chipStrDate = null;
        }
    },
    /**
    * サービスコード
    * @param {String} aServiceCode
    */
    setServiceCode: function setServiceCode(aServiceCode) {
        if (aServiceCode) {
            this.chipServiceCode = aServiceCode;
        }
    },
    /**
    * 来店フラグ
    * @param {String} aWalkIn
    */
    setWalkIn: function setWalkIn(aWalkIn) {
        if (aWalkIn) {
            this.chipWalkIn = aWalkIn;
        }
    },
    /**
    * 車両番号
    * @param {String} aVclRegNo
    */
    setVclRegNo: function setVclRegNo(aVclRegNo) {
        if (aVclRegNo) {
            this.chipVclRegNo = aVclRegNo;
        }
    },
    /**
    * 更新カウンタ
    * @param {String} aUpdateCount
    */
    setUpdateCount: function setUpdateCount(aUpdateCount) {
        try {
            if (aUpdateCount) {
                this.chipUpdateCount = aUpdateCount;
            }
        }
        catch (e) {
            this.chipUpdateCount = 0;
        }
    },
    /**
    * OrderNumber
    * @param {String} aOrderNumber
    */
    setOrderNumber: function setOrderNumber(aOrderNumber) {
        try {
            if (aOrderNumber) {
                //空白文字を除去して格納する.
                this.orderNumber = trimString(aOrderNumber);
            }
        }
        catch (e) {
            this.orderNumber = "";
        }
    },
    /**
    * 子番号
    * @param {String} aChildNumber
    */
    setChildNumber: function setChildNumber(aChildNumber) {
        try {
            if (aChildNumber) {
                this.childNumber = parseInt(aChildNumber);
                if (this.childNumber < 0 || isNAN(this.childNumber)) {
                    this.childNumber = 0;
                }
            }
        }
        catch (e) {
            this.childNumber = 0;
        }
    },
    /**
    * 定期点検の距離
    * @param {String} aDistance
    */
    setDistance: function setDistance(aDistance) {
        try {
            if (aDistance) {
                this.distance = aDistance;
            }
        }
        catch (e) {
            this.distance = "";
        }
    },
    /**
    * 定期点検の距離単位
    * @param {String} aDistanceUnit
    */
    setDistanceUnit: function setDistanceUnit(aDistanceUnit) {
        try {
            if (aDistanceUnit) {
                this.distanceUnit = aDistanceUnit;
            }
        }
        catch (e) {
            this.distanceUnit = "";
        }
    },
    //2012/02/27 KN 渡辺 KN_0225_HH_2_結合テスト（現地）課題票の修正 Start
    /**
    * 入庫予定時間
    * @param {String} aCrryInTime
    */
    setChipCrryInTime: function setCrryInTime(aCrryInTime) {
        try {
            if (aCrryInTime) {
                this.chipCrryInTime = new Date(aCrryInTime);
            }
        }
        catch (e) {
            this.chipCrryInTime = null;
        }
    },
    //2012/02/27 KN 渡辺 KN_0225_HH_2_結合テスト（現地）課題票の修正 End

    // 2012/06/01 KN 西田 STEP1 重要課題対応 START
    /*
    * 着工指示区分
    * @param {String} aInstruct
    */
    setInstruct: function setInstruct(aInstruct) {
        try {
            if (aInstruct) {
                this.instruct = aInstruct;
            }
        }
        catch (e) {
            this.instruct = "";
        }
    },

    /*
    * 作業連番
    * @param {Integer} aWorkSeq
    */
    setWorkSeq: function setWorkSeq(aWorkSeq) {
        try {
            if (aWorkSeq) {
                this.workSeq = aWorkSeq;
            }
        }
        catch (e) {
            this.workSeq = 0;
        }
    },

    /*
    * 部品準備完了フラグ
    * @param {Integer} aMerchandiseFlag
    */
    setMerchandiseFlag: function setMerchandiseFlag(aMerchandiseFlag) {
        try {
            if (aMerchandiseFlag) {
                this.merchandiseFlag = aMerchandiseFlag;
            }
        }
        catch (e) {
            this.merchandiseFlag = 0;
        }
    },
    // 2012/06/01 KN 西田 STEP1 重要課題対応 END

    // 2012/11/29 TMEJ 河原 【A. STEP2】SA ストール予約受付機能開発 仕分けNO.74対応 START
    /*
    * フラグ
    * @param {Integer} aMerchandiseFlag
    */
    setinspectionReqFlag: function setinspectionReqFlag(aInspectionReqFlag) {
        try {
            if (aInspectionReqFlag) {
                this.inspectionReqFlag = aInspectionReqFlag;
            }
        }
        catch (e) {
            this.inspectionReqFlag = 0;
        }
    },
    // 2012/11/29 TMEJ 河原 【A. STEP2】SA ストール予約受付機能開発 仕分けNO.74対応 END

    //13/08/08 TMEJ 成澤 【A.STEP2】タブレット版SMB開発に向けた要件定義 START
    /*
    * ストール利用ステータス
    * @param {Integer} aStallUseStatus
    */
    setStallUseStatus: function setStallUseStatus(aStallUseStatus) {
        try {
            if (aStallUseStatus) {
                this.stallUseStatus = aStallUseStatus;
            }
        }
        catch (e) {
            this.stallUseStatus = 0;
        }
    },
    //13/08/08 TMEJ 成澤 【A.STEP2】タブレット版SMB開発に向けた要件定義 END

    //2013/11/26 TMEJ 成澤【IT9573】次世代e-CRBサービス 店舗展開に向けた標準作業確立 START
    /*
    * 販売店コード
    * @param {String} aStallUseStatus
    */
    setDealerCode: function setDealerCode(aDealerCode) {
        try {
            if (aDealerCode) {
                this.dealerCode = aDealerCode;
            }
        }
        catch (e) {
            this.dealerCode = 0;
        }
    }
    //2013/11/26 TMEJ 成澤【IT9573】次世代e-CRBサービス 店舗展開に向けた標準作業確立 END
}