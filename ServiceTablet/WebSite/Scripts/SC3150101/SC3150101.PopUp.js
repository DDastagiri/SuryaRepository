//SC3150101.PopUp.js
//------------------------------------------------------------------------------
//機能：TC画面の中断ポップアップ
//作成：2014/07/25 TMEJ 三輪 【開発】IT9711_タブレットSMB Job Dispatch機能開発
//更新：2016/03/30 NSK  小牟禮 アクティビティインジケータが消えない問題
//------------------------------------------------------------------------------

//2014/07/25 TMEJ 三輪 タブレットSMB Job Dispatch機能開発 START

var C_MAXSTOPTIME = 9995;                   //中断時間の最大時間
var gResizeInterval = 5;                    // リサイズの単位(分)
var C_RADIX = "10";                         //parseIntで使用する基数
var C_NUMPATTERN = "^[0-9]+$";              //半角数字のみの正規表現パターン
var C_CELL_HEIGHT = 73;                     // 1つセルの高さ
var CAll_BY_SC3150101 = "0";
var CAll_BY_SC3150102 = "1";
var CAll_BY_SC3150102_2 = "2";
var STOP_REASON_TYPE_PART = "01";
var STOP_REASON_TYPE_CUSTOMER = "02";
var STOP_REASON_TYPE_OTHER = "99";
var RestJobCount = 0;

function ConfirmStopWindow() {
    
    //入力値を格納
    $("#HiddenStopTime").val($(".StopTimeLabel").html());

    //2017/09/16 NSK 竹中(璃) REQ-SVT-TMT-20161109-001 SMB iPadにストールクローズ機能の追加 START
    //格納先が誤っているため修正
    //$("#HiddenChildStopMemo").val($("#txtStopMemo").val());
    $("#HiddenStopMemo").val($("#txtStopMemo").val());
    //2017/09/16 NSK 竹中(璃) REQ-SVT-TMT-20161109-001 SMB iPadにストールクローズ機能の追加 END

    //子画面の要素に値を格納
    $('#stc01Box03').contents().find("#HiddenChildStopTime").val($(".StopTimeLabel").html());
    $('#stc01Box03').contents().find("#HiddenChildStopMemo").val($("#txtStopMemo").val()); 

    //呼び出し元がTCメインの場合
    if ($("#HiddenJobStopWindowFlg").val() == CAll_BY_SC3150101) {
        //2016/03/30 NSK 小牟禮 アクティビティインジケータが消えない問題 START
        //$("#HiddenButtonJobStop").click();

        //クルクル表示
        LoadingScreen();

        setTimeout(function () {
            $("#HiddenButtonJobStop").click();
        }, 0);
        //2016/03/30 NSK 小牟禮 アクティビティインジケータが消えない問題 END

    } else {
        if (RestJobCount == 1) {
            document.getElementById('stc01Box03').contentWindow.JobStopBattonClick(1);
        } else {
            document.getElementById('stc01Box03').contentWindow.JobStopBattonClick(0);
        }
       
    }

    CancelStopWindow();

    //2016/03/30 NSK 小牟禮 アクティビティインジケータが消えない問題 START
    //LoadingScreen();
    //2016/03/30 NSK 小牟禮 アクティビティインジケータが消えない問題 END
}

//中断ポップアップを閉じる
function CancelStopWindow() {
    $(".popStopWindowBase").css("display", "none");
    $("#BlackWindow").css("display", "none");
}

//中断ポップアップを開く
function ChanselPopUp() {

    $(".popStopWindowBase").css("display", "block");
    $("#BlackWindow").css("display", "block");

    //呼び出し元がTCメインの場合
    if ($("#HiddenJobStopWindowFlg").val() == CAll_BY_SC3150101) {
        $(".TableWorkingHours").css("display", "block");
    } else {
        //全ての作業が中断にならない場合、作業中断時間を表示しない
        if (RestJobCount == 1) {
            $(".TableWorkingHours").css("display", "block");
        } else {
            $(".TableWorkingHours").css("display", "none");
        }
    }
    

    // ディフォルト値の設定
    // 時間の初期化
    $(".popStopWindowBase .StopTimeLabel").html("0" + $("#HiddenStopTimeWord").val());
    // メモエリアとウィンドウスクロールの初期化
    $(".popStopWindowBase #txtStopMemo").val("").css("height", 100);
    $("#StopMemoScrollBox .scroll-inner").height($("#StopMemoScrollBox .innerDataBox").height());
    $("#StopMemoScrollBox .scroll-inner").css({ "transform": "translate3d(0px, 0px, 0px)" });
    // 選択ボックスの初期化
    document.getElementById("dpDetailStopMemo").selectedIndex = 0;
    $("#lblDetailStopMemo").text($("#dpDetailStopMemo")[0].options[0].text);


    // 中断理由のディフォルト値
    SelectStopArea(0);
}

