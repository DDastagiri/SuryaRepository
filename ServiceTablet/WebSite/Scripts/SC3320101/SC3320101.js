//---------------------------------------------------------
//SC3320101.js
//---------------------------------------------------------
//機能：メイン画面処理
//作成：2014/08/14 TMEJ 丁 メインメニュー(移動マン)
//---------------------------------------------------------


//DOMロード直後の処理(重要事項).
//@return {void}
$(function () {
    //hiddenコントロールの日付フォマートを取得する
    if ((document.getElementById("hidDateFormatMMdd").value != "") && (document.getElementById("hidDateFormatHHmm").value != "")) {
        gDateFormat = document.getElementById("hidDateFormatMMdd").value + " " + document.getElementById("hidDateFormatHHmm").value;
    }

    //クライアントとサーバとの時間の差を設定
    SetServerTimeDifference();
    //スクロール
    $("#VisitInfoContents").SC3320101fingerScroll();
    $(".scroll-inner").css({ "top": C_SC3320101SCR_DEFAULTTOP });

    //タイトル文言設定する
    SetDisplayTitle($("#HeadTitleHidden").val());

    //画面初期化
    RefreshASA();

    $(".SearchIcon").bind("click", function (e) {

        if (gEditeFlg) {
            return;
        }
        if (!gSearchFlg) {
            $(".SearchArea").css({
                "-webkit-transition": "400ms linear",
                "width": "461px"
            });
            $("#search").css({
                "-webkit-transition": "400ms linear",
                "width": "376px"
            });
            $("#search").click();
        }
    });
});

/**
* 画面最新化
* @return {なし}
*/
function RefreshASA() {
    //アクティブインジケーター
    $.master.OpenLoadingScreen();
    //リフレッシュタイマーセット
    commonRefreshTimer(RefreshASA);
    // 定期リフレッシュをやめる
    clearInterval(gFuncRefreshTimerInterval);
    gFuncRefreshTimerInterval = "";
    //callbackで最新データを取得する
    var param = new Array();
    param[0] = {
        MethodName: "Refresh"
    };
    gCallbackSC3320101.doCallback(param, SC3320101AfterCallBack);

}

/**
* 定期リフレッシュ
* @return {なし}
*/
function AutoRefreshASA() {
    //テキスト編集中は定期リフレッシュしない
    if (gEditeFlg) {
        return;
    }
//    //テキストを変更された時は定期リフレッシュしない
//    var blChangFlg = false;
//    for (var visitSeq in gArrObjVisitInfo) {
//        var strParkingCd = $("#" + visitSeq + "_" + C_ID_ALP)[0].value.toString().Trim() + $("#" + visitSeq + "_" + C_ID_NUM)[0].value.toString().Trim();
//        if (gArrObjVisitInfo[visitSeq] != undefined) {
//            if (gArrObjVisitInfo[visitSeq].ParkingCode != strParkingCd) {
//                blChangFlg = true;
//                break;
//            }
//        }
////    }
//    if (blChangFlg) {
//        return;
//    }
    //アクティブインジケーター
    $.master.OpenLoadingScreen();
    //リフレッシュタイマーセット
    commonRefreshTimer(RefreshASA);
    // 定期リフレッシュをやめる
    clearInterval(gFuncRefreshTimerInterval);
    gFuncRefreshTimerInterval = "";

    //callback
    var param = new Array();
    param[0] = {
        MethodName: "Refresh"
    };
    gCallbackSC3320101.doCallback(param, SC3320101AfterCallBack);

}


