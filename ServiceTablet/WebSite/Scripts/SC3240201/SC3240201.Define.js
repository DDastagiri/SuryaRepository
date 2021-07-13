/** 
* @fileOverview SC3240201.Define.js(チップ詳細)
* 
* @author TMEJ 岩城
* @version 1.0.0
* 更新：2013/12/04 TMEJ 下村 タブレット版SMB チーフテクニシャン機能開発
* 更新：2014/07/15 TMEJ 丁 タブレットSMB Job Dispatch機能開発
*/

/****************************************
* 定数宣言
****************************************/
//テキストエリアの高さデフォルト値
var C_SC3240201TA_DEFAULTHEIGHT = 90;

//ポップアップをチップ左に表示時の吹出し三角の相対left値
var C_SC3240201POP_DISPLEFT_ARROW_X = 362;

//ポップアップをチップ右に表示時の吹出し三角の相対left値
var C_SC3240201POP_DISPRIGHT_ARROW_X = -17;

//ポップアップを画面右端に表示時のポップアップの相対left値
var C_SC3240201POP_DISPRIGHT_DEFAULT_X = 635;

//タッチイベント名
var C_SC3240201_TOUCH = "mousedown touchstart";

//コールバック時にサーバー側で処理分岐用メソッド名
//画面の作成
var C_SC3240201CALLBACK_CREATEDISP = "CreateChipDetailSL";

//登録ボタンクリック
var C_SC3240201CALLBACK_REGISTER = "Register";

//整備名の取得
var C_SC3240201CALLBACK_GETMERC = "CreateChipDetailMercSL";

//青チェック(変更可能)のクラス名
var C_SC3240201CLASS_CHECKBLUE = "CheckBlue";

//黒チェック(変更不可)のクラス名
var C_SC3240201CLASS_CHECKBLACK = "CheckBlack";

//青テキスト(変更可能)のクラス名
var C_SC3240201CLASS_TEXTBLUE = "TextBlue";

//黒テキスト(変更不可)のクラス名
var C_SC3240201CLASS_TEXTBLACK = "TextBlack";

//背景色グレー(変更不可)のクラス名
var C_SC3240201CLASS_BACKGROUNDGRAY = "BackGroundGray";

//自チップのフォントのクラス名
var C_SC3240201CLASS_FONTBOLD = "FontBold";

//他チップのフォントのクラス名
var C_SC3240201CLASS_FONTNORMAL = "FontNormal";

//丸め込みの時間5分
//var C_ROUNDUPUNITS_5 = 5;

//作業時間最小値（分）
//var C_SC3240201_MINWORKTIME = 5;

//作業時間最大値（分）
var C_SC3240201_MAXWORKTIME = 9995;

//日付の最小値
var C_SC3240201_DATE_MIN_VALUE = Date.parse("1900/01/01 0:00:00");

//チップの状態
var C_SC3240201_NULL             = "0";   // ステータスなし
var C_SC3240201_REZ_TEMP         = "1";   // 仮予約
var C_SC3240201_REZ_COMMITTED    = "2";   // 本予約
var C_SC3240201_CARIN            = "3";   // 入庫済み
var C_SC3240201_WORK_ORDER       = "4";   // 着工指示済み
var C_SC3240201_WORKING          = "5";   // 作業中
var C_SC3240201_FINISH_WORK      = "6";   // 作業完了
var C_SC3240201_STOP             = "7";   // 中断実績
var C_SC3240201_REPOST           = "8";   // 中断再配置
var C_SC3240201_WAIT_WASH        = "9";   // 洗車待ち
var C_SC3240201_WASHING          = "10";  // 洗車中
var C_SC3240201_WAIT_INSPECTION  = "11";  // 検査待ち
var C_SC3240201_INSPECTION       = "12";  // 検査中
var C_SC3240201_WAIT_DELI        = "13";  // 納車待ち
var C_SC3240201_CP_DELI = "14";           // 納車済み
var C_SC3240201_MIDFINISH = "15";         // 日跨ぎ終了

///サービス・商品項目必須区分
var C_SC3240201_NOCHECK = "0";                  // 0：サービス分類、商品を必須入力としない
var C_SC3240201_CHECK_MAINTE_AND_MERC = "1";    // 1：サービス分類、商品を必須入力とする
var C_SC3240201_CHECK_MAINTE = "2";             // 2：サービス分類を必須入力とする


