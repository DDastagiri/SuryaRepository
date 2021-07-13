//━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
//SC3080204.js
//─────────────────────────────────────
//機能： 顧客メモ
//補足：
//作成： 2011/11/26 TCS 安田
//更新： 2012/01/26 TCS 安田 【SALES_1B】顧客メモ入力欄の自動サイズ調整実行
//更新： 2012/01/26 TCS 安田 【SALES_1B】ボタン押下事ハイライト処理追加
//更新： 2012/01/26 TCS 安田 【SALES_1B】削除ボタン消す処理の追加
//更新： 2012/01/26 TCS 安田 【SALES_1B】入力チェック時Trim処理追加
//更新： 2012/01/26 TCS 安田 【SALES_1B】スクロール処理追加
//更新： 2012/01/26 TCS 安田 【SALES_1B】スワイプ処理調整
//更新： 2012/03/14 TCS 寺本 【SALES_2】 削除ボタン調整
//更新： 2012/06/04 TCS 安田 バグ修正
//更新： 2012/06/04 TCS 安田 FS開発
//更新： 2013/02/04 TCS 河原 GL0872
//更新： 2013/06/30 TCS 未　 【A STEP2】i-CROP新DB適応に向けた機能開発（既存流用）　
//更新： 2014/11/21 TCS 河原 TMT B案
//─────────────────────────────────────

// 顧客メモ一覧選択時に不正なアクションの発生を防ぐためフラグで制御する
var cacnelFlg = false;

