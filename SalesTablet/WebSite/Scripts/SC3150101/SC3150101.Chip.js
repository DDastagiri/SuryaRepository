//---------------------------------------------------------
//SC3150101.Chip.js
//---------------------------------------------------------
//機能：TCメインメニュー_予約チップクラス
//補足：
//作成：2012/01/30 KN 渡辺
//更新：2012/02/27 KN 渡辺 KN_0225_HH_2_結合テスト（現地）課題票の修正
//---------------------------------------------------------

//ステータス
var C_STATUS_NULL = 0;   //ステータスなし
var C_STATUS_REZ_FIX = 1; //ストール本予約
var C_STATUS_REZ_TEMP = 2;    //ストール仮予約
var C_STATUS_UNAVAILABLE = 3; //使用不可
var C_STATUS_DELIVERY = 4;        //引取・納車
var C_STATUS_REST = 99;       //休憩

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

//サービスコード
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
    //this.strCancelFlag = null;
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
        this.setRezStartTime(aDataSet.REZ_START_TIME);
        this.setRezEndTime(aDataSet.REZ_END_TIME);
        this.setDrawStartTime();    //チップを配置する開始時間
        this.setDrawEndTime();      //チップを配置する終了時間
        this.setStrDate(aDataSet.STRDATE);  //入庫日時
        this.setServiceCode(aDataSet.SERVICECODE);  //サービスコード
        this.setWalkIn(aDataSet.WALKIN);    //来店フラグ
        this.setVclRegNo(aDataSet.VCLREGNO);    //車両番号
        this.setUpdateCount(aDataSet.UPDATECOUNT);  //チップの更新カウンタ
        this.setOrderNumber(aDataSet.ORDERNO);  //OrderNumber
        this.setChildNumber(aDataSet.REZCHILDNO);   //子番号
        this.setDistance(aDataSet.SVCORGNMCT); //定期点検距離
        this.setDistanceUnit(aDataSet.SVCORGNMCB); //定期点検距離の単位
        //2012/02/27 KN 渡辺 KN_0225_HH_2_結合テスト（現地）課題票の修正 Start
        this.setChipCrryInTime(aDataSet.CRRYINTIME);    //入庫予定時間
        //2012/02/27 KN 渡辺 KN_0225_HH_2_結合テスト（現地）課題票の修正 End
    },


    //チップ情報の更新時間を設定する
    setUpdateTime: function setUpdateTime(dtmUPTime) {
        this.dtmUpdateTime = dtmUPTime;
    },


    //生成するチップ種別を設定（併せて、タップの有効フラグを設定する）
    setChipColor: function setChipColor() {

        //ステータスに応じて、チップ種別を決定する
        if (this.chipStatus == C_STATUS_REZ_TEMP) {
            this.chipColor = C_CHIP_REZ_TEMP;
            this.chipTapFlag = true;
        } else if (this.chipStatus == C_STATUS_UNAVAILABLE) {
            this.chipColor = C_CHIP_UNAVAILABLE;
        } else if (this.chipStatus == C_STATUS_DELIVERY) {
            this.chipColor = C_CHIP_DELIVERY;
            this.chipTapFlag = true;
        } else if (this.chipStatus == C_STATUS_REST) {
            this.chipColor = C_CHIP_REST;
        } else if (this.chipStatus == C_STATUS_REZ_FIX) {
            //ストール本予約の場合、遅れ検出、実績ステータスのチェックを行い、チップ種別を確定する
            //まずは、実績チップかを判定する（作業完了しているか）
            if (this.chipResultStatus == C_RESULT_STATUS_COMPLETION) {
                this.chipColor = C_CHIP_COMPLETION;
                this.chipTapFlag = true;
            }
            //遅れチップなのか否かを現在時間が、終了時間（予定）を超えているかを判定する
            else if (this.chipEndTime < getServerTimeNow()) {
                this.chipColor = C_CHIP_DELAY;
                this.chipTapFlag = true;
            } else {
                this.chipColor = C_CHIP_REZ_FIX;
                this.chipTapFlag = true;
            }
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
        objChipLowerArea.append(this.createLowerArea02());
        objChipLowerArea.append(this.createLowerArea03());
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

        //実績ステータスが作業待ちの場合のみ入庫状態を描画する
        if (this.chipResultStatus == C_RESULT_STATUS_WAIT) {
            //入庫日時が指定されている場合、入庫済みアイコンを配置する
            if (this.chipStrDate) {
                //チップ種別、予約_受付納車区分より、入庫済みアイコンとするCSSクラスを追加する
                objChipAddIcon02 = $("<div />").addClass("AddIcon02");
                objChipAddIcon02.addClass("CrryIn" + this.chipColor + this.chipRezReception);
            }

            //2012/02/27 KN 渡辺 KN_0225_HH_2_結合テスト（現地）課題票の修正 Start
            //入庫日時が指定されていない場合、入庫前のクラスを設定する
            else {
                objChipAddIcon02 = $("<div />").addClass("AddIcon02Text");

                var crryInTime = new Date();
                if (this.chipCrryInTime) {
                    crryInTime = this.chipCrryInTime;
                }
                //入庫予定時間が存在しない場合、開始予定時間を入庫予定時間とする.
                else {
                    crryInTime = this.chipStartTime;
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
                    crryInTimeText += formatTime(crryInTime.getMonth());
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
            if ((this.chipResultStartTime != null) && (this.chipResultStartTime != NaN) && (this.chipResultStartTime != "Invalid Date")) {
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
            //終了時間（実績）が存在する場合、終了時間を表示する
            if ((this.chipResultEndTime != null) && (this.chipResultEndTime != NaN) && (this.chipResultEndTime != "Invalid Date")) {
                objChipAddIcon02_li03 = $("<li />").addClass("time");
                //終了時間（実績）が終了時間（予定）よりも大きい場合、文字色を赤に設定する
                if (this.chipResultEndTime > this.chipRezEndTime) {
                    objChipAddIcon02_li03.addClass("TextColorDelay");
                }
                objChipAddIcon02_li03.text(formatTime(this.chipResultEndTime.getHours()) + ":" + formatTime(this.chipResultEndTime.getMinutes()));
                objChipAddIcon02.append(objChipAddIcon02_li03);
            }

        }

        return objChipAddIcon02;
    },


    //作業種別部分を描画するタグ要素を生成し、返す
    createLowerArea03: function createLowerArea03() {
        var objChipAddIcon03;

        //定期点検距離がブランクでない場合、定期点検を表示する.
        if (this.distance != "") {
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
        //ステータスに引取納車が設定されている場合、引取or納車のアイコンを表示する
        else if (this.chipStatus == C_STATUS_DELIVERY) {
            objChipAddIcon03 = $("<div />").addClass("AddIcon03");
        }
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
        }
        //使用不可チップを生成する
        else if (this.chipColor == C_CHIP_UNAVAILABLE) {
            this.createChipUnavailable();
        }
        //その他・休憩・使用不可以外のチップを生成する
        else {
            this.createWorkChip();
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
    setChipFilter: function setChipFilter(blnSelected) {

        //チップのCSSクラスをフィールド値にしたがって描画し、チップを再配置する
        //選択中フラグがtrueの場合、背景にライム色を指定
        if (this.chipTapFlag) {
            if (blnSelected) {
                this.objChipsFilter.css("opacity", C_FILTER_CLEAR);
            } else {
                this.objChipsFilter.css("opacity", C_FILTER_TRANSLUCENT);
            }
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
                this.rezId = parseInt(aRezId);
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
                this.dSeqNo = parseInt(aDSeqNo);
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
                this.seqNo = parseInt(aSeqNo);
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
                this.chipUpdateCount = parseInt(aUpdateCount);
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
                //this.childNumber = parseInt(aChildNumber);
                this.childNumber = parseInt(aChildNumber) - 1;
                if (this.childNumber <= 0) {
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
    }
    //2012/02/27 KN 渡辺 KN_0225_HH_2_結合テスト（現地）課題票の修正 End
}