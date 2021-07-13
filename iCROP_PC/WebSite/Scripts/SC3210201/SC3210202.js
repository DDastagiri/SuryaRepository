/** 
* @fileOverview ショールームステータスビジュアライゼーション(サブエリア)の
* 処理を記述するファイル.
* 
* @author t.mizumoto
* @version 1.0.0
*/
// ==============================================================
// 定数
// ==============================================================
// フェードインのミリ秒
var C_FADE_IN_OVERLAY = 0; // フェードインなし
var C_FADE_IN_CHIP_GRAY_OUT = 0; // フェードインなし

// フェードアウトのミリ秒
var C_FADE_OUT_OVERLAY = 0; // フェードアウトなし
var C_FADE_OUT_CHIP_GRAY_OUT = 0; // フェードアウトなし

// スタッフの経過時間の最大文字列長
var C_MAX_LENGTH_STAFF_SPAN_TIME = 8;

// 顧客の経過時間の最大文字列長
var C_MAX_LENGTH_CUSTOMER_SPAN_TIME = 6;

// ==============================================================
// 変数
// ==============================================================
var gSalesStartTimeSpanList = [];
var gVisitVisitTimeSpanList = [];
var gWaitVisitTimeSpanDateList = [];
var gRequestAssessmentTimeSpanList = [];
var gRequestPriceConsultationTimeSpanList = [];
var gRequestHelpTimeSpanList = [];

