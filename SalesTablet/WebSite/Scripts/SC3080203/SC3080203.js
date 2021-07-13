/*
━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
SC3080203.js
─────────────────────────────────────
機能： 顧客詳細(活動登録)
補足： 
作成：  
更新： 2012/03/07 TCS 河原 【SALES_2】
更新： 2012/04/26 TCS 河原 HTMLエンコード対応
更新： 2012/05/17 TCS 安田 クルクル対応
更新： 2012/08/23 TCS 山口 【A STEP2】次世代e-CRB 新車商談機能改善
更新： 2012/11/05 TCS 神本 【A.STEP2】次世代e-CRB  新車タブレット横展開に向けた機能開発
更新： 2013/01/10 TCS 上田 【A.STEP2】次世代e-CRB 新車タブレットSC活動支援機能開発
更新： 2013/12/13 TCS 市川 Aカード情報相互連携開発
更新： 2015/12/15 TCS 鈴木 受注後工程蓋閉め対応
─────────────────────────────────────
*/

/// <reference path="../jquery.js"/>
/// <reference path="../jquery.fingerscroll.js"/>

//登録ボタン押下時のイベントハンドラ登録
$(function () {
    SC3080201.addRegistEventHandlers(InputCheck);
});

// 初期表示設定 //
$(function () {
    var h = $("#confirmContents60 .nscListBoxSet.HeightB").height();
    var listname;
    var listvalue;
    //2013/06/30 TCS 小幡 2013/10対応版　既存流用 START
    //元のFollow-upBoxがHot、Prospect、Walk-in以外の場合Walk-inの活動結果を非表示にする
    //var fllwDvs = $("#FllwDvs").attr("value")
    //if ((fllwDvs == "1" || fllwDvs == "2" || fllwDvs == "6") || fllwDvs == "") {
    //    $(".nscListIcnB1").css("display", "block");
    //} else {
    //    $(".nscListIcnB1").css("display", "none");
    //}
    //2013/06/30 TCS 小幡 2013/10対応版　既存流用 END
    $(".HeightB").css("display", "none");
    $(".HeightC").css("display", "none");
    $(".HeightD").css("display", "none");

    //2012/03/07 TCS 河原 【SALES_2】 START
    listname = "#NextActContactlist"
    listvalue = this_form.selectNextActContact.value
    if ($(listname + listvalue).size() > 0) {
        $(listname + listvalue).addClass("Selection")
    }
    $(".scNscNextActContactName").text(this_form.NextActContactTitle.value);

    listname = "#FollowContactlist"
    listvalue = this_form.selectFollowContact.value
    if ($(listname + listvalue).size() > 0) {
        $(listname + listvalue).addClass("Selection")
    }
    $(".scNscFollowContactName").text(this_form.FollowContactTitle.value)
    //2012/03/07 TCS 河原 【SALES_2】 END

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
        $("#confirmContents60 .nscListBoxSet.HeightB").css({ height: 0 }).show().animate({ height: h }, 0);
        this_form.HD_nscListIcnB3.value = "1";
    }
    if (fllwstatus == "2") {
        $("#confirmContents60 .nscListBoxSet .nscListBoxSetIn .nscListIcnBset .nscListIcnB2").css("text-shadow", "0px -1px 1px #000");
        $("#confirmContents60 .nscListBoxSet .nscListBoxSetIn .nscListIcnBset .nscListIcnB2").css("color", "#FFF");
        $("#confirmContents60 .nscListBoxSet .nscListBoxSetIn .nscListIcnBset .nscListIcnB2").css("text-shadow", "none");
        $("#confirmContents60 .nscListBoxSet .nscListBoxSetIn .nscListIcnBset .nscListIcnB2").css("background", "url(../Styles/Images/SC3080201/nsc60icn2bOver.png) center 9px no-repeat,-webkit-gradient(linear, left top, left bottom,color-stop(0, #7b97ed),color-stop(50%, #416ae5),color-stop(51%, #1e4ee1),color-stop(100%, #1e4ee1))");
        $("#confirmContents60 .nscListBoxSet.HeightB").css({ height: 0 }).show().animate({ height: h }, 0);
        this_form.HD_nscListIcnB2.value = "1";
    }
    if (fllwstatus == "7") {
        $("#confirmContents60 .nscListBoxSet .nscListBoxSetIn .nscListIcnBset .nscListIcnB1").css("text-shadow", "0px -1px 1px #000");
        $("#confirmContents60 .nscListBoxSet .nscListBoxSetIn .nscListIcnBset .nscListIcnB1").css("color", "#FFF");
        $("#confirmContents60 .nscListBoxSet .nscListBoxSetIn .nscListIcnBset .nscListIcnB1").css("text-shadow", "none");
        $("#confirmContents60 .nscListBoxSet .nscListBoxSetIn .nscListIcnBset .nscListIcnB1").css("background", "url(../Styles/Images/SC3080201/nsc60icn2aOver.png) center 9px no-repeat,-webkit-gradient(linear, left top, left bottom,color-stop(0, #7b97ed),color-stop(50%, #416ae5),color-stop(51%, #1e4ee1),color-stop(100%, #1e4ee1))");
        $("#confirmContents60 .nscListBoxSet.HeightB").css({ height: 0 }).show().animate({ height: h }, 0);
        this_form.HD_nscListIcnB1.value = "1";
    }
    if (this_form.selectActRlst.value == "4") {
        $("#confirmContents60 .nscListBoxSet .nscListBoxSetIn .nscListIcnBset .nscListIcnB4").css("text-shadow", "0px -1px 1px #000");
        $("#confirmContents60 .nscListBoxSet .nscListBoxSetIn .nscListIcnBset .nscListIcnB4").css("color", "#FFF");
        $("#confirmContents60 .nscListBoxSet .nscListBoxSetIn .nscListIcnBset .nscListIcnB4").css("text-shadow", "none");
        $("#confirmContents60 .nscListBoxSet .nscListBoxSetIn .nscListIcnBset .nscListIcnB4").css("background", "url(../Styles/Images/SC3080201/nsc60icn2dOver.png) center 9px no-repeat,-webkit-gradient(linear, left top, left bottom,color-stop(0, #7b97ed),color-stop(50%, #416ae5),color-stop(51%, #1e4ee1),color-stop(100%, #1e4ee1))");
        $("#confirmContents60 .nscListBoxSet.HeightC").css({ height: 0 }).show().animate({ height: h }, 0);
        this_form.HD_nscListIcnB4.value = "1";
    }
    if (this_form.selectActRlst.value == "5") {
        $("#confirmContents60 .nscListBoxSet .nscListBoxSetIn .nscListIcnBset .nscListIcnB5").css("text-shadow", "0px -1px 1px #000");
        $("#confirmContents60 .nscListBoxSet .nscListBoxSetIn .nscListIcnBset .nscListIcnB5").css("color", "#FFF");
        $("#confirmContents60 .nscListBoxSet .nscListBoxSetIn .nscListIcnBset .nscListIcnB5").css("text-shadow", "none");
        $("#confirmContents60 .nscListBoxSet .nscListBoxSetIn .nscListIcnBset .nscListIcnB5").css("background", "url(../Styles/Images/SC3080201/nsc60icn2eOver.png) center 9px no-repeat,-webkit-gradient(linear, left top, left bottom,color-stop(0, #7b97ed),color-stop(50%, #416ae5),color-stop(51%, #1e4ee1),color-stop(100%, #1e4ee1))");
        $("#confirmContents60 .nscListBoxSet.HeightD").css({ height: 0 }).show().animate({ height: h }, 0);
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
    //2013/12/13 TCS 市川 Aカード情報相互連携開発 START 
    //this_form.GiveupReason.value = this_form.selectGiveupReason.value
    $("#GiveupReasonDetail").val($("#selectGiveupReason").val());
    //2013/12/13 TCS 市川 Aカード情報相互連携開発 END

    var FollowAct = $(".FollowContactlist").children("span").attr("value");

    if (this_form.FollowFlg.value == "1") {
        $("#confirmContents60 .nscListBoxSet .nscListBoxSetIn .nscListBoxSetRight").css("display", "block")
    }
    else {
        $("#confirmContents60 .nscListBoxSet .nscListBoxSetIn .nscListBoxSetRight").css("display", "none")
    }

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

    //2015/12/15 TCS 河原 受注後工程蓋閉め対応 START
    //成約車種が表示されていない場合(受注時の場合)成約ボタンを初期選択にする
    if ($("#nscListBoxSet_HeightC_Panel").is(':visible') == false) {
        setTimeout(function () {
            $("#confirmContents60 .nscListBoxSet .nscListBoxSetIn .nscListIcnBset .nscListIcnB4").click();
        }, 0);
    }
    //2015/12/15 TCS 河原 受注後工程蓋閉め対応 END

    // 2012/05/17 TCS 安田 クルクル対応 START
    SC3080201.salesAfterFlg = false;
    // 2012/05/17 TCS 安田 クルクル対応 END
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

// 成約車種 //
$(function () {
    $(".SelectedCarlist").live("click", function (e) {
        //2012/11/05 TCS 神本 【A.STEP2】次世代e-CRB  新車タブレット横展開に向けた機能開発 START
        //var listname = "#SelectedCarlist";
        //var listvalue;
        //2012/11/05 TCS 神本 【A.STEP2】次世代e-CRB  新車タブレット横展開に向けた機能開発 END
        var i;
        var j;
        //2012/11/05 TCS 神本 【A.STEP2】次世代e-CRB  新車タブレット横展開に向けた機能開発 START
        //var sel = $(this).attr("id");
        //2013/01/10 TCS 上田 【A.STEP2】次世代e-CRB 新車タブレットSC活動支援機能開発 START
        //var keyvalue = $(this)[0].value;
        var sel = $(this).attr("id");
        var keyvalue = sel.replace("SelectedCarlist", "");
        //2013/01/10 TCS 上田 【A.STEP2】次世代e-CRB 新車タブレットSC活動支援機能開発 END
        //2012/11/05 TCS 神本 【A.STEP2】次世代e-CRB  新車タブレット横展開に向けた機能開発 END
        var seledary;
        var seledarydetail;
        var seledarycreate = "";
        //2012/11/05 TCS 神本 【A.STEP2】次世代e-CRB  新車タブレット横展開に向けた機能開発 START
        //sel = sel.replace("SelectedCarlist", "");
        //2012/11/05 TCS 神本 【A.STEP2】次世代e-CRB  新車タブレット横展開に向けた機能開発 END
        var seled = this_form.selectSelSeries.value;
        seledary = seled.split(";");
        for (i = 0; i < seledary.length - 1; i++) {
            seledarydetail = seledary[i].split(",");
            //2012/11/05 TCS 神本 【A.STEP2】次世代e-CRB  新車タブレット横展開に向けた機能開発 START
            //if (seledarydetail[0] == sel) {
            if (seledarydetail[0] == keyvalue) {
                //2012/11/05 TCS 神本 【A.STEP2】次世代e-CRB  新車タブレット横展開に向けた機能開発 END
                if (seledarydetail[1] == "0") {
                    seledarydetail[1] = "1";
                    //2012/11/05 TCS 神本 【A.STEP2】次世代e-CRB  新車タブレット横展開に向けた機能開発 START
                    //listvalue = seledarydetail[0]
                    //$(listname + listvalue).addClass("Selection")
                    $(this).addClass("Selection");
                    //2012/11/05 TCS 神本 【A.STEP2】次世代e-CRB  新車タブレット横展開に向けた機能開発 END
                }
                else if (seledarydetail[1] == "1") {
                    seledarydetail[1] = "0";
                    //2012/11/05 TCS 神本 【A.STEP2】次世代e-CRB  新車タブレット横展開に向けた機能開発 START
                    //listvalue = seledarydetail[0]
                    //$(listname + listvalue).removeClass("Selection")
                    $(this).removeClass("Selection");
                    //2012/11/05 TCS 神本 【A.STEP2】次世代e-CRB  新車タブレット横展開に向けた機能開発 END
                }
            }
            //2012/11/05 TCS 神本 【A.STEP2】次世代e-CRB  新車タブレット横展開に向けた機能開発 START
            //seledary[i] = seledarydetail[0] + "," + seledarydetail[1];
            //2013/01/10 TCS 上田 【A.STEP2】次世代e-CRB 新車タブレットSC活動支援機能開発 START
            //seledary[i] = seledarydetail[0] + "," + seledarydetail[1] + "," + seledarydetail[2] + "," + seledarydetail[3];
            seledary[i] = seledarydetail[0] + "," + seledarydetail[1] + "," + seledarydetail[2] + "," + seledarydetail[3] + "," + seledarydetail[4] + "," + seledarydetail[5];
            //2013/01/10 TCS 上田 【A.STEP2】次世代e-CRB 新車タブレットSC活動支援機能開発 END
            //2012/11/05 TCS 神本 【A.STEP2】次世代e-CRB  新車タブレット横展開に向けた機能開発 END
            seledarycreate = seledarycreate + seledary[i] + ";";
        }
        this_form.selectSelSeries.value = seledarycreate;
    });
});

//スクロール化
$(function () {
    $("#SuccessSelectedCar").fingerScroll();
    $(".scNscNextActContactListBox").fingerScroll();
    $(".scNscFollowContactListBox").fingerScroll();
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
                    $("#confirmContents60 .nscListBoxSet .nscListBoxSetIn .nscListIcnBset .nscListIcnB2").css("text-shadow", "0px 1px 1px #FFF");
                    $("#confirmContents60 .nscListBoxSet .nscListBoxSetIn .nscListIcnBset .nscListIcnB2").css("color", "#808080");
                    $("#confirmContents60 .nscListBoxSet .nscListBoxSetIn .nscListIcnBset .nscListIcnB2").css("text-shadow", "block");
                    $("#confirmContents60 .nscListBoxSet .nscListBoxSetIn .nscListIcnBset .nscListIcnB2").css("background", "url(../Styles/Images/SC3080201/nsc60icn2b.png) center 9px no-repeat,-webkit-gradient(linear, left top, left bottom,color-stop(0, #fbfbfb),color-stop(100%, #cacaca))");
                    this_form.HD_nscListIcnB2.value = "0"
                }

                /* Hot → Walk-inの場合 */
                else if (this_form.HD_nscListIcnB3.value == "1") {
                    $("#confirmContents60 .nscListBoxSet .nscListBoxSetIn .nscListIcnBset .nscListIcnB3").css("text-shadow", "0px 1px 1px #FFF");
                    $("#confirmContents60 .nscListBoxSet .nscListBoxSetIn .nscListIcnBset .nscListIcnB3").css("color", "#808080");
                    $("#confirmContents60 .nscListBoxSet .nscListBoxSetIn .nscListIcnBset .nscListIcnB3").css("text-shadow", "block");
                    $("#confirmContents60 .nscListBoxSet .nscListBoxSetIn .nscListIcnBset .nscListIcnB3").css("background", "url(../Styles/Images/SC3080201/nsc60icn2c.png) center 9px no-repeat,-webkit-gradient(linear, left top, left bottom,color-stop(0, #fbfbfb),color-stop(100%, #cacaca))");
                    this_form.HD_nscListIcnB3.value = "0"
                }

                /* Success → Walk-inの場合 */
                else if (this_form.HD_nscListIcnB4.value == "1") {
                    //2012/03/27 TCS 平野 【SALES_2】(Sales1A ユーザテスト No.226) START
                    $("#SuccessSeriesUpdatePanel").hide();
                    //2012/03/27 TCS 平野 【SALES_2】(Sales1A ユーザテスト No.226) END

                    $("#SuccessSelectedCar .scroll-inner").css("-webkit-transition-property", "initial")
                    $("#confirmContents60 .nscListBoxSet .nscListBoxSetIn .nscListIcnBset .nscListIcnB4").css("text-shadow", "0px 1px 1px #FFF");
                    $("#confirmContents60 .nscListBoxSet .nscListBoxSetIn .nscListIcnBset .nscListIcnB4").css("color", "#808080");
                    $("#confirmContents60 .nscListBoxSet .nscListBoxSetIn .nscListIcnBset .nscListIcnB4").css("text-shadow", "block");
                    $("#confirmContents60 .nscListBoxSet .nscListBoxSetIn .nscListIcnBset .nscListIcnB4").css("background", "url(../Styles/Images/SC3080201/nsc60icn2d.png) center 9px no-repeat,-webkit-gradient(linear, left top, left bottom,color-stop(0, #fbfbfb),color-stop(100%, #cacaca))");
                    $("#confirmContents60 .nscListBoxSet.HeightC").css({ height: h }).show().animate({ height: 0 }, 300, function () {
                        $("#confirmContents60 .nscListBoxSet.HeightC").hide();
                        $("#confirmContents60 .nscListBoxSet.HeightB").css({ height: 0 }).show().animate({ height: h }, 300);
                    });
                    this_form.HD_nscListIcnB4.value = "0"
                }

                /* Give-up → Walk-inの場合 */
                else if (this_form.HD_nscListIcnB5.value == "1") {
                    $("#GiveupReason").hide();
                    $("#confirmContents60 .nscListBoxSet .nscListBoxSetIn .nscListIcnBset .nscListIcnB5").css("text-shadow", "0px 1px 1px #FFF");
                    $("#confirmContents60 .nscListBoxSet .nscListBoxSetIn .nscListIcnBset .nscListIcnB5").css("color", "#808080");
                    $("#confirmContents60 .nscListBoxSet .nscListBoxSetIn .nscListIcnBset .nscListIcnB5").css("text-shadow", "block");
                    $("#confirmContents60 .nscListBoxSet .nscListBoxSetIn .nscListIcnBset .nscListIcnB5").css("background", "url(../Styles/Images/SC3080201/nsc60icn2e.png) center 9px no-repeat,-webkit-gradient(linear, left top, left bottom,color-stop(0, #fbfbfb),color-stop(100%, #cacaca))");
                    $("#confirmContents60 .nscListBoxSet.HeightD").css({ height: h }).show().animate({ height: 0 }, 300, function () {
                        $("#confirmContents60 .nscListBoxSet.HeightD").hide();
                        $("#confirmContents60 .nscListBoxSet.HeightB").css({ height: 0 }).show().animate({ height: h }, 300);
                    });
                    this_form.HD_nscListIcnB5.value = "0"
                }
                else {
                    $("#confirmContents60 .nscListBoxSet.HeightB").css({ height: 0 }).show().animate({ height: h }, 300);
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
                    $("#confirmContents60 .nscListBoxSet .nscListBoxSetIn .nscListIcnBset .nscListIcnB1").css("text-shadow", "0px 1px 1px #FFF");
                    $("#confirmContents60 .nscListBoxSet .nscListBoxSetIn .nscListIcnBset .nscListIcnB1").css("color", "#808080");
                    $("#confirmContents60 .nscListBoxSet .nscListBoxSetIn .nscListIcnBset .nscListIcnB1").css("text-shadow", "block");
                    $("#confirmContents60 .nscListBoxSet .nscListBoxSetIn .nscListIcnBset .nscListIcnB1").css("background", "url(../Styles/Images/SC3080201/nsc60icn2a.png) center 9px no-repeat,-webkit-gradient(linear, left top, left bottom,color-stop(0, #fbfbfb),color-stop(100%, #cacaca))");
                    this_form.HD_nscListIcnB1.value = "0"
                }

                /* Hot → Prospectの場合 */
                else if (this_form.HD_nscListIcnB3.value == "1") {
                    $("#confirmContents60 .nscListBoxSet .nscListBoxSetIn .nscListIcnBset .nscListIcnB3").css("text-shadow", "0px 1px 1px #FFF");
                    $("#confirmContents60 .nscListBoxSet .nscListBoxSetIn .nscListIcnBset .nscListIcnB3").css("color", "#808080");
                    $("#confirmContents60 .nscListBoxSet .nscListBoxSetIn .nscListIcnBset .nscListIcnB3").css("text-shadow", "block");
                    $("#confirmContents60 .nscListBoxSet .nscListBoxSetIn .nscListIcnBset .nscListIcnB3").css("background", "url(../Styles/Images/SC3080201/nsc60icn2c.png) center 9px no-repeat,-webkit-gradient(linear, left top, left bottom,color-stop(0, #fbfbfb),color-stop(100%, #cacaca))");
                    this_form.HD_nscListIcnB3.value = "0"
                }

                /* Success → Prospectの場合 */
                else if (this_form.HD_nscListIcnB4.value == "1") {
                    //2012/03/27 TCS 平野 【SALES_2】(Sales1A ユーザテスト No.226) START
                    $("#SuccessSeriesUpdatePanel").hide();
                    //2012/03/27 TCS 平野 【SALES_2】(Sales1A ユーザテスト No.226) END

                    $("#SuccessSelectedCar .scroll-inner").css("-webkit-transition-property", "initial")
                    $("#confirmContents60 .nscListBoxSet .nscListBoxSetIn .nscListIcnBset .nscListIcnB4").css("text-shadow", "0px 1px 1px #FFF");
                    $("#confirmContents60 .nscListBoxSet .nscListBoxSetIn .nscListIcnBset .nscListIcnB4").css("color", "#808080");
                    $("#confirmContents60 .nscListBoxSet .nscListBoxSetIn .nscListIcnBset .nscListIcnB4").css("text-shadow", "block");
                    $("#confirmContents60 .nscListBoxSet .nscListBoxSetIn .nscListIcnBset .nscListIcnB4").css("background", "url(../Styles/Images/SC3080201/nsc60icn2d.png) center 9px no-repeat,-webkit-gradient(linear, left top, left bottom,color-stop(0, #fbfbfb),color-stop(100%, #cacaca))");
                    $("#confirmContents60 .nscListBoxSet.HeightC").css({ height: h }).show().animate({ height: 0 }, 300, function () {
                        $("#confirmContents60 .nscListBoxSet.HeightC").hide();
                        $("#confirmContents60 .nscListBoxSet.HeightB").css({ height: 0 }).show().animate({ height: h }, 300);
                    });
                    this_form.HD_nscListIcnB4.value = "0"
                }

                /* Give-up → Prospectの場合 */
                else if (this_form.HD_nscListIcnB5.value == "1") {
                    //2012/03/27 TCS 平野 【SALES_2】(Sales1A ユーザテスト No.226) START
                    $("#GiveupReason").hide();
                    //2012/03/27 TCS 平野 【SALES_2】(Sales1A ユーザテスト No.226) END

                    $("#confirmContents60 .nscListBoxSet .nscListBoxSetIn .nscListIcnBset .nscListIcnB5").css("text-shadow", "0px 1px 1px #FFF");
                    $("#confirmContents60 .nscListBoxSet .nscListBoxSetIn .nscListIcnBset .nscListIcnB5").css("color", "#808080");
                    $("#confirmContents60 .nscListBoxSet .nscListBoxSetIn .nscListIcnBset .nscListIcnB5").css("text-shadow", "block");
                    $("#confirmContents60 .nscListBoxSet .nscListBoxSetIn .nscListIcnBset .nscListIcnB5").css("background", "url(../Styles/Images/SC3080201/nsc60icn2e.png) center 9px no-repeat,-webkit-gradient(linear, left top, left bottom,color-stop(0, #fbfbfb),color-stop(100%, #cacaca))");
                    $("#confirmContents60 .nscListBoxSet.HeightD").css({ height: h }).show().animate({ height: 0 }, 300, function () {
                        $("#confirmContents60 .nscListBoxSet.HeightD").hide();
                        $("#confirmContents60 .nscListBoxSet.HeightB").css({ height: 0 }).show().animate({ height: h }, 300);
                    });
                    this_form.HD_nscListIcnB5.value = "0"
                }
                else {
                    $("#confirmContents60 .nscListBoxSet.HeightB").css({ height: 0 }).show().animate({ height: h }, 300);
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
                    $("#confirmContents60 .nscListBoxSet .nscListBoxSetIn .nscListIcnBset .nscListIcnB1").css("text-shadow", "0px 1px 1px #FFF");
                    $("#confirmContents60 .nscListBoxSet .nscListBoxSetIn .nscListIcnBset .nscListIcnB1").css("color", "#808080");
                    $("#confirmContents60 .nscListBoxSet .nscListBoxSetIn .nscListIcnBset .nscListIcnB1").css("text-shadow", "block");
                    $("#confirmContents60 .nscListBoxSet .nscListBoxSetIn .nscListIcnBset .nscListIcnB1").css("background", "url(../Styles/Images/SC3080201/nsc60icn2a.png) center 9px no-repeat,-webkit-gradient(linear, left top, left bottom,color-stop(0, #fbfbfb),color-stop(100%, #cacaca))");
                    this_form.HD_nscListIcnB1.value = "0"
                }

                /* Prospect → Hotの場合 */
                else if (this_form.HD_nscListIcnB2.value == "1") {
                    $("#confirmContents60 .nscListBoxSet .nscListBoxSetIn .nscListIcnBset .nscListIcnB2").css("text-shadow", "0px 1px 1px #FFF");
                    $("#confirmContents60 .nscListBoxSet .nscListBoxSetIn .nscListIcnBset .nscListIcnB2").css("color", "#808080");
                    $("#confirmContents60 .nscListBoxSet .nscListBoxSetIn .nscListIcnBset .nscListIcnB2").css("text-shadow", "block");
                    $("#confirmContents60 .nscListBoxSet .nscListBoxSetIn .nscListIcnBset .nscListIcnB2").css("background", "url(../Styles/Images/SC3080201/nsc60icn2b.png) center 9px no-repeat,-webkit-gradient(linear, left top, left bottom,color-stop(0, #fbfbfb),color-stop(100%, #cacaca))");
                    this_form.HD_nscListIcnB2.value = "0"
                }

                /* Success → Hotの場合 */
                else if (this_form.HD_nscListIcnB4.value == "1") {
                    //2012/03/27 TCS 平野 【SALES_2】(Sales1A ユーザテスト No.226) START
                    $("#SuccessSeriesUpdatePanel").hide();
                    //2012/03/27 TCS 平野 【SALES_2】(Sales1A ユーザテスト No.226) END

                    $("#SuccessSelectedCar .scroll-inner").css("-webkit-transition-property", "initial")
                    $("#confirmContents60 .nscListBoxSet .nscListBoxSetIn .nscListIcnBset .nscListIcnB4").css("text-shadow", "0px 1px 1px #FFF");
                    $("#confirmContents60 .nscListBoxSet .nscListBoxSetIn .nscListIcnBset .nscListIcnB4").css("color", "#808080");
                    $("#confirmContents60 .nscListBoxSet .nscListBoxSetIn .nscListIcnBset .nscListIcnB4").css("text-shadow", "block");
                    $("#confirmContents60 .nscListBoxSet .nscListBoxSetIn .nscListIcnBset .nscListIcnB4").css("background", "url(../Styles/Images/SC3080201/nsc60icn2d.png) center 9px no-repeat,-webkit-gradient(linear, left top, left bottom,color-stop(0, #fbfbfb),color-stop(100%, #cacaca))");
                    $("#confirmContents60 .nscListBoxSet.HeightC").css({ height: h }).show().animate({ height: 0 }, 300, function () {
                        $("#confirmContents60 .nscListBoxSet.HeightC").hide();
                        $("#confirmContents60 .nscListBoxSet.HeightB").css({ height: 0 }).show().animate({ height: h }, 300);
                    });
                    this_form.HD_nscListIcnB4.value = "0"
                }

                /* Give-up → Hotの場合 */
                else if (this_form.HD_nscListIcnB5.value == "1") {
                    //2012/03/27 TCS 平野 【SALES_2】(Sales1A ユーザテスト No.226) START
                    $("#GiveupReason").hide();
                    //2012/03/27 TCS 平野 【SALES_2】(Sales1A ユーザテスト No.226) END

                    $("#confirmContents60 .nscListBoxSet .nscListBoxSetIn .nscListIcnBset .nscListIcnB5").css("text-shadow", "0px 1px 1px #FFF");
                    $("#confirmContents60 .nscListBoxSet .nscListBoxSetIn .nscListIcnBset .nscListIcnB5").css("color", "#808080");
                    $("#confirmContents60 .nscListBoxSet .nscListBoxSetIn .nscListIcnBset .nscListIcnB5").css("text-shadow", "block");
                    $("#confirmContents60 .nscListBoxSet .nscListBoxSetIn .nscListIcnBset .nscListIcnB5").css("background", "url(../Styles/Images/SC3080201/nsc60icn2e.png) center 9px no-repeat,-webkit-gradient(linear, left top, left bottom,color-stop(0, #fbfbfb),color-stop(100%, #cacaca))");
                    $("#confirmContents60 .nscListBoxSet.HeightD").css({ height: h }).show().animate({ height: 0 }, 300, function () {
                        $("#confirmContents60 .nscListBoxSet.HeightD").hide();
                        $("#confirmContents60 .nscListBoxSet.HeightB").css({ height: 0 }).show().animate({ height: h }, 300);
                    });
                    this_form.HD_nscListIcnB5.value = "0"
                }
                else {
                    $("#confirmContents60 .nscListBoxSet.HeightB").css({ height: 0 }).show().animate({ height: h }, 300);
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
                //2012/03/27 TCS 平野 【SALES_2】(Sales1A ユーザテスト No.226) START
                $("#SuccessSeriesUpdatePanel").show();
                //2012/03/27 TCS 平野 【SALES_2】(Sales1A ユーザテスト No.226) END

                $(this).css("text-shadow", "0px -1px 1px #000");
                $(this).css("color", "#FFF");
                $(this).css("text-shadow", "none");
                $(this).css("background", "url(../Styles/Images/SC3080201/nsc60icn2dOver.png) center 9px no-repeat,-webkit-gradient(linear, left top, left bottom,color-stop(0, #7b97ed),color-stop(50%, #416ae5),color-stop(51%, #1e4ee1),color-stop(100%, #1e4ee1))");

                var callback = function () {
                    setTimeout(function () {

                        $("#SuccessSelectedCar .scroll-inner").css("-webkit-transition-property", "none")

                    }, 0);
                };

                /* Walk-in → Successの場合 */
                if (this_form.HD_nscListIcnB1.value == "1") {
                    $("#confirmContents60 .nscListBoxSet .nscListBoxSetIn .nscListIcnBset .nscListIcnB1").css("text-shadow", "0px 1px 1px #FFF");
                    $("#confirmContents60 .nscListBoxSet .nscListBoxSetIn .nscListIcnBset .nscListIcnB1").css("color", "#808080");
                    $("#confirmContents60 .nscListBoxSet .nscListBoxSetIn .nscListIcnBset .nscListIcnB1").css("text-shadow", "block");
                    $("#confirmContents60 .nscListBoxSet .nscListBoxSetIn .nscListIcnBset .nscListIcnB1").css("background", "url(../Styles/Images/SC3080201/nsc60icn2a.png) center 9px no-repeat,-webkit-gradient(linear, left top, left bottom,color-stop(0, #fbfbfb),color-stop(100%, #cacaca))");
                    $("#confirmContents60 .nscListBoxSet.HeightB").css({ height: h }).show().animate({ height: 0 }, 300, function () {
                        $("#confirmContents60 .nscListBoxSet.HeightB").hide();
                        $("#confirmContents60 .nscListBoxSet.HeightC").css({ height: 0 }).show().animate({ height: h }, 300, callback);
                    });
                    this_form.HD_nscListIcnB1.value = "0"
                }

                /* Prospect → Successの場合  */
                else if (this_form.HD_nscListIcnB2.value == "1") {
                    $("#confirmContents60 .nscListBoxSet .nscListBoxSetIn .nscListIcnBset .nscListIcnB2").css("text-shadow", "0px 1px 1px #FFF");
                    $("#confirmContents60 .nscListBoxSet .nscListBoxSetIn .nscListIcnBset .nscListIcnB2").css("color", "#808080");
                    $("#confirmContents60 .nscListBoxSet .nscListBoxSetIn .nscListIcnBset .nscListIcnB2").css("text-shadow", "block");
                    $("#confirmContents60 .nscListBoxSet .nscListBoxSetIn .nscListIcnBset .nscListIcnB2").css("background", "url(../Styles/Images/SC3080201/nsc60icn2b.png) center 9px no-repeat,-webkit-gradient(linear, left top, left bottom,color-stop(0, #fbfbfb),color-stop(100%, #cacaca))");
                    $("#confirmContents60 .nscListBoxSet.HeightB").css({ height: h }).show().animate({ height: 0 }, 300, function () {
                        $("#confirmContents60 .nscListBoxSet.HeightB").hide();
                        $("#confirmContents60 .nscListBoxSet.HeightC").css({ height: 0 }).show().animate({ height: h }, 300, callback);
                    });
                    this_form.HD_nscListIcnB2.value = "0"
                }

                /* Hot → Successの場合 */
                else if (this_form.HD_nscListIcnB3.value == "1") {
                    $("#confirmContents60 .nscListBoxSet .nscListBoxSetIn .nscListIcnBset .nscListIcnB3").css("text-shadow", "0px 1px 1px #FFF");
                    $("#confirmContents60 .nscListBoxSet .nscListBoxSetIn .nscListIcnBset .nscListIcnB3").css("color", "#808080");
                    $("#confirmContents60 .nscListBoxSet .nscListBoxSetIn .nscListIcnBset .nscListIcnB3").css("text-shadow", "block");
                    $("#confirmContents60 .nscListBoxSet .nscListBoxSetIn .nscListIcnBset .nscListIcnB3").css("background", "url(../Styles/Images/SC3080201/nsc60icn2c.png) center 9px no-repeat,-webkit-gradient(linear, left top, left bottom,color-stop(0, #fbfbfb),color-stop(100%, #cacaca))");
                    $("#confirmContents60 .nscListBoxSet.HeightB").css({ height: h }).show().animate({ height: 0 }, 300, function () {
                        $("#confirmContents60 .nscListBoxSet.HeightB").hide();
                        $("#confirmContents60 .nscListBoxSet.HeightC").css({ height: 0 }).show().animate({ height: h }, 300, callback);
                    });
                    this_form.HD_nscListIcnB3.value = "0"
                }

                /* Give-up → Successの場合 */
                else if (this_form.HD_nscListIcnB5.value == "1") {
                    //2012/03/27 TCS 平野 【SALES_2】(Sales1A ユーザテスト No.226) START
                    $("#GiveupReason").hide();
                    //2012/03/27 TCS 平野 【SALES_2】(Sales1A ユーザテスト No.226) END

                    $("#confirmContents60 .nscListBoxSet .nscListBoxSetIn .nscListIcnBset .nscListIcnB5").css("text-shadow", "0px 1px 1px #FFF");
                    $("#confirmContents60 .nscListBoxSet .nscListBoxSetIn .nscListIcnBset .nscListIcnB5").css("color", "#808080");
                    $("#confirmContents60 .nscListBoxSet .nscListBoxSetIn .nscListIcnBset .nscListIcnB5").css("text-shadow", "block");
                    $("#confirmContents60 .nscListBoxSet .nscListBoxSetIn .nscListIcnBset .nscListIcnB5").css("background", "url(../Styles/Images/SC3080201/nsc60icn2e.png) center 9px no-repeat,-webkit-gradient(linear, left top, left bottom,color-stop(0, #fbfbfb),color-stop(100%, #cacaca))");
                    $("#confirmContents60 .nscListBoxSet.HeightD").css({ height: h }).show().animate({ height: 0 }, 300, function () {
                        $("#confirmContents60 .nscListBoxSet.HeightD").hide();
                        $("#confirmContents60 .nscListBoxSet.HeightC").css({ height: 0 }).show().animate({ height: h }, 300, callback);
                    });
                    this_form.HD_nscListIcnB5.value = "0"
                }
                else {
                    $("#confirmContents60 .nscListBoxSet.HeightC").css({ height: 0 }).show().animate({ height: h }, 300, callback);
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
                //2012/03/27 TCS 平野 【SALES_2】(Sales1A ユーザテスト No.226) START
                $("#GiveupReason").show();
                //2012/03/27 TCS 平野 【SALES_2】(Sales1A ユーザテスト No.226) END

                $(this).css("text-shadow", "0px -1px 1px #000");
                $(this).css("color", "#FFF");
                $(this).css("text-shadow", "none");
                $(this).css("background", "url(../Styles/Images/SC3080201/nsc60icn2eOver.png) center 9px no-repeat,-webkit-gradient(linear, left top, left bottom,color-stop(0, #7b97ed),color-stop(50%, #416ae5),color-stop(51%, #1e4ee1),color-stop(100%, #1e4ee1))");

                var callback = function () {
                    //Give-upに変更時に断念理由が表示されない不具合の対応のため、一瞬だけ別の色に変更して戻す
                    $("#GiveupReason").css("color", "#000001");
                    setTimeout(function () {
                        $("#GiveupReason").css("color", "#000000");
                    }, 0);
                };

                /* Walk-in → Give-upの場合 */
                if (this_form.HD_nscListIcnB1.value == "1") {
                    $("#confirmContents60 .nscListBoxSet .nscListBoxSetIn .nscListIcnBset .nscListIcnB1").css("text-shadow", "0px 1px 1px #FFF");
                    $("#confirmContents60 .nscListBoxSet .nscListBoxSetIn .nscListIcnBset .nscListIcnB1").css("color", "#808080");
                    $("#confirmContents60 .nscListBoxSet .nscListBoxSetIn .nscListIcnBset .nscListIcnB1").css("text-shadow", "block");
                    $("#confirmContents60 .nscListBoxSet .nscListBoxSetIn .nscListIcnBset .nscListIcnB1").css("background", "url(../Styles/Images/SC3080201/nsc60icn2a.png) center 9px no-repeat,-webkit-gradient(linear, left top, left bottom,color-stop(0, #fbfbfb),color-stop(100%, #cacaca))");
                    $("#confirmContents60 .nscListBoxSet.HeightB").css({ height: h }).show().animate({ height: 0 }, 300, function () {
                        $("#confirmContents60 .nscListBoxSet.HeightB").hide();
                        $("#confirmContents60 .nscListBoxSet.HeightD").css({ height: 0 }).show().animate({ height: h }, 300,callback);
                    });
                    this_form.HD_nscListIcnB1.value = "0"
                }

                /* Prospect → Give-upの場合 */
                else if (this_form.HD_nscListIcnB2.value == "1") {
                    $("#confirmContents60 .nscListBoxSet .nscListBoxSetIn .nscListIcnBset .nscListIcnB2").css("text-shadow", "0px 1px 1px #FFF");
                    $("#confirmContents60 .nscListBoxSet .nscListBoxSetIn .nscListIcnBset .nscListIcnB2").css("color", "#808080");
                    $("#confirmContents60 .nscListBoxSet .nscListBoxSetIn .nscListIcnBset .nscListIcnB2").css("text-shadow", "block");
                    $("#confirmContents60 .nscListBoxSet .nscListBoxSetIn .nscListIcnBset .nscListIcnB2").css("background", "url(../Styles/Images/SC3080201/nsc60icn2b.png) center 9px no-repeat,-webkit-gradient(linear, left top, left bottom,color-stop(0, #fbfbfb),color-stop(100%, #cacaca))");
                    $("#confirmContents60 .nscListBoxSet.HeightB").css({ height: h }).show().animate({ height: 0 }, 300, function () {
                        $("#confirmContents60 .nscListBoxSet.HeightB").hide();
                        $("#confirmContents60 .nscListBoxSet.HeightD").css({ height: 0 }).show().animate({ height: h }, 300,callback);
                    });
                    this_form.HD_nscListIcnB2.value = "0"
                }

                /* Hot → Give-upの場合 */
                else if (this_form.HD_nscListIcnB3.value == "1") {
                    $("#confirmContents60 .nscListBoxSet .nscListBoxSetIn .nscListIcnBset .nscListIcnB3").css("text-shadow", "0px 1px 1px #FFF");
                    $("#confirmContents60 .nscListBoxSet .nscListBoxSetIn .nscListIcnBset .nscListIcnB3").css("color", "#808080");
                    $("#confirmContents60 .nscListBoxSet .nscListBoxSetIn .nscListIcnBset .nscListIcnB3").css("text-shadow", "block");
                    $("#confirmContents60 .nscListBoxSet .nscListBoxSetIn .nscListIcnBset .nscListIcnB3").css("background", "url(../Styles/Images/SC3080201/nsc60icn2c.png) center 9px no-repeat,-webkit-gradient(linear, left top, left bottom,color-stop(0, #fbfbfb),color-stop(100%, #cacaca))");
                    $("#confirmContents60 .nscListBoxSet.HeightB").css({ height: h }).show().animate({ height: 0 }, 300, function () {
                        $("#confirmContents60 .nscListBoxSet.HeightB").hide();
                        $("#confirmContents60 .nscListBoxSet.HeightD").css({ height: 0 }).show().animate({ height: h }, 300,callback);
                    });
                    this_form.HD_nscListIcnB3.value = "0"
                }

                /* Success → Give-upの場合 */
                else if (this_form.HD_nscListIcnB4.value == "1") {
                    //2012/03/27 TCS 平野 【SALES_2】(Sales1A ユーザテスト No.226) START
                    $("#SuccessSeriesUpdatePanel").hide();
                    //2012/03/27 TCS 平野 【SALES_2】(Sales1A ユーザテスト No.226) END

                    $("#SuccessSelectedCar .scroll-inner").css("-webkit-transition-property", "initial")
                    $("#confirmContents60 .nscListBoxSet .nscListBoxSetIn .nscListIcnBset .nscListIcnB4").css("text-shadow", "0px 1px 1px #FFF");
                    $("#confirmContents60 .nscListBoxSet .nscListBoxSetIn .nscListIcnBset .nscListIcnB4").css("color", "#808080");
                    $("#confirmContents60 .nscListBoxSet .nscListBoxSetIn .nscListIcnBset .nscListIcnB4").css("text-shadow", "block");
                    $("#confirmContents60 .nscListBoxSet .nscListBoxSetIn .nscListIcnBset .nscListIcnB4").css("background", "url(../Styles/Images/SC3080201/nsc60icn2d.png) center 9px no-repeat,-webkit-gradient(linear, left top, left bottom,color-stop(0, #fbfbfb),color-stop(100%, #cacaca))");
                    $("#confirmContents60 .nscListBoxSet.HeightC").css({ height: h }).show().animate({ height: 0 }, 300, function () {
                        $("#confirmContents60 .nscListBoxSet.HeightC").hide();
                        $("#confirmContents60 .nscListBoxSet.HeightD").css({ height: 0 }).show().animate({ height: h }, 300,callback);
                    });
                    this_form.HD_nscListIcnB4.value = "0"
                }
                else {
                    $("#confirmContents60 .nscListBoxSet.HeightD").css({ height: 0 }).show().animate({ height: h }, 300);
                }
                this_form.HD_nscListIcnB5.value = "1";
                $("#selectActRlst").val("5")
            }
        }
    );
});


