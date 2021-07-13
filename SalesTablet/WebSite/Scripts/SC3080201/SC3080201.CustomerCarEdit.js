//━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
//SC3080201.CustomerCarEdit.js
//─────────────────────────────────────
//機能： 車両編集PopUp
//補足： 車両編集PopUpを開くタイミングにて遅延ロードする
//作成： 2014/04/21 TCS 市川 GTMCタブレット高速化対応（BTS-386）
//更新： 2018/06/18 TCS 前田 TKM Next Gen e-CRB Project Application development Block B-1
//更新： 2018/11/22 TCS 三浦 活動区分「3,4」選択時、理由を選択できるようにする(TR-SLT-TMT-20180611-001)
//─────────────────────────────────────

 // 車両編集　完了クリック　------------------------------------------------
 $("#scVehicleEditingWindown #scVehicleEditingWindownBox .scVehicleEditingCompletionButton").live("click",
	 function (e) {

		 //if ($("#serverProcessFlgHidden").val() == "1") {  //サーバーサイド処理フラグ判定 (1:処理中)
		 //    return;
		 //}

		 //ステータス管理ポップアップを表示するためのフラグ設定
		 if ($("#vehiclePopUpAutoOpenFlg").val() == "1") {
			 this_form.UseAutoOpening.value = "1";
			 this_form.vehiclePopUpAutoOpenFlg.value = "0";
		 }

		 //2:未取引客時のみ
		 //2012/08/23 TCS 河原 【A STEP2】次世代e-CRB 新車商談機能改善 START
		 if ($("#editVehicleModeHidden").val() != "0" || $("#vclregnoTextBox").val() == "") {
			 if ($("#custFlgHidden").val() == "2") {
				 //モデル未入力エラー
				 if ($("#modelTextBox").val() == "") {
					 alert($("#vehicleNoModelErrMsg").val());
					 return;
				 }
			 }
		 }
		 //2012/08/23 TCS 河原 【A STEP2】次世代e-CRB 新車商談機能改善 END

		 var prms = "";
		 prms = prms + encodeURIComponent($("#makerTextBox").val()) + ",";                  //1:メーカー
		 if ($("#modelTextBox").val() != "") {
		     prms = prms + encodeURIComponent($("#modelTextBox").val()) + ",";              //2:モデル
		 }else{
		     prms = prms + encodeURIComponent($("#DefaultMaker").val()) + ",";              //2:モデル
		 }
		 //2012/08/23 TCS 河原 【A STEP2】次世代e-CRB 新車商談機能改善 END
		 
		 prms = prms + encodeURIComponent($("#vclregnoTextBox").val()) + ",";               //3:車両登録No
		 prms = prms + encodeURIComponent($("#vinTextBox").val()) + ",";                    //4:VIN
		 prms = prms + encodeURIComponent($("#vcldelidateDateTime").val()) + ",";           //5:納車日
		 prms = prms + encodeURIComponent($("#editVehicleModeHidden").val()) + ",";         //6:処理モード
		 prms = prms + encodeURIComponent($("#actvctgryidHidden").val()) + ",";             //7:活動区分
		 prms = prms + encodeURIComponent($("#reasonidHidden").val()) + ",";                //8:断念理由
		 //2012/03/08 TCS 河原 【SALES_1B】コールバック時の文字列のエンコード処理追加 END
		 //2018/06/18 TCS 前田 TKM Next Gen e-CRB Project Application development Block B-1 START
		 prms = prms + encodeURIComponent($("#vclMileTextBox").val()) + ",";                //9:走行距離
		 prms = prms + encodeURIComponent($("#modelYearHidden").val()) + ",";               //10:年式
		 //2018/06/18 TCS 前田 TKM Next Gen e-CRB Project Application development Block B-1 END

		 //処理中フラグを立てる
		 $("#serverProcessFlgHidden").val("1");         //サーバーサイド処理フラグ
		 SC3080201.startServerCallback();

		 callback.doCallback("VehicleUpdate", prms, function (result, context) {

			 $("#serverProcessFlgHidden").val("");      //サーバーサイド処理フラグ
			 SC3080201.endServerCallback();

			 var resArray = result.split(",");
			 if (resArray[1] == "0") {
				 //$(".scVehicleEditingCancellButton").click();

				 //編集モードにする
				 $("#editVehicleModeHidden").val("1");

				 //タイトル等変更処理
				 changeVehicleMode()

				 //// 車両編集-更新前情報の情報を保存する
				 backUpVehicleInfo()

				 //車両編集（新規登録、編集時両方共通）
				 CustomerCarEditPopUpClose()
				 //2012/03/08 TCS 山口 【SALES_2】性能改善 START
				 //車両情報再読込み
				 CustomerCarAreaReload()
				 ////車両編集画面を閉じる
				 //$("#scVehicleEditingWindown").fadeOut(100);
				 //2012/03/08 TCS 山口 【SALES_2】性能改善 END

			 } else {
				 alert(SC3080201HTMLDecode(resArray[2]));
				 //SC3080201.endServerCallback();
			 }
		 });
	 }
 );
 //    });

 // 車両編集　キャンセルクリック　-------------------------------------------------
 $("#scVehicleEditingWindown #scVehicleEditingWindownBox .scVehicleEditingCancellButton").live("click",
	 function (e) {
		 //2012/03/08 TCS 山口 【SALES_2】性能改善 START
		 //$("#vehicleCancelButtonLabel").click();

		 var page = $("#vehiclePageHidden").val();
		 if (page == "page1") {
			 //画面を閉じる
			 CustomerCarEditPopUpClose();

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
			//2018/06/18 TCS 前田 TKM Next Gen e-CRB Project Application development Block B-1 START
            } else if (page == "page4") {
                //年式でキャンセルボタンを押下した場合は、起動前の値に戻す
                if ($("#tempModelYearidHidden").val() != "") {
                    $("#modelYearHidden").val($("#tempModelYearidHidden").val());
                    $("#modelYearNameHidden").val($("#tempModelYearnmHidden").val());

                    $("#modelYearLabel2").text($("#modelYearNameHidden").val());

                    // 年式リスト初期チェックセット
                    modelyearlist("#scVehicleEditingWindown");
                }

                //1ページ目表示設定
                setPopupVehiclePage("page1");
            //2018/06/18 TCS 前田 TKM Next Gen e-CRB Project Application development Block B-1 END
            } else {
				 //2ページ目表示設定
                 setPopupVehiclePage("page2");
			 }
		 }

		 e.stopImmediatePropagation();
		 //2012/03/08 TCS 山口 【SALES_2】性能改善 END
	 });
 //    });


 // 保有車両を追加クリック　-------------------------------------------------
 //    $("#scVehicleEditingWindown #scVehicleEditingWindownBox .scVehicleAppendButton").click(function (e) {
 $("#scVehicleEditingWindown #scVehicleEditingWindownBox .scVehicleAppendButton").live("click",
	 function (e) {
		 var prms = "";

		 //2012/03/08 TCS 山口 【SALES_2】性能改善 START
		 //共通読込みアニメーション変更
		 $("#processingServer").addClass("carEditPopupLoadingAnimation");
		 $("#registOverlayBlack").addClass("popupLoadingBackgroundColor");
		 //2012/03/08 TCS 山口 【SALES_2】性能改善 END

		 $("#serverProcessFlgHidden").val("1");         //サーバーサイド処理フラグ
		 SC3080201.startServerCallback();

		 callback.doCallback("VehicleAppend", prms, function (result, context) {

			 //2012/03/08 TCS 山口 【SALES_2】性能改善 START
			 //共通読込みアニメーション戻し
			 $("#processingServer").removeClass("carEditPopupLoadingAnimation");
			 $("#registOverlayBlack").removeClass("popupLoadingBackgroundColor");
			 $("#registOverlayBlack").removeAttr("style");
			 //2012/03/08 TCS 山口 【SALES_2】性能改善 END

			 $("#serverProcessFlgHidden").val("");      //サーバーサイド処理フラグ
			 SC3080201.endServerCallback();

			 var resArray = result.split(",");
			 if (resArray[1] == "0") {
				 //各項目の内容をクリアする
				 //2012/03/08 TCS 山口 【SALES_2】性能改善 START
				 $("#makerTextBox").val("");
				 $("#modelTextBox").val("");
				 $("#vclregnoTextBox").val("");
				 $("#vinTextBox").val("");
				 //2012/03/08 TCS 山口 【SALES_2】性能改善 END
				 $("#vcldelidateDateTime").val("");
				 $("#editVehicleModeHidden").val("0");
				 //2018/06/18 TCS 前田 TKM Next Gen e-CRB Project Application development Block B-1 START
				 $("#modelYearHidden").val("");
				 $("#modelYearNameHidden").val("");
				 $("#modelYearLabel2").text("");
				 $("#vclMileTextBox").val("");
				 $("#scVehicleEditingWindown .dataWindModelYear .modelyearlist").removeClass("Selection");
				 //2018/06/18 TCS 前田 TKM Next Gen e-CRB Project Application development Block B-1 END

				 //タイトル等の変更処理
				 changeVehicleMode();

			 } else {
				 alert(SC3080201HTMLDecode(resArray[2]));
			 }

		 });
	 }
 );


