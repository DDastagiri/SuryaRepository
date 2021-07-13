/** 
 * @fileOverview SC3100103 パネル内の処理
 * 
 * @author m.okamura
 * @version 1.0.0
 */

// ==============================================================
// 定数
// ==============================================================
// タッチデバイスの判定用
var gIsTouch = false;
var gAgent = navigator.userAgent.toLowerCase();
if (0 <= gAgent.indexOf('iphone') || 0 <= gAgent.indexOf('ipad')) {
    gIsTouch = true;
}

// タッチ系イベント名
var C_TOUCH_START = gIsTouch ? 'touchstart' : 'mousedown';
var C_TOUCH_MOVE = gIsTouch ? 'touchmove' : 'mousemove';
var C_TOUCH_END = gIsTouch ? 'touchend' : 'mouseup';

/**
 * 使用中・未使用の初期表示処理を行う.
 * 
 */
$(window).load(function () {

    // 汎用タップイベント
    // ・ドラッグ時は動作しない
    $.fn.setCommonEvent = function () {

        var touchStart = false;
        var touchMove = false;
        var eventTarget = null;

        $(this).live(C_TOUCH_START, function (aEvent) {

            touchStart = true;
            touchMove = false;
            eventTarget = aEvent.target;

        });

        $(this).live(C_TOUCH_MOVE, function (aEvent) {

            if (!touchStart) {
                return;
            }

            touchMove = true;
        });

        $(this).live(C_TOUCH_END, function (aEvent) {

            // ムーブで入ってきた場合なので処理対象除外
            if (!touchStart) {
                return;
            }

            // ムーブした場合なので処理対象除外
            if (touchMove) {
                return;
            }

            // 初期化
            touchStart = false;
            touchMove = false;

            $(this).trigger('tap', eventTarget);

        });

        return $(this);
    }

    // エリア定義定義を行う
    initArea();

    // スタッフチップ定義を行う
    initStaffChip();

    // 矢印ボタン定義を行う
    initSelectButton();

    /**
    * エリア定義定義を行う.
    */
    function initArea() {

        // エリア選択時
        $('div#bodyFrame').bind(C_TOUCH_START, function (aEvent) {

            // フリックエリアを除く
            if ($(aEvent.target).parents('div.innerDataBox').length > 0) {
                return;
            }

            // 矢印ボタン(上)を除く
            if ($(aEvent.target).parents('div#SelectTopButton').length > 0) {
                return;
            }

            // 矢印ボタン(下)を除く
            if ($(aEvent.target).parents('div#SelectBottomButton').length > 0) {
                return;
            }

            // 矢印ボタンを消去
            var selectButton = $('div#SelectTopButton, div#SelectBottomButton');
            selectButton.css({ display: 'none' });
            // 現在選択中のチップを選択解除する
            var selectChip = $('div#StandByStaffPanel').find('.SelectedRow');
            selectChip.removeClass('SelectedRow');
        });
    }

    /**
    * スタッフチップ定義を行う.
    */
    function initStaffChip() {

        // スタッフチップ
        var staffChip = $('div.NormalRow');

        // 独自イベント設定
        staffChip.setCommonEvent();

        // タップ
        staffChip.bind('tap', function () {

            // 矢印ボタン
            var selectTopButton = $('div#SelectTopButton');
            var selectBottomButton = $('div#SelectBottomButton');

            selectTopButton.add(selectBottomButton).css({ display: 'none' });
            // 選択中の場合は選択解除
            if ($(this).hasClass('SelectedRow')) {

                // 選択解除する
                $(this).removeClass('SelectedRow');
                return;
            }

            // 現在選択中のチップを選択解除する
            var selectChip = $('div#StandByStaffPanel').find('.SelectedRow');
            selectChip.removeClass('SelectedRow');

            // 選択中にする
            $(this).addClass('SelectedRow');

            // スタッフチップ一覧
            var staffChip = $('div#StandByStaffPanel').find('div.NormalRow');
            // 選択中のチップ番号
            var selectChipIndex = -1;
            // 選択中のものを検索
            staffChip.each(function (aIndex, aValue) {

                if ($(this).hasClass('SelectedRow')) {

                    selectChipIndex = aIndex;
                }
            });

            if (selectChipIndex != 0) {
                selectTopButton.css({ display: 'block' });
            }
            if (selectChipIndex != staffChip.length - 1) {
                selectBottomButton.css({ display: 'block' });
            }
        });
    }

    /**
    * 矢印ボタン定義を行う.
    */
    function initSelectButton() {

        // 矢印ボタン
        var selectButton = $('div#SelectTopButton, div#SelectBottomButton');

        // 独自イベント設定
        selectButton.setCommonEvent();

        // タップ
        selectButton.bind('tap', function () {

            // スタッフチップ一覧
            var staffChip = $('div#StandByStaffPanel').find('div.NormalRow');
            // 選択中のチップ番号
            var selectChipIndex = -1;

            // 選択中のものを検索
            staffChip.each(function (aIndex, aValue) {

                if ($(this).hasClass('SelectedRow')) {

                    selectChipIndex = aIndex;
                }
            });

            // 選択状態がない場合は対象外
            if (selectChipIndex < 0) {

                return;
            }

            // DB設定値
            var staffChipAccount = $('div#StandByStaffPanel').find('input#Account');
            var staffChipPresenceCategoryDate = $('div#StandByStaffPanel').find('input#PresenceCategoryDate');
            var selectAccount = $(staffChipAccount[selectChipIndex]).val();
            var selectPresenceCategoryDate = $(staffChipPresenceCategoryDate[selectChipIndex]).val();

            // 入れ替えチップ番号
            var changeStaffChipIndex;

            // 入れ替え処理
            if ($(this).attr('id') == ('SelectTopButton')) {

                // 最下段の行が選択されている場合
                if (selectChipIndex == (staffChip.length - 1)) {

                    $('div#SelectBottomButton').css({ display: 'block' });
                }

                // 1つ上のチップ
                changeStaffChipIndex = selectChipIndex - 1;

                // 選択チップが最上段に移動した場合
                if (changeStaffChipIndex <= 0) {

                    $('div#SelectTopButton').css({ display: 'none' });
                }

                // 選択チップの後ろに1つ上のチップを持っていく(描画崩れが発生するためTimerを設ける)
                setTimeout(function () {
                    $(staffChip[selectChipIndex]).after(staffChip[changeStaffChipIndex]);
                }, 20);
            }
            else {

                // 最上段の行が選択されている場合
                if (selectChipIndex <= 0) {

                    $('div#SelectTopButton').css({ display: 'block' });
                }

                // 1つ下のチップ
                changeStaffChipIndex = selectChipIndex + 1;

                // 選択チップが最下段に移動した場合
                if (changeStaffChipIndex == (staffChip.length - 1)) {

                    $('div#SelectBottomButton').css({ display: 'none' });
                }

                // 1つ下のチップの後ろに選択チップを持っていく(描画崩れが発生するためTimerを設ける)
                setTimeout(function () {
                    $(staffChip[changeStaffChipIndex]).after(staffChip[selectChipIndex]);
                }, 20);
            }
            
            setTimeout(function () {
                if ($(staffChip[changeStaffChipIndex]).hasClass('SecondRow')) {
                    $(staffChip[changeStaffChipIndex]).removeClass('SecondRow');
                    $(staffChip[selectChipIndex]).removeClass('SelectedRow').addClass('SecondRow').addClass('SelectedRow');
                } else {
                    $(staffChip[changeStaffChipIndex]).addClass('SecondRow');
                    $(staffChip[selectChipIndex]).removeClass('SecondRow');
                }
            }, 40);

            $(staffChipAccount[selectChipIndex]).val($(staffChipAccount[changeStaffChipIndex]).val());
            $(staffChipAccount[changeStaffChipIndex]).val(selectAccount);
            $(staffChipPresenceCategoryDate[selectChipIndex]).val($(staffChipPresenceCategoryDate[changeStaffChipIndex]).val());
            $(staffChipPresenceCategoryDate[changeStaffChipIndex]).val(selectPresenceCategoryDate);

            // 移動後チップのトップ位置
            // (Timerを設けているため、移動する先のチップトップ位置を取得)
            var offsetTopPos = $(staffChip[changeStaffChipIndex]).offset().top;

            // チップ位置が表示範囲(上)を超えていないかチェック
            if (offsetTopPos < 0) {

                // 位置の移動(描画崩れが発生するためTimerを設ける)
                setTimeout(function () {
                    $('.innerDataBox').fingerScroll({ action: 'move', moveY: offsetTopPos - 37, moveX: 0 });
                }, 80);

                return;
            }

            // 最大表示範囲(チップのマージン分引いておく)
            var displayMaxHeight = $('.innerDataBox').height() - 2;
            // チップの実際の高さ
            var offsetHeight = $(staffChip[selectChipIndex]).innerHeight();

            // チップ位置が表示範囲(下)を超えていないかチェック
            if (displayMaxHeight < (offsetTopPos)) {

                // 位置の移動(描画崩れが発生するためTimerを設ける)
                setTimeout(function () {
                    $('.innerDataBox').fingerScroll({ action: 'move', moveY: offsetTopPos - (displayMaxHeight - offsetHeight) - 37, moveX: 0 });
                }, 80);
            }
        });
    }

    if ($('#StandByStaffClickStatus', $(parent.document)).val() == '1') {
        parent.closePopOver();
    }

    // データ件数を取得
    var cnt = $('.NormalRow').size() - 1;

    // 件数が0件の場合
    if (cnt == -1) {

        $('#StandByStaffClickStatus', window.parent.document).val('-1');
        parent.$('.StandByStaffPopupRegistButton').addClass('StandByStaffPopupRegistButtonOff');
    } else {

        // clickイベントの復帰
        $('#StandByStaffClickStatus', window.parent.document).val('0');
        parent.$('.StandByStaffPopupRegistButton').removeClass('StandByStaffPopupRegistButtonOff');

        // スクロールの設定を行う
        $('.innerDataBox').fingerScroll();
        // 実際の高さを再設定する(影の分だけ追加)
        $('.scroll-inner').height($('.scroll-inner').height() + 2);
    }
});