// 初期ロード　//
function SC3080204PageLoad() {

    // 初期処理
    InitialProcess();

    //スクロール設定
    $("#messageInner01").fingerScroll();        //メモ一覧
    $("#memoEreaInner01").fingerScroll();

    //スワイプの設定
    swipeSetting();

    // 以下、イベントをバインドする //
 
    // メモ欄タップ (編集モード) //
    $("#memoTextBox").focus(function (e) {

        //2014/11/21 TCS 河原 TMT B案 START
        if ($("#DBDiv").val() == "V3") {
            return
        }
        //2014/11/21 TCS 河原 TMT B案 END
        
        //編集モードにする
        EditMemo();

        //2012/03/14 TCS 寺本 【SALES_2】 START
        DeleteButtonClear();
        //2012/03/14 TCS 寺本 【SALES_2】 END
    });

    // 2012/06/04 TCS 安田 FS開発 START
    // メモ欄タップ (参照モード) //
    //$("#memoView").bind("mousedown touchend", function (e) {
    $("#memoView").click(function (e) {

        if (linkClickFlg === false) {

            $("#memoTextBox").focus();


        } else {
            //URL, メールアドレスリンクタップ時
            linkClickFlg = false;
        }
    });
    // 2012/06/04 TCS 安田 FS開発 END

    // ＋ボタン押下 //
    $(".scNscCustomerMemoContentsPlusButoon").click(function (e) {
    
        //メモを入力モードにする
        $("#modeMemo").val('append');

        //右パネル(メモ参照・入力画面)の設定をする
        ModeSetting();

        $("#titleLabelMemo").text("");
        $("#memoTextBox").val("");
        // 2012/06/04 TCS 安田 FS開発 START
        $("#memoView").val("");
        // 2012/06/04 TCS 安田 FS開発 END

        // 2012/01/26 TCS 安田 【SALES_1B】顧客メモ入力欄の自動サイズ調整実行 START
        $("#memoTextBox").change();
        // 2012/01/26 TCS 安田 【SALES_1B】顧客メモ入力欄の自動サイズ調整実行 END
        
        var seqno = $("#activeSEQNOMemo").val();
        var idname = '';
        if (seqno != '') {
            //一覧選択状態を解除する
            idname = "#memolist" + seqno;
            $(idname).removeClass("scNscCustomerMemoListBoxActive");
            $(idname).addClass("scNscCustomerMemoListBoxDisable");
        }

        $("#memoTextBox").focus();

        // 2012/01/26 TCS 安田 【SALES_1B】スクロール処理追加 START
        //$("#memoEreaInner01").fingerScroll().refreshScrollBar();
        //$("#memoEreaInner01").fingerScroll();
        $("#memoEreaInner01 .scroll-inner").css({ "transform": "translate3d(0px, 0px, 0px)" });
        // 2012/01/26 TCS 安田 【SALES_1B】スクロール処理追加 END
    });
    

    // 保存ボタン押下 //
    $(".scNscCustomerMemoContentsSaveButoon").click(function (e) {

        //顧客メモ未入力時
        // 2012/01/26 TCS 安田 【SALES_1B】入力チェック時Trim処理追加 START
        if (trim($("#memoTextBox").val()) == '') {
            alert($("#noMemoText").text());
            return;
        }
        // 2012/01/26 TCS 安田 【SALES_1B】入力チェック時Trim処理追加 END


        // 2012/06/04 TCS 安田 バグ修正 START
        //顧客メモ保存処理追加
        //$("#saveMemoButton").click();

        //非同期にて、入力チェックを先に実行する
        SC3080201.startServerCallback();
        var prms = encodeURIComponent($("#memoTextBox").val());
        callback2.doCallback("InputCheckMemo", prms, function (result, context) {
            
            SC3080201.endServerCallback();

            //2013/02/04 TCS 河原 GL0872 オーバーレイ表示処理削除

            var resArray = result.split(",");

            if (resArray[0] == "999") {	//異常終了時エラーメッセージ
                alert(resArray[1]);
                return;
            } else {
                $("#saveMemoButton").click();
            }
        });
        // 2012/06/04 TCS 安田 バグ修正 END

    });

    // キャンセルボタン押下 //
    $(".scNscCustomerMemoContentsCancellationButoon").click(function (e) {

        //メモを参照モードにする
        $("#memoTextBox").blur();
        $("#modeMemo").val('look');
        
        //右パネル(メモ参照・入力画面)の設定をする
        ModeSetting();
        
        //選択されていたメモを選択状態にする
        var seqno = $("#activeSEQNOMemo").val();
        if (seqno != '') {
            var idname = "#memolist" + seqno;
            $(idname).addClass("scNscCustomerMemoListBoxActive");
            $("#titleLabelMemo").text($(idname).children(".scNscCustomerMemoListTxt").children("span").text());
            $("#dateLabel").text($(idname).children(".updateDayHidden").text());
            $("#timeLabel").text($(idname).children(".updateTimeHidden").text());
            
            $("#memoTextBox").val($(idname).children(".memoDetailHidden").text());
            // 2012/06/04 TCS 安田 FS開発 START
            //メモの原文を取得する
            var memoTxt = $(idname).children(".memoDetailHidden").get(0).innerHTML;
            //リンク加工したメモをセットする
            $("#memoView").get(0).innerHTML = editMemoLink(memoTxt);
            //高さを調節する
            editMemoHeight();
            // 2012/06/04 TCS 安田 FS開発 END

            //2014/11/21 TCS 河原 TMT B案 START
            if ($(idname).children(".DBDiv").val() == "V3") {
                $("#memoTextBox").attr("disabled", "disabled")
                $(".scNscCustomerMemoDetailsPaperAreaView").css("background-color", "#EBEBE4")
            } else {
                $("#memoTextBox").attr("disabled", "")
                $(".scNscCustomerMemoDetailsPaperAreaView").css("background-color", "#FFFFFF")
            }
            //2014/11/21 TCS 河原 TMT B案 END
        }

        // 2012/01/26 TCS 安田 【SALES_1B】顧客メモ入力欄の自動サイズ調整実行 START
        $("#memoTextBox").change();
        // 2012/01/26 TCS 安田 【SALES_1B】顧客メモ入力欄の自動サイズ調整実行 END

        // 2012/01/26 TCS 安田 【SALES_1B】スクロール処理追加 START
        //$("#memoEreaInner01").fingerScroll().refreshScrollBar();
        //$("#memoEreaInner01").fingerScroll();
        $("#memoEreaInner01 .scroll-inner").css({ "transform": "translate3d(0px, 0px, 0px)" });
        // 2012/01/26 TCS 安田 【SALES_1B】スクロール処理追加 END
    });

    // 顧客メモ選択 //
    $(".scNscCustomerMemoListBoxDisable").click(function (e) {

        // 2012/01/26 TCS 安田 【SALES_1B】削除ボタン消す処理の追加 START
        if (cacnelFlg == true) {    //キャンセルフラグ＝true時は処理をしない。
            cacnelFlg = false;
            return;
        }
        // 2012/01/26 TCS 安田 【SALES_1B】削除ボタン消す処理の追加 END

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
        // 2013/06/30 TCS 未 2013/10対応版 START
        $("#activeCSTMemoLockVersionHidden").val($(idname + ' input.cstMemoLockVersionHidden').val());
        // 2013/06/30 TCS 未 2013/10対応版 END

        //選択されていたメモをの内容を設定する
        $("#activeSEQNOMemo").val(seqno);               //SEQNo

        $("#titleLabelMemo").text($(this).children(".scNscCustomerMemoListTxt").children("span").text());   //タイトル
        $("#dateLabel").text($(this).children(".updateDayHidden").text());                                  //日付
        $("#timeLabel").text($(this).children(".updateTimeHidden").text());                                 //時間
        $("#memoTextBox").val($(this).children(".memoDetailHidden").text()); 		                    //メモ内容

        //2014/11/21 TCS 河原 TMT B案 START
        if ($(this).children(".DBDiv").val() == "V3") {
            $("#memoTextBox").attr("disabled", "disabled")
            $(".scNscCustomerMemoDetailsPaperAreaView").css("background-color", "#EBEBE4")
        } else {
            $("#memoTextBox").attr("disabled", "")
            $(".scNscCustomerMemoDetailsPaperAreaView").css("background-color", "#FFFFFF")
        }
        $("#DBDiv").val($(this).children(".DBDiv").val());
        //2014/11/21 TCS 河原 TMT B案 END

        // 2012/06/04 TCS 安田 FS開発 START
        //メモの原文を取得する
        var memoTxt = $(this).children(".memoDetailHidden").get(0).innerHTML;
        //リンク加工したメモをセットする
        $("#memoView").get(0).innerHTML = editMemoLink(memoTxt);
        //高さを調節する
        editMemoHeight();
        // 2012/06/04 TCS 安田 FS開発 END

        $("#memoTextBox").attr("RaedOnly", "True");  //読み取り専用
        $("#modeMemo").val('look');
        $("#memoTextBox").blur();                    //フォーカスアウト

        // 2012/01/26 TCS 安田 【SALES_1B】顧客メモ入力欄の自動サイズ調整実行 START
        $("#memoTextBox").change();
        // 2012/01/26 TCS 安田 【SALES_1B】顧客メモ入力欄の自動サイズ調整実行 END

        // 2012/01/26 TCS 安田 【SALES_1B】スクロール処理追加 START
        //$("#memoEreaInner01").fingerScroll().refreshScrollBar();
        //$("#memoEreaInner01").fingerScroll();
        $("#memoEreaInner01 .scroll-inner").css({ "transform": "translate3d(0px, 0px, 0px)" });
        // 2012/01/26 TCS 安田 【SALES_1B】スクロール処理追加 END

    });

    // メモ一覧選択 //
    $("#messageInner01").click(function (e) {
        //メモを参照モードにする
        $("#memoTextBox").blur();
    });
    
    // ラベルの矩形処理
    $("#CustomLabel2Memo").CustomLabel({ 'useEllipsis': 'true' });  // 顧客詳細へボタン
    $("#titleLabelMemo").CustomLabel({ 'useEllipsis': 'true' });    // メモタイトル

    var $memolist = $('#scNscCustomerMemo #scNscCustomerMemoListArea .scNscCustomerMemoListBox').children();
    //for (i = 0; i < $memolist.length; i++) {
    //    var $memoTxt = $memolist[i].children(0).children(0);
    //    $("#" + $memoTxt.id + "").CustomLabel({ 'useEllipsis': 'true' });            // 顧客一覧 (メモ)
    //}
    $memolist.find("span").CustomLabel({ 'useEllipsis': 'true' });

    // 2012/01/26 TCS 安田 【SALES_1B】ボタン押下事ハイライト処理追加 START
    //ボタンクリック時の色を登録
    SetColorToButton($(".scNscCustomerMemoContentsSaveButoon"));                // 保存ボタン押下
    SetColorToButton($(".scNscCustomerMemoContentsCancellationButoon"));        // キャンセルボタン
    SetColorToButton($(".scNscCustomerMemoContentsPlusButoon"));                // ＋ボタン押下
    
    // 顧客詳細ボタン
    $(".scNscCustomerMemoListHadderCustomerButoon").bind("mousedown touchstart", function (e) {
        $(".scNscCustomerMemoListHadderCustomerButoon").css("background-color", "#059BF5");
        $(".scNscCustomerMemoListHadderCustomerButoonArrow").css("background-color", "#059BF5");
    });
    $(".scNscCustomerMemoListHadderCustomerButoon").bind("mouseup touchend", function (e) {
        $(".scNscCustomerMemoListHadderCustomerButoon").css("background-color", "#FFF");
        $(".scNscCustomerMemoListHadderCustomerButoonArrow").css("background-color", "#FFF");
    });
    // 2012/01/26 TCS 安田 【SALES_1B】ボタン押下事ハイライト処理追加 END

    // 2012/01/26 TCS 安田 【SALES_1B】スクロール処理追加 START
    // キーボード表示時に必ずスクロールさせる
    $("#memoTextBox").live("focusin", function (e) {
        //alert($(window.parent).scrollTop());
        //var top = Math.max(0, $(window.parent).scrollTop() + 300);
        $(window.parent).scrollTop(159);
    });
    // 2012/01/26 TCS 安田 【SALES_1B】スクロール処理追加 END
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
            // 2012/06/04 TCS 安田 FS開発 START
            //メモの原文を取得する
            var memoTxt = $(idname).children(".memoDetailHidden").get(0).innerHTML;
            //リンク加工したメモをセットする
            $("#memoView").get(0).innerHTML = editMemoLink(memoTxt);
            //高さを調節する
            editMemoHeight();
            // 2012/06/04 TCS 安田 FS開発 END
        }
    }

    // 2012/01/26 TCS 安田 【SALES_1B】顧客メモ入力欄の自動サイズ調整実行 START
    //自動サイズ調整 (200px余白を設定する)
    $("#memoTextBox").autoResize({ extraSpace: 200, limit: 2000 });
    $("#memoTextBox").change();
    // 2012/01/26 TCS 安田 【SALES_1B】顧客メモ入力欄の自動サイズ調整実行 END

    //右パネル(メモ参照・入力画面)の設定をする
    ModeSetting();

    // 2012/01/26 TCS 安田 【SALES_1B】削除ボタン消す処理の追加 START
    //削除ボタンをクリアする 
    DeleteButtonClear();
    // 2012/01/26 TCS 安田 【SALES_1B】削除ボタン消す処理の追加 END

    // 2012/01/26 TCS 安田 【SALES_1B】スクロール処理追加 START
    $("#memoEreaInner01").fingerScroll();
    // 2012/01/26 TCS 安田 【SALES_1B】スクロール処理追加 END

}

