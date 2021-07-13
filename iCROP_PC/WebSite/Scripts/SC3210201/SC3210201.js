/** 
* @fileOverview ショールームステータスビジュアライゼーション(メインエリア)の
* 処理を記述するファイル.
* 
* @author t.mizumoto
* @version 1.0.0
* 更新： 2012/08/28 TMEJ m.okamura 新車受付機能改善 $01
*/
// ==============================================================
// 定数
// ==============================================================
// 依頼送信日時経過時間の最大文字列長
var C_MAX_LENGTH_NOTICE_SPAN_TIME = 7;

// 商談経過時間の最大文字列長
var C_MAX_LENGTH_STAFF_SPAN_TIME = 8;

// ==============================================================
// 変数
// ==============================================================
// ロックフラグ
var gLockFlag = false;

// ハイライトフラグ
var gGrayOutFlg = false;

// ロック解除カウンター
var gLockCounter = 0;

// ロック解除秒数
var gLockResetInterval = 0;

// フレーム情報
var gFrameObject = null;

// ポップアップフラグ
var gPopupFlg = false;

// デバッグ情報
var gDebugObject = null;

// 来店時間警告秒数
var gVisitTimeAlertSpan = 0;

// 待ち時間警告秒数
var gWaitTimeAlertSpan = 0;

// 通知依頼警告秒数(査定)
var gAssessmentTimeAlertSpan = 0;

// 通知依頼警告秒数(価格相談)
var gPriceTimeAlertSpan = 0;

// 通知依頼警告秒数(ヘルプ)
var gHelpTimeAlertSpan = 0;

// 商談中詳細ポップオーバー 通知依頼送信日時リスト
var gStaffDetailRequestSendDateTimeList = [];

// 警告音出力中番号リスト
//var gAlertOutputNoList = [];

// 操作権限による警告音出力フラグ
var gMainAlertFlg = false;

// プッシュ送信による警告音出力フラグ
var gPushAlertFlg = false;

// 警告チップの数
var gAlertChipCount = 0;

