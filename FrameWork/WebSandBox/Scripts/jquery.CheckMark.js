//CheckMark (checkbox)
(function ($) {
    var pluginName = "CheckMark",
		pluginImpl;

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
            onIconUrl: null,
            offIconUrl: null,
            check: null
        },

        create: function (options, elem) {
            //constructor
            var self = this,
				element = $(elem);

            this.originalElement = element;
            this.options = $.extend(true, {}, this.options, options);

            element
				.bind("click." + pluginName, function (e) {
				    var checkValue = $(this).attr("checked")
				    if (!element.hasClass("icrop-CheckMark-checked")) {
				        element
							.attr("checked", "checked")
							.addClass("icrop-CheckMark-checked");
				        if (self.options.onIconUrl) {
				            element.css("background-image", "url(" + self.options.onIconUrl + ")");
				        }
				        if (self.options.check) {
				            self.options.check(true);
				        }
				    } else {
				        element
							.removeAttr("checked")
							.removeClass("icrop-CheckMark-checked");
				        if (self.options.offIconUrl) {
				            element.css("background-image", "url(" + self.options.offIconUrl + ")");
				        } else {
				            element.css("background-image", "");
				        }
				        if (self.options.check) {
				            self.options.check(false);
				        }
				    }
				})
				.addClass("icrop-CheckMark")
				.css("-webkit-appearance", "none");

            if (element.attr("checked")) {
                element.addClass("icrop-CheckMark-checked");
                if (this.options.onIconUrl) {
                    element.css("background-image", "url(" + this.options.onIconUrl + ")");
                }
            } else {
                if (this.options.offIconUrl) {
                    element.css("background-image", "url(" + this.options.offIconUrl + ")");
                } else {
                    element.css("background-image", "");
                }
            }
        },

        init: function () {
            //reload options
        },

        disabled: function (value) {
            if (value === undefined) {
                //getter
                return this.originalElement.hasClass("icrop-disabled");
            } else {
                //setter
                if (value === true) {
                    this.originalElement.addClass("icrop-disabled");
                } else {
                    this.originalElement.removeClass("icrop-disabled");
                }
            }
        }

    };
    pluginImpl.prototype.create.prototype = pluginImpl.prototype;
})(jQuery);