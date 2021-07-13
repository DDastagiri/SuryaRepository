//━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
//SC3080201.js
//─────────────────────────────────────
//機能： 顧客情報
//補足：
//作成： 2011/11/26 TCS 山口
//更新： 2012/01/26 TCS 安田 【SALES_1B】チェックマークのチェックの色（青／赤）を切り替える
//更新： 2012/01/26 TCS 安田 【SALES_1B】敬称押下 (×ボタンの表示を防ぐため)
//更新： 2012/01/26 TCS 安田 【SALES_1B】顧客編集→車両編集遷移
//更新： 2012/01/26 TCS 安田 【SALES_1B】RMM配信区分の幅を調整
//更新： 2012/03/08 TCS 河原 【SALES_1B】コールバック時の文字列のエンコード処理追加
//更新： 2012/03/27 TCS 高橋 【SALES_2】(Sales1A ユーザテスト No.163)
//更新： 2012/04/12 TCS 安田 【SALES_2】来店から車両登録が不可（ユーザー課題 No.49）
//更新： 2012/04/17 TCS 河原 【SALES_2】通知アイコンが点滅しない件の対応(ユーザーテスト課題No.17)
//更新： 2012/04/19 TCS 河原 【SALES_2】顔写真パス変更対応
//更新： 2012/05/09 TCS 河原 【SALES_1A】お客様メモエリアで左スワイプで、商談メモへ切り替えられない
//更新： 2012/05/17 TCS 安田 クルクル対応
//更新： 2012/06/01 TCS 河原 FS開発
//更新： 2012/08/23 TCS 山口 【A STEP2】次世代e-CRB 新車商談機能改善
//更新： 2013/01/22 TCS 河原 【A.STEP2】次世代e-CRB 新車タブレットSC活動支援機能開発
//更新： 2013/11/06 TCS 山田 i-CROP再構築後の新車納車システムに追加したリンク対応
//更新： 2013/11/29 TCS 市川,各務 Aカード情報相互連携開発
//更新： 2014/03/18 TCS 市川  新PF開発No.41（商業情報活動）
//更新： 2014/04/21 TCS 市川 GTMCタブレット高速化対応（BTS-386）
//更新： 2014/05/01 TCS 市川 車両PopUp不具合対応（BTS-404）
//更新： 2014/08/28 TCS 外崎 TMT NextStep2 UAT-BTS D-117
//更新： 2016/09/09 TCS 藤井 セールスタブレット性能改善
//更新： 2017/11/16 TCS 河原 TKM独自機能開発
//更新： 2018/06/18 TCS 前田 TKM Next Gen e-CRB Project Application development Block B-1
//更新： 2018/11/22 TCS 三浦 活動区分「3,4」選択時、理由を選択できるようにする(TR-SLT-TMT-20180611-001)
//更新： 2018/12/20 TCS 前田   TKM-UAT課題No.89 SuggestiveFieldからの検索時にスクロール位置を初期化する
//更新： 2019/02/13 TCS 河原 顧客編集ポップアップを欄外タップで閉じないように変更
//─────────────────────────────────────

////2016/09/09 TCS 藤井 セールスタブレット性能改善 ADD START
$(function () {
    //フッター(TCV)ボタンが活性になっている場合、実行関数【openTcv】を紐付ける 
    if ($("#MstPG_FootItem_Main_300").attr("disabled") == undefined) {

        setTimeout(function () {
            if (openTcv !== undefined) {
                $('#MstPG_FootItem_Main_300')
                .unbind('click')
                .bind('click', openTcv);
            }
        }, 0);
    }
});
//2016/09/09 TCS 藤井 セールスタブレット性能改善 ADD END

//顧客編集　START -------------------------------------------------------------------------------------

/**
* HTMLデコードを行う
* 
* @param {String} value 
* 
*/
function SC3080201HTMLDecode(value) {
    return $("<Div>").html(value).text();
}

// チェックマーク関連　-------------------------------------------------

// 2012/01/26 TCS 安田 【SALES_1B】チェックマークのチェックの色（青／赤）を切り替える START
// チェックマークのチェックの色（青／赤）を切り替える
function setCheckColor(targetElement) {

    var flg = targetElement.attr("checked");
    var wrapperElement = targetElement.parent();
    var checkElement2 = wrapperElement.children("span:nth-child(1)");

    //チェックマークの青／赤を切り替える
    if (flg == true) {
        //チェックあり　青にする
        checkElement2.addClass("scBuleCheck");
    } else {
        //チェックなし　青にする
        checkElement2.removeClass("scBuleCheck");
    }
}
// 2012/01/26 TCS 安田 【SALES_1B】チェックマークのチェックの色（青／赤）を切り替える END

// コールバック関数定義　-------------------------------------------------
var callback = {
    doCallback: function (method, argument, callbackFunction) {
        this.method = method;
        this.argument = argument;
        this.packedArgument = method + "," + argument;
        this.endCallback = callbackFunction;
        this.beginCallback();
    }
};

//2012/03/08 TCS 山口 【SALES_2】性能改善 START
//// 車両編集-更新前情報の情報を保存する
function backUpVehicleInfo() {
    $("#editVehicleModeBackHidden").val($("#editVehicleModeHidden").val());         //処理モード
}
//2012/03/08 TCS 山口 【SALES_2】性能改善 END

