/**
* @fileOverview SC3010203　CalDavデータ定義および処理関数
*
* @author TCS 寺本
* @version 1.0.0
*/
/// <reference path="../jquery.js"/>
/// <reference path="../icropScript.js"/>
(function (window) {

    //スケジュールオブジェクトを拡張
    $.extend(window, { schedule: {} });

    //スケジュール管理データリスト宣言
    $.extend(schedule, {
    
        /**
        * Todoリスト
        * @return {Arrau}
        */
        todoItems: new Array(),

        /**
        * スケジュールリスト
        * @return {Arrau}
        */
        scheduleItems: new Array(),

        /**
        * 終日イベントリスト
        * @return {Arrau}
        */
        dayEventItems: new Array(),

        /**
        * UIDよりTODO情報取得
        * @param {String} uid コールバック
        * @return {Function} Todoオブジェクト
        */
        getTodoFromUid: function (uid) {
            var hit = null;
            $.each(schedule.todoItems, function (index, item) {
                if (item.UID == uid) {
                    hit = item;
                    return false;
                }
            });
            return hit;
        },

        /**
        * UIDよりEvent情報取得
        * @param {String} uid コールバック
        * @return {Function} Todoオブジェクト
        */
        getEventFromUid: function (uid) {
            var hit = null;
            $.each(schedule.scheduleItems, function (index, item) {
                if (item.UID == uid) {
                    hit = item;
                    return false;
                }
            });
            return hit;

        },

        /**
        * UIDより終日Event情報取得
        * @param {String} uid コールバック
        * @return {Function} Todoオブジェクト
        */
        getDayEventFromUid: function (uid) {
            var hit = null;
            $.each(schedule.dayEventItems, function (index, item) {
                if (item.UID == uid) {
                    hit = item;
                    return false;
                }
            });
            return hit;

        },

        /**
        * Todoソート
        */
        sortTodo: function () {

            //並び替え
            schedule.todoItems.sort(function (obj1, obj2) {
                //時間切捨て
                var truncDate1 = new Date(obj1.endDateTime.getFullYear(), obj1.endDateTime.getMonth(), obj1.endDateTime.getDate());
                var truncDate2 = new Date(obj2.endDateTime.getFullYear(), obj2.endDateTime.getMonth(), obj2.endDateTime.getDate());
                //日付での比較
                if (truncDate1.getTime() < truncDate2.getTime()) return -1;
                if (truncDate1.getTime() > truncDate2.getTime()) return 1;
                //時間指定の有無
                if (obj1.timeFlg === true && obj2.timeFlg === false) return -1;
                if (obj1.timeFlg === false && obj2.timeFlg === true) return 1;
                //時間の比較
                if (obj1.endDateTime.getTime() < obj2.endDateTime.getTime()) return -1;
                if (obj1.endDateTime.getTime() > obj2.endDateTime.getTime()) return 1;
                //接触方法
                if (obj1.contactNo == 6 && obj2.contactNo != 6) return -1;
                if (obj1.contactNo != 6 && obj2.contactNo == 6) return 1;
                if (parseInt(obj1.contactNo) < parseInt(obj2.contactNo)) return -1;
                if (parseInt(obj1.contactNo) > parseInt(obj2.contactNo)) return 1;
                return 0;
            });

        },

        /**
        * 重複レベル設定
        */
        setJuhukuLv: function () {

            //並び替え
            schedule.scheduleItems.sort(function (obj1, obj2) {

                if (obj1.startDateTime.getTime() < obj2.startDateTime.getTime()) return -1;
                if (obj1.startDateTime.getTime() > obj2.startDateTime.getTime()) return 1;
                //間隔
                if (obj1.endDateTime.getTime() - obj1.startDateTime.getTime() > obj2.endDateTime.getTime() - obj2.startDateTime.getTime()) {
                    return -1;
                } else if (obj1.endDateTime.getTime() - obj1.startDateTime.getTime() < obj2.endDateTime.getTime() - obj2.startDateTime.getTime()) {
                    return 1;
                } else {
                    //更新日時
                    if (obj1.updateDate.getTime() > obj2.updateDate.getTime()) return -1;
                    else if (obj1.updateDate.getTime() < obj2.updateDate.getTime()) return 1;
                    else return 0;
                }
            });


            //時間重複チェック
            function chkTime(targetItem, baseItem) {
                return (targetItem.startDateTime.getTime() >= baseItem.startDateTime.getTime()
                            && targetItem.startDateTime.getTime() < baseItem.endDateTime.getTime());
            }

            //重複チップ存在チェック
            function chkJuhuku(targetItem, endIndex, lv) {
                for (var i = 0; i <= endIndex; i++) {
                    if (schedule.scheduleItems[i].juhukuLv === lv) {
                        //重複有
                        if (chkTime(targetItem, schedule.scheduleItems[i]) === true) return true;
                    }
                }
                //なし
                return false;
            }

            //メインループ
            $.each(schedule.scheduleItems, function (index, item) {
                if (index === 0) {
                    item.lv = 0;
                    return true;
                }
                var lv = -1;
                var flg = true;

                //重複チェックループ
                while (flg === true) {
                    lv++;
                    flg = chkJuhuku(item, index - 1, lv);
                }
                //重複レベル決定
                item.juhukuLv = lv;
            });

            //最大重複レベルを設定
            $.each(schedule.scheduleItems, function (index, item) {
                var maxLv = item.juhukuLv;
                $.each(schedule.scheduleItems, function (index2, item2) {

                    //自身又は親のレベル
                    if (index === index2 || maxLv >= item2.juhukuLv) return true; 



                    //return (targetItem.startDateTime.getTime() >= baseItem.startDateTime.getTime()
                      //      && targetItem.startDateTime.getTime() < baseItem.endDateTime.getTime());

                    if (chkTime(item2, item) === true || chkTime(item, item2) === true) maxLv = item2.juhukuLv;
                });
                item.maxJuhukuLv = maxLv;
            });

        }

    });

})(window);

