/** 
 * @fileOverview フォロー設定の処理を記述するファイル.
 * 
 * @author t.mizumoto
 * @version 1.0.0
 */
// ==============================================================
// 定数
// ==============================================================
// 再描画タイプ
var C_SC3190602_REFRESH_TYPE_ON = 1;        // 再描画あり
var C_SC3190602_REFRESH_TYPE_OFF = 0;       // 再描画なし

var C_SC3190602_DATE_FORMAT = "dd/mm/yy";   // 日付フォーマット

// ==============================================================
// DOMロード時処理
// ==============================================================
$(function () {

    // 登録ボタン押下
    $('#SC3190602_Panel .Button_OutBox').live("click", function () {

        // 「OK」時の処理開始 ＋ 確認ダイアログの表示
        if (window.confirm($('<div/>').html($('#SC3190602_RegisterWordHD').val()).text())) {
            // 「OK」時の処理終了
            // 読み込み中アイコン表示
            showLodingSC3190602();

            // 入力情報をJSON形式に変換
            if (createInputToJsonSC3190602()) {
                // 必須入力項目が入力済みの場合、登録処理用の隠しボタンを押下
                $('#SC3190602_RegisterButton').click();
            }
            else {
                // 必須入力項目未入力の場合、エラーメッセージ表示
                alert($('<div/>').html($("#SC3190602_CompulsoryInputWordHD").val()).text());
                closeLodingSC3190602();
            }
        }
        else {
            // 「キャンセル」時の処理開始
            return;
        }
    });

    // キャンセルボタン押下
    $('#SC3190602_Panel .CloseBtn').live("click", function () {
        closeSC3190602Self(C_SC3190602_REFRESH_TYPE_OFF);
    });

    // チェックボタン
    $('#SC3190602_Panel .SC3190602_Check').live("click", function () {
        if ($(this).hasClass("Checked")) {
            $(this).removeClass("Checked");
        }
        else {
            $(this).addClass("Checked");
        }

    });

    // 作業エリア追加処理
    $('#OperationScroll .SC3190602_JobName').live('focusout', function () {

        var inputValue = $(this).val();
        var index = $('#OperationScroll .SC3190602_JobName').index(this) + 1;

        // 入力がある And 一番下の作業エリアであれば新規の作業エリアを追加する
        if (inputValue != "" && $('#OperationScroll .SC3190602_JobName').size() == index) {

            // 作業追加
            $(this).parents("#OperationScroll").append($("#OperationAreaHD").attr('innerHTML'));

            // 作業の表示制御
            SetOperationArea();

            // 対象作業エリアの最終部品行にDatePickerを設定
            var addOperationArea = $('#OperationScroll .OperationArea:last-child');
            addOperationArea.find(".partsTable tr").each(function () {
                setDatePicker($(this).find(".SC3190602_OrdDate"));
                setDatePicker($(this).find(".SC3190602_ArrivalScheDate"));
            });

        }
    });

    // Enter押下時のフォーカス処理
    $(".PopUp .SC3190602Tub").live('keypress', function (aEvent) {

        var elements = ".PopUp .SC3190602Tub";
        var keyCode = aEvent.which ? aEvent.which : aEvent.keyCode;

        if (keyCode == 13) {
            var index = $(elements).index(this);
            if (index < $(elements).size() - 1) {
                $(elements + ":gt(" + index + "):first").focus();
            } else {
                $(elements + ":first").focus();
            }
            aEvent.preventDefault();
        }
    });

    // 部品欄入力時
    $("#OperationScroll").find(".partsTable tr input").live('change', function (aEvent) {

        // 登録ボタンの活性非活性制御
        SetRegisterButton();
    });

    // 作業削除ボタン押下時
    $('#SC3190602_JobDelIcon').live('click', function () {

        // 作業エリアの削除
        $(this).parent().parent().remove();

        // 作業の表示制御
        SetOperationArea();

        // 登録ボタンの活性非活性制御
        SetRegisterButton();
    });

    // 部品の追加
    $('#SC3190602_PartsAddIcon').live('click', function () {

        // 部品追加
        $(this).parents(".OperationArea").find(".partsTable tbody").append($("#OperationAreaHD .partsTable tbody").attr('innerHTML'));

        // 対象作業エリアの最終部品行にDatePickerを設定
        var addPartsRow = $(this).parents(".OperationArea").find(".partsTable tr:last-child");
        setDatePicker(addPartsRow.find(".SC3190602_OrdDate"));
        setDatePicker(addPartsRow.find(".SC3190602_ArrivalScheDate"));

        // 登録ボタンの活性非活性制御
        SetRegisterButton();
    });

    // 部品の削除
    $('#SC3190602_PartsDelIcon').live('click', function () {

        // 部品の削除
        $(this).parent().parent().remove();

        // 登録ボタンの活性非活性制御
        SetRegisterButton();
    });
});