/**
* PullDownRefresh画面最新化
* @return {なし}
*/
function PullDownRefresh() {
    //見えないフィルムを貼る
    $("#LoadingScreen").css({ "display": "block" });
    if ($(".Bottom_TBL tr").length - $(".Bottom_TBL tr.DisplayOff").length > 10) {
        $(".Bottom_TBL tr").addClass("DisplayOff");
        $(".Bottom_TBL tr").removeClass("DisplayOn");
        $(".Bottom_TBL tr:nth-of-type(1)").removeClass("DisplayOff");
        $(".Bottom_TBL tr:nth-of-type(1)").addClass("DisplayOn");
        $(".Bottom_TBL tr:nth-of-type(2)").removeClass("DisplayOff");
        $(".Bottom_TBL tr:nth-of-type(2)").addClass("DisplayOn");
        $(".Bottom_TBL tr:nth-of-type(3)").removeClass("DisplayOff");
        $(".Bottom_TBL tr:nth-of-type(3)").addClass("DisplayOn");
        $(".Bottom_TBL tr:nth-of-type(4)").removeClass("DisplayOff");
        $(".Bottom_TBL tr:nth-of-type(4)").addClass("DisplayOn");
        $(".Bottom_TBL tr:nth-of-type(5)").removeClass("DisplayOff");
        $(".Bottom_TBL tr:nth-of-type(5)").addClass("DisplayOn");
        $(".Bottom_TBL tr:nth-of-type(6)").removeClass("DisplayOff");
        $(".Bottom_TBL tr:nth-of-type(6)").addClass("DisplayOn");
        $(".Bottom_TBL tr:nth-of-type(7)").removeClass("DisplayOff");
        $(".Bottom_TBL tr:nth-of-type(7)").addClass("DisplayOn");
        $(".Bottom_TBL tr:nth-of-type(8)").removeClass("DisplayOff");
        $(".Bottom_TBL tr:nth-of-type(8)").addClass("DisplayOn");
        $(".Bottom_TBL tr:nth-of-type(9)").removeClass("DisplayOff");
        $(".Bottom_TBL tr:nth-of-type(9)").addClass("DisplayOn");
        $(".Bottom_TBL tr:nth-of-type(10)").removeClass("DisplayOff");
        $(".Bottom_TBL tr:nth-of-type(10)").addClass("DisplayOn");
    }
//    //アクティブインジケーター
//    $.master.OpenLoadingScreen();
    //リフレッシュタイマーセット
    commonRefreshTimer(RefreshASA);
    // 定期リフレッシュをやめる
    clearInterval(gFuncRefreshTimerInterval);
    gFuncRefreshTimerInterval = "";
    var param = new Array();

    //callback
    param[0] = {
        MethodName: "Refresh"
    };
    gCallbackSC3320101.doCallback(param, SC3320101AfterCallBack);

}


/**
* 画面初期化
* @return {なし}
*/
function InitPage() {
    //定期リフレッシュタイムの取得
    gRefreshTimerInterval = Number($("#RefureshTimeHidden").val());
    // intervalで定期リフレッシュを行う
    if (gFuncRefreshTimerInterval == "") {
        gFuncRefreshTimerInterval = setInterval("AutoRefreshASA()", gRefreshTimerInterval * 1000);
    }
    //画面設定
    SetMainMenu();
 
}


