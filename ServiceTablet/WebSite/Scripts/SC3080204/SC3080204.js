//━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
//SC3080204.js
//─────────────────────────────────────
//機能： 顧客メモ
//補足：
//作成： 2011/12/??  ????
//更新： 2013/07/05 TMEJ 河原 IT9513_【A.STEP2】次世代e-CRBタブレット　新DB適応に向けた機能設計
//更新： 2014/12/03 TMEJ 成澤 IT9842_新プラットフォーム版e-CRB 移行方式見直しに伴う仕様変更対応
//更新： 2015/11/24 TM 浅野 TR-SVT-TMT-20151026-001
//更新： 2016/06/17 NSK 小牟禮 TR-SVT-TMT-20160524-001
//─────────────────────────────────────

// 初期ロード　//
function SC3080204PageLoad() {

    // 初期処理
    InitialProcess();

    //スクロール設定
    $("#messageInner01").fingerScroll();
    
    //スワイプの設定
    swipeSetting();

    
    // 以下、イベントをバインドする //
 
    // メモ欄タップ //
    $("#memoTextBox").focus(function (e) {

        //2014/12/03 TMEJ 成澤 IT9842_新プラットフォーム版e-CRB 移行方式見直しに伴う仕様変更対応 START
        if ($("#DBDiv").val() == "V3") {
            return
        }
        //2014/12/03 TMEJ 成澤 IT9842_新プラットフォーム版e-CRB 移行方式見直しに伴う仕様変更対応 END

        EditMemo();
    });

    // ＋ボタン押下 //
    $(".scNscCustomerMemoContentsPlusButoon").click(function (e) {

        //メモを入力モードにする
        $("#modeMemo").val('append');

        //2014/12/03 TMEJ 成澤 IT9842_新プラットフォーム版e-CRB 移行方式見直しに伴う仕様変更対応 START
        $("#memoTextBox").attr("disabled", "")
        $("#memoTextBox").css("background-color", "#FFFFFF")
        //2014/12/03 TMEJ 成澤 IT9842_新プラットフォーム版e-CRB 移行方式見直しに伴う仕様変更対応 END

        //右パネル(メモ参照・入力画面)の設定をする
        ModeSetting();

        $("#titleLabelMemo").text("");
        $("#memoTextBox").val(""); 

        var seqno = $("#activeSEQNOMemo").val();
        var idname = '';
        if (seqno != '') {
            //一覧選択状態を解除する
            idname = "#memolist" + seqno;
            $(idname).removeClass("scNscCustomerMemoListBoxActive");
            $(idname).addClass("scNscCustomerMemoListBoxDisable");
        }
        $("#memoTextBox").focus();
    });

    // 保存ボタン押下 //
    $(".scNscCustomerMemoContentsSaveButoon").click(function (e) {
        
        //顧客メモ未入力時
        if (trim($("#memoTextBox").val()) == '') {
            alert($("#noMemoText").text());
            return;
        }

        // 2015/11/24 TM 浅野 TR-SVT-TMT-20151026-001 Start
        //アクティブインジケータ表示フラグ：TRUE表示
        gIsRegist = true;
        // 2015/11/24 TM 浅野 TR-SVT-TMT-20151026-001 End

        //2013/07/05 TMEJ 河原 IT9513_【A.STEP2】次世代e-CRBタブレット　新DB適応に向けた機能設計 START

        //        $("#saveMemoButton").click();

        var prms = encodeURIComponent($("#memoTextBox").val());
        callback2.doCallback("InputCheckMemo", prms, function (result, context) {

            var resArray = result.split(",");

            if (resArray[0] == "999") {	//異常終了時エラーメッセージ
                alert(resArray[1]);
                return;
            } else {
                $("#SaveMemoButton").click();
            }
        });

        //2013/07/05 TMEJ 河原 IT9513_【A.STEP2】次世代e-CRBタブレット　新DB適応に向けた機能設計 END

    });

    // キャンセルボタン押下 //
    $(".scNscCustomerMemoContentsCancellationButoon").click(function (e) {

        //メモを参照モードにする
        $("#memoTextBox").blur();
        $("#modeMemo").val('look');

        //右パネル(メモ参照・入力画面)の設定をする
        ModeSetting();

        //2014/12/03 TMEJ 成澤 IT9842_新プラットフォーム版e-CRB 移行方式見直しに伴う仕様変更対応 START
        //選択しているメモがV3の場合、メモを非活性
        if ($("#DBDiv").val() == "V3") {
            $("#memoTextBox").attr("disabled", "disabled")
            $("#memoTextBox").css("background-color", "#EBEBE4")
        }
        //2014/12/03 TMEJ 成澤 IT9842_新プラットフォーム版e-CRB 移行方式見直しに伴う仕様変更対応 END

        //選択されていたメモを選択状態にする
        var seqno = $("#activeSEQNOMemo").val();
        if (seqno != '') {
            var idname = "#memolist" + seqno;
            $(idname).addClass("scNscCustomerMemoListBoxActive");
            $("#titleLabelMemo").text($(idname).children(".scNscCustomerMemoListTxt").children("span").text());
            $("#dateLabel").text($(idname).children(".updateDayHidden").text());
            $("#timeLabel").text($(idname).children(".updateTimeHidden").text());
            $("#memoTextBox").val($(idname).children(".memoDetailHidden").text());
        }
    });

    // 顧客メモ選択 //
    $(".scNscCustomerMemoListBoxDisable").click(function (e) {
        //追加・編集モード→参照モード
        if ($("#modeMemo").val() != 'look') {

            //参照モードにする
            $("#modeMemo").val('look');

            //右パネル(メモ参照・入力画面)の設定をする
            ModeSetting();
        }

        //選択されていたメモを解除する
        var seqno = $("#activeSEQNOMemo").val();
        var idname = '';
        if (seqno != '') {
            idname = "#memolist" + seqno;
            $(idname).removeClass("scNscCustomerMemoListBoxActive");
            $(idname).addClass("scNscCustomerMemoListBoxDisable");
        }

        //選択されていたメモを選択状態にする
        seqno = $(this).attr("value");
        idname = "#memolist" + seqno;
        $(idname).removeClass("scNscCustomerMemoListBoxDisable");
        $(idname).addClass("scNscCustomerMemoListBoxActive");
        //2013/07/05 TMEJ 河原 IT9513_【A.STEP2】次世代e-CRBタブレット　新DB適応に向けた機能設計 START
        $("#activeCSTMemoLockVersionHidden").val($(idname + ' input.cstMemoLockVersionHidden').val());
        //2013/07/05 TMEJ 河原 IT9513_【A.STEP2】次世代e-CRBタブレット　新DB適応に向けた機能設計 END

        //選択されていたメモをの内容を設定する
        $("#activeSEQNOMemo").val(seqno);               //SEQNo

        $("#titleLabelMemo").text($(this).children(".scNscCustomerMemoListTxt").children("span").text());   //タイトル
        $("#dateLabel").text($(this).children(".updateDayHidden").text());                                  //日付
        $("#timeLabel").text($(this).children(".updateTimeHidden").text());                                 //時間
        $("#memoTextBox").val($(this).children(".memoDetailHidden").text());    			                //メモ内容
        $("#memoTextBox").attr("RaedOnly", "True");  //読み取り専用

        //2014/12/03 TMEJ 成澤 IT9842_新プラットフォーム版e-CRB 移行方式見直しに伴う仕様変更対応 START
        if ($(this).children(".DBDiv").val() == "V3") {
            $("#memoTextBox").attr("disabled", "disabled")
            $("#memoTextBox").css("background-color", "#EBEBE4")
        } else {
            $("#memoTextBox").attr("disabled", "")
            $("#memoTextBox").css("background-color", "#FFFFFF")
        }
        $("#DBDiv").val($(this).children(".DBDiv").val());
        //2014/12/03 TMEJ 成澤 IT9842_新プラットフォーム版e-CRB 移行方式見直しに伴う仕様変更対応 END

        $("#memoTextBox").blur();                    //フォーカスアウト

        //参照モードにする
        $("#modeMemo").val('look');
    });

    
    // メモ一覧選択 //
    $("#messageInner01").click(function (e) {
        //メモを参照モードにする
        $("#memoTextBox").blur();
    });


    // ラベルの矩形処理

    // 2015/11/24 TM 浅野 TR-SVT-TMT-20151026-001 Start
    //$("#CustomLabel2Memo").CustomLabel({ 'useEllipsis': 'true' });  // 顧客詳細へボタン
    $("#CustomLabel2Memo").css({ 'white-space': 'nowrap', 'overflow': 'hidden', 'text-overflow': 'ellipsis', '-webkit-text-overflow': 'ellipsis' }); // 顧客詳細へボタン
    // 2015/11/24 TM 浅野 TR-SVT-TMT-20151026-001 End

    $("#titleLabelMemo").CustomLabel({ 'useEllipsis': 'true' });    // メモタイトル

    var $memolist = $('#scNscCustomerMemo #scNscCustomerMemoListArea .scNscCustomerMemoListBox').children();
    for (i = 0; i < $memolist.length; i++) {
        var $memoTxt = $memolist[i].children(0).children(0);
        $("#" + $memoTxt.id + "").CustomLabel({ 'useEllipsis': 'true' });            // 顧客一覧 (メモ)
    }

}

