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
                wrapperElement = $("<div style='position:relative; display: inline-block;'></div>"),
                switchElement = $("<div class='icrop-SwitchButton-switch' style='display:block; position:absolute; z-index:1; background-color:black;'></div>"),
                switchElementWidth = (element.width() / 2),
                onLabelElement = $("<span style='display:inline-block;position:absolute;text-align:center;'></span>"),
                offLabelElement = $("<span style='display:inline-block;position:absolute;text-align:center;'></span>");

            this.originalElement = element;
            this.options = $.extend(true, {}, this.options, options);

            wrapperElement
				.bind("click." + pluginName, function (e) {
				    if (!element.hasClass("icrop-SwitchButton-selected")) {
				        element
                            .addClass("icrop-SwitchButton-selected")
                            .attr("checked", "checked");
				        switchElement.animate({ left: switchElementWidth }, 500);
				        if (self.options.check) {
				            self.options.check(true);
				        }
				    } else {
				        element
                            .removeClass("icrop-SwitchButton-selected")
                            .removeAttr("checked");
				        switchElement.animate({ left: 0 }, 500);
				        if (self.options.check) {
				            self.options.check(false);
				        }
				    }
				})
                .width(element.width())
                .height(element.height());
            switchElement
                .width(switchElementWidth)
                .height(element.height());
            onLabelElement
                .text(this.options.onLabel)
                .width(switchElementWidth)
                .css("left", 0);
            offLabelElement
                .text(this.options.offLabel)
                .width(switchElementWidth)
                .css("left", switchElementWidth);

            element
                .wrap(wrapperElement)
                .after(onLabelElement)
                .after(offLabelElement)
                .after(switchElement)
				.addClass("icrop-SwitchButton")
				.css({
				    "-webkit-appearance": "none",
				    "position": "absolute"
				});

            if (element.attr("checked")) {
                element.addClass("icrop-SwitchButton-selected");
                switchElement.animate({ left: switchElementWidth }, 500);
            } else {
                switchElement.animate({ left: 0 }, 500);
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