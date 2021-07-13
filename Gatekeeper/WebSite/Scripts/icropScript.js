var icropScript = {}

//スクリプトエラー制御
window.onerror = function (msg, url, line) {
    var errorDecription = "(unknown error)";

    try {
        var postTarget = "../Error/SC3010304.aspx";
        if (window.location.href.indexOf('/Pages/') < 0) {
            postTarget = "./Error/SC3010304.aspx";
        }
        errorDescription = " [" + window.location.href + "] " + msg + "(line:" + line + ")";
        $.post(postTarget, { ClientError: errorDescription });
    } catch (e) { }

    if (icropScript.ui.enableScriptErrorNotification) {
        var rootWindow = window.parent || window;
        if (rootWindow.confirm("Script error occured. Do you reload this page ?\n\n" + errorDescription)) {
            rootWindow.location.reload(true);
        }
//20200610 TC竹中 ログイン時エラーダイアログを強制非表示 start
/*
    } else {
        if (icropScript.ui.isLoading) {
            if (window.parent == null || window.parent == window) {
                alert(icropScript.ui.words.reload);
                location.reload(true);
            }
        }
*/
//20200610 TC竹中 ログイン時エラーダイアログを強制非表示 end
    }
};

//公開API
icropScript.ShowMessageBox = function (code, word, detail, origin) {
    var originValue = ((origin === "S") ? "S" : "C"),
        codeValue = (code || ""),
        detailValue = (detail || ""),
        now = new Date(),
        year = (1900 + now.getYear()),
        month = now.getMonth() + 1,
        day = now.getDate(),
        hour = now.getHours(),
        min = now.getMinutes(),
        sec = now.getSeconds(),
        innerFunc = null,
        body = $("body"),
        backGround = $("<div class='icrop-message-background'></div>"),
        foreGround = $("<div class='icrop-message'><span class='icrop-message-timestamp'></span><br><span class='icrop-message-code'></span><br><span class='icrop-message-detail'></span></div>");

    if (month < 10) { month = "0" + month };
    if (day < 10) { day = "0" + day };
    if (hour < 10) { hour = "0" + hour };
    if (min < 10) { min = "0" + min };
    if (sec < 10) { sec = "0" + sec };

    backGround.width(body.width()).height(body.height());
    body.append(backGround);

    foreGround.children(".icrop-message-timestamp").text("[ " + year + "/" + month + "/" + day + " " + hour + ":" + min + ":" + sec + " (" + icropScript.ui.account + ")]");
    foreGround.children(".icrop-message-code").text(originValue + ":" + codeValue);
    foreGround.children(".icrop-message-detail").text(detailValue);
    body.append(foreGround);

    innerFunc = function () { alert(word); backGround.remove(); foreGround.remove(); };
    if (originValue === "S") {
        setTimeout(innerFunc, 1000);
    } else {
        innerFunc();
    }
};

icropScript.OperationLocked = false;

icropScript.tcvCallback = {};

