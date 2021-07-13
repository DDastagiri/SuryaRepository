//---------------------------------------------------------
//SC3090401.js
//---------------------------------------------------------
//機能：メイン画面処理
//作成：2018/02/19 NSK 河谷 REQ-SVT-TMT-20170615-001 iPod Gatekeeperに顧客の車両登録番号を入力する機能を追加
//更新：2019/02/28 NSK 河谷 REQ-SVT-TMT-20180601-001 Gate Keeper機能の視認性操作性改善 $01
//---------------------------------------------------------

//DOMロード直後の処理(重要事項).
//@return {void}
$(function () {
    //hiddenコントロールの日付フォーマートを取得する
    if ((document.getElementById("hidDateFormatMMdd").value != "") && (document.getElementById("hidDateFormatHHmm").value != "")) {
        gDateFormat = document.getElementById("hidDateFormatMMdd").value + " " + document.getElementById("hidDateFormatHHmm").value;
    }

    // 戻るボタンの初期化
    $('#btn_back_o').css('display', 'none');
    $('#btn_back').css('display', 'block');

    // 来店済表示ボタンの初期化
    $('#btn_all_o').css('display', 'none');
    $('#btn_all').css('display', 'block');

    // ソート区分ボタンの初期化
    $('#btn_sort_no').css('display', 'none');
    $('#btn_sort_time').css('display', 'block');

    // 初回読み込み時にボタン名を設定
    gButtonName = C_MAIN_LOAD_BUTTON;

    //初期表示ボタンイベント
    PageLoad();
});

/**
* ページ読込時イベントの定義
* 
* @param {-} - -
* @return {-} -
* 
* @example 
*  -
*/
function PageLoad() {

    gIsReload = false;

    //アクティブインジケータ
    $.master.OpenLoadingScreen();

    //タイマークリア
    commonClearTimer();

    //タイマーセット
    commonRefreshTimer(RefreshDisplay);

    // 初回読み込み時の処理を実行
    $('#InitButton').click();

    // UpdatePanel処理前後イベント
    $(document).ready(function () {
        var prm = Sys.WebForms.PageRequestManager.getInstance();
        // 開始時のイベント
        prm.add_beginRequest(function () { });
        // 終了時のイベント
        prm.add_endRequest(EndRequest);
        function EndRequest(sender, args) {

            // 初回読み込み時の設定
            if (gButtonName == C_MAIN_LOAD_BUTTON) {

                //スクロールの初期化
                $("#VisitInfoContents").SC3090401fingerScroll();
                $(".scroll-inner").css({
                    "top": C_SC3090401SCR_DEFAULTTOP
                });

                // リストの最終更新日時を取得
                $("#MessageUpdateTime").text(getUpdateTime());

                // ボタンイベントの設定
                AddEvent();

                gButtonName = "";
            }

            // プルダウンリフレッシュ時、画面更新後の処理を実行
            if (gButtonName == C_PULLDOWN_REFRESH_BUTTON) {

                endRefresh();
            }

            // 最終更新日時を設定
            $("#MessageUpdateTime").text(getUpdateTime());

            // 来店済みのチップの背景色を灰色にする
            var appointmentList = $(".Bottom_TBL li");
            var count = 0;
            appointmentList.each(function () {

                count = count + 1;

                // UpdateDateに値が入っている場合、来店済み
                if (0 < $(this).find("#UpdateDate").val().length) {

                    // $01 start 2019/02/28 NSK 河谷 REQ-SVT-TMT-20180601-001 Gate Keeper機能の視認性操作性改善
                    // $(this).addClass("TC_BG_GRAY");
                    $(this).find(".WCBoxType01").addClass("TC_BG_GRAY");
                    // $01 end 2019/02/28 NSK 河谷 REQ-SVT-TMT-20180601-001 Gate Keeper機能の視認性操作性改善
                } else {

                    // $01 start 2019/02/28 NSK 河谷 REQ-SVT-TMT-20180601-001 Gate Keeper機能の視認性操作性改善
                    // $(this).addClass("TC_BG_WHITE");
                    $(this).find(".WCBoxType01").addClass("TC_BG_WHITE");
                    // $01 end 2019/02/28 NSK 河谷 REQ-SVT-TMT-20180601-001 Gate Keeper機能の視認性操作性改善
                }
            });

            // 来店登録、来店取消のイベントを設定
            if (0 < count) {

                AddVisitEvent();
            }

            // 前のN件ボタンのイベントを設定
            if ($('#BackPage').css('display') != 'none') {

                count = count + 1;

                AddBackPageEvent();
            }

            // 次のN件ボタンのイベントを設定
            if ($('#NextPage').css('display') != 'none') {

                count = count + 1;

                AddNextPageEvent();
            }

            //スクロールを設定
            var nheight = count * C_SC3090401TA_DEFAULTHEIGHT - 2;
            if (nheight < C_SC3090401SCR_DEFAULTHEIGHT) {
                nheight = C_SC3090401SCR_DEFAULTHEIGHT;
            }
            //スクロールのサイズを調整
            $(".scroll-inner").css({
                "height": nheight,
                "width": C_SC3090401SCR_DEFAULTWIDTH,
                "top": C_SC3090401SCR_DEFAULTTOP
            });

            $("#VisitInfoContents").SC3090401fingerScroll();

            //タイマークリア
            commonClearTimer();

            //アクティブインジケータ終了
            $.master.CloseLoadingScreen();

            gIsReload = false;
        }
    });
}

