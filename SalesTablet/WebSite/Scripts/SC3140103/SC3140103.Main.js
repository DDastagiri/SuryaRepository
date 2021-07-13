//------------------------------------------------------------------------------
//SC3140103.Main.js
//------------------------------------------------------------------------------
//機能：メインメニュー（SA）_javascript
//補足：
//作成：2012/01/16 KN 森下
//更新：
//------------------------------------------------------------------------------

// 定数
var TOUCH_START = "mousedown touchstart";
var TOUCH_MOVE = "touchmove mousemove";
var TOUCH_END = "touchend mouseup";
var DBL_TAP_INTERVAL = 200;

//カウンター対応
var MAX_PROC_TIME = 100 * 60000;
var MIN_PROC_TIME = -100 * 60000;

//現在選択中のチップ
var nowSelectArea;
var detailsArea = 0;
var detailsVisitNo = 0;
var detailsOrderNo = '';
var detailsApprovalId = '';

// -------------------------------------------------------------
// 独自イベント定義
// -------------------------------------------------------------
// チップタップイベント
// ・ドラッグ時は動作しない
// ・ダブルタップ時間内に再度タップがあれば動作しない
jQuery.event.special.chipTap = {
    setup: (function () {
        return function () {

            var touchStart = false;
            var touchMove = false;
            var singleTap = false;

            $(this).bind(TOUCH_START, function (event) {

                touchStart = true;
                touchMove = false;
                singleTap = !singleTap;

            });

            $(this).bind(TOUCH_MOVE, function (event) {

                if (!touchStart) {
                    return;
                }

                touchMove = true;
                //タッチムーブ後にダブルタップした際、chipTap処理をしないよう制御
                singleTap = false;
            });

            $(this).bind(TOUCH_END, function (event) {

                if (!touchStart) {
                    return;
                }

                if (touchMove) {
                    return;
                }

                touchStart = false;
                touchMove = false;

                var obj = $(this);

                setTimeout(
                    function () {
                        if (singleTap) {
                            obj.trigger("chipTap");
                            singleTap = false;
                        }
                    }
                    , DBL_TAP_INTERVAL
                );
            });

        }
    })()
}

// チップダブルタップ
jQuery.event.special.chipDblTap = {
    setup: (function () {
        return function () {

            var flag = false;

            $(this).bind(TOUCH_END, function (event) {

                if (flag) {
                    $(this).trigger("chipDblTap");
                    flag = false;
                }
                else {
                    flag = true;
                }

                setTimeout(
            function () {
                flag = false;
            }
            , DBL_TAP_INTERVAL
        );
            });

        }
    })()
}

// -------------------------------------------------------------
// メイン処理
// -------------------------------------------------------------
//DOMロード時の処理
$(function () {

    //受付エリア
    $('#flickable1').flickable({ dragStart: function () {
        FlickChip(nowSelectArea);
    }
    });
    //追加承認エリア
    $('#flickable2').flickable({ dragStart: function () {
        FlickChip(nowSelectArea);
    }
    });
    //納車準備エリア
    $('#flickable3').flickable({ dragStart: function () {
        FlickChip(nowSelectArea);
    }
    });
    //納車作業エリア
    $('#flickable4').flickable({ dragStart: function () {
        FlickChip(nowSelectArea);
    }
    });
    //作業中エリア
    $('#flickable5').flickable({ dragStart: function () {
        FlickChip(nowSelectArea);
    }
    });

    // ポップオーバーの設定
    SetPoover()

    // チップ選択クリア
    ClearChip()

    //フッターアプリの起動設定
    SetFutterApplication();

    // UpdatePanel処理前後イベント
    $(document).ready(function () {
        var prm = Sys.WebForms.PageRequestManager.getInstance();

        // 開始時のイベント
        prm.add_beginRequest(function () {
        });
        // 終了時のイベント
        prm.add_endRequest(EndRequest);
        function EndRequest(sender, args) {
            // 呼び出し元判定
            // 2012/02/23 KN 森下【SERVICE_1】START
            var id = '';
            if (sender._postBackSettings.sourceElement != null) {
                id = sender._postBackSettings.sourceElement.id;
            }
            // 2012/02/23 KN 森下【SERVICE_1】END
            switch (id) {
                case 'MainPolling':         // 更新
                    //受付エリア
                    $('#flickable1').flickable({ dragStart: function () {
                        FlickChip(nowSelectArea);
                    }
                    });
                    //追加承認エリア
                    $('#flickable2').flickable({ dragStart: function () {
                        FlickChip(nowSelectArea);
                    }
                    });
                    //納車準備エリア
                    $('#flickable3').flickable({ dragStart: function () {
                        FlickChip(nowSelectArea);
                    }
                    });
                    //納車作業エリア
                    $('#flickable4').flickable({ dragStart: function () {
                        FlickChip(nowSelectArea);
                    }
                    });
                    //作業中エリア
                    $('#flickable5').flickable({ dragStart: function () {
                        FlickChip(nowSelectArea);
                    }
                    });

                    // ポップオーバーの再設定
                    SetPoover()

                    // チップ選択クリア
                    ClearChip()

                    // 2012/02/22 KN 森下【SERVICE_1】START
                    // 工程管理ボックスの読み込み中アイコン停止
                    $('#loadingSchedule').attr("style", "visibility: hidden");
                    // 2012/02/22 KN 森下【SERVICE_1】END
                    break;
                case 'DetailPopupButton':   // チップ詳細
                default:
                    break;
            };
        }
    });
    // 2012/02/22 KN 森下【SERVICE_1】START
    // 工程管理ボックスの読み込み中アイコン停止
    $('#loadingSchedule').attr("style", "visibility: hidden");
    // 2012/02/22 KN 森下【SERVICE_1】END
});

