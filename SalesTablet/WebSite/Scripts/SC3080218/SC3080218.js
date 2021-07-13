/**
* @fileOverview Sc3080218 初期ロード時処理
*
* @author TCS 安田
* @version 1.0.0
*/

//━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
//SC3080218.js
//─────────────────────────────────────
//機能：顧客詳細(活動内容) 
//補足：
//作成： 2011/11/24 TCS 安田
//更新： 2012/07/31 TCS 河原 【A STEP2】次世代e-CRB 新車商談機能改善
//更新： 2013/01/18 TCS 坪根 【A.STEP2】次世代e-CRB 新車タブレットSC活動支援機能開発
//更新： 2013/10/02 TCS 安田 次世代e-CRBタブレット iOS7.0 VersionUp対応（セールス）
//─────────────────────────────────────

//登録ボタン押下時のイベントハンドラ登録
$(function () {
    SC3080201.addRegistEventHandlers(SC3080218SetDate);
});


/* 2013/10/02 TCS 安田 次世代e-CRBタブレット iOS7.0 VersionUp対応（セールス） START */
//文字列を日付型にする
function changeStringToDateIcrop(dateValue) {

    if (dateValue == null || dateValue == ""){
        return null;
    }
    
    var strDate = String(dateValue);
    strDate = strDate.replace(/-/g, '/');
    strDate = strDate.replace('T', ' ');
    
    return new Date(Date.parse(strDate));
}
/* 2013/10/02 TCS 安田 次世代e-CRBタブレット iOS7.0 VersionUp対応（セールス） END */