// ==============================================================
// DOMロード時処理
// ==============================================================
$(function () {

    // エリア定義定義を行う
    initArea();

    // アンドンチップ定義を行う。
    initStatusChip()

    // スタッフチップ定義を行う
    initStaffChip();

    // ポップオーバー定義を行う
    initPopOver();

    // カウンター定義を行う
    initCounter();

    /**
    * エリア定義定義を行う.
    */
    function initArea() {

        // エリア選択時
        $('div#bodyFrame').bind(C_MOUSE_DOWN, function (aEvent) {

            // 警告音停止処理を行う
            //parent.stopAlertOutput();
        });

        // 共通イベント定義
        $('ul.SituationListSet, div.Cassette').setCommonEvent();

        // スタッフ状況・来店状況・待ち状況エリアのチップとチップの間の処理を実現する為の定義
        $('ul.SituationListSet, div.Cassette').bind('tap', function (aEvent, aTarget) {

            // アンドン未選択状態 (ハイライトではない場合)
            if ($('div#nssv00HeadLeftBox').find('li.Active').length == 0) {

                return;
            }

            // タップしたチップがハイライトの場合は解除しない
            if ($(aTarget).parents('li.Active, div.Active').length > 0) {

                // ロックカウンター初期化
                parent.lockCounterReset();
                return;
            }

            // ハイライト表示を解除
            removeSelectAreaAll();
            // グレーアウトを解除
            hideOverlayGray();
            // ロックを解除
            parent.lockReset();
        });

        // スタッフ状況・来店状況・待ち状況エリアにてスクロールされた場合の定義
        $('ul.SituationListSet, div.Cassette').scroll(function () {

            // ロックカウンター初期化
            parent.lockCounterReset();

            // 警告音停止処理を行う
            //parent.stopAlertOutput();
        });

        // z-indexについて、以下の順で定義する
        // ・ポップオーバーエリア
        // ・読み込み中エリア                    z-index:1000
        // ・オーバーレイ(透明)                  z-index:1000
        // ・選択状況(スクロール)エリア          z-index:999
        // ・アンドンチップ(ハイライト)          z-index:999
        // ・スタッフチップ(ハイライト)          z-index:999
        // ・顧客チップ(ハイライト)              z-index:999
        // ・オーバーレイ(グレー)                z-index:600
        // ・アンドンチップ(グレー)              z-index:500
        // ・スタッフチップ(グレー)              z-index:500
        // ・顧客チップ(グレー)                  z-index:500

        // オーバーレイ(透明)
        var mainAreaOverlayTransparency = $('<div id="MainAreaOverlayTransparency" />');
        mainAreaOverlayTransparency.addClass('ScreenBlack');
        mainAreaOverlayTransparency.css({ background: 'rgba(0, 0, 0, 0.0)', zIndex: '1000', display: 'none' });

        $('div#InnerMainBlock').append(mainAreaOverlayTransparency);

        // 透明なオーバーレイ押下時にポップオーバー画面を解除
        mainAreaOverlayTransparency.bind(C_MOUSE_DOWN, function () {

            hidePopOver();
            parent.hidePopOver();
        });

        // オーバーレイ(グレー)
        var mainAreaOverlayGray = $('<div id="MainAreaOverlayGray" />');
        mainAreaOverlayGray.addClass('ScreenBlack');
        mainAreaOverlayGray.css({ display: 'none' });

        $('div#InnerMainBlock').append(mainAreaOverlayGray);

        // オーバーレイ(グレー)押下時にグレーアウトを解除
        mainAreaOverlayGray.bind(C_MOUSE_DOWN, function () {

            // ハイライト表示を解除
            removeSelectAreaAll();
            hideOverlayGray();
            // ロックを解除
            parent.lockReset();
        });
    }

    /**
    * アンドンチップ定義を行う.
    */
    function initStatusChip() {

        // 来店
        $('li#ReceptionistMainComingAria').setAndonChipEvent($('div#ComingStore').find('div#CustomerChip'));
        // 待ち
        $('li#ReceptionistMainWaitAria').setAndonChipEvent($('div#WaitStore').find('div#CustomerChip'));
        // 査定
        $('li#ReceptionistMainAssessmentAria').setAndonChipEvent($('img#AssessmentIconOn').parents('li#StuffChip'));
        // 価格相談
        $('li#ReceptionistMainPriceConsultationAria').setAndonChipEvent($('img#PriceIconOn').parents('li#StuffChip'));
        // ヘルプ
        $('li#ReceptionistMainHelpAria').setAndonChipEvent($('img#HelpIconOn').parents('li#StuffChip'));
    }

    /**
    * スタッフチップ定義を行う.
    */
    function initStaffChip() {

        // スタッフチップ
        var staffChip = $('div.staffChip');

        // 独自イベント設定
        staffChip.setCommonEvent();

        // タップ
        staffChip.bind(C_MOUSE_DOWN, function () {

            // 商談中以外のチップの場合は対象外
            if ($(this).find('div.ListBox').length == 0) {

                return;
            }

            parent.lock();

            // タイトル初期化
            $('h3', $(parent.document)).css({ 'display': 'none' });

            // 商談中詳細画面の削除
            parent.DeleteStaffDetailPopOver();

            showLodingWindow($('div.scNscPopUpContactVisitListArea', $(parent.document)));

            // 商談中詳細に必要なパラメータを設定
            $('input#StaffDetailDialogVisitSeq', $(parent.document)).val($(this).find('input#VisitSeq').val());

            var index = 0;
            var visitSeq = $(this).find('input#VisitSeq').val();

            //インデックス番号
            staffChip.each(function (aIndex, aValue) {

                //通信依頼種別事の信号を出力
                if ($(this).find('input#VisitSeq').length > 0 &&
                    $(this).find('input#VisitSeq').val() == visitSeq) {

                    index = aIndex;
                }
            });

            $('input#StaffDetailDialogIndex', $(parent.document)).val(index);

            $('input#StaffDetailDisplayButton', $(parent.document)).click();

            $(this).trigger('showPopover');
        });
    }

    /**
    * ポップオーバー定義を行う.
    */
    function initPopOver() {

        // 商談中詳細
        $('div.staffChip').popoverEx({

            contentId: $('div.scNscPopUpContactVisit', $(parent.document))
            , openEvent: function () {

                parent.popupOpen();

                // 透明なオーバーレイを設定する
                showOverlayTransparency();
            }
            , closeEvent: function () {

                // キーボードを非表示にするため領域を削除する(入力エリアのフォーカスを外す)
                parent.DeleteStaffDetailPopOver();

                parent.popupClose();

                // 透明なオーバーレイを削除する
                hideOverlayTransparency();
            }
            , offsetX: $('div#MainBlock', $(parent.document)).offset().left
            , offsetY: $('div#MainBlock', $(parent.document)).offset().top
        });
    }

    /**
    * カウンター定義を行う.
    */
    function initCounter() {

        // 商談開始日時
        if ($('input#SalesStartTimeList').val().length > 0) {

            gSalesStartTimeSpanList = $.evalJSON($('input#SalesStartTimeList').val());
        }
        // 来店日時
        if ($('input#VisitVisitTimeList').val().length > 0) {

            gVisitVisitTimeSpanList = $.evalJSON($('input#VisitVisitTimeList').val());
        }
        // 待ち日時
        if ($('input#WaitVisitTimeDateList').val().length > 0) {

            gWaitVisitTimeSpanDateList = $.evalJSON($('input#WaitVisitTimeDateList').val());
        }
        // 通知依頼(査定)
        if ($('input#RequestAssessmentTimeDateList').val().length > 0) {

            gRequestAssessmentTimeSpanList = $.evalJSON($('input#RequestAssessmentTimeDateList').val());
        }
        // 通知依頼(価格相談)
        if ($('input#RequestPriceConsultationTimeDateList').val().length > 0) {

            gRequestPriceConsultationTimeSpanList = $.evalJSON($('input#RequestPriceConsultationTimeDateList').val());
        }
        // 通知依頼(ヘルプ)
        if ($('input#RequestHelpTimeDateList').val().length > 0) {

            gRequestHelpTimeSpanList = $.evalJSON($('input#RequestHelpTimeDateList').val());
        }
        counter(true);
        counterFlashing();
        setInterval(counter, 1000);
        setInterval(counterFlashing, 500);
    }
});