// ==============================================================
// DOMロード時処理
// ==============================================================
$(function () {

    // ローディングウィンドウを開く
    showLodingWindow();

    /**
    * フレームに関する情報をセットする.
    * 
    * @class フレームのクラス.<br>
    * フレームの情報を所持し、それらを取り扱う機能を保有する.
    */
    var Frame = function () {

        /**
        * 表示中フレームのjQueryオブジェクト.
        * @return {Object} jQueryオブジェクト
        */
        this._jQuery1;

        /**
        * 非表示中フレームのjQueryオブジェクト.
        * @return {Object} jQueryオブジェクト
        */
        this._jQuery2;

        /**
        * フレーム番号.
        * @return {Integer} フレーム番号
        */
        this._dispNo = 1;

        /**
        * リロードフラグ
        * @return {Boolean}
        */
        this.reloadFlg = false;

        /**
        * ローディングフラグ
        * @return {Boolean}
        */
        this.loadingFlg = false;

        // 初期化処理
        this.init();
    }

    Frame.prototype = {

        /**
        * 現在表示中のDOMオブジェクトを取得する.
        * @return {Object} DOMオブジェクト
        */
        getActiveDom: function () {

            if (this._dispNo == 1) {

                return frame1_1;
            }
            else {

                return frame1_2;
            }
        },

        /**
        * 現在非表示中のDOMオブジェクトを取得する.
        * @return {Object} DOMオブジェクト
        */
        getNoActiveDom: function () {

            if (this._dispNo == 1) {

                return frame1_2;
            }
            else {

                return frame1_1;
            }
        },

        /**
        * 現在表示中のjQueryオブジェクトを取得する.
        * @return {Object} jQueryオブジェクト
        */
        getActiveJQuery: function () {

            if (this._dispNo == 1) {

                return this._jQuery1;
            }
            else {

                return this._jQuery2;
            }
        },

        /**
        * 現在非表示中のjQueryオブジェクトを取得する.
        * @return {Object} jQueryオブジェクト
        */
        getNoActiveJQuery: function () {

            if (this._dispNo == 1) {

                return this._jQuery2;
            }
            else {

                return this._jQuery1;
            }
        },

        /**
        * 初期化する.
        */
        init: function () {

            var frame = $("<iframe seamless src='SC3210202.aspx' scrolling='no' frameborder='0'></iframe>");
            // サイズ切り替えに対応する為、メインブロックから大きさを読み取る
            frame.attr('width', $('div#MainBlock').outerWidth() + 'px');
            frame.attr('height', $('div#MainBlock').outerHeight() + 'px');
            frame.attr('id', 'fr1_1');
            frame.attr('name', 'frame1_1');
            $('div#MainBlock').append(frame);

            this._jQuery1 = frame;

            frame.load(function () {

                // フレームを初期化する
                initFrame();

                // ローディングウィンドウを閉じる
                closeLodingWindow();
            });
        },

        /**
        * リロード要求処理を行う.
        */
        reloadRequest: function () {

            this.reloadArea(false);
        },

        /**
        * 強制リロード処理を行う.
        */
        reloadForce: function () {

            this.reloadArea(true);
        },

        /**
        * ロードする.
        */
        reloadArea: function (aReloadForceFlag) {

            // リロード要求フラグを降ろす
            this.reloadFlg = false;

            // リロードキャンセルフラグ
            var reloadCancelFlag = false;

            // 読み込み中の場合
            if (this.loadingFlg) {

                // 強制リロードの場合
                if (aReloadForceFlag) {

                    // 読み込み中のリロードをキャンセルさせる
                    reloadCancelFlag = true;
                }
                // リロード要求の場合
                else {

                    // リロード要求フラグを立てる
                    this.reloadFlg = true;
                    return;
                }
            }

            var frame = $("<iframe seamless src='SC3210202.aspx' scrolling='no' frameborder='0'></iframe>");
            // サイズ切り替えに対応する為、メインブロックから大きさを読み取る
            frame.attr('width', $('div#MainBlock').outerWidth() + 'px');
            frame.attr('height', $('div#MainBlock').outerHeight() + 'px');
            frame.css('display', 'none');

            // 初回時の考慮
            if (this._jQuery2 == undefined) {

                frame.attr('id', 'fr1_2');
                frame.attr('name', 'frame1_2');
            }
            else {

                if (this._dispNo == 1) {

                    frame.attr('id', 'fr1_2');
                    frame.attr('name', 'frame1_2');
                }
                else {

                    frame.attr('id', 'fr1_1');
                    frame.attr('name', 'frame1_1');
                }
            }

            $('div#MainBlock').append(frame);

            // デバッグ用
            if (gDebugObject.debugFlag) {

                var loadCount = $('span#LoadData').text() - 0 + 1;
                $('span#LoadData').text(loadCount)
                $('span#LoadFlag').show();
            }

            var obj = this;

            this.loadingFlg = true;

            frame.load(function () {

                obj.loadingFlg = false;

                // 【PUSH受信中に更新処理を行った場合の考慮】
                // 読み込み中のリロードをキャンセルさせる
                if (reloadCancelFlag) {

                    reloadCancelFlag = false;
                    // 読み込んだ要素を削除する
                    frame.remove();
                    return;
                }
                // 【リロード中にロックがかかった場合の考慮】
                // ロック中である場合は切り替え不可能であるため処理を終了する
                else if (gLockFlag) {

                    // 読み込んだ要素を削除する
                    frame.remove();
                    // リロード要求フラグを立てる
                    obj.reloadFlg = true;

                    // デバッグ用
                    if (gDebugObject.debugFlag) {

                        $('span#LoadFlag').hide();
                    }
                    return;
                }
                // 【読み込み中にPush受信を行った場合の考慮】
                // 読み込み中にリロード要求が入った場合
                else if (obj.reloadFlg) {
                    // 画面をロックする
                    gLockFlag = true;
                }

                // 初回時の考慮
                if (obj._jQuery2 == undefined) {

                    obj._jQuery2 = frame;
                }
                else {

                    if (obj._dispNo == 1) {

                        obj._jQuery2 = frame;
                    }
                    else {

                        obj._jQuery1 = frame;
                    }
                }

                // 表示フレームを切り替える
                changeDisp();

                // フレームを初期化する
                initFrame();

                // ローディングウィンドウを閉じる
                closeLodingWindow();
            });
        },

        /**
        * 表示切替を行う
        */
        changeDisp: function () {

            var activeFrameJQuery = this.getActiveJQuery();
            var noActiveFrameJQuery = this.getNoActiveJQuery();

            activeFrameJQuery.remove();
            noActiveFrameJQuery.css('display', 'block');

            if (this._dispNo == 1) {

                this._dispNo = 2;
            }
            else {

                this._dispNo = 1;
            }
        }
    };

    /**
    * デバッグ用の情報をセットする.
    * 
    * @class デバッグのクラス.<br>
    * デバッグ用の情報を所持する.
    * 
    * @param {Boolean} aDebugFlag デバッグフラグ
    */
    var Debug = function (aDebugFlag) {

        /**
        * デバッグフラグ.
        * @return {Boolean} debugFlag デバッグフラグ
        */
        this.debugFlag = aDebugFlag;
    };

    // 変数定義
    gFrameObject = new Frame();

    // デバッグ用
    if ($('div#DebugArea').length > 0) {

        gDebugObject = new Debug(true);
    }
    else {

        gDebugObject = new Debug(false);
    }

    // デバッグ用
    if (gDebugObject.debugFlag) {

        // ロック
        $('div#LockButton').bind('click', function () {

            lock();
        });

        // ロック解除
        $('div#ResetButton').bind('click', function () {

            lockReset();
        });

        // リロード要求
        $('div#ReloadRequestButton').bind('click', function () {

            reloadRequest();
        });

        // 強制リロード
        $('div#ReloadForceButton').bind('click', function () {

            reloadForce();
        });

        // Push受信
        $('div#PushReserveButton').bind('click', function () {
            SC3210201Update($('select#functionNo').val(), $('select#logicNo').val());
        });
    }

    // エリア定義定義を行う
    initArea();

    // ポップオーバー定義を行う
    initPopOver();

    // カウンター定義を行う
    initCounter();

    // アラート定義を行う
    initAlert();

    /**
    * エリア定義定義を行う.
    */
    function initArea() {

        // エリア選択時
        $('div#bodyFrame').bind(C_MOUSE_DOWN, function (aEvent) {

            // デバッグ用
            if (gDebugObject.debugFlag) {

                if ($(aEvent.target).parents('div#DebugArea').length > 0) {

                    return;
                }
            }

            // 警告音停止処理を行う
            //stopAlertOutput();

            // メインブロックの場合を除く
            if ($(aEvent.target).attr('id') == 'MainBlock') {

                return;
            }

            // 商談中詳細ポップアップの場合を除く
            if ($(aEvent.target).parents('div.scNscPopUpContactVisit').length > 0) {

                return;
            }

            var activeDom = gFrameObject.getActiveDom();
            activeDom.hidePopOver();
            activeDom.hideOverlay();
            lockReset();
        });

        // 読み込みエリア出力中に画面を操作するとスクリプトエラーが発生する件の対応
        $('div.registOverlay').bind(C_MOUSE_DOWN, function (e) {
            //ブラウザのデフォルト動作（ダブルタップ、ピンチ）を禁止
            e.preventDefault();
            //イベントパブリングを抑制
            e.stopPropagation();
        });
        $('div.registOverlay').bind(C_MOUSE_MOVE, function (e) {
            //ブラウザのデフォルト動作（ダブルタップ、ピンチ）を禁止
            e.preventDefault();
            //イベントパブリングを抑制
            e.stopPropagation();
        });
        $('div.registOverlay').bind(C_MOUSE_UP, function (e) {
            //ブラウザのデフォルト動作（ダブルタップ、ピンチ）を禁止
            e.preventDefault();
            //イベントパブリングを抑制
            e.stopPropagation();
        });
    }

    /**
    * ポップオーバー定義を行う.
    */
    function initPopOver() {

        // ポップオーバー画面でのクリックされた場合の定義
        $('.popoverEx').bind(C_MOUSE_DOWN, function () {

            // ロックカウンターリセット処理
            lockCounterReset();
        });
    }

    /**
    * カウンター定義を行う.
    */
    function initCounter() {

        if ($('input#LockResetInterval').val().length > 0) {

            gLockResetInterval = $('input#LockResetInterval').val();
        }

        if ($('input#VisitTimeAlertSpan').val().length > 0) {

            // 分を秒に変換して保持する
            gVisitTimeAlertSpan = ($('input#VisitTimeAlertSpan').val() - 0);
        }

        if ($('input#WaitTimeAlertSpan').val().length > 0) {

            // 分を秒に変換して保持する
            gWaitTimeAlertSpan = ($('input#WaitTimeAlertSpan').val() - 0);
        }

        // 通知依頼警告時間(査定)
        if ($('input#AssessmentAlertSpan').val().length > 0) {

            gAssessmentTimeAlertSpan = ($('input#AssessmentAlertSpan').val() - 0);
        }

        // 通知依頼警告時間(価格相談)
        if ($('input#PriceAlertSpan').val().length > 0) {

            gPriceTimeAlertSpan = ($('input#PriceAlertSpan').val() - 0);
        }

        // 通知依頼警告時間(ヘルプ)
        if ($('input#HelpAlertSpan').val().length > 0) {

            gHelpTimeAlertSpan = ($('input#HelpAlertSpan').val() - 0);
        }
        counter();

        setInterval(counter, 1000);
    }

    /**
    * アラート定義を行う.
 　  */
    function initAlert() {

        // 警告音出力フラグ判定
        if ($('input#AlarmOutputStatus').val() == "1") {

            gMainAlertFlg = true;
        }
    }
});


