//SwitchButton (checkbox)
(function ($) {
    var pluginName = "SwitchButton",
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
            onLabel: "On",
            offLabel: "Off",
            check: null
        },

        create: function (options, elem) {
            //constructor
            var self = this,
				element = $(elem),
                wrapperElement = $("<div class='icrop-SwitchButton'></div>"),
                backgroundElement = $("<div class='icrop-SwitchButton-background'></div>"),
                onLabelElement = $("<span class='icrop-SwitchButton-label-on'></span>"),
                offLabelElement = $("<span class='icrop-SwitchButton-label-off'></span>");

            this.elem = elem;
            this.originalElement = element;
            this.options = $.extend(true, {}, this.options, options);


            wrapperElement
                .bind("click." + pluginName, function (e) {
                    if (self.disabled()) {
                        return false;
                    }
                    onLabelElement.hide();
                    offLabelElement.hide();
                    if (!element.hasClass("icrop-SwitchButton-selected")) {
                        element
                            .addClass("icrop-SwitchButton-selected")
                            .attr("checked", "checked");

                        backgroundElement.animate({ left: 0 }, "fast", "linear", function () {
                            onLabelElement.show();
                            if (self.options.check) {
                                self.options.check.call(self.elem, true);
                            }
                        });
                    } else {
                        element
                            .removeClass("icrop-SwitchButton-selected")
                            .removeAttr("checked");

                        backgroundElement.animate({ left: -50 }, "fast", "linear", function () {
                            offLabelElement.show();
                            if (self.options.check) {
                                self.options.check.call(self.elem, false);
                            }
                        });
                    }
                })
                .addClass("icrop-SwitchButton");

            onLabelElement.text(this.options.onLabel);
            offLabelElement.text(this.options.offLabel);

            element
                .wrap(wrapperElement)
                .after(onLabelElement)
                .after(offLabelElement)
                .after(backgroundElement)
				.css({
				    "display": "none"
				});

            if (element.attr("checked")) {
                element.addClass("icrop-SwitchButton-selected");
                backgroundElement.css("left", 0);
                onLabelElement.show();
                offLabelElement.hide();
            } else {
                backgroundElement.css("left", -50);
                onLabelElement.hide();
                offLabelElement.show();
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
                    this.originalElement
						.addClass("icrop-disabled")
						.attr("disabled", "disabled");
                } else {
                    this.originalElement
						.removeClass("icrop-disabled")
						.removeAttr("disabled");
                }
            }
        }

    };
    pluginImpl.prototype.create.prototype = pluginImpl.prototype;
})(jQuery);