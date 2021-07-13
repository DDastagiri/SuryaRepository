/** 
* @fileOverview SC3240201.Event.js
* 
* @author TMEJ 岩城
* @version 1.0.0
* 更新： 2013/12/04 TMEJ 下村 タブレット版SMB チーフテクニシャン機能開発
* 更新： 2013/12/26 TMEJ 下村 次世代e-CRBタブレット(サービス) iOS7.0 VersionUp対応
* 更新： 2014/04/01 TMEJ 丁 次世代e-CRBタブレット(サービス) チーフテクニシャン機能開発
* 更新： 2015/06/15 TMEJ 河原  サービス入庫予約情報のバリデーション実行タイミング変更に向けた評価用アプリ作成
* 更新： 2020/01/16 NSK 夏目  TR-SVT-TMT-20200110-002 追加jobでサービスの種類を選択できない
* 更新： 2020/01/08 NSK 鈴木 TR-V4-BMS-20191001-001 (トライ店システム評価)コールセンター業務における洗車要否判断の効率化検証 END
*/

/**	
* チップ詳細(小)のテキストイベントの設定を行う
*
*/

function SetDetailSTextEvent() {

    //2013/12/26 TMEJ 下村 次世代e-CRBタブレット(サービス) iOS7.0 VersionUp対応 START
    ////来店予定日時
    //$("#DetailSPlanVisitDateTimeSelector")
    //    .blur(function () {
    //        //DateTimeSelectorで選択した日時を表示用に変換してラベルに設定
    //        $("#DetailSPlanVisitLabel").text(smbScript.ConvertDateOrTime($(this).get(0).valueAsDate, $("#hidShowDate").val()));
    //        //チップ詳細(大)に反映
    //        $("#DetailLPlanVisitDateTimeSelector").get(0).valueAsDate = $(this).get(0).valueAsDate;
    //        $("#DetailLPlanVisitLabel").text($("#DetailSPlanVisitLabel").text());
    //    });
    //
    ////作業開始予定日時
    //$("#DetailSPlanStartDateTimeSelector")
    //    .blur(function () {
    //        //作業開始予定日時がnullでない場合
    //        if ($(this).get(0).valueAsDate != null) {
    //            //作業開始予定日時を5分単位で丸め込んだ日時を計算し、DateTimSelectorに設定しなおす
    //            var wkDate = new Date(smbScript.RoundUpTimeTo5Units($(this).get(0).valueAsDate));
    //
    //            ////営業時間外を選択している場合は、指定した日の営業開始時刻に補正する
    //            //var newDate = smbScript.GetStartDateExcludeOutOfTime(wkDate);
    //            //営業時間外を選択している場合は、翌営業開始時刻に補正する
    //            var newDate = smbScript.GetDateExcludeOutOfTime(wkDate);
    //
    //            $(this).get(0).valueAsDate = newDate;
    //            //DateTimeSelectorで選択した日時を表示用に変換してラベルに設定
    //            $("#DetailSPlanStartLabel").text(smbScript.ConvertDateOrTime(newDate, $("#hidShowDate").val()));
    //
    //            //チップ詳細(大)に反映
    //            $("#DetailLPlanStartDateTimeSelector").get(0).valueAsDate = newDate;
    //            $("#DetailLPlanStartLabel").text($("#DetailSPlanStartLabel").text());
    //
    //            //作業終了予定日時がnullでない場合
    //            if ($("#DetailSPlanFinishDateTimeSelector").get(0).valueAsDate != null) {
    //                //開始日時と終了日時から差分時間を計算し、作業時間に設定する　※営業時間外の時間帯は減算して計算
    //                //var timeSpan = smbScript.CalcTimeSpan($(this).get(0).valueAsDate, $("#DetailSPlanFinishDateTimeSelector").get(0).valueAsDate);
    //                var timeSpan = smbScript.CalcTimeSpan_ExcludeOutOfTime($(this).get(0).valueAsDate, $("#DetailSPlanFinishDateTimeSelector").get(0).valueAsDate);
    //                if (timeSpan != null) {
    //                    //最大値を超える場合、作業時間最大値（分）をセット
    //                    if (timeSpan > C_SC3240201_MAXWORKTIME) {
    //                        timeSpan = C_SC3240201_MAXWORKTIME;
    //
    //                        //開始日時と加算時間を元に、終了日時を計算する（営業時間外の時間帯は除外して計算）
    //                        var newEndDateTime = smbScript.CalcEndDate_ExcludeOutOfTime($("#DetailSPlanStartDateTimeSelector").get(0).valueAsDate, C_SC3240201_MAXWORKTIME);
    //
    //                        $("#DetailSPlanFinishDateTimeSelector").get(0).valueAsDate = newEndDateTime;
    //                        $("#DetailSPlanFinishLabel").text(smbScript.ConvertDateOrTime(newEndDateTime, $("#hidShowDate").val()));
    //
    //                        //チップ詳細(大)に反映
    //                        $("#DetailLPlanFinishDateTimeSelector").get(0).valueAsDate = newEndDateTime;
    //                        $("#DetailLPlanFinishLabel").text($("#DetailSPlanFinishLabel").text());
    //                    }
    //                    $("#DetailSWorkTimeTxt").val(timeSpan);
    //                    $("#DetailLWorkTimeTxt").val(timeSpan);
    //                    $("#DetailSWorkTimeLabel").text(timeSpan + $("#WordWorkTimeUnitHidden").val());
    //                    $("#DetailLWorkTimeLabel").text(timeSpan + $("#WordWorkTimeUnitHidden").val());
    //                }
    //            }
    //        }
    //        else {
    //            //ラベルをEmptyにする
    //            $("#DetailSPlanStartLabel").text("");
    //
    //            //チップ詳細(大)に反映
    //            $("#DetailLPlanStartDateTimeSelector").get(0).valueAsDate = null;
    //            $("#DetailLPlanStartLabel").text("");
    //        }
    //        
    //        //必須項目がEmptyなら登録ボタンを非活性にする
    //        $("#DetailRegisterBtn").attr("disabled", IsMandatoryChipDetailEmpty());
    //    });
    //
    ////作業終了予定日時
    //$("#DetailSPlanFinishDateTimeSelector")
    //    .blur(function () {
    //        //作業終了予定日時がnullでない場合
    //        if ($(this).get(0).valueAsDate != null) {
    //            //作業終了予定日時を5分単位で丸め込んだ日時を計算し、DateTimSelectorに設定しなおす
    //            var wkDate = new Date(smbScript.RoundUpTimeTo5Units($(this).get(0).valueAsDate));
    //
    //            ////営業時間外を選択している場合は、指定した日の営業終了時刻に補正する
    //            //var newDate = smbScript.GetEndDateExcludeOutOfTime(wkDate);
    //            //営業時間外を選択している場合は、翌営業開始時刻に補正する
    //            var newDate = smbScript.GetDateExcludeOutOfTime(wkDate);
    //
    //            $(this).get(0).valueAsDate = newDate;
    //            //DateTimeSelectorで選択した日時を表示用に変換してラベルに設定
    //            $("#DetailSPlanFinishLabel").text(smbScript.ConvertDateOrTime(newDate, $("#hidShowDate").val()));
    //
    //            //チップ詳細(大)に反映
    //            $("#DetailLPlanFinishDateTimeSelector").get(0).valueAsDate = newDate;
    //            $("#DetailLPlanFinishLabel").text($("#DetailSPlanFinishLabel").text());
    //
    //            //作業開始予定日時がnullでない場合
    //            if ($("#DetailSPlanStartDateTimeSelector").get(0).valueAsDate != null) {
    //                //開始日時と終了日時から差分時間を計算し、作業時間に設定する　※営業時間外の時間帯は減算して計算
    //                //var timeSpan = smbScript.CalcTimeSpan($("#DetailSPlanStartDateTimeSelector").get(0).valueAsDate, $(this).get(0).valueAsDate);
    //                var timeSpan = smbScript.CalcTimeSpan_ExcludeOutOfTime($("#DetailSPlanStartDateTimeSelector").get(0).valueAsDate, $(this).get(0).valueAsDate);
    //                if (timeSpan != null) {
    //                    //最大値を超える場合、作業時間最大値（分）をセット
    //                    if (timeSpan > C_SC3240201_MAXWORKTIME) {
    //                        timeSpan = C_SC3240201_MAXWORKTIME;
    //
    //                        //開始日時と加算時間を元に、終了日時を計算する（営業時間外の時間帯は除外して計算）
    //                        var newEndDateTime = smbScript.CalcEndDate_ExcludeOutOfTime($("#DetailSPlanStartDateTimeSelector").get(0).valueAsDate, C_SC3240201_MAXWORKTIME);
    //
    //                        $("#DetailSPlanFinishDateTimeSelector").get(0).valueAsDate = newEndDateTime;
    //                        $("#DetailSPlanFinishLabel").text(smbScript.ConvertDateOrTime(newEndDateTime, $("#hidShowDate").val()));
    //
    //                        //チップ詳細(大)に反映
    //                        $("#DetailLPlanFinishDateTimeSelector").get(0).valueAsDate = newEndDateTime;
    //                        $("#DetailLPlanFinishLabel").text($("#DetailSPlanFinishLabel").text());
    //                    }
    //                    $("#DetailSWorkTimeTxt").val(timeSpan);
    //                    $("#DetailLWorkTimeTxt").val(timeSpan);
    //                    $("#DetailSWorkTimeLabel").text(timeSpan + $("#WordWorkTimeUnitHidden").val());
    //                    $("#DetailLWorkTimeLabel").text(timeSpan + $("#WordWorkTimeUnitHidden").val());
    //                }
    //            }
    //        }
    //        else {
    //            //ラベルをEmptyにする
    //            $("#DetailSPlanFinishLabel").text("");
    //
    //            //チップ詳細(大)に反映
    //            $("#DetailLPlanFinishDateTimeSelector").get(0).valueAsDate = null;
    //            $("#DetailLPlanFinishLabel").text("");
    //        }
    //
    //        //必須項目がEmptyなら登録ボタンを非活性にする
    //        $("#DetailRegisterBtn").attr("disabled", IsMandatoryChipDetailEmpty());
    //    });
    //
    ////納車予定日時
    //$("#DetailSPlanDeriveredDateTimeSelector")
    //    .blur(function () {
    //        //DateTimeSelectorで選択した日時を表示用に変換してラベルに設定
    //        $("#DetailSPlanDeriveredLabel").text(smbScript.ConvertDateOrTime($(this).get(0).valueAsDate, $("#hidShowDate").val()));
    //        //チップ詳細(大)に反映
    //        $("#DetailLPlanDeriveredDateTimeSelector").get(0).valueAsDate = $(this).get(0).valueAsDate;
    //        $("#DetailLPlanDeriveredLabel").text($("#DetailSPlanDeriveredLabel").text());
    //    });
    //
    ////作業開始実績時間
    //$("#DetailSProcessStartDateTimeSelector")
    //    .blur(function () {
    //        //DateTimeSelectorで選択した日時を表示用に変換してラベルに設定
    //        $("#DetailSProcessStartLabel").text(smbScript.ConvertDateOrTime($(this).get(0).valueAsDate, $("#hidShowDate").val()));
    //        //チップ詳細(大)に反映
    //        $("#DetailLProcessStartDateTimeSelector").get(0).valueAsDate = $(this).get(0).valueAsDate;
    //        $("#DetailLProcessStartLabel").text($("#DetailSProcessStartLabel").text());
    //    });
    //
    ////作業終了実績時間
    //$("#DetailSProcessFinishDateTimeSelector")
    //    .blur(function () {
    //        $("#DetailSProcessFinishLabel").text(smbScript.ConvertDateOrTime($(this).get(0).valueAsDate, $("#hidShowDate").val()));
    //        //チップ詳細(大)に反映
    //        $("#DetailLProcessFinishDateTimeSelector").get(0).valueAsDate = $(this).get(0).valueAsDate;
    //        $("#DetailLProcessFinishLabel").text($("#DetailSProcessFinishLabel").text());
    //    });

    // 2020/01/16 NSK 夏目  TR-SVT-TMT-20200110-002 追加jobでサービスの種類を選択できない start
    ////来店予定日時
    //$("#DetailSPlanVisitDateTimeSelector")
    //    .blur(function () {
    //        //DateTimeSelectorで選択した日時を表示用に変換してラベルに設定
    //        $("#DetailSPlanVisitLabel").text(smbScript.ConvertDateOrTime(smbScript.changeStringToDateIcrop($(this).get(0).value), $("#hidShowDate").val()));

    //        //チップ詳細(大)に反映
    //        $("#DetailLPlanVisitDateTimeSelector").get(0).value = $(this).get(0).value;
    //        $("#DetailLPlanVisitLabel").text($("#DetailSPlanVisitLabel").text());
    //    });
    //来店予定日時
    $("#DetailSPlanVisitDateTimeSelector")
        .change(function () {
            //DateTimeSelectorで選択した日時を表示用に変換してラベルに設定
            $("#DetailSPlanVisitLabel").text(smbScript.ConvertDateOrTime(smbScript.changeStringToDateIcrop($(this).get(0).value), $("#hidShowDate").val()));

            //チップ詳細(大)に反映
            $("#DetailLPlanVisitDateTimeSelector").get(0).value = $(this).get(0).value;
            $("#DetailLPlanVisitLabel").text($("#DetailSPlanVisitLabel").text());
        });
    // 2020/01/16 NSK 夏目  TR-SVT-TMT-20200110-002 追加jobでサービスの種類を選択できない end

    // 2020/01/16 NSK 夏目  TR-SVT-TMT-20200110-002 追加jobでサービスの種類を選択できない start
    ////作業開始予定日時
    //$("#DetailSPlanStartDateTimeSelector")
    //    .blur(function () {
    //
    //        //作業開始予定日時がnullでない場合
    //        if ($(this).get(0).value != null && $(this).get(0).value != "") {
    //
    //            //作業開始予定日時を5分単位で丸め込んだ日時を計算し、DateTimSelectorに設定しなおす
    //            var wkDate = new Date(smbScript.RoundUpTimeTo5Units(smbScript.changeStringToDateIcrop($(this).get(0).value)));
    //
    //            //営業時間外を選択している場合は、翌営業開始時刻に補正する
    //            var newDate = smbScript.GetDateExcludeOutOfTime(wkDate);
    //
    //            $(this).get(0).value = smbScript.getDateTimelocalDate(newDate);
    //
    //            //DateTimeSelectorで選択した日時を表示用に変換してラベルに設定
    //            $("#DetailSPlanStartLabel").text(smbScript.ConvertDateOrTime(newDate, $("#hidShowDate").val()));
    //
    //            //チップ詳細(大)に反映
    //            $("#DetailLPlanStartDateTimeSelector").get(0).value = smbScript.getDateTimelocalDate(newDate);
    //            $("#DetailLPlanStartLabel").text($("#DetailSPlanStartLabel").text());
    //
    //            //作業終了予定日時がnullでない場合
    //            if ($("#DetailSPlanFinishDateTimeSelector").get(0).value != null && $("#DetailSPlanFinishDateTimeSelector").get(0).value != "") {
    //
    //                //開始日時と終了日時から差分時間を計算し、作業時間に設定する　※営業時間外の時間帯は減算して計算
    //                var timeSpan = smbScript.CalcTimeSpan_ExcludeOutOfTime(smbScript.changeStringToDateIcrop($(this).get(0).value), smbScript.changeStringToDateIcrop($("#DetailSPlanFinishDateTimeSelector").get(0).value));
    //
    //                if (timeSpan != null) {
    //                    //最大値を超える場合、作業時間最大値（分）をセット
    //                    if (timeSpan > C_SC3240201_MAXWORKTIME) {
    //                        timeSpan = C_SC3240201_MAXWORKTIME;
    //
    //                        //開始日時と加算時間を元に、終了日時を計算する（営業時間外の時間帯は除外して計算）
    //                        var newEndDateTime = smbScript.CalcEndDate_ExcludeOutOfTime(smbScript.changeStringToDateIcrop($("#DetailSPlanStartDateTimeSelector").get(0).value), C_SC3240201_MAXWORKTIME);
    //                        $("#DetailSPlanFinishDateTimeSelector").get(0).value = smbScript.getDateTimelocalDate(newEndDateTime);
    //                        $("#DetailSPlanFinishLabel").text(smbScript.ConvertDateOrTime(newEndDateTime, $("#hidShowDate").val()));
    //
    //                        //チップ詳細(大)に反映
    //                        $("#DetailLPlanFinishDateTimeSelector").get(0).value = smbScript.getDateTimelocalDate(newEndDateTime);
    //                        $("#DetailLPlanFinishLabel").text($("#DetailSPlanFinishLabel").text());
    //                    }
    //                    $("#DetailSWorkTimeTxt").val(timeSpan);
    //                    $("#DetailLWorkTimeTxt").val(timeSpan);
    //                    $("#DetailSWorkTimeLabel").text(timeSpan + $("#WordWorkTimeUnitHidden").val());
    //                    $("#DetailLWorkTimeLabel").text(timeSpan + $("#WordWorkTimeUnitHidden").val());
    //                }
    //            }
    //        }
    //        else {
    //            //ラベルをEmptyにする
    //            $("#DetailSPlanStartLabel").text("");
    //
    //            //チップ詳細(大)に反映
    //            $("#DetailLPlanStartDateTimeSelector").get(0).value = null;
    //            $("#DetailLPlanStartLabel").text("");
    //        }
    //      
    //        //必須項目がEmptyなら登録ボタンを非活性にする
    //        $("#DetailRegisterBtn").attr("disabled", IsMandatoryChipDetailEmpty());
    //    });
    //作業開始予定日時
    $("#DetailSPlanStartDateTimeSelector")
        .change(function () {

            //作業開始予定日時がnullでない場合
            if ($(this).get(0).value != null && $(this).get(0).value != "") {

                //作業開始予定日時を5分単位で丸め込んだ日時を計算し、DateTimSelectorに設定しなおす
                var wkDate = new Date(smbScript.RoundUpTimeTo5Units(smbScript.changeStringToDateIcrop($(this).get(0).value)));

                //営業時間外を選択している場合は、翌営業開始時刻に補正する
                var newDate = smbScript.GetDateExcludeOutOfTime(wkDate);

                $(this).get(0).value = smbScript.getDateTimelocalDate(newDate);

                //DateTimeSelectorで選択した日時を表示用に変換してラベルに設定
                $("#DetailSPlanStartLabel").text(smbScript.ConvertDateOrTime(newDate, $("#hidShowDate").val()));

                //チップ詳細(大)に反映
                $("#DetailLPlanStartDateTimeSelector").get(0).value = smbScript.getDateTimelocalDate(newDate);
                $("#DetailLPlanStartLabel").text($("#DetailSPlanStartLabel").text());

                //作業終了予定日時がnullでない場合
                if ($("#DetailSPlanFinishDateTimeSelector").get(0).value != null && $("#DetailSPlanFinishDateTimeSelector").get(0).value != "") {

                    //開始日時と終了日時から差分時間を計算し、作業時間に設定する　※営業時間外の時間帯は減算して計算
                    var timeSpan = smbScript.CalcTimeSpan_ExcludeOutOfTime(smbScript.changeStringToDateIcrop($(this).get(0).value), smbScript.changeStringToDateIcrop($("#DetailSPlanFinishDateTimeSelector").get(0).value));

                    if (timeSpan != null) {
                        //最大値を超える場合、作業時間最大値（分）をセット
                        if (timeSpan > C_SC3240201_MAXWORKTIME) {
                            timeSpan = C_SC3240201_MAXWORKTIME;

                            //開始日時と加算時間を元に、終了日時を計算する（営業時間外の時間帯は除外して計算）
                            var newEndDateTime = smbScript.CalcEndDate_ExcludeOutOfTime(smbScript.changeStringToDateIcrop($("#DetailSPlanStartDateTimeSelector").get(0).value), C_SC3240201_MAXWORKTIME);
                            $("#DetailSPlanFinishDateTimeSelector").get(0).value = smbScript.getDateTimelocalDate(newEndDateTime);
                            $("#DetailSPlanFinishLabel").text(smbScript.ConvertDateOrTime(newEndDateTime, $("#hidShowDate").val()));

                            //チップ詳細(大)に反映
                            $("#DetailLPlanFinishDateTimeSelector").get(0).value = smbScript.getDateTimelocalDate(newEndDateTime);
                            $("#DetailLPlanFinishLabel").text($("#DetailSPlanFinishLabel").text());
                        }
                        $("#DetailSWorkTimeTxt").val(timeSpan);
                        $("#DetailLWorkTimeTxt").val(timeSpan);
                        $("#DetailSWorkTimeLabel").text(timeSpan + $("#WordWorkTimeUnitHidden").val());
                        $("#DetailLWorkTimeLabel").text(timeSpan + $("#WordWorkTimeUnitHidden").val());
                    }
                }
            }
            else {
                //ラベルをEmptyにする
                $("#DetailSPlanStartLabel").text("");

                //チップ詳細(大)に反映
                $("#DetailLPlanStartDateTimeSelector").get(0).value = null;
                $("#DetailLPlanStartLabel").text("");
            }

            //必須項目がEmptyなら登録ボタンを非活性にする
            $("#DetailRegisterBtn").attr("disabled", IsMandatoryChipDetailEmpty());
        });
    // 2020/01/16 NSK 夏目  TR-SVT-TMT-20200110-002 追加jobでサービスの種類を選択できない end

    // 2020/01/16 NSK 夏目  TR-SVT-TMT-20200110-002 追加jobでサービスの種類を選択できない start
    ////作業終了予定日時
    //$("#DetailSPlanFinishDateTimeSelector")
    //    .blur(function () {

    //        //作業終了予定日時がnullでない場合
    //        if ($(this).get(0).value != null && $(this).get(0).value != "") {

    //            //作業終了予定日時を5分単位で丸め込んだ日時を計算し、DateTimSelectorに設定しなおす
    //            var wkDate = new Date(smbScript.RoundUpTimeTo5Units(smbScript.changeStringToDateIcrop($(this).get(0).value)));

    //            //営業時間外を選択している場合は、翌営業開始時刻に補正する
    //            var newDate = smbScript.GetDateExcludeOutOfTime(wkDate);

    //            $(this).get(0).value = smbScript.getDateTimelocalDate(newDate);

    //            //DateTimeSelectorで選択した日時を表示用に変換してラベルに設定
    //            $("#DetailSPlanFinishLabel").text(smbScript.ConvertDateOrTime(newDate, $("#hidShowDate").val()));

    //            //チップ詳細(大)に反映
    //            $("#DetailLPlanFinishDateTimeSelector").get(0).value = smbScript.getDateTimelocalDate(newDate);
    //            $("#DetailLPlanFinishLabel").text($("#DetailSPlanFinishLabel").text());

    //            //作業開始予定日時がnullでない場合
    //            if ($("#DetailSPlanStartDateTimeSelector").get(0).value != null && $("#DetailSPlanStartDateTimeSelector").get(0).value != "") {

    //                //開始日時と終了日時から差分時間を計算し、作業時間に設定する　※営業時間外の時間帯は減算して計算
    //                var timeSpan = smbScript.CalcTimeSpan_ExcludeOutOfTime(smbScript.changeStringToDateIcrop($("#DetailSPlanStartDateTimeSelector").get(0).value), smbScript.changeStringToDateIcrop($(this).get(0).value));

    //                if (timeSpan != null) {
    //                    //最大値を超える場合、作業時間最大値（分）をセット
    //                    if (timeSpan > C_SC3240201_MAXWORKTIME) {
    //                        timeSpan = C_SC3240201_MAXWORKTIME;

    //                        //開始日時と加算時間を元に、終了日時を計算する（営業時間外の時間帯は除外して計算）
    //                        var newEndDateTime = smbScript.CalcEndDate_ExcludeOutOfTime(smbScript.changeStringToDateIcrop($("#DetailSPlanStartDateTimeSelector").get(0).value), C_SC3240201_MAXWORKTIME);
    //                        $("#DetailSPlanFinishDateTimeSelector").get(0).value = smbScript.getDateTimelocalDate(newEndDateTime);
    //                        $("#DetailSPlanFinishLabel").text(smbScript.ConvertDateOrTime(newEndDateTime, $("#hidShowDate").val()));

    //                        //チップ詳細(大)に反映
    //                        $("#DetailLPlanFinishDateTimeSelector").get(0).value = smbScript.getDateTimelocalDate(newEndDateTime);
    //                        $("#DetailLPlanFinishLabel").text($("#DetailSPlanFinishLabel").text());
    //                    }
    //                    $("#DetailSWorkTimeTxt").val(timeSpan);
    //                    $("#DetailLWorkTimeTxt").val(timeSpan);
    //                    $("#DetailSWorkTimeLabel").text(timeSpan + $("#WordWorkTimeUnitHidden").val());
    //                    $("#DetailLWorkTimeLabel").text(timeSpan + $("#WordWorkTimeUnitHidden").val());
    //                }
    //            }
    //        }
    //        else {
    //            //ラベルをEmptyにする
    //            $("#DetailSPlanFinishLabel").text("");

    //            //チップ詳細(大)に反映
    //            $("#DetailLPlanFinishDateTimeSelector").get(0).value = null;
    //            $("#DetailLPlanFinishLabel").text("");
    //        }

    //        //必須項目がEmptyなら登録ボタンを非活性にする
    //        $("#DetailRegisterBtn").attr("disabled", IsMandatoryChipDetailEmpty());
    //    });
    //作業終了予定日時
    $("#DetailSPlanFinishDateTimeSelector")
        .change(function () {

            //作業終了予定日時がnullでない場合
            if ($(this).get(0).value != null && $(this).get(0).value != "") {

                //作業終了予定日時を5分単位で丸め込んだ日時を計算し、DateTimSelectorに設定しなおす
                var wkDate = new Date(smbScript.RoundUpTimeTo5Units(smbScript.changeStringToDateIcrop($(this).get(0).value)));

                //営業時間外を選択している場合は、翌営業開始時刻に補正する
                var newDate = smbScript.GetDateExcludeOutOfTime(wkDate);

                $(this).get(0).value = smbScript.getDateTimelocalDate(newDate);

                //DateTimeSelectorで選択した日時を表示用に変換してラベルに設定
                $("#DetailSPlanFinishLabel").text(smbScript.ConvertDateOrTime(newDate, $("#hidShowDate").val()));

                //チップ詳細(大)に反映
                $("#DetailLPlanFinishDateTimeSelector").get(0).value = smbScript.getDateTimelocalDate(newDate);
                $("#DetailLPlanFinishLabel").text($("#DetailSPlanFinishLabel").text());

                //作業開始予定日時がnullでない場合
                if ($("#DetailSPlanStartDateTimeSelector").get(0).value != null && $("#DetailSPlanStartDateTimeSelector").get(0).value != "") {

                    //開始日時と終了日時から差分時間を計算し、作業時間に設定する　※営業時間外の時間帯は減算して計算
                    var timeSpan = smbScript.CalcTimeSpan_ExcludeOutOfTime(smbScript.changeStringToDateIcrop($("#DetailSPlanStartDateTimeSelector").get(0).value), smbScript.changeStringToDateIcrop($(this).get(0).value));

                    if (timeSpan != null) {
                        //最大値を超える場合、作業時間最大値（分）をセット
                        if (timeSpan > C_SC3240201_MAXWORKTIME) {
                            timeSpan = C_SC3240201_MAXWORKTIME;

                            //開始日時と加算時間を元に、終了日時を計算する（営業時間外の時間帯は除外して計算）
                            var newEndDateTime = smbScript.CalcEndDate_ExcludeOutOfTime(smbScript.changeStringToDateIcrop($("#DetailSPlanStartDateTimeSelector").get(0).value), C_SC3240201_MAXWORKTIME);
                            $("#DetailSPlanFinishDateTimeSelector").get(0).value = smbScript.getDateTimelocalDate(newEndDateTime);
                            $("#DetailSPlanFinishLabel").text(smbScript.ConvertDateOrTime(newEndDateTime, $("#hidShowDate").val()));

                            //チップ詳細(大)に反映
                            $("#DetailLPlanFinishDateTimeSelector").get(0).value = smbScript.getDateTimelocalDate(newEndDateTime);
                            $("#DetailLPlanFinishLabel").text($("#DetailSPlanFinishLabel").text());
                        }
                        $("#DetailSWorkTimeTxt").val(timeSpan);
                        $("#DetailLWorkTimeTxt").val(timeSpan);
                        $("#DetailSWorkTimeLabel").text(timeSpan + $("#WordWorkTimeUnitHidden").val());
                        $("#DetailLWorkTimeLabel").text(timeSpan + $("#WordWorkTimeUnitHidden").val());
                    }
                }
            }
            else {
                //ラベルをEmptyにする
                $("#DetailSPlanFinishLabel").text("");

                //チップ詳細(大)に反映
                $("#DetailLPlanFinishDateTimeSelector").get(0).value = null;
                $("#DetailLPlanFinishLabel").text("");
            }

            //必須項目がEmptyなら登録ボタンを非活性にする
            $("#DetailRegisterBtn").attr("disabled", IsMandatoryChipDetailEmpty());
        });
    // 2020/01/16 NSK 夏目  TR-SVT-TMT-20200110-002 追加jobでサービスの種類を選択できない end

    // 2020/01/16 NSK 夏目  TR-SVT-TMT-20200110-002 追加jobでサービスの種類を選択できない start
    ////納車予定日時
    //$("#DetailSPlanDeriveredDateTimeSelector")
    //    .blur(function () {
    //        //DateTimeSelectorで選択した日時を表示用に変換してラベルに設定
    //        $("#DetailSPlanDeriveredLabel").text(smbScript.ConvertDateOrTime(smbScript.changeStringToDateIcrop($(this).get(0).value), $("#hidShowDate").val()));
    //        //チップ詳細(大)に反映
    //        $("#DetailLPlanDeriveredDateTimeSelector").get(0).value = $(this).get(0).value;
    //        $("#DetailLPlanDeriveredLabel").text($("#DetailSPlanDeriveredLabel").text());
    //    });
    //納車予定日時
    $("#DetailSPlanDeriveredDateTimeSelector")
        .change(function () {
            //DateTimeSelectorで選択した日時を表示用に変換してラベルに設定
            $("#DetailSPlanDeriveredLabel").text(smbScript.ConvertDateOrTime(smbScript.changeStringToDateIcrop($(this).get(0).value), $("#hidShowDate").val()));
            //チップ詳細(大)に反映
            $("#DetailLPlanDeriveredDateTimeSelector").get(0).value = $(this).get(0).value;
            $("#DetailLPlanDeriveredLabel").text($("#DetailSPlanDeriveredLabel").text());
        });
    // 2020/01/16 NSK 夏目  TR-SVT-TMT-20200110-002 追加jobでサービスの種類を選択できない end

    //作業開始実績時間
    $("#DetailSProcessStartDateTimeSelector")
        .blur(function () {
            //DateTimeSelectorで選択した日時を表示用に変換してラベルに設定
            $("#DetailSProcessStartLabel").text(smbScript.ConvertDateOrTime(smbScript.changeStringToDateIcrop($(this).get(0).value), $("#hidShowDate").val()));
            //チップ詳細(大)に反映
            $("#DetailLProcessStartDateTimeSelector").get(0).value = $(this).get(0).value;
            $("#DetailLProcessStartLabel").text($("#DetailSProcessStartLabel").text());
        });

    //作業終了実績時間
    $("#DetailSProcessFinishDateTimeSelector")
        .blur(function () {
            $("#DetailSProcessFinishLabel").text(smbScript.ConvertDateOrTime(smbScript.changeStringToDateIcrop($(this).get(0).value), $("#hidShowDate").val()));
            //チップ詳細(大)に反映
            $("#DetailLProcessFinishDateTimeSelector").get(0).value = $(this).get(0).value;
            $("#DetailLProcessFinishLabel").text($("#DetailSProcessFinishLabel").text());
        });
    //2013/12/26 TMEJ 下村 次世代e-CRBタブレット(サービス) iOS7.0 VersionUp対応 END

    // 2020/01/16 NSK 夏目  TR-SVT-TMT-20200110-002 追加jobでサービスの種類を選択できない start
    ////整備種類
    //$("#DetailSMaintenanceTypeList")
    //    .blur(function () {
    //        var e = document.getElementById("DetailSMaintenanceTypeList");
    //        $("#DetailSMaintenanceTypeLabel").text(e.options[e.selectedIndex].text);
    //        //チップ詳細(大)に反映
    //        $("#DetailLMaintenanceTypeList").val(e.options[e.selectedIndex].value);
    //        $("#DetailLMaintenanceTypeLabel").text(e.options[e.selectedIndex].text);
    //    });
    //整備種類
    $("#DetailSMaintenanceTypeList")
        .change(function () {
            var e = document.getElementById("DetailSMaintenanceTypeList");
            $("#DetailSMaintenanceTypeLabel").text(e.options[e.selectedIndex].text);
            //チップ詳細(大)に反映
            $("#DetailLMaintenanceTypeList").val(e.options[e.selectedIndex].value);
            $("#DetailLMaintenanceTypeLabel").text(e.options[e.selectedIndex].text);
        });
    // 2020/01/16 NSK 夏目  TR-SVT-TMT-20200110-002 追加jobでサービスの種類を選択できない end

    //整備種類コンボボックスのフォーカスInイベント
    $("#DetailSMaintenanceTypeList").bind('focusin', DetailSLSvcClassIDFocusIn);

    //整備種類コンボボックスのフォーカスOutイベント
    // 2020/01/16 NSK 夏目  TR-SVT-TMT-20200110-002 追加jobでサービスの種類を選択できない start
    //$("#DetailSMaintenanceTypeList").bind('focusout', DetailSLSvcClassIDFocusOut);
    $("#DetailSMaintenanceTypeList").bind('change', DetailSLSvcClassIDFocusOut);
    // 2020/01/16 NSK 夏目  TR-SVT-TMT-20200110-002 追加jobでサービスの種類を選択できない end    

    // 2020/01/16 NSK 夏目  TR-SVT-TMT-20200110-002 追加jobでサービスの種類を選択できない start
    ////整備名
    //$("#DetailSMercList")
    //    .blur(function () {
    //        var e = document.getElementById("DetailSMercList");
    //        $("#DetailSMercLabel").text(e.options[e.selectedIndex].text);
    //        //チップ詳細(大)に反映
    //        $("#DetailLMercList").val(e.options[e.selectedIndex].value);
    //        $("#DetailLMercLabel").text(e.options[e.selectedIndex].text);
    //    });
    //整備名
    $("#DetailSMercList")
        .change(function () {
            var e = document.getElementById("DetailSMercList");
            $("#DetailSMercLabel").text(e.options[e.selectedIndex].text);
            //チップ詳細(大)に反映
            $("#DetailLMercList").val(e.options[e.selectedIndex].value);
            $("#DetailLMercLabel").text(e.options[e.selectedIndex].text);
        });
    // 2020/01/16 NSK 夏目  TR-SVT-TMT-20200110-002 追加jobでサービスの種類を選択できない end

    //整備名コンボボックスのフォーカスInイベント
    $("#DetailSMercList").bind('focusin', DetailSLMercIDFocusIn);

    //整備名コンボボックスのフォーカスOutイベント
    // 2020/01/16 NSK 夏目  TR-SVT-TMT-20200110-002 追加jobでサービスの種類を選択できない start
    //$("#DetailSMercList").bind('focusout', DetailSLMercIDFocusOut);
    $("#DetailSMercList").bind('change', DetailSLMercIDFocusOut);
    // 2020/01/16 NSK 夏目  TR-SVT-TMT-20200110-002 追加jobでサービスの種類を選択できない end

    //作業時間
    $("#DetailSWorkTimeTxt")
        .click(function () {
            //編集可能な場合のみ、ラベルとテキストを切り替える
            if ($(this).hasClass(C_SC3240201CLASS_TEXTBLACK) == false) {
                $("#DetailSWorkTimeTxt").css({
                    "opacity": "1"
                });
                $("#DetailSWorkTimeLabel").css({
                    "opacity": "0"
                });
            }
        })
        .blur(function () {
            //編集可能な場合のみ、ラベルとテキストを切り替える
            if ($(this).hasClass(C_SC3240201CLASS_TEXTBLACK) == false) {
                $("#DetailSWorkTimeTxt").css({
                    "opacity": "0"
                });
                $("#DetailSWorkTimeLabel").css({
                    "opacity": "1"
                });
            }
        })
        .change(function (e) {
            var min;

            //作業時間を空白にした場合、5を設定する
            if ($("#DetailSWorkTimeTxt").val() == "") {
                min = gResizeInterval;
            }
            //それ以外は5分単位で丸め込む
            else {
                min = smbScript.RoundUpToNumUnits($("#DetailSWorkTimeTxt").val(), gResizeInterval, gResizeInterval, C_SC3240201_MAXWORKTIME);
            }

            $("#DetailSWorkTimeTxt").val(min);
            $("#DetailLWorkTimeTxt").val(min);
            $("#DetailSWorkTimeLabel").text(min + $("#WordWorkTimeUnitHidden").val());
            $("#DetailLWorkTimeLabel").text(min + $("#WordWorkTimeUnitHidden").val());

            //作業開始日時がnullでない場合、設定した作業時間に合わせて作業終了予定時間を変更する
            //2013/12/26 TMEJ 下村 次世代e-CRBタブレット(サービス) iOS7.0 VersionUp対応 START
            //var startDateTime = $("#DetailSPlanStartDateTimeSelector").get(0).valueAsDate;
            var startDateTime = smbScript.changeStringToDateIcrop($("#DetailSPlanStartDateTimeSelector").get(0).value);
            //2013/12/26 TMEJ 下村 次世代e-CRBタブレット(サービス) iOS7.0 VersionUp対応 END

            //2014/04/01 TMEJ 丁 次世代e-CRBタブレット(サービス) チーフテクニシャン機能開発 START
            //if (startDateTime != null){
            if ((startDateTime != null) && (!$("#DetailSPlanFinishDateTimeSelector")[0].disabled)) {
            //2014/04/01 TMEJ 丁 次世代e-CRBタブレット(サービス) チーフテクニシャン機能開発 END

                //var newEndDateTime = smbScript.CalcEndDateTime(startDateTime, min);
                //開始日時と加算時間を元に、終了日時を計算する（営業時間外の時間帯は除外して計算）
                var newEndDateTime = smbScript.CalcEndDate_ExcludeOutOfTime(startDateTime, min);

                //2013/12/26 TMEJ 下村 次世代e-CRBタブレット(サービス) iOS7.0 VersionUp対応 START
                //$("#DetailSPlanFinishDateTimeSelector").get(0).valueAsDate = newEndDateTime;
                $("#DetailSPlanFinishDateTimeSelector").get(0).value = smbScript.getDateTimelocalDate(newEndDateTime);
                //2013/12/26 TMEJ 下村 次世代e-CRBタブレット(サービス) iOS7.0 VersionUp対応 END

                $("#DetailSPlanFinishLabel").text(smbScript.ConvertDateOrTime(newEndDateTime, $("#hidShowDate").val()));

                //チップ詳細(大)に反映
                //2013/12/26 TMEJ 下村 次世代e-CRBタブレット(サービス) iOS7.0 VersionUp対応 START
                //$("#DetailLPlanFinishDateTimeSelector").get(0).valueAsDate = newEndDateTime;
                $("#DetailLPlanFinishDateTimeSelector").get(0).value = smbScript.getDateTimelocalDate(newEndDateTime);
                //2013/12/26 TMEJ 下村 次世代e-CRBタブレット(サービス) iOS7.0 VersionUp対応 END

                $("#DetailLPlanFinishLabel").text($("#DetailSPlanFinishLabel").text());

                //必須項目がEmptyなら登録ボタンを非活性にする
                $("#DetailRegisterBtn").attr("disabled", IsMandatoryChipDetailEmpty());
            }

        })
        .CustomTextBox({
            clear: function (e) {    //CustomTextBoxの×ボタンタップイベント
                $("#DetailSWorkTimeTxt").val(gResizeInterval);
                $("#DetailLWorkTimeTxt").val(gResizeInterval);
                $("#DetailSWorkTimeLabel").text(gResizeInterval + $("#WordWorkTimeUnitHidden").val());
                $("#DetailLWorkTimeLabel").text(gResizeInterval + $("#WordWorkTimeUnitHidden").val());

                //作業開始日時がnullでない場合、設定した作業時間に合わせて作業終了予定時間を変更する
                //2013/12/26 TMEJ 下村 次世代e-CRBタブレット(サービス) iOS7.0 VersionUp対応 START
                //var startDateTime = $("#DetailSPlanStartDateTimeSelector").get(0).valueAsDate;
                var startDateTime = smbScript.changeStringToDateIcrop($("#DetailSPlanStartDateTimeSelector").get(0).value);
                //2013/12/26 TMEJ 下村 次世代e-CRBタブレット(サービス) iOS7.0 VersionUp対応 END

                //2013/12/26 TMEJ 下村 次世代e-CRBタブレット(サービス) iOS7.0 VersionUp対応 START

                //2014/04/01 TMEJ 丁 次世代e-CRBタブレット(サービス) チーフテクニシャン機能開発 START
                //if (startDateTime != null){
                if ((startDateTime != null) && (!$("#DetailSPlanFinishDateTimeSelector")[0].disabled)) {
                //2014/04/01 TMEJ 丁 次世代e-CRBタブレット(サービス) チーフテクニシャン機能開発 END
                    var newEndDateTime = smbScript.CalcEndDateTime(startDateTime, gResizeInterval);

                    //2013/12/26 TMEJ 下村 次世代e-CRBタブレット(サービス) iOS7.0 VersionUp対応 START
                    //$("#DetailSPlanFinishDateTimeSelector").get(0).valueAsDate = newEndDateTime;
                    $("#DetailSPlanFinishDateTimeSelector").get(0).value = smbScript.getDateTimelocalDate(newEndDateTime);
                    //2013/12/26 TMEJ 下村 次世代e-CRBタブレット(サービス) iOS7.0 VersionUp対応 END

                    $("#DetailSPlanFinishLabel").text(smbScript.ConvertDateOrTime(newEndDateTime, $("#hidShowDate").val()));

                    //チップ詳細(大)に反映
                    //2013/12/26 TMEJ 下村 次世代e-CRBタブレット(サービス) iOS7.0 VersionUp対応 START
                    //$("#DetailLPlanFinishDateTimeSelector").get(0).valueAsDate = newEndDateTime;
                    $("#DetailLPlanFinishDateTimeSelector").get(0).value = smbScript.getDateTimelocalDate(newEndDateTime);
                    //2013/12/26 TMEJ 下村 次世代e-CRBタブレット(サービス) iOS7.0 VersionUp対応 END

                    $("#DetailLPlanFinishLabel").text($("#DetailSPlanFinishLabel").text());

                    //必須項目がEmptyなら登録ボタンを非活性にする
                    $("#DetailRegisterBtn").attr("disabled", IsMandatoryChipDetailEmpty());
                }
            }
        });

    //ご用命
    $("#DetailSOrderTxt")
        .click(function () {
            //チップ詳細(小)のコントロールにフォーカスが当たらない不具合を解消する
            $(this).focus();
        })
        .blur(function () {
            $("#DetailLOrderTxt").val($("#DetailSOrderTxt").val());
            ControlLengthTextarea($("#DetailSOrderTxt"));
            AdjusterDetailTextArea($("#DetailSOrderTxt"), $("#DetailSOrderDt"));
            //スクロール位置ずれ調整
            //AdjustChipDetailDisplay();
        	//2013/12/04 TMEJ 下村 タブレット版SMB チーフテクニシャン機能開発 START
            //$("#DetailSDummyBtn").focus();
            //$("#DetailSDummyBtn").blur();
		    $("#ChipDetailSContent").animate({
		        scrollTop: 0,
		        scrollLeft: 0
		    }, 'normal');        
        	//2013/12/04 TMEJ 下村 タブレット版SMB チーフテクニシャン機能開発 END
        })
        .bind("paste", function (e) {
            setTimeout(function () {
                ControlLengthTextarea($("#DetailSOrderTxt"));
                AdjusterDetailTextArea($("#DetailSOrderTxt"), $("#DetailSOrderDt"));
            }, 0);
        })
        .bind("keyup", function () {
            ControlLengthTextarea($("#DetailSOrderTxt"));
            AdjusterDetailTextArea($("#DetailSOrderTxt"), $("#DetailSOrderDt"));
        })
        .bind("keydown", function () {
            ControlLengthTextarea($("#DetailSOrderTxt"));
        });

//2013/12/04 TMEJ 下村 タブレット版SMB チーフテクニシャン機能開発 START
//    //故障原因
//    $("#DetailSFailureTxt")
//        .click(function () {
//            //チップ詳細(小)のコントロールにフォーカスが当たらない不具合を解消する
//            $(this).focus();
//        })
//        .blur(function () {
//            $("#DetailLFailureTxt").val($("#DetailSFailureTxt").val());            
//            ControlLengthTextarea($("#DetailSFailureTxt"));
//            AdjusterDetailTextArea($("#DetailSFailureTxt"), $("#DetailSFailureDt"));
//            //スクロール位置ずれ調整
//            //AdjustChipDetailDisplay();
//            $("#DetailSDummyBtn").focus();
//        })
//        .bind("paste", function (e) {
//            setTimeout(function () {
//                ControlLengthTextarea($("#DetailSFailureTxt"));
//                AdjusterDetailTextArea($("#DetailSFailureTxt"), $("#DetailSFailureDt"));
//            }, 0);
//        })
//        .bind("keyup", function () {
//            ControlLengthTextarea($("#DetailSFailureTxt"));
//            AdjusterDetailTextArea($("#DetailSFailureTxt"), $("#DetailSFailureDt"));
//        })
//        .bind("keydown", function () {
//            ControlLengthTextarea($("#DetailSFailureTxt"));
//        });

//    //診断結果
//    $("#DetailSResultTxt")
//        .click(function () {
//            //チップ詳細(小)のコントロールにフォーカスが当たらない不具合を解消する
//            $(this).focus();
//        })
//        .blur(function () {
//            $("#DetailLResultTxt").val($("#DetailSResultTxt").val());            
//            ControlLengthTextarea($("#DetailSResultTxt"));
//            AdjusterDetailTextArea($("#DetailSResultTxt"), $("#DetailSResultDt"));
//            //スクロール位置ずれ調整
//            //AdjustChipDetailDisplay();
//            $("#DetailSDummyBtn").focus();
//        })
//        .bind("paste", function (e) {
//            setTimeout(function () {
//                ControlLengthTextarea($("#DetailSResultTxt"));
//                AdjusterDetailTextArea($("#DetailSResultTxt"), $("#DetailSResultDt"));
//            }, 0);
//        })
//        .bind("keyup", function () {
//            ControlLengthTextarea($("#DetailSResultTxt"));
//            AdjusterDetailTextArea($("#DetailSResultTxt"), $("#DetailSResultDt"));
//        })
//        .bind("keydown", function () {
//            ControlLengthTextarea($("#DetailSResultTxt"));
//        });

//    //アドバイス
//    $("#DetailSAdviceTxt")
//        .click(function () {
//            //チップ詳細(小)のコントロールにフォーカスが当たらない不具合を解消する
//            $(this).focus();
//        })
//        .blur(function () {
//            $("#DetailLAdviceTxt").val($("#DetailSAdviceTxt").val());            
//            ControlLengthTextarea($("#DetailSAdviceTxt"));
//            AdjusterDetailTextArea($("#DetailSAdviceTxt"), $("#DetailSAdviceDt"));
//            //スクロール位置ずれ調整
//            //AdjustChipDetailDisplay();
//            $("#DetailSDummyBtn").focus();
//        })
//        .bind("paste", function (e) {
//            setTimeout(function () {
//                ControlLengthTextarea($("#DetailSAdviceTxt"));
//                AdjusterDetailTextArea($("#DetailSAdviceTxt"), $("#DetailSAdviceDt"));
//            }, 0);
//        })
//        .bind("keyup", function () {
//            ControlLengthTextarea($("#DetailSAdviceTxt"));
//            AdjusterDetailTextArea($("#DetailSAdviceTxt"), $("#DetailSAdviceDt"));
//        })
//        .bind("keydown", function () {
//            ControlLengthTextarea($("#DetailSAdviceTxt"));
//                });

        //メモ
        $("#DetailSMemoTxt")
        .click(function () {
            //チップ詳細(小)のコントロールにフォーカスが当たらない不具合を解消する
            $(this).focus();
        })
        .blur(function () {
            $("#DetailLMemoTxt").val($("#DetailSMemoTxt").val());
            ControlLengthTextarea($("#DetailSMemoTxt"));
            AdjusterDetailTextArea($("#DetailSMemoTxt"), $("#DetailSMemoDt"));
            //スクロール位置ずれ調整
            //AdjustChipDetailDisplay();
        })
        .bind("paste", function (e) {
            setTimeout(function () {
                ControlLengthTextarea($("#DetailSMemoTxt"));
                AdjusterDetailTextArea($("#DetailSMemoTxt"), $("#DetailSMemoDt"));
            }, 0);
        })
        .bind("keyup", function () {
            ControlLengthTextarea($("#DetailSMemoTxt"));
            AdjusterDetailTextArea($("#DetailSMemoTxt"), $("#DetailSMemoDt"));
        })
        .bind("keydown", function () {
            ControlLengthTextarea($("#DetailSMemoTxt"));
        });
//2013/12/04 TMEJ 下村 タブレット版SMB チーフテクニシャン機能開発 END

} //SetDetailSTextEvent End


