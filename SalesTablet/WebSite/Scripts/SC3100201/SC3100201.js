/** 	
* @fileOverview 未対応来店客の画面制御クラス.	
* 	
* @author KN Hirose
* @version 1.0.0	
*/

/**
* 処理中のローディング画像の表示を行う.
* 
* @param {-} - -
* @return {-} -
* 
* @example 
*  -
*/
function setProcessWaiting() {

    clearAdjustStaffTimer()
    SC3100201.startServerCallback();
}

/**
* 顧客写真のクリック時の処理を行う.
* 
* @param {String} selectedItemIndex 選択したRepeaterItem
* @return {-} -
* 
* @example 
*  -
*/
function onClickButtonCustomer(selectedItemIndex) {

    $('#SelectedItemIndex').val(selectedItemIndex);
    $('#ButtonCustomer').click();
    setProcessWaiting();
}

/**
* 了解ボタンのクリック時の処理を行う.
* 
* @param {String} selectedItemIndex 選択したRepeaterItem
* @return {-} -
* 
* @example 
*  -
*/
function onClickButtonConsent(selectedItemIndex) {

    $('#SelectedItemIndex').val(selectedItemIndex);
    $('#ButtonConsent').click();
    setProcessWaiting();
}

/**
* 待ちボタンのクリック時の処理を行う.
* 
* @param {String} selectedItemIndex 選択したRepeaterItem
* @return {-} -
* 
* @example 
*  -
*/
function onClickButtonWait(selectedItemIndex) {

    $('#SelectedItemIndex').val(selectedItemIndex);
    $('#ButtonWait').click();
    setProcessWaiting();
}

/**
* 不可ボタンのクリック時の処理を行う.
* 
* @param {String} selectedItemIndex 選択したRepeaterItem
* @return {-} -
* 
* @example 
*  -
*/
function onClickButtonNotConsent(selectedItemIndex) {

    $('#SelectedItemIndex').val(selectedItemIndex);
    $('#ButtonNotConsent').click();
    setProcessWaiting();
}

/**
* 顧客詳細画面への遷移処理を行う.
* 
* @param {-} - -
* @return {-} -
* 
* @example 
*  -
*/
function redirectSC3080201() {

    $('#this_form').attr('target', '_parent')
    $('#RedirectButton').click();
    setProcessWaiting();
}

/**
* 初期表示処理を行う.
* 
* @param {-} - -
* @return {-} -
* 
* @example 
*  -
*/
function pageInit() {

    $("#processingServerSC3100201").addClass("show");
    $('#PageInitButton').click();
    SC3100201.startServerCallback();
}

/**
* ページ読み込み後に実行する処理を行う.
* 
* @param {-} - -
* @return {-} -
* 
* @example 
*  -
*/
$(window).load(function () {

    // 来店経過時間の設定
    setVisitTimer('NotDealVisit_Timer');

    // 1秒毎にタイマーの更新処理
    visitTimeCountTimer = setInterval('visitTimeCount()', 1000);
    adjustStaffTimer = setInterval('adjustStaffSwitchImage()', 500);

    $('#Div_VisitorList').fingerScroll();
});

/** 来店者の来店時間 */
var visitTimeMap = new Array();
/** 来店経過時間のインターバルタイマー */
var visitTimeCountTimer;
/** スタッフ写真点滅のインターバルタイマー */
var adjustStaffTimer;

/**
* 来店者の来店時間の設定処理
* 
* @param {String} className 来店時間の取得対象
* @return {-} -
* 
* @example 
*  -
*/
function setVisitTimer(className) {

    for (index = 0; index < $('.' + className).size(); index++) {

        var id = $('.' + className + ':eq(' + index + ')').attr('id');
        visitTimeMap[id] = $('#' + id + ' > .Timer_Data').text();
    }
}

