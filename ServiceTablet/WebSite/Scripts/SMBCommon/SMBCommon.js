/** 
* @fileOverview SMBCommon.js
* 
* @author TMEJ 明瀬
* @version 1.0.0
* 更新： 2013/12/26 TMEJ 下村 次世代e-CRBタブレット(サービス) iOS7.0 VersionUp対応
* 更新： 2014/04/01 TMEJ 丁 タブレット版SMB チーフテクニシャン機能開発
*/

var smbScript = {};

//半角数字のみの正規表現パターン
var C_NUMPATTERN = "^[0-9]+$";

//HH:mmの正規表現パターン
var C_TIMEPATTERN = "^([01]?[0-9]|2[0-3]):([0-5][0-9])$";

//parseIntで使用する基数
var C_RADIX = "10";

//1日(1440分)のミリ秒
var C_1DAYMILLI = 86400000;

/******************************
* RoundUp系関数
******************************/ 

/**
* 数値の丸め込みを行う。<br>
* 
* @param {String} src 丸め込み対象の数値文字列
* @param {String} num 丸め込み基準の数値文字列(整数も可)
* @param {String} minnum 丸め込み後の最小値
* @param {String} maxnum 丸め込み後の最大値
* @return {Integer} 丸め込み後の数値
* 
* @example 
* RoundUpToNumUnits("a", "5", "5", "995");
* 出力:「5」
* RoundUpToNumUnits("-1", "5", "5", "995");
* 出力:「5」
* RoundUpToNumUnits("30", "5", "5", "995");
* 出力:「30」
* RoundUpToNumUnits("42", "5", "5", "995");
* 出力:「45」
*/
smbScript.RoundUpToNumUnits = function (src, num, minnum, maxnum) {

    var rtnVal = 0;
    var intNum = parseInt(num, C_RADIX);
    var intMinNum = parseInt(minnum, C_RADIX);
    var intMaxNum = parseInt(maxnum, C_RADIX);

    //丸め込み対象が数値の場合
    if (smbScript.CheckOnlyHalfWidthDigitFormat(src)) {

        var intSrc = parseInt(src, C_RADIX);

        //丸め込み対象が負の場合
        if (intSrc <= 0) {
            rtnVal = intMinNum;
        }
        //丸め込み対象が丸め込み単位で割り切れる場合
        else if (intSrc % intNum == 0) {
            rtnVal = intSrc;
        }
        //それ以外
        else {
            rtnVal = intSrc + (intNum - (intSrc % intNum));
        }

        //丸め込み後の値が最小値を下回る場合
        if (rtnVal < intMinNum) {
            rtnVal = intMinNum;
        }
        //丸め込み後の値が最大値を超える場合
        else if (rtnVal > intMaxNum) {
            rtnVal = intMaxNum;
        }
    }
    //丸め込み対象が数値以外の場合
    else {
        rtnVal = intMinNum;
    }

    return rtnVal;
}

/**
* HH:mm形式文字列を5分単位で丸め込みを行う。<br>
* 
* @param {String} src 丸め込み対象のyyyy/MM/dd HH:mm形式文字列
* @return {String} 丸め込み後のyyyy/MM/dd HH:mm形式文字列
* 
* @example 
* RoundUpTimeTo5Units("2013/01/01 10:00");
* 出力:「"2013/01/01 10:00"」
* RoundUpTimeTo5Units("2013/01/01 10:06");
* 出力:「"2013/01/01 10:10"」
* RoundUpTimeTo5Units("2013/01/01 10:26");
* 出力:「"2013/01/01 10:30"」
* RoundUpTimeTo5Units("2013/01/01 10:44");
* 出力:「"2013/01/01 10:45"」
* RoundUpTimeTo5Units("2013/01/01 10:55");
* 出力:「"2013/01/01 10:55"」
* RoundUpTimeTo5Units("2013/01/01 23:52");
* 出力:「"2013/01/01 23:55"」
*/
smbScript.RoundUpTimeTo5Units = function (src) {

    var rtnVal;
    var time;
    var countUpDay = false;

    //2013/12/26 TMEJ 下村 次世代e-CRBタブレット(サービス) iOS7.0 VersionUp対応 START
    //var date = new Date(src);
    var date;
    //引数がStringだったらDateに変換する
    if (typeof src == "string") date = new Date(src);
    else date = src;
    //2013/12/26 TMEJ 下村 次世代e-CRBタブレット(サービス) iOS7.0 VersionUp対応 END

    var countUpdate = new Date();
    var roundUpDate;
    var roundUpDateMilli;

    var hh = date.getHours();
    var mm = date.getMinutes();

    //mmが00の場合
    if (mm == 0) {
        time = String(hh) + ":" + "00";
    }
    //mmが01～05の場合
    else if (mm <= 5) {
        time = String(hh) + ":" + "05";
    }
    //mmが05～10の場合
    else if (mm <= 10) {
        time = String(hh) + ":" + "10";
    }
    //mmが10～15の場合
    else if (mm <= 15) {
        time = String(hh) + ":" + "15";
    }
    //mmが16～20の場合
    else if (mm <= 20) {
        time = String(hh) + ":" + "20";
    }
    //mmが21～25の場合
    else if (mm <= 25) {
        time = String(hh) + ":" + "25";
    }
    //mmが26～30の場合 
    else if (mm <= 30) {
        time = String(hh) + ":" + "30";
    }
    //mmが31～35の場合
    else if (mm <= 35) {
        time = String(hh) + ":" + "35";
    }
    //mmが36～40の場合
    else if (mm <= 40) {
        time = String(hh) + ":" + "40";
    }
    //mmが41～45の場合
    else if (mm <= 45) {
        time = String(hh) + ":" + "45";
    }
    //mmが46～50の場合
    else if (mm <= 50) {
        time = String(hh) + ":" + "50";
    }
    //mmが51～55の場合
    else if (mm <= 55) {
        time = String(hh) + ":" + "55";
    }
    //mmが56～59の場合
    else if (mm >= 56) {
        var newHh = hh + 1;
        //HHが24を超えた場合
        if (newHh >= 24) {
            //time = "23" + ":" + "55";
            time = "00" + ":" + "00";
            countUpDay = true;
        }
        else {
            time = String(newHh) + ":" + "00";
        }
    }

    //yyyy/MM/dd HH:mm形式の文字列を返却
    //HHが24を超えない場合
    if (countUpDay == false) {
        rtnVal = date.getFullYear() + "/" + (date.getMonth() + 1) + "/" + date.getDate() + " " + time;
    } else {
        //1日プラスする
        roundUpDate = new Date(date.getFullYear() + "/" + (date.getMonth() + 1) + "/" + date.getDate() + " " + time);
        roundUpDateMilli = roundUpDate.getTime() + C_1DAYMILLI;
        countUpdate.setTime(roundUpDateMilli);
        rtnVal = countUpdate.getFullYear() + "/" + (countUpdate.getMonth() + 1) + "/" + countUpdate.getDate() + " " + time;
    }

    return rtnVal;
}