// 初期処理　//
function InitialProcess() {

    var seqno = $("#activeSEQNOMemo").val();

    //選択中の顧客メモを選択表示にする
    if (seqno != '') {
        var idname = "#memolist" + seqno;

        //参照モード
        if ($("#modeMemo").val() == 'look') {
            $(idname).addClass("scNscCustomerMemoListBoxActive");
            $("#titleLabelMemo").text($(idname).children(".scNscCustomerMemoListTxt").children("span").text());
            $("#dateLabel").text($(idname).children(".updateDayHidden").text());
            $("#timeLabel").text($(idname).children(".updateTimeHidden").text());
            $("#memoTextBox").val($(idname).children(".memoDetailHidden").text());

            // 2016/06/17 NSK 小牟禮 TR-SVT-TMT-20160524-001 START
            // メモ削除時にV3メモが編集できてしまうため、初期表示のタイミングで非活性に変更する処理を追加
            if ($(idname).children(".DBDiv").val() == "V3") {
                $("#memoTextBox").attr("disabled", "disabled")
                $("#memoTextBox").css("background-color", "#EBEBE4")
            }
            // 2016/06/17 NSK 小牟禮 TR-SVT-TMT-20160524-001 END

        }
    }

    //右パネル(メモ参照・入力画面)の設定をする
    ModeSetting();
}

