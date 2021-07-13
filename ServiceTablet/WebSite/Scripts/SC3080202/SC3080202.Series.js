/// <reference path="../jquery.js"/>
/// <reference path="../jquery.fingerscroll.js"/>
/// <reference path="../jquery.NumericKeypad.js"/>
/// <reference path="../SC3080201/Common.js"/>
/***********************************************************
 希望車種イベント
 ***********************************************************/
$(function () {
    // 車種選択ボタン押下時
    $("#scNscSelectCarArea ul li.scNscSelectCarButton").live("click", function () {

        //選択設定
        $("#selectPosHidden").val($(this).parent().index());
        $("#selSeqnoHidden").val($(this).parent().children(":nth-child(12)").val());

        // ボタンの色変化
        if ($(this).hasClass("On") === false) {
            $(this).parent().parent().children().children(":nth-child(1)").removeClass("On");
            $(this).addClass("On");
        }

        //希望車種情報
        $("#selSeriescdHidden").val($(this).parent().children(":nth-child(2)").val());
        $("#selModelcdHidden").val($(this).parent().children(":nth-child(4)").val());
        $("#selColorcdHidden").val($(this).parent().children(":nth-child(6)").val());
        $("#selSeqnoHidden").val($(this).parent().children(":nth-child(12)").val());

        // 詳細情報表示
        // イメージ
        selectSelectedCarPictureAndLogo($(this).parent().children(":nth-child(10)").val(), $(this).parent().children(":nth-child(11)").val());

        //モデル
        $("#dispSelectedModel").text($(this).parent().children(":nth-child(5)").val());
        //色
        $("#dispSelectedColor").text($(this).parent().children(":nth-child(7)").val());
        //見積金額
        $("#dispSelectedMoney").text($(this).parent().children(":nth-child(8)").val());
        //台数
        $("#dispSelectedQuantity").text($(this).parent().children(":nth-child(9)").val());

        processDisplay();
    });

    // [>]ボタン押下時　10台後を表示
    $("#scNscSelectCarButtonArrow").live("click",
        function () {
            $("#selectPosHidden").val(parseInt($("#startPosHidden").val()) + 10);
            $("#startPosHidden").val(parseInt($("#startPosHidden").val()) + 10);
            $("#endPosHidden").val(parseInt($("#endPosHidden").val()) + 10);
            var id = parseInt($("#selectPosHidden").val()) + 1;
            $("#selSeqnoHidden").val($(".scNscSelectCarButtonList").children(":nth-child(" + id + ")").children(":nth-child(12)").val());

            // [>]アイコン表示有無
            if ($("#scNscSelectCarArea ul li.scNscSelectCarButton").parent().size() > $("#endPosHidden").val()) {
            } else {
                $("#scNscSelectCarButtonArrow").hide(0);
            }

            // [<]アイコン表示有無
            if ($("#startPosHidden").val() != 1) {
                $("#scNscSelectCarButtonArrowFor").show(0);
            }

            // ボタンの情報を変更
            $("#scNscSelectCarArea ul li.scNscSelectCarButton").each(function () {
                // ボタンのINDEX表示
                $(this).text($(this).parent().index());
                // 10台のみ表示
                if (($(this).parent().index()) > $("#endPosHidden").val()) {
                    $(this).parent().hide(0);
                } else if (($(this).parent().index()) < $("#startPosHidden").val()) {
                    $(this).parent().hide(0);
                } else {
                    $(this).parent().show(0);
                }
                // ボタンの色変化
                if ($(this).text() == $("#selectPosHidden").val()) {
                    $(this).addClass("On");
                } else {
                    if ($(this).hasClass("On")) {
                        $(this).removeClass("On");
                    }
                }

                // hiddenから、詳細情報を取得して設定
                if ($(this).text() == $("#selectPosHidden").val()) {

                    //希望車種情報
                    $("#selSeriescdHidden").val($(this).parent().children(":nth-child(2)").val());
                    $("#selModelcdHidden").val($(this).parent().children(":nth-child(4)").val());
                    $("#selColorcdHidden").val($(this).parent().children(":nth-child(6)").val());
                    $("#selSeqnoHidden").val($(this).parent().children(":nth-child(12)").val());

                    // イメージ
                    selectSelectedCarPictureAndLogo($(this).parent().children(":nth-child(10)").val(), $(this).parent().children(":nth-child(11)").val());

                    // モデル
                    $("#dispSelectedModel").text($(this).parent().children(":nth-child(5)").val());
                    // 色
                    $("#dispSelectedColor").text($(this).parent().children(":nth-child(7)").val());
                    // 見積金額
                    $("#dispSelectedMoney").text($(this).parent().children(":nth-child(8)").val());
                    // 台数
                    $("#dispSelectedQuantity").text($(this).parent().children(":nth-child(9)").val());
                }
            });
            // プロセスを再表示
            processDisplay();
        }
    );

    // [<]ボタン押下時　10台前を表示
    $("#scNscSelectCarButtonArrowFor").live("click",
        function () {
            $("#selectPosHidden").val(parseInt($("#startPosHidden").val()) - 10);
            $("#startPosHidden").val(parseInt($("#startPosHidden").val()) - 10);
            $("#endPosHidden").val(parseInt($("#endPosHidden").val()) - 10);
            var id = parseInt($("#selectPosHidden").val()) + 1;
            $("#selSeqnoHidden").val($(".scNscSelectCarButtonList").children(":nth-child(" + id + ")").children(":nth-child(12)").val());

            // [>]アイコン表示有無
            if ($("#scNscSelectCarArea ul li.scNscSelectCarButton").parent().size() > $("#endPosHidden").val()) {
                $("#scNscSelectCarButtonArrow").show(0);
            } else {
            }

            // [<]アイコン表示有無
            if ($("#startPosHidden").val() == 1) {
                $("#scNscSelectCarButtonArrowFor").hide(0);
            }

            // ボタンの情報を変更
            $("#scNscSelectCarArea ul li.scNscSelectCarButton").each(function () {
                // ボタンのINDEX表示
                $(this).text($(this).parent().index());
                // 10台のみ表示
                if (($(this).parent().index()) > $("#endPosHidden").val()) {
                    $(this).parent().hide(0);
                } else if (($(this).parent().index()) < $("#startPosHidden").val()) {
                    $(this).parent().hide(0);
                } else {
                    $(this).parent().show(0);
                }
                // ボタンの色変化
                if ($(this).text() == $("#selectPosHidden").val()) {
                    $(this).addClass("On");
                } else {
                    if ($(this).hasClass("On")) {
                        $(this).removeClass("On");
                    }
                }

                // hiddenから、詳細情報を取得して設定
                if ($(this).text() == $("#selectPosHidden").val()) {
                    //希望車種情報
                    $("#selSeriescdHidden").val($(this).parent().children(":nth-child(2)").val());
                    $("#selModelcdHidden").val($(this).parent().children(":nth-child(4)").val());
                    $("#selColorcdHidden").val($(this).parent().children(":nth-child(6)").val());
                    $("#selSeqnoHidden").val($(this).parent().children(":nth-child(12)").val());

                    // イメージ
                    selectSelectedCarPictureAndLogo($(this).parent().children(":nth-child(10)").val(), $(this).parent().children(":nth-child(11)").val());
                    // モデル
                    $("#dispSelectedModel").text($(this).parent().children(":nth-child(5)").val());
                    // 色
                    $("#dispSelectedColor").text($(this).parent().children(":nth-child(7)").val());
                    // 見積金額
                    $("#dispSelectedMoney").text($(this).parent().children(":nth-child(8)").val());
                    // 台数
                    $("#dispSelectedQuantity").text($(this).parent().children(":nth-child(9)").val());
                }
            });
            // プロセスを再表示
            processDisplay();
        }
    );

    //希望車種未選択のエラーメッセージ
    SC3080201.addPageMoveEventHandler(function (pageClass) {
        if ($("#PageMoveFlgHidden").val() === "False" && pageClass === "page3") {
            icropScript.ShowMessageBox(0, $("#PageMoveErrorMessage").val(), "");
            return false;
        }
    });

});


