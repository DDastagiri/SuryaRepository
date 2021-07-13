/// <reference path="../jquery-1.4.4.js"/>
/// <reference path="../jquery.fingerscroll.js"/>
/// <reference path="Common.js"/>

//顧客編集　START -------------------------------------------------------------------------------------

// 車両編集表示 (ページ指定)　-------------------------------------------------
function setCheck(targetElement, flg) {

    //チェックマークのチェックのON OFFを切り替える
    if (flg == true) {
        //チェックする
        targetElement.attr("checked", "checked");
        var wrapperElement = targetElement.parent();
        var chkimg = wrapperElement.children(".icrop-CheckMark-checked");
        chkimg.removeClass("icrop-CheckMark-checked");
        var checkElement = wrapperElement.children("span:nth-child(3)");
        checkElement.addClass("icrop-CheckMark-checked");

    } else {
        //チェックを外す
        targetElement.removeAttr("checked");
        var wrapperElement = targetElement.parent();
        var chkimg = wrapperElement.children(".icrop-CheckMark-checked");
        chkimg.removeClass("icrop-CheckMark-checked");
    }
}

// コールバック関数定義　-------------------------------------------------
var callback = {
    doCallback: function (method, argument, callbackFunction) {
        this.method = method;
        this.argument = argument;
        this.packedArgument = method + "," + argument;
        this.endCallback = callbackFunction;
        this.beginCallback();
    }
};

// サーバーに値を連携する　-------------------------------------------------
$(function () {
    // 住所検索　クリック　-------------------------------------------------
    $("#zipSerchButton").click(function () {

        if ($("#serverProcessFlgHidden").val() == "1") {  //サーバーサイド処理フラグ判定 (1:処理中)
            return;
        }
        var prms = "";
        if ($("#zipcodeTextBox").val() != "") {
            $("#serverProcessFlgHidden").val("1");              //サーバーサイド処理フラグ
            SC3080201.startServerCallback();

            prms = prms + $("#zipcodeTextBox").val();           //郵便番号
            callback.doCallback("GetAddress", prms, function (result, context) {

                $("#serverProcessFlgHidden").val("");           //サーバーサイド処理フラグ
                SC3080201.endServerCallback();

                var resArray = result.split(",");
                if (resArray[1] == "1") {
                    $("#addressTextBox").CustomTextBox("updateText", resArray[2]);
                } else {
                    alert(resArray[2]);
                }
            });
        }
    });

    // 顧客編集　完了クリック　-------------------------------------------------
    $("#scNscCustomerEditingWindown #scNscCustomerEditingWindownBox .scNscCustomerEditingCompletionButton").click(function (e) {
        if ($("#serverProcessFlgHidden").val() == "1") {  //サーバーサイド処理フラグ判定 (1:処理中)
            return;
        }

        //氏名未入力エラー
        if ($("#nameTextBox").attr("disabled") == false) {
            if ($("#nameTextBox").val() == "") {
                alert($("#custNoNameErrMsg").val());
                return;
            }
        }

        //自宅・携帯電話未入力エラー
        if (($("#mobileTextBox").attr("disabled") == false) || ($("#telnoTextBox").attr("disabled") == false)) {
            if (($("#mobileTextBox").val() == "") && ($("#telnoTextBox").val() == "")) {
                alert($("#custNoTelNoErrMsg").val());
                return;
            }
        }

        var prms = "";
        prms = prms + $("#nameTextBox").val() + ",";                      //氏名
        prms = prms + $("#nameTitleHidden").val() + ",";                  //敬称コード
        prms = prms + $("input[id=manCheckBox]:checked").val() + ",";     //男
        prms = prms + $("input[id=girlCheckBox]:checked").val() + ",";    //女
        prms = prms + $("input[id=kojinCheckBox]:checked").val() + ",";   //法人
        prms = prms + $("input[id=houjinCheckBox]:checked").val() + ",";  //個人

        prms = prms + $("#employeenameTextBox").val() + ",";               //担当者氏名
        prms = prms + $("#employeedepartmentTextBox").val() + ",";         //担当者部署名
        prms = prms + $("#employeepositionTextBox").val() + ",";           //役職

        prms = prms + $("#mobileTextBox").val() + ",";                     //携帯
        prms = prms + $("#telnoTextBox").val() + ",";                      //自宅
        prms = prms + $("#businesstelnoTextBox").val() + ",";              //勤務先
        prms = prms + $("#faxnoTextBox").val() + ",";                      //FAX

        prms = prms + $("#zipcodeTextBox").val() + ",";                    //郵便番号
        prms = prms + $("#addressTextBox").val() + ",";                    //住所

        prms = prms + $("#email1TextBox").val() + ",";                     //E-Mail1
        prms = prms + $("#email2TextBox").val() + ",";                     //E-Mail2

        prms = prms + $("#socialidTextBox").val() + ",";                    //国民ID

        prms = prms + $("#birthdayTextBox").val() + ",";                    //誕生日

        prms = prms + $("#actvctgryidHidden").val() + ",";                  //活動区分
        prms = prms + $("#reasonidHidden").val() + ",";                     //断念理由

        prms = prms + $("input[id=smsCheckButton]:checked").val() + ",";    //SMS
        prms = prms + $("input[id=emailCheckButton]:checked").val() + ",";  //EMail
        prms = prms + $("#nameTitleTextHidden").val();                      //敬称

        //処理中フラグを立てる
        $("#serverProcessFlgHidden").val("1");          //サーバーサイド処理フラグ (1:処理中)
        //SC3080201.startServerCallback();

        callback.doCallback("CustomerUpdate", prms, function (result, context) {

            $("#serverProcessFlgHidden").val("");       //サーバーサイド処理フラグ

            var resArray = result.split(",");
            if (resArray[1] == "0") {
                //$(".scNscCustomerEditingCancellButton").click();

                //SC3080201.endServerCallback();

                if ($("#editModeHidden").val() == "0") {
                    //顧客編集（新規登録時）
                    CustomerInsertPopUpClose()
                } else {
                    //顧客編集（編集時）
                    CustomerEditPopUpClose()
                }

                // 顧客編集-更新後情報の情報を保存する
                backUpCustomerInfo()

                //画面を消す
                $("#scNscCustomerEditingWindown").fadeOut(300);

            } else {
                alert(resArray[2]);
                //SC3080201.endServerCallback();
            }
        });
    });

    // 顧客編集　キャンセルクリック　-------------------------------------------------
    $("#scNscCustomerEditingWindown #scNscCustomerEditingWindownBox .scNscCustomerEditingCancellButton").click(function (e) {
        $("#cancelButtonLabel").click();
    });

    // 顧客編集　キャンセルクリック　-------------------------------------------------
    $("#cancelButtonLabel").click(function (e) {
        var page = $("#custPageHidden").val();
        if (page == "page1") {

            //画面を閉じる
            $("#scNscCustomerEditingWindown").fadeOut(200);

            // 顧客編集-キャンセル-変更前情報の情報に戻す
            cancelCustomerInfo();

            //念のためフラグをクリアする
            $("#serverProcessFlgHidden").val("");

        } else {
            if (page == "page2") {

                //活動区分でキャンセルボタンを押下した場合は、起動前の値に戻す
                if (($("#tempActvctgryidHidden").val() != "")) {
                    $("#actvctgryidHidden").val($("#tempActvctgryidHidden").val());
                    $("#reasonidHidden").val($("#tempReasonidHidden").val());
                    $("#actvctgryNameHidden").val($("#tempActvctgrynmHidden").val());
                    $("#reasonNameHidden").val($("#tempReasonnmHidden").val());

                    var str = ""
                    str = $("#actvctgryNameHidden").val();
                    if ($("#reasonidHidden").val() != "") {
                        str = str + "-";
                        str = str + $("#reasonNameHidden").val();
                    }
                    $("#actvctgryLabel").text(str);

                    // 活動区分リスト初期チェックセット
                    actvctgrylist("#scNscCustomerEditingWindown");

                    // 断念理由リスト初期チェックセット
                    reasonidlist("#scNscCustomerEditingWindown");
                }

                //1ページ目表示設定
                setPopupCustomerEditPage("page1");
            } else {
                //2ページ目表示設定
                setPopupCustomerEditPage("page2");
            }
        }

        e.stopImmediatePropagation();
    });




    // 車両編集　完了クリック　------------------------------------------------
    //    $("#scVehicleEditingWindown #scVehicleEditingWindownBox .scVehicleEditingCompletionButton").click(function (e) {
    $("#scVehicleEditingWindown #scVehicleEditingWindownBox .scVehicleEditingCompletionButton").live("click",
        function (e) {

            if ($("#serverProcessFlgHidden").val() == "1") {  //サーバーサイド処理フラグ判定 (1:処理中)
                return;
            }

            //2:未取引客時のみ
            if ($("#custFlgHidden").val() == "2") {
                //モデル未入力エラー
                if ($("#vehicleNoModelErrMsg").val() == "") {
                    alert($("#modelTextBox").val());
                    return;
                }
            }

            var prms = "";
            prms = prms + $("#makerTextBox").val() + ",";                  //メーカー
            prms = prms + $("#modelTextBox").val() + ",";                  //モデル
            prms = prms + $("#vclregnoTextBox").val() + ",";               //車両登録No
            prms = prms + $("#vinTextBox").val() + ",";                    //VIN

            prms = prms + $("#vcldelidateDateTime").val() + ",";           //納車日
            prms = prms + $("#editVehicleModeHidden").val() + ",";         //処理モード

            prms = prms + $("#actvctgryidHidden").val() + ",";             //活動区分
            prms = prms + $("#reasonidHidden").val() + ",";                //断念理由

            //処理中フラグを立てる
            $("#serverProcessFlgHidden").val("1");         //サーバーサイド処理フラグ


            callback.doCallback("VehicleUpdate", prms, function (result, context) {

                $("#serverProcessFlgHidden").val("");      //サーバーサイド処理フラグ

                var resArray = result.split(",");
                if (resArray[1] == "0") {
                    //$(".scVehicleEditingCancellButton").click();

                    //編集モードにする
                    $("#editVehicleModeHidden").val("1");

                    //SC3080201.endServerCallback();

                    //タイトル等変更処理
                    changeVehicleMode()

                    // 車両編集-更新前情報の情報を保存する
                    backUpVehicleInfo()

                    //車両編集（新規登録、編集時両方共通）
                    CustomerCarEditPopUpClose()

                    //車両編集画面を閉じる
                    $("#scVehicleEditingWindown").fadeOut(300);

                } else {
                    alert(resArray[2]);
                    //SC3080201.endServerCallback();
                }
            });
        }
    );
    //    });

    // 車両編集　キャンセルクリック　-------------------------------------------------
    //    $("#scVehicleEditingWindown #scVehicleEditingWindownBox .scVehicleEditingCancellButton").click(function (e) {
    $("#scVehicleEditingWindown #scVehicleEditingWindownBox .scVehicleEditingCancellButton").live("click",
        function (e) {
            $("#vehicleCancelButtonLabel").click();
        });
    //    });

    // 車両編集　キャンセルクリック　-------------------------------------------------
    $("#vehicleCancelButtonLabel").click(function (e) {
        var page = $("#vehiclePageHidden").val();
        if (page == "page1") {

            //画面を閉じる
            $("#scVehicleEditingWindown").fadeOut(200);

            // 車両編集-キャンセル-変更前情報の情報に戻す
            cancelVehicleInfo();

            //念のためフラグをクリアする
            $("#serverProcessFlgHidden").val("");
            SC3080201.endServerCallback();

        } else {
            if (page == "page2") {

                //活動区分でキャンセルボタンを押下した場合は、起動前の値に戻す
                if ($("#tempActvctgryidHidden").val() != "") {
                    $("#actvctgryidHidden").val($("#tempActvctgryidHidden").val());
                    $("#reasonidHidden").val($("#tempReasonidHidden").val());
                    $("#actvctgryNameHidden").val($("#tempActvctgrynmHidden").val());
                    $("#reasonNameHidden").val($("#tempReasonnmHidden").val());

                    var str = ""
                    str = $("#actvctgryNameHidden").val();
                    if ($("#reasonidHidden").val() != "") {
                        str = str + "-";
                        str = str + $("#reasonNameHidden").val();
                    }
                    $("#actvctgryLabel2").text(str);

                    // 活動区分リスト初期チェックセット
                    actvctgrylist("#scVehicleEditingWindown");

                    // 断念理由リスト初期チェックセット
                    reasonidlist("#scVehicleEditingWindown");
                }

                //1ページ目表示設定
                setPopupVehiclePage("page1");
            } else {
                //2ページ目表示設定
                setPopupVehiclePage("page2");
            }
        }

        e.stopImmediatePropagation();
    });


    // 保有車両を追加クリック　-------------------------------------------------
    //    $("#scVehicleEditingWindown #scVehicleEditingWindownBox .scVehicleAppendButton").click(function (e) {
    $("#scVehicleEditingWindown #scVehicleEditingWindownBox .scVehicleAppendButton").live("click",
        function (e) {
            if ($("#serverProcessFlgHidden").val() == "1") {  //サーバーサイド処理フラグ判定 (1:処理中)
                return;
            }

            var prms = "";

            $("#serverProcessFlgHidden").val("1");         //サーバーサイド処理フラグ
            SC3080201.startServerCallback();

            callback.doCallback("VehicleAppend", prms, function (result, context) {

                $("#serverProcessFlgHidden").val("");      //サーバーサイド処理フラグ
                SC3080201.endServerCallback();

                var resArray = result.split(",");
                if (resArray[1] == "0") {
                    //各項目の内容をクリアする
                    $("#makerTextBox").CustomTextBox("updateText", "");
                    $("#modelTextBox").CustomTextBox("updateText", "");
                    $("#vclregnoTextBox").CustomTextBox("updateText", "");
                    $("#vinTextBox").CustomTextBox("updateText", "");
                    $("#vcldelidateDateTime").val("");
                    $("#editVehicleModeHidden").val("0");

                    //タイトル等の変更処理
                    changeVehicleMode();

                } else {
                    alert(resArray[2]);
                }
            });
        }
    );
    //    });

    // 顧客編集ポップアップ関連 -------------------------------------------------------------------
    // 顧客編集ポップアップ設定
    setPopupCustomerEditIinital();

    //スクロール設定
    $("#scNscCustomerEditingWindown #scNscCustomerEditingWindownBox .scNscCustomerEditingListBox2").fingerScroll();
    $("#scNscCustomerEditingWindown #scNscCustomerEditingWindownBox .dataWindNameTitle .ListBox01").fingerScroll();

    //ポップアップクローズの監視
    $(document.body).bind("mousedown touchstart", function (e) {
        if ($("#scNscCustomerEditingWindown").is(":visible") === false) return;
        if ($(e.target).is("#scNscCustomerEditingWindown, #scNscCustomerEditingWindown *") === false) {
            $("#scNscCustomerEditingWindown").fadeOut(200);

            // 顧客編集-キャンセル-変更前情報の情報に戻す
            cancelCustomerInfo();
        }
    });

    // 車両編集ポップアップ関連 -------------------------------------------------------------------
    // 車両編集ポップアップ設定
    setPopupVehicleIinital();

    //スクロール設定
    //1:自社客時のみ
    if ($("#custFlgHidden").val() == "1") {
        $("#scVehicleEditingWindown #scVehicleEditingWindownBox .scVehicleEditingListBox2").fingerScroll();
        //$("#scNscCustomerEditingWindown #scNscCustomerEditingWindownBox .dataWindNameTitle .ListBox01").fingerScroll();
    }

    //ポップアップクローズの監視
    $(document.body).bind("mousedown touchstart", function (e) {
        if ($("#scVehicleEditingWindown").is(":visible") === false) return;
        if ($(e.target).is("#scVehicleEditingWindown, #scVehicleEditingWindown *") === false) {
            $("#scVehicleEditingWindown").fadeOut(200);

            // 車両編集-キャンセル-変更前情報の情報に戻す
            cancelVehicleInfo();
        }
    });

});

