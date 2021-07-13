
/** 
* @fileOverview SC3240601.Main.js
* 
* @author TMEJ 陳
* @version 1.0.0
* 作成： 2014/07/09 TMEJ 陳 タブレット版SMB（テレマ走行距離機能開発）
* 更新： 
*/


// 定数

/* クリックイベント */
var C_CLICK_EVENT = "click";
// タッチ開始
var C_TOUCH_START = "touchstart mousedown";

/* SA権限コード */
var C_OPARATION_SA = "9";

// 変数
var gTapEvent = "Init";

/**
* DOMロード直後の処理(重要事項).
* @return {void}
*/
$(function () {

    //リストクルクル表示
    SetListLoadingStart();

    //読み込み中のレコード削除、タイムアウトエラーの再描画時に出てくるのでここで消しておく

    //タイマー設定
    commonRefreshTimer(function () { __doPostBack("", ""); });

    //グラフ初期化
    DrawCharts();

    //情報取得
    $("#MainAreaReload").click();

    // UpdatePanel処理前後イベント
    $(document).ready(function () {
        var prm = Sys.WebForms.PageRequestManager.getInstance();

        // 開始時のイベント
        prm.add_beginRequest(function () {
        });
        // 終了時のイベント
        prm.add_endRequest(EndRequest);

        function EndRequest(sender, args) {
            //タイマー初期化
            commonClearTimer();

            if (gTapEvent == "BtnBox01" || gTapEvent == "BtnBox03") {
                //グラフクルクル非表示
                SetGraphLoadingEnd();

                //グラフイベント再設定
                SetGraphZoomBtn();
                SetMileageGraphPreYearBtnEvent();
                SetMileageGraphNextYearBtnEvent();

                //次件のイベント設定.
                SetNextPageBtnEvent();

                //前件のイベント設定.
                SetBackPageBtnEvent();

            }
            else if (gTapEvent == "BackPage" || gTapEvent == "NextPage") {
                //一覧イベント設定
                SetMileageListEvent();

                //一覧スクロール位置を設定
                $('.ListScrollBox .scroll-inner').css(
                                'transform', 'translate3d(0px, -' + $('#HiddenScrollPosition').val() + 'px, 0px)'
                            );

                //操作有効
                OperationRun();

                //グラフイベント再設定
                SetGraphZoomBtn();
                SetMileageGraphPreYearBtnEvent();
                SetMileageGraphNextYearBtnEvent();
            }
            else {

                //初期表示用のイベント設定
                if (gTapEvent == "Init") {
                    SetInitEvent();

                    //フリック
                    touch_flick();
                }

                //一覧イベント設定
                SetMileageListEvent();

                //グラフイベントPreYear設定
                SetMileageGraphPreYearBtnEvent();

                //グラフイベントNextYear設定
                SetMileageGraphNextYearBtnEvent();

                //ZOOMボタンイベン設定
                SetGraphZoomBtn();

                SetPopUpEvent();

                //スクロール設定
                $(".ListScrollBox").fingerScroll();

                //三点文字の設定
                $(".Ellipsis").CustomLabel({ useEllipsis: true });

                //リストクルクル非表示
                SetListLoadingEnd();
            }
        }
    });

});

/**
* 初期表示イベント設定.
* @return {}
*/
function SetInitEvent() {

    //フッター「顧客詳細ボタン」クリック時の動作
    $('#MstPG_FootItem_Main_700').bind(C_CLICK_EVENT, function (event) {

        $('#MstPG_CustomerSearchTextBox').focus();

        event.stopPropagation();
    });
    //スケジューラーと電話帳のアプリケーション設定
    SetFooterApplication();
}
/**
* 全ての操作を有効設定.
* @return {}
*/
function OperationRun() {
    $("#OrderListOverlayBlack").css("display", "none");
    $(".ListScrollBox").fingerScroll({ action: "restart" });
}

/**
* PopUpを閉じる
* @return {}
*/
function ClosePopUp() {
    setTimeout(function (event) {
        //ポップアップ画面を閉じる処理
        $('#OrderListOverlayBlack').click();
    }, 300);

    return false;
}

