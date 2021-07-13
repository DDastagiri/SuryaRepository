//------------------------------------------------------------------------------
//SC3140103.Main.js
//------------------------------------------------------------------------------
//機能：メインメニュー（SA）_javascript
//作成：2012/01/16 KN 森下
//更新：2012/04/09 KN 森下 【SERVICE_1】次世代サービス_企画＿プレユーザテスト課題不具合表 No149の不具合対応
//更新：2012/04/16 KN 西田 ユーザーテスト課題No.37 ダッシュボードをタップしてもチップ詳細が閉じられない START
//更新：2012/06/18 KN 西岡 【SERVICE_2】事前準備対応
//更新：2012/07/25 TMEJ 西岡  【SERVICE_2】STEP2対応(顧客検索機能追加)
//更新：2012/08/03 TMEJ 日比野【SERVICE_2】STEP2対応(初期表示処理の変更)
//更新：2012/08/03 TMEJ 彭 性能改善のために（ブラウザに送るファイルのサイズを最小化するために）、古い修正履歴を削除（TFSで参照可能）
//更新：2012/08/16 TMEJ 日比野【SERVICE_2】カウンターの表示変更(00'00 → --'--)
//更新：2012/09/21 TMEJ 小澤 【SERVICE_2】SAストール予約受付機能開発（仕分けNo.35）
//更新：2012/11/02 TMEJ 小澤 【SERVICE_2】サービス業務管理機能開発
//更新：2012/11/14 TMEJ 小澤 【SERVICE_2】次世代サービス アクティブインジゲータ対応 No.1
//更新：2012/11/21 TMEJ 丁   【SERVICE_2】SAストール予約受付機能開発（仕分けNo.72）
//更新：2012/12/06 TMEJ 小澤  問連対応「GTMC121204094」
//更新：2012/12/19 TMEJ 河原 【SERVICE_2】次世代サービスROステータス切り離し対応
//更新：2013/03/12 TMEJ 丁 【A.STEP1】来店受付管理オペレーション確立に向けた評価アプリ作成
//更新：2013/06/03 TMEJ 河原 【A.STEP2】次世代e-CRBタブレット　新DB適応に向けた機能設計
//更新：2013/12/10 TMEJ 河原 次世代サービス 工程管理機能開発
//更新：2017/10/13 NSK 竹中(悠) REQ-SVT-TMT-20170227-002 見込納車遅れ日時計算ロジック統一
//更新：2018/03/02 NSK 皆川 17PRJ02362_（トライ店システム評価）次世代サービスオペレーションにおける、サービス業務完了チェックタイミングの適合性検証
//更新：2018/06/15 NSK 坂本 17PRJ03047-02 CloseJob機能の開発に伴うボタン追加及びフレームの作成
//更新：2018/06/20 NSK 可児 TKM Next Gen e-CRB Project Application development Block B-1  Pマーク、L マークなどを表示
//更新：2019/07/18 NSK 鈴木 [TKM]PUAT-4072 SAがRO起票ボタンを押下するとくるくる表示が止まらない
//更新
//------------------------------------------------------------------------------

// 定数
var TOUCH_START = "mousedown touchstart";
var TOUCH_MOVE = "touchmove mousemove";
var TOUCH_END = "touchend mouseup";
var DBL_TAP_INTERVAL = 200;

//カウンター対応
var MAX_PROC_TIME = 100 * 60000;
var MIN_PROC_TIME = -100 * 60000;
// スライドアニメーション速度
var SCROLLSPEED = '300';

//現在選択中のチップ
var nowSelectArea = null;
var detailsArea = 0;
var detailsVisitNo = 0;
var detailsOrderNo = '';
var detailsApprovalId = '';
var detailsRezId = '';
//2013/03/12 TMEJ 丁 【A.STEP1】来店受付管理オペレーション確立に向けた評価アプリ作成　START
var detailsCallStatus = '';
//2013/03/12 TMEJ 丁 【A.STEP1】来店受付管理オペレーション確立に向けた評価アプリ作成　END

//2013/12/10 TMEJ 河原 次世代サービス 工程管理機能開発 START

//VISIT_MANAGEMENT更新日時(排他制御用)
var detailsUpDateDate = '';

//ポップアップの表示ポジション
var popOrverPosition = '';

//三角アイコンの表示位置
var trianglePosition = '';
var triangleRotate = '';
var triangleDeleteRotate = '';
var trianglePositionY = 0;
var triangleRotateLeft = "triangleRotateLeft";
var triangleRotateRigth = "triangleRotateRigth";

//2013/12/10 TMEJ 河原 次世代サービス 工程管理機能開発 END

//非同期処理のトリガーコントロール名称格納配列
var aryPostCtrl = new Array();
//2012/11/21 TMEJ 丁   【SERVICE_2】SAストール予約受付機能開発（仕分けNo.72） START
var countResult =0;
var color = 0;
//2012/11/21 TMEJ 丁   【SERVICE_2】SAストール予約受付機能開発（仕分けNo.72） END
//事前準備ボタン制御用フラグ
var flgFooterCtrlRight = true;
var flgTouchStart = false;
var slideDownFlag = false;
// 顧客検索機能用
var selectSearchTypeIndex = 0;
var resetVisitNumber = "";
var resetArea = "";
var flagChipChanges = false;
var CustomerClearFlag = false;
var slideCount = 0;
//2012/11/14 TMEJ 小澤 【SERVICE_2】次世代サービス アクティブインジゲータ対応 No.1 START
// 再描画フラグ
var RefreshFlag = true;
//2012/11/14 TMEJ 小澤 【SERVICE_2】次世代サービス アクティブインジゲータ対応 No.1 END
//2013/03/12 TMEJ 丁 【A.STEP1】来店受付管理オペレーション確立に向けた評価アプリ作成　START
// タイムアウト変数
var timeoutTimer = null;
//2013/03/12 TMEJ 丁 【A.STEP1】来店受付管理オペレーション確立に向けた評価アプリ作成　END

//標準ボタンステータスAttributes名
var SubMenuButtonStatusCalss = "SubMenuButtonStatus"

//2ボタン標準ボタン左右判定Attributes名
var ButtonStatus = "BtnStatus"

//選択中チップ情報
var SelectedChipInfo = null;

// 2019/07/18 NSK 鈴木 [TKM]PUAT-4072 SAがRO起票ボタンを押下するとくるくる表示が止まらない START
// 画面更新中フラグ
// true:画面更新中、false：画面更新中ではない
var gUpdatingFlag = false;
// 2019/07/18 NSK 鈴木 [TKM]PUAT-4072 SAがRO起票ボタンを押下するとくるくる表示が止まらない END

// -------------------------------------------------------------
// 独自イベント定義
// -------------------------------------------------------------
// チップタップイベント
// ・ドラッグ時は動作しない
// ・ダブルタップ時間内に再度タップがあれば動作しない
jQuery.event.special.chipTap = {
    setup: (function () {
        return function () {

            var touchStart = false;
            var touchMove = false;
            var singleTap = false;

            $(this).bind(TOUCH_START, function (event) {
                if (event.type == 'touchstart') {
                    flgTouchStart = true;
                } else {
                    if (flgTouchStart) {
                        return;
                    }
                }
                touchStart = true;
                touchMove = false;
                singleTap = !singleTap;

            });

            $(this).bind(TOUCH_MOVE, function (event) {
                if (event.type == 'touchmove') {
                } else {
                    if (flgTouchStart) {
                        return;
                    }
                }
                if (!touchStart) {
                    return;
                }

                touchMove = true;
                //タッチムーブ後にダブルタップした際、chipTap処理をしないよう制御
                singleTap = false;
            });

            $(this).bind(TOUCH_END, function (event) {
                if (event.type == 'touchend') {
                } else {
                    if (flgTouchStart) {
                        return;
                    }
                }
                if (!touchStart) {
                    return;
                }

                if (touchMove) {
                    return;
                }

                touchStart = false;
                touchMove = false;

                var obj = $(this);
                obj.trigger("chipTap");
            });

        }
    })()
}

function inputChanged() {
    if ($("#txtRegNo").val().trim().length > 0) {
        $("#ButtonRegister").attr('disabled', false);
        $("#ButtonRegister").removeAttr('disabled');
    } else {
        $('#ButtonRegister').attr('disabled', true);
    }
}

// -------------------------------------------------------------
// メイン処理
// -------------------------------------------------------------
//DOMロード時の処理

