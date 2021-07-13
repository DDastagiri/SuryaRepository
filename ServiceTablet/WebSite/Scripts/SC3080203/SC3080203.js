/// <reference path="../jquery.js"/>
/// <reference path="../jquery.fingerscroll.js"/>

//登録ボタン押下時のイベントハンドラ登録
$(function () {
    SC3080201.addRegistEventHandlers(InputCheck);
});


/****************************************************************

今回活動日時のポップアップ動作

*****************************************************************/

$(function () {


    //活動日時のコミット値保存
    $("#ActTimeFromSelector, #ActTimeToSelector").each(function () {
        this.commitValue = this.valueAsDate;
    });

    //今回活動時間ポップアップ完了ボタン
    $(".scNscActTimeCompletionButton").click(function () {

        //活動日時のコミット値保存
        $("#ActTimeFromSelector, #ActTimeToSelector").each(function () {
            this.commitValue = this.valueAsDate;
        });

        //表示用日付文字列設定
        $(".ActTime").text(getDisplayDate("ActTime"));

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
        //活動日用のポップアップ
        $("#ActTimeFromSelector, #ActTimeToSelector").each(function () {
            this.valueAsDate = this.commitValue;
        });
    }


    //ポップアップのスタイル
    setTimeout(function () {
        $(".popover").each(function () {
            if ($(this).find("#MstPG_Logout").length <= 0 && $(this).find(".icrop-NumericKeypad-content-frame").length <= 0) {
                $(this).addClass("cust");
            }
        });
    }, 0);

});




// 初期表示設定 //
$(function () {
    var h = $("#confirmContents60 .nscListBoxSet.HeightB").height();
    var listname;
    var listvalue;

    //元のFollow-upBoxがHot、Prospect、Walk-in以外の場合Walk-inの活動結果を非表示にする
    var fllwDvs = $("#FllwDvs").attr("value")
    if ((fllwDvs == "1" || fllwDvs == "2" || fllwDvs == "6") || fllwDvs == "") {
        $(".nscListIcnB1").css("display", "block");
    } else {
        $(".nscListIcnB1").css("display", "none");
    }

    $(".HeightB").css("display", "none");
    $(".HeightC").css("display", "none");
    $(".HeightD").css("display", "none");

    listname = "#Stafflist";
    listvalue = this_form.selectStaff.value;
    $(listname + listvalue).addClass("Selection");
    $(".scNscStaffName").text($(listname + listvalue).attr("title"));


    listname = "#ActContactlist"
    listvalue = this_form.selectActContact.value
    $(listname + listvalue).addClass("Selection")
    $(".scNscActContactName").text($(listname + listvalue).attr("title"))

    listname = "#NextActContactlist"
    listvalue = this_form.selectNextActContact.value
    $(listname + listvalue).addClass("Selection")
    $(".scNscNextActContactName").text($(listname + listvalue).attr("title"))

    listname = "#FollowContactlist"
    listvalue = this_form.selectFollowContact.value
    $(listname + listvalue).addClass("Selection")
    $(".scNscFollowContactName").text($(listname + listvalue).attr("title"))


    //今回活動日(From)の復元
    if (this_form.ActDayFrom.value != "") {
  //      $("#ActTimeFromSelector").val(this_form.ActDayFrom.value)
    }

    //今回活動日(To)の復元
    if (this_form.ActDayTo.value != "") {
    //    $("#ActTimeToSelector").val(this_form.ActDayTo.value)
    }

    if (this_form.NextActDayFrom.value != "") {
        if (this_form.NextActDayToFlg.value == "1") {
            //次回活動日(From)の復元
            $("#NextActTimeFromSelector").val(this_form.NextActDayFrom.value)
        }
        else {
            //次回活動日(期限)の復元
            $("#NextActTimeFromSelectorTime").val(this_form.NextActDayFrom.value)
        }
    }

    //次回活動日(To)の復元
    if (this_form.NextActDayTo.value != "") {
        $("#NextActTimeToSelector").val(this_form.NextActDayTo.value)
    }

    if (this_form.FollowDayFrom.value != "") {
        if (this_form.FollowDayToFlg.value == "1") {
            //フォロー日(From)の復元
            $("#FollowTimeFromSelector").val(this_form.FollowDayFrom.value)
        }
        else {
            //フォロー日(期限)の復元
            $("#FollowTimeFromSelectorTime").val(this_form.FollowDayFrom.value)
        }
    }

    //フォロー日(To)の復元
    if (this_form.FollowDayTo.value != "") {
        $("#FollowTimeToSelector").val(this_form.FollowDayTo.value)
    }



    //プロセス欄の非表示化
    if (this_form.ProcessFlg.value == "1") {
        $("#confirmContents60 .nscListBoxSet .nscListBoxSetIn .nscListBoxSetRight2").animate({ width: "show" }, 0);
    }
    else {
        $("#confirmContents60 .nscListBoxSet .nscListBoxSetIn .nscListBoxSetRight2").animate({ width: "hide" }, 0);
    }



    //カタログ選択内容の復元
    this_form.selectActCatalogWK.value = this_form.selectActCatalog.value;
    var listname = "#Cataloglist";
    var listvalue;
    var i;
    var seledary
    var seledarydetail
    var seled = this_form.selectActCatalog.value;
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
        $("#confirmContents60 .nscListBoxSet .nscListBoxSetIn .nscListIcnAset .nscListIcnA1").css("text-shadow", "0px -1px 1px #000");
        $("#confirmContents60 .nscListBoxSet .nscListBoxSetIn .nscListIcnAset .nscListIcnA1").css("color", "#FFF");
        $("#confirmContents60 .nscListBoxSet .nscListBoxSetIn .nscListIcnAset .nscListIcnA1").css("text-shadow", "none");
        $("#confirmContents60 .nscListBoxSet .nscListBoxSetIn .nscListIcnAset .nscListIcnA1").css("background", "url(" + this_form.CatalogSelPath.value + ") center 9px no-repeat,-webkit-gradient(linear, left top, left bottom,color-stop(0, #7b97ed),color-stop(50%, #416ae5),color-stop(51%, #1e4ee1),color-stop(100%, #1e4ee1))");
        this_form.HD_nscListIcnA1.value = "1";
    } else {
        $("#confirmContents60 .nscListBoxSet .nscListBoxSetIn .nscListIcnAset .nscListIcnA1").css("text-shadow", "0px -1px 1px #FFF");
        $("#confirmContents60 .nscListBoxSet .nscListBoxSetIn .nscListIcnAset .nscListIcnA1").css("color", "#808080");
        $("#confirmContents60 .nscListBoxSet .nscListBoxSetIn .nscListIcnAset .nscListIcnA1").css("text-shadow", "block");
        $("#confirmContents60 .nscListBoxSet .nscListBoxSetIn .nscListIcnAset .nscListIcnA1").css("background", "url(" + this_form.CatalogNonSelPath.value + ") center 9px no-repeat,-webkit-gradient(linear, left top, left bottom,color-stop(0, #fbfbfb),color-stop(100%, #cacaca))");
        this_form.HD_nscListIcnA1.value = "0";
    }


    //試乗内容の復元
    this_form.selectActTestDriveWK.value = this_form.selectActTestDrive.value;
    var listname = "#TestDrivelist";
    var listvalue;
    var i;
    var seledary
    var seledarydetail
    var seled = this_form.selectActTestDrive.value;
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
        $("#confirmContents60 .nscListBoxSet .nscListBoxSetIn .nscListIcnAset .nscListIcnA2").css("text-shadow", "0px -1px 1px #000");
        $("#confirmContents60 .nscListBoxSet .nscListBoxSetIn .nscListIcnAset .nscListIcnA2").css("color", "#FFF");
        $("#confirmContents60 .nscListBoxSet .nscListBoxSetIn .nscListIcnAset .nscListIcnA2").css("text-shadow", "none");
        $("#confirmContents60 .nscListBoxSet .nscListBoxSetIn .nscListIcnAset .nscListIcnA2").css("background", "url(" + this_form.TestDriveSelPath.value + ") center 9px no-repeat,-webkit-gradient(linear, left top, left bottom,color-stop(0, #7b97ed),color-stop(50%, #416ae5),color-stop(51%, #1e4ee1),color-stop(100%, #1e4ee1))");
        this_form.HD_nscListIcnA1.value = "1";
    } else {
        $("#confirmContents60 .nscListBoxSet .nscListBoxSetIn .nscListIcnAset .nscListIcnA2").css("text-shadow", "0px -1px 1px #FFF");
        $("#confirmContents60 .nscListBoxSet .nscListBoxSetIn .nscListIcnAset .nscListIcnA2").css("color", "#808080");
        $("#confirmContents60 .nscListBoxSet .nscListBoxSetIn .nscListIcnAset .nscListIcnA2").css("text-shadow", "block");
        $("#confirmContents60 .nscListBoxSet .nscListBoxSetIn .nscListIcnAset .nscListIcnA2").css("background", "url(" + this_form.TestDriveNonSelPath.value + ") center 9px no-repeat,-webkit-gradient(linear, left top, left bottom,color-stop(0, #fbfbfb),color-stop(100%, #cacaca))");
        this_form.HD_nscListIcnA1.value = "0";
    }


    //査定内容の復元
    if (this_form.selectActAssesment.value == "1") {
        $("#confirmContents60 .nscListBoxSet .nscListBoxSetIn .nscListIcnAset .nscListIcnA3").css("text-shadow", "0px -1px 1px #000");
        $("#confirmContents60 .nscListBoxSet .nscListBoxSetIn .nscListIcnAset .nscListIcnA3").css("color", "#FFF");
        $("#confirmContents60 .nscListBoxSet .nscListBoxSetIn .nscListIcnAset .nscListIcnA3").css("text-shadow", "none");
        $("#confirmContents60 .nscListBoxSet .nscListBoxSetIn .nscListIcnAset .nscListIcnA3").css("background", "url(" + this_form.AssesmentSelPath.value + ") center 9px no-repeat,-webkit-gradient(linear, left top, left bottom,color-stop(0, #7b97ed),color-stop(50%, #416ae5),color-stop(51%, #1e4ee1),color-stop(100%, #1e4ee1))");
    } else {
        $("#confirmContents60 .nscListBoxSet .nscListBoxSetIn .nscListIcnAset .nscListIcnA3").css("text-shadow", "0px -1px 1px #FFF");
        $("#confirmContents60 .nscListBoxSet .nscListBoxSetIn .nscListIcnAset .nscListIcnA3").css("color", "#808080");
        $("#confirmContents60 .nscListBoxSet .nscListBoxSetIn .nscListIcnAset .nscListIcnA3").css("text-shadow", "block");
        $("#confirmContents60 .nscListBoxSet .nscListBoxSetIn .nscListIcnAset .nscListIcnA3").css("background", "url(" + this_form.AssesmentNonSelPath.value + ") center 9px no-repeat,-webkit-gradient(linear, left top, left bottom,color-stop(0, #fbfbfb),color-stop(100%, #cacaca))");
    }


    //見積り内容の復元
    this_form.selectActValuationWK.value = this_form.selectActValuation.value;
    var listname = "#Valuationlist";
    var listvalue;
    var i;
    var seledary
    var seledarydetail
    var seled = this_form.selectActValuation.value;
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
        $("#confirmContents60 .nscListBoxSet .nscListBoxSetIn .nscListIcnAset .nscListIcnA4").css("text-shadow", "0px -1px 1px #000");
        $("#confirmContents60 .nscListBoxSet .nscListBoxSetIn .nscListIcnAset .nscListIcnA4").css("color", "#FFF");
        $("#confirmContents60 .nscListBoxSet .nscListBoxSetIn .nscListIcnAset .nscListIcnA4").css("text-shadow", "none");
        $("#confirmContents60 .nscListBoxSet .nscListBoxSetIn .nscListIcnAset .nscListIcnA4").css("background", "url(" + this_form.ValuationSelPath.value + ") center 9px no-repeat,-webkit-gradient(linear, left top, left bottom,color-stop(0, #7b97ed),color-stop(50%, #416ae5),color-stop(51%, #1e4ee1),color-stop(100%, #1e4ee1))");
        this_form.HD_nscListIcnA1.value = "1";
    } else {
        $("#confirmContents60 .nscListBoxSet .nscListBoxSetIn .nscListIcnAset .nscListIcnA4").css("text-shadow", "0px -1px 1px #FFF");
        $("#confirmContents60 .nscListBoxSet .nscListBoxSetIn .nscListIcnAset .nscListIcnA4").css("color", "#808080");
        $("#confirmContents60 .nscListBoxSet .nscListBoxSetIn .nscListIcnAset .nscListIcnA4").css("text-shadow", "block");
        $("#confirmContents60 .nscListBoxSet .nscListBoxSetIn .nscListIcnAset .nscListIcnA4").css("background", "url(" + this_form.ValuationNonSelPath.value + ") center 9px no-repeat,-webkit-gradient(linear, left top, left bottom,color-stop(0, #fbfbfb),color-stop(100%, #cacaca))");
        this_form.HD_nscListIcnA1.value = "0";
    }


    //活動結果の復元
    var fllwstatus
    if (this_form.selectActRlst.value != "") {
        if (this_form.selectActRlst.value == "1") {
            fllwstatus = "7"
        }
        if (this_form.selectActRlst.value == "2") {
            fllwstatus = "2"
        }
        if (this_form.selectActRlst.value == "3") {
            fllwstatus = "1"
        }
    }
    else {
        fllwstatus = this_form.fllwStatus.value;
    }
    if (fllwstatus == "1") {
        $("#confirmContents60 .nscListBoxSet .nscListBoxSetIn .nscListIcnBset .nscListIcnB3").css("text-shadow", "0px -1px 1px #000");
        $("#confirmContents60 .nscListBoxSet .nscListBoxSetIn .nscListIcnBset .nscListIcnB3").css("color", "#FFF");
        $("#confirmContents60 .nscListBoxSet .nscListBoxSetIn .nscListIcnBset .nscListIcnB3").css("text-shadow", "none");
        $("#confirmContents60 .nscListBoxSet .nscListBoxSetIn .nscListIcnBset .nscListIcnB3").css("background", "url(../Styles/Images/SC3080201/nsc60icn2cOver.png) center 9px no-repeat,-webkit-gradient(linear, left top, left bottom,color-stop(0, #7b97ed),color-stop(50%, #416ae5),color-stop(51%, #1e4ee1),color-stop(100%, #1e4ee1))");
        $("#confirmContents60 .nscListBoxSet.HeightB").css({ height: 0 }).show(0).animate({ height: h }, 0);
        this_form.HD_nscListIcnB3.value = "1";
    }
    if (fllwstatus == "2") {
        $("#confirmContents60 .nscListBoxSet .nscListBoxSetIn .nscListIcnBset .nscListIcnB2").css("text-shadow", "0px -1px 1px #000");
        $("#confirmContents60 .nscListBoxSet .nscListBoxSetIn .nscListIcnBset .nscListIcnB2").css("color", "#FFF");
        $("#confirmContents60 .nscListBoxSet .nscListBoxSetIn .nscListIcnBset .nscListIcnB2").css("text-shadow", "none");
        $("#confirmContents60 .nscListBoxSet .nscListBoxSetIn .nscListIcnBset .nscListIcnB2").css("background", "url(../Styles/Images/SC3080201/nsc60icn2bOver.png) center 9px no-repeat,-webkit-gradient(linear, left top, left bottom,color-stop(0, #7b97ed),color-stop(50%, #416ae5),color-stop(51%, #1e4ee1),color-stop(100%, #1e4ee1))");
        $("#confirmContents60 .nscListBoxSet.HeightB").css({ height: 0 }).show(0).animate({ height: h }, 0);
        this_form.HD_nscListIcnB2.value = "1";
    }
    if (fllwstatus == "7") {
        $("#confirmContents60 .nscListBoxSet .nscListBoxSetIn .nscListIcnBset .nscListIcnB1").css("text-shadow", "0px -1px 1px #000");
        $("#confirmContents60 .nscListBoxSet .nscListBoxSetIn .nscListIcnBset .nscListIcnB1").css("color", "#FFF");
        $("#confirmContents60 .nscListBoxSet .nscListBoxSetIn .nscListIcnBset .nscListIcnB1").css("text-shadow", "none");
        $("#confirmContents60 .nscListBoxSet .nscListBoxSetIn .nscListIcnBset .nscListIcnB1").css("background", "url(../Styles/Images/SC3080201/nsc60icn2aOver.png) center 9px no-repeat,-webkit-gradient(linear, left top, left bottom,color-stop(0, #7b97ed),color-stop(50%, #416ae5),color-stop(51%, #1e4ee1),color-stop(100%, #1e4ee1))");
        $("#confirmContents60 .nscListBoxSet.HeightB").css({ height: 0 }).show(0).animate({ height: h }, 0);
        this_form.HD_nscListIcnB1.value = "1";
    }
    if (this_form.selectActRlst.value == "4") {
        $("#confirmContents60 .nscListBoxSet .nscListBoxSetIn .nscListIcnBset .nscListIcnB4").css("text-shadow", "0px -1px 1px #000");
        $("#confirmContents60 .nscListBoxSet .nscListBoxSetIn .nscListIcnBset .nscListIcnB4").css("color", "#FFF");
        $("#confirmContents60 .nscListBoxSet .nscListBoxSetIn .nscListIcnBset .nscListIcnB4").css("text-shadow", "none");
        $("#confirmContents60 .nscListBoxSet .nscListBoxSetIn .nscListIcnBset .nscListIcnB4").css("background", "url(../Styles/Images/SC3080201/nsc60icn2dOver.png) center 9px no-repeat,-webkit-gradient(linear, left top, left bottom,color-stop(0, #7b97ed),color-stop(50%, #416ae5),color-stop(51%, #1e4ee1),color-stop(100%, #1e4ee1))");
        $("#confirmContents60 .nscListBoxSet.HeightC").css({ height: 0 }).show(0).animate({ height: h }, 0);
        this_form.HD_nscListIcnB4.value = "1";
    }
    if (this_form.selectActRlst.value == "5") {
        $("#confirmContents60 .nscListBoxSet .nscListBoxSetIn .nscListIcnBset .nscListIcnB5").css("text-shadow", "0px -1px 1px #000");
        $("#confirmContents60 .nscListBoxSet .nscListBoxSetIn .nscListIcnBset .nscListIcnB5").css("color", "#FFF");
        $("#confirmContents60 .nscListBoxSet .nscListBoxSetIn .nscListIcnBset .nscListIcnB5").css("text-shadow", "none");
        $("#confirmContents60 .nscListBoxSet .nscListBoxSetIn .nscListIcnBset .nscListIcnB5").css("background", "url(../Styles/Images/SC3080201/nsc60icn2eOver.png) center 9px no-repeat,-webkit-gradient(linear, left top, left bottom,color-stop(0, #7b97ed),color-stop(50%, #416ae5),color-stop(51%, #1e4ee1),color-stop(100%, #1e4ee1))");
        $("#confirmContents60 .nscListBoxSet.HeightD").css({ height: 0 }).show(0).animate({ height: h }, 0);
        this_form.HD_nscListIcnB5.value = "1";
    }




    //成約車種の復元
    var listname = "#SelectedCarlist";
    var listvalue;
    var i;
    var j;
    var seledary
    var seledarydetail
    var seledarycreate = ""
    var seled = this_form.selectSelSeries.value;
    seledary = seled.split(";");
    for (i = 0; i < seledary.length - 1; i++) {
        seledarydetail = seledary[i].split(",");
        if (seledarydetail[1] == "1") {
            listvalue = seledarydetail[0]
            $(listname + listvalue).addClass("Selection")
        }
        else if (seledarydetail[1] == "0") {
            listvalue = seledarydetail[0]
            $(listname + listvalue).removeClass("Selection")
        }
    }

    //断念車種の復元
    $("#GiveupCar" + this_form.selectGiveupCar.value).addClass("Selection");
    $(".Giveup").html(this_form.selectGiveupCarName.value);

    //断念理由の復元
    this_form.GiveupReason.value = this_form.selectGiveupReason.value

    var Format = this_form.dateFormt.value
    var FollowTimeFrom = this_form.ActTimeFromSelector.valueAsDate;
    var FollowTimeTo = this_form.ActTimeToSelector.valueAsDate;

    FollowTimeTo = FollowTimeTo.getUTCHours() + ":" + ("0" + FollowTimeTo.getUTCMinutes()).slice(-2);
    var month = FollowTimeFrom.getMonth() + 1;
    var day = FollowTimeFrom.getDate();
    Format = Format.replace("%3", day);
    Format = Format.replace("%2", month);
    var time = FollowTimeFrom.getHours() + ":" + ("0" + FollowTimeFrom.getMinutes()).slice(-2);
    $(".ActTime").html((Format + " " + time + "-" + FollowTimeTo));
    var NextAct = $(".NextActContactlist").children("span").attr("value").split("_");

    if (this_form.NextActDayToFlg.value == "1") {
        NextAct[1] = "2"
    }
    if (this_form.NextActDayToFlg.value == "0") {
        NextAct[1] = "1"
    }