/****************************************
* グローバル変数宣言
****************************************/
//チップ詳細(小)表示時のleft値保存用
var gDetailSPopX;

//チップ表示用開始時間
var gSC3240201DisplayChipStartTime;

//チップ表示用終了時間
var gSC3240201DisplayChipEndTime;

//チップ詳細に表示した予約IDのリスト
var gSC3240201RezIdList;

//チップ詳細に表示した予約のストール利用ステータスリスト
var gSC3240201RezId_StallUseStatusList;

//整備と紐付く予約IDのリスト
var gSC3240201MatchingRezIdList;

//チップ詳細表示時点での、整備と紐付く予約IDのリスト
var gSC3240201Before_MatchingRezIdList;

//チップ詳細表示時点での、飛び込みフラグ
var gSC3240201Before_RezFlg;

//チップ詳細表示時点での、洗車フラグ
var gSC3240201Before_CarWashFlg;

//チップ詳細表示時点での、待ち方フラグ
var gSC3240201Before_WaitingFlg;

//チップ詳細表示時点での、ご用命
var gSC3240201Before_Order;

//2013/12/04 TMEJ 下村 タブレット版SMB チーフテクニシャン機能開発 START
////チップ詳細表示時点での、故障原因
//var gSC3240201Before_Failure;

////チップ詳細表示時点での、診断結果
//var gSC3240201Before_Result;

////チップ詳細表示時点での、アドバイス
//var gSC3240201Before_Advice;
//2013/12/04 TMEJ 下村 タブレット版SMB チーフテクニシャン機能開発 END

//整備コードのリスト
var gSC3240201FixItemCodeList;

//2013/12/04 TMEJ 下村 タブレット版SMB チーフテクニシャン機能開発 START
////整備連番のリスト
//var gSC3240201FixItemSeqList;

//作業内容IDのリスト
var gSC3240201JobinstrucDtlIdList

//作業指示枝番のリスト
var gSC3240201JobInstructSeqList

//作業指示IDのリスト
var gSC3240201JobInstructIdList
//2013/12/04 TMEJ 下村 タブレット版SMB チーフテクニシャン機能開発 END

//作業連番のリスト
var gSC3240201RoJobSeqList;

//2013/12/04 TMEJ 下村 タブレット版SMB チーフテクニシャン機能開発 START
//枝番のリスト
//var gSC3240201SrvAddSeqList;

//チップ詳細に表示した予約の着工指示フラグリスト
var gSC3240201RezId_InvisibleInstructFlgList;
//2013/12/04 TMEJ 下村 タブレット版SMB チーフテクニシャン機能開発 END

//変更前の値保存用（整備種類コンボボックスのサービス分類ID）
var gSC3240201BeforeSvcClassID;

//変更前の値保存用（整備名称コンボボックスの商品ID）
var gSC3240201BeforeMercID;

//2013/12/04 TMEJ 下村 タブレット版SMB チーフテクニシャン機能開発 START
//チップ詳細表示時点での、完成検査フラグ
var gSC3240201Before_CompleteExaminationFlg;

//チップ詳細表示時点での、メモ
var gSC3240201Before_Memo;
//2013/12/04 TMEJ 下村 タブレット版SMB チーフテクニシャン機能開発 END

//2014/07/15 TMEJ 丁 タブレットSMB Job Dispatch機能開発 START
//Chip中断フラグ
var gStopChipFig = "1";
//Chip開始フラグ
var gStartChipFig = "1";
//Chip終了フラグ
var gFinishChipFig = "1";

var gBeforeStartFlg = false;

var gBeforeEndWorkCount = 0;

//2014/07/15 TMEJ 丁 タブレットSMB Job Dispatch機能開発 END

/**
* コールバック関数定義
* 
* @param {String} argument サーバーに渡すパラメータ(JSON形式)
* @param {String} callbackFunction コールバック後に実行するメソッド
* 
*/
var gCallbackSC3240201 = {
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
var gDetailSActiveIndicator = {
    show: function () {
        $("#DetailSActiveIndicator").addClass("show");
    }
    ,
    hide: function () {
        $("#DetailSActiveIndicator").removeClass("show");
    }
};

/**
* オーバーレイ表示操作関数定義
* 
*/
var gDetailOverlay = {
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