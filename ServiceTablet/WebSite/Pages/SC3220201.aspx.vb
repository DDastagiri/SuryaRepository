'------------------------------------------------------------------------------
'SC3220201.aspx.vb
'------------------------------------------------------------------------------
'機能：SAマネジメント全体管理
'補足：
'作成： 2013/02/28 TMEJ 小澤
'更新： 2013/06/03 TMEJ 河原 IT9513_【A.STEP2】次世代e-CRBタブレット　新DB適応に向けた機能設計
'更新： 2013/12/10 TMEJ 河原 TMEJ次世代サービス 工程管理機能開発
'更新： 2014/07/01 TMEJ 丁　 TMT_UAT対応
'更新： 2017/09/14 NSK  竹中(悠) REQ-SVT-TMT-20170227-002 見込納車遅れ日時計算ロジック統一 
'更新： 2018/06/29 NSK 可児 TKM Next Gen e-CRB Project Application development Block B-1  Pマーク、Lマークなどを表示
'更新：
'------------------------------------------------------------------------------
Option Strict On
Option Explicit On

Imports System.Web.Services
Imports System.Web.Script.Serialization
Imports System.Globalization
Imports System.Data
Imports Toyota.eCRB.SystemFrameworks.Core

Imports Toyota.eCRB.SMBLinkage.GetUserList.Api.BizLogic
Imports Toyota.eCRB.SMBLinkage.GetUserList.Api.DataAccess

Imports Toyota.eCRB.CommonUtility.SMBCommonClass.Api.BizLogic
Imports Toyota.eCRB.CommonUtility.SMBCommonClass.Api.DataAccess
Imports Toyota.eCRB.SMBLinkage.GetServiceLT.Api.DataAccess.IC3810701DataSet

Imports Toyota.eCRB.DMSLinkage.CompleteCheck.DataAccess.SC3220201DataSet
Imports Toyota.eCRB.SystemFrameworks.Web.Controls
Imports Toyota.eCRB.DMSLinkage.CompleteCheck.BizLogic

'2013/12/10 TMEJ 河原 TMEJ次世代サービス 工程管理機能開発 START

Imports Toyota.eCRB.CommonUtility.ServiceCommonClass.Api.DataAccess

'2013/12/10 TMEJ 河原 TMEJ次世代サービス 工程管理機能開発 END


Partial Class Pages_Default
    Inherits BasePage

#Region "定数"

    ''' <summary>
    ''' 画面ID
    ''' </summary>
    ''' <remarks></remarks>
    Private Const APPLICATION_ID As String = "SC3220201"

    ''' <summary>
    ''' SAのオンラインステータス:オンライン
    ''' </summary>
    ''' <remarks></remarks>
    Private Const SA_ONLINESTATE_ONLINE As String = "1"
    ''' <summary>
    ''' SAのオンラインステータス:退席中
    ''' </summary>
    ''' <remarks></remarks>
    Private Const SA_ONLINESTATE_AWAY As String = "2"
    ''' <summary>
    ''' SAのオンラインステータス:オフライン
    ''' </summary>
    ''' <remarks></remarks>
    Private Const SA_ONLINESTATE_OFFLINE As String = "3"
    ''' <summary>
    ''' チップの工程ステータス:受付中
    ''' </summary>
    ''' <remarks></remarks>
    Private Const CHIP_PROGRESSSTATE_RECEPTION As Integer = 1
    ''' <summary>
    ''' チップの工程ステータス:追加作業
    ''' </summary>
    ''' <remarks></remarks>
    Private Const CHIP_PROGRESSSTATE_ADDITION_WORKING As Integer = 2

    ''' <summary>
    ''' チップの工程ステータス:洗車・納車準備
    ''' </summary>
    ''' <remarks></remarks>
    Private Const CHIP_PROGRESSSTATE_PREPARATION_DELIVERY As Integer = 3
    ''' <summary>
    ''' チップの工程ステータス:納車作業
    ''' </summary>
    ''' <remarks></remarks>
    Private Const CHIP_PROGRESSSTATE_DELIVERY As Integer = 4
    ''' <summary>
    ''' チップの工程ステータス:作業中
    ''' </summary>
    ''' <remarks></remarks>
    Private Const CHIP_PROGRESSSTATE_WORKING As Integer = 5
    ''' <summary>
    ''' チップの工程ステータス:来店(受付待ち)
    ''' </summary>
    ''' <remarks></remarks>
    Private Const CHIP_PROGRESSSTATE_RECEPTION_WAIT As Integer = 6

    ''' <summary>
    ''' フッターコード：メインメニュー
    ''' </summary>
    ''' <remarks></remarks>
    Private Const FOOTER_MAINMENU As Integer = 100
    ''' <summary>
    ''' フッターコード：顧客詳細
    ''' </summary>
    ''' <remarks></remarks>
    Private Const FOOTER_CUSTOMER As Integer = 200
    ''' <summary>
    ''' フッターコード：R/Oボタン
    ''' </summary>
    ''' <remarks></remarks>
    Private Const FOOTER_RO As Integer = 600
    ''' <summary>
    ''' フッターコード：追加作業ボタン
    ''' </summary>
    ''' <remarks></remarks>
    Private Const FOOTER_ADD_LIST As Integer = 1100
    ''' <summary>
    ''' フッターコード：スケジューラ
    ''' </summary>
    ''' <remarks></remarks>
    Private Const FOOTER_SCHEDULE As Integer = 400
    ''' <summary>
    ''' フッターコード：電話帳
    ''' </summary>
    ''' <remarks></remarks>
    Private Const FOOTER_TEL_DIRECTORY As Integer = 500

    '2013/06/03 TMEJ 河原 IT9513_【A.STEP2】次世代e-CRBタブレット　新DB適応に向けた機能設計 START
    ''' <summary>
    ''' フッターコード：SMB
    ''' </summary>
    ''' <remarks></remarks>
    Private Const FOOTER_SMB As Integer = 800
    '2013/06/03 TMEJ 河原 IT9513_【A.STEP2】次世代e-CRBタブレット　新DB適応に向けた機能設計 END

    ''' <summary>
    ''' メインメニュー(SM)画面ID
    ''' </summary>
    ''' <remarks></remarks>
    Private Const MAINMENU_ID As String = "SC3220101"
    ''' <summary>
    ''' R/O一覧画面ID
    ''' </summary>
    ''' <remarks></remarks>
    Private Const REPAIR_ORDERE_LIST_PAGE As String = "SC3160101"
    ''' <summary>
    ''' 追加作業画面ID
    ''' </summary>
    ''' <remarks></remarks>
    Private Const ADDITION_WORK_LIST_PAGE As String = "SC3170101"
    ''' <summary>
    ''' 来店管理画面ID
    ''' </summary>
    ''' <remarks></remarks>
    Private Const VISIT_MANAGEMENT_LIST_PAGE As String = "SC3100303"

    ''' <summary>
    ''' スペース
    ''' </summary>
    ''' <remarks></remarks>
    Private Const C_DEFAULT_CHIP_SPACE As String = "&nbsp;"

    ''' <summary>
    ''' 作業中チップの左位置の初期値
    ''' </summary>
    ''' <remarks></remarks>
    Private Const LeftPading As Long = 12
    ''' <summary>
    ''' 作業中チップの左位置の加算値
    ''' </summary>
    ''' <remarks></remarks>
    Private Const AddLeftPading As Long = 99

    ''' <summary>
    ''' FromToフラグ「頭」
    ''' </summary>
    ''' <remarks></remarks>
    Private Const FROMTO_FROM As String = "0"
    ''' <summary>
    ''' FromToフラグ「後ろ」
    ''' </summary>
    ''' <remarks></remarks>
    Private Const FROMTO_TO As String = "1"

    ''' <summary>
    ''' 遅れ「赤枠」
    ''' </summary>
    ''' <remarks></remarks>
    Private Const CHIP_BORDER_RED As String = "ColumnBoxBorderRed"
    ''' <summary>
    ''' 遅見込「黄枠」
    ''' </summary>
    ''' <remarks></remarks>
    Private Const CHIP_BORDER_YELLOW As String = "ColumnBoxBorderYellow"

    ''' <summary>
    ''' 仕掛け前「白チップ」
    ''' </summary>
    ''' <remarks></remarks>
    Private Const CHIP_COLOR_NORMAL As String = "ColumnContents02BoderIn"
    ''' <summary>
    ''' 仕掛け中「水色チップ」
    ''' </summary>
    ''' <remarks></remarks>
    Private Const CHIP_COLOR_AQUA As String = "ColumnContents02BoderIn ColumnBoxAqua"

    ''' <summary>
    ''' 作業エリアの背景色設定の件数
    ''' </summary>
    ''' <remarks></remarks>
    Private Const CHIP_WORK_COUNT As Long = 30
    ''' <summary>
    ''' 背景色の設定
    ''' </summary>
    ''' <remarks></remarks>
    Private Const CHIP_BACK_IMAGE As String = "chipsArea maxChipArea"

    ''' <summary>
    ''' 予約フラグ有無:無し
    ''' </summary>
    ''' <remarks></remarks>
    Private Const CHIP_RESERVE_NONE As String = "0"
    ''' <summary>
    ''' iフラグ有無:無し
    ''' </summary>
    ''' <remarks></remarks>
    Private Const CHIP_I_NONE As String = "0"
    '2018/06/29 NSK 可児 TKM Next Gen e-CRB Project Application development Block B-1  Pマーク、Lマークなどを表示 START
    ''' <summary>
    ''' アイコンフラグ1（1：M/E/T/P表示）
    ''' </summary>
    ''' <remarks></remarks>
    Private Const ICON_FLAG_1 As String = "1"
    ''' <summary>
    ''' アイコンフラグ2（2：B/L表示）
    ''' </summary>
    ''' <remarks></remarks>
    Private Const ICON_FLAG_2 As String = "2"
    '2018/06/29 NSK 可児 TKM Next Gen e-CRB Project Application development Block B-1  Pマーク、Lマークなどを表示 END
    ''' <summary>
    ''' Sフラグ有無:無し
    ''' </summary>
    ''' <remarks></remarks>
    Private Const CHIP_S_NONE As String = "0"
    ''' <summary>
    ''' 洗車有無:無し
    ''' </summary>
    ''' <remarks></remarks>
    Private Const CHIP_WASHING_NONE As String = "0"
    ''' <summary>
    ''' 仕掛中有無:無し
    ''' </summary>
    ''' <remarks></remarks>
    Private Const CHIP_DEVISES_NONE As String = "0"

    ''' <summary>
    ''' 追加作業アイコン非表示
    ''' </summary>
    ''' <remarks></remarks>
    Private Const CHIP_ADDWORK_DISP_NONE As String = "ColumnTextBox"
    ''' <summary>
    ''' 追加作業アイコン表示
    ''' </summary>
    ''' <remarks></remarks>
    Private Const CHIP_ADDWORK_DISP As String = "ColumnTextBox Icn01"

    ''' <summary>
    ''' 作業エリアのチップ位置の置換用文字列
    ''' </summary>
    ''' <remarks></remarks>
    Private Const CHIP_REPLACE_LEFT_PADDING As String = "left:{0}px;"

    ''' <summary>
    ''' フッターイベントの置換用文字列
    ''' </summary>
    ''' <remarks></remarks>
    Private Const FOOTER_REPLACE_EVENT As String = "FooterButtonClick({0});"

    ''' <summary>
    ''' スケジューラボタンのイベント
    ''' </summary>
    ''' <remarks></remarks>
    Private Const FOOTER_EVENT_SCHEDULER As String = "return schedule.appExecute.executeCaleNew();"
    ''' <summary>
    ''' 電話帳ボタンのイベント
    ''' </summary>
    ''' <remarks></remarks>
    Private Const FOOTER_EVENT_TEL As String = "return schedule.appExecute.executeCont();"

    '2013/06/03 TMEJ 河原 IT9513_【A.STEP2】次世代e-CRBタブレット　新DB適応に向けた機能設計 START
    ''' <summary>
    ''' 工程管理画面ID
    ''' </summary>
    ''' <remarks></remarks>
    Private Const PROCESS_CONTROL_PAGE As String = "SC3240101"
    '2013/06/03 TMEJ 河原 IT9513_【A.STEP2】次世代e-CRBタブレット　新DB適応に向けた機能設計 END

    '2013/12/10 TMEJ 河原 TMEJ次世代サービス 工程管理機能開発 START

    ''' <summary>
    ''' 未振当て一覧
    ''' </summary>
    ''' <remarks></remarks>
    Private Const ASSIGN_LIST_PAGE As String = "SC3100401"

    ''' <summary>
    ''' 編集モードフラグ("0"；編集) 
    ''' </summary>
    Private Const EditMode As String = "0"

    ''' <summary>
    ''' 編集モードフラグ("1"；リードオンリー) 
    ''' </summary>
    Private Const ReadMode As String = "1"

    ''' <summary>
    ''' プレビューフラグ("0"；プレビュー) 
    ''' </summary>
    Private Const PreviewFlag As String = "0"

    ''' <summary>親のRO作業連番編集(0) 
    ''' </summary>
    Private Const ParentJobSeq As String = "0"

    ''' <summary>
    ''' 基幹画面連携用フレームID("SC3010501")
    ''' </summary>
    Private Const APPLICATIONID_FRAMEID As String = "SC3010501"

    ''' <summary>
    ''' 基幹画面連携用フレーム用セッション名("Session.Param1")
    ''' </summary>
    Private Const SessionParam01 As String = "Session.Param1"

    ''' <summary>
    ''' 基幹画面連携用フレーム用セッション名("Session.Param2")
    ''' </summary>
    Private Const SessionParam02 As String = "Session.Param2"

    ''' <summary>
    ''' 基幹画面連携用フレーム用セッション名("Session.Param3")
    ''' </summary>
    Private Const SessionParam03 As String = "Session.Param3"

    ''' <summary>
    ''' 基幹画面連携用フレーム用セッション名("Session.Param4")
    ''' </summary>
    Private Const SessionParam04 As String = "Session.Param4"

    ''' <summary>
    ''' 基幹画面連携用フレーム用セッション名("Session.Param5")
    ''' </summary>
    Private Const SessionParam05 As String = "Session.Param5"

    ''' <summary>
    ''' 基幹画面連携用フレーム用セッション名("Session.Param6")
    ''' </summary>
    Private Const SessionParam06 As String = "Session.Param6"

    ''' <summary>
    ''' 基幹画面連携用フレーム用セッション名("Session.Param7")
    ''' </summary>
    Private Const SessionParam07 As String = "Session.Param7"

    ''' <summary>
    ''' 基幹画面連携用フレーム用セッション名("Session.Param8")
    ''' </summary>
    Private Const SessionParam08 As String = "Session.Param8"

    ''' <summary>
    ''' 基幹画面連携用フレーム用セッション名("Session.Param9")
    ''' </summary>
    Private Const SessionParam09 As String = "Session.Param9"

    ''' <summary>
    ''' 基幹画面連携用フレーム用セッション名("Session.Param10")
    ''' </summary>
    Private Const SessionParam10 As String = "Session.Param10"

    ''' <summary>
    ''' 基幹画面連携用フレーム用セッション名("Session.DISP_NUM")
    ''' </summary>
    Private Const SessionDispNum As String = "Session.DISP_NUM"

    ''' <summary>
    ''' 顧客詳細画面用セッション名("SessionKey.DMS_CST_ID")
    ''' </summary>
    Private Const SessionDMSID As String = "SessionKey.DMS_CST_ID"

    ''' <summary>
    ''' 顧客詳細画面用セッション名("SessionKey.VIN")
    ''' </summary>
    Private Const SessionVIN As String = "SessionKey.VIN"

    ''' <summary>
    ''' セッション名("DealerCode")
    ''' </summary>
    Private Const SessionDealerCode As String = "DealerCode"

    ''' <summary>
    ''' セッション名("BranchCode")
    ''' </summary>
    Private Const SessionBranchCode As String = "BranchCode"

    ''' <summary>
    ''' セッション名("LoginUserID")
    ''' </summary>
    Private Const SessionLoginUserID As String = "LoginUserID"

    ''' <summary>
    ''' セッション名("SAChipID")
    ''' </summary>
    Private Const SessionSAChipID As String = "SAChipID"

    ''' <summary>
    ''' セッション名("BASREZID")
    ''' </summary>
    Private Const SessionBASREZID As String = "BASREZID"

    ''' <summary>
    ''' セッション名("R_O")
    ''' </summary>
    Private Const SessionRO As String = "R_O"

    ''' <summary>
    ''' セッション名("SEQ_NO")
    ''' </summary>
    Private Const SessionSEQNO As String = "SEQ_NO"

    ''' <summary>
    ''' セッション名("VIN_NO")
    ''' </summary>
    Private Const SessionVINNO As String = "VIN_NO"

    ''' <summary>
    ''' セッション名("ViewMode")
    ''' </summary>
    Private Const SessionViewMode As String = "ViewMode"

    ''' <summary>
    ''' R/O作成・R/O編集画面(DISP_NUM:"1")
    ''' </summary>
    Private Const APPLICATIONID_ORDERNEW As String = "1"

    ''' <summary>
    ''' 追加作業起票・追加作業編集画面(DISP_NUM:"6")
    ''' </summary>
    Private Const APPLICATIONID_WORK As String = "6"

    ''' <summary>
    ''' R/O参照画面(DISP_NUM:"13")
    ''' </summary>
    Private Const APPLICATIONID_ORDEROUT As String = "13"

    ''' <summary>
    ''' 顧客詳細画面("SC3080225")
    ''' </summary>
    Private Const APPLICATIONID_CUSTOMEROUT As String = "SC3080225"

    ''' <summary>
    ''' R/O一覧画面(DISP_NUM:"14")
    ''' </summary>
    Private Const APPLICATIONID_ORDERLIST As String = "14"

    ''' <summary>
    ''' 商品訴求コンテンツ画面("SC3250101")
    ''' </summary>
    Private Const APPLICATIONID_PRODUCTSAPPEALCONTENT As String = "SC3250101"

    ''' <summary>
    ''' キャンペーン画面(DISP_NUM:"15")
    ''' </summary>
    Private Const APPLICATIONID_CAMPAIGN As String = "15"


#End Region

#Region "列挙対"

    ''' <summary>
    ''' 文言ID
    ''' </summary>
    Private Enum WordID
        ''' <summary>なし</summary>
        id000 = 0
        ''' <summary>全体管理</summary>
        id001 = 1
        ''' <summary>予約</summary>
        id002 = 2
        ''' <summary>受付</summary>
        id003 = 3
        ''' <summary>作業</summary>
        id004 = 4
        ''' <summary>洗車/精算</summary>
        id005 = 5
        ''' <summary>納車</summary>
        id006 = 6
        ''' <summary>来店管理</summary>
        id007 = 7
        ''' <summary>全体管理</summary>
        id008 = 8
        ''' <summary>予</summary>
        id009 = 9
        ''' <summary>i</summary>
        id010 = 10
        ''' <summary>S</summary>
        id011 = 11
        ''' <summary>～</summary>
        id012 = 12
        ''' <summary>--:--</summary>
        id013 = 13
        '2018/06/29 NSK 可児 TKM Next Gen e-CRB Project Application development Block B-1  Pマーク、Lマークなどを表示 START
        ''' <summary>M</summary>
        id10001 = 10001
        ''' <summary>B</summary>
        id10002 = 10002
        ''' <summary>E</summary>
        id10003 = 10003
        ''' <summary>T</summary>
        id10004 = 10004
        ''' <summary>P</summary>
        id10005 = 10005
        ''' <summary>L</summary>
        id10006 = 10006
        '2018/06/29 NSK 可児 TKM Next Gen e-CRB Project Application development Block B-1  Pマーク、Lマークなどを表示 END
        ''' <summary>データベースへのアクセスにてタイムアウトが発生しました。再度実行して下さい。</summary>
        id901 = 901

        '2013/12/10 TMEJ 河原 TMEJ次世代サービス 工程管理機能開発 START
        ''' <summary>予期せぬエラー</summary>
        id909 = 909
        '2013/12/10 TMEJ 河原 TMEJ次世代サービス 工程管理機能開発 END

    End Enum

