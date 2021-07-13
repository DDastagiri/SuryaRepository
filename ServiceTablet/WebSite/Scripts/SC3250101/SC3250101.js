// 整備選択画面に表示するアイコン
var images = [
    "Icon_Inspaction"
    , "Icon_Replace_Black"
    , "Icon_Fixing"
    , "Icon_Swapping"
    , "Icon_Cleaning"
    , "Icon_None"
    , "Icon_Reset"
    , "Icon_Replace_Red"
];

//2014/06/23　拡大画面のアイコン変更　START　↓↓↓
var imagesPop = [
    "Icon_Inspaction_Pop"
    , "Icon_Replace_Black_Pop"
    , "Icon_Fixing_Pop"
    , "Icon_Swapping_Pop"
    , "Icon_Cleaning_Pop"
    , "Icon_None_Pop"
    , "Icon_Reset_Pop"
    , "Icon_Replace_Red_Pop"
];
//2014/06/23　拡大画面のアイコン変更　END　　↑↑↑

var SUGGEST_CHANGE_FLAG_ON = '1';
var SUGGEST_CHANGE_FLAG_OFF = '0';

var INSPEC_ITEM_CD = 0;
var SUGGEST_ICON = 1;
var SUGGEST_STATUS = 2;
var CHANGE_FLAG = 3;
var DEFAULT_STATUS = 4;
var BEFORE_STATUS = 5;

var SelectedItemNo;
var SelectedElementId;

var TitleOneLine = 'TbaleTitleLine OneLine';
var TitleOneLineRed = 'TbaleTitleLine OneLineRed';
var TitleTwoLine = 'TbaleTitleLine TwoLine';
var TitleTwoLineRed = 'TbaleTitleLine TwoLineRed';

$(function () {
    //フッターアプリの起動設定
    SetFooterApplication();

    //フッター「顧客詳細ボタン」クリック時の動作
    $('#MstPG_FootItem_Main_700').bind("click", function (event) {
        //ヘッダーの顧客検索にフォーカスを当てる
        $('#MstPG_CustomerSearchTextBox').focus();
        event.stopPropagation();
    });

    //スクロール機能設定

    $("#popUpListIn").fingerScroll();
    $("#scrollArea01").fingerScroll();
    $("#scrollArea02").fingerScroll();
    $("#scrollArea03").fingerScroll();
    $("#scrollArea04").fingerScroll();
    $("#scrollArea05").fingerScroll();
    $("#scrollArea06").fingerScroll();
    $("#scrollArea07").fingerScroll();
    $("#scrollArea08").fingerScroll();
    $("#scrollArea09").fingerScroll();
    $("#roAdviceboxContens").fingerScroll();

    //ヘッダーの戻るボタンをタップしたときにクルクルを表示する
/*    $('#MstPG_BackLinkButton').bind("click", function () {
        ActiveDisplayOn();
    });
    */
});


/**
* フッターボタンのクリックイベント.
* @return {}
*/
function FooterButtonClick(Id) {
    //顧客ボタン、TCメインボタンの場合は何もしない
    if (Id == 700 || Id == 200) {
        return false;
    }

    //全体クルクル表示
    ActiveDisplayOn();

    //タイマーセット
    commonRefreshTimer(function () { __doPostBack("", ""); });
    //各イベント処理実行
    switch (Id) {
        case 100:
            //メインメニュー
            __doPostBack('ctl00$MstPG_FootItem_Main_100', '');
            break;
        case 300:
            //FMメインメニュー
            __doPostBack('ctl00$MstPG_FootItem_Main_300', '');
            break;
        case 400:
            //予約管理
            __doPostBack('ctl00$MstPG_FootItem_Main_400', '');
            break;
        case 500:
            //R/O
            __doPostBack('ctl00$MstPG_FootItem_Main_500', '');
            break;
        case 600:
            //連絡先
            __doPostBack('ctl00$MstPG_FootItem_Main_600', '');
            break;
        case 700:
            //顧客
            __doPostBack('ctl00$MstPG_FootItem_Main_700', '');
            break;
        case 800:
            //商品訴求コンテンツ
            __doPostBack('ctl00$MstPG_FootItem_Main_800', '');
            break;
        case 900:
            //キャンペーン
            __doPostBack('ctl00$MstPG_FootItem_Main_900', '');
            break;
        case 1000:
            //全体管理
            __doPostBack('ctl00$MstPG_FootItem_Main_1000', '');
            break;
        case 1100:
            //SMB
            __doPostBack('ctl00$MstPG_FootItem_Main_1100', '');
            break;
        case 1200:
            //追加作業
            __doPostBack('ctl00$MstPG_FootItem_Main_1200', '');
            break;
    }
}