/**
* 来店経過時間のインターバルタイマー.
* 
* @param {-} - -
* @return {-} -
* 
* @example 
*  -
*/
function visitTimeCount() {

    //対象のカウンタ分繰り返し
    for (id in visitTimeMap) {

        //対象日時
        var selector = "#" + id;
        var visitTime = visitTimeMap[id];

        //現在時間を取得
        var nowDate = new Date();

        //経過時間取得
        var nowTime = new Number(visitTimeMap[id]) + 1;
        visitTimeMap[id] = nowTime;
        // var nowTime = nowDate.getTime() - visitTime.getTime();

        //分取得
        var nowMinute = Math.floor(nowTime / (60));
        var nowTimeS = nowTime - (nowMinute * 60);

        //秒取得
        var nowSecond = Math.floor(nowTimeS);

        // ゼロ埋め
        if (nowSecond < 10) {

            nowSecond = '0' + nowSecond;
        }

        // 表示する時間
        var text = nowMinute + '\'' + nowSecond + '\'\'';

        // スタイルの設定
        var style = 'Blue'; // 標準表示
        // 2012/02/28 KN広瀬 【SALES_1B】仕様変更対応 - 単位を分から秒に変更
        var warningSeconds = new Number($('#NotDealTimeAlertSpan').attr('value')); // 未対応警告時間

        if (warningSeconds > 0) {

            if (nowTime > warningSeconds * 2) {
                //未対応警告時間×2分を経過

                style = 'Red';

            } else if (nowTime > warningSeconds) {
                //未対応警告時間を経過

                style = 'Yellow';
            }
        }

        $(selector + " > .Timer_Disp").html('<p class="' + style + '">' + text + '</p>');
    }
}

/**
* スタッフ写真点滅のインターバルタイマー.
* 
* @param {-} - -
* @return {-} -
* 
* @example 
*  -
*/
function adjustStaffSwitchImage() {

    for (index = 0; index < $('.AdjustStaff').size(); index++) {

        var visibility = $('.AdjustStaff:eq(' + index + ')').css('visibility');
        if (visibility == 'hidden') {

            $('.AdjustStaff:eq(' + index + ')').css('visibility', 'visible');

        } else {

            $('.AdjustStaff:eq(' + index + ')').css('visibility', 'hidden');
        }
    }
}

/**
* 調整中のスタッフ写真の点滅を止める.
* 
* @param {-} - -
* @return {-} -
* 
* @example 
*  -
*/
function clearAdjustStaffTimer() {

    clearInterval(adjustStaffTimer)
    for (index = 0; index < $('.AdjustStaff').size(); index++) {

        var visibility = $('.AdjustStaff:eq(' + index + ')').css('visibility');
        $('.AdjustStaff:eq(' + index + ')').css('visibility', 'hidden');
    }
}

/**
* 初期処理
*/
(function (window) {

    $.extend(window, { SC3100201: {} });
    $.extend(SC3100201, {

        /**
        * コールバック開始
        */
        startServerCallback: function () {
            SC3100201.showLoding();
        },

        /**
        * コールバック終了
        */
        endServerCallback: function () {
            SC3100201.closeLoding();
        },

        /******************************************************************************
        読み込み中表示
        ******************************************************************************/

        /**
        * 読み込み中アイコン表示
        */
        showLoding: function () {

            //オーバーレイ表示
            $("#registOverlayBlackSC3100201").css("display", "block");
            //アニメーション
            setTimeout(function () {
                $("#processingServerSC3100201").addClass("show");
                $("#registOverlayBlackSC3100201").addClass("open");
            }, 0);

        },

        /**
        * 読み込み中アイコンを非表示にする
        */
        closeLoding: function () {

            $("#processingServerSC3100201").removeClass("show");
            $("#registOverlayBlackSC3100201").removeClass("open").one("webkitTransitionEnd", function (we) {
                $("#registOverlayBlackSC3100201").css("display", "none");
            });
        }
    });

})(window);