/******************************
* Calc系関数
******************************/ 

/**
* 日付の時間差分(単位:分)を計算する。<br>
* 
* @param {String or Date} argStartDateTime yyyy/MM/dd HH:mm形式文字列、またはDate値(開始日時)
* @param {String or Date} argEndDateTime   yyyy/MM/dd HH:mm形式文字列、またはDate値(終了日時)
* @return {Integer} 差分時間(単位:分)
* 
* @example 
* CalcTimeSpan("2013/01/01 10:30", "2013/01/01 10:00");
* 出力:「null」
* CalcTimeSpan("2013/01/01 10:00", "2013/01/01 10:30");
* 出力:「30」
*/
smbScript.CalcTimeSpan = function (argStartDateTime, argEndDateTime) {

    var rtnVal;

    var startDateTime;
    var endDateTime;

    //引数がStringだったらDateに変換する
    if (typeof argStartDateTime == "string") startDateTime = new Date(argStartDateTime);
    else startDateTime = argStartDateTime;

    if (typeof argEndDateTime == "string") endDateTime = new Date(argEndDateTime);
    else endDateTime = argEndDateTime;

    //終了日時ミリ秒から開始日時ミリ秒の差分を計算
    var timeSpan = endDateTime.getTime() - startDateTime.getTime();

    //差分が正の値
    if (0 < timeSpan) {
        //ミリ秒を分に直して返却
        rtnVal = timeSpan / 1000 / 60;
    }
    //差分が負の値
    else {
        rtnVal = null;
    }

    return rtnVal;
}

/**
* 日付の時間差分(単位:分)を計算する。<br>
* 営業時間外の時間帯は減算して計算する。<br>
* 
* @param {String or Date} argStartDateTime yyyy/MM/dd HH:mm形式文字列、またはDate値(開始日時)
* @param {String or Date} argEndDateTime   yyyy/MM/dd HH:mm形式文字列、またはDate値(終了日時)
* @return {Integer} 差分時間(単位:分)
* 
* @example 
* CalcTimeSpan("2013/01/01 10:30", "2013/01/01 10:00");
* 出力:「null」
* CalcTimeSpan("2013/01/01 10:00", "2013/01/01 10:30");
* 出力:「30」
*/
smbScript.CalcTimeSpan_ExcludeOutOfTime = function (argStartDateTime, argEndDateTime) {

    var rtnVal;

    var startDateTime;
    var endDateTime;

    var dtStallStartTime;          //営業開始日時
    var dtStallEndTime;            //営業終了日時
    var dtNextStallStartTime;      //翌日の営業開始日時
    var StallIdleMinute;           //営業時間外（分）

    //引数がStringだったらDateに変換する
    if (typeof argStartDateTime == "string") startDateTime = new Date(argStartDateTime);
    else startDateTime = argStartDateTime;

    if (typeof argEndDateTime == "string") endDateTime = new Date(argEndDateTime);
    else endDateTime = argEndDateTime;

    //営業開始日時
    //dtStallStartTime = new Date($("#hidShowDate").val() + " " + $("#hidStallStartTime").val() + ":00");
    dtStallStartTime = new Date(startDateTime.getFullYear() + "/" + (startDateTime.getMonth() + 1) + "/" + startDateTime.getDate() + " " + $("#hidStallStartTime").val() + ":00");

    //営業終了日時
    //dtStallEndTime = new Date($("#hidShowDate").val() + " " + $("#hidStallEndTime").val() + ":00");
    dtStallEndTime = new Date(startDateTime.getFullYear() + "/" + (startDateTime.getMonth() + 1) + "/" + startDateTime.getDate() + " " + $("#hidStallEndTime").val() + ":00");

    //翌日の営業開始日時（本日の営業開始日時に1440分(1日)を足して算出）
    dtNextStallStartTime = smbScript.CalcEndDateTime(dtStallStartTime, 1440);

    //ストールの営業時間外(分)　※翌日の営業開始日時 － 本日の営業終了日時 で算出
    StallIdleMinute = smbScript.CalcTimeSpan(dtStallEndTime, dtNextStallStartTime);

    //終了日時ミリ秒から開始日時ミリ秒の差分を計算
    var timeSpan = endDateTime.getTime() - startDateTime.getTime();

    //差分が正の値
    if (0 < timeSpan) {
        //ミリ秒を分に直して返却
        //rtnVal = timeSpan / 1000 / 60;

        //開始日時の日付部分のみでミリ秒を計算
        var wkStartDateTime = new Date(startDateTime.getFullYear() + "/" + (startDateTime.getMonth() + 1) + "/" + startDateTime.getDate() + " " + "00:00");
        var startMilli = wkStartDateTime.getTime();

        //終了日時の日付部分のみでミリ秒を計算
        var wkEndDateTime = new Date(endDateTime.getFullYear() + "/" + (endDateTime.getMonth() + 1) + "/" + endDateTime.getDate() + " " + "00:00");
        var endMilli = wkEndDateTime.getTime();

        //営業時間外を跨いだ回数を算出
        var countOutOfTime = (endMilli - startMilli) / C_1DAYMILLI;

        //「営業時間外を考慮せずに算出した"分"」に、「跨いだ営業時間外の"分"」を減算して返す
        rtnVal = (timeSpan / 1000 / 60) - (StallIdleMinute * countOutOfTime);
    }
    //差分が負の値
    else {
        rtnVal = null;
    }

    return rtnVal;
}


///**
//* HH:mm形式の開始時間と加算時間を元に、終了時間を計算する。<br>
//* 
//* @param {String} startTime HH:mm形式文字列(開始時間)
//* @param {Integer} timeSpan 開始時間に足す時間(単位:分)
//* @return {Integer} HH:mm形式文字列(計算後の終了時間)
//* 
//* @example 
//* CalcEndTime("123aa", 60);
//* 出力:「null」
//* CalcEndTime("10:00", 60);
//* 出力:「"11:00"」
//* CalcEndTime("23:00". 60);
//* 出力:「"0:00"」
//*/
//smbScript.CalcEndTime = function (startTime, timeSpan) {

//    var rtnVal;

//    //開始時間がHH:mmのフォーマットの場合
//    if (smbScript.CheckTimeFormat(startTime)) {
//        var sTime = startTime.split(":");
//        var startHours = parseInt(sTime[0], C_RADIX);
//        var startMinutes = parseInt(sTime[1], C_RADIX);
//        var totalMinutes = (startHours * 60) + startMinutes + parseInt(timeSpan, C_RADIX);

//        var endTimeHours = Math.floor(totalMinutes / 60);
//        var endTimeMinutes = totalMinutes % 60;

