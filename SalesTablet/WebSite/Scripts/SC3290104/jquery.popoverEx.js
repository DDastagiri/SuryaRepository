/** 
 * @fileOverview フォロー設定のポップアップ表示処理を記述するファイル.
 * 
 * @author t.mizumoto
 * @version 1.0.0
 */
// Modified from version taken -> https://github.com/juggy/jquery-popover

(function ($) {
    $.fn.popoverEx = function (options) {
        var KEY_ESC = 27;

        // settings stored options and state
        var settings = $.extend({
            id: '', 					// id for created popover
            openEvent: null, // callback function to be called when popup opened
            closeEvent: null, // callback function to be called when popup closed
            offsetX: 0, 			// fixed offset to correct popup X position
            offsetY: 0, 			// fixed offset to correct popup Y position
            zindex: 100000, 	// default z-index value
            padding: 18, 		// default settings.padding around popover from document edges
            closeOnEsc: true, // change to false to disable ESC
            preventLeft: false, 	// pass true to prevent left popover
            preventRight: false, // pass true to prevent right popover
            preventTop: false, 	// pass true to prevent top popover
            preventBottom: false, // pass true to prevent bottom popover
            parentPopover: null,
            live: false						// popover created on live selector
        }, options || {});

        this.data("parentPopover", settings.parentPopover);
        this.data("popoverId", settings.id);

        // functions to claculate popover direction and position 

        function getNormalizedOffset(offset) {
            // t.mizumoto 調整位置について位置計算の前に反映するように修正
            //return { top: offset.top - $(window).scrollTop(), left: offset.left - $(window).scrollLeft() };
            return { top: offset.top - $(window).scrollTop() + settings.offsetY, left: offset.left - $(window).scrollLeft() + settings.offsetX };
        }

        function calcPopoverDirPossible(button, coord) {
            var possibleDir = {
                left: false,
                right: false,
                top: false,
                bottom: false
            }

            if (coord.buttonOffset.top + coord.buttonHeight + coord.triangleSize + coord.popoverHeight <=
									coord.docHeight - settings.padding) {
                possibleDir.bottom = true;
            }

            if (coord.buttonOffset.top - coord.triangleSize - coord.popoverHeight >= settings.padding) {
                possibleDir.top = true;
            }

            if (coord.buttonOffset.left + coord.buttonWidth + coord.triangleSize + coord.popoverWidth <=
									coord.docWidth - settings.padding) {
                possibleDir.right = true;
            }

            if (coord.buttonOffset.left - coord.triangleSize - coord.popoverWidth >= settings.padding) {
                possibleDir.left = true;
            }

            return possibleDir;
        }

        function chooseDir(possibleDir) {
            /*
            // remove directions prevented by settings
            if (settings.preventBottom)
            possibleDir.bottom = false;
            if (settings.preventTop)
            possibleDir.top = false;
            if (settings.preventLeft)
            possibleDir.left = false;
            if (settings.preventRight)
            possibleDir.right = false;

            // determine default direction if nothing works out
            // make sure it is not one of the prevented directions
            var dir = 'right';
            if (settings.preventRight)
            dir = 'bottom';
            if (settings.preventBottom)
            dir = 'top';
            if (settings.preventTop)
            dir = 'left';

            if (possibleDir.right)
            dir = 'right';
            else if (possibleDir.bottom)
            dir = 'bottom';
            else if (possibleDir.left)
            dir = 'left';
            else if (possibleDir.top)
            dir = 'top';
            */

            // t.mizumoto 強制的に指定した位置に表示できるように修正
            var dir = '';

            if (settings.preventLeft) {
                dir = 'left';
            }
            else if (settings.preventRight) {
                dir = 'right';
            }
            else if (settings.preventTop) {
                dir = 'top';
            }
            else {
                dir = 'bottom';
            }

            return dir;
        }

        function calcPopoverPos(button) {
            // Set this first for the layout calculations to work.
            settings.popover$.css('display', 'block');

            var coord = {
                popoverDir: 'bottom',
                popoverX: 0,
                popoverY: 0,
                deltaX: 0,
                deltaY: 0,
                triangleX: 0,
                triangleY: 0,
                triangleSize: 15, // needs to be updated if triangle changed in css
                docWidth: $(window).width(),
                docHeight: $(window).height(),
                popoverWidth: settings.popover$.outerWidth(),
                popoverHeight: settings.popover$.outerHeight(),
                buttonWidth: button.outerWidth(),
                buttonHeight: button.outerHeight(),
                buttonOffset: getNormalizedOffset(button.offset())
            }

            // calculate the possible directions based on popover size and button position
            var possibleDir = calcPopoverDirPossible(button, coord);

            // choose selected direction
            coord.popoverDir = chooseDir(possibleDir);

            // Calculate popover top
            if (coord.popoverDir == 'bottom')
                coord.popoverY = coord.buttonOffset.top + coord.buttonHeight + coord.triangleSize;
            else if (coord.popoverDir == 'top')
                coord.popoverY = coord.buttonOffset.top - coord.triangleSize - coord.popoverHeight;
            else // same Y for left & right
                coord.popoverY = coord.buttonOffset.top + (coord.buttonHeight - coord.popoverHeight) / 2;

            // Calculate popover left
            if ((coord.popoverDir == 'bottom') || (coord.popoverDir == 'top')) {

                coord.popoverX = coord.buttonOffset.left + (coord.buttonWidth - coord.popoverWidth) / 2;

                if (coord.popoverX < settings.padding) {
                    // out of the document at left
                    coord.deltaX = coord.popoverX - settings.padding;
                } else if (coord.popoverX + coord.popoverWidth > coord.docWidth - settings.padding) {
                    // out of the document right
                    coord.deltaX = coord.popoverX + coord.popoverWidth - coord.docWidth + settings.padding;
                }

                // calc triangle pos
                coord.triangleX = coord.popoverWidth / 2 - coord.triangleSize + coord.deltaX;
                coord.triangleY = 0;
            }
            else {	// left or right direction

                if (coord.popoverDir == 'right')
                    coord.popoverX = coord.buttonOffset.left + coord.buttonWidth + coord.triangleSize;
                else // left
                    coord.popoverX = coord.buttonOffset.left - coord.triangleSize - coord.popoverWidth;

                if (coord.popoverY < settings.padding) {
                    // out of the document at top
                    coord.deltaY = coord.popoverY - settings.padding;
                } else if (coord.popoverY + coord.popoverHeight > coord.docHeight - settings.padding) {
                    // out of the document bottom
                    coord.deltaY = coord.popoverY + coord.popoverHeight - coord.docHeight + settings.padding;
                }

                // calc triangle pos
                coord.triangleX = 0;
                coord.triangleY = coord.popoverHeight / 2 - coord.triangleSize + coord.deltaY;
            }

            return coord;
        }

        function positionPopover(coord) {
            // set the triangle class for it's direction
            settings.triangle$.removeClass("left top right bottom");
            settings.triangle$.addClass(coord.popoverDir);

            if (coord.triangleX > 0) {
                // t.mizumoto 調整位置について矢印の位置に反映
                //settings.triangle$.css('left', coord.triangleX);
                settings.triangle$.css('left', coord.triangleX - settings.offsetY);
            }

            if (coord.triangleY > 0) {
                // t.mizumoto 調整位置について矢印の位置に反映
                //settings.triangle$.css('top', coord.triangleY);
                settings.triangle$.css('top', coord.triangleY - settings.offsetY);
            }

            // position popover
            settings.popover$.css({
                top: coord.popoverY - coord.deltaY + settings.offsetY + $(window).scrollTop(),
                left: coord.popoverX - coord.deltaX + settings.offsetX + $(window).scrollLeft()
            });

            // set popover css and show it
            settings.popover$.css('z-index', settings.zindex).show();
        }

        // toggle a button popover. If show set to true do not toggle - always show
        function togglePopover(button, show) {
            var parent = null,
                parentCover = null,
                pop = null;

            // if this button popover is already open close it an return
            if ($.fn.popover.openedPopup) {
                pop = $.fn.popover.openedPopup;
                while (pop) {
                    if (pop.get(0) === button.get(0)) {
                        if (!show)
                            hideOpenPopover();
                        return;
                    }
                    pop = pop.data("parentPopover");
                }
            }

            if ($.isFunction(settings.openEvent)) {
                if (settings.openEvent(button) === false) {
                    hideOpenPopover();
                    return;
                }
            }

            // hide any open popover
            if (settings.parentPopover == null) {
                hideOpenPopover();
            }

            // reset triangle
            settings.triangle$.attr("style", "");

            // calculate all the coordinates needed for positioning the popover and position it 
            positionPopover(calcPopoverPos(button));

            //Timeout for webkit transitions to take effect
            setTimeout(function () {
                settings.popover$.addClass("active");
            }, 0);

            $.fn.popover.openedPopup = button;
            button.addClass('popover-on');
            $("#bodyFrame").trigger('popoverOpened');


            //When parent popover is clicked, child control is closed.
            parentCover = button.data("parentCover");
            if (parentCover) {
                parentCover.remove();
                button.data("parentCover", null);
            }
            parent = getParentPopover();
            if (parent) {
                parentCover = $("<div class='popover-cover' style='position:absolute;top:0px;left:0px;'></div>");
                parentCover
                    .width(parent.width())
                    .height(parent.height())
                    .bind("click.popover", function (e) {
                        hidePopover(button);
                        return false;
                    })
                    .appendTo(parent);
                button.data("parentCover", parentCover);
            }
        }

        // hide a button popover
        function hidePopover(button) {
            if ($.isFunction(settings.closeEvent)) {
                if (settings.closeEvent(button) === false) {
                    return;
                }
            }

            button.removeClass('popover-on');
            $("#bodyFrame").trigger('popoverClosed');
            settings.popover$.removeClass("active").attr("style", "").hide();
            $.fn.popover.openedPopup = settings.parentPopover || null;

            var parentCover = button.data("parentCover");
            if (parentCover) {
                parentCover.remove();
                button.data("parentCover", null);
            }
        }

        // hide the currently open popover if there is one
        function hideOpenPopover() {
            var pop = $.fn.popover.openedPopup;
            while (pop) {
                pop.trigger('hidePopover');
                pop = pop.data("parentPopover");
            }
        }

        function getParentPopover() {
            var current = $.fn.popover.openedPopup,
                parent = null,
                parentPopoverId = null,
                parentPopover = null;

            if (current) {
                parent = current.data("parentPopover");
                if (parent) {
                    parentPopoverId = parent.data("popoverId");
                    if (parentPopoverId != "") {
                        parentPopover = $("#" + parentPopoverId);
                    }
                }
            }

            return parentPopover;
        }

        // build HTML popover
        // t.mizumoto 部分更新を可能にするため、ポップアップエリアのDOM要素を直接指定するように修正
        if (settings.contentId != null && settings.contentId.length > 0) {
            settings.popover$ = settings.contentId;
        }
        else {
            // build HTML popover
            settings.popover$ = $('<div class="popover" id="' + settings.id + '">'
            //+ '<div class="triangle"></div>'
					+ '<div class="header"></div>'
					+ '<div class="content"></div>'
                    + '<div class="triangle"><div class="triangleBorder"><div class="triangleInner"></div></div></div>'
					+ '</div>').appendTo('body');
            $('.header', settings.popover$).append($(settings.header).detach());
            $('.content', settings.popover$).append($(settings.content).detach());
        }

        settings.triangle$ = $('.triangle', settings.popover$);

        // setup global document bindings
        $("#bodyFrame").unbind(".popover");

        // document click closes active popover		
        $("#bodyFrame").bind("click.popover", function (event) {
            if (($(event.target).parents(".popover").length == 0)
					&& (!$(event.target).hasClass('popover-button'))) {
                hideOpenPopover();
            }
        });

        // document hidePopover event causes active popover to close
        $("#bodyFrame").bind("hideOpenPopover.popover", hideOpenPopover);

        // document Esc key listener
        var selector = this;
        if (settings.closeOnEsc) {
            $("#bodyFrame").bind('keydown', function (event) {
                if (!event.altKey && !event.ctrlKey && !event.shiftKey
							&& (event.keyCode == KEY_ESC)) {
                    selector.trigger('hidePopover');
                }
            });
        }

        // setup callbacks for each popover button in wrapped set & return wrapped set
        if (!settings.live) {
            return this.each(function () {
                var button = $(this);
                button.addClass("popover-button");

                button.bind('click', function () {
                    // t.mizumoto Clickイベントとは別のイベントで表示できるように、処理を無効にしておく
                    //togglePopover(button);
                    return false;
                });

                button.bind('showPopover', function () {
                    hideOpenPopover();
                    togglePopover(button, true);
                    return false;
                });

                button.bind('hidePopover', function () {
                    hidePopover(button);
                    return false;
                });
            });
        }
        else { // live popover		
            this.live('click', function (event) {
                $(event.target).addClass("popover-button");
                togglePopover($(event.target));
                return false;
            });

            this.live('showPopover', function (event) {
                hideOpenPopover();
                $(event.target).addClass("popover-button");
                togglePopover($(event.target), true);
                return false;
            });

            this.live('hidePopover', function (event) {
                hidePopover($(event.target));
                return false;
            });

            return this;
        }
    };
})(jQuery);
