/** 
* @fileOverview SC3240501.Event.js
* 
* @author TMEJ 下村
* @version 1.0.0
*/

/**	
* 新規予約作成のテキストイベントの設定を行う
*　更新：2015/06/15 TMEJ 河原  サービス入庫予約情報のバリデーション実行タイミング変更に向けた評価用アプリ作成
*  更新：2019/10/31 NSK 鈴木【（FS）MaaSビジネス向けサービス予約の登録オペレーションの効率化に向けた試験研究】[No156]予定入庫日時、予定納車日時の自動セット
*　更新：2020/01/16 NSK 夏目  TR-SVT-TMT-20200110-002 追加jobでサービスの種類を選択できない
*　更新：2020/01/08 NSK 鈴木 TR-V4-BMS-20191001-001 (トライ店システム評価)コールセンター業務における洗車要否判断の効率化検証
*　更新：
*/

function SetNewChipTextEvent() {

    // 2020/01/16 NSK 夏目  TR-SVT-TMT-20200110-002 追加jobでサービスの種類を選択できない start
    ////来店予定日時
    //$("#NewChipPlanVisitDateTimeSelector")
    //    .blur(function () {
    //        //DateTimeSelectorで選択した日時を表示用に変換してラベルに設定
    //        //$("#NewChipPlanVisitTimeLabel").text(smbScript.ConvertDateToString($(this).get(0).valueAsDate).substr(0,16));
    //        $("#NewChipPlanVisitTimeLabel").text(smbScript.ConvertDateToStringForDisplay(smbScript.changeStringToDateIcrop($(this).get(0).value)));
    //    });
    //来店予定日時
    $("#NewChipPlanVisitDateTimeSelector")
        .change(function () {
            //DateTimeSelectorで選択した日時を表示用に変換してラベルに設定
            //$("#NewChipPlanVisitTimeLabel").text(smbScript.ConvertDateToString($(this).get(0).valueAsDate).substr(0,16));
            $("#NewChipPlanVisitTimeLabel").text(smbScript.ConvertDateToStringForDisplay(smbScript.changeStringToDateIcrop($(this).get(0).value)));
        });
    // 2020/01/16 NSK 夏目  TR-SVT-TMT-20200110-002 追加jobでサービスの種類を選択できない end

    // 2020/01/16 NSK 夏目  TR-SVT-TMT-20200110-002 追加jobでサービスの種類を選択できない start
    ////作業開始予定日時
    //$("#NewChipPlanStartDateTimeSelector")
    //    .blur(function () {
    //        //作業開始予定日時がnullでない場合
    //        //if ($(this).get(0).valueAsDate != null) {
    //        if ($(this).get(0).value != null && $(this).get(0).value != "") {
    //            //作業開始予定日時を5分単位で丸め込んだ日時を計算し、DateTimSelectorに設定しなおす
    //            //var wkDate = new Date(smbScript.RoundUpTimeTo5Units($(this).get(0).valueAsDate));
    //            var wkDate = new Date(smbScript.RoundUpTimeTo5Units(smbScript.changeStringToDateIcrop($(this).get(0).value)));

    //            //営業時間外を選択している場合は、翌営業開始時刻に補正する
    //            var newDate = smbScript.GetDateExcludeOutOfTime(wkDate);

    //            //$(this).get(0).valueAsDate = newDate;
    //            $(this).get(0).value = smbScript.getDateTimelocalDate(newDate);

    //            //DateTimeSelectorで選択した日時を表示用に変換してラベルに設定
    //            $("#NewChipPlanStartTimeLabel").text(smbScript.ConvertDateToStringForDisplay(newDate));

    //            //作業終了予定日時がnullでない場合
    //            //if ($("#NewChipPlanFinishDateTimeSelector").get(0).valueAsDate != null) {
    //            if ($("#NewChipPlanFinishDateTimeSelector").get(0).value != null && $("#NewChipPlanFinishDateTimeSelector").get(0).value != "") {

    //                //開始日時と終了日時から差分時間を計算し、作業時間に設定する　※営業時間外の時間帯は減算して計算
    //                //var timeSpan = smbScript.CalcTimeSpan_ExcludeOutOfTime($(this).get(0).valueAsDate, $("#NewChipPlanFinishDateTimeSelector").get(0).valueAsDate);
    //                var timeSpan = smbScript.CalcTimeSpan_ExcludeOutOfTime(smbScript.changeStringToDateIcrop($(this).get(0).value), smbScript.changeStringToDateIcrop($("#NewChipPlanFinishDateTimeSelector").get(0).value));

    //                if (timeSpan != null) {
    //                    //最大値を超える場合、作業時間最大値（分）をセット
    //                    if (timeSpan > C_SC3240501_MAXWORKTIME) {
    //                        timeSpan = C_SC3240501_MAXWORKTIME;

    //                        //開始日時と加算時間を元に、終了日時を計算する（営業時間外の時間帯は除外して計算）
    //                        //var newEndDateTime = smbScript.CalcEndDate_ExcludeOutOfTime($("#NewChipPlanStartDateTimeSelector").get(0).valueAsDate, C_SC3240501_MAXWORKTIME);
    //                    	var newEndDateTime = smbScript.CalcEndDate_ExcludeOutOfTime(smbScript.changeStringToDateIcrop($("#NewChipPlanStartDateTimeSelector").get(0).value), C_SC3240501_MAXWORKTIME);

    //                        //$("#NewChipPlanFinishDateTimeSelector").get(0).valueAsDate = newEndDateTime;
    //                    	$("#NewChipPlanFinishDateTimeSelector").get(0).value = smbScript.getDateTimelocalDate(newEndDateTime);
    //                        $("#NewChipPlanFinishTimeLabel").text(smbScript.ConvertDateToStringForDisplay(newEndDateTime));
    //                    }
    //                    $("#NewChipWorkTimeHidden").val(timeSpan);
    //                }
    //            }
    //        }
    //        else {
    //            //ラベルをEmptyにする
    //            $("#NewChipPlanStartTimeLabel").text("");
    //        }
            
    //        //必須項目がEmptyなら登録ボタンを非活性にする
    //        $("#NewChipRegisterBtn").attr("disabled", IsMandatoryNewChipTextEmpty());
    //    });
    //作業開始予定日時
    $("#NewChipPlanStartDateTimeSelector")
        .change(function () {
            //作業開始予定日時がnullでない場合
            //if ($(this).get(0).valueAsDate != null) {
            if ($(this).get(0).value != null && $(this).get(0).value != "") {
                //作業開始予定日時を5分単位で丸め込んだ日時を計算し、DateTimSelectorに設定しなおす
                //var wkDate = new Date(smbScript.RoundUpTimeTo5Units($(this).get(0).valueAsDate));
                var wkDate = new Date(smbScript.RoundUpTimeTo5Units(smbScript.changeStringToDateIcrop($(this).get(0).value)));

                //営業時間外を選択している場合は、翌営業開始時刻に補正する
                var newDate = smbScript.GetDateExcludeOutOfTime(wkDate);

                //$(this).get(0).valueAsDate = newDate;
                $(this).get(0).value = smbScript.getDateTimelocalDate(newDate);

                //DateTimeSelectorで選択した日時を表示用に変換してラベルに設定
                $("#NewChipPlanStartTimeLabel").text(smbScript.ConvertDateToStringForDisplay(newDate));

                // 2019/10/31 NSK 鈴木 [No156]予定入庫日時、予定納車日時の自動セット START
                // 予定入庫日時を算出し表示する。
                calculateScheSvcinDateTime();
                // 2019/10/31 NSK 鈴木 [No156]予定入庫日時、予定納車日時の自動セット END

                //作業終了予定日時がnullでない場合
                //if ($("#NewChipPlanFinishDateTimeSelector").get(0).valueAsDate != null) {
                if ($("#NewChipPlanFinishDateTimeSelector").get(0).value != null && $("#NewChipPlanFinishDateTimeSelector").get(0).value != "") {

                    //開始日時と終了日時から差分時間を計算し、作業時間に設定する　※営業時間外の時間帯は減算して計算
                    //var timeSpan = smbScript.CalcTimeSpan_ExcludeOutOfTime($(this).get(0).valueAsDate, $("#NewChipPlanFinishDateTimeSelector").get(0).valueAsDate);
                    var timeSpan = smbScript.CalcTimeSpan_ExcludeOutOfTime(smbScript.changeStringToDateIcrop($(this).get(0).value), smbScript.changeStringToDateIcrop($("#NewChipPlanFinishDateTimeSelector").get(0).value));

                    if (timeSpan != null) {
                        //最大値を超える場合、作業時間最大値（分）をセット
                        if (timeSpan > C_SC3240501_MAXWORKTIME) {
                            timeSpan = C_SC3240501_MAXWORKTIME;

                            //開始日時と加算時間を元に、終了日時を計算する（営業時間外の時間帯は除外して計算）
                            //var newEndDateTime = smbScript.CalcEndDate_ExcludeOutOfTime($("#NewChipPlanStartDateTimeSelector").get(0).valueAsDate, C_SC3240501_MAXWORKTIME);
                            var newEndDateTime = smbScript.CalcEndDate_ExcludeOutOfTime(smbScript.changeStringToDateIcrop($("#NewChipPlanStartDateTimeSelector").get(0).value), C_SC3240501_MAXWORKTIME);

                            //$("#NewChipPlanFinishDateTimeSelector").get(0).valueAsDate = newEndDateTime;
                            $("#NewChipPlanFinishDateTimeSelector").get(0).value = smbScript.getDateTimelocalDate(newEndDateTime);
                            $("#NewChipPlanFinishTimeLabel").text(smbScript.ConvertDateToStringForDisplay(newEndDateTime));

                            // 2019/10/31 NSK 鈴木 [No156]予定入庫日時、予定納車日時の自動セット START
                            // 予定納車日時を算出し表示する。
                            calculateScheDeliDateTime();
                            // 2019/10/31 NSK 鈴木 [No156]予定入庫日時、予定納車日時の自動セット END
                        }
                        $("#NewChipWorkTimeHidden").val(timeSpan);
                    }
                }
            }
            else {
                //ラベルをEmptyにする
                $("#NewChipPlanStartTimeLabel").text("");
            }

            //必須項目がEmptyなら登録ボタンを非活性にする
            $("#NewChipRegisterBtn").attr("disabled", IsMandatoryNewChipTextEmpty());
        });
    // 2020/01/16 NSK 夏目  TR-SVT-TMT-20200110-002 追加jobでサービスの種類を選択できない end

    // 2020/01/16 NSK 夏目  TR-SVT-TMT-20200110-002 追加jobでサービスの種類を選択できない start
    ////作業終了予定日時
    //$("#NewChipPlanFinishDateTimeSelector")
    //    .blur(function () {
    //        //作業終了予定日時がnullでない場合
    //        //if ($(this).get(0).valueAsDate != null) {
    //        if ($(this).get(0).value != null && $(this).get(0).value != "") {
    //            //作業終了予定日時を5分単位で丸め込んだ日時を計算し、DateTimSelectorに設定しなおす
    //            //var wkDate = new Date(smbScript.RoundUpTimeTo5Units($(this).get(0).valueAsDate));
    //            var wkDate = new Date(smbScript.RoundUpTimeTo5Units(smbScript.changeStringToDateIcrop($(this).get(0).value)));

    //            //営業時間外を選択している場合は、翌営業開始時刻に補正する
    //            var newDate = smbScript.GetDateExcludeOutOfTime(wkDate);

    //            //$(this).get(0).valueAsDate = newDate;
    //            $(this).get(0).value = smbScript.getDateTimelocalDate(newDate);

    //            //DateTimeSelectorで選択した日時を表示用に変換してラベルに設定
    //            $("#NewChipPlanFinishTimeLabel").text(smbScript.ConvertDateToStringForDisplay(newDate));

    //            //作業開始予定日時がnullでない場合
    //            //if ($("#NewChipPlanStartDateTimeSelector").get(0).valueAsDate != null) {
    //            if ($("#NewChipPlanStartDateTimeSelector").get(0).value != null && $("#NewChipPlanStartDateTimeSelector").get(0).value != "") {
    //                //開始日時と終了日時から差分時間を計算し、作業時間に設定する　※営業時間外の時間帯は減算して計算
    //                //var timeSpan = smbScript.CalcTimeSpan_ExcludeOutOfTime($("#NewChipPlanStartDateTimeSelector").get(0).valueAsDate, $(this).get(0).valueAsDate);
    //                var timeSpan = smbScript.CalcTimeSpan_ExcludeOutOfTime(smbScript.changeStringToDateIcrop($("#NewChipPlanStartDateTimeSelector").get(0).value), smbScript.changeStringToDateIcrop($(this).get(0).value));

    //            	if (timeSpan != null) {
    //                    //最大値を超える場合、作業時間最大値（分）をセット
    //                    if (timeSpan > C_SC3240501_MAXWORKTIME) {
    //                        timeSpan = C_SC3240501_MAXWORKTIME;

    //                        //開始日時と加算時間を元に、終了日時を計算する（営業時間外の時間帯は除外して計算）
    //                        //var newEndDateTime = smbScript.CalcEndDate_ExcludeOutOfTime($("#NewChipPlanStartDateTimeSelector").get(0).valueAsDate, C_SC3240501_MAXWORKTIME);
    //                        var newEndDateTime = smbScript.CalcEndDate_ExcludeOutOfTime(smbScript.changeStringToDateIcrop($("#NewChipPlanStartDateTimeSelector").get(0).value), C_SC3240501_MAXWORKTIME);

    //                        //$("#NewChipPlanFinishDateTimeSelector").get(0).valueAsDate = newEndDateTime;
    //                    	$("#NewChipPlanFinishDateTimeSelector").get(0).value = smbScript.getDateTimelocalDate(newEndDateTime);

    //                    	$("#NewChipPlanFinishTimeLabel").text(smbScript.ConvertDateToStringForDisplay(newEndDateTime));
    //                    }
    //                    $("#NewChipWorkTimeHidden").val(timeSpan);
    //                }
    //            }
    //        }
    //        else {
    //            //ラベルをEmptyにする
    //            $("#NewChipPlanFinishTimeLabel").text("");
    //        }

    //        //必須項目がEmptyなら登録ボタンを非活性にする
    //        $("#NewChipRegisterBtn").attr("disabled", IsMandatoryNewChipTextEmpty());
    //    });
    //作業終了予定日時
    $("#NewChipPlanFinishDateTimeSelector")
        .change(function () {
            //作業終了予定日時がnullでない場合
            //if ($(this).get(0).valueAsDate != null) {
            if ($(this).get(0).value != null && $(this).get(0).value != "") {
                //作業終了予定日時を5分単位で丸め込んだ日時を計算し、DateTimSelectorに設定しなおす
                //var wkDate = new Date(smbScript.RoundUpTimeTo5Units($(this).get(0).valueAsDate));
                var wkDate = new Date(smbScript.RoundUpTimeTo5Units(smbScript.changeStringToDateIcrop($(this).get(0).value)));

                //営業時間外を選択している場合は、翌営業開始時刻に補正する
                var newDate = smbScript.GetDateExcludeOutOfTime(wkDate);

                //$(this).get(0).valueAsDate = newDate;
                $(this).get(0).value = smbScript.getDateTimelocalDate(newDate);

                //DateTimeSelectorで選択した日時を表示用に変換してラベルに設定
                $("#NewChipPlanFinishTimeLabel").text(smbScript.ConvertDateToStringForDisplay(newDate));

                //作業開始予定日時がnullでない場合
                //if ($("#NewChipPlanStartDateTimeSelector").get(0).valueAsDate != null) {
                if ($("#NewChipPlanStartDateTimeSelector").get(0).value != null && $("#NewChipPlanStartDateTimeSelector").get(0).value != "") {
                    //開始日時と終了日時から差分時間を計算し、作業時間に設定する　※営業時間外の時間帯は減算して計算
                    //var timeSpan = smbScript.CalcTimeSpan_ExcludeOutOfTime($("#NewChipPlanStartDateTimeSelector").get(0).valueAsDate, $(this).get(0).valueAsDate);
                    var timeSpan = smbScript.CalcTimeSpan_ExcludeOutOfTime(smbScript.changeStringToDateIcrop($("#NewChipPlanStartDateTimeSelector").get(0).value), smbScript.changeStringToDateIcrop($(this).get(0).value));

                    if (timeSpan != null) {
                        //最大値を超える場合、作業時間最大値（分）をセット
                        if (timeSpan > C_SC3240501_MAXWORKTIME) {
                            timeSpan = C_SC3240501_MAXWORKTIME;

                            //開始日時と加算時間を元に、終了日時を計算する（営業時間外の時間帯は除外して計算）
                            //var newEndDateTime = smbScript.CalcEndDate_ExcludeOutOfTime($("#NewChipPlanStartDateTimeSelector").get(0).valueAsDate, C_SC3240501_MAXWORKTIME);
                            var newEndDateTime = smbScript.CalcEndDate_ExcludeOutOfTime(smbScript.changeStringToDateIcrop($("#NewChipPlanStartDateTimeSelector").get(0).value), C_SC3240501_MAXWORKTIME);

                            //$("#NewChipPlanFinishDateTimeSelector").get(0).valueAsDate = newEndDateTime;
                            $("#NewChipPlanFinishDateTimeSelector").get(0).value = smbScript.getDateTimelocalDate(newEndDateTime);

                            $("#NewChipPlanFinishTimeLabel").text(smbScript.ConvertDateToStringForDisplay(newEndDateTime));
                        }
                        $("#NewChipWorkTimeHidden").val(timeSpan);
                    }
                }

                // 2019/10/31 NSK 鈴木 [No156]予定入庫日時、予定納車日時の自動セット START
                // 予定納車日時を算出し表示する。
                calculateScheDeliDateTime();
                // 2019/10/31 NSK 鈴木 [No156]予定入庫日時、予定納車日時の自動セット END
            }
            else {
                //ラベルをEmptyにする
                $("#NewChipPlanFinishTimeLabel").text("");
            }

            //必須項目がEmptyなら登録ボタンを非活性にする
            $("#NewChipRegisterBtn").attr("disabled", IsMandatoryNewChipTextEmpty());
        });
    // 2020/01/16 NSK 夏目  TR-SVT-TMT-20200110-002 追加jobでサービスの種類を選択できない end

    // 2020/01/16 NSK 夏目  TR-SVT-TMT-20200110-002 追加jobでサービスの種類を選択できない start
    ////納車予定日時
    //$("#NewChipPlanDeriveredDateTimeSelector")
    //    .blur(function () {
    //        //DateTimeSelectorで選択した日時を表示用に変換してラベルに設定
    //        //$("#NewChipPlanDeriveredTimeLabel").text(smbScript.ConvertDateToString($(this).get(0).valueAsDate).substr(0,16));
    //        $("#NewChipPlanDeriveredTimeLabel").text(smbScript.ConvertDateToStringForDisplay(smbScript.changeStringToDateIcrop($(this).get(0).value)));
    //    });
    //納車予定日時
    $("#NewChipPlanDeriveredDateTimeSelector")
        .change(function () {
            //DateTimeSelectorで選択した日時を表示用に変換してラベルに設定
            //$("#NewChipPlanDeriveredTimeLabel").text(smbScript.ConvertDateToString($(this).get(0).valueAsDate).substr(0,16));
            $("#NewChipPlanDeriveredTimeLabel").text(smbScript.ConvertDateToStringForDisplay(smbScript.changeStringToDateIcrop($(this).get(0).value)));
        });
    // 2020/01/16 NSK 夏目  TR-SVT-TMT-20200110-002 追加jobでサービスの種類を選択できない end

	//登録No
    $("#NewChipRegNoText")
        .blur(function () {
			//登録ボタンの活性・非活性を制御
		    $("#NewChipRegisterBtn").attr("disabled", IsMandatoryNewChipTextEmpty());
        });
	//VIN
    $("#NewChipVinText")
        .blur(function () {
			//登録ボタンの活性・非活性を制御
		    $("#NewChipRegisterBtn").attr("disabled", IsMandatoryNewChipTextEmpty());
        });
	//車種
    $("#NewChipVehicleText")
        .blur(function () {
			//登録ボタンの活性・非活性を制御
		    $("#NewChipRegisterBtn").attr("disabled", IsMandatoryNewChipTextEmpty());
        });
	//顧客名
    $("#NewChipCstNameText")
        .blur(function () {
			//登録ボタンの活性・非活性を制御
		    $("#NewChipRegisterBtn").attr("disabled", IsMandatoryNewChipTextEmpty());
        });

    // 2020/01/16 NSK 夏目  TR-SVT-TMT-20200110-002 追加jobでサービスの種類を選択できない start
    ////敬称
    //$("#NewChipTitleList")
    //    .blur(function () {
    //        var e = document.getElementById("NewChipTitleList");
    //        $("#NewChipTitleLabel").text(e.options[e.selectedIndex].text);
	//		//登録ボタンの活性・非活性を制御
	//	    $("#NewChipRegisterBtn").attr("disabled", IsMandatoryNewChipTextEmpty());
    //    });
    //敬称
    $("#NewChipTitleList")
        .change(function () {
            var e = document.getElementById("NewChipTitleList");
            $("#NewChipTitleLabel").text(e.options[e.selectedIndex].text);
            //登録ボタンの活性・非活性を制御
            $("#NewChipRegisterBtn").attr("disabled", IsMandatoryNewChipTextEmpty());
        });
    // 2020/01/16 NSK 夏目  TR-SVT-TMT-20200110-002 追加jobでサービスの種類を選択できない end

    //Mobile
	$("#NewChipMobileText")
        .blur(function () {
			//登録ボタンの活性・非活性を制御
		    $("#NewChipRegisterBtn").attr("disabled", IsMandatoryNewChipTextEmpty());
        });
    //Home
    $("#NewChipHomeText")
        .blur(function () {
			//登録ボタンの活性・非活性を制御
		    $("#NewChipRegisterBtn").attr("disabled", IsMandatoryNewChipTextEmpty());
        });

    // 2020/01/16 NSK 夏目  TR-SVT-TMT-20200110-002 追加jobでサービスの種類を選択できない start
    ////担当SA
    //$("#NewChipSAList")
    //    .blur(function () {
    //        var e = document.getElementById("NewChipSAList");
    //        $("#NewChipSALabel").text(e.options[e.selectedIndex].text);
    //    });
    //担当SA
    $("#NewChipSAList")
        .change(function () {
            var e = document.getElementById("NewChipSAList");
            $("#NewChipSALabel").text(e.options[e.selectedIndex].text);
        });
    // 2020/01/16 NSK 夏目  TR-SVT-TMT-20200110-002 追加jobでサービスの種類を選択できない end

    //顧客住所
    $("#NewChipCstAddressText")
        .blur(function () {
			//登録ボタンの活性・非活性を制御
		    $("#NewChipRegisterBtn").attr("disabled", IsMandatoryNewChipTextEmpty());
        	ControlLengthTextarea($("#NewChipCstAddressText"));
        	AdjusterNewChipTextAreaAddress();
        	//スクロール位置ずれ調整
        	//$("#NewChipDummyBtn").focus();
        	//$("#NewChipDummyBtn").blur();
		    $("#NewChipContent").animate({
		        scrollTop: 0,
		        scrollLeft: 0
		    }, 'normal');
        })
        .bind("paste", function (e) {
            setTimeout(function () {
                ControlLengthTextarea($("#NewChipCstAddressText"));
            	AdjusterNewChipTextAreaAddress();
            }, 0);
        })
        .bind("keyup", function () {
        	ControlLengthTextarea($("#NewChipCstAddressText"));
            AdjusterNewChipTextAreaAddress();
        })
        .bind("keydown", function () {
        	ControlLengthTextarea($("#NewChipCstAddressText"));
        });
    // 2020/01/16 NSK 夏目  TR-SVT-TMT-20200110-002 追加jobでサービスの種類を選択できない start
    ////整備種類
    //$("#NewChipMaintenanceTypeList")
    //    .blur(function () {
    //        var e = document.getElementById("NewChipMaintenanceTypeList");
    //        $("#NewChipMaintenanceTypeLabel").text(e.options[e.selectedIndex].text);
    //    });
    //整備種類
    $("#NewChipMaintenanceTypeList")
        .change(function () {
            var e = document.getElementById("NewChipMaintenanceTypeList");
            $("#NewChipMaintenanceTypeLabel").text(e.options[e.selectedIndex].text);
        });
    // 2020/01/16 NSK 夏目  TR-SVT-TMT-20200110-002 追加jobでサービスの種類を選択できない end

    //整備種類コンボボックスのフォーカスInイベント
    $("#NewChipMaintenanceTypeList").bind('focusin', NewChipSvcClassIDFocusIn);

    //整備種類コンボボックスのフォーカスOutイベント
    // 2020/01/16 NSK 夏目  TR-SVT-TMT-20200110-002 追加jobでサービスの種類を選択できない start
    //$("#NewChipMaintenanceTypeList").bind('focusout', NewChipSvcClassIDFocusOut);
    $("#NewChipMaintenanceTypeList").bind('change', NewChipSvcClassIDFocusOut);
    // 2020/01/16 NSK 夏目  TR-SVT-TMT-20200110-002 追加jobでサービスの種類を選択できない end

    // 2020/01/16 NSK 夏目  TR-SVT-TMT-20200110-002 追加jobでサービスの種類を選択できない start
    ////整備名
    //$("#NewChipMercList")
    //    .blur(function () {
    //        var e = document.getElementById("NewChipMercList");
    //        $("#NewChipMercLabel").text(e.options[e.selectedIndex].text);
    //    });
    //整備名
    $("#NewChipMercList")
        .change(function () {
            var e = document.getElementById("NewChipMercList");
            $("#NewChipMercLabel").text(e.options[e.selectedIndex].text);
        });
    // 2020/01/16 NSK 夏目  TR-SVT-TMT-20200110-002 追加jobでサービスの種類を選択できない end

    //整備名コンボボックスのフォーカスInイベント
    $("#NewChipMercList").bind('focusin', NewChipMercIDFocusIn);

    
    //整備名コンボボックスのフォーカスOutイベント
    // 2020/01/16 NSK 夏目  TR-SVT-TMT-20200110-002 追加jobでサービスの種類を選択できない start
    //$("#NewChipMercList").bind('focusout', NewChipMercIDFocusOut);
    $("#NewChipMercList").bind('change', NewChipMercIDFocusOut);
    // 2020/01/16 NSK 夏目  TR-SVT-TMT-20200110-002 追加jobでサービスの種類を選択できない end

    //ご用命
    $("#NewChipOrderTxt")
        .click(function () {
            //新規予約作成のコントロールにフォーカスが当たらない不具合を解消する
            $(this).focus();
        })
        .blur(function () {
            ControlLengthTextarea($("#NewChipOrderTxt"));
            AdjusterNewChipTextArea($("#NewChipOrderTxt"), $("#NewChipOrderDt"));
            //スクロール位置ずれ調整
            //$("#NewChipDummyBtn").focus();
        	//$("#NewChipDummyBtn").blur();
		    $("#NewChipContent").animate({
		        scrollTop: 0,
		        scrollLeft: 0
		    }, 'normal');        
        })
        .bind("paste", function (e) {
            setTimeout(function () {
                ControlLengthTextarea($("#NewChipOrderTxt"));
                AdjusterNewChipTextArea($("#NewChipOrderTxt"), $("#NewChipOrderDt"));
            }, 0);
        })
        .bind("keyup", function () {
            ControlLengthTextarea($("#NewChipOrderTxt"));
            AdjusterNewChipTextArea($("#NewChipOrderTxt"), $("#NewChipOrderDt"));
        })
        .bind("keydown", function () {
            ControlLengthTextarea($("#NewChipOrderTxt"));
        });

} //SetNewChipTextEvent End