// ポップオーバーの設定
function SetPoover() {

    var chipVisit = $('.CustomerChipRight, .CustomerChipLeft, .CustomerChipTop');

    // タップイベント処理
    chipVisit.bind('chipTap', function () {
        // チップ選択(解除あり)
        SetChipCheck(this);
    });
    // ダブルタップイベント処理
    chipVisit.bind('chipDblTap', function () {
        // チップ選択(解除なし)
        SetChip(this);
        // 2012/02/22 KN 森下【SERVICE_1】START
        // 読み込み中アイコン表示（非表示はサーバー側で行う）
        $('#IconLoadingPopup').attr("style", "visibility: visible");
        // 2012/02/22 KN 森下【SERVICE_1】END
        // 前回表示したポップオーバーの内容クリア
        PopupDataClear();
        // ポップオーバー表示
        $(this).trigger('showPopover');
        // ポップオーバー表示チェック
        if ($(this).popoverEx.openedPopup) {
            // チップ詳細表示
            SetDetail(this);
            // ポップオーバー表示
            $("#DetailPopupButton").click(); 
        }
    });

    // ポップオーバーの設定
    $('.CustomerChipRight').popoverEx({ contentId: '#CustomerPopOver', preventLeft: true, preventRight: false, preventTop: true, preventBottom: true });
    $('.CustomerChipLeft').popoverEx({ contentId: '#CustomerPopOver', preventLeft: false, preventRight: true, preventTop: true, preventBottom: true });
    $('.CustomerChipTop').popoverEx({ contentId: '#CustomerPopOver', preventLeft: true, preventRight: true, preventTop: false, preventBottom: true });
}

// チップ選択チェック
function SetChipCheck(chip) {
    // チップ詳細設定
    SetDetail(chip);
    // 現在選択中チップ取得
    var area = $('#DetailsArea').val();
    var visitNo = $('#DetailsVisitNo').val();
    var orderNo = $('#DetailsOrderNo').val();
    var approvalId = $('#DetailsApprovalId').val();

    // 選択チップチェック
    if (detailsArea == area && detailsVisitNo == visitNo) {
        // 選択中チップの選択
        UnsetChip(chip);
    } else {
        // 選択外チップの選択
        SetChip(chip);
        // ポップオーバー消失
        $(chip).trigger('hidePopover');
    }
}

// チップ選択
function SetChip(chip) {
    // 選択状態のチップがあれば選択解除
    $(nowSelectArea).removeClass('selectArea');
    // 選択されたチップの情報保持
    nowSelectArea = $(chip).children(':first');
    // 選択されたチップを選択状態にする
    $(nowSelectArea).addClass('selectArea');
    // チップ詳細設定
    SetDetail(chip);
    $('#DetailsArea').val(detailsArea);
    $('#DetailsVisitNo').val(detailsVisitNo);
    $('#DetailsOrderNo').val(detailsOrderNo);
    $('#DetailsApprovalId').val(detailsApprovalId);
}

