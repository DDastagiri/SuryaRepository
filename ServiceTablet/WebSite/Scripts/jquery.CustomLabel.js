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

                        //�R�_���[�_�\������Ă��邩�`�F�b�N
                        var text = $target.text();
                        //�\���e�L�X�g
                        var $test = $("<span/>").text(text);

                        //�t�H���g�Ɋւ�������R�s�[(���m�ɕ����擾���邽�߂�)
                        $.each(fontStyleAry, function (idx, val) {
                            $test.css(val, $target.css(val));   //style�̃R�s�[����
                        });
                        $test.css("display", "none");
                        //�e�X�g�p�Ƀ^�O�ǉ�(��������Ȃ��ƕ����}��Ȃ�)
                        $("body").append($test);

                        //�e�L�X�g���t���ɕ\�������ꍇ�̕����v��
                        var w = $test.innerWidth();

                        //�R�_���[�_�[���\������Ă��邩�`�F�b�N
                        if ($target.innerWidth() < w) {

                            //���Έʒu��ݒ�
                            var x = (e.pageX || event.changedTouches[0].clientX) - $target.offset().left;
                            var y = (e.pageY || event.changedTouches[0].clientY) - $target.offset().top;

                            //�R�_���[�_�̕����v��(44px��菬�����ꍇ��44px�Œ�)
                            var rw = Math.max($test.text("�c").innerWidth() * 1.5, 44);
                            //�R�_���[�_�t�߂��N���b�N���Ă��邩�`�F�b�N
                            if ($target.innerWidth() - rw <= x) {

                                //�`�b�v�̃X�^�C���ƕ\������e�L�X�g���w��
                                var $tip = $("<div class='icrop-CustomLabel-tooltip'/>").css({
                                    position: "absolute",
                                    display: "none"
                                }).html($target.html());

                                //�c�[���`�b�v��\������ꍇ�́A�C�x���g�p�u�����O��}��
                                //�C�x���g�p�u�����O��}��
                                event.stopPropagation();
                                //�`�b�v�o�^
                                $("body").append($tip);

                                //�`�b�v�\���ʒu�w��
                                var tOffset = $target.offset();
                                var tipX = tOffset.left + 30;
                                var tipY = tOffset.top - $tip.outerHeight() - 12;

                                if ($(document).width() < (tipX + $tip.outerWidth())) {
                                    //�`�b�v����ʊO�ɉB��Ȃ��悤�Ɉʒu��␳
                                    tipX = $(document).width() - $tip.outerWidth() - 12;
                                }

                                $tip.css({ left: tipX, top: tipY });

                                /////////////////////////////////////////////////////////////////////
                                //�c�[���`�b�v�\������
                                $tip.fadeIn(400, function () {
                                    //5�b��Ɏ�������
                                    var t = setTimeout(function () {
                                        if ($tip.is(":visible")) {
                                            $tip.fadeOut(400, function () {
                                                $tip.remove();
                                                t = null;
                                            });
                                        }
                                    }, 5000);

                                    //�`�b�v����邽�߂ɁA�^�b�`�n�C�x���g���Ď�
                                    $("#bodyFrame").bind("click." + pluginName, function (e) {
                                        $tip.remove();
                                        $(this).unbind("." + pluginName);
                                    });
                                });

                                return false;
                            }
                        }
                        //�e�X�g�v�f�폜
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