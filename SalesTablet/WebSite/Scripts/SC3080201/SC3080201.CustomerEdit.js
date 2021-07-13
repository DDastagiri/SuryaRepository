//━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
//SC3080201.CustomerEdit.js
//─────────────────────────────────────
//機能： 顧客編集PopUp
//補足： 顧客編集PopUpを開くタイミングにて遅延ロードする
//作成： 2014/04/21 TCS 市川 GTMCタブレット高速化対応（BTS-386）
//更新： 2014/05/01 TCS 市川 車両PopUp不具合対応（BTS-404）
//更新： 2014/05/07 TCS 市川 PCとタブレットで顧客入力情報不一致対応(CHG-354)
//更新： 2014/07/15 TCS 市川 複数の個人法人項目コードにて単一の敬称コードを表示させる(TMT-UAT-BTS-80)
//更新： 2014/10/02 TCS 河原 商業情報必須指定解除のタブレット側対応
//更新： 2017/11/16 TCS 河原 TKM独自機能開発
//更新： 2018/04/24 TCS 前田 （トライ店システム評価）基幹連携を用いたセールス業務実績入力の検証
//更新： 2018/08/27 TCS 佐々木 TKM Next Gen e-CRB Project Application development Block B-1
//更新： 2018/11/22 TCS 三浦 活動区分「3,4」選択時、理由を選択できるようにする(TR-SLT-TMT-20180611-001)
//更新： 2018/12/18 TCS 舩橋   TKM-UAT課題No.89 Subcategory2が1件に絞られた場合のみ、自動反映する
//更新： 2018/12/20 TCS 前田   TKM-UAT課題No.89 SuggestiveFieldからの検索時にスクロール位置を初期化する
//更新： 2019/02/13 TCS 河原 顧客編集ポップアップを欄外タップで閉じないように変更
//更新： 2019/04/08 TS  舩橋   POST-UAT3110 電話番号検索すると、サブカテゴリ２が消える
//更新： 2019/06/06 TS  村井 (FS)サービススタッフ納期遵守オペレーション確立に向けた試験研究
//更新： 2020/02/04 TS  舩橋 TKM Change request development for Next Gen e-CRB (CR008,CR060,CR072)
//─────────────────────────────────────

// 2018/08/27 TCS 佐々木 TKM Next Gen e-CRB Project Application development Block B-1 START
/**
 * 文字列をJSONとして評価し、配列オブジェクトを返す
 * @param {string} text
 * @returns {[*]}
 */
var parseOrEmptyArray = function (text) {
    try {
        var obj = JSON.parse(text)
        return obj instanceof Array ? obj : [obj];
    }
    catch (e) { return [] }
};

// ES2016 Array.prototype.includes の Polyfill
if (typeof Array.prototype.includes !== 'function') {
    Object.defineProperty(Array.prototype, 'includes', {
        configurable: true,
        writable: true,
        value: function (item, index) {
            return this.indexOf(item, index) !== -1;
        },
    });
}

// filter(':visible') では期待した結果が得られないことがある為
$.fn.filterVisibleItems = function () {
    return this.filter(function () { return $(this).css('display') !== 'none' });
}
// 2018/08/27 TCS 佐々木 TKM Next Gen e-CRB Project Application development Block B-1 END

// 住所検索　クリック　-------------------------------------------------
$("#zipSerchButton").live("click",
	 function () {

	     var prms = "";
	     if ($("#zipcodeTextBox").val() != "") {
	         $("#serverProcessFlgHidden").val("1");              //サーバーサイド処理フラグ
	         SC3080201.startServerCallback();

	         prms = prms + $("#zipcodeTextBox").val();           //郵便番号
	         callback.doCallback("GetAddress", prms, function (result, context) {

	             //--2013/11/29 TCS 各務 Aカード情報相互連携開発 DELETE

	             var resArray = result.split(",");
	             if (resArray[1] == "1") {

	                 //--2013/11/29 TCS 各務 Aカード情報相互連携開発 START
	                 //地域リスト取得
	                 var prms2 = "";
	                 prms2 = prms2 + encodeURIComponent(resArray[3]);           //州コード
	                 callback.doCallback("GetDistrict", prms2, function (result, context) {

	                     //地域リスト作成
	                     makeDistrictList(result);

	                     //市リスト取得
	                     var prms3 = "";
	                     prms3 = prms3 + encodeURIComponent(resArray[3]) + "," + encodeURIComponent(resArray[4]);           //州コード＋地域コード
	                     callback.doCallback("GetCity", prms3, function (result, context) {

	                         //市リスト作成
	                         makeCityList(result);

	                         //地区リスト取得
	                         var prms4 = "";
	                         prms4 = prms4 + encodeURIComponent(resArray[3]) + "," + encodeURIComponent(resArray[4]) + "," + encodeURIComponent(resArray[5]); //州コード＋地域コード＋市コード
	                         callback.doCallback("GetLocation", prms4, function (result, context) {

	                             //地区リスト作成
	                             makeLocationList(result);

	                             //取得結果を画面に反映
	                             //--2019/06/06 TS  村井 (FS)サービススタッフ納期遵守オペレーション確立に向けた試験研究 START
	                             //--住所１自動入力機能ONの時のみ値をセット
	                             if ($("#address1AutoInputHidden").val() == "1") {
	                                 $("#addressTextBox").CustomTextBox("updateText", SC3080201HTMLDecode(resArray[2]));
	                             }
	                             //--2019/06/06 TS  村井 (FS)サービススタッフ納期遵守オペレーション確立に向けた試験研究 END
	                             $("#stateHidden").val(resArray[3]);
	                             $("#districtHidden").val(resArray[4]);
	                             $("#cityHidden").val(resArray[5]);
	                             $("#locationHidden").val(resArray[6]);
	                             addresslist("#scNscCustomerEditingWindown");
	                             //--2019/06/06 TS  村井 (FS)サービススタッフ納期遵守オペレーション確立に向けた試験研究 START
	                             changeAddress();
	                             //--2019/06/06 TS  村井 (FS)サービススタッフ納期遵守オペレーション確立に向けた試験研究 END

	                             $("#serverProcessFlgHidden").val("");           //サーバーサイド処理フラグ
	                             SC3080201.endServerCallback();

	                         });

	                     });

	                 });

	                 //--2013/11/29 TCS 各務 Aカード情報相互連携開発 END

	             } else {
	                 //--2013/11/29 TCS 各務 Aカード情報相互連携開発 START
	                 $("#serverProcessFlgHidden").val("");           //サーバーサイド処理フラグ
	                 SC3080201.endServerCallback();
	                 //--2013/11/29 TCS 各務 Aカード情報相互連携開発 END
	                 alert(SC3080201HTMLDecode(resArray[2]));
	             }
	         });
	     }
	 }
 );


 //2012/04/12 TCS 安田 【SALES_2】来店から車両登録が不可（ユーザー課題 No.49）START
 // 顧客編集　完了クリック　-------------------------------------------------
 $("#scNscCustomerEditingWindown #scNscCustomerEditingWindownBox .scNscCustomerEditingCompletionButton").live("click",
	 function (e) {
		 CompletionCustomerInfo(e);
	 }
 );
 //2012/04/12 TCS 安田 【SALES_2】来店から車両登録が不可（ユーザー課題 No.49）END

 // 顧客編集　完了クリック(車両登録へボタン)　-------------------------------------------------
 $("#scNscCustomerEditingWindown #scNscCustomerEditingWindownBox .scNscCustomerEditingCompletionButton2").live("click",
	 function (e) {
		 //2012/04/12 TCS 安田 【SALES_2】来店から車両登録が不可（ユーザー課題 No.49）START
		 CompletionCustomerInfo(e);
		 //2012/04/12 TCS 安田 【SALES_2】来店から車両登録が不可（ユーザー課題 No.49）END
	 }
 );

 // 顧客編集　キャンセルクリック　-------------------------------------------------
 $("#scNscCustomerEditingWindown #scNscCustomerEditingWindownBox .scNscCustomerEditingCancellButton").live("click",
	 function (e) {
		 //2012/03/08 TCS 山口 【SALES_2】性能改善 START
		 var page = $("#custPageHidden").val();
		 if (page == "page1") {
			 //画面を閉じる
			 CustomerEditPopUpWindowClose();

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
				 //2012/03/08 TCS 山口 【SALES_2】性能改善 START
				 //FingerScroll初期化
				 $(".dataWindNameTitle .ListBox01 .scroll-inner").css({ "transform": "translate3d(0px, 0px, 0px)" });
				 //--2013/11/29 TCS 各務 Aカード情報相互連携開発 START
				 $(".dataWindPrivateFleetItem .ListBox01 .scroll-inner").css({ "transform": "translate3d(0px, 0px, 0px)" });
				 $(".dataWindState .ListBox01 .scroll-inner").css({ "transform": "translate3d(0px, 0px, 0px)" });
				 //--2013/11/29 TCS 各務 Aカード情報相互連携開発 END
				 //2012/03/08 TCS 山口 【SALES_2】性能改善 END

				 //--2013/11/29 TCS 各務 Aカード情報相互連携開発 START
	         } else if (page == "page3") {
	             //2ページ目表示設定
	             if ($("#customerTitleLabel").text() == $("#reasonTitleLabel").text()) {
	                 setPopupCustomerEditPage("page2", "actvctgryList");
	             } else if ($("#customerTitleLabel").text() == $("#districtLabel").text()) {
	                 setPopupCustomerEditPage("page2", "stateList");
	                 $(".dataWindDistrict .ListBox01 .scroll-inner").css({ "transform": "translate3d(0px, 0px, 0px)" });
	             }
	         } else if (page == "page4") {
	             //3ページ目表示設定
	             setPopupCustomerEditPage("page3", "districtList");
	             $(".dataWindCity .ListBox01 .scroll-inner").css({ "transform": "translate3d(0px, 0px, 0px)" });
	         } else {
	             //4ページ目表示設定
	             setPopupCustomerEditPage("page4");
	             $(".dataWindLocation .ListBox01 .scroll-inner").css({ "transform": "translate3d(0px, 0px, 0px)" });
	         }
	         //--2013/11/29 TCS 各務 Aカード情報相互連携開発 END
         }

		 e.stopImmediatePropagation();

		 //2012/03/08 TCS 山口 【SALES_2】性能改善 END
	 }
 );


 //2012/08/23 TCS 山口 【A STEP2】次世代e-CRB 新車商談機能改善 START
 //電話番号検索（携帯）
 $(".scNscCustomerEditingListItem5 table td #mobileSerchButtonImage").live("click",
	 function (e) {
		 if ($("#mobileTextBox").val() != "") {
			 $("#birthdayHidden").val($("#birthdayTextBox").val());
			 $("#mobileSerchButton").click();
		 }
	 }
 );
 //電話番号検索（自宅）
 $(".scNscCustomerEditingListItem5 table td #telnoSerchButtonImage").live("click",
	 function (e) {
		 if ($("#telnoTextBox").val() != "") {
			 $("#birthdayHidden").val($("#birthdayTextBox").val());
			 $("#telnoSerchButton").click();
		 }
	 }
 );
 //2012/08/23 TCS 山口 【A STEP2】次世代e-CRB 新車商談機能改善 END

//--2013/11/29 TCS 各務 Aカード情報相互連携開発 START
//国民ID検索
$(".scNscCustomerEditingListItem5 table td #socialIdSearchButtonImage").live("click",
        function (e) {
            if ($("#socialidTextBox").val() != "") {
                $("#birthdayHidden").val($("#birthdayTextBox").val());
                $("#socialIdSearchButton").click();
            }
        }
    );
//--2013/11/29 TCS 各務 Aカード情報相互連携開発 END


 // 顧客編集ポップアップ関連 -------------------------------------------------------------------

 //ポップアップクローズの監視
 $(document.body).bind("mousedown touchstart", function (e) {
	 if ($("#scNscCustomerEditingWindown").is(":visible") === false) return;
	 //2012/03/08 TCS 山口 【SALES_2】性能改善 START
	 if ($("#registOverlayBlack").hasClass("popupLoadingBackgroundColor") === true) return;
	 //2012/03/08 TCS 山口 【SALES_2】性能改善 END
	 if ($(e.target).is("#scNscCustomerEditingWindown, #scNscCustomerEditingWindown *") === false) {
		 //2012/03/08 TCS 山口 【SALES_2】性能改善 START
         //2019/02/13 TCS 河原 顧客編集ポップアップを欄外タップで閉じないように変更 DELETE
		 //2012/03/08 TCS 山口 【SALES_2】性能改善 END
	 }
 });

 // 車両編集ポップアップ関連 -------------------------------------------------------------------

 //ポップアップクローズの監視
 $(document.body).bind("mousedown touchstart", function (e) {
	 if ($("#scVehicleEditingWindown").is(":visible") === false) return;
	 //2012/03/08 TCS 山口 【SALES_2】性能改善 START
	 if ($("#registOverlayBlack").hasClass("popupLoadingBackgroundColor") === true) return;
	 //2012/03/08 TCS 山口 【SALES_2】性能改善 END
	 if ($(e.target).is("#scVehicleEditingWindown, #scVehicleEditingWindown *") === false) {
		 //2012/03/08 TCS 山口 【SALES_2】性能改善 START
		 CustomerCarEditPopUpClose();
		 //2012/03/08 TCS 山口 【SALES_2】性能改善 END
	 }
 });


