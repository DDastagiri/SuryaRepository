//---------------------------------------------------------
//SC3340101.js
//---------------------------------------------------------
//機能：洗車マンメインメニュー(CW)画面
//作成：2015/01/05 TMEJ 範  NextSTEPサービス 洗車用端末開発向けた評価用アプリ作成
//---------------------------------------------------------

/****************************************
* 定数
****************************************/

// 日付デフォルト値
var C_DATE_DEFAULT_VALUE = Date.parse("1900/01/01 0:00:00");

// タッチ開始
var C_TOUCH_START = "touchstart mousedown";

// タッチで移動
var C_TOUCH_MOVE = "touchmove mousemove";

// タッチ終わり
var C_TOUCH_END = "touchend mouseup";

// 洗車待ち
var C_SVCSTATUS_CARWASHWAIT = "07";

//洗車中
var C_SVCSTATUS_CARWASHSTART = "08";

/****************************************
* グローバル変数宣言
****************************************/
//来店情報配列の初期化
var gArrObjCarWashInfo = new Array();

//ページ取得時のサーバとクライアントの時間差
var gServerTimeDifference = 0;

// データフォーマット：YYYY/MM/dd HH:mm
var gDateFormat = "YYYY/MM/dd HH:mm";

// 編集モードフラグ
var gEditFlg = false;

// 絞り込みモードフラグ
var gSearchFlg = false;

//件数
var gCount = 0;

//定期リフレッシュ時間（秒）
var gRefreshTime = 30;

//タイムアウト値
var gTimeoutValue;

//バナータップフラグ
var gBanaTapFlg = false;

//タップされたバナーのサービス入庫ID
var gTapSvcinId = "";

//chipTapイベント用フラグ
var gTouchStartFlg = false;

var gRefreshFlg = false;

//定期リフレッショ用
var gInterval = "";

var gFilterFlg = false;

//スクロール中
var gIsScrolling = false;

var gEyeSearchFlg = false;

var gKeyBoardFlg = false;

// N秒中に再タップが無効フラグ
var gTouchTimeoutFlg = false;


//チップタップイベント
//・ドラッグ時は動作しない
//・ダブルタップ時間内に再度タップがあれば動作しない
jQuery.event.special.chipTap = {
    setup: (function () {
        return function () {
            var touchStart = false;
            var touchMove = false;
            var singleTap = false;

            $(this).bind(C_TOUCH_START, function (event) {
                if (event.type == 'touchstart') {
                    gTouchStartFlg = true;
                } else {
                    if (gTouchStartFlg) {
                        return;
                    }
                }
                touchStart = true;
                touchMove = false;
                singleTap = !singleTap;
            });

            $(this).bind(C_TOUCH_MOVE, function (event) {
                if (event.type == 'touchmove') {
                } else {
                    if (gTouchStartFlg) {
                        return;
                    }
                }
                if (!touchStart) {
                    return;
                }

                touchMove = true;
                singleTap = false;  //タッチムーブ後にダブルタップした際、chipTap処理をしないよう制御
            });

            $(this).bind(C_TOUCH_END, function (event) {
                if (event.type == 'touchend') {
                } else {
                    if (gTouchStartFlg) {
                        return;
                    }
                }
                if (!touchStart) {
                    return;
                }
                if (touchMove) {
                    return;
                }
                touchStart = false;
                touchMove = false;

                var obj = $(this);
                obj.trigger("chipTap");
            });
        }
    })()
}

/**
* アクティブインジケータ表示操作関数定義
*/
var gMainAreaActiveIndicator = {

    show: function () {
        $.master.OpenLoadingScreen();
    }
    ,
    hide: function () {
        $.master.CloseLoadingScreen();
    }
};

/**
* 洗車情報オブジェクト作成
* @param {Object} objCarWash 洗車オブジェクト
* @return {なし}
*/
function SetCarWashInfoObj(objCarWash) {

    //サービス入庫ID
    this.svcinId = objCarWash.SVCIN_ID;

    //作業内容ID
    this.jobDtlId = objCarWash.JOB_DTL_ID;

    //ストール利用ID
    this.stallUseId = objCarWash.STALL_USE_ID;

    //RO番号
    this.roNum = objCarWash.RO_NUM;

    //顧客ID
    this.cstId = objCarWash.CST_ID;

    //車両ID
    this.vclId = objCarWash.VCL_ID;

    //モデル名
    this.modelName = objCarWash.MODEL_NAME;

    //Reg No.
    this.regNum = objCarWash.REG_NUM;

    //納車予定時刻
    this.scheDeliDatetime = objCarWash.SCHE_DELI_DATETIME;

    //遅れ見込み日時
    this.planDelayDate = objCarWash.PLAN_DELAYDATE;

    //引取納車区分
    this.pickDeliType = objCarWash.PICK_DELI_TYPE;

    //受付区分
    this.acceptanceType = objCarWash.ACCEPTANCE_TYPE;

    //行ロックバージョン
    this.rowLockVesion = objCarWash.ROW_LOCK_VERSION;

    //サービスステータス
    this.svcStatus = objCarWash.SVC_STATUS;

}

