'------------------------------------------------------------------------------
'WarningDetail.aspx.vb
'------------------------------------------------------------------------------
'機能：走行距離履歴一覧
'補足：
'作成： 2014/05/12 TMEJ 陳   IT9678_タブレット版SMB（テレマ走行距離機能開発）
'更新： 2016/12/06 NSK 竹中 サブエリアのTCメインフッターのDisable対応
'更新： 
'------------------------------------------------------------------------------
Option Strict On
Option Explicit On

Imports System.Data
Imports System.Reflection
Imports System.Web.Script.Serialization
Imports System.Globalization

Imports Toyota.eCRB.SystemFrameworks.Web.Controls
Imports Toyota.eCRB.SystemFrameworks.Core
Imports Toyota.eCRB.SystemFrameworks.Core.iCROP.BizLogic
Imports Toyota.eCRB.SystemFrameworks.Core.iCROP.DataAccess
Imports Toyota.eCRB.CommonUtility.ServiceCommonClass.Api.DataAccess
Imports Toyota.eCRB.CommonUtility.ServiceCommonClass.Api.BizLogic
Imports Toyota.eCRB.CommonUtility.SMBCommonClass.Api.BizLogic
Imports Toyota.eCRB.CommonUtility.ServiceCommonClass.Api.DataAccess.ServiceCommonClassDataSet
Imports Toyota.eCRB.SMB.Telema.DataAccess
Imports Toyota.eCRB.SMB.Telema.BizLogic
Imports Toyota.eCRB.SMB.Telema.DataAccess.SC3240601DataSet


Partial Class Pages_SC3240601
    Inherits BasePage

#Region "定数"

    ''' <summary>
    ''' 電話帳ボタンのイベント
    ''' </summary>
    ''' <remarks></remarks>
    Private Const FOOTER_EVENT_TEL As String = "return schedule.appExecute.executeCont();"
    ''' <summary>
    ''' フッターボタンコントロールのイベント
    ''' </summary>
    ''' <remarks></remarks>
    Private Const FOOTER_BUTTON_CONTROL_CALL As String = "return FooterButtonControl();"
    ''' <summary>
    ''' Warning詳細ポップアップ表示フラグ（0：何もしない）
    ''' </summary>
    ''' <remarks></remarks>
    Private Const POPUP_TYPE_NONE As String = "0"
    ''' <summary>
    ''' Warning詳細ポップアップ表示フラグ（1：表示）
    ''' </summary>
    ''' <remarks></remarks>
    Private Const POPUP_TYPE_DISPLAY As String = "1"
    ''' <summary>
    ''' １つスベース
    ''' </summary>
    ''' <remarks></remarks>
    Private Const ONE_SPACE As String = " "
    ''' <summary>
    ''' セッションキー(表示番号22：追加作業一覧)
    ''' </summary>
    ''' <remarks></remarks>
    Private Const SESSION_DATA_DISP_NUM_ADD_LIST As Long = 22
    ''' <summary>
    ''' Warning詳細ボタン利用可否フラグ。０：利用不可
    ''' </summary>
    ''' <remarks></remarks>
    Private Const DETAIL_BUTTON_NODISPLAY_FLG As String = "0"
    ''' <summary>
    ''' 登録方法区分1: 基幹入庫履歴
    ''' </summary>
    ''' <remarks></remarks>
    Private Const INFORMATIONSOURCE_CODE_1 As String = "1"
    ''' <summary>
    ''' 登録方法区分2: サイト入力
    ''' </summary>
    ''' <remarks></remarks>
    Private Const INFORMATIONSOURCE_CODE_2 As String = "2"
    ''' <summary>
    ''' 登録方法区分3: 走行距離アンケート
    ''' </summary>
    ''' <remarks></remarks>
    Private Const INFORMATIONSOURCE_CODE_3 As String = "3"
    ''' <summary>
    ''' 登録方法区分4: コールセンター入力
    ''' </summary>
    ''' <remarks></remarks>
    Private Const INFORMATIONSOURCE_CODE_4 As String = "4"
    ''' <summary>
    ''' 登録方法区分5: G-BOOK（代表）
    ''' </summary>
    ''' <remarks></remarks>
    Private Const INFORMATIONSOURCE_CODE_5 As String = "5"
    ''' <summary>
    ''' 登録方法区分6: G-BOOK（複写）
    ''' </summary>
    ''' <remarks></remarks>
    Private Const INFORMATIONSOURCE_CODE_6 As String = "6"
    ''' <summary>
    ''' 登録方法区分7: サイト入力データ（複写）
    ''' </summary>
    ''' <remarks></remarks>
    Private Const INFORMATIONSOURCE_CODE_7 As String = "7"
    ''' <summary>
    ''' 登録方法区分0: Warning情報
    ''' </summary>
    ''' <remarks></remarks>
    Private Const INFORMATIONSOURCE_CODE_0 As String = "0"
    ''' <summary>
    ''' 一覧検索開始Index
    ''' </summary>
    ''' <remarks></remarks>
    Private Const START_INDEX As String = "1"
    ''' <summary>
    ''' 初期スケールモード初期値
    ''' </summary>
    ''' <remarks></remarks>
    Private Const MILE_SCALE_INI_VALUE As String = "3"
    ''' <summary>
    ''' スケールモード:日数初期値
    ''' </summary>
    ''' <remarks></remarks>
    Private Const MILE_SCALE_DAY_DAYSCOUNT As String = "7"
    ''' <summary>
    ''' スケールモード:週数初期値
    ''' </summary>
    ''' <remarks></remarks>
    Private Const MILE_SCALE_WEEK_DAYSCOUNT As String = "28"
    ''' <summary>
    ''' スケールモード:月数初期値
    ''' </summary>
    ''' <remarks></remarks>
    Private Const MILE_SCALE_MONTH_MONTHCOUNT As String = "6"
    ''' <summary>
    ''' インジケータイメージ表示フラグ　1:表示する
    ''' </summary>
    ''' <remarks></remarks>
    Private Const INDICATOR_IMAGE_DISP_FLG_1 As String = "1"
    ''' <summary>
    ''' Warning情報表示フラグ　1:表示する
    ''' </summary>
    ''' <remarks></remarks>
    Private Const USE_WARN_FLG_VALUE_1 As String = "1"
    ''' <summary>
    ''' テレマ情報表示フラグ　1:表示する
    ''' </summary>
    ''' <remarks></remarks>
    Private Const MILE_TLM_DISP_FLG_VALUE_1 As String = "1"
    ''' <summary>
    ''' グラフ線色：青
    ''' </summary>
    ''' <remarks></remarks>
    Private Const LINE_COLOR_BLUE As String = "#00d8ff"
    ''' <summary>
    ''' グラフ線色：黄
    ''' </summary>
    ''' <remarks></remarks>
    Private Const LINE_COLOR_YELLOW As String = "#fcff00"
    ''' <summary>
    ''' グラフ線色：赤
    ''' </summary>
    ''' <remarks></remarks>
    Private Const LINE_COLOR_RED As String = "#e2005a"
    ''' <summary>
    ''' グラフ線色：緑
    ''' </summary>
    ''' <remarks></remarks>
    Private Const LINE_COLOR_GREEN As String = "#00e200"
    ''' <summary>
    ''' SessionValue(ViewMode)：編集
    ''' </summary>
    ''' <remarks></remarks>
    Private Const SESSIONVALUE_EDIT As String = "0"
    ''' <summary>
    ''' SessionValue(ViewMode)：参照
    ''' </summary>
    ''' <remarks></remarks>
    Private Const SESSIONVALUE_READ As String = "1"
    ''' <summary>
    ''' SessionValue(DISP_NUM)：「1：R/O作成」固定
    ''' </summary>
    ''' <remarks></remarks>
    Private Const SESSIONVALUE_DISP_NUM_ROCREATE As String = "1"
    ''' <summary>
    ''' SessionValue(画面番号)：RO一覧
    ''' </summary>
    ''' <remarks></remarks>
    Private Const SESSIONVALUE_RO_LIST As String = "14"
    ''' <summary>
    ''' SessionValue(画面番号)：キャンペーン
    ''' </summary>
    ''' <remarks></remarks>
    Private Const SESSIONVALUE_CAMPAIGN As String = "15"
    ''' <summary>
    ''' 判断用文字true
    ''' </summary>
    ''' <remarks></remarks>
    Private Const CONST_TRUE As String = "true"
    ''' <summary>
    ''' 判断用文字false
    ''' </summary>
    ''' <remarks></remarks>
    Private Const CONST_FALSE As String = "false"
    ''' <summary>
    ''' 走行距離置換用文字列「000」
    ''' </summary>
    ''' <remarks></remarks>
    Private Const REPLACE_MILEAGE_ZERO_TWO As String = "00"
    ''' <summary>
    ''' 走行距離置換用文字列「00」
    ''' </summary>
    ''' <remarks></remarks>
    Private Const REPLACE_MILEAGE_ZERO_THREE As String = "000"
    ''' <summary>
    ''' 走行距離置換用文字列「0000」
    ''' </summary>
    ''' <remarks></remarks>
    Private Const REPLACE_MILEAGE_ZERO_FOUR As String = "0000"

#Region "画面ID"

    ''' <summary>
    ''' 当画面機能ID
    ''' </summary>
    ''' <remarks></remarks>
    Private Const APPLICATION_ID As String = "SC3240601"
    ''' <summary>
    ''' メインメニュー(SA)画面ID
    ''' </summary>
    ''' <remarks></remarks>
    Private Const MAINMENU_ID_SA As String = "SC3140103"
    ''' <summary>
    ''' 全体管理画面ID
    ''' </summary>
    ''' <remarks></remarks>
    Private Const PROGRAM_ID_ALL_MANAGMENT As String = "SC3220201"
    ''' <summary>
    ''' メインメニュー(FM)画面ID
    ''' </summary>
    ''' <remarks></remarks>
    Private Const MAINMENU_ID_FM As String = "SC3230101"
    ''' <summary>
    ''' 工程管理画面ID
    ''' </summary>
    ''' <remarks></remarks>
    Private Const PROCESS_CONTROL_PAGE As String = "SC3240101"
    ''' <summary>
    ''' 来店管理画面ID
    ''' </summary>
    ''' <remarks></remarks>
    Private Const APPLICATIONID_VSTMANAGER As String = "SC3100303"
    ''' <summary>
    ''' 現地にシステム連携用画面ID
    ''' </summary>
    ''' <remarks></remarks>
    Private Const OTHER_LINKAGE_PAGE As String = "SC3010501"
    ''' <summary>
    ''' プログラムID：商品訴求コンテンツ画面
    ''' </summary>
    ''' <remarks></remarks>
    Private Const PGMID_GOOD_SOLICITATION_CONTENTS As String = "SC3250101"

#End Region

#Region "システム設定名"

    ''' <summary>
    ''' 検索標準読み込み数
    ''' </summary>
    ''' <remarks></remarks>
    Private Const DEFAULT_READ_COUNT As String = "SC3240601_DEFAULT_READ_COUNT"
    ''' <summary>
    ''' 検索最大表示数
    ''' </summary>
    ''' <remarks></remarks>
    Private Const MAX_DISPLAY_COUNT As String = "SC3240601_MAX_DISPLAY_COUNT"
    ''' <summary>
    ''' テレマ導入フラグ
    ''' </summary>
    ''' <remarks></remarks>
    Private Const TEREMA_INTRODUCTION As String = "TEREMA_INTRODUCTION"
    ''' <summary>
    ''' 表示する日数
    ''' </summary>
    ''' <remarks></remarks>
    Private Const MILE_WARN_DISP_DAY_COUNT As String = "MILE_WARN_DISP_DAY_COUNT"
    ''' <summary>
    ''' 入庫情報連携元基幹システム名
    ''' </summary>
    ''' <remarks></remarks>
    Private Const SVC_DMS_NAME As String = "SVC_DMS_NAME"
    ''' <summary>
    ''' Warning詳細イメージ表示フラグ
    ''' </summary>
    ''' <remarks></remarks>
    Private Const USE_WARN_IMG_DISP_FLG As String = "USE_WARN_IMG_DISP_FLG"
    ''' <summary>
    ''' Warning詳細イメージ表示フラグ
    ''' </summary>
    ''' <remarks></remarks>
    Private Const WARN_IMG_BASE_URL_TABLET As String = "WARN_IMG_BASE_URL_TABLET"
    ''' <summary>
    ''' 初期スケールモード
    ''' </summary>
    ''' <remarks></remarks>
    Private Const MILE_SCALE_INIT As String = "MILE_SCALE_INIT"
    ''' <summary>
    ''' スケールモード:日数
    ''' </summary>
    ''' <remarks></remarks>
    Private Const MILE_SCALE_DAY_COUNT As String = "MILE_SCALE_DAY_COUNT"
    ''' <summary>
    ''' スケールモード:週数
    ''' </summary>
    ''' <remarks></remarks>
    Private Const MILE_SCALE_WEEKLY_COUNT As String = "MILE_SCALE_WEEKLY_COUNT"
    ''' <summary>
    ''' スケールモード:月数
    ''' </summary>
    ''' <remarks></remarks>
    Private Const MILE_SCALE_MONTH_COUNT As String = "MILE_SCALE_MONTH_COUNT"
    ''' <summary>
    ''' GBOOK表示件数
    ''' </summary>
    ''' <remarks></remarks>
    Private Const TLM_DISP_COUNT As String = "TLM_DISP_COUNT"
    ''' <summary>
    ''' Warning情報表示フラグ
    ''' </summary>
    ''' <remarks></remarks>
    Private Const USE_WARN_FLG As String = "USE_WARN_FLG"
    ''' <summary>
    ''' テレマ情報表示フラグ
    ''' </summary>
    ''' <remarks></remarks>
    Private Const MILE_TLM_DISP_FLG As String = "MILE_TLM_DISP_FLG"

#End Region

#Region "SESSION KEY"

    ''' <summary>
    ''' Session.VCL_ID
    ''' </summary>
    ''' <remarks></remarks>
    Private Const SESSIONKEY_VCL_ID As String = "Session.VCL_ID"

    ''' <summary>
    ''' SessionKey(DealerCode)
    ''' </summary>
    ''' <remarks></remarks>
    Private Const SESSIONKEY_TONEC_DEALERCODE As String = "DealerCode"
    ''' <summary>
    ''' SessionKey(BranchCode)
    ''' </summary>
    ''' <remarks></remarks>
    Private Const SESSIONKEY_TONEC_BRANCHCODE As String = "BranchCode"
    ''' <summary>
    ''' SessionKey(LoginUserID)
    ''' </summary>
    ''' <remarks></remarks>
    Private Const SESSIONKEY_TONEC_LOGINUSERID As String = "LoginUserID"
    ''' <summary>
    ''' SessionKey(SAChipID)
    ''' </summary>
    ''' <remarks></remarks>
    Private Const SESSIONKEY_TONEC_SACHIPID As String = "SAChipID"
    ''' <summary>
    ''' SessionKey(BASREZID)
    ''' </summary>
    ''' <remarks></remarks>
    Private Const SESSIONKEY_TONEC_BASREZID As String = "BASREZID"
    ''' <summary>
    ''' SessionKey(R_O)
    ''' </summary>
    ''' <remarks></remarks>
    Private Const SESSIONKEY_TONEC_RO As String = "R_O"
    ''' <summary>
    ''' SessionKey(SEQ_NO)
    ''' </summary>
    ''' <remarks></remarks>
    Private Const SESSIONKEY_TONEC_SEQ_NO As String = "SEQ_NO"
    ''' <summary>
    ''' SessionKey(VIN_NO)
    ''' </summary>
    ''' <remarks></remarks>
    Private Const SESSIONKEY_TONEC_VIN_NO As String = "VIN_NO"
    ''' <summary>
    ''' SessionKey(CustomerID)
    ''' </summary>
    ''' <remarks></remarks>
    Private Const SESSIONKEY_TONEC_CUSTOMERID As String = "CustomerID"
    ''' <summary>
    ''' SessionKey(ViewMode)
    ''' </summary>
    ''' <remarks></remarks>
    Private Const SESSIONKEY_TONEC_VIEWMODE As String = "ViewMode"

    ''' <summary>
    ''' Sessionキー（整備受注No）
    ''' </summary>
    ''' <remarks></remarks>
    Private Const SESSIONKEY_ORDERNO As String = "OrderNo"
    ''' <summary>
    ''' SessionKey(DearlerCode):ログインユーザーのDMS販売店コード
    ''' </summary>
    ''' <remarks></remarks>
    Private Const SESSIONKEY_DEARLER_CODE As String = "Session.Param1"
    ''' <summary>
    ''' SessionKey(BranchCode):ログインユーザーのDMS店舗コード
    ''' </summary>
    ''' <remarks></remarks>
    Private Const SESSIONKEY_BRANCH_CODE As String = "Session.Param2"
    ''' <summary>
    ''' SessionKey(LoginUserID):ログインユーザーのアカウント
    ''' </summary>
    ''' <remarks></remarks>
    Private Const SESSIONKEY_LOGIN_USER_ID As String = "Session.Param3"
    ''' <summary>
    ''' SessionKey(SAChipID):来店管理番号
    ''' </summary>
    ''' <remarks></remarks>
    Private Const SESSIONKEY_SA_CHIP_ID As String = "Session.Param4"
    ''' <summary>
    ''' SessionKey(BASREZID):DMS予約ID
    ''' </summary>
    ''' <remarks></remarks>
    Private Const SESSIONKEY_BASREZID As String = "Session.Param5"
    ''' <summary>
    ''' SessionKey(R_O):RO番号
    ''' </summary>
    ''' <remarks></remarks>
    Private Const SESSIONKEY_R_O As String = "Session.Param6"
    ''' <summary>
    ''' SessionKey(SEQ_NO):RO作業連番
    ''' </summary>
    ''' <remarks></remarks>
    Private Const SESSIONKEY_SEQ_NO As String = "Session.Param7"
    ''' <summary>
    ''' SessionKey(VIN_NO):車両登録No.のVIN
    ''' </summary>
    ''' <remarks></remarks>
    Private Const SESSIONKEY_VIN_NO As String = "Session.Param8"
    ''' <summary>
    ''' SessionKey(ViewMode)
    ''' </summary>
    ''' <remarks></remarks>
    Private Const SESSIONKEY_VIEW_MODE As String = "Session.Param9"
    ''' <summary>
    ''' SessionKey(Format)
    ''' </summary>
    ''' <remarks></remarks>
    Private Const SESSIONKEY_FORMAT As String = "Session.Param10"
    ''' <summary>
    ''' SessionKey(CustomerID)
    ''' </summary>
    ''' <remarks></remarks>
    Private Const SESSIONKEY_CUSTOMER_ID As String = "Session.Param10"
    ''' <summary>
    ''' SessionKey(DISP_NUM)：画面番号
    ''' </summary>
    ''' <remarks></remarks>
    Private Const SESSIONKEY_DISP_NUM As String = "Session.DISP_NUM"

