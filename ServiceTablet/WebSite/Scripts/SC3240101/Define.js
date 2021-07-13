//---------------------------------------------------------
//Define.js
//---------------------------------------------------------
//機能：グローバル変数とマクロの定義
//作成：2012/12/22 TMEJ 張 タブレット版SMB機能開発(工程管理)
//更新：2013/11/29 TMEJ 丁 タブレット版SMB チーフテクニシャン機能開発
//更新：2013/11/29 TMEJ 張 タブレット版SMB チーフテクニシャン機能開発
//更新：2014/06/18 TMEJ 丁 タブレット版SMB テレマ走行距離機能開発
//更新：2014/07/15 TMEJ 張 タブレットSMB Job Dispatch機能開発
//更新：2014/07/15 TMEJ 丁 タブレットSMB Job Dispatch機能開発
//更新：2015/02/13 TMEJ 範 新プラットフォーム版i-CROP仕様変更対応 (SMBチップ検索及び作業中断機能強化)
//更新：2015/07/30 TMEJ 明瀬 アクティビティインジケータが消えない問題
//更新：2017/08/30 NSK 竹中(悠) REQ-SVT-TMT-20161109-001 SMB iPadにストールクローズ機能の追加
//更新：2017/09/05 NSK 小川 REQ-SVT-TMT-20160906-003 計画済みチップをストールから簡単に外せるようにする
//更新：2017/09/25 NSK 竹中(悠) REQ-SVT-TMT-20170227-002 見込納車遅れ日時計算ロジック統一
//更新：2018/07/06 NSK 可児 TKM Next Gen e-CRB Project Application development Block B-1  Pマーク、Lマークなどを表示
//更新：2019/06/18 NSK 皆川 (トライ店システム評価)次世代サービスオペレーション効率化に向けた、業務適合性検証
//更新：2019/12/06 NSK 皆川 (トライ店システム評価)整備作業における休憩時間取得判定向上検証
//---------------------------------------------------------
/******************************************************
* 定数
*******************************************************/

//中断理由区分
//部品欠品
var C_STOPREASON_PARTSSTOCKOUT = "01";
//お客様連絡待ち
var C_STOPREASON_GUSTOMERREPORTWAITING = "02";
//検査不合格
var C_STOPREASON_INSPECTIONFAILURE = "03";
//その他
var C_STOPREASON_OTHERS = "99";

//日付の最小値
var C_DATE_MIN_VALUE = Date.parse("1900/01/01 0:00:00");
//日付省略値
var C_DATE_DEFAULT_VALUE = Date.parse("1900/01/01 0:00:00");
//数値省略値
var C_NUM_DEFAULT_VALUE = 0;
//文字列省略値
var C_STR_DEFAULT_VALUE = "";
//サービスステータス
// 未入庫
var C_SVCSTATUS_NOTCARIN  = "00";
// 未来店客（予定通りの入庫がないため一旦チップをNoShowエリアに移動している）
var C_SVCSTATUS_NOSHOW  = "01";
// キャンセル
var C_SVCSTATUS_CANCEL  = "02";
// 着工指示待ち
var C_SVCSTATUS_WORKORDERWAIT  = "03";
// 作業開始待ち
var C_SVCSTATUS_STARTWAIT  = "04";
// 作業中
var C_SVCSTATUS_START  = "05";
// 次の作業開始待ち
var C_SVCSTATUS_NEXTSTARTWAIT = "06";
// 洗車待ち
var C_SVCSTATUS_CARWASHWAIT = "07";
//洗車中
var C_SVCSTATUS_CARWASHSTART = "08";
//検査待ち
var C_SVCSTATUS_INSPECTIONWAIT = "09";
//検査中
var C_SVCSTATUS_INSPECTIONSTART = "10";
//預かり中（DropOff）
var C_SVCSTATUS_DROPOFFCUSTOMER = "11";
//納車待ち（Waiting）
var C_SVCSTATUS_WAITINGCUSTOMER = "12";
//納車済み
var C_SVCSTATUS_DELIVERY = "13";

//ストール利用ステータス
//着工指示待ち
var C_STALLUSE_STATUS_WORKORDERWAIT = "00";
//作業開始待ち
var C_STALLUSE_STATUS_STARTWAIT = "01";
//作業中
var C_STALLUSE_STATUS_START = "02";
//完了
var C_STALLUSE_STATUS_FINISH = "03";
//作業計画の一部の作業が中断
var C_STALLUSE_STATUS_STARTINCLUDESTOPJOB = "04";
//中断
var C_STALLUSE_STATUS_STOP = "05";
//日跨ぎ終了
var C_STALLUSE_STATUS_MIDFINISH = "06";
//未来店客（予定通りの入庫がないため一旦チップをNoShowエリアに移動している）
var C_STALLUSE_STATUS_NOSHOW = "07";