$(function () {

    //2012/11/14 TMEJ 小澤 【SERVICE_2】次世代サービス アクティブインジゲータ対応 No.1 START
    //タイマーセット
    commonRefreshTimer(RefreshDisplay);
    //2012/11/14 TMEJ 小澤 【SERVICE_2】次世代サービス アクティブインジゲータ対応 No.1 END

    //2018/06/15 NSK 坂本 17PRJ03047-02 CloseJob機能の開発に伴うボタン追加及びフレームの作成 START
    $('#FooterButton999').bind("click", function () {

        //ボタン背景点灯
        $('#FooterButton999').addClass("icrop-pressed");
        setTimeout(function () {
            //ボタン背景を戻す
            $('#FooterButton999').removeClass("icrop-pressed");
            $('#OtherjobDummyButton').click();
        }, 500);

        event.stopPropagation();
    });
    //2018/06/15 NSK 坂本 17PRJ03047-02 CloseJob機能の開発に伴うボタン追加及びフレームの作成 END

    $("#txtRegNo").bind('input', inputChanged);

    //受付登録バインド情報セット
    $("#ButtonRegister").bind("touchstart click", function (event) {
        //2012/11/14 TMEJ 小澤 【SERVICE_2】次世代サービス アクティブインジゲータ対応 No.1 START
        //タイマーセット
        commonRefreshTimer(RefreshDisplay);
        //2012/11/14 TMEJ 小澤 【SERVICE_2】次世代サービス アクティブインジゲータ対応 No.1 END
        $("#hfRegNo").blur();
        $("#hfRegNo").val($("#txtRegNo").val().trim());

        // 2019/07/18 NSK 鈴木 [TKM]PUAT-4072 SAがRO起票ボタンを押下するとくるくる表示が止まらない START
        // 画面更新中フラグをtrueにする
        setUpdatingFlag(true);
        // 2019/07/18 NSK 鈴木 [TKM]PUAT-4072 SAがRO起票ボタンを押下するとくるくる表示が止まらない END

        //2012/11/14 TMEJ 小澤 【SERVICE_2】次世代サービス アクティブインジゲータ対応 No.1 START
        //クルクルを一番上のINDEXにする
        setTimeout(function () {
            $('#MstPG_LoadingScreen').attr("style", "z-index:100001");
            $.master.OpenLoadingScreen();
        }, 600);
        //2012/11/14 TMEJ 小澤 【SERVICE_2】次世代サービス アクティブインジゲータ対応 No.1 END
    });
    //2013/03/12 TMEJ 丁 【A.STEP1】来店受付管理オペレーション確立に向けた評価アプリ作成　START
    //フッター「来店管理」クリック時の動作
    $('.InnerBox01').bind(TOUCH_START, function (event) {
        //ボタン背景点灯
        $('.InnerBox01').addClass("icrop-pressed");

        // 2019/07/18 NSK 鈴木 [TKM]PUAT-4072 SAがRO起票ボタンを押下するとくるくる表示が止まらない START
        // 画面更新中フラグをtrueにする
        setUpdatingFlag(true);
        // 2019/07/18 NSK 鈴木 [TKM]PUAT-4072 SAがRO起票ボタンを押下するとくるくる表示が止まらない END

        //クルクル表示
        $.master.OpenLoadingScreen();
        setTimeout(function () {
            //ボタン背景を戻す
            $('.InnerBox01').removeClass("icrop-pressed");
            //タイマーセット
            commonRefreshTimer(function () { __doPostBack("", ""); });
            //画面遷移イベント実行
            $("#VisitManagementFooterButton").click();
        }, 500);
        event.stopPropagation();
    });
    //2013/03/12 TMEJ 丁 【A.STEP1】来店受付管理オペレーション確立に向けた評価アプリ作成　END

    //2013/12/10 TMEJ 河原 次世代サービス 工程管理機能開発 START

    //フッター「顧客詳細ボタン」クリック時の動作
    $('#MstPG_FootItem_Main_700').bind("click", function (event) {

        $('#MstPG_CustomerSearchTextBox').focus();

        event.stopPropagation();
    });

    //2013/12/10 TMEJ 河原 次世代サービス 工程管理機能開発 END

    // 2019/07/18 NSK 鈴木 [TKM]PUAT-4072 SAがRO起票ボタンを押下するとくるくる表示が止まらない START
    // 通知リフレッシュボタン(隠しボタン)クリック時の動作
    $('#MainPolling').bind("click", function (event) {
        // 画面更新中フラグをtrueにする
        setUpdatingFlag(true);
    });
    // 2019/07/18 NSK 鈴木 [TKM]PUAT-4072 SAがRO起票ボタンを押下するとくるくる表示が止まらない END

    //フッターアプリの起動設定
    SetFutterApplication();
    //工程管理エリアのチップ表示処理
    $("#MainPolling").click();
    //非同期処理のトリガーコントロール名称格納
    aryPostCtrl.push("MainPolling");
    // UpdatePanel処理前後イベント
    $(document).ready(function () {
        var prm = Sys.WebForms.PageRequestManager.getInstance();

        // 開始時のイベント
        prm.add_beginRequest(function () {
        });
        // 終了時のイベント
        prm.add_endRequest(EndRequest);
        function EndRequest(sender, args) {
            if (document.getElementById("SASelector").value == icropScript.ui.account) {
                //2012/11/21 TMEJ 丁   【SERVICE_2】SAストール予約受付機能開発（仕分けNo.72） START
                countResult = document.getElementById("AdvancePreparationsCntHidden").value;
                color = document.getElementById("AdvancePreparationsColorHidden").value;
                //2012/11/21 TMEJ 丁   【SERVICE_2】SAストール予約受付機能開発（仕分けNo.72） END
            }
            if (aryPostCtrl.length > 0) {
                // トリガー配列を取得、削除
                var postBackControl = aryPostCtrl.shift();

                // UpdatePanel更新後処理を行うか判定
                if (IsJudgeEndRequest(postBackControl) == false) {
                    return;
                }

                // 呼び出し元判定（非同期処理のトリガーコントロール名称確認）
                switch (postBackControl) {
                    case 'MainPolling':         // 更新

                        //2013/12/10 TMEJ 河原 次世代サービス 工程管理機能開発 START

                        //受付エリア
                        //$('#flickable1').flickable();
                        //追加承認エリア
                        //$('#flickable2').flickable();
                        //納車準備エリア
                        //$('#flickable3').flickable();
                        //納車作業エリア
                        //$('#flickable4').flickable();
                        //作業中エリア
                        //$('#flickable5').flickable();

                        $("div.ColumnContentsFlameIn").fingerScroll();

                        //ツールチップを特定のところのみ未使用にする
                        $("#SearchCancel, #ChipChanges, #SelectRegNo, #SelectVin, #SelectName, #SelectTelNo").CustomLabelEx({ useEllipsis: false });

                        //･･･の設定(タイトル)
                        $(".Ellipsis").CustomLabelEx({ useEllipsis: true });

                        //2013/12/10 TMEJ 河原 次世代サービス 工程管理機能開発 END


                        //2012/11/21 TMEJ 丁   【SERVICE_2】SAストール予約受付機能開発（仕分けNo.72） START
                        $('ctl00_ctl00_content_SC3010201rightBox_SASelector_popOver_content').flickable();
                        //2012/11/21 TMEJ 丁   【SERVICE_2】SAストール予約受付機能開発（仕分けNo.72） END

                        // ポップオーバーの再設定
                        //SetPoover()
                        $('.CustomerChipRight, .CustomerChipLeft, .CustomerChipTop').bind('chipTap', function () {
                            if ($("#CustomerPopOver").css('display') === 'block') {
                                CloseAdvancePreparations();
                            }

                            // チップ選択(解除あり)
                            SetChipCheck(this);
                        });

                        // チップ選択クリア
                        ClearChip();

                        //2013/12/10 TMEJ 河原 次世代サービス 工程管理機能開発 START

                        // 工程管理ボックスの読み込み中アイコン停止                        
                        $('#loadingSchedule').hide(0);

                        //2013/12/10 TMEJ 河原 次世代サービス 工程管理機能開発 END

                        // 事前準備ボタンの件数表示、およびアイコン種別の設定
                        $('#AdvancePreparationsCnt').text($('#AdvancePreparationsCntHidden').val());

                        //アイコンステータス
                        var ButtonStatus = $('#AdvancePreparationsColorHidden').val();

                        //アイコン種別の設定
                        if (ButtonStatus === "1") {
                            $('#AdvancePreparationsButton').css('background', 'url(../Styles/Images/SC3140103/saAdvancePreparatironsActive.png) no-repeat');
                            $('.AdvancePreparationsName').css('color', '#FFF');
                        } else if (ButtonStatus === "2") {
                            $('#AdvancePreparationsButton').css('background', 'url(../Styles/Images/SC3140103/saAdvancePreparatironsRedActive.png) no-repeat');
                            $('.AdvancePreparationsName').css('color', '#FFF');
                        } else {
                            $('#AdvancePreparationsButton').css('background', 'url(../Styles/Images/SC3140103/saAdvancePreparatironsDeactive.png) no-repeat');
                            $('.AdvancePreparationsName').css('color', '#666');
                        };

                        // 事前準備ボタン制御解除
                        flgFooterCtrlRight = false;

                        if (resetArea === "" || resetVisitNumber === "") {

                            // 顧客付替えによる再表示の場合、チップ選択（チップ詳細表示）処理を行う

                        } else {

                            //2013/12/10 TMEJ 河原 次世代サービス 工程管理機能開発 START

                            //receptionArray = $('.ColumnBox01').find('.CustomerChipRight');
                            receptionArray = $('.ColumnBox02').find('.CustomerChipRight');

                            //2013/12/10 TMEJ 河原 次世代サービス 工程管理機能開発 END

                            receptionArray.each(function () {
                                item = $(this).children('#ReceptionDeskDevice')
                                visitNumber = item.attr('visitNo');
                                if (resetVisitNumber === visitNumber) {
                                    // チップ選択(解除あり)
                                    SetChipCheck(this);
                                    resetArea = "";
                                    resetVisitNumber = "";

                                    // 2019/07/18 NSK 鈴木 [TKM]PUAT-4072 SAがRO起票ボタンを押下するとくるくる表示が止まらない START
                                    // 画面更新中フラグをfalseにする
                                    setUpdatingFlag(false);
                                    // 2019/07/18 NSK 鈴木 [TKM]PUAT-4072 SAがRO起票ボタンを押下するとくるくる表示が止まらない END

                                    return false;
                                }
                            });
                        };

                        proccounter(diffseconds);

                        //2013/12/10 TMEJ 河原 次世代サービス 工程管理機能開発 START

                        setTimeout(function () { $(".WhatNewDisableDiv").css("display", "none"); }, 1000);

                        //2013/12/10 TMEJ 河原 次世代サービス 工程管理機能開発 END

                        // 2019/07/18 NSK 鈴木 [TKM]PUAT-4072 SAがRO起票ボタンを押下するとくるくる表示が止まらない START
                        // 画面更新中フラグをfalseにする
                        setUpdatingFlag(false);
                        // 2019/07/18 NSK 鈴木 [TKM]PUAT-4072 SAがRO起票ボタンを押下するとくるくる表示が止まらない END

                        break;

                    case 'DetailPopupButton':   // チップ詳細

                        // 納車時刻の変更回数が0以外の場合
                        if ($("#HiddenDeliveryPlanUpdateCount").val() != "0") {

                            $(".StatusInfoInnaerDataBoxDiv").click(function () {
                                if (slideDownFlag) {
                                    $("#HeadInfomationPullDiv").slideUp();
                                    $('.DetailFlickableBox .scroll-inner').css({
                                        'transform': 'translate3d(0px, 0px, 0px)',
                                        '-webkit-transition': 'transform 400ms'
                                    });
                                    slideDownFlag = false;
                                } else {
                                    $("#HeadInfomationPullDiv").slideDown();
                                    slideDownFlag = true;
                                }
                            });
                        } else {
                            //変更回数が0の場合は非表示
                            $("#FixDownArrow").css("display", "none");
                            $("#FixSlashLabel").css("display", "none");
                            $("#ChangeCountLabel").css("display", "none");
                        };

                        //2013/12/10 TMEJ 河原 次世代サービス 工程管理機能開発 START

                        // 受付エリアのチップ選択時は顧客検索ボタンを表示     
                        //if (detailsArea == 1){                
                        if (detailsArea == 1 || detailsArea == 7) {

                            //2013/12/10 TMEJ 河原 次世代サービス 工程管理機能開発 END

                            CallPlaceSave();

                            $('.SearchIcon').css('display', 'block');
                            $('#ItemRegistrationNumber').css('padding-top', '4px');

                            if ($('.TextArea').text() == "") {
                                $('.TextArea').attr('value', $.trim($('#DetailsRegistrationNumber').text()));
                            };

                            $('.DetailDeleteFooterBox').css('display', 'block');

                        } else {

                            $('.SearchIcon').css('display', 'none');

                            $('#ItemRegistrationNumber').css('padding-top', '0');

                            $('.DetailDeleteFooterBox').css('display', 'none');
                        };

                        // 2018/03/02 NSK 皆川 17PRJ02362_（トライ店システム評価）次世代サービスオペレーションにおける、サービス業務完了チェックタイミングの適合性検証 START

                        // 納車準備エリア選択時はチェックシートプレビューボタンを表示
                        if (detailsArea == 3 && $('#DetailbottomDiv04').css('display') === 'block') {

                            $('#DetailBottomBox').css('height', '104px');
                            $('#DetailbottomDiv').css('top', '57px');
                            $('#DetailbottomDiv02').css('top', '57px');
                            $('#DetailbottomDiv03').css('top', '57px');
                        }

                        // 2018/03/02 NSK 皆川 17PRJ02362_（トライ店システム評価）次世代サービスオペレーションにおける、サービス業務完了チェックタイミングの適合性検証 END

                        //2013/12/10 TMEJ 河原 次世代サービス 工程管理機能開発 START

                        // 2012/12/19 TMEJ 河原 【SERVICE_2】次世代サービスROステータス切り離し対応 START
                        // 作業中エリアかつフラグの判定
                        //if (detailsArea == 5 && $("#HiddenDetailsInspectionButtonStatus").val() == "1") {
                        //// 完成検査承認ボタン表示
                        //$('#DetailButtonInspection').css({ 'display': 'inline', 'width': '63px' });
                        //$("#DetailButtonInspection").addClass("FooterButton04_on");
                        //$("#DetailButtonLeft").css({ 'width': '63px' });
                        //$("#DetailButtonCenter").css({ 'width': '63px', 'left': '75px' });
                        //$("#DetailButtonRight").css({ 'width': '63px', 'left': '150px' });
                        //} else {
                        // 完成検査承認ボタン非表示
                        //$("#DetailButtonInspection").removeClass("FooterButton04_on");
                        //$('#DetailButtonInspection').css('display', 'none');
                        //$("#DetailButtonLeft").css({ 'width': '88px', 'left': '0px' });
                        //$("#DetailButtonCenter").css({ 'width': '88px', 'left': '100px' });
                        //$("#DetailButtonRight").css({ 'width': '88px', 'left': '', 'right': '0px' });
                        //};
                        // 2012/12/19 TMEJ 河原 【SERVICE_2】次世代サービスROステータス切り離し対応 END

                        //2013/12/10 TMEJ 河原 次世代サービス 工程管理機能開発 END

                        // 顧客情報サブボタンの活性・非活性を設定
                        if ($("#HiddenDetailsCustomerButtonStatus").val() == "0") {
                            //非活性
                            $("#DetailButtonLeft").removeClass("FooterButton01_on").addClass("FooterButton01_off");
                        } else {
                            //活性
                            $("#DetailButtonLeft").removeClass("FooterButton01_off").addClass("FooterButton01_on");
                        };

                        //	R/Oサブボタンの活性・非活性を設定
                        if ($("#HiddenDetailsROButtonStatus").val() == "0") {
                            //非活性
                            $("#DetailButtonRight").removeClass("FooterButton02_on").addClass("FooterButton02_off");
                        } else {
                            //活性
                            $("#DetailButtonRight").removeClass("FooterButton02_off").addClass("FooterButton02_on");
                        };

                        //2013/12/10 TMEJ 河原 次世代サービス 工程管理機能開発 START

                        // R/Oサブボタンの活性・非活性を設定
                        //if ($("#HiddenDetailsROButtonStatus").val() == "0") {
                        //非活性
                        //$("#DetailButtonCenter").removeClass("FooterButton02_on").addClass("FooterButton02_off");
                        //} else {
                        //活性
                        //$("#DetailButtonCenter").removeClass("FooterButton02_off").addClass("FooterButton02_on");
                        //}

                        //	追加作業サブボタンの活性・非活性を設定
                        //if ($("#HiddenDetailsApprovalButtonStatus").val() == "0") {
                        //非活性
                        //$("#DetailButtonRight").removeClass("FooterButton03_on").addClass("FooterButton03_off");
                        //} else {
                        //活性
                        //$("#DetailButtonRight").removeClass("FooterButton03_off").addClass("FooterButton03_on");
                        //}

                        //2013/12/10 TMEJ 河原 次世代サービス 工程管理機能開発 END

                        //チップ詳細スクロール設定時のHeight微調整
                        if ($('#DetailBottomBox').css('display') === 'none') {

                            $('.DetailFlickableBox').css('height', '559px');

                        } else {

                            //2013/12/10 TMEJ 河原 次世代サービス 工程管理機能開発 START

                            //$('.DetailFlickableBox').css('height', '497px');
                            $('.DetailFlickableBox').css('height', '497px');

                            //2013/12/10 TMEJ 河原 次世代サービス 工程管理機能開発 END


                        };

                        $("#FixDeliveryTimeLabel").css("display", "inline-block");
                        $("#FixSlashLabel").css("display", "inline-block");
                        $("#FixDeliveryEstimateLabel").css("display", "inline-block");

                        //チップ詳細の画面遷移ボタン表示
                        toggleRegularDetailButtonControl(false);
                        $('#IconLoadingPopup').hide(0);
                        $(".DetailFlickableBox").fingerScroll();

                        //2013/12/10 TMEJ 河原 次世代サービス 工程管理機能開発 START
                        //･･･の設定(タイトル)
                        $(".Ellipsis").CustomLabelEx({ useEllipsis: true });

                        //2013/12/10 TMEJ 河原 次世代サービス 工程管理機能開発 END

                        break;

                    case 'AdvancePreparationsClick':  // 事前準備ボタン

                        $('#LoadAdvancePreparations').hide(0);

                        // 事前準備ボタンの件数表示、およびアイコン種別の設定
                        $('#AdvancePreparationsCnt').text($('#AdvancePreparationsCntHidden').val());

                        var ButtonStatus = $('#AdvancePreparationsColorHidden').val();

                        if (ButtonStatus === "1") {

                            $('#AdvancePreparationsButton').css('background', 'url(../Styles/Images/SC3140103/saAdvancePreparatironsActive.png) no-repeat');
                            $('.AdvancePreparationsName').css('color', '#FFF');

                        } else if (ButtonStatus === "2") {

                            $('#AdvancePreparationsButton').css('background', 'url(../Styles/Images/SC3140103/saAdvancePreparatironsRedActive.png) no-repeat');
                            $('.AdvancePreparationsName').css('color', '#FFF');

                        } else {

                            $('#AdvancePreparationsButton').css('background', 'url(../Styles/Images/SC3140103/saAdvancePreparatironsDeactive.png) no-repeat');
                            $('.AdvancePreparationsName').css('color', '#666');

                        };


                        $('.CustomerChipFooter').bind('chipTap', function () {
                            // チップ選択(解除あり)
                            SetChipCheck(this);
                        });

                        $("#flickableF").flickable();

                        break;

                    case 'SearchCustomer':  // 顧客検索ボタン

                        $(".SearchDataBox").fingerScroll();


                        if ($('#SearchSelectTypeHidden').val() === "1") {
                            // 次のN件を選択時
                            $('.NextSearchingImage').css('display', 'none');
                            $('.NextListSearching').css('display', 'none');

                            $('.SearchDataBox .scroll-inner').css(
                                'transform', 'translate3d(0px, -' + $('#ScrollPositionHidden').val() + 'px, 0px)'
                            );


                        } else if ($('#SearchSelectTypeHidden').val() === "-1") {
                            // 前のN件を選択時
                            $('.FrontSearchingImage').css('display', 'none');
                            $('.FrontListSearching').css('display', 'none');
                            $('.SearchDataBox .scroll-inner').css(
                                'transform', 'translate3d(0px, -' + $('#ScrollPositionHidden').val() + 'px, 0px)'
                            );

                        } else {

                            // 初回検索時
                            $('#SearchDataLoading').css('display', 'none');
                        };

                        if (0 < $('#SearchListBox').children('li').length) {

                            $('#SearchBottomButton').removeClass('BottomButtonDisable');
                            CustomerClearFlag = true;

                        } else {

                            $('#SearchBottomButton').addClass('BottomButtonDisable');
                            CustomerClearFlag = false;
                        };

                        $('.SearchData').bind('click', function (event) {
                            $('.SearchData').removeClass('SelectionOn');
                            $(this).addClass('SelectionOn');
                            $('#SearchRegistrationNumberChange').val($(this).children('#SearchRegistrationNumber').text());
                            $('#SearchVinChange').val($(this).children('#SearchVinNo').text());
                            $('#SearchCustomerNameChange').val($(this).children('#SearchCustomerName').text());
                            $('#SearchPhoneChange').val($(this).children('#SearchPhone').text());
                            $('#SearchMobileChange').val($(this).children('#SearchMobile').text());

                            var ChangeParameterDiv = $(this).children('#CustomerChangeParameter');

                            $('#SearchCustomerCodeChange').val(ChangeParameterDiv.attr("CustomerCodeParameter"));
                            $('#SearchDMSIdChange').val(ChangeParameterDiv.attr("DmsIdParameter"));
                            $('#SearchModelChange').val(ChangeParameterDiv.attr("ModelParameter"));
                            $('#SearchSACodeChange').val(ChangeParameterDiv.attr("SacodeParameter"));

                            //2013/12/10 TMEJ 河原 次世代サービス 工程管理機能開発 START

                            //$('#ChipChanges').addClass('ButtonRightOn');
                            $('#ButtonRight').addClass('ButtonRightOn');

                            //2013/12/10 TMEJ 河原 次世代サービス 工程管理機能開発 END

                            flagChipChanges = true;
                        });

                        break;

                    case 'BeforeChipChanges':  // 顧客付替えボタン（付替え前処理）

                        checkResult = $('#ChipResultChange').val();

                        if (checkResult === "0") {

                        }
                        else if (checkResult === "101") {
                            checkMessage = $('#ChipConfirmChange').val();
                            selectConfirm = confirm(checkMessage);
                            if (selectConfirm) {
                            } else {
                                //2012/11/14 TMEJ 小澤 【SERVICE_2】次世代サービス アクティブインジゲータ対応 No.1 START
                                //$.master.CloseLoadingScreen();
                                //2012/11/14 TMEJ 小澤 【SERVICE_2】次世代サービス アクティブインジゲータ対応 No.1 END
                                //2013/03/12 TMEJ 丁 【A.STEP1】来店受付管理オペレーション確立に向けた評価アプリ作成　START
                                $.master.CloseLoadingScreen();
                                //2013/03/12 TMEJ 丁 【A.STEP1】来店受付管理オペレーション確立に向けた評価アプリ作成　END

                                // 2019/07/18 NSK 鈴木 [TKM]PUAT-4072 SAがRO起票ボタンを押下するとくるくる表示が止まらない START
                                // 画面更新中フラグをfalseにする
                                setUpdatingFlag(false);
                                // 2019/07/18 NSK 鈴木 [TKM]PUAT-4072 SAがRO起票ボタンを押下するとくるくる表示が止まらない END

                                break;
                            };
                        }
                        else {
                            //2012/11/14 TMEJ 小澤 【SERVICE_2】次世代サービス アクティブインジゲータ対応 No.1 START
                            //$.master.CloseLoadingScreen();
                            //2012/11/14 TMEJ 小澤 【SERVICE_2】次世代サービス アクティブインジゲータ対応 No.1 END
                            //2013/03/12 TMEJ 丁 【A.STEP1】来店受付管理オペレーション確立に向けた評価アプリ作成　START
                            $.master.CloseLoadingScreen();
                            //2013/03/12 TMEJ 丁 【A.STEP1】来店受付管理オペレーション確立に向けた評価アプリ作成　END

                            // 2019/07/18 NSK 鈴木 [TKM]PUAT-4072 SAがRO起票ボタンを押下するとくるくる表示が止まらない START
                            // 画面更新中フラグをfalseにする
                            setUpdatingFlag(false);
                            // 2019/07/18 NSK 鈴木 [TKM]PUAT-4072 SAがRO起票ボタンを押下するとくるくる表示が止まらない END

                            break;
                        };

                        aryPostCtrl.push('ChipChanges');

                        $('#ChipChangesDummyButton').click();

                        break;

                    case 'ChipChanges':  // 顧客付替えボタン


                        //2013/12/10 TMEJ 河原 次世代サービス 工程管理機能開発 START

                        $(".TipBlackOut").css('background', '');

                        //2013/12/10 TMEJ 河原 次世代サービス 工程管理機能開発 END

                        resetArea = $('#DetailsArea').val();
                        resetVisitNumber = $('#DetailsVisitNo').val();
                        $.master.CloseLoadingScreen();

                        // 2019/07/18 NSK 鈴木 [TKM]PUAT-4072 SAがRO起票ボタンを押下するとくるくる表示が止まらない START
                        // 画面更新中フラグをfalseにする
                        setUpdatingFlag(false);
                        // 2019/07/18 NSK 鈴木 [TKM]PUAT-4072 SAがRO起票ボタンを押下するとくるくる表示が止まらない END

                        MainRefresh();

                        break;

                    case 'AssignmentRefreshButton':  // 振当待ちリフレッシュボタン


                        $("div#AssignmentArea").fingerScroll();

                        //･･･の設定(タイトル)
                        $(".Ellipsis").CustomLabelEx({ useEllipsis: true });


                        //SetPoover()
                        $('div#Assignment').bind('chipTap', function () {
                            if ($("#CustomerPopOver").css('display') === 'block') {
                                CloseAdvancePreparations();
                            }

                            // チップ選択(解除あり)
                            SetChipCheck(this);
                        });

                        //選択中のチップがあるかをチェック
                        if (SelectedChipInfo != null && SelectedChipInfo != undefined) {
                            //存在する場合
                            //チップ一つ一つにグレーフィルタをかける
                            $(".TipBlackOut").css('background', 'rgba(0,0,0,0.4)');
                            //選択チップのみグレーフィルタをはずす
                            $(SelectedChipInfo).children(".TipBlackOut").css('background', '');
                        }

                        break;

                    default:

                        break;

                };

                //フッター制御フラグを戻す
                flgFooterCtrl = true;
            };
            //2012/06/12 西岡 事前準備対応 END
        };
    });
    $("#SearchText").keydown(function (e) {
        if (e.which == 13) {
            SearchCustomer();
            $("#SearchText").blur();
            return false;
        }
    });
});

