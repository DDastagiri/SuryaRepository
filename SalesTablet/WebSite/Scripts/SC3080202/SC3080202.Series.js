//━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
//SC3080202.Series.js
//─────────────────────────────────────
//機能： 顧客詳細(商談情報)
//補足： 
//作成： 2011/11/24 TCS 小野
//更新： 2012/01/26 TCS 山口 【SALES_1B】
//更新： 2012/04/26 TCS 河原 HTMLエンコード対応
//更新： 2013/12/09 TCS 市川 Aカード情報相互連携開発
//作成： 2014/04/21 TCS 市川 GTMCタブレット高速化対応（BTS-386）
//更新： 2017/11/20 TCS 河原 TKM独自機能開発
//更新： 2018/04/18 TCS 前田 （トライ店システム評価）基幹連携を用いたセールス業務実績入力の検証
//更新： 2018/07/10 TCS 河原 TKM Next Gen e-CRB Project Application development Block B-1
//─────────────────────────────────────

/// <reference path="../jquery.js"/>
/// <reference path="../jquery.fingerscroll.js"/>
/// <reference path="../jquery.NumericKeypad.js"/>
/// <reference path="../SC3080201/Common.js"/>
//2017/11/20 TCS 河原 TKM独自機能開発 START
/***********************************************************
希望車種イベント
***********************************************************/
$(function () {


    var mng = Sys.WebForms.PageRequestManager.getInstance();
    mng.add_initializeRequest(function (sender, args) {
        if (args.get_postBackElement().id == "MostPreferredUpdateDummyButton") {
            //一押し希望車サーバ処理中は希望車エリアのみ操作をくるくるでブロックする。
            $("#scNscSelectCarAreaCover").css("visibility", "visible");
            var handler = function (sender, args) {
                $("#scNscSelectCarAreaCover").css("visibility", "hidden");
                mng.remove_endRequest(handler);
            };
            mng.add_endRequest(handler);
        }
    });


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
        $("#selModelcdHidden").val($(this).parent().children(":nth-child(2)").val());
        $("#selGradecdHidden").val($(this).parent().children(":nth-child(4)").val());
        $("#selSuffixcdHidden").val($(this).parent().children(":nth-child(6)").val());
        $("#selExteriorColorcdHidden").val($(this).parent().children(":nth-child(8)").val());
        $("#selInteriorColorcdHidden").val($(this).parent().children(":nth-child(10)").val());
        $("#selSeqnoHidden").val($(this).parent().children(":nth-child(16)").val());
        $("#selLockvrHidden").val($(this).parent().children(":nth-child(17)").val());
        $("#selMostPreferredHidden").val($(this).parent().children(":nth-child(18)").val());


        // 詳細情報表示

        // イメージ
        selectSelectedCarPictureAndLogo($(this).parent().children(":nth-child(14)").val(), $(this).parent().children(":nth-child(15)").val());

        //モデル
        $("#dispSelectedModel").text(HtmlDecode($(this).parent().children(":nth-child(5)").val()));

        //サフィックス
        //2018/04/18 TCS 前田 （トライ店システム評価）基幹連携を用いたセールス業務実績入力の検証 START
        if (isSuffixAvailable()) {
            $("#dispSelectedSuffix").text(HtmlDecode($(this).parent().children(":nth-child(7)").val()));
        } else {
            $("#dispSelectedSuffix").text("");
        }

        //色
        if (isInteriorColorAvailable()) {
            $("#dispSelectedColor").text(HtmlDecode($(this).parent().children(":nth-child(9)").val()) + " / " + HtmlDecode($(this).parent().children(":nth-child(11)").val()));
        } else {
            $("#dispSelectedColor").text(HtmlDecode($(this).parent().children(":nth-child(9)").val()));
        }
        //2018/04/18 TCS 前田 （トライ店システム評価）基幹連携を用いたセールス業務実績入力の検証 END

        //見積金額
        $("#dispSelectedMoney").text($(this).parent().children(":nth-child(12)").val());

        //台数
        $("#dispSelectedQuantity").text($(this).parent().children(":nth-child(13)").val());


        if ($(this).parent().children(":nth-child(18)").val() == "1") {
            $("#dispSelectedMostPreferred").removeClass("NotMost").addClass("Most").unbind("click", changeMostPreferred);
        } else {
            $("#dispSelectedMostPreferred").removeClass("Most").addClass("NotMost").unbind("click", changeMostPreferred).bind("click", changeMostPreferred);
        }


        processDisplay();
    });

    // [>]ボタン押下時　10台後を表示
    $("#scNscSelectCarButtonArrow").live("click",
        function () {
            $("#startPosHidden").val(parseInt($("#startPosHidden").val()) + 10);
            $("#endPosHidden").val(parseInt($("#endPosHidden").val()) + 10);
            var id = parseInt($("#selectPosHidden").val()) + 1;
            $("#selSeqnoHidden").val($(".scNscSelectCarButtonList").children(":nth-child(" + id + ")").children(":nth-child(12)").val());

            // [>]アイコン表示有無
            if ($("#scNscSelectCarArea ul li.scNscSelectCarButton").parent().size() > $("#endPosHidden").val()) {
            } else {
                $("#scNscSelectCarButtonArrow").hide();
            }

            // [<]アイコン表示有無
            if ($("#startPosHidden").val() != 1) {
                $("#scNscSelectCarButtonArrowFor").show();
            }

            // ボタンの情報を変更
            $("#scNscSelectCarArea ul li.scNscSelectCarButton").each(function () {
                // ボタンのINDEX表示
                $(this).text($(this).parent().index());
                // 10台のみ表示
                if (($(this).parent().index()) > $("#endPosHidden").val()) {
                    $(this).parent().hide();
                } else if (($(this).parent().index()) < $("#startPosHidden").val()) {
                    $(this).parent().hide();
                } else {
                    $(this).parent().show();
                }
                // ボタンの色変化
                if ($(this).text() == $("#selectPosHidden").val()) {
                    $(this).addClass("On");
                } else {
                    if ($(this).hasClass("On")) {
                        $(this).removeClass("On");
                    }
                }

                //一押し希望車アイコン
                if ($(this).parent().children(":nth-child(18)").val() == "1") $(this).removeClass("NotMost").addClass("Most");


                // hiddenから、詳細情報を取得して設定
                if ($(this).text() == $("#selectPosHidden").val()) {

                    //希望車種情報
                    $("#selModelcdHidden").val($(this).parent().children(":nth-child(2)").val());
                    $("#selGradecdHidden").val($(this).parent().children(":nth-child(4)").val());
                    $("#selSuffixcdHidden").val($(this).parent().children(":nth-child(6)").val());
                    $("#selExteriorColorcdHidden").val($(this).parent().children(":nth-child(8)").val());
                    $("#selInteriorColorcdHidden").val($(this).parent().children(":nth-child(10)").val());
                    $("#selSeqnoHidden").val($(this).parent().children(":nth-child(16)").val());
                    $("#selLockvrHidden").val($(this).parent().children(":nth-child(17)").val());
                    $("#selMostPreferredHidden").val($(this).parent().children(":nth-child(18)").val());

                    // イメージ
                    selectSelectedCarPictureAndLogo($(this).parent().children(":nth-child(14)").val(), $(this).parent().children(":nth-child(15)").val());

                    // モデル
                    $("#dispSelectedModel").text(HtmlDecode($(this).parent().children(":nth-child(5)").val()));

                    //サフィックス
                    //2018/04/18 TCS 前田 （トライ店システム評価）基幹連携を用いたセールス業務実績入力の検証 START
                    if (isSuffixAvailable()) {
                        $("#dispSelectedSuffix").text(HtmlDecode($(this).parent().children(":nth-child(7)").val()));
                    } else {
                        $("#dispSelectedSuffix").text("");
                    }

                    //色
                    if (isInteriorColorAvailable()) {
                        $("#dispSelectedColor").text(HtmlDecode($(this).parent().children(":nth-child(9)").val()) + " / " + HtmlDecode($(this).parent().children(":nth-child(11)").val()));
                    } else {
                        $("#dispSelectedColor").text(HtmlDecode($(this).parent().children(":nth-child(9)").val()));
                    }
                    //2018/04/18 TCS 前田 （トライ店システム評価）基幹連携を用いたセールス業務実績入力の検証 END

                    // 見積金額
                    $("#dispSelectedMoney").text($(this).parent().children(":nth-child(12)").val());

                    // 台数
                    $("#dispSelectedQuantity").text($(this).parent().children(":nth-child(13)").val());


                    if ($(this).parent().children(":nth-child(18)").val() == "1") {
                        $("#dispSelectedMostPreferred").removeClass("NotMost").addClass("Most").unbind("click", changeMostPreferred);
                    } else {
                        $("#dispSelectedMostPreferred").removeClass("Most").addClass("NotMost").unbind("click", changeMostPreferred).bind("click", changeMostPreferred);
                    }

                }
            });
            // プロセスを再表示
            processDisplay();
        }
    );

    // [<]ボタン押下時　10台前を表示
    $("#scNscSelectCarButtonArrowFor").live("click",
        function () {
            $("#startPosHidden").val(parseInt($("#startPosHidden").val()) - 10);
            $("#endPosHidden").val(parseInt($("#endPosHidden").val()) - 10);
            var id = parseInt($("#selectPosHidden").val()) + 1;
            $("#selSeqnoHidden").val($(".scNscSelectCarButtonList").children(":nth-child(" + id + ")").children(":nth-child(12)").val());

            // [>]アイコン表示有無
            if ($("#scNscSelectCarArea ul li.scNscSelectCarButton").parent().size() > $("#endPosHidden").val()) {
                $("#scNscSelectCarButtonArrow").show();
            } else {
            }

            // [<]アイコン表示有無
            if ($("#startPosHidden").val() == 1) {
                $("#scNscSelectCarButtonArrowFor").hide();
            }

            // ボタンの情報を変更
            $("#scNscSelectCarArea ul li.scNscSelectCarButton").each(function () {
                // ボタンのINDEX表示
                $(this).text($(this).parent().index());
                // 10台のみ表示
                if (($(this).parent().index()) > $("#endPosHidden").val()) {
                    $(this).parent().hide();
                } else if (($(this).parent().index()) < $("#startPosHidden").val()) {
                    $(this).parent().hide();
                } else {
                    $(this).parent().show();
                }
                // ボタンの色変化
                if ($(this).text() == $("#selectPosHidden").val()) {
                    $(this).addClass("On");
                } else {
                    if ($(this).hasClass("On")) {
                        $(this).removeClass("On");
                    }
                }

                //一押し希望車アイコン
                if ($(this).parent().children(":nth-child(18)").val() == "1") $(this).removeClass("NotMost").addClass("Most");

                // hiddenから、詳細情報を取得して設定
                if ($(this).text() == $("#selectPosHidden").val()) {
                    //希望車種情報
                    $("#selModelcdHidden").val($(this).parent().children(":nth-child(2)").val());
                    $("#selGradecdHidden").val($(this).parent().children(":nth-child(4)").val());
                    $("#selSuffixcdHidden").val($(this).parent().children(":nth-child(6)").val());
                    $("#selExteriorColorcdHidden").val($(this).parent().children(":nth-child(8)").val());
                    $("#selInteriorColorcdHidden").val($(this).parent().children(":nth-child(10)").val());
                    $("#selSeqnoHidden").val($(this).parent().children(":nth-child(16)").val());
                    $("#selLockvrHidden").val($(this).parent().children(":nth-child(17)").val());
                    $("#selMostPreferredHidden").val($(this).parent().children(":nth-child(18)").val());

                    // イメージ
                    selectSelectedCarPictureAndLogo($(this).parent().children(":nth-child(14)").val(), $(this).parent().children(":nth-child(15)").val());

                    // モデル
                    $("#dispSelectedModel").text(HtmlDecode($(this).parent().children(":nth-child(5)").val()));

                    //サフィックス
                    //2018/04/18 TCS 前田 （トライ店システム評価）基幹連携を用いたセールス業務実績入力の検証 START
                    if (isSuffixAvailable()) {
                        $("#dispSelectedSuffix").text(HtmlDecode($(this).parent().children(":nth-child(7)").val()));
                    } else {
                        $("#dispSelectedSuffix").text("");
                    }

                    //色
                    if (isInteriorColorAvailable()) {
                        $("#dispSelectedColor").text(HtmlDecode($(this).parent().children(":nth-child(9)").val()) + " / " + HtmlDecode($(this).parent().children(":nth-child(11)").val()));
                    } else {
                        $("#dispSelectedColor").text(HtmlDecode($(this).parent().children(":nth-child(9)").val()));
                    }
                    //2018/04/18 TCS 前田 （トライ店システム評価）基幹連携を用いたセールス業務実績入力の検証 END

                    // 見積金額
                    $("#dispSelectedMoney").text($(this).parent().children(":nth-child(12)").val());

                    // 台数
                    $("#dispSelectedQuantity").text($(this).parent().children(":nth-child(13)").val());

                    if ($(this).parent().children(":nth-child(18)").val() == "1") {
                        $("#dispSelectedMostPreferred").removeClass("NotMost").addClass("Most").unbind("click", changeMostPreferred);
                    } else {
                        $("#dispSelectedMostPreferred").removeClass("Most").addClass("NotMost").unbind("click", changeMostPreferred).bind("click", changeMostPreferred);
                    }

                }
            });
            // プロセスを再表示
            processDisplay();
        }
    );

    //2018/07/10 TCS 河原 TKM Next Gen e-CRB Project Application development Block B-1 START
    //希望車種未選択のエラーメッセージ
    SC3080201.addPageMoveEventHandler(function (pageClass) {
        if ((($("#PageMoveFlgHidden").val() === "False") || $("#CleansingResult").val() == "1" || checkDemandStructure() != "00000") && pageClass === "page3") {

            if ($("#Use_Customerdata_Cleansing_Flg").val() == "1") {
                //事前クレンジング結果がNGの場合、メッセージを表示しスライドを中断する
                if ($("#CleansingResult").val() == "1") {
                    alert($("#Cleansingerror").text());

                    //1枚目にスライド
                    SC3080201.executeSlidePage("page1");

                    //
                    $("#CleansingModeFlg").val("1");

                    //顧客編集画面を表示
                    CustomerEditPopUpOpen();

                    return false;
                }
            }

            //受注後の場合はチェック不要
            if ($("#selFllwupboxSalesAfterFlg").val() != "1") {
                //購入分類が未選択の場合、メッセージを表示しスライドを中断する
                var checkResult = checkDemandStructure();
                if (checkResult != "00000") {
                    //エラー有り
                    alert(errMsg(checkResult));
                    return false;
                }
            }

            if (trim($("#PageMoveErrorMessage").val()) != "") {
                icropScript.ShowMessageBox(0, $("#PageMoveErrorMessage").val(), "");
                return false;
            }
        }
    });
    //2018/07/10 TCS 河原 TKM Next Gen e-CRB Project Application development Block B-1 END

});