#End Region

#Region "変数"

    ''' <summary>
    ''' ユーザ情報（セッションより）
    ''' </summary>
    ''' <remarks></remarks>
    Private objStaffContext As StaffContext

    ''' <summary>
    ''' 現在時刻
    ''' </summary>
    ''' <remarks></remarks>
    Private nowDateTime As Date

    ''' <summary>
    ''' 納車準備_異常表示標準時間（分）
    ''' </summary>
    ''' <remarks></remarks>
    Private deliverypreAbnormalLt As Long = 0

    ''' <summary>
    ''' ～ 文言
    ''' </summary>
    ''' <remarks></remarks>
    Private wordFromTo As String = ""

    ''' <summary>
    ''' 予約マーク 文言
    ''' </summary>
    ''' <remarks></remarks>
    Private wordRightIcnD As String = ""

    ''' <summary>
    ''' JDP調査対象客マーク 文言
    ''' </summary>
    ''' <remarks></remarks>
    Private wordRightIcnI As String = ""

    ''' <summary>
    ''' SSCマーク 文言
    ''' </summary>
    ''' <remarks></remarks>
    Private wordRightIcnS As String = ""

    ''' <summary>
    ''' --:-- 文言
    ''' </summary>
    ''' <remarks></remarks>
    Private wordNoTime As String = ""

    '2018/06/29 NSK 可児 TKM Next Gen e-CRB Project Application development Block B-1  Pマーク、Lマークなどを表示 START
    ''' <summary>
    ''' Mマーク文言
    ''' </summary>
    ''' <remarks></remarks>
    Private wordRightIcnM As String = ""
    ''' <summary>
    ''' Bマーク文言
    ''' </summary>
    ''' <remarks></remarks>
    Private wordRightIcnB As String = ""
    ''' <summary>
    ''' Eマーク文言
    ''' </summary>
    ''' <remarks></remarks>
    Private wordRightIcnE As String = ""
    ''' <summary>
    ''' Tマーク文言
    ''' </summary>
    ''' <remarks></remarks>
    Private wordRightIcnT As String = ""
    ''' <summary>
    ''' Pマーク文言
    ''' </summary>
    ''' <remarks></remarks>
    Private wordRightIcnP As String = ""

    ''' <summary>
    ''' Lマーク文言
    ''' </summary>
    ''' <remarks></remarks>
    Private wordRightIcnL As String = ""
    '2018/06/29 NSK 可児 TKM Next Gen e-CRB Project Application development Block B-1  Pマーク、Lマークなどを表示 END
#End Region