// 選択車種更新後
function commitCompleteSelectedSeriesButtonDummyAfter(editMode, delMode, selSeqNo) {
    // 希望車種を設定
    selectedSeriesDisplay(editMode,delMode,selSeqNo);
    // プロセスを設定
    processDisplay();

}

// 選択車種表示
function selectedSeriesDisplay(editMode, delMode, selSeqNo) {
    // 希望車種有無
    if ($("#scNscSelectCarArea ul li.scNscSelectCarButton").size() == 0) {
        $("#scNscSelectCarArea").hide(0);
        $("#scNsc51MainSample").show(0);
    } else {
        $("#scNsc51MainSample").hide(0);
        $("#scNscSelectCarArea").show(0);
    }

    // 選択設定
    // 初期表示
    if (editMode == "") {
        if ($("#scNscSelectCarArea ul li.scNscSelectCarButton").parent().size() < 1) {
            $("#selectPosHidden").val(0);
            $("#startPosHidden").val(0);
            $("#endPosHidden").val(0);
        } else {
            // １番目を選択状態
            $("#selectPosHidden").val(1);
            $("#startPosHidden").val(1);
            if ($("#scNscSelectCarArea ul li.scNscSelectCarButton").parent().size() < 10) {
                $("#endPosHidden").val($("#scNscSelectCarArea ul li.scNscSelectCarButton").parent().size());
            } else {
                $("#endPosHidden").val(10);
            }
        }
    }
    // 削除
    if (editMode == "1" && delMode == "1") {
        if ($("#scNscSelectCarArea ul li.scNscSelectCarButton").parent().size() < 1) {
            $("#selectPosHidden").val(0);
            $("#startPosHidden").val(0);
            $("#endPosHidden").val(0);
        } else {
            // １番目を選択状態
            $("#selectPosHidden").val(1);
            $("#startPosHidden").val(1);
            if ($("#scNscSelectCarArea ul li.scNscSelectCarButton").parent().size() < 10) {
                $("#endPosHidden").val($("#scNscSelectCarArea ul li.scNscSelectCarButton").parent().size());
            } else {
                $("#endPosHidden").val(10);
            }
        }
    }
    // 編集
    if (editMode == "1" && delMode == "0") {
        // 変更なし
    }
    // 追加
    if (editMode == "0") {
        // 追加した希望車種を選択状態
        $("#selectPosHidden").val($("#scNscSelectCarArea ul li.scNscSelectCarButton").parent().size());
        var startposCount = $("#selectPosHidden").val();
        var endposCount;
        startposCount = Math.floor((startposCount - 1) / 10);
        startposCount = ((startposCount + 1) * 10);
        endposCount = startposCount - 9;
        $("#startPosHidden").val(endposCount);
        if ($("#scNscSelectCarArea ul li.scNscSelectCarButton").parent().size() < 10) {
            $("#endPosHidden").val($("#scNscSelectCarArea ul li.scNscSelectCarButton").parent().size());
        } else {
            $("#endPosHidden").val(startposCount);
        }
    }
    // [>]アイコン表示有無
    if ($("#scNscSelectCarArea ul li.scNscSelectCarButton").parent().size() > $("#endPosHidden").val()) {
        $("#scNscSelectCarButtonArrow").show(0);
    } else {
        $("#scNscSelectCarButtonArrow").hide(0);
    }
    // [<]アイコン表示有無
    if ($("#startPosHidden").val() != 1) {
        $("#scNscSelectCarButtonArrowFor").show(0);
    } else {
        $("#scNscSelectCarButtonArrowFor").hide(0);
    }

    if ($("#scNscSelectCarArea ul li.scNscSelectCarButton").size() == 0) {
        $("#selSeriescdHidden").val("");
        $("#selModelcdHidden").val("");
        $("#selColorcdHidden").val("");
        $("#selSeqnoHidden").val("");
    }

    $("#scNscSelectCarArea ul li.scNscSelectCarButton").each(function () {
        // ボタンのINDEX表示
        $(this).text($(this).parent().index())
        // 10台のみ表示
        if (($(this).parent().index()) > $("#endPosHidden").val()) {
            $(this).parent().hide(0);
        }
        if (($(this).parent().index()) < $("#startPosHidden").val()) {
            $(this).parent().hide(0);
        }
        // ボタンの色変化
        if ($(this).hasClass("On")) {
            $(this).removeClass("On");
        }
        if ($(this).text() == $("#selectPosHidden").val()) {
            $(this).addClass("On");
        }

        // [<]アイコン分のPADDING
        if ($(this).parent().index() == 1) {
            $(this).addClass("NotArrow");
        }

        if ($(this).text() == $("#selectPosHidden").val()) {
            //希望車種情報
            $("#selSeriescdHidden").val($(this).parent().children(":nth-child(2)").val());
            $("#selModelcdHidden").val($(this).parent().children(":nth-child(4)").val());
            $("#selColorcdHidden").val($(this).parent().children(":nth-child(6)").val());
            $("#selSeqnoHidden").val($(this).parent().children(":nth-child(12)").val());
            // イメージ
            selectSelectedCarPictureAndLogo($(this).parent().children(":nth-child(10)").val(), $(this).parent().children(":nth-child(11)").val());
            // モデル
            $("#dispSelectedModel").text($(this).parent().children(":nth-child(5)").val());
            // 色
            $("#dispSelectedColor").text($(this).parent().children(":nth-child(7)").val());
            // 見積金額
            $("#dispSelectedMoney").text($(this).parent().children(":nth-child(8)").val());
            // 台数
            $("#dispSelectedQuantity").text($(this).parent().children(":nth-child(9)").val());
        }
    });
}


