/**
* @fileOverview SC3080216 初期ロード時処理
*　　　　　　　　　　　　 各変数の定義
*　　　　　　　　　　　　 各クラスの作成
*
* @author TCS 安田
* @version 1.0.0
*/

//左位置のマージン
var maginX = 200;

//最大左位置の位置
var maxLeftPos = 0;

//現在時の左位置の取得
var currentDateLeftPos = 0;

//アイコン情報配列 (開始工程コード, 終了工程コード, アイコン分類, アイコンURL) ×　アイコン数
var iconSettingArray = null;

//日付リストデータクラスインスタンス作成
var dateCls = null;

//当初目標、現在目標、実績のアイコンクラス
var iconClsA1 = null;     //振当
var iconClsA2 = null;     //入金
var iconClsA3 = null;     //納車
var iconClsB1 = null;     //振当
var iconClsB2 = null;     //入金
var iconClsB3 = null;     //納車
var iconClsD0 = null;     //振当
var iconClsD1 = null;     //振当
var iconClsD2 = null;     //入金
var iconClsD3 = null;     //納車

//計画アイコンクラス
var iconClsC1 = null;     //振当
var iconClsC2 = null;     //入金
var iconClsC3 = null;     //納車

//工程リスト／日時指定ポップアップ
var pouupCls = null;

//工程リスト／日付指定ポップアップ操作用クラス
var procCls1 = null;     //振当
var procCls2 = null;     //入金
var procCls3 = null;     //納車

//工程リスト／日付指定ポップアップ操作用クラス配列
var procClsArray = new Array();

// 初期ロード　//
function SC308016Initialile() {

    //初期値設定　////////////////////////////////////////////////////////////////////////////////////////////////////
    //アイコン情報配列情報セット
    var iconSetting = $("#iconPathList").val();
    iconSettingArray = iconSetting.split(",");

    //日付リストデータクラスインスタンス作成
    dateCls = new DateDataClass();

    //最大左位置の位置　(表示されてるい最後の日付の左位置)
    maxLeftPos = dateCls.getMaxLeftPos();

    //現在時の左位置の取得
    currentDateLeftPos = dateCls.getDateToLeft($("#todayDate").val());

    //当初目標、現在目標、実績のアイコン系　///////////////////////////////////////////////////////////////////////////////////
    //当初目標、現在目標、実績のアイコン操作用クラス作成
    //工程コード、対照ＤＩＶ、日付、アイコン分類（1:通常、4:完了）
    iconClsA1 = new IconDataClass("001", $(".CoordinateA1"), $("#A1Date").val(), 1);
    iconClsA2 = new IconDataClass("002", $(".CoordinateA2"), $("#A2Date").val(), 1);
    iconClsA3 = new IconDataClass("005", $(".CoordinateA3"), $("#A3Date").val(), 1);
    iconClsB1 = new IconDataClass("001", $(".CoordinateB1"), $("#B1Date").val(), 1);
    iconClsB2 = new IconDataClass("002", $(".CoordinateB2"), $("#B2Date").val(), 1);
    iconClsB3 = new IconDataClass("005", $(".CoordinateB3"), $("#B3Date").val(), 1);
    iconClsD0 = new IconDataClass("000", $(".CoordinateD0"), $("#D0Date").val(), 4);
    iconClsD1 = new IconDataClass("001", $(".CoordinateD1"), $("#D1Date").val(), 4);
    iconClsD2 = new IconDataClass("002", $(".CoordinateD2"), $("#D2Date").val(), 4);
    iconClsD3 = new IconDataClass("005", $(".CoordinateD3"), $("#D3Date").val(), 4);

    //前工程／次工程セット
    iconClsA1.setNestData(null, iconClsA2);
    iconClsA2.setNestData(iconClsA1, iconClsA3);
    iconClsA3.setNestData(iconClsA2, null);
    iconClsB1.setNestData(null, iconClsB2);
    iconClsB2.setNestData(iconClsB1, iconClsB3);
    iconClsB3.setNestData(iconClsB2, null);
    iconClsD0.setNestData(null, iconClsD1);
    iconClsD1.setNestData(iconClsD0, iconClsD2);
    iconClsD2.setNestData(iconClsD1, iconClsD3);
    iconClsD3.setNestData(iconClsD2, null);

    //アイコン表示
    iconClsA1.displayIcon();
    iconClsA2.displayIcon();
    iconClsA3.displayIcon();
    iconClsB1.displayIcon();
    iconClsB2.displayIcon();
    iconClsB3.displayIcon();
    iconClsD0.displayIcon();
    iconClsD1.displayIcon();
    iconClsD2.displayIcon();
    iconClsD3.displayIcon();


    //計画アイコン系　////////////////////////////////////////////////////////////////////////////////////////////////////
    //計画アイコン操作用クラス作成
    //工程コード、対照ＤＩＶ、計画日、計画開始日時、計画終了時間、実績日、現在目標
    iconClsC1 = new IconPlanClass("001", $("#Plan1PopupTrigger"), $("#C1Date"), $("#C1StTime"), $("#C1EdTime"), $("#D1Date"), $("#B1Date"));
    iconClsC2 = new IconPlanClass("002", $("#Plan2PopupTrigger"), $("#C2Date"), $("#C2StTime"), $("#C2EdTime"), $("#D2Date"), $("#B2Date"));
    iconClsC3 = new IconPlanClass("005", $("#Plan3PopupTrigger"), $("#C3Date"), $("#C3StTime"), $("#C3EdTime"), $("#D3Date"), $("#B3Date"));

    //前後関係を定義する
    iconClsC1.setNestData(10, null, iconClsC2);
    iconClsC2.setNestData(11, iconClsC1, iconClsC3);
    iconClsC3.setNestData(12, iconClsC2, null);

    //計画アイコンを表示
    iconClsC1.displayIcon();
    iconClsC2.displayIcon();
    iconClsC3.displayIcon();

    setTimeout(function () {
        //アイコンを傾ける
        iconClsC1.transformIcon();
        iconClsC2.transformIcon();
        iconClsC3.transformIcon();
    }, 500);

    //ホップアップ系　////////////////////////////////////////////////////////////////////////////////////////////////////
    //工程リスト／日時指定ポップアップ
    pouupCls = new DatePopupClass();

    //工程リスト／日付指定ポップアップ操作用クラス作成
    procCls1 = new ProcessDateClass(pouupCls, iconClsC1, $("#scPlan1Popup"), $("#planicon1"), $("#planTitleHidden").val());
    procCls2 = new ProcessDateClass(pouupCls, iconClsC2, $("#scPlan2Popup"), $("#planicon2"), $("#planTitleHidden2").val());
    procCls3 = new ProcessDateClass(pouupCls, iconClsC3, $("#scPlan3Popup"), $("#planicon3"), $("#planTitleHidden3").val());

    procCls1.setNestData(null, procCls2);
    procCls2.setNestData(procCls1, procCls3);
    procCls3.setNestData(procCls2, null);

    //工程リストを配列にセット
    procClsArray = new Array();
    procClsArray.push(procCls1);
    procClsArray.push(procCls2);
    procClsArray.push(procCls3);


    //その他設定　////////////////////////////////////////////////////////////////////////////////////////////////////
    //日付軸の設定
    $(".TargetCoordinate0").css("display", "none");
    if (currentDateLeftPos >= 0) {
        //日付軸を設定する
        $(".TargetCoordinate0").css("left", currentDateLeftPos - 7);
        $(".TargetCoordinate0").css("display", "block");
    }
    
}