/**
* 走行距離履歴グラフイベント設定.
* PreYearイベント設定.
* @return {}
*/
function SetMileageGraphPreYearBtnEvent() {

    $('.BtnBox01').bind(C_TOUCH_START, function (event) {

        if ($("#HiddenGraphPreButtonEnable").val() == "false") { return; };

        //イベント中は処理しない
        if (event.preventDefault() == false) { return; };

        //イベント実施判定関数
        event.preventDefault();

        //クルクル表示
        SetGraphLoadingStart();

        setTimeout(function (event) {

            //タイマー設定
            commonRefreshTimer(function () { __doPostBack("", ""); });

            //イベント変数設定
            gTapEvent = "BtnBox01";

            //イベント実行
            $("#GraphPreYearButton").click();
        }, 300);

        event.stopPropagation();

        //イベントを無効
        $('.BtnBox01, .BtnBox03, .Btn01, .Btn02, .Btn03').unbind(C_TOUCH_START);
        $('#BackPage, #NextPage').unbind('touchend mouseup');
    });
}

/**
* 走行距離履歴グラフイベント設定.
* NextYearイベント設定.
* @return {}
*/
function SetMileageGraphNextYearBtnEvent() {

    $('.BtnBox03').bind(C_TOUCH_START, function (event) {

        if ($("#HiddenGraphNextButtonEnable").val() == "false") { return; };

        //イベント中は処理しない
        if (event.preventDefault() == false) { return; };

        //イベント実施判定関数
        event.preventDefault();

        //クルクル表示
        SetGraphLoadingStart();

        setTimeout(function (event) {

            //タイマー設定
            commonRefreshTimer(function () { __doPostBack("", ""); });

            //イベント変数設定
            gTapEvent = "BtnBox03";

            //イベント実行
            $("#GraphNextYearButton").click();
        }, 300);

        event.stopPropagation();

        //イベントを無効
        $('.BtnBox01, .BtnBox03, .Btn01, .Btn02, .Btn03').unbind(C_TOUCH_START);
        $('#BackPage, #NextPage').unbind('touchend mouseup');
    });
}

/**
* 前件のイベント設定.
* @return {}
*/
function SetBackPageBtnEvent() {

    //取得件数が1件以上あれば設定する
    if (0 < $('#HiddenSearchListCount').val()) {

        //前件のイベント設定
        $('#BackPage').bind('touchend mouseup', function (event) {

            //読み込み中を表示
            $("#BackPage").css("display", "none");
            $("#BackPageLoad").css("display", "block");

            setTimeout(function (event) {

                //タイマー設定
                commonRefreshTimer(function () { __doPostBack("", ""); });

                //イベント変数設定
                gTapEvent = "BackPage";

                //表示
                $("#BackPageButton").click();
            }, 300);

            //イベント中は全ての操作無効
            $("#OrderListOverlayBlack").css("display", "block");
            $(".ListScrollBox").fingerScroll({ action: "stop" });

            event.stopPropagation();

            //イベントを無効
            $('.BtnBox01, .BtnBox03, .Btn01, .Btn02, .Btn03').unbind(C_TOUCH_START);
        });
    }
}

/**
* 次件のイベント設定.
* @return {}
*/
function SetNextPageBtnEvent() {

    //取得件数が1件以上あれば設定する
    if (0 < $('#HiddenSearchListCount').val()) {

        //次件のイベント設定
        $('#NextPage').bind('touchend mouseup', function (event) {

            //読み込み中を表示
            $("#NextPage").css("display", "none");
            $("#NextPageLoad").css("display", "block");

            setTimeout(function (event) {

                //タイマー設定
                commonRefreshTimer(function () { __doPostBack("", ""); });

                //イベント変数設定
                gTapEvent = "NextPage";

                //表示
                $("#NextPageButton").click();
            }, 300);

            //イベント中は全ての操作無効
            $("#OrderListOverlayBlack").css("display", "block");
            $(".ListScrollBox").fingerScroll({ action: "stop" });

            event.stopPropagation();

            //イベントを無効
            $('.BtnBox01, .BtnBox03, .Btn01, .Btn02, .Btn03').unbind(C_TOUCH_START);
        });
    }
}