/**
* 希望車種の画像とロゴのSrc属性設定
*/
function selectSelectedCarPictureAndLogo(pictureSrc, logoSrc) {

    if (pictureSrc === undefined || pictureSrc === "") {
        $("#dispSelectedPicture").attr("src", "dummy.jpg");
    } else {
        $("#dispSelectedPicture").attr("src", pictureSrc);
    }

    if (logoSrc === undefined || logoSrc === "") {
        $("#dispSelectedLogo").attr("src", "dummy.jpg");
    } else {
        $("#dispSelectedLogo").attr("src", logoSrc);
    }
    
}

/***********************************************************
希望車種ポップアップイベント
***********************************************************/
//ポップアップ表示
function openSeriesPopup() {

    //
    if ($("#SeriesSelectPopup").hasClass("ptn1") === true) {
        var left = Math.max($("#plus").offset().left - 170, 0);
        $("#SeriesSelectPopup").css("left", left + "px");
    } else {
        $("#SeriesSelectPopup").css("left", "");
    }

    //フェードイン
    $("#SeriesSelectPopup").show(0, function () {
        //表示
        $("#SeriesSelectPopup").addClass("opened");
    });
};

//ポップアップクローズ
function closeSeriesPopup() {
    if ($("#SeriesSelectPopup").hasClass("opened") === true) {
        //クローズ
        $("#SeriesSelectPopup").removeClass("opened").one("webkitTransitionEnd", function (e) {
            $("#SeriesSelectPopup").hide(0);
        });
    }
};