//【追加要件１】．今回の点検以外の点検を選択できるようにする　START　↓↓↓
//Suggestリストボックスから表示する点検種類を選択
function OnChangeDdlSuggest() {
    //変更内容があれば確認メッセージを表示する処理のメソッドを作成
    var DeleteCheck = function () {
        var AlreadySendFlag = document.getElementById("hdnAlreadySendFlag");
        //Webサービス送信済み　→　実績データが登録されていれば確認メッセージを表示
        if (0 < parseInt(AlreadySendFlag.value)) {
            var confirmMessage = document.getElementById("ClearCartMessageID").value
            answer = confirm(confirmMessage);
            if (answer == true) {
                //「Yes」タップ　→　データベース、カートのクリア実行をサーバサイドにて実行
                hdnProcMode.value = "";
                //全体クルクル表示
                ActiveDisplayOn();
            } else {
                //「NO」タップ　→　Suggestの変更をキャンセルして戻る
                //Suggestドロップダウンの値を変更前に戻す
                var SuggestList = document.getElementById("ddlSuggest");
                var SuggestValue = document.getElementById("hdnSuggest");
                for (i = 0; i < SuggestList.length; i++) {
                    if (SuggestList.options[i].value == SuggestValue.value) {
                        SuggestList.options[i].selected = true;
                        break;
                    }
                }
                return true;
            }
        } else {
            hdnProcMode.value = "";
            //全体クルクル表示
            ActiveDisplayOn();
        }

        this_form.submit();
    }

    //一定時間後に処理を開始する
    //（すぐに確認メッセージを表示するとフリーズしてしまうため）
    var timer = setTimeout(DeleteCheck, 100);

}
//【追加要件１】．今回の点検以外の点検を選択できるようにする　END　　↑↑↑


