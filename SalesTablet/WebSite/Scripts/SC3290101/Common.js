/** 
 * @fileOverview フォロー設定の共通処理を記述するファイル.
 * 
 * @author t.mizumoto
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


// ==============================================================
// プラグイン定義
// ==============================================================
(function ($) {

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

})(jQuery);