/**
* 走行距離履歴一覧イベント設定.
* @return {}
*/
function SetMileageListEvent() {

    //取得件数が1件以上あれば設定する
    if (0 < $('#HiddenSearchListCount').val()) {

        //Detailエリアのイベント設定
        $('.DetailButtonAreaClass').bind('touchend mouseup', function (event) {

            //イベント中は処理しない
            if (event.preventDefault() == false) { return; };

            //隠しパラメータからデータを取得
            var selecttedDetailFlg = $(this).attr("SelectDetailFlg");
            var selecttedDetailDate = $(this).attr("SelectDetailDate");
            var selecttedDetailCode = $(this).attr("SelectDetailCode");
            var selecttedDetailMileage = $(this).attr("SelectDetailMileage");
            var selecttedDetailName = $(this).attr("SelectDetailName");
            var selecttedDetailIndicator = $(this).attr("SelectDetailIndicator");
            var description = $(this).attr("value");

            //Warning発生日時を設定
            $(".Date_Value_Detail").html(selecttedDetailDate);
            //Warningエラーコードを設定
            $(".Code_Value_Detail").html(selecttedDetailCode);
            //Warning走行距離を設定
            $(".Mileage_Value_Detail").html(selecttedDetailMileage);
            //Warning詳細を設定
            $(".Name_Value_Detail").html(selecttedDetailName);

            //インジケータの値チェック
            if (selecttedDetailIndicator == "-1") {
                //存在しない場合
                //インジケータ非表示を設定
                SetIndicatorDisplay(0)
                document.getElementById("IndicatorImage").src = ""

            } else {
                //存在する場合
                //インジケータ表示を設定
                SetIndicatorDisplay(1)
                document.getElementById("IndicatorImage").src = selecttedDetailIndicator;

            }

            //Warning説明の設定
            $(".Description_Value_Detail").html(description);

            //イベント有効
            DetailCloseEvent();

            event.stopPropagation();

            setTimeout(function () {
                $(".PopUpOrderListClass").attr("style", "");

                $("#OrderListOverlayBlack").css("display", "block");

                //初期表示データが5行以上ある場合、設定値はscrollHeight
                if (100 < $(".Description_Value_Detail").attr("scrollHeight")) {
                    settingHeight = $(".Description_Value_Detail").attr("scrollHeight");

                } else {
                    //5行未満はデフォルト値
                    settingHeight = 100;

                }

                //高さを設定
                $(".Description_Value_Detail").height(settingHeight);
                $(".ListBox03").height(settingHeight + 9);

                //スクロール設定
                $(".InnerDatas").fingerScroll();
                $(".ListScrollBox").fingerScroll({ action: "stop" });

            }, 0);
        });

        //次件のイベント設定.
        SetNextPageBtnEvent();

        //前件のイベント設定.
        SetBackPageBtnEvent();
    }
}


/**
* ウォーニング詳細非表示イベント設定.
* @return {}
*/
function DetailCloseEvent() {

    //ウォーニング詳細画面クローズイベント
    $('#OrderListOverlayBlack, #header, #foot').bind(C_TOUCH_START, function (event) {
        //イベント中は処理しない
        if (event.preventDefault() == false) { return; };

        //イベント中判断関数
        event.preventDefault();

        $(".PopUpOrderListClass").attr("style", "display:none");
        $("#OrderListOverlayBlack").css("display", "none");
        $(".ListScrollBox").fingerScroll({ action: "restart" });

        //イベントを無効
        $('#OrderListOverlayBlack, #header, #foot').unbind(C_TOUCH_START);
    });
}

/**
* グラフZOOMボタンイベント設定.
* @return {}
*/
function SetGraphZoomBtn() {

    //日数スケール
    $('.Btn01').bind(C_TOUCH_START, function (event) {

        //イベント中は処理しない
        if (event.preventDefault() == false) { return; };

        //イベント実施判定関数
        event.preventDefault();

        changeZoomChart(1);
    });
    //週数スケール
    $('.Btn02').bind(C_TOUCH_START, function (event) {

        //イベント中は処理しない
        if (event.preventDefault() == false) { return; };

        //イベント実施判定関数
        event.preventDefault();

        changeZoomChart(2);
    });
    //月数スケール
    $('.Btn03').bind(C_TOUCH_START, function (event) {

        //イベント中は処理しない
        if (event.preventDefault() == false) { return; };

        //イベント実施判定関数
        event.preventDefault();

        changeZoomChart(3);
    });
}