#End Region

#Region "文言ID"
    ''' <summary>
    ''' 文言ID
    ''' </summary>
    ''' <remarks></remarks>
    Private Enum WordId
        ''' <summary>なし</summary>
        id000 = 0
        ''' <summary>G-BOOK Information</summary>
        id001 = 1
        ''' <summary>Vehicle Information</summary>
        id002 = 2
        ''' <summary>Owner</summary>
        id003 = 3
        ''' <summary>Model</summary>
        id004 = 4
        ''' <summary>VIN</summary>
        id005 = 5
        ''' <summary>Reg. No.</summary>
        id006 = 6
        ''' <summary>Mileage Trajectory</summary>
        id007 = 7
        ''' <summary>G-BOOK</summary>
        id008 = 8
        ''' <summary>Site/i-CROP</summary>
        id009 = 9
        ''' <summary>Warning</summary>
        id010 = 10
        ''' <summary>L-DMS</summary>
        id011 = 11
        ''' <summary>Day</summary>
        id012 = 12
        ''' <summary>Week</summary>
        id013 = 13
        ''' <summary>Month</summary>
        id014 = 14
        ''' <summary>No.</summary>
        id015 = 15
        ''' <summary>Date</summary>
        id016 = 16
        ''' <summary>Mileage</summary>
        id017 = 17
        ''' <summary>Information Source</summary>
        id018 = 18
        ''' <summary>Customer</summary>
        id019 = 19
        ''' <summary>Information</summary>
        id020 = 20
        ''' <summary>Detail</summary>
        id021 = 21
        ''' <summary>km</summary>
        id022 = 22
        ''' <summary>Other Dealer</summary>
        id023 = 23
        ''' <summary>Owner Site</summary>
        id024 = 24
        ''' <summary>SMS</summary>
        id025 = 25
        ''' <summary>i-CROP</summary>
        id026 = 26
        ''' <summary>G-BOOK</summary>
        id027 = 27
        ''' <summary>G-BOOK(Warning)</summary>
        id028 = 28
        ''' <summary>{0}({1})</summary>
        id029 = 29
        ''' <summary>Close</summary>
        id030 = 30
        ''' <summary>Warning Details</summary>
        id031 = 31
        ''' <summary>Date</summary>
        id032 = 32
        ''' <summary>Code</summary>
        id033 = 33
        ''' <summary>Mileage</summary>
        id034 = 34
        ''' <summary>Name</summary>
        id035 = 35
        ''' <summary>Indicator</summary>
        id036 = 36
        ''' <summary>Description</summary>
        id037 = 37
        ''' <summary>Load {0} before…</summary>
        id038 = 38
        ''' <summary>Loading…</summary>
        id039 = 39
        ''' <summary>Load {0} more... </summary>
        id040 = 40
        ''' <summary>-</summary>
        id041 = 41
        ''' <summary>データベースへのアクセスにてタイムアウトが発生しました。再度実行して下さい。</summary>
        id901 = 901
        ''' <summary>予期せぬエラーが発生したため、顧客検索できませんでした。</summary>
        id902 = 902
    End Enum
#End Region

#End Region

#Region "外部変数"
    ''' <summary>
    ''' グラフデータ
    ''' </summary>
    ''' <remarks></remarks>
    Private ChartJsonDataTabel As New SC3240601GraphJsonDataTable

#End Region