//    if (NextAct[1] == "1") {
//        $(".NextActTimeTo").css("display", "none");
//        $(".NextActTimeStart").css("display", "none");
//        $(".NextActTimeLimit").css("display", "block");
//        var Format = this_form.dateFormt.value;
//        var FollowTimeFrom = this_form.NextActTimeFromSelectorTime.valueAsDate;
//        var month = FollowTimeFrom.getMonth() + 1;
//        var day = FollowTimeFrom.getDate();
//        Format = Format.replace("%3", day);
//        Format = Format.replace("%2", month);
//        var time = FollowTimeFrom.getHours() + ":" + ("0" + FollowTimeFrom.getMinutes()).slice(-2);
//        $(".NextActTime").html(Format + " " + time);
//        this_form.NextActDayToFlg.value = "0";
//    }
//    if (NextAct[1] == "2") {
//        $(".NextActTimeTo").css("display", "block");
//        $(".NextActTimeStart").css("display", "block");
//        $(".NextActTimeLimit").css("display", "none");
//        var Format = this_form.dateFormt.value
//        var FollowTimeFrom = this_form.NextActTimeFromSelector.valueAsDate;
//        var FollowTimeTo = this_form.NextActTimeToSelector.valueAsDate;
//        FollowTimeTo = FollowTimeTo.getUTCHours() + ":" + ("0" + FollowTimeTo.getUTCMinutes()).slice(-2);
//        var month = FollowTimeFrom.getMonth() + 1;
//        var day = FollowTimeFrom.getDate();
//        Format = Format.replace("%3", day)
//        Format = Format.replace("%2", month)
//        var time = FollowTimeFrom.getHours() + ":" + ("0" + FollowTimeFrom.getMinutes()).slice(-2);
//        $(".NextActTime").html((Format + " " + time + "-" + FollowTimeTo));
//        this_form.NextActDayToFlg.value = "1";
    //    }


    

    var FollowAct = $(".FollowContactlist").children("span").attr("value");

