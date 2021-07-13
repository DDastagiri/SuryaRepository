/**
* @fileOverview SC3010203　データ取得・更新用通信関数
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
        * CalDavよりデータ取得
        * @param {Function} callback コールバック
        */
        loadCalDav: function (callback) {
            //サーバー問い合わせ
            schedule.ajaxStart(function () {
                //重複レベル設定
                schedule.setJuhukuLv();
                //TODOソート
                schedule.sortTodo();
                callback.call(null);
            });

            //テストデータ
            /*
            schedule.testDataTodo();
            schedule.testDataSchedule();
            schedule.testDataDayEvent();
            callback.call(null);
            */
        },

        /**
        * データ取得通信開始
        * @param {Function} callback コールバック
        */
        ajaxStart: function (callback) {

            //ページメソッドの呼び出し
            PageMethods.GetScheduleXmlText(
                function (result, userContext, methodName) {
                    //パース
                    var doc = $($.parseXML(result));
                    $(doc).find("Detail").each(schedule.xmlReadFuncs.detailLoopInner);
                    callback.call(null);
                },
                function () {
                    //エラー表示
                    setTimeout(function () {
                        icropScript.ShowMessageBox(0, $("#CaldavSelectErrorMessage").val(), "");
                        callback.call(null);
                    }, 100);
                });
        },

        /**
        * データ登録通信開始
        * @param {Function} todoItem Todoクラス
        * @param {Number} hour   時
        * @param {Number} minute 分
        */
        ajaxRegistStart: function (todoItem, hour, minute) {

            //ページメソッドの呼び出し(登録メソッド)
            PageMethods.RegistSchedule(todoItem.dlrCd,
                todoItem.strCd,
                todoItem.todoID,
                todoItem.scheduleID,
                parseInt($("#Yearhidden").val(), 10),
                parseInt($("#Monthhidden").val(), 10),
                parseInt($("#Dayhidden").val(), 10),
                hour,
                minute,
                function (result, userContext, methodName) {
                    //エラーメッセージ
                    if (result != 0) {
                        icropScript.ShowMessageBox(0, $("#CaldavRegistErrorMessage").val(), "");
                        window.location.reload();
                    }
                },
                function () {
                    //エラー
                    icropScript.ShowMessageBox(0, $("#CaldavRegistErrorMessage").val(), "");
                    window.location.reload();
                });
        },

        /**
        * @class xml解析処理クラス
        */
        xmlReadFuncs: {

            //Detailタグのループ内部処理
            detailLoopInner: function () {

                //COMMON読み出し
                var commonXml = $(this).find("Common");
                //スケジュール
                var scheduleInfo = $(this).find("ScheduleInfo");
                //TODOタグ分ループ
                $(this).find("VTodo").each(function () {
                    schedule.xmlReadFuncs.todoLoopInner(commonXml, scheduleInfo, $(this));
                });
                //イベントタグ分ループ
                $(this).find("VEvent").each(function () {
                    schedule.xmlReadFuncs.scheduleLoopInner(commonXml, scheduleInfo, $(this));
                });
            },

            /**
            * TODOタグの読み出し
            * @param {jQuery} commonXml     共通ノード
            * @param {jQuery} scheduleInfo  スケジュールノード
            * @param {jQuery} todoXml       Todoノード
            */
            todoLoopInner: function (commonXml, scheduleInfo, todoXml) {

                //入庫予約はTODOが表示しない
                //if (todoXml.find("ScheduleDiv") == "1") return;

                var item = new schedule.calDavObject();
                item.chipType = "TODO";
                item.UID = todoXml.find("SeqNo").text();
                item.createLocation = commonXml.find("CreateLocation").text();
                item.dlrCd = commonXml.find("DealerCode").text();
                item.strCd = commonXml.find("BranchCode").text();
                item.scheduleID = commonXml.find("ScheduleID").text();
                item.scheduleDvs = commonXml.find("ScheduleDiv").text();
                item.todoID = todoXml.find("TodoID").text();
                //顧客詳細遷移用
                item.custKind = scheduleInfo.find("CustomerDiv").text();
                item.customerClass = scheduleInfo.find("CustomerDiv").text();
                item.crCustId = scheduleInfo.find("CustomerCode").text();

                item.contactNo = todoXml.find("ContactNo").text();
                item.title = todoXml.find("Summary").text();
                item.startDateTime = schedule.xmlReadFuncs.convXmlDateStringToDate(todoXml.find("DtStart").text());
                item.endDateTime = schedule.xmlReadFuncs.convXmlDateStringToDate(todoXml.find("Due").text());
                item.dispTime = todoXml.find("DispTime").text();
                item.timeFlg = todoXml.find("TimeFlg").text() == "1" ? true : false;
                item.delay = todoXml.find("Delay").text() == "1" ? true : false;
                item.completion = todoXml.find("CompFlg").text() == "1" ? true : false;
                item.backcolor = todoXml.find("XiCropColor").text();
                item.scheduleBackcolor = todoXml.find("ScheduleColor").text();
                item.eventFlg = todoXml.find("EventFlg").text() == "1" ? true : false;
                item.iconPath = todoXml.find("IconPath").text();
                //item.updateDate = new Date();
                //TODOリストに追加
                item.UID = "T" + (schedule.todoItems.length + 1);
                schedule.todoItems.push(item);
            },

            /**
            * イベントタグ読み出し
            * @param {jQuery} commonXml     共通ノード
            * @param {jQuery} scheduleInfo  スケジュールノード
            * @param {jQuery} scheduleXml   スケジュールノード
            */
            scheduleLoopInner: function (commonXml, scheduleInfo, scheduleXml) {

                var item = new schedule.calDavObject();
                item.chipType = "SCHEDULE";
                item.createLocation = commonXml.find("CreateLocation").text();
                item.dlrCd = commonXml.find("DealerCode").text();
                item.strCd = commonXml.find("BranchCode").text();
                item.scheduleID = commonXml.find("ScheduleID").text();
                item.scheduleDvs = commonXml.find("ScheduleDiv").text();
                //顧客詳細遷移用
                item.custKind = scheduleInfo.find("CustomerDiv").text();
                item.customerClass = scheduleInfo.find("CustomerDiv").text();
                item.crCustId = scheduleInfo.find("CustomerCode").text();

                //alert(scheduleXml.find("Summary").text() + scheduleXml.find("DtStart").text());
                item.contactNo = scheduleXml.find("ContactNo").text();
                item.title = scheduleXml.find("Summary").text();
                
                item.startDateTime = schedule.xmlReadFuncs.convXmlDateStringToDate(scheduleXml.find("DtStart").text());
                item.endDateTime = schedule.xmlReadFuncs.convXmlDateStringToDate(scheduleXml.find("DtEnd").text());
                item.dispTime = scheduleXml.find("DispTime").text();
                item.timeFlg = scheduleXml.find("TimeFlg").text() == "1" ? true : false;
                item.delay = scheduleXml.find("Delay").text() == "1" ? true : false;
                item.backcolor = scheduleXml.find("XiCropColor").text();
                item.scheduleBackcolor = scheduleXml.find("ScheduleColor").text();
                item.completion = scheduleXml.find("CompFlg").text() == "1" ? true : false;
                item.eventId = scheduleXml.find("EventID").text();
                item.updateDate = schedule.xmlReadFuncs.convXmlDateStringToDate(scheduleXml.find("UpdateDate").text());

                if (scheduleXml.find("AllDayFlg").text() == "1") {
                    //終日イベントリストに追加
                    item.UID = "D" + (schedule.dayEventItems.length + 1);
                    schedule.dayEventItems.push(item);
                }
                else {
                    //スケジュールリストに追加
                    item.UID = "S" + (schedule.scheduleItems.length + 1);
                    schedule.scheduleItems.push(item);
                }
            },

            /**
            * 日付変換(yyyy/MM/dd HH:mm:ss)
            * @param {String} textDate   日付文字列
            * @return {Date} 日付オブジェクト
            */
            convXmlDateStringToDate: function (textDate) {
                //空文字
                if ($.trim(textDate).length === 0) return null;
                var year = parseInt(textDate.substring(0, 4), 10);
                var month = parseInt(textDate.substring(5, 7), 10) - 1;
                var day = parseInt(textDate.substring(8, 10), 10);
                var hour = parseInt(textDate.substring(11, 13), 10);
                var minute = parseInt(textDate.substring(14, 16), 10);
                //日付型を変換する
                return new Date(year, month, day, hour, minute, 0);
            }
        }

    });
})(window);