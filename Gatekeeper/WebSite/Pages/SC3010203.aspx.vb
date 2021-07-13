Imports System.Web.Script.Services
Imports System.Web.Services
Imports System.Xml
Imports System.Globalization
Imports Toyota.eCRB.SystemFrameworks.Web.CommonMasterPage
Imports Toyota.eCRB.SystemFrameworks.Core
Imports Toyota.eCRB.Common.MainMenu.BizLogic
Imports Toyota.eCRB.Common.MainMenu.DataAccess.SC3010203DataSet

''' <summary>
''' SCメイン
''' </summary>
''' <remarks></remarks>
Partial Class Pages_SC3010203
    Inherits BasePage

#Region " 定数 "
    ''' <summary>
    ''' CalDav連携エラーのメッセージ
    ''' </summary>
    ''' <remarks></remarks>
    Private Const CALDAV_ERROR_MESSAGEID As Integer = 901
    Private Const CALDAV_SELECT_ERROR_MESSAGEID As Integer = 902
    ''' <summary>
    ''' CalDav連携正常
    ''' </summary>
    ''' <remarks></remarks>
    Private Const CALDAV_NORMALEND As Integer = 0
#End Region

#Region " 初期処理 "
    ''' <summary>
    ''' ページロード時の処理です。
    ''' </summary>
    ''' <param name="sender">イベント発生元</param>
    ''' <param name="e">イベントデータ</param>
    ''' <remarks></remarks>
    Protected Sub Page_Load(sender As Object, e As System.EventArgs) Handles Me.Load

        '現在日付を設定
        NowDateLiteral.Text = DateTimeFunc.FormatDate(7, DateTimeFunc.Now(StaffContext.Current.DlrCd))

        'JavaScript用
        Dim serverNow As Date = DateTimeFunc.Now
        Yearhidden.Value = serverNow.Year
        Monthhidden.Value = serverNow.Month
        Dayhidden.Value = serverNow.Day
        HourHidden.Value = serverNow.Hour
        MinuteHidden.Value = serverNow.Minute

        If Not Me.IsPostBack Then
            'スケジュールの時間を設定
            TimeRepeater.DataSource = GetScheduleTimeList()
            TimeRepeater.DataBind()
            '３０分ごとのメモリ線
            TimeLineBorderRepeater.DataSource = Enumerable.Range(1, 48)
            TimeLineBorderRepeater.DataBind()
            'CalDav連携エラー時のメッセージ
            CaldavRegistErrorMessage.Value = WebWordUtility.GetWord(CALDAV_ERROR_MESSAGEID)
            CaldavSelectErrorMessage.Value = WebWordUtility.GetWord(CALDAV_SELECT_ERROR_MESSAGEID)
        End If

        'フッターの制御
        InitFooterEvent()

        'test
        'Me.RedirectNextScreen("SC3080201")
    End Sub
#End Region