/****************************************************************

初期ロード

*****************************************************************/
$(function () {
    //スクロール化
    $(".scNscStaffListBox").fingerScroll();
    $(".scNscActContactListBox").fingerScroll();
    $(".scNscTestDriveListBox").fingerScroll();
    $(".scNscCatalogListBox").fingerScroll();
    $(".scNscValuationListBox").fingerScroll();

    //活動日(From)の復元
    if (this_form.Sc3080218ActTimeFromSelectorWK.value != "") {
        $("#Sc3080218ActTimeFromSelector").val(this_form.Sc3080218ActTimeFromSelectorWK.value)
    }

    //活動日(To)の復元
    if (this_form.Sc3080218ActTimeToSelectorWK.value != "") {
        $("#Sc3080218ActTimeToSelector").val(this_form.Sc3080218ActTimeToSelectorWK.value)
    }

    //活動日付表示
    //2012/07/31 TCS 河原 【A STEP2】次世代e-CRB 新車商談機能改善 START
    if ($("#SC3080218UpdateRWFlg").attr("value") == "0" || $("#Sc3080218ActTimeToSelectorWK2").attr("value") != "") {
        $(".ActTime").text(getDisplayDate218WK("ActTime"));
        this_form.SC3080218UpdateRWFlg.value = "0"
    } else {
        $(".ActTime").text(getInitDisplayDate218WK("ActTime"));
    }

    //2012/07/31 TCS 河原 【A STEP2】次世代e-CRB 新車商談機能改善 END

    //対応SC表示
    var listname;
    var listvalue;
    listname = "#Sc3080218Stafflist";
    listvalue = this_form.Sc3080218selectStaff.value;
    if ($(listname + listvalue).size() > 0) {
        $(listname + listvalue).addClass("Selection");
    }
    $(".scNscStaffName").text(this_form.Sc3080218selectStaffName.value)

    //分類表示
    listname = "#Sc3080218ActContactlist"
    listvalue = this_form.Sc3080218selectActContact.value
    if ($(listname + listvalue).size() > 0) {
        $(listname + listvalue).addClass("Selection")
    }
    $(".scNscActContactName").text(this_form.Sc3080218selectActContactTitle.value)

    //プロセス欄の非表示化
    var BookedFlg = this_form.Sc3080218BookedFlg.value;
    if (this_form.Sc3080218ProcessFlg.value == "1" && BookedFlg == "0") {
        $("#confirmContents218 .BoxtypeNSC61A .nscListBoxSetRight2 ").animate({ width: "show" }, 0);
    }
    else {
        $("#confirmContents218 .BoxtypeNSC61A .nscListBoxSetRight2 ").animate({ width: "hide" }, 0);
    }

    //カタログ選択内容の復元
    this_form.Sc3080218selectActCatalogWK.value = this_form.Sc3080218selectActCatalog.value;
    var listname = "#Cataloglist";
    var listvalue;
    var i;
    var seledary
    var seledarydetail
    var seled = this_form.Sc3080218selectActCatalog.value;
    var selfalg = "0";
    seledary = seled.split(";");
    for (i = 0; i < seledary.length - 1; i++) {
        seledarydetail = seledary[i].split(",");
        listvalue = seledarydetail[0];
        if (seledarydetail[1] == "1") {
            $(listname + listvalue).addClass("Selection")
            selfalg = "1";
        }
        else {
            $(listname + listvalue).removeClass("Selection")
        }
    }
    if (selfalg == "1") {
        $("#confirmContents218 .BoxtypeNSC61A .nscListBoxSetRight2 .nscListIcnAset .nscListIcnA1").css("text-shadow", "0px -1px 1px #000");
        $("#confirmContents218 .BoxtypeNSC61A .nscListBoxSetRight2 .nscListIcnAset .nscListIcnA1").css("color", "#FFF");
        $("#confirmContents218 .BoxtypeNSC61A .nscListBoxSetRight2 .nscListIcnAset .nscListIcnA1").css("text-shadow", "none");
        $("#confirmContents218 .BoxtypeNSC61A .nscListBoxSetRight2 .nscListIcnAset .nscListIcnA1").css("background", "url(" + this_form.Sc3080218CatalogSelPath.value + ") center 9px no-repeat,-webkit-gradient(linear, left top, left bottom,color-stop(0, #7b97ed),color-stop(50%, #416ae5),color-stop(51%, #1e4ee1),color-stop(100%, #1e4ee1))");
        this_form.Sc3080218HD_nscListIcnA1.value = "1";
    } else {
        $("#confirmContents218 .BoxtypeNSC61A .nscListBoxSetRight2 .nscListIcnAset .nscListIcnA1").css("text-shadow", "0px -1px 1px #FFF");
        $("#confirmContents218 .BoxtypeNSC61A .nscListBoxSetRight2 .nscListIcnAset .nscListIcnA1").css("color", "#808080");
        $("#confirmContents218 .BoxtypeNSC61A .nscListBoxSetRight2 .nscListIcnAset .nscListIcnA1").css("text-shadow", "block");
        $("#confirmContents218 .BoxtypeNSC61A .nscListBoxSetRight2 .nscListIcnAset .nscListIcnA1").css("background", "url(" + this_form.Sc3080218CatalogNonSelPath.value + ") center 9px no-repeat,-webkit-gradient(linear, left top, left bottom,color-stop(0, #fbfbfb),color-stop(100%, #cacaca))");
        this_form.Sc3080218HD_nscListIcnA1.value = "0";
    }

    //試乗内容の復元
    this_form.Sc3080218selectActTestDriveWK.value = this_form.Sc3080218selectActTestDrive.value;
    var listname = "#TestDrivelist";
    var listvalue;
    var i;
    var seledary
    var seledarydetail
    var seled = this_form.Sc3080218selectActTestDrive.value;
    var selfalg = "0";
    seledary = seled.split(";");
    for (i = 0; i < seledary.length - 1; i++) {
        seledarydetail = seledary[i].split(",");
        listvalue = seledarydetail[0];
        if (seledarydetail[1] == "1") {
            $(listname + listvalue).addClass("Selection")
            selfalg = "1";
        }
        else {
            $(listname + listvalue).removeClass("Selection")
        }
    }
    if (selfalg == "1") {
        $("#confirmContents218 .BoxtypeNSC61A .nscListBoxSetRight2 .nscListIcnAset .nscListIcnA2").css("text-shadow", "0px -1px 1px #000");
        $("#confirmContents218 .BoxtypeNSC61A .nscListBoxSetRight2 .nscListIcnAset .nscListIcnA2").css("color", "#FFF");
        $("#confirmContents218 .BoxtypeNSC61A .nscListBoxSetRight2 .nscListIcnAset .nscListIcnA2").css("text-shadow", "none");
        $("#confirmContents218 .BoxtypeNSC61A .nscListBoxSetRight2 .nscListIcnAset .nscListIcnA2").css("background", "url(" + this_form.Sc3080218TestDriveSelPath.value + ") center 9px no-repeat,-webkit-gradient(linear, left top, left bottom,color-stop(0, #7b97ed),color-stop(50%, #416ae5),color-stop(51%, #1e4ee1),color-stop(100%, #1e4ee1))");
        this_form.Sc3080218HD_nscListIcnA1.value = "1";
    } else {
        $("#confirmContents218 .BoxtypeNSC61A .nscListBoxSetRight2 .nscListIcnAset .nscListIcnA2").css("text-shadow", "0px -1px 1px #FFF");
        $("#confirmContents218 .BoxtypeNSC61A .nscListBoxSetRight2 .nscListIcnAset .nscListIcnA2").css("color", "#808080");
        $("#confirmContents218 .BoxtypeNSC61A .nscListBoxSetRight2 .nscListIcnAset .nscListIcnA2").css("text-shadow", "block");
        $("#confirmContents218 .BoxtypeNSC61A .nscListBoxSetRight2 .nscListIcnAset .nscListIcnA2").css("background", "url(" + this_form.Sc3080218TestDriveNonSelPath.value + ") center 9px no-repeat,-webkit-gradient(linear, left top, left bottom,color-stop(0, #fbfbfb),color-stop(100%, #cacaca))");
        this_form.Sc3080218HD_nscListIcnA1.value = "0";
    }

    /* 2013/01/18 TCS 坪根 【A.STEP2】次世代e-CRB 新車タブレットSC活動支援機能開発 START */
    //査定内容の復元
    //    if (this_form.Sc3080218selectActAssesment.value == "1") {
    //        $("#confirmContents218 .BoxtypeNSC61A .nscListBoxSetRight2 .nscListIcnAset .nscListIcnA3").css("text-shadow", "0px -1px 1px #000");
    //        $("#confirmContents218 .BoxtypeNSC61A .nscListBoxSetRight2 .nscListIcnAset .nscListIcnA3").css("color", "#FFF");
    //        $("#confirmContents218 .BoxtypeNSC61A .nscListBoxSetRight2 .nscListIcnAset .nscListIcnA3").css("text-shadow", "none");
    //        $("#confirmContents218 .BoxtypeNSC61A .nscListBoxSetRight2 .nscListIcnAset .nscListIcnA3").css("background", "url(" + this_form.Sc3080218AssesmentSelPath.value + ") center 9px no-repeat,-webkit-gradient(linear, left top, left bottom,color-stop(0, #7b97ed),color-stop(50%, #416ae5),color-stop(51%, #1e4ee1),color-stop(100%, #1e4ee1))");
    //    } else {
    //        $("#confirmContents218 .BoxtypeNSC61A .nscListBoxSetRight2 .nscListIcnAset .nscListIcnA3").css("text-shadow", "0px -1px 1px #FFF");
    //        $("#confirmContents218 .BoxtypeNSC61A .nscListBoxSetRight2 .nscListIcnAset .nscListIcnA3").css("color", "#808080");
    //        $("#confirmContents218 .BoxtypeNSC61A .nscListBoxSetRight2 .nscListIcnAset .nscListIcnA3").css("text-shadow", "block");
    //        $("#confirmContents218 .BoxtypeNSC61A .nscListBoxSetRight2 .nscListIcnAset .nscListIcnA3").css("background", "url(" + this_form.Sc3080218AssesmentNonSelPath.value + ") center 9px no-repeat,-webkit-gradient(linear, left top, left bottom,color-stop(0, #fbfbfb),color-stop(100%, #cacaca))");
    //    }

    Sc3080218ActAssesmentButtonOnOff(this_form.Sc3080218selectActAssesment.value);
    /* 2013/01/18 TCS 坪根 【A.STEP2】次世代e-CRB 新車タブレットSC活動支援機能開発 END */

    //見積り内容の復元
    this_form.Sc3080218selectActValuationWK.value = this_form.Sc3080218selectActValuation.value;
    var listname = "#Valuationlist";
    var listvalue;
    var i;
    var seledary
    var seledarydetail
    var seled = this_form.Sc3080218selectActValuation.value;
    var selfalg = "0";
    seledary = seled.split(";");
    for (i = 0; i < seledary.length - 1; i++) {
        seledarydetail = seledary[i].split(",");

        /* 2013/01/18 TCS 坪根 【A.STEP2】次世代e-CRB 新車タブレットSC活動支援機能開発 START */
        //見積車両を出力する為、コントロールIDは見積管理IDで見る
        //listvalue = seledarydetail[0];
        listvalue = seledarydetail[4];
        /* 2013/01/18 TCS 坪根 【A.STEP2】次世代e-CRB 新車タブレットSC活動支援機能開発 END */

        if (seledarydetail[1] == "1") {
            $(listname + listvalue).addClass("Selection")
            selfalg = "1";
        }
        else {
            $(listname + listvalue).removeClass("Selection")
        }
    }

    if (selfalg == "1") {
        $("#confirmContents218 .BoxtypeNSC61A .nscListBoxSetRight2 .nscListIcnAset .nscListIcnA4").css("text-shadow", "0px -1px 1px #000");
        $("#confirmContents218 .BoxtypeNSC61A .nscListBoxSetRight2 .nscListIcnAset .nscListIcnA4").css("color", "#FFF");
        $("#confirmContents218 .BoxtypeNSC61A .nscListBoxSetRight2 .nscListIcnAset .nscListIcnA4").css("text-shadow", "none");
        $("#confirmContents218 .BoxtypeNSC61A .nscListBoxSetRight2 .nscListIcnAset .nscListIcnA4").css("background", "url(" + this_form.Sc3080218ValuationSelPath.value + ") center 9px no-repeat,-webkit-gradient(linear, left top, left bottom,color-stop(0, #7b97ed),color-stop(50%, #416ae5),color-stop(51%, #1e4ee1),color-stop(100%, #1e4ee1))");
        this_form.Sc3080218HD_nscListIcnA1.value = "1";
    } else {
        $("#confirmContents218 .BoxtypeNSC61A .nscListBoxSetRight2 .nscListIcnAset .nscListIcnA4").css("text-shadow", "0px -1px 1px #FFF");
        $("#confirmContents218 .BoxtypeNSC61A .nscListBoxSetRight2 .nscListIcnAset .nscListIcnA4").css("color", "#808080");
        $("#confirmContents218 .BoxtypeNSC61A .nscListBoxSetRight2 .nscListIcnAset .nscListIcnA4").css("text-shadow", "block");
        $("#confirmContents218 .BoxtypeNSC61A .nscListBoxSetRight2 .nscListIcnAset .nscListIcnA4").css("background", "url(" + this_form.Sc3080218ValuationNonSelPath.value + ") center 9px no-repeat,-webkit-gradient(linear, left top, left bottom,color-stop(0, #fbfbfb),color-stop(100%, #cacaca))");
        this_form.Sc3080218HD_nscListIcnA1.value = "0";
    }

    //ポップアップ表示時にスタイルを調整する
    $("#Sc3080218ActTimePopupTrigger").click(function () {
        setPopupIniit($("#Sc3080218ActTimePopOver_content"));
    });
    $("#Sc3080218UsersTrigger").click(function () {
        setPopupIniit($("#Sc3080218PopOver8_content"));
    });
    $("#Sc3080218ActContactTrigger").click(function () {
        setPopupIniit($("#Sc3080218PopOver9_content"));
    });
    $("#Sc3080218popupTrigger4").click(function () {
        setPopupIniit($("#Sc3080218PopOver4_content"));
    });
    $("#Sc3080218popupTrigger5").click(function () {
        setPopupIniit($("#Sc3080218PopOver5_content"));
    });
    $("#Sc3080218popupTrigger6").click(function () {
        setPopupIniit($("#Sc3080218PopOver6_content"));
    });

});

//ポップアップ表示時にスタイルを調整する
function setPopupIniit(contentTag) {
    contentTag.parents(".popover").css("border", "0px solid black");
    contentTag.parents(".popover").css("background", "Transparent");
    contentTag.parents(".popover").css("box-shadow", "None");
    contentTag.parents(".popover").find(".content").css("padding", "0px");
    contentTag.parents(".popover").find(".content").css("margin", "0px");
    contentTag.parents(".popover").find(".content").css("background", "Transparent");
    contentTag.parents(".popover").find(".content").css("border", "none");
}