// チップ選択解除
function UnsetChip(chip) {
    // 選択状態されたチップの選択解除
    $(nowSelectArea).removeClass('selectArea');
    // 選択されたチップの情報解除
    nowSelectArea = null;
    // チップ詳細設定解除
    ClearChip();
    // ポップオーバー消失
    $(chip).trigger('hidePopover');
}

// チップ選択クリア
function ClearChip() {
    // チップ詳細設定クリア
    detailsArea = 0;
    detailsVisitNo = 0;
    detailsOrderNo = '';
    detailsApprovalId = '';
    $('#DetailsArea').val(detailsArea);
    $('#DetailsVisitNo').val(detailsVisitNo);
    $('#DetailsOrderNo').val(detailsOrderNo);
    $('#DetailsApprovalId').val(detailsApprovalId);
}

// チップ詳細設定
function SetDetail(chip) {
    // 2012/02/27 KN 西田【SERVICE_1】START
    var item;
    // エリア判定
    var area = $(chip).attr('id');
    switch (area) {
        case 'Reception':   // 受付
            // チップ情報設定
            detailsArea = 1;
            item = $(chip).children('#ReceptionDeskDevice')
            break;
        case 'Approval':    // 追加承認
            // チップ情報設定
            detailsArea = 2;
            item = $(chip).children('#ApprovalDeskDevice')
            break;
        case 'Preparation': // 納車準備
            // チップ情報設定
            detailsArea = 3;
            item = $(chip).children('#PreparationDeskDevice')
            break;
        case 'Delivery':    // 納車作業
            // チップ情報設定
            detailsArea = 4;
            item = $(chip).children('#DeliveryDeskDevice')
            break;
        case 'Work':        // 作業中
            // チップ情報設定
            detailsArea = 5;
            item = $(chip).children('#Working')
            break;
        default:
            detailsArea = 0;
            detailsVisitNo = 0;
            detailsOrderNo = '';
            detailsApprovalId = '';
            break;
    }
    this.detailsVisitNo = item.attr('visitNo');
    this.detailsOrderNo = item.attr('orderNo');
    this.detailsApprovalId = item.attr('approvalId')
    // 2012/02/27 KN 西田【SERVICE_1】END
}


// 過去ポップオーバー情報クリア
function PopupDataClear() {
    
    // アイコン要素の削除 //
    $('.PopoverRightIcnD').empty();
    $('.PopoverRightIcnD').remove();
    $('.PopoverRightIcnI').empty();
    $('.PopoverRightIcnI').remove();
    $('.PopoverRightIcnS').empty();
    $('.PopoverRightIcnS').remove();
    // 詳細内容クリア //
    $('#DetailsRegistrationNumber').text("");
    $('#DetailsCarModel').text("");
    $('#DetailsModel').text("");
    $('#DetailsVin').text("");
    $('#DetailsMileage').text("");
    $('#DetailsDeliveryCarDay').text("");
    $('#DetailsCustomerName').text("");
    $('#DetailsPhoneNumber').text("");
    $('#DetailsMobileNumber').text("");
    $('#DetailsVisitTime').text("");
    $('#ItemTime').text("");//可変項目クリア
    $('#DetailsRepresentativeWarehousing').text("");
    // ボタン制御 //
    $('#DetailButtonLeft').val("");
    $('#DetailButtonRight').val("");
    // 2012/02/22 KN 森下【SERVICE_1】START
    $('#DetailButtonLeft_Dammy').val("");
    $('#DetailButtonRight_Dammy').val("");
    // 2012/02/22 KN 森下【SERVICE_1】END
    // 非活性制御(サーバポスト中に遷移処理出来ないように) //
    $('#DetailButtonLeft').attr("disabled", "disabled");
    $('#DetailButtonRight').attr("disabled", "disabled");
}

