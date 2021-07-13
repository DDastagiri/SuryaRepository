/** 
* @fileOverview SC3240501.Define.js(新規予約作成)
* 
* @author TMEJ 下村
* @version 1.0.0
* 更新：2019/10/31 NSK 鈴木【（FS）MaaSビジネス向けサービス予約の登録オペレーションの効率化に向けた試験研究】[No156]予定入庫日時、予定納車日時の自動セット
*/

/****************************************
* 定数宣言
****************************************/
//テキストエリアの高さデフォルト値
var C_SC3240501TA_DEFAULTHEIGHT = 90;

//ポップアップをチップ左に表示時の吹出し三角の相対left値
var C_SC3240501POP_DISPLEFT_ARROW_X = 362;

//ポップアップをチップ右に表示時の吹出し三角の相対left値
var C_SC3240501POP_DISPRIGHT_ARROW_X = -17;

//ポップアップを画面右端に表示時のポップアップの相対left値
var C_SC3240501POP_DISPRIGHT_DEFAULT_X = 635;

//タッチイベント名
var C_SC3240501_TOUCH = "mousedown touchstart";

//コールバック時にサーバー側で処理分岐用メソッド名
//画面の作成
var C_SC3240501CALLBACK_CREATEDISP = "CreateNewChip";

//登録ボタンクリック
var C_SC3240501CALLBACK_REGISTER = "RegisterNewChip";

//整備名の取得
var C_SC3240501CALLBACK_GETMERC = "GetMercList";

//青チェック(変更可能)のクラス名
var C_SC3240501CLASS_CHECKBLUE = "CheckBlue";

//黒チェック(変更不可)のクラス名
var C_SC3240501CLASS_CHECKBLACK = "CheckBlack";

//青テキスト(変更可能)のクラス名
var C_SC3240501CLASS_TEXTBLUE = "TextBlue";

//黒テキスト(変更不可)のクラス名
var C_SC3240501CLASS_TEXTBLACK = "TextBlack";

//背景色グレー(変更不可)のクラス名
var C_SC3240501CLASS_BACKGROUNDGRAY = "BackGroundGray";

//自チップのフォントのクラス名
var C_SC3240501CLASS_FONTBOLD = "FontBold";

//他チップのフォントのクラス名
var C_SC3240501CLASS_FONTNORMAL = "FontNormal";

//丸め込みの時間5分
//var C_ROUNDUPUNITS_5 = 5;

//作業時間最小値（分）
//var C_SC3240501_MINWORKTIME = 5;

//作業時間最大値（分）
var C_SC3240501_MAXWORKTIME = 9995;

//日付の最小値
var C_SC3240501_DATE_MIN_VALUE = Date.parse("1900/01/01 0:00:00");

//チップの状態
var C_SC3240501_NULL             = "0";   // ステータスなし
var C_SC3240501_REZ_TEMP         = "1";   // 仮予約
var C_SC3240501_REZ_COMMITTED    = "2";   // 本予約
var C_SC3240501_CARIN            = "3";   // 入庫済み
var C_SC3240501_WORK_ORDER       = "4";   // 着工指示済み
var C_SC3240501_WORKING          = "5";   // 作業中
var C_SC3240501_FINISH_WORK      = "6";   // 作業完了
var C_SC3240501_STOP             = "7";   // 中断実績
var C_SC3240501_REPOST           = "8";   // 中断再配置
var C_SC3240501_WAIT_WASH        = "9";   // 洗車待ち
var C_SC3240501_WASHING          = "10";  // 洗車中
var C_SC3240501_WAIT_INSPECTION  = "11";  // 検査待ち
var C_SC3240501_INSPECTION       = "12";  // 検査中
var C_SC3240501_WAIT_DELI        = "13";  // 納車待ち
var C_SC3240501_CP_DELI = "14";  // 納車済み
var C_SC3240501_MIDFINISH        = "15";  // 日跨ぎ終了

///サービス・商品項目必須区分
var C_SC3240501_NOCHECK = "0";                  // 0：サービス分類、商品を必須入力としない
var C_SC3240501_CHECK_MAINTE_AND_MERC = "1";    // 1：サービス分類、商品を必須入力とする
var C_SC3240501_CHECK_MAINTE = "2";             // 2：サービス分類を必須入力とする

// 2019/10/31 NSK 鈴木 [No156]予定入庫日時、予定納車日時の自動セット START
// 予定入庫納車自動表示フラグ
var C_SCHESVCINDELIAUTODISPFLG_ENABLE = "1";    // 表示
// 検査必要フラグ
var C_INSPECTIONNEEDFLG_NEED = "1";             // 必要
// 洗車必要フラグ
var C_CARWASHNEEDFLG_NEED = "1";                // 必要
// 2019/10/31 NSK 鈴木 [No156]予定入庫日時、予定納車日時の自動セット END

/****************************************
* グローバル変数宣言
****************************************/
//チップ表示時のleft値保存用
var gNewChipPopX;

//チップ表示用開始時間
var gSC3240501DisplayChipStartTime;

//チップ表示用終了時間
var gSC3240501DisplayChipEndTime;

//チップに表示した予約IDのリスト
var gSC3240501RezIdList;

//チップに表示した予約のストール利用ステータスリスト
var gSC3240501RezId_StallUseStatusList;

//整備と紐付く予約IDのリスト
var gSC3240501MatchingRezIdList;

//チップ表示時点での、整備と紐付く予約IDのリスト
var gSC3240501Before_MatchingRezIdList;

//チップ表示時点での、飛び込みフラグ
var gSC3240501Before_RezFlg;

//チップ表示時点での、完成検査フラグ
var gSC3240501Before_CompleteExaminationFlg;

//チップ表示時点での、洗車フラグ
var gSC3240501Before_CarWashFlg;

//チップ表示時点での、待ち方フラグ
var gSC3240501Before_WaitingFlg;

//チップ表示時点での、ご用命
var gSC3240501Before_Order;

//チップ表示時点での、メモ
var gSC3240501Before_Memo;

//整備コードのリスト
var gSC3240501FixItemCodeList;

//整備連番のリスト
var gSC3240501FixItemSeqList;

//変更前の値保存用（整備種類コンボボックスのサービス分類ID）
var gSC3240501BeforeSvcClassID;

//変更前の値保存用（整備名称コンボボックスの商品ID）
var gSC3240501BeforeMercID;

/**
* コールバック関数定義
* 
* @param {String} argument サーバーに渡すパラメータ(JSON形式)
* @param {String} callbackFunction コールバック後に実行するメソッド
* 
*/
var gCallbackSC3240501 = {
    doCallback: function (argument, callbackFunction) {
        this.packedArgument = JSON.stringify(argument);
        this.endCallback = callbackFunction;
        this.beginCallback();
    }
};

/**
* アクティブインジケータ表示操作関数定義
* 
*/
var gNewChipActiveIndicator = {
    show: function () {
        $("#NewChipActiveIndicator").addClass("show");
    }
    ,
    hide: function () {
        $("#NewChipActiveIndicator").removeClass("show");
    }
};

/**
* オーバーレイ表示操作関数定義
* 
*/
var gNewChipOverlay = {
    show: function () {
        $("#MstPG_registOverlayBlack").css({
            "display": "block"
            , "z-index": "10002"
            , "opacity": "0.1"
            , "height": "768px"
        });
    }
    ,
    hide: function () {
        $("#MstPG_registOverlayBlack").css({
            "display": "none"
            , "z-index": "4"
            , "opacity": "0"
            , "height": "703px"
        });
    }
};