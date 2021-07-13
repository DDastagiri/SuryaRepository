/**
* @fileOverview SC3010201_メインメニュー
*
* @author TCS 寺本
* @version 1.0.0
*/

/// <reference path="../jquery-1.5.2.js"/>
/// <reference path="SC3010201.MainMenuFingerScroll.js"/>

//2019/03/14 NSK 山田 18PRJ02750-00_(トライ店システム評価)サービス業務における次世代オペレーション実現の為の性能対策 START
/****************************************
* グローバル変数宣言
****************************************/
//IFrameロード終了フラグ
var gIsIFrameLoadFlag;

//データ読み込み終了フラグ
var gIsDataReadFlag;

//2019/03/14 NSK 山田 18PRJ02750-00_(トライ店システム評価)サービス業務における次世代オペレーション実現の為の性能対策 END

//DOMロード時の処理(重要事項)
$(function () {

    //スクロール設定
    //MOD START 2012/01/16
    //$("#messageInner02").fingerScroll();
    $("#messageInner02").mainMenuFingerScroll({ scrollMode: "all" });
    //MOD END 2012/01/16

    //重要事項タップ処理
    $("#messageInner01 ul li").live("click", function (e) {

        var $target = $(e.target).is("#messageInner01 ul li") ?
                      $(e.target) : $(e.target).parents("#messageInner01 ul li");
        if ($target.length !== 1) return;
        if ($target.is(".selectionItem") === true) return;
        //通常サイズの場合は大きいサイズに変更
        //MOD START 2012/01/16
        //if ($("#messageInner01 ul").is(".normalMode") === true) messageSizeChange();
        if ($("#messageInner01").is(".normalMode") === true) messageSizeChange();
        //MOD END 2012/01/16
        //選択中の項目解除
        clearSelection();
        //選択設定
        var h = $target.outerHeight() + Math.max($target.find("div.msgDetail").height() + 10, 42);
        $target.addClass("selectionItem").css("height", h + "px").one("webkitTransitionEnd", function () {
            $target.find("div.msgDetail").show(80);
        }).find(".messegeLineBox").css("height", (h - 3) + "px");

    });

    // 2012/02/24 TCS 平野 【SALES_1B】 START
    $("#messageWinPopupBlack").bind("click", function (event) {
        $("#cancelButton").click();
    });
    // 2012/02/24 TCS 平野 【SALES_1B】 END

    // 2012/01/23 TCS 相田 【SALES_1B】 START
    //削除ボタンタップ処理
    $("#messageInner01 ul li.selectionItem div.deleteBntArea").live("click", function (e) {
        var account = $("#messageInner01 ul li.selectionItem div.hiddenValueAreaMessageNo").find("span").text().trim();
        //コードビハインド用に代入
        $("#createAccountHiddenField").val(account);
        $("#DeleteButtonHidden").click();
    });

    //スワイプの設定
    swipeSetting();
    // 2012/01/23 TCS 相田 【SALES_1B】 END

    //テキスト選択を抑制
    $("#messageInner01 ul li").bind("select", function () { event.preventDefault(); return false; });

    //選択項目の解除
    function clearSelection() {
        // 2012/01/23 TCS 相田 【SALES_1B】 START
        //削除ボタン非表示
        $("#messageInner01 ul li.selectionItem div.deleteBntArea").hide();
        // 2012/01/23 TCS 相田 【SALES_1B】 END

        $("#messageInner01 ul li.selectionItem div.msgDetail").hide(0);
        $("#messageInner01 ul li.selectionItem").removeClass("selectionItem").css("height", "").find(".messegeLineBox").css("height", "");

    }

    //重要事項サイズ変更
    function messageSizeChange() {

        //MOD START 2012/01/16
        //if ($("#messageInner01 ul").is(".normalMode") === true) {
        if ($("#messageInner01").is(".normalMode") === true) {
            //MOD END 2012/01/16
            //通常サイズから大きいサイズ
            $("#messageInner02").addClass("bigMessageWindow");
            //MOD START 2012/01/16
            //$("#messageInner01 ul").removeClass("normalMode");
            $("#messageInner01").removeClass("normalMode");
            //MOD END 2012/01/16
            //DEL START 2012/01/16
            //$("#MessageBigSizeLink").hide(0);
            //$("#MessageNormalSizeLink").show(0);
            //DEL END 2012/01/16
        } else {
            //大きいサイズから通常サイズ
            $("#messageInner02").removeClass("bigMessageWindow").one("webkitTransitionEnd", function (e) {
                //枠を縮めてから、４件目以降の連絡事項を消す
                //MOD START 2012/01/16
                //$("#messageInner01 ul").addClass("normalMode");
                $("#messageInner01").addClass("normalMode");
                //MOD END 2012/01/16
            });
            //選択中の項目を解除
            clearSelection();
            //DEL START 2012/01/16
            //$("#MessageBigSizeLink").show(0);
            //$("#MessageNormalSizeLink").hide(0);
            //DEL END 2012/01/16
            //スクロール位置初期化
            //MOD START 2012/01/16
            //$("#messageInner02").fingerScroll();
            $("#messageInner02").mainMenuFingerScroll({ scrollMode: "all" });
            //MOD END 2012/01/16
        }
        //ポストバック抑制
        return false;
    }

    //イベントバインド
    //MOD START 2012/01/16
    $("#MessageBigSizeLink, #MessageNormalSizeLink").live("mousedown touchstart", messageSizeChange);
    //MOD END 2012/01/16

    //2013/12/19 TMEJ 小澤 IT9611_次世代サービス 工程管理機能開発 START
    //表示アイコンイベント
    $(".WhatsNewButtonClass").live("click", function (e) {
        //現在の座標一覧を取得
        var WhatsNewData = new WebKitCSSMatrix($(".WhatsNewArea").css("transform"))

        //座標チェック
        if (WhatsNewData.e == 0) {
            //まだ動かしていない場合

            //WhatsNewを表示する
            WhatsNewOpen();

            //クルクル表示
            WhatsNewActiveOn();

            //2019/03/14 NSK 山田 18PRJ02750-00_(トライ店システム評価)サービス業務における次世代オペレーション実現の為の性能対策 START
            //IFrameロード終了フラグ初期化
            gIsIFrameLoadFlag = false;
            //データ読み込み終了フラグ初期化
            gIsDataReadFlag = false;

            //iFrame読み込み
            var container = $('#dashboardBox');
            var $iframe = $("<iframe id='dashboardFrame'  height='100%' width='100%' src='SC3140102.aspx' />");
            container.empty().append($iframe);
            //2019/03/14 NSK 山田 18PRJ02750-00_(トライ店システム評価)サービス業務における次世代オペレーション実現の為の性能対策 END

            //再描画
            $("#ReloadButton").click();

        } else {
            //すでに動かしている場合場合

            //WhatsNewを非表示にする
            WhatsNewClose();

        }

    });
    //2013/12/19 TMEJ 小澤 IT9611_次世代サービス 工程管理機能開発 END

});

