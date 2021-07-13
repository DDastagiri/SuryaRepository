/** 
* @fileOverview SC3100104 パネル内の処理
* 
* @author t.shimamura
* @version 1.0.0
* 更新： 2013/09/27 TMEJ t.shimamura iOS7対応 $01
*/

// ==============================================================
// 定数
// ==============================================================
// タッチデバイスの判定用
var gIsTouch = false;
var gAgent = navigator.userAgent.toLowerCase();
if (0 <= gAgent.indexOf('iphone') || 0 <= gAgent.indexOf('ipad')) {
    gIsTouch = true;
}

// タッチ系イベント名
var C_TOUCH_START = gIsTouch ? 'touchstart' : 'mousedown';
var C_TOUCH_MOVE = gIsTouch ? 'touchmove' : 'mousemove';
var C_TOUCH_END = gIsTouch ? 'touchend' : 'mouseup';

var eventType = '0';

/**
* 使用中・未使用の初期表示処理を行う.
* 
*/
$(window).load(function () {

    var mng = Sys.WebForms.PageRequestManager.getInstance();

    // 非同期ポストバックの完了時に呼び出される
    // イベント・ハンドラを定義
    mng.add_endRequest(
        function (aSender, aArgs) {
            // 検索終了時処理
            $('#SC3100104_OverRay').css('display', 'none');
            $('#CreateCustomerChipCanClick', $(parent.document)).val('1');

            if (eventType == '1') {
                $('#SearchTextStringDummy').css('display', 'none');
                $('#SearchTextString').val($('#SearchTextStringDummy').attr('innerText'));
                $('#SearchTextStringDummy').attr('innerText', "");
                $('#InputSearchText').val("");
                $('#LoadingAnimation').css('display', 'none');
            }
            else if (eventType == '2') {
                $('#SearchTextStringDummy').css('display', 'none');
                $('#SearchTextString').val($('#SearchTextStringDummy').attr('innerText'));
                $('#SearchTextStringDummy').attr('innerText', "");
                $('#InputSearchText').val("");
                $('#LoadingAnimation2').css('display', 'none');
                // 正常終了した場合のみポップアップを閉じ、親画面をリロード
                if ($('#CreateChipEndFlg').val() == '1') {
                    $('#CreateCustomerChipClickStatus', window.parent.document).val('1');
                    parent.sc3100104_ClosePopOver();
                }
            }
            eventType = '0';

            // スクロール設定
            $(".ListContent").fingerScroll();
        });

    // 汎用タップイベント
    // ・ドラッグ時は動作しない
    $.fn.setCommonEvent = function () {

        var touchStart = false;
        var touchMove = false;
        var eventTarget = null;

        $(this).live(C_TOUCH_START, function (aEvent) {

            touchStart = true;
            touchMove = false;
            eventTarget = aEvent.target;

        });

        $(this).live(C_TOUCH_MOVE, function (aEvent) {

            if (!touchStart) {
                return;
            }

            touchMove = true;
        });

        $(this).live(C_TOUCH_END, function (aEvent) {

            // ムーブで入ってきた場合なので処理対象除外
            if (!touchStart) {
                return;
            }

            // ムーブした場合なので処理対象除外
            if (touchMove) {
                return;
            }

            // 初期化
            touchStart = false;
            touchMove = false;

            $(this).trigger('tap', eventTarget);

        });

        return $(this);
    }

    // スクロール設定
    $(".ListContent").fingerScroll();

    // エリア定義定義を行う
    initArea();

    // スタッフチップ定義を行う
    initStaffChip();

    // 検索タイプチップ定義
    initSerchTypeChip()

    // 来店人数チップ定義
    initPersonNumberChip()

    /**
    * エリア定義を行う.
    */
    function initArea() {

        // エリア選択時
        $('div#CreateCustomerChipPopOverForm_popover', $(parent.document)).children(".content").bind(C_TOUCH_START, function (aEvent) {
            // キーボードが表示されている場合にキーボードを閉じるための対応
            setTimeout(function () {
                $('#CustomerSearchButton').focus();
            }, 100);
        });

        // エリア選択時
        $('div.innerDataBox').bind(C_TOUCH_START, function (aEvent) {

            // キーボードが表示されている場合にキーボードを閉じるための対応
            setTimeout(function () {
                $('#CustomerSearchButton').focus();
            }, 100);
        });
    }

    /**
    * 顧客チップ定義を行う.
    */
    function initStaffChip() {

        // 顧客チップ
        var staffChip = $('li#CustomerRow');

        // 独自イベント設定
        staffChip.setCommonEvent();

        // タップ
        staffChip.live('tap', function () {
            //キーダウン
            // 選択中の場合は選択解除
            if ($(this).hasClass('SelectedRow')) {

                // 選択解除する
                $(this).removeClass('SelectedRow');
                // 選択フラグ解除
                $('#SelectedCustomerFlag').val("0");
                return;
            }

            // 現在選択中のチップを選択解除する
            var selectChip = $('#SearchResultList').find('.SelectedRow');
            selectChip.removeClass('SelectedRow');

            // 選択中にする
            $(this).addClass('SelectedRow');

            // 顧客チップ一覧
            var CustomerChip = $('div#CustomerInnerDataBox').find('li#CustomerRow');

            $('#SelectedCustomerFlag').val("1");

            // $01 start iOS7対応
            // 現在選択している顧客情報を退避
            $('#SelectedCustName').val($(this).children("#CurrentCustName")[0].value);
            $('#SelectedCustNameTitle').val($(this).children("#CurrentCustNameTitle")[0].value);
            $('#SelectedCustKubun').val($(this).children("#CurrentCustKBN")[0].value);
            $('#SelectedRegNo').val($(this).children("#CurrentCustVclRegNo")[0].value);
            $('#SelectedVIN').val($(this).children("#CurrentCustVIN")[0].value);
            $('#SelectedCustID').val($(this).children("#CurrentCustID")[0].value);
            $('#SelectedCustType').val($(this).children("#CurrentCustType")[0].value);
            $('#SelectedCustStaffCode').val($(this).children("#CurrentCustStaffCode")[0].value);
            // $01 end iOS7対応

        });
    }

    /**
    * 人数定義を行う.
    */
    function initPersonNumberChip() {

        // 人数チップ
        var personNumChip = $('li.PersonButton');

        // タップ
        personNumChip.live(C_TOUCH_START, function () {
            //キーダウン
            // 選択中の場合はそのまま
            if ($(this).hasClass('SelectedBottun')) {

                return;
            }

            // 現在選択中のチップを選択解除する
            var selectChip = $('.NoButton').find('.SelectedBottun');
            selectChip.removeClass('SelectedBottun');

            // 選択中にする
            $(this).addClass('SelectedBottun');

            $('#SelectedPersonNumber').val(this.innerText);
            $('#SelectedPersonNumberFlag').val("1");
        });
    }

    /**
    * 検索タイプ定義
    */
    function initSerchTypeChip() {

        // 検索タイプチップ
        var SearchTypeChip = $('li.SearchTypeButton');

        // タップ
        SearchTypeChip.live(C_TOUCH_START, function () {
            //キーダウン
            // 選択中の場合はそのまま
            if ($(this).hasClass('SelectedBottun')) {

                return;
            }

            // 現在選択中のチップを選択解除する
            var selectChip = $('.SearchBottun').find('.SelectedBottun');
            selectChip.removeClass('SelectedBottun');

            // 選択中にする
            $(this).addClass('SelectedBottun');

            $('#SerchType').val(this.value);

            $('#SelectedSearchTypeFlag').val("1");
        });
    }

    /**
    * 検索ボタンタップ
    */
    $('#CustomerSearchButton').click(function () {
        $('#SearchTextString').blur();
        SearchFunction();

    });

    /**
    * クリアボタンタップ
    *
    * display:noneで制御した場合、クリックイベントが動作しないため、
    * opacityで表示非表示を切り替える
    */
    $('#ClearButton').bind("click", function (e) {
        $('#SearchTextString').focus();
    })
    $('#ClearButton').bind("touchstart mousedown", function (e) {
        //クリアボタンが表示状態の場合のみ、テキストをクリアする
        if ($('#ClearButton').css("opacity") == 1) {
            $('#SearchTextString').val("");
        }
    })
    $('#SearchTextString').focus(function (e) {
        setClearButtonOpacity(this);
    });
    $('#SearchTextString').bind("keyup", function (e) {
        setClearButtonOpacity(this);
    })
    $('#SearchTextString').blur(function (e) {
        setTimeout(function () {
            $('#ClearButton').css("opacity", 0);
        }, 100);
    });
});

