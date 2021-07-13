/** 
 * @fileOverview 受付待ち画面
 * 
 * @author skfc kushiro
 * @version 1.0.0
 * 更新： 2013/03/27 skfc kushiro 新規作成
 */
// タイマーID
var gRefreshAllTimerId;
var gAfterCalleeNextTimerId;
// 呼出中履歴データ
var gAfterCalleeList;
var gAfterCalleeListPage;
// 日付フォーマット
var strFormatDate = [];
var C_DATE_FORMAT_NUMBER = 1;

// 定期更新間隔
var C_REFRESH_ALL_INTERVAL = 60000;
// 日付更新間隔
var C_DATE_INTERVAL = 30000;
// 呼出中履歴切替間隔
var C_AFTER_CALLEE_NEXT_PAGE_INTERVAL = 5000;


/**
 * 現地国フォーマットの日時文字列を取得する<br>
 *
 * @param aIntKind {number} フォーマット種別
 * @param aStrVal {string} 日時文字列
 * @return {String} 現地国フォーマットの日時
 */
var _localeDate = function (aIntKind, aStrVal) {
    var strFormat;
    var rtnCD = "";
    var strDate = new Array();
    if (aIntKind > strFormatDate.length - 1)
    {
        return rtnCD;
    }
    if (aStrVal.match(/[^0-9]/g))
    {
        if (aStrVal.length == 19)
        {
            strDate[0] = aStrVal.substr(0, 4);
            strDate[1] = aStrVal.substr(5, 2);
            strDate[2] = aStrVal.substr(8, 2);
            strDate[3] = aStrVal.substr(11, 2);
            strDate[4] = aStrVal.substr(14, 2);
            strDate[5] = aStrVal.substr(17, 2);
        } else
        {
            if (aStrVal.length == 10)
            {
                strDate[0] = aStrVal.substr(0, 4);
                strDate[1] = aStrVal.substr(5, 2);
                strDate[2] = aStrVal.substr(8, 2);
                strDate[3] = "00";
                strDate[4] = "00";
                strDate[5] = "00";
            } else
            {
                return rtnCD;
            }
        }
        for (i = 0; i < 6; i++)
        {
            if (strDate[i].match(/[^0-9]/g))
            {
                return rtnCD;
            }
        }
    } else
    {
        if (aStrVal.length == 14)
        {
            strDate[0] = aStrVal.substr(0, 4);
            strDate[1] = aStrVal.substr(4, 2);
            strDate[2] = aStrVal.substr(6, 2);
            strDate[3] = aStrVal.substr(8, 2);
            strDate[4] = aStrVal.substr(10, 2);
            strDate[5] = aStrVal.substr(12, 2);
        } else
        {
            if (aStrVal.length == 8)
            {
                strDate[0] = aStrVal.substr(0, 4);
                strDate[1] = aStrVal.substr(4, 2);
                strDate[2] = aStrVal.substr(6, 2);
                strDate[3] = "00"
                strDate[4] = "00"
                strDate[5] = "00"
            } else
            {
                return rtnCD;
            }
        }
    }
    strFormat = strFormatDate[aIntKind];
    for (j = 0; j < 6; j++)
    {
        strFormat = strFormat.replace("%" + (j + 1), strDate[j]);
    }
    if (strFormat.indexOf("%7") >= 0)
    {
        var dd = new Date(strDate[0], strDate[1] - 1, strDate[2], strDate[3], strDate[4], strDate[5])
        strFormat = strFormat.replace("%7", strWeekName[dd.getDay()]);
    }
    if (strFormat.indexOf("%8") >= 0)
    {
        strFormat = strFormat.replace("%8", strMonthName[strDate[1] - 1]);
    }

    strFormat = strFormat.replace("%9", strDate[0].substr(2, 2));
    return strFormat;
}

/**
 * 1桁の数字を0埋めで2桁にする<br>
 *
 * @param aNum {number} 対象数値
 * @return {String} 0埋め文字列
 */
var _to2Digits = function (aNum) {
    aNum += "";
    if (aNum.length === 1)
    {

        aNum = "0" + aNum;
    }
    return aNum;
};

/**
 * 日付をYYYY/MM/DD HH:MI形式で取得<br>
 *
 * @return {String} 日時文字列(YYYY/MM/DD HH:MI)
 */
var _dateNow = function () {
    var date = new Date();
    var yyyy = date.getFullYear();
    var mm = _to2Digits(date.getMonth() + 1);
    var dd = _to2Digits(date.getDate());
    var hh = _to2Digits(date.getHours());
    var mi = _to2Digits(date.getMinutes());
    var ss = _to2Digits(date.getSeconds());
    return yyyy + '/' + mm + '/' + dd + ' ' + hh + ':' + mi +':' + ss;
};

/**
 * DOMロード後の処理<br>
 */
$(function () {
    // グローバル変数の初期化
    gRefreshAllTimerId = 0;
    gAfterCalleeNextTimerId = 0;
    gAfterCalleeList = [];
    gAfterCalleeListPage = 0;

    // 日時表示
    $(".Data").text(_localeDate(C_DATE_FORMAT_NUMBER, _dateNow()));
    setInterval(function () {
        $(".Data").text(_localeDate(C_DATE_FORMAT_NUMBER, _dateNow()));
    }, C_DATE_INTERVAL);

    // 全表示初期化
    _displayCallee({ "number": "", "place": "", "saName": "" });
    _displayAfterCallee();
    _displayWaitNumber(0);
    // 初期表示用のデータ要求
    _refreshAllTimer(false);
});