//整備種類コンボボックスのフォーカスInイベント
function DetailSLSvcClassIDFocusIn() {

    //フォーカスINされた時の値をグローバル変数に格納
    gSC3240201BeforeSvcClassID = $(this).val();
};

//整備種類コンボボックスのフォーカスOutイベント
function DetailSLSvcClassIDFocusOut() {    

    //整備種類コンボボックスにフォーカスINされた時の値をグローバル変数から取得
    var beforeValue = gSC3240201BeforeSvcClassID;

    //次のイベント用に初期化
    gSC3240201BeforeSvcClassID = null;

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

        //チップ詳細(小)　商品コンボボックスを初期化
        var e = document.getElementById("DetailSMercList");
        e.options.length = 0; //コンボボックス内のデータをクリア
        $("#DetailSMercLabel").text("");

        //チップ詳細(大)　商品コンボボックスを初期化
        var f = document.getElementById("DetailLMercList");
        f.options.length = 0; //コンボボックス内のデータをクリア
        $("#DetailLMercLabel").text("");

        //商品コンボボックスを非活性にする
        $("#DetailSMercList").attr("disabled", true);
        $("#DetailLMercList").attr("disabled", true);

        // 2015/06/15 TMEJ 河原  サービス入庫予約情報のバリデーション実行タイミング変更に向けた評価用アプリ作成 STRAT

        //商品データなし("0"：商品データ無し)
        $("#DetailSMercList").attr("MERCITEM", 0);

        // 2015/06/15 TMEJ 河原  サービス入庫予約情報のバリデーション実行タイミング変更に向けた評価用アプリ作成 END

        return false;
    } 
    else {
        //変更された

        //登録ボタンを非活性にしておく
        $("#DetailRegisterBtn").attr("disabled", true);

        //商品情報を取得する（コンボボックス内の値をDBから取得）

        //アクティブインジケータ表示
        gDetailSActiveIndicator.show();

        //オーバーレイ表示
        gDetailOverlay.show();
    
        //リフレッシュタイマーセット
        commonRefreshTimer(ReDisplayChipDetail);

        //「サービス分類ID,標準作業時間」の文字列を分解
        var svcClassInfo = afterValue.split(",");

        //サーバーに渡すパラメータを作成
        var prms = CreateCallBackGetMercParam(C_SC3240201CALLBACK_GETMERC, svcClassInfo[0]);

        //標準作業時間の連動処理
        SetDetailSL_StandardWorkTime(svcClassInfo[1]);

        //2013/12/04 TMEJ 下村 タブレット版SMB チーフテクニシャン機能開発 START
        //完成検査有無の連動処理
        SetDetailSL_CompleteExaminationArea(svcClassInfo[2]);

        // 2020/01/08 NSK 鈴木 TR-V4-BMS-20191001-001 (トライ店システム評価)コールセンター業務における洗車要否判断の効率化検証 START
        // //洗車有無の連動処理
        //SetDetailSL_CarWashArea(svcClassInfo[3]);

        if (canChangeCarwashNeedFlg()) {
            //洗車有無の連動処理
            SetDetailSL_CarWashArea(svcClassInfo[3]);
        }
        // 2020/01/08 NSK 鈴木 TR-V4-BMS-20191001-001 (トライ店システム評価)コールセンター業務における洗車要否判断の効率化検証 END

        //2013/12/04 TMEJ 下村 タブレット版SMB チーフテクニシャン機能開発 END

        //コールバック開始
        DoCallBack(C_CALLBACK_WND201, prms, SC3240201AfterCallBack, "GetMercList");
        return false;
    };
};

