﻿/// <reference path="jquery.js"/>
/// <reference path="eCRB.js"/>
/// <reference path="eCRB.ui.js"/>
/** 	
* @fileOverview マスターページ(iPod).	
* 	
* @author KN Hirose
* @version 1.0.0	
* 更新： 2012/05/23 KN 浅野 クルクル対応
*/

/****************************************************************************

マスターページに関する処理のjQUERY拡張

****************************************************************************/
(function (window) {
    $.extend({ master: {

        blinkIcropLogoTimer: null,

        //i-CROPアイコン点滅開始
        blinkStartIcropLogo: function() {
            this.blinkIcropLogoTimer = setInterval(function() {
                 $("#mstpg_icropLogo").is(":hidden") ? $("#mstpg_icropLogo").fadeIn(200) : $("#mstpg_icropLogo").fadeOut(200);
            }, 200);
        },

        //i-CROPアイコン点滅終了
        blinkEndIcropLogo: function() {
            if (this.blinkIcropLogoTimer) clearInterval(this.blinkIcropLogoTimer);
            $("#mstpg_icropLogo").show(0);
        },

        OpenLoadingScreen: function () {
            $("#MstPG_LoadingScreen").css({ "width": $(window).width() + "px", "height": $(window).height() + "px" });
            setTimeout(function () {
                $("#MstPG_LoadingScreen").css({ "display": "table"});
            }, 0);
        },

        CloseLoadingScreen: function() {
            $("#MstPG_LoadingScreen").css({ "display": "none" });
        }
    }});
})(window);


/****************************************************************************

キーボード制御

****************************************************************************/
$(function () {

    $(document).keydown(function (e) {
        
        if (e.which != 13) return true; //13:Enterキー(Goボタン)
        var tclass = (e.target.className).toUpperCase();

        if (tclass == "VALIDKBPROTECT") {
            e.target.blur();
            return false;
        }

        var tagName = (e.target.tagName).toUpperCase();
        if (tagName != "INPUT" && tagName != "SELECT") return true;
        if (tagName == "INPUT") {
            var type = (e.target.type).toUpperCase();
            if (type == "SEARCH" || type == "PASSWORD") return true;
        }

        if (tclass == "UNVALIDKBPROTECT") return true;
        e.target.blur();
        return false;
    });

});

var timerClearTime = 0;

//2012/05/23 KN 浅野  クルクル対応 START
/**
* 再表示タイマーをセットする.
* 
* @param {refreshFunc} 再表示用のJavaScript関数 -
* @return {-} -
* 
* @example 
*  -
*/
function commonRefreshTimer(refreshFunc) {

    // タイマー間隔の取得
    var refreshTime = Number($("#MstPG_RefreshTimerTime").val());

    // タイマー時間が取得できなければタイマーの設定は行わない。
    if (refreshTime == 0) {
        return;
    }

    //開始時間を保持する
    var startTime = new Date().getTime();

    setTimeout(function () {

        //clearTimer()がされている場合は処理しない
        if (startTime <= timerClearTime) {
            return;
        }

        //出力メッセージを選択する
        var messageString = $("#MstPG_RefreshTimerMessage1").val();

        //警告メッセージ出力
        alert(messageString);

        //各画面でリフレッシュ処理をする
        if (refreshFunc() === false) {
            //falseが帰ってきたら再読み込み処理をしない
            return;
        }

        //再度、タイマーをセットする
        commonRefreshTimer(refreshFunc)

    }, refreshTime);
}

/**
* 再表示タイマーをリセットする.
* 
* @param {-} - -
* @return {-} -
* 
* @example 
*  -
*/
function commonClearTimer() {
    //現在時、以前のタイマーを無視する
    timerClearTime = new Date().getTime();
}
//2012/05/23 KN 浅野  クルクル対応 END