/*
* UpdatePanel更新後処理 実行判定
*
* @param {string} postBackCtrl 処理実行フラグ名
* @return {boolean} true:処理を実行 / false:処理をキャンセル
*/
function IsJudgeEndRequest(postBackCtrl) {

    var rtn = true;

    // 同フラグがある場合は処理しない。（最後のフラグで処理）
    if (aryPostCtrl.length > 0) {
        var str = "";
        for (var i = 0; i < aryPostCtrl.length; i++) {
            str = aryPostCtrl[i];

            if (postBackCtrl == str) {
                // 一致した場合処理しない
                rtn = false;
                break;
            }
        }
    }

    return rtn;
}


// チップ選択チェック
function SetChipCheck(chip) {

    //2013/03/12 TMEJ 丁 【A.STEP1】来店受付管理オペレーション確立に向けた評価アプリ作成　START

    // search div未だ表示されない場合、他のチップをタップする時、search divを表示する必要がない
    if (timeoutTimer) {
        clearTimeout(timeoutTimer);
    };

    //2013/03/12 TMEJ 丁 【A.STEP1】来店受付管理オペレーション確立に向けた評価アプリ作成　END

    //2013/12/10 TMEJ 河原 次世代サービス 工程管理機能開発 START

    //Whats Newを閉じる
    WhatsNewClose();

    //2013/12/10 TMEJ 河原 次世代サービス 工程管理機能開発 END

    
    // チップ詳細設定
    SetDetail(chip);

    // 2012/07/20 西岡 顧客検索機能追加 START
    if (slideDownFlag) {

        $("#HeadInfomationPullDiv").slideUp();
        $('.DetailFlickableBox .scroll-inner').css({ 'transform': 'translate3d(0px, 0px, 0px)',
            '-webkit-transition': 'transform 400ms'
        });

        slideDownFlag = false;
    };
    // 2012/07/20 西岡 顧客検索機能追加 END

    // 現在選択中チップ取得
    var area = $('#DetailsArea').val();
    var visitNo = $('#DetailsVisitNo').val();
    var orderNo = $('#DetailsOrderNo').val();
    var approvalId = $('#DetailsApprovalId').val();
    var rezId = $('#DetailsRezId').val();

    //2013/12/10 TMEJ 河原 次世代サービス 工程管理機能開発 START
    //if ($('.contentInner').css('left') != '0px') {
    if ($('.contentInner').css('transform') != 'translate3d(0px, 0px, 0px)') {

        //2013/12/10 TMEJ 河原 次世代サービス 工程管理機能開発 END

        if (slideCount == 0) {
            SlideStatus("clear");
        } else {
            SlideStatus("click");
        };

        $('.TextArea').blur();

    };

    searchListClear();

    //2013/12/10 TMEJ 河原 次世代サービス 工程管理機能開発 START

    //$("#SearchCancel").css('display', 'none');
    //$("#ChipChanges").css('display', 'none');

    $("#ButtonLeft").css('display', 'none');    
    $("#ButtonRight").css('display', 'none');

    //2013/12/10 TMEJ 河原 次世代サービス 工程管理機能開発 END


    //2012/11/14 TMEJ 小澤 【SERVICE_2】次世代サービス アクティブインジゲータ対応 No.1 START
    //タイマークリア
    commonClearTimerSA();
    //2012/11/14 TMEJ 小澤 【SERVICE_2】次世代サービス アクティブインジゲータ対応 No.1 END

    // 選択チップチェック
    if (area != 6 && detailsArea == area && detailsVisitNo == visitNo ||
			(area == 6 && detailsArea == area && detailsRezId == rezId)) {

        // 選択中チップの選択
        UnsetChip(chip);

    } else {
        
        // 選択外チップの選択
        SetChip(chip);

        // ポップオーバー消失
        //$(chip).trigger('hidePopover');

        aryPostCtrl.push("DetailPopupButton");

        //チップ詳細のサーバー処理中はフッターを制御する
        flgFooterCtrl = false;

        //ボタン制御
        toggleRegularDetailButtonControl(true);

        //チップ詳細クリア
        PopupDataClear();

        //くるくるだす（Update時に消える）
        $('#IconLoadingPopup').attr("style", "visibility: visible");
       
        //固定ポップオーバー出力
        $("#CustomerPopOver2").attr("style", "display: block;");

        //2013/12/10 TMEJ 河原 次世代サービス 工程管理機能開発 STAT

        //ポップアップオーバーの表示位置設定
        $("#CustomerPopOver2").css("left", (popOrverPosition + "px"));

        //選択されたチップの座標取得し三角アイコンの位置を計算
        var currentHeight = $(chip).offset().top - ($(chip).height() / 2) + 15 - trianglePositionY;

        //三角アイコンが領域外へいかないように制御
        //上段領域外かチェック
        if (currentHeight < 0) {

            //上段領域外の場合は7pxで固定
            currentHeight = 7;
        };

        //下段領域外かチェック
        if (564 < currentHeight) {

            //下段領域外の場合は560pxで固定
            currentHeight = 560;
        };

        //三角アイコンの表示位置設定
        //$("div.PoPuPArrowLeft1-1").css("top", currentHeight);
        //$("div.PoPuPArrowLeft1-1").css("left", trianglePosition);

        //三角アイコンの表示方向設定
        $("div.PoPuPArrowLeft1-1").removeClass(triangleDeleteRotate);
        $("div.PoPuPArrowLeft1-1").addClass(triangleRotate);


        //三角アイコンのスタイル微調整
        if (triangleRotate == triangleRotateLeft) {
            //三角アイコン表示方向設定(左側)

            $("div.PoPuPArrowLeft1-3").css("border-top", "1px solid rgba(255,255,255,0.25)");
            $("div.PoPuPArrowLeft1-3").css("border-left", "");

        } else {
            //三角アイコン表示方向設定(右側)

            $("div.PoPuPArrowLeft1-3").css("border-left", "1px solid rgba(255,255,255,0.25)");
            $("div.PoPuPArrowLeft1-3").css("border-top", "");

        };

        //三角アイコンの表示を綺麗に見せるため一度非表示にする
        $("div.PoPuPArrowLeft1-1").css("display", "none");

        //2013/12/10 TMEJ 河原 次世代サービス 工程管理機能開発 END

        
        //呼び出し詳細を表示させる(受付の場合)
        var strAreaNo = $('#DetailsArea').val();

        //2013/12/10 TMEJ 河原 次世代サービス 工程管理機能開発 START

        //受付モニター使用フラグ
        var useReception = $('#UseReception').val();

        //受付モニター使用フラグがON("1")の場合、呼出エリアを表示
        if (useReception == 1) {

            //if (strAreaNo == 1) {
            if (strAreaNo == 1 || strAreaNo == 7) {
                //振当待ちエリアと受付エリアのみ表示

                //2013/12/10 TMEJ 河原 次世代サービス 工程管理機能開発 END

                //呼び出しエリア
                $("#VisitCustomer").attr("style", "display: block;");
            };

        };

        //2012/11/14 TMEJ 小澤 【SERVICE_2】次世代サービス アクティブインジゲータ対応 No.1 START
        //タイマーセット
        commonRefreshTimer(RefreshDisplay);
        //2012/11/14 TMEJ 小澤 【SERVICE_2】次世代サービス アクティブインジゲータ対応 No.1 END

        $("#DetailPopupButton").click();

        //2013/03/12 TMEJ 丁 【A.STEP1】来店受付管理オペレーション確立に向けた評価アプリ作成　START
        //Divをスライドした後で、ずれるので、調整する
        AdjustDiv();
        //2013/03/12 TMEJ 丁 【A.STEP1】来店受付管理オペレーション確立に向けた評価アプリ作成　END

        //2013/12/10 TMEJ 河原 次世代サービス 工程管理機能開発 SATRT

        //三角アイコンの表示を綺麗に見せるため一度非表示したものを表示させる
        $("div.PoPuPArrowLeft1-1").css("display", "");

        //三角アイコンの表示位置設定
        $("div.PoPuPArrowLeft1-1").css("top", currentHeight);
        $("div.PoPuPArrowLeft1-1").css("left", trianglePosition);

        //画面全体のタッチイベントの重複設定を避けるため一旦イベント解除
        $('#bodyFrame').unbind(TOUCH_START, PopOverCloseCheck);

        setTimeout(function () {

            //画面全体にタッチイベント(タッチスタート)を設定
            $('#bodyFrame').bind(TOUCH_START, PopOverCloseCheck);

        }, 100);

        //画面全体にタッチイベント(タッチエンド)を設定
        $('#bodyFrame').bind(TOUCH_END, function () {

            //タッチが終了した場合にムーブイベントを解除する

            //タッチイベント(タッチムーブ：スクロール)を解除
            $(event.target).unbind(TOUCH_MOVE, MoveCheck);
        });

        //2013/12/10 TMEJ 河原 次世代サービス 工程管理機能開発 END

    };
};