//整備名コンボボックスのフォーカスInイベント
function DetailSLMercIDFocusIn() {

    //フォーカスINされた時の値をグローバル変数に格納
    gSC3240201BeforeMercID = $(this).val();
};

//整備名コンボボックスのフォーカスOutイベント
function DetailSLMercIDFocusOut() {    

    //整備名コンボボックスにフォーカスINされた時の値をグローバル変数から取得
    var beforeValue = gSC3240201BeforeMercID;

    //次のイベント用に初期化
    gSC3240201BeforeMercID = null;

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
        SetDetailSL_StandardWorkTime(mercInfo[1]);

        return false;
    };
};


/**	
* チップ詳細(大)のテキストイベントの設定を行う
* 	
*/
function SetDetailLTextEvent() {

    //2013/12/26 TMEJ 下村 次世代e-CRBタブレット(サービス) iOS7.0 VersionUp対応 START
    ////来店予定時間
    //$("#DetailLPlanVisitDateTimeSelector")
    //    .click(function () {
    //        //チップ詳細(大)のコントロールにフォーカスが当たらない不具合を解消する
    //        $(this).focus();
    //    })
    //    .blur(function () {
    //        $("#DetailLPlanVisitLabel").text(smbScript.ConvertDateOrTime($(this).get(0).valueAsDate, $("#hidShowDate").val()));
    //        
    //        //チップ詳細(小)に反映
    //        $("#DetailSPlanVisitDateTimeSelector").get(0).valueAsDate = $(this).get(0).valueAsDate;
    //        $("#DetailSPlanVisitLabel").text($("#DetailLPlanVisitLabel").text());
    //    });
    //
    ////作業開始予定時間
    //$("#DetailLPlanStartDateTimeSelector")
    //    .click(function () {
    //        //チップ詳細(大)のコントロールにフォーカスが当たらない不具合を解消する
    //        $(this).focus();
    //    })
    //    .blur(function () {
    //        //作業開始予定日時がnullでない場合
    //        if ($(this).get(0).valueAsDate != null) {
    //            //作業開始予定日時を5分単位で丸め込んだ日時を計算し、DateTimSelectorに設定しなおす
    //            var wkDate = new Date(smbScript.RoundUpTimeTo5Units($(this).get(0).valueAsDate));
    //
    //            ////営業時間外を選択している場合は、指定した日の営業開始時刻に補正する
    //            //var newDate = smbScript.GetStartDateExcludeOutOfTime(wkDate);
    //            //営業時間外を選択している場合は、翌営業開始時刻に補正する
    //            var newDate = smbScript.GetDateExcludeOutOfTime(wkDate);
    //
    //            $(this).get(0).valueAsDate = newDate;
    //            //DateTimeSelectorで選択した日時を表示用に変換してラベルに設定
    //            $("#DetailLPlanStartLabel").text(smbScript.ConvertDateOrTime(newDate, $("#hidShowDate").val()));
    //
    //            //チップ詳細(小)に反映
    //            $("#DetailSPlanStartDateTimeSelector").get(0).valueAsDate = newDate;
    //            $("#DetailSPlanStartLabel").text($("#DetailLPlanStartLabel").text());
    //
    //            //作業終了予定日時がnullでない場合
    //            if ($("#DetailLPlanFinishDateTimeSelector").get(0).valueAsDate != null) {
    //                //開始日時と終了日時から差分時間を計算し、作業時間に設定する　※営業時間外の時間帯は減算して計算
    //                //var timeSpan = smbScript.CalcTimeSpan($(this).get(0).valueAsDate, $("#DetailLPlanFinishDateTimeSelector").get(0).valueAsDate);
    //                var timeSpan = smbScript.CalcTimeSpan_ExcludeOutOfTime($(this).get(0).valueAsDate, $("#DetailLPlanFinishDateTimeSelector").get(0).valueAsDate);
    //                if (timeSpan != null) {
    //                    //最大値を超える場合、作業時間最大値（分）をセット
    //                    if (timeSpan > C_SC3240201_MAXWORKTIME) {
    //                        timeSpan = C_SC3240201_MAXWORKTIME;
    //
    //                        //開始日時と加算時間を元に、終了日時を計算する（営業時間外の時間帯は除外して計算）
    //                        var newEndDateTime = smbScript.CalcEndDate_ExcludeOutOfTime($("#DetailLPlanStartDateTimeSelector").get(0).valueAsDate, C_SC3240201_MAXWORKTIME);
    //
    //                        $("#DetailLPlanFinishDateTimeSelector").get(0).valueAsDate = newEndDateTime;
    //                        $("#DetailLPlanFinishLabel").text(smbScript.ConvertDateOrTime(newEndDateTime, $("#hidShowDate").val()));
    //
    //                        //チップ詳細(小)に反映
    //                        $("#DetailSPlanFinishDateTimeSelector").get(0).valueAsDate = newEndDateTime;
    //                        $("#DetailSPlanFinishLabel").text($("#DetailLPlanFinishLabel").text());
    //                    }
    //                    $("#DetailSWorkTimeTxt").val(timeSpan);
    //                    $("#DetailLWorkTimeTxt").val(timeSpan);
    //                    $("#DetailSWorkTimeLabel").text(timeSpan + $("#WordWorkTimeUnitHidden").val());
    //                    $("#DetailLWorkTimeLabel").text(timeSpan + $("#WordWorkTimeUnitHidden").val());
    //                }
    //            }
    //        }
    //        else {
    //            //ラベルをEmptyにする
    //            $("#DetailLPlanStartLabel").text("");
    //
    //            //チップ詳細(小)に反映
    //            $("#DetailSPlanStartDateTimeSelector").get(0).valueAsDate = null;
    //            $("#DetailSPlanStartLabel").text("");
    //        }
    //
    //        //必須項目がEmptyなら登録ボタンを非活性にする
    //        $("#DetailRegisterBtn").attr("disabled", IsMandatoryChipDetailEmpty());
    //    });
    //
    ////作業終了予定時間
    //$("#DetailLPlanFinishDateTimeSelector")
    //    .click(function () {
    //        //チップ詳細(大)のコントロールにフォーカスが当たらない不具合を解消する
    //        $(this).focus();
    //    })
    //    .blur(function () {
    //        //作業終了予定日時がnullでない場合
    //        if ($(this).get(0).valueAsDate != null) {
    //            //作業終了予定日時を5分単位で丸め込んだ日時を計算し、DateTimSelectorに設定しなおす
    //            var wkDate = new Date(smbScript.RoundUpTimeTo5Units($(this).get(0).valueAsDate));
    //
    //            ////営業時間外を選択している場合は、指定した日の営業終了時刻に補正する
    //            //var newDate = smbScript.GetEndDateExcludeOutOfTime(wkDate);
    //            //営業時間外を選択している場合は、翌営業開始時刻に補正する
    //            var newDate = smbScript.GetDateExcludeOutOfTime(wkDate);
    //
    //            $(this).get(0).valueAsDate = newDate;
    //            //DateTimeSelectorで選択した日時を表示用に変換してラベルに設定
    //            $("#DetailLPlanFinishLabel").text(smbScript.ConvertDateOrTime(newDate, $("#hidShowDate").val()));
    //
    //            //チップ詳細(小)に反映
    //            $("#DetailSPlanFinishDateTimeSelector").get(0).valueAsDate = newDate;
    //            $("#DetailSPlanFinishLabel").text($("#DetailLPlanFinishLabel").text());
    //
    //            //作業開始予定日時がnullでない場合
    //            if ($("#DetailLPlanStartDateTimeSelector").get(0).valueAsDate != null) {
    //                //開始日時と終了日時から差分時間を計算し、作業時間に設定する　※営業時間外の時間帯は減算して計算
    //                //var timeSpan = smbScript.CalcTimeSpan($("#DetailLPlanStartDateTimeSelector").get(0).valueAsDate, $(this).get(0).valueAsDate);
    //                var timeSpan = smbScript.CalcTimeSpan_ExcludeOutOfTime($("#DetailLPlanStartDateTimeSelector").get(0).valueAsDate, $(this).get(0).valueAsDate);
    //                if (timeSpan != null) {
    //                    //最大値を超える場合、作業時間最大値（分）をセット
    //                    if (timeSpan > C_SC3240201_MAXWORKTIME) {
    //                        timeSpan = C_SC3240201_MAXWORKTIME;
    //
    //                        //開始日時と加算時間を元に、終了日時を計算する（営業時間外の時間帯は除外して計算）
    //                        var newEndDateTime = smbScript.CalcEndDate_ExcludeOutOfTime($("#DetailLPlanStartDateTimeSelector").get(0).valueAsDate, C_SC3240201_MAXWORKTIME);
    //
    //                        $("#DetailLPlanFinishDateTimeSelector").get(0).valueAsDate = newEndDateTime;
    //                        $("#DetailLPlanFinishLabel").text(smbScript.ConvertDateOrTime(newEndDateTime, $("#hidShowDate").val()));
    //
    //                        //チップ詳細(小)に反映
    //                        $("#DetailSPlanFinishDateTimeSelector").get(0).valueAsDate = newEndDateTime;
    //                        $("#DetailSPlanFinishLabel").text($("#DetailLPlanFinishLabel").text());
    //                    }
    //                    $("#DetailSWorkTimeTxt").val(timeSpan);
    //                    $("#DetailLWorkTimeTxt").val(timeSpan);
    //                    $("#DetailSWorkTimeLabel").text(timeSpan + $("#WordWorkTimeUnitHidden").val());
    //                    $("#DetailLWorkTimeLabel").text(timeSpan + $("#WordWorkTimeUnitHidden").val());
    //                }
    //            }
    //        }
    //        else {
    //            //ラベルをEmptyにする
    //            $("#DetailLPlanFinishLabel").text("");
    //
    //            //チップ詳細(小)に反映
    //            $("#DetailSPlanFinishDateTimeSelector").get(0).valueAsDate = null;
    //            $("#DetailSPlanFinishLabel").text("");
    //        }
    //
    //        //必須項目がEmptyなら登録ボタンを非活性にする
    //        $("#DetailRegisterBtn").attr("disabled", IsMandatoryChipDetailEmpty());
    //    });
    //
    ////納車予定時間
    //$("#DetailLPlanDeriveredDateTimeSelector")
    //    .click(function () {
    //        //チップ詳細(大)のコントロールにフォーカスが当たらない不具合を解消する
    //        $(this).focus();
    //    })
    //    .blur(function () {
    //        //DateTimeSelectorで選択した日時を表示用に変換してラベルに設定
    //        $("#DetailLPlanDeriveredLabel").text(smbScript.ConvertDateOrTime($(this).get(0).valueAsDate, $("#hidShowDate").val()));
    //        //チップ詳細(小)に反映
    //        $("#DetailSPlanDeriveredDateTimeSelector").get(0).valueAsDate = $(this).get(0).valueAsDate;
    //        $("#DetailSPlanDeriveredLabel").text($("#DetailLPlanDeriveredLabel").text());
    //    });
    //
    ////作業開始実績時間
    //$("#DetailLProcessStartDateTimeSelector")
    //    .click(function () {
    //        //チップ詳細(大)のコントロールにフォーカスが当たらない不具合を解消する
    //        $(this).focus();
    //    })
    //    .blur(function () {
    //        //DateTimeSelectorで選択した日時を表示用に変換してラベルに設定
    //        $("#DetailLProcessStartLabel").text(smbScript.ConvertDateOrTime($(this).get(0).valueAsDate, $("#hidShowDate").val()));
    //        //チップ詳細(小)に反映
    //        $("#DetailSProcessStartDateTimeSelector").get(0).valueAsDate = $(this).get(0).valueAsDate;
    //        $("#DetailSProcessStartLabel").text($("#DetailLProcessStartLabel").text());
    //    });
    //
    ////作業終了実績時間
    //$("#DetailLProcessFinishDateTimeSelector")
    //    .click(function () {
    //        $(this).focus();
    //    })
    //    .blur(function () {
    //        //DateTimeSelectorで選択した日時を表示用に変換してラベルに設定
    //        $("#DetailLProcessFinishLabel").text(smbScript.ConvertDateOrTime($(this).get(0).valueAsDate, $("#hidShowDate").val()));
    //        //チップ詳細(小)に反映
    //        $("#DetailSProcessFinishDateTimeSelector").get(0).valueAsDate = $(this).get(0).valueAsDate;
    //        $("#DetailSProcessFinishLabel").text($("#DetailLProcessFinishLabel").text());
    //    });

    // 2020/01/16 NSK 夏目  TR-SVT-TMT-20200110-002 追加jobでサービスの種類を選択できない start
    ////来店予定時間
    //$("#DetailLPlanVisitDateTimeSelector")
    //    .click(function () {
    //        //チップ詳細(大)のコントロールにフォーカスが当たらない不具合を解消する
    //        $(this).focus();
    //    })
    //    .blur(function () {
    //        $("#DetailLPlanVisitLabel").text(smbScript.ConvertDateOrTime(smbScript.changeStringToDateIcrop($(this).get(0).value), $("#hidShowDate").val()));
            
    //        //チップ詳細(小)に反映
    //        $("#DetailSPlanVisitDateTimeSelector").get(0).value = $(this).get(0).value;
    //        $("#DetailSPlanVisitLabel").text($("#DetailLPlanVisitLabel").text());
    //    });
    //来店予定時間
    $("#DetailLPlanVisitDateTimeSelector")
        .click(function () {
            //チップ詳細(大)のコントロールにフォーカスが当たらない不具合を解消する
            $(this).focus();
        })
        .change(function () {
            $("#DetailLPlanVisitLabel").text(smbScript.ConvertDateOrTime(smbScript.changeStringToDateIcrop($(this).get(0).value), $("#hidShowDate").val()));

            //チップ詳細(小)に反映
            $("#DetailSPlanVisitDateTimeSelector").get(0).value = $(this).get(0).value;
            $("#DetailSPlanVisitLabel").text($("#DetailLPlanVisitLabel").text());
        });
    // 2020/01/16 NSK 夏目  TR-SVT-TMT-20200110-002 追加jobでサービスの種類を選択できない end

    // 2020/01/16 NSK 夏目  TR-SVT-TMT-20200110-002 追加jobでサービスの種類を選択できない start
    ////作業開始予定時間
    //$("#DetailLPlanStartDateTimeSelector")
    //    .click(function () {
    //        //チップ詳細(大)のコントロールにフォーカスが当たらない不具合を解消する
    //        $(this).focus();
    //    })
    //    .blur(function () {

    //        //作業開始予定日時がnullでない場合
    //        if ($(this).get(0).value != null && $(this).get(0).value != "") {

    //            //作業開始予定日時を5分単位で丸め込んだ日時を計算し、DateTimSelectorに設定しなおす
    //            var wkDate = new Date(smbScript.RoundUpTimeTo5Units(smbScript.changeStringToDateIcrop($(this).get(0).value)));

    //            //営業時間外を選択している場合は、翌営業開始時刻に補正する
    //            var newDate = smbScript.GetDateExcludeOutOfTime(wkDate);

    //            $(this).get(0).value = smbScript.getDateTimelocalDate(newDate);

    //            //DateTimeSelectorで選択した日時を表示用に変換してラベルに設定
    //            $("#DetailLPlanStartLabel").text(smbScript.ConvertDateOrTime(newDate, $("#hidShowDate").val()));

    //            //チップ詳細(小)に反映
    //            $("#DetailSPlanStartDateTimeSelector").get(0).value = smbScript.getDateTimelocalDate(newDate);
    //            $("#DetailSPlanStartLabel").text($("#DetailLPlanStartLabel").text());

    //            //作業終了予定日時がnullでない場合
    //            if ($("#DetailLPlanFinishDateTimeSelector").get(0).value != null && $("#DetailLPlanFinishDateTimeSelector").get(0).value != "") {

    //                //開始日時と終了日時から差分時間を計算し、作業時間に設定する　※営業時間外の時間帯は減算して計算
    //                var timeSpan = smbScript.CalcTimeSpan_ExcludeOutOfTime(smbScript.changeStringToDateIcrop($(this).get(0).value), smbScript.changeStringToDateIcrop($("#DetailLPlanFinishDateTimeSelector").get(0).value));

    //                if (timeSpan != null) {
    //                    //最大値を超える場合、作業時間最大値（分）をセット
    //                    if (timeSpan > C_SC3240201_MAXWORKTIME) {
    //                        timeSpan = C_SC3240201_MAXWORKTIME;

    //                        //開始日時と加算時間を元に、終了日時を計算する（営業時間外の時間帯は除外して計算）
    //                        var newEndDateTime = smbScript.CalcEndDate_ExcludeOutOfTime(smbScript.changeStringToDateIcrop($("#DetailLPlanStartDateTimeSelector").get(0).value), C_SC3240201_MAXWORKTIME);

    //                        $("#DetailLPlanFinishDateTimeSelector").get(0).value = smbScript.getDateTimelocalDate(newEndDateTime);
    //                        $("#DetailLPlanFinishLabel").text(smbScript.ConvertDateOrTime(newEndDateTime, $("#hidShowDate").val()));

    //                        //チップ詳細(小)に反映
    //                        $("#DetailSPlanFinishDateTimeSelector").get(0).value = smbScript.getDateTimelocalDate(newEndDateTime);
    //                        $("#DetailSPlanFinishLabel").text($("#DetailLPlanFinishLabel").text());
    //                    }
    //                    $("#DetailSWorkTimeTxt").val(timeSpan);
    //                    $("#DetailLWorkTimeTxt").val(timeSpan);
    //                    $("#DetailSWorkTimeLabel").text(timeSpan + $("#WordWorkTimeUnitHidden").val());
    //                    $("#DetailLWorkTimeLabel").text(timeSpan + $("#WordWorkTimeUnitHidden").val());
    //                }
    //            }
    //        }
    //        else {
    //            //ラベルをEmptyにする
    //            $("#DetailLPlanStartLabel").text("");

    //            //チップ詳細(小)に反映
    //            $("#DetailSPlanStartDateTimeSelector").get(0).value = null;
    //            $("#DetailSPlanStartLabel").text("");
    //        }

    //        //必須項目がEmptyなら登録ボタンを非活性にする
    //        $("#DetailRegisterBtn").attr("disabled", IsMandatoryChipDetailEmpty());
    //    });
    //作業開始予定時間
    $("#DetailLPlanStartDateTimeSelector")
        .click(function () {
            //チップ詳細(大)のコントロールにフォーカスが当たらない不具合を解消する
            $(this).focus();
        })
        .change(function () {

            //作業開始予定日時がnullでない場合
            if ($(this).get(0).value != null && $(this).get(0).value != "") {

                //作業開始予定日時を5分単位で丸め込んだ日時を計算し、DateTimSelectorに設定しなおす
                var wkDate = new Date(smbScript.RoundUpTimeTo5Units(smbScript.changeStringToDateIcrop($(this).get(0).value)));

                //営業時間外を選択している場合は、翌営業開始時刻に補正する
                var newDate = smbScript.GetDateExcludeOutOfTime(wkDate);

                $(this).get(0).value = smbScript.getDateTimelocalDate(newDate);

                //DateTimeSelectorで選択した日時を表示用に変換してラベルに設定
                $("#DetailLPlanStartLabel").text(smbScript.ConvertDateOrTime(newDate, $("#hidShowDate").val()));

                //チップ詳細(小)に反映
                $("#DetailSPlanStartDateTimeSelector").get(0).value = smbScript.getDateTimelocalDate(newDate);
                $("#DetailSPlanStartLabel").text($("#DetailLPlanStartLabel").text());

                //作業終了予定日時がnullでない場合
                if ($("#DetailLPlanFinishDateTimeSelector").get(0).value != null && $("#DetailLPlanFinishDateTimeSelector").get(0).value != "") {

                    //開始日時と終了日時から差分時間を計算し、作業時間に設定する　※営業時間外の時間帯は減算して計算
                    var timeSpan = smbScript.CalcTimeSpan_ExcludeOutOfTime(smbScript.changeStringToDateIcrop($(this).get(0).value), smbScript.changeStringToDateIcrop($("#DetailLPlanFinishDateTimeSelector").get(0).value));

                    if (timeSpan != null) {
                        //最大値を超える場合、作業時間最大値（分）をセット
                        if (timeSpan > C_SC3240201_MAXWORKTIME) {
                            timeSpan = C_SC3240201_MAXWORKTIME;

                            //開始日時と加算時間を元に、終了日時を計算する（営業時間外の時間帯は除外して計算）
                            var newEndDateTime = smbScript.CalcEndDate_ExcludeOutOfTime(smbScript.changeStringToDateIcrop($("#DetailLPlanStartDateTimeSelector").get(0).value), C_SC3240201_MAXWORKTIME);

                            $("#DetailLPlanFinishDateTimeSelector").get(0).value = smbScript.getDateTimelocalDate(newEndDateTime);
                            $("#DetailLPlanFinishLabel").text(smbScript.ConvertDateOrTime(newEndDateTime, $("#hidShowDate").val()));

                            //チップ詳細(小)に反映
                            $("#DetailSPlanFinishDateTimeSelector").get(0).value = smbScript.getDateTimelocalDate(newEndDateTime);
                            $("#DetailSPlanFinishLabel").text($("#DetailLPlanFinishLabel").text());
                        }
                        $("#DetailSWorkTimeTxt").val(timeSpan);
                        $("#DetailLWorkTimeTxt").val(timeSpan);
                        $("#DetailSWorkTimeLabel").text(timeSpan + $("#WordWorkTimeUnitHidden").val());
                        $("#DetailLWorkTimeLabel").text(timeSpan + $("#WordWorkTimeUnitHidden").val());
                    }
                }
            }
            else {
                //ラベルをEmptyにする
                $("#DetailLPlanStartLabel").text("");

                //チップ詳細(小)に反映
                $("#DetailSPlanStartDateTimeSelector").get(0).value = null;
                $("#DetailSPlanStartLabel").text("");
            }

            //必須項目がEmptyなら登録ボタンを非活性にする
            $("#DetailRegisterBtn").attr("disabled", IsMandatoryChipDetailEmpty());
        });
    // 2020/01/16 NSK 夏目  TR-SVT-TMT-20200110-002 追加jobでサービスの種類を選択できない end

    // 2020/01/16 NSK 夏目  TR-SVT-TMT-20200110-002 追加jobでサービスの種類を選択できない start
    //作業終了予定時間
    //$("#DetailLPlanFinishDateTimeSelector")
    //    .click(function () {
    //        //チップ詳細(大)のコントロールにフォーカスが当たらない不具合を解消する
    //        $(this).focus();
    //    })
    //    .blur(function () {

    //        //作業終了予定日時がnullでない場合
    //        if ($(this).get(0).value != null && $(this).get(0).value != "") {

    //            //作業終了予定日時を5分単位で丸め込んだ日時を計算し、DateTimSelectorに設定しなおす
    //            var wkDate = new Date(smbScript.RoundUpTimeTo5Units(smbScript.changeStringToDateIcrop($(this).get(0).value)));

    //            //営業時間外を選択している場合は、翌営業開始時刻に補正する
    //            var newDate = smbScript.GetDateExcludeOutOfTime(wkDate);

    //            $(this).get(0).value = smbScript.getDateTimelocalDate(newDate);

    //            //DateTimeSelectorで選択した日時を表示用に変換してラベルに設定
    //            $("#DetailLPlanFinishLabel").text(smbScript.ConvertDateOrTime(newDate, $("#hidShowDate").val()));

    //            //チップ詳細(小)に反映
    //            $("#DetailSPlanFinishDateTimeSelector").get(0).value = smbScript.getDateTimelocalDate(newDate);
    //            $("#DetailSPlanFinishLabel").text($("#DetailLPlanFinishLabel").text());

    //            //作業開始予定日時がnullでない場合
    //            if ($("#DetailLPlanStartDateTimeSelector").get(0).value != null && $("#DetailLPlanStartDateTimeSelector").get(0).value != "") {

    //                //開始日時と終了日時から差分時間を計算し、作業時間に設定する　※営業時間外の時間帯は減算して計算
    //                var timeSpan = smbScript.CalcTimeSpan_ExcludeOutOfTime(smbScript.changeStringToDateIcrop($("#DetailLPlanStartDateTimeSelector").get(0).value), smbScript.changeStringToDateIcrop($(this).get(0).value));

    //                if (timeSpan != null) {
    //                    //最大値を超える場合、作業時間最大値（分）をセット
    //                    if (timeSpan > C_SC3240201_MAXWORKTIME) {
    //                        timeSpan = C_SC3240201_MAXWORKTIME;

    //                        //開始日時と加算時間を元に、終了日時を計算する（営業時間外の時間帯は除外して計算）
    //                        var newEndDateTime = smbScript.CalcEndDate_ExcludeOutOfTime(smbScript.changeStringToDateIcrop($("#DetailLPlanStartDateTimeSelector").get(0).value), C_SC3240201_MAXWORKTIME);

    //                        $("#DetailLPlanFinishDateTimeSelector").get(0).value = smbScript.getDateTimelocalDate(newEndDateTime);
    //                        $("#DetailLPlanFinishLabel").text(smbScript.ConvertDateOrTime(newEndDateTime, $("#hidShowDate").val()));

    //                        //チップ詳細(小)に反映
    //                        $("#DetailSPlanFinishDateTimeSelector").get(0).value = smbScript.getDateTimelocalDate(newEndDateTime);
    //                        $("#DetailSPlanFinishLabel").text($("#DetailLPlanFinishLabel").text());
    //                    }
    //                    $("#DetailSWorkTimeTxt").val(timeSpan);
    //                    $("#DetailLWorkTimeTxt").val(timeSpan);
    //                    $("#DetailSWorkTimeLabel").text(timeSpan + $("#WordWorkTimeUnitHidden").val());
    //                    $("#DetailLWorkTimeLabel").text(timeSpan + $("#WordWorkTimeUnitHidden").val());
    //                }
    //            }
    //        }
    //        else {
    //            //ラベルをEmptyにする
    //            $("#DetailLPlanFinishLabel").text("");

    //            //チップ詳細(小)に反映
    //            $("#DetailSPlanFinishDateTimeSelector").get(0).value = null;
    //            $("#DetailSPlanFinishLabel").text("");
    //        }

    //        //必須項目がEmptyなら登録ボタンを非活性にする
    //        $("#DetailRegisterBtn").attr("disabled", IsMandatoryChipDetailEmpty());
    //    });
    $("#DetailLPlanFinishDateTimeSelector")
        .click(function () {
            //チップ詳細(大)のコントロールにフォーカスが当たらない不具合を解消する
            $(this).focus();
        })
        .change(function () {

            //作業終了予定日時がnullでない場合
            if ($(this).get(0).value != null && $(this).get(0).value != "") {

                //作業終了予定日時を5分単位で丸め込んだ日時を計算し、DateTimSelectorに設定しなおす
                var wkDate = new Date(smbScript.RoundUpTimeTo5Units(smbScript.changeStringToDateIcrop($(this).get(0).value)));

                //営業時間外を選択している場合は、翌営業開始時刻に補正する
                var newDate = smbScript.GetDateExcludeOutOfTime(wkDate);

                $(this).get(0).value = smbScript.getDateTimelocalDate(newDate);

                //DateTimeSelectorで選択した日時を表示用に変換してラベルに設定
                $("#DetailLPlanFinishLabel").text(smbScript.ConvertDateOrTime(newDate, $("#hidShowDate").val()));

                //チップ詳細(小)に反映
                $("#DetailSPlanFinishDateTimeSelector").get(0).value = smbScript.getDateTimelocalDate(newDate);
                $("#DetailSPlanFinishLabel").text($("#DetailLPlanFinishLabel").text());

                //作業開始予定日時がnullでない場合
                if ($("#DetailLPlanStartDateTimeSelector").get(0).value != null && $("#DetailLPlanStartDateTimeSelector").get(0).value != "") {

                    //開始日時と終了日時から差分時間を計算し、作業時間に設定する　※営業時間外の時間帯は減算して計算
                    var timeSpan = smbScript.CalcTimeSpan_ExcludeOutOfTime(smbScript.changeStringToDateIcrop($("#DetailLPlanStartDateTimeSelector").get(0).value), smbScript.changeStringToDateIcrop($(this).get(0).value));

                    if (timeSpan != null) {
                        //最大値を超える場合、作業時間最大値（分）をセット
                        if (timeSpan > C_SC3240201_MAXWORKTIME) {
                            timeSpan = C_SC3240201_MAXWORKTIME;

                            //開始日時と加算時間を元に、終了日時を計算する（営業時間外の時間帯は除外して計算）
                            var newEndDateTime = smbScript.CalcEndDate_ExcludeOutOfTime(smbScript.changeStringToDateIcrop($("#DetailLPlanStartDateTimeSelector").get(0).value), C_SC3240201_MAXWORKTIME);

                            $("#DetailLPlanFinishDateTimeSelector").get(0).value = smbScript.getDateTimelocalDate(newEndDateTime);
                            $("#DetailLPlanFinishLabel").text(smbScript.ConvertDateOrTime(newEndDateTime, $("#hidShowDate").val()));

                            //チップ詳細(小)に反映
                            $("#DetailSPlanFinishDateTimeSelector").get(0).value = smbScript.getDateTimelocalDate(newEndDateTime);
                            $("#DetailSPlanFinishLabel").text($("#DetailLPlanFinishLabel").text());
                        }
                        $("#DetailSWorkTimeTxt").val(timeSpan);
                        $("#DetailLWorkTimeTxt").val(timeSpan);
                        $("#DetailSWorkTimeLabel").text(timeSpan + $("#WordWorkTimeUnitHidden").val());
                        $("#DetailLWorkTimeLabel").text(timeSpan + $("#WordWorkTimeUnitHidden").val());
                    }
                }
            }
            else {
                //ラベルをEmptyにする
                $("#DetailLPlanFinishLabel").text("");

                //チップ詳細(小)に反映
                $("#DetailSPlanFinishDateTimeSelector").get(0).value = null;
                $("#DetailSPlanFinishLabel").text("");
            }

            //必須項目がEmptyなら登録ボタンを非活性にする
            $("#DetailRegisterBtn").attr("disabled", IsMandatoryChipDetailEmpty());
        });
    // 2020/01/16 NSK 夏目  TR-SVT-TMT-20200110-002 追加jobでサービスの種類を選択できない end

    // 2020/01/16 NSK 夏目  TR-SVT-TMT-20200110-002 追加jobでサービスの種類を選択できない start
    ////納車予定時間
    //$("#DetailLPlanDeriveredDateTimeSelector")
    //    .click(function () {
    //        //チップ詳細(大)のコントロールにフォーカスが当たらない不具合を解消する
    //        $(this).focus();
    //    })
    //    .blur(function () {
    //        //DateTimeSelectorで選択した日時を表示用に変換してラベルに設定
    //        $("#DetailLPlanDeriveredLabel").text(smbScript.ConvertDateOrTime(smbScript.changeStringToDateIcrop($(this).get(0).value), $("#hidShowDate").val()));

    //        //チップ詳細(小)に反映
    //        $("#DetailSPlanDeriveredDateTimeSelector").get(0).value = $(this).get(0).value;
    //        $("#DetailSPlanDeriveredLabel").text($("#DetailLPlanDeriveredLabel").text());
    //    });
    //納車予定時間
    $("#DetailLPlanDeriveredDateTimeSelector")
        .click(function () {
            //チップ詳細(大)のコントロールにフォーカスが当たらない不具合を解消する
            $(this).focus();
        })
        .change(function () {
            //DateTimeSelectorで選択した日時を表示用に変換してラベルに設定
            $("#DetailLPlanDeriveredLabel").text(smbScript.ConvertDateOrTime(smbScript.changeStringToDateIcrop($(this).get(0).value), $("#hidShowDate").val()));

            //チップ詳細(小)に反映
            $("#DetailSPlanDeriveredDateTimeSelector").get(0).value = $(this).get(0).value;
            $("#DetailSPlanDeriveredLabel").text($("#DetailLPlanDeriveredLabel").text());
        });
    // 2020/01/16 NSK 夏目  TR-SVT-TMT-20200110-002 追加jobでサービスの種類を選択できない end

    //作業開始実績時間
    $("#DetailLProcessStartDateTimeSelector")
        .click(function () {
            //チップ詳細(大)のコントロールにフォーカスが当たらない不具合を解消する
            $(this).focus();
        })
        .blur(function () {
            //DateTimeSelectorで選択した日時を表示用に変換してラベルに設定
            $("#DetailLProcessStartLabel").text(smbScript.ConvertDateOrTime(smbScript.changeStringToDateIcrop($(this).get(0).value), $("#hidShowDate").val()));

            //チップ詳細(小)に反映
            $("#DetailSProcessStartDateTimeSelector").get(0).value = $(this).get(0).value;
            $("#DetailSProcessStartLabel").text($("#DetailLProcessStartLabel").text());
        });

    //作業終了実績時間
    $("#DetailLProcessFinishDateTimeSelector")
        .click(function () {
            $(this).focus();
        })
        .blur(function () {
            //DateTimeSelectorで選択した日時を表示用に変換してラベルに設定
            $("#DetailLProcessFinishLabel").text(smbScript.ConvertDateOrTime(smbScript.changeStringToDateIcrop($(this).get(0).value), $("#hidShowDate").val()));

            //チップ詳細(小)に反映
            $("#DetailSProcessFinishDateTimeSelector").get(0).value = $(this).get(0).value;
            $("#DetailSProcessFinishLabel").text($("#DetailLProcessFinishLabel").text());
        });
    //2013/12/26 TMEJ 下村 次世代e-CRBタブレット(サービス) iOS7.0 VersionUp対応 END

    //整備種類
    $("#DetailLMaintenanceTypeList")
        .click(function () {
            //チップ詳細(大)のコントロールにフォーカスが当たらない不具合を解消する
            $(this).focus();
        })
        .change(function () {
            var e = document.getElementById("DetailLMaintenanceTypeList");
            $("#DetailLMaintenanceTypeLabel").text(e.options[e.selectedIndex].text);
            //チップ詳細(小)に反映
            $("#DetailSMaintenanceTypeList").val(e.options[e.selectedIndex].value);
            $("#DetailSMaintenanceTypeLabel").text(e.options[e.selectedIndex].text);
        })

    // 2020/01/16 NSK 夏目  TR-SVT-TMT-20200110-002 追加jobでサービスの種類を選択できない start
    //$("#DetailLMaintenanceTypeList")
    //    .blur(function () {
    //        var e = document.getElementById("DetailLMaintenanceTypeList");
    //        $("#DetailLMaintenanceTypeLabel").text(e.options[e.selectedIndex].text);
    //        //チップ詳細(小)に反映
    //        $("#DetailSMaintenanceTypeList").val(e.options[e.selectedIndex].value);
    //        $("#DetailSMaintenanceTypeLabel").text(e.options[e.selectedIndex].text);
    //    });
    $("#DetailLMaintenanceTypeList")
        .change(function () {
            var e = document.getElementById("DetailLMaintenanceTypeList");
            $("#DetailLMaintenanceTypeLabel").text(e.options[e.selectedIndex].text);
            //チップ詳細(小)に反映
            $("#DetailSMaintenanceTypeList").val(e.options[e.selectedIndex].value);
            $("#DetailSMaintenanceTypeLabel").text(e.options[e.selectedIndex].text);
        });
    // 2020/01/16 NSK 夏目  TR-SVT-TMT-20200110-002 追加jobでサービスの種類を選択できない end

    //整備種類コンボボックスのフォーカスInイベント
    $("#DetailLMaintenanceTypeList").bind('focusin', DetailSLSvcClassIDFocusIn);

    //整備種類コンボボックスのフォーカスOutイベント
    // 2020/01/16 NSK 夏目  TR-SVT-TMT-20200110-002 追加jobでサービスの種類を選択できない start
    //$("#DetailLMaintenanceTypeList").bind('focusout', DetailSLSvcClassIDFocusOut);
    $("#DetailLMaintenanceTypeList").bind('change', DetailSLSvcClassIDFocusOut);
    // 2020/01/16 NSK 夏目  TR-SVT-TMT-20200110-002 追加jobでサービスの種類を選択できない end

    //整備名
    $("#DetailLMercList")
        .click(function () {
            //チップ詳細(大)のコントロールにフォーカスが当たらない不具合を解消する
            $(this).focus();
        })
        .change(function () {
            var e = document.getElementById("DetailLMercList");
            $("#DetailLMercLabel").text(e.options[e.selectedIndex].text);
            //チップ詳細(小)に反映
            $("#DetailSMercList").val(e.options[e.selectedIndex].value);
            $("#DetailSMercLabel").text(e.options[e.selectedIndex].text);
        })

    // 2020/01/16 NSK 夏目  TR-SVT-TMT-20200110-002 追加jobでサービスの種類を選択できない start
    //$("#DetailLMercList")  
    //    .blur(function () {
    //        var e = document.getElementById("DetailLMercList");
    //        $("#DetailLMercLabel").text(e.options[e.selectedIndex].text);
    //        //チップ詳細(小)に反映
    //        $("#DetailSMercList").val(e.options[e.selectedIndex].value);
    //        $("#DetailSMerceLabel").text(e.options[e.selectedIndex].text);
    //    });
    $("#DetailLMercList")
        .change(function () {
            var e = document.getElementById("DetailLMercList");
            $("#DetailLMercLabel").text(e.options[e.selectedIndex].text);
            //チップ詳細(小)に反映
            $("#DetailSMercList").val(e.options[e.selectedIndex].value);
            $("#DetailSMerceLabel").text(e.options[e.selectedIndex].text);
        });
    // 2020/01/16 NSK 夏目  TR-SVT-TMT-20200110-002 追加jobでサービスの種類を選択できない end

    //整備名コンボボックスのフォーカスInイベント
    $("#DetailLMercList").bind('focusin', DetailSLMercIDFocusIn);

    //整備名コンボボックスのフォーカスOutイベント
    // 2020/01/16 NSK 夏目  TR-SVT-TMT-20200110-002 追加jobでサービスの種類を選択できない start
    //$("#DetailLMercList").bind('focusout', DetailSLMercIDFocusOut);
    $("#DetailLMercList").bind('change', DetailSLMercIDFocusOut);
    // 2020/01/16 NSK 夏目  TR-SVT-TMT-20200110-002 追加jobでサービスの種類を選択できない end

    //作業時間
    $("#DetailLWorkTimeTxt")
        .click(function () {
            //編集可能な場合のみ、ラベルとテキストを切り替える
            if ($(this).hasClass(C_SC3240201CLASS_TEXTBLACK) == false) {
                //チップ詳細(大)のコントロールにフォーカスが当たらない不具合を解消する
                $(this).focus();

                $("#DetailLWorkTimeTxt").css({
                    "opacity": "1"
                });
                $("#DetailLWorkTimeLabel").css({
                    "opacity": "0"
                });
            }
        })
        .blur(function () {
            //編集可能な場合のみ、ラベルとテキストを切り替える
            if ($(this).hasClass(C_SC3240201CLASS_TEXTBLACK) == false) {
                $("#DetailLWorkTimeTxt").css({
                    "opacity": "0"
                });
                $("#DetailLWorkTimeLabel").css({
                    "opacity": "1"
                });
            }
        })
        .change(function (e) {
            var min;

            //作業時間を空白にした場合、5を設定する
            if ($("#DetailLWorkTimeTxt").val() == "") {
                min = gResizeInterval;
            }
            //それ以外は5分単位で丸め込む
            else {
                min = smbScript.RoundUpToNumUnits($("#DetailLWorkTimeTxt").val(), gResizeInterval, gResizeInterval, C_SC3240201_MAXWORKTIME);
            }

            $("#DetailSWorkTimeTxt").val(min);
            $("#DetailLWorkTimeTxt").val(min);
            $("#DetailSWorkTimeLabel").text(min + $("#WordWorkTimeUnitHidden").val());
            $("#DetailLWorkTimeLabel").text(min + $("#WordWorkTimeUnitHidden").val());

            //作業開始日時がnullでない場合、設定した作業時間に合わせて作業終了予定時間を変更する
            //2013/12/26 TMEJ 下村 次世代e-CRBタブレット(サービス) iOS7.0 VersionUp対応 START
            //var startDateTime = $("#DetailLPlanStartDateTimeSelector").get(0).valueAsDate;
            var startDateTime = smbScript.changeStringToDateIcrop($("#DetailLPlanStartDateTimeSelector").get(0).value);
            //2013/12/26 TMEJ 下村 次世代e-CRBタブレット(サービス) iOS7.0 VersionUp対応 END

            //2014/04/01 TMEJ 丁 次世代e-CRBタブレット(サービス) チーフテクニシャン機能開発 START
            //if (startDateTime != null){
            if ((startDateTime != null) && (!$("#DetailLPlanFinishDateTimeSelector")[0].disabled)) {
                //2014/04/01 TMEJ 丁 次世代e-CRBタブレット(サービス) チーフテクニシャン機能開発 END
                //var newEndDateTime = smbScript.CalcEndDateTime(startDateTime, min);

                //開始日時と加算時間を元に、終了日時を計算する（営業時間外の時間帯は除外して計算）
                var newEndDateTime = smbScript.CalcEndDate_ExcludeOutOfTime(startDateTime, min);

                //2013/12/26 TMEJ 下村 次世代e-CRBタブレット(サービス) iOS7.0 VersionUp対応 START
                //$("#DetailLPlanFinishDateTimeSelector").get(0).valueAsDate = newEndDateTime;
                $("#DetailLPlanFinishDateTimeSelector").get(0).value = smbScript.getDateTimelocalDate(newEndDateTime);
                //2013/12/26 TMEJ 下村 次世代e-CRBタブレット(サービス) iOS7.0 VersionUp対応 END

                $("#DetailLPlanFinishLabel").text(smbScript.ConvertDateOrTime(newEndDateTime, $("#hidShowDate").val()));

                //チップ詳細(小)に反映
                //2013/12/26 TMEJ 下村 次世代e-CRBタブレット(サービス) iOS7.0 VersionUp対応 START
                //$("#DetailSPlanFinishDateTimeSelector").get(0).valueAsDate = newEndDateTime;
                $("#DetailSPlanFinishDateTimeSelector").get(0).value = smbScript.getDateTimelocalDate(newEndDateTime);
                //2013/12/26 TMEJ 下村 次世代e-CRBタブレット(サービス) iOS7.0 VersionUp対応 END

                $("#DetailSPlanFinishLabel").text($("#DetailLPlanFinishLabel").text());

                //必須項目がEmptyなら登録ボタンを非活性にする
                $("#DetailRegisterBtn").attr("disabled", IsMandatoryChipDetailEmpty());
            }
        })
        .CustomTextBox({
            clear: function (e) {    //CustomTextBoxの×ボタンタップイベント
                $("#DetailSWorkTimeTxt").val(gResizeInterval);
                $("#DetailLWorkTimeTxt").val(gResizeInterval);
                $("#DetailSWorkTimeLabel").text(gResizeInterval + $("#WordWorkTimeUnitHidden").val());
                $("#DetailLWorkTimeLabel").text(gResizeInterval + $("#WordWorkTimeUnitHidden").val());

                //作業開始日時がnullでない場合、設定した作業時間に合わせて作業終了予定時間を変更する
                //2013/12/26 TMEJ 下村 次世代e-CRBタブレット(サービス) iOS7.0 VersionUp対応 START
                //var startDateTime = $("#DetailLPlanStartDateTimeSelector").get(0).valueAsDate;
                var startDateTime = smbScript.changeStringToDateIcrop($("#DetailLPlanStartDateTimeSelector").get(0).value);
                //2013/12/26 TMEJ 下村 次世代e-CRBタブレット(サービス) iOS7.0 VersionUp対応 END

                //2014/04/01 TMEJ 丁 次世代e-CRBタブレット(サービス) チーフテクニシャン機能開発 START
                //if (startDateTime != null){
                if ((startDateTime != null) && (!$("#DetailLPlanFinishDateTimeSelector")[0].disabled)) {
                //2014/04/01 TMEJ 丁 次世代e-CRBタブレット(サービス) チーフテクニシャン機能開発 END

                    var newEndDateTime = smbScript.CalcEndDateTime(startDateTime, gResizeInterval);

                    //2013/12/26 TMEJ 下村 次世代e-CRBタブレット(サービス) iOS7.0 VersionUp対応 START
                    //$("#DetailLPlanFinishDateTimeSelector").get(0).valueAsDate = newEndDateTime;
                    $("#DetailLPlanFinishDateTimeSelector").get(0).value = smbScript.getDateTimelocalDate(newEndDateTime);
                    //2013/12/26 TMEJ 下村 次世代e-CRBタブレット(サービス) iOS7.0 VersionUp対応 END

                    $("#DetailLPlanFinishLabel").text(smbScript.ConvertDateOrTime(newEndDateTime, $("#hidShowDate").val()));

                    //チップ詳細(小)に反映
                    //2013/12/26 TMEJ 下村 次世代e-CRBタブレット(サービス) iOS7.0 VersionUp対応 START
                    //$("#DetailSPlanFinishDateTimeSelector").get(0).valueAsDate = newEndDateTime;
                    $("#DetailSPlanFinishDateTimeSelector").get(0).value = smbScript.getDateTimelocalDate(newEndDateTime);
                    //2013/12/26 TMEJ 下村 次世代e-CRBタブレット(サービス) iOS7.0 VersionUp対応 END

                    $("#DetailSPlanFinishLabel").text($("#DetailLPlanFinishLabel").text());

                    //必須項目がEmptyなら登録ボタンを非活性にする
                    $("#DetailRegisterBtn").attr("disabled", IsMandatoryChipDetailEmpty());
                }
            }
        });


    //ご用命
    $("#DetailLOrderTxt")
        .click(function () {
            //チップ詳細(大)のコントロールにフォーカスが当たらない不具合を解消する
            $(this).focus();
        })
        .blur(function () {
            $("#DetailSOrderTxt").val($("#DetailLOrderTxt").val());
            ControlLengthTextarea($("#DetailLOrderTxt"));
            AdjusterDetailTextArea($("#DetailLOrderTxt"), $("#DetailLOrderDt"));
            //スクロール位置ずれ調整
            //AdjustChipDetailDisplay();
        	//2013/12/04 TMEJ 下村 タブレット版SMB チーフテクニシャン機能開発 START
            //$("#DetailLDummyBtn").focus();
            //$("#DetailLDummyBtn").blur();
		    $("#ChipDetailLContent").animate({
		        scrollTop: 0,
		        scrollLeft: 0
		    }, 'normal');
        	//2013/12/04 TMEJ 下村 タブレット版SMB チーフテクニシャン機能開発 END
        })
        .bind("paste", function (e) {
            setTimeout(function () {
                ControlLengthTextarea($("#DetailLOrderTxt"));
                AdjusterDetailTextArea($("#DetailLOrderTxt"), $("#DetailLOrderDt"));
            }, 0);
        })
        .bind("keyup", function () {
            ControlLengthTextarea($("#DetailLOrderTxt"));
            AdjusterDetailTextArea($("#DetailLOrderTxt"), $("#DetailLOrderDt"));
        })
        .bind("keydown", function () {
            ControlLengthTextarea($("#DetailLOrderTxt"));
        });

//2013/12/04 TMEJ 下村 タブレット版SMB チーフテクニシャン機能開発 START
//    //故障原因
//    $("#DetailLFailureTxt")
//        .click(function () {
//            //チップ詳細(大)のコントロールにフォーカスが当たらない不具合を解消する
//            $(this).focus();
//        })
//        .blur(function () {
//            $("#DetailSFailureTxt").val($("#DetailLFailureTxt").val());
//            ControlLengthTextarea($("#DetailLFailureTxt"));
//            AdjusterDetailTextArea($("#DetailLFailureTxt"), $("#DetailLFailureDt"));
//            //スクロール位置ずれ調整
//            //AdjustChipDetailDisplay();
//            $("#DetailLDummyBtn").focus();
//        })
//        .bind("paste", function (e) {
//            setTimeout(function () {
//                ControlLengthTextarea($("#DetailLFailureTxt"));
//                AdjusterDetailTextArea($("#DetailLFailureTxt"), $("#DetailLFailureDt"));
//            }, 0);
//        })
//        .bind("keyup", function () {
//            ControlLengthTextarea($("#DetailLFailureTxt"));
//            AdjusterDetailTextArea($("#DetailLFailureTxt"), $("#DetailLFailureDt"));
//        })
//        .bind("keydown", function () {
//            ControlLengthTextarea($("#DetailLFailureTxt"));
//        });

//    //診断結果
//    $("#DetailLResultTxt")
//        .click(function () {
//            //チップ詳細(大)のコントロールにフォーカスが当たらない不具合を解消する
//            $(this).focus();
//        })
//        .blur(function () {
//            $("#DetailSResultTxt").val($("#DetailLResultTxt").val());
//            ControlLengthTextarea($("#DetailLResultTxt"));
//            AdjusterDetailTextArea($("#DetailLResultTxt"), $("#DetailLResultDt"));
//            //スクロール位置ずれ調整
//            //AdjustChipDetailDisplay();
//            $("#DetailLDummyBtn").focus();
//        })
//        .bind("paste", function (e) {
//            setTimeout(function () {
//                ControlLengthTextarea($("#DetailLResultTxt"));
//                AdjusterDetailTextArea($("#DetailLResultTxt"), $("#DetailLResultDt"));
//            }, 0);
//        })
//        .bind("keyup", function () {
//            ControlLengthTextarea($("#DetailLResultTxt"));
//            AdjusterDetailTextArea($("#DetailLResultTxt"), $("#DetailLResultDt"));
//        })
//        .bind("keydown", function () {
//            ControlLengthTextarea($("#DetailLResultTxt"));
//        });

//    //アドバイス
//    $("#DetailLAdviceTxt")
//        .click(function () {
//            //チップ詳細(大)のコントロールにフォーカスが当たらない不具合を解消する
//            $(this).focus();
//        })
//        .blur(function () {
//            $("#DetailSAdviceTxt").val($("#DetailLAdviceTxt").val());
//            ControlLengthTextarea($("#DetailLAdviceTxt"));
//            AdjusterDetailTextArea($("#DetailLAdviceTxt"), $("#DetailLAdviceDt"));
//            //スクロール位置ずれ調整
//            //AdjustChipDetailDisplay();
//            $("#DetailLDummyBtn").focus();
//        })
//        .bind("paste", function (e) {
//            setTimeout(function () {
//                ControlLengthTextarea($("#DetailLAdviceTxt"));
//                AdjusterDetailTextArea($("#DetailLAdviceTxt"), $("#DetailLAdviceDt"));
//            }, 0);
//        })
//        .bind("keyup", function () {
//            ControlLengthTextarea($("#DetailLAdviceTxt"));
//            AdjusterDetailTextArea($("#DetailLAdviceTxt"), $("#DetailLAdviceDt"));
//        })
//        .bind("keydown", function () {
//            ControlLengthTextarea($("#DetailLAdviceTxt"));
//        });

        //メモ
        $("#DetailLMemoTxt")
        .click(function () {
            //チップ詳細(大)のコントロールにフォーカスが当たらない不具合を解消する
            $(this).focus();
        })
        .blur(function () {
            $("#DetailSMemoTxt").val($("#DetailLMemoTxt").val());
            ControlLengthTextarea($("#DetailLMemoTxt"));
            AdjusterDetailTextArea($("#DetailLMemoTxt"), $("#DetailLMemoDt"));
            //スクロール位置ずれ調整
            //AdjustChipDetailDisplay();
        })
        .bind("paste", function (e) {
            setTimeout(function () {
                ControlLengthTextarea($("#DetailLMemoTxt"));
                AdjusterDetailTextArea($("#DetailLMemoTxt"), $("#DetailLMemoDt"));
            }, 0);
        })
        .bind("keyup", function () {
            ControlLengthTextarea($("#DetailLMemoTxt"));
            AdjusterDetailTextArea($("#DetailLMemoTxt"), $("#DetailLMemoDt"));
        })
        .bind("keydown", function () {
            ControlLengthTextarea($("#DetailLMemoTxt"));
        });
 //2013/12/04 TMEJ 下村 タブレット版SMB チーフテクニシャン機能開発 END

} //SetDetailLTextEvent End

