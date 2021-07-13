/** 
 * @fileOverview SC3110101 ヘッダ部分の処理
 * 
 * @author shota akiyama
 * @version 1.0.0
 */

/*
 * 受付の権限コード
 * @return {int}
 *
 */
var C_OPERATIONCODE = 51;
var gPopOverFrom = null;


/* ポップアップを表示する
 * 
 * @param {Object} pop
 * @param {Object} index
 * @param {Object} args
 * @param {Object} container
 * @param {Object} header
 * 
 */
function TestDrivePopOverForm_render(pop, index, args, container, header) {

    gPopOverFrom = pop;

    page = container.children("#Panel_SC3110101");
    page = $("#Panel_SC3110101").css("display", "block");
    container.empty().append(page);

    // 受付の場合は表示する
    if ($("#opeCd").val() == C_OPERATIONCODE) {

        header.attr("style", "display:block;");
    } else {

        header.attr("style", "display:none;");
    }

    // ヘッダ部分の表示
    var headerTitle = header.find(".icrop-PopOverForm-header-title");
    headerTitle.text($("#wordTitle").val());
    headerTitle.addClass("PopupTitle");

    // 登録処理
    var headerRight = header.find(".icrop-PopOverForm-header-right");
    headerRight.text($("#wordSubmit").val());
    headerRight.addClass("PopupRegistButton");
    headerRight.click(function (e) {

        // コントロールが無効の場合は処理しない
        if ($("#clickStatus").val() == "1" || $("#clickStatus").val() == "-1") {

            return;
        }  

        $("#clickStatus").val("1");

        // ボタン押下時の処理 戻り値に対しての処理
        $("#Frame_SC3110101").contents().find("#RegisterButton").click();
        $("#Frame_SC3110101").contents().find("#RegisterButton_Pre").click();
    });

    // キャンセルボタン
    var headerLeft = header.find(".icrop-PopOverForm-header-left");
    headerLeft.text($("#wordCancel").val());
    headerLeft.addClass("PopupCancelButton");
    headerLeft.click(function (e) {

        // コントロールが無効の場合は処理しない
        if ($("#clickStatus").val() == "1") {

            return;
        }

        // 画面を閉じる
        pop.closePopOver();
    });
}

/* ポップアップをクローズする
 * 
 * @param {Object} pop
 * @param {Object} result
 * 
 */
function TestDrivePopOverForm_close(pop, result) {
    //ポストバックさせたい場合のみ、trueを返す
    return false;
}

function closePopOver() {

    // コントロールが無効の場合は処理しない
    if ($("#clickStatus").val() != "1") {

        return;
    }

    gPopOverFrom.closePopOver();

}