// 中断ポップアップ表示時裏画面を操作できなくするためのページ全体を覆う DIV の作成
function divOpen(evt) {

    CanselPopUp();

}

/**
* 中断理由選択項目にタップする
* @param {Integer} タップの項目順番
* @return {なし}
*/
function SelectStopArea(nIndex) {
    // 色が変更される
    $(".popStopWindowBase .dataBox .innerDataBox .DataListTable li").removeClass("Check");
    if (nIndex == 0) {
        $(".popStopWindowBase .dataBox .innerDataBox .DataListTable li:first").addClass("Check");
        $("#HiddenStopReasonType").val(STOP_REASON_TYPE_PART);
        $('#stc01Box03').contents().find("#HiddenChildStopReasonType").val(STOP_REASON_TYPE_PART);
    } else if (nIndex == 1) {
        $(".popStopWindowBase .dataBox .innerDataBox .DataListTable li:eq(1)").addClass("Check");
        $("#HiddenStopReasonType").val(STOP_REASON_TYPE_CUSTOMER);
        $('#stc01Box03').contents().find("#HiddenChildStopReasonType").val(STOP_REASON_TYPE_CUSTOMER);
    } else {
        $(".popStopWindowBase .dataBox .innerDataBox .DataListTable li:eq(2)").addClass("Check");
        $("#HiddenStopReasonType").val(STOP_REASON_TYPE_OTHER);
        $('#stc01Box03').contents().find("#HiddenChildStopReasonType").val(STOP_REASON_TYPE_OTHER);
    }
}

/**
* 中断時間を変更する
* @param {Integer} 変える時間
* @return {なし}
*/
function ChangeStopMinutes(nChangeMinutes) {
    var nMinutes = parseInt($(".popStopWindowBase .StopTimeLabel").html());
    // 最小値、最大値を超える場合、
    if ((nMinutes + nChangeMinutes < 0)
        || (nMinutes + nChangeMinutes > C_MAXSTOPTIME)) {
        return;
    }
    nMinutes += nChangeMinutes;
    $(".popStopWindowBase .StopTimeLabel").text(nMinutes + $("#HiddenStopTimeWord").val());
}

