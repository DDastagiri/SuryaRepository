/**
* @fileOverview SC3080216 初期ロード時処理
*
* @author TCS 森
* @version 2.0.0
* 
* 新規： 2014/02/13 TCS 森 受注後フォロー機能開発
* 
*/

/****************************************************************

初期ロード

*****************************************************************/
function newCustomerDummyErrorAfter() {
    //コンタクト履歴の再読み込みフラグを強制的にON
    $("#reloadFlg").val("1");

    //マーカー設定
    $("#scNscAllBoxContentsArea").removeClass("page1 page2 page3").addClass("page1");
    //移動
    $("#scNscAllBoxContentsArea").css({ "transform": "translate3d(0px, 0px, 0px)" });
    //ページ上部のナビゲーション
    SC3080201.setPageNavi();

    setTimeout(function () {
        //顧客編集実行
        CustomerEditPopUpOpen();
    }, 2000);
}

$(function () {

    $("#rightBox ul").fingerScroll();

    SC3080216Main();

    $("#SC3080216_ToDoToggle").click(function () {
        // ToDoボタンの背景色と文字色を変更する
        if ($("#SC3080216_ToDoToggle").hasClass("SC3080216_toggle_off")) {

            $("#SC3080216_ToDoToggle").removeClass("SC3080216_toggle_off").addClass("SC3080216_toggle_on");
            $("#SC3080216_AllToggle").removeClass("SC3080216_toggle_on").addClass("SC3080216_toggle_off");

            // 完了済み活動を非表示にする
            $("#rightBox li.rightBoxRow").each(function () {
                var dom = $(this).children(".icon1").children(".icon1_rightbottom");
                // 日付別表示用の完了フラグ確認
                if (dom.children("#SC3080216_Act_Comp_Days").val() == '1') {
                    $(this).css("display", "none");

                }

                // 工程別表示用の完了フラグ確認
                if (dom.children("#SC3080216_Act_Comp_Prcs").val() == '1') {
                    $(this).css("display", "none");

                }
                // スクロールバーを上端に移動させる
                $("#rightBox ul .scroll-inner").css({ "transform": "translate3d(0px, 0px, 0px)" });

            });
            // タイトルを非表示する
            $("#rightBox li.rightBoxTitle").each(function () {
                if ($(this).children("#SC3080216_Title_Flg_Days").val() == '1' || $(this).children("#SC3080216_Title_Flg_Prcs").val() == '1') {
                    $(this).css("display", "none");
                }
            });
        }
    });

    $("#SC3080216_AllToggle").click(function () {
        //Allボタンの背景色と文字色を変更する
        if ($("#SC3080216_AllToggle").hasClass("SC3080216_toggle_off")) {
            $("#SC3080216_AllToggle").removeClass("SC3080216_toggle_off").addClass("SC3080216_toggle_on");
            $("#SC3080216_ToDoToggle").removeClass("SC3080216_toggle_on").addClass("SC3080216_toggle_off");

            // すべての活動を表示する
            $("#rightBox li.rightBoxRow").show();
            // タイトルを表示する
            $("#rightBox li.rightBoxTitle").each(function () {
                if ($(this).children("#SC3080216_Title_Flg_Days").val() == '1' || $(this).children("#SC3080216_Title_Flg_Prcs").val() == '1') {
                    $(this).show();
                }
            });
        }
    });

    $("#SC3080216_TimeToggle").click(function () {
        // 日付別表示に切り替える
        if ($("#SC3080216_TimeToggle").hasClass("SC3080216_toggle_off")) {
            $("#SC3080216_TimeToggle").removeClass("SC3080216_toggle_off").addClass("SC3080216_toggle_on");
            $("#SC3080216_ProcessToggle").removeClass("SC3080216_toggle_on").addClass("SC3080216_toggle_off");
            $("#act_Time").show();
            $("#act_Process").css("display", "none");
        }
    });

    $("#SC3080216_ProcessToggle").click(function () {
        // 工程別表示に切り替える
        if ($("#SC3080216_ProcessToggle").hasClass("SC3080216_toggle_off")) {
            $("#SC3080216_ProcessToggle").removeClass("SC3080216_toggle_off").addClass("SC3080216_toggle_on");
            $("#SC3080216_TimeToggle").removeClass("SC3080216_toggle_on").addClass("SC3080216_toggle_off");
            $("#act_Time").css("display", "none");
            $("#act_Process").show();
        }
    });

    // チェックボックス(日付別)を更新する
    $("#AfterActivityDaysMain li .icon1_rightbottom").click(function () {

        //契約活動、担当外の場合はチェックボックスのONOFFをしない
        if ($(this).children("#SC3080216AfterActNoCheckDays").val() == '0') {
            var sava_act_code = "";
            sava_act_code = $(this).children("#SC3080216_After_Act_Code_days").val();

            // DBから送られてきたデータの完了フラグが立っているものをチェックする
            if ($(this).find("div").hasClass("Check")) {

                // DBから取得した文言を表示する
                if ($(this).children("#SC3080216_Act_Comp_Days").val() == '1') {
                    alert($("#ActCheckOffMsg").val());
                }

                // チェックボックス解除
                $(this).find("div").removeClass("Check");
                $(this).children("#SC3080216_save_flg_days").val('0');

                // 裏の同一受注後活動に対して登録フラグの同期を取る
                $("#AfterActivityPrcsMain li.rightBoxRow").each(function () {
                    var dom = $(this).children("#CheckBorderAreaPrcs").children("#CheckBorderImageAreaPrcs");
                    if (dom.children("#SC3080216_After_Act_Code_prcs").val() == sava_act_code) {
                        // チェックボックス解除
                        $(this).find(".icon1_rightbottom div").removeClass("Check");
                        dom.children("#SC3080216_save_flg_prcs").val('0');
                    }
                });

            } else {

                // チェックボックス設定
                $(this).find("div").addClass("Check");
                $(this).children("#SC3080216_save_flg_days").val('1');

                $("#AfterActivityPrcsMain li.rightBoxRow").each(function () {
                    var dom = $(this).children("#CheckBorderAreaPrcs").children("#CheckBorderImageAreaPrcs");
                    if (dom.children("#SC3080216_After_Act_Code_prcs").val() == sava_act_code) {
                        // チェックボックス設定
                        $(this).find(".icon1_rightbottom div").addClass("Check");
                        dom.children("#SC3080216_save_flg_prcs").val('1');
                    }
                });
            }
        }
    });

    // チェックボックス(工程別)を更新する
    $("#AfterActivityPrcsMain  li .icon1_rightbottom").click(function () {

        //契約活動、担当外の場合はチェックボックスのONOFFをしない
        if ($(this).children("#SC3080216AfterActNoCheckPrcs").val() == '0') {
            var sava_act_code = "";
            sava_act_code = $(this).children("#SC3080216_After_Act_Code_prcs").val();

            // DBから送られてきたデータの完了フラグが立っているものをチェックする
            if ($(this).find("div").hasClass("Check")) {

                // DBから取得した文言を表示する
                if ($(this).children("#SC3080216_Act_Comp_Prcs").val() == '1') {
                    alert($("#ActCheckOffMsg").val());
                }

                // チェックボックス解除
                $(this).find("div").removeClass("Check");
                $(this).children("#SC3080216_save_flg_prcs").val('0');

                // 裏の同一受注後活動に対して登録フラグの同期を取る
                $("#AfterActivityDaysMain li.rightBoxRow").each(function () {
                    var dom = $(this).children("#CheckBorderAreaDays").children("#CheckBorderImageAreaDays");
                    if (dom.children("#SC3080216_After_Act_Code_days").val() == sava_act_code) {
                        // チェックボックス解除
                        $(this).find(".icon1_rightbottom div").removeClass("Check");
                        dom.children("#SC3080216_save_flg_days").val('0');
                    }
                });

            } else {

                // チェックボックス設定
                $(this).find("div").addClass("Check");
                $(this).children("#SC3080216_save_flg_prcs").val('1');

                $("#AfterActivityDaysMain li.rightBoxRow").each(function () {
                    var dom = $(this).children("#CheckBorderAreaDays").children("#CheckBorderImageAreaDays");
                    if (dom.children("#SC3080216_After_Act_Code_days").val() == sava_act_code) {
                        // チェックボックス設定
                        $(this).find(".icon1_rightbottom div").addClass("Check");
                        dom.children("#SC3080216_save_flg_days").val('1');
                    }
                });
            }
        }
    });

    $("#RegistButton").click(function () {
        var saveActCode = "";
        var saveCompFlag = "";

        // 登録対象となる受注後活動ID,完了フラグを取得する
        $("#AfterActivityDaysMain li.rightBoxRow").each(function () {
            var dom = $(this).children("#CheckBorderAreaDays").children("#CheckBorderImageAreaDays");
            if (dom.children("#SC3080216_Act_Comp_Days").val() != dom.children("#SC3080216_save_flg_days").val()) {
                saveActCode = saveActCode + dom.children("#SC3080216_After_Act_Code_days").val() + "-" + dom.children("#SC3080216_save_flg_days").val() + ",";
                saveCompFlag = saveCompFlag + dom.children("#SC3080216_save_flg_days").val() + ",";

            }
        });
        // 末尾のカンマを削除する
        $("#UpdAfterActCdList").val(saveActCode.substring(0, saveActCode.length - 1));
        $("#UpdAfterActCompFlgList").val(saveCompFlag.substring(0, saveCompFlag.length - 1));

    });

    //初期表示処理
    function SC3080216Main() {
        var contact = "";
        var prcs = "";

        //Ellipsis設定
        $("#AfterActivityPrcsMain li.rightBoxTitle").find(".ellipsis").CustomLabel({ 'useEllipsis': 'true' });

        $("#AfterActivityDaysMain li.rightBoxRow").each(function () {
            // 完了済み活動のチェックボックスをONにする
            if ($(this).children("#CheckBorderAreaDays").children("#CheckBorderImageAreaDays").children("#SC3080216_save_flg_days").val() == '1'
            && $(this).children("#CheckBorderAreaDays").children("#CheckBorderImageAreaDays").children("#SC3080216AfterActCheckMarkFlgDays").val() == '0') {
                $(this).find(".icon1_rightbottom div").addClass("Check");
            } else {
                $(this).find(".icon1_rightbottom div").removeClass("Check");
            }
            //アイコン接触方法
            contact = $(this).find("div.icon2");
            contact.css("background", "url(" + contact.children("#SC3080216ContactIconDays").val() + ") ")
                .css("background-position", "center center")
                .css("background-repeat", "no-repeat")
                .css("background-color", "#777");
            //アイコン工程
            prcs = $(this).find("div.icon3");
            prcs.css("background", "url(" + prcs.children("#SC3080216PrcsIconDays").val() + ") ")
                .css("background-position", "center center")
                .css("background-repeat", "no-repeat")
                .css("background-color", "white");
        });
        $("#AfterActivityPrcsMain li.rightBoxRow").each(function () {
            // 完了済み活動のチェックボックスをONにする
            if ($(this).children("#CheckBorderAreaPrcs").children("#CheckBorderImageAreaPrcs").children("#SC3080216_save_flg_prcs").val() == '1'
            && $(this).children("#CheckBorderAreaPrcs").children("#CheckBorderImageAreaPrcs").children("#SC3080216AfterActCheckMarkFlgPrcs").val() == '0') {
                $(this).find(".icon1_rightbottom div").addClass("Check");
            } else {
                $(this).find(".icon1_rightbottom div").removeClass("Check");
            }
            //アイコン接触方法
            contact = $(this).find("div.icon2");
            contact.css("background", "url(" + contact.children("#SC3080216ContactIconPrcs").val() + ") ")
                .css("background-position", "center center")
                .css("background-repeat", "no-repeat")
                .css("background-color", "#777");
        });

        $("#AfterActivityPrcsMain li.rightBoxTitle").each(function () {
            //アイコン工程
            prcs = $(this).find("div.icon4");
            prcs.css("background", "url(" + prcs.children("#SC3080216PrcsIconPrcs").val() + ") ")
                .css("background-position", "center center")
                .css("background-repeat", "no-repeat")
                .css("background-color", "white");
        });
    }
});