//        //24時以上になってしまった場合
//        if (endTimeHours > 23) {
//            endTimeHours -= 24;
//        }
//        rtnVal = String(endTimeHours) + ":" + smbScript.PadLeft(String(endTimeMinutes), "0", 2);
//    }
//    else {
//        rtnVal = null;
//    }

//    return rtnVal;
//}


/**
* 開始日時と加算時間を元に、終了日時を計算する。<br>
* 
* @param {String or Date} argStartDateTime yyyy/MM/dd HH:mm形式文字列、またはDate値(開始日時)
* @param {Integer} timeSpan 開始時間に足す時間(単位:分)
* @return {Date} 計算後の終了日時
* 
* @example 
* CalcEndTime("2013/01/01 10:00", 60);
* 出力:「2013/01/01 11:00のDate型」
* CalcEndTime("2013/01/01 23:00", 60);
* 出力:「2013/01/02 0:00のDate型」
*/
smbScript.CalcEndDateTime = function (argStartDateTime, timeSpan) {

    var rtnVal = new Date();

    var startDateTime;

    //開始日時がStringだったらDateに変換する
    if (typeof argStartDateTime == "string") startDateTime = new Date(argStartDateTime);
    else startDateTime = argStartDateTime;

    //それぞれの値をミリ秒に変換する
    var startMilliSeconds = startDateTime.getTime();
    var timeSpanMilliSeconds = parseInt(timeSpan, C_RADIX) * 60 * 1000;

    //変換した値の和をsetTimeしてDate型として終了日時を返却
    rtnVal.setTime(startMilliSeconds + timeSpanMilliSeconds);

    return rtnVal;
}

/**
* 開始日時と加算時間を元に、終了日時を計算する。<br>
* 営業時間外の時間帯は除外して計算する。<br>
* 
* @param {String or Date} argStartDateTime yyyy/MM/dd HH:mm形式文字列、またはDate値(開始日時)
* @param {Integer} timeSpan 開始時間に足す時間(単位:分)
* @return {Date} 計算後の終了日時
* 
* @example 
* 営業時間が 8:00 ～ 23:00 の場合
* CalcEndTime("2013/01/01 10:00", 60);
* 出力:「2013/01/01 11:00のDate型」
* CalcEndTime("2013/07/24 22:00", 90);
* 出力:「2013/07/25 08:30のDate型」
*/
smbScript.CalcEndDate_ExcludeOutOfTime = function (argStartDateTime, timeSpan) {

    var rtnVal = new Date();

    var startDateTime;             //作業開始日時
    var dtStallStartTime;          //営業開始日時
    var dtStallEndTime;            //営業終了日時
    var dtNextStallStartTime;      //翌日の営業開始日時
    var StallIdleMinute;           //営業時間外（分）

    //パラメータの作業開始日時がStringだったらDateに変換する
    if (typeof argStartDateTime == "string") startDateTime = new Date(argStartDateTime);
    else startDateTime = argStartDateTime;

    //営業開始日時
    //dtStallStartTime = new Date($("#hidShowDate").val() + " " + $("#hidStallStartTime").val() + ":00");
    dtStallStartTime = new Date(startDateTime.getFullYear() + "/" + (startDateTime.getMonth() + 1) + "/" + startDateTime.getDate() + " " + $("#hidStallStartTime").val() + ":00");

    //営業終了日時
    //dtStallEndTime = new Date($("#hidShowDate").val() + " " + $("#hidStallEndTime").val() + ":00");
    dtStallEndTime = new Date(startDateTime.getFullYear() + "/" + (startDateTime.getMonth() + 1) + "/" + startDateTime.getDate() + " " + $("#hidStallEndTime").val() + ":00");

    //翌日の営業開始日時（本日の営業開始日時に1440分(1日)を足して算出）
    dtNextStallStartTime = smbScript.CalcEndDateTime(dtStallStartTime, 1440);

    //ストールの営業時間外（分）※翌日の営業開始日時 － 本日の営業終了日時 で算出
    StallIdleMinute = smbScript.CalcTimeSpan(dtStallEndTime, dtNextStallStartTime);

    //それぞれの値をミリ秒に変換する
    var startMilli = startDateTime.getTime();                            //作業開始日時ミリ秒
    var timeSpanMilli = parseInt(timeSpan, C_RADIX) * 60 * 1000;         //加算時間ミリ秒
    var stallStartTimeMilli = dtStallStartTime.getTime();                //営業開始日時ミリ秒
    var stallEndTimeMilli = dtStallEndTime.getTime();                    //営業終了日時ミリ秒
    var stallIdleMilli = parseInt(StallIdleMinute, C_RADIX) * 60 * 1000; //ストールの営業時間外ミリ秒

    //作業用
    var wkResultMilli = startMilli;        //計算後の作業終了日時（初期値は作業開始日時からStart）
    var wkTimeSpanMilli = timeSpanMilli;   //残りの作業時間
    var i = 0;

    //残りの作業時間がある場合はLoop
    while (wkTimeSpanMilli > 0) {

        //開始(基点)＋残りの作業時間　＞　営業終了日時　の場合
        if ((wkResultMilli + wkTimeSpanMilli) > stallEndTimeMilli) {

            //この日に割り当てる作業時間＋翌日の営業時間外分を加算し、計算後の基点を明日に進める
            wkResultMilli = wkResultMilli + (stallEndTimeMilli - startMilli) + stallIdleMilli;

            //この日に割り当てる作業時間を差し引き、残りの作業時間を再計算する
            wkTimeSpanMilli = wkTimeSpanMilli - (stallEndTimeMilli - startMilli);

            //営業開始日時を1日分進める(初回Loop時は作業開始日時、2回目以降のLoopでは営業開始日時を基点に使う)
            if (i == 0) {
                startMilli = stallStartTimeMilli + C_1DAYMILLI;
            } else {
                startMilli = startMilli + C_1DAYMILLI;
            }

            //営業終了日時を1日分進める
            stallEndTimeMilli = stallEndTimeMilli + C_1DAYMILLI;

        } else {

            //残りの作業時間を加算する
            wkResultMilli = wkResultMilli + wkTimeSpanMilli;

            //残りの作業時間を0にしてLoopを抜ける
            wkTimeSpanMilli = 0;
        }
        i = i + 1;
    }

    //変換した値をsetTimeしてDate型として終了日時を返却
    rtnVal.setTime(wkResultMilli);
    return rtnVal;
}

