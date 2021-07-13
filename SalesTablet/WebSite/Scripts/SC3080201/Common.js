/// <reference path="../jquery.js"/>
/**
* @fileOverview SC3080201 顧客詳細 共通
*
* @author TCS 寺本
* @version 1.0.0
*/

//━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
//Common.js
//─────────────────────────────────────
//機能： 顧客詳細 共通
//補足： 
//作成： 2011/10/24 TCS 寺本
//更新： 2012/04/24 TCS 安田 【SALES_1A】初期入力時に、画面が少し動くバグ修正（ユーザー課題 No.71）
//更新： 2012/04/26 TCS 河原 HTMLエンコード対応
//更新： 2012/05/09 TCS 河原 【SALES_1A】お客様メモエリアで左スワイプで、商談メモへ切り替えられない
//更新： 2012/05/17 TCS 安田 クルクル対応
//更新： 2012/06/01 TCS 河原 FS開発
//更新： 2012/07/31 TCS 河原 【A STEP2】次世代e-CRB 新車商談機能改善
//更新： 2013/03/06 TCS 河原 GL0874
//更新： 2013/03/26 TCS 河原 GL0876
//更新： 2013/01/18 TCS 坪根 【A.STEP2】次世代e-CRB 新車タブレットSC活動支援機能開発
//更新： 2013/12/12 TCS 市川 Aカード情報相互連携開発
//更新： 2014/07/10 TCS 外崎 TMT BTS-UAT-74
//更新： 2014/11/04 TCS 藤井 iOS8 対応(i-CROP_V4_salesよりマージ)
//更新： 2018/07/10 TCS 河原 TKM Next Gen e-CRB Project Application development Block B-1
//更新： 2019/02/13 TCS 河原 顧客編集ポップアップを欄外タップで閉じないように変更
//─────────────────────────────────────

