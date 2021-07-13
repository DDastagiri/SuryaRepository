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
        options: {
            //default option values 
        },

        create: function (options, elem) {
            //constructor
            var self = this,
				element = $(elem),
				clearElement = $("<div class='icrop-CustomTextBox-clear' style='cursor: pointer; position: absolute; width:23px; height:23px; top:2px; left:0px;'></div>");


            this.originalElement = element;
            this.options = $.extend(true, {}, this.options, options);

            clearElement
				.bind("click." + pluginName, function (e) {
				    if (self.clearElementHideTimeout) {
				        clearTimeout(self.clearElementHideTimeout);
				        self.clearElementHideTimeout = null;
				    }
				    element.val("").focus().addClass('icrop-focused');
				})
				.css({ "left": (element.width() - 24) + "px", "top": (element.height() - 20) + "px" })
				.hide();

            element
				.bind("focusin." + pluginName, function (e) {
				    $(this)
						.addClass('icrop-focused')
						.trigger("keyup." + pluginName);
				})
				.bind("focusout." + pluginName, function (e) {
				    $(this).removeClass('icrop-focused');
				    self.clearElementHideTimeout = setTimeout(function () {
				        clearElement.hide();
				        self.clearElementHideTimeout = null;
				    }, 300);
				})
				.bind("keyup." + pluginName, function (e) {
				    if ($(this).val().length > 0) {
				        clearElement.show();
				    } else {
				        clearElement.hide();
				    }
				})
				.addClass("icrop-CustomTextBox")
				.wrap("<div style='position:relative; display: inline-block;'></div>")
				.after(clearElement);

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