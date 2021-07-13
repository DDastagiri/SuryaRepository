//SC3180201.Main.js
//------------------------------------------------------------------------------
//機能：完成検査承認_javascript
//更新：
//------------------------------------------------------------------------------

// 2014/07/03　アイコンタップ時の反応改善　START　↓↓↓
var isTouch = true;
// 2014/07/03　アイコンタップ時の反応改善　END　　↑↑↑
var hiddenDataNo = {
    InspecItemRegistMode: 0,
    InspecItemMode: 1,
    InspecItemsCheck: 2,
    InspecItemTextInputMode: 3,
    JOB_DTL_ID: 4,
    JOB_INSTRUCT_ID: 5,
    JOB_INSTRUCT_SEQ: 6,
    INSPEC_ITEM_CD: 7,
    STALL_USE_ID: 8,
    TrnRowLockVersion: 9,
    back_InspecItemsCheck: 10,
    back_InspecItemsTextBefore: 11,
    back_InspecItemsTextAfter: 12,
    back_InspecItemsSelector:13	
}
/**
 * DOMロード直後の処理(重要事項).
 * @return {void}
 */
$("document").ready(function () {

    //スクロール設定
    $("body").unbind("touchmove.icropScript");
    $(".STC04BlockRightMain").fingerScroll();
//  2019/10/18 スクロールの位置がおかしくなるためDivでのスクロールは廃止
//    $(".SCT03BlockLeftAdviceMemo").fingerScroll();
//    $(".AdviceMemo").fingerScroll();
    $(".SCT03BlockLeftWorkBox2").fingerScroll();

    //ヘッダーの戻るボタンをタップしたときにクルクルを表示する
/*    $('#MstPG_BackLinkButton').bind("click", function () {
        ActiveDisplayOn();
    });
*/
    if (null != document.getElementById('ScrollBlock_Engine')) {
        if (170 >= $('#ScrollBlock_Engine').innerHeight()) {
            document.getElementById('ScrollBlock_Engine').style.height = 171 + "px";
        } else {
            document.getElementById('ScrollBlock_Engine').style.height = $('#ScrollBlock_Engine').innerHeight() + "px";
        }
    }
    if (null != document.getElementById('ScrollBlock_Inroom')) {
        if (170 >= $('#ScrollBlock_Inroom').innerHeight()) {
            document.getElementById('ScrollBlock_Inroom').style.height = 171 + "px";
        } else {
            document.getElementById('ScrollBlock_Inroom').style.height = $('#ScrollBlock_Inroom').innerHeight() + "px";
        }
    }
    if (null != document.getElementById('ScrollBlock_Left')) {
        if (170 >= $('#ScrollBlock_Left').innerHeight()) {
            document.getElementById('ScrollBlock_Left').style.height = 171 + "px";
        } else {
            document.getElementById('ScrollBlock_Left').style.height = $('#ScrollBlock_Left').innerHeight() + "px";
        }
    }
    if (null != document.getElementById('ScrollBlock_Right')) {
        if (170 >= $('#ScrollBlock_Right').innerHeight()) {
            document.getElementById('ScrollBlock_Right').style.height = 171 + "px";
        } else {
            document.getElementById('ScrollBlock_Right').style.height = $('#ScrollBlock_Right').innerHeight() + "px";
        }
    }
    if (null != document.getElementById('ScrollBlock_Under')) {
        if (170 >= $('#ScrollBlock_Under').innerHeight()) {
            document.getElementById('ScrollBlock_Under').style.height = 171 + "px";
        } else {
            document.getElementById('ScrollBlock_Under').style.height = $('#ScrollBlock_Under').innerHeight() + "px";
        }
    }
    if (null != document.getElementById('ScrollBlock_Trunk')) {
        if (170 >= $('#ScrollBlock_Trunk').innerHeight()) {
            document.getElementById('ScrollBlock_Trunk').style.height = 171 + "px";
        } else {
            document.getElementById('ScrollBlock_Trunk').style.height = $('#ScrollBlock_Trunk').innerHeight() + "px";
        }
    }
    if (null != document.getElementById('ScrollBlock_Maintenance')) {
        if (170 >= $('#ScrollBlock_Maintenance').innerHeight()) {
            document.getElementById('ScrollBlock_Maintenance').style.height = 171 + "px";
        } else {
            document.getElementById('ScrollBlock_Maintenance').style.height = $('#ScrollBlock_Maintenance').innerHeight() + "px";
        }
    }

    $("#ButtonRejectWork").bind("click", function () {

        //2019/4/19 [PUAT4226 アドバイスコメント上限対応]対応　Start
        var adv = document.getElementById("InputStrMsg").value;
        adv = adv.replace(/<br>/g, "\r\n");
        adv = adv.replace(/&lt;/g, "<");
        adv = adv.replace(/&gt;/g, ">");
        adv = adv.replace(/&amp;/g, "&");

        if (adv.length > 1300) {
            alert(document.getElementById('overText').value);
        }
        //2019/4/19 [PUAT4226 アドバイスコメント上限対応]対応　End

        else if (document.getElementById('ViewMode').value != "1" && "" == document.getElementById('ErrorFlg').value) {
            //全体クルクル表示
            ActiveDisplayOn();

            HiddenButtonRejectWork.click();
        }
    });
    $("#ButtonApproveWork").bind("click", function () {

        //2019/4/19 [PUAT4226 アドバイスコメント上限対応]対応　Start
        var adv = document.getElementById("InputStrMsg").value;
        adv = adv.replace(/<br>/g, "\r\n");
        adv = adv.replace(/&lt;/g, "<");
        adv = adv.replace(/&gt;/g, ">");
        adv = adv.replace(/&amp;/g, "&");

        if (adv.length > 1300) {
            alert(document.getElementById('overText').value);
        }
        //2019/4/19 [PUAT4226 アドバイスコメント上限対応]対応　End

        else if (document.getElementById('ViewMode').value != "1" && "" == document.getElementById('ErrorFlg').value) {
            if (true == allcheck()) {
                // 項目チェックOK
                //全体クルクル表示
                ActiveDisplayOn();

                HiddenButtonApproveWork.click();
            } else {
                // 項目チェックNG(エラーメッセージ表示)
                alert(document.getElementById('ItemCheckErrorMessage').value);
            }
        }
    });

    //$("#ButtonRejectWork").css("display", "inline-block");
    //$("#ButtonApproveWork").css("display", "inline-block");
    $("#ButtonRejectWork").css("display", "table-cell");
    $("#ButtonApproveWork").css("display", "table-cell");

    /* ViewMode=1又はエラー時にはボタンの色を変更する */
    if ((document.getElementById('ViewMode').value != "" && document.getElementById('ViewMode').value != "0") || "" != document.getElementById('ErrorFlg').value) {
        $("#ButtonRejectWork").css("background", "#999999");
        $("#ButtonApproveWork").css("background", "#999999");
    }

    for (intPos = 1; intPos <= 6; intPos++) {
        intRow = 1;
        while (null != document.getElementById('BeforeText' + intPos + '_' + intRow)) {
// 2014/07/17　電卓キー表示対応　START　↓↓↓
//            document.getElementById('BeforeText' + intPos + '_' + intRow).onfocus = function (e) { return valueCnv_OnFocus(this, 1); }
//            document.getElementById('BeforeText' + intPos + '_' + intRow).onkeydown = function (e) { return isValueKey(e); }
//            document.getElementById('BeforeText' + intPos + '_' + intRow).onblur = function (e) { return valueCnv_OnBlur(this, 1); }
//            document.getElementById('AfterText' + intPos + '_' + intRow).onfocus = function (e) { return valueCnv_OnFocus(this, 2); }
//            document.getElementById('AfterText' + intPos + '_' + intRow).onkeydown = function (e) { return isValueKey(e); }
//            document.getElementById('AfterText' + intPos + '_' + intRow).onblur = function (e) { return valueCnv_OnBlur(this, 2); }

            //電卓キー表示イベントの追加
            SetNumericKeypad("BeforeText");
            SetNumericKeypad("AfterText");
// 2014/07/17　電卓キー表示対応　END　　↑↑↑

            intRow++;
        }
    }

    /* 検査項目の初期表示(非表示) */
    pos = -1;
    if ("" != document.getElementById('VehicleChartNo').value) {
        pos = document.getElementById('VehicleChartNo').value;
    }
    VehicleChartClick(pos);

    // 2019/10/18 スクロールの位置がおかしくなるためDivでのスクロールは廃止
    //inichangetext();

    /* 全体の項目チェック */
    allcheck();

    /* 全体の項目値コピー */
    allinit();
});