$(function () {

    //希望車種ポップアップの初期化
    $(".scNsc51PopUpScrollWrap").fingerScroll();

    //新規登録
    $("#plus, #scNscCarSelectArea2").live("click", function (e) {

        if ($("#PageEnabledFlgHidden").val() == "False") return;
        //選択項目クリア
        $("#scNsc51PopUpHiddenWrap :hidden").val("");
        //パターン設定 
        setPopupPtn($(this).attr("id") == "plus" ? "ptn1" : "ptn2");
        //ボタンエリア
        $("#SeriesSelectPopup").removeClass("updateMode");
        //削除ボタン非表示
        $(".scNsc51PopUpListDeleteButton").css("display", "none");
        //新規モード設定
        $("#SelectSeriesEidtMode").val("0");
        $("#SelectSeriesDelMode").val("0");

        //新規の為、選択値をクリア
        $("#SelectSeriescdHidden").val("");
        $("#SelectModelcdHidden").val("");
        $("#SelectColorcdHidden").val("");
        $("#SelectSeqnoHidden").val("");

        setPopupPage("page1");
        openSeriesPopup();
    });

    //画像表示エリアタップ
    $("#scNscCarSelectArea1").live("click", function (e) {

        if ($("#PageEnabledFlgHidden").val() == "False") return;
        if ($(e.target).is(".scNscCarStatusArea li span.scNscCarIconCar,.scNscCarStatusArea li span.scNscCarIconCar *") === true) {
            return;
        }

        //パターン設定
        setPopupPtn("ptn2");

        //編集モード設定
        $("#SelectSeriesEidtMode").val("1");
        $("#SelectSeriesDelMode").val("0");

        //更新のため、現在の値をキー値としてセット
        $("#SelectSeriescdHidden").val($("#selSeriescdHidden").val());
        $("#SelectModelcdHidden").val($("#selModelcdHidden").val());
        $("#SelectColorcdHidden").val($("#selColorcdHidden").val());
        $("#SelectSeqnoHidden").val($("#selSeqnoHidden").val());

        //更新モードを設定
        $("#SeriesSelectPopup").addClass("updateMode");

        //２ページ目から開始
        setPopupPage("page2");

        //削除ボタン表示
        $(".scNsc51PopUpListDeleteButton").show(0);

        //グレードのフィルタリング
        modelMasterDisplay($("#SelectSeriescdHidden").val());

        //ボタンエリア
        openSeriesPopup();
    });

    /**
    * ポップアップ位置パターン設定
    */
    function setPopupPtn(ptnClass) {
        $("#SeriesSelectPopup").removeClass("ptn1 ptn2").addClass(ptnClass);
    };

    /**
    * ページ切り替え関数
    */
    function setPopupPage(pageClass) {

        //モード毎のラベル・ボタンを一旦全部非表示にする
        $("#SeriesSelectCancelLabel, #SeriesSelectBackSeriesLabel, #SeriesSelectBackModelLabel, #SeriesSelectPage1Title, #SeriesSelectPage2Title, #SeriesSelectPage3Title, .scNscPopUpCompleteButton").css("display", "none");

        //ページ１
        if (pageClass === "page1") {
            //ボタンタイトル
            $("#SeriesSelectCancelLabel").show(0);
            $("#SeriesSelectPage1Title").show(0);
            setSeriesCheckMark();
        }

        //ページ２
        if (pageClass === "page2") {
            //ボタンタイトル
            if ($("#SeriesSelectPopup").hasClass("updateMode") === true) {
                //更新
                $("#SeriesSelectCancelLabel").show(0);
            } else {
                //新規追加
                $("#SeriesSelectBackSeriesLabel").show(0);
            }
            setModelCheckMark();
            $("#SeriesSelectPage2Title").show(0);
            $(".scNscPopUpCompleteButton").show(0);
        }

        //ページ３
        if (pageClass === "page3") {
            //ボタンタイトル
            $("#SeriesSelectBackModelLabel").show(0);
            $("#SeriesSelectPage3Title").show(0);
            $(".scNscPopUpCompleteButton").show(0);
            setColorCheckMark();
        }

        $("#scNsc51PopUpListWrap").removeClass("page1 page2 page3").addClass(pageClass);
        //スクロール初期化
        $(".scNsc51PopUpScrollWrap").fingerScroll();
    };

    //車種を選択した時のイベント
    $(".scNsc51PopUpList01 li.scNsc51ListLi1").live("click", function (e) {

        //キー取得
        var id = $(this).attr("itemid");
        //グレード制御
        modelMasterDisplay(id);
        gradeMasterStyle(id);
        //保存
        $("#SelectSeriescdHidden").val(id);

        //ページ１→ページ２
        setPopupPage("page2");
    });

    //グレードを選択した時のイベント
    $(".scNsc51PopUpList02 li.scNsc51ListLi2").live("click", function (e) {

        //キー取得
        var id = $(this).attr("itemid");
        var id2 = $(this).attr("itemid2");

        //カラー制御
        colorMasterDisplay(id, id2);
        colorMasterStyle(id, id2);

        if (id2 !== $("#SelectModelcdHidden").val()) {
            $("#SelectColorcdHidden").val("");
        }

        //保存
        $("#SelectModelcdHidden").val(id2);

        //ページ２→ページ３
        setPopupPage("page3");
    });

    //カラーを選択した時のイベント
    $(".scNsc51PopUpList03 li.scNsc51ListLi3").live("click", function (e) {

        //キー取得
        var id = $(this).attr("itemid");
        //保存
        $("#SelectColorcdHidden").val(id);

        //ポップアップ終了
        selectedCloseSetting();
    });

    //キャンセルまたは戻るボタンのイベント
    $(".scNscPopUpCancelButton").bind("click", function (e) {
        //ページ１を表示している場合
        //if ($("#scNsc51PopUpListWrap").hasClass("page1") === true) $("#SeriesSelectPopup").fadeOut(300);
        if ($("#scNsc51PopUpListWrap").hasClass("page1") === true) closeSeriesPopup();
        //ページ２を表示している場合
        if ($("#scNsc51PopUpListWrap").hasClass("page2") === true) {
            if ($("#SeriesSelectPopup").hasClass("updateMode") === true) {
                //更新モード
                closeSeriesPopup();
            } else {
                //新規モード
                setPopupPage("page1");
            }
        }
        //ページ３を表示している場合
        if ($("#scNsc51PopUpListWrap").hasClass("page3") === true) setPopupPage("page2");
    });

    //削除ボタンクリック
    $(".scNsc51PopUpListDeleteButton").bind("click", function (e) {
        //削除処理
        $("#SelectSeriesDelMode").val("1");
        selectedCloseSetting();
    });

    //完了ボタンクリック
    $(".scNscPopUpCompleteButton").bind("click", function (e) {
        selectedCloseSetting();
    });

    //シーリーズのチェックマーク
    function setSeriesCheckMark() {
        //一旦全部のチェック状態削除
        $(".scNsc51PopUpList01 li.scNsc51ListLi1").removeClass("On");
        if ($("#SelectSeriescdHidden").val() != "") $(".scNsc51PopUpList01 li.scNsc51ListLi1[itemid='" + $("#SelectSeriescdHidden").val() + "']").addClass("On");
    };

    //グレードのチェックマーク
    function setModelCheckMark() {
        //一旦全部のチェック状態削除
        $(".scNsc51PopUpList02 li.scNsc51ListLi2").removeClass("On");
        if ($("#SelectModelcdHidden").val() != "") $(".scNsc51PopUpList02 li.scNsc51ListLi2"
                                                    + "[itemid='" + $("#SelectSeriescdHidden").val() + "']"
                                                    + "[itemid2='" + $("#SelectModelcdHidden").val() + "']")
                                                    .addClass("On");
    };

    //カラーのチェックマーク
    function setColorCheckMark() {
        //一旦全部のチェック状態削除
        $(".scNsc51PopUpList03 li.scNsc51ListLi3").removeClass("On");
        if ($("#SelectColorcdHidden").val() != "") $(".scNsc51PopUpList03 li.scNsc51ListLi3"
                                                    + "[itemid='" + $("#SelectColorcdHidden").val() + "']"
                                                    + "[seriescd='" + $("#SelectSeriescdHidden").val() + "']"
                                                    + "[modelcd='" + $("#SelectModelcdHidden").val() + "']")
                                                    .addClass("On");
    };

    //ポップアップクローズの監視
    $(document.body).bind("mousedown touchstart", function (e) {
        if ($("#SeriesSelectPopup").is(":visible") === false) return;
        if ($(e.target).is("#SeriesSelectPopup, #SeriesSelectPopup *, #plus, #plus *") === false) {
            closeSeriesPopup();
        }
    });
});