#Region "初期処理"

    ''' <summary>
    ''' ページロード時の処理
    ''' </summary>
    ''' <param name="sender">イベント発生元</param>
    ''' <param name="e">イベントデータ</param>
    ''' <remarks></remarks>
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} START" _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name))

        'ユーザ情報の取得.
        objStaffContext = StaffContext.Current

        'フッタボタンの初期化を行う.
        InitFooterButton()

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} END" _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name))
    End Sub

    ''' <summary>
    ''' 表示処理
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub MainAreaReload_Click(sender As Object, e As System.EventArgs) Handles MainAreaReload.Click
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} START" _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name))

        '表示処理
        Me.InitVisitChip()

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} END" _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name))
    End Sub

    '''-----------------------------------------------------------------------
    ''' <summary>
    ''' チップ情報取得
    ''' </summary>
    ''' <remarks></remarks>
    '''-----------------------------------------------------------------------
    Private Sub InitVisitChip()
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} START" _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name))

        ' 現在時刻取得
        Dim staffInfo As StaffContext = StaffContext.Current
        Me.nowDateTime = DateTimeFunc.Now(staffInfo.DlrCD)

        Using bl As New SC3220201BusinessLogic(Me.deliverypreAbnormalLt, Me.nowDateTime)
            Try
                'サービス標準LT取得
                Dim dtStanderdLt As StandardLTListDataTable = bl.GetStandardLTList(staffInfo.DlrCD, _
                                                                                   staffInfo.BrnCD)

                ' チップ情報取得
                Dim dt As SC3220201VisitChipDataTable = bl.GetVisitChip()

                'アイコンの固定文字列取得
                Me.wordFromTo = WebWordUtility.GetWord(APPLICATION_ID, WordID.id012)
                Me.wordRightIcnD = WebWordUtility.GetWord(APPLICATION_ID, WordID.id009)
                Me.wordRightIcnI = WebWordUtility.GetWord(APPLICATION_ID, WordID.id010)
                Me.wordRightIcnS = WebWordUtility.GetWord(APPLICATION_ID, WordID.id011)
                Me.wordNoTime = WebWordUtility.GetWord(APPLICATION_ID, WordID.id013)
                '2018/06/29 NSK 可児 TKM Next Gen e-CRB Project Application development Block B-1  Pマーク、Lマークなどを表示 START
                Me.wordRightIcnM = WebWordUtility.GetWord(APPLICATION_ID, WordID.id10001)
                Me.wordRightIcnB = WebWordUtility.GetWord(APPLICATION_ID, WordID.id10002)
                Me.wordRightIcnE = WebWordUtility.GetWord(APPLICATION_ID, WordID.id10003)
                Me.wordRightIcnT = WebWordUtility.GetWord(APPLICATION_ID, WordID.id10004)
                Me.wordRightIcnP = WebWordUtility.GetWord(APPLICATION_ID, WordID.id10005)
                Me.wordRightIcnL = WebWordUtility.GetWord(APPLICATION_ID, WordID.id10006)
                '2018/06/29 NSK 可児 TKM Next Gen e-CRB Project Application development Block B-1  Pマーク、Lマークなどを表示 END

                '予約エリアチップ初期設定
                Me.SetInitReserveData(dt, dtStanderdLt)

                '受付エリアチップ初期設定
                Me.SetInitReceptionistData(dt, dtStanderdLt)

                '作業中エリアチップ初期設定
                Me.SetInitWorkData(dt, dtStanderdLt)

                '洗車/精算エリアチップ初期設定
                Me.SetInitWashData(dt)

                '納車エリアチップ初期設定
                Me.SetInitDeliveryData(dt)

                'エリアの更新
                Me.ContentUpdateMainPanel.Update()

            Catch ex As OracleExceptionEx When ex.Number = 1013
                'ORACLEのタイムアウトのみ処理
                Me.ShowMessageBox(WordID.id901)
            End Try
        End Using

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} END" _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name))
    End Sub

    ''' <summary>
    ''' 予約エリアチップ初期設定
    ''' </summary>
    ''' <param name="dt">チップ情報</param>
    ''' <remarks></remarks>
    Private Sub SetInitReserveData(ByVal dt As SC3220201VisitChipDataTable, _
                                   ByVal dtStanderdLt As StandardLTListDataTable)
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} START" _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name))

        ' コントロールにバインドする
        Me.ReserveAreaRepeater.DataSource = dt.Select(String.Format(CultureInfo.CurrentCulture, _
                                                                    "DISP_DIV = '{0}'", _
                                                                    SC3220201BusinessLogic.DisplayDivReserve), "DISP_SORT")
        Me.ReserveAreaRepeater.DataBind()

        Dim rowList As SC3220201VisitChipRow() = _
            DirectCast(Me.ReserveAreaRepeater.DataSource, SC3220201VisitChipRow())

        'データを設定する
        For i = 0 To Me.ReserveAreaRepeater.Items.Count - 1

            Dim ReserveAreaControl As Control = Me.ReserveAreaRepeater.Items(i)
            Dim drSC3220201VisitChip As SC3220201VisitChipRow = rowList(i)

            '予約フラグ
            If Me.SetNullToString(drSC3220201VisitChip.REZ_MARK, CHIP_RESERVE_NONE).Equals(CHIP_RESERVE_NONE) Then
                CType(ReserveAreaControl.FindControl("RightIcnD"), HtmlControl).Visible = False
            Else
                CType(ReserveAreaControl.FindControl("RightIcnD"), HtmlControl).Visible = True
                CType(ReserveAreaControl.FindControl("ReserveAreaReserveIcon"), CustomLabel).Text = Me.wordRightIcnD
            End If
            '2018/06/29 NSK 可児 TKM Next Gen e-CRB Project Application development Block B-1  Pマーク、Lマークなどを表示 START
            ''iフラグ
            'If Me.SetNullToString(drSC3220201VisitChip.JDP_MARK, CHIP_I_NONE).Equals(CHIP_I_NONE) Then
            '    CType(ReserveAreaControl.FindControl("RightIcnI"), HtmlControl).Visible = False
            'Else
            '    CType(ReserveAreaControl.FindControl("RightIcnI"), HtmlControl).Visible = True
            '    CType(ReserveAreaControl.FindControl("ReserveAreaIIcon"), CustomLabel).Text = Me.wordRightIcnI
            'P or Lフラグ
            If ICON_FLAG_1.Equals(drSC3220201VisitChip.JDP_MARK) Then
                CType(ReserveAreaControl.FindControl("RightIcnL"), HtmlControl).Visible = False
                CType(ReserveAreaControl.FindControl("RightIcnP"), HtmlControl).Visible = True
                CType(ReserveAreaControl.FindControl("ReserveAreaPIcon"), CustomLabel).Text = Me.wordRightIcnP
            ElseIf ICON_FLAG_2.Equals(drSC3220201VisitChip.JDP_MARK) Then
                CType(ReserveAreaControl.FindControl("RightIcnP"), HtmlControl).Visible = False
                CType(ReserveAreaControl.FindControl("RightIcnL"), HtmlControl).Visible = True
                CType(ReserveAreaControl.FindControl("ReserveAreaLIcon"), CustomLabel).Text = Me.wordRightIcnL
            Else
                CType(ReserveAreaControl.FindControl("RightIcnP"), HtmlControl).Visible = False
                CType(ReserveAreaControl.FindControl("RightIcnL"), HtmlControl).Visible = False
            End If
            'M or Bフラグ
            If ICON_FLAG_1.Equals(drSC3220201VisitChip.SML_AMC_FLG) Then
                CType(ReserveAreaControl.FindControl("RightIcnB"), HtmlControl).Visible = False
                CType(ReserveAreaControl.FindControl("RightIcnM"), HtmlControl).Visible = True
                CType(ReserveAreaControl.FindControl("ReserveAreaMIcon"), CustomLabel).Text = Me.wordRightIcnM
            ElseIf ICON_FLAG_2.Equals(drSC3220201VisitChip.SML_AMC_FLG) Then
                CType(ReserveAreaControl.FindControl("RightIcnM"), HtmlControl).Visible = False
                CType(ReserveAreaControl.FindControl("RightIcnB"), HtmlControl).Visible = True
                CType(ReserveAreaControl.FindControl("ReserveAreaBIcon"), CustomLabel).Text = Me.wordRightIcnB
            Else
                CType(ReserveAreaControl.FindControl("RightIcnM"), HtmlControl).Visible = False
                CType(ReserveAreaControl.FindControl("RightIcnB"), HtmlControl).Visible = False
            End If
            'Eフラグ
            If ICON_FLAG_1.Equals(drSC3220201VisitChip.EW_FLG) Then
                CType(ReserveAreaControl.FindControl("RightIcnE"), HtmlControl).Visible = True
                CType(ReserveAreaControl.FindControl("ReserveAreaEIcon"), CustomLabel).Text = Me.wordRightIcnE
            Else
                CType(ReserveAreaControl.FindControl("RightIcnE"), HtmlControl).Visible = False
            End If
            'Tフラグ
            If ICON_FLAG_1.Equals(drSC3220201VisitChip.TLM_MBR_FLG) Then
                CType(ReserveAreaControl.FindControl("RightIcnT"), HtmlControl).Visible = True
                CType(ReserveAreaControl.FindControl("ReserveAreaTIcon"), CustomLabel).Text = Me.wordRightIcnT
            Else
                CType(ReserveAreaControl.FindControl("RightIcnT"), HtmlControl).Visible = False
            End If
            '2018/06/29 NSK 可児 TKM Next Gen e-CRB Project Application development Block B-1  Pマーク、Lマークなどを表示 END

            'Sフラグ
            If Me.SetNullToString(drSC3220201VisitChip.SSC_MARK, CHIP_S_NONE).Equals(CHIP_S_NONE) Then
                CType(ReserveAreaControl.FindControl("RightIcnS"), HtmlControl).Visible = False
            Else
                CType(ReserveAreaControl.FindControl("RightIcnS"), HtmlControl).Visible = True
                CType(ReserveAreaControl.FindControl("ReserveAreaSIcon"), CustomLabel).Text = Me.wordRightIcnS
            End If

            '車両No
            CType(ReserveAreaControl.FindControl("ReserveAreaVclNo"), CustomLabel).Text = _
                Me.SetNullToString(drSC3220201VisitChip.VCLREGNO, C_DEFAULT_CHIP_SPACE)

            '名前
            CType(ReserveAreaControl.FindControl("ReserveAreaName"), CustomLabel).Text = _
                Me.SetNullToString(drSC3220201VisitChip.CUSTOMERNAME, C_DEFAULT_CHIP_SPACE)

            '表示日時
            CType(ReserveAreaControl.FindControl("ReserveAreaDeliveryDate"), CustomLabel).Text = _
                Me.SetDateTimeToString(drSC3220201VisitChip.ITEM_DATE, wordFromTo, FROMTO_TO)

            '代表整備項目
            CType(ReserveAreaControl.FindControl("ReserveAreaFixitem"), CustomLabel).Text = _
                Me.SetNullToString(drSC3220201VisitChip.MERCHANDISENAME, C_DEFAULT_CHIP_SPACE)

            'チップ色設定
            With CType(ReserveAreaControl.FindControl("chip"), HtmlContainerControl)
                ' 仕掛中チェック
                If drSC3220201VisitChip.VISITTIMESTAMP = Date.MinValue Then
                    .Attributes("class") = CHIP_COLOR_NORMAL
                    '遅れチェック
                    With CType(ReserveAreaControl.FindControl("ChipBorder"), HtmlContainerControl)
                        If nowDateTime >= drSC3220201VisitChip.PROC_DATE Then
                            .Attributes("class") = CHIP_BORDER_RED
                        End If
                        '判断用のデータ保持
                        .Attributes("chipDate") = drSC3220201VisitChip.PROC_DATE.ToString(CultureInfo.CurrentCulture)
                        .Attributes("delayDate") = Date.MaxValue.ToString(CultureInfo.CurrentCulture)
                    End With
                Else
                    .Attributes("class") = CHIP_COLOR_AQUA
                    '遅れチェック
                    Dim addMinutes As Long = 0
                    If dtStanderdLt IsNot Nothing AndAlso 0 < dtStanderdLt.Rows.Count Then
                        Dim rowStanderdLt As StandardLTListRow = DirectCast(dtStanderdLt.Rows(0), StandardLTListRow)
                        If Not rowStanderdLt.IsRECEPT_GUIDE_STANDARD_LTNull Then
                            addMinutes = rowStanderdLt.RECEPT_GUIDE_STANDARD_LT
                        End If
                    End If
                    With CType(ReserveAreaControl.FindControl("ChipBorder"), HtmlContainerControl)
                        If nowDateTime >= drSC3220201VisitChip.PROC_DATE.AddMinutes(addMinutes) Then
                            .Attributes("class") = CHIP_BORDER_RED
                        End If
                        '判断用のデータ保持
                        .Attributes("chipDate") = drSC3220201VisitChip.PROC_DATE.AddMinutes(addMinutes).ToString(CultureInfo.CurrentCulture)
                        .Attributes("delayDate") = Date.MaxValue.ToString(CultureInfo.CurrentCulture)
                    End With
                End If
            End With
        Next

        'ヘッダーとデータ表示件数を表示する
        Me.ReserveAreaTitle.Text = WebWordUtility.GetWord(APPLICATION_ID, WordID.id002)
        Me.ReserveAreaChipCount.Text = Me.ReserveAreaRepeater.Items.Count.ToString(CultureInfo.CurrentCulture)

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} END" _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name))
    End Sub

    ''' <summary>
    ''' 受付エリアチップ初期設定
    ''' </summary>
    ''' <param name="dt">チップ情報</param>
    ''' <param name="dtStanderdLt">サービス標準LT情報</param>
    ''' <remarks></remarks>
    Private Sub SetInitReceptionistData(ByVal dt As SC3220201VisitChipDataTable, _
                                        ByVal dtStanderdLt As StandardLTListDataTable)
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} START" _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name))

        ' コントロールにバインドする
        Me.ReceptionistAreaRepeater.DataSource = dt.Select(String.Format(CultureInfo.CurrentCulture, _
                                                                        "DISP_DIV = '{0}'", _
                                                                        SC3220201BusinessLogic.DisplayDivReception), "DISP_SORT")
        Me.ReceptionistAreaRepeater.DataBind()

        Dim rowList As SC3220201VisitChipRow() = _
            DirectCast(Me.ReceptionistAreaRepeater.DataSource, SC3220201VisitChipRow())

        'データを設定する
        For i = 0 To Me.ReceptionistAreaRepeater.Items.Count - 1

            Dim ReceptionistAreaControl As Control = Me.ReceptionistAreaRepeater.Items(i)
            Dim drSC3220201VisitChip As SC3220201VisitChipRow = rowList(i)

            '予約フラグ
            If Me.SetNullToString(drSC3220201VisitChip.REZ_MARK, CHIP_RESERVE_NONE).Equals(CHIP_RESERVE_NONE) Then
                CType(ReceptionistAreaControl.FindControl("RightIcnD"), HtmlControl).Visible = False
            Else
                CType(ReceptionistAreaControl.FindControl("RightIcnD"), HtmlControl).Visible = True
                CType(ReceptionistAreaControl.FindControl("ReceptionistAreaReserveIcon"), CustomLabel).Text = Me.wordRightIcnD
            End If

            '2018/06/29 NSK 可児 TKM Next Gen e-CRB Project Application development Block B-1  Pマーク、Lマークなどを表示 START
            ''iフラグ
            'If Me.SetNullToString(drSC3220201VisitChip.JDP_MARK, CHIP_I_NONE).Equals(CHIP_I_NONE) Then
            '    CType(ReceptionistAreaControl.FindControl("RightIcnI"), HtmlControl).Visible = False
            'Else
            '    CType(ReceptionistAreaControl.FindControl("RightIcnI"), HtmlControl).Visible = True
            '    CType(ReceptionistAreaControl.FindControl("ReceptionistAreaIIcon"), CustomLabel).Text = Me.wordRightIcnI
            'End If
            'P or Lフラグ
            If ICON_FLAG_1.Equals(drSC3220201VisitChip.JDP_MARK) Then
                CType(ReceptionistAreaControl.FindControl("RightIcnL"), HtmlControl).Visible = False
                CType(ReceptionistAreaControl.FindControl("RightIcnP"), HtmlControl).Visible = True
                CType(ReceptionistAreaControl.FindControl("ReceptionistAreaPIcon"), CustomLabel).Text = Me.wordRightIcnP
            ElseIf ICON_FLAG_2.Equals(drSC3220201VisitChip.JDP_MARK) Then
                CType(ReceptionistAreaControl.FindControl("RightIcnP"), HtmlControl).Visible = False
                CType(ReceptionistAreaControl.FindControl("RightIcnL"), HtmlControl).Visible = True
                CType(ReceptionistAreaControl.FindControl("ReceptionistAreaLIcon"), CustomLabel).Text = Me.wordRightIcnL
            Else
                CType(ReceptionistAreaControl.FindControl("RightIcnP"), HtmlControl).Visible = False
                CType(ReceptionistAreaControl.FindControl("RightIcnL"), HtmlControl).Visible = False
            End If
            'M or Bフラグ
            If ICON_FLAG_1.Equals(drSC3220201VisitChip.SML_AMC_FLG) Then
                CType(ReceptionistAreaControl.FindControl("RightIcnB"), HtmlControl).Visible = False
                CType(ReceptionistAreaControl.FindControl("RightIcnM"), HtmlControl).Visible = True
                CType(ReceptionistAreaControl.FindControl("ReceptionistAreaMIcon"), CustomLabel).Text = Me.wordRightIcnM
            ElseIf ICON_FLAG_2.Equals(drSC3220201VisitChip.SML_AMC_FLG) Then
                CType(ReceptionistAreaControl.FindControl("RightIcnM"), HtmlControl).Visible = False
                CType(ReceptionistAreaControl.FindControl("RightIcnB"), HtmlControl).Visible = True
                CType(ReceptionistAreaControl.FindControl("ReceptionistAreaBIcon"), CustomLabel).Text = Me.wordRightIcnB
            Else
                CType(ReceptionistAreaControl.FindControl("RightIcnM"), HtmlControl).Visible = False
                CType(ReceptionistAreaControl.FindControl("RightIcnB"), HtmlControl).Visible = False
            End If
            'Eフラグ
            If ICON_FLAG_1.Equals(drSC3220201VisitChip.EW_FLG) Then
                CType(ReceptionistAreaControl.FindControl("RightIcnE"), HtmlControl).Visible = True
                CType(ReceptionistAreaControl.FindControl("ReceptionistAreaEIcon"), CustomLabel).Text = Me.wordRightIcnE
            Else
                CType(ReceptionistAreaControl.FindControl("RightIcnE"), HtmlControl).Visible = False
            End If
            'Tフラグ
            If ICON_FLAG_1.Equals(drSC3220201VisitChip.TLM_MBR_FLG) Then
                CType(ReceptionistAreaControl.FindControl("RightIcnT"), HtmlControl).Visible = True
                CType(ReceptionistAreaControl.FindControl("ReceptionistAreaTIcon"), CustomLabel).Text = Me.wordRightIcnT
            Else
                CType(ReceptionistAreaControl.FindControl("RightIcnT"), HtmlControl).Visible = False
            End If
            '2018/06/29 NSK 可児 TKM Next Gen e-CRB Project Application development Block B-1  Pマーク、Lマークなどを表示 END

            'Sフラグ
            If Me.SetNullToString(drSC3220201VisitChip.SSC_MARK, CHIP_S_NONE).Equals(CHIP_S_NONE) Then
                CType(ReceptionistAreaControl.FindControl("RightIcnS"), HtmlControl).Visible = False
            Else
                CType(ReceptionistAreaControl.FindControl("RightIcnS"), HtmlControl).Visible = True
                CType(ReceptionistAreaControl.FindControl("ReceptionistAreaSIcon"), CustomLabel).Text = Me.wordRightIcnS
            End If

            '車両No
            CType(ReceptionistAreaControl.FindControl("ReceptionistAreaVclNo"), CustomLabel).Text = _
                Me.SetNullToString(drSC3220201VisitChip.VCLREGNO, C_DEFAULT_CHIP_SPACE)

            '名前
            CType(ReceptionistAreaControl.FindControl("ReceptionistAreaName"), CustomLabel).Text = _
                Me.SetNullToString(drSC3220201VisitChip.CUSTOMERNAME, C_DEFAULT_CHIP_SPACE)

            '納車予定日時
            CType(ReceptionistAreaControl.FindControl("ReceptionistAreaDeliveryDate"), CustomLabel).Text = Me.wordNoTime

            '代表整備項目
            CType(ReceptionistAreaControl.FindControl("ReceptionistAreaFixitem"), CustomLabel).Text = _
                Me.SetNullToString(drSC3220201VisitChip.MERCHANDISENAME, C_DEFAULT_CHIP_SPACE)

            'チップ色設定
            With CType(ReceptionistAreaControl.FindControl("chip"), HtmlContainerControl)
                If Me.SetNullToString(drSC3220201VisitChip.DISP_START, CHIP_DEVISES_NONE).Equals(CHIP_DEVISES_NONE) Then
                    '仕掛前チェック
                    .Attributes("class") = CHIP_COLOR_NORMAL
                Else
                    '仕掛中チェック
                    .Attributes("class") = CHIP_COLOR_AQUA
                End If
            End With

            '遅れチェック
            Dim addMinutes As Long = 0
            If dtStanderdLt IsNot Nothing AndAlso 0 < dtStanderdLt.Rows.Count Then
                Dim rowStanderdLt As StandardLTListRow = DirectCast(dtStanderdLt.Rows(0), StandardLTListRow)
                If Not rowStanderdLt.IsRECEPT_STANDARD_LTNull Then
                    addMinutes = rowStanderdLt.RECEPT_STANDARD_LT
                End If
            End If
            With CType(ReceptionistAreaControl.FindControl("ChipBorder"), HtmlContainerControl)
                '2017/10/13 NSK 竹中(悠) REQ-SVT-TMT-20170227-002 見込納車遅れ日時計算ロジック統一 START
                'If nowDateTime >= drSC3220201VisitChip.PROC_DATE.AddMinutes(addMinutes) Then
                '    .Attributes("class") = CHIP_BORDER_RED
                'End If
                '予定納車日時が最小日付の場合、遅れ管理しない
                If Not (drSC3220201VisitChip.ITEM_DATE = DateTime.MinValue) Then
                    If nowDateTime >= drSC3220201VisitChip.PROC_DATE.AddMinutes(addMinutes) Then
                        .Attributes("class") = CHIP_BORDER_RED
                    End If
                End If
                '2017/10/13 NSK 竹中(悠) REQ-SVT-TMT-20170227-002 見込納車遅れ日時計算ロジック統一 END
                '判断用のデータ保持
                .Attributes("chipDate") = drSC3220201VisitChip.PROC_DATE.AddMinutes(addMinutes).ToString(CultureInfo.CurrentCulture)
                .Attributes("delayDate") = Date.MaxValue.ToString(CultureInfo.CurrentCulture)
            End With
        Next

        'ヘッダーとデータ表示件数を表示する
        Me.ReceptionistAreaTitle.Text = WebWordUtility.GetWord(APPLICATION_ID, WordID.id003)
        Me.ReceptionistAreaChipCount.Text = Me.ReceptionistAreaRepeater.Items.Count.ToString(CultureInfo.CurrentCulture)

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} END" _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name))
    End Sub

    ''' <summary>
    ''' 作業中エリアチップ初期設定
    ''' </summary>
    ''' <param name="dt">チップ情報</param>
    ''' <param name="dtStanderdLt">サービス標準LT情報</param>
    ''' <remarks></remarks>
    Private Sub SetInitWorkData(ByVal dt As SC3220201VisitChipDataTable, _
                                ByVal dtStanderdLt As StandardLTListDataTable)
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} START" _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name))

        '表示用にデータ加工
        Dim columnsList As New List(Of List(Of SC3220201VisitChipRow))
        Dim rowList As New List(Of SC3220201VisitChipRow)
        Dim rowCount As Integer = 0
        For Each drSC3220201VisitChip As SC3220201VisitChipRow In dt.Select(String.Format(CultureInfo.CurrentCulture, _
                                                                                           "DISP_DIV = '{0}'", _
                                                                                           SC3220201BusinessLogic.DisplayDivWork), "DISP_SORT")
            If rowCount Mod 5 = 0 Then
                rowList = New List(Of SC3220201VisitChipRow)
                columnsList.Add(rowList)
            End If
            rowList.Add(drSC3220201VisitChip)
            rowCount = rowCount + 1
        Next

        ' コントロールにバインドする
        Me.WorkAreaRepeater.DataSource = columnsList
        Me.WorkAreaRepeater.DataBind()

        '30件以下の場合は背景を設定
        If rowCount <= CHIP_WORK_COUNT Then
            Me.WorkArea.Attributes("class") = CHIP_BACK_IMAGE
        End If

        'データ設定
        For i As Integer = 0 To Me.WorkAreaRepeater.Items.Count - 1
            Dim chipLeftPading As Long = LeftPading
            Dim WorkAreaControl As Control = Me.WorkAreaRepeater.Items(i)
            Dim rowBox As List(Of SC3220201VisitChipRow) = columnsList(i)
            ' 5チップセットのリストをバインド
            Dim rowListRepeater As Repeater = CType(WorkAreaControl.FindControl("WorkAreaRowRepeater"), Repeater)
            rowListRepeater.DataSource = rowBox
            rowListRepeater.DataBind()

            For l = 0 To rowListRepeater.Items.Count - 1
                Dim WorkRowAreaControl As Control = rowListRepeater.Items(l)
                Dim drSC3220201VisitChip As SC3220201VisitChipRow = rowBox(l)

                '予約フラグ
                If Me.SetNullToString(drSC3220201VisitChip.REZ_MARK, CHIP_RESERVE_NONE).Equals(CHIP_RESERVE_NONE) Then
                    CType(WorkRowAreaControl.FindControl("RightIcnD"), HtmlControl).Visible = False
                Else
                    CType(WorkRowAreaControl.FindControl("RightIcnD"), HtmlControl).Visible = True
                    CType(WorkRowAreaControl.FindControl("WorkAreaReserveIcon"), CustomLabel).Text = Me.wordRightIcnD
                End If

                '2018/06/29 NSK 可児 TKM Next Gen e-CRB Project Application development Block B-1  Pマーク、Lマークなどを表示 START
                ''iフラグ
                'If Me.SetNullToString(drSC3220201VisitChip.JDP_MARK, CHIP_I_NONE).Equals(CHIP_I_NONE) Then
                '    CType(WorkRowAreaControl.FindControl("RightIcnI"), HtmlControl).Visible = False
                'Else
                '    CType(WorkRowAreaControl.FindControl("RightIcnI"), HtmlControl).Visible = True
                '    CType(WorkRowAreaControl.FindControl("WorkAreaIIcon"), CustomLabel).Text = Me.wordRightIcnI
                'End If
                'P or Lフラグ
                If ICON_FLAG_1.Equals(drSC3220201VisitChip.JDP_MARK) Then
                    CType(WorkRowAreaControl.FindControl("RightIcnL"), HtmlControl).Visible = False
                    CType(WorkRowAreaControl.FindControl("RightIcnP"), HtmlControl).Visible = True
                    CType(WorkRowAreaControl.FindControl("WorkAreaPIcon"), CustomLabel).Text = Me.wordRightIcnP
                ElseIf ICON_FLAG_2.Equals(drSC3220201VisitChip.JDP_MARK) Then
                    CType(WorkRowAreaControl.FindControl("RightIcnP"), HtmlControl).Visible = False
                    CType(WorkRowAreaControl.FindControl("RightIcnL"), HtmlControl).Visible = True
                    CType(WorkRowAreaControl.FindControl("WorkAreaLIcon"), CustomLabel).Text = Me.wordRightIcnL
                Else
                    CType(WorkRowAreaControl.FindControl("RightIcnP"), HtmlControl).Visible = False
                    CType(WorkRowAreaControl.FindControl("RightIcnL"), HtmlControl).Visible = False
                End If
                'M or Bフラグ
                If ICON_FLAG_1.Equals(drSC3220201VisitChip.SML_AMC_FLG) Then
                    CType(WorkRowAreaControl.FindControl("RightIcnB"), HtmlControl).Visible = False
                    CType(WorkRowAreaControl.FindControl("RightIcnM"), HtmlControl).Visible = True
                    CType(WorkRowAreaControl.FindControl("WorkAreaMIcon"), CustomLabel).Text = Me.wordRightIcnM
                ElseIf ICON_FLAG_2.Equals(drSC3220201VisitChip.SML_AMC_FLG) Then
                    CType(WorkRowAreaControl.FindControl("RightIcnM"), HtmlControl).Visible = False
                    CType(WorkRowAreaControl.FindControl("RightIcnB"), HtmlControl).Visible = True
                    CType(WorkRowAreaControl.FindControl("WorkAreaBIcon"), CustomLabel).Text = Me.wordRightIcnB
                Else
                    CType(WorkRowAreaControl.FindControl("RightIcnM"), HtmlControl).Visible = False
                    CType(WorkRowAreaControl.FindControl("RightIcnB"), HtmlControl).Visible = False
                End If
                'Eフラグ
                If ICON_FLAG_1.Equals(drSC3220201VisitChip.EW_FLG) Then
                    CType(WorkRowAreaControl.FindControl("RightIcnE"), HtmlControl).Visible = True
                    CType(WorkRowAreaControl.FindControl("WorkAreaEIcon"), CustomLabel).Text = Me.wordRightIcnE
                Else
                    CType(WorkRowAreaControl.FindControl("RightIcnE"), HtmlControl).Visible = False
                End If
                'Tフラグ
                If ICON_FLAG_1.Equals(drSC3220201VisitChip.TLM_MBR_FLG) Then
                    CType(WorkRowAreaControl.FindControl("RightIcnT"), HtmlControl).Visible = True
                    CType(WorkRowAreaControl.FindControl("WorkAreaTIcon"), CustomLabel).Text = Me.wordRightIcnT
                Else
                    CType(WorkRowAreaControl.FindControl("RightIcnT"), HtmlControl).Visible = False
                End If
                '2018/06/29 NSK 可児 TKM Next Gen e-CRB Project Application development Block B-1  Pマーク、Lマークなどを表示 END

                'Sフラグ
                If Me.SetNullToString(drSC3220201VisitChip.SSC_MARK, CHIP_S_NONE).Equals(CHIP_S_NONE) Then
                    CType(WorkRowAreaControl.FindControl("RightIcnS"), HtmlControl).Visible = False
                Else
                    CType(WorkRowAreaControl.FindControl("RightIcnS"), HtmlControl).Visible = True
                    CType(WorkRowAreaControl.FindControl("WorkAreaSIcon"), CustomLabel).Text = Me.wordRightIcnS
                End If

                '車両No
                CType(WorkRowAreaControl.FindControl("WorkAreaVclNo"), CustomLabel).Text = _
                    Me.SetNullToString(drSC3220201VisitChip.VCLREGNO, C_DEFAULT_CHIP_SPACE)

                '名前
                CType(WorkRowAreaControl.FindControl("WorkAreaName"), CustomLabel).Text = _
                    Me.SetNullToString(drSC3220201VisitChip.CUSTOMERNAME, C_DEFAULT_CHIP_SPACE)

                '納車予定日時
                CType(WorkRowAreaControl.FindControl("WorkAreaDeliveryDate"), CustomLabel).Text = _
                    Me.SetDateTimeToString(drSC3220201VisitChip.ITEM_DATE, wordFromTo)

                '代表整備項目
                CType(WorkRowAreaControl.FindControl("WorkAreaFixitem"), CustomLabel).Text = _
                    Me.SetNullToString(drSC3220201VisitChip.MERCHANDISENAME, C_DEFAULT_CHIP_SPACE)

                '追加作業有無
                Dim additionalWorkNumber As Long = drSC3220201VisitChip.APPROVAL_COUNT
                If -1 < additionalWorkNumber Then
                    '画像と件数を表示
                    CType(WorkRowAreaControl.FindControl("chipInfo"), HtmlContainerControl).Attributes("class") = CHIP_ADDWORK_DISP
                    CType(WorkRowAreaControl.FindControl("WorkAreaAddWork"), CustomLabel).Text = _
                        additionalWorkNumber.ToString(CultureInfo.CurrentCulture)

                Else
                    '画像と件数を非表示
                    CType(WorkRowAreaControl.FindControl("chipInfo"), HtmlContainerControl).Attributes("class") = CHIP_ADDWORK_DISP_NONE
                    CType(WorkRowAreaControl.FindControl("WorkAreaAddWork"), CustomLabel).Text = String.Empty
                End If

                'チップ色設定
                With CType(WorkRowAreaControl.FindControl("chip"), HtmlContainerControl)
                    ' 仕掛中チェック
                    If Me.SetNullToString(drSC3220201VisitChip.DISP_START, CHIP_DEVISES_NONE).Equals(CHIP_DEVISES_NONE) Then
                        .Attributes("class") = CHIP_COLOR_NORMAL
                    Else
                        .Attributes("class") = CHIP_COLOR_AQUA
                    End If
                End With

                '遅れチェック
                '2017/09/14 NSK 竹中(悠) REQ-SVT-TMT-20170227-002 見込納車遅れ日時計算ロジック統一 START
                'Dim addMinutes As Long = 0
                'If dtStanderdLt IsNot Nothing AndAlso 0 < dtStanderdLt.Rows.Count Then
                '    Dim rowStanderdLt As StandardLTListRow = DirectCast(dtStanderdLt.Rows(0), StandardLTListRow)
                '    'データがない場合のため初期値として「0」を入れておく
                '    If rowStanderdLt.IsDELIVERYWR_STANDARD_LTNull Then rowStanderdLt.DELIVERYWR_STANDARD_LT = 0
                '    If rowStanderdLt.IsDELIVERYPRE_STANDARD_LTNull Then rowStanderdLt.DELIVERYPRE_STANDARD_LT = 0
                '    If rowStanderdLt.IsWASHTIMENull Then rowStanderdLt.WASHTIME = 0

                '    If CHIP_WASHING_NONE.Equals(drSC3220201VisitChip.WASHFLG) Then
                '        addMinutes = rowStanderdLt.DELIVERYWR_STANDARD_LT + rowStanderdLt.DELIVERYPRE_STANDARD_LT
                '    Else
                '        Dim addLongTime As Long = System.Math.Max(rowStanderdLt.DELIVERYPRE_STANDARD_LT, rowStanderdLt.WASHTIME)
                '        addMinutes = rowStanderdLt.DELIVERYWR_STANDARD_LT + addLongTime
                '    End If
                'End If
                'With CType(WorkRowAreaControl.FindControl("ChipBorder"), HtmlContainerControl)
                '    If nowDateTime >= drSC3220201VisitChip.PROC_DATE Then
                '        .Attributes("class") = CHIP_BORDER_RED
                '    ElseIf nowDateTime >= drSC3220201VisitChip.PROC_DATE.AddMinutes(-addMinutes) Then
                '        .Attributes("class") = CHIP_BORDER_YELLOW
                '    End If
                '    '判断用のデータ保持
                '    .Attributes("chipDate") = drSC3220201VisitChip.PROC_DATE.ToString(CultureInfo.CurrentCulture)
                '    .Attributes("delayDate") = drSC3220201VisitChip.PROC_DATE.AddMinutes(-addMinutes).ToString(CultureInfo.CurrentCulture)
                'End With
                With CType(WorkRowAreaControl.FindControl("ChipBorder"), HtmlContainerControl)
                    '2017/10/13 NSK 竹中(悠) REQ-SVT-TMT-20170227-002 見込納車遅れ日時計算ロジック統一 START
                    'If nowDateTime >= drSC3220201VisitChip.PROC_DATE Then
                    '    .Attributes("class") = CHIP_BORDER_RED
                    'ElseIf nowDateTime >= drSC3220201VisitChip.DELAY_DELI_TIME Then
                    '    .Attributes("class") = CHIP_BORDER_YELLOW
                    'End If
                    '予定納車日時が日付最小値の場合、遅れ管理しない
                    If Not (drSC3220201VisitChip.ITEM_DATE = DateTime.MinValue) Then
                        If nowDateTime >= drSC3220201VisitChip.PROC_DATE Then
                            .Attributes("class") = CHIP_BORDER_RED
                        ElseIf nowDateTime >= drSC3220201VisitChip.DELAY_DELI_TIME Then
                            .Attributes("class") = CHIP_BORDER_YELLOW
                        End If
                    End If
                    '2017/10/13 NSK 竹中(悠) REQ-SVT-TMT-20170227-002 見込納車遅れ日時計算ロジック統一 END
                    '判断用のデータ保持
                    .Attributes("chipDate") = drSC3220201VisitChip.PROC_DATE.ToString(CultureInfo.CurrentCulture)
                    .Attributes("delayDate") = drSC3220201VisitChip.DELAY_DELI_TIME.ToString(CultureInfo.CurrentCulture)
                End With
                '2017/09/14 NSK 竹中(悠) REQ-SVT-TMT-20170227-002 見込納車遅れ日時計算ロジック統一 END

                'チップ位置を設定
                CType(WorkRowAreaControl.FindControl("mainChip"), HtmlControl).Attributes("style") = _
                    String.Format(CultureInfo.CurrentCulture, CHIP_REPLACE_LEFT_PADDING, chipLeftPading.ToString(CultureInfo.CurrentCulture))

                '次のチップ位置を設定
                chipLeftPading += AddLeftPading
            Next
        Next

        'ヘッダーとデータ表示件数を表示する
        Me.WorkAreaTitle.Text = WebWordUtility.GetWord(APPLICATION_ID, WordID.id004)
        Me.WorkAreaChipCount.Text = rowCount.ToString(CultureInfo.CurrentCulture)

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} END" _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name))
    End Sub

    ''' <summary>
    ''' 納車準備エリアチップ初期設定
    ''' </summary>
    ''' <param name="dt">チップ情報</param>
    ''' <remarks></remarks>
    Private Sub SetInitWashData(ByVal dt As SC3220201VisitChipDataTable)
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} START" _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name))

        ' コントロールにバインドする
        Me.WashAreaRepeater.DataSource = dt.Select(String.Format(CultureInfo.CurrentCulture, _
                                                                 "DISP_DIV = '{0}'", _
                                                                 SC3220201BusinessLogic.DisplayDivPreparation), "DISP_SORT")
        Me.WashAreaRepeater.DataBind()

        Dim rowList As SC3220201VisitChipRow() = _
            DirectCast(WashAreaRepeater.DataSource, SC3220201VisitChipRow())

        ' データを設定する
        For i = 0 To Me.WashAreaRepeater.Items.Count - 1
            Dim WashAreaControl As Control = Me.WashAreaRepeater.Items(i)
            Dim drSC3220201VisitChip As SC3220201VisitChipRow = rowList(i)

            '予約フラグ
            If Me.SetNullToString(drSC3220201VisitChip.REZ_MARK, CHIP_RESERVE_NONE).Equals(CHIP_RESERVE_NONE) Then
                CType(WashAreaControl.FindControl("RightIcnD"), HtmlControl).Visible = False
            Else
                CType(WashAreaControl.FindControl("RightIcnD"), HtmlControl).Visible = True
                CType(WashAreaControl.FindControl("WashAreaReserveIcon"), CustomLabel).Text = Me.wordRightIcnD
            End If

            '2018/06/29 NSK 可児 TKM Next Gen e-CRB Project Application development Block B-1  Pマーク、Lマークなどを表示 START
            ''iフラグ
            'If Me.SetNullToString(drSC3220201VisitChip.JDP_MARK, CHIP_I_NONE).Equals(CHIP_I_NONE) Then
            '    CType(WashAreaControl.FindControl("RightIcnI"), HtmlControl).Visible = False
            'Else
            '    CType(WashAreaControl.FindControl("RightIcnI"), HtmlControl).Visible = True
            '    CType(WashAreaControl.FindControl("WashAreaIIcon"), CustomLabel).Text = Me.wordRightIcnI
            'End If
            'P or Lフラグ
            If ICON_FLAG_1.Equals(drSC3220201VisitChip.JDP_MARK) Then
                CType(WashAreaControl.FindControl("RightIcnL"), HtmlControl).Visible = False
                CType(WashAreaControl.FindControl("RightIcnP"), HtmlControl).Visible = True
                CType(WashAreaControl.FindControl("WashAreaPIcon"), CustomLabel).Text = Me.wordRightIcnP
            ElseIf ICON_FLAG_2.Equals(drSC3220201VisitChip.JDP_MARK) Then
                CType(WashAreaControl.FindControl("RightIcnP"), HtmlControl).Visible = False
                CType(WashAreaControl.FindControl("RightIcnL"), HtmlControl).Visible = True
                CType(WashAreaControl.FindControl("WashAreaLIcon"), CustomLabel).Text = Me.wordRightIcnL
            Else
                CType(WashAreaControl.FindControl("RightIcnP"), HtmlControl).Visible = False
                CType(WashAreaControl.FindControl("RightIcnL"), HtmlControl).Visible = False
            End If
            'M or Bフラグ
            If ICON_FLAG_1.Equals(drSC3220201VisitChip.SML_AMC_FLG) Then
                CType(WashAreaControl.FindControl("RightIcnB"), HtmlControl).Visible = False
                CType(WashAreaControl.FindControl("RightIcnM"), HtmlControl).Visible = True
                CType(WashAreaControl.FindControl("WashAreaMIcon"), CustomLabel).Text = Me.wordRightIcnM
            ElseIf ICON_FLAG_2.Equals(drSC3220201VisitChip.SML_AMC_FLG) Then
                CType(WashAreaControl.FindControl("RightIcnM"), HtmlControl).Visible = False
                CType(WashAreaControl.FindControl("RightIcnB"), HtmlControl).Visible = True
                CType(WashAreaControl.FindControl("WashAreaBIcon"), CustomLabel).Text = Me.wordRightIcnB
            Else
                CType(WashAreaControl.FindControl("RightIcnM"), HtmlControl).Visible = False
                CType(WashAreaControl.FindControl("RightIcnB"), HtmlControl).Visible = False
            End If
            'Eフラグ
            If ICON_FLAG_1.Equals(drSC3220201VisitChip.EW_FLG) Then
                CType(WashAreaControl.FindControl("RightIcnE"), HtmlControl).Visible = True
                CType(WashAreaControl.FindControl("WashAreaEIcon"), CustomLabel).Text = Me.wordRightIcnE
            Else
                CType(WashAreaControl.FindControl("RightIcnE"), HtmlControl).Visible = False
            End If
            'Tフラグ
            If ICON_FLAG_1.Equals(drSC3220201VisitChip.TLM_MBR_FLG) Then
                CType(WashAreaControl.FindControl("RightIcnT"), HtmlControl).Visible = True
                CType(WashAreaControl.FindControl("WashAreaTIcon"), CustomLabel).Text = Me.wordRightIcnT
            Else
                CType(WashAreaControl.FindControl("RightIcnT"), HtmlControl).Visible = False
            End If
            '2018/06/29 NSK 可児 TKM Next Gen e-CRB Project Application development Block B-1  Pマーク、Lマークなどを表示 END

            'Sフラグ
            If Me.SetNullToString(drSC3220201VisitChip.SSC_MARK, CHIP_S_NONE).Equals(CHIP_S_NONE) Then
                CType(WashAreaControl.FindControl("RightIcnS"), HtmlControl).Visible = False
            Else
                CType(WashAreaControl.FindControl("RightIcnS"), HtmlControl).Visible = True
                CType(WashAreaControl.FindControl("WashAreaSIcon"), CustomLabel).Text = Me.wordRightIcnS
            End If

            '車両No
            CType(WashAreaControl.FindControl("WashAreaVclNo"), CustomLabel).Text = _
                Me.SetNullToString(drSC3220201VisitChip.VCLREGNO, C_DEFAULT_CHIP_SPACE)

            '名前
            CType(WashAreaControl.FindControl("WashAreaName"), CustomLabel).Text = _
                Me.SetNullToString(drSC3220201VisitChip.CUSTOMERNAME, C_DEFAULT_CHIP_SPACE)

            '納車予定日時
            CType(WashAreaControl.FindControl("WashAreaDeliveryDate"), CustomLabel).Text = _
                Me.SetDateTimeToString(drSC3220201VisitChip.ITEM_DATE, wordFromTo)

            '代表整備項目
            CType(WashAreaControl.FindControl("WashAreaFixitem"), CustomLabel).Text = _
                Me.SetNullToString(drSC3220201VisitChip.MERCHANDISENAME, C_DEFAULT_CHIP_SPACE)

            'チップ色設定
            With CType(WashAreaControl.FindControl("chip"), HtmlContainerControl)
                ' 仕掛中チェック
                If Me.SetNullToString(drSC3220201VisitChip.DISP_START, CHIP_DEVISES_NONE).Equals(CHIP_DEVISES_NONE) Then
                    .Attributes("class") = CHIP_COLOR_NORMAL
                Else
                    .Attributes("class") = CHIP_COLOR_AQUA
                End If
            End With

            '遅れチェック
            With CType(WashAreaControl.FindControl("ChipBorder"), HtmlContainerControl)
                '2017/10/13 NSK 竹中(悠) REQ-SVT-TMT-20170227-002 見込納車遅れ日時計算ロジック統一 START
                'If nowDateTime >= drSC3220201VisitChip.PROC_DATE Then
                '    .Attributes("class") = CHIP_BORDER_RED
                'End If
                '予定納車日時が最小値の場合、遅れ管理しない
                If Not (drSC3220201VisitChip.ITEM_DATE = DateTime.MinValue) Then
                    If nowDateTime >= drSC3220201VisitChip.PROC_DATE Then
                        .Attributes("class") = CHIP_BORDER_RED
                    End If
                End If
                '2017/10/13 NSK 竹中(悠) REQ-SVT-TMT-20170227-002 見込納車遅れ日時計算ロジック統一 END
                '判断用のデータ保持
                .Attributes("chipDate") = drSC3220201VisitChip.PROC_DATE.ToString(CultureInfo.CurrentCulture)
                .Attributes("delayDate") = Date.MaxValue.ToString(CultureInfo.CurrentCulture)
            End With

        Next

        'ヘッダーとデータ表示件数を表示する
        Me.WashAreaTitle.Text = WebWordUtility.GetWord(APPLICATION_ID, WordID.id005)
        Me.WashAreaChipCount.Text = Me.WashAreaRepeater.Items.Count.ToString(CultureInfo.CurrentCulture)

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} END" _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name))
    End Sub

    ''' <summary>
    ''' 納車作業エリアチップ初期設定
    ''' </summary>
    ''' <param name="dt">チップ情報</param>
    ''' <remarks></remarks>
    Private Sub SetInitDeliveryData(ByVal dt As SC3220201VisitChipDataTable)
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} START" _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name))

        'コントロールにバインドする
        Me.DeliveryAreaRepeater.DataSource = dt.Select(String.Format(CultureInfo.CurrentCulture, _
                                                                     "DISP_DIV = '{0}'", _
                                                                     SC3220201BusinessLogic.DisplayDivDelivery), "DISP_SORT")
        Me.DeliveryAreaRepeater.DataBind()

        Dim rowList As SC3220201VisitChipRow() = _
            DirectCast(DeliveryAreaRepeater.DataSource, SC3220201VisitChipRow())

        'データ設定
        For i = 0 To DeliveryAreaRepeater.Items.Count - 1

            Dim DeliveryAreaControl As Control = Me.DeliveryAreaRepeater.Items(i)
            Dim drSC3220201VisitChip As SC3220201VisitChipRow = rowList(i)

            '予約フラグ
            If Me.SetNullToString(drSC3220201VisitChip.REZ_MARK, CHIP_RESERVE_NONE).Equals(CHIP_RESERVE_NONE) Then
                CType(DeliveryAreaControl.FindControl("RightIcnD"), HtmlControl).Visible = False
            Else
                CType(DeliveryAreaControl.FindControl("RightIcnD"), HtmlControl).Visible = True
                CType(DeliveryAreaControl.FindControl("DeliveryAreaReserveIcon"), CustomLabel).Text = Me.wordRightIcnD
            End If

            '2018/06/29 NSK 可児 TKM Next Gen e-CRB Project Application development Block B-1  Pマーク、Lマークなどを表示 START
            ''iフラグ
            'If Me.SetNullToString(drSC3220201VisitChip.JDP_MARK, CHIP_I_NONE).Equals(CHIP_I_NONE) Then
            '    CType(DeliveryAreaControl.FindControl("RightIcnI"), HtmlControl).Visible = False
            'Else
            '    CType(DeliveryAreaControl.FindControl("RightIcnI"), HtmlControl).Visible = True
            '    CType(DeliveryAreaControl.FindControl("DeliveryAreaIIcon"), CustomLabel).Text = Me.wordRightIcnI
            'End If
            'P or Lフラグ
            If ICON_FLAG_1.Equals(drSC3220201VisitChip.JDP_MARK) Then
                CType(DeliveryAreaControl.FindControl("RightIcnL"), HtmlControl).Visible = False
                CType(DeliveryAreaControl.FindControl("RightIcnP"), HtmlControl).Visible = True
                CType(DeliveryAreaControl.FindControl("DeliveryAreaPIcon"), CustomLabel).Text = Me.wordRightIcnP
            ElseIf ICON_FLAG_2.Equals(drSC3220201VisitChip.JDP_MARK) Then
                CType(DeliveryAreaControl.FindControl("RightIcnP"), HtmlControl).Visible = False
                CType(DeliveryAreaControl.FindControl("RightIcnL"), HtmlControl).Visible = True
                CType(DeliveryAreaControl.FindControl("DeliveryAreaLIcon"), CustomLabel).Text = Me.wordRightIcnL
            Else
                CType(DeliveryAreaControl.FindControl("RightIcnP"), HtmlControl).Visible = False
                CType(DeliveryAreaControl.FindControl("RightIcnL"), HtmlControl).Visible = False
            End If
            'M or Bフラグ
            If ICON_FLAG_1.Equals(drSC3220201VisitChip.SML_AMC_FLG) Then
                CType(DeliveryAreaControl.FindControl("RightIcnB"), HtmlControl).Visible = False
                CType(DeliveryAreaControl.FindControl("RightIcnM"), HtmlControl).Visible = True
                CType(DeliveryAreaControl.FindControl("DeliveryAreaMIcon"), CustomLabel).Text = Me.wordRightIcnM
            ElseIf ICON_FLAG_2.Equals(drSC3220201VisitChip.SML_AMC_FLG) Then
                CType(DeliveryAreaControl.FindControl("RightIcnM"), HtmlControl).Visible = False
                CType(DeliveryAreaControl.FindControl("RightIcnB"), HtmlControl).Visible = True
                CType(DeliveryAreaControl.FindControl("DeliveryAreaBIcon"), CustomLabel).Text = Me.wordRightIcnB
            Else
                CType(DeliveryAreaControl.FindControl("RightIcnM"), HtmlControl).Visible = False
                CType(DeliveryAreaControl.FindControl("RightIcnB"), HtmlControl).Visible = False
            End If
            'Eフラグ
            If ICON_FLAG_1.Equals(drSC3220201VisitChip.EW_FLG) Then
                CType(DeliveryAreaControl.FindControl("RightIcnE"), HtmlControl).Visible = True
                CType(DeliveryAreaControl.FindControl("DeliveryAreaEIcon"), CustomLabel).Text = Me.wordRightIcnE
            Else
                CType(DeliveryAreaControl.FindControl("RightIcnE"), HtmlControl).Visible = False
            End If
            'Tフラグ
            If ICON_FLAG_1.Equals(drSC3220201VisitChip.TLM_MBR_FLG) Then
                CType(DeliveryAreaControl.FindControl("RightIcnT"), HtmlControl).Visible = True
                CType(DeliveryAreaControl.FindControl("DeliveryAreaTIcon"), CustomLabel).Text = Me.wordRightIcnT
            Else
                CType(DeliveryAreaControl.FindControl("RightIcnT"), HtmlControl).Visible = False
            End If
            '2018/06/29 NSK 可児 TKM Next Gen e-CRB Project Application development Block B-1  Pマーク、Lマークなどを表示 END

            'Sフラグ
            If Me.SetNullToString(drSC3220201VisitChip.SSC_MARK, CHIP_S_NONE).Equals(CHIP_S_NONE) Then
                CType(DeliveryAreaControl.FindControl("RightIcnS"), HtmlControl).Visible = False
            Else
                CType(DeliveryAreaControl.FindControl("RightIcnS"), HtmlControl).Visible = True
                CType(DeliveryAreaControl.FindControl("DeliveryAreaSIcon"), CustomLabel).Text = Me.wordRightIcnS
            End If

            '車両No
            CType(DeliveryAreaControl.FindControl("DeliveryAreaVclNo"), CustomLabel).Text = _
                Me.SetNullToString(drSC3220201VisitChip.VCLREGNO, C_DEFAULT_CHIP_SPACE)

            '名前
            CType(DeliveryAreaControl.FindControl("DeliveryAreaName"), CustomLabel).Text = _
                Me.SetNullToString(drSC3220201VisitChip.CUSTOMERNAME, C_DEFAULT_CHIP_SPACE)

            '納車予定日時
            CType(DeliveryAreaControl.FindControl("DeliveryAreaDeliveryDate"), CustomLabel).Text = _
                Me.SetDateTimeToString(drSC3220201VisitChip.ITEM_DATE, wordFromTo)

            '代表整備項目
            CType(DeliveryAreaControl.FindControl("DeliveryAreaFixitem"), CustomLabel).Text = _
                Me.SetNullToString(drSC3220201VisitChip.MERCHANDISENAME, C_DEFAULT_CHIP_SPACE)

            'チップ色設定
            With CType(DeliveryAreaControl.FindControl("chip"), HtmlContainerControl)
                ' 仕掛中チェック
                If Me.SetNullToString(drSC3220201VisitChip.DISP_START, CHIP_DEVISES_NONE).Equals(CHIP_DEVISES_NONE) Then
                    .Attributes("class") = CHIP_COLOR_NORMAL
                Else
                    .Attributes("class") = CHIP_COLOR_AQUA
                End If
            End With

            '遅れチェック
            With CType(DeliveryAreaControl.FindControl("ChipBorder"), HtmlContainerControl)
                '2017/10/13 NSK 竹中(悠) REQ-SVT-TMT-20170227-002 見込納車遅れ日時計算ロジック統一 START
                'If nowDateTime >= drSC3220201VisitChip.PROC_DATE Then
                '    .Attributes("class") = CHIP_BORDER_RED
                'End If
                '予定納車日時が最小値の場合、遅れ管理しない
                If Not (drSC3220201VisitChip.ITEM_DATE = DateTime.MinValue) Then
                    If nowDateTime >= drSC3220201VisitChip.PROC_DATE Then
                        .Attributes("class") = CHIP_BORDER_RED
                    End If
                End If
                '2017/10/13 NSK 竹中(悠) REQ-SVT-TMT-20170227-002 見込納車遅れ日時計算ロジック統一 END
                '判断用のデータ保持
                .Attributes("chipDate") = drSC3220201VisitChip.PROC_DATE.ToString(CultureInfo.CurrentCulture)
                .Attributes("delayDate") = Date.MaxValue.ToString(CultureInfo.CurrentCulture)
            End With
        Next

        'ヘッダーとデータ表示件数を表示する
        Me.DeliveryAreaTitle.Text = WebWordUtility.GetWord(APPLICATION_ID, WordID.id006)
        Me.DeliveryAreaChipCount.Text = Me.DeliveryAreaRepeater.Items.Count.ToString(CultureInfo.CurrentCulture)


        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} END" _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name))
    End Sub

#End Region

#Region "フッター制御"

    ''' <summary>
    ''' フッター制御
    ''' </summary>
    ''' <param name="commonMaster">マスターページ</param>
    ''' <param name="category">カテゴリ</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    ''' <History>
    ''' 2013/12/10 TMEJ 河原 次世代サービス 工程管理機能開発
    ''' </History>
    Public Overrides Function DeclareCommonMasterFooter(ByVal commonMaster As CommonMasterPage, _
                                                        ByRef category As FooterMenuCategory) As Integer()
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} START" _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name))

        'category = FooterMenuCategory.MainMenu

        '2013/12/10 TMEJ 河原 次世代サービス 工程管理機能開発 START

        'ログイン情報取得
        Dim staffInfo As StaffContext = StaffContext.Current

        '権限チェック
        If staffInfo.OpeCD = iCROP.BizLogic.Operation.SVR Then
            'SVR権限

            '自ページの所属メニューを宣言
            category = FooterMenuCategory.WholeManagement

        Else
            'その他(SM権限)

            '自ページの所属メニューを宣言
            category = FooterMenuCategory.MainMenu


        End If

        '2013/12/10 TMEJ 河原 次世代サービス 工程管理機能開発 END

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
    ''' <history>
    ''' 2013/12/10 TMEJ 河原 TMEJ次世代サービス 工程管理機能開発
    ''' </history>
    Private Sub InitFooterButton()
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} START" _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name))

        'メインメニューボタンの設定
        Dim mainMenuButton As CommonMasterFooterButton = _
            CType(Me.Master, CommonMasterPage).GetFooterButton(FOOTER_MAINMENU)
        AddHandler mainMenuButton.Click, AddressOf MainMenuButton_Click
        mainMenuButton.OnClientClick = _
            String.Format(CultureInfo.CurrentCulture, _
                          FOOTER_REPLACE_EVENT, _
                          FOOTER_MAINMENU.ToString(CultureInfo.CurrentCulture))


        '2013/12/10 TMEJ 河原 TMEJ次世代サービス 工程管理機能開発 START

        ''2013/06/03 TMEJ 河原 IT9513_【A.STEP2】次世代e-CRBタブレット　新DB適応に向けた機能設計 START

        ''SMBボタンの設定
        'Dim smbButton As CommonMasterFooterButton = _
        'CType(Me.Master, CommonMasterPage).GetFooterButton(FOOTER_SMB)
        'AddHandler smbButton.Click, AddressOf SMBButton_Click
        'smbButton.OnClientClick = _
        '    String.Format(CultureInfo.CurrentCulture, _
        '                  FOOTER_REPLACE_EVENT, _
        '                  FOOTER_SMB.ToString(CultureInfo.CurrentCulture))

        ''2013/06/03 TMEJ 河原 IT9513_【A.STEP2】次世代e-CRBタブレット　新DB適応に向けた機能設計 END

        ''顧客詳細ボタンの設定
        'Dim customerButton As CommonMasterFooterButton = _
        '    CType(Me.Master, CommonMasterPage).GetFooterButton(FOOTER_CUSTOMER)
        'customerButton.OnClientClick = _
        '    String.Format(CultureInfo.CurrentCulture, _
        '                  FOOTER_REPLACE_EVENT, _
        '                  FOOTER_CUSTOMER.ToString(CultureInfo.CurrentCulture))

        ''R/Oボタンの設定
        'Dim roButton As CommonMasterFooterButton = _
        '    CType(Me.Master, CommonMasterPage).GetFooterButton(FOOTER_RO)
        'AddHandler roButton.Click, AddressOf RoButton_Click
        'roButton.OnClientClick = _
        '    String.Format(CultureInfo.CurrentCulture, FOOTER_REPLACE_EVENT, FOOTER_RO.ToString(CultureInfo.CurrentCulture))

        ' ''追加作業ボタンの設定
        'Dim addListButton As CommonMasterFooterButton = _
        '    CType(Me.Master, CommonMasterPage).GetFooterButton(FOOTER_ADD_LIST)
        'AddHandler addListButton.Click, AddressOf AddListButton_Click
        'addListButton.OnClientClick = _
        '    String.Format(CultureInfo.CurrentCulture, _
        '                  FOOTER_REPLACE_EVENT, _
        '                  FOOTER_ADD_LIST.ToString(CultureInfo.CurrentCulture))

        ''スケジュールボタンのイベント設定
        'Dim scheduleButton As CommonMasterFooterButton = _
        '    CType(Me.Master, CommonMasterPage).GetFooterButton(FOOTER_SCHEDULE)
        'scheduleButton.OnClientClick = FOOTER_EVENT_SCHEDULER

        ''電話帳ボタンのイベント設定
        'Dim telDirectoryButton As CommonMasterFooterButton = _
        '    CType(Me.Master, CommonMasterPage).GetFooterButton(FOOTER_TEL_DIRECTORY)
        'telDirectoryButton.OnClientClick = FOOTER_EVENT_TEL

        ''来店管理ボタンの文言とイベント設定
        'Me.VisitManagementFooterLabel.Text = WebWordUtility.GetWord(APPLICATION_ID, WordID.id007)

        ''全体管理ボタンの文言とイベント設定
        'Me.AllManagementFooterLabel.Text = WebWordUtility.GetWord(APPLICATION_ID, WordID.id008)

        'R/Oボタンの設定
        Dim roButton As CommonMasterFooterButton = _
            CType(Me.Master, CommonMasterPage).GetFooterButton(FooterMenuCategory.RepairOrderList)
        AddHandler roButton.Click, AddressOf RoButton_Click
        roButton.OnClientClick = "FooterButtonClick(" & FooterMenuCategory.RepairOrderList & ");"

        '来店管理
        Dim ReserveManagementButton As CommonMasterFooterButton = _
        CType(Me.Master, CommonMasterPage).GetFooterButton(FooterMenuCategory.ReserveManagement)
        AddHandler ReserveManagementButton.Click, AddressOf ReserveManagement_Click
        ReserveManagementButton.OnClientClick = "return FooterButtonClick(" & FooterMenuCategory.ReserveManagement & ");"

        '電話帳ボタンのイベント設定
        Dim telDirectoryButton As CommonMasterFooterButton = _
        CType(Me.Master, CommonMasterPage).GetFooterButton(FooterMenuCategory.Contact)
        telDirectoryButton.OnClientClick = FOOTER_EVENT_TEL

        'ログイン情報取得
        Dim staffInfo As StaffContext = StaffContext.Current

        '権限チェック
        If staffInfo.OpeCD = iCROP.BizLogic.Operation.SVR Then
            'SVR権限

            '全体管理
            Dim WholeManagementButton As CommonMasterFooterButton = _
            CType(Me.Master, CommonMasterPage).GetFooterButton(FooterMenuCategory.WholeManagement)
            AddHandler WholeManagementButton.Click, AddressOf WholeManagementButton_Click
            WholeManagementButton.OnClientClick = "return FooterButtonClick(" & FooterMenuCategory.WholeManagement & ");"

        Else
            '上記以外(SM権限)

            '顧客情報画面(ヘッダー顧客検索機能へフォーカス)
            Dim customerButton As CommonMasterFooterButton = _
            CType(Me.Master, CommonMasterPage).GetFooterButton(FooterMenuCategory.CustomerDetail)
            customerButton.OnClientClick = "FooterButtonClick(" & FooterMenuCategory.CustomerDetail & ");"

            '商品訴求コンテンツ
            Dim productsAppealContentButton As CommonMasterFooterButton = _
            CType(Me.Master, CommonMasterPage).GetFooterButton(FooterMenuCategory.GoodsSolicitationContents)
            AddHandler productsAppealContentButton.Click, AddressOf productsAppealContentButton_Click
            productsAppealContentButton.OnClientClick = "return FooterButtonClick(" & FooterMenuCategory.GoodsSolicitationContents & ");"

            'キャンペーン
            Dim campaignButton As CommonMasterFooterButton = _
            CType(Me.Master, CommonMasterPage).GetFooterButton(FooterMenuCategory.Campaign)
            AddHandler campaignButton.Click, AddressOf campaignButton_Click
            campaignButton.OnClientClick = "return FooterButtonClick(" & FooterMenuCategory.Campaign & ");"

            'SMB
            Dim smbButton As CommonMasterFooterButton = _
            CType(Me.Master, CommonMasterPage).GetFooterButton(FooterMenuCategory.SMB)
            AddHandler smbButton.Click, AddressOf SMBButton_Click
            smbButton.OnClientClick = "return FooterButtonClick(" & FooterMenuCategory.SMB & ");"

        End If

        '2013/12/10 TMEJ 河原 TMEJ次世代サービス 工程管理機能開発 END

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
    ''' <History>
    ''' 2013/12/10 TMEJ 河原 次世代サービス 工程管理機能開発
    ''' </History>
    Private Sub MainMenuButton_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} START" _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name))

        '2013/12/10 TMEJ 河原 次世代サービス 工程管理機能開発 START

        '' メインメニュー(SM)に遷移する
        'Me.RedirectNextScreen(MAINMENU_ID)

        'ログイン情報取得
        Dim staffInfo As StaffContext = StaffContext.Current

        '権限チェック
        If staffInfo.OpeCD = iCROP.BizLogic.Operation.SVR Then
            'SVR権限

            '未振当て一覧画面に遷移する
            Me.RedirectNextScreen(ASSIGN_LIST_PAGE)

        Else

            'メインメニュー(全体管理画面)に遷移する
            Me.RedirectNextScreen(APPLICATION_ID)

        End If

        '2013/12/10 TMEJ 河原 次世代サービス 工程管理機能開発 END


        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} END" _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name))
    End Sub

    '2013/12/10 TMEJ 河原 TMEJ次世代サービス 工程管理機能開発 START

    ' ''' <summary>
    ' ''' R/Oボタンを押した時の処理
    ' ''' </summary>
    ' ''' <param name="sender"></param>
    ' ''' <param name="e"></param>
    ' ''' <remarks></remarks>
    'Private Sub RoButton_Click(ByVal sender As Object, ByVal e As System.EventArgs)
    '    Logger.Info(String.Format(CultureInfo.CurrentCulture _
    '                , "{0}.{1} START" _
    '                , Me.GetType.ToString _
    '                , System.Reflection.MethodBase.GetCurrentMethod.Name))

    '    'R/O一覧画面に遷移する
    '    Me.RedirectNextScreen(REPAIR_ORDERE_LIST_PAGE)

    '    Logger.Info(String.Format(CultureInfo.CurrentCulture _
    '                , "{0}.{1} END" _
    '                , Me.GetType.ToString _
    '                , System.Reflection.MethodBase.GetCurrentMethod.Name))
    'End Sub

    ' ''' <summary>
    ' ''' 追加作業ボタンを押した時の処理
    ' ''' </summary>
    ' ''' <param name="sender"></param>
    ' ''' <param name="e"></param>
    ' ''' <remarks></remarks>
    'Private Sub AddListButton_Click(ByVal sender As Object, ByVal e As System.EventArgs)
    '    Logger.Info(String.Format(CultureInfo.CurrentCulture _
    '                , "{0}.{1} START" _
    '                , Me.GetType.ToString _
    '                , System.Reflection.MethodBase.GetCurrentMethod.Name))

    '    '追加作業一覧画面に遷移する
    '    Me.RedirectNextScreen(ADDITION_WORK_LIST_PAGE)

    '    Logger.Info(String.Format(CultureInfo.CurrentCulture _
    '                , "{0}.{1} END" _
    '                , Me.GetType.ToString _
    '                , System.Reflection.MethodBase.GetCurrentMethod.Name))
    'End Sub

    '2013/12/10 TMEJ 河原 TMEJ次世代サービス 工程管理機能開発 END

    '2013/12/10 TMEJ 河原 TMEJ次世代サービス 工程管理機能開発 START

    ''' <summary>
    ''' ROボタンを押した時の処理
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub RoButton_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Logger.Info("RoButton_Click Start")

        'R/O一覧画面遷移処理(パラメータ設定)
        Me.RedirectOrderList()

        Logger.Info("RoButton_Click End")
    End Sub

    ''' <summary>
    ''' 商品訴求コンテンツボタンを押した時の処理
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    ''' <history>
    ''' </history>
    Private Sub productsAppealContentButton_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Logger.Info("productsAppealContentButton_Click Start")

        '商品訴求コンテンツ画面遷移処理(パラメータ設定)
        Me.RedirectProductsAppealContent()

        Logger.Info("productsAppealContentButton_Click End")
    End Sub

    ''' <summary>
    ''' キャンペーンボタンを押した時の処理
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    ''' <history>
    ''' </history>
    Private Sub campaignButton_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Logger.Info("campaignButton_Click Start")

        'キャンペーン画面遷移処理(パラメータ設定)
        Me.RedirectCampaign()

        Logger.Info("campaignButton_Click End")
    End Sub

    ''' <summary>
    ''' 来店管理ボタンを押した時の処理
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    ''' <history>
    ''' </history>
    Private Sub ReserveManagement_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Logger.Info("ReserveManagement_Click Start")

        '工程管理画面に遷移する
        Me.RedirectNextScreen(VISIT_MANAGEMENT_LIST_PAGE)

        Logger.Info("ReserveManagement_Click End")
    End Sub

    ''' <summary>
    ''' 全体管理ボタンを押した時の処理
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    ''' <history>
    ''' </history>
    Private Sub WholeManagementButton_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Logger.Info("WholeManagementButton_Click Start")

        '全体管理画面に遷移する
        Me.RedirectNextScreen(APPLICATION_ID)

        Logger.Info("WholeManagementButton_Click End")
    End Sub

    ''' <summary>
    ''' SAボタンを押した時の処理
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    ''' <history>
    ''' </history>
    Protected Sub FooterButtonSADummy_Click(sender As Object, e As System.EventArgs) Handles FooterButtonSADummy.Click
        Logger.Info("FooterButtonSADummy_Click Start")

        'SMステータスマネージメント画面に遷移する
        Me.RedirectNextScreen(MAINMENU_ID)

        Logger.Info("FooterButtonSADummy_Click End")
    End Sub

    '2013/12/10 TMEJ 河原 TMEJ次世代サービス 工程管理機能開発 END

    '2013/06/03 TMEJ 河原 IT9513_【A.STEP2】次世代e-CRBタブレット　新DB適応に向けた機能設計 START

    ''' <summary>
    ''' SMBボタンを押した時の処理
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    ''' <history>
    ''' 2013/06/03 TMEJ 河原 IT9513_【A.STEP2】次世代e-CRBタブレット　新DB適応に向けた機能設計
    ''' </history>
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

    '2013/06/03 TMEJ 河原 IT9513_【A.STEP2】次世代e-CRBタブレット　新DB適応に向けた機能設計 END

    '2013/12/10 TMEJ 河原 TMEJ次世代サービス 工程管理機能開発 START

    ' ''' <summary>
    ' ''' 来店管理ボタンを押した時の処理
    ' ''' </summary>
    ' ''' <param name="sender"></param>
    ' ''' <param name="e"></param>
    ' ''' <remarks></remarks>
    'Protected Sub VisitManagementFooterButton_Click(ByVal sender As Object, _
    '                                                ByVal e As System.EventArgs) Handles VisitManagementFooterButton.Click
    '    Logger.Info(String.Format(CultureInfo.CurrentCulture _
    '                , "{0}.{1} START" _
    '                , Me.GetType.ToString _
    '                , System.Reflection.MethodBase.GetCurrentMethod.Name))

    '    '来店管理画面に遷移する
    '    Me.RedirectNextScreen(VISIT_MANAGEMENT_LIST_PAGE)

    '    Logger.Info(String.Format(CultureInfo.CurrentCulture _
    '                , "{0}.{1} END" _
    '                , Me.GetType.ToString _
    '                , System.Reflection.MethodBase.GetCurrentMethod.Name))
    'End Sub

    '2013/12/10 TMEJ 河原 TMEJ次世代サービス 工程管理機能開発 END