//2013/12/19 TMEJ 小澤 IT9611_次世代サービス 工程管理機能開発 START
/**
* WhatsNew表示処理.
* @return {}
*/
function WhatsNewOpen() {
    $(".WhatsNewArea").css({
        '-webkit-transition': 'transform 500ms ease',
        'transform': 'translate(351px, 0)'
    });
}
/**
* WhatsNew非表示処理.
* @return {}
*/
function WhatsNewClose() {
    $(".WhatsNewArea").css({
        '-webkit-transition': 'transform 500ms ease',
        'transform': 'translate(0px, 0)'
    });
}
/**
* クルクル表示.
* @return {}
*/
function WhatsNewActiveOn() {
    $(".WhatsNewActiveOverlay").css("display", "block");
    $(".WhatsNewActiveIcon").css("display", "block");
    //2019/03/14 NSK 山田 18PRJ02750-00_(トライ店システム評価)サービス業務における次世代オペレーション実現の為の性能対策 START
    //マスクON
    $("#contentsLeftBoxInner").css({
        "display": "none"
        , "z-index": "4"
    });
    //2019/03/14 NSK 山田 18PRJ02750-00_(トライ店システム評価)サービス業務における次世代オペレーション実現の為の性能対策 END
}

/**
* クルクル非表示.
* @return {}
*/
function WhatsNewActiveOff() {
    $(".WhatsNewActiveOverlay").css("display", "none");
    $(".WhatsNewActiveIcon").css("display", "none");
    //2019/03/14 NSK 山田 18PRJ02750-00_(トライ店システム評価)サービス業務における次世代オペレーション実現の為の性能対策 START
    //マスクOFF
    $("#contentsLeftBoxInner").css({
        "display": "block"
        , "z-index": "10002"
    });
    //2019/03/14 NSK 山田 18PRJ02750-00_(トライ店システム評価)サービス業務における次世代オペレーション実現の為の性能対策 END
}
//2013/12/19 TMEJ 小澤 IT9611_次世代サービス 工程管理機能開発 END

