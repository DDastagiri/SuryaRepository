/// <reference path="../jquery.js"/>
/** 
* @fileOverview SC3010101.aspクラスを記述するファイル.
* 
* @author TCS hirano
* @version 1.0.0
*/

/**
* 表示期限の初期値設定
*/
$(function () {
    $("#id").CustomTextBox({
        clear: function () {
            checkInput();
        }
    });

    $("#password").CustomTextBox({
        clear: function () {
            checkInput();
        }
    });
});

/**
* ログインボタンの表示を制御する.
* 
* @param {-} - -
* @return {-} -
* 
* @example 
*  -
*/
function checkInput() {
    var txtId = document.getElementById("id");
    var txtPw = document.getElementById("password");
    var divUp = document.getElementById("loginBtn");
    var divDw = document.getElementById("loginDown");

    if (txtId && txtPw) {
        var strId = txtId.value.trim();
        var strPw = txtPw.value.trim();

        if (strId == "" || strPw == "") {
            divUp.style.display = "block";
            divDw.style.display = "none";
        }
        else {
            divUp.style.display = "none";
            divDw.style.display = "block";
        }
    }
}

/**
* Trim処理.
* 
* @param {-} - -
* @return {-} -
* 
* @example 
*  -
*/
String.prototype.trim = function () {
    return this.replace(/^[\s ]+|[\s ]+$/g, '');
}

/**
* Macaddress取得処理(タブレット基盤).
* 
* @param {-} - -
* @return {-} -
* 
* @example 
*  -
*/
function getMacaddress() {
    window.location = "icrop:gmac:reloadForDB";
}

function getMacaddress_openam() {
    window.location = "icrop:gmac:reloadForDB_openam";
}

/**
* Macaddress取得処理.
* 
* @param {String} macaddress マックアドレス
* @return {-} -
* 
* @example 
*  -
*/
function reloadForDB(macaddress) {
    var hdnMac = document.getElementById("hdnMac");
    if (hdnMac != null) {
        if (macaddress == "" || macaddress == undefined) {
            hdnMac.value = "912";
        } else {
            hdnMac.value = macaddress;
        }
    }

    document.getElementById("autoSubmitButton").click(); //DB接続確認のためリロード
    //this_form.submit(); //DB接続確認のためリロード
}

function reloadForDB_openam(macaddress) {
    var hdnMac = document.getElementById("hdnMac");
    if (hdnMac != null) {
        if (macaddress == "" || macaddress == undefined) {
            hdnMac.value = "912";
        } else {
            hdnMac.value = macaddress;
        }
    }

    document.getElementById("OpenAMAuthButton").click(); //DB接続確認のためリロード
    //this_form.submit(); //DB接続確認のためリロード
}

/**
* ログイン中Load処理.
* 
* @param {-} - -
* @return {-} -
* 
* @example 
*  -
*/
function login() {
    var divlogin = document.getElementById("login");
    var divLoading = document.getElementById("loading");
    var divUp = document.getElementById("loginBtn");
    var divDw = document.getElementById("loginDown");
    $("#loginBtn").show(0);
    $("#loginDown").hide(0);
    divlogin.style.display = "none";
    divLoading.style.display = "block";
    //divUP.style.display = "block";
    //divDW.style.display = "none";

    //再表示タイマーセット
    loginRefreshTimer(
        function () {
            $("#refreshButton").click();
        }
    );
}

/**
* PushServer登録.
* 
* @param {String} id ユーザアカウント
* @return {-} -
* 
* @example 
*  -
*/
//2012/07/06 KN 小澤 STEP2対応 START
//function movePage(id) {
function movePage(id, isServiceUser) {
    //2012/07/06 KN 小澤 STEP2対応 END
    //--------------PushServer登録
    $(function () {

        //再表示タイマーセット
        loginRefreshTimer(
            function () {
                $("#refreshButton").click();
            }
        );

        window.location = "icrop:lgin:" + id;
        //2012/07/06 KN 小澤 STEP2対応 START
        //setTimeout(reloadPage, 10);
        //サービスユーザーの場合は来店処理のないファンクションをキックする
        //if (isServiceUser == "True") {
        //    setTimeout(reloadServicePage, 10);
        //} else {
        //    setTimeout(reloadPage, 10);
        //}
        //2012/07/06 KN 小澤 STEP2対応 END

        //2018/01/31 ES サービスサイトでは IC3100301の呼び出しは行わない
        setTimeout(reloadServicePage, 10);
    });
}

/**
* 来店実績_ログイン更新後ページ遷移.
* 
* @param {-} - -
* @return {-} -
* 
* @example 
*  -
*/
function reloadPage() {
    var path = window.location.href;
    var url = path.split("/");
    path = path.replace(url[url.length - 1], "");
    var postTarget = path + "Services/IC3100301.asmx/UpdateVisitLogin";

    $.ajax({
        type: "POST",
        url: postTarget,
        contentType: "application/xml; charset=UTF-8",
        async: false,
        success: function (ret) {
            //--------------更新結果反映
            var hdnUploadFlg = document.getElementById("hdnUploadFlg");
            hdnUploadFlg.value = $(ret).find("ResultId").text();
        },
        error: function (e) {
            var hdnUploadFlg = document.getElementById("hdnUploadFlg");
            hdnUploadFlg.value = "9999";
        }
    });

    //--------------画面リロード
    document.getElementById("autoSubmitButton").click();
    //this_form.submit();
}

//2012/07/06 KN 小澤 STEP2対応 START
/**
* ログイン更新後ページ遷移.
* 
* @param {-} - -
* @return {-} -
* 
* @example 
*  -
*/
function reloadServicePage() {
    //--------------画面リロード
    document.getElementById("autoSubmitButton").click();
    //this_form.submit();
}
//2012/07/06 KN 小澤 STEP2対応 END


/**
* 再表示タイマーをセットする.
* 
* @param {refreshFunc} 再表示用のJavaScrep関数 -
* @return {-} -
* 
* @example 
*  -
*/
function loginRefreshTimer(refreshFunc) {

    //タイマー間隔の取得
    var refreshTime = Number($("#loginPG_RefreshTimerTime").val());

    setTimeout(function () {

        //出力メッセージを選択する
        var messageString = $("#loginPG_RefreshTimerMessage1").val();

        //警告メッセージ出力
        alert(messageString);

        //各画面でリフレッシュ処理をする
        if (refreshFunc() === false) {
            //falseが帰ってきたら再読み込み処理をしない
            return;
        }

        //再度、タイマーをセットする
        loginRefreshTimer(refreshFunc)

    }, refreshTime);
}
