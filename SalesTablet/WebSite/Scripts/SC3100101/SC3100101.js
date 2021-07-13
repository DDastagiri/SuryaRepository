/** 
* @fileOverview 受付メイン（メインエリア）の処理を記述するファイル.
* 
* @author t.mizumoto
* @version 1.0.0
* 更新： 2012/05/24 KN m.asano クルクル対応 $01
* 更新： 2012/08/27 TMEJ m.okamura 新車受付機能改善 $02
* 更新： 2013/01/16 TMEJ m.asano 新車タブレットショールーム管理機能開発 $03
* 更新： 2020/02/06 NSK s.natsume TKM Change request development for Next Gen e-CRB (CR075) $04
* 更新： 2020/03/12 NSK s.natsume TKM Change request development for Next Gen e-CRB (CR060) $05
*/
// ==============================================================
// 定数
// ==============================================================
// 更新権限
var C_UPDATE_OPERATION = 1; // 1:更新、2:読取専用

// 依頼の経過時間の最大文字列長
var C_MAX_LENGTH_NOTICE_SPAN_TIME = 7;

// 商談の経過時間の最大文字列長
var C_MAX_LENGTH_STAFF_SPAN_TIME = 8;

// 紐付け解除画面の来店経過時間の最大文字列長
var C_MAX_LENGTH_LINKING_CANCEL_SPAN_TIME = 7;

var C_FADE_IN_OVERLAY = 0;       // フェードインなし
var C_FADE_OUT_OVERLAY = 0;       // フェードインなし


// ==============================================================
// 変数
// ==============================================================
// ロックフラグ
var gLockFlag = false;

// ハイライトフラグ
var gGrayOutFlg = false;

// 入力中フラグ
var gInputFlg = false;

// ロック解除カウンター
var gLockCounter = 0;

// ロック解除秒数
var gLockResetInterval = 0;

// フレーム情報
var gFrameObject = null;

// ポップアップフラグ
var gPopupFlg = false;

// ドラッグフラグ
var gDragFlg = false;

// デバッグ情報
var gDebugObject = null;

// 来店時間警告秒数
var gVisitTimeAlertSpan = 0;

// 待ち時間警告秒数
var gWaitTimeAlertSpan = 0;

// $02 start 新車タブレットショールーム管理機能開発
// 接客不要警告秒数(第１段階)
var gUnNecessaryFirstTimeAlertSpan = 0;
// 接客不要警告秒数(第２段階)
var gUnNecessarySecondTimeAlertSpan = 0;
// 商談中断警告秒数
var gStopTimeAlertSpan = 0;
// $02 end   新車タブレットショールーム管理機能開発

// 通知依頼警告秒数(査定)
var gAssessmentTimeAlertSpan = 0;

// 通知依頼警告秒数(価格相談)
var gPriceTimeAlertSpan = 0;

// 通知依頼警告秒数(ヘルプ)
var gHelpTimeAlertSpan = 0;

// 商談中詳細ポップオーバー 通知依頼送信日時リスト
var gStaffDetailRequestSendDateTimeList = [];

// 紐付け解除ポップオーバー 来店時間リスト
var gLinkingCancelVisitTimeList = [];

// 警告音出力中番号リスト
//var gAlertOutputNoList = [];

// 操作権限による警告音出力フラグ
var gMainAlertFlg = false;

// プッシュ送信による警告音出力フラグ
var gPushAlertFlg = false;

// 警告チップの数
var gAlertChipCount = 0;

// 処理停止フラグ
var gLogicStopFlag = false;

// $01 start クルクル対応
// 初期表示かどうかを示すフラグ
var gDispInitFlag = true;

// ローディンぐ時のタイマー起動フラグ
var gTimerFlag = false;
// $01 end   クルクル対応

// $04 start TKM Change request development for Next Gen e-CRB (CR075)
//定期リフレッシュカウンター
var gRefreshCounter = 0;

//定期リフレッシュ間隔(秒)
var gRefreshInterval = 0;
// $04 start TKM Change request development for Next Gen e-CRB (CR075)