$(function () {
    if ($("#PageEnabledFlgHidden").val() == "False") return;
    //キーボード表示
    $(".scNscCarStatusArea li span.scNscCarIconCar").NumericKeypad({
        maxDigits: 2,
        acceptDecimalPoint: false,
        defaultValue: 1,
        completionLabel: $("#CompletionNumericMessage").val(),
        cancelLabel: $("#CancelNumericMessage").val(),
        valueChanged: function (num) {
            if (isNaN(num) || isNaN(parseInt(num)) || parseInt(num) <= 0) {
                if (num == "") icropScript.ShowMessageBox(0, $("#QuantityErrorMessageReqiored").val(), "");
                if (num != "") icropScript.ShowMessageBox(0, $("#QuantityErrorMessageNumric").val(), "");
            } else {

                //選択項目のHIDDENに格納
                $("#dispSelectedQuantity").text(num);
                $("#inputSelectQuantiryHidden").val(num);
                //リストHIDDENに格納
                $(".scNscSelectCarButtonList li.scNscSelectCarButton[seqno='" + $("#selSeqnoHidden").val() + "']").parent().children(":nth-child(9)").val(num);
                $("#commitCompleteSeriesQuantiryButtonDummy").get(0).click();
            }
        },
        open: function () {
            var quantiry = 0;
            if ($.trim($("#dispSelectedQuantity").text()).length > 0) quantiry = parseInt($("#dispSelectedQuantity").text());
            $(".scNscCarStatusArea li span.scNscCarIconCar").NumericKeypad("setValue", quantiry);
        }
    });

});


