//━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
//SC3080202.Compcar.js
//─────────────────────────────────────
//機能： 顧客詳細(商談情報)
//補足： 
//作成： 2011/11/24 TCS 小野
//更新： 2012/04/24 TCS 安田 【SALES_2】競合車種ページ遷移の速度改善（ユーザー課題 No.22）
//更新： 2014/04/21 TCS 市川 GTMCタブレット高速化対応（BTS-386）
//─────────────────────────────────────

/// <reference path="../jquery.js"/>
/// <reference path="../jquery.fingerscroll.js"/>

/***********************************************************
競合車種
***********************************************************/

//2014/04/21 TCS市川 GTMCタブレット高速化対応 DELETE

/***********************************************************
競合車種ポップアップ初期化
***********************************************************/
/**
* 競合車種ポップアップ初期化
*/
function initCompPopup() {

    //チェック状態クリア
    $(".scNsc51CompPopUpList01 li.scNsc51CompListLi1, .scNsc51CompPopUpList02 li.scNsc51CompListLi2").removeClass("On");

    //選択されている競合車種をポップアップに反映
    $(".scNscCompetingCarAreaHidden").each(function () {
        var maker = $(this).children(":nth-child(2)").val();
        var model = $(this).children(":nth-child(1)").val();
        $(".scNsc51CompPopUpList01 li.scNsc51CompListLi1[makercd='" + maker + "']").addClass("On");
        $(".scNsc51CompPopUpList02 li.scNsc51CompListLi2[makercd='" + maker + "'][compcd='" + model + "']").addClass("On");
    });
}

/**
* 競合車種非同期更新終了時の関数
*/
function commitCompleteSelectedCompButtonDummyAfter() {
    //$("#dispCompeCarCountNoFlg").toggle($(".scNscCompetingCarAreaHidden").size() === 0);
    compCarEventSizeChange("normalMode");
}

/***********************************************************
競合車種ポップアップイベント
***********************************************************/

//2012/03/16 TCS 藤井 【SALES_2】性能改善 Add Start
/**
* ページ切り替え関数
*/
function setPopupPageComp(pageClass) {

    //2012/04/24 TCS 安田 【SALES_2】競合車種ページ遷移の速度改善（ユーザー課題 No.22）START
    //モード毎のラベル・ボタンを一旦全部非表示にする
    //$("#CompCarPopupMakerTitle,#CompCarPopupModelTitle,#CompCarPopupCancelLabel,#CompCarPopupMakerBkLabel,.scNscCompPopUpCompleteButton").css("display", "none");
    //2012/04/24 TCS 安田 【SALES_2】競合車種ページ遷移の速度改善（ユーザー課題 No.22）END

    //ページ１
    if (pageClass === "page1") {

        //取り消し、メーカー選択表示
        $(".scNscCompPopUpCancelButton").css("display", "block");

        //文言ラベルの制御
        $("#CompCarPopupCancelLabel").show();
        $("#CompCarPopupMakerTitle").show();

        //2012/04/24 TCS 安田 【SALES_2】競合車種ページ遷移の速度改善（ユーザー課題 No.22）START
        //モード毎のラベル・ボタンを非表示にする
        $("#CompCarPopupModelTitle,#CompCarPopupMakerBkLabel,.scNscCompPopUpCompleteButton").css("display", "none");
        //ページクラス設定
        $("#CompCarSelectPopupListWrap").removeClass("page2").addClass(pageClass);
        //2012/04/24 TCS 安田 【SALES_2】競合車種ページ遷移の速度改善（ユーザー課題 No.22）END
    }

    //ページ２
    if (pageClass === "page2") {

        //文言ラベルの制御
        $("#CompCarPopupModelTitle").show();
        $("#CompCarPopupMakerBkLabel").show();
        $(".scNscCompPopUpCompleteButton").show();

        //2012/04/24 TCS 安田 【SALES_2】競合車種ページ遷移の速度改善（ユーザー課題 No.22）START
        //モード毎のラベル・ボタンを非表示にする
        $("#CompCarPopupMakerTitle,#CompCarPopupCancelLabel").css("display", "none");
        //ページクラス設定
        $("#CompCarSelectPopupListWrap").removeClass("page1").addClass(pageClass);
        //2012/04/24 TCS 安田 【SALES_2】競合車種ページ遷移の速度改善（ユーザー課題 No.22）END
    }

    //2012/04/24 TCS 安田 【SALES_2】競合車種ページ遷移の速度改善（ユーザー課題 No.22）START
    $(".scNsc51PopUpScrollWrapComp").each(function () {
        //最初の１回のみスクロール化処理を実施する
        if ($(this).children(".scroll-bar").length === 0) {
            //スクロール初期化
            $(this).fingerScroll();
        }
    });

    //ページ２ならば、スクロールを先頭にする
    if (pageClass === "page2") {
        $("#CompPopUpList02Scroll").children(".scroll-inner").css({ "transform": "translate3d(0px, 0px, 0px)" });
    }
    //2012/04/24 TCS 安田 【SALES_2】競合車種ページ遷移の速度改善（ユーザー課題 No.22）END

}