// 車両情報クリック時に車両編集ポップアップ表示(サーバー処理後)
function CustomerCarEditPopUpOpenAfter() {

    //カスタムコントロール設定
    $(".scVehicleEditingListArea input:text").CustomTextBox({ "useEllipsis": "true" });
    $("#vcldelidateDateTime").DateTimeSelector();

    // 車両編集ポップアップ設定
    setPopupVehicleIinital();

    //スクロール設定
    //1:自社客時のみ
    if ($("#custFlgHidden").val() == "1") {
        $("#scVehicleEditingWindown #scVehicleEditingWindownBox .scVehicleEditingListBox2").fingerScroll();
        //$("#scNscCustomerEditingWindown #scNscCustomerEditingWindownBox .dataWindNameTitle .ListBox01").fingerScroll();
        //2012/03/08 TCS 山口 【SALES_2】性能改善 START
        $("#scVehicleEditingWindown #scVehicleEditingWindownBox .dataWindActvctgry .ListBox01 .dataWind2").fingerScroll();
        $("#scVehicleEditingWindown #scVehicleEditingWindownBox .dataWindReason .ListBox01 .dataWind2").fingerScroll();
        //2012/03/08 TCS 山口 【SALES_2】性能改善 END
    }
    //2018/06/18 TCS 前田 TKM Next Gen e-CRB Project Application development Block B-1 START
    $("#scVehicleEditingWindown #scVehicleEditingWindownBox .dataWindModelYear .ListBox01 .dataWind2").fingerScroll();
    //2018/06/18 TCS 前田 TKM Next Gen e-CRB Project Application development Block B-1 END

    //未顧客
    if ($("#custFlgHidden").val() == '2') {
        //テーブルに対して、一番したの行の罫線を消す処理
        $(".scVehicleEditingListItemBottomBorder2").css("border-bottom-width", "0px");
    }

    //ポップアップ設定
    cancelVehicleInfo();

    //車両編集表示
    changeVehicleMode();

    // 2012/01/26 TCS 安田 【SALES_1B】チェックマークのチェックの色（青／赤）を切り替える START
    //1:自社客時のみ
    if ($("#custFlgHidden").val() == "1") {
        //G-Bookチェックの表示色セット
        setCheckColor($("#gbookCheckButton"));
    }
    // 2012/01/26 TCS 安田 【SALES_1B】チェックマークのチェックの色（青／赤）を切り替える END
    setPopupVehiclePage("page1");

    $("#scVehicleEditingWindown").fadeIn(0);

    //共通読込みアニメーション戻し
    $("#processingServer").removeClass("carEditPopupLoadingAnimation");
    $("#registOverlayBlack").removeClass("popupLoadingBackgroundColor");
}