// 最初のメモを選択状態にする　//
function SelectFirstMemo() {
    var $memolist = $('#scNscCustomerMemo #scNscCustomerMemoListArea .scNscCustomerMemoListBox').children();
    if ($memolist.length == 0) {

        //右パネル(メモ参照・入力画面)の設定をする
        $("#modeMemo").val('append');
        ModeSetting();

        $('#activeSEQNOMemo').val("");
        $("#titleLabelMemo").text("");
        $("#memoTextBox").val("");
    } else {

        //最初の１件目を選択状態にする
        $("#" + $memolist[0].id + "").click();
    }
}

// モード切替　//
function ModeSetting() {

    //追加モード
    if ($("#modeMemo").val() == 'append') {
        $(".scNscCustomerMemoContentsPlusButoon").css("display", "none");            //＋ボタン  非表示
        $(".scNscCustomerMemoContentsCancellationButoon").css("display", "block");   //キャンセルボタン  表示
        $(".scNscCustomerMemoContentsSaveButoon").css("display", "block");           //保存ボタン  表示
        $("#memoTextBox").attr("RaedOnly", "False");
        $("#dateLabel").text($("#todayHidden").val());
        $("#timeLabel").text($("#nowTimeHidden").val());
    }

    //編集モード
    if ($("#modeMemo").val() == 'edit') {
        $(".scNscCustomerMemoContentsPlusButoon").css("display", "none");            //＋ボタン  非表示
        $(".scNscCustomerMemoContentsCancellationButoon").css("display", "block");   //キャンセルボタン  表示
        $(".scNscCustomerMemoContentsSaveButoon").css("display", "block");           //保存ボタン  表示
        $("#memoTextBox").attr("RaedOnly", "False");
    }

    //参照モード
    if ($("#modeMemo").val() == 'look') {
        $(".scNscCustomerMemoContentsPlusButoon").css("display", "block");           //＋ボタン  表示
        $(".scNscCustomerMemoContentsCancellationButoon").css("display", "none");    //キャンセルボタン  非表示
        $(".scNscCustomerMemoContentsSaveButoon").css("display", "none");            //保存ボタン  非表示
        $("#memoTextBox").attr("RaedOnly", "True");
    }
}


