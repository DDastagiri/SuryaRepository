//SC3150102.js
//------------------------------------------------------------------------------
//機能：メインメニュー（TC）_R/O情報タブ_javascript
//更新：12/06/15 KN 西田     STEP1 重要課題対応 作業終了時、R/O情報欄にグレーフィルターがかからない
//更新：12/08/09 TMEJ 小澤   【SERVICE_2】矢印アイコン制御追加
//更新：12/11/14 TMEJ 彭健   アクティブインジゲータ対応（クルクルのタイムアウト対応）、サイズ削減の為に古い履歴を削除
//更新：2012/11/30 TMEJ 小澤 【SERVICE_2】SAストール予約受付機能開発（仕分けNo.75）
//更新：2012/11/30 TMEJ 成澤  TCステータスモニターへの遷移機能
//更新：2013/11/26 TMEJ 成澤 【IT9573】次世代e-CRBサービス 店舗展開に向けた標準作業確立
//更新：2013/12/12 TMEJ 成澤 【開発】IT9611_次世代サービス 工程管理機能開発
//更新：2014/07/23 TMEJ 成澤 【開発】IT9711_タブレットSMB Job Dispatch機能開発
//更新：2014/08/29 TMEJ 成澤 【開発】IT9737_NextSTEPサービス ロケ管理の効率化に向けた評価用アプリ作成
//更新：2014/09/12 TMEJ 成澤  自主研追加対応_ROプレビュー遷移
//更新：2014/12/05 TMEJ 岡田　IT9857_DMS連携版サービスタブレット JobDispatch完成検査入力制御開発
//更新：2015/02/10 TMEJ 成澤  BTS-TMT2販売店 No.82 クロスドメインにより画面リフレッシュされない 対応
//更新：2016/03/30 NSK  小牟禮 アクティビティインジケータが消えない問題
//更新：2019/06/03 NSK 鈴木 18PRJ03142_(FS)業務検証:セールス受注前活動活動結果登録オペレーション[TKM]PUAT-4109 部品詳細にて、バックオーダー(B/O)が表示されない
//更新：2019/08/01 NSK 近藤 (トライ店システム評価)次世代サービスオペレーション効率化に向けた、業務適合性検証
//更新：2019/12/06 NSK 皆川 (トライ店システム評価)整備作業における休憩時間取得判定向上検証
//更新：
//------------------------------------------------------------------------------

//基本情報タブのCSSクラス名
var C_ROTAB_CLASS_BASE = "TabButton01";
//ご用命事項タブのCSSクラス名
var C_ROTAB_CLASS_ORDER = "TabButton02";
//作業内容タブのCSSクラス名
var C_ROTAB_CLASS_WORK = "TabButton03";

//基本情報タブのタブ番号
var C_ROTAB_CLASS_BASE_NUMBER = 1;
//ご用命事項タブのタブ番号
var C_ROTAB_CLASS_ORDER_NUMBER = 2;
//作業内容タブのタブ番号
var C_ROTAB_CLASS_WORK_NUMBER = 3;

//display属性の設定値
//表示しない
var C_DISPLAY_NONE = "none";
//表示する
var C_DISPLAY_BLOCK = "inline-block";

//基本情報のデータ定数
//燃料
var C_BASIC_FUEL_EMPTY = "0";
var C_BASIC_FUEL_QUARTER = "1";
var C_BASIC_FUEL_HALF = "2";
var C_BASIC_FUEL_THREE_QUARTER = "3";
var C_BASIC_FUEL_FULL = "4";
//オーディオ
var C_BASIC_AUDIO_OFF = "0";
var C_BASIC_AUDIO_CD = "1";
var C_BASIC_AUDIO_FM = "2";
//エアコン
var C_BASIC_AIR_CONDITIONER_OFF = "0";
var C_BASIC_AIR_CONDITIONER_ON = "1";
//付属品
var C_BASIC_ACCESSORY_CHECKED = "1";
var C_BASIC_ACCESSORY_MAX = 6;

//ご用命事項・確認事項のデータ定数
//交換部品
var C_ORDER_EXCHANGE_PARTS_TAKEOUT = "0";
var C_ORDER_EXCHANGE_PARTS_INSURANCE = "1";
var C_ORDER_EXCHANGE_PARTS_DISPOSE = "2";
//待ち方
var C_ORDER_WAITING_IN = "0";
var C_ORDER_WAITING_OUT = "1";
//洗車
var C_ORDER_WASHING_DO = "1";
var C_ORDER_WASHING_NONE = "0";
//支払方法
var C_ORDER_PAYMENT_CASH = "0";
var C_ORDER_PAYMENT_CARD = "1";
var C_ORDER_PAYMENT_OTHER = "2";
//CSI時間
var C_ORDER_CSI_AM = "1";
var C_ORDER_CSI_PM = "2";
var C_ORDER_CSI_ALWAYS = "0";

//ご用命事項・問診項目のデータ定数
//WNG
var C_ORDER_WNG_ALWAYS = "1";
var C_ORDER_WNG_OFTEN = "2";
var C_ORDER_WNG_NONE = "0";
//故障発生時間
var C_ORDER_OCCURRENCE_RECENTLY = "0";
var C_ORDER_OCCURRENCE_WEEK = "1";
var C_ORDER_OCCURRENCE_OTHER = "2";
//故障発生頻度
var C_ORDER_FREQUENCY_HIGH = "0";
var C_ORDER_FREQUENCY_OFTEN = "1";
var C_ORDER_FREQUENCY_ONCE = "2";
//再現可能
var C_ORDER_REAPPEAR_YES = "1";
var C_ORDER_REAPPEAR_NO = "0";
//水温
var C_ORDER_WATERT_LOW = "0";
var C_ORDER_WATERT_HIGH = "1";
//気温
var C_ORDER_TEMPERATURE_LOW = "0";
var C_ORDER_TEMPERATURE_HIGH = "1";
//発生場所
var C_ORDER_PLACE_PARKING = "0";
var C_ORDER_PLACE_ORDINARY = "1";
var C_ORDER_PLACE_MOTORWAY = "2";
var C_ORDER_PLACE_SLOPE = "3";
//渋滞状況
var C_ORDER_TRAFFICJAM_HAPPEN = "1";
var C_ORDER_TRAFFICJAM_NONE = "0";
//車両状態
var C_ORDER_CARSTATUS_ON = "1";
var C_ORDER_CARSTATUS_OFF = "0";
//var C_ORDER_CARSTATUS_STARTUP = "1";
//var C_ORDER_CARSTATUS_IDLLING = "2";
//var C_ORDER_CARSTATUS_COLD = "3";
//var C_ORDER_CARSTATUS_WARM = "4";
//走行時
var C_ORDER_TRAVELING_LOWSPEED = "0";
var C_ORDER_TRAVELING_ACCELERATION = "1";
var C_ORDER_TRAVELING_SLOWDOWN = "2";
//非純正用品
var C_ORDER_NONGENUINE_YES = "1";
var C_ORDER_NONGENUINE_NO = "0";

//部品準備が完了していない状態
var C_PARTS_REPARE_UNPREPARED = "0";
//部品準備が完了している状態
var C_PARTS_REPARE_PREPARED = "1";

//左フリックをしたと判定する値
var C_LEFT_FLICK_THRESHOLD = -200;

//R/O情報欄のフィルタフラグ：フィルタをかける
var C_REPAIR_ORDER_FILTER_ON = "1";
//R/O情報欄のフィルタフラグ：フィルタをかけない
var C_REPAIR_ORDER_FILTER_OFF = "0";

var C_WORK_END_FLG = "1";

//フリック移動距離
var gDiffX = 0;

//追加作業アイコンを一度に表示可能な数
var C_ICON_DISPLAY_LIMIT = 3;
//追加作業アイコンの1アイコンに必要な描画幅

var C_ICON_WIDTH = 32;

var C_ICON_SCROLLMAX = 92;
//追加作業アイコンを移動するときの動作時間
var C_ICON_MOVE_TIME = 1;

//追加作業アイコンを配置するBoxの幅
var gPagingDivMaxLen = 1;
var gScrollNowLen = 0;
//追加作業アイコンの移動変数
var gScrollNumber = 0;

//追加作業アイコンの選択されているインデックス
var selectedAddWorkIndex = 0;
//追加作業アイコンの表示数
var maxAddWorkIndex = 0
//追加作業スクロール領域
var elementDivScroll = ''

//2013/11/26 TMEJ 成澤【IT9573】次世代e-CRBサービス 店舗展開に向けた標準作業確立 START
//特定の販売店コード
var specificDealerCode = "44A10";
//2013/11/26 TMEJ 成澤【IT9573】次世代e-CRBサービス 店舗展開に向けた標準作業確立 END

//2014/07/23 TMEJ 成澤　【開発】IT971_タブレットSMB Job Dispatch機能開発 START

//休憩Popupの表示フラグ：表示
var C_BREAK_POPUP_DISPLAY = "1";
//休憩Popupの表示フラグ：非表示
var C_BREAK_POPUP_NONE = "0";

var JOB_START_BUTTON_ID = "JobStartButton";
var JOB_FINISH_BUTTON_ID = "JobFinishButton";
var JOB_STOP_BUTTON_ID = "JobStopButton";
var JOB_RESTART_BUTTON_ID = "JobReStartButton";

var CAll_BY_SC3150102 = "1";
var CAll_BY_SC3150102_2 = "2";

var JOB_STATUS_WORKIG = "0";
var JOB_STATUS_COMPLETE = "1";
var JOB_STATUS_STOP = "2";
var StopJobCount = 0;
var FinishJobCount = 0;
var AllJobCount = 0;
var RestJobCount = 0;
var STALL_USE_STATUS_STOP = "05";
//2014/07/23 TMEJ 成澤　【開発】IT9711_タブレットSMB Job Dispatch機能開発 END