(function (window) {

    schedule.calDavObject = new Function();

    /**
    * @class CalDavデータクラス
    */
    schedule.calDavObject.prototype = {
        
        /**
        * チップのタイプ(TODO or SCHEDULE or DAYEVENT)
        * @return {String}
        */
        chipType: "",

        /**
        * キー
        * @return {String}
        */
        UID: "",

        /**
        * データ作成区分
        * @return {String}
        */
        createLocation: "",

        /**
        * 販売店
        * @return {String}
        */
        dlrCd: "",

        /**
        * 店舗
        * @return {String}
        */
        strCd: "",

        /**
        * スケジュールID
        * @return {String}
        */
        scheduleID: "",

        /**
        * TODOID
        * @return {String}
        */
        todoID: "",

        /**
        * スケジュール区分(0:来店、1:入庫)
        * @return {String}
        */
        scheduleDvs: "",

        /**
        * 接触方法No(1:来店予約、2:CALL-IN、3:CALL-OUT、4:SMS, 5:E-MAIL、6:DM)
        * @return {String}
        */
        contactNo: "",

        /**
        * 顧客種別
        * @return {String}
        */
        custKind: "",

        /**
        * 顧客分類
        * @return {String}
        */
        customerClass: "",

        /**
        * 顧客コード
        * @return {String}
        */
        crCustId: "",

        /**
        * タイトル
        * @return {String}
        */
        title: "",

       /**
        * 開始時間
        * @return {Date}
        */
        startDateTime: null,

       /**
        * 終了時間
        * @return {Date}
        */
        endDateTime: null,

       /**
        * 表示用の時間
        * @return {String}
        */
        dispTime: "",

       /**
        * 時間指定フラグ(true:指定、false:指定なし)
        * @return {boolean}
        */
        timeFlg: false,

       /**
        * 変更フラグ(true:変更、false:変更してない)
        * @return {boolean}
        */
        modify: false,

       /**
        * 重複レベル
        * @return {Number}
        */
        juhukuLv: 0,

       /**
        * 最大重複レベル
        * @return {Number}
        */
        maxJuhukuLv: 0,

       /**
        * 遅れフラグ
        * @return {boolean}
        */
        delay: false,

       /**
        * 完了フラグ
        * @return {boolean}
        */
        completion: false,

       /**
        * 更新日付
        * @return {Date}
        */
        updateDate: null,

       /**
        * アイコンパス
        * @return {String}
        */
        iconPath: "",

       /**
        * イベント有無しフラグ(TODO用) true:イベントあり、false:イベントなし(ドラッグ可)
        * @return {boolean}
        */
        eventFlg: false,

       /**
        * 背景色
        * @return {String}
        */
        backcolor: "255,255,255,1",

       /**
        * スケジュール用背景色
        * @return {String}
        */
        scheduleBackcolor: "255,255,255,1",

       /**
        * イベントのID
        * @return {String}
        */
        eventId: "",

        /**
        * rgbaオブジェクト作成
        * @return {Function} rgbaオブジェクト
        */
        getRgba: function (color) {
            var ary = color.split(",");
            var rgba = { r: 255, g: 255, b: 255, a: 1 };
            if (ary.length !== 4) return rgba;
            //数値に変換して格納
            rgba.r = parseInt(ary[0], 10);
            rgba.g = parseInt(ary[1], 10);
            rgba.b = parseInt(ary[2], 10);
            rgba.a = parseFloat(ary[3]);
            return rgba;
        },


        /**
        * TODOの背景色取得
        * @return {String} 色コード
        */
        getTodoBackColor: function () {
            return this._editBackColor(this.getRgba(this.backcolor));
        },

        /**
        * スケジュールの背景色取得
        * @return {String} 色コード
        */
        getScheduleBackColor: function () {
            return this._editBackColor(this.getRgba(this.scheduleBackcolor));
        },

        /**
        * 取得したカラーコードをCSSで使用できる形式に変換
        * @return {String} 色コード
        */
        _editBackColor: function(rgba) {
            cssText = "-webkit-gradient(linear, left top, right bottom,";
            cssText += "color-stop(0%,  rgba(" + rgba.r + "," + rgba.g + "," + rgba.b + "," + rgba.a + ")),";
            cssText += "color-stop(50%, rgba(" + rgba.r + "," + rgba.g + "," + rgba.b + "," + (rgba.a - 0.4) + ")),";
            cssText += "color-stop(100%,rgba(" + rgba.r + "," + rgba.g + "," + rgba.b + "," + rgba.a + ")))";
            return cssText;
        },

        /**
        * 時間間隔取得
        * @return {Number} 間隔
        */
        getTimeDiff: function () {
            var df = { hour: 0, minute: 0 };
            if (this.startDateTime === null || this.endDateTime === null) return df;
            var minute1 = this.startDateTime.getHours() * 60 + this.startDateTime.getMinutes();
            var minute2 = this.endDateTime.getHours() * 60 + this.endDateTime.getMinutes();
            df.hour = Math.floor((minute2 - minute1) / 60);
            df.minute = (minute2 - minute1) % 60;
            return df;
        }
    };

})(window);