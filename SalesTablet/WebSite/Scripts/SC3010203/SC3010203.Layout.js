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

                //各チップ数
                var TotalCntSalesTodoChip = 0
                var unCompCntSalesTodoChip = 0
                var TotalCntBookedAfterTodoChip = 0
                var unCompCntBookedAfterTodoChip = 0
                var TotalCntDeliAfterTodoChip = 0
                var unCompCntDeliAfterTodoChip = 0
                var toDayDate = new Date()
                var inDisPlayDate = new Date($("#isDisplayDate").val()) 

                //全部クリア
                $("#SalestodoChipBoxInner").empty();
                $("#BookedAftertodoChipBoxInner").empty();
                $("#DeliAftertodoChipBoxInner").empty();

                //Todo貼り付けループ
                $.each(schedule.todoItems, function (index, calDav) {

                    //チップを作成
                    var $chip = $('<div id="SCMainChip" class="SCMainChip todoChip"><div class="InnerBox"><span class="inUserName useEllipsis"></span><br><span class="addInfomation useEllipsis"></span><span class="IconBox01"><a href="#"><img src="" width="20" height="20" class="iconBox"></a></span><span class="checkPoint1"></span><span class="checkPoint2"></span></div></div>');

                    $(".inUserName", $chip).text(calDav.CustomerName).CustomLabel({ useEllipsis: true });      //顧客名
                    $(".addInfomation", $chip).text(calDav.ContactName).CustomLabel({ useEllipsis: true });    //接触方法
                    $(".checkPoint1", $chip).text(calDav.Rslt).CustomLabel({ useEllipsis: true });          //初回商談日 or 契約日 or 納車日
                    $(".checkPoint2", $chip).text(calDav.dispTime).CustomLabel({ useEllipsis: true });         //次回活動日
                    $chip.css("background", calDav.getTodoBackColor());                                        //背景
                    $chip.css("border", "#EBEBEB 2px solid");                                                  //枠線
                    $chip.attr("UID", calDav.UID);                                                             //UID

                    //完了済み判定
                    if (calDav.completion == "1") {
                        $chip.addClass("completion");
                    }

                    //アイコン
                    if ($.trim(calDav.iconPath) !== "") {
                        $(".iconBox", $chip).attr("src", calDav.iconPath);
                    } else {
                        $(".iconBox", $chip).css("display", "none");
                        $(".inText", $chip).css("padding-left", "2px");
                    }

                    //受注区分で処理を分岐
                    if (calDav.OdrDiv == "0") {
                        $("#SalestodoChipBoxInner").append($chip);
                        TotalCntSalesTodoChip++;
                        //完了済み判定
                        if (calDav.completion == "0") {
                            unCompCntSalesTodoChip++;
                        }
                    } else if (calDav.OdrDiv == "1") {
                        $("#BookedAftertodoChipBoxInner").append($chip);
                        TotalCntBookedAfterTodoChip++;
                        //完了済み判定
                        if (calDav.completion == "0") {
                            unCompCntBookedAfterTodoChip++;
                        }
                    } else if (calDav.OdrDiv == "2") {
                        $("#DeliAftertodoChipBoxInner").append($chip);
                        TotalCntDeliAfterTodoChip++;
                        //完了済み判定
                        if (calDav.completion == "0") {
                            unCompCntDeliAfterTodoChip++;
                        }
                    }

                });

                //ToDoチップの表示対象の切り替え
                ToDoDispChange()

                //件数を反映
                //$("#TotalCntSalesTodoChip").text(TotalCntSalesTodoChip);
                //$("#unCompCntSalesTodoChip").text(unCompCntSalesTodoChip);

                //$("#TotalCntBookedAfterTodoChip").text(TotalCntBookedAfterTodoChip);
                //$("#unCompCntBookedAfterTodoChip").text(unCompCntBookedAfterTodoChip);

                //$("#TotalCntDeliAfterTodoChip").text(TotalCntDeliAfterTodoChip);
                //$("#unCompCntDeliAfterTodoChip").text(unCompCntDeliAfterTodoChip);

                if ($("#Yearhidden").val() == toDayDate.getFullYear()
                && $("#Monthhidden").val() == toDayDate.getMonth() + 1
                && $("#Dayhidden").val() == toDayDate.getDate()) {
                    $("#CntSalesTodoChip").text(unCompCntSalesTodoChip.toString() + this_form.slash.value + TotalCntSalesTodoChip.toString())
                    $("#CntBookedAfterTodoChip").text(unCompCntBookedAfterTodoChip.toString() + this_form.slash.value + TotalCntBookedAfterTodoChip.toString())
                    $("#CntDeliAfterTodoChip").text(unCompCntDeliAfterTodoChip.toString() + this_form.slash.value + TotalCntDeliAfterTodoChip.toString())
                } else {
                    $("#CntSalesTodoChip").text(TotalCntSalesTodoChip.toString())
                    $("#CntBookedAfterTodoChip").text(TotalCntBookedAfterTodoChip.toString())
                    $("#CntDeliAfterTodoChip").text(TotalCntDeliAfterTodoChip.toString())

                }

                //くるくるを非表示にする
                $("#toDoBoxIn").removeClass("loadingToDo1")
                $("#BookedAftertoDoBoxIn").removeClass("loadingToDo2")
                $("#DeliAftertoDoBoxIn").removeClass("loadingToDo3")

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
                        var $chip = $('<div class="timeScheduleChip ScheduleChip Small"><div class="InnerBox"><span class="inUserName"></span></div></div>')
                        $chip.attr("UID", calDav.UID).css("background", calDav.getScheduleBackColor());
                        $chip.attr("UID", calDav.UID).css("border", "#EBEBEB 2px solid");
                        $(".inUserName", $chip).text(calDav.title).CustomLabel({ useEllipsis: true });

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
                var setHour;
                if (parseInt($("#HourHidden").val()) <= 19) {
                    var setHour = parseInt($("#HourHidden").val() - 1);
                } else {
                    var setHour = 19;
                }
                $("#timeScheduleBoxOut").fingerScroll({ action: "move", moveY: schedule.constants["oneHourHeight"] * setHour });
            }, 0);
        }

    });

})(window);