//2013/12/10 TMEJ 河原 次世代サービス 工程管理機能開発 SATRT

function PopOverCloseCheck() {

    //タッチしたポイントがチップ詳細かの判定
    if (1 <= $(event.target).parents('.saPopOver2').length) {

        //タッチしたポイントがチップ詳細の中だった場合
        //処理無し(チップ詳細のクローズは行わない)
        return;
    };

    //タッチしたポイントが他のチップの上かチェック
    if (($(event.target).is(".CustomerChipRight, .TipBlackOut"))) {
        //タッチしたポイントが他のチップの上の場合

        //タッチムーブイベントの設定
        //工程エリアをスクロールした場合はチップ詳細をクローズするため
        $(event.target).bind(TOUCH_MOVE, MoveCheck);

        //イベントの内容チェック
        if ($(event.type).selector === "mousemove" || $(event.type).selector === "touchmove") {

            //タッチムーブイベントは上記で設定しているため処理無し

        } else {
            //上記以外
            //(タッチエンドイベント)

            //処理無し(チップ詳細のクローズは行わない)他のチップの詳細を表示するため
            return;
      
        };        
    };

    //チップ詳細が表示されているかチェック
    if ($("#CustomerPopOver2").css('display') === 'block') {
        //チップ詳細が表示されている場合

        //テキストのフォーカスをはずす
        $('.TextArea').blur();
        //選択チップの解除
        UnsetChip(this);
        //チップ詳細を閉じて閉じる
        CloseChipDetail();
    };


};

function MoveCheck() {

    //タッチムーブイベント処理

    //ムーブイベントを解除
    $(event.target).unbind(TOUCH_MOVE, MoveCheck);
    //テキストのフォーカスをはずす
    $('.TextArea').blur();
    //選択チップの解除
    UnsetChip(this);
    //チップ詳細を閉じて閉じる
    CloseChipDetail();

};

//2013/12/10 TMEJ 河原 次世代サービス 工程管理機能開発 END

// チップ選択
function SetChip(chip) {

    //2013/12/10 TMEJ 河原 次世代サービス 工程管理機能開発 STRAT
    // 選択状態のチップがあれば選択解除
    //if (nowSelectArea != null) {
        //$($(nowSelectArea).parent()).css('z-index', '0');
    // }

    //2013/12/10 TMEJ 河原 次世代サービス 工程管理機能開発 END

    // 選択されたチップの情報保持
    nowSelectArea = $(chip).children(':first');
    SelectedChipInfo = chip;


    //2013/12/10 TMEJ 河原 次世代サービス 工程管理機能開発 START

    //画面全体ににグレーフィルタをかける
    $('.BlackWindow').css('display', 'inline');
  
    // 選択チップのみグレー上に表示
    //$(chip).css('z-index', '999');

    //2013/12/10 TMEJ 河原 次世代サービス 工程管理機能開発 END

    //2013/12/10 TMEJ 河原 次世代サービス 工程管理機能開発 START

    //$(".TipBlackOut").css('display', 'inline');
    //$(chip).children(".TipBlackOut").css('display', 'none');

    //チップ一つ一つにグレーフィルタをかける
    $(".TipBlackOut").css('background', 'rgba(0,0,0,0.4)');
    //選択チップのみグレーフィルタをはずす
    $(chip).children(".TipBlackOut").css('background', '');
    //2013/12/10 TMEJ 河原 次世代サービス 工程管理機能開発 END

    // チップ詳細設定
    SetDetail(chip);
    $('#DetailsArea').val(detailsArea);
    $('#DetailsVisitNo').val(detailsVisitNo);
    $('#DetailsOrderNo').val(detailsOrderNo);
    $('#DetailsApprovalId').val(detailsApprovalId);
    $('#DetailsRezId').val(detailsRezId);
    $('#DetailsVisitUpdateDate').val(detailsUpDateDate);
    //2013/03/12 TMEJ 丁 【A.STEP1】来店受付管理オペレーション確立に向けた評価アプリ作成　START
    $('#DetailsCallStatus').val(detailsCallStatus);
    //2013/03/12 TMEJ 丁 【A.STEP1】来店受付管理オペレーション確立に向けた評価アプリ作成　END

}

// チップ選択解除
function UnsetChip(chip) {


    ///2013/12/10 TMEJ 河原 次世代サービス 工程管理機能開発 START

    //すべてのチップのグレーフィルタをはずす
    $(".TipBlackOut").css('background', '');
    //画面全体のグレーフィルタをはずす
    $('.BlackWindow').css('display', 'none');
    //選択チップ情報を削除
    SelectedChipInfo = null;

    ///2013/12/10 TMEJ 河原 次世代サービス 工程管理機能開発 END

    //$(chip).css('z-index', '0');

    // 選択されたチップの情報解除
    nowSelectArea = null;
    // チップ詳細設定解除
    ClearChip();
    // ポップオーバー消失
    CloseChipDetail();

    slideCount = 0;
}

// チップ選択クリア
function ClearChip() {
    // チップ詳細設定クリア
    detailsArea = 0;
    detailsVisitNo = 0;
    detailsOrderNo = '';
    detailsApprovalId = '';
    detailsRezId = '';

    ///2013/12/10 TMEJ 河原 次世代サービス 工程管理機能開発 START

    detailsUpDateDate = '';

    detailsRoInfoRowLockVersion = '';

    ///2013/12/10 TMEJ 河原 次世代サービス 工程管理機能開発 END


    //2013/03/12 TMEJ 丁 【A.STEP1】来店受付管理オペレーション確立に向けた評価アプリ作成　START
    detailsCallStatus = '';
    //2013/03/12 TMEJ 丁 【A.STEP1】来店受付管理オペレーション確立に向けた評価アプリ作成　END
    $('#DetailsArea').val(detailsArea);
    $('#DetailsVisitNo').val(detailsVisitNo);
    $('#DetailsOrderNo').val(detailsOrderNo);
    $('#DetailsApprovalId').val(detailsApprovalId);
    $('#DetailsRezId').val(detailsRezId);
    //2013/03/12 TMEJ 丁 【A.STEP1】来店受付管理オペレーション確立に向けた評価アプリ作成　START
    $('#DetailsCallStatus').val(detailsCallStatus);
    //2013/03/12 TMEJ 丁 【A.STEP1】来店受付管理オペレーション確立に向けた評価アプリ作成　END

    //2013/06/03 TMEJ 河原 【A.STEP2】次世代e-CRBタブレット　新DB適応に向けた機能設計 START
    $('#HiddenVehicleModel').val('');
    //2013/06/03 TMEJ 河原 【A.STEP2】次世代e-CRBタブレット　新DB適応に向けた機能設計 END

    ///2013/12/10 TMEJ 河原 次世代サービス 工程管理機能開発 START

    $('#DetailsVisitUpdateDate').val(detailsUpDateDate);


    $('#RoInfoRowLockVersion').val(detailsRoInfoRowLockVersion);

    //2013/12/10 TMEJ 河原 次世代サービス 工程管理機能開発 END


    ///2013/12/10 TMEJ 河原 次世代サービス 工程管理機能開発 START
    
    //検索エリアが表示されている場合は元に戻す
    $('.contentInner').css({
        "transform": ""
    });

    ///2013/12/10 TMEJ 河原 次世代サービス 工程管理機能開発 END

}

// チップ詳細設定
function SetDetail(chip) {
    var item;
    // エリア判定
    var area = $(chip).attr('id');
    switch (area) {
        case 'Reception':   // 受付
            // チップ情報設定
            detailsArea = 1;
            item = $(chip).children('#ReceptionDeskDevice')
            //2013/03/12 TMEJ 丁 【A.STEP1】来店受付管理オペレーション確立に向けた評価アプリ作成　START
            this.detailsCallStatus = item.attr('callStatus');
            //2013/03/12 TMEJ 丁 【A.STEP1】来店受付管理オペレーション確立に向けた評価アプリ作成　END

            //2013/12/10 TMEJ 河原 次世代サービス 工程管理機能開発 START
            //ポップアップの表示位置設定
            popOrverPosition = $('.ColumnBox03').offset().left;
            //三角アイコンの表示位置設定
            trianglePosition = "-21px";
            //三角アイコン表示方向設定(左側)
            triangleRotate = triangleRotateLeft;
            triangleDeleteRotate = triangleRotateRigth;
            //三角アイコンの表示位置の微調整
            trianglePositionY = 5;
            //2013/12/10 TMEJ 河原 次世代サービス 工程管理機能開発 END

            break;

        case 'Approval':    // 追加承認
            // チップ情報設定
            detailsArea = 2;
            item = $(chip).children('#ApprovalDeskDevice')

            //2013/12/10 TMEJ 河原 次世代サービス 工程管理機能開発 START
            //ポップアップの表示位置設定
            popOrverPosition = $('.ColumnBox04').offset().left;
            //三角アイコンの表示位置設定
            trianglePosition = "-21px";
            //三角アイコン表示方向設定(左側)
            triangleRotate = triangleRotateLeft;
            triangleDeleteRotate = triangleRotateRigth;
            //三角アイコンの表示位置の微調整
            trianglePositionY = 5;
            //2013/12/10 TMEJ 河原 次世代サービス 工程管理機能開発 END

            break;

        case 'Preparation': // 納車準備
            // チップ情報設定
            detailsArea = 3;
            item = $(chip).children('#PreparationDeskDevice')

            //2013/12/10 TMEJ 河原 次世代サービス 工程管理機能開発 START
            //ポップアップの表示位置設定
            popOrverPosition = $('.ColumnBox05').offset().left - 8 - $('.saPopOver2').width();
            //三角アイコンの表示位置設定
            trianglePosition = "395px";
            //三角アイコン表示方向設定(右側)
            triangleRotate = triangleRotateRigth;
            triangleDeleteRotate = triangleRotateLeft;
            //三角アイコンの表示位置の微調整
            trianglePositionY = 0;

            //2013/12/10 TMEJ 河原 次世代サービス 工程管理機能開発 END

            break;

        case 'Delivery':    // 納車作業
            // チップ情報設定
            detailsArea = 4;
            item = $(chip).children('#DeliveryDeskDevice')

            //2013/12/10 TMEJ 河原 次世代サービス 工程管理機能開発 START
            //ポップアップの表示位置設定
            popOrverPosition = $('.ColumnBox06').offset().left - 8 - $('.saPopOver2').width();
            //三角アイコンの表示位置設定
            trianglePosition = "395px";
            //三角アイコン表示方向設定(右側)
            triangleRotate = triangleRotateRigth;
            triangleDeleteRotate = triangleRotateLeft;
            //三角アイコンの表示位置の微調整
            trianglePositionY = 0;
            //2013/12/10 TMEJ 河原 次世代サービス 工程管理機能開発 END

            break;

        case 'Work':        // 作業中
            // チップ情報設定
            detailsArea = 5;

            //2013/12/10 TMEJ 河原 次世代サービス 工程管理機能開発 START
            //item = $(chip).children('#Working')
            item = $(chip).children('#WorkDeskDevice').children('#Working')

            //ポップアップの表示位置設定
            popOrverPosition = $('.ColumnBox04').offset().left - 8 - $('.saPopOver2').width();
            //三角アイコンの表示位置設定
            trianglePosition = "395px";
            //三角アイコン表示方向設定(右側)
            triangleRotate = triangleRotateRigth;
            triangleDeleteRotate = triangleRotateLeft;
            //三角アイコンの表示位置の微調整
            trianglePositionY = 0;
            //2013/12/10 TMEJ 河原 次世代サービス 工程管理機能開発 END

            break;

        case 'AdvancePreparations': // 事前準備
            // チップ情報設定
            detailsArea = 6;
            item = $(chip).children('#AdvancePreparationsDeskDevice');
            this.detailsRezId = item.attr('rezId');

            break;

        //2013/12/10 TMEJ 河原 次世代サービス 工程管理機能開発 START 
        case 'Assignment': // 振当待ちエリア

            // チップ情報設定
            detailsArea = 7;
            item = $(chip).children('#AssignmentDeskDevice');

            //ポップアップの表示位置設定
            popOrverPosition = $('.ColumnBox02').offset().left;
            //三角アイコンの表示位置設定
            trianglePosition = "-21px";
            //三角アイコン表示方向設定(左側)
            triangleRotate = triangleRotateLeft;
            triangleDeleteRotate = triangleRotateRigth;
            //三角アイコンの表示位置の微調整
            trianglePositionY = 5;

            break;

        //2013/12/10 TMEJ 河原 次世代サービス 工程管理機能開発 END 
        default:
            detailsArea = 0;
            detailsVisitNo = 0;
            detailsOrderNo = '';
            detailsApprovalId = '';

            //2013/12/10 TMEJ 河原 次世代サービス 工程管理機能開発 START
            detailsUpDateDate = '';
            detailsRezId = '';
           //2013/12/10 TMEJ 河原 次世代サービス 工程管理機能開発 END

            break;
    };

    this.detailsVisitNo = item.attr('visitNo');
    this.detailsOrderNo = item.attr('orderNo');
    this.detailsApprovalId = item.attr('approvalId');

    //2013/12/10 TMEJ 河原 次世代サービス 工程管理機能開発 START
    this.detailsUpDateDate = item.attr('updatedate');
    this.detailsRezId = item.attr('rezId');
    //2013/12/10 TMEJ 河原 次世代サービス 工程管理機能開発 END

}


