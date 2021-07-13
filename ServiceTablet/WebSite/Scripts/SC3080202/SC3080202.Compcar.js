/// <reference path="../jquery.js"/>
/// <reference path="../jquery.fingerscroll.js"/>

/***********************************************************
競合車種
***********************************************************/

/**
* 競合車種の拡大縮小
*/
function compCarEventSizeChange(modeClass) {

    if ($(".titleCompeMaker").size() <= 0) {
        $("#dispCompeCarCountFlg").hide(0);
        $("#dispCompeCarCountNoFlg").show(0);
    } else {
        $("#dispCompeCarCountFlg").show(0);
        $("#dispCompeCarCountNoFlg").hide(0);
    }

    if (modeClass !== undefined && typeof modeClass === "string") {
        //指定モードで設定
        $("#scNscCompetingCarAreaInner").removeClass("normalMode bigMode").addClass(modeClass);
    } else {

        //切り替え処理
        if ($("#scNscCompetingCarAreaInner").hasClass("normalMode") === true) {
            //拡大サイズ
            $("#scNscCompetingCarAreaInner").removeClass("normalMode").addClass("bigMode");
        } else {
            //縮小サイズ
            $("#scNscCompetingCarAreaInner").removeClass("bigMode").addClass("normalMode");            
        }

    }

    //拡大縮小ボタン
    if ($("#scNscCompetingCarAreaInner").hasClass("normalMode") === true) {
        //縮小サイズ
        if ($("#otherCountHidden").val() == "0") {
            $("#bigSizeLinkButton").hide(0);
        } else {
            $("#bigSizeLinkButton").show(0);
        }
        $("#normalSizeLinkButton").hide(0);
    } else {
        //拡大サイズ
        $("#bigSizeLinkButton").hide(0);
        $("#normalSizeLinkButton").show(0);
    }
    $("#ScNscCompeCarScrollPane").fingerScroll();
    return false;
}

$(function () {
    //イベントバインド
    $("#scNscCompeCarArea .scNscCompetingCarArea .moreCarEvent").live("mousedown touchstart", compCarEventSizeChange);
});


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
$(function () {

    //希望車種ポップアップの初期化
    $(".scNsc51PopUpScrollWrapComp").fingerScroll();
    

    /**
    * ページ切り替え関数
    */
    function showPopup() {

        //読み取り専用の場合は無視
        if ($("#PageEnabledFlgHidden").val() == "False") return;

        //１ページ目を表示
        setPopupPage("page1");

        //ポップアップ初期化
        initCompPopup();

        //ポップアップ表示
        $("#CompCarSelectPopup").fadeIn(300);
    };

    /**
    * ページ切り替え関数
    */
    function setPopupPage(pageClass) {
        //モード毎のラベル・ボタンを一旦全部非表示にする
        $("#CompCarPopupMakerTitle,#CompCarPopupModelTitle,#CompCarPopupCancelLabel,#CompCarPopupMakerBkLabel,.scNscCompPopUpCompleteButton").css("display", "none");

        //ページ１
        if (pageClass === "page1") {
            //文言ラベルの制御
            $("#CompCarPopupCancelLabel").show(0);
            $("#CompCarPopupMakerTitle").show(0);
        }

        //ページ２
        if (pageClass === "page2") {
            //文言ラベルの制御
            $("#CompCarPopupModelTitle").show(0);
            $("#CompCarPopupMakerBkLabel").show(0);
            $(".scNscCompPopUpCompleteButton").show(0);
        }
        //ページクラス設定
        $("#CompCarSelectPopupListWrap").removeClass("page1 page2").addClass(pageClass);
        //スクロール初期化
        $(".scNsc51PopUpScrollWrapComp").fingerScroll();
    };

    //競合車種エリア押下時の処理 (ポップアップ表示)
    $("#scNscCompetingCarAreaInner,#dispCompeCarCountNoFlg").live("click", function (e) {
        if ($(e.target).is("#scNscCompeCarArea .scNscCompetingCarArea .moreCarEvent, #scNscCompeCarArea .scNscCompetingCarArea .moreCarEvent *") === true) return;
        //ポップアップ表示
        showPopup();
    });

    //キャンセルボタン押下時の処理
    $(".scNscCompPopUpCancelButton").bind("click", function (e) {
        //ページ１を表示している場合
        if ($("#CompCarSelectPopupListWrap").hasClass("page1") === true) $("#CompCarSelectPopup").fadeOut(300);
        //ページ２を表示している場合
        if ($("#CompCarSelectPopupListWrap").hasClass("page2") === true) {
            setPopupPage("page1");
        }
    });

    //メーカーを選択した時のイベント
    $(".scNsc51CompPopUpList01 li.scNsc51CompListLi1").live("click", function (e) {

        //キー取得
        var id = $(this).attr("makercd");
        $(".scNsc51CompPopUpList02 li.scNsc51CompListLi2").css("display", "none");
        $(".scNsc51CompPopUpList02 li.scNsc51CompListLi2[makercd='" + id + "']").show(0);
        modelMasterStyle(id);
        //ページ１→ページ２
        setPopupPage("page2");
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