$(window).load(function () {
    if ("" != document.getElementById("hdnErrorMsg").value) {
        alert(document.getElementById('hdnErrorMsg').value);
    }
    // 2015/5/1 強制納車対応 警告時POPUP表示と前画面遷移リクエスト start
    if ("" != document.getElementById("hdnWarningMsg").value) {
        alert(document.getElementById('hdnWarningMsg').value);
        ActiveDisplayOn();
        HiddenButtonWarning.click()
    }
    // 2015/5/1 強制納車対応 警告時POPUP表示と前画面遷移リクエスト end


});

// 2014/07/17　電卓キー表示対応　START　↓↓↓
/**
 * 電卓キー(数値型キーボード)呼び出し処理
 * @param {string} defaultID ： "BeforeText" or "AfterText"
 * @return {bool} 正常:true/異常:false
 */
function SetNumericKeypad(defaultID) {

    // 点検結果入力(Before/After)項目のデフォルト表示用文言取得("Before" or "After")
    var defaultText = document.getElementById(defaultID).value;

    // コントロール名(例："#BeforeText1_1")
    var controlName = "#" + defaultID + intPos + "_" + intRow;

    // 対象コントロールタップ時に電卓キー呼び出し関連付け
    $(controlName).NumericKeypad({
        maxDigits: 6,
        acceptDecimalPoint: true,
//        defaultValue: "",
        completionLabel: "OK",
        cancelLabel: "Cancel",
//        valueChanged: function (num) { $(this).val(num); },
        valueChanged: function (num) { valueCnv_OnBlur(this, num, defaultText); },
//        parentPopover: null,
//        open: function () { $(this).NumericKeypad("setValue", $(this).val()); },
        open: function () { valueCnv_OnFocus(this, defaultText); },
        close: function () { $(".icrop-NumericKeypad-content-TextArea").innerText = ""; }
    });
}
// 2014/07/17　電卓キー表示対応　END　　↑↑↑

/**
* 全体の項目チェック
* @param {void}
* @return {bool} 正常:true/異常:false
*/
function allcheck() {

    var bResult = true;
    for (var pos = 1; pos <= 6; pos++) {
        if (true == poscheck(pos)) {
            // 未入力項目なし
        } else {
            // 未入力項目あり
            bResult = false;
        }
    }

    if (false == posMainteCheck()) {
        bResult = false;
    }

    return bResult;
}

