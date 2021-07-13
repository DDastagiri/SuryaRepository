/** 
* @fileOverview SC3240501.js(新規予約作成)
* 
* @author TMEJ 下村
* @version 1.0.0
* 更新： 2014/07/18 TMEJ 明瀬 休憩取得ポップアップ文言文字化け対応
* 更新： 2014/09/12 TMEJ 張 BTS-390 「1ｽﾄｰﾙで2ﾁｯﾌﾟが作業中（表示のみ）」対応
* 更新： 2015/04/23 TMEJ 明瀬  DMS連携版サービスタブレット強制納車機能追加開発
* 更新： 2015/06/15 TMEJ 河原  サービス入庫予約情報のバリデーション実行タイミング変更に向けた評価用アプリ作成
* 更新： 2019/08/02 NSK 近藤 (トライ店システム評価)次世代サービスオペレーション効率化に向けた、業務適合性検証
* 更新： 2019/10/31 NSK 鈴木【（FS）MaaSビジネス向けサービス予約の登録オペレーションの効率化に向けた試験研究】[No156]予定入庫日時、予定納車日時の自動セット
* 更新： 2019/12/06 NSK 皆川 (トライ店システム評価)整備作業における休憩時間取得判定向上検証
* 更新： 2020/01/08 NSK 鈴木 TR-V4-BMS-20191001-001 (トライ店システム評価)コールセンター業務における洗車要否判断の効率化検証
*/

/****************************************
 * 定数宣言
 ****************************************/
//メッセージID：異常終了（その他エラー）
var C_NEWCHIP_MESSAGEID_ERROR = "9999";

//テキストエリアの高さデフォルト値
var C_NEWCHIP_TA_DEFAULTHEIGHT = 90;

//テキストエリア(住所)の高さデフォルト値
var C_NEWCHIP_TA_DEFAULTHEIGHT_ADDRESS = 54;

//テキストエリア(住所)の親リスト全体の高さデフォルト値
var C_NEWCHIP_TA_DEFAULTHEIGHT_CUSTINFO = 204;

//テキストエリア(住所)のオフセット値
var C_NEWCHIP_TA_OFFSET_PADDING_ADDRESS = 16;

//ポップアップをチップ左に表示時の吹出し三角の相対left値
var C_NEWCHIP_POP_DISPLEFT_ARROW_X = 362;

//ポップアップをチップ右に表示時の吹出し三角の相対left値
var C_NEWCHIP_POP_DISPRIGHT_ARROW_X = -17;

//ポップアップを画面右端に表示時のポップアップの相対left値
var C_NEWCHIP_POP_DISPRIGHT_DEFAULT_X = 635;

//タッチイベント名
var C_SC3240501_TOUCH = "mousedown touchstart";


/****************************************
* グローバル変数宣言
****************************************/
//新規予約作成表示時のleft値保存用
var gNewChipPopX;

//顧客検索実行済みフラグ (0:検索未、1:検索済)
var gNewChipSearchedFlg;

//顧客検索画面の検索条件Index
var selectSearchTypeIndex = 0;

//非同期処理のトリガーコントロール名称格納配列
var aryPostCtrl = new Array();

$(function () {

    // UpdatePanel処理前後イベント
    $(document).ready(function () {
        var prm = Sys.WebForms.PageRequestManager.getInstance();

        // 開始時のイベント
        prm.add_beginRequest(function () {
        });
        // 終了時のイベント
        prm.add_endRequest(EndRequest);
        function EndRequest(sender, args) {
            if (aryPostCtrl.length > 0) {
                // トリガー配列を取得、削除
                var postBackControl = aryPostCtrl.shift();

                // UpdatePanel更新後処理を行うか判定
                if (IsJudgeEndRequest(postBackControl) == false) {
                    return;
                }

                // 呼び出し元判定（非同期処理のトリガーコントロール名称確認）
                switch (postBackControl) {
                    case 'SearchCustomer':  // 顧客検索ボタン

                        //顧客検索結果表示欄のclick処理
                        $('.SearchData').bind('click', function (event) {
                            $('.SearchData').removeClass('SelectionOn');
                            $(this).addClass('SelectionOn');
                            $('#SearchRegistrationNumberChange').val($(this).children('#SearchRegistrationNumber').text());
                            $('#SearchVinChange').val($(this).children('#SearchVinNo').text());
                            $('#SearchCustomerNameChange').val($(this).children('#SearchCustomerName').text());
                            $('#SearchPhoneChange').val($(this).children('#SearchPhone').text());
                            $('#SearchMobileChange').val($(this).children('#SearchMobile').text());
                        	
                            var ChangeParameterDiv = $(this).children('#CustomerChangeParameter');
                            $('#SearchVehicleChange').val(ChangeParameterDiv.attr("VehicleParameter"));
                            $('#SearchSANameChange').val(ChangeParameterDiv.attr("SaNameParameter"));
                            $('#SearchCustomerAddressChange').val(ChangeParameterDiv.attr("CustomerAddressParameter"));                            
                            $('#SearchDmsCstCodeChange').val(ChangeParameterDiv.attr("DmsCstCodeParameter"));
                            $('#SearchTitleCodeChange').val(ChangeParameterDiv.attr("NameTitleCodeParameter"));
                            $('#SearchTitleChange').val(ChangeParameterDiv.attr("NameTitleNameParameter"));
                            $('#InsertSaCode').val(ChangeParameterDiv.attr("SaCodeParameter"));
                            $('#InsertCstId').val(ChangeParameterDiv.attr("CstIdParameter"));
                            $('#InsertVin').val(ChangeParameterDiv.attr("VinParameter"));
                            $('#InsertVclId').val(ChangeParameterDiv.attr("VclIdParameter"));
                            $('#InsertCstVclType').val(ChangeParameterDiv.attr("CustomerVehicleTypeParameter"));

                            //登録ボタンを活性にする
                            $("#SearchRegisterBtn").attr("disabled", false);

                            //顧客詳細ボタンを活性にする
                            $('#SearchBottomButton').removeClass('BottomButtonDisable');
                        	$("#SearchBottomButton").attr("disabled", false);
                        });
                        break;

                    default:
                        break;
                };
            }
        }
    });
});

/*
* UpdatePanel更新後処理 実行判定
*
* @param {string} postBackCtrl 処理実行フラグ名
* @return {boolean} true:処理を実行 / false:処理をキャンセル
*/
function IsJudgeEndRequest(postBackCtrl) {

    var rtn = true;

    // 同フラグがある場合は処理しない。（最後のフラグで処理）
    if (aryPostCtrl.length > 0) {
        var str = "";
        for (var i = 0; i < aryPostCtrl.length; i++) {
            str = aryPostCtrl[i];

            if (postBackCtrl == str) {
                // 一致した場合処理しない
                rtn = false;
                break;
            }
        }
    }

    return rtn;
}

/**
* アクティブインジケータ表示操作関数定義
* 
*/
var gNewChipActiveIndicator = {
    show: function () {
        $("#NewChipActiveIndicator").addClass("show");
    }
    , 
    hide: function () {
        $("#NewChipActiveIndicator").removeClass("show");
    }
};

/**
* オーバーレイ表示操作関数定義
* 
*/
var gNewChipOverlay = {
    show: function () {
        $("#MstPG_registOverlayBlack").css({
            "display": "block"
            , "z-index": "10002"
            , "opacity": "0.1"
            , "height": "768px"
        });
    }
    ,
    hide: function () {
        $("#MstPG_registOverlayBlack").css({
            "display": "none"
            , "z-index": "4"
            , "opacity": "0"
            , "height": "703px"
        });
    }
};

/**	
* 新規予約作成画面用のキャンセルボタン・登録ボタンを表示、もしくは非表示にする
* 	
*/
function SetNewChipHeaderButton(val) {
    $("#NewChipCancelBtn").css("display", val);
    $("#NewChipRegisterBtn").css("display", val);
}

/**	
* 顧客検索画面用のキャンセルボタン・登録ボタンを表示、もしくは非表示にする
* 	
*/
function SetSearchHeaderButton(val) {
    $("#SearchCancelBtn").css("display", val);
    $("#SearchRegisterBtn").css("display", val);
}

/**
* ポップアップ表示(詳細ボタンクリック時にコール) 
*/
function ShowNewChip() {

    //アクティブインジケータ・オーバーレイ非表示
    gNewChipActiveIndicator.hide();
    gNewChipOverlay.hide();
    $('#SearchDataLoading').css('display', 'none');

    //顧客検索画面用のキャンセルボタン・登録ボタンを非表示にする
    SetSearchHeaderButton("none");

    //顧客検索画面用のヘッダーラベルを非表示にする
    $("#SearchHeaderLabel").css("display", "none");

    //新規予約作成画面用のキャンセルボタン・登録ボタンを表示させる
    SetNewChipHeaderButton("block");

    //新規予約作成画面用のヘッダーラベルを表示させる
    $("#NewChipHeaderLabel").css("display", "inline-block");

    //新規予約作成画面用の登録ボタンを非活性にする
    $("#NewChipRegisterBtn").attr("disabled", true);

    //顧客検索画面用の登録ボタンを非活性にする
    $("#SearchRegisterBtn").attr("disabled", true);

    //新規予約作成のコンテンツを削除
    $('#NewChipContent>div').remove();

    //工程管理画面で選択されているチップ
    var baseCtrl = $("#" + gSelectedChipId);

    //ポップアップの表示位置を設定
    SetNewChipPopoverPosition(CalcNewChipPopoverPosition(baseCtrl));

    //ポップアップ表示
    $("#NewChipPopup").fadeIn(300);

    //アクティブインジケータ表示
    gNewChipActiveIndicator.show();  

    //リフレッシュタイマーセット
    commonRefreshTimer(ReDisplayNewChip);

    //画面初期化情報を取得して作成する
    CreateNewChipPage();
};