// 車両編集-キャンセル-変更前情報の情報に戻す
function cancelVehicleInfo() {
    $("#editVehicleModeHidden").val($("#editVehicleModeBackHidden").val());                     //処理モード
}

//2012/03/08 TCS 山口 【SALES_2】性能改善 START
// 顧客情報クリック時に顧客編集ポップアップ表示(サーバー処理前)　
function CustomerEditPopUpOpen() {
    SC3080201.requirePartialScript("../Scripts/SC3080201/SC3080201.CustomerEdit.js?20200309000000", function () {
        //共通読込みアニメーション変更
        $("#processingServer").addClass("customerEditPopupLoadingAnimation");
        $("#registOverlayBlack").addClass("popupLoadingBackgroundColor");
        //コンタクト履歴処理中イメージのz-index削除
        $("#ContactHistoryRepeater .icrop-CustomRepeater-progress").css({ "z-index": "0" });

        //顧客編集ポップアップ表示
        $("#scNscCustomerEditingWindown").fadeIn(300);

        // 2017/11/16 TCS 河原 TKM独自機能開発 START
        $("#CleansingMode").val("0");
        // 2017/11/16 TCS 河原 TKM独自機能開発 END

        //2019/02/13 TCS 河原 顧客編集ポップアップを欄外タップで閉じないように変更 START
        $("#CustomerEditOverlayBlack").css("display", "block");
        //2019/02/13 TCS 河原 顧客編集ポップアップを欄外タップで閉じないように変更 END

        setTimeout(function () {
            //サーバー処理実行
            $("#CustomerEditPopupOpenButton").click();
        }, 300);
    });
}

//2012/03/08 TCS 山口 【SALES_2】性能改善 START
// 車両情報クリック時に車両編集ポップアップ表示(サーバー処理前)　
function CustomerCarEditPopUpOpen() {
    SC3080201.requirePartialScript("../Scripts/SC3080201/SC3080201.CustomerCarEdit.js?20181122000000", function () {
        //共通読込みアニメーション変更
        $("#processingServer").addClass("carEditPopupLoadingAnimation");
        $("#registOverlayBlack").addClass("popupLoadingBackgroundColor");
        //コンタクト履歴処理中イメージのz-index削除
        $("#ContactHistoryRepeater .icrop-CustomRepeater-progress").css({ "z-index": "0" });

        //車両編集ポップアップ表示
        $("#scVehicleEditingWindown").fadeIn(300);

        setTimeout(function () {
            //サーバー処理実行
            $("#CustomerCarEditPopupOpenButton").click();
        }, 300);
    });
}

//顧客編集（追加）ポップアップ呼出用
$(function () {
    //新規顧客時に自動起動
    if ($("#customerEditPopUpAutoOpenFlg").val() == "1") {
        CustomerEditPopUpOpen();
    }
    //新規顧客登録→車両編集を自動起動
    if ($("#vehiclePopUpAutoOpenFlg").val() == "1") {
        //車両編集表示
        CustomerCarEditPopUpOpen();
    }
});

//顧客編集　END -------------------------------------------------------------------------------------


//車両編集ポップアップ関連/////////////////////
//2012/03/08 TCS 山口 【SALES_2】性能改善 START
//車両情報再表示用
//function CustomerCarEditPopUpClose() {
//    $("#customerCarReload").click();
//}
//車両情報再表示用
function CustomerCarAreaReload() {
    $("#customerCarReload").click();
}

//保有車両選択ポップアップ処理(サーバー処理前)
function CustomerCarSelectPopUpOpen() {
    ////コンタクト履歴リロード中はタップ不可
    //if ($("#reloadFlg").val() == "1") return;

    //共通読込みアニメーション変更
    $("#processingServer").addClass("carSelectPopupLoadingAnimation");
    $("#registOverlayBlack").addClass("popupLoadingBackgroundColor");
    //コンタクト履歴処理中イメージのz-index削除
    $("#ContactHistoryRepeater .icrop-CustomRepeater-progress").css({ "z-index": "0" });

    //$(".scNscSelectionListBox").fingerScroll();
    //ポップアップ表示(枠のみ)
    $("#scNscSelectionWindownVehicleSelect").fadeIn(300);

    setTimeout(function () {
        //サーバー処理実行
        $("#CustomerCarSelectPopupOpenButton").click();
    }, 300);
}

//保有車両選択ポップアップ表示処理(サーバー処理後)
function CustomerCarSelectPopUpOpenAfter() {
    $(".scNscSelectionListBox").fingerScroll();

    //ポップアップ表示
    $("#scNscSelectionWindownVehicleSelect").fadeIn(0);

    //共通読込みアニメーション戻し
    $("#processingServer").removeClass("carSelectPopupLoadingAnimation");
    $("#registOverlayBlack").removeClass("popupLoadingBackgroundColor");
}
//保有車両選択ポップアップ非表示処理
function CustomerCarSelectPopupClose() {
    //ポップアップ非表示
    $("#scNscSelectionWindownVehicleSelect").fadeOut(300);
    setTimeout(function () {
        //HTML削除
        $("#CustomerCarVisiblePanel").empty();
    }, 300);
}
//2012/03/08 TCS 山口 【SALES_2】性能改善 END