var callback = {
    doCallback: function (method, argument, callbackFunction) {
        this.method = method;
        this.argument = argument;
        this.packedArgument = method + "," + argument;
        this.endCallback = callbackFunction;
        this.beginCallback();
    }

};


// メモ編集　//
function EditMemo() {

    //参照モード→編集モード
    if ($("#modeMemo").val() == 'look') {
    
        //選択されていたメモを解除する
        var seqno = $("#activeSEQNOMemo").val();
        if (seqno == '') {
            //メモを追加モードにする
            $("#modeMemo").val('append');
        } else {
            //メモを編集モードにする
            $("#modeMemo").val('edit');
        }

        //右パネル(メモ参照・入力画面)の設定をする
        ModeSetting();

        //SEQ Noをセットする
        var idname = '';
        if (seqno != '') {
            //一覧選択状態を解除する
            idname = "#memolist" + seqno;
            $(idname).removeClass("scNscCustomerMemoListBoxActive");
            $(idname).addClass("scNscCustomerMemoListBoxDisable");
        }
    }

}

// 顧客詳細ボタン　//
function CustomerMemoCloseButton() {
    parent.reloadMemo();
}

// Trim関数
function trim(argValue) {
    return String(argValue).replace(/^[ 　]*/gim, "").replace(/[ 　]*$/gim, "");
}

// コールバック関数定義　-------------------------------------------------
var callback2 = {
    doCallback: function (method, argument, callbackFunction) {
        this.method = method;
        this.argument = argument;
        this.packedArgument = method + "," + argument;
        this.endCallback = callbackFunction;
        this.beginCallback();
    }
};

// スワイプ用
var swipeOptions =
{
    swipeStatus: swipeStatus,
    threshold: 80
}

// スワイプ登録
function swipeSetting() {
    var swipeTarget = $("#scNscCustomerMemoListArea li").children("p:nth-child(1)");
    swipeTarget.swipe(swipeOptions);
}