/**
* 画面を作成する
* 
*/
function CreateNewChipPage() {

    //サーバーに渡すパラメータを作成
    var prms = CreateNewChipCallBackDisplayParam(C_SC3240501CALLBACK_CREATEDISP, gSelectedChipId);

    //コールバック開始
    DoCallBack(C_CALLBACK_WND501, prms, SC3240501AfterCallBack, "CreateNewChipPage");
}


/**
* コールバックでサーバーに渡すパラメータを作成する
*
* @param {String}   method:   コールバック時のメソッド分岐用
* @param {Integer}  chipId:   チップのID
*
*/
function CreateNewChipCallBackDisplayParam(method, chipId) {

    var rtnVal = {
          Method: method
        , DisplayStartDate: gArrObjChip[C_NEWCHIPID].displayStartDate    //チップ表示用開始時間
        , DisplayEndDate: gArrObjChip[C_NEWCHIPID].displayEndDate        //チップ表示用終了時間

        // 2020/01/08 NSK 鈴木 TR-V4-BMS-20191001-001 (トライ店システム評価)コールセンター業務における洗車要否判断の効率化検証 START
        , StallId: gArrObjChip[C_NEWCHIPID].stallId                      //ストールID
        // 2020/01/08 NSK 鈴木 TR-V4-BMS-20191001-001 (トライ店システム評価)コールセンター業務における洗車要否判断の効率化検証 END
    };

    return rtnVal;
}

/**
* コールバックでサーバーに渡すパラメータを作成する(登録ボタン)
*
* @param {String}   method:   コールバック時のメソッド分岐用
* @param {Integer}  chipId:   チップのID
*
*/
function CreateNewChipCallBackRegistParam(method, chipId, restFlg) {

    //整備種類のDropDownListで選択されているIDを取得
    var e = document.getElementById("NewChipMaintenanceTypeList");
    var regSvcClassId;
    if (e.selectedIndex < 0) {
        regSvcClassId = 0;
    } else {
        //「サービス分類ID,標準作業時間」の文字列を分解
        var svcClassInfo = e.options[e.selectedIndex].value.split(",");
        regSvcClassId = svcClassInfo[0];
    }

    //整備名のDropDownListで選択されているIDを取得
    var f = document.getElementById("NewChipMercList");
    var regMercId;
    if (f.selectedIndex < 0) {
        regMercId = 0;
    } else {
        //「商品ID,標準作業時間」の文字列を分解
        var mercInfo = f.options[f.selectedIndex].value.split(",");
        regMercId = mercInfo[0];
    }

	var regNameTitleCd;
	var regNameTitleName;
	var regSACd;
	if (gNewChipSearchedFlg == 1){
		//顧客検索済みの場合
		//敬称
		regNameTitleCd = $("#SearchTitleCodeChange").val()
		regNameTitleName = $("#SearchTitleChange").val()
		
		//担当SA
		regSACd = $("#InsertSaCode").val()
	} else {
		//敬称のDropDownListで選択されている敬称コードを取得
	    var g = document.getElementById("NewChipTitleList");
	    regNameTitleCd = g.options[g.selectedIndex].value;
		regNameTitleName = g.options[g.selectedIndex].text;
		
		//担当SAのDropDownListで選択されている担当SAコードを取得
	    var h = document.getElementById("NewChipSAList");
	    regSACd = h.options[h.selectedIndex].value;
	}
	
    var rtnVal = {
          Method: method
        , DlrCD: $("#hidDlrCD").val()                                    //販売店コード
        , StrCD: $("#hidBrnCD").val()                                    //店舗コード
        , ShowDate: $("#hidShowDate").val()                              //表示日時(yyyy/MM/dd)
        , Account: icropScript.ui.account                                //ログインアカウント
        , StallId: gArrObjChip[C_NEWCHIPID].stallId                      //ストールID
        //, VisitPlanTime: smbScript.ConvertDateToString2($("#NewChipPlanVisitDateTimeSelector").get(0).valueAsDate)           //来店予定時間
        //, StartPlanTime: smbScript.ConvertDateToString2($("#NewChipPlanStartDateTimeSelector").get(0).valueAsDate)           //開始予定時間
        //, FinishPlanTime: smbScript.ConvertDateToString2($("#NewChipPlanFinishDateTimeSelector").get(0).valueAsDate)         //終了予定時間
        //, DeriveredPlanTime: smbScript.ConvertDateToString2($("#NewChipPlanDeriveredDateTimeSelector").get(0).valueAsDate)   //納車予定時間
    	//, NewChipDispStartDate: $("#NewChipPlanStartDateTimeSelector").get(0).valueAsDate                                    //表示開始日時
        , VisitPlanTime: smbScript.ConvertDateToString2(smbScript.changeStringToDateIcrop($("#NewChipPlanVisitDateTimeSelector").get(0).value))           //来店予定時間
        , StartPlanTime: smbScript.ConvertDateToString2(smbScript.changeStringToDateIcrop($("#NewChipPlanStartDateTimeSelector").get(0).value))           //開始予定時間
        , FinishPlanTime: smbScript.ConvertDateToString2(smbScript.changeStringToDateIcrop($("#NewChipPlanFinishDateTimeSelector").get(0).value))         //終了予定時間
        , DeriveredPlanTime: smbScript.ConvertDateToString2(smbScript.changeStringToDateIcrop($("#NewChipPlanDeriveredDateTimeSelector").get(0).value))   //納車予定時間
    	, NewChipDispStartDate: smbScript.changeStringToDateIcrop($("#NewChipPlanStartDateTimeSelector").get(0).value)                                    //表示開始日時
        , WorkTime: $("#NewChipWorkTimeHidden").val()                    //作業時間
        , Order: $("#NewChipOrderTxt").val()                             //ご用命
        , RezFlg: $("#NewChipRezFlgHidden").val()                        //予約有無
        , CarWashFlg: $("#NewChipCarWashFlgHidden").val()                //洗車有無
        , WaitingFlg: $("#NewChipWaitingFlgHidden").val()                //待ち方
        , CstId: $("#InsertCstId").val()                                 //顧客ID
        , Vin: $("#NewChipVinText").val()                                //VIN
        , VclId: $("#InsertVclId").val()                                 //車両ID
        , CstVclType: $("#InsertCstVclType").val()                       //顧客車両区分
        , SACode: regSACd                                                //担当SAコード
        , RegNo: $("#NewChipRegNoText").val()                            //登録No.
        , ValidateCode: CheckNewChipInputValue()                         //入力項目チェック結果コード
        , Vehicle: $("#NewChipVehicleText").val()                        //車種
        , CstName: $("#NewChipCstNameText").val()                        //顧客名
        , Mobile: $("#NewChipMobileText").val()                          //携帯番号
        , Home: $("#NewChipHomeText").val()                              //電話番号
        , Merc: $("#NewChipMercLabel").text()                            //整備名
    	, CstAddress: $("#NewChipCstAddressText").val()                  //顧客住所    	
        , DmsCstCode: $("#SearchDmsCstCodeChange").val()                 //顧客コード
        , CompleteExaminationFlg: $("#NewChipCompleteExaminationFlgHidden").val()      //完成検査有無
        , SvcClassId: regSvcClassId                                      //表示サービス分類ID
        , MercId: regMercId                                              //表示商品ID
        , NameTitleCD: regNameTitleCd                                    //敬称コード
        , NameTitleName: regNameTitleName                                //敬称名
        , RowLockVersion: gArrObjChip[C_NEWCHIPID].rowLockVersion        //行ロックバージョン
        , RestFlg: restFlg                                               //休憩取得フラグ
        , StallStartTime: $("#hidStallStartTime").val()                  //営業開始時間(HH:mm)
        , StallEndTime: $("#hidStallEndTime").val()                      //営業終了時間(HH:mm)
        , InputStallStartTime: GetInputStallStartTime()                  //営業開始時間(date)
        , InputStallEndTime: GetInputStallEndTime()                      //営業終了時間(date)
        , SearchedFlg: gNewChipSearchedFlg                               //顧客検索実行済みフラグ (0:検索未、1:検索済)
    };

    return rtnVal;
}

/**
* コールバックでサーバーに渡すパラメータを作成する（整備名の取得用）
*
* @param {String}   method:   コールバック時のメソッド分岐用
* @param {Integer}  aftValue: 選択した変更後のサービス分類ID（整備種類）
*
*/
function CreateCallBackGetMercParam(method, aftValue) {

    //パラメータ
    var rtnVal = {
          Method: method                                    //サーバー処理分岐用
        , DlrCD: $("#hidDlrCD").val()                       //販売店コード
        , StrCD: $("#hidBrnCD").val()                       //店舗コード
        , SvcClassId: aftValue                              //選択した変更後のサービス分類ID（整備種類）
    };

    return rtnVal;
}