/****************************************************************

今回活動日時のポップアップ動作

*****************************************************************/
$(function () {
/* 2013/10/02 TCS 安田 次世代e-CRBタブレット iOS7.0 VersionUp対応（セールス） START */
    //活動日時のコミット値保存
    $("#Sc3080218ActTimeFromSelector").each(function () {
        this.commitValue = this.value;
    });
    
    $("#Sc3080218ActTimeToSelector").each(function () {
        this.commitValue = this.valueAsDate;
    });
/* 2013/10/02 TCS 安田 次世代e-CRBタブレット iOS7.0 VersionUp対応（セールス） END */
    
    //今回活動時間ポップアップ完了ボタン
    $(".scNscActTimeCompletionButton").click(function () {
        //活動日(From)の必須チェック
/* 2013/10/02 TCS 安田 次世代e-CRBタブレット iOS7.0 VersionUp対応（セールス） START */
        if ($("#Sc3080218ActTimeFromSelector").val() == "") {
        //if ($("#Sc3080218ActTimeFromSelector").get(0).valueAsDate == null) {
/* 2013/10/02 TCS 安田 次世代e-CRBタブレット iOS7.0 VersionUp対応（セールス） END */
            alert(this_form.Sc3080218ErrWord1.value)
            return ;
        }
        //活動日(To)の必須チェック
        if ($("#Sc3080218ActTimeToSelector").get(0).valueAsDate == null) {
            alert(this_form.Sc3080218ErrWord2.value)
            return;
        }
        
/* 2013/10/02 TCS 安田 次世代e-CRBタブレット iOS7.0 VersionUp対応（セールス） START */
        //活動日時のコミット値保存
        $("#Sc3080218ActTimeFromSelector").each(function () {
            this.commitValue = this.value;
        });
        //活動日時のコミット値保存
        $("#Sc3080218ActTimeToSelector").each(function () {
            this.commitValue = this.valueAsDate;
        });
/* 2013/10/02 TCS 安田 次世代e-CRBタブレット iOS7.0 VersionUp対応（セールス） END */
        
        //表示用日付文字列設定
        $(".ActTime").text(getDisplayDate218("ActTime"));
        
        //ポップアップを閉じる
        $("#bodyFrame").trigger("click.popover");
    });
    
    //今回活動時間ポップアップキャンセルボタン
    $(".scNscActTimeCancellButton").click(function () {
        rollbackTime();
        $("#bodyFrame").trigger("click.popover");
    });
    
    //クローズ処理
    $("#bodyFrame").bind("click", function (e) {
        rollbackTime();
    });
    
    //値をポップアップ表示前に戻す
    function rollbackTime() {
/* 2013/10/02 TCS 安田 次世代e-CRBタブレット iOS7.0 VersionUp対応（セールス） START */
        //活動日用のポップアップ
        $("#Sc3080218ActTimeFromSelector").each(function () {
            this.value = this.commitValue;
        });
        $("#Sc3080218ActTimeToSelector").each(function () {
            this.valueAsDate = this.commitValue;
        });
/* 2013/10/02 TCS 安田 次世代e-CRBタブレット iOS7.0 VersionUp対応（セールス） END */
    }
});

//表示用日付文字列取得
function getDisplayDate218(area) {
    
    var fromDate = null;   //Dateを想定
    var fromTime = "";     //Stringを想定
    var toTime = "";       //Stringを想定
    
    //時間文字列を作成
    function getTimeString(dateValue) {
        //HH:MM
        return dateValue !== null ? dateValue.getHours() + ":" + ("0" + dateValue.getMinutes()).slice(-2) : "";
    }
    
    var timeType; //1:Fromのみ　2:From-To
    
    //日付書式
    var Format = this_form.Sc3080218dateFormt.value;
    var dateString = "";
    
    //活動時間
    if (area === "ActTime") {
        timeType = "2";
/* 2013/10/02 TCS 安田 次世代e-CRBタブレット iOS7.0 VersionUp対応（セールス） START */
        fromDate = changeStringToDateIcrop($("#Sc3080218ActTimeFromSelector").val());
        //fromDate = $("#Sc3080218ActTimeFromSelector").get(0).valueAsDate;
/* 2013/10/02 TCS 安田 次世代e-CRBタブレット iOS7.0 VersionUp対応（セールス） END */
        fromTime = getTimeString(fromDate);
        toTime = $("#Sc3080218ActTimeToSelector").val();
        if (toTime.charAt(0) == "0") {
            toTime = toTime.substr(1,4)
        }
    }
    
    //月と日を書式化
    if (fromDate !== null) {
        dateString = Format.replace("%3", fromDate.getDate());
        dateString = dateString.replace("%2", fromDate.getMonth() + 1);
    }
    //開始時間
    dateString += " " + fromTime;
    
    if (timeType === "2") {
        //From-To
        dateString += "-" + toTime;
    }
    
    //作成した日付文字列返却
    return dateString;
}

//表示用日付文字列取得(WK用)
function getDisplayDate218WK(area) {
    var fromDate = null;   //Dateを想定
    var fromTime = "";     //Stringを想定
    var toTime = "";       //Stringを想定
    //時間文字列を作成
    function getTimeString(dateValue) {
        //HH:MM
        return dateValue !== null ? dateValue.getHours() + ":" + ("0" + dateValue.getMinutes()).slice(-2) : "";
    }
    var timeType; //1:Fromのみ　2:From-To
    //日付書式
    var Format = this_form.Sc3080218dateFormt.value;
    var dateString = "";
    //活動時間
    if (area === "ActTime") {
        timeType = "2";
        //fromDate = $("#Sc3080218ActTimeFromSelectorWK2").get(0).valueAsDate;
        
/* 2013/10/02 TCS 安田 次世代e-CRBタブレット iOS7.0 VersionUp対応（セールス） START */
        fromDate = changeStringToDateIcrop($("#Sc3080218ActTimeFromSelectorWK2").val());
        //fromDate = $("#Sc3080218ActTimeFromSelectorWK2").get(0).valueAsDate;
/* 2013/10/02 TCS 安田 次世代e-CRBタブレット iOS7.0 VersionUp対応（セールス） END */

        fromTime = getTimeString(fromDate);
        toTime = $("#Sc3080218ActTimeToSelectorWK2").val();
        if (toTime.charAt(0) == "0") {
            toTime = toTime.substr(1, 4);
        }
    }
    //月と日を書式化
    if (fromDate !== null) {
        dateString = Format.replace("%3", fromDate.getDate());
        dateString = dateString.replace("%2", fromDate.getMonth() + 1);
    }
    //開始時間
    dateString += " " + fromTime;
    if (timeType === "2") {
        //From-To
        dateString += "-" + toTime;
    }
    //作成した日付文字列返却
    return dateString;
}

//2012/07/31 TCS 河原 【A STEP2】次世代e-CRB 新車商談機能改善 START
//表示用日付文字列取得(WK用)
function getInitDisplayDate218WK(area) {
    var fromDate = null;   //Dateを想定
    var fromTime = "";     //Stringを想定
    var toTime = "";       //Stringを想定
    //時間文字列を作成
    function getTimeString(dateValue) {
        //HH:MM
        return dateValue !== null ? dateValue.getHours() + ":" + ("0" + dateValue.getMinutes()).slice(-2) : "";
    }
    var timeType; //1:Fromのみ　2:From-To
    //日付書式
    var Format = this_form.Sc3080218dateFormt.value;
    var dateString = "";
    //活動時間
    if (area === "ActTime") {
        timeType = "2";
/* 2013/10/02 TCS 安田 次世代e-CRBタブレット iOS7.0 VersionUp対応（セールス） START */
        fromDate = changeStringToDateIcrop($("#Sc3080218ActTimeFromSelectorWK2").val());
        //fromDate = $("#Sc3080218ActTimeFromSelectorWK2").get(0).valueAsDate;
/* 2013/10/02 TCS 安田 次世代e-CRBタブレット iOS7.0 VersionUp対応（セールス） END */

        
        fromTime = getTimeString(fromDate);
    }
    //月と日を書式化
    if (fromDate !== null) {
        dateString = Format.replace("%3", fromDate.getDate());
        dateString = dateString.replace("%2", fromDate.getMonth() + 1);
    }
    //開始時間
    dateString += " " + fromTime;
    if (timeType === "2") {
        //From-To
        dateString += "-" + toTime;
    }
    //作成した日付文字列返却
    return dateString;
}
//2012/07/31 TCS 河原 【A STEP2】次世代e-CRB 新車商談機能改善 END

// 対応SC欄 //
$(function () {
    $(".Stafflist").live("click",function (e) {
        $(".scNscStaffName").html($(this).html());
        $(".Stafflist").removeClass("Selection");
        $(this).addClass("Selection");
        this_form.Sc3080218selectStaff.value = $(this).children("span").attr("value");
        this_form.Sc3080218selectStaffName.value = $(this).attr("title");
        $("#bodyFrame").trigger("click.popover");
    });
    $(".scNscStaffCancellButton").click(function () {
        $("#bodyFrame").trigger("click.popover");
    });
});