#End Region

#Region "その他"

    ''' <summary>
    ''' 文字列変換
    ''' </summary>
    ''' <param name="str"></param>
    ''' <returns>変換値</returns>
    Private Function SetNullToString(ByVal str As String, _
                                     Optional ByVal strNull As String = "") As String

        ' 空白チェック
        If String.IsNullOrEmpty(str) Then
            Return strNull
        End If

        Return str

    End Function

    '''-----------------------------------------------------------------------
    ''' <summary>
    ''' 時間変換 「hh:mm～」「MM:dd～」
    ''' </summary>
    ''' <param name="time">対象時間</param>
    ''' <param name="fromToWord">「～」の文言</param>
    ''' <param name="fromToType">文言配置位置「0：最初に結合、1：最後に結合」</param>
    ''' <returns>変換値</returns>
    '''-----------------------------------------------------------------------
    Private Function SetDateTimeToString(ByVal time As Date, _
                                         ByVal fromToWord As String, _
                                         Optional fromToType As String = FROMTO_FROM) As String

        Dim strResult As String
        Try
            ' 日付チェック
            If time.Equals(DateTime.MinValue) Then
                Return Me.wordNoTime
            End If
            If Not nowDateTime.Date = time.Date Then

                '2013/12/10 TMEJ 河原 TMEJ次世代サービス 工程管理機能開発 START

                '「MM/dd」
                'strResult = time.ToString("MM/dd", CultureInfo.CurrentCulture)

                strResult = DateTimeFunc.FormatDate(11, time)

                '2013/12/10 TMEJ 河原 TMEJ次世代サービス 工程管理機能開発 END

            Else
                '「hh:mm」
                strResult = DateTimeFunc.FormatDate(14, time)
            End If
        Catch ex As FormatException
            Return Me.wordNoTime
        End Try
        If FROMTO_FROM.Equals(fromToType) Then
            Return String.Concat(fromToWord, strResult)
        ElseIf FROMTO_TO.Equals(fromToType) Then
            Return String.Concat(strResult, fromToWord)
        Else
            Return Me.wordNoTime
        End If

    End Function

#End Region

    '2013/12/10 TMEJ 河原 次世代サービス 工程管理機能開発 START

#Region "画面遷移メソッド"

#Region "R/O一覧画面"

    ''' <summary>
    ''' R/O一覧画面遷移処理(パラメータ設定)
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub RedirectOrderList()

        '開始ログ
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                  , "{0}.{1} START" _
                  , Me.GetType.ToString _
                  , System.Reflection.MethodBase.GetCurrentMethod.Name))

        'ログインスタッフ情報取得
        Dim staffInfo As StaffContext = StaffContext.Current

        'SC3220101BusinessLogicインスタンス
        Using biz As New SC3220201BusinessLogic(Me.deliverypreAbnormalLt, Me.nowDateTime)

            '基幹コードへ変換処理
            Dim rowDmsCodeMap As ServiceCommonClassDataSet.DmsCodeMapRow = biz.ChangeDmsCode(staffInfo)

            '基幹販売店コードチェック
            If String.IsNullOrEmpty(rowDmsCodeMap.CODE1) Then
                '値無し

                'エラーログ
                Logger.Info(String.Format(CultureInfo.CurrentCulture _
                               , "{0}.{1} Err:rowDmsCodeMap.CODE1=NOTHING" _
                               , Me.GetType.ToString _
                               , System.Reflection.MethodBase.GetCurrentMethod.Name))

                'エラーメッセージ表示
                Me.ShowMessageBox(WordID.id909)

                '処理終了
                Exit Sub

            End If

            '基幹店舗コードチェック
            If String.IsNullOrEmpty(rowDmsCodeMap.CODE2) Then
                '値無し

                'エラーログ
                Logger.Info(String.Format(CultureInfo.CurrentCulture _
                               , "{0}.{1} Err:rowDmsCodeMap.CODE2=NOTHING" _
                               , Me.GetType.ToString _
                               , System.Reflection.MethodBase.GetCurrentMethod.Name))

                'エラーメッセージ表示
                Me.ShowMessageBox(WordID.id909)

                '処理終了
                Exit Sub

            End If

            '基幹アカウントチェック
            If String.IsNullOrEmpty(rowDmsCodeMap.ACCOUNT) Then
                '値無し

                'エラーログ
                Logger.Info(String.Format(CultureInfo.CurrentCulture _
                               , "{0}.{1} Err:rowDmsCodeMap.ACCOUNT=NOTHING" _
                               , Me.GetType.ToString _
                               , System.Reflection.MethodBase.GetCurrentMethod.Name))

                'エラーメッセージ表示
                Me.ShowMessageBox(WordID.id909)

                '処理終了
                Exit Sub

            End If

            '次画面遷移パラメータ設定

            '販売店コード
            Me.SetValue(ScreenPos.Next, SessionParam01, rowDmsCodeMap.CODE1)
            '店舗コード
            Me.SetValue(ScreenPos.Next, SessionParam02, rowDmsCodeMap.CODE2)
            'アカウント
            Me.SetValue(ScreenPos.Next, SessionParam03, rowDmsCodeMap.ACCOUNT)
            '来店者実績連番
            Me.SetValue(ScreenPos.Next, SessionParam04, String.Empty)
            'DMS予約ID
            Me.SetValue(ScreenPos.Next, SessionParam05, String.Empty)
            'RO
            Me.SetValue(ScreenPos.Next, SessionParam06, String.Empty)
            'RO_JOB_SEQ
            Me.SetValue(ScreenPos.Next, SessionParam07, String.Empty)
            'VIN
            Me.SetValue(ScreenPos.Next, SessionParam08, String.Empty)
            'ViewMode
            Me.SetValue(ScreenPos.Next, SessionParam09, EditMode)
            'DISP_NUM
            Me.SetValue(ScreenPos.Next, SessionDispNum, APPLICATIONID_ORDERLIST)

        End Using

        '基幹画面連携用フレーム呼出処理
        Me.ScreenTransition()


        '終了ログ
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                   , "{0}.{1} END" _
                   , Me.GetType.ToString _
                   , System.Reflection.MethodBase.GetCurrentMethod.Name))


    End Sub

#End Region

#Region "商品訴求コンテンツ画面"

    ''' <summary>
    ''' 商品訴求コンテンツ画面遷移処理(パラメータ設定)
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub RedirectProductsAppealContent()

        '開始ログ
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                  , "{0}.{1} START" _
                  , Me.GetType.ToString _
                  , System.Reflection.MethodBase.GetCurrentMethod.Name))

        'ログインスタッフ情報取得
        Dim staffInfo As StaffContext = StaffContext.Current

        '次画面遷移パラメータ設定

        '販売店コード
        Me.SetValue(ScreenPos.Next, SessionDealerCode, Space(1))
        '店舗コード
        Me.SetValue(ScreenPos.Next, SessionBranchCode, Space(1))
        'アカウント
        Me.SetValue(ScreenPos.Next, SessionLoginUserID, Space(1))
        '来店者実績連番
        Me.SetValue(ScreenPos.Next, SessionSAChipID, String.Empty)
        'DMS予約ID
        Me.SetValue(ScreenPos.Next, SessionBASREZID, String.Empty)
        'RO
        Me.SetValue(ScreenPos.Next, SessionRO, String.Empty)
        'RO_JOB_SEQ       
        Me.SetValue(ScreenPos.Next, SessionSEQNO, String.Empty)
        'VIN
        Me.SetValue(ScreenPos.Next, SessionVINNO, String.Empty)
        'ViewMode
        Me.SetValue(ScreenPos.Next, SessionViewMode, ReadMode)


        '商品訴求コンテンツ画面遷移
        Me.RedirectNextScreen(APPLICATIONID_PRODUCTSAPPEALCONTENT)


        '終了ログ
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                   , "{0}.{1} END" _
                   , Me.GetType.ToString _
                   , System.Reflection.MethodBase.GetCurrentMethod.Name))


    End Sub

#End Region

#Region "キャンペーン画面"

    ''' <summary>
    ''' キャンペーン画面遷移処理(パラメータ設定)
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub RedirectCampaign()

        '開始ログ
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                  , "{0}.{1} START" _
                  , Me.GetType.ToString _
                  , System.Reflection.MethodBase.GetCurrentMethod.Name))

        'ログインスタッフ情報取得
        Dim staffInfo As StaffContext = StaffContext.Current

        'SC3220101BusinessLogicインスタンス
        Using biz As New SC3220201BusinessLogic(Me.deliverypreAbnormalLt, Me.nowDateTime)

            '基幹コードへ変換処理
            Dim rowDmsCodeMap As ServiceCommonClassDataSet.DmsCodeMapRow = biz.ChangeDmsCode(staffInfo)

            '基幹販売店コードチェック
            If String.IsNullOrEmpty(rowDmsCodeMap.CODE1) Then
                '値無し

                'エラーログ
                Logger.Info(String.Format(CultureInfo.CurrentCulture _
                               , "{0}.{1} Err:rowDmsCodeMap.CODE1=NOTHING" _
                               , Me.GetType.ToString _
                               , System.Reflection.MethodBase.GetCurrentMethod.Name))

                'エラーメッセージ表示
                Me.ShowMessageBox(WordID.id909)

                '処理終了
                Exit Sub

            End If

            '基幹店舗コードチェック
            If String.IsNullOrEmpty(rowDmsCodeMap.CODE2) Then
                '値無し

                'エラーログ
                Logger.Info(String.Format(CultureInfo.CurrentCulture _
                               , "{0}.{1} Err:rowDmsCodeMap.CODE2=NOTHING" _
                               , Me.GetType.ToString _
                               , System.Reflection.MethodBase.GetCurrentMethod.Name))

                'エラーメッセージ表示
                Me.ShowMessageBox(WordID.id909)

                '処理終了
                Exit Sub

            End If

            '基幹アカウントチェック
            If String.IsNullOrEmpty(rowDmsCodeMap.ACCOUNT) Then
                '値無し

                'エラーログ
                Logger.Info(String.Format(CultureInfo.CurrentCulture _
                               , "{0}.{1} Err:rowDmsCodeMap.ACCOUNT=NOTHING" _
                               , Me.GetType.ToString _
                               , System.Reflection.MethodBase.GetCurrentMethod.Name))

                'エラーメッセージ表示
                Me.ShowMessageBox(WordID.id909)

                '処理終了
                Exit Sub

            End If

            '次画面遷移パラメータ設定

            '販売店コード
            Me.SetValue(ScreenPos.Next, SessionParam01, rowDmsCodeMap.CODE1)
            '店舗コード
            Me.SetValue(ScreenPos.Next, SessionParam02, rowDmsCodeMap.CODE2)
            'アカウント
            Me.SetValue(ScreenPos.Next, SessionParam03, rowDmsCodeMap.ACCOUNT)
            '来店者実績連番
            Me.SetValue(ScreenPos.Next, SessionParam04, String.Empty)
            'DMS予約ID
            Me.SetValue(ScreenPos.Next, SessionParam05, String.Empty)
            'RO
            Me.SetValue(ScreenPos.Next, SessionParam06, String.Empty)
            'RO_JOB_SEQ
            Me.SetValue(ScreenPos.Next, SessionParam07, String.Empty)
            'VIN
            Me.SetValue(ScreenPos.Next, SessionParam08, String.Empty)
            'ViewMode
            '2014/07/01 TMEJ 丁　 TMT_UAT対応 START
            'Me.SetValue(ScreenPos.Next, SessionParam09, EditMode)
            Me.SetValue(ScreenPos.Next, SessionParam09, ReadMode)
            '2014/07/01 TMEJ 丁　 TMT_UAT対応 END
            'DISP_NUM
            Me.SetValue(ScreenPos.Next, SessionDispNum, APPLICATIONID_CAMPAIGN)

        End Using


        '基幹画面連携用フレーム呼出処理
        Me.ScreenTransition()


        '終了ログ
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                     , "{0}.{1} END" _
                     , Me.GetType.ToString _
                     , System.Reflection.MethodBase.GetCurrentMethod.Name))


    End Sub

#End Region

#Region "全画面共通(基幹画面連携用フレーム呼出処理)"

    ''' <summary>
    ''' 基幹画面連携用フレーム呼出処理
    ''' </summary>
    ''' <history>
    ''' </history>
    Private Sub ScreenTransition()

        '開始ログ
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                  , "{0}.{1} START" _
                  , Me.GetType.ToString _
                  , System.Reflection.MethodBase.GetCurrentMethod.Name))

        '基幹画面連携用フレーム呼出
        Me.RedirectNextScreen(APPLICATIONID_FRAMEID)

        '終了ログ
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                   , "{0}.{1} END" _
                   , Me.GetType.ToString _
                   , System.Reflection.MethodBase.GetCurrentMethod.Name))

    End Sub

#End Region

#End Region

    '2013/12/10 TMEJ 河原 次世代サービス 工程管理機能開発 END

End Class
