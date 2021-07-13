/*━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
SC3080215.Popup.js
─────────────────────────────────────
機能： CSSurvey一覧・詳細
補足： CSSurvey一覧・詳細PopUpを開くタイミングにて遅延ロードする
作成： 2014/04/21 TCS 市川 GTMCタブレット高速化対応（BTS-386）
─────────────────────────────────────*/

/**
* アンケート一覧画面を作成する
* 
*/
function CreateCSSurveyListWindow() {

    var prms = '';

    //１ページ目のコンテンツを削除
    $('#CSSurveyPage1>div').remove();

    CallbackSC3080215.doCallback('CreateCSSurveyList', prms, function (result, context) {

        var resArray = result.split("|");

        //処理結果の判定
        if (resArray[0] == constants.messageIdSys) {
            /*****************************************
            * 異常終了
            *****************************************/
            SC3080215.endServerCallback();
            alert(resArray[1]);
            popForm.closePopOver();
        } else {
            //画面の初期化 
            InitListWindowSC3080215(result, context);
            //処理中表示終了
            SC3080215.endServerCallback();
        }
    });
}

/**
* アンケート一覧画面の初期化を行う
* 
* @param {String} result 
* @param {String} context 
* 
*/
function InitListWindowSC3080215(result, context) {

    //コールバックによって取得したアンケート画面のHTMLを格納
    var contents = $('<Div>').html(result).text();

    //１ページ目のコンテンツを取得
    var csSurveyList = $(contents).find('#CSSurveyPage1');

    //１ページ目のコンテンツを削除
    $('#CSSurveyPage1>div').remove();

    //１ページ目のコンテンツを設定
    csSurveyList.children('div').clone(true).appendTo('#CSSurveyPage1');

    //２ページ目（アンケート詳細）のコンテンツを削除
    $('#CSSurveyPage2>div').remove();

    //アンケート一覧画面に上下スクロールの設定
    $('#CSSurveyListScroll').fingerScroll();

    //***** クラス名でHTML操作を行うと他画面に影響があるため変更 Start *****
    //    $(".ui-flickable-wrapper").css("background-color", "rgba(0, 0, 0, 0)");
    $("#SC3080215PopOver .content .ui-flickable-wrapper").css("background-color", "rgba(0, 0, 0, 0)");
    //***** クラス名でHTML操作を行うと他画面に影響があるため変更 End *****

    //アンケート一覧画面のヘッダータイトル文言を設定
    //2012/04/13 TCS 明瀬 HTMLエンコード対応 Start    
    $('#CSSurveyTitleLabel').text($(contents).find("#SC3080215Word0001Hidden").val());
    //2012/04/13 TCS 明瀬 HTMLエンコード対応 End
}