// ==============================================================
// イベント
// ==============================================================
$(window).load(function () {

    // -------------------------------
    // 非同期ポストバック用処理
    // -------------------------------
    var pageRequestManager = Sys.WebForms.PageRequestManager.getInstance();

    var targetElementId = '';

    // 非同期ポストバックの開始前に呼び出される
    pageRequestManager.add_initializeRequest(

        function (aSender, aArgs) {

            targetElementId = aArgs.get_postBackElement().id;

            // デバッグ用
            if (gDebugObject.debugFlag) {

                var partialRenderingCount = $('span#PartialRenderingCount').text() - 0 + 1;
                $('span#PartialRenderingCount').text(partialRenderingCount)
                $('span#PartialRenderingFlag').show();
            }
        }
    );

    // 非同期ポストバックの完了後に呼び出される
    pageRequestManager.add_endRequest(

        function (aSender, aArgs) {

            //ポップアップを表示していないときは処理を行わない(エラー時対策)
            if (!gPopupFlg) {

                return;
            }

            // デバッグ用
            if (gDebugObject.debugFlag) {

                $('span#PartialRenderingFlag').hide();
            }

            var activeDom = gFrameObject.getActiveDom();

            // 非同期通信に失敗した場合
            if (aArgs.get_error() != undefined) {

                showLodingWindowMainArea();
                onFailedClient(aArgs.get_error().message);
                aArgs.set_errorHandled(true);
            }

            var errorMessage = $('input#StaffDetailPopoverErrorMessage').val();

            // エラー時
            if (errorMessage !== '') {

                icropScript.ShowMessageBox(0, errorMessage, '');

                activeDom.hidePopOver();
                activeDom.hideOverlay();
                showLodingWindowMainArea();
                reloadForce();
                errorMessage = '';

                return;
            }

            activeDom.closeLodingWindow();

            // 初期処理
            if (targetElementId == 'StaffDetailDisplayButton') {
                // 商談中詳細画面の初期化
                InitStaffDetailPopOver();
            }
        }
    );
});