function SetStopDlgPosition(strChipId) {

    // ウィンドウの左座標の最小値
    var nMinLeft = 156;
    var nMaxLeft = 656;
    // 選択したチップの左座標
    var nChipLeft = $("#" + strChipId).offset().left;
    var nChipWidth = $("#" + strChipId).width();
    var nChipRight = nChipLeft + nChipWidth;

    var nDlgWidth = $(".popStopWindowBase").width();

    var nLeft = -1;

    // 横座標設定
    // 左部分で直接表示できる場合
    if (nChipLeft - nDlgWidth - 17 >= nMinLeft) {
        nLeft = nChipLeft - nDlgWidth - 17;
        $(".popStopWindowBase .gradationBox .ArrowMaskL").css("display", "none");
        $(".popStopWindowBase .gradationBox .ArrowMaskR").css("display", "block");
    }
    // 未設定且つ右部分で表示できる場合
    if ((nChipRight <= nMaxLeft) && (nLeft == -1)) {
        nLeft = nChipRight + 17;
        $(".popStopWindowBase .gradationBox .ArrowMaskL").css("display", "block");
        $(".popStopWindowBase .gradationBox .ArrowMaskR").css("display", "none");
    }

    // 直接表示できない場合、ディフォルト位置で表示する
    if (nLeft == -1) {
        $(".popStopWindowBase .gradationBox .ArrowMaskL").css("display", "none");
        $(".popStopWindowBase .gradationBox .ArrowMaskR").css("display", "block");
        nLeft = 276;
    }

    // 縦標設定
    var nTop = -1;
    // ウィンドウの上と下辺の座標
    var nMinTop = 63;
    var nMaxTop = 648;
    // チップの中心座標
    var nRowNo = GetRowNoByChipId(strChipId);
    var nChipTop = $(".Row" + nRowNo).offset().top - C_CHIPAREA_OFFSET_TOP;
    var nChipBottom = nChipTop + $(".Row" + nRowNo).height();
    var nArrowTop;
    // 中心点が中断理由の縦の座標の外の場合、ウィンドウのtop座標を調整する
    if (nChipTop < 95) {
        nTop = 40;
        // 半分チップしか表示されない、全部表示するようにスクロール
        if (nChipTop < nMinTop) {
            // 最大値にスクロール
            var nScroll = nMinTop - nChipTop;
            // 縦方向でスクロール
            $(".ChipArea_trimming").SmbFingerScroll({
                action: "move", moveY: -nScroll, moveX: 0
            });
            // 矢印の座標を調整する
            nChipTop = $(".Row" + nRowNo).offset().top - C_CHIPAREA_OFFSET_TOP; ;
        }
    } else if (nChipBottom > 600) {
        nTop = 145;
        // 半分チップしか表示されない、全部表示するようにスクロール
        if (nChipBottom > nMaxTop) {
            // 最大値にスクロール
            var nScroll = nChipBottom - nMaxTop;
            // 縦方向でスクロール
            $(".ChipArea_trimming").SmbFingerScroll({
                action: "move", moveY: nScroll, moveX: 0
            });
            // 矢印の座標を調整する
            nChipTop = $(".Row" + nRowNo).offset().top - C_CHIPAREA_OFFSET_TOP; ;
        }
    } else {
        // ディフォルトtop座標
        nTop = 95;
    }
    // 矢印の座標を調整する
    nArrowTop = nChipTop - nTop + 6;
    $(".popStopWindowBase .gradationBox .ArrowMaskL").css("top", nArrowTop);
    $(".popStopWindowBase .gradationBox .ArrowMaskR").css("top", nArrowTop);

    $(".popStopWindowBase").css({ "top": nTop, "left": nLeft });
}

//
function ClickStopTime() {

    var nMinutes = parseInt($(".popStopWindowBase .StopTimeLabel").html());
    $(".popStopWindowBase .StopTimeLabel").css("display", "none");
    $(".popStopWindowBase #StopTimeTxt").css({ "display": "block",
        "font-size": 14,
        "font-weight": "bold",
        "line-height": "22px"
    })
                                        .val(nMinutes)
                                        .focus();
}

/**
* 数値の丸め込み後の数値をテキストに表示する。<br>
* 
* @param {String} $("#StopTimeTxt").val() StopTimeTxtに入力されている丸め込みを行う値
* @param {String} gResizeInterval 丸め込みを行う単位
* @param {String} 0 丸め込み後の最小値
* @param {String} C_MAXSTOPTIME 丸め込み後の最大値
* @return {Integer}　丸め込み後の数値
* 
*/
function BindStopWndEvent() {

    var nMinutes = RoundUpToNumUnits($("#StopTimeTxt").val(), gResizeInterval, 0, C_MAXSTOPTIME);
    $(".popStopWindowBase .StopTimeLabel").html(nMinutes + $("#HiddenStopTimeWord").val());      
    $("#StopTimeTxt").css("display", "none");
    $(".StopTimeLabel").css("display", "block");
}

/**
* 数値の丸め込みを行う。<br>
* 
* @param {String} src 丸め込み対象の数値文字列
* @param {String} num 丸め込み基準の数値文字列(整数も可)
* @param {String} minnum 丸め込み後の最小値
* @param {String} maxnum 丸め込み後の最大値
* @return {Integer} 丸め込み後の数値
* 
* @example 
* RoundUpToNumUnits("a", "5", "5", "995");
* 出力:「5」
* RoundUpToNumUnits("-1", "5", "5", "995");
* 出力:「5」
* RoundUpToNumUnits("30", "5", "5", "995");
* 出力:「30」
* RoundUpToNumUnits("42", "5", "5", "995");
* 出力:「45」
*/

