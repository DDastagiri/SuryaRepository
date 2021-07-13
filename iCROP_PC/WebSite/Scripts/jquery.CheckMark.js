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
            checkIconWidth: 30,
            checkIconHeight: 20,
            checkIconPosition: "left",
            label: "",
            check: null
        },

        create: function (options, elem) {
            //constructor
            var self = this,
				element = $(elem),
                wrapperElement,
                labelElement;

            this.elem = elem;
            this.originalElement = element;
            this.options = $.extend(true, {}, this.options, options);
            this.checkElement = $("<span style='display:inline-block; position:absolute;'></span>");

            element
                .wrap("<span style='display:inline-block;position:relative;'></span>")
                .width(this.options.checkIconWidth)
                .height(this.options.checkIconHeight)
				.css("visibility", "hidden");

            wrapperElement = element.parent();
            wrapperElement
			    .bind("click." + pluginName, function (e) {
			        if (self.disabled()) {
			            return false;
			        }
			        if (!self.checkElement.hasClass("icrop-CheckMark-checked")) {
			            element.attr("checked", "checked");
			            self.checkElement.addClass("icrop-CheckMark-checked");
			            if (self.options.onIconUrl) {
			                self.checkElement.css("background-image", "url(" + self.options.onIconUrl + ")");
			            }
			            if (self.options.check) {
			                self.options.check.call(self.elem, true);
			            }
			        } else {
			            element.removeAttr("checked");
			            self.checkElement.removeClass("icrop-CheckMark-checked");
			            if (self.options.offIconUrl) {
			                self.checkElement.css("background-image", "url(" + self.options.offIconUrl + ")");
			            } else {
			                self.checkElement.css("background-image", "");
			            }
			            if (self.options.check) {
			                self.options.check.call(self.elem, false);
			            }
			        }
			    })
			    .addClass("icrop-CheckMark");

            if (this.options.label) {
                labelElement = $("<span class='icrop-CheckMark-label'></span>");
                labelElement.text(this.options.label);
                if (this.options.position == "left") {
                    element.after(labelElement);
                } else {
                    element.before(labelElement);
                }
            }

            this.checkElement
                .width(element.width())
                .height(element.height())
                .appendTo(wrapperElement)
                .offset(element.offset());


            if (element.attr("checked")) {
                self.checkElement.addClass("icrop-CheckMark-checked");
                if (this.options.onIconUrl) {
                    self.checkElement.css("background-image", "url(" + this.options.onIconUrl + ")");
                }
            } else {
                if (this.options.offIconUrl) {
                    self.checkElement.css("background-image", "url(" + this.options.offIconUrl + ")");
                } else {
                    self.checkElement.css("background-image", "");
                }
            }
        },

        init: function () {
            //reload options
        },

        disabled: function (value) {
            if (value === undefined) {
                //getter
                return this.checkElement.hasClass("icrop-disabled");
            } else {
                //setter
                if (value === true) {
                    this.originalElement.attr("disabled", "disabled");
                    this.checkElement.addClass("icrop-disabled");

                } else {
                    this.originalElement.removeAttr("disabled");
                    this.checkElement.removeClass("icrop-disabled");
                }
            }
        }

    };
    pluginImpl.prototype.create.prototype = pluginImpl.prototype;
})(jQuery);