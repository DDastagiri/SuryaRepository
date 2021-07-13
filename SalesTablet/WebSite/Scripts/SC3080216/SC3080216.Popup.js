/**
* @fileOverview SC3080216 工程リスト／日時指定ポップアップ制御処理
*
* @author TCS 安田
* @version 1.0.0
* 
* 更新： 2013/10/02 TCS 安田 次世代e-CRBタブレット iOS7.0 VersionUp対応（セールス）
*/

//工程リスト／日時指定ポップアップ制御クラス
var DatePopupClass = function () {

    //ページ番号　(page1:工程リストポップアップ、page2:日時指定ポップアップ)
    this.planPageNo = "page2";

    //複数工程フラグ　(1:単一工程、2:複数工程)
    this.planPopupFlg = "1";

    //現在表示中のProcessDateClass
    this.activePlanDataCls = null;

    //自身のクラス (イベント処理で使用するため)
    var myClass = this;

    //工程リストから次のページへ遷移する
    //前工程、次工程セット
    this.movePlanPage = function (pageNo) {
        this.planPageNo = pageNo;
        $("#scNscPlanTimeWindown #scNscPlanTimeWindownBox .scNscPlanTimeListBox").removeClass("page1 page2").addClass(pageNo);
        this.setTimeSelect();
        this.setPlanPageLabel();
    }

    //工程リストのキャプションの設定をする
    this.setPlanPageLabel = function () {

        var strDateTitle = $("#planDateHidden").val();                               //計画日

        if (this.planPopupFlg == "1") {

            $("#planCancelLabel").text($("#planCancelBtnHidden").val());        //キャンセル
            $("#planTitleLabel").text(this.activePlanDataCls.strTitle);         //○○日時　

            $(".scNscPlanTimeCancellButton").removeClass("scNscPlanTimeCancellButton1 scNscPlanTimeCancellButton2").addClass("scNscPlanTimeCancellButton2");

            $("#dateTimeCompletionButton").css("display", "block");
            $(".scNscPlanTimeCancellButtonArrow").css("display", "none");

        } else {
            //工程リスト
            if (this.planPageNo == "page1") {
                $("#planCancelLabel").text($("#planCancelBtnHidden").val());    //キャンセル
                $("#planTitleLabel").text(strDateTitle);

                $(".scNscPlanTimeCancellButton").removeClass("scNscPlanTimeCancellButton1 scNscPlanTimeCancellButton2").addClass("scNscPlanTimeCancellButton2");

                $("#dateTimeCompletionButton").css("display", "block");
                $(".scNscPlanTimeCancellButtonArrow").css("display", "none");
            }
            //日付指定
            if (this.planPageNo == "page2") {
                $("#planCancelLabel").text(strDateTitle);                       //計画日
                $("#planTitleLabel").text(this.activePlanDataCls.strTitle);     //○○日時

                $(".scNscPlanTimeCancellButton").removeClass("scNscPlanTimeCancellButton1 scNscPlanTimeCancellButton2").addClass("scNscPlanTimeCancellButton1");

                $("#dateTimeCompletionButton").css("display", "none");
                $(".scNscPlanTimeCancellButtonArrow").css("display", "block");

            }
        }
    }

    //時間指定の設定オン／オフ変更時の対応
    this.setTimeSelect = function () {

        if ($("#endPlanTime").val() == "") {
            //時間指定なし
            $("#planEndTime").removeClass("whiteColor glayColor").addClass("glayColor");
            $("#planEndTime2").removeClass("whiteColor glayColor").addClass("glayColor");
            $("#endPlanTime").css("display", "none");

            this.setCheck($("#timeSelect"), false);

            $("#startPlanTime").css("display", "none");
            $("#startPlanDate").css("display", "block");
        } else {
            //時間指定あり
            $("#planEndTime").removeClass("whiteColor glayColor").addClass("whiteColor");
            $("#planEndTime2").removeClass("whiteColor glayColor").addClass("whiteColor");
            $("#endPlanTime").css("display", "block");

            this.setCheck($("#timeSelect"), true);

            $("#startPlanTime").css("display", "block");
            $("#startPlanDate").css("display", "none");
        }

    }

    this.clickEvent = false;

    //時間指定の設定オン／オフを切り替える
    this.setCheck = function (targetElement, flg) {

        if ($("#timeSelect").attr("checked") != flg) {
            this.clickEvent = true;
            $("#scNscPlanTimeWindown #scNscPlanTimeWindownBox .icrop-SwitchButton").click();
            this.clickEvent = false;
        }
    }


    //画面表示
    this.DisplayPopup = function (planPopupFlg, planPageNo) {

        //計画日指定ポップアップ表示
        $("#scNscPlanTimeWindown #scNscPlanTimeWindownBox .scNscPlanTimeListBox").removeClass("page1 page2").addClass(planPageNo);

        //複数工程－１ページ目を表示
        this.planPopupFlg = planPopupFlg;
        this.planPageNo = planPageNo;

        this.setTimeSelect();        //時間指定オン／オフ

        //タイトル等表示
        this.setPlanPageLabel();

        //ボタンを押下不可にする
        $("#dateTimeCompletionButton").removeClass("scNscPlanTimeCompletionButton scNscPlanTimeCompletionButtonHidden").addClass("scNscPlanTimeCompletionButtonHidden");

    }

    //時間指定チェック押下時
    $(".timeSelect").click(function (e) {

        if (myClass.clickEvent == true) {
            myClass.clickEvent = false;
            return true;
        }
        if ($("#timeSelect").attr("checked")) {
            //var strTime = $("#startPlanDate").val() + "T00:00:00+09:00";
            //$("#startPlanTime").val(strTime);

/* 2013/10/02 TCS 安田 次世代e-CRBタブレット iOS7.0 VersionUp対応（セールス） START */
            if ($("#startPlanDate").val() == "") {
                $("#startPlanTime").val("");
            }else{
                var dtTemp = $("#startPlanDate").get(0).valueAsDate;
                dtTemp.setHours(0, 0, 0, 0);
                $("#startPlanTime").val(geDateTimelocalDate(dtTemp));
            }

            //$("#startPlanTime").get(0).valueAsDate = dtTemp;
/* 2013/10/02 TCS 安田 次世代e-CRBタブレット iOS7.0 VersionUp対応（セールス） END */


            $("#endPlanTime").val("00:00");
        } else {

/* 2013/10/02 TCS 安田 次世代e-CRBタブレット iOS7.0 VersionUp対応（セールス） START */

            if ($("#startPlanTime").val() == "") {
               $("#startPlanDate").val("");
            }else{
               var dtTemp = changeStringToDateIcrop($("#startPlanTime").val());
               $("#startPlanDate").val(geHeifunDate(dtTemp));
            }
            
            //var dtTemp = $("#startPlanTime").get(0).valueAsDate;
/* 2013/10/02 TCS 安田 次世代e-CRBタブレット iOS7.0 VersionUp対応（セールス） END */
            

            $("#endPlanTime").val("");
        }

        myClass.setTimeSelect();

        //ボタンを押下可にする
        $("#dateTimeCompletionButton").removeClass("scNscPlanTimeCompletionButton scNscPlanTimeCompletionButtonHidden").addClass("scNscPlanTimeCompletionButton");

    });

    //開始／終了日時の必須チェック
    function checkPlanDate() {

        if ($("#timeSelect").attr("checked")) {
            //時間指定オン
            if ($("#startPlanTime").val() == "") {
                alert($("#startDateTimeEmptyErrorHidden").val());
                return false;
            }
            if ($("#endPlanTime").val() == "") {
                alert($("#endDateTimeEmptyErrorHidden").val());
                return false;
            }

/* 2013/10/02 TCS 安田 次世代e-CRBタブレット iOS7.0 VersionUp対応（セールス） START */
            var dtTempDt = changeStringToDateIcrop($("#startPlanTime").val());
            var hhmm = getHHMM(dtTempDt);
            //var hhmm = getHHMM($("#startPlanTime").get(0).valueAsDate);
/* 2013/10/02 TCS 安田 次世代e-CRBタブレット iOS7.0 VersionUp対応（セールス） END */

            var hhmm2 = $("#endPlanTime").val();

            if (hhmm > hhmm2) {
                alert($("#checkPlanTimeErrorHidden").val());
                return false;
            }

        } else {
            //時間指定オフ
            if ($("#startPlanDate").val() == "") {
                alert($("#startDateEmptyErrorHidden").val());
                return false;
            }
        }

        return true;
    }

    //アイコン表示表示
    this.displayIcon = function () {

        //全工程を対象に再表示処理
        for (var i = 0; i < procClsArray.length; i++) {
            procCls = procClsArray[i];

            //Hidden項目に日付を設定（確定）する
            procCls.commotTimeTemp();

            //アイコン表示する
            procCls.planDataCls.displayIcon();
        }

        //アイコンを傾ける
        setTimeout(function () {

            //全工程を対象に再表示処理
            for (var i = 0; i < procClsArray.length; i++) {
                procCls = procClsArray[i];

                //アイコンを傾ける
                procCls.planDataCls.transformIcon();
            }
        }, 300);
    }


    //開始日付 (変更時、完了ボタンを押下可にする)
    $("#startPlanDate").blur(function () {

        if ($("#startPlanDate").val() != geHeifunDate(myClass.activePlanDataCls.tempStartDate)) {
            //ボタンを押下可にする
            $("#dateTimeCompletionButton").removeClass("scNscPlanTimeCompletionButton scNscPlanTimeCompletionButtonHidden").addClass("scNscPlanTimeCompletionButton");
        }
    });

    //開始時間 (変更時、完了ボタンを押下可にする)
    $("#startPlanTime").blur(function () {

        if ($("#startPlanTime").val() != myClass.activePlanDataCls.tempStartDate) {
            //ボタンを押下可にする
            $("#dateTimeCompletionButton").removeClass("scNscPlanTimeCompletionButton scNscPlanTimeCompletionButtonHidden").addClass("scNscPlanTimeCompletionButton");
        }
    });

    //終了時間 (変更時、完了ボタンを押下可にする)
    $("#endPlanTime").blur(function () {

        if ($("#endPlanTime").val() != myClass.activePlanDataCls.tempEndDate) {
            //ボタンを押下可にする
            $("#dateTimeCompletionButton").removeClass("scNscPlanTimeCompletionButton scNscPlanTimeCompletionButtonHidden").addClass("scNscPlanTimeCompletionButton");
        }
    });

    //時間指定ポップアップ－完了ボタン
    $(".scNscPlanTimeCompletionButton").click(function () {

        //ボタンが非可視の場合は処理しない
        if ($(this).hasClass("scNscPlanTimeCompletionButton") === false) {
            return;
        }

        if (myClass.planPopupFlg == "1") {

            //日時指定ポップアップ　（単一工程）

            //入力チェック
            if (checkPlanDate() == false) {
                return;
            }

            //入力値を保存する
            myClass.activePlanDataCls.setTimeTemp();

            //入力チェック
            if (myClass.activePlanDataCls.checkPlanDate(0) == false) {
                return;
            }

            //Hidden項目に日付を設定（確定）する
            myClass.activePlanDataCls.commotTimeTemp();

            //アイコン表示する
            myClass.displayIcon();

        } else {
            //工程リストポップアップ　（複数工程）

            //入力チェック
            var procCls = null;
            for (var i = 0; i < procClsArray.length; i++) {
                procCls = procClsArray[i];
                if (procCls.checkPlanDate(1) == false) {
                    return;
                }
            }

            //アイコン表示する
            myClass.displayIcon();
        }

        //ポップアップを閉じる
        $("#bodyFrame").trigger("click.popover");

    });


    //時間指定ポップアップ－キャンセルボタン
    $(".scNscPlanTimeCancellButton").click(function () {

        if (myClass.planPopupFlg == "1") {

            //日時指定ポップアップ　（単一工程）
            //ポップアップを閉じる
            $("#bodyFrame").trigger("click.popover");

        } else {

            if (myClass.planPageNo == "page1") {
                //ポップアップを閉じる
                $("#bodyFrame").trigger("click.popover");
                return;
            }
            //工程リストポップアップ　（複数工程）
            //入力チェック
            if (checkPlanDate() == false) {
                return;
            }

            if (myClass.planPageNo == "page2") {

                //入力値をテンポラリーに保存する
                myClass.activePlanDataCls.setTimeTemp();

                //日時ラベルを表示する
                myClass.activePlanDataCls.setLiLabel();

                //１ページ目へ遷移する
                myClass.movePlanPage("page1");
            }
        }
    });
}




