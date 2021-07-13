//---------------------------------------------------------
//Common.js
//---------------------------------------------------------
//機能：共通関数
//作成：2012/12/22 TMEJ 張 タブレット版SMB機能開発(工程管理)
//更新：2013/11/29 TMEJ 張 タブレット版SMB チーフテクニシャン機能開発
//更新：2013/11/29 TMEJ 丁 タブレット版SMB チーフテクニシャン機能開発
//更新：2014/09/12 TMEJ 張 BTS-390 「1ｽﾄｰﾙで2ﾁｯﾌﾟが作業中（表示のみ）」対応
//更新：2017/08/30 NSK 竹中(悠) REQ-SVT-TMT-20161109-001 SMB iPadにストールクローズ機能の追加
//---------------------------------------------------------
/**
* 1桁の数値を2桁の文字列として返す
*
* @param {Date} lngValue format前の値
* @return {String} format後の値
*/
function add_zero(lngValue) {

    var strResult = lngValue.toString();

    if (lngValue < 10) {
        strResult = "0" + strResult;
    }

    return strResult;
}
/**
* データがnullの場合、""を返却する
* @param {Date} lngValue format前の値
* @return {Integer} format後の値
*/
function transferNullToBlank(strValue) {

    if (strValue == null) {
        return "";
    }
    return strValue;
}

