//━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
//SC3080202.SeriesPopup.js
//─────────────────────────────────────
//機能： 希望車PopUp
//補足： 希望車PopUpを開くタイミングにて遅延ロードする
//作成： 2014/04/21 TCS 市川 GTMCタブレット高速化対応（BTS-386）
//更新： 2017/11/20 TCS 河原 TKM独自機能開発
//更新： 2018/04/18 TCS 前田 （トライ店システム評価）基幹連携を用いたセールス業務実績入力の検証
//─────────────────────────────────────

/// <reference path="../jquery.js"/>
/// <reference path="../jquery.fingerscroll.js"/>
/// <reference path="../jquery.NumericKeypad.js"/>
/// <reference path="../SC3080201/Common.js"/>
/***********************************************************
希望車種イベント
***********************************************************/
$(function () {
});

// 選択車種更新後
function commitCompleteSelectedSeriesButtonDummyAfter(editMode, delMode, selSeqNo) {
    // 希望車種を設定
    selectedSeriesDisplay(editMode, delMode, selSeqNo);
    // プロセスを設定
    processDisplay();

}

/***********************************************************
希望車種ポップアップイベント
***********************************************************/


//ポップアップクローズ
function closeSeriesPopup() {
    if ($("#SeriesSelectPopup").hasClass("opened") === true) {
        //クローズ
        $("#SeriesSelectPopup").removeClass("opened").one("webkitTransitionEnd", function (e) {
            $("#SeriesSelectPopup").hide();
        });
    }
};

//2012/03/16 TCS 藤井 【SALES_2】性能改善 Add Start

/**
* ポップアップ位置パターン設定
*/
function setPopupPtn(ptnClass) {
    $("#SeriesSelectPopup").removeClass("ptn1 ptn2").addClass(ptnClass);
}

//2017/11/20 TCS 河原 TKM独自機能開発 START
/**
* ページ切り替え関数
*/
function setPopupPage(pageClass) {

    //モード毎のラベル・ボタンを一旦全部非表示にする
    $("#SeriesSelectCancelLabel, #SeriesSelectBackModelLabel, #SeriesSelectBackGradeLabel, #SeriesSelectBackSuffixLabel, #SeriesSelectBackExteriorColorLabel, #SeriesSelectPage1Title, #SeriesSelectPage2Title, #SeriesSelectPage3Title, #SeriesSelectPage4Title, #SeriesSelectPage5Title, .scNscPopUpCompleteButton").css("display", "none");

    //ページ1
    if (pageClass === "page1") {
        //ボタンタイトル
        $("#SeriesSelectCancelLabel").show();
        $("#SeriesSelectPage1Title").show();
        setModelCheckMark();
    }

    //ページ2
    if (pageClass === "page2") {
        //取り消し表示
        $(".scNscPopUpCancelButton").css("display", "block");

        //ボタンタイトル
        if ($("#SeriesSelectPopup").hasClass("updateMode") === true) {
            //更新
            $("#SeriesSelectCancelLabel").show();
            //削除ボタン表示
            $(".scNsc51PopUpListDeleteButton").show(300);
        } else {
            //新規追加
            $("#SeriesSelectBackModelLabel").show();
        }
        setGradeCheckMark();
        $("#SeriesSelectPage2Title").show();
        $(".scNscPopUpCompleteButton").show();
    }

    //ページ3
    if (pageClass === "page3") {
        //取り消し表示
        $(".scNscPopUpCancelButton").css("display", "block");

        $("#SeriesSelectBackGradeLabel").show();

        setSuffixCheckMark();
        $("#SeriesSelectPage3Title").show();
        $(".scNscPopUpCompleteButton").show();

        if ($("#SeriesSelectPopup").hasClass("updateMode") === true) {
            //削除ボタン非表示
            $(".scNsc51PopUpListDeleteButton").hide(300);
        }
    }

    //ページ4
    if (pageClass === "page4") {
        //取り消し表示
        $(".scNscPopUpCancelButton").css("display", "block");

        //2018/04/18 TCS 前田 （トライ店システム評価）基幹連携を用いたセールス業務実績入力の検証 START
        if (isSuffixAvailable()) {
            $("#SeriesSelectBackSuffixLabel").show();
        } else {
            $("#SeriesSelectBackGradeLabel").show();
        }
        //2018/04/18 TCS 前田 （トライ店システム評価）基幹連携を用いたセールス業務実績入力の検証 END

        setExteriorColorCheckMark();
        $("#SeriesSelectPage4Title").show();
        $(".scNscPopUpCompleteButton").show();
    }

    //ページ5
    if (pageClass === "page5") {
        //ボタンタイトル
        $("#SeriesSelectBackExteriorColorLabel").show();
        $("#SeriesSelectPage5Title").show();
        $(".scNscPopUpCompleteButton").show();
        setInteriorColorCheckMark();


    }

    $("#scNsc51PopUpListWrap").removeClass("page1 page2 page3 page4 page5").addClass(pageClass);

    //スクロール初期化
    $(".scNsc51PopUpScrollWrap").fingerScroll();
}