/**
* 位置毎の項目チェック
* @param {int} 位置
* @return {bool} 正常:true/異常:false
*/
function poscheck(pos) {

    bResult = true;
    idx = 1;
    while (null != document.getElementById('HiddenAllData' + pos + '_' + idx) && true == bResult) {
        var hiddenDataList = document.getElementById('HiddenAllData' + pos + '_' + idx).value.split("|")
        //        strInspecItemMode = document.getElementById('InspecItemMode' + pos + '_' + idx).value
        strInspecItemMode = hiddenDataList[hiddenDataNo.InspecItemMode]
        //if ("1" == strInspecItemMode) {
        //    // チェック対象(現在(InspecItemModeX_X=1))
        if ("1" == strInspecItemMode || "2" == strInspecItemMode) {
            // チェック対象(現在,過去(InspecItemModeX_X=1,2))
            //            strInspecItemsCheck = document.getElementById('InspecItemsCheck' + pos + '_' + idx).value
            strInspecItemsCheck = hiddenDataList[hiddenDataNo.InspecItemsCheck]
            if ("1" == strInspecItemsCheck || "2" == strInspecItemsCheck || "3" == strInspecItemsCheck || "4" == strInspecItemsCheck || "5" == strInspecItemsCheck || "6" == strInspecItemsCheck) {
                // InspecItemsCheckX_Xの範囲が1～6：正常
                //                strInspecItemTextInputMode = document.getElementById('InspecItemTextInputMode' + pos + '_' + idx).value
                strInspecItemTextInputMode = hiddenDataList[hiddenDataNo.InspecItemTextInputMode]

                if ("1" == strInspecItemTextInputMode) {
                    // Before,After入力あり
                    strBeforeText = document.getElementById('BeforeText' + pos + '_' + idx).value
                    if (true == isNaN(strBeforeText)) {
                        // 数値でない：異常
                        bResult = false;
//                    } else {
//                        // 数値：正常
//                        floBeforeText = parseFloat(strBeforeText)
//                        if (0 == floBeforeText) {
//                            // 0(入力なしと同じ)：異常
//                            bResult = false;
//                        } else {
//                            // 0以外：正常
//                        }
                    }
                } else {
                    // Before,After入力なし：正常
                }
            }else if ("7" == strInspecItemsCheck){
            
            }else {
                // InspecItemsCheckX_Xの範囲が1～6以外：エラー
                bResult = false;
            }
        }
        idx++;
    }

    var strBtnID = "";
    if (1 == pos) {
        strBtnID = "EngineRoomBtn";
    } else if (2 == pos) {
        strBtnID = "InroomBtn";
    } else if (3 == pos) {
        strBtnID = "LeftBtn";
    } else if (4 == pos) {
        strBtnID = "RightBtn";
    } else if (5 == pos) {
        strBtnID = "UnderBtn";
    } else if (6 == pos) {
        strBtnID = "TrunkBtn";
    }

    if (1 == idx) {
        // データなし
        document.getElementById(strBtnID).style.background = "-webkit-gradient(linear, left top, left bottom, from(#B3B1B1), to(#797878))";
    } else {
        // データあり
        if (true == bResult) {
            // 未入力項目なし
            // ボタン色を緑に変更
            document.getElementById(strBtnID).style.background = "-webkit-gradient(linear, left top, left bottom, from(#66DA65), to(#228221))";
        } else {
            // 未入力項目あり
            //ボタン色をオレンジに変更
            document.getElementById(strBtnID).style.background = "-webkit-gradient(linear, left top, left bottom, from(#FCBF05), to(#A17A03))";
            bResult = false;
        }
    }

    return bResult;
}

/**
* 位置毎の項目チェック(メンテナンス用)
* @return {bool} 正常:true/異常:false
*/
function posMainteCheck() {

    var strBtnID = "";
    bResult = true;
    var idx = 1;
    while (null != document.getElementById('MainteCheck7_' + idx) && true == bResult) {
        var strMainteMode = document.getElementById('MainteMode7_' + idx).value
        //if ("1" == strMainteMode) {
        //    // チェック対象(現在(MainteMode7_X=1))
        if ("1" == strMainteMode || "2" == strMainteMode) {
            // チェック対象(現在,過去(MainteMode7_X=1,2))
            var objMaintenance1 = document.getElementById('Maintenance7_' + idx + '_1')
            var objMaintenance2 = document.getElementById('Maintenance7_' + idx + '_2')
            if (true == objMaintenance1.checked || true == objMaintenance2.checked) {
                // Maintenance7__Xの範囲が1,2：正常
            } else {
                // Maintenance7__Xの範囲が1,2以外：エラー
                bResult = false;
            }
        }
        idx++;
    }

    strBtnID = "MaintenanceBtn";
    if (1 == idx) {
        // データなし
        document.getElementById(strBtnID).style.background = "-webkit-gradient(linear, left top, left bottom, from(#B3B1B1), to(#797878))";
    } else {
        // データあり
        if (true == bResult) {
            // 未入力項目なし
            // ボタン色を緑に変更
            document.getElementById(strBtnID).style.background = "-webkit-gradient(linear, left top, left bottom, from(#66DA65), to(#228221))";
        } else {
            // 未入力項目あり
            //ボタン色をオレンジに変更
            document.getElementById(strBtnID).style.background = "-webkit-gradient(linear, left top, left bottom, from(#FCBF05), to(#A17A03))";
            bResult = false;
        }
    }

    return bResult;
}