/**
* コールバック後の処理関数
* 
* @param {String} result コールバック呼び出し結果
* @param {String} context
*
*/
function SC3240501AfterCallBack(result, context) {

    var jsonResult = JSON.parse(result);

    //タイマーをクリア
    commonClearTimer();

    //商品コンボボックス用データが存在しない場合
    if (jsonResult.ResultCode == 3) {

        //アクティブインジケータ・オーバーレイ非表示
        gNewChipActiveIndicator.hide();
        gNewChipOverlay.hide();

        //商品コンボボックスを初期化
        var e = document.getElementById("NewChipMercList");
        e.options.length = 0; //コンボボックス内のデータをクリア
        $("#NewChipMercLabel").text("");

        //商品コンボボックスを非活性にする
        $("#NewChipMercList").attr("disabled", true);

        //必須項目がEmptyなら登録ボタンを非活性にする
        $("#NewChipRegisterBtn").attr("disabled", IsMandatoryNewChipTextEmpty());

        //次の操作を実行する
        AfterCallBack();

        // 2015/06/15 TMEJ 河原  サービス入庫予約情報のバリデーション実行タイミング変更に向けた評価用アプリ作成 STRAT

        //商品データなし("0"：商品データ無し)
        $("#NewChipMercList").attr("MERCITEM", 0);

        // 2015/06/15 TMEJ 河原  サービス入庫予約情報のバリデーション実行タイミング変更に向けた評価用アプリ作成 END

        return false;
    }

    //2015/04/23 TMEJ 明瀬  DMS連携版サービスタブレット強制納車機能追加開発 START

    //else if (jsonResult.ResultCode != 0) {
    else if (jsonResult.ResultCode != 0 && 
             jsonResult.ResultCode != -9000) {

    //2015/04/23 TMEJ 明瀬  DMS連携版サービスタブレット強制納車機能追加開発 START
   
    	//サーバー処理でエラー発生の場合
    	
        //休憩・使用不可チップとの重複以外は、ここでエラーメッセージダイアログを表示
        //  (登録時、更新チップが休憩・使用不可チップと重複した場合は、別途confirmで出す)
        if (jsonResult.ResultCode != 4) {
        	icropScript.ShowMessageBox(jsonResult.ResultCode, jsonResult.Message, "");
        }

        //アクティブインジケータ・オーバーレイ非表示
        gNewChipActiveIndicator.hide();
        gNewChipOverlay.hide();

        //キャンセルボタンを活性状態にする
        $("#NewChipCancelBtn").attr("disabled", false);

    	if(jsonResult.ResultCode == 4){
    	    //『休憩を取得しますか？(取得する場合はOKを選択、取得しない場合はキャンセルを選択)』

    	    //2014/07/18 TMEJ 明瀬 休憩取得ポップアップ文言文字化け対応 START
            //var result = confirm($("#NewChipWordDuplicateRestOrUnavailableHidden").val());
            var result = confirm(htmlDecode($("#NewChipWordDuplicateRestOrUnavailableHidden").val()));
            //2014/07/18 TMEJ 明瀬 休憩取得ポップアップ文言文字化け対応 END

            //休憩取得フラグ（0:休憩を取得しない／1:休憩を取得する）
            var restFlg = 0;

            //OK(1:休憩を取得する)をタップした場合
            if (result) {
                restFlg = 1;
            }
            else {
                //作業終了予定時間が読取専用でない場合
                if (!$("#NewChipPlanFinishDateTimeSelector").attr("readonly")) {

                    //作業終了予定を再設定する
                    //開始日時と加算時間を元に、終了日時を計算する（営業時間外の時間帯は除外して計算）
                    //var newEndDateTime = smbScript.CalcEndDate_ExcludeOutOfTime($("#NewChipPlanStartDateTimeSelector").get(0).valueAsDate, $("#NewChipWorkTimeHidden").val());
                    var newEndDateTime = smbScript.CalcEndDate_ExcludeOutOfTime(smbScript.changeStringToDateIcrop($("#NewChipPlanStartDateTimeSelector").get(0).value), $("#NewChipWorkTimeHidden").val());

                    //$("#NewChipPlanFinishDateTimeSelector").get(0).valueAsDate = newEndDateTime;
                    $("#NewChipPlanFinishDateTimeSelector").get(0).value = smbScript.getDateTimelocalDate(newEndDateTime);
                    $("#NewChipPlanFinishLabel").text(smbScript.ConvertDateOrTime(newEndDateTime, $("#hidShowDate").val()));
                }
            }

		    //アクティブインジケータ表示
		    gNewChipActiveIndicator.show();
		    //オーバーレイ表示
		    gNewChipOverlay.show();

		    //リフレッシュタイマーセット
		    commonRefreshTimer(ReDisplayNewChip);

		    //サーバーに渡すパラメータを作成
		    var prms = CreateNewChipCallBackRegistParam(C_SC3240501CALLBACK_REGISTER, gSelectedChipId, restFlg);
    		
            //次の操作を実行する
            AfterCallBack();

            //コールバック開始
            DoCallBack(C_CALLBACK_WND501, prms, SC3240501AfterCallBack, "NewChipRegisterButton");
    	}
    	else if(jsonResult.ResultCode == 6){
            //チップ詳細を閉じる
            CloseNewChip();

            //選択したチップを解放する(SetTableUnSelectedStatusは~/SC3240101/Table.js内のメソッド)
            SetTableUnSelectedStatus(gSelectedChipId);

            //チップ選択状態を解除する(SetChipUnSelectedStatusは~/SC3240101/Chip.js内のメソッド)
            SetChipUnSelectedStatus();

            //操作リストをクリアする
            ClearOperationList();

            //工程管理画面のチップを再描画する(ClickChangeDate~/SC3240101/Table.js内のメソッド)
            ClickChangeDate(0);
    	}
        else {
            //次の操作を実行する
            AfterCallBack();
            return false;
        }
    }
    else {
        //画面作成 
        if (jsonResult.Caller == C_SC3240501CALLBACK_CREATEDISP) {

            //画面の初期化 
            InitNewChipPage(result, context);

            //アクティブインジケータ・オーバーレイ非表示
            gNewChipActiveIndicator.hide();
            gNewChipOverlay.hide();

            // 2020/01/08 NSK 鈴木 TR-V4-BMS-20191001-001 (トライ店システム評価)コールセンター業務における洗車要否判断の効率化検証 START
            if (jsonResult.NewChipJson != null) {
                //商品コンボボックス用データが存在する場合

                //JSON形式の商品情報を変換する
                var mercDataList = $.parseJSON(htmlDecode(jsonResult.NewChipJson));

                //商品コンボボックスを初期化
                var e = document.getElementById("NewChipMercList");
                e.options.length = 0; //コンボボックス内のデータをクリア
                $("#NewChipMercLabel").text("");

                //空行を追加
                e.options[0] = new Option("", 0);    //チップ詳細(小)
                var i = 1;

                //商品コンボボックスにDBから取得した値をセット
                for (var keyString in mercDataList) {
                    var mercData = mercDataList[keyString];
                    e.options[i] = new Option(mercData.MERC_NAME, mercData.MERCID_TIME);    //チップ詳細(小)
                    i = i + 1;
                }

                //商品データが存在する("1"：商品データ有り)
                $("#NewChipMercList").attr("MERCITEM", 1);

                //商品コンボボックスを活性にする
                $("#NewChipMercList").attr("disabled", false);
            }
            // 2020/01/08 NSK 鈴木 TR-V4-BMS-20191001-001 (トライ店システム評価)コールセンター業務における洗車要否判断の効率化検証 END

            //登録ボタンの活性・非活性を制御
            $("#NewChipRegisterBtn").attr("disabled", IsMandatoryNewChipTextEmpty());
        	
            //次の操作を実行する
            AfterCallBack();
        }
        //整備種類ボタンクリック後 
    	else if (jsonResult.Caller == C_SC3240501CALLBACK_GETMERC) {
            //アクティブインジケータ、オーバーレイ非表示
            gNewChipActiveIndicator.hide();
            gNewChipOverlay.hide();

            //JSON形式の商品情報を変換する
            var mercDataList = $.parseJSON(htmlDecode(jsonResult.NewChipJson));

            //商品コンボボックスを初期化
            var e = document.getElementById("NewChipMercList");
            e.options.length = 0; //コンボボックス内のデータをクリア
            $("#NewChipMercLabel").text("");

            //空行を追加
            e.options[0] = new Option("", 0);    //チップ詳細(小)
            var i = 1;

            //商品コンボボックスにDBから取得した値をセット
            for (var keyString in mercDataList) {
                var mercData = mercDataList[keyString];
                e.options[i] = new Option(mercData.MERC_NAME, mercData.MERCID_TIME);    //チップ詳細(小)
                i = i + 1;
            }

            // 2015/06/15 TMEJ 河原  サービス入庫予約情報のバリデーション実行タイミング変更に向けた評価用アプリ作成 STRAT

            //商品データが存在する("1"：商品データ有り)
            $("#NewChipMercList").attr("MERCITEM", 1);

            // 2015/06/15 TMEJ 河原  サービス入庫予約情報のバリデーション実行タイミング変更に向けた評価用アプリ作成 END

            //商品コンボボックスを活性にする
            $("#NewChipMercList").attr("disabled", false);

            //必須項目がEmptyなら登録ボタンを非活性にする
            $("#NewChipRegisterBtn").attr("disabled", IsMandatoryNewChipTextEmpty());

            //次の操作を実行する 
            AfterCallBack();
        }
        //登録ボタンクリック後 
        else {
            //アクティブインジケータ・オーバーレイ非表示
            gNewChipActiveIndicator.hide();
            gNewChipOverlay.hide();

            //2015/04/23 TMEJ 明瀬  DMS連携版サービスタブレット強制納車機能追加開発 START

            //DMS除外エラーの警告が発生した場合
            if (jsonResult.ResultCode == -9000) {

                //メッセージを表示する
                icropScript.ShowMessageBox(jsonResult.ResultCode, jsonResult.Message, "");

            }

            //2015/04/23 TMEJ 明瀬  DMS連携版サービスタブレット強制納車機能追加開発 END

            //新規予約作成を閉じる
            CloseNewChip();

            //JSON形式のチップ情報を変換する。
            var newChipDataList = $.parseJSON(htmlDecode(jsonResult.NewChipJson));

            // 2014/09/12 TMEJ 張 BTS-390 「1ｽﾄｰﾙで2ﾁｯﾌﾟが作業中（表示のみ）」対応 START

            // 更新した後で、更新されたチップの情報を取得する
            var otherChipsInfo = htmlDecode(jsonResult.Contents);
            // 2014/09/12 TMEJ 張 BTS-390 「1ｽﾄｰﾙで2ﾁｯﾌﾟが作業中（表示のみ）」対応 END


            //取得したチップ情報をチップクラスに格納し、工程管理画面に表示するチップを描画する
            for (var keyString in newChipDataList) {
                var newChipData = newChipDataList[keyString];

                var strKey = newChipData.STALL_USE_ID;
                if (gArrObjChip[strKey] == undefined) {
                    gArrObjChip[strKey] = new ReserveChip(strKey);
                }
                gArrObjChip[strKey].setChipParameter(newChipData);

                //チップ生成
                gArrObjChip[strKey].createChip(C_CHIPTYPE_STALL);

                //透明の新規チップを削除する
                $("#" + C_NEWCHIPID).remove();

                //チップの位置を設定する
                SetChipPosition(strKey, "", "", "");

                //幅によって、チップに表示内容を調整する
                AdjustChipItemByWidth(strKey, gArrObjChip[strKey].vclRegNo);

                //チップをタップする時のイベントを登録
                BindChipClickEvent(strKey);

                //チップを更新（再描画）する
                UpdateChipColorByDelayDate(strKey);

                //選択したチップを解放する
                SetTableUnSelectedStatus(strKey);

                //チップ選択状態を解除する
                SetChipUnSelectedStatus();
            }

            // 2014/09/12 TMEJ 張 BTS-390 「1ｽﾄｰﾙで2ﾁｯﾌﾟが作業中（表示のみ）」対応 START
            // チップ情報により、画面を更新する
            ShowLatestChips(otherChipsInfo, false, false);
            // 2014/09/12 TMEJ 張 BTS-390 「1ｽﾄｰﾙで2ﾁｯﾌﾟが作業中（表示のみ）」対応 END

            //次の操作を実行する
            AfterCallBack();
        }
    }
}