//モデルのチェックマーク
function setModelCheckMark() {
    //一旦全部のチェック状態削除
    $(".scNsc51PopUpList01 li.scNsc51ListLi1").removeClass("On");
    if ($("#SelectModelcdHidden").val() != "") $(".scNsc51PopUpList01 li.scNsc51ListLi1[itemid='" + $("#SelectModelcdHidden").val() + "']").addClass("On");
}

//グレードのチェックマーク
function setGradeCheckMark() {
    //一旦全部のチェック状態削除
    $(".scNsc51PopUpList02 li.scNsc51ListLi2").removeClass("On");
    if ($("#SelectGradecdHidden").val() != "") $(".scNsc51PopUpList02 li.scNsc51ListLi2"
                                               + "[itemid='" + $("#SelectModelcdHidden").val() + "']"
                                               + "[itemid2='" + $("#SelectGradecdHidden").val() + "']")
                                                    .addClass("On");
}

//サフィックスのチェックマーク
function setSuffixCheckMark() {
    //一旦全部のチェック状態削除
    $(".scNsc51PopUpList03 li.scNsc51ListLi3").removeClass("On");
    if ($("#SelectSuffixcdHidden").val() != "") $(".scNsc51PopUpList03 li.scNsc51ListLi3"
                                               + "[itemid='" + $("#SelectModelcdHidden").val() + "']"
                                               + "[itemid2='" + $("#SelectGradecdHidden").val() + "']"
                                               + "[itemid3='" + $("#SelectSuffixcdHidden").val() + "']")
                                               .addClass("On");
}

//外装色のチェックマーク
function setExteriorColorCheckMark() {
    //一旦全部のチェック状態削除
    $(".scNsc51PopUpList04 li.scNsc51ListLi4").removeClass("On");
    if ($("#SelectExteriorColorcdHidden").val() != "") $(".scNsc51PopUpList04 li.scNsc51ListLi4"
                                               + "[itemid='" + $("#SelectModelcdHidden").val() + "']"
                                               + "[itemid2='" + $("#SelectGradecdHidden").val() + "']"
                                               + "[itemid3='" + $("#SelectSuffixcdHidden").val() + "']"
                                               + "[itemid4='" + $("#SelectExteriorColorcdHidden").val() + "']")
                                               .addClass("On");
}

//内装色のチェックマーク
function setInteriorColorCheckMark() {
    //一旦全部のチェック状態削除
    $(".scNsc51PopUpList05 li.scNsc51ListLi5").removeClass("On");
    if ($("#SelectInteriorColorcdHidden").val() != "") $(".scNsc51PopUpList05 li.scNsc51ListLi5"
                                                       + "[itemid='" + $("#SelectModelcdHidden").val() + "']"
                                                       + "[itemid2='" + $("#SelectGradecdHidden").val() + "']"
                                                       + "[itemid3='" + $("#SelectSuffixcdHidden").val() + "']"
                                                       + "[itemid4='" + $("#SelectExteriorColorcdHidden").val() + "']"
                                                       + "[itemid5='" + $("#SelectInteriorColorcdHidden").val() + "']")
                                                       .addClass("On");
}