//DOMロード直後の処理(重要事項)
$(function () {
    //window.onerror = function (desc, page, line, chr) { alert('[Error caught by SC3150102.js]' + ' desc:' + desc + ', page:' + page + ', line:' + line + ', chr:' + chr); }

    //2015/02/10 TMEJ 成澤  BTS-TMT2販売店 No.82 クロスドメインにより画面リフレッシュされない 対応 START
    //リフレッシュフラグONの場合画面をリフレシュ
    if ($("#HiddenRefreshFlg").val() == "1") {
        $("#HiddenRefreshFlg").val("0");
        parentScreenReLoad();
        return;
    }
    //2015/02/10 TMEJ 成澤  BTS-TMT2販売店 No.82 クロスドメインにより画面リフレッシュされない 対応 END

    try {

        //スモークフィルタの設置フラグを取得
        var str01Box03FilterFlag = $("#Hidden01Box03Filter").val();

        var workEndFlg = parent.getEndWorkFlg();

        //グレーフィルターOFF、且つ作業終了フラグONでない場合は、グレーフィルターかけない
        if (str01Box03FilterFlag == C_REPAIR_ORDER_FILTER_OFF && workEndFlg != C_WORK_END_FLG) {
            $(".stc01Box03").css("opacity", 1);
        } else {
            $(".stc01Box03").css("opacity", 0.5);
            //2014/07/23 TMEJ 成澤 【開発】IT9711_タブレットSMB Job Dispatch機能開発 START
            $(".w05").css("pointer-events", "none");
            //2014/07/23 TMEJ 成澤 【開発】IT9711_タブレットSMB Job Dispatch機能開発 END
        }

        // 部品準備完了フラグ取得
        var strPartsReadyFlag = parent.getPartsReadyFlag();
        //部品準備のスモークフィルタの設置処理
        if (strPartsReadyFlag == C_PARTS_REPARE_PREPARED) {
            //部品詳細の実体化
            $("#S-TC-01RightBody").css("opacity", "1");
            //部品詳細フィルタの透明化
            $(".S-TC-01RightScrollFilter").css("opacity", "0");
        } else {
            //部品詳細の半透明化
            $("#S-TC-01RightBody").css("opacity", "0.5");
            //部品詳細フィルタの実体化
            $(".S-TC-01RightScrollFilter").css("opacity", "1");
        }

        //基本情報・ご用命事項・作業内容パネルの選択処理をクリックイベントにバインドする.
        $(".Box03In > .TabButtonSet > ul > li").bind("touchstart click", function () {
            clickTabButtonSet(this);
        });

        //ご用命事項の確認事項・問診項目の選択処理をクリックイベントにバインドする.
        $(".TabBox02 > .S-TC-07TabWrap > .S-TC-07Right > .S-TC-07RightTab > ul > li").bind("click", function () {
            clickOrderTab(this);
        });

        //ご用命事項の問診項目の「走行時」をクリックしたときの動作をバインドする.
        $("#S-SA-07Tab02Right1-5-1").bind("touch click", function () {
            clickOrderTabTraveling();
        });

        //履歴情報をタップした時の処理をバインドする.
        // 2012/11/30 TMEJ 小澤【SERVICE_2】SAストール予約受付機能開発（仕分けNo.75）START
        $("#S-TC-05RightScroll > .S-TC-05Right1-1").bind("touchstart click", function () {
            clickHistory(this);
        });

        $("#S-TC-05RightScroll .S-TC-05Right1-1").bind("touchstart click", function () {
            clickHistory(this);
        });
        // 2012/11/30 TMEJ 小澤【SERVICE_2】SAストール予約受付機能開発（仕分けNo.75）END
        //基本情報のデータ表示設定
        initBasicInfo();
        //作業内容の追加作業アイコンの作成
        createChildChipIcon();
        //追加作業アイコンの表示
        scrollOnload();

        //基本情報の入庫履歴にフリックイベントを設定.
        //基本情報の入庫履歴は、最大5件ということなので、そもそもスクロール可能とならない.
        //少しでも負担を減らすため、flickableは設定しない.
        $("#S-TC-05RightScroll").fingerScroll();
        //ご用命事項のご用命事項欄にフリックイベントを設定.
        $('#S-TC-07LeftMemo2').fingerScroll();
        //ご用命事項の各タブにフリックイベントを設定.
        $('#S-TC-07RightScroll').fingerScroll();
        //作業内容の各テーブルにフリックイベントを設定.
        $("#S-TC-01LeftBody").fingerScroll();
        $("#S-TC-01RightBody").fingerScroll();

        //R/O情報パネル全体にフリックイベントを設定.
        $(".Box03In").bind("touchstart mousedown", function (event) {
            flickStart(event);
        });

        //フリックイベント設定時、display:noneに設定されているとその箇所はフリックできないため、
        //初期状態では表示しておき、フリックイベント設定後、デフォルト以外を非表示設定にする.
        $("#S-TC-07RightTab_01").click();           //ご用命事項タブ・確認事項タブ押下処理
        $("." + C_ROTAB_CLASS_WORK).click();        //作業内容タブ押下処理
        $("#S-SA-07Tab02Right1-5-1").click();       //走行時ボタン押下処理

        //親ページのR/Oステータスに値を格納する.
        parent.setOrderStatus($("#HiddenFieldOrderStatus").val())

        //2013/12/12 TMEJ 成澤 【開発】IT9611_次世代サービス 工程管理機能開発　START
        //        //2012/08/09 TMEJ 小澤【SERVICE_2】矢印アイコン制御追加 START
        //        //▼の表示
        //       parent.setArrowImage();
        //        //2012/08/09 TMEJ 小澤【SERVICE_2】矢印アイコン制御追加 END
        //2013/12/12 TMEJ 成澤 【開発】IT9611_次世代サービス 工程管理機能開発　END

        //作業進捗エリアのR/ONoに枝番表示するために、親ページにR/ONoの枝番を投げる.
        parent.setSrvAddSeq($("#HiddenFieldTactSrvAddSeq").val())
        //親ページに担当SA名を投げる.
        parent.setSaName($("#HiddenFieldSAName").val())

        //2013/11/26 TMEJ 成澤【IT9573】次世代e-CRBサービス 店舗展開に向けた標準作業確立 START
        //FM 呼出しボタンの表示・非表示制御
        //        if ($("#HiddenFieldOrderNo", top.document).val() != "" ) {
        //            $("#ButtonSendNoticeToFM").show()
        //        } else {
        //            $("#ButtonSendNoticeToFM").hide()
        //        }

        //2014/07/23 TMEJ 成澤　【開発】IT9711_タブレットSMB Job Dispatch機能開発 START
        //        if ($("#HiddenFieldOrderNo", top.document).val() != "" && $("#HiddenSelectdealerCode", top.document).val() == specificDealerCode) {
        //            $("#ButtonSendNoticeToFM").show()
        //        } else {
        //            $("#ButtonSendNoticeToFM").hide()
        //        }
        //2014/07/23 TMEJ 成澤　【開発】IT9711_タブレットSMB Job Dispatch機能開発 END

        //2013/11/26 TMEJ 成澤【IT9573】次世代e-CRBサービス 店舗展開に向けた標準作業確立 END
        var liItems;

        //作業グループのポップアップ設定
        var triggerItems = $("div[id^='divWorkgroup_']:has('.Popoverable'),#CustomerLiteral328");

        //2013/12/12 TMEJ 成澤 【開発】IT9611_次世代サービス 工程管理機能開発 START

        //        if ($("div[id^='divWorkgroup_']:has('.Popoverable')").length > 0) { // 選択されているChipに紐付いた整備が一つ以上の場合のみ、作業グループのPopoverが表示可能にする
        //            $("#CustomerLiteral328").addClass("UnderLine");

        //            $(".Popoverable").each(function () {
        //                if (0 == $(this).text().trim().length) {
        //                    $(this).html("&nbsp;");
        //                }
        //            });

        //            triggerItems.bind("touchstart click", function (event) {
        //                triggerItems.trigger('hideOpenPopover');
        //                $("#poWorkgroupList").attr("data-TriggerClientID", $(this).attr("id"));
        //                return true;
        //            });

        //            triggerItems.popover({
        //                header: "#poWorkgroupList_header",
        //                content: "#poWorkgroupList_content",
        //                openEvent: function () {
        //                    var selectOffset = 0;
        //                    var triggerClientID = "#" + $("#poWorkgroupList").attr("data-TriggerClientID");

        //                    liItems = $("#poWorkgroupList_content .innerDataBox ul li");
        //                    if ("#CustomerLiteral328" == triggerClientID) {     // 一括モード
        //                        liItems.removeClass("Check");
        //                        liItems.bind("click", feedbackAllWorkgroupSettingAndClosePopover);
        //                    } else {                                            // 個別モード
        //                        var workgroupSelected = $("#" + $("#poWorkgroupList").attr("data-TriggerClientID") + " #HiddenFieldWorkByCode").val().trim();
        //                        liItems.each(function () {
        //                            if ($(this).next().val().trim() == workgroupSelected) {
        //                                $(this).addClass("Check");
        //                                if (liItems.length > 8) {
        //                                    selectOffset = Math.min(liItems.index(this), liItems.length - 8) * 44;    // TODO: use liItems.css("line-height) without unit instead of 44
        //                                }
        //                            } else {
        //                                $(this).removeClass("Check");
        //                            }
        //                        });
        //                        liItems.bind("click", feedbackOneWorkgroupSettingAndClosePopover);
        //                    }

        //                    $("#divPopoverScroll").fingerScroll();
        //                    $("#divPopoverScroll").fingerScroll({ action: "move", moveY: selectOffset, moveX: 0 });
        //                },
        //                closeEvent: function () {
        //                    liItems.unbind("click");
        //                }
        //            });

        //            $("#btnRegisterWorkgroup").bind("touchstart click", function () {
        //                parent.LoadingScreen();

        //                setTimeout(function () {
        //                    parent.reloadPageIfNoResponse();
        //                    HiddenButtonRegisterWorkgroup.click();
        //                }, 0);

        //                $("#btnRegisterWorkgroup").hide();
        //                $("#CustomerLiteral328").show();
        //            });
        //        } else {
        //            $("#CustomerLiteral328").removeClass("UnderLine");
        //        }

        //2013/12/12 TMEJ 成澤 【開発】IT9611_次世代サービス 工程管理機能開発 END
        if (0 == $("#lblCageNo").text().trim().length) {
            $("#lblCageNo").html("&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;");
        }

        //親ページでポップアップの制御を実施する.
        $("*").bind("click.popover", function (event) {
            parent.ParentPopoverClose();
        });
    }
    finally {

        //2016/03/30 NSK 小牟禮 アクティビティインジケータが消えない問題 START
        //リロードが終了したタイミングで親のリロードフラグをOFFにする
        //parent.InitReloadFlag();
        //parent.StopLodingIcon('#loadingroInfomation');
        //parent.UnloadingScreen();
        //parent.clearTimer();

        //クルクル非表示
        parent.UnloadingScreen();
        //2016/03/30 NSK 小牟禮 アクティビティインジケータが消えない問題 END
    }

    //2013/02/21 TMEJ 成澤【SERVICE_2】TCステータスモニターへの遷移機能 START


    //マウスクリックイベント
    $('html').mousedown(function (e) {
        //親ページのスクリーンセイバータイマーリセット
        parent.ScreenTimerRestart();
    });
    //2013/02/21 TMEJ 成澤【SERVICE_2】TCステータスモニターへの遷移機能 END

    //2013/12/10　TMEJ　成澤【開発】IT9611_次世代サービス 工程管理機能開発 START
    setIframeUrl();

    //三点文字の設定
    $(".Ellipsis").CustomLabel({ useEllipsis: true });
    //2013/12/10　TMEJ　成澤【開発】IT9611_次世代サービス 工程管理機能開発 END

    //2014/07/23 TMEJ 成澤　【開発】IT9711_タブレットSMB Job Dispatch機能開発 START

    //JobActionボタンクリックイベント
    $(".BtnOn").bind("click", function (e) {

        var clickBtnId = $(this).attr("id");

        selectedJobItem(this);

        //2016/03/30 NSK 小牟禮 アクティビティインジケータが消えない問題 START
        //switch (clickBtnId) {
        //    //スタートボタン               
        //    case JOB_START_BUTTON_ID:
        //        $("#HiddenButtonJobStart").click();
        //        //クルクル表示
        //        parent.LoadingScreen();
        //        break;
        //    //フィニッシュボタン               
        //    case JOB_FINISH_BUTTON_ID:
        //        $("#HiddenButtonJobFinish").click();
        //        //クルクル表示
        //        parent.LoadingScreen();
        //        break;
        //    //ストップボタン               
        //    case JOB_STOP_BUTTON_ID:
        //        //呼び出し元フラグを設定
        //        parent.WindowCallByFlg(CAll_BY_SC3150102);
        //        //中断理由ポップアップ表示
        //        parent.ChanselPopUp();
        //        break;
        //    //リスタートボタン               
        //    case JOB_RESTART_BUTTON_ID:
        //        $("#HiddenButtonJobStart").click();
        //        //クルクル表示
        //        parent.LoadingScreen();
        //        break;
        //
        //}
        switch (clickBtnId) {
            //スタートボタン                  
            case JOB_START_BUTTON_ID:

                //クルクル表示
                parent.LoadingScreen();

                setTimeout(function () {
                    $("#HiddenButtonJobStart").click();
                }, 0);

                break;
            //フィニッシュボタン                  
            case JOB_FINISH_BUTTON_ID:

                //クルクル表示
                parent.LoadingScreen();

                setTimeout(function () {
                    $("#HiddenButtonJobFinish").click();
                }, 0);

                break;
            //ストップボタン                  
            case JOB_STOP_BUTTON_ID:
                //呼び出し元フラグを設定
                parent.WindowCallByFlg(CAll_BY_SC3150102);
                //中断理由ポップアップ表示
                parent.ChanselPopUp();
                break;
            //リスタートボタン                  
            case JOB_RESTART_BUTTON_ID:

                //クルクル表示
                parent.LoadingScreen();

                setTimeout(function () {
                    $("#HiddenButtonJobStart").click();
                }, 0);

                break;

        }
        //2016/03/30 NSK 小牟禮 アクティビティインジケータが消えない問題 END

    });

    //休憩をとる・とらないを問うpopupを表示する処理.
    if ($("#HiddenBreakPopupChild").val() == C_BREAK_POPUP_DISPLAY) {
        //フラグを初期化する.
        $("#HiddenBreakPopupChild").val(C_BREAK_POPUP_NONE);
        parent.selectClass(CAll_BY_SC3150102);
    }

    ////2014/09/12 TMEJ 成澤  自主研追加対応_ROプレビュー遷移 START
    //ROアイコンタップイベント
    $(".imgicon01").bind("touchstart mousedown", function (event) {
        clickRepairOrderIco(this);
        //2016/03/30 NSK 小牟禮 アクティビティインジケータが消えない問題 START
        //parent.LoadingScreen();
        //2016/03/30 NSK 小牟禮 アクティビティインジケータが消えない問題 END
    });

    $(".imgicon02").bind("touchstart mousedown", function (event) {
        clickRepairOrderIco(this);
        //2016/03/30 NSK 小牟禮 アクティビティインジケータが消えない問題 START
        //parent.LoadingScreen();
        //2016/03/30 NSK 小牟禮 アクティビティインジケータが消えない問題 END
    });
    ////2014/09/12 TMEJ 成澤  自主研追加対応_ROプレビュー遷移　END

    //残りのjobの数を計算
    countRestJob();

    //2014/07/23 TMEJ 成澤　【開発】IT9711_タブレットSMB Job Dispatch機能開発 END

    //2014/12/05 TMEJ 岡田　DMS連携版サービスタブレット JobDispatch完成検査入力制御開発 STRAT

    JobFinishBtnDispCtrl();

    //2014/12/05 TMEJ 岡田　IT9857_DMS連携版サービスタブレット JobDispatch完成検査入力制御開発 END

});