// 部位拡大画面を閉じる
function ClosePopUp() {
    var contentsMainonBoard = document.getElementById("contentsMainonBoard");
    var closeBtn = document.getElementById("closeBtn");
    var popUp = document.getElementById("popUp");
    var popUpList = document.getElementById("popUpList");
    var balloon_top_back = document.getElementById("balloon_top_back");
    var balloon_top_front = document.getElementById("balloon_top_front");
    var floatBox = document.getElementById("floatBox");

    contentsMainonBoard.style.display = "none";
    closeBtn.style.display = "none";
    popUp.style.display = "none";
    popUpList.style.display = "none";
    balloon_top_back.style.display = "none";
    balloon_top_front.style.display = "none";
    floatBox.style.display = "none";

    //for (var i = 0; i < 9; i++) {
    //選択されていた部位の明細部テーブルのZ-Indexをクリアする
    document.getElementById("list" + document.getElementById("hdnClickedListNo").value).style.zIndex = "";
    //}
    // 明細部分の部位テーブルの整備選択コントロールを選択している行に選択したアイテムのインデックスを保持
    //var ListData = document.getElementById("List" + hdnClickedListNo.value + "_Data");
    //ListData.rows[hdnClickedRowNo.value].cells[2].children[0].src = images[SelectedItemNo.value - 1];

    // 2014/06/02 レスポンス対策　START　↓↓↓

    //for (var i = 0; i < ListData.rows.length; i++) {
    //    if (ListData.rows[i].cells[2].children[1].value != "") {
    //        ListData.rows[i].cells[2].children[0].className = images[ListData.rows[i].cells[2].children[1].value];
    //        if (ListData.rows[i].cells[2].children[1].value == "1" && ListData.rows[i].cells[2].children[4].value != "0") {
    //            ListData.rows[i].cells[2].children[0].className = images[7];
    //        }
    //    }
    //}

    ////2014/06/25 強く推奨アイコンがあったら色を赤色にする　START　↓↓↓
    //部位名の色を標準色に戻す
    //var ListDataheader = document.getElementById("List" + hdnClickedListNo.value + "_Col1_Title");
    //if (ListDataheader.className == TitleOneLineRed) {
    //    ListDataheader.className = TitleOneLine;
    //} else if (ListDataheader.className == TitleTwoLineRed) {
    //    ListDataheader.className = TitleTwoLine;
    //}
    //2014/06/25 強く推奨アイコンがあったら色を赤色にする　END　　↑↑↑
    //for (var i = 0; i < ListData.rows.length; i++) {
    //    var SuggestInfos = ListData.rows[i].cells[2].children[1].value;
    //    //「,」で分離し、配列SuggestInfoに格納する
    //    var SuggestInfo = SuggestInfos.split(",");
    //    if (SuggestInfo[SUGGEST_ICON] != "") {
    //        ListData.rows[i].cells[2].children[2].className = images[SuggestInfo[SUGGEST_ICON]];
    //        if (SuggestInfo[SUGGEST_ICON] == "1" && SuggestInfo[SUGGEST_STATUS] != "0") {
    //            ListData.rows[i].cells[2].children[2].className = images[7];
    //            //2014/06/23 ヘッダーの色も再表示するように追加　START　↓↓↓
    //            var ListDataheader = document.getElementById("List" + hdnClickedListNo.value + "_Col1_Title");
    //            if (ListDataheader.className == TitleOneLine) {
    //                ListDataheader.className = TitleOneLineRed;
    //            } else if (ListDataheader.className == TitleTwoLine) {
    //               ListDataheader.className = TitleTwoLineRed;
    //            }
    //            //2014/06/23 ヘッダーの色も再表示するように追加　END　　↑↑↑
    //        }
    //    }
    //}

    //2014/06/02 レスポンス対策　END　　↑↑↑

    hdnProcMode.value = "";

    //変更されたアイテムがあれば「Register」ボタンを有効化する
    var hdnChangeFlg = document.getElementById("hdnChangeFlg");
    var RegisterImg = document.getElementById("imgRegister");
    if (0 < parseInt(hdnChangeFlg.value)) {
        RegisterImg.className = "Register_Enable";
    } else {
        // 2014/06/12 Registerボタン活性・非活性処理修正　START　↓↓↓
        RegisterImg.className = "Register_Disable";
        // 2014/06/12 Registerボタン活性・非活性処理修正　END　　↑↑↑
    }

}

