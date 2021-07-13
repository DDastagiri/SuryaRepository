/** 
 * @fileOverview SC3110101 パネル内の処理
 * 
 * @author shota akiyama
 * @version 1.0.0
 */

/*
 * 受付の権限コード
 * @return {int}
 *
 */
var C_OPERATIONCODE = 51;

/**
 * 使用中・未使用の切り替え処理を行う.
 * 
 * @param {String} changeUse 切り替える親のID
 * @param {String} num 切り替え対象の番号
 * 
 */
function isUseCar(changeUse, num) {

    // 権限がある場合は切り替え処理を許可する
    if ($("#authority").val() == C_OPERATIONCODE) {
        if ($(changeUse + num + ">#testDriveCarStatus").val() == "1") {

            $(changeUse + num + ">#changeID").css("visibility", "hidden");
            $(changeUse + num + ">#testDriveCarStatus").val("0");
        } else {

            $(changeUse + num + ">#changeID").css("visibility", "visible");
            $(changeUse + num + ">#testDriveCarStatus").val("1");
        }
    }
}

/**
 * 使用中・未使用の初期表示処理を行う.
 * 
 */
$(window).load(function () {

    if ($("#clickStatus", $(parent.document)).val() == "1") {
        parent.closePopOver();
    }
	
    // データ件数を取得
    var cnt = $(".statusCnt").size() - 1;

    // 件数分だけループさせる
    for (var i = 0; i <= cnt; i++) {

        // 試乗車ステータスに合わせて使用中の表示・非表示を行う
        if ($("#changeID" + i + ">#testDriveCarStatus").val() == "1") {

            $("#changeID" + i + ">#changeID").css("visibility", "visible");
        } else {

            $("#changeID" + i + ">#changeID").css("visibility", "hidden");
        }
    }

    // スクロールの設定を行う
    $(".innerDataBox").fingerScroll();

    // 件数が0件の場合
    if (cnt == -1) {

        $("#clickStatus", window.parent.document).val("-1");
    } else {

        // clickイベントの復帰
        $("#clickStatus", window.parent.document).val("0");
    } 
});

/**
* 登録ボタン押下時処理.
* 
* @param {-} - -
* @return {-} -
* 
* @example 
*  -
*/
function redirectSC3110101() {

    SC3110101.startServerCallback();
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
    if ($('#Panel_SC3110101:visible', $(parent.document)).length == 0) {
        return;
    }

    $('#LoadSpinButton').click();
    SC3110101.startServerCallback();
}

/**
* 初期処理
*/
(function (window) {

    $.extend(window, { SC3110101: {} });
    $.extend(SC3110101, {

        /**
        * コールバック開始
        */
        startServerCallback: function () {
            SC3110101.showLoding();
        },

        /**
        * コールバック終了
        */
        endServerCallback: function () {
            SC3110101.closeLoding();
        },

        /******************************************************************************
        読み込み中表示
        ******************************************************************************/

        /**
        * 読み込み中アイコン表示
        */
        showLoding: function () {

            //オーバーレイ表示
            $("#registOverlayBlackSC3110101").css("display", "block");
            //アニメーション
            setTimeout(function () {
                $("#processingServerSC3110101").addClass("show");
                $("#registOverlayBlackSC3110101").addClass("open");
            }, 0);

        },

        /**
        * 読み込み中アイコンを非表示にする
        */
        closeLoding: function () {

            $("#processingServerSC3110101").removeClass("show");
            $("#registOverlayBlackSC3110101").removeClass("open").one("webkitTransitionEnd", function (we) {
                $("#registOverlayBlackSC3110101").css("display", "none");
            });
        }
    });

})(window);