// 顧客編集-更新後情報の情報を保存する
function backUpCustomerInfo() {

    //変更前情報の情報を保存する
    $("#nameTextBoxBackHidden").val($("#nameTextBox").val());
    $("#nameTitleTextHiddenBackHidden").val($("#nameTitle").val());
    $("#nameTitleHiddenBackHidden").val($("#nameTitleHidden").val());
    $("#nameTitleTextHiddenBackHidden").val($("#nameTitleTextHidden").val());
    if ($("#manCheckBox").attr("checked")) {
        $("#manCheckBoxBackHidden").val("on");
    }else{
        $("#manCheckBoxBackHidden").val("");
    }
    if ($("#girlCheckBox").attr("checked")) {
        $("#girlCheckBoxBackHidden").val("on");
    }else{
        $("#girlCheckBoxBackHidden").val("");
    }
    if ($("#kojinCheckBox").attr("checked")) {
        $("#kojinCheckBoxBackHidden").val("on");
    }else{
        $("#kojinCheckBoxBackHidden").val("");
    }
    if ($("#houjinCheckBox").attr("checked")) {
        $("#houjinCheckBoxBackHidden").val("on");
    }else{
        $("#houjinCheckBoxBackHidden").val("");
    }
    $("#employeenameTextBoxBackHidden").val($("#employeenameTextBox").val());
    $("#employeedepartmentTextBoxBackHidden").val($("#employeedepartmentTextBox").val());
    $("#employeepositionTextBoxBackHidden").val($("#employeepositionTextBox").val());
    $("#mobileTextBoxBackHidden").val($("#mobileTextBox").val());
    $("#telnoTextBoxBackHidden").val($("#telnoTextBox").val());
    $("#businesstelnoTextBoxBackHidden").val($("#businesstelnoTextBox").val());
    $("#faxnoTextBoxBackHidden").val($("#faxnoTextBox").val());
    $("#zipcodeTextBoxBackHidden").val($("#zipcodeTextBox").val());
    $("#addressTextBoxBackHiddenx").val($("#addressTextBox").val());
    $("#email1TextBoxBackHidden").val($("#email1TextBox").val());
    $("#email2TextBoxBackHidden").val($("#email2TextBox").val());
    $("#socialidTextBoxBackHidden").val($("#socialidTextBox").val());
    $("#birthdayTextBoxBackHidden").val($("#birthdayTextBox").val());

    if ($("#smsCheckButton").attr("checked")) {
        $("#smsCheckButtonBackHidden").val("on");
    }else{
        $("#smsCheckButtonBackHidden").val("");
    }
    if ($("#emailCheckButton").attr("checked")) {
        $("#emailCheckButtonBackHidden").val("on");
    }else{
        $("#emailCheckButtonBackHidden").val("");
    }

    //2:未取引客時のみ
    if ($("#custFlgHidden").val() == "2") {
        $("#actvctgryidHiddenBackHidden").val($("#actvctgryidHidden").val());
        $("#reasonidHiddenBackHidden").val($("#reasonidHidden").val());
        $("#actvctgryLabelBackHidden").val($("#actvctgryLabel").html());

        $("#actvctgryNameBackHidden").val($("#actvctgryNameHidden").val());            //活動区分名称
        $("#reasonNameBackHidden").val($("#reasonNameHidden").val());                  //活動断念理由名称
    }
}