/**
* 画面を初期化する
*
*/
function InitNewChipPage(result, context) {

    //コールバックによって取得したHTMLを設定
    var jsonResult = JSON.parse(result);
    SetNewChipContents(jsonResult.Contents);

    //新規予約作成に縦スクロールの設定
    $("#NewChipContent").fingerScroll();

    //CustomLabelの適用
    $("#NewChipPopup .NewChipEllipsis").CustomLabel({ useEllipsis: true });

    //チェックエリア(予約有無・完成検査有無・洗車有無・待ち方)の設定
    SetNewChipReservationArea();
	SetEventNewChipCompleteExaminationArea();
    SetNewChipCarWashArea();
    SetNewChipWaitingArea();

	//顧客検索実行済みフラグを初期化
	gNewChipSearchedFlg = 0;
	
	//テキストボックスエリアの初期化
	InitNewChipTextBoxArea();
	
	//検索エリアの隠し文言の初期化
	InitNewChipHiddenSearchArea();
	
    //ご用命エリアの初期化
    InitNewChipTextArea($("#NewChipOrderTxt"), $("#NewChipOrderDt"));

    //作業時間の初期値を設定（開始時間と終了時間から差分時間を計算）
    //var timeSpan = smbScript.CalcTimeSpan($("#NewChipPlanStartDateTimeSelector").get(0).valueAsDate, $("#NewChipPlanFinishDateTimeSelector").get(0).valueAsDate);
    var timeSpan = smbScript.CalcTimeSpan(smbScript.changeStringToDateIcrop($("#NewChipPlanStartDateTimeSelector").get(0).value), smbScript.changeStringToDateIcrop($("#NewChipPlanFinishDateTimeSelector").get(0).value));
    if (timeSpan != null) {
        $("#NewChipWorkTimeHidden").val(timeSpan);
    }

    //新規予約作成のテキストイベント設定
    SetNewChipTextEvent();

    //顧客検索のテキストイベント設定
    SetSearchTextEvent();

    //顧客検索画面の検索条件を初期化
	$('#Selection1').addClass('ButtonOn');

    //顧客検索画面の入力エリアに「登録No.で検索」をセット
    $('.TextArea')[0].placeholder = $('#SearchPlaceRegNo').html();

    //顧客情報ボタンを非活性にする
    $("#NewChipCustDetailBtn").attr("disabled", true);

    //R/O参照ボタンを非活性にする
    $("#NewChipRORefBtn").attr("disabled", true);

}//InitNewChipPage End

/**
* コールバックで取得したHTMLを画面に設定する
* 
* @param {String} cbResult コールバック呼び出し結果
* 
*/
function SetNewChipContents(cbResult) {

    //コールバックによって取得したHTMLを格納
    var contents = $('<Div>').html(cbResult).text();

    //新規予約作成のコンテンツを取得
    var newChip = $(contents).find('#NewChipContent');

    //顧客検索のコンテンツを取得
    var search = $(contents).find('#search');

    //新規予約作成のHiddenコンテンツを取得
    var newChipHidden = $(contents).find('#SC3240501HiddenArea');

    //新規予約作成のコンテンツを削除
    $('#NewChipContent>div').remove();

    //新規予約作成のコンテンツを設定
    newChip.children('div').clone(true).appendTo('#NewChipContent');

    //顧客検索のコンテンツを削除
    $('#search>div').remove();

    //顧客検索のコンテンツを設定
    search.children('div').clone(true).appendTo('#search');

    //新規予約作成のHiddenコンテンツを削除
    $('#SC3240501HiddenArea>div').remove();

    //新規予約作成のHiddenコンテンツを設定
    newChipHidden.children('div').clone(true).appendTo('#SC3240501HiddenArea');

} //SetNewChipContents End