// 最初のメモを選択状態にする　//
// 顧客情報のメモ欄クリック時に呼び出される //
function SelectFirstMemo() {
    var $memolist = $('#scNscCustomerMemo #scNscCustomerMemoListArea .scNscCustomerMemoListBox').children();
    if ($memolist.length == 0) {

        //右パネル(メモ参照・入力画面)の設定をする
        $("#modeMemo").val('append');

        $('#activeSEQNOMemo').val("");
        $("#titleLabelMemo").text("");
        $("#memoTextBox").val("");
        // 2012/06/04 TCS 安田 FS開発 START
        $("#memoView").val("");
        // 2012/06/04 TCS 安田 FS開発 END

        ModeSetting();
    } else {

        //最初の１件目を選択状態にする
        $("#" + $memolist[0].id + "").click();

        //スクロールを元の位置に戻す
        //$("#messageInner01").fingerScroll().refreshScrollBar();
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

        // 2012/06/04 TCS 安田 FS開発 START
        $("#memoTextBox").show();
        $("#memoView").hide();
        // 2012/06/04 TCS 安田 FS開発 END

        //2014/11/21 TCS 河原 TMT B案 START
        $("#memoTextBox").attr("disabled", "")
        $(".scNscCustomerMemoDetailsPaperAreaView").css("background-color", "#FFFFFF")
        //2014/11/21 TCS 河原 TMT B案 END


    }

    //編集モード
    if ($("#modeMemo").val() == 'edit') {
        $(".scNscCustomerMemoContentsPlusButoon").css("display", "none");            //＋ボタン  非表示
        $(".scNscCustomerMemoContentsCancellationButoon").css("display", "block");   //キャンセルボタン  表示
        $(".scNscCustomerMemoContentsSaveButoon").css("display", "block");           //保存ボタン  表示
        $("#memoTextBox").attr("RaedOnly", "False");

        // 2012/06/04 TCS 安田 FS開発 START
        $("#memoTextBox").show();
        $("#memoView").hide();
        // 2012/06/04 TCS 安田 FS開発 END
    }

    //参照モード
    if ($("#modeMemo").val() == 'look') {
        $(".scNscCustomerMemoContentsPlusButoon").css("display", "block");           //＋ボタン  表示
        $(".scNscCustomerMemoContentsCancellationButoon").css("display", "none");    //キャンセルボタン  非表示
        $(".scNscCustomerMemoContentsSaveButoon").css("display", "none");            //保存ボタン  非表示
        $("#memoTextBox").attr("RaedOnly", "True");

        // 2012/06/04 TCS 安田 FS開発 START
        $("#memoTextBox").hide();
        $("#memoView").show();
        // 2012/06/04 TCS 安田 FS開発 END
    }

}

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

        // 2012/06/04 TCS 安田 FS開発 START
        //カーソルを先頭にする
        $("#memoTextBox").show();
        $("#memoTextBox").get(0).selectionStart = 0;
        $("#memoTextBox").get(0).selectionEnd = 0;
        $("#memoView").hide();
        // 2012/06/04 TCS 安田 FS開発 END

        //右パネル(メモ参照・入力画面)の設定をする
        ModeSetting();
    }
}