function feedbackOneWorkgroupSettingAndClosePopover() {
    var pop = $("#poWorkgroupList");
    var triggerClientID = "#" + pop.attr("data-TriggerClientID");

    if ( $(triggerClientID + " #HiddenFieldWorkByCode").val() != $(this).next().val() ) {
        $(triggerClientID + " #LabelWorkgroupInfo").text($(this).text());
        $(triggerClientID + " #HiddenFieldWorkByCode").val($(this).next().val());

        $("#CustomerLiteral328").hide();
        $("#btnRegisterWorkgroup").show();
    }

    $(triggerClientID).trigger('hidePopover');

    return true;
}


function feedbackAllWorkgroupSettingAndClosePopover() {
    var pop = $("#poWorkgroupList");
    var triggerClientID = "#" + pop.attr("data-TriggerClientID");
    var destItems = $(".Popoverable");
    var srcItem = $(this);
    var haveChange = false;

    destItems.each(function (i) {
        if ($(this).next().val() != srcItem.next().val()) {
            $(this).text(srcItem.text());
            $(this).next().val(srcItem.next().val());
            haveChange = true;
        }
    });

    if (haveChange) {
        $("#CustomerLiteral328").hide();
        $("#btnRegisterWorkgroup").show();
    }

    $(triggerClientID).trigger('hidePopover');

    return true;
}


/**
 * R/O情報欄におけるフリック開始イベント処理
 * @param {event} event
 * @return {void}
 */
function flickStart(event) {

    //開始位置座標値を取得する.
    if (event.type === "touchstart") {
//        startX = event.originalEvent.touches[0].pageX;
        startX = event.originalEvent.pageX;
    } else {
        startX = event.pageX;
    }

    //開始イベントをアンバインドし、移動・終了イベントをバインドする.
    $(".Box03In").unbind("touchstart mousedown")
    .bind("touchmove mousemove", function (event) {
        flickMove(event);
    }).bind("touchend mouseup mouseleave", function (event) {
        flickEnd(event)
    });
}

/**
 * R/O情報欄における、フリック移動イベント処理
 * @param {event} event
 * @return {void}
 */
function flickMove(event) {

    //移動座標を取得し、開始座標からの差異を取得する.
    if (event.type === "touchmove") {
        //pointX = event.originalEvent.touches[0].pageX;
        pointX = event.originalEvent.pageX;
    } else {
        pointX = event.pageX;
    }
    gDiffX = pointX - startX;
}

/**
 * R/O情報欄における、フリック終了イベント処理
 * @param {event} event
 * @return {void}
 */
function flickEnd(event) {
    
    //移動・終了イベントをアンバインドし、開始イベントをバインドする.
    $(".Box03In").unbind("touchmove mousemove touchend mouseup mouseleave")
    .bind("touchstart mousedown", function(event) {
        flickStart(event);
    });

    //左フリックを実施したと判定される値より、移動距離が大きい場合、左フリック処理を実施する.
    if (gDiffX < C_LEFT_FLICK_THRESHOLD) {
        //親画面のR/O情報左フリック処理を呼び出す.
        parent.flickRepairOrderInfomation();
    }
}


/**
 * 基本情報・ご用命事項・作業内容パネルを選択時イベント処理
 * @param {Object} tapObject 選択されたオブジェクト
 * @return {void}
 */
