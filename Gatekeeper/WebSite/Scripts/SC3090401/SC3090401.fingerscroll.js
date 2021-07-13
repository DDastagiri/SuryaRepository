///<reference path="jquery.js"/>

(function (window) {

    var fn = {

        /**
        * @class 定数クラス
        */
        constants: {
            //開始イベント
            startEvent: "mousedown touchstart",
            //選択イベント
            selectEvent: "select",
            //ドラッグ
            drag: "mousemove touchmove",
            //終了イベント
            endEvent: "mouseup touchend",
            //はみ出し量
            dumper: 0.02,
            //移動率
            scrollDeltaMod: 4.7,
            //スクロールバーの幅
            scrollbarWidth: 5,
            //スクロールバーの最小の高さ
            minScrollHeight: 13,
            //フリックリリースの高さ
            flickReleaseHeight: 60,
            //スクロールアニメーションの時間
            animateTimeNormal: 800,
            animateTimeLong: 1600
        },

        /**
        * 現在のtop位置、left位置を取得
        * @param {Function} data 内部管理データ
        * @return {Position} 位置
        */
        getTranslate: function (data) {
            var attr = data.inner.get(0).style["-webkit-transform"];
            var m = attr.match(/translate3d\((.+)px,\s*(.+)px,\s*(.+)px\)/);
            return { top: parseInt(m[2]), left: parseInt(m[1]) };
        },

        /**
        * スクロール位置を設定
        * @param {Function} data 内部管理データ
        * @param {Position} position 位置
        */
        setTranslate: function (data, position) {
            data.inner.css({ "-webkit-transform": "translate3d( 0px,"+ position.top + "px, 0px)" });
        },

        /**
        * スクロールバーの位置を設定
        * @param {Function} data 内部管理データ
        * @param {Position} position 位置
        */
        setScrollBarTranslate: function (data, position) {
            data.scrollBar.css({ "-webkit-transform": "translate3d(0px, " + position.scrollTop + "px, 0px)", "opacity": 1 });
        },

        /**
        * webkitのアニメーションを中断
        * @param {Function} data 内部管理データ
        */
        stopAnimate: function (data) {

            var matrix = new WebKitCSSMatrix(window.getComputedStyle(data.inner.get(0)).webkitTransform);
            var matrixBar = new WebKitCSSMatrix(window.getComputedStyle(data.scrollBar.get(0)).webkitTransform);
            fn.setTranslate(data, { top: parseInt(matrix.f), left: parseInt(matrix.e) });
            fn.setScrollBarTranslate(data, { scrollTop: parseInt(matrix.f) });
            data.inner.css({ "-webkit-transition": "none" });
            data.scrollBar.css({ "-webkit-transition": "none" });

        },

        /**
        * 初期化処理
        * @param {Function} param パラメータ
        */
        init: function (param) {

            //CSS属性変更
            var $target = $(this).css({
                "position": "relative",
                "overflow": "hidden",
                "-webkit-tap-highlight-color": "rgba(0,0,0,0)"
            });

            //スクロール内部用のDIVを作成
            if ($target.find(".scroll-inner").length == 0) {
                $target.wrapInner('<div class="scroll-inner" style="-webkit-transform:translate3d(0px,0px,0px);left:0px;top:0px;position:relative;" />');
            }
            var $inner = $target.find(".scroll-inner");

            //イベントデータ用に、外枠、内枠のDOM要素をセット
            var data = {};
            data.target = $target;
            data.inner = $inner;
            data.popover = (param !== undefined && param.popover === true);

            //スクロールバー作成
            fn.createScrollBar(data);

            if (param !== undefined && param.action !== undefined && param.action) {
                if (param.action == "stop") {
                    //スクロール停止
                    if (data.target.hasClass("fingerscroll-stop") === false) data.target.addClass("fingerscroll-stop");
                } else if (param.action == "move") {
                    //移動
                    fn.stopAnimate(data);
                    var pos = fn.getTranslate(data);
                    var moveValue = { top: pos.top, left: pos.left };
                    if (param.moveY !== undefined) moveValue.top += (parseInt(param.moveY) * -1);
                    if (param.moveX !== undefined) moveValue.left += (parseInt(param.moveX) * -1);
                    fn.setTranslate(data, moveValue);
                } else if (param.action == "restart") {
                    //スクロール再開
                    fn.stopAnimate(data);
                    data.target.removeClass("fingerscroll-stop");
                    //2014/08/14 TMEJ 丁 メインメニュー(移動マン) START
                    //再開時の場合で、スクロール領域からはみ出ている場合は、位置を元に戻すアニメーション実行
                    fn.setSize(data);
                    fn._animationScroll(data, 0, 0, fn.constants.animateTimeNormal);
                    //2014/08/14 TMEJ 丁 メインメニュー(移動マン) END
                }
            } else {
                //開始
                data.target.removeClass("fingerscroll-stop");
                $inner.css({ "-webkit-transform": "translate3d(0px, 0px, 0px)" });
            }

            //スクロールバーのリフレッシュイベント設定
            $inner.unbind("refreshScrollBar", fn.refreshScrollBar).bind("refreshScrollBar", data, fn.refreshScrollBar);
            //イベントをバインド
            $target.unbind(fn.constants.startEvent, fn.start).bind(fn.constants.startEvent, data, fn.start);
        },

        /**
        * スクロールバーのリフレッシュ
        * @param {Evnet} e イベント
        */
        refreshScrollBar: function (e) {
            //スクロールバーのリサイズ
            fn.setSize(e.data);
            if (fn.resizeScrollBar(e.data)) {
                fn.setScrollBarTranslate(e.data, fn.calcScroll(e.data, { top: 0, left: 0 }, "refreshScrollBar"));
                e.data.scrollBar.show(0);
                //タイマクリア
                if (e.data.refreshScrollBarTimer) clearTimeout(e.data.refreshScrollBarTimer);
                //２秒間スクロールバーを表示
                e.data.refreshScrollBarTimer = setTimeout(function () {
                    e.data.scrollBar.fadeOut(150);
                }, 2000);
            }
        },

        /**
        * サイズ情報の更新
        * @param {Function} data 内部管理データ
        */
        setSize: function (data) {
            //内部サイズを計測
            data.innerSize = {
                width: data.inner.outerWidth({ margin: true }) - data.target.innerWidth(),
                height: data.inner.outerHeight({ margin: true }) - data.target.innerHeight()
            };
            //全体の高さ
            data.dataHeight = data.inner.outerHeight(true);
            //表示領域
            data.scrollHeight = data.target.innerHeight();
        },

        /**
        * テキスト選択の抑制
        * @param {Event} e イベント
        */
        select: function (e) {
            event.preventDefault();
            return false;
        },

        /**
        * スクロール開始
        * @param {Event} e イベント
        */
        start: function (e) {

            //アニメーションを停止
            fn.stopAnimate(e.data);

            //イベント登録を解除
            $(document).unbind(fn.constants.drag, fn.drag);
            $(document).unbind(fn.constants.endEvent, fn.stop);
            if (!event.changedTouches === undefined && event.changedTouches.length > 1) return;

            if (e.data.target.hasClass("fingerscroll-stop") === true) return;

            //内部サイズを計測
            fn.setSize(e.data);
            if (e.data.innerSize.width <= 0 && e.data.innerSize.height <= 0) {
                if (e.type === "mousedown") event.preventDefault();
                return;
            }

            //監視処理
            $(document).bind(fn.constants.drag, e.data, fn.drag).bind(fn.constants.endEvent, e.data, fn.stop);
            e.data.capture = {};

            //位置記憶
            e.data.position = fn.getEventPosition();
            e.data.startPosition = fn.getEventPosition();

            //スクロールバーのリサイズ
            if (fn.resizeScrollBar(e.data)) {
                fn.setScrollBarTranslate(e.data, fn.calcScroll(e.data, { top: 0, left: 0 }, "start"));
            }

            //タイマクリア
            if (e.data.refreshScrollBarTimer) clearTimeout(e.data.refreshScrollBarTimer);

            //マウス(位置)の移動履歴
            e.data.captures = [{ x: e.data.position.x, y: e.data.position.y, time: new Date()}];

            //フリックリリース系のイベント監視
            e.data.isFlickReleaseTop = e.data.isFlickReleaseBottom = false;

            var curTranslate = fn.getTranslate(e.data);
            if (Math.abs(curTranslate.top) <= 5) {
                //フリックリリース(上)を監視
                e.data.isFlickReleaseTop = true;
            } else if (e.data.target.height() + Math.abs(curTranslate.top) + 5 >= e.data.inner.height()) {
                //フリックリリース(下)を監視
                e.data.isFlickReleaseBottom = true;
            }
        },

        /**
        * スクロール中の処理
        * @param {Event} e イベント
        */
        drag: function (e) {

            if (e.data.target.hasClass("fingerscroll-stop") === true) {
                fn.stop(e);
                if (event) event.preventDefault();
                return;
            }

            //マウス位置
            var evtPos = fn.getEventPosition();
            var y = evtPos.y, x = evtPos.x;

            //移動距離を計算
            var move = { top: y - e.data.position.y, left: x - e.data.position.x };

            //スクロール位置設定
            var src = fn.calcScroll(e.data, move, "drag");
            fn.setTranslate(e.data, src);           //本体
            fn.setScrollBarTranslate(e.data, src);  //スクロールバー
            e.data.scrollBar.show(0);

            //2014/08/14 TMEJ 丁 メインメニュー(移動マン) START
            //イベント生成
            $(e.data.target).trigger("move.fingerscroll", fn.getTranslate(e.data));
            //2014/08/14 TMEJ 丁 メインメニュー(移動マン) END

            //位置保存
            e.data.position.y = y;
            e.data.position.x = x;

            //移動位置を記録
            if (e.data.captures.length > 4) e.data.captures.shift();
            e.data.captures.push({ x: e.data.position.x, y: e.data.position.y, time: new Date() });

            //2014/08/14 TMEJ 丁 メインメニュー(移動マン) START
            //クリックイベント抑制
            if (e.data.preventClick === false && (Math.abs(y - e.data.startPosition.y) > 20 || Math.abs(x - e.data.startPosition.x) > 20)) {
                //抑制処理
                $(".Coordinate").bind("click", function (clickEvent) {
                    $(".Coordinate").unbind("click", arguments.callee);

                    if (fn.constants.scrollClickControllFlg == "1" || fn.constants.scrollClickControllFlg == "0") {
                        fn.constants.scrollClickControllFlg = "0"
                        return true
                    }
                    if (clickEvent) clickEvent.stopPropagation();
                    if (clickEvent) clickEvent.preventDefault();

                    return false;
                });

                $(".Coordinate").bind("mouseup", function (clickEvent) {

                    $(".Coordinate").unbind("mouseup", arguments.callee);
                    fn.constants.scrollClickToTagetId = $(clickEvent.target).attr("id");
                });

                $(e.data.target).bind("click", function (clickEvent) {
                    if ($(e.originalEvent.target).hasClass("Coordinate")) {
                        if ($(e.originalEvent.target).attr("id") == fn.constants.scrollClickToTagetId) {
                            fn.constants.scrollClickControllFlg = "2"
                        } else {
                            fn.constants.scrollClickControllFlg = "1"
                        }
                    } else {
                        fn.constants.scrollClickControllFlg = "1"
                    }
                    if (clickEvent) clickEvent.stopPropagation();
                    if (clickEvent) clickEvent.preventDefault();
                    $("#" + e.data.target.get(0).id).unbind("click", arguments.callee);

                    return false;
                });

                e.data.preventClick = true;
                hideDetailsPopover();
            }
            //2014/08/14 TMEJ 丁 メインメニュー(移動マン) END

            if (e.data.popover) {
                return false;
            }
        },

        /**
        * スクロール終了
        * @param {Event} e イベント
        */
        stop: function (e) {
            //ドラッグイベントのハンドル解除
            $(document).unbind(fn.constants.drag, fn.drag).unbind(fn.constants.endEvent, fn.stop);

            //イベント生成
            $(e.data.target).trigger("end.fingerscroll", fn.getTranslate(e.data));

            if (e.data.target.hasClass("fingerscroll-stop") === true) {
                //fn.stop(e);
                if (event) event.preventDefault();
                return;
            }

            //マウス位置
            var evtPos = fn.getEventPosition();
            var y = evtPos.y, x = evtPos.x;

            //一定時間以上ポインタを同じ位置に置いたままドラッグ終了した場合
            var now = new Date(), lastDragTime = e.data.captures[e.data.captures.length - 1].time;
            if (now.getTime() - lastDragTime.getTime() >= 210) {
                e.data.captures.push({ x: e.data.position.x, y: e.data.position.y, time: new Date() });
            }

            //移動距離を計算
            var x1, x2, y1, y2;
            x1 = x2 = e.data.captures[e.data.captures.length - 1].x, y1 = y2 = e.data.captures[e.data.captures.length - 1].y;

            var lastTime = e.data.captures[e.data.captures.length - 1].time.getTime();
            for (var i = e.data.captures.length - 2; i >= 0; i--) {
                if (lastTime - e.data.captures[i].time.getTime() <= 30 || i == e.data.captures.length - 2) {
                    x1 = e.data.captures[i].x;
                    y1 = e.data.captures[i].y;
                }
            }

            //アニメーション処理
            var aniTime = fn.constants.animateTimeNormal;
            var top = 0, left = 0;

            if (Math.abs(y2 - y1) > 7 || Math.abs(x2 - x1) > 7) {
                //移動距離を計算
                top = fn.constants.scrollDeltaMod * (y2 - y1);
                left = fn.constants.scrollDeltaMod * (x2 - x1);
                if (fn.constants.scrollDeltaMod * Math.abs(x2 - x1) > 1000) aniTime = fn.constants.animateTimeLong;
            } else {
                //スクロール位置設定
                aniTime = fn.constants.animateTimeNormal;
            }

            //アニメーションスクロール
            fn._animationScroll(e.data, top, left, aniTime);

            $("#scrollDiv").click();
        },

        /**
        * ドラッグ終了時のアニメーションスクロール
        */
        _animationScroll: function (data, movetop, moveleft, aniTime) {

            var src = fn.calcScroll(data, { top: movetop, left: moveleft }, "stop");
            var timingFunction = "cubic-bezier(0.0, 1, 0.5, 1)";

            //スクロールアニメーションを開始する
            if (Math.abs(movetop) > 0 || Math.abs(moveleft) > 0 || src.overTopSize > 0 || src.overBottomSize > 0) {

                //スクロール用のDIV
                data.inner.css({
                    "-webkit-transition": aniTime + "ms " + timingFunction,
                    "-webkit-transform": "translate3d(" + src.left + "px, " + src.top + "px, 0px)"
                }).one("webkitTransitionEnd", data, function (we) {
                    we.data.scrollBar.fadeOut(fn.constants.scrollDuration);
                    we.data.inner.css({ "-webkit-transition": "none" });
                });

                //スクロールバーのアニメーション
                if (Math.abs(movetop) > 0 || Math.abs(moveleft) > 0) {
                    //スクロールバー
                    data.scrollBar.css({
                        "-webkit-transition": aniTime + "ms " + timingFunction,
                        "-webkit-transform": "translate3d(0px, " + src.scrollTop + "px, 0px)"
                    }).one("webkitTransitionEnd", data, function (we) {
                        we.data.scrollBar.css({ "-webkit-transition": "none" });
                    });
                }

            } else {
                //アニメーションなしのスクロール
                fn.setTranslate(data, fn.calcScroll(data, { top: 0, left: 0 }, "stop"));
                data.scrollBar.fadeOut(0);
            }

        },

        /**
        * スクロール位置計算
        * @param {Function} data 内部管理データ
        * @param {Function} move 移動量
        * @param {Function} action アクション
        * @return {Function} 計算結果
        */
        calcScroll: function (data, move, action) {

            var curTranslate = fn.getTranslate(data);   //現在の位置
            var top = curTranslate.top + move.top;

            if (action === "drag") {
                //ドラッグ処理
                if (top > 0) {
                    if (top > data.target.height() * 0.6) top = Math.ceil(data.target.height() * 0.6);
                    top -= fn.constants.dumper * top;
                }
                if (top < -data.innerSize.height) {
                    if (Math.abs(top + data.innerSize.height) > data.target.height() * 0.6)
                        top = -(data.innerSize.height + Math.ceil(data.target.height() * 0.6));
                    top -= fn.constants.dumper * (top + data.innerSize.height);
                }
            } else {
                top = Math.max(Math.min(0, top), -data.innerSize.height);
            }

            var left = curTranslate.left + move.left;

            if (action === "drag") {
                //ドラッグ処理
                if (left > 0) left -= fn.constants.dumper * left;
                if (left < -data.innerSize.width) left -= fn.constants.dumper * (left + data.innerSize.width);
            } else {
                left = Math.max(Math.min(0, left), -data.innerSize.width);
            }
            if (data.innerSize.width <= 0) left = 0;

            //スクロールバーの縦位置を計算
            var scrollY;
            if (top < 0) {
                var rate = Math.min((Math.abs(top) + data.scrollHeight) / data.dataHeight, 1);
                scrollY = Math.max(0, Math.ceil(data.scrollHeight * rate) - data.scrollBar.outerHeight()) - 1;
            } else {
                scrollY = 0;
            }


            //戻り値を返却
            return {
                top: top,
                left: left,
                scrollTop: scrollY,
                //curTranslate.top > 0 ? true : (curTranslate.top < -data.innerSize.height ? true : false),
                overTopSize: curTranslate.top > 0 ? curTranslate.top : 0,
                overBottomSize: curTranslate.top < -data.innerSize.height ? Math.abs(curTranslate.top + data.innerSize.height) : 0
            };
        },

        /**
        * イベント発生時のポジション取得
        */
        getEventPosition: function () {

            if (event.changedTouches !== undefined && event.changedTouches) {
                //iPad
                return { x: event.changedTouches[0].clientX, y: event.changedTouches[0].clientY };
            } else {
                //PC
                return { x: event.pageX, y: event.pageY };
            }
        },

        /**
        * スクロールバー要素作成
        * @param {Function} data 内部管理データ
        */
        createScrollBar: function (data) {
            //rgba(100,100,100,0.8)
            if ($(".scroll-bar", data.target).length == 0) data.target.append('<div class="scroll-bar" />');
            //スクロールバーを登録
            var $bar = $(".scroll-bar", data.target).css({
                "position": "absolute",
                "border": "1px solid #777",
                "border-radius": "5px",
                "background": "rgba(100,100,100,0.8)",
                "width": fn.constants.scrollbarWidth + "px",
                "top": "0px",
                "right": "0px",
                "display": "none",
                "-webkit-transform": "translate3d(0px, 0px, 0px)",
                "box-sizing": "border-box"
            });
            //スクロールバーオブジェクトをセット
            data.scrollBar = $bar;
        },

        /**
        * スクロールバーのリサイズ
        * @param {Function} data 内部管理データ
        */
        resizeScrollBar: function (data) {

            var scrollH = data.target.height(), dataH = data.inner.height(), scrollBarH;
            var rate = scrollH > dataH ? 1 : scrollH / dataH;
            //バーの高さを求める(規定値以下のスクロールバーの高さになるのであれば、規定値にする)
            data.scrollBar.height(Math.max(Math.ceil(scrollH * rate), fn.constants.minScrollHeight));
            //スクロールが必要ならTrue、それ以外はFalse
            return rate !== 1;
        }

    };

    //スクロール設定を行います。
    //引数なしでコールすることで、スクロール設定を行えます。
    //任意の引数として、引数paramにJSON形式で以下のパラメータが指定できます。
    //  action: 「stop」スクロール機能を中断する。
    //          「restart」スクロールを中断した位置から再開します。
    //          「move」スクロール位置を移動します。
    //  moveY:  Y軸のスクロール移動量 (actionに「move」を指定する場合必須）
    //  moveX:  X軸のスクロール移動量 (actionに「move」を指定する場合必須）
    // [サンプルコード]
    //  1.セレクターで指定したDIVタグを１本指でのスクロールを可能にする
    //     $(selector).fingerScroll();
    //  2.１本指でのスクロールを中断する
    //     $(selector).fingerScroll({ action: "stop" });
    //  3.中断したスクロールを中断した位置から再開します。
    //     $(selector).fingerScroll({ action: "restart" });
    //  4.スクロール位置を下に10px移動する
    //     $(selector).fingerScroll({ action: "move", moveY: 10, moveX: 0 });
    $.fn.SC3090401fingerScroll = function (param) {
        return this.each(function () {
            fn.init.call(this, param);
        });
    };

})(window);