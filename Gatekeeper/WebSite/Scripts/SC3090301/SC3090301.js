/** 	
* @fileOverview ゲートキーパーメインの画面制御クラス.	
* 	
* @author KN Asano
* @version 1.0.0	
* 更新： 2012/05/16 KN 浅野  【SALES_2】号口(課題No.131)対応
* 更新： 2012/05/22 KN 浅野  クルクル対応
* 更新： 2012/06/04 KN 浅野  クルクル対応(検証不具合修正)
* 更新： 2013/09/27 TMEJ 浅野 iOS7対応 $04
* 更新： 2013/12/02 TMEJ 嶋村 次世代e-CRBサービス 店舗展開に向けた標準作業確立 $05
* 更新： 2018/02/19 NSK 河谷 REQ-SVT-TMT-20170615-001_iPod Gatekeeperに顧客の車両登録番号を入力する機能を追加 $06
* 更新： 2019/02/28 NSK 河谷 REQ-SVT-TMT-20180601-001 Gate Keeper機能の視認性操作性改善 $07
*/

//2018/02/19 NSK 河谷 REQ-SVT-TMT-20170615-001_iPod Gatekeeperに顧客の車両登録番号を入力する機能を追加 START
//車両登録番号入力有無フラグ
var gRegNumInputFlag;
//現在選択中の来店目的ボタンのIDを格納
var gCurrentId;

//車両登録番号入力有無フラグ「0：入力無し」
var C_REGNUM_NOT_INPUT = "0";
//車両登録番号入力有無フラグ「1：入力有り」
var C_REGNUM_INPUT = "1";
//2018/02/19 NSK 河谷 REQ-SVT-TMT-20170615-001_iPod Gatekeeperに顧客の車両登録番号を入力する機能を追加 END

//2019/02/28 NSK 河谷 REQ-SVT-TMT-20180601-001 Gate Keeper機能の視認性操作性改善 START
//車両登録番号入力区分「1：文字・数字が入力可能」
var C_VCLREGNO_INPUT_TYPE_TEXT = "1";
//車両登録番号入力区分「2：数字のみ入力可能」
var C_VCLREGNO_INPUT_TYPE_NUMBER_ONLY = "2";

//タブレット使用フラグ「1：タブレット使用店舗」
var C_TABLET_USE_FLG_ON = "1";

//来店目的「1：セールス」
var C_VISIT_PURPOSE_SALES_ID = "btn_new";
//来店目的「2：サービス」
var C_VISIT_PURPOSE_SERVICE_ID = "btn_repair";
//来店目的「3：対象外」
var C_VISIT_PURPOSE_OTHER_ID = "btn_other";

//来店人数：1名
var C_VISIT_PERSON_ONE = "1";

// 来店目的ボタンの画面上部からの高さ(1行表示)
var C_VISIT_PURPOSE_BUTTON_ONE_LINE_TOP = 760;
// 来店目的ボタンの画面上部からの高さ(2行表示)
var C_VISIT_PURPOSE_BUTTON_TWO_LINE_TOP = 680;

// 次のN件、前のN件、未送信件数の画面上部からの高さ(来店人数エリア表示)
var C_COUNT_LABEL_NUMBER_VISIBLE_TOP = 600;
// 次のN件、前のN件、未送信件数の画面上部からの高さ(来店人数エリア非表示)
var C_COUNT_LABEL_DEFAULT_TOP = 680;

// 1車両分の情報を表示する領域の高さ(来店人数エリア表示)
var C_REGNOREAD_NUMBER_VISIBLE_HEIGHT = 482;
// 1車両分の情報を表示する領域の高さ(来店人数エリア非表示)
var C_REGNOREAD_DEFAULT_HEIGHT = 562;

// 車両情報・顧客名を表示する領域の高さ(来店人数エリア表示)
var C_CSTVCLAREA_NUMBER_VISIBLE_HEIGHT = 402;
// 車両情報・顧客名を表示する領域の高さ(来店人数エリア非表示)
var C_CSTVCLAREA_DEFAULT_HEIGHT = 482;
//2019/02/28 NSK 河谷 REQ-SVT-TMT-20180601-001 Gate Keeper機能の視認性操作性改善 END

// ポップアップオブジェクト
var gPopOverFrom = null;

// 削除ポップアップ表示フラグ
var delDispFlag = 0;

var bottunPushFlag = 0;

$(window).load(function () {

    if ($('#dispType').attr('value') == '2' || $('#dispType').attr('value') == '1') {
        // 車両情報の初期表示位置を指定
        //2019/02/28 NSK 河谷 REQ-SVT-TMT-20180601-001 Gate Keeper機能の視認性操作性改善 START
        // $('#vclNoRead').scrollTop(370);
        $('#vclNoRead').scrollTop(562);
        //2019/02/28 NSK 河谷 REQ-SVT-TMT-20180601-001 Gate Keeper機能の視認性操作性改善 END
    }

    //前のN件押下後のスクロール
    if ($('#autoScrollFlag').val() == "2") {
        //フリック関連
        var carInfoMaxIndex = $('#vclNoRead > .contents').length - 1;
        var carHeight = $('#vclNoRead > .contents:eq(0)').height();
        var maxDisplayNumber = parseInt($('#MaxDisplayCountNumber').val());
        var readInfoNumber = parseInt($('#NextOrPreviewDisplayCountNumber').val());
        var CurrentHeadInfo = parseInt($('#CurrentDisplayHeaderNumber').val());
        var unsentDataCount = parseInt($('#unsetDataCount').val());
        var autoMoveY = readInfoNumber * carHeight;

        $('#vclNoRead').scrollTop(autoMoveY);
        $('#selectVclNoIndex').val(readInfoNumber)
        $('#autoScrollFlag').val("0")

    }

    //次のN件押下後のスクロール
    if ($('#autoScrollFlag').val() == "1") {
        //フリック関連
        var carInfoMaxIndex = $('#vclNoRead > .contents').length - 1;
        var carHeight = $('#vclNoRead > .contents:eq(0)').height();
        var maxDisplayNumber = parseInt($('#MaxDisplayCountNumber').val());
        var readInfoNumber = parseInt($('#NextOrPreviewDisplayCountNumber').val());
        var CurrentHeadInfo = parseInt($('#CurrentDisplayHeaderNumber').val());
        var unsentDataCount = parseInt($('#unsetDataCount').val());


        var autoMoveY = carHeight * (readInfoNumber + 1);

        $('#vclNoRead').scrollTop(autoMoveY);
        $('#selectVclNoIndex').val(readInfoNumber + 1)
        $('#autoScrollFlag').val("0")

    }

    //送信処理後のスクロール
    if ($('#autoScrollFlag').val() == "3") {
        //フリック関連
        var carInfoMaxIndex = $('#vclNoRead > .contents').length - 1;
        var carHeight = $('#vclNoRead > .contents:eq(0)').height();
        var maxDisplayNumber = parseInt($('#MaxDisplayCountNumber').val());
        var readInfoNumber = parseInt($('#NextOrPreviewDisplayCountNumber').val());
        var CurrentHeadInfo = parseInt($('#CurrentDisplayHeaderNumber').val());
        var unsentDataCount = parseInt($('#unsetDataCount').val());
        var preSelectedVclIndex = parseInt($('#preSelectVclNoIndex').val());

        var autoMoveY = carHeight * (preSelectedVclIndex);

        $('#vclNoRead').scrollTop(autoMoveY);
        $('#selectVclNoIndex').val(preSelectedVclIndex)
        $('#autoScrollFlag').val("0")

        //        if (CurrentHeadInfo + maxDisplayNumber - 1 < unsentDataCount && maxDisplayNumber == preSelectedVclIndex) {
        //            $('#lbl_NextDataCount').css("display", "inline-block");
        //        } else if (CurrentHeadInfo > 1 && preSelectedVclIndex == 1) {
        //            $('#lbl_PreviewDataCount').css("display", "inline-block");
        //        }
    }
    //2018/02/19 NSK 河谷 REQ-SVT-TMT-20170615-001_iPod Gatekeeperに顧客の車両登録番号を入力する機能を追加 START

    // 新規登録画面[車]を開いている場合、
    // 手入力した車両登録番号の入力有無をチェックする
    if ($('#dispType').attr('value') == '3') {

        // テキストボックスに1文字以上入力があった場合、
        // 車両登録番号入力有無フラグを「1：入力あり」とする
        if (0 < $('#RegNumTxt').val().length) {

            gRegNumInputFlag = C_REGNUM_INPUT;

            // テキストボックスに入力が無い場合は、
            // 車両登録番号入力有無フラグを「0：入力なし」とする
        } else {

            gRegNumInputFlag = C_REGNUM_NOT_INPUT;
        }
    }
    //2018/02/19 NSK 河谷 REQ-SVT-TMT-20170615-001_iPod Gatekeeperに顧客の車両登録番号を入力する機能を追加 END
});

