/** 
* @fileOverview 受付メイン（サブエリア）の処理を記述するファイル.
* 
* @author t.mizumoto
* @version 1.0.0
* 更新： 2012/08/27 TMEJ m.okamura 新車受付機能改善 $01
* 更新： 2013/01/16 TMEJ m.asano 新車タブレットショールーム管理機能開発 $02
* 更新： 2013/02/27 TMEJ t.shimamura 新車受付画面管理指標変更対応 $03
* 更新： 2018/07/12 NSK a.kani TKM Next Gen e-CRB Project Application development Block B-1  Pマーク、Lマークなどを表示
*/
// ==============================================================
// 定数
// ==============================================================
// フェードインのミリ秒
var C_FADE_IN_CUSTOMER_DELETE_BUTTON = 100; // 100
var C_FADE_IN_OVERLAY = 0;       // フェードインなし
var C_FADE_IN_CHIP_GRAY_OUT = 0; // フェードインなし

// フェードアウトのミリ秒
var C_FADE_OUT_CUSTOMER_DELETE_BUTTON = 100; // 100
var C_FADE_OUT_OVERLAY = 0;       // フェードインなし
var C_FADE_OUT_CHIP_GRAY_OUT = 0; // フェードインなし

// スタッフの経過時間の最大文字列長
var C_MAX_LENGTH_STAFF_SPAN_TIME = 8;
// 顧客の経過時間の最大文字列長
var C_MAX_LENGTH_CUSTOMER_SPAN_TIME = 6;

// 更新権限
var C_UPDATE_OPERATION = 1; // 1:更新、2:読取専用

// スタッフステータス（スタンバイ）
var C_STAFF_STATUS_STANDBY = '1';
// スタッフステータス（商談中）
var C_STAFF_STATUS_NEGO = '2';
// スタッフステータス（退席中）
var C_STAFF_STATUS_REAVE = '3';
// スタッフステータス（オフライン）
var C_STAFF_STATUS_OFFLINE = '4';

// $02 start 新車タブレットショールーム管理機能開発
// 来店実績ステータス（フリー）
var C_VISIT_STATUS_FREE = '01';
// 来店実績ステータス（待ち）
var C_STAFF_STATUS_WAIT = '06';
// 来店実績ステータス（商談中断）
var C_STAFF_STATUS_STOP = '09';
// $02 end   新車タブレットショールーム管理機能開発

// カウンター解除用変数
var counterInterval = null;
var counterFlashingInterval = null;


// ==============================================================
// 変数
// ==============================================================
// $02 start 新車タブレットショールーム管理機能開発
var gWaitAssginedTimeSpanList = [];
var gWaitServiceTimeSpanList = [];
var gNegotiationTimeSpanList = [];
// $02 end   新車タブレットショールーム管理機能開発

var gRequestAssessmentTimeSpanList = [];
var gRequestPriceConsultationTimeSpanList = [];
var gRequestHelpTimeSpanList = [];