// 分類欄(活動内容) //
$(function () {
    $(".ActContactlist").live("click", function (e) {
        //受注後フラグ(受注後であればプロセス欄は常に非表示)
        var BookedFlg = this_form.Sc3080218BookedFlg.value;
        if ($(this).children("span").attr("value") == "1" && BookedFlg == "0") {
            $("#confirmContents218 .BoxtypeNSC61A .nscListBoxSetRight2 ").animate({ width: "show" }, 300);
            this_form.Sc3080218ProcessFlg.value = "1"
        }
        else {
            $("#confirmContents218 .BoxtypeNSC61A .nscListBoxSetRight2 ").animate({ width: "hide" }, 300);
            this_form.Sc3080218ProcessFlg.value = "0"
        }
        $(".scNscActContactName").html($(this).html());
        $(".ActContactlist").removeClass("Selection");
        $(this).addClass("Selection");
        this_form.Sc3080218selectActContact.value = $(this).attr("value");
        this_form.Sc3080218selectActContactTitle.value = $(this).attr("title");
        $("#bodyFrame").trigger("click.popover");
    });
    $(".scNscActContactCancellButton").click(function () {
        $("#bodyFrame").trigger("click.popover");
    });
});

// カタログ //
$(function () {
    $(".Cataloglist").live("click", function (e) {
        var listname = "#Sc3080218Cataloglist";
        var listvalue;
        var i;
        var j;
        var sel = $(this).attr("id");
        var seledary
        var seledarydetail
        var seledarycreate = ""
        sel = sel.replace("Sc3080218Cataloglist", "");
        var seled = $("#Sc3080218selectActCatalogWK").attr("value");
        seledary = seled.split(";");
        for (i = 0; i < seledary.length - 1; i++) {
            seledarydetail = seledary[i].split(",");
            if (seledarydetail[0] == sel) {
                if (seledarydetail[1] == "0") {
                    seledarydetail[1] = "1";
                    listvalue = seledarydetail[0]
                    $(listname + listvalue).addClass("Selection")
                }
                else if (seledarydetail[1] == "1") {
                    seledarydetail[1] = "0";
                    listvalue = seledarydetail[0]
                    $(listname + listvalue).removeClass("Selection")
                }
            }
            seledary[i] = seledarydetail[0] + "," + seledarydetail[1];
            seledarycreate = seledarycreate + seledary[i] + ";";
        }
        $("#Sc3080218selectActCatalogWK").val(seledarycreate)
    });
    $(".scNscCatalogCancellButton").click(function () {
        $("#bodyFrame").trigger("click.popover");
    });
    /* 2012/03/15 TCS 高橋 【SALES_2】(Sales1A ユーザテスト No.222) START */
    //関数化
    $(".scNscCatalogCompletionButton").click(function () {
//      $("#Sc3080218selectActCatalog").val($("#Sc3080218selectActCatalogWK").attr("value"));
//      var selfalg = "0";
//      var seled = $("#Sc3080218selectActCatalog").attr("value");
//      var seledary = seled.split(";");
//      for (i = 0; i < seledary.length - 1; i++) {
//          seledarydetail = seledary[i].split(",");
//          if (seledarydetail[1] == "1") {
//              selfalg = "1";
//          }
//      }
//      if (selfalg == "1") {
//          $("#confirmContents218 .BoxtypeNSC61A .nscListBoxSetRight2 .nscListIcnAset .nscListIcnA1").css("text-shadow", "0px -1px 1px #000");
//          $("#confirmContents218 .BoxtypeNSC61A .nscListBoxSetRight2 .nscListIcnAset .nscListIcnA1").css("color", "#FFF");
//          $("#confirmContents218 .BoxtypeNSC61A .nscListBoxSetRight2 .nscListIcnAset .nscListIcnA1").css("text-shadow", "none");
//          $("#confirmContents218 .BoxtypeNSC61A .nscListBoxSetRight2 .nscListIcnAset .nscListIcnA1").css("background", "url(" + this_form.Sc3080218CatalogSelPath.value + ") center 9px no-repeat,-webkit-gradient(linear, left top, left bottom,color-stop(0, #7b97ed),color-stop(50%, #416ae5),color-stop(51%, #1e4ee1),color-stop(100%, #1e4ee1))");
//          this_form.Sc3080218HD_nscListIcnA1.value = "1";
//      } else {
//          $("#confirmContents218 .BoxtypeNSC61A .nscListBoxSetRight2 .nscListIcnAset .nscListIcnA1").css("text-shadow", "0px -1px 1px #FFF");
//          $("#confirmContents218 .BoxtypeNSC61A .nscListBoxSetRight2 .nscListIcnAset .nscListIcnA1").css("color", "#808080");
//          $("#confirmContents218 .BoxtypeNSC61A .nscListBoxSetRight2 .nscListIcnAset .nscListIcnA1").css("text-shadow", "block");
//          $("#confirmContents218 .BoxtypeNSC61A .nscListBoxSetRight2 .nscListIcnAset .nscListIcnA1").css("background", "url(" + this_form.Sc3080218CatalogNonSelPath.value + ") center 9px no-repeat,-webkit-gradient(linear, left top, left bottom,color-stop(0, #fbfbfb),color-stop(100%, #cacaca))");
//          this_form.Sc3080218HD_nscListIcnA1.value = "0";
//      }
//      $("#bodyFrame").trigger("click.popover");
        SetSc3080218ActCatalogButton();
    });
    /* 2012/03/15 TCS 高橋 【SALES_2】(Sales1A ユーザテスト No.222) END */
});
$(function () {
    $("#confirmContents218 .BoxtypeNSC61A .nscListBoxSetRight2 .nscListIcnAset .nscListIcnA1").click(function () {
        $("#Sc3080218selectActCatalogWK").val($("#Sc3080218selectActCatalog").attr("value"));
        var listname = "#Sc3080218Cataloglist";
        var listvalue;
        var i;
        var seledary
        var seledarydetail
        var seled = $("#Sc3080218selectActCatalog").attr("value");
        seledary = seled.split(";");
        for (i = 0; i < seledary.length - 1; i++) {
            seledarydetail = seledary[i].split(",");
            listvalue = seledarydetail[0];
            if (seledarydetail[1] == "1") {
                $(listname + listvalue).addClass("Selection")
            }
            else {
                $(listname + listvalue).removeClass("Selection")
            }
        }
    }
    );
});