// 過去ポップオーバー情報クリア
function PopupDataClear() {

    // アイコン要素の削除 //
    $('.PopoverRightIcnD').empty();
    $('.PopoverRightIcnD').remove();
    // 2018/06/20 NSK 可児 TKM Next Gen e-CRB Project Application development Block B-1  Pマーク、L マークなどを表示 START
    //$('.PopoverRightIcnI').empty();
    //$('.PopoverRightIcnI').remove();
    $('.PopoverRightIcnP').empty();
    $('.PopoverRightIcnP').remove();
    $('.PopoverRightIcnL').empty();
    $('.PopoverRightIcnL').remove();
    // 2018/06/20 NSK 可児 TKM Next Gen e-CRB Project Application development Block B-1  Pマーク、L マークなどを表示 END
    $('.PopoverRightIcnS').empty();
    $('.PopoverRightIcnS').remove();
    // 詳細内容クリア //
    $('#IconStatsLabel').empty();
    $('#InterruptionCauseRepeaterDiv').empty();
    $("#DeliveryTimeLabel").empty();
    $("#ChangeCountLabel").empty();
    $("#DeliveryEstimateLabel").empty();
    $("#HeadInfomationPullDiv").empty();
    $('#DetailsRegistrationNumber').text("");
    $('#DetailsCarModel').text("");
    $('#DetailsModel').text("");
    $('#DetailsCustomerName').text("");
    $('#DetailsPhoneNumber').text("");
    $('#DetailsMobileNumber').text("");
    $('#DetailsServiceContents').text("");
    $('#DetailsWaitPlan').text("");

    //呼び出しエリア
    $('#DetailsCallNo').text("");
    $('#DetailsCallPlace').val("");
    $('#DetailsVisitName').text("");
    $('#DetailsVisitTelno').text("");
    $('#DetailsCallPlace').blur();

    // 2012/09/21 TMEJ 小澤 【SERVICE_2】SAストール予約受付機能開発（仕分けNo.35）START
    $('#DetailsDrawer').text("");
    // 2012/09/21 TMEJ 小澤 【SERVICE_2】SAストール予約受付機能開発（仕分けNo.35）END
    // ボタン制御 //
    $('#DetailButtonLeft_Dammy').val("");
    $('#DetailButtonCenter_Dammy').val("");
    $('#DetailButtonRight_Dammy').val("");
    $('#DetailbottomButton').val("");
    // 非活性制御(サーバポスト中に遷移処理出来ないように) //
    $('#DetailButtonLeft').attr("disabled", "disabled");
    $('#DetailButtonCenter').attr("disabled", "disabled");
    $('#DetailButtonRight').attr("disabled", "disabled");

    //2013/12/10 TMEJ 河原 次世代サービス 工程管理機能開発 START
    //標準ボタン非表示
    $('#DetailBottomBox').css("display", "none")
    //ボタンステータスクリア
    $('#DetailClickButtonStatus').val("");
    //削除ボタン非表示
    $('.DetailFlickableBox').css('display', 'none');
    //スクロールを元に戻す
    $('.DetailFlickableBox').children('.scroll-inner').css('transform', 'translate3d(0px, 0px, 0px)');
    //RO_INFOの行ロックバージョンクリア
    $('#RoInfoRowLockVersion').val("");
    //2013/12/10 TMEJ 河原 次世代サービス 工程管理機能開発 END

    $("#FixDeliveryTimeLabel").css("display", "none");
    $("#FixSlashLabel").css("display", "none");
    $("#FixDownArrow").css("display", "none");
    $("#FixDeliveryEstimateLabel").css("display", "none");

    //呼び出しエリア
    $("#BtnCALLCancel").attr("style", "display: none;");
    $("#BtnCALL").attr("style", "display: none;");
    $("#VisitCustomer").attr("style", "display: none;");

    // 2012/09/21 TMEJ 小澤 【SERVICE_2】SAストール予約受付機能開発（仕分けNo.35）START
    $("#DrawerTable").css("display", "none");
    // 2012/09/21 TMEJ 小澤 【SERVICE_2】SAストール予約受付機能開発（仕分けNo.35）END
    // 2012/11/02 TMEJ 小澤 【SERVICE_2】サービス業務管理機能開発 START
    $(".DetailDeleteFooterBox").css("display", "none");
    // 2012/11/02 TMEJ 小澤 【SERVICE_2】サービス業務管理機能開発 END

    // 2012/12/19 TMEJ 河原 【SERVICE_2】次世代サービスROステータス切り離し対応 START
    $("#HiddenDetailsInspectionButtonStatus").val("")
    // 2012/12/19 TMEJ 河原 【SERVICE_2】次世代サービスROステータス切り離し対応 END
    //2013/03/12 TMEJ 丁 【A.STEP1】来店受付管理オペレーション確立に向けた評価アプリ作成　START
    $('.SearchIcon').css('display', 'none');
    //2013/03/12 TMEJ 丁 【A.STEP1】来店受付管理オペレーション確立に向けた評価アプリ作成　END

    //2013/06/03 TMEJ 河原 【A.STEP2】次世代e-CRBタブレット　新DB適応に向けた機能設計 START
    $('#HiddenVehicleModel').val("");
    //2013/06/03 TMEJ 河原 【A.STEP2】次世代e-CRBタブレット　新DB適応に向けた機能設計 END

}

//カウンター対応
function proccounter(diffseconds) {
    //システム時刻の取得
    var sysTime = (new Date()).getTime() + diffseconds;
    //procgroupのアイテムの取得
    var items = document.getElementsByName('procgroup');
    // 2012/07/23 TMEJ 日比野 STEP2 START
    //カウンター更新
    for (var i = 0; i < items.length; i++) {
        //アイテム情報を取得
        var item = items[i];
        var counterDiv = $(item).prevAll("#ColumnCount");

        //アイテムの時刻を取得
        var limitTime = new Date(item.getAttribute("limittime"));
        if (limitTime == "Invalid Date") {
            //counterDiv.html("00'00");
            counterDiv.html("--'--");
            counterDiv.removeClass().addClass("ColumnCount");
            continue;
        }
        //計測時間の取得
        var procTime = sysTime - limitTime.getTime();

        //表示色の設定
        if (procTime > 0) {

            //2013/12/10 TMEJ 河原 次世代サービス 工程管理機能開発 START

            //counterDiv.removeClass().addClass("ColumnCountRed");

            //カウンターの色の制御
            if (item.attributes.area) {

                //振当待ちエリアの場合は赤色にはしない
                counterDiv.removeClass().addClass("ColumnCount");

            } else {
                //上記以外

                counterDiv.removeClass().addClass("ColumnCountRed");
            };

            //2013/12/10 TMEJ 河原 次世代サービス 工程管理機能開発 END

        } else {
            counterDiv.removeClass().addClass("ColumnCount");
        }

        //ミリセックは切り上げる。
        var procTimeSec = Math.ceil(procTime / 1000) * 1000;

        if (procTimeSec >= MAX_PROC_TIME) {
            procTimeSec = MAX_PROC_TIME - 1;
        } else if (MIN_PROC_TIME >= procTimeSec) {
            procTimeSec = MIN_PROC_TIME + 1;
        }

        var minutes = parseInt(Math.abs(procTimeSec) / 60000);
        var seconds = parseInt(Math.abs(procTimeSec) % 60000 / 1000);

        if ((isNaN(minutes) == true) || (isNaN(seconds) == true)) {
            //counterDiv.html("00'00");
            counterDiv.html("--'--");
            continue;
        }
        //計測時間の設定
        counterDiv.html("" + ("0" + minutes).slice(-2) + "'" + ("0" + seconds).slice(-2));
    }

    //納車予定時刻の表示色を更新
    ProcCounterColumnTimeUpdate(diffseconds, items);

    // 2012/07/23 TMEJ 日比野 STEP2 END
}

// 2012/07/23 TMEJ 日比野 STEP2 START

// 納車予定時刻の表示色を更新
function ProcCounterColumnTimeUpdate(diffseconds, items) {

    var now = new Date(new Date().getTime() + diffseconds);

    for (var i = 0; i < items.length; i++) {
        var item = items[i];

        var colTimeDiv = $(item).prevAll("#ColumnTime");

        if (colTimeDiv.size() == 0) {
            continue;
        }

        //アイテムの納車予定時刻を取得
        var overTime2 = new Date(item.getAttribute("overtime2"));

        //2017/10/13 NSK 竹中(悠) REQ-SVT-TMT-20170227-002 見込納車遅れ日時計算ロジック統一 START
        // 日付の最小値
        var minDateTime = new Date("0001/01/01 00:00");

        // 納車予定時刻が日付最小値の場合は遅れ管理しない
        if ((overTime2 - minDateTime) == 0) {
            
            // 常に青
            var className = "ColumnTimeBlue";
            colTimeDiv.removeClass().addClass(className);
            continue;
        }
        //2017/10/13 NSK 竹中(悠) REQ-SVT-TMT-20170227-002 見込納車遅れ日時計算ロジック統一 END

        var className = "";
        if (overTime2 < now) {
            className = "ColumnTimeRed";
        } else {
            //アイテムの納車見込遅れ時刻を取得
            var overTime1 = new Date(item.getAttribute("overtime1"));

            if (overTime1 < now) {
                className = "ColumnTimeYellow";
            } else {
                className = "ColumnTimeBlue";
            }
        }

        colTimeDiv.removeClass().addClass(className);
    }
}
// 2012/07/23 TMEJ 日比野 STEP2 END

// 通知リフレッシュ処理
function MainRefresh() {
    // 2019/07/18 NSK 鈴木 [TKM]PUAT-4072 SAがRO起票ボタンを押下するとくるくる表示が止まらない START
    // 画面更新中の場合、処理を終了する。
    if (gUpdatingFlag) {
        return;
    }

    // 画面更新中フラグをtrueにする
    setUpdatingFlag(true);
    // 2019/07/18 NSK 鈴木 [TKM]PUAT-4072 SAがRO起票ボタンを押下するとくるくる表示が止まらない END

    // アニメーション表示
    $('#loadingSchedule').attr("style", "visibility: visible");
    //2012/11/14 TMEJ 小澤 【SERVICE_2】次世代サービス アクティブインジゲータ対応 No.1 START
    //タイマーセット
    commonRefreshTimer(RefreshDisplay);
    //2012/11/14 TMEJ 小澤 【SERVICE_2】次世代サービス アクティブインジゲータ対応 No.1 END
    // 隠しボタン押下しリフレッシュ
    $("#MainPolling").click();
    //通知リフレッシュ中は工程管理エリアは触れないように
    $("#contentsRightBox_LoadingScreen").css({ "display": "table" });
    //2012/06/12 西岡 事前準備対応 START
    if (slideDownFlag) {
        $("#HeadInfomationPullDiv").slideUp();
        $('.DetailFlickableBox .scroll-inner').css({ 'transform': 'translate3d(0px, 0px, 0px)',
            '-webkit-transition': 'transform 400ms'
        });

        slideDownFlag = false;
    }

    if ($('.headerInner').css('left') != '0') {
        SlideStatus("clear");
        $('.TextArea').blur();
    }
    searchListClear();

    //2013/12/10 TMEJ 河原 次世代サービス 工程管理機能開発 START

    //$("#SearchCancel").css('display', 'none');
    $("#ButtonLeft").css('display', 'none');

    //$("#ChipChanges").css('display', 'none');
    $("#ButtonRight").css('display', 'none');

    //2013/12/10 TMEJ 河原 次世代サービス 工程管理機能開発 END

    //チップ詳細削除
    //CloseChipDetail();
    //CloseAdvancePreparations();
    //$('.BlackWindow').css('display', 'none');
    //非同期処理のトリガーコントロール名称格納
    aryPostCtrl.push("MainPolling");
    //postBackControl = "MainPolling";
    // チップ選択クリア
    //ClearChip()

    //選択中のチップが存在するかどうかチェック
    if (SelectedChipInfo != null && SelectedChipInfo != undefined) {
        //選択中の場合
        //選択を解除する
        UnsetChip(SelectedChipInfo);
    }

    // 2019/07/18 NSK 鈴木 [TKM]PUAT-4072 SAがRO起票ボタンを押下するとくるくる表示が止まらない START
    // 画面更新中フラグをfalseにする
    setUpdatingFlag(false);
    // 2019/07/18 NSK 鈴木 [TKM]PUAT-4072 SAがRO起票ボタンを押下するとくるくる表示が止まらない END

    // ポップアップ表示戻り値
    return "TRUE";
}