//    if (this_form.FollowDayToFlg.value == "1") {
//        FollowAct = "2"
//    }
//    if (this_form.FollowDayToFlg.value == "0") {
//        FollowAct = "1"
//    }

//    if (FollowAct == "1") {
//        $(".FollowTimeTo").css("display", "none");
//        $(".FollowTimeStart").css("display", "none");
//        $(".FollowTimeLimit").css("display", "block");
//        var Format = this_form.dateFormt.value
//        var FollowTime = this_form.FollowTimeFromSelectorTime.valueAsDate;
//        var month = FollowTime.getMonth() + 1;
//        var day = FollowTime.getDate();
//        Format = Format.replace("%3", day)
//        Format = Format.replace("%2", month)
//        var time = FollowTime.getHours() + ":" + ("0" + FollowTime.getMinutes()).slice(-2);
//        $(".FollowTime").html(Format + " " + time);
//        this_form.FollowDayToFlg.value = "0";
//    }
//    if (FollowAct == "2") {
//        $(".FollowTimeTo").css("display", "block");
//        $(".FollowTimeStart").css("display", "block");
//        $(".FollowTimeLimit").css("display", "none");
//        var Format = this_form.dateFormt.value
//        //var FollowTimeFrom = this_form.FollowTimeFromSelector.valueAsDate;
//        var FollowTimeTo = this_form.FollowTimeToSelector.valueAsDate;
//        FollowTimeTo.getUTCHours() + ":" + ("0" + FollowTimeTo.getUTCMinutes()).slice(-2);
//        var month = FollowTimeFrom.getMonth() + 1;
//        var day = FollowTimeFrom.getDate();
//        Format = Format.replace("%3", day)
//        Format = Format.replace("%2", month)
//        var time = FollowTimeFrom.getHours() + ":" + ("0" + FollowTimeFrom.getMinutes()).slice(-2);
//        $(".FollowTime").html((Format + " " + time + "-" + FollowTimeTo));
//        this_form.FollowDayToFlg.value = "1";
    //    }



    if (this_form.FollowFlg.value == "1") {
        $("#confirmContents60 .nscListBoxSet .nscListBoxSetIn .nscListBoxSetRight").css("display", "block")
    }
    else {
        $("#confirmContents60 .nscListBoxSet .nscListBoxSetIn .nscListBoxSetRight").css("display", "none")
    }

    this_form.ActTimeFromSelector_WK.value = this_form.ActTimeFromSelector.value
    this_form.ActTimeToSelector_WK.value = this_form.ActTimeToSelector.value

//    this_form.NextActTimeFromSelector_WK.value = this_form.NextActTimeFromSelector.value
//    this_form.NextActTimeFromSelectorTime_WK.value = this_form.NextActTimeFromSelectorTime.value
//    this_form.NextActTimeToSelector_WK.value = this_form.NextActTimeToSelector.value

//    this_form.FollowTimeFromSelector_WK.value = this_form.FollowTimeFromSelector.value
//    this_form.FollowTimeFromSelectorTime_WK.value = this_form.FollowTimeFromSelectorTime.value
//    this_form.FollowTimeToSelector_WK.value = this_form.FollowTimeToSelector.value

    //次回活動のアラートの初期値設定
    if (this_form.selectNextActAlert.value != "" && this_form.NextActDayToFlg.value == "1") {
        $("#scNscNextActTimeListSelDiv #AlertSelList" + this_form.selectNextActAlert.value).addClass("Selection");
        $(".NextActAletName").html($("#scNscNextActTimeListSelDiv #AlertSelList" + this_form.selectNextActAlert.value).attr("title"));
        $(".NextActAletNamePop").html($("#scNscNextActTimeListSelDiv #AlertSelList" + this_form.selectNextActAlert.value).attr("title"));
    }
    else if (this_form.selectNextActAlert.value != "" && this_form.NextActDayToFlg.value == "0") {
        $("#scNscNextActTimeListNonSelDiv #AlertNonSelList" + this_form.selectNextActAlert.value).addClass("Selection");
        $(".NextActAletName").html($("#scNscNextActTimeListNonSelDiv #AlertNonSelList" + this_form.selectNextActAlert.value).attr("title"));
        $(".NextActAletNamePop").html($("#scNscNextActTimeListNonSelDiv #AlertNonSelList" + this_form.selectNextActAlert.value).attr("title"));
    }
    else {
        $("#scNscNextActTimeListSelDiv #AlertSelList0").addClass("Selection");
        $(".NextActAletName").html($("#scNscNextActTimeListSelDiv #AlertSelList0").attr("title"));
        $(".NextActAletNamePop").html($("#scNscNextActTimeListSelDiv #AlertSelList0").attr("title"));
    }

    //フォローのアラートの初期値設定
    if (this_form.selectFollowAlert.value != "" && this_form.FollowDayToFlg.value == "1") {
        $("#scNscFollowTimeListSelDiv #AlertSelList" + this_form.selectFollowAlert.value).addClass("Selection");
        $(".FollowAletName").html($("#scNscFollowTimeListSelDiv #AlertSelList" + this_form.selectFollowAlert.value).attr("title"));
        $(".FollowAletNamePop").html($("#scNscFollowTimeListSelDiv #AlertSelList" + this_form.selectFollowAlert.value).attr("title"));
    }
    else if (this_form.selectFollowAlert.value != "" && this_form.FollowDayToFlg.value == "0") {
        $("#scNscFollowTimeListNonSelDiv #AlertNonSelList" + this_form.selectFollowAlert.value).addClass("Selection");
        $(".FollowAletName").html($("#scNscFollowTimeListNonSelDiv #AlertNonSelList" + this_form.selectFollowAlert.value).attr("title"));
        $(".FollowAletNamePop").html($("#scNscFollowTimeListNonSelDiv #AlertNonSelList" + this_form.selectFollowAlert.value).attr("title"));
    }
    else {
        $("#scNscFollowTimeListSelDiv #AlertSelList0").addClass("Selection");
        $(".FollowAletName").html($("#scNscFollowTimeListSelDiv #AlertSelList0").attr("title"));
        $(".FollowAletNamePop").html($("#scNscFollowTimeListSelDiv #AlertSelList0").attr("title"));
    }


});




// 次回活動時間を開いた時 //
$(function () {
    $("#popupTrigger2").click(function () {
        $("#NextActTimeFromSelector").val(this_form.NextActTimeFromSelector_WK.value)
        $("#NextActTimeFromSelectorTime").val(this_form.NextActTimeFromSelectorTime_WK.value)
        $("#NextActTimeToSelector").val(this_form.NextActTimeToSelector_WK.value)
    }
   );
});

// フォロー時間を開いた時 //
$(function () {
    $("#popupTrigger3").click(function () {
        $("#FollowTimeFromSelector").val(this_form.FollowTimeFromSelector_WK.value)
        $("#FollowTimeFromSelectorTime").val(this_form.FollowTimeFromSelectorTime_WK.value)
        $("#FollowTimeToSelector").val(this_form.FollowTimeToSelector_WK.value)
    }
   );
});



// 次回活動時間ポップアップキャンセルボタン //
$(function () {
    $(".scNscNextActTimeCancellButton").click(function () {
        $("#bodyFrame").trigger("click.popover");
    }
    );
});

// フォロー時間ポップアップキャンセルボタン //
$(function () {
    $(".scNscNextFollowTimeCancellButton").click(function () {
        $("#bodyFrame").trigger("click.popover");
    }
    );
});



// 次回活動時間ポップアップ完了ボタン //
$(function () {
    $(".scNscNextActTimeCompletionButton").click(function () {
        var NextAct = $(".NextActContactlist.Selection").children("span").attr("value").split("_");
        if (NextAct[1] == "1") {
            var Format = this_form.dateFormt.value;
            var FollowTimeFrom = this_form.NextActTimeFromSelectorTime.valueAsDate;
            var month = FollowTimeFrom.getMonth() + 1;
            var day = FollowTimeFrom.getDate();
            Format = Format.replace("%3", day);
            Format = Format.replace("%2", month);
            var time = FollowTimeFrom.getHours() + ":" + ("0" + FollowTimeFrom.getMinutes()).slice(-2)
            $(".NextActTime").html((Format + " " + time));
        }
        if (NextAct[1] == "2") {
            var Format = this_form.dateFormt.value
            var FollowTimeFrom = this_form.NextActTimeFromSelector.valueAsDate;
            var FollowTimeTo = this_form.NextActTimeToSelector.valueAsDate;
            FollowTimeTo = FollowTimeTo.getUTCHours() + ":" + ("0" + FollowTimeTo.getUTCMinutes()).slice(-2)
            var month = FollowTimeFrom.getMonth() + 1;
            var day = FollowTimeFrom.getDate();
            Format = Format.replace("%3", day);
            Format = Format.replace("%2", month);
            var time = FollowTimeFrom.getHours() + ":" + ("0" + FollowTimeFrom.getMinutes()).slice(-2)
            $(".NextActTime").html((Format + " " + time + "-" + FollowTimeTo));
        }

        this_form.NextActTimeFromSelector_WK.value = $("#NextActTimeFromSelector").attr("value")
        this_form.NextActTimeFromSelectorTime_WK.value = $("#NextActTimeFromSelectorTime").attr("value")
        this_form.NextActTimeToSelector_WK.value = $("#NextActTimeToSelector").attr("value")

        $("#bodyFrame").trigger("click.popover");
    }
    );
    // 今回活動時間ポップアップ開く //
    $("#NextActpopTri").click(function () {
        $("#NextActTimeFromSelectorTime").val(this_form.NextActTimeFromSelectorTime_WK.value);
        $("#NextActTimeToSelector").val(this_form.NextActTimeToSelector_WK.value);
        $("#NextActTimeFromSelector").val(this_form.NextActTimeFromSelector_WK.value);
        this_form.selectNextActAlertWK.value = this_form.selectNextActAlert.value;
    }
    );
});