/**
* ボタンイベントの定義
* 
* @param {-} - -
* @return {-} -
* 
* @example 
*  -
*/
function AddEvent() {

    /**
    * 戻るボタン押下時の制御を行う。
    * 
    * @param {-} - -
    * @return {-} -
    * 
    * @example 
    *  -
    */
    $('#btn_back').click(function () {

        // リロード中は無視
        if (gIsReload == true) {

            return;
        }

        //ボタン背景点灯
        $('#btn_back').css('display', 'none');
        $('#btn_back_o').css('display', 'block');

        gIsReload = true;

        setTimeout(function () {

            //ボタン背景を戻す
            $('#btn_back_o').css('display', 'none');
            $('#btn_back').css('display', 'block');

            //アクティブインジケータ
            $.master.OpenLoadingScreen();

            //タイマーセット
            commonRefreshTimer(RefreshDisplay);

            $('#BackButton').click();
        }, 300);
    });
    /**
    * 来店済表示ボタン押下時の制御を行う。
    * 
    * @param {-} - -
    * @return {-} -
    * 
    * @example 
    *  -
    */
    $('#btn_all').click(function () {

        // リロード中は無視
        if (gIsReload == true) {

            return;
        }

        $('#btn_all').hide();
        $('#btn_all_o').show();

        // 来店済表示フラグを切り替える
        $('#AllDisplayFlag').val(C_ALL_DISPLAY_FLAG_ON);

        AllDisplayButtonCommon();
    });
    /**
    * 来店済非表示ボタン押下時の制御を行う。
    * 
    * @param {-} - -
    * @return {-} -
    * 
    * @example 
    *  -
    */
    $('#btn_all_o').click(function () {

        // リロード中は無視
        if (gIsReload == true) {

            return;
        }

        $('#btn_all_o').hide();
        $('#btn_all').show();

        // 来店済表示フラグを切り替える
        $('#AllDisplayFlag').val(C_ALL_DISPLAY_FLAG_OFF);

        AllDisplayButtonCommon();
    });
    /**
    * 来店済表示切替ボタン押下時の共通処理を行う。
    * 
    * @param {-} - -
    * @return {-} -
    * 
    * @example 
    *  -
    */
    function AllDisplayButtonCommon() {

        //アクティブインジケータ
        $.master.OpenLoadingScreen();

        gIsReload = true;

        //タイマーセット
        commonRefreshTimer(RefreshDisplay);

        $('#AllDisplayButton').click();
    }
    /**
    * ソート順切替ボタン(予約日時)押下時の制御を行う。
    * 
    * @param {-} - -
    * @return {-} -
    * 
    * @example 
    *  -
    */
    $('#btn_sort_time').click(function () {

        // リロード中は無視
        if (gIsReload == true) {

            return;
        }

        $('#btn_sort_time').hide();
        $('#btn_sort_no').show();

        // ソート区分を切り替える
        $('#SortType').val(C_SORT_TYPE_REG_NUM);

        SortButtonCommon();
    });
    /**
    * ソート順切替ボタン(車両登録番号)押下時の制御を行う。
    * 
    * @param {-} - -
    * @return {-} -
    * 
    * @example 
    *  -
    */
    $('#btn_sort_no').click(function () {

        // リロード中は無視
        if (gIsReload == true) {

            return;
        }

        $('#btn_sort_no').hide();
        $('#btn_sort_time').show();

        // 来店済表示フラグを切り替える
        $('#SortType').val(C_SORT_TYPE_REZ_DATE);

        SortButtonCommon();
    });
    /**
    * ソート順切替ボタン押下時の共通処理を行う。
    * 
    * @param {-} - -
    * @return {-} -
    * 
    * @example 
    *  -
    */
    function SortButtonCommon() {

        //アクティブインジケータ
        $.master.OpenLoadingScreen();

        gIsReload = true;

        //タイマーセット
        commonRefreshTimer(RefreshDisplay);

        $('#SortButton').click();
    }
}
/**
* 前のN件ボタンイベントの定義
* 
* @param {-} - -
* @return {-} -
* 
* @example 
*  -
*/
function AddBackPageEvent() {

    /**
    * 前のN件ボタン押下時の制御を行う。
    * 
    * @param {-} - -
    * @return {-} -
    * 
    * @example 
    *  -
    */
    $('#BackPage').click(function () {

        // リロード中は無視
        if (gIsReload == true) {

            return;
        }
        //読み込み中を表示
        $("#BackPage").css("display", "none");
        $("#BackPageLoad").css("display", "block");

        //アクティブインジケータ
        $.master.OpenLoadingScreen();

        gIsReload = true;

        //タイマーセット
        commonRefreshTimer(RefreshDisplay);

        $('#BackPageButton').click();
    });
}

