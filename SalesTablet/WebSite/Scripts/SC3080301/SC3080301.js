/*━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
SC3080301.js
─────────────────────────────────────
機能： 査定依頼
補足： 
作成： 2012/01/05 TCS 鈴木(恭)
更新： 2012/02/21 TCS 鈴木(健) 【SALES_1B】カーチェックシート画面への遷移時の処理中表示追加
更新： 2012/03/09 TCS 鈴木(恭) 【SALES_2】コールバック時の文字列のエンコード処理追加
更新： 2012/04/13 TCS 鈴木(恭) HTMLエンコード対応
更新： 2012/05/23 TCS 鈴木(恭) ローディング中にフリーズする問題(号口課題No.83)
更新： 2013/01/28 TCS 神本     GL0869_価格相談、査定依頼、ヘルプ依頼の表示が崩れる
更新： 2014/04/21 TCS 市川 GTMCタブレット高速化対応（BTS-386）
更新： 2014/11/04 TCS 藤井 iOS8 対応(i-CROP_V4_salesよりマージ)
更新： 2017/12/07 TCS 河原 販売流通関連システムのiOS10.3適用
─────────────────────────────────────*/

/***********************************************************
査定依頼
***********************************************************/

//コールバック関数定義
var callbackSC3080301 = {
    doCallback: function (method, argument, callbackFunction) {
        this.method = method;
        this.argument = argument;
        this.packedArgument = method + "," + argument;
        this.endCallback = callbackFunction;
        this.beginCallback();
    }
};	

/**
* ポップアップ表示
*/
function showAssessmentPopup() {

    //2017/12/07 TCS 河原 販売流通関連システムのiOS10.3適用 START
    var ans = $("#MstPG_FootItem_Sub_202").offset();
    var assessmentLeft = ans.left;
    assessmentLeft = assessmentLeft + 263;
    $("#AssessmentCarSelectPopup").parent().css("left", assessmentLeft);
    //2017/12/07 TCS 河原 販売流通関連システムのiOS10.3適用 END

    //2014/04/21 TCS市川 GTMCタブレット高速化対応 START
    //スクリプトの遅延読み込み
    SC3080201.requirePartialScript("../Scripts/SC3080301/SC3080301.Popup.js", function () {
    //2014/04/21 TCS市川 GTMCタブレット高速化対応 END

        //2012/05/23 TCS 鈴木(恭) ローディング中にフリーズする問題(号口課題No.83) START
        //メイン画面のコンテンツを削除	
        $('.scNsc412PopUpModelSelectListArea2>div').remove();

        //GL0869_価格相談、査定依頼、ヘルプ依頼の表示が崩れる START
        $("#AssessmentCarPopupMakerTitle,#AssessmentCarPopupModelTitle,#scNscPopUpClosePanel,#scNscPopUpReturnPanel").css("display", "none");
        $("#scNscPopUpClosePanel").css("display", "block");
        $("#AssessmentCarPopupMakerTitle").css("display", "block");
        //GL0869_価格相談、査定依頼、ヘルプ依頼の表示が崩れる END

        //ポップアップ表示
        $("#AssessmentCarSelectPopup").fadeIn(300);

        SC3080301.startServerCallback();

        commonRefreshTimer(SC3080301displayAlert);

        //画面初期化情報を取得する
        SC3080301displayAlert();
        //2012/05/23 TCS 鈴木(恭) ローディング中にフリーズする問題(号口課題No.83) END
    //2014/04/21 TCS市川 GTMCタブレット高速化対応 START
    });
    //2014/04/21 TCS市川 GTMCタブレット高速化対応 END
};

//2014/04/21 TCS市川 GTMCタブレット高速化対応 DELETE

/**
* 初期処理
*/
(function (window) {

    $.extend(window, { SC3080301: {} });
    $.extend(SC3080301, {

        /**
        * コールバック開始
        */
        startServerCallback: function () {
            SC3080301.showLoding();
        },

        /**
        * コールバック終了
        */
        endServerCallback: function () {
            SC3080301.closeLoding();
        },

        /******************************************************************************
        読み込み中表示
        ******************************************************************************/
        
        /**
        * 読み込み中アイコン表示
        */
        showLoding: function () {

            //オーバーレイ表示
            //2012/02/21 TCS 鈴木(健) 【SALES_1B】カーチェックシート画面への遷移時の処理中表示追加 START
            //$("#registOverlayBlackSC3080301").css("display", "block");
            if ($('#IsCarCheckSheetOpenHidden').val() == "True") {
                $("#registOverlayBlackSC3080301_Redirect").css("display", "block");
            } else {
                $("#registOverlayBlackSC3080301").css("display", "block");
            }
            //2012/02/21 TCS 鈴木(健) 【SALES_1B】カーチェックシート画面への遷移時の処理中表示追加 END
            //アニメーション
            setTimeout(function () {
                //2012/02/21 TCS 鈴木(健) 【SALES_1B】カーチェックシート画面への遷移時の処理中表示追加 START
                //$("#processingServerSC3080301").addClass("show");
                //$("#registOverlayBlackSC3080301").addClass("open");
                if ($('#IsCarCheckSheetOpenHidden').val() == "True") {
                    $("#processingServerSC3080301_Redirect").addClass("show");
                    $("#registOverlayBlackSC3080301_Redirect").addClass("open");
                } else {
                    $("#processingServerSC3080301").addClass("show");
                    $("#registOverlayBlackSC3080301").addClass("open");
                }
                //2012/02/21 TCS 鈴木(健) 【SALES_1B】カーチェックシート画面への遷移時の処理中表示追加 END
            }, 0);

        },

        /**
        * 読み込み中アイコンを非表示にする
        */
        closeLoding: function () {
        //2012/05/23 TCS 鈴木(恭) ローディング中にフリーズする問題(号口課題No.83) START
        //2012/02/21 TCS 鈴木(健) 【SALES_1B】カーチェックシート画面への遷移時の処理中表示追加 START
                if ($('#IsCarCheckSheetOpenHidden').val() == "True") {
                    $("#processingServerSC3080301_Redirect").removeClass("show");
                    //2014/11/04 TCS 藤井 iOS8 対応(i-CROP_V4_salesよりマージ)  Start
                    $("#registOverlayBlackSC3080301_Redirect").removeClass("open");
                    setTimeout(function () {
                        $("#registOverlayBlackSC3080301_Redirect").css("display", "none");
                    }, 300);
                    //2014/11/04 TCS 藤井 iOS8 対応(i-CROP_V4_salesよりマージ)  End
                    $('#IsCarCheckSheetOpenHidden').val("False");
                } else {
                    $("#processingServerSC3080301").removeClass("show");
                    //2014/11/04 TCS 藤井 iOS8 対応(i-CROP_V4_salesよりマージ)  Start
                    $("#registOverlayBlackSC3080301").removeClass("open");
                    setTimeout(function () {
                            $("#registOverlayBlackSC3080301").css("display", "none");
                    }, 300);
                    //2014/11/04 TCS 藤井 iOS8 対応(i-CROP_V4_salesよりマージ)  End
                }
        //2012/02/21 TCS 鈴木(健) 【SALES_1B】カーチェックシート画面への遷移時の処理中表示追加 END
        //2012/05/23 TCS 鈴木(恭) ローディング中にフリーズする問題(号口課題No.83) END
        }
    });

})(window);