#Region "初期処理"

    ''' <summary>
    ''' 初期処理
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    ''' <hitory></hitory>
    Protected Sub Page_Load(sender As Object, e As System.EventArgs) Handles Me.Load
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} START" _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name))

        'スタッフ情報保持
        Dim staffInfo As StaffContext = StaffContext.Current

        '初回読み込み時
        If Not IsPostBack Then

            'グラフボタン、来年ボタン利用不可
            Me.HiddenGraphPreButtonEnable.Value = CONST_FALSE
            'グラフボタン、去年ボタン利用不可
            Me.HiddenGraphNextButtonEnable.Value = CONST_FALSE

            'システム設定取得変数
            Dim systemEnv As New SystemEnvSetting

            'システム設定格納変数
            Dim drSystemEnvSetting As SystemEnvSettingDataSet.SYSTEMENVSETTINGRow

            '一回読み込み件数を取得
            Dim loadCount As String = "10"
            drSystemEnvSetting = systemEnv.GetSystemEnvSetting(DEFAULT_READ_COUNT)

            '取得結果チェック
            If Not (IsNothing(drSystemEnvSetting)) Then
                '取得できた場合

                '結果を設定
                loadCount = systemEnv.GetSystemEnvSetting(DEFAULT_READ_COUNT).PARAMVALUE

            End If

            '一覧最大表示件数を取得
            Dim maxDisplayCount As String = "20"
            drSystemEnvSetting = systemEnv.GetSystemEnvSetting(MAX_DISPLAY_COUNT)

            '取得結果チェック
            If Not (IsNothing(drSystemEnvSetting)) Then
                '取得できた場合

                '結果を設定
                maxDisplayCount = systemEnv.GetSystemEnvSetting(MAX_DISPLAY_COUNT).PARAMVALUE

            End If

            'Warning詳細イメージ表示URLベースを保持
            drSystemEnvSetting = systemEnv.GetSystemEnvSetting(WARN_IMG_BASE_URL_TABLET)

            '取得結果チェック
            If Not (IsNothing(drSystemEnvSetting)) Then
                '取得できた場合

                '結果を設定
                Me.HiddenImageUrl.Value = systemEnv.GetSystemEnvSetting(WARN_IMG_BASE_URL_TABLET).PARAMVALUE

            End If

            '検索開始Index
            Me.HiddenStartIndex.Value = START_INDEX
            '検索終了Index
            Me.HiddenEndIndex.Value = loadCount
            '一回読み込み件数を保持
            Me.HiddenLoadCount.Value = loadCount
            '一覧最大表示件数を保持
            Me.HiddenMaxDisplayCount.Value = maxDisplayCount

            '販売店システム設定変数
            Dim daDealerEnvSetting As New DealerEnvSetting

            '販売店システム設定格納変数
            Dim drDealerEnvSetting As DlrEnvSettingDataSet.DLRENVSETTINGRow

            'テレマ導入フラグを取得
            Dim teremaIntroduction As String = "0"
            drDealerEnvSetting = daDealerEnvSetting.GetEnvSetting(staffInfo.DlrCD, TEREMA_INTRODUCTION)
            '取得結果チェック
            If Not (IsNothing(drDealerEnvSetting)) Then
                '取得できた場合

                '結果を設定
                teremaIntroduction = daDealerEnvSetting.GetEnvSetting(staffInfo.DlrCD, TEREMA_INTRODUCTION).PARAMVALUE

            End If

            'テレマ導入フラグを設定
            Me.HiddenTeremaIntroduction.Value = teremaIntroduction

            'よく使う文言を保持
            'G-BOOK
            Me.HiddenWord008GBOOK.Value = WebWordUtility.GetWord(APPLICATION_ID, WordId.id008)
            'Detail
            Me.HiddenWord021Detail.Value = WebWordUtility.GetWord(APPLICATION_ID, WordId.id021)
            'Other Dealer
            Me.HiddenWord023OtherDealer.Value = WebWordUtility.GetWord(APPLICATION_ID, WordId.id023)
            'Owner Site
            Me.HiddenWord024OwnerSite.Value = WebWordUtility.GetWord(APPLICATION_ID, WordId.id024)
            'SMS
            Me.HiddenWord025SMS.Value = WebWordUtility.GetWord(APPLICATION_ID, WordId.id025)
            'i-CROP
            Me.HiddenWord026iCROP.Value = WebWordUtility.GetWord(APPLICATION_ID, WordId.id026)
            'G-BOOK
            Me.HiddenWord027GBOOK.Value = WebWordUtility.GetWord(APPLICATION_ID, WordId.id027)
            'G-BOOK(Warning)
            Me.HiddenWord028GBOOKWarning.Value = WebWordUtility.GetWord(APPLICATION_ID, WordId.id028)
            'Format: {0}({1})
            Me.HiddenWord029Format.Value = WebWordUtility.GetWord(APPLICATION_ID, WordId.id029)
            '-
            Me.HiddenWord041Hyphen.Value = WebWordUtility.GetWord(APPLICATION_ID, WordId.id041)

            '所有者情報タイトル文言を取得
            'ロゴ　タイトル
            Me.lblTitleVehicleInformation.Text = WebWordUtility.GetWord(APPLICATION_ID, WordId.id002)
            'オーナー名　タイトル
            Me.lblTitleOwner.Text = WebWordUtility.GetWord(APPLICATION_ID, WordId.id003)
            'VIN　タイトル
            Me.lblTitleVin.Text = WebWordUtility.GetWord(APPLICATION_ID, WordId.id005)
            'モデル　タイトル
            Me.lblTitleModel.Text = WebWordUtility.GetWord(APPLICATION_ID, WordId.id004)
            '登録番号　タイトル
            Me.lblTitleRegNo.Text = WebWordUtility.GetWord(APPLICATION_ID, WordId.id006)

            'ページング文言
            '前件読込
            Me.BackPageWord.Text = WebWordUtility.GetWord(APPLICATION_ID, WordId.id038).Replace("{0}", loadCount)
            '読込中（前件）
            Me.BackPageLoadWord.Text = WebWordUtility.GetWord(APPLICATION_ID, WordId.id039)
            '次件読込
            Me.NextPageWord.Text = WebWordUtility.GetWord(APPLICATION_ID, WordId.id040).Replace("{0}", loadCount)
            '読込中（次件）
            Me.NextPageLoadWord.Text = WebWordUtility.GetWord(APPLICATION_ID, WordId.id039)

            'グラフエリア　タイトル
            Me.lblMileageTrajectoryTitle.Text = WebWordUtility.GetWord(APPLICATION_ID, WordId.id007)
            'Dayスケールボタン　タイトル
            Me.lblGraphDayTitle.Text = WebWordUtility.GetWord(APPLICATION_ID, WordId.id012)
            'Weekスケールボタン　タイトル
            Me.lblGraphWeekTitle.Text = WebWordUtility.GetWord(APPLICATION_ID, WordId.id013)
            'Monthスケールボタン　タイトル
            Me.lblGraphMonthTitle.Text = WebWordUtility.GetWord(APPLICATION_ID, WordId.id014)

            'グラフ関係文言
            Dim dmsName As String = String.Empty
            Dim occurdateOffsetCount As String = String.Empty
            Dim imageDisplayFlg As String = String.Empty
            Dim mileScaleInt As String = String.Empty
            Dim mileScaleDayCOunt As String = String.Empty
            Dim mileScaleWeeklyCount As String = String.Empty
            Dim mileScaleMonthCount As String = String.Empty
            Dim telemaDisplayCount As String = String.Empty
            Me.HiddenUserWarnFlg.Value = String.Empty
            Me.HiddenMileTlmDispFlg.Value = String.Empty

            Using serviceCommonBiz As New ServiceCommonClassBusinessLogic
                '基幹システム名
                dmsName = serviceCommonBiz.GetSystemSettingValueBySettingName(SVC_DMS_NAME)
                '発生日時計算用値
                occurdateOffsetCount = serviceCommonBiz.GetSystemSettingValueBySettingName(MILE_WARN_DISP_DAY_COUNT)
                'Warning詳細イメージ表示フラグ
                imageDisplayFlg = serviceCommonBiz.GetSystemSettingValueBySettingName(USE_WARN_IMG_DISP_FLG)
                'グラフ初期スケールモード
                mileScaleInt = serviceCommonBiz.GetSystemSettingValueBySettingName(MILE_SCALE_INIT)
                'グラフスケールモード：日数
                mileScaleDayCOunt = serviceCommonBiz.GetSystemSettingValueBySettingName(MILE_SCALE_DAY_COUNT)
                'グラフスケールモード：週数
                mileScaleWeeklyCount = serviceCommonBiz.GetSystemSettingValueBySettingName(MILE_SCALE_WEEKLY_COUNT)
                'グラフスケールモード：月数
                mileScaleMonthCount = serviceCommonBiz.GetSystemSettingValueBySettingName(MILE_SCALE_MONTH_COUNT)
                'GBOOK表示件数
                telemaDisplayCount = serviceCommonBiz.GetSystemSettingValueBySettingName(TLM_DISP_COUNT)
                'グラフWarning情報表示フラグ
                Me.HiddenUserWarnFlg.Value = serviceCommonBiz.GetSystemSettingValueBySettingName(USE_WARN_FLG)
                'グラフテレマ情報表示フラグ
                Me.HiddenMileTlmDispFlg.Value = serviceCommonBiz.GetSystemSettingValueBySettingName(MILE_TLM_DISP_FLG)
            End Using

            'Warning詳細イメージ表示フラグを保持
            Me.HiddenImageDisplayFlg.Value = imageDisplayFlg
            '発生日時計算用値を保持
            Me.HiddenWarningDispDays.Value = occurdateOffsetCount

            If Not String.IsNullOrEmpty(dmsName) Then
                '基幹システム名はNullOrEmptyではない場合、全空白を消す処理
                If String.IsNullOrEmpty(dmsName.Trim()) Then
                    dmsName = dmsName.Trim()
                End If
            Else
                dmsName = String.Empty
            End If
            Me.HiddenDmsName.Value = dmsName

            'グラフLengend1文言
            Me.HiddenGraphLegend1.Value = Me.HiddenWord008GBOOK.Value.ToString(CultureInfo.CurrentCulture)
            'グラフLengend2文言
            Me.HiddenGraphLegend2.Value = WebWordUtility.GetWord(APPLICATION_ID, WordId.id009)
            'グラフLengend3文言
            Me.HiddenGraphLegend3.Value = WebWordUtility.GetWord(APPLICATION_ID, WordId.id010)

            If String.IsNullOrEmpty(dmsName) Then
                '基幹システム名はNullOrEmptyの場合、「-」をグラフLengend4の文言にする
                Me.HiddenGraphLegend4.Value = Me.HiddenWord041Hyphen.Value.ToString(CultureInfo.CurrentCulture)
            Else
                'グラフLengend4表示文字
                Me.HiddenGraphLegend4.Value = dmsName
            End If

            'km単位　文言
            Me.HiddenKm.Value = WebWordUtility.GetWord(APPLICATION_ID, WordId.id022)

            '走行履歴情報一覧タイトル
            'No　列タイトル
            Me.lblNoHeader.Text = WebWordUtility.GetWord(APPLICATION_ID, WordId.id015)
            'Date　列タイトル
            Me.lblDateHeader.Text = WebWordUtility.GetWord(APPLICATION_ID, WordId.id016)
            'Mileage　列タイトル
            Me.lblMileageHeader.Text = WebWordUtility.GetWord(APPLICATION_ID, WordId.id017)
            'InformationSource　列タイトル
            Me.lblInformationSourceHeader.Text = WebWordUtility.GetWord(APPLICATION_ID, WordId.id018)
            'Customer　列タイトル
            Me.lblCustomerHeader.Text = WebWordUtility.GetWord(APPLICATION_ID, WordId.id019)
            'Information　列タイトル
            Me.lblInformationHeader.Text = WebWordUtility.GetWord(APPLICATION_ID, WordId.id020)
            'Detail　列タイトル
            Me.lblDetailHeader.Text = Me.HiddenWord021Detail.Value.ToString(CultureInfo.CurrentCulture)

            'Warning詳細
            'PopUp画面タイトル
            Me.lblPopUpTitle.Text = WebWordUtility.GetWord(APPLICATION_ID, WordId.id031)
            'Closeボタン文言
            Me.PopUpCloseButton.Text = WebWordUtility.GetWord(APPLICATION_ID, WordId.id030)
            'Date　文言
            Me.lblDate_Title_Detail.Text = WebWordUtility.GetWord(APPLICATION_ID, WordId.id032)
            'Code　文言
            Me.lblCode_Title_Detail.Text = WebWordUtility.GetWord(APPLICATION_ID, WordId.id033)
            'Mileage　文言
            Me.lblMileage_Title_Detail.Text = WebWordUtility.GetWord(APPLICATION_ID, WordId.id034)
            'Name　文言
            Me.lblName_Title_Detail.Text = WebWordUtility.GetWord(APPLICATION_ID, WordId.id035)
            'Indicator　文言
            Me.lblIndicator_Title_Detail.Text = WebWordUtility.GetWord(APPLICATION_ID, WordId.id036)
            'Description　文言
            Me.lblDescription_Title_Detail.Text = WebWordUtility.GetWord(APPLICATION_ID, WordId.id037)

            'スケールパラメータを設定
            If Not String.IsNullOrEmpty(mileScaleInt) Then
                '初期スケールはNullOrEmptyではない

                If IsNumeric(mileScaleInt) AndAlso _
                   CType(mileScaleInt, Integer) >= 1 AndAlso _
                   CType(mileScaleInt, Integer) <= 3 Then

                    '値は数字、そして1から3の範囲の場合
                    Me.HiddenMileScaleInit.Value = mileScaleInt
                Else

                    '初期値にする
                    Me.HiddenMileScaleInit.Value = MILE_SCALE_INI_VALUE
                End If
            Else
                '初期値にする
                Me.HiddenMileScaleInit.Value = MILE_SCALE_INI_VALUE
            End If

            If Not String.IsNullOrEmpty(mileScaleDayCOunt) Then
                'スケール日数はNullOrEmptyではない

                If IsNumeric(mileScaleDayCOunt) AndAlso _
                   CType(mileScaleDayCOunt, Integer) >= 2 AndAlso _
                   CType(mileScaleDayCOunt, Integer) <= 12 Then

                    '値は数字、そして2から12の範囲の場合
                    Me.HiddenMileScaleDayCount.Value = mileScaleDayCOunt
                Else
                    '初期値にする
                    Me.HiddenMileScaleDayCount.Value = MILE_SCALE_DAY_DAYSCOUNT
                End If
            Else
                '初期値にする
                Me.HiddenMileScaleDayCount.Value = MILE_SCALE_DAY_DAYSCOUNT
            End If

            If Not String.IsNullOrEmpty(mileScaleWeeklyCount) Then
                'スケール週数はNullOrEmptyではない

                If IsNumeric(mileScaleWeeklyCount) AndAlso _
                   CType(mileScaleWeeklyCount, Integer) >= 1 AndAlso _
                   CType(mileScaleWeeklyCount, Integer) <= 12 Then
                    '値は数字、そして1から12の範囲の場合

                    Dim weeks As Integer = CType(mileScaleWeeklyCount, Integer)
                    Dim days As Integer = 7 * weeks
                    Me.HiddenMileScaleWeeklyCount.Value = days.ToString(CultureInfo.CurrentCulture)
                Else
                    '初期値にする
                    Me.HiddenMileScaleWeeklyCount.Value = MILE_SCALE_WEEK_DAYSCOUNT
                End If
            Else
                '初期値にする
                Me.HiddenMileScaleWeeklyCount.Value = MILE_SCALE_WEEK_DAYSCOUNT
            End If

            If Not String.IsNullOrEmpty(mileScaleMonthCount) Then

                'スケール月数はNullOrEmptyではない
                If IsNumeric(mileScaleMonthCount) AndAlso _
                   CType(mileScaleMonthCount, Integer) >= 1 AndAlso _
                   CType(mileScaleMonthCount, Integer) <= 12 Then
                    '値は数字、そして1から12の範囲の場合

                    Dim month As Integer = CType(mileScaleMonthCount, Integer)
                    Me.HiddenMileScaleMonthCount.Value = mileScaleMonthCount
                    Dim spDays As TimeSpan
                    spDays = Date.Now - Date.Now.AddMonths(-month)
                    Me.HiddenMileScaleMonthCountDays.Value = spDays.Days.ToString(CultureInfo.CurrentCulture)
                Else

                    Dim spDays As TimeSpan
                    spDays = Date.Now - Date.Now.AddMonths(-6)
                    '初期値にする
                    Me.HiddenMileScaleMonthCount.Value = MILE_SCALE_MONTH_MONTHCOUNT
                    Me.HiddenMileScaleMonthCountDays.Value = spDays.Days.ToString(CultureInfo.CurrentCulture)
                End If
            Else

                Dim spDays As TimeSpan
                spDays = Date.Now - Date.Now.AddMonths(-6)
                '初期値にする
                Me.HiddenMileScaleMonthCount.Value = MILE_SCALE_MONTH_MONTHCOUNT
                Me.HiddenMileScaleMonthCountDays.Value = spDays.Days.ToString(CultureInfo.CurrentCulture)
            End If

            'GBOOK表示件数のデータチェック
            If String.IsNullOrEmpty(telemaDisplayCount) Then
                '存在しない場合
                '「0」を固定で設定
                Me.HiddenTelemaDisplayCount.Value = "0"

            Else
                '上記以外の場合
                '取得値を設定
                Me.HiddenTelemaDisplayCount.Value = telemaDisplayCount

            End If

            'グラン表示日付区間
            Me.HiddenGraphStartDate.Value = String.Empty
            Me.HiddenGraphEndDate.Value = String.Empty

            'VINと発生日時初期化
            Me.HiddenVin.Value = String.Empty
            Me.HiddenOccurdate.Value = String.Empty

            'PopUp制御用
            Me.HiddenOrderListDisplayType.Value = POPUP_TYPE_NONE

            'コントロール更新
            Me.ContentUpdateButtonPanel.Update()

            'session情報を取得し格納する＆保持
            Dim sessionVclID As String = String.Empty

            Try
                'Seesion値を取得
                sessionVclID = CType(GetValue(ScreenPos.Current, SESSIONKEY_VCL_ID, False), String)

            Catch ex As Exception
                'Seesion値を取得できない場合

                'エラーログ
                Logger.Error(String.Format(CultureInfo.CurrentCulture _
                               , "{0}.{1} Error rowDmsCodeMap.CODE1=NOTHING" _
                               , Me.GetType.ToString _
                               , System.Reflection.MethodBase.GetCurrentMethod.Name))

                '予想外エラー発生メッセージを出す
                Me.ShowMessageBox(WordId.id902)

                Return

            End Try

            'VclIDを保持
            Me.HiddenVclID.Value = sessionVclID

            'コントロール更新
            Me.ContentUpdateButtonPanel.Update()

        End If

        'フッター設定
        Me.InitFooterButton()

    End Sub

#End Region

#Region "フッター関係"

#Region "フッターイベント"

    ''' <summary>
    ''' ハイライトフッター設定
    ''' </summary>
    ''' <param name="commonMaster">マスターページ</param>
    ''' <param name="category">カテゴリ</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Overrides Function DeclareCommonMasterFooter(commonMaster As CommonMasterPage, _
                        ByRef category As FooterMenuCategory) As Integer()

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} START" _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name))


        'スタッフ情報保持
        Dim staffInfo As StaffContext = StaffContext.Current

        '権限チェック
        If staffInfo.OpeCD = Operation.SA _
        OrElse staffInfo.OpeCD = Operation.SM _
        OrElse staffInfo.OpeCD = Operation.FM Then
            'SA,SM,FM権限の場合SMBフッターボタンがハイライトにする
            category = FooterMenuCategory.SMB
        Else
            'メインメニューフッターボタンがハイライトにする
            category = FooterMenuCategory.MainMenu
        End If

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} END" _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name))
        Return New Integer() {}

    End Function

    ''' <summary>
    ''' フッターボタンの初期化
    ''' </summary>
    ''' <remarks></remarks>
    ''' <hitory></hitory>
    Private Sub InitFooterButton()
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} START" _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name))

        'メインメニューボタンの設定
        Dim mainMenuButton As CommonMasterFooterButton = _
            CType(Me.Master, CommonMasterPage).GetFooterButton(FooterMenuCategory.MainMenu)
        If Not IsNothing(mainMenuButton) Then
            AddHandler mainMenuButton.Click, AddressOf MainMenuButton_Click
            mainMenuButton.OnClientClick = FOOTER_BUTTON_CONTROL_CALL
        End If

        '電話帳ボタンの設定
        Dim telDirectoryButton As CommonMasterFooterButton = _
            CType(Me.Master, CommonMasterPage).GetFooterButton(FooterMenuCategory.Contact)
        If Not IsNothing(telDirectoryButton) Then
            telDirectoryButton.OnClientClick = FOOTER_EVENT_TEL
        End If

        Dim inStaffInfo As StaffContext = StaffContext.Current

        '権限チェック
        If inStaffInfo.OpeCD = Operation.SA OrElse inStaffInfo.OpeCD = Operation.SM Then

            '顧客詳細ボタンの設定
            Dim customerButton As CommonMasterFooterButton = _
                CType(Me.Master, CommonMasterPage).GetFooterButton(FooterMenuCategory.CustomerDetail)
            If Not IsNothing(customerButton) Then
                customerButton.OnClientClick = "return false ;"
            End If

            'R/Oボタンの設定
            Dim roButton As CommonMasterFooterButton = _
                CType(Me.Master, CommonMasterPage).GetFooterButton(FooterMenuCategory.RepairOrderList)
            If Not IsNothing(roButton) Then
                AddHandler roButton.Click, AddressOf RoButton_Click
                roButton.OnClientClick = FOOTER_BUTTON_CONTROL_CALL
            End If

            '商品訴求ボタン
            Dim footerGoodsSolicitationContentsButton As CommonMasterFooterButton = _
            CType(Me.Master, CommonMasterPage).GetFooterButton(FooterMenuCategory.GoodsSolicitationContents)
            If Not IsNothing(footerGoodsSolicitationContentsButton) Then
                AddHandler footerGoodsSolicitationContentsButton.Click, AddressOf footerGoodsSolicitationContentsMenuButton_Click
                footerGoodsSolicitationContentsButton.OnClientClick = FOOTER_BUTTON_CONTROL_CALL
            End If

            'キャンペーンボタン
            Dim footerCampaignButton As CommonMasterFooterButton = _
            CType(Me.Master, CommonMasterPage).GetFooterButton(FooterMenuCategory.Campaign)
            If Not IsNothing(footerCampaignButton) Then
                AddHandler footerCampaignButton.Click, AddressOf footerCampaignMenuButton_Click
                footerCampaignButton.OnClientClick = FOOTER_BUTTON_CONTROL_CALL
            End If

            '来店管理ボタンの設定
            Dim addReserveManagement As CommonMasterFooterButton = _
                CType(Me.Master, CommonMasterPage).GetFooterButton(FooterMenuCategory.ReserveManagement)
            If Not IsNothing(addReserveManagement) Then
                AddHandler addReserveManagement.Click, AddressOf ReserveManagement_Click
                addReserveManagement.OnClientClick = FOOTER_BUTTON_CONTROL_CALL
            End If

            'SMBボタンの設定
            Dim smbButton As CommonMasterFooterButton = _
                CType(Me.Master, CommonMasterPage).GetFooterButton(FooterMenuCategory.SMB)
            If Not IsNothing(smbButton) Then
                AddHandler smbButton.Click, AddressOf SMBButton_Click
                smbButton.OnClientClick = FOOTER_BUTTON_CONTROL_CALL
            End If

            '権限チェック
        ElseIf inStaffInfo.OpeCD = Operation.CHT Then

            'TCメインボタンの設定
            Dim tcMainButton As CommonMasterFooterButton = _
                CType(Me.Master, CommonMasterPage).GetFooterButton(FooterMenuCategory.TechnicianMain)
            If Not IsNothing(tcMainButton) Then
                tcMainButton.OnClientClick = "return false ;"
            End If

            '2016/12/06 NSK 竹中 サブエリアのTCメインフッターのDisable対応 START
            tcMainButton.Enabled = False
            '2016/12/06 NSK 竹中 サブエリアのTCメインフッターのDisable対応 END

            'FMメインボタンの設定
            Dim fmMainButton As CommonMasterFooterButton = _
                CType(Me.Master, CommonMasterPage).GetFooterButton(FooterMenuCategory.ForemanMain)
            If Not IsNothing(fmMainButton) Then
                AddHandler fmMainButton.Click, AddressOf FormanMainButton_Click
                fmMainButton.OnClientClick = FOOTER_BUTTON_CONTROL_CALL
            End If

            'R/Oボタンの設定
            Dim roButton As CommonMasterFooterButton = _
                CType(Me.Master, CommonMasterPage).GetFooterButton(FooterMenuCategory.RepairOrderList)
            If Not IsNothing(roButton) Then
                AddHandler roButton.Click, AddressOf RoButton_Click
                roButton.OnClientClick = FOOTER_BUTTON_CONTROL_CALL
            End If

            '追加作業ボタンの設定
            Dim addWorkLisButton As CommonMasterFooterButton = _
                CType(Me.Master, CommonMasterPage).GetFooterButton(FooterMenuCategory.AddWorkList)
            If Not IsNothing(addWorkLisButton) Then
                AddHandler addWorkLisButton.Click, AddressOf AddListButton_Click
                addWorkLisButton.OnClientClick = FOOTER_BUTTON_CONTROL_CALL
            End If

            '権限チェック
        ElseIf inStaffInfo.OpeCD = Operation.FM Then

            'SMBボタンの設定
            Dim smbButton As CommonMasterFooterButton = _
                CType(Me.Master, CommonMasterPage).GetFooterButton(FooterMenuCategory.SMB)
            If Not IsNothing(smbButton) Then
                AddHandler smbButton.Click, AddressOf SMBButton_Click
                smbButton.OnClientClick = FOOTER_BUTTON_CONTROL_CALL
            End If

            'R/Oボタンの設定
            Dim roButton As CommonMasterFooterButton = _
                CType(Me.Master, CommonMasterPage).GetFooterButton(FooterMenuCategory.RepairOrderList)
            If Not IsNothing(roButton) Then
                AddHandler roButton.Click, AddressOf RoButton_Click
                roButton.OnClientClick = FOOTER_BUTTON_CONTROL_CALL
            End If

            '追加作業ボタンの設定
            Dim addWorkLisButton As CommonMasterFooterButton = _
                CType(Me.Master, CommonMasterPage).GetFooterButton(FooterMenuCategory.AddWorkList)
            If Not IsNothing(addWorkLisButton) Then
                AddHandler addWorkLisButton.Click, AddressOf AddListButton_Click
                addWorkLisButton.OnClientClick = FOOTER_BUTTON_CONTROL_CALL
            End If

            '権限チェック
        ElseIf inStaffInfo.OpeCD = Operation.CT Then

            'R/Oボタンの設定
            Dim roButton As CommonMasterFooterButton = _
                CType(Me.Master, CommonMasterPage).GetFooterButton(FooterMenuCategory.RepairOrderList)
            If Not IsNothing(roButton) Then
                AddHandler roButton.Click, AddressOf RoButton_Click
                roButton.OnClientClick = FOOTER_BUTTON_CONTROL_CALL
            End If

            '追加作業ボタンの設定
            Dim addWorkLisButton As CommonMasterFooterButton = _
                CType(Me.Master, CommonMasterPage).GetFooterButton(FooterMenuCategory.AddWorkList)
            If Not IsNothing(addWorkLisButton) Then
                AddHandler addWorkLisButton.Click, AddressOf AddListButton_Click
                addWorkLisButton.OnClientClick = FOOTER_BUTTON_CONTROL_CALL
            End If

        End If

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} END" _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name))
    End Sub

    ''' <summary>
    ''' メインメニューボタンを押した時の処理
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    ''' <hitory></hitory>
    Private Sub MainMenuButton_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} START" _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name))

        Dim staffInfo As StaffContext = StaffContext.Current

        '権限によって遷移先を変える
        If staffInfo.OpeCD = Operation.SA Then
            'メインメニュー(SA)に遷移する
            Me.RedirectNextScreen(MAINMENU_ID_SA)

        ElseIf staffInfo.OpeCD = Operation.SM Then
            '全体管理画面に遷移する
            Me.RedirectNextScreen(PROGRAM_ID_ALL_MANAGMENT)

        ElseIf staffInfo.OpeCD = Operation.CT Then
            'メインメニュー(CT)に遷移する
            Me.RedirectNextScreen(PROCESS_CONTROL_PAGE)

        ElseIf staffInfo.OpeCD = Operation.FM Then
            'メインメニュー(FM)に遷移する
            Me.RedirectNextScreen(MAINMENU_ID_FM)
        ElseIf staffInfo.OpeCD = Operation.CHT Then
            '工程管理に遷移する
            Me.RedirectNextScreen(PROCESS_CONTROL_PAGE)

        End If

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} END" _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name))
    End Sub

    ''' <summary>
    ''' SMBボタンを押した時の処理
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    ''' <hitory></hitory>
    Private Sub SMBButton_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} START" _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name))

        '工程管理画面に遷移する
        Me.RedirectNextScreen(PROCESS_CONTROL_PAGE)

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} END" _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name))
    End Sub

    ''' <summary>
    ''' R/O一覧ボタンを押した時の処理
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    ''' <hitory></hitory>
    Private Sub RoButton_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} START" _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name))

        'ログインスタッフ情報取得
        Dim staffInfo As StaffContext = StaffContext.Current

        Using biz As New SC3240601BusinessLogic

            '基幹コードへ変換処理
            Dim rowDmsCodeMap As ServiceCommonClassDataSet.DmsCodeMapRow = biz.ChangeDmsCode(staffInfo)

            '基幹販売店コードチェック
            If String.Empty.Equals(rowDmsCodeMap.CODE1) Then
                '値無し

                'エラーログ
                Logger.Info(String.Format(CultureInfo.CurrentCulture _
                               , "{0}.{1} Error rowDmsCodeMap.CODE1=NOTHING" _
                               , Me.GetType.ToString _
                               , System.Reflection.MethodBase.GetCurrentMethod.Name))

                '予想外エラー発生メッセージを出す
                Me.ShowMessageBox(WordId.id902)

                '処理終了
                Exit Sub

            End If

            '基幹店舗コードチェック
            If String.Empty.Equals(rowDmsCodeMap.CODE2) Then
                '値無し

                'エラーログ
                Logger.Info(String.Format(CultureInfo.CurrentCulture _
                               , "{0}.{1} Err:rowDmsCodeMap.CODE2=NOTHING" _
                               , Me.GetType.ToString _
                               , System.Reflection.MethodBase.GetCurrentMethod.Name))

                '予想外エラー発生メッセージを出す
                Me.ShowMessageBox(WordId.id902)

                '処理終了
                Exit Sub

            End If

            '基幹アカウントチェック
            If String.Empty.Equals(rowDmsCodeMap.ACCOUNT) Then
                '値無し

                'エラーログ
                Logger.Info(String.Format(CultureInfo.CurrentCulture _
                               , "{0}.{1} Error rowDmsCodeMap.ACCOUNT=NOTHING" _
                               , Me.GetType.ToString _
                               , System.Reflection.MethodBase.GetCurrentMethod.Name))

                '予想外エラー発生メッセージを出す
                Me.ShowMessageBox(WordId.id902)

                '処理終了
                Exit Sub

            End If


            'セション値の設定
            'DMS用販売店コード
            Me.SetValue(ScreenPos.Next, SESSIONKEY_DEARLER_CODE, rowDmsCodeMap.CODE1)

            'DMS用店舗コード
            Me.SetValue(ScreenPos.Next, SESSIONKEY_BRANCH_CODE, rowDmsCodeMap.CODE2)

            'ログインユーザアカウント
            Me.SetValue(ScreenPos.Next, SESSIONKEY_LOGIN_USER_ID, rowDmsCodeMap.ACCOUNT)

            '来店実績連番
            Me.SetValue(ScreenPos.Next, SESSIONKEY_SA_CHIP_ID, "")

            'DMS予約ID
            Me.SetValue(ScreenPos.Next, SESSIONKEY_BASREZID, "")

            'RO番号
            Me.SetValue(ScreenPos.Next, SESSIONKEY_R_O, "")

            'RO作業連番
            Me.SetValue(ScreenPos.Next, SESSIONKEY_SEQ_NO, "")

            '車両登録NOのVIN
            Me.SetValue(ScreenPos.Next, SESSIONKEY_VIN_NO, "")

            '編集フラグ
            Me.SetValue(ScreenPos.Next, SESSIONKEY_VIEW_MODE, SESSIONVALUE_EDIT)

            '画面番号(RO一覧)
            Me.SetValue(ScreenPos.Next, SESSIONKEY_DISP_NUM, SESSIONVALUE_RO_LIST)

        End Using

        '他システム連携画面に遷移
        Me.RedirectNextScreen(OTHER_LINKAGE_PAGE)

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} END" _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name))
    End Sub

    ''' <summary>
    ''' 追加作業一覧ボタンを押した時の処理
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    ''' <hitory></hitory>
    Private Sub AddListButton_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} START" _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name))

        'スタッフ情報取得
        Dim staffInfo As StaffContext = StaffContext.Current

        Using biz As New SC3240601BusinessLogic

            Try
                '基幹コードへ変換処理
                Dim rowDmsCodeMap As ServiceCommonClassDataSet.DmsCodeMapRow = biz.ChangeDmsCode(staffInfo)

                '基幹販売店コードチェック
                If String.Empty.Equals(rowDmsCodeMap.CODE1) Then
                    '値無し

                    'エラーログ
                    Logger.Info(String.Format(CultureInfo.CurrentCulture _
                                   , "{0}.{1} Error rowDmsCodeMap.CODE1=NOTHING" _
                                   , Me.GetType.ToString _
                                   , System.Reflection.MethodBase.GetCurrentMethod.Name))

                    '予想外エラー発生メッセージを出す
                    Me.ShowMessageBox(WordId.id902)

                    '処理終了
                    Exit Sub

                End If

                '基幹店舗コードチェック
                If String.Empty.Equals(rowDmsCodeMap.CODE2) Then
                    '値無し

                    'エラーログ
                    Logger.Info(String.Format(CultureInfo.CurrentCulture _
                                   , "{0}.{1} Error rowDmsCodeMap.CODE2=NOTHING" _
                                   , Me.GetType.ToString _
                                   , System.Reflection.MethodBase.GetCurrentMethod.Name))

                    '予想外エラー発生メッセージを出す
                    Me.ShowMessageBox(WordId.id902)

                    '処理終了
                    Exit Sub

                End If

                '基幹アカウントチェック
                If String.Empty.Equals(rowDmsCodeMap.ACCOUNT) Then
                    '値無し

                    'エラーログ
                    Logger.Info(String.Format(CultureInfo.CurrentCulture _
                                   , "{0}.{1} Error rowDmsCodeMap.ACCOUNT=NOTHING" _
                                   , Me.GetType.ToString _
                                   , System.Reflection.MethodBase.GetCurrentMethod.Name))

                    '予想外エラー発生メッセージを出す
                    Me.ShowMessageBox(WordId.id902)

                    '処理終了
                    Exit Sub

                End If

                'DMS情報のチェック
                If Not (IsNothing(rowDmsCodeMap)) Then
                    '取得できた場合
                    '画面間パラメータを設定
                    '表示番号
                    Me.SetValue(ScreenPos.Next, SESSIONKEY_DISP_NUM, SESSION_DATA_DISP_NUM_ADD_LIST)

                    'DMS販売店コード
                    Me.SetValue(ScreenPos.Next, SESSIONKEY_DEARLER_CODE, rowDmsCodeMap.CODE1)

                    'DMS店舗コード
                    Me.SetValue(ScreenPos.Next, SESSIONKEY_BRANCH_CODE, rowDmsCodeMap.CODE2)

                    'アカウント
                    Me.SetValue(ScreenPos.Next, SESSIONKEY_LOGIN_USER_ID, rowDmsCodeMap.ACCOUNT)

                    '来店実績連番
                    Me.SetValue(ScreenPos.Next, SESSIONKEY_SA_CHIP_ID, String.Empty)

                    'DMS予約ID
                    Me.SetValue(ScreenPos.Next, SESSIONKEY_BASREZID, String.Empty)

                    'RO番号
                    Me.SetValue(ScreenPos.Next, SESSIONKEY_R_O, String.Empty)

                    'RO作業連番
                    Me.SetValue(ScreenPos.Next, SESSIONKEY_SEQ_NO, String.Empty)

                    'VIN
                    Me.SetValue(ScreenPos.Next, SESSIONKEY_VIN_NO, String.Empty)

                    '編集モード
                    Me.SetValue(ScreenPos.Next, SESSIONKEY_VIEW_MODE, SESSIONVALUE_EDIT)

                    '追加作業画面(枠)に遷移する
                    Me.RedirectNextScreen(OTHER_LINKAGE_PAGE)

                Else
                    '取得できなかった場合
                    'エラー
                    Logger.Info(String.Format(CultureInfo.CurrentCulture _
                                             , "{0}.{1} Error DMS information nofound " _
                                             , Me.GetType.ToString _
                                             , System.Reflection.MethodBase.GetCurrentMethod.Name))
                    '予想外エラー発生メッセージを出す
                    Me.ShowMessageBox(WordId.id902)
                End If

            Catch ex As OracleExceptionEx When ex.Number = 1013
                'ORACLEのタイムアウト処理
                Logger.Error(String.Format(CultureInfo.CurrentCulture _
                                         , "{0}.{1} DB TIMEOUT:{2}" _
                                         , Me.GetType.ToString _
                                         , System.Reflection.MethodBase.GetCurrentMethod.Name _
                                         , ex.Message))

                'DBタイムアウトのメッセージ表示
                Me.ShowMessageBox(WordId.id901)

            End Try

        End Using

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} END" _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name))
    End Sub

    ''' <summary>
    ''' FMメインボタンを押した時の処理
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    ''' <hitory></hitory>
    Private Sub FormanMainButton_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} START" _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name))

        'メインメニュー(FM)画面に遷移する
        Me.RedirectNextScreen(MAINMENU_ID_FM)

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} END" _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name))
    End Sub

    ''' <summary>
    ''' 来店管理ボタンを押した時の処理
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    ''' <hitory></hitory>
    Private Sub ReserveManagement_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} START" _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name))

        '来店管理画面に遷移する
        Me.RedirectNextScreen(APPLICATIONID_VSTMANAGER)

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} END" _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name))
    End Sub

    ''' <summary>
    ''' 商品訴求コンテンツボタンタップイベント初期化
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub InitGoodsSolicitationContentsButtonEvent()

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                                , "{0}.{1} START" _
                                , Me.GetType.ToString _
                                , System.Reflection.MethodBase.GetCurrentMethod.Name))

        'スタッフ情報保持
        Dim staffInfo As StaffContext = StaffContext.Current

        'SA、SMの場合、商品訴求ボタンのイベントを登録する
        If staffInfo.OpeCD = iCROP.BizLogic.Operation.SA _
            OrElse staffInfo.OpeCD = iCROP.BizLogic.Operation.SM Then

            '商品訴求ボタン
            Dim footerGoodsSolicitationContentsButton As CommonMasterFooterButton = _
            CType(Me.Master, CommonMasterPage).GetFooterButton(FooterMenuCategory.GoodsSolicitationContents)
            'イベントをbindする
            AddHandler footerGoodsSolicitationContentsButton.Click, AddressOf footerGoodsSolicitationContentsMenuButton_Click
            footerGoodsSolicitationContentsButton.OnClientClick = FOOTER_BUTTON_CONTROL_CALL

        End If

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                                , "{0}.{1} END" _
                                , Me.GetType.ToString _
                                , System.Reflection.MethodBase.GetCurrentMethod.Name))

    End Sub

    ''' <summary>
    ''' キャンペーンボタンタップイベント初期化
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub InitCampaignButtonEvent()

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                                , "{0}.{1} START" _
                                , Me.GetType.ToString _
                                , System.Reflection.MethodBase.GetCurrentMethod.Name))

        'スタッフ情報保持
        Dim staffInfo As StaffContext = StaffContext.Current

        'SA、SMの場合、キャンペーンボタンタップすると、現地のキャンペーン画面に遷移
        If staffInfo.OpeCD = Operation.SA _
            OrElse staffInfo.OpeCD = Operation.SM Then

            'キャンペーンボタン
            Dim footerCampaignButton As CommonMasterFooterButton = _
            CType(Me.Master, CommonMasterPage).GetFooterButton(FooterMenuCategory.Campaign)

            'メインボタンのClientとサーバ両方のクリックイベントをbind
            AddHandler footerCampaignButton.Click, AddressOf footerCampaignMenuButton_Click
            footerCampaignButton.OnClientClick = FOOTER_BUTTON_CONTROL_CALL

        End If

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                                , "{0}.{1} END" _
                                , Me.GetType.ToString _
                                , System.Reflection.MethodBase.GetCurrentMethod.Name))

    End Sub

    ''' <summary>
    ''' 商品訴求コンテンツボタンタップイベント
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub footerGoodsSolicitationContentsMenuButton_Click(sender As Object, e As CommonMasterFooterButtonClickEventArgs)

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                                , "{0}.{1} START" _
                                , Me.GetType.ToString _
                                , System.Reflection.MethodBase.GetCurrentMethod.Name))

        'セション値の設定
        'DMS用販売店コード
        Me.SetValue(ScreenPos.Next, SESSIONKEY_TONEC_DEALERCODE, ONE_SPACE)

        'DMS用店舗コード
        Me.SetValue(ScreenPos.Next, SESSIONKEY_TONEC_BRANCHCODE, ONE_SPACE)

        'ログインユーザアカウント
        Me.SetValue(ScreenPos.Next, SESSIONKEY_TONEC_LOGINUSERID, ONE_SPACE)

        '来店実績連番
        Me.SetValue(ScreenPos.Next, SESSIONKEY_TONEC_SACHIPID, "")

        'DMS予約ID
        Me.SetValue(ScreenPos.Next, SESSIONKEY_TONEC_BASREZID, "")

        'RO番号
        Me.SetValue(ScreenPos.Next, SESSIONKEY_TONEC_RO, "")

        'RO作業連番
        Me.SetValue(ScreenPos.Next, SESSIONKEY_TONEC_SEQ_NO, "")

        '車両登録NOのVIN
        Me.SetValue(ScreenPos.Next, SESSIONKEY_TONEC_VIN_NO, "")

        '編集フラグ
        Me.SetValue(ScreenPos.Next, SESSIONKEY_TONEC_VIEWMODE, SESSIONVALUE_READ)

        '商品訴求コンテンツ画面に遷移
        Me.RedirectNextScreen(PGMID_GOOD_SOLICITATION_CONTENTS)

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                                , "{0}.{1} END" _
                                , Me.GetType.ToString _
                                , System.Reflection.MethodBase.GetCurrentMethod.Name))

    End Sub

    ''' <summary>
    ''' キャンペーンボタンタップイベント
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub footerCampaignMenuButton_Click(sender As Object, e As CommonMasterFooterButtonClickEventArgs)

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                                , "{0}.{1} START" _
                                , Me.GetType.ToString _
                                , System.Reflection.MethodBase.GetCurrentMethod.Name))

        'ログインスタッフ情報取得
        Dim staffInfo As StaffContext = StaffContext.Current

        Using biz As New SC3240601BusinessLogic

            '基幹コードへ変換処理
            Dim rowDmsCodeMap As ServiceCommonClassDataSet.DmsCodeMapRow = biz.ChangeDmsCode(staffInfo)

            '基幹販売店コードチェック
            If String.Empty.Equals(rowDmsCodeMap.CODE1) Then
                '値無し

                'エラーログ
                Logger.Info(String.Format(CultureInfo.CurrentCulture _
                               , "{0}.{1} Error rowDmsCodeMap.CODE1=NOTHING" _
                               , Me.GetType.ToString _
                               , System.Reflection.MethodBase.GetCurrentMethod.Name))

                '予想外エラー発生メッセージを出す
                Me.ShowMessageBox(WordId.id902)

                '処理終了
                Exit Sub

            End If

            '基幹店舗コードチェック
            If String.Empty.Equals(rowDmsCodeMap.CODE2) Then
                '値無し

                'エラーログ
                Logger.Info(String.Format(CultureInfo.CurrentCulture _
                               , "{0}.{1} Error rowDmsCodeMap.CODE2=NOTHING" _
                               , Me.GetType.ToString _
                               , System.Reflection.MethodBase.GetCurrentMethod.Name))

                '予想外エラー発生メッセージを出す
                Me.ShowMessageBox(WordId.id902)

                '処理終了
                Exit Sub

            End If

            '基幹アカウントチェック
            If String.Empty.Equals(rowDmsCodeMap.ACCOUNT) Then
                '値無し

                'エラーログ
                Logger.Info(String.Format(CultureInfo.CurrentCulture _
                               , "{0}.{1} Error rowDmsCodeMap.ACCOUNT=NOTHING" _
                               , Me.GetType.ToString _
                               , System.Reflection.MethodBase.GetCurrentMethod.Name))

                '予想外エラー発生メッセージを出す
                Me.ShowMessageBox(WordId.id902)

                '処理終了
                Exit Sub

            End If


            'セション値の設定
            'DMS用販売店コード
            Me.SetValue(ScreenPos.Next, SESSIONKEY_DEARLER_CODE, rowDmsCodeMap.CODE1)

            'DMS用店舗コード
            Me.SetValue(ScreenPos.Next, SESSIONKEY_BRANCH_CODE, rowDmsCodeMap.CODE2)

            'ログインユーザアカウント
            Me.SetValue(ScreenPos.Next, SESSIONKEY_LOGIN_USER_ID, rowDmsCodeMap.ACCOUNT)

            '来店実績連番
            Me.SetValue(ScreenPos.Next, SESSIONKEY_SA_CHIP_ID, "")

            'DMS予約ID
            Me.SetValue(ScreenPos.Next, SESSIONKEY_BASREZID, "")

            'RO番号
            Me.SetValue(ScreenPos.Next, SESSIONKEY_R_O, "")

            'RO作業連番
            Me.SetValue(ScreenPos.Next, SESSIONKEY_SEQ_NO, "")

            '車両登録NOのVIN
            Me.SetValue(ScreenPos.Next, SESSIONKEY_VIN_NO, "")

            '編集フラグ
            Me.SetValue(ScreenPos.Next, SESSIONKEY_VIEW_MODE, SESSIONVALUE_READ)

            '画面番号(キャンペーン)
            Me.SetValue(ScreenPos.Next, SESSIONKEY_DISP_NUM, SESSIONVALUE_CAMPAIGN)

        End Using

        '他システム連携画面に遷移
        Me.RedirectNextScreen(OTHER_LINKAGE_PAGE)

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                                , "{0}.{1} END" _
                                , Me.GetType.ToString _
                                , System.Reflection.MethodBase.GetCurrentMethod.Name))

    End Sub

#End Region

#End Region

#Region "イベント"

    ''' <summary>
    ''' 初期表示用
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    ''' <hitory></hitory>
    Protected Sub MainAreaReload_Click(sender As Object, e As System.EventArgs) Handles MainAreaReload.Click
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} START" _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name))

        '画面初期表示データを作成
        Me.SetMileageData(1, CType(Me.HiddenLoadCount.Value, Long), 0, 0)
        '最新データをグラフに反映する
        ScriptManager.RegisterStartupScript(Me, Me.GetType, "UpdateChartData", "UpdateChartData();", True)

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} END" _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name))
    End Sub

    ''' <summary>
    ''' グラフPreYearボタン処理
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    ''' <hitory></hitory>
    Protected Sub GraphPreYearButton_Click(sender As Object, e As System.EventArgs) Handles GraphPreYearButton.Click
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} START" _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name))


        If CONST_FALSE.Equals(Me.HiddenGraphPreButtonEnable.Value.ToString(CultureInfo.CurrentCulture)) Then
            '去年ボタン非活性の場合、処理終了にする
            Logger.Info(String.Format(CultureInfo.CurrentCulture _
                        , "{0}.{1} Button Enable False END" _
                        , Me.GetType.ToString _
                        , System.Reflection.MethodBase.GetCurrentMethod.Name))
            Return
        End If

        '去年のグラフデータをセットする
        Me.SetGraphPreBtn()
        '最新データをグラフに反映する
        ScriptManager.RegisterStartupScript(Me, Me.GetType, "UpdateChartData", "UpdateChartData();", True)

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} END" _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name))
    End Sub

    ''' <summary>
    ''' グラフNextYearボタン処理
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    ''' <hitory></hitory>
    Protected Sub GraphNextYearButton_Click(sender As Object, e As System.EventArgs) Handles GraphNextYearButton.Click
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} START" _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name))

        If CONST_FALSE.Equals(Me.HiddenGraphNextButtonEnable.Value.ToString(CultureInfo.CurrentCulture)) Then
            '来年ボタン非活性の場合、処理終了にする
            Logger.Info(String.Format(CultureInfo.CurrentCulture _
                        , "{0}.{1} Button Enable False END" _
                        , Me.GetType.ToString _
                        , System.Reflection.MethodBase.GetCurrentMethod.Name))
            Return
        End If

        '来年のグラフデータをセットする
        Me.SetGraphNextBtn()
        '最新データをグラフに反映する
        ScriptManager.RegisterStartupScript(Me, Me.GetType, "UpdateChartData", "UpdateChartData();", True)

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} END" _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name))
    End Sub

    ''' <summary>
    ''' 前件読み込み
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    ''' <hitory></hitory>
    Protected Sub BackPageButton_Click(sender As Object, e As System.EventArgs) Handles BackPageButton.Click
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} START" _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name))

        '検索開始Index
        Dim searchStartIndex As Long
        '検索終了Index
        Dim searchEndIndex As Long

        '現在のページ情報を取得する
        Dim startIndex As Long = CType(Me.HiddenStartIndex.Value, Long)
        Dim endIndex As Long = CType(Me.HiddenEndIndex.Value, Long)
        Dim loadCount As Long = CType(Me.HiddenLoadCount.Value, Long)
        Dim maxDisplayCount As Long = CType(Me.HiddenMaxDisplayCount.Value, Long)

        ' 開始行の設定
        Dim setStartMin As Long = startIndex - loadCount
        If setStartMin <= 0 Then
            '計算結果０以下の場合、検索開始Indexは初期１とする
            searchStartIndex = 1
        Else
            '他の場合、検索開始Indexに設定する
            searchStartIndex = setStartMin
        End If
        ' 終了行の設定
        Dim setEndMin As Long = endIndex - searchStartIndex + 1
        If setEndMin < maxDisplayCount Then
            '計算結果は最大表示件数を超えていなかったら、検索終了Indexは現ページの終了Indexとする
            searchEndIndex = endIndex
        Else
            '他の場合、「検索開始Index＋最大表示件数-１」に設定する
            searchEndIndex = searchStartIndex + maxDisplayCount - 1
        End If

        '前件を表示させる
        Me.SetMileageData(searchStartIndex, searchEndIndex, 1, 0)

        'スクロール一件目に確かに停止するため
        Me.HiddenScrollPosition.Value = "0"

        '計算結果更新
        Me.ContentUpdateButtonPanel.Update()

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} END" _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name))
    End Sub

    ''' <summary>
    ''' 次件読み込み
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    ''' <hitory></hitory>
    Protected Sub NextPageButton_Click(sender As Object, e As System.EventArgs) Handles NextPageButton.Click
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} START" _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name))

        '検索開始Index
        Dim searchStartIndex As Long
        '検索終了Index
        Dim searchEndIndex As Long

        '現在のページ情報を取得する
        Dim startIndex As Long = CType(Me.HiddenStartIndex.Value, Long)
        Dim endIndex As Long = CType(Me.HiddenEndIndex.Value, Long)
        Dim loadCount As Long = CType(Me.HiddenLoadCount.Value, Long)
        Dim maxDisplayCount As Long = CType(Me.HiddenMaxDisplayCount.Value, Long)

        ' 終了行の設定
        searchEndIndex = endIndex + loadCount

        ' 開始行の設定
        Dim setStartMax As Long = searchEndIndex - startIndex + 1
        If setStartMax <= maxDisplayCount Then
            '検索必要件数が、最大表示件数以内の場合、検索開始Indexに現ページ開始Indexを設定する
            searchStartIndex = startIndex
        Else
            '多の場合、検索開始Indexを計算する
            searchStartIndex = searchEndIndex - maxDisplayCount + 1

            If searchStartIndex <= 0 Then
                '計算結果０以下の場合、検索開始Indexは初期１とする
                searchStartIndex = 1
            End If
        End If

        '次件を表示させる
        '詳細情報占める総行計算用件数を返却（一件情報に対して2行詳細情報以内の場合0を返却）
        Dim offsetDetailCaleCount = Me.SetMileageData(searchStartIndex, searchEndIndex, endIndex, 2)

        '表示位置計算件数を算出
        Dim offsetPoisition As Long
        If endIndex >= searchStartIndex Then

            offsetPoisition = endIndex - searchStartIndex + 1 + offsetDetailCaleCount

        Else

            offsetPoisition = 0

        End If

        If 1 < searchStartIndex Then
            '開始Indexが一件目ではない場合、「前件読込」が占める行高さをプラス
            Me.HiddenScrollPosition.Value = _
                       (offsetPoisition * 45 + 46).ToString(CultureInfo.CurrentCulture)
        Else
            '開始Indexが一件目の場合
            Me.HiddenScrollPosition.Value = _
                       (offsetPoisition * 45).ToString(CultureInfo.CurrentCulture)
        End If

        '計算結果更新
        Me.ContentUpdateButtonPanel.Update()

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} END" _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name))
    End Sub