// 2012/01/26 TCS 安田 【SALES_1B】削除ボタン消す処理の追加 START
// 削除ボタン削除　//
function DeleteButtonClear() {

    var $delbtnlist = $("#scNscCustomerMemo #scNscCustomerMemoListArea .scNscCustomerMemoListDeleteButton");

    for (i = 0; i < $delbtnlist.length; i++) {

        //削除ボタンを非表示にする。
        var delButton = $($delbtnlist[i]);
        var parentTag = delButton.parent().parent("li");

        //削除ボタンを非表示にする。
        delButton.remove();
        parentTag.children("p:nth-child(3)").css("width", "0px");
        parentTag.children("p:nth-child(2)").show();

    }
}
// 2012/01/26 TCS 安田 【SALES_1B】削除ボタン消す処理の追加 END

// 顧客詳細ボタン押下　//
function CustomerMemoCloseButton() {

    // 2012/01/26 TCS 安田 【SALES_1B】削除ボタン消す処理の追加 START
    //削除ボタンをクリアする
    DeleteButtonClear();
    // 2012/01/26 TCS 安田 【SALES_1B】削除ボタン消す処理の追加 END
    
    parent.reloadMemo();

}

// 2012/01/26 TCS 安田 【SALES_1B】ボタン押下事ハイライト処理追加 START
// ボタンクリック時色変更処理　//
function SetColorToButton(target) {
    target.bind("mousedown touchstart", function (e) {
        target.css("background-color", "#059BF5");
    });
    target.bind("mouseup touchend", function (e) {
        target.css("background-color", "#FFF");
    });
}
// 2012/01/26 TCS 安田 【SALES_1B】ボタン押下事ハイライト処理追加 END