function CompletionCustomerInfo(e) {

    //2017/11/16 TCS 河原 TKM独自機能開発 START
    //クレンジングモード時は通常のチェックは実施しない
    if ($("#CleansingMode").val() == "0") {
        //2012/08/23 TCS 山口 【A STEP2】次世代e-CRB 新車商談機能改善 START
        //氏名未入力エラー
        //新規登録時は必須チェックをしない
        if ($("#editModeHidden").val() == "1") {
            if ($("#nameTextBox").attr("disabled") == false) {
                if ($("#nameTextBox").val() == "") {
                    //--2013/11/29 TCS 各務 Aカード情報相互連携開発 START
                    //ラベル・敬称設定フラグON かつ 個人法人項目コード = "C"(Company) or "G"(Govt Org)のとき
                    if ($("#labelNametitleSettingHidden").val() == "1" &&
                     ($("#privateFleetItemHidden").val() == "C" || $("#privateFleetItemHidden").val() == "G")) {
                        // 会社名を入力してください。
                        alert($("#custNoFirmNameErrMsg").val());
                    } else {
                        // ファーストネームを入力してください。
                        alert($("#custNoNameErrMsg").val());
                    }
                    //--2013/11/29 TCS 各務 Aカード情報相互連携開発 END
                    return;
                }
            }
        }
        //2012/08/23 TCS 山口 【A STEP2】次世代e-CRB 新車商談機能改善 END

        //2012/08/23 TCS 山口 【A STEP2】次世代e-CRB 新車商談機能改善 START
        //顧客更新時かつダミー名称フラグがONの場合、
        if ($("#editModeHidden").val() == "1") {
            if ($("#dummyNameFlgHidden").val() == "1") {
                //氏名が変更されていない場合、エラー
                if ($("#nameTextBox").val() == $("#nameBeforeHidden").val()) {
                    alert($("#custNoDummyNameFlgErrMsg").val());
                    return;
                }
            }
        }
        //2012/08/23 TCS 山口 【A STEP2】次世代e-CRB 新車商談機能改善 END

        //2012/08/23 TCS 山口 【A STEP2】次世代e-CRB 新車商談機能改善 START
        //自宅・携帯電話未入力エラー
        //新規登録時は必須チェックをしない
        if ($("#editModeHidden").val() == "1") {
            if (($("#mobileTextBox").attr("disabled") == false) || ($("#telnoTextBox").attr("disabled") == false)) {
                if (($("#mobileTextBox").val() == "") && ($("#telnoTextBox").val() == "")) {
                    alert($("#custNoTelNoErrMsg").val());
                    return;
                }
            }
        }
        //2012/08/23 TCS 山口 【A STEP2】次世代e-CRB 新車商談機能改善 END

        //--2013/11/29 TCS 各務 Aカード情報相互連携開発 START
        //ミドルネーム未入力エラー
        //新規登録時は必須チェックをしない、また入力設定フラグ=2(表示、必須)のときのみチェックする
        if ($("#editModeHidden").val() == "1" && $("#inputSettingHidden02").val() == "2") {
            if ($("#middleNameTextBox").attr("disabled") == false) {
                if ($("#middleNameTextBox").val() == "") {
                    alert($("#custNoMiddleNameErrMsg").val());
                    return;
                }
            }
        }

        //ラストネーム未入力エラー
        //新規登録時は必須チェックをしない、また入力設定フラグ=2(表示、必須)のときのみチェックする
        if ($("#editModeHidden").val() == "1" && $("#inputSettingHidden03").val() == "2") {
            if ($("#lastNameTextBox").attr("disabled") == false) {
                if ($("#lastNameTextBox").val() == "") {
                    //ラベル・敬称設定フラグON かつ 個人法人項目コード = "C"(Company) or "G"(Govt Org)のとき
                    if ($("#labelNametitleSettingHidden").val() == "1" &&
                     ($("#privateFleetItemHidden").val() == "C" || $("#privateFleetItemHidden").val() == "G")) {
                        // 担当者を入力してください。
                        alert($("#custNoContactPersonErrMsg").val());
                    } else {
                        // ラストネームを入力してください。
                        alert($("#custNoLastNameErrMsg").val());
                    }
                    return;
                }
            }
        }

        //性別未入力エラー
        //新規登録時は必須チェックをしない、また入力設定フラグ=2(表示、必須)のときのみチェックする
        if ($("#editModeHidden").val() == "1" && $("#inputSettingHidden04").val() == "2") {
            if (!($("#manCheckBox").attr("disabled")) || !($("#girlCheckBox").attr("disabled")) ||
            !($("#otherCheckBox").attr("disabled")) || !($("#unknownCheckBox").attr("disabled"))) {
                if (!($("#manCheckBox").attr("checked")) && !($("#girlCheckBox").attr("checked")) &&
                !($("#otherCheckBox").attr("checked")) && !($("#unknownCheckBox").attr("checked"))) {
                    alert($("#custNoSexErrMsg").val());
                    return;
                }
            }
        }

        //敬称未入力エラー
        //新規登録時は必須チェックをしない、また入力設定フラグ=2(表示、必須)のときのみチェックする
        if ($("#editModeHidden").val() == "1" && $("#inputSettingHidden05").val() == "2") {
            if ($("#nameTitle").attr("disabled") == false) {
                if ($("#nameTitle").val() == "") {
                    alert($("#custNoNameTitleErrMsg").val());
                    return;
                }
            }
        }

        // 2020/02/04 TS 舩橋 TKM Change request development for Next Gen e-CRB (CR008,CR060,CR072) START
        //個人/法人未入力エラー
        //入力設定フラグ=2(表示、必須)のときのみチェック、自社客の時はチェックをしない
        if ($("#inputSettingHidden06").val() == "2" && $("#cust_flg_hidden").val() != "1") {
            if (!($("#kojinCheckBox").attr("disabled")) || !($("#houjinCheckBox").attr("disabled"))) {
                if (!($("#kojinCheckBox").attr("checked")) && !($("#houjinCheckBox").attr("checked"))) {
                    alert($("#custNoCustypeErrMsg").val());
                    return;
                }
            }
        }

        // 個人法人項目未入力エラー
        // 顧客組織名称未入力エラー
        // 顧客サブカテゴリ2未入力エラー

        // 2020/02/04 TS 舩橋 TKM Change request development for Next Gen e-CRB (CR008,CR060,CR072) END

        //担当者氏名未入力エラー
        //新規登録時は必須チェックをしない、また入力設定フラグ=2(表示、必須)のときのみチェックする
        if ($("#editModeHidden").val() == "1" && $("#inputSettingHidden08").val() == "2") {
            if ($("#employeenameTextBox").attr("disabled") == false) {
                if ($("#employeenameTextBox").val() == "") {
                    alert($("#custNoEmpNameErrMsg").val());
                    return;
                }
            }
        }

        //担当者部署未入力エラー
        //新規登録時は必須チェックをしない、また入力設定フラグ=2(表示、必須)のときのみチェックする
        if ($("#editModeHidden").val() == "1" && $("#inputSettingHidden09").val() == "2") {
            if ($("#employeedepartmentTextBox").attr("disabled") == false) {
                if ($("#employeedepartmentTextBox").val() == "") {
                    alert($("#custNoEmpDeptErrMsg").val());
                    return;
                }
            }
        }

        //担当者役職未入力エラー
        //新規登録時は必須チェックをしない、また入力設定フラグ=2(表示、必須)のときのみチェックする
        if ($("#editModeHidden").val() == "1" && $("#inputSettingHidden10").val() == "2") {
            if ($("#employeepositionTextBox").attr("disabled") == false) {
                if ($("#employeepositionTextBox").val() == "") {
                    alert($("#custNoEmpPosErrMsg").val());
                    return;
                }
            }
        }

        //勤務先電話番号未入力エラー
        //新規登録時は必須チェックをしない、また入力設定フラグ=2(表示、必須)のときのみチェックする
        if ($("#editModeHidden").val() == "1" && $("#inputSettingHidden13").val() == "2") {
            if ($("#businesstelnoTextBox").attr("disabled") == false) {
                if ($("#businesstelnoTextBox").val() == "") {
                    alert($("#custNoBussinessTelErrMsg").val());
                    return;
                }
            }
        }

        //FAX番号未入力エラー
        //新規登録時は必須チェックをしない、また入力設定フラグ=2(表示、必須)のときのみチェックする
        if ($("#editModeHidden").val() == "1" && $("#inputSettingHidden14").val() == "2") {
            if ($("#faxnoTextBox").attr("disabled") == false) {
                if ($("#faxnoTextBox").val() == "") {
                    alert($("#custNoFaxErrMsg").val());
                    return;
                }
            }
        }

        // 2020/02/04 TS 舩橋 TKM Change request development for Next Gen e-CRB (CR008,CR060,CR072) START
        //郵便番号未入力エラー
        //新規登録時は必須チェックをしない、また入力設定フラグ=2(表示、必須)のときのみチェックする、自社客の時はチェックをしない
        if ($("#editModeHidden").val() == "1" && $("#inputSettingHidden15").val() == "2" && $("#cust_flg_hidden").val() != "1") {
            // 2020/02/04 TS 舩橋 TKM Change request development for Next Gen e-CRB (CR008,CR060,CR072) END
            if ($("#zipcodeTextBox").attr("disabled") == false) {
                if ($("#zipcodeTextBox").val() == "") {
                    alert($("#custNoZipErrMsg").val());
                    return;
                }
            }
        }

        //住所1未入力エラー
        //新規登録時は必須チェックをしない、また入力設定フラグ=2(表示、必須)のときのみチェックする
        if ($("#editModeHidden").val() == "1" && $("#inputSettingHidden16").val() == "2") {
            if ($("#addressTextBox").attr("disabled") == false) {
                if ($("#addressTextBox").val() == "") {
                    alert($("#custNoAddress1ErrMsg").val());
                    return;
                }
            }
        }

        //住所2未入力エラー
        //新規登録時は必須チェックをしない、また入力設定フラグ=2(表示、必須)のときのみチェックする
        if ($("#editModeHidden").val() == "1" && $("#inputSettingHidden17").val() == "2") {
            if ($("#address2TextBox").attr("disabled") == false) {
                if ($("#address2TextBox").val() == "") {
                    alert($("#custNoAddress2ErrMsg").val());
                    return;
                }
            }
        }

        //住所3未入力エラー
        //新規登録時は必須チェックをしない、また入力設定フラグ=2(表示、必須)のときのみチェックする
        if ($("#editModeHidden").val() == "1" && $("#inputSettingHidden18").val() == "2") {
            if ($("#address3TextBox").attr("disabled") == false) {
                if ($("#address3TextBox").val() == "") {
                    alert($("#custNoAddress3ErrMsg").val());
                    return;
                }
            }
        }

        //住所(州)未入力エラー
        //新規登録時は必須チェックをしない、また入力設定フラグ=2(表示、必須)のときのみチェックする
        if ($("#editModeHidden").val() == "1" && $("#inputSettingHidden19").val() == "2") {
            if ($("#addressState").attr("disabled") == false) {
                if ($("#addressState").val() == "") {
                    alert($("#custNoStateErrMsg").val());
                    return;
                }
            }
        }

        //住所(地域)未入力エラー
        //新規登録時は必須チェックをしない、また入力設定フラグ=2(表示、必須)のときのみチェックする
        if ($("#editModeHidden").val() == "1" && $("#inputSettingHidden20").val() == "2") {
            if ($("#addressDistrict").attr("disabled") == false) {
                if ($("#addressDistrict").val() == "") {
                    alert($("#custNoDistrictErrMsg").val());
                    return;
                }
            }
        }

        //住所(市)未入力エラー
        //新規登録時は必須チェックをしない、また入力設定フラグ=2(表示、必須)のときのみチェックする
        if ($("#editModeHidden").val() == "1" && $("#inputSettingHidden21").val() == "2") {
            if ($("#addressCity").attr("disabled") == false) {
                if ($("#addressCity").val() == "") {
                    alert($("#custNoCityErrMsg").val());
                    return;
                }
            }
        }

        //住所(地区)未入力エラー
        //新規登録時は必須チェックをしない、また入力設定フラグ=2(表示、必須)のときのみチェックする
        if ($("#editModeHidden").val() == "1" && $("#inputSettingHidden22").val() == "2") {
            if ($("#addressLocation").attr("disabled") == false) {
                if ($("#addressLocation").val() == "") {
                    alert($("#custNoLocationErrMsg").val());
                    return;
                }
            }
        }

        //本籍未入力エラー
        //新規登録時は必須チェックをしない、また入力設定フラグ=2(表示、必須)のときのみチェックする
        if ($("#editModeHidden").val() == "1" && $("#inputSettingHidden23").val() == "2") {
            if ($("#domicileTextBox").attr("disabled") == false) {
                if ($("#domicileTextBox").val() == "") {
                    alert($("#custNoDomicileErrMsg").val());
                    return;
                }
            }
        }

        //e-Mail1未入力エラー
        //新規登録時は必須チェックをしない、また入力設定フラグ=2(表示、必須)のときのみチェックする
        if ($("#editModeHidden").val() == "1" && $("#inputSettingHidden24").val() == "2") {
            if ($("#email1TextBox").attr("disabled") == false) {
                if ($("#email1TextBox").val() == "") {
                    alert($("#custNoEmail1ErrMsg").val());
                    return;
                }
            }
        }

        //e-Mail2未入力エラー
        //新規登録時は必須チェックをしない、また入力設定フラグ=2(表示、必須)のときのみチェックする
        if ($("#editModeHidden").val() == "1" && $("#inputSettingHidden25").val() == "2") {
            if ($("#email2TextBox").attr("disabled") == false) {
                if ($("#email2TextBox").val() == "") {
                    alert($("#custNoEmail2ErrMsg").val());
                    return;
                }
            }
        }

        //国籍未入力エラー
        //新規登録時は必須チェックをしない、また入力設定フラグ=2(表示、必須)のときのみチェックする
        if ($("#editModeHidden").val() == "1" && $("#inputSettingHidden26").val() == "2") {
            if ($("#countryTextBox").attr("disabled") == false) {
                if ($("#countryTextBox").val() == "") {
                    alert($("#custNoCountryErrMsg").val());
                    return;
                }
            }
        }

        //国民ID未入力エラー
        //新規登録時は必須チェックをしない、また入力設定フラグ=2(表示、必須)のときのみチェックする
        if ($("#editModeHidden").val() == "1" && $("#inputSettingHidden27").val() == "2") {
            if ($("#socialidTextBox").attr("disabled") == false) {
                if ($("#socialidTextBox").val() == "") {
                    alert($("#custNoSocialIdErrMsg").val());
                    return;
                }
            }
        }

        //誕生日未入力エラー
        //新規登録時は必須チェックをしない、また入力設定フラグ=2(表示、必須)のときのみチェックする
        if ($("#editModeHidden").val() == "1" && $("#inputSettingHidden28").val() == "2") {
            if ($("#birthdayTextBox").attr("disabled") == false) {
                if ($("#birthdayTextBox").val() == "") {
                    alert($("#custNoBirtydayErrMsg").val());
                    return;
                }
            }
        }

        //活動区分未入力エラー
        //新規登録時は必須チェックをしない、また入力設定フラグ=2(表示、必須)のときのみチェックする
        if ($("#editModeHidden").val() == "1" && $("#inputSettingHidden29").val() == "2") {
            if ($("#useActvctgryHidden").val() == "1") {
                if ($("#actvctgryLabel").text() == "") {
                    alert($("#custNoActvctgryErrMsg").val());
                    return;
                }
            }
        }
        //--2013/11/29 TCS 各務 Aカード情報相互連携開発 END

        //2014/03/18 TCS 市川  新PF開発No.41（商業情報活動）START
        //2014/10/02 TCS 河原 商業情報必須指定解除のタブレット側対応 START
        //商業情報受取区分未入力エラー
        //新規登録時は必須チェックをしない、また入力設定フラグ=2(表示、必須)のときのみチェックする
        if ($("#editModeHidden").val() == "1" && $("#inputSettingHidden36").val() == "2") {
            if (!$("#commercialRecvType_Empty").attr("disabled")) {
                if ($(".CommercialRecvType input:checked").length == 0) {
                    alert($("#custNoCommercialRecvType").val());
                    return;
                }
            }
        }
    //2014/10/02 TCS 河原 商業情報必須指定解除のタブレット側対応 END
    //2014/03/18 TCS 市川  新PF開発No.41（商業情報活動）END
    }
    //2017/11/16 TCS 河原 TKM独自機能開発 END

    var prms = "";
    
    //--2013/11/29 TCS 各務 Aカード情報相互連携開発 START
    prms = prms + encodeURIComponent($("#nameTextBox").val()) + ",";                      //ファーストネーム
    prms = prms + encodeURIComponent($("#middleNameTextBox").val()) + ",";                //ミドルネーム
    prms = prms + encodeURIComponent($("#lastNameTextBox").val()) + ",";                  //ラストネーム
    //--2013/11/29 TCS 各務 Aカード情報相互連携開発 END
    prms = prms + encodeURIComponent($("#nameTitleHidden").val()) + ",";                  //敬称コード
    prms = prms + encodeURIComponent($("input[id=manCheckBox]:checked").val()) + ",";     //男
    prms = prms + encodeURIComponent($("input[id=girlCheckBox]:checked").val()) + ",";    //女
    //--2013/11/29 TCS 各務 Aカード情報相互連携開発 START
    prms = prms + encodeURIComponent($("input[id=otherCheckBox]:checked").val()) + ",";   //その他
    prms = prms + encodeURIComponent($("input[id=unknownCheckBox]:checked").val()) + ","; //不明
    //--2013/11/29 TCS 各務 Aカード情報相互連携開発 END
    prms = prms + encodeURIComponent($("input[id=kojinCheckBox]:checked").val()) + ",";   //法人
    prms = prms + encodeURIComponent($("input[id=houjinCheckBox]:checked").val()) + ",";  //個人

    prms = prms + encodeURIComponent($("#employeenameTextBox").val()) + ",";               //担当者氏名
    prms = prms + encodeURIComponent($("#employeedepartmentTextBox").val()) + ",";         //担当者部署名
    prms = prms + encodeURIComponent($("#employeepositionTextBox").val()) + ",";           //役職

    prms = prms + encodeURIComponent($("#mobileTextBox").val()) + ",";                     //携帯
    prms = prms + encodeURIComponent($("#telnoTextBox").val()) + ",";                      //自宅
    prms = prms + encodeURIComponent($("#businesstelnoTextBox").val()) + ",";              //勤務先
    prms = prms + encodeURIComponent($("#faxnoTextBox").val()) + ",";                      //FAX

    prms = prms + encodeURIComponent($("#zipcodeTextBox").val()) + ",";                    //郵便番号
    //--2013/11/29 TCS 各務 Aカード情報相互連携開発 START
    prms = prms + encodeURIComponent($("#addressTextBox").val()) + ",";                    //住所1
    prms = prms + encodeURIComponent($("#address2TextBox").val()) + ",";                   //住所2
    prms = prms + encodeURIComponent($("#address3TextBox").val()) + ",";                   //住所3
    prms = prms + encodeURIComponent($("#stateHidden").val()) + ",";                       //住所(州)
    prms = prms + encodeURIComponent($("#addressState").val()) + ",";                      //住所(州)(名称)
    prms = prms + encodeURIComponent($("#districtHidden").val()) + ",";                    //住所(地域)
    prms = prms + encodeURIComponent($("#addressDistrict").val()) + ",";                   //住所(地域)(名称)
    prms = prms + encodeURIComponent($("#cityHidden").val()) + ",";                        //住所(市)
    prms = prms + encodeURIComponent($("#addressCity").val()) + ",";                       //住所(市)(名称)
    prms = prms + encodeURIComponent($("#locationHidden").val()) + ",";                    //住所(地区)
    prms = prms + encodeURIComponent($("#addressLocation").val()) + ",";                   //住所(地区)(名称)
    //--2013/11/29 TCS 各務 Aカード情報相互連携開発 END

    prms = prms + encodeURIComponent($("#email1TextBox").val()) + ",";                     //E-Mail1
    prms = prms + encodeURIComponent($("#email2TextBox").val()) + ",";                     //E-Mail2

    prms = prms + encodeURIComponent($("#socialidTextBox").val()) + ",";                    //国民ID

    prms = prms + encodeURIComponent($("#birthdayTextBox").val()) + ",";                    //誕生日

    prms = prms + encodeURIComponent($("#actvctgryidHidden").val()) + ",";                  //活動区分
    prms = prms + encodeURIComponent($("#reasonidHidden").val()) + ",";                     //断念理由

    prms = prms + encodeURIComponent($("input[id=smsCheckButton]:checked").val()) + ",";    //SMS
    prms = prms + encodeURIComponent($("input[id=emailCheckButton]:checked").val()) + ",";  //EMail
    prms = prms + encodeURIComponent($("#nameTitleTextHidden").val()) + ",";                //敬称
    //--2013/11/29 TCS 各務 Aカード情報相互連携開発 START
    prms = prms + encodeURIComponent($("#privateFleetItemHidden").val()) + ",";             //個人法人項目
    prms = prms + encodeURIComponent($("#domicileTextBox").val()) + ",";                    //本籍
    prms = prms + encodeURIComponent($("#countryTextBox").val());                           //国籍
    //--2013/11/29 TCS 各務 Aカード情報相互連携開発 END
    //2012/03/08 TCS 河原 【SALES_1B】コールバック時の文字列のエンコード処理追加 END
    //2014/03/18 TCS 市川  新PF開発No.41（商業情報活動）START
    prms = prms + "," + encodeURIComponent($(".CommercialRecvType input:checked").attr("value"));
    //2014/03/18 TCS 市川  新PF開発No.41（商業情報活動）END
    //2014/05/07 TCS 市川 PCとタブレットで顧客入力情報不一致対応(CHG-354) START
    prms = prms + "," + encodeURIComponent($("#incomeTextBox").val());                      //収入
    //2014/05/07 TCS 市川 PCとタブレットで顧客入力情報不一致対応(CHG-354) END

    // 2018/08/27 TCS 佐々木 TKM Next Gen e-CRB Project Application development Block B-1 START
    // 顧客組織コード
    // 顧客組織入力区分
    // 顧客組織名称
    // 顧客サブカテゴリ2コード
    prms += ',' + ['#custOrgnzHidden', '#custOrgnzInputTypeHidden', '#custOrgnz', '#custSubCtgry2Hidden']
        .map(function (selector) { return encodeURIComponent($(selector).val()) })
        .join(',');
    // 2018/08/27 TCS 佐々木 TKM Next Gen e-CRB Project Application development Block B-1 END

    //処理中フラグを立てる
    $("#serverProcessFlgHidden").val("1");          //サーバーサイド処理フラグ (1:処理中)
    SC3080201.startServerCallback();


    //2012/05/17 TCS 安田 クルクル対応 START

    //再表示判定Function
    function updateRefreshTimerFunc() {

        //二度押し防止フラグをキャンセル(false)にする
        SC3080201.serverProcessing = false;

        if ($("#editModeHidden").val() == "0") {

            //（追加時）
            //顧客編集（新規登録時）
            CustomerInsertPopUpClose()

            //繰り返し処理をしない
            return false;

        } else {

            //（編集時）
            //顧客編集ポップアップを閉じる
            CustomerEditPopUpWindowClose();

            //顧客編集領域の再表示
            CustomerEditPopUpClose()

            //繰り返し処理をしない
            return false;
        }
    }

    //タイマーセット
    commonRefreshTimer(updateRefreshTimerFunc);

    //2012/05/17 TCS 安田 クルクル対応 END

    callback.doCallback("CustomerUpdate", prms, function (result, context) {

        //2012/05/17 TCS 安田 クルクル対応 START
        commonClearTimer();
        //2012/05/17 TCS 安田 クルクル対応 END

        $("#serverProcessFlgHidden").val("");       //サーバーサイド処理フラグ

        SC3080201.endServerCallback();

        var resArray = result.split(",");
        if (resArray[1] == "0") {

            //$(".scNscCustomerEditingCancellButton").click();

            if ($("#editModeHidden").val() == "0") {
                //顧客編集（新規登録時）
                CustomerInsertPopUpClose()

                SC3080201.startServerCallback();
            } else {
                //2012/03/08 TCS 山口 【SALES_2】性能改善 START
                CustomerEditPopUpWindowClose();
                //2012/03/08 TCS 山口 【SALES_2】性能改善 END

                //顧客編集（編集時）
                CustomerEditPopUpClose()
            }

            //2012/03/08 TCS 山口 【SALES_2】性能改善 DELETE

        } else {
            alert(SC3080201HTMLDecode(resArray[2].replace('@@@',',')));
        }
        //2017/11/16 TCS 河原 TKM独自機能開発 START
        $("#CleansingResult").val(resArray[3]);
        //2017/11/16 TCS 河原 TKM独自機能開発 END  
    });
}