//DOMロード直後の処理(重要事項).
//@return {void}
$(function () {

    // グルグルを表示する
    gMainAreaActiveIndicator.show();

    gRefreshFlg = true;

    //hiddenコントロールの日付フォマートを取得する
    if ((document.getElementById("hidDateFormatMMdd").value != "") && (document.getElementById("hidDateFormatHHmm").value != "")) {

        gDateFormat = document.getElementById("hidDateFormatMMdd").value + " " + document.getElementById("hidDateFormatHHmm").value;

    }

    // N秒(画面自動リフレッシュ時間単位)
    gRefreshTime = Number($("#RefreshTimeHidden").val());

    //クライアントとサーバとの時間の差を設定
    SetServerTimeDifference();

    //タイトル文言設定する
    SetDisplayTitle(htmlDecode($("#HeadTitleHidden").val()));

    if (htmlDecode($("#CarWashHiddenInfo").val()) != "") {

        // 洗車情報を取得して、グローバル変数に保存
        GetCarWashInfo();

        // 最後のboderLineを削除;
        $(".boderLine:last-child").remove();

        // 虫眼鏡アイコンタップイベントをバインド
        BindSearchEvent();

        //画面スクロールイベントを生成
        CreatFingerScroll();

        // 更新時間を設定する
        $("#MessageUpdateTime").text(getUpdateTime());

        // バナータップイベント
        BindBanaTapEvent();

        // 定期リフレッシュ開始
        StartRefreshMainWndTimer();

        if ($("#hidErrorMeg").val() != "") {

            alert($("#hidErrorMeg").val());

            $("#hidErrorMeg").val("");
        }

        //0.05秒後グルグルを非表示
        setTimeout(function () {

            $("#ClickCarCount").css("display", "none");
            gMainAreaActiveIndicator.hide();
            gRefreshFlg = false;

        }, 50);

    } else {

        //情報取得
        MainLoadingButton();

    }

});

/**
* 洗車情報を取得
* @param {Object} objCarWash 洗車オブジェクト
* @return {なし}
*/
function GetCarWashInfo() {

    //JSON形式のチップ情報読み込み
    var jsonData = htmlDecode($("#CarWashHiddenInfo").val());

    //HTMLにバナー情報をクリア
    $("#CarWashHiddenInfo").attr("value", "");

    //バナー情報のJsonString→バナーデータインスタンスに設定
    FormatCarWashInfo(jsonData);
}

/**
* バナー情報のJsonString→バナーデータインスタンスに設定
* @param {String} バナー情報のJsonString
* @return {なし} 
*/
function FormatCarWashInfo(jsonData) {

    var carWashDataList = $.parseJSON(jsonData);

    gCount = 0;

    gArrObjCarWashInfo = new Array();
        
    //取得したチップ情報をチップクラスに格納し、再描画
    for (var strKey in carWashDataList) {

        gCount++;

        var objCarwash = new SetCarWashInfoObj(carWashDataList[strKey]);

        //バナーデータインスタンスにデータを設定
        gArrObjCarWashInfo[carWashDataList[strKey].SVCIN_ID] = objCarwash;
        
    }

}