/**
* 営業時間外の日時の場合、翌営業開始時刻に補正する。<br>
* 
* @param {Date} argInDateTime yyyy/MM/dd HH:mm形式文字列、またはDate値(対象日時)
* @return {Date} 補正後の日時
* 
*/
smbScript.GetDateExcludeOutOfTime = function (argInDateTime) {

    var rtnVal;
    var tmpVal;

    var inDateTime;                //対象日時
    var dtStallStartTime;          //営業開始日時
    var dtStallEndTime;            //営業終了日時

    //パラメータの日時がStringだったらDateに変換する
    if (typeof argInDateTime == "string") inDateTime = new Date(argInDateTime);
    else inDateTime = argInDateTime;

    //営業開始日時
    //dtStallStartTime = new Date($("#hidShowDate").val() + " " + $("#hidStallStartTime").val() + ":00");
    dtStallStartTime = new Date(inDateTime.getFullYear() + "/" + (inDateTime.getMonth() + 1) + "/" + inDateTime.getDate() + " " + $("#hidStallStartTime").val() + ":00");

    //営業終了日時
    //dtStallEndTime = new Date($("#hidShowDate").val() + " " + $("#hidStallEndTime").val() + ":00");
    dtStallEndTime = new Date(inDateTime.getFullYear() + "/" + (inDateTime.getMonth() + 1) + "/" + inDateTime.getDate() + " " + $("#hidStallEndTime").val() + ":00");

    //対象日時のhhmmを取得
    var hh = inDateTime.getHours();
    var mm = inDateTime.getMinutes();
    var time = String(smbScript.PadLeft(String(hh), "0", 2)) + String(smbScript.PadLeft(String(mm), "0", 2));
    var intTime = parseInt(time, C_RADIX);

    //営業開始日時のhhmmを取得
    var startHh = dtStallStartTime.getHours();
    var startMm = dtStallStartTime.getMinutes();
    var startTime = String(smbScript.PadLeft(String(startHh), "0", 2)) + String(smbScript.PadLeft(String(startMm), "0", 2));
    var startIntTime = parseInt(startTime, C_RADIX);

    //営業終了日時のhhmmを取得
    var endHh = dtStallEndTime.getHours();
    var endMm = dtStallEndTime.getMinutes();
    var endTime = String(smbScript.PadLeft(String(endHh), "0", 2)) + String(smbScript.PadLeft(String(endMm), "0", 2));
    var endIntTime = parseInt(endTime, C_RADIX);

    //営業時間内の場合
    if ((startIntTime <= intTime) && (intTime <= endIntTime)) {

        //パラメータをそのまま返す
        rtnVal = inDateTime;
    } else {

        //営業時間外の場合、時刻部分のみ営業開始時刻に置き換える
        tmpVal = new Date(inDateTime.getFullYear() + "/" + (inDateTime.getMonth() + 1) + "/" + inDateTime.getDate() + " " + String(smbScript.PadLeft(String(startHh), "0", 2)) + ":" + String(smbScript.PadLeft(String(startMm), "0", 2)));

        //パラメータの日時 > 上記で算出した日時 の場合、算出した日時を＋１日して返す（※翌営業開始日時を返却するため）
        if (inDateTime.getTime() > tmpVal.getTime()) {

            rtnVal = new Date();
            rtnVal.setTime(tmpVal.getTime() + C_1DAYMILLI);

        } else {
            rtnVal = tmpVal;
        }

    }

    return rtnVal;
}

/**
* 営業時間外の日時の場合、営業開始時刻に補正する。※開始日時用<br>
* 
* @param {Date} argStartDateTime yyyy/MM/dd HH:mm形式文字列、またはDate値(開始日時)
* @return {Date} 補正後の開始日時
* 
*/
smbScript.GetStartDateExcludeOutOfTime = function (argStartDateTime) {

    var rtnVal;

    var startDateTime;             //作業開始日時
    var dtStallStartTime;          //営業開始日時
    var dtStallEndTime;            //営業終了日時

    //パラメータの作業開始日時がStringだったらDateに変換する
    if (typeof argStartDateTime == "string") startDateTime = new Date(argStartDateTime);
    else startDateTime = argStartDateTime;

    //営業開始日時
    //dtStallStartTime = new Date($("#hidShowDate").val() + " " + $("#hidStallStartTime").val() + ":00");
    dtStallStartTime = new Date(startDateTime.getFullYear() + "/" + (startDateTime.getMonth() + 1) + "/" + startDateTime.getDate() + " " + $("#hidStallStartTime").val() + ":00");

    //営業終了日時
    //dtStallEndTime = new Date($("#hidShowDate").val() + " " + $("#hidStallEndTime").val() + ":00");
    dtStallEndTime = new Date(startDateTime.getFullYear() + "/" + (startDateTime.getMonth() + 1) + "/" + startDateTime.getDate() + " " + $("#hidStallEndTime").val() + ":00");

    //作業開始日時のhhmmを取得
    var hh = startDateTime.getHours();
    var mm = startDateTime.getMinutes();
    var time = String(smbScript.PadLeft(String(hh), "0", 2)) + String(smbScript.PadLeft(String(mm), "0", 2));
    var intTime = parseInt(time, C_RADIX);

    //営業開始日時のhhmmを取得
    var startHh = dtStallStartTime.getHours();
    var startMm = dtStallStartTime.getMinutes();
    var startTime = String(smbScript.PadLeft(String(startHh), "0", 2)) + String(smbScript.PadLeft(String(startMm), "0", 2));
    var startIntTime = parseInt(startTime, C_RADIX);

    //営業終了日時のhhmmを取得
    var endHh = dtStallEndTime.getHours();
    var endMm = dtStallEndTime.getMinutes();
    var endTime = String(smbScript.PadLeft(String(endHh), "0", 2)) + String(smbScript.PadLeft(String(endMm), "0", 2));
    var endIntTime = parseInt(endTime, C_RADIX);

    //営業時間内の場合
    if ((startIntTime <= intTime) && (intTime <= endIntTime)) {

        //パラメータをそのまま返す
        rtnVal = startDateTime;
    } else {

        //営業時間外の場合、時刻部分のみ営業開始時刻に置き換えて返す
        rtnVal = new Date(startDateTime.getFullYear() + "/" + (startDateTime.getMonth() + 1) + "/" + startDateTime.getDate() + " " + String(smbScript.PadLeft(String(startHh), "0", 2)) + ":" + String(smbScript.PadLeft(String(startMm), "0", 2)));
    }

    return rtnVal;
}