function clickTabButtonSet(tapObject) {
    var clickClassName = $(tapObject).attr("class");
    var clickTabNumber = 0;

    var display01 = C_DISPLAY_NONE;
    var display02 = C_DISPLAY_NONE;
    var display03 = C_DISPLAY_NONE;

    //取得したクラス名がTabButton01の場合、タブナンバーに1を返す
    if (clickClassName == (C_ROTAB_CLASS_BASE)) {
        display01 = C_DISPLAY_BLOCK;
        clickTabNumber = C_ROTAB_CLASS_BASE_NUMBER;
    } else if (clickClassName == (C_ROTAB_CLASS_ORDER)) {
        display02 = C_DISPLAY_BLOCK;
        clickTabNumber = C_ROTAB_CLASS_ORDER_NUMBER;
    } else if (clickClassName == (C_ROTAB_CLASS_WORK)) {
        display03 = C_DISPLAY_BLOCK;
        clickTabNumber = C_ROTAB_CLASS_WORK_NUMBER;
    }
    $(".TabBox01").css("display", display01);
    $(".TabBox02").css("display", display02);
    $(".TabBox03").css("display", display03);

    $("." + C_ROTAB_CLASS_BASE + " > div").toggleClass("Rollover", (clickClassName == C_ROTAB_CLASS_BASE));
    $("." + C_ROTAB_CLASS_BASE + " > div").toggleClass("Button", (clickClassName != C_ROTAB_CLASS_BASE));
    $("." + C_ROTAB_CLASS_ORDER + " > div").toggleClass("Rollover", (clickClassName == C_ROTAB_CLASS_ORDER));
    $("." + C_ROTAB_CLASS_ORDER + " > div").toggleClass("Button", (clickClassName != C_ROTAB_CLASS_ORDER));
    $("." + C_ROTAB_CLASS_WORK + " > div").toggleClass("Rollover", (clickClassName == C_ROTAB_CLASS_WORK));
    $("." + C_ROTAB_CLASS_WORK + " > div").toggleClass("Button", (clickClassName != C_ROTAB_CLASS_WORK));

    //親画面に渡すための部品情報数を取得する.
    var partsCount = $("#HiddenFieldPartsCount").val();

    // 2019/06/03 NSK 鈴木 18PRJ03142_(FS)業務検証:セールス受注前活動活動結果登録オペレーション [TKM]PUAT-4109 部品詳細にて、バックオーダー(B/O)が表示されない START
    // //親画面に渡すためのB/O数を取得する.
    // var backOrderCount = $("#HiddenFieldPartsBackOrderCount").val();
    // 2019/06/03 NSK 鈴木 18PRJ03142_(FS)業務検証:セールス受注前活動活動結果登録オペレーション [TKM]PUAT-4109 部品詳細にて、バックオーダー(B/O)が表示されない END

    var inspectionApproval = $("#HiddenFieldInspectionApprovalFlag").val();

    //親画面のタブ変更メソッドを呼び出す
    // 2019/06/03 NSK 鈴木 18PRJ03142_(FS)業務検証:セールス受注前活動活動結果登録オペレーション [TKM]PUAT-4109 部品詳細にて、バックオーダー(B/O)が表示されない START
    // parent.CheckChengeTab(clickTabNumber, partsCount, backOrderCount, inspectionApproval);
    parent.CheckChengeTab(clickTabNumber, partsCount, inspectionApproval);
    // 2019/06/03 NSK 鈴木 18PRJ03142_(FS)業務検証:セールス受注前活動活動結果登録オペレーション [TKM]PUAT-4109 部品詳細にて、バックオーダー(B/O)が表示されない END

    // 作業内容タブが選択された際に追加作業チップの表示位置をリフレッシュ
    if (clickClassName == C_ROTAB_CLASS_WORK && elementDivScroll != '') {
        // 追加作業アイコンの個数を取得
        var maxAddWorkIndex = changeIntegerFromString($("#HiddenFieldAddWorkCount").val()) + 1;

        // 選択しているチップが最左にいかない場合、選択チップを最左にできるチップのIndexに変更
        if (maxAddWorkIndex - 2 < selectedAddWorkIndex + 1) {
            selectedAddWorkIndex = maxAddWorkIndex - 3;
        }
        elementDivScroll.flickable('select', selectedAddWorkIndex);
    }
}


/**
 * ご用命事項の確認事項・問診項目の選択時のイベント処理
 * @param {Object} selectedOrderTab
 * @return {void}
 */
function clickOrderTab(selectedOrderTab) {

    //クリックされたIDを取得する
    var clickTabId = $(selectedOrderTab).attr("id");

    var rightTabDisplay01 = C_DISPLAY_NONE;
    var rightTabDisplay02 = C_DISPLAY_NONE;
    //確認事項タブである場合、確認事項エリアを表示可能にする
    if (clickTabId == "S-TC-07RightTab_01") {
        rightTabDisplay01 = C_DISPLAY_BLOCK;
    } else if (clickTabId == "S-TC-07RightTab_02") {
        rightTabDisplay02 = C_DISPLAY_BLOCK;
    }

    $(".S-TC-07RightBody").css("display", rightTabDisplay01);
    $(".S-TC-07RightScroll").css("display", rightTabDisplay02);

    //確認事項タブでない場合、選択されていないクラスを追加（確認事項タブが選択された場合は除去）
    $("#S-TC-07RightTab_01").toggleClass("S-TC-07RightTabNoSelected", (clickTabId != "S-TC-07RightTab_01"));
    //問診項目タブでない場合、選択されていないクラスを追加（問診項目タブが選択された場合は除去）
    $("#S-TC-07RightTab_02").toggleClass("S-TC-07RightTabNoSelected", (clickTabId != "S-TC-07RightTab_02"));
}


/**
 * ご用命事項の「走行時」のタップイベント処理
 * @return {void}
 */
function clickOrderTabTraveling() {
    //「走行時」のdisplay情報を取得する
    var _display = $("#S-SA-07Tab02Right1-5-1Display").css("display");
    var _nextDisplay = C_DISPLAY_NONE;

    //display情報に応じて、走行時以降の表示状態を設定する
    if (_display == C_DISPLAY_BLOCK) {
        _nextDisplay = C_DISPLAY_NONE;
    } else {
        _nextDisplay = C_DISPLAY_BLOCK;
    }
    //変更後のdisplay状態を設定する
    $("#S-SA-07Tab02Right1-5-1Display").css("display", _nextDisplay);

    //走行時の非活性状態クラスの設定
    $("#S-SA-07Tab02Right1-5-1").toggleClass("S-SA-07Tab02Right1-5-1Off", (_nextDisplay == C_DISPLAY_NONE));
    //走行時の活性状態クラスの設定
    $("#S-SA-07Tab02Right1-5-1").toggleClass("S-SA-07Tab02Right1-5-1", (_nextDisplay == C_DISPLAY_BLOCK));
}


/**
 * 追加作業アイコンの初期表示位置を決定し、初期表示する.
 * @return {void}
 */
function scrollOnload() {

    //枝番（親チップを含まない数量）
    changeImageLen = changeIntegerFromString($("#HiddenFieldAddWorkCount").val());

	//追加作業アイコンを格納するBoxの幅を設定する.
    gPagingDivMaxLen = C_ICON_WIDTH * (changeImageLen + 1);

    $("#divScroll ul").css({ "width": gPagingDivMaxLen });

    //枝番がアイコンの表示上限未満の場合、＜＞ボタンを表示しない.
    if (changeImageLen < C_ICON_DISPLAY_LIMIT) {
        $("#S-TC-01Paging").css({ "background": "none" });      // Page_loadが二回実行されるバグを修正するために、"url()"を"none"に変更

    } else {
        elementDivScroll = $('#divScroll').flickable({
            section: 'li',
            cancel: 'ul',
            elasticConstant: 0.4,
            friction: 0.5
        });

        ////活性状態のアイコン番号を取得する（0から）.
        //選択チップのTact枝番を取得する.
        selectedAddWorkIndex = changeIntegerFromString($("#HiddenSelectedWorkSeq", top.document).val());    // 0値で親チップ（R）、1以降でworkSeq番目の追加作業となる
        var childNo = changeIntegerFromString($("#HiddenFieldChildNo", top.document).val());                // 子予約連番（TBL_STALLREZINFOのREZCHILDNO）。0:取引、999:納車

        if (childNo == 999) {
	        //納車チップの場合は親チップとする(引取チップは0のため考慮いらず)
	        selectedAddWorkIndex = 0;
	    }
	    //追加作業アイコン数を取得する.（ROアイコン分を加算）
	    maxAddWorkIndex = changeIntegerFromString($("#HiddenFieldAddWorkCount").val()) + 1;

	    // 選択しているチップが最左にいかない場合、選択チップを最左にできるチップのIndexに変更
	    if (maxAddWorkIndex - 2 < selectedAddWorkIndex + 1) {
	        selectedAddWorkIndex = maxAddWorkIndex - 3;
	    }

	    //追加作業アイコンの表示位置初期設定.
	    elementDivScroll.flickable('select', selectedAddWorkIndex);

	    //＜＞ボタンのクリック処理.
	    $("#S-TC-01Paging > li.liFast").click(function () {
	        if (selectedAddWorkIndex <= maxAddWorkIndex - 4) {
	            selectedAddWorkIndex++
	            elementDivScroll.flickable('select', selectedAddWorkIndex);
	            return false;
	        }
	    });

	    $("#S-TC-01Paging > li.liLast").click(function () {
	        if (selectedAddWorkIndex > 0) {
	            selectedAddWorkIndex--
	            elementDivScroll.flickable('select', selectedAddWorkIndex);
	            return false;
	        }
	    });
    }
}


/**
 * 作業内容タブの、R/O追加作業チップを作成する.
 * @return {void}
 */
function createChildChipIcon() {

    var addWorkCount = changeIntegerFromString($("#HiddenFieldAddWorkCount").val());            // 追加作業の数量(親チップを含まない数量。即ち、最小値が0)
    var workSeq = changeIntegerFromString($("#HiddenSelectedWorkSeq", top.document).val());     // 0値で親チップ（R）、1以降でworkSeq番目の追加作業となる
    var childNo = changeIntegerFromString($("#HiddenFieldChildNo", top.document).val());        // 子予約連番（TBL_STALLREZINFOのREZCHILDNO）。0:取引、999:納車

    //作業内容タブのR/O追加作業アイコンを生成し、紐付ける.
    appendChildChipIcon(addWorkCount, workSeq, childNo);

    //タップイベントをバインドする.
    $("#divScroll li").bind("touchstart click", function () {
    	var tapIconNumber = $("#divScroll li").index(this);
    	parent.tapRepairOrderIcon(tapIconNumber, workSeq);
    });
}


/**
 * 作業内容タブのR/O追加作業アイコンを生成し、紐付ける.
 * @param {Integer} childChipCount  追加作業総数
 * @param {Integer} workSeq         作業連番
 * @param {Integer} childNo         子予約連番
 * @return {void}
 */