// ==============================================================
// DOMロード時処理
// ==============================================================
$(function () {

    // エリア定義定義を行う
    initArea();

    // $02 start 新車タブレットショールーム管理機能開発
    // スタッフチップの定義を行う。
    initStaffChip();

    // 接客待ちチップの定義を行う。
    initReceptionChip();

    // 接客中チップの定義を行う。
    initNegotiationChip();
    // $02 end   新車タブレットショールーム管理機能開発

    // ポップオーバー定義を行う
    initPopOver();

    // カウンター定義を行う
    initCounter();

    /**
    * エリア定義定義を行う.
    */
    function initArea() {

        // エリア選択時
        $('div#bodyFrame').bind(C_TOUCH_START, function (aEvent) {

            // 削除ボタン表示中の場合
            if ($('div.ComingStoreOffDeletionButton:visible').length > 0) {

                // 削除ボタンを除く
                if ($(aEvent.target).hasClass('ComingStoreOffDeletionButton') || $(aEvent.target).parents('div.ComingStoreOffDeletionButton').length > 0) {
                    return;
                }

                // チップに対するイベント処理を無効にする（ロック解除後にタップイベントなどによって再びロックがかけられることを防ぐ）
                $.fn.setChipEvent.cancelEventFlag = true;

                hideDeleteButton()
                parent.lockReset();

                return;
            }

            // フリックエリアを除く
            if ($(aEvent.target).parents('div.scroll-inner').length > 0) {
                return;
            }

            parent.lockReset();
        });

        // $02 start 新車タブレットショールーム管理機能開発
        // フリックエリアの高さ調整
        var addHeight = 0;
        if ($('div#staffFlickAreaInner').find('li').length > 15) {
            addHeight = 5;
        }
        $('div#staffFlickAreaInner').height($('div#staffFlickAreaInner').find('ul').attr('scrollHeight') + addHeight);
        $('div#CassetteBox03FlickAreaInner').height($('div#CassetteBox03FlickArea').attr('scrollHeight'));
        // 幅の補正
        $('div#staffFlickArea').width(990);

        // フリック設定
        // （FingerScrollを行うと内部の個々の要素のz-indexが効かなくなるため全体のz-indexを設定しておく）
        $('div#staffFlickArea, div#CassetteBox01FlickArea, div#CassetteBox02FlickArea, div#CassetteBox03FlickArea').fingerScroll();

        // フリックエリア 共通イベント定義
        $('div#staffFlickArea, div#CassetteBox01FlickArea, div#CassetteBox02FlickArea, div#CassetteBox03FlickArea').setCommonEvent();
        // $02 end   新車タブレットショールーム管理機能開発

        // z-indexについて、以下の順で定義する
        // （フリックエリアはハイライト時にz-indexが25となり、
        //       内部の要素はフリックエリア内でしか上下しない。）
        // ・ポップオーバーエリア
        // ・読み込み中エリア                            z-index:10010
        // ・オーバーレイ（透明）                        z-index:30
        // ・フリックエリア内（ハイライトチップ）        z-index:15.25
        // ・フリックエリア内（苦情、テーブルNoグレー）  z-index:15.20
        // ・フリックエリア内（苦情、テーブルNo）        z-index:15.15
        // ・フリックエリア内（スタッフチップグレー）    z-index:15.10
        // ・フリックエリア内（オフラインカバー）        z-index:15.5
        // ・フリックエリア                              z-index:15

        // オーバーレイ（透明）
        var mainAreaOverlayTransparency = $("<div id='MainAreaOverlayTransparency'/>").css({
            position: 'absolute', top: '0px', left: '0px',
            width: $('body').outerWidth() > $(window).width() ? $('body').outerWidth() : $(window).width(),
            height: $('body').outerHeight() > $(window).height() ? $('body').outerHeight() : $(window).height(),
            background: 'rgba(0, 0, 0, 0.0)', zIndex: '30', display: 'none'
        });

        $('div#bodyFrame').append(mainAreaOverlayTransparency);

        // 透明なオーバーレイ押下時にポップオーバー画面を解除
        mainAreaOverlayTransparency.bind(C_TOUCH_START, function () {
            hidePopOver();
            parent.hidePopOver();
        });
    }

    // $02 start 新車タブレットショールーム管理機能開発
    /**
    * スタッフチップ定義を行う.
    */
    function initStaffChip() {

        /* 更新権限がある場合のみ実装 */
        if ($('input#OperationStatus', $(parent.document)).val() != C_UPDATE_OPERATION) {
            return;
        }

        // スタッフチップ
        var staffChip = $('li.StaffChip');

        // 独自イベント設定
        staffChip.setChipEvent({ holdTimeInterval: 300 });

        // ホールド
        staffChip.bind('chipHold', function (aEvent, options) {

            // スタッフステータスがオフライン以外の場合に処理を行う。
            if ($(this).find('input#Status').val() == C_STAFF_STATUS_OFFLINE) {
                return;
            }

            options.target.addClass('selectArea').css({ opacity: 0.2 });

            // ロック開始
            parent.lock();

            var scrollArea = $('div#staffFlickArea');
            
            // ドラッグアンドドロップ
            options.target.dragAndDrop({
                event: options.event,
                zIndex: 40,
                dropOptionList: [
                            {
                                dropElement: $('div.ReceptionChip'),
                                dropHoverClass: 'BigMainListBox',
                                dropArea: $('div#CassetteBox01FlickArea, div#CassetteBox02FlickArea, div#CassetteBox03FlickArea')
                            }
                        ],
                offsetX: $('div#frameArea', $(parent.document)).offset().left,
                offsetY: $('div#frameArea', $(parent.document)).offset().top,
                dragStart: function (aEvent) {
                    parent.gDragFlg = true;
                    scrollArea.fingerScroll({ action: "stop" });
                },
                dragEnd: function (aEvent, targetChip, dropChip) {

                    parent.gDragFlg = false;

                    if (dropChip != null) {

                        var visitSeq;
                        if (dropChip.parents('div#CassetteBox01FlickArea').length > 0) {
                            visitSeq = dropChip.parents('.CassetteBack').find('input#WaitAssginedVisitSeq').val();
                        }
                        else if (dropChip.parents('div#CassetteBox02FlickArea').length > 0) {
                            visitSeq = dropChip.parents('.CassetteBack').find('input#WaitServiceVisitSeq').val();
                        }
                        else {
                            visitSeq = dropChip.parents('.CassetteBack').find('input#NegotiationVisitSeq').val();
                        }

                        //対象のアカウントの取得
                        var account = targetChip.find('input#Account').val();

                        // 来店実績連番又はアカウントが取得できなかった場合は処理しない
                        if (visitSeq == undefined || account == undefined) {
                            return;
                        }

                        // 二度押し防止
                        if (C_UPDATE_FLAG_ON != dropChip.data(C_UPDATE_FLAG_NAME)) {

                            parent.showLodingWindowMainArea();

                            dropChip.data(C_UPDATE_FLAG_NAME, C_UPDATE_FLAG_ON);

                            // デバッグ用
                            if (parent.gDebugObject.debugFlag) {
                                var pageMethodsCount = $('span#PageMethodsCount', $(parent.document)).text() - 0 + 1;
                                $('span#PageMethodsCount', $(parent.document)).text(pageMethodsCount)
                                $('span#PageMethodsFlag', $(parent.document)).show();
                            }

                            // 紐付け処理を行う
                            parent.visitorChipSend(visitSeq, account);
                        }

                    }
                    else {
                        /* ロック解除 */
                        parent.lockReset();
                    }

                    options.target.removeClass('selectArea').css({ opacity: 1 });
                    scrollArea.fingerScroll({ action: "restart" });
                }
            });
        });
    }

    /**
    * 接客待ちチップ定義を行う.
    */
    function initReceptionChip() {

        /* 更新権限がある場合のみ実装 */
        if ($('input#OperationStatus', $(parent.document)).val() != C_UPDATE_OPERATION) {
            return;
        }

        // 顧客チップ
        // $02 start 新車タブレットショールーム管理機能開発
        var customerChip = $('div.ReceptionChip');
        // $02 end   新車タブレットショールーム管理機能開発

        /*2018/07/12 NSK a.kani TKM Next Gen e-CRB Project Application development Block B-1  Pマーク、Lマークなどを表示 START*/
        //Lアイコンを表示したときは、顧客名の幅を狭くする
        customerChip.each(function (index) {
            var chip = $('div.ReceptionChip:eq(' + index + ')')
            if (chip.find('.LIcon').length) {
                chip.find('.Name').width(66);
            }
        });
        /*2018/07/12 NSK a.kani TKM Next Gen e-CRB Project Application development Block B-1  Pマーク、Lマークなどを表示 END*/

        // 独自イベント設定
        customerChip.setChipEvent();

        // タップ
        customerChip.bind('chipTap', function (aEvent, aTarget) {

            // 削除ボタン押下時は処理対象としない
            if ($(aTarget).hasClass('ComingStoreOffDeletionButton') || $(aTarget).parents('div.ComingStoreOffDeletionButton').length > 0) {
                return;
            }

            // 削除ボタンを非表示にする
            hideDeleteButton();

            // ロック開始
            parent.lock();

            showLodingWindow($('div#CustomerPopOverBody', $(parent.document)));

            // お客様情報入力画面の削除
            parent.DeleteCustomerPopOver();

            if ($(this).parents('div#CassetteBox01FlickArea').length > 0) {
                $('input#CustomerDialogVisitSeq', $(parent.document)).val($(this).parents('.CassetteBack').find('input#WaitAssginedVisitSeq').val());
                $('input#CustomerDialogVisitStatus', $(parent.document)).val($(this).parents('.CassetteBack').find('input#WaitAssginedVisitStatus').val());
            }
            else if ($(this).parents('div#CassetteBox02FlickArea').length > 0) {
                $('input#CustomerDialogVisitSeq', $(parent.document)).val($(this).parents('.CassetteBack').find('input#WaitServiceVisitSeq').val());
                $('input#CustomerDialogVisitStatus', $(parent.document)).val($(this).parents('.CassetteBack').find('input#WaitServiceVisitStatus').val());
            }
            else {
                $('input#CustomerDialogVisitSeq', $(parent.document)).val($(this).parents('.CassetteBack').find('input#NegotiationVisitSeq').val());
                $('input#CustomerDialogVisitStatus', $(parent.document)).val($(this).parents('.CassetteBack').find('input#NegotiationVisitStatus').val());
            }

            $('input#CustomerDialogDisplayButton', $(parent.document)).click();

            $(this).trigger('showPopover');
        });

        // スワイプ
        customerChip.bind('chipSwipe', function () {

            // ドラッグ中は処理しない
            if ($.fn.dragAndDrop.dragHelper) {

                return;
            }

            var deleteButton = $(this).parents('.CassetteBack').find('div.ComingStoreOffDeletionButton');

            if (deleteButton.css('display') == 'none') {
                deleteButton.fadeIn(C_FADE_IN_CUSTOMER_DELETE_BUTTON);
                parent.lock();
            }
            else {
                hideDeleteButton()
                parent.lockReset();
            }

        });

        // 削除ボタン
        var deleteButton = $('div.ComingStoreOffDeletionButton');

        // 共通イベント定義
        deleteButton.setCommonEvent();

        // タップ
        deleteButton.bind('tap', function (aEvent, aTarget) {

            //イベントパブリングを抑制
            aEvent.stopPropagation();
            // 来店実績連番の取得
            var visitSeq = '';
            if ($(this).parents('div#CassetteBox01FlickArea').length > 0) {
                visitSeq = $(this).parents('.CassetteBack').find('input#WaitAssginedVisitSeq').val();
            }
            else if ($(this).parents('div#CassetteBox02FlickArea').length > 0) {
                visitSeq = $(this).parents('.CassetteBack').find('input#WaitServiceVisitSeq').val();
            }
            else {
                visitSeq = $(this).parents('.CassetteBack').find('input#NegotiationVisitSeq').val();
            }

            // 来店実績連番が取得できなかった場合は処理しない
            if (visitSeq == undefined) {
                return;
            }

            // 二度押し防止
            if (C_UPDATE_FLAG_ON != deleteButton.data(C_UPDATE_FLAG_NAME)) {

                parent.showLodingWindowMainArea();

                deleteButton.data(C_UPDATE_FLAG_NAME, C_UPDATE_FLAG_ON);

                // デバッグ用
                if (parent.gDebugObject.debugFlag) {
                    var pageMethodsCount = $('span#PageMethodsCount', $(parent.document)).text() - 0 + 1;
                    $('span#PageMethodsCount', $(parent.document)).text(pageMethodsCount)
                    $('span#PageMethodsFlag', $(parent.document)).show();
                }

                // 顧客チップの削除処理を行う
                parent.visitorChipDelete(visitSeq);
            }
        });
    }

    /**
    * 接客中チップ定義を行う.
    */
    function initNegotiationChip() {

        // 商談・納車作業中チップ
        var negotiationChip = $('div.NegotiationChip');

        /*2018/07/12 NSK a.kani TKM Next Gen e-CRB Project Application development Block B-1  Pマーク、Lマークなどを表示 START*/
        //Lアイコンを表示したときは、顧客名の幅を狭くする
        negotiationChip.each(function (index) {
            var chip = $('div.NegotiationChip:eq(' + index + ')')
            if (chip.find('.LIcon').length) {
                chip.find('.Name').width(66);
            }
        });
        /*2018/07/12 NSK a.kani TKM Next Gen e-CRB Project Application development Block B-1  Pマーク、Lマークなどを表示 END*/
        // 独自イベント設定
        negotiationChip.setChipEvent();

        // タップ
        negotiationChip.bind('chipTap', function () {

            // 画面ロック
            parent.lock();

            // タイトル初期化
            $('h3#StaffName', $(parent.document)).css({ 'display': 'none' });
            $('h3#TableNo', $(parent.document)).css({ 'display': 'none' });
            $('a#scNscPopUpStaffDetailCompleteButton', $(parent.document)).css({ display: 'block' });
            $('div#scNscPopUpStaffDetailCustomerButton', $(parent.document)).css({ display: 'none' });
            $('a#scNscPopUpStaffDetailCancelButton', $(parent.document)).css({ display: 'block' });

            // 権限別表示(読取専用時は非活性化)
            if ($('input#OperationStatus', $(parent.document)).val() != C_UPDATE_OPERATION) {
                $('a#scNscPopUpStaffDetailCompleteButton', $(parent.document)).attr('class', 'scNscPopUpStaffDetailCompleteButtonOff');
            }

            showLodingWindow($('div#StaffDetailPopOverBody', $(parent.document)));

            //商談中詳細画面の削除
            parent.DeleteStaffDetailPopOver();

            //商談中詳細に必要なパラメータを設定
            $('input#StaffDetailDialogVisitSeq', $(parent.document)).val($(this).parents('.CassetteBack').find('input#NegotiationVisitSeq').val());
            // $03 start 納車作業ステータス対応
            $('input#StaffDetailDialogVisitStatus', $(parent.document)).val($(this).parents('.CassetteBack').find('input#NegotiationVisitStatus').val());
            // $03 end 納車作業ステータス対応

            var index = 0;
            var visitSeq = $(this).parents('.CassetteBack').find('input#NegotiationVisitSeq').val();

            //インデックス番号
            negotiationChip.each(function (aIndex, aValue) {

                //通信依頼種別事の信号を出力
                if ($(this).parents('.CassetteBack').find('input#NegotiationVisitSeq').length > 0 &&
                    $(this).parents('.CassetteBack').find('input#NegotiationVisitSeq').val() == visitSeq) {
                    index = aIndex;
                }

            });

            $('input#StaffDetailDialogIndex', $(parent.document)).val(index);

            $('input#StaffDetailDisplayButton', $(parent.document)).click();

            $(this).trigger('showPopover');

        });
    }
    // $02 end   新車タブレットショールーム管理機能開発

    /**
    * ポップオーバー定義を行う.
  　 */
    function initPopOver() {

        /* 更新権限がある場合のみ実装 */
        if ($('input#OperationStatus', $(parent.document)).val() == C_UPDATE_OPERATION) {

            // お客様情報-ポップオーバー（商談中のスタッフチップと顧客チップが対象）
            $('div.ReceptionChip').popoverEx({

                contentId: $('div#CustomerPopOver', $(parent.document))
                , openEvent: function () {

                    parent.popupOpen();

                    // 透明なオーバーレイを設定する
                    showOverlayTransparency();
                }
                , closeEvent: function () {

                    parent.DeleteCustomerPopOver();

                    parent.popupClose();

                    // 透明なオーバーレイを削除する
                    hideOverlayTransparency();

                }
                , offsetX: $('div#frameArea', $(parent.document)).offset().left
                , offsetY: $('div#frameArea', $(parent.document)).offset().top
            });
        }

        // 商談中詳細-ポップオーバー（商談中のスタッフチップが対象）
        $('div.NegotiationChip').popoverEx({
            contentId: $('div#StaffDetailPopOver', $(parent.document))
            , openEvent: function () {

                parent.popupOpen();

                // オーバーレイを設定する
                showOverlayTransparency();
            }
            , closeEvent: function () {

                // キーボードを非表示にするため領域を削除する（入力エリアのフォーカスを外す）
                parent.DeleteStaffDetailPopOver();

                parent.popupClose();

                // 透明なオーバーレイを削除する
                hideOverlayTransparency();

            }
            , offsetX: $('div#frameArea', $(parent.document)).offset().left
            , offsetY: $('div#frameArea', $(parent.document)).offset().top
        });

    }

    /**
    * カウンター定義を行う.
    */
    function initCounter() {

        // $02 start 新車タブレットショールーム管理機能開発
        // 各エリアの日時を取得を取得
        if ($('input#WaitAssginedTimeList').val().length > 0) {
            gWaitAssginedTimeSpanList = $.evalJSON($('input#WaitAssginedTimeList').val());
        }
        if ($('input#WaitServiceTimeList').val().length > 0) {
            gWaitServiceTimeSpanList = $.evalJSON($('input#WaitServiceTimeList').val());
        }
        if ($('input#NegotiationTimeList').val().length > 0) {
            gNegotiationTimeSpanList = $.evalJSON($('input#NegotiationTimeList').val());
        }
        // $02 end   新車タブレットショールーム管理機能開発

        // 通知依頼（査定）
        if ($('input#RequestAssessmentTimeDateList').val().length > 0) {
            gRequestAssessmentTimeSpanList = $.evalJSON($('input#RequestAssessmentTimeDateList').val());
        }
        // 通知依頼（価格相談）
        if ($('input#RequestPriceConsultationTimeDateList').val().length > 0) {
            gRequestPriceConsultationTimeSpanList = $.evalJSON($('input#RequestPriceConsultationTimeDateList').val());
        }
        // 通知依頼（ヘルプ）
        if ($('input#RequestHelpTimeDateList').val().length > 0) {
            gRequestHelpTimeSpanList = $.evalJSON($('input#RequestHelpTimeDateList').val());
        }

        counter(true);

        // 処理停止中の場合は処理しない
        if (parent.gLogicStopFlag) {
            return;
        }

        counterInterval = setInterval(counter, 1000);
        counterFlashingInterval = setInterval(counterFlashing, 500);

    }

});

