/** 
* @fileOverview ウェルカムボードの処理を記述するファイル.
* 
* @author t.shimamura
* @version 1.0.0
* 更新： 2013/04/16 TMEJ m.asano ウェルカムボード仕様変更対応 $01
*/

// ==============================================================
// 定数
// ==============================================================

// 表示顧客名最大数
var C_MAX_CUSTOMER_NUM = 3

// 既存顧客区分
var C_NEW_OR_ORG_CUSTOMER_CLASS = '1'

// 来店通知画面終了判定スパン(ミリ秒)
var C_COUNTER_CALL_SPAN = 1000

// $01 start ウェルカムボード仕様変更対応 
// 顧客名の表示最大幅(この幅に収まるように顧客名用のフォントを調整する。)
var C_NAME_WIDTH_MAX = 1240;

// 顧客名称の最大フォント値
var C_FONT_SIZE_MAX = 122;

// 顧客チップの表示時間(秒)
// この値を超えると顧客チップを自動で削除する。
var C_CUST_CHIP_DELETE_TIME_SPAN = 600;
// $01 end ウェルカムボード仕様変更対応 

// ==============================================================
// 変数
// ==============================================================

// $01 start ウェルカムボード仕様変更対応 
// 顧客情報を保持するリスト(Json)
var gCustInfoJsonList = [];
// $01 end ウェルカムボード仕様変更対応 

// カウンター解除用変数
var counterInterval = null;

// デバッグ用変数
var gDebugFlag = false;
var testCounter = 0;
var testCustName = "顧客";

// jQuery.readyイベントでは動作しないためonloadイベントにて実行する
$.event.add(window, "load", function () {
    icropBase.SwitchMC3A01013FunctionEnabled(true);
});

$(function () {

    // 初期化
    init();

    function init() {

        // デバッグ用
        if ($('div#DebugArea').length > 0) {
            gDebugFlag = true;
        }

        setMessageMargin();

        // $01 start ウェルカムボード仕様変更対応 
        // カウンタの起動
        counterInterval = setInterval(counter, C_COUNTER_CALL_SPAN);
        // $01 end ウェルカムボード仕様変更対応 
    }

    if (gDebugFlag) {
        // デバッグエリア 新規顧客来店
        $('#TestPushNewCustomerButton').live("mousedown", function (aEvent) {
            SC3100304Update('0', '');
        });

        // デバッグエリア 既存顧客来店
        $('#TestPushOrgCustomerButton').live("mousedown", function (aEvent) {
            //SC3100304Update('1', 'test' + testCounter + '太\n郎さんAA Mr.先生');
            SC3100304Update('1', decodeURIComponent(testCustName + testCounter + "様"));
            testCounter++;
            testCustName = testCustName + "あ";
        });
    }
});

/**
* カウンター処理.
*/
function counter() {

    if (gCustInfoJsonList.length == 0) {
        return;
    }

    var delList = [];
    for (i = 0; i <= gCustInfoJsonList.length - 1; i++) {
        var dispTime = gCustInfoJsonList[i].DispCount;
        // 表示秒数がN分を超えている場合は、削除する。
        if (dispTime >= C_CUST_CHIP_DELETE_TIME_SPAN) {
            delList.push(i);
        }
        // 表示秒数を加算
        gCustInfoJsonList[i].DispCount = dispTime + 1;
        if ($('Div#DebugArea').size() > 0) {
            $('span#CounterNum' + i).text(dispTime + 1);
        }
    }

    if (delList.length != 0) {
        DeleteCustData(delList);
        setCustomerName(gCustInfoJsonList);
    }
}