/**	
* 登録時、新規予約作成の入力項目値のチェックを行う
* 	
*/
function CheckNewChipInputValue() {

    var rtnVal = 0;
    var planStart = smbScript.changeStringToDateIcrop($("#NewChipPlanStartDateTimeSelector").get(0).value);      //作業開始予定日時
    var planEnd = smbScript.changeStringToDateIcrop($("#NewChipPlanFinishDateTimeSelector").get(0).value);       //作業終了予定日時
    var planVisit = smbScript.changeStringToDateIcrop($("#NewChipPlanVisitDateTimeSelector").get(0).value);      //来店予定日時
    var planDeli = smbScript.changeStringToDateIcrop($("#NewChipPlanDeriveredDateTimeSelector").get(0).value);   //納車予定日時

    // 2015/06/15 TMEJ 河原  サービス入庫予約情報のバリデーション実行タイミング変更に向けた評価用アプリ作成 STRAT

    //サービス・商品項目必須区分の取得
    //サービス・商品項目必須区分
    //　 0：サービス分類、商品を必須入力としない
    //　 1：サービス分類、商品を必須入力とする
    //　 2：サービス分類を必須入力とする
    var mercMandType = $("#NewChipMercMandatoryTypeHidden").val();

    // 2015/06/15 TMEJ 河原  サービス入庫予約情報のバリデーション実行タイミング変更に向けた評価用アプリ作成 END

    //チップの配置時間が営業時間外の場合
    if (!smbScript.CheckChipInStallTime(planStart, planEnd)) {
        rtnVal = 906;   //チップの配置時間が営業時間外です。
    }
	//予定時間の前後関係が不正な場合
    else if (!smbScript.CheckContextOfPlan(planVisit, planStart, planEnd, planDeli)) {
        rtnVal = 904;   //予定日時の大小関係が不正です。
    }
    //入庫日時・納車日時必須フラグ (1:必須)
    else if ($("#NewChipMandatoryFlgHidden").val() == "1") {
    	if (planVisit == null) {
        	rtnVal = 911;		//入庫予定日時が入力されていません。
    	}
    	else if (planDeli == null){
    		rtnVal = 912;		//納車予定日時が入力されていません。
    	}
    };
    // 2015/06/15 TMEJ 河原  サービス入庫予約情報のバリデーション実行タイミング変更に向けた評価用アプリ作成 STRAT
    //サービス・商品項目必須区分の確認
    if (mercMandType != C_SC3240501_NOCHECK && rtnVal == 0) {
        //サービス・商品項目必須区分"0"以外の場合は必須チェックが必要
        //1,2以外はスルー

        //画面から各値の取得
        var maintenanceType = $("#NewChipMaintenanceTypeList").val();  //整備種類(サービス分類)
        var mercType = $("#NewChipMercList").val();                    //整備名(商品)


        switch (mercMandType) {

            case C_SC3240501_CHECK_MAINTE_AND_MERC:
                //"1"の場合はサービス分類、商品を必須入力とする

                if (maintenanceType == "0" || maintenanceType == null) {
                    //整備種類(サービス分類)に値無し

                    rtnVal = 914; 	//整備種類が入力されていません。

                }
                else if (mercType == "0" || mercType == null) {
                    //整備名(商品)に値無し

                    ////商品データが存在しているかチェック(商品選択不可かのチェック)
                    if ($("#NewChipMercList").attr("MERCITEM") == 1) {
                        //商品が存在している場合(商品選択可能)

                        //サービス分類に紐つく商品があるが選択されていない場合
                        rtnVal = 915; 	//整備名が入力されていません。

                    }

                }
                break;

            case C_SC3240501_CHECK_MAINTE:
                //"2"の場合はサービス分類を必須入力とする

                if (maintenanceType == "0" || maintenanceType == null) {
                    //整備種類(サービス分類)に値無し

                    rtnVal = 914; 	//整備種類が入力されていません。

                }
                break;

            default:
                //"1","2"以外場合はスルー

                break;

        }
    }
    // 2015/06/15 TMEJ 河原  サービス入庫予約情報のバリデーション実行タイミング変更に向けた評価用アプリ作成 END


    return rtnVal;
}

/**	
* 必須項目の空チェックを行う
* 	
*/
function IsMandatoryNewChipTextEmpty() {

    var rtnVal = false;
    var checkItem1 = $("#NewChipRegNoText").val();               //登録No.
    var checkItem2 = $("#NewChipVinText").val();                 //VIN
    var checkItem3 = $("#NewChipVehicleText").val();             //車種
    var checkItem4 = $("#NewChipCstNameText").val();             //顧客名
    var checkItem5 = $("#NewChipMobileText").val();              //Mobile
    var checkItem6 = $("#NewChipHomeText").val();                //Home
    var checkItem7 = $("#NewChipPlanStartTimeLabel").text();     //作業開始予定
    var checkItem8 = $("#NewChipPlanFinishTimeLabel").text();    //作業終了予定
    var checkItem9 = $("#NewChipWorkTimeHidden").val();          //作業時間
    var checkItem10 = $("#NewChipCstAddressText").val();         //住所
    var checkItem11 = $("#NewChipTitleLabel").text();            //敬称

    //「登録No.」が空で、かつ「VIN」も空の場合
    if(checkItem1 == "" && checkItem2 == "" ){
        rtnVal = true;
    }
    //「Mobile」が空で、かつ「Home」も空の場合
    else if(checkItem5 == "" && checkItem6 == "" ){
        rtnVal = true;
    }
    //「車種」、「顧客名」、「作業開始予定」、「作業終了予定」、「作業時間」、「住所」、「敬称」が空の場合
    else if(checkItem3 == "" || checkItem4 == "" || checkItem7 == "" || checkItem8 == "" || checkItem9 == "" || checkItem10 == "" || checkItem11 == ""){
        rtnVal = true;
    }

    return rtnVal;
}

/**	
* テキストエリアの初期化を行う
* 	
* @param {$(textarea)} ctrlTa
* @param {$(dt)} ctrlDt
* @return {-} -
*
*/
function InitNewChipTextArea(ctrlTa, ctrlDt) {

    var settingHeight = 0;
    var textArea = ctrlTa;
    var headerDt = ctrlDt;

    //初期表示データが5行以上ある場合、設定値はscrollHeight
    if (C_NEWCHIP_TA_DEFAULTHEIGHT < textArea.attr("scrollHeight")) {
        settingHeight = textArea.attr("scrollHeight");
    }
    //5行未満はデフォルト値
    else {
        settingHeight = C_NEWCHIP_TA_DEFAULTHEIGHT;
    }

    //テキストエリアとヘッダーに高さ設定
    textArea.height(settingHeight);
    headerDt.css("line-height", settingHeight + 12 + "px");
}

/**	
* テキストエリアの設定を行う（検索後の住所）
* 	
* @param {$(textarea)} ctrlTa
* @param {$(dt)} ctrlDt
* @return {-} -
*
*/
function SetNewChipTextAreaAddress(ctrlTa, ctrlDt) {

    var settingHeight = 0;
    var textArea = $("#NewChipCstAddressText");
    var textAreaDt = $("#NewChipCstAddressDt");
	var textAreaDd = $("#NewChipCstAddressDd");
    var textAreaDl = $("#NewChipCstAddressDl");
	var textAreali = $(".newChipCstAddress");
	var CustInfoUi = $("#NewChipCustInfoUl");

    //初期表示データが3行以上ある場合、設定値はscrollHeight
    if (C_NEWCHIP_TA_DEFAULTHEIGHT_ADDRESS < textArea.attr("scrollHeight")) {
        settingHeight = textArea.attr("scrollHeight");
    }
    //3行未満はデフォルト値
    else {
        settingHeight = C_NEWCHIP_TA_DEFAULTHEIGHT_ADDRESS;
    }
	
    //テキストエリアとヘッダーに高さ設定
	CustInfoUi.height(C_NEWCHIP_TA_DEFAULTHEIGHT_CUSTINFO + settingHeight - C_NEWCHIP_TA_DEFAULTHEIGHT_ADDRESS);
    textArea.height(settingHeight - C_NEWCHIP_TA_OFFSET_PADDING_ADDRESS);
	textAreaDt.height(settingHeight);
    textAreaDt.css("line-height", settingHeight + 'px');
	textAreaDd.height(settingHeight);
	textAreaDl.height(settingHeight);
	textAreali.height(settingHeight);
}
/**	
* テキストボックスエリアの初期化を行う
* 	
* @return {-} -
*
*/
function InitNewChipTextBoxArea(){
    //「登録No.」
    $("#NewChipRegNoText").val("");

    //「VIN」
    $("#NewChipVinText").val("");

    //「車種」
    $("#NewChipVehicleText").val("");

    //「顧客名」
    $("#NewChipCstNameText").val("");
	
    //「Mobile」
    $("#NewChipMobileText").val("");

    //「Home」
    $("#NewChipHomeText").val("");

    //「顧客住所」
    $("#NewChipCstAddressText").val("");
}

/**	
* 検索エリアの隠し文言の初期化を行う
* 	
* @return {-} -
*
*/
function InitNewChipHiddenSearchArea(){
    //「登録No.」
    $("#SearchRegistrationNumberChange").val("");

    //「VIN」
    $("#SearchVinChange").val("");

    //「車種」
    $("#SearchVehicleChange").val("");

    //「顧客名」
    $("#SearchCustomerNameChange").val("");
	
    //「Mobile」
    $("#SearchMobileChange").val("");

    //「Home」
    $("#SearchPhoneChange").val("");

    //「顧客住所」
    $("#SearchCustomerAddressChange").val("");
	
    //「担当SA」
    $("#SearchSANameChange").val("");
	
    //「基幹顧客コード」
    $("#SearchDmsCstCodeChange").val("");

    //「敬称」
    $("#SearchTitleChange").val("");

    //「敬称コード」
    $("#SearchTitleCodeChange").val("");
	
    //「担当SA」
    $("#InsertSaCode").val("");
	
    //「顧客ID」
    $("#InsertCstId").val("");
                    
    //「Vin」
    $("#InsertVin").val("");

    //「車両ID」
    $("#InsertVclId").val("");

    //「顧客車両区分」
    $("#InsertCstVclType").val("");
}

/**	
* テキストエリアの高さ調整を行う
* 	
* @param {$(textarea)} ctrlTa
* @param {$(dt)} ctrlDt
* @return {-} -
*
*/
function AdjusterNewChipTextArea(ctrlTa, ctrlDt) {

    var textArea = ctrlTa;
    var headerDt = ctrlDt;

    textArea.height(C_NEWCHIP_TA_DEFAULTHEIGHT);
    //headerDt.css("line-height", (C_NEWCHIP_TA_DEFAULTHEIGHT + 12) + 'px');

    var tmp_sh = textArea.attr("scrollHeight");

    while (tmp_sh > textArea.attr("scrollHeight")) {
        tmp_sh = textArea.attr("scrollHeight");
        textarea[0].scrollHeight++;
    }

    if (textArea.attr("scrollHeight") >= textArea.attr("offsetHeight")) {
        textArea.height(textArea.attr("scrollHeight"));
        headerDt.css("line-height", (textArea.attr("scrollHeight") + 12) + 'px');
    }
} //AdjusterNewChipTextArea End

