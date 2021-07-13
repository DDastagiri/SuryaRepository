//NumericKeypad
(function ($) {
    var pluginName = "TCSNumericKeypad",
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
            maxDigits: 12, 		                 //最大桁数：（１～１２）
            acceptDecimalPoint: true,            //小数点の有無（true/false）
            defaultValue: "", 	                 //初期値
            completionLabel: "OK",               //完了ボタンの文言
            cancelLabel: "Cancel",               //キャンセルボタンの文言
            valueChanged: null, 	             //入力確定時に呼び出されるハンドラ
            parentPopover: null,
            open: function () { return true; },  //ポップアップ表示直前に呼び出されるハンドラ
            close: function () { return true; }  //ポップアップ終了直前に呼び出されるハンドラ
        },


        create: function (options, elem) {
            //constructor
            var self = this,
			    element = $(elem);

            this.elem = elem;
            this.originalElement = element;
            this.options = $.extend(true, {}, this.options, options);

            //2012/11/22 TCS 坪根 【A.STEP2】ADD 次世代e-CRB  新車タブレット横展開に向けた機能開発 START
            var clearFlg = false;
            //2012/11/22 TCS 坪根 【A.STEP2】ADD 次世代e-CRB  新車タブレット横展開に向けた機能開発 END

            var maxDigitsValue = this.options.maxDigits;
            var acceptDecimalPointValue = this.options.acceptDecimalPoint;

            var header = $("<div class='icrop-NumericKeypad-head-frame'><div class='icrop-NumericKeypad-head-header'><div class='icrop-NumericKeypad-head-CancellButton'>" + this.options.cancelLabel + "</div><div class='icrop-NumericKeypad-head-CompletionButton'>" + this.options.completionLabel + "</div></div></div>");
            var content = $("<div class='icrop-NumericKeypad-content-frame'><div class='icrop-NumericKeypad-content-ListArea'><div class='icrop-NumericKeypad-content-ListBox'><div class='icrop-NumericKeypad-content-ListItemBox'><div class='icrop-NumericKeypad-content-TextArea'>123465789012</div><ul class='icrop-NumericKeypad-content-ListBoxSetIn'><li class='Button7'>7</li><li class='Button8'>8</li><li class='Button9'>9</li><li class='Button4'>4</li><li class='Button5'>5</li><li class='Button6'>6</li><li class='Button1'>1</li><li class='Button2'>2</li><li class='Button3'>3</li><li class='Button0'>0</li><li class='ButtonPeriod'>.</li><li class='ButtonDelete'><span>&nbsp;</span></li></ul><div class='clearboth'>&nbsp;</div></div></div></div></div>");

            this.inputArea = content.find(".icrop-NumericKeypad-content-TextArea");
            this.inputArea.text(self.options.defaultValue);

            element
			    .TCSpopover({
			        header: header,
			        parentPopover: this.options.parentPopover,
			        content: content,
			        openEvent: function () {
			            content.parent().css({
			                "padding": "0px",
			                "overflow": "hidden"
			            });
			            if (acceptDecimalPointValue == false) {
			                content.find(".ButtonPeriod").hide();
			                content.find(".Button0").width("132px");
			            }
			            //2012/11/22 TCS 坪根 【A.STEP2】ADD 次世代e-CRB  新車タブレット横展開に向けた機能開発 START
			            clearFlg = false;
			            //2012/11/22 TCS 坪根 【A.STEP2】ADD 次世代e-CRB  新車タブレット横展開に向けた機能開発 END

			            return self.options.open.call(self.elem);
			        },
			        closeEvent: function () {
			            return self.options.close.call(self.elem);
			        }
			    });

            this.numericKeyButton = content.find(".icrop-NumericKeypad-content-ListBoxSetIn > li");
            this.numericKeyButton
			    .mousedown(function () {
			        var dataClick = $(this).text().trim();

			        //2012/11/22 TCS 坪根 【A.STEP2】ADD 次世代e-CRB  新車タブレット横展開に向けた機能開発 START
			        if (!clearFlg) {
			            clearFlg = true;
			            if (dataClick != "") {
			                self.inputArea.text("");
			            }
			        }
			        //2012/11/22 TCS 坪根 【A.STEP2】ADD 次世代e-CRB  新車タブレット横展開に向けた機能開発 END

			        var countNumeric = self.inputArea.text().length;

			        if (countNumeric < maxDigitsValue) {
			            var dataTextAreaDisplay;
			            dataTextAreaDisplay = self.inputArea.text();

			            if (dataTextAreaDisplay != "0") {
			                dataTextAreaDisplay = self.inputArea.text() + dataClick;
			            }
			            if (dataTextAreaDisplay == "0" && dataClick != ".") {
			                dataTextAreaDisplay = dataClick;
			            }
			            else if (dataTextAreaDisplay == ".") {
			                dataTextAreaDisplay = "0.";
			            }
			            else {
			                dataTextAreaDisplay = self.inputArea.text() + dataClick;
			            }
			            var periodSplit = (dataTextAreaDisplay.split('.')).length - 1;
			            if (periodSplit <= 1) {
			                self.inputArea.text(dataTextAreaDisplay);
			            }
			        }
			    })
                .addTouch();

            this.endButton = header.find(".icrop-NumericKeypad-head-CompletionButton");
            this.endButton
			    .click(function () {
			        var dataChange = self.inputArea.text();
			        options.valueChanged.call(self.elem, dataChange);
			        self.originalElement.trigger("hidePopover");
			    });

            this.cancelButton = header.find(".icrop-NumericKeypad-head-CancellButton");
            this.cancelButton
			    .click(function () {
			        self.originalElement.trigger("hidePopover");
			    });

            this.deleteButton = content.find(".ButtonDelete");
            this.deleteButton
			    .mousedown(function () {
			        var countNumeric = self.inputArea.text().trim().length;
			        if (countNumeric > 0) {
			            var deleteString = self.inputArea.text().substring(0, countNumeric - 1);
			            self.inputArea.text(deleteString);
			        }
			    })
                .addTouch();

        },

        init: function () {
            //reload options
        },

        setValue: function (value) {
            this.inputArea.text(value);
        }
    };
    pluginImpl.prototype.create.prototype = pluginImpl.prototype;
})(jQuery);