function RoundUpToNumUnits(src, num, minnum, maxnum) {

    var rtnVal = 0;
    var intNum = parseInt(num, C_RADIX);
    var intMinNum = parseInt(minnum, C_RADIX);
    var intMaxNum = parseInt(maxnum, C_RADIX);

    //丸め込み対象が数値の場合
    if (CheckOnlyHalfWidthDigitFormat(src)) {

        var intSrc = parseInt(src, C_RADIX);

        //丸め込み対象が負の場合
        if (intSrc <= 0) {
            rtnVal = intMinNum;
        }
        //丸め込み対象が丸め込み単位で割り切れる場合
        else if (intSrc % intNum == 0) {
            rtnVal = intSrc;
        }
        //それ以外
        else {
            rtnVal = intSrc + (intNum - (intSrc % intNum));
        }

        //丸め込み後の値が最小値を下回る場合
        if (rtnVal < intMinNum) {
            rtnVal = intMinNum;
        }
        //丸め込み後の値が最大値を超える場合
        else if (rtnVal > intMaxNum) {
            rtnVal = intMaxNum;
        }
    }
    //丸め込み対象が数値以外の場合
    else {
        rtnVal = intMinNum;
    }

    return rtnVal;
}

/**
* 正規表現での半角数字のみフォーマットチェックを行う。<br>
* 
* @param {String} src チェック対象文字列
* @return {Boolean} true:チェックOK/false:チェックNG
* 
*/


function CheckOnlyHalfWidthDigitFormat(src) {

    var rtnVal = CheckFormat(src, C_NUMPATTERN);

    return rtnVal;
}

/**
* 正規表現でのフォーマットチェックを行う。<br>
* 
* @param {String} src チェック対象文字列
* @param {String} pattern チェックに使用する正規表現パターン
* @return {Boolean} true:チェックOK/false:チェックNG
* 
*/

function CheckFormat(src, pattern) {

    var rtnVal = false;

    if (src.match(pattern)) {

        rtnVal = true;
    }

    return rtnVal;
}



