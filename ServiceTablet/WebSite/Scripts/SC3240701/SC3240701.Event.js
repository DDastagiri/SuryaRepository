//---------------------------------------------------------
//SC3240701.Event.js
//---------------------------------------------------------
//機能：ストール使用不可画面のイベント定義
//作成：2017/08/30 NSK 竹中(悠) REQ-SVT-TMT-20161109-001 SMB iPadにストールクローズ機能の追加
//更新：
//---------------------------------------------------------

/**	
*  使用不可画面のテキストイベントの設定を行う
*　更新：
*/

function SetUnavailableSettingTextEvent() {

    $("#StartIdleDateTimeSelector")
        .blur(function () {
            //使用不可開始日時がnullでない場合
            if ($(this).get(0).value != null && $(this).get(0).value != "") {
                //使用不可開始日時を5分単位に切り上げた日時を計算する
                var newDate = new Date(smbScript.RoundUpTimeTo5Units(smbScript.changeStringToDateIcrop($(this).get(0).value)));

                //DateTimSelectorに使用不可開始日時を設定しなおす
                $(this).get(0).value = smbScript.getDateTimelocalDate(newDate);

                $("#StartIdleDateTimeLabel").text(smbScript.ConvertDateToStringForDisplay(newDate));

                //使用不可終了日時がnullでない場合
                if ($("#FinishIdleDateTimeSelector").get(0).value != null && $("#FinishIdleDateTimeSelector").get(0).value != "") {

                    //開始日時と終了日時から差分時間を計算し、作業時間に設定する　※営業時間外の時間帯は減算して計算
                    var timeSpan = smbScript.CalcTimeSpan_ExcludeOutOfTime(smbScript.changeStringToDateIcrop($(this).get(0).value), smbScript.changeStringToDateIcrop($("#FinishIdleDateTimeSelector").get(0).value));

                    if (timeSpan != null) {
                        //最大値を超える場合、使用不可時間の最大値（分）をセット
                        if (timeSpan > C_SC3240701_MAXWORKTIME) {
                            timeSpan = C_SC3240701_MAXWORKTIME;

                            //開始日時と加算時間を元に、終了日時を計算する（営業時間外の時間帯は除外して計算）
                            var newEndDateTime = smbScript.CalcEndDate_ExcludeOutOfTime(smbScript.changeStringToDateIcrop($("#StartIdleDateTimeSelector").get(0).value), C_SC3240701_MAXWORKTIME);

                            $("#FinishIdleDateTimeSelector").get(0).value = smbScript.getDateTimelocalDate(newEndDateTime);
                            $("#FinishIdleDateTimeLabel").text(smbScript.ConvertDateToStringForDisplay(newEndDateTime));
                        }
                        $("#UnavailableWorkTimeHidden").val(timeSpan);
                    }
                }
            }
            else {
                //ラベルをEmptyにする
                $("#StartIdleDateTimeLabel").text("");
            }
            //登録ボタンの活性・非活性
            $("#UnavailableRegisterBtn").attr("disabled", IsMandatoryUnavailableSettingEmpty());
        });

    //使用不可終了日時
    $("#FinishIdleDateTimeSelector")
        .blur(function () {
            //使用不可終了日時がnullでない場合
            if ($(this).get(0).value != null && $(this).get(0).value != "") {
                //使用不可終了日時を5分単位に切り上げた日時を計算する
                var newDate = new Date(smbScript.RoundUpTimeTo5Units(smbScript.changeStringToDateIcrop($(this).get(0).value)));

                //DateTimSelectorに使用不可終了日時を設定しなおす
                $(this).get(0).value = smbScript.getDateTimelocalDate(newDate);

                //DateTimeSelectorで選択した日時を表示用に変換してラベルに設定
                $("#FinishIdleDateTimeLabel").text(smbScript.ConvertDateToStringForDisplay(newDate));

                //使用不可開始日時がnullでない場合
                if ($("#StartIdleDateTimeSelector").get(0).value != null && $("#StartIdleDateTimeSelector").get(0).value != "") {
                    //開始日時と終了日時から差分時間を計算し、作業時間に設定する　※営業時間外の時間帯は減算して計算
                    var timeSpan = smbScript.CalcTimeSpan_ExcludeOutOfTime(smbScript.changeStringToDateIcrop($("#StartIdleDateTimeSelector").get(0).value), smbScript.changeStringToDateIcrop($(this).get(0).value));

                    if (timeSpan != null) {
                        //最大値を超える場合、使用不可時間の最大値（分）をセット
                        if (timeSpan > C_SC3240701_MAXWORKTIME) {
                            timeSpan = C_SC3240701_MAXWORKTIME;

                            //開始日時と加算時間を元に、終了日時を計算する（営業時間外の時間帯は除外して計算）
                            var newEndDateTime = smbScript.CalcEndDate_ExcludeOutOfTime(smbScript.changeStringToDateIcrop($("#StartIdleDateTimeSelector").get(0).value), C_SC3240701_MAXWORKTIME);

                            $("#FinishIdleDateTimeSelector").get(0).value = smbScript.getDateTimelocalDate(newEndDateTime);

                            $("#FinishIdleDateTimeLabel").text(smbScript.ConvertDateToStringForDisplay(newEndDateTime));
                        }
                        $("#IdleTimeHidden").val(timeSpan);
                    }
                }
               
            }
            else {
                //ラベルをEmptyにする
                $("#FinishIdleDateTimeLabel").text("");

            }
            //登録ボタンの活性・非活性
            $("#UnavailableRegisterBtn").attr("disabled", IsMandatoryUnavailableSettingEmpty());
        });

    //メモ欄
    $("#IdleMemoTxt")
        .click(function () {
            //フォーカスを当てる
            $(this).focus();
        })

        .blur(function () {
            //メモの領域外タップイベント
            ControlLengthTextarea($("#IdleMemoTxt"));
            UnavailableTextArea($("#IdleMemoTxt"), $("#IdleMemoDt"));
            //スクロール位置ずれ調整
            $("#UnavailableSettingDetailContent").animate({
                scrollTop: 0,
                scrollLeft: 0
            }, 'normal');
        })

        .bind("paste", function (e) {
            //貼り付け時イベント
            setTimeout(function () {
                ControlLengthTextarea($("#IdleMemoTxt"));
                UnavailableTextArea($("#IdleMemoTxt"), $("#IdleMemoDt"));
            }, 0);
        })

        .bind("keyup", function () {
            //入力後イベント
            ControlLengthTextarea($("#IdleMemoTxt"));
            UnavailableTextArea($("#IdleMemoTxt"), $("#IdleMemoDt"));
        })

        .bind("keydown", function () {
            //入力中イベント
            ControlLengthTextarea($("#IdleMemoTxt"));
        });
}


