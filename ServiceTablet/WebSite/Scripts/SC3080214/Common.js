/// <reference path="../jquery.js"/>
/**
* @fileOverview SC3080214 顧客詳細 共通
*
* @author TCS 寺本
* @version 1.0.0
*/

var gIsRegist = false;

/**
* 初期処理
*/
(function (window) {

    $.extend(window, { SC3080214: {} });
    $.extend(SC3080214, {

        //デバッグフラグ
        isDebug: false,

        /**
        * 初期化
        */
        init: function () {

            //二重押下禁止処理
            SC3080214.initProcessControl();
        },

        /******************************************************************************
        ここから二重押下禁止処理
        ******************************************************************************/

        /**
        * サーバー処理を実行中かどうか
        */
        serverProcessing: false,

        /**
        * Ms Ajax リクエストマネージャー
        */
        requestMgr: null,

        /**
        * 二重起動監視処理の初期化
        */
        initProcessControl: function () {

            //リクエストマネージャー設定
            SC3080214.requestMgr = Sys.WebForms.PageRequestManager.getInstance();

            Sys.Application.add_load(SC3080214.appLoad);                                //同期・非同期通信完了後のロードイベント
            SC3080214.requestMgr.add_initializeRequest(SC3080214.initAsyncPostback);    //非同期通信開始前の処理を追加
            SC3080214.requestMgr.add_endRequest(SC3080214.endAsyncPostback);            //非同期通信終了後の処理を追加

            //同期ポストバックのボタンの二度押し禁止
            $("form").bind("submit", function (e) {
                //alert("sub");
                if (window.event.returnValue === true) {

                    //処理キャンセル
                    if (SC3080214.serverProcessing === true) {
                        SC3080214.setDebugMessage("現在処理中です・・・S");
                        window.event.returnValue = false;
                        return false;
                    }

                    //処理中フラグを立てる
                    SC3080214.serverProcessing = true;
                }
            });

            //リンクボタンの同期ポストバック制御
            var _originalSubmit = $("form").get(0).submit;
            $("form").get(0).submit = function () {
                //処理キャンセル
                if (SC3080214.serverProcessing === true) {
                    SC3080214.setDebugMessage("現在処理中です・・・L");
                    return;
                }

                //処理中フラグを立てる
                SC3080214.serverProcessing = true;
                _originalSubmit.call(this, arguments);
            };

        },

        /**
        * 非同期通信の開始を監視し、２重起動を防止する
        */
        initAsyncPostback: function (sender, args) {
            var cancelFlg = false;

            //サーバー処理中
            if (SC3080214.serverProcessing === true) cancelFlg = true;

            //別の非同期通信が起動中
            if (SC3080214.requestMgr.get_isInAsyncPostBack() === true) cancelFlg = true;

            //処理中の場合はキャンセル
            if (cancelFlg === true) {

                SC3080214.setDebugMessage("現在処理中です・・・");
                args.set_cancel(true);
            } else {
                SC3080214.showLoding();
                SC3080214.serverProcessing = true;
            }

        },

        /**
        * 非同期通信終了
        */
        endAsyncPostback: function (sender, args) {
            if (args.get_error() !== null) {
                //サーバーエラー
                if (SC3080214.requestMgr.get_isInAsyncPostBack() === false) SC3080214.serverProcessing = false;
            }
            if (SC3080214.requestMgr.get_isInAsyncPostBack() === false) SC3080214.closeLoding();
        },

        /**
        * ロード処理(同期・非同期共通）
        */
        appLoad: function () {
            //処理中フラグ変更
            SC3080214.serverProcessing = false;
        },

        //デバッグメッセージ表示
        setDebugMessage: function (msg) {
            if (SC3080214.isDebug === true) {

                var div = $("<div style='border:2px solid #CCF; width:200px; height:60px;position:absolute;bottom:52px;right:-200px;background:#EEE;'/>");
                $(document.body).append(div);

                div.text(msg).animate({ right: 0 }, 700, function () {
                    setTimeout(function () {
                        div.fadeOut(200, function () {
                            div.remove();
                        });
                    }, 3000);
                });
            }
        },

        /**
        * コールバック開始
        */
        startServerCallback: function () {
            SC3080214.serverProcessing = true;
            SC3080214.showLoding();
        },


        /**
        * コールバック終了
        */
        endServerCallback: function () {
            SC3080214.serverProcessing = false;
            SC3080214.closeLoding();
        },

        /******************************************************************************
        読み込み中表示
        ******************************************************************************/

        /**
        * 読み込み中アイコン表示
        */
        showLoding: function () {
        
            // 2015/11/24 TR-SVT-TMT-20151026-001 Start
            if (gIsRegist === false) return;
        
            LoadProcess();
            // 2015/11/24 TR-SVT-TMT-20151026-001 End
        },

        /**
        * 読み込み中アイコンを非表示にする
        */
        closeLoding: function () {

            // 2015/11/24 TR-SVT-TMT-20151026-001 Start
            if (gIsRegist === true) gIsRegist = false;

            LoadProcessHide();
            // 2015/11/24 TR-SVT-TMT-20151026-001 End
        }
    });

})(window);

/**
* DOMロード時の処理
*/
$(function () {

    //$("#scNscAllBoxContentsArea").addClass("page3");
    SC3080214.init();

});