/**	
* 必須項目の空チェックを行う
* 	
*/
function IsMandatoryChipDetailEmpty() {

    var rtnVal = false;

    var checkTxt1 = $("#DetailSPlanStartLabel").text();
    var checkTxt2 = $("#DetailSPlanFinishLabel").text();

    //サブチップボックスならば数値が返却される
    var subAreaId = GetSubChipType(gSelectedChipId);

    //受付・追加作業エリアの場合
    if (subAreaId == C_FT_BTNTP_CONFIRMED_RO || subAreaId == C_FT_BTNTP_WAIT_CONFIRMEDADDWORK) {

        rtnVal = false;
    }
    else {
        //受付・追加作業エリア以外の場合

        if (checkTxt1 == "" || checkTxt2 == "") rtnVal = true;
    }

    return rtnVal;
}

/**	
* チップ詳細(小)のチップエリアのイベント設定を行う
* 	
*/
function SetEventDetailSChipArea() {

    //チップ詳細(小)のチップエリアタップイベントの登録
    //$("#detailSTableChipUl .Cassette").bind("touchstart", function (e) {
    $("#detailSTableChipUl .Cassette").bind("click", function (e) {

        var closestDl = $(e.target).closest('dl');
        var closestDiv = $(e.target).closest('div');
        var selectRowIndex = 0;
        var selectChipIndex = 0;
        var selectChipRezId = -1;
        var detailLChipTr;
        var selectTxt = "";

        //整備列の背景が灰色の場合、何もせず終了
        if (closestDl.find("dd").hasClass(C_SC3240201CLASS_BACKGROUNDGRAY)) {
            return;
        }

        //複数行表示した場合の、チップ列の背景が灰色の場合、何もせず終了
        if (closestDiv.hasClass(C_SC3240201CLASS_BACKGROUNDGRAY)) {
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
            closestDl.find("div").removeClass(C_SC3240201CLASS_CHECKBLUE);

            //複数行エリア内の選択したチップ情報にチェックをつける
            closestDiv.addClass(C_SC3240201CLASS_CHECKBLUE);

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
                //closestDl.find("dd").children(".SingleLine").addClass(C_SC3240201CLASS_FONTBOLD);
                closestDl.find("dd").children(".SingleLine").css("font-weight", "bold");
            }
            else {
                //他チップは細字
                //closestDl.find("dd").children(".SingleLine").addClass(C_SC3240201CLASS_FONTNORMAL);
                closestDl.find("dd").children(".SingleLine").css("font-weight", "normal");
            }

            //１行表示時の高さを設定
            closestDl.height(29);

            //閉じる状態のステータスに変更
            closestDl.attr("openFlg", "0");

            selectRowIndex = closestDl.attr("rowindex");
            selectChipIndex = closestDiv.attr("chipindex");

            //********** チップ詳細(大)に反映 Start **********
            detailLChipTr = $(".ChipDetailPopStyle .detailLTableChip2 #chipInfoTable tr:eq(" + selectRowIndex + ")");

            //行の保存用選択チップインデックスを更新
            detailLChipTr.attr("selectchipindex", selectChipIndex).attr("selectrezid", selectChipRezId);

            //trエリア配下にあるdivの青チェックを一度全て外す
            detailLChipTr.find("div").removeClass(C_SC3240201CLASS_CHECKBLUE);

            //該当チップのdivに青チェックをつける(未選択の場合、インデックスは0でeqの指定が負の値になるため、どこにも青チェックがつかない)
            detailLChipTr.find("div:eq(" + (selectChipIndex - 1) + ")").addClass(C_SC3240201CLASS_CHECKBLUE);
            //********** チップ詳細(大)に反映 End **********
        }
    });
} //SetEventDetailSChipArea End