//2017/07/12 NSK  河谷 REQ-SVT-TMT-20160906-001 チップ検索の検索範囲をPC-SMBと同じにする START
//ROステータス(20:FM承認待ち)
var C_RO_STATUS_WAITING_FM_APPROVAL = 20;
//ROステータス(50:着工指示待ち)
var C_RO_STATUS_STARTWAIT = 50;
//ROステータス(60:作業中)
var C_RO_STATUS_WORKING = 60;

//仮置きフラグ(0:仮置きでない)
var C_TEMP_FLAG_OFF = 0;
//仮置きフラグ(1:仮置き)
var C_TEMP_FLAG_ON = 1;
//2017/07/12 NSK  河谷 REQ-SVT-TMT-20160906-001 チップ検索の検索範囲をPC-SMBと同じにする END

//休憩取得フラグ(選択してない)
var C_RESTTIMEGETFLG_NOSET = -1;
//休憩取得フラグ(取得しない)
var C_RESTTIMEGETFLG_NOGETREST = 0;
//休憩取得フラグ(取得する)
var C_RESTTIMEGETFLG_GETREST = 1;

//中断理由
//部品欠品
var C_STOPREASONTYPE_STOCKOUT = "01"
//顧客承認待ち
var C_STOPREASONTYPE_WAITCONFIRMED = "02"
//その他
var C_STOPREASONTYPE_OTHER = "99"

// 引取納車区分:Waiting
var C_DELITYPE_WAITING  = "0";

// 引取納車区分:Drop off
var C_DELITYPE_DROPOFF  = "4";

// 検査承認待ち
var C_INSPECTION_APPROVAL = "1";


//2017/09/25 NSK 竹中(悠) REQ-SVT-TMT-20170227-002 見込納車遅れ日時計算ロジック統一 START
// 残完成検査区分
// 完成検査入力未完了
var C_NOTFINISH_FINAL_INSPECTION = "0";

// 完成検査承認待ち
var C_WAITHING_FINAL_INPSECTION = "1";

// その他（完成検査承認完了または不要)
var C_FINISH_FINAL_INSPECTION = "2";

//2017/09/25 NSK 竹中(悠) REQ-SVT-TMT-20170227-002 見込納車遅れ日時計算ロジック統一 END

// 1つセルの幅
var C_CELL_WIDTH = 42;
// 1つセルの高さ
var C_CELL_HEIGHT = 73;
// サブエリアを開く時、メインストールスクロールを拡大範囲
var C_CHIPAREA_OFFSET_HEIGHT = 175;

// interval時間
var C_INTERVAL_TIME = 60000;
// 2013/11/29 TMEJ 張 タブレット版SMB チーフテクニシャン機能開発 START
// 新規チップのID
var C_NEWCHIPID = "NewChip";
// 2013/11/29 TMEJ 張 タブレット版SMB チーフテクニシャン機能開発 END
// 選択したチップ移動する時、表示されるチップ
var C_MOVINGCHIPID = "MovingChip";
// 別の日からチップ
var C_OTHERDTCHIPID = "OtherDtChip";
// サブエリアからメイン画面に置く時、一時用チップID
var C_TEMPCHIPID = "TempChipId";
// コピーされたチップID(コピー操作中)
var C_COPYCHIPID = "-1";
// コピーされたチップID array(画面上でコピー操作がもう終わった、サーバに送信してる)
var C_ARR_COPYEDCHIPID = new Array(-10, -11, -12, -13, -14, -15, -16, -17, -18, -19);
// 使用不可エリアチップ
var C_UNAVALIABLECHIPID = "UnavaliableChip";
// 使用不可エリアチップ(新規用)
var C_UNAVALIABLENEWCHIPID = "UnavaliableChipNew";
// 仮仮チップID
var C_KARIKARICHIPID = "KariKariChip";
// 休憩エリアチップ
var C_RESTCHIPID = "RestChip";
// 使用不可エリアの移動チップ
var C_MOVINGUNAVALIABLECHIPID = "MovingUnavaliableChip";
// 新規チップの幅(セル個数)
var C_NEWCHIPID_COLUM_NUM = 3;
// タッチ開始
var C_TOUCH_START = "touchstart mousedown";
// タッチで移動
var C_TOUCH_MOVE = "touchmove mousemove";
// タッチ終わり
var C_TOUCH_END = "touchend mouseup";