function appendChildChipIcon(childChipCount, workSeq, childNo) {

    //R/O追加作業チップを配置する親要素を取得する.
    var elementParent = $("#divScroll");
   
    var elementUl = $("<ul />");
	
    //追加作業の数量分処理をループさせる.
    for (var i = 0; i <= childChipCount; i++) {
    
        //<li>要素のオブジェクトを作成する.
        var elementList = $("<li />");
        
        //チップに付与するCSSクラス名を定義する.
        //ループインデックスとworkSeqが同値となる場合、CSSの活性クラスを指定する.それ以外は、非活性クラスを指定する.
        var appendCssClass = "S-TC-01PagingOff";
        if (i == workSeq) {
            appendCssClass = "S-TC-01PagingOn";
        }
        else if (i == 0 && childNo == 999) {        // 納車チップの場合、親R/Oを活性化（？）
            appendCssClass = "S-TC-01PagingOn";
        }
        //<div>要素のオブジェクトを作成し、作成したオブジェクトに、CSSのクラスを付与する
        var elementDiv = $("<div />").addClass(appendCssClass);

        if (i == 0) {
            //ループインデックスが0の場合、リペアオーダーのイニシャル文字をDivタグにテキストとして格納する.
            elementDiv.text($("#HiddenFieldRepairOrderInitialWord").val());
        }
        else {
            //追加作業の場合、現在のループインデックスを<span>タグのテキストとし、<div>要素の子要素として追加する.
            var elementSpan = $("<span />");
            elementSpan.text(i.toString());
            elementDiv.append(elementSpan);
        }
        elementList.append(elementDiv);
        //親要素の<ul>タグに、生成された<li>要素を紐付ける.
        elementUl.append(elementList);
    }
    elementParent.append(elementUl);
}


function changeIntegerFromString(stringData) {
    var integerValue;
    try {
        integerValue = Number(stringData);
        if (isNaN(integerValue)) {
            integerValue = 0;
        }
        return integerValue;
    }
    catch (e) {
        integerValue = 0;
        return integerValue;
    }
}


/**
 * 履歴情報をタップした際の処理.
 * @return {void}
 */
function clickHistory(selectedHistory) {
    var orderNumber = $(selectedHistory).children("#HiddenFieldHOrderNo").val()

    if (orderNumber != "") {
        //2013/12/12 TMEJ 成澤 【開発】IT9611_次世代サービス 工程管理機能開発 START

        //2012/11/30 TMEJ 小澤【SERVICE_2】SAストール予約受付機能開発（仕分けNo.75）START
        //parent.tapHistory(orderNumber);
//        var dealerCode = $(selectedHistory).children("#HiddenFieldHDealerCode").val();
//        parent.tapHistory(orderNumber, dealerCode);
        //2012/11/30 TMEJ 小澤【SERVICE_2】SAストール予約受付機能開発（仕分けNo.75）END
        var dealerCode = $(selectedHistory).children("#HiddenFieldHDealerCode").val();
        var serviceInNumder = $(selectedHistory).children("#HiddenFieldServiceInNumber").val()
        parent.tapHistory(orderNumber, dealerCode, serviceInNumder);

        //2013/12/12 TMEJ 成澤 【開発】IT9611_次世代サービス 工程管理機能開発 END
    }
}


/**
 * 基本情報における情報の表示をする
 * @return {void}
 */
function initBasicInfo() {

    //燃料情報の設定
    setBasicFuelInfo();
    //オーディオ情報の設定
    setBasicAudioInfo();
    //エアコン情報の設定
    setBasicAirConditionerInfo();
    //付属品情報の設定
    setBasicAccessoryInfo();

    //交換部品情報の設定
    setOrderExchangePartsInfo();
    //待ち方の設定
    setOrderWaitingInfo();
    //洗車の設定
    setOrderWashingInfo();
    //支払方法の設定
    setOrderPaymentInfo();
    //CSI時間の設定
    setOrderCSIInfo();

    //WNGの設定
    setOrderWNGInfo();
    //故障発生時間の設定
    setOrderOccurrenceInfo();
    //故障発生頻度の設定
    setOrderFrequencyInfo();
    //再現可能
    setOrderReappearInfo();
    //水温
    setOrderWaterTemperatureInfo();
    //気温
    setOrderTemperatureInfo();
    //発生場所
    setOrderPlaceInfo();
    //渋滞状況
    setOrderTrafficjamInfo();
    //車両状態
    setOrderCarStatusInfo();
    //車両状態、走行時
    setOrderTravelingInfo();
    //非純正用品
    setOrderNonGenuineInfo();
}


/**
 * 基本情報・初期状態の燃料情報を表示する.
 * @return {void}
 */
function setBasicFuelInfo() {

    //燃料情報の取得.
    var _fuelValue = $("#HiddenField05_Fuel").val();

    //燃料の1メモリ目のCSSを設定する.
    $("#TC05_Fuel01").toggleClass("S-TC-05Left2-3-1On2", (_fuelValue == C_BASIC_FUEL_QUARTER));
    $("#TC05_Fuel01").toggleClass("S-TC-05Left2-3-1On", ((_fuelValue == C_BASIC_FUEL_HALF) || (_fuelValue == C_BASIC_FUEL_THREE_QUARTER) || (_fuelValue == C_BASIC_FUEL_FULL)));
    $("#TC05_Fuel01").toggleClass("S-TC-05Left2-3-1Off", ((_fuelValue != C_BASIC_FUEL_QUARTER) && (_fuelValue != C_BASIC_FUEL_HALF) && (_fuelValue != C_BASIC_FUEL_THREE_QUARTER) && (_fuelValue != C_BASIC_FUEL_FULL)));

    //燃料の2メモリ目のCSSを設定する.
    $("#TC05_Fuel02").toggleClass("S-TC-05Left2-3-2On2", (_fuelValue == C_BASIC_FUEL_HALF));
    $("#TC05_Fuel02").toggleClass("S-TC-05Left2-3-2On", ((_fuelValue == C_BASIC_FUEL_THREE_QUARTER) || (_fuelValue == C_BASIC_FUEL_FULL)));
    $("#TC05_Fuel02").toggleClass("S-TC-05Left2-3-2Off", ((_fuelValue != C_BASIC_FUEL_HALF) && (_fuelValue != C_BASIC_FUEL_THREE_QUARTER) && (_fuelValue != C_BASIC_FUEL_FULL)));

    //燃料の3メモリ目のCSSを設定する.
    $("#TC05_Fuel03").toggleClass("S-TC-05Left2-3-3On2", (_fuelValue == C_BASIC_FUEL_THREE_QUARTER));
    $("#TC05_Fuel03").toggleClass("S-TC-05Left2-3-3On", (_fuelValue == C_BASIC_FUEL_FULL));
    $("#TC05_Fuel03").toggleClass("S-TC-05Left2-3-3Off", ((_fuelValue != C_BASIC_FUEL_THREE_QUARTER) && (_fuelValue != C_BASIC_FUEL_FULL)));

    //燃料の4メモリ目のCSSを設定する.
    $("#TC05_Fuel04").toggleClass("S-TC-05Left2-3-4On", (_fuelValue == C_BASIC_FUEL_FULL));
    $("#TC05_Fuel04").toggleClass("S-TC-05Left2-3-4Off", (_fuelValue != C_BASIC_FUEL_FULL));
}

/**
 * 基本情報・初期状態のオーディオ情報を表示する
 * @return {void}
 */
function setBasicAudioInfo() {
    
    //オーディオ情報の取得
    var _audio = $("#HiddenField05_Audio").val();

    //オーディオ「オフ」の設定
    $("#TC05_AudioOff").toggleClass("S-TC-05Left2-6-1On", (_audio == C_BASIC_AUDIO_OFF));
    $("#TC05_AudioOff").toggleClass("S-TC-05Left2-6-1Off", (_audio != C_BASIC_AUDIO_OFF));

    //オーディオ「CD」の設定
    $("#TC05_AudioCD").toggleClass("S-TC-05Left2-6-2On", (_audio == C_BASIC_AUDIO_CD));
    $("#TC05_AudioCD").toggleClass("S-TC-05Left2-6-2Off", (_audio != C_BASIC_AUDIO_CD));

    //オーディオ「FM」の設定
    $("#TC05_AudioFM").toggleClass("S-TC-05Left2-6-3On", (_audio == C_BASIC_AUDIO_FM));
    $("#TC05_AudioFM").toggleClass("S-TC-05Left2-6-3Off", (_audio != C_BASIC_AUDIO_FM));
}


/**
* 基本情報・初期状態のエアコン情報を表示する
* @return {void}
*/
function setBasicAirConditionerInfo() {
    
    //エアコン情報の取得
    var _air = $("#HiddenField05_AirConditioner").val();

    //エアコン「オフ」の設定
    $("#TC05_AirConditionerOff").toggleClass("S-TC-05Left2-8-1On", (_air == C_BASIC_AIR_CONDITIONER_OFF));
    $("#TC05_AirConditionerOff").toggleClass("S-TC-05Left2-8-1Off", (_air != C_BASIC_AIR_CONDITIONER_OFF));

    //エアコン「オン」の設定
    $("#TC05_AirConditionerOn").toggleClass("S-TC-05Left2-8-2On", (_air == C_BASIC_AIR_CONDITIONER_ON));
    $("#TC05_AirConditionerOn").toggleClass("S-TC-05Left2-8-2Off", (_air != C_BASIC_AIR_CONDITIONER_ON));
}


/**
* 基本情報・初期状態の付属品情報を表示する
* @return {void}
*/
function setBasicAccessoryInfo() {

    //付属品の数だけループ処理をする    
    for (var i=1; i<=C_BASIC_ACCESSORY_MAX; i++) {
        //付属品情報の取得
        var _accessory = $("#HiddenField05_Accessory" + i.toString()).val();

        //付属品情報の設定
        $("#TC05_Accessory" + i.toString()).toggleClass("S-TC-05Left2-9Checked", (_accessory == C_BASIC_ACCESSORY_CHECKED));
    }
}