/**
* 全体の更新チェック
* @param {void}
* @return {bool} 更新なし:true/更新あり:false
*/
function alledited() {

    var bResult = true;

    if ("1" == document.getElementById('TextEditedFlg').value) {
        return false;
    }

    for (var pos = 1; pos <= 6 && true == bResult; pos++) {
        bResult = poseditedcheck(pos);
    }
    return bResult;
}

/**
* 位置毎の更新チェック
* @param {int} 位置
* @return {bool} 更新なし:true/更新あり:false
*/
function poseditedcheck(pos) {

    bResult = true;
    idx = 1;
    while (null != document.getElementById('HiddenAllData' + pos + '_' + idx) && true == bResult) {
        var hiddenDataList = document.getElementById('HiddenAllData' + pos + '_' + idx).value.split("|")

//        strInspecItemsCheck = document.getElementById('InspecItemsCheck' + pos + '_' + idx).value;
//        strBAK_InspecItemsCheck = document.getElementById('BAK_InspecItemsCheck' + pos + '_' + idx).value;

//        strBeforeText = document.getElementById('BeforeText' + pos + '_' + idx).value;
//        strBAK_BeforeText = document.getElementById('BAK_BeforeText' + pos + '_' + idx).value;

//        strAfterText = document.getElementById('AfterText' + pos + '_' + idx).value;
//        strBAK_AfterText = document.getElementById('BAK_AfterText' + pos + '_' + idx).value;

//        strInspecItemsSelector = document.getElementById('InspecItemsSelector' + pos + '_' + idx).value;
//        strBAK_InspecItemsSelector = document.getElementById('BAK_InspecItemsSelector' + pos + '_' + idx).value;

        strInspecItemsCheck = hiddenDataList[hiddenDataNo.InspecItemsCheck];
        strBAK_InspecItemsCheck = hiddenDataList[hiddenDataNo.back_InspecItemsCheck];

        strBeforeText = document.getElementById('BeforeText' + pos + '_' + idx).value;
        strBAK_BeforeText = hiddenDataList[hiddenDataNo.back_InspecItemsTextBefore];

        strAfterText = document.getElementById('AfterText' + pos + '_' + idx).value;
        strBAK_AfterText = hiddenDataList[hiddenDataNo.back_InspecItemsTextAfter];

        //strInspecItemsSelector = document.getElementById('InspecItemsSelector' + pos + '_' + idx).value;
        var InspecItemsSelector = document.getElementById('InspecItemsSelector' + pos + '_' + idx);
        var SelectorValue = [];
        for (i = 0; i < InspecItemsSelector.length; i++) {
            if (InspecItemsSelector[i].selected) {
                SelectorValue.push(InspecItemsSelector[i].value);
            }
        }
        SelectorValue.sort();
        strInspecItemsSelector = SelectorValue.join(',');

        strBAK_InspecItemsSelector = hiddenDataList[hiddenDataNo.back_InspecItemsSelector];

        if (strInspecItemsCheck != strBAK_InspecItemsCheck || strBeforeText != strBAK_BeforeText || strAfterText != strBAK_AfterText || strInspecItemsSelector != strBAK_InspecItemsSelector) {
            bResult = false;
        }
        idx++;
    }

    return bResult;
}

/**
* 全体の項目値コピー
* @param {void}
* @return {void} なし
*/
function allinit() {

    for (var pos = 1; pos <= 6 && true == bResult; pos++) {
        bResult = poseditedcheck(pos);
    }
}

/**
* 位置毎の項目値コピー
* @param {int} 位置
* @return {void} なし
*/
function posinit(pos) {

    idx = 1;
    while (null != document.getElementById('HiddenAllData' + pos + '_' + idx) && true == bResult) {
        var hiddenDataobj = document.getElementById('HiddenAllData' + pos + '_' + idx);
        var hiddenDataList = hiddenDataobj.value.split("|");

//        document.getElementById('BAK_InspecItemsCheck' + pos + '_' + idx).value = document.getElementById('InspecItemsCheck' + pos + '_' + idx).value;

//        document.getElementById('BAK_BeforeText' + pos + '_' + idx).value = document.getElementById('BeforeText' + pos + '_' + idx).value;

//        document.getElementById('BAK_AfterText' + pos + '_' + idx).value = document.getElementById('AfterText' + pos + '_' + idx).value;

        //        document.getElementById('BAK_InspecItemsSelector' + pos + '_' + idx).value = document.getElementById('InspecItemsSelector' + pos + '_' + idx).value;
        hiddenDataList[hiddenDataNo.back_InspecItemsCheck] = hiddenDataList[hiddenDataNo.InspecItemsCheck];

        hiddenDataList[hiddenDataNo.back_InspecItemsTextBefore] = document.getElementById('BeforeText' + pos + '_' + idx).value;

        hiddenDataList[hiddenDataNo.back_InspecItemsTextAfter] = document.getElementById('AfterText' + pos + '_' + idx).value;

//      hiddenDataList[hiddenDataNo.back_InspecItemsSelector] = document.getElementById('InspecItemsSelector' + pos + '_' + idx).value;
        var InspecItemsSelector = document.getElementById('InspecItemsSelector' + pos + '_' + idx);
        var SelectorValue = [];
        for (i = 0; i < InspecItemsSelector.length; i++) {
            if (InspecItemsSelector[i].selected) {
                SelectorValue.push(InspecItemsSelector[i].value);
            }
        }
        SelectorValue.sort();
        hiddenDataList[hiddenDataNo.back_InspecItemsSelector] = SelectorValue.join(',');

        hiddenDataobj.value = hiddenDataList.join("|")
        idx++;
    }
}