//共通基盤 内部API
icropScript.ui = {
    curDialog: null,    //ダイアログが表示されている場合、ダイアログの土台となるオーバーレイオブジェクトを格納

    account: "",        //ログインユーザーアカウント

    words: {            //文言情報
        reload: 'Failed to connect to server. Tap OK to reload.'
    },

    isLoading: true,    //ロード中：true、ロード済：false

    enableScriptErrorNotification: false, //スクリプトエラーを表示する：true、表示しない：false

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
                    var lblForum = document.getElementById("MstPG_lblForum");
                    var addStatus = document.getElementById("addStatus");
                    var myIMG_blink = document.getElementById("myIMG_blink");
                    var myIMG = document.getElementById("myIMG");

                    if (addStatus != null) {
                        if (cnt == 0) {
                            addStatus.style.display = "none";
                            myIMG_blink.style.display = "none";
                            myIMG.style.display = "block";
                        } else {
                            lblForum.innerText = cnt;
                            addStatus.style.display = "block";
                            myIMG_blink.style.display = "block";
                            myIMG.style.display = "none";
                        }
                    }
                },
                error: function (e) { }
            });
        } catch (e) { }

        //ロック状態を返却
        var locked = $("#MstPG_OperationLocked");
        if (locked.val() == 1) {
            return "FALSE";
        } else {
            //商談中を判定する
            var status = $("#MstPG_PresenceCategory").val();
            if (status === "2") {
                return "TRUE";
            } else {
                return "TONE";
            }
        }
    },

    //通知画面の表示処理
    openNoticeDialog: function () {
        var myIMG_blink = document.getElementById("myIMG_blink");
        var myIMG = document.getElementById("myIMG");

        if (myIMG_blink != null) {
            if (myIMG_blink.style.display == "block") {
                setTimeout(function () { $("#myIMG_blink").trigger('showPopover'); }, 500);
            }
        }

        if (myIMG != null) {
            if (myIMG.style.display == "block") {
                setTimeout(function () { $("#myIMG").trigger('showPopover'); }, 500);
            }
        }
    },

    //未対応来店客件数セット+アイコン切替
    setVisitor: function () {
        try {
            var url = window.location.href.split("/Pages");
            var postTarget = url[0] + "/Services/IC3100201.asmx/GetNotDealCount";

            $.ajax({
                type: "POST",
                url: postTarget,
                contentType: "application/xml; charset=UTF-8",
                success: function (ret) {
                    //通知件数取得 
                    var cnt = ret.childNodes[0].childNodes[0].nodeValue;

                    //通知アイコン切替
                    var lblForum = document.getElementById("MstPG_lblVisitor");
                    var addStatus = document.getElementById("addStatusVisitor");
                    var myIMG_blink = document.getElementById("myVisit_blink");
                    var myIMG = document.getElementById("myVisit");

                    if (addStatus != null) {
                        if (cnt == 0) {
                            addStatus.style.display = "none";
                            myIMG_blink.style.display = "none";
                            myIMG.style.display = "block";
                        } else {
                            lblForum.innerText = cnt;
                            addStatus.style.display = "block";
                            myIMG_blink.style.display = "block";
                            myIMG.style.display = "none";
                        }
                    }
                },
                error: function (e) { }
            });
        } catch (e) { }

        //ロック状態を返却
        var locked = $("#MstPG_OperationLocked");
        if (locked.val() == 1) {
            return "FALSE";
        } else {
            //商談中を判定する
            var status = $("#MstPG_PresenceCategory").val();
            if (status === "2") {
                return "TRUE";
            } else {
                return "TONE";
            }
        }
    },

    //音声再生
    beep: function (beepKb) {

        var query = "";
        if (beepKb === 1) {
            // 区分=1:通知音
            query = "icrop:soundon:notice";
        } else if (beepKb === 2) {
            // 区分=2:警告音１
            query = "icrop:soundon:1";
        } else if (beepKb === 3) {
            // 区分=3:警告音２
            query = "icrop:soundon:2";
        } else {
            return;
        }

        location.href = query;

    },

    //未対応来店客一覧を表示
    openVisitorListDialog: function () {
        var myVisit_blink = document.getElementById("myVisit_blink");
        var myVisit = document.getElementById("myVisit");

        if (myVisit_blink != null) {
            if (myVisit_blink.style.display == "block") {
                setTimeout(function () { $("#myVisit_blink").trigger('showPopover'); }, 500);
            }
        }

        if (myVisit != null) {
            if (myVisit.style.display == "block") {
                setTimeout(function () { $("#myVisit").trigger('showPopover'); }, 500);
            }
        }
    },

    //管理者用通知一覧画面の表示処理
    openNoticeList: function () {
        if (parent.noticeListiFrame) {
            parent.noticeListiFrame.openNoticeList();

            icropScript.ui.beep(2);

            //ロック状態を返却
            return "FALSE";
        }
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

    //通知一覧終了処理
    $(document.body).bind("mousedown touchstart", function (e) {
        if (parent.noticeListiFrame) {
            if ($(e.target).is("#SC3040802Main, #SC3040802Main *") === false) {
                parent.noticeListiFrame.closeNoticeList();
            }
        }
    });
});