// 整備選択コントロール表示
function ShowBalloon(elementId, row_no) {
    SelectedElementId = elementId;
    var hdnClickedRowNo = document.getElementById("hdnClickedRowNo");
    hdnClickedRowNo.value = row_no;
    var SelectedCell = document.getElementById("popUpDetail").rows[row_no].cells[2];

    //データを分割して格納する配列painの作成
    var NeedIcon = new Array();

    //選択された商品データを変数NeedIconsに入れる
    var NeedIcons = SelectedCell.children[1].value;

    //「,」で分離し、配列NeedIconに格納する
    NeedIcon = NeedIcons.split(",");

    //Need Inspectionアイコン
    //var NeedIN = SelectedCell.children[1];
    //if (NeedIN.value == "1") {
    if (NeedIcon[0] == "1") {
        // 表示
        document.getElementById("li1").style.display = "block";
    } else {
        // 非表示
        document.getElementById("li1").style.display = "none";
    }

    //Need Replaceアイコン
    //var NeedRP = SelectedCell.children[2];
    //if (NeedRP.value == "1") {
    if (NeedIcon[1] == "1") {
        // 表示
        document.getElementById("li2").style.display = "block";
    } else {
        // 非表示
        document.getElementById("li2").style.display = "none";
    }

    //Need Fixアイコン
    //var NeedFX = SelectedCell.children[3];
    //if (NeedFX.value == "1") {
    if (NeedIcon[2] == "1") {
        // 表示
        document.getElementById("li3").style.display = "block";
    } else {
        // 非表示
        document.getElementById("li3").style.display = "none";
    }

    //Need Cleanアイコン
    //var NeedCL = SelectedCell.children[4];
    //if (NeedCL.value == "1") {
    if (NeedIcon[3] == "1") {
        // 表示
        document.getElementById("li5").style.display = "block";
    } else {
        // 非表示
        document.getElementById("li5").style.display = "none";
    }

    //Need Swapアイコン
    //var NeedSW = SelectedCell.children[5];
    //if (NeedSW.value == "1") {
    if (NeedIcon[4] == "1") {
        // 表示
        document.getElementById("li4").style.display = "block";
    } else {
        // 非表示
        document.getElementById("li4").style.display = "none";
    }

    //var SelectedCell = document.getElementById(elementId);

    //クリックしたセルの座標を求める
    var bounds = SelectedCell.getBoundingClientRect();

    //吹き出し本体を表示する
    var floatBox = document.getElementById("floatBox");
    floatBox.style.display = "block";

    //吹き出し本体の座標をセット
    var floatbox_top = (bounds.top + 36);
    var floatbox_left = (bounds.left - (floatBox.offsetWidth / 2) + 30);
    floatBox.style.top = floatbox_top + "px";
    floatBox.style.left = floatbox_left + "px";

    //吹き出し上部の凸部の座標をセット
    var balloon_top_back = document.getElementById("balloon_top_back");
    var balloon_top_front = document.getElementById("balloon_top_front");
    balloon_top_back.style.display = "block";
    balloon_top_front.style.display = "block";

    var back_top = bounds.top + 27;
    var back_left = bounds.left + 21;
    var front_top = bounds.top + 29;
    var front_left = bounds.left + 23;
    balloon_top_back.style.top = back_top + "px";
    balloon_top_back.style.left = back_left + "px";
    balloon_top_front.style.top = front_top + "px";
    balloon_top_front.style.left = front_left + "px";

    var display_right = contentsMainonBoard.offsetTop + contentsMainonBoard.offsetWidth;
    var floatBox_right = floatBox.offsetLeft + floatBox.offsetWidth;
    var right_diff = floatBox_right - display_right;
    if (floatBox_right > display_right) {
        floatBox.style.left = (floatbox_left - right_diff) + "px";
    }
    // 押下状態の背景イメージをクリア
    var Items = document.getElementById("Items");
    for (var i = 0; i < Items.children.length; i++) {
        Items.children[i].className = "";
    }
    // 押下された行を保持する
    //SelectedItemNo = document.getElementById("hdn_" + elementId);
    SelectedItemNo = SelectedCell.children[2];
    // 押下済のアイテムの背景色に押下状態にする
    var hdnClickedListNo = document.getElementById("hdnClickedListNo");
    var ListData = document.getElementById("List" + hdnClickedListNo.value + "_Data");
    // 該当行で選択されているアイテムのインデックス取得
    
    //2014/06/02 レスポンス対策　START　↓↓↓

    //var SelectedItemNoValue = ListData.rows[hdnClickedRowNo.value].cells[2].children[1].value;

    //if (SelectedItemNoValue != "") {
    //    document.getElementById("li" + (Number(SelectedItemNoValue) + 1)).className = "push";
    //}

    var SuggestInfos = ListData.rows[hdnClickedRowNo.value].cells[2].children[1].value;
    //「,」で分離し、配列SuggestInfoに格納する
    var SuggestInfo = SuggestInfos.split(",");

    if (SuggestInfo[SUGGEST_ICON] != "") {
	if (Number(SuggestInfo[SUGGEST_ICON]) == 4) {
            document.getElementById("li" + (Number(SuggestInfo[SUGGEST_ICON]) + 1)).className = "pushClean";
	} else {
            document.getElementById("li" + (Number(SuggestInfo[SUGGEST_ICON]) + 1)).className = "push";
        }
    }

    //2014/06/02 レスポンス対策　END　　↑↑↑

}