// 顧客編集-キャンセル-変更前情報の情報に戻す
function cancelCustomerInfo() {

    //変更前情報の情報に戻す
    $("#nameTextBox").CustomTextBox("updateText", $("#nameTextBoxBackHidden").val());
    $("#nameTitle").CustomTextBox("updateText", $("#nameTitleTextHiddenBackHidden").val());
    $("#nameTitleHidden").val($("#nameTitleHiddenBackHidden").val());
    $("#nameTitleTextHidden").val($("#nameTitleTextHiddenBackHidden").val());
    setCheck($("#manCheckBox"), ($("#manCheckBoxBackHidden").val() == "on"));
    setCheck($("#girlCheckBox"), ($("#girlCheckBoxBackHidden").val() == "on"));
    setCheck($("#kojinCheckBox"), ($("#kojinCheckBoxBackHidden").val() == "on"));
    setCheck($("#houjinCheckBox"), ($("#houjinCheckBoxBackHidden").val() == "on"));
    $("#employeenameTextBox").CustomTextBox("updateText", $("#employeenameTextBoxBackHidden").val());
    $("#employeedepartmentTextBox").CustomTextBox("updateText", $("#employeedepartmentTextBoxBackHidden").val());
    $("#employeepositionTextBox").CustomTextBox("updateText", $("#employeepositionTextBoxBackHidden").val());
    $("#mobileTextBox").CustomTextBox("updateText", $("#mobileTextBoxBackHidden").val());
    $("#telnoTextBox").CustomTextBox("updateText", $("#telnoTextBoxBackHidden").val());
    $("#businesstelnoTextBox").CustomTextBox("updateText", $("#businesstelnoTextBoxBackHidden").val());
    $("#faxnoTextBox").CustomTextBox("updateText", $("#faxnoTextBoxBackHidden").val());
    $("#zipcodeTextBox").CustomTextBox("updateText", $("#zipcodeTextBoxBackHidden").val());
    $("#addressTextBox").CustomTextBox("updateText", $("#addressTextBoxBackHiddenx").val());
    $("#email1TextBox").CustomTextBox("updateText", $("#email1TextBoxBackHidden").val());
    $("#email2TextBox").CustomTextBox("updateText", $("#email2TextBoxBackHidden").val());
    $("#socialidTextBox").CustomTextBox("updateText", $("#socialidTextBoxBackHidden").val());
    $("#birthdayTextBox").val($("#birthdayTextBoxBackHidden").val());
    setCheck($("#smsCheckButton"), ($("#smsCheckButtonBackHidden").val() == "on"));
    setCheck($("#emailCheckButton"), ($("#emailCheckButtonBackHidden").val() == "on"));

    //法人
    if ($("input[id=houjinCheckBox]:checked").val() == 'on') {
        //法人情報の表示
        $("#houjinPanel").css("display", "block");
    } else {
        //法人情報の非表示
        $("#houjinPanel").css("display", "none");
    }

    // 敬称リスト初期チェックセット
    changeNamelist("#scNscCustomerEditingWindown");

    //2:未取引客時のみ
    if ($("#custFlgHidden").val() == "2") {
        $("#actvctgryidHidden").CustomTextBox("updateText", $("#actvctgryidHiddenBackHidden").val());
        $("#reasonidHidden").CustomTextBox("updateText", $("#reasonidHiddenBackHidden").val());

        $("#actvctgryidHidden").val($("#actvctgryidHiddenBackHidden").val());
        $("#reasonidHidden").val($("#reasonidHiddenBackHidden").val());

        $("#actvctgryLabel").html($("#actvctgryLabelBackHidden").val());

        // 活動区分リスト初期チェックセット
        actvctgrylist("#scNscCustomerEditingWindown");

        // 断念理由リスト初期チェックセット
        reasonidlist("#scNscCustomerEditingWindown");

        //活動区分ワーク関係
        $("#tempActvctgryidHidden").val("");
        $("#tempReasonidHidden").val("");
        $("#tempActvctgrynmHidden").val("");
        $("#tempReasonnmHidden").val("");

        $("#actvctgryNameHidden").val($("#actvctgryNameBackHidden").val());            //活動区分名称
        $("#reasonNameHidden").val($("#reasonNameBackHidden").val());                  //活動断念理由名称
    }

}

// 車両編集-更新前情報の情報を保存する
function backUpVehicleInfo() {

    //変更前情報の情報を保存する
    $("#makerTextBoxBackHidden").val($("#makerTextBox").val());                     //メーカー
    $("#modelTextBoxBackHidden").val($("#modelTextBox").val());                     //モデル
    $("#vclregnoTextBoxBackHidden").val($("#vclregnoTextBox").val());               //車両登録No
    $("#vinTextBoxBackHidden").val($("#vinTextBox").val());                         //VIN
    $("#vcldelidateDateTimeBackHidden").val($("#vcldelidateDateTime").val());       //納車日
    $("#editVehicleModeBackHidden").val($("#editVehicleModeHidden").val());         //処理モード
    $("#actvctgryLabel2BackHidden").val($("#actvctgryLabel2").html());              //活動区分名

    //1:自社客時のみ
    if ($("#custFlgHidden").val() == "1") {
        $("#actvctgryidHiddenBackHidden").val($("#actvctgryidHidden").val());           //活動区分
        $("#reasonidHiddenBackHidden").val($("#reasonidHidden").val());        	        //断念理由

        $("#actvctgryNameBackHidden").val($("#actvctgryNameHidden").val());            //活動区分名称
        $("#reasonNameBackHidden").val($("#reasonNameHidden").val());                  //活動断念理由名称

    }
}

// 車両編集-キャンセル-変更前情報の情報に戻す
function cancelVehicleInfo() {

    //変更前情報の情報に戻す
    $("#makerTextBox").CustomTextBox("updateText", $("#makerTextBoxBackHidden").val());         //メーカー
    $("#modelTextBox").CustomTextBox("updateText", $("#modelTextBoxBackHidden").val());         //モデル
    $("#vclregnoTextBox").CustomTextBox("updateText", $("#vclregnoTextBoxBackHidden").val());   //車両登録No
    $("#vinTextBox").CustomTextBox("updateText", $("#vinTextBoxBackHidden").val());             //VIN
    $("#vcldelidateDateTime").val($("#vcldelidateDateTimeBackHidden").val());                   //納車日
    $("#editVehicleModeHidden").val($("#editVehicleModeBackHidden").val());                     //処理モード

    //1:自社客時のみ
    if ($("#custFlgHidden").val() == "1") {
        $("#actvctgryLabel2").html($("#actvctgryLabel2BackHidden").val());                          //活動区分名
        $("#actvctgryidHidden").val($("#actvctgryidHiddenBackHidden").val());                       //活動区分
        $("#reasonidHidden").val($("#reasonidHiddenBackHidden").val());                             //断念理由

        // 活動区分リスト初期チェックセット
        actvctgrylist("#scVehicleEditingWindown");

        // 断念理由リスト初期チェックセット
        reasonidlist("#scVehicleEditingWindown");

        //活動区分ワーク関係
        $("#tempActvctgryidHidden").val("");
        $("#tempReasonidHidden").val("");
        $("#tempActvctgrynmHidden").val("");
        $("#tempReasonnmHidden").val("");

        $("#actvctgryNameHidden").val($("#actvctgryNameBackHidden").val());            //活動区分名称
        $("#reasonNameHidden").val($("#reasonNameBackHidden").val());                  //活動断念理由名称
    }
}

// 顧客情報クリック時に顧客編集ポップアップ表示　
function CustomerEditPopUpOpen() {
    //顧客詳細表示
    $("#scNscCustomerEditingWindown").fadeIn(300);
    setPopupCustomerEditPage("page1");
}


// 車両情報クリック時に車両編集ポップアップ表示　
function CustomerCarEditPopUpOpen() {
    //ポップアップ設定
    cancelVehicleInfo();

    //車両編集表示
    changeVehicleMode();
    $("#scVehicleEditingWindown").fadeIn(300);
    setPopupVehiclePage("page1");
}

// 車両編集ポップアップ設定　-------------------------------------------------
function setPopupVehicleIinital() {

    //1ページ目表示設定
    setPopupVehiclePage("page1");

    //活動区分
    page = $("#scActvctgryPopWindown .popWind .dataWind1 .ListBox01");
    page = page.clone(true);

    $("#scVehicleEditingWindown .dataWindActvctgry").append(page);

    //情報不備詳細
    page = $("#scReasonPopWindown .popWind .dataWind1 .ListBox01");
    page = page.clone(true);

    $("#scVehicleEditingWindown .dataWindReason").append(page);

    // 活動区分リスト初期チェックセット
    actvctgrylist("#scVehicleEditingWindown");

    // 断念理由リスト初期チェックセット
    reasonidlist("#scVehicleEditingWindown");

    //活動区分リスト押下
    $("#scVehicleEditingWindown").find(".scVehicleEditingActvctgry").click(function (e) {

        //キャンセル時にもとの値に戻すため初期値を保存する
        $("#tempActvctgryidHidden").val($("#actvctgryidHidden").val());
        $("#tempReasonidHidden").val($("#reasonidHidden").val());
        $("#tempActvctgrynmHidden").val($("#actvctgryNameHidden").val());
        $("#tempReasonnmHidden").val($("#reasonNameHidden").val());

        //活動区分選択時処理
        $("#scVehicleEditingWindown .dataWindActvctgry .actvctgrylist").click(function (e) {

            var cd = $(this).children(".actvctgryHidden").text();
            var nm = $(this).children(".actvctgryLabel").text();
            $("#actvctgryidHidden").val(cd);
            $("#actvctgryNameHidden").val(nm);
            $("#scVehicleEditingWindown .dataWindActvctgry .actvctgrylist").removeClass("Selection");
            $(this).addClass("Selection");

            $("#actvctgryLabel2").text(nm);

            if (cd == "2") {    //2:情報不備の場合のみ情報不備詳細へ遷移する

                //情報不備詳細選択
                $("#scVehicleEditingWindown .dataWindReason .reasonlist").click(function (e) {

                    var cd2 = $(this).children(".reasoncdHidden").text();
                    var nm2 = $(this).children(".reasoncdLabel").text();
                    $("#reasonidHidden").val(cd2);
                    $("#reasonNameHidden").val(nm2);
                    $("#scVehicleEditingWindown .dataWindReason .reasonlist").removeClass("Selection");
                    $(this).addClass("Selection");

                    var str = ""
                    str = str + $("#actvctgryLabel2").text();
                    str = str + "-";
                    str = str + nm2;
                    $("#actvctgryLabel2").text(str);

                    setPopupVehiclePage("page1");

                    e.stopImmediatePropagation();
                });

                setPopupVehiclePage("page3");

            } else {
                $("#scVehicleEditingWindown .dataWindReason .reasonlist").removeClass("Selection");
                $("#reasonidHidden").val("");

                setPopupVehiclePage("page1");
            }
        });

        setPopupVehiclePage("page2", "actvctgryList");

    });

}