function selectCarTypeClick(val) {
    for (i = 1; i <= parseInt($("#CustomerCarTypeNumberLabel").text()); i++) {
        if (i == val) {
            $("#customerCarsSelectedHiddenField").val($("#customerCarKey" + parseInt(i)).val());
        }
    }
    $("#customerCarButtonDummy").click();
    $("#scNscSelectionWindownVehicleSelect").fadeOut(300);

    //2013/01/22 TCS 河原 【A.STEP2】次世代e-CRB 新車タブレットSC活動支援機能開発 START
    //現在選択中のコンタクト履歴のタブがサービスの場合読み込みなおす
    if ($(".scNscCurriculumTabServiceOff").size() == 0) {
        $("#TabService").click();
    }
    //2013/01/22 TCS 河原 【A.STEP2】次世代e-CRB 新車タブレットSC活動支援機能開発 END

    return true;
}

//顧客メモポップアップ処理/////////////////////////
function setPopupCustomerMemoOpen() {
    //2012/05/09 TCS 河原 【SALES_1A】お客様メモエリアで左スワイプで、商談メモへ切り替えられない START
    if ($("#CustomerMemoDummyAreaFlg").size() > 0) {
        if (this_form.CustomerMemoDummyAreaFlg.value == "0") {
            //セッションを設定する
            $("#CustomerMemoEditOpenButton").click();
        }
    }
    //2012/05/09 TCS 河原 【SALES_1A】お客様メモエリアで左スワイプで、商談メモへ切り替えられない END
}

function commitCompleteOpenCustomerMemoEdit() {
    //    //Iframe削除
    //    $("#CustomerMemoIframe").remove();
    //    //Iframe作成
    //    var $iframe = $("<iframe id='CustomerMemoIframe' src='./SC3080204.aspx' width='1014px' height='645px' scrolling='no' frameborder='0' style='border:2px solid #666'></iframe>");
    //    //タグ追加
    //    $("#CustomerMemoEdit").append($iframe);

    //顧客メモをスライドインする
    $("#CustomerMemoEdit").fadeIn(300);

    //先頭のメモを選択状態にする
    SelectFirstMemo();

    //$("#CustomerMemoEdit").PageLoad();

}
////////////////////////////////////////////////////


//職業ポップアップ処理

//職業ポップアップ表示処理(サーバー処理前)
function setPopupOccupationPageOpen() {
    SC3080201.requirePartialScript("../Scripts/SC3080201/SC3080201.Occupation.js", function () {
        //共通読込みアニメーション変更
        $("#processingServer").addClass("occupationPopupLoadingAnimation");
        $("#registOverlayBlack").addClass("popupLoadingBackgroundColor");

        //ポップアップ表示(枠のみ)
        $("#CustomerRelatedOccupationPopupArea").fadeIn(300);

        //--2013/11/29 TCS 市川 Aカード情報相互連携開発 START
        //アニメーション位置調整(表示後のみ可能)
        $("#processingServer").css("top", ($("#OccupationPopopBody").attr("offsetHeight") / 2 + $("#OccupationPopopBody").offset().top - 19) + "px");
        //--2013/11/29 TCS 市川 Aカード情報相互連携開発 END

        setTimeout(function () {
            //サーバー処理実行
            $("#OccupationOpenButton").click();
        }, 300);
    });
}

//GoogleMapポップアップ処理
function googleMapOpen() {
    //GoogleMap
    //    var posX = 450;
    //    var posY = 200;
    //    var width = 520;
    //    var height = 660;

    var ArrowDir = 1;
    var posX = 490;
    var posY = 220;
    var width = 500;
    var height = 657;
    //    var width = 500;
    //    var height = 640;
    var address = $("#customerAddressTextBox").val();

    if (address == "-") {
        return;
    }

    var query = "";
    query += "icrop:pmap";
    query += ":" + ArrowDir + ":";
    query += ":" + posX + ":";
    query += ":" + posY + ":";
    query += ":" + width + ":";
    query += ":" + height + ":";
    query += ":" + address;

    location.href = query;
}

//顔写真ポップアップ処理
function photoSelectOpen() {
    //Photo
    var posX = 80;
    var posY = 150;
    //    var file = $("#customerIdTextBox").val() + $("#faceFileNameTimeHiddenField").val();
    var file = $("#customerIdTextBox").val();
    var cbmethod = "CallBackCustomerPhoto";
    //2012/04/19 TCS 河原 【SALES_2】顔写真パス変更対応 START
    var path = this_form.FacePicUploadPath.value;
    //2012/04/19 TCS 河原 【SALES_2】顔写真パス変更対応 END

    var query = "";
    query += "icrop:came"
    query += ":" + posX + ":";
    query += ":" + posY + ":";
    //2012/04/19 TCS 河原 【SALES_2】顔写真パス変更対応 START
    //query += ":" + file + ":";
    query += ":" + path + file + ":";
    //2012/04/19 TCS 河原 【SALES_2】顔写真パス変更対応 END
    query += ":" + cbmethod;

    location.href = query;
}

