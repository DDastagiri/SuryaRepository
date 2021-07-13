//CustomLabel (block)
(function ($) {
    var pluginName = "CustomLabel",
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
            useEllipsis: false
        },

        create: function (options, elem) {
            //constructor
            var self = this,
				element = $(elem);

            this.originalElement = element;
            this.options = $.extend(true, {}, this.options, options);

            element.addClass("icrop-CustomLabel");
            if (this.options.useEllipsis) {
                element
                    .bind("click." + pluginName, function (e) {
                        var $target = $(e.target);
                        var fontStyleAry = ["font-size", "font-family", "font-weight", "font-stretch", "font-style", "font-weight", "font-variant"];

                        //３点リーダ表示されているかチェック
                        var text = $target.text();
                        //表示テキスト
                        var $test = $("<span/>").text(text);

                        //フォントに関する情報をコピー(正確に幅を取得するために)
                        $.each(fontStyleAry, function (idx, val) {
                            $test.css(val, $target.css(val));   //styleのコピー処理
                        });
                        $test.css("display", "none");
                        //テスト用にタグ追加(これをしないと幅が図れない)
                        $("body").append($test);

                        //テキストをフルに表示した場合の幅を計測
                        var w = $test.innerWidth();

                        //３点リーダーが表示されているかチェック
                        if ($target.innerWidth() < w) {

                            //相対位置を設定
                            var x = (e.pageX || event.changedTouches[0].clientX) - $target.offset().left;
                            var y = (e.pageY || event.changedTouches[0].clientY) - $target.offset().top;

                            //３点リーダの幅を計測(44pxより小さい場合は44px固定)
                            var rw = Math.max($test.text("…").innerWidth() * 1.5, 44);
                            //３点リーダ付近をクリックしているかチェック
                            if ($target.innerWidth() - rw <= x) {

                                //チップのスタイルと表示するテキストを指定
                                var $tip = $("<div class='icrop-CustomLabel-tooltip'/>").css({
                                    position: "absolute",
                                    display: "none"
                                }).html($target.html());

                                //ツールチップを表示する場合は、イベントパブリングを抑制
                                //イベントパブリングを抑制
                                event.stopPropagation();
                                //チップ登録
                                $("body").append($tip);

                                //チップ表示位置指定
                                var tOffset = $target.offset();
                                var tipX = tOffset.left + 30;
                                var tipY = tOffset.top - $tip.outerHeight() - 12;

                                if ($(document).width() < (tipX + $tip.outerWidth())) {
                                    //チップが画面外に隠れないように位置を補正
                                    tipX = $(document).width() - $tip.outerWidth() - 12;
                                }

                                $tip.css({ left: tipX, top: tipY });

                                /////////////////////////////////////////////////////////////////////
                                //ツールチップ表示処理
                                $tip.fadeIn(400, function () {
                                    //5秒後に自動消滅
                                    var t = setTimeout(function () {
                                        if ($tip.is(":visible")) {
                                            $tip.fadeOut(400, function () {
                                                $tip.remove();
                                                t = null;
                                            });
                                        }
                                    }, 5000);

                                    //チップを閉じるために、タッチ系イベントを監視
                                    $("#bodyFrame").bind("click." + pluginName, function (e) {
                                        $tip.remove();
                                        $(this).unbind("." + pluginName);
                                    });
                                });

                                return false;
                            }
                        }
                        //テスト要素削除
                        $test.remove();
                    })
                    .css({
                        "overflow": "hidden",
                        "white-space": "nowrap",
                        "text-overflow": "ellipsis"
                    });
            }
        },

        init: function () {
            //reload options
        }
    };
    pluginImpl.prototype.create.prototype = pluginImpl.prototype;
})(jQuery);