// 2013/11/29 TMEJ 張 タブレット版SMB チーフテクニシャン機能開発 START
var C_APPLICATION_BUTTON = "MstPG_FootItem_Main_"

// CTの場合
var C_OPECODE_CT = 55;
// FMの場合
var C_OPECODE_FM = 58;
// SAの場合
var C_OPECODE_SA = 9;
// SMの場合
var C_OPECODE_SM = 10;
// CHTの場合
var C_OPECODE_CHT = 62;
// 2013/11/29 TMEJ 張 タブレット版SMB チーフテクニシャン機能開発 END

// オフセットトップ座標
var C_CHIPAREA_OFFSET_TOP = 33;
// オフセット左の座標
var C_CHIPAREA_OFFSET_LEFT = 143;
// スクリーンにより、オフセット左の座標
var C_SCREEN_CHIPAREA_OFFSET_LEFT = 163;
// エラー情報
var C_NO_ERROR = 0;
// 非透明
var C_OPACITY = 1;
// 半透明
var C_OPACITY_TRANSPARENT = 0.8;

// ポップアップボックスに最大チップ数
var C_POPUP_MAX_CHIP_NUM = 8;

var C_MAXSTOPTIME = 9995;

// 明日以降
var C_DATE_AFTER_TOMMORROW = -1;
// 昨日以前
var C_DATE_BEFORE_YESTDAY = -2;
// 今日未だ仕事開始してない
var C_DATE_TODAY_BEFORESTART = -3;
// 今日仕事が終わる
var C_DATE_TODAY_AFTEREND = -4;

var C_DISP_NONE = 0;                    // 表示されているポップアップウィンドウがない
var C_DISP_NEW = 1;                     // 新規ウィンドウが表示中
var C_DISP_DETAIL = 2;                  // 詳細ウィンドウが表示中
var C_DISP_STOP = 3;                    // 新規ウィンドウが表示中
var C_DISP_REST = 4;                    // 休憩ウィンドウが表示中
var C_DISP_DUPL = 5;                    // 重複ウィンドウが表示中
// 2013/11/29 TMEJ 張 タブレット版SMB チーフテクニシャン機能開発 START
var C_DISP_TECH = 6;                    // テクニシャンウィンドウが表示中

//2017/09/01 NSK 竹中(悠) REQ-SVT-TMT-20161109-001 SMB iPadにストールクローズ機能の追加
var C_DISP_UNAVAILABLE = 7; // ストール使用不可画面が表示中
//2017/09/01 NSK 竹中(悠) REQ-SVT-TMT-20161109-001 SMB iPadにストールクローズ機能の追加

var C_CALLBACK_REFRESH = 1;             // CALLBACKでエラーを出して、画面をリフレッシュ   
var C_CALLBACK_JUST_ALERT = 2;          // CALLBACKでエラーを出して、画面がそのままで表示する
// 2013/11/29 TMEJ 張 タブレット版SMB チーフテクニシャン機能開発 END

// 2014/07/15 TMEJ 張 タブレットSMB Job Dispatch機能開発 START
var C_CALLBACK_CONFIRM = 3;             // CALLBACKで確認メッセージボックスを出す
// 2014/07/15 TMEJ 張 タブレットSMB Job Dispatch機能開発 END

// 2014/09/12 TMEJ 張 BTS-390 「1ｽﾄｰﾙで2ﾁｯﾌﾟが作業中（表示のみ）」対応 START
// チップタップできるフラグ
var C_DATA_CHIPTAP_FLG = "CanTapChipFlg"
// 2014/09/12 TMEJ 張 BTS-390 「1ｽﾄｰﾙで2ﾁｯﾌﾟが作業中（表示のみ）」対応 END

// フッター部の情報---------------------------------->

var C_FT_DISPTP_UNSELECTED = 1;   // チップ未選択時
var C_FT_DISPTP_SELECTED = 2;     // チップ選択時

