/// <reference path="../jquery.js"/>
/**
* @fileOverview SC3080214 顧客詳細 共通
*
* @author TCS 寺本
* @version 1.0.0
*/

/**
* 初期処理
*/
(function (window) {

    $.extend(window, { SC3080214: {} });
    $.extend(SC3080214, {

        //デバッグフラグ
        isDebug: false,

        /**
        * @class 定数
        */
        constants: {
            onePageWidth: 1024,
            pageCount: 0 //ここの値はinit関数で設定
        },

        /**
        * スライド中かどうかをあらわすフラグ
        */
        moving: false,

        /**
        * ページ移動時に発生させるイベントハンドラのリスト
        * これはプライベートプロパティです
        */
        _pageMoveEventHandlers: new Array(),

        /**
        * 完了ボタン押下時に発生させるイベントハンドラのリスト
        * これはプライベートプロパティです
        */
        _registEventHandlers: new Array(),

        /**
        * ページ移動時に発生させるイベントハンドラを追加
        */
        addPageMoveEventHandler: function (func) {
            //リストに追加
            SC3080214._pageMoveEventHandlers.push(func);
        },

        /**
        * 登録ボタン押下時のイベントハンドラ追加
        */
        addRegistEventHandlers: function (func) {
            SC3080214._registEventHandlers.push(func);
        },

        /**
        * 新規活動中かどうかを示すフラグ(aspx.vb側からセットされる)
        */
        newActivityFlg: false,

        /**
        * 新規活動中に他画面に行こうとした場合に表示するメッセージ(aspx.vb側からセットされる)
        */
        redirectMessage: "",

        /**
        * 初期化
        */
        init: function () {

            //ページ数
            SC3080214.constants.pageCount = parseInt($("#CustDetailPageCountHidden").val());

            //ページマーク初期化
            $("#scNscCircleArea .customerDetail1Navi").toggle(SC3080214.constants.pageCount >= 1);
            $("#scNscCircleArea .customerDetail2Navi").toggle(SC3080214.constants.pageCount >= 2);
            $("#scNscCircleArea .customerDetail3Navi").toggle(SC3080214.constants.pageCount >= 3);
            SC3080214.setPageNavi();

            //スライドイベント
            var data = {};
            //$("#scNscAllBoxContentsArea").live("mousedown touchstart", data, SC3080214.start);

            //完了ボタンのリックを監視
            $("#RegistButton").bind("click", SC3080214._registButtonClick);


            //二重押下禁止処理
            SC3080214.initProcessControl();

            //popover用にロードイベント終了後に活性状態の制御を行う。
            setTimeout(function () {

                $("#scNscAllBoxContentsArea .scNscOneBoxContentsWrap").removeClass("loding");

                //入力項目の活性状態取得
                //SC3080214.savaAllInputDisabled();
                //アクティブでないページの入力項目を非活性にする
                //SC3080214.setInputDisabled(SC3080214.getActivePageClass())
                SC3080214.initTabMoveControl();

            }, 0);

        },

        start: function (e) {

            //var test = $(".mytest");
            //if (test.size() <= 0) {
            //  test = $("<div class='mytest' style='position:absolute;top:0px;left:0px;width:100px;height:100px;z-index:10000;background:#FFF;color:#000;'/>");
            //$(document.body).append(test);
            //}

            //アニメーション中は無視
            if (SC3080214.moving === true) return;
            if ($(e.target).is("textarea, input[type='text']") === true) return;

            //内部データクリア
            e.data = {};

            //開始位置と移動位置の初期値設定
            e.data.startPosition = SC3080214.getEventXY();
            e.data.movePosition = SC3080214.getEventXY();

            //フリック用
            e.data.startTime = (new Date()).getTime();

            //移動を監視
            $("#scNscAllBoxContentsArea").die("mousemove touchmove").live("mousemove touchmove", e.data, SC3080214.move);
            $("#scNscAllBoxContentsArea").die("mouseup mouseleave touchend").live("mouseup mouseleave touchend", e.data, SC3080214.end);
        },

        move: function (e) {

            //移動量計算
            var moveValue;
            var before = e.data.movePosition;
            var after = SC3080214.getEventXY();

            //移動距離を計算
            moveValue = SC3080214.calcmoveValue(before, after);

            //始点と現在の点の角度を求め、規定角度を超えたら移動しない。
            var r = SC3080214.getRotate(e.data.startPosition, after);
            if (Math.abs(Math.abs(Math.ceil(r)) - 90) <= 35) {
                e.data.movePosition = after;
                return;
            }

            //移動位置計算
            var translate = SC3080214.getTranslate();
            translate.left += moveValue.left;

            //はみ出さないよう調整
            if (translate.left > 0) translate.left = 0;
            if (translate.left < -1 * (SC3080214.constants.onePageWidth * (SC3080214.constants.pageCount - 1)))
                translate.left = -1 * (SC3080214.constants.onePageWidth * (SC3080214.constants.pageCount - 1));

            //TOP位置は固定
            translate.top = 0;
            //移動
            SC3080214.setTranslate(translate);

            //移動位置保存
            e.data.movePosition = after;

        },

        end: function (e) {

            var diffX = e.data.movePosition.x - e.data.startPosition.x;
            var timeDiff = (new Date()).getTime() - e.data.startTime;

            var flikClass = "";
            if (diffX > 300 || (timeDiff < 300 && diffX > 90)) {
                //左方向フリック
                flikClass = SC3080214.getpagemoveClass(-1);
                if (flikClass !== "") SC3080214.movepage(flikClass);
            } else if (diffX < -300 || (timeDiff < 300 && diffX < -80)) {
                //右方法フリック
                flikClass = SC3080214.getpagemoveClass(1);
                if (flikClass !== "") SC3080214.movepage(flikClass);
            }

            if (flikClass === "") {
                var pageClass = SC3080214.getCurpositionPageClass();
                SC3080214.movepage(pageClass);
            }

            //イベントバインド解除
            $("#scNscAllBoxContentsArea").die("mousemove touchmove");
            $("#scNscAllBoxContentsArea").die("mouseup mouseleave touchend");
        },

        getRotate: function (pos1, pos2) {
            return Math.atan2(pos2.y - pos1.y, pos2.x - pos1.x) * 180 / Math.PI;
        },

        /**
        * 移動量計算
        */
        calcmoveValue: function (before, after) {
            //移動距離を計算
            var moveValue = { top: 0, left: 0 };
            moveValue.left = after.x - before.x;
            moveValue.top = after.y - before.y;
            return moveValue;
        },

        ///////
        getpagemoveClass: function (moveValue) {
            var className = "";
            if (moveValue > 0) {
                //ページ１からページ２
                if ($("#scNscAllBoxContentsArea").hasClass("page1") === true) {
                    if (SC3080214.constants.pageCount >= 2) {
                        className = "page2";
                    } else {
                        className = "";
                    }
                }
                //ページ２からページ３
                if ($("#scNscAllBoxContentsArea").hasClass("page2") === true) {
                    if (SC3080214.constants.pageCount >= 3) {
                        className = "page3";
                    } else {
                        className = "";
                    }
                }
                if ($("#scNscAllBoxContentsArea").hasClass("page3") === true) className = "";
            } else {
                if ($("#scNscAllBoxContentsArea").hasClass("page1") === true) className = "";
                if ($("#scNscAllBoxContentsArea").hasClass("page2") === true) className = "page1";
                if ($("#scNscAllBoxContentsArea").hasClass("page3") === true) className = "page2";
            }
            return className;
        },

        /**
        * タップホールドの移動監視
        * @param {String} pageClass ページ番号をあらわすクラス
        */
        movepage: function (pageClass) {

            //ページ切り替えが発生する場合、登録済みイベントハンドラを呼び出し、ページ切り替え可否を問う
            if ($("#scNscAllBoxContentsArea").hasClass(pageClass) === false) {
                //ループ
                var moveOk = true;
                $.each(SC3080214._pageMoveEventHandlers, function (index, func) {
                    if ($.isFunction(func) === true) {
                        var ret = func.call(null, pageClass);
                        if (ret !== undefined && ret === false) {
                            //ページ切り替えキャンセルがハンドラから帰ってきた場合
                            moveOk = false;
                            return false;
                        }
                    }
                });
            }

            if (moveOk === false) {
                //切り替えキャンセル
                var cancelPageClass = "page1";
                for (var i = 1; i <= SC3080214.constants.pageCount; i++) {
                    if ($("#scNscAllBoxContentsArea").hasClass("page" + i) === true) cancelPageClass = "page" + i;
                }
                //元のページに戻すアクションを実行
                SC3080214.executeSlidePage(cancelPageClass);
            } else {
                //bodyクリックでポップアップなどを消す
                //var curPageClass = SC3080214.getCurpositionPageClass();
                var curPageClass = SC3080214.getActivePageClass();

                //SC3080214.setDebugMessage(pageClass);

                if (curPageClass != pageClass) {

                    //擬似クリックを発生させ、出ているポップアップを消す
                    $("#bodyFrame").trigger("click");

                    //フォーム系部品のフォーカスアウト
                    $("input[type='text'], textarea").blur();

                    setTimeout(function () {
                        //現ページの入力項目活性状態を保存する
                        //SC3080214.savaInputDisabled(curPageClass);

                        //スライド先ページの入力項目活性状態を復元する
                        //SC3080214.setInputDisabled(pageClass);
                    }, 0);

                }

                //切替
                SC3080214.executeSlidePage(pageClass);
            }

            //保存
            $("#PageNumberClassHidden").val(pageClass);
        },

        /**
        * ページのスライド実行
        */
        executeSlidePage: function (pageClass) {

            //LEFT位置
            var left = 0;
            if (pageClass === "page1") left = 0;
            if (pageClass === "page2") left = (-1) * SC3080214.constants.onePageWidth;
            if (pageClass === "page3") left = (-2) * SC3080214.constants.onePageWidth;

            //マーカー設定
            $("#scNscAllBoxContentsArea").removeClass("page1 page2 page3").addClass(pageClass);

            //アニメーションする必要があるかチェック
            var translate = SC3080214.getTranslate();
            if (translate.left !== left) {
                //アニメーション設定
                SC3080214.moving = true;
                $("#scNscAllBoxContentsArea").css("-webkit-transition", "transform 400ms ease-out 0")
                .one("webkitTransitionEnd", function (e) {
                    //終了
                    $("#scNscAllBoxContentsArea").css({ "-webkit-transition": "none" });
                    //ページ上部のナビゲーション
                    SC3080214.setPageNavi();
                    SC3080214.moving = false;
                });
                //移動
                translate.left = left;
                SC3080214.setTranslate(translate);
            } else {
                //ページ上部のナビゲーション
                SC3080214.setPageNavi();
            }

            //フッター
            SC3080214.setFootNavi(pageClass);

        },

        /**
        * 現在のLeft位置から表示されるべきページ番号のクラス名取得
        */
        getCurpositionPageClass: function () {
            return SC3080214.getPageClassFromLeftPosition(Math.abs(SC3080214.getTranslate().left));
        },

        /**
        * Left位置から表示されるべきページ番号のクラス名取得
        */
        getPageClassFromLeftPosition: function (left) {
            var page = Math.floor(left / SC3080214.constants.onePageWidth) + 1;
            if (left % SC3080214.constants.onePageWidth > SC3080214.constants.onePageWidth / 2) page++;
            return "page" + page;
        },

        /**
        * 現在アクティブになっているページクラス名を取得
        */
        getActivePageClass: function () {
            return $("#scNscAllBoxContentsArea").hasClass("page1") === true ? "page1" :
                        $("#scNscAllBoxContentsArea").hasClass("page2") === true ? "page2" : "page3"
        },

        /**
        * イベント発生x,y座標取得
        * @return {Position} 位置
        */
        getEventXY: function () {
            //イベント発生時のx,y座標を返却(PC/iPADを考慮)
            return event.changedTouches !== undefined && event.changedTouches
                       ? { x: event.changedTouches[0].clientX, y: event.changedTouches[0].clientY }
                       : { x: event.pageX, y: event.pageY };
        },

        /**
        * 現在のtop位置、left位置を取得
        * @return {Position} 位置
        */
        getTranslate: function () {
            var matrix = new WebKitCSSMatrix(window.getComputedStyle($("#scNscAllBoxContentsArea").get(0)).webkitTransform);
            return { top: parseInt(matrix.f), left: parseInt(matrix.e) };
        },

        /**
        * top,left位置設定
        * @param {Position} position 位置
        */
        setTranslate: function (position) {
            return $("#scNscAllBoxContentsArea").css({ "transform": "translate3d(" + position.left + "px, " + position.top + "px, 0px)" });
        },

        /**
        * ページ上部のナビゲーション設定
        */
        setPageNavi: function () {
            var naviClass;
            //●をつけるエレメントを特定
            if ($("#scNscAllBoxContentsArea").hasClass("page1") === true) naviClass = ".customerDetail1Navi";
            if ($("#scNscAllBoxContentsArea").hasClass("page2") === true) naviClass = ".customerDetail2Navi";
            if ($("#scNscAllBoxContentsArea").hasClass("page3") === true) naviClass = ".customerDetail3Navi";
            //クリア
            $("#scNscCircleArea > p.customerDetail1Navi,#scNscCircleArea > p.customerDetail2Navi,#scNscCircleArea > p.customerDetail3Navi")
            .removeClass("scNscCircleOn")
            .addClass("scNscCircleOff")
            .filter(naviClass)
            .removeClass("scNscCircleOff")
            .addClass("scNscCircleOn");
        },

        /**
        * フッター
        */
        setFootNavi: function (pageClass) {
            if (pageClass !== "page3") {
                $(".RegisterButtonWrap").fadeOut(400);
            } else {
                $(".RegisterButtonWrap").fadeIn(400);
            }
        },

        /**
        * 完了ボタンクリック
        */
        _registButtonClick: function (e) {

            var checkOk = true;
            $.each(SC3080214._registEventHandlers, function (index, func) {
                if ($.isFunction(func) === true) {
                    var ret = func.call(null);
                    if (ret !== undefined && ret === false) {
                        //ページ切り替えキャンセルがハンドラから帰ってきた場合
                        checkOk = false;
                        return false;
                    }
                }
            });

            if (checkOk === true) {
                SC3080214.showLoding();
            }

            //チェック結果返却
            return checkOk;
        },

        /******************************************************************************
        ここから二重押下禁止処理
        ******************************************************************************/

        /**
        * サーバー処理を実行中かどうか
        */
        serverProcessing: false,

        /**
        * Ms Ajax リクエストマネージャー
        */
        requestMgr: null,

        /**
        * 二重起動監視処理の初期化
        */
        initProcessControl: function () {

            //リクエストマネージャー設定
            SC3080214.requestMgr = Sys.WebForms.PageRequestManager.getInstance();

            Sys.Application.add_load(SC3080214.appLoad);                                //同期・非同期通信完了後のロードイベント
            SC3080214.requestMgr.add_initializeRequest(SC3080214.initAsyncPostback);    //非同期通信開始前の処理を追加
            SC3080214.requestMgr.add_endRequest(SC3080214.endAsyncPostback);            //非同期通信終了後の処理を追加

            //同期ポストバックのボタンの二度押し禁止
            $("form").bind("submit", function (e) {
                //alert("sub");
                if (window.event.returnValue === true) {

                    //処理キャンセル
                    if (SC3080214.serverProcessing === true) {
                        SC3080214.setDebugMessage("現在処理中です・・・S");
                        window.event.returnValue = false;
                        return false;
                    }

                    //処理中フラグを立てる
                    SC3080214.serverProcessing = true;
                }
            });

            //リンクボタンの同期ポストバック制御
            var _originalSubmit = $("form").get(0).submit;
            $("form").get(0).submit = function () {
                //alert("ppp");
                //処理キャンセル
                if (SC3080214.serverProcessing === true) {
                    SC3080214.setDebugMessage("現在処理中です・・・L");
                    return;
                }

                //処理中フラグを立てる
                SC3080214.serverProcessing = true;
                _originalSubmit.call(this, arguments);
            };

        },

        /**
        * 非同期通信の開始を監視し、２重起動を防止する
        */
        initAsyncPostback: function (sender, args) {
            var cancelFlg = false;

            //サーバー処理中
            if (SC3080214.serverProcessing === true) cancelFlg = true;

            //別の非同期通信が起動中
            if (SC3080214.requestMgr.get_isInAsyncPostBack() === true) cancelFlg = true;

            //処理中の場合はキャンセル
            if (cancelFlg === true) {

                SC3080214.setDebugMessage("現在処理中です・・・");
                args.set_cancel(true);
            } else {
                SC3080214.showLoding();
                SC3080214.serverProcessing = true;
            }

        },

        /**
        * 非同期通信終了
        */
        endAsyncPostback: function (sender, args) {
            if (args.get_error() !== null) {
                //サーバーエラー
                if (SC3080214.requestMgr.get_isInAsyncPostBack() === false) SC3080214.serverProcessing = false;
            }
            if (SC3080214.requestMgr.get_isInAsyncPostBack() === false) SC3080214.closeLoding();
        },

        /**
        * ロード処理(同期・非同期共通）
        */
        appLoad: function () {
            //処理中フラグ変更
            SC3080214.serverProcessing = false;
        },

        //デバッグメッセージ表示
        setDebugMessage: function (msg) {
            if (SC3080214.isDebug === true) {

                var div = $("<div style='border:2px solid #CCF; width:200px; height:60px;position:absolute;bottom:52px;right:-200px;background:#EEE;'/>");
                $(document.body).append(div);

                div.text(msg).animate({ right: 0 }, 700, function () {
                    setTimeout(function () {
                        div.fadeOut(200, function () {
                            div.remove();
                        });
                    }, 3000);
                });
            }
        },

        /**
        * コールバック開始
        */
        startServerCallback: function () {
            SC3080214.serverProcessing = true;
            SC3080214.showLoding();
        },


        /**
        * コールバック終了
        */
        endServerCallback: function () {
            SC3080214.serverProcessing = false;
            SC3080214.closeLoding();
        },

        /******************************************************************************
        読み込み中表示
        ******************************************************************************/

        /**
        * 読み込み中アイコン表示
        */
        showLoding: function () {

            //オーバーレイ表示
            $("#registOverlayBlack").css("display", "block");
            //アニメーション
            setTimeout(function () {
                $("#processingServer").addClass("show");
                $("#registOverlayBlack").addClass("open");
            }, 0);

        },

        /**
        * 読み込み中アイコンを非表示にする
        */
        closeLoding: function () {
            $("#processingServer").removeClass("show");
            $("#registOverlayBlack").removeClass("open").one("webkitTransitionEnd", function (we) {
                $("#registOverlayBlack").css("display", "none");
            });
        },

        /******************************************************************************
        戻る・ログアウト・メニューボタンの制御
        ******************************************************************************/

        /**
        * 新規活動破棄チェック
        */
        cancellationCheck: function () {

            if (SC3080214.newActivityFlg === true) {
                //新規活動中
                return confirm(SC3080214.redirectMessage);
            }
            return true;
        },


        /******************************************************************************
        前へ・次へキー対応の為の、入力項目活性制御
        ******************************************************************************/


        /**
        * 前へ・次へタブフォーカス遷移の制御
        */
        initTabMoveControl: function () {

            $(".dummyFocusControlText").bind("focus", function () {
                var target = this;
                setTimeout(function () {
                    $(target).blur();
                    $("#bodyFrame").trigger("click");
                    //SC3080214.setDebugMessage(target.id);
                }, 0);
            });


            //前へ・次へキーで発生するスクロールイベントを監視
            $("#scNscOnePageDisplayArea").bind("scroll", function (e) {
                //連打対応
                $("#scNscOnePageDisplayArea").scrollLeft(0);
                return false;
            });
        },


        /**
        * 活性状態を保存(全ページ)
        */
        savaAllInputDisabled: function () {

            //入力要素の活性状態を取得し、独自拡張属性に設定する
            $("#custDtlPage1 :input, #custDtlPage2 :input, #custDtlPage3 :input").not("input[type=hidden]").each(function () {
                //保存
                $(this).attr("data-original-disabled", $(this).is(":disabled"));
            });
        },

        /**
        * 活性状態を保存(1ページ)
        */
        savaInputDisabled: function (activePage) {

            var targetWrapId = SC3080214.getPageClassToWrapDivId(activePage);

            //入力要素の活性状態を取得し、独自拡張属性に設定する
            $(targetWrapId + " :input").not("input[type=hidden]").each(function () {
                //保存
                $(this).attr("data-original-disabled", $(this).is(":disabled"));
            });
        },

        /**
        * ページクラス名から、対象ページを囲うDIVタグのIDを取得する
        */
        getPageClassToWrapDivId: function (pageClass) {
            if (pageClass === "page1") return "#custDtlPage1";
            if (pageClass === "page2") return "#custDtlPage2";
            if (pageClass === "page3") return "#custDtlPage3";
        },

        /**
        * 入力項目の活性状態を設定
        */
        setInputDisabled: function (activePage) {
            return;
            var targetWrapId = SC3080214.getPageClassToWrapDivId(activePage);

            //対象以外の要素を非活性にする
            $.each(["#custDtlPage1", "#custDtlPage2", "#custDtlPage3"], function (index, id) {
                if (targetWrapId !== id) $(id + " :input").not("input[type=hidden]").attr("disabled", "disabled");
            });

            //アクティブページの活性状態を復元する
            $(targetWrapId + " :input").not("input[type=hidden]").each(function () {

                //保存されているか(後から動的に追加されたタグは対象外)
                if (typeof $(this).attr("data-original-disabled") !== "string") return true;

                //属性に設定する活性状態を判定
                var disabledValue = $(this).attr("data-original-disabled") === "true" ? "disabled" : "";

                //判定した活性状態を設定
                $(this).attr("disabled", disabledValue);

            });

        }

    });

})(window);

/**
* DOMロード時の処理
*/
$(function () {

    //$("#scNscAllBoxContentsArea").addClass("page3");
    SC3080214.init();

});
