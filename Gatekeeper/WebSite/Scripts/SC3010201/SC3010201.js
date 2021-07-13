/**
* @fileOverview SC3010201_メインメニュー
*
* @author TCS 寺本
* @version 1.0.0
*/

/// <reference path="../jquery-vsdoc.js"/>
/// <reference path="../jquery.fingerscroll.js"/>
/// <reference path="../eCRB.js"/>
/// <reference path="../eCRB.ui.js"/>

//DOMロード時の処理(重要事項)
$(function () {

    //スクロール設定
    $("#messageInner02").fingerScroll();

    //重要事項タップ処理
    $("#messageInner01 ul li").live("click", function (e) {

        var $target = $(e.target).is("#messageInner01 ul li") ? 
                      $(e.target) : $(e.target).parents("#messageInner01 ul li");
        if ($target.length !== 1) return;
        if ($target.is(".selectionItem") === true) return;
        //通常サイズの場合は大きいサイズに変更
        if ($("#messageInner01 ul").is(".normalMode") === true) messageSizeChange();
        //選択中の項目解除
        clearSelection();
        //選択設定
        var h = $target.outerHeight() + Math.max($target.find("div.msgDetail").height() + 10, 42);
        $target.addClass("selectionItem").css("height", h + "px").one("webkitTransitionEnd", function () {
            $target.find("div.msgDetail").show(80);
        }).find(".messegeLineBox").css("height", (h - 3) + "px");

    });



    //テキスト選択を抑制
    $("#messageInner01 ul li").bind("select", function () { event.preventDefault(); return false; });

    //選択項目の解除
    function clearSelection() {
        $("#messageInner01 ul li.selectionItem div.msgDetail").hide(0);
        $("#messageInner01 ul li.selectionItem").removeClass("selectionItem").css("height", "").find(".messegeLineBox").css("height", "");
    }

    //重要事項サイズ変更
    function messageSizeChange() {

        if ($("#messageInner01 ul").is(".normalMode") === true) {
            //通常サイズから大きいサイズ
            $("#messageInner02").addClass("bigMessageWindow");
            $("#messageInner01 ul").removeClass("normalMode");
            $("#MessageBigSizeLink").hide(0);
            $("#MessageNormalSizeLink").show(0);
        } else {
            //大きいサイズから通常サイズ
            $("#messageInner02").removeClass("bigMessageWindow").one("webkitTransitionEnd", function (e) {
                //枠を縮めてから、４件目以降の連絡事項を消す
                $("#messageInner01 ul").addClass("normalMode");
            });
            //選択中の項目を解除
            clearSelection();
            $("#MessageBigSizeLink").show(0);
            $("#MessageNormalSizeLink").hide(0);
            //スクロール位置初期化
            $("#messageInner02").fingerScroll();
        }
        //ポストバック抑制
        return false;
    }

    //イベントバインド
    $("#MessageBigSizeLink, #MessageNormalSizeLink").bind("mousedown touchstart", messageSizeChange);


});


//DOMロード時の処理(RSS)
$(function () {

    //RSSサイズ変更
    function rssSizeChange() {

        if ($("#newsInner01 ul").is(".normalMode") === true) {
            //通常サイズから大きいサイズ
            $("#newsInner02").addClass("bigMessageWindow");
            $("#newsInner02 ul").removeClass("normalMode");
            $("#RssBigSizeLink").hide(0);
            $("#RssNormalSizeLink").show(0);
        } else {
            //大きいサイズから通常サイズ
            $("#newsInner02").removeClass("bigMessageWindow").one("webkitTransitionEnd", function (e) {
                //枠を縮めてから、４件目以降の連絡事項を消す
                $("#newsInner01 ul").addClass("normalMode");
            });
            $("#RssBigSizeLink").show(0);
            $("#RssNormalSizeLink").hide(0);
        }
        return false;
    }

    //イベントバインド
    $("#RssBigSizeLink, #RssNormalSizeLink").bind("mousedown touchstart", rssSizeChange);

    //URL
    $("#newsInner01 ul li").each(function () {
        //クリックイベント
        $(this).bind("click", $(this), function (e) {
            var url = e.data.attr("siteUrl");
            window.location = "icrop:iurl:20::73::980::624::-1::" + url;
        });
    });
});


/**
* 重要事項新規登録
*/
$(function () {



    /**
    * ポップアップの表示／非表示切り替え
    * @param {boolean} flg 表示／非表示フラグ
    */
    var toggleFunc = function (flg) {
        $("#messageWinPopup").toggle(flg);
        $("#messageWinPopupBlack").toggle(flg);
    };

    //登録ボタンクリック時の処理
    $("#messageAddButton").bind("click", function (e) {
        toggleFunc(true);
        setTimeout(function () {
            //登録ウィンドウ表示
            $("#messageWinPopup").addClass("open");
            $("#messageWinPopupBlack").addClass("open");
        }, 0);
    });
    //閉じる処理
    $("#cancelButton").bind("click", function (e) {
        $("#messageWinPopup").removeClass("open").one("webkitTransitionEnd", function (e) {
            toggleFunc(false);
        });
        $("#messageWinPopupBlack").removeClass("open");
        return false;
    });

    if ($("#postButton").is("[data-errorflg='yes']") === true) {
        toggleFunc(true);
        setTimeout(function () {
            //登録ウィンドウ表示
            $("#messageWinPopup").addClass("open");
            $("#messageWinPopupBlack").addClass("open");
        }, 0);
    }

});