'━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
'SC3130501.aspx.vb
'─────────────────────────────────────
'機能： 受付待ち画面(PC端末アプリ)
'補足： 
'作成：            SKFC 久代 【A. STEP1】
'更新： 2013/03/27 SKFC 久代 【A. STEP1】来店受付管理オペレーション確立に向けた評価アプリ作成
'─────────────────────────────────────
Imports Toyota.eCRB.SystemFrameworks.Web
Imports Toyota.eCRB.SystemFrameworks.Core
Imports Toyota.eCRB.SystemFrameworks.Core.iCROP.BizLogic
Imports Toyota.eCRB.SystemFrameworks.Core.iCROP.DataAccess
Imports Toyota.eCRB.SystemFrameworks.Web.Controls
Imports Toyota.eCRB.Visit.Api.BizLogic
Imports Toyota.eCRB.CommonUtility.BizLogic
Imports Toyota.eCRB.CommonUtility.DataAccess
Imports System.Text
Imports System.Data
Imports System.Globalization
Imports System.Web.Services
Imports System.Web.Script.Serialization
Imports Toyota.eCRB.iCROP.BizLogic.SC3130501
Imports Toyota.eCRB.iCROP.DataAccess.SC3130501


Partial Class SC3130501
    Inherits System.Web.UI.Page

#Region "非公開定数"

    ''' <summary>
    ''' 文言の数
    ''' </summary>
    ''' <remarks></remarks>
    Private Const WordDictionaryCount As Integer = 12

    ''' <summary>
    ''' SSV画面ID
    ''' </summary>
    ''' <remarks></remarks>
    Private Const ReceptionistId As String = "SC3130501"

#End Region

#Region "処理"

    ''' <summary>
    ''' ページ読み込み(イベントハンドラ)
    ''' </summary>
    ''' <remarks></remarks>
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        _initializeClient()

        '文言管理
        Dim wordDictionary As New Dictionary(Of Decimal, String)

        '文言取得(パフォーマンスを考慮しログ出力は行わない)
        For displayId As Decimal = 1 To WordDictionaryCount
            wordDictionary.Add(displayId, WebWordUtility.GetWord(ReceptionistId, displayId))
        Next

        ' 文言設定
        _localizeWord(wordDictionary)


    End Sub

    ''' <summary>
    ''' 呼出中データ取得
    ''' </summary>
    ''' <returns>受付データ(JSON形式)</returns>
    ''' <remarks></remarks>
    <System.Web.Services.WebMethod()> _
    Public Shared Function getCalleeList() As String
        Dim resultJson As String

        ' データ取得用のビジネスロジック作成
        Dim businessLogic As New Toyota.eCRB.iCROP.BizLogic.SC3130501.SC3130501BusinessLogic

        ' 表示データ取得
        resultJson = businessLogic.GetDisplayData()

        ' 後処理
        businessLogic = Nothing

        Return (resultJson)
    End Function

    ''' <summary>
    ''' クライアント初期化
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub _initializeClient()
        Dim initilizeScript As New StringBuilder
        Dim refreshAllInterval As Integer
        Dim dateInterval As Integer
        Dim nextPageInterval As Integer
        Dim dateFormatList As String()

        ' 値取得
        ' 定期更新間隔
        refreshAllInterval = System.Configuration.ConfigurationManager.AppSettings("SC3130501RefreshAllInterval")
        If refreshAllInterval <= 0 Then
            ' デフォルト60秒
            refreshAllInterval = 60000
        End If
        ' 日付更新間隔
        dateInterval = System.Configuration.ConfigurationManager.AppSettings("SC3130501RefreshDateInterval")
        If dateInterval <= 0 Then
            ' デフォルト30秒
            dateInterval = 30000
        End If
        ' 呼出中履歴切替間隔
        nextPageInterval = System.Configuration.ConfigurationManager.AppSettings("SC3130501RefreshNextPageInterval")
        If nextPageInterval <= 0 Then
            ' デフォルト5秒
            nextPageInterval = 5000
        End If

        ' 日付フォーマット取得
        Dim businessLogic As New Toyota.eCRB.iCROP.BizLogic.SC3130501.SC3130501BusinessLogic
        dateFormatList = businessLogic.GetDateFormat(EnvironmentSetting.CountryCode)
        businessLogic = Nothing

        ' JS生成
        initilizeScript.Append("<script type='text/javascript'>")
        ' 定期更新間隔
        initilizeScript.Append("    C_REFRESH_ALL_INTERVAL = " & refreshAllInterval & ";")
        ' 日付更新間隔
        initilizeScript.Append("    C_DATE_INTERVAL = " & dateInterval & ";")
        ' 呼出中履歴切替間隔
        initilizeScript.Append("    C_AFTER_CALLEE_NEXT_PAGE_INTERVAL = " & nextPageInterval & ";")

        ' 日付フォーマット
        For i As Integer = 0 To (dateFormatList.Count - 1)
            initilizeScript.Append("    strFormatDate[" & i & "] = """ & dateFormatList(i) & """;")
        Next

        initilizeScript.Append("</script>")

        Dim cs As ClientScriptManager = Page.ClientScript
        cs.RegisterStartupScript(Me.GetType, "init", initilizeScript.ToString)
    End Sub

    ''' <summary>
    ''' 文言管理にDB登録を行い文言番号より取得する
    ''' </summary>
    ''' <param name="wordDictionary">文言管理</param>
    ''' <remarks></remarks>
    Private Sub _localizeWord(ByVal wordDictionary As Dictionary(Of Decimal, String))

        ' 受付待ち
        StaticPageTitle.Text = Server.HtmlEncode(wordDictionary(1))
        ' お呼出番号
        StaticCalleeTitle.Text = Server.HtmlEncode(wordDictionary(2))
        ' 券番号
        StaticMainNumberFront.Text = Server.HtmlEncode(wordDictionary(3))
        ' 番のお客様
        StaticMainNumberBack.Text = Server.HtmlEncode(wordDictionary(4))
        ' までお越し下さい
        StaticMainPlaceBack.Text = Server.HtmlEncode(wordDictionary(5))
        ' が担当致します
        StaticMainSaNameBack.Text = Server.HtmlEncode(wordDictionary(6))
        ' 待ち人数
        StaticWaitNumberTitle.Text = Server.HtmlEncode(wordDictionary(7))
        ' 只今、
        StaticWaitNumberFront.Text = Server.HtmlEncode(wordDictionary(8))
        ' 人待ちとなっております
        StaticWaitNumberBack.Text = Server.HtmlEncode(wordDictionary(9))
        ' 只今の受付番号
        StaticHistoryTitle.Text = Server.HtmlEncode(wordDictionary(10))
        ' 券番号
        StaticHistoryNumber.Text = Server.HtmlEncode(wordDictionary(11))
        ' 場所
        StaticHistoryPlace.Text = Server.HtmlEncode(wordDictionary(12))
    End Sub

#End Region

End Class

