//VScroll
(function ($) {
    var pluginName = "VScroll",
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
        },

        create: function (options, elem) {
            //constructor
            var self = this,
				element = $(elem);

            this.elem = elem;
            this.originalElement = element;
            this.originalElement
                .bind("mousedown touchstart", function (e) {
                    $(this).data("prevY", e.pageY);
                    icropScript.ui.bypassPreventDefault = true;
                })
                .bind("mousemove touchmove", function (e) {
                    var prevY = $(this).data("prevY"),
                        stop = $(this).scrollTop(),
                        smin = 0,
                        smax = $(this).children(".VScroll-inner").height() - $(this).height();

                    if (stop <= smin) {
                        icropScript.ui.bypassPreventDefault = ((e.pageY - prevY) < 0);
                    } else if (smax <= stop) {
                        icropScript.ui.bypassPreventDefault = (0 < (e.pageY - prevY));
                    } else {
                        icropScript.ui.bypassPreventDefault = true;
                    }
                    $(this).data("prevY", e.pageY);
                })
                .css({
                    "overflow": "scroll",
                    "-overflow:scroll": "touch"
                })
                .wrapInner("<div class='VScroll-inner'></div>");

            this.options = $.extend(true, {}, this.options, options);

        },

        init: function () {
            //reload options
        }
    };
    pluginImpl.prototype.create.prototype = pluginImpl.prototype;
})(jQuery);