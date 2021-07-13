/** 
 * @fileOverview ショールームステータスビジュアライゼーションの
 * 共通処理を記述するファイル.
 * 
 * @author t.mizumoto
 * @version 1.0.0
 */
// ==============================================================
// 定数
// ==============================================================
// イベント名
var C_MOUSE_DOWN = 'mousedown';
var C_MOUSE_MOVE = 'mousemove';
var C_MOUSE_UP = 'mouseup';

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
        
        $(this).live(C_MOUSE_DOWN, function (aEvent) {

            touchStart = true;
            touchMove = false;
            eventTarget = aEvent.target;

        });

        $(this).live(C_MOUSE_MOVE, function (aEvent) {

            if (!touchStart) {
                return;
            }

            touchMove = true;
        });

        $(this).live(C_MOUSE_UP, function (aEvent) {

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
    
    var touchAndonAction = null;
    var touchAndonFlag = false;
    var touchAndonInterval = 200; // アンドン連続タップ禁止時間 : ms

    // アンドン共通処理
    $.fn.setAndonChipEvent = function (chipTarget) {

        $(this).bind(C_MOUSE_DOWN, function () {
            
            if (touchAndonFlag) {
                // フラグが立って入るときは処理をしない
                return;
            }

            // フラグを立て、連続タップを防止
            touchAndonFlag = true;

            var selectStatusArea = $(this);

            // 既に選択されている(ハイライト時)の場合
            if (selectStatusArea.hasClass('Active')) {
                
                // チップの選択解除
                removeSelectAreaAll();
                // ロック解除
                parent.lockReset();
                // グレーアウトを解除
                hideOverlayGray();
                // フラグを落とす
                touchAndonFlag = false;
                return;
            }

            // 通常時

            //ロックする
            parent.lock();
            // アンドンチップ、選択したアンドンチップ対応するスクロールエリア・チップを選択状態にする
            chipTarget.add(selectStatusArea).add(chipTarget.parent()).addClass('Active');
            // チップがスタッフチップの場合
            if (chipTarget.filter('#StuffChip').length > 0) {

                // チップをグレーアウトする
                showSelectedChipGrayOut();
            }
            // グレーオーバーレイ処理
            showOverlayGray();

            touchAndonAction = setTimeout(function () {
                // フラグを落とし、タイマーをクリア
                touchAndonFlag = false;
                clearTimeout(touchAndonAction);
            }, touchAndonInterval);
        });
        return $(this);
    }
})(jQuery);

/**
 * 経過時間を表示する文字列を取得する.
 * @param {Integer} aSecondSpan 経過秒
 * @param {Integer} aMaxLength 最大文字列長
 * @return {String} 表示文字列
 */
function getDispTime(aSecondSpan, aMaxLength) {
    var minutes = Math.floor(aSecondSpan / 60);
    var second = aSecondSpan % 60;
    var secondDisp = ('0' + second).slice(-2);

    var result = minutes + '\'' + secondDisp + '\"';

    if (result.length > aMaxLength) {
        result = result.substr(0, aMaxLength);
    }

    return result;
}