// 2012/01/26 TCS 安田 【SALES_1B】削除ボタン消す処理の追加 START
// Trim関数作成　//
function trim(argValue) {
    return String(argValue).replace(/^[ 　]*/gim, "").replace(/[ 　]*$/gim, "");
}
// 2012/01/26 TCS 安田 【SALES_1B】削除ボタン消す処理の追加 END

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

// *** スワイプ用 ****************************************************************** //
var swipeOptions =
{
    swipeStatus: swipeStatus,
    threshold: 80
}

// スワイプ登録 ////
function swipeSetting() {
    var swipeTarget = $("#scNscCustomerMemoListArea li").children("p:nth-child(1)");
    swipeTarget.swipe(swipeOptions);
    //更新： 2012/01/26 TCS 安田 【SALES_1B】スワイプ処理調整 START
    var swipeTarget = $("#scNscCustomerMemoListArea li").children("p:nth-child(2)");
    swipeTarget.swipe(swipeOptions);
    //更新： 2012/01/26 TCS 安田 【SALES_1B】スワイプ処理調整 END
}

// スワイプ実行時 (メモ削除) //
function swipeStatus(event, phase, direction, distance) {

    var target = $(this).parent("li");
    var appendTarget = target.children("p:nth-child(3)");

    //2014/11/21 TCS 河原 TMT B案 START
    if (target.children(".DBDiv").val() == 'V3') {
        return;
    }
    //2014/11/21 TCS 河原 TMT B案 END

    //更新： 2012/01/26 TCS 安田 【SALES_1B】スワイプ処理調整 START
    //左スワイプにする
    if (phase == "end" && direction == "left") {

        if (($("#modeMemo").val() == 'append') || ($("#modeMemo").val() == 'edit')) {
            //キャンセル処理　(参照モードにする)
            cacnelFlg = true;
            $(".scNscCustomerMemoContentsCancellationButoon").click();
        }

        //削除ボタンをクリアする
        DeleteButtonClear();    

        target.children("p:nth-child(2)").hide();                   //日時項目
        target.children("p:nth-child(3)").css("width", "70px");     //削除ボタン項目

        //削除ボタンラベル
        var delString = $("#deleteHidden").val();

        var taskButton = $("<a class='scNscCustomerMemoListDeleteButton'>" + delString + "</a>");

        taskButton.fadeIn(100, function () {
            //削除ボタンクリック時
            taskButton.click(function () {
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
                                $("#modeMemo").val('append');
                                $('#activeSEQNOMemo').val("");
                                $("#titleLabelMemo").text("");
                                $("#memoTextBox").val("");
                                // 2012/06/04 TCS 安田 FS開発 START
                                $("#memoView").val("");
                                // 2012/06/04 TCS 安田 FS開発 END
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
                                            // 2013/06/30 TCS 未 2013/10対応版 START
                                            $("#activeCSTMemoLockVersionHidden").val($("#memolist" + beforeSeqno + " input.cstMemoLockVersionHidden").val());
                                            // 2013/06/30 TCS 未 2013/10対応版 END
                                        } else {
                                            //次メモ選択
                                            seqnoTarget = $("#" + $memolist[i + 1].id + "").attr("value");    //SEQ No
                                            $('#activeSEQNOMemo').val(seqnoTarget);
                                            // 2013/06/30 TCS 未 2013/10対応版 START
                                            $("#activeCSTMemoLockVersionHidden").val($("#memolist" + seqnoTarget + " input.cstMemoLockVersionHidden").val());
                                            // 2013/06/30 TCS 未 2013/10対応版 END
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

        cacnelFlg = false;
    }

    if ((phase == "cancel" && distance == 0) || (phase == "end" && direction == "right")) {
        DeleteButtonClear();    //削除ボタンをクリアする
    }
    //更新： 2012/01/26 TCS 安田 【SALES_1B】スワイプ処理調整 END
}




// 2012/06/04 TCS 安田 FS開発 START

//リンククリックフラグ　(true=クリック中)
var linkClickFlg = false;

/**
* URLリンククリック時
* @param {String} myUrl 対象URL
*/
function clickMemoLink(myUrl) {
    linkClickFlg = true;                //リンククリックフラグを(true=クリック中)にする
    var url = $('#urlSchemeBrowzer').val();

    //URLスキーム置き換え
    if (myUrl.match(/http:/i)) {
        myUrl = myUrl.replace(/http:/i, this_form.urlSchemeBrowzer.value + ":");
    }else{
        myUrl = myUrl.replace(/https:/i, this_form.urlSchemeBrowzers.value + ":");
    }

    location.href = myUrl;
}

/**
* メールアドレスリンククリック時
* @param {String} myUrl 対象URL
*/
function clickMemoAddress(myUrl) {
    linkClickFlg = true;                //リンククリックフラグを(true=クリック中)にする
    //location.href = "mailto:" & myUrl;
}

/**
* メモ内容を、リンク加工する
* @param {String} memoTxt 変換対象のHTMLタグ
* @return {String} リンク加工したHTMLタグ
*/
function editMemoLink(memoTxt) {


    //※※※　リンク機能をなくす場合、以下の記述のコメントをなくす　※※※
    //return memoTxt


    var result = memoTxt;

    //半角の空白を変換する　※前後に空白を付加する
    var re = / /g;
    result = result.replace(re, " &nbsp; ");

    //URLリンク加工する
    result = urlMemoEncode(result);
    //メールアドレスリンク加工する
    result = emailMemoEncode(result);

    //半角の空白を変換する　※前後に空白を削除する
    var re2 = / &nbsp; /g;
    result = result.replace(re2, "&nbsp;");

    //スクロールが途中で切れることがあるのを防ぐため改行コードを付加する
    result = result + "<br/><br/>"

    return result;
}

/**
* メモ(参照)の高さを調節する
*/
function editMemoHeight() {

    var iMemoHei = 430;
    var iHei = Number($("#memoView").height());
    if (iHei < iMemoHei) {
        var divStr = "<div style='width:565px; height:" + (iMemoHei - iHei) + "px; display:block'></div>";
        $("#memoView").get(0).innerHTML = $("#memoView").get(0).innerHTML + divStr;
    }
}

/**
* メモ内容を、URLリンク加工する
* @param {String} memoTxt メモ内容
* @return {String} URLリンク加工したメモ内容
*/
function urlMemoEncode(memoTxt) {

    var result = "";                                    //結果文字列
    var httpTag = new Array("http://", "https://");     //HTTP-URL判定用文字決
    var splitTag = new Array(" ", "　", "\n");          //区切り文字

    var iLength = memoTxt.length;                       //文字列長さ

    //HTTP-URL位置取得
    var iPos = indexOfArray(memoTxt, 0, httpTag);

    //1件も HTTPタグがない場合はなにもしないで返す
    if (iPos === -1) {  
        return memoTxt;
    }

    //最初のHTTP-URLまでの文字列を格納する
    result = result + memoTxt.substr(0, iPos);

    while (iPos !== -1) {

        //HTTP-URLの終了位置を取得する
        var iEnd = indexOfArray(memoTxt, iPos + 1, splitTag);
        if (iEnd === -1) {
            iEnd = iLength;
        }

        //HTTP-URLの取得
        var strLink = memoTxt.substr(iPos, iEnd - iPos);

        if ((strLink.toLocaleLowerCase() === httpTag[0]) || (strLink.toLocaleLowerCase() === httpTag[1])) {
            //HTTP-URL以降、文字がない場合
            result = result + strLink;
        } else {
            //HTTP-URLをリンク加工して、付加する
            result = result + "<A class='scNscCustomerMemoLink' onclick=clickMemoLink('" + strLink + "');>" + strLink + "</A>";
        }

        //次のHTTP-URL位置を取得する
        iPos = indexOfArray(memoTxt, iEnd + 1, httpTag);

        if (iPos > -1) {
            //次のHTTP-URL位置までの文字列を付加
            result = result + memoTxt.substr(iEnd, iPos - iEnd);
        } else {
            //最後までの文字列を付加
            result = result + memoTxt.substr(iEnd, iLength - iEnd);
        }
    }

    return result;
}

/**
* メモ内容を、メールアドレスリンク加工する
* @param {String} memoTxt メモ内容
* @return {String} メールアドレスリンク加工したメモ内容
*/
function emailMemoEncode(memoTxt) {

    var result = "";                                    //結果文字列
    var httpTag = new Array("http://", "https://");     //HTTP-URL判定用文字決
    var splitTag = new Array(" ", "　", "\n");          //区切り文字
    var iStartFlg = true;                               //処理開始判定用
    var iLength = memoTxt.length;                       //文字列長さ

    //@位置の取得
    var iPos = memoTxt.indexOf("@", 0);                 

    //メールアドレスがひとつもない場合
    if (iPos === -1) {
        return memoTxt;
    }
    while (iPos !== -1) {

        //メールアドレス開始位置取得
        var iStart = lastIndexOfArray(memoTxt, iPos + 1, splitTag);
        if (iStart === -1) {
            iStart = 0;
        }
        //メールアドレス終了位置取得
        var iEnd = indexOfArray(memoTxt, iPos + 1, splitTag);
        if (iEnd === -1) {
            iEnd = iLength;
        }

        //最初の１件以前の文字列を付加する
        if (iStartFlg === true) {
            if (iStart > 0) {
                result = memoTxt.substr(0, iStart);
            }
            iStartFlg = false;
        }

        //メールアドレスの文字列を取得する
        var strLink = memoTxt.substr(iStart, iEnd - iStart);

        //HTTP-URLでないか調査する
        var iHtmlIndex = indexOfArray(strLink, 0, httpTag);

        if ((iHtmlIndex > -1) || (strLink.substr(0, 1) === "@") || (strLink.substr(strLink.length - 1) === "@")) {
            //HTTP-URLもしくは、最初か最後の文字が@の場合
            result = result + strLink;
        } else {
            //メールアドレスをリンク加工して、付加する
            result = result + "<A class='scNscCustomerMemoLink' href='mailto:" + strLink + "' onclick=clickMemoAddress('" + strLink + "');>" + strLink + "</A>";
        }

        //次のメールアドレス位置を取得する
        iPos = memoTxt.indexOf("@", iEnd + 1);

        if (iPos > -1) {
            //次のメールアドレスの開始位置を取得する
            iStart = lastIndexOfArray(memoTxt, iPos, splitTag);
            if (iStart === -1) {
                //最後までの文字列を付加
                result = result + memoTxt.substr(iEnd, iLength - iEnd);
            } else {
                //次のメールアドレス位置までの文字列を付加
                result = result + memoTxt.substr(iEnd, iStart - iEnd);
            }
        } else {
            //最後までの文字列を付加
            result = result + memoTxt.substr(iEnd, iLength - iEnd);
        }
    }

    return result;
}

/**
* 配列で渡された検索文字列の中で、最初に一致する位置を取得する
* @param {String} memoTxt メモ内容
* @param {Number} iStart 検索開始位置
* @param {Array} ary 検索文字列の配列
* @return {String} 配列で渡された検索文字列の中で、最初に一致する位置
*/
function indexOfArray(memoTxt, iStart, ary) {

    //小文字で探す。小文字変換する
    var lowerMemoTxt = memoTxt.toLocaleLowerCase();

    var result = -1;
    for (var i = 0; i <= ary.length; i++) {
        //検索文字列で探す。
        var iPos = lowerMemoTxt.indexOf(ary[i], iStart);
        if (iPos != -1) {
            //検索位置がより小さい場合にセットする
            if (result == -1 || result > iPos) {
                result = iPos;
            }
        }
    }

    return result;
}

/**
* 配列で渡された検索文字列の中で、最後に一致する位置を取得する
* @param {String} memoTxt メモ内容
* @param {Number} iEnd 検索終了位置
* @param {Array} ary 検索文字列の配列
* @return {String} 配列で渡された検索文字列の中で、最後に一致する位置
*/
function lastIndexOfArray(memoTxt, iEnd, ary) {

    //小文字で探す。小文字変換する
    var lowerMemoTxt = memoTxt.toLocaleLowerCase();
    var memoSubStr = lowerMemoTxt.substr(0, iEnd);

    var result = -1;
    for (var i = 0; i <= ary.length; i++) {
        //検索文字列で探す。
        var iPos = memoSubStr.lastIndexOf(ary[i]);
        if (iPos != -1) {
            //検索位置がより大きい場合にセットする
            if (result == -1 || result < iPos) {
                result = iPos + 1;
            }
        }
    }

    return result;
}

// 2012/06/04 TCS 安田 FS開発 END