//敬称コンボボックスのフォーカスInイベント
function NewChipNameTitleFocusIn() {

    //フォーカスINされた時の値をグローバル変数に格納
    gSC3240501BeforeNameTitle = $(this).val();
};

//敬称コンボボックスのフォーカスOutイベント
function NewChipNameTitleFocusOut() {    

    //敬称コンボボックスにフォーカスINされた時の値をグローバル変数から取得
    var beforeValue = gSC3240501BeforeNameTitle;

    //次のイベント用に初期化
    gSC3240501BeforeNameTitle = null;
};

//整備種類コンボボックスのフォーカスInイベント
function NewChipSvcClassIDFocusIn() {

    //フォーカスINされた時の値をグローバル変数に格納
    gSC3240501BeforeSvcClassID = $(this).val();
};

//整備種類コンボボックスのフォーカスOutイベント
function NewChipSvcClassIDFocusOut() {    

    //整備種類コンボボックスにフォーカスINされた時の値をグローバル変数から取得
    var beforeValue = gSC3240501BeforeSvcClassID;

    //次のイベント用に初期化
    gSC3240501BeforeSvcClassID = null;

    //フォーカスOUTされた時の値を変数に格納
    var afterValue = $(this).val();

    //選択値が変更されたか確認
    if (beforeValue == afterValue) {
        //変更されていない場合

        //何もしない
        return;
    } 
    else if (afterValue.trim() == "0") {
        //空白を選択された場合

        //商品コンボボックスを初期化
        var e = document.getElementById("NewChipMercList");
        e.options.length = 0; //コンボボックス内のデータをクリア
        $("#NewChipMercLabel").text("");

        //商品コンボボックスを非活性にする
        $("#NewChipMercList").attr("disabled", true);

        // 2015/06/15 TMEJ 河原  サービス入庫予約情報のバリデーション実行タイミング変更に向けた評価用アプリ作成 STRAT

        //商品データなし("0"：商品データ無し)
        $("#NewChipMercList").attr("MERCITEM", 0);

        // 2015/06/15 TMEJ 河原  サービス入庫予約情報のバリデーション実行タイミング変更に向けた評価用アプリ作成 END

        return false;
    } 
    else {
        //変更された

        //登録ボタンを非活性にしておく
        $("#NewChipRegisterBtn").attr("disabled", true);

        //商品情報を取得する（コンボボックス内の値をDBから取得）

        //アクティブインジケータ表示
        gNewChipActiveIndicator.show();

        //オーバーレイ表示
        gNewChipOverlay.show();
    
        //リフレッシュタイマーセット
        commonRefreshTimer(ReDisplayNewChip);

        //「サービス分類ID,標準作業時間」の文字列を分解
        var svcClassInfo = afterValue.split(",");

        //サーバーに渡すパラメータを作成
        var prms = CreateCallBackGetMercParam(C_SC3240501CALLBACK_GETMERC, svcClassInfo[0]);

        //標準作業時間の連動処理
        SetNewChip_StandardWorkTime(svcClassInfo[1]);

        //完成検査有無の連動処理
        SetNewChip_CompleteExaminationArea(svcClassInfo[2]);

        //完成検査有無の連動処理
        SetNewChip_CarWashArea(svcClassInfo[3]);

        // 2019/10/31 NSK 鈴木 [No156]予定入庫日時、予定納車日時の自動セット START
        // 予定納車日時を算出し表示する。
        calculateScheDeliDateTime();
        // 2019/10/31 NSK 鈴木 [No156]予定入庫日時、予定納車日時の自動セット END

        //コールバック開始
        DoCallBack(C_CALLBACK_WND501, prms, SC3240501AfterCallBack, "NewChipSvcClassIDFocusOut");
        return false;
    };
};