// 選択チップの状態
var C_FT_BTNTP_REZ_KARIKARIREZ = 1;       // 仮仮予約(ストール)
var C_FT_BTNTP_REZ_TEMP = 2;              // 仮予約(ストール)
var C_FT_BTNTP_REZ_DECIDED = 3;           // 確定予約(ストール)
var C_FT_BTNTP_WALKIN = 4;                // 来店予約(ストール)
var C_FT_BTNTP_CONFIRMED_RO = 5;          // R/Oお客様承認(受付ボックス)
var C_FT_BTNTP_CONFIRMED_ADDWORK = 6;     // 追加作業お客様承認(受付ボックス)
var C_FT_BTNTP_RO_PUBLISHED = 7;          // R/Oお客様承認(ストール)
var C_FT_BTNTP_DECIDED_WORKPLAN = 8;      // 作業計画確定(ストール)
var C_FT_BTNTP_WORKING = 9;               // 作業中(ストール)
var C_FT_BTNTP_INTERRRUPT_BOX = 10;       // 中断中(中断ボックス)
var C_FT_BTNTP_INTERRRUPT_STALL = 11;     // 中断中(ストール中断再配置)
var C_FT_BTNTP_END_WORK = 12;             // 作業終了(ストール)
var C_FT_BTNTP_END_DELIVERY = 13;         // 納車済み(ストール)
var C_FT_BTNTP_CONFIRMED_INSPECTION = 14; // 完成検査承認待(完成検査ボックス)
var C_FT_BTNTP_WAIT_CONFIRMEDADDWORK = 15;// 追加作業承認待(追加作業ボックス)
var C_FT_BTNTP_WAITING_WASH = 16;         // 洗車開始待ち(洗車ボックス)
var C_FT_BTNTP_WASHING = 17;              // 洗車中(洗車ボックス)
var C_FT_BTNTP_WAIT_DELIVERY = 18;        // 納車待ち(納車ボックス)
var C_FT_BTNTP_CHANGING_DATE = 19;        // 日付切り替え中(ストール名上に表示)
var C_FT_BTNTP_NOSHOW = 20;               // NoShowボックス
var C_FT_BTNTP_STOP = 21;                 // 中断ボックス
var C_FT_BTNTP_UNAVAILABLE = 22;          // 使用不可エリア(ストール)
var C_FT_BTNTP_CONFIRMED_RO_AVOIDCOPY = 23;          // R/Oお客様承認COPY除き(受付ボックス)
var C_FT_BTNTP_CONFIRMED_ADDWORK_AVOIDCOPY = 24;     // 追加作業お客様承認COPY除き(受付ボックス)
var C_FT_BTNTP_WAIT_CONFIRMEDADDWORK_AVOIDCOPY = 25; // 追加作業承認待COPY除き(追加作業ボックス)
var C_FT_BTNTP_COPYCHIP = 26;              // リレーション配置前チップ
var C_FT_BTNTP_REZ_TEMP_CARIN = 27;        // 仮予約(入庫した)(ストール)
var C_FT_BTNTP_WALKIN_DECIDED = 28;        // 確定予約(飛び込む客)(ストール)
var C_FT_BTNTP_WORKING_NOTSTARTDAY = 29;   // 作業中(実績開始日時と違う日付)(ストール)
//2013/11/29 TMEJ 丁 タブレット版SMB チーフテクニシャン機能開発 START
var C_FT_BTNTP_WAIT_DELIVERY_WASH = 30;    // 納車待ち(納車ボックスの洗車ありのチップ)
var C_FT_BTNTP_REZ_NEW = 31;               // 新規予約(ストール(ロングタップ))
//2013/11/29 TMEJ 丁 タブレット版SMB チーフテクニシャン機能開発 END
// 背景色
var C_BK_COLOR_GRAY = "TbGrayBack";
var C_BK_COLOR_WHITE = "TbWhiteBack";
// ボタン色
var C_FT_BTNCLR_NOCOLOR = 0;              // 色がない
var C_FT_BTNCLR_BLUE = 1;                 // 青色
var C_FT_BTNCLR_RED = 2;                  // 赤色

// ボタンを表示するかどうか
var C_FT_BTNDISP_OFF = 0;                   // ボタン表示しない
var C_FT_BTNDISP_ON = 1;                    // ボタン表示する