// 整備選択コントロールでアイテム選択時の処理
function SelectItem(ItemNo) {
    var Items = document.getElementById("Items");
    for (var i = 0; i < Items.children.length; i++) {
        if (i == ItemNo) {
            // 選択されたアイテムの背景色を選択状態にする
            // itemNo=4 Cleanの場合
            if (i == 4) {
                Items.children[i].className = "pushClean";
            } else {
                Items.children[i].className = "push";
            }
        } else {
            // 選択されたアイテムの背景色を未選択状態にする
            Items.children[i].className = "";
        }
    }
    // 選択されたアイテムのインデックスを保持
    SelectedItemNo.value = ItemNo + 1;
    // 整備選択コントロールを非表示にする
    document.getElementById("balloon_top_back").style.display = "none";
    document.getElementById("balloon_top_front").style.display = "none";
    document.getElementById("floatBox").style.display = "none";
    // 部位選択画面の整備選択コントロールを選択している行の表示イメージを変更
    //document.getElementById(SelectedElementId + "_img").src = images[ItemNo];
    var hdnClickedRowNo = document.getElementById("hdnClickedRowNo");
    var SelectedCell = document.getElementById("popUpDetail").rows[hdnClickedRowNo.value].cells[2];

    // 明細部分の部位テーブルの整備選択コントロールを選択している行に選択したアイテムのインデックスを保持
    var hdnClickedListNo = document.getElementById("hdnClickedListNo");
    var hdnClickedRowNo = document.getElementById("hdnClickedRowNo");
    var ListData = document.getElementById("List" + hdnClickedListNo.value + "_Data");

    var SuggestInfos = ListData.rows[hdnClickedRowNo.value].cells[2].children[1].value;
    //「,」で分離し、配列SuggestInfoに格納する
    var SuggestInfo = SuggestInfos.split(",");

    // 2014/06/05　Resetボタンタップ時の処理　START　↓↓↓
    //2014/06/23　拡大画面のアイコン変更　START　↓↓↓
    if (ItemNo == 6) {
        if (SuggestInfo[DEFAULT_STATUS] == "1" && SuggestInfo[SUGGEST_STATUS] != "0") {
            SelectedCell.children[0].className = imagesPop[7];
        } else {
            SelectedCell.children[0].className = imagesPop[SuggestInfo[DEFAULT_STATUS]];
        }
    } else {
        //2014/06/25 強く推奨アイコンがあったら色を赤色にする　START　↓↓↓
        if (ItemNo == "1" && SuggestInfo[SUGGEST_STATUS] != "0") {
            SelectedCell.children[0].className = imagesPop[7];
        } else {
            SelectedCell.children[0].className = imagesPop[ItemNo];
        }
        //SelectedCell.children[0].className = imagesPop[ItemNo];
        //2014/06/25 強く推奨アイコンがあったら色を赤色にする　END　　↑↑↑
    }
    //2014/06/23　拡大画面のアイコン変更　　END　↑↑↑
    // 2014/06/05　Resetボタンタップ時の処理　END　　↑↑↑

    //2014/06/02 レスポンス対策　START　↓↓↓

    //var exItemno = ListData.rows[hdnClickedRowNo.value].cells[2].children[1].value; // 変更前アイテム値退避
    //ListData.rows[hdnClickedRowNo.value].cells[2].children[1].value = ItemNo;       // SUGGEST_ICONの設定
    //ListData.rows[hdnClickedRowNo.value].cells[2].children[4].value = "0";          // SUGGEST_STATUSを「0」にする
    //// 明細部分の部位テーブルの整備選択コントロールを選択している行に変更フラグをセット。
    //// このフラグが「1」である場合、DB登録・更新対象となる
    //exItemno = parseInt(exItemno) + 1;
    //var hdnViewMode = document.getElementById("hdnViewMode");
    //
    //if ((hdnViewMode.value == "0") && (exItemno != SelectedItemNo.value)) {
    //    ListData.rows[hdnClickedRowNo.value].cells[2].children[2].value = "1";      //hdnChangeFlagを「1」にする
    //    var hdnChangeFlg = document.getElementById("hdnChangeFlg");
    //    hdnChangeFlg.value = "1";                                                   //hdnChangeFlgを「1」にする
    //}

    var exItemno = SuggestInfo[SUGGEST_ICON];                                                    // 変更前アイテム値退避
    // 2014/06/05　Resetボタンタップ時の処理　START　↓↓↓
    if (ItemNo == 6){
        SuggestInfo[SUGGEST_ICON] =  SuggestInfo[DEFAULT_STATUS];                                    // SUGGEST_ICONの設定
        ItemNo = parseInt(SuggestInfo[DEFAULT_STATUS]);
    } else {
        SuggestInfo[SUGGEST_ICON] = ItemNo;                                                          // SUGGEST_ICONの設定
        //SuggestInfo[SUGGEST_STATUS] = "0";                                                           // SUGGEST_STATUSを「0」にする
    }
    // 2014/06/05　Resetボタンタップ時の処理　END　　↑↑↑

    // 明細部分の部位テーブルの整備選択コントロールを選択している行に変更フラグをセット。
    // このフラグが「1」である場合、DB登録・更新対象となる
    //exItemno = parseInt(exItemno) + 1;
    var hdnViewMode = document.getElementById("hdnViewMode");
    var hdnChangeFlg = document.getElementById("hdnChangeFlg");

    if ((hdnViewMode.value == "0") && (exItemno != ItemNo)) {
        if(SuggestInfo[CHANGE_FLAG] == "2"){
            //一時WKに保存しているデータのため、変更フラグはそのままにする
            SuggestInfo[CHANGE_FLAG] = "2"
        } else if(SuggestInfo[CHANGE_FLAG] == "1" && ItemNo == SuggestInfo[BEFORE_STATUS]){
            //最初の状態に戻したため、変更フラグを0に戻す
            SuggestInfo[CHANGE_FLAG] = "0";
            hdnChangeFlg.value = parseInt(hdnChangeFlg.value) - 1;                                                   //hdnChangeFlgを「1」にする
        } else {
            SuggestInfo[CHANGE_FLAG] = "1";                                                       //hdnChangeFlagを「1」にする
            hdnChangeFlg.value = parseInt(hdnChangeFlg.value) + 1;                                                   //hdnChangeFlgを「1」にする
        }
    }

    ListData.rows[hdnClickedRowNo.value].cells[2].children[1].value = SuggestInfo[INSPEC_ITEM_CD] + ',' 
                                                                     + SuggestInfo[SUGGEST_ICON] + ',' 
                                                                     + SuggestInfo[SUGGEST_STATUS] + ',' 
                                                                     + SuggestInfo[CHANGE_FLAG] + ',' 
                                                                     + SuggestInfo[DEFAULT_STATUS] + ',' 
                                                                     + SuggestInfo[BEFORE_STATUS];

    //2014/06/02 レスポンス対策　END　　↑↑↑

    //【追加要件２】Resultアイコンの種類によって部位名を赤で表示する　START　↓↓↓
    //Result欄の推奨フラグを取得する
    var ResultFlag = document.getElementById("ResultFlag" + hdnClickedListNo.value);
    //【追加要件２】Resultアイコンの種類によって部位名を赤で表示する　END　　↑↑↑

    //2014/06/25 強く推奨アイコンがあったら色を赤色にする　START　↓↓↓
    //部位名の色を標準色に戻す
    var ListDataheader = document.getElementById("List" + hdnClickedListNo.value + "_Col1_Title");
    if (ListDataheader.className == TitleOneLineRed) {
        //【追加要件２】Resultアイコンの種類によって部位名を赤で表示する　START　↓↓↓
        if (ResultFlag.value == "0") {
            ListDataheader.className = TitleOneLine;
        }
        //【追加要件２】Resultアイコンの種類によって部位名を赤で表示する　END　　↑↑↑
    } else if (ListDataheader.className == TitleTwoLineRed) {
        //【追加要件２】Resultアイコンの種類によって部位名を赤で表示する　START　↓↓↓
        if (ResultFlag.value == "0") {
            ListDataheader.className = TitleTwoLine;
        }
        //【追加要件２】Resultアイコンの種類によって部位名を赤で表示する　END　　↑↑↑
    }
    //拡大画面の部位名の色を標準に戻す
    var PopUpHeader = document.getElementById("popUpTitle");
    //【追加要件２】Resultアイコンの種類によって部位名を赤で表示する　START　↓↓↓
    if (ResultFlag.value == "0") {
        PopUpHeader.className = '';
    }
    //【追加要件２】Resultアイコンの種類によって部位名を赤で表示する　END　　↑↑↑

    for (var i = 0; i < ListData.rows.length; i++) {
        var SuggestInfos = ListData.rows[i].cells[2].children[1].value;
        //「,」で分離し、配列SuggestInfoに格納する
        var SuggestInfo = SuggestInfos.split(",");
        if (SuggestInfo[SUGGEST_ICON] != "") {
            ListData.rows[i].cells[2].children[2].className = images[SuggestInfo[SUGGEST_ICON]];
            if (SuggestInfo[SUGGEST_ICON] == "1" && SuggestInfo[SUGGEST_STATUS] != "0") {
                ListData.rows[i].cells[2].children[2].className = images[7];
                if (ListDataheader.className == TitleOneLine) {
                    ListDataheader.className = TitleOneLineRed;
                } else if (ListDataheader.className == TitleTwoLine) {
                    ListDataheader.className = TitleTwoLineRed;
                }
                PopUpHeader.className = 'OneLineRed';
            }
        }
    }
    //2014/06/25 強く推奨アイコンがあったら色を赤色にする　　END　　↑↑↑
    
}

