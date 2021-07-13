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

});

//顧客一覧で顧客選択
function selectCoustomer(updateFlg, cstkindHidden, crcustidHidden, vclHidden, staffcdHidden, columeId) {

    //更新フラグが1の場合のみ処理する。セールススタッフは、自顧客以外は遷移できなくする。
    if (updateFlg == "1") {
    
        var $divlist = $("#" + columeId + "").children("div");
        for (i = 0; i < $divlist.length; i++) {
            $divlist[i].className = $divlist[i].className + " ColorBlue";
        }
        
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

        $("#nextButton").click();
    }

}

var prevMessage = "Previous1";
var nextMessage = "Next1";