// ボタンID
var C_FT_BTNID_CONFIRMED = 300;             // 完成検査承認
var C_FT_BTNID_LATER = 800;                 // 遅れボタン
var C_FT_BTNID_DETAIL = 900;                // 詳細ボタン
var C_FT_BTNID_REZ_CONFIRMED = 1000;        // 予約確定
var C_FT_BTNID_CARIN = 1100;                // 入庫ボタン
var C_FT_BTNID_CANCEL_CARIN = 1200;         // 予約確定取消ボタン
var C_FT_BTNID_TENTATIVE_REZ = 1300;        // 入庫取消ボタン
var C_FT_BTNID_NOSHOW = 1400;               // NOSHOWボタン
var C_FT_BTNID_START = 1500;                // 開始ボタン
var C_FT_BTNID_END = 1600;                  // 終了ボタン
var C_FT_BTNID_STOPJOB = 1700;              // 中断ボタン
var C_FT_BTNID_WASHSTART = 2000;            // 洗車開始ボタン
var C_FT_BTNID_WASHEND = 2100;              // 洗車終了ボタン
var C_FT_BTNID_DELI = 2200;                 // 納車ボタン
var C_FT_BTNID_DEL = 2500;                  // 削除ボタン
// 2013/11/29 TMEJ 丁 タブレット版SMB チーフテクニシャン機能開発 START
var C_FT_BTNID_UNDO = 2600;                 // 削除ボタン
var C_FT_BTNID_MIDFINISH = 2700;            // 日跨ぎ終了ボタン
var C_FT_BTNTP_MOVETOWASH = 2900;           // 洗車へ移動ボタン(納車待ちボックスの洗車ありのチップ)
var C_FT_BTNTP_MOVETODELI = 3000;           // 納車へ移動ボタン(洗車待ちボックスの洗車待ちチップ)
// 2013/11/29 TMEJ 丁 タブレット版SMB チーフテクニシャン機能開発 END

//2014/06/18 TMEJ 丁 タブレット版SMB テレマ走行距離機能開発 START
var C_FT_BTNID_GBOOK = 3100;                  // G-BOOKボタン
//2014/06/18 TMEJ 丁 タブレット版SMB テレマ走行距離機能開発 END

//2015/02/13 TMEJ 範 新プラットフォーム版i-CROP仕様変更対応 (SMBチップ検索及び作業中断機能強化) START
var C_FT_BTNID_FINISHSTOPCHIP = 3200;         // 中断終了ボタン
//2015/02/13 TMEJ 範 新プラットフォーム版i-CROP仕様変更対応 (SMBチップ検索及び作業中断機能強化) END

//2017/08/30 NSK 竹中(悠) REQ-SVT-TMT-20161109-001 SMB iPadにストールクローズ機能の追加 START
var C_FT_BTNID_UNAVAILABLESETTING = 3300;
//2017/08/30 NSK 竹中(悠) REQ-SVT-TMT-20161109-001 SMB iPadにストールクローズ機能の追加 END

// 2017/09/05 NSK 小川 REQ-SVT-TMT-20160906-003 計画済みチップをストールから簡単に外せるようにする START
var C_FT_BTNID_TORECEPTION = 3400;         // 計画取消ボタン
// 2017/09/05 NSK 小川 REQ-SVT-TMT-20160906-003 計画済みチップをストールから簡単に外せるようにする END

// 2019/06/18 NSK 皆川 (トライ店システム評価)次世代サービスオペレーション効率化に向けた、業務適合性検証 START
var C_FT_BTNTP_NOBREAK = 3500;              // 休憩なしボタン 
var C_FT_BTNTP_BREAK = 3600;                // 休憩ありボタン
// 2019/06/18 NSK 皆川 (トライ店システム評価)次世代サービスオペレーション効率化に向けた、業務適合性検証 END

// <----------------------------------フッター部の情報

// チップの情報---------------------------------->
// 遅刻状態
var C_NO_DELAY = 0;             // 遅刻してない
var C_DELAY_PROSPECTS = 1;      // 予定遅刻
var C_DELAY = 2;                // 実際遅刻

// チップタイプ
var C_CHIPTYPE_STALL = 0;           // ストールにあるチップ
var C_CHIPTYPE_STALL_MOVING = 1;    // 移動しているチップ
var C_CHIPTYPE_STALL_NEW = 2;       // 新規チップ
var C_CHIPTYPE_SUBCHIP = 3;         // サブエリアのチップ
var C_CHIPTYPE_OTHER_DAY = 4;       // 別の日で選択しているチップ
var C_CHIPTYPE_POPUP = 5;           // ポップアップボクスに表示されれるチップ
var C_CHIPTYPE_STALL_FASTER = 6;    // C_CHIPTYPE_STALLと表示結果が同じで、スピードが毎チップ0.01秒速い
var C_CHIPTYPE_COPY = 7;            // コピーされたチップ
var C_CHIPTYPE_STALL_KARIKARI = 8;  // 新規チップ
var C_CHIPTYPE_STALL_COPYMOVING = 9; // コピーされたチップがストールに移動チップ