/**
* 営業時間外の日時の場合、営業終了時刻に補正する。※終了日時用<br>
* 
* @param {Date} argEndDateTime yyyy/MM/dd HH:mm形式文字列、またはDate値(終了日時)
* @return {Date} 補正後の終了日時
* 
*/
smbScript.GetEndDateExcludeOutOfTime = function (argEndDateTime) {

    var rtnVal;

    var endDateTime;               //作業終了日時
    var dtStallStartTime;          //営業開始日時
    var dtStallEndTime;            //営業終了日時

    //パラメータの作業終了日時がStringだったらDateに変換する
    if (typeof argEndDateTime == "string") endDateTime = new Date(argEndDateTime);
    else endDateTime = argEndDateTime;

    //営業開始日時
    //dtStallStartTime = new Date($("#hidShowDate").val() + " " + $("#hidStallStartTime").val() + ":00");
    dtStallStartTime = new Date(argEndDateTime.getFullYear() + "/" + (argEndDateTime.getMonth() + 1) + "/" + argEndDateTime.getDate() + " " + $("#hidStallStartTime").val() + ":00");

    //営業終了日時
    //dtStallEndTime = new Date($("#hidShowDate").val() + " " + $("#hidStallEndTime").val() + ":00");
    dtStallEndTime = new Date(argEndDateTime.getFullYear() + "/" + (argEndDateTime.getMonth() + 1) + "/" + argEndDateTime.getDate() + " " + $("#hidStallEndTime").val() + ":00");

    //作業終了日時のhhmmを取得
    var hh = endDateTime.getHours();
    var mm = endDateTime.getMinutes();
    var time = String(smbScript.PadLeft(String(hh), "0", 2)) + String(smbScript.PadLeft(String(mm), "0", 2));
    var intTime = parseInt(time, C_RADIX);

    //営業開始日時のhhmmを取得
    var startHh = dtStallStartTime.getHours();
    var startMm = dtStallStartTime.getMinutes();
    var startTime = String(smbScript.PadLeft(String(startHh), "0", 2)) + String(smbScript.PadLeft(String(startMm), "0", 2));
    var startIntTime = parseInt(startTime, C_RADIX);

    //営業終了日時のhhmmを取得
    var endHh = dtStallEndTime.getHours();
    var endMm = dtStallEndTime.getMinutes();
    var endTime = String(smbScript.PadLeft(String(endHh), "0", 2)) + String(smbScript.PadLeft(String(endMm), "0", 2));
    var endIntTime = parseInt(endTime, C_RADIX);

    //営業時間内の場合
    if ((startIntTime <= intTime) && (intTime <= endIntTime)) {

        //パラメータをそのまま返す
        rtnVal = endDateTime;
    } else {

        //営業時間外の場合、時刻部分のみ営業終了時刻に置き換えて返す
        rtnVal = new Date(endDateTime.getFullYear() + "/" + (endDateTime.getMonth() + 1) + "/" + endDateTime.getDate() + " " + String(smbScript.PadLeft(String(endHh), "0", 2)) + ":" + String(smbScript.PadLeft(String(endMm), "0", 2)));
    }

    return rtnVal;
}

/******************************
* Check系関数
******************************/

/**
* 正規表現でのフォーマットチェックを行う。<br>
* 
* @param {String} src チェック対象文字列
* @param {String} pattern チェックに使用する正規表現パターン
* @return {Boolean} true:チェックOK/false:チェックNG
* 
*/
smbScript.CheckFormat = function (src, pattern) {

    var rtnVal = false;

    if (src.match(pattern)) {
        rtnVal = true;
    }

    return rtnVal;
}

/**
* 正規表現での時間フォーマットチェックを行う。<br>
* 
* @param {String} src チェック対象文字列
* @return {Boolean} true:チェックOK/false:チェックNG
* 
*/
smbScript.CheckTimeFormat = function (src) {

    var rtnVal = smbScript.CheckFormat(src, C_TIMEPATTERN);

    return rtnVal;
}

/**
* 正規表現での半角数字のみフォーマットチェックを行う。<br>
* 
* @param {String} src チェック対象文字列
* @return {Boolean} true:チェックOK/false:チェックNG
* 
*/
smbScript.CheckOnlyHalfWidthDigitFormat = function (src) {

    var rtnVal = smbScript.CheckFormat(src, C_NUMPATTERN);

    return rtnVal;
}

/**
* 来店・作業開始・作業終了・納車時間の前後関係をチェックする（予定時間用）。<br>
* 
* @param {String or Date} argVisitDateTime yyyy/MM/dd HH:mm形式文字列、またはDate値(来店時間)
* @param {String or Date} argStartDateTime yyyy/MM/dd HH:mm形式文字列、またはDate値(作業開始時間)
* @param {String or Date} argEndDateTime yyyy/MM/dd HH:mm形式文字列、またはDate値(作業終了時間)
* @param {String or Date} argDeliDateTime yyyy/MM/dd HH:mm形式文字列、またはDate値(納車時間)
* @return {Boolean} True:チェックOK/False:チェックNG
* 
*/
smbScript.CheckContextOfPlan = function (argVisitDateTime, argStartDateTime, argEndDateTime, argDeliDateTime) {

    var rtnVal = false;

    //下記、① ≦ ② ＜ ③ ≦ ④のチェックを行う
    //① 来店時間
    //② 作業開始時間
    //③ 作業終了時間
    //④ 納車時間

    var check1 = false;     //① 来店時間　　　≦　② 作業開始時間　のチェック
    var check2 = false;     //② 作業開始時間　＜　③ 作業終了時間　のチェック
    var check3 = false;     //③ 作業終了時間　≦　④ 納車時間　　　のチェック
    var check4 = false;     //① 来店時間　　　＜　③ 作業終了時間　のチェック
    var check5 = false;     //① 来店時間　　　＜　④ 納車時間　　　のチェック
    var check6 = false;     //② 作業開始時間　＜　④ 納車時間　　　のチェック

    var visitTime = null;
    var startTime = null;
    var endTime = null;
    var deliTime = null;

    var visitDateTime = null;
    var startDateTime = null;
    var endDateTime = null;
    var deliDateTime = null;

    //来店時間がStringだったらDateに変換する
    if (argVisitDateTime != null) {
        if (typeof argVisitDateTime == "string") visitDateTime = new Date(argVisitDateTime);
        else visitDateTime = argVisitDateTime;

        visitTime = new Date(smbScript.ConvertDateToString2(visitDateTime)).getTime();
    }

    //作業開始時間がStringだったらDateに変換する
    if (argStartDateTime != null) {
        if (typeof argStartDateTime == "string") startDateTime = new Date(argStartDateTime);
        else startDateTime = argStartDateTime;

        startTime = new Date(smbScript.ConvertDateToString2(startDateTime)).getTime();
    }

    //作業終了時間がStringだったらDateに変換する
    if (argEndDateTime != null) {
        if (typeof argEndDateTime == "string") endDateTime = new Date(argEndDateTime);
        else endDateTime = argEndDateTime;

        endTime = new Date(smbScript.ConvertDateToString2(endDateTime)).getTime();
    }

    //納車時間がStringだったらDateに変換する
    if (argDeliDateTime != null) {
        if (typeof argDeliDateTime == "string") deliDateTime = new Date(argDeliDateTime);
        else deliDateTime = argDeliDateTime;

        deliTime = new Date(smbScript.ConvertDateToString2(deliDateTime)).getTime();
    }

    //チェック１：① 来店時間　≦　② 作業開始時間　のチェック
    //来店時間または作業開始時間がnullならチェックなしでtrue
    if (visitTime == null || startTime == null) {
        check1 = true;
    }
    else {
        if (visitTime <= startTime) {
            check1 = true;
        }
    }

    //チェック２：② 作業開始時間　＜　③ 作業終了時間　のチェック
    //作業開始時間または作業終了時間がnullならチェックなしでtrue
    if (startTime == null || endTime == null) {
        check2 = true;
    }
    else {
        if (startTime < endTime) {
            check2 = true;
        }
    }

    //チェック３：③ 作業終了時間　≦　④ 納車時間　のチェック
    //作業終了時間または納車時間がnullならチェックなしでtrue
    if (endTime == null || deliTime == null) {
        check3 = true;
    }
    else {
        if (endTime <= deliTime) {
            check3 = true;
        }
    }

    //チェック４：① 来店時間　＜　③ 作業終了時間　のチェック
    //来店時間または作業終了時間がnullならチェックなしでtrue
    if (visitTime == null || endTime == null) {
        check4 = true;
    }
    else {
        if (visitTime < endTime) {
            check4 = true;
        }
    }

    //チェック５：① 来店時間　＜　④ 納車時間　のチェック
    //来店時間または納車時間がnullならチェックなしでtrue
    if (visitTime == null || deliTime == null) {
        check5 = true;
    }
    else {
        if (visitTime < deliTime) {
            check5 = true;
        }
    }

    //チェック６：② 作業開始時間　＜　④ 納車時間　のチェック
    //作業開始時間または納車時間がnullならチェックなしでtrue
    if (startTime == null || deliTime == null) {
        check6 = true;
    }
    else {
        if (startTime < deliTime) {
            check6 = true;
        }
    }

    if (check1 && check2 && check3 && check4 && check5 && check6) rtnVal = true;

    return rtnVal;
}