/**	
* テキストエリアの高さ調整を行う(顧客住所)
* 	
* @param {$(textarea)} ctrlTa
* @param {$(dt)} ctrlDt
* @return {-} -
*
*/
function AdjusterNewChipTextAreaAddress() {

    var textArea = $("#NewChipCstAddressText");
    var textAreaDt = $("#NewChipCstAddressDt");
	var textAreaDd = $("#NewChipCstAddressDd");
    var textAreaDl = $("#NewChipCstAddressDl");
	var textAreali = $(".newChipCstAddress");
	var CustInfoUi = $("#NewChipCustInfoUl");

	textArea.height(C_NEWCHIP_TA_DEFAULTHEIGHT_ADDRESS - C_NEWCHIP_TA_OFFSET_PADDING_ADDRESS);
	
    if (textArea.attr("scrollHeight") >= textArea.attr("offsetHeight")) {
    	CustInfoUi.height(C_NEWCHIP_TA_DEFAULTHEIGHT_CUSTINFO + textArea.attr("scrollHeight") - C_NEWCHIP_TA_DEFAULTHEIGHT_ADDRESS);
        textArea.height(textArea.attr("scrollHeight") - C_NEWCHIP_TA_OFFSET_PADDING_ADDRESS);
    	textAreaDt.height(textArea.attr("scrollHeight"));
        textAreaDt.css("line-height", (textArea.attr("scrollHeight")) + 'px');
    	textAreaDd.height(textArea.attr("scrollHeight"));
    	textAreaDl.height(textArea.attr("scrollHeight"));
    	textAreali.height(textArea.attr("scrollHeight"));
    }
} //AdjusterNewChipTextArea End

/**	
* マルチテキストの高さが自動で広がって行った後、スクロール位置がずれる現象をなくすための調整処理
* 	
*/
function AdjustNewChipDisplay() {
    $("#NewChipPopup").css("display", "inline-block");
    setTimeout(function () {
        $("#NewChipPopup").css("display", "block");
    }, 0);
}

/**	
* テキストエリア内の文字列長制御を行う
* 	
* @param {$(textarea)} ta
*
*/
function ControlLengthNewChipTextarea(ta) {

    var maxLen = ta.attr("maxlen");

    if (ta.val().length > maxLen) {
        ta.val(ta.val().substr(0, maxLen));
    }
}

/**	
* 新規予約作成画面のキャンセルボタン
* 	
*/
function NewChipCancelButton() {
    CloseNewChip();
    return false;
}

/**	
* 顧客検索画面のキャンセルボタン
* 	
*/
function SearchCancelButton() {

    //顧客検索画面用のキャンセルボタン・登録ボタンを非表示にする
    SetSearchHeaderButton("none");

    //顧客検索画面用のヘッダーラベルを非表示にする
    $("#SearchHeaderLabel").css("display", "none");

    //新規予約作成画面用のキャンセルボタン・登録ボタンを表示させる
    SetNewChipHeaderButton("block");

    //新規予約作成画面用のヘッダーラベルを表示させる
    $("#NewChipHeaderLabel").css("display", "inline-block");

    SlideStatus();
    return false;
}

/**	
* 新規予約作成画面の登録ボタン
* 	
*/
function NewChipRegisterButton() {

    //アクティブインジケータ表示
    gNewChipActiveIndicator.show();
    //オーバーレイ表示
    gNewChipOverlay.show();

    //リフレッシュタイマーセット
    commonRefreshTimer(ReDisplayNewChip);

    //サーバーに渡すパラメータを作成
    var prms = CreateNewChipCallBackRegistParam(C_SC3240501CALLBACK_REGISTER, gSelectedChipId, -1);

    //コールバック開始
    DoCallBack(C_CALLBACK_WND501, prms, SC3240501AfterCallBack, "NewChipRegisterButton");

    return false;
}

/**	
* 顧客検索画面の登録ボタン
* 	
*/
function SearchRegisterButton() {

	//顧客検索実行済みフラグを実行済に設定
	gNewChipSearchedFlg = 1;

	//顧客検索結果を新規予約作成画面に設定
	SetSearchValue();
	
    //顧客検索画面用のキャンセルボタン・登録ボタンを非表示にする
    SetSearchHeaderButton("none");

    //顧客検索画面用のヘッダーラベルを非表示にする
    $("#SearchHeaderLabel").css("display", "none");

    //新規作成画面用のキャンセルボタン・登録ボタンを表示させる
    SetNewChipHeaderButton("block");

    //新規作成画面用のヘッダーラベルを表示させる
    $("#NewChipHeaderLabel").css("display", "inline-block");

    //顧客情報ボタンを活性状態にする
    $("#NewChipCustDetailBtn").attr("disabled", false);

    //新規予約作成画面へのスライド処理
    SlideStatus();

    //登録ボタンの活性・非活性を制御
    $("#NewChipRegisterBtn").attr("disabled", IsMandatoryNewChipTextEmpty());

    return false;
}

/**	
* 顧客検索結果を新規予約作成画面に設定し編集不可にする
* 	
*/
function SetSearchValue() {
    //「登録No.」
    $("#NewChipRegNoText").val($("#SearchRegistrationNumberChange").val());
	$("#NewChipRegNoText").addClass(C_SC3240501CLASS_TEXTBLACK);
    $("#NewChipRegNoText").attr("readonly", true);

    //「VIN」
    $("#NewChipVinText").val($("#SearchVinChange").val());
	$("#NewChipVinText").addClass(C_SC3240501CLASS_TEXTBLACK);
    $("#NewChipVinText").attr("readonly", true);

    //「車種」
    $("#NewChipVehicleText").val($("#SearchVehicleChange").val());
	$("#NewChipVehicleText").addClass(C_SC3240501CLASS_TEXTBLACK);
    $("#NewChipVehicleText").attr("readonly", true);

    //「顧客名」
    $("#NewChipCstNameText").val($("#SearchCustomerNameChange").val());
	$("#NewChipCstNameText").addClass(C_SC3240501CLASS_TEXTBLACK);
    $("#NewChipCstNameText").attr("readonly", true);

	//「敬称」
    $("#NewChipTitleLabel").text($("#SearchTitleChange").val());
	$("#NewChipTitleLabel").addClass(C_SC3240501CLASS_TEXTBLACK);
    $("#NewChipTitleList").attr("disabled", true);
    $("#NewChipTitle").attr("style", "background:none;");
	
    //「Mobile」
    $("#NewChipMobileText").val($("#SearchMobileChange").val());
	$("#NewChipMobileText").addClass(C_SC3240501CLASS_TEXTBLACK);
    $("#NewChipMobileText").attr("readonly", true);

    //「Home」
    $("#NewChipHomeText").val($("#SearchPhoneChange").val());
	$("#NewChipHomeText").addClass(C_SC3240501CLASS_TEXTBLACK);
    $("#NewChipHomeText").attr("readonly", true);

    //「担当SA名」
    $("#NewChipSALabel").text($("#SearchSANameChange").val());
	$("#NewChipSALabel").addClass(C_SC3240501CLASS_TEXTBLACK);
    $("#NewChipSAList").attr("disabled", true);
    $("#NewChipSA").attr("style", "background:none;");

    //「顧客住所」
	$("#NewChipCstAddressText").height(C_NEWCHIP_TA_DEFAULTHEIGHT_ADDRESS - C_NEWCHIP_TA_OFFSET_PADDING_ADDRESS);	// テキストが設定される前にテキストエリア初期化
    $("#NewChipCstAddressText").val($("#SearchCustomerAddressChange").val());
	$("#NewChipCstAddressText").addClass(C_SC3240501CLASS_TEXTBLACK);
    $("#NewChipCstAddressText").attr("onFocus", "this.blur()").attr("readonly", true);
	SetNewChipTextAreaAddress();
}

/**	
* 新規予約作成のポップアップを閉じる
* 	
*/
function CloseNewChip() {

    $("#NewChipRegisterBtn").unbind();
    $("#NewChipPopup").fadeOut(300);

    //新規予約作成画面へのスライド処理
    setTimeout(function () {
        SlideStatus();
    }, 300);

    //フッターボタンを再表示する
    CreateFooterButton(C_FT_DISPTP_SELECTED, C_FT_BTNTP_REZ_NEW);

//    //ポップアップ監視イベントを取り除く(画面を表示するたびにbindするため)
//    $(document.body).unbind(C_SC3240501_TOUCH, ObserveNewChipClose);

    //タイマーをクリア
    commonClearTimer();

    return false;
}


