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

    this_form.submit(); //DB接続確認のためリロード
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
function movePage(id) {
    //--------------PushServer登録
    $(function () {
        reloadPage();
    });
}

/**
* 画面遷移.
* 
* @param {-} - -
* @return {-} -
* 
* @example 
*  -
*/
function reloadPage() {

    var hdnUploadFlg = document.getElementById("hdnUploadFlg");
    hdnUploadFlg.value = "0";

    //--------------画面リロード
    this_form.submit();
}