// フォロー時間ポップアップ完了ボタン //
$(function () {
    $(".scNscNextFollowTimeCompletionButton").click(function () {
        var NextAct = $(".FollowContactlist.Selection").children("span").attr("value")
        if (NextAct == "1") {
            var Format = this_form.dateFormt.value;
            var FollowTimeFrom = this_form.FollowTimeFromSelectorTime.valueAsDate;
            var month = FollowTimeFrom.getMonth() + 1;
            var day = FollowTimeFrom.getDate();
            Format = Format.replace("%3", day)
            Format = Format.replace("%2", month)
            var time = FollowTimeFrom.getHours() + ":" + ("0" + FollowTimeFrom.getMinutes()).slice(-2);
            $(".FollowTime").html((Format + " " + time));
        }
        if (NextAct == "2") {
            var Format = this_form.dateFormt.value;
            var FollowTimeFrom = this_form.FollowTimeFromSelector.valueAsDate;
            var FollowTimeTo = this_form.FollowTimeToSelector.valueAsDate;
            FollowTimeTo = FollowTimeTo.getUTCHours() + ":" + ("0" + FollowTimeTo.getUTCMinutes()).slice(-2);
            var month = FollowTimeFrom.getMonth() + 1;
            var day = FollowTimeFrom.getDate();
            Format = Format.replace("%3", day)
            Format = Format.replace("%2", month)
            var time = FollowTimeFrom.getHours() + ":" + ("0" + FollowTimeFrom.getMinutes()).slice(-2);
            $(".FollowTime").html((Format + " " + time + "-" + FollowTimeTo));
        }
        this_form.FollowTimeFromSelector_WK.value = $("#FollowTimeFromSelector").attr("value")
        this_form.FollowTimeFromSelectorTime_WK.value = $("#FollowTimeFromSelectorTime").attr("value")
        this_form.FollowTimeToSelector_WK.value = $("#FollowTimeToSelector").attr("value")
        $("#bodyFrame").trigger("click.popover");
    }
    );
    // 今回活動時間ポップアップ開く //
    $("#FollowpopTri").click(function () {
        $("#FollowTimeFromSelectorTime").val(this_form.FollowTimeFromSelectorTime_WK.value);
        $("#FollowTimeToSelector").val(this_form.FollowTimeToSelector_WK.value);
        $("#FollowTimeFromSelector").val(this_form.FollowTimeFromSelector_WK.value);
        this_form.selectFollowAlertWK.value = this_form.selectFollowAlert.value;
    }
    );
});

// 対応SC欄 //
$(function () {
    $(".Stafflist").click(function (e) {
        $(".scNscStaffName").html($(this).html());
        $(".Stafflist").removeClass("Selection");
        $(this).addClass("Selection");
        this_form.selectStaff.value = $(this).children("span").attr("value");
        $("#bodyFrame").trigger("click.popover");
    });
    $(".scNscStaffCancellButton").click(function () {
        $("#bodyFrame").trigger("click.popover");
    });
});

// 分類欄(活動内容) //
$(function () {
    $(".ActContactlist").click(function (e) {
        if ($(this).children("span").attr("value") == "1") {
            $("#confirmContents60 .nscListBoxSet .nscListBoxSetIn .nscListBoxSetRight2").animate({ width: "show" }, 300);
            this_form.ProcessFlg.value = "1"
        }
        else {
            $("#confirmContents60 .nscListBoxSet .nscListBoxSetIn .nscListBoxSetRight2").animate({ width: "hide" }, 300);
            this_form.ProcessFlg.value = "0"
        }
        $(".scNscActContactName").html($(this).html());
        $(".ActContactlist").removeClass("Selection");
        $(this).addClass("Selection");
        this_form.selectActContact.value = $(this).attr("value");
        $("#bodyFrame").trigger("click.popover");
    });
    $(".scNscActContactCancellButton").click(function () {
        $("#bodyFrame").trigger("click.popover");
    });
});