//スクロールイベント作成
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
            var attr = data.inner.get(0).style["transform"];
            var m = attr.match(/translate3d\((.+)px,\s*(.+)px,\s*(.+)px\)/);
            return { top: parseInt(m[2]), left: parseInt(m[1]) };
        },

        /**
        * スクロール位置を設定
        * @param {Function} data 内部管理データ
        * @param {Position} position 位置
        */
        setTranslate: function (data, position) {
            data.inner.css({ "transform": "translate3d(" + position.left + "px, " + position.top + "px, 0px)" });
        },

        /**
        * スクロールバーの位置を設定
        * @param {Function} data 内部管理データ
        * @param {Position} position 位置
        */
        setScrollBarTranslate: function (data, position) {
            data.scrollBar.css({ "transform": "translate3d(0px, " + position.scrollTop + "px, 0px)", "opacity": 1 });
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
                $target.wrapInner('<div class="scroll-inner" style="transform:translate3d(0px,0px,0px);left:0px;top:0px;position:relative;" />');
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
                }
            } else {
                //開始
                data.target.removeClass("fingerscroll-stop");
                $inner.css({ "transform": "translate3d(0px, 0px, 0px)" });
            }

            //input要素配列を作成（height順）
            var inputs = $inner.find("input");
            data.inputs = [];
            data.manualFocus = false;
            data.topLimit = $target.offset().top + ($target.height() / 2);
            data.bottomLimit = $target.offset().top + ($target.height());
            for (i = 0; i < inputs.size(); i++) {
                var $a = $(inputs[i]);
                var ah = $a.offset().top;
                for (j = 0; j < data.inputs.length; j++) {
                    var $b = data.inputs[j];
                    var bh = $b.offset().top;
                    if (ah < bh) {
                        data.inputs.splice(j, 0, $a);
                        $a = null;
                        break;
                    }
                }
                if ($a) {
                    data.inputs.push($a);
                }
            }
            for (i = 0; i < data.inputs.length; i++) {
                data.inputs[i]
            		.data("pos", i)
                    .bind("touchstart mousedown", data, function (e) {
                        e.data.manualFocus = true;
                    })
             		.bind("focusin", data, function (e) {
             		    var self = $(this),
                            prevInput = e.data.target.data("prevInput"),
            		        prevIndex = (prevInput ? prevInput.data("pos") : -1),
            		        currIndex = self.data("pos"),
            		        deltaY = 0;

             		    //translate3dによる移動を行うとfocusinイベントが何度も発生する為、２回目以降のイベントを無視する
             		    if (prevInput && (prevInput.get(0) == self.get(0))) {
             		        return;
             		    }

             		    e.data.target.data("prevInput", self);

             		    if (e.data.manualFocus) {
             		        e.data.manualFocus = false;
             		    } else {
             		        if (prevIndex == -1) {
             		            //from outside
             		        } else if (prevIndex < currIndex) {
             		            //next input
             		            if (e.data.bottomLimit < self.offset().top) {
             		                deltaY = self.offset().top - prevInput.offset().top;
             		            }
             		        } else if (currIndex < prevIndex) {
             		            //prev input
             		            if (self.offset().top < e.data.topLimit) {
             		                if (-10 < fn.getTranslate(e.data).top) {
             		                    deltaY = 0;
             		                } else {
             		                    deltaY = self.offset().top - prevInput.offset().top;
             		                }
             		            }
             		        }
             		        e.data.target.fingerScroll({ action: "move", moveY: deltaY });
             		    }
             		});
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

            //位置保存
            e.data.position.y = y;
            e.data.position.x = x;

            //移動位置を記録
            if (e.data.captures.length > 4) e.data.captures.shift();
            e.data.captures.push({ x: e.data.position.x, y: e.data.position.y, time: new Date() });

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
            var timingFunction = "cubic-bezier(0.0, 1, 0.5, 1)"; //ease-outとまよう
            var src, eventName = "", aniTime = fn.constants.animateTimeNormal;
            var top = 0, left = 0;

            if (Math.abs(y2 - y1) > 7 || Math.abs(x2 - x1) > 7) {
                //移動距離を計算
                top = fn.constants.scrollDeltaMod * (y2 - y1);
                left = fn.constants.scrollDeltaMod * (x2 - x1);
                src = fn.calcScroll(e.data, { top: top, left: left }, "stop");
                if (fn.constants.scrollDeltaMod * Math.abs(x2 - x1) > 1000) aniTime = fn.constants.animateTimeLong;
            } else {
                //スクロール位置設定
                src = fn.calcScroll(e.data, { top: top, left: left }, "stop");
                aniTime = fn.constants.animateTimeNormal;
            }

            //フリックリリースのイベント名
            var startEvent = "", endEvnet = "";

            //フリックリリースイベントの発生処理
            if (e.data.isFlickReleaseTop === true) {
                //フリックリリース(上)を監視中
                if (src.overTopSize >= fn.constants.flickReleaseHeight) {
                    startEvent = "startFlickReleaseTop"; endEvnet = "endFlickReleaseTop";
                }
            } else if (e.data.isFlickReleaseBottom === true) {
                //フリックリリース(下)を監視中
                if (src.overBottomSize >= fn.constants.flickReleaseHeight) {
                    startEvent = "startFlickReleaseBottom"; endEvnet = "endFlickReleaseBottom";
                }
            }

            //フリックリリース開始前イベントを発生させる
            if (startEvent !== "") e.data.target.triggerHandler(startEvent);
            //スクロールアニメーションを開始する
            if (Math.abs(top) > 0 || Math.abs(left) > 0 || src.overTopSize > 0 || src.overBottomSize > 0) {

                //スクロール用のDIV
                e.data.inner.css({
                    "-webkit-transition": aniTime + "ms " + timingFunction,
                    "transform": "translate3d(" + src.left + "px, " + src.top + "px, 0px)"
                }).one("webkitTransitionEnd", e.data, function (we) {
                    we.data.scrollBar.fadeOut(fn.constants.scrollDuration);
                    we.data.inner.css({ "-webkit-transition": "none" });
                    //フリックリリース終了イベントを発生させる
                    if (endEvnet !== "") we.data.target.triggerHandler(endEvnet);
                });

                //スクロールバーのアニメーション
                if (Math.abs(top) > 0 || Math.abs(left) > 0) {
                    //スクロールバー
                    e.data.scrollBar.css({
                        "-webkit-transition": aniTime + "ms " + timingFunction,
                        "transform": "translate3d(0px, " + src.scrollTop + "px, 0px)"
                    }).one("webkitTransitionEnd", e.data, function (we) {
                        we.data.scrollBar.css({ "-webkit-transition": "none" });
                    });
                }
            } else {
                //アニメーションなしのスクロール
                fn.setTranslate(e.data, fn.calcScroll(e.data, { top: 0, left: 0 }, "stop"));
                e.data.scrollBar.fadeOut(0);
            }

            if (e.data.popover && (e.data.startPosition.x != x && e.data.startPosition.y != y)) {
                return false;
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
                "transform": "translate3d(0px, 0px, 0px)",
                "box-sizing": "border-box"
                , "visibility": "hidden"
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
    $.fn.StopmemoFingerscroll = function (param) {
        return this.each(function () {
            fn.init.call(this, param);
        });
    };

})(window);