/**
* アンケート一覧でアンケートを選択したときの処理を行う
* 
* @param {-} - -
* @return {-} -
* 
* @example 
*  -
*/
function selectCSServeyList(listRowId) {

    //2度押し防止
    if ($("#SC3080215SelectedFlgHidden").val() === "1") {
        return false;
    }

    //アンケート2度押し防止フラグを立てる
    $("#SC3080215SelectedFlgHidden").val("1");

    //処理中表示開始
    SC3080215.startServerCallback();

    //タップされたliタグの属性から値を取得
    var answerId = $(listRowId).attr("answerid");
    var paperName = $(listRowId).attr("papername");
    var iconFileName = $(listRowId).attr("iconfilename");
    var staffName = $(listRowId).attr("staffname");
    var seriesName = $(listRowId).attr("seriesname");
    var vclRegNo = $(listRowId).attr("vclregno");
    var dateWord = $(listRowId).attr("dateword");

    var prms = '';
    prms = prms + encodeURIComponent(answerId) + ',';
    prms = prms + encodeURIComponent(paperName) + ',';
    prms = prms + encodeURIComponent(iconFileName) + ',';
    prms = prms + encodeURIComponent(staffName) + ',';
    prms = prms + encodeURIComponent(seriesName) + ',';
    prms = prms + encodeURIComponent(vclRegNo) + ',';
    prms = prms + encodeURIComponent(dateWord) + ',';

    CallbackSC3080215.doCallback('CreateCSSurveyDetail', prms, function (result, context) {

        var resArray = result.split("|");

        //処理結果の判定
        if (resArray[0] == constants.messageIdSys) {
            /*****************************************
            * 異常終了
            *****************************************/
            //処理中表示終了
            SC3080215.endServerCallback();

            //アンケート2度押し防止フラグを下げる
            $("#SC3080215SelectedFlgHidden").val("0");

            //エラーメッセージ表示
            alert(resArray[1]);

            popForm.closePopOver();
        } else {
            //画面の初期化 
            InitDetailWindowSC3080215(result, context);

            //２ページ目に移動する
            popForm.pushPage();

            //ヘッダーの左ボタン定義（pushPageメソッドの直後に書かないと、自動でBackボタンが生成されてしまう）
            //***** クラス名でHTML操作を行うと他画面に影響があるため変更 Start *****
            //            $('.icrop-PopOverForm-header-left').unbind('click');
            //            popForm.headerElement.find(".icrop-PopOverForm-header-left").removeClass("icrop-PopOverForm-header-back");
            //            $('.icrop-PopOverForm-header-left').empty().html('<a href="#" runat="server" class="CSSurveyEllipsis" style="width:65px; font-size:0.85em;">' + $("#SC3080215Word0001Hidden").val() + '</a><span class="tgLeft">&nbsp;</span>');
            //            $('.icrop-PopOverForm-header-left').css('display', 'block');
            $('#CSSurveyBackButton').unbind('click');
            popForm.headerElement.find("#CSSurveyBackButton").removeClass("icrop-PopOverForm-header-back");
            //2012/04/13 TCS 明瀬 HTMLエンコード対応 Start            
            var backWord = $($('<Div>').html(result).text()).find("#SC3080215Word0001Hidden").val();
            $('#CSSurveyBackButton').empty().html('<a href="#" id="CSSurveyBackButtonWord" runat="server" class="CSSurveyEllipsis" style="width:65px; font-size:0.85em;">' + HtmlEncodeSC3080215(backWord) + '</a><span class="tgLeft">&nbsp;</span>');
            //2012/04/13 TCS 明瀬 HTMLエンコード対応 End
            $('#CSSurveyBackButton').css('display', 'block');
            //***** クラス名でHTML操作を行うと他画面に影響があるため変更 End *****

            //第２ヘッダータップ防止のオーバーレイ表示
            $('#detailheadsetOverlay1').css('display', 'block');
            $('#detailheadsetOverlay2').css('display', 'block');

            //処理中表示終了
            SC3080215.endServerCallback();

            //アンケート2度押し防止フラグを下げる
            $("#SC3080215SelectedFlgHidden").val("0");
        }
    });
}

/**
* アンケート詳細画面の初期化を行う
* 
* @param {String} result 
* @param {String} context 
* 
*/
function InitDetailWindowSC3080215(result, context) {

    //コールバックによって取得したアンケート画面のHTMLを格納
    var contents = $('<Div>').html(result).text();

    //２ページ目（アンケート詳細）のコンテンツを取得
    var csSurveyDetail = $(contents).find('#CSSurveyPage2');

    //２ページ目（アンケート詳細）のコンテンツを削除
    $('#CSSurveyPage2>div').remove();

    //２ページ目（アンケート詳細）のコンテンツを設定
    csSurveyDetail.children('div').clone(true).appendTo('#CSSurveyPage2');

    //アンケート詳細画面に上下スクロールの設定
    $('#CSSurveyDetailScroll').fingerScroll();

    //スクロール領域の背景色を透明にする
    //***** クラス名でHTML操作を行うと他画面に影響があるため変更 Start *****
    //    $(".ui-flickable-wrapper").css("background-color", "rgba(0, 0, 0, 0)");
    $("#SC3080215PopOver .content .ui-flickable-wrapper").css("background-color", "rgba(0, 0, 0, 0)");
    //***** クラス名でHTML操作を行うと他画面に影響があるため変更 End *****

    //アンケート一覧画面のヘッダータイトル文言を設定
    $('#CSSurveyTitleLabel').text($(contents).find("#paperNameHidden").val());
}