// 顧客情報クリック時に顧客編集ポップアップ表示(サーバー処理後)
function CustomerEditPopUpOpenAfter() {

    //カスタムコントロール設定
    $(".scNscCustomerEditingListArea input:text").CustomTextBox({ "useEllipsis": "true" });
    $("#birthdayTextBox").DateTimeSelector();

    // 顧客編集ポップアップ設定
    setPopupCustomerEditIinital();

    //スクロール設定
    $("#scNscCustomerEditingWindown #scNscCustomerEditingWindownBox .scNscCustomerEditingListBox2").fingerScroll();
    $("#scNscCustomerEditingWindown #scNscCustomerEditingWindownBox .dataWindNameTitle .ListBox01").fingerScroll();
    //2012/03/08 TCS 山口 【SALES_2】性能改善 START
    $("#scNscCustomerEditingWindown #scNscCustomerEditingWindownBox .dataWindActvctgry .ListBox01 .dataWind2").fingerScroll();
    $("#scNscCustomerEditingWindown #scNscCustomerEditingWindownBox .dataWindReason .ListBox01 .dataWind2").fingerScroll();
    //2012/03/08 TCS 山口 【SALES_2】性能改善 END
    //--2013/11/29 TCS 各務 Aカード情報相互連携開発 START
    $("#scNscCustomerEditingWindown #scNscCustomerEditingWindownBox .dataWindPrivateFleetItem .ListBox01").fingerScroll();
    $("#scNscCustomerEditingWindown #scNscCustomerEditingWindownBox .dataWindState .ListBox01").fingerScroll();
    $("#scNscCustomerEditingWindown #scNscCustomerEditingWindownBox .dataWindDistrict .ListBox01").fingerScroll();
    $("#scNscCustomerEditingWindown #scNscCustomerEditingWindownBox .dataWindCity .ListBox01").fingerScroll();
    $("#scNscCustomerEditingWindown #scNscCustomerEditingWindownBox .dataWindLocation .ListBox01").fingerScroll();

    //顧客組織リストのスクロール
    $("#scNscCustomerEditingWindown #scNscCustomerEditingWindownBox .dataWindCustOrgnz .ListBox01").fingerScroll();

    //顧客サブカテゴリ2リストのスクロール
    $("#scNscCustomerEditingWindown #scNscCustomerEditingWindownBox .dataWindCustSubCtgry2 .ListBox01").fingerScroll();

    //--2013/11/29 TCS 各務 Aカード情報相互連携開発 END

    //2017/11/16 TCS 河原 TKM独自機能開発 START
    //法人
    if ($("input[id=houjinCheckBox]:checked").val() == 'on') {
        if ($("#CleansingMode").val() == "0") {
            //法人情報の表示
            $("#houjinPanel").css("display", "block");
        }

        if ($("#Use_Customerdata_Cleansing_Flg").val() == "1") {
            $("#middleNameTextBox").val("");
            $("#lastNameTextBox").val("");
            $("#middleNameTextBox").attr("disabled", "true");
            $("#lastNameTextBox").attr("disabled", "true");

            //ミドルネームとラストネーム欄を表示
            $("#row02").css("display", "none");
            $("#row03").css("display", "none");

            //デザインの調整
            $("#header01").removeClass("scNscCustomerEditingListItemBottomBorder");
            $("#data01").removeClass("scNscCustomerEditingListItemBottomBorder");
        }
    }

    changeAddress();
    //2017/11/16 TCS 河原 TKM独自機能開発 END

    //--2013/11/29 TCS 各務 Aカード情報相互連携開発 START
    var nameLastRow = 1; //ファーストネームは必ず表示
    var fleetCnt = 0;
    var fleetLastRow = 0;
    var telLastRow = 12; //携帯番号、自宅番号は必ず表示
    var addressCnt = 0;
    var addressLastRow = 0;
    var emailCnt = 0;
    var emailLastRow = 0;

    //入力項目設定(hidden)を確認
    for (i = 1; i <= 29; i++) {
        // 2020/02/04 TS 舩橋 TKM Change request development for Next Gen e-CRB (CR008,CR060,CR072) START
        // 2018/08/27 TCS 佐々木 TKM Next Gen e-CRB Project Application development Block B-1 START
        if ([7].includes(i)) continue;
        // 2018/08/27 TCS 佐々木 TKM Next Gen e-CRB Project Application development Block B-1 END
    　　// 2020/02/04 TS 舩橋 TKM Change request development for Next Gen e-CRB (CR008,CR060,CR072) END　
    
        var strId = i.toString();
        if (strId.length == 1) {
            strId = "0" + strId;
        }

        if (i == 1 || i == 11 || i == 12) {
            //ファーストネーム、携帯番号、自宅番号は必ず表示、入力必須
            $("#header" + strId).addClass("scNscCustomerEditingListItemRedTxt");
        } else {
            //"1"のとき(表示、必須でない)
            if ($("#inputSettingHidden" + strId).val() == "1") {
                $("#header" + strId).addClass("scNscCustomerEditingListItemHeavyGrayTxt");
            }
            //"2"のとき(表示、必須)
            else if ($("#inputSettingHidden" + strId).val() == "2") {
                $("#header" + strId).addClass("scNscCustomerEditingListItemRedTxt");
            }
            //それ以外(非表示)
            else {
                $("#row" + strId).css("display", "none");
            }
        }

        //表示項目数をカウント
        //氏名
        if (i >= 2 && i <= 3) {
            if ($("#inputSettingHidden" + strId).val() == "1" || $("#inputSettingHidden" + strId).val() == "2") {
                nameLastRow = i;
            }
        }
        //法人項目
        if (i >= 8 && i <= 10) {
            if ($("#inputSettingHidden" + strId).val() == "1" || $("#inputSettingHidden" + strId).val() == "2") {
                fleetCnt++;
                fleetLastRow = i;
            }
        }
        //電話番号
        if (i >= 13 && i <= 14) {
            if ($("#inputSettingHidden" + strId).val() == "1" || $("#inputSettingHidden" + strId).val() == "2") {
                telLastRow = i;
            }
        }
        //住所
        if (i >= 15 && i <= 22) {
            if ($("#inputSettingHidden" + strId).val() == "1" || $("#inputSettingHidden" + strId).val() == "2") {
                addressCnt++;
                addressLastRow = i;
            }
        }
        //e-mail
        if (i >= 24 && i <= 25) {
            if ($("#inputSettingHidden" + strId).val() == "1" || $("#inputSettingHidden" + strId).val() == "2") {
                emailCnt++;
                emailLastRow = i;
            }
        }
    }

    //2014/10/02 TCS 河原 商業情報必須指定解除のタブレット側対応 START
    //"1"のとき(表示、必須でない)
    if ($("#inputSettingHidden36").val() == "1") {
        $("#header36").addClass("scNscCustomerEditingListItemHeavyGrayTxt");
    }
    //"2"のとき(表示、必須)
    else if ($("#inputSettingHidden36").val() == "2") {
        $("#header36").addClass("scNscCustomerEditingListItemRedTxt");
    }
    //それ以外(非表示)
    else {
        $("#row36").css("display", "none");
    }
    //2014/10/02 TCS 河原 商業情報必須指定解除のタブレット側対応 END

    // 2020/02/04 TS 舩橋 TKM Change request development for Next Gen e-CRB (CR008,CR060,CR072) START
    // Customer Category
    //"1"のとき(表示、必須でない)
    if ($("#inputSettingHidden06").val() == "1") 
    {
        $("#header06").addClass("scNscCustomerEditingListItemHeavyGrayTxt");
    }
    //"2"のとき(表示、必須)
    else if ($("#inputSettingHidden06").val() == "2") 
    {
        // "1"のとき自社客
        if($("#cust_flg_hidden").val() == "1")
        {
            $("#header06").addClass("scNscCustomerEditingListItemHeavyGrayTxt");
        }        
        else
        {
            $("#header06").addClass("scNscCustomerEditingListItemRedTxt");
        }
    }

    // SubCategory1、組織、SubCategory2 を任意項目スタイル
    $('#header07, #headerCustOrgnz, #headerCustSubCtgry2').addClass('scNscCustomerEditingListItemHeavyGrayTxt');

    // 郵便番号
    //"1"のとき(表示、必須でない)
    if ($("#inputSettingHidden15").val() == "1") 
    {
        $("#header15").addClass("scNscCustomerEditingListItemHeavyGrayTxt");
    }
    //"2"のとき(表示、必須)
    else if ($("#inputSettingHidden15").val() == "2") 
    {
        // "1"のとき自社客
        if($("#cust_flg_hidden").val() == "1")
        {
            $("#header15").addClass("scNscCustomerEditingListItemHeavyGrayTxt");
        }        
        else
        {
            $("#header15").addClass("scNscCustomerEditingListItemRedTxt");
        }
    }
    // 2020/02/04 TS 舩橋 TKM Change request development for Next Gen e-CRB (CR008,CR060,CR072) END　

    //余計な罫線を消す
    //氏名
    if (nameLastRow != 3 && nameLastRow != 0) {
        //氏名欄の最終行がラストネームでない(ラストネームが非表示)とき、下罫線を削除
        $("#header" + "0" + nameLastRow.toString()).removeClass("scNscCustomerEditingListItemBottomBorder");
        $("#data" + "0" + nameLastRow.toString()).removeClass("scNscCustomerEditingListItemBottomBorder");
    }
    //法人項目
    if (fleetLastRow != 10 && fleetLastRow != 0) {
        //法人項目欄の最終行が役職でない(役職が非表示)とき、下罫線を削除
        $("#header" + "0" + fleetLastRow.toString()).removeClass("scNscCustomerEditingListItemBottomBorder");
        $("#data" + "0" + fleetLastRow.toString()).removeClass("scNscCustomerEditingListItemBottomBorder");
    }
    //電話番号
    if (telLastRow != 14 && telLastRow != 0) {
        //電話番号欄の最終行がFAX番号でない(FAX番号が非表示)とき、下罫線を削除
        $("#header" + telLastRow.toString()).removeClass("scNscCustomerEditingListItemBottomBorder");
        $("#data" + telLastRow.toString()).removeClass("scNscCustomerEditingListItemBottomBorder");
    }
    //住所
    if (addressLastRow != 22 && addressLastRow != 0) {
        //住所欄の最終行が地区でない(地区が非表示)とき、下罫線を削除
        $("#header" + addressLastRow.toString()).removeClass("scNscCustomerEditingListItemBottomBorder");
        $("#data" + addressLastRow.toString()).removeClass("scNscCustomerEditingListItemBottomBorder");
    }
    //e-mail
    if (emailLastRow != 25 && emailLastRow != 0) {
        //e-mail欄の最終行がe-mail2でない(e-mail2が非表示)とき、下罫線を削除
        $("#header" + emailLastRow.toString()).removeClass("scNscCustomerEditingListItemBottomBorder");
        $("#data" + emailLastRow.toString()).removeClass("scNscCustomerEditingListItemBottomBorder");
    }

    //余計な外枠を消す
    //法人項目
    if (fleetCnt == 0) {
        $("#houjinTable").css("display", "none");
    }
    //住所
    if (addressCnt == 0) {
        $("#addressTable").css("display", "none");
    }
    //e-mail
    if (emailCnt == 0) {
        $("#emailTable").css("display", "none");
    }
    //氏名、電話番号は必ず表示行がある(外枠削除の必要なし)

    //住所検索ボタンの非表示設定
    if ($("#postSearchVisibleHidden").val() == "0") {
        $("#zipSerchButton").css("display", "none");
    }
    //--2013/11/29 TCS 各務 Aカード情報相互連携開発 END

    setPopupCustomerEditPage("page1");

    // 2012/01/26 TCS 安田 【SALES_1B】チェックマークのチェックの色（青／赤）を切り替える START
    //チェックマークの色を変える
    setCheckColor($("#manCheckBox"));
    setCheckColor($("#girlCheckBox"));
    //--2013/11/29 TCS 各務 Aカード情報相互連携開発 START
    setCheckColor($("#otherCheckBox"));
    setCheckColor($("#unknownCheckBox"));
    //--2013/11/29 TCS 各務 Aカード情報相互連携開発 END
    setCheckColor($("#kojinCheckBox"));
    setCheckColor($("#houjinCheckBox"));
    setCheckColor($("#smsCheckButton"));
    setCheckColor($("#emailCheckButton"));
    setCheckColor($("#dmailCheckButton"));
    // 2012/01/26 TCS 安田 【SALES_1B】チェックマークのチェックの色（青／赤）を切り替える END
    // 2014/03/18 TCS 市川  新PF開発No.41（商業情報活動）START
    setCheckColor($('#commercialRecvType_Empty'));
    setCheckColor($('#commercialRecvType_Yes'));
    setCheckColor($('#commercialRecvType_No'));     
    // 2014/03/18 TCS 市川  新PF開発No.41（商業情報活動）END

    //--2013/11/29 TCS 各務 Aカード情報相互連携開発 START
    //名前のラベルを変更する
    changeNameLabel();
    //--2013/11/29 TCS 各務 Aカード情報相互連携開発 END

    //2012/03/14 TCS 寺本 【SALES_2】 START (ポップアップ後読み込み化に伴い処理タイミング変更)
    $(function () {
        //チェックアイコンの位置調整(offset関数は非可視要素にはきかないので)
        setTimeout(function () {
            $(".scNscCustomerEditingListItemBox span.icrop-CheckMark").each(function () {
                $("span", this).not("icrop-CheckMark-label").css({ left: "", right: "0px", top: "5px" });
            });
        }, 0);
    });
    //2012/03/14 TCS 寺本 【SALES_2】 END

    $("#scNscCustomerEditingWindown").fadeIn(0);
   
    // 2018/08/27 TCS 佐々木 TKM Next Gen e-CRB Project Application development Block B-1 START
    //サブカテゴリ2リスト生成
    setTimeout(function () {
        var createParams = function (obj) {return Object.keys(obj).map(function (key) { return key + '=' + encodeURIComponent(obj[key]) }).join(',');};
        var params = {privateFleetItemCd: $("#privateFleetItemHidden").val(),custOrgnzName: "",custOrgnzCd: $("#custOrgnzHidden").val()};
        var SelectCustOrgnzListCallback = function (result, context) {var format = function (tmpl) {var args = Array.prototype.slice.call(arguments, 1);return args.reduce(function (acc, curr) { return acc.replace('{}', curr) }, tmpl);};var tmpl = '<li id="custSubCtgry2List{}" class="custSubCtgry2List"><p class="custSubCtgry2Label">{}</p><p class="custSubCtgry2Hidden">{}</p><p class="custSubCtgry2PrivateFleetItemCd" style="display: none">{}</p><p class="custSubCtgry2CustOrgnzCd" style="display: none">{}</p></li>';var list = parseOrEmptyArray(result);$('.custSubCtgry2ListBoxSetIn').empty().append(list.map(function (obj) { return format(tmpl, obj.subcat2Cd, obj.subcat2Name, obj.subcat2Cd, obj.privatefleetItemCd, obj.orgnzCd) }).join('')).find('.custOrgnzList:last-child').addClass('endRow');};
        callback.doCallback('ConfirmCustOrgnz', createParams(params), function (result, context) {SelectCustOrgnzListCallback(result, context);});
    }, 0);
    // 2018/08/27 TCS 佐々木 TKM Next Gen e-CRB Project Application development Block B-1 END
    
    //共通読込みアニメーション戻し
    $("#processingServer").removeClass("customerEditPopupLoadingAnimation");
    $("#registOverlayBlack").removeClass("popupLoadingBackgroundColor");
}