/**
* 来店・作業開始・作業終了・納車時間の前後関係をチェックする（実績時間用）。<br>
* 
* @param {String or Date} argVisitDateTime yyyy/MM/dd HH:mm形式文字列、またはDate値(来店時間)
* @param {String or Date} argStartDateTime yyyy/MM/dd HH:mm形式文字列、またはDate値(作業開始時間)
* @param {String or Date} argEndDateTime yyyy/MM/dd HH:mm形式文字列、またはDate値(作業終了時間)
* @param {String or Date} argDeliDateTime yyyy/MM/dd HH:mm形式文字列、またはDate値(納車時間)
* @return {Boolean} True:チェックOK/False:チェックNG
* 
*/
smbScript.CheckContextOfProcess = function (argVisitDateTime, argStartDateTime, argEndDateTime, argDeliDateTime) {

    var rtnVal = false;

    //下記、① ≦ ② ≦ ③ ≦ ④のチェックを行う
    //① 来店時間
    //② 作業開始時間
    //③ 作業終了時間
    //④ 納車時間

    var check1 = false;     //① 来店時間　　　≦　② 作業開始時間　のチェック
    var check2 = false;     //② 作業開始時間　≦　③ 作業終了時間　のチェック
    var check3 = false;     //③ 作業終了時間　≦　④ 納車時間　　　のチェック

    var visitTime = null;
    var startTime = null;
    var endTime = null;
    var deliTime = null;

    var visitDateTime = null;
    var startDateTime = null;
    var endDateTime = null;
    var deliDateTime = null;

    //来店時間がStringだったらDateに変換する
    if (argVisitDateTime != null) {
        if (typeof argVisitDateTime == "string") visitDateTime = new Date(argVisitDateTime);
        else visitDateTime = argVisitDateTime;

        visitTime = new Date(smbScript.ConvertDateToString2(visitDateTime)).getTime();
    }

    //作業開始時間がStringだったらDateに変換する
    if (argStartDateTime != null) {
        if (typeof argStartDateTime == "string") startDateTime = new Date(argStartDateTime);
        else startDateTime = argStartDateTime;

        startTime = new Date(smbScript.ConvertDateToString2(startDateTime)).getTime();
    }

    //作業終了時間がStringだったらDateに変換する
    if (argEndDateTime != null) {
        if (typeof argEndDateTime == "string") endDateTime = new Date(argEndDateTime);
        else endDateTime = argEndDateTime;

        endTime = new Date(smbScript.ConvertDateToString2(endDateTime)).getTime();
    }

    //納車時間がStringだったらDateに変換する
    if (argDeliDateTime != null) {
        if (typeof argDeliDateTime == "string") deliDateTime = new Date(argDeliDateTime);
        else deliDateTime = argDeliDateTime;

        deliTime = new Date(smbScript.ConvertDateToString2(deliDateTime)).getTime();
    }

    //チェック１：① 来店時間　≦　② 作業開始時間　のチェック
    //来店時間または作業開始時間がnullならチェックなしでtrue
    if (visitTime == null || startTime == null) {
        check1 = true;
    }
    else {
        if (visitTime <= startTime) {
            check1 = true;
        }
    }

    //チェック２：② 作業開始時間　≦　③ 作業終了時間　のチェック
    //作業開始時間または作業終了時間がnullならチェックなしでtrue
    if (startTime == null || endTime == null) {
        check2 = true;
    }
    else {
        if (startTime <= endTime) {
            check2 = true;
        }
    }

    //チェック３：③ 作業終了時間　≦　④ 納車時間　のチェック
    //作業終了時間または納車時間がnullならチェックなしでtrue
    if (endTime == null || deliTime == null) {
        check3 = true;
    }
    else {
        if (endTime <= deliTime) {
            check3 = true;
        }
    }

    if (check1 && check2 && check3) rtnVal = true;

    return rtnVal;
}