/**
* 画面表示設定
* @return {なし}
*/
function SetMainMenu() {
    // グロバール変数の初期化
    gArrObjVisitInfo = null;
    gArrObjVisitInfo = new Array();
    var count = 0;
    var nVisitSeq;
    //ロケーション番号
    var strParkingCd;
    //ロケーション番号数字部分
    var strParkingCdNum;
    //ロケーションアルファベット部分
    var strParkingAlp;
    //アルファベットの正規表現
    var regExp = /^[A-Za-z]+$/;
    //車両登録番号
    var strRegNum;
    var detailSTableVisitTr = $(".Bottom_TBL tr");

    //サーチテキストボックスをクリア
    $("#search").val("");
    $("#search")[0].defaultValue = "";
    $(".SearchArea").removeAttr("style");
    $("#search").removeAttr("style");
    $(".SearchArea").css("width", "0px");
    $("#search").css("width", "0px");
    //テーブルの行数分をループしてレイアウトを調整する
    detailSTableVisitTr.each(function (i, elem) {
        count = i + 1;

        if ($(this).find("#ParkingCodeNumTxt").length > 0) {
            nVisitSeq = Number($(this).find("#ParkingCodeNumTxt").attr("visitseq"));
            //アルファード
            strParkingAlp = $(this).find("#ParkingCodeAlpTxt")[0].value.toString().Trim();
            //数字
            strParkingCdNum = $(this).find("#ParkingCodeNumTxt")[0].value.toString().Trim();
            //完全体
            strParkingCd = strParkingAlp + strParkingCdNum;
            if (!regExp.test(strParkingAlp)) {
                $(this).find("#ParkingCodeAlpTxt").val("");
                $(this).find("#ParkingCodeAlpTxt")[0].defaultValue = "";
                if (strParkingCd.length < 3) {
                    $(this).find("#ParkingCodeNumTxt").val(strParkingCd);
                    $(this).find("#ParkingCodeNumTxt")[0].defaultValue = strParkingCd;
                } else {
                    $(this).find("#ParkingCodeNumTxt").val(strParkingCd.substr(1, 2));
                    $(this).find("#ParkingCodeNumTxt")[0].defaultValue = strParkingCd.substr(1, 2);
                }
            }


            //車両登録番号
            strRegNum = $(this).find("#ParkingCodeNumTxt").attr("regnum");
            if (gArrObjVisitInfo[nVisitSeq] == undefined) {
                gArrObjVisitInfo[nVisitSeq] = new SetVisitInfoObj(nVisitSeq, strParkingCd, strRegNum);
                $(this).find("#ParkingCodeNumTxt").attr("id", nVisitSeq + "_" + C_ID_NUM);
                $(this).find("#ParkingCodeAlpTxt").attr("id", nVisitSeq + "_" + C_ID_ALP);

//                $("#" + nVisitSeq + "_" + C_ID_NUM)[0].pattern = "[0-9]*"
                //                $("#" + nVisitSeq + "_" + C_ID_ALP)[0].pattern = "[A-Z]*"
                //ロケーションコードのテキストにイベントBIND
                //                BindLoctionCodeText(nVisitSeq);
            }

            //背景色を設定する
            if (strParkingCd == "") {
                if ((count / 2) % 2 == 0) {
                    $(".Bottom_TBL tr:nth-of-type(" + i + ")").addClass("TC_BG03");
                    $(".Bottom_TBL tr:nth-of-type(" + count + ")").addClass("TC_BG03");
                } else {
                    $(".Bottom_TBL tr:nth-of-type(" + i + ")").addClass("TC_BG02");
                    $(".Bottom_TBL tr:nth-of-type(" + count + ")").addClass("TC_BG02");
                }
            } else {
                if ((count / 2) % 2 == 0) {
                    $(".Bottom_TBL tr:nth-of-type(" + i + ")").addClass("TC_BG08");
                    $(".Bottom_TBL tr:nth-of-type(" + count + ")").addClass("TC_BG08");
                } else {
                    $(".Bottom_TBL tr:nth-of-type(" + i + ")").addClass("TC_BG07");
                    $(".Bottom_TBL tr:nth-of-type(" + count + ")").addClass("TC_BG07");
                }
            }

        }

    });

//    $(".TC_Button02").removeClass("TC_BG09");
//    //Registボタンの制御
//    if (count == 0) {
//        $(".TC_Button02").addClass("TC_BG09");
//        $(".TC_Button02").unbind();
//    } else {
//        $(".TC_Button02").addClass("TC_BG04");
////        $(".TC_Button02").unbind().bind("click", function (e) {
////            ClickRegistBtn();
////        });
//    }

    //スクロールを設定
    nheight = count * C_SC3320101TA_DEFAULTHEIGHT-2;
    if (nheight < C_SC3320101SCR_DEFAULTHEIGHT) {
        nheight = C_SC3320101SCR_DEFAULTHEIGHT;
    }
    //スクロールのサイズを調整
    $(".scroll-inner").css({ "height": nheight, "width": C_SC3320101SCR_DEFAULTWIDTH, "top": C_SC3320101SCR_DEFAULTTOP });

    //更新時間を設定する
    $("#MessageUpdateTime").text(getUpdateTime());
}