//中断メモの制御
$(function () {

    // 中断メモの選択ボックス
    $("#dpDetailStopMemo")
        .click(function () {
            $("#dpDetailStopMemo").focus();
        })
        .blur(function () {
            // 中断メモに選択した内容を追加する
            var e = document.getElementById("dpDetailStopMemo");
            if (e.selectedIndex != 0) {
                $("#lblDetailStopMemo").text(e.options[e.selectedIndex].text);
                var strText = $("#txtStopMemo").val();
                strText += e.options[e.selectedIndex].text;
                $("#txtStopMemo").val(strText);
                // 中断メモに文字が追加されるため、高さ、文字最大数などを再計算
                DeleteOverString($("#txtStopMemo"));
                AdjusterStopTextArea();
            }
        });


    // 中断メモ
    $("#txtStopMemo")
        .blur(function () {
            DeleteOverString($("#txtStopMemo"));
            AdjusterStopTextArea();
            $("#btnJobStopDummy").focus();
        })

        .click(function () {
            DeleteOverString($("#txtStopMemo"));
            $("#txtStopMemo").focus();

        })
        .bind("paste", function (e) {
            setTimeout(function () {
                DeleteOverString($("#txtStopMemo"));
                AdjusterStopTextArea();
            }, 0);

        })
        .bind("keyup", function () {
            DeleteOverString($("#txtStopMemo"));
            AdjusterStopTextArea();

        })
        .bind("keydown", function () {
            DeleteOverString($("#txtStopMemo"));
        });
        $("#StopMemoScrollBox").StopmemoFingerscroll();
})
/**	
* テキストエリア内の文字列長制御を行う 	
* @param {$(textarea)} ta
*/
function DeleteOverString(ta) {
    //許容する最大バイト数

    var maxLen = ta.attr("maxlen");
    if (ta.val().length > maxLen) {
        ta.val(ta.val().substring(0, maxLen));
    }
}
/**	
* テキストエリア内の文字列長制御を行う 	
*/
function AdjusterStopTextArea() {

    var textArea = $("#txtStopMemo");


   // $("#StopMemoScrollBox .scroll-inner").height($("#StopMemoScrollBox .innerDataBox").height());

    textArea.height(100);

    // 表示されてる1行目の行数目を取得する
    var nScrollTop = $("#txtStopMemo").position().top;

    var nScrollHeight = nScrollTop % C_CELL_HEIGHT;
    var tmp_sh = textArea.attr("scrollHeight");

    while (tmp_sh > textArea.attr("scrollHeight")) {
        tmp_sh = textArea.attr("scrollHeight");
        textarea[0].scrollHeight++;
    }

    if (textArea.attr("scrollHeight") >= textArea.attr("offsetHeight")) {
        textArea.height(textArea.attr("scrollHeight"));
        if ($("#HiddenJobStopWindowFlg").val() == 0) {
            $("#StopMemoScrollBox .scroll-inner").height(445 + textArea.attr("scrollHeight") - 100);
        } else {
            $("#StopMemoScrollBox .scroll-inner").height(365 + textArea.attr("scrollHeight") - 100);
        }
        $("#StopMemoScrollBox .scroll-inner").css({ "transform": "translate3d(0px, 0px, 0px)" });
    }
}

/**	
* 呼び出し元のフラグを設定
*/
function WindowCallByFlg(value) {

    if (value == CAll_BY_SC3150101) {

        $("#HiddenJobStopWindowFlg").val(CAll_BY_SC3150101)

    } else if (value == CAll_BY_SC3150102) {

        $("#HiddenJobStopWindowFlg").val(CAll_BY_SC3150102)
    }

}

function SetRestJobFlg(value) {

    RestJobCount = value;

}