// ROプレビューボタン押下時の処理
//function ShowROPreview() {
//    var answer;
//    var hdnChangeFlg = document.getElementById("hdnChangeFlg");

//    if (0 < parseInt(hdnChangeFlg.value)) {
//        var confirmMessage = document.getElementById("ClearMessageID").value
//        answer = confirm(confirmMessage);
//        if (answer == true) {
//            hdnProcMode.value = "ShowROPreview";
//        } else {
//            hdnProcMode.value = "ShowROPreview_Save";
//        }
//    } else {
//        hdnProcMode.value = "ShowROPreview";
//    }
//    ActiveDisplayOn();
//    this_form.submit();
//}


// 点検項目ボタン押下時の処理
function OnClickItem(ListNo) {
    ActiveDisplayOn();
    // 押下された部位テーブルのインデックスを保持
    var hdnClickedListNo = document.getElementById("hdnClickedListNo");
    // 処理モード:ShowPopUpにセットし、サーバサイドにて部位拡大画面表示処理を実行
    var hdnProcMode = document.getElementById("hdnProcMode");
    hdnClickedListNo.value = ListNo;
    hdnProcMode.value = "ShowPopUp";
    this_form.submit();
}

// Registerボタン押下時処理
function OnClickRegister() {

    var hdnViewMode = document.getElementById("hdnViewMode");
    var RegisterImg = document.getElementById("imgRegister");
    if (RegisterImg.className == "Register_Enable") {
        //変更フラグを0に戻す
        var hdnChangeFlg = document.getElementById("hdnChangeFlg");
        hdnChangeFlg.value = "0";
        //クルクル表示
        ActiveDisplayOn();
        // 処理モード:Registerにセットし、サーバサイドにてDB登録処理を実行
        var hdnProcMode = document.getElementById("hdnProcMode");
        hdnProcMode.value = "Register";
        this_form.submit();
    }
}