/**
* チップの表示時間が営業時間内かどうかをチェックする。<br>
* 
* @param {String or Date} argStartDateTime yyyy/MM/dd HH:mm形式文字列、またはDate値(作業開始予定日時)
* @param {String or Date} argEndDateTime yyyy/MM/dd HH:mm形式文字列、またはDate値(作業終了予定日時)
* @return {Boolean} True:チェックOK/False:チェックNG
* 
*/
smbScript.CheckChipInStallTime = function (argStartDateTime, argEndDateTime) {

    var rtnVal = false;

    var check1 = false;     //開始時間のチェック
    var check2 = false;     //終了時間のチェック

    var startDateTime;      //作業開始予定日時
    var endDateTime;        //作業終了予定日時

    var startHH;            //作業開始予定（HH）
    var endHH;              //作業終了予定（HH）

    var startmm;            //作業開始予定（mm）
    var endmm;              //作業終了予定（mm）

    var startHHmm;          //作業開始予定（HHmmの数値に変換して保持）
    var endHHmm;            //作業終了予定（HHmmの数値に変換して保持）

    var stallStartTime;     //営業開始時刻（HHmmの数値に変換して保持）
    var stallEndTime;       //営業終了時刻（HHmmの数値に変換して保持）

    //作業開始予定日時がStringだったらDateに変換する
    if (typeof argStartDateTime == "string") startDateTime = new Date(argStartDateTime);
    else startDateTime = argStartDateTime;

    //作業終了予定日時がStringだったらDateに変換する
    if (typeof argEndDateTime == "string") endDateTime = new Date(argEndDateTime);
    else endDateTime = argEndDateTime;

    //作業開始予定日時をHHmmの数値に変換する
    startHH = smbScript.PadLeft(String(startDateTime.getHours()), "0", 2);
    startmm = smbScript.PadLeft(String(startDateTime.getMinutes()), "0", 2);
    startHHmm = parseInt((startHH + startmm), C_RADIX);

    //作業終了予定日時をHHmmの数値に変換する
    endHH = smbScript.PadLeft(String(endDateTime.getHours()), "0", 2);
    endmm = smbScript.PadLeft(String(endDateTime.getMinutes()), "0", 2);
    endHHmm = parseInt((endHH + endmm), C_RADIX);

    //営業開始時刻をHHmmの数値に変換する
    stallStartTime = parseInt($("#hidStallStartTime").val().replace(":", ""), C_RADIX);

    //営業終了時刻をHHmmの数値に変換する
    stallEndTime = parseInt($("#hidStallEndTime").val().replace(":", ""), C_RADIX);


    //時間の前後関係チェックは全て5桁で行う(ex. 9:00→09:00)

    //チェック①：チップの開始時間が営業開始時間以上
    if (stallStartTime <= startHHmm) {
        check1 = true;
    }

    //チェック②：チップの終了時間が営業終了時間以下
    if (endHHmm <= stallEndTime) {
        check2 = true;
    }
    //更新： 2014/04/01 TMEJ 丁 タブレット版SMB チーフテクニシャン機能開発 START
    //if (check1 && check2) rtnVal = true;
    if ((check1 && check2) || ($("#ChipDetailStallUseStatusHidden").val().Trim() == "02")) rtnVal = true;
    //更新： 2014/04/01 TMEJ 丁 タブレット版SMB チーフテクニシャン機能開発 END

    return rtnVal;
}

/**
* 二つの日時が同じ年月日かどうかチェックする。<br>
* 
* @param {String} src1 比較対象日時1
* @param {String} src2 比較対象日時2
* @return {Boolean} True:チェックOK/False:チェックNG
* 
*/
smbScript.CheckSameTwoDates = function (src1, src2) {

    var rtnVal = true;

    var date1;
    var date2;

    if (typeof src1 == "string") date1 = new Date(src1);
    else date1 = src1;

    if (typeof src2 == "string") date2 = new Date(src2);
    else date2 = src2;

    //同じ年月日かどうかチェック
    if ((date1.getFullYear() != date2.getFullYear()) ||
        (date1.getMonth() != date2.getMonth()) ||
        (date1.getDate() != date2.getDate())) {
        rtnVal = false;
    }

    return rtnVal;
}

/******************************
 * Convert系関数
 ******************************/ 

/**
* Date型の値をyyyy/MM/dd HH:mm:ssの文字列に変換する。<br>
* 
* @param {Date} date 変換対象日時
* @return {String} yyyy/MM/dd HH:mm:ss形式の文字列
* 
*/
smbScript.ConvertDateToString = function (date) {

    var dateVal;
    var rtnVal;

    if (date == null) {
        rtnVal = "";
    }
    else {
        if (typeof date == "string") dateVal = new Date(date);
        else dateVal = date;

        rtnVal = dateVal.getFullYear() + '/'
                + ('00' + (dateVal.getMonth() + 1)).slice(-2) + '/'
                + ('00' + dateVal.getDate()).slice(-2) + ' '
                + ('00' + dateVal.getHours()).slice(-2) + ':'
                + ('00' + dateVal.getMinutes()).slice(-2) + ':'
                + ('00' + dateVal.getSeconds()).slice(-2);
    }
    return rtnVal;
}

/**
* Date型の値をyyyy/MM/dd HH:mm:00の文字列に変換する。<br>
* 
* @param {Date} date 変換対象日時
* @return {String} yyyy/MM/dd HH:mm:00形式の文字列
* 
*/
smbScript.ConvertDateToString2 = function (date) {

    var dateVal;
    var rtnVal;

    if (date == null) {
        rtnVal = "";
    }
    else {
        if (typeof date == "string") dateVal = new Date(date);
        else dateVal = date;

        rtnVal = dateVal.getFullYear() + '/'
                + ('00' + (dateVal.getMonth() + 1)).slice(-2) + '/'
                + ('00' + dateVal.getDate()).slice(-2) + ' '
                + ('00' + dateVal.getHours()).slice(-2) + ':'
                + ('00' + dateVal.getMinutes()).slice(-2) + ':00';
    }
    return rtnVal;
}

//2013/12/04 TMEJ 下村 タブレット版SMB チーフテクニシャン機能開発 START
/**
* Date型の値をyyyy/MM/dd HH:mmの文字列に変換する。(画面表示用)<br>
* 
* @param {Date} date 変換対象日時
* @return {String} yyyy/MM/dd HH:mm形式の文字列
* 
*/
smbScript.ConvertDateToStringForDisplay = function (date) {

    var dateVal;
    var rtnVal;

    if (date == null) {
        rtnVal = "";
    }
    else {
        if (typeof date == "string") dateVal = new Date(date);
        else dateVal = date;

    	//国によって表示を切り替える
    	if(gDateFormatMMdd == "MM/dd"){
    		if(gDateFormatHHmm == "HH:mm"){
	    		//"yyyy/MM/dd HH:mm"
		        rtnVal =  dateVal.getFullYear() + '/'
		                + ('00' + (dateVal.getMonth() + 1)).slice(-2) + '/'
		                + ('00' + dateVal.getDate()).slice(-2) + ' '
		                + ('00' + dateVal.getHours()).slice(-2) + ':'
		                + ('00' + dateVal.getMinutes()).slice(-2);
    		} else {
    			//"yyyy/MM/dd mm:HH"
		        rtnVal =  dateVal.getFullYear() + '/'
		                + ('00' + (dateVal.getMonth() + 1)).slice(-2) + '/'
		                + ('00' + dateVal.getDate()).slice(-2) + ' '
		                + ('00' + dateVal.getMinutes()).slice(-2) + ':'
		                + ('00' + dateVal.getHours()).slice(-2);
    		}
    	} else {
    		if(gDateFormatHHmm == "HH:mm"){
	    		//"dd/MM/yyyy HH:mm"
		        rtnVal =  ('00' + dateVal.getDate()).slice(-2) + '/'
		                + ('00' + (dateVal.getMonth() + 1)).slice(-2) + '/'
		                + dateVal.getFullYear() + ' '
		                + ('00' + dateVal.getHours()).slice(-2) + ':'
		                + ('00' + dateVal.getMinutes()).slice(-2);
    		} else {
	    		//"dd/MM/yyyy mm:HH"
		        rtnVal =  ('00' + dateVal.getDate()).slice(-2) + '/'
		                + ('00' + (dateVal.getMonth() + 1)).slice(-2) + '/'
		                + dateVal.getFullYear() + ' '
		                + ('00' + dateVal.getMinutes()).slice(-2) + ':'
		                + ('00' + dateVal.getHours()).slice(-2);
    		}
    	}
    }
    return rtnVal;
}
//2013/12/04 TMEJ 下村 タブレット版SMB チーフテクニシャン機能開発 END