/**
* アンケート一覧・詳細画面を作成する
* 
*/
function CreateCSSurveyAllWindow() {

    var prms = '';

    //１ページ目のコンテンツを削除
    $('#CSSurveyPage1>div').remove();

    //２ページ目のコンテンツを削除
    $('#CSSurveyPage2>div').remove();

    CallbackSC3080215.doCallback('CreateCSSurveyAll', prms, function (result, context) {

        var resArray = result.split("|");

        //処理結果の判定
        if (resArray[0] == constants.messageIdSys) {
            /*****************************************
            * 異常終了
            *****************************************/
            SC3080215.endServerCallback();
            alert(resArray[1]);
            popForm.closePopOver();
        } else {
            //画面の初期化 
            InitAllWindowSC3080215(result, context);

            //２ページ目に移動する
            popForm.pushPage();

            //***** クラス名でHTML操作を行うと他画面に影響があるため変更 Start *****
            //ヘッダーの左ボタン定義（pushPageメソッドの直後に書かないと、自動でBackボタンが生成されてしまう）
            //            $('.icrop-PopOverForm-header-left').unbind('click');
            //            popForm.headerElement.find(".icrop-PopOverForm-header-left").removeClass("icrop-PopOverForm-header-back");
            //            $('.icrop-PopOverForm-header-left').empty().html('<a href="#" runat="server" class="CSSurveyEllipsis" style="width:70px; font-size:0.85em;">' + $("#SC3080215Word0001Hidden").val() + '</a><span class="tgLeft">&nbsp;</span>');
            //            $('.icrop-PopOverForm-header-left').css('display', 'block');
            $('#CSSurveyBackButton').unbind('click');
            popForm.headerElement.find("#CSSurveyBackButton").removeClass("icrop-PopOverForm-header-back");
            //2012/04/13 TCS 明瀬 HTMLエンコード対応 Start            
            var backWord = $($('<Div>').html(result).text()).find("#SC3080215Word0001Hidden").val();
            $('#CSSurveyBackButton').empty().html('<a href="#" id="CSSurveyBackButtonWord" runat="server" class="CSSurveyEllipsis" style="width:65px; font-size:0.85em;">' + HtmlEncodeSC3080215(backWord) + '</a><span class="tgLeft">&nbsp;</span>');            
            //2012/04/13 TCS 明瀬 HTMLエンコード対応 End
            $('#CSSurveyBackButton').css('display', 'block');
            //***** クラス名でHTML操作を行うと他画面に影響があるため変更 End *****

            //第２ヘッダータップ防止のオーバーレイ表示
            $('#detailheadsetOverlay1').css('display', 'block');
            $('#detailheadsetOverlay2').css('display', 'block');

            //処理中表示終了
            SC3080215.endServerCallback();
        }
    });
}