// チップフリック時
function FlickChip(chip) {
    //SC3140102.jsでCALLしているためメソッド自体はとりあえず残す
}

//フッターアプリの起動設定
function SetFutterApplication() {

    //スケジュールオブジェクトを拡張
    $.extend(window, { schedule: {} });

    //スケジュールオブジェクトに関数追加
    $.extend(schedule, {

        /**
        * @class アプリ起動クラス
        */
        appExecute: {

            /**
            * カレンダーアプリ起動(単体)
            */
            executeCaleNew: function () {
                window.location = "icrop:cale:";
                return false;
            },
            /**
            * 電話帳アプリ起動(単体)
            */
            executeCont: function () {
                window.location = "icrop:cont:";
                return false;
            }
        }

    });

}

// 読み込み中アイコンの非表示
function StopIcon(itemName) {
    $(itemName).hide(0);
}

// チップ詳細のボタン押下時の2度押し防止用
function ButtonControl(detailClickButtonName) {
    // 2019/07/18 NSK 鈴木 [TKM]PUAT-4072 SAがRO起票ボタンを押下するとくるくる表示が止まらない START
    // 画面更新中フラグをtrueにする
    setUpdatingFlag(true);
    // 2019/07/18 NSK 鈴木 [TKM]PUAT-4072 SAがRO起票ボタンを押下するとくるくる表示が止まらない END
    
    $.master.OpenLoadingScreen();

    // 非活性制御(ダミーボタンを表示し2度押しできないように) //
    toggleRegularDetailButtonControl(true);

    // ダミーボタンにボタン名称を格納 //
    $('#DetailButtonLeft_Dammy').val($('#DetailButtonLeft').val());

    //2013/12/10 TMEJ 河原 次世代サービス 工程管理機能開発 START

    //('#DetailButtonCenter_Dammy').val($('#DetailButtonCenter').val());

    //2013/12/10 TMEJ 河原 次世代サービス 工程管理機能開発 END

    $('#DetailButtonRight_Dammy').val($('#DetailButtonRight').val());

    //2013/12/10 TMEJ 河原 次世代サービス 工程管理機能開発 START

    // チップ詳細で押下されたボタンの名称を格納(サーバでの遷移先判定に必要) //
    //$('#DetailClickButtonName').val($(detailClickButtonName).val());

    //ボタン名称では判断が難しいためボタンステータスを格納するように変更する
    $('#DetailClickButtonStatus').val($(detailClickButtonName).attr(SubMenuButtonStatusCalss));

    //2ボタン標準ボタン用の左右判定フラグ
    $('#DetailClickButtonCheck').val($(detailClickButtonName).attr(ButtonStatus));

    //2013/12/10 TMEJ 河原 次世代サービス 工程管理機能開発 END


    //2012/11/14 TMEJ 小澤 【SERVICE_2】次世代サービス アクティブインジゲータ対応 No.1 START
    //タイマーセット
    commonRefreshTimer(RefreshDisplay);
    //2012/11/14 TMEJ 小澤 【SERVICE_2】次世代サービス アクティブインジゲータ対応 No.1 END

    //画面遷移ボタンクリック
    $("#DetailNextScreenCommonButton").click();


    return false;
}

/*
* ダミーボタンを表示/非表示
*
* @param {boolean} isFlgDammy ダミー表示フラグ
*/
function toggleRegularDetailButtonControl(isFlgDammy) {
    if (isFlgDammy) {
        // ダミーを表示
        $('#DetailButtonLeft').css('display', 'none');
        $('#DetailButtonCenter').css('display', 'none');
        $('#DetailButtonRight').css('display', 'none');
        $('#DetailButtonLeft_Dammy').css('display', 'inline');
        $('#DetailButtonCenter_Dammy').css('display', 'inline');
        $('#DetailButtonRight_Dammy').css('display', 'inline');

        // 2012/12/19 TMEJ 河原 【SERVICE_2】次世代サービスROステータス切り離し対応 START
        $('#DetailButtonInspection').css('display', 'none');
        // 2012/12/19 TMEJ 河原 【SERVICE_2】次世代サービスROステータス切り離し対応 END
    }
    else {
        // 遷移ボタンを表示
        $('#DetailButtonLeft_Dammy').css('display', 'none');
        $('#DetailButtonCenter_Dammy').css('display', 'none');
        $('#DetailButtonRight_Dammy').css('display', 'none');
        $('#DetailButtonLeft').css('display', 'inline');
        $('#DetailButtonCenter').css('display', 'inline');
        $('#DetailButtonRight').css('display', 'inline');
    }
}

// フッターボタンの2度押し制御
function FooterButtonControl() {
    // 2019/07/18 NSK 鈴木 [TKM]PUAT-4072 SAがRO起票ボタンを押下するとくるくる表示が止まらない START
    // 画面更新中フラグをtrueにする
    setUpdatingFlag(true);
    // 2019/07/18 NSK 鈴木 [TKM]PUAT-4072 SAがRO起票ボタンを押下するとくるくる表示が止まらない END

    $.master.OpenLoadingScreen();
    return true;
}

/**
* ダッシュボードをクリックした際に、POPOVERを制御する.
* @return {void}
*/
function ParentPopoverClose() {
    //SC3140102.jsでCALLされているため、メソッド自体は残す
}

/*
* チップ詳細を非表示とし、内容もクリアする。
*
*/
function CloseChipDetail() {
    //チップ詳細クリア
    PopupDataClear();

    //2013/12/10 TMEJ 河原 次世代サービス 工程管理機能開発 START
    $('#bodyFrame').unbind(TOUCH_START, PopOverCloseCheck);
    //2013/12/10 TMEJ 河原 次世代サービス 工程管理機能開発 END

    $("#CustomerPopOver2").attr("style", "display: none;");
}
/*
* 事前準備ボタン押下時の処理
*
*/
function AdvancePreparations() {
    // 初期表示時のボタン制御
    if (flgFooterCtrlRight) return;
    // チップ詳細画面表示時は詳細画面をクローズ
    if ($("#CustomerPopOver2").css('display') === 'block') {
        //if ($('.headerInner').css('left') != '0') {
            //SlideStatus();
        //}
        UnsetChip($(nowSelectArea).parent());
    }
    // 事前準備画面表示時は事前準備画面をクローズ
    if ($("#CustomerPopOver").css('display') === 'block') {
        CloseAdvancePreparations();
        //2012/11/21 TMEJ 丁   【SERVICE_2】SAストール予約受付機能開発（仕分けNo.72） START
		document.getElementById("SASelector").value = icropScript.ui.account
		document.getElementById("AdvancePreparationsCntHidden").value = countResult;
		document.getElementById("AdvancePreparationsColorHidden").value = color;
		$('#AdvancePreparationsCnt').text($('#AdvancePreparationsCntHidden').val());
		var ButtonStatus = $('#AdvancePreparationsColorHidden').val();
		if (ButtonStatus === "1") {
			$('#AdvancePreparationsButton').css('background', 'url(../Styles/Images/SC3140103/saAdvancePreparatironsActive.png) no-repeat');
			$('.AdvancePreparationsName').css('color', '#FFF');
		} else if (ButtonStatus === "2") {
			$('#AdvancePreparationsButton').css('background', 'url(../Styles/Images/SC3140103/saAdvancePreparatironsRedActive.png) no-repeat');
			$('.AdvancePreparationsName').css('color', '#FFF');
		} else {
			$('#AdvancePreparationsButton').css('background', 'url(../Styles/Images/SC3140103/saAdvancePreparatironsDeactive.png) no-repeat');
			$('.AdvancePreparationsName').css('color', '#666');
		}
		//2012/11/21 TMEJ 丁   【SERVICE_2】SAストール予約受付機能開発（仕分けNo.72） END
        return;
    }

    CloseAdvancePreparations();
    // 事前準備画面を表示
    $("#CustomerPopOver").attr("style", "display: block;");
    $('#LoadAdvancePreparations').attr("style", "visibility: visible");
    aryPostCtrl.push("AdvancePreparationsClick");
    $("#contentsRightBox1").css("display", "none");
    //2012/11/14 TMEJ 小澤 【SERVICE_2】次世代サービス アクティブインジゲータ対応 No.1 START
    //タイマーセット
    commonRefreshTimer(RefreshDisplay);
    //2012/11/14 TMEJ 小澤 【SERVICE_2】次世代サービス アクティブインジゲータ対応 No.1 END
    $('#AdvancePreparationsClick').click();
}
/*
* 事前準備画面のクローズ処理
*
*/
function CloseAdvancePreparations() {
    // ポップオーバーのチップ消去
    var chipDataBox = $('#flickableF').children();
    $(chipDataBox).remove();
    $(chipDataBox).empty();
    if ($("#contentsRightBox1").css("display") == "none") {
        $("#contentsRightBox1").css("display", "block");
        // 通常のチップエリアスクロールが変動する為、一番上になるよう補正をかける
        $("#flickable1").flickable('scroll', 0, 209);
        $("#flickable2").flickable('scroll', 0, 209);
        $("#flickable3").flickable('scroll', 0, 209);
        $("#flickable4").flickable('scroll', 0, 209);
        $("#flickable5").flickable('scroll', 328, 0);
    }
    $("#CustomerPopOver").css('display', 'none');
}