/**
* VehicleChartの項目クリック
* @param {int} 指定番号によるイベントコール
* @return {void}
*/
function VehicleChartClick(pos) {

    // 2014/07/03　アイコンタップ時の反応改善　START　↓↓↓
    //TouchMoveイベントが発生していたら処理しない
    if (isTouch == false) {
        return false;
    }
    // 2014/07/03　アイコンタップ時の反応改善　END　　↑↑↑

        document.getElementById('VehicleChartNo').value = pos;

        document.getElementById('OperationItems_Maintenance').style.display = 'none';
        document.getElementById('OperationItems_Maintenance').style.height = '1px';

        document.getElementById('OperationItems_Engine').style.display = 'none';
        document.getElementById('OperationItems_Engine').style.height = '1px';

        document.getElementById('OperationItems_Inroom').style.display = 'none';
        document.getElementById('OperationItems_Inroom').style.height = '1px';

        document.getElementById('OperationItems_Left').style.display = 'none';
        document.getElementById('OperationItems_Left').style.height = '1px';

        document.getElementById('OperationItems_Right').style.display = 'none';
        document.getElementById('OperationItems_Right').style.height = '1px';

        document.getElementById('OperationItems_Under').style.display = 'none';
        document.getElementById('OperationItems_Under').style.height = '1px';

        document.getElementById('OperationItems_Trunk').style.display = 'none';
        document.getElementById('OperationItems_Trunk').style.height = '1px';

        document.getElementById('OperationItems_Error').style.display = 'none';
        document.getElementById('OperationItems_Error').style.height = '1px';

    if ("" == document.getElementById('ErrorFlg').value) {
        if (0 == pos) {
            document.getElementById('OperationItems_Maintenance').style.display = '';
            document.getElementById('OperationItems_Maintenance').style.height = '*';
        } else if (1 == pos) {
            document.getElementById('OperationItems_Engine').style.display = '';
            document.getElementById('OperationItems_Engine').style.height = '*';
        } else if (2 == pos) {
            document.getElementById('OperationItems_Inroom').style.display = '';
            document.getElementById('OperationItems_Inroom').style.height = '*';
        } else if (3 == pos) {
            document.getElementById('OperationItems_Left').style.display = '';
            document.getElementById('OperationItems_Left').style.height = '*';
        } else if (4 == pos) {
            document.getElementById('OperationItems_Right').style.display = '';
            document.getElementById('OperationItems_Right').style.height = '*';
        } else if (5 == pos) {
            document.getElementById('OperationItems_Under').style.display = '';
            document.getElementById('OperationItems_Under').style.height = '*';
        } else if (6 == pos) {
            document.getElementById('OperationItems_Trunk').style.display = '';
            document.getElementById('OperationItems_Trunk').style.height = '*';
        }
    } else {
        document.getElementById('OperationItems_Error').style.display = '';
        document.getElementById('OperationItems_Error').style.height = '*';
    }

    return false;
}