// 車両編集表示 (ページ指定)　-------------------------------------------------
function setPopupVehiclePage(page, subId) {

    //ページ番号セット
    $("#vehiclePageHidden").val(page);

    $("#scVehicleEditingWindown #scVehicleEditingWindownBox .scVehicleEditingListBox").removeClass("page1 page2 page3").addClass(page);

    //タイトルを変更する
    var strCancelLable = "";
    var strTitleLable = "";
    var strCompletionLable = "";
    if (page == "page1") {

        $(".scVehicleEditingCompletionButton").show(0);                    //右ボタンを表示

        strCompletionLable = $("#completionLabel").text();                     //登録
        strCancelLable = $("#cancelLabel").text(); //キャンセル

        if (($("#editVehicleModeHidden").val() == "0")) {
            //追加時
            strTitleLable = $("#createVehicleLabel").text();
        } else {
            //更新時
            strTitleLable = $("#editVehicleLabel").text();
        }
    } else {

        $(".scVehicleEditingCompletionButton").hide(0);                    //右ボタンは非表示

        if (page == "page2") {
            if (($("#editVehicleModeHidden").val() == "0")) {
                //追加時
                strCancelLable = $("#createVehicleLabel").text();
            } else {
                //更新時
                strCancelLable = $("#editVehicleLabel").text();
            }
            strTitleLable = $("#actvctgryTitleLabel").text();

        } else {

            strCancelLable = $("#reasonBackLabel").text();

            strTitleLable = $("#reasonTitleLabel").text();
        }
    }

    $("#vehicleCancelButtonLabel").text(strCancelLable);           //キャンセルボタン
    $("#vehicleCompletionButtonLabel").text(strCompletionLable);   //登録ボタン
    $("#vehicleTitleLabel").text(strTitleLable);           //タイトル

    $("#vehicleCancelButtonLabel").CustomLabel({ 'useEllipsis': 'true' });     //TODO:できてない
    $("#vehicleCompletionButtonLabel").CustomLabel({ 'useEllipsis': 'true' });
}



// 顧客編集ポップアップ設定　-------------------------------------------------
function setPopupCustomerEditIinital() {


    //1ページ目表示設定
    setPopupCustomerEditPage("page1");
    
    //敬称
    page = $("#scNameTitlePopWindown .popWind .dataWind1 .ListBox01");
    page = page.clone(true);

    $("#scNscCustomerEditingWindown .dataWindNameTitle").append(page);
 
    //敬称リスト押下
    $("#scNscCustomerEditingWindown").find(".scNscCustomerEditingNameTitle").click(function (e) {

        if ($("#useNameTitleHidden").val() == "1") {

            //キャンセル時にもとの値に戻すため初期値を保存する
            $("#tempActvctgryidHidden").val("");
            $("#tempReasonidHidden").val("");
            $("#tempActvctgrynmHidden").val("");
            $("#tempReasonnmHidden").val("");

            //敬称選択時処理
            $("#scNscCustomerEditingWindown .dataWindNameTitle .nameTitlelist").click(function (e) {

                var cd = $(this).children(".namecdHidden").text();
                var nm = $(this).children(".nameTitleLabel").text();
                $("#nameTitleHidden").val(cd);
                $("#nameTitleTextHidden").val(nm);
                $("#nameTitle").CustomTextBox("updateText", nm);
                $("#scNscCustomerEditingWindown .dataWindNameTitle .nameTitlelist").removeClass("Selection");
                $(this).addClass("Selection");

                setPopupCustomerEditPage("page1");

            });

            setPopupCustomerEditPage("page2", "nameTitleList");
        }
    });

    //活動区分
    page = $("#scActvctgryPopWindown .popWind .dataWind1 .ListBox01");
    page = page.clone(true);

    $("#scNscCustomerEditingWindown .dataWindActvctgry").append(page);

    //情報不備詳細
    page = $("#scReasonPopWindown .popWind .dataWind1 .ListBox01");
    page = page.clone(true);

    $("#scNscCustomerEditingWindown .dataWindReason").append(page);

    // 敬称リスト初期チェックセット
    changeNamelist("#scNscCustomerEditingWindown");

    // 活動区分リスト初期チェックセット
    actvctgrylist("#scNscCustomerEditingWindown");

    // 断念理由リスト初期チェックセット
    reasonidlist("#scNscCustomerEditingWindown");
    
    //活動区分リスト押下
    $("#scNscCustomerEditingWindown").find(".scNscCustomerEditingActvctgry").click(function (e) {

        if ($("#useActvctgryHidden").val() == "1") {

            //キャンセル時にもとの値に戻すため初期値を保存する
            $("#tempActvctgryidHidden").val($("#actvctgryidHidden").val());
            $("#tempReasonidHidden").val($("#reasonidHidden").val());
            $("#tempActvctgrynmHidden").val($("#actvctgryNameHidden").val());
            $("#tempReasonnmHidden").val($("#reasonNameHidden").val());

            //活動区分選択時処理
            $("#scNscCustomerEditingWindown .dataWindActvctgry .actvctgrylist").click(function (e) {

                var cd = $(this).children(".actvctgryHidden").text();
                var nm = $(this).children(".actvctgryLabel").text();
                $("#actvctgryidHidden").val(cd);
                $("#actvctgryNameHidden").val(nm);
                $("#scNscCustomerEditingWindown .dataWindActvctgry .actvctgrylist").removeClass("Selection");
                $(this).addClass("Selection");

                $("#actvctgryLabel").text(nm);

                if (cd == "2") {    //2:情報不備の場合のみ情報不備詳細へ遷移する

                    //情報不備詳細選択
                    $("#scNscCustomerEditingWindown .dataWindReason .reasonlist").click(function (e) {

                        var cd2 = $(this).children(".reasoncdHidden").text();
                        var nm2 = $(this).children(".reasoncdLabel").text();
                        $("#reasonidHidden").val(cd2);
                        $("#reasonNameHidden").val(nm2);
                        $("#scNscCustomerEditingWindown .dataWindReason .reasonlist").removeClass("Selection");
                        $(this).addClass("Selection");

                        var str = ""
                        str = str + $("#actvctgryLabel").text();
                        str = str + "-";
                        str = str + nm2;
                        
                        $("#actvctgryLabel").text(str);

                        setPopupCustomerEditPage("page1");

                        e.stopImmediatePropagation();
                        
                    });

                    setPopupCustomerEditPage("page3");

                } else {
                    $("#scNscCustomerEditingWindown .dataWindReason .reasonlist").removeClass("Selection");
                    $("#reasonidHidden").val("");

                    setPopupCustomerEditPage("page1");
                }
            });
            
            setPopupCustomerEditPage("page2", "actvctgryList");
        }

    });
    
}

// 顧客編集表示 (ページ指定)　-------------------------------------------------
function setPopupCustomerEditPage(page, subId) {

    //ページ番号セット
    $("#custPageHidden").val(page);

    $("#scNscCustomerEditingWindown #scNscCustomerEditingWindownBox .scNscCustomerEditingListBox").removeClass("page1 page2 page3").addClass(page);

    if (page == "page2") {
        if (subId == "nameTitleList") {
            //敬称リスト
            $("#scNscCustomerEditingWindown .dataWindNameTitle").css("display", "block");
            $("#scNscCustomerEditingWindown .dataWindActvctgry").css("display", "none");
            
        } else {
            //活動区分リスト
            $("#scNscCustomerEditingWindown .dataWindNameTitle").css("display", "none");
            $("#scNscCustomerEditingWindown .dataWindActvctgry").css("display", "block");
        }
    }

    //タイトルを変更する
    var strCancelLable = "";
    var strTitleLable = "";
    var strCompletionLable = "";
    if (page == "page1") {

        $(".scNscCustomerEditingCompletionButton").show(0);                    //右ボタンを表示

        strCompletionLable = $("#completionLabel").text();                     //登録
        strCancelLable = $("#cancelLabel").text();                          　 //キャンセル

        if (($("#editModeHidden").val() == "0")) {
            //追加時
            strTitleLable = $("#createCustomerLabel").text();
        } else {
            //更新時
            strTitleLable = $("#editCustomerLabel").text();
        }
    } else {

        $(".scNscCustomerEditingCompletionButton").hide(0);                    //右ボタンは非表示

        //キーボードを消すためキャンセルボタンにフォーカスセットする
        $(".scNscCustomerEditingCancellButton").focus();

        if (page == "page2") {
            if (($("#editModeHidden").val() == "0")) {
                //追加時
                strCancelLable = $("#createCustomerLabel").text();
            } else {
                //更新時
                strCancelLable = $("#editCustomerLabel").text();
            }
            if (subId == "nameTitleList") {
                strTitleLable = $("#nameTitleLabel").text();
            } else {
                strTitleLable = $("#actvctgryTitleLabel").text();
            }

        } else {

            strCancelLable = $("#reasonBackLabel").text();

            strTitleLable = $("#reasonTitleLabel").text();
        }
    }

    $("#cancelButtonLabel").text(strCancelLable);           //キャンセルボタン
    $("#completionButtonLabel").text(strCompletionLable);   //登録ボタン
    $("#customerTitleLabel").text(strTitleLable);           //タイトル

    $("#cancelButtonLabel").CustomLabel({ 'useEllipsis': 'true' });     //TODO:できてない
    $("#completionButtonLabel").CustomLabel({ 'useEllipsis': 'true' });
}