//--2013/11/29 TCS 各務 Aカード情報相互連携開発 START
// 顧客編集　個人法人項目の値で名前のラベル変更　-----------------------------------
function changeNameLabel() {
    //ラベル・敬称設定フラグONの場合のみ
    if ($("#labelNametitleSettingHidden").val() == "1") {
        var privateFleetSel = $("#privateFleetItemHidden").val(); //個人法人項目コード(選択中)
        //個人法人項目コード = "C"(Company) or "G"(Govt Org)のとき
        if (privateFleetSel == "C" || privateFleetSel == "G") {
            //ラベル(無印)を非表示に、ラベル2を表示にする
            $("#customerFiestNameLabel").css("display", "none");
            $("#customerLastNameLabel").css("display", "none");
            $("#customerFiestNameLabel2").css("display", "block");
            $("#customerLastNameLabel2").css("display", "block");
        } else {
            $("#customerFiestNameLabel").css("display", "block");
            $("#customerLastNameLabel").css("display", "block");
            $("#customerFiestNameLabel2").css("display", "none");
            $("#customerLastNameLabel2").css("display", "none");
        }
    }
}
//--2013/11/29 TCS 各務 Aカード情報相互連携開発 END

//顧客編集ポップアップ非表示処理
function CustomerEditPopUpWindowClose() {
    //ポップアップ非表示
    $("#scNscCustomerEditingWindown").fadeOut(300);
    setTimeout(function () {
        //強制的に1ページ目に
        setPopupCustomerEditPage("page1");
        //HTML削除
        $("#CustomerEditVisiblePanel").empty();
    }, 300);
    //顧客編集-キャンセル-変更前情報の情報に戻す
    //cancelCustomerInfo();

    //念のためフラグをクリアする
    $("#serverProcessFlgHidden").val("");

    //更新： 2019/02/13 TCS 河原 顧客編集ポップアップを欄外タップで閉じないように変更
    $("#CustomerEditOverlayBlack").css("display", "none");
    //更新： 2019/02/13 TCS 河原 顧客編集ポップアップを欄外タップで閉じないように変更

}
//2012/03/08 TCS 山口 【SALES_2】性能改善 END