//初期設定
$(function () {

    bindFingerScroll();

    $(".scNscSelectionListBox").fingerScroll();

    //--2013/11/29 TCS 市川 Aカード情報相互連携開発 START
    //職業ポップアップスクロール設定
    $("#occupationPopOverForm_1").fingerScroll();
    //趣味ポップアップスクロール設定
    $("#CustomerRelatedHobbyPopupPage1").fingerScroll();
    //--2013/11/29 TCS 市川 Aカード情報相互連携開発 END

    //ポップアップクローズの監視

    //車両選択ポップアップ
    $(document.body).bind("mousedown touchstart", function (e) {
        if ($("#scNscSelectionWindownVehicleSelect").is(":visible") === false) return;
        //2012/03/08 TCS 山口 【SALES_2】性能改善 START
        if ($("#registOverlayBlack").hasClass("popupLoadingBackgroundColor") === true) return;
        //2012/03/08 TCS 山口 【SALES_2】性能改善 END
        if ($(e.target).is("#scNscSelectionWindownVehicleSelect, #scNscSelectionWindownVehicleSelect *") === false) {
            //2012/03/08 TCS 山口 【SALES_2】性能改善 START
            //画面初期化
            CustomerCarSelectPopupClose();
            //$("#scNscSelectionWindownVehicleSelect").fadeOut(300);
            //2012/03/08 TCS 山口 【SALES_2】性能改善 END
        }
    });

    /* 2012/06/01 TCS 河原 FS開発 START */
    $(document.body).bind("touchstart", function (e) {
        if ($(e.target).is("#SnsIdInputPopup *") === false) {
            //画面初期化
            SnsIdInputPopupClose();
        }
    });

    $(document.body).bind("touchstart", function (e) {
        if ($(e.target).is("#KeywordSearchInputPopup *") === false) {
            //画面初期化
            KeywordSearchInputPopupClose();
        }
    });
    /* 2012/06/01 TCS 河原 FS開発 END */

});

function bindFingerScroll() {
    $("#CustomerRelatedFamilyPageArea .popupScrollArea").fingerScroll();
}

function CallBackCustomerPhoto(iRc) {
    if (iRc == 1) {
        $("#customerInfoUpdateButton").click();
    }
}

function RefleshCustomerInfo() {
    $("#customerInfoUpdateButton").click();
}

//家族ポップアップ処理

//家族ポップアップ表示処理(サーバー処理前)
function setPopupFamilyPageOpen() {
    SC3080201.requirePartialScript("../Scripts/SC3080201/SC3080201.Family.js", function () {
        //コンタクト履歴リロード中はタップ不可
        //if ($("#reloadFlg").val() == "1") return;

        //共通読込みアニメーション変更
        $("#processingServer").addClass("familyPopupLoadingAnimation");
        $("#registOverlayBlack").addClass("popupLoadingBackgroundColor");

        //ポップアップ表示(枠のみ)
        $("#CustomerRelatedFamilyPopupArea").fadeIn(300);

        setTimeout(function () {
            //サーバー処理実行
            $("#FamilyOpenButton").click();
        }, 300);
    });
}

//趣味関連
var g_hobbyPage = "page1";
var g_hobyRow = -1;

//趣味ポップアップ表示処理(サーバー処理前)
function setPopupHobbyPageOpen() {
    SC3080201.requirePartialScript("../Scripts/SC3080201/SC3080201.Hobby.js", function () {
        //共通読込みアニメーション変更
        $("#processingServer").addClass("hobbyPopupLoadingAnimation");
        $("#registOverlayBlack").addClass("popupLoadingBackgroundColor");

        //ポップアップ表示(枠のみ)
        $("#CustomerRelatedHobbyPopupArea").fadeIn(300);

        //--2013/11/29 TCS 市川 Aカード情報相互連携開発 START
        //アニメーション位置調整(表示後のみ可能)
        $("#processingServer").css("top", ($("#HobbyPopupBody").attr("offsetHeight") / 2 + $("#HobbyPopupBody").offset().top - 19) + "px");
        //--2013/11/29 TCS 市川 Aカード情報相互連携開発 END

        setTimeout(function () {
            //サーバー処理実行
            $("#HobbyOpenButton").click();
        }, 300);
    });
}

//連絡方法関連

//連絡方法ポップアップ表示処理(サーバー処理前)
function setPopupContactPageOpen() {
    SC3080201.requirePartialScript("../Scripts/SC3080201/SC3080201.Contact.js", function () {
        //共通読込みアニメーション変更
        $("#processingServer").addClass("contactPopupLoadingAnimation");
        $("#registOverlayBlack").addClass("popupLoadingBackgroundColor");

        //ポップアップ表示(枠のみ)
        $("#CustomerRelatedContactPopupArea").fadeIn(300);

        setTimeout(function () {
            //サーバー処理実行
            $("#ContactOpenButton").click();
        }, 300);
    });
}

function reloadMemo() {
    $("#CustomerMemoEdit").fadeOut(300);
    $("#CustomerMemoEditCloseButton").click();
}

//2012/02/15 TCS 山口 【SALES_2】 START
//CSServey
function CSSurveyClick() {
    //コンタクト履歴処理中イメージのz-index削除
    $("#ContactHistoryRepeater .icrop-CustomRepeater-progress").css({ "z-index": "0" });
    //    $("#CSserveyOpenButton").click();
}

//2012/03/08 TCS 山口 【SALES_2】性能改善 START
//コンタクト履歴ロード中処理
var prm = Sys.WebForms.PageRequestManager.getInstance()
prm.add_endRequest(ContactHistoryReload)
function ContactHistoryReload(sender, arg) {
    //ロード中にUpdatePanelでのリクエストが発生した場合、コンタクト履歴を再検索
    if ($("#reloadFlg").val() == "1") {
        //リロード中フラグON
        $("#reloadFlg").val("1");
        //コンタクト履歴再検索
        $("#ContactHistoryRepeater").CustomRepeater("reload", $("#ContactHistoryTabIndex").val());
    }
}
//2012/03/08 TCS 山口 【SALES_2】性能改善 START