//整備名コンボボックスのフォーカスInイベント
function NewChipMercIDFocusIn() {

    //フォーカスINされた時の値をグローバル変数に格納
    gSC3240501BeforeMercID = $(this).val();
};

//整備名コンボボックスのフォーカスOutイベント
function NewChipMercIDFocusOut() {    

    //整備名コンボボックスにフォーカスINされた時の値をグローバル変数から取得
    var beforeValue = gSC3240501BeforeMercID;

    //次のイベント用に初期化
    gSC3240501BeforeMercID = null;

    //フォーカスOUTされた時の値を変数に格納
    var afterValue = $(this).val();

    if (afterValue == null) {
        afterValue = "0";
    }

    //選択値が変更されたか確認
    if (beforeValue == afterValue) {
        //変更されていない場合

        //何もしない
        return;
    } 
    else if (afterValue.trim() == "0") {
        //空白を選択された場合

        //何もしない
        return false;
    } 
    else {
        //変更された

        //「商品ID,標準作業時間」の文字列を分解
        var mercInfo = afterValue.split(",");

        //標準作業時間の連動処理
        SetNewChip_StandardWorkTime(mercInfo[1]);

        // 2019/10/31 NSK 鈴木 [No156]予定入庫日時、予定納車日時の自動セット START
        // 予定納車日時を算出し表示する。
        calculateScheDeliDateTime();
        // 2019/10/31 NSK 鈴木 [No156]予定入庫日時、予定納車日時の自動セット END

        return false;
    };
};