// ==============================================================
// 関数
// ==============================================================
/**
* カウンターを解除する.
*/
function resetCounter() {

    // チップに対するイベント処理を無効にする（画面切り替え後にホールドイベントが発生してしまうことを防ぐ）
    $.fn.setChipEvent.cancelEventFlag = true;

    clearTimeout(counterInterval);
    clearTimeout(counterFlashingInterval);
}

/**
* 非同期メソッド実行時のエラー処理を行う.
*/
function onFailedPageMethods(error) {
    parent.onFailedPageMethods(error);
}

var drawFlag = true;

/**
* 画像点滅のカウンター処理を行う.
*/
function counterFlashing() {

    if (drawFlag) {
        // 画像の点滅
        for (index = 0; index < $('img.imageFlashing').size(); index++) {

            var visibility = $('img.imageFlashing:eq(' + index + ')').css('visibility');
            if (visibility == 'hidden') {

                $('img.imageFlashing:eq(' + index + ')').css('visibility', 'visible');

            } else {

                $('img.imageFlashing:eq(' + index + ')').css('visibility', 'hidden');
            }
        }
    }
}

/**
* カウンター処理を行う.
*/
function counter(aFirstFlag) {

    // スクロールバーが存在する場合は、描画を行わず、ロックカウンターをリセットする
    // 親のポップオーバーのスクロールバーも対象とする
    if ($('div.scroll-bar:visible').length > 0 || $('div.scroll-bar:visible', $(parent.document)).length > 0) {
        drawFlag = false;

        // ロックカウンターをリセットする
        parent.lockCounterReset();
    }
    // ドラッグ中、又はポップオーバー表示中は描画を行わない
    else if ($.fn.dragAndDrop.dragHelper || $.fn.popoverEx.openedPopup) {
        drawFlag = false;
    }
    else {

        drawFlag = true;
    }

    // $02 start 新車タブレットショールーム管理機能開発
    // 振当て待ち
    $.each(gWaitAssginedTimeSpanList, function (aIndex, aValue) {
        if (0 < aValue.length) {
            if (drawFlag) {

                // 来店実績ステータスの取得
                var status = $('div#CassetteBox01FlickArea').find('input#WaitAssginedVisitStatus:eq(' + aIndex + ')').val();
                var classVal = '';
                if (status == C_VISIT_STATUS_FREE) {
                    // 01：フリーの場合
                    // 遅れ判定
                    if (0 <= parent.gVisitTimeAlertSpan && parent.gVisitTimeAlertSpan < (aValue - 0)) {
                        classVal = 'BackColor_Red';
                    }
                }
                else {
                    // 10：接客不要の場合
                    // 遅れ判定
                    if (0 <= parent.gUnNecessaryFirstTimeAlertSpan && (parent.gUnNecessaryFirstTimeAlertSpan + parent.gUnNecessarySecondTimeAlertSpan) < (aValue - 0)) {
                        classVal = 'BackColor_Red';
                        //$('div#CassetteBox01FlickArea').find('div.CassetteBack:eq(' + aIndex + ')').removeClass('BackColor_Yellow');
                    }
                    else if (0 <= parent.gUnNecessaryFirstTimeAlertSpan && parent.gUnNecessaryFirstTimeAlertSpan < (aValue - 0)) {
                        classVal = 'BackColor_Yellow';
                    }
                }

                if (classVal != '') {
                    if (!$('div#CassetteBox01FlickArea').find('div.CassetteBack:eq(' + aIndex + ')').hasClass(classVal)) {
                        $('div#CassetteBox01FlickArea').find('div.CassetteBack:eq(' + aIndex + ')').addClass(classVal);
                    }
                }
                $('div#CassetteBox01FlickArea').find('div.Time:eq(' + aIndex + ')').text(getDispTime(aValue, C_MAX_LENGTH_STAFF_SPAN_TIME));
            }
            gWaitAssginedTimeSpanList[aIndex] = (aValue - 0) + 1 + '';
        }
    });

    // 接客待ち
    $.each(gWaitServiceTimeSpanList, function (aIndex, aValue) {
        if (0 < aValue.length) {
            if (drawFlag) {
                var status = $('div#CassetteBox02FlickArea').find('input#WaitServiceVisitStatus:eq(' + aIndex + ')').val();
                var classVal = '';
                if (status == C_STAFF_STATUS_WAIT) {
                    // 06：待ちの場合
                    // 遅れ判定
                    if (0 <= parent.gWaitTimeAlertSpan && parent.gWaitTimeAlertSpan < (aValue - 0)) {
                        classVal = 'BackColor_Red';
                    }
                }
                else {
                    // 06：待ち以外の場合
                    if (0 <= parent.gVisitTimeAlertSpan && parent.gVisitTimeAlertSpan < (aValue - 0)) {
                        classVal = 'BackColor_Red';
                    }
                }
                if (classVal != '') {
                    if (!$('div#CassetteBox02FlickArea').find('div.CassetteBack:eq(' + aIndex + ')').hasClass(classVal)) {
                        $('div#CassetteBox02FlickArea').find('div.CassetteBack:eq(' + aIndex + ')').addClass(classVal);
                    }
                }
                $('div#CassetteBox02FlickArea').find('div.Time:eq(' + aIndex + ')').text(getDispTime(aValue, C_MAX_LENGTH_STAFF_SPAN_TIME));
            }
            gWaitServiceTimeSpanList[aIndex] = (aValue - 0) + 1 + '';
        }
    });

    // 接客中
    $.each(gNegotiationTimeSpanList, function (aIndex, aValue) {
        if (0 < aValue.length) {
            if (drawFlag) {
                var status = $('div#CassetteBox03FlickArea').find('input#NegotiationVisitStatus:eq(' + aIndex + ')').val();
                if (status == C_STAFF_STATUS_STOP) {
                    // 09：商談中断の場合
                    // 遅れ判定
                    if (0 <= parent.gStopTimeAlertSpan && parent.gStopTimeAlertSpan < (aValue - 0)) {
                        if (!$('div#CassetteBox03FlickArea').find('div.CassetteBack:eq(' + aIndex + ')').hasClass('BackColor_Red')) {
                            $('div#CassetteBox03FlickArea').find('div.CassetteBack:eq(' + aIndex + ')').addClass('BackColor_Red');
                        }
                    }
                }
                $('div#CassetteBox03FlickArea').find('div.Time:eq(' + aIndex + ')').text(getDispTime(aValue, C_MAX_LENGTH_STAFF_SPAN_TIME));
            }
            gNegotiationTimeSpanList[aIndex] = (aValue - 0) + 1 + '';
        }
    });
    // $02 end   新車タブレットショールーム管理機能開発

    // 査定依頼通知待ち時間
    $.each(gRequestAssessmentTimeSpanList, function (aIndex, aValue) {
        if (0 < aValue.length) {

            if (drawFlag) {
                if (0 <= parent.gAssessmentTimeAlertSpan && parent.gAssessmentTimeAlertSpan < (aValue - 0)) {
                    // $02 start 新車タブレットショールーム管理機能開発
                    // スタッフチップの判定
                    if (!$('div#CassetteBox03FlickArea').find('div.CassetteBack:eq(' + aIndex + ')').find('li#AssessmentRequest').hasClass('Color_Red')) {
                        $('div#CassetteBox03FlickArea').find('div.CassetteBack:eq(' + aIndex + ')').find('li#AssessmentRequest').addClass('Color_Red');
                    }
                    // $02 end   新車タブレットショールーム管理機能開発
                }
            }
            gRequestAssessmentTimeSpanList[aIndex] = (aValue - 0) + 1 + '';
        }
    });

    // 価格相談依頼通知待ち時間
    $.each(gRequestPriceConsultationTimeSpanList, function (aIndex, aValue) {
        if (0 < aValue.length) {

            if (drawFlag) {

                if (0 <= parent.gPriceTimeAlertSpan && parent.gPriceTimeAlertSpan < (aValue - 0)) {
                    // $02 start 新車タブレットショールーム管理機能開発
                    // スタッフチップの判定
                    if (!$('div#CassetteBox03FlickArea').find('div.CassetteBack:eq(' + aIndex + ')').find('li#PriceRequest').hasClass('Color_Red')) {
                        $('div#CassetteBox03FlickArea').find('div.CassetteBack:eq(' + aIndex + ')').find('li#PriceRequest').addClass('Color_Red');
                    }
                    // $02 end   新車タブレットショールーム管理機能開発
                }
            }
            gRequestPriceConsultationTimeSpanList[aIndex] = (aValue - 0) + 1 + '';
        }
    });

    // ヘルプ依頼通知待ち時間
    $.each(gRequestHelpTimeSpanList, function (aIndex, aValue) {
        if (0 < aValue.length) {
            if (drawFlag) {

                if (0 <= parent.gHelpTimeAlertSpan && parent.gHelpTimeAlertSpan < (aValue - 0)) {
                    // $02 start 新車タブレットショールーム管理機能開発
                    // スタッフチップの判定
                    if (!$('div#CassetteBox03FlickArea').find('div.CassetteBack:eq(' + aIndex + ')').find('li#HelpRequest').hasClass('Color_Red')) {
                        $('div#CassetteBox03FlickArea').find('div.CassetteBack:eq(' + aIndex + ')').find('li#HelpRequest').addClass('Color_Red');
                    }
                    // $02 end   新車タブレットショールーム管理機能開発
                }
            }
            gRequestHelpTimeSpanList[aIndex] = (aValue - 0) + 1 + '';
        }
    });

    if (drawFlag) {

        // 初期表示以外の場合、警告音出力処理を行う（カウンター処理）
        if (!aFirstFlag) {
            alertOutputCounter();
        }
    }
}