/**
* 初期処理
*/
(function (window) {

    $.extend(window, { SC3080201: {} });
    $.extend(SC3080201, {

        //デバッグフラグ
        isDebug: false,

        /**
        * @class 定数
        */
        constants: {
            onePageWidth: 1024,
            pageCount: 0 //ここの値はinit関数で設定
        },

        requirePartialScript: function (partialScriptUrl, partialScriptCallback) {
            if (SC3080201._loadedPartialScripts[partialScriptUrl] !== undefined) {
                partialScriptCallback();
                return;
            }

            $.ajax({
                url: partialScriptUrl,
                dataType: "script",
                cache: true,
                async: false,
                success: function () {
                    SC3080201._loadedPartialScripts[partialScriptUrl] = true;
                    partialScriptCallback();
                }
            });
        },
        _loadedPartialScripts: {},

        /**
        * スライド中かどうかをあらわすフラグ
        */
        moving: false,

        // 2012/05/17 TCS 安田 クルクル対応 START
        /**
        * 活動結果登録(false)／受注後工程フォロー(true)かどうかをあらわすフラグ
        */
        salesAfterFlg: false,
        // 2012/05/17 TCS 安田 クルクル対応 END

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
            SC3080201._pageMoveEventHandlers.push(func);
        },

        /**
        * 登録ボタン押下時のイベントハンドラ追加
        */
        addRegistEventHandlers: function (func) {
            SC3080201._registEventHandlers.push(func);
        },

        /**
        * 新規活動中かどうかを示すフラグ(aspx.vb側からセットされる)
        */
        newActivityFlg: false,

        /**
        * 新規活動中に他画面に行こうとした場合に表示するメッセージ(aspx.vb側からセットされる)
        */
        redirectMessage: "",

        //2013/12/12 TCS 市川 Aカード情報相互連携開発 START
        /**
        * 非同期通信中の待機アニメーション(クルクル)を表示させるか示すフラグ
        */
        asyncAnimationEnable: true,
        //2013/12/12 TCS 市川 Aカード情報相互連携開発 END

        /**
        * 初期化
        */
        init: function () {

            //ページ数
            SC3080201.constants.pageCount = parseInt($("#CustDetailPageCountHidden").val());

            //ページマーク初期化
            $("#scNscCircleArea .customerDetail1Navi").toggle(SC3080201.constants.pageCount >= 1);
            $("#scNscCircleArea .customerDetail2Navi").toggle(SC3080201.constants.pageCount >= 2);
            $("#scNscCircleArea .customerDetail3Navi").toggle(SC3080201.constants.pageCount >= 3);
            SC3080201.setPageNavi();

            //スライドイベント
            var data = {};
            //2012/03/14 TCS 寺本 【SALES_2】 START
            //$("#scNscAllBoxContentsArea").live("mousedown touchstart", data, SC3080201.start);
            $("#scNscAllBoxContentsArea").bind("mousedown touchstart", data, SC3080201.start);
            //2012/03/14 TCS 寺本 【SALES_2】 END

            //完了ボタンのリックを監視
            $("#RegistButton").bind("click", SC3080201._registButtonClick);

            //完了ボタンの表示可否
            var pageClass = SC3080201.getCurpositionPageClass();
            SC3080201.setFootNavi(pageClass);

            //二重押下禁止処理
            SC3080201.initProcessControl();

            //popover用にロードイベント終了後に活性状態の制御を行う。
            setTimeout(function () {

                //初期表示時のちらつき防止＆性能対策の為削除
                //$("#scNscAllBoxContentsArea .scNscOneBoxContentsWrap").removeClass("loding");

                //入力項目の活性状態取得
                //SC3080201.savaAllInputDisabled();
                //アクティブでないページの入力項目を非活性にする
                //SC3080201.setInputDisabled(SC3080201.getActivePageClass())
                SC3080201.initTabMoveControl();

            }, 0);

            //触履歴タブ表示 (非活性時)
            if ($("#ReadOnlyFlagHidden").val() == "1") {
                $("#TabAll").removeClass("scNscCurriculumTabAllAc");
                $("#TabAll").addClass("scNscCurriculumTabSalesOff");
            }

            //E-Mailクリック時の動作抑制 (非活性時)
            $("#EmailLink").bind("click", function (e) {
                if ($("#ReadOnlyFlagHidden").val() == "1") {
                    e.preventDefault();
                }
            });

        },

        start: function (e) {

            //var test = $(".mytest");
            //if (test.size() <= 0) {
            //  test = $("<div class='mytest' style='position:absolute;top:0px;left:0px;width:100px;height:100px;z-index:10000;background:#FFF;color:#000;'/>");
            //$(document.body).append(test);
            //}

            //2019/02/13 TCS 河原 顧客編集ポップアップを欄外タップで閉じないように変更 START
            //顧客編集ポップアップ表示時中は無視
            if ($("#scNscCustomerEditingWindown").is(":visible") === true) return;
            //2019/02/13 TCS 河原 顧客編集ポップアップを欄外タップで閉じないように変更 END

            //アニメーション中は無視
            if (SC3080201.moving === true) return;
            if ($(e.target).is("textarea, input[type='text']") === true) return;

            //内部データクリア
            e.data = {};

            //開始位置と移動位置の初期値設定
            e.data.startPosition = SC3080201.getEventXY();
            e.data.movePosition = SC3080201.getEventXY();

            //フリック用
            e.data.startTime = (new Date()).getTime();

            //2012/06/01 TCS 河原 FS開発 START
            //移動フラグ(1年後くらいに消すため使用する場合は注意すること)
            this_form.MoveFlg.value = "0"
            //2012/06/01 TCS 河原 FS開発 END

            //移動を監視
            //2012/03/14 TCS 寺本 【SALES_2】 START
            //$("#scNscAllBoxContentsArea").die("mousemove touchmove").live("mousemove touchmove", e.data, SC3080201.move);
            //$("#scNscAllBoxContentsArea").die("mouseup mouseleave touchend").live("mouseup mouseleave touchend", e.data, SC3080201.end);
            $("#scNscAllBoxContentsArea").unbind("mousemove touchmove").bind("mousemove touchmove", e.data, SC3080201.move);
            $("#scNscAllBoxContentsArea").unbind("mouseup mouseleave touchend").bind("mouseup mouseleave touchend", e.data, SC3080201.end);
            //2012/03/14 TCS 寺本 【SALES_2】 END
        },

        move: function (e) {

            if (e.originalEvent.touches && 1 < e.originalEvent.touches.length) {
                return;
            }

            //移動量計算
            var moveValue;
            var before = e.data.movePosition;
            var after = SC3080201.getEventXY();

            //移動距離を計算
            moveValue = SC3080201.calcmoveValue(before, after);

            //2012/06/01 TCS 河原 FS開発 START
            if ($("#MoveFlg").size() > 0) {
                var diffX = e.data.movePosition.x - e.data.startPosition.x;
                var diffY = e.data.movePosition.y - e.data.startPosition.y;

                //縦か横に一定量移動していたら移動フラグを立てる
                if (Math.abs(diffX) > 5 || Math.abs(diffY) > 5) {
                    this_form.MoveFlg.value = "1"
                } else {
                    this_form.MoveFlg.value = "0"
                }
            }
            //2012/06/01 TCS 河原 FS開発 END

            //始点と現在の点の角度を求め、規定角度を超えたら移動しない。
            var r = SC3080201.getRotate(e.data.startPosition, after);
            if (Math.abs(Math.abs(Math.ceil(r)) - 90) <= 35) {
                e.data.movePosition = after;
                return;
            }

            //移動位置計算
            var translate = SC3080201.getTranslate();
            translate.left += moveValue.left;

            //はみ出さないよう調整
            if (translate.left > 0) translate.left = 0;
            if (translate.left < -1 * (SC3080201.constants.onePageWidth * (SC3080201.constants.pageCount - 1)))
                translate.left = -1 * (SC3080201.constants.onePageWidth * (SC3080201.constants.pageCount - 1));

            //TOP位置は固定
            translate.top = 0;
            //移動
            SC3080201.setTranslate(translate);

            //移動位置保存
            e.data.movePosition = after;

        },

        end: function (e) {

            var diffX = e.data.movePosition.x - e.data.startPosition.x;
            var timeDiff = (new Date()).getTime() - e.data.startTime;

            var flikClass = "";
            if (diffX > 300 || (timeDiff < 300 && diffX > 90)) {
                //左方向フリック
                flikClass = SC3080201.getpagemoveClass(-1);
                if (flikClass !== "") SC3080201.movepage(flikClass);
            } else if (diffX < -300 || (timeDiff < 300 && diffX < -80)) {
                //右方法フリック
                flikClass = SC3080201.getpagemoveClass(1);
                if (flikClass !== "") SC3080201.movepage(flikClass);
            }

            if (flikClass === "") {
                var pageClass = SC3080201.getCurpositionPageClass();
                SC3080201.movepage(pageClass);
            }

            //イベントバインド解除
            //2012/03/14 TCS 寺本 【SALES_2】 START
            //$("#scNscAllBoxContentsArea").die("mousemove touchmove");
            //$("#scNscAllBoxContentsArea").die("mouseup mouseleave touchend");
            $("#scNscAllBoxContentsArea").unbind("mousemove touchmove");
            $("#scNscAllBoxContentsArea").unbind("mouseup mouseleave touchend");
            //2012/03/14 TCS 寺本 【SALES_2】 END

            //2012/05/09 TCS 河原 【SALES_1A】お客様メモエリアで左スワイプで、商談メモへ切り替えられない START
            //移動量を確認
            if ($("#CustomerMemoDummyAreaFlg").size() > 0) {
                if (Math.abs(diffX) > 10) {
                    this_form.CustomerMemoDummyAreaFlg.value = "1"
                } else {
                    this_form.CustomerMemoDummyAreaFlg.value = "0"
                }
            }
            //2012/05/09 TCS 河原 【SALES_1A】お客様メモエリアで左スワイプで、商談メモへ切り替えられない END
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
                    if (SC3080201.constants.pageCount >= 2) {
                        className = "page2";
                    } else {
                        className = "";
                    }
                }
                //ページ２からページ３
                if ($("#scNscAllBoxContentsArea").hasClass("page2") === true) {
                    if (SC3080201.constants.pageCount >= 3) {
                        className = "page3";

                        //2012/07/31 TCS 河原 【A STEP2】次世代e-CRB 新車商談機能改善 START
                        //2012/07/31 TCS 河原 【A STEP2】次世代e-CRB 新車商談機能改善 END

                        /* 2013/01/18 TCS 坪根 【A.STEP2】次世代e-CRB 新車タブレットSC活動支援機能開発 START */
                        /* 2018/07/10 TCS 河原 TKM Next Gen e-CRB Project Application development Block B-1 DELETE */
                        //最初に3ページ目に遷移した場合のみ、査定ボタンをOFF自動設定
                        if (this_form.DispPage3Flg.value == "0" && $("#PageMoveFlgHidden").val() == "True") {
                            //査定依頼機能フラグをHiddenに設定
                            this_form.Sc3080218selectActAssesment.value = "0";
                            this_form.Sc3080218selectActAssesmentWK.value = "0";

                            //プロセス(査定)ボタンの設定
                            Sc3080218ActAssesmentButtonOnOff("0");
                        }
                        /* 2013/01/18 TCS 坪根 【A.STEP2】次世代e-CRB 新車タブレットSC活動支援機能開発 END */
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
                $.each(SC3080201._pageMoveEventHandlers, function (index, func) {
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
                for (var i = 1; i <= SC3080201.constants.pageCount; i++) {
                    if ($("#scNscAllBoxContentsArea").hasClass("page" + i) === true) cancelPageClass = "page" + i;
                }
                //元のページに戻すアクションを実行
                SC3080201.executeSlidePage(cancelPageClass);
            } else {
                //bodyクリックでポップアップなどを消す
                //var curPageClass = SC3080201.getCurpositionPageClass();
                var curPageClass = SC3080201.getActivePageClass();

                //SC3080201.setDebugMessage(pageClass);

                if (curPageClass != pageClass) {

                    //擬似クリックを発生させ、出ているポップアップを消す
                    $("#bodyFrame").trigger("click");

                    //フォーム系部品のフォーカスアウト
                    $("input[type='text'], textarea").blur();

                    setTimeout(function () {
                        //現ページの入力項目活性状態を保存する
                        //SC3080201.savaInputDisabled(curPageClass);

                        //スライド先ページの入力項目活性状態を復元する
                        //SC3080201.setInputDisabled(pageClass);
                    }, 0);

                }

                //切替
                SC3080201.executeSlidePage(pageClass);
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
            if (pageClass === "page2") left = (-1) * SC3080201.constants.onePageWidth;
            if (pageClass === "page3") left = (-2) * SC3080201.constants.onePageWidth;

            //マーカー設定
            $("#scNscAllBoxContentsArea").removeClass("page1 page2 page3").addClass(pageClass);

            //アニメーションする必要があるかチェック
            var translate = SC3080201.getTranslate();
            if (translate.left !== left) {
                //アニメーション設定
                SC3080201.moving = true;
                //2014/11/04 TCS 藤井 iOS8 対応(i-CROP_V4_salesよりマージ)  Start
                $("#scNscAllBoxContentsArea").css("-webkit-transition", "transform 400ms ease-out 0");
                setTimeout(function () {
                //2014/11/04 TCS 藤井 iOS8 対応(i-CROP_V4_salesよりマージ)  End
                    //終了
                    $("#scNscAllBoxContentsArea").css({ "-webkit-transition": "none" });
                    //ページ上部のナビゲーション
                    SC3080201.setPageNavi();
                    SC3080201.moving = false;
                    //2013/03/06 TCS 河原 GL0874 START
                    //3ページ目にスライドしたタイミングで、キャンセル後の結果登録であればメッセージを表示する
                    if ($("#scNscAllBoxContentsArea").hasClass("page3") === true && ($("#SC3080201ContractCancelFlg").val() == "1" || $("#SC3080202ContractCancelFlg").val() == "1")) {
                        icropScript.ShowMessageBox(0, $("#ErrWord6").val(), "");
                        $("#SC3080201ContractCancelFlg").val("0");
                        $("#SC3080202ContractCancelFlg").val("0");
                    }
                    //2013/03/06 TCS 河原 GL0874 END

                    //2013/03/06 TCS 河原 GL0876 END
                    //最初に3ページ目に遷移したときの時間を設定
                    if ($("#scNscAllBoxContentsArea").hasClass("page3") === true) {
                        if (this_form.DispPage3Flg.value == "0" && $("#PageMoveFlgHidden").val() == "True") {
                            var updateFlg = "0"
                            if ($("#SC3080203UpdateRWFlg").size() > 0) {
                                if (this_form.SC3080203UpdateRWFlg.value == "1") {
                                    updateFlg = "1"
                                }
                            }
                            if ($("#SC3080216UpdateRWFlg").size() > 0) {
                                if (this_form.SC3080216UpdateRWFlg.value == "1") {
                                    updateFlg = "1"
                                }
                            }
                            if (updateFlg == "1") {
                                var now = new Date()
                                var h = now.getHours()
                                if (h < 10) h = "0" + h;
                                var m = now.getMinutes()
                                if (m < 10) m = "0" + m;
                                $("#FastDispTime").val(h + ":" + m);
                                $("#Sc3080218ActTimeToSelector").val($("#FastDispTime").attr("value"));
                                $("#Sc3080218ActTimeToSelectorWK").val($("#FastDispTime").attr("value"));
                                $("#Sc3080218ActTimeToSelectorWK2").val($("#FastDispTime").attr("value"));
                                $(".ActTime").text(getDisplayDate218WK("ActTime"));
                                this_form.DispPage3Flg.value = "1";
                                this_form.SC3080218UpdateRWFlg.value = "1"
                                this_form.Sc3080218ActTimePopupFlg.value = "0"
                            }
                        }
                    }
                    //2013/03/06 TCS 河原 GL0876 END

                //2014/11/04 TCS 藤井 iOS8 対応(i-CROP_V4_salesよりマージ)  Start
                }, 400);
                //2014/11/04 TCS 藤井 iOS8 対応(i-CROP_V4_salesよりマージ)  End

                //移動
                translate.left = left;
                SC3080201.setTranslate(translate);
            } else {
                //ページ上部のナビゲーション
                SC3080201.setPageNavi();
            }

            //フッター
            SC3080201.setFootNavi(pageClass);

        },

        /**
        * 現在のLeft位置から表示されるべきページ番号のクラス名取得
        */
        getCurpositionPageClass: function () {
            return SC3080201.getPageClassFromLeftPosition(Math.abs(SC3080201.getTranslate().left));
        },

        /**
        * Left位置から表示されるべきページ番号のクラス名取得
        */
        getPageClassFromLeftPosition: function (left) {
            var page = Math.floor(left / SC3080201.constants.onePageWidth) + 1;
            if (left % SC3080201.constants.onePageWidth > SC3080201.constants.onePageWidth / 2) page++;
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
            $.each(SC3080201._registEventHandlers, function (index, func) {
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

                //2012/05/17 TCS 安田 クルクル対応 START

                //再表示判定Function
                function registTimerFunc() {

                    //二度押し防止フラグをキャンセル(false)にする
                    SC3080201.serverProcessing = false;

                    //再表示用ボタン押下
                    if (SC3080201.salesAfterFlg === true) {
                        //受注後工程フォロー
                        $("#refreshProgramHidden").val("SC3080216");
                    } else {
                        //活動結果登録
                        $("#refreshProgramHidden").val("SC3080203");
                    }
                    $("#refreshButton").click();

                    //繰り返し処理をする
                    return true;
                }

                //タイマーセット
                commonRefreshTimer(registTimerFunc);

                //2012/05/17 TCS 安田 クルクル対応 END

                SC3080201.showLoding();
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
            SC3080201.requestMgr = Sys.WebForms.PageRequestManager.getInstance();

            Sys.Application.add_load(SC3080201.appLoad);                                //同期・非同期通信完了後のロードイベント
            SC3080201.requestMgr.add_initializeRequest(SC3080201.initAsyncPostback);    //非同期通信開始前の処理を追加
            SC3080201.requestMgr.add_endRequest(SC3080201.endAsyncPostback);            //非同期通信終了後の処理を追加

            //同期ポストバックのボタンの二度押し禁止
            $("form").bind("submit", function (e) {
                //alert("sub");
                if (window.event.returnValue === true) {

                    //処理キャンセル
                    if (SC3080201.serverProcessing === true) {
                        SC3080201.setDebugMessage("現在処理中です・・・S");
                        window.event.returnValue = false;
                        return false;
                    }

                    //処理中フラグを立てる
                    SC3080201.serverProcessing = true;
                }
            });

            //リンクボタンの同期ポストバック制御
            var _originalSubmit = $("form").get(0).submit;
            $("form").get(0).submit = function () {
                //alert("ppp");
                //処理キャンセル
                if (SC3080201.serverProcessing === true) {
                    SC3080201.setDebugMessage("現在処理中です・・・L");
                    return;
                }

                //処理中フラグを立てる
                SC3080201.serverProcessing = true;
                _originalSubmit.call(this, arguments);
            };

        },

        /**
        * 非同期通信の開始を監視し、２重起動を防止する
        */
        initAsyncPostback: function (sender, args) {
            var cancelFlg = false;

            //サーバー処理中
            if (SC3080201.serverProcessing === true) cancelFlg = true;

            //別の非同期通信が起動中
            if (SC3080201.requestMgr.get_isInAsyncPostBack() === true) cancelFlg = true;

            //処理中の場合はキャンセル
            if (cancelFlg === true) {

                SC3080201.setDebugMessage("現在処理中です・・・");
                args.set_cancel(true);
            } else {
                SC3080201.showLoding();
                SC3080201.serverProcessing = true;
            }

        },

        /**
        * 非同期通信終了
        */
        endAsyncPostback: function (sender, args) {
            if (args.get_error() !== null) {
                //サーバーエラー
                if (SC3080201.requestMgr.get_isInAsyncPostBack() === false) SC3080201.serverProcessing = false;
            }
            if (SC3080201.requestMgr.get_isInAsyncPostBack() === false) SC3080201.closeLoding();
        },

        /**
        * ロード処理(同期・非同期共通）
        */
        appLoad: function () {
            //処理中フラグ変更
            SC3080201.serverProcessing = false;
        },

        //デバッグメッセージ表示
        setDebugMessage: function (msg) {
            if (SC3080201.isDebug === true) {

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
            SC3080201.serverProcessing = true;
            SC3080201.showLoding();
        },


        /**
        * コールバック終了
        */
        endServerCallback: function () {
            SC3080201.serverProcessing = false;
            SC3080201.closeLoding();
        },

        /******************************************************************************
        読み込み中表示
        ******************************************************************************/

        /**
        * 読み込み中アイコン表示
        */
        showLoding: function () {

            //2013/12/12 TCS 市川 Aカード情報相互連携開発 START
            if (!this.asyncAnimationEnable) return;
            //2013/12/12 TCS 市川 Aカード情報相互連携開発 END

            //オーバーレイ表示
            $("#registOverlayBlack").css("display", "block");
            //アニメーション
            $("#processingServer").addClass("show");
            $("#registOverlayBlack").addClass("open");
        },

        /**
        * 読み込み中アイコンを非表示にする
        */
        closeLoding: function () {
            $("#processingServer").removeClass("show");
            //2014/11/04 TCS 藤井 iOS8 対応(i-CROP_V4_salesよりマージ)  Start
            $("#registOverlayBlack").removeClass("open");
            setTimeout(function () {
                $("#registOverlayBlack").css("display", "none");

                //ステータス管理ポップアップを表示
                if ($("#UseAutoOpening").val() == "1") {
                    $("#MstPG_IcropIcon").trigger("showPopover");
                    this_form.UseAutoOpening.value = "0";
                }
            }, 300);
            //2014/11/04 TCS 藤井 iOS8 対応(i-CROP_V4_salesよりマージ)  End
        },

        /******************************************************************************
        戻る・ログアウト・メニューボタンの制御
        ******************************************************************************/

        /**
        * 新規活動破棄チェック
        */
        cancellationCheck: function () {

            if (SC3080201.newActivityFlg === true) {
                //新規活動中
                return confirm(SC3080201.redirectMessage);
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
                    //SC3080201.setDebugMessage(target.id);
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

            var targetWrapId = SC3080201.getPageClassToWrapDivId(activePage);

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
            var targetWrapId = SC3080201.getPageClassToWrapDivId(activePage);

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
    SC3080201.init();

    //2012/04/24 TCS 安田 【SALES_1A】初期入力時に、画面が少し動くバグ修正（ユーザー課題 No.71）START
    //ダミーコントロールにフォーカスをセットする
    $("#dummyInitButton").focus();
    //2012/04/24 TCS 安田 【SALES_1A】初期入力時に、画面が少し動くバグ修正（ユーザー課題 No.71）END
});

//$01 Add Start
/**
* 商談・営業キャンセル時のチェック処理
*/
function cancelCheck(kind) {
    if (kind == 1) {
        if (window.confirm(this_form.ErrWord4.value)) {
            startServerCallback()
            return true;
        }
        $("#bodyFrame").trigger("click.popover");
        return false;
    }
    else {
        if (window.confirm(this_form.ErrWord5.value)) {
            startServerCallback()
            return true;
        }
        $("#bodyFrame").trigger("click.popover");
        return false;
    }
}

/**
* ステータス変更時のロード画面表示処理
*/
function startServerCallback() {
    //オーバーレイ表示
    $("#registOverlayBlack").css("display", "block");

    //アニメーション
    setTimeout(function () {
        $("#processingServer").addClass("show");
        $("#registOverlayBlack").addClass("open");
    }, 0);

    //2012/05/17 TCS 安田 クルクル対応 START
    //タイマーセット
    commonRefreshTimer(
       function() {
         //二度押し防止フラグをキャンセル(false)にする
         SC3080201.serverProcessing = false;
         //再表示ボタンクリック
         $("#refreshButton").click();
    });
    //2012/05/17 TCS 安田 クルクル対応 END

    $("#bodyFrame").trigger("click.popover");

    return true;
}

/**
* ポップアップのデザイン調整
*/
$(function () {
    //$("#MstPG_IcropIcon").click(function () {
    //    $("#MstPG_PopOver1_content").parents(".popover").css("border", "#1c232f 1px solid");
    //    $("#MstPG_PopOver1_content").parents(".popover").css("background", "-webkit-gradient(linear, left top, left bottom, from(rgba(201,203,208,0.9)), color-stop(0.002, rgba(120,128,147,0.9)),color-stop(0.015, rgba(87,95,114,0.9)),color-stop(0.036, rgba(29,40,59,0.9)),color-stop(0.0365, rgba(3,11,26,0.9)),to(rgba(7,11,29,0.9)))");
    //    $("#MstPG_PopOver1_content").parents(".popover").find(".content").css("padding", "10px");
    //    $("#MstPG_PopOver1_content").parents(".popover").find(".content").css("margin", "5px 5px 5px 5px");
    //    $("#MstPG_PopOver1_content").parents(".popover").find(".content").css("background", "-webkit-gradient(linear, left top, left bottom, from(#a2a2a2),color-stop(0.018, #fff),color-stop(0.6, #fff),to(#e6e6e6))");
    //    $("#MstPG_PopOver1_content").parents(".popover").find(".content").css("border", "#a5a5a5 1px solid");
    //});

    //活動日時選択ポップアップ
    $("#ActTimePopupTrigger").click(function () {
        $("#ActTimePopOver_content").parents(".popover").css("border", "0px solid black");
        $("#ActTimePopOver_content").parents(".popover").css("background", "Transparent");
        $("#ActTimePopOver_content").parents(".popover").find(".content").css("padding", "0px");
        $("#ActTimePopOver_content").parents(".popover").find(".content").css("margin", "0px");
        $("#ActTimePopOver_content").parents(".popover").find(".content").css("background", "Transparent");
        $("#ActTimePopOver_content").parents(".popover").find(".content").css("border", "none");
    });

    //担当SC選択ポップアップ
    $("#UsersTrigger").click(function () {
        $("#scNscStaffWindown").parents(".popover").css("border", "0px solid black");
        $("#scNscStaffWindown").parents(".popover").css("background", "Transparent");
        $("#scNscStaffWindown").parents(".popover").find(".content").css("padding", "0px");
        $("#scNscStaffWindown").parents(".popover").find(".content").css("margin", "0px");
        $("#scNscStaffWindown").parents(".popover").find(".content").css("background", "Transparent");
        $("#scNscStaffWindown").parents(".popover").find(".content").css("border", "none");
    });

    //活動分類選択ポップアップ
    $("#ActContactTrigger").click(function () {
        $("#scNscActContactWindown").parents(".popover").css("border", "0px solid black");
        $("#scNscActContactWindown").parents(".popover").css("background", "Transparent");
        $("#scNscActContactWindown").parents(".popover").find(".content").css("padding", "0px");
        $("#scNscActContactWindown").parents(".popover").find(".content").css("margin", "0px");
        $("#scNscActContactWindown").parents(".popover").find(".content").css("background", "Transparent");
        $("#scNscActContactWindown").parents(".popover").find(".content").css("border", "none");
    });

    //カタログ選択ポップアップ
    $("#popupTrigger4").click(function () {
        $("#scNscCatalogWindown").parents(".popover").css("border", "0px solid black");
        $("#scNscCatalogWindown").parents(".popover").css("background", "Transparent");
        $("#scNscCatalogWindown").parents(".popover").find(".content").css("padding", "0px");
        $("#scNscCatalogWindown").parents(".popover").find(".content").css("margin", "0px");
        $("#scNscCatalogWindown").parents(".popover").find(".content").css("background", "Transparent");
        $("#scNscCatalogWindown").parents(".popover").find(".content").css("border", "none");
    });

    //試乗選択ポップアップ
    $("#popupTrigger5").click(function () {
        $("#scNscTestDriveWindown").parents(".popover").css("border", "0px solid black");
        $("#scNscTestDriveWindown").parents(".popover").css("background", "Transparent");
        $("#scNscTestDriveWindown").parents(".popover").find(".content").css("padding", "0px");
        $("#scNscTestDriveWindown").parents(".popover").find(".content").css("margin", "0px");
        $("#scNscTestDriveWindown").parents(".popover").find(".content").css("background", "Transparent");
        $("#scNscTestDriveWindown").parents(".popover").find(".content").css("border", "none");
    });

    //見積り選択ポップアップ
    $("#popupTrigger6").click(function () {
        $("#scNscValuationWindown").parents(".popover").css("border", "0px solid black");
        $("#scNscValuationWindown").parents(".popover").css("background", "Transparent");
        $("#scNscValuationWindown").parents(".popover").find(".content").css("padding", "0px");
        $("#scNscValuationWindown").parents(".popover").find(".content").css("margin", "0px");
        $("#scNscValuationWindown").parents(".popover").find(".content").css("background", "Transparent");
        $("#scNscValuationWindown").parents(".popover").find(".content").css("border", "none");
    });

    //次回活動分類選択ポップアップ
    $("#NextActContactTrigger").click(function () {
        $("#scNscNextActContactWindown").parents(".popover").css("border", "0px solid black");
        $("#scNscNextActContactWindown").parents(".popover").css("background", "Transparent");
        $("#scNscNextActContactWindown").parents(".popover").find(".content").css("padding", "0px");
        $("#scNscNextActContactWindown").parents(".popover").find(".content").css("margin", "0px");
        $("#scNscNextActContactWindown").parents(".popover").find(".content").css("background", "Transparent");
        $("#scNscNextActContactWindown").parents(".popover").find(".content").css("border", "none");
    });

    //予約フォロー分類選択ポップアップ
    $("#FollowContactTrigger").click(function () {
        $("#scNscFollowContactWindown").parents(".popover").css("border", "0px solid black");
        $("#scNscFollowContactWindown").parents(".popover").css("background", "Transparent");
        $("#scNscFollowContactWindown").parents(".popover").find(".content").css("padding", "0px");
        $("#scNscFollowContactWindown").parents(".popover").find(".content").css("margin", "0px");
        $("#scNscFollowContactWindown").parents(".popover").find(".content").css("background", "Transparent");
        $("#scNscFollowContactWindown").parents(".popover").find(".content").css("border", "none");
    });

});
//$01 Add End
//2012/04/26 TCS 河原 HTMLエンコード対応
function HtmlEncode(value) {
    return $("<Div>").text(value).html();
}
function HtmlDecode(value) {
    return $("<Div>").html(value).text();
}
//2012/04/26 TCS 河原 HTMLエンコード対応

/**
* その他初期化処理
*/
$(function () {
    ////更新： 2013/03/06 TCS 河原 GL0874 START
    if ($("#SC3080201ContractCancelFlg").val() == "1" || $("#SC3080202ContractCancelFlg").val() == "1") {
        $("#scNscOneBoxContentsArea2").css("display", "none");
    }
    ////更新： 2013/03/06 TCS 河原 GL0874 END

    //2014/07/10 TCS 外崎 TMT BTS-UAT-74 START
    $("#registOverlayBlack").bind("mousedown touchstart", function (e) { e.stopPropagation(); });
    //2014/07/10 TCS 外崎 TMT BTS-UAT-74 END
});