//DOMロード時の処理(RSS)
$(function () {

    //ADD START 2012/01/16
    $("#newsInner02").mainMenuFingerScroll({ scrollMode: "all" });
    //ADD END 2012/01/16

    //RSSサイズ変更
    function rssSizeChange() {

        //MOD START 2012/01/16
        //if ($("#newsInner01 ul").is(".normalMode") === true) {
        if ($("#newsInner01").is(".normalMode") === true) {
            //MOD END 2012/01/16
            //通常サイズから大きいサイズ
            $("#newsInner02").addClass("bigMessageWindow");
            //MOD START 2012/01/16
            //$("#newsInner02 ul").removeClass("normalMode");
            $("#newsInner01").removeClass("normalMode");
            //MOD END 2012/01/16
            //DEL START 2012/01/16
            //$("#RssBigSizeLink").hide(0);
            //$("#RssNormalSizeLink").show(0);
            //DEL END 2012/01/16
        } else {
            //大きいサイズから通常サイズ
            $("#newsInner02").removeClass("bigMessageWindow").one("webkitTransitionEnd", function (e) {
                //枠を縮めてから、４件目以降の連絡事項を消す
                //MOD START 2012/01/16
                //$("#newsInner01 ul").addClass("normalMode");
                $("#newsInner01").addClass("normalMode");
                //MOD END 2012/01/16
            });
            //DEL START 2012/01/16
            //$("#RssBigSizeLink").show(0);
            //$("#RssNormalSizeLink").hide(0);
            //DEL END 2012/01/16
        }
        return false;
    }

    //イベントバインド
    $("#RssBigSizeLink, #RssNormalSizeLink").live("mousedown touchstart", rssSizeChange);

    //MOD START 2012/01/19
    //$("#newsInner01 ul li").each(function () {
    //クリックイベント
    //MOD START 2012/01/19
    //$(this).bind("click", $(this), function (e) {
    //var url = e.data.attr("siteUrl");
    //window.location = "icrop:iurl:20::73::980::624::0::" + url;
    //});
    //});
    //URLタップ
    $("#newsInner01 ul li").live("click", function () {
        var url = $(this).attr("siteUrl");
        window.location = "icrop:iurl:20::73::980::624::0::" + url;
    });
    //MOD END 2012/01/19
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

        //2012/03/02 TCS 平野 【SALES_1B】 START
        if (flg == true) {
            $("#MstPG_CustomerSearchTextBox").CustomTextBox("disabled", true);
        } else {
            $("#MstPG_CustomerSearchTextBox").CustomTextBox("disabled", false);
        }
        //2012/03/02 TCS 平野 【SALES_1B】 END
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

// 2012/01/23 TCS 相田 【SALES_1B】 START
// スワイプ処理********************************************************************************/
var swipeOptions =
{
    swipeLeft: swipeLeft,
    swipeRight: swipeRight,
    threshold: 60 //60pxスワイプすると処理実行
}

/**
* スワイプ登録.
* 
* @param {-} - -
* @return {-} -
* 
* @example 
*  -
*/
function swipeSetting() {
    var swipeTarget = $("#messageInner02 li");
    swipeTarget.swipe(swipeOptions);
}

/**
* スワイプ実行時.
* 
* @param {object} event イベントオブジェクト
* @param {String} direction 方向(up ,down, left, right)
* @param {Integer} distance 移動距離px
* @return {-} -
* 
* @example 
*  -
*/
// 左にスワイプ操作した時の処理
function swipeLeft(event, direction, distance) {
    var del = $("#messageInner01 ul li.selectionItem div.hiddenValueArea").find("span").text();
    var loginAccount = $("#accountHiddenField").val();

    //削除ボタンを表示にする
    if ((distance > 0) && (del.trim() == loginAccount.trim())) {
        //ボタン表示位置が詳細部分ではなくメッセージ全体の中央になるように計算
        var divHeight = $("#messageInner01 ul li.selectionItem").height() / 2;
        var buttonHeight = $("#messageInner01 ul li.selectionItem div.deleteBntArea").height() / 2;
        var topSize = divHeight - buttonHeight;
        $("#messageInner01 ul li.selectionItem div.deleteBntArea").css("top", topSize + "px");
        $("#messageInner01 ul li.selectionItem div.deleteBntArea").show();
    }
}

// 右にスワイプ操作した時の処理
function swipeRight(event, direction, distance) {
    //削除ボタンを非表示にする
    if (distance > 0) {
        $("#messageInner01 ul li.selectionItem div.deleteBntArea").hide();
    }
}

// 2012/01/23 TCS 相田 【SALES_1B】 END

//2019/03/14 NSK 山田 18PRJ02750-00_(トライ店システム評価)サービス業務における次世代オペレーション実現の為の性能対策 START
/**
* データ読み込み後処理
* @return {}
*/
function endReadData() {
    //データ読み込み終了フラグON
    gIsDataReadFlag = true;
    maskOff();
}

/**
* IFrameロード後処理
* @return {}
*/
function endLoadIFrame() {
    //IFrameロード終了フラグON
    gIsIFrameLoadFlag = true;
    maskOff();
}

/**
* データ読み込みフラグ、IFrameロードフラグを判定してマスク非表示を呼ぶ
* @return {}
*/
function maskOff() {
    if (gIsDataReadFlag && gIsIFrameLoadFlag){
        //IFrameロード、データ読み込みが終了していたらクルクル、マスク解除
        WhatsNewActiveOff();
    }
    else{
        //IFrameロード終了、データ読み込み終了が同時に来てマスクが残るのを防ぐためもう一度実施
        setTimeout(function () { maskOff(); }, 100);
    }
}

//2019/03/14 NSK 山田 18PRJ02750-00_(トライ店システム評価)サービス業務における次世代オペレーション実現の為の性能対策 END