$(function () {
    $(".GiveupCarList").click(function () {
        //2012/04/26 TCS 河原 HTMLエンコード対応 START
        $(".Giveup").html(HtmlEncode($(this).attr("title")));
        this_form.selectGiveupCarName.value = HtmlEncode($(this).attr("title"));
        //2012/04/26 TCS 河原 HTMLエンコード対応 END
        //2013/07/12 TCS 小幡 2013/10対応版　既存流用 START
        this_form.selectGiveupCar.value = $(this).attr("id");
        this_form.selectGiveupCarWK.value = $(this).attr("id");
        // 2013/07/12 TCS 小幡 2013/10対応版　既存流用 END
        this_form.selectGiveupMaker.value = this_form.selectGiveupMakerWK.value;
        $(".GiveupCarList").removeClass("Selection");
        $("#GiveupCar" + $(this).attr("value")).addClass("Selection");
        $("#bodyFrame").trigger("click.popover");
    });
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
        $(".NextActAletNamePop").html($(this).attr("title"));
    });
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
        $(".NextActAletNamePop").html($(this).attr("title"));
    });
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
        $(".FollowAletNamePop").html($(this).attr("title"));
    });
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
        $(".FollowAletNamePop").html($(this).attr("title"));
    });
});

function InputCheck() {
    //活動結果入力チェック
    if (this_form.selectActRlst.value == "") {
        alert(this_form.ErrWord1.value)
        return false;
    }
    //2015/12/11 TCS 鈴木 受注後工程蓋閉め対応 START
    //成約車種が表示されている場合
    if ($("#nscListBoxSet_HeightC_Panel").is(':visible')) {
        //2015/12/11 TCS 鈴木 受注後工程蓋閉め対応 END
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
    }

    //Give-up時の断念理由チェック
    if (this_form.selectActRlst.value == "5") {

        //2013/12/13 TCS 市川 Aカード情報相互連携開発 START
        var strWk = $("#dispGiveupReason").text();

        //その他選択の時のみ、手入力欄をチェックする。
        if ($("#selectGiveupReasonOtherFlg").val() == 'true') {
            strWk = $("#selectGiveupReason").val();
        }
        //2013/12/13 TCS 市川 Aカード情報相互連携開発 END
        
        strWk = strWk.replace(/^[\s]+/g, "");
        strWk = strWk.replace(/[\s]+$/g, "");
        
        if (strWk = null || strWk == "") {
            alert(this_form.ErrWord3.value);
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
    //2013/12/12 this_form.selectGiveupReason.value = this_form.GiveupReason.value
    
    //次回活動アラート
    this_form.selectNextActAlert.value = this_form.selectNextActAlertWK.value
    
    //フォローアラート
    this_form.selectFollowAlert.value = this_form.selectFollowAlertWK.value
    
    //他社成約車種
    this_form.selectGiveupMaker.value = this_form.selectGiveupMakerWK.value
    this_form.selectGiveupCar.value = this_form.selectGiveupCarWK.value
}

/* 2012/03/07 TCS 河原 【SALES_2】 START
$(function () {
    //ポップアップの内のリストの最後の横線を消す
    $(".nscListBoxSetIn li:last-child").addClass("end");
    
    //各プロセスポップアップの最後の横線を消す
    $("#popupTrigger4").click(function () {
        $(".nscListBoxSetIn li:last-child").addClass("end");
    });
    $("#popupTrigger5").click(function () {
        $(".nscListBoxSetIn li:last-child").addClass("end");
    });
    $("#popupTrigger6").click(function () {
        $(".nscListBoxSetIn li:last-child").addClass("end");
    });
});
2012/03/07 TCS 河原 【SALES_2】 END */

//2012/08/23 TCS 山口 【A STEP2】次世代e-CRB 新車商談機能改善 START
function newCustomerDummyErrorActivity() {
    //コンタクト履歴の再読み込みフラグを強制的にON
    $("#reloadFlg").val("1");

    //マーカー設定
    $("#scNscAllBoxContentsArea").removeClass("page1 page2 page3").addClass("page1");
    //移動
    $("#scNscAllBoxContentsArea").css({ "transform": "translate3d(0px, 0px, 0px)" });
    //ページ上部のナビゲーション
    SC3080201.setPageNavi();

    setTimeout(function () {
        //顧客編集実行
        CustomerEditPopUpOpen();
    }, 2000);
}
//2012/08/23 TCS 山口 【A STEP2】次世代e-CRB 新車商談機能改善 END