// 選択車種更新後
function commitCompleteSelectedSeriesButtonDummyAfter(editMode, delMode, selSeqNo) {
    // 希望車種を設定
    selectedSeriesDisplay(editMode, delMode, selSeqNo);
    // プロセスを設定
    processDisplay();

}

// 選択車種表示
function selectedSeriesDisplay(editMode, delMode, selSeqNo) {
    // 希望車種有無
    if ($("#scNscSelectCarArea ul li.scNscSelectCarButton").size() == 0) {
        $("#scNscSelectCarArea").hide();
        $("#scNsc51MainSample").show();
    } else {
        $("#scNsc51MainSample").hide();
        $("#scNscSelectCarArea").show();
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
        $("#scNscSelectCarButtonArrow").show();
    } else {
        $("#scNscSelectCarButtonArrow").hide();
    }
    // [<]アイコン表示有無
    if ($("#startPosHidden").val() != 1) {
        $("#scNscSelectCarButtonArrowFor").show();
    } else {
        $("#scNscSelectCarButtonArrowFor").hide();
    }

    // 受注後の成約車種欄ボタン活性制御
    if ($("#selFllwupboxSalesBkgno").val() == "") {
    } else {
        $("#scNscSelectCarArea ul li.scNscSelectCarButton").hide();
        $("#scNscSelectCarButtonArrowFor").hide();
        $("#scNscSelectCarButtonArrow").hide();
        $("#plus").hide();
    }

    if ($("#scNscSelectCarArea ul li.scNscSelectCarButton").size() == 0) {
        $("#selSeriescdHidden").val("");
        $("#selModelcdHidden").val("");
        $("#selColorcdHidden").val("");
        $("#selSeqnoHidden").val("");
        $("#selLockvrHidden").val(0);
        $("#selMostPreferredHidden").val("0");
    }

    $("#scNscSelectCarArea ul li.scNscSelectCarButton").each(function () {
        // ボタンのINDEX表示
        $(this).text($(this).parent().index())
        // 10台のみ表示
        if (($(this).parent().index()) > $("#endPosHidden").val()) {
            $(this).parent().hide();
        }
        if (($(this).parent().index()) < $("#startPosHidden").val()) {
            $(this).parent().hide();
        }
        // ボタンの色変化
        if ($(this).hasClass("On")) {
            $(this).removeClass("On");
        }
        if ($(this).text() == $("#selectPosHidden").val()) {
            $(this).addClass("On");
        }


        //一押し希望車アイコン
        if ($(this).parent().children(":nth-child(18)").val() == "1") $(this).removeClass("NotMost").addClass("Most");


        // [<]アイコン分のPADDING
        if ($(this).parent().index() == 1) {
            $(this).addClass("NotArrow");
        }

        if ($(this).text() == $("#selectPosHidden").val()) {
            //希望車種情報
            $("#selModelcdHidden").val($(this).parent().children(":nth-child(2)").val());
            $("#selGradecdHidden").val($(this).parent().children(":nth-child(4)").val());
            $("#selSuffixcdHidden").val($(this).parent().children(":nth-child(6)").val());
            $("#selExteriorColorcdHidden").val($(this).parent().children(":nth-child(8)").val());
            $("#selInteriorColorcdHidden").val($(this).parent().children(":nth-child(10)").val());
            $("#selSeqnoHidden").val($(this).parent().children(":nth-child(16)").val());
            $("#selLockvrHidden").val($(this).parent().children(":nth-child(17)").val());
            $("#selMostPreferredHidden").val($(this).parent().children(":nth-child(18)").val());

            // イメージ
            selectSelectedCarPictureAndLogo($(this).parent().children(":nth-child(14)").val(), $(this).parent().children(":nth-child(15)").val());

            // モデル
            $("#dispSelectedModel").text(HtmlDecode($(this).parent().children(":nth-child(5)").val()));

            //サフィックス
            //2018/04/18 TCS 前田 （トライ店システム評価）基幹連携を用いたセールス業務実績入力の検証 START
            if (isSuffixAvailable()) {
                $("#dispSelectedSuffix").text(HtmlDecode($(this).parent().children(":nth-child(7)").val()));
            } else {
                $("#dispSelectedSuffix").text("");
            }

            //色
            if (isInteriorColorAvailable()) {
                $("#dispSelectedColor").text(HtmlDecode($(this).parent().children(":nth-child(9)").val()) + " / " + HtmlDecode($(this).parent().children(":nth-child(11)").val()));
            } else {
                $("#dispSelectedColor").text(HtmlDecode($(this).parent().children(":nth-child(9)").val()));
            }
            //2018/04/18 TCS 前田 （トライ店システム評価）基幹連携を用いたセールス業務実績入力の検証 END

            // 見積金額
            $("#dispSelectedMoney").text($(this).parent().children(":nth-child(12)").val());

            // 台数
            $("#dispSelectedQuantity").text($(this).parent().children(":nth-child(13)").val());

            if ($(this).parent().children(":nth-child(18)").val() == "1") {
                $("#dispSelectedMostPreferred").removeClass("NotMost").addClass("Most").unbind("click", changeMostPreferred);
            } else {
                $("#dispSelectedMostPreferred").removeClass("Most").addClass("NotMost").unbind("click", changeMostPreferred).bind("click", changeMostPreferred);
            }

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

    //スクリプトの遅延読み込み
    SC3080201.requirePartialScript("../Scripts/SC3080202/SC3080202.SeriesPopup.js", function () {

        //HTML削除
        $(".scNsc51PopUpScrollWrap").empty();

        //取り消し、完成非表示
        $(".scNscPopUpCancelButton").css("display", "none");
        $(".scNscPopUpCompleteButton").css("display", "none");

        //ウィンドウ
        if ($("#SeriesSelectPopup").hasClass("ptn1") === true) {
            //パターン設定
            setPopupPtn("ptn1");
        } else {
            setPopupPtn("ptn2");
        }

        //削除ボタン非表示
        $(".scNsc51PopUpListDeleteButton").css("display", "none");

        //フェードイン
        $("#SeriesSelectPopup").show(0, function () {
            //表示
            $("#SeriesSelectPopup").addClass("opened");
        });
    });
};

//ポップアップクローズ
function closeSeriesPopup() {
    if ($("#SeriesSelectPopup").hasClass("opened") === true) {
        //クローズ
        $("#SeriesSelectPopup").removeClass("opened").one("webkitTransitionEnd", function (e) {
            $("#SeriesSelectPopup").hide();
        });
    }
};

/**
* 希望車種ポップアップ処理(新規)
*/
function showSeriesSelect() {
    if ($("#PageEnabledFlgHidden").val() == "False") return;

    //スクリプトの遅延読み込み
    SC3080201.requirePartialScript("../Scripts/SC3080202/SC3080202.SeriesPopup.js", function () {

        //選択項目クリア
        $("#scNsc51PopUpHiddenWrap :hidden").val("");
        //パターン設定 
        setPopupPtn($(this).attr("id") == "plus" ? "ptn1" : "ptn2");
        //ボタンエリア
        $("#SeriesSelectPopup").removeClass("updateMode");
        //新規モード設定
        $("#SelectSeriesEidtMode").val("0");
        $("#SelectSeriesDelMode").val("0");

        //新規の為、選択値をクリア
        $("#SelectModelcdHidden").val("");
        $("#SelectGradecdHidden").val("");
        $("#SelectSuffixcdHidden").val("");
        $("#SelectExteriorColorcdHidden").val("");
        $("#SelectInteriorColorcdHidden").val("");
        $("#SelectSeqnoHidden").val("");
        $("#SelectLockvrHidden").val(0);
        $("#selMostPreferredHidden").val("0");

        //1ページ目から開始
        setPopupPage("page1");

        //削除ボタン表示
        $(".scNsc51PopUpListDeleteButton").hide();

        //共通読込みアニメーション戻し
        $("#processingServer").removeClass("seriesSelectPopupLoadingAnimation");
        $("#registOverlayBlack").removeClass("popupLoadingBackgroundColor");
    });
}

/**
* 希望車種ポップアップ処理(更新)
*/
function showSeriesSelectUpdate() {
    //スクリプトの遅延読み込み
    SC3080201.requirePartialScript("../Scripts/SC3080202/SC3080202.SeriesPopup.js", function () {

        //編集モード設定
        $("#SelectSeriesEidtMode").val("1");
        $("#SelectSeriesDelMode").val("0");

        //更新のため、現在の値をキー値としてセット
        $("#SelectModelcdHidden").val($("#selModelcdHidden").val());
        $("#SelectGradecdHidden").val($("#selGradecdHidden").val());
        $("#SelectSuffixcdHidden").val($("#selSuffixcdHidden").val());
        $("#SelectExteriorColorcdHidden").val($("#selExteriorColorcdHidden").val());
        $("#SelectInteriorColorcdHidden").val($("#selInteriorColorcdHidden").val());
        $("#SelectSeqnoHidden").val($("#selSeqnoHidden").val());
        $("#SelectLockvrHidden").val($("#selLockvrHidden").val());
        $("#SelectMostPreferredHidden").val($("#selMostPreferredHidden").val());

        //更新モードを設定
        $("#SeriesSelectPopup").addClass("updateMode");

        //取り消し、完成表示
        $(".scNscPopUpCancelButton").css("display", "block");
        $(".scNscPopUpCompleteButton").css("display", "block");

        //２ページ目から開始
        setPopupPage("page2");

        //削除ボタン表示
        $(".scNsc51PopUpListDeleteButton").show();

        //グレードのフィルタリング
        gradeMasterDisplay($("#SelectModelcdHidden").val());
        gradeMasterStyle($("#SelectModelcdHidden").val());

        //共通読込みアニメーション戻し
        $("#processingServer").removeClass("seriesSelectPopupLoadingAnimation");
        $("#registOverlayBlack").removeClass("popupLoadingBackgroundColor");
    });
}

$(function () {

    //希望車種ポップアップの初期化
    $(".scNsc51PopUpScrollWrap").fingerScroll();

    //新規登録
    $("#plus, #scNscCarSelectArea2").live("click", function (e) {
        if ($("#PageEnabledFlgHidden").val() == "False") return;

        //スクリプトの遅延読み込み
        SC3080201.requirePartialScript("../Scripts/SC3080202/SC3080202.SeriesPopup.js", function () {

            //共通読込みアニメーション変更
            $("#processingServer").addClass("seriesSelectPopupLoadingAnimation");
            $("#registOverlayBlack").addClass("popupLoadingBackgroundColor");

            //タイトル表示(車種)
            $("#SeriesSelectPage1Title").show();

            //タイトル非表示(グレード、色)
            $("#SeriesSelectPage2Title").css("display", "none");
            $("#SeriesSelectPage3Title").css("display", "none");
            $("#SeriesSelectPage4Title").css("display", "none");
            $("#SeriesSelectPage5Title").css("display", "none");

            //ボタンエリア
            openSeriesPopup();

            setTimeout(function () {
                //サーバー処理実行
                $("#SeriesSelectPopupButtonDummy").click();
            }, 300);
        });

    });

    //画像表示エリアタップ
    $("#scNscCarSelectArea1").live("click", function (e) {
        if ($("#PageEnabledFlgHidden").val() == "False") return;
        if ($(e.target).is(".scNscCarStatusArea li span.scNscCarIconCar,.scNscCarStatusArea li span.scNscCarIconCar *") === true) {
            return;
        }
        if ($("#selFllwupboxSalesBkgno").val().length > 0) return;

        //スクリプトの遅延読み込み
        SC3080201.requirePartialScript("../Scripts/SC3080202/SC3080202.SeriesPopup.js", function () {

            //共通読込みアニメーション変更
            $("#processingServer").addClass("seriesSelectPopupLoadingAnimation");
            $("#registOverlayBlack").addClass("popupLoadingBackgroundColor");

            //タイトル表示(グレード)
            $("#SeriesSelectPage2Title").show();

            //タイトル非表示(車種、色)
            $("#SeriesSelectPage1Title").css("display", "none");
            $("#SeriesSelectPage3Title").css("display", "none");
            $("#SeriesSelectPage4Title").css("display", "none");
            $("#SeriesSelectPage5Title").css("display", "none");

            //ボタンエリア
            openSeriesPopup();

            setTimeout(function () {
                //サーバー処理実行
                $("#SeriesSelectPopupUpdateButtonDummy").click();
            }, 300);
        });
    });
});

$(function () {
    if ($("#PageEnabledFlgHidden").val() == "False") return;
    if ($("#selFllwupboxSalesBkgno").val().length > 0) return;
    //キーボード表示
    //$01 No.181 タップエリア変更
    $(".scNscCarStatusArea li span.scNscCarIconCar .scNscCarIconCarTapArea").NumericKeypad({
        maxDigits: 2,
        acceptDecimalPoint: false,
        defaultValue: 1,

        completionLabel: HtmlEncode($("#CompletionNumericMessage").val()),
        cancelLabel: HtmlEncode($("#CancelNumericMessage").val()),

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
            $(".scNscCarStatusArea li span.scNscCarIconCar .scNscCarIconCarTapArea").NumericKeypad("setValue", quantiry);
        }
    });
});

/** 一押し車種切り替えイベント **/
function changeMostPreferred() {

    //商談中のみ編集可能
    if ($("#PageEnabledFlgHidden").val() == "False") return;

    //一押し希望車ボタン無効化
    $(this).removeClass("NotMost").addClass("Most").unbind("click", changeMostPreferred);

    //一押し車種変更を画面に反映する。
    $("#ScNscSelectCarAreaUpdatePanel > ul > div").each(function (index, Element) {
        if ($(this).children(":nth-child(1)").attr("seqno") == $("#selSeqnoHidden").val()) {
            $(this).children(":nth-child(14)").val("1");
            $(this).children(":nth-child(1)").removeClass("Most").removeClass("NotMost").addClass("Most");
        } else {
            $(this).children(":nth-child(14)").val("0")
            $(this).children(":nth-child(1)").removeClass("Most").removeClass("NotMost").addClass("NotMost");
        }
    });

    //DB処理
    //一時的に非同期通信のクルクル表示を外す。
    window.SC3080201.asyncAnimationEnable = false;
    $("#MostPreferredUpdateDummyButton").click();
    window.SC3080201asyncAnimationEnable = true;
}

//2017/11/20 TCS 河原 TKM独自機能開発 END

//2018/04/18 TCS 前田 （トライ店システム評価）基幹連携を用いたセールス業務実績入力の検証 START
/**
* サフィックス使用可否フラグ判定
* @return {Boolean} 判定結果（true:使用可／false:使用不可）
*/
function isSuffixAvailable() {
    if ("1" == $("#useFlgSuffix").val()) {
        return true;
    } else {
        return false;
    }
}

/**
* 内装色使用可否フラグ判定
* @return {Boolean} 判定結果（true:使用可／false:使用不可）
*/
function isInteriorColorAvailable() {
    if ("1" == $("#useFlgInteriorColor").val()) {
        return true;
    } else {
        return false;
    }
}
//2018/04/18 TCS 前田 （トライ店システム評価）基幹連携を用いたセールス業務実績入力の検証 END
