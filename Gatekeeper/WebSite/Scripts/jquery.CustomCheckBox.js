//CustomCheckBox (checkbox)
(function ($) {
    var pluginName = "CustomCheckBox",
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
            onIconUrl: "../Styles/Images/checkMark12.png",
            offIconUrl: "../Styles/Images/checkMark11.png",
            check: null
        },

        create: function (options, elem) {
            //constructor
            var self = this,
				element = $(elem);

            this.elem = elem;
            this.originalElement = element;
            this.options = $.extend(true, {}, this.options, options);

            this.wrapperElement = element.parent(".icrop-CustomCheckBox");
            if (this.wrapperElement.size() == 0) {
                return;
            }

            element
                .addClass("icrop-CustomCheckBox-check")
                .css("visibility", "hidden");

            this.wrapperElement
			    .bind("click." + pluginName, function (e) {
			        if (self.disabled()) {
			            return false;
			        }
			        self.value(!(self.value()));
			    });

            this.checkElement = $("<div style='position:absolute; background-repeat:no-repeat; background-position:center center;'></div>");
            this.checkElement
                .width(element.outerWidth())
                .height(element.outerHeight())
                .css({ "top": element.position().top, "left": element.position().left })
                .appendTo(this.wrapperElement);

            if (element.attr("checked")) {
                self.checkElement.css("background-image", "url(" + this.options.onIconUrl + ")");
            } else {
                self.checkElement.css("background-image", "url(" + this.options.offIconUrl + ")");
            }
        },

        init: function () {
            //reload options
        },

        value: function (checked) {
            if (checked === undefined) {
                //getter
                return this.originalElement.attr("checked");
            } else {
                //setter
                if (checked === true) {
                    this.originalElement.attr("checked", "checked");
                    this.checkElement.css("background-image", "url(" + this.options.onIconUrl + ")");
                    if (this.options.check) {
                        this.options.check.call(this.originalElement, true);
                    }

                } else {
                    this.originalElement.removeAttr("checked");
                    this.checkElement.css("background-image", "url(" + this.options.offIconUrl + ")");
                    if (this.options.check) {
                        this.options.check.call(this.originalElement, false);
                    }
                }
            }
        },

        disabled: function (value) {
            if (value === undefined) {
                //getter
                return this.wrapperElement.hasClass("icrop-disabled");
            } else {
                //setter
                if (value === true) {
                    this.originalElement.attr("disabled", "disabled");
                    this.wrapperElement.addClass("icrop-disabled");

                } else {
                    this.originalElement.removeAttr("disabled");
                    this.wrapperElement.removeClass("icrop-disabled");
                }
            }
        }

    };
    pluginImpl.prototype.create.prototype = pluginImpl.prototype;
})(jQuery);