// カタログ //
$(function () {
    $(".Cataloglist").live("click", function (e) {
        var listname = "#Cataloglist";
        var listvalue;
        var i;
        var j;
        var sel = $(this).attr("id");
        var seledary
        var seledarydetail
        var seledarycreate = ""
        sel = sel.replace("Cataloglist", "");
        var seled = this_form.selectActCatalogWK.value;
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
        this_form.selectActCatalogWK.value = seledarycreate;

    });
    $(".scNscCatalogCancellButton").click(function () {
        $("#bodyFrame").trigger("click.popover");
    });
    $(".scNscCatalogCompletionButton").click(function () {
        this_form.selectActCatalog.value = this_form.selectActCatalogWK.value;
        var selfalg = "0";
        var seled = this_form.selectActCatalog.value;
        var seledary = seled.split(";");
        for (i = 0; i < seledary.length - 1; i++) {
            seledarydetail = seledary[i].split(",");
            if (seledarydetail[1] == "1") {
                selfalg = "1";
            }
        }
        if (selfalg == "1") {
            $("#confirmContents60 .nscListBoxSet .nscListBoxSetIn .nscListIcnAset .nscListIcnA1").css("text-shadow", "0px -1px 1px #000");
            $("#confirmContents60 .nscListBoxSet .nscListBoxSetIn .nscListIcnAset .nscListIcnA1").css("color", "#FFF");
            $("#confirmContents60 .nscListBoxSet .nscListBoxSetIn .nscListIcnAset .nscListIcnA1").css("text-shadow", "none");
            $("#confirmContents60 .nscListBoxSet .nscListBoxSetIn .nscListIcnAset .nscListIcnA1").css("background", "url(" + this_form.CatalogSelPath.value + ") center 9px no-repeat,-webkit-gradient(linear, left top, left bottom,color-stop(0, #7b97ed),color-stop(50%, #416ae5),color-stop(51%, #1e4ee1),color-stop(100%, #1e4ee1))");
            this_form.HD_nscListIcnA1.value = "1";
        } else {
            $("#confirmContents60 .nscListBoxSet .nscListBoxSetIn .nscListIcnAset .nscListIcnA1").css("text-shadow", "0px -1px 1px #FFF");
            $("#confirmContents60 .nscListBoxSet .nscListBoxSetIn .nscListIcnAset .nscListIcnA1").css("color", "#808080");
            $("#confirmContents60 .nscListBoxSet .nscListBoxSetIn .nscListIcnAset .nscListIcnA1").css("text-shadow", "block");
            $("#confirmContents60 .nscListBoxSet .nscListBoxSetIn .nscListIcnAset .nscListIcnA1").css("background", "url(" + this_form.CatalogNonSelPath.value + ") center 9px no-repeat,-webkit-gradient(linear, left top, left bottom,color-stop(0, #fbfbfb),color-stop(100%, #cacaca))");
            this_form.HD_nscListIcnA1.value = "0";
        }
        $("#bodyFrame").trigger("click.popover");
    });
});
$(function () {
    $("#confirmContents60 .nscListBoxSet .nscListBoxSetIn .nscListIcnAset .nscListIcnA1").click(function () {
        this_form.selectActCatalogWK.value = this_form.selectActCatalog.value;
        var listname = "#Cataloglist";
        var listvalue;
        var i;
        var seledary
        var seledarydetail
        var seled = this_form.selectActCatalog.value;
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
        var listname = "#TestDrivelist";
        var listvalue;
        var i;
        var j;
        var sel = $(this).attr("id");
        var seledary
        var seledarydetail
        var seledarycreate = ""
        sel = sel.replace("TestDrivelist", "");
        var seled = this_form.selectActTestDriveWK.value;
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
        this_form.selectActTestDriveWK.value = seledarycreate;

    });
    $(".scNscTestDriveCancellButton").click(function () {
        $("#bodyFrame").trigger("click.popover");
    });
    $(".scNscTestDriveCompletionButton").click(function () {
        this_form.selectActTestDrive.value = this_form.selectActTestDriveWK.value;
        var selfalg = "0";
        var seled = this_form.selectActTestDrive.value;
        var seledary = seled.split(";");
        for (i = 0; i < seledary.length - 1; i++) {
            seledarydetail = seledary[i].split(",");
            if (seledarydetail[1] == "1") {
                selfalg = "1";
            }
        }
        if (selfalg == "1") {
            $("#confirmContents60 .nscListBoxSet .nscListBoxSetIn .nscListIcnAset .nscListIcnA2").css("text-shadow", "0px -1px 1px #000");
            $("#confirmContents60 .nscListBoxSet .nscListBoxSetIn .nscListIcnAset .nscListIcnA2").css("color", "#FFF");
            $("#confirmContents60 .nscListBoxSet .nscListBoxSetIn .nscListIcnAset .nscListIcnA2").css("text-shadow", "none");
            $("#confirmContents60 .nscListBoxSet .nscListBoxSetIn .nscListIcnAset .nscListIcnA2").css("background", "url(" + this_form.TestDriveSelPath.value + ") center 9px no-repeat,-webkit-gradient(linear, left top, left bottom,color-stop(0, #7b97ed),color-stop(50%, #416ae5),color-stop(51%, #1e4ee1),color-stop(100%, #1e4ee1))");
            this_form.HD_nscListIcnA1.value = "1";
        } else {
            $("#confirmContents60 .nscListBoxSet .nscListBoxSetIn .nscListIcnAset .nscListIcnA2").css("text-shadow", "0px -1px 1px #FFF");
            $("#confirmContents60 .nscListBoxSet .nscListBoxSetIn .nscListIcnAset .nscListIcnA2").css("color", "#808080");
            $("#confirmContents60 .nscListBoxSet .nscListBoxSetIn .nscListIcnAset .nscListIcnA2").css("text-shadow", "block");
            $("#confirmContents60 .nscListBoxSet .nscListBoxSetIn .nscListIcnAset .nscListIcnA2").css("background", "url(" + this_form.TestDriveNonSelPath.value + ") center 9px no-repeat,-webkit-gradient(linear, left top, left bottom,color-stop(0, #fbfbfb),color-stop(100%, #cacaca))");
            this_form.HD_nscListIcnA1.value = "0";
        }
        $("#bodyFrame").trigger("click.popover");
    });
});
$(function () {
    $("#confirmContents60 .nscListBoxSet .nscListBoxSetIn .nscListIcnAset .nscListIcnA2").click(function () {
        this_form.selectActTestDriveWK.value = this_form.selectActTestDrive.value;
        var listname = "#TestDrivelist";
        var listvalue;
        var i;
        var seledary
        var seledarydetail
        var seled = this_form.selectActTestDrive.value;
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
    $("#confirmContents60 .nscListBoxSet .nscListBoxSetIn .nscListIcnAset .nscListIcnA3").live("click", function () {
        if (this_form.selectActAssesment.value == "1") {
            $("#bodyFrame").trigger("click.popover");
            this_form.selectActAssesment.value = "0";
            this_form.selectActAssesmentWK.value = "0";
            $(this).css("text-shadow", "0px -1px 1px #FFF");
            $(this).css("color", "#808080");
            $(this).css("text-shadow", "block");
            $(this).css("background", "url(" + this_form.AssesmentNonSelPath.value + ") center 9px no-repeat,-webkit-gradient(linear, left top, left bottom,color-stop(0, #fbfbfb),color-stop(100%, #cacaca))");
        }
        else {
            $("#bodyFrame").trigger("click.popover");
            this_form.selectActAssesment.value = "1";
            this_form.selectActAssesmentWK.value = "1";
            $(this).css("text-shadow", "0px -1px 1px #000");
            $(this).css("color", "#FFF");
            $(this).css("text-shadow", "none");
            $(this).css("background", "url(" + this_form.AssesmentSelPath.value + ") center 9px no-repeat,-webkit-gradient(linear, left top, left bottom,color-stop(0, #7b97ed),color-stop(50%, #416ae5),color-stop(51%, #1e4ee1),color-stop(100%, #1e4ee1))");
        }
    }
    );
});

// 見積り //
$(function () {
    $(".Valuationlist").live("click", function (e) {
        var listname = "#Valuationlist";
        var listvalue;
        var i;
        var j;
        var sel = $(this).attr("id");
        var seledary
        var seledarydetail
        var seledarycreate = ""
        sel = sel.replace("Valuationlist", "");
        var seled = this_form.selectActValuationWK.value;
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
        this_form.selectActValuationWK.value = seledarycreate;

    });
    $(".scNscValuationCancellButton").click(function () {
        $("#bodyFrame").trigger("click.popover");
    });
    $(".scNscValuationCompletionButton").click(function () {
        this_form.selectActValuation.value = this_form.selectActValuationWK.value;
        var selfalg = "0";
        var seled = this_form.selectActValuation.value;
        var seledary = seled.split(";");
        for (i = 0; i < seledary.length - 1; i++) {
            seledarydetail = seledary[i].split(",");
            if (seledarydetail[1] == "1") {
                selfalg = "1";
            }
        }
        if (selfalg == "1") {
            $("#confirmContents60 .nscListBoxSet .nscListBoxSetIn .nscListIcnAset .nscListIcnA4").css("text-shadow", "0px -1px 1px #000");
            $("#confirmContents60 .nscListBoxSet .nscListBoxSetIn .nscListIcnAset .nscListIcnA4").css("color", "#FFF");
            $("#confirmContents60 .nscListBoxSet .nscListBoxSetIn .nscListIcnAset .nscListIcnA4").css("text-shadow", "none");
            $("#confirmContents60 .nscListBoxSet .nscListBoxSetIn .nscListIcnAset .nscListIcnA4").css("background", "url(" + this_form.ValuationSelPath.value + ") center 9px no-repeat,-webkit-gradient(linear, left top, left bottom,color-stop(0, #7b97ed),color-stop(50%, #416ae5),color-stop(51%, #1e4ee1),color-stop(100%, #1e4ee1))");
            this_form.HD_nscListIcnA1.value = "1";
        } else {
            $("#confirmContents60 .nscListBoxSet .nscListBoxSetIn .nscListIcnAset .nscListIcnA4").css("text-shadow", "0px -1px 1px #FFF");
            $("#confirmContents60 .nscListBoxSet .nscListBoxSetIn .nscListIcnAset .nscListIcnA4").css("color", "#808080");
            $("#confirmContents60 .nscListBoxSet .nscListBoxSetIn .nscListIcnAset .nscListIcnA4").css("text-shadow", "block");
            $("#confirmContents60 .nscListBoxSet .nscListBoxSetIn .nscListIcnAset .nscListIcnA4").css("background", "url(" + this_form.ValuationNonSelPath.value + ") center 9px no-repeat,-webkit-gradient(linear, left top, left bottom,color-stop(0, #fbfbfb),color-stop(100%, #cacaca))");
            this_form.HD_nscListIcnA1.value = "0";
        }
        $("#bodyFrame").trigger("click.popover");
    });
});
$(function () {
    $("#confirmContents60 .nscListBoxSet .nscListBoxSetIn .nscListIcnAset .nscListIcnA4").click(
        function () {
            this_form.selectActValuationWK.value = this_form.selectActValuation.value;
            var listname = "#Valuationlist";
            var listvalue;
            var i;
            var seledary
            var seledarydetail
            var seled = this_form.selectActValuation.value;
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


// 成約車種 //
$(function () {
    $(".SelectedCarlist").live("click", function (e) {
        var listname = "#SelectedCarlist";
        var listvalue;
        var i;
        var j;
        var sel = $(this).attr("id");
        var seledary
        var seledarydetail
        var seledarycreate = ""
        sel = sel.replace("SelectedCarlist", "");
        var seled = this_form.selectSelSeries.value;
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
        this_form.selectSelSeries.value = seledarycreate;
    });
});





//スクロール化
$(function () {
    $("#SuccessSelectedCar").fingerScroll();
    //$("#confirmContents60 .nscListBoxSet .nscListBoxSetIn .nscListBoxSetLeft").css("width", $("#SuccessSelectedCar ul").width() + 5);
    $(".scNscStaffListBox").fingerScroll();
    $(".scNscActContactListBox").fingerScroll();
    $(".scNscNextActContactListBox").fingerScroll();
    $(".scNscFollowContactListBox").fingerScroll();
    $(".scNscTestDriveListBox").fingerScroll();
    $(".scNscCatalogListBox").fingerScroll();
    $(".scNscValuationListBox").fingerScroll();
});





/*    Walk-inボタン    */
$(function () {
    var h = $("#confirmContents60 .nscListBoxSet.HeightB").height();
    $("#confirmContents60 .nscListBoxSet .nscListBoxSetIn .nscListIcnBset .nscListIcnB1").click(
        function () {
            if (this_form.HD_nscListIcnB1.value == "0") {
                $(this).css("text-shadow", "0px -1px 1px #000");
                $(this).css("color", "#FFF");
                $(this).css("text-shadow", "none");
                $(this).css("background", "url(../Styles/Images/SC3080201/nsc60icn2aOver.png) center 9px no-repeat,-webkit-gradient(linear, left top, left bottom,color-stop(0, #7b97ed),color-stop(50%, #416ae5),color-stop(51%, #1e4ee1),color-stop(100%, #1e4ee1))");

                /* Prospect → Walk-inの場合 */
                if (this_form.HD_nscListIcnB2.value == "1") {
                    $("#confirmContents60 .nscListBoxSet .nscListBoxSetIn .nscListIcnBset .nscListIcnB2").css("text-shadow", "0px -1px 1px #FFF");
                    $("#confirmContents60 .nscListBoxSet .nscListBoxSetIn .nscListIcnBset .nscListIcnB2").css("color", "#808080");
                    $("#confirmContents60 .nscListBoxSet .nscListBoxSetIn .nscListIcnBset .nscListIcnB2").css("text-shadow", "block");
                    $("#confirmContents60 .nscListBoxSet .nscListBoxSetIn .nscListIcnBset .nscListIcnB2").css("background", "url(../Styles/Images/SC3080201/nsc60icn2b.png) center 9px no-repeat,-webkit-gradient(linear, left top, left bottom,color-stop(0, #fbfbfb),color-stop(100%, #cacaca))");
                    this_form.HD_nscListIcnB2.value = "0"
                }

                /* Hot → Walk-inの場合 */
                else if (this_form.HD_nscListIcnB3.value == "1") {
                    $("#confirmContents60 .nscListBoxSet .nscListBoxSetIn .nscListIcnBset .nscListIcnB3").css("text-shadow", "0px -1px 1px #FFF");
                    $("#confirmContents60 .nscListBoxSet .nscListBoxSetIn .nscListIcnBset .nscListIcnB3").css("color", "#808080");
                    $("#confirmContents60 .nscListBoxSet .nscListBoxSetIn .nscListIcnBset .nscListIcnB3").css("text-shadow", "block");
                    $("#confirmContents60 .nscListBoxSet .nscListBoxSetIn .nscListIcnBset .nscListIcnB3").css("background", "url(../Styles/Images/SC3080201/nsc60icn2c.png) center 9px no-repeat,-webkit-gradient(linear, left top, left bottom,color-stop(0, #fbfbfb),color-stop(100%, #cacaca))");
                    this_form.HD_nscListIcnB3.value = "0"
                }

                /* Success → Walk-inの場合 */
                else if (this_form.HD_nscListIcnB4.value == "1") {
                    $("#SuccessSelectedCar .scroll-inner").css("-webkit-transition-property","initial")
                    $("#confirmContents60 .nscListBoxSet .nscListBoxSetIn .nscListIcnBset .nscListIcnB4").css("text-shadow", "0px -1px 1px #FFF");
                    $("#confirmContents60 .nscListBoxSet .nscListBoxSetIn .nscListIcnBset .nscListIcnB4").css("color", "#808080");
                    $("#confirmContents60 .nscListBoxSet .nscListBoxSetIn .nscListIcnBset .nscListIcnB4").css("text-shadow", "block");
                    $("#confirmContents60 .nscListBoxSet .nscListBoxSetIn .nscListIcnBset .nscListIcnB4").css("background", "url(../Styles/Images/SC3080201/nsc60icn2d.png) center 9px no-repeat,-webkit-gradient(linear, left top, left bottom,color-stop(0, #fbfbfb),color-stop(100%, #cacaca))");
                    $("#confirmContents60 .nscListBoxSet.HeightC").css({ height: h }).show(0).animate({ height: 0 }, 300, function () {
                        $("#confirmContents60 .nscListBoxSet.HeightC").hide();
                        $("#confirmContents60 .nscListBoxSet.HeightB").css({ height: 0 }).show(0).animate({ height: h }, 300);
                    });
                    this_form.HD_nscListIcnB4.value = "0"
                }

                /* Give-up → Walk-inの場合 */
                else if (this_form.HD_nscListIcnB5.value == "1") {
                    $("#confirmContents60 .nscListBoxSet .nscListBoxSetIn .nscListIcnBset .nscListIcnB5").css("text-shadow", "0px -1px 1px #FFF");
                    $("#confirmContents60 .nscListBoxSet .nscListBoxSetIn .nscListIcnBset .nscListIcnB5").css("color", "#808080");
                    $("#confirmContents60 .nscListBoxSet .nscListBoxSetIn .nscListIcnBset .nscListIcnB5").css("text-shadow", "block");
                    $("#confirmContents60 .nscListBoxSet .nscListBoxSetIn .nscListIcnBset .nscListIcnB5").css("background", "url(../Styles/Images/SC3080201/nsc60icn2e.png) center 9px no-repeat,-webkit-gradient(linear, left top, left bottom,color-stop(0, #fbfbfb),color-stop(100%, #cacaca))");
                    $("#confirmContents60 .nscListBoxSet.HeightD").css({ height: h }).show(0).animate({ height: 0 }, 300, function () {
                        $("#confirmContents60 .nscListBoxSet.HeightD").hide();
                        $("#confirmContents60 .nscListBoxSet.HeightB").css({ height: 0 }).show(0).animate({ height: h }, 300);
                    });
                    this_form.HD_nscListIcnB5.value = "0"
                }
                else {
                    $("#confirmContents60 .nscListBoxSet.HeightB").css({ height: 0 }).show(0).animate({ height: h }, 300);
                }
                this_form.HD_nscListIcnB1.value = "1";
                $("#selectActRlst").val("1")
            }
        }
    );
});

// Prospectボタン //
$(function () {
    var h = $("#confirmContents60 .nscListBoxSet.HeightB").height();
    $("#confirmContents60 .nscListBoxSet .nscListBoxSetIn .nscListIcnBset .nscListIcnB2").click(
        function () {
            if (this_form.HD_nscListIcnB2.value == "0") {
                $(this).css("text-shadow", "0px -1px 1px #000");
                $(this).css("color", "#FFF");
                $(this).css("text-shadow", "none");
                $(this).css("background", "url(../Styles/Images/SC3080201/nsc60icn2bOver.png) center 9px no-repeat,-webkit-gradient(linear, left top, left bottom,color-stop(0, #7b97ed),color-stop(50%, #416ae5),color-stop(51%, #1e4ee1),color-stop(100%, #1e4ee1))");

                /* Walk-in → Prospectの場合 */
                if (this_form.HD_nscListIcnB1.value == "1") {
                    $("#confirmContents60 .nscListBoxSet .nscListBoxSetIn .nscListIcnBset .nscListIcnB1").css("text-shadow", "0px -1px 1px #FFF");
                    $("#confirmContents60 .nscListBoxSet .nscListBoxSetIn .nscListIcnBset .nscListIcnB1").css("color", "#808080");
                    $("#confirmContents60 .nscListBoxSet .nscListBoxSetIn .nscListIcnBset .nscListIcnB1").css("text-shadow", "block");
                    $("#confirmContents60 .nscListBoxSet .nscListBoxSetIn .nscListIcnBset .nscListIcnB1").css("background", "url(../Styles/Images/SC3080201/nsc60icn2a.png) center 9px no-repeat,-webkit-gradient(linear, left top, left bottom,color-stop(0, #fbfbfb),color-stop(100%, #cacaca))");
                    this_form.HD_nscListIcnB1.value = "0"
                }

                /* Hot → Prospectの場合 */
                else if (this_form.HD_nscListIcnB3.value == "1") {
                    $("#confirmContents60 .nscListBoxSet .nscListBoxSetIn .nscListIcnBset .nscListIcnB3").css("text-shadow", "0px -1px 1px #FFF");
                    $("#confirmContents60 .nscListBoxSet .nscListBoxSetIn .nscListIcnBset .nscListIcnB3").css("color", "#808080");
                    $("#confirmContents60 .nscListBoxSet .nscListBoxSetIn .nscListIcnBset .nscListIcnB3").css("text-shadow", "block");
                    $("#confirmContents60 .nscListBoxSet .nscListBoxSetIn .nscListIcnBset .nscListIcnB3").css("background", "url(../Styles/Images/SC3080201/nsc60icn2c.png) center 9px no-repeat,-webkit-gradient(linear, left top, left bottom,color-stop(0, #fbfbfb),color-stop(100%, #cacaca))");
                    this_form.HD_nscListIcnB3.value = "0"
                }

                /* Success → Prospectの場合 */
                else if (this_form.HD_nscListIcnB4.value == "1") {
                    $("#SuccessSelectedCar .scroll-inner").css("-webkit-transition-property","initial")
                    $("#confirmContents60 .nscListBoxSet .nscListBoxSetIn .nscListIcnBset .nscListIcnB4").css("text-shadow", "0px -1px 1px #FFF");
                    $("#confirmContents60 .nscListBoxSet .nscListBoxSetIn .nscListIcnBset .nscListIcnB4").css("color", "#808080");
                    $("#confirmContents60 .nscListBoxSet .nscListBoxSetIn .nscListIcnBset .nscListIcnB4").css("text-shadow", "block");
                    $("#confirmContents60 .nscListBoxSet .nscListBoxSetIn .nscListIcnBset .nscListIcnB4").css("background", "url(../Styles/Images/SC3080201/nsc60icn2d.png) center 9px no-repeat,-webkit-gradient(linear, left top, left bottom,color-stop(0, #fbfbfb),color-stop(100%, #cacaca))");
                    $("#confirmContents60 .nscListBoxSet.HeightC").css({ height: h }).show(0).animate({ height: 0 }, 300, function () {
                        $("#confirmContents60 .nscListBoxSet.HeightC").hide();
                        $("#confirmContents60 .nscListBoxSet.HeightB").css({ height: 0 }).show(0).animate({ height: h }, 300);
                    });
                    this_form.HD_nscListIcnB4.value = "0"
                }

                /* Give-up → Prospectの場合 */
                else if (this_form.HD_nscListIcnB5.value == "1") {
                    $("#confirmContents60 .nscListBoxSet .nscListBoxSetIn .nscListIcnBset .nscListIcnB5").css("text-shadow", "0px -1px 1px #FFF");
                    $("#confirmContents60 .nscListBoxSet .nscListBoxSetIn .nscListIcnBset .nscListIcnB5").css("color", "#808080");
                    $("#confirmContents60 .nscListBoxSet .nscListBoxSetIn .nscListIcnBset .nscListIcnB5").css("text-shadow", "block");
                    $("#confirmContents60 .nscListBoxSet .nscListBoxSetIn .nscListIcnBset .nscListIcnB5").css("background", "url(../Styles/Images/SC3080201/nsc60icn2e.png) center 9px no-repeat,-webkit-gradient(linear, left top, left bottom,color-stop(0, #fbfbfb),color-stop(100%, #cacaca))");
                    $("#confirmContents60 .nscListBoxSet.HeightD").css({ height: h }).show(0).animate({ height: 0 }, 300, function () {
                        $("#confirmContents60 .nscListBoxSet.HeightD").hide();
                        $("#confirmContents60 .nscListBoxSet.HeightB").css({ height: 0 }).show(0).animate({ height: h }, 300);
                    });
                    this_form.HD_nscListIcnB5.value = "0"
                }
                else {
                    $("#confirmContents60 .nscListBoxSet.HeightB").css({ height: 0 }).show(0).animate({ height: h }, 300);
                }
                this_form.HD_nscListIcnB2.value = "1";
                $("#selectActRlst").val("2")
            }
        }
    );
});

/*    Hotボタン    */
$(function () {
    var h = $("#confirmContents60 .nscListBoxSet.HeightB").height();
    $("#confirmContents60 .nscListBoxSet .nscListBoxSetIn .nscListIcnBset .nscListIcnB3").click(
        function () {
            if (this_form.HD_nscListIcnB3.value == "0") {
                $(this).css("text-shadow", "0px -1px 1px #000");
                $(this).css("color", "#FFF");
                $(this).css("text-shadow", "none");
                $(this).css("background", "url(../Styles/Images/SC3080201/nsc60icn2cOver.png) center 9px no-repeat,-webkit-gradient(linear, left top, left bottom,color-stop(0, #7b97ed),color-stop(50%, #416ae5),color-stop(51%, #1e4ee1),color-stop(100%, #1e4ee1))");

                /* Walk-in → Hotの場合 */
                if (this_form.HD_nscListIcnB1.value == "1") {
                    $("#confirmContents60 .nscListBoxSet .nscListBoxSetIn .nscListIcnBset .nscListIcnB1").css("text-shadow", "0px -1px 1px #FFF");
                    $("#confirmContents60 .nscListBoxSet .nscListBoxSetIn .nscListIcnBset .nscListIcnB1").css("color", "#808080");
                    $("#confirmContents60 .nscListBoxSet .nscListBoxSetIn .nscListIcnBset .nscListIcnB1").css("text-shadow", "block");
                    $("#confirmContents60 .nscListBoxSet .nscListBoxSetIn .nscListIcnBset .nscListIcnB1").css("background", "url(../Styles/Images/SC3080201/nsc60icn2a.png) center 9px no-repeat,-webkit-gradient(linear, left top, left bottom,color-stop(0, #fbfbfb),color-stop(100%, #cacaca))");
                    this_form.HD_nscListIcnB1.value = "0"
                }

                /* Prospect → Hotの場合 */
                else if (this_form.HD_nscListIcnB2.value == "1") {
                    $("#confirmContents60 .nscListBoxSet .nscListBoxSetIn .nscListIcnBset .nscListIcnB2").css("text-shadow", "0px -1px 1px #FFF");
                    $("#confirmContents60 .nscListBoxSet .nscListBoxSetIn .nscListIcnBset .nscListIcnB2").css("color", "#808080");
                    $("#confirmContents60 .nscListBoxSet .nscListBoxSetIn .nscListIcnBset .nscListIcnB2").css("text-shadow", "block");
                    $("#confirmContents60 .nscListBoxSet .nscListBoxSetIn .nscListIcnBset .nscListIcnB2").css("background", "url(../Styles/Images/SC3080201/nsc60icn2b.png) center 9px no-repeat,-webkit-gradient(linear, left top, left bottom,color-stop(0, #fbfbfb),color-stop(100%, #cacaca))");
                    this_form.HD_nscListIcnB2.value = "0"
                }

                /* Success → Hotの場合 */
                else if (this_form.HD_nscListIcnB4.value == "1") {
                    $("#SuccessSelectedCar .scroll-inner").css("-webkit-transition-property","initial")
                    $("#confirmContents60 .nscListBoxSet .nscListBoxSetIn .nscListIcnBset .nscListIcnB4").css("text-shadow", "0px -1px 1px #FFF");
                    $("#confirmContents60 .nscListBoxSet .nscListBoxSetIn .nscListIcnBset .nscListIcnB4").css("color", "#808080");
                    $("#confirmContents60 .nscListBoxSet .nscListBoxSetIn .nscListIcnBset .nscListIcnB4").css("text-shadow", "block");
                    $("#confirmContents60 .nscListBoxSet .nscListBoxSetIn .nscListIcnBset .nscListIcnB4").css("background", "url(../Styles/Images/SC3080201/nsc60icn2d.png) center 9px no-repeat,-webkit-gradient(linear, left top, left bottom,color-stop(0, #fbfbfb),color-stop(100%, #cacaca))");
                    $("#confirmContents60 .nscListBoxSet.HeightC").css({ height: h }).show(0).animate({ height: 0 }, 300, function () {
                        $("#confirmContents60 .nscListBoxSet.HeightC").hide();
                        $("#confirmContents60 .nscListBoxSet.HeightB").css({ height: 0 }).show(0).animate({ height: h }, 300);
                    });
                    this_form.HD_nscListIcnB4.value = "0"
                }

                /* Give-up → Hotの場合 */
                else if (this_form.HD_nscListIcnB5.value == "1") {
                    $("#confirmContents60 .nscListBoxSet .nscListBoxSetIn .nscListIcnBset .nscListIcnB5").css("text-shadow", "0px -1px 1px #FFF");
                    $("#confirmContents60 .nscListBoxSet .nscListBoxSetIn .nscListIcnBset .nscListIcnB5").css("color", "#808080");
                    $("#confirmContents60 .nscListBoxSet .nscListBoxSetIn .nscListIcnBset .nscListIcnB5").css("text-shadow", "block");
                    $("#confirmContents60 .nscListBoxSet .nscListBoxSetIn .nscListIcnBset .nscListIcnB5").css("background", "url(../Styles/Images/SC3080201/nsc60icn2e.png) center 9px no-repeat,-webkit-gradient(linear, left top, left bottom,color-stop(0, #fbfbfb),color-stop(100%, #cacaca))");
                    $("#confirmContents60 .nscListBoxSet.HeightD").css({ height: h }).show(0).animate({ height: 0 }, 300, function () {
                        $("#confirmContents60 .nscListBoxSet.HeightD").hide();
                        $("#confirmContents60 .nscListBoxSet.HeightB").css({ height: 0 }).show(0).animate({ height: h }, 300);
                    });
                    this_form.HD_nscListIcnB5.value = "0"
                }
                else {
                    $("#confirmContents60 .nscListBoxSet.HeightB").css({ height: 0 }).show(0).animate({ height: h }, 300);
                }
                this_form.HD_nscListIcnB3.value = "1";
                $("#selectActRlst").val("3")
            }
        }
    );
});

/*    Successボタン    */
$(function () {
    var h = $("#confirmContents60 .nscListBoxSet.HeightC").height();

    $("#confirmContents60 .nscListBoxSet .nscListBoxSetIn .nscListIcnBset .nscListIcnB4").click(

        function () {
            if (this_form.HD_nscListIcnB4.value == "0") {
                $(this).css("text-shadow", "0px -1px 1px #000");
                $(this).css("color", "#FFF");
                $(this).css("text-shadow", "none");
                $(this).css("background", "url(../Styles/Images/SC3080201/nsc60icn2dOver.png) center 9px no-repeat,-webkit-gradient(linear, left top, left bottom,color-stop(0, #7b97ed),color-stop(50%, #416ae5),color-stop(51%, #1e4ee1),color-stop(100%, #1e4ee1))");

                var callback = function () {
                    setTimeout(function () {
                        
                        $("#SuccessSelectedCar .scroll-inner").css("-webkit-transition-property","none")
                        
                    }, 0);
                };

                /* Walk-in → Successの場合 */
                if (this_form.HD_nscListIcnB1.value == "1") {
                    $("#confirmContents60 .nscListBoxSet .nscListBoxSetIn .nscListIcnBset .nscListIcnB1").css("text-shadow", "0px -1px 1px #FFF");
                    $("#confirmContents60 .nscListBoxSet .nscListBoxSetIn .nscListIcnBset .nscListIcnB1").css("color", "#808080");
                    $("#confirmContents60 .nscListBoxSet .nscListBoxSetIn .nscListIcnBset .nscListIcnB1").css("text-shadow", "block");
                    $("#confirmContents60 .nscListBoxSet .nscListBoxSetIn .nscListIcnBset .nscListIcnB1").css("background", "url(../Styles/Images/SC3080201/nsc60icn2a.png) center 9px no-repeat,-webkit-gradient(linear, left top, left bottom,color-stop(0, #fbfbfb),color-stop(100%, #cacaca))");
                    $("#confirmContents60 .nscListBoxSet.HeightB").css({ height: h }).show(0).animate({ height: 0 }, 300, function () {
                        $("#confirmContents60 .nscListBoxSet.HeightB").hide(0);
                        $("#confirmContents60 .nscListBoxSet.HeightC").css({ height: 0 }).show(0).animate({ height: h }, 300, callback);
                    });
                    this_form.HD_nscListIcnB1.value = "0"
                }

                /* Prospect → Successの場合  */
                else if (this_form.HD_nscListIcnB2.value == "1") {
                    $("#confirmContents60 .nscListBoxSet .nscListBoxSetIn .nscListIcnBset .nscListIcnB2").css("text-shadow", "0px -1px 1px #FFF");
                    $("#confirmContents60 .nscListBoxSet .nscListBoxSetIn .nscListIcnBset .nscListIcnB2").css("color", "#808080");
                    $("#confirmContents60 .nscListBoxSet .nscListBoxSetIn .nscListIcnBset .nscListIcnB2").css("text-shadow", "block");
                    $("#confirmContents60 .nscListBoxSet .nscListBoxSetIn .nscListIcnBset .nscListIcnB2").css("background", "url(../Styles/Images/SC3080201/nsc60icn2b.png) center 9px no-repeat,-webkit-gradient(linear, left top, left bottom,color-stop(0, #fbfbfb),color-stop(100%, #cacaca))");
                    $("#confirmContents60 .nscListBoxSet.HeightB").css({ height: h }).show(0).animate({ height: 0 }, 300, function () {
                        $("#confirmContents60 .nscListBoxSet.HeightB").hide(0);
                        $("#confirmContents60 .nscListBoxSet.HeightC").css({ height: 0 }).show(0).animate({ height: h }, 300, callback);
                    });
                    this_form.HD_nscListIcnB2.value = "0"
                }

                /* Hot → Successの場合 */
                else if (this_form.HD_nscListIcnB3.value == "1") {
                    $("#confirmContents60 .nscListBoxSet .nscListBoxSetIn .nscListIcnBset .nscListIcnB3").css("text-shadow", "0px -1px 1px #FFF");
                    $("#confirmContents60 .nscListBoxSet .nscListBoxSetIn .nscListIcnBset .nscListIcnB3").css("color", "#808080");
                    $("#confirmContents60 .nscListBoxSet .nscListBoxSetIn .nscListIcnBset .nscListIcnB3").css("text-shadow", "block");
                    $("#confirmContents60 .nscListBoxSet .nscListBoxSetIn .nscListIcnBset .nscListIcnB3").css("background", "url(../Styles/Images/SC3080201/nsc60icn2c.png) center 9px no-repeat,-webkit-gradient(linear, left top, left bottom,color-stop(0, #fbfbfb),color-stop(100%, #cacaca))");
                    $("#confirmContents60 .nscListBoxSet.HeightB").css({ height: h }).show(0).animate({ height: 0 }, 300, function () {
                        $("#confirmContents60 .nscListBoxSet.HeightB").hide(0);
                        $("#confirmContents60 .nscListBoxSet.HeightC").css({ height: 0 }).show(0).animate({ height: h }, 300, callback);
                    });
                    this_form.HD_nscListIcnB3.value = "0"
                }

                /* Give-up → Successの場合 */
                else if (this_form.HD_nscListIcnB5.value == "1") {
                    $("#confirmContents60 .nscListBoxSet .nscListBoxSetIn .nscListIcnBset .nscListIcnB5").css("text-shadow", "0px -1px 1px #FFF");
                    $("#confirmContents60 .nscListBoxSet .nscListBoxSetIn .nscListIcnBset .nscListIcnB5").css("color", "#808080");
                    $("#confirmContents60 .nscListBoxSet .nscListBoxSetIn .nscListIcnBset .nscListIcnB5").css("text-shadow", "block");
                    $("#confirmContents60 .nscListBoxSet .nscListBoxSetIn .nscListIcnBset .nscListIcnB5").css("background", "url(../Styles/Images/SC3080201/nsc60icn2e.png) center 9px no-repeat,-webkit-gradient(linear, left top, left bottom,color-stop(0, #fbfbfb),color-stop(100%, #cacaca))");
                    $("#confirmContents60 .nscListBoxSet.HeightD").css({ height: h }).show(0).animate({ height: 0 }, 300, function () {
                        $("#confirmContents60 .nscListBoxSet.HeightD").hide(0);
                        $("#confirmContents60 .nscListBoxSet.HeightC").css({ height: 0 }).show(0).animate({ height: h }, 300, callback);
                    });
                    this_form.HD_nscListIcnB5.value = "0"
                }
                else {
                    $("#confirmContents60 .nscListBoxSet.HeightC").css({ height: 0 }).show(0).animate({ height: h }, 300, callback);
                }
                this_form.HD_nscListIcnB4.value = "1";
                $("#selectActRlst").val("4")


            }
        }
    );
});

/*    Give-upボタン    */
$(function () {
    var h = $("#confirmContents60 .nscListBoxSet.HeightD").height();
    $("#confirmContents60 .nscListBoxSet .nscListBoxSetIn .nscListIcnBset .nscListIcnB5").click(
        function () {
            if (this_form.HD_nscListIcnB5.value == "0") {
                $(this).css("text-shadow", "0px -1px 1px #000");
                $(this).css("color", "#FFF");
                $(this).css("text-shadow", "none");
                $(this).css("background", "url(../Styles/Images/SC3080201/nsc60icn2eOver.png) center 9px no-repeat,-webkit-gradient(linear, left top, left bottom,color-stop(0, #7b97ed),color-stop(50%, #416ae5),color-stop(51%, #1e4ee1),color-stop(100%, #1e4ee1))");

                /* Walk-in → Give-upの場合 */
                if (this_form.HD_nscListIcnB1.value == "1") {
                    $("#confirmContents60 .nscListBoxSet .nscListBoxSetIn .nscListIcnBset .nscListIcnB1").css("text-shadow", "0px -1px 1px #FFF");
                    $("#confirmContents60 .nscListBoxSet .nscListBoxSetIn .nscListIcnBset .nscListIcnB1").css("color", "#808080");
                    $("#confirmContents60 .nscListBoxSet .nscListBoxSetIn .nscListIcnBset .nscListIcnB1").css("text-shadow", "block");
                    $("#confirmContents60 .nscListBoxSet .nscListBoxSetIn .nscListIcnBset .nscListIcnB1").css("background", "url(../Styles/Images/SC3080201/nsc60icn2a.png) center 9px no-repeat,-webkit-gradient(linear, left top, left bottom,color-stop(0, #fbfbfb),color-stop(100%, #cacaca))");
                    $("#confirmContents60 .nscListBoxSet.HeightB").css({ height: h }).show(0).animate({ height: 0 }, 300, function () {
                        $("#confirmContents60 .nscListBoxSet.HeightB").hide();
                        $("#confirmContents60 .nscListBoxSet.HeightD").css({ height: 0 }).show(0).animate({ height: h }, 300);
                    });
                    this_form.HD_nscListIcnB1.value = "0"
                }

                /* Prospect → Give-upの場合 */
                else if (this_form.HD_nscListIcnB2.value == "1") {
                    $("#confirmContents60 .nscListBoxSet .nscListBoxSetIn .nscListIcnBset .nscListIcnB2").css("text-shadow", "0px -1px 1px #FFF");
                    $("#confirmContents60 .nscListBoxSet .nscListBoxSetIn .nscListIcnBset .nscListIcnB2").css("color", "#808080");
                    $("#confirmContents60 .nscListBoxSet .nscListBoxSetIn .nscListIcnBset .nscListIcnB2").css("text-shadow", "block");
                    $("#confirmContents60 .nscListBoxSet .nscListBoxSetIn .nscListIcnBset .nscListIcnB2").css("background", "url(../Styles/Images/SC3080201/nsc60icn2b.png) center 9px no-repeat,-webkit-gradient(linear, left top, left bottom,color-stop(0, #fbfbfb),color-stop(100%, #cacaca))");
                    $("#confirmContents60 .nscListBoxSet.HeightB").css({ height: h }).show(0).animate({ height: 0 }, 300, function () {
                        $("#confirmContents60 .nscListBoxSet.HeightB").hide();
                        $("#confirmContents60 .nscListBoxSet.HeightD").css({ height: 0 }).show(0).animate({ height: h }, 300);
                    });
                    this_form.HD_nscListIcnB2.value = "0"
                }

                /* Hot → Give-upの場合 */
                else if (this_form.HD_nscListIcnB3.value == "1") {
                    $("#confirmContents60 .nscListBoxSet .nscListBoxSetIn .nscListIcnBset .nscListIcnB3").css("text-shadow", "0px -1px 1px #FFF");
                    $("#confirmContents60 .nscListBoxSet .nscListBoxSetIn .nscListIcnBset .nscListIcnB3").css("color", "#808080");
                    $("#confirmContents60 .nscListBoxSet .nscListBoxSetIn .nscListIcnBset .nscListIcnB3").css("text-shadow", "block");
                    $("#confirmContents60 .nscListBoxSet .nscListBoxSetIn .nscListIcnBset .nscListIcnB3").css("background", "url(../Styles/Images/SC3080201/nsc60icn2c.png) center 9px no-repeat,-webkit-gradient(linear, left top, left bottom,color-stop(0, #fbfbfb),color-stop(100%, #cacaca))");
                    $("#confirmContents60 .nscListBoxSet.HeightB").css({ height: h }).show(0).animate({ height: 0 }, 300, function () {
                        $("#confirmContents60 .nscListBoxSet.HeightB").hide();
                        $("#confirmContents60 .nscListBoxSet.HeightD").css({ height: 0 }).show(0).animate({ height: h }, 300);
                    });
                    this_form.HD_nscListIcnB3.value = "0"
                }

                /* Success → Give-upの場合 */
                else if (this_form.HD_nscListIcnB4.value == "1") {
                    $("#SuccessSelectedCar .scroll-inner").css("-webkit-transition-property","initial")
                    $("#confirmContents60 .nscListBoxSet .nscListBoxSetIn .nscListIcnBset .nscListIcnB4").css("text-shadow", "0px -1px 1px #FFF");
                    $("#confirmContents60 .nscListBoxSet .nscListBoxSetIn .nscListIcnBset .nscListIcnB4").css("color", "#808080");
                    $("#confirmContents60 .nscListBoxSet .nscListBoxSetIn .nscListIcnBset .nscListIcnB4").css("text-shadow", "block");
                    $("#confirmContents60 .nscListBoxSet .nscListBoxSetIn .nscListIcnBset .nscListIcnB4").css("background", "url(../Styles/Images/SC3080201/nsc60icn2d.png) center 9px no-repeat,-webkit-gradient(linear, left top, left bottom,color-stop(0, #fbfbfb),color-stop(100%, #cacaca))");
                    $("#confirmContents60 .nscListBoxSet.HeightC").css({ height: h }).show(0).animate({ height: 0 }, 300, function () {
                        $("#confirmContents60 .nscListBoxSet.HeightC").hide();
                        $("#confirmContents60 .nscListBoxSet.HeightD").css({ height: 0 }).show(0).animate({ height: h }, 300);
                    });
                    this_form.HD_nscListIcnB4.value = "0"
                }
                else {
                    $("#confirmContents60 .nscListBoxSet.HeightD").css({ height: 0 }).show(0).animate({ height: h }, 300);
                }
                this_form.HD_nscListIcnB5.value = "1";
                $("#selectActRlst").val("5")
            }
        }
    );
});

$(function () {
    $(".GiveupCarList").click(function () {
        $(".Giveup").html($(this).attr("title"));
        this_form.selectGiveupCarName.value = $(this).attr("title");
        this_form.selectGiveupCar.value = $(this).attr("value");
        this_form.selectGiveupCarWK.value = $(this).attr("value");
        this_form.selectGiveupMaker.value = this_form.selectGiveupMakerWK.value;
        $(".GiveupCarList").removeClass("Selection");
        $("#GiveupCar" + $(this).attr("value")).addClass("Selection");
        $("#bodyFrame").trigger("click.popover");
    }
    );
});












//次回活動のアラートFrom-To版
$(function () {
    $(".scNscNextActTimeListDiv .AlertSelList").click(function () {
        //全部未選択状態に
        $(".scNscNextActTimeListDiv .AlertSelList").removeClass("Selection");

        //対象を指定
        var targetClass = "#scNscNextActTimeListSelDiv #AlertSelList" + $(this).attr("value")

        //対象を選択状態に
        $(targetClass).addClass("Selection");

        //選択されたアラートNoをHiddenにセット
        $("#selectNextActAlertWK").val($(this).attr("value"));

        //画面上に反映
        //$(".NextActAletName").html($(this).attr("title"));
        $(".NextActAletNamePop").html($(this).attr("title"));

        //ポップアップ閉じる
        //$("#bodyFrame").trigger("click.popover");
    }
    );
});


//次回活動のアラート期限版
$(function () {
    $(".scNscNextActTimeListDiv .AlertNonSelList").click(function () {
        //全部未選択状態に
        $(".scNscNextActTimeListDiv .AlertNonSelList").removeClass("Selection");

        //対象を指定
        var targetClass = "#scNscNextActTimeListNonSelDiv #AlertNonSelList" + $(this).attr("value")

        //対象を選択状態に
        $(targetClass).addClass("Selection");

        //選択されたアラートNoをHiddenにセット
        $("#selectNextActAlertWK").val($(this).attr("value"));

        //画面上に反映
        //$(".NextActAletName").html($(this).attr("title"));
        $(".NextActAletNamePop").html($(this).attr("title"));

        //ポップアップ閉じる
        //$("#bodyFrame").trigger("click.popover");
    }
    );
});


//フォローのアラートFrom-To版
$(function () {
    $(".scNscFollowTimeListDiv .AlertSelList").click(function () {
        //全部未選択状態に
        $(".scNscFollowTimeListDiv .AlertSelList").removeClass("Selection");

        //対象を指定
        var targetClass = "#scNscFollowTimeListSelDiv #AlertSelList" + $(this).attr("value")

        //対象を選択状態に
        $(targetClass).addClass("Selection");

        //選択されたアラートNoをHiddenにセット
        $("#selectFollowAlertWK").val($(this).attr("value"));

        //画面上に反映
        //$(".FollowAletName").html($(this).attr("title"));
        $(".FollowAletNamePop").html($(this).attr("title"));

        //ポップアップ閉じる
        //$("#bodyFrame").trigger("click.popover");
    }
    );
});


//フォローのアラート期限版
$(function () {
    $(".scNscFollowTimeListDiv .AlertNonSelList").click(function () {
        //全部未選択状態に
        $(".scNscFollowTimeListDiv .AlertNonSelList").removeClass("Selection");

        //対象を指定
        var targetClass = "#scNscFollowTimeListNonSelDiv #AlertNonSelList" + $(this).attr("value")

        //対象を選択状態に
        $(targetClass).addClass("Selection");

        //選択されたアラートNoをHiddenにセット
        $("#selectFollowAlertWK").val($(this).attr("value"));

        //画面上に反映
        //$(".FollowAletName").html($(this).attr("title"));
        $(".FollowAletNamePop").html($(this).attr("title"));

        //ポップアップ閉じる
        //$("#bodyFrame").trigger("click.popover");
    }
    );
});




function InputCheck() {
    //活動結果入力チェック
    if (this_form.selectActRlst.value == "") {
        alert(this_form.ErrWord1.value)
        return false;
    }
    //Success時の成約車種チェック
    if (this_form.selectActRlst.value == "4") {
        var flg = "0"
        var SuccessSeriesSet = this_form.selectSelSeries.value;
        var SuccessSeriesSetAry = SuccessSeriesSet.split(";");
        var SuccessSeriesAry;
        for (var i = 0; i < SuccessSeriesSetAry.length - 1; i++) {
            SuccessSeriesAry = SuccessSeriesSetAry[i].split(",");
            if (SuccessSeriesAry[1] == "1") {
                flg = "1";
            }
        }
        if (flg == "0") {
            alert(this_form.ErrWord2.value)
            return false;
        }
    }
    //Give-up時の断念理由チェック
    if (this_form.selectActRlst.value == "5") {
        var strWk = this_form.GiveupReason.value
        strWk = strWk.replace(/^[\s]+/g, "");
        strWk = strWk.replace(/[\s]+$/g, "");
        if (strWk == "") {
            alert(this_form.ErrWord3.value)
            return false;
        }
    }
    //今回活動日をHidden値へセット
    this_form.ActDayFrom.value = $("#ActTimeFromSelector").attr("value");
    this_form.ActDayTo.value = $("#ActTimeToSelector").attr("value");
    
    this_form.ActTimeFromSelectorWK.value = $("#ActTimeFromSelector").attr("value");
    this_form.ActTimeToSelectorWK.value = $("#ActTimeToSelector").attr("value");
    
    //次回活動日関連をHidden値へセット
    if (this_form.NextActDayToFlg.value == "1") {
        this_form.NextActDayFrom.value = $("#NextActTimeFromSelector_WK").attr("value");
        this_form.NextActDayTo.value = $("#NextActTimeToSelector_WK").attr("value");
    }
    else {
        this_form.NextActDayFrom.value = $("#NextActTimeFromSelectorTime_WK").attr("value");
    }

    //次回フォロー日関連をHidden値へセット
    if (this_form.FollowDayToFlg.value == "1") {
        this_form.FollowDayFrom.value = $("#FollowTimeFromSelector_WK").attr("value");
        this_form.FollowDayTo.value = $("#FollowTimeToSelector_WK").attr("value");
    }
    else {
        this_form.FollowDayFrom.value = $("#FollowTimeFromSelectorTime_WK").attr("value");
    }
    this_form.selectGiveupReason.value = this_form.GiveupReason.value

    //WK領域からの戻し
    //カタログ
    this_form.selectActCatalog.value = this_form.selectActCatalogWK.value

    //試乗
    this_form.selectActTestDrive.value = this_form.selectActTestDriveWK.value

    //査定
    this_form.selectActAssesment.value = this_form.selectActAssesmentWK.value

    //見積り
    this_form.selectActValuation.value = this_form.selectActValuationWK.value

    //次回活動アラート
    this_form.selectNextActAlert.value = this_form.selectNextActAlertWK.value

    //フォローアラート
    this_form.selectFollowAlert.value = this_form.selectFollowAlertWK.value

    //他社成約車種
    this_form.selectGiveupMaker.value = this_form.selectGiveupMakerWK.value
    this_form.selectGiveupCar.value = this_form.selectGiveupCarWK.value


}
