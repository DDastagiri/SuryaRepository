/// <reference path="../jquery.js"/>
/// <reference path="../jquery.fingerscroll.js"/>
/// <reference path="../icropScript.js"/>
/// <reference path="../jquery.js"/>
/// <reference path="../jquery.fingerscroll.js"/>
/// <reference path="../icropScript.js"/>
/// <reference path="SC3010203.Data.js"/>
/// <reference path="SC3010203.Layout.js"/>
/// <reference path="SC3010203.Drag.js"/>
/// <reference path="SC3010203.Ajax.js"/>

//CalDav系処理
(function (window) {


    $.extend(schedule, {

        //終日イベント
        testDataDayEvent: function () {
            var item;
            var nowD = new Date();
            for (var i = 0; i < 100; i++) {
                item = new schedule.calDavObject();
                item.chipType = "DAYEVENT";
                item.UID = i;
                item.title = "終日イベント１１１１１１１１１１１１１１１１１１１" + (i + 1);
                item.dlrCd = "11A20";
                item.strCd = "000";
                item.dispTime = "今日";
                item.startDateTime = new Date(nowD.getFullYear(), nowD.getMonth(), nowD.getDate(), 0, 0, 0);
                item.endDateTime = new Date(nowD.getFullYear(), nowD.getMonth(), nowD.getDate(), 0, 0, 0);
                item.updateDate = new Date(nowD.getFullYear(), nowD.getMonth(), nowD.getDate(), 0, 0, 0);
                schedule.dayEventItems.push(item);
            }
        },

        //TODOテストデータ生成
        testDataTodo: function () {
            var item;
            var nowD = new Date();
            for (var i = 0; i <20; i++) {
                item = new schedule.calDavObject();
                item.chipType = "TODO";
                item.UID =  i;
                item.title = "テストチップ" + (i + 1);
                item.dlrCd = "11A20";
                item.strCd = "000";
                item.dispTime = "今日";
                item.startDateTime = new Date(nowD.getFullYear(), nowD.getMonth(), nowD.getDate(), i, 0, 0);
                item.endDateTime = new Date(nowD.getFullYear(), nowD.getMonth(), nowD.getDate(), i, 30, 0);
                item.updateDate = new Date(nowD.getFullYear(), nowD.getMonth(), nowD.getDate(), i, 30, 0);
                item.iconPath = "../Styles/Images/SC3010203/Nsc_Todo_Icon_callin.png";
                item.todoID = "3333";
                item.scheduleID = "11111";
                if (i == 0) item.backcolor = "255,0,0,0.7";
                if (i == 1) item.backcolor = "128,177,206,0.7";
                if (i == 2) item.backcolor = "192,139,212,0.7";
                if (i == 3) item.backcolor = "114,237,247,0.7";
                if (i == 4) item.backcolor = "181,189,189,0.7";
                if (i == 5) item.backcolor = "147,186,115,0.7";
                if (i >= 6) item.backcolor = "255,0,0,0.7";
                if (i <= 3) item.delay = true;
                schedule.todoItems.push(item);
            }
        },

        testDataSchedule: function () {
            var item;
            var nowD = new Date();

            for (var i = 0; i < 10; i++) {
                item = new schedule.calDavObject();
                item.chipType = "SCHEDULE";
                item.UID = i;
                item.title = "テストチップ" + (i + 1);
                item.dlrCd = "11A20";
                item.strCd = "000";
                item.dispTime = "-";
                item.startDateTime = new Date(nowD.getFullYear(), nowD.getMonth(), nowD.getDate(), i * 2, 0, 0);
                item.endDateTime = new Date(nowD.getFullYear(), nowD.getMonth(), nowD.getDate(), i * 2 + 1, 0, 0);
                item.updateDate = new Date(nowD.getFullYear(), nowD.getMonth(), nowD.getDate(), i * 2, 30, 0);

                if (i == 0) item.backcolor = "255,0,0,0.7";
                if (i == 1) item.backcolor = "128,177,206,0.7";
                if (i == 2) item.backcolor = "192,139,212,0.7";
                if (i == 3) item.backcolor = "114,237,247,0.7";
                if (i == 4) item.backcolor = "181,189,189,0.7";
                if (i == 5) item.backcolor = "147,186,115,0.7";
                if (i >= 6) item.backcolor = "255,0,0,0.7";
                schedule.scheduleItems.push(item);
            }
        }

    });
})(window);