//工程リストのアイコン操作用クラス
//planDataCls   計画のアイコン操作用クラス
//popupDiv      Popup起動Div
//liTag         明細行liタグ
//strTitle      タイトル
var ProcessDateClass = function (perentCls, planDataCls, popupDiv, liTag, strTitle) {

    //計画のアイコン操作用クラス
    this.planDataCls = planDataCls;

    //Popup起動Div
    this.popupDiv = popupDiv;

    //工程リストのLiタグ
    this.liTag = liTag;

    //日付ポップアップのタイトル
    this.strTitle = strTitle;

    //工程リストのアイコンSPAN
    this.processIconDiv = liTag.children("span:nth-child(1)");

    //工程リストの日時表示SPAN
    this.labelDiv = liTag.children("span:nth-child(2)");

    //前工程クラス
    this.prevIconDataClass = null;

    //後工程クラス
    this.nextIconDataClass = null;

    //自身のクラス (イベント処理で使用するため)
    var myClass = this;

    //プロセスアイコンのイメージを取得する
    var procIconImage = getIconImagePath(2, this.processIconDiv, this.processIconDiv);
    this.processIconDiv.children("div:nth-child(1)").css("background-image", procIconImage);

    //前工程、次工程セット
    this.setNestData = function (prev, next) {

        this.prevIconDataClass = prev;
        this.nextIconDataClass = next;
    }

    // 計画アイコンクリック中フラグ
    var iconClickDoEvent = false;

    // 計画アイコンクリック時 (日付指定ポップアップ表示) 
    planDataCls.iconDiv.click(function (e) {

        //実績日が入力されている場合は処理しない
        if (planDataCls.jissekiDate != "") {
            return;
        }

        //計画アイコンクリック中フラグを１秒間Trueにする
        //※まれに、計画アイコンクリック時に、工程リストクリックイベントが動作してしまうのを防ぐため
        iconClickDoEvent = true;
        setTimeout(function () {
            iconClickDoEvent = false;
        }, 800);

        //ポップアップ表示
        $("#scNscPlanTimeWindown").css("display", "block");
        popupDiv.append($("#scNscPlanTimeWindown"));

        //重なっている工程リストを取得
        var kouteiArray = new Array();
        kouteiArray = planDataCls.repeatPrpcessArray(kouteiArray, planDataCls.iconDateObj.val(), true);
        if (kouteiArray.length == 1) {

            //現在の時間保持
            perentCls.activePlanDataCls = myClass;

            //すべての工程を対象
            for (var i = 0; i < procClsArray.length; i++) {
                procCls = procClsArray[i];
                //現在の時間保持
                procCls.initTimeTemp();
            }

            //画面表示値セット
            perentCls.activePlanDataCls.setDisplayDateValue(planDataCls);

            //ポップアップの内容表示
            perentCls.DisplayPopup("1", "page2");

        } else {

            var procCls = null;
            var procClsTemp = null;

            //すべての工程を対象
            for (var i = 0; i < procClsArray.length; i++) {

                procCls = procClsArray[i];

                //非表示にする
                procCls.liTag.css("display", "none");
                procCls.liTag.removeClass("liFirst liLast");

                //現在の時間保持
                procCls.initTimeTemp();

                //日時ラベルを表示する
                procCls.setLiLabel();
            }

            //選択された工程を対象
            for (var i = 0; i < kouteiArray.length; i++) {

                for (var n = 0; n < procClsArray.length; n++) {
                    procClsTemp = procClsArray[n];
                    if (kouteiArray[i] == procClsTemp.planDataCls.iconProcess) {
                        //表示にする
                        procCls = procClsTemp;
                        procCls.liTag.css("display", "block");

                        //最初の行は、上線を表示する
                        if (i == 0) {
                            procCls.liTag.addClass("liFirst");
                        }
                        //最後の行は、下線を表示する
                        if (i == (kouteiArray.length - 1)) {
                            procCls.liTag.addClass("liLast");
                        }
                    }
                }
            }

            //ポップアップの内容表示
            perentCls.DisplayPopup("2", "page1");

        }

    });

    // 工程リスト選択 //
    liTag.click(function (e) {

        //計画アイコンクリック中フラグは動作しないようにする
        //※まれに、計画アイコンクリック時にこのイベントが動作してしまうのを防ぐため
        if (iconClickDoEvent == true) {
            iconClickDoEvent = false;
            return;
        }

        perentCls.activePlanDataCls = myClass;

        $("#startPlanDate").val(geHeifunDate(myClass.tempStartDate));
/* 2013/10/02 TCS 安田 次世代e-CRBタブレット iOS7.0 VersionUp対応（セールス） START */
        $("#startPlanTime").val(geDateTimelocalDate(myClass.tempStartDate));
        //$("#startPlanTime").get(0).valueAsDate = myClass.tempStartDate;
/* 2013/10/02 TCS 安田 次世代e-CRBタブレット iOS7.0 VersionUp対応（セールス） END */
        
        
        $("#endPlanTime").val(myClass.tempEndDate);

        //ページ遷移
        perentCls.movePlanPage("page2");
    });

    //入力日時チェック
    this.checkPlanDate = function (msgType) {

        var stdate = "";
        var sthhmm = "";
        var edhhmm = "";

        //実績入力済み
        if (this.planDataCls.jissekiDate != "") {
            return true;
        }

        if (this.tempEndDate != "") {
            stdate = this.tempStartDate;
            sthhmm = getHHMM(stdate);
            edhhmm = this.tempEndDate;
        } else {
            stdate = this.tempStartDate;
        }

        var yyyymmdd = getYYYYMMDD(stdate);

        //日付範囲チェック 前工程より小さい場合
        var prevDt = this.getPrevDate();
        if ((prevDt != "") && (yyyymmdd < prevDt)) {
            if (msgType == 1) {
                alert($("#checkPlanInputErrorHidden").val());
            } else {
                alert($("#beforePlanInputErrorHidden").val());
            }
            return false;
        }

        //日付範囲チェック 次工程より大きい場合
        var nextDt = this.getNextDate();
        if ((nextDt != "") && (yyyymmdd > nextDt)) {
            if (msgType == 1) {
                alert($("#checkPlanInputErrorHidden").val());
            } else {
                alert($("#afterPlanInputErrorHidden").val());
            }
            return false;
        }

        //時間でチェック
        //終了日時入力時（時間指定オン）	終了日時　＞　開始時間
        if (this.tempEndDate != "") {

            //前工程と同一日の場合は時間で比較する
            if (yyyymmdd == prevDt) {
                var prevEdhhmm = this.getPrevTime();
                if ((prevEdhhmm != "") && (sthhmm < prevEdhhmm)) {
                    if (msgType == 1) {
                        alert($("#checkPlanInputErrorHidden").val());
                    } else {
                        alert($("#beforePlanInputErrorHidden").val());
                    }
                    return false;
                }
            }

            //次工程と同一日の場合は時間で比較する
            if (yyyymmdd == nextDt) {
                var nextSthhmm = this.getNextTime();
                if ((nextSthhmm != "") && (edhhmm > nextSthhmm)) {
                    if (msgType == 1) {
                        alert($("#checkPlanInputErrorHidden").val());
                    } else {
                        alert($("#afterPlanInputErrorHidden").val());
                    }
                    return false;
                }
            }
        }
    }

    //テンポラリーに日時保持
    this.initTimeTemp = function () {
/* 2013/10/02 TCS 安田 次世代e-CRBタブレット iOS7.0 VersionUp対応（セールス） START */
        //this.tempStartDate = planDataCls.iconStTimeObj.get(0).valueAsDate;
        this.tempStartDate = changeStringToDateIcrop(planDataCls.iconStTimeObj.val());
/* 2013/10/02 TCS 安田 次世代e-CRBタブレット iOS7.0 VersionUp対応（セールス） END */
        this.tempEndDate = planDataCls.iconEdTimeObj.val();
    }

    //画面日時設定
    this.setDisplayDateValue = function () {
        $("#startPlanDate").val(geHeifunDate(this.tempStartDate));
/* 2013/10/02 TCS 安田 次世代e-CRBタブレット iOS7.0 VersionUp対応（セールス） START */
        $("#startPlanTime").val(geDateTimelocalDate(this.tempStartDate));
        //$("#startPlanTime").get(0).valueAsDate = this.tempStartDate;
/* 2013/10/02 TCS 安田 次世代e-CRBタブレット iOS7.0 VersionUp対応（セールス） END */
        $("#endPlanTime").val(this.tempEndDate);
    }

    //入力値をテンポラリーに保存する
    this.setTimeTemp = function () {

        if ($("#timeSelect").attr("checked")) {
/* 2013/10/02 TCS 安田 次世代e-CRBタブレット iOS7.0 VersionUp対応（セールス） START */
            this.tempStartDate = changeStringToDateIcrop($("#startPlanTime").val());
    //this.tempStartDate = $("#startPlanTime").get(0).valueAsDate;
/* 2013/10/02 TCS 安田 次世代e-CRBタブレット iOS7.0 VersionUp対応（セールス） END */
            this.tempEndDate = $("#endPlanTime").val();
        } else {
            this.tempStartDate = $("#startPlanDate").get(0).valueAsDate;
            this.tempEndDate = "";
        }
    }

    //Hidden項目に日付を設定（確定）する
    this.commotTimeTemp = function () {

/* 2013/10/02 TCS 安田 次世代e-CRBタブレット iOS7.0 VersionUp対応（セールス） START */
        planDataCls.iconStTimeObj.val(geDateTimelocalDate(this.tempStartDate));
        //planDataCls.iconStTimeObj.get(0).valueAsDate = this.tempStartDate;
/* 2013/10/02 TCS 安田 次世代e-CRBタブレット iOS7.0 VersionUp対応（セールス） END */
        
        planDataCls.iconEdTimeObj.val(this.tempEndDate);

        //画面値取得
/* 2013/10/02 TCS 安田 次世代e-CRBタブレット iOS7.0 VersionUp対応（セールス） START */
        var stdate = changeStringToDateIcrop(planDataCls.iconStTimeObj.val());
        //var stdate = planDataCls.iconStTimeObj.get(0).valueAsDate;
/* 2013/10/02 TCS 安田 次世代e-CRBタブレット iOS7.0 VersionUp対応（セールス） END */
        
        var yyyymmdd = getYYYYMMDD(stdate);

        //画面値取得
        planDataCls.iconDateObj.val(yyyymmdd);
    }

    //表示用日付文字列取得
    //fromDate  //Date型を想定
    //toTime    //String型を想定
    //timeType; //1:Fromのみ　2:From-To
    this.getLiDisplayDate = function (fromDate, toTime, timeType) {

        if (fromDate === null) {
            return "";
        }

        //時間文字列を作成
        function getTimeString(dateValue) {
            //HH:MM
            return dateValue !== null ? dateValue.getHours() + ":" + ("0" + dateValue.getMinutes()).slice(-2) : "";
        }

        //日付書式
        var Format = this_form.dateFormt.value;
        var dateString = "";
        var fromTime = "";     //Stringを想定

        if (timeType === 2) {
            fromTime = getTimeString(fromDate);
        }

        //月と日を書式化
        dateString = Format.replace("%3", fromDate.getDate());
        dateString = dateString.replace("%2", fromDate.getMonth() + 1);

        //開始時間
        dateString += " " + fromTime;

        if (timeType === 2) {
            //From-To
            /* 2012/03/27 TCS 高橋 【SALES_2】SalesStep2 ST 問題管理No.0076 START */
            //dateString += "-" + toTime;
            if (toTime.charAt(0) == "0") {
                dateString += "-" + toTime.substr(1, 4);
            }
            else {
                dateString += "-" + toTime;
            }
            /* 2012/03/27 TCS 高橋 【SALES_2】SalesStep2 ST 問題管理No.0076 END */
        }

        //作成した日付文字列返却
        return dateString;
    }

    //Liラベルの日付を表示する
    this.setLiLabel = function () {

        //２ページ目　計画日時
        var stdate;
        if (this.tempEndDate != "") {
            stdate = this.tempStartDate;
            eddate = this.tempEndDate;
            this.labelDiv.text(this.getLiDisplayDate(stdate, eddate, 2));
        } else {
            stdate = this.tempStartDate;
            this.labelDiv.text(this.getLiDisplayDate(stdate, "", 1));
        }
    }



    //前工程の日付を取得する
    this.getPrevDate = function () {

        //001:振当ならば受注日を変えす。
        if (this.planDataCls.iconProcess == "001") {
            return $("#D0Date").val();
        }

        if (this.planDataCls.prevIconDataClass == null) {
            //このパターンはないが念のために記載
            return "";
        } else {
            if (this.planDataCls.prevIconDataClass.jissekiDate != "") {
                //実績日が入っていれば実績日
                return this.planDataCls.prevIconDataClass.jissekiDate;
            } else {
                //前工程の日付
                return getYYYYMMDD(this.prevIconDataClass.tempStartDate);
            }
        }
    }

    //前工程の時間を取得する
    this.getPrevTime = function () {

        //001:振当
        if (this.planDataCls.iconProcess == "001") {
            return "";
        }

        if (this.planDataCls.prevIconDataClass == null) {
            //このパターンはないが念のために記載
            return "";
        } else {
            //前工程
            if (this.planDataCls.prevIconDataClass.jissekiDate != "") {
                //実績日が入っていれば時間指定なし
                return "";
            } else {
                //前工程の終了時間
                return this.prevIconDataClass.tempEndDate;
            }
        }
    }

    //次工程の日付を取得する
    this.getNextDate = function () {

        if (this.planDataCls.nextIconDataClass == null) {
            return "";
        } else {
            //次工程の日付
            return getYYYYMMDD(this.nextIconDataClass.tempStartDate);
        }
    }


    //次工程の時間を取得する
    this.getNextTime = function () {

        if (this.planDataCls.nextIconDataClass == null) {
            return "";
        } else {
            //次工程の終了時間
            if (this.nextIconDataClass.tempEndDate != "") {
                return getHHMM(this.nextIconDataClass.tempStartDate);
            } else {
                return "";
            }
        }
    }

}