// ==============================================================
// 関数定義
// ==============================================================
/**
 * 非同期メソッド実行時のエラー処理を行う.
 */
function onFailedPageMethods(error) {

    onFailedClient(error.get_message());
}

/**
 * 非同期通信時のエラー処理を行う.
 */
function onFailedClient(errorMessage) {

    $('input#ErrorMessage').val(errorMessage);
    $('input#SendErrorMessageButton').click();
}

/**
 * 商談中詳細画面の初期化.
 */
function InitStaffDetailPopOver() {

    // ポップオーバー画面でのスクロールされた場合の定義
    $('div.scNscPopUpContactVisitScroll').scroll(function () {

        // ロックカウンターリセット処理
        lockCounterReset();

        // 警告音停止処理を行う
        //stopAlertOutput();
    });

    // 通知依頼送信日時を設定
    if ($('input#SendDateList').val().length > 0) {

        gStaffDetailRequestSendDateTimeList = $.evalJSON($('input#SendDateList').val());
    }

    // 依頼リスト 信号処理
    $("li#NoticeName").each(function (aIndex, aValue) {

        // 通信依頼種別事の信号を出力
        if ($(this).find('input#NoticeReqctg').val() == '01') {

            // 査定
            $(this).find('p#NoticeTime').data('NoticeLimitTime', gAssessmentTimeAlertSpan);

        } else if ($(this).find('input#NoticeReqctg').val() == '02') {

            // 価格相談
            $(this).find('p#NoticeTime').data('NoticeLimitTime', gPriceTimeAlertSpan);

        } else if ($(this).find('input#NoticeReqctg').val() == '03') {

            // ヘルプ
            $(this).find('p#NoticeTime').data('NoticeLimitTime', gHelpTimeAlertSpan);

        }
    });
    $("div.LeftBoxTime").val($('input#StaffDetailDialogSalesStartTime').val());
    // 商談中詳細画面のカウンター処理
    StaffDetailPopOverCounter(true, false);
}