/**
* 二つの日時を比較し、時刻（HH:mm）もしくは日付（MM/dd）を返す。(画面表示用)<br>
* 
* @param {String} src 入力日時
* @param {String} showDate 工程管理画面で選択されている日付
* @return {String} 同日の場合: 時刻（HH:mm）/同日でない場合: 日付（MM/dd）
* 
*/
smbScript.ConvertDateOrTime = function (src, showDate) {

    var rtnVal;

    if (src == null) {
        rtnVal = "";
    }
    else {
        var dtShowDate = new Date(showDate);

        //2013/12/26 TMEJ 下村 次世代e-CRBタブレット(サービス) iOS7.0 VersionUp対応 START
        //var dtSrc = new Date(src);
        var dtSrc;
        if (typeof src == "string") dtSrc = new Date(src);
        else dtSrc = src;
        //2013/12/26 TMEJ 下村 次世代e-CRBタブレット(サービス) iOS7.0 VersionUp対応 END

        //工程管理画面で選択されている日付と同じ場合、画面で入力された時刻（HH:mm）を返す
        if (smbScript.CheckSameTwoDates(src, showDate)) {
        	//2013/12/04 TMEJ 下村 タブレット版SMB チーフテクニシャン機能開発 START
            //rtnVal = (dtSrc.getHours()) + ":" + smbScript.PadLeft(String(dtSrc.getMinutes()), "0", 2);
        	
        	//国によって表示を切り替える
        	if(gDateFormatHHmm == "HH:mm"){
            	rtnVal = (dtSrc.getHours()) + ":" + smbScript.PadLeft(String(dtSrc.getMinutes()), "0", 2);
        	} else {
            	rtnVal = smbScript.PadLeft(String(dtSrc.getMinutes()), "0", 2) + ":" + (dtSrc.getHours());
        	}
        	//2013/12/04 TMEJ 下村 タブレット版SMB チーフテクニシャン機能開発 END
        }
        //工程管理画面で選択されている日付と違う場合、画面で入力された日付（MM/dd）を返す
        else {
        	//2013/12/04 TMEJ 下村 タブレット版SMB チーフテクニシャン機能開発 START
        	//rtnVal = (dtSrc.getMonth() + 1) + "/" + dtSrc.getDate();
        	
        	//国によって表示を切り替える
        	if(gDateFormatMMdd == "MM/dd"){
        		//"MM/dd"
        		rtnVal = (dtSrc.getMonth() + 1) + "/" + dtSrc.getDate();
        	} else {
        		//"dd/MM"
        		rtnVal = dtSrc.getDate() + "/" + (dtSrc.getMonth() + 1);
        	}
        	//2013/12/04 TMEJ 下村 タブレット版SMB チーフテクニシャン機能開発 END
        }
    }

    return rtnVal;
}

/**
* 文字列の左埋めを行う。<br>
* 
* @param {String} src 左埋め対象文字列
* @param {String} chr 左埋めに使用する文字
* @param {Integer} len 左埋め完了後の長さ
* @return {Integer} 左埋めされた文字列
* 
* @example 
* PadLeft("12345", "a", 10);
* 出力:「"aaaaa12345"」
*/
smbScript.PadLeft = function (src, chr, len) {

    var rtnVal = src;

    while (rtnVal.length < len) {
        rtnVal = chr + rtnVal;
    }

    return rtnVal;
}

/**
* 文字列(str)の先頭から、指定したバイト数(byteSize)で切り出す。<br>
* 
* @param {String} str 切り出す対象の文字列
* @param {Integer} byteSize 切り出し後のバイト数
* @return {String} 先頭から指定したバイト数で切り出された文字列
* 
* @example 
* trimStr("1234567890", 7);
* 出力:「"1234567"」
*/
smbScript.trimStr = function (str, byteSize) {
	var byte = 0;
	var trimStr = "";

	for (var j = 0, len = str.length; j < len ; j++) {

		if (str[j].match(/[^\x00-\xff]/ig) != null) {
			byte += 2;
		}
		else {
			byte += 1;
		}

		trimStr += str.charAt(j);
		if(byte >= byteSize){
			trimStr = trimStr.substr(0, j + 1);
			break;
		}
	}
	return trimStr;
}

//2013/12/26 TMEJ 下村 次世代e-CRBタブレット(サービス) iOS7.0 VersionUp対応 START
/**
* datetime-localの書式（yyyy-MM-ddTHH:mm:ss）文字列を、日付型に変換する。<br>
* ※セールスのSC3080216.Util.js（iOS7.0対応）から流用。<br>
* ※yyyy-MM-ddTHH:mm:ss.000 の形式にも対応
* 
* @param {String} dateValue datetime-localの書式（yyyy-MM-ddTHH:mm:ss）文字列
* @return {Date} 引数を日付型に変換した値
* 
*/
smbScript.changeStringToDateIcrop = function (dateValue) {

    if (dateValue == null || dateValue == ""){
        return null;
    }
    
    var strDate = String(dateValue);
    strDate = strDate.replace(/-/g, '/');
    strDate = strDate.replace('T', ' ');

    var strSplitDate = strDate.split(".");

    return new Date(Date.parse(strSplitDate[0]));
}

//2013/12/26 TMEJ 下村 次世代e-CRBタブレット(サービス) iOS7.0 VersionUp対応 END

//2013/12/26 TMEJ 下村 次世代e-CRBタブレット(サービス) iOS7.0 VersionUp対応 START
/**
* 日付型の値を、datetime-localの書式（yyyy-MM-ddTHH:mm:ss）文字列に変換する。<br>
* ※セールスのSC3080216.Util.js（iOS7.0対応）から流用。<br>
* 
* @param {Date} dt 日付型の値
* @return {String} datetime-localの書式（yyyy-MM-ddTHH:mm:ss）文字列
* 
*/
smbScript.getDateTimelocalDate = function (dt) {

    var yyyy = dt.getFullYear();
    var mm = dt.getMonth() + 1;
    var dd = dt.getDate();

    var hh = dt.getHours();
    var mi = dt.getMinutes();

    var ret = '';

    if (mm < 10) {
        mm = '0' + mm;
    }
    if (dd < 10) {
        dd = '0' + dd;
    }
    
    if (hh < 10) {
        hh = '0' + hh;
    }
    if (mi < 10) {
        mi = '0' + mi;
    }
    
    ret = '' + yyyy + "-" + mm + "-" + dd + "T" + hh + ':' + mi + ":00";

    return ret;
}
//2013/12/26 TMEJ 下村 次世代e-CRBタブレット(サービス) iOS7.0 VersionUp対応 END