function IconRadioChange(pos, idx, num) {

    // 2014/07/03　アイコンタップ時の反応改善　START　↓↓↓
    //TouchMoveイベントが発生していたら処理しない
    if (isTouch == false) {
        return false;
    }
    // 2014/07/03　アイコンタップ時の反応改善　END　　↑↑↑

    var objimg = new Array;
    if (null != document.getElementById('CheckIcon' + pos + '_' + idx + '_7')) {
        document.getElementById('CheckIcon' + pos + '_' + idx + '_7').className = 'Icon7 No_Check_blue';
    }
    if (null != document.getElementById('CheckIcon' + pos + '_' + idx + '_1')) {
        document.getElementById('CheckIcon' + pos + '_' + idx + '_1').className = 'Icon1 Good_blue';
    }
    if (null != document.getElementById('CheckIcon' + pos + '_' + idx + '_2')) {
        document.getElementById('CheckIcon' + pos + '_' + idx + '_2').className = 'Icon2 Inspect_blue';
    }
    if (null != document.getElementById('CheckIcon' + pos + '_' + idx + '_3')) {
        document.getElementById('CheckIcon' + pos + '_' + idx + '_3').className = 'Icon3 Replace_blue';
    }
    if (null != document.getElementById('CheckIcon' + pos + '_' + idx + '_4')) {
        document.getElementById('CheckIcon' + pos + '_' + idx + '_4').className = 'Icon4 Fix_blue';
    }
    if (null != document.getElementById('CheckIcon' + pos + '_' + idx + '_5')) {
        document.getElementById('CheckIcon' + pos + '_' + idx + '_5').className = 'Icon5 Cleaning_blue';
    }
    if (null != document.getElementById('CheckIcon' + pos + '_' + idx + '_6')) {
        document.getElementById('CheckIcon' + pos + '_' + idx + '_6').className = 'Icon6 Swap_blue';
    }

    /* アイコンの種類を1つに限定(イメージが全て揃っていないため) */
    if (7 == num) {
        if (null != document.getElementById('CheckIcon' + pos + '_' + idx + '_7')) {
            document.getElementById('CheckIcon' + pos + '_' + idx + '_7').className = 'Icon7 No_Check_green';
        }
        document.getElementById('BeforeText' + pos + '_' + idx).value = document.getElementById('BeforeText').value;
        document.getElementById('AfterText' + pos + '_' + idx).value = document.getElementById('AfterText').value;
        var DdlOptions = document.getElementById('InspecItemsSelector' + pos + '_' + idx).options;
        for (var i = 0; i < DdlOptions.length; i++) {
            DdlOptions[i].selected = false;
        } 
        document.getElementById('BeforeText' + pos + '_' + idx).disabled="true";
        document.getElementById('AfterText' + pos + '_' + idx).disabled="true";
        document.getElementById('InspecItemsSelector' + pos + '_' + idx).disabled="true";
    } else {
        document.getElementById('BeforeText' + pos + '_' + idx).disabled="";
        document.getElementById('AfterText' + pos + '_' + idx).disabled="";
        document.getElementById('InspecItemsSelector' + pos + '_' + idx).disabled="";
        if (1 == num) {
            if (null != document.getElementById('CheckIcon' + pos + '_' + idx + '_1')) {
                document.getElementById('CheckIcon' + pos + '_' + idx + '_1').className = 'Icon1 Good_green';
            }
        } else if (2 == num) {
            if (null != document.getElementById('CheckIcon' + pos + '_' + idx + '_2')) {
                document.getElementById('CheckIcon' + pos + '_' + idx + '_2').className = 'Icon2 Inspect_green';
            }
        } else if (3 == num) {
            if (null != document.getElementById('CheckIcon' + pos + '_' + idx + '_3')) {
                document.getElementById('CheckIcon' + pos + '_' + idx + '_3').className = 'Icon3 Replace_green';
            }
        } else if (4 == num) {
            if (null != document.getElementById('CheckIcon' + pos + '_' + idx + '_4')) {
                document.getElementById('CheckIcon' + pos + '_' + idx + '_4').className = 'Icon4 Fix_green';
            }
        } else if (5 == num) {
            if (null != document.getElementById('CheckIcon' + pos + '_' + idx + '_5')) {
                document.getElementById('CheckIcon' + pos + '_' + idx + '_5').className = 'Icon5 Cleaning_green';
            }
        } else if (6 == num) {
            if (null != document.getElementById('CheckIcon' + pos + '_' + idx + '_6')) {
                document.getElementById('CheckIcon' + pos + '_' + idx + '_6').className = 'Icon6 Swap_green';
            }
        }
    }
    var hiddenDataobj = document.getElementById('HiddenAllData' + pos + '_' + idx);
    var hiddenDataList = hiddenDataobj.value.split("|");
    hiddenDataList[hiddenDataNo.InspecItemsCheck] = num;
    hiddenDataobj.value = hiddenDataList.join("|")
//    document.getElementById('InspecItemsCheck' + pos + '_' + idx).value = num;
    // 2014/07/03　アイコンタップ時の反応改善　START　↓↓↓
    //document.getElementById('InspecItemsSelector' + pos + '_' + idx).focus();
    //document.getElementById('InspecItemsSelector' + pos + '_' + idx).blur();

    //allcheck();

    var timerFunction = function () {
        document.getElementById('InspecItemsSelector' + pos + '_' + idx).focus();
        document.getElementById('InspecItemsSelector' + pos + '_' + idx).blur();

        allcheck();
    }
    var timer = setTimeout(timerFunction, 10);
    // 2014/07/03　アイコンタップ時の反応改善　END　　↑↑↑
}

/* スクロールの位置がおかしくなるためDivでのスクロールは廃止
var Changeflag = false;
$(document).ready(function () {
    var msg = document.getElementById('InputStrMsg');
    msg.style.overflowY = "auto";
    function TouchMoveFunc(e) {
        //alert("test");
    }
    if (document.addEventListener) {
        // スワイプ時に発生するイベント
        document.addEventListener("touchmove", TouchMoveFunc, false);
    }

});

function changeinput() {
    if (Changeflag) return;
    Changeflag = true;
    var msg = document.getElementById('InputStrMsg');
    var msgtext = msg.innerHTML;
    msg.style.overflow = "hidden";
    msgtext = msgtext.replace(/<br>/g, "\r\n");
    msg.innerHTML = "<form><TEXTAREA class=\"ChangeInput\" id=\"InputStrMsg\" onblur=\"changetext();\">" + msgtext + "</textarea></form>";
    var inp = document.getElementById('InputStrMsg');
    //inp.select();
    inp.focus();
}

function inichangetext() {
    var inp = document.getElementById('TechnicianAdvice');
    var inpval = inp.value;
    var msg = document.getElementById('InputStrMsg');
    msg.style.overflowY = "auto";
    inpval = inpval.replace(/\r\n/g, "<br/>");
    inpval = inpval.replace(/(\n|\r)/g, "<br/>");
    msg.innerHTML = inpval;
    Changeflag = false;

    document.getElementById('InputStrMsg').style.height = getAdviceHeight(inp.value) + "px";
}

function changetext() {
    var inp = document.getElementById('InputStrMsg');
    var inpval = inp.value;
    var msg = document.getElementById('InputStrMsg');
    msg.style.overflowY = "auto";
    inpval = inpval.replace(/\r\n/g, "<br/>");
    inpval = inpval.replace(/(\n|\r)/g, "<br/>");
    msg.innerHTML = inpval + "<br/>" + document.getElementById('UserName').value + " " + nowDateTime();
    Changeflag = false;

    document.getElementById('TechnicianAdvice').value = msg.innerHTML.replace(/<br>/g, "\r\n");
    document.getElementById('TextEditedFlg').value = "1";

    document.getElementById('InputStrMsg').style.height = getAdviceHeight(inp.value) + "px";
}
*/