// 顧客編集　敬称リストの切り替え　-------------------------------------------------
function changeNamelist(parentTag) {

    $(parentTag + " " + ".nameTitlelist").removeClass("Selection");

    var nameTitleCd = $("#nameTitleHidden").val();

    var $namelist = $(parentTag + " " + ".dataWindNameTitle ul.nscListBoxSetIn").children();

    var endrow = -1;

    //敬称リストの表示切替
    //表示区分＝(0:常に表示  1: 個人のみ表示  2: 法人のみ表示)
    for (i = 0; i < $namelist.length; i++) {
        var flg = true;     //true:表示する／false:表示しない
        var namecd = $(parentTag + " " + "#" + $namelist[i].id + "").children(".namecdHidden").text();    //敬称コード
        var dispflg = $(parentTag + " " + "#" + $namelist[i].id + "").children(".dispHidden").text();     //表示区分

        
        //個人チェック
        if ($("#kojinCheckBox").attr("checked")) {
            if (dispflg == "2") {
                flg = false;
            }
        }

        //法人チェック
        if ($("#houjinCheckBox").attr("checked")) {
            if (dispflg == "1") {
                flg = false;
            }
        }

        if (flg == true) {
            //表示する
            $(parentTag + " " + "#" + $namelist[i].id + "").css("display", "list-item");

            //選択状態にする
            if (namecd == nameTitleCd) {
                $(parentTag + " " + "#" + $namelist[i].id + "").addClass("Selection");
            }

            endrow = i;
        } else {
            //表示しない
            $(parentTag + " " + "#" + $namelist[i].id + "").css("display", "none");

            //選択状態から外す
            if (namecd == nameTitleCd) {
                $("#nameTitleHidden").val("");
                $("#nameTitle").CustomTextBox("updateText", "");

                $(parentTag + " " + "#" + $namelist[i].id + "").removeAttr("Selection");
            }
        }
    }

    if (endrow >= 0) {
        $(parentTag + " " + "#" + $namelist[endrow].id + "").addClass("endRow");
    }

    //敬称コードがない場合は、名称をそのまま出力する
    if (nameTitleCd == "") {
        $("#nameTitleHidden").val("");
        if ($("#nameTitleTextHiddenBackHidden").val() != "") {
            $("#nameTitle").CustomTextBox("updateText", $("#nameTitleTextHiddenBackHidden").val());
            return true;
        }
    }       
}

// 活動区分　-------------------------------------------------
function actvctgrylist(parentTag) {

    $(parentTag + " " + ".actvctgrylist").removeClass("Selection");

    var cdhidden = $("#actvctgryidHidden").val();

    var $list2 = $(parentTag + " " + " ul.actvctgryListBoxSetIn").children();

    for (i = 0; i < $list2.length; i++) {
        var cd = $(parentTag + " " + "#" + $list2[i].id + "").children(".actvctgryHidden").text();

        //選択状態にする
        if (cd == cdhidden) {
            $(parentTag + " " + "#" + $list2[i].id + "").addClass("Selection");
        }
    }

    $(parentTag + " " + "#" + $list2[$list2.length - 1].id + "").css("border-bottom", "none");

    return true;
}

// 断念理由　-------------------------------------------------
function reasonidlist(parentTag) {

    $(parentTag + " " + ".reasonlist").removeClass("Selection");

    var cdhidden = $("#reasonidHidden").val();

    var $list2 = $(parentTag + " " + " ul.reasonListBoxSetIn").children();

    for (i = 0; i < $list2.length; i++) {
        var cd = $(parentTag + " " + "#" + $list2[i].id + "").children(".reasoncdHidden").text();

        //選択状態にする
        if (cd == cdhidden) {
            $(parentTag + " " + "#" + $list2[i].id + "").addClass("Selection");
        }
    }

    $(parentTag + " " + "#" + $list2[$list2.length - 1].id + "").css("border-bottom","none");

    return true;
}



// 顧客編集　個人　-------------------------------------------------
$(function () {
    $(".scKojinCheck").click(function (e) {

        if ($("#kojinCheckBox").attr("checked")) {

            //個人チェック時に、法人項目を非表示にして、法人チェックを外す
            $("#houjinPanel").css("display", "none");

            $("#houjinCheckBox").removeAttr("checked");
            var wrapperElement = $("#houjinCheckBox").parent();
            var chkimg = wrapperElement.children(".icrop-CheckMark-checked");
            if (chkimg != null) {
                chkimg.removeClass("icrop-CheckMark-checked");
            }
        }

        // 敬称リストセット
        changeNamelist("#scNscCustomerEditingWindown");

    });
});

// 顧客編集　法人　-------------------------------------------------
$(function () {
    $(".scHoujinCheck").click(function (e) {

        if ($("#houjinCheckBox").attr("checked")) {
            $("#houjinPanel").css("display", "block");

            //個人チェック時に、法人項目を表示して、個人チェックを外す
            $("#kojinCheckBox").removeAttr("checked");
            var wrapperElement = $("#kojinCheckBox").parent();
            var chkimg = wrapperElement.children(".icrop-CheckMark-checked");
            if (chkimg != null) {
                chkimg.removeClass("icrop-CheckMark-checked");
            }

        } else {
            $("#houjinPanel").css("display", "none");

        }

        // 敬称リストセット
        changeNamelist("#scNscCustomerEditingWindown");
    });
});

//郵便番号入力　-------------------------------------------------
function changeZipCode(zipTxt, searchBtn) {
    if (zipTxt.value == '') {
        searchBtn.disabled = true;
    } else {
        searchBtn.disabled = false;
    }
}


// 顧客編集　男　-------------------------------------------------
$(function () {
    $('.scMunCheck').click(function (e) {

        if ($("#manCheckBox").attr("checked")) {
            //男チェック時に、女チェックを外す
            $("#girlCheckBox").removeAttr("checked");
            var wrapperElement = $("#girlCheckBox").parent();
            var chkimg = wrapperElement.children(".icrop-CheckMark-checked");
            if (chkimg != null) {
                chkimg.removeClass("icrop-CheckMark-checked");
            }
        }
    });
    $('.scGirlCheck').click(function (e) {
        if ($("#girlCheckBox").attr("checked")) {
            //女チェック時に、男チェックを外す
            $("#manCheckBox").removeAttr("checked");
            var wrapperElement = $("#manCheckBox").parent();
            var chkimg = wrapperElement.children(".icrop-CheckMark-checked");
            if (chkimg != null) {
                chkimg.removeClass("icrop-CheckMark-checked");
            }
        }
    });
});
//顧客編集　END -------------------------------------------------------------------------------------



//車両編集　START -------------------------------------------------------------------------------------

// 初期表示設定 //
$(function () {
    //未顧客
    if ($("#custFlgHidden").val() == '2') {
        //テーブルに対して、一番したの行の罫線を消す処理
        $(".scVehicleEditingListItemBottomBorder2").css("border-bottom-width", "0px");
    }
    //法人
    if ($("input[id=houjinCheckBox]:checked").val() == 'on') {
        //法人情報の表示
        $("#houjinPanel").css("display", "block");
    }
});

//車両編集入力モード変更時　-------------------------------------------------
function changeVehicleMode() {

    //タイトルを変更する
    var strLable = "";
    if (($("#editVehicleModeHidden").val() == "0")) {
        //追加時
        strLable = $("#createVehicleLabel").text();
    } else {
        //更新時
        strLable = $("#editVehicleLabel").text();
    }
    $("#vehicleTitleLabel").text(strLable);     //タイトル

    //保有車両を追加ボタンの制御
    if (($("#custFlgHidden").val() == "2") && ($("#editVehicleModeHidden").val() == "1")) {
        //更新モードで未顧客の場合のみ表示する
        $("#newVehiclePanel").css("display", "block");
    } else {
        $("#newVehiclePanel").css("display", "none");
    }

}


//車両編集　END -------------------------------------------------------------------------------------





//コンタクト履歴タブ
$(function () {
    $("#TabAll").click(function () {
        $("#TabAll").removeClass("scNscCurriculumTabSalesOff");
        $("#TabAll").addClass("scNscCurriculumTabAllAc");
        $("#TabSales").removeClass("scNscCurriculumTabAllAc");
        $("#TabSales").addClass("scNscCurriculumTabSalesOff");
        $("#imageSales").attr("src", "../Styles/Images/SC3080201/scNscCurriculumTabIcon1.png");
        $("#imageSales").attr("width", "26");
    });
    $("#TabSales").click(function () {
        $("#TabAll").removeClass("scNscCurriculumTabAllAc");
        $("#TabAll").addClass("scNscCurriculumTabSalesOff");
        $("#TabSales").removeClass("scNscCurriculumTabSalesOff");
        $("#TabSales").addClass("scNscCurriculumTabAllAc");
        $("#imageSales").attr("src", "../Styles/Images/SC3080201/scNscCurriculumListCarIcon1.png");
        $("#imageSales").attr("width", "20");
    });
});

//顧客編集ポップアップ関連/////////////////////
//顧客情報再表示用
function CustomerEditPopUpClose() {
    $("#customerReload").click();
}

//画面全体再表示用
function CustomerInsertPopUpClose() {
    //SC3080201.showLoding();
    $("#customerReloadAll").click();
}


//顧客編集（追加）ポップアップ呼出用
window.onload = function () {
    //新規顧客時に自動起動
    if ($("#customerEditPopUpAutoOpenFlg").val() == "1") {
        $("#customerPopUpOpen").click();
    }
}



//車両編集ポップアップ関連/////////////////////
//車両情報再表示用
function CustomerCarEditPopUpClose() {
    $("#customerCarReload").click();
}




//保有車両選択ポップアップ処理
function CustomerCarSelectPopUpOpen() {
    $(".scNscSelectionListBox").fingerScroll();
    $("#scNscSelectionWindownVehicleSelect").fadeIn(300);
}


function selectCarTypeClick(val) {
    //    //ポップアップの選択状態、非選択状態の制御
    //    for (i = 1; i <= parseInt($("#CustomerCarTypeNumberLabel").text()); i++) {
    //        if (i == 1) {
    //            //1行目の制御
    //            if (i == parseInt(val)) {
    //                //1行目を選択した場合の制御
    //                styleSelect(i);
    //                $("#selectKey").val($("#customerCarKey" + i).val());

    //            } else {
    //                //1行目以外を選択した場合の制御
    //                style1Row(i);

    //            }
    //        } else {
    //            //1行目以外の制御
    //            if (i == parseInt(val)) {
    //                //選択した場合の制御
    //                styleSelect(i);
    //                $("#selectKey").val($("#customerCarKey" + i).val());
    //            } else {
    //                //選択されていない場合の制御
    //                styleNotSelect(i);

    //            }
    //        }
    //    }
    for (i = 1; i <= parseInt($("#CustomerCarTypeNumberLabel").text()); i++) {
        if (i == val) {
            $("#customerCarsSelectedHiddenField").val($("#customerCarKey" + parseInt(i)).val());
        }
    }
    $("#customerCarButtonDummy").click();
    $("#scNscSelectionWindownVehicleSelect").fadeOut(300);
    return true;
}