/**
* Regist押す処理
* @return {なし}
*/
function ClickRegistBtn() {
    //アクティブインジケーター
    $.master.OpenLoadingScreen();

    //リフレッシュタイマーセット
    commonRefreshTimer(RefreshASA);

    // 定期リフレッシュをやめる
    clearInterval(gFuncRefreshTimerInterval);
    gFuncRefreshTimerInterval = "";

    // グロバール変数の初期化
    gArrObjUpdVisitInfo = null;
    gArrObjUpdVisitInfo = new Array();
    var nVisitSeq;
    var strParkingCd;
    var nArrObjVisitInfo = new Array();

    if (gSelectedVisitSeq == "") {
        //アクティブインジケーターを消す
        $.master.CloseLoadingScreen();
        //タイマーをクリア
        commonClearTimer();
        // intervalを再開する(定期リフレッシュ)
        if (gFuncRefreshTimerInterval == "") {
            gFuncRefreshTimerInterval = setInterval("AutoRefreshASA()", gRefreshTimerInterval * 1000);
        }
        return;
    }
    nVisitSeq = gSelectedVisitSeq;

    AllDisplay();

    strParkingCd = $("#" + nVisitSeq + "_" + C_ID_ALP)[0].value.toString().Trim() + $("#" + nVisitSeq + "_" + C_ID_NUM)[0].value.toString().Trim();
    if (gArrObjVisitInfo[nVisitSeq] != undefined) {
        if (gArrObjVisitInfo[nVisitSeq].ParkingCode != strParkingCd) {
            nArrObjVisitInfo[nVisitSeq] = new SetVisitInfoObj(nVisitSeq, strParkingCd, gArrObjVisitInfo[nVisitSeq].RegNum)
            gArrObjUpdVisitInfo.push(nArrObjVisitInfo[nVisitSeq]);
        }
    }

    if (gArrObjUpdVisitInfo.length > 0) {
        gCallbackSC3320101.doCallback(gArrObjUpdVisitInfo, SC3320101AfterCallBack);
    } else {
//        var errorMsg = $("#NotChangeErrMsgHidden").val();
//        icropScript.ShowMessageBox("", errorMsg, "");
        //アクティブインジケーターを消す
        $.master.CloseLoadingScreen();
        //タイマーをクリア
        commonClearTimer();
        // intervalを再開する(定期リフレッシュ)
        if (gFuncRefreshTimerInterval == "") {
            gFuncRefreshTimerInterval = setInterval("AutoRefreshASA()", gRefreshTimerInterval * 1000);
        }
    }
    
    
}


/**
* 来店情報オブジェクト作成
* @param {Integer} nVisitSeq 来店シーケンス
* @param {String} strParkingCode ロケーションコード
* @return {なし}
*/
function SetVisitInfoObj(nVisitSeq, strParkingCode,strRegNum) {
    //来店シーケンス
    this.VisitSeq = nVisitSeq;

    //ロケーションコード
    this.ParkingCode = strParkingCode;

    //車両登録番号
    this.RegNum = strRegNum;
}

/**
* コールバック後の処理関数
* 
* @param {String} result コールバック呼び出し結果
*
*/
function SC3320101AfterCallBack(result) {
    var jsonResult = JSON.parse(result);
    //アクティブインジケーターを消す
    $.master.CloseLoadingScreen();
    $("#LoadingScreen").css({ "display": "none" });
    //タイマーをクリア
    commonClearTimer();

    //コールバック結果コードの取得
    var resultCD = jsonResult.ResultCode;

    if (resultCD == 0) {
        //画面最新化
//        HideKeyBoard();
        SetMainmenuContents(jsonResult.Contents);
        InitPage();
//        //画面更新後の処理
        endRefresh();
    } else if ((resultCD == -1) || (resultCD == 3)) {
        //エラーメッセージを表示する
        var errorMsg = htmlDecode(jsonResult.Message);
        icropScript.ShowMessageBox("", errorMsg, "");
        //画面最新化
//        HideKeyBoard();
        SetMainmenuContents(jsonResult.Contents);
        InitPage();
    }else{
        var errorMsg = htmlDecode(jsonResult.Message);
        icropScript.ShowMessageBox("", errorMsg, "");
        // intervalを再開する(定期リフレッシュ)
        if (gFuncRefreshTimerInterval == "") {
            gFuncRefreshTimerInterval = setInterval("AutoRefreshASA()", gRefreshTimerInterval * 1000);
        }
    }
}

/**
* コールバックで取得したHTMLを画面に設定する
* 
* @param {String} result コールバック呼び出し結果
* 
*/
function SetMainmenuContents(result) {
    
    //コールバックによって取得したHTMLを格納
    var contents = $('<Div>').html(result).text();

    //画面のコンテンツを取得
    var visitInfoContents = $(contents).find('.Bottom_TBL');

    //画面のコンテンツを設定
    $('.Bottom_TBL')[0].innerHTML = visitInfoContents[0].innerHTML

}


///**
//* ロケーションコードのテキストにイベントBIND
//* 
//* @id {String} コントロールのID
//* 
//*/
//function BindLoctionCodeText(id) {