/**
 * 商談中詳細画面のカウンター処理.
 */
function StaffDetailPopOverCounter(aDrawFlag, aCountUpFlag) {

    // 商談中詳細 依頼リスト 経過時間処理
    $.each(gStaffDetailRequestSendDateTimeList, function (aIndex, aValue) {

        if (0 < aValue.length) {

            // カウントアップを行ってから表示する(開いた瞬間はカウントアップしないため)
            if (aCountUpFlag) {

                gStaffDetailRequestSendDateTimeList[aIndex] = (aValue - 0) + 1 + '';
            }

            if (aDrawFlag) {

                // 警告時間の取得
                var limitTime = $('div.scNscPopUpContactVisitListArea').find('p#NoticeTime:eq(' + aIndex + ')').data('NoticeLimitTime') - 0;

                // 指定の時間を経過してしまった場合
                if (0 <= limitTime && limitTime < (gStaffDetailRequestSendDateTimeList[aIndex] - 0)) {

                    $('div.scNscPopUpContactVisitListArea').find('p#NoticeTime:eq(' + aIndex + ')').addClass('FontRed');
                }
                // 時間の描画
                $('div.scNscPopUpContactVisitListArea').find('p#NoticeTime:eq(' + aIndex + ')').text(
                getDispTime(gStaffDetailRequestSendDateTimeList[aIndex], C_MAX_LENGTH_NOTICE_SPAN_TIME));
            }
        }
    });

    // カウントアップを行ってから表示する(開いた瞬間はカウントアップしないため)
    if (aCountUpFlag) {

        var addDate = ($('div.LeftBoxTime').val() - 0) + 1 + '';
        $("div.LeftBoxTime").val(addDate);
    }
    else {

        // 商談経過時間がずれるため、サブ画面の配列より商談経過時間を取得する
        var activeDom = gFrameObject.getActiveDom();
        $("div.LeftBoxTime").val(activeDom.gSalesStartTimeSpanList[$("input#StaffDetailDialogIndex").val()]);
    }

    if (aDrawFlag) {

        // 商談時間
        $("div.LeftBoxTime").text(getDispTime($('div.LeftBoxTime').val(), C_MAX_LENGTH_STAFF_SPAN_TIME));
    }
}

