/**
* @fileOverview SC3010203 Todoチップドラッグ＆ドロップ処理
*
* @author TCS 寺本
* @version 1.0.0
*
* @update TS  2019/05/28 TS  舩橋 PostUAT-3098 カレンダーアプリ呼び出し変更
*/
/// <reference path="../jquery.js"/>
/// <reference path="../jquery.fingerscroll.js"/>
/// <reference path="../icropScript.js"/>
/// <reference path="SC3010203.Data.js"/>
/// <reference path="SC3010203.Layout.js"/>
(function (window) {

    //スケジュールオブジェクトに関数追加
    $.extend(schedule, {

        /**
        * @class イベントのハンドルクラス
        */
        eventHandle: {

            /**
            * タップホールド用タイマー
            * @return {Timer}
            */
            holdTimer: null,

            /**
            * マウスダウン・タップ開始
            * @param {Event} e イベント
            */
            down: function (e) {

                if ($("#isToDoChipDrop").val() === "1") return;

                //１本指のみ対象
                if (schedule.eventHandle.getFingerCnt() > 1) return;

                //対象チップを特定
                if ($(e.target).is("#todoChipBox .todoChip") === true) {
                    //チップ自体をタップ
                    e.data.dragChip = $(e.target);
                } else {
                    //チップ内の子タグをタップ
                    e.data.dragChip = $(e.target).parents("#todoChipBox .todoChip");
                    if (e.data.dragChip.length !== 1) return;
                }

                //ダミーチップを入れる箱
                e.data.copyChip = null;
                e.data.naviHourMinute = null;

                //イベント開始位置保存
                e.data.dragStart = schedule.eventHandle.getEventXY();
                e.data.position = schedule.eventHandle.getEventXY();
                //ホールドチェック
                e.data.holdFlg = true;
                //UID保存
                e.data.todoUID = e.data.dragChip.attr("UID");

                //イベントハンドル(タップホールド監視)
                $(document).bind("mousemove touchmove", e.data, schedule.eventHandle.move1);
                $(document).bind("mouseup touchend", e.data, schedule.eventHandle.end);

                //ホールド監視タイマー処理
                if (schedule.eventHandle.holdTimer) clearTimeout(schedule.eventHandle.holdTimer);
                schedule.eventHandle.holdTimer = setTimeout(function () {
                    schedule.eventHandle.clearHoldHandle();
                    if (e.data.holdFlg === true) {
                        //タップホールド成立
                        schedule.eventHandle.hold(e);
                    }
                }, 400);

                if (e.type === "mousedown") event.preventDefault();
            },

            /**
            * タップホールドの移動監視
            * @param {Event} e イベント
            */
            move1: function (e) {
                var movePosition = schedule.eventHandle.getEventXY();
                if (Math.abs(movePosition.x - e.data.dragStart.x) > 10 || Math.abs(movePosition.y - e.data.dragStart.y) > 10) {
                    schedule.eventHandle.end(e);
                }
            },

            /**
            * タップホールドイベントハンドリング解除
            */
            clearHoldHandle: function () {
                $(document).unbind("mousemove touchmove", schedule.eventHandle.move1);
                $(document).unbind("mouseup touchend", schedule.eventHandle.end);
            },

            /**
            * タップホールド監視終了
            * @param {Event} e イベント
            */
            end: function (e) {
                schedule.eventHandle.clearHoldHandle();
                e.data.holdFlg = false;

                var dragEnd = schedule.eventHandle.getEventXY();
                if (Math.abs(e.data.dragStart.x - dragEnd.x) >= 12
                    || Math.abs(e.data.dragStart.y - dragEnd.y) >= 12) {
                    //クリックイベントを発生させないようにする
                    $(e.target).bind("click", function (e2) {
                        if (event) event.stopPropagation();
                        if (event) event.preventDefault();
                        $(e.target).unbind("click", arguments.callee);
                        return false;
                    });
                }

            },

            /**
            * タップホールド成立
            * @param {Event} e イベント
            */
            hold: function (e) {

                //移動系イベント監視開始
                schedule.eventHandle.clearHoldHandle();

                //クリックイベントを発生させないようにする
                $(e.target).bind("click", function (e2) {
                    if (event) event.stopPropagation();
                    if (event) event.preventDefault();
                    $(e.target).unbind("click", arguments.callee);
                    return false;
                });

                //対象アイテムを取得
                var item = schedule.getTodoFromUid(e.data.todoUID);
                if (item === null) return;
                if (item.eventFlg === true || item.completion === true) return;

                //スクロール停止
                $("#todoChipBox").fingerScroll({ action: "stop" });
                //ダミーチップ作成
                e.data.copyChip = e.data.dragChip.clone();
                e.data.copyChip.addClass("dragChip").css({
                    "margin": "0px 0px 0px 6px",
                    "padding": "2px",
                    //"box-shadow": "3px 3px 5px #000",
                    "top": (e.data.dragChip.offset().top + 1) + "px",
                    "left": (e.data.dragChip.offset().left - 5) + "px",
                    //"border-top": "2px solid #868686",
                    //"border-left": "2px solid #868686",
                    //"border-bottom": "2px solid #f8f9f9",
                    //"border-right": "2px solid #f8f9f9"
                });

                //アイコン削除
                e.data.copyChip.find(".iconBox").remove();
                e.data.copyChip.find(".inText").css("padding-left", "2px");

                //親要素にセット
                e.data.dragChip.addClass("dragChipOwner");

                //終日イベント拡大中の時は、閉じる
                if ($("#DateScheduleBox ul").is(".normalMode") === false) dayEventSizeChange();

                //BODY末尾に追加
                $(document.body).append(e.data.copyChip);
                $(document).bind("mousemove touchmove", e.data, schedule.eventHandle.move2).bind("mouseup touchend", e.data, schedule.eventHandle.up);
            },

            /**
            * マウス移動・タップ移動(ドラッグ)
            * @param {Event} e イベント
            */
            move2: function (e) {

                var moveValue = { top: 0, left: 0 };
                //前のイベント位置取得
                var beforePosition = e.data.position;
                var afterPosition = schedule.eventHandle.getEventXY();

                $("#isToDoChipDrop").val("1");

                //移動距離を計算
                moveValue.left = afterPosition.x - beforePosition.x;
                moveValue.top = afterPosition.y - beforePosition.y;

                //チップを移動
                var bfCssTop = parseInt(e.data.copyChip.css("top"), 10) + moveValue.top;
                var bfCssLeft = parseInt(e.data.copyChip.css("left"), 10) + moveValue.left;
                e.data.copyChip.css({ "top": bfCssTop + "px", "left": bfCssLeft + "px" });

                //イベント開始位置保存
                e.data.position = afterPosition;

                //ドラッグエリアチェック
                var timeInfo = schedule.areaCheck(e);
                if (e.data.naviHourMinute === undefined || !e.data.naviHourMinute) {
                    //時分未設定
                    e.data.naviHourMinute = timeInfo;
                    schedule.changeNaviHourMinute(e);
                } else if (e.data.naviHourMinute.hour != timeInfo.hour || e.data.naviHourMinute.minute != timeInfo.minute) {
                    //時間変更
                    e.data.naviHourMinute = timeInfo;
                    schedule.changeNaviHourMinute(e);
                }

                if (event) event.preventDefault();
                return false;
            },

            /**
            * マウスアップ・タップ終了(ドラッグ終了)
            * @param {Event} e イベント
            */
            up: function (e) {

                //バインド解除
                $(document).unbind("mousemove touchmove", schedule.eventHandle.move2);
                $(document).unbind("mouseup touchend", schedule.eventHandle.up);

                $("#isToDoChipDrop").val("0");

                if (e.data.naviHourMinute !== undefined && e.data.naviHourMinute !== null && e.data.naviHourMinute.hour !== -1) {
                    //領域内へのドロップ
                    //スケジュール配列に登録
                    schedule.addScheduleFromTodo(e.data.todoUID, e.data.naviHourMinute, function () {
                        //ダミー削除
                        e.data.copyChip.remove();
                        e.data.dragChip.removeClass("dragChipOwner");
                    });

                } else {
                    //領域外へのドロップ
                    var position = { top: e.data.dragChip.offset().top + 1, left: e.data.dragChip.offset().left - 7 };
                    //移動している場合
                    if (parseInt(e.data.copyChip.css("top"), 10) != position.top || parseInt(e.data.copyChip.css("left"), 10) != position.left) {
                        var diff = Math.max(Math.abs(parseInt(e.data.copyChip.css("top"), 10) - position.top),
                                            Math.abs(parseInt(e.data.copyChip.css("left"), 10) - position.left));
                        var animateTime = diff >= 60 ? 500 : 140;
                        //元の位置に戻るアニメーション
                        e.data.copyChip.css({
                            "webkit-transition": animateTime + "ms ease-out",
                            "top": (e.data.dragChip.offset().top + 1) + "px",
                            "left": (e.data.dragChip.offset().left - 7) + "px"
                        }).one("webkitTransitionEnd", { copy: e.data.copyChip, owner: e.data.dragChip }, function (e) {
                            e.data.copy.fadeOut(50, function () {
                                e.data.owner.removeClass("dragChipOwner");
                                e.data.copy.remove();
                            });
                        });
                    } else { e.data.copyChip.remove(); }
                }

                //ナビゲーション非表示
                schedule.hideNaviHourMinute();

                //スクロール再開
                $("#todoChipBox").fingerScroll({ action: "restart" });
                //終了位置保存
                e.data.dragEnd = schedule.eventHandle.getEventXY();
                return false;
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
            * イベントを発生させた指の本数を取得
            * @return {Numver} 本数
            */
            getFingerCnt: function () {
                //PADの場合は本数、PCの場合は１本
                return event.changedTouches !== undefined && event.changedTouches
                       ? event.changedTouches.length : 1;
            }

        },

        /**
        * ドラッグorドロップのエリアチェック
        * @param {Event} e イベント
        */
        areaCheck: function (e) {

            var chipOffset = e.data.copyChip.offset();
            var boxOffset = $("#timeScheduleChipBox").offset();
            var boxRect = {
                x1: boxOffset.left,
                y1: boxOffset.top,
                x2: boxOffset.left + $("#timeScheduleChipBox").width(),
                y2: boxOffset.top + $("#timeScheduleChipBox").height()
            };

            //上部分が隠れている場合
            if (boxRect.y1 < $("#timeScheduleBoxOut").offset().top) boxRect.y1 = $("#timeScheduleBoxOut").offset().top;
            //下部分が隠れている場合
            if (boxRect.y2 > $("#timeScheduleBoxOut").offset().top + $("#timeScheduleBoxOut").height())
                boxRect.y2 = $("#timeScheduleBoxOut").offset().top + $("#timeScheduleBoxOut").height();

            var ret = { hour: -1, minute: -1 };

            //スケジュールチップ枠内かチェック
            if (boxRect.x1 <= chipOffset.left && boxRect.x2 >= chipOffset.left
                    && boxRect.y1 <= chipOffset.top && boxRect.y2 >= chipOffset.top) {
                //枠内にドラッグ又はドロップ

                //スケジュール上でのY座標
                var scheduleTop = chipOffset.top - (boxOffset.top);
                ret.hour = Math.floor(scheduleTop / schedule.constants.oneHourHeight);
                var mod = Math.ceil(scheduleTop % schedule.constants.oneHourHeight);

                //分を算出
                for (var i = 1; i <= 60 / schedule.constants.naviTime; i++) {
                    if (mod <= (schedule.constants.oneHourHeight / (60 / schedule.constants.naviTime)) * i) {
                        ret.minute = schedule.constants.naviTime * (i - 1);
                        break;
                    }
                }
            }

            //処理結果返却
            return ret;
        },

        /**
        * ナビゲーション時間の変更
        * @param {Event} e イベント
        */
        changeNaviHourMinute: function (e) {

            //枠外へのドラッグ
            if (e.data.naviHourMinute.hour === -1) {
                schedule.hideNaviHourMinute();
                return;
            }

            //時間を太字にする
            $("#timeScheduleLeftBox p.navihour").removeClass("navihour");
            $("#timeScheduleLeftBox p:nth-child(" + (e.data.naviHourMinute.hour + 1) + ")").addClass("navihour");

            //0分以外な分のナビゲーション
            if (e.data.naviHourMinute.minute === 0) {
                //0分
                $("#timeScheduleLeftBox p.naviminute").hide(0);
            } else {
                //0分以外
                var top = schedule.constants["oneHourHeight"] * e.data.naviHourMinute.hour;
                top += schedule.constants["oneHourHeight"] * (e.data.naviHourMinute.minute / 60);
                $("#timeScheduleLeftBox p.naviminute").css("top", (top - 8) + "px").text(":" + e.data.naviHourMinute.minute).show(0);
            }
        },

        /**
        * 指定UIDのTODOをスケジュールに追加する
        * @param {String} uid UID
        * @param {Function} hourMinute 時分
        * @param {Function} callback コールバック
        */
        addScheduleFromTodo: function (uid, hourMinute, callback) {

            var todo = schedule.getTodoFromUid(uid);
            var item = new schedule.calDavObject();
            item.chipType = "SCHEDULE";
            item.UID = uid;
            item.title = todo.title;
            item.dlrCd = todo.dlrCd;
            item.strCd = todo.strCd;
            item.dispTime = todo.dispTime;
            var n = new Date(parseInt($("#Yearhidden").val(), 10), parseInt($("#Monthhidden").val(), 10) - 1, parseInt($("#Dayhidden").val(), 10));
            item.startDateTime = new Date(n.getFullYear(), n.getMonth(), n.getDate(), hourMinute.hour, hourMinute.minute, 0);
            item.endDateTime = new Date(n.getFullYear(), n.getMonth(), n.getDate(), hourMinute.hour + 1, hourMinute.minute, 0);
            item.updateDate = new Date();
            item.backcolor = todo.backcolor;
            item.scheduleBackcolor = todo.scheduleBackcolor;
            schedule.scheduleItems.push(item);

            //ドラッグ不可にする
            todo.eventFlg = true;

            //連携開始
            schedule.ajaxRegistStart(todo, hourMinute.hour, hourMinute.minute);

            setTimeout(function () {
                //重複レベル設定
                schedule.setJuhukuLv();
                //スケジュール領域を張替え
                schedule.layoutChip.scheduleLayout();
                //コールバック
                callback.call(null);
            }, 0);
        },

        /**
        * @class アプリ起動クラス
        */
        appExecute: {

            // 2019/05/28 TS 舩橋 PostUAT-3098 カレンダーアプリ呼び出し変更 DEL 

            /**
            * 電話帳アプリ起動
            */
            executeCont: function () {
                window.location = "icrop:cont:";
                return false;
            },

            /**
            * 新車納車システムリンクメニュー押下時の処理
            * @param {String} url リンク先URL(URLスキーマ置き換え済み)
            */
            linkMenu: function (url) {
                location.href = url;
                return false;
            }

        }

    });
})(window);