////    //フォーカスInイベント
////    $("#" + id + "_" + C_ID_ALP).bind('focusin', function (e) {
////        SelectLoctionCodeText(id);
////    });

////    $("#" + id + "_" + C_ID_NUM).bind('focusin', function (e) {
////        SelectLoctionCodeText(id);
////    });


////    //フォーカスOutイベント
////    $("#" + id + "_" + C_ID_ALP).blur(function (e) {
////        UnSelectLoctionCodeText();
////    });
////    $("#" + id + "_" + C_ID_NUM).blur(function (e) {
////        UnSelectLoctionCodeText();
////    });

////    $("#" + id + "_" + C_ID_NUM).bind("change",function (e) {
////        e.preventDefault();
////        alert("aa");
////        $("#" + id + "_" + C_ID_NUM).focus();
////    });


////    $("#" + id + "_" + C_ID_ALP).bind("change", function (e) {
////        ChangeFocus(id);
////    });

//}


///**
//* ロケーションコードのテキストにフォーカスInイベント
//* 
//* @visitSeq {String} コントロールID
//* 
//*/
//function SelectLoctionCodeText(visitSeq) {
//    //フォーカスInのテキストのKeyを保存する
//    gSelectedVisitSeq = visitSeq;

//}

///**
//* ロケーションコードのテキストにフォーカスOutイベント
//* 
//* 
//*/
//function UnSelectLoctionCodeText() {
//    //フォーカスInのテキストのKeyをクリアする
//    gSelectedVisitSeq = "";
//}


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
* 画面の更新時間を返す.
* @return {Date}
*/
function getUpdateTime() {
    var dtPreRefreshDatetime = GetServerTimeNow();
    return DateFormat(dtPreRefreshDatetime, gDateFormat);
}


/**
* サーバの現在時刻を算出し、返す
* @return {Date}
* 
*/
function GetServerTimeNow() {
    var serverTime = new Date();    //サーバの現在時刻を算出  
    serverTime.setTime(serverTime.getTime() + gServerTimeDifference);
    return serverTime;
}

/**
* サーバとの時間差を算出し、グローバル変数に格納する.
* @return {void}
* 
*/
function SetServerTimeDifference() {
    var pageLoadServerTime = new Date($("#ServerTimeHidden").val());   //ページ読込時のサーバ時間を取得
    var pageLoadClientTime = new Date();    //クライアントの現在時刻を取得
    gServerTimeDifference = pageLoadServerTime - pageLoadClientTime;    //サーバとの時間差を算出し、格納（ミリ秒）
}



///**
//* RFIDでロケーション番号をセットする
//* @param {String} strParkCd RFIDで読み取った値
//* 
//*/
//function SetSelectLoctionCodeText(strParkCd) {
//    //選択されなかった場合はエラーを表示
//    if ((gSelectedVisitSeq == "") || ($("#" + gSelectedVisitSeq).length == 0)) {
//        var errorMsg = $("#NotSelectedErrMsgHidden").val();
//        icropScript.ShowMessageBox("", errorMsg, "");
//    } else {
//        $("#" + gSelectedVisitSeq).val(strParkCd);
//    }
//}

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

/**
* 文字列からspaceを消す
* @param {String} str
* @return {array} 削除した後配列
*/
String.prototype.Trim = function () {
    return this.replace(/(^\s*)|(\s*$)/g, "");
}

/**
* テキストの自動フォーカスイン
*/
function ChangeFocus() {
    if (targetTextBox.value.length == 1) {
        var targetId = targetTextBox.id + " ";
        targetId = targetId.replace(C_ID_ALP + " ", C_ID_NUM)
        $("#" + targetId).click();
    }
}



/**
*後方一致検索
* @param  @targetStr {String} ターゲット文字列
* @param  @searchStr {String} 検索文字列
*/
function IsMatchStringAfter(targetStr, searchStr) {
    var str = targetStr + " ";
    if (str.indexOf(searchStr + " ") !== -1) {
        return true;
    } else {
        return false;
    }
}