/**
* SetIndicatorImageDivDisplay.
* @return {}
*/
function SetIndicatorDisplay(Param) {
    if (Param == 1) {
        $(".ListBox02").attr("style", "display:block;");
        //$(".ListBox03").attr("style", "height:auto;min-height:98px;");
        //$(".ListBox03 table").attr("style", "height:100%;;min-height:98px;");
    }
    else {
        $(".ListBox02").attr("style", "display:none;");
        //$(".ListBox03").attr("style", "height:auto;min-height:326px;");
        //$(".ListBox03 table").attr("style", "height:100%;;min-height:326px;");
    }
}

/**
* SetGraphPreYearButtonEnable.
* @return {}
*/
function SetGraphPreYearButtonEnable(Param) {
    if (Param == 1) {
        $(".BtnBox01").attr("class", "BtnBox01 BtnOFF");
    }
    else {
        $(".BtnBox01").attr("class", "BtnBox01 BtnOn");
    }
}

/**
* SetNextPageLinkEnable.
* @return {}
*/
function SetNextPageLinkEnable(Param) {
    if (Param == 1) {
        $(".NextPageClass").attr("style", "display: block; text-align: center;line-height:46px;font-size: 14px;");
    }
    else {
        $(".NextPageClass").attr("style", "display: none; text-align: center;line-height:46px;font-size: 14px;");
    }
}

/**
* SetPrePageLinkEnable.
* @return {}
*/
function SetPrePageLinkEnable(Param) {
    if (Param == 1) {
        $(".BackPageClass").attr("style", "display: block; text-align: center;line-height:46px;font-size: 14px;");
    }
    else {
        $(".BackPageClass").attr("style", "display: none; text-align: center;line-height:46px;font-size: 14px;");
    }
}

/**
* SetGraphNextYearButtonEnable.
* @return {}
*/
function SetGraphNextYearButtonEnable(Param) {
    if (Param == 1) {
        $(".BtnBox03").attr("class", "BtnBox03 BtnOFF");
    }
    else {
        $(".BtnBox03").attr("class", "BtnBox03 BtnOn");
    }
}

/**
* PopUpイベント設定.
* @return {}
*/
function SetPopUpEvent() {

    $('.LeftBtn').bind(C_TOUCH_START, function (event) {

        $(".PopUpOrderListClass").attr("style", "display:none");
        $("#OrderListOverlayBlack").css("display", "none");

        $(".ListScrollBox").fingerScroll({ action: "restart" });

        event.stopPropagation();
    });

}

/**
* スケジュールボタンと電話帳ボタンの設定する.
* @return {}
*/
function SetFooterApplication() {
    //スケジュールオブジェクトを拡張
    $.extend(window, { schedule: {} });
    //スケジュールオブジェクトに関数追加
    $.extend(schedule, {
        //アプリ起動クラス
        appExecute: {
            //カレンダーアプリ起動(単体)
            executeCaleNew: function () {
                window.location = "icrop:cale:";
                return false;
            },
            //電話帳アプリ起動(単体)
            executeCont: function () {
                window.location = "icrop:cont:";
                return false;
            }
        }
    });
}

/**
* リスト読み込み中画面を表示する.
* @return {}
*/
function SetListLoadingStart() {
    $("#ServerProcessListOverlay").css("display", "block");
    $("#ServerProcessListIcon").css("display", "block");
}

/**
* リスト読み込み中画面を非表示にする.
* @return {}
*/
function SetListLoadingEnd() {
    $("#ServerProcessListOverlay").css("display", "none");
    $("#ServerProcessListIcon").css("display", "none");
}


/**
* グラフ読み込み中画面を表示する.
* @return {}
*/
function SetGraphLoadingStart() {
    $("#ServerProcessGraphBtnsOverlay").css("display", "block");
    $("#ServerProcessGraphOverlay").css("display", "block");
    $("#ServerProcessGraphIcon").css("display", "block");
}