/**	
* 新規予約作成のチップエリアのイベント設定を行う
* 	
*/
function SetEventNewChipChipArea() {

    //新規予約作成のチップエリアタップイベントの登録
    //$("#NewChipTableChipUl .Cassette").bind("touchstart", function (e) {
    $("#NewChipTableChipUl .Cassette").bind("click", function (e) {

        var closestDl = $(e.target).closest('dl');
        var closestDiv = $(e.target).closest('div');
        var selectRowIndex = 0;
        var selectChipIndex = 0;
        var selectChipRezId = -1;
        var selectTxt = "";

        //整備列の背景が灰色の場合、何もせず終了
        if (closestDl.find("dd").hasClass(C_SC3240501CLASS_BACKGROUNDGRAY)) {
            return;
        }

        //複数行表示した場合の、チップ列の背景が灰色の場合、何もせず終了
        if (closestDiv.hasClass(C_SC3240501CLASS_BACKGROUNDGRAY)) {
            return;
        }

        //現在が単行表示の場合
        if (closestDl.attr("openFlg") == "0") {

            //１行表示用ラベルを非表示
            closestDl.find(".SingleLine").hide();

            //保存している最大高さに設定
            closestDl.height(closestDl.attr("maxh"));

            //開く状態のステータスに変更
            closestDl.attr("openFlg", "1");
        }
        //現在が複数行表示の場合
        else {
            //複数行エリアのチェックを一度全て外す
            closestDl.find("div").removeClass(C_SC3240501CLASS_CHECKBLUE);

            //複数行エリア内の選択したチップ情報にチェックをつける
            closestDiv.addClass(C_SC3240501CLASS_CHECKBLUE);

            //未選択表示を選択した場合
            if (closestDiv.hasClass("Unselected")) {
                //未選択用のテキストを１行表示用ラベルのテキストに設定する
                selectTxt = $("#WordChipUnselectedHidden").val();
            }
            else {
                //選択したチップ情報のテキストを１行表示用ラベルのテキストに設定する
                selectTxt = closestDl.find(".CheckBlue").children("span").text()
            }
            closestDl.find("dd").children(".SingleLine").text(selectTxt);

            //選択されたチップの予約ID(未選択なら-1)
            selectChipRezId = closestDiv.attr("rezid");

            //選択したチップのインデックスを親dlに保存する
            closestDl.attr("selectrezid", selectChipRezId);

            //１行表示用ラベルを表示する
            closestDl.find("dd").children(".SingleLine").css("display", "block");

            //自チップの作業内容ID　＝　整備に紐付いている作業内容ID　の場合
            //if ($("#MyJobDtlIdHidden").val() == selectChipRezId) {
            if (($("#MyJobDtlIdHidden").val() == selectChipRezId) || closestDiv.hasClass("Unselected")) {
                //自チップは太字
                //closestDl.find("dd").children(".SingleLine").addClass(C_SC3240501CLASS_FONTBOLD);
                closestDl.find("dd").children(".SingleLine").css("font-weight", "bold");
            }
            else {
                //他チップは細字
                //closestDl.find("dd").children(".SingleLine").addClass(C_SC3240501CLASS_FONTNORMAL);
                closestDl.find("dd").children(".SingleLine").css("font-weight", "normal");
            }

            //１行表示時の高さを設定
            closestDl.height(29);

            //閉じる状態のステータスに変更
            closestDl.attr("openFlg", "0");

            selectRowIndex = closestDl.attr("rowindex");
            selectChipIndex = closestDiv.attr("chipindex");

        }
    });
} //SetEventNewChipChipArea End