// カートボタン押下時処理
function OnClickCart() {
    var hdnViewMode = document.getElementById("hdnViewMode");
    var CartImg = document.getElementById("imgCart");
    if (hdnViewMode.value == "0") {
        if (CartImg.className == "Cart_Enable") {
            ActiveDisplayOn();
            // 処理モード:ShowCartにセットし、サーバサイドにてDB登録処理を実行
            var ListData = document.getElementById("List" + hdnClickedListNo.value + "_Data");
            var hdnProcMode = document.getElementById("hdnProcMode");
            hdnProcMode.value = "ShowCart";
            this_form.submit();
        }
    }
}

// フッターボタンの2度押し制御
function FooterButtonControl() {

    var answer;
    var hdnChangeFlg = document.getElementById("hdnChangeFlg");

    if (0 < parseInt(hdnChangeFlg.value)) {
        var confirmMessage = document.getElementById("ClearMessageID").value
        answer = confirm(confirmMessage);
        if (answer == true) {
            hdnProcMode.value = "";
        } else {
            hdnProcMode.value = "SaveWK";
        }
    } else {
        hdnProcMode.value = "";
    }

    $.master.OpenLoadingScreen();
    return true;
}

/**
* フッター部のアプリ
* @return {void}
*/
function SetFooterApplication() {

    //スケジュールオブジェクトを拡張
    $.extend(window, { schedule: {} });

    //スケジュールオブジェクトに関数追加
    $.extend(schedule, {

        /**
        * @class アプリ起動クラス
        */
        appExecute: {

            /**
            * カレンダーアプリ起動(単体)
            */
            executeCaleNew: function () {
                window.location = "icrop:cale:";
                return false;
            },
            /**
            * 電話帳アプリ起動(単体)
            */
            executeCont: function () {
                window.location = "icrop:cont:";
                return false;
            }
        }

    });
}