/**
* クリアボタンのopacity設定.
* 
* @param  {Object} aElement イベント設定対象のオブジェクト
* @return {-} -
* 
* @example 
*  -
*/
function setClearButtonOpacity(aElement) {
    //テキストが一文字以上入力されている場合のみ、クリアボタン表示
    if ($(aElement).val().length > 0) {
        $('#ClearButton').css("opacity", 1);
    }
    else {
        $('#ClearButton').css("opacity", 0);
    }
}

function SearchFunction() {
    //テキストが無ければ実行しない
    if ($('#SearchTextString')[0].value != undefined && $.trim($('#SearchTextString')[0].value) != "") {
        //非活性化処理
        $('#CreateCustomerChipCanClick', $(parent.document)).val('0');
        $('#SC3100104_OverRay').css('display', 'block');
        $('#CustomerSerchEnd').val('0');
        $('#LoadingAnimation').css('display', 'block');

        eventType = '1';

        //リスト作成
        $('#SearchTextStringDummy').attr('innerText', $('#SearchTextString')[0].value);
        $('#SearchTextStringDummy').css('display', 'block');
        $('#InputSearchText').val(htmlEscape($('#SearchTextString')[0].value));      
        $('#SearchTextString').val(" ");      
    }
}

/**
* 登録ボタン押下時処理(登録後).
* 
* @param {-} - -
* @return {-} -
* 
* @example 
*  -
*/
function redirectSC3100104() {

    SC3100104.startServerCallback();
    $('#CreateChipEndFlg').val('0');
    eventType = '2';
}

