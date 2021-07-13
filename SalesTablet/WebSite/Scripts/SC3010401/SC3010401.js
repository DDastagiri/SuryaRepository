/* 
* ToDo一覧
* 作成： 2012/02/01 TCS 竹内
* 更新： 2012/03/13 TCS 渡邊 $01 SalesStep2ユーザーテスト課題No.15、18、36
* 更新： 2012/04/23 TCS 神本 $02 号口課題No.123
* 更新： 2012/05/29 TCS 神本 クルクル対応 
* 更新： 2013/01/11 TCS 橋本 【A.STEP2】次世代e-CRB 新車タブレットショールーム管理機能開発
* 更新： 2014/02/17 TCS 山田 受注後フォロー機能開発
* 更新： 2014/11/04 TCS 藤井 iOS8 対応(i-CROP_V4_salesよりマージ) 
* 更新： 2020/02/17 TS 髙橋(龍) TR-SLT-TMT-20200218-001
*/
var sc3010401Script = function () {
    var constants = {
        open: 1,
        close: 0,
        sortType: { carName: 1, status: 2, cract: 3 },
        sortOrder: { asc: 1, desc: 2 }
    }

    var beforeChangeCriteria = {};
    var status = constants.close;

    //検索条件エリアを開く
    function openCriteria() {
        $(".WindDown").css("display", "none");
        //変更前の抽出条件を保存
        saveCriteria();
        status = constants.open;
        //2020/02/17 TS 髙橋(龍) TR-SLT-TMT-20200218-001対応 START
        $("#CheckBoxArea").css("display", "block");
        //2020/02/17 TS 髙橋(龍) TR-SLT-TMT-20200218-001対応 END
        $("#SetIcons").addClass("SetIconsHeightL");
        //2013/01/11 TCS 橋本 【A.STEP2】Add Start
        $(".WindUp").css("display", "block");
        //2013/01/11 TCS 橋本 【A.STEP2】Add End
        //2014/02/17 TCS 山田 受注後フォロー機能開発 START
        //検索条件エリアの行数を可変にする
        $("#SetIcons").css("height", 120 + (50 * (Math.ceil(3 + $(".AfterCriteria").length) / 8)) + "px");
        $(".WindUp").css("top", 100 + (50 * (Math.ceil(3 + $(".AfterCriteria").length) / 8)) + "px");
        //2014/02/17 TCS 山田 受注後フォロー機能開発 END
        return false;
    }

    //検索条件エリアを閉じる
    function closeCriteria(isOutOfArea) {

        //2013/01/11 TCS 橋本 【A.STEP2】Add Start
        $(".WindUp").css("display", "none");
        //2020/02/17 TS 髙橋(龍) TR-SLT-TMT-20200218-001対応 START
        $("#CheckBoxArea").css("display", "none");
        //2020/02/17 TS 髙橋(龍) TR-SLT-TMT-20200218-001対応 END
        //2013/01/11 TCS 橋本 【A.STEP2】Add End
        $("#SetIcons").removeClass("SetIconsHeightL");
        //2014/02/17 TCS 山田 受注後フォロー機能開発 START
        //検索条件エリアの行数を元に戻す
        $("#SetIcons").css("height", "51px");
        //2014/02/17 TCS 山田 受注後フォロー機能開発 END
        if (isOutOfArea && status == constants.open) {
            resetCriteria();
        }
        status = constants.close;
        $(".WindDown").css("display", "block");
        return false;
    }

    //検索条件保存
    function saveCriteria() {
        //検索条件を保存
        $.extend(beforeChangeCriteria, getCriteria());
    }

    //検索条件を取得する
    function getCriteria() {
        //2014/02/17 TCS 山田 受注後フォロー機能開発 START
        var AfterOdrProc = [];
        $(".icrop-CustomCheckBox>:checkbox.AfterCriteria").each(function () {
            AfterOdrProc.push($(this).CustomCheckBox("value"));
        });
        //2014/02/17 TCS 山田 受注後フォロー機能開発 END
        return {
            //2014/02/17 TCS 山田 受注後フォロー機能開発 START
            toDoSearchType: $("#ToDoSegmentedButton input:checked").val(),
            toDoSearchText: $("#ToDoSearchTextBox").val(),
            //2014/02/17 TCS 山田 受注後フォロー機能開発 END
            isCheckDelay: $("#checkDelay").CustomCheckBox("value"),
            isCheckDue: $("#checkDue").CustomCheckBox("value"),
            isCheckFuture: $("#checkFuture").CustomCheckBox("value"),
            isCheckCold: $("#checkCold").CustomCheckBox("value"),
            isCheckWarm: $("#checkWarm").CustomCheckBox("value"),
            isCheckHot: $("#checkHot").CustomCheckBox("value"),
            //2014/02/17 TCS 山田 受注後フォロー機能開発 START
            isCheckAfter: AfterOdrProc,
            //2014/02/17 TCS 山田 受注後フォロー機能開発 END
            sortType: $("#sortTypeHidden").val(),
            sortOrder: $("#sortOrderHidden").val()

        }
    }

    //検索条件を元に戻す
    function resetCriteria() {
        //検索条件をすべてONにする

        $(".icrop-CustomCheckBox>:checkbox").each(function () {
            $(this).CustomCheckBox("value", true);
        });
        //前回検索条件を反映する
        $("#checkDelay").CustomCheckBox("value", beforeChangeCriteria.isCheckDelay);
        $("#checkDue").CustomCheckBox("value", beforeChangeCriteria.isCheckDue);
        $("#checkFuture").CustomCheckBox("value", beforeChangeCriteria.isCheckFuture);
        $("#checkCold").CustomCheckBox("value", beforeChangeCriteria.isCheckCold);
        $("#checkWarm").CustomCheckBox("value", beforeChangeCriteria.isCheckWarm);
        $("#checkHot").CustomCheckBox("value", beforeChangeCriteria.isCheckHot);
        //2014/02/17 TCS 山田 受注後フォロー機能開発 START
        for (i = 0; i < $(".AfterCriteria").length; i++) {
            var j = 0;
            $(".icrop-CustomCheckBox>:checkbox.AfterCriteria").each(function () {
                if (i == j) {
                    $(this).CustomCheckBox("value", beforeChangeCriteria.isCheckAfter[i]);
                }
                j++;
            });
        }
        //2014/02/17 TCS 山田 受注後フォロー機能開発 END

    }

    //TODO一覧で明細選択
    function selectCustomer(cstkindHidden, customerclassHidden, crcustidHidden, fllwupboxseqHidden, strcdHidden, columeId) {
        //2012/05/29 TCS 神本 クルクル対応 START
        //        displayOverlay();
        //2012/05/29 TCS 神本 クルクル対応 End
        var $divlist = $("#" + columeId + "").children("td");
        for (i = 0; i < $divlist.length; i++) {
            $divlist[i].className = $divlist[i].className + " ColorBlue";
        }

        $("#cstkindHidden").val(cstkindHidden);
        $("#customerclassHidden").val(customerclassHidden);
        $("#crcustidHidden").val(crcustidHidden);
        $("#fllwupboxseqHidden").val(fllwupboxseqHidden);
        $("#strcdHidden").val(strcdHidden);



        //2012/05/29 TCS 神本 クルクル対応 START
        commonRefreshTimer(function () {
            $("#refreshButton").click();
            //return true;
        });
        displayOverlay();
        $("#nextButton").click();
        //2012/05/29 TCS 神本 クルクル対応 End
    }

    //オーバーレイ表示
    function displayOverlay() {

        //オーバーレイ表示
        $("#serverProcessOverlayBlack").css("display", "block");
        //アニメーション(ロード中)
        setTimeout(function () {
            $("#serverProcessIcon").addClass("show");
            $("#serverProcessOverlayBlack").addClass("open");
        }, 0);


    }

    //オーバーレイ終了
    function closeOverlay() {
        $("#serverProcessIcon").removeClass("show");
        //2014/11/04 TCS 藤井 iOS8 対応(i-CROP_V4_salesよりマージ)  Start
        $("#serverProcessOverlayBlack").removeClass("open");
        setTimeout(function () {
            $("#serverProcessOverlayBlack").css("display", "none");
        }, 300);
        //2014/11/04 TCS 藤井 iOS8 対応(i-CROP_V4_salesよりマージ)  End
    }

    function escapeHTML(text) {
        return $("<span>").text(text).html();
    }

    return {
        constants: constants,
        openCriteria: openCriteria,
        closeCriteria: closeCriteria,
        getCriteria: getCriteria,
        displayOverlay: displayOverlay,
        closeOverlay: closeOverlay,
        saveCriteria: saveCriteria,
        selectCustomer: selectCustomer,
        escapeHTML: escapeHTML
    }
} ();

    //2014/02/17 TCS 山田 受注後フォロー機能開発 START
    //顧客検索用ラジオボタン変更時
    var g_IniLoad = false;
    function ToDoSearchTypeSegmenteButton_select(value) {
        if (value == '001') {
    		$("#ToDoSearchTextBox")[0].placeholder = $("#ToDoSearchTypeWordNameHidden").val();
      	}
      	else if (value == '002') {
      		$("#ToDoSearchTextBox")[0].placeholder = $("#ToDoSearchTypeWordTelHidden").val();
      	}
      	else if (value == '003') {
      		$("#ToDoSearchTextBox")[0].placeholder = $("#ToDoSearchTypeWordSocialIDHidden").val();
      	}
      	else if (value == '004') {
      		$("#ToDoSearchTextBox")[0].placeholder = $("#ToDoSearchTypeWordBookingNoHidden").val();
      	}
      	else if (value == '005') {
      		$("#ToDoSearchTextBox")[0].placeholder = $("#ToDoSearchTypeWordVinHidden").val();
      	}

      	if (g_IniLoad) {
      		$("#FocusinDummyButton").click();
      	} else {
      		g_IniLoad = true;
      	}
    }

    //検索条件入力テキストボックス選択時
    function FocusInToDoSearchTextBox(text) {
        if (text == "dummy") {
            $("#ToDoSearchTextBox").focus();
        }
      	custSearchfouusFlg = true;
   		$("#ToDoSearchTextBox").CustomTextBox("showClearButton");
    }

    //検索条件入力テキストボックス入力時
    function InputInToDoSearchTextBox() {
        //Enter押下時のみ、検索処理実行
        if (event.keyCode == 13) {
            $("#ToDoSearchTextBox").blur();
            $("#AddIconRight").click();
        }
    }
    //2014/02/17 TCS 山田 受注後フォロー機能開発 END

