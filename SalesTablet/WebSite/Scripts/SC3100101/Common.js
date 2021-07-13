/** 
 * @fileOverview 受付メインの共通処理を記述するファイル.
 * 
 * @author t.mizumoto
 * @version 1.0.0
 * 更新： 2013/01/18 TMEJ m.asano 新車タブレットショールーム管理機能開発 $02
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

// 更新系ボタンの2度押し防止用
var C_UPDATE_FLAG_NAME = 'updateFlag';
var C_UPDATE_FLAG_ON = '1';
var C_UPDATE_FLAG_OFF = '0';


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

    // チップタップイベント
    // ・ドラッグ時は動作しない
    // チップスワイプイベント
    // チップホールドイベント
    $.fn.setChipEvent = function (aOptions) {

        var options = $.extend({
            swipeIntervalX: 80        // スワイプ時の許容範囲 : 横軸
          , swipeIntervalY: 10        // スワイプ時の許容範囲 : 縦軸
          , holdTimeInterval: 600     // ホールド判定時間     : ms
        }, aOptions);
        
        var touchStart = false;
        var touchMove = false;
        var eventTarget = null;
        
        var startPageX = null;
        var startPageY = null;
        var isSetSwipe = false;

        var holdAction = null;
        var touchHold = false;

        $.fn.setChipEvent.cancelEventFlag = false;

        $(this).bind(C_TOUCH_START, function (aEvent) {

            if (holdAction != null) {
                clearTimeout(holdAction);
            }

            touchStart = true;
            touchMove = false;
            touchHold = false;
            $.fn.setChipEvent.cancelEventFlag = false;

            eventTarget = aEvent.target;

            isSetSwipe = false;
            startPageX = gIsTouch ? aEvent.originalEvent.touches[0].pageX : aEvent.pageX;
            startPageY = gIsTouch ? aEvent.originalEvent.touches[0].pageY : aEvent.pageY;

            // ホールドイベント
            var holdObj = $(this);
            holdAction = setTimeout(function () {
                touchHold = true;
                if(!$.fn.setChipEvent.cancelEventFlag) {
                    holdObj.trigger("chipHold", {event:aEvent, target:holdObj});
                }
                clearTimeout(holdAction);
            }, options.holdTimeInterval);

        });

        $(this).bind(C_TOUCH_MOVE, function (aEvent) {

            if (!touchStart) {
                return;
            }
            
            if (holdAction != null && !touchMove) {
                clearTimeout(holdAction);
            }

            touchMove = true;
            touchHold = false;

            if (!isSetSwipe) {

                var movePageY = gIsTouch ? aEvent.originalEvent.touches[0].pageY : aEvent.pageY;
                if (Math.abs(startPageY - movePageY) >= options.swipeIntervalY) {
                    return;
                }

                var movePageX = gIsTouch ? aEvent.originalEvent.touches[0].pageX : aEvent.pageX;

                if (Math.abs(startPageX - movePageX) >= options.swipeIntervalX) {
                    isSetSwipe = true;
                    if(!$.fn.setChipEvent.cancelEventFlag) {
                        $(this).trigger('chipSwipe');
                    }
                }

            }
        });

        $(this).bind(C_TOUCH_END, function (aEvent) {

            // ムーブで入ってきた場合なので処理対象除外
            if (!touchStart) {
                return;
            }

            // ムーブした場合なので処理対象除外
            if (touchMove) {
                return;
            }

            // ホールドが完了しているので処理対象除外
            if (touchHold) {
                return;
            }
            if (holdAction != null) {
                clearTimeout(holdAction);
            }

            // 初期化
            touchStart = false;
            touchMove = false;
            touchHold = false;

            if(!$.fn.setChipEvent.cancelEventFlag) {
                $(this).trigger("chipTap", eventTarget);
            }

        });

    }

    var touchAndonAction = null;
    var touchAndonFlag = false;
    var touchAndonInterval = 200; // アンドン連続タップ禁止時間 : ms

    // アンドン共通処理
    // ハイライト対象チップ  z-index:15.25
    // フリックエリア        z-index:15
    $.fn.setAndonChipEvent = function (flickArea, highlightChip) {

        $(this).bind(C_TOUCH_START, function () {

            if (touchAndonFlag) {
                // フラグが立って入るときは処理をしない
                return;
            }

            // フラグを立て、連続タップを防止
            touchAndonFlag = true;

            var selectStatusChip = $(this).parents('div.StatusChip');

            // 既に選択されている (ハイライト時)
            if (selectStatusChip.hasClass('selectArea')) {
                hideOverlay();
                parent.lockReset();
                // フラグを落とす
                touchAndonFlag = false;
                return;
            }

            parent.lock();
            
            // アンドン、ハイライトチップ、フリックエリアのz-Indexを設定
            selectStatusChip.add(highlightChip).addClass('selectArea');
            flickArea.addClass('selectFlickArea');

            // グレーアウト処理
            showOverlayGray();

            touchAndonAction = setTimeout(function () {
                // フラグを落とし、タイマーをクリア
                touchAndonFlag = false;
                clearTimeout(touchAndonAction);
            }, touchAndonInterval);
        });

        return $(this);
    }

    // フリックページ移動
    $.fn.flickPage = function (aOptions) {

        var options = $.extend({
            page: 1,            // 移動するページ
            section: null,      // ページ要素
            duration: 250,      // 遷移のスピード
            flickEnd: null      // フリック終了時コールバック関数
        }, aOptions);

        var pageWidth = options.section.filter(':eq(0)').width();

        var scrollPosX = (options.page - 1) * pageWidth;

        $(this).animate({
            scrollLeft: scrollPosX + 'px'
        }, {
            duration: options.duration,
            easing: 'swing',
            complete: function () {
                if (options.flickEnd != null && typeof options.flickEnd == 'function') {
                    options.flickEnd();
                }
            }
        });

        return $(this);
    }

    // ドラッグ&ドロップ
    $.fn.dragAndDrop = function (aOptions) {

		var options = $.extend({
            event: null,                // イベントオブジェクト
            dragObject: $(this),        // ドラッグ対象オブジェクト
            zIndex: 0,                  // ドラッグ移動中の要素のz-index
            dragOpacity: 0.9,           // ドラッグ移動中の要素のopacity 
            dropOptionList: [{
                dropElement: null,      // ドロップされる要素
                dropHoverClass: null,   // ドロップされる要素の移動中に設定するクラス
                dropArea: null          // ドロップされる要素のエリア（省略可能）
            }],
            dragStart: null,            // ドラッグ開始時コールバック関数
            dragEnd: null,              // ドラッグ終了時コールバック関数
            offsetX: 0,                 // オフセットX
            offsetY: 0,                 // オフセットY
            initX: 5,                   // 初期移動量X
            initY: 1                    // 初期移動量Y
		}, aOptions);

        var dropObject = null;

        $.fn.dragAndDrop.dragHelper = options.dragObject.clone();
        $.fn.dragAndDrop.dragHelper.attr('id', 'dragChip').css({ 
            opacity: options.dragOpacity, 
            zIndex: options.zIndex, 
            position: 'absolute', 
            top: (options.dragObject.offset().top + options.offsetY + options.initY) + "px", 
            left: (options.dragObject.offset().left + options.offsetX + options.initX) + "px"
        });

        if (options.dragStart != null && typeof options.dragStart == 'function') {
            options.dragStart(event);
        }

        // 親フレームのBODY末尾に追加
        $(parent.document.body).append($.fn.dragAndDrop.dragHelper);

        var startPageX = gIsTouch ? options.event.originalEvent.touches[0].pageX : options.event.pageX;
        var startPageY = gIsTouch ? options.event.originalEvent.touches[0].pageY : options.event.pageY;

        var positionX = startPageX - options.dragObject.offset().left - options.initX;
        var positionY = startPageY - options.dragObject.offset().top - options.initY;

        var movePageX;
        var movePageY;

        $(document).bind(C_TOUCH_MOVE + ".dragAndDrop", function (aEvent) {

            movePageX = gIsTouch ? aEvent.originalEvent.touches[0].pageX : aEvent.pageX;
            movePageY = gIsTouch ? aEvent.originalEvent.touches[0].pageY : aEvent.pageY;

            var top = movePageY + options.offsetY - positionY;
            var left = movePageX + options.offsetX - positionX;
            
            $.fn.dragAndDrop.dragHelper.css({ 
                top: top + "px", 
                left: left + "px" });

            dropObject = null;
            var isSearch = false;

            // ドロップされる要素のスタイルを操作する
            $.each(options.dropOptionList, function (aIndex, aValue) {

                isSearch = false;
                    
                if (dropObject == null) {

                    // エリア判定
                    if (aValue.dropArea != null) {
                        // $02 start 新車タブレットショールーム管理機能開発
                        // 指定エリア分繰り替えす
                        aValue.dropArea.each(function () {
                            // X軸が範囲内
                            if ($(this).offset().left <= movePageX && movePageX <= ($(this).offset().left + $(this).width())) {
                                // Y軸が範囲内
                                if ($(this).offset().top <= movePageY && movePageY <= ($(this).offset().top + $(this).height())) {
                                    isSearch = true;
                                }
                            }
                        });
                        // $02 end   新車タブレットショールーム管理機能開発
                    }
                    else {
                        isSearch = true;
                    }
                }

                if (isSearch) {

                    // 要素数分繰り返す
                    aValue.dropElement.each(function () {

                        // X軸が範囲内
                        if ($(this).offset().left <= movePageX && movePageX <= ($(this).offset().left + $(this).width())) {
                            // Y軸が範囲内
                            if ($(this).offset().top <= movePageY && movePageY <= ($(this).offset().top + $(this).height())) {
                                dropObject = $(this);
                            }
                        }

                    });

                }

                // チップが存在しない場合
                if (dropObject == null || dropObject.length == 0) {
                    
                    $('.' + aValue.dropHoverClass).removeClass(aValue.dropHoverClass);
                }
                // 他のチップに選択が移動した場合
                else if (!dropObject.hasClass(aValue.dropHoverClass)) {

                    $('.' + aValue.dropHoverClass).removeClass(aValue.dropHoverClass);
                   
                    // $02 start 新車タブレットショールーム管理機能開発
                    dropObject.parents('.CassetteBack').addClass(aValue.dropHoverClass);
                    // $02 end   新車タブレットショールーム管理機能開発

                } 
            });

        });

        $(document).bind(C_TOUCH_END + ".dragAndDrop", function (aEvent) {

            endDrop(aEvent);
        });

        $(document).bind(C_TOUCH_START + ".dragAndDrop", function (aEvent) {

            dropObject = null;

            endDrop(aEvent);
        });

        function endDrop(aEvent) {
        
            //バインド解除
            $(document).unbind(C_TOUCH_MOVE + ".dragAndDrop");
            $(document).unbind(C_TOUCH_END + ".dragAndDrop");
            $(document).unbind(C_TOUCH_START + ".dragAndDrop");

            // ドロップされる要素のスタイルを元に戻す
            $.each(options.dropOptionList, function (aIndex, aValue) {

                $('.' + aValue.dropHoverClass).removeClass(aValue.dropHoverClass);
            });

            if (dropObject != null) {

                // 吸い込まれるように消えるアニメーション
                $.fn.dragAndDrop.dragHelper.css({
                    "-webkit-transition" : "all linear 400ms",
                    "transform": "scale(0.0, 0.0)",
                    top: dropObject.offset().top + (dropObject.height() / 2) - ($.fn.dragAndDrop.dragHelper.height() / 2) + options.offsetY,
                    left: dropObject.offset().left + (dropObject.width() / 2) - ($.fn.dragAndDrop.dragHelper.width() / 2) + options.offsetX
                });
                $.fn.dragAndDrop.dragHelper.one('webkitTransitionEnd', function () {
                    $.fn.dragAndDrop.dragHelper.remove();
                    $.fn.dragAndDrop.dragHelper = null;
                });

                if (options.dragEnd != null && typeof options.dragEnd == 'function') {
                    options.dragEnd(aEvent, options.dragObject, dropObject);
                }

            }
            else {

                $.fn.dragAndDrop.dragHelper.css({
                    "webkit-transition": "400ms ease-out",
                    "top": options.dragObject.offset().top + options.offsetY,
                    "left": options.dragObject.offset().left + options.offsetX
                });
                $.fn.dragAndDrop.dragHelper.one("webkitTransitionEnd", function (e) {
                    $.fn.dragAndDrop.dragHelper.remove();
                    $.fn.dragAndDrop.dragHelper = null;

                    if (options.dragEnd != null && typeof options.dragEnd == 'function') {
                        options.dragEnd(aEvent, options.dragObject, dropObject);
                    }
                });
            }

        }

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

    var result = minutes + '’' + secondDisp + '”';

    if (result.length > aMaxLength) {
        result = result.substr(0, aMaxLength);
    }

    return result;
}