/**
* ウェルカムボード来店通知画面表示を行う（PUSH機能にて実行される前提）.
*/
function SC3100304Update(aCustomerClass, aCustomerName) {

    icropBase.Execute('icrop:log:SC3100304_DebugLog_Start CallFunc[SC3100304Update] SC3100304UpdateParm[aCustomerClass=' + aCustomerClass + ' ,aCustomerName=' + aCustomerName + ']');
    
    // 顧客種別により表示内容を決定
    // 既存顧客
    if (aCustomerClass == C_NEW_OR_ORG_CUSTOMER_CLASS) {

        // $01 start ウェルカムボード仕様変更対応 
        // 既に3件データがある場合は1件目を削除
        if (gCustInfoJsonList.length >= C_MAX_CUSTOMER_NUM) {
            var delList = [0];
            DeleteCustData(delList);
        }

        // 顧客情報をリストに追加
        gCustInfoJsonList.push({ "CustName": decodeURIComponent(aCustomerName), "DispCount": 0 });

        // 顧客情報の表示処理
        setCustomerName(gCustInfoJsonList);
        // $01 end ウェルカムボード仕様変更対応 
    }
    // 新規顧客
    else {
        setMessageMargin()
    }
    
    icropBase.Execute('icrop:log:SC3100304_DebugLog_END CallFunc[SC3100304Update] SC3100304UpdateParm[aCustomerClass=' + aCustomerClass + ' ,aCustomerName=' + aCustomerName + ']');
}

// 顧客名表示処理
function setCustomerName(aCustomerName) {

    removeCustomerName()
    
	// $01 start ウェルカムボード仕様変更対応 
    var i = 0;
    var fontSize = false;
    var fontSizeNow = 0;
    // 顧客情報リスト中から3件分表示
    while ((aCustomerName[i] != null) && (i < C_MAX_CUSTOMER_NUM)) {
        var addCustomer = null;
        addCustomer = $('<li><span class="userName ellipsis">' + aCustomerName[i].CustName + '</li>');
        $('#WelcomeCustomerList').append(addCustomer);

        // フォントサイズの調整処理
        fontSize = false;
        fontSizeNow = C_FONT_SIZE_MAX;
        var custName = addCustomer.children('.userName');
        while (!fontSize) {
            if (C_NAME_WIDTH_MAX - 20 >= custName.width()) {
                // 顧客名称が表示幅内に収まっていれば処理を抜ける。
                fontSize = true;
            }
            else {
                // 顧客名称が表示幅内に収まっていなければフォントサイズを1px小さくする。
                fontSizeNow = fontSizeNow - 1;
                custName.css('font-size', fontSizeNow + 'px');
            }
        }
        custName.css('width', C_NAME_WIDTH_MAX + 'px');
        i++;
    }

    // 表示高さの調整
    if (i == 0) {
        $('.DummyCellMiddle').css('height', '90px');
    }
    else {
        $('.DummyCellMiddle').css('height', '0px');
    }
    // $01 end ウェルカムボード仕様変更対応 

    setMessageMargin()
}

// 顧客名表示削除処理
function removeCustomerName() {

    var ul = document.getElementById('CenteringCell').getElementsByTagName('ul')[0];
    while (ul.getElementsByTagName('li')[0]) {
        var li = ul.getElementsByTagName('li')[0];
        ul.removeChild(li)
    }
}

// 表示内容の垂直センタリング
function setMessageMargin() {

    var topMargin = ($('#AddRegion').outerHeight() - 50 - $('#CenteringCell').outerHeight()) / 2;
    $('#WelcomeMain .CenteringCell').css('margin-top', topMargin + 'px');
}


// $01 start ウェルカムボード仕様変更対応 
/**
* 顧客情報リストから特定の顧客情報を削除する.
*/
function DeleteCustData(aDelList) {
    var custInfoJsonListCopy = [];
    var delFlag = false;
    for (i = 0; i <= gCustInfoJsonList.length - 1; i++) {
        delFlag = false;
        for (j = 0; j <= aDelList.length - 1; j++) {
            if (i == aDelList[j]) {
                delFlag = true;
            }
        }
        if (!delFlag) {
            custInfoJsonListCopy.push(gCustInfoJsonList[i]);
        }
    }
    gCustInfoJsonList = custInfoJsonListCopy;
}
/**
* 顧客チップクリックイベント.
*/
$('.userName').live('mousedown', function (aEvent) {

    // クリックされたチップの位置を取得
    var delList = [$('#WelcomeCustomerList').children('li').index($(aEvent.currentTarget).parents('li'))];

    // 顧客名の表示切替
    DeleteCustData(delList);
    setCustomerName(gCustInfoJsonList);
});
// $01 end ウェルカムボード仕様変更対応 