/**
* Stringのleft関数
*
* @param {Date} mainStr 元のstr
* @param {Date} lngLen 左から何桁目
* @return {Integer} format後の値
*/
function left(mainStr, lngLen) {
    if (mainStr == null) {
        return null;
    }
    if (lngLen > 0) {
        return mainStr.substring(0, lngLen);
    } else {
    return null;
    }
}
/**
* Stringのright関数
*
* @param {Date} mainStr 元のstr
* @param {Date} lngLen 右から何桁目
* @return {Integer} format後の値
*/
function right(mainStr, lngLen) {
    if (mainStr == null) {
        return null;
    }
    if (mainStr.length - lngLen >= 0 && mainStr.length >= 0 && mainStr.length - lngLen <= mainStr.length) {
        return mainStr.substring(mainStr.length - lngLen, mainStr.length);
    } else {
        return null;
    }
}
/**
* HH:MM形式のデータ→date形式に転換する
* @param {String} strDate HH:MM
* @return {Integer} date形式の値
*/
function convertHHMMToDt(strDate) {
    if ((strDate) && (strDate != "")) {
        return new Date($("#hidShowDate").val() + " " + strDate + ":00");
    } else {
        return "";
    }
}
/**
* HTMLのencode
* @param {String} encode前の値
* @return {String} encode後の値
*/
function htmlEncode(text) {
    try {
        return text.replace(/&/g, '&amp').replace(/\"/g, '&quot;').replace(/</g, '&lt;').replace(/>/g, '&gt;');
    }
    catch (e) {
        return "";
    }
}
/**
* HTMLのdecode
* @param {String} decode前の値
* @return {String} decode後の値
*/
function htmlDecode(text) {
    try {
        return text.replace(/&amp;/g, '&').replace(/&quot;/g, '"').replace(/&lt;/g, '<').replace(/&gt;/g, '>');
    }
    catch (e) {
        return "";
    }
}
/**
* strStringのlengthがnLimitNumを超える場合、左からnLimitNum-1桁を切って、後のは...をする
* @param {String} LimitString前の値
* @param {Integer} nLimitNum 限界桁
* @return {String} LimitString後の値
*/
function LimitString(strString, nLimitNum) {
    // strStringのlengthがnLimitNumを超える場合
    if (strString.length > nLimitNum) {
        // 左からnLimitNum-1桁を切って、後は...をする
        strString = left(strString, nLimitNum - 1) + "...";
    }
    return strString;
}
/**
* 指定のvalが配列の中の位置を取得する
* @param {Date} val 
* @return {array} 指定のvalが配列の中の位置
*/
Array.prototype.indexOf = function (val) {
    for (var i = 0; i < this.length; i++) {
        if (this[i] == val) {
            return i;
        }
    }
    return -1;
}
/**
* 値により、arrayからアイテムを削除する
* @param {Date} val 削除したアイテムの値
* @return {array} 削除した後配列
*/
Array.prototype.removeValue = function (val) {
    var index = this.indexOf(val);
    if (index > -1) {
        this.splice(index, 1);
    }
}

/**
* indexにより、arrayからアイテムを削除する
* @param {Integer} index
* @return {array} 削除した後配列
*/
Array.prototype.remove = function (dx) {
    if (isNaN(dx) || dx > this.length) {
        return false;
    }
    for (var i = 0, n = 0; i < this.length; i++) {
        if (this[i] != this[dx]) {
            this[n++] = this[i]; 
        }
    }
    this.length -= 1;
}
/**
* 左方向でスペースを削除する
* @param {String} str
* @return {array} 削除した後配列
*/
String.prototype.LTrim = function () {
    return this.replace(/(^\s*)/g, "");
} 
/**
* 右方向でスペースを削除する
* @param {String} str
* @return {array} 削除した後配列
*/
String.prototype.RTrim = function () {
    return this.replace(/(\s*$)/g, "");
} 
/**
* 両方で
* @param {String} str
* @return {array} 削除した後配列
*/
String.prototype.Trim = function () {
    return this.replace(/(^\s*)|(\s*$)/g, "");
}

//コールバック隊列構造体
function OperationObj(callMethodName, parameter, callBackMethodName, excuteMethodName) {
    // コールバック実行関数
    this.callMethodName = callMethodName;
    // パラメタ
    this.parameter = parameter;
    // コールバック後実行関数
    this.callBackMethodName = callBackMethodName;
    // 実行メッソド名
    this.excuteMethodName = excuteMethodName;
}

//コールバック開始
function DoCallBack(callMethodName, parameter, callBackMethodName, excuteMethodName) {

    // 2014/09/12 TMEJ 張 BTS-390 「1ｽﾄｰﾙで2ﾁｯﾌﾟが作業中（表示のみ）」対応 START
    // リフレッシュ用更新日時をJsonストリングに設定
    parameter.PreRefreshDateTime = GetPreRefreshDate();
    // 2014/09/12 TMEJ 張 BTS-390 「1ｽﾄｰﾙで2ﾁｯﾌﾟが作業中（表示のみ）」対応 END

    if (gOperationList.length == 0) {
        switch (callMethodName) {
            case C_CALLBACK_WND101:
                //コールバック開始
                gCallbackSC3240101.doCallback(parameter, callBackMethodName);
                break;
            case C_CALLBACK_WND201:
                //コールバック開始
                gCallbackSC3240201.doCallback(parameter, callBackMethodName);
                break;
            case C_CALLBACK_WND301:
                //コールバック開始
                gCallbackSC3240301.doCallback(parameter, callBackMethodName);
                break;
            // 2013/11/29 TMEJ 張 タブレット版SMB チーフテクニシャン機能開発 START 
            case C_CALLBACK_WND501:
                gCallbackSC3240501.doCallback(parameter, callBackMethodName);
                break;
            // 2013/11/29 TMEJ 張 タブレット版SMB チーフテクニシャン機能開発 END  

            //2017/08/30 NSK 竹中(悠) REQ-SVT-TMT-20161109-001 SMB iPadにストールクローズ機能の追加 START 
            case C_CALLBACK_WND701:
                gCallbackSC3240701.doCallback(parameter, callBackMethodName);
                break;
            //2017/08/30 NSK 竹中(悠) REQ-SVT-TMT-20161109-001 SMB iPadにストールクローズ機能の追加 END
        }
    } 
    var objSubOperation = new OperationObj(callMethodName, parameter, callBackMethodName, excuteMethodName);
    ReplaceOperationList(objSubOperation);
}

//コールバック終了
function AfterCallBack() {
    // 実行終わった関数をリストから削除する
    if (gOperationList.length > 0) {
        gOperationList.shift();
    }
    // 次の関数を送信する
    if (gOperationList.length > 0) {
        switch (gOperationList[0].callMethodName) {
            case C_CALLBACK_WND101:
                //メイン画面コールバック開始
                gCallbackSC3240101.doCallback(gOperationList[0].parameter, gOperationList[0].callBackMethodName);
                break;
            case C_CALLBACK_WND201:
                //コールバック開始
                gCallbackSC3240201.doCallback(gOperationList[0].parameter, gOperationList[0].callBackMethodName);
                break;
            case C_CALLBACK_WND301:
                if (GetOperationType(gOperationList[0].parameter.ButtonID) == C_OPERATIONTYPE_OTHER) {
                //サブボックス開く以外の処理なら開始
                    //コールバック開始
                    gCallbackSC3240301.doCallback(gOperationList[0].parameter, gOperationList[0].callBackMethodName);
                } else if (gOpenningSubBoxId == gOperationList[0].parameter.ButtonID) {
                //正しいサブボックスが開いている状態のみ処理を開始
                    //コールバック開始
                    gCallbackSC3240301.doCallback(gOperationList[0].parameter, gOperationList[0].callBackMethodName);
                } else {
                    //サブボックスがとじられたら、コールバックしない、次の処理を開始
                    AfterCallBack();
                }
                break;
            // 2013/11/29 TMEJ 張 タブレット版SMB チーフテクニシャン機能開発 START
            case C_CALLBACK_WND501:
                gCallbackSC3240501.doCallback(gOperationList[0].parameter, gOperationList[0].callBackMethodName);
                break;
            // 2013/11/29 TMEJ 張 タブレット版SMB チーフテクニシャン機能開発 END 

            //2017/08/30 NSK 竹中(悠) REQ-SVT-TMT-20161109-001 SMB iPadにストールクローズ機能の追加 START  
            case C_CALLBACK_WND701:
                gCallbackSC3240701.doCallback(gOperationList[0].parameter, gOperationList[0].callBackMethodName);
                break;
            //2017/08/30 NSK 竹中(悠) REQ-SVT-TMT-20161109-001 SMB iPadにストールクローズ機能の追加 END 
        }
    }
}

//操作リストをクリアする
function ClearOperationList() {
    gOperationList = new Array();
} 


/**
* 操作リストに処理を追加する
* @param {List} addItem　指定項目
*/
function ReplaceOperationList(addItem){
    //実行中の処理は対象外
    if(gOperationList.length > 1){
        var replaceFlg = false;
        //サブチップボックスの場合
        if ((addItem.callMethodName == C_CALLBACK_WND301) && (GetOperationType(addItem.parameter.ButtonID) == C_OPERATIONTYPE_GETSUBBOXCHIP)) {
            //gOperationListをループして、必要ない処理を書き換える/削除する
            for (var i = 1; i < gOperationList.length; ++i) {
                switch (gOperationList[i].excuteMethodName) {
                    case "ShowReceptionchip":
                    case "ShowAddWorkchip":
                    case "ShowCompletionchip":
                    case "ShowCarWashchip":
                    case "ShowDeliverdCarchip":
                    case "ShowNoShowchip":
                    case "ShowStopchip":
                        if (replaceFlg) {
                            gOperationList.splice(i, 1);
                        } else {
                            gOperationList.splice(i, 1, addItem);
                            replaceFlg = true;
                        }
                        break;
                }
            }
            // 2013/11/29 TMEJ 張 タブレット版SMB チーフテクニシャン機能開発 START
        } else if ((addItem.callMethodName == C_CALLBACK_WND101)
            && (addItem.excuteMethodName == "GetTechnicians")) {
            // テクニシャンエリア開く時、前のリストにあれば、取得しないように
            for (var i = 1; i < gOperationList.length; ++i) {
                if (gOperationList[i].excuteMethodName == "GetTechnicians") {
                    replaceFlg = true;
                    break;
                }
            }
            // 2013/11/29 TMEJ 張 タブレット版SMB チーフテクニシャン機能開発 END
        } 
        if(!replaceFlg){
            gOperationList.push(addItem); 
        }
    }else{
        gOperationList.push(addItem);   
    }
}

//2013/11/29 TMEJ 丁 タブレット版SMB チーフテクニシャン機能開発 START
/**
* 日付フォマット
* @param {inDate} Date　指定日付
* @param {fmt} String　変換したいフォマット
*/
function DateFormat(inDate, fmt) {
    var reDate = fmt;
    var o = {
        "M+": inDate.getMonth() + 1,                 //月  
        "d+": inDate.getDate(),                    //日   
        "H+": inDate.getHours(),                   //時  
        "m+": inDate.getMinutes(),                 //分   
        "s+": inDate.getSeconds(),                 //秒 
        "q+": Math.floor((inDate.getMonth() + 3) / 3), //季節   
        "S": inDate.getMilliseconds()             //ミリ秒   
    };
    if (/(y+)/.test(fmt))
        reDate = reDate.replace(RegExp.$1, (inDate.getFullYear() + "").substr(4 - RegExp.$1.length));
    for (var k in o)
        if (new RegExp("(" + k + ")").test(fmt))
            reDate = reDate.replace(RegExp.$1, (RegExp.$1.length == 1) ? (o[k]) : (("00" + o[k]).substr(("" + o[k]).length)));
    return reDate;
}

//2013/11/29 TMEJ 丁 タブレット版SMB チーフテクニシャン機能開発 END  