/*━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
SC3070210.js
─────────────────────────────────────
'機能： 相談履歴
'補足： 
'作成： 2015/03/17 TCS 鈴木  次世代e-CRB 価格相談履歴参照機能開発
─────────────────────────────────────*/
var SC3070210 = {};

$(function () {
    SC3070210.reload = function (showAll) {
        var eventArgument = "",
            prm = Sys.WebForms.PageRequestManager.getInstance(),
            autoScrollHandler;


        if (showAll === undefined) {
            eventArgument = $("#SC3070210_IsShowingAll").val();
        } else {
            eventArgument = showAll ? "True" : "False";

            autoScrollHandler = function () {
                prm.remove_endRequest(autoScrollHandler);
               	$("#tcvNcv50Main").animate({ scrollTop: 660 + $(".SC3070210_Pager").offset().top });
            };

            prm.add_endRequest(autoScrollHandler);
        }

        $("#SC3070210_IsShowingAll").val(eventArgument);
        $("#SC3070210_ProcessingBlock").show();


        prm.beginAsyncPostBack(["SC3070210_UpdateArea"], "reload", eventArgument, false);

        return true;
    };

    icropScript.ui.setNoticeHandler("SC3070210.reload", function (type) {
        SC3070210.reload();
    });
});