/*
* 顧客検索表示への切り替えスライド処理
* 
*/
function SlideSearch() {

    //2013/03/12 TMEJ 丁 【A.STEP1】来店受付管理オペレーション確立に向けた評価アプリ作成　START
    $('.TextArea').css('display', 'block');
    //2013/03/12 TMEJ 丁 【A.STEP1】来店受付管理オペレーション確立に向けた評価アプリ作成　END


    //2013/12/10 TMEJ 河原 次世代サービス 工程管理機能開発 START
    // テキストエリアの値を変数に格納してテキストエリアの内容を一旦消す
    var textAreaValue = $('.TextArea').val();
    $('.TextArea').val("");
    //2013/12/10 TMEJ 河原 次世代サービス 工程管理機能開発 END


    // フォーカス当て
    $('.TextArea').focus();


    //2013/12/10 TMEJ 河原 次世代サービス 工程管理機能開発 START

    //ブラウズの仕様上フォーカスをあてると自動的にスクロールしてしまう
    //またスクロールしない場合も存在しているためスクロールしているかしていないか
    //判定しアニメーションのスタート位置を微調整する

    if ($('.contentInner').offset().left < $("#CustomerPopOver2").offset().left) {

        //自動スクロールしている場合Leftの位置を微調整
        $('.contentInner').css('left', '248px');
    };

    //2013/12/10 TMEJ 河原 次世代サービス 工程管理機能開発 END


    // すぐにテキストボックスを消す
    $('#SearchText').css('display', 'none');


    //2013/12/10 TMEJ 河原 次世代サービス 工程管理機能開発 START
    /* テキストにフォーカスが当たってズレた分を戻す*/
    //2013/03/12 TMEJ 丁 【A.STEP1】来店受付管理オペレーション確立に向けた評価アプリ作成　START
    //var headWidth;
    //2013/03/12 TMEJ 丁 【A.STEP1】来店受付管理オペレーション確立に向けた評価アプリ作成　END
    //if (slideCount == 0) {
        //$('.contentInner').css('left', '285px');
        //2013/03/12 TMEJ 丁 【A.STEP1】来店受付管理オペレーション確立に向けた評価アプリ作成　START
        //headWidth = 239;
        //2013/03/12 TMEJ 丁 【A.STEP1】来店受付管理オペレーション確立に向けた評価アプリ作成　END
    //} else {
        //$('.contentInner').css('left', '324px');
        //2013/03/12 TMEJ 丁 【A.STEP1】来店受付管理オペレーション確立に向けた評価アプリ作成　START
        //headWidth = 320;
        //2013/03/12 TMEJ 丁 【A.STEP1】来店受付管理オペレーション確立に向けた評価アプリ作成　END
    //}
    //2013/12/10 TMEJ 河原 次世代サービス 工程管理機能開発 END

    slideCount++;

    //2013/12/10 TMEJ 河原 次世代サービス 工程管理機能開発 START
    //2013/03/12 TMEJ 丁 【A.STEP1】来店受付管理オペレーション確立に向けた評価アプリ作成　START
    //var headWidth = parseInt($('.contentInner').children('div').css('width'));
    //setTimeout(
    //timeoutTimer = setTimeout(
    //2013/03/12 TMEJ 丁 【A.STEP1】来店受付管理オペレーション確立に向けた評価アプリ作成　END
        //function () {
    	    //$('.contentInner').animate({ left: -headWidth }, {
    	        //speed: SCROLLSPEED,
    	        //complete: function () {
    	            //$('#SearchText').css('display', 'block');
    	        //}
    	    //});
        //}
    //, 1000);

    //アニメーション設定
    //検索ボタンをタップされた場合、検索エリアをスライドインさせる
    $('.contentInner').css({
        "transform": "translate3d(-397px, 0px, 0px)",
        "-webkit-transition": "transform " + SCROLLSPEED + "ms"
    }).one("webkitTransitionEnd", function () {

        //アニメーション終了後処理
        //テキストエリアを再表示され格納していた値を元に戻す
        $('#SearchText').css('display', 'block');
        $('.TextArea').val(textAreaValue); 

     }); ;

    //$("#SearchCancel").css('display', 'inline-block');
    $("#ButtonLeft").css('display', 'inline-block');

    //$("#ChipChanges").css('display', 'inline-block');
    $("#ButtonRight").css('display', 'inline-block');

    //2013/12/10 TMEJ 河原 次世代サービス 工程管理機能開発 END


	$('#Selection1').addClass('ButtonOn');

    return false;
}
/*
* 詳細表示への切り替えスライド処理
* 
*/
function SlideStatus(cmd) {

    //2013/12/10 TMEJ 河原 次世代サービス 工程管理機能開発 START

    //$("#SearchCancel").css('display', 'none');
    $("#ButtonLeft").css('display', 'none');

    //$("#ChipChanges").css('display', 'none');
    $("#ButtonRight").css('display', 'none');

    //2013/12/10 TMEJ 河原 次世代サービス 工程管理機能開発 END


    if (cmd == "clear") {
        $('.contentInner').css("left", "0px"); 
        $('.SelectionButton').children('ul').children('li').removeClass('ButtonOn');
        selectSearchTypeIndex = 0;

        //2013/12/10 TMEJ 河原 次世代サービス 工程管理機能開発 STRAT

        $('#SearchTypeIndexHidden').val("")

        //2013/12/10 TMEJ 河原 次世代サービス 工程管理機能開発 END

        $('.TextArea')[0].placeholder = $('#SearchPlaceRegNo').html();
        searchListClear();
        $('#SearchBottomButton').addClass('BottomButtonDisable');
        CustomerClearFlag = false;
    } else {

        //2013/12/10 TMEJ 河原 次世代サービス 工程管理機能開発 STRAT

        //2013/03/12 TMEJ 丁 【A.STEP1】来店受付管理オペレーション確立に向けた評価アプリ作成　START
        //$('.contentInner').animate({ left: 35 }, {
        //$('.contentInner').animate({ left: 0 }, {
        //2013/03/12 TMEJ 丁 【A.STEP1】来店受付管理オペレーション確立に向けた評価アプリ作成　END
            //speed: SCROLLSPEED,
            //complete: function () {
                //$('.SelectionButton').children('ul').children('li').removeClass('ButtonOn');
                //$('#Selection1').addClass('ButtonOn');
                //selectSearchTypeIndex = 0;
                //$('.TextArea')[0].placeholder = $('#SearchPlaceRegNo').html();
                //searchListClear();
                //$('#SearchBottomButton').addClass('BottomButtonDisable');
                //CustomerClearFlag = false;
            //}
        //});
        $('.contentInner').css('left', '0px');
        $('.contentInner').css({
            "transform": "translate3d(0px, 0px, 0px)",
            "-webkit-transition": "transform " + SCROLLSPEED + "ms"
        }).one("webkitTransitionEnd", function () {

            $('.SelectionButton').children('ul').children('li').removeClass('ButtonOn');
            $('#Selection1').addClass('ButtonOn');
            selectSearchTypeIndex = 0;

            //2013/12/10 TMEJ 河原 次世代サービス 工程管理機能開発 STRAT

            $('#SearchTypeIndexHidden').val("")

            $('#SearchCustomerAllCountHidden').val("")

            //2013/12/10 TMEJ 河原 次世代サービス 工程管理機能開発 END

            $('.TextArea')[0].placeholder = $('#SearchPlaceRegNo').html();
            searchListClear();
            $('#SearchBottomButton').addClass('BottomButtonDisable');
            CustomerClearFlag = false;
                
         });


        //2013/12/10 TMEJ 河原 次世代サービス 工程管理機能開発 END

    };

    //2013/03/12 TMEJ 丁 【A.STEP1】来店受付管理オペレーション確立に向けた評価アプリ作成　START
    //Divをスライドした後で、ずれるので、調整する
    //AdjustDiv();
    //2013/03/12 TMEJ 丁 【A.STEP1】来店受付管理オペレーション確立に向けた評価アプリ作成　END

    return false;
}
/*
* 検索条件の切り替え処理
*
*/
function SelectSearchType(select) {
    $('.TextArea').blur();
    this.selectSearchTypeIndex = $('.SelectionButton ul li').index(select);
    selectButton = $(select).attr('className');
    switch (selectSearchTypeIndex) {
        case 0:
            $('.TextArea')[0].placeholder = $('#SearchPlaceRegNo').html();
            break;
        case 1:
            $('.TextArea')[0].placeholder = $('#SearchPlaceVin').html();
            break;
        case 2:
            $('.TextArea')[0].placeholder = $('#SearchPlaceName').html();
            break;
        case 3:
            $('.TextArea')[0].placeholder = $('#SearchPlacePhone').html();
            break;
        default:
            break;
    }
    // 選択されているボタンを押した場合はテキストボックスにフォーカスを当てるのみ
    if (selectButton === 'ButtonOn') {
        $('.TextArea').focus();
        return false;
    }
    // 検索条件ボタンの状態を切替えて、テキストボックスにフォーカスを当てる
    searchUl = $('.SelectionButton').children('ul');
    $(searchUl).children('li').removeClass('ButtonOn');
    $(select).addClass('ButtonOn');
    $('#SearchFocusInDummyButton').click();
    return false;
}
/*
* 顧客検索開始
*
*/
function SearchCustomer() {
    
    searchListClear();
    if ($('.NoSearchImage').css('display') === 'block') {
        $('.NoSearchImage').css('display', 'none');
    }
    searchText = $('.TextArea').attr('value');
    $('#SearchRegistrationNumberHidden').val("");
    $('#SearchVinHidden').val("");
    $('#SearchCustomerNameHidden').val("");

    //2013/12/10 TMEJ 河原 次世代サービス 工程管理機能開発 START

    //$('#SearchPhoneNumberHidden').val("");
    $('#SearchAppointNumberHidden').val("");

    //2013/12/10 TMEJ 河原 次世代サービス 工程管理機能開発 END

    $('#SearchTypeIndexHidden').val(this.selectSearchTypeIndex);

    switch (this.selectSearchTypeIndex) {
        case 0:
            $('#SearchRegistrationNumberHidden').val(searchText);
            break;
        case 1:
            $('#SearchVinHidden').val(searchText);
            break;
        case 2:
            $('#SearchCustomerNameHidden').val(searchText);
            break;
        case 3:

            //2013/12/10 TMEJ 河原 次世代サービス 工程管理機能開発 START

            //$('#SearchPhoneNumberHidden').val(searchText);
            $('#SearchAppointNumberHidden').val(searchText);

            //2013/12/10 TMEJ 河原 次世代サービス 工程管理機能開発 END

            break;
        default:
            break;
    }

    //2013/12/10 TMEJ 河原 次世代サービス 工程管理機能開発 START

    //$('#ChipChanges').removeClass('ButtonRightOn');
    $('#ButtonRight').removeClass('ButtonRightOn');

    //2013/12/10 TMEJ 河原 次世代サービス 工程管理機能開発 END

    flagChipChanges = false;

    $('#SearchStartRowHidden').val("1");
    $('#SearchEndRowHidden').val("1");
    $('#SearchSelectTypeHidden').val("0");
    $('#SearchDataLoading').css('display', 'block');
    aryPostCtrl.push("SearchCustomer");
    //2012/11/14 TMEJ 小澤 【SERVICE_2】次世代サービス アクティブインジゲータ対応 No.1 START
    //タイマーセット
    commonRefreshTimer(RefreshDisplay);
    //2012/11/14 TMEJ 小澤 【SERVICE_2】次世代サービス アクティブインジゲータ対応 No.1 END
    $('#SearchCustomerDummyButton').click();
    return false;
}
/*
* 顧客検索テキストクリア
*
*/
function TextClear() {
    searchText = $('.TextArea').attr('value');
    if (searchText == undefined || searchText == "") {
        $('#SearchFocusInDummyButton').click();
        return false;
    }
    searchText = "";
    $('.TextArea').attr('value', searchText);
    $('#SearchFocusInDummyButton').click();
}

function FocusInSearchTextBox() {
    $('.TextArea').blur();
    $('.TextArea').focus();
}
/*
* 次のN件を読み込むを選択
*
*/
function SearchNextList() {

    $('#SearchSelectTypeHidden').val("1");

    //2013/12/10 TMEJ 河原 次世代サービス 工程管理機能開発 START

    //$('#ChipChanges').removeClass('ButtonRightOn');
    $('#ButtonRight').removeClass('ButtonRightOn');

    //2013/12/10 TMEJ 河原 次世代サービス 工程管理機能開発 END

    flagChipChanges = false;
    aryPostCtrl.push("SearchCustomer");
    $('.NextList').css('display', 'none');
    $('.NextSearchingImage').css('display', 'block');
    $('.NextListSearching').css('display', 'block');
    //2012/11/14 TMEJ 小澤 【SERVICE_2】次世代サービス アクティブインジゲータ対応 No.1 START
    //タイマーセット
    commonRefreshTimer(RefreshDisplay);
    //2012/11/14 TMEJ 小澤 【SERVICE_2】次世代サービス アクティブインジゲータ対応 No.1 END
    $('#SearchCustomerDummyButton').click();
}
/*
* 前のN件を読み込むを選択
*
*/
function SearchFrontList() {

    $('#SearchSelectTypeHidden').val("-1");

    //2013/12/10 TMEJ 河原 次世代サービス 工程管理機能開発 START

    //$('#ChipChanges').removeClass('ButtonRightOn');
    $('#ButtonRight').removeClass('ButtonRightOn');

    //2013/12/10 TMEJ 河原 次世代サービス 工程管理機能開発 END

    flagChipChanges = false;
    aryPostCtrl.push("SearchCustomer");
    $('.FrontList').css('display', 'none');
    $('.FrontSearchingImage').css('display', 'block');
    $('.FrontListSearching').css('display', 'block');
    //2012/11/14 TMEJ 小澤 【SERVICE_2】次世代サービス アクティブインジゲータ対応 No.1 START
    //タイマーセット
    commonRefreshTimer(RefreshDisplay);
    //2012/11/14 TMEJ 小澤 【SERVICE_2】次世代サービス アクティブインジゲータ対応 No.1 END
    $('#SearchCustomerDummyButton').click();
}

/*
* 検索状態のクリア
* 
*/
function searchListClear() {
    var searchList = $('#SearchListBox');
    $(searchList).remove();
    $(searchList).empty();
    if ($('.NoSearchImage').css('display') === 'block') {
        $('.NoSearchImage').css('display', 'none');
    }
    if ($('.FrontLink').css('display') === 'block') {
        $('.FrontLink').css('display', 'none');
    }
    if ($('.EndLink').css('display') === 'block') {
        $('.EndLink').css('display', 'none');
    }
    flagChipChanges = false;

    //2013/12/10 TMEJ 河原 次世代サービス 工程管理機能開発 START

    //$('#ChipChanges').removeClass('ButtonRightOn');
    $('#ButtonRight').removeClass('ButtonRightOn');

    //2013/12/10 TMEJ 河原 次世代サービス 工程管理機能開発 END

}
/* 
* 付替えボタン押下
*
*/
function ChipChange() {
    if (!flagChipChanges) return;

    // 2019/07/18 NSK 鈴木 [TKM]PUAT-4072 SAがRO起票ボタンを押下するとくるくる表示が止まらない START
    // 画面更新中フラグをtrueにする
    setUpdatingFlag(true);
    // 2019/07/18 NSK 鈴木 [TKM]PUAT-4072 SAがRO起票ボタンを押下するとくるくる表示が止まらない END

    $.master.OpenLoadingScreen();
    aryPostCtrl.push("BeforeChipChanges");
    //2012/11/14 TMEJ 小澤 【SERVICE_2】次世代サービス アクティブインジゲータ対応 No.1 START
    //タイマーセット
    commonRefreshTimer(RefreshDisplay);
    //2012/11/14 TMEJ 小澤 【SERVICE_2】次世代サービス アクティブインジゲータ対応 No.1 END
    $('#BeforeChipChangesDummyButton').click();
    return false;
}
/* 
* 顧客解除ボタン押下
*
*/
function CustomerClear() {
    if (!CustomerClearFlag) return;

    // 2019/07/18 NSK 鈴木 [TKM]PUAT-4072 SAがRO起票ボタンを押下するとくるくる表示が止まらない START
    // 画面更新中フラグをtrueにする
    setUpdatingFlag(true);
    // 2019/07/18 NSK 鈴木 [TKM]PUAT-4072 SAがRO起票ボタンを押下するとくるくる表示が止まらない END

    $.master.OpenLoadingScreen();
    aryPostCtrl.push("ChipChanges");
    //2012/11/14 TMEJ 小澤 【SERVICE_2】次世代サービス アクティブインジゲータ対応 No.1 START
    //タイマーセット
    commonRefreshTimer(RefreshDisplay);
    //2012/11/14 TMEJ 小澤 【SERVICE_2】次世代サービス アクティブインジゲータ対応 No.1 END
    $('#ChipClearDummyButton').click();
    return false;
}
/*
* チップ詳細顧客ボタン押下
*
*/
function DetailCustomerButton() {
    if ($("#HiddenDetailsCustomerButtonStatus").val() == "0") {
        return false;
    }

    // 2019/07/18 NSK 鈴木 [TKM]PUAT-4072 SAがRO起票ボタンを押下するとくるくる表示が止まらない START
    // 画面更新中フラグをtrueにする
    setUpdatingFlag(true);
    // 2019/07/18 NSK 鈴木 [TKM]PUAT-4072 SAがRO起票ボタンを押下するとくるくる表示が止まらない END

    $.master.OpenLoadingScreen();
    //2012/11/14 TMEJ 小澤 【SERVICE_2】次世代サービス アクティブインジゲータ対応 No.1 START
    //タイマーセット
    commonRefreshTimer(RefreshDisplay);
    //2012/11/14 TMEJ 小澤 【SERVICE_2】次世代サービス アクティブインジゲータ対応 No.1 END
    $('#DetailButtonLeftDummy').click();
    return false;
}
/*
* チップ詳細R/Oボタン押下
*
*/
function DetailOrderButton() {
    if ($("#HiddenDetailsROButtonStatus").val() == "0") {
        return false;
    }

    // 2019/07/18 NSK 鈴木 [TKM]PUAT-4072 SAがRO起票ボタンを押下するとくるくる表示が止まらない START
    // 画面更新中フラグをtrueにする
    setUpdatingFlag(true);
    // 2019/07/18 NSK 鈴木 [TKM]PUAT-4072 SAがRO起票ボタンを押下するとくるくる表示が止まらない END

    $.master.OpenLoadingScreen();
    //2012/11/14 TMEJ 小澤 【SERVICE_2】次世代サービス アクティブインジゲータ対応 No.1 START
    //タイマーセット
    commonRefreshTimer(RefreshDisplay);
    //2012/11/14 TMEJ 小澤 【SERVICE_2】次世代サービス アクティブインジゲータ対応 No.1 END
    $('#DetailButtonRightDummy').click();
    return false;
}
/*
* チップ詳細作業ボタン押下
*
*/
function DetailApprovalButton() {
    if ($("#HiddenDetailsApprovalButtonStatus").val() == "0") {
        return false;
    }

    // 2019/07/18 NSK 鈴木 [TKM]PUAT-4072 SAがRO起票ボタンを押下するとくるくる表示が止まらない START
    // 画面更新中フラグをtrueにする
    setUpdatingFlag(true);
    // 2019/07/18 NSK 鈴木 [TKM]PUAT-4072 SAがRO起票ボタンを押下するとくるくる表示が止まらない END

    $.master.OpenLoadingScreen();
    //2012/11/14 TMEJ 小澤 【SERVICE_2】次世代サービス アクティブインジゲータ対応 No.1 START
    //タイマーセット
    commonRefreshTimer(RefreshDisplay);
    //2012/11/14 TMEJ 小澤 【SERVICE_2】次世代サービス アクティブインジゲータ対応 No.1 END
    $('#DetailButtonRightDummy').click();
    return false;
}