/**
* ポップアップを閉じる.
*/
function hidePopOver() {
    if ($.fn.popoverEx.openedPopup) {
        $.fn.popoverEx.openedPopup.trigger('hidePopover');
        parent.lockReset();
    }
}

/**
* 透明なオーバーレイを表示する.
*/
function showOverlayTransparency() {
    $('div#MainAreaOverlayTransparency').fadeIn(0);
}

/**
* 透明なオーバーレイを非表示にする.
*/
function hideOverlayTransparency() {
    $('div#MainAreaOverlayTransparency').fadeOut(0);
}

/**
* 透明とグレーなオーバーレイを非表示にする.
*/
function hideOverlay() {
    // 更新ロックフラグを解除
    parent.gGrayOutFlg = false;

    // オーバーレイを解除
    $('div#MainAreaOverlayTransparency').fadeOut(C_FADE_OUT_OVERLAY);
}

/**
* 読み込み中エリアを表示する.
*/
function showLodingWindow(aContainer) {

    var overlay = aContainer.find('div.MstPG_LoadingScreen');
    overlay.css({
        position: 'relative',
        width: aContainer.outerWidth() + 'px',
        height: aContainer.outerHeight() + 'px'
    });

    overlay.css({ 'display': 'table' });
}

/**
* 読み込み中エリアを非表示にする.
*/
function closeLodingWindow() {
    $('div.MstPG_LoadingScreen:visible').css({ display: 'none' });

}