////$(function () {

////    $(".CarTypeSelect").live("click",
////        function () {
////            //ポップアップの選択状態、非選択状態の制御
////            for (i = 1; i <= parseInt($("#CustomerCarTypeNumberLabel").text()); i++) {
////                if (i == 1) {
////                    //1行目の制御
////                    if (i == parseInt($(this).attr("index"))) {
////                        //1行目を選択した場合の制御
////                        styleSelect(i);
////                        $("#selectKey").val($("#customerCarKey" + i).val());

////                    } else {
////                        //1行目以外を選択した場合の制御
////                        style1Row(i);

////                    }
////                } else {
////                    //1行目以外の制御
////                    if (i == parseInt($(this).attr("index"))) {
////                        //選択した場合の制御
////                        styleSelect(i);
////                        $("#selectKey").val($("#customerCarKey" + i).val());
////                    } else {
////                        //選択されていない場合の制御
////                        styleNotSelect(i);

////                    }
////                }
////            }

////            $("#customerCarButtonDummy").click();
////            $("#scNscSelectionWindownVehicleSelect").fadeOut(300);
////            return false;
////        }
////    );
////    function style1Row(i) {
////        //Class
////        $("#carTypeDivMain" + i).removeClass();
////        $("#carTypeDivMain" + i).addClass("scNscSelectionCassette1");
////        $("#customerCarSeriesTable" + i).removeClass();
////        $("#customerCarSeriesTable" + i).addClass("CarTypeBlack");
////        $("#customerCarSeriesNmTd" + i).removeClass();
////        $("#customerCarSeriesNmTd" + i).addClass("CarTypeBoldTextBlack");
////        $("#customerCarGradeDiv" + i).removeClass();
////        $("#customerCarGradeDiv" + i).addClass("scNscSelectionList1Black");
////        $("#customerCarsBdyclrnmDiv" + i).removeClass();
////        $("#customerCarsBdyclrnmDiv" + i).addClass("scNscSelectionList2Black");
////        $("#customerCarsRightTable" + i).removeClass();
////        $("#customerCarsRightTable" + i).addClass("scNscCustomerCarTypeData1");
////        //Image
////        $("#carTypeLogoP" + i).attr("ImageUrl", $("#logoNotSelectid").attr("value"));
////    }
////    function styleSelect(i) {
////        //Class
////        $("#carTypeDivMain" + i).removeClass();
////        $("#carTypeDivMain" + i).addClass("scNscSelectionCassette2");
////        $("#customerCarSeriesTable" + i).removeClass();
////        $("#customerCarSeriesTable" + i).addClass("CarTypeWhite");
////        $("#customerCarSeriesNmTd" + i).removeClass();
////        $("#customerCarSeriesNmTd" + i).addClass("CarTypeBoldTextWhite");
////        $("#customerCarGradeDiv" + i).removeClass();
////        $("#customerCarGradeDiv" + i).addClass("scNscSelectionList1White");
////        $("#customerCarsBdyclrnmDiv" + i).removeClass();
////        $("#customerCarsBdyclrnmDiv" + i).addClass("scNscSelectionList2White");
////        $("#customerCarsRightTable" + i).removeClass();
////        $("#customerCarsRightTable" + i).addClass("scNscCustomerCarTypeData2");
////        //Image
////        $("#carTypeLogoP" + i).attr("ImageUrl", $("#logoSelectid").attr("value"));

////    }
////    function styleNotSelect(i) {
////        //Class
////        $("#carTypeDivMain" + i).removeClass();
////        $("#carTypeDivMain" + i).addClass("scNscSelectionCassette3");
////        $("#customerCarSeriesTable" + i).removeClass();
////        $("#customerCarSeriesTable" + i).addClass("CarTypeBlack");
////        $("#customerCarSeriesNmTd" + i).removeClass();
////        $("#customerCarSeriesNmTd" + i).addClass("CarTypeBoldTextBlack");
////        $("#customerCarGradeDiv" + i).removeClass();
////        $("#customerCarGradeDiv" + i).addClass("scNscSelectionList1Black");
////        $("#customerCarsBdyclrnmDiv" + i).removeClass();
////        $("#customerCarsBdyclrnmDiv" + i).addClass("scNscSelectionList2Black");
////        $("#customerCarsRightTable" + i).removeClass();
////        $("#customerCarsRightTable" + i).addClass("scNscCustomerCarTypeData1");
////        //Image
////        $("#carTypeLogoP" + i).attr("ImageUrl", $("#logoNotSelectid").attr("value"));

////    }
////});
////////////////////////////////////////////////////



//顧客メモポップアップ処理/////////////////////////
function setPopupCustomerMemoOpen() {
    //セッションを設定する
    $("#CustomerMemoEditOpenButton").click();

}

function commitCompleteOpenCustomerMemoEdit() {
//    //Iframe削除
//    $("#CustomerMemoIframe").remove();
//    //Iframe作成
//    var $iframe = $("<iframe id='CustomerMemoIframe' src='./SC3080204.aspx' width='1014px' height='645px' scrolling='no' frameborder='0' style='border:2px solid #666'></iframe>");
//    //タグ追加
//    $("#CustomerMemoEdit").append($iframe);
    
    //顧客メモをスライドインする
    $("#CustomerMemoEdit").fadeIn(300);
    
    //先頭のメモを選択状態にする
    SelectFirstMemo();
    
    //$("#CustomerMemoEdit").PageLoad();

}
////////////////////////////////////////////////////


//長谷川追加分
function setPopupOccupationPageOpen() {

    $("#CustomerRelatedOccupationOtherIdHiddenField").val("");
    $("#CustomerRelatedOccupationPopupArea").fadeIn(300);
}

//function setPopupOccupationPage(page, occupationId) {

//    $("#CustomerRelatedOccupationPageArea").removeClass("page1 page2").addClass(page);

//    if (page == "page1") {
//        $("#CustomerRelatedOccupationTitleLabel").text($("#OccupationPopuupTitlePage1").val());
//        $("#CustomerRelatedOccupationOtherIdHiddenField").val("");
//        $("#CustomerRelatedOccupationPopupArea .btnL").hide(0);
//        $("#CustomerRelatedOccupationPopupArea .btnR").hide(0);
//    } else {
//        $("#CustomerRelatedOccupationTitleLabel").text($("#OccupationPopuupTitlePage2").val());
//        $("#CustomerRelatedOccupationOtherIdHiddenField").val(occupationId);
//        $("#CustomerRelatedOccupationPopupArea .btnL").show(0);
//        $("#CustomerRelatedOccupationPopupArea .btnR").show(0);
//    }
//}
function setPopupOccupationPage(page, occupationId) {
    //スライド処理
    var leftpoint = 0;
    $("#CustomerRelatedOccupationPageArea").css({ "-webkit-transition": "transform 500ms ease-in-out 0" });
    if (page == "page1") {
        $("#CustomerRelatedOccupationOtherIdHiddenField").val("");
        $("#CustomerRelatedOccupationTitleLabel").text($("#OccupationPopuupTitlePage1").val());
        $("#CustomerRelatedOccupationPopupArea .btnL").hide(0);
        $("#CustomerRelatedOccupationPopupArea .btnR").hide(0);
        leftpoint = 0;
    } else {
        $("#CustomerRelatedOccupationOtherIdHiddenField").val(occupationId);
        $("#CustomerRelatedOccupationTitleLabel").text($("#OccupationPopuupTitlePage2").val());
        $("#CustomerRelatedOccupationPopupArea .btnL").show(0);
        $("#CustomerRelatedOccupationPopupArea .btnR").show(0);
        leftpoint = -370;
    }
    $("#CustomerRelatedOccupationPageArea").removeClass("page1 page2").addClass(page).one("webkitTransitionEnd", function () {
        $("#CustomerRelatedOccupationPageArea").css({ "-webkit-transition": "none" });
        $("#CustomerRelatedOccupationPageArea").removeClass(page);
        $("#CustomerRelatedOccupationPageArea").css({ "left": leftpoint });
    });
}

function checkOtherOccupation() {

    if ($("#CustomerRelatedOccupationOtherCustomTextBox").val() == "") {
        alert($("#OccupationOtherErrMsg").val());
        return false;
    }
    return true;
}

function transitionFamilyCountBox(size) {

    $("#TriangulArrowDown").hide(0);
    $("#TriangulArrowUp").hide(0);

    if (size) {
        $("#FamilyCountBox").css({
            "-webkit-transition": "200ms linear",
            "height": "65px"
        }).one("webkitTransitionEnd", function () {
            $("#FamilyCountBox").css({ "-webkit-transition": "none" });
            $("#TriangulArrowUp").show(0);
        });
    } else {
        $("#FamilyCountBox").css({
            "-webkit-transition": "200ms linear",
            "height": "25px"
        }).one("webkitTransitionEnd", function () {
            $("#FamilyCountBox").css({ "-webkit-transition": "none" });
            $("#TriangulArrowDown").show(0);
        });
    }
}

function googleMapOpen() {
    //GoogleMap
//    var posX = 450;
//    var posY = 200;
//    var width = 520;
//    var height = 660;

    var ArrowDir = 1;
    var posX = 490;
    var posY = 220;
    var width = 500;
    var height = 657;
    //    var width = 500;
    //    var height = 640;
    var address = $("#customerAddressTextBox").val();
    
    if (address == "-") {
        return;
    }

    var query = "";
    query += "icrop:pmap";
    query += ":" + ArrowDir + ":";    
    query += ":" + posX + ":";
    query += ":" + posY + ":";
    query += ":" + width + ":";
    query += ":" + height + ":";
    query += ":" + address;

    location.href = query;
}

function photoSelectOpen() {
    //Photo
    var posX = 80;
    var posY = 150;
//    var file = $("#customerIdTextBox").val() + $("#faceFileNameTimeHiddenField").val();
    var file = $("#customerIdTextBox").val();
    var cbmethod = "CallBackCustomerPhoto";

    var query = "";
    query += "icrop:came"
    query += ":" + posX + ":";
    query += ":" + posY + ":";
    query += ":" + file + ":";
    query += ":" + cbmethod;

    location.href = query;
}

