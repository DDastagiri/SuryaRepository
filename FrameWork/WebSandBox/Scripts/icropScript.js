var icropScript = {}

//公開API
icropScript.ShowMessageBox = function (code, word, detail) {
    alert(word);
};

icropScript.OperationLocked = false;

icropScript.tcvCallback = {};

//共通基盤 内部API
icropScript.ui = {
    curDialog: null,    //ダイアログが表示されている場合、ダイアログの土台となるオーバーレイオブジェクトを格納

    //オーバーレイ作成
    createOverlay: function ($container) {

        var $overlay;

        if ($container === undefined || !($container)) {
            //ウィンドウ
            $overlay = $("<div class='openWindowBack'/>").css({
                position: "absolute", top: "0px", left: "0px",
                width: $("body").outerWidth() > $(window).width() ? $("body").outerWidth() : $(window).width(),
                height: $("body").outerHeight() > $(window).height() ? $("body").outerHeight() : $(window).height(),
                background: "rgba(0, 0, 0, 0.5)"
            });
        } else {
            //コンテナ内
            $overlay = $("<div class='openWindowBack'/>").css({
                position: "absolute",
                top: $container.offset().top + "px",
                left: $container.offset().left + "px",
                width: $container.outerWidth() + "px",
                height: $container.outerHeight() + "px",
                background: "rgba(100, 100, 100, 0.1)"
            });
        }

        $overlay.bind("mousedown click touchstart", function (e) {
            //イベントパブリングを抑制
            e.stopPropagation();
        });

        return $overlay;
    },


    //読み込み中ウィンドウ表示
    showLodingWindow: function ($container) {

        var $overlay = icropScript.ui.createOverlay($container);
        var $cont = $container === undefined ? $(window) : $container;

        //オーバーレイ登録
        $("body").append($overlay);
        $overlay.append("<div class='lodingIconBack'><div class='lodingIcon'/></div>");
        //スピンアイコンの背景
        var iconHeight = $overlay.find(".lodingIconBack").height();
        var iconWidth = $overlay.find(".lodingIconBack").width();
        if ($cont.is(window)) {
            $overlay.find(".lodingIconBack").css({
                top: ($(window).scrollTop() + $(window).height() / 2) - (iconHeight / 2),
                left: ($(window).scrollLeft() + $(window).width() / 2) - (iconWidth / 2)
            });
        } else {
            $overlay.find(".lodingIconBack").css({
                top: ($cont.height() / 2) - (iconHeight / 2),
                left: ($cont.width() / 2) - (iconWidth / 2)
            });
        }
        //スピンアイコン
        $overlay.find(".lodingIcon").css({
            top: $overlay.find(".lodingIconBack").height() / 2 - ($overlay.find(".lodingIcon").height() / 2),
            left: $overlay.find(".lodingIconBack").width() / 2 - ($overlay.find(".lodingIcon").width() / 2)
        });

        //表示
        $overlay.fadeIn(50);

    },

    //読み込み中ウィンドウを非表示にする
    closeLodingWindow: function () {
        var $overlay = $("div.openWindowBack");
        $overlay.fadeOut(200);
    },

    //通知件数セット+アイコン切替処理
    setNotice: function () {
        try {
            var url = window.location.href.split("/Pages");
            var postTarget = url[0] + "/Services/IC3040802.asmx/GetUnreadNotice";

            $.ajax({
                type: "POST",
                url: postTarget,
                contentType: "application/xml; charset=UTF-8",
                success: function (ret) {
                    //通知件数取得 
                    var cnt = ret.childNodes[0].childNodes[0].nodeValue;

                    //通知アイコン切替
                    var forum = document.getElementById("divForum");
                    var forumblink = document.getElementById("divForumblink");
                    var lblForum = document.getElementById("MstPG_lblForum");

                    if (cnt == 0) {
                        forum.style.display = "block";
                        forumblink.style.display = "none";
                    } else {
                        forum.style.display = "none";
                        forumblink.style.display = "block";
                        lblForum.innerText = cnt;
                    }

                    //ロック状態を返却
                    var locked = $("#MstPG_OperationLocked");
                    if (locked.val() == 1) {
                        return false;
                    } else {
                        return true;
                    }
                },
                error: function (e) { }
            });
        } catch (e) { }
    },

    //通知画面の表示処理
    openNoticeDialog: function () {
        icropScript.ui.openNoticeDialogMain("SC3040801.aspx", 70, 20);
    },

    //ダイアログ表示処理（表示位置指定バージョン）
    openNoticeDialogMain: function (url, left, top) {
        var container = window;
        //オーバーレイ
        var $overlay = icropScript.ui.createOverlay();
        //フレームを囲うDIV作成
        var $wrap = $("<div/>").css({
            "-webkit-transform": "translate3d(0px, 0px, 0px)",
            top: "0px", left: "0px",
            position: "absolute",
            display: "none",
            background: "#FFF",
            border: "5px solid #333"
        });
        //フレーム作成
        var $iframe = $("<iframe frameborder='0' src='" + url + "'></iframe>").css({ margin: "0px", padding: "0px" });
        //フレーム内のページ読み込み終了イベント
        $iframe.bind("load", function (e) {
            //内部コンテンツの高さ・幅を取得する
            var size = {
                width: this.contentWindow.document.documentElement.scrollWidth + 10,
                height: this.contentWindow.document.documentElement.scrollHeight + 10
            };
            //フレームサイズ設定
            $iframe.css({ width: size.width + "px", height: size.height + "px" });

            //サイズ調整
            if (size.width > $(container).width()) size.width = $(container).width();
            if (size.height > $(container).height()) size.height = $(container).height();
            $wrap.css({ width: size.width + "px", height: size.height + "px" });

            //位置設定
            $wrap.css({ "-webkit-transform": "translate3d(" + left + "px, " + top + "px, 0px)" });
            $iframe.unbind("load", arguments.callee);
            $wrap.css({ "-webkit-transform": "translate3d(" + left + "px, " + top + "px, 0px)" });
            $wrap.fadeIn(500);
            //フレーム表示
            $wrap.show(0);
        });

        //タグ追加
        $("body").append($overlay.append($wrap.append($iframe)));
        $overlay.get(0).postbackfunc = function () { __doPostBack('__Page', ''); };
        icropScript.ui.curDialog = $overlay;
    },

    //ダイアログ表示処理
    openDialog: function (url, effect, postbackCallBack) {
        var container = window;
        //オーバーレイ
        var $overlay = icropScript.ui.createOverlay();
        //フレームを囲うDIV作成
        var $wrap = $("<div/>").css({
            "-webkit-transform": "translate3d(0px, 0px, 0px)",
            top: "0px", left: "0px",
            position: "absolute",
            display: "none",
            background: "#FFF",
            border: "5px solid #333"
        });
        //フレーム作成
        var $iframe = $("<iframe frameborder='0' src='" + url + "'></iframe>").css({ margin: "0px", padding: "0px" });
        //effect = "bottom";
        //フレーム内のページ読み込み終了イベント
        $iframe.bind("load", function (e) {
            //内部コンテンツの高さ・幅を取得する
            var size = {
                width: this.contentWindow.document.documentElement.scrollWidth + 10,
                height: this.contentWindow.document.documentElement.scrollHeight + 10
            };
            //フレームサイズ設定
            $iframe.css({ width: size.width + "px", height: size.height + "px" });

            //サイズ調整
            if (size.width > $(container).width()) size.width = $(container).width();
            if (size.height > $(container).height()) size.height = $(container).height();
            $wrap.css({ width: size.width + "px", height: size.height + "px" });

            //位置調整
            var left = Math.floor(($(container).width() / 2) - ($wrap.outerWidth() / 2));
            var top = Math.floor(($(container).height() / 2) - ($wrap.outerHeight() / 2));

            //位置設定
            $wrap.css({ "-webkit-transform": "translate3d(" + left + "px, " + top + "px, 0px)" });
            $iframe.unbind("load", arguments.callee);
            if (effect === "left") {
                //左からスライドイン
                $wrap.css({ "-webkit-transform": "translate3d(" + ($wrap.outerWidth() * -1) + "px, " + top + "px, 0px)" });
                left = 0;
            } else if (effect === "right") {
                //右からスライドイン
                $wrap.css({ "-webkit-transform": "translate3d(" + ($wrap.outerWidth() + $overlay.outerWidth()) + "px, " + top + "px, 0px)" });
                left = $overlay.outerWidth() - $wrap.outerWidth();
            } else if (effect == "top") {
                //上からスライドイン
                $wrap.css({ "-webkit-transform": "translate3d(" + left + "px, " + ($wrap.outerHeight() * -1) + "px, 0px)" });
                top = 0;
            } else if (effect == "bottom") {
                //下からスライドイン
                $wrap.css({ "-webkit-transform": "translate3d(" + left + "px, " + ($wrap.outerHeight() + $overlay.outerHeight()) + "px, 0px)" });
                top = $overlay.outerHeight() - $wrap.outerHeight();
            } else {
                //フェードイン
                $wrap.css({ "-webkit-transform": "translate3d(" + left + "px, " + top + "px, 0px)" });
                $wrap.fadeIn(500);
                return;
            }
            //フレーム表示
            $wrap.show(0);
            //スライドインアニメーション
            $wrap.css({
                "-webkit-transition": "500ms ease-out",
                "-webkit-transform": "translate3d(" + left + "px, " + top + "px, 0px)"
            }).one("webkitTransitionEnd", function (we) { $wrap.css({ "-webkit-transition": "none" }); });

        });

        //タグ追加
        $("body").append($overlay.append($wrap.append($iframe)));
        $overlay.get(0).postbackfunc = postbackCallBack;
        $overlay.get(0).dialogEffect = effect;
        icropScript.ui.curDialog = $overlay;
    },

    //ダイアログクローズ処理
    closeDialog: function () {

        var $overlay = null;
        //ダイアログのオーバーレイを取得
        if (icropScript.ui.curDialog === null) {
            if (window.parent.icropScript.ui.curDialog !== null) $overlay = window.parent.icropScript.ui.curDialog;
        } else {
            $overlay = icropScript.ui.curDialog;
        }
        //ダイアログのクローズ処理
        if ($overlay === null) return;
        //フレームアンロード
        $overlay.find("iframe").trigger("unload");
        var $wrap = $overlay.find("> div");
        var matrix = new WebKitCSSMatrix(window.getComputedStyle($wrap.get(0)).webkitTransform);
        var top = matrix.f, left = matrix.e;

        var effect = $overlay.get(0).dialogEffect;
        if (effect === "left") left = $wrap.outerWidth() * -1; //左からスライドイン
        else if (effect === "right") left = $wrap.outerWidth() + $overlay.outerWidth(); //右からスライドイン
        else if (effect == "top") top = $wrap.outerHeight() * -1; //上からスライドイン
        else if (effect == "bottom") top = $wrap.outerHeight() + $overlay.outerHeight(); //下からスライドイン
        else {
            //フェードイン
            $overlay.fadeOut(300, function () {
                if ($.isFunction($overlay.get(0).postbackfunc)) $overlay.get(0).postbackfunc();
                $overlay.remove();
            });
        }
        //スライドインアニメーション
        $wrap.css({
            "-webkit-transition": "500ms ease-out",
            "-webkit-transform": "translate3d(" + left + "px, " + top + "px, 0px)"
        }).one("webkitTransitionEnd", function (we) {
            $wrap.css({ "-webkit-transition": "none" });
            $overlay.fadeOut(20, function () {
                if ($.isFunction($overlay.get(0).postbackfunc)) $overlay.get(0).postbackfunc();
                $overlay.remove();
            });
        });
    }
};

$(function () {
    //ブラウザのデフォルト動作（ダブルタップ、ピンチ）を禁止
    $("body")
        .bind("touchstart.icropScript", function (e) {
            var now = new Date();
            var prevTouchTime = $(this).data("prevTouchTime");
            if (prevTouchTime) {
                var currentTouchTime = now.getTime();
                if ((currentTouchTime - prevTouchTime) < 500) {
                    e.preventDefault();
                    return;
                }
            }
            $(this).data("prevTouchTime", now.getTime());
        })
        .bind("touchmove.icropScript", function (e) {
            if (!icropScript.ui.bypassPreventDefault) {
                e.preventDefault();
            }
            icropScript.ui.bypassPreventDefault = false;
        });

    //スクリプトエラーをサーバーへ送信
    window.onerror = function (msg, url, line) {
        try {
            var postTarget = "../Error/SC3010304.aspx";
            if (window.location.href.indexOf('/Pages/') < 0) {
                postTarget = "./Error/SC3010304.aspx";
            }
            $.post(postTarget, { ClientError: " [" + window.location.href + "] " + msg + "(line:" + line + ")" });
        } catch (e) { }
    };
});