// ==============================================================
// 関数定義
// ==============================================================
/**
 * B/O部品入力の表示を行う.
 * 
 * @param  {String} aBoId B/O ID
 * @return {-} -
 */
function showSC3190602(aBoId) {

    // エリアの表示
    $("#PopUp_Back").show();
    $("#SC3190602_Panel").show();

    // 読み込み中アイコンの表示
    $("#SC3190602_Panel .PopUp").hide();
    showLodingSC3190602();

    // B/O ID保持
    $('#SC3190602_BoId').val(aBoId);
    $('#SC3190602_Input').val("");

    // サーバサイド処理の呼び出し
    $('#SC3190602_LoadSpinButton').click();
}

/**
 * B/O部品入力登録完了処理.
 * 
 * @param  {-} -
 * @return {-} -
 */
function registerCompleteSC3190602() {

    closeSC3190602Self(C_SC3190602_REFRESH_TYPE_ON);
}

/**
 * B/O部品入力を閉じる処理.
 * 
 * @param  {String} aRefreshType 再描画フラグ（1：再描画あり、0：再描画なし）
 * @return {-} -
 */
function closeSC3190602Self(aRefreshType) {

    // エリアの表示
    $("#PopUp_Back").hide();
    $("#SC3190602_Panel").hide();

    // 呼び出し元画面に結果を通知する
    closeSC3190602(aRefreshType);
}

/**
 * 読み込み中アイコンを表示処理
 *
 * @param  {-} -
 * @return {-} -
 */
function showLodingSC3190602() {

    // オーバーレイ表示
    $("#SC3190602_Overlay").show();
    $("#SC3190602_ProcessingServer").show();
}

/**
 * 読み込み中アイコンを非表示処理
 *
 * @param  {-} -
 * @return {-} -
 */
function closeLodingSC3190602() {

    // オーバーレイ非表示
    $("#SC3190602_Overlay").hide();
    $("#SC3190602_ProcessingServer").hide();
    $("#SC3190602_Panel .PopUp").show();
}

/**
 * 描画エリアの初期化処理処理
 *
 * @param  {-} -
 * @return {-} -
 */
function initDisplaySC3190602() {

    // PO ROの設定
    $("#SC3190602_PoNum").val($('<div/>').html($('#SC3190602_PoNumHD').val()).text());
    $("#SC3190602_RoNum").val($('<div/>').html($('#SC3190602_RoNumHD').val()).text());
    $("#SC3190602_CstAppointmentDate").val($('#SC3190602_CstAppDateHD').val());

    // DatePickerの設定
    setDatePicker($("#SC3190602_CstAppointmentDate"));
    
    // 部品エリアの表示制御
    SetOperationArea();

    // 作業エリア分処理を繰り返す
    $('#OperationScroll .OperationArea').each(function () {

        // 部品情報分繰り返す
        $(this).find(".partsTable tr").each(function () {
            // DatePickerの設定            
            setDatePicker($(this).find(".SC3190602_OrdDate"));
            setDatePicker($(this).find(".SC3190602_ArrivalScheDate"));
        });
    });

    // 登録ボタンの活性、非活性
    SetRegisterButton();

    // P/O番号へフォーカスをセット
    $("#SC3190602_PoNum").focus();

}

/**
 * 入力情報をJSON形式に変換処理
 *
 * @param  {-} -
 * @return {bool} true:正常 false:異常(必須項目未入力エラー)
 */