$(function () {
    $(document.body).bind("mousedown touchstart", function (e) {
        if ($("#ImportantContactLeftAreaOpenFlg").val() == "0") return;
        if ($(e.target).is(".nsc40-02BoxLeft, .nsc40-02BoxLeft *") === false) {
            //重要連絡表示枠縮小
            importantContactClose();
        }
    });

    //fingerScroll
    $(".nsc40-02BoxLeft .textBox02").fingerScroll();

    //コンタクト履歴タブ初期設定
    ContactHistoryTabOff("0");
    ContactHistoryTabOn($("#ContactHistoryTabIndex").val());

    //コンタクト履歴タブ選択イベント
    $("#TabAll").click(function () {
        if ($("#ReadOnlyFlagHidden").val() == "1") return;

        //リロード中はタップ不可
        if ($("#reloadFlg").val() == "1") return;
        //リロード中フラグON
        $("#reloadFlg").val("1");

        //タブ切り替え
        ContactHistoryTabOff($("#ContactHistoryTabIndex").val());
        $("#ContactHistoryTabIndex").val("0");
        ContactHistoryTabOn($("#ContactHistoryTabIndex").val());

        //条件全てで再検索
        $("#ContactHistoryRepeater").CustomRepeater("reload", $("#ContactHistoryTabIndex").val());

    });
    $("#TabSales").click(function () {
        if ($("#ReadOnlyFlagHidden").val() == "1") return;

        //リロード中はタップ不可
        if ($("#reloadFlg").val() == "1") return;
        //リロード中フラグON
        $("#reloadFlg").val("1");

        //タブ切り替え
        ContactHistoryTabOff($("#ContactHistoryTabIndex").val());
        $("#ContactHistoryTabIndex").val("1");
        ContactHistoryTabOn($("#ContactHistoryTabIndex").val());

        //条件セールスで再検索
        $("#ContactHistoryRepeater").CustomRepeater("reload", $("#ContactHistoryTabIndex").val());
    });
    //更新： 2013/01/22 TCS 河原 【A.STEP2】次世代e-CRB 新車タブレットSC活動支援機能開発 START
    $("#TabService").click(function () {
        if ($("#ReadOnlyFlagHidden").val() == "1") return;

        //リロード中はタップ不可
        if ($("#reloadFlg").val() == "1") return;
        //リロード中フラグON
        $("#reloadFlg").val("1");

        //タブ切り替え
        ContactHistoryTabOff($("#ContactHistoryTabIndex").val());
        $("#ContactHistoryTabIndex").val("2");
        ContactHistoryTabOn($("#ContactHistoryTabIndex").val());

        //条件CRで再検索
        $("#ContactHistoryRepeater").CustomRepeater("reload", $("#ContactHistoryTabIndex").val());
        //        
    });
    //更新： 2013/01/22 TCS 河原 【A.STEP2】次世代e-CRB 新車タブレットSC活動支援機能開発 END
    $("#TabCr").click(function () {
        if ($("#ReadOnlyFlagHidden").val() == "1") return;

        //リロード中はタップ不可
        if ($("#reloadFlg").val() == "1") return;
        //リロード中フラグON
        $("#reloadFlg").val("1");

        //タブ切り替え
        ContactHistoryTabOff($("#ContactHistoryTabIndex").val());
        $("#ContactHistoryTabIndex").val("3");
        ContactHistoryTabOn($("#ContactHistoryTabIndex").val());

        //条件CRで再検索
        $("#ContactHistoryRepeater").CustomRepeater("reload", $("#ContactHistoryTabIndex").val());
        //        
    });

    //更新： 2013/01/22 TCS 河原 【A.STEP2】次世代e-CRB 新車タブレットSC活動支援機能開発 START
    //コンタクト履歴タップ
    $(".scNscCurriculumListBox li").live("click", function (e) {
        if ($(this).attr("actualKind") == "3" || $(this).attr("actualKind") == "2") {
            if ($(this).hasClass("open") === true) {
                ContactHistoryRowClose(this);
            } else {
                ContactHistoryRowOpen(this);
            }
        }
    });
    //更新： 2013/01/22 TCS 河原 【A.STEP2】次世代e-CRB 新車タブレットSC活動支援機能開発 END
});

//更新： 2013/01/22 TCS 河原 【A.STEP2】次世代e-CRB 新車タブレットSC活動支援機能開発 START
//コンタクト履歴行Open
function ContactHistoryRowOpen(key) {
    //高さ計算
    var openHeight = $(key).outerHeight() + Math.max($(key).find("div.contactHistoryCrArea").height() + 5)

    if ($(key).attr("actualKind") == "3" || $(key).attr("actualKind") == "2") {
        $(key).css({
            "-webkit-transition": "200ms linear",
            "height": openHeight + "px"
        }).one("webkitTransitionEnd", function () {
            $(key).css({ "-webkit-transition": "none" });
            $(key).addClass("open");
        });
    }
}
//コンタクト履歴行Close
function ContactHistoryRowClose(key) {
    $(key).css({
        "-webkit-transition": "200ms linear",
        "height": "40px"
    }).one("webkitTransitionEnd", function () {
        $(key).removeAttr("style");
        $(key).removeClass("open");
    });
}
//更新： 2013/01/22 TCS 河原 【A.STEP2】次世代e-CRB 新車タブレットSC活動支援機能開発 END