/**
* 削除ボタンを非表示にする.
*/
function hideDeleteButton() {

    if ($('div.ComingStoreOffDeletionButton:visible').length > 0) {
        $('div.ComingStoreOffDeletionButton:visible').fadeOut(C_FADE_OUT_CUSTOMER_DELETE_BUTTON);
        return true;
    }
    return false;
}

/**
* 現在の警告チップの数を取得する.
*/
function getNowAlertChipCount() {

    // 現在の警告チップの数
    // $02 start 新車タブレットショールーム管理機能開発
    var nowAlertChipCount =
        $('div#CassetteBox01FlickArea').find('div.BackColor_Red').length +
        $('div#CassetteBox01FlickArea').find('div.BackColor_Yellow').length +
        $('div#CassetteBox02FlickArea').find('div.BackColor_Red').length +
        $('div#CassetteBox03FlickArea').find('div.BackColor_Red').length +
        $('div#CassetteBox03FlickArea').find('li.Color_Red').length;
    // $02 end   新車タブレットショールーム管理機能開発

    return nowAlertChipCount;
}

/**
* 警告音出力処理を行う（カウンター処理）.
*/
function alertOutputCounter() {

    // 処理停止中の場合は処理しない
    if (parent.gLogicStopFlag) {
        return;
    }

    if (!parent.gMainAlertFlg) {
        return;
    }

    // 現在の警告チップの数
    var nowAlertChipCount = getNowAlertChipCount();

    // 保持しておいた警告チップの数
    var beforeAlertChipCount = parent.gAlertChipCount;

    // 現在の警告チップの数を保持しておく
    parent.gAlertChipCount = nowAlertChipCount;

    // デバッグ用
    if (parent.gDebugObject.debugFlag) {
        $('span#BeforeAlertCount', $(parent.document)).text(beforeAlertChipCount);
        $('span#AfterAlertCount', $(parent.document)).text(nowAlertChipCount);
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

        //alert('プッシュ受信による警告音出力');
        icropScript.ui.beep(2);
    }
    else {
        //alert('警告チップ増加による警告音出力');
        icropScript.ui.beep(3);
    }

    // 警告音出力中リストに番号を追加
    //gAlertOutputNoList.push(aAlertOutputNo);
    //}
}