/**
* 次のN件ボタンイベントの定義
* 
* @param {-} - -
* @return {-} -
* 
* @example 
*  -
*/
function AddNextPageEvent() {

    /**
    * 次のN件ボタン押下時の制御を行う。
    * 
    * @param {-} - -
    * @return {-} -
    * 
    * @example 
    *  -
    */
    $('#NextPage').click(function () {
        // リロード中は無視
        if (gIsReload == true) {
            return;
        }
        //読み込み中を表示
        $("#NextPage").css("display", "none");
        $("#NextPageLoad").css("display", "block");

        //アクティブインジケータ
        $.master.OpenLoadingScreen();

        gIsReload = true;

        //タイマーセット
        commonRefreshTimer(RefreshDisplay);

        $('#NextPageButton').click();
    });
}

/**
* 来店管理、来店取消イベントの定義
* 
* @param {-} - -
* @return {-} -
* 
* @example 
*  -
*/
function AddVisitEvent() {
    /**
    * 予約情報を押下した時のイベントを定義
    * 
    * @param {-} - -
    * @return {-} -
    * 
    * @example 
    *  -
    */
    $(".Bottom_TBL li").click(function () {

        // リロード中は無視
        if (gIsReload == true) {

            return;
        }

        var selectedRow = $(this);

        var selectedServiceinId = selectedRow.find("#ServiceinId").val();
        var selectedUpdateDate = selectedRow.find("#UpdateDate").val();

        // 更新日時に値が入っている場合、来店取消
        if (0 < selectedRow.find("#UpdateDate").val().length) {

            ExecuteCancelVisit(selectedServiceinId, selectedUpdateDate, selectedRow);
        }
        // 更新日時に値が入っていない場合、来店登録
        else {

            ExecuteRegistVisit(selectedServiceinId, selectedRow);
        }
    });
    /**
    * 来店登録ボタン押下イベントを定義
    * 
    * @param {aSvcinId} 選択した予約のサービス入庫ID
    * @param {aSelectedRow} 選択した行
    * @return {-} -
    * 
    * @example 
    *  -
    */
    function ExecuteRegistVisit(aSvcinId, aSelectedRow) {

        aSelectedRow.removeClass("TC_BG_WHITE");
        aSelectedRow.addClass("icrop-pressed");

        setTimeout(function () {

            aSelectedRow.removeClass("icrop-pressed");
            aSelectedRow.addClass("TC_BG_WHITE");

            setTimeout(function () {

                // 確認ダイアログを表示
                var result = window.confirm($("#RegistConfirmMessageText").val());

                // 確認ダイアログではいが選択された場合、
                // 手入力した車両登録番号をHiddenFieldに格納する
                if (result) {

                    $('#HiddenSelectServiceinId').val(aSvcinId);

                    //アクティブインジケータ
                    $.master.OpenLoadingScreen();

                    gIsReload = true;

                    //タイマーセット
                    commonRefreshTimer(RefreshDisplay);

                    $('#VisitEventButton').click();
                }
            }, 50);
        }, 300);
    }
    /**
    * 来店取消ボタン押下イベントを定義
    * 
    * @param {aSvcinId} 選択した予約のサービス入庫ID
    * @param {aUpdateDate} 選択した予約の更新日時
    * @param {aSelectedRow} 選択した行
    * @return {-} -
    * 
    * @example 
    *  -
    */
    function ExecuteCancelVisit(aSvcinId, aUpdateDate, aSelectedRow) {

        aSelectedRow.removeClass("TC_BG_GRAY");
        aSelectedRow.addClass("icrop-pressed");

        setTimeout(function () {

            aSelectedRow.removeClass("icrop-pressed");
            aSelectedRow.addClass("TC_BG_GRAY");

            setTimeout(function () {

                // 確認ダイアログを表示
                var result = window.confirm($("#CancelConfirmMessageText").val());

                // 確認ダイアログではいが選択された場合、
                // 手入力した車両登録番号をHiddenFieldに格納する
                if (result) {

                    $('#HiddenSelectServiceinId').val(aSvcinId);
                    $('#HiddenSelectUpdateDate').val(aUpdateDate);

                    //アクティブインジケータ
                    $.master.OpenLoadingScreen();

                    gIsReload = true;

                    //タイマーセット
                    commonRefreshTimer(RefreshDisplay);

                    $('#VisitCancelButton').click();
                }
            }, 50);
        }, 300);
    }
}