//2014/04/21 TCS市川 GTMCタブレット高速化対応　DELETE

//2012/03/16 TCS 藤井 【SALES_2】性能改善 Add End

$(function () {

    //2012/04/24 TCS 安田 【SALES_2】競合車種ページ遷移の速度改善（ユーザー課題 No.22）START
    //競合車種ポップアップの初期化
    //$(".scNsc51PopUpScrollWrapComp").fingerScroll();
    //2012/04/24 TCS 安田 【SALES_2】競合車種ページ遷移の速度改善（ユーザー課題 No.22）END

    //2012/03/16 TCS 藤井 【SALES_2】性能改善 Delete 

    //2014/04/21 TCS市川 GTMCタブレット高速化対応　DELETE

    //キャンセルボタン押下時の処理
    $(".scNscCompPopUpCancelButton").bind("click", function (e) {
        //ページ１を表示している場合
        if ($("#CompCarSelectPopupListWrap").hasClass("page1") === true) $("#CompCarSelectPopup").fadeOut(300);
        //ページ２を表示している場合
        if ($("#CompCarSelectPopupListWrap").hasClass("page2") === true) {

            //2012/03/16 TCS 藤井 【SALES_2】性能改善 Modify Start
            //setPopupPage("page1");
            setPopupPageComp("page1");
            //2012/03/16 TCS 藤井 【SALES_2】性能改善 Modify End

        }
    });

    //メーカーを選択した時のイベント
    $(".scNsc51CompPopUpList01 li.scNsc51CompListLi1").live("click", function (e) {

        //キー取得
        var id = $(this).attr("makercd");
        $(".scNsc51CompPopUpList02 li.scNsc51CompListLi2").css("display", "none");
        $(".scNsc51CompPopUpList02 li.scNsc51CompListLi2[makercd='" + id + "']").show();
        modelMasterStyle(id);
        //ページ１→ページ２

        //2012/03/16 TCS 藤井 【SALES_2】性能改善 Modify Start
        //setPopupPage("page2");
        setPopupPageComp("page2");
        //2012/03/16 TCS 藤井 【SALES_2】性能改善 Modify End

    });

    //モデルを選択した時のイベント
    $(".scNsc51CompPopUpList02 li.scNsc51CompListLi2").live("click", function (e) {

        //キー取得
        var maker = $(this).attr("makercd");
        var model = $(this).attr("compcd");
        $(this).toggleClass("On");
        //メーカーリストのチェック状態を最新化
        setMekerCheckState(maker);
    });

    //メーカーリストのチェック状態を最新化
    function setMekerCheckState(maker) {

        if ($(".scNsc51CompPopUpList02 li.scNsc51CompListLi2[makercd='" + maker + "']").is(".On") === true) {
            //メーカーリストのチェックOn
            $(".scNsc51CompPopUpList01 li.scNsc51CompListLi1[makercd='" + maker + "']").addClass("On");
        } else {
            //メーカーリストのチェックOff
            $(".scNsc51CompPopUpList01 li.scNsc51CompListLi1[makercd='" + maker + "']").removeClass("On");
        }
    }

    //完了ボタンを押したときのイベント
    $(".scNscCompPopUpCompleteButton").bind("click", function (e) {
        //隠し項目に一括反映
        //チェックOFFを全体に反映
        $(".scNsc51CompPopUpList02 li .scNsc51CompPopUpList02Hidden input[type='hidden']:nth-child(3)").val("False");
        $(".scNsc51CompPopUpList02 li.On .scNsc51CompPopUpList02Hidden input[type='hidden']:nth-child(3)").val("True");
        //ＤＢ反映用ダミーボタン押下
        $("#commitCompleteSelectedCompButtonDummy").click();
        //ポップアップクローズ
        $("#CompCarSelectPopup").fadeOut(300);
    });

    //ポップアップクローズの監視
    $(document.body).bind("mousedown touchstart", function (e) {
        if ($("#CompCarSelectPopup").is(":visible") === false) return;
        if ($(e.target).is("#CompCarSelectPopup, #CompCarSelectPopup *, #scNscCompetingCarAreaInner, #scNscCompetingCarAreaInner *") === false) {

            //2012/03/16 TCS 藤井 【SALES_2】性能改善 Add Start
            if ($("#registOverlayBlack").hasClass("popupLoadingBackgroundColor") === true) return;
            //HTML削除
            $(".scNsc51PopUpScrollWrapComp").empty();
            //2012/03/16 TCS 藤井 【SALES_2】性能改善 Add End

            $("#CompCarSelectPopup").fadeOut(300);
        }
    });
});

/** モデル先頭行、最終行判定 **/
function modelMasterStyle(id) {
    var index = 0
    var count = $(".scNsc51CompPopUpList02 li.scNsc51CompListLi2").parent().children("[makercd='" + id + "']").size()
    //表示対象のスタイル設定
    $(".scNsc51CompPopUpList02 li.scNsc51CompListLi2").each(function () {
        if ($(this).attr("makercd") == id) {
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