/**
* 虫眼鏡アイコンタップイベントをバインド
* @param {なし} 
* @return {なし}
*/
function BindSearchEvent() {

    // 虫眼鏡アイコンタップ
    $(".SearchBox").bind("click", function (e) {

        if (gEditFlg || gEyeSearchFlg == true) {
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
* ディフォルト日付をチェック
* @param {Date} dtDate チェック日付
* @return {Bool} true：ディフォルト日付
*/
function IsDefaultDate(dtDate) {

    var inDate = new Date(dtDate);

    var dtDefault = new Date(C_DATE_DEFAULT_VALUE);

    if ((inDate - dtDefault) == 0) {

        return true;

    } else {

        return false;

    }
}

/**
* 画面スクロール
* @param {なし}
* @return {なし} 
*/
function CreatFingerScroll() {

    // スクロールが既存ですか

    $(".InnerBox").SC3340101fingerScroll();
    
    // FingerScrollの高さを設定
    SetFingerScrollHeight();

}

/**
* FingerScrollの高さを設定
* @param {なし}
* @return {なし} 
*/
function SetFingerScrollHeight() {

    if ($("#divCarCount").css("display") == "none") {
        //次のn件ボタンが非表示の場合

        if ($(".InnerScrollDiv .WCBoxType01").length < 6) {

            $(".InnerBox .scroll-inner").height($(".InnerBox").height() +16);

        } else {

            $(".InnerBox .scroll-inner").height($(".InnerScrollDiv").height() + 25);

        }


    } else {
        //次のn件ボタンが表示の場合

        if ($(".InnerScrollDiv .WCBoxType01").length < 5) {

            if ($(".InnerBox").height() > $("#UpdatePanel1").height()) {

                $(".InnerBox .scroll-inner").height($(".InnerBox").height() + 16);

            } else {

                $(".InnerBox .scroll-inner").height($(".InnerScrollDiv").height() + 111);

            }

        } else {

            $(".InnerBox .scroll-inner").height($(".InnerScrollDiv").height() + 111);

        }

    }

    $(".insiderSelect").height($(".InnerBox .scroll-inner").height());

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
*絞り込みモード切り替え
* @param  @targetControl {Object} コントロール
* @param  @keyboardType {int} キーボードタイプ
*/
function SwitchSearchMode(targetControl, keyboardType) {

    if (gEditFlg || gKeyBoardFlg) {

        return;

    } else {

        //キーボード表示
        ShowOriginalKeyBoard(targetControl, keyboardType, ScreenDisplay,ScreenDisplayCancel);
        
        $("#footer").css("display", "none");

        //スクロールをとめる
        $(".InnerBox").SC3340101fingerScroll({

            action: "stop"

        });

        // 定期リフレッショをとめる
        clearInterval(gInterval);
        gInterval = "";

        gSearchFlg = true;
    }
}

/**
* チップタップ時のイベントを登録
* @param {String} strChipId チップID
* @return {なし}
*/
function BindBanaTapEvent() {

    $(".WCBoxType01 ").unbind();

    //チップタップ時のイベントを登録
    $(".WCBoxType01 ").bind("chipTap", function (e) {

        // 0.5秒再タップを無効にする
        if (gTouchTimeoutFlg) {

            return;

        }

        gTouchTimeoutFlg = true;

        setTimeout(function () { gTouchTimeoutFlg = false; }, 500);

        // リフレッシュフラグ
        if (gRefreshFlg == true) {

            return false;

        }

        var strClassName = e.target.className;

        var strSvcinId = strClassName.substring(12, strClassName.length);

        // バナーをタップ
        TapBana(strSvcinId);

    });

}

/**
* バナーをタップ
* @param {String} サービス入庫ID
* @return {-} -
*/
function TapBana(inSvcinId) {

    var nShowBanaCount = GetShowBanaCount();

    if ($("#divCarCount").css("display") == "none") {
        //次のN件ボタン非表示の場合


        if (nShowBanaCount < 6) {
            //画面上バナーが5つ以下の場合

            if (($(".scroll-inner").position().top > 15)
                || $("#OriginalKeyBoard").css("visibility") != "hidden"
                || $(".scroll-inner").position().top < - ($(".scroll-inner").height() - $(".InnerBox").height())+25) {
                //キーボードが表示中、またはタップ時点でスクロール中（$(".scroll-inner").position().topで判断）場合

                //タップ不能
                return false;

            }

        } else {
            //画面上バナーが6つ以上の場合

            if (($(".scroll-inner").position().top > 15)
                || $("#OriginalKeyBoard").css("visibility") != "hidden"
                || $(".scroll-inner").position().top < -($("#UpdatePanel1").height() - $(".InnerBox").height())) {
                //キーボードが表示中、またはタップ時点でスクロール中（$(".scroll-inner").position().topで判断）場合

                //タップ不能
                return false;

            }

        }

    } else {
        //次のN件ボタンが表示の場合

        if (nShowBanaCount < 5) {
            //画面上バナーが4つ以下の場合

            if (($(".scroll-inner").position().top > 15)
                || $("#OriginalKeyBoard").css("visibility") != "hidden"
                || $(".scroll-inner").position().top < ($(".scroll-inner").height() - $(".InnerBox").height()) - 70) {
                //キーボードが表示中、またはタップ時点でスクロール中（$(".scroll-inner").position().topで判断）場合

                //タップ不能
                return false;

            }

        } else {
            //画面上バナーが5つ以上の場合

            if (($(".scroll-inner").position().top > 15)
                || $("#OriginalKeyBoard").css("visibility") != "hidden"
                || $(".scroll-inner").position().top < -($("#UpdatePanel1").height() - $(".InnerBox").height())) {
                //キーボードが表示中、またはタップ時点でスクロール中（$(".scroll-inner").position().topで判断）場合

                //タップ不能
                return false;

            }

        }
    
    }

    //スクロールしている場合、タップ不能にする
    if (gIsScrolling) {

        return false;

    }

    if (gBanaTapFlg == false) {
        // バナーが選択されていない場合

        //スクロール停止
        $(".InnerBox").SC3340101fingerScroll({ action: "stop" });

        // バナーが選択されてない場合
        if ($(".scroll-inner").height() < $(".InnerBox").height()) {

            $(".insiderSelect").height($(".InnerBox").height());

        } else {

            $(".insiderSelect").height($(".scroll-inner").height());

        }

        // バック色(黒い)を表示
        DisplayBackColor();

        $("." + inSvcinId).css({"z-index" : "901"});

        // タップされたサービス入庫IDを設定
        gTapSvcinId = inSvcinId;

        // タップされたフラグ
        gBanaTapFlg = true;

        // 定期リフレッショをとめる
        clearInterval(gInterval);
        gInterval = "";


        // 選択されたバナーにより、ボタンの活性、非活性を設定する
        ChangeBtnStatus(gArrObjCarWashInfo[inSvcinId].svcStatus);


    } else {

        // バナーが選択されてる場合

        // バック色(黒い)を非表示
        UndisplayBackColor();

        $("." + inSvcinId).css({ "z-index": "1" });

        // タップされたサービス入庫IDをクリア
        gTapSvcinId = "";

        gBanaTapFlg = false;

        // 四つボタン全部非活性にする
        ChangeBtnStatus("");

        //定期リフレッシュ再開
        StartRefreshMainWndTimer();

        $(".InnerBox").SC3340101fingerScroll({ action: "restart" });
    }
}

/**
* 四つボタンの活性/非活性を設定
* @param {inSvcStatus} サービスステータス
* @return {-} -
*/
function ChangeBtnStatus(inSvcStatus) {

    if (inSvcStatus == C_SVCSTATUS_CARWASHWAIT) {
        //洗車待ち

        //洗車開始、洗車スキップが活性
        $("#btnStart").css("visibility", "hidden");
        $("#btnStartOn").css("visibility", "visible");

        $("#btnSkip").css("visibility", "hidden");
        $("#btnSkipOn").css("visibility", "visible");

        //洗車終了、洗車Undoが非活性 
        $("#btnFinish").css("visibility", "visible");
        $("#btnFinishOn").css("visibility", "hidden");
        $("#btnUndo").css("visibility", "visible");
        $("#btnUndoOn").css("visibility", "hidden");

    } else if (inSvcStatus == C_SVCSTATUS_CARWASHSTART) {
        //洗車中

        //洗車開始、洗車スキップが非活性
        $("#btnStart").css("visibility", "visible");
        $("#btnStartOn").css("visibility", "hidden");
        $("#btnSkip").css("visibility", "visible");
        $("#btnSkipOn").css("visibility", "hidden");

        //洗車終了、洗車Undoが活性
        $("#btnFinish").css("visibility", "hidden");
        $("#btnFinishOn").css("visibility", "visible");
        $("#btnUndo").css("visibility", "hidden");
        $("#btnUndoOn").css("visibility", "visible"); 

    } else {

        //四つボタン全部非活性
        $("#btnStart").css("visibility", "visible");
        $("#btnStartOn").css("visibility", "hidden");
        $("#btnSkip").css("visibility", "visible");
        $("#btnSkipOn").css("visibility", "hidden");
        $("#btnFinish").css("visibility", "visible");
        $("#btnFinishOn").css("visibility", "hidden");
        $("#btnUndo").css("visibility", "visible");
        $("#btnUndoOn").css("visibility", "hidden");

    }

}

/**
* 定期リフレッシュ
* @param {refreshFunc} 再表示用のJavaScrep関数 -
* @return {-} -
*/
function StartRefreshMainWndTimer() {

    if (gFilterFlg == false &&
        gBanaTapFlg == false &&
        $("#OriginalKeyBoard").css("visibility") == "hidden") {
        //絞り込まない
        //バナー選択してない
        //キーボード表示してない

        // 定期リフレッショをとめる
        clearInterval(gInterval);
        gInterval = "";

        //定期リフレッシュ
        gInterval = setInterval(function () {

            // リフレッシュ
            MainRefresh();

        }, gRefreshTime * 1000);

    }

}

/**
* PullDownでリフレッシュ関数
* @param {なし}
*/
function PullDownRefresh() {

    gEyeSearchFlg = true;
    gFilterFlg = false;
    gKeyBoardFlg = true;
    // PullDownでリフレッシュ時、画面更新(大きなグルグル表示しない)
    MainLoadingButton();
    
}

/**
* リフレッシュ関数
* @param {なし}
*/
function MainRefresh() {

    if (gFilterFlg == true ||
        gBanaTapFlg == true ||
        $("#OriginalKeyBoard").css("visibility") != "hidden") {
        //絞り込む中
        //バナー選択中
        //キーボード表示中

        // 戻る（リフレッシュしない）
        return;
    }

    //ぐるぐる表示
    gMainAreaActiveIndicator.show();

    //リフレッシュ
    MainLoadingButton();

}

/**
* バック色(黒い)を表示
* @param {なし}
*/
function DisplayBackColor() {

    // 黒い背景色を表示
    $(".insiderSelect").css("display", "");
    $(".SelectWindow").css("display", "");
    $(".SelectWindowLeft").css("display", "");
    $(".SelectWindowRight").css("display", "");
    $(".SelectWindowBottom").css("display", "");

}

/**
* バック色(黒い)を非表示
* @param {なし}
*/
function UndisplayBackColor() {

    // 黒い背景色を非表示
    $(".insiderSelect").css("display", "none");
    $(".SelectWindow").css("display", "none");
    $(".SelectWindowLeft").css("display", "none");
    $(".SelectWindowRight").css("display", "none");
    $(".SelectWindowBottom").css("display", "none");

}

/**
* 開始ボタンをクリックイベント
* @param {なし}
*/
function ClickBtnStart() {

    // 非活性の場合、戻る
    if ($("#btnStartOn").css("visibility") == "hidden") {
        
        return false;

    }

    $("#btnStartOn").addClass("btn-pressed");

    setTimeout(function () {

        // ボタンの青色を解除
        $("#btnStartOn").removeClass("btn-pressed");

        // バック色(黒い)を非表示
        UndisplayBackColor();

        // グルグルを表示する
        gMainAreaActiveIndicator.show();


    }, 300);

    var svcinId = gTapSvcinId;

    var strParam = '{'
    strParam += '"Method":"' + "ClickBtnStart" + '"';
    strParam += ',"SvcInId":"' + svcinId + '"';
    strParam += ',"JobDtlId":"' + gArrObjCarWashInfo[svcinId].jobDtlId + '"';
    strParam += ',"StallUseId":"' + gArrObjCarWashInfo[svcinId].stallUseId + '"';
    strParam += ',"RowLockVersion":"' + gArrObjCarWashInfo[svcinId].rowLockVesion + '"';
    strParam += '}';

    //画面遷移のためポストバック
    $('#hidPostBackParamClass').val(strParam);

    commonRefreshTimer(ReDisplay);

    //0.05秒後洗車を開始
    setTimeout(function () {

        btnCarWashStart.click();

    }, 50);
    
}

/**
* スキップボタンをクリックイベント
* @param {なし}
*/
function ClickBtnSkip() {

    // 非活性の場合、戻る
    if ($("#btnSkipOn").css("visibility") == "hidden") {

        return false;

    }

    $("#btnSkipOn").addClass("btn-pressed");

    setTimeout(function () {

        // ボタンの青色を解除
        $("#btnSkipOn").removeClass("btn-pressed");

        // バック色(黒い)を非表示
        UndisplayBackColor();

        // グルグルを表示する
        gMainAreaActiveIndicator.show();

    }, 300);

    var svcinId = gTapSvcinId;

    var strParam = '{'
    strParam += '"Method":"' + "ClickBtnSkip" + '"';
    strParam += ',"SvcInId":"' + gTapSvcinId.toString() + '"';
    strParam += ',"JobDtlId":"' + gArrObjCarWashInfo[svcinId].jobDtlId + '"';
    strParam += ',"StallUseId":"' + gArrObjCarWashInfo[svcinId].stallUseId + '"';
    strParam += ',"PickDeliType":"' + gArrObjCarWashInfo[svcinId].pickDeliType + '"';
    strParam += ',"RowLockVersion":"' + gArrObjCarWashInfo[svcinId].rowLockVesion + '"';
    strParam += ',"RoNum":"' + gArrObjCarWashInfo[svcinId].roNum + '"';
    strParam += '}';


    //画面遷移のためポストバック
    $('#hidPostBackParamClass').val(strParam);

    commonRefreshTimer(ReDisplay);

    //0.05秒後洗車をスキップ
    setTimeout(function () {

        btnCarWashSkip.click();

    }, 50);

    return false;

}

/**
* 終了ボタンをクリックイベント
* @param {なし}
*/
function ClickBtnFinish() {

    // 非活性の場合、戻る
    if ($("#btnFinishOn").css("visibility") == "hidden") {

        return false;

    }

    $("#btnFinishOn").addClass("btn-pressed");

    setTimeout(function () {

        // ボタンの青色を解除
        $("#btnFinishOn").removeClass("btn-pressed");

        // バック色(黒い)を非表示
        UndisplayBackColor();

        // グルグルを表示する
        gMainAreaActiveIndicator.show();

    }, 300);

    var svcinId = gTapSvcinId;

    var strParam = '{'
    strParam += '"Method":"' + "ClickBtnFinish" + '"';
    strParam += ',"SvcInId":"' + svcinId.toString() + '"';
    strParam += ',"JobDtlId":"' + gArrObjCarWashInfo[svcinId].jobDtlId + '"';
    strParam += ',"StallUseId":"' + gArrObjCarWashInfo[svcinId].stallUseId + '"';
    strParam += ',"PickDeliType":"' + gArrObjCarWashInfo[svcinId].pickDeliType + '"';
    strParam += ',"RowLockVersion":"' + gArrObjCarWashInfo[svcinId].rowLockVesion + '"';
    strParam += ',"RoNum":"' + gArrObjCarWashInfo[svcinId].roNum + '"';
    strParam += '}';

    //画面遷移のためポストバック
    $('#hidPostBackParamClass').val(strParam);

    commonRefreshTimer(ReDisplay);

    //0.05秒後洗車を終了
    setTimeout(function () {

        btnCarWashFinish.click();

    }, 50);

    return false;

}

/**
* Undoボタンをクリックイベント
* @param {なし}
*/
function ClickBtnUndo() {

    // 非活性の場合、戻る
    if ($("#btnUndoOn").css("visibility") == "hidden") {

        return false;

    }

    $("#btnUndoOn").addClass("btn-pressed");

    setTimeout(function () {

        // ボタンの青色を解除
        $("#btnUndoOn").removeClass("btn-pressed");

        // バック色(黒い)を非表示
        UndisplayBackColor();

        // グルグルを表示する
        gMainAreaActiveIndicator.show();


    }, 300);

    var svcinId = gTapSvcinId;

    var strParam = '{'
    strParam += '"Method":"' + "ClickBtnUndo" + '"';
    strParam += ',"SvcInId":"' + svcinId.toString() + '"';
    strParam += ',"JobDtlId":"' + gArrObjCarWashInfo[svcinId].jobDtlId + '"';
    strParam += ',"StallUseId":"' + gArrObjCarWashInfo[svcinId].stallUseId + '"';
    strParam += ',"RowLockVersion":"' + gArrObjCarWashInfo[svcinId].rowLockVesion + '"';
    strParam += '}';

    //画面遷移のためポストバック
    $('#hidPostBackParamClass').val(strParam);

    commonRefreshTimer(ReDisplay);

    //0.05秒後洗車を終了
    setTimeout(function () {

        btnCarWashUndo.click();

    }, 50);

    return false;

}

/**
* メインloadingの処理関数
* @param {String} result コールバック呼び出し結果
* @param {String} context
*/
function MainLoadingButton() {

    setTimeout(function () {

        commonRefreshTimer(ReDisplay);
        btnMainLoading.click();

    }, 50);

}

/**
* 画面を再表示する(commonRefreshTimerにセットする関数)
* @return {-} 
*/
function ReDisplay() {

    window.location.reload();

}

/**
* 次のN件洗車情報を取得
* @param {Object} objCarWash 洗車オブジェクト
* @return {なし}
*/
function GetNextCarWash() {

    if ($("#OriginalKeyBoard").css("visibility") != "hidden") {

        return false;
    
    }
    gRefreshFlg = true;

    $("#ClickCarCount").removeAttr("style");
    $("#divCarCount").remove();

    commonRefreshTimer(ReDisplay);

    // 次のN件ボタンをクリック
    btnAddLoading.click();
      
}

/**
*絞り込みキャンセル
*/
function ScreenDisplayCancel() {

    if (gSearchFlg) {

        gSearchFlg = false;
        
    }

    //定期リフレッシュ再開
    StartRefreshMainWndTimer();

    $(".InnerBox").SC3340101fingerScroll({
        action: "restart"
    });

    $("#footer").removeAttr("style");
}

/**
* 虫眼鏡(キーボードとsearchボックス)を非表示
* @param  @keyword {String} 検索文字列
*/
function CancelEyeSearch() {

    $("#search")[0].value = "";

    $(".WCBoxType01").removeAttr("style");
    $(".boderLine").removeAttr("style");
    $("#footer").removeAttr("style");

    if ($('#hidPostBackParamClass')[0].value != "") {

        if ($(".InnerScrollDiv .WCBoxType01").length < parseInt($('#hidPostBackParamClass')[0].value)) {

            $("#divCarCount").removeAttr("style");

        }

    }

    $(".SearchArea").css({
        "-webkit-transition": "400ms linear",
        "width": "0px"
    });
    $("#search").css({
        "-webkit-transition": "400ms linear",
        "width": "0px"
    });

    gSearchFlg = false;
    //定期リフレッシュ再開
    StartRefreshMainWndTimer();
}

/**
*絞り込み表示
* @param  @keyword {String} 検索文字列
*/
function ScreenDisplay() {

    // グルグルを表示する
    gMainAreaActiveIndicator.show();

    var keyword = $("#search")[0].value;

    $("#divCarCount").css("display", "none");

    if (keyword == "") {

        gFilterFlg = false;

        // 虫眼鏡(キーボードとsearchボックス)を非表示
        CancelEyeSearch();

        //FingerScrollの高さを設定
        SetFingerScrollHeight();

        // 一行目移動
        $(".InnerBox").SC3340101fingerScroll({
            action: "move",
            moveY: $(".scroll-inner").position().top - 15,
            moveX: 0
        });

        // スクロール再開
        $(".InnerBox ").SC3340101fingerScroll({
            action: "restart"
        });

    } else {

        var displayList = $(".WCBoxType01");

        var divCount = 0;

        gFilterFlg = true;

        //取得したチップ情報をチップクラスに格納し、再描画
        for (var i = 0; i < displayList.length; i++) {

            var strClassName = displayList[i].className;

            var strSvcinId = strClassName.substring(12, strClassName.length);

            if (!IsMatchStringAfter(gArrObjCarWashInfo[strSvcinId].regNum, keyword)) {

                $("." + gArrObjCarWashInfo[strSvcinId].svcinId).css("display", "none");
                $("." + gArrObjCarWashInfo[strSvcinId].svcinId + "Line").css("display", "none");

            }else{
                
                divCount++;
                $("." + gArrObjCarWashInfo[strSvcinId].svcinId).removeAttr("style");
                $("." + gArrObjCarWashInfo[strSvcinId].svcinId + "Line").removeAttr("style");
            
            }
        }

        if ((divCount * 126) < $(".InnerBox").height()) {

            // スクロール高さリセット
            $(".InnerBox .scroll-inner").height($(".InnerBox").height() + 17);

            $(".InnerBox").SC3340101fingerScroll({
                action: "move",
                moveY: 0,
                moveX: 0
            });

            //スクロール再開
            $(".InnerBox").SC3340101fingerScroll({
                action: "restart"
            });

        }else{

            // スクロール高さリセット
            $(".InnerBox .scroll-inner").height($(".InnerScrollDiv").height() + 25);

            // 一行目移動
            $(".InnerBox").SC3340101fingerScroll({
                action: "move",
                moveY: $(".scroll-inner").position().top - 15,
                moveX: 0
            });

            //スクロール再開
            $(".InnerBox").SC3340101fingerScroll({
                action: "restart"
            });

        }

        // ボタン表示
        $("#footer").removeAttr("style");

        if (gSearchFlg) {

            gSearchFlg = false;
            //定期リフレッシュ再開
            StartRefreshMainWndTimer();

        }
    }

    //0.05秒後グルグルを非表示
    setTimeout(function () {

        gMainAreaActiveIndicator.hide();

    }, 50);

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
* UpdatePanel更新後走る関数
* @param  @targetStr {String} ターゲット文字列
* @param  @searchStr {String} 検索文字列
*/
function AfterUpdatePanel(){

    gEyeSearchFlg = false;

    commonClearTimer();

    // 洗車情報を取得して、グローバル変数に保存
    GetCarWashInfo();

    // 最後のboderLineを削除
    $(".boderLine:last-child").remove();

    // 虫眼鏡アイコンタップイベントをバインド
    BindSearchEvent();

    // FingerScrollを生成する(既存の場合、高さをリセット)
    CreatFingerScroll();

    // 更新時間を設定する
    $("#MessageUpdateTime").text(getUpdateTime());

    // 定期リフレッシュ開始
    StartRefreshMainWndTimer();

    if ($("#hidErrorMeg").val() != "") {

        alert($("#hidErrorMeg").val());

        $("#hidErrorMeg").val("");

        //画面再表示
        // グルグルを表示する
        gMainAreaActiveIndicator.show();
        MainLoadingButton();
    }

    $(".InnerBox").unbind("end.fingerscroll");

    //スクロール終了イベントを監視
    $(".InnerBox").bind("end.fingerscroll", function (e, position) {
        
        //工程管理画面と同じサイズ180以上でドラッグしすると、
        if ((position.top >= 180) && (!gSearchFlg)) {

            //更新中にする
            $(".pullDownToRefresh").removeClass("step0").addClass("step2");

            //スクロール停止
            $(".InnerBox").SC3340101fingerScroll({ action: "stop" });

            //更新処理
            PullDownRefresh();
        }
    });

    // バナータップイベント
    BindBanaTapEvent();

    //画面更新後の処理
    EndRefresh();

    $("#search")[0].value = "";

    $(".SearchArea").css({
        "-webkit-transition": "400ms linear",
        "width": "0px"
    });
    $("#search").css({
        "-webkit-transition": "400ms linear",
        "width": "0px"
    });

    $(".insiderSelect").height($(".InnerBox .scroll-inner").height());
    $("#search")[0].defaultValue = "";

    //0.05秒後グルグルを非表示
    setTimeout(function () {

        gMainAreaActiveIndicator.hide();
        gRefreshFlg = false;
        gKeyBoardFlg = false;

    }, 50);
}

/**
* UpdatePanel更新後走る関数
* @param  @targetStr {String} ターゲット文字列
* @param  @searchStr {String} 検索文字列
*/
function AfterNextUpdatePanel() {

    gEyeSearchFlg = false;

    commonClearTimer();

    // 洗車情報を取得して、グローバル変数に保存
    GetCarWashInfo();

    // 最後のboderLineを削除
    $(".boderLine:last-child").remove();

    // 虫眼鏡アイコンタップイベントをバインド
    BindSearchEvent();

    // FingerScrollの高さを設定
    SetFingerScrollHeight();

    // 更新時間を設定する
    $("#MessageUpdateTime").text(getUpdateTime());

    // 定期リフレッシュ開始
    StartRefreshMainWndTimer();

    if ($("#hidErrorMeg").val() != "") {

        alert($("#hidErrorMeg").val());

        $("#hidErrorMeg").val("");

        //画面再表示
        // グルグルを表示する
        gMainAreaActiveIndicator.show();
        MainLoadingButton();
    }

    $(".InnerBox").unbind("end.fingerscroll");

    //スクロール終了イベントを監視
    $(".InnerBox").bind("end.fingerscroll", function (e, position) {

        //工程管理画面と同じサイズ180以上でドラッグしすると、
        if ((position.top >= 180) && (!gSearchFlg)) {

            //更新中にする
            $(".pullDownToRefresh").removeClass("step0").addClass("step2");

            //スクロール停止
            $(".InnerBox").SC3340101fingerScroll({ action: "stop" });

            //更新処理
            PullDownRefresh();
        }
    });

    // バナータップイベント
    BindBanaTapEvent();

    //画面更新後の処理
    EndRefresh();

    $("#search")[0].value = "";

    $(".SearchArea").css({
        "-webkit-transition": "400ms linear",
        "width": "0px"
    });
    $("#search").css({
        "-webkit-transition": "400ms linear",
        "width": "0px"
    });

    $(".insiderSelect").height($(".InnerBox .scroll-inner").height());
    $("#search")[0].defaultValue = "";

    //0.05秒後グルグルを非表示
    setTimeout(function () {

        gMainAreaActiveIndicator.hide();
        gRefreshFlg = false;
        gKeyBoardFlg = false;
    }, 50);
}

/**
* 画面更新後の処理.
* @return {void}
*/
function EndRefresh() {

    $(".pullDownToRefresh").removeClass("step2").addClass("step0");
    $(".InnerBox").SC3340101fingerScroll({ action: "restart" });

}

/**
* 表示しているバナー個数を取得
* @return {void}
*/
function GetShowBanaCount() {

    //表示しているバナー個数を取得
    var count = 0;

    //非表示の件数を取得
    $(".InnerScrollDiv .WCBoxType01").each(function () {

        var strClassName = this.className;

        var strSvcinId = strClassName.substring(12, strClassName.length);

        if ($("." + strSvcinId).css("display") == "none") {

            count++;

        }

    });


    //表示件数を戻す（総件数-非表示件数）
    return $(".InnerScrollDiv .WCBoxType01").length - count;

}