//更新： 2013/01/22 TCS 河原 【A.STEP2】次世代e-CRB 新車タブレットSC活動支援機能開発 START
//コンタクト履歴タブ制御
function ContactHistoryTabOn(key) {
    switch (key) {
        case "0":
            $("#TabAll").removeClass("scNscCurriculumTabSalesOff");
            $("#TabAll").addClass("scNscCurriculumTabAllAc");
            break;
        case "1":
            $("#TabSales").removeClass("scNscCurriculumTabSalesOff");
            $("#TabSales").addClass("scNscCurriculumTabAllAc");
            $("#imageSales").attr("src", "../Styles/Images/SC3080201/nsc40icn11.png");
            break;
        case "2":
            $("#TabService").removeClass("scNscCurriculumTabServiceOff");
            $("#TabService").addClass("scNscCurriculumTabAllAc");
            $("#imageService").attr("src", "../Styles/Images/SC3080201/contact_service_on.png");
            break;
        case "3":
            $("#TabCr").removeClass("scNscCurriculumTabCrOff");
            $("#TabCr").addClass("scNscCurriculumTabAllAc");
            $("#imageCr").attr("src", "../Styles/Images/SC3080201/ico115b.png");
            break;
    }
}
function ContactHistoryTabOff(key) {
    switch (key) {
        case "0":
            $("#TabAll").removeClass("scNscCurriculumTabAllAc");
            $("#TabAll").addClass("scNscCurriculumTabSalesOff");
            break;
        case "1":
            $("#TabSales").removeClass("scNscCurriculumTabAllAc");
            $("#TabSales").addClass("scNscCurriculumTabSalesOff");
            $("#imageSales").attr("src", "../Styles/Images/SC3080201/ico113.png");
            break;
        case "2":
            $("#TabService").removeClass("scNscCurriculumTabAllAc");
            $("#TabService").addClass("scNscCurriculumTabServiceOff");
            $("#imageService").attr("src", "../Styles/Images/SC3080201/contact_service_off.png");
            break;
        case "3":
            $("#TabCr").removeClass("scNscCurriculumTabAllAc");
            $("#TabCr").addClass("scNscCurriculumTabCrOff");
            $("#imageCr").attr("src", "../Styles/Images/SC3080201/ico115.png");
            break;
    }
}
//更新： 2013/01/22 TCS 河原 【A.STEP2】次世代e-CRB 新車タブレットSC活動支援機能開発 END



//重要連絡
function importantContactOpen(size) {
    $(".arrowOpen").hide();
    $(".arrowClose").hide();

    if (size == 1) {
        //表示枠拡大
        $("#ComplaintOverviewLabel").removeClass("ellipsis");
        $("#ComplaintOverviewLabel").removeAttr("width");
        $("#ComplaintDetailLabel").removeClass("ellipsis");
        $("#ComplaintDetailLabel").removeAttr("width");
        $(".nsc40-02BoxLeft").css({ "border-bottom-right-radius": "6px" });
        $(".nsc40-02BoxLeft").css({
            "-webkit-transition": "300ms linear",
            "height": "240px"
        }).one("webkitTransitionEnd", function () {
            $(".nsc40-02BoxLeft").css({ "-webkit-transition": "none" });
            $(".arrowClose").show();
            //開閉フラグON
            $("#ImportantContactLeftAreaOpenFlg").val("1");
            //ポップアップ系ボタン制御
            $("#messageWinPopupBlack").addClass("open");

        });
    } else {
        //表示枠縮小
        importantContactClose();
    }

}
function importantContactClose() {
    //表示枠縮小
    $(".nsc40-02BoxLeft").css({
        "-webkit-transition": "300ms linear",
        "height": "69px"
    }).one("webkitTransitionEnd", function () {
        $(".nsc40-02BoxLeft").css({ "-webkit-transition": "none" });
        $(".arrowOpen").show();
        $("#ComplaintOverviewLabel").addClass("ellipsis");
        $("#ComplaintOverviewLabel").attr("width", "300px");
        $("#ComplaintDetailLabel").addClass("ellipsis");
        $("#ComplaintDetailLabel").attr("width", "300px");
        $(".nsc40-02BoxLeft").css({ "border-bottom-right-radius": "0px" });
        //FingerScroll初期化
        $(".textBox02 .scroll-inner").css({ "transform": "translate3d(0px, 0px, 0px)" });
        //開閉フラグOFF
        $("#ImportantContactLeftAreaOpenFlg").val("0");
        setTimeout(function () {
            //ポップアップ系ボタン制御
            $("#messageWinPopupBlack").removeClass("open");
        }, 300);
    });
}
//2012/02/15 TCS 山口 【SALES_2】 END