/**	
* チップ詳細(大)のチップエリアのイベント設定を行う
* 	
*/
function SetEventDetailLChipArea() {

    //チップ詳細(大)のチップエリアタップイベントの登録
    $(".DetailLChip").bind("click", function (e) {

        var closestTr = $(e.target).closest('tr');
        var closestTd = $(e.target).closest('td');
        var checkDiv = $(e.target);
        var selectRowIndex = 0;
        var selectChipIndex = 0;
        var selectTxt = "";
        var selectChipRezId = -1;
        var detailSTableStallDl = $("#detailSTableChipUl dl");

        //背景が灰色の場合、何もせず終了（該当行）
        if (closestTr.hasClass(C_SC3240201CLASS_BACKGROUNDGRAY)) {
            return;
        }

        //背景が灰色の場合、何もせず終了（該当列）
        if (closestTd.hasClass(C_SC3240201CLASS_BACKGROUNDGRAY)) {
            return;
        }

        selectRowIndex = closestTr.attr("rowindex");

        //現在青チェックが入っている場合
        if (checkDiv.hasClass(C_SC3240201CLASS_CHECKBLUE)) {

            //青チェックをはずす
            checkDiv.removeClass(C_SC3240201CLASS_CHECKBLUE);

            //選択チップのインデックスは未選択(0)、予約IDを-1(未選択)にする
            closestTr.attr("selectChipIndex", "0").attr("selectrezid", "-1");

            //チップ詳細(小)にも反映
            $(detailSTableStallDl[selectRowIndex]).attr("selectChipIndex", "0").attr("selectrezid", "-1");

            //複数行エリアのチェックを一度全て外す
            $(detailSTableStallDl[selectRowIndex]).find("div").removeClass(C_SC3240201CLASS_CHECKBLUE);

            //複数行エリア内の選択したチップ情報にチェックをつける
            $(detailSTableStallDl[selectRowIndex]).find("div:last").addClass(C_SC3240201CLASS_CHECKBLUE);

            //未選択用の文言を１行表示用ラベルに設定
            selectTxt = $("#WordChipUnselectedHidden").val();
            $(detailSTableStallDl[selectRowIndex]).find("dd").children(".SingleLine").text(selectTxt);
        }
        //現在青チェックが入っていない場合
        else {
            //trエリア配下にあるdivの青チェックを一度全て外す
            closestTr.find("div").removeClass(C_SC3240201CLASS_CHECKBLUE);

            //選択したチップ情報に青チェックをつける
            checkDiv.addClass(C_SC3240201CLASS_CHECKBLUE);

            //選択チップのインデックスを保存する
            closestTr.attr("selectChipIndex", checkDiv.attr("chipindex"))
                     .attr("selectrezid", checkDiv.attr("rezid"));

            selectChipIndex = checkDiv.attr("chipindex");

            //********** チップ詳細(小)に反映 Start **********
            $(detailSTableStallDl[selectRowIndex]).attr("selectChipIndex", selectChipIndex).attr("selectrezid", checkDiv.attr("rezid"));

            //複数行エリアのチェックを一度全て外す
            $(detailSTableStallDl[selectRowIndex]).find("div").removeClass(C_SC3240201CLASS_CHECKBLUE);

            //複数行エリア内の選択したチップ情報にチェックをつける
            $(detailSTableStallDl[selectRowIndex]).find("div:eq(" + (selectChipIndex - 1) + ")").addClass(C_SC3240201CLASS_CHECKBLUE);

            //選択チップ名の文言を１行表示用ラベルに設定
            selectTxt = $(detailSTableStallDl[selectRowIndex]).find(".CheckBlue").children("span").text();
            $(detailSTableStallDl[selectRowIndex]).find("dd").children(".SingleLine").text(selectTxt);
            //********** チップ詳細(小)に反映 End **********
        }

        //********** チップ詳細(小)に反映 Start *********
        //選択されたチップの予約ID
        selectChipRezId = checkDiv.attr("rezid");

        //自チップの作業内容ID　＝　整備に紐付いている作業内容ID、もしくは未選択　の場合
        //if (($("#MyJobDtlIdHidden").val() == selectChipRezId) || (selectChipRezId == -1)) {
        if (($("#MyJobDtlIdHidden").val() == selectChipRezId) || (selectChipIndex == 0)) {
            //自チップは太字
            $(detailSTableStallDl[selectRowIndex]).find("dd").children(".SingleLine").css("font-weight", "bold");
        }
        else {
            //他チップは細字
            $(detailSTableStallDl[selectRowIndex]).find("dd").children(".SingleLine").css("font-weight", "normal");
        }
        //********** チップ詳細(小)に反映 End **********

    });
} //SetEventDetailLChipArea End