/**
 * 商談中詳細画面の削除.
 */
function DeleteStaffDetailPopOver() {

    // 削除
    $('div.scNscPopUpContactVisit').find('div.scNscPopUpContactVisitSttl01').html('');
    $('div.scNscPopUpContactVisit').find('div.scNscPopUpContactVisitBox01').html('');
}

/**
 * カウンター処理を行う.
 */
function counter() {

    var loadFlag = false;

    if ($('div.registOverlay:visible').length > 0) {

        loadFlag = true;

        // ロックカウンターをリセットする
        lockCounterReset();
    }

    // ロック中かつロード中でなければカウントアップ
    if (gLockFlag && !loadFlag) {

        this.gLockCounter++;

        if (0 <= gLockCounter && gLockResetInterval < gLockCounter) {

            var activeDom = gFrameObject.getActiveDom();
            activeDom.hidePopOver();
            activeDom.hideOverlay();
            lockReset();
        }
    }

    // デバッグ用
    if (gDebugObject.debugFlag) {

        $('#pageData').text('LockFlag = ' + gLockFlag + ', LockCounter = ' + gLockCounter + ', gGrayOutFlg = ' + gGrayOutFlg + ', gPopupFlg = ' + gPopupFlg + ', gLoadFlg = ' + loadFlag);
        $('#frameData').text('dispNo = ' + gFrameObject._dispNo + ', reloadFlg = ' + gFrameObject.reloadFlg);
    }

    // 商談中詳細画面のカウンター処理
    if ($('div.scNscPopUpContactVisit:visible').length > 0) {

        StaffDetailPopOverCounter(true, true);
    }
}

/**
 * ロック処理を行う.
 */
function lock() {

    icropBase.Execute('icrop:log:SC3210201_DebagLog_Start CallFunc[lock] LockParm[gLockFlag=' + gLockFlag + ' ,gLockCounter=' + gLockCounter + ',gLockResetInterval=' + gLockResetInterval + ']');
    gLockFlag = true;
    gLockCounter = 0;
    icropBase.Execute('icrop:log:SC3210201_DebagLog_End CallFunc[lock] LockParm[gLockFlag=' + gLockFlag + ' ,gLockCounter=' + gLockCounter + ',gLockResetInterval=' + gLockResetInterval + ']');  
}

/**
 * ロックカウンターリセット処理.
 */
function lockCounterReset() {

    icropBase.Execute('icrop:log:SC3210201_DebagLog_Start CallFunc[lockCounterReset] LockParm[gLockFlag=' + gLockFlag + ' ,gLockCounter=' + gLockCounter + ',gLockResetInterval=' + gLockResetInterval + ']');
    gLockCounter = 0;
    icropBase.Execute('icrop:log:SC3210201_DebagLog_End CallFunc[lockCounterReset] LockParm[gLockFlag=' + gLockFlag + ' ,gLockCounter=' + gLockCounter + ',gLockResetInterval=' + gLockResetInterval + ']');
}

/**
 * ロック解除処理を行う.
 */