// 試乗 //
$(function () {
    $(".TestDrivelist").live("click", function (e) {
        var listname = "#Sc3080218TestDrivelist";
        var listvalue;
        var i;
        var j;
        var sel = $(this).attr("id");
        var seledary
        var seledarydetail
        var seledarycreate = ""
        sel = sel.replace("Sc3080218TestDrivelist", "");
        //var seled = this_form.Sc3080218selectActTestDriveWK.value;
        var seled = $("#Sc3080218selectActTestDriveWK").attr("value");
        seledary = seled.split(";");
        for (i = 0; i < seledary.length - 1; i++) {
            seledarydetail = seledary[i].split(",");
            if (seledarydetail[0] == sel) {
                if (seledarydetail[1] == "0") {
                    seledarydetail[1] = "1";
                    listvalue = seledarydetail[0]
                    $(listname + listvalue).addClass("Selection")
                }
                else if (seledarydetail[1] == "1") {
                    seledarydetail[1] = "0";
                    listvalue = seledarydetail[0]
                    $(listname + listvalue).removeClass("Selection")
                }
            }
            seledary[i] = seledarydetail[0] + "," + seledarydetail[1];
            seledarycreate = seledarycreate + seledary[i] + ";";
        }
        //this_form.Sc3080218selectActTestDriveWK.value = seledarycreate;
        $("#Sc3080218selectActTestDriveWK").val(seledarycreate)
    });
    $(".scNscTestDriveCancellButton").click(function () {
        $("#bodyFrame").trigger("click.popover");
    });
    /* 2012/03/15 TCS 高橋 【SALES_2】(Sales1A ユーザテスト No.222) START */
    //関数化
    $(".scNscTestDriveCompletionButton").click(function () {
//      //this_form.Sc3080218selectActTestDrive.value = this_form.Sc3080218selectActTestDriveWK.value;
//      $("#Sc3080218selectActTestDrive").val($("#Sc3080218selectActTestDriveWK").attr("value"));
//      var selfalg = "0";
//      //var seled = this_form.Sc3080218selectActTestDrive.value;
//      var seled = $("#Sc3080218selectActTestDrive").attr("value");
//      var seledary = seled.split(";");
//      for (i = 0; i < seledary.length - 1; i++) {
//          seledarydetail = seledary[i].split(",");
//          if (seledarydetail[1] == "1") {
//              selfalg = "1";
//          }
//      }
//      if (selfalg == "1") {
//          $("#confirmContents218 .BoxtypeNSC61A .nscListBoxSetRight2 .nscListIcnAset .nscListIcnA2").css("text-shadow", "0px -1px 1px #000");
//          $("#confirmContents218 .BoxtypeNSC61A .nscListBoxSetRight2 .nscListIcnAset .nscListIcnA2").css("color", "#FFF");
//          $("#confirmContents218 .BoxtypeNSC61A .nscListBoxSetRight2 .nscListIcnAset .nscListIcnA2").css("text-shadow", "none");
//          $("#confirmContents218 .BoxtypeNSC61A .nscListBoxSetRight2 .nscListIcnAset .nscListIcnA2").css("background", "url(" + this_form.Sc3080218TestDriveSelPath.value + ") center 9px no-repeat,-webkit-gradient(linear, left top, left bottom,color-stop(0, #7b97ed),color-stop(50%, #416ae5),color-stop(51%, #1e4ee1),color-stop(100%, #1e4ee1))");
//          this_form.Sc3080218HD_nscListIcnA1.value = "1";
//      } else {
//          $("#confirmContents218 .BoxtypeNSC61A .nscListBoxSetRight2 .nscListIcnAset .nscListIcnA2").css("text-shadow", "0px -1px 1px #FFF");
//          $("#confirmContents218 .BoxtypeNSC61A .nscListBoxSetRight2 .nscListIcnAset .nscListIcnA2").css("color", "#808080");
//          $("#confirmContents218 .BoxtypeNSC61A .nscListBoxSetRight2 .nscListIcnAset .nscListIcnA2").css("text-shadow", "block");
//          $("#confirmContents218 .BoxtypeNSC61A .nscListBoxSetRight2 .nscListIcnAset .nscListIcnA2").css("background", "url(" + this_form.Sc3080218TestDriveNonSelPath.value + ") center 9px no-repeat,-webkit-gradient(linear, left top, left bottom,color-stop(0, #fbfbfb),color-stop(100%, #cacaca))");
//          this_form.Sc3080218HD_nscListIcnA1.value = "0";
//      }
//      $("#bodyFrame").trigger("click.popover");
        SetSc3080218ActTestDriveButton();
    });
    /* 2012/03/15 TCS 高橋 【SALES_2】(Sales1A ユーザテスト No.222) END */
});
$(function () {
    $("#confirmContents218 .BoxtypeNSC61A .nscListBoxSetRight2 .nscListIcnAset .nscListIcnA2").click(function () {
        //this_form.Sc3080218selectActTestDriveWK.value = this_form.Sc3080218selectActTestDrive.value;
        $("#Sc3080218selectActTestDriveWK").val($("#Sc3080218selectActTestDrive").attr("value"));
        var listname = "#Sc3080218TestDrivelist";
        var listvalue;
        var i;
        var seledary
        var seledarydetail
        //var seled = this_form.Sc3080218selectActTestDrive.value;
        var seled = $("#Sc3080218selectActTestDrive").attr("value");
        seledary = seled.split(";");
        for (i = 0; i < seledary.length - 1; i++) {
            seledarydetail = seledary[i].split(",");
            listvalue = seledarydetail[0];
            if (seledarydetail[1] == "1") {
                $(listname + listvalue).addClass("Selection")
            }
            else {
                $(listname + listvalue).removeClass("Selection")
            }
        }
    }
    );
});

// 査定ボタン //
$(function () {
    $("#confirmContents218 .BoxtypeNSC61A .nscListBoxSetRight2 .nscListIcnAset .nscListIcnA3").live("click", function () {
        /* 2013/01/18 TCS 坪根 【A.STEP2】次世代e-CRB 新車タブレットSC活動支援機能開発 START */
        //査定依頼機能を使用しない場合のみ、ボタンイベントを有効
        if ($("#SC3080218usedFlgAssess").attr("value") == "0") {
            /* 2013/01/18 TCS 坪根 【A.STEP2】次世代e-CRB 新車タブレットSC活動支援機能開発 END */

            //if (this_form.Sc3080218selectActAssesment.value == "1") {
            if ($("#Sc3080218selectActAssesment").attr("value") == "1") {
                $("#bodyFrame").trigger("click.popover");
                //this_form.Sc3080218selectActAssesment.value = "0";
                //this_form.Sc3080218selectActAssesmentWK.value = "0";
                $("#Sc3080218selectActAssesment").val("0");
                $("#Sc3080218selectActAssesmentWK").val("0");
                $(this).css("text-shadow", "0px -1px 1px #FFF");
                $(this).css("color", "#808080");
                $(this).css("text-shadow", "block");
                $(this).css("background", "url(" + this_form.Sc3080218AssesmentNonSelPath.value + ") center 9px no-repeat,-webkit-gradient(linear, left top, left bottom,color-stop(0, #fbfbfb),color-stop(100%, #cacaca))");
            }
            else {
                $("#bodyFrame").trigger("click.popover");
                //this_form.Sc3080218selectActAssesment.value = "1";
                //this_form.Sc3080218selectActAssesmentWK.value = "1";
                $("#Sc3080218selectActAssesment").val("1");
                $("#Sc3080218selectActAssesmentWK").val("1");
                $(this).css("text-shadow", "0px -1px 1px #000");
                $(this).css("color", "#FFF");
                $(this).css("text-shadow", "none");
                $(this).css("background", "url(" + this_form.Sc3080218AssesmentSelPath.value + ") center 9px no-repeat,-webkit-gradient(linear, left top, left bottom,color-stop(0, #7b97ed),color-stop(50%, #416ae5),color-stop(51%, #1e4ee1),color-stop(100%, #1e4ee1))");
            }

            /* 2013/01/18 TCS 坪根 【A.STEP2】次世代e-CRB 新車タブレットSC活動支援機能開発 START */
        }
        /* 2013/01/18 TCS 坪根 【A.STEP2】次世代e-CRB 新車タブレットSC活動支援機能開発 END */
    }
    );
});