/**
 * 登録ボタン押下時処理.
 * 
 * @param {-} - -
 * @return {-} -
 * 
 * @example 
 *  -
 */
function redirectSC3100103() {

    SC3100103.startServerCallback();
    if ($('#StandByStaffErrorMessage').val() != '0') {
        $('#StandByStaffClickStatus', window.parent.document).val('0');
    }
    else {
        $('#StandByStaffClickStatus', window.parent.document).val('1');
    }
}

/**
 * 初期表示.
 * 
 * @param {-} - -
 * @return {-} -
 * 
 * @example 
 *  -
 */
function pageInit() {

    // 非表示の場合は読み込みを行わない
    if ($('#Panel_SC3100103:visible', $(parent.document)).length == 0) {
        return;
    }

    $('#LoadSpinButton').click();
    SC3100103.startServerCallback();
}

/**
 * 初期処理
 */
(function (window) {

    $.extend(window, { SC3100103: {} });
    $.extend(SC3100103, {

        /**
         * コールバック開始
         */
        startServerCallback: function () {
            SC3100103.showLoding();
        },

        /**
         * コールバック終了
         */
        endServerCallback: function () {
            SC3100103.closeLoding();
        },

        /******************************************************************************
        読み込み中表示
        ******************************************************************************/

        /**
         * 読み込み中アイコン表示
         */
        showLoding: function () {

            //オーバーレイ表示
            $('#registOverlayBlackSC3100103').css('display', 'block');
            //アニメーション
            setTimeout(function () {
                $('#processingServerSC3100103').addClass('show');
                $('#registOverlayBlackSC3100103').addClass('open');
            }, 0);

        },

        /**
         * 読み込み中アイコンを非表示にする
         */
        closeLoding: function () {

            $('#processingServerSC3100103').removeClass('show');
            $('#registOverlayBlackSC3100103').removeClass('open').one('webkitTransitionEnd', function (we) {
                $('#registOverlayBlackSC3100103').css('display', 'none');
            });
        }
    });

})(window);