#End Region

#Region "グラフボタン処理"

    ''' <summary>
    ''' グラフ去年ボタン処理
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub SetGraphPreBtn()
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} START" _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name))

        '検索終了日付
        Dim endDate As Date = CType(Me.HiddenGraphStartDate.Value, Date)
        '検索開始日付
        Dim startDate As Date = endDate.AddYears(-1)
        '発生日時
        Dim Occurdate As Date = CType(Me.HiddenOccurdate.Value, Date)
        'VIN
        Dim Vin As String = CType(Me.HiddenVin.Value, String)
        '車両ID
        Dim VclId As Decimal = CType(Me.HiddenVclID.Value, Decimal)
        'オーナーズID
        Dim OwnersId As String = CType(Me.HiddenOwnerID.Value, String)

        Dim dtGraphRows As SC3240601TelemaInfoDataTable
        Using biz As New SC3240601BusinessLogic

            Try
                'グラフ情報取得
                dtGraphRows = biz.GetMileageGraph(VclId, Vin, OwnersId, Occurdate, startDate, endDate)

                If IsNothing(dtGraphRows) Then
                    'Nothingの場合終了にする
                    Logger.Info(String.Format(CultureInfo.CurrentCulture _
                                , "{0}.{1} Error GraphInfo Nothing END" _
                                , Me.GetType.ToString _
                                , System.Reflection.MethodBase.GetCurrentMethod.Name))

                    '予想外エラー発生メッセージを出す
                    Me.ShowMessageBox(WordId.id902)

                    Return
                End If

                If (dtGraphRows.Rows.Count > 0) Then
                    '取得情報がある場合

                    'グラフデータを作成する
                    Me.SetGraphData(dtGraphRows, startDate, endDate)

                    Dim dtGraphPreYearRows As SC3240601TelemaInfoDataTable
                    Dim startPreYearDate As Date = startDate.AddYears(-1)
                    Dim endPreYearDate As Date = startDate
                    'PreYearボタン制御するため、去年のグラフデータを検索
                    dtGraphPreYearRows = biz.GetMileageGraph(VclId, Vin, OwnersId, Occurdate, startPreYearDate, endPreYearDate)

                    If IsNothing(dtGraphPreYearRows) Then
                        'Nothingの場合終了にする
                        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                                    , "{0}.{1} Error BackYear GraphInfo Nothing END" _
                                    , Me.GetType.ToString _
                                    , System.Reflection.MethodBase.GetCurrentMethod.Name))

                        '予想外エラー発生メッセージを出す
                        Me.ShowMessageBox(WordId.id902)

                        Return
                    End If

                    If (dtGraphPreYearRows.Rows.Count > 0) Then
                        '取得情報がある場合、PreYearボタン活性にする
                        ScriptManager.RegisterStartupScript(Me, Me.GetType, "SetGraphPreYearButtonEnable", "SetGraphPreYearButtonEnable(1);", True)
                        Me.HiddenGraphPreButtonEnable.Value = CONST_TRUE
                    Else
                        '取得情報がある場合、PreYearボタン非活性にする
                        ScriptManager.RegisterStartupScript(Me, Me.GetType, "SetGraphPreYearButtonEnable", "SetGraphPreYearButtonEnable(0);", True)
                        Me.HiddenGraphPreButtonEnable.Value = CONST_FALSE
                    End If

                    'Next Yearボタン活性にする
                    ScriptManager.RegisterStartupScript(Me, Me.GetType, "SetGraphNextYearButtonEnable", "SetGraphNextYearButtonEnable(1);", True)
                    Me.HiddenGraphNextButtonEnable.Value = CONST_TRUE

                Else
                    '取得情報がない場合、ボタン制御のため、Pre Yearボタン非活性にする
                    ScriptManager.RegisterStartupScript(Me, Me.GetType, "SetGraphPreYearButtonEnable", "SetGraphPreYearButtonEnable(0);", True)
                    Me.HiddenGraphPreButtonEnable.Value = CONST_FALSE

                    Logger.Info(String.Format(CultureInfo.CurrentCulture _
                                , "{0}.{1} END" _
                                , Me.GetType.ToString _
                                , System.Reflection.MethodBase.GetCurrentMethod.Name))
                    Return
                End If

            Catch ex As OracleExceptionEx When ex.Number = 1013
                'ORACLEのタイムアウト処理
                Logger.Error(String.Format(CultureInfo.CurrentCulture _
                                         , "{0}.{1} DB TIMEOUT:{2}" _
                                         , Me.GetType.ToString _
                                         , System.Reflection.MethodBase.GetCurrentMethod.Name _
                                         , ex.Message))

                'DBタイムアウトのメッセージ表示
                Me.ShowMessageBox(WordId.id901)

                Return
            End Try

        End Using

        'グラフ表示区間、開始日付を保持
        Me.HiddenGraphStartDate.Value = startDate.ToString(CultureInfo.CurrentCulture)
        'グラフ表示区間、終了日付を保持
        Me.HiddenGraphEndDate.Value = endDate.ToString(CultureInfo.CurrentCulture)

        'エリア更新
        Me.ContentUpdateButtonPanel.Update()

        'グラフスケール初期化
        ScriptManager.RegisterStartupScript(Me, Me.GetType, "zoomChart", "zoomChart();", True)

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} END" _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name))
    End Sub

    ''' <summary>
    ''' グラフ来年ボタン処理
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub SetGraphNextBtn()
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} START" _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name))

        '検索開始日付
        Dim startDate As Date = CType(Me.HiddenGraphEndDate.Value, Date)
        '検索終了日付
        Dim endDate As Date = startDate.AddYears(+1)
        '発生日時
        Dim Occurdate As Date = CType(Me.HiddenOccurdate.Value, Date)
        'VIN
        Dim Vin As String = CType(Me.HiddenVin.Value, String)
        '車両ID
        Dim VclId As Decimal = CType(Me.HiddenVclID.Value, Decimal)
        'オーナーズID
        Dim OwnersId As String = CType(Me.HiddenOwnerID.Value, String)

        Dim dtGraphRows As SC3240601TelemaInfoDataTable
        Using biz As New SC3240601BusinessLogic

            Try
                'グラフ情報取得
                dtGraphRows = biz.GetMileageGraph(VclId, Vin, OwnersId, Occurdate, startDate, endDate)

                If IsNothing(dtGraphRows) Then
                    'Nothingの場合終了にする

                    Logger.Info(String.Format(CultureInfo.CurrentCulture _
                                , "{0}.{1} Error GraphInfo Nothing END" _
                                , Me.GetType.ToString _
                                , System.Reflection.MethodBase.GetCurrentMethod.Name))

                    '予想外エラー発生メッセージを出す
                    Me.ShowMessageBox(WordId.id902)

                    Return
                End If

                If (dtGraphRows.Count > 0) Then
                    '取得情報がある場合

                    'グラフデータを作成する
                    Me.SetGraphData(dtGraphRows, startDate, endDate)

                    Dim dtGraphNextYearRows As SC3240601TelemaInfoDataTable
                    Dim startNextYearDate As Date = endDate
                    '一年間後の日付
                    Dim endNextYearDate As Date = endDate.AddYears(+1)
                    'NextYearボタン制御するため、去年のグラフデータを検索
                    dtGraphNextYearRows = biz.GetMileageGraph(VclId, Vin, OwnersId, Occurdate, startNextYearDate, endNextYearDate)

                    If IsNothing(dtGraphNextYearRows) Then
                        'Nothingの場合終了にする

                        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                                    , "{0}.{1} Error NextYear GraphInfo Nothing END" _
                                    , Me.GetType.ToString _
                                    , System.Reflection.MethodBase.GetCurrentMethod.Name))

                        '予想外エラー発生メッセージを出す
                        Me.ShowMessageBox(WordId.id902)

                        Return
                    End If

                    If (dtGraphNextYearRows.Rows.Count > 0) Then
                        '取得情報がある場合、NextYearボタン活性にする
                        ScriptManager.RegisterStartupScript(Me, Me.GetType, "SetGraphNextYearButtonEnable", "SetGraphNextYearButtonEnable(1);", True)
                        Me.HiddenGraphNextButtonEnable.Value = CONST_TRUE
                    Else
                        '取得情報がある場合、NextYearボタン非活性にする
                        ScriptManager.RegisterStartupScript(Me, Me.GetType, "SetGraphNextYearButtonEnable", "SetGraphNextYearButtonEnable(0);", True)
                        Me.HiddenGraphNextButtonEnable.Value = CONST_FALSE
                    End If

                    'Pre Yearボタン活性にする
                    ScriptManager.RegisterStartupScript(Me, Me.GetType, "SetGraphPreYearButtonEnable", "SetGraphPreYearButtonEnable(1);", True)
                    Me.HiddenGraphPreButtonEnable.Value = CONST_TRUE

                Else
                    '取得情報がない場合、ボタン制御のため、NextYearボタン非活性にする
                    ScriptManager.RegisterStartupScript(Me, Me.GetType, "SetGraphNextYearButtonEnable", "SetGraphNextYearButtonEnable(0);", True)
                    Me.HiddenGraphNextButtonEnable.Value = CONST_FALSE

                    Logger.Info(String.Format(CultureInfo.CurrentCulture _
                                , "{0}.{1} GraphInfo Nofound END" _
                                , Me.GetType.ToString _
                                , System.Reflection.MethodBase.GetCurrentMethod.Name))
                    Return
                End If

            Catch ex As OracleExceptionEx When ex.Number = 1013
                'ORACLEのタイムアウト処理
                Logger.Error(String.Format(CultureInfo.CurrentCulture _
                                         , "{0}.{1} DB TIMEOUT:{2}" _
                                         , Me.GetType.ToString _
                                         , System.Reflection.MethodBase.GetCurrentMethod.Name _
                                         , ex.Message))

                'DBタイムアウトのメッセージ表示
                Me.ShowMessageBox(WordId.id901)

                Return
            End Try

        End Using

        'グラフ表示区間、開始日付を保持
        Me.HiddenGraphStartDate.Value = startDate.ToString(CultureInfo.CurrentCulture)
        'グラフ表示区間、終了日付を保持
        Me.HiddenGraphEndDate.Value = endDate.ToString(CultureInfo.CurrentCulture)

        'エリア更新
        Me.ContentUpdateButtonPanel.Update()

        'グラフスケール初期化
        ScriptManager.RegisterStartupScript(Me, Me.GetType, "zoomChart", "zoomChart();", True)

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} END" _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name))
    End Sub