// 見積り //
$(function () {
    /* 2013/01/18 TCS 坪根 【A.STEP2】次世代e-CRB 新車タブレットSC活動支援機能開発 START */
    //画面上から選択できないようにする
//    $(".Valuationlist").live("click", function (e) {
//        var listname = "#Sc3080218Valuationlist";
//        var listvalue;
//        var i;
//        var j;
//        var sel = $(this).attr("id");
//        var seledary
//        var seledarydetail
//        var seledarycreate = ""
//        sel = sel.replace("Sc3080218Valuationlist", "");
//        //var seled = this_form.Sc3080218selectActValuationWK.value;
//        var seled = $("#Sc3080218selectActValuationWK").attr("value");
//        seledary = seled.split(";");
//        for (i = 0; i < seledary.length - 1; i++) {
//            seledarydetail = seledary[i].split(",");
//            if (seledarydetail[0] == sel) {
//                if (seledarydetail[1] == "0") {
//                    seledarydetail[1] = "1";
//                    listvalue = seledarydetail[0]
//                    $(listname + listvalue).addClass("Selection")
//                }
//                else if (seledarydetail[1] == "1") {
//                    seledarydetail[1] = "0";
//                    listvalue = seledarydetail[0]
//                    $(listname + listvalue).removeClass("Selection")
//                }
//            }
//            seledary[i] = seledarydetail[0] + "," + seledarydetail[1];
//            seledarycreate = seledarycreate + seledary[i] + ";";
//        }
//        //this_form.Sc3080218selectActValuationWK.value = seledarycreate;
//        $("#Sc3080218selectActValuationWK").val(seledarycreate)
//    });
    /* 2013/01/18 TCS 坪根 【A.STEP2】次世代e-CRB 新車タブレットSC活動支援機能開発 END */

    $(".scNscValuationCancellButton").click(function () {
        $("#bodyFrame").trigger("click.popover");
    });

    /* 2013/01/18 TCS 坪根 【A.STEP2】次世代e-CRB 新車タブレットSC活動支援機能開発 START */
//    /* 2012/03/15 TCS 高橋 【SALES_2】(Sales1A ユーザテスト No.222) START */
//    //関数化
//    $(".scNscValuationCompletionButton").click(function () {
////      //this_form.Sc3080218selectActValuation.value = this_form.Sc3080218selectActValuationWK.value;
////      $("#Sc3080218selectActValuation").val($("#Sc3080218selectActValuationWK").attr("value"));
////      var selfalg = "0";
////      //var seled = this_form.Sc3080218selectActValuation.value;
////      var seled = $("#Sc3080218selectActValuation").attr("value");
////      var seledary = seled.split(";");
////      for (i = 0; i < seledary.length - 1; i++) {
////          seledarydetail = seledary[i].split(",");
////          if (seledarydetail[1] == "1") {
////              selfalg = "1";
////          }
////      }
////      if (selfalg == "1") {
////          $("#confirmContents218 .BoxtypeNSC61A .nscListBoxSetRight2 .nscListIcnAset .nscListIcnA4").css("text-shadow", "0px -1px 1px #000");
////          $("#confirmContents218 .BoxtypeNSC61A .nscListBoxSetRight2 .nscListIcnAset .nscListIcnA4").css("color", "#FFF");
////          $("#confirmContents218 .BoxtypeNSC61A .nscListBoxSetRight2 .nscListIcnAset .nscListIcnA4").css("text-shadow", "none");
////          $("#confirmContents218 .BoxtypeNSC61A .nscListBoxSetRight2 .nscListIcnAset .nscListIcnA4").css("background", "url(" + this_form.Sc3080218ValuationSelPath.value + ") center 9px no-repeat,-webkit-gradient(linear, left top, left bottom,color-stop(0, #7b97ed),color-stop(50%, #416ae5),color-stop(51%, #1e4ee1),color-stop(100%, #1e4ee1))");
////          this_form.Sc3080218HD_nscListIcnA1.value = "1";
////      } else {
////          $("#confirmContents218 .BoxtypeNSC61A .nscListBoxSetRight2 .nscListIcnAset .nscListIcnA4").css("text-shadow", "0px -1px 1px #FFF");
////          $("#confirmContents218 .BoxtypeNSC61A .nscListBoxSetRight2 .nscListIcnAset .nscListIcnA4").css("color", "#808080");
////          $("#confirmContents218 .BoxtypeNSC61A .nscListBoxSetRight2 .nscListIcnAset .nscListIcnA4").css("text-shadow", "block");
////          $("#confirmContents218 .BoxtypeNSC61A .nscListBoxSetRight2 .nscListIcnAset .nscListIcnA4").css("background", "url(" + this_form.Sc3080218ValuationNonSelPath.value + ") center 9px no-repeat,-webkit-gradient(linear, left top, left bottom,color-stop(0, #fbfbfb),color-stop(100%, #cacaca))");
////          this_form.Sc3080218HD_nscListIcnA1.value = "0";
////      }
////      $("#bodyFrame").trigger("click.popover");
//        SetSc3080218ActValuationButton();
//    });
//    /* 2012/03/15 TCS 高橋 【SALES_2】(Sales1A ユーザテスト No.222) END */
    /* 2013/01/18 TCS 坪根 【A.STEP2】次世代e-CRB 新車タブレットSC活動支援機能開発 END */
});
$(function () {
    $("#confirmContents218 .BoxtypeNSC61A .nscListBoxSetRight2 .nscListIcnAset .nscListIcnA4").click(
        function () {
            /* 2013/01/18 TCS 坪根 【A.STEP2】次世代e-CRB 新車タブレットSC活動支援機能開発 START */
            //見積車種が存在する場合のみ、タップイベントを付与
            var selectValuation = $("#Sc3080218selectActValuation").attr("value");
            if (selectValuation != '') {
                /* 2013/01/18 TCS 坪根 【A.STEP2】次世代e-CRB 新車タブレットSC活動支援機能開発 END */

                //this_form.Sc3080218selectActValuationWK.value = this_form.Sc3080218selectActValuation.value;
                $("#Sc3080218selectActValuationWK").val($("#Sc3080218selectActValuation").attr("value"));
                var listname = "#Sc3080218Valuationlist";
                var listvalue;
                var i;
                var seledary
                var seledarydetail
                //var seled = this_form.Sc3080218selectActValuation.value;
                var seled = $("#Sc3080218selectActValuation").attr("value");
                seledary = seled.split(";");
                for (i = 0; i < seledary.length - 1; i++) {
                    seledarydetail = seledary[i].split(",");

                    /* 2013/01/18 TCS 坪根 【A.STEP2】次世代e-CRB 新車タブレットSC活動支援機能開発 START */
                    //見積車両を出力する為、コントロールIDは見積管理IDで見る
                    //listvalue = seledarydetail[0];
                    listvalue = seledarydetail[4];
                    /* 2013/01/18 TCS 坪根 【A.STEP2】次世代e-CRB 新車タブレットSC活動支援機能開発 END */

                    if (seledarydetail[1] == "1") {
                        $(listname + listvalue).addClass("Selection")
                    }
                    else {
                        $(listname + listvalue).removeClass("Selection")
                    }
                }
                /* 2013/01/18 TCS 坪根 【A.STEP2】次世代e-CRB 新車タブレットSC活動支援機能開発 START */
            }
            /* 2013/01/18 TCS 坪根 【A.STEP2】次世代e-CRB 新車タブレットSC活動支援機能開発 END */
        }
    );
});

//日付をHidden項目へセットする
function SC3080218SetDate() {

    //今回活動日をHidden値へセット
    this_form.Sc3080218ActTimeFromSelectorWK.value = $("#Sc3080218ActTimeFromSelector").attr("value");
    
    if($("#Sc3080218ActTimeFromSelector").size() > 0){
        this_form.Sc3080218ActTimeFromSelectorWK2.value = $("#Sc3080218ActTimeFromSelector").attr("value");
    }
    
    this_form.Sc3080218ActTimeToSelectorWK.value = $("#Sc3080218ActTimeToSelector").attr("value");
    
    if($("#Sc3080218ActTimeToSelector").size() > 0){
        this_form.Sc3080218ActTimeToSelectorWK2.value = $("#Sc3080218ActTimeToSelector").attr("value");
    }
    
    //WK領域からの戻し
    //カタログ
    $("#Sc3080218selectActCatalog").val($("#Sc3080218selectActCatalogWK").attr("value"));
    
    //試乗
    $("#Sc3080218selectActTestDrive").val($("#Sc3080218selectActTestDriveWK").attr("value"));
    
    //査定
    $("#Sc3080218selectActAssesment").val($("#Sc3080218selectActAssesmentWK").attr("value"));
    
    //見積り
    $("#Sc3080218selectActValuation").val($("#Sc3080218selectActValuationWK").attr("value"));
}

/****************************************************************
ポップアップの後読み込み対応
*****************************************************************/
$(function () {
    $("#Sc3080218popupTrigger4").click(function () {
        var flg = $("#Sc3080218CatalogListPopupFlg").attr("value");
        if (flg == "0") {
            $("#processingServer").addClass("Sc3080218CatalogListPopupLoadingAnimation");
            $("#registOverlayBlack").addClass("BGColor");
            $("#Sc3080218CatalogListButton").click();
        }
        $(".nscListBoxSetIn li:last-child").addClass("end");
    });
    $("#Sc3080218popupTrigger5").click(function () {
        var flg = $("#Sc3080218TestDriveListPopupFlg").attr("value");
        if (flg == "0") {
            $("#processingServer").addClass("Sc3080218TestDriveListPopupLoadingAnimation");
            $("#registOverlayBlack").addClass("BGColor");
            $("#Sc3080218TestDriveListButton").click();
        }
        $(".nscListBoxSetIn li:last-child").addClass("end");
    });

    $("#Sc3080218popupTrigger6").click(function () {
        /* 2013/01/18 TCS 坪根 【A.STEP2】次世代e-CRB 新車タブレットSC活動支援機能開発 START */
        //見積車種が存在する場合のみ、タップイベントを付与
        var selectValuation = $("#Sc3080218selectActValuation").attr("value");
        if (selectValuation != '') {
            /* 2013/01/18 TCS 坪根 【A.STEP2】次世代e-CRB 新車タブレットSC活動支援機能開発 END */

            var flg = $("#Sc3080218ValuationListPopupFlg").attr("value");
            if (flg == "0") {
                $("#processingServer").addClass("Sc3080218ValuationListPopupLoadingAnimation");
                $("#registOverlayBlack").addClass("BGColor");
                $("#Sc3080218ValuationListButton").click();
            }
            $(".nscListBoxSetIn li:last-child").addClass("end");

            /* 2013/01/18 TCS 坪根 【A.STEP2】次世代e-CRB 新車タブレットSC活動支援機能開発 START */
        }
        /* 2013/01/18 TCS 坪根 【A.STEP2】次世代e-CRB 新車タブレットSC活動支援機能開発 END */
    });

    $("#Sc3080218UsersTrigger").click(function () {
        var flg = $("#Sc3080218StaffListPopupFlg").attr("value");
        if (flg == "0") {
            $("#processingServer").addClass("Sc3080218StaffListPopupLoadingAnimation");
            $("#registOverlayBlack").addClass("BGColor");
            $("#Sc3080218StaffListButton").click();
        }
        $(".nscListBoxSetIn li:last-child").addClass("end");
    });
    $("#Sc3080218ActContactTrigger").click(function () {
        var flg = $("#Sc3080218ActContactListPopupFlg").attr("value");
        if (flg == "0") {
            $("#processingServer").addClass("Sc3080218ActContactListPopupLoadingAnimation");
            $("#registOverlayBlack").addClass("BGColor");
            $("#Sc3080218ActContactListButton").click();
        }
        $(".nscListBoxSetIn li:last-child").addClass("end");
    });
    $("#Sc3080218ActTimePopupTrigger").click(function () {
        var flg = $("#Sc3080218ActTimePopupFlg").attr("value");
        if (flg == "0") {
            $("#processingServer").addClass("Sc3080218ActTimePopupLoadingAnimation");
            $("#registOverlayBlack").addClass("BGColor");
            $("#Sc3080218ActTimeButton").click();
        }
    });
});

