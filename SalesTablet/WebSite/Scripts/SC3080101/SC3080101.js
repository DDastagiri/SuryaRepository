//━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
//SC3080101.js
//─────────────────────────────────────
//機能： 顧客検索一覧
//補足： 
//作成： 2011/11/18 TCS 安田
//更新： 2012/04/26 TCS 安田 HTMLエンコード対応
//更新： 2012/05/17 TCS 安田 クルクル対応
//更新： 2012/08/13 TCS 安田 【SALES_3】顧客編集より顧客検索機能追加
//─────────────────────────────────────
//初期処理
$(function () {

    //お客様名リンク押下
    $(".scCoutomerNameButton").click(function () {
        if ($("#sortTypeHidden").val() == "1") {
            if ($("#sortOrderHidden").val() == "1") {
                $("#sortOrderHidden").val("2");
            } else {
                $("#sortOrderHidden").val("1");
            }
        } else {
            $("#sortTypeHidden").val("1");
            $("#sortOrderHidden").val("1");
        }
        $("#sortButton").click();
    });

    //保有車両リンク押下
    $(".scVclregButton").click(function () {

        if ($("#sortTypeHidden").val() == "2") {
            if ($("#sortOrderHidden").val() == "1") {
                $("#sortOrderHidden").val("2");
            } else {
                $("#sortOrderHidden").val("1");
            }
        } else {
            $("#sortTypeHidden").val("2");
            $("#sortOrderHidden").val("1");
        }

        $("#sortButton").click();
    });

    //SCリンク押下
    $(".scSCButton").click(function () {
        if ($("#sortTypeHidden").val() == "3") {
            if ($("#sortOrderHidden").val() == "1") {
                $("#sortOrderHidden").val("2");
            } else {
                $("#sortOrderHidden").val("1");
            }
        } else {
            $("#sortTypeHidden").val("3");
            $("#sortOrderHidden").val("1");
        }
        $("#sortButton").click();
    });

    //SAリンク押下
    $(".scSAButton").click(function () {
        if ($("#sortTypeHidden").val() == "4") {
            if ($("#sortOrderHidden").val() == "1") {
                $("#sortOrderHidden").val("2");
            } else {
                $("#sortOrderHidden").val("1");
            }
        } else {
            $("#sortTypeHidden").val("4");
            $("#sortOrderHidden").val("1");
        }
        $("#sortButton").click();
    });

    //2012/08/13 TCS 安田 【SALES_3】顧客編集より顧客検索機能追加 START
    if ($("#backFlgHidden").val() === "1") {
        //顧客詳細画面へ戻る
        $("#backButton").click();
    }
    //2012/08/13 TCS 安田 【SALES_3】顧客編集より顧客検索機能追加 END
    //2012/08/23 TCS 山口 【A STEP2】次世代e-CRB 新車商談機能改善 START
    if ($("#updateVisitCustomerInfoFlg").val() === "1") {
        //顧客詳細画面へ
        $("#nextButton").click();
    }
    //2012/08/23 TCS 山口 【A STEP2】次世代e-CRB 新車商談機能改善 END

});

//顧客一覧で顧客選択
function selectCoustomer(updateFlg, cstkindHidden, crcustidHidden, vclHidden, staffcdHidden, columeId) {

    //更新フラグが1の場合のみ処理する。セールススタッフは、自顧客以外は遷移できなくする。
    if (updateFlg == "1") {

        //選択された行の背景色を青にする
        var $divlist = $("#" + columeId + "").children("div");
        for (i = 0; i < $divlist.length; i++) {
            $divlist[i].className = $divlist[i].className + " ColorBlue";
        }

        //2012/08/13 TCS 安田 【SALES_3】顧客編集より顧客検索機能追加 START
        //電話番号検索フラグ＝1:顧客編集の電話番号検索ボタンより遷移時
        if ($("#searchFlgHidden").val() === "1") {

            //同一メソット内では、選択行が青にならないので、タイマーで時間をずらす。
            setTimeout(function () {
                //確認メッセージを出力する
                if (confirm($("#selectConfirmHidden").val()) === true) {
                    //フラグ解除後に再起処理をして顧客詳細へ遷移させる
                    $("#searchFlgHidden").val("0");
                    selectCoustomer(updateFlg, cstkindHidden, crcustidHidden, vclHidden, staffcdHidden, columeId);
                } else {
                    //選択色を解除する
                    for (i2 = 0; i2 < $divlist.length; i2++) {
                        $($divlist[i2]).removeClass("ColorBlue");
                    }
                    return;
                }
            }, 200);

            //処理を抜ける
            return;
        }
        //2012/08/13 TCS 安田 【SALES_3】顧客編集より顧客検索機能追加 END

        $("#cstkindHidden").val(cstkindHidden);
        $("#crcustidHidden").val(crcustidHidden);
        $("#vclHidden").val(vclHidden);
        $("#salessStaffcdHidden").val(staffcdHidden);

        //オーバーレイ表示
        $("#serverProcessOverlayBlack").css("display", "block");
        //アニメーション(ロード中)
        setTimeout(function () {
            $("#serverProcessIcon").addClass("show");
            $("#serverProcessOverlayBlack").addClass("open");
        }, 0);

        //2012/05/17 TCS 安田 クルクル対応 START
        //注意) $("#nextButton").click();の前にcommonRefreshTimer処理がいる。
        //タイマーセット
        commonRefreshTimer(
            function () {
                $("#refreshButton").click();    //再表示用ボタン押下
            }
        );
        //2012/05/17 TCS 安田 クルクル対応 END


        $("#nextButton").click();
    }

}

//2012/04/26 TCS 安田 HTMLエンコード対応 START
/**
* HTMLデコードを行う
* 
* @param {String} value 
* 
*/
function SC3080101HTMLDecode(value) {
    return $("<Div>").html(value).text();
}
//2012/04/26 TCS 安田 HTMLエンコード対応 END

var prevMessage = "Previous1";
var nextMessage = "Next1";