function lockReset() {

    icropBase.Execute('icrop:log:SC3210201_DebagLog_Start CallFunc[lockReset] LockParm[gLockFlag=' + gLockFlag + ' ,gLockCounter=' + gLockCounter + ',gLockResetInterval=' + gLockResetInterval + ']');
    // ロックカウンターをリセットする
    lockCounterReset();

    // グレーアウト状態である場合は解除しない(グレーアウト中のポップオーバーを閉じた場合の考慮)
    // グレーアウト状態でもロックを解除したい場合は必ずこのフラグを落とすようにすること
    if (gGrayOutFlg) {
        icropBase.Execute('icrop:log:SC3210201_DebagLog_End CallFunc[lockReset] LockParm[gLockFlag=' + gLockFlag + ' ,gLockCounter=' + gLockCounter + ',gLockResetInterval=' + gLockResetInterval + ']');
        return;
    }

    // リロードする必要がある場合のみリロードする
    if (gFrameObject.reloadFlg) {

        gFrameObject.reloadRequest();
    }
    gLockFlag = false;
    gLockCounter = 0;
    icropBase.Execute('icrop:log:SC3210201_DebagLog_End CallFunc[lockReset] LockParm[gLockFlag=' + gLockFlag + ' ,gLockCounter=' + gLockCounter + ',gLockResetInterval=' + gLockResetInterval + ']');
}

/**
 * リロード要求処理を行う.
 */
function reloadRequest() {

    // ロック中の場合
    if (gLockFlag) {

        // 必ずリロードしない
        gFrameObject.reloadFlg = true;
    }
    // ロックなしの場合
    else {

        // 必ずリロードする
        gFrameObject.reloadRequest();
        gLockCounter = 0;
    }
}

/**
 * 強制リロード処理を行う.
 */
function reloadForce() {

    // 必ずリロードする
    gFrameObject.reloadForce();
    gLockFlag = false;
    gLockCounter = 0;
}

/**
 * フレームを初期化する.
 */
function initFrame() {

    var activeFrameJQuery = gFrameObject.getActiveJQuery();
    var con = $('div#MainBlock');

    // 子エリア選択時
    activeFrameJQuery.contents().find('div#InnerMainBlock').bind(C_MOUSE_DOWN, function (aEvent) {

        var tagName = (aEvent.target.tagName).toUpperCase();

        if (tagName == "INPUT") {

            return;
        }
        // フレーム内のハブリングが行われないため手動でハブリング設定しておく
        con.trigger(C_MOUSE_DOWN);
    });

    // ロックカウンターリセット処理
    lockCounterReset();

    // 警告音出力処理を行う（初期表示処理）
    alertOutputInitialDisplay();
}

/**
 * フレーム切り替え処理を行う.
 */
function changeDisp() {

    // デバッグ用
    if (gDebugObject.debugFlag) {

        $('span#LoadFlag').hide();
    }
    gFrameObject.changeDisp();
}

/**
 * ポップアップオープン時処理.
 */
function popupOpen() {

    lock();
    gPopupFlg = true;
}

/**
 * ポップアップクローズ時処理を行う.
 */
function popupClose() {

    gPopupFlg = false;
}

/**
 * ポップアップを閉じる.
 */
function hidePopOver() {

    if ($.fn.popover.openedPopup) {

        $.fn.popover.openedPopup.trigger('hidePopover');
    }
}

/**
 * 読み込み中エリアを表示する.
 */
function showLodingWindow() {

    var overlay = $('div.registOverlay');

    //オーバーレイ表示
    overlay.css({ width: $(window).width() + 'px', height: $(window).height() + 'px' });
    overlay.css('display', 'table');
}

/**
 * 読み込み中エリアを表示する(メインエリア).
 */
function showLodingWindowMainArea() {

    var container = $('div#MainBlock');

    overlay = container.find('div.registOverlay');
    overlay.css({
        width: container.width() + 'px',
        height: container.height() + 'px',
        top: container.offset().top + 'px',
        left: container.offset().left + 'px'
    });
    overlay.css({ 'display': 'table' });
}

/**
 * 読み込み中エリアを非表示にする.
 */
function closeLodingWindow() {

    $('div.registOverlay').css('display', 'none');
}

/**
 * エリア更新を行う(PUSH機能にて実行される前提).
 */