/**
* アンケート一覧・詳細画面の初期化を行う
* 
* @param {String} result 
* @param {String} context 
* 
*/
function InitAllWindowSC3080215(result, context) {

    //コールバックによって取得したアンケート画面のHTMLを格納
    var contents = $('<Div>').html(result).text();

    //１ページ目のコンテンツを取得
    //var csSurveyList = $(contents).find('#CSSurveyPage1');
    SC3080215Html1 = $(contents).find('#CSSurveyPage1');

    //２ページ目（アンケート詳細）のコンテンツを取得
    var csSurveyDetail = $(contents).find('#CSSurveyPage2');

    //１ページ目のコンテンツを削除
    $('#CSSurveyPage1>div').remove();

    //    //１ページ目のコンテンツを設定
    //    csSurveyList.children('div').clone(true).appendTo('#CSSurveyPage1');
    //１ページ目は空白のページを設定する
    $('<div id="CSSurveyEmpty" style="width:480px; height:550px;"></div>').appendTo('#CSSurveyPage1');

    //２ページ目（アンケート詳細）のコンテンツを削除
    $('#CSSurveyPage2>div').remove();

    //２ページ目（アンケート詳細）のコンテンツを設定
    csSurveyDetail.children('div').clone(true).appendTo('#CSSurveyPage2');

    //アンケート詳細画面に上下スクロールの設定
    $('#CSSurveyDetailScroll').fingerScroll();

    //スクロール領域の背景色を透明にする
    //***** クラス名でHTML操作を行うと他画面に影響があるため変更 Start *****
    //    $(".ui-flickable-wrapper").css("background-color", "rgba(0, 0, 0, 0)");
    $("#SC3080215PopOver .content .ui-flickable-wrapper").css("background-color", "rgba(0, 0, 0, 0)");
    //***** クラス名でHTML操作を行うと他画面に影響があるため変更 End *****

    //アンケート一覧画面のヘッダータイトル文言を設定
    $('#CSSurveyTitleLabel').text($(contents).find("#paperNameHidden").val());
}

/**
* アンケート詳細で戻るボタンを選択したときの処理を行う
* 
* @param {-} - -
* @return {-} -
* 
* @example 
*  -
*/
function moveToCSServeyList() {

    //戻るボタン非活性
    $('#CSSurveyBackButton').attr('disabled', 'disabled');

    //通知から直接本画面が開かれ、詳細から一覧に戻るとき(１回目のみ)
    if (SC3080215Html1 != "") {
        //１ページ目のコンテンツを削除
        $('#CSSurveyPage1>div').remove();

        //１ページ目のコンテンツを設定
        SC3080215Html1.children('div').clone(true).appendTo('#CSSurveyPage1');

        SC3080215Html1 = '';
    }

    //アンケート一覧画面に上下スクロールの設定
    $('#CSSurveyListScroll').fingerScroll();

    //スクロール領域の背景色を透明にする
    //***** クラス名でHTML操作を行うと他画面に影響があるため変更 Start *****
    //    $(".ui-flickable-wrapper").css("background-color", "rgba(0, 0, 0, 0)");
    $("#SC3080215PopOver .content .ui-flickable-wrapper").css("background-color", "rgba(0, 0, 0, 0)");
    //***** クラス名でHTML操作を行うと他画面に影響があるため変更 End *****

    //アンケート一覧画面のヘッダータイトル文言を設定
    //2012/04/13 TCS 明瀬 HTMLエンコード対応 Start    
    $('#CSSurveyTitleLabel').text($('#CSSurveyBackButtonWord').text());
    //2012/04/13 TCS 明瀬 HTMLエンコード対応 End

    //２ページ目（表示領域）のコンテンツを削除
    $('#CSSurveyPage2>div').remove();

    //ポップページをするたびに追加されるため、削除
    //***** クラス名でHTML操作を行うと他画面に影響があるため変更 Start *****
    //$('.icrop-PopOverForm-header-left').remove();
    //$('.icrop-PopOverForm-header-right').remove();
    $('#CSSurveyBackButton').remove();
    $('#CSSurveyHeaderRight').remove();
    //***** クラス名でHTML操作を行うと他画面に影響があるため変更 End *****

    //アンケート一覧に戻る
    popForm.popPage();

    //第２ヘッダータップ防止のオーバーレイ表示
    $('#detailheadsetOverlay1').css('display', 'none');
    $('#detailheadsetOverlay2').css('display', 'none');

}

