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
				element = $(elem);

            this.elem = elem;
            this.originalElement = element;
            this.options = $.extend(true, {}, this.options, options);
            this.labelElement = $("<div class='icrop-CheckButton-label' style='position:absolute;'></div>");

            element
                .wrap("<span style='position:relative; display: inline-block;'></span>")
				.css({
				    "-webkit-appearance": "none"
				});


            this.wrapperElement = element.parent();
            this.wrapperElement
                .width(element.width())
                .height(element.height())
                .addClass("icrop-CheckButton")
                .doubletap(
				    function (e) {
				        if (self.disabled()) {
				            return false;
				        }
				        if (self.wrapperElement.hasClass("icrop-selected")) {
				            self.wrapperElement.removeClass("icrop-selected");
				            self.labelElement.removeClass("icrop-selected");
				            self.originalElement.removeAttr("checked");

				            if (self.options.offIconUrl) {
				                self.originalElement.css("background-image", "url(" + options.offIconUrl + ")");
				            }
				            if (self.options.check) {
				                self.options.check.call(self.elem, false);
				            }
				        }
				    },
                    function (e) {
                        if (self.disabled()) {
                            return false;
                        }
                        if (!self.wrapperElement.hasClass("icrop-selected")) {
                            self.wrapperElement.addClass("icrop-selected");
                            self.labelElement.addClass("icrop-selected");
                            self.originalElement.attr("checked", "checked");

                            if (self.options.onIconUrl) {
                                self.originalElement.css("background-image", "url(" + options.onIconUrl + ")");
                            }
                            if (self.options.check) {
                                self.options.check.call(self.elem, true);
                            }
                        }
                    });


            if (options.label) {
                this.labelElement
                    .text(options.label)
                    .width(element.width());
                element.after(this.labelElement);
            }

            if (element.attr("checked")) {
                this.wrapperElement.addClass("icrop-selected");
                this.labelElement.addClass("icrop-selected");
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
                return this.wrapperElement.hasClass("icrop-disabled");
            } else {
                //setter
                if (value === true) {
                    this.wrapperElement.addClass("icrop-disabled");
                    this.labelElement.addClass("icrop-disabled");
                    this.originalElement.attr("disabled", "disabled");

                } else {
                    this.wrapperElement.removeClass("icrop-disabled");
                    this.labelElement.removeClass("icrop-disabled");
                    this.originalElement.removeAttr("disabled");
                }
            }
        }

    };
    pluginImpl.prototype.create.prototype = pluginImpl.prototype;
})(jQuery);