// スワイプ実行時 (メモ削除)
function swipeStatus(event, phase, direction, distance) {

    var target = $(this).parent("li");
    var appendTarget = target.children("p:nth-child(2)");

    //2014/12/03 TMEJ 成澤 IT9842_新プラットフォーム版e-CRB 移行方式見直しに伴う仕様変更対応 START
    if (target.children(".DBDiv").val() == 'V3') {
        return;
    }
    //2014/12/03 TMEJ 成澤 IT9842_新プラットフォーム版e-CRB 移行方式見直しに伴う仕様変更対応 END

    if (phase == "end" && direction == "left") {

        var delString = $("#deleteLabel").text();

        var taskButton = $("<a class='scNscCustomerMemoListDeleteButton'>" + delString + "</a>");

        taskButton.fadeIn(100, function () {
            taskButton
			.click(function () {
			    var proTarget = $("<div class='Loading'><div class = 'Loadingicn'> <img src='../Styles/Images/SC3080204/animeicn.png'/></div></div>");
			    proTarget.fadeIn(100, function () {
			        target.css('opacity', 0.5);
			        proTarget.appendTo(target);
			    });
			    setTimeout(function () {
			        target.slideUp(50, function () {

			            //選択されていたメモを解除する
			            var seqno = $("#activeSEQNOMemo").val();
			            var idname = '';
			            if (seqno != '') {
			                idname = "#memolist" + seqno;
			                $(idname).removeClass("scNscCustomerMemoListBoxActive");
			                $(idname).addClass("scNscCustomerMemoListBoxDisable");
			            }

			            var selectseqno = target.attr("value");
			            var prms = selectseqno; 			       //SEQ No
			            prms = prms + "," + $('#listCountHidden').val();           //リスト行数
                        
			            callback2.doCallback("DeleteMemo", prms, function (result, context) {

			                var resArray = result.split(",");

			                if (resArray[0] == "999") {	//異常終了時エラーメッセージ
			                    alert(resArray[1]);
			                    return;
			                }

			                $('#listCountHidden').val(resArray[0]);
			                $('#countLabel').text(resArray[1]);

			                var $memolist = $('#scNscCustomerMemo #scNscCustomerMemoListArea .scNscCustomerMemoListBox').children();
			                if ($memolist.length <= 1) {

			                    //最後の１行の場合は、メモを入力モードにする
			                    $("#modeMemo").val('append')

			                    $('#activeSEQNOMemo').val("");

			                    $("#titleLabelMemo").text("");

			                    $("#memoTextBox").val("");

			                } else {
			                    //メモを参照モードにする
			                    $("#modeMemo").val('look');

			                    //削除したメモの次の行を選択する　(最終行ならば、１つ手前の行を選択する)
			                    var beforeSeqno = "";
			                    for (i = 0; i < $memolist.length; i++) {
			                        var seqnoTarget = $("#" + $memolist[i].id + "").attr("value");    //SEQ No
			                        if (seqnoTarget == selectseqno) {
			                            if ($memolist.length == (i + 1)) {
			                                //最終行の場合は、前メモ選択
			                                $('#activeSEQNOMemo').val(beforeSeqno);
			                                //2013/07/05 TMEJ 河原 IT9513_【A.STEP2】次世代e-CRBタブレット　新DB適応に向けた機能設計 START
			                                $("#activeCSTMemoLockVersionHidden").val($("#memolist" + beforeSeqno + " input.cstMemoLockVersionHidden").val());
			                                //2013/07/05 TMEJ 河原 IT9513_【A.STEP2】次世代e-CRBタブレット　新DB適応に向けた機能設計 END
			                            } else {
			                                //次メモ選択
			                                seqnoTarget = $("#" + $memolist[i + 1].id + "").attr("value");    //SEQ No
			                                $('#activeSEQNOMemo').val(seqnoTarget);
			                                //2013/07/05 TMEJ 河原 IT9513_【A.STEP2】次世代e-CRBタブレット　新DB適応に向けた機能設計 START
			                                $("#activeCSTMemoLockVersionHidden").val($("#memolist" + seqnoTarget + " input.cstMemoLockVersionHidden").val());
			                                //2013/07/05 TMEJ 河原 IT9513_【A.STEP2】次世代e-CRBタブレット　新DB適応に向けた機能設計 END
			                            }
			                        }

			                        beforeSeqno = seqnoTarget;
			                    }
			                }

			                // 初期処理
			                InitialProcess();
			                // 選択リスト削除
			                target.remove();

			            });

			        });
			    }, 500);
			})
			.appendTo(appendTarget);
        });
    }

    if ((phase == "cancel" && distance == 0) || (phase == "end" && direction == "right")) {
        target.find("a").fadeOut(100, function () {
            //削除ボタンを非表示にする。
            $(this).remove();
        });
    }
}