//車両編集ポップアップ非表示処理
function CustomerCarEditPopUpClose() {
    //ポップアップ非表示
    $("#scVehicleEditingWindown").fadeOut(300);
    setTimeout(function () {
        //強制的に1ページ目に
        setPopupVehiclePage("page1");
        //HTML削除
        $("#CustomerCarEditVisiblePanel").empty();
    }, 300);

    cancelVehicleInfo();

    //念のためフラグをクリアする
    $("#serverProcessFlgHidden").val("");
}

//2012/03/08 TCS 山口 【SALES_2】性能改善 END

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

    //2018/06/18 TCS 前田 TKM Next Gen e-CRB Project Application development Block B-1 START
    //年式
    page = $("#scModelYearPopWindown .popWind .dataWind1 .ListBox01");
    page = page.clone(true);

    $("#scVehicleEditingWindown .dataWindModelYear").append(page);
    //2018/06/18 TCS 前田 TKM Next Gen e-CRB Project Application development Block B-1 END

    // 活動区分リスト初期チェックセット
    actvctgrylist("#scVehicleEditingWindown");

    // 断念理由リスト初期チェックセット
    reasonidlist("#scVehicleEditingWindown");

    //2018/06/18 TCS 前田 TKM Next Gen e-CRB Project Application development Block B-1 START
    // 年式リスト初期チェックセット
    modelyearlist("#scVehicleEditingWindown");
    //2018/06/18 TCS 前田 TKM Next Gen e-CRB Project Application development Block B-1 END

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

            //2018/11/22 TCS 三浦 活動区分「3,4」選択時、理由を選択できるようにする(TR-SLT-TMT-20180611-001) START
            if (cd == "2" || cd == "3" || cd == "4") {    //2,3,4の場合、情報不備詳細へ遷移する
                changeCarReasonListItems();
                $("#reasonTitleLabel").text($("#actvctgryNameHidden").val());
                //2018/11/22 TCS 三浦 活動区分「3,4」選択時、理由を選択できるようにする(TR-SLT-TMT-20180611-001) END

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

        //2018/06/18 TCS 前田 TKM Next Gen e-CRB Project Application development Block B-1 START
        //活動区分ポップアップ／年式ポップアップの表示切替
        $("#scVehicleEditingWindown .dataWindActvctgry").css("display", "block");
        $("#scVehicleEditingWindown .dataWindModelYear").css("display", "none");
        //2018/06/18 TCS 前田 TKM Next Gen e-CRB Project Application development Block B-1 END

        setPopupVehiclePage("page2", "actvctgryList");

    });

    //2018/06/18 TCS 前田 TKM Next Gen e-CRB Project Application development Block B-1 START
    //年式リスト押下
    $("#scVehicleEditingWindown").find(".scVehicleEditingModelYear").click(function (e) {

        //キャンセル時にもとの値に戻すため初期値を保存する
        $("#tempModelYearidHidden").val($("#modelYearHidden").val());
        $("#tempModelYearnmHidden").val($("#modelYearNameHidden").val());

        //年式選択時処理
        $("#scVehicleEditingWindown .dataWindModelYear .modelyearlist").click(function (e) {

            //選択肢の値と名称を取得
            var cd = $(this).children(".modelYearCdHidden").text();
            var nm = $(this).children(".modelYearLabel").text();
            //取得値をHiddenFieldに退避
            $("#modelYearHidden").val(cd);
            $("#modelYearNameHidden").val(nm);
            //選択肢の選択状態を切り替え
            $("#scVehicleEditingWindown .dataWindModelYear .modelyearlist").removeClass("Selection");
            $(this).addClass("Selection");

            //車両編集PopUp（起動元）の名称を更新
            $("#modelYearLabel2").text(nm);

            setPopupVehiclePage("page1");
        });

        //活動区分ポップアップ／年式ポップアップの表示切替
        $("#scVehicleEditingWindown .dataWindActvctgry").css("display", "none");
        $("#scVehicleEditingWindown .dataWindModelYear").css("display", "block");

        setPopupVehiclePage("page4", "modelYearList");

    });
    //2018/06/18 TCS 前田 TKM Next Gen e-CRB Project Application development Block B-1 END
}