/**	
* 新規予約作成の予約有無エリアの設定を行う
* 	
*/	
function SetNewChipReservationArea() {

    //予約フラグを1(予約)にする
    $("#NewChipRezFlgHidden").val("0");

    //選択エリアを青文字テキストにする
    $("#NewChipReserveLi dd").addClass(C_SC3240501CLASS_TEXTBLUE);

    //「予約」に青チェック
    $("#NewChipReserveLi dd:first").addClass(C_SC3240501CLASS_CHECKBLUE);

    //新規予約作成の「予約」クリック時のイベント登録
    $("#NewChipReserveLi dd:first").bind(C_SC3240501_TOUCH, function () {

        //青チェックを付け直す
        $("#NewChipReserveLi dd").removeClass(C_SC3240501CLASS_CHECKBLUE);
        $("#NewChipReserveLi dd:first").addClass(C_SC3240501CLASS_CHECKBLUE);

        //予約フラグを1(予約)にする
        $("#NewChipRezFlgHidden").val("0");

    });

    //新規予約作成の「飛び込み」クリック時のイベント登録
    $("#NewChipReserveLi dd:last").bind(C_SC3240501_TOUCH, function () {

        //青チェックを付け直す
        $("#NewChipReserveLi dd").removeClass(C_SC3240501CLASS_CHECKBLUE);
        $("#NewChipReserveLi dd:last").addClass(C_SC3240501CLASS_CHECKBLUE);

        //予約フラグを0(飛び込み)にする
        $("#NewChipRezFlgHidden").val("1");

    });
} //SetNewChipReservationArea End

