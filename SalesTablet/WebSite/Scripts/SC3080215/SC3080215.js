/*━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
SC3080215.js
─────────────────────────────────────
機能： CSSurvey一覧・詳細
補足： 
作成： 2012/02/20 TCS 明瀬
更新： 2012/04/13 TCS 明瀬 HTMLエンコード対応
更新： 2014/04/21 TCS 市川 GTMCタブレット高速化対応（BTS-386）
更新： 2014/11/04 TCS 藤井 iOS8 対応(i-CROP_V4_salesよりマージ) 
更新： 2019/06/06 TS  重松 ポップアップの表示位置を制御 上下の表示設定を消去（UAT-0504）
─────────────────────────────────────*/

/**
* コールバック関数定義
* 
* @param {String} method コールバック呼び出し時に実行するメソッド名
* @param {String} argument 引き渡すパラメータ（カンマ区切り））
* @param {String} callbackFunction コールバック後に実行するメソッド
* 
*/
var CallbackSC3080215 = {
    doCallback: function (method, argument, callbackFunction) {
        this.method = method;
        this.argument = argument;
        this.packedArgument = method + "," + argument;
        this.endCallback = callbackFunction;
        this.beginCallback();
    }
};

/**
* @class 定数
*/
var constants = {
    messageIdSys: "9999",               //メッセージID：異常終了（その他エラー）
    pageTypeList: "list",
    pageTypeDetail: "detail"
}

//2012/04/13 TCS 明瀬 HTMLエンコード対応 Start
/**
* HTMLエンコードを行う関数
* 
* @param {String} value HTMLエンコード対象文字列
* @return {String} HTMLエンコード処理後の文字列
* 
* @example 
* HtmlEncodeSC3080215("<br>");
* 出力:「&lt;br&gt;」
*/
function HtmlEncodeSC3080215(value) {
    return $('<div/>').text(value).html();
}
//2012/04/13 TCS 明瀬 HTMLエンコード対応 End

/**
* @class ポップオーバーフォーム
*/
var popForm;

/**
* @class １ページ目のコンテンツ保持用
*/
var SC3080215Html1;

function popOverFormSC3080215() {
    $("#CSSurveyPopOverForm").TCSPopOverForm({

        open: function (pop, elem) {
            popForm = pop;
            
			//2014/04/21 TCS市川 GTMCタブレット高速化対応 START
			//スクリプトの遅延読み込み
			SC3080201.requirePartialScript("../Scripts/SC3080215/SC3080215.Popup.js", function () {
			//2014/04/21 TCS市川 GTMCタブレット高速化対応 END
			
	            //コンテンツ保持用変数初期化
	            SC3080215Html1 = '';

	            //処理中表示開始
	            SC3080215.startServerCallback();

	            //ヘッダータイトル削除
	            $('#CSSurveyTitleLabel').text('');
	            //１ページ目のコンテンツを削除
	            $('#CSSurveyPage1>div').remove();
	            //２ページ目（アンケート詳細）のコンテンツを削除
	            $('#CSSurveyPage2>div').remove();

	            if ($("#answerIdHidden").val() == "" || $('#SC3080215FirstOpenHidden').val() == "1") {
	                //アンケート一覧画面の作成
	                CreateCSSurveyListWindow();
	            } else {
	                //アンケート一覧・詳細画面の作成
	                CreateCSSurveyAllWindow();

	                //詳細画面の直接表示を一度しか通らないようにするフラグを立てる
	                $('#SC3080215FirstOpenHidden').val("1");
	            }
			//2014/04/21 TCS市川 GTMCタブレット高速化対応 START
            });
			//2014/04/21 TCS市川 GTMCタブレット高速化対応 END
        },
        render: function (pop, index, args, container, header) {
        },

        //        close: function(){
        //            $("#registOverlayBlackSC3080215").css("display", "none");
        //        },

        preventLeft: true,
        preventRight: false,
        //2019/06/06 TS 重松 ポップアップの表示位置を制御 上下の表示設定を消去（UAT-0504）DELETE
        elasticConstant: 0.3,
        id: "SC3080215PopOver",
        paddingTop: 65
    });
};

//2014/04/21 TCS市川 GTMCタブレット高速化対応 DELETE

/**
* 初期処理
*/
(function (window) {

    $.extend(window, { SC3080215: {} });
    $.extend(SC3080215, {

        /**
        * コールバック開始
        */
        startServerCallback: function () {
            SC3080215.showLoding();
        },

        /**
        * コールバック終了
        */
        endServerCallback: function () {
            SC3080215.closeLoding();
        },

        /******************************************************************************
        * 読み込み中表示
        ******************************************************************************/

        /**
        * 読み込み中アイコン表示
        */
        showLoding: function () {

            //オーバーレイ表示
            $("#registOverlayBlackSC3080215").css("display", "block");
            //アニメーション
            setTimeout(function () {
                $("#processingServerSC3080215").addClass("show");
                $("#registOverlayBlackSC3080215").addClass("open");
            }, 0);

        },

        /**
        * 読み込み中アイコンを非表示にする
        */
        closeLoding: function () {
            $("#processingServerSC3080215").removeClass("show");
            //2014/11/04 TCS 藤井 iOS8 対応(i-CROP_V4_salesよりマージ)  Start
            $("#registOverlayBlackSC3080215").removeClass("open");
            setTimeout(function () {
                $("#registOverlayBlackSC3080215").css("display", "none");
            }, 300);
            //2014/11/04 TCS 藤井 iOS8 対応(i-CROP_V4_salesよりマージ)  End
        }
    });

})(window);