/**
* ポップアップを表示する方向を計算し、設定値を決定します
*/
function CalcNewChipPopoverPosition(baseCtrl) {

    var possibleDir = {
        left: false,
        right: false
    }

    var ctrlPosition = {
        popX: 0,    //ポップアップのleft値
        arrowX: 0,  //吹出し三角のleft値
        arrowY: 0   //吹出し三角のtop値
    }

    //ポップアップをチップの左に表示する場合のleft値
    var popDispLeftX = baseCtrl.offset().left - (($("#NewChipPopup .ArrowMask").width() / 2) - 3) - $("#NewChipPopupContent").width();
    //var popDispLeftX = 100;
    
    //ポップアップをチップの右に表示する場合のleft値
    var popDispRightX = baseCtrl.offset().left + baseCtrl.width() + (($("#NewChipPopup .ArrowMask").width() / 2) - 3);
    
    //チップのleft + チップのwidth + (吹出し三角のwidth / 2 - 3(微調整値)) + ポップアップのwidth <= 画面全体のwidth
    if (popDispRightX + $("#NewChipPopupContent").width() <= $(document.body).width()) {
        possibleDir.right = true;
    }
    //0 <= チップのleft - (吹出し三角のwidth / 2 - 3(微調整値)) - ポップアップのwidth
    else if (0 <= popDispLeftX) {
        possibleDir.left = true;
    }

    if (possibleDir.right) {
        ctrlPosition.popX = popDispRightX;
        ctrlPosition.arrowX = C_NEWCHIP_POP_DISPRIGHT_ARROW_X;
    }
    else if (possibleDir.left) {
        ctrlPosition.popX = popDispLeftX;
        ctrlPosition.arrowX = C_NEWCHIP_POP_DISPLEFT_ARROW_X;
    }
    else {  //チップが長すぎてどちらも適切でない場合
        ctrlPosition.popX = C_NEWCHIP_POP_DISPRIGHT_DEFAULT_X;
        ctrlPosition.arrowX = C_NEWCHIP_POP_DISPRIGHT_ARROW_X;
    }

    //チップのtop + (チップのheight / 2) - 79(調整値)
    ctrlPosition.arrowY = baseCtrl.offset().top + (baseCtrl.height() / 2) - 79;

    return ctrlPosition;
}

/**
* ポップアップを表示する位置を設定します
*/
function SetNewChipPopoverPosition(ctrlPosition) {
    $("#NewChipPopupContent").css("left", ctrlPosition.popX);
    $(".ArrowMask").css("left", ctrlPosition.arrowX);
    $(".ArrowMask").css("top", ctrlPosition.arrowY);
    //新規予約作成表示時のleft値を保持しておく
    gNewChipPopX = ctrlPosition.popX;
}


/*
* 顧客検索画面へのスライド処理
* 
*/
function SlideSearch() {

    //新規予約作成画面用のキャンセルボタン・登録ボタンを非表示にする
    SetNewChipHeaderButton("none");

    //新規予約作成画面用のヘッダーラベルを非表示にする
    $("#NewChipHeaderLabel").css("display", "none");

    //顧客検索画面用のキャンセルボタン・登録ボタンを表示させる
    SetSearchHeaderButton("block");

    //顧客検索画面用のヘッダーラベルを表示させる
    $("#SearchHeaderLabel").css("display", "inline-block");

    //顧客検索画面用の登録ボタンを非活性にしておく
    $("#SearchRegisterBtn").attr("disabled", true);

    //顧客検索画面の検索結果エリアのスクロール位置を初期化
    $('.SearchDataBox .scroll-inner').css('transform', 'translate3d(0px, 0px, 0px)');

    //「条件に一致する検索結果がありません」を表示させる
    $("#NoSearchImage").css('display', 'block');

    //スライド処理
    $('.contentInner').css("-webkit-transition", "transform 500ms ease-in-out 0").css("transform", "translate3d(-401px, 0, 0)");

    //検索条件のボタンを登録No.に設定
	$('#Selection1').addClass('ButtonOn');

    //入力エリアに「登録No.で検索」をセット
    $('.TextArea')[0].placeholder = $('#SearchPlaceRegNo').html();

	//顧客検索画面用の顧客詳細ボタンを非活性にする
    $('#SearchBottomButton').addClass('BottomButtonDisable');
	$("#SearchBottomButton").attr("disabled", true);
	
    return false;
}

/*
* 新規予約作成画面へのスライド処理
* 
*/
function SlideStatus() {

    $('.contentInner').css("transform", "translate3d(0, 0, 0)");

    $('.SelectionButton').children('ul').children('li').removeClass('ButtonOn');
    $('#Selection1').addClass('ButtonOn');

    selectSearchTypeIndex = 0;
    $('.TextArea')[0].placeholder = $('#SearchPlaceRegNo').html();
    searchListClear();
    $('#SearchBottomButton').addClass('BottomButtonDisable');
	$("#SearchBottomButton").attr("disabled", true);

    return false;
}

/*
* 検索条件の切り替え処理
*
*/
function SelectSearchType(select) {
    $('.TextArea').blur();
    this.selectSearchTypeIndex = $('.SelectionButton ul li').index(select);
    selectButton = $(select).attr('className');
    switch (selectSearchTypeIndex) {
        case 0:
            $('.TextArea')[0].placeholder = $('#SearchPlaceRegNo').html();
            break;
        case 1:
            $('.TextArea')[0].placeholder = $('#SearchPlaceVin').html();
            break;
        case 2:
            $('.TextArea')[0].placeholder = $('#SearchPlaceName').html();
            break;
        case 3:
            $('.TextArea')[0].placeholder = $('#SearchPlacePhone').html();
            break;
        default:
            break;
    }
    //選択されているボタンを押した場合はテキストボックスにフォーカスを当てるのみ
    if (selectButton === 'ButtonOn') {
        $('.TextArea').focus();
        return false;
    }
    //検索条件ボタンの状態を切替える
    searchUl = $('.SelectionButton').children('ul');
    $(searchUl).children('li').removeClass('ButtonOn');
    $(select).addClass('ButtonOn');

    return false;
}
/*
* 顧客検索開始
*
*/
function SearchCustomer() {

    // 検索条件の入力チェック
    if (SearchInputCheck() == false) {
        //falseが帰ってきたら検索処理をしない
        return;
    }

    searchListClear();
    searchText = $('.TextArea').attr('value');

    $('#SearchRegistrationNumberHidden').val("");
    $('#SearchVinHidden').val("");
    $('#SearchCustomerNameHidden').val("");
    $('#SearchPhoneNumberHidden').val("");
    switch (this.selectSearchTypeIndex) {
        case 0:
            $('#SearchRegistrationNumberHidden').val(searchText);
            break;
        case 1:
            $('#SearchVinHidden').val(searchText);
            break;
        case 2:
            $('#SearchCustomerNameHidden').val(searchText);
            break;
        case 3:
            $('#SearchPhoneNumberHidden').val(searchText);
            break;
        default:
            break;
    }
    $('#SearchStartRowHidden').val("1");
    $('#SearchEndRowHidden').val("1");
    $('#SearchSelectTypeHidden').val("0");
    $('#SearchDataLoading').css('display', 'block');
    aryPostCtrl.push("SearchCustomer");

    //リフレッシュタイマーセット
    commonRefreshTimer(ReDisplayNewChip);

    $('#SearchCustomerDummyButton').click();

    return false;
}

/**
* 検索条件の入力チェック
*/
function SearchInputCheck() {

    var rtnVal = true;

    searchText = $('.TextArea').attr('value');

    //空の場合
    if(searchText.trim() == "" ){
        //エラーメッセージダイアログを表示（検索文字を入力してください。）
        icropScript.ShowMessageBox(903, $("#SearchErrMsg1Hidden").val(), "");
        rtnVal = false;
    }

    return rtnVal;
}

/*
* 顧客検索テキストクリア
*
*/
function TextClear() {
    searchText = $('.TextArea').attr('value');
    if (searchText == undefined || searchText == "") {
        return false;
    }
    searchText = "";
    $('.TextArea').attr('value', searchText);
}

/*
* 次のN件を読み込むを選択
*
*/
function SearchNextList() {
    
    $('#SearchSelectTypeHidden').val("1");
    aryPostCtrl.push("SearchCustomer");

    //顧客検索画面用の登録ボタンを非活性にする
    $("#SearchRegisterBtn").attr("disabled", true);

    //顧客検索画面用の顧客詳細ボタンを非活性にする
    $('#SearchBottomButton').addClass('BottomButtonDisable');
	$("#SearchBottomButton").attr("disabled", true);

    $('.NextList').css('display', 'none');
    $('.NextSearchingImage').css('display', 'block');
    $('.NextListSearching').css('display', 'block');

    //リフレッシュタイマーセット
    commonRefreshTimer(ReDisplayNewChip);

    $('#SearchCustomerDummyButton').click();
}
/*
* 前のN件を読み込むを選択
*
*/
function SearchFrontList() {
    
    $('#SearchSelectTypeHidden').val("-1");
    aryPostCtrl.push("SearchCustomer");

    //顧客検索画面用の登録ボタンを非活性にする
    $("#SearchRegisterBtn").attr("disabled", true);

    //顧客検索画面用の顧客詳細ボタンを非活性にする
    $('#SearchBottomButton').addClass('BottomButtonDisable');
	$("#SearchBottomButton").attr("disabled", true);

    $('.FrontList').css('display', 'none');
    $('.FrontSearchingImage').css('display', 'block');
    $('.FrontListSearching').css('display', 'block');

    //リフレッシュタイマーセット
    commonRefreshTimer(ReDisplayNewChip);

    $('#SearchCustomerDummyButton').click();
}