//2012/06/01 TCS 河原 FS開発 START
//SNSサイトボタンタッチイベント
$(function () {
    $(".Sns_Icon")
		.live("mousedown touchstart", function (event) {
		    this_form.SnsOpenFlg.value = "1"
		    var taskTarget = $(this);
		    taskTarget.data("tapHold", setTimeout(function () {
		        clearTimeout($(this).data("tapHold"));
		        $(this).data("tapHold", null);
		        this_form.SnsOpenFlg.value = "0"
		        if (taskTarget.attr("id") == "Icon_Renren") {
		            $("#SnsIdInputPopup").css("left", "178px");
		            $("#SnsIdInputPopup").css("left", "236px");
		            this_form.SnsIdInputPopupInputText.value = this_form.Snsid_Renren_Hidden.value;
		            this_form.SnsOpenMode.value = "1";
		            $("#Title_Renren").css("display", "block");
		            $("#Title_Kaixin").css("display", "none");
		            $("#Title_Weibo").css("display", "none");

		        } else if (taskTarget.attr("id") == "Icon_Kaixin") {
		            $("#SnsIdInputPopup").css("left", "236px");
		            $("#SnsIdInputPopup").css("left", "178px");
		            this_form.SnsIdInputPopupInputText.value = this_form.Snsid_Kaixin_Hidden.value;
		            this_form.SnsOpenMode.value = "2";
		            $("#Title_Renren").css("display", "none");
		            $("#Title_Kaixin").css("display", "block");
		            $("#Title_Weibo").css("display", "none");
		        }
		        else if (taskTarget.attr("id") == "Icon_Weibo") {
		            $("#SnsIdInputPopup").css("left", "294px");
		            this_form.SnsIdInputPopupInputText.value = this_form.Snsid_Weibo_Hidden.value;
		            $("#Title_Renren").css("display", "none");
		            $("#Title_Kaixin").css("display", "none");
		            $("#Title_Weibo").css("display", "block");
		            this_form.SnsOpenMode.value = "3";
		        }
		        if (this_form.ReadOnlyFlagHidden.value == "0" && this_form.MoveFlg.value == "0") {
		            $("#SnsIdInputPopup").fadeIn(300);
		        }

		    }, 1000));
		})
	    .live("mouseup touchend", function (event) {
	        if ($(this).data("tapHold")) {
	            clearTimeout($(this).data("tapHold"));
	            $(this).data("tapHold", null);
	            if (this_form.SnsOpenFlg.value == "1") {
	                var url;
	                if ($(this).attr("id") == "Icon_Renren") {
	                    //renrenを表示
	                    if (this_form.Snsid_Renren_Hidden.value == " ") {
	                        url = this_form.Snsurl_Search_Renren_Hidden.value
	                    } else {
	                        url = this_form.Snsurl_Account_Renren_Hidden.value
	                        url = url.replace("{0}", encodeURI(this_form.Snsid_Renren_Hidden.value));
	                    }
	                } else if ($(this).attr("id") == "Icon_Kaixin") {
	                    //kaixinを表示
	                    if (this_form.Snsid_Kaixin_Hidden.value == " ") {
	                        url = this_form.Snsurl_Search_Kaixin_Hidden.value
	                    } else {
	                        url = this_form.Snsurl_Account_Kaixin_Hidden.value
	                        url = url.replace("{0}", encodeURI(this_form.Snsid_Kaixin_Hidden.value));
	                    }
	                }
	                else if ($(this).attr("id") == "Icon_Weibo") {
	                    //weiboを表示
	                    if (this_form.Snsid_Weibo_Hidden.value == " ") {
	                        url = this_form.Snsurl_Search_Weibo_Hidden.value
	                    } else {
	                        url = this_form.Snsurl_Account_Weibo_Hidden.value
	                        url = url.replace("{0}", encodeURI(this_form.Snsid_Weibo_Hidden.value));
	                    }
	                }
	                if (this_form.ReadOnlyFlagHidden.value == "0" && this_form.MoveFlg.value == "0") {
	                    url = url_Scheme(url)
	                    location.href = url;
	                }
	            }
	        }
	    });
});

//SNSID登録ポップアップクローズ処理
function SnsIdInputPopupClose() {
    $("#SnsIdInputPopup").fadeOut(300);
}

//キーワード検索ボタンタッチイベント
$(function () {
    $("#KeywordSearch")
		.bind("mousedown touchstart", function (event) {
		    this_form.KeywordSearchOpenFlg.value = "1"
		    var taskTarget = $(this);
		    taskTarget.data("tapHold", setTimeout(function () {
		        clearTimeout($(this).data("tapHold"));
		        $(this).data("tapHold", null);
		        this_form.KeywordSearchOpenFlg.value = "0"
		        this_form.KeywordSearchInputPopupInputText.value = this_form.Keyword_Hidden.value;
		        if (this_form.ReadOnlyFlagHidden.value == "0" && this_form.MoveFlg.value == "0") {
		            $("#KeywordSearchInputPopup").fadeIn(300);
		        }
		    }, 1000));
		})
	    .bind("mouseup touchend", function (event) {
	        if ($(this).data("tapHold")) {
	            clearTimeout($(this).data("tapHold"));
	            $(this).data("tapHold", null);
	            if (this_form.KeywordSearchOpenFlg.value == "1") {
	                var url;
	                url = this_form.Search_Baidu_Hidden.value
	                url = url.replace("{0}", encodeURI(this_form.Keyword_Hidden.value));
	                if (this_form.ReadOnlyFlagHidden.value == "0" && this_form.MoveFlg.value == "0") {
	                    url = url_Scheme(url)
	                    location.href = url;
	                }
	            }
	        }
	    });
});

//キーワード登録ポップアップクローズ処理
function KeywordSearchInputPopupClose() {
    $("#KeywordSearchInputPopup").fadeOut(300);
}