//2018/11/22 TCS 三浦 活動区分「3,4」選択時、理由を選択できるようにする(TR-SLT-TMT-20180611-001) START
function changeCarReasonListItems() {
    var $list = $('.reasonListBoxSetIn .reasonlist', $('#CustomerCarEditVisiblePanel')[0])
    $list.each(function () {

        var $actvctgryid = $('.actvctgryidHidden', this);
        $(this).toggle($actvctgryid.text() === $("#actvctgryidHidden").val());
    });
    $list.removeClass('endRow').filter(function () { return $(this).css('display') !== 'none' }).last().addClass('endRow');
};
//2018/11/22 TCS 三浦 活動区分「3,4」選択時、理由を選択できるようにする(TR-SLT-TMT-20180611-001) END

// 車両編集表示 (ページ指定)　-------------------------------------------------
function setPopupVehiclePage(page, subId) {

    //ページ番号セット
    $("#vehiclePageHidden").val(page);

    $("#scVehicleEditingWindown #scVehicleEditingWindownBox .scVehicleEditingListBox").removeClass("page1 page2 page3 page4").addClass(page);

    //タイトルを変更する
    var strCancelLable = "";
    var strTitleLable = "";
    var strCompletionLable = "";
    if (page == "page1") {

        $(".scVehicleEditingCompletionButton").show();                    //右ボタンを表示

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

        $(".scVehicleEditingCompletionButton").hide();                    //右ボタンは非表示

        if (page == "page2") {
            if (($("#editVehicleModeHidden").val() == "0")) {
                //追加時
                strCancelLable = $("#createVehicleLabel").text();
            } else {
                //更新時
                strCancelLable = $("#editVehicleLabel").text();
            }

            strTitleLable = $("#actvctgryTitleLabel").text();
        //2018/06/18 TCS 前田 TKM Next Gen e-CRB Project Application development Block B-1 START
        } else if (page == "page4") {
            if (($("#editVehicleModeHidden").val() == "0")) {
                //追加時
                strCancelLable = $("#createVehicleLabel").text();
            } else {
                //更新時
                strCancelLable = $("#editVehicleLabel").text();
            }

            strTitleLable = $("#modelYearTitleLabel").text();

        //2018/06/18 TCS 前田 TKM Next Gen e-CRB Project Application development Block B-1 END
        } else {

            strCancelLable = $("#reasonBackLabel").text();

            strTitleLable = $("#reasonTitleLabel").text();
        }
    }

    $("#vehicleCancelButtonLabel").text(strCancelLable);           //キャンセルボタン
    $("#vehicleCompletionButtonLabel").text(strCompletionLable);   //登録ボタン
    $("#vehicleTitleLabel").text(strTitleLable);                   //タイトル

    //$("#vehicleCancelButtonLabel").CustomLabel({ 'useEllipsis': 'true' });
    //$("#vehicleCompletionButtonLabel").CustomLabel({ 'useEllipsis': 'true' });
}

//車両編集　START -------------------------------------------------------------------------------------

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