/**
* グラフ読み込み中画面を非表示にする.
* @return {}
*/
function SetGraphLoadingEnd() {
    $("#ServerProcessGraphBtnsOverlay").css("display", "none");
    $("#ServerProcessGraphOverlay").css("display", "none");
    $("#ServerProcessGraphIcon").css("display", "none");
}

// フッターボタンの2度押し制御
function FooterButtonControl() {
    $.master.OpenLoadingScreen();
    return true;
}

/**
* フッターボタンのクリックイベント.
* @return {}
*/
function FooterButtonClick(Id) {
    //顧客詳細ボタンの場合は何もしない
    if (Id == 700 || Id == 200) {
        return false;
    }
}

/**
* フリックイベント
* @return {}
*/
function touch_flick() {

    var slideY;
    var savetouch;
    var animeflg = 2;
    var savescroll = $(".scroll-inner").height();
    var finger3d;
    var fingeranime;
    var moverange = 0;
    var fingerheight = 0;

    /* ドラッグ開始 */
    $('.Knob').mousedown(function (e) {

        //アニメーション中判断
        if ($('.dummybottombar').hasClass('animeflg')) { return; };
        //イベント中は処理しない
        if (e.preventDefault() == false) { return; };
        //イベント実施判定関数
        e.preventDefault();
        e.stopImmediatePropagation();

        //フィンガースクロール制御
        $('.ListScrollBox').fingerScroll({ action: "stop" });
        $('.dummyscroll').css({ "display": "block" });

        //イベント座標及び、要素位置取得
        this.touchY = event.pageY;
        slideY = parseInt($('.DisplayList').css("top").replace('px', ''));

        //フィンガースクロール高さ計算
        fingerheight = 0 - ($('.scroll-inner').height() - 163 - 290);

        /* ドラッグ中 */
        $('.Knob').mousemove(function (e) {

            //最大移動値計算
            if ($('.ListScrollBox').height() <= 163 && this.touchY <= event.pageY) { return; };
            if ($('.ListScrollBox').height() >= 452 && this.touchY >= event.pageY) { return; };
            //イベント中は処理しない
            if (e.preventDefault() == false) { return; };
            //イベント実施判定関数
            e.preventDefault();
            e.stopImmediatePropagation();

            //要素高さ取得
            var divheight = $('.DisplayList').height() + (this.touchY - event.pageY);
            var listheight = $('.ListScrollBox').height() + (this.touchY - event.pageY);
            var scrollheight = $('.scroll-inner').height() + (this.touchY - event.pageY);

            //フィンガースクロールアニメーション位置取得
            finger3d = parseInt(($('.scroll-inner').css("transform").split(','))[5].replace(')', ''));
            fingeranime = finger3d + (this.touchY - event.pageY);

            //移動距離把握
            moverange = moverange + (this.touchY - event.pageY);

            //表示位置計算
            slideY = slideY - (this.touchY - event.pageY);

            //バウンド計算
            if (listheight < 163) {
                if ($('.DisplayList').css("transform") != "none") {
                    var webitem = $('.DisplayList').css("transform").split(',');
                    slideY = 0 - parseInt(webitem[5].replace(')', ''));
                } else {
                    slideY = 0
                }
                divheight = 225;
                listheight = 163;
            } else if (listheight > 452) {
                if ($('.DisplayList').css("transform") != "none") {
                    var webitem = $('.DisplayList').css("transform").split(',');
                    slideY = -290 - parseInt(webitem[5].replace(')', ''));
                } else {
                    slideY = -290
                }
                divheight = 514;
                listheight = 452;

                //フィンガースクロール誤差計算
                fingeranime = fingeranime - (moverange - 290);
                if (fingerheight > fingeranime) {
                    fingeranime = fingerheight;
                }
            }

            //要素移動
            var firstflg = $('.ListScrollBox').height();
            $('.dummyscroll').height(listheight);
            $('.ListScrollBox').height(listheight);
            $('.DisplayList').height(divheight).css({ top: slideY });
            if (savescroll > 452) {
                $(".scroll-inner").height(scrollheight);
            }

            //アニメーション判断
            if (savetouch >= event.pageY || firstflg == 163) {
                animeflg = 0;
                //フィンガースクロール移動
                if ($(".scroll-inner").height() > 452) {
                    if (fingeranime <= fingerheight || parseInt(($('.scroll-inner').css("transform").split(','))[5].replace(')', '')) <= fingerheight) {
                        $('.scroll-inner').css({
                            "transform": "translate3d(0px, " + fingeranime + "px, 0px)",
                            "-webkit-transition": "transform " + "0" + "ms"
                        }).one("webkitTransitionEnd", function () { });
                    }
                }
            } else if (savetouch < event.pageY || firstflg == 452) {
                animeflg = 1;
            } else {
                animeflg = 2;
            }

            //イベント位置を退避
            savetouch = this.touchY;
            this.touchY = event.pageY;
        });
    });

    /* ドラッグ終了 */
    $('.Knob').mouseup(function (e) {

        //アニメーション中判断
        if ($('.dummybottombar').hasClass('animeflg')) { return; };
        //イベント中は処理しない
        if (e.preventDefault() == false) { return; };
        //イベント実施判定関数
        e.preventDefault();
        e.stopImmediatePropagation();

        //フィンガースクロール制御
        $('.ListScrollBox').fingerScroll({ action: "restart" });
        $('.ListScrollBox').fingerScroll({ action: "stop" });

        //イベントトップ取得
        var animemove = parseInt($('.DisplayList').css("top").replace('px', ''));
        var CurrentY;

        //アニメーション設定
        //最後まで要素を移動
        if (animeflg == 0 && $('.dummybottombar').hasClass('animeflg') == false) {
            //移動距離リセット
            moverange = 290;

            //アニメーション
            $(".dummybottombar").addClass("animeflg");
            CurrentY = -290 - animemove;
            $(".dummyscroll").height(452);
            $(".ListScrollBox").height(452);
            if (savescroll > 452) {
                $(".scroll-inner").height(453);
            }
            $('.DisplayList').css({
                "transform": "translate3d(0px, " + CurrentY + "px, 0px)",
                "-webkit-transition": "transform " + "200" + "ms"
            }).css({ "height": "514"
            }).one("webkitTransitionEnd", function () { });
            //フィンガースクロールアニメ
            if ($(".scroll-inner").height() > 452) {
                if (parseInt(($('.scroll-inner').css("transform").split(','))[5].replace(')', '')) < fingerheight) {
                    $('.scroll-inner').css({
                        "transform": "translate3d(0px, " + fingerheight + "px, 0px)",
                        "-webkit-transition": "transform " + "200" + "ms"
                    }).one("webkitTransitionEnd", function () { });
                }
            } else {
                $('.scroll-inner').css({
                    "transform": "translate3d(0px, " + 0 + "px, 0px)",
                    "-webkit-transition": "transform " + "200" + "ms"
                }).one("webkitTransitionEnd", function () { });
            }
        } else if (animeflg == 1 && $('.dummybottombar').hasClass('animeflg') == false) {
            //移動距離リセット
            moverange = 0;

            //アニメーション
            $(".dummybottombar").addClass("animeflg");
            CurrentY = -animemove;
            $('.DisplayList').css({
                "transform": "translate3d(0px, " + CurrentY + "px, 0px)",
                "-webkit-transition": "transform " + "200" + "ms"
            }).css({ "height": "225"
            }).one("webkitTransitionEnd", function () {
                if ($(".ListScrollBox").height() != 452) {
                    if ($('.dummybottombar').hasClass('animeflg')) {
                        $(".dummyscroll").height(163);
                        $(".ListScrollBox").height(163);
                    };
                    if (savescroll > 452) {
                        $(".scroll-inner").css({ "height": "auto" });
                    }
                }
            });
        }
        if (animeflg == 2) {
            //フィンガースクロール制御
            $('.ListScrollBox').fingerScroll({ action: "restart" });
            $('.dummyscroll').css({ "display": "none" });
        } else {
            setTimeout(function () {
                //フィンガースクロール制御
                $('.ListScrollBox').fingerScroll({ action: "restart" });
                $('.dummyscroll').css({ "display": "none" });
                $(".dummybottombar").removeClass("animeflg");
            }, 300);
        }
        animeflg = 2;
        //イベント削除
        $('.Knob').unbind("mousemove");
    });

    //TABLET
    $('.Knob').bind({
        /* フリック開始時 */
        'touchstart': function (e) {

            //アニメーション中判断
            if ($('.dummybottombar').hasClass('animeflg')) { return; };
            //イベント中は処理しない
            if (e.preventDefault() == false) { return; };
            //イベント実施判定関数
            e.preventDefault();
            e.stopImmediatePropagation();

            //フィンガースクロール制御
            $('.ListScrollBox').fingerScroll({ action: "stop" });
            $('.dummyscroll').css({ "display": "block" });

            //イベント座標及び、要素位置取得
            this.touchY = event.changedTouches[0].pageY;
            slideY = parseInt($('.DisplayList').css("top").replace('px', ''));

            //フィンガースクロール高さ計算
            fingerheight = 0 - ($('.scroll-inner').height() - 163 - 290);
        },
        /* フリック中 */
        'touchmove': function (e) {

            //アニメーション中判断
            if ($('.dummybottombar').hasClass('animeflg')) { return; };
            //イベント中は処理しない
            if (e.preventDefault() == false) { return; };
            //イベント実施判定関数
            e.preventDefault();
            e.stopImmediatePropagation();

            //フィンガースクロール制御
            $('.ListScrollBox').fingerScroll({ action: "stop" });
            $('.dummyscroll').css({ "display": "block" });

            //最大移動値計算
            if ($('.ListScrollBox').height() <= 163 && this.touchY <= event.changedTouches[0].pageY) { return; };
            if ($('.ListScrollBox').height() >= 452 && this.touchY >= event.changedTouches[0].pageY) { return; };

            //要素高さ取得
            var divheight = $('.DisplayList').height() + (this.touchY - event.changedTouches[0].pageY);
            var listheight = $('.ListScrollBox').height() + (this.touchY - event.changedTouches[0].pageY);
            var scrollheight = $('.scroll-inner').height() + (this.touchY - event.changedTouches[0].pageY);

            //フィンガースクロールアニメーション位置取得
            finger3d = parseInt(($('.scroll-inner').css("transform").split(','))[5].replace(')', ''));
            fingeranime = finger3d + (this.touchY - event.changedTouches[0].pageY);

            //移動距離把握
            moverange = moverange + (this.touchY - event.changedTouches[0].pageY);

            //表示位置計算
            slideY = slideY - (this.touchY - event.changedTouches[0].pageY);

            //バウンド計算
            if (listheight < 163) {
                if ($('.DisplayList').css("transform") != "none") {
                    var webitem = $('.DisplayList').css("transform").split(',');
                    slideY = 0 - parseInt(webitem[5].replace(')', ''));
                } else {
                    slideY = 0
                }
                divheight = 225;
                listheight = 163;
            } else if (listheight > 452) {
                if ($('.DisplayList').css("transform") != "none") {
                    var webitem = $('.DisplayList').css("transform").split(',');
                    slideY = -290 - parseInt(webitem[5].replace(')', ''));
                } else {
                    slideY = -290
                }
                divheight = 514;
                listheight = 452;

                //フィンガースクロール誤差計算
                fingeranime = fingeranime - (moverange - 290);
                if (fingerheight > fingeranime) {
                    fingeranime = fingerheight;
                }
            }

            //要素移動
            var firstflg = $('.ListScrollBox').height();
            $('.dummyscroll').height(listheight);
            $('.ListScrollBox').height(listheight);
            $('.DisplayList').height(divheight).css({ top: slideY });
            if (savescroll > 452) {
                $(".scroll-inner").height(scrollheight);
            }
            //アニメーション判断
            if (savetouch >= event.changedTouches[0].pageY || firstflg == 163) {
                animeflg = 0;
                //フィンガースクロール移動
                if ($(".scroll-inner").height() > 452) {
                    if (fingeranime <= fingerheight || parseInt(($('.scroll-inner').css("transform").split(','))[5].replace(')', '')) <= fingerheight) {
                        $('.scroll-inner').css({
                            "transform": "translate3d(0px, " + fingeranime + "px, 0px)",
                            "-webkit-transition": "transform " + "0" + "ms"
                        }).one("webkitTransitionEnd", function () { });
                    }
                }
            } else if (savetouch < event.changedTouches[0].pageY || firstflg == 452) {
                animeflg = 1;
            } else {
                animeflg = 2;
            }

            //イベント位置を退避
            savetouch = this.touchY;
            this.touchY = event.changedTouches[0].pageY;
        },
        /* フリック終了 */
        'touchend': function (e) {

            //アニメーション中判断
            if ($('.dummybottombar').hasClass('animeflg')) { return; };
            //イベント中は処理しない
            if (e.preventDefault() == false) { return; };
            //イベント実施判定関数
            e.preventDefault();
            e.stopImmediatePropagation();

            //フィンガースクロール制御
            $('.ListScrollBox').fingerScroll({ action: "restart" });
            $('.ListScrollBox').fingerScroll({ action: "stop" });

            //イベントトップ取得
            var animemove = parseInt($('.DisplayList').css("top").replace('px', ''));
            var CurrentY;

            //アニメーション設定
            //最後まで要素を移動
            if (animeflg == 0 && $('.dummybottombar').hasClass('animeflg') == false) {
                //移動距離リセット
                moverange = 290;

                //アニメーション
                $(".dummybottombar").addClass("animeflg");
                CurrentY = -290 - animemove;
                $(".dummyscroll").height(452);
                $(".ListScrollBox").height(452);
                if (savescroll > 452) {
                    $(".scroll-inner").css({ "height": "453" });
                }
                $('.DisplayList').css({
                    "transform": "translate3d(0px, " + CurrentY + "px, 0px)",
                    "-webkit-transition": "transform " + "200" + "ms"
                }).css({ "height": "514"
                }).one("webkitTransitionEnd", function () { });
                //フィンガースクロールアニメ
                if ($(".scroll-inner").height() > 452) {
                    if (parseInt(($('.scroll-inner').css("transform").split(','))[5].replace(')', '')) < fingerheight) {
                        $('.scroll-inner').css({
                            "transform": "translate3d(0px, " + fingerheight + "px, 0px)",
                            "-webkit-transition": "transform " + "200" + "ms"
                        }).one("webkitTransitionEnd", function () { });
                    }
                } else {
                    $('.scroll-inner').css({
                        "transform": "translate3d(0px, " + 0 + "px, 0px)",
                        "-webkit-transition": "transform " + "200" + "ms"
                    }).one("webkitTransitionEnd", function () { });
                }
            } else if (animeflg == 1 && $('.dummybottombar').hasClass('animeflg') == false) {
                //移動距離リセット
                moverange = 0;

                //アニメーション
                $(".dummybottombar").addClass("animeflg");
                CurrentY = -animemove;
                $('.DisplayList').css({
                    "transform": "translate3d(0px, " + CurrentY + "px, 0px)",
                    "-webkit-transition": "transform " + "200" + "ms"
                }).css({ "height": "225"
                }).one("webkitTransitionEnd", function () {
                    if ($(".ListScrollBox").height() != 452) {
                        if ($('.dummybottombar').hasClass('animeflg')) {
                            $(".dummyscroll").height(163);
                            $(".ListScrollBox").height(163);
                        };
                        if (savescroll > 452) {
                            $(".scroll-inner").css({ "height": "auto" });
                        }
                    }
                });
            }
            if (animeflg == 2) {
                //フィンガースクロール制御
                $('.ListScrollBox').fingerScroll({ action: "restart" });
                $('.dummyscroll').css({ "display": "none" });
            } else {
                setTimeout(function () {
                    //フィンガースクロール制御
                    $('.ListScrollBox').fingerScroll({ action: "restart" });
                    $('.dummyscroll').css({ "display": "none" });
                    $(".dummybottombar").removeClass("animeflg");
                }, 300);
            }
            animeflg = 2;
        }
    });
};