/*
* 検索状態のクリア
* 
*/
function searchListClear() {
    var searchList = $('#SearchListBox');
    $(searchList).remove();
    $(searchList).empty();
    if ($('.NoSearchImage').css('display') === 'block') {
        $('.NoSearchImage').css('display', 'none');
    }
    if ($('.FrontLink').css('display') === 'block') {
        $('.FrontLink').css('display', 'none');
    }
    if ($('.EndLink').css('display') === 'block') {
        $('.EndLink').css('display', 'none');
    }

    //顧客検索画面用の登録ボタンを非活性にする
    $("#SearchRegisterBtn").attr("disabled", true);

    //顧客検索画面用の顧客詳細ボタンを非活性にする
    $('#SearchBottomButton').addClass('BottomButtonDisable');
	$("#SearchBottomButton").attr("disabled", true);
}

/**	
* 画面を再表示する(commonRefreshTimerにセットする関数)
* 	
*/
function ReDisplayNewChip() {
    CloseNewChip();
    ClearOperationList();
    setTimeout(function () {
        FooterEvent(C_FT_BTNID_DETAIL);
    }, 200);
}

/**	
* 顧客詳細ボタンとR/O参照ボタンをタップしたときにsubmitしないようにする
* ※OnClientClickにreturn false;を書く方法はうまくいかなかった
* 	
*/
function SC3240501SubmitCancel() {
    return false;
}

/**	
* 顧客詳細ボタン
* ※OnClientClickにreturn false;を書く方法はうまくいかなかった
* 	
*/
function NewChipCustButton() {
    // ボタンを青色にする
    $("#NewChipCustBtnDiv").addClass("icrop-pressed");
	
	if($("#SearchDmsCstCodeChange").val() == "") {
	    //未取引客の場合（新規顧客登録をしてください。）
	    icropScript.ShowMessageBox(910, $("#NewChipCstBtnErrMsgHidden").val(), "");

        // ボタンの青色を解除
        $("#NewChipCustBtnDiv").removeClass("icrop-pressed");
	} else {
	    setTimeout(function () {
	        //アクティブインジケータ表示
            gNewChipActiveIndicator.show();

	        //オーバーレイ表示
	        gNewChipOverlay.show();

	        // ボタンの青色を解除
	        $("#NewChipCustBtnDiv").removeClass("icrop-pressed");
	    }, 300);

	    //リフレッシュタイマーセット
	    commonRefreshTimer(ReDisplayNewChip);

	    $('#NewChipCustButtonDummy').click();
	}
    return false;
}

/**	
* 顧客詳細ボタン(検索画面用)
* ※OnClientClickにreturn false;を書く方法はうまくいかなかった
* 	
*/
function NewChipCustButton_Serch() {
    // ボタンを青色にする
	$("#SearchBottomButton").attr("style", "background:#126EE4;");
	
	if($("#SearchDmsCstCodeChange").val() == "") {
	    //未取引客の場合（新規顧客登録をしてください。）
	    icropScript.ShowMessageBox(910, $("#NewChipCstBtnErrMsgHidden").val(), "");

        // ボタンの青色を解除
		$("#SearchBottomButton").attr("style", "background:-webkit-gradient(linear, left top, left bottom, from(#fff), color-stop(0.50, #f7f8f9), color-stop(0.51, #edeff3), to(#edeff3));");
	} else {
	    setTimeout(function () {
	        //アクティブインジケータ表示
            gNewChipActiveIndicator.show();

	        //オーバーレイ表示
	        gNewChipOverlay.show();

	        // ボタンの青色を解除
			$("#SearchBottomButton").attr("style", "background:-webkit-gradient(linear, left top, left bottom, from(#fff), color-stop(0.50, #f7f8f9), color-stop(0.51, #edeff3), to(#edeff3));");
	    }, 300);

	    //リフレッシュタイマーセット
	    commonRefreshTimer(ReDisplayNewChip);

	    $('#NewChipCustButtonDummy').click();
	}
    return false;
}

/**	
* 営業開始時間(date)を取得する
* 	
*/
function GetInputStallStartTime() {
    
    var rtnVal = 0;
    //var planStart = $("#NewChipPlanStartDateTimeSelector").get(0).valueAsDate;        //作業開始予定日時
    var planStart = smbScript.changeStringToDateIcrop($("#NewChipPlanStartDateTimeSelector").get(0).value);        //作業開始予定日時
    var wkStart;

    //「作業開始予定日 営業開始時刻」を返却する
    wkStart = new Date(planStart);
    rtnVal = new Date(wkStart.getFullYear() + "/" + (wkStart.getMonth() + 1) + "/" + wkStart.getDate() + " " + $("#hidStallStartTime").val() + ":00");

    return rtnVal;
}

/**	
* 営業終了時間(date)を取得する
* 	
*/
function GetInputStallEndTime() {
    
    var rtnVal = 0;
    //var planEnd = $("#NewChipPlanFinishDateTimeSelector").get(0).valueAsDate;        //作業終了予定日時
    var planEnd = smbScript.changeStringToDateIcrop($("#NewChipPlanFinishDateTimeSelector").get(0).value);         //作業終了予定時間
    var wkEnd;

    //「作業終了予定日 営業終了時刻」を返却する
    wkEnd = new Date(planEnd);
    rtnVal = new Date(wkEnd.getFullYear() + "/" + (wkEnd.getMonth() + 1) + "/" + wkEnd.getDate() + " " + $("#hidStallEndTime").val() + ":00");

    return rtnVal;
}

// 2019/10/31 NSK 鈴木 [No156]予定入庫日時、予定納車日時の自動セット START
/**
 * 予定入庫日時を算出し表示する。
 */
function calculateScheSvcinDateTime() {

    // 自動計算を行わない場合、処理終了
    if ($("#ScheSvcinDeliAutoDispFlg").val() != C_SCHESVCINDELIAUTODISPFLG_ENABLE) {
        return;
    }

    // 予定開始日時がブランクの場合、処理終了
    var newChipPlanStart = $("#NewChipPlanStartDateTimeSelector").get(0).value;
    if (!newChipPlanStart) {
        return;
    }

    // 予定開始日時取得
    var scheSvcinDateTime = smbScript.changeStringToDateIcrop(newChipPlanStart);

    // 標準受付時間（分）を減算する。
    scheSvcinDateTime = smbScript.CalcEndDateTime(scheSvcinDateTime, -1 * $("#StdAcceptanceTime").val());

    // 予定入庫日時設定
    $("#NewChipPlanVisitDateTimeSelector").get(0).value = smbScript.getDateTimelocalDate(scheSvcinDateTime);
    $("#NewChipPlanVisitTimeLabel").text(smbScript.ConvertDateToStringForDisplay(scheSvcinDateTime));
}

/**
 * 予定納車日時を算出し表示する。
 */
function calculateScheDeliDateTime() {

    // 自動計算を行わない場合、処理終了
    if ($("#ScheSvcinDeliAutoDispFlg").val() != C_SCHESVCINDELIAUTODISPFLG_ENABLE) {
        return;
    }

    // 予定終了日時がブランクの場合、処理終了
    var newChipPlanFinish = $("#NewChipPlanFinishDateTimeSelector").get(0).value;
    if (!newChipPlanFinish) {
        return;
    }

    // 予定終了日時取得
    var scheDeliDateTime = smbScript.changeStringToDateIcrop(newChipPlanFinish);

    // 検査が必要な場合、標準検査時間（分）を加算する。
    if ($("#NewChipCompleteExaminationFlgHidden").val() == C_INSPECTIONNEEDFLG_NEED) {
        scheDeliDateTime = smbScript.CalcEndDateTime(scheDeliDateTime, $("#StdInspectionTime").val());
    }

    // 標準納車準備時間（分）、標準洗車時間（分）のうち、長いほうを加算する。
    // 標準納車準備時間（分）、標準洗車時間（分）の型が文字列数字で大小比較を行うため、数値化する。
    var addTime = Number($("#StdDeliPreparationTime").val());
    var stdCarwashTime = Number($("#StdCarwashTime").val());
    if (($("#NewChipCarWashFlgHidden").val() == C_CARWASHNEEDFLG_NEED)
        && (addTime < stdCarwashTime)) {
        addTime = stdCarwashTime;
    }
    scheDeliDateTime = smbScript.CalcEndDateTime(scheDeliDateTime, addTime);

    // 標準納車時間（分）を加算する。
    scheDeliDateTime = smbScript.CalcEndDateTime(scheDeliDateTime, $("#StdDeliTime").val());

    // 予定納車日時設定
    $("#NewChipPlanDeriveredDateTimeSelector").get(0).value = smbScript.getDateTimelocalDate(scheDeliDateTime);
    $("#NewChipPlanDeriveredTimeLabel").text(smbScript.ConvertDateToStringForDisplay(scheDeliDateTime));
}
// 2019/10/31 NSK 鈴木 [No156]予定入庫日時、予定納車日時の自動セット END