/**
 * 呼出追加<br>
 */
function addCallee() {
    // 定期更新タイマーを止める
    _stopRefreshAllTimer();
    // 更新処理実行(音有り)
    _refreshAllTimer(true);
}

/**
 * 呼出キャンセル(削除)<br>
 */
function delCallee() {
    // 定期更新タイマーを止める
    _stopRefreshAllTimer();
    // 更新処理実行(音無し)
    _refreshAllTimer(false);
}

/**
 * 定期更新タイマーの開始<br>
 */
function _startRefreshAllTimer() {
    if (gRefreshAllTimerId == 0)
    { // 更新タイマーが動いていない場合
        // 受付待ち表示が複数あるため一定間隔が表示を更新する
        gRefreshAllTimerId = setTimeout(_onRefreshAllTimer, C_REFRESH_ALL_INTERVAL);
    }
}

/**
 * 定期更新タイマーの終了<br>
 */
function _stopRefreshAllTimer() {
    // タイマーを停止する
    if (gRefreshAllTimerId != 0)
    {
        clearTimeout(gRefreshAllTimerId);
        gRefreshAllTimerId = 0;
    }
}

/**
* 定期更新タイマーの開始<br>
*/
function _startAfterCalleeNextTimer() {
    if (gAfterCalleeNextTimerId == 0)
    { // 更新タイマーが動いていない場合
        // 受付待ち表示が複数あるため一定間隔が表示を更新する
        gAfterCalleeNextTimerId = setInterval(_onDisplayAfterCallee, C_AFTER_CALLEE_NEXT_PAGE_INTERVAL);
    }
}

/**
* 定期更新タイマーの終了<br>
*/
function _stopAfterCalleeNextTimer() {
    // タイマーを停止する
    if (gAfterCalleeNextTimerId != 0)
    {
        clearInterval(gAfterCalleeNextTimerId);
        gAfterCalleeNextTimerId = 0;
    }
}

/**
* 更新タイマーのコールバック関数<br>
*/
function _onRefreshAllTimer() {
    gRefreshAllTimerId = 0;
    _refreshAllTimer(false);
}

/**
 * 更新タイマーのコールバック関数<br>
 * @param aBeep {Boolean} true:音を鳴らす false:音を鳴らさない
 */
function _refreshAllTimer(aBeep) {
    // タイマー停止
    _stopAfterCalleeNextTimer();
    // 呼出表示データ取得
    $.ajax({
        type: "post",
        datatype: "json",
        url: "SC3130501.aspx/getCalleeList",
        data: "{}",
        contentType: "application/json; charset=utf-8",
        success: function (result) {
            gTimerIdStackRefresh = 0;

            if (result)
            {
                // json配列生成
                callee = $.parseJSON(result.d);
                // 呼出待ち人数表示
                _displayWaitNumber(callee.waitNumber);
                // 今回の呼出データを表示
                _displayCallee(callee.stackCallee);
                // 呼出済データを表示
                gAfterCalleeList = callee.afterCallee;
                _displayAfterCallee();
                if (aBeep)
                {
                    // 音を鳴らす
                    icropBase.Execute('icrop:soundon:3');
                }
            }
            // タイマーを再開
            _startRefreshAllTimer();
            _startAfterCalleeNextTimer();
        },
        error: function () {
            // タイマーを再開
            _startRefreshAllTimer();
            _startAfterCalleeNextTimer();
        }
    });
}

/**
 * メインの呼出情報を表示する<br>
 *
 * @param {json} aCallee 
 *
 * @example 
 *  _displayCallee({"number":"001","place":"受付","saName":"Mike"});
 */
function _displayCallee(aCallee) {
    // 表示更新
    $("#MainNumber").text(aCallee.number);
    $("#MainPlace").text(aCallee.place);
    $("#MainSa").text(aCallee.saName);
};

/**
 * 呼出済み定期更新<br>
 */
function _onDisplayAfterCallee() {
    _displayAfterCallee();

    gAfterCalleeListPage++;
}

/**
 * 呼出済み更新<br>
 *  昇順並び
 */
function _displayAfterCallee() {
    var startIndex;
    var i = 0;
    var num = gAfterCalleeList.length;
    // 表示ページからインデックスを算出
    startIndex = gAfterCalleeListPage * 6;
    if (startIndex >= num)
    {
        startIndex = 0;
        gAfterCalleeListPage = 0;
    }
    // 券番号表示
    i = startIndex;
    $.each($(".NumberT"), function () {
        $(this).text("");
        if (i < num)
        {
            $(this).text(gAfterCalleeList[i].number);
        }
        i++;
    });
    // 呼出場所表示
    i = startIndex;
    $.each($(".Location"), function () {
        $(this).text("");
        if (i < num)
        {
            $(this).text(gAfterCalleeList[i].place);
        }
        i++;
    });

    // 表示枠を表示/非表示する
    i = startIndex;
    $.each($(".myListe dd"), function () {
        if (i < num)
        {
            $(this).show();
        }
        else
        {
            $(this).hide();
        }
        i++;
    });
};

/**
 * 呼出待ち人数表示<br>
 * @param {String or Number}呼出待ち人数
 *
 * @example 
 * _displayWaitNumber();
 * 呼出待ち人数の更新を行う
 */
function _displayWaitNumber(aWaitNumber) {
    $("#WaitNumber").text(aWaitNumber);
}