/**
* ポップアップを閉じるときの処理
*/
function selectedCloseSetting() {
    //$("#SeriesSelectPopup").fadeOut(300);
    closeSeriesPopup();
    $("#commitCompleteSelectedSeriesButtonDummy").click();
}

/**
* グレード表示・非表示
* @param {String} id キー
*/
function modelMasterDisplay(id) {

    $(".scNsc51PopUpList02 li.scNsc51ListLi2").each(function () {
        $(this).css({ "display": "none" });
        //選択された車種に緋付くグレードを表示
        if ($(this).attr("itemid") == id) {
            $(this).css({ "display": "block" });
        }
    });
}

/** グレード先頭行、最終行判定 **/
function gradeMasterStyle(id) {
    var index = 0
    var count = $(".scNsc51PopUpList02 li.scNsc51ListLi2").parent().children("[itemid='" + id + "']").size()
    //表示対象のスタイル設定
    $(".scNsc51PopUpList02 li.scNsc51ListLi2").each(function () {
        if ($(this).attr("itemid") == id) {
            //先頭行のスタイル
            if (index == 0) {
                $(this).addClass("top");
            }
            //最終行のスタイル
            if (index == count - 1) {
                $(this).addClass("bottom");
            }
            index++;
        }
    });
}

/**
* カラー表示・非表示
* @param {String} id キー
* @param {String} id2 キー2
*/
function colorMasterDisplay(id, id2) {

    //一旦全部非表示してから対象のみ表示
    $(".scNsc51PopUpList03 li.scNsc51ListLi3").each(function () {
        $(this).css({ "display": "none" });
        //選択された車種に緋付くグレードを表示
        if ($(this).attr("seriescd") == id && $(this).attr("modelcd") == id2) {
            $(this).css({ "display": "block" });
        }
    });
}

/** カラー先頭行、最終行判定 **/
function colorMasterStyle(id,id2) {
    var index = 0
    var count = $(".scNsc51PopUpList03 li.scNsc51ListLi3").parent().children("[seriescd='" + id + "'][modelcd='" + id2 + "']").size()
    //表示対象のスタイル設定
    $(".scNsc51PopUpList03 li.scNsc51ListLi3").each(function () {
        if ($(this).attr("seriescd") == id && $(this).attr("modelcd") == id2) {
            //先頭行のスタイル
            if (index == 0) {
                $(this).addClass("top");
            }
            //最終行のスタイル
            if (index == count - 1) {
                $(this).addClass("bottom");
            }
            index++;
        }
    });
}