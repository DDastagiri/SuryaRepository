/*
━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
SC3080203.js
─────────────────────────────────────
機能： 顧客詳細(活動登録)
補足： 
作成：  
更新： 2012/03/07 TCS 河原 【SALES_2】
       2012/03/16 TCS 高橋 【SALES_2】(Sales1A ユーザテスト No.219)
       2012/03/20 TCS 相田 【SALES_2】(TCS_0315ka_04 対応)
更新： 2013/10/02 TCS 安田 次世代e-CRBタブレット iOS7.0 VersionUp対応（セールス）
更新： 2018/11/08 TCS 前田 TKM Next Gen e-CRB Project Application development Block B-1
─────────────────────────────────────
*/

/// <reference path="../jquery.js"/>
/// <reference path="../jquery.fingerscroll.js"/>

/****************************************************************

時間・アラーム入力ポップアップの動作

*****************************************************************/

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

$(function () {
    //日付コントロールのセレクタ

    /* 2013/10/02 TCS 安田 次世代e-CRBタブレット iOS7.0 VersionUp対応（セールス） START */
    var nextActDateControlSelectors = "#NextActStartDateSelector, #NextActEndDateTimeSelector, #NextActStartTimeSelector";
    var followDateControlSelectors = "#FollowStartDateSelector, #FollowEndDateTimeSelector, #FollowStartTimeSelector";
    var nextActDateControlSelectors2 = "#NextActStartDateTimeSelector";
    var followDateControlSelectors2 = "#FollowStartDateTimeSelector";
    /* 2013/10/02 TCS 安田 次世代e-CRBタブレット iOS7.0 VersionUp対応（セールス） END */


    //アラートクリック
    $("#scNscNextActivityPopPage1 li.Arrow").live("click", function (e) {
        //ページ移動
        $("#scNscNextActivityListArea").removeClass("page1").addClass("page2");
        $("#nextActivityPopupBack1").hide();
        $("#nextActivityPopupBack2").show();
    });

    //ポップアップの戻るボタンクリック
    $(".scNscNextActivityTimeCancellButton > a").bind("click", backAction);

    //次回活動日
    $("#NextActpopTri").bind("click", function (e) {
        //コンタクトタイプ取得
        var NextAct
        if ($(".NextActContactlist.Selection").children("span").size() > 0) {
            NextAct = $(".NextActContactlist.Selection").children("span").attr("value").split("_");
        } else {
            NextAct = this_form.NextActContactNextactivity.value.split("_");
        }

        //アラート非表示暫定処理(2012/01/10)
        if ($("#scNscNextActivityTimeWindown").is(":visible") === false) {
            //$(".AlertArea").css("display", "block");
            $("#custDtlPage3 #scNscNextActivityTimeWindown").css("top", "280px");
            $("#custDtlPage3 #scNscNextActivityTimeWindown").css("height", "250px");
            $("#custDtlPage3 .scNscNextActivityListArea > div.popupPage").css("height", "160px");
            /* 2012/03/16 TCS 高橋 【SALES_2】(Sales1A ユーザテスト No.219) START */
            $("#custDtlPage3 #scNscNextActivityTimeWindownBox").removeClass("headerGradient-Low");
            $("#custDtlPage3 #scNscNextActivityTimeWindownBox").addClass("headerGradient-high");
            /* 2012/03/16 TCS 高橋 【SALES_2】(Sales1A ユーザテスト No.219) END*/
        }

        //表示
        showPopup("next", NextAct[1]);
    });

    //フォロー
    $("#FollowpopTri").bind("click", function (e) {
        //コンタクトタイプ取得
        var NextAct
        if ($(".FollowContactlist.Selection").size() > 0) {
            NextAct = $(".FollowContactlist.Selection").children("span").attr("value");
        } else {
            NextAct = this_form.selectFollowContact.value
        }

        if (NextAct == "0") return;

        //アラート非表示暫定処理(2012/01/10)
        if ($("#scNscNextActivityTimeWindown").is(":visible") === false) {
            $("#custDtlPage3 #scNscNextActivityTimeWindown").css("top", "315px");
            $("#custDtlPage3 #scNscNextActivityTimeWindown").css("height", "215px");
            $("#custDtlPage3 .scNscNextActivityListArea > div.popupPage").css("height", "125px");
            /* 2012/03/16 TCS 高橋 【SALES_2】(Sales1A ユーザテスト No.219) START */
            $("#custDtlPage3 #scNscNextActivityTimeWindownBox").removeClass("headerGradient-high");
            $("#custDtlPage3 #scNscNextActivityTimeWindownBox").addClass("headerGradient-Low");
            /* 2012/03/16 TCS 高橋 【SALES_2】(Sales1A ユーザテスト No.219) END*/
        }

        //表示
        showPopup("follow", NextAct);
    });

    //アラート設定
    function setAlertState(type, contact) {

        //設定エリアの表示非表示
        $("#scNscNextActivityPopPage1 li.AlertArea").toggle(type === "next" && contact === "2");

        if (type === "next" && contact === "2") {
            /*** 来店 ***/
            //一旦クリア
            $("#scNscNextActivityPopPage2 li.Selection").removeClass("Selection");
            //キー設定
            var ano = type === "next" ? $("#NextActivityAlertNoHidden").val() : $("#FollowAlertNoHidden").val();
            //選択
            $("#scNscNextActivityPopPage2 li[alertno=" + ano + "]").addClass("Selection");
            //テキスト設定
            $("#nextActivityPopupSelectAlert").text($("#scNscNextActivityPopPage2 li.Selection").find("span").text());
        }
    }

    //表示
    function showPopup(type, contact) {
        //開いている場合閉じる
        if ($("#scNscNextActivityTimeWindown").is(":visible") === true) {
            //ロールバック
            rollbackTime();
            //ポップアップを閉じる
            closePopup();
            return;
        }

        //１ページ目を設定
        $("#scNscNextActivityListArea").removeClass("page2").addClass("page1");

        //文言初期化
        $("#nextActivityPopupBack1").show();
        $("#nextActivityPopupBack2").hide();

        //日付選択初期化
        $(nextActDateControlSelectors).toggle(type === "next");
        $(followDateControlSelectors).toggle(type === "follow");

        /* 2013/10/03 TCS 安田 【A STEP2】次世代e-CRB iOs7対応 START */
        $(nextActDateControlSelectors2).toggle(type === "next");
        $(followDateControlSelectors2).toggle(type === "follow");
        /* 2013/10/03 TCS 安田 【A STEP2】次世代e-CRB iOs7対応 END */

        //開始のラベル初期化
        $("#NextActTimePopupTitle1").toggle(contact === "2");
        $("#NextActTimePopupTitle2").toggle(contact === "1");

        //開始日付
        if (type === "next") {
            //次回活動
            $("#NextActStartDateSelector").toggle(contact === "1");
            $("#NextActStartDateTimeSelector").toggle(contact === "2");
        } else {
            //フォロー
            $("#FollowStartDateSelector").toggle(contact === "1");
            $("#FollowStartDateTimeSelector").toggle(contact === "2");
        }

        //開始時間
        $("#scNscNextActivityPopPage1 li.startTime").toggle(contact === "1");

        //終了
        $("#scNscNextActivityPopPage1 li.endTime").toggle(contact === "2");

        //アラーム設定
        setAlertState(type, contact);

        //表示位置のクラスを設定
        $("#scNscNextActivityTimeWindown").removeClass("nextMode followMode")
        .addClass(type === "next" ? "nextMode" : "followMode");

        //２ページ目をスクロール化
        $("#scNscNextActivityPopPage2").fingerScroll();

        //表示
        $("#scNscNextActivityTimeWindown").fadeIn(300);

        //完了ボタン押下時の処理を登録
        $("a.scNscNextActivityTimeCompletionButton").unbind("click").bind("click", function (e) {
            commitTimeAlert(type, contact);
        });
    }

    //ポップアップ完了ボタンクリック
    function commitTimeAlert(type, contact) {
        if (type === "next") {
            //コミット値保存
            saveNextActDateValue();

            //アラート
            $("#NextActivityAlertNoHidden").val($("#scNscNextActivityPopPage2 li.Selection").attr("alertno"));
            //親画面のアラート名設定
            $(".NextActAletName").text($("#nextActivityPopupSelectAlert").text());
            //日時
            $(".NextActTime").text(getDisplayDate("NextTime"));

        } else {
            //コミット値保存
            saveFollowDateValue();
            //アラート
            $("#FollowAlertNoHidden").val($("#scNscNextActivityPopPage2 li.Selection").attr("alertno"));
            //親画面のアラート名設定
            $(".FollowAletName").text($("#nextActivityPopupSelectAlert").text());
            //日時
            $(".FollowTime").text(getDisplayDate("FllowTime"));
        }

        //ポップアップを閉じる
        closePopup();
    }

    //次回活動の日付コントロール値確定
    function saveNextActDateValue() {
        //独自プロパティに保持
        $(nextActDateControlSelectors).each(function () {
            this.commitValue = this.valueAsDate;
        })

        /* 2013/10/03 TCS 安田 【A STEP2】次世代e-CRB iOs7対応 START */
        $(nextActDateControlSelectors2).each(function () {
            this.commitValue = changeStringToDateIcrop(this.value);
        })
        /* 2013/10/03 TCS 安田 【A STEP2】次世代e-CRB iOs7対応 END */;
    }

    //フォローの日付コントロール値確定
    function saveFollowDateValue() {
        //独自プロパティに保持
        $(followDateControlSelectors).each(function () {
            this.commitValue = this.valueAsDate;
        });

        /* 2013/10/03 TCS 安田 【A STEP2】次世代e-CRB iOs7対応 START */
        $(followDateControlSelectors2).each(function () {
            this.commitValue = changeStringToDateIcrop(this.value);
        });
        /* 2013/10/03 TCS 安田 【A STEP2】次世代e-CRB iOs7対応 END */;
    }

    //サーバー側にて設定された日付を確定値として保存
    saveNextActDateValue();
    saveFollowDateValue();

    //値をポップアップ表示前に戻す
    function rollbackTime() {
        if ($("#scNscNextActivityTimeWindown").hasClass("nextMode") === true) {
            //次回活動日用のポップアップ
            $(nextActDateControlSelectors).each(function () {
                this.valueAsDate = this.commitValue;
            });

            /* 2013/10/03 TCS 安田 【A STEP2】次世代e-CRB iOs7対応 START */
            //次回活動日用のポップアップ
            $(nextActDateControlSelectors2).each(function () {
                this.value = this.commitValue;
            });
            /* 2013/10/03 TCS 安田 【A STEP2】次世代e-CRB iOs7対応 END */;
        } else {
            //フォロー用のポップアップ
            $(followDateControlSelectors).each(function () {
                this.valueAsDate = this.commitValue;
            });

            /* 2013/10/03 TCS 安田 【A STEP2】次世代e-CRB iOs7対応 START */
            //次回活動日用のポップアップ
            $(followDateControlSelectors2).each(function () {
                this.value = this.commitValue;
            });
            /* 2013/10/03 TCS 安田 【A STEP2】次世代e-CRB iOs7対応 END */;
        }
    }

    //戻るアクション
    function backAction() {
        if ($("#scNscNextActivityListArea").hasClass("page1") === true) {
            //ページ１
            rollbackTime();    //ロールバック
            //ポップアップを閉じる
            closePopup();
        } else {
            //ページ２
            $("#scNscNextActivityListArea").removeClass("page2").addClass("page1");
            $("#nextActivityPopupBack1").show();
            $("#nextActivityPopupBack2").hide();
        }
    }

    //アラームの選択
    $("#scNscNextActivityPopPage2 li").bind("click", function (e) {
        //選択クリア
        $("#scNscNextActivityPopPage2 li.Selection").removeClass("Selection");
        $(this).addClass("Selection");
        $("#nextActivityPopupSelectAlert").text($(this).find("span").text());
        backAction();
    });

    //ポップアップクローズの監視
    $(document.body).bind("mousedown touchstart", function (e) {
        if ($("#scNscNextActivityTimeWindown").is(":visible") === false) return;
        if ($(e.target).is("#scNscNextActivityTimeWindown, #scNscNextActivityTimeWindown *, #NextActpopTri, #NextActpopTri *, #FollowpopTri, #FollowpopTri *") === false) {
            //ロールバック
            rollbackTime();
            //ポップアップを閉じる
            closePopup();
        }
    });

    //クローズ処理
    function closePopup() {
        if ($("#registOverlayBlack").attr("class").indexOf("BGColor", 0) != -1) {
            //ロード時のフィルタを非表示
            $("#registOverlayBlack").css("display", "none")

            //ロードアイコンの設定を解除
            $("#processingServer").removeClass("show");
            $("#processingServer").removeClass("NextActTimeLoadingAnimation");
            $("#processingServer").removeClass("FollowTimeLoadingAnimation");
            $("#registOverlayBlack").removeClass("BGColor");

            //読み込みを停止
            stop();
        }
        $("#scNscNextActivityTimeWindown").fadeOut(300);
    }

});
/****************************************************************

次回活動エリア-次回活動入力欄の動作

*****************************************************************/
// 分類欄(次回活動活動) //
$(function () {
    //コンタクトタイプ取得
    //2012/03/07 TCS 河原 【SALES_2】 START
    var NextActTypeAry
    var NextActType
    if($(".NextActContactlist.Selection").children("span").size() > 0){
        NextActTypeAry = $(".NextActContactlist.Selection").children("span").attr("value").split("_");
        NextActType = NextActTypeAry[0];
    }else{
        NextActType = this_form.NextActNextAct.value;
    }
    //2012/03/07 TCS 河原 【SALES_2】 END
    
    //時分とアラート名を初期化
    $(".NextActAletName").text($("#scNscNextActivityPopPage2 li[alertno=" + $("#NextActivityAlertNoHidden").val() + "] span").text());
    $(".NextActTime").text(getDisplayDate("NextTime"));
    $("#nextActTimeAndArt").toggle(NextActType === "2");
    
    //次回活動分類ポップアップのコンタクトタイプを選択した時の動作
    $(".NextActContactlist").live("click", function (e) {
        var NextAct = $(this).children("span").attr("value").split("_");
        
        //選択状態クリア
        $(".NextActContactlist").removeClass("Selection");
        
        //選択した行にチェックマークをつける
        $(this).addClass("Selection");
        
        //選択したコンタクトタイプ設定
        $("#selectNextActContact").val($(this).attr("value"));
        
        //ポップアップを閉じる
        $("#bodyFrame").trigger("click.popover");
        
        //表示用日付文字列を設定
        $(".NextActTime").text(getDisplayDate("NextTime"));
        
        //選択したコンタクト名を設定
        $(".scNscNextActContactName").text($(this).text());
        //2012/03/07 TCS 河原 【SALES_2】 START
        this_form.NextActContactTitle.value = $(this).text();
        
        //選択された項目の値を設定
        this_form.NextActNextAct.value = NextAct[0]
        this_form.NextActFromTo.value = NextAct[1]
        
        //2012/03/07 TCS 河原 【SALES_2】 END
        
        if (NextAct[0] == "2") {
            $("#confirmContents60 .nscListBoxSet .nscListBoxSetIn .nscListBoxSetRight").animate({ width: "show" }, 300);
            $("#FollowFlg").val("1");
            $("#NextActivityFromToFlg").val("1");
        } else {
            $("#confirmContents60 .nscListBoxSet .nscListBoxSetIn .nscListBoxSetRight").animate({ width: "hide" }, 300);
            $("#FollowFlg").val("2");
            $("#NextActivityFromToFlg").val("0");
        }
        
        $("#nextActTimeAndArt").toggle(NextAct[0] === "2");
    });
    
    //キャンセルボタンクリック
    $(".scNscNextActContactCancellButton").click(function () {
        $("#bodyFrame").trigger("click.popover");
    });
});