// 2019/10/18 スクロールの位置がおかしくなるためDivでのスクロールは廃止しユーザー名とTextEditedFlgの更新のみ行う
function changetext() {
    var inp = document.getElementById('InputStrMsg');
    var inpval = inp.value;
    inp.value = inpval + "\r\n" + document.getElementById('UserName').value + " " + nowDateTime();
    document.getElementById('TextEditedFlg').value = "1";
}

function nowDateTime() {
    var dateTime = new Date();
    var strDateTime = "";
    strDateTime = dateTime.getFullYear() + '/' + (dateTime.getMonth() + 1) + '/' + dateTime.getDate();
    strDateTime += ' ';
    strDateTime += dateTime.getHours() + ":" + dateTime.getMinutes() + ":" + dateTime.getSeconds();
    return strDateTime;
}

function getAdviceHeight(msg) {
    var result = 110;
    if ("" == msg) {
        result = 110;
    } else if (result < (msg.split("\n").length + 2) * 110 / 7) {
        result = (msg.split("\n").length + 2) * 110 / 7;
    }
    return result;
}

function executeCaleNew() {
    var ymd = { year: "", month: "", day: "" };
    ymd.year = $("#Yearhidden").val();
    ymd.month = ("0" + $("#Monthhidden").val()).slice(-2);
    ymd.day = ("0" + $("#Dayhidden").val()).slice(-2);
    window.location = "icrop:cale:::" + ymd.year + "-" + ymd.month + "-" + ymd.day;
    return false;
}

function SetFutterApplication() {
    //スケジュールオブジェクトを拡張
    $.extend(window, { schedule: {} });
    //スケジュールオブジェクトに関数追加
    $.extend(schedule, {
        //アプリ起動クラス
        appExecute: {
            //カレンダーアプリ起動(単体)
            executeCaleNew: function () {
                window.location = "icrop:cale:";
                return false;
            },
            //電話帳アプリ起動(単体)
            executeCont: function () {
                window.location = "icrop:cont:";
                return false;
            }
        }
    });
}

/**
* フッターボタンのクリックイベント.
* @return {}
*/
function FooterButtonClick(Id) {

    //TR-SVT-TMT-20160512-001 サービス来店者管理が存在しない場合
    //BASREZIDが空文字の場合には、R/O一覧と追加作業一覧には遷移しないとする
    if (document.getElementById("BASREZID").value == "") {
        if (Id == 500 || Id == 1200) {
            return;
        }
    }

    //全体クルクル表示
    ActiveDisplayOn();

        //ViewModeの際に変更チェックをしない
    if (document.getElementById('ViewMode').value != "1" && "" == document.getElementById('ErrorFlg').value) {
        //編集チェック(何か変更されたか)
        if (false == alledited()) {
            myRet = confirm(document.getElementById('EditedMessage').value);
            if (true != myRet) {

                //クルクル非表示
                ActiveDisplayOff();
                return;
            }
        }
    }

    //タイマーセット
    commonRefreshTimer(function () { __doPostBack("", ""); });
    //各イベント処理実行
    switch (Id) {
        case 100:
            __doPostBack('ctl00$MstPG_FootItem_Main_100', '');
            break;
        case 200:
            __doPostBack('ctl00$MstPG_FootItem_Main_200', '');
            break;
        case 300:
            __doPostBack('ctl00$MstPG_FootItem_Main_300', '');
            break;
        case 400:
            __doPostBack('ctl00$MstPG_FootItem_Main_400', '');
            break;
        case 500:
            __doPostBack('ctl00$MstPG_FootItem_Main_500', '');
            break;
        case 600:
            __doPostBack('ctl00$MstPG_FootItem_Main_600', '');
            break;
        case 700:
            __doPostBack('ctl00$MstPG_FootItem_Main_700', '');
            break;
        case 800:
            __doPostBack('ctl00$MstPG_FootItem_Main_800', '');
            break;
        case 900:
            __doPostBack('ctl00$MstPG_FootItem_Main_900', '');
            break;
        case 1000:
            __doPostBack('ctl00$MstPG_FootItem_Main_1000', '');
            break;
        case 1100:
            __doPostBack('ctl00$MstPG_FootItem_Main_1100', '');
            break;
        case 1200:
            __doPostBack('ctl00$MstPG_FootItem_Main_1200', '');
            break;
    }
}

function executeCont() {
    window.location = "icrop:cont:";
    return false;
}

/**
* 入力された文字がパーセント（数値 or 「.」）かどうかを返す
* @return {}
*/
function isValueKey(e) {
    if (isSpecialKey(e)) {
        return true;
    }
    if (isNumericKey(e)) {
        return true;
    }
    var code = getCharCode(e);
    switch (code) {
        case 190:   // 「.」
            if (!shiftKey(e)) {
                return true;
            }
            break;
        case 110:   // 「.」（テンキー）
            return true;
            break;
    }
    return false;
}