function setSc3080218CatalogListPageOpenEnd() {
    $(".nscListBoxSetIn li:last-child").addClass("end");
    $("#processingServer").removeClass("Sc3080218CatalogListPopupLoadingAnimation");
    $("#registOverlayBlack").removeClass("BGColor");
}
function setSc3080218TestDriveListPageOpenEnd() {
    $(".nscListBoxSetIn li:last-child").addClass("end");
    $("#processingServer").removeClass("Sc3080218TestDriveListPopupLoadingAnimation");
    $("#registOverlayBlack").removeClass("BGColor");
}
function setSc3080218ValuationListPageOpenEnd() {
    /* 2013/01/18 TCS 坪根 【A.STEP2】次世代e-CRB 新車タブレットSC活動支援機能開発 START */
    //見積りで実績がある場合、選択状態にする
    InitSc3080218selectActValuation();
    /* 2013/01/18 TCS 坪根 【A.STEP2】次世代e-CRB 新車タブレットSC活動支援機能開発 END */

    $(".nscListBoxSetIn li:last-child").addClass("end");
    $("#processingServer").removeClass("Sc3080218ValuationListPopupLoadingAnimation");
    $("#registOverlayBlack").removeClass("BGColor");
}
function setSc3080218StaffListPageOpenEnd() {
    var listname;
    var listvalue;
    listname = "#Sc3080218Stafflist";
    listvalue = this_form.Sc3080218selectStaff.value;
    $(listname + listvalue).addClass("Selection");
    $(".nscListBoxSetIn li:last-child").addClass("end");
    $("#processingServer").removeClass("Sc3080218StaffListPopupLoadingAnimation");
    $("#registOverlayBlack").removeClass("BGColor");
    
}
function setSc3080218ActContactListPageOpenEnd() {
    var listname;
    var listvalue;
    listname = "#Sc3080218ActContactlist"
    listvalue = this_form.Sc3080218selectActContact.value
    $(listname + listvalue).addClass("Selection")
    $(".nscListBoxSetIn li:last-child").addClass("end");
    $("#processingServer").removeClass("Sc3080218ActContactListPopupLoadingAnimation");
    $("#registOverlayBlack").removeClass("BGColor");
}
function setSc3080218ActTimePageOpenEnd() {
    $("#Sc3080218ActTimePanel").find("input").addClass("icrop-DateTimeSelector")
    $("#processingServer").removeClass("Sc3080218ActTimePopupLoadingAnimation");
    $("#registOverlayBlack").removeClass("BGColor");
}

/* 2012/03/15 TCS 高橋 【SALES_2】(Sales1A ユーザテスト No.222) START */
function SetSc3080218ProcessIcon() {
    SetSc3080218ActCatalogButton();
    SetSc3080218ActTestDriveButton();
    SetSc3080218ActValuationButton();
}

function SetSc3080218ActCatalogButton() {
    $("#Sc3080218selectActCatalog").val($("#Sc3080218selectActCatalogWK").attr("value"));
    var selfalg = "0";
    var seled = $("#Sc3080218selectActCatalog").attr("value");
    var seledary = seled.split(";");
    for (i = 0; i < seledary.length - 1; i++) {
        seledarydetail = seledary[i].split(",");
        if (seledarydetail[1] == "1") {
            selfalg = "1";
        }
    }
    if (selfalg == "1") {
        $("#confirmContents218 .BoxtypeNSC61A .nscListBoxSetRight2 .nscListIcnAset .nscListIcnA1").css("text-shadow", "0px -1px 1px #000");
        $("#confirmContents218 .BoxtypeNSC61A .nscListBoxSetRight2 .nscListIcnAset .nscListIcnA1").css("color", "#FFF");
        $("#confirmContents218 .BoxtypeNSC61A .nscListBoxSetRight2 .nscListIcnAset .nscListIcnA1").css("text-shadow", "none");
        $("#confirmContents218 .BoxtypeNSC61A .nscListBoxSetRight2 .nscListIcnAset .nscListIcnA1").css("background", "url(" + this_form.Sc3080218CatalogSelPath.value + ") center 9px no-repeat,-webkit-gradient(linear, left top, left bottom,color-stop(0, #7b97ed),color-stop(50%, #416ae5),color-stop(51%, #1e4ee1),color-stop(100%, #1e4ee1))");
        this_form.Sc3080218HD_nscListIcnA1.value = "1";
    } else {
        $("#confirmContents218 .BoxtypeNSC61A .nscListBoxSetRight2 .nscListIcnAset .nscListIcnA1").css("text-shadow", "0px -1px 1px #FFF");
        $("#confirmContents218 .BoxtypeNSC61A .nscListBoxSetRight2 .nscListIcnAset .nscListIcnA1").css("color", "#808080");
        $("#confirmContents218 .BoxtypeNSC61A .nscListBoxSetRight2 .nscListIcnAset .nscListIcnA1").css("text-shadow", "block");
        $("#confirmContents218 .BoxtypeNSC61A .nscListBoxSetRight2 .nscListIcnAset .nscListIcnA1").css("background", "url(" + this_form.Sc3080218CatalogNonSelPath.value + ") center 9px no-repeat,-webkit-gradient(linear, left top, left bottom,color-stop(0, #fbfbfb),color-stop(100%, #cacaca))");
        this_form.Sc3080218HD_nscListIcnA1.value = "0";
    }
    $("#bodyFrame").trigger("click.popover");
}
function SetSc3080218ActTestDriveButton() {
    //this_form.Sc3080218selectActTestDrive.value = this_form.Sc3080218selectActTestDriveWK.value;
    $("#Sc3080218selectActTestDrive").val($("#Sc3080218selectActTestDriveWK").attr("value"));
    var selfalg = "0";
    //var seled = this_form.Sc3080218selectActTestDrive.value;
    var seled = $("#Sc3080218selectActTestDrive").attr("value");
    var seledary = seled.split(";");
    for (i = 0; i < seledary.length - 1; i++) {
        seledarydetail = seledary[i].split(",");
        if (seledarydetail[1] == "1") {
            selfalg = "1";
        }
    }
    if (selfalg == "1") {
        $("#confirmContents218 .BoxtypeNSC61A .nscListBoxSetRight2 .nscListIcnAset .nscListIcnA2").css("text-shadow", "0px -1px 1px #000");
        $("#confirmContents218 .BoxtypeNSC61A .nscListBoxSetRight2 .nscListIcnAset .nscListIcnA2").css("color", "#FFF");
        $("#confirmContents218 .BoxtypeNSC61A .nscListBoxSetRight2 .nscListIcnAset .nscListIcnA2").css("text-shadow", "none");
        $("#confirmContents218 .BoxtypeNSC61A .nscListBoxSetRight2 .nscListIcnAset .nscListIcnA2").css("background", "url(" + this_form.Sc3080218TestDriveSelPath.value + ") center 9px no-repeat,-webkit-gradient(linear, left top, left bottom,color-stop(0, #7b97ed),color-stop(50%, #416ae5),color-stop(51%, #1e4ee1),color-stop(100%, #1e4ee1))");
        this_form.Sc3080218HD_nscListIcnA1.value = "1";
    } else {
        $("#confirmContents218 .BoxtypeNSC61A .nscListBoxSetRight2 .nscListIcnAset .nscListIcnA2").css("text-shadow", "0px -1px 1px #FFF");
        $("#confirmContents218 .BoxtypeNSC61A .nscListBoxSetRight2 .nscListIcnAset .nscListIcnA2").css("color", "#808080");
        $("#confirmContents218 .BoxtypeNSC61A .nscListBoxSetRight2 .nscListIcnAset .nscListIcnA2").css("text-shadow", "block");
        $("#confirmContents218 .BoxtypeNSC61A .nscListBoxSetRight2 .nscListIcnAset .nscListIcnA2").css("background", "url(" + this_form.Sc3080218TestDriveNonSelPath.value + ") center 9px no-repeat,-webkit-gradient(linear, left top, left bottom,color-stop(0, #fbfbfb),color-stop(100%, #cacaca))");
        this_form.Sc3080218HD_nscListIcnA1.value = "0";
    }
    $("#bodyFrame").trigger("click.popover");
}