/**
* ご用命事項・確認事項の交換部品情報を表示する
* @return {void}
*/
function setOrderExchangePartsInfo() {
    
    //交換部品情報を取得
    var _parts = $("#HiddenField07_ExchangeParts").val();

    //交換部品「持帰り」の設定
    $("#TC07_ExchangeParts1").toggleClass("S-TC-07Right01-1", (_parts == C_ORDER_EXCHANGE_PARTS_TAKEOUT));
    $("#TC07_ExchangeParts1").toggleClass("S-TC-07Right01-1Off", (_parts != C_ORDER_EXCHANGE_PARTS_TAKEOUT));

    //交換部品「保険提出」の設定
    $("#TC07_ExchangeParts2").toggleClass("S-TC-07Right01-2", (_parts == C_ORDER_EXCHANGE_PARTS_INSURANCE));
    $("#TC07_ExchangeParts2").toggleClass("S-TC-07Right01-2Off", (_parts != C_ORDER_EXCHANGE_PARTS_INSURANCE));

    //交換部品「店内処分」の設定
    $("#TC07_ExchangeParts3").toggleClass("S-TC-07Right01-3", (_parts == C_ORDER_EXCHANGE_PARTS_DISPOSE));
    $("#TC07_ExchangeParts3").toggleClass("S-TC-07Right01-3Off", (_parts != C_ORDER_EXCHANGE_PARTS_DISPOSE));
}


/**
* ご用命事項・確認事項の待ち方情報を表示する
* @return {void}
*/
function setOrderWaitingInfo() {
    
    //待ち方情報を取得
    var _waiting = $("#HiddenField07_Waiting").val();

    //待ち方「店内」の設定
    $("#TC07_WaitingIn").toggleClass("S-TC-07Right02-1", (_waiting == C_ORDER_WAITING_IN));
    $("#TC07_WaitingIn").toggleClass("S-TC-07Right02-1Off", (_waiting != C_ORDER_WAITING_IN));

    //持ち方「店外」の設定
    $("#TC07_WaitingOut").toggleClass("S-TC-07Right02-2", (_waiting == C_ORDER_WAITING_OUT));
    $("#TC07_WaitingOut").toggleClass("S-TC-07Right02-2Off", (_waiting != C_ORDER_WAITING_OUT));
}


/**
* ご用命事項・確認事項の洗車情報を表示する
* @return {void}
*/
function setOrderWashingInfo() {
    
    //洗車情報を取得
    var _washing = $("#HiddenField07_Washing").val();

    //洗車「する」の設定
    $("#TC07_WashingDo").toggleClass("S-TC-07Right02-1", (_washing == C_ORDER_WASHING_DO));
    $("#TC07_WashingDo").toggleClass("S-TC-07Right02-1Off", (_washing != C_ORDER_WASHING_DO));

    //洗車「しない」の設定
    $("#TC07_WashingNone").toggleClass("S-TC-07Right02-2", (_washing == C_ORDER_WASHING_NONE));
    $("#TC07_WashingNone").toggleClass("S-TC-07Right02-2Off", (_washing != C_ORDER_WASHING_NONE));
}


/**
* ご用命事項・確認事項の支払方法情報を表示する
* @return {void}
*/
function setOrderPaymentInfo() {
    
    //支払方法情報を取得
    var _payment = $("#HiddenField07_Payment").val();

    //支払方法「現金」の設定
    $("#TC07_PaymentCash").toggleClass("S-TC-07Right01-1", (_payment == C_ORDER_PAYMENT_CASH));
    $("#TC07_PaymentCash").toggleClass("S-TC-07Right01-1Off", (_payment != C_ORDER_PAYMENT_CASH));

    //支払方法「カード」の設定
    $("#TC07_PaymentCard").toggleClass("S-TC-07Right01-2", (_payment == C_ORDER_PAYMENT_CARD));
    $("#TC07_PaymentCard").toggleClass("S-TC-07Right01-2Off", (_payment != C_ORDER_PAYMENT_CARD));

    //支払方法「その他」の設定
    $("#TC07_PaymentOther").toggleClass("S-TC-07Right01-3", (_payment == C_ORDER_PAYMENT_OTHER));
    $("#TC07_PaymentOther").toggleClass("S-TC-07Right01-3Off", (_payment != C_ORDER_PAYMENT_OTHER));
}


/**
* ご用命事項・確認事項のCSI時間情報を表示する
* @return {void}
*/
function setOrderCSIInfo() {

    //CSI時間情報の取得
    var _csi = $("#HiddenField07_Csi").val();

    //CSI時間「午前」の設定
    $("#TC07_CSI_AM").toggleClass("S-TC-07Right01-1", (_csi == C_ORDER_CSI_AM));
    $("#TC07_CSI_AM").toggleClass("S-TC-07Right01-1Off", (_csi != C_ORDER_CSI_AM));

    //CSI時間「午後」の設定
    $("#TC07_CSI_PM").toggleClass("S-TC-07Right01-2", (_csi == C_ORDER_CSI_PM));
    $("#TC07_CSI_PM").toggleClass("S-TC-07Right01-2Off", (_csi != C_ORDER_CSI_PM));

    //CSI時間「指定なし」の設定
    $("#TC07_CSI_Always").toggleClass("S-TC-07Right01-3", (_csi == C_ORDER_CSI_ALWAYS));
    $("#TC07_CSI_Always").toggleClass("S-TC-07Right01-3Off", (_csi != C_ORDER_CSI_ALWAYS));
}


/**
* ご用命事項・問診項目のWNG情報を表示する.
* @return {void}
*/
function setOrderWNGInfo() {

    //WNG情報の取得.
    var _wng = $("#HiddenField07_Warning").val();

    //WNG「常時点灯」の設定.
    $("#TC07_WNG_Always").toggleClass("S-TC-07Right01-1", (_wng == C_ORDER_WNG_ALWAYS));
    $("#TC07_WNG_Always").toggleClass("S-TC-07Right01-1Off", (_wng != C_ORDER_WNG_ALWAYS));

    //WNG「頻繁に点灯」の設定.
    $("#TC07_WNG_Often").toggleClass("S-TC-07Right01-2", (_wng == C_ORDER_WNG_OFTEN));
    $("#TC07_WNG_Often").toggleClass("S-TC-07Right01-2Off", (_wng != C_ORDER_WNG_OFTEN));

    //WNG「表示なし」の設定.
    $("#TC07_WNG_None").toggleClass("S-TC-07Right01-3", (_wng == C_ORDER_WNG_NONE));
    $("#TC07_WNG_None").toggleClass("S-TC-07Right01-3Off", (_wng != C_ORDER_WNG_NONE));
}


/**
* ご用命事項・問診項目の故障発生時間情報を表示する.
* @return {void}
*/
function setOrderOccurrenceInfo() {

    //故障発生時間の取得.
    var _occurrence = $("#HiddenField07_Occurrence").val();

    //故障発生時間「最近」の設定.
    $("#TC07_Occurrence_Recently").toggleClass("S-TC-07Right01-1", (_occurrence == C_ORDER_OCCURRENCE_RECENTLY));
    $("#TC07_Occurrence_Recently").toggleClass("S-TC-07Right01-1Off", (_occurrence != C_ORDER_OCCURRENCE_RECENTLY));

    //故障発生時間「一週間前」の設定.
    $("#TC07_Occurrence_Week").toggleClass("S-TC-07Right01-2", (_occurrence == C_ORDER_OCCURRENCE_WEEK));
    $("#TC07_Occurrence_Week").toggleClass("S-TC-07Right01-2Off", (_occurrence != C_ORDER_OCCURRENCE_WEEK));

    //故障発生時間「その他」の設定.
    $("#TC07_Occurrence_Other").toggleClass("S-TC-07Right01-3", (_occurrence == C_ORDER_OCCURRENCE_OTHER));
    $("#TC07_Occurrence_Other").toggleClass("S-TC-07Right01-3Off", (_occurrence != C_ORDER_OCCURRENCE_OTHER));
}


/**
* ご用命事項・問診項目の故障発生頻度情報を表示する.
* @return {void}
*/
function setOrderFrequencyInfo() {

    //故障発生頻度の取得.
    var _frequency = $("#HiddenField07_Frequency").val();

    //故障発生頻度「頻繁に」の設定.
    $("#TC07_Frequency_High").toggleClass("S-TC-07Right01-1", (_frequency == C_ORDER_FREQUENCY_HIGH));
    $("#TC07_Frequency_High").toggleClass("S-TC-07Right01-1Off", (_frequency != C_ORDER_FREQUENCY_HIGH));

    //故障発生頻度「時々」の設定.
    $("#TC07_Frequency_Often").toggleClass("S-TC-07Right01-2", (_frequency == C_ORDER_FREQUENCY_OFTEN));
    $("#TC07_Frequency_Often").toggleClass("S-TC-07Right01-2Off", (_frequency != C_ORDER_FREQUENCY_OFTEN));

    //故障発生頻度「一回だけ」の設定
    $("#TC07_Frequency_Once").toggleClass("S-TC-07Right01-3", (_frequency == C_ORDER_FREQUENCY_ONCE));
    $("#TC07_Frequency_Once").toggleClass("S-TC-07Right01-3Off", (_frequency != C_ORDER_FREQUENCY_ONCE));
}


/**
* ご用命事項・問診項目の再現可能情報を表示する.
* @return {void}
*/
function setOrderReappearInfo() {

    //再現可能情報の取得.
    var _reappear = $("#HiddenField07_Reappear").val();

    //再現可能「はい」の設定.
    $("#TC07_Reappear_Yes").toggleClass("S-TC-07Right02-1", (_reappear == C_ORDER_REAPPEAR_YES));
    $("#TC07_Reappear_Yes").toggleClass("S-TC-07Right02-1Off", (_reappear != C_ORDER_REAPPEAR_YES));

    //再現可能「いいえ」の設定
    $("#TC07_Reappear_No").toggleClass("S-TC-07Right02-2", (_reappear == C_ORDER_REAPPEAR_NO));
    $("#TC07_Reappear_No").toggleClass("S-TC-07Right02-2Off", (_reappear != C_ORDER_REAPPEAR_NO));
}