/**	
* チップ詳細(小・大)の予約有無エリアのイベント設定を行う
* 	
*/
function SetEventDetailReservationArea() {
    //チップ詳細(小)の「予約」クリック時のイベント登録
    $("#DetailSReserveLi dd:first").bind(C_SC3240201_TOUCH, function () {

        //青チェックを付け直す
        $("#DetailSReserveLi dd").removeClass(C_SC3240201CLASS_CHECKBLUE);
        $("#DetailSReserveLi dd:first").addClass(C_SC3240201CLASS_CHECKBLUE);

        //予約フラグを1(予約)にする
        $("#RezFlgHidden").val("0");

        //チップ詳細(大)に反映
        $("#DetailLReserveLi dd").removeClass(C_SC3240201CLASS_CHECKBLUE);
        $("#DetailLReserveLi dd:first").addClass(C_SC3240201CLASS_CHECKBLUE);
    });

    //チップ詳細(小)の「飛び込み」クリック時のイベント登録
    $("#DetailSReserveLi dd:last").bind(C_SC3240201_TOUCH, function () {

        //青チェックを付け直す
        $("#DetailSReserveLi dd").removeClass(C_SC3240201CLASS_CHECKBLUE);
        $("#DetailSReserveLi dd:last").addClass(C_SC3240201CLASS_CHECKBLUE);

        //予約フラグを0(飛び込み)にする
        $("#RezFlgHidden").val("1");

        //チップ詳細(大)に反映
        $("#DetailLReserveLi dd").removeClass(C_SC3240201CLASS_CHECKBLUE);
        $("#DetailLReserveLi dd:last").addClass(C_SC3240201CLASS_CHECKBLUE);
    });

    //チップ詳細(大)の「予約」クリック時のイベント登録
    $("#DetailLReserveLi dd:first").bind(C_SC3240201_TOUCH, function () {

        //青チェックを付け直す
        $("#DetailLReserveLi dd").removeClass(C_SC3240201CLASS_CHECKBLUE);
        $("#DetailLReserveLi dd:first").addClass(C_SC3240201CLASS_CHECKBLUE);

        //予約フラグを1(予約)にする
        $("#RezFlgHidden").val("0");

        //チップ詳細(小)に反映
        $("#DetailSReserveLi dd").removeClass(C_SC3240201CLASS_CHECKBLUE);
        $("#DetailSReserveLi dd:first").addClass(C_SC3240201CLASS_CHECKBLUE);
    });

    //チップ詳細(大)の「飛び込み」クリック時のイベント登録
    $("#DetailLReserveLi dd:last").bind(C_SC3240201_TOUCH, function () {

        //青チェックを付け直す
        $("#DetailLReserveLi dd").removeClass(C_SC3240201CLASS_CHECKBLUE);
        $("#DetailLReserveLi dd:last").addClass(C_SC3240201CLASS_CHECKBLUE);

        //予約フラグを0(飛び込み)にする
        $("#RezFlgHidden").val("1");

        //チップ詳細(小)に反映
        $("#DetailSReserveLi dd").removeClass(C_SC3240201CLASS_CHECKBLUE);
        $("#DetailSReserveLi dd:last").addClass(C_SC3240201CLASS_CHECKBLUE);
    });
}