function SetSc3080218ActValuationButton() {
    //this_form.Sc3080218selectActValuation.value = this_form.Sc3080218selectActValuationWK.value;
    $("#Sc3080218selectActValuation").val($("#Sc3080218selectActValuationWK").attr("value"));

    /* 2013/01/18 TCS 坪根 【A.STEP2】次世代e-CRB 新車タブレットSC活動支援機能開発 START */
    var listname = "#Valuationlist";
    var listvalue;
    /* 2013/01/18 TCS 坪根 【A.STEP2】次世代e-CRB 新車タブレットSC活動支援機能開発 END */

    var selfalg = "0";
    //var seled = this_form.Sc3080218selectActValuation.value;
    var seled = $("#Sc3080218selectActValuation").attr("value");
    var seledary = seled.split(";");
    for (i = 0; i < seledary.length - 1; i++) {
        seledarydetail = seledary[i].split(",");

        /* 2013/01/18 TCS 坪根 【A.STEP2】次世代e-CRB 新車タブレットSC活動支援機能開発 START */
        //見積車両を出力する為、コントロールIDは見積管理IDで見る
        listvalue = seledarydetail[4];
        /* 2013/01/18 TCS 坪根 【A.STEP2】次世代e-CRB 新車タブレットSC活動支援機能開発 END */

        if (seledarydetail[1] == "1") {
            /* 2013/01/18 TCS 坪根 【A.STEP2】次世代e-CRB 新車タブレットSC活動支援機能開発 START */
            $(listname + listvalue).addClass("Selection");
            /* 2013/01/18 TCS 坪根 【A.STEP2】次世代e-CRB 新車タブレットSC活動支援機能開発 END */
            selfalg = "1";
        }
        /* 2013/01/18 TCS 坪根 【A.STEP2】次世代e-CRB 新車タブレットSC活動支援機能開発 START */
        else {
            $(listname + listvalue).removeClass("Selection");
        }
        /* 2013/01/18 TCS 坪根 【A.STEP2】次世代e-CRB 新車タブレットSC活動支援機能開発 END */
    }
    if (selfalg == "1") {
        $("#confirmContents218 .BoxtypeNSC61A .nscListBoxSetRight2 .nscListIcnAset .nscListIcnA4").css("text-shadow", "0px -1px 1px #000");
        $("#confirmContents218 .BoxtypeNSC61A .nscListBoxSetRight2 .nscListIcnAset .nscListIcnA4").css("color", "#FFF");
        $("#confirmContents218 .BoxtypeNSC61A .nscListBoxSetRight2 .nscListIcnAset .nscListIcnA4").css("text-shadow", "none");
        $("#confirmContents218 .BoxtypeNSC61A .nscListBoxSetRight2 .nscListIcnAset .nscListIcnA4").css("background", "url(" + this_form.Sc3080218ValuationSelPath.value + ") center 9px no-repeat,-webkit-gradient(linear, left top, left bottom,color-stop(0, #7b97ed),color-stop(50%, #416ae5),color-stop(51%, #1e4ee1),color-stop(100%, #1e4ee1))");
        this_form.Sc3080218HD_nscListIcnA1.value = "1";
    } else {
        $("#confirmContents218 .BoxtypeNSC61A .nscListBoxSetRight2 .nscListIcnAset .nscListIcnA4").css("text-shadow", "0px -1px 1px #FFF");
        $("#confirmContents218 .BoxtypeNSC61A .nscListBoxSetRight2 .nscListIcnAset .nscListIcnA4").css("color", "#808080");
        $("#confirmContents218 .BoxtypeNSC61A .nscListBoxSetRight2 .nscListIcnAset .nscListIcnA4").css("text-shadow", "block");
        $("#confirmContents218 .BoxtypeNSC61A .nscListBoxSetRight2 .nscListIcnAset .nscListIcnA4").css("background", "url(" + this_form.Sc3080218ValuationNonSelPath.value + ") center 9px no-repeat,-webkit-gradient(linear, left top, left bottom,color-stop(0, #fbfbfb),color-stop(100%, #cacaca))");
        this_form.Sc3080218HD_nscListIcnA1.value = "0";
    }
    $("#bodyFrame").trigger("click.popover");
}
/* 2012/03/15 TCS 高橋 【SALES_2】(Sales1A ユーザテスト No.222) END */
$(function () {
    $("#registOverlayBlack").click(function () {
        if($("#registOverlayBlack").attr("class").indexOf("BGColor",0) != -1){
            //ロード時のフィルタを非表示
            $("#registOverlayBlack").css("display","none")
            $("#registOverlayBlack").removeClass("BGColor");
            
            //ロードアイコンの設定を解除
            $("#processingServer").removeClass("show");
            $("#processingServer").removeClass("Sc3080218CatalogListPopupLoadingAnimation");
            $("#processingServer").removeClass("Sc3080218TestDriveListPopupLoadingAnimation");
            $("#processingServer").removeClass("Sc3080218ValuationListPopupLoadingAnimation");
            $("#processingServer").removeClass("Sc3080218StaffListPopupLoadingAnimation");
            $("#processingServer").removeClass("Sc3080218ActContactListPopupLoadingAnimation");
            $("#processingServer").removeClass("Sc3080218ActTimePopupLoadingAnimation");
            $("#processingServer").removeClass("NextActContactPopupLoadingAnimation");
            $("#processingServer").removeClass("FollowContactPopupLoadingAnimation");
            $("#processingServer").removeClass("NextActTimeLoadingAnimation");
            $("#processingServer").removeClass("FollowTimeLoadingAnimation");
            $("#processingServer").removeClass("GiveupReasonPopupLoadingAnimation");
            
            //読み込みを停止
            stop();
        }
    });
});

/* 2013/01/18 TCS 坪根 【A.STEP2】次世代e-CRB 新車タブレットSC活動支援機能開発 START */
/**
* @プロセス(査定)ボタン設定
*
* @param {String} actAssesment 査定実績状態値
* @remarks
* @活動登録画面への遷移時も、このメソッドをコール
*/
function Sc3080218ActAssesmentButtonOnOff(actAssesment) {
    if (actAssesment == "1") {
        $("#confirmContents218 .BoxtypeNSC61A .nscListBoxSetRight2 .nscListIcnAset .nscListIcnA3").css("text-shadow", "0px -1px 1px #000");
        $("#confirmContents218 .BoxtypeNSC61A .nscListBoxSetRight2 .nscListIcnAset .nscListIcnA3").css("color", "#FFF");
        $("#confirmContents218 .BoxtypeNSC61A .nscListBoxSetRight2 .nscListIcnAset .nscListIcnA3").css("text-shadow", "none");
        $("#confirmContents218 .BoxtypeNSC61A .nscListBoxSetRight2 .nscListIcnAset .nscListIcnA3").css("background", "url(" + this_form.Sc3080218AssesmentSelPath.value + ") center 9px no-repeat,-webkit-gradient(linear, left top, left bottom,color-stop(0, #7b97ed),color-stop(50%, #416ae5),color-stop(51%, #1e4ee1),color-stop(100%, #1e4ee1))");
    } else {
        $("#confirmContents218 .BoxtypeNSC61A .nscListBoxSetRight2 .nscListIcnAset .nscListIcnA3").css("text-shadow", "0px -1px 1px #FFF");
        $("#confirmContents218 .BoxtypeNSC61A .nscListBoxSetRight2 .nscListIcnAset .nscListIcnA3").css("color", "#808080");
        $("#confirmContents218 .BoxtypeNSC61A .nscListBoxSetRight2 .nscListIcnAset .nscListIcnA3").css("text-shadow", "block");
        $("#confirmContents218 .BoxtypeNSC61A .nscListBoxSetRight2 .nscListIcnAset .nscListIcnA3").css("background", "url(" + this_form.Sc3080218AssesmentNonSelPath.value + ") center 9px no-repeat,-webkit-gradient(linear, left top, left bottom,color-stop(0, #fbfbfb),color-stop(100%, #cacaca))");
    }
}

/**
* @初期表示時、見積車種で実績がある場合、選択状態にする
*
* @remarks
* @初期表示時にコールされる。
*/
function InitSc3080218selectActValuation() {
    var listname = "#Sc3080218Valuationlist";
    var listvalue;
    var i;
    var seledary
    var seledarydetail
    var seled = $("#Sc3080218selectActValuation").attr("value");
    seledary = seled.split(";");
    for (i = 0; i < seledary.length - 1; i++) {
        seledarydetail = seledary[i].split(",");
        listvalue = seledarydetail[4];

        if (seledarydetail[1] == "1") {
            $(listname + listvalue).addClass("Selection")
        }
        else {
            $(listname + listvalue).removeClass("Selection")
        }
    }
}
/* 2013/01/18 TCS 坪根 【A.STEP2】次世代e-CRB 新車タブレットSC活動支援機能開発 END */