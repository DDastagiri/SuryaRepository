//PopOverForm (composite)
(function ($) {
    var pluginName = "PopOverForm",
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
            pageCapacity: 5,
            render: null,           //callback function(index, args, container)
            open: null,             //callback function()
            close: null,            //callback function(result)
            postbackToServer: null, //server side control only
            callbackToServer: null, //server side control only
            preventLeft: false, 	// pass true to prevent left popover
            preventRight: false,    // pass true to prevent right popover
            preventTop: false, 	    // pass true to prevent top popover
            preventBottom: false    // pass true to prevent bottom popover
        },

        create: function (options, elem) {
            //constructor
            var self = this,
				element = $(elem);

            this.elem = elem;
            this.originalElement = element;
            this.options = $.extend(true, {}, this.options, options);
            this.pageIndex = -1;
            this.headerButtonStack = [];

            //flickable creation is failed if element was hidden. (display:none)
            this.headerElement = element.children(".icrop-PopOverForm-header");
            this.contentElement = element.children(".icrop-PopOverForm-content");
            this.contentElement.flickable({
                section: '.icrop-PopOverForm-page',
                elasticConstant: 0.3,
                friction: 0.96
            });

            this.headerSelector = '#' + element.attr('id') + ' > div.icrop-PopOverForm-header';
            this.contentSelector = '#' + element.attr('id') + ' > div.icrop-PopOverForm-content';

            element.addClass("popover");
            this.popoverElement = $("#" + element.attr("data-TriggerClientID"));
            this.popoverElement.popover({
                id: element.attr("id") + "_popover",
                preventLeft: self.options.preventLeft,
                preventRight: self.options.preventRight,
                preventTop: self.options.preventTop,
                preventBottom: self.options.preventBottom,
                header: this.headerSelector,
                content: this.contentSelector,
                openEvent: function () {
                    if (self.options.open) {
                        if (self.options.open.call(self.elem, self) === false) {
                            return false;
                        }
                    }
                    self.pageIndex = -1;
                    self.pushPage();
                    return true;
                },
                closeEvent: function () {
                    self.headerButtonStack = [];
                }
            });

        },

        init: function () {
            //reload options
        },

        resize: function (pageWidth, pageHeight) {
            //constructor
            var self = this,
                element = this.originalElement;

            //flickable creation is failed if element was hidden. (display:none)
            this.headerElement.css({ "width": pageWidth + "px" });
            this.contentElement.find('.icrop-PopOverForm-page').css({ "width": pageWidth + "px", "height": pageHeight + "px" });
            this.contentElement
                .css({ "width": pageWidth + "px", "height": pageHeight + "px" })
                .flickable('refresh');
        },

        pushPage: function (args) {
            var self = this,
                headerLeft = this.headerElement.find(".icrop-PopOverForm-header-left").wrapInner("<div></div>").unbind("click"),
                headerRight = this.headerElement.find(".icrop-PopOverForm-header-right").wrapInner("<div></div>").unbind("click"),
                container,
                isPageChanged = (0 <= this.pageIndex),
                headerLeftContent = headerLeft.children("div"),
                headerRightContent = headerRight.children("div");

            this.headerButtonStack.push({ left: headerLeftContent.remove(), right: headerRightContent.remove() });

            if (this.pageIndex < this.options.pageCapacity) {
                this.pageIndex += 1;
                container = $(this.contentElement.find(".icrop-PopOverForm-page").get(this.pageIndex));
                if (isPageChanged) {
                    this.contentElement.flickable('select', this.pageIndex);
                }
                this.options.render.call(this.elem, this, this.pageIndex, args, container, this.headerElement);
            }

            if (this.pageIndex == 0) {
                headerLeft.removeClass("icrop-PopOverForm-header-back");
            } else {
                headerLeft
                    .text("Back")
                    .addClass("icrop-PopOverForm-header-back")
                    .click(function (e) {
                        self.popPage();
                    });
            }
        },

        popPage: function () {
            var self = this,
                headerLeft = this.headerElement.find(".icrop-PopOverForm-header-left"),
                headerRight = this.headerElement.find(".icrop-PopOverForm-header-right"),
                headerButtons = this.headerButtonStack.pop();

            headerLeft.empty().unbind("click").append(headerButtons.left);
            headerRight.empty().unbind("click").append(headerButtons.right);

            if (0 < this.pageIndex) {
                this.pageIndex -= 1;
                this.contentElement.flickable('select', this.pageIndex);
            }

            if (this.pageIndex == 0) {
                headerLeft.removeClass("icrop-PopOverForm-header-back");
            } else {
                headerLeft
                    .text("Back")
                    .addClass("icrop-PopOverForm-header-back")
                    .click(function (e) {
                        self.popPage();
                    });
            }
        },

        closePopOver: function (result) {
            $("#bodyFrame").trigger("click.popover");
            if (this.options.close) {
                if (this.options.close.call(this.elem, this, result) && this.options.postbackToServer) {
                    $("input[name='" + this.originalElement.attr("id") + "']").val(result);
                    this.options.postbackToServer.call(this.elem);
                }
            }
        },

        callbackServer: function (args, callback) {
            if (this.options.callbackToServer) {
                this.callbackArguments = args;
                this.callbackResponseHandler = callback;
                this.options.callbackToServer.call(this.elem);
            }
        }

    };
    pluginImpl.prototype.create.prototype = pluginImpl.prototype;
})(jQuery);