$(function () {

    bindFingerScroll();
    $(".scNscCurriculumListBox").fingerScroll();
    $(".scNscSelectionListBox").fingerScroll();

    //ポップアップクローズの監視
    $(document.body).bind("mousedown touchstart", function (e) {
        if ($("#CustomerRelatedOccupationPopupArea").is(":visible") === false) return;
        if ($(e.target).is("#CustomerRelatedOccupationPopupArea, #CustomerRelatedOccupationPopupArea *") === false) {
            $("#CustomerRelatedOccupationPopupArea").fadeOut(300);
            //画面初期化
            $("#CustomerRelatedOccupationCancelButton").click();
//            $("#CustomerRelatedOccupationPopupArea .btnL").hide(0);
//            $("#CustomerRelatedOccupationPopupArea .btnR").hide(0);

        }
    });

    $(document.body).bind("mousedown touchstart", function (e) {
        if ($("#CustomerRelatedFamilyPopupArea").is(":visible") === false) return;
        if ($(e.target).is("#CustomerRelatedFamilyPopupArea, #CustomerRelatedFamilyPopupArea *") === false) {
            g_familyPage = "page1"
            $("#CustomerRelatedFamilyPopupArea").fadeOut(300);
            //画面初期化
            $("#CustomerRelatedFamilyCancelButton").click();
        }
    });

    $(document.body).bind("mousedown touchstart", function (e) {
        if ($("#CustomerRelatedHobbyPopupArea").is(":visible") === false) return;
        if ($(e.target).is("#CustomerRelatedHobbyPopupArea, #CustomerRelatedHobbyPopupArea *") === false) {
            g_hobbyPage = "page1";
            $("#CustomerRelatedHobbyPopupArea").fadeOut(300);
            //画面初期化
            $("#CustomerRelatedHobbyPopupCancelButton").click();
        }
    });

    $(document.body).bind("mousedown touchstart", function (e) {
        if ($("#CustomerRelatedContactPopupArea").is(":visible") === false) return;
        if ($(e.target).is("#CustomerRelatedContactPopupArea, #CustomerRelatedContactPopupArea *") === false) {
            $("#CustomerRelatedContactPopupArea").fadeOut(300);
            //画面初期化
            $("#CustomerRelatedContactPopupCancelButton").click();
        }
    });

    $(document.body).bind("mousedown touchstart", function (e) {
        if ($("#scNscSelectionWindownVehicleSelect").is(":visible") === false) return;
        if ($(e.target).is("#scNscSelectionWindownVehicleSelect, #scNscSelectionWindownVehicleSelect *") === false) {
            $("#scNscSelectionWindownVehicleSelect").fadeOut(300);
        }
    });
});

function bindFingerScroll() {
    $("#CustomerRelatedFamilyPageArea .popupScrollArea").fingerScroll();
}

function CallBackCustomerPhoto(iRc) {
    if (iRc == 1) {
        $("#customerInfoUpdateButton").click();
    }
}

function RefleshCustomerInfo() {
    $("#customerInfoUpdateButton").click();
}

//家族ポップアップ処理
function setPopupFamilyPageOpen() {
    $("#familyBirthdayListBirthdayDate_Row_0").val($("#birthdayTextBox").val());
    $("#CustomerRelatedFamilyPopupArea").fadeIn(300);
}

var g_familyPage = "page1";
var g_familyRow = -1;
function setPopupFamilyPage(page, prev, row) {
    //スライド処理
    var leftpoint = 0;
    $("#CustomerRelatedFamilyPageArea").css({ "-webkit-transition": "transform 500ms ease-in-out 0" });

    g_familyPage = page;

    if (page == "page1") {
        if (prev == "page3") {
            page = "page3";
        }
        $("#CustomerRelatedFamilyPopUpTitleLabel").text($("#FamilyPopuupTitlePage1").val());
        $("#CustomerRelatedFamilyPopupArea .btnR").show(0);
        leftpoint = 0;
    } else if (page == "page2") {
        if (prev == "page1") {
            page = "page2";
            g_familyRow = row;
            var relationNo = $("#familyBirthdayListRelationNoHidden_Row_" + row).val();
            $("#familyRelationship li").removeClass("familyRelationshipOn");
            $("#familyRelationshipList_No_" + relationNo).addClass("familyRelationshipOn")

            var other = $("#RelationOtherNoHidden").val();
            if (relationNo == other) {
                var word = $("#familyBirthdayListRelationLabel_Row_" + row).text();
                $("#familyRelationshipLabel_No_" + other).text(word);
                $("#familyOtherRelationshipTextBox").val(word);
            } else {
                $("#familyRelationshipLabel_No_" + other).text($("#RelationOtherWordHidden").val());
                $("#familyOtherRelationshipTextBox").val("");
                $("#familyOtherRelationshipTextBox").CustomTextBox("updateText", "");
            }
        } else {
            page = "page1";
        }
        $("#CustomerRelatedFamilyPopUpTitleLabel").text($("#FamilyPopuupTitlePage2").val());
        $("#CustomerRelatedFamilyPopupArea .btnR").hide(0);
        leftpoint = -320;
    } else if (page == "page3") {
        page = "page2";
        $("#familyOtherRelationshipNoHidden").val(row);
        $("#CustomerRelatedFamilyPopUpTitleLabel").text($("#FamilyPopuupTitlePage3").val());
        $("#CustomerRelatedFamilyPopupArea .btnR").show(0);
        leftpoint = -640;

    }

    $("#CustomerRelatedFamilyPageArea").removeClass("page1 page2 page3").addClass(page).one("webkitTransitionEnd", function () {
        $("#CustomerRelatedFamilyPageArea").css({ "-webkit-transition": "none" });
        $("#CustomerRelatedFamilyPageArea").removeClass(page);
        $("#CustomerRelatedFamilyPageArea").css({ "left": leftpoint });
    });
//    $("#CustomerRelatedFamilyPageArea").removeClass("page1 page2 page3").addClass(page);
//    $("#CustomerRelatedFamilyPageArea").css({ "-webkit-transition": "none" });
//    $("#CustomerRelatedFamilyPageArea").removeClass(page);
//    $("#CustomerRelatedFamilyPageArea").css({ "left": leftpoint });
    
}

function CancelCustomerRelatedFamily() {

    if (g_familyPage == "page1") {
        $("#CustomerRelatedFamilyPopupArea").fadeOut(300);
        //画面初期化
        $("#CustomerRelatedFamilyCancelButton").click();
    } else if (g_familyPage == "page2") {
        g_familyRow = -1;
        setPopupFamilyPage("page1", g_familyPage);
    } else if (g_familyPage == "page3") {
        $("#familyOtherRelationshipNoHidden").val("");
        setPopupFamilyPage("page2", g_familyPage);
    }
}

function RegistCustomerRelatedFamily() {
    if (g_familyPage == "page1") {
        for (i = 0; i < 10; i++) {
            $("#familyBirthdayHidden_Row_" + i).val($("#familyBirthdayListBirthdayDate_Row_" + i).val());
        }
        return true;
    } else if (g_familyPage == "page3") {

        if ($("#familyOtherRelationshipTextBox").val() == "") {
            alert($("#RelationOtherErrMsgHidden").val());
            return false;
        }

        $("#familyBirthdayListRelationLabel_Row_" + g_familyRow).text($("#familyOtherRelationshipTextBox").val());
        $("#familyBirthdayListRelationNoHidden_Row_" + g_familyRow).val($("#RelationOtherNoHidden").val());
        $("#familyBirthdayListRelationOtherHidden_Row_" + g_familyRow).val($("#familyOtherRelationshipTextBox").val());

        setPopupFamilyPage("page1", g_familyPage);
        return false;
    }
}

function selectFamilyRelationship(relationNo) {

    $("#familyBirthdayListRelationLabel_Row_" + g_familyRow).text($("#familyRelationshipLabel_No_" + relationNo).text());
    $("#familyBirthdayListRelationNoHidden_Row_" + g_familyRow).val(relationNo);
    $("#familyBirthdayListRelationOtherHidden_Row_" + g_familyRow).val("");
    $("#familyOtherRelationshipTextBox").CustomTextBox("updateText", "");

    setPopupFamilyPage("page1", "page2");

}

function SelectFamilyCount(row) {

    $("#FamilyCount").val(row + 1);

    $("#FamilyCountBox li a").removeClass("selectedButton");
    $("#FamilyCountBox li a:eq(" + row + ")").addClass("selectedButton");

    $("#familyBirthdayListArea li").removeClass("displaynone familyBirthdayListAreaNoBorder");
    $("#familyBirthdayListArea li:eq(" + row + ")").addClass("familyBirthdayListAreaNoBorder");
    $("#familyBirthdayListArea li:gt(" + (row) + ")").addClass("displaynone");

}

//function editfamilyBirthday(row) {

//    $("#familyBirthdayHidden_Row_" + row).val($("#familyBirthdayListBirthdayDate_Row_" + row).val());
//}



//趣味関連
var g_hobbyPage = "page1";
var g_hobyRow = -1;

function setPopupHobbyPageOpen() {
    $("#CustomerRelatedHobbyPopupArea").fadeIn(300);
}

//function setCustomerRelatedHobbyPopupPage(page, row) {

//    g_hobbyPage = page;
//    g_hobyRow = row;

//    if ($("#CustomerRelatedHobbyPopupSelectButtonCheck_Row_" + row).val() == "1") {
//        g_hobbyPage = "page1";
//        selectCustomerRelatedHobbyPopupButton(row);
//        $("#CustomerRelatedHobbyPopupSelectButtonTitleLabel_Row_" + row).text($("#CustomerRelatedHobbyPopupOtherHobbyDefaultText").val());
//        $("#CustomerRelatedHobbyPopupOtherText").val("");
//        $("#CustomerRelatedHobbyPopupOtherText").CustomTextBox("updateText", "");
//        return;
//    }

//    if (page == "page1") {
//        $("#CustomerRelatedHobbyPopupTitleLabel").text($("#CustomerRelatedHobbyPopupTitlePage1").val());
//        g_hobyRow = -1;
//    } else if (page == "page2") {
//        $("#CustomerRelatedHobbyPopupTitleLabel").text($("#CustomerRelatedHobbyPopupTitlePage2").val());
//    }