// 2012/12/19 TMEJ 河原 【SERVICE_2】次世代サービスROステータス切り離し対応 START
/*
* チップ詳細完成検査承認ボタン押下
*
*/
function DetailInspectionButton() {
    // 2019/07/18 NSK 鈴木 [TKM]PUAT-4072 SAがRO起票ボタンを押下するとくるくる表示が止まらない START
    // 画面更新中フラグをtrueにする
    setUpdatingFlag(true);
    // 2019/07/18 NSK 鈴木 [TKM]PUAT-4072 SAがRO起票ボタンを押下するとくるくる表示が止まらない END

    $.master.OpenLoadingScreen();
    //タイマーセット
    commonRefreshTimer(RefreshDisplay);

    $('#DetailButtonInspectionDummy').click();
    return false;
}
// 2012/12/19 TMEJ 河原 【SERVICE_2】次世代サービスROステータス切り離し対応 END

// 2012/11/02 TMEJ 小澤 【SERVICE_2】サービス業務管理機能開発 START
/*
* 削除ボタン押下
*
*/
function DetailDeleteButton() {

    //2013/12/10 TMEJ 河原 次世代サービス 工程管理機能開発 START

    var result

    //表示エリアでコンファーム文言変更                        
    if (detailsArea == 1) {
        // 受付エリア

        result = confirm($("#HiddenDeleteConfirmWord").text());

    } else if (detailsArea == 7) {
        //振当待ちエリア
        
        result = confirm($("#HiddenDeleteConfirmWord02").text());

    } else {
        //上記以外

        //予期せぬエラーの表示
        alert($("#UnanticipatedMessageField").val());

        return false;
    };


    //2013/12/10 TMEJ 河原 次世代サービス 工程管理機能開発 END
    if (result) {
        // 2019/07/18 NSK 鈴木 [TKM]PUAT-4072 SAがRO起票ボタンを押下するとくるくる表示が止まらない START
        // 画面更新中フラグをtrueにする
        setUpdatingFlag(true);
        // 2019/07/18 NSK 鈴木 [TKM]PUAT-4072 SAがRO起票ボタンを押下するとくるくる表示が止まらない END

        $.master.OpenLoadingScreen();
        //2012/11/14 TMEJ 小澤 【SERVICE_2】次世代サービス アクティブインジゲータ対応 No.1 START
        //タイマーセット
        commonRefreshTimer(RefreshDisplay);
        //2012/11/14 TMEJ 小澤 【SERVICE_2】次世代サービス アクティブインジゲータ対応 No.1 END
        $('#DetailButtonDeleteDummy').click();
        return false;
    }
    return false;
}
// 2012/11/02 TMEJ 小澤 【SERVICE_2】サービス業務管理機能開発 END

//2012/11/14 TMEJ 小澤 【SERVICE_2】次世代サービス アクティブインジゲータ対応 No.1 START
/*
* 再描画用処理
*
*/
function RefreshDisplay() {
    if (RefreshFlag) {
        window.location.reload();
    } else {
        RefreshFlag = false;
    }
    
    return true;
}
/*
* タイマークリア（SA用）
*
*/
function commonClearTimerSA(){
    timerClearTime = (new Date()).getTime() - 1;
}
//2012/11/14 TMEJ 小澤 【SERVICE_2】次世代サービス アクティブインジゲータ対応 No.1 END

//2012/11/21 TMEJ 丁   【SERVICE_2】SAストール予約受付機能開発（仕分けNo.72） START
function SAchange() {
    // 初期表示時のボタン制御
    if (flgFooterCtrlRight) return;
    // チップ詳細画面表示時は詳細画面をクローズ
    if ($("#CustomerPopOver2").css('display') === 'block') {
        //if ($('.headerInner').css('left') != '0') {
            //SlideStatus();
        //}
        UnsetChip($(nowSelectArea).parent());
    }
    // 事前準備画面を表示
    $("#CustomerPopOver").attr("style", "display: block;");
    $('#LoadAdvancePreparations').attr("style", "visibility: visible");
    aryPostCtrl.push("AdvancePreparationsClick");
    $("#contentsRightBox1").css("display", "none");
    //タイマーセット
    commonRefreshTimer(RefreshDisplay);
    $('#AdvancePreparationsClick').click();
}
//2012/11/21 TMEJ 丁   【SERVICE_2】SAストール予約受付機能開発（仕分けNo.72） END

//2012/12/06 TMEJ 小澤  問連対応「GTMC121204094」START
//異常エラー時に再描画するスクリプト
function ErrorRefreshScript() {
    //チップ詳細のクルクルは消しておく
    $('#IconLoadingPopup').hide(0);

    // 2019/07/18 NSK 鈴木 [TKM]PUAT-4072 SAがRO起票ボタンを押下するとくるくる表示が止まらない START
    // 画面更新中フラグをtrueにする
    setUpdatingFlag(true);
    // 2019/07/18 NSK 鈴木 [TKM]PUAT-4072 SAがRO起票ボタンを押下するとくるくる表示が止まらない END

    //メインのクルクルを表示する
    $.master.OpenLoadingScreen();
    //タイマーリセット
    timerClearTime = (new Date()).getTime() - 1;
    //再描画ボタンクリック
    window.location.reload();
    return false;
}
//2012/12/06 TMEJ 小澤  問連対応「GTMC121204094」END

//2013/03/12 TMEJ 丁 【A.STEP1】来店受付管理オペレーション確立に向けた評価アプリ作成 START
//お客様呼び出し処理
function CustomerCall() {

    if ($("#BtnCALL").css('display') === 'block') {
        $('#IconLoadingPopup').attr("style", "visibility: visible");
        aryPostCtrl.push("DetailPopupButton");
        //タイマーセット
        commonRefreshTimer(RefreshDisplay);
        $("#CallButton").click();
    }else {
    return false;
    }
}
//お客様呼び出しキャンセル処理
function CustomerCallCancel() {

    if ($("#BtnCALLCancel").css('display') === 'block') {
        $('#IconLoadingPopup').attr("style", "visibility: visible");
        aryPostCtrl.push("DetailPopupButton");
        //タイマーセット
        commonRefreshTimer(RefreshDisplay);
        $("#CallCancelButton").click();
    } else {
    return false;
    }
}
//呼び出しOR呼び出しキャンセル処理が終わった後
function CallCompleted() {
    commonClearTimerSA();
}
//呼び出し場所更新処理
function CallPlaceChange() {
    aryPostCtrl.push("DetailPopupButton");
    $('#IconLoadingPopup').attr("style", "visibility: visible");
    $('#BtnCALL').attr("onclick","");
    //タイマーセット
    commonRefreshTimer(RefreshDisplay);
    $("#CallPlaceChangeButton").click();
    return false;
}
//呼出場所を退避する
function CallPlaceSave() {
    var strCallPlace = $('#DetailsCallPlace').val();
    $('#BakCallPlace').val(strCallPlace);

    //2013/12/10 TMEJ 河原 次世代サービス 工程管理機能開発 START

    VisitUpdateDate();

    //2013/12/10 TMEJ 河原 次世代サービス 工程管理機能開発 END

}

//2013/12/10 TMEJ 河原 次世代サービス 工程管理機能開発 START

//排他用更新日時を最新化する
function VisitUpdateDate() {

    //更新日時(ラベル)の値をチェック
    if ($('#DetailsVisitUpdateDateLabel').text() == "") {
        //更新日時(ラベル)にあればない場合は処理無し


    } else {
        //更新日時(ラベル)にあればその値が最新のため入替える

        $('#DetailsVisitUpdateDate').val($('#DetailsVisitUpdateDateLabel').text());


        //チップのタグにも設定する
        $("[visitNo=" + detailsVisitNo + "]").attr("updatedate", $('#DetailsVisitUpdateDateLabel').text());

    };

    


};
//2013/12/10 TMEJ 河原 次世代サービス 工程管理機能開発 END


//Divをスライドした後で、ずれるので、調整する
function AdjustDiv() {
    $("#search").attr("style", "display: none;");
    $('.contentInner').css("width", "100%");
    setTimeout(
        	    function () {
        	        $('.contentInner').css("width", "200%");
        	        $("#search").attr("style", "display: block;");
        	    }
            , 1000);
};
//2013/03/12 TMEJ 丁 【A.STEP1】来店受付管理オペレーション確立に向けた評価アプリ作成　END



//2013/12/10 TMEJ 河原 次世代サービス 工程管理機能開発 START

function AssignmentRefresh() {
    // 2019/07/18 NSK 鈴木 [TKM]PUAT-4072 SAがRO起票ボタンを押下するとくるくる表示が止まらない START
    // 画面更新中の場合、処理を終了する。
    if (gUpdatingFlag) {
        return;
    }

    // 画面更新中フラグをtrueにする
    setUpdatingFlag(true);
    // 2019/07/18 NSK 鈴木 [TKM]PUAT-4072 SAがRO起票ボタンを押下するとくるくる表示が止まらない END

    //タイマー設定
    commonRefreshTimer(RefreshDisplay);

    //アクティブインジケータの表示
    $("div.AssignmentLoadingDiv").attr("style", "display: block;");

    //振当待ちエリアのチップ詳細が開いている場合は閉じる
    //チップ詳細が表示されているかチェック
    if ($("#CustomerPopOver2").css('display') === 'block') {
        //チップ詳細が表示されている場合

        //チップ詳細が開いているエリアが振当待ちエリアかチェック
        if (detailsArea == 7) {
            //チップ詳細が開いているエリアが振当待ちの場合

            //テキストのフォーカスをはずす
            $('.TextArea').blur();
            //選択チップの解除
            UnsetChip(this);
            //チップ詳細を閉じて閉じる
            CloseChipDetail();

        };

    };

    //非同期処理のトリガーコントロール名称格納の設定
    aryPostCtrl.push("AssignmentRefreshButton");

    //リフレッシュボタンクリック
    $("#AssignmentRefreshButton").click();

    // 2019/07/18 NSK 鈴木 [TKM]PUAT-4072 SAがRO起票ボタンを押下するとくるくる表示が止まらない START
    // 画面更新中フラグをfalseにする
    setUpdatingFlag(false);
    // 2019/07/18 NSK 鈴木 [TKM]PUAT-4072 SAがRO起票ボタンを押下するとくるくる表示が止まらない END
};

//2013/12/10 TMEJ 河原 次世代サービス 工程管理機能開発 END

// 2019/07/18 NSK 鈴木 [TKM]PUAT-4072 SAがRO起票ボタンを押下するとくるくる表示が止まらない START
/**
* 画面更新中フラグ設定。<br>
* 画面更新中フラグを設定する.
*
* @param {boolean} aUpdatingFlag 画面更新中フラグ
*/
function setUpdatingFlag(aUpdatingFlag) {
    gUpdatingFlag = aUpdatingFlag;
};
// 2019/07/18 NSK 鈴木 [TKM]PUAT-4072 SAがRO起票ボタンを押下するとくるくる表示が止まらない END

