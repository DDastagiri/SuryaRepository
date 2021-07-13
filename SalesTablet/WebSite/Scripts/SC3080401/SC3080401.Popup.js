//━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
//SC3080401.Popup.js
//─────────────────────────────────────
//機能： ヘルプ依頼PopUp
//補足： ヘルプ依頼PopUpを開くタイミングにて遅延ロードする
//作成： 2014/04/21 TCS 市川 GTMCタブレット高速化対応（BTS-386）
//─────────────────────────────────────

/**
* ヘルプ依頼画面の初期化を行う.
* 
* @param {String} result ヘルプ依頼画面のHTML
* @param {String} context 
* 
*/
function InitializeWindowSC3080401(result, context) {

    //コールバックによって取得したヘルプ依頼のHTMLを格納
	var contents = $('<Div>').html(result).text();
	//1ページ目のコンテンツを得取
	var helpRequestMain = $(contents).find('#HelpRequestMain');
	//３ページ目（依頼先一覧）のコンテンツを取得
	var sendAccountList = $(contents).find('#SendAccountList');
	//４ページ目（ヘルプ内容一覧）のコンテンツを取得
	var helpMstList = $(contents).find('#HelpMstList');

	//メイン画面のコンテンツを削除
	$('#HelpRequestMain>div').remove();
	//1ページ目のコンテンツを設定
	helpRequestMain.children('div').clone(true).appendTo('#HelpRequestMain');
	//依頼先一覧のコンテンツを削除
	$('#SendAccountList>div').remove();
	//依頼先一覧のコンテンツを設定
	sendAccountList.children('div').clone(true).appendTo('#SendAccountList');
	//ヘルプ内容一覧のコンテンツを削除
	$('#HelpMstList>div').remove();
	//ヘルプ内容一覧のコンテンツを設定
	helpMstList.children('div').clone(true).appendTo('#HelpMstList');

	//上下スクロールの設定
	$('#HelpRequestMain').fingerScroll();

	//2012/04/12 TCS 鈴木(健) HTMLエンコード対応 START
	//文言を取得
	SC3080401Word0001 = $(contents).find("#WordNo0001HiddenField").val()
	SC3080401Word0002 = $(contents).find("#WordNo0002HiddenField").val()
	SC3080401Word0003 = $(contents).find("#WordNo0003HiddenField").val()
	SC3080401Word0004 = $(contents).find("#WordNo0004HiddenField").val()
	SC3080401Word0005 = $(contents).find("#WordNo0005HiddenField").val()
	SC3080401Word0006 = $(contents).find("#WordNo0006HiddenField").val()
	SC3080401Word0007 = $(contents).find("#WordNo0007HiddenField").val()
	SC3080401Word0008 = $(contents).find("#WordNo0008HiddenField").val()
	SC3080401Word0009 = $(contents).find("#WordNo0009HiddenField").val()
	SC3080401Word9001 = $(contents).find("#WordNo9001HiddenField").val()

	//依頼ボタンの文言を設定
	$('#RequestButtonLabel').text(SC3080401Word0003);
	//キャンセルボタンの文言を設定
	$('#CancelButtonLabel').text(SC3080401Word0005);
	//依頼中ラベルの文言を設定
	$('#UnderRequest').text(SC3080401Word0004);
	//依頼先不在時の文言を設定
	$('#NoSendAccountLabel').text(SC3080401Word0009);
	//2012/04/12 TCS 鈴木(健) HTMLエンコード対応 END

	//依頼先情報欄をクリックした時の動作を削除
	$('#SelectedSendAccountArea').unbind('click');

    /**
    * 依頼先情報欄をクリックした時の動作を定義
    * 
    * @param {String} e イベントデータ
    * 
    */
    $('#SelectedSendAccountArea').bind('click', function (e) {

        //依頼中の場合は処理しない
        if ($('#IsUnderHelpRequest').val() == constants.trueString) {
            return;
        }

        //２ページ目表示領域をクリアする
        $('#DisplayPage>div').remove();
        //依頼先一覧のコンテンツを２ページ目にコピー
        $('#SendAccountList>div').clone(true).appendTo('#DisplayPage');

        //２ページ目に移動する
        popForm.pageIndex = 0;
        popForm.pushPage();

        //上下スクロールの設定
        $('#DisplayPage').fingerScroll();

        //ヘッダーの左ボタン（ヘルプへ）の定義
//2012/03/22 TCS 明瀬【SALES_2】TCS_0321ks_01対応 Mod Start
//        $('.icrop-PopOverForm-header-left').unbind('click');
//        popForm.headerElement.find(".icrop-PopOverForm-header-left").removeClass("icrop-PopOverForm-header-back");
        $('#helpRequestHeaderLeft').unbind('click');
        popForm.headerElement.find("#helpRequestHeaderLeft").removeClass("icrop-PopOverForm-header-back");
//2012/03/22 TCS 明瀬【SALES_2】TCS_0321ks_01対応 Mod End
        // 2012/03/13 TCS 鈴木(健) 【SALES_2】ポップオーバーフォームのキャンセルボタンが重複する問題の修正 START
        //$('.icrop-PopOverForm-header-left').empty().html('<div class="helpRequestHeaderBackButton"><a href="#" id="HeaderCancelButton" class="helpRequestUseCut"></a><span class="tgLeft">&nbsp;</span></div>');
        //$('#HeaderCancelButton').text($('#WordNo0006HiddenField').val());
//2012/03/22 TCS 明瀬【SALES_2】TCS_0321ks_01対応 Mod Start
//        $('.icrop-PopOverForm-header-left').empty().html('<div class="helpRequestHeaderBackButton"><a href="#" id="HeaderCancelButton" class="helpRequestUseCut">' + $('#WordNo0006HiddenField').val() + '</a><span class="tgLeft">&nbsp;</span></div>');
        //2012/04/12 TCS 鈴木(健) HTMLエンコード対応 START
        $('#helpRequestHeaderLeft').empty().html('<div class="helpRequestHeaderBackButton"><a href="#" id="HeaderCancelButton" class="helpRequestUseCut">' + SC3080401HTMLEncode(SC3080401Word0006) + '</a><span class="tgLeft">&nbsp;</span></div>');
//2012/03/22 TCS 明瀬【SALES_2】TCS_0321ks_01対応 Mod End
        // 2012/03/13 TCS 鈴木(健) 【SALES_2】ポップオーバーフォームのキャンセルボタンが重複する問題の修正 END
        $('#HeaderTitle').text(SC3080401Word0007);
        $('.helpRequestHeaderBackButton').click(function (e) {
//2012/03/22 TCS 明瀬【SALES_2】TCS_0321ks_01対応 Mod Start
//            $('.icrop-PopOverForm-header-left').remove();
//            $('.icrop-PopOverForm-header-right').remove();
            $('#helpRequestHeaderLeft').remove();
            $('#helpRequestHeaderRight').remove();
//2012/03/22 TCS 明瀬【SALES_2】TCS_0321ks_01対応 Mod End
            $('#HeaderTitle').text(SC3080401Word0001);
            //2012/04/12 TCS 鈴木(健) HTMLエンコード対応 END
            popForm.popPage();
        });

        //依頼先を選択した時の動作を削除
        $('#DisplayPage #SendAccountRow').unbind('click');

        /**
        * 依頼先を選択した時の動作を定義
        * 
        * @param {String} e イベントデータ
        * 
        */
        $('#DisplayPage #SendAccountRow').bind('click', function (e) {

            //選択された依頼先がオフラインの場合は処理しない
            //2013/10/03 TCS 市川【次世代e-CRBタブレット iOS7.0 VersionUp対応（セールス）】MOD START
            //iOS7対応：同一IDが複数ある場合の.FINDメソッド不具合回避のためコントロールIDによる制御からclass名による制御へ変更
            //if ($(this).find('#OnlineStatus').val() == constants.presenceCategoryOffline) {
            if ($(this).find('.OnlineStatus').val() == constants.presenceCategoryOffline) {
                //2013/10/03 TCS 市川【次世代e-CRBタブレット iOS7.0 VersionUp対応（セールス）】MOD END
                return;
            }

            //すべてのチェックマークを削除する
            $('#DisplayPage #SendAccountRow').removeClass('Check');
            
            //選択された行にチェックマークを設定する
            $(this).addClass('Check');

            //ヘルプ依頼画面メインに選択された依頼先を反映
            //2013/10/03 TCS 市川 【次世代e-CRBタブレット iOS7.0 VersionUp対応（セールス）】MOD START
//            //2012/04/12 TCS 鈴木(健) HTMLエンコード対応 START
//            $('#SelectedSendAccountName_Display').text(SC3080401HTMLDecode($(this).find('#SendAccountName').val()));
//            $('#SelectedSendAccountName').val(SC3080401HTMLDecode($(this).find('#SendAccountName').val()));
//            $('#SelectedSendAccount').val(SC3080401HTMLDecode($(this).find('#SendAccount').val()));
//            //2012/04/12 TCS 鈴木(健) HTMLエンコード対応 END            
            //iOS7対応：同一IDが複数ある場合の.FINDメソッド不具合回避のためコントロールIDによる制御からclass名による制御へ変更
            $('#SelectedSendAccountName_Display').text(SC3080401HTMLDecode($(this).find('.SendAccountName').val()));
            $('#SelectedSendAccountName').val(SC3080401HTMLDecode($(this).find('.SendAccountName').val()));
            $('#SelectedSendAccount').val(SC3080401HTMLDecode($(this).find('.SendAccount').val()));
            //2013/10/03 TCS 市川 【次世代e-CRBタブレット iOS7.0 VersionUp対応（セールス）】MOD END

            //ヘッダーボタンの削除（POPする度にボタンが追加されてしまうため）
//2012/03/22 TCS 明瀬【SALES_2】TCS_0321ks_01対応 Mod Start
//            $('.icrop-PopOverForm-header-left').remove();
//            $('.icrop-PopOverForm-header-right').remove();
            $('#helpRequestHeaderLeft').remove();
            $('#helpRequestHeaderRight').remove();
//2012/03/22 TCS 明瀬【SALES_2】TCS_0321ks_01対応 Mod End
            //2012/04/12 TCS 鈴木(健) HTMLエンコード対応 START
            $('#HeaderTitle').text(SC3080401Word0001);
            //2012/04/12 TCS 鈴木(健) HTMLエンコード対応 END
            popForm.popPage();

            //依頼先一覧のコンテンツを削除
            $('#SendAccountList').find('div').remove();
            //表示領域の内容を依頼先一覧にコピー
            $('#DisplayPage>div').clone(true).appendTo('#SendAccountList');
        });
    });

	//ヘルプ内容欄をクリックした時の動作を削除
    $('#SelectedHelpMstArea').unbind('click');

    /**
    * ヘルプ内容欄をクリックした時の動作を定義
    * 
    * @param {String} e イベントデータ
    * 
    */
    $('#SelectedHelpMstArea').bind('click', function (e) {

        //依頼中の場合は処理しない
        if ($('#IsUnderHelpRequest').val() == constants.trueString) {
            return;
        }

        //２ページ目表示領域をクリアする
        $('#DisplayPage>div').remove();
        //ヘルプ内容一覧のコンテンツを２ページ目にコピー
        $('#HelpMstList>div').clone(true).appendTo('#DisplayPage');

        //２ページ目に移動する
        popForm.pageIndex = 0;
		popForm.pushPage();

		//上下スクロールの設定
		$('#DisplayPage').fingerScroll();

		//ヘッダーの左ボタン（ヘルプへ）の定義
//2012/03/22 TCS 明瀬【SALES_2】TCS_0321ks_01対応 Mod Start
//		$('.icrop-PopOverForm-header-left').unbind('click');
//		popForm.headerElement.find(".icrop-PopOverForm-header-left").removeClass("icrop-PopOverForm-header-back");
		$('#helpRequestHeaderLeft').unbind('click');
		popForm.headerElement.find("#helpRequestHeaderLeft").removeClass("icrop-PopOverForm-header-back");
//2012/03/22 TCS 明瀬【SALES_2】TCS_0321ks_01対応 Mod Start
        // 2012/03/13 TCS 鈴木(健) 【SALES_2】ポップオーバーフォームのキャンセルボタンが重複する問題の修正 START
		//$('.icrop-PopOverForm-header-left').empty().html('<div class="helpRequestHeaderBackButton"><a href="#" id="HeaderCancelButton"></a><span class="tgLeft">&nbsp;</span></div>');
		//$('#HeaderCancelButton').text($('#WordNo0006HiddenField').val());
//2012/03/22 TCS 明瀬【SALES_2】TCS_0321ks_01対応 Mod Start
//		$('.icrop-PopOverForm-header-left').empty().html('<div class="helpRequestHeaderBackButton"><a href="#" id="HeaderCancelButton">' + $('#WordNo0006HiddenField').val() + '</a><span class="tgLeft">&nbsp;</span></div>');
		//2012/04/12 TCS 鈴木(健) HTMLエンコード対応 START
		$('#helpRequestHeaderLeft').empty().html('<div class="helpRequestHeaderBackButton"><a href="#" id="HeaderCancelButton" class="helpRequestUseCut">' + SC3080401HTMLEncode(SC3080401Word0006) + '</a><span class="tgLeft">&nbsp;</span></div>');
//2012/03/22 TCS 明瀬【SALES_2】TCS_0321ks_01対応 Mod End
        // 2012/03/13 TCS 鈴木(健) 【SALES_2】ポップオーバーフォームのキャンセルボタンが重複する問題の修正 END
		$('#HeaderTitle').text(SC3080401Word0008);
		$('.helpRequestHeaderBackButton').click(function (e) {
//2012/03/22 TCS 明瀬【SALES_2】TCS_0321ks_01対応 Mod Start
//			$('.icrop-PopOverForm-header-left').remove();
//			$('.icrop-PopOverForm-header-right').remove();
			$('#helpRequestHeaderLeft').remove();
			$('#helpRequestHeaderRight').remove();
//2012/03/22 TCS 明瀬【SALES_2】TCS_0321ks_01対応 Mod End
			$('#HeaderTitle').text(SC3080401Word0001);
			//2012/04/12 TCS 鈴木(健) HTMLエンコード対応 END
			popForm.popPage();
        });

        //ヘルプ内容を選択した時の動作を削除
        $('#DisplayPage #HelpMstRow').unbind('click');

        /**
        * ヘルプ内容を選択した時の動作を定義
        * 
        * @param {String} e イベントデータ
        * 
        */
        $('#DisplayPage #HelpMstRow').bind('click', function (e) {
            //すべてのチェックマークを削除する
            $('#DisplayPage #HelpMstRow').removeClass('Check');
            //選択された行にチェックマークを設定する
            $(this).addClass('Check');

            //ヘルプ依頼画面メインに選択されたヘルプ内容を反映
            //2013/10/03 TCS 市川【次世代e-CRBタブレット iOS7.0 VersionUp対応（セールス）】MOD START
//            //2012/04/12 TCS 鈴木(健) HTMLエンコード対応 START
//			$('#SelectedHelpName_Display').text(SC3080401HTMLDecode($(this).find('#HelpName').val()));
//			$('#SelectedHelpName').val(SC3080401HTMLDecode($(this).find('#HelpName').val()));
//			$('#SelectedHelpid').val(SC3080401HTMLDecode($(this).find('#Helpid').val()));
//			//2012/04/12 TCS 鈴木(健) HTMLエンコード対応 END
            //iOS7対応：同一IDが複数ある場合の.FINDメソッド不具合回避のためコントロールIDによる制御からclass名による制御へ変更
			$('#SelectedHelpName_Display').text(SC3080401HTMLDecode($(this).find('.HelpName').val()));
            $('#SelectedHelpName').val(SC3080401HTMLDecode($(this).find('.HelpName').val()));
            $('#SelectedHelpid').val(SC3080401HTMLDecode($(this).find('.Helpid').val()));
			//2013/10/03 TCS 市川【次世代e-CRBタブレット iOS7.0 VersionUp対応（セールス）】MOD END

			//ヘッダーボタンの削除（POPする度にボタンが追加されてしまうため）
//2012/03/22 TCS 明瀬【SALES_2】TCS_0321ks_01対応 Mod Start
//			$('.icrop-PopOverForm-header-left').remove();
//			$('.icrop-PopOverForm-header-right').remove();
			$('#helpRequestHeaderLeft').remove();
			$('#helpRequestHeaderRight').remove();
//2012/03/22 TCS 明瀬【SALES_2】TCS_0321ks_01対応 Mod End
			//2012/04/12 TCS 鈴木(健) HTMLエンコード対応 START
			$('#HeaderTitle').text(SC3080401Word0001);
			//2012/04/12 TCS 鈴木(健) HTMLエンコード対応 END
			popForm.popPage();

			//ヘルプ内容一覧のコンテンツを削除
			$('#HelpMstList').find('div').remove();
			//表示領域の内容をヘルプ内容一覧にコピー
			$('#DisplayPage>div').clone(true).appendTo('#HelpMstList');
		});
	});

	//
    /**
    * 依頼ボタンクリック時の動作を定義
    * 
    */
	$('#RequestButton').click(function () {

        //ボタンが活性の場合は非活性にする
	    if ($('#IsRequestButtonEnabled').val() == constants.trueString) {
	        $('#IsRequestButtonEnabled').val(constants.falseString);
	        $(this).removeClass('helpRequestPopUpUnderRequestButton');
	        $(this).addClass('helpRequestPopUpUnderRequestButtonDisabled');
	    //ボタンが非活性の場合は処理しない
	    } else {
	        return;
	    }

	    //処理中表示開始
	    SC3080401.startServerCallback();

	    var prms = '';
	    // 2012/03/13 TCS 鈴木(健) 【SALES_2】アカウント/アカウント名にカンマが含まれると正しいデータが登録できない不具合修正 START
	    //prms = prms + $('#SelectedSendAccount').val() + ',';
	    //prms = prms + $('#SelectedSendAccountName').val() + ',';
	    prms = prms + encodeURIComponent($('#SelectedSendAccount').val()) + ',';
	    prms = prms + encodeURIComponent($('#SelectedSendAccountName').val()) + ',';
	    // 2012/03/13 TCS 鈴木(健) 【SALES_2】アカウント/アカウント名にカンマが含まれると正しいデータが登録できない不具合修正 END
	    prms = prms + $('#SelectedHelpid').val() + ',';

	    CallbackSC3080401.doCallback('RequestButton_Click', prms, function (result, context) {

	        //処理結果の判定
	        switch (result) {
	            //正常終了  
	            case constants.messageIdSuccess:
	                CreateHelpRequestWindow();
	                break;
	            //異常終了（DBタイムアウト）  
	            case constants.messageIdDbTimeOut:
	                //処理中表示終了
	                SC3080401.endServerCallback();

	                //2012/04/12 TCS 鈴木(健) HTMLエンコード対応 START
	                alert(SC3080401Word9001);

	                //依頼ボタンを活性にする
	                $('#IsRequestButtonEnabled').val(constants.trueString);
	                $('#RequestButton').removeClass('helpRequestPopUpUnderRequestButtonDisabled');
	                $('#RequestButton').addClass('helpRequestPopUpUnderRequestButton');
	                //2012/04/12 TCS 鈴木(健) HTMLエンコード対応 END
	                break;
	            //異常終了（DBタイムアウト以外）   
	            default:
	                //処理中表示終了
	                SC3080401.endServerCallback();

	                alert(result);
	                break;
	        }
	    });
	});

	/**
	* キャンセルボタンクリック時の動作を定義
	* 
	*/
	$('#CancelButton').click(function () {

	    //ボタンが活性の場合は非活性にする
	    if ($('#IsCancelButtonEnabled').val() == constants.trueString) {
	        $('#IsCancelButtonEnabled').val(constants.falseString);
	        $(this).removeClass('helpRequestPopUpUnderCancelButton');
	        $(this).addClass('helpRequestPopUpUnderCancelButtonDisabled');
	    //ボタンが非活性の場合は処理しない
	    } else {
	        return;
	    }
	    
	    //処理中表示開始
	    SC3080401.startServerCallback();

	    var prms = '';
	    // 2012/03/13 TCS 鈴木(健) 【SALES_2】アカウント/アカウント名にカンマが含まれると正しいデータが登録できない不具合修正 START
	    //prms = prms + $('#SelectedSendAccount').val() + ',';
	    //prms = prms + $('#SelectedSendAccountName').val() + ',';
	    prms = prms + encodeURIComponent($('#SelectedSendAccount').val()) + ',';
	    prms = prms + encodeURIComponent($('#SelectedSendAccountName').val()) + ',';
	    // 2012/03/13 TCS 鈴木(健) 【SALES_2】アカウント/アカウント名にカンマが含まれると正しいデータが登録できない不具合修正 END
	    prms = prms + $('#SelectedHelpid').val() + ',';
	    prms = prms + $('#HelpNo').val() + ',';
	    prms = prms + $('#NoticeReqId').val() + ',';

	    CallbackSC3080401.doCallback('CancelButton_Click', prms, function (result, context) {
            
	        //処理結果の判定
	        switch (result) {
	            //正常終了 
	            case constants.messageIdSuccess:
	                CreateHelpRequestWindow();
	                break;
	            //異常終了（DBタイムアウト） 
	            case constants.messageIdDbTimeOut:

	                //処理中表示終了
	                SC3080401.endServerCallback();

	                //2012/04/12 TCS 鈴木(健) HTMLエンコード対応 START
	                alert(SC3080401Word9001);

	                //キャンセルボタンを活性にする
	                $('#IsCancelButtonEnabled').val(constants.trueString);
	                $('#CancelButton').removeClass('helpRequestPopUpUnderCancelButtonDisabled');
	                $('#CancelButton').addClass('helpRequestPopUpUnderCancelButton');
	                //2012/04/12 TCS 鈴木(健) HTMLエンコード対応 END
	                break;
	            //異常終了（DBタイムアウト以外）  
	            default:

	                //処理中表示終了
	                SC3080401.endServerCallback();

	                alert(result);
	                break;
	        }
	    });
	});
}
