//CustomRepeater
(function ($) {
    var pluginName = "CustomRepeater",
		pluginImpl;

    $[pluginName] = {
        getCallbackArguments: function (id) {
            return $.toJSON($("#" + id).data(pluginName).callbackArguments);
        },
        getCallbackResponseFromServer: function (jsonString, id) {
            //JSON形式の文字列を変換
            var result = $.parseJSON(jsonString),
                self = $("#" + id).data(pluginName);

            if (self.callbackResponseHandler) {
                self.callbackResponseHandler(result);
            }
        }
    };

    $.fn[pluginName] = pluginImpl = function (options) {
        if (typeof options == "string") {
            // method call
            var args = Array.prototype.slice.call(arguments, 1),
				returnValue = this;
            this.each(function () {
                var instance = $.data(this, pluginName),
					value = (instance && $.isFunction(instance[options])) ? instance[options].apply(instance, args) : instance;
                if (value !== instance && value !== undefined) {
                    returnValue = value;
                    return false;
                }
            });
            return returnValue;
        } else {
            // constructor call (create or init)
            return this.each(function () {
                var instance = $.data(this, pluginName);
                if (instance) {
                    $.extend(true, instance.options, options)
                    instance.init();
                } else {
                    $.data(this, pluginName, new pluginImpl.prototype.create(options, this));
                }
            });
        }
    };

    pluginImpl.prototype = {
        options: {
            //default option values 
            load: null,                 // table load(this, rowIndex, rewind)
            render: null,               // void render(row, view)
            loadCallbackResponse: null,  // server side control only
            callbackToServer: null,     // server side control only
            pageRows: 100,
            maxCacheRows: 500,
            defaultPage: 1,
            rewindPagerLabel: "Previous",
            forwardPagerLabel: "Next",
            criteria: "",
            preventMoveEvent: true
        },

        getPosition: function (elem) {
            var attr = elem.get(0).style["transform"];
            var m = attr.match(/translate3d\((.+)px,\s*(.+)px,\s*(.+)px\)/);
            return { top: parseInt(m[2]), left: parseInt(m[1]) };
        },

        setPosition: function (elem, top, left, duration, completion) {
            var animationTimer, passed, currentTop, step;
            if (duration) {
                currentTop = this.getPosition(elem).top;
                step = (top - currentTop) / (duration / 50);
                passed = 0;
                animationTimer = setInterval(function () {
                    passed += 50;
                    if (duration < passed) {
                        clearInterval(animationTimer);
                        if (completion) {
                            completion();
                        }
                    } else {
                        currentTop += step;
                        elem.css("transform", "translate3d(" + left + "px, " + currentTop + "px, 0px)");
                    }
                }, 50);
            } else {
                elem.css("transform", "translate3d(" + left + "px, " + top + "px, 0px)");
            }
        },

        create: function (options, elem) {
            //constructor
            var self = this,
				element = $(elem),
                data = {},
                progressIcon;

            this.elem = elem;
            this.originalElement = element;
            this.originalElement
                .bind("mousedown touchstart", data, function (e) {
                    var dragArgs = e.data,
                        innerPosition = self.getPosition(self.inner);
                    rewindTop = {
                        min: (self.innerTop.height()) * -1,
                        max: (self.innerTop.height() - self.innerTopPager.height()) * -1
                    },
                    forwardTop = {
                        min: (self.innerTop.height() + self.innerTopPager.height() + (self.innerBottomPager.height() * 2) + Math.max(0, self.innerContent.height() - self.originalElement.height())) * -1,
                        max: (self.innerTop.height() + self.innerTopPager.height() + (self.innerBottomPager.height() * 1) + Math.max(0, self.innerContent.height() - self.originalElement.height())) * -1
                    };

                    dragArgs.self = self;
                    dragArgs.mode = ""; //scroll, scrol or rewind, scroll or forward
                    dragArgs.innerContentTop = (self.innerTop.height() + self.innerTopPager.height());
                    dragArgs.beginTop = innerPosition.top;
                    dragArgs.prevTop = dragArgs.beginTop
                    dragArgs.prevY = null;
                    dragArgs.accelY = 0;
                    dragArgs.reload = false;


                    if ((0 < self.innerTopPager.height()) && (rewindTop.min - 10) < innerPosition.top && innerPosition.top < (rewindTop.max + 10)) {
                        dragArgs.mode = "rewind";
                    } else if ((0 < self.innerBottomPager.height()) && (forwardTop.min - 10) < innerPosition.top && innerPosition.top < (forwardTop.max + 10)) {
                        dragArgs.mode = "forward";
                    } else {
                        dragArgs.mode = "scroll";
                    }

                    //スクロールバーのリサイズ
                    if (self.resizeScrollBar(dragArgs)) {
                        dragArgs.scrollBar.fadeIn(200);
                    }

                    if (self.scrollTimer) {
                        clearInterval(self.scrollTimer);
                        self.scrollTimer = null;
                    }

                    $("#bodyFrame")
                        .unbind(".CustomRepeater")
                        .bind("mousemove.CustomRepeater touchmove.CustomRepeater", dragArgs, self.mousemove)
                        .bind("mouseup.CustomRepeater touchend.CustomRepeater", dragArgs, self.mouseup);

                })
                .css({
                    "position": "relative",
                    "overflow": "hidden"
                });

            this.options = $.extend(true, {}, this.options, options);

            this.firstPage = this.lastPage = this.options.defaultPage;

            this.cache = [];
            this.cacheTopIndex = -1;
            this.cacheBottomIndex = -1;

            this.progress = $("<div class='icrop-CustomRepeater-progress' style='position:absolute;top:0px;left:0px;'><div style='position:absolute;' class='icrop-CustomRepeater-progress-inner-icon'></div></div>").appendTo(element);
            this.progress
                .width(element.width())
                .height(element.height());

            progressIcon = this.progress.find(".icrop-CustomRepeater-progress-inner-icon");
            progressIcon.css({
                top: ((element.height() / 2) - (progressIcon.height() / 2)),
                left: ((element.width() / 2) - (progressIcon.width() / 2))
            });

            this.inner = $('<div class="icrop-CustomRepeater-inner" style="position:relative;transform:translate3d(0px,0px,0px);" />').appendTo(element);

            this.innerTop = $("<div class='icrop-CustomRepeater-inner-top'></div>");
            this.innerTop
                .width(element.width())
                .height(element.height())
                .appendTo(this.inner);

            this.innerTopPager = $("<div class='icrop-CustomRepeater-inner-topPager'><span class='icrop-CustomRepeater-inner-topPager-label'></span><span class='icrop-CustomRepeater-inner-topPager-icon' style='display:none;'></span></div>");
            this.innerTopPager
                .click(function (e) {
                    self.firstPage -= 1;
                    self.renderRewind();
                })
                .appendTo(this.inner);
            this.innerTopPager.children(".icrop-CustomRepeater-inner-topPager-label")
                .text(this.options.rewindPagerLabel);

            this.innerContent = $("<div class='icrop-CustomRepeater-inner-content'></div>");
            this.innerContent.appendTo(this.inner);

            this.innerBottomPager = $("<div class='icrop-CustomRepeater-inner-bottomPager'><span class='icrop-CustomRepeater-inner-bottomPager-label'></span><span class='icrop-CustomRepeater-inner-bottomPager-icon' style='display:none;'></span></div>");
            this.innerBottomPager
                .click(function (e) {
                    self.lastPage += 1;
                    self.renderForward();
                })
                .appendTo(this.inner);
            this.innerBottomPager.children(".icrop-CustomRepeater-inner-bottomPager-label")
                .text(this.options.forwardPagerLabel);

            this.innerBottom = $("<div class='icrop-CustomRepeater-inner-bottom'></div>");
            this.innerBottom
                .width(element.width())
                .height(element.height())
                .appendTo(this.inner);

            if (this.firstPage <= 1) {
                this.innerTopPager.height(0).hide();
            }

            self.setPosition(this.inner, (this.innerTop.height() + this.innerTopPager.height() - 10) * -1, 0);

            data.target = element;
            data.inner = this.innerContent;
            data.self = self;
            this.createScrollBar(data);


            $(function () { self.renderForward(); });
        },

        init: function () {
            //reload options
        },

        reload: function (criteria) {
            this.options.criteria = criteria;

            this.firstPage = this.lastPage = this.options.defaultPage;

            this.cache = [];
            this.cacheTopIndex = -1;
            this.cacheBottomIndex = -1;

            this.innerTopPager.height(0).hide();
            this.innerBottomPager.height(0).hide();

            this.innerContent.children("div").remove();
            this.setPosition(this.inner, (this.innerTop.height() + this.innerTopPager.height() - 10) * -1, 0);

            this.renderForward();
        },

        mousemove: function (e) {
            var dragArgs = e.data;
            if (!dragArgs) {
                return;
            }

            var self = dragArgs.self,
                deltaY = (dragArgs.prevY) ? (e.pageY - dragArgs.prevY) : 0,
                currentTop = dragArgs.prevTop + deltaY,
                rewindTop = {
                    min: (self.innerTop.height()) * -1,
                    max: (self.innerTop.height() - self.innerTopPager.height()) * -1
                },
                forwardTop = {
                    min: (self.innerTop.height() + self.innerTopPager.height() + (self.innerBottomPager.height() * 2) + Math.max(0, self.innerContent.height() - self.originalElement.height())) * -1,
                    max: (self.innerTop.height() + self.innerTopPager.height() + (self.innerBottomPager.height() * 1) + Math.max(0, self.innerContent.height() - self.originalElement.height())) * -1
                },
                degree,
                h1,
                h2;

            if (self.scrollTimer) {
                clearInterval(self.scrollTimer);
                self.scrollTimer = null;
            }

            dragArgs.accelY = deltaY;

            if (dragArgs.mode == "rewind" && (deltaY < 0)) {
                dragArgs.mode = "scroll";
            } else if (dragArgs.mode == "forward" && (0 < deltaY)) {
                dragArgs.mode = "scroll";
            }

            if (dragArgs.mode == "rewind") {
                if (rewindTop.max < currentTop) {
                    dragArgs.mode = "do rewind";
                    degree = -180;
                } else if (currentTop < rewindTop.min) {
                    dragArgs.mode = "do forward";
                    degree = 0;
                } else {
                    h1 = Math.abs(rewindTop.max - rewindTop.min);
                    h2 = Math.abs(currentTop - rewindTop.min);
                    degree = -180 * (h2 / h1);
                }
                self.innerTopPager.children('.icrop-CustomRepeater-inner-topPager-icon').show().css("transform", "rotate(" + degree + "deg)");
            } else if (dragArgs.mode == "forward") {
                if (forwardTop.max < currentTop) {
                    degree = 0;
                } else if (currentTop < forwardTop.min) {
                    dragArgs.mode = "do forward";
                    degree = 180;
                } else {
                    h1 = Math.abs(forwardTop.max - forwardTop.min);
                    h2 = Math.abs(currentTop - forwardTop.min);
                    degree = 180 * (1 - (h2 / h1));
                }
                self.innerBottomPager.children('.icrop-CustomRepeater-inner-bottomPager-icon').show().css("transform", "rotate(" + degree + "deg)");
            } else {
                //noop
            }

            self.setPosition(self.inner, currentTop, 0);
            self.resizeScrollBar(dragArgs);

            dragArgs.prevTop = currentTop;
            dragArgs.prevY = e.pageY;

            return (self.options.preventMoveEvent !== true);
        },

        mouseup: function (e) {
            var dragArgs = e.data;
            if (!dragArgs) {
                return;
            }

            var self = dragArgs.self,
                innerPosition = self.getPosition(self.inner);

            if (self.scrollTimer) {
                clearInterval(self.scrollTimer);
                self.scrollTimer = null;
            }

            if (dragArgs.mode == "do rewind") {
                self.setPosition(self.inner, dragArgs.beginTop, 0, 200, function () {
                    self.firstPage -= 1;
                    self.renderRewind();
                });
            } else if (dragArgs.mode == "do forward") {
                self.setPosition(self.inner, dragArgs.beginTop, 0, 200, function () {
                    self.lastPage += 1;
                    self.renderForward();
                });
            } else {
                //慣性スクロール
                self.accelY = dragArgs.accelY * 1.5;
                self.scrollTimer = setInterval(function () {
                    var limitTop = {
                        max: (self.innerTop.height() * -1),
                        min: ((self.innerTop.height() + self.innerTopPager.height() + Math.max(self.originalElement.height(), self.innerContent.height()) + self.innerBottomPager.height() - self.originalElement.height()) * -1)
                    },
                    accelTop = self.getPosition(self.inner).top + self.accelY;

                    if (accelTop < limitTop.min) {
                        self.setPosition(self.inner, limitTop.min + self.accelY, 0);
                    } else if (limitTop.max < accelTop) {
                        self.setPosition(self.inner, limitTop.max + self.accelY, 0);
                    } else {
                        self.setPosition(self.inner, accelTop, 0);
                    }

                    self.accelY *= 0.8;
                    if (-1 < self.accelY && self.accelY < 1) {
                        self.accelY = 0;
                        clearInterval(self.scrollTimer);
                        self.scrollTimer = null;
                        self.innerContent.children("div").css('visibility', 'visible');
                    }
                }, 50);
            }

            e.data.scrollBar.fadeOut(0);
            self.innerTopPager.children('.icrop-CustomRepeater-inner-topPager-icon').hide();
            self.innerBottomPager.children('.icrop-CustomRepeater-inner-bottomPager-icon').hide();
            $("#bodyFrame").unbind(".CustomRepeater");
        },

        renderForward: function (loadedCache) {
            var targetTopIndex = (this.lastPage - 1) * this.options.pageRows,
                targetBottomIndex = targetTopIndex + (this.options.pageRows - 1),
                purgeRowCount = 0,
                originalInnerHeight,
                innerHeightAdded = 0,
                innerHeightRemoved = 0,
                innerTopPagerDelta = 0,
                result = false;

            this.progress.show();

            if (this.cacheBottomIndex < targetBottomIndex) {
                //データ取得
                if (loadedCache === undefined) {
                    loadedCache = this.options.load.call(this.elem, this, ((this.cacheBottomIndex < 0) ? targetTopIndex : (this.cacheBottomIndex + 1)), false, this.options.criteria);
                }
                if (loadedCache === false) {
                    //非同期ロード
                    return result;
                } else if (0 < loadedCache.length) {
                    this.cache = this.cache.concat(loadedCache);
                    if (this.cacheTopIndex < 0) {
                        this.cacheTopIndex = targetTopIndex;
                        this.cacheBottomIndex = (this.cacheTopIndex + loadedCache.length - 1);
                    } else {
                        this.cacheBottomIndex += loadedCache.length;
                    }
                }
            }

            originalInnerHeight = this.inner.height();
            if (this.cacheBottomIndex < targetTopIndex) {
                //キャッシュミス（次ページなし）
                this.lastPage -= 1;
                this.renderForwardRows(-1, -1, false);
                result = false;
            } else if (this.cacheBottomIndex < targetBottomIndex) {
                //部分行キャッシュヒット（次ページなし）
                this.renderForwardRows(targetTopIndex, this.cacheBottomIndex, false);
                result = true;
            } else {
                //全行キャッシュヒット（次ページあり）
                this.renderForwardRows(targetTopIndex, targetBottomIndex, true);
                result = true;
            }
            innerHeightAdded = (this.inner.height() - originalInnerHeight);

            //maxCacheRowsを超える行を削除
            originalInnerHeight = this.inner.height();
            purgeRowCount = this.innerContent.children("div").size() - this.options.maxCacheRows;
            if (0 < purgeRowCount) {
                this.innerContent.children("div:lt(" + purgeRowCount + ")").remove();

                this.firstPage += 1;
                this.cacheTopIndex += purgeRowCount;
                this.cache.splice(0, purgeRowCount);

                if (this.innerTopPager.height() <= 0) {
                    this.innerTopPager.css("height", "").show();
                    innerTopPagerDelta = this.innerTopPager.height();
                }
            }
            innerHeightRemoved = (originalInnerHeight - this.inner.height());

            //改ページ位置に移動
            this.setPosition(this.inner, (this.getPosition(this.inner).top - innerTopPagerDelta + innerHeightRemoved), 0);
            this.setPosition(this.inner, (this.getPosition(this.inner).top - 10), 0, 500);

            //0件チェック
            if (this.cache.length == 0) {
                this.innerTopPager.height(0).hide();
                this.innerBottomPager.height(0).hide();
            }

            this.progress.hide();

            return result;
        },

        renderForwardRows: function (topIndex, bottomIndex, showPager) {
            var index, row, self = this;
            if (0 <= topIndex && 0 <= bottomIndex) {
                for (index = topIndex; index <= bottomIndex; index++) {
                    row = $("<div data-Index='" + index + "'></div>");
                    row.bind("mousedown touchstart", function (e) {
                        var i,
                            currentIndex = parseInt($(this).attr("data-index")),
                            minIndex = Math.max(parseInt(self.innerContent.children("div:first").attr("data-index")), currentIndex - 10),
                            maxIndex = Math.min(parseInt(self.innerContent.children("div:last").attr("data-index")), currentIndex + 10);

                        self.innerContent.children("div").css('visibility', 'hidden');
                        for (i = minIndex; i <= maxIndex; i++) {
                            self.innerContent.children("div[data-Index='" + i + "']").css('visibility', 'visible');
                        }
                    });
                    this.options.render.call(this.elem, this.cache[index - this.cacheTopIndex], row);
                    this.innerContent.append(row);
                }
            }
            if (showPager) {
                this.innerBottomPager.css("height", "").show();
            } else {
                this.innerBottomPager.height(0).hide();
            }
        },

        renderRewind: function (loadedCache) {
            var targetTopIndex = (this.firstPage - 1) * this.options.pageRows,
                targetBottomIndex = targetTopIndex + (this.options.pageRows - 1),
                purgeRowCount = 0,
                originalInnerHeight,
                innerHeightAdded = 0,
                innerHeightRemoved = 0,
                innerTopPagerDelta = 0,
                result = false;

            if (this.cacheTopIndex <= targetBottomIndex) {
                targetBottomIndex = this.cacheTopIndex - 1;
            }

            this.progress.show();

            if (targetTopIndex < this.cacheTopIndex) {
                //データ取得
                if (loadedCache === undefined) {
                    loadedCache = this.options.load.call(this.elem, this, ((this.cacheTopIndex < 0) ? targetBottomIndex : (this.cacheTopIndex - 1)), true, this.options.criteria);
                }
                if (loadedCache === false) {
                    //非同期ロード
                    return result;
                } else if (0 < loadedCache.length) {
                    this.cache = loadedCache.concat(this.cache);
                    if (this.cacheBottomIndex < 0) {
                        this.cacheBottomIndex = targetBottomIndex;
                        this.cacheTopIndex = (this.cacheBottomIndex - loadedCache.length + 1);
                    } else {
                        this.cacheTopIndex -= loadedCache.length;
                    }
                }
            }

            originalInnerHeight = this.inner.height();
            if (targetBottomIndex < this.cacheTopIndex) {
                //キャッシュミス（前ページなし）
                this.firstPage += 1;
                this.renderRewindRows(-1, -1, false);
                result = false;
            } else if (targetTopIndex < this.cacheTopIndex) {
                //部分行キャッシュヒット（前ページなし）
                this.renderRewindRows(this.cacheTopIndex, targetBottomIndex, false);
                result = true;
            } else {
                //全行キャッシュヒット（前ページありかも）
                this.renderRewindRows(targetTopIndex, targetBottomIndex, (1 < this.firstPage));
                result = true;
            }
            innerHeightAdded = (this.inner.height() - originalInnerHeight);

            //maxCacheRowsを超える行を削除
            originalInnerHeight = this.inner.height();
            purgeRowCount = this.innerContent.children("div").size() - this.options.maxCacheRows;
            if (0 < purgeRowCount) {
                this.innerContent.children("div:gt(" + (this.options.maxCacheRows - 1) + ")").remove();

                this.lastPage -= 1;
                this.cacheBottomIndex -= purgeRowCount;
                this.cache.splice((this.cache.length - purgeRowCount), purgeRowCount);

                this.innerBottomPager.css("height", "").show();
            }
            innerHeightRemoved = (originalInnerHeight - this.inner.height());

            //改ページ位置に移動
            this.setPosition(this.inner, (this.getPosition(this.inner).top - innerHeightAdded), 0);

            //0件チェック
            if (this.cache.length == 0) {
                this.innerTopPager.height(0).hide();
                this.innerBottomPager.height(0).hide();
            }

            this.progress.hide();

            return result;
        },

        renderRewindRows: function (topIndex, bottomIndex, showPager) {
            var index, row, self = this;
            if (0 <= topIndex && 0 <= bottomIndex) {
                for (index = bottomIndex; topIndex <= index; index--) {
                    row = $("<div data-Index='" + index + "'></div>");
                    row.bind("mousedown touchstart", function (e) {
                        var i,
                            currentIndex = parseInt($(this).attr("data-index")),
                            minIndex = Math.max(parseInt(self.innerContent.children("div:first").attr("data-index")), currentIndex - 10),
                            maxIndex = Math.min(parseInt(self.innerContent.children("div:last").attr("data-index")), currentIndex + 10);

                        self.innerContent.children("div").css('visibility', 'hidden');
                        for (i = minIndex; i <= maxIndex; i++) {
                            self.innerContent.children("div[data-Index='" + i + "']").css('visibility', 'visible');
                        }
                    });
                    this.options.render.call(this.elem, this.cache[index - this.cacheTopIndex], row);
                    this.innerContent.prepend(row);
                }
            }
            if (showPager) {
                this.innerTopPager.css("height", "").show();
            } else {
                this.innerTopPager.height(0).hide();
            }
        },

        loadCallbackResponse: function (result, rewind) {
            if (this.options.loadCallbackResponse) {
                this.options.loadCallbackResponse.call(this.elem, result);
            }
            if (rewind) {
                this.renderRewind(result.rows);
            } else {
                this.renderForward(result.rows);
            }
        },

        callbackServer: function (args, callback) {
            if (this.options.callbackToServer) {
                this.callbackArguments = args;
                this.callbackResponseHandler = callback;
                this.options.callbackToServer.call(this.elem);
            }
        },

        createScrollBar: function (data) {
            if ($(".scroll-bar", data.target).length == 0) data.target.append('<div class="scroll-bar" />');
            var $bar = $(".scroll-bar", data.target).css({
                "position": "absolute",
                "border": "1px solid #777",
                "border-radius": "5px",
                "background": "rgba(100,100,100,0.8)",
                "width": "5px",
                "transform": "translate3d(0px,0px,0px)",
                "top": "0px",
                "right": "0px",
                "display": "none",
                "box-sizing": "border-box"
            });
            data.scrollBar = $bar;
        },

        resizeScrollBar: function (data) {
            var scrollH = data.target.height(), dataH = data.inner.height(), scrollBarH;
            var rate = scrollH > dataH ? 1 : scrollH / dataH;
            var top = (data.prevTop - data.beginTop + (data.beginTop + data.innerContentTop)) * rate * -1;
            //バーの高さを求める(規定値以下のスクロールバーの高さになるのであれば、規定値にする)
            data.scrollBar.height(Math.max(Math.ceil(scrollH * rate), 20));
            this.setPosition(data.scrollBar, top, 0);

            //スクロールが必要ならTrue、それ以外はFalse
            return rate !== 1;
        }
    };
    pluginImpl.prototype.create.prototype = pluginImpl.prototype;
})(jQuery);