//2013/12/04 TMEJ 下村 タブレット版SMB チーフテクニシャン機能開発 START
/**	
* チップ詳細(小・大)の完成検査有無エリアのイベント設定を行う
* 	
*/
function SetEventDetailCompleteExaminationArea() {
    //チップ詳細(小)の「有り」クリック時のイベント登録
    $("#DetailSCompleteExaminationLi dd:first").bind(C_SC3240201_TOUCH, function () {

        //青チェックを付け直す
        $("#DetailSCompleteExaminationLi dd").removeClass(C_SC3240201CLASS_CHECKBLUE);
        $("#DetailSCompleteExaminationLi dd:first").addClass(C_SC3240201CLASS_CHECKBLUE);

        //完成検査フラグを1(有り)にする
        $("#CompleteExaminationFlgHidden").val("1");

        //チップ詳細(大)に反映
        $("#DetailLCompleteExaminationLi dd").removeClass(C_SC3240201CLASS_CHECKBLUE);
        $("#DetailLCompleteExaminationLi dd:first").addClass(C_SC3240201CLASS_CHECKBLUE);

    });

    //チップ詳細(小)の「無し」クリック時のイベント登録
    $("#DetailSCompleteExaminationLi dd:last").bind(C_SC3240201_TOUCH, function () {

        //青チェックを付け直す
        $("#DetailSCompleteExaminationLi dd").removeClass(C_SC3240201CLASS_CHECKBLUE);
        $("#DetailSCompleteExaminationLi dd:last").addClass(C_SC3240201CLASS_CHECKBLUE);

        //完成検査フラグを0(無し)にする
        $("#CompleteExaminationFlgHidden").val("0");

        //チップ詳細(大)に反映
        $("#DetailLCompleteExaminationLi dd").removeClass(C_SC3240201CLASS_CHECKBLUE);
        $("#DetailLCompleteExaminationLi dd:last").addClass(C_SC3240201CLASS_CHECKBLUE);

    });

    //チップ詳細(大)の「有り」クリック時のイベント登録
    $("#DetailLCompleteExaminationLi dd:first").bind(C_SC3240201_TOUCH, function () {

        //青チェックを付け直す
        $("#DetailLCompleteExaminationLi dd").removeClass(C_SC3240201CLASS_CHECKBLUE);
        $("#DetailLCompleteExaminationLi dd:first").addClass(C_SC3240201CLASS_CHECKBLUE);

        //完成検査フラグを1(有り)にする
        $("#CompleteExaminationFlgHidden").val("1");

        //チップ詳細(小)に反映
        $("#DetailSCompleteExaminationLi dd").removeClass(C_SC3240201CLASS_CHECKBLUE);
        $("#DetailSCompleteExaminationLi dd:first").addClass(C_SC3240201CLASS_CHECKBLUE);

    });

    //チップ詳細(大)の「無し」クリック時のイベント登録
    $("#DetailLCompleteExaminationLi dd:last").bind(C_SC3240201_TOUCH, function () {

        //青チェックを付け直す
        $("#DetailLCompleteExaminationLi dd").removeClass(C_SC3240201CLASS_CHECKBLUE);
        $("#DetailLCompleteExaminationLi dd:last").addClass(C_SC3240201CLASS_CHECKBLUE);

        //完成検査フラグを0(無し)にする
        $("#CompleteExaminationFlgHidden").val("0");

        //チップ詳細(小)に反映
        $("#DetailSCompleteExaminationLi dd").removeClass(C_SC3240201CLASS_CHECKBLUE);
        $("#DetailSCompleteExaminationLi dd:last").addClass(C_SC3240201CLASS_CHECKBLUE);

    });
}
//2013/12/04 TMEJ 下村 タブレット版SMB チーフテクニシャン機能開発 END