function createInputToJsonSC3190602() {

    // 入力情報をJSON形式に変換
    var json = {};

    json["BoId"] = $("#SC3190602_BoId").val();

    json["PoNum"] = $('<div/>').text($("#SC3190602_PoNum").val()).html();
    json["RoNum"] = $('<div/>').text($("#SC3190602_RoNum").val()).html();
    json["VclStatus"] = $("#SC3190602_VclPartakeFlg").val();
    json["CstAppDate"] = $('<div/>').text($("#SC3190602_CstAppointmentDate").val()).html();

    json["JobList"] = [];

    // 作業情報分繰り返す
    var isNotInput = true;
    $('#OperationScroll .OperationArea').each(function () {

        // 作業情報
        var job = {};

        // 部品情報の配列
        var partsList = [];

        // 部品情報分繰り返す
        $(this).find(".partsTable tr").each(function () {

            // チェックが付いている場合は対象外
            if (!$(this).find(".SC3190602_Check").hasClass("Checked")) {

                // 入力済みエリアの件数
                var inputedItem = $(this).find("input").filter(function () {
                    return 0 < $(this).val().length;
                });

                // 1件以上入力されている場合
                if (0 < inputedItem.length) {

                    // 部品情報
                    var parts = {};
                    parts["PartsName"] = $('<div/>').text($(this).find(".SC3190602_PartsName").val()).html();
                    parts["PartsCode"] = $('<div/>').text($(this).find(".SC3190602_PartsCd").val()).html();
                    parts["PartsAmount"] = $('<div/>').text($(this).find(".SC3190602_PartsAmount").val()).html();
                    parts["OrderDate"] = $('<div/>').text($(this).find(".SC3190602_OrdDate").val()).html();
                    parts["ArrivalDate"] = $('<div/>').text($(this).find(".SC3190602_ArrivalScheDate").val()).html();

                    partsList.push(parts);
                }
            }

        });

        // 部品が1件以上入力されている場合
        if (0 < partsList.length) {

            // 作業名称 番号必須入力チェック
            if ($(this).find(".SC3190602_JobName").val() == "") {
                isNotInput = false;
                return;
            }

            job["JobName"] = $('<div/>').text($(this).find(".SC3190602_JobName").val()).html();
            job["PartsList"] = partsList;
            json["JobList"].push(job);
        }
    });

    if (!isNotInput)
    {
        return false;
    }

    // JSON形式の入力情報を文字列に変換
    var input = JSON.stringify(json);

    // 入力情報をHidden項目に設定
    $("#SC3190602_Input").val(input);

    return true;
}

/**
* 登録ボタンの表示切り替え処理
*
* @param  {-} -
* @return {-} -
*/
function SetRegisterButton() {

    var isInput = false;

    // 部品情報を1行もない場合は、登録ボタンを非活性化
    if ($("#OperationScroll").find(".partsTable tr").size() <= 0) {
        $('#RegisterButton').removeClass("Button_OutBox").addClass("Button_OutBoxOff");
        return;
    }

    // 部品情報分繰り返す
    $("#OperationScroll").find(".partsTable tr").each(function () {

        if (isInput) {
            return;
        }

        // 入力済みエリアの件数
        var inputedItem = $(this).find("input").filter(function () {
            return 0 < $(this).val().length;
        });

        // 1件以上入力されている場合
        if (0 < inputedItem.length) {
            // 登録ボタンを活性化しループを抜ける
            $('#RegisterButton').removeClass("Button_OutBoxOff").addClass("Button_OutBox");
            isInput = true;
        }
        else {
            // 登録ボタンを非活性化
            $('#RegisterButton').removeClass("Button_OutBox").addClass("Button_OutBoxOff");
        }
    });
}

/**
* 作業エリアの表示切り替え
*
* @param  {-} -
* @return {-} -
*/
function SetOperationArea() {
    
    // 作業タイトル文言
    var operationString = $('<div/>').html($('#SC3190602_JobNameWordHD').val()).text();

    // 作業エリア分処理を繰り返す
    var operationAreaList = $('#OperationScroll').children('div.OperationArea');
    for (i = 0; i < operationAreaList.size(); i++) {
        
        // 作業エリアの連番振り直し
        operationAreaList.eq(i).find('#SC3190602_JobNameWord').text(operationString.replace("{0}", i + 1));

        // 最終行の場合[-]ボタン非表示
        if (i == operationAreaList.size() - 1) {
            operationAreaList.eq(i).find('#SC3190602_JobDelIcon').css("display", "none");
        }
        else {
            operationAreaList.eq(i).find('#SC3190602_JobDelIcon').css("display", "block");
        }
    }
}

/**
* メッセージ表示処理を行う
*
* @param  {String} aMessage メッセージ
* @return {-} -
*/
function showMessageBoxSC3190602(aMessage) {

    alert(aMessage);
    closeLodingSC3190602();
}

/**
* DatePickerの設定を行う
*
* @param  {String} aElement 要素
* @return {-} -
*/
function setDatePicker(aElement) {

    var val = aElement.val();
    aElement.datepicker();
    aElement.datepicker("option", "dateFormat", C_SC3190602_DATE_FORMAT);
    aElement.datepicker("setDate", val);
}