/**
*絞り込み表示
* @param  @keyword {String} 検索文字列
*/
function ScreenDisplay() {
    var keyword = $("#search")[0].value;
    var detailSTableVisitTr = $(".Bottom_TBL tr");
    var count;
    var secondCount;
    var rowCount=0;

    //テーブルの行数分をループしてレイアウトを調整す
    if (keyword.toString().Trim()) {
        $(".Bottom_TBL tr").addClass("DisplayOff");
        $(".Bottom_TBL tr").removeClass("DisplayOn");
        detailSTableVisitTr.each(function (i, elem) {
            count = i + 1;

            if ($(this).find("#RegNoLabel").length > 0) {
                nRegNum = $(this).find("#RegNoLabel")[0].textContent.toString().Trim();
                secondCount = count + 1;
                //絞り込み判断で、行を表示、非表示する
                if (IsMatchStringAfter(nRegNum, keyword)) {
                    $(".Bottom_TBL tr:nth-of-type(" + count + ")").removeClass("DisplayOff");
                    $(".Bottom_TBL tr:nth-of-type(" + count + ")").addClass("DisplayOn");
                    $(".Bottom_TBL tr:nth-of-type(" + secondCount + ")").removeClass("DisplayOff");
                    $(".Bottom_TBL tr:nth-of-type(" + secondCount + ")").addClass("DisplayOn");
                    rowCount = rowCount + 1;

                    //背景色を設定する
                    var alpLocaCode = $(".Bottom_TBL tr:nth-of-type(" + secondCount + ")").find(".Loc_BoxAlp")[0].children[0].defaultValue
                    var numLocaCode = $(".Bottom_TBL tr:nth-of-type(" + secondCount + ")").find(".Loc_BoxNum")[0].children[0].defaultValue
                    var locaCode = alpLocaCode + numLocaCode
                    if (locaCode == "") {
                        if (rowCount% 2 == 0) {
                            $(".Bottom_TBL tr:nth-of-type(" + count + ")").removeClass("TC_BG02 TC_BG03 TC_BG07 TC_BG08").addClass("TC_BG03");
                            $(".Bottom_TBL tr:nth-of-type(" + secondCount + ")").removeClass("TC_BG02 TC_BG03 TC_BG07 TC_BG08").addClass("TC_BG03");
                        } else {
                            $(".Bottom_TBL tr:nth-of-type(" + count + ")").removeClass("TC_BG02 TC_BG03 TC_BG07 TC_BG08").addClass("TC_BG02");
                            $(".Bottom_TBL tr:nth-of-type(" + secondCount + ")").removeClass("TC_BG02 TC_BG03 TC_BG07 TC_BG08").addClass("TC_BG02");
                        }
                    } else {
                        if (rowCount % 2 == 0) {
                            $(".Bottom_TBL tr:nth-of-type(" + count + ")").removeClass("TC_BG02 TC_BG03 TC_BG07 TC_BG08").addClass("TC_BG08");
                            $(".Bottom_TBL tr:nth-of-type(" + secondCount + ")").removeClass("TC_BG02 TC_BG03 TC_BG07 TC_BG08").addClass("TC_BG08");
                        } else {
                            $(".Bottom_TBL tr:nth-of-type(" + count + ")").removeClass("TC_BG02 TC_BG03 TC_BG07 TC_BG08").addClass("TC_BG07");
                            $(".Bottom_TBL tr:nth-of-type(" + secondCount + ")").removeClass("TC_BG02 TC_BG03 TC_BG07 TC_BG08").addClass("TC_BG07");
                        }
                    }
                }
            }      

        });
        //スクロールを設定
        var nheight = (rowCount*2 * C_SC3320101TA_DEFAULTHEIGHT) - 2;
        if (nheight < C_SC3320101SCR_DEFAULTHEIGHT) {
            nheight = C_SC3320101SCR_DEFAULTHEIGHT;
        }
        //スクロールのサイズを調整
        $(".scroll-inner").css({ "height": nheight, "width": C_SC3320101SCR_DEFAULTWIDTH, "top": C_SC3320101SCR_DEFAULTTOP });
    } else {
        ScreenDisplayClear();
    }
    //スクロール再開
    $("#VisitInfoContents").SC3320101fingerScroll({
        action: "restart"
    });
        //スクロールTopに戻す
    $("#VisitInfoContents").SC3320101fingerScroll({
        action: "move",
        moveY: $(".scroll-inner").position().top+4,
        moveX: $(".scroll-inner").position().left
    });
    if (gSearchFlg) {
        gSearchFlg = false;
    }

}