/****************************************************************

次回活動エリア-次回フォロー入力欄の動作

*****************************************************************/
// 分類欄(次回フォロー) //
$(function () {
    //コンタクトタイプ取得
    var NextActType
    if($(".FollowContactlist.Selection").children("span").size() > 0){
        NextActType = $(".FollowContactlist.Selection").children("span").attr("value");
    }else{
        if(this_form.selectFollowContact.value != 0){
            NextActType = "1"
        }else{
            NextActType = "0"
        }
    }
    //時分とアラート名を初期化
    $(".FollowAletName").text($("#scNscNextActivityPopPage2 li[alertno=" + $("#FollowAlertNoHidden").val() + "] span").text());
    if (NextActType === "0") {
        $(".FollowTime").text("");
    } else {
        $(".FollowTime").text(getDisplayDate("FllowTime"));
    }
    //次回フォローのコンタクトタイプを選択した際の動作
    $(".FollowContactlist").live("click", function (e) {
        var NextAct = $(this).children("span").attr("value");
        
        //選択したコンタクト名を設定
        $(".scNscFollowContactName").text($(this).text());
        //2012/03/07 TCS 河原 【SALES_2】 START
        this_form.FollowContactTitle.value = $(this).text();
        
        //選択された項目の値を設定
        this_form.FollowFromTo.value = NextAct
        //2012/03/07 TCS 河原 【SALES_2】 END
        
        //チェックマークをつける
        $(".FollowContactlist").removeClass("Selection");
        $(this).addClass("Selection");
        
        //コンタクト方法
        $("#selectFollowContact").val($(this).attr("value"));
        
        //ポップアップ閉じる
        $("#bodyFrame").trigger("click.popover");
        
        if (NextAct === "0") {
            //Noneを選択
            $(".FollowTime").text("");
            $("#FollowAlertNoHidden").val("0");
            $("#FollowFromToFlg").val("0");
            //アラームをNoneに設定
            $(".FollowAletName").text($("#scNscNextActivityPopPage2 li[alertno=0] span").eq(0).text());
        } else {
            //表示用日付文字列を設定
            $(".FollowTime").text(getDisplayDate("FllowTime"));
            $("#FollowFromToFlg").val(NextAct == "1" ? "0" : "1");
        }
    });
    
    //キャンセルボタン押下時の動作
    $(".scNscFollowContactCancellButton").click(function () {
        $("#bodyFrame").trigger("click.popover");
    });
});


