//CheckButton (checkbox)
(function ($) {
    var pluginName = "CheckButton",
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
            //default option values
            offIconUrl: null,
            onIconUrl: null,
            label: null,
            check: null
        },

        create: function (options, elem) {
            //constructor
            var self = this,
				element = $(elem),
                wrapperElement = $("<div style='position:relative; display: inline-block;'></div>"),
                labelElement;

            this.originalElement = element;
            this.options = $.extend(true, {}, this.options, options);

            wrapperElement
                .width(element.width())
                .height(element.height());

            element
				.bind("click." + pluginName, function (e) {
				    if (!element.hasClass("icrop-selected")) {
				        element
                            .attr("checked", "checked")
				            .addClass("icrop-selected");
				        if (self.options.onIconUrl) {
				            element.css("background-image", "url(" + options.onIconUrl + ")");
				        }
				        if (self.options.check) {
				            self.options.check(true);
				        }
				    }
				})
				.bind("dblclick." + pluginName, function (e) {
				    if (element.hasClass("icrop-selected")) {
				        element
                            .removeAttr("checked")
                            .removeClass("icrop-selected");
				        if (self.options.offIconUrl) {
				            element.css("background-image", "url(" + options.offIconUrl + ")");
				        }
				        if (self.options.check) {
				            self.options.check(false);
				        }
				    }
				})
                .wrap(wrapperElement)
				.addClass("icrop-CheckButton")
				.css({
				    "-webkit-appearance": "none",
				    "position": "absolute"
				});

            if (options.label) {
                labelElement = $("<div class='icrop-CheckButton-label' style='position:absolute;text-align:center;'></div>");
                labelElement
                    .text(options.label)
					.width(element.width());
                element.after(labelElement);
            }

            if (element.attr("checked")) {
                if (options.onIconUrl) {
                    element.css("background-image", "url(" + options.onIconUrl + ")");
                }
            } else {
                if (options.offIconUrl) {
                    element.css("background-image", "url(" + options.offIconUrl + ")");
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