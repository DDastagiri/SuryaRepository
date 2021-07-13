//CustomButton (div or button)
(function ($) {
    var pluginName = "CustomButton",
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
            iconUrl: null,
            label: null,
            badgeCount: 0,
            buttonId: null,
            arrowMarginLeft: 0,
            click: null
        },

        create: function (options, elem) {


            //constructor
            var self = this,
				element = $(elem);

            this.elem = elem;
            this.originalElement = element;
            this.options = $.extend(true, {}, this.options, options);

            /* 2013/12/16 TMEJ 小澤 次世代サービス 工程管理機能開発 START */
            //            element
            //				.bind("click." + pluginName, function (e) {
            //				    if (self.disabled()) {
            //				        return false;
            //				    }

            //				    element.addClass("icrop-pressed");
            //				    setTimeout(function () {
            //				        element.removeClass("icrop-pressed");
            //				        if (options.click) {
            //				            options.click.call(elem, e);
            //				        }
            //				    }, 300);
            //				})
            //				.addClass("icrop-CustomButton")
            //				.css("position", "relative");
            element.addClass("icrop-CustomButton").css("position", "relative");
            /* 2013/12/16 TMEJ 小澤 次世代サービス 工程管理機能開発 END */


            //2012/07/06 KN 小澤 STEP2対応 START
            this.arrowElement = $("<div class='footerArrowMain' id='Arrow" + this.options.buttonId + "' style='left:" + this.options.arrowMarginLeft + "px;display:none;  background-image:url(../Styles/Images/footerArrow.png);'>").appendTo(element);
            //2012/07/06 KN 小澤 STEP2対応 END
            this.badgeElement = $("<div class='icrop-CustomButton-badge' style='position: absolute;'></div>").appendTo(element);
            this.badgeCount(this.options.badgeCount);

            /* 2013/12/16 TMEJ 小澤 次世代サービス 工程管理機能開発 START */
            //ボタンIDがある場合のみ設定をする
            if (this.options.buttonId != null && this.options.buttonId != undefined && this.options.buttonId != "") {
                this.sampleElement = $("<div id='sample" + this.options.buttonId + "' style='width: 78px;height: 43px;position: absolute;'></div>").appendTo(element);

                $(this.sampleElement)
				.bind("click." + pluginName, function (e) {
				    if (self.disabled()) {
				        return false;
				    }

				    element.addClass("icrop-pressed");
				    setTimeout(function () {
				        element.removeClass("icrop-pressed");
				        if (options.click) {
				            options.click.call(elem, e);
				        }
				    }, 300);
				})
				.addClass("icrop-CustomButton")
				.css("position", "relative");
            } else {
                element
    				.bind("click." + pluginName, function (e) {
    				    if (self.disabled()) {
    				        return false;
    				    }

    				    element.addClass("icrop-pressed");
    				    setTimeout(function () {
    				        element.removeClass("icrop-pressed");
    				        if (options.click) {
    				            options.click.call(elem, e);
    				        }
    				    }, 300);
    				})
    				.addClass("icrop-CustomButton")
    				.css("position", "relative");
            }
            /* 2013/12/16 TMEJ 小澤 次世代サービス 工程管理機能開発 END */


            if (options.label) {
                this.labelElement = $("<div class='icrop-CustomButton-label'></div>");
                this.labelElement
					.text(options.label)
					.appendTo(element);
                if (0 < element.width()) {
                    this.labelElement.width(element.width());
                }
            }
            if (options.iconUrl) {
                element.css("background-image", "url(" + options.iconUrl + ")");
                if (options.label) {
                    this.labelElement.addClass("icrop-CustomButton-imageLabel");
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

        badgeCount: function (value) {
            if (value === undefined) {
                //getter
                return this.options.badgeCount;
            } else {
                //setter
                this.options.badgeCount = value;
                if (this.options.badgeCount <= 0) {
                    this.badgeElement.hide();
                } else {
                    this.badgeElement.text("" + this.options.badgeCount).show();
                }
            }
        }

    };
    pluginImpl.prototype.create.prototype = pluginImpl.prototype;
})(jQuery);