/****************************************************************

共通

*****************************************************************/
//表示用日付文字列取得
function getDisplayDate(area) {
    
    var fromDate = null;   //Dateを想定
    var fromTime = "";     //Stringを想定
    var toTime = "";       //Stringを想定
    
    //時間文字列を作成
    function getTimeString(dateValue) {
        //HH:MM
        return dateValue !== null ? dateValue.getHours() + ":" + ("0" + dateValue.getMinutes()).slice(-2) : "";
    }
    
    //1:Fromのみ 2:From-To
    var timeType; 
    
    if (area === "ActTime") {
        //活動時間
        timeType = "2";
        fromDate = $("#ActTimeFromSelector").get(0).valueAsDate;
        fromTime = getTimeString(fromDate);
        toTime = $("#ActTimeToSelector").val();
        
    } else if (area === "NextTime") {
        //次回活動
        //2012/03/07 TCS 河原 【SALES_2】 START
        if($(".NextActContactlist.Selection").size() > 0){
            timeType = $(".NextActContactlist.Selection").children("span").attr("value").split("_")[0];
        }else{
            timeType = this_form.NextActContactNextactivity.value.split("_")[0];
        }
        //2012/03/07 TCS 河原 【SALES_2】 END
        
        if (timeType === "1") {
            //来店以外
            if($("#NextActStartDateSelector").size() > 0){
                fromDate = $("#NextActStartDateSelector").get(0).valueAsDate;
            }else{
                fromDate = $("#NextActStartDateSelectorWK").get(0).valueAsDate;
            }
            
            if($("#NextActStartTimeSelector").size() > 0){
                fromTime = $("#NextActStartTimeSelector").val();
            }else{
                fromTime = $("#NextActStartTimeSelectorWK").val();
            }
            if(fromTime.charAt(0) == "0"){
                fromTime = fromTime.substr(1,5)
            }
        } else {
            //来店
            if($("#NextActStartDateTimeSelector").size() > 0){
/* 2013/10/02 TCS 安田 次世代e-CRBタブレット iOS7.0 VersionUp対応（セールス） START */
                fromDate = changeStringToDateIcrop($("#NextActStartDateTimeSelector").val());
/* 2013/10/02 TCS 安田 次世代e-CRBタブレット iOS7.0 VersionUp対応（セールス） END */
                
            }else{
/* 2013/10/02 TCS 安田 次世代e-CRBタブレット iOS7.0 VersionUp対応（セールス） START */
                fromDate = changeStringToDateIcrop($("#NextActStartDateTimeSelectorWK").val());
/* 2013/10/02 TCS 安田 次世代e-CRBタブレット iOS7.0 VersionUp対応（セールス） END */

            }
            
            fromTime = getTimeString(fromDate);
        }
        
        if($("#NextActEndDateTimeSelector").size() > 0){
            toTime = $("#NextActEndDateTimeSelector").val();
        }else{
            toTime = $("#NextActEndDateTimeSelectorWK").val();
        }
        if(toTime.charAt(0) == "0"){
            toTime = toTime.substr(1,5)
        }
        
    } else if (area === "FllowTime") {
        //フォロー
        timeType = $(".FollowContactlist.Selection").children("span").attr("value");
        
        if (timeType === "1") {
            //来店以外
            if($("#FollowStartDateSelector").size() > 0){
                fromDate = $("#FollowStartDateSelector").get(0).valueAsDate;
            }else{
                fromDate = $("#FollowStartDateSelectorWK").get(0).valueAsDate;
            }
            if($("#FollowStartTimeSelector").size() > 0){
                fromTime = $("#FollowStartTimeSelector").val();
            }else{
                fromTime = $("#FollowStartTimeSelectorWK").val();
            }
            if(fromTime.charAt(0) == "0"){
                fromTime = fromTime.substr(1,5)
            }
        } else {
            //来店
            if($("#FollowStartDateTimeSelector").size() > 0){
/* 2013/10/02 TCS 安田 次世代e-CRBタブレット iOS7.0 VersionUp対応（セールス） START */
                fromDate = changeStringToDateIcrop($("#FollowStartDateTimeSelector").val());
/* 2013/10/02 TCS 安田 次世代e-CRBタブレット iOS7.0 VersionUp対応（セールス） END */
            }else{
/* 2013/10/02 TCS 安田 次世代e-CRBタブレット iOS7.0 VersionUp対応（セールス） START */
                fromDate = changeStringToDateIcrop($("#FollowStartDateTimeSelectorWK").val());
/* 2013/10/02 TCS 安田 次世代e-CRBタブレット iOS7.0 VersionUp対応（セールス） END */
            }
            fromTime = getTimeString(fromDate);
        }
        if($("#FollowEndDateTimeSelector").size() > 0){
            toTime = $("#FollowEndDateTimeSelector").val();
        }else{
            toTime = $("#FollowEndDateTimeSelectorWK").val();
        }
        if(toTime.charAt(0) == "0"){
            toTime = toTime.substr(1,5)
        }
    }
    
    //日付書式
    var Format = this_form.dateFormt.value;
    var dateString = "";
    
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

//2012/03/07 TCS 河原 【SALES_2】 START
/****************************************************************
ポップアップの後読み込み対応
*****************************************************************/
$(function () {
    //次回活動分類ポップアップ
    $("#NextActContactTrigger").click(function () {
        var flg = $("#NextActPopupFlg").attr("value");
        if (flg == "0")
        {
            $("#processingServer").addClass("NextActContactPopupLoadingAnimation");
            $("#registOverlayBlack").addClass("BGColor");
            $("#NextActContactButton").click();
        }
        $(".nscListBoxSetIn li:last-child").addClass("end");
    });
    
    //予約フォロー分類ポップアップ
    $("#FollowContactTrigger").click(function () {
        var flg = $("#FollowContactPopupFlg").attr("value");
        if (flg == "0")
        {
            $("#processingServer").addClass("FollowContactPopupLoadingAnimation");
            $("#registOverlayBlack").addClass("BGColor");
            $("#FollowContactButton").click();
        }
        $(".nscListBoxSetIn li:last-child").addClass("end");
    });
        
    //他社成約車種ポップアップ
    $("#popOverButton2").click(function () {
        var flg = $("#GiveupReasonPopupFlg").attr("value");
        if (flg == "0")
        {
            $("#processingServer").addClass("GiveupReasonPopupLoadingAnimation");
            $("#registOverlayBlack").addClass("BGColor");
            $("#GiveupReasonButton").click();
        }
        $(".nscListBoxSetIn li:last-child").addClass("end");
    });
    
    //次回活動日ポップアップ
    $("#NextActpopTri").click(function () {
        var flg = $("#NextActTimePopupFlg").attr("value");
        if (flg == "0")
        {
            $("#processingServer").addClass("NextActTimeLoadingAnimation");
            $("#registOverlayBlack").addClass("BGColor");
            $("#NextActTimeButton").click();
            $("#registOverlayBlack").css("display","none")
        }
    });
    
    //予約フォロー活動日ポップアップ
    $("#FollowpopTri").click(function () {
        var NextAct = this_form.selectFollowContact.value
        if (NextAct == "0") return;
        var flg = $("#NextActTimePopupFlg").attr("value");
        if (flg == "0")
        {
            $("#processingServer").addClass("FollowTimeLoadingAnimation");
            $("#registOverlayBlack").addClass("BGColor");
            $("#FollowTimeButton").click();
            $("#registOverlayBlack").css("display","none")
        }
    });
});
//次回活動分類ポップアップ(後処理)
function setNextActContactPageOpenEnd() {
    listname = "#NextActContactlist";
    listvalue = this_form.selectNextActContact.value;
    $(listname + listvalue).addClass("Selection");
    $(".nscListBoxSetIn li:last-child").addClass("end");
    $("#processingServer").removeClass("NextActContactPopupLoadingAnimation");
    $("#registOverlayBlack").removeClass("BGColor");
}

//予約フォロー活動分類ポップアップ(後処理)
function setFollowContactPageOpenEnd() {
    listname = "#FollowContactlist";
    listvalue = this_form.selectNextActContact.value;
    $(listname + listvalue).addClass("Selection");
    $(".nscListBoxSetIn li:last-child").addClass("end");
    $("#processingServer").removeClass("FollowContactPopupLoadingAnimation");
    $("#registOverlayBlack").removeClass("BGColor");
}

//他社成約車種ポップアップ(後処理)
function setGiveupReasonPageOpenEnd() {
    $("#processingServer").removeClass("GiveupReasonPopupLoadingAnimation");
    $("#registOverlayBlack").removeClass("BGColor");
}
//次回活動日ポップアップ(後処理)
function setNextActTimePageOpenEnd() {
    $("#scNscNextActivityListArea").find("input").addClass("icrop-DateTimeSelector")
    //2012/03/20 TCS 相田 【SALES_2】(TCS_0315ka_04 対応) START
    if ($(".NextActContactlist.Selection").size() > 0) {
        timeType = $(".NextActContactlist.Selection").children("span").attr("value").split("_")[0];
    } else {
        timeType = this_form.NextActContactNextactivity.value.split("_")[0];
    }
    //2012/03/20 TCS 相田 【SALES_2】(TCS_0315ka_04 対応) END
    showPopup("next", timeType);
    $("#processingServer").removeClass("NextActTimeLoadingAnimation");
    $("#registOverlayBlack").removeClass("BGColor");
}
//予約フォローポップアップ(後処理)
function setFollowTimePageOpenEnd() {
    $("#scNscNextActivityListArea").find("input").addClass("icrop-DateTimeSelector")
    timeType = "1"
    showPopup("follow", timeType);
    $("#processingServer").removeClass("FollowTimeLoadingAnimation");
    $("#registOverlayBlack").removeClass("BGColor");
}
//表示
function showPopup(type, contact) {
    //１ページ目を設定
    $("#scNscNextActivityListArea").removeClass("page2").addClass("page1");
    //文言初期化
    $("#nextActivityPopupBack1").show();
    $("#nextActivityPopupBack2").hide();
    
    
/* 2013/10/03 TCS 安田 【A STEP2】次世代e-CRB iOs7対応 START */
    var nextActDateControlSelectors = "#NextActStartDateSelector, #NextActEndDateTimeSelector, #NextActStartTimeSelector";
    var followDateControlSelectors = "#FollowStartDateSelector, #FollowEndDateTimeSelector, #FollowStartTimeSelector";
    var nextActDateControlSelectors2 = "#NextActStartDateTimeSelector";
    var followDateControlSelectors2 = "#FollowStartDateTimeSelector";
/* 2013/10/03 TCS 安田 【A STEP2】次世代e-CRB iOs7対応 END */
    
    //日付選択初期化
    $(nextActDateControlSelectors).toggle(type === "next");
    $(followDateControlSelectors).toggle(type === "follow");

/* 2013/10/03 TCS 安田 【A STEP2】次世代e-CRB iOs7対応 START */
    $(nextActDateControlSelectors2).toggle(type === "next");
    $(followDateControlSelectors2).toggle(type === "follow");
/* 2013/10/03 TCS 安田 【A STEP2】次世代e-CRB iOs7対応 END */
    
    //開始のラベル初期化
    $("#NextActTimePopupTitle1").toggle(contact === "2");
    $("#NextActTimePopupTitle2").toggle(contact === "1");
    //開始日付
    if (type === "next") {
        //次回活動
        $("#NextActStartDateSelector").toggle(contact === "1");
        $("#NextActStartDateTimeSelector").toggle(contact === "2");
    } else {
        //フォロー
        $("#FollowStartDateSelector").toggle(contact === "1");
        $("#FollowStartDateTimeSelector").toggle(contact === "2");
    }
    //開始時間
    $("#scNscNextActivityPopPage1 li.startTime").toggle(contact === "1");
    //終了
    $("#scNscNextActivityPopPage1 li.endTime").toggle(contact === "2");
    //アラーム設定
    setAlertState(type, contact);
    //表示位置のクラスを設定
    $("#scNscNextActivityTimeWindown").removeClass("nextMode followMode")
    .addClass(type === "next" ? "nextMode" : "followMode");
    //２ページ目をスクロール化
    $("#scNscNextActivityPopPage2").fingerScroll();
    //表示
    $("#scNscNextActivityTimeWindown").fadeIn(300);
    //完了ボタン押下時の処理を登録
    $("a.scNscNextActivityTimeCompletionButton").unbind("click").bind("click", function (e) {
        commitTimeAlert(type, contact);
    });
}

//ポップアップ完了ボタンクリック
function commitTimeAlert(type, contact) {
    if (type === "next") {
        //コミット値保存
        saveNextActDateValue();
        //アラート
        $("#NextActivityAlertNoHidden").val($("#scNscNextActivityPopPage2 li.Selection").attr("alertno"));
        //親画面のアラート名設定
        $(".NextActAletName").text($("#nextActivityPopupSelectAlert").text());
        //日時
        $(".NextActTime").text(getDisplayDate("NextTime"));
    } else {
        //コミット値保存
        saveFollowDateValue();
        //アラート
        $("#FollowAlertNoHidden").val($("#scNscNextActivityPopPage2 li.Selection").attr("alertno"));
        //親画面のアラート名設定
        $(".FollowAletName").text($("#nextActivityPopupSelectAlert").text());
        //日時
        $(".FollowTime").text(getDisplayDate("FllowTime"));
    }
    //ポップアップを閉じる
    $("#scNscNextActivityTimeWindown").fadeOut(300);
}

//値をポップアップ表示前に戻す
function rollbackTime() {
/* 2013/10/03 TCS 安田 【A STEP2】次世代e-CRB iOs7対応 START */
    var nextActDateControlSelectors = "#NextActStartDateSelector, #NextActEndDateTimeSelector, #NextActStartTimeSelector";
    var followDateControlSelectors = "#FollowStartDateSelector, #FollowEndDateTimeSelector, #FollowStartTimeSelector";
    var nextActDateControlSelectors2 = "#NextActStartDateTimeSelector";
    var followDateControlSelectors2 = "#FollowStartDateTimeSelector";
/* 2013/10/03 TCS 安田 【A STEP2】次世代e-CRB iOs7対応 END */

    if ($("#scNscNextActivityTimeWindown").hasClass("nextMode") === true) {
        //次回活動日用のポップアップ
        $(nextActDateControlSelectors).each(function () {
            this.valueAsDate = this.commitValue;
        });
/* 2013/10/03 TCS 安田 【A STEP2】次世代e-CRB iOs7対応 START */
        $(nextActDateControlSelectors2).each(function () {
            this.value = this.commitValue;
        });
/* 2013/10/03 TCS 安田 【A STEP2】次世代e-CRB iOs7対応 END */
    } else {
        //フォロー用のポップアップ
        $(followDateControlSelectors).each(function () {
            this.valueAsDate = this.commitValue;
        });
/* 2013/10/03 TCS 安田 【A STEP2】次世代e-CRB iOs7対応 START */
        $(followDateControlSelectors2).each(function () {
            this.value = this.commitValue;
        });
/* 2013/10/03 TCS 安田 【A STEP2】次世代e-CRB iOs7対応 END */
    }
}

//アラート設定
function setAlertState(type, contact) {

    //設定エリアの表示非表示
    $("#scNscNextActivityPopPage1 li.AlertArea").toggle(type === "next" && contact === "2");
    if (type === "next" && contact === "2") {
        /*** 来店 ***/
        //一旦クリア
        $("#scNscNextActivityPopPage2 li.Selection").removeClass("Selection");
        //キー設定
        var ano = type === "next" ? $("#NextActivityAlertNoHidden").val() : $("#FollowAlertNoHidden").val();
        //選択
        $("#scNscNextActivityPopPage2 li[alertno=" + ano + "]").addClass("Selection");
        //テキスト設定
        $("#nextActivityPopupSelectAlert").text($("#scNscNextActivityPopPage2 li.Selection").find("span").text());
    }
}

//次回活動の日付コントロール値確定
function saveNextActDateValue() {
    /* 2013/10/03 TCS 安田 【A STEP2】次世代e-CRB iOs7対応 START */
    var nextActDateControlSelectors = "#NextActStartDateSelector, #NextActEndDateTimeSelector, #NextActStartTimeSelector";
    var followDateControlSelectors = "#FollowStartDateSelector, #FollowEndDateTimeSelector, #FollowStartTimeSelector";
    var nextActDateControlSelectors2 = "#NextActStartDateTimeSelector";
    var followDateControlSelectors2 = "#FollowStartDateTimeSelector";
    /* 2013/10/03 TCS 安田 【A STEP2】次世代e-CRB iOs7対応 END */

    //独自プロパティに保持
    $(nextActDateControlSelectors).each(function () {
        this.commitValue = this.valueAsDate;
    });
    
/* 2013/10/03 TCS 安田 【A STEP2】次世代e-CRB iOs7対応 START */
    $(nextActDateControlSelectors2).each(function () {
        this.commitValue = changeStringToDateIcrop(this.value);
    });
/* 2013/10/03 TCS 安田 【A STEP2】次世代e-CRB iOs7対応 END */
}

//2018/11/08 TCS 前田 TKM Next Gen e-CRB Project Application development Block B-1 START
//フォローの日付コントロール値確定
function saveFollowDateValue() {
    var nextActDateControlSelectors = "#NextActStartDateSelector, #NextActEndDateTimeSelector, #NextActStartTimeSelector";
    var followDateControlSelectors = "#FollowStartDateSelector, #FollowEndDateTimeSelector, #FollowStartTimeSelector";
    var nextActDateControlSelectors2 = "#NextActStartDateTimeSelector";
    var followDateControlSelectors2 = "#FollowStartDateTimeSelector";

    //独自プロパティに保持
    $(followDateControlSelectors).each(function () {
        this.commitValue = this.valueAsDate;
    });

    $(followDateControlSelectors2).each(function () {
        this.commitValue = changeStringToDateIcrop(this.value);
    });
}
//2018/11/08 TCS 前田 TKM Next Gen e-CRB Project Application development Block B-1 END
//2012/03/07 TCS 河原 【SALES_2】 END