#Region " CalDavデータ取得処理 "
    ''' <summary>
    ''' スケジュール表示の為の24H分の時間テキストを取得します。
    ''' </summary>
    ''' <returns>24H分の時：分文字列の配列</returns>
    ''' <remarks></remarks>
    Private Function GetScheduleTimeList() As List(Of String)

        Dim listTime As New List(Of String)
        Dim timeValue As Date = DateTimeFunc.Now(StaffContext.Current.DlrCd)
        Dim day As Integer = timeValue.Day

        '時間切捨て
        timeValue = New Date(timeValue.Year, timeValue.Month, timeValue.Day)

        While day = timeValue.Day
            '時間文字列格納
            listTime.Add(DateTimeFunc.FormatDate(14, timeValue))
            '1H追加
            timeValue = timeValue.AddHours(1)
        End While

        '次の日の00:00時
        listTime.Add(DateTimeFunc.FormatDate(14, timeValue))

        '処理結果返却
        Return listTime

    End Function

    ''' <summary>
    ''' スケジュール情報取得処理。
    ''' </summary>
    ''' <returns>XML</returns>
    ''' <remarks></remarks>
    <WebMethod(EnableSession:=True)> _
    Public Shared Function GetScheduleXmlText() As String

        Dim bizLogic As New SC3010203BusinessLogic

        ''スケジュール情報取得
        'Dim xmlString As String
        'Dim listValues As New List(Of String)

        ''CalDav情報取得
        'xmlString = SC3010203BusinessLogic.ReadMySchedule()

        ''分割



        Return SC3010203BusinessLogic.ReadMySchedule()

    End Function

    ''' <summary>
    ''' スケジュール情報取得処理。
    ''' </summary>
    ''' <returns>XML</returns>
    ''' <remarks></remarks>
    <WebMethod(EnableSession:=True)> _
    Public Shared Function GetTest() As String()

        Dim lis As List(Of String) = New List(Of String) From {"あああああああああああああああああああああああああ"}
        Return lis.ToArray()

    End Function
#End Region

#Region " CalDavデータ登録処理 "

    ''' <summary>
    ''' スケジュール登録
    ''' </summary>
    ''' <param name="dlrcd">販売店コード</param>
    ''' <param name="strcd">店舗コード</param>
    ''' <param name="todoid">TODOID</param>
    ''' <param name="scheduleid">スケジュールID</param>
    ''' <param name="year">年</param>
    ''' <param name="month">月</param>
    ''' <param name="day">日</param>
    ''' <param name="hour">時</param>
    ''' <param name="minute">分</param>
    ''' <returns>処理結果(0:正常、それ以外:エラー)</returns>
    ''' <remarks></remarks>
    <WebMethod(EnableSession:=True)> _
    Public Shared Function RegistSchedule(ByVal dlrcd As String, _
                                          ByVal strcd As String, _
                                          ByVal todoid As String, _
                                          ByVal scheduleid As String, _
                                          ByVal year As Integer, _
                                          ByVal month As Integer, _
                                          ByVal day As Integer, _
                                          ByVal hour As Integer, _
                                          ByVal minute As Integer) As Integer


        Using dtRegistInfo As New SC3010203CalDavRegistInfoDataTable

            Dim row As SC3010203CalDavRegistInfoRow = dtRegistInfo.NewSC3010203CalDavRegistInfoRow()

            '登録用データ格納
            With row
                .DLRCD = dlrcd
                .BRNCD = strcd
                .TODOID = todoid
                .SCHEDULEID = scheduleid
                .STARTTIME = New Date(year, month, day, hour, minute, 0)
                .ENDTIME = .STARTTIME.AddHours(1)
            End With
            dtRegistInfo.Rows.Add(row)

            '連携処理
            Return SC3010203BusinessLogic.RegistMySchedule(dtRegistInfo)
        End Using

    End Function
#End Region

#Region " フッター制御 "

    ''' <summary>
    ''' メニュー
    ''' </summary>
    ''' <remarks></remarks>
    Private Const MAIN_MENU As Integer = 100

    ''' <summary>
    ''' スケジュールサブメニュー
    ''' </summary>
    Private Const SUBMENU_SCHEDULE As Integer = 101
    ''' <summary>
    ''' 電話帳サブメニュー
    ''' </summary>
    Private Const SUBMENU_CONT As Integer = 102

    ''' <summary>
    ''' 顧客検索
    ''' </summary>
    Private Const CUSTOMER_SEARCH As Integer = 200

    ''' <summary>
    ''' フッター制御
    ''' </summary>
    ''' <param name="commonMaster">マスターページ</param>
    ''' <param name="category">カテゴリ</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Overrides Function DeclareCommonMasterFooter(commonMaster As Toyota.eCRB.SystemFrameworks.Web.CommonMasterPage, ByRef category As FooterMenuCategory) As Integer()
        'カテゴリ 
        category = FooterMenuCategory.MainMenu
        Return {SUBMENU_SCHEDULE, SUBMENU_CONT}
    End Function

    ''' <summary>
    ''' フッターボタンの制御
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub InitFooterEvent()

        'スケジュールアプリ起動
        Dim scheduleButton As CommonMasterFooterButton = CType(Me.Master.Master, CommonMasterPage).GetFooterButton(SUBMENU_SCHEDULE)
        scheduleButton.OnClientClick = "return schedule.appExecute.executeCaleNew();"
        '連絡先アプリ起動
        Dim contButton As CommonMasterFooterButton = CType(Me.Master.Master, CommonMasterPage).GetFooterButton(SUBMENU_CONT)
        contButton.OnClientClick = "return schedule.appExecute.executeCont();"

        'メニュー
        CType(Me.Master.Master, CommonMasterPage).GetFooterButton(MAIN_MENU).OnClientClick = "return false;"

        '顧客詳細
        Dim custSearch As CommonMasterFooterButton = CType(Me.Master.Master, CommonMasterPage).GetFooterButton(CUSTOMER_SEARCH)
        AddHandler custSearch.Click, _
            Sub()
                '顧客詳細に遷移
                Me.RedirectNextScreen("SC3080201")
            End Sub

        'Dim tcvButton As CommonMasterFooterButton = CType(Me.Master.Master, CommonMasterPage).GetFooterButton(FooterMenuCategory.TCV)
        'AddHandler tcvButton.Click, AddressOf tcvButton_Click
    End Sub

    ''' <summary>
    ''' TCSとの連携ボタン
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub tcvButton_Click(sender As Object, e As CommonMasterFooterButtonClickEventArgs)

        Dim context As StaffContext = StaffContext.Current

        'TCV機能に渡す引数を設定
        e.Parameters.Add("DataSource", "none")
        e.Parameters.Add("MenuLockFlag", False)
        e.Parameters.Add("Account", context.Account)
        e.Parameters.Add("NewActFlag", False)
        e.Parameters.Add("AccountStrCd", context.BrnCD)
        e.Parameters.Add("DlrCd", context.DlrCD)
        e.Parameters.Add("StrCd", String.Empty)
        e.Parameters.Add("FollowupBox_SeqNo", String.Empty)
        e.Parameters.Add("CstKind", String.Empty)
        e.Parameters.Add("CustomerClass", String.Empty)
        e.Parameters.Add("CRCustId", String.Empty)

    End Sub

#End Region

#Region " 顧客詳細画面へ遷移する為の処理 "

    ''' <summary>顧客種別</summary>
    Private Const SESSION_KEY_CSTKIND As String = "SearchKey.CSTKIND"
    ''' <summary>顧客分類</summary>
    Private Const SESSION_KEY_CUSTOMERCLASS As String = "SearchKey.CUSTOMERCLASS"
    ''' <summary>活動先顧客コード</summary>
    Private Const SESSION_KEY_CRCUSTID As String = "SearchKey.CRCUSTID"
    ''' <summary>FBOX SEQNO</summary>
    Private Const SESSION_KEY_FOLLOW_UP_BOX As String = "SearchKey.FOLLOW_UP_BOX"
    ''' <summary>Follow-upBoxの店舗コード</summary>
    Dim CONST_FLLWUPBOX_STRCD As String = "SearchKey.FLLWUPBOX_STRCD"
    ''' <summary>担当セールススタッフコード</summary>
    Private Const SESSION_KEY_SALESSTAFFCD As String = "SearchKey.SALESSTAFFCD"

    ''' <summary>
    ''' 顧客詳細に遷移する為のダミーボタンクリック
    ''' </summary>
    ''' <param name="sender">イベント発生元</param>
    ''' <param name="e">イベント引数</param>
    ''' <remarks></remarks>
    Protected Sub CustDetailDummyButton_Click(sender As Object, e As System.EventArgs) Handles CustDetailDummyButton.Click
        Dim cuctInfoDt As SC3010203CustInfoDataTable

        Using paramDt As New SC3010203CustInfoDataTable
            Dim dataRow As SC3010203CustInfoRow = paramDt.NewRow()

            '検索条件設定
            dataRow.DLRCD = selectDLRCD.Value
            dataRow.STRCD = selectSTRCD.Value
            dataRow.FLLWUPBOX_SEQNO = Long.Parse(selectFOLLOWUPBOXSEQNO.Value, CultureInfo.InvariantCulture)
            paramDt.Rows.Add(dataRow)

            '活動先検索
            cuctInfoDt = SC3010203BusinessLogic.GetCustInfo(paramDt)
        End Using
 
        '次画面遷移パラメータ設定
        Me.SetValue(ScreenPos.Next, SESSION_KEY_CSTKIND, cuctInfoDt(0).CUSTSEGMENT)
        Me.SetValue(ScreenPos.Next, SESSION_KEY_CUSTOMERCLASS, cuctInfoDt(0).CUSTOMERCLASS)
        Me.SetValue(ScreenPos.Next, SESSION_KEY_CRCUSTID, cuctInfoDt(0).CRCUSTID)
        Me.SetValue(ScreenPos.Next, CONST_FLLWUPBOX_STRCD, cuctInfoDt(0).STRCD)
        Me.SetValue(ScreenPos.Next, SESSION_KEY_FOLLOW_UP_BOX, cuctInfoDt(0).FLLWUPBOX_SEQNO.ToString(CultureInfo.InvariantCulture))
        Me.SetValue(ScreenPos.Next, SESSION_KEY_SALESSTAFFCD, cuctInfoDt(0).STAFFCD)

        '遷移処理
        Me.RedirectNextScreen("SC3080201")

    End Sub

#End Region

End Class
