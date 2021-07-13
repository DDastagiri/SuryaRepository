//━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
//SC3080401.js
//─────────────────────────────────────
//機能： ヘルプ依頼
//補足： 
//作成： 
//更新： 2014/04/21 TCS 市川 GTMCタブレット高速化対応（BTS-386）
//更新： 2014/11/04 TCS 藤井 iOS8 対応(i-CROP_V4_salesよりマージ)
//─────────────────────────────────────

/**
* コールバック関数定義
* 
* @param {String} method コールバック呼び出し時に実行するメソッド名
* @param {String} argument 引き渡すパラメータ（カンマ区切り））
* @param {String} callbackFunction コールバック後に実行するメソッド
* 
*/
var CallbackSC3080401 = {
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
	trueString: "True",                 //Trueの文字列
	falseString: "False",               //Falseの文字列
	messageIdSuccess: "0",              //メッセージID：正常終了
	messageIdDbTimeOut: "9001",         //メッセージID：異常終了（DBタイムアウト）
	messageIdSys: "9999",               //メッセージID：異常終了（その他エラー）
	presenceCategoryOffline: "4"        //在籍状態（大分類）：オフライン
}


/**
* ヘルプ依頼画面
* @return {Object}
*/
var popForm;

//2012/04/12 TCS 鈴木(健) HTMLエンコード対応 START
/**
* 文言
* @return {Object}
*/
var SC3080401Word0001;
var SC3080401Word0002;
var SC3080401Word0003;
var SC3080401Word0004;
var SC3080401Word0005;
var SC3080401Word0006;
var SC3080401Word0007;
var SC3080401Word0008;
var SC3080401Word0009;
var SC3080401Word9001;
//2012/04/12 TCS 鈴木(健) HTMLエンコード対応 END

$(function () {
    /**
    * ヘルプ依頼画面作成
    * 
    * @param {String} method コールバック呼び出し時に実行するメソッド名
    * @param {String} argument 引き渡すパラメータ（カンマ区切り））
    * @param {String} callbackFunction コールバック後に実行するメソッド
    * 
    */
    $("#HelpRequestPopOverForm").TCSPopOverForm({
        open: function (pop, elem) {

            popForm = pop;

            //処理中表示開始
            SC3080401.startServerCallback();

            //ヘルプ依頼画面の作成
            CreateHelpRequestWindow();
        },
        render: function (pop, index, args, container, header) {
            if (index == 0) {
                //ヘッダーのキャンセルボタンを定義
                // 2012/03/13 TCS 鈴木(健) 【SALES_2】ポップオーバーフォームのキャンセルボタンが重複する問題の修正 START
                //$('.icrop-PopOverForm-header-left').empty().html('<a href="#" id="HeaderCancelButton" class="helpRequestPopUpCancelButton helpRequestUseCut"></a>')
                //$('#HeaderCancelButton').text($('#WordNo0002HiddenField').val());
                //2012/03/22 TCS 明瀬【SALES_2】TCS_0321ks_01対応 Mod Start
                //2012/04/12 TCS 鈴木(健) HTMLエンコード対応 START
                $('#helpRequestHeaderLeft').empty().html('<a href="#" id="HeaderCancelButton" class="helpRequestPopUpCancelButton helpRequestUseCut">' + SC3080401HTMLEncode($('#WordNo0002PreHiddenField').text()) + '</a>')
                //2012/03/22 TCS 明瀬【SALES_2】TCS_0321ks_01対応 Mod End
                // 2012/03/13 TCS 鈴木(健) 【SALES_2】ポップオーバーフォームのキャンセルボタンが重複する問題の修正 END
                $('#HeaderTitle').text($('#WordNo0001PreHiddenField').text());
                //2012/04/12 TCS 鈴木(健) HTMLエンコード対応 END
                $('.helpRequestPopUpCancelButton').bind('click', function (e) {
                    pop.closePopOver();
                });

                //メインコンテンツのマージンを設定（テスト）
                var targetElement = $('#SC3080401PopOver').children('div')[1];
                if (targetElement != null) {
                    if (targetElement.style.margin == "") {
                        targetElement.style.margin = '5px';
                    }
                }
            }
        },
        preventLeft: true,
        preventRight: true,
        preventTop: false,
        preventBottom: true,
        elasticConstant: 0.3,
        id: "SC3080401PopOver"
    });
});

//2014/04/21 TCS市川 GTMCタブレット高速化対応 DELETE(SC3080401.Popup.jsへ移動）

/**
* ヘルプ依頼画面を作成する.
* 
*/
function CreateHelpRequestWindow() {

	//2014/04/21 TCS市川 GTMCタブレット高速化対応 START
	//スクリプトの遅延読み込み
    SC3080201.requirePartialScript("../Scripts/SC3080401/SC3080401.Popup.js", function () {
	//2014/04/21 TCS市川 GTMCタブレット高速化対応 END

	    var prms = '';

	    //メイン画面のコンテンツを削除
	    $('#HelpRequestMain>div').remove();

	    //画面初期化情報を取得する
	    CallbackSC3080401.doCallback('CreateHelpRequestWindow', prms, function (result, context) {

	        //処理結果の判定
	        switch (result) {
	            //異常終了 
	            case constants.messageIdSys:
	                //処理中表示終了
	                SC3080401.endServerCallback();

	                alert(result);
	                break;
	            //正常終了
	            default:
	                //画面の初期化 
	                InitializeWindowSC3080401(result, context);

	                //処理中表示終了
	                SC3080401.endServerCallback();

	                break;
	        }
	    });
	//2014/04/21 TCS市川 GTMCタブレット高速化対応 START
	});
	//2014/04/21 TCS市川 GTMCタブレット高速化対応 END
}

/**
* 初期処理
*/
(function (window) {

    $.extend(window, { SC3080401: {} });
    $.extend(SC3080401, {

        /**
        * コールバック開始
        */
        startServerCallback: function () {
            SC3080401.showLoding();
        },

        /**
        * コールバック終了
        */
        endServerCallback: function () {
            SC3080401.closeLoding();
        },

        /******************************************************************************
        読み込み中表示
        ******************************************************************************/

        /**
        * 読み込み中アイコン表示
        */
        showLoding: function () {

            //オーバーレイ表示
            $("#registOverlayBlackSC3080401").css("display", "block");
            //アニメーション
            setTimeout(function () {
                $("#processingServerSC3080401").addClass("show");
                $("#registOverlayBlackSC3080401").addClass("open");
            }, 0);

        },

        /**
        * 読み込み中アイコンを非表示にする
        */
        closeLoding: function () {
            $("#processingServerSC3080401").removeClass("show");
            //2014/11/04 TCS 藤井 iOS8 対応(i-CROP_V4_salesよりマージ)  Start
            $("#registOverlayBlackSC3080401").removeClass("open");
            setTimeout(function () {
                $("#registOverlayBlackSC3080401").css("display", "none");
            }, 300);
            //2014/11/04 TCS 藤井 iOS8 対応(i-CROP_V4_salesよりマージ)  End
        }
    });

})(window);

//2012/04/12 TCS 鈴木(健) HTMLエンコード対応 START
/**
* HTMLエンコードを行う
* 
* @param {String} value 
* 
*/
function SC3080401HTMLEncode(value) {
    return $("<Div>").text(value).html();
}

/**
* HTMLデコードを行う
* 
* @param {String} value 
* 
*/
function SC3080401HTMLDecode(value) {
    return $("<Div>").html(value).text();
}
//2012/04/12 TCS 鈴木(健) HTMLエンコード対応 END
