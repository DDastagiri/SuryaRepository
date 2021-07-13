/**
* SC3090401.Define.js
* ---------------------------------------------------------
* 作成：2018/02/19 NSK 河谷 REQ-SVT-TMT-20170615-001 iPod Gatekeeperに顧客の車両登録番号を入力する機能を追加
* 更新：2019/02/28 NSK 河谷 REQ-SVT-TMT-20180601-001 Gate Keeper機能の視認性操作性改善 $01
* ---------------------------------------------------------
*/

/****************************************
* 定数宣言
****************************************/
//Cellの高さデフォルト値
// $01 start 2019/02/28 NSK 河谷 REQ-SVT-TMT-20180601-001 Gate Keeper機能の視認性操作性改善
//var C_SC3090401TA_DEFAULTHEIGHT = 142;
var C_SC3090401TA_DEFAULTHEIGHT = 175;
// $01 end 2019/02/28 NSK 河谷 REQ-SVT-TMT-20180601-001 Gate Keeper機能の視認性操作性改善

//スクロールの高さデフォルト値
var C_SC3090401SCR_DEFAULTHEIGHT = 721;
//スクロールの幅デフォルト値
var C_SC3090401SCR_DEFAULTWIDTH = 608;
//スクロールのTOPデフォルト値
var C_SC3090401SCR_DEFAULTTOP = -3;

// データフォーマット：YYYY/MM/dd HH:mm
var gDateFormat = "YYYY/MM/dd HH:mm";

//ロケーションテキストのID
var C_ID_NUM = "Num";
var C_ID_ALP = "Alp";

// 来店済み取得フラグ(0:取得しない)
var C_ALL_DISPLAY_FLAG_OFF = "0";
// 来店済み取得フラグ(1:取得する)
var C_ALL_DISPLAY_FLAG_ON = "1";

// ソート条件区分（0:予約日時）
var C_SORT_TYPE_REZ_DATE = "0";
// ソート条件区分（1:車両登録番号）
var C_SORT_TYPE_REG_NUM = "1";

//初期表示ボタン名
var C_MAIN_LOAD_BUTTON = "MainLoadingButton";

//プルダウンリフレッシュボタン名
var C_PULLDOWN_REFRESH_BUTTON = "PullDownRefreshButton";

/****************************************
* グローバル変数宣言
****************************************/
//来店情報配列の初期化
var gArrObjVisitInfo = new Array();
//更新用来店情報配列の初期化
var gArrObjUpdVisitInfo = new Array();

//選択されたテキストのKey
var gSelectedVisitSeq = "";

//ページ取得時のサーバとクライアントの時間差
var gServerTimeDifference = 0;

// 編集モードフラグ
var gEditeFlg = false;

// 絞り込みモードフラグ
var gSearchFlg = false;

// リロード中であるかどうか
var gIsReload = false;

// ボタン名
var gButtonName;