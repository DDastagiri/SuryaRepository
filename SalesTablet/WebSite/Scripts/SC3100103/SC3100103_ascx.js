/** 
 * @fileOverview SC3100103 ヘッダ部分の処理
 * 
 * @author m.okamura
 * @version 1.0.0
 */

// ==============================================================
// 定数
// ==============================================================
// ポップアップオブジェクト
var gPopOverFrom = null;

// 登録ボタン押下判定フラグ
var gIsRegist = false;

/* ポップアップを表示する
 * 
 * @param {Object} pop
 * @param {Object} index
 * @param {Object} args
 * @param {Object} container
 * @param {Object} header
 * 
 */
function StandByStaffPopOverForm_render(pop, index, args, container, header) {

    gPopOverFrom = pop;

    page = container.children('#Panel_SC3100103');
    page = $('#Panel_SC3100103').css('display', 'block');
    container.empty().append(page);
    header.attr('style', 'display:block;');

    // ヘッダ部分の表示
    var headerTitle = header.find('.icrop-PopOverForm-header-title');
    headerTitle.text($('#StandByStaffWordTitle').val());
    headerTitle.addClass('StandByStaffPopupTitle');

    // 登録処理
    var headerRight = header.find('.icrop-PopOverForm-header-right');
    headerRight.text($('#StandByStaffWordRegister').val());
    headerRight.addClass('StandByStaffPopupRegistButton');
    headerRight.click(function (e) {

        // コントロールが無効の場合は処理しない
        if ($('#StandByStaffClickStatus').val() == '1' || $('#StandByStaffClickStatus').val() == '-1') {

            return;
        }

        //$('#StandByStaffClickStatus').val('1');

        // ボタン押下時の処理 戻り値に対しての処理
        $('#Frame_SC3100103').contents().find('#RegisterButton').click();
        $('#Frame_SC3100103').contents().find('#RegisterButton_Pre').click();
        gIsRegist = true;
    });

    // キャンセルボタン
    var headerLeft = header.find('.icrop-PopOverForm-header-left');
    headerLeft.text($('#StandByStaffWordCancel').val());
    headerLeft.addClass('StandByStaffPopupCancelButton');
    headerLeft.click(function (e) {

        // コントロールが無効の場合は処理しない
        if ($('#StandByStaffClickStatus').val() == '1') {

            return;
        }

        gIsRegist = false;
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
function StandByStaffPopOverForm_close(pop, result) {

    $('#StandByStaffClickStatus').val('-1');
    // ポストバックさせたい場合のみ、trueを返す
    return gIsRegist;
}

function closePopOver() {

    // コントロールが無効の場合は処理しない
    if ($('#StandByStaffClickStatus').val() != '1') {

        return;
    }

    gPopOverFrom.closePopOver();
}