// 顧客編集ポップアップ設定　-------------------------------------------------
function setPopupCustomerEditIinital() {

    // 2012/01/26 TCS 安田 【SALES_1B】RMM配信区分の幅を調整 START
    //D-Mailが非表示の場合は、幅を調節する
    if ($("#dmailDisplayFlgHidden").val() == "0") {
        $("#scNscCustomerEditingWindown #scNscCustomerEditingWindownBox .scNscCustomerEditingListItem6 td").css("width", "170px");
    }
    // 2012/01/26 TCS 安田 【SALES_1B】RMM配信区分の幅を調整 END

    //1ページ目表示設定
    setPopupCustomerEditPage("page1");

    //敬称
    page = $("#scNameTitlePopWindown .popWind .dataWind1 .ListBox01");
    page = page.clone(true);

    $("#scNscCustomerEditingWindown .dataWindNameTitle").append(page);

    // 2012/01/26 TCS 安田 【SALES_1B】敬称押下 (×ボタンの表示を防ぐため) START
    $("#nameTitle").focusin(function (e) {
        e.stopImmediatePropagation();
    });
    // 2012/01/26 TCS 安田 【SALES_1B】敬称押下 (×ボタンの表示を防ぐため) END

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

                //2012/03/08 TCS 山口 【SALES_2】性能改善 START
                //FingerScroll初期化
                $(".dataWindNameTitle .ListBox01 .scroll-inner").css({ "transform": "translate3d(0px, 0px, 0px)" });
                //2012/03/08 TCS 山口 【SALES_2】性能改善 END
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

    //--2013/11/29 TCS 各務 Aカード情報相互連携開発 START
    //個人法人項目
    page = $("#scPrivateFleetItemPopWindown .popWind .dataWind1 .ListBox01");
    page = page.clone(true);

    $("#scNscCustomerEditingWindown .dataWindPrivateFleetItem").append(page);

   // 2018/08/27 TCS 佐々木 TKM Next Gen e-CRB Project Application development Block B-1 START
    // 顧客組織
    page = $("#scCustOrgnzPopWindown .popWind .dataWind1 .ListBox01");
    page = page.clone(true);

    $("#scNscCustomerEditingWindown .dataWindCustOrgnz").append(page);

    // サブカテゴリ2
    page = $("#scCustSubCtgry2PopWindown .popWind .dataWind1 .ListBox01");
    page = page.clone(true);

    $("#scNscCustomerEditingWindown .dataWindCustSubCtgry2").append(page);

    /**
     * SuggestiveTextbox 用のコールバック関数
     * @param {string} result
     * @param {*} context
     */
    var updateCustOrgnzListCallback = function (result, context) {
        /**
         * .NET の System.String.Format や Python の str.format などのようなもの
         * @param {string} tmpl
         * @param {...string} args
         * @returns {string}
         */
        var format = function (tmpl) {
            /** @type {string[]} */
            var args = Array.prototype.slice.call(arguments, 1);
            return args.reduce(function (acc, curr) { return acc.replace('{}', curr) }, tmpl);
        };
        // ul を空にして、その中に li の集合を詰める
        var tmpl = '<li id="custOrgnzList{}" class="custOrgnzList"><p class="custOrgnzHidden">{}</p><p class="custOrgnzLabel">{}</p></li>';
        var list = parseOrEmptyArray(result);

        $('.custOrgnzListBoxSetIn')
            .empty()
            .append(list.map(function (obj) { return format(tmpl, obj.orgnzCd, obj.orgnzCd, obj.name) }).join(''))
            .find('.custOrgnzList:last-child').addClass('endRow');
    };


    /**
     * SuggestiveTextbox 用のコールバック関数
     * @param {string} result
     * @param {*} context
     */
    var SelectCustOrgnzListCallback = function (result, context) {
        /**
         * .NET の System.String.Format や Python の str.format などのようなもの
         * @param {string} tmpl
         * @param {...string} args
         * @returns {string}
         */
         
        var format = function (tmpl) {
            /** @type {string[]} */
            var args = Array.prototype.slice.call(arguments, 1);
            return args.reduce(function (acc, curr) {return acc.replace('{}', curr) }, tmpl);
        };
        // ul を空にして、その中に li の集合を詰める
        var tmpl = '<li id="custSubCtgry2List{}" class="custSubCtgry2List"><p class="custSubCtgry2Label">{}</p><p class="custSubCtgry2Hidden">{}</p><p class="custSubCtgry2PrivateFleetItemCd" style="display: none">{}</p><p class="custSubCtgry2CustOrgnzCd" style="display: none">{}</p></li>';
        
        var list = parseOrEmptyArray(result);

        $('.custSubCtgry2ListBoxSetIn')
            .empty()
            .append(list.map(function (obj) { return format(tmpl, obj.subcat2Cd, obj.subcat2Name, obj.subcat2Cd, obj.privatefleetItemCd, obj.orgnzCd) }).join(''))
            .find('.custOrgnzList:last-child').addClass('endRow');
    };

    
    /**
     * privateFleetItemCd, custOrgnzCd に応じてサブカテゴリ2の表示項目を切り替える
     * @param {string} privateFleetItemCd
     * @param {string} custOrgnzCd
     * @param {string} custSubCtgry2
     */
    var changeSubcat2ListItems = function (privateFleetItemCd, custOrgnzCd) {
            
        var $list = $('.custSubCtgry2ListBoxSetIn .custSubCtgry2List', $('#CustomerEditVisiblePanel')[0])
        $list.removeClass('endRow').filterVisibleItems().last().addClass('endRow');

        // custSubCtgry2Label と等しいサブカテゴリ2を選択済みにする
        var custSubCtgry2 = $('#custSubCtgry2Hidden').val();
        $list.filterVisibleItems()
            .filter(function () { return $('.custSubCtgry2Hidden', this).text() === custSubCtgry2 })
            .addClass('Selection');
    };
    // 2018/08/27 TCS 佐々木 TKM Next Gen e-CRB Project Application development Block B-1 END

    //個人法人項目リスト押下
    $("#scNscCustomerEditingWindown").find(".scNscCustomerEditingPrivateFleetItem").unbind('click').bind('click',function (e) {

        //個人/法人区分がチェックされているときのみ押下可能
        if ($("#kojinCheckBox").attr("checked") || $("#houjinCheckBox").attr("checked")) {
            if ($("#usePrivateFleetItemHidden").val() == "1") {

                //キャンセル時にもとの値に戻すため初期値を保存する
                $("#tempActvctgryidHidden").val("");
                $("#tempReasonidHidden").val("");
                $("#tempActvctgrynmHidden").val("");
                $("#tempReasonnmHidden").val("");

                //個人法人項目選択時処理
                $("#scNscCustomerEditingWindown .dataWindPrivateFleetItem .privateFleetItemList").unbind('click').bind('click',function (e) {

                    var cd = $(this).children(".privateFleetItemHidden").text();
                    var nm = $(this).children(".privateFleetItemLabel").text();

                    // 2018/08/27 TCS 佐々木 TKM Next Gen e-CRB Project Application development Block B-1 START
                    if (nm !== $("#privateFleetItem").val()) {
                        resetCustOrgnzName({alsoBelow: true});
                    }
                    // 2018/08/27 TCS 佐々木 TKM Next Gen e-CRB Project Application development Block B-1 END
                    $("#privateFleetItemHidden").val(cd);
                    $("#privateFleetItemTextHidden").val(nm);
                    $("#privateFleetItem").CustomTextBox("updateText", nm);
                    $("#scNscCustomerEditingWindown .dataWindPrivateFleetItem .privateFleetItemList").removeClass("Selection");
                    $(this).addClass("Selection");

                    setPopupCustomerEditPage("page1");

                    //FingerScroll初期化
                    $(".dataWindPrivateFleetItem .ListBox01 .scroll-inner").css({ "transform": "translate3d(0px, 0px, 0px)" });
                    // 敬称リストセット
                    changeNamelist("#scNscCustomerEditingWindown");
                    //名前のラベルを変更する
                    changeNameLabel();

                    // 2018/08/27 TCS 佐々木 TKM Next Gen e-CRB Project Application development Block B-1 START
                    var $selection = $('.privateFleetItemListBoxSetIn li.Selection');
                    var refType = $selection.children('.cstOrgnzNameRefType').text();
                    var nameInputType = $selection.children('.cstOrgnzNameInputType').text();
                    $('#cstOrgnzNameRefType').val(refType);
                    $('#custOrgnzNameInputTypeHidden').val(nameInputType);

                    callback.doCallback('UpdateCustOrgnzList', 'privateFleetItemCd=' + cd, updateCustOrgnzListCallback);
                    // 2018/08/27 TCS 佐々木 TKM Next Gen e-CRB Project Application development Block B-1 END
                });

                setPopupCustomerEditPage("page2", "privateFleetItemList");
            }
        }

    }).end()
    // 2018/08/27 TCS 佐々木 TKM Next Gen e-CRB Project Application development Block B-1 START
    .find('.scNscCustomerEditingCustOrgnz').unbind('click').bind('click',function () {

        // 個人法人項目が未選択のとき押下不可能
        if ($.trim($('#privateFleetItem').val()) === '') return true; // $.trim に null や undefined を渡すと空文字を返すためフォールバックは不要

        /**
         * 列挙可能なプロパティをi-CROP用の特殊パラメータ形式に変換する
         * @param {{}} obj
         * @returns {string}
         * @example
         * // returns a=A,b=B,c=C
         * createParams({a: 'A', b: 'B', c: 'C'})
         */
        var createParams = function (obj) {
            return Object.keys(obj)
                .map(function (key) { return key + '=' + encodeURIComponent(obj[key]) })
                .join(',');
        };

        var $parent = $('#CustomerEditVisiblePanel');
        var $inputs = $('#custOrgnzNameTextBox, #custOrgnzNameSuggestiveTextBox', $parent[0]);
        var privateFleetItemCd = $('#privateFleetItemHidden').val();
        var nameInputType = $('#custOrgnzNameInputTypeHidden').val();
        var custOrgnzName = $('#custOrgnz').val();
        var custOrgnzCd = $('#custOrgnzHidden').val();

        // 選択した個人法人項目に応じて表示項目を切り替える / '0': free field, '2': suggestive free field
        // '0' なら #custOrgnzNameTextBox と #custOrgnzNameTextBoxTable を表示
        // '2' なら #custOrgnzNameSuggestiveTextBox と #custOrgnzNameTextBoxTable を表示
        // それ以外('1') ならどちらも表示しない
        $parent
            .find('#custOrgnzNameTextBox').toggle(nameInputType === '0').end()
            .find('#custOrgnzNameSuggestiveTextBox').toggle(nameInputType === '2').end()
            .find('#custOrgnzNameTextBoxTable').toggle(['0', '2'].includes(nameInputType)).end();

        $inputs.filterVisibleItems().val(custOrgnzName);

        // 既存顧客で顧客組織名称が既に登録されていて、かつ顧客編集画面で一度もサブカテゴリ1を変更していない場合に
        // 顧客組織名称リストが不正に表示されてしまう問題へのワークアラウンド
        // ※ サーバーとの通信が必要で、タイミング次第で不正なリストが見えてしまう可能性があるため、実装方法は要検討
        var params = {
            custOrgnzNameHead: custOrgnzName,
            privateFleetItemCd: privateFleetItemCd,
        };
        callback.doCallback('UpdateCustOrgnzList', createParams(params), function (result, context) {
            updateCustOrgnzListCallback(result, context);

            // custOrgnzName に一致するリスト項目に .Selection を付与する
            if (custOrgnzName !== '') {
                $('.custOrgnzList', '.custOrgnzListBoxSetIn')
                    .filter(function () { return $('.custOrgnzHidden', this).text() === custOrgnzCd })
                    .addClass('Selection');
            }
        });

        /**
         * 確定処理
         * @param {string} name CST_ORGNZ_NAME
         */
        var confirmItem = function (name,orgnzCd) {
            var params = {
                privateFleetItemCd: privateFleetItemCd,
                custOrgnzName: name,
                custOrgnzCd: orgnzCd,
            };

            callback.doCallback('ConfirmCustOrgnz', createParams(params), function (result, context) {
                
                SelectCustOrgnzListCallback(result, context);
                
                var data = parseOrEmptyArray(result);
                var datum = data[0];
                $('#custOrgnzHidden').val(orgnzCd);
                $('#custOrgnzInputTypeHidden').val(trim(orgnzCd) !== '' ? '1' : '2'); // 1: マスタから選択, 2: 手入力
                $('#custOrgnz').CustomTextBox('updateText', name);

                // 2018/12/18 TCS 舩橋 TKM-UAT課題No.89 Subcategory2が1件に絞られた場合のみ、自動反映する START
                var cstReferenceType = (trim(orgnzCd) !== '' ? '1' : '2');
                if ('' !== name && ('1' == nameInputType|| '2' == nameInputType) && '1' == cstReferenceType && 1 == data.length)
                {
                    $('#custSubCtgry2Hidden').val(datum.subcat2Cd);
                    $('#custSubCtgry2').CustomTextBox('updateText', datum.subcat2Name);
                }
                // 2018/12/18 TCS 舩橋 TKM-UAT課題No.89 Subcategory2が1件に絞られた場合のみ、自動反映する END
            });
            if (name !== $('#custOrgnz').val()) {
                resetCustSubCtgry2();
            } 
            $('#custOrgnz').CustomTextBox('updateText', name);
            // 2019/04/08 TS 舩橋 POST-UAT3110 電話番号検索すると、サブカテゴリ２が消える START
            $('#custOrgnzNameHidden').val(name);
            // 2019/04/08 TS 舩橋 POST-UAT3110 電話番号検索すると、サブカテゴリ２が消える END

            setPopupCustomerEditPage('page1');

            //FingerScroll初期化
            $('.dataWindCustOrgnz .ListBox01 .scroll-inner').css('-webkit-transform', 'translate3d(0px, 0px, 0px)');
        };

        // SuggestiveTextBox 用インクリメンタルサーチのディレイタイマー
        var timeout = null;

        // キャンセルボタンクリック時に確定ボタン削除
        var detachButton = function () {
            $confirmButton.remove();
        };
        $('#cancelButtonLabel').unbind('click', detachButton).bind('click', detachButton);

        // TextBox / SuggestiveTextBox 用の確定ボタン
        var $confirmButton = $('#scNscCustomerEditingCompletion').clone(true)
            .removeAttr('id')
            .toggle(['0', '2'].includes(nameInputType))
            .filterVisibleItems()
            .click(function (ev, name) {
                if (typeof timeout === 'number') clearTimeout(timeout);

                //2018-10-08 No.6 ADD START
                if(nameInputType === '0'){
                  name = name || $('#custOrgnzNameTextBox', $parent[0]).val();
                }else if(nameInputType === '2'){
                  name = name || $('#custOrgnzNameSuggestiveTextBox', $parent[0]).val();
                }

                while(name.substr(0,1) === ','){
                  name = name.substr(1);
                }
                //2018-10-08 No.6 ADD END
                
                //名前が完全一致する組織があればその組織IDを取得
                var orgnzCd
                orgnzCd = '';
                $(".custOrgnzLabel").each(function () {
                    if($(this).text() == name){
                        orgnzCd = $(this).parent().find(".custOrgnzHidden").text()
                        return false;
                    }
                });
                
                confirmItem(name,orgnzCd);
                $confirmButton.remove();
            
                // '2': SuggestiveTextBox
                if (nameInputType === '2') {
                    // 次回顧客組織名称リスト表示時のためにリスト更新
                    var params = {
                        custOrgnzNameHead: name || '',
                        privateFleetItemCd: privateFleetItemCd
                    };
                    callback.doCallback('UpdateCustOrgnzList', createParams(params), updateCustOrgnzListCallback);
                }
            })
            .appendTo('#scNscCustomerEditingWindownBox > .scNscCustomerEditingHadder');

        // SuggestiveTextBox の入力イベントを捕捉
        $('#custOrgnzNameSuggestiveTextBox', $parent[0])
            .filterVisibleItems()
            .unbind('input').bind('input', function (ev) {
                var delay = 200; // 最後に入力してから 200ms 後に発火

                if (typeof timeout === 'number') clearTimeout(timeout);

                timeout = setTimeout(function (node) {
                    var params = {
                        custOrgnzNameHead: node.value,
                        privateFleetItemCd: privateFleetItemCd
                    };

                    //FingerScroll初期化
                    $('.dataWindCustOrgnz .ListBox01 .scroll-inner').css('-webkit-transform', 'translate3d(0px, 0px, 0px)');

                    callback.doCallback('UpdateCustOrgnzList', createParams(params), updateCustOrgnzListCallback);
                }, delay, ev.target);
            });

        // 顧客組織選択時
        var $custOrgnzListItems = $('.custOrgnzListBoxSetIn', $parent[0])
            .undelegate('click').delegate('.custOrgnzList', 'click', function (ev) {
                var e = ev.currentTarget;
                confirmItem($('.custOrgnzLabel', e).text(),$('.custOrgnzHidden', e).text());
                $custOrgnzListItems.find('.custOrgnzList').removeClass('Selection');
                $(e).addClass('Selection');

                // '2': SuggestiveTextBox
                if (nameInputType === '2') {
                    var custOrgnzName = $('.custOrgnzLabel', e).text();
                    $('#custOrgnzNameSuggestiveTextBox', $parent[0]).val(custOrgnzName);
                    $confirmButton.trigger('click', [custOrgnzName]);
                }
            });

        setPopupCustomerEditPage('page2', 'custOrgnzList');
    }).end()
    .find('.scNscCustomerEditingCustSubCtgry2').click(function () {
        
        // 顧客組織が未確定のとき押下不可能
        if ($('#custOrgnz').val() === '') return true;

        var privateFleetItemCd = $('#privateFleetItemHidden').val();
        var custOrgnzCd = $('#custOrgnzHidden').val();
        changeSubcat2ListItems(privateFleetItemCd, custOrgnzCd);

        // サブカテゴリ2選択時処理
        var $subCtgry2ListItems = $('.dataWindCustSubCtgry2', $('#scNscCustomerEditingWindown')[0])
            .undelegate('click').delegate('.custSubCtgry2List', 'click', function (ev) {
                var e = ev.currentTarget;
                $('#custSubCtgry2Hidden').val($('.custSubCtgry2Hidden', e).text());
                $('#custSubCtgry2').CustomTextBox('updateText', $('.custSubCtgry2Label', e).text());
                $subCtgry2ListItems.find('.custSubCtgry2List').removeClass('Selection');
                $(e).addClass('Selection');

                setPopupCustomerEditPage('page1');

                //FingerScroll初期化
                $('.dataWindCustSubCtgry2 .ListBox01 .scroll-inner').css('-webkit-transform', 'translate3d(0px, 0px, 0px)');
            });

        setPopupCustomerEditPage('page2', 'custSubCtgry2List');
    });
    // 2018/08/27 TCS 佐々木 TKM Next Gen e-CRB Project Application development Block B-1 END

    //個人法人項目リスト初期チェックセット
    changePrivateFleetItemlist("#scNscCustomerEditingWindown");

    //州リスト
    page = $("#scStatePopWindown .popWind .dataWind1 .ListBox01");
    page = page.clone(true);

    $("#scNscCustomerEditingWindown .dataWindState").append(page);

    //地域リスト
    page = $("#scDistrictPopWindown .popWind .dataWind1 .ListBox01");
    page = page.clone(true);

    $("#scNscCustomerEditingWindown .dataWindDistrict").append(page);

    //市リスト
    page = $("#scCityPopWindown .popWind .dataWind1 .ListBox01");
    page = page.clone(true);

    $("#scNscCustomerEditingWindown .dataWindCity").append(page);

    //地区リスト
    page = $("#scLocationPopWindown .popWind .dataWind1 .ListBox01");
    page = page.clone(true);

    $("#scNscCustomerEditingWindown .dataWindLocation").append(page);

    //州リスト押下
    $("#scNscCustomerEditingWindown").find(".scNscCustomerEditingState").click(function (e) {

        if ($("#useStateHidden").val() == "1") {
            makeStateListEvent();
            setPopupCustomerEditPage("page2", "stateList");
        }
    });

    //地域リスト押下
    $("#scNscCustomerEditingWindown").find(".scNscCustomerEditingDistrict").click(function (e) {

        var $districtList = $("#scNscCustomerEditingWindown .dataWindDistrict ul.districtListBoxSetIn").children();
        if ($districtList.length > 0) {
            //地域リストの件数が1件以上あるときのみ押下可能
            if ($("#useDistrictHidden").val() == "1") {
                makeStateListEvent();
                makeDistrictListEvent();
                setPopupCustomerEditPage("page3", "districtList");
            }
        }
    });

    //市リスト押下
    $("#scNscCustomerEditingWindown").find(".scNscCustomerEditingCity").click(function (e) {

        var $cityList = $("#scNscCustomerEditingWindown .dataWindCity ul.cityListBoxSetIn").children();
        if ($cityList.length > 0) {
            //市リストの件数が1件以上あるときのみ押下可能
            if ($("#useCityHidden").val() == "1") {
                makeStateListEvent();
                makeDistrictListEvent();
                makeCityListEvent();
                setPopupCustomerEditPage("page4");
            }
        }
    });

    //地区リスト押下
    $("#scNscCustomerEditingWindown").find(".scNscCustomerEditingLocation").click(function (e) {

        var $locationList = $("#scNscCustomerEditingWindown .dataWindLocation ul.locationListBoxSetIn").children();
        if ($locationList.length > 0) {
            //地区リストの件数が1件以上あるときのみ押下可能
            if ($("#useLocationHidden").val() == "1") {
                makeStateListEvent();
                makeDistrictListEvent();
                makeCityListEvent();
                makeLocationListEvent();
                setPopupCustomerEditPage("page5");
            }
        }
    });
    //--2013/11/29 TCS 各務 Aカード情報相互連携開発 END

    // 敬称リスト初期チェックセット
    changeNamelist("#scNscCustomerEditingWindown");

    // 活動区分リスト初期チェックセット
    actvctgrylist("#scNscCustomerEditingWindown");

    // 断念理由リスト初期チェックセット
    reasonidlist("#scNscCustomerEditingWindown");

    //--2013/11/29 TCS 各務 Aカード情報相互連携開発 START
    // 住所(州～地区)リスト初期チェックセット
    addresslist("#scNscCustomerEditingWindown");
    //--2013/11/29 TCS 各務 Aカード情報相互連携開発 END

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

               //2018/11/22 TCS 三浦 活動区分「3,4」選択時、理由を選択できるようにする(TR-SLT-TMT-20180611-001) START
                if (cd == "2" || cd == "3" || cd == "4") {    //2,3,4の場合、情報不備詳細へ遷移する
                    changeReasonListItems();
                    $("#reasonTitleLabel").text($("#actvctgryNameHidden").val());
                    //2018/11/22 TCS 三浦 活動区分「3,4」選択時、理由を選択できるようにする(TR-SLT-TMT-20180611-001) END

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
                    
                    //--2013/11/29 TCS 各務 Aカード情報相互連携開発 START
                    setPopupCustomerEditPage("page3", "reasonList");
                    //--2013/11/29 TCS 各務 Aカード情報相互連携開発 END

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

//2018/11/22 TCS 三浦 活動区分「3,4」選択時、理由を選択できるようにする(TR-SLT-TMT-20180611-001) START
function changeReasonListItems() {
    var $list = $('.reasonListBoxSetIn .reasonlist', $('#CustomerCarEditVisiblePanel')[0])
    $list.each(function () {

        var $actvctgryid = $('.actvctgryidHidden', this);
        $(this).toggle($actvctgryid.text() === $("#actvctgryidHidden").val());
    });
    $list.removeClass('endRow').filter(function () { return $(this).css('display') !== 'none' }).last().addClass('endRow');
};
//2018/11/22 TCS 三浦 活動区分「3,4」選択時、理由を選択できるようにする(TR-SLT-TMT-20180611-001) END

//--2013/11/29 TCS 各務 Aカード情報相互連携開発 START
// 州リストイベントハンドラ登録　----------------------------------------------
function makeStateListEvent() {
    //州選択時処理
    $("#scNscCustomerEditingWindown .dataWindState .stateList").click(function (e) {

        e.stopImmediatePropagation();
        var cd = $(this).children(".stateHidden").text();
        var nm = $(this).children(".stateLabel").text();
        $("#stateHidden").val(cd);
        $("#stateTextHidden").val(nm);
        $("#addressState").CustomTextBox("updateText", nm);
        $("#scNscCustomerEditingWindown .dataWindState .stateList").removeClass("Selection");
        $(this).addClass("Selection");

        //州を変更したとき、その下位をすべて消す
        $("#addressDistrict").CustomTextBox("updateText", "");
        $("#addressCity").CustomTextBox("updateText", "");
        $("#addressLocation").CustomTextBox("updateText", "");
        $("#scNscCustomerEditingWindown .dataWindDistrict .districtListBoxSetIn").empty();
        $("#scNscCustomerEditingWindown .dataWindCity .cityListBoxSetIn").empty();
        $("#scNscCustomerEditingWindown .dataWindLocation .locationListBoxSetIn").empty();
        $("#districtHidden").val("");
        $("#districtTextHidden").val("");
        $("#cityHidden").val("");
        $("#cityTextHidden").val("");
        $("#locationHidden").val("");
        $("#locationTextHidden").val("");

        //FingerScroll初期化
        $(".dataWindState .ListBox01 .scroll-inner").css({ "transform": "translate3d(0px, 0px, 0px)" });

        if ($("#inputSettingHidden20").val() == "1" || $("#inputSettingHidden20").val() == "2") {    //地域リストが表示の場合のみ地域リストへ遷移する

            var prms = "";
            if (cd != "") {
                $("#serverProcessFlgHidden").val("1");              //サーバーサイド処理フラグ
                SC3080201.startServerCallback();

                prms = prms + encodeURIComponent(cd);           //州コード
                callback.doCallback("GetDistrict", prms, function (result, context) {

                    //地域リスト作成
                    makeDistrictList(result);
                    makeDistrictListEvent();

                    $("#serverProcessFlgHidden").val("");           //サーバーサイド処理フラグ
                    SC3080201.endServerCallback();

                    setPopupCustomerEditPage("page3", "districtList");

                });
            }
        } else {
            setPopupCustomerEditPage("page1");
        }
    });
}

// 地域リスト作成　------------------------------------------------------------
function makeDistrictList(districtStr) {

    var districtArray = districtStr.split(",");
    var innerHtml;

    $("#scNscCustomerEditingWindown .dataWindDistrict .districtListBoxSetIn").empty();

    if (districtArray[1] == "1") {
        for (i = 3; i < districtArray.length; i += 2) {
            innerHtml = "";
            innerHtml += "<li id=\"districtList" + SC3080201HTMLDecode(districtArray[i - 1]) + "\" class = \"districtList\">";
            innerHtml += "<p class=\"districtLabel\">" + SC3080201HTMLDecode(districtArray[i]) + "</p>";
            innerHtml += "<p class=\"districtHidden\">" + SC3080201HTMLDecode(districtArray[i - 1]) + "</p>";
            innerHtml += "</li>";
            $("#scNscCustomerEditingWindown .dataWindDistrict .districtListBoxSetIn").append(innerHtml);
        }
    }
}
// 地域リストイベントハンドラ登録　--------------------------------------------
function makeDistrictListEvent() {
    //地域選択
    $("#scNscCustomerEditingWindown .dataWindDistrict .districtList").click(function (e) {

        e.stopImmediatePropagation();

        var cd = $(this).children(".districtHidden").text();
        var nm = $(this).children(".districtLabel").text();
        $("#districtHidden").val(cd);
        $("#districtTextHidden").val(nm);
        $("#addressDistrict").CustomTextBox("updateText", nm);
        $("#scNscCustomerEditingWindown .dataWindDistrict .districtList").removeClass("Selection");
        $(this).addClass("Selection");

        //地域を変更したとき、その下位をすべて消す
        $("#addressCity").CustomTextBox("updateText", "");
        $("#addressLocation").CustomTextBox("updateText", "");
        $("#scNscCustomerEditingWindown .dataWindCity .cityListBoxSetIn").empty();
        $("#scNscCustomerEditingWindown .dataWindLocation .locationListBoxSetIn").empty();
        $("#cityHidden").val("");
        $("#cityTextHidden").val("");
        $("#locationHidden").val("");
        $("#locationTextHidden").val("");

        //FingerScroll初期化
        $(".dataWindDistrict .ListBox01 .scroll-inner").css({ "transform": "translate3d(0px, 0px, 0px)" });

        if ($("#inputSettingHidden21").val() == "1" || $("#inputSettingHidden21").val() == "2") {    //市リストが表示の場合のみ市リストへ遷移する

            var prms = "";
            if (cd != "") {
                $("#serverProcessFlgHidden").val("1");              //サーバーサイド処理フラグ
                SC3080201.startServerCallback();

                var state = $("#stateHidden").val();
                prms = prms + encodeURIComponent(state) + "," + encodeURIComponent(cd);           //州コード＋地域コード
                callback.doCallback("GetCity", prms, function (result, context) {

                    //市リスト作成
                    makeCityList(result);
                    makeCityListEvent();

                    $("#serverProcessFlgHidden").val("");           //サーバーサイド処理フラグ
                    SC3080201.endServerCallback();

                    setPopupCustomerEditPage("page4");

                });
            }
        } else {
            setPopupCustomerEditPage("page1");
        }

    });
}

// 市リスト作成　------------------------------------------------------------
function makeCityList(cityStr) {

    var cityArray = cityStr.split(",");
    var innerHtml;

    $("#scNscCustomerEditingWindown .dataWindCity .cityListBoxSetIn").empty();

    if (cityArray[1] == "1") {
        for (i = 3; i < cityArray.length; i += 2) {
            innerHtml = "";
            innerHtml += "<li id=\"cityList" + SC3080201HTMLDecode(cityArray[i - 1]) + "\" class = \"cityList\">";
            innerHtml += "<p class=\"cityLabel\">" + SC3080201HTMLDecode(cityArray[i]) + "</p>";
            innerHtml += "<p class=\"cityHidden\">" + SC3080201HTMLDecode(cityArray[i - 1]) + "</p>";
            innerHtml += "</li>";
            $("#scNscCustomerEditingWindown .dataWindCity .cityListBoxSetIn").append(innerHtml);
        }
    }
}
// 市リストイベントハンドラ登録　--------------------------------------------
function makeCityListEvent() {
    //市選択
    $("#scNscCustomerEditingWindown .dataWindCity .cityList").click(function (e) {

        e.stopImmediatePropagation();

        var cd = $(this).children(".cityHidden").text();
        var nm = $(this).children(".cityLabel").text();
        $("#cityHidden").val(cd);
        $("#cityTextHidden").val(nm);
        $("#addressCity").CustomTextBox("updateText", nm);
        $("#scNscCustomerEditingWindown .dataWindCity .cityList").removeClass("Selection");
        $(this).addClass("Selection");

        //市を変更したとき、その下位をすべて消す
        $("#addressLocation").CustomTextBox("updateText", "");
        $("#scNscCustomerEditingWindown .dataWindLocation .locationListBoxSetIn").empty();
        $("#locationHidden").val("");
        $("#locationTextHidden").val("");

        //FingerScroll初期化
        $(".dataWindCity .ListBox01 .scroll-inner").css({ "transform": "translate3d(0px, 0px, 0px)" });

        if ($("#inputSettingHidden22").val() == "1" || $("#inputSettingHidden22").val() == "2") {    //地区リストが表示の場合のみ地区リストへ遷移する

            var prms = "";
            if (cd != "") {
                $("#serverProcessFlgHidden").val("1");              //サーバーサイド処理フラグ
                SC3080201.startServerCallback();

                var state = $("#stateHidden").val();
                var district = $("#districtHidden").val();
                prms = prms + encodeURIComponent(state) + "," + encodeURIComponent(district) + "," + encodeURIComponent(cd);  //州コード＋地域コード＋市コード
                callback.doCallback("GetLocation", prms, function (result, context) {

                    //地区リスト作成
                    makeLocationList(result);
                    makeLocationListEvent();

                    $("#serverProcessFlgHidden").val("");           //サーバーサイド処理フラグ
                    SC3080201.endServerCallback();

                    setPopupCustomerEditPage("page5");

                });
            }
        } else {
            setPopupCustomerEditPage("page1");
        }

    });
}

// 地区リスト作成　------------------------------------------------------------
// 2017/11/16 TCS 河原 TKM独自機能開発 START
function makeLocationList(locationStr) {
    // locationArray[]
    //  [0]：コールバック関数名（"GetLocation"）
    //  [1]：処理結果（"1"：OK、"9"：エラー）
    //  [2]：* TB_M_LOCATION.LOCATION_CD
    //  [3]：* TB_M_LOCATION.LOCATION_NAME
    //  [4]：* TB_M_LOCATION.ZIP_CD
    //  [n]：以下、取得レコード分だけ * を繰り返す
    var locationArray = locationStr.split(",");
    var innerHtml;

    $("#scNscCustomerEditingWindown .dataWindLocation .locationListBoxSetIn").empty();

    if (locationArray[1] == "1") {
        for (i = 2; i < locationArray.length; i += 3) {
            innerHtml = "";
            innerHtml += "<li id=\"locationList" + SC3080201HTMLDecode(locationArray[i]) + "\" class = \"locationList\">"; //LOCATION_CD
            innerHtml += "<p class=\"locationLabel\">" + SC3080201HTMLDecode(locationArray[i + 1]) + "</p>";     //LOCATION_NAME
            innerHtml += "<p class=\"locationHidden\">" + SC3080201HTMLDecode(locationArray[i]) + "</p>";        //LOCATION_CD
            innerHtml += "<p class=\"locationZipHidden\">" + SC3080201HTMLDecode(locationArray[i + 2]) + "</p>"; //ZIP_CD
            innerHtml += "</li>";
            $("#scNscCustomerEditingWindown .dataWindLocation .locationListBoxSetIn").append(innerHtml);
        }
    }
}

// 地区リストイベントハンドラ登録　--------------------------------------------
function makeLocationListEvent() {
    //地区選択
    $("#scNscCustomerEditingWindown .dataWindLocation .locationList").click(function (e) {

        e.stopImmediatePropagation();

        var cd = $(this).children(".locationHidden").text();
        var nm = $(this).children(".locationLabel").text();
        $("#locationHidden").val(cd);
        $("#locationTextHidden").val(nm);
        $("#addressLocation").CustomTextBox("updateText", nm);
        $("#scNscCustomerEditingWindown .dataWindLocation .locationList").removeClass("Selection");
        $(this).addClass("Selection");

        //FingerScroll初期化
        $(".dataWindLocation .ListBox01 .scroll-inner").css({ "transform": "translate3d(0px, 0px, 0px)" });
        setPopupCustomerEditPage("page1");

        //郵便番号自動設定
        if ($("#CleansingMode").val() == "1") {
            $("#zipcodeTextBox").val($(this).children(".locationZipHidden").text());
        }

    });
}
// 2017/11/16 TCS 河原 TKM独自機能開発 END
//--2013/11/29 TCS 各務 Aカード情報相互連携開発 END

// 顧客編集表示 (ページ指定)　-------------------------------------------------
function setPopupCustomerEditPage(page, subId) {

    //ページ番号セット
    $("#custPageHidden").val(page);

    //--2013/11/29 TCS 各務 Aカード情報相互連携開発 START
    $("#scNscCustomerEditingWindown #scNscCustomerEditingWindownBox .scNscCustomerEditingListBox").removeClass("page1 page2 page3 page4 page5").addClass(page);
    //--2013/11/29 TCS 各務 Aカード情報相互連携開発 END
    
  if (page == "page2") {
        // 2018/08/27 TCS 佐々木 TKM Next Gen e-CRB Project Application development Block B-1 START
        $("#scNscCustomerEditingWindown")
        // 敬称リスト
        .find(".dataWindNameTitle").toggle(subId == "nameTitleList").end()
        // 活動区分リスト
        .find(".dataWindActvctgry").toggle(subId == "actvctgryList").end()
        // 個人法人項目リスト
        .find(".dataWindPrivateFleetItem").toggle(subId == "privateFleetItemList").end()
        // 顧客組織リスト
        .find(".dataWindCustOrgnz").toggle(subId == "custOrgnzList").end()
        // サブカテゴリ2リスト
        .find(".dataWindCustSubCtgry2").toggle(subId == "custSubCtgry2List").end()
        // 州リスト
        .find(".dataWindState").toggle(subId == "stateList").end();
        // 2018/08/27 TCS 佐々木 TKM Next Gen e-CRB Project Application development Block B-1 END
    }
    //--2013/11/29 TCS 各務 Aカード情報相互連携開発 START
    if (page == "page3") {
        if (subId == "reasonList") {
            //情報不備詳細リスト
            $("#scNscCustomerEditingWindown .dataWindReason").css("display", "block");
            $("#scNscCustomerEditingWindown .dataWindDistrict").css("display", "none");
        } else if (subId = "districtList") {
            //地域リスト
            $("#scNscCustomerEditingWindown .dataWindReason").css("display", "none");
            $("#scNscCustomerEditingWindown .dataWindDistrict").css("display", "block");
        }
    }
    //--2013/11/29 TCS 各務 Aカード情報相互連携開発 END
    
    //タイトルを変更する
    var strCancelLable = "";
    var strTitleLable = "";
    var strCompletionLable = "";

    // 2012/01/26 TCS 安田 【SALES_1B】顧客編集→車両編集遷移 START
    $(".scNscCustomerEditingCompletionArrow").hide();                          //車両登録の右矢印
    // 2012/01/26 TCS 安田 【SALES_1B】顧客編集→車両編集遷移 END

    if (page == "page1") {

        // 2012/01/26 TCS 安田 【SALES_1B】顧客編集→車両編集遷移 START
        $("#scNscCustomerEditingCompletion").show();                           //右ボタンを表示
        $("#scNscCustomerEditingCompletion").removeClass("scNscCustomerEditingCompletionButton scNscCustomerEditingCompletionButton2 Arrow");                     //右ボタンを表示

        if (($("#nextVehicleFlg").val() == "1")) {
            $(".scNscCustomerEditingCompletionArrow").show();
            $("#scNscCustomerEditingCompletion").addClass("scNscCustomerEditingCompletionButton2");

            strCompletionLable = $("#nextVehicleLabel").text();                //車両登録
        } else {
            $("#scNscCustomerEditingCompletion").addClass("scNscCustomerEditingCompletionButton");

            strCompletionLable = $("#completionLabel").text();                 //登録
        }
        // 2012/01/26 TCS 安田 【SALES_1B】顧客編集→車両編集遷移 END

        strCancelLable = $("#cancelLabel").text(); //キャンセル

        //2017/11/16 TCS 河原 TKM独自機能開発 START
        if ($("#CleansingModeFlg").val() == "1" || $("#CleansingMode").val() == "1") {
            strTitleLable = $("#dataCleansingLabel").text();
        } else if (($("#editModeHidden").val() == "0")) {
            //追加時
            strTitleLable = $("#createCustomerLabel").text();
        } else {
            //更新時
            strTitleLable = $("#editCustomerLabel").text();
        }
        //2017/11/16 TCS 河原 TKM独自機能開発 END
    } else {

        // 2012/01/26 TCS 安田 【SALES_1B】顧客編集→車両編集遷移 START
        $("#scNscCustomerEditingCompletion").hide();                          //右ボタンは非表示
        // 2012/01/26 TCS 安田 【SALES_1B】顧客編集→車両編集遷移 END

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
                //--2013/11/29 TCS 各務 Aカード情報相互連携開発 START
                //} else {
            } else if (subId == "actvctgryList") {
                //--2013/11/29 TCS 各務 Aカード情報相互連携開発 END
                strTitleLable = $("#actvctgryTitleLabel").text();
                //--2013/11/29 TCS 各務 Aカード情報相互連携開発 START
            } else if (subId == "privateFleetItemList") {
                strTitleLable = $("#privateFleetItemLabel").text();
            // 2018/08/27 TCS 佐々木 TKM Next Gen e-CRB Project Application development Block B-1 START
            } else if (subId == "custOrgnzList") {
                strTitleLable = $("#custOrgnzLabel").text();
            } else if (subId == "custSubCtgry2List") {
                strTitleLable = $("#custSubCtgry2Label").text();
            // 2018/08/27 TCS 佐々木 TKM Next Gen e-CRB Project Application development Block B-1 END
            } else if (subId == "stateList") {
                strTitleLable = $("#stateLabel").text();
                //--2013/11/29 TCS 各務 Aカード情報相互連携開発 END
            }

            //--2013/11/29 TCS 各務 Aカード情報相互連携開発 START
        } else if (page == "page3") {
            if (subId == "reasonList") {
                strCancelLable = $("#reasonBackLabel").text();
                strTitleLable = $("#reasonTitleLabel").text();
            } else if (subId == "districtList") {
                strCancelLable = $("#districtLabel2").text();
                strTitleLable = $("#districtLabel").text();
            }
        } else if (page == "page4") {
            strCancelLable = $("#cityLabel2").text();
            strTitleLable = $("#cityLabel").text();
        } else if (page == "page5") {
            strCancelLable = $("#locationLabel2").text();
            strTitleLable = $("#locationLabel").text();
        }
        //--2013/11/29 TCS 各務 Aカード情報相互連携開発 END
    }

    $("#cancelButtonLabel").text(strCancelLable);           //キャンセルボタン
    $("#completionButtonLabel").text(strCompletionLable);   //登録ボタン
    $("#customerTitleLabel").text(strTitleLable);           //タイトル

}