function SC3210201Update(aFunctionNo, aLogicNo) {

    icropBase.Execute('icrop:log:SC3210201_DebagLog_Start CallFunc[SC3210201Update] LockParm[gLockFlag=' + gLockFlag + ' ,gLockCounter=' + gLockCounter + ',gLockResetInterval=' + gLockResetInterval + ']');
    // 警告音設定
    // 操作権限による警告音出力フラグ(総数)が立っている場合のみ処理を行う
    if (gMainAlertFlg) {

        // ゲートキーパー 来店通知 来店件数の増加
        if (aFunctionNo == '01' && aLogicNo == '01') {

            gPushAlertFlg = true;
        }
        // 未対応来店客 「了」ボタンタップ(お客様が待ち状態) 来店件数の増加
        else if (aFunctionNo == '02' && aLogicNo == '02') {

            gPushAlertFlg = true;
        }
        // 未対応来店客 「待」ボタンタップ 待ち件数の増加
        else if (aFunctionNo == '02' && aLogicNo == '03') {

            gPushAlertFlg = true;
        }
        // 未対応来店客 「不」ボタンタップ(お客様が待ち状態)来店件数の増加
        else if (aFunctionNo == '02' && aLogicNo == '05') {

            gPushAlertFlg = true;
        }
        // $01 start 複数顧客に対する商談平行対応
        // 商談中断送信時
        else if (aFunctionNo == '03' && aLogicNo == '09') {
            gPushAlertFlg = true;
        }
        // $01 end   複数顧客に対する商談平行対応
        // 顧客情報画面 査定の送信時(できれば商談中スタッフ) 査定件数の増加
        else if (aFunctionNo == '03' && aLogicNo == '03') {

            gPushAlertFlg = true;
        }
        // 顧客情報画面 価格相談の送信時(できれば商談中スタッフ) 価格相談件数の増加
        else if (aFunctionNo == '03' && aLogicNo == '05') {

            gPushAlertFlg = true;
        }
        // 顧客情報画面 ヘルプの送信時(できれば商談中スタッフ) ヘルプ件数の増加
        else if (aFunctionNo == '03' && aLogicNo == '07') {

            gPushAlertFlg = true;
        }
    }
    // リロード要求
    reloadRequest();
    icropBase.Execute('icrop:log:SC3210201_DebagLog_End CallFunc[SC3210201Update] LockParm[gLockFlag=' + gLockFlag + ' ,gLockCounter=' + gLockCounter + ',gLockResetInterval=' + gLockResetInterval + ']');
}

/**
* 警告音出力処理を行う（初期表示処理）.
*/
function alertOutputInitialDisplay() {

    if (!gMainAlertFlg) {
        return;
    }

    // 現在の警告チップの数
    var activeFrameDom = gFrameObject.getActiveDom();
    var nowAlertChipCount = activeFrameDom.getNowAlertChipCount();

    // 保持しておいた警告チップの数
    var beforeAlertChipCount = gAlertChipCount;

    // 現在の警告チップの数を保持しておく
    gAlertChipCount = nowAlertChipCount;

    // プッシュ送信による警告音出力フラグのチェック
    if (gPushAlertFlg) {

        gPushAlertFlg = false;

        // 警告音出力処理を行う
        alertOutput('1');
    }

    // 警告チップの数が増加した場合
    if (beforeAlertChipCount < nowAlertChipCount) {

        // 警告音出力処理を行う
        alertOutput('2');
    }

}

/**
* 警告音出力処理を行う.
*/
function alertOutput(aAlertOutputNo) {

    // 警告音出力中リストに含まれていない場合
    //if ($.inArray(aAlertOutputNo, gAlertOutputNoList) < 0) {

    if (aAlertOutputNo == '1') {
        // 警告音出力
        icropBase.beep(2);
    }
    else {
        icropBase.beep(3);
    }

        // 警告音出力中リストに番号を追加
        //gAlertOutputNoList.push(aAlertOutputNo);
    //}
}

/**
* 警告音停止処理を行う.
*/
/*
function stopAlertOutput() {

    if (gAlertOutputNoList.length == 0) {
        return;
    }

    $.each(gAlertOutputNoList, function (aIndex, aValue) {

        // 警告音停止
        icropBase.Execute('icrop:soundoff:' + aValue);

    });

    gAlertOutputNoList = [];
}
*/