/**
* PullDownRefresh画面最新化
* @return {なし}
*/
function PullDownRefresh() {

    gIsReload = true;

    //ボタン名を設定
    gButtonName = C_PULLDOWN_REFRESH_BUTTON;

    //タイマーセット
    commonRefreshTimer(RefreshDisplay);

    $('#PullDownRefreshButton').click();
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
    var serverTime = new Date();
    //サーバの現在時刻を算出  
    serverTime.setTime(serverTime.getTime() + gServerTimeDifference);
    return serverTime;
}

/**
* サーバとの時間差を算出し、グローバル変数に格納する.
* @return {void}
* 
*/
function SetServerTimeDifference() {

    //ページ読込時のサーバ時間を取得
    var pageLoadServerTime = new Date($("#ServerTimeHidden").val());
    //クライアントの現在時刻を取得
    var pageLoadClientTime = new Date();
    //サーバとの時間差を算出し、格納（ミリ秒）
    gServerTimeDifference = pageLoadServerTime - pageLoadClientTime;
}

/**
* 日付フォマット
* @param {inDate} Date　指定日付
* @param {fmt} String　変換したいフォマット
*/
function DateFormat(inDate, fmt) {
    var reDate = fmt;
    var o = {
        "M+": inDate.getMonth() + 1, //月  
        "d+": inDate.getDate(), //日   
        "H+": inDate.getHours(), //時  
        "m+": inDate.getMinutes(), //分   
        "s+": inDate.getSeconds(), //秒 
        "q+": Math.floor((inDate.getMonth() + 3) / 3), // 季節
        "S": inDate.getMilliseconds()//ミリ秒   

    };
    if (/(y+)/.test(fmt)) reDate = reDate.replace(RegExp.$1, (inDate.getFullYear() + "").substr(4 - RegExp.$1.length));
    for (var k in o) if (new RegExp("(" + k + ")").test(fmt)) reDate = reDate.replace(RegExp.$1, (RegExp.$1.length == 1) ? (o[k]) : (("00" + o[k]).substr(("" + o[k]).length)));
    return reDate;
}

//再描画イベント
function RefreshDisplay() {

    //アクティブインジケータ
    $.master.OpenLoadingScreen();

    gIsReload = true;

    //タイマークリア
    commonClearTimer();

    //タイマーセット
    commonRefreshTimer(RefreshDisplay);

    // 初回読み込み時の処理を実行
    $('#InitButton').click();
}
			