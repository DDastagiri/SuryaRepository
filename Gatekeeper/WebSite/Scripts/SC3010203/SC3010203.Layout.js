/**
* @fileOverview SC3010203 画面レイアウト関数
*
* @author TCS 寺本
* @version 1.0.0
*/
/// <reference path="../jquery.js"/>
/// <reference path="../jquery.fingerscroll.js"/>
/// <reference path="../icropScript.js"/>
/// <reference path="SC3010203.Data.js"/>
(function (window) {

    //スケジュールオブジェクトに関数追加
    $.extend(schedule, {

        /**
        * @class 定数
        */
        constants: {
            oneHourHeight: 44,  //１時間の高さ
            herfHourHeight: 22, //３０分の高さ
            chipBoxWidth: 321,  //チップ配置エリアの幅
            naviTime: 15        //ナビゲーション単位(15分)
        },

        /**
        * 初期化
        */
        init: function () {
            //イベントバインド
            var data = {};
            $("#todoChipBox .todoChip").live("mousedown touchstart", data, schedule.eventHandle.down);
            //現在日付のバー
            schedule.setNowBar();
            //初期スクロール位置
            schedule.setInitPosition();
        },

        /**
        * @class チップレイアウトクラス
        */
        layoutChip: {

            /**
            * Todo、時間スケジュール、終日イベントのレイアウト処理
            */
            allLayout: function () {
                schedule.layoutChip.dayEventLayout();
                schedule.layoutChip.todoLayout();
                schedule.layoutChip.scheduleLayout();
            },

            /**
            * ToDoのレイアウト処理
            */
            todoLayout: function () {

                var delayCnt = 0;

                //全部クリア
                $("#todoChipBoxInner").empty();

                //Todo貼り付けループ
                $.each(schedule.todoItems, function (index, calDav) {

                    var $chip = $('<div class="todoChip"><span class="iconBox"></span><span class="date"></span><span class="inText"></span></div>');

                    $(".inText", $chip).text(calDav.title).CustomLabel({ useEllipsis: true }); //タイトル
                    $(".date", $chip).text(calDav.dispTime).CustomLabel({ useEllipsis: true });            //日時
                    $chip.attr("UID", calDav.UID);                      //UID
                    $chip.css("background", calDav.getTodoBackColor()); //背景

                    //アイコン
                    if ($.trim(calDav.iconPath) !== "") {
                        $(".iconBox", $chip).css("background", "url(" + calDav.iconPath + ") no-repeat");
                    } else {
                        $(".iconBox", $chip).css("display", "none");
                        $(".inText", $chip).css("padding-left", "2px");
                    }

                    //チップ配置ボックスに追加
                    $("#todoChipBoxInner").append($chip);
                    //遅れ件数
                    if (calDav.delay === true && calDav.completion === false) delayCnt++;
                });

                $("#todoDelayCount").text(delayCnt);
                $("#toDoBoxIn .toDoBoxNote .toDoBoxNoteInner").toggle(delayCnt !== 0);

            },

            /**
            * スケジュールのレイアウト処理
            */
            scheduleLayout: function () {

                //全部クリア
                $("#timeScheduleChipBox").empty();

                var addCount;
                var lv = 0;

                while (addCount !== 0) {
                    addCount = 0;
                    //スケジュール貼り付けループ
                    $.each(schedule.scheduleItems, function (index, calDav) {
                        //レベルチェック
                        if (calDav.juhukuLv !== lv) return true;
                        
                        //チップエレメント作成
                        var $chip = $("<div class='timeScheduleChip'/>").text(calDav.title).CustomLabel({ useEllipsis: true });
                        $chip.attr("UID", calDav.UID).css("background", calDav.getScheduleBackColor());
                        var rect = { left: "", top: "", width: "", height: "" };

                        //縦位置
                        var top = calDav.startDateTime.getHours() * schedule.constants["oneHourHeight"];
                        top += (calDav.startDateTime.getMinutes() / 60) * schedule.constants["oneHourHeight"];
                        rect.top = top + "px";

                        //横幅・横位置
                        if (calDav.juhukuLv === 0) {
                            //重複LV0
                            rect.left = "0px";
                            rect.width = "100%";
                        } else {
                            //1以降
                            rect.left = ((100 / (calDav.maxJuhukuLv + 1)) * (calDav.juhukuLv)) + "%";
                            rect.width = (100 / (calDav.maxJuhukuLv + 1)) + "%";
                        }

                        //縦幅
                        var df = calDav.getTimeDiff();
                        var height = df.hour * schedule.constants["oneHourHeight"];
                        height += (df.minute / 60) * schedule.constants["oneHourHeight"];
                        rect.height = height + "px";

                        //登録
                        $("#timeScheduleChipBox").append($chip.css(rect));
                        addCount++;

                    });
                    lv++;
                }
            },

            /**
            * 終日イベントのレイアウト処理
            */
            dayEventLayout: function () {

                //クリア
                $("#DateScheduleInner ul").empty();

                //終日イベント設定ループ
                $.each(schedule.dayEventItems, function (index, calDav) {
                    var $chip = $("<li/>").text(calDav.title).CustomLabel({ useEllipsis: true }).attr("UID", calDav.UID);
                    $("#DateScheduleInner ul").append($chip);
                });

                //２件以上存在する場合は拡大・縮小ボタン表示
                if (schedule.dayEventItems.length > 2) {
                    $("#DayEventOtherCount").text(schedule.dayEventItems.length - 2);
                    $("#DayEventBigSizeLink").show(0);
                } else {
                    $("#DayEventBigSizeLink").hide(0);
                }
                //データなしの場合のテキスト表示
                $("#dayEventNotFound").toggle(schedule.dayEventItems.length === 0);
            }
        },

        /**
        * 時分のナビゲーションを非表示にする
        */
        hideNaviHourMinute: function () {
            $("#timeScheduleLeftBox p.naviminute").hide(0);
            $("#timeScheduleLeftBox p.navihour").removeClass("navihour");
        },

        /**
        * 現在日時のバーを表示
        */
        setNowBar: function () {

            var serverHour = parseInt($("#HourHidden").val());
            var serverMinute = parseInt($("#MinuteHidden").val());
            //時間の高さ
            var top = (11 - 9) + serverHour * schedule.constants["oneHourHeight"];
            top += Math.ceil(schedule.constants["oneHourHeight"] * (serverMinute / 60));
            //バーの高さを設定する
            $("#timeScheduleBoxOut .borderLine").css("top", top + "px");

        },

        /**
        * 初期位置設定(8時をスクロールの先頭にもってくる)
        */
        setInitPosition: function () {
            setTimeout(function () {
                $("#timeScheduleBoxOut").fingerScroll({ action: "move", moveY: schedule.constants["oneHourHeight"] * 8 });
            }, 0);
        }

    });

})(window);