// URLスキームにてポップアップ表示
function ShowUrlSchemePopup(url) {
    // 下記のようにしてもポップアップ表示されません、試行錯誤中です。
    //window.location = "icrop:noTitlePopup?url=" + url + "&x=20&y=20&w=1024&h=655&eventFunc=eventFunc&enFunc=endFunc&close=NO:";
    //カレンダーアプリは起動しました。
    //window.location = "icrop:cale:";
    //このやり方でポップアップ表示出来た 
    //window.location = "icrop:iurl:20::73::980::624::0::" + url;
    window.location = url;
    return false;
}

// 車両ロゴの取得に失敗した
function ImageErrorFunc() {
    var ImageLogo = document.getElementById("ImageLogo");
    ImageLogo.style.display = "none";
}

// 2014/07/09　カメラポップアップ表示処理変更（SC3170209のjsを参考）　START　↓↓↓
/**
* 写真選択ポップアップ画面表示
*/
function ShowUrlSchemeNoTitlePopup(targetparam) {

    // タイトルバー無しポップアップ
    //var params = targetparam.slice(targetparam.indexOf('?'), targetparam.length);
    var strUrl = window.location.href;
    var target = strUrl.slice(0, strUrl.slice(0, strUrl.slice(0, strUrl.indexOf('?')).lastIndexOf('/')).lastIndexOf('/')) + "/SC3170210.aspx" + targetparam;

    var scheme = "icrop:noTitlePopup?";
    scheme += "url=" + target;
    scheme += "::x=0";
    scheme += "::y=56";
    scheme += "::w=1024";
    scheme += "::h=656";

    window.location.href = scheme;
}
// 2014/07/09　カメラポップアップ表示処理変更（SC3170209のjsを参考）　END　　↑↑↑

// 2014/08/04　部品説明画面遷移処理追加　START　↓↓↓
// 拡大画面の点検項目欄タップ
function OnClickPartsDetail(InspecItemCD) {

    //クリア確認ダイアログを表示するか確認
    var hdnChangeFlg = document.getElementById("hdnChangeFlg");

    if (0 < parseInt(hdnChangeFlg.value)) {
        //確認ダイアログ表示
        var confirmMessage = document.getElementById("ClearMessageID").value
        var answer = confirm(confirmMessage);
        if (answer == true) {
            hdnProcMode.value = "ShowPartsDetail";
        } else {
            hdnProcMode.value = "SaveWK_ShowPartsDetail";
        }
    } else {
        //確認ダイアログ表示なし
        hdnProcMode.value = "ShowPartsDetail";
    }

    //クルクル表示
    ActiveDisplayOn();

    //点検項目コードをセット
    var hdnClickedInspecCD = document.getElementById("hdnClickedInspecCD");
    hdnClickedInspecCD.value = InspecItemCD;

    //ポストバック
    this_form.submit();
}
// 2014/08/04　部品説明画面遷移処理追加　END　　↑↑↑


function eventFunc() {
}
function endFunc() {
}
function endPhoto() {
}

// 【***完成検査_排他制御***】 start
/* 排他エラーが存在すれば表示 */
function initDisplay(){
    //公開API
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