/**	
* 新規予約作成の完成検査有無エリアのイベント設定を行う
* 	
*/
function SetEventNewChipCompleteExaminationArea() {
    // 2019/10/31 NSK 鈴木 [No156]予定入庫日時、予定納車日時の自動セット START
    // //予約フラグを1(予約)にする
    // $("#NewChipCompleteExaminationFlgHidden").val("1");
    // 2019/10/31 NSK 鈴木 [No156]予定入庫日時、予定納車日時の自動セット END

    //選択エリアを青文字テキストにする
    $("#NewChipCompleteExaminationLi dd").addClass(C_SC3240501CLASS_TEXTBLUE);

    // 2020/01/08 NSK 鈴木 TR-V4-BMS-20191001-001 (トライ店システム評価)コールセンター業務における洗車要否判断の効率化検証 START
    // //「予約」に青チェック
    // $("#NewChipCompleteExaminationLi dd:first").addClass(C_SC3240501CLASS_CHECKBLUE);

    if ("1" == $("#NewChipCompleteExaminationFlgHidden").val()) {
        // 完成検査フラグが1（あり）の場合
        $("#NewChipCompleteExaminationLi dd:first").addClass(C_SC3240501CLASS_CHECKBLUE);

    } else {
        // 完成検査フラグが0（なし）の場合
        $("#NewChipCompleteExaminationLi dd:last").addClass(C_SC3240501CLASS_CHECKBLUE);
    }
    // 2020/01/08 NSK 鈴木 TR-V4-BMS-20191001-001 (トライ店システム評価)コールセンター業務における洗車要否判断の効率化検証 END

    //新規予約作成の「有り」クリック時のイベント登録
    $("#NewChipCompleteExaminationLi dd:first").bind(C_SC3240501_TOUCH, function () {

        //青チェックを付け直す
        $("#NewChipCompleteExaminationLi dd").removeClass(C_SC3240501CLASS_CHECKBLUE);
        $("#NewChipCompleteExaminationLi dd:first").addClass(C_SC3240501CLASS_CHECKBLUE);

        //完成検査フラグを1(有り)にする
        $("#NewChipCompleteExaminationFlgHidden").val("1");

        // 2019/10/31 NSK 鈴木 [No156]予定入庫日時、予定納車日時の自動セット START
        // 予定納車日時を算出し表示する。
        calculateScheDeliDateTime();
        // 2019/10/31 NSK 鈴木 [No156]予定入庫日時、予定納車日時の自動セット END
    });

    //新規予約作成の「無し」クリック時のイベント登録
    $("#NewChipCompleteExaminationLi dd:last").bind(C_SC3240501_TOUCH, function () {

        //青チェックを付け直す
        $("#NewChipCompleteExaminationLi dd").removeClass(C_SC3240501CLASS_CHECKBLUE);
        $("#NewChipCompleteExaminationLi dd:last").addClass(C_SC3240501CLASS_CHECKBLUE);

        //完成検査フラグを0(無し)にする
        $("#NewChipCompleteExaminationFlgHidden").val("0");

        // 2019/10/31 NSK 鈴木 [No156]予定入庫日時、予定納車日時の自動セット START
        // 予定納車日時を算出し表示する。
        calculateScheDeliDateTime();
        // 2019/10/31 NSK 鈴木 [No156]予定入庫日時、予定納車日時の自動セット END
    });
}

