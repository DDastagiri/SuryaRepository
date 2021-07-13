/**
* @fileOverview SC3080216 ユーティリティ処理
*
* @author TCS 安田
* @version 1.0.0
*
* 更新： 2013/10/02 TCS 安田 次世代e-CRBタブレット iOS7.0 VersionUp対応（セールス）
*/

//アイコン画像パスの取得
//アイコン分類 (0:プロセス、1:通常、2:編集、3:遅れ、4:完了)、開始工程コード、 終了工程コード
function getIconImagePath(iconFlg, stateCd, endCd) {

    //対象のアイコンを探す
    for (var i = 3; i < iconSettingArray.length; ) {

        var stateProcCd = iconSettingArray[i - 3];  //開始工程コード
        var endProcCd = iconSettingArray[i - 2];    //終了工程コード
        var iconKb = iconSettingArray[i - 1];       //アイコン分類
        var iconUrl = iconSettingArray[i];          //アイコンURL

        if ((iconKb == iconFlg) && (stateCd == stateProcCd) && (endCd == endProcCd)) {
            return "url(" + iconUrl + ")";
        }

        i = i + 4;
    }

    return "";
}

//YYYY/MM/DD日付文字列取得
function getParseDateValue(dtString) {

    var yy = dtString.substring(0, 4);
    var mm = dtString.substring(4, 6);
    var dd = dtString.substring(6, 8);
    
    var dtTemp = new Date();
    dtTemp.setYear(yy);
    dtTemp.setMonth(mm-1);
    dtTemp.setDate(dd);
    dtTemp.setHours(0, 0, 0, 0);

    return dtTemp;
}

/* 2013/10/02 TCS 安田 次世代e-CRBタブレット iOS7.0 VersionUp対応（セールス） START */
//yyyy-MM-ddTHH:mm:ss日付文字列取得
function geDateTimelocalDate(dt) {

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
/* 2013/10/02 TCS 安田 次世代e-CRBタブレット iOS7.0 VersionUp対応（セールス） END */

//YYYYMMDD日付文字列取得
function geHeifunDate(dt) {

    var yyyy = dt.getFullYear();
    var mm = dt.getMonth() + 1;
    var dd = dt.getDate();

    var ret = '';

    if (mm < 10) {
        mm = '0' + mm;
    }
    if (dd < 10) {
        dd = '0' + dd;
    }
    ret = '' + yyyy + "-" + mm + "-" + dd;

    return ret;
}

//HH:MM時間文字列取得
function getHHMM(dt) {

    var hh = dt.getHours();
    var mm = dt.getMinutes();

    var ret = '';

    if (hh < 10) {
        hh = '0' + hh;
    }
    if (mm < 10) {
        mm = '0' + mm;
    }
    ret = '' + hh + ':' + mm;

    return ret;
}


//YYYYMMDD日付文字列取得
function getYYYYMMDD(dt) {

    var yyyy = dt.getFullYear();
    var mm = dt.getMonth() + 1;
    var dd = dt.getDate();

    var ret = '';

    if (mm < 10) {
        mm = '0' + mm;
    }
    if (dd < 10) {
        dd = '0' + dd;
    }
    ret = '' + yyyy + mm + dd;

    return ret;
}


/* 2013/10/02 TCS 安田 次世代e-CRBタブレット iOS7.0 VersionUp対応（セールス） START */
//文字列を日付型にする
function changeStringToDateIcrop(dateValue) {

    if (dateValue == null || dateValue == ""){
        return null;
    }
    
    var strDate = String(dateValue);
    strDate = strDate.replace(/-/g, '/');
    strDate = strDate.replace('T', ' ');
    
    return new Date(Date.parse(strDate));
}
/* 2013/10/02 TCS 安田 次世代e-CRBタブレット iOS7.0 VersionUp対応（セールス） END */