// ==============================================================
// 関数
// ==============================================================
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
        $('img.imageFlashing').toggleClass("imageDisplayNone");
    }
}

/**
* カウンター処理を行う.
*/
function counter(aFirstFlag) {

    // ポップオーバー表示中は描画を行わない
    if ($.fn.popoverEx.openedPopup) {

        drawFlag = false;
    }
    else {

        drawFlag = true;
    }

    // 商談時間
    var index = 0;
    $.each(gSalesStartTimeSpanList, function (aIndex, aValue) {
        if (0 < aValue.length) {

            if (drawFlag) {

                $('div.SituationFlame').find('div#BusinessLapsedTime:eq(' + aIndex + ')').text(getDispTime(aValue, C_MAX_LENGTH_STAFF_SPAN_TIME));
            }
            gSalesStartTimeSpanList[aIndex] = (aValue - 0) + 1 + '';

        }
    });

    // 来店時間
    $.each(gVisitVisitTimeSpanList, function (aIndex, aValue) {
        if (0 < aValue.length) {

            if (drawFlag) {

                if (0 <= parent.gVisitTimeAlertSpan && parent.gVisitTimeAlertSpan < (aValue - 0)) {

                    if (!$('div#ComingStore').find('div#CustomerChip:eq(' + aIndex + ')').hasClass('CassetteRed')) {
                        $('div#ComingStore').find('div#CustomerChip:eq(' + aIndex + ')').addClass('CassetteRed');
                    }
                    if (!$('div#ReceptionistMainComing').hasClass('colorRad')) {
                        $('div#ReceptionistMainComing').removeClass('colorNone').addClass('colorRad');
                    }
                }
                $('div#ComingStore').find('p#VisLapsedTime:eq(' + aIndex + ')').text(getDispTime(aValue, C_MAX_LENGTH_CUSTOMER_SPAN_TIME));
            }
            gVisitVisitTimeSpanList[aIndex] = (aValue - 0) + 1 + '';

        }
    });

    // 待ち時間
    $.each(gWaitVisitTimeSpanDateList, function (aIndex, aValue) {
        if (0 < aValue.length) {

            if (drawFlag) {

                if (0 <= parent.gWaitTimeAlertSpan && parent.gWaitTimeAlertSpan < (aValue - 0)) {

                    if (!$('div#WaitStore').find('div#CustomerChip:eq(' + aIndex + ')').hasClass('CassetteRed')) {
                        $('div#WaitStore').find('div#CustomerChip:eq(' + aIndex + ')').addClass('CassetteRed');
                    }
                    if (!$('div#ReceptionistMainWait').hasClass('colorRad')) {
                        $('div#ReceptionistMainWait').removeClass('colorNone').addClass('colorRad');
                    }
                }
                $('div#WaitStore').find('p#WaitLapsedTime:eq(' + aIndex + ')').text(getDispTime(aValue, C_MAX_LENGTH_CUSTOMER_SPAN_TIME));
            }
            gWaitVisitTimeSpanDateList[aIndex] = (aValue - 0) + 1 + '';

        }
    });

    // 査定依頼通知待ち時間
    $.each(gRequestAssessmentTimeSpanList, function (aIndex, aValue) {
        if (0 < aValue.length) {

            if (drawFlag) {

                if (0 <= parent.gAssessmentTimeAlertSpan && parent.gAssessmentTimeAlertSpan < (aValue - 0)) {

                    if (!$('div.SituationFlame').find('div#MainDiv:eq(' + aIndex + ')').hasClass('ListBoxRed')) {
                        $('div.SituationFlame').find('div#MainDiv:eq(' + aIndex + ')').addClass('ListBoxRed');
                    }
                    if (!$('div#ReceptionistMainAssessment').hasClass('colorRad')) {
                        $('div#ReceptionistMainAssessment').removeClass('colorNone').addClass('colorRad');
                    }
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

                    if (!$('div.SituationFlame').find('div#MainDiv:eq(' + aIndex + ')').hasClass('ListBoxRed')) {
                        $('div.SituationFlame').find('div#MainDiv:eq(' + aIndex + ')').addClass('ListBoxRed');
                    }
                    if (!$('div#ReceptionistMainPriceConsultation').hasClass('colorRad')) {
                        $('div#ReceptionistMainPriceConsultation').removeClass('colorNone').addClass('colorRad');
                    }
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

                    // スタッフチップの判定
                    if (!$('div.SituationFlame').find('div#MainDiv:eq(' + aIndex + ')').hasClass('ListBoxRed')) {
                        $('div.SituationFlame').find('div#MainDiv:eq(' + aIndex + ')').addClass('ListBoxRed');
                    }
                    if (!$('div#ReceptionistMainHelp').hasClass('colorRad')) {
                        $('div#ReceptionistMainHelp').removeClass('colorNone').addClass('colorRad');
                    }
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
        parent.gPopOverStaffIndex = 0;
    }
}

/**
* 透明なオーバーレイを表示する.
*/
function showOverlayTransparency() {

    $('div#MainAreaOverlayTransparency').fadeIn(C_FADE_IN_OVERLAY);
}

/**
* 透明なオーバーレイを非表示にする.
*/
function hideOverlayTransparency() {

    $('div#MainAreaOverlayTransparency').fadeOut(C_FADE_OUT_OVERLAY);
}

/**
* グレーオーバーレイを表示する.
*/
function showOverlayGray() {

    // 更新ロックフラグを設定
    parent.gGrayOutFlg = true;
    // 全体をグレーアウト
    $('div#MainAreaOverlayGray').css('display', 'block');
}

/**
* グレーオーバーレイを非表示にする.
*/
function hideOverlayGray() {

    // 更新ロックフラグを解除
    parent.gGrayOutFlg = false;
    $('div#MainAreaOverlayGray').css('display', 'none');
    // チップのグレーアウトを解除
    hideSelectedChipGrayOut();
}

/**
* オーバーレイを非表示にする.
*/
function hideOverlay() {

    // グレーオーバーレイ
    hideOverlayGray();
    // 透明なオーバーレイ
    hideOverlayTransparency();
    // 選択状態エリア・チップを解除する
    removeSelectAreaAll();
}

/**
* 選択中チップ以外のチップをグレーアウトする.
*/
function showSelectedChipGrayOut() {

    // スタッフチップ
    $('li#StuffChip').not('li.Active').find('div.StuffScreenBlack').fadeIn(C_FADE_IN_CHIP_GRAY_OUT);
}

/**
* チップのグレーアウトを解除する.
*/
function hideSelectedChipGrayOut() {

    $('div.StuffScreenBlack').fadeOut(C_FADE_IN_CHIP_GRAY_OUT);
}

/**
* 選択チップをすべて未選択状態にする.
*/
function removeSelectAreaAll() {

    // アンドンチップ
    var Receptionist = $('div#nssv00HeadLeftBox').find('.Active');
    // スタッフ・来店・待ち状況エリア、スタッフチップ
    var selectArea = $('ul.SituationListSet, div.Cassette, li#StuffChip').filter('.Active');
    // 選択状態を解除
    selectArea.add(Receptionist).removeClass('Active');
}

/**
* 読み込み中エリアを表示する.
*/
function showLodingWindow(aContainer) {

    var overlay = aContainer.find('div.registOverlay');
    overlay.css({
        width: aContainer.outerWidth() + 'px',
        height: aContainer.outerHeight() + 'px'
    });
    overlay.css({ 'display': 'table' });
}

/**
* 読み込み中エリアを非表示にする.
*/
function closeLodingWindow() {

    $('div.registOverlay:visible').css({ display: 'none' });
}

/**
* 現在の警告チップの数を取得する.
*/
function getNowAlertChipCount() {

    // 現在の警告チップの数
    var nowAlertChipCount =
        $('div#nssv00HeadLeftBox').find('div.colorRad').length +
        $('ul.SituationListSet').find('div.ListBoxRed').length +
        $('div#ComingStore, div#WaitStore').find('div.CassetteRed').length;

    return nowAlertChipCount;
}

/**
* 警告音出力処理を行う（カウンター処理）.
*/
function alertOutputCounter() {

    if (!parent.gMainAlertFlg) {
        return;
    }

    // 現在の警告チップの数
    var nowAlertChipCount = getNowAlertChipCount();

    // 保持しておいた警告チップの数
    var beforeAlertChipCount = parent.gAlertChipCount;

    // 現在の警告チップの数を保持しておく
    parent.gAlertChipCount = nowAlertChipCount;

    // 警告チップの数が増加した場合
    if (beforeAlertChipCount < nowAlertChipCount) {

        // 異常警告音出力
        parent.alertOutput('2');
    }
}
