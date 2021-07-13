//CustomTextBox
(function ($) {
    var pluginName = "CustomTextBox",
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
        CLEAR_BUTTON_WIDTH: 27,

        options: {
            //default option values 
            clear: function () { }
        },

        create: function (options, elem) {
            //constructor
            var self = this,
				element = $(elem),
                elementPosition;

            this.elem = elem;
            this.originalElement = element;

            this.options = $.extend(true, {}, this.options, options);

            this.clearElement = $("<div class='icrop-CustomTextBox-clear' style='position:absolute; display:none;'></div>");
            this.clearElement
                .bind("touchstart." + pluginName + " mousedown." + pluginName, function (e) {
                    element.val("");
                    self.options.clear.call(self.elem);

                    self.hideClearButton();
                    return false;
                });

            element
				.bind("focusin." + pluginName, function (e) {
				    self.originalElement
						.addClass('icrop-focused')
						.trigger("keyup." + pluginName);
				})
				.bind("focusout." + pluginName, function (e) {
				    self.originalElement
                        .removeClass('icrop-focused');
				    self.hideClearButton();
				})
				.bind("keyup." + pluginName, function (e) {
				    if (self.disabled() || self.originalElement.attr("readonly")) {
				        return;
				    }
				    if (self.originalElement.val() !== "") {
				        setTimeout(function () { self.showClearButton(); }, 200);
				    } else {
				        self.hideClearButton();
				    }
				})
				.addClass("icrop-CustomTextBox")
				.wrap("<div style='position:relative; display: inline-block; margin:0px; padding:0px;'></div>")
				.after(this.clearElement);

            self.updateText(self.originalElement.val());
        },

        init: function () {
            //reload options
        },

        updateLabelStyle: function (value) {
            //noop
        },

        updateText: function (value) {
            this.originalElement.val(value);
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
        },

        showClearButton: function () {
            if (this.clearElement.is(":hidden")) {
                this.clearElement
                    .show()
                    .css({
                        "left": 500,
                        "top": this.originalElement.position().top + ((this.originalElement.outerHeight({ mergin: true }) - 22) / 2)
                    });
                this.originalElement.css("padding-right", this.CLEAR_BUTTON_WIDTH).width(this.originalElement.width() - this.CLEAR_BUTTON_WIDTH);
            }
        },

        hideClearButton: function () {
            if (this.clearElement.is(":visible")) {
                this.clearElement.hide();
                this.originalElement.css("padding-right", 0).width(this.originalElement.width() + this.CLEAR_BUTTON_WIDTH);
            }
        }

    };
    pluginImpl.prototype.create.prototype = pluginImpl.prototype;
})(jQuery);