/**
*絞り込み解除
*/
function ScreenDisplayClear() {
    var detailSTableVisitTr = $(".Bottom_TBL tr");
    detailSTableVisitTr.removeClass("DisplayOff");
    detailSTableVisitTr.addClass("DisplayOn");
    //スクロールを設定
    var rowCount = $(".Bottom_TBL tr").length;
    var nheight = rowCount * C_SC3320101TA_DEFAULTHEIGHT - 2;
    if (nheight < C_SC3320101SCR_DEFAULTHEIGHT) {
        nheight = C_SC3320101SCR_DEFAULTHEIGHT;
    }
    //スクロールのサイズを調整
    $(".scroll-inner").css({ "height": nheight, "width": C_SC3320101SCR_DEFAULTWIDTH, "top": C_SC3320101SCR_DEFAULTTOP });

    detailSTableVisitTr.each(function (i, elem) {
        count = i + 1;

        if ($(".Bottom_TBL tr:nth-of-type(" + count + ")").find(".Loc_BoxAlp").length > 0) {

            //背景色を設定する
            var alpLocaCode = $(".Bottom_TBL tr:nth-of-type(" + count + ")").find(".Loc_BoxAlp")[0].children[0].defaultValue
            var numLocaCode = $(".Bottom_TBL tr:nth-of-type(" + count + ")").find(".Loc_BoxNum")[0].children[0].defaultValue
            var locaCode = alpLocaCode + numLocaCode
            if (locaCode == "") {
                if ((count / 2) % 2 == 0) {
                    $(".Bottom_TBL tr:nth-of-type(" + i + ")").removeClass("TC_BG02 TC_BG03 TC_BG07 TC_BG08").addClass("TC_BG03");
                    $(".Bottom_TBL tr:nth-of-type(" + count + ")").removeClass("TC_BG02 TC_BG03 TC_BG07 TC_BG08").addClass("TC_BG03");
                } else {
                    $(".Bottom_TBL tr:nth-of-type(" + i + ")").removeClass("TC_BG02 TC_BG03 TC_BG07 TC_BG08").addClass("TC_BG02");
                    $(".Bottom_TBL tr:nth-of-type(" + count + ")").removeClass("TC_BG02 TC_BG03 TC_BG07 TC_BG08").addClass("TC_BG02");
                }
            } else {
                if ((count / 2) % 2 == 0) {
                    $(".Bottom_TBL tr:nth-of-type(" + i + ")").removeClass("TC_BG02 TC_BG03 TC_BG07 TC_BG08").addClass("TC_BG08");
                    $(".Bottom_TBL tr:nth-of-type(" + count + ")").removeClass("TC_BG02 TC_BG03 TC_BG07 TC_BG08").addClass("TC_BG08");
                } else {
                    $(".Bottom_TBL tr:nth-of-type(" + i + ")").removeClass("TC_BG02 TC_BG03 TC_BG07 TC_BG08").addClass("TC_BG07");
                    $(".Bottom_TBL tr:nth-of-type(" + count + ")").removeClass("TC_BG02 TC_BG03 TC_BG07 TC_BG08").addClass("TC_BG07");
                }
            }
        }

    });

    if (gEditeFlg) {
        gEditeFlg = false;
    }
    if (gSearchFlg) {
        gSearchFlg = false;
    }

    gSelectedVisitSeq = "";

//    //スクロール再開
//    $("#VisitInfoContents").SC3320101fingerScroll({
//        action: "restart"
//    });
    $(".SearchArea").css({
        "-webkit-transition": "400ms linear",
        "width": "0px"
    });
    $("#search").css({
        "-webkit-transition": "400ms linear",
        "width": "0px"
    });

}
/**
*絞り込みキャンセル
*/
function ScreenDisplayCancel() {

    if (gSearchFlg) {
        gSearchFlg = false;
    }

    //スクロール再開
    $("#VisitInfoContents").SC3320101fingerScroll({
        action: "restart"
    });
}