/**	
* 新規予約作成の洗車有無エリアの設定を行う
* 	
*/
function SetNewChipCarWashArea() {

    // 2019/10/31 NSK 鈴木 [No156]予定入庫日時、予定納車日時の自動セット START
    // //洗車フラグを1(有り)にする
    // $("#NewChipCarWashFlgHidden").val("1");
    // 2019/10/31 NSK 鈴木 [No156]予定入庫日時、予定納車日時の自動セット END

    //選択エリアを青文字テキストにする
    $("#NewChipCarWashLi dd").addClass(C_SC3240501CLASS_TEXTBLUE);

    // 2020/01/08 NSK 鈴木 TR-V4-BMS-20191001-001 (トライ店システム評価)コールセンター業務における洗車要否判断の効率化検証 START
    // //「有り」に青チェック
    // $("#NewChipCarWashLi dd:first").addClass(C_SC3240501CLASS_CHECKBLUE);

    if ("1" == $("#NewChipCarWashFlgHidden").val()) {
        // 洗車フラグが1（あり）の場合
        $("#NewChipCarWashLi dd:first").addClass(C_SC3240501CLASS_CHECKBLUE);

    } else {
        // 洗車フラグが0（なし）の場合
        $("#NewChipCarWashLi dd:last").addClass(C_SC3240501CLASS_CHECKBLUE);
    }
    // 2020/01/08 NSK 鈴木 TR-V4-BMS-20191001-001 (トライ店システム評価)コールセンター業務における洗車要否判断の効率化検証 END

    //新規予約作成の「有り」クリック時のイベント登録
    $("#NewChipCarWashLi dd:first").bind(C_SC3240501_TOUCH, function () {

        //青チェックを付け直す
        $("#NewChipCarWashLi dd").removeClass(C_SC3240501CLASS_CHECKBLUE);
        $("#NewChipCarWashLi dd:first").addClass(C_SC3240501CLASS_CHECKBLUE);

        //洗車フラグを1(有り)にする
        $("#NewChipCarWashFlgHidden").val("1");

        // 2019/10/31 NSK 鈴木 [No156]予定入庫日時、予定納車日時の自動セット START
        // 予定納車日時を算出し表示する。
        calculateScheDeliDateTime();
        // 2019/10/31 NSK 鈴木 [No156]予定入庫日時、予定納車日時の自動セット END
    });

    //新規予約作成の「無し」クリック時のイベント登録
    $("#NewChipCarWashLi dd:last").bind(C_SC3240501_TOUCH, function () {

        //青チェックを付け直す
        $("#NewChipCarWashLi dd").removeClass(C_SC3240501CLASS_CHECKBLUE);
        $("#NewChipCarWashLi dd:last").addClass(C_SC3240501CLASS_CHECKBLUE);

        //洗車フラグを0(無し)にする
        $("#NewChipCarWashFlgHidden").val("0");

        // 2019/10/31 NSK 鈴木 [No156]予定入庫日時、予定納車日時の自動セット START
        // 予定納車日時を算出し表示する。
        calculateScheDeliDateTime();
        // 2019/10/31 NSK 鈴木 [No156]予定入庫日時、予定納車日時の自動セット END
    });

} //SetNewChipCarWashArea End

