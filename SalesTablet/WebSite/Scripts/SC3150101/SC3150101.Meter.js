/**
 * @fileOverview 作業進捗メータクラスを記述するファイル.
 *
 * @author KN 渡辺 憲治
 * @version 1.0.0
 */


//定数
/**
 * 作業進捗メータの1メモリの表示幅
 */
var C_METER_SCALE_WIDTH = 37;

/**
 * 作業進捗メータの最大メモリ数
 */
var C_MAX_METER_LEVEL = 10;

/**
 * 進捗メータの新規生成時の直前のメータメモリ
 */
var C_INIT_LAST_METER_LEVEL = -1;



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
    * 最後に更新されたときのメータレベル
    * @return {Integer}
    */
    this.lastMeterLevel = C_INIT_LAST_METER_LEVEL;
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
    setMeterParameter: function setMeterParameter(aSTime, aETime, aRONo, aResultSTime, aResultETime, aResultStatus) {
        this.meterStartTime = aSTime;
        this.meterEndTime = aETime;
        this.meterRONo = aRONo;
        this.chipResultStartTime = aResultSTime;
        this.chipResultEndTime = aResultETime;
        this.chipResultStatus = aResultStatus;
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
            $("#LabelRONumber").text(this.meterRONo);
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
            }
            $("#LiteralStartTimeText").text(startTimeWord);
            $("#LiteralEndTimeText").text(endTimeWord);

            //作業情報を更新する.
            $("#LabelMeterStartTime").text(formatTime(this.meterStartTime.getHours()) + ":" + formatTime(this.meterStartTime.getMinutes()));
            $("#LabelMeterEndTime").text(formatTime(this.meterEndTime.getHours()) + ":" + formatTime(this.meterEndTime.getMinutes()));

            //作業進捗メーターを描画する.
            this.drawMeter();

            //            //選択中のチップIDと作業対象チップIDが合致する場合、グレーフィルタの透過度を0にする.
            //            if (checkSelectedIsCandidateId()) {
            //                $("#stc02Box02Filter").css("opacity", C_FILTER_CLEAR);
            //            } else {
            //                $("#stc02Box02Filter").css("opacity", C_FILTER_TRANSLUCENT);
            //            }
            //チップの実績ステータスが、待機中・作業中の場合、スモークフィルタをクリアする.
            if ((this.chipResultStatus == C_RESULT_STATUS_WAIT) || (this.chipResultStatus == C_RESULT_STATUS_WORKING)) {
                $("#stc02Box02Filter").css("opacity", C_FILTER_CLEAR);
            } else {
                $("#stc02Box02Filter").css("opacity", C_FILTER_TRANSLUCENT);
            }
        }
        catch (e) {
            //例外発生時、グレーフィルタを設定する.
            $("#stc02Box02Filter").css("opacity", C_FILTER_TRANSLUCENT);
        }
    },


    /**
    * 作業進捗メータを描画する.
    *
    * @return {void}
    *
    */
    drawMeter: function drawMeter() {

        var _materLevel = 0;

        //チップの実績ステータスが、作業中・実績の場合メータを描画する.
        //if (checkSelectedIsCandidateId()) {
        if ((this.chipResultStatus == C_RESULT_STATUS_WORKING) || (this.chipResultStatus == C_RESULT_STATUS_COMPLETION)) {

            //チップの実績終了時刻とチップの実績開始時刻の差を取得し、メーターの1メモリの時間を算出する.
            var _diffTime = this.chipResultEndTime - this.chipResultStartTime;
            var _materPitch = _diffTime / C_MAX_METER_LEVEL;

            //チップの実績開始時刻と現在時刻との差より、表示するメーターのメモリ数を算出する.
            if (_materPitch > 0) {
                _materLevel = Math.floor((getServerTimeNow() - this.chipResultStartTime) / _materPitch);
            }
            if (_materLevel < 0) {
                _materLevel = 0;
            }
            //            //作業終了予定時刻と作業開始予定時刻の差を取得し、メーターの1メモリの時間を算出する.
            //            var _diffTime = this.meterEndTime - this.meterStartTime;
            //            var _materPitch = _diffTime / C_MAX_METER_LEVEL;

            //            //作業開始予定時刻と現在時刻との差より、表示するメーターのメモリ数を算出する.
            //            if (_materPitch > 0) {
            //                _materLevel = Math.floor((getServerTimeNow() - this.meterStartTime) / _materPitch);
            //            }
            //            if (_materLevel < 0) {
            //                _materLevel = 0;
            //            }
        }

        //格納されている前回のメーターのメモリ数と、今回のメモリ数を比較して違いがある場合、
        //作業進捗メーターに現在の時間経過状況を表示する.
        //前回のメーターのメモリ数を更新する.
        if (_materLevel != this.lastMeterLevel) {
            var _materDrawWidth = _materLevel * C_METER_SCALE_WIDTH;
            $("#MeterColor").css("width", _materDrawWidth.toString() + "px");
            this.lastMeterLevel = _materLevel;
        }
    }
}