#End Region

#Region "走行距離履歴画面表示処理"

    ''' <summary>
    ''' 画面情報出力処理（所有者情報・走行距離履歴一覧・初期グラフエリア出力）
    ''' </summary>
    ''' <param name="inStartIndex">検索開始Index</param>
    ''' <param name="inEndIndex">検索終了Index</param>
    ''' <param name="inPreEndIndex">前ページの終了Index、「0」の場合は加算されない</param>
    ''' <param name="inCallID">コールID：0、初期コール；1、前件読み込み；2、次件読込</param>
    ''' <returns>一覧の行高の計算数（各行の詳細総件数より計算数）を返却、-1:エラー発生</returns>
    ''' <remarks></remarks>
    Private Function SetMileageData(ByVal inStartIndex As Long, _
                              ByVal inEndIndex As Long, _
                              ByVal inPreEndIndex As Long, _
                              ByVal inCallID As Long) As Integer
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} START: inStartIndex:[{2}], inEndIndex:[{3}], inCallID:[{4}](0:IniEvent,1:BackPageEvent,2:NextPageEvent)," _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name _
                    , inStartIndex _
                    , inEndIndex _
                    , inCallID))

        'スタッフ情報保持
        Dim staffInfo As StaffContext = StaffContext.Current

        '返却変数を宣言
        Dim retCaleCount As Integer = 0

        '表示する日数
        Dim OccurdateOffsetCount As String = Me.HiddenWarningDispDays.Value

        'GBOOK表示件数
        Dim telemaDispCount As Long = CType(Me.HiddenTelemaDisplayCount.Value, Long)

        '発生日時
        Dim Occurdate As Date = DateTimeFunc.Now(staffInfo.DlrCD)
        'VIN
        Dim Vin As String = String.Empty
        '車両ID
        Dim VclId As Decimal = CType(Me.HiddenVclID.Value, Decimal)

        'システム設定から表示する日数を取得
        If IsNothing(OccurdateOffsetCount) Then
            'Nothingの場合、処理終了
            Logger.Info(String.Format(CultureInfo.CurrentCulture _
                        , "{0}.{1} Error GetSystemSetting[MILE_WARN_DISP_DAY_COUNT] Nothing, Return:-1 END" _
                        , Me.GetType.ToString _
                        , System.Reflection.MethodBase.GetCurrentMethod.Name))

            '予想外エラー発生メッセージを出す
            Me.ShowMessageBox(WordId.id902)

            Return -1

        End If

        '発生日時計算＝現在日時－システム設定値
        If IsNumeric(OccurdateOffsetCount) Then
            Occurdate = Occurdate.AddDays(-CType(OccurdateOffsetCount, Integer))
        End If

        '走行距離履歴一覧データ保持
        Dim dtRows As SC3240601TelemaInfoDataTable
        'グラフデータ保持
        Dim dtGraphRows As SC3240601TelemaInfoDataTable
        '所有者情報データ保持
        Dim dtOwnerInfoRow As SC3240601DataSet.SC3240601OwnerInfoRow
        '走行距離履歴一覧総件数保持
        Dim totalCount As Long

        Using biz As New SC3240601BusinessLogic

            Try
                '所有者情報取得＆出力
                dtOwnerInfoRow = biz.GetOwnerInfo(staffInfo.DlrCD, VclId)

                If IsNothing(dtOwnerInfoRow) Then
                    'Nothingの場合、処理終了
                    Logger.Info(String.Format(CultureInfo.CurrentCulture _
                                , "{0}.{1} Error GetOwnerInfo Nothing, Return:-1 END" _
                                , Me.GetType.ToString _
                                , System.Reflection.MethodBase.GetCurrentMethod.Name))

                    '予想外エラー発生メッセージを出す
                    Me.ShowMessageBox(WordId.id902)

                    Return -1
                End If

                If Not dtOwnerInfoRow.IsOWNERNull Then
                    'オーナーズID出力
                    Me.lblOwnerValue.Text = dtOwnerInfoRow.OWNER
                End If
                If Not dtOwnerInfoRow.IsMODEL_CDNull Then
                    'モデルコード出力
                    Me.lblModelValue.Text = dtOwnerInfoRow.MODEL_CD
                End If
                If Not dtOwnerInfoRow.IsVINNull Then
                    'VIN保持
                    Vin = dtOwnerInfoRow.VIN
                    'VIN出力
                    Me.lblVinValue.Text = dtOwnerInfoRow.VIN
                End If
                If Not dtOwnerInfoRow.IsREG_NUMNull Then
                    '車両登録番号出力
                    Me.lblRegNoValue.Text = dtOwnerInfoRow.REG_NUM
                End If

                'VIN保持
                Me.HiddenVin.Value = Vin
                '発生日時保持
                Me.HiddenOccurdate.Value = Occurdate.ToString(CultureInfo.CurrentCulture)

                'エリア更新
                Me.ContentUpdateButtonPanel.Update()

                'オーナーズIDを取得する
                Dim OwnersId As String = String.Empty

                'テレマ導入フラグの判定
                If String.Equals("1", Me.HiddenTeremaIntroduction.Value) Then
                    '導入している場合

                    OwnersId = biz.GetOwnerId(Vin, VclId)

                End If

                'オーナーズID保持
                Me.HiddenOwnerID.Value = OwnersId
                
                '走行距離履歴一覧取得
                dtRows = biz.GetMileageList(VclId, _
                                            Vin, _
                                            OwnersId, _
                                            Occurdate, _
                                            inStartIndex, _
                                            inEndIndex, _
                                            telemaDispCount)

                If IsNothing(dtRows) Then
                    'Nothingの場合、処理終了
                    Logger.Info(String.Format(CultureInfo.CurrentCulture _
                            , "{0}.{1} Return GetMileageList Nothing, Return:-1 END" _
                            , Me.GetType.ToString _
                            , MethodBase.GetCurrentMethod.Name))

                    '予想外エラー発生メッセージを出す
                    Me.ShowMessageBox(WordId.id902)

                    Return -1
                End If
                
                '走行距離履歴一覧件数
                totalCount = biz.GetMileageListCount(VclId, _
                                                     Vin, _
                                                     OwnersId, _
                                                     Occurdate, _
                                                     telemaDispCount)

                If totalCount = -1 Then
                    '総件数取得エラーの場合、処理終了
                    Logger.Info(String.Format(CultureInfo.CurrentCulture _
                            , "{0}.{1} Error List Count -1, Return:-1 END" _
                            , Me.GetType.ToString _
                            , MethodBase.GetCurrentMethod.Name))

                    '予想外エラー発生メッセージを出す
                    Me.ShowMessageBox(WordId.id902)

                    Return -1
                End If

                If inEndIndex >= totalCount Then
                    '検索終了Indexは総件数を超える場合、総件数が検索終了Indexに設定
                    inEndIndex = totalCount
                End If

                If (dtRows.Rows.Count > 0) Then
                    '一覧データがある場合

                    '走行距離履歴一覧データをセットする
                    retCaleCount = Me.SetMileageListData(dtRows, inStartIndex, inPreEndIndex)

                    '読込文言制御処理
                    If 1 < inStartIndex Then
                        '前N件読込　文言表示させる
                        ScriptManager.RegisterStartupScript(Me, Me.GetType, "SetPrePageLinkEnable", "SetPrePageLinkEnable(1);", True)
                    Else
                        '前N件読込　文言表示しない
                        ScriptManager.RegisterStartupScript(Me, Me.GetType, "SetPrePageLinkEnable", "SetPrePageLinkEnable(0);", True)
                    End If

                    If inEndIndex < totalCount Then
                        '次N件読込　文言表示させる
                        ScriptManager.RegisterStartupScript(Me, Me.GetType, "SetNextPageLinkEnable", "SetNextPageLinkEnable(1);", True)
                    Else
                        '次N件読込　文言表示しない
                        ScriptManager.RegisterStartupScript(Me, Me.GetType, "SetNextPageLinkEnable", "SetNextPageLinkEnable(0);", True)
                    End If

                    '読み込み中を非表示設定
                    Me.BackPageLoad.Attributes("style") = "display:none; text-align: center;line-height:46px;font-size: 14px;"
                    Me.NextPageLoad.Attributes("style") = "display:none; text-align: center;line-height:46px;font-size: 14px;"

                    '検索開始Indexを保持
                    Me.HiddenStartIndex.Value = CType(inStartIndex, String)
                    '検索終了Indexを保持
                    Me.HiddenEndIndex.Value = CType(inEndIndex, String)

                    'エリア更新
                    Me.ContentUpdateListScrollBox.Update()
                    Me.ContentUpdateButtonPanel.Update()

                Else
                    '一覧データがない場合、処理終了
                    Logger.Info(String.Format(CultureInfo.CurrentCulture _
                                , "{0}.{1} GetMileageList Data NoFound, Return: -1 END" _
                                , Me.GetType.ToString _
                                , System.Reflection.MethodBase.GetCurrentMethod.Name))
                    Return -1
                End If

                If inCallID = 0 Then
                    '画面初期化の場合のみ、グラフ情報を取得する

                    '検索終了日付
                    Dim endDate As Date = DateTimeFunc.Now(staffInfo.DlrCD)
                    '検索開始日付、終了日付より1年前
                    Dim startDate = endDate.AddYears(-1)

                    'グラフ情報を取得
                    dtGraphRows = biz.GetMileageGraph(VclId, Vin, OwnersId, Occurdate, startDate, endDate)

                    If IsNothing(dtGraphRows) Then
                        'Nothingの場合、処理終了
                        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                                , "{0}.{1} Error GetMileageGraph Nothing, Return: -1 END" _
                                , Me.GetType.ToString _
                                , MethodBase.GetCurrentMethod.Name))

                        '予想外エラー発生メッセージを出す
                        Me.ShowMessageBox(WordId.id902)

                        Return -1
                    End If

                    If (dtGraphRows.Rows.Count > 0) Then
                        'グラフ情報がある場合

                        'グラフ情報を作成する
                        Me.SetGraphData(dtGraphRows, startDate, endDate)
                    Else
                        '情報がの場合、処理終了
                        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                                    , "{0}.{1} GetMileageGraph Data Nothing, Return:-1 END" _
                                    , Me.GetType.ToString _
                                    , System.Reflection.MethodBase.GetCurrentMethod.Name))
                        Return -1
                    End If

                    Dim dtGraphPreYearRows As SC3240601TelemaInfoDataTable
                    '一年前の日付を設定
                    Dim startPreYearDate As Date = startDate.AddYears(-1)
                    Dim endPreYearDate As Date = startDate
                    'Pre Yearボタン制御するため、去年のグラフ情報を取得する
                    dtGraphPreYearRows = biz.GetMileageGraph(VclId, Vin, OwnersId, Occurdate, startPreYearDate, endPreYearDate)

                    If IsNothing(dtGraphPreYearRows) Then
                        'Nothingの場合、処理終了
                        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                                , "{0}.{1} Error BackYear GetMileageGraph Nothing, Return:-1 END" _
                                , Me.GetType.ToString _
                                , MethodBase.GetCurrentMethod.Name))

                        '予想外エラー発生メッセージを出す
                        Me.ShowMessageBox(WordId.id902)

                        Return -1
                    End If

                    If (dtGraphPreYearRows.Rows.Count > 0) Then
                        'グラフ情報がある場合
                        'PreYearボタン活性にする
                        ScriptManager.RegisterStartupScript(Me, Me.GetType, "SetGraphPreYearButtonEnable", "SetGraphPreYearButtonEnable(1);", True)
                        Me.HiddenGraphPreButtonEnable.Value = CONST_TRUE
                    Else
                        'PreYearボタン非活性にする
                        ScriptManager.RegisterStartupScript(Me, Me.GetType, "SetGraphPreYearButtonEnable", "SetGraphPreYearButtonEnable(0);", True)
                        Me.HiddenGraphPreButtonEnable.Value = CONST_FALSE
                    End If

                    'Next Yearボタン制御(初期状態は非活性)
                    ScriptManager.RegisterStartupScript(Me, Me.GetType, "SetGraphNextYearButtonEnable", "SetGraphNextYearButtonEnable(0);", True)
                    Me.HiddenGraphNextButtonEnable.Value = CONST_FALSE

                    'グラフ開始日付を保持
                    Me.HiddenGraphStartDate.Value = startDate.ToString(CultureInfo.CurrentCulture)
                    'グラフ終了日付を保持
                    Me.HiddenGraphEndDate.Value = endDate.ToString(CultureInfo.CurrentCulture)
                End If

                'エリア更新
                Me.ContentUpdateButtonPanel.Update()

            Catch ex As OracleExceptionEx When ex.Number = 1013
                'ORACLEのタイムアウト処理
                Logger.Error(String.Format(CultureInfo.CurrentCulture _
                                         , "{0}.{1} DB TIMEOUT:{2}, Return:-1 END" _
                                         , Me.GetType.ToString _
                                         , System.Reflection.MethodBase.GetCurrentMethod.Name _
                                         , ex.Message))

                'DBタイムアウトのメッセージ表示
                Me.ShowMessageBox(WordId.id901)

                Return -1

            Finally

            End Try

        End Using

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} Return:{2} END" _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name _
                    , retCaleCount.ToString(CultureInfo.CurrentCulture)))

        Return retCaleCount

    End Function

    ''' <summary>
    ''' グラフ情報を作成する
    ''' </summary>
    ''' <param name="inRows">走行距離履歴情報</param>
    ''' <param name="inStartDate">開始日付</param>
    ''' <param name="inEndDate">終了日付</param>
    ''' <remarks></remarks>
    Private Sub SetGraphData(ByVal inRows As SC3240601TelemaInfoDataTable, _
                             ByVal inStartDate As Date, _
                             ByVal inEndDate As Date)
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} START: inRows:[{2}], inStartDate:[{3}], inEndDate:[{4}]," _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name _
                    , inRows _
                    , inStartDate.ToString(CultureInfo.CurrentCulture) _
                    , inEndDate.ToString(CultureInfo.CurrentCulture)))

        'グラフDataTableクリア
        Me.ChartJsonDataTabel.Clear()

        '終了日付-開始日付の日数を計算する
        Dim spDays As TimeSpan
        spDays = inEndDate - inStartDate

        '走行距離履歴情報の開始日付を取得
        Dim graphStartDate As Date = inRows(inRows.Rows.Count - 1).REG_DATE
        '走行距離履歴情報の最終日付を取得
        Dim graphEndDate As Date = inRows(0).REG_DATE

        '国コード
        Dim CountryCode As String = EnvironmentSetting.CountryCode
        '基幹システム名
        Dim DmsName As String = CType(Me.HiddenDmsName.Value, String)
        '販売店名称
        Dim DealerName As String = String.Empty
        '行Index
        Dim iIndex As Integer = 0

        Using biz As New SC3240601BusinessLogic

            'Loopでグラフデータを作る
            For iDay As Integer = 0 To spDays.Days
                '抽出グラフ情報より、最末の距離登録日付は最初日付ので、最末から情報分析する
                Dim tempDate As Date = inEndDate.AddDays(-iDay)

                If tempDate.Date >= graphStartDate.Date AndAlso tempDate.Date <= graphEndDate.Date _
                    AndAlso iIndex < inRows.Rows.Count Then
                    'グラフ区間に入るの場合

                    'Warning名称を保持
                    Dim WarningName As String = String.Empty

                    'ROW取得
                    Dim drInfo As SC3240601DataSet.SC3240601TelemaInfoRow = _
                        CType(inRows(iIndex), SC3240601DataSet.SC3240601TelemaInfoRow)

                    '同じ登録日付のデータを取得、表示順より、一件目は次の処理対象にする
                    Dim drFilterRows As SC3240601TelemaInfoRow() = _
                        (From dr In inRows _
                         Order By dr.MARK_SORT Descending _
                         Select dr _
                         Where dr.REG_DATE = drInfo.REG_DATE).ToArray

                    If INFORMATIONSOURCE_CODE_0.Equals(drFilterRows(0).REG_MTD) Then
                        '登録方法は0:Warningの場合、Warning名称情報が取得する必要がある

                        'Warning情報・詳細を取得する
                        Dim rowsWarningDetail As SC3240601WarningDetailDataTable _
                            = biz.GetWarningDetail(CountryCode, drFilterRows(0).OWNERS_ID, drFilterRows(0).VIN, drFilterRows(0).RECEIVESEQ, drFilterRows(0).SEQNO)

                        If IsNothing(rowsWarningDetail) Then
                            'Nothingの場合、処理終了する
                            Logger.Info(String.Format(CultureInfo.CurrentCulture _
                                        , "{0}.{1} Error GetWarningDetail Return Nothing END" _
                                        , Me.GetType.ToString _
                                        , System.Reflection.MethodBase.GetCurrentMethod.Name))

                            '予想外エラー発生メッセージを出す
                            Me.ShowMessageBox(WordId.id902)

                            Return
                        End If

                        For m As Integer = 0 To rowsWarningDetail.Rows.Count - 1

                            'ROW取得
                            Dim drDetail As SC3240601DataSet.SC3240601WarningDetailRow = _
                                CType(rowsWarningDetail(m), SC3240601DataSet.SC3240601WarningDetailRow)

                            'Warning名称保持
                            WarningName = drDetail.WARNINGNAME.ToString(CultureInfo.CurrentCulture)
                        Next

                    End If

                    'グラフに反映できるデータを作成
                    Me.SetGraph(drFilterRows(0), WarningName, tempDate)

                    '次の距離採取日から、Loopを続く
                    Dim spSubDays As TimeSpan
                    spSubDays = tempDate - drFilterRows(0).REG_DATE

                    iDay = iDay + spSubDays.Days
                    '次の日付のデータに移り
                    iIndex = iIndex + drFilterRows.Length

                ElseIf 0 = iDay OrElse spDays.Days = iDay Then
                    'グラフ区間以外、表示最初日付と最後日付になった場合、端点の空データをグラフに追加
                    Me.SetGraph(Nothing, String.Empty, tempDate)
                End If

            Next
        End Using

        'グラフデータ保持
        Me.HiddenGraphDataField.Value = Me.ToJson(ChartJsonDataTabel)

        'Updateパネル更新
        Me.ContentUpdateButtonPanel.Update()

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} END" _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name))

    End Sub

    ''' <summary>
    ''' 走行距離履歴一覧データ出力処理
    ''' </summary>
    ''' <param name="inRows">走行距離履歴情報</param>
    ''' <param name="inStartIndex">検索開始Index</param>
    ''' <param name="inPreEndIndex">前ページの終了Index</param>
    ''' <returns>一覧の行高の計算数（各行の詳細総件数より計算数）を返却、-1:エラー発生</returns>
    ''' <remarks></remarks>
    Private Function SetMileageListData(ByVal inRows As SC3240601TelemaInfoDataTable, _
                                        ByVal inStartIndex As Long, _
                                        ByVal inPreEndIndex As Long) As Integer
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} START: inRows:[{2}]" _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name _
                    , inRows))

        'スタッフ情報保持
        Dim staffInfo As StaffContext = StaffContext.Current

        '返却変数を宣言
        Dim retCaleCount As Integer = 0

        '国コード
        Dim CountryCode As String = EnvironmentSetting.CountryCode
        '基幹システム名
        Dim DmsName As String = CType(Me.HiddenDmsName.Value, String)

        'Dataバインド
        Me.WarningInfoRepeater.DataSource = Nothing
        Me.WarningInfoRepeater.DataSource = inRows
        Me.WarningInfoRepeater.DataBind()

        'Repeaterに行ごとにデータ出力する
        For i As Integer = 0 To Me.WarningInfoRepeater.Items.Count - 1

            '画面定義取得
            Dim warningInfoArea As Control = Me.WarningInfoRepeater.Items(i)

            'ROW取得
            Dim drInfo As SC3240601DataSet.SC3240601TelemaInfoRow = _
                CType(inRows(i), SC3240601DataSet.SC3240601TelemaInfoRow)

            'Noエリア
            CType(warningInfoArea.FindControl("lblNumberRecord"), CustomLabel).Text = _
                drInfo.NO.ToString(CultureInfo.CurrentCulture)
            'Dateエリア
            If Not drInfo.IsREG_DATENull Then
                'YYYYMMDDをフォーマット
                Dim strBaseDate = DateTimeFunc.FormatDate(3, drInfo.REG_DATE)
                CType(warningInfoArea.FindControl("lblDateRecord"), CustomLabel).Text = _
                    strBaseDate.ToString(CultureInfo.CurrentCulture)
            End If
            'Mileageエリア
            If Not drInfo.IsREG_MILENull Then
                '距離＋km
                Dim strMileage As String = String.Empty

                '走行距離の整数と小数点以下のデータ抽出
                Dim numberMileage As Decimal = Math.Truncate(drInfo.REG_MILE)
                Dim fractionMileage As Decimal = drInfo.REG_MILE - numberMileage

                '小数点以下のデータチェック
                If 0 < fractionMileage Then
                    '小数点以下が存在する場合
                    '整数データと小数データを整形したデータを格納「"#,###0" + ".#0" or ".##0" or ".###0" or ".####" + "km"」※以下に例を記述
                    '「1000.0   →1,000km」
                    '「1000.1   →1,000.10km」
                    '「1000.11  →1,000.110km」
                    '「1000.111 →1,000.1110km」
                    '「1000.1111→1,000.1111km」
                    Dim strFractionMileage As String = fractionMileage.ToString("#.0000")
                    Dim replaceString As String = String.Empty

                    '一番後ろの数値文字列チェック
                    If REPLACE_MILEAGE_ZERO_THREE.Equals(strFractionMileage.Substring(strFractionMileage.Length - 3)) Then
                        '「000」の場合
                        '置換文字列を「000」に設定
                        replaceString = REPLACE_MILEAGE_ZERO_THREE

                    ElseIf REPLACE_MILEAGE_ZERO_TWO.Equals(strFractionMileage.Substring(strFractionMileage.Length - 2)) Then
                        '「00」の場合
                        '置換文字列を「00」に設定
                        replaceString = REPLACE_MILEAGE_ZERO_TWO

                    Else
                        '上記以外の場合
                        '置換文字列を「0000」に設定
                        replaceString = REPLACE_MILEAGE_ZERO_FOUR

                    End If

                    strMileage = String.Concat(numberMileage.ToString("#,##0"), _
                                               strFractionMileage.Replace(replaceString, "0"), _
                                               Me.HiddenKm.Value)

                Else
                    '上記以外の場合
                    '整数データを整形したデータを格納「"#,###0" + "km"」
                    strMileage = String.Concat(numberMileage.ToString("#,##0"), Me.HiddenKm.Value)

                End If

                CType(warningInfoArea.FindControl("lblMileageRecord"), CustomLabel).Text = strMileage

            End If

            'InformationSourceエリア
            Dim InformationSourceWord As String = String.Empty
            If Not drInfo.IsREG_MTDNull Then
                If INFORMATIONSOURCE_CODE_1.Equals(drInfo.REG_MTD) Then
                    ' １：基幹入庫履歴

                    '販売店表示名
                    Dim DisplayDealerName As String = String.Empty

                    Using biz As New SC3240601BusinessLogic

                        Dim DealerName As String = String.Empty
                        '販売店名称を取得
                        DealerName = biz.GetBranchName(drInfo.DLR_CD, drInfo.BRN_CD)

                        '空白名称をStringEmptyにする
                        If Not String.IsNullOrEmpty(DealerName) Then
                            If String.IsNullOrEmpty(DealerName.Trim()) Then
                                DealerName = DealerName.Trim()
                            End If
                        End If

                        If String.IsNullOrEmpty(DealerName) AndAlso _
                           Not drInfo.DLR_CD.Equals(staffInfo.DlrCD) Then
                            ' 販売店取得できなかった場合、代わりに「Other Dealer」を表示
                            DisplayDealerName = Me.HiddenWord023OtherDealer.Value.ToString(CultureInfo.CurrentCulture)
                        ElseIf String.IsNullOrEmpty(DealerName) AndAlso _
                               drInfo.DLR_CD.Equals(staffInfo.DlrCD) Then
                            '販売店コードはログイン販売店コードと一致、その名称はない場合
                            '販売店名称は空白にする
                            DisplayDealerName = String.Empty
                        Else
                            '名称がある場合
                            DisplayDealerName = DealerName
                        End If

                    End Using

                    If String.IsNullOrEmpty(DmsName) Then
                        '基幹システム名ない場合
                        If String.IsNullOrEmpty(DisplayDealerName) Then
                            '販売店表示名ない場合、「-」を出力
                            InformationSourceWord = Me.HiddenWord041Hyphen.Value.ToString(CultureInfo.CurrentCulture)
                        Else
                            '販売店表示名を出力
                            InformationSourceWord = DisplayDealerName
                        End If
                    Else
                        If String.IsNullOrEmpty(DisplayDealerName) Then
                            '販売店表示名ない場合、基幹システム名のみ出力
                            InformationSourceWord = DmsName
                        Else
                            '販売店表示名ある場合、「基幹システム名（販売店名）」の形で出力
                            InformationSourceWord = _
                                Me.HiddenWord029Format.Value.ToString(CultureInfo.CurrentCulture).Replace("{0}", DmsName)
                            InformationSourceWord = InformationSourceWord.Replace("{1}", DisplayDealerName)
                        End If
                    End If
                ElseIf INFORMATIONSOURCE_CODE_2.Equals(drInfo.REG_MTD) Then
                    ' ２：サイト入力の場合、「Owner Site」を出力
                    InformationSourceWord = Me.HiddenWord024OwnerSite.Value.ToString(CultureInfo.CurrentCulture)
                ElseIf INFORMATIONSOURCE_CODE_3.Equals(drInfo.REG_MTD) Then
                    ' ３：走行距離アンケートの場合、「SMS」を出力
                    InformationSourceWord = Me.HiddenWord025SMS.Value.ToString(CultureInfo.CurrentCulture)
                ElseIf INFORMATIONSOURCE_CODE_4.Equals(drInfo.REG_MTD) Then
                    ' ４：コールセンター入力
                    InformationSourceWord = _
                        Me.HiddenWord029Format.Value.ToString(CultureInfo.CurrentCulture).Replace( _
                            "{0}", _
                            Me.HiddenWord026iCROP.Value.ToString(CultureInfo.CurrentCulture) _
                            )
                    If Not drInfo.IsSTF_NAMENull Then
                        'スタッフ名がある場合、「iCROP(スタッフ名)」を出力
                        InformationSourceWord = InformationSourceWord.Replace("{1}", drInfo.STF_NAME)
                    Else
                        'スタッフ名がない場合、「iCROP()」を出力
                        InformationSourceWord = InformationSourceWord.Replace("{1}", String.Empty)
                    End If
                ElseIf INFORMATIONSOURCE_CODE_5.Equals(drInfo.REG_MTD) Then
                    ' ５：G-BOOK（代表）場合、「G-BOOK」を出力
                    InformationSourceWord = Me.HiddenWord027GBOOK.Value.ToString(CultureInfo.CurrentCulture)
                ElseIf INFORMATIONSOURCE_CODE_6.Equals(drInfo.REG_MTD) Then
                    ' ６：G-BOOK（複写）場合、「G-BOOK」を出力
                    InformationSourceWord = Me.HiddenWord027GBOOK.Value.ToString(CultureInfo.CurrentCulture)
                ElseIf INFORMATIONSOURCE_CODE_7.Equals(drInfo.REG_MTD) Then
                    ' ７：サイト入力データ（複写）場合、「Owner Site」を出力
                    InformationSourceWord = Me.HiddenWord024OwnerSite.Value.ToString(CultureInfo.CurrentCulture)
                ElseIf INFORMATIONSOURCE_CODE_0.Equals(drInfo.REG_MTD) Then
                    ' Warning情報の場合、「G-BOOK(Warning)」を出力
                    InformationSourceWord = Me.HiddenWord028GBOOKWarning.Value.ToString(CultureInfo.CurrentCulture)
                End If
            End If
            CType(warningInfoArea.FindControl("lblISRecord"), CustomLabel).Text = _
                InformationSourceWord

            'Customerエリア
            If Not drInfo.IsREG_MTDNull Then
                If INFORMATIONSOURCE_CODE_1.Equals(drInfo.REG_MTD) Then
                    ' １：基幹入庫履歴
                    If drInfo.IsCST_NAMENull Then
                        '顧客名称がない場合、「-」を出力
                        CType(warningInfoArea.FindControl("lblCustomerRecord"), CustomLabel).Text = _
                            Me.HiddenWord041Hyphen.Value.ToString(CultureInfo.CurrentCulture)
                    Else
                        '顧客名称がある場合、顧客名称を出力
                        CType(warningInfoArea.FindControl("lblCustomerRecord"), CustomLabel).Text = _
                            drInfo.CST_NAME
                    End If
                Else
                    '他の場合、「-」を出力
                    CType(warningInfoArea.FindControl("lblCustomerRecord"), CustomLabel).Text = _
                        Me.HiddenWord041Hyphen.Value.ToString(CultureInfo.CurrentCulture)
                End If
            Else
                '他の場合、「-」を出力
                CType(warningInfoArea.FindControl("lblCustomerRecord"), CustomLabel).Text = _
                    Me.HiddenWord041Hyphen.Value.ToString(CultureInfo.CurrentCulture)
            End If

            'Informationエリア・Detailエリア出力処理
            '一覧の行高の計算数を取得
            Dim iCaleCount As Integer = Me.SetInformationAndDetailArea( _
                               drInfo, _
                               warningInfoArea, _
                               CountryCode)

            '一覧の行高の計算数を計処理
            If inPreEndIndex >= inStartIndex AndAlso _
               inStartIndex + i <= inPreEndIndex Then
                '前ページ終了Indexが当開始Indexより大きい、
                'なお現処理行目が前ページ終了Indexより前の場合
                '計算処理行う
                retCaleCount = retCaleCount + iCaleCount
            End If

        Next

        '件数保持
        Me.HiddenSearchListCount.Value = CType(WarningInfoRepeater.Items.Count, String)
        Me.HiddenOrderListDisplayType.Value = POPUP_TYPE_DISPLAY

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} Return{2} END" _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name _
                    , retCaleCount.ToString(CultureInfo.CurrentCulture)))

        '一覧の行高の計算数を返却
        Return retCaleCount

    End Function

    ''' <summary>
    ''' グラフデータ作成
    ''' </summary>
    ''' <param name="inMileageRow">走行距離履歴情報</param>
    ''' <param name="inWarningName">Warning名称</param>
    ''' <param name="inRegDate">登録日付（グラフ区間以外）</param>
    ''' <remarks></remarks>
    Private Sub SetGraph(ByVal inMileageRow As SC3240601DataSet.SC3240601TelemaInfoRow, _
                         ByVal inWarningName As String, _
                         ByVal inRegDate As Date)
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} START: inMileageRow:[{2}], inWarningName:[{3}], inRegDate:[{4}]," _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name _
                    , inMileageRow _
                    , inWarningName _
                    , inRegDate.ToString(CultureInfo.CurrentCulture)))

        'グラフ吹き出し表示文字
        Dim GSource As String = String.Empty
        'グラフ吹き出し・線色
        Dim GColor As String = String.Empty

        If Not (inMileageRow Is Nothing) Then
            'グラフ区間内の場合

            If Not inMileageRow.IsREG_MTDNull Then

                'グラフデータカラー設定

                If INFORMATIONSOURCE_CODE_1.Equals(inMileageRow.REG_MTD) Then
                    '登録方法１：基幹入庫履歴
                    If MILE_TLM_DISP_FLG_VALUE_1.Equals(Me.HiddenMileTlmDispFlg.Value) Then
                        '走行距離履歴表示する場合、線色つけ
                        GColor = LINE_COLOR_GREEN
                    End If
                ElseIf INFORMATIONSOURCE_CODE_2.Equals(inMileageRow.REG_MTD) Or _
                    INFORMATIONSOURCE_CODE_3.Equals(inMileageRow.REG_MTD) Or _
                    INFORMATIONSOURCE_CODE_4.Equals(inMileageRow.REG_MTD) Or _
                    INFORMATIONSOURCE_CODE_7.Equals(inMileageRow.REG_MTD) Then
                    '登録方法２: サイト入力，３: 走行距離アンケート，４: コールセンター入力，登録方法７: サイト入力データ（複写）の場合
                    If MILE_TLM_DISP_FLG_VALUE_1.Equals(Me.HiddenMileTlmDispFlg.Value) Then
                        '走行距離履歴表示する場合、線色つけ
                        GColor = LINE_COLOR_YELLOW
                    End If
                ElseIf (INFORMATIONSOURCE_CODE_5.Equals(inMileageRow.REG_MTD) Or _
                    INFORMATIONSOURCE_CODE_6.Equals(inMileageRow.REG_MTD)) AndAlso _
                    USE_WARN_FLG_VALUE_1.Equals(Me.HiddenUserWarnFlg.Value) Then
                    '登録方法５: G-BOOK（代表），６: G-BOOK（複写の場合
                    GColor = LINE_COLOR_BLUE
                ElseIf INFORMATIONSOURCE_CODE_0.Equals(inMileageRow.REG_MTD) AndAlso _
                    USE_WARN_FLG_VALUE_1.Equals(Me.HiddenUserWarnFlg.Value) Then
                    '登録方法０：Warning情報、そしてWarning情報表示フラグ１の場合
                    GColor = LINE_COLOR_RED
                End If
            End If

            'グラフデータ文言設定

            If Not inMileageRow.IsREG_MTDNull Then
                If INFORMATIONSOURCE_CODE_1.Equals(inMileageRow.REG_MTD) Then
                    '登録方法１：基幹入庫履歴の場合
                    '基幹システム名を出力
                    GSource = Me.HiddenGraphLegend4.Value.ToString(CultureInfo.CurrentCulture)
                ElseIf INFORMATIONSOURCE_CODE_2.Equals(inMileageRow.REG_MTD) Or _
                    INFORMATIONSOURCE_CODE_7.Equals(inMileageRow.REG_MTD) Then
                    '登録方法２: サイト入力，７: サイト入力データ（複写）の場合
                    'Owner Siteを出力
                    GSource = Me.HiddenWord024OwnerSite.Value.ToString(CultureInfo.CurrentCulture)
                ElseIf INFORMATIONSOURCE_CODE_5.Equals(inMileageRow.REG_MTD) Or _
                    INFORMATIONSOURCE_CODE_6.Equals(inMileageRow.REG_MTD) Then
                    '登録方法５: G-BOOK（代表），６: G-BOOK（複写の場合
                    'G-BOOKを出力
                    GSource = Me.HiddenWord008GBOOK.Value.ToString(CultureInfo.CurrentCulture)
                ElseIf INFORMATIONSOURCE_CODE_3.Equals(inMileageRow.REG_MTD) Then
                    '登録方法３: 走行距離アンケートの場合
                    'SMSを出力
                    GSource = Me.HiddenWord025SMS.Value.ToString(CultureInfo.CurrentCulture)
                ElseIf INFORMATIONSOURCE_CODE_4.Equals(inMileageRow.REG_MTD) Then
                    '登録方法４: コールセンター入力の場合
                    'i-CROPを出力
                    GSource = Me.HiddenWord026iCROP.Value.ToString(CultureInfo.CurrentCulture)
                ElseIf INFORMATIONSOURCE_CODE_0.Equals(inMileageRow.REG_MTD) Then
                    '登録方法０：Warning情報の場合
                    'Warning名称を出力
                    GSource = inWarningName
                End If
            End If

            'グラフ表示用Data作る
            Dim drTemp As SC3240601GraphJsonRow = Me.ChartJsonDataTabel.NewSC3240601GraphJsonRow()
            'グラフ日付情報
            drTemp.REGDATE = String.Format("{0:yyyy/MM/dd}", inMileageRow.REG_DATE)
            'グラフ走行距離情報

            drTemp.REGMILE = inMileageRow.REG_MILE

            '走行距離の整数と小数点以下のデータ抽出
            Dim numberMileage As Decimal = Math.Truncate(inMileageRow.REG_MILE)
            Dim fractionMileage As Decimal = inMileageRow.REG_MILE - numberMileage

            '小数点以下のデータチェック
            If 0 < fractionMileage Then
                '小数点以下が存在する場合
                '整数データと小数データを整形したデータを格納「"#,###0" + ".#0" or ".##0" or ".###0" or ".####" + "km"」※以下に例を記述
                '「1000.0   →1,000km」
                '「1000.1   →1,000.10km」
                '「1000.11  →1,000.110km」
                '「1000.111 →1,000.1110km」
                '「1000.1111→1,000.1111km」
                Dim strFractionMileage As String = fractionMileage.ToString("#.0000")
                Dim replaceString As String = String.Empty

                '一番後ろの数値文字列チェック
                If REPLACE_MILEAGE_ZERO_THREE.Equals(strFractionMileage.Substring(strFractionMileage.Length - 3)) Then
                    '「000」の場合
                    '置換文字列を「000」に設定
                    replaceString = REPLACE_MILEAGE_ZERO_THREE

                ElseIf REPLACE_MILEAGE_ZERO_TWO.Equals(strFractionMileage.Substring(strFractionMileage.Length - 2)) Then
                    '「00」の場合
                    '置換文字列を「00」に設定
                    replaceString = REPLACE_MILEAGE_ZERO_TWO

                Else
                    '上記以外の場合
                    '置換文字列を「0000」に設定
                    replaceString = REPLACE_MILEAGE_ZERO_FOUR

                End If

                drTemp.DISPREGMILE = String.Concat(numberMileage.ToString("#,##0"), _
                                                   strFractionMileage.Replace(replaceString, "0"), _
                                                   Me.HiddenKm.Value)

            Else
                '上記以外の場合
                '整数データを整形したデータを格納「"#,###0" + "km"」
                drTemp.DISPREGMILE = String.Concat(numberMileage.ToString("#,##0"), Me.HiddenKm.Value)

            End If

            'グラフ吹き出し詳細内容
            drTemp.DESCRIPTION = GSource
            'グラフ線・吹き出し背景色
            drTemp.LINECOLOR = GColor

            '日付順次にするため、新規データはテーブルの先頭に追加
            Me.ChartJsonDataTabel.Rows.InsertAt(drTemp, 0)
            'End If
        Else
            'グラフ区間以外、走行距離ない区間で、日時だけ埋める

            'グラフ表示用Data作る
            Dim drTemp As SC3240601GraphJsonRow = Me.ChartJsonDataTabel.NewSC3240601GraphJsonRow()
            'グラフ日付情報
            drTemp.REGDATE = String.Format("{0:yyyy/MM/dd}", inRegDate)
            'グラフ走行距離情報
            drTemp.REGMILE = -1
            'グラフ吹き出し詳細内容
            drTemp.DESCRIPTION = String.Empty
            'グラフ線・吹き出し背景色
            drTemp.LINECOLOR = String.Empty

            '日付順次にするため、新規データはテーブルの先頭に追加
            Me.ChartJsonDataTabel.Rows.InsertAt(drTemp, 0)

        End If

        'テーブル更新
        Me.ChartJsonDataTabel.AcceptChanges()

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} END" _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name))

    End Sub

    ''' <summary>
    ''' Informationエリア・Detailボタンエリア出力処理
    ''' </summary>
    ''' <param name="inRow">走行距離履歴情報</param>
    ''' <param name="inWarningInfoArea">Repeaterエリア Object</param>
    ''' <param name="inCountryCode">国コード</param>
    ''' <returns>一覧の行高の計算数（詳細件数-1）を返却、-1:エラー発生</returns>
    ''' <remarks></remarks>
    Private Function SetInformationAndDetailArea(ByVal inRow As SC3240601DataSet.SC3240601TelemaInfoRow, _
                               ByVal inWarningInfoArea As Control, _
                               ByVal inCountryCode As String) As Integer
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} START: inRow:[{2}], inInformationArea:[{3}], inCountryCode:[{4}]," _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name _
                    , inRow _
                    , inWarningInfoArea _
                    , inCountryCode.ToString(CultureInfo.CurrentCulture)))

        Dim dtMileageInfo As SC3240601TelemaInfoDataTable
        If INFORMATIONSOURCE_CODE_0.Equals(inRow.REG_MTD) Then
            'Warning情報の場合

            Using biz As New SC3240601BusinessLogic
                '走行距離履歴Warning情報取得
                dtMileageInfo = biz.GetMileageWarningList(inRow.OWNERS_ID, inRow.VIN, inRow.OCCURDATE, inRow.RECEIVESEQ)

                If IsNothing(dtMileageInfo) Then
                    'Nothingの場合、処理終了
                    Logger.Info(String.Format(CultureInfo.CurrentCulture _
                                , "{0}.{1} Error Return -1 END" _
                                , Me.GetType.ToString _
                                , System.Reflection.MethodBase.GetCurrentMethod.Name))

                    '予想外エラー発生メッセージを出す
                    Me.ShowMessageBox(WordId.id902)

                    Return -1
                End If

            End Using
        Else
            '他の場合、該当行を入れる
            dtMileageInfo = New SC3240601TelemaInfoDataTable
            dtMileageInfo.ImportRow(inRow)
        End If

        'データバインド
        CType(inWarningInfoArea.FindControl("InformationRepeater"), Repeater).DataSource = dtMileageInfo
        CType(inWarningInfoArea.FindControl("InformationRepeater"), Repeater).DataBind()

        CType(inWarningInfoArea.FindControl("DetailButtonRepeater"), Repeater).DataSource = dtMileageInfo
        CType(inWarningInfoArea.FindControl("DetailButtonRepeater"), Repeater).DataBind()

        '件数を保持
        Dim listCount As Integer = _
            CType(inWarningInfoArea.FindControl("InformationRepeater"), Repeater).Items.Count

        'イメージ表示フラグ
        Dim imageDisplayFlg As String = CType(Me.HiddenImageDisplayFlg.Value, String)
        'イメージベースURL
        Dim imageUrl As String = CType(Me.HiddenImageUrl.Value, String)

        For j As Integer = 0 To listCount - 1

            '画面定義取得
            Dim listInformationArea As Control = _
                CType(inWarningInfoArea.FindControl("InformationRepeater"), Repeater).Items(j)
            Dim listDetailButtonArea As Control = _
                CType(inWarningInfoArea.FindControl("DetailButtonRepeater"), Repeater).Items(j)

            'ROW取得
            Dim drInfo As SC3240601DataSet.SC3240601TelemaInfoRow = dtMileageInfo(j)

            'ボタンタイトル文言
            CType(listDetailButtonArea.FindControl("lblDetailButtonRecord"), CustomLabel).Text = _
                Me.HiddenWord021Detail.Value.ToString(CultureInfo.CurrentCulture)

            If Not drInfo.IsREG_MTDNull Then
                If INFORMATIONSOURCE_CODE_0.Equals(drInfo.REG_MTD) Then
                    'Warning情報の場合

                    Using biz As New SC3240601BusinessLogic
                        'Warning詳細を取得する
                        Dim rowsWarningDetail As SC3240601WarningDetailDataTable _
                            = biz.GetWarningDetail(inCountryCode, drInfo.OWNERS_ID, drInfo.VIN, drInfo.RECEIVESEQ, drInfo.SEQNO)

                        If IsNothing(rowsWarningDetail) Then
                            'Nothingの場合、処理終了
                            Logger.Info(String.Format(CultureInfo.CurrentCulture _
                                        , "{0}.{1} Error WarningDetail DataTable Nothing, Return -1 END" _
                                        , Me.GetType.ToString _
                                        , System.Reflection.MethodBase.GetCurrentMethod.Name))

                            '予想外エラー発生メッセージを出す
                            Me.ShowMessageBox(WordId.id902)

                            Return -1
                        End If

                        'Warningコードを保持
                        Dim outWarningCode As String = String.Empty
                        'Warning名称を保持
                        Dim outWarningName As String = String.Empty

                        For m As Integer = 0 To rowsWarningDetail.Rows.Count - 1

                            'ROW取得
                            Dim drDetail As SC3240601DataSet.SC3240601WarningDetailRow = _
                                CType(rowsWarningDetail(m), SC3240601DataSet.SC3240601WarningDetailRow)

                            'Warning詳細保持(PopUp)
                            'Warning詳細 発生日時
                            Dim detailDate As String = String.Empty
                            'Warning詳細 Warningコード
                            Dim detailCode As String = String.Empty
                            'Warning詳細 走行距離
                            Dim detailMileage As String = String.Empty
                            'Warning詳細 Warning名称
                            Dim detailName As String = String.Empty
                            'Warning詳細 インジケータイメージファイル名
                            Dim detailIndicator As String = String.Empty
                            'Warning詳細 詳細内容
                            Dim detailDescription As String = String.Empty

                            'Warningコードを取得
                            detailCode = drDetail.WARNINGCODE.ToString(CultureInfo.CurrentCulture)

                            'Warning詳細情報
                            If Not drDetail.IsOCCURDATENull Then
                                '「YYYYMMDD HHMI」
                                detailDate = DateTimeFunc.FormatDate(2, drDetail.OCCURDATE)
                            End If

                            If Not drDetail.IsMILEAGENull Then
                                '走行距離+km
                                Dim strMileage As String = String.Empty

                                '走行距離の整数と小数点以下のデータ抽出
                                Dim numberMileage As Decimal = Math.Truncate(drDetail.MILEAGE)
                                Dim fractionMileage As Decimal = drDetail.MILEAGE - numberMileage

                                '小数点以下のデータチェック
                                If 0 < fractionMileage Then
                                    '小数点以下が存在する場合
                                    '整数データと小数データを整形したデータを格納「"#,###0" + ".#0" or ".##0" or ".###0" or ".####" + "km"」※以下に例を記述
                                    '「1000.0   →1,000km」
                                    '「1000.1   →1,000.10km」
                                    '「1000.11  →1,000.110km」
                                    '「1000.111 →1,000.1110km」
                                    '「1000.1111→1,000.1111km」
                                    Dim strFractionMileage As String = fractionMileage.ToString("#.0000")
                                    Dim replaceString As String = String.Empty

                                    '一番後ろの数値文字列チェック
                                    If REPLACE_MILEAGE_ZERO_THREE.Equals(strFractionMileage.Substring(strFractionMileage.Length - 3)) Then
                                        '「000」の場合
                                        '置換文字列を「000」に設定
                                        replaceString = REPLACE_MILEAGE_ZERO_THREE

                                    ElseIf REPLACE_MILEAGE_ZERO_TWO.Equals(strFractionMileage.Substring(strFractionMileage.Length - 2)) Then
                                        '「00」の場合
                                        '置換文字列を「00」に設定
                                        replaceString = REPLACE_MILEAGE_ZERO_TWO

                                    Else
                                        '上記以外の場合
                                        '置換文字列を「0000」に設定
                                        replaceString = REPLACE_MILEAGE_ZERO_FOUR

                                    End If

                                    strMileage = String.Concat(numberMileage.ToString("#,##0"), _
                                                               strFractionMileage.Replace(replaceString, "0"), _
                                                               Me.HiddenKm.Value)

                                Else
                                    '上記以外の場合
                                    '整数データを整形したデータを格納「"#,###0" + "km"」
                                    strMileage = String.Concat(numberMileage.ToString("#,##0"), Me.HiddenKm.Value)

                                End If

                                detailMileage = strMileage

                            End If

                            If Not drDetail.IsWARNINGCODENull Then
                                'Warningコード
                                outWarningCode = drDetail.WARNINGCODE.ToString(CultureInfo.CurrentCulture)
                            End If

                            If Not drDetail.IsWARNINGNAMENull Then
                                'Warning名称
                                detailName = drDetail.WARNINGNAME.ToString(CultureInfo.CurrentCulture)
                                outWarningName = detailName
                            End If

                            If Not drDetail.IsINDICATOR_IMGFILENull Then
                                'インジケータイメージファイル名
                                detailIndicator = drDetail.INDICATOR_IMGFILE

                                'インジケータ表示フラグとファイル名のチェック
                                If INDICATOR_IMAGE_DISP_FLG_1.Equals(imageDisplayFlg) AndAlso Not String.IsNullOrEmpty(detailIndicator) Then
                                    'イメージ表示フラグ１、それにファイル名がある場合
                                    'URLを構成
                                    Dim indicateImageUrl = String.Concat(imageUrl, detailIndicator)

                                    Try
                                        'ファイル存在チェック
                                        If System.IO.File.Exists(Server.MapPath(indicateImageUrl)) Then
                                            detailIndicator = indicateImageUrl

                                        Else
                                            '画像情報が取得できない場合
                                            detailIndicator = "-1"

                                        End If

                                    Catch ex As Exception
                                        '画像情報が取得できない場合
                                        detailIndicator = "-1"

                                    End Try

                                Else
                                    '画像表示しない場合
                                    detailIndicator = "-1"

                                End If

                            End If

                            If Not drDetail.IsEXPLANATIONNull Then
                                'Warning詳細内容
                                detailDescription = drDetail.EXPLANATION.ToString(CultureInfo.CurrentCulture)
                            End If

                            '詳細情報を画面に保持
                            CType(listDetailButtonArea.FindControl("DetailButtonArea"), HtmlContainerControl).Attributes("name") = _
                                "1" + _
                                "|" + detailDate + _
                                "|" + detailCode + _
                                "|" + detailMileage + _
                                "|" + detailName + _
                                "|" + detailIndicator

                            CType(listDetailButtonArea.FindControl("DetailButtonArea"), HtmlContainerControl).Attributes("SelectDetailFlg") = "1"
                            CType(listDetailButtonArea.FindControl("DetailButtonArea"), HtmlContainerControl).Attributes("SelectDetailDate") = detailDate
                            CType(listDetailButtonArea.FindControl("DetailButtonArea"), HtmlContainerControl).Attributes("SelectDetailCode") = detailCode
                            CType(listDetailButtonArea.FindControl("DetailButtonArea"), HtmlContainerControl).Attributes("SelectDetailMileage") = detailMileage
                            CType(listDetailButtonArea.FindControl("DetailButtonArea"), HtmlContainerControl).Attributes("SelectDetailName") = detailName
                            CType(listDetailButtonArea.FindControl("DetailButtonArea"), HtmlContainerControl).Attributes("SelectDetailIndicator") = detailIndicator

                            CType(listDetailButtonArea.FindControl("DetailButtonArea"), HtmlContainerControl).Attributes("value") = _
                                detailDescription

                        Next

                        'Informationエリア処理開始
                        If Not String.IsNullOrEmpty(outWarningCode) AndAlso Not String.IsNullOrEmpty(outWarningName) Then
                            'Warningコードがある場合

                            Dim InformationWord As String = String.Empty
                            '「Warning名称(Warningコード)」で出力
                            InformationWord = _
                                Me.HiddenWord029Format.Value.ToString(CultureInfo.CurrentCulture).Replace("{0}", outWarningName)
                            InformationWord = InformationWord.Replace("{1}", outWarningCode)

                            CType(listInformationArea.FindControl("lblInformationRecord"), CustomLabel).Text = _
                                InformationWord
                        Else
                            'Warningコードがない場合、「-」で出力
                            CType(listInformationArea.FindControl("lblInformationRecord"), CustomLabel).Text = _
                                Me.HiddenWord041Hyphen.Value.ToString(CultureInfo.CurrentCulture)
                        End If

                        'Detailボタンエリア処理
                        For m As Integer = 0 To rowsWarningDetail.Rows.Count - 1

                            'ROW取得
                            Dim drDetail As SC3240601DataSet.SC3240601WarningDetailRow = _
                                CType(rowsWarningDetail(m), SC3240601DataSet.SC3240601WarningDetailRow)

                        Next
                        'Informationエリア処理終了

                    End Using

                    'Detailボタンエリア処理開始
                    'Detailボタン制御
                    If Not (drInfo.IsREG_MTDNull) AndAlso INFORMATIONSOURCE_CODE_0.Equals(drInfo.REG_MTD) Then
                        'Warning情報の場合、Detailボタン活性
                        CType(listDetailButtonArea.FindControl("DetailButtonArea"), HtmlContainerControl).Attributes("class") = "DetailButtonAreaClass BtnBoxOn"

                    Else
                        'Warning情報の場合、Detailボタン非活性
                        CType(listDetailButtonArea.FindControl("DetailButtonArea"), HtmlContainerControl).Attributes("class") = "DetailButtonAreaClass BtnBoxOff"

                    End If

                    If 1 <= j Then
                        '複数ボタン多行表示する場合、レイアウトの調整
                        Dim buttonHeight As String = CType((j * 45) + 8, String)
                        CType(listDetailButtonArea.FindControl("DetailButtonArea"), HtmlContainerControl).Attributes("style") = "top:" + buttonHeight + "px"

                    End If
                    'Detailボタンエリア処理終了

                ElseIf INFORMATIONSOURCE_CODE_1.Equals(drInfo.REG_MTD) Or _
                   INFORMATIONSOURCE_CODE_2.Equals(drInfo.REG_MTD) Or _
                   INFORMATIONSOURCE_CODE_3.Equals(drInfo.REG_MTD) Or _
                   INFORMATIONSOURCE_CODE_4.Equals(drInfo.REG_MTD) Then
                    '登録方法は１，２，３，４の場合
                    If Not drInfo.IsSVC_NAME_MILENull Then
                        'サービス名称がある場合、「サービス名称」を出力
                        CType(listInformationArea.FindControl("lblInformationRecord"), CustomLabel).Text = _
                            drInfo.SVC_NAME_MILE

                    Else
                        'サービス名称がない場合、「-」を出力
                        CType(listInformationArea.FindControl("lblInformationRecord"), CustomLabel).Text = _
                            Me.HiddenWord041Hyphen.Value.ToString(CultureInfo.CurrentCulture)

                    End If

                Else
                    '他の登録方法の場合、「-」で出力
                    CType(listInformationArea.FindControl("lblInformationRecord"), CustomLabel).Text = _
                        Me.HiddenWord041Hyphen.Value.ToString(CultureInfo.CurrentCulture)

                End If

            Else
                '他の場合、「-」で出力
                CType(listInformationArea.FindControl("lblInformationRecord"), CustomLabel).Text = _
                    Me.HiddenWord041Hyphen.Value.ToString(CultureInfo.CurrentCulture)

            End If

        Next


        If 1 < CType(inWarningInfoArea.FindControl("InformationRepeater"), Repeater).Items.Count Then
            '複数行がある場合、行の高さ＆Topを調整
            Dim recordHeight As String = CType((listCount * 45), String)
            Dim recordHeight2 As String = CType(((listCount - 1) * 45 / 2), String)
            CType(inWarningInfoArea.FindControl("WarningInfoRow"), HtmlContainerControl).Attributes("style") = "height:" + recordHeight + "px"
            CType(inWarningInfoArea.FindControl("NumberRecord"), HtmlContainerControl).Attributes("style") = "height:" + recordHeight + "px"
            CType(inWarningInfoArea.FindControl("DateRecord"), HtmlContainerControl).Attributes("style") = "height:" + recordHeight + "px"
            CType(inWarningInfoArea.FindControl("MileageRecord"), HtmlContainerControl).Attributes("style") = "height:" + recordHeight + "px"
            CType(inWarningInfoArea.FindControl("ISRecord"), HtmlContainerControl).Attributes("style") = "height:" + recordHeight + "px"
            CType(inWarningInfoArea.FindControl("CustomerRecord"), HtmlContainerControl).Attributes("style") = "height:" + recordHeight + "px"
            CType(inWarningInfoArea.FindControl("InformationRecord"), HtmlContainerControl).Attributes("style") = "height:" + recordHeight + "px"

            CType(inWarningInfoArea.FindControl("lblNumberRecord"), CustomLabel).Attributes("style") = "padding-top:" + recordHeight2 + "px"
            CType(inWarningInfoArea.FindControl("lblDateRecord"), CustomLabel).Attributes("style") = "padding-top:" + recordHeight2 + "px"
            CType(inWarningInfoArea.FindControl("lblMileageRecord"), CustomLabel).Attributes("style") = "padding-top:" + recordHeight2 + "px"
            CType(inWarningInfoArea.FindControl("lblISRecord"), CustomLabel).Attributes("style") = "padding-top:" + recordHeight2 + "px"
            CType(inWarningInfoArea.FindControl("lblCustomerRecord"), CustomLabel).Attributes("style") = "padding-top:" + recordHeight2 + "px"

        End If

        If 1 < CType(inWarningInfoArea.FindControl("DetailButtonRepeater"), Repeater).Items.Count Then
            '複数行がある場合、行の高さを調整
            Dim recordHeight As String = CType((listCount * 45), String)
            Dim recordHeight2 As String = CType(((listCount - 1) * 45 / 2), String)
            CType(inWarningInfoArea.FindControl("DetailButtonRecord"), HtmlContainerControl).Attributes("style") = "height:" + recordHeight + "px"

        End If

        If (0 = listCount) Then
            '詳細を取得できない場合、０を返却する
            Logger.Info(String.Format(CultureInfo.CurrentCulture _
                        , "{0}.{1} Return:0 END" _
                        , Me.GetType.ToString _
                        , System.Reflection.MethodBase.GetCurrentMethod.Name))
            Return 0

        End If

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} Return:{2} END" _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name _
                    , (listCount - 1).ToString(CultureInfo.CurrentCulture)))

        '一覧の行高の計算数（詳細件数-1）を返却
        Return listCount - 1

    End Function

#End Region

#Region "内部メソッド"

    ''' <summary>
    ''' Json文変換処理
    ''' </summary>
    ''' <param name="inData">変換元DataTable</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function ToJson(ByVal inData As SC3240601GraphJsonDataTable) As String
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} START: inData:[{2}]" _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name _
                    , inData))

        Dim jss As New JavaScriptSerializer()
        Dim array As New ArrayList()

        'グラフデータLoop
        For m As Integer = 0 To inData.Rows.Count - 1

            'ROW取得
            Dim row As SC3240601GraphJsonRow = CType(inData(m), SC3240601GraphJsonRow)

            Dim dict As New Dictionary(Of String, Object)

            If String.IsNullOrEmpty(row.LINECOLOR) Then
                'グラフ区間以外のデータは、日付以外の値を作らない
                'リストに追加する
                dict.Add(inData.REGDATEColumn.Caption, row.REGDATE)
            Else
                '列ごとに追加
                dict.Add(inData.REGDATEColumn.Caption, row.REGDATE)
                dict.Add(inData.REGMILEColumn.Caption, row.REGMILE)
                dict.Add(inData.DISPREGMILEColumn.Caption, row.DISPREGMILE)
                dict.Add(inData.DESCRIPTIONColumn.Caption, row.DESCRIPTION)
                dict.Add(inData.LINECOLORColumn.Caption, row.LINECOLOR)
            End If

            'リストに追加する
            array.Add(dict)

        Next

        'Jsonデータ変換処理
        Dim retJson As String = jss.Serialize(array)

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} Return Json:{0} END" _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name, retJson))

        '変換後データを返却
        Return retJson

    End Function

#End Region

End Class