//ステータス
var C_S_NULL        = "0";       // ステータスなし
var C_S_BEFORE_WORK = "10";      // 作業前
var C_S_WORKING     = "20";      // 作業中
var C_S_COMPLETION  = "30";      // 全チップ作業完了(検査中)
var C_S_WAIT_WASH   = "40";      // 洗車待ち
var C_S_WASHING     = "50";      // 洗車中
var C_S_WAIT_DELI   = "60";      // 納車待ち
var C_S_CLEAR_OFF   = "70";      // 清算済み
var C_S_CP_DELI     = "80";      // 納車済み

// 待ち方
var C_WAIT_IN       = "0";       // 店内
var C_WAIT_OUT      = "1";       // 店外

// 受付区分
var C_RFLG_RESERVE  = "0";       // 予約客
var C_RFLG_WALKIN   = "1";       // 飛び込み

// 予約区分
var C_RTYPE_TEMP    = "0";       // 仮予約
var C_RTYPE_COMMITTED = "1";     // 本予約

// 洗車有無
var C_WTYPE_NO      = "0";       // 洗車無し
var C_WTYPE_WASH    = "1";       // 洗車あり

// 部品準備完了
var C_MERCHANDISE_FLAG_COMPLETE = "1";

// 追加作業起票申請状態
var C_AW_ADDINGWORK = "1";       // 追加作業起票中
var C_AW_WAIT_COMMITTED = "2";   // 追加作業承認待ち

// 2013/11/29 TMEJ 張 タブレット版SMB チーフテクニシャン機能開発 START
var C_TEMP_FLG_ON = "1";        // 仮置きフラグ(tempエリアに配置した)
// 2013/11/29 TMEJ 張 タブレット版SMB チーフテクニシャン機能開発 END

// 操作タイプ
var C_ACTION_START = 0;         // 開始
var C_ACTION_MOVE = 1;          // 移動
var C_ACTION_FINISH = 2;        // 終了
var C_ACTION_STOP = 3;          // 中断
var C_ACTION_MIDFINISH = 4;     // 日跨ぎ終了
var C_ACTION_SUBCHIPMOVE = 5;   // 移動(サブチップ)

//2014/07/15 TMEJ 丁 タブレットSMB Job Dispatch機能開発 START
var C_ACTION_ALLSTART = 6;   // 全開始(チップ詳細)
//2014/07/15 TMEJ 丁 タブレットSMB Job Dispatch機能開発 END

// 日跨ぎタイプ
var C_OVERDAYS_NONE = 0;        // 日跨ぎじゃない
var C_OVERDAYS_LEFT = 1;        // 左端が日跨ぎ
var C_OVERDAYS_RIGHT = 2;       // 右端が日跨ぎ
var C_OVERDAYS_BOTH = 3;        // 両端が日跨ぎ

//2014/07/15 TMEJ 張 タブレットSMB Job Dispatch機能開発 START
// 中断されてるJob再開ステータス
var C_RESTARTJOB_NOTSET = 0;    // まだ設定してない
var C_RESTARTJOB_YES = 1;       // 再開する
var C_RESTARTJOB_NO = 2;        // 再開しない

// 中断されてるJob以外のJobを終了するかどうかステータス
var C_FINISHEXCEPTSTOPJOB_NOTSET = 0;    // まだ設定してない
var C_FINISHEXCEPTSTOPJOB_YES = 1;       // 再開する

// 中断Job含むフラグ
var C_STOPPINGJOB_NOTEXIST = "0"; // 中断作業がない
var C_STOPPINGJOB_EXIST = "1";    // 中断作業がある

//2014/07/15 TMEJ 張 タブレットSMB Job Dispatch機能開発 END

//2018/07/06 NSK 可児 TKM Next Gen e-CRB Project Application development Block B-1  Pマーク、Lマークなどを表示 START
var C_ICON_FLAG_P = "1"        //Pアイコン表示
var C_ICON_FLAG_L = "2"        //Lアイコン表示
//2018/07/06 NSK 可児 TKM Next Gen e-CRB Project Application development Block B-1  Pマーク、Lマークなどを表示 END

// <----------------------------------チップの情報

// CallBack---------------------------------->
// callback関数
var C_CALLBACK_WND101 = "gCallbackSC3240101";
var C_CALLBACK_WND201 = "gCallbackSC3240201";
var C_CALLBACK_WND301 = "gCallbackSC3240301";
// 2013/11/29 TMEJ 張 タブレット版SMB チーフテクニシャン機能開発 START
var C_CALLBACK_WND501 = "gCallbackSC3240501";
// 2013/11/29 TMEJ 張 タブレット版SMB チーフテクニシャン機能開発 END