//カウンター対応
function proccounter(diffseconds) {
    //システム時刻の取得
    var sysTime = (new Date()).getTime() + diffseconds;
    //procgroupのアイテムの取得
    var items = document.getElementsByName('procgroup');
    for (var i = 0; i < items.length; i++) {
        //アイテム情報を取得
        var item = items[i];
        //アイテムの時刻を取得
        var procdate = new Date(item.getAttribute("procdate"));
        if (procdate == "Invalid Date") {
            item.innerHTML = "00'00";
            continue;
        }
        //計測時間の取得
        var proctime = sysTime - procdate.getTime();

        //ミリセックは切り上げる。
        var proctimeCl = Math.ceil(proctime / 1000) * 1000

        if (proctimeCl >= MAX_PROC_TIME)
            proctimeCl = MAX_PROC_TIME - 1;
        else if (MIN_PROC_TIME >= proctimeCl)
            proctimeCl = MIN_PROC_TIME + 1;

        var minutes = parseInt(Math.abs(proctimeCl) / 60000);
        var seconds = parseInt(Math.abs(proctimeCl) % 60000 / 1000);

        if ((isNaN(minutes) == true)
            || (isNaN(seconds) == true)) {
            item.innerHTML = "00'00";
            continue;
        }
        //計測時間の設定
        item.innerHTML = "" + ("0" + minutes).slice(-2) + "'" + ("0" + seconds).slice(-2);

        //表示色の取得
        var className = item.getAttribute("defclass");
        var overseconds1 = item.getAttribute("overseconds1");
        var overclass1 = item.getAttribute("overclass1");
        var overseconds2 = item.getAttribute("overseconds2");
        var overclass2 = item.getAttribute("overclass2");
        //警告色
        if ((overseconds1.length > 0) && (proctime - (overseconds1 * 1000) >= 0))
            className = overclass1;
        //異常色
        if ((overseconds2.length > 0) && (proctime - (overseconds2 * 1000) >= 0))
            className = overclass2;
        //表示色の設定
        if (className != null && className.length > 0)
            item.parentNode.className = className;
    }
}

// 通知リフレッシュ処理
function MainRefresh() {
    // 2012/02/22 KN 森下【SERVICE_1】START
    // アニメーション表示
    $('#loadingSchedule').attr("style", "visibility: visible");
    // 2012/02/22 KN 森下【SERVICE_1】END
    // 隠しボタン押下しリフレッシュ
    $("#MainPolling").click();
    // ポップアップ表示戻り値
    return "TRUE";
}

// チップフリック時
function FlickChip(chip) {
    if (chip != null) {
        // ポップオーバー消失
        $(chip).trigger('hidePopover');
    }
}
//フッターアプリの起動設定
function SetFutterApplication() {

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
// 2012/02/22 KN 森下【SERVICE_1】START
// 読み込み中アイコンの非表示
function StopIcon(itemName) {
    $(itemName).hide(0);
}
// 2012/02/22 KN 森下【SERVICE_1】END
// 2012/02/22 KN 森下【SERVICE_1】START
// チップ詳細のボタン押下時の2度押し防止用
function ButtonControl(detailClickButtonName) {
    // 2012/02/29 KN 森下【SERVICE_1】START
    $.master.OpenLoadingScreen();
    // 2012/02/29 KN 森下【SERVICE_1】END
    // 非活性制御(ダミーボタンを表示し2度押しできないように) //
    $('#DetailButtonLeft').css('display', 'none');
    $('#DetailButtonRight').css('display', 'none');
    $('#DetailButtonLeft_Dammy').css('display', 'inline');
    $('#DetailButtonRight_Dammy').css('display', 'inline');
    // ダミーボタンにボタン名称を格納 //
    $('#DetailButtonLeft_Dammy').val($('#DetailButtonLeft').val());
    $('#DetailButtonRight_Dammy').val($('#DetailButtonRight').val());

    // 2012/02/29 KN 森下【SERVICE_1】START
    // チップ詳細で押下されたボタンの名称を格納(サーバでの遷移先判定に必要) //
    $('#DetailClickButtonName').val($(detailClickButtonName).val());
    $("#DetailNextScreenCommonButton").click();
    // 2012/02/29 KN 森下【SERVICE_1】END
}
// 2012/02/22 KN 森下【SERVICE_1】END

// 2012/02/23 KN 上田【SERVICE_1】START
// フッターボタンの2度押し制御
function FooterButtonControl() {
    // 2012/02/29 KN 森下【SERVICE_1】START
    $.master.OpenLoadingScreen();
    // 2012/02/29 KN 森下【SERVICE_1】END
    return true;
}
// 2012/02/23 KN 上田【SERVICE_1】END