/**
* ご用命事項・問診項目の水温情報を表示する.
* @return {void}
*/
function setOrderWaterTemperatureInfo() {

    //水温情報の取得.
    var _water = $("#HiddenField07_WaterT").val();

    //水温「冷」の設定.
    $("#TC07_WaterT_Low").toggleClass("S-TC-07Right03-1", (_water == C_ORDER_WATERT_LOW));
    $("#TC07_WaterT_Low").toggleClass("S-TC-07Right03-1Off", (_water != C_ORDER_WATERT_LOW));

    //水温「熱」の設定.
    $("#TC07_WaterT_High").toggleClass("S-TC-07Right03-2", (_water == C_ORDER_WATERT_HIGH));
    $("#TC07_WaterT_High").toggleClass("S-TC-07Right03-2Off", (_water != C_ORDER_WATERT_HIGH));
}


/**
* ご用命事項・問診項目の気温情報を表示する.
* @return {void}
*/
function setOrderTemperatureInfo() {

    //気温情報の取得.
    var _temperature = $("#HiddenField07_Temperature").val();

    //気温「寒」の設定.
    $("#TC07_Temperature_Low").toggleClass("S-TC-07Right03-1", (_temperature == C_ORDER_TEMPERATURE_LOW));
    $("#TC07_Temperature_Low").toggleClass("S-TC-07Right03-1Off", (_temperature != C_ORDER_TEMPERATURE_LOW));

    //気温「暑」の設定.
    $("#TC07_Temperature_High").toggleClass("S-TC-07Right03-2", (_temperature == C_ORDER_TEMPERATURE_HIGH));
    $("#TC07_Temperature_High").toggleClass("S-TC-07Right03-2Off", (_temperature != C_ORDER_TEMPERATURE_HIGH));
}


/**
* ご用命事項・問診項目の発生場所情報を表示する.
* @return {void}
*/
function setOrderPlaceInfo() {

    //発生場所情報の取得.
    var _place = $("#HiddenField07_Place").val();

    //発生場所「駐車場」の設定.
    $("#TC07_Place_Parking").toggleClass("S-TC-07Right04-1", (_place == C_ORDER_PLACE_PARKING));
    $("#TC07_Place_Parking").toggleClass("S-TC-07Right04-1Off", (_place != C_ORDER_PLACE_PARKING));

    //発生場所「一般道路」の設定.
    $("#TC07_Place_Ordinary").toggleClass("S-TC-07Right04-2", (_place == C_ORDER_PLACE_ORDINARY));
    $("#TC07_Place_Ordinary").toggleClass("S-TC-07Right04-2Off", (_place != C_ORDER_PLACE_ORDINARY));

    //発生場所「高速道路」の設定.
    $("#TC07_Place_Motorway").toggleClass("S-TC-07Right04-3", (_place == C_ORDER_PLACE_MOTORWAY));
    $("#TC07_Place_Motorway").toggleClass("S-TC-07Right04-3Off", (_place != C_ORDER_PLACE_MOTORWAY));

    //発生場所「坂道」の設定.
    $("#TC07_Place_Slope").toggleClass("S-TC-07Right04-4", (_place == C_ORDER_PLACE_SLOPE));
    $("#TC07_Place_Slope").toggleClass("S-TC-07Right04-4Off", (_place != C_ORDER_PLACE_SLOPE));
}


/**
* ご用命事項・問診項目の渋滞状況情報を表示する.
* @return {void}
*/
function setOrderTrafficjamInfo() {

    //渋滞状況の取得.
    var _traffic = $("#HiddenField07_TrafficJam").val();

    //渋滞状況「あり」の設定.
    $("#TC07_Trafficjam_Happen").toggleClass("S-TC-07Right02-1", (_traffic == C_ORDER_TRAFFICJAM_HAPPEN));
    $("#TC07_Trafficjam_Happen").toggleClass("S-TC-07Right02-1Off", (_traffic != C_ORDER_TRAFFICJAM_HAPPEN));

    //渋滞状況「なし」の設定.
    $("#TC07_Trafficjam_None").toggleClass("S-TC-07Right02-2", (_traffic == C_ORDER_TRAFFICJAM_NONE));
    $("#TC07_Trafficjam_None").toggleClass("S-TC-07Right02-2Off", (_traffic != C_ORDER_TRAFFICJAM_NONE));
}


/**
* ご用命事項・問診項目の車両状態情報を表示する.
* @return {void}
*/
function setOrderCarStatusInfo() {

    //車両状態の取得.
    var _status = $("#HiddenField07_CarStatus").val();

    //車両状態「起動時」の設定.
    setOrderCarStatus("HiddenField07_CarStatus_Startup", "TC07_CarStatus_Startup", "S-TC-07Right04-1", "S-TC-07Right04-1Off");

    //車両状態「アイドル時」の設定.
    setOrderCarStatus("HiddenField07_CarStatus_Idling", "TC07_CarStatus_Idlling", "S-TC-07Right04-2", "S-TC-07Right04-2Off");
    
    //車両状態「冷間時」の設定.
    setOrderCarStatus("HiddenField07_CarStatus_Cold", "TC07_CarStatus_Cold", "S-TC-07Right04-3", "S-TC-07Right04-3Off");

    //車両状態「温間時」の設定.
    setOrderCarStatus("HiddenField07_CarStatus_Warm", "TC07_CarStatus_Warm", "S-TC-07Right04-4", "S-TC-07Right04-4Off");

    //「駐車時」の設定.
    setOrderCarStatus("HiddenField07_CarStatus_Parking", "TC07_CarControl1_Parking", "S-TC-07Right04-1", "S-TC-07Right04-1Off");

    //「前進時」の設定.
    setOrderCarStatus("HiddenField07_CarStatus_Advance", "TC07_CarControl1_Advance", "S-TC-07Right04-2", "S-TC-07Right04-2Off");

    //「変速時」の設定.
    setOrderCarStatus("HiddenField07_CarStatus_ShiftChange", "TC07_CarControl1_ShiftChange", "S-TC-07Right04-3", "S-TC-07Right04-3Off");

    //「後退時」の設定.
    setOrderCarStatus("HiddenField07_CarStatus_Back", "TC07_CarControl1_Back", "S-TC-07Right04-4", "S-TC-07Right04-4Off");

    //「ブレーキ時」の設定.
    setOrderCarStatus("HiddenField07_CarStatus_Brake", "TC07_CarControl2_Brake", "S-TC-07Right01-1", "S-TC-07Right01-1Off");

    //「迂回時」の設定.
    setOrderCarStatus("HiddenField07_CarStatus_Detour", "TC07_CarControl2_Detour", "S-TC-07Right01-2", "S-TC-07Right01-2Off");

    // 「旋回時」の設定.
    setOrderCarStatus("HiddenField07_CarStatus_SteeringWheel", "TC07_CarControl2_SteeringWheel", "S-TC-07Right01-3", "S-TC-07Right01-3Off");
}


/**
* ご用命事項・問診項目の車両状態を表示する.
* @param {String} aHiddenFieldId
* @param {String} aTargetId
* @param {String} aCssClassNameOn
* @param {String} aCssClassNameOff
* @return {void}
*/
function setOrderCarStatus(aHiddenFieldId, aTargetId, aCssClassNameOn, aCssClassNameOff) {

    //車両状態の情報取得.
    var _idlling = $("#" + aHiddenFieldId).val();

    //車両状態の情報を表示する.
    $("#" + aTargetId).toggleClass(aCssClassNameOn, (_idlling == C_ORDER_CARSTATUS_ON));
    $("#" + aTargetId).toggleClass(aCssClassNameOff, (_idlling != C_ORDER_CARSTATUS_ON));
}


/**
* ご用命事項・問診項目の走行時情報を表示する.
* @return {void}
*/
function setOrderTravelingInfo() {

    //走行時の情報取得.
    var _traveling = $("#HiddenField07_Traveling").val();

    //走行時「穏速」の設定.
    $("#TC07_Traveling_Lowspeed").toggleClass("S-TC-07RightListChecked", (_traveling == C_ORDER_TRAVELING_LOWSPEED));

    //走行時「加速」の設定.
    $("#TC07_Traveling_Acceleration").toggleClass("S-TC-07RightListChecked", (_traveling == C_ORDER_TRAVELING_ACCELERATION));

    //走行時「減速」の設定.
    $("#TC07_Traveling_Slowdown").toggleClass("S-TC-07RightListChecked", (_traveling == C_ORDER_TRAVELING_SLOWDOWN));
}


/**
* ご用命事項・問診項目の非純正用品を表示する.
* @return {void}
*/
function setOrderNonGenuineInfo() {

    //非純正用品情報の取得.
    var _genuine = $("#HiddenField07_NonGenuine").val();

    //非純正用品「あり」の設定.
    $("#TC07_NonGenuine_Yes").toggleClass("S-TC-07Right02-1", (_genuine == C_ORDER_NONGENUINE_YES));
    $("#TC07_NonGenuine_Yes").toggleClass("S-TC-07Right02-1Off", (_genuine != C_ORDER_NONGENUINE_YES));

    //非純正用品「なし」の設定.
    $("#TC07_NonGenuine_No").toggleClass("S-TC-07Right02-2", (_genuine == C_ORDER_NONGENUINE_NO));
    $("#TC07_NonGenuine_No").toggleClass("S-TC-07Right02-2Off", (_genuine != C_ORDER_NONGENUINE_NO));
}

// 2012/11/30 TMEJ 小澤【SERVICE_2】SAストール予約受付機能開発（仕分けNo.75）START
/**
* 読み込み中の画像と操作不可用のDIVを設定.
* @return {void}
*/
function clickAllLink() {
    $('div.S-TC-05Right2-1').hide();
    $('div.S-TC-05Right-NextLoding').show();
    $('div.DisabledDiv').show();
}
function clickNextLink() {
    $('div.S-TC-05Right2-1').hide();
    $('div.S-TC-05Right2-2').hide();
    $('div.S-TC-05Right-NextLoding').show();
    $('div.DisabledDiv').show();
}
// 2012/11/30 TMEJ 小澤【SERVICE_2】SAストール予約受付機能開発（仕分けNo.75）END