/**
*全表示
*/
function AllDisplay() {
    $(".Bottom_TBL tr").removeClass("DisplayOff");
    $(".Bottom_TBL tr").addClass("DisplayOn");
    //スクロールを設定
    var rowCount = $(".Bottom_TBL tr").length;
    var nheight = rowCount * C_SC3320101TA_DEFAULTHEIGHT - 2;
    if (nheight < C_SC3320101SCR_DEFAULTHEIGHT) {
        nheight = C_SC3320101SCR_DEFAULTHEIGHT;
    }
    //スクロールのサイズを調整
    $(".scroll-inner").css({ "height": nheight, "width": C_SC3320101SCR_DEFAULTWIDTH, "top": C_SC3320101SCR_DEFAULTTOP });

    if (gEditeFlg) {
        gEditeFlg = false;
    }

    gSelectedVisitSeq = "";

//    //スクロール再開
//    $("#VisitInfoContents").SC3320101fingerScroll({
//        action: "restart"
//    });
}

///**
//*編集キャンセル
//*/
//function EditingCancel() {

//    if ($("#search")[0].value.length == 0) {

//        $(".Bottom_TBL tr").removeClass("DisplayOff");
//        //スクロールを設定
//        var rowCount = $(".Bottom_TBL tr").length;
//        var nheight = rowCount * C_SC3320101TA_DEFAULTHEIGHT - 2;
//        if (nheight < C_SC3320101SCR_DEFAULTHEIGHT) {
//            nheight = C_SC3320101SCR_DEFAULTHEIGHT;
//        }
//        //スクロールのサイズを調整
//        $(".scroll-inner").css({ "height": nheight, "width": C_SC3320101SCR_DEFAULTWIDTH, "top": C_SC3320101SCR_DEFAULTTOP });
//    }

//    if (gEditeFlg) {
//        gEditeFlg = false;
//    }

//    gSelectedVisitSeq = "";

//    //    //スクロール再開
//    //    $("#VisitInfoContents").SC3320101fingerScroll({
//    //        action: "restart"
//    //    });
//}


/**
*絞り込みモード切り替え
* @param  @targetControl {Object} コントロール
* @param  @keyboardType {int} キーボードタイプ
*/
function SwitchSearchMode(targetControl, keyboardType) {
    if (gEditeFlg) {
        return;
    } else {
        //キーボード表示
        ShowOriginalKeyBoard(targetControl, keyboardType, ScreenDisplay, ScreenDisplayCancel);
        //スクロール再開
        $("#VisitInfoContents").SC3320101fingerScroll({
            action: "stop"
        });
        gSearchFlg = true;
    }

} 


/**
*編集モード切り替え
* @param  @targetControl {Object} コントロール
* @param  @keyboardType {int} キーボードタイプ
*/
function SwitchEditingMode(targetControl, keyboardType) {

    if (gSearchFlg) {
        return;
    }


    //キーボード表示
    ShowOriginalKeyBoard(targetControl, keyboardType, ClickRegistBtn, AllDisplay);
            
//    //スクロール制御
//    $("#VisitInfoContents").SC3320101fingerScroll({
//        action: "stop"
//    });

    var detailSTableVisitTr = $(".Bottom_TBL tr");

    if ($(targetTextBox).length > 0) {
        var targetId = targetTextBox.id;
        gSelectedVisitSeq = targetId.split("_")[0];
    } else {
        return;
    }

    
    if (gEditeFlg) {
        return;
    }

    $(".Bottom_TBL tr").addClass("DisplayOff");
    $(".Bottom_TBL tr").removeClass("DisplayOn");
    detailSTableVisitTr.each(function (i, elem) {
        count = i + 1;

        if ($(this).find("#" + targetId).length > 0) {
            $(".Bottom_TBL tr:nth-of-type(" + count + ")").removeClass("DisplayOff");
            $(".Bottom_TBL tr:nth-of-type(" + count + ")").addClass("DisplayOn");
            $(".Bottom_TBL tr:nth-of-type(" + i + ")").removeClass("DisplayOff");
            $(".Bottom_TBL tr:nth-of-type(" + i + ")").addClass("DisplayOn");
            gEditeFlg = true;

            // 定期リフレッシュをやめる
            clearInterval(gFuncRefreshTimerInterval);
            gFuncRefreshTimerInterval = "";
            return false;
        }

    });
    //スクロールを設定
    $(".scroll-inner").css({ "height": 680, "width": C_SC3320101SCR_DEFAULTWIDTH, "top": C_SC3320101SCR_DEFAULTTOP });
    //スクロールTopに戻す
    $("#VisitInfoContents").SC3320101fingerScroll({
        action: "move",
        moveY: $(".scroll-inner").position().top + 4,
        moveX: $(".scroll-inner").position().left
        });

}