$(function () {
    //上段チェックボックス作成
    $(".icrop-CustomCheckBox>:checkbox" + ".dateCriteria").CustomCheckBox(
	{ "check": function (value) {
	    if ($(".icrop-CustomCheckBox>:checkbox" + "[checked=true]" + ".dateCriteria").length == 0 && value == false) {
	        $(this).CustomCheckBox("value", true);
	    }
	}
	});

    //2014/02/17 TCS 山田 受注後フォロー機能開発 START DEL
   	//2014/02/17 TCS 山田 受注後フォロー機能開発 END

    //$02 Add Start
    $(".SetIconsList>li").click(function (e) {
        //イベント発生元がチェックボックスの場合は、処理しない
        if ($(e.target).parents(".icrop-CustomCheckBox").length == 1) {
            return;
        }
        var checkbox = $(this).find(".icrop-CustomCheckBox>:checkbox");
        checkbox.CustomCheckBox("value", !checkbox.CustomCheckBox("value"));
    });
    //$02 Add End

    //検索エリアのアニメーション処理
    $("#SetIcons").swipe({
        swipeUp: sc3010401Script.closeCriteria,
        swipeDown: sc3010401Script.openCriteria,
        threshold: 20,
        triggerOnTouchEnd: false
    });

    //2014/02/17 TCS 山田 受注後フォロー機能開発 START
    var CheckAllFlg = 0;
    var CheckEachFlg = 0;
    //受注前一括チェックボックスのチェック時
    $("#CheckAllBefore").CustomCheckBox({
    	check: function (value) {
    		if (value === true) {
    			CheckAllFlg = 1;
    			$(".BeforeCriteria").CustomCheckBox("value", true);
    			CheckAllFlg = 0;
    		} else {
    			if (CheckEachFlg == 0) {
    				CheckAllFlg = 1;
    				$(".BeforeCriteria").CustomCheckBox("value", false);
    				CheckAllFlg = 0;
    			}
    		}
    	}
    });

    //受注後一括チェックボックスのチェック時
    $("#CheckAllAfter").CustomCheckBox({
    	check: function (value) {
            if (value === true) {
    			CheckAllFlg = 1;
    			$(".AfterCriteria").CustomCheckBox("value", true);
    			CheckAllFlg = 0;
    		} else {
    			if (CheckEachFlg == 0) {
    				CheckAllFlg = 1;
    				$(".AfterCriteria").CustomCheckBox("value", false);
    				CheckAllFlg = 0;
    			}
    		}
    	}
    });

    //各受注前チェックボックスのチェック時
    $(".icrop-CustomCheckBox>:checkbox" + ".BeforeCriteria").CustomCheckBox(
        { "check": function (value) {
        	$(this)[0].checked = value;
        	if ($(".icrop-CustomCheckBox>:checkbox" + "[checked=true]" + ".BeforeCriteria").length == $(".BeforeCriteria").length
                 && $("#CheckAllBefore").CustomCheckBox("value") == false) {
        		$("#CheckAllBefore").CustomCheckBox("value", true);
        	}
        	if ($(".icrop-CustomCheckBox>:checkbox" + "[checked=true]" + ".BeforeCriteria").length < $(".BeforeCriteria").length
                 && $("#CheckAllBefore").CustomCheckBox("value") == true && CheckAllFlg == 0) {
        		CheckEachFlg = 1;
        		$("#CheckAllBefore").CustomCheckBox("value", false);
        		CheckEachFlg = 0;
        	}
        }
    });

    //各受注後チェックボックスのチェック時
    $(".icrop-CustomCheckBox>:checkbox" + ".AfterCriteria").CustomCheckBox(
        { "check": function (value) {
        	$(this)[0].checked = value;
        	if ($(".icrop-CustomCheckBox>:checkbox" + "[checked=true]" + ".AfterCriteria").length == $(".AfterCriteria").length
                 && $("#CheckAllAfter").CustomCheckBox("value") == false) {
        		$("#CheckAllAfter").CustomCheckBox("value", true);
        	}
        	if ($(".icrop-CustomCheckBox>:checkbox" + "[checked=true]" + ".AfterCriteria").length < $(".AfterCriteria").length
                 && $("#CheckAllAfter").CustomCheckBox("value") == true && CheckAllFlg == 0) {
        		CheckEachFlg = 1;
        		$("#CheckAllAfter").CustomCheckBox("value", false);
        		CheckEachFlg = 0;
        	}
        }
    });
    //2014/02/17 TCS 山田 受注後フォロー機能開発 END

    /* 神本修正Start */
    //下三角ボタン(検索条件開く処理)
    $(".WindDown").click(function () {
        sc3010401Script.openCriteria();
    });

    //上三角ボタン(検索条件閉じる処理)
    $(".WindUp").click(function () {
        sc3010401Script.closeCriteria();
    });
    /* 神本修正End */

    //検索エリア外のタッチ処理(検索エリアが開いていたら閉じる)
    $("#bodyFrame").click(function () {
        try {
            if ($(event.target).parents("#SetIcons").length == 0) {
                sc3010401Script.closeCriteria(true);
            }
        } catch (e) {

   	    }

   	});

    /* 神本修正Start */
    //2013/01/11 TCS 橋本 【A.STEP2】Del Start
    /*
    //下三角ボタン(検索条件開く処理)
    $(".WindDown").click(function () {
        sc3010401Script.openCriteria();
    });
    */
    //2013/01/11 TCS 橋本 【A.STEP2】Del End

    //検索ボタン押下処理
    $("#AddIconRight").click(function () {
        //$01 Add Start
        //ソート条件初期化
        $("#sortTypeHidden").val(sc3010401Script.constants.sortType.cract);
        $("#sortOrderHidden").val(sc3010401Script.constants.sortOrder.asc);
        //$01 Add End
        //検索条件保存
        sc3010401Script.saveCriteria();
        $("#customerRepeater").CustomRepeater("reload", sc3010401Script.getCriteria());
    });

    ///車両リンク押下
    $("#CustomLabelCar").click(function () {
        if ($("#sortTypeHidden").val() == sc3010401Script.constants.sortType.carName) {
            if ($("#sortOrderHidden").val() == sc3010401Script.constants.sortOrder.asc) {
                $("#sortOrderHidden").val(sc3010401Script.constants.sortOrder.desc);
            } else {
                $("#sortOrderHidden").val(sc3010401Script.constants.sortOrder.asc);
            }
        } else {
            $("#sortTypeHidden").val(sc3010401Script.constants.sortType.carName);
            $("#sortOrderHidden").val(sc3010401Script.constants.sortOrder.asc);
        }
        //ソート処理（再検索）
        $("#customerRepeater").CustomRepeater("reload", sc3010401Script.getCriteria());
    });

    //ステイタスリンク押下
    $("#CustomLabelStatus").click(function () {
        if ($("#sortTypeHidden").val() == sc3010401Script.constants.sortType.status) {
            if ($("#sortOrderHidden").val() == sc3010401Script.constants.sortOrder.asc) {
                $("#sortOrderHidden").val(sc3010401Script.constants.sortOrder.desc);
            } else {
                $("#sortOrderHidden").val(sc3010401Script.constants.sortOrder.asc);
            }
        } else {
            $("#sortTypeHidden").val(sc3010401Script.constants.sortType.status);
            $("#sortOrderHidden").val(sc3010401Script.constants.sortOrder.asc);
        }

        //ソート処理（再検索）
        $("#customerRepeater").CustomRepeater("reload", sc3010401Script.getCriteria());
    });

    //次回活動日リンク押下
    $("#CustomLabelCRACT").click(function () {
        if ($("#sortTypeHidden").val() == sc3010401Script.constants.sortType.cract) {
            if ($("#sortOrderHidden").val() == sc3010401Script.constants.sortOrder.asc) {
                $("#sortOrderHidden").val(sc3010401Script.constants.sortOrder.desc);
            } else {
                $("#sortOrderHidden").val(sc3010401Script.constants.sortOrder.asc);
            }
        } else {
            $("#sortTypeHidden").val(sc3010401Script.constants.sortType.cract);
            $("#sortOrderHidden").val(sc3010401Script.constants.sortOrder.asc);
        }

        //ソート処理（再検索）
        $("#customerRepeater").CustomRepeater("reload", sc3010401Script.getCriteria());
    });



});