//2017/08/30 NSK 竹中(悠) REQ-SVT-TMT-20161109-001 SMB iPadにストールクローズ機能の追加 START
var C_CALLBACK_WND701 = "gCallbackSC3240701";
//2017/08/30 NSK 竹中(悠) REQ-SVT-TMT-20161109-001 SMB iPadにストールクローズ機能の追加 END

// <----------------------------------CallBack
/******************************************************
* グロバール変数
*******************************************************/
var gStartWorkTime;
var gEndWorkTime;

// チップクラス配列の初期化
var gArrObjChip;
// 関連チップクラス配列の初期化
var gArrObjRelationChip;
// ストールとテクニシャンのarray
var gArrObjStall = Array();
// 2013/11/29 TMEJ 張 タブレット版SMB チーフテクニシャン機能開発 START
var gArrTapStallTechnician;
// 2013/11/29 TMEJ 張 タブレット版SMB チーフテクニシャン機能開発 END
// 描画の情報を記録用array
var gArrDrawCell = new Array();
// CallBack用array
var gOperationList = new Array();
// MovingSubChipのオブジェクト
var gMovingSubChipObj;
// タイムオウト値のarray
var gArrTimeoutValue;
// 初期表示時、画面で表示しないチップのjob_dtl_idアレイ
var gArrJobDtlId;
// 選択しているチップのid
var gSelectedChipId;
// 選択しているセルのid
var gSelectedCellId;
// WhiteBorderの枠が含まれるチップのid
var gWBChipId = "";
// MovingChipのオブジェクト
var gMovingChipObj;
// 他の日のチップのオブジェクト
var gOtherDtChipObj;
// 関連コピーされたチップのオブジェクト
var gCopyChipObj;
// BackChipのオブジェクト
var gArrBackChipObj = Array();
// 検索で探したチップID
var gSearchedChipId;
//2017/07/12 NSK  河谷 REQ-SVT-TMT-20160906-001 チップ検索の検索範囲をPC-SMBと同じにする START
// 検索で探したチップのRO番号
var gSearchedChipRoNum;
// 検索で探したチップRO連番
var gSearchedChipRoSeq;
//2017/07/12 NSK  河谷 REQ-SVT-TMT-20160906-001 チップ検索の検索範囲をPC-SMBと同じにする END
// 検索で探したチップがサプチップのエリアId
var gSearchedSubChipAreaId;
// 最大行数
var gMaxRow;
// 最大列数    
var gMaxCol;
// 1分単位でスクロール関数
var gScrollTimerInterval = "";
// 定期リフレッシュ関数
var gFuncRefreshTimerInterval = "";
// リサイズの単位(分)
var gResizeInterval = 5;
// 画面自動リフレッシュ時間単位(秒)
var gRefreshTimerInterval = 180;
//ページ取得時のサーバとクライアントの時間差
var gServerTimeDifference = 0;
// chipTapイベント用フラグ
var gTouchStartFlg = false;
// チップリサイズ用フラグ
var gStopFingerScrollFlg = false;
// 遅れストールが表示中フラグ
var gShowLaterStallFlg = false;
// スクロール範囲が拡大中フラグ
var gEnlargeScrollHeight = false;
// ポップアップボックスが表示中フラグ
var gPopupBoxId = "";
// 表示されている日付
var gShowDate;
// 文言
var gSC3240101WordIni;
var gSC3240301WordIni;
// 解除関数をするかフラグ
var gCancelFlg = false;
// TbRowにタップフラグ
var gCanTbRowTapFlg = true;
// 使用不可チップ移動できるフラグ
var gCanMoveUnavailableFlg = true;
// タップしているフラグ
var gOnTouchingFlg = false;
// スクロールできるかどうかフラグ
var gCanScrollFlg = true;
// 2013/11/29 TMEJ 張 タブレット版SMB チーフテクニシャン機能開発 START
//// 重複ポップアップウィンドウを削除できるフラグ
//var gCanRemoveDuplWndFlg = true;
// 2013/11/29 TMEJ 張 タブレット版SMB チーフテクニシャン機能開発 END
// 青いdivが表示できるフラグ
var gShowBlueDivFlg = true;
// どの操作タイプで休憩ウィンドウ表示される
var gPopupRestType;
//チップタップ時にストールのスクロール位置記録用
var gTranslateValStallX;
var gTranslateValStallY;
//チップタップ時にサブチップボックスのスクロール位置記録用
var gTranslateValSubBoxX;
// ループ用コピーされたチップID
var gLoopCopyedId = 0;
//開いているサブボックスIDを記録
var gOpenningSubBoxId = "";
// 2013/11/29 TMEJ 張 タブレット版SMB チーフテクニシャン機能開発 START
// 権限コード
var gOpeCode;
// ボタン切替えるステータス
var gButtonStatus = 0;
// データフォーマット：MM/dd
var gDateFormatMMdd = "MM/dd";
// データフォーマット：HH:mm
var gDateFormatHHmm = "HH:mm";
// データフォーマット：YYYY/MM/dd HH:mm
var gDateFormatYYYYMMddHHmm = "YYYY/MM/dd HH:mm";
// 2013/11/29 TMEJ 張 タブレット版SMB チーフテクニシャン機能開発 END