// ボタン選択フラグ
var gIsSelectNinzuButton = false;
var gIsSelectPurposeButton = false;

//2012/06/04 KN 浅野  クルクル対応(検証不具合修正) START
var gIsReload = false;
//2012/06/04 KN 浅野  クルクル対応(検証不具合修正) END

$(function () {

    //2012/06/04 KN 浅野  クルクル対応(検証不具合修正) START
    // フラグ初期化
    gIsReload = false;
    //2012/06/04 KN 浅野  クルクル対応(検証不具合修正) END

    // 画面の表示タイプを判断し、各アイコンの表示・非表示を制御する。
    switch ($('#dispType').attr('value')) {
        case '1':

            // 待機画面
            // 車ボタン、歩きボタン活性
            $('#btn_car').css('display', 'block');
            $('#btn_car_o').css('display', 'none');
            $('#btn_person').css('display', 'block');
            $('#btn_person_o').css('display', 'none');
            //2018/02/19 NSK 河谷 REQ-SVT-TMT-20170615-001_iPod Gatekeeperに顧客の車両登録番号を入力する機能を追加 START
            $('#btn_AppList').css('display', 'block');
            $('#btn_AppList_o').css('display', 'none');
            //2018/02/19 NSK 河谷 REQ-SVT-TMT-20170615-001_iPod Gatekeeperに顧客の車両登録番号を入力する機能を追加 END

            // 来店人数ボタン非活性
            clearButton('number', 'ninzuButton', 'n');

            // 来店目的ボタン非活性
            clearButton('purpose', 'purposeButton', 'n');

            // 初回読み込み判断
            var firstLoad = $('#firstLoad').attr('value');
            if (firstLoad == '1') {
                $('#firstLoad').attr('value', '0')

                // 以前のCookie情報が残っていればクリアしておく。
                if (document.cookie) {
                    document.cookie = "IsReload=0;"
                }

                //2012/05/22 KN 浅野  クルクル対応 START
                //タイマーセット
                if (gIsReload == false) {
                    gIsReload = true;
                    // これ以前のtimerを無視する。
                    commonClearTimer();
                    commonRefreshTimer(
                        function () {
                            //リロード処理
                            $('#initButton').click();
                        }
                    );
                }
                //2012/05/22 KN 浅野  クルクル対応 END

                // 初期読み込み
                $('#initButton').click();

                // ロード画面表示
                $.master.OpenLoadingScreen();

            }
            else {

                // 顧客情報エリアのサイズ変更
                initCustomerAria();

                // 選択位置を初期化
                $('#selectVclNoIndex').attr('value', 1);
                $('#selectCustIndex').attr('value', 0);
            }
            break;

        case '2':

            // 登録番号読取時画面
            // 車ボタン、歩きボタン活性
            $('#btn_car').css('display', 'block');
            $('#btn_car_o').css('display', 'none');
            $('#btn_person').css('display', 'block');
            $('#btn_person_o').css('display', 'none');
            //2018/02/19 NSK 河谷 REQ-SVT-TMT-20170615-001_iPod Gatekeeperに顧客の車両登録番号を入力する機能を追加 START
            $('#btn_AppList').css('display', 'block');
            $('#btn_AppList_o').css('display', 'none');
            //2018/02/19 NSK 河谷 REQ-SVT-TMT-20170615-001_iPod Gatekeeperに顧客の車両登録番号を入力する機能を追加 END

            // 来店人数ボタン活性
            clearButton('number', 'ninzuButton', '');

            // 来店目的ボタン活性
            clearButton('purpose', 'purposeButton', '');

            // スクロールランプの初期化
            initScrollLamp();

            // 顧客情報エリアのサイズ変更
            initCustomerAria();

            // 選択位置を初期化
            $('#selectVclNoIndex').attr('value', 1);
            $('#selectCustIndex').attr('value', 0);

            break;

        case '3':

            // 新規登録画面[車]
            // 車ボタン、歩きボタン活性
            $('#btn_car').css('display', 'none');
            $('#btn_car_o').css('display', 'block');
            $('#btn_person').css('display', 'block');
            $('#btn_person_o').css('display', 'none');
            //2018/02/19 NSK 河谷 REQ-SVT-TMT-20170615-001_iPod Gatekeeperに顧客の車両登録番号を入力する機能を追加 START
            $('#btn_AppList').css('display', 'block');
            $('#btn_AppList_o').css('display', 'none');

            // 車両登録番号入力欄に入力した文字列を初期化
            $('#RegNumTxt').attr('value', $("#InputRegNumber").val());
            //2018/02/19 NSK 河谷 REQ-SVT-TMT-20170615-001_iPod Gatekeeperに顧客の車両登録番号を入力する機能を追加 END

            //2019/02/28 NSK 河谷 REQ-SVT-TMT-20180601-001 Gate Keeper機能の視認性操作性改善 START
            // 車両登録番号入力区分が「1：文字・数字が入力可能(文字がデフォルト)」の場合
            if ($('#VclRegNoInputType').val() == C_VCLREGNO_INPUT_TYPE_TEXT) {

                // 初期表示するキーボードを文字入力に設定する
                $('#RegNumTxt').get(0).type = 'text';
                $('#RegNumTxt').removeAttr('pattern');
            }
            // 車両登録番号入力区分が「2：数字のみ入力可能」の場合
            else if ($('#VclRegNoInputType').val() == C_VCLREGNO_INPUT_TYPE_NUMBER_ONLY) {

                // 数字のみ入力できるキーボードを設定する
                $('#RegNumTxt').get(0).type = 'text';
                $('#RegNumTxt').attr('pattern', '[0-9]*');
            }
            //2019/02/28 NSK 河谷 REQ-SVT-TMT-20180601-001 Gate Keeper機能の視認性操作性改善 END

            // 来店人数ボタン活性
            clearButton('number', 'ninzuButton', '');

            // 来店目的ボタン活性
            clearButton('purpose', 'purposeButton', '');

            break;

        case '4':

            // 新規登録画面[歩き]
            // 車ボタン、歩きボタン活性
            $('#btn_car').css('display', 'block');
            $('#btn_car_o').css('display', 'none');
            $('#btn_person').css('display', 'none');
            $('#btn_person_o').css('display', 'block');
            //2018/02/19 NSK 河谷 REQ-SVT-TMT-20170615-001_iPod Gatekeeperに顧客の車両登録番号を入力する機能を追加 START
            $('#btn_AppList').css('display', 'block');
            $('#btn_AppList_o').css('display', 'none');
            //2018/02/19 NSK 河谷 REQ-SVT-TMT-20170615-001_iPod Gatekeeperに顧客の車両登録番号を入力する機能を追加 END

            // 来店人数ボタン活性
            clearButton('number', 'ninzuButton', '');

            // 来店目的ボタン活性
            clearButton('purpose', 'purposeButton', '');

            break;
    };

    //2018/02/19 NSK 河谷 REQ-SVT-TMT-20170615-001_iPod Gatekeeperに顧客の車両登録番号を入力する機能を追加 START

    // HiddenFieldに格納した車両登録番号の値を初期化する
    document.getElementById("InputRegNumber").value = "";

    //2018/02/19 NSK 河谷 REQ-SVT-TMT-20170615-001_iPod Gatekeeperに顧客の車両登録番号を入力する機能を追加 END

    //2019/02/28 NSK 河谷 REQ-SVT-TMT-20180601-001 Gate Keeper機能の視認性操作性改善 START
    //// 送信ボタン非活性
    //$('#btn_submit_o').css('display', 'none');
    //$('#btn_submit_n').css('display', 'block');

    // 来店目的ボタンの初期表示の高さを指定する
    changePurposeButtonOneLine()
    //2019/02/28 NSK 河谷 REQ-SVT-TMT-20180601-001 Gate Keeper機能の視認性操作性改善 END

    gIsSelectNinzuButton = false;
    gIsSelectPurposeButton = false;

    //2012/06/04 KN 浅野  クルクル対応(検証不具合修正) START
    // クッキーの値を確認しリロード要求があれば再度読み込みを行う。
    if (document.cookie) {

        var cookies = document.cookie.split(";");
        for (var index = 0; index < cookies.length; index++) {
            var str = cookies[index].split("=");
            if (str[0] == "IsReload" && str[1] == "1") {
                document.cookie = "IsReload=0;"
                $('#initButton').click();
                break;
            }
        }
    }
    //2012/06/04 KN 浅野  クルクル対応(検証不具合修正) END

    /**
    * 来店客人数ボタン押下時の制御を行う。
    * 
    * @param {-} - -
    * @return {-} -
    * 
    * @example 
    *  -
    */
    $('.ninzuButton').bind('click', function () {

        // 来店人数ボタンを選択済みとする。
        gIsSelectNinzuButton = true;

        // ボタンの制御
        changeButton('number', 'ninzuButton', this.id);

        // 選択されたボタンの人数を保持
        $('#personNum').attr('value', this.innerText);

        //2019/02/28 NSK 河谷 REQ-SVT-TMT-20180601-001 Gate Keeper機能の視認性操作性改善 START
        // 送信処理を実施
        // ボタンを青くしてから確認ダイアログを出すためにsetTimeoutでタイミングをずらしている
        setTimeout(function () {

            submit();
        }, 10);
        //2019/02/28 NSK 河谷 REQ-SVT-TMT-20180601-001 Gate Keeper機能の視認性操作性改善 END

    });

    /**
    * 来店客目的ボタン押下時の制御を行う。
    * 
    * @param {-} - -
    * @return {-} -
    * 
    * @example 
    *  -
    */
    $('.purposeButton').bind('click', function () {

        //来店目的ボタンを選択済みとする。
        gIsSelectPurposeButton = true;

        // ボタンの制御
        changeButton('purpose', 'purposeButton', this.id);

        // 選択されたボタンの目的を保持
        $('#purposeType').attr('value', this.children[0].innerText);

        //2019/02/28 NSK 河谷 REQ-SVT-TMT-20180601-001 Gate Keeper機能の視認性操作性改善 START
        // サービスボタン、対象外ボタン押下時に送信処理を実施
        if (this.id == C_VISIT_PURPOSE_SERVICE_ID || this.id == C_VISIT_PURPOSE_OTHER_ID) {

            // 来店人数ボタンが表示されている場合、来店人数ボタンを非表示にする
            if ($('#number').css('display') == 'block') {

                hideNumberButtonArea();
            }

            // サービスは来店人数を使用しないため、来店人数に"1"を入れておく
            $("#personNum").val(C_VISIT_PERSON_ONE);

            // 送信処理を実行
            // ボタンを青くしてから確認ダイアログを出すためにsetTimeoutでタイミングをずらしている
            setTimeout(function () {

                submit();
            }, 10);
        }
        // セールスボタン押下、かつ、来店人数ボタンが非表示の場合、来店人数ボタンを表示する
        else if (this.id == C_VISIT_PURPOSE_SALES_ID && $('#number').css('display') == 'none') {

            showNumberButtonArea();
        }
        //2019/02/28 NSK 河谷 REQ-SVT-TMT-20180601-001 Gate Keeper機能の視認性操作性改善 END
    });

    /**
    * 各ボタンの制御を行う。
    * 
    * @param {String} aAreaId 制御対象ボタンのエリアID
    * @param {String} aClassName 制御対象ボタンのクラス名
    * @param {String} aIdVal 表示させるボタンタイプ
    * @return {-} -
    * 
    * @example 
    *  -
    */
    function changeButton(aAreaId, aClassName, aIdVal) {

        // ボタンの活性・非活性判断
        if (aIdVal.slice(-1) == 'n') {

            // 非活性の場合は、処理しない。
            return;
        }

        // ボタンを表示(非選択)とする。
        clearButton(aAreaId, aClassName, '')

        // 押下されたボタンを活性化
        var id = ''
        if (aIdVal.slice(-1) != 'o') {
            id = aIdVal + '_o';
        }
        else {
            id = aIdVal;
        }
        $('#' + id).css('display', 'block');

        //2019/02/28 NSK 河谷 REQ-SVT-TMT-20180601-001 Gate Keeper機能の視認性操作性改善 START
        //// 来店人数ボタン、来店目的ボタンの選択状況判断
        //if (aIdVal.indexOf('other') > 0) {
        //
        //    // 来店目的ボタン(対象外)が押下された場合、送信ボタン活性化
        //    $('#btn_submit_n').css('display', 'none');
        //    $('#btn_submit_o').css('display', 'block');
        //
        //}
        ////2018/02/19 NSK 河谷 REQ-SVT-TMT-20170615-001_iPod Gatekeeperに顧客の車両登録番号を入力する機能を追加 START
        //else if ($('#dispType').attr('value') == '3' && gRegNumInputFlag == C_REGNUM_NOT_INPUT) {
        //
        //    // 新規入力画面(車)かつ、車両登録Noが未入力の場合、送信ボタン非活性化
        //    $('#btn_submit_o').css('display', 'none');
        //    $('#btn_submit_n').css('display', 'block');
        //}
        ////2018/02/19 NSK 河谷 REQ-SVT-TMT-20170615-001_iPod Gatekeeperに顧客の車両登録番号を入力する機能を追加 END
        //else if (gIsSelectNinzuButton && gIsSelectPurposeButton) {
        //
        //    // 来店人数ボタン、来店目的ボタンが共に選択済みの場合、送信ボタン活性化
        //    $('#btn_submit_n').css('display', 'none');
        //    $('#btn_submit_o').css('display', 'block');
        //}
        //else {
        //    // 上記以外は、送信ボタン非活性化
        //    $('#btn_submit_n').css('display', 'block');
        //    $('#btn_submit_o').css('display', 'none');
        //}

        // 新規入力画面(車)の場合
        if ($('#dispType').attr('value') == '3') {

            // 車両登録Noが未入力の場合、
            if (gRegNumInputFlag == C_REGNUM_NOT_INPUT) {

                // サービスボタンを非活性にする
                disablePurposeButton(C_VISIT_PURPOSE_SERVICE_ID);

                // セールスボタンを非活性にする
                disablePurposeButton(C_VISIT_PURPOSE_SALES_ID);
            }
            // 車両登録Noが入力されている場合
            else {

                // サービスボタンを活性にする
                enablePurposeButton(C_VISIT_PURPOSE_SERVICE_ID);

                // セールスボタンを活性にする
                enablePurposeButton(C_VISIT_PURPOSE_SALES_ID);
            }
        }
        //2019/02/28 NSK 河谷 REQ-SVT-TMT-20180601-001 Gate Keeper機能の視認性操作性改善 END

        //2018/02/19 NSK 河谷 REQ-SVT-TMT-20170615-001_iPod Gatekeeperに顧客の車両登録番号を入力する機能を追加 START
        // 来店目的ボタン押下時にボタンのIDを格納する
        if (aClassName == "purposeButton") {
            gCurrentId = aIdVal;
        }
        //2018/02/19 NSK 河谷 REQ-SVT-TMT-20170615-001_iPod Gatekeeperに顧客の車両登録番号を入力する機能を追加 END
    }

    /**
    * 各ボタンの初期化を行う。
    * 
    * @param {String} aAreaId 制御対象ボタンのエリアID
    * @param {String} aClassName 制御対象ボタンのクラス名
    * @param {String} aEnableVal 表示させるボタンタイプ
    * @return {-} -
    * 
    * @example 
    *  -
    */
    function clearButton(aAreaId, aClassName, aEnableVal) {

        // 対象のカウンタ分繰り返し
        for (i = 0; i < $('#' + aAreaId + ' > .' + aClassName).size(); i++) {
            var id = $('#' + aAreaId + ' > .' + aClassName + ':eq(' + i + ')').attr('id');

            // 初期化
            if (id.slice(-1) == aEnableVal || (aEnableVal == '' && id.slice(-1) != 'n' && id.slice(-1) != 'o')) {
                $('#' + id).css('display', 'block');
            }
            else {
                $('#' + id).css('display', 'none');
            }
        }

        //2019/02/28 NSK 河谷 REQ-SVT-TMT-20180601-001 Gate Keeper機能の視認性操作性改善 START
        // 来店目的ボタンの初期化時に、ボタンの表示・非表示の制御を行う
        if (aClassName == 'purposeButton') {

            // 来店目的ボタンの個数、幅を指定する
            initPurposeButton();

            // 新規入力画面(車)かつ、車両登録番号が入力されていない場合、
            // サービス・セールスボタンを非活性にする
            if ($('#dispType').attr('value') == '3' && $('#RegNumTxt').val().length == 0) {

                // サービスボタンを非活性にする
                disablePurposeButton(C_VISIT_PURPOSE_SERVICE_ID);

                // セールスボタンを非活性にする
                disablePurposeButton(C_VISIT_PURPOSE_SALES_ID);
            }
        }
        //2019/02/28 NSK 河谷 REQ-SVT-TMT-20180601-001 Gate Keeper機能の視認性操作性改善 END
    }

    //2019/02/28 NSK 河谷 REQ-SVT-TMT-20180601-001 Gate Keeper機能の視認性操作性改善 START
    /**
    * 来店目的ボタンの初期化処理を行う。
    * 
    * @param {-} - -
    * @return {-} -
    * 
    * @example 
    *  -
    */
    function initPurposeButton() {

        // タブレット使用フラグに応じて、ボタンの非表示処理とボタン幅の調整を行う

        // セールスタブレット使用店舗である
        if ($("#SalesTabletUseFlg").val() == C_TABLET_USE_FLG_ON) {

            // サービスタブレット・セールスタブレット使用店舗
            if ($("#ServiceTabletUseFlg").val() == C_TABLET_USE_FLG_ON) {

                // セールスボタンの調整
                setPurposeButtonStyle('btn_new', 'btn_3_position_left');
                setPurposeButtonStyle('img_new', 'img_3_position_new');

                // サービスボタンの調整
                setPurposeButtonStyle('btn_repair', 'btn_3_position_center');
                setPurposeButtonStyle('img_repair', 'img_3_position_repair');

                // 対象外ボタンの調整
                setPurposeButtonStyle('btn_other', 'btn_3_position_right');
                setPurposeButtonStyle('img_other', 'img_3_position_other');
            }
            // セールスタブレットのみ使用店舗
            else {

                // サービスボタン非表示
                hidePurposeButton(C_VISIT_PURPOSE_SERVICE_ID);

                // セールスボタンの調整
                setPurposeButtonStyle('btn_new', 'btn_2_position_left');
                setPurposeButtonStyle('img_new', 'img_2_position_new');

                // 対象外ボタンの調整
                setPurposeButtonStyle('btn_other', 'btn_2_position_right');
                setPurposeButtonStyle('img_other', 'img_2_position_other');
            }
        }
        // サービスタブレットのみ使用店舗
        else if ($("#ServiceTabletUseFlg").val() == C_TABLET_USE_FLG_ON) {

            // セールスボタン非表示
            hidePurposeButton(C_VISIT_PURPOSE_SALES_ID);

            // サービスボタンの調整
            setPurposeButtonStyle('btn_repair', 'btn_2_position_left');
            setPurposeButtonStyle('img_repair', 'img_2_position_repair');

            // 対象外ボタンの調整
            setPurposeButtonStyle('btn_other', 'btn_2_position_right');
            setPurposeButtonStyle('img_other', 'img_2_position_other');
        }
        // セールスタブレット使用店舗でも、サービスタブレット使用店舗でもない
        else {

            // サービスボタン・セールスボタン非表示
            hidePurposeButton(C_VISIT_PURPOSE_SALES_ID);
            hidePurposeButton(C_VISIT_PURPOSE_SERVICE_ID);

            // 対象外ボタンの調整
            setPurposeButtonStyle('btn_other', 'btn_1_position');
            setPurposeButtonStyle('img_other', 'img_1_position_other');
        }
    }

    /**
    * 来店目的ボタンにスタイルを設定する
    * 
    * @param {String} buttonId スタイルを設定するボタンのID
    * @param {String} styleClass ボタンに設定するスタイル(クラス名)
    * @return {-} -
    * 
    * @example 
    *  -
    */
    function setPurposeButtonStyle(buttonId, styleClass) {

        $('#' + buttonId).addClass(styleClass);
        $('#' + buttonId + '_o').addClass(styleClass);
        $('#' + buttonId + '_n').addClass(styleClass);
    }

    /**
    * 来店目的ボタンを非表示にする
    * 
    * @param {String} buttonId 非表示にするボタンのID
    * @return {-} -
    * 
    * @example 
    *  -
    */
    function hidePurposeButton(buttonId) {

        $("#" + buttonId).css("display", "none");
        $("#" + buttonId + "_o").css("display", "none");
        $("#" + buttonId + "_n").css("display", "none");
    }

    /**
    * 来店目的ボタンを非活性にする
    * 
    * @param {String} buttonId 非活性にするボタンのID
    * @return {-} -
    * 
    * @example 
    *  -
    */
    function disablePurposeButton(buttonId) {

        // ボタンが表示されている場合、非活性にする
        if ($('#' + buttonId).css('display') == 'block') {

            $('#' + buttonId + '_n').css('display', 'block');
        }
    }

    /**
    * 来店目的ボタンを活性にする
    * 
    * @param {String} buttonId 活性にするボタンのID
    * @return {-} -
    * 
    * @example 
    *  -
    */
    function enablePurposeButton(buttonId) {

        // ボタンが表示されている場合、活性にする
        if ($('#' + buttonId).css('display') == 'block') {

            $('#' + buttonId + '_n').css('display', 'none');
        }
    }

    /**
    * 来店人数ボタンを表示する
    * 
    * @param {-} -
    * @return {-} -
    * 
    * @example 
    *  -
    */
    function showNumberButtonArea() {

        // 現在表示している画面の種類を取得する
        var dispType = $('#dispType').attr('value');

        // 新規登録画面[車]または新規登録画面[歩き]
        if (dispType == '3' || dispType == '4') {

            // 新規入力画面の画像表示エリアを来店人数ボタンの表示の高さ分短くする
            $('.img_contents').css('height', C_CSTVCLAREA_NUMBER_VISIBLE_HEIGHT + 'px');
        }
        // 待機画面または登録番号読み取り画面
        else {

            // 車両情報のセルを画面上に表示する領域の高さを設定する
            $('.contentsAria').css('height', C_REGNOREAD_NUMBER_VISIBLE_HEIGHT + 'px');

            // 車両情報のセル1つあたりの高さを設定する
            $('.contents').css('height', C_REGNOREAD_NUMBER_VISIBLE_HEIGHT + 'px');
            carHeight = $('#vclNoRead > .contents:eq(0)').height();

            // 選択している車両情報のセルを画面内に表示するために、
            // スクロールする高さを調整する
            var selectedIndex = Number($('#selectVclNoIndex').val());
            $('#vclNoRead').scrollTop(carHeight * selectedIndex);

            // 登録番号読取画面の情報表示エリア
            $('.tableArea').css('height', C_CSTVCLAREA_NUMBER_VISIBLE_HEIGHT + 'px');

            // 前のN件、次のN件の表示位置
            $('#lbl_PreviewDataCount').css('top', C_COUNT_LABEL_NUMBER_VISIBLE_TOP + 'px');
            $('#lbl_NextDataCount').css('top', C_COUNT_LABEL_NUMBER_VISIBLE_TOP + 'px');
        }

        // 未送信件数の表示位置
        $('#lbl_unSendCount').css('top', C_COUNT_LABEL_NUMBER_VISIBLE_TOP + 'px');

        // 来店人数ボタンを表示する処理
        $('#number').css('display', 'block');

        // 来店目的ボタンを2段表示の上部に表示させる
        changePurposeButtonTwoLine();
    }

    /**
    * 来店人数ボタンを非表示にする
    * 
    * @param {-} -
    * @return {-} -
    * 
    * @example 
    *  -
    */
    function hideNumberButtonArea() {

        // 現在表示している画面の種類を取得する
        var dispType = $('#dispType').attr('value');

        // 新規登録画面[車]または新規登録画面[歩き]
        if (dispType == '3' || dispType == '4') {

            // 新規入力画面の画像表示エリアを来店人数エリアの表示の高さ分長くする。
            $('.img_contents').css('height', C_CSTVCLAREA_DEFAULT_HEIGHT + 'px');

        }
        // 待機画面または登録番号読み取り画面
        else {

            // 車両情報のセルを画面上に表示する領域の高さを設定する
            $('.contentsAria').css('height', C_REGNOREAD_DEFAULT_HEIGHT + 'px');

            // 車両情報のセル1つあたりの高さを設定する
            $('.contents').css('height', C_REGNOREAD_DEFAULT_HEIGHT + 'px');
            carHeight = $('#vclNoRead > .contents:eq(0)').height();

            // 選択している車両情報のセルを画面内に表示するために、
            // スクロールする高さを調整する
            var selectedIndex = Number($('#selectVclNoIndex').val());
            $('#vclNoRead').scrollTop(carHeight * selectedIndex);

            // 登録番号読取画面の情報表示エリア
            $('.tableArea').css('height', C_CSTVCLAREA_DEFAULT_HEIGHT + 'px');

            // 前のN件、次のN件の表示位置を変更する
            $('#lbl_NextDataCount').css('top', C_COUNT_LABEL_DEFAULT_TOP + 'px');
            $('#lbl_PreviewDataCount').css('top', C_COUNT_LABEL_DEFAULT_TOP + 'px');
        }

        // 未送信件数の表示位置を変更する
        $('#lbl_unSendCount').css('top', C_COUNT_LABEL_DEFAULT_TOP + 'px');


        // 来店人数ボタンを非表示にする処理
        $('#number').css('display', 'none');

        // 来店目的ボタンの1行を表示する
        changePurposeButtonOneLine();
    }

    /**
    * 来店目的ボタンを1段表示にする
    * 
    * @param {-} -
    * @return {-} -
    * 
    * @example 
    *  -
    */
    function changePurposeButtonOneLine() {

        $('.purposeButton').css('top', C_VISIT_PURPOSE_BUTTON_ONE_LINE_TOP + 'px');
    }

    /**
    * 来店目的ボタンを2段表示の上部に表示にする
    * 
    * @param {-} -
    * @return {-} -
    * 
    * @example 
    *  -
    */
    function changePurposeButtonTwoLine() {

        $('.purposeButton').css('top', C_VISIT_PURPOSE_BUTTON_TWO_LINE_TOP + 'px');
    }
    //2019/02/28 NSK 河谷 REQ-SVT-TMT-20180601-001 Gate Keeper機能の視認性操作性改善 END

    /**
    * スクロールランプの初期化を行う。
    * 
    * @param {-} - -
    * @return {-} -
    * 
    * @example 
    *  -
    */
    function initScrollLamp() {
        for (i = 0; i < $('.lampAria').size(); i++) {

            // 顧客件数を取得
            // $04 iOS7対応 START
            var count = $('#input1 > .lampAria').eq(i).parents('#contents1').find('.photo').children(0).eq(3).attr('value');
            // $04 iOS7対応 END

            // 顧客件数が最大件数以下の場合のみ先頭のランプを点灯させる。
            if (count <= 17) {
                $('#input1 > .lampAria').eq(i).children(0).eq(0).css('display', 'none');
                $('#input1 > .lampAria').eq(i).children(0).eq(1).css('display', 'block');
            }
        }
    }

    /**
    * 顧客情報エリアのサイズを変更を行う。
    * 
    * @param {-} - -
    * @return {-} -
    * 
    * @example 
    *  -
    */
    function initCustomerAria() {

        for (i = 0; i < $('.photo').size(); i++) {
            var count = $('.photo').eq(i).children(0).eq(3).attr('value');
            var size = count * 640;
            $('.photo').eq(i).css('width', size + 'px');
        }
    }

    //フリック関連
    var carInfoMaxIndex = $('#vclNoRead > .contents').length - 1;
    var carHeight = $('#vclNoRead > .contents:eq(0)').height();
    var carDispHeight = carInfoMaxIndex * carHeight;
    var isTouch = ('ontouchstart' in window);
    var isTouchStart = false;
    var isUpDuwn = false;
    var isCheakMove = false;
    var startY = 0;
    var startX = 0;
    var dragY = 0;
    var dragX = 0;
    var moveY = 0;
    var moveX = 0;
    var startScY = 0;
    var startScX = 0;
    var moveFlg = false;
    var maxDisplayNumber = parseInt($('#MaxDisplayCountNumber').val());
    var readInfoNumber = parseInt($('#NextOrPreviewDisplayCountNumber').val());
    var CurrentHeadInfo = parseInt($('#CurrentDisplayHeaderNumber').val());
    var unsentDataCount = parseInt($('#unsetDataCount').val());


    // 縦フリック
    $('#vclNoRead').bind({
        'touchstart mousedown': function (aEvent) {

            //2012/05/16 KN 浅野  【SALES_2】号口(課題No.131)対応 START
            // 慣性スクロール中は処理しない。
            if (moveFlg == true) {
                return;
            }
            //2012/05/16 KN 浅野  【SALES_2】号口(課題No.131)対応 END

            if (isTouch && aEvent.originalEvent.touches == undefined) {
                return;
            }

            // タッチ開始
            isTouchStart = true;

            // タップ時の位置取得
            startY = isTouch ? aEvent.originalEvent.touches[0].clientY : aEvent.clientY;
            startX = isTouch ? aEvent.originalEvent.touches[0].clientX : aEvent.clientX;

            // タップ時のスクロール位置を取得
            startScY = $(this).scrollTop();
        },

        'touchmove mousemove': function (aEvent) {

            aEvent.preventDefault();

            // タッチイベント中以外は反応させない。
            if (!isTouchStart) {
                return;
            }

            //2012/05/16 KN 浅野  【SALES_2】号口(課題No.131)対応 START
            // 慣性スクロール中は処理しない。
            if (moveFlg == true) {
                return;
            }
            //2012/05/16 KN 浅野  【SALES_2】号口(課題No.131)対応 END

            if (isTouch && aEvent.originalEvent.touches == undefined) {
                return;
            }

            //ドラッグ時の位置取得
            dragY = isTouch ? aEvent.originalEvent.touches[0].clientY : aEvent.clientY;
            dragX = isTouch ? aEvent.originalEvent.touches[0].clientX : aEvent.clientX;

            //上下左右判定
            if (!isCheakMove) {
                isCheakMove = true;
                if (Math.abs(startX - dragX) > Math.abs(startY - dragY)) {
                    // 左右時
                    isUpDuwn = false;
                }
                else {
                    // 上下時
                    isUpDuwn = true;
                }
            }

            // 上下フリック時以外は反応させない。
            if (!isUpDuwn) {
                return;
            }

            // タップ時から移動した距離を取得
            moveY = startY - dragY;

            // スクロール
            $(this).scrollTop(startScY + moveY);

            // 一番上の要素で下フリックをした場合
            var selectIndex = Number($('#selectVclNoIndex').attr('value'));

            if (selectIndex == 1) {
                if (moveY <= -150) {
                    if (!$('#downCursor').hasClass('step1')) {
                        $('#downCursor').css('display', 'block');
                        $('#downCursor').addClass('step1');
                        $('#pullDownString').css('display', 'none');
                        $('#releaseString').css('display', 'block');
                        $('#loadString').css('display', 'none');
                    }

                }
                else {
                    $('#downCursor').css('display', 'block');
                    $('#downCursor').removeClass('step1');
                    $('#pullDownString').css('display', 'block');
                    $('#releaseString').css('display', 'none');
                    $('#loadString').css('display', 'none');

                }
            }

        },

        'touchend mouseup': function (aEvent) {

            // タッチイベント中以外は反応させない。
            if (!isTouchStart) {
                return;
            }

            // 上下フリック時以外は反応させない。
            if (!isUpDuwn) {
                return;
            }

            //2012/05/16 KN 浅野  【SALES_2】号口(課題No.131)対応 START
            // 慣性スクロール中は処理しない。
            if (moveFlg == true) {
                return;
            }
            //2012/05/16 KN 浅野  【SALES_2】号口(課題No.131)対応 END

            // フラグ初期化
            isTouchStart = false;
            isUpDuwn = false;
            isCheakMove = false;

            // 選択されている車両登録Noの位置を取得
            var oldIndex = Number($('#selectVclNoIndex').attr('value'));
            var newIndex = 0;

            // フリックの上下判断
            if (moveY >= 0) {
                // 上フリック時
                if (oldIndex != carInfoMaxIndex - 1) {
                    newIndex = oldIndex + 1;
                }
                else {
                    newIndex = carInfoMaxIndex - 1;
                }
            }
            else {
                // 下フリック時
                if (oldIndex != 1) {
                    newIndex = oldIndex - 1;
                }
                else {
                    newIndex = 1;
                }
            }

            // 慣性スクロール
            var moveVal;

            if ((oldIndex == 1 && newIndex == 1) && (moveY <= -150)) {
                moveVal = carHeight - 150;
            }
            else {
                moveVal = newIndex * carHeight;
            }

            moveElement($(this), moveVal, 'Y');

            // 選択位置を保持
            $('#selectVclNoIndex').attr('value', newIndex);

            // 最初の要素で下フリックを行った場合
            if (oldIndex == 1 && newIndex == 1) {

                // 画面を再描画する。(フリック＆リリース)
                if (moveY <= -150) {

                    //2012/05/22 KN 浅野  クルクル対応 START
                    //タイマーセット
                    if (gIsReload == false) {
                        gIsReload = true;
                        // これ以前のtimerを無視する。
                        commonClearTimer();
                        commonRefreshTimer(
                        function () {
                            //リロード処理
                            $('#refreshButton').click();
                        }
                        );
                    }
                    //2012/05/22 KN 浅野  クルクル対応 END

                    // 初期読み込み
                    $('#initButton').click();

                    // ロード画面表示
                    $('#downCursor').css('display', 'none');
                    $('#downString').css('display', 'none');
                    $('#releaseString').css('display', 'none');
                    $('#loadCursor').css('display', 'block');
                    $('#loadString').css('display', 'block');
                }
                else {
                    return;
                }

                //最上位または最下位のレコードを表示したときの処理
            } else if (newIndex == maxDisplayNumber) {

                var currentDataNumber = CurrentHeadInfo + newIndex - 1;
                if (unsentDataCount > currentDataNumber) {
                    $('#lbl_NextDataCount').css('display', 'inline-block');
                }
            } else if (newIndex == 1 && CurrentHeadInfo != 1) {
                $('#lbl_PreviewDataCount').css('display', 'inline-block');
            } else {
                $('#lbl_NextDataCount').css('display', 'none');
                $('#lbl_PreviewDataCount').css('display', 'none');
            }






            // 慣性スクロール終了後に下記処理を実行させる。
            var action = null;
            action = setTimeout(
                    function () {

                        // 顧客情報選択位置を初期化
                        $('#selectCustIndex').attr('value', 0);

                        // 最左部を表示
                        moveElement($('#vclNoRead > .contents').eq(oldIndex).find('.slidemask'), moveVal, 'Y');

                        // スクロールランプの初期化
                        for (i = 0; i < $('#vclNoRead > .contents').eq(oldIndex).find('.lampAria div').size(); i++) {
                            // $04 iOS7対応 START
                            var count = $('#vclNoRead > .contents').eq(oldIndex).find('.photo').children(0).eq(3).attr('value');
                            // $04 iOS7対応 END
                            if (count <= 17) {

                                if (i == 0) {
                                    $('#vclNoRead > .contents').eq(oldIndex).find('.lampAria').children(0).eq(i).css('display', 'none');
                                }
                                else if (i == 1) {
                                    $('#vclNoRead > .contents').eq(oldIndex).find('.lampAria').children(0).eq(i).css('display', 'block');
                                }
                                else if ((i % 2) == 0) {
                                    $('#vclNoRead > .contents').eq(oldIndex).find('.lampAria').children(0).eq(i).css('display', 'block');
                                }
                                else {
                                    $('#vclNoRead > .contents').eq(oldIndex).find('.lampAria').children(0).eq(i).css('display', 'none');
                                }
                            }
                        }

                        // 来店人数ボタンの初期化
                        clearButton('number', 'ninzuButton', '');

                        // 来店目的ボタン初期化
                        clearButton('purpose', 'purposeButton', '');

                        //2019/02/28 NSK 河谷 REQ-SVT-TMT-20180601-001 Gate Keeper機能の視認性操作性改善 START
                        //// 送信ボタン非活性
                        //$('#btn_submit_o').css('display', 'none');
                        //$('#btn_submit_n').css('display', 'block');

                        // 来店人数ボタンを非表示にする
                        hideNumberButtonArea();
                        //2019/02/28 NSK 河谷 REQ-SVT-TMT-20180601-001 Gate Keeper機能の視認性操作性改善 END

                        // フラグ初期化
                        gIsSelectNinzuButton = false;
                        gIsSelectPurposeButton = false;
                        clearTimeout(action);

                    }
            //2012/05/16 KN 浅野  【SALES_2】号口(課題No.131)対応 START
            //, 260
                    , 160
            //2012/05/16 KN 浅野  【SALES_2】号口(課題No.131)対応 END
                );
        }
    });

    //横フリック
    $('.slidemask').bind({
        'touchstart mousedown': function (aEvent) {

            //2012/05/16 KN 浅野  【SALES_2】号口(課題No.131)対応 START
            // 慣性スクロール中は処理しない。
            if (moveFlg == true) {
                return;
            }
            //2012/05/16 KN 浅野  【SALES_2】号口(課題No.131)対応 END

            if (isTouch && aEvent.originalEvent.touches == undefined) {
                return;
            }

            // タッチ開始
            isTouchStart = true;

            // タップ時の位置取得
            startX = isTouch ? aEvent.originalEvent.touches[0].clientX : aEvent.clientX;

            // タップ時のスクロール位置を取得
            startScX = $(this).scrollLeft();
        },

        'touchmove mousemove': function (aEvent) {

            aEvent.preventDefault();

            // タッチイベント中以外は反応させない。
            if (!isTouchStart) {
                return;
            }

            // 上下フリック時は反応させない。
            if (isUpDuwn) {
                return;
            }

            //2012/05/16 KN 浅野  【SALES_2】号口(課題No.131)対応 START
            // 慣性スクロール中は処理しない。
            if (moveFlg == true) {
                return;
            }
            //2012/05/16 KN 浅野  【SALES_2】号口(課題No.131)対応 END

            if (isTouch && aEvent.originalEvent.touches == undefined) {
                return;
            }

            // タップ時から移動した距離を取得
            dragX = isTouch ? aEvent.originalEvent.touches[0].clientX : aEvent.clientX;
            moveX = startX - dragX;

            // スクロール
            $(this).scrollLeft(startScX + moveX);

        },

        'touchend mouseup': function (aEvent) {

            // タッチイベント中以外は反応させない。
            if (!isTouchStart) {
                return;
            }

            // 上下フリック時は反応させない。
            if (isUpDuwn) {
                return;
            }

            //2012/05/16 KN 浅野  【SALES_2】号口(課題No.131)対応 START
            // 慣性スクロール中は処理しない。
            if (moveFlg == true) {
                return;
            }
            //2012/05/16 KN 浅野  【SALES_2】号口(課題No.131)対応 END

            // フラグ初期化
            isTouchStart = false;
            isUpDuwn = false;
            isCheakMove = false;

            // 選択位置取得
            var oldIndex = Number($('#selectCustIndex').attr('value'));
            var newIndex = 0;
            // $04 iOS7対応 START
            var maxIndex = Number($(this).children('#photo1').children('#custCount').eq(0).val());
            // $04 iOS7対応 END

            //顧客情報がないときは処理なし
            if (maxIndex == 0) {
                return;
            }

            // フリックの左右判断
            if (moveX >= 0) {
                // 左フリック
                if (oldIndex != (maxIndex - 1)) {
                    newIndex = oldIndex + 1;
                }
                else {
                    return;
                }
            }
            else {
                // 右フリック
                if (oldIndex != 0) {
                    newIndex = oldIndex - 1;
                }
                else {
                    return;
                }
            }

            // 慣性スクロール
            var moveVal = newIndex * 640;
            moveElement($(this), moveVal, 'X');

            // 選択位置を保持
            $('#selectCustIndex').attr('value', newIndex);

            //スクロールランプの移動
            if (maxIndex <= 17) {
                var lampNewSec = newIndex * 2;
                var lampNewSec_n = lampNewSec + 1;
                var lampOldSec = oldIndex * 2;
                var lampOldSec_n = lampOldSec + 1;

                $(this).parents('#contents1').find('.lampAria div:eq(' + lampNewSec + ')').css('display', 'none');
                $(this).parents('#contents1').find('.lampAria div:eq(' + lampNewSec_n + ')').css('display', 'block');
                $(this).parents('#contents1').find('.lampAria div:eq(' + lampOldSec + ')').css('display', 'block');
                $(this).parents('#contents1').find('.lampAria div:eq(' + lampOldSec_n + ')').css('display', 'none');
            }
        }
    });
    //2018/02/19 NSK 河谷 REQ-SVT-TMT-20170615-001_iPod Gatekeeperに顧客の車両登録番号を入力する機能を追加 START
    //テキストボックス(フォーカスOutイベント追加)    
    $("#RegNumTxt").bind('focusout', function (aEvent) {

        // テキストボックスに1文字以上入力があった場合、
        // 車両登録番号入力有無フラグを「1：入力あり」とする
        if (0 < $(this).val().length) {

            gRegNumInputFlag = C_REGNUM_INPUT;

            // テキストボックスに入力が無い場合は、
            // 車両登録番号入力有無フラグを「0：入力なし」とする
        } else {

            gRegNumInputFlag = C_REGNUM_NOT_INPUT;
        }

        // ボタン制御関数を呼び出す
        changeButton('RegNumTxt', 'input_left', '');
    });
    //2018/02/19 NSK 河谷 REQ-SVT-TMT-20170615-001_iPod Gatekeeperに顧客の車両登録番号を入力する機能を追加 END

    /**
    * 要素の移動処理
    * 
    * @param {Object}  aElement  スクロール対象となる要素
    * @param {Integer} aPosition スクロール位置
    * @param {String}  aAxisType スクロールタイプ
    * @return {-} -
    * 
    * @example 
    *  -
    */
    function moveElement(aElement, aPosition, aAxisType) {

        var scrollPosY = 0;
        var scrollPosX = 0;

        // 縦横判断
        if (aAxisType == 'Y') {
            scrollPosY = aPosition;
            scrollPosX = 0;
        }
        else {
            scrollPosY = 0;
            scrollPosX = aPosition;
        }

        // 慣性スクロール
        if (moveFlg == false) {
            moveFlg = true;
            aElement.animate({
                scrollTop: scrollPosY + 'px',
                scrollLeft: scrollPosX + 'px'
            }, {
                //2012/05/16 KN 浅野  【SALES_2】号口(課題No.131)対応 START
                //duration: 250,
                duration: 150,
                //2012/05/16 KN 浅野  【SALES_2】号口(課題No.131)対応 END
                easing: 'swing',
                //2012/05/16 KN 浅野  【SALES_2】号口(課題No.131)対応 START
                complete: function () {
                    moveFlg = false;
                }
                //2012/05/16 KN 浅野  【SALES_2】号口(課題No.131)対応 END
            });
            //2012/05/16 KN 浅野  【SALES_2】号口(課題No.131)対応 START
            //moveFlg = false;
            //2012/05/16 KN 浅野  【SALES_2】号口(課題No.131)対応 END
        }
    }

    /**
    * 車アイコン押下時の制御を行う。
    * 
    * @param {-} - -
    * @return {-} -
    * 
    * @example 
    *  -
    */
    $('#btn_car').click(function () {

        // $05 start リロード中は無視
        if (gIsReload == true) {
            return;
        }
        //2012/05/22 KN 浅野  クルクル対応 START
        // ロード画面表示
        //$.master.OpenLoadingScreen();
        CarButtonCommon();
        //2012/05/22 KN 浅野  クルクル対応 END
    });

    $('#btn_car_o').click(function () {

        // $05 start リロード中は無視
        if (gIsReload == true) {
            return;
        }
        //2012/05/22 KN 浅野  クルクル対応 START
        // ロード画面表示
        //$.master.OpenLoadingScreen();
        CarButtonCommon();
        //2012/05/22 KN 浅野  クルクル対応 END
    });


    /**
    * 歩きアイコン押下時の制御を行う。
    * 
    * @param {-} - -
    * @return {-} -
    * 
    * @example 
    *  -
    */
    $('#btn_person').click(function () {

        // $05 start リロード中は無視
        if (gIsReload == true) {
            return;
        }
        //2012/05/22 KN 浅野  クルクル対応 START
        // ロード画面表示
        //$.master.OpenLoadingScreen();
        PersonButtonCommon();
        //2012/05/22 KN 浅野  クルクル対応 END
    });

    $('#btn_person_o').click(function () {

        // $05 start リロード中は無視
        if (gIsReload == true) {
            return;
        }
        //2012/05/22 KN 浅野  クルクル対応 START
        // ロード画面表示
        //$.master.OpenLoadingScreen();
        PersonButtonCommon();
        //2012/05/22 KN 浅野  クルクル対応 END
    });
    //2018/02/19 NSK 河谷 REQ-SVT-TMT-20170615-001_iPod Gatekeeperに顧客の車両登録番号を入力する機能を追加 START
    /**
    * 予約一覧アイコン押下時の制御を行う。
    * 
    * @param {-} - -
    * @return {-} -
    * 
    * @example 
    *  -
    */
    $('#btn_AppList').click(function () {

        // リロード中は無視
        if (gIsReload == true) {
            return;
        }

        //ボタン背景点灯
        $('#btn_AppList').css('display', 'none');
        $('#btn_AppList_o').css('display', 'block');
        setTimeout(function () {
            //ボタン背景を戻す
            $('#btn_AppList_o').css('display', 'none');
            $('#btn_AppList').css('display', 'block');

            // ロード画面表示
            $.master.OpenLoadingScreen();

            gIsReload = true;
            //タイマーセット
            commonRefreshTimer(
                function () {
                    //リロード処理
                    $('#refreshButton').click();
                }
            );

            $('#ReserveListButton').click();

        }, 300);
    });
    //2018/02/19 NSK 河谷 REQ-SVT-TMT-20170615-001_iPod Gatekeeperに顧客の車両登録番号を入力する機能を追加 END

    //2012/05/22 KN 浅野  クルクル対応 START
    /**
    * 車アイコン押下時の共通処理を行う。
    * 
    * @param {-} - -
    * @return {-} -
    * 
    * @example 
    *  -
    */
    function CarButtonCommon() {

        // ロード画面表示
        $.master.OpenLoadingScreen();

        //タイマーセット
        if (gIsReload == false) {
            gIsReload = true;
            // これ以前のtimerを無視する。
            commonClearTimer();
            commonRefreshTimer(
                function () {
                    //リロード処理
                    $('#CarButton').click();
                }
            );
        }

        $('#CarButton').click();
    }


    /**
    * 歩きアイコン押下時の共通処理を行う。
    * 
    * @param {-} - -
    * @return {-} -
    * 
    * @example 
    *  -
    */
    function PersonButtonCommon() {


        // ロード画面表示
        $.master.OpenLoadingScreen();

        //タイマーセット
        if (gIsReload == false) {
            gIsReload = true;
            // これ以前のtimerを無視する。
            commonClearTimer();
            commonRefreshTimer(
                function () {
                    //リロード処理
                    $('#PersonButton').click();
                }
            );
        }

        $('#PersonButton').click();
    }
    //2012/05/22 KN 浅野  クルクル対応 END

    //2019/02/28 NSK 河谷 REQ-SVT-TMT-20180601-001 Gate Keeper機能の視認性操作性改善 START
    ///**
    //* 送信ボタン押下時の制御を行う。
    //* 
    //* @param {-} - -
    //* @return {-} -
    //* 
    //* @example 
    //*  -
    //*/
    //$('#btn_submit_o').click(function () {
    //
    //    // $05 start リロード中は無視
    //    if (gIsReload == true) {
    //        return;
    //    }
    //
    //    //2018/02/19 NSK 河谷 REQ-SVT-TMT-20170615-001_iPod Gatekeeperに顧客の車両登録番号を入力する機能を追加 START
    //    // 新規登録画面(車)の場合、確認ダイアログを表示
    //    if ($('#dispType').attr('value') == '3' && gCurrentId.indexOf('other') == -1) {
    //
    //        var result = window.confirm($("#ConfirmMessageText").val());
    //
    //        // 確認ダイアログではいが選択された場合、
    //        // 手入力した車両登録番号をHiddenFieldに格納する
    //        if (result) {
    //
    //            document.getElementById("InputRegNumber").value = $('#RegNumTxt').val();
    //
    //            // 車両登録番号入力欄に入力した文字列を初期化
    //            $('#RegNumTxt').attr('value', "");
    //        }
    //        // 確認ダイアログでいいえが選択された場合、
    //        // 以降の処理は行わない
    //        else {
    //
    //            return;
    //        }
    //    }
    //    //2018/02/19 NSK 河谷 REQ-SVT-TMT-20170615-001_iPod Gatekeeperに顧客の車両登録番号を入力する機能を追加 END
    //
    //    // 送信ボタン非活性状態へ
    //    $('#btn_submit_o').css('display', 'none');
    //    $('#btn_submit_n').css('display', 'block');
    //
    //    // ロード画面表示
    //    $.master.OpenLoadingScreen();
    //
    //    //2012/05/22 KN 浅野  クルクル対応 START
    //
    //    //タイマーセット
    //    if (gIsReload == false) {
    //        gIsReload = true;
    //        // これ以前のtimerを無視する。
    //        commonClearTimer();
    //        commonRefreshTimer(
    //                function () {
    //                    //リロード処理
    //                    $('#refreshButton').click();
    //                }
    //            );
    //    }
    //
    //    // 送信処理
    //    $('#submitButton').click();
    //    //2012/05/22 KN 浅野  クルクル対応 END
    //
    //});

    /**
    * 送信処理を行う。
    * 
    * @param {-} - -
    * @return {-} -
    * 
    * @example 
    *  -
    */
    function submit() {

        // リロード中は無視
        if (gIsReload == true) {
            return;
        }

        // 確認ダイアログを表示
        var result = window.confirm($("#ConfirmMessageText").val());

        // 確認ダイアログではいが選択された場合、かつ、新規入力画面(車)の場合、
        // 手入力した車両登録番号をHiddenFieldに格納する
        if (result) {

            if ($('#dispType').attr('value') == '3' && gCurrentId.indexOf('other') == -1) {
                document.getElementById("InputRegNumber").value = $('#RegNumTxt').val();

                // 車両登録番号入力欄に入力した文字列を初期化
                $('#RegNumTxt').attr('value', "");
            }
        }
        // 確認ダイアログでいいえが選択された場合、
        // 以降の処理は行わない
        else {

            // 来店人数ボタンが表示されている場合、来店人数ボタンを非表示にする
            if ($('#number').css('display') == 'block') {

                hideNumberButtonArea();

                // 来店人数ボタンの選択状態を初期化する
                clearButton('number', 'ninzuButton', '');
            }

            // 来店目的ボタンの選択状態を初期化する
            clearButton('purpose', 'purposeButton', '');

            // フラグ初期化
            gIsSelectNinzuButton = false;
            gIsSelectPurposeButton = false;

            return;
        }

        // ロード画面表示
        $.master.OpenLoadingScreen();

        //タイマーセット
        if (gIsReload == false) {
            gIsReload = true;
            // これ以前のtimerを無視する。
            commonClearTimer();
            commonRefreshTimer(
                    function () {
                        //リロード処理
                        $('#refreshButton').click();
                    }
                );
        }

        // 送信処理
        $('#submitButton').click();
    }
    //2019/02/28 NSK 河谷 REQ-SVT-TMT-20180601-001 Gate Keeper機能の視認性操作性改善 END


    //$05 削除ポップアップを閉じる
    $('#BaseBox').click(function () {
        if (delDispFlag == 2) {
            delDispFlag = 1;
            return;
        } else if (delDispFlag == 1) {
            delDispFlag = 0;
            $('#popOver_Delete_popover').css("display", "none");
            return;
        }
    });

    $('#MstPG_IcropIcon').click(function () {
        if (delDispFlag == 1) {
            delDispFlag = 0;
            $('#popOver_Delete_popover').css("display", "none");
            return;
        }
    });

    //$05 画面タイトルタップ時処理
    $('#MstPG_TitleLabel').click(function () {


        if (gIsReload == true) {
            return;
        }

        // 未送信データが存在するときのみ表示させる
        if (parseInt($('#unsetDataCount').val()) < 1) {
            $('#popOver_Delete_popover').css("display", "none");
            return;
        }
        if (delDispFlag == 1) {
            delDispFlag = 0;
            $('#popOver_Delete_popover').css("display", "none");
        } else {
            delDispFlag = 2;
            $('#popOver_Delete_popover').css("display", "block");
        }
    });

    //$05 削除ボタン押下時の処理
    $('#AllDeleteButton_Click').click(function () {


        // 「OK」時の処理開始 ＋ 確認ダイアログの表示
        if (window.confirm($('#AllDeleteText').val())) {
            $.master.OpenLoadingScreen();
            $('#DeleteButton').click();

        }
        // 「OK」時の処理終了

        // 「キャンセル」時の処理開始
        else {
            return;

        }
        // 「キャンセル」時の処理終了

    });


    //$05 次のN件ボタン押下時の処理
    $('#lbl_NextDataCount').click(function () {

        // $05 start リロード中は無視
        if (gIsReload == true) {
            return;
        }
        gIsReload = true;
        // ロード画面表示
        $.master.OpenLoadingScreen();
        $('#NextDataShowButton').click();


    });

    //$05 前のN件ボタン押下時の処理
    $('#lbl_PreviewDataCount').click(function () {

        // $05 start リロード中は無視
        if (gIsReload == true) {
            return;
        }
        gIsReload = true;
        $.master.OpenLoadingScreen();
        $('#PreviewDataShowButton').click();

    });

});

/**
* Push機能よりゲート通知受け取り時の処理を行う。
* 
* @param {-} - -
* @return {-} -
* 
* @example 
*  -
*/
function sc3090301pushRecv() {

    //2012/06/04 KN 浅野  クルクル対応(検証不具合修正) START
    // リロード中は、Push受信を無視する。
    if (gIsReload == true) {

        //Cookieへリロード要求フラグを保持させる。
        document.cookie = "IsReload=1;";

        //処理しない。
        return;
    }
    //2012/06/04 KN 浅野  クルクル対応(検証不具合修正) END

    var dispType = $('#dispType').attr('value');

    // 新規登録画面[車]又は新規登録画面[歩き]
    if (dispType == '3' || dispType == '4') {

        //処理しない。
        return;
    }

    // 画面操作中
    if (gIsSelectNinzuButton || gIsSelectPurposeButton) {

        //処理しない。
        return;
    }

    //$05 キャンセルポップアップ表示中
    if (delDispFlag != 0) {

        //処理しない。
        return;
    }

    // 画面再描画
    $('#initButton').click();
}
