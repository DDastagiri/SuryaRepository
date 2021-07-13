/*━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
SC3040802.js
─────────────────────────────────────
機能： 通知一覧(MG用)
補足： 
作成： 2012/01/05 TCS 明瀬
更新： 2012/04/18 TCS 明瀬 HTMLエンコード対応
更新： 2014/05/10 TCS 武田 受注後フォロー機能開発
─────────────────────────────────────*/

/**
* 通知一覧で通知を選択したときの処理を行う
* 
* @param {-} - -
* @return {-} -
* 
* @example 
*  -
*/
function selectNotice(prm) {

    //画面遷移用ボタンをクリックする(画面が全体表示のときのみ)
    if ($("#openCloseHidden").val() === "open") {

        var prmArray = prm.split(",");
        //Hiddenに値を格納する
        //2012/04/18 TCS 明瀬 HTMLエンコード対応 Start
        $("#reqNoticeHidden").val(decodeURIComponent(prmArray[0]));         //通知依頼ID
        $("#reqCtgIdHidden").val(decodeURIComponent(prmArray[1]));          //通知依頼種別(02:価格相談、03:ヘルプ)
        $("#reqClassIdHidden").val(decodeURIComponent(prmArray[2]));        //依頼種別ID(見積管理IDまたはヘルプID)
        $("#cstKindHidden").val(decodeURIComponent(prmArray[3]));           //顧客種別
        $("#cstClassHidden").val(decodeURIComponent(prmArray[4]));          //顧客分類
        $("#crCstIdHidden").val(decodeURIComponent(prmArray[5]));           //顧客コード
        $("#toAccountHidden").val(decodeURIComponent(prmArray[6]));         //送信先アカウント
        $("#toAccountNameHidden").val(decodeURIComponent(prmArray[7]));     //送信先アカウント名
        $("#cstNameHidden").val(decodeURIComponent(prmArray[8]));           //顧客名
        $("#salesStaffCDHidden").val(decodeURIComponent(prmArray[9]));      //顧客担当セールススタッフコード
        $("#fllwUpBoxStrCDHidden").val(decodeURIComponent(prmArray[10]));   //Follow-up Box店舗コード
        $("#fllwUpBoxHidden").val(decodeURIComponent(prmArray[11]));        //Follow-up Box内連番
        $("#lastStatusHidden").val(decodeURIComponent(prmArray[12]));       //最終ステータス
        //2012/04/18 TCS 明瀬 HTMLエンコード対応 End

        //親画面にターゲットを合わせる(画面遷移を親画面にさせるため)
        $('#this_form').attr('target', '_parent');

        //オーバーレイ表示
        $("#serverProcessOverlayBlack").css("display", "block");
        //アニメーション(ロード中)
        setTimeout(function () {
            $("#serverProcessIcon").addClass("show");
            $("#serverProcessOverlayBlack").addClass("open");
        }, 0);

        $("#nextButton").click();
    }
}

/**
* 基盤から呼ばれる関数（画面をポストバックして開く）
* 
* @param {-} - -
* @return {-} -
* 
* @example 
*  -
*/
function openNoticeList() {

    //ページをポストバック
    $("#postBackButton").click();
}

/**
* 基盤から呼ばれる関数（通知画面外をタップで閉じる）
* 
* @param {-} - -
* @return {-} -
* 
* @example 
*  -
*/
function closeNoticeList() {

    if ($("#openCloseHidden").val() === "open") {
        $("#openCloseButton").click();
    }
}

//2012/04/18 TCS 明瀬 HTMLエンコード対応 Start
/**
* HTMLエンコードを行う関数
* 
* @param {String} value HTMLエンコード対象文字列
* @return {String} HTMLエンコード処理後の文字列
* 
* @example 
* HtmlEncodeSC3040802("<br>");
* 出力:「&lt;br&gt;」
*/
function HtmlEncodeSC3040802(value) {
    return $('<div/>').text(value).html();
}
//2012/04/18 TCS 明瀬 HTMLエンコード対応 End

/**
* 通知一覧(MG用)の画面制御を行う。
* 
* @param {-} - -
* @return {-} -
* 
* @example 
*  -
*/
$(function () {

    // openとclose         
    $('#openCloseButton').click(function () {

        //ボタンを一旦非活性にする
        $('#openCloseButton').attr('disabled', 'disabled');

        //全体表示なら縮小表示に変更
        if ($('#openCloseHidden').val() === 'open') {

            //親画面のiframe座標を閉じた状態に変更
            $('#noticeListFrame', parent.document).css('left', '978px');

            //親画面のiframeを囲ったdivのサイズを閉じた状態に変更
            $('#noticeListFrame', parent.document).width('46');

            //ボタンを活性にする
            $('#openCloseButton').removeAttr('disabled');

            //2012/04/18 TCS 明瀬 HTMLエンコード対応 Start
            $('#openCloseHidden').val(HtmlEncodeSC3040802('close'));
            //2012/04/18 TCS 明瀬 HTMLエンコード対応 End

            //submitしないように戻り値falseを返却
            return false;

        } else {

            //ページをポストバック
            $("#postBackButton").click();

            return true;
        }
    });
});

//2014/05/10 TCS 武田 受注後フォロー機能開発 START
/**
* 通知件数0件時、通知ウィンドウを閉じる
* 
* @param {-} - -
* @return {-} -
* 
* @example 
*  -
*/
$(function () {

    //通知件数が0件の場合、ウィンドウを非表示
    if ($('#noticeCountHidden').val() === '0') {
        $('#noticeListFrame', parent.document).width('0');
    }
});
//2014/05/10 TCS 武田 受注後フォロー機能開発 END