/**
* 初期表示.
* 
* @param {-} - -
* @return {-} -
* 
* @example 
*  -
*/
function pageInit() {

    // 非表示の場合は読み込みを行わない
    if ($('#Panel_SC3100104:visible', $(parent.document)).length == 0) {
        return;
    }
    //ボタン非活性
    $('#CreateCustomerChipCanClick', $(parent.document)).val('1');

    $('#LoadSpinButton').click();
    SC3100104.startServerCallback();
}

function htmlEscape(s) {
    s = s.replace(/&/g, '&amp;');
    s = s.replace(/>/g, '&gt;');
    s = s.replace(/</g, '&lt;');
    return s;
}
/**
* 初期処理
*/
(function (window) {
    $.extend(window, { SC3100104: {} });
    $.extend(SC3100104, {

        /**
        * コールバック開始
        */
        startServerCallback: function () {
            SC3100104.showLoding();
        },

        /**
        * コールバック終了
        */
        endServerCallback: function () {
            SC3100104.closeLoding();
        },

        /******************************************************************************
        読み込み中表示
        ******************************************************************************/

        /**
        * 読み込み中アイコン表示
        */
        showLoding: function () {

            //ボタン非活性
            //$('#CreateCustomerChipCanClick', $(parent.document)).val('0');

            //オーバーレイ表示
            $('#SC3100104_OverRay').css('display', 'block');
            //アニメーション
            $('#LoadingAnimation2').css('display', 'block');
        },

        /**
        * 読み込み中アイコンを非表示にする
        * 画面活性化
        */
        closeLoding: function () {

            $('#CreateCustomerChipCanClick', $(parent.document)).val('1');
            $('#SC3100104_OverRay').css('display', 'none');
            $('#LoadingAnimation2').css('display', 'none');

        }
    });

})(window);