//2013/12/10　TMEJ　成澤【開発】IT9611_次世代サービス 工程管理機能開発 START
/**
* 画面連携のURLを設定する.
* @return {void}
*/
function setIframeUrl() {
    document.getElementById("CST_DETAIL_IFRAME").src = $("#HiddenFieldCutDtlIframeUrl").val();
    document.getElementById("CST_REQUEST_IFRAME").src = $("#HiddenFieldCutReqIframeUrl").val();
}
//2013/12/10　TMEJ　成澤【開発】IT9611_次世代サービス 工程管理機能開発 END


//2014/07/23 TMEJ 成澤　【開発】IT9711_タブレットSMB Job Dispatch機能開発 START

/**
* 整備項目の選択行の値の格納
* @return {void}
*/
function selectedJobItem(selectedItem) {

    //親要素を取得
    var parentId = $(selectedItem).parent().parent();

    //選択した行の値を格納
    $("#HiddenSelectedJobInstructId").val(parentId.children("#HiddenJobInstructId").val());
    $("#HiddenSelectedJobInstructSeq").val(parentId.children("#HiddenJobInstructSeq").val());


}

/**
* TC画面のリフレッシュ関数を呼び出す
* @return {void}
*/
function parentScreenReLoad() {
    parent.reloadPage();

}

/**
* 中断用の非表示ボタンクリック
* @return {void}
*/
function JobStopBattonClick(lastJobFlg) {

    //最後の整備の場合
    if (lastJobFlg == 1) {
        //休憩をとる・とらないを問うpopupを表示する処理.
        if ($("#HiddenBreakPopupChild").val() == C_BREAK_POPUP_DISPLAY) {
            //フラグを初期化する.
            $("#HiddenBreakPopupChild").val(C_BREAK_POPUP_NONE);
            parent.selectClass(CAll_BY_SC3150102);
        }
    }

    //2016/03/30 NSK 小牟禮 アクティビティインジケータが消えない問題 START
    //$("#HiddenButtonJobStop").click();

    //クルクル表示
    parent.LoadingScreen();

    setTimeout(function () {
        $("#HiddenButtonJobStop").click();
    }, 0);
    //2016/03/30 NSK 小牟禮 アクティビティインジケータが消えない問題 END
}

/**
* 休憩取得用の非表示ボタンクリック
* @return {void}
*/
function BreakBattonClick(clickFlg) {

    //2016/03/30 NSK 小牟禮 アクティビティインジケータが消えない問題 START
    ////休憩取得をタップした場合
    //if (clickFlg) {
    //    $("#HiddenButtonChildTakeBreak").click();
    //    //休憩取得しないをタップした場合
    //} else {
    //    $("#HiddenButtonChildDoNotBreak").click();
    //}

    //クルクル表示
    parent.LoadingScreen();

    //休憩取得をタップした場合
    if (clickFlg) {
        
        setTimeout(function () {
            $("#HiddenButtonChildTakeBreak").click();
        }, 0);

        //休憩取得しないをタップした場合
    } else {
        
        setTimeout(function () {
            $("#HiddenButtonChildDoNotBreak").click();
        }, 0);
    }
    //2016/03/30 NSK 小牟禮 アクティビティインジケータが消えない問題 END

    //ポップアップを閉じる
    parent.BreakPopupClose(true);
}

/**
* Jobのステータスに合わせてActionBattonのスタイル変更
* @return {void}
*/
function JobActionBattonConvert(i) {

    //実績がある場合開始ボタンを非活性
    if (($("#HiddenRsltSTratDatetime" + i).val() != "") || ($("#HiddenJobStatus" + i).val() != "")) {
        $("#Textbox01_" + i).children("#JobStartButton").removeClass().addClass("BtnOff");
    }
    //作業中以外の場合終了ボタンを非活性
    if ($("#HiddenJobStatus" + i).val() != JOB_STATUS_WORKIG) {
        $("#Textbox02_" + i).children("#JobFinishButton").removeClass().addClass("BtnOff");
    }
    //作業中以外の場合中断ボタンを非活性
    if ($("#HiddenJobStatus" + i).val() != JOB_STATUS_WORKIG) {
        $("#Textbox03_" + i).children("#JobStopButton").removeClass().addClass("BtnOff");
    }
    //中断の場合再開ボタンを表示、中断ボタンを非表示
    if ($("#HiddenJobStatus" + i).val() == JOB_STATUS_STOP) {
        $("#Textbox03_" + i).children("#JobStopButton").css("display", "none");
        $("#Textbox03_" + i).children("#JobReStartButton").css("display", "block");
        if ($("#HiddenStallUseStatus").val() == STALL_USE_STATUS_STOP) {
            $("#Textbox03_" + i).children("#JobReStartButton").removeClass().addClass("BtnOff");
        }
    } else {
        $("#Textbox03_" + i).children("#JobStopButton").css("display", "block");
        $("#Textbox03_" + i).children("#JobReStartButton").css("display", "none");
    }

    //2014/12/05 TMEJ 岡田　IT9857_DMS連携版サービスタブレット JobDispatch完成検査入力制御開発 STRAT
    //紐付いてないJOBをカウント（フィルターの数をカウント）
    if ($("#Textbox01_" + i).parent('div').parent('td').parent('tr').hasClass('S-TC-01LeftGrayZone') == false
        && $("#Textbox01_" + i).parent('div').parent('td').parent('tr').hasClass('S-TC-01LeftGrayZone2') == false) {

        //完了している整備の数をカウント
        if ($("#HiddenJobStatus" + i).val() == JOB_STATUS_COMPLETE) {
            FinishJobCount += 1;
        }
        //中断している整備の数をカウント
        if ($("#HiddenJobStatus" + i).val() == JOB_STATUS_STOP) {
            StopJobCount += 1;
        }

        //全ての整備の数をカウント
        AllJobCount += 1;

    }
//2014/12/05 TMEJ 岡田　IT9857_DMS連携版サービスタブレット JobDispatch完成検査入力制御開発 End

}
/**
* 整備一覧の行ごとにID名を変更
* @return {void}
*/
function JobActionClassConvert(i) {

    $("#Textbox01").attr('id', "Textbox01_" + i);

    $("#Textbox02").attr('id', "Textbox02_" + i);

    $("#Textbox03").attr('id', "Textbox03_" + i);

    $("#HiddenRsltSTratDatetime").attr('id', "HiddenRsltSTratDatetime" + i);
    $("#HiddenJobStatus").attr('id', "HiddenJobStatus" + i);

}

/**
* HTMLのdecode
* @param {String} decode前の値
* @return {String} decode後の値
*/
function htmlDecode(text) {

    try {

        return text.replace(/&amp;/g, '&').replace(/&quot;/g, '"').replace(/&lt;/g, '<').replace(/&gt;/g, '>');
    }
    catch (e) {
        return "";
    }
}

/**
* 残り整備の数をカウント
* @param {String} decode前の値
* @return {String} decode後の値
*/
function countRestJob() {

    RestJobCount = AllJobCount - (StopJobCount + FinishJobCount);
    parent.SetRestJobFlg(RestJobCount);
}


//2014/07/23 TMEJ 成澤　【開発】IT9711_タブレットSMB Job Dispatch機能開発 END

//2014/08/29 TMEJ 成澤 【開発】IT9737_NextSTEPサービス ロケ管理の効率化に向けた評価用アプリ作成　START
/**
* かご番号の横幅変更
* @return {void}
*/
function SetCagoNumberWidth(itemCount) {

    //初期幅取得
    var widthValue = $("#lblCageNo").width();

    //アイテム数が7以上は長さ固定
    if (itemCount > 7) {
        $("#lblCageNo").width(208);
    } else {
        //アイテム数に合わせて長さ変更
        $("#lblCageNo").width(widthValue + (itemCount * 28));

    }

}
//2014/08/29 TMEJ 成澤 【開発】IT9737_NextSTEPサービス ロケ管理の効率化に向けた評価用アプリ作成　END

//2014/09/12 TMEJ 成澤  自主研追加対応_ROプレビュー遷移　START
/**
* ROアイコンをタップした際の処理.
* @return {void}
*/
function clickRepairOrderIco(selectedHistory) {
    
    //選択した行のRO番号格納
    var orderNumber = $(selectedHistory).parent().children("#HiddenFieldHOrderNo").val()

    if (orderNumber != "") {
        //選択した行のRO枝番格納
        var orderNumberSeq = $(selectedHistory).parent().children("#HiddenFieldHOrderNoSeq").val();
        //親画面の関数呼び出す
        parent.newTapRepairOrderIcon(orderNumber, orderNumberSeq);

    }
}
//2014/09/12 TMEJ 成澤  自主研追加対応_ROプレビュー遷移　END

//2014/12/05 TMEJ 岡田　IT9857_DMS連携版サービスタブレット JobDispatch完成検査入力制御開発 STRAT
/**
* JobFinishButton表示制御
* @return {void}
*/
function JobFinishBtnDispCtrl() {

    //残りの開始しているJOBが1つ以外なら処理終了
    if ((RestJobCount) != 1) {
        return;
    }

    //作業停止しているJOBが0以外なら処理終了
    if ((StopJobCount) >= 1) {
        return;
    }

    //JOBの数分ループ
    for (var i = 0; i <= AllJobCount; i++) {
        //作業開始のJOBがある場合
        if ($("#HiddenJobStatus" + String(i)).val() == 0) {
            //フィルターがかかってない場合のみ、処理する
            if ($("#Textbox01_" + i).parent('div').parent('td').parent('tr').hasClass('S-TC-01LeftGrayZone') == false
            && $("#Textbox01_" + i).parent('div').parent('td').parent('tr').hasClass('S-TC-01LeftGrayZone2') == false) {
                //FinishButtonを非活性にする
                $("#Textbox02_" + i).children("#JobFinishButton").removeClass().addClass("BtnOff");
                $("#Textbox02_" + i).children("#JobFinishButton").unbind('click');
                //処理終了
                break;
            }
        }
    }

}
//2014/12/05 TMEJ 岡田　IT9857_DMS連携版サービスタブレット JobDispatch完成検査入力制御開発 END