/**	
* チップ詳細(小・大)の洗車有無エリアのイベント設定を行う
* 	
*/
function SetEventDetailCarWashArea() {
    //チップ詳細(小)の「有り」クリック時のイベント登録
    $("#DetailSCarWashLi dd:first").bind(C_SC3240201_TOUCH, function () {

        //青チェックを付け直す
        $("#DetailSCarWashLi dd").removeClass(C_SC3240201CLASS_CHECKBLUE);
        $("#DetailSCarWashLi dd:first").addClass(C_SC3240201CLASS_CHECKBLUE);

        //洗車フラグを1(有り)にする
        $("#CarWashFlgHidden").val("1");

        //チップ詳細(大)に反映
        $("#DetailLCarWashLi dd").removeClass(C_SC3240201CLASS_CHECKBLUE);
        $("#DetailLCarWashLi dd:first").addClass(C_SC3240201CLASS_CHECKBLUE);

    });

    //チップ詳細(小)の「無し」クリック時のイベント登録
    $("#DetailSCarWashLi dd:last").bind(C_SC3240201_TOUCH, function () {

        //青チェックを付け直す
        $("#DetailSCarWashLi dd").removeClass(C_SC3240201CLASS_CHECKBLUE);
        $("#DetailSCarWashLi dd:last").addClass(C_SC3240201CLASS_CHECKBLUE);

        //洗車フラグを0(無し)にする
        $("#CarWashFlgHidden").val("0");

        //チップ詳細(大)に反映
        $("#DetailLCarWashLi dd").removeClass(C_SC3240201CLASS_CHECKBLUE);
        $("#DetailLCarWashLi dd:last").addClass(C_SC3240201CLASS_CHECKBLUE);

    });

    //チップ詳細(大)の「有り」クリック時のイベント登録
    $("#DetailLCarWashLi dd:first").bind(C_SC3240201_TOUCH, function () {

        //青チェックを付け直す
        $("#DetailLCarWashLi dd").removeClass(C_SC3240201CLASS_CHECKBLUE);
        $("#DetailLCarWashLi dd:first").addClass(C_SC3240201CLASS_CHECKBLUE);

        //洗車フラグを1(有り)にする
        $("#CarWashFlgHidden").val("1");

        //チップ詳細(小)に反映
        $("#DetailSCarWashLi dd").removeClass(C_SC3240201CLASS_CHECKBLUE);
        $("#DetailSCarWashLi dd:first").addClass(C_SC3240201CLASS_CHECKBLUE);

    });

    //チップ詳細(大)の「無し」クリック時のイベント登録
    $("#DetailLCarWashLi dd:last").bind(C_SC3240201_TOUCH, function () {

        //青チェックを付け直す
        $("#DetailLCarWashLi dd").removeClass(C_SC3240201CLASS_CHECKBLUE);
        $("#DetailLCarWashLi dd:last").addClass(C_SC3240201CLASS_CHECKBLUE);

        //洗車フラグを0(無し)にする
        $("#CarWashFlgHidden").val("0");

        //チップ詳細(小)に反映
        $("#DetailSCarWashLi dd").removeClass(C_SC3240201CLASS_CHECKBLUE);
        $("#DetailSCarWashLi dd:last").addClass(C_SC3240201CLASS_CHECKBLUE);

    });
}

/**	
* チップ詳細(小・大)の待ち方エリアのイベント設定を行う
* 	
*/
function SetEventDetailWaitingArea() {
    //チップ詳細(小)の「店内」クリック時のイベント登録
    $("#DetailSWaitingLi dd:first").bind(C_SC3240201_TOUCH, function () {

        //青チェックを付け直す
        $("#DetailSWaitingLi dd").removeClass(C_SC3240201CLASS_CHECKBLUE);
        $("#DetailSWaitingLi dd:first").addClass(C_SC3240201CLASS_CHECKBLUE);

        //待ち方フラグを0(店内)にする
        $("#WaitingFlgHidden").val("0");

        //チップ詳細(大)に反映
        $("#DetailLWaitingLi dd").removeClass(C_SC3240201CLASS_CHECKBLUE);
        $("#DetailLWaitingLi dd:first").addClass(C_SC3240201CLASS_CHECKBLUE);

    });

    //チップ詳細(小)の「店外」クリック時のイベント登録
    $("#DetailSWaitingLi dd:last").bind(C_SC3240201_TOUCH, function () {

        //青チェックを付け直す
        $("#DetailSWaitingLi dd").removeClass(C_SC3240201CLASS_CHECKBLUE);
        $("#DetailSWaitingLi dd:last").addClass(C_SC3240201CLASS_CHECKBLUE);

        //待ち方フラグを4(店外)にする
        $("#WaitingFlgHidden").val("4");

        //チップ詳細(大)に反映
        $("#DetailLWaitingLi dd").removeClass(C_SC3240201CLASS_CHECKBLUE);
        $("#DetailLWaitingLi dd:last").addClass(C_SC3240201CLASS_CHECKBLUE);

    });

    //チップ詳細(大)の「店内」クリック時のイベント登録
    $("#DetailLWaitingLi dd:first").bind(C_SC3240201_TOUCH, function () {

        //青チェックを付け直す
        $("#DetailLWaitingLi dd").removeClass(C_SC3240201CLASS_CHECKBLUE);
        $("#DetailLWaitingLi dd:first").addClass(C_SC3240201CLASS_CHECKBLUE);

        //待ち方フラグを0(店内)にする
        $("#WaitingFlgHidden").val("0");

        //チップ詳細(小)に反映
        $("#DetailSWaitingLi dd").removeClass(C_SC3240201CLASS_CHECKBLUE);
        $("#DetailSWaitingLi dd:first").addClass(C_SC3240201CLASS_CHECKBLUE);

    });

    //チップ詳細(大)の「店外」クリック時のイベント登録
    $("#DetailLWaitingLi dd:last").bind(C_SC3240201_TOUCH, function () {

        //青チェックを付け直す
        $("#DetailLWaitingLi dd").removeClass(C_SC3240201CLASS_CHECKBLUE);
        $("#DetailLWaitingLi dd:last").addClass(C_SC3240201CLASS_CHECKBLUE);

        //待ち方フラグを1(店外)にする
        $("#WaitingFlgHidden").val("4");

        //チップ詳細(小)に反映
        $("#DetailSWaitingLi dd").removeClass(C_SC3240201CLASS_CHECKBLUE);
        $("#DetailSWaitingLi dd:last").addClass(C_SC3240201CLASS_CHECKBLUE);

    });
}

/**	
* 整備種類・整備名称の変更時、標準作業時間の連動処理を行う
* 	
*/
function SetDetailSL_StandardWorkTime(standardWorkTime) {

    //作業時間が編集不可の場合は何もしない
    if ($("#DetailSWorkTimeTxt").hasClass(C_SC3240201CLASS_TEXTBLACK) == true) {
        return false;
    }

    var min;

    //標準作業時間が空白、もしくは0の場合
    if (standardWorkTime == "" || standardWorkTime == "0") {

        //ストールのインターバル時間をセット
        min = gResizeInterval;
    }
    //それ以外は、標準作業時間をストールのインターバル時間単位で丸め込む
    else {
        min = smbScript.RoundUpToNumUnits(standardWorkTime, gResizeInterval, gResizeInterval, C_SC3240201_MAXWORKTIME);
    }

    $("#DetailSWorkTimeTxt").val(min);
    $("#DetailLWorkTimeTxt").val(min);
    $("#DetailSWorkTimeLabel").text(min + $("#WordWorkTimeUnitHidden").val());
    $("#DetailLWorkTimeLabel").text(min + $("#WordWorkTimeUnitHidden").val());

    //作業開始日時がnullでない場合、設定した作業時間に合わせて作業終了予定時間を変更する
    //2013/12/26 TMEJ 下村 次世代e-CRBタブレット(サービス) iOS7.0 VersionUp対応 START
    //var startDateTime = $("#DetailSPlanStartDateTimeSelector").get(0).valueAsDate;
    var startDateTime = smbScript.changeStringToDateIcrop($("#DetailSPlanStartDateTimeSelector").get(0).value);
    //2013/12/26 TMEJ 下村 次世代e-CRBタブレット(サービス) iOS7.0 VersionUp対応 END

    if (startDateTime != null) {

        //開始日時と加算時間を元に、終了日時を計算する（営業時間外の時間帯は除外して計算）
        var newEndDateTime = smbScript.CalcEndDate_ExcludeOutOfTime(startDateTime, min);

        //2013/12/26 TMEJ 下村 次世代e-CRBタブレット(サービス) iOS7.0 VersionUp対応 START
        //$("#DetailSPlanFinishDateTimeSelector").get(0).valueAsDate = newEndDateTime;
        $("#DetailSPlanFinishDateTimeSelector").get(0).value = smbScript.getDateTimelocalDate(newEndDateTime);
        //2013/12/26 TMEJ 下村 次世代e-CRBタブレット(サービス) iOS7.0 VersionUp対応 END

        $("#DetailSPlanFinishLabel").text(smbScript.ConvertDateOrTime(newEndDateTime, $("#hidShowDate").val()));

        //チップ詳細(大)に反映
        //2013/12/26 TMEJ 下村 次世代e-CRBタブレット(サービス) iOS7.0 VersionUp対応 START
        //$("#DetailLPlanFinishDateTimeSelector").get(0).valueAsDate = newEndDateTime;
        $("#DetailLPlanFinishDateTimeSelector").get(0).value = smbScript.getDateTimelocalDate(newEndDateTime);
        //2013/12/26 TMEJ 下村 次世代e-CRBタブレット(サービス) iOS7.0 VersionUp対応 END

        $("#DetailLPlanFinishLabel").text($("#DetailSPlanFinishLabel").text());

        //必須項目がEmptyなら登録ボタンを非活性にする
        $("#DetailRegisterBtn").attr("disabled", IsMandatoryChipDetailEmpty());
    }

    return false;
}

//2013/12/04 TMEJ 下村 タブレット版SMB チーフテクニシャン機能開発 START
/**	
* 整備種類の変更時、完成検査有無の連動処理を行う
* 	
*/
function SetDetailSL_CompleteExaminationArea(svcClassType) {

	if($("#DetailSCompleteExaminationLi dd").hasClass(C_SC3240201CLASS_TEXTBLACK) == false){
		if(svcClassType.trim() != ""){
		    //サービス分類区分が「1:EM」または、「2:PM」の場合
			if ((svcClassType == "1") || (svcClassType == "2")){
		        //青チェックを付け直す
		        $("#DetailSCompleteExaminationLi dd").removeClass(C_SC3240201CLASS_CHECKBLUE);
		        $("#DetailSCompleteExaminationLi dd:last").addClass(C_SC3240201CLASS_CHECKBLUE);
		        $("#DetailLCompleteExaminationLi dd").removeClass(C_SC3240201CLASS_CHECKBLUE);
		        $("#DetailLCompleteExaminationLi dd:last").addClass(C_SC3240201CLASS_CHECKBLUE);
		    	
		        //完成検査フラグを0(無し)にする
		        $("#CompleteExaminationFlgHidden").val("0");
		    }
		    else {
		        //青チェックを付け直す
		        $("#DetailSCompleteExaminationLi dd").removeClass(C_SC3240201CLASS_CHECKBLUE);
		        $("#DetailSCompleteExaminationLi dd:first").addClass(C_SC3240201CLASS_CHECKBLUE);
		        $("#DetailLCompleteExaminationLi dd").removeClass(C_SC3240201CLASS_CHECKBLUE);
		        $("#DetailLCompleteExaminationLi dd:first").addClass(C_SC3240201CLASS_CHECKBLUE);
		    	
		        //完成検査フラグを1(有り)にする
		        $("#CompleteExaminationFlgHidden").val("1");
		    }
		}
	}

    return false;
}

/**	
* 整備種類の変更時、洗車有無の連動処理を行う
* 	
*/
function SetDetailSL_CarWashArea(carWashNeedFlg) {

	if($("#DetailSCarWashLi dd").hasClass(C_SC3240201CLASS_TEXTBLACK) == false){
		if(carWashNeedFlg.trim() != ""){
			//洗車必要フラグが「1」の場合
		    if (carWashNeedFlg == "1") {
		        //青チェックを付け直す
		        $("#DetailSCarWashLi dd").removeClass(C_SC3240201CLASS_CHECKBLUE);
		        $("#DetailSCarWashLi dd:first").addClass(C_SC3240201CLASS_CHECKBLUE);
		        $("#DetailLCarWashLi dd").removeClass(C_SC3240201CLASS_CHECKBLUE);
		        $("#DetailLCarWashLi dd:first").addClass(C_SC3240201CLASS_CHECKBLUE);

		        //洗車フラグを1(有り)にする
		        $("#CarWashFlgHidden").val("1");
		    }
		    else {
		        //青チェックを付け直す
		        $("#DetailSCarWashLi dd").removeClass(C_SC3240201CLASS_CHECKBLUE);
		        $("#DetailSCarWashLi dd:last").addClass(C_SC3240201CLASS_CHECKBLUE);
		        $("#DetailLCarWashLi dd").removeClass(C_SC3240201CLASS_CHECKBLUE);
		        $("#DetailLCarWashLi dd:last").addClass(C_SC3240201CLASS_CHECKBLUE);

		        //洗車フラグを0(無し)にする
		        $("#CarWashFlgHidden").val("0");
		    }
		}
	}

    return false;
}
//2013/12/04 TMEJ 下村 タブレット版SMB チーフテクニシャン機能開発 END

///**	
//* チップ詳細ポップアップのクローズ監視イベント設定を行う
//* 	
//*/
//function SetEventChipDetailClose() {
//    //ポップアップクローズの監視
//    $(document.body).bind(C_SC3240201_TOUCH, ObserveChipDetailClose);
//}

///**	
//* チップ詳細ポップアップのクローズ監視処理
//* 	
//*/
//function ObserveChipDetailClose(event) {
//    if ($("#ChipDetailPopup").is(":visible") === false) return;
//    //タップ領域がチップ詳細の領域内・オーバーレイ・ラベルのツールチップ以外の場合
//    if ($(event.target).is("#ChipDetailPopup, #ChipDetailPopup *, #MstPG_registOverlayBlack, .icrop-CustomLabel-tooltip") === false) {
//        //チップ詳細を閉じる
//        CloseChipDetail();
//    }
//}

/**	
* マルチテキストの高さが自動で広がって行った後、スクロール位置がずれる現象をなくすための調整処理
* 	
*/
//function AdjustChipDetailDisplay() {
//    $("#ChipDetailPopup").css("display", "inline-block");
//    setTimeout(function () {
//        $("#ChipDetailPopup").css("display", "block");
//    }, 0);
//}

// 2020/01/08 NSK 鈴木 TR-V4-BMS-20191001-001 (トライ店システム評価)コールセンター業務における洗車要否判断の効率化検証 START
/**
* 洗車必要フラグが変更可能か判定する
* 
* @return {boolean} true：変更可能、false：変更不可
* 
*/
function canChangeCarwashNeedFlg() {

    // 戻り値
    var canChange = true;

    // 自分に紐づく関連チップ取得
    var arrRelationChips = FindRelationChips("", $('#ChipDetail_ServiceInIDHidden').val());

    // 関連チップが2件以上で、洗車必要フラグが必要の場合、変更不可とする。
    if ((1 < arrRelationChips.length) && ("1" == $('#CarWashFlgHidden').val())) {
        canChange = false;
    }

    return canChange;
}
// 2020/01/08 NSK 鈴木 TR-V4-BMS-20191001-001 (トライ店システム評価)コールセンター業務における洗車要否判断の効率化検証 END
