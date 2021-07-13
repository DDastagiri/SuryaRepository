/// <reference path="../jquery.js"/>
/// <reference path="../jquery.fingerscroll.js"/>


/****************************************************************

時間・アラーム入力ポップアップの動作

*****************************************************************/

$(function () {

    //日付コントロールのセレクタ
    var nextActDateControlSelectors = "#NextActStartDateTimeSelector, #NextActStartDateSelector, #NextActEndDateTimeSelector, #NextActStartTimeSelector";
    var followDateControlSelectors = "#FollowStartDateTimeSelector, #FollowStartDateSelector, #FollowEndDateTimeSelector, #FollowStartTimeSelector";

    //アラートクリック
    $("#scNscNextActivityPopPage1 li.Arrow").bind("click", function (e) {
        //ページ移動
        $("#scNscNextActivityListArea").removeClass("page1").addClass("page2");
        $("#nextActivityPopupBack1").hide(0);
        $("#nextActivityPopupBack2").show(0);
    });

    //ポップアップの戻るボタンクリック
    $(".scNscNextActivityTimeCancellButton > a").bind("click", backAction);

    //次回活動日
    $("#NextActpopTri").bind("click", function (e) {
        //コンタクトタイプ取得
        var NextAct = $(".NextActContactlist.Selection").children("span").attr("value").split("_");

        //アラート非表示暫定処理(2012/01/10)
        if ($("#scNscNextActivityTimeWindown").is(":visible") === false) {
            //$(".AlertArea").css("display", "block");
            $("#custDtlPage3 #scNscNextActivityTimeWindown").css("top", "280px");
            $("#custDtlPage3 #scNscNextActivityTimeWindown").css("height", "250px");
            $("#custDtlPage3 .scNscNextActivityListArea > div.popupPage").css("height", "160px");
        }

        //表示
        showPopup("next", NextAct[1]);
    });

    //フォロー
    $("#FollowpopTri").bind("click", function (e) {
        //コンタクトタイプ取得
        var NextAct = $(".FollowContactlist.Selection").children("span").attr("value");
        if (NextAct == "0") return;

        //アラート非表示暫定処理(2012/01/10)
        if ($("#scNscNextActivityTimeWindown").is(":visible") === false) {
            //$(".AlertArea").css("display", "none");
            $("#custDtlPage3 #scNscNextActivityTimeWindown").css("top", "315px");
            $("#custDtlPage3 #scNscNextActivityTimeWindown").css("height", "215px");
            $("#custDtlPage3 .scNscNextActivityListArea > div.popupPage").css("height", "125px");
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
            $("#scNscNextActivityTimeWindown").fadeOut(300);
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

    //次回活動の日付コントロール値確定
    function saveNextActDateValue() {
        //独自プロパティに保持
        $(nextActDateControlSelectors).each(function () {
            this.commitValue = this.valueAsDate;
        });
    }

    //フォローの日付コントロール値確定
    function saveFollowDateValue() {
        //独自プロパティに保持
        $(followDateControlSelectors).each(function () {
            this.commitValue = this.valueAsDate;
        });
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
        } else {
            //フォロー用のポップアップ
            $(followDateControlSelectors).each(function () {
                this.valueAsDate = this.commitValue;
            });
        }
    }

    //戻るアクション
    function backAction() {

        if ($("#scNscNextActivityListArea").hasClass("page1") === true) {
            //ページ１
            rollbackTime();    //ロールバック
            //ポップアップを閉じる
            $("#scNscNextActivityTimeWindown").fadeOut(300);
        } else {
            //ページ２
            $("#scNscNextActivityListArea").removeClass("page2").addClass("page1");
            $("#nextActivityPopupBack1").show(0);
            $("#nextActivityPopupBack2").hide(0);
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
            $("#scNscNextActivityTimeWindown").fadeOut(300);
        }
    });
});


/****************************************************************

次回活動エリア-次回活動入力欄の動作

*****************************************************************/


// 分類欄(次回活動活動) //
$(function () {


    //コンタクトタイプ取得
    var NextActType = $(".NextActContactlist.Selection").children("span").attr("value").split("_");
    //時分とアラート名を初期化
    $(".NextActAletName").text($("#scNscNextActivityPopPage2 li[alertno=" + $("#NextActivityAlertNoHidden").val() + "] span").text());
    $(".NextActTime").text(getDisplayDate("NextTime"));
    $("#nextActTimeAndArt").toggle(NextActType[0] === "2");

    //次回活動分類ポップアップのコンタクトタイプを選択した時の動作
    $(".NextActContactlist").bind("click", function (e) {

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
    var NextActType = $(".FollowContactlist.Selection").children("span").attr("value");

    //時分とアラート名を初期化
    $(".FollowAletName").text($("#scNscNextActivityPopPage2 li[alertno=" + $("#FollowAlertNoHidden").val() + "] span").text());
    if (NextActType === "0") {
        $(".FollowTime").text("");
    } else {
        $(".FollowTime").text(getDisplayDate("FllowTime"));
    }

    //次回フォローのコンタクトタイプを選択した際の動作
    $(".FollowContactlist").bind("click", function (e) {
        var NextAct = $(this).children("span").attr("value");

        //選択したコンタクト名を設定
        $(".scNscFollowContactName").text($(this).text());

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

    var timeType; //1:Fromのみ　2:From-To

    if (area === "ActTime") {
        
        //活動時間
        timeType = "2";
        fromDate = $("#ActTimeFromSelector").get(0).valueAsDate;
        fromTime = getTimeString(fromDate);
        toTime = $("#ActTimeToSelector").val();

    } else if (area === "NextTime") {
        
        //次回活動
        timeType = $(".NextActContactlist.Selection").children("span").attr("value").split("_")[0];
        
        if (timeType === "1") {
            //来店以外
            fromDate = $("#NextActStartDateSelector").get(0).valueAsDate;
            fromTime = $("#NextActStartTimeSelector").val();
        } else {
            //来店
            fromDate = $("#NextActStartDateTimeSelector").get(0).valueAsDate;
            fromTime = getTimeString(fromDate);
        }
        toTime = $("#NextActEndDateTimeSelector").val();

    } else if (area === "FllowTime") {
        
        //フォロー
        timeType = $(".FollowContactlist.Selection").children("span").attr("value");

        if (timeType === "1") {
            //来店以外
            fromDate = $("#FollowStartDateSelector").get(0).valueAsDate;
            fromTime = $("#FollowStartTimeSelector").val();
        } else {
            //来店
            fromDate = $("#FollowStartDateTimeSelector").get(0).valueAsDate;
            fromTime = getTimeString(fromDate);
        }
        toTime = $("#FollowEndDateTimeSelector").val();

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
