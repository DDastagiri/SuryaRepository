/** 
 * @fileOverview SC3100104 ヘッダ部分の処理
 * 
 * @author m.asano
 * @version 1.0.0
 */

// ==============================================================
// 定数
// ==============================================================
// ポップアップオブジェクト
var gPopOverFrom = null;

// 登録ボタン押下判定フラグ
var gIsRegist = false;

$(window).load(function () {

    // エリア選択時
    $('div#bodyFrame').live(C_TOUCH_START, function (aEvent) {

        if ($('div#CreateCustomerChipPopOverForm_popover', $(parent.document)).hasClass('active')) {
            // キーボードが表示されている場合にキーボードを閉じるための対応
            $('#Frame_SC3100104').contents().find('div.Search1').html(' ');
        }
    });

});
/* ポップアップを表示する
 * 
 * @param {Object} pop
 * @param {Object} index
 * @param {Object} args
 * @param {Object} container
 * @param {Object} header
 * 
 */
function CreateCustomerChipPopOverForm_render(pop, index, args, container, header) {
        
    gPopOverFrom = pop;

    page = container.children('#Panel_SC3100104');
    page = $('#Panel_SC3100104').css('display', 'block');
    container.empty().append(page);
    header.attr('style', 'display:block;');

    // ヘッダ部分の表示
    var headerTitle = header.find('.icrop-PopOverForm-header-title');
    headerTitle.text($('#CreateCustomerChipWordTitle').val());
    headerTitle.addClass('CreateCustomerChipPopupTitle');
    headerTitle.addClass('clip');
    
    // 登録処理
    var headerRight = header.find('.icrop-PopOverForm-header-right');
    headerRight.text($('#CreateCustomerChipWordRegister').val());
    headerRight.addClass('CreateCustomerChipPopupRegistButton');
    headerRight.addClass('clip');
    headerRight.click(function (e) {

        // コントロールが無効の場合は処理しない
        if ($('#CreateCustomerChipCanClick').val() == '0') {
            return;
        }
        $('#CreateCustomerChipCanClick').val('0');
        //$('#Frame_SC3100104').contents().find('li#CustomerRow').remove();
        $('#Frame_SC3100104').contents().find('#LoadingAnimation2').css('display', 'block');
        $('#Frame_SC3100104').contents().find('#SC3100104_OverRay').css('display', 'block');

        $('#Frame_SC3100104').contents().find('#SearchTextStringDummy').attr('innerText', $('#Frame_SC3100104').contents().find('#SearchTextString')[0].value);
        $('#Frame_SC3100104').contents().find('#SearchTextStringDummy').css('display', 'block');
        $('#Frame_SC3100104').contents().find('#SearchTextString').val(" ");

        // キーボードが表示されている場合にキーボードを閉じるための対応
        $('#Frame_SC3100104').contents().find('#CustomerSearchButton').focus();

        // ボタン押下時の処理 戻り値に対しての処理
        $('#Frame_SC3100104').contents().find('#RegisterButton').click();
        //戻り値受け取り後
        $('#Frame_SC3100104').contents().find('#RegisterButton_Pre').click();

        gIsRegist = true;
    });

    // キャンセルボタン
    var headerLeft = header.find('.icrop-PopOverForm-header-left');
    headerLeft.text($('#CreateCustomerChipWordCancel').val());
    headerLeft.addClass('CreateCustomerChipPopupCancelButton');
    headerLeft.addClass('clip');
    headerLeft.click(function (e) {

        // コントロールが無効の場合は処理しない
        if ($('#CreateCustomerChipClickStatus').val() == '1') {
            return;
        }

        // キーボードが表示されている場合にキーボードを閉じるための対応
        $('#Frame_SC3100104').contents().find('#CustomerSearchButton').focus();

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
function CreateCustomerChipPopOverForm_close(pop, result) {

    $('#CreateCustomerChipClickStatus').val('-1');
    // ポストバックさせたい場合のみ、trueを返す
    return gIsRegist;
}

function sc3100104_ClosePopOver() {

    // コントロールが無効の場合は処理しない
    if ($('#CreateCustomerChipClickStatus').val() != '1') {
        return;
    }

    gPopOverFrom.closePopOver();
}