// 顧客編集　敬称リストの切り替え　-------------------------------------------------
function changeNamelist(parentTag) {

    $(parentTag + " " + ".nameTitlelist").removeClass("Selection");

    var nameTitleCd = $("#nameTitleHidden").val();

    var $namelist = $(parentTag + " " + ".dataWindNameTitle ul.nscListBoxSetIn").children();

    var endrow = -1;

    //2014/07/15 TCS 市川 複数の個人法人項目コードにて単一の敬称コードを表示させる(TMT-UAT-BTS-80) START
    var doublingCd = ""; 
    var selectedCount = 0;
    //2014/07/15 TCS 市川 複数の個人法人項目コードにて単一の敬称コードを表示させる(TMT-UAT-BTS-80) END

    //敬称リストの表示切替
    //表示区分＝(0:常に表示  1: 個人のみ表示  2: 法人のみ表示)
    for (i = 0; i < $namelist.length; i++) {
        var flg = true;     //true:表示する／false:表示しない
        var namecd = $(parentTag + " " + "#" + $namelist[i].id + "").children(".namecdHidden").text();    //敬称コード
        var dispflg = $(parentTag + " " + "#" + $namelist[i].id + "").children(".dispHidden").text();     //表示区分
        //--2013/11/29 TCS 各務 Aカード情報相互連携開発 START
        var privateFleet = $(parentTag + " " + "#" + $namelist[i].id + "").children(".privateFleetHidden").text();     //個人法人項目コード
        var privateFleetSel = $("#privateFleetItemHidden").val(); //個人法人項目コード(選択中)
        //--2013/11/29 TCS 各務 Aカード情報相互連携開発 END
        //2014/07/15 TCS 市川 複数の個人法人項目コードにて単一の敬称コードを表示させる(TMT-UAT-BTS-80) START
        var nameTitle = $(parentTag + " " + "#" + $namelist[i].id + "").children(".nameTitleLabel").text();
        var doublingFlg = false;
        //2014/07/15 TCS 市川 複数の個人法人項目コードにて単一の敬称コードを表示させる(TMT-UAT-BTS-80) END

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

        //--2013/11/29 TCS 各務 Aカード情報相互連携開発 START
        //個人法人項目コードチェック(ラベル・敬称設定フラグONの場合のみ)
        if ($("#labelNametitleSettingHidden").val() == "1") {

            //2014/07/15 TCS 市川 複数の個人法人項目コードにて単一の敬称コードを表示させる(TMT-UAT-BTS-80) START
            if (privateFleetSel != "") {
                if (privateFleet != "") {
                    if (privateFleet != privateFleetSel) {
                        flg = false;
                    }
                } else {
                    flg = false;
                }
            }
            //2014/07/15 TCS 市川 複数の個人法人項目コードにて単一の敬称コードを表示させる(TMT-UAT-BTS-80) END
        }
        //--2013/11/29 TCS 各務 Aカード情報相互連携開発 END
        
        //2014/07/15 TCS 市川 複数の個人法人項目コードにて単一の敬称コードを表示させる(TMT-UAT-BTS-80) START
        //共通コード化のため、重複データを検出
        if (doublingCd != namecd) {
            //表示対象のデータのみ重複のカウント対象とする。
            if(flg) doublingCd = namecd;
        } else {            
            flg = false;
        }
        //2014/07/15 TCS 市川 複数の個人法人項目コードにて単一の敬称コードを表示させる(TMT-UAT-BTS-80) END

        if (flg) {
            //表示する
            $(parentTag + " " + "#" + $namelist[i].id + "").css("display", "list-item");
            //選択状態にする
            if (namecd == nameTitleCd) {
                $(parentTag + " " + "#" + $namelist[i].id + "").addClass("Selection");
                //2014/07/15 TCS 市川 複数の個人法人項目コードにて単一の敬称コードを表示させる(TMT-UAT-BTS-80) START
                selectedCount += 1;
                //2014/07/15 TCS 市川 複数の個人法人項目コードにて単一の敬称コードを表示させる(TMT-UAT-BTS-80) END
            }
            endrow = i;
        } else {
            //表示しない
            $(parentTag + " " + "#" + $namelist[i].id + "").css("display", "none");
            //選択状態から外す
            if (namecd == nameTitleCd) {
                $(parentTag + " " + "#" + $namelist[i].id + "").removeAttr("Selection");
                //2014/07/15 TCS 市川 複数の個人法人項目コードにて単一の敬称コードを表示させる(TMT-UAT-BTS-80) DELETE
            }
        }
    }

    //2014/07/15 TCS 市川 複数の個人法人項目コードにて単一の敬称コードを表示させる(TMT-UAT-BTS-80) START
    //重複データでない場合のみクリア処理する
    if (selectedCount == 0) { 
        $("#nameTitleHidden").val("");
        //--2013/11/29 TCS 各務 Aカード情報相互連携開発 START
        $("#nameTitleTextHidden").val("");
        //--2013/11/29 TCS 各務 Aカード情報相互連携開発 END
        $("#nameTitle").CustomTextBox("updateText", "");
    }
    //2014/07/15 TCS 市川 複数の個人法人項目コードにて単一の敬称コードを表示させる(TMT-UAT-BTS-80) END

    if (endrow >= 0) {
        //endRowを一度削除
        $("#scNscCustomerEditingWindown .dataWind2 .nscListBoxSetIn li").removeClass("endRow");
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

// 顧客編集　個人法人項目リストの切り替え　-------------------------------------------------
function changePrivateFleetItemlist(parentTag) {

    $(parentTag + " " + ".privateFleetItemList").removeClass("Selection");

    var privateFleetItemCd = $("#privateFleetItemHidden").val();

    var $itemlist = $(parentTag + " " + ".dataWindPrivateFleetItem ul.privateFleetItemListBoxSetIn").children();

    var endrow = -1;

    //個人法人項目リストの表示切替
    //表示区分＝(0:常に表示  1: 個人のみ表示  2: 法人のみ表示)
    for (i = 0; i < $itemlist.length; i++) {
        var flg = false;     //true:表示する／false:表示しない
        var itemcd = $(parentTag + " " + "#" + $itemlist[i].id + "").children(".privateFleetItemHidden").text();    //個人法人項目コード
        var fleetflg = $(parentTag + " " + "#" + $itemlist[i].id + "").children(".fleetHidden").text();     //法人フラグ


        //個人チェック
        if ($("#kojinCheckBox").attr("checked")) {
            if (fleetflg == "0") {
                flg = true;
            }
        }

        //法人チェック
        if ($("#houjinCheckBox").attr("checked")) {
            if (fleetflg == "1") {
                flg = true;
            }
        }

        if (flg == true) {
            //表示する
            $(parentTag + " " + "#" + $itemlist[i].id + "").css("display", "list-item");

            //選択状態にする
            if (itemcd == privateFleetItemCd) {
                $(parentTag + " " + "#" + $itemlist[i].id + "").addClass("Selection");
            }

            endrow = i;
        } else {
            //表示しない
            $(parentTag + " " + "#" + $itemlist[i].id + "").css("display", "none");

            //選択状態から外す
            if (itemcd == privateFleetItemCd) {
                $("#privateFleetItemHidden").val("");
                $("#privateFleetItem").CustomTextBox("updateText", "");
                // 2018/08/27 TCS 佐々木 TKM Next Gen e-CRB Project Application development Block B-1 START
                resetCustOrgnzName({alsoBelow: true});
                // 2018/08/27 TCS 佐々木 TKM Next Gen e-CRB Project Application development Block B-1 END

                $(parentTag + " " + "#" + $itemlist[i].id + "").removeAttr("Selection");
            }
        }
    }

    if (endrow >= 0) {
        //endRowを一度削除
        $("#scNscCustomerEditingWindown .dataWind2 .privateFleetItemListBoxSetIn li").removeClass("endRow");
        $(parentTag + " " + "#" + $itemlist[endrow].id + "").addClass("endRow");
    }

} 

// 2018/08/27 TCS 佐々木 TKM Next Gen e-CRB Project Application development Block B-1 START
/**
 * HTMLで保持されている各種フォームデータをASP.NETに通知する
 * @param {HTMLElement|JQueryObject} baseElement
 */
function updateCustomerData(baseElement) {
    var selectors = [
        { key: 'firstName', sel: '#nameTextBox' },                                  // ファーストネーム
        { key: 'middleName', sel: '#middleNameTextBox' },                           // ミドルネーム
        { key: 'lastName', sel: '#lastNameTextBox' },                               // ラストネーム

        { key: 'nameTitle', sel: '#nameTitleHidden' },                              // 敬称コード

        { key: 'man', sel: 'input[id=manCheckBox]:checked' },                       // 男
        { key: 'girl', sel: 'input[id=girlCheckBox]:checked' },                     // 女
        { key: 'other', sel: 'input[id=otherCheckBox]:checked' },                   // その他
        { key: 'unknown', sel: 'input[id=unknownCheckBox]:checked' },               // 不明

        { key: 'kojin', sel: 'input[id=kojinCheckBox]:checked' },                   // 個人
        { key: 'houjin', sel: 'input[id=houjinCheckBox]:checked' },                 // 法人

        { key: 'employeeName', sel: '#employeenameTextBox' },                       // 担当者氏名
        { key: 'employeeDepartment', sel: '#employeedepartmentTextBox' },           // 担当者部署名
        { key: 'employeePosition', sel: '#employeepositionTextBox' },               // 役職

        { key: 'mobile', sel: '#mobileTextBox' },                                   // 携帯
        { key: 'telNo', sel: '#telnoTextBox' },                                     // 自宅
        { key: 'businessTelNo', sel: '#businesstelnoTextBox' },                     // 勤務先
        { key: 'faxNo', sel: '#faxnoTextBox' },                                     // FAX

        { key: 'zipCode', sel: '#zipcodeTextBox' },                                 // 郵便番号
        { key: 'address', sel: '#addressTextBox' },                                 // 住所1
        { key: 'address2', sel: '#address2TextBox' },                               // 住所2
        { key: 'address3', sel: '#address3TextBox' },                               // 住所3
        { key: 'state', sel: '#stateHidden' },                                      // 住所(州)
        { key: 'addressState', sel: '#addressState' },                              // 住所(州)(名称)
        { key: 'district', sel: '#districtHidden' },                                // 住所(地域)
        { key: 'addressDistrict', sel: '#addressDistrict' },                        // 住所(地域)(名称)
        { key: 'city', sel: '#cityHidden' },                                        // 住所(市)
        { key: 'addressCity', sel: '#addressCity' },                                // 住所(市)(名称)
        { key: 'location', sel: '#locationHidden' },                                // 住所(地区)
        { key: 'addressLocation', sel: '#addressLocation' },                        // 住所(地区)(名称)

        { key: 'email1', sel: '#email1TextBox' },                                   // E-Mail1
        { key: 'email2', sel: '#email2TextBox' },                                   // E-Mail2

        { key: 'socialId', sel: '#socialidTextBox' },                               // 国民ID

        { key: 'birthDay', sel: '#birthdayTextBox' },                               // 誕生日

        { key: 'actvCtgryId', sel: '#actvctgryidHidden' },                          // 活動区分
        { key: 'reasonId', sel: '#reasonidHidden' },                                // 断念理由

        { key: 'sms', sel: 'input[id=smsCheckButton]:checked' },                    // SMS
        { key: 'email', sel: 'input[id=emailCheckButton]:checked' },                // EMail
        { key: 'nameTitleText', sel: '#nameTitleTextHidden' },                      // 敬称
        { key: 'privateFleetItem', sel: '#privateFleetItemHidden' },                // 個人法人項目
        { key: 'domicile', sel: '#domicileTextBox' },                               // 本籍
        { key: 'country', sel: '#countryTextBox' },                                 // 国籍
        { key: 'commercialRecvType', sel: '.CommercialRecvType input:checked' },    // 商業情報受取区分
        { key: 'income', sel: '#incomeTextBox' },                                   // 収入

        { key: 'custOrgnz', sel: '#custOrgnzHidden' },                              // 顧客組織コード
        { key: 'custOrgnzInputType', sel: '#custOrgnzInputTypeHidden' },            // 顧客組織入力区分
        { key: 'custOrgnzName', sel: '#custOrgnz' },                                // 顧客組織名称
        { key: 'custSubCtgry2', sel: '#custSubCtgry2Hidden' },                      // 顧客サブカテゴリ2コード
    ];
    var prms = selectors
        .map(function (item) { return { key: item.key, el: $(item.sel, baseElement)[0] } })
        .map(function (item) { return item.key + '=' + (item.el ? encodeURIComponent(item.el.value) : '') })
        .join(',');
    
    callback.doCallback('ApplyFormData', prms);
}

/**
 * 顧客組織リセット
 * @param {{alsoBelow?: boolean}} opts 
 */
function resetCustOrgnzName(opts) {
    var $parent = $('#CustomerEditVisiblePanel');
    $('#custOrgnzNameTextBox, #custOrgnzNameSuggestiveTextBox', $parent[0]).val('');
    $('.custOrgnzListBoxSetIn .custOrgnzList', $parent).removeClass('Selection endRow');
    $('#custOrgnz').CustomTextBox('updateText', '');
    $('#custOrgnzHidden, #custOrgnzInputTypeHidden').val('');
    // 2019/04/08 TS 舩橋 POST-UAT3110 電話番号検索すると、サブカテゴリ２が消える START
    $('#custOrgnzNameHidden').val('');
    // 2019/04/08 TS 舩橋 POST-UAT3110 電話番号検索すると、サブカテゴリ２が消える END

    if ((opts || {}).alsoBelow) {
        resetCustSubCtgry2(opts);
    }
    else {
        updateCustomerData();
    }
}

/**
 * サブカテゴリ2リセット
 * @param {{alsoBelow?: boolean}} opts 
 */
function resetCustSubCtgry2(opts) {
    var $parent = $('#CustomerEditVisiblePanel');
    $('.custSubCtgry2ListBoxSetIn .custSubCtgry2List', $parent[0]).removeClass('Selection endRow');
    $('#custSubCtgry2').CustomTextBox('updateText', '');
    $('#custSubCtgry2Hidden').val('');

    if ((opts || {}).alsoBelow) {
        void 0; // do nothing
    }
    updateCustomerData();
}
// 2018/08/27 TCS 佐々木 TKM Next Gen e-CRB Project Application development Block B-1 END

// 住所(州～地区) 選択状態にする　-------------------------------------------------
function addresslist(parentTag) {

    // 現在選択を解除する
    $(parentTag + " " + ".stateList").removeClass("Selection");
    $(parentTag + " " + ".districtList").removeClass("Selection");
    $(parentTag + " " + ".cityList").removeClass("Selection");
    $(parentTag + " " + ".locationList").removeClass("Selection");

    var stateCdhidden = $("#stateHidden").val();
    var districtCdhidden = $("#districtHidden").val();
    var cityCdhidden = $("#cityHidden").val();
    var locationCdhidden = $("#locationHidden").val();

    // 州
    var $list2 = $(parentTag + " " + " ul.stateListBoxSetIn").children();
    for (i = 0; i < $list2.length; i++) {
        var cd = $(parentTag + " " + "#" + $list2[i].id + "").children(".stateHidden").text();

        //選択状態にする
        if (cd == stateCdhidden) {
            $(parentTag + " " + "#" + $list2[i].id + "").addClass("Selection");
            //テキストボックス反映
            $("#addressState").CustomTextBox("updateText", $(parentTag + " " + "#" + $list2[i].id + "").children(".stateLabel").text());
        }
    }
    // 地域
    $list2 = $(parentTag + " " + " ul.districtListBoxSetIn").children();
    for (i = 0; i < $list2.length; i++) {
        var cd = $(parentTag + " " + "#" + $list2[i].id + "").children(".districtHidden").text();

        //選択状態にする
        if (cd == districtCdhidden) {
            $(parentTag + " " + "#" + $list2[i].id + "").addClass("Selection");
            //テキストボックス反映
            $("#addressDistrict").CustomTextBox("updateText", $(parentTag + " " + "#" + $list2[i].id + "").children(".districtLabel").text());
        }
    }
    // 市
    $list2 = $(parentTag + " " + " ul.cityListBoxSetIn").children();
    for (i = 0; i < $list2.length; i++) {
        var cd = $(parentTag + " " + "#" + $list2[i].id + "").children(".cityHidden").text();

        //選択状態にする
        if (cd == cityCdhidden) {
            $(parentTag + " " + "#" + $list2[i].id + "").addClass("Selection");
            //テキストボックス反映
            $("#addressCity").CustomTextBox("updateText", $(parentTag + " " + "#" + $list2[i].id + "").children(".cityLabel").text());
        }
    }
    // 地区
    $list2 = $(parentTag + " " + " ul.locationListBoxSetIn").children();
    for (i = 0; i < $list2.length; i++) {
        var cd = $(parentTag + " " + "#" + $list2[i].id + "").children(".locationHidden").text();

        //選択状態にする
        if (cd == locationCdhidden) {
            $(parentTag + " " + "#" + $list2[i].id + "").addClass("Selection");
            //テキストボックス反映
            $("#addressLocation").CustomTextBox("updateText", $(parentTag + " " + "#" + $list2[i].id + "").children(".locationLabel").text());
        }
    }

    return true;
}
//--2013/11/29 TCS 各務 Aカード情報相互連携開発 END

// 顧客編集　個人　-------------------------------------------------
$(function () {
    $(".scKojinCheck").live("click", function (e) {

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

        //2012/01/26 TCS 安田 【SALES_1B】チェックマークのチェックの色（青／赤）を切り替える START
        // 色を切り替える
        setCheckColor($("#kojinCheckBox"));
        setCheckColor($("#houjinCheckBox"));
        //2012/01/26 TCS 安田 【SALES_1B】チェックマークのチェックの色（青／赤）を切り替える END

        //--2013/11/29 TCS 各務 Aカード情報相互連携開発 START
        // 個人法人項目リストセット
        changePrivateFleetItemlist("#scNscCustomerEditingWindown");
        //--2013/11/29 TCS 各務 Aカード情報相互連携開発 END

        // 敬称リストセット
        changeNamelist("#scNscCustomerEditingWindown");

        setDispNameArea();
    });
});


// 顧客編集　法人　-------------------------------------------------
$(function () {
    $(".scHoujinCheck").live("click", function (e) {

        if ($("#houjinCheckBox").attr("checked")) {
            //2017/11/16 TCS 河原 TKM独自機能開発 START
            if ($("#CleansingMode").val() == "0") {
                //法人情報の表示
                $("#houjinPanel").css("display", "block");
            }
            //2017/11/16 TCS 河原 TKM独自機能開発 END

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

        //2012/01/26 TCS 安田 【SALES_1B】チェックマークのチェックの色（青／赤）を切り替える START
        // 色を切り替える
        setCheckColor($("#kojinCheckBox"));
        setCheckColor($("#houjinCheckBox"));
        //2012/01/26 TCS 安田 【SALES_1B】チェックマークのチェックの色（青／赤）を切り替える END

        //--2013/11/29 TCS 各務 Aカード情報相互連携開発 START
        // 個人法人項目リストセット
        changePrivateFleetItemlist("#scNscCustomerEditingWindown");
        //--2013/11/29 TCS 各務 Aカード情報相互連携開発 END

        // 敬称リストセット
        changeNamelist("#scNscCustomerEditingWindown");

        setDispNameArea();
    });
});

//2018/09/27 TCS 河原 （トライ店システム評価）基幹連携を用いたセールス業務実績入力の検証 START
function setDispNameArea() {
    if ($("#Use_Customerdata_Cleansing_Flg").val() == "1") {
        if ($("#houjinCheckBox").attr("checked")) {
            //法人のチェックがon
            //ミドルネームとラストネーム欄を非表示
            $("#row02").css("display", "none");
            $("#row03").css("display", "none");
        
            //デザインの調整
            $("#header01").removeClass("scNscCustomerEditingListItemBottomBorder");
            $("#data01").removeClass("scNscCustomerEditingListItemBottomBorder");
        } else {
            //個人のチェックがon or どちらもoff
            //ミドルネームとラストネーム欄を表示
            if ($("#inputSettingHidden02").val() != "0") {
                $("#row02").removeAttr("style");
                if ($("#cust_flg_hidden").val() != "1") {
                    $("#middleNameTextBox").removeAttr("disabled");
                }
            }
            if ($("#inputSettingHidden03").val() != "0") {
                $("#row03").removeAttr("style");
                if ($("#cust_flg_hidden").val() != "1") {
                    $("#lastNameTextBox").removeAttr("disabled");
                }
            }

            if ($("#inputSettingHidden02").val() != "0" || $("#inputSettingHidden03").val() != "0") {
                //デザインの調整
                $("#header01").addClass("scNscCustomerEditingListItemBottomBorder");
                $("#data01").addClass("scNscCustomerEditingListItemBottomBorder");
            }
        }
    }
}
//2018/09/27 TCS 河原 （トライ店システム評価）基幹連携を用いたセールス業務実績入力の検証 END

//2012/01/26 TCS 安田 【SALES_1B】チェックマークのチェックの色（青／赤）を切り替える START
// 顧客編集　SMSチェック　-------------------------------------------------
$(function () {
    $(".scSmsCheck").live("click", function (e) {
        // 色を切り替える
        setCheckColor($("#smsCheckButton"));
    });
});

// 顧客編集　E-mailチェック　-------------------------------------------------
$(function () {
    $(".scEmailCheck").live("click", function (e) {
        // 色を切り替える
        setCheckColor($("#emailCheckButton"));
    });
});
//2012/01/26 TCS 安田 【SALES_1B】チェックマークのチェックの色（青／赤）を切り替える END

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
    $('.scMunCheck').live("click", function (e) {

        if ($("#manCheckBox").attr("checked")) {
            //男チェック時に、女チェックを外す
            $("#girlCheckBox").removeAttr("checked");
            var wrapperElement = $("#girlCheckBox").parent();
            var chkimg = wrapperElement.children(".icrop-CheckMark-checked");
            if (chkimg != null) {
                chkimg.removeClass("icrop-CheckMark-checked");
            }
            //--2013/11/29 TCS 各務 Aカード情報相互連携開発 START
            //男チェック時に、その他、不明のチェックを外す
            $("#otherCheckBox").removeAttr("checked");
            wrapperElement = $("#otherCheckBox").parent();
            chkimg = wrapperElement.children(".icrop-CheckMark-checked");
            if (chkimg != null) {
                chkimg.removeClass("icrop-CheckMark-checked");
            }
            $("#unknownCheckBox").removeAttr("checked");
            wrapperElement = $("#unknownCheckBox").parent();
            chkimg = wrapperElement.children(".icrop-CheckMark-checked");
            if (chkimg != null) {
                chkimg.removeClass("icrop-CheckMark-checked");
            }
            //--2013/11/29 TCS 各務 Aカード情報相互連携開発 END
        }

        // 色を切り替える
        setCheckColor($("#manCheckBox"));
        setCheckColor($("#girlCheckBox"));
        //--2013/11/29 TCS 各務 Aカード情報相互連携開発 START
        setCheckColor($("#otherCheckBox"));
        setCheckColor($("#unknownCheckBox"));
        //--2013/11/29 TCS 各務 Aカード情報相互連携開発 END
    });
    $('.scGirlCheck').live("click", function (e) {
        if ($("#girlCheckBox").attr("checked")) {
            //女チェック時に、男チェックを外す
            $("#manCheckBox").removeAttr("checked");
            var wrapperElement = $("#manCheckBox").parent();
            var chkimg = wrapperElement.children(".icrop-CheckMark-checked");
            if (chkimg != null) {
                chkimg.removeClass("icrop-CheckMark-checked");
            }
            //--2013/11/29 TCS 各務 Aカード情報相互連携開発 START
            //女チェック時に、その他、不明のチェックを外す
            $("#otherCheckBox").removeAttr("checked");
            wrapperElement = $("#otherCheckBox").parent();
            chkimg = wrapperElement.children(".icrop-CheckMark-checked");
            if (chkimg != null) {
                chkimg.removeClass("icrop-CheckMark-checked");
            }
            $("#unknownCheckBox").removeAttr("checked");
            wrapperElement = $("#unknownCheckBox").parent();
            chkimg = wrapperElement.children(".icrop-CheckMark-checked");
            if (chkimg != null) {
                chkimg.removeClass("icrop-CheckMark-checked");
            }
            //--2013/11/29 TCS 各務 Aカード情報相互連携開発 END
        }

        //2012/01/26 TCS 安田 【SALES_1B】チェックマークのチェックの色（青／赤）を切り替える START
        // 色を切り替える
        setCheckColor($("#manCheckBox"));
        setCheckColor($("#girlCheckBox"));
        //2012/01/26 TCS 安田 【SALES_1B】チェックマークのチェックの色（青／赤）を切り替える END
        //--2013/11/29 TCS 各務 Aカード情報相互連携開発 START
        setCheckColor($("#otherCheckBox"));
        setCheckColor($("#unknownCheckBox"));
        //--2013/11/29 TCS 各務 Aカード情報相互連携開発 END
    });
    //--2013/11/29 TCS 各務 Aカード情報相互連携開発 START
    // 性別(その他)
    $('.scOtherCheck').live("click", function (e) {
        if ($("#otherCheckBox").attr("checked")) {
            //その他チェック時に、それ以外のチェックを外す
            $("#manCheckBox").removeAttr("checked");
            var wrapperElement = $("#manCheckBox").parent();
            var chkimg = wrapperElement.children(".icrop-CheckMark-checked");
            if (chkimg != null) {
                chkimg.removeClass("icrop-CheckMark-checked");
            }
            $("#girlCheckBox").removeAttr("checked");
            wrapperElement = $("#girlCheckBox").parent();
            chkimg = wrapperElement.children(".icrop-CheckMark-checked");
            if (chkimg != null) {
                chkimg.removeClass("icrop-CheckMark-checked");
            }
            $("#unknownCheckBox").removeAttr("checked");
            wrapperElement = $("#unknownCheckBox").parent();
            chkimg = wrapperElement.children(".icrop-CheckMark-checked");
            if (chkimg != null) {
                chkimg.removeClass("icrop-CheckMark-checked");
            }
        }

        // 色を切り替える
        setCheckColor($("#manCheckBox"));
        setCheckColor($("#girlCheckBox"));
        setCheckColor($("#otherCheckBox"));
        setCheckColor($("#unknownCheckBox"));
    });
    // 性別(不明)
    $('.scUnknownCheck').live("click", function (e) {
        if ($("#unknownCheckBox").attr("checked")) {
            //不明チェック時に、それ以外のチェックを外す
            $("#manCheckBox").removeAttr("checked");
            var wrapperElement = $("#manCheckBox").parent();
            var chkimg = wrapperElement.children(".icrop-CheckMark-checked");
            if (chkimg != null) {
                chkimg.removeClass("icrop-CheckMark-checked");
            }
            $("#girlCheckBox").removeAttr("checked");
            wrapperElement = $("#girlCheckBox").parent();
            chkimg = wrapperElement.children(".icrop-CheckMark-checked");
            if (chkimg != null) {
                chkimg.removeClass("icrop-CheckMark-checked");
            }
            $("#otherCheckBox").removeAttr("checked");
            wrapperElement = $("#otherCheckBox").parent();
            chkimg = wrapperElement.children(".icrop-CheckMark-checked");
            if (chkimg != null) {
                chkimg.removeClass("icrop-CheckMark-checked");
            }
        }

        // 色を切り替える
        setCheckColor($("#manCheckBox"));
        setCheckColor($("#girlCheckBox"));
        setCheckColor($("#otherCheckBox"));
        setCheckColor($("#unknownCheckBox"));
    });
    //--2013/11/29 TCS 各務 Aカード情報相互連携開発 END
});

// 2014/03/18 TCS 市川  新PF開発No.41（商業情報活動）START 
// 顧客編集　商業情報　-------------------------------------------------
$(function () {
    $('.CommercialRecvType').live("click", function (e) {
        
        //チェックが入るイベントの時のみ・クリア処理をいれる
        if ($(this).find('.icrop-CheckMark input').attr("checked")) {
            //チェック済みのクリア
            $('.CommercialRecvType input').removeAttr("checked");
            $('.CommercialRecvType .icrop-CheckMark-checked').removeClass("icrop-CheckMark-checked");
                        
            //クリック対象のチェック済み設定
            $(this).find('.icrop-CheckMark input').attr("checked", "checked");
            $(this).find('.icrop-CheckMark span:last').addClass("icrop-CheckMark-checked");
        }

        // 色を切り替える
        setCheckColor($('#commercialRecvType_Empty'));
        setCheckColor($('#commercialRecvType_Yes'));
        setCheckColor($('#commercialRecvType_No'));
    });
});
// 2014/03/18 TCS 市川  新PF開発No.41（商業情報活動）END 

//顧客編集ポップアップ関連/////////////////////
//顧客情報再表示用
function CustomerEditPopUpClose() {
    $("#customerReload").click();
}

//画面全体再表示用
function CustomerInsertPopUpClose() {

    //2012/05/17 TCS 安田 クルクル対応 START
    //タイマーセット
    commonRefreshTimer(function () {
        //二度押し防止フラグをキャンセル(false)にする
        SC3080201.serverProcessing = false;
        //再読み込み実施
        location.replace($("#CustomerReLoadURL").val());
    });
    //2012/05/17 TCS 安田 クルクル対応 END

    location.replace($("#CustomerReLoadURL").val());
}


//顧客編集　END -------------------------------------------------------------------------------------

// 2017/11/16 TCS 河原 TKM独自機能開発 START
function CleansingMode() {

    //性別
    $(".scNscCustomerEditingListItem3").css("display", "none");

    //個人法人項目
    $("#PrivateFleetItem").css("display", "none");

    // 2018/08/27 TCS 佐々木 TKM Next Gen e-CRB Project Application development Block B-1 START
    // 顧客組織
    $("#CustOrgnz").hide();
    // 2018/08/27 TCS 佐々木 TKM Next Gen e-CRB Project Application development Block B-1 END

    //法人
    $("#houjinPanel").css("display", "none");

    //職場電話番号
    $("#row13").css("display", "none");

    //職場電話番号
    $("#row14").css("display", "none");

    //電話番号欄のデザインの調整
    $("#header12").removeClass("scNscCustomerEditingListItemBottomBorder");
    $("#data12").removeClass("scNscCustomerEditingListItemBottomBorder");

    //本籍
    $("#Domicile").css("display", "none");

    //e-mail
    $("#mail").css("display", "none");

    //国籍
    $("#country").css("display", "none");

    //国民ID
    $("#socialId").css("display", "none");

    //誕生日
    $("#birthday").css("display", "none");

    //商業情報受取区分
    $("#commercialRecvType").css("display", "none");

    //年収
    $("#income").css("display", "none");

    //活動区分
    $("#actvctgryPanel").css("display", "none");

    //自社客の場合、住所も非表示にする
    if ($("#cust_flg_hidden").val() == "1") {
        $("#addressAll").css("display", "none");
    }

    //個人/法人区分
    $(".scNscCustomerEditingListItem4").css("display", "none");

    //サブカテゴリ2
    $("#CustSubCtgry2").css("display", "none");

    //表示時のフラグをoffに
    $("#CleansingModeFlg").val("0");

    //表示中フラグをonに
    $("#CleansingMode").val("1");
}

//住所1が未入力の場合、州・地区・市・地域欄を非活性にする
$("#addressTextBox").live("change",
 	 function () {
 	     changeAddress()
 	 }
);

//--2019/06/06 TS  村井 (FS)サービススタッフ納期遵守オペレーション確立に向けた試験研究 START
//--住所1がフォーカスを失う時、州・地区・市・地域欄の活性／非活性を再評価
//--クリアボタンでテキストを削除された時の対応 
$("#addressTextBox").live("focusout",
 	 function () {
 	     changeAddress()
 	 }
);

//--2019/06/06 TS  村井 (FS)サービススタッフ納期遵守オペレーション確立に向けた試験研究 END

//州・地区・市・地域欄を非活性にする
function changeAddress() {
    if ($("#Use_Customerdata_Cleansing_Flg").val() == "1") {
        if ($("#addressTextBox").val() == "") {

            //--2019/06/06 TS  村井 (FS)サービススタッフ納期遵守オペレーション確立に向けた試験研究 START
            if ($("#address1AutoInputHidden").val() == "1") {
            //--2019/06/06 TS  村井 (FS)サービススタッフ納期遵守オペレーション確立に向けた試験研究 END

                //州をクリックしたときのイベントの削除
                $("#scNscCustomerEditingWindown").find(".scNscCustomerEditingState").unbind("click");

                //各選択状態を解除
                $("#addressState").CustomTextBox("updateText", "");
                $("#addressDistrict").CustomTextBox("updateText", "");
                $("#addressCity").CustomTextBox("updateText", "");
                $("#addressLocation").CustomTextBox("updateText", "");
                $("#scNscCustomerEditingWindown .dataWindDistrict .districtListBoxSetIn").empty();
                $("#scNscCustomerEditingWindown .dataWindCity .cityListBoxSetIn").empty();
                $("#scNscCustomerEditingWindown .dataWindLocation .locationListBoxSetIn").empty();
                $("#districtHidden").val("");
                $("#districtTextHidden").val("");
                $("#cityHidden").val("");
                $("#cityTextHidden").val("");
                $("#locationHidden").val("");
                $("#locationTextHidden").val("");

                $("#scNscCustomerEditingWindown .stateList").removeClass("Selection");
                $("#scNscCustomerEditingWindown .districtList").removeClass("Selection");
                $("#scNscCustomerEditingWindown .cityList").removeClass("Selection");
                $("#scNscCustomerEditingWindown .locationList").removeClass("Selection");

                $("#stateHidden").val("");
                $("#districtHidden").val("");
                $("#cityHidden").val("");
                $("#locationHidden").val("");

            }
       } else {
           //州をクリックしたときのイベントの追加
           $("#scNscCustomerEditingWindown").find(".scNscCustomerEditingState").click(function (e) {

               if ($("#useStateHidden").val() == "1") {
                   makeStateListEvent();
                   setPopupCustomerEditPage("page2", "stateList");
               }
           });
       }
    }
}
// 2017/11/16 TCS 河原 TKM独自機能開発 END