// ==============================================================
// DOMロード時処理
// ==============================================================
$(function () {

    // ローディングウィンドウを開く
    showLodingWindow();

    // $01 start クルクル対応
    gDispInitFlag = false;
    // $01 end   クルクル対応

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
        * リロード要求フラグ
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

            // $02 start 新車タブレットショールーム管理機能開発
            var frame = $("<iframe seamless src='SC3100102.aspx' width ='1000px' height='625px' scrolling='no' frameborder='0' style='position:absolute; left:14px;'></iframe>");
            // $02 end   新車タブレットショールーム管理機能開発
            frame.attr('id', 'fr1_1');
            frame.attr('name', 'frame1_1');
            $('div#frameArea').append(frame);

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

            // $02 start 新車タブレットショールーム管理機能開発
            var frame = $("<iframe seamless src='SC3100102.aspx' width ='1000px'  height='625px' scrolling='no' frameborder='0' style='position:absolute; left:14px;'></iframe>");
            // $02 end   新車タブレットショールーム管理機能開発
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

            // 同一ＩＤのフレームが存在する場合は削除する
            if ($('#' + frame.attr('id')).length > 0) {
                $('#' + frame.attr('id')).remove();
            }

            $('div#frameArea').append(frame);

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
            var activeFrameDom = this.getActiveDom();
            var noActiveFrameJQuery = this.getNoActiveJQuery();

            // カウンターをリセットする
            activeFrameDom.resetCounter();

            // 表示の切り替えを行う
            activeFrameJQuery.css('display', 'none');
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

    // z-indexについて、以下の順で定義する
    // ・フレームエリア                              z-index:20
    // ・オーバーレイ（グレー）                      z-index:10
    // オーバーレイ（グレー）
    var mainAreaOverlayGray = $("<div id='MainAreaOverlayGray'/>").css({
        position: 'absolute', top: '0px', left: '0px',
        width: $('body').outerWidth() > $(window).width() ? $('body').outerWidth() : $(window).width(),
        height: $('body').outerHeight() > $(window).height() ? $('body').outerHeight() : $(window).height(),
        background: 'rgba(0, 0, 0, 0.5)', display: 'none'
    });

    $('div#frameArea').append(mainAreaOverlayGray);

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
        $('input#LockButton').bind('click', function () {
            lock();
        });

        // ロック解除
        $('input#ResetButton').bind('click', function () {
            lockReset();
        });

        // リロード要求
        $('input#ReloadRequestButton').bind('click', function () {
            reloadRequest();
        });

        // 強制リロード
        $('input#ReloadForceButton').bind('click', function () {
            reloadForce();
        });

        // Push受信
        $('input#PushReserveButton').bind('click', function () {
            SC3100101Update($('select#functionNo').val(), $('select#logicNo').val());
        });
    }

    // 処理停止フラグ判定
    if ($('input#LogicStopStatus').val() == "1") {
        gLogicStopFlag = true;
    }

    // エリア定義定義を行う
    initArea();

    // フッター定義を行う
    initFooter();

    // ポップオーバー定義を行う
    initPopOver();

    // カウンター定義を行う
    initCounter();

    // アラート定義を行う
    initAlert();

    /**
    * エリア定義を行う.
    */
    function initArea() {

        // エリア選択時
        $('div#bodyFrame').bind(C_TOUCH_START, function (aEvent) {

            // デバッグ用
            if (gDebugObject.debugFlag) {

                if ($(aEvent.target).parents('div#DebugArea').length > 0) {
                    return;
                }

            }

            // 警告音停止処理を行う(廃止)
            //stopAlertOutput();

            // フレームエリアの場合を除く
            if ($(aEvent.target).attr('id') == 'frameArea') {
                return;
            }

            // お客様情報ポップアップの場合を除く
            if ($(aEvent.target).parents('div#CustomerPopOver').length > 0) {
                return;
            }

            // 紐付け解除ポップアップの場合を除く
            if ($(aEvent.target).parents('div#LinkingCancelPopOver').length > 0) {
                return;
            }

            // 商談中詳細ポップアップの場合を除く
            if ($(aEvent.target).parents('div#StaffDetailPopOver').length > 0) {
                return;
            }

            var activeDom = gFrameObject.getActiveDom();
            activeDom.hidePopOver();
            activeDom.hideOverlay();
            activeDom.hideDeleteButton();
            lockReset();
        });

        // 読み込みエリア出力中に画面を操作するとスクリプトエラーが発生する件の対応
        $('div.MstPG_LoadingScreen:eq(0)').bind(C_TOUCH_START, function (e) {
            //ブラウザのデフォルト動作（ダブルタップ、ピンチ）を禁止
            e.preventDefault();
            //イベントパブリングを抑制
            e.stopPropagation();
        });
        $('div.MstPG_LoadingScreen:eq(0)').bind(C_TOUCH_MOVE, function (e) {
            //ブラウザのデフォルト動作（ダブルタップ、ピンチ）を禁止
            e.preventDefault();
            //イベントパブリングを抑制
            e.stopPropagation();
        });
        $('div.MstPG_LoadingScreen:eq(0)').bind(C_TOUCH_END, function (e) {
            //ブラウザのデフォルト動作（ダブルタップ、ピンチ）を禁止
            e.preventDefault();
            //イベントパブリングを抑制
            e.stopPropagation();
        });

    }

    /**
    * フッター定義を行う.
    */
    function initFooter() {

        $('div#MstPG_FootItem_Main_100').click(function () {
            showLodingWindow()
        });

        $('div#MstPG_FootItem_Main_1200').click(function () {
            showLodingWindow()
        });

        if ($('input#OperationStatus').val() != C_UPDATE_OPERATION) {

            //顧客詳細
            $('div#MstPG_FootItem_Main_200').click(function () {
                showLodingWindow()
            });

            //TCV
            $('div#MstPG_FootItem_Main_300').click(function () {
                showLodingWindow()
            });
        }
    }

    /**
    * ポップオーバー定義を行う.
    */
    function initPopOver() {

        $("#bodyFrame").bind('popoverOpened', function () {
            // 透明なオーバーレイを設定する
            showOverlayTransparency();
        });

        $("#bodyFrame").bind('popoverClosed', function () {
            // オーバーレイを削除する
            hideOverlay();
        });

        // ポップオーバー画面でのロックカウンターリセット処理
        $('.popoverEx').live(C_TOUCH_START, function () {
            lockCounterReset();
        });

        $('.popoverEx').find('input:text').live('focusin', function () {
            lockCounterReset();
            gInputFlg = true;
        });

        $('.popoverEx').find('input:text').live('focusout', function (e) {
            gInputFlg = false;
        });

        // お客様情報入力画面（非同期通信エリアであるためbindではなくliveでイベントを紐付ける）
        var customerPopOver = $('div#CustomerPopOver');

        // お客様情報入力画面-登録ボタンタップ
        var scNscPopUpCompleteButton = $('a#scNscPopUpCompleteButton');
        scNscPopUpCompleteButton.live(C_TOUCH_START, function () {

            var activeDom = gFrameObject.getActiveDom();

            // 読み込み中は処理しない
            if (customerPopOver.find("div.MstPG_LoadingScreen:visible").length > 0) {
                return;
            }

            // 二度押し防止
            if (C_UPDATE_FLAG_ON != scNscPopUpCompleteButton.data(C_UPDATE_FLAG_NAME)) {

                showLodingWindowMainArea();

                scNscPopUpCompleteButton.data(C_UPDATE_FLAG_NAME, C_UPDATE_FLAG_ON);

                // デバッグ用
                if (gDebugObject.debugFlag) {
                    var pageMethodsCount = $('span#PageMethodsCount').text() - 0 + 1;
                    $('span#PageMethodsCount').text(pageMethodsCount)
                    $('span#PageMethodsFlag').show();
                }

                var customerNameTextBoxValue = "";
                if (customerPopOver.find('input#CustomerNameTextBox').length > 0) {
                    customerNameTextBoxValue = customerPopOver.find('input#CustomerNameTextBox').val();
                }

                //$05 start NSK s.natsume TKM Change request development for Next Gen e-CRB (CR060)
                //電話番号を取得する
                var customerTelNumberTextBoxValue = "";
                if (customerPopOver.find('input#CustomerTelNumberTextBox').length > 0) {
                    customerTelNumberTextBoxValue = customerPopOver.find('input#CustomerTelNumberTextBox').val();
                }
                //$05 end NSK s.natsume TKM Change request development for Next Gen e-CRB (CR060)

                //ページメソッドの呼び出し
                //$05 start NSK s.natsume TKM Change request development for Next Gen e-CRB (CR060)
                //                PageMethods.RegistrationButton_Click(
                //                    customerPopOver.find('input#CustomerDialogVisitSeq').val()
                //                    , customerPopOver.find('input#CustomerDialogCustomerSegment').val()
                //                    , customerNameTextBoxValue
                //                    , customerPopOver.find('input#CustomerDialogSalesTableNoOld').val()
                //                    , customerPopOver.find('input#CustomerDialogSalesTableNoNew').val()
                //                    , function (aResult, aUserContext, aMethodName) {

                //                        // デバッグ用
                //                        if (gDebugObject.debugFlag) {
                //                            $('span#PageMethodsFlag').hide();
                //                        }

                //                        scNscPopUpCompleteButton.data(C_UPDATE_FLAG_NAME, C_UPDATE_FLAG_OFF);

                //                        // エラー時
                //                        if (aResult !== '') {
                //                            icropScript.ShowMessageBox(0, aResult, '');
                //                            activeDom.hideOverlay();
                //                            reloadForce();
                //                            activeDom.hidePopOver();
                //                            return;
                //                        }

                //                        // 正常時
                //                        activeDom.hideOverlay();
                //                        reloadForce();
                //                        activeDom.hidePopOver();
                //                    }
                //                , onFailedPageMethods);
                PageMethods.RegistrationButton_Click(
                    customerPopOver.find('input#CustomerDialogVisitSeq').val()
                    , customerPopOver.find('input#CustomerDialogCustomerSegment').val()
                    , customerNameTextBoxValue
                    , customerPopOver.find('input#CustomerDialogSalesTableNoOld').val()
                    , customerPopOver.find('input#CustomerDialogSalesTableNoNew').val()
                    , customerTelNumberTextBoxValue
                    , $('input#TentativeNameCharacterType').val()
                    , $('input#TelNumberCharacterType').val()
                    , function (aResult, aUserContext, aMethodName) {

                        // デバッグ用
                        if (gDebugObject.debugFlag) {
                            $('span#PageMethodsFlag').hide();
                        }

                        scNscPopUpCompleteButton.data(C_UPDATE_FLAG_NAME, C_UPDATE_FLAG_OFF);

                        // エラー時
                        if (aResult !== '') {
                            icropScript.ShowMessageBox(0, aResult, '');
                            activeDom.hideOverlay();
                            reloadForce();
                            activeDom.hidePopOver();
                            return;
                        }

                        // 正常時
                        activeDom.hideOverlay();
                        reloadForce();
                        activeDom.hidePopOver();
                    }
                , onFailedPageMethods);
                //$05 end NSK s.natsume TKM Change request development for Next Gen e-CRB (CR060)
            }
        });

        // お客様情報入力画面-送信ボタンタップ
        var popupContactVisitSubmitButtonOn = $('div#PopupContactVisitSubmitButtonOn');
        popupContactVisitSubmitButtonOn.live(C_TOUCH_START, function () {

            var activeDom = gFrameObject.getActiveDom();

            // 二度押し防止
            if (C_UPDATE_FLAG_ON != popupContactVisitSubmitButtonOn.data(C_UPDATE_FLAG_NAME)) {

                showLodingWindowMainArea();

                popupContactVisitSubmitButtonOn.data(C_UPDATE_FLAG_NAME, C_UPDATE_FLAG_ON);

                // デバッグ用
                if (gDebugObject.debugFlag) {
                    var pageMethodsCount = $('span#PageMethodsCount').text() - 0 + 1;
                    $('span#PageMethodsCount').text(pageMethodsCount)
                    $('span#PageMethodsFlag').show();
                }

                var customerNameTextBoxValue = "";
                if (customerPopOver.find('input#CustomerNameTextBox').length > 0) {
                    customerNameTextBoxValue = customerPopOver.find('input#CustomerNameTextBox').val();
                }

                //$05 start NSK s.natsume TKM Change request development for Next Gen e-CRB (CR060)
                //電話番号を取得する
                var customerTelNumberTextBoxValue = "";
                if (customerPopOver.find('input#CustomerTelNumberTextBox').length > 0) {
                    customerTelNumberTextBoxValue = customerPopOver.find('input#CustomerTelNumberTextBox').val();
                }
                //$05 end NSK s.natsume TKM Change request development for Next Gen e-CRB (CR060)

                //ページメソッドの呼び出し
                //$05 start NSK s.natsume TKM Change request development for Next Gen e-CRB (CR060)
                //                PageMethods.BroadcastButton_Click(
                //                        customerPopOver.find('input#CustomerDialogVisitSeq').val()
                //                    , customerPopOver.find('input#CustomerDialogCustomerSegment').val()
                //                    , customerNameTextBoxValue
                //                    , customerPopOver.find('input#CustomerDialogSalesTableNoOld').val()
                //                    , customerPopOver.find('input#CustomerDialogSalesTableNoNew').val()
                //                    , customerPopOver.find('input#CustomerDialogVehicleRegistrationNo').val()
                //                    , function (aResult, aUserContext, aMethodName) {

                //                        // デバッグ用
                //                        if (gDebugObject.debugFlag) {
                //                            $('span#PageMethodsFlag').hide();
                //                        }

                //                        popupContactVisitSubmitButtonOn.data(C_UPDATE_FLAG_NAME, C_UPDATE_FLAG_OFF);

                //                        var resultList = $.evalJSON(aResult);

                //                        // エラー時
                //                        if (resultList[0] == '1') {
                //                            icropScript.ShowMessageBox(0, resultList[1], '');
                //                            activeDom.hideOverlay();
                //                            reloadForce();
                //                            activeDom.hidePopOver();
                //                            return;
                //                        }

                //                        // 正常時でメッセージがある場合
                //                        if (resultList[1] !== '') {
                //                            icropScript.ShowMessageBox(0, resultList[1], '');
                //                            closeLodingWindow();
                //                            return;
                //                        }

                //                        // 正常時
                //                        activeDom.hideOverlay();
                //                        reloadForce();
                //                        activeDom.hidePopOver();
                //                    }
                //                    , onFailedPageMethods);
                PageMethods.BroadcastButton_Click(
                        customerPopOver.find('input#CustomerDialogVisitSeq').val()
                    , customerPopOver.find('input#CustomerDialogCustomerSegment').val()
                    , customerNameTextBoxValue
                    , customerPopOver.find('input#CustomerDialogSalesTableNoOld').val()
                    , customerPopOver.find('input#CustomerDialogSalesTableNoNew').val()
                    , customerPopOver.find('input#CustomerDialogVehicleRegistrationNo').val()
                    , customerTelNumberTextBoxValue
                    , $('input#TentativeNameCharacterType').val()
                    , $('input#TelNumberCharacterType').val()
                    , function (aResult, aUserContext, aMethodName) {

                        // デバッグ用
                        if (gDebugObject.debugFlag) {
                            $('span#PageMethodsFlag').hide();
                        }

                        popupContactVisitSubmitButtonOn.data(C_UPDATE_FLAG_NAME, C_UPDATE_FLAG_OFF);

                        var resultList = $.evalJSON(aResult);

                        // エラー時
                        if (resultList[0] == '1') {
                            icropScript.ShowMessageBox(0, resultList[1], '');
                            activeDom.hideOverlay();
                            reloadForce();
                            activeDom.hidePopOver();
                            return;
                        }

                        // 正常時でメッセージがある場合
                        if (resultList[1] !== '') {
                            icropScript.ShowMessageBox(0, resultList[1], '');
                            closeLodingWindow();
                            return;
                        }

                        // 正常時
                        activeDom.hideOverlay();
                        reloadForce();
                        activeDom.hidePopOver();
                    }
                    , onFailedPageMethods);
                //$05 end NSK s.natsume TKM Change request development for Next Gen e-CRB (CR060)
            }
        });

        // お客様情報入力画面-キャンセルボタンタップ
        $('a#scNscPopUpCancelButton').live(C_TOUCH_START, function () {

            var activeDom = gFrameObject.getActiveDom();

            activeDom.hidePopOver();
            lockReset();
        });

        // お客様情報入力画面-テーブル番号タップ時イベント
        customerPopOver.find('ul.scNscPopUpContactVisitNoButton').find('div').live(C_TOUCH_START, function () {

            // 現在選択されているチップを取得する
            var selectNew = $(this);
            var selectNewTableNo = selectNew.find('input#SelectSalesTableNo').val();

            // 以前に選択されていたチップを取得する
            var selectOld = customerPopOver.find('div.NoButtonOn');
            var selectOldTableNo = '';
            if (selectOld.length > 0) {
                selectOldTableNo = selectOld.find('input#SelectSalesTableNo').val();
            }

            // 変更が無い場合
            if (selectOldTableNo == selectNewTableNo) {
                // 選択したチップの状態を元に戻す
                selectNew.attr('class', selectNew.data('beforeClass'));

                // 値を保持する
                $('input#CustomerDialogSalesTableNoNew').val('');
            }
            // 変更がある場合
            else {
                // 以前に選択されていたチップの状態を元に戻す
                selectOld.attr('class', selectOld.data('beforeClass'));
                // 選択したチップの状態を保持する
                $(this).data('beforeClass', $(this).attr('class'));
                // 選択したチップの状態を変更する
                $(this).attr('class', 'NoButtonOn');

                // 値を保持する
                $('input#CustomerDialogSalesTableNoNew').val(selectNewTableNo);
            }
        });

        // $02 start 新車タブレットショールーム管理機能開発
        // お客様情報入力画面-接客不要ボタンタップ
        var popupUnNecessarySubmitButtonOn = $('div#PopupUnNecessarySubmitButtonOn');
        popupUnNecessarySubmitButtonOn.live(C_TOUCH_START, function () {
            var activeDom = gFrameObject.getActiveDom();
            // 二度押し防止
            if (C_UPDATE_FLAG_ON != popupUnNecessarySubmitButtonOn.data(C_UPDATE_FLAG_NAME)) {

                showLodingWindowMainArea();

                popupUnNecessarySubmitButtonOn.data(C_UPDATE_FLAG_NAME, C_UPDATE_FLAG_ON);

                // デバッグ用
                if (gDebugObject.debugFlag) {
                    var pageMethodsCount = $('span#PageMethodsCount').text() - 0 + 1;
                    $('span#PageMethodsCount').text(pageMethodsCount)
                    $('span#PageMethodsFlag').show();
                }

                // 仮登録氏名取得
                var customerNameTextBoxValue = "";
                if (customerPopOver.find('input#CustomerNameTextBox').length > 0) {
                    customerNameTextBoxValue = customerPopOver.find('input#CustomerNameTextBox').val();
                }

                //$05 start NSK s.natsume TKM Change request development for Next Gen e-CRB (CR060)
                //電話番号を取得する
                var customerTelNumberTextBoxValue = "";
                if (customerPopOver.find('input#CustomerTelNumberTextBox').length > 0) {
                    customerTelNumberTextBoxValue = customerPopOver.find('input#CustomerTelNumberTextBox').val();
                }
                //$05 end NSK s.natsume TKM Change request development for Next Gen e-CRB (CR060)

                //ページメソッドの呼び出し
                //$05 start NSK s.natsume TKM Change request development for Next Gen e-CRB (CR060)
                //                PageMethods.UnNecessaryButton_Click(
                //                      customerPopOver.find('input#CustomerDialogVisitSeq').val()
                //                    , customerPopOver.find('input#CustomerDialogCustomerSegment').val()
                //                    , customerNameTextBoxValue
                //                    , customerPopOver.find('input#CustomerDialogSalesTableNoOld').val()
                //                    , customerPopOver.find('input#CustomerDialogSalesTableNoNew').val()
                //                    , function (aResult, aUserContext, aMethodName) {

                //                        // デバッグ用
                //                        if (gDebugObject.debugFlag) {
                //                            $('span#PageMethodsFlag').hide();
                //                        }

                //                        popupUnNecessarySubmitButtonOn.data(C_UPDATE_FLAG_NAME, C_UPDATE_FLAG_OFF);

                //                        var resultList = $.evalJSON(aResult);

                //                        // エラー時
                //                        if (resultList[0] == '1') {
                //                            icropScript.ShowMessageBox(0, resultList[1], '');
                //                            activeDom.hideOverlay();
                //                            reloadForce();
                //                            activeDom.hidePopOver();
                //                            return;
                //                        }

                //                        // 正常時でメッセージがある場合
                //                        if (resultList[1] !== '') {
                //                            icropScript.ShowMessageBox(0, resultList[1], '');
                //                            closeLodingWindow();
                //                            return;
                //                        }

                //                        // 正常時
                //                        activeDom.hideOverlay();
                //                        reloadForce();
                //                        activeDom.hidePopOver();
                //                    }
                //                    , onFailedPageMethods);
                PageMethods.UnNecessaryButton_Click(
                      customerPopOver.find('input#CustomerDialogVisitSeq').val()
                    , customerPopOver.find('input#CustomerDialogCustomerSegment').val()
                    , customerNameTextBoxValue
                    , customerPopOver.find('input#CustomerDialogSalesTableNoOld').val()
                    , customerPopOver.find('input#CustomerDialogSalesTableNoNew').val()
                    , customerTelNumberTextBoxValue
                    , $('input#TentativeNameCharacterType').val()
                    , $('input#TelNumberCharacterType').val()
                    , function (aResult, aUserContext, aMethodName) {

                        // デバッグ用
                        if (gDebugObject.debugFlag) {
                            $('span#PageMethodsFlag').hide();
                        }

                        popupUnNecessarySubmitButtonOn.data(C_UPDATE_FLAG_NAME, C_UPDATE_FLAG_OFF);

                        var resultList = $.evalJSON(aResult);

                        // エラー時
                        if (resultList[0] == '1') {
                            icropScript.ShowMessageBox(0, resultList[1], '');
                            activeDom.hideOverlay();
                            reloadForce();
                            activeDom.hidePopOver();
                            return;
                        }

                        // 正常時でメッセージがある場合
                        if (resultList[1] !== '') {
                            icropScript.ShowMessageBox(0, resultList[1], '');
                            closeLodingWindow();
                            return;
                        }

                        // 正常時
                        activeDom.hideOverlay();
                        reloadForce();
                        activeDom.hidePopOver();
                    }
                    , onFailedPageMethods);
                //$05 end NSK s.natsume TKM Change request development for Next Gen e-CRB (CR060)
            }
        });
        // $02 end   新車タブレットショールーム管理機能開発

        // 商談中詳細画面（非同期通信エリアであるためbindではなくliveでイベントを紐付ける）
        var StaffDetailPopOver = $('div#StaffDetailPopOver');

        // 商談中詳細画面-登録ボタンタップ
        var scNscPopUpStaffDetailCompleteButton = $('a.scNscPopUpStaffDetailCompleteButton');
        scNscPopUpStaffDetailCompleteButton.live(C_TOUCH_START, function () {

            var activeDom = gFrameObject.getActiveDom();

            // 読み込み中は処理しない
            if (StaffDetailPopOver.find("div.MstPG_LoadingScreen:visible").length > 0) {
                return;
            }

            // 二度押し防止
            if (C_UPDATE_FLAG_ON != scNscPopUpStaffDetailCompleteButton.data(C_UPDATE_FLAG_NAME)) {

                showLodingWindowMainArea();

                scNscPopUpStaffDetailCompleteButton.data(C_UPDATE_FLAG_NAME, C_UPDATE_FLAG_ON);

                // デバッグ用
                if (gDebugObject.debugFlag) {
                    var pageMethodsCount = $('span#PageMethodsCount').text() - 0 + 1;
                    $('span#PageMethodsCount').text(pageMethodsCount)
                    $('span#PageMethodsFlag').show();
                }

                //ページメソッドの呼び出し
                PageMethods.StaffDetailSubmitButton_Click(
                        StaffDetailPopOver.find('input#StaffDetailDialogVisitSeq').val()
                    , StaffDetailPopOver.find('input#StaffDetailDialogSalesTableNoOld').val()
                    , StaffDetailPopOver.find('input#StaffDetailDialogSalesTableNoNew').val()
                    , function (aResult, aUserContext, aMethodName) {

                        // デバッグ用
                        if (gDebugObject.debugFlag) {
                            $('span#PageMethodsFlag').hide();
                        }

                        scNscPopUpStaffDetailCompleteButton.data(C_UPDATE_FLAG_NAME, C_UPDATE_FLAG_OFF);

                        var resultList = $.evalJSON(aResult);

                        // エラー時
                        if (resultList[0] == '1') {
                            icropScript.ShowMessageBox(0, resultList[1], '');
                            activeDom.hideOverlay();
                            reloadForce();
                            activeDom.hidePopOver();
                            return;
                        }

                        // 正常時でメッセージがある場合
                        if (resultList[1] !== '') {
                            icropScript.ShowMessageBox(0, resultList[1], '');
                            closeLodingWindow();
                            return;
                        }

                        // 正常時
                        activeDom.hideOverlay();
                        reloadForce();
                        activeDom.hidePopOver();
                    }
                    , onFailedPageMethods);
            }
        });

        // 商談中詳細画面-キャンセルボタンタップ
        $('a#scNscPopUpStaffDetailCancelButton').live(C_TOUCH_START, function () {


            var activeDom = gFrameObject.getActiveDom();

            activeDom.hidePopOver();
            lockReset();
        });

        // 商談中詳細画面-テーブル番号タップ時イベント
        StaffDetailPopOver.find('ul.scNscPopUpStaffDetailNoButton').find('div').live(C_TOUCH_START, function () {

            // 現在選択されているチップを取得する
            var selectNew = $(this);
            var selectNewTableNo = selectNew.find('input#SelectSalesTableNo').val();

            // 以前に選択されていたチップを取得する
            var selectOld = StaffDetailPopOver.find('div.NoButtonOn');
            var selectOldTableNo = '';
            if (selectOld.length > 0) {
                selectOldTableNo = selectOld.find('input#SelectSalesTableNo').val();
            }

            // 変更が無い場合
            if (selectOldTableNo == selectNewTableNo) {
                // 選択したチップの状態を元に戻す
                selectNew.attr('class', selectNew.data('beforeClass'));

                // 値を保持する
                $('input#StaffDetailDialogSalesTableNoNew').val('');
            }
            // 変更がある場合
            else {
                // 以前に選択されていたチップの状態を元に戻す
                selectOld.attr('class', selectOld.data('beforeClass'));
                // 選択したチップの状態を保持する
                $(this).data('beforeClass', $(this).attr('class'));
                // 選択したチップの状態を変更する
                $(this).attr('class', 'NoButtonOn');

                // 値を保持する
                $('input#StaffDetailDialogSalesTableNoNew').val(selectNewTableNo);
            }
        });

        // 商談中詳細画面-テーブルNo.入力タップ
        /* 更新権限がある場合のみ実装 */
        if ($('input#OperationStatus').val() == C_UPDATE_OPERATION) {
            $('li#TableNoLink').setCommonEvent();
            $('li#TableNoLink').live('tap', function () {
                // フリックの設定
                $('div#StaffDetailPopOverFlickArea').flickPage({
                    page: 2,
                    section: $('div.StaffDetailPopOverPage'),
                    flickEnd: function () {

                        //フリックした後にヘッダーを変更する
                        $('div#scNscPopUpStaffDetailCustomerButton').css({ display: 'block' });
                        $('a#scNscPopUpStaffDetailCompleteButton').css({ display: 'none' });
                        $('a#scNscPopUpStaffDetailCancelButton').css({ display: 'none' });

                        //ヘッダ名、テーブルNo
                        $('h3#StaffName').css({ display: 'none' });
                        $('h3#TableNo').css({ display: 'block' });
                    }
                });
            });
        }

        // 商談中詳細画面-テーブルNo.入力画面-戻るボタンタップ
        $('div#scNscPopUpStaffDetailCustomerButton').live(C_TOUCH_START, function () {

            // フリックの設定
            $('div#StaffDetailPopOverFlickArea').flickPage({
                page: 1,
                section: $('div.StaffDetailPopOverPage'),
                flickEnd: function () {

                    //フリックした後にヘッダーを変更する
                    $('a#scNscPopUpStaffDetailCompleteButton').css({ display: 'block' });
                    $('div#scNscPopUpStaffDetailCustomerButton').css({ display: 'none' });
                    $('a#scNscPopUpStaffDetailCancelButton').css({ display: 'block' });

                    //テーブルNoのテキスト表示と値を反映させる
                    var tableNo = $('input#StaffDetailDialogSalesTableNoNew').val();
                    if (tableNo.length != 0) {
                        $('#StaffDetailTableNoLiteral').attr('class', 'on');
                        $('#DisplayTableNo').text($('input#StaffDetailDialogSalesTableNoNew').val());
                    } else {
                        $('#StaffDetailTableNoLiteral').attr('class', 'off');
                        $('#DisplayTableNo').text("-");
                    }
                    $('#DisplayTableNo').val($('input#StaffDetailDialogSalesTableNoNew').val());

                    //スタッフ名
                    $('h3#StaffName').css({ display: 'block' });
                    $('h3#TableNo').css({ display: 'none' });

                }
            });

        });

        // 読み取り権限の場合
        if ($('input#OperationStatus').val() != C_UPDATE_OPERATION) {

            var staffDetailCustomerNameButton = $('input#StaffDetailCustomerNameButton');

            // 商談中詳細画面-顧客名タップ
            $('li.list1').setCommonEvent();
            $('li.list1').live('tap', function () {

                // 二度押し防止
                if (C_UPDATE_FLAG_ON != staffDetailCustomerNameButton.data(C_UPDATE_FLAG_NAME)) {

                    showLodingWindowMainArea();

                    staffDetailCustomerNameButton.data(C_UPDATE_FLAG_NAME, C_UPDATE_FLAG_ON);

                    //顧客画面遷移用呼び出しボタンをキックさせる
                    $('input#StaffDetailCustomerNameButton').click();
                }
            });
        }

        // $02 start 複数顧客に対する商談平行対応
        /*
        linkingCancelCustomerList = $('div#LinkingCancelPopOver').find('ul.ListSet01').find('li.customerList');

        // 紐付け解除画面 共通イベント定義
        linkingCancelCustomerList.setCommonEvent();

        // 紐付け解除画面-顧客リスト選択処理
        linkingCancelCustomerList.live('tap', function () {

            var listTarget = $(event.target);

            if (listTarget.parents('li.customerList').length != 0) {

                listTarget = listTarget.parents('li.customerList');

            }

            // 選択済み
            if (listTarget.hasClass('Selection')) {

                listTarget.removeClass('Selection');

                // 1件も選択されていない場合は、完了ボタンを非活性にする
                if (!$('ul.ListSet01').find('li.Selection').length > 0) {
                    $('a#scNscPopUpLinkingCancelCompleteButton').attr('class', 'scNscPopUpCompleteButtonOff');
                }
            }
            else {

                listTarget.addClass('Selection');

                // 活性状態に変更
                if (!$('a#scNscPopUpLinkingCancelCompleteButton').hasClass('scNscPopUpCompleteButton')) {
                    $('a#scNscPopUpLinkingCancelCompleteButton').attr('class', 'scNscPopUpCompleteButton');
                }
            }

        });

        var linkingCancelPopOver = $('div#LinkingCancelPopOver');

        // 紐付け解除画面 - 登録ボタンタップ
        var linkingCancelCompleteButton = $('a#scNscPopUpLinkingCancelCompleteButton');

        linkingCancelCompleteButton.live(C_TOUCH_START, function () {

            var activeDom = gFrameObject.getActiveDom();

            // 読み込み中は処理しない
            if (linkingCancelPopOver.find("div.MstPG_LoadingScreen:visible").length > 0) {
                return;
            }

            // 更新対象スタッフコード
            var updateAccount = linkingCancelPopOver.find('input#LinkingCancelDialogAccount').val();

            // 表示内容をリストに格納
            var updateVisitSeqList = [];

            // チェックされている顧客のみ更新リストに加える
            $('li.customerList').each(function () {

                if ($(this).hasClass('Selection')) {

                    updateVisitSeqList.push($(this).find('input#LinkingCustomerVisitSeq').val());
                }
            });

            if (updateVisitSeqList.length <= 0) {
                return;
            }

            // 二度押し防止
            if (C_UPDATE_FLAG_ON != linkingCancelCompleteButton.data(C_UPDATE_FLAG_NAME)) {

                showLodingWindowMainArea();

                linkingCancelCompleteButton.data(C_UPDATE_FLAG_NAME, C_UPDATE_FLAG_ON);

                // デバッグ用
                if (gDebugObject.debugFlag) {
                    var pageMethodsCount = $('span#PageMethodsCount').text() - 0 + 1;
                    $('span#PageMethodsCount').text(pageMethodsCount)
                    $('span#PageMethodsFlag').show();
                }

                // ページメソッドの呼び出し
                PageMethods.LinkingCancelButton_Click(
                    updateVisitSeqList
                    , updateAccount
                    , function (aResult, aUserContext, aMethodName) {

                        // デバッグ用
                        if (gDebugObject.debugFlag) {
                            $('span#PageMethodsFlag').hide();
                        }

                        linkingCancelCompleteButton.data(C_UPDATE_FLAG_NAME, C_UPDATE_FLAG_OFF);

                        // エラー時
                        if (aResult !== '') {
                            icropScript.ShowMessageBox(0, aResult, '');
                            activeDom.hideOverlay();
                            reloadForce();
                            activeDom.hidePopOver();
                            return;
                        }

                        // 正常時
                        activeDom.hideOverlay();
                        reloadForce();
                        activeDom.hidePopOver();
                    }
                , onFailedPageMethods);
            }
        });

        // 紐付け解除画面-キャンセルボタンタップ
        $('a#scNscPopUpLinkingCancelCancelButton').live(C_TOUCH_START, function () {

            var activeDom = gFrameObject.getActiveDom();

            activeDom.hidePopOver();
            lockReset();

        });
        */
        // $02 end   複数顧客に対する商談平行対応
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
            gVisitTimeAlertSpan = $('input#VisitTimeAlertSpan').val() - 0;
        }

        if ($('input#WaitTimeAlertSpan').val().length > 0) {
            // 分を秒に変換して保持する
            gWaitTimeAlertSpan = $('input#WaitTimeAlertSpan').val() - 0;
        }

        // $02 start 新車タブレットショールーム管理機能開発
        // 接客不要警告秒数(第１段階)
        if ($('input#UnNecessaryFirstTimeAlertSpan').val().length > 0) {
            // 分を秒に変換して保持する
            gUnNecessaryFirstTimeAlertSpan = $('input#UnNecessaryFirstTimeAlertSpan').val() - 0;
        }
        // 接客不要警告秒数(第２段階)
        if ($('input#UnNecessarySecondTimeAlertSpan').val().length > 0) {
            // 分を秒に変換して保持する
            gUnNecessarySecondTimeAlertSpan = $('input#UnNecessarySecondTimeAlertSpan').val() - 0;
        }
        // 談中断警告秒数
        if ($('input#StopTimeAlertSpan').val().length > 0) {
            // 分を秒に変換して保持する
            gStopTimeAlertSpan = $('input#StopTimeAlertSpan').val() - 0;
        }
        // $02 end   新車タブレットショールーム管理機能開発

        // $04 start TKM Change request development for Next Gen e-CRB (CR075)
        if ($('input#RefreshInterval').val().length > 0) {
            gRefreshInterval = $('input#RefreshInterval').val();
        }
        // $04 end TKM Change request development for Next Gen e-CRB (CR075)

        // 通知依頼警告時間(査定)
        if ($('input#AssessmentAlertSpan').val().length > 0) {
            gAssessmentTimeAlertSpan = $('input#AssessmentAlertSpan').val() - 0;
        }

        // 通知依頼警告時間(価格相談)
        if ($('input#PriceAlertSpan').val().length > 0) {
            gPriceTimeAlertSpan = $('input#PriceAlertSpan').val() - 0;
        }

        // 通知依頼警告時間(ヘルプ)
        if ($('input#HelpAlertSpan').val().length > 0) {
            gHelpTimeAlertSpan = $('input#HelpAlertSpan').val() - 0;
        }

        counter();

        // 処理停止中の場合は処理しない
        if (gLogicStopFlag) {
            return;
        }

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

            // エラーメッセージの取得
            var errorMessageObject = null;

            if (targetElementId == 'CustomerDialogDisplayButton') {
                errorMessageObject = $('input#CustomerPopoverErrorMessage');
            }
            else if (targetElementId == 'StaffDetailDisplayButton') {
                errorMessageObject = $('input#StaffDetailPopoverErrorMessage');
            }
            else if (targetElementId == 'LinkingCancelDialogDisplayButton') {
                errorMessageObject = $('input#LinkingCancelPopoverErrorMessage');
            }

            // エラー時
            if (errorMessageObject.val() !== '') {
                icropScript.ShowMessageBox(0, errorMessageObject.val(), '');

                activeDom.hideOverlay();
                showLodingWindowMainArea();
                reloadForce();
                activeDom.hidePopOver();
                errorMessageObject.val('');
                return;
            }

            activeDom.closeLodingWindow();

            // 初期処理
            if (targetElementId == 'CustomerDialogDisplayButton') {
                // お客様情報入力画面の初期化
                InitCustomerPopOver();
            }
            else if (targetElementId == 'StaffDetailDisplayButton') {
                // 商談中詳細画面の初期化
                InitStaffDetailPopOver();
            }
            // $02 start 複数顧客に対する商談平行対応
            /*
            else if (targetElementId == 'LinkingCancelDialogDisplayButton') {
                // 紐付け解除画面の初期化
                InitLinkingCancelPopOver();
            }
            */
            // $02 end   複数顧客に対する商談平行対応

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
* 顧客チップの削除処理を行う.
*/
function visitorChipDelete(aVisitSeq) {

    var activeDom = gFrameObject.getActiveDom();

    //ページメソッドの呼び出し
    PageMethods.VisitorDelButton_Click(
        aVisitSeq
        , function (aResult, aUserContext, aMethodName) {

            // デバッグ用
            if (gDebugObject.debugFlag) {
                $('span#PageMethodsFlag').hide();
            }

            // エラー時
            if (aResult !== '') {
                icropScript.ShowMessageBox(0, aResult, '');
                activeDom.hideOverlay();
                reloadForce();
                activeDom.hidePopOver();
                return;
            }

            // 正常時
            activeDom.hideOverlay();
            reloadForce();
            activeDom.hidePopOver();
        }
        , onFailedPageMethods
    );
}

/**
* 紐付け処理を行う.
*/
function visitorChipSend(aVisitSeq, aAccount) {

    var activeDom = gFrameObject.getActiveDom();

    //ページメソッドの呼び出し
    PageMethods.SendButton_Click(
        aVisitSeq
        , aAccount
        , function (aResult, aUserContext, aMethodName) {

            // デバッグ用
            if (gDebugObject.debugFlag) {
                $('span#PageMethodsFlag').hide();
            }

            // エラー時
            if (aResult !== '') {
                icropScript.ShowMessageBox(0, aResult, '');
                activeDom.hideOverlay();
                reloadForce();
                activeDom.hidePopOver();
                return;
            }

            // 正常時
            activeDom.hideOverlay();
            reloadForce();
            activeDom.hidePopOver();
        }
    , onFailedPageMethods);
}

/**
* お客様情報入力画面の初期化.
*/
function InitCustomerPopOver() {

    // テキストボックスの初期化
    $('div#CustomerPopOver').find('input:text').CustomTextBox();

    // 初期選択状態設定
    var tableNo = $('input#CustomerDialogSalesTableNoNew').val();
    var tableNoList = $('div#CustomerPopOver').find('ul.scNscPopUpContactVisitNoButton').find('div');

    tableNoList.each(function () {

        if ($(this).find('input#SelectSalesTableNo').val() == tableNo) {
            $(this).data('beforeClass', $(this).attr('class'));
            $(this).attr('class', 'NoButtonOn');
        }
    });

}

/**
* 商談中詳細画面の初期化.
*/
function InitStaffDetailPopOver() {

    // テーブルNoの初期選択状態設定
    var tableNo = $('input#StaffDetailDialogSalesTableNoNew').val();
    var tableNoList = $('div#StaffDetailPopOver').find('ul.scNscPopUpStaffDetailNoButton').find('div');

    if (tableNo.length != 0) {
        tableNoList.each(function () {
            if ($(this).find('input#SelectSalesTableNo').val() == tableNo) {

                $(this).data('beforeClass', $(this).attr('class'));
                $(this).attr('class', 'NoButtonOn');
            }
        });
    }

    /* 通知依頼送信日時を設定 */
    if ($('input#SendDateList').val().length > 0) {
        gStaffDetailRequestSendDateTimeList = $.evalJSON($('input#SendDateList').val());
    }

    /* 依頼リスト 信号処理 */
    $("li#NoticeName").each(function (aIndex, aValue) {

        //通信依頼種別事の信号を出力
        if ($(this).find('input#NoticeReqctg').val() == '01') {

            $(this).find('p#NoticeTime').data('NoticeLimitTime', gAssessmentTimeAlertSpan);

        } else if ($(this).find('input#NoticeReqctg').val() == '02') {

            $(this).find('p#NoticeTime').data('NoticeLimitTime', gPriceTimeAlertSpan);

        } else if ($(this).find('input#NoticeReqctg').val() == '03') {

            $(this).find('p#NoticeTime').data('NoticeLimitTime', gHelpTimeAlertSpan);

        }

    });

    $("div#NegotiateTime").val($('input#StaffDetailDialogSalesStartTime').val());

    // スクロールの設定を行う
    $(".scNscPopUpStaffDetailScroll").fingerScroll();
    $('div.scroll-inner').css('paddingBottom', '10px');

    // 商談中詳細画面のカウンター処理
    StaffDetailPopOverCounter(true, false);

}

/**
* 商談中詳細画面のカウンター処理.
*/
function StaffDetailPopOverCounter(aDrawFlag, aCountUpFlag) {

    /* 商談中詳細 依頼リスト 経過時間処理 */
    $.each(gStaffDetailRequestSendDateTimeList, function (aIndex, aValue) {

        if (0 < aValue.length) {

            // カウントアップを行ってから表示する（開いた瞬間はカウントアップしないため）
            if (aCountUpFlag) {
                gStaffDetailRequestSendDateTimeList[aIndex] = (aValue - 0) + 1 + '';
            }

            if (aDrawFlag) {

                // 警告時間の取得
                var limitTime = $('div#StaffDetailPopOverBody').find('p#NoticeTime:eq(' + aIndex + ')').data('NoticeLimitTime') - 0;

                // 指定の時間を経過してしまった場合
                if (0 <= limitTime && limitTime < (gStaffDetailRequestSendDateTimeList[aIndex] - 0)) {
                    $('div#StaffDetailPopOverBody').find('p#NoticeTime:eq(' + aIndex + ')').addClass('FontRed');
                }

                // 時間の描画
                $('div#StaffDetailPopOverBody').find('p#NoticeTime:eq(' + aIndex + ')').text(
                    getDispTime(gStaffDetailRequestSendDateTimeList[aIndex], C_MAX_LENGTH_NOTICE_SPAN_TIME));

            }

        }

    });

    // カウントアップを行ってから表示する（開いた瞬間はカウントアップしないため）
    if (aCountUpFlag) {
        var addDate = ($('div#NegotiateTime').val() - 0) + 1 + '';
        $("div#NegotiateTime").val(addDate);
    }
    else {
        // 商談経過時間がずれるため、サブ画面の配列より商談経過時間を取得する
        var activeDom = gFrameObject.getActiveDom();
        $("div#NegotiateTime").val(activeDom.gNegotiationTimeSpanList[$("input#StaffDetailDialogIndex").val()]);
    }

    if (aDrawFlag) {
        /* 商談時間 */
        $("div#NegotiateTime").text(getDispTime($('div#NegotiateTime').val(), C_MAX_LENGTH_STAFF_SPAN_TIME));
    }

}

/**
* 商談中詳細画面の削除.
*/
function DeleteStaffDetailPopOver() {

    // 削除
    $('div#StaffDetailPopOver').find('div.scNscPopUpStaffDetailListArea').html('');

}

// $02 start 複数顧客に対する商談平行対応
/**
* 紐付け解除画面の初期化.
*/
/*
function InitLinkingCancelPopOver() {

    // 来店日時を取得
    if ($('input#VisitTimeList').val().length > 0) {
        gLinkingCancelVisitTimeList = $.evalJSON($('input#VisitTimeList').val());
    }

    // 警告時間の設定
    $("li.customerList").each(function (aIndex, aValue) {

        //通信依頼種別事の信号を出力
        if ($(this).find('input#LinkingCustomerVisitStatus').val() == '06') {

            $(this).find('p.LinkingCancelVisitTime').data('LinkingCancelLimitTime', gWaitTimeAlertSpan);

        } else {

            $(this).find('p.LinkingCancelVisitTime').data('LinkingCancelLimitTime', gVisitTimeAlertSpan);

        }

    });

    // フリック設定
    $('div.scNscPopUpLinkingCancelScroll').fingerScroll();

    // 位置の調節
    $('div.scroll-inner').css('paddingBottom', '10px');

    // 紐付け解除画面のカウンター処理
    LinkingCancelPopOverCounter(true, false);
}
*/

/**
* 紐付け解除画面のカウンター処理.
*/
/*
function LinkingCancelPopOverCounter(aDrawFlag, aCountUpFlag) {

    // 紐付け解除 来店時間
    $.each(gLinkingCancelVisitTimeList, function (aIndex, aValue) {

        if (0 < aValue.length) {

            // カウントアップを行ってから表示する（開いた瞬間はカウントアップしないため）
            if (aCountUpFlag) {
                gLinkingCancelVisitTimeList[aIndex] = (aValue - 0) + 1 + '';
            }

            if (aDrawFlag) {

                // 警告時間の取得
                var limitTime = $('div#LinkingCancelPopOver').find('p.LinkingCancelVisitTime:eq(' + aIndex + ')').data('LinkingCancelLimitTime') - 0;

                // 指定の時間を経過してしまった場合
                if (0 <= limitTime && limitTime < (gLinkingCancelVisitTimeList[aIndex] - 0)) {
                    $('div#LinkingCancelPopOver').find('li.customerList:eq(' + aIndex + ')').addClass('FontRed');
                }

                // 時間の描画
                $('div#LinkingCancelPopOver').find('p.LinkingCancelVisitTime:eq(' + aIndex + ')').text(
                    getDispTime(gLinkingCancelVisitTimeList[aIndex], C_MAX_LENGTH_LINKING_CANCEL_SPAN_TIME));

            }
        }

    });

}
*/
// $02 end   複数顧客に対する商談平行対応

/**
* お客様情報入力画面の削除.
*/
function DeleteCustomerPopOver() {

    // 削除
    // $02 start 新車タブレットショールーム管理機能開発
    $('div#CustomerPopOver').find('div.dataBox').html('');
    // $02 end   新車タブレットショールーム管理機能開発
}

/**
* 紐付け解除画面の削除.
*/
function DeleteLinkingCancelPopOver() {

    // 削除
    $('div#LinkingCancelPopOver').find('div.scNscPopUpLinkingCancelListArea').html('');

}

/**
* カウンター処理を行う.
*/
function counter() {

    var loadFlag = false;

    if ($('div.MstPG_LoadingScreen:visible').length > 0) {
        loadFlag = true;

        // ロックカウンターをリセットする
        lockCounterReset();

    }

    // 以下の場合、ロックカウンタをカウントアップしない
    // ・ロード中
    // ・ドラッグ中
    // ・入力中
    if (gLockFlag && !loadFlag && !gDragFlg && !gInputFlg) {

        this.gLockCounter++;

        if (0 <= gLockCounter && gLockResetInterval < gLockCounter) {

            var activeDom = gFrameObject.getActiveDom();
            activeDom.hidePopOver();
            activeDom.hideOverlay();
            activeDom.hideDeleteButton();
            lockReset();
        }
    }

    // デバッグ用
    if (gDebugObject.debugFlag) {
        $('#pageData').text('LockFlag:' + gLockFlag + ', LockCounter:' + gLockCounter + ', GrayOutFlg:' + gGrayOutFlg + ', PopupFlg:' + gPopupFlg + ', LoadFlg:' + loadFlag);
        $('#frameData').text('dispNo:' + gFrameObject._dispNo + ', reloadFlg:' + gFrameObject.reloadFlg + ', refreshCounter:' + gRefreshCounter);
    }

    var drawFlag = true;

    // スクロールバーが存在する場合は描画処理を行わない
    if ($('div.scroll-bar:visible').length > 0) {
        drawFlag = false;

        // ロックカウンターをリセットする
        lockCounterReset();
    }

    // 商談中詳細画面のカウンター処理
    if ($('div#StaffDetailPopOver:visible').length > 0) {
        StaffDetailPopOverCounter(drawFlag, true);
    }

    // $02 start 複数顧客に対する商談平行対応
    /*
    // 紐付け解除画面のカウンター処理
    if ($('div#LinkingCancelPopOver:visible').length > 0) {
        LinkingCancelPopOverCounter(drawFlag, true);
    }
    */
    // $02 end   複数顧客に対する商談平行対応
    // $04 start TKM Change request development for Next Gen e-CRB (CR075)
    //定期リフレッシュ間隔が0秒の場合、定期リフレッシュを行わない
    if (gRefreshInterval != 0) {

        gRefreshCounter++;

        if (gRefreshInterval <= gRefreshCounter && !gFrameObject.reloadFlg) {
            reloadRequest();
        }
    }
    // $04 end TKM Change request development for Next Gen e-CRB (CR075)
}

/**
* ロック処理を行う.
*/
function lock() {
    gLockFlag = true;
    gLockCounter = 0;
}

/**
* ロックカウンターリセット処理.
*/
function lockCounterReset() {
    gLockCounter = 0;
}

/**
* ロック解除処理を行う.
*/
function lockReset() {

    // ロックカウンターをリセットする
    lockCounterReset();

    // グレーアウト状態である場合は解除しない（グレーアウト中のポップオーバーを閉じた場合の考慮）
    // グレーアウト状態でもロックを解除したい場合は必ずこのフラグを落とすようにすること
    if (gGrayOutFlg) {
        return;
    }

    // リロードする必要がある場合のみリロードする
    if (gFrameObject.reloadFlg) {
        gFrameObject.reloadRequest();
    }

    gLockFlag = false;
    gLockCounter = 0;

}

// $04 start TKM Change request development for Next Gen e-CRB (CR075)
/**
* 定期リフレッシュカウンターリセット処理.
*/
function refreshCounterReset() {
    gRefreshCounter = 0;
}
// $04 end TKM Change request development for Next Gen e-CRB (CR075)

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
    var con = $('div#frameArea');

    // 子エリア選択時
    activeFrameJQuery.contents().find('div#bodyFrame').bind(C_TOUCH_START, function (aEvent) {

        var tagName = (aEvent.target.tagName).toUpperCase();
        if (tagName == "INPUT") {

            return;
        }

        // フレーム内のハブリングが行われないため手動でハブリング設定しておく
        con.trigger(C_TOUCH_START);

    });

    // ロックカウンターリセット処理
    lockCounterReset();

    // $04 start TKM Change request development for Next Gen e-CRB (CR075)
    //定期リフレッシュカウンターリセット処理
    refreshCounterReset();
    // $04 end TKM Change request development for Next Gen e-CRB (CR075)

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
* 透明なオーバーレイを表示する.
*/
function showOverlayTransparency() {
    var activeDom = gFrameObject.getActiveDom();
    activeDom.showOverlayTransparency();
}

/**
* オーバーレイを非表示にする.
*/
function hideOverlay() {
    var activeDom = gFrameObject.getActiveDom();
    activeDom.hideOverlay();
}

/**
* 読み込み中エリアを表示する.
*/
function showLodingWindow() {

    var overlay = $('div.MstPG_LoadingScreen:eq(0)');
    overlay.css({ width: $(window).width() + 'px', height: $(window).height() + 'px' });

    overlay.css({ display: 'table' });

    // $01 start クルクル対応
    //タイマーの二重セットを防ぐため事前チェック
    if (gTimerFlag == false) {
        gTimerFlag = true;
        if (gDispInitFlag) {
            // タイマーセット
            commonRefreshTimer(
                function () {
                    window.location.reload();
                }
            );
        }
        else {
            //タイマーセット
            commonRefreshTimer(
             function () {
                    var activeDom = gFrameObject.getActiveDom();
                    //リロード処理
                    activeDom.hideOverlay();
                    reloadForce();
                    activeDom.hidePopOver();
                }
            );
        }
    }
    // $01 end   クルクル対応
}

/**
* 読み込み中エリアを表示する（メインエリア）.
*/
function showLodingWindowMainArea() {

    //var container = $('div#frameArea');
    //overlay = container.find('div.MstPG_LoadingScreen:eq(0)');
    //overlay.css({
    //    width: container.width() + 'px',
    //    height: container.height() + 'px',
    //    top: container.offset().top + 'px',
    //    left: container.offset().left + 'px'
    //});

    //overlay.css({ 'display': 'table' });

    showLodingWindow();
}

/**
* 読み込み中エリアを非表示にする.
*/
function closeLodingWindow() {
    $("div.MstPG_LoadingScreen:visible").css({ display: 'none' });

    // $01 start クルクル対応
    //タイマー解除
    commonClearTimer();
    gTimerFlag = false;
    // $01 end   クルクル対応
}

/**
* カレンダーアプリ起動
*/
function displayCale() {
    var ymd = { year: "", month: "", day: "" };
    ymd.year = $("input#NowDateString").val().substr(0, 4);
    ymd.month = $("input#NowDateString").val().substr(4, 2);
    ymd.day = $("input#NowDateString").val().substr(6, 2);
    window.location = "icrop:cale:::" + ymd.year + "-" + ymd.month + "-" + ymd.day;
    return false;
}

/**
* 電話帳アプリ起動
*/
function displayCont() {
    window.location = "icrop:cont:";
    return false;
}

/**
* エリア更新を行う（PUSH機能にて実行される前提）.
*/
function SC3100101Update(aFunctionNo, aLogicNo) {

    // 処理停止中の場合は処理しない
    if (gLogicStopFlag) {
        return;
    }

    // 警告音設定
    // 操作権限による警告音出力フラグが立っている場合のみ処理を行う
    if (gMainAlertFlg) {
        // ゲートキーパー 来店通知 来店件数の増加
        if (aFunctionNo == '01' && aLogicNo == '01') {
            gPushAlertFlg = true;
        }
        // $02 start 新車タブレットショールーム管理機能開発
        // 商談開始時　
        else if (aFunctionNo == '03' && aLogicNo == '01') {
            gPushAlertFlg = true;
        }
        // $02 end   新車タブレットショールーム管理機能開発
        // 顧客情報画面 査定の送信時（できれば商談中スタッフ） 査定件数の増加
        else if (aFunctionNo == '03' && aLogicNo == '03') {
            gPushAlertFlg = true;
        }
        // 顧客情報画面 価格相談の送信時（できれば商談中スタッフ） 価格相談件数の増加
        else if (aFunctionNo == '03' && aLogicNo == '05') {
            gPushAlertFlg = true;
        }
        // 顧客情報画面 ヘルプの送信時（できれば商談中スタッフ） ヘルプ件数の増加
        else if (aFunctionNo == '03' && aLogicNo == '07') {
            gPushAlertFlg = true;
        }
    }

    // リロード要求
    reloadRequest();
}

/**
* 警告音出力処理を行う（初期表示処理）.
*/
function alertOutputInitialDisplay() {

    // 処理停止中の場合は処理しない
    if (gLogicStopFlag) {
        return;
    }

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

    // デバッグ用
    if (gDebugObject.debugFlag) {
        $('span#BeforeAlertCount').text(beforeAlertChipCount);
        $('span#AfterAlertCount').text(nowAlertChipCount);
    }
    
    // プッシュ送信による警告音出力フラグのチェック
    if (gPushAlertFlg) {

        gPushAlertFlg = false;

        // 警告音出力処理を行う
        activeFrameDom.alertOutput('1');
    }

    // 警告チップの数が増加した場合
    if (beforeAlertChipCount < nowAlertChipCount) {

        // 警告音出力処理を行う
        activeFrameDom.alertOutput('2');
    }

}

/**
* 警告音停止処理を行う.(廃止)
*/
//function stopAlertOutput() {

//    if (gAlertOutputNoList.length == 0) {
//        return;
//    }

//    /*** TODO ここに警告音を止める処理を組み込む想定 ***/

//    if (gAlertOutputNoList.length == 2) {
//        //alert('プッシュ受信による警告音停止＋警告チップ増加による警告音停止');
//    }
//    else {

//        $.each(gAlertOutputNoList, function (aIndex, aValue) {

//            if (aValue == '1') {
//                //alert('プッシュ受信による警告音停止');
//            }
//            else {
//                //alert('警告チップ増加による警告音停止');
//            }

//        });
//    }

//    gAlertOutputNoList = [];
//}