// 前ページ、次ページボタン押下時　//
function SC308016Initialile2() {

    //画面表示の再設定

    //日付リストデータクラスインスタンス作成
    dateCls = new DateDataClass();

    //現在時の左位置の取得
    currentDateLeftPos = dateCls.getDateToLeft($("#todayDate").val());

    //アイコン表示
    iconClsA1.displayIcon();
    iconClsA2.displayIcon();
    iconClsA3.displayIcon();
    iconClsB1.displayIcon();
    iconClsB2.displayIcon();
    iconClsB3.displayIcon();
    iconClsD0.displayIcon();
    iconClsD1.displayIcon();
    iconClsD2.displayIcon();
    iconClsD3.displayIcon();

    //計画アイコンを表示
    iconClsC1.displayIcon();
    iconClsC2.displayIcon();
    iconClsC3.displayIcon();

    setTimeout(function () {
        //アイコンを傾ける
        iconClsC1.transformIcon();
        iconClsC2.transformIcon();
        iconClsC3.transformIcon();
    }, 500);

    //日付軸の設定
    $(".TargetCoordinate0").css("display", "none");
    if (currentDateLeftPos >= 0) {
        //日付軸を設定する
        $(".TargetCoordinate0").css("left", currentDateLeftPos - 7);
        $(".TargetCoordinate0").css("display", "block");
    }

    if (currentDateLeftPos < 0) {
        if ($("#todayDate").val() < $("#currentDateYYYYMMDD").val()) {
            currentDateLeftPos = -500;
        } else {
            currentDateLeftPos = 500;
        }
    }
}