//2014/07/15 TMEJ 張 タブレットSMB Job Dispatch機能開発 START
// バックアップした開始イベントで送信するJson
var gBackupStartJson = new Array();
//2014/07/15 TMEJ 張 タブレットSMB Job Dispatch機能開発 END

//2015/07/30 TMEJ 明瀬 アクティビティインジケータが消えない問題 START

//画面更新中フラグ
//true:更新中 / false:更新中でない
//※初期表示終了後、必ずfalseが設定される
var gUpdatingDisplayFlg;

//2015/07/30 TMEJ 明瀬 アクティビティインジケータが消えない問題 END

/**
* コールバック関数定義
* @param {String} argument サーバーに渡すパラメータ(JSON形式)
* @param {String} callbackFunction コールバック後に実行するメソッド
*/
var gCallbackSC3240101 = {
    doCallback: function (argument, callbackFunction) {

        //2015/07/30 TMEJ 明瀬 アクティビティインジケータが消えない問題 START

        //RefreshMainWndTimer(ReDisplay);

        if (argument.Method != "ShowMainArea" &&
            argument.Method != "ReShowMainAreaFromTheTime") {
            //Callback処理の発生元が以下以外の場合
            //※以下の場合はAI表示直後にリフレッシュタイマーをセットするように改修
            // ・初期表示
            // ・手動更新
            // ・Push更新
            // ・定期更新

            //リフレッシュタイマーセット
            RefreshMainWndTimer(ReDisplay);
        }

        //2015/07/30 TMEJ 明瀬 アクティビティインジケータが消えない問題 END

        this.packedArgument = JSON.stringify(argument);
        this.endCallback = callbackFunction;
        this.beginCallback();
    }
};

/**
* アクティブインジケータ表示操作関数定義
*/
var gMainAreaActiveIndicator = {
    show: function () {
        $.master.OpenLoadingScreen();
    }
    ,
    hide: function () {
        $("#MainAreaActiveIndicator").removeClass("show");
        $("#MainAreaBackGroundLogo").removeClass("SMBBackGroundZIndex1");
        $("#MainAreaBackGroundColor").removeClass("SMBBackGroundZIndex2");
        $.master.CloseLoadingScreen();
    }
};

// 2013/11/29 TMEJ 張 タブレット版SMB チーフテクニシャン機能開発 START
/**
* テクニシャンウィンドウにアクティブインジケータ表示操作関数定義
* 
*/
var gTechnicianActiveIndicator = {
    show: function () {
        $("#TechnicianActiveIndicator").addClass("show");
    }
    ,
    hide: function () {
        $("#TechnicianActiveIndicator").removeClass("show");
    }
};
// 2013/11/29 TMEJ 張 タブレット版SMB チーフテクニシャン機能開発 END


//2013/11/29 TMEJ 丁 タブレット版SMB チーフテクニシャン機能開発 START
/**
* 再表示タイマーをセットする(TabletSMB).
* 
* @param {refreshFunc} 再表示用のJavaScrep関数 -
* @return {-} -
* 
* @example 
*  -
*/
function commonRefreshTimerTabletSMB(refreshFunc) {

    //タイマー間隔の取得
    var refreshTime = Number($("#MstPG_RefreshTimerTime").val());

    //開始時間を保持する
    var startTime = new Date().getTime();

    setTimeout(function () {

        //clearTimer()がされている場合は処理しない
        if (startTime <= timerClearTime) {
            return;
        }

        //出力メッセージを選択する
        var messageString = $("#MstPG_RefreshTimerMessage1").val();

        //警告メッセージ出力
        alert(messageString);

        //操作リストをクリアする
        ClearOperationList();

        //各画面でリフレッシュ処理をする
        if (refreshFunc() === false) {
            //falseが帰ってきたら再読み込み処理をしない
            return;
        }

        //再度、タイマーをセットする
        commonRefreshTimer(refreshFunc)

    }, refreshTime);
}
//2013/11/29 TMEJ 丁 タブレット版SMB チーフテクニシャン機能開発 END