//顧客登録以前はキーワード検索ボタンをデザイン変更する
$(function () {
    var cstid = this_form.customerIdTextBox.value;
    if (cstid == "") {
        $("#KeywordSearch").addClass("buttonOff");
        $("#KeywordSearch").removeClass("buttonOn");
    }
});

//ボタンが非活性の場合、デザインを変更する
$(function () {
    if (this_form.ReadOnlyFlagHidden.value == "1") {
        $("#KeywordSearch").addClass("buttonOff");
        $("#KeywordSearch").removeClass("buttonOn");
    }
});

//クルクル対応
$(function () {
    //キーワード登録ボタン
    $("#KeywordSearchPopUpCompleteButton").live("click", function () {
        commonRefreshTimer();
    });
    //SNSID登録ボタン
    $("#SnsIdPopUpCompleteButton").live("click", function () {
        commonRefreshTimer();
    });
});

//URLスキーム置き換え
function url_Scheme(url) {
    url = url.replace("http://", this_form.Url_Scheme_Hidden.value + "://");
    url = url.replace("https://", this_form.Url_Schemes_Hidden.value + "://");
    return url
}
//2012/06/01 TCS 河原 FS開発 END

//2013/11/06 TCS 山田 ADD i-CROP再構築後の新車納車システムに追加したリンク対応 START
/**
* 新車納車システムリンクメニュー押下時の処理
* @param {String} url リンク先URL(URLスキーマ置き換え済み)
*/
function linkMenu(url) {
    location.href = url;
    return false;
}
//2013/11/06 TCS 山田 ADD i-CROP再構築後の新車納車システムに追加したリンク対応 END

//2014/05/01 TCS 市川 車両PopUp不具合対応（BTS-404）START
//活動区分は自社客：車両編集/未取引客：顧客編集 にて表示されるため、共通利用初期化関数は遅延ロードしない。
// 活動区分 選択状態にする　-------------------------------------------------
function actvctgrylist(parentTag) {

    // 現在選択を解除する
    $(parentTag + " " + ".actvctgrylist").removeClass("Selection");

    var cdhidden = $("#actvctgryidHidden").val();

    var $list2 = $(parentTag + " " + " ul.actvctgryListBoxSetIn").children();

    for (i = 0; i < $list2.length; i++) {
        var cd = $(parentTag + " " + "#" + $list2[i].id + "").children(".actvctgryHidden").text();

        //選択状態にする
        if (cd == cdhidden) {
            $(parentTag + " " + "#" + $list2[i].id + "").addClass("Selection");
        }
    }

    $(parentTag + " " + "#" + $list2[$list2.length - 1].id + "").css("border-bottom", "none");

    return true;
}

// 断念理由 選択状態にする　-------------------------------------------------
function reasonidlist(parentTag) {

    // 現在選択を解除する
    $(parentTag + " " + ".reasonlist").removeClass("Selection");

    //2018/11/22 TCS 三浦 活動区分「3,4」選択時、理由を選択できるようにする(TR-SLT-TMT-20180611-001) START
    var cdhidden = $("#actvctgryidHidden").val() + "-" + $("#reasonidHidden").val();
    //2018/11/22 TCS 三浦 活動区分「3,4」選択時、理由を選択できるようにする(TR-SLT-TMT-20180611-001) END

    var $list2 = $(parentTag + " " + " ul.reasonListBoxSetIn").children();

    for (i = 0; i < $list2.length; i++) {
        //2018/11/22 TCS 三浦 活動区分「3,4」選択時、理由を選択できるようにする(TR-SLT-TMT-20180611-001) START
        var cd = $(parentTag + " " + "#" + $list2[i].id + "").children(".actvctgryidHidden").text() + "-" + $(parentTag + " " + "#" + $list2[i].id + "").children(".reasoncdHidden").text();
        //2018/11/22 TCS 三浦 活動区分「3,4」選択時、理由を選択できるようにする(TR-SLT-TMT-20180611-001) END

        //選択状態にする
        if (cd == cdhidden) {
            $(parentTag + " " + "#" + $list2[i].id + "").addClass("Selection");
        }
    }

    $(parentTag + " " + "#" + $list2[$list2.length - 1].id + "").css("border-bottom", "none");

    return true;
}
//2014/05/01 TCS 市川 車両PopUp不具合対応（BTS-404）END

//2018/06/18 TCS 前田 TKM Next Gen e-CRB Project Application development Block B-1 START
// 年式 選択状態にする　-------------------------------------------------
function modelyearlist(parentTag) {

    // 現在選択を解除する
    $(parentTag + " " + ".modelyearlist").removeClass("Selection");

    var cdhidden = $("#modelYearHidden").val();

    var $list2 = $(parentTag + " " + " ul.modelYearListBoxSetIn").children();

    for (i = 0; i < $list2.length; i++) {
        var cd = $(parentTag + " " + "#" + $list2[i].id + "").children(".modelYearCdHidden").text();

        //選択状態にする
        if (cd == cdhidden) {
            $(parentTag + " " + "#" + $list2[i].id + "").addClass("Selection");
        }
    }

    $(parentTag + " " + "#" + $list2[$list2.length - 1].id + "").css("border-bottom", "none");

    return true;
}
//2018/06/18 TCS 前田 TKM Next Gen e-CRB Project Application development Block B-1 END