//2012/03/16 TCS 藤井 【SALES_2】性能改善 Add End
$(function () {

    //モデルを選択した時のイベント
    $(".scNsc51PopUpList01 li.scNsc51ListLi1").live("click", function (e) {

        //キー取得
        var id = $(this).attr("itemid");

        //グレード制御
        gradeMasterDisplay(id);
        gradeMasterStyle(id);

        //保存
        $("#SelectModelcdHidden").val(id);

        //ページ1→ページ2
        setPopupPage("page2");
    });

    //グレードを選択した時のイベント
    $(".scNsc51PopUpList02 li.scNsc51ListLi2").live("click", function (e) {

        //キー取得
        var id = $(this).attr("itemid");
        var id2 = $(this).attr("itemid2");

        //2018/04/18 TCS 前田 （トライ店システム評価）基幹連携を用いたセールス業務実績入力の検証 START
        if (isSuffixAvailable()) {
            //サフィックス制御
            SuffixMasterDisplay(id, id2);
            SuffixMasterStyle(id, id2);
        } else {
            //外装色制御
            ExteriorColorMasterDisplay(id, id2, "");
            ExteriorColorMasterStyle(id, id2, "");
        }
        //2018/04/18 TCS 前田 （トライ店システム評価）基幹連携を用いたセールス業務実績入力の検証 END

        if (id2 !== $("#SelectGradecdHidden").val()) {
            $("#SelectSuffixcdHidden").val("");
            $("#SelectExteriorColorcdHidden").val("");
            $("#SelectInteriorColorcdHidden").val("");
        }

        //保存
        $("#SelectGradecdHidden").val(id2);

        //2018/04/18 TCS 前田 （トライ店システム評価）基幹連携を用いたセールス業務実績入力の検証 START
        if (isSuffixAvailable()) {
            //ページ2→ページ3
            setPopupPage("page3");
        } else {
            //保存
            $("#SelectSuffixcdHidden").val("");
            //ページ2→ページ4
            setPopupPage("page4");
        }
        //2018/04/18 TCS 前田 （トライ店システム評価）基幹連携を用いたセールス業務実績入力の検証 END
    });

    //サフィックスを選択した時のイベント
    $(".scNsc51PopUpList03 li.scNsc51ListLi3").live("click", function (e) {

        //キー取得
        var id = $(this).attr("itemid");
        var id2 = $(this).attr("itemid2");
        var id3 = $(this).attr("itemid3");

        //外装色制御
        ExteriorColorMasterDisplay(id, id2, id3);
        ExteriorColorMasterStyle(id, id2, id3);

        if (id3 !== $("#SelectSuffixcdHidden").val()) {
            $("#SelectExteriorColorcdHidden").val("");
            $("#SelectInteriorColorcdHidden").val("");
        }

        //保存
        $("#SelectSuffixcdHidden").val(id3);

        //ページ3→ページ4
        setPopupPage("page4");
    });


    //外装色を選択した時のイベント
    $(".scNsc51PopUpList04 li.scNsc51ListLi4").live("click", function (e) {

        //キー取得
        var id = $(this).attr("itemid");
        var id2 = $(this).attr("itemid2");
        var id3 = $(this).attr("itemid3");
        var id4 = $(this).attr("itemid4");

        //内装色制御
        InteriorColorMasterDisplay(id, id2, id3, id4);
        InteriorColorMasterStyle(id, id2, id3, id4);

        if (id4 !== $("#SelectExteriorColorcdHidden").val()) {
            $("#SelectInteriorColorcdHidden").val("");
        }

        //保存
        $("#SelectExteriorColorcdHidden").val(id4);

        //2018/04/18 TCS 前田 （トライ店システム評価）基幹連携を用いたセールス業務実績入力の検証 START
        if (isInteriorColorAvailable()) {
            //ページ4→ページ5
            setPopupPage("page5");
        } else {
            //保存
            $("#SelectInteriorColorcdHidden").val("");

            //ポップアップ終了
            selectedCloseSetting();
        }
        //2018/04/18 TCS 前田 （トライ店システム評価）基幹連携を用いたセールス業務実績入力の検証 END
    });

    //内装色を選択した時のイベント
    $(".scNsc51PopUpList05 li.scNsc51ListLi5").live("click", function (e) {

        //キー取得
        var id5 = $(this).attr("itemid5");

        //保存
        $("#SelectInteriorColorcdHidden").val(id5);

        //ポップアップ終了
        selectedCloseSetting();
    });

    //キャンセルまたは戻るボタンのイベント
    $(".scNscPopUpCancelButton").bind("click", function (e) {
        //ページ1を表示している場合
        //if ($("#scNsc51PopUpListWrap").hasClass("page1") === true) $("#SeriesSelectPopup").fadeOut(300);
        if ($("#scNsc51PopUpListWrap").hasClass("page1") === true) closeSeriesPopup();
        //ページ2を表示している場合
        if ($("#scNsc51PopUpListWrap").hasClass("page2") === true) {
            if ($("#SeriesSelectPopup").hasClass("updateMode") === true) {
                //更新モード
                closeSeriesPopup();
            } else {
                //新規モード
                setPopupPage("page1");
            }
        }
        //ページ3を表示している場合
        if ($("#scNsc51PopUpListWrap").hasClass("page3") === true) {
            setPopupPage("page2");
        }
        //ページ4を表示している場合
        if ($("#scNsc51PopUpListWrap").hasClass("page4") === true) {
            //2018/04/18 TCS 前田 （トライ店システム評価）基幹連携を用いたセールス業務実績入力の検証 START
            if (isSuffixAvailable()) {
                setPopupPage("page3");
            } else {
                setPopupPage("page2");
            }
            //2018/04/18 TCS 前田 （トライ店システム評価）基幹連携を用いたセールス業務実績入力の検証 END
        }
        //ページ5を表示している場合
        if ($("#scNsc51PopUpListWrap").hasClass("page5") === true) {
            setPopupPage("page4");
        }
    });

    //削除ボタンクリック
    //2012/03/16 TCS 藤井 【SALES_2】性能改善 Modify Start
    //    $(".scNsc51PopUpListDeleteButton").bind("click", function (e) {
    $(".scNsc51PopUpListDeleteButton").live("click", function (e) {
        //2012/03/16 TCS 藤井 【SALES_2】性能改善 Modify End

        //削除処理
        $("#SelectSeriesDelMode").val("1");
        selectedCloseSetting();
    });

    //完了ボタンクリック
    $(".scNscPopUpCompleteButton").bind("click", function (e) {
        selectedCloseSetting();
    });

    //ポップアップクローズの監視
    $(document.body).bind("mousedown touchstart", function (e) {
        if ($("#SeriesSelectPopup").is(":visible") === false) return;

        //2012/03/16 TCS 藤井 【SALES_2】性能改善 Modify Start
        //if ($(e.target).is("#SeriesSelectPopup, #SeriesSelectPopup *, #plus, #plus *") === false) {
        if ($(e.target).is("#SeriesSelectPopup, #SeriesSelectPopup *, #plus, #plus *,#scNscCarSelectArea1 *,#scNscCarSelectArea2 *") === false) {
            //2012/03/16 TCS 藤井 【SALES_2】性能改善 Modify End

            //2012/03/16 TCS 藤井 【SALES_2】性能改善 Add Start
            if ($("#registOverlayBlack").hasClass("popupLoadingBackgroundColor") === true) return;
            //HTML削除
            $(".scNsc51PopUpScrollWrap").empty();
            //2012/03/16 TCS 藤井 【SALES_2】性能改善 Add End

            closeSeriesPopup();
        }

        //2012/03/29 TCS 河原 【SALES_2】 START
        //台数選択ポップアップ表示時に希望車種選択ポップアップを閉じるように修正
        if ($(e.target).is(".scNscCarIconCarTapArea") === true) {
            if ($("#registOverlayBlack").hasClass("popupLoadingBackgroundColor") === true) return;
            $(".scNsc51PopUpScrollWrap").empty();
            closeSeriesPopup();
        }
        //2012/03/29 TCS 河原 【SALES_2】 END

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
function gradeMasterDisplay(id) {
    $(".scNsc51PopUpList02 li.scNsc51ListLi2").each(function () {
        $(this).css({ "display": "none" });
        //選択されたモデルに緋付くグレードを表示
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
* サフィックス表示・非表示
* @param {String} id  キー
* @param {String} id2 キー2
* @param {String} id3 キー3
*/
function SuffixMasterDisplay(id, id2) {
    //一旦全部非表示してから対象のみ表示
    $(".scNsc51PopUpList03 li.scNsc51ListLi3").each(function () {
        $(this).css({ "display": "none" });

        //選択されたモデル、グレードに緋付くサフィックスを表示
        if ($(this).attr("itemid") == id && $(this).attr("itemid2") == id2) {
            $(this).css({ "display": "block" });
        }
    });
}

/** サフィックス先頭行、最終行判定 **/
function SuffixMasterStyle(id, id2) {
    var index = 0
    var count = $(".scNsc51PopUpList03 li.scNsc51ListLi3").parent().children("[itemid='" + id + "'][itemid2='" + id2 + "']").size()

    //表示対象のスタイル設定
    $(".scNsc51PopUpList03 li.scNsc51ListLi3").each(function () {
        if ($(this).attr("itemid") == id && $(this).attr("itemid2") == id2) {
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
* 外装色表示・非表示
* @param {String} id  キー
* @param {String} id2 キー2
* @param {String} id3 キー3
*/
function ExteriorColorMasterDisplay(id, id2, id3) {
    //一旦全部非表示してから対象のみ表示
    $(".scNsc51PopUpList04 li.scNsc51ListLi4").each(function () {
        $(this).css({ "display": "none" });

        //2018/04/18 TCS 前田 （トライ店システム評価）基幹連携を用いたセールス業務実績入力の検証 START
        //表示対象の外装色の場合
        if (isDisplayableExteriorColor($(this), id, id2, id3)) {
        //2018/04/18 TCS 前田 （トライ店システム評価）基幹連携を用いたセールス業務実績入力の検証 END
            $(this).css({ "display": "block" });
        }
    });
}

/** 外装色先頭行、最終行判定 **/
function ExteriorColorMasterStyle(id, id2, id3) {
    var index = 0;
    //2018/04/18 TCS 前田 （トライ店システム評価）基幹連携を用いたセールス業務実績入力の検証 START
    var count = countDisplayableExteriorColor(id, id2, id3);
    //2018/04/18 TCS 前田 （トライ店システム評価）基幹連携を用いたセールス業務実績入力の検証 END

    //表示対象のスタイル設定
    $(".scNsc51PopUpList04 li.scNsc51ListLi4").each(function () {
        //2018/04/18 TCS 前田 （トライ店システム評価）基幹連携を用いたセールス業務実績入力の検証 START
        //表示対象の外装色の場合
        if (isDisplayableExteriorColor($(this), id, id2, id3)) {
        //2018/04/18 TCS 前田 （トライ店システム評価）基幹連携を用いたセールス業務実績入力の検証 END
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
* 内装色表示・非表示
* @param {String} id  キー
* @param {String} id2 キー2
* @param {String} id3 キー3
* @param {String} id4 キー4
*/
function InteriorColorMasterDisplay(id, id2, id3, id4) {
    //一旦全部非表示してから対象のみ表示
    $(".scNsc51PopUpList05 li.scNsc51ListLi5").each(function () {
        $(this).css({ "display": "none" });

        //2018/04/18 TCS 前田 （トライ店システム評価）基幹連携を用いたセールス業務実績入力の検証 START
        //表示対象の内装色の場合
        if (isDisplayableInteriorColor($(this), id, id2, id3, id4)) {
        //2018/04/18 TCS 前田 （トライ店システム評価）基幹連携を用いたセールス業務実績入力の検証 END
            $(this).css({ "display": "block" });
        }
    });
}

/** 内装色先頭行、最終行判定 **/
function InteriorColorMasterStyle(id, id2, id3, id4) {
    var index = 0;
    //2018/04/18 TCS 前田 （トライ店システム評価）基幹連携を用いたセールス業務実績入力の検証 START
    var count = countDisplayableInteriorColor(id, id2, id3, id4);
    //2018/04/18 TCS 前田 （トライ店システム評価）基幹連携を用いたセールス業務実績入力の検証 END

    //表示対象のスタイル設定
    $(".scNsc51PopUpList05 li.scNsc51ListLi5").each(function () {
        //2018/04/18 TCS 前田 （トライ店システム評価）基幹連携を用いたセールス業務実績入力の検証 START
        //表示対象の内装色の場合
        if (isDisplayableInteriorColor($(this), id, id2, id3, id4)) {
        //2018/04/18 TCS 前田 （トライ店システム評価）基幹連携を用いたセールス業務実績入力の検証 END
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

/**
* 表示対象外装色判定
* @param  {Object}  listItem DOMオブジェクト（外装色リストのListItem）
* @param  {String}  id       キー
* @param  {String}  id2      キー2
* @param  {String}  id3      キー3
* @return {Boolean} 判定結果（true:表示対象である／false:表示対象でない）
*/
function isDisplayableExteriorColor(listItem, id, id2, id3) {
    var ret = false;

    if (isSuffixAvailable()) {
        //サフィックス使用可の場合
        //選択されたモデル、グレード、サフィックスに緋付く外装色を表示
        if (listItem.attr("itemid") == id && listItem.attr("itemid2") == id2 && listItem.attr("itemid3") == id3) {
            ret = true;
        }
    } else {
        //サフィックス使用不可の場合
        //選択されたモデル、グレードに紐緋付く外装色を表示
        if (listItem.attr("itemid") == id && listItem.attr("itemid2") == id2) {
            ret = true;
        }
    }
    return ret;
}

/**
* 表示対象外装色数を算出
* @param  {String} id  キー
* @param  {String} id2 キー2
* @param  {String} id3 キー3
* @return {int}    表示対象となる外装色の数
*/
function countDisplayableExteriorColor(id, id2, id3) {
    var count = 0;

    if (isSuffixAvailable()) {
        //サフィックス使用可の場合
        count = $(".scNsc51PopUpList04 li.scNsc51ListLi4").parent().children("[itemid='" + id + "'][itemid2='" + id2 + "'][itemid3='" + id3 + "']").size();
    } else {
        //サフィックス使用不可の場合
        count = $(".scNsc51PopUpList04 li.scNsc51ListLi4").parent().children("[itemid='" + id + "'][itemid2='" + id2 + "']").size();
    }
    return count;
}

/**
* 表示対象内装色判定
* @param  {Object}  listItem DOMオブジェクト（内装色リストのListItem）
* @param  {String}  id       キー
* @param  {String}  id2      キー2
* @param  {String}  id3      キー3
* @param  {String}  id4      キー4
* @return {Boolean} 判定結果（true:表示対象である／false:表示対象でない）
*/
function isDisplayableInteriorColor(listItem, id, id2, id3, id4) {
    var ret = false;

    if (isSuffixAvailable()) {
        //サフィックス使用可の場合
        //選択されたモデル、グレード、サフィックス、外装色に緋付く内装色を表示
        if (listItem.attr("itemid") == id && listItem.attr("itemid2") == id2 && listItem.attr("itemid3") == id3 && listItem.attr("itemid4") == id4) {
            ret = true;
        }
    } else {
        //サフィックス使用不可の場合
        //選択されたモデル、グレード、外装色に緋付く内装色を表示
        if (listItem.attr("itemid") == id && listItem.attr("itemid2") == id2 && listItem.attr("itemid4") == id4) {
            ret = true;
        }
    }
    return ret;
}

/**
* 表示対象内装色数を算出
* @param  {String} id  キー
* @param  {String} id2 キー2
* @param  {String} id3 キー3
* @param  {String} id4 キー4
* @return {int}    表示対象となる内装色の数
*/
function countDisplayableInteriorColor(id, id2, id3, id4) {
    var count = 0;

    if (isSuffixAvailable()) {
        //サフィックス使用可の場合
        count = $(".scNsc51PopUpList05 li.scNsc51ListLi5").parent().children("[itemid='" + id + "'][itemid2='" + id2 + "'][itemid3='" + id3 + "'][itemid4='" + id4 + "']").size();
    } else {
        //サフィックス使用不可の場合
        count = $(".scNsc51PopUpList05 li.scNsc51ListLi5").parent().children("[itemid='" + id + "'][itemid2='" + id2 + "'][itemid4='" + id4 + "']").size();
    }
    return count;
}
//2018/04/18 TCS 前田 （トライ店システム評価）基幹連携を用いたセールス業務実績入力の検証 END