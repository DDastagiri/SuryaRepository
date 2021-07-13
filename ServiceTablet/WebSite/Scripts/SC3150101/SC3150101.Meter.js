/**
 * @fileOverview 作業進捗メータクラスを記述するファイル.
 *
 * @author KN 渡辺 憲治
 * @version 1.0.0
 *
 * 更新: 2014/08/29 TMEJ 成澤 【開発】IT9737_NextSTEPサービス ロケ管理の効率化に向けた評価用アプリ作成
 * 更新:
 */


//定数

/**
 * チップ選択がなされている状態を示す
 */
var C_SELECTED_CHIP_ON = "1";

/**
* R/O No.の枝番表示時に使用する固定値
*/
var C_RONO_SUB_CHAR = "-";

/**
* 作業進捗メータを描画する.
*
* @class 作業進捗メータの描画クラス.
* 作業進捗メータの描画に必要な情報を所持し、それを取り扱い、作業進捗メータを描画する機能を保有する.
*
*/
function workMeter() {

    /**
     * 作業開始予定時刻
     * @return {Date}
     */
    this.meterStartTime = new Date();

    /**
     * 作業終了時刻
     * @return {Date}
     */
    this.meterEndTime = new Date();

    /**
     * R/O No.
     * @return {String}
     */
    this.meterRONo = "";

    /**
     * 担当SA名
     * @return {String}
     */
    this.meterSAName = "";

    /**
     * チップの実績開始時刻
     * @return {Date}
     */
    this.chipResultStartTime = new Date();

    /**
     * チップの実績終了時刻
     * @return {Date}
     */
    this.chipResultEndTime = new Date();

    /**
     * チップの実績ステータス
     * @return {String}
     */
    this.chipResultStatus = C_RESULT_STATUS_WAIT;

    /**
    * R/O Noの枝番(TACT)
    * @return {string}
    */
    this.meterSrvAddSeq = "";
}


workMeter.prototype = {
    /**
    * 作業進捗メータクラスのパラメータを設定する.
    *
    * @param {Date} aSTime 作業開始予定時刻
    * @param {Date} aETime 作業終了予定時刻
    * @param {String} aRONo R/O番号
    * @param {Date} aResultSTime チップの実績開始時刻
    * @param {Date} aResultETime チップの実績終了時刻
    * @param {String} aResultStatus チップの実績ステータス
    * @return {void}
    *
    */
    setMeterParameter: function setMeterParameter(aSTime, aETime, aRONo, aResultSTime, aResultETime, aResultStatus, aLocstionNumber) {
        this.meterStartTime = aSTime;
        this.meterEndTime = aETime;
        this.meterRONo = aRONo;
        this.chipResultStartTime = aResultSTime;
        this.chipResultEndTime = aResultETime;
        this.chipResultStatus = aResultStatus;
        //2014/08/29 TMEJ 成澤 【開発】IT9737_NextSTEPサービス ロケ管理の効率化に向けた評価用アプリ作成　START
        this.chipLocstionNumber = aLocstionNumber;
        //2014/08/29 TMEJ 成澤 【開発】IT9737_NextSTEPサービス ロケ管理の効率化に向けた評価用アプリ作成　END
    },

    setMaterParameterSrvAddSeq: function setMaterParameterSrvAddSeq(srvAddSeq) {
        this.meterSrvAddSeq = srvAddSeq;
    },

    /**
    * 作業進捗メータの担当SA名パラメータを設定する.
    * @param {String} aSAName 担当SA名
    */
    setMeterParameterSaName: function setMeterParameterSaName(aSAName) {
        this.meterSAName = aSAName;
    },


    /**
    * 作業進捗メータの更新処理を行う.
    *
    * @return {void}
    *
    */
    refreshMeter: function refreshMeter() {

        try {
            //リペアオーダ番号・担当SA名を更新する
            if (this.meterSrvAddSeq == "") {
                $("#LabelRONumber").text(this.meterRONo);
            }
            else {
                $("#LabelRONumber").text(this.meterRONo + C_RONO_SUB_CHAR + this.meterSrvAddSeq);
            }
            $("#LabelChargeSA").text(this.meterSAName);

            //作業開始時刻・終了時刻の文言を更新する.
            var startTimeWord = $("#HiddenStartTimeWord").val();
            var endTimeWord = $("#HiddenEndTimeWord").val();
            //実績ステータスが作業中の場合、作業開始時刻の文言を変更する.
            if ((this.chipResultStatus == C_RESULT_STATUS_WORKING) || (this.chipResultStatus == C_RESULT_STATUS_COMPLETION)) {
                startTimeWord = $("#HiddenResultStartTimeWord").val();
                //実績ステータスが、作業完了の場合、作業終了時刻の文言を変更する.
                if (this.chipResultStatus == C_RESULT_STATUS_COMPLETION) {
                    endTimeWord = $("#HiddenResultEndTimeWord").val();
                }
            //2014/08/29 TMEJ 成澤 【開発】IT9737_NextSTEPサービス ロケ管理の効率化に向けた評価用アプリ作成　START
            }else{
                $("#locationLabel").text(this.chipLocstionNumber);
            }
            //2014/08/29 TMEJ 成澤 【開発】IT9737_NextSTEPサービス ロケ管理の効率化に向けた評価用アプリ作成　END

            $("#LiteralStartTimeText").text(startTimeWord);
            $("#LiteralEndTimeText").text(endTimeWord);


            //作業情報を更新する.
            //いずれかの作業チップを選択している場合のみ時間を表示
            var _selectedChipStatus = $("#HiddenSelectedChip").val();
            if (_selectedChipStatus == C_SELECTED_CHIP_ON) {
                $("#LabelMeterStartTime").text(formatTime(this.meterStartTime.getHours()) + ":" + formatTime(this.meterStartTime.getMinutes()));
                $("#LabelMeterEndTime").text(formatTime(this.meterEndTime.getHours()) + ":" + formatTime(this.meterEndTime.getMinutes()));
            } else {
                $("#LabelMeterStartTime").text(" ");
                $("#LabelMeterEndTime").text(" ");
            }

            //チップの実績ステータスが、待機中・作業中の場合、スモークフィルタをクリアする.
            if ((this.chipResultStatus == C_RESULT_STATUS_WAIT) || (this.chipResultStatus == C_RESULT_STATUS_WORKING)) {
                $(".stc01Box02").css("opacity", 1);     // 作業進捗エリアの透明度を完全に不透明にする。(→裏にあるグレーフィルターを表示する。)
            } else {
                $(".stc01Box02").css("opacity", C_FILTER_TRANSLUCENT);
            }
        }
        catch (e) {
            //例外発生時、グレーフィルタを設定する.
            $(".stc01Box02").css("opacity", C_FILTER_TRANSLUCENT);
        }
    },
}