/**
* 入力された文字が特殊文字かどうかを返す
* @return {}
*/
function isSpecialKey(e) {
    var code = getCharCode(e);
    switch (code) {
        case 8:     // BackSpace
        case 9:     // Tab
        case 46:    // Delete
        case 37:    // 「←」
        case 39:    // 「→」
            return true;
            break;
    }
    return false;
}

/**
* 入力された文字が数値（0～9）かどうかを返す
* @return {}
*/
function isNumericKey(e) {
    if (isSpecialKey(e)) {
        return true;
    }
    var code = getCharCode(e);
    if (48 <= code && code <= 57 && !shiftKey(e)) {
        // 「0」～「9」
        return true;
    }
    if (96 <= code && code <= 105) {
        // 「0」～「9」（テンキー）
        return true;
    }
    return false;
}

/**
* 入力されたキーコードを取得する
* @return {}
*/
function getCharCode(e) {
    var e = e || window.event;
    var code = e.charCode || e.keyCode;
    return code;
}

/**
* Shiftキーの有無を返す
* @return {}
*/
function shiftKey(e) {
    var e = e || window.event;
    return e.shiftKey;
}

/**
* Shiftキーの有無を返す
* @return {}
*/
function trim(value) {
    value = value.replace(/(^\s+)|(\s+$)/g, "");
    return value;
}

// 2014/07/17　電卓キー表示対応　START　↓↓↓
///**
//* フォーカスイン
//* @return {}
//*/
//function valueCnv_OnFocus(obj, pos) {
//    beforeText = document.getElementById('BeforeText').value;
//    afterText = document.getElementById('AfterText').value;
//    value = trim(obj.value);
//    if (1 == pos) {
//        // Before
//        if (beforeText == value) {
//            value = "";
//        }
//    } else {
//        // After
//        if (afterText == value) {
//            value = "";
//        }
//    }
//    obj.value = value
//}
//
///**
//* フォーカスアウト
//* @return {}
//*/
//function valueCnv_OnBlur(obj, pos) {
//    beforeText = document.getElementById('BeforeText').value;
//    afterText = document.getElementById('AfterText').value;
//    value = trim(obj.value);
//    if (1 == pos) {
//        // Before
//        if ("" == value) {
//            value = beforeText;
//        } else if (false == isNaN(value)) {
//            if (0 == value) {
//                // 0は入力可能(0⇒0.00、000⇒0.00、0.000⇒0.00等の入力に対応)
//                value = "0.00";
//            } else if (1000 <= value) {
//                // 1000.0以上
//                value = beforeText;
//            } else {
//                // 小数点第二位未満の桁を削除
//                var pos = value.lastIndexOf('.');
//                if (pos < 0) {
//                    value = value + ".00";
//                } else {
//                    value = (value + "00").substring(0, pos + 3);
//                }
//            }
//        } else {
//            value = beforeText;
//        }
//    } else {
//        // After
//        if ("" == value) {
//            value = afterText;
//        } else if (false == isNaN(value)) {
//            if (0 == value) {
//                // 0は入力可能(0⇒0.00、000⇒0.00、0.000⇒0.00等の入力に対応)
//                value = "0.00";
//            } else if (1000 <= value) {
//                // 1000.0以上
//                value = afterText;
//            } else {
//                // 小数点第二位未満の桁を削除
//                var pos = value.lastIndexOf('.');
//                if (pos < 0) {
//                    value = value + ".00";
//                } else {
//                    value = (value + "00").substring(0, pos + 3);
//                }
//            }
//        } else {
//            value = afterText;
//        }
//    }
//    obj.value = value
//
//    /* 全体の項目チェック */
//    allcheck();
//}

/**
* 電卓キー表示時処理
* @return {}
*/
function valueCnv_OnFocus(obj, defaultText) {

    // 対象項目から値を取得し、デフォルト表示文言かチェック
    value = $(obj).val();
    if (defaultText == value) {
        value = "";
    }

    // 電卓キーへ初期値セット
    $(obj).NumericKeypad("setValue", value);
}

/**
* 電卓キーOKボタン押下時処理
* @return {}
*/
function valueCnv_OnBlur(obj, value, defaultText) {

    if ("" == value) {
        value = defaultText;
    } else if (false == isNaN(value)) {
        if (0 == value) {
            //// 0は入力可能(0⇒0.00、000⇒0.00、0.000⇒0.00等の入力に対応)
            //value = "0.00";
            // 0は入力可能
            value = "0";
        } else if (1000 <= value) {
            // 1000.0以上
            value = defaultText;
        } else {
            // 小数点第二位未満の桁を削除
            var pos = value.lastIndexOf('.');
            if (pos < 0) {
                //value = value + ".00";
            } else {
                //value = (value + "00").substring(0, pos + 3);
                value = (value).substring(0, pos + 3);
            }
        }
    } else {
        value = defaultText;
    }

    $(obj).val(value);

    /* 全体の項目チェック */
    allcheck();
}
// 2014/07/17　電卓キー表示対応　END　　↑↑↑

// 【***完成検査_排他制御***】 start
/* 排他エラーが存在すれば出す */
function initDisplay(){
    icropScript.ShowMessageBox = function (code, word, detail, origin) {
        var body = $("body"),
            backGround = $("<div class='icrop-message-background'></div>");
        backGround.width(body.width()).height(body.height());
        body.append(backGround);
        setTimeout(function(){
            alert(word); backGround.remove();
        }, 1000)
    };
}
// 【***完成検査_排他制御***】 end