//    $("#CustomerRelatedHobbyPopupPageWrap").removeClass("page1 page2").addClass(page);

//}
function setCustomerRelatedHobbyPopupPage(page, row) {
    //スライド処理
    g_hobbyPage = page;
    g_hobyRow = row;
    
    if ($("#CustomerRelatedHobbyPopupSelectButtonCheck_Row_" + row).val() == "1") {
        g_hobbyPage = "page1";
        selectCustomerRelatedHobbyPopupButton(row);
        $("#CustomerRelatedHobbyPopupSelectButtonTitleLabel_Row_" + row).text($("#CustomerRelatedHobbyPopupOtherHobbyDefaultText").val());
        $("#CustomerRelatedHobbyPopupOtherText").val("");
        $("#CustomerRelatedHobbyPopupOtherText").CustomTextBox("updateText", "");
        return;
    }

    var leftpoint = 0;
    $("#CustomerRelatedHobbyPopupPageWrap").css({ "-webkit-transition": "transform 500ms ease-in-out 0" });
    if (page == "page1") {
        $("#CustomerRelatedHobbyPopupTitleLabel").text($("#CustomerRelatedHobbyPopupTitlePage1").val());
        g_hobyRow = -1;
        leftpoint = 0;
    } else if (page == "page2") {
        $("#CustomerRelatedHobbyPopupTitleLabel").text($("#CustomerRelatedHobbyPopupTitlePage2").val());
        leftpoint = -370;
    }

    $("#CustomerRelatedHobbyPopupPageWrap").removeClass("page1 page2").addClass(page).one("webkitTransitionEnd", function () {
        $("#CustomerRelatedHobbyPopupPageWrap").css({ "-webkit-transition": "none" });
        $("#CustomerRelatedHobbyPopupPageWrap").removeClass(page);
        $("#CustomerRelatedHobbyPopupPageWrap").css({ "left": leftpoint });
    });
}

function cancelCustomerRelatedHobby() {

    if (g_hobbyPage == "page1") {
        $("#CustomerRelatedHobbyPopupArea").fadeOut(300);
        //画面初期化
        $("#CustomerRelatedHobbyPopupCancelButton").click();
    } else if (g_hobbyPage == "page2") {
        setCustomerRelatedHobbyPopupPage("page1");
    }
}

function registCustomerRelatedHobby() {

    if (g_hobbyPage == "page1") {
        return true;
    } else if (g_hobbyPage == "page2") {

        if ($("#CustomerRelatedHobbyPopupOtherText").val() == "") {
            alert($("#HobbyOthererrMsg").val());
            return false;
        }

        $("#CustomerRelatedHobbyPopupSelectButtonTitleLabel_Row_" + g_hobyRow).text($("#CustomerRelatedHobbyPopupOtherText").val());
        $("#CustomerRelatedHobbyPopupOtherHiddenField").val($("#CustomerRelatedHobbyPopupOtherText").val());
        selectCustomerRelatedHobbyPopupButton(g_hobyRow);
        setCustomerRelatedHobbyPopupPage("page1");
        return false;
    }
}

function selectCustomerRelatedHobbyPopupButton(row) {

    if ($("#CustomerRelatedHobbyPopupSelectButtonCheck_Row_" + row).val() == "1") {
        $("#CustomerRelatedHobbyPopupSelectButtonPanel_Row_" + row).css({ "background-image": "url(" + $("#CustomerRelatedHobbyPopupNotSelectedButtonPath_Row_" + row).val() + ")" })
        $("#CustomerRelatedHobbyPopupSelectButtonTitleLabel_Row_" + row).removeClass("selectedButton");
        $("#CustomerRelatedHobbyPopupSelectButtonCheck_Row_" + row).val("0");
    }else{
        $("#CustomerRelatedHobbyPopupSelectButtonPanel_Row_" + row).css({ "background-image": "url(" + $("#CustomerRelatedHobbyPopupSelectedButtonPath_Row_" + row).val() + ")" })
        $("#CustomerRelatedHobbyPopupSelectButtonTitleLabel_Row_" + row).addClass("selectedButton");
        $("#CustomerRelatedHobbyPopupSelectButtonCheck_Row_" + row).val("1");
    }

}

//連絡方法関連
function setPopupContactPageOpen() {
    $("#CustomerRelatedContactPopupArea").fadeIn(300);
}

function selectContactTool(tool) {

    var selectorLi = "";
    var selectorImage = "";
    var selectorHidden = "";
    switch (tool) {
        case 1:
            selectorLi = "#ContactToolMobileLi";
            selectorImage = "#ContactToolMobileImage";
            selectorHidden = "#ContactToolMobileHidden";
            break;
        case 2:
            selectorLi = "#ContactToolTelLi";
            selectorImage = "#ContactToolTelImage";
            selectorHidden = "#ContactToolTelHidden";
            break;
        case 3:
            selectorLi = "#ContactToolSMSLi";
            selectorImage = "#ContactToolSMSImage";
            selectorHidden = "#ContactToolSMSHidden";
            break;
        case 4:
            selectorLi = "#ContactToolEmailLi";
            selectorImage = "#ContactToolEmailImage";
            selectorHidden = "#ContactToolEmailHidden";
            break;
        case 5:
            selectorLi = "#ContactToolDMLi";
            selectorImage = "#ContactToolDMImage";
            selectorHidden = "#ContactToolDMHidden";
            break;
    }

    if ($(selectorHidden).val() == "1") {
        $(selectorLi).removeClass("scNscPopUpContactSelectBtnMiddleOn");
        $(selectorHidden).val("0");
        $(selectorImage).removeClass("selected");
    } else {
        $(selectorLi).addClass("scNscPopUpContactSelectBtnMiddleOn");
        $(selectorHidden).val("1");
        $(selectorImage).addClass("selected");
    }
}

function selectContactWeek(kind, days) {

    for (i = 0; i < days.length; i++) {
        var selector = GetWeekSelector(kind, days[i]);
        var selectorHidden = GetWeekSelectorHidden(kind, days[i]);

        if ($(selectorHidden).val() == "1") {
            $(selector).removeClass("scNscPopUpDaySelectBtnSmallOn");
            $(selectorHidden).val("0");
        } else {
            $(selector).addClass("scNscPopUpDaySelectBtnSmallOn");
            $(selectorHidden).val("1");
        }
    }
}


function selectContactWeekday(kind) {
    var delDays = [6, 7];
    for (i = 0; i < delDays.length; i++) {
        var selector = GetWeekSelector(kind, delDays[i])
        var selectorHidden = GetWeekSelectorHidden(kind, delDays[i]);
        $(selector).removeClass("scNscPopUpDaySelectBtnSmallOn");
        $(selectorHidden).val("0");
    }

    var selDays = [1, 2, 3, 4, 5];
    for (j = 0; j < selDays.length; j++) {
        var selectorHidden = GetWeekSelectorHidden(kind, selDays[j]);
        $(selectorHidden).val("0");
    }

    selectContactWeek(kind, selDays)
}

function selectContactWeekend(kind) {

    var delDays = [1, 2, 3, 4, 5];
    for (i = 0; i < delDays.length; i++) {
        var selector = GetWeekSelector(kind, delDays[i])
        var selectorHidden = GetWeekSelectorHidden(kind, delDays[i])
        $(selector).removeClass("scNscPopUpDaySelectBtnSmallOn");
        $(selectorHidden).val("0");
    }

    var selDays = [6, 7];
    for (j = 0; j < selDays.length; j++) {
        var selectorHidden = GetWeekSelectorHidden(kind, selDays[j])
        $(selectorHidden).val("0");
    }

    selectContactWeek(kind, selDays)
}

function GetWeekSelector(kind, day) {

    var selector = "";
    switch (day) {
        case 1:
            selector = "#ContactWeek" + kind + "MonLi";
            break;
        case 2:
            selector = "#ContactWeek" + kind + "TueLi";
            break;
        case 3:
            selector = "#ContactWeek" + kind + "WedLi";
            break;
        case 4:
            selector = "#ContactWeek" + kind + "TurLi";
            break;
        case 5:
            selector = "#ContactWeek" + kind + "FriLi";
            break;
        case 6:
            selector = "#ContactWeek" + kind + "SatLi";
            break;
        case 7:
            selector = "#ContactWeek" + kind + "SunLi";
            break;
    }
    return selector;
}

function GetWeekSelectorHidden(kind, day) {

    var selector = "";
    switch (day) {
        case 1:
            selector = "#ContactWeek" + kind + "MonHidden";
            break;
        case 2:
            selector = "#ContactWeek" + kind + "TueHidden";
            break;
        case 3:
            selector = "#ContactWeek" + kind + "WedHidden";
            break;
        case 4:
            selector = "#ContactWeek" + kind + "TurHidden";
            break;
        case 5:
            selector = "#ContactWeek" + kind + "FriHidden";
            break;
        case 6:
            selector = "#ContactWeek" + kind + "SatHidden";
            break;
        case 7:
            selector = "#ContactWeek" + kind + "SunHidden";
            break;
    }
    return selector;
}

function selectContactTime(kind, row) {

    var li = $("#ContactTime" + kind + "Li_Row_" + row);
    var hidden = $("#ContactTime" + kind + "Hidden_Row_" + row);
    if (hidden.val() == "1") {
        li.removeClass("scNscPopUpContactSelectBtnMiddleOn");
        hidden.val("0");
    } else {
        li.addClass("scNscPopUpContactSelectBtnMiddleOn");
        hidden.val("1");
    }
}

function cancelContact() {
    $("#CustomerRelatedContactPopupArea").fadeOut(300);
    //画面初期化
    $("#CustomerRelatedContactPopupCancelButton").click();
}

function registContact() {

    var days = [1, 2, 3, 4, 5, 6, 7];

    for (i = 0; i < days.length; i++) {
        if ($(GetWeekSelectorHidden(1, days[i])).val() == "1" && $(GetWeekSelectorHidden(2, days[i])).val() == "1") {
            alert($("#ContactErrMsg").val());
            return false;
        }
    }
    return true;
}

function reloadMemo() {
    $("#CustomerMemoEdit").fadeOut(300);
    $("#CustomerMemoEditCloseButton").click();
}