/**	
* 新規予約作成の待ち方エリアの設定を行う。
* 	
* @param {-} -
* @return {-} -
*
*/
function SetNewChipWaitingArea() {

    //店内待ちフラグを0(店内)にする
    $("#NewChipWaitingFlgHidden").val("0");

    //選択エリアを青文字テキストにする
    $("#NewChipWaitingLi dd").addClass(C_SC3240501CLASS_TEXTBLUE);

    //「店内」に青チェック
    $("#NewChipWaitingLi dd:first").addClass(C_SC3240501CLASS_CHECKBLUE);

    //新規予約作成の「店内」クリック時のイベント登録
    $("#NewChipWaitingLi dd:first").bind(C_SC3240501_TOUCH, function () {

        //青チェックを付け直す
        $("#NewChipWaitingLi dd").removeClass(C_SC3240501CLASS_CHECKBLUE);
        $("#NewChipWaitingLi dd:first").addClass(C_SC3240501CLASS_CHECKBLUE);

        //店内待ちフラグを0(店内)にする
        $("#NewChipWaitingFlgHidden").val("0");

    });

    //新規予約作成の「店外」クリック時のイベント登録
    $("#NewChipWaitingLi dd:last").bind(C_SC3240501_TOUCH, function () {

        //青チェックを付け直す
        $("#NewChipWaitingLi dd").removeClass(C_SC3240501CLASS_CHECKBLUE);
        $("#NewChipWaitingLi dd:last").addClass(C_SC3240501CLASS_CHECKBLUE);

        //店内待ちフラグを4(店外)にする
        $("#NewChipWaitingFlgHidden").val("4");

    });

} //SetNewChipWaitingArea End

/**	
* 整備種類・整備名称の変更時、標準作業時間の連動処理を行う
* 	
*/
function SetNewChip_StandardWorkTime(standardWorkTime) {

    var min;

    //標準作業時間が空白、もしくは0の場合
    if (standardWorkTime == "" || standardWorkTime == "0") {

        //ストールのインターバル時間をセット
        min = gResizeInterval;
    }
    //それ以外は、標準作業時間をストールのインターバル時間単位で丸め込む
    else {
        min = smbScript.RoundUpToNumUnits(standardWorkTime, gResizeInterval, gResizeInterval, C_SC3240501_MAXWORKTIME);
    }

    $("#NewChipWorkTimeHidden").val(min);

    //作業開始日時がnullでない場合、設定した作業時間に合わせて作業終了予定時間を変更する
    //var startDateTime = $("#NewChipPlanStartDateTimeSelector").get(0).valueAsDate;
    var startDateTime = smbScript.changeStringToDateIcrop($("#NewChipPlanStartDateTimeSelector").get(0).value);

    if (startDateTime != null) {
        //開始日時と加算時間を元に、終了日時を計算する（営業時間外の時間帯は除外して計算）
        var newEndDateTime = smbScript.CalcEndDate_ExcludeOutOfTime(startDateTime, min);

        //$("#NewChipPlanFinishDateTimeSelector").get(0).valueAsDate = newEndDateTime;
        $("#NewChipPlanFinishDateTimeSelector").get(0).value = smbScript.getDateTimelocalDate(newEndDateTime);
    	$("#NewChipPlanFinishTimeLabel").text(smbScript.ConvertDateToStringForDisplay(newEndDateTime));

        //必須項目がEmptyなら登録ボタンを非活性にする
        $("#NewChipRegisterBtn").attr("disabled", IsMandatoryNewChipTextEmpty());
    }

    return false;
}

/**	
* 整備種類の変更時、完成検査有無の連動処理を行う
* 	
*/
function SetNewChip_CompleteExaminationArea(svcClassType) {

	if(svcClassType.trim() != ""){
	    //サービス分類区分が「1:EM」または、「2:PM」の場合
		if ((svcClassType == "1") || (svcClassType == "2")){
	        //青チェックを付け直す
	        $("#NewChipCompleteExaminationLi dd").removeClass(C_SC3240501CLASS_CHECKBLUE);
	        $("#NewChipCompleteExaminationLi dd:last").addClass(C_SC3240501CLASS_CHECKBLUE);

	        //完成検査フラグを0(無し)にする
	        $("#NewChipCompleteExaminationFlgHidden").val("0");
	    }
	    else {
	        //青チェックを付け直す
	        $("#NewChipCompleteExaminationLi dd").removeClass(C_SC3240501CLASS_CHECKBLUE);
	        $("#NewChipCompleteExaminationLi dd:first").addClass(C_SC3240501CLASS_CHECKBLUE);

	        //完成検査フラグを1(有り)にする
	        $("#NewChipCompleteExaminationFlgHidden").val("1");
	    }
	}
	
    return false;
}

/**	
* 整備種類の変更時、洗車有無の連動処理を行う
* 	
*/
function SetNewChip_CarWashArea(carWashNeedFlg) {

	if(carWashNeedFlg.trim() != ""){
	    //洗車必要フラグが「1」の場合
	    if (carWashNeedFlg == "1") {
	        //青チェックを付け直す
	        $("#NewChipCarWashLi dd").removeClass(C_SC3240501CLASS_CHECKBLUE);
	        $("#NewChipCarWashLi dd:first").addClass(C_SC3240501CLASS_CHECKBLUE);

	        //洗車フラグを1(有り)にする
	    	$("#NewChipCarWashFlgHidden").val("1");
	    }
	    else {
	        //青チェックを付け直す
	        $("#NewChipCarWashLi dd").removeClass(C_SC3240501CLASS_CHECKBLUE);
	        $("#NewChipCarWashLi dd:last").addClass(C_SC3240501CLASS_CHECKBLUE);

	        //洗車フラグを0(無し)にする
	    	$("#NewChipCarWashFlgHidden").val("0");
	    }
	}

    return false;
}

/**	
* 顧客検索のテキストイベントの設定を行う
*
*/
function SetSearchTextEvent() {

    //検索用テキストボックス
    $("#SearchText").keydown(function (e) {
        if (e.which == 13) {
            SearchCustomer();
            $("#SearchText").blur();
            return false;
        }
    });
}

/**	
* テキストエリア内の文字列長制御を行う
* 	
* @param {$(textarea)} ta
*
*/
function ControlLengthTextarea(ta) {

    //許容する最大バイト数
    var maxLen = ta.attr("maxlen");
    var overFlg = 0;
    var v = ta.val();

    if (v.length > maxLen) {
        var overFlg = 1;
    }

    //許容する最大バイト数を超えていた場合のみ、切り出し処理を実施してセットしなおす
    if (overFlg == "1") {
        var AfterStr = v.substr(0, maxLen);
        ta.val(AfterStr);
    }
}

