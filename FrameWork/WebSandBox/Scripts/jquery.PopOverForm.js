//PopOverForm (composite)
(function ($) {
    var pluginName = "PopOverForm",
		pluginImpl;

    $[pluginName] = {
        getCallbackArguments: function(id) {
            return $.toJSON($("#" + id).data(pluginName).callbackArguments);
        },
        getCallbackResponseFromServer: function(jsonString, id) {
            //JSON形式の文字列を変換
            var result = $.parseJSON(jsonString),
                self = $("#" + id).data(pluginName);

            if (self.callbackResponseHandler) {
                self.callbackResponseHandler(result);
            }
        }
    };

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
            pageCapacity: 5,
            render: null,           //callback function(index, args, container)
            close: null,            //callback function(result)
            postbackToServer: null, //server side control only
            callbackToServer: null  //server side control only
        },

        create: function (options, elem) {
            //constructor
            var self = this,
				element = $(elem);

            this.originalElement = element;
            this.options = $.extend(true, {}, this.options, options);
            this.pageIndex = -1;

            //not working if instruction order is popover -> flickable
            this.flickableElement = element.children(".icrop-PopOverForm-content");
            this.flickableElement.flickable({
                section: '.icrop-PopOverForm-page',
                elasticConstant: 0.3,
                friction: 0.96
            });

            var headerSelector = '#' + element.attr('id') + ' > div.icrop-PopOverForm-header';
            var contentSelector = '#' + element.attr('id') + ' > div.icrop-PopOverForm-content';

            this.backButton = $(headerSelector).children(".icrop-PopOverForm-back");
            this.backButton.click(function (e) {
                self.popPage();
            });

            $("#" + element.attr("data-TriggerClientID")).popover({
                header: headerSelector,
                content: contentSelector,
                openEvent: function () {
                    self.pageIndex = -1;
                    self.pushPage();
                },
                closeEvent: function () {
                    //noop    
                }
            });

        },

        init: function () {
            //reload options
        },

        pushPage: function (args) {
            if (this.pageIndex < this.options.pageCapacity) {
                this.pageIndex += 1;
                var container = $(this.flickableElement.find(".icrop-PopOverForm-page").get(this.pageIndex));
                container.empty().append($("<div class='sheetPage-progress'>Progress..</div>"));

                this.flickableElement.flickable('select', this.pageIndex);
                this.options.render(this, this.pageIndex, args, container);
            }
            if (this.pageIndex == 0) {
                this.backButton.hide();
            } else {
                this.backButton.show();
            }
        },

        popPage: function () {
            if (0 < this.pageIndex) {
                this.pageIndex -= 1;
                this.flickableElement.flickable('select', this.pageIndex);
            }
            if (this.pageIndex == 0) {
                this.backButton.hide();
            } else {
                this.backButton.show();
            }
        },

        closePopOver: function (result) {
            $(document).trigger("click.popover");
            if (this.options.close) {
                if (this.options.close(this, result) && this.options.postbackToServer) {
                    $("input[name='" + this.originalElement.attr("id") + "']").val(result);
                    this.options.postbackToServer();
                }
            }
        },

        callbackServer: function (args, callback) {
            if (this.options.callbackToServer) {
                this.callbackArguments = args;
                this.callbackResponseHandler = callback;
                this.options.callbackToServer();
            }
        }

    };
    pluginImpl.prototype.create.prototype = pluginImpl.prototype;
})(jQuery);