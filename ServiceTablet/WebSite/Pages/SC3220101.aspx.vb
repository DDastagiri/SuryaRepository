'------------------------------------------------------------------------------
'SC3220101.aspx.vb
'------------------------------------------------------------------------------
'機能：SAマネジメントメインメニュー
'補足：
'作成： 2012/07/28 TMEJ 日比野
'更新： 2012/09/19 TMEJ 日比野 【SERVICE_2】受付待ち工程の追加対応
'更新： 2012/09/21 TMEJ 小澤 【SERVICE_2】SAストール予約受付機能開発（仕分けNo.35）
'更新： 2012/11/13 TMEJ 丁 【A.STEP2】次世代サービス アクティブインジゲータ対応 No.1
'更新： 2013/03/12 TMEJ 岩城 【A.STEP1】来店受付管理オペレーション確立に向けた評価アプリ作成
'更新： 2013/06/03 TMEJ 小澤 【A.STEP2】次世代e-CRBサービスタブレット 新DB適応に向けた機能開発
'更新： 2013/12/10 TMEJ 河原 次世代サービス 工程管理機能開発
'更新： 2014/07/01 TMEJ 丁　 TMT_UAT対応
'更新： 2015/09/01 TMEJ 井上 (トライ店システム評価)サービス入庫予約のユーザ管理機能開発
'更新： 2018/06/22 NSK 可児 TKM Next Gen e-CRB Project Application development Block B-1  Pマーク、Lマークなどを表示
'更新：
'------------------------------------------------------------------------------
Option Strict On
Option Explicit On

Imports System.Web.Services
Imports System.Web.Script.Serialization
Imports System.Globalization
Imports System.Data
Imports Toyota.eCRB.SystemFrameworks.Core

Imports Toyota.eCRB.iCROP.BizLogic.SC3220101
Imports Toyota.eCRB.iCROP.BizLogic.SC3220101.SC3220101BusinessLogic
Imports Toyota.eCRB.iCROP.DataAccess.SC3220101.SC3220101DataSet

Imports Toyota.eCRB.SMBLinkage.GetUserList.Api.BizLogic
Imports Toyota.eCRB.SMBLinkage.GetUserList.Api.DataAccess

Imports Toyota.eCRB.CommonUtility.SMBCommonClass.Api.BizLogic
Imports Toyota.eCRB.CommonUtility.SMBCommonClass.Api.DataAccess
Imports Toyota.eCRB.SMBLinkage.GetServiceLT.Api.DataAccess.IC3810701DataSet

'2013/12/10 TMEJ 河原 次世代サービス 工程管理機能開発 START

Imports Toyota.eCRB.CommonUtility.ServiceCommonClass.Api.DataAccess

'2013/12/10 TMEJ 河原 次世代サービス 工程管理機能開発 END

Partial Class Pages_Default
    Inherits BasePage

#Region "定数"

    ''' <summary>
    ''' 画面ID
    ''' </summary>
    ''' <remarks></remarks>
    Private Const APPLICATION_ID As String = "SC3220101"
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
    ''' チップの洗車有無:洗車なし
    ''' </summary>
    ''' <remarks></remarks>
    Private Const CHIP_WASHING_NONE As String = "0"

    ''' <summary>
    ''' チップの洗車状況:洗車未完了
    ''' </summary>
    ''' <remarks></remarks>
    Private Const CHIP_WASHING_IMPERFECT As String = "1"

    ''' <summary>
    ''' チップの洗車有無:洗車完了
    ''' </summary>
    ''' <remarks></remarks>
    Private Const CHIP_WASHING_FINISH As String = "2"
    ''' <summary>
    ''' チップのR/O有無:なし
    ''' </summary>
    ''' <remarks></remarks>
    Private Const CHIP_RO_EXISTENCE_FALSE As String = "0"
    ''' <summary>
    ''' チップのR/Oの有無:あり
    ''' </summary>
    ''' <remarks></remarks>
    Private Const CHIP_RO_EXISTENCE_TRUE As String = "1"
    ''' <summary>
    ''' フッターコード：メインメニュー
    ''' </summary>
    ''' <remarks></remarks>
    Private Const FOOTER_MAINMENU As Integer = 100

    '2013/06/03 TMEJ 小澤 【A.STEP2】次世代e-CRBサービスタブレット 新DB適応に向けた機能開発 START
    ''' <summary>
    ''' フッターコード：SMB
    ''' </summary>
    ''' <remarks></remarks>
    Private Const FOOTER_SMB As Integer = 800
    '2013/06/03 TMEJ 小澤 【A.STEP2】次世代e-CRBサービスタブレット 新DB適応に向けた機能開発 END

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
    ''' <summary>
    ''' 詳細ポップアップのサブボタンステース：非活性
    ''' </summary>
    ''' <remarks></remarks>
    Private Const DETAILS_SUB_BUTTON_INACTIVE As String = "0"
    ''' <summary>
    ''' 詳細ポップアップのサブボタンステース：非活性
    ''' </summary>
    ''' <remarks></remarks>
    Private Const DETAILS_SUB_BUTTON_ACTIVE As String = "1"
    ''' <summary>
    ''' 詳細ポップアップのマーク：非表示
    ''' </summary>
    ''' <remarks></remarks>
    Private Const DETAILS_MARK_INACTIVE As String = "0"
    ''' <summary>
    ''' 詳細ポップアップのマーク：表示
    ''' </summary>
    ''' <remarks></remarks>
    Private Const DETAILS_MARK_ACTIVE As String = "1"
    '2018/06/22 NSK 可児 TKM Next Gen e-CRB Project Application development Block B-1  Pマーク、Lマークなどを表示 START
    ''' <summary>
    ''' 詳細ポップアップのPマーク：表示
    ''' </summary>
    ''' <remarks></remarks>
    Private Const DETAILS_PMARK_ACTIVE As String = "1"
    ''' <summary>
    ''' 詳細ポップアップのLマーク：表示
    ''' </summary>
    ''' <remarks></remarks>
    Private Const DETAILS_LMARK_ACTIVE As String = "2"
    '2018/06/22 NSK 可児 TKM Next Gen e-CRB Project Application development Block B-1  Pマーク、Lマークなどを表示 END
    ''' <summary>
    ''' 詳細ポップアップの顧客区分：自社客
    ''' </summary>
    ''' <remarks></remarks>
    Private Const DETAILS_CUSTERMAR_STATUS_TRUE As String = "1"
    ''' <summary>
    ''' 詳細ポップアップの顧客区分：未取引客
    ''' </summary>
    ''' <remarks></remarks>
    Private Const DETAILS_CUSTERMAR_STATUS_FALSE As String = "2"
    ''' <summary>
    ''' 詳細ポップアップのR/O有無：なし
    ''' </summary>
    ''' <remarks></remarks>
    Private Const DETAILS_RO_FLG_FALSE As String = "0"
    ''' <summary>
    ''' 詳細ポップアップのR/O有無：あり
    ''' </summary>
    ''' <remarks></remarks>
    Private Const DETAILS_RO_FLG_TRUE As String = "1"

    ''' <summary>
    ''' 予約_受付納車区分:Waiting
    ''' </summary>
    ''' <remarks></remarks>
    Private Const REZ_RECEPTION_WAITING As String = "0"

    ''' <summary>
    ''' 予約_受付納車区分:Drop off
    ''' </summary>
    ''' <remarks></remarks>
    Private Const REZ_RECEPTION_DROP_OFF As String = "4"

    ''' <summary>
    ''' メッセージID管理
    ''' </summary>
    ''' <remarks></remarks>
    Private Enum MsgID
        ''' <summary>タイムアウト</summary>
        id901 = 901
        ''' <summary></summary>
        id902 = 902
        ''' <summary>顧客情報が未登録</summary>
        id903 = 903
        ''' <summary>R/Oが未作成</summary>
        id904 = 904

        '2013/12/10 TMEJ 河原 次世代サービス 工程管理機能開発 START
        ''' <summary>予期せぬエラー</summary>
        id905 = 905
        '2013/12/10 TMEJ 河原 次世代サービス 工程管理機能開発 END

    End Enum

    ''' <summary>
    ''' R/O一覧画面ID
    ''' </summary>
    ''' <remarks></remarks>
    Private Const REPAIR_ORDERE_LIST_PAGE As String = "SC3160101"
    ''' <summary>
    ''' 追加作業一覧ページID
    ''' </summary>
    ''' <remarks></remarks>
    Private Const ADDITION_WORK_LIST_PAGE As String = "SC3170101"
    ''' <summary>
    ''' 顧客詳細ページID
    ''' </summary>
    ''' <remarks></remarks>
    Private Const CUSTOMER_DETAILS_PAGE As String = "SC3080208"
    ''' <summary>
    ''' R/O参照画面ID
    ''' </summary>
    ''' <remarks></remarks>
    Private Const REPAIR_ORDERE_PREVIEW_PAGE As String = "SC3160208"
    ''' <summary>
    ''' 来店管理画面ID
    ''' </summary>
    ''' <remarks></remarks>
    ''' <history>
    ''' 2013/03/12 TMEJ 岩城 【A.STEP1】来店受付管理オペレーション確立に向けた評価アプリ作成
    ''' </history>
    Private Const VISIT_MANAGEMENT_PAGE As String = "SC3100303"
    ''' <summary>
    ''' 全体管理画面ID
    ''' </summary>
    ''' <remarks></remarks>
    ''' <history>
    ''' 2013/03/12 TMEJ 岩城 【A.STEP1】来店受付管理オペレーション確立に向けた評価アプリ作成
    ''' </history>
    Private Const GENERAL_MANAGEMENT_PAGE As String = "SC3220201"
    '2013/06/03 TMEJ 小澤 【A.STEP2】次世代e-CRBサービスタブレット 新DB適応に向けた機能開発 START
    ''' <summary>
    ''' 工程管理画面ID
    ''' </summary>
    ''' <remarks></remarks>
    Private Const PROCESS_CONTROL_PAGE As String = "SC3240101"
    '2013/06/03 TMEJ 小澤 【A.STEP2】次世代e-CRBサービスタブレット 新DB適応に向けた機能開発 END

    '2013/12/10 TMEJ 河原 次世代サービス 工程管理機能開発 START

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
    ''' プレビューフラグ("0"；プレビュー) 
    ''' </summary>
    Private Const PreviewFlag As String = "0"

    ''' <summary>
    ''' 編集モードフラグ("1"；リードオンリー) 
    ''' </summary>
    Private Const ReadMode As String = "1"

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
    ''' 基幹画面連携用フレーム用セッション名("Session.Param11")
    ''' </summary>
    Private Const SessionParam11 As String = "Session.Param11"

    ''' <summary>
    ''' 基幹画面連携用フレーム用セッション名("Session.Param12")
    ''' </summary>
    Private Const SessionParam12 As String = "Session.Param12"

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

    ''' <summary>
    ''' 詳細ポップアップサブボタン("10"：顧客詳細ボタン)
    ''' </summary>
    ''' <remarks></remarks>
    Private Const DETAILS_SUB_BUTTON_CUSTOMER As Long = 10

    ''' <summary>
    ''' 詳細ポップアップサブボタン("20"：RO参照ボタン)
    ''' </summary>
    ''' <remarks></remarks>
    Private Const DETAILS_SUB_BUTTON_RO As Long = 20

    '2013/12/10 TMEJ 河原 次世代サービス 工程管理機能開発 END

#End Region

#Region "メンバ変数"

    ''' <summary>
    ''' ユーザ情報（セッションより）
    ''' </summary>
    ''' <remarks></remarks>
    Private objStaffContext As StaffContext

#End Region
    '2018/06/22 NSK 可児 TKM Next Gen e-CRB Project Application development Block B-1  Pマーク、Lマークなどを表示 START
#Region "列挙型"
#Region "文言ID"
    ''' <summary>
    ''' アイコンの文言
    ''' </summary>
    ''' <remarks></remarks>
    Private Enum WordID
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

    End Enum
#End Region

#End Region
    '2018/06/22 NSK 可児 TKM Next Gen e-CRB Project Application development Block B-1  Pマーク、Lマークなどを表示 END

#Region "初期処理"

    ''' <summary>
    ''' ページロード時の処理
    ''' </summary>
    ''' <param name="sender">イベント発生元</param>
    ''' <param name="e">イベントデータ</param>
    ''' <remarks></remarks>
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Logger.Info("Page_Load Start")

        'ユーザ情報の取得.
        objStaffContext = StaffContext.Current

        'フッタボタンの初期化を行う.
        InitFooterButton()

        '非同期通信の場合
        If "GetDataAjax".Equals(Request("method")) Then

            Me.Page_Flick()

            Return

        End If

        '更新：2012/11/13 TMEJ 丁 【A.STEP2】次世代サービス アクティブインジゲータ対応 No.1 START

        HiddenButtonDetailCustomer.Attributes.Add("OnClick", "FooterButtonclick(1200)")

        HiddenButtonDetailRo.Attributes.Add("OnClick", "FooterButtonclick(1300)")

        '更新：2012/11/13 TMEJ 丁 【A.STEP2】次世代サービス アクティブインジゲータ対応 No.1 END

        '2018/06/22 NSK 可児 TKM Next Gen e-CRB Project Application development Block B-1  Pマーク、Lマークなどを表示 START
        'hiddenコントロールにアイコン用の文言を設定する
        SendWordToIcon()
        '2018/06/22 NSK 可児 TKM Next Gen e-CRB Project Application development Block B-1  Pマーク、Lマークなどを表示 END

        Logger.Info("Page_Load End")
    End Sub

    ''' <summary>
    ''' 現在のサーバ時間をHiddenFieldにセットする.
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub SetServerCurrentTime()
        Logger.Info("SetServerCurrentTime Start")

        '2013/12/10 TMEJ 河原 次世代サービス 工程管理機能開発 START

        'サーバ時間を文字列として取得して、HiddenFieldに格納.（yyyy/MM/dd HH:mm:ss形式）
        'Me.HiddenServerTime.Value = DateTimeFunc.FormatDate(1, DateTimeFunc.Now(objStaffContext.DlrCD))

        Me.HiddenServerTime.Value = String.Format(CultureInfo.CurrentCulture, "{0:yyyy/MM/dd HH:mm:ss}", DateTimeFunc.Now(objStaffContext.DlrCD))

        '2013/12/10 TMEJ 河原 次世代サービス 工程管理機能開発 END

        Logger.Info("SetServerCurrentTime End SetTime:" + Me.HiddenServerTime.Value)
    End Sub

    '2018/06/22 NSK 可児 TKM Next Gen e-CRB Project Application development Block B-1  Pマーク、Lマークなどを表示 START
    ''' <summary>
    ''' hiddenコントロールにアイコンの文言を設定する
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub SendWordToIcon()
        Dim sbWord As StringBuilder = New StringBuilder

        With sbWord
            .Append("{""WordM"":""")
            .Append(HttpUtility.HtmlEncode(WebWordUtility.GetWord(APPLICATION_ID, WordID.id10001)))
            .Append(""",""WordB"":""")
            .Append(HttpUtility.HtmlEncode(WebWordUtility.GetWord(APPLICATION_ID, WordID.id10002)))
            .Append(""",""WordE"":""")
            .Append(HttpUtility.HtmlEncode(WebWordUtility.GetWord(APPLICATION_ID, WordID.id10003)))
            .Append(""",""WordT"":""")
            .Append(HttpUtility.HtmlEncode(WebWordUtility.GetWord(APPLICATION_ID, WordID.id10004)))
            .Append(""",""WordP"":""")
            .Append(HttpUtility.HtmlEncode(WebWordUtility.GetWord(APPLICATION_ID, WordID.id10005)))
            .Append(""",""WordL"":""")
            .Append(HttpUtility.HtmlEncode(WebWordUtility.GetWord(APPLICATION_ID, WordID.id10006)))
            .Append("""}")
        End With

        Me.HiddenIconWord.Value = sbWord.ToString()

    End Sub
    '2018/06/22 NSK 可児 TKM Next Gen e-CRB Project Application development Block B-1  Pマーク、Lマークなどを表示 END

#End Region

#Region "チップ更新用"

    ''' <summary>
    ''' 工程表示エリアをフリック＆リリース
    ''' </summary>
    ''' <remarks></remarks>
    ''' <history>
    ''' 2013/06/13 TMEJ 小澤 【A.STEP2】次世代e-CRBサービスタブレット 新DB適応に向けた機能開発
    ''' </history>
    Private Sub Page_Flick()
        Logger.Info("Page_Flick Start")

        Try

            '2012/09/19 TMEJ 日比野 【SERVICE_2】 受付待ち工程の追加対応 START

            '2013/12/10 TMEJ 河原 次世代サービス 工程管理機能開発 START

            'Dim biz As New SC3220101BusinessLogic
            'SC3220101BusinessLogicインスタンス
            Using biz As New SC3220101BusinessLogic

                '2013/12/10 TMEJ 河原 次世代サービス 工程管理機能開発 END

                '担当SA情報の取得
                'Dim operationCodeList As New List(Of Long)
                'operationCodeList.Add(9)
                'Dim IC3810601Biz As New IC3810601BusinessLogic

                Dim SATable As IC3810601DataSet.AcknowledgeStaffListDataTable = _
                                    biz.GetAcknowledgeStaffList(objStaffContext.DlrCD, _
                                                                objStaffContext.BrnCD)

                'サービス標準LT取得
                Dim dtStanderdLT As StandardLTListDataTable = biz.GetStandardLTList(objStaffContext.DlrCD,
                                                                                    objStaffContext.BrnCD)

                '受付エリアから納車作業エリアの情報を取得
                Dim ChipTable As SC3220101ChipInfoDataTable = biz.GetVisitChip(dtStanderdLT)

                '来店(受付待ち)エリアの情報を取得
                '2013/06/13 TMEJ 小澤 【A.STEP2】次世代e-CRBサービスタブレット 新DB適応に向けた機能開発 START
                'Dim dtReseptionWait As SC3220101VisitAreaInfoDataTable = biz.GetVisitAreaChip()
                Dim dtReseptionWait As SC3220101VisitAreaInfoDataTable = biz.GetVisitAreaChip(objStaffContext)
                '2013/06/13 TMEJ 小澤 【A.STEP2】次世代e-CRBサービスタブレット 新DB適応に向けた機能開発 END

                Dim dataList As List(Of SAItem) = biz.CreateVisitChip(SATable, _
                                                                      ChipTable, _
                                                                      dtReseptionWait, _
                                                                      dtStanderdLT)

                '2012/09/19 TMEJ 日比野 【SERVICE_2】 受付待ち工程の追加対応 END

                'HiddenFieldにJSON形式へ変換したデータを設定
                Dim serializer As JavaScriptSerializer = New JavaScriptSerializer
                Me.HiddenChipData.Value = serializer.Serialize(dataList)


                '2013/12/10 TMEJ 河原 次世代サービス 工程管理機能開発 START

            End Using

            '2013/12/10 TMEJ 河原 次世代サービス 工程管理機能開発 END


        Catch ex As OracleExceptionEx When ex.Number = 1013
            'ORACLEのタイムアウトのみ処理
            Me.ShowMessageBox(MsgID.id901)

        Finally
            'サーバ時間を取得し、設定する
            Me.SetServerCurrentTime()
        End Try

        Logger.Info("Page_Flick END")
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
    Public Overrides Function DeclareCommonMasterFooter( _
        ByVal commonMaster As Toyota.eCRB.SystemFrameworks.Web.CommonMasterPage, _
        ByRef category As Toyota.eCRB.SystemFrameworks.Web.FooterMenuCategory) As Integer()

        Logger.Info("Override DeclareCommonMasterFooter Start")

        '2013/12/10 TMEJ 河原 次世代サービス 工程管理機能開発 START

        ''自ページの所属メニューを宣言
        category = FooterMenuCategory.MainMenu

        ''ログイン情報取得
        'Dim staffInfo As StaffContext = StaffContext.Current

        ''権限チェック
        'If staffInfo.OpeCD = iCROP.BizLogic.Operation.SVR Then
        '    'SVR権限

        '    '自ページの所属メニューを宣言
        '    category = FooterMenuCategory.WholeManagement

        'End If

        '2013/12/10 TMEJ 河原 次世代サービス 工程管理機能開発 END

        Logger.Info("Override DeclareCommonMasterFooter End")
        '表示非表示に関わらず、使用するサブメニューボタンを宣言
        Return New Integer() {}

    End Function

    ''' <summary>
    ''' フッターボタンの初期化
    ''' </summary>
    ''' <remarks></remarks>
    ''' <History>
    ''' 2013/12/10 TMEJ 河原 次世代サービス 工程管理機能開発
    ''' </History>
    Private Sub InitFooterButton()
        Logger.Info("InitFooterButton Start")

        'メインメニューボタンの設定
        Dim mainMenuButton As CommonMasterFooterButton = _
            CType(Me.Master, CommonMasterPage).GetFooterButton(FOOTER_MAINMENU)
        AddHandler mainMenuButton.Click, AddressOf MainMenuButton_Click
        '更新：2012/11/13 TMEJ 丁 【A.STEP2】次世代サービス アクティブインジゲータ対応 No.1 START
        mainMenuButton.OnClientClick = "FooterButtonclick(" & FOOTER_MAINMENU & ");"
        '更新：2012/11/13 TMEJ 丁 【A.STEP2】次世代サービス アクティブインジゲータ対応 No.1 END

        '2013/12/10 TMEJ 河原 次世代サービス 工程管理機能開発 START

        ''2013/06/03 TMEJ 小澤 【A.STEP2】次世代e-CRBサービスタブレット 新DB適応に向けた機能開発 START
        ''SMBボタンの設定
        'Dim smbButton As CommonMasterFooterButton = _
        '    CType(Me.Master, CommonMasterPage).GetFooterButton(FOOTER_SMB)
        'AddHandler smbButton.Click, AddressOf SMBButton_Click
        'smbButton.OnClientClick = "FooterButtonclick(" & FOOTER_SMB & ");"
        ''2013/06/03 TMEJ 小澤 【A.STEP2】次世代e-CRBサービスタブレット 新DB適応に向けた機能開発 END

        ''顧客詳細ボタンの設定
        ''Dim customerButton As CommonMasterFooterButton = _
        ''CType(Me.Master, CommonMasterPage).GetFooterButton(FOOTER_CUSTOMER)

        ''R/Oボタンの設定
        'Dim roButton As CommonMasterFooterButton = _
        '    CType(Me.Master, CommonMasterPage).GetFooterButton(FOOTER_RO)
        'AddHandler roButton.Click, AddressOf RoButton_Click
        ''更新：2012/11/13 TMEJ 丁 【A.STEP2】次世代サービス アクティブインジゲータ対応 No.1 START
        'roButton.OnClientClick = "FooterButtonclick(" & FOOTER_RO & ");"
        ''更新：2012/11/13 TMEJ 丁 【A.STEP2】次世代サービス アクティブインジゲータ対応 No.1 END
        ' ''追加作業ボタンの設定
        'Dim addListButton As CommonMasterFooterButton = _
        '    CType(Me.Master, CommonMasterPage).GetFooterButton(FOOTER_ADD_LIST)
        'AddHandler addListButton.Click, AddressOf AddListButton_Click
        ''更新：2012/11/13 TMEJ 丁 【A.STEP2】次世代サービス アクティブインジゲータ対応 No.1 START
        'addListButton.OnClientClick = "FooterButtonclick(" & FOOTER_ADD_LIST & ");"
        ''更新：2012/11/13 TMEJ 丁 【A.STEP2】次世代サービス アクティブインジゲータ対応 No.1 END
        ''スケジュールボタンのイベント設定
        'Dim scheduleButton As CommonMasterFooterButton = _
        '    CType(Me.Master, CommonMasterPage).GetFooterButton(FOOTER_SCHEDULE)
        'scheduleButton.OnClientClick = "return schedule.appExecute.executeCaleNew();"

        'R/Oボタンの設定
        Dim roButton As CommonMasterFooterButton = _
            CType(Me.Master, CommonMasterPage).GetFooterButton(FooterMenuCategory.RepairOrderList)
        AddHandler roButton.Click, AddressOf RoButton_Click
        roButton.OnClientClick = "FooterButtonclick(" & FooterMenuCategory.RepairOrderList & ");"

        '来店管理
        Dim ReserveManagementButton As CommonMasterFooterButton = _
        CType(Me.Master, CommonMasterPage).GetFooterButton(FooterMenuCategory.ReserveManagement)
        AddHandler ReserveManagementButton.Click, AddressOf ReserveManagement_Click
        ReserveManagementButton.OnClientClick = "return FooterButtonclick(" & FooterMenuCategory.ReserveManagement & ");"

        '電話帳ボタンのイベント設定
        Dim telDirectoryButton As CommonMasterFooterButton = _
        CType(Me.Master, CommonMasterPage).GetFooterButton(FooterMenuCategory.Contact)
        telDirectoryButton.OnClientClick = "return schedule.appExecute.executeCont();"

        'ログイン情報取得
        Dim staffInfo As StaffContext = StaffContext.Current

        '権限チェック
        If staffInfo.OpeCD = iCROP.BizLogic.Operation.SVR Then
            'SVR権限

            '全体管理
            Dim WholeManagementButton As CommonMasterFooterButton = _
            CType(Me.Master, CommonMasterPage).GetFooterButton(FooterMenuCategory.WholeManagement)
            AddHandler WholeManagementButton.Click, AddressOf WholeManagementButton_Click
            WholeManagementButton.OnClientClick = "return FooterButtonclick(" & FooterMenuCategory.WholeManagement & ");"

        Else
            '上記以外(SM権限)

            '顧客情報画面(ヘッダー顧客検索機能へフォーカス)
            Dim customerButton As CommonMasterFooterButton = _
            CType(Me.Master, CommonMasterPage).GetFooterButton(FooterMenuCategory.CustomerDetail)
            customerButton.OnClientClick = "FooterButtonclick(" & FooterMenuCategory.CustomerDetail & ");"

            '商品訴求コンテンツ
            Dim productsAppealContentButton As CommonMasterFooterButton = _
            CType(Me.Master, CommonMasterPage).GetFooterButton(FooterMenuCategory.GoodsSolicitationContents)
            AddHandler productsAppealContentButton.Click, AddressOf productsAppealContentButton_Click
            productsAppealContentButton.OnClientClick = "return FooterButtonclick(" & FooterMenuCategory.GoodsSolicitationContents & ");"

            'キャンペーン
            Dim campaignButton As CommonMasterFooterButton = _
            CType(Me.Master, CommonMasterPage).GetFooterButton(FooterMenuCategory.Campaign)
            AddHandler campaignButton.Click, AddressOf campaignButton_Click
            campaignButton.OnClientClick = "return FooterButtonclick(" & FooterMenuCategory.Campaign & ");"

            'SMB
            Dim smbButton As CommonMasterFooterButton = _
            CType(Me.Master, CommonMasterPage).GetFooterButton(FooterMenuCategory.SMB)
            AddHandler smbButton.Click, AddressOf SMBButton_Click
            smbButton.OnClientClick = "return FooterButtonclick(" & FooterMenuCategory.SMB & ");"

        End If



        '2013/12/10 TMEJ 河原 次世代サービス 工程管理機能開発 END



        Logger.Info("InitFooterButton End")
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
        Logger.Info("MainMenuButton_Click Start")

        '2013/12/10 TMEJ 河原 次世代サービス 工程管理機能開発 START

        ''再表示
        'Me.RedirectNextScreen(APPLICATION_ID)

        'ログイン情報取得
        Dim staffInfo As StaffContext = StaffContext.Current

        '権限チェック
        If staffInfo.OpeCD = iCROP.BizLogic.Operation.SVR Then
            'SVR権限

            '未振当て一覧画面に遷移する
            Me.RedirectNextScreen(ASSIGN_LIST_PAGE)

        Else

            '全体管理画面に遷移する
            Me.RedirectNextScreen(GENERAL_MANAGEMENT_PAGE)

        End If

        '2013/12/10 TMEJ 河原 次世代サービス 工程管理機能開発 END

        Logger.Info("MainMenuButton_Click End")
    End Sub

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

    '2013/12/10 TMEJ 河原 次世代サービス 工程管理機能開発 START

    ' ''' <summary>
    ' ''' 追加作業ボタンを押した時の処理
    ' ''' </summary>
    ' ''' <param name="sender"></param>
    ' ''' <param name="e"></param>
    ' ''' <remarks></remarks>
    'Private Sub AddListButton_Click(ByVal sender As Object, ByVal e As System.EventArgs)
    '    Logger.Info("AddListButton_Click Start")

    '    '追加作業一覧画面に遷移する
    '    Me.RedirectNextScreen(ADDITION_WORK_LIST_PAGE)

    '    Logger.Info("AddListButton_Click End")
    'End Sub

    ' ''' <summary>
    ' ''' フッターの来店管理ボタンタップ処理
    ' ''' </summary>
    ' ''' <param name="sender">イベント発生元</param>
    ' ''' <param name="e">イベントデータ</param>
    ' ''' <remarks></remarks>
    ' ''' <history>
    ' ''' 2013/03/12 TMEJ 岩城 【A.STEP1】来店受付管理オペレーション確立に向けた評価アプリ作成
    ' ''' </history>
    'Protected Sub FooterButtonDummy100_Click(sender As Object, e As System.EventArgs) Handles FooterButtonDummy100.Click
    '    Logger.Info("FooterButtonDummy100_Click Start")

    '    '来店管理画面に遷移する
    '    Me.RedirectNextScreen(VISIT_MANAGEMENT_PAGE)

    '    Logger.Info("FooterButtonDummy100_Click End")
    'End Sub

    ' ''' <summary>
    ' ''' フッターの全体管理ボタンタップ処理
    ' ''' </summary>
    ' ''' <param name="sender">イベント発生元</param>
    ' ''' <param name="e">イベントデータ</param>
    ' ''' <remarks></remarks>
    ' ''' <history>
    ' ''' 2013/03/12 TMEJ 岩城 【A.STEP1】来店受付管理オペレーション確立に向けた評価アプリ作成
    ' ''' </history>
    'Protected Sub FooterButtonDummy200_Click(sender As Object, e As System.EventArgs) Handles FooterButtonDummy200.Click
    '    Logger.Info("FooterButtonDummy200_Click Start")

    '    '全体管理画面に遷移する
    '    Me.RedirectNextScreen(GENERAL_MANAGEMENT_PAGE)

    '    Logger.Info("FooterButtonDummy200_Click End")
    'End Sub

    '2013/12/10 TMEJ 河原 次世代サービス 工程管理機能開発 END

    '2013/06/03 TMEJ 小澤 【A.STEP2】次世代e-CRBサービスタブレット 新DB適応に向けた機能開発 START

    '2013/12/10 TMEJ 河原 次世代サービス 工程管理機能開発 START

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
        Me.RedirectNextScreen(VISIT_MANAGEMENT_PAGE)

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
        Me.RedirectNextScreen(GENERAL_MANAGEMENT_PAGE)

        Logger.Info("WholeManagementButton_Click End")
    End Sub

    '2013/12/10 TMEJ 河原 次世代サービス 工程管理機能開発 END

    ''' <summary>
    ''' SMBボタンを押した時の処理
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    ''' <history>
    ''' 2013/06/03 TMEJ 小澤 【A.STEP2】次世代e-CRBサービスタブレット 新DB適応に向けた機能開発
    ''' </history>
    Private Sub SMBButton_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Logger.Info("FooterButtonDummy100_Click Start")

        '工程管理画面に遷移する
        Me.RedirectNextScreen(PROCESS_CONTROL_PAGE)

        Logger.Info("FooterButtonDummy100_Click End")
    End Sub

    '2013/06/03 TMEJ 小澤 【A.STEP2】次世代e-CRBサービスタブレット 新DB適応に向けた機能開発 END

    '2013/12/10 TMEJ 河原 次世代サービス 工程管理機能開発 START

    ''' <summary>
    ''' フッターのSAボタンタップ処理
    ''' </summary>
    ''' <param name="sender">イベント発生元</param>
    ''' <param name="e">イベントデータ</param>
    ''' <remarks></remarks>
    ''' <history>
    ''' </history>
    Protected Sub FooterButtonDummy300_Click(sender As Object, e As System.EventArgs) Handles FooterButtonDummy300.Click
        Logger.Info("FooterButtonDummy300_Click Start")

        '再表示
        Me.RedirectNextScreen(APPLICATION_ID)

        Logger.Info("FooterButtonDummy300_Click End")
    End Sub

    '2013/12/10 TMEJ 河原 次世代サービス 工程管理機能開発 END


#End Region

#Region "詳細ポップアップ関連"

    ''' <summary>
    ''' チップ詳細サブボタン(顧客詳細)を押した時の処理
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    ''' <History>
    ''' 2013/12/10 TMEJ 河原 次世代サービス 工程管理機能開発
    ''' </History>
    Protected Sub DetailCustomerButton_Click _
                            (sender As Object, _
                             e As System.EventArgs) Handles HiddenButtonDetailCustomer.Click
        Logger.Info("DetailCustomerButton_Click Start")

        '2013/12/10 TMEJ 河原 次世代サービス 工程管理機能開発 START

        'Try
        '    Dim visitseq As Integer = CType(Me.HiddenSelectedVisitSeq.Value, Integer)

        '    Dim biz As New SC3220101BusinessLogic

        '    Dim dt As SC3220101ServiceVisitManagerInfoDataTable = biz.GetVisitManager(visitseq)

        '    If dt IsNot Nothing AndAlso 0 < dt.Rows.Count Then

        '        Dim dataRow As SC3220101ServiceVisitManagerInfoRow = _
        '            CType(dt.Rows(0), SC3220101ServiceVisitManagerInfoRow)

        '        If Not DETAILS_CUSTERMAR_STATUS_TRUE.Equals _
        '            (Me.SetStringData(dataRow.Item("CUSTSEGMENT"), "")) Then
        '            '自社客でない場合、メッセージを表示する
        '            Me.ShowMessageBox(MsgID.id903)
        '            Exit Try
        '        End If

        '        Dim session_Visitseq As String = CType(dataRow.VISITSEQ, String)    '来店実績連番
        '        Dim session_CustomerName As String = dataRow.NAME                   '顧客名
        '        Dim session_VclregNo As String = dataRow.VCLREGNO                   '車両登録No
        '        Dim session_Vin As String = dataRow.VIN                             'VIN
        '        Dim session_ModelCode As String = dataRow.MODELCODE                 'モデルコード
        '        Dim session_TellNo As String = dataRow.TELNO                        '電話番号
        '        Dim session_Mobile As String = dataRow.MOBILE                       '携帯番号
        '        Dim session_Dlrcd As String = dataRow.DLRCD                         '販売店コード

        '        Dim strLog As New StringBuilder
        '        With strLog
        '            .Append("DetailCustomerButton_Click Param:")
        '            .Append("Redirect.VISITSEQ=").Append(session_Visitseq).Append(", ")
        '            .Append("Redirect.NAME=").Append(session_CustomerName).Append(", ")
        '            .Append("Redirect.VCLREGNO=").Append(session_VclregNo).Append(", ")
        '            .Append("Redirect.VIN=").Append(session_Vin).Append(", ")
        '            .Append("Redirect.MODELCODE=").Append(session_ModelCode).Append(", ")
        '            .Append("Redirect.TELNO=").Append(session_TellNo).Append(", ")
        '            .Append("Redirect.MOBILE=").Append(session_Mobile).Append(", ")
        '            .Append("Redirect.DLRCD=").Append(session_Dlrcd).Append(", ")
        '            .Append("Redirect.FLAG=").Append("1")
        '        End With
        '        Logger.Info(strLog.ToString)

        '        MyBase.SetValue(ScreenPos.Next, "Redirect.REGISTERNO", session_VclregNo)
        '        MyBase.SetValue(ScreenPos.Next, "Redirect.VINNO", session_Vin)
        '        MyBase.SetValue(ScreenPos.Next, "Redirect.NAME", session_CustomerName)
        '        MyBase.SetValue(ScreenPos.Next, "Redirect.MODELCODE", session_ModelCode)
        '        MyBase.SetValue(ScreenPos.Next, "Redirect.TEL1", session_TellNo)
        '        MyBase.SetValue(ScreenPos.Next, "Redirect.TEL2", session_Mobile)
        '        MyBase.SetValue(ScreenPos.Next, "Redirect.VISITSEQ", session_Visitseq)
        '        MyBase.SetValue(ScreenPos.Next, "Redirect.CRDEALERCODE", session_Dlrcd)
        '        MyBase.SetValue(ScreenPos.Next, "Redirect.FLAG", "1")

        '        '顧客詳細画面に遷移する
        '        Me.RedirectNextScreen(CUSTOMER_DETAILS_PAGE)
        '    Else
        '        '顧客情報を取得できない場合、メッセージを表示する
        '        Me.ShowMessageBox(MsgID.id903)
        '        Exit Try
        '    End If

        'Catch ex As OracleExceptionEx When ex.Number = 1013
        '    'ORACLEのタイムアウトのみ処理
        '    Me.ShowMessageBox(MsgID.id901)
        'End Try


        '来店実績連番
        Dim visitseq As Long = CType(Me.HiddenSelectedVisitSeq.Value, Long)

        '画面遷移処理
        Me.SetNextScreen(visitseq, DETAILS_SUB_BUTTON_CUSTOMER)

        '2013/12/10 TMEJ 河原 次世代サービス 工程管理機能開発 END

        Logger.Info("DetailCustomerButton_Click End")
    End Sub


    ''' <summary>
    ''' チップ詳細サブボタン(R/O参照)を押した時の処理
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    ''' <History>
    ''' 2013/12/10 TMEJ 河原 次世代サービス 工程管理機能開発
    ''' </History>
    Protected Sub DetailOrderButton_Click(sender As Object, _
                                          e As System.EventArgs) Handles HiddenButtonDetailRo.Click
        Logger.Info("DetailOrderButton_Click Start")

        '2013/12/10 TMEJ 河原 次世代サービス 工程管理機能開発 STRAT

        'Dim visitseq As Integer = CType(Me.HiddenSelectedVisitSeq.Value, Integer)

        'Dim biz As New SC3220101BusinessLogic

        'Try
        '    Dim dt As SC3220101ServiceVisitManagerInfoDataTable = biz.GetVisitManager(visitseq)

        '    If dt IsNot Nothing AndAlso dt.Rows.Count > 0 Then
        '        Dim dataRow As SC3220101ServiceVisitManagerInfoRow = _
        '            CType(dt.Rows(0), SC3220101ServiceVisitManagerInfoRow)

        '        If String.IsNullOrEmpty(dataRow.ORDERNO) Then
        '            '整備受注Noが登録されていない
        '            Me.ShowMessageBox(MsgID.id904)
        '            Exit Try
        '        End If

        '        ' 整備受注No
        '        Logger.Info("DetailOrderButton_Click Param:Redirect.ORDERNO=" + dataRow.ORDERNO)

        '        MyBase.SetValue(ScreenPos.Next, "OrderNo", dataRow.ORDERNO)

        '        'R/O参照画面に遷移する
        '        Me.RedirectNextScreen(REPAIR_ORDERE_PREVIEW_PAGE)

        '    Else
        '        '顧客情報を取得できない場合、メッセージを表示する
        '        Me.ShowMessageBox(MsgID.id904)
        '        Exit Try
        '    End If

        'Catch ex As OracleExceptionEx When ex.Number = 1013
        '    'ORACLEのタイムアウトのみ処理
        '    Me.ShowMessageBox(MsgID.id901)
        'End Try

        '来店実績連番
        Dim visitseq As Long = CType(Me.HiddenSelectedVisitSeq.Value, Long)

        '画面遷移処理
        Me.SetNextScreen(visitseq, DETAILS_SUB_BUTTON_RO)

        '2013/12/10 TMEJ 河原 次世代サービス 工程管理機能開発 END

        Logger.Info("DetailOrderButton_Click End")
    End Sub


    ''' <summary>
    ''' アイコンを押した時の処理
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    ''' <history>
    ''' 2013/06/03 TMEJ 小澤 【A.STEP2】次世代e-CRBサービスタブレット 新DB適応に向けた機能開発
    ''' 2013/12/10 TMEJ 河原 次世代サービス 工程管理機能開発
    ''' </history>
    Protected Sub DetailPopupButton_Click(sender As Object, _
                                          e As System.EventArgs) Handles HiddenButtonDetailPopup.Click
        Logger.Info("DetailPopupButton_Click Start")

        Dim visitseq As String = Me.HiddenSelectedVisitSeq.Value
        Dim chipDetail As ChipDetail = Nothing

        'IF用ログ
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                                  , "CALL IF:SMBCommonClass.GetChipDetailsData" +
                                    " IN:DlrCD={0}, BrnCD={1}, visitseq={2}" _
                                  , objStaffContext.DlrCD _
                                  , objStaffContext.BrnCD _
                                  , visitseq))

        Using SMBCommonBiz As New SMBCommonClassBusinessLogic
            Try
                '来店チップ詳細情報の取得
                chipDetail = SMBCommonBiz.GetChipDetailVisit(objStaffContext.DlrCD, _
                                                             objStaffContext.BrnCD, _
                                                             CType(visitseq, Long))
            Catch ex As OracleExceptionEx When ex.Number = 1013
                'ORACLEのタイムアウトのみ処理
                Me.ShowMessageBox(MsgID.id901)
                Logger.Info("DetailPopupButton_Click END")
                Exit Sub
            End Try
        End Using

        If chipDetail Is Nothing Then
            Me.ShowMessageBox(MsgID.id902)
            Logger.Info("DetailPopupButton_Click END")
            Exit Sub
        End If

        Dim strNullDateTime As String = WebWordUtility.GetWord(APPLICATION_ID, 18) '「--:--」

        '2012/09/19 TMEJ 日比野 【SERVICE_2】 受付待ち工程の追加対応 START
        If Me.HiddenSelectedDisplayArea.Value.Equals(CType(CHIP_PROGRESSSTATE_RECEPTION_WAIT, String)) Then
            '来店(受付待ち)エリアの場合
            'ステータス
            Me.AiconStatsLabel.Text = WebWordUtility.GetWord(APPLICATION_ID, 37)
        Else
            '2012/09/19 TMEJ 日比野 【SERVICE_2】 受付待ち工程の追加対応 END
            'ステータス
            Me.AiconStatsLabel.Text = chipDetail.Status
        End If

        '納車予定時刻
        Me.DeliveryTimeLabel.Text = Me.SetNullToString(chipDetail.DeliveryPlanDate, strNullDateTime)

        '2013/12/10 TMEJ 河原 次世代サービス 工程管理機能開発 START

        ''納車予定時刻変更回数
        'Me.ChangeCountLabel.Text = _
        '    WebWordUtility.GetWord(APPLICATION_ID, 15) _
        '                      .Replace("%1", CType(chipDetail.DeliveryPlanDateUpdateCount, String))

        '2013/12/10 TMEJ 河原 次世代サービス 工程管理機能開発 END

        Me.HiddenDeliveryPlanUpdateCount.Value = CType(chipDetail.DeliveryPlanDateUpdateCount, String)

        '2013/12/10 TMEJ 河原 次世代サービス 工程管理機能開発 START

        '納車予定変更回数のチェック
        If 0 < chipDetail.DeliveryPlanDateUpdateCount Then
            '変更回数が1件以上の場合

            'スラッシュの表示
            Me.FixSlashLabel.Visible = True

            '納車予定時刻変更回数を設定
            Me.ChangeCountLabel.Text = WebWordUtility.GetWord(APPLICATION_ID, 15) _
                                      .Replace("%1", CType(chipDetail.DeliveryPlanDateUpdateCount, String))

        Else
            ' 変更回数が0件の場合

            'スラッシュの非表示
            Me.FixSlashLabel.Visible = False

        End If

        '2013/12/10 TMEJ 河原 次世代サービス 工程管理機能開発 END

        '納車見込時刻
        Me.DeliveryEstimateLabel.Text = _
            Me.SetNullToString(chipDetail.DeliveryHopeDate, strNullDateTime)

        '車両登録No.
        Me.VclregNoLabel.Text = chipDetail.VehicleRegNo

        '2013/12/10 TMEJ 河原 次世代サービス 工程管理機能開発 START

        'プロビンスチェック
        If chipDetail.RegisterAreaName Is Nothing Then
            'プロビンス情報無し

            'プロビンス領域非表示
            Me.DetailsProvince.Visible = False
        Else
            'データがある場合

            'プロビンス領域表示
            Me.DetailsProvince.Visible = True

            'プロビンスを設定
            Me.DetailsProvince.Text = chipDetail.RegisterAreaName

        End If

        '2013/12/10 TMEJ 河原 次世代サービス 工程管理機能開発 END

        '予約マーク
        If DETAILS_MARK_ACTIVE.Equals(chipDetail.WalkIn) Then
            Me.DetailsRightIconD.Visible = True
        Else
            Me.DetailsRightIconD.Visible = False
        End If

        'JDP調査対象客マーク
        '2018/06/22 NSK 可児 TKM Next Gen e-CRB Project Application development Block B-1  Pマーク、Lマークなどを表示 START
        'If DETAILS_MARK_ACTIVE.Equals(chipDetail.JdpType) Then
        'Me.DetailsRightIconI.Visible = True
        'Else
        'Me.DetailsRightIconI.Visible = False
        'End If
        'Pマーク/Lマーク
        If DETAILS_PMARK_ACTIVE.Equals(chipDetail.JdpType) Then
            Me.DetailsRightIconP.Visible = True
            Me.DetailsRightIconL.Visible = False
        ElseIf DETAILS_LMARK_ACTIVE.Equals(chipDetail.JdpType) Then
            Me.DetailsRightIconL.Visible = True
            Me.DetailsRightIconP.Visible = False
        Else
            Me.DetailsRightIconP.Visible = False
            Me.DetailsRightIconL.Visible = False
        End If
        '2018/06/22 NSK 可児 TKM Next Gen e-CRB Project Application development Block B-1  Pマーク、Lマークなどを表示 END

        'SSCマーク
        If DETAILS_MARK_ACTIVE.Equals(chipDetail.SscType) Then
            Me.DetailsRightIconS.Visible = True
        Else
            Me.DetailsRightIconS.Visible = False
        End If

        '車種名
        Me.CarModelLabel.Text = chipDetail.VehicleName
        'グレード
        Me.CarGradeLabel.Text = chipDetail.Grade
        '顧客名
        Me.CustomerNameLabel.Text = chipDetail.CustomerName
        '電話番号
        Me.TelNoLable.Text = chipDetail.TelNo
        '携帯電話番号
        Me.PortableTelNoLable.Text = chipDetail.Mobile
        '整備内容
        Me.ServiceContentsLable.Text = chipDetail.MerchandiseName
        '待ち方
        If REZ_RECEPTION_WAITING.Equals(chipDetail.ReserveReception) Then
            Me.WaitPlanLabel.Text = WebWordUtility.GetWord(APPLICATION_ID, 29) '店内
        ElseIf REZ_RECEPTION_DROP_OFF.Equals(chipDetail.ReserveReception) Then
            Me.WaitPlanLabel.Text = WebWordUtility.GetWord(APPLICATION_ID, 30) '店外
        Else
            Me.WaitPlanLabel.Text = ""
        End If
        '2012/09/21 TMEJ 小澤 【SERVICE_2】SAストール予約受付機能開発（仕分けNo.35） START
        '追加作業エリアのチップ詳細の場合は起票者ストールを表示する
        If SMBCommonClassBusinessLogic.DisplayType.AddApprove = CType(Me.HiddenSelectedDisplayArea.Value, Integer) AndAlso _
           "1".Equals(chipDetail.ReissueVouchers) Then
            Me.DrawerTable.Style("display") = String.Empty
            Me.DrawerLabel.Text = chipDetail.AddAccountName
        Else
            Me.DrawerTable.Style("display") = "none"
            Me.DrawerLabel.Text = String.Empty
        End If
        '2012/09/21 TMEJ 小澤 【SERVICE_2】SAストール予約受付機能開発（仕分けNo.35） END

        '中断理由
        If chipDetail.StopReasonList IsNot Nothing AndAlso 0 < chipDetail.StopReasonList.Count Then
            Using dtInterruptionInfo As New SC3220101InterruptionInfoDataTable
                For Each item As StopReason In chipDetail.StopReasonList

                    Dim rowInterruptionInfo As SC3220101InterruptionInfoRow = _
                        CType(dtInterruptionInfo.NewRow(), SC3220101InterruptionInfoRow)

                    ' 中断理由
                    '2013/06/03 TMEJ 小澤 【A.STEP2】次世代e-CRBサービスタブレット 新DB適応に向けた機能開発 START
                    'rowInterruptionInfo.InterruptionCause = Me.GetResultStatusWord(item.ResultStatus)
                    rowInterruptionInfo.InterruptionCause = item.ResultStatus
                    '2013/06/03 TMEJ 小澤 【A.STEP2】次世代e-CRBサービスタブレット 新DB適応に向けた機能開発 END
                    ' 中断注釈
                    rowInterruptionInfo.InterruptionDetails = item.StopMemo

                    dtInterruptionInfo.AddSC3220101InterruptionInfoRow(rowInterruptionInfo)
                Next

                Me.InterruptionCauseRepeater.DataSource = dtInterruptionInfo
                Me.InterruptionCauseRepeater.DataBind()
            End Using
        Else
            Me.InterruptionCauseRepeater.DataSource = Nothing
            Me.InterruptionCauseRepeater.DataBind()
        End If

        '納車時刻変更履歴
        If chipDetail.DeliveryChgList IsNot Nothing AndAlso 0 < chipDetail.DeliveryChgList.Count Then
            Using dtDeliChange As New SC3220101DeliveryTimeChangeLogInfoDataTable
                For Each item As DeliveryChg In chipDetail.DeliveryChgList

                    Dim rowDeliChange As SC3220101DeliveryTimeChangeLogInfoRow = _
                        CType(dtDeliChange.NewRow(), SC3220101DeliveryTimeChangeLogInfoRow)

                    '変更前納車予定時刻
                    rowDeliChange.ChangeFromTime = _
                        Me.SetDateTimeToString(item.OldDeliveryHopeDate)
                    '変更後納車予定時刻
                    rowDeliChange.ChangeToTime = _
                        Me.SetDateTimeToString(item.NewDeliveryHopeDate)
                    '変更日時
                    rowDeliChange.UpdateTime = Me.SetDateTimeToString(item.ChangeDate)
                    '変更理由
                    rowDeliChange.UpdatePretext = item.ChangeReason

                    dtDeliChange.AddSC3220101DeliveryTimeChangeLogInfoRow(rowDeliChange)
                Next

                Me.ChangeTimeRepeater.DataSource = dtDeliChange
                Me.ChangeTimeRepeater.DataBind()
            End Using
        Else
            Me.ChangeTimeRepeater.DataSource = Nothing
            Me.ChangeTimeRepeater.DataBind()
        End If

        'サブボタンの非活性の設定
        '顧客ボタン
        If DETAILS_CUSTERMAR_STATUS_TRUE.Equals(chipDetail.CustomerType) Then
            Me.HiddenDetailsCustomerButtonStatus.Value = "1"
        Else
            '未取引客の場合は非活性
            Me.HiddenDetailsCustomerButtonStatus.Value = "0"
        End If

        'R/Oボタン
        If DETAILS_RO_FLG_TRUE.Equals(chipDetail.OrderDataType) Then
            Me.HiddenDetailsROButtonStatus.Value = "1"
        Else
            'R/O有無がなしの場合は非活性
            Me.HiddenDetailsROButtonStatus.Value = "0"
        End If

        Me.ContentUpdatePanelDetail.Update()
        'タイマークリア 更新：2012/11/13 TMEJ 丁 【A.STEP2】次世代サービス アクティブインジゲータ対応 No.1 START
        ScriptManager.RegisterStartupScript(Me, Me.GetType, "ClearTimer", "commonClearTimer();", True)
        '更新：2012/11/13 TMEJ 丁 【A.STEP2】次世代サービス アクティブインジゲータ対応 No.1 END 
        Logger.Info("DetailPopupButton_Click End")
    End Sub

    '2013/06/03 TMEJ 小澤 【A.STEP2】次世代e-CRBサービスタブレット 新DB適応に向けた機能開発 START
    ' ''' <summary>
    ' ''' 実績ステータスに該当する文言を取得する
    ' ''' </summary>
    ' ''' <param name="ResultStatus">実績ステータス</param>
    ' ''' <returns></returns>
    ' ''' <remarks></remarks>
    'Private Function GetResultStatusWord(ByVal ResultStatus As String) As String

    '    Select Case (ResultStatus)
    '        Case "30"
    '            Return WebWordUtility.GetWord(APPLICATION_ID, 31)
    '        Case "31"
    '            Return WebWordUtility.GetWord(APPLICATION_ID, 32)
    '        Case "38"
    '            Return WebWordUtility.GetWord(APPLICATION_ID, 33)
    '        Case "39"
    '            Return WebWordUtility.GetWord(APPLICATION_ID, 34)
    '        Case Else
    '            Return ""
    '    End Select

    'End Function
    '2013/06/03 TMEJ 小澤 【A.STEP2】次世代e-CRBサービスタブレット 新DB適応に向けた機能開発 END

#End Region

#Region "その他"

    ''' <summary>
    ''' 文字列変換
    ''' </summary>
    ''' <param name="str"></param>
    ''' <returns>変換値</returns>
    Private Function SetNullToString(ByVal str As String, Optional ByVal strNull As String = "") As String

        ' 空白チェック
        If String.IsNullOrEmpty(str) Then
            Return strNull
        End If

        Return str

    End Function

    ' ''' <summary>
    ' ''' DBNullのデータをデフォルト値で返す
    ' ''' </summary>
    ' ''' <param name="src"></param>
    ' ''' <param name="defult">デフォルト値</param>
    ' ''' <returns></returns>
    ' ''' <remarks></remarks>
    'Private Function SetStringData(ByVal src As Object, ByVal defult As String) As String

    '    Dim returnValue As String

    '    If IsDBNull(src) = True Then
    '        returnValue = defult
    '    Else
    '        returnValue = DirectCast(src, String)
    '    End If

    '    Return returnValue

    'End Function

    ''' <summary>
    ''' 時間変換 (hh:mm)
    ''' </summary>
    ''' <param name="time">対象時間</param>
    ''' <returns>変換値</returns>
    Private Function SetDateTimeToString(ByVal time As DateTime) As String

        Dim strResult As String

        ' 日付チェック
        If time.Equals(DateTime.MinValue) Then
            Return String.Empty
        End If

        Try
            If Not DateTimeFunc.Now(objStaffContext.DlrCD).Date = time.Date Then

                '2013/12/10 TMEJ 河原 次世代サービス 工程管理機能開発 START

                '(MM/dd hh:mm)
                'strResult = time.ToString("MM/dd HH:mm", CultureInfo.CurrentCulture)

                strResult = String.Concat(DateTimeFunc.FormatDate(11, time), Space(1), DateTimeFunc.FormatDate(14, time))

                '2013/12/10 TMEJ 河原 次世代サービス 工程管理機能開発 END

            Else
                ' (hh:mm)
                strResult = DateTimeFunc.FormatDate(14, time)
            End If

        Catch ex As FormatException
            strResult = String.Empty
        End Try

        Return strResult

    End Function

#End Region

#Region "画面遷移メソッド"

    ''' <summary>
    ''' 画面遷移処理
    ''' </summary>
    ''' <param name="inVisitSeq">来店実績連番</param>
    ''' <param name="inButtonID">ボタンイベントID</param>
    ''' <remarks></remarks>
    ''' <History>
    ''' </History>
    Private Sub SetNextScreen(ByVal inVisitSeq As Long, _
                              ByVal inButtonID As Long)

        '開始ログ
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} START VISITSEQ = {2} BUTTONID = {3}" _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name _
                    , inVisitSeq, inButtonID))

        Try

            'SC3220101BusinessLogicインスタンス
            Using biz As New SC3220101BusinessLogic

                'サービス来店管理情報の取得
                Dim dtVisitInfo As SC3220101ServiceVisitManagerInfoDataTable = biz.GetVisitManager(inVisitSeq)

                'サービス来店管理情報取得チェック
                If dtVisitInfo Is Nothing OrElse dtVisitInfo.Count < 0 Then
                    'サービス来店管理情報が存在しない場合

                    'エラーログ
                    Logger.Info(String.Format(CultureInfo.CurrentCulture _
                               , "{0}.{1} ERR:GetVisitManager = NOTHING  VISITSEQ = {2}" _
                               , Me.GetType.ToString _
                               , System.Reflection.MethodBase.GetCurrentMethod.Name _
                               , inVisitSeq))

                    'エラーメッセージ
                    Me.ShowMessageBox(MsgID.id905)

                    '処理中断
                    Exit Sub

                Else
                    'サービス来店管理情報取得成功

                    'ROWに変換
                    Dim rowVisitInfo As SC3220101ServiceVisitManagerInfoRow = _
                        DirectCast(dtVisitInfo.Rows(0), SC3220101ServiceVisitManagerInfoRow)

                    'イベントIDで処理分岐
                    Select Case inButtonID

                        Case DETAILS_SUB_BUTTON_CUSTOMER
                            '顧客詳細ボタン

                            '顧客詳細画面遷移処理(パラメータチェック)
                            Me.ChipDetailCustomerButton(rowVisitInfo)

                        Case DETAILS_SUB_BUTTON_RO
                            'RO参照ボタン

                            ' R/O参照画面遷移処理(パラメータチェック)
                            Me.ChipDetailOrderDispButton(rowVisitInfo)

                        Case Else

                            'エラーメッセージ
                            Me.ShowMessageBox(MsgID.id905)

                    End Select

                End If

            End Using

        Catch ex As OracleExceptionEx When ex.Number = 1013
            'ORACLEのタイムアウトのみ処理

            'エラーメッセージ
            Me.ShowMessageBox(MsgID.id901)

        End Try

        '終了ログ
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                   , "{0}.{1} END" _
                   , Me.GetType.ToString _
                   , System.Reflection.MethodBase.GetCurrentMethod.Name))

    End Sub


#Region "R/O参照画面"

    ''' <summary>
    ''' R/O参照画面遷移処理(パラメータチェック)
    ''' </summary>
    ''' <param name="inRowVisitInfo">画面遷移用来店情報</param>
    ''' <remarks></remarks>
    Private Sub ChipDetailOrderDispButton(ByVal inRowVisitInfo As SC3220101ServiceVisitManagerInfoRow)

        '開始ログ
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} START" _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name))

        '必須項目チェック

        'DMS販売店コードのチェック
        If inRowVisitInfo.IsDMSDLRCDNull _
            OrElse String.IsNullOrEmpty(inRowVisitInfo.DMSDLRCD.Trim) Then
            'DMS販売店コードが存在しない場合

            'エラーログ
            Logger.Info(String.Format(CultureInfo.CurrentCulture _
                       , "{0}.{1} ERR:DMSDLRCD = NOTHING  VISITSEQ = {2}" _
                       , Me.GetType.ToString _
                       , System.Reflection.MethodBase.GetCurrentMethod.Name _
                       , inRowVisitInfo.VISITSEQ))

            'タイマークリア
            ScriptManager.RegisterStartupScript(Me, Me.GetType, "ClearTimer", "ErrorRefreshScript();", True)

            'エラーメッセージ
            Me.ShowMessageBox(MsgID.id905)

            '処理中断
            Exit Sub

        End If

        'DMS店舗コードのチェック
        If inRowVisitInfo.IsDMSSTRCDNull _
            OrElse String.IsNullOrEmpty(inRowVisitInfo.DMSSTRCD.Trim) Then
            'DMS店舗コードが存在しない場合

            'エラーログ
            Logger.Info(String.Format(CultureInfo.CurrentCulture _
                       , "{0}.{1} ERR:DMSSTRCD = NOTHING  VISITSEQ = {2}" _
                       , Me.GetType.ToString _
                       , System.Reflection.MethodBase.GetCurrentMethod.Name _
                       , inRowVisitInfo.VISITSEQ))

            'タイマークリア
            ScriptManager.RegisterStartupScript(Me, Me.GetType, "ClearTimer", "ErrorRefreshScript();", True)

            'エラーメッセージ
            Me.ShowMessageBox(MsgID.id905)

            '処理中断
            Exit Sub

        End If

        'DMSアカウントのチェック
        If inRowVisitInfo.IsDMSACCOUNTNull _
            OrElse String.IsNullOrEmpty(inRowVisitInfo.DMSACCOUNT.Trim) Then
            'DMSアカウントが存在しない場合

            'エラーログ
            Logger.Info(String.Format(CultureInfo.CurrentCulture _
                       , "{0}.{1} ERR:DMSACCOUNT = NOTHING  VISITSEQ = {2}" _
                       , Me.GetType.ToString _
                       , System.Reflection.MethodBase.GetCurrentMethod.Name _
                       , inRowVisitInfo.VISITSEQ))

            'タイマークリア
            ScriptManager.RegisterStartupScript(Me, Me.GetType, "ClearTimer", "ErrorRefreshScript();", True)

            'エラーメッセージ
            Me.ShowMessageBox(MsgID.id905)

            '処理中断
            Exit Sub

        End If

        'VINのチェック
        If inRowVisitInfo.IsVINNull _
            OrElse String.IsNullOrEmpty(inRowVisitInfo.VIN.Trim) Then
            'VINが存在しない場合

            'エラーログ
            Logger.Info(String.Format(CultureInfo.CurrentCulture _
                       , "{0}.{1} ERR:VIN = NOTHING  VISITSEQ = {2}" _
                       , Me.GetType.ToString _
                       , System.Reflection.MethodBase.GetCurrentMethod.Name _
                       , inRowVisitInfo.VISITSEQ))

            'タイマークリア
            ScriptManager.RegisterStartupScript(Me, Me.GetType, "ClearTimer", "ErrorRefreshScript();", True)

            'エラーメッセージ
            Me.ShowMessageBox(MsgID.id903)

            '処理中断
            Exit Sub

        End If

        'DMSIDのチェック
        If inRowVisitInfo.IsDMSIDNull _
            OrElse String.IsNullOrEmpty(inRowVisitInfo.DMSID.Trim) Then
            'DMSIDが存在しない場合

            'エラーログ
            Logger.Info(String.Format(CultureInfo.CurrentCulture _
                       , "{0}.{1} ERR:DMSID = NOTHING  VISITSEQ = {2}" _
                       , Me.GetType.ToString _
                       , System.Reflection.MethodBase.GetCurrentMethod.Name _
                       , inRowVisitInfo.VISITSEQ))

            'タイマークリア
            ScriptManager.RegisterStartupScript(Me, Me.GetType, "ClearTimer", "ErrorRefreshScript();", True)

            'エラーメッセージ
            Me.ShowMessageBox(MsgID.id903)

            '処理中断
            Exit Sub

        End If

        'RO_NUMのチェック
        If inRowVisitInfo.IsORDERNONull _
            OrElse String.IsNullOrEmpty(inRowVisitInfo.ORDERNO.Trim) Then
            'RO_NUMが存在しない場合

            'エラーログ
            Logger.Info(String.Format(CultureInfo.CurrentCulture _
                       , "{0}.{1} ERR:RO_NUM = NOTHING  VISITSEQ = {2}" _
                       , Me.GetType.ToString _
                       , System.Reflection.MethodBase.GetCurrentMethod.Name _
                       , inRowVisitInfo.VISITSEQ))

            'タイマークリア
            ScriptManager.RegisterStartupScript(Me, Me.GetType, "ClearTimer", "ErrorRefreshScript();", True)

            'エラーメッセージ
            Me.ShowMessageBox(MsgID.id905)

            '処理中断
            Exit Sub

        End If

        'SMBCommonClassBusinessLogicのインスタンス
        Using smbCommon As New SMBCommonClassBusinessLogic

            '基幹顧客コード変換処理
            inRowVisitInfo.DMSID = smbCommon.ReplaceBaseCustomerCode(inRowVisitInfo.DLRCD, inRowVisitInfo.DMSID)

        End Using


        'R/O参照画面遷移処理
        Me.RedirectOrderDisp(inRowVisitInfo)


        '終了ログ
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                   , "{0}.{1} END" _
                   , Me.GetType.ToString _
                   , System.Reflection.MethodBase.GetCurrentMethod.Name))

    End Sub

    ''' <summary>
    ''' R/O参照画面遷移処理(パラメータ設定)
    ''' </summary>
    ''' <param name="inRowVisitInfo">画面遷移用来店情報</param>
    ''' <remarks></remarks>
    Private Sub RedirectOrderDisp(ByVal inRowVisitInfo As SC3220101ServiceVisitManagerInfoRow)

        '開始ログ
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                  , "{0}.{1} START" _
                  , Me.GetType.ToString _
                  , System.Reflection.MethodBase.GetCurrentMethod.Name))

        '次画面遷移パラメータ設定

        '販売店コード
        Me.SetValue(ScreenPos.Next, SessionParam01, inRowVisitInfo.DMSDLRCD)
        '店舗コード
        Me.SetValue(ScreenPos.Next, SessionParam02, inRowVisitInfo.DMSSTRCD)
        'アカウント
        Me.SetValue(ScreenPos.Next, SessionParam03, inRowVisitInfo.DMSACCOUNT)
        '来店者実績連番
        Me.SetValue(ScreenPos.Next, SessionParam04, inRowVisitInfo.VISITSEQ)
        'DMS予約ID
        Me.SetValue(ScreenPos.Next, SessionParam05, String.Empty)
        'RO
        Me.SetValue(ScreenPos.Next, SessionParam06, inRowVisitInfo.ORDERNO)
        'RO_JOB_SEQ(親のRO_JOB_SEQ = 0)            
        Me.SetValue(ScreenPos.Next, SessionParam07, ParentJobSeq)
        'VIN
        Me.SetValue(ScreenPos.Next, SessionParam08, inRowVisitInfo.VIN)
        'ViewMode
        Me.SetValue(ScreenPos.Next, SessionParam09, EditMode)
        'Format
        Me.SetValue(ScreenPos.Next, SessionParam10, PreviewFlag)
        'SVCIN_NUM
        Me.SetValue(ScreenPos.Next, SessionParam11, String.Empty)
        'SVCIN_DealerCode
        Me.SetValue(ScreenPos.Next, SessionParam12, String.Empty)
        'DISP_NUM
        Me.SetValue(ScreenPos.Next, SessionDispNum, APPLICATIONID_ORDEROUT)


        '基幹画面連携用フレーム呼出処理
        Me.ScreenTransition()

        '終了ログ
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                   , "{0}.{1} END" _
                   , Me.GetType.ToString _
                   , System.Reflection.MethodBase.GetCurrentMethod.Name))
    End Sub

#End Region

#Region "顧客詳細画面"

    ''' <summary>
    ''' 顧客詳細画面遷移処理(パラメータチェック)
    ''' </summary>
    ''' <param name="inRowVisitInfo">画面遷移用来店情報</param>
    ''' <remarks></remarks>
    ''' <history>
    ''' </history>
    Private Sub ChipDetailCustomerButton(ByVal inRowVisitInfo As SC3220101ServiceVisitManagerInfoRow)

        '開始ログ
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                  , "{0}.{1} START" _
                  , Me.GetType.ToString _
                  , System.Reflection.MethodBase.GetCurrentMethod.Name))

        '必須項目チェック

        '2015/09/01 TMEJ 井上 (トライ店システム評価)サービス入庫予約のユーザ管理機能開発 START

        'DMSIDのチェック
        'If inRowVisitInfo.IsDMSIDNull _
        '   OrElse String.IsNullOrEmpty(inRowVisitInfo.DMSID.Trim) Then
        'DMSIDが存在しない場合

        'エラーログ
        'Logger.Error(String.Format(CultureInfo.CurrentCulture _
        '          , "{0}.{1} ERR:DMSID = NOTHING  VISITSEQ = {2}" _
        '           , Me.GetType.ToString _
        '           , System.Reflection.MethodBase.GetCurrentMethod.Name _
        '           , inRowVisitInfo.VISITSEQ))

        If inRowVisitInfo.IsDMSID_CSTDTLUSENull _
           OrElse String.IsNullOrEmpty(inRowVisitInfo.DMSID_CSTDTLUSE.Trim) Then
            'DMSID_CSTDTLUSE(基幹顧客ID(顧客詳細用))が存在しない場合

            'エラーログ
            Logger.Info(String.Format(CultureInfo.CurrentCulture _
                       , "{0}.{1} ERR:DMSID_CSTDTLUSE = NOTHING  VISITSEQ = {2}" _
                       , Me.GetType.ToString _
                       , System.Reflection.MethodBase.GetCurrentMethod.Name _
                       , inRowVisitInfo.VISITSEQ))

            '2015/09/01 TMEJ 井上 (トライ店システム評価)サービス入庫予約のユーザ管理機能開発 END

            'タイマークリア
            ScriptManager.RegisterStartupScript(Me, Me.GetType, "ClearTimer", "ErrorRefreshScript();", True)

            'エラーメッセージ
            Me.ShowMessageBox(MsgID.id903)

            '処理中断
            Exit Sub

        End If


        '顧客詳細画面遷移処理(パラメータ設定)
        Me.RedirectCustomer(inRowVisitInfo)


        '終了ログ
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                   , "{0}.{1} END" _
                   , Me.GetType.ToString _
                   , System.Reflection.MethodBase.GetCurrentMethod.Name))

    End Sub

    ''' <summary>
    ''' 顧客詳細画面遷移処理(パラメータ設定)
    ''' </summary>
    ''' <param name="inRowVisitInfo">画面遷移用来店情報</param>
    ''' <remarks></remarks>
    ''' <history>
    ''' </history>
    Private Sub RedirectCustomer(ByVal inRowVisitInfo As SC3220101ServiceVisitManagerInfoRow)

        '開始ログ
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                  , "{0}.{1} START" _
                  , Me.GetType.ToString _
                  , System.Reflection.MethodBase.GetCurrentMethod.Name))

        '次画面遷移パラメータ設定

        '2015/09/01 TMEJ 井上 (トライ店システム評価)サービス入庫予約のユーザ管理機能開発 START

        'DMSID
        'Me.SetValue(ScreenPos.Next, SessionDMSID, inRowVisitInfo.DMSID)

        '基幹顧客ID(顧客詳細用)
        Me.SetValue(ScreenPos.Next, SessionDMSID, inRowVisitInfo.DMSID_CSTDTLUSE)

        '2015/09/01 TMEJ 井上 (トライ店システム評価)サービス入庫予約のユーザ管理機能開発 END

        'VINチェック
        If Not inRowVisitInfo.IsVINNull _
            OrElse Not String.IsNullOrEmpty(inRowVisitInfo.VIN.Trim) Then

            'VIN
            Me.SetValue(ScreenPos.Next, SessionVIN, inRowVisitInfo.VIN)

        End If


        '顧客詳細画面遷移
        Me.RedirectNextScreen(APPLICATIONID_CUSTOMEROUT)


        '終了ログ
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                   , "{0}.{1} END" _
                   , Me.GetType.ToString _
                   , System.Reflection.MethodBase.GetCurrentMethod.Name))

    End Sub

#End Region

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
        Using biz As New SC3220101BusinessLogic

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
                Me.ShowMessageBox(MsgID.id905)

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
                Me.ShowMessageBox(MsgID.id905)

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
                Me.ShowMessageBox(MsgID.id905)

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
        Using biz As New SC3220101BusinessLogic

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
                Me.ShowMessageBox(MsgID.id905)

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
                Me.ShowMessageBox(MsgID.id905)

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
                Me.ShowMessageBox(MsgID.id905)

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

End Class
