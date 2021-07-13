'━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
'SC3010203.aspx.vb
'─────────────────────────────────────
'機能： SCメイン
'補足： 
'作成： 2011/11/18 TCS 寺本
'更新： 2014/02/26 TCS 河原
'更新： 2013/10/31 TCS 山田 i-CROP再構築後の新車納車システムに追加したリンク対応
'更新： 2014/05/20 TCS 河原 マネージャー機能
'更新： 2019/05/28 TS  舩橋 PostUAT-3098 カレンダーアプリ呼び出し変更
'更新： 2019/06/04 TS  重松 (FS)納車時オペレーションCS向上にむけた評価 Contactsボタン蓋閉め（UAT-0030）
'更新： 2019/06/21 TS  舩橋 PostUAT-3044 マネージャーメイン画面の「TCV Setting」ボタンを非表示にする
'─────────────────────────────────────

Imports System.Reflection
Imports System.Web.Services
Imports System.Globalization
Imports Toyota.eCRB.SystemFrameworks.Core
Imports Toyota.eCRB.SystemFrameworks.Core.iCROP.BizLogic
Imports Toyota.eCRB.Common.MainMenu.BizLogic
Imports Toyota.eCRB.Common.MainMenu.DataAccess
Imports Toyota.eCRB.Common.MainMenu.DataAccess.SC3010203DataSet
Imports Toyota.eCRB.SystemFrameworks.Core.iCROP.DataAccess

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
    ''' <summary>
    ''' 来店実績通常チップカラー
    ''' </summary>
    ''' <remarks></remarks>
    Private Const VISIT_SALES_TIP_COLOR As String = "VISIT_SALES_TIP_COLOR"
    '2013/10/31 TCS 山田 ADD i-CROP再構築後の新車納車システムに追加したリンク対応 START
    'URLスキーム
    Private Const URL_SCHEME As String = "TABLET_BROWSER_URL_SCHEME"
    Private Const URL_SCHEMES As String = "TABLET_BROWSER_URL_SCHEMES"
    '2013/10/31 TCS 山田 ADD i-CROP再構築後の新車納車システムに追加したリンク対応 END
#End Region


#Region " 初期処理 "
    Private DisplayDate As Date
    Private NextDisplayDate As Date
    Private CurrentDate As Date

    ''' <summary>
    ''' ページロード時の処理です。
    ''' </summary>
    ''' <param name="sender">イベント発生元</param>
    ''' <param name="e">イベントデータ</param>
    ''' <remarks></remarks>
    Protected Sub Page_Load(sender As Object, e As System.EventArgs) Handles Me.Load

        ' ======================== ログ出力 開始 ========================
        Logger.Info(String.Format(CultureInfo.InvariantCulture, " {0}_Start", MethodBase.GetCurrentMethod.Name))
        ' ======================== ログ出力 終了 ========================

        '現在日付を設定
        'CurrentDate = DateTimeFunc.FormatDate(3, DateTimeFunc.Now(StaffContext.Current.DlrCD))
        Dim NowDt As Date = DateTimeFunc.Now(StaffContext.Current.DlrCD)
        CurrentDate = New Date(NowDt.Year, NowDt.Month, NowDt.Day)

        '来店実績の初期化
        Dim visitActualList As New SC3010203DataSet.SC3010203VisitActualDataTable
        'Me.ActualVisitRepeater.DataSource = visitActualList
        'Me.ActualVisitRepeater.DataBind()
        'Me.VisitSalesCount.Visible = False

        '------------------TEST-------------------
        Logger.Info(String.Format(CultureInfo.InvariantCulture,
                                  "11111111111111111 NowDateLiteral.Text={0}",
                                  NowDateLiteral.Text))
        '------------------TEST-------------------

        'JavaScript用
        Dim serverNow As Date = DateTimeFunc.Now
        Yearhidden.Value = serverNow.Year
        Monthhidden.Value = serverNow.Month
        Dayhidden.Value = serverNow.Day
        HourHidden.Value = serverNow.Hour
        MinuteHidden.Value = serverNow.Minute

        If Not Me.IsPostBack Then
            isDisplayDate.Value = CurrentDate
            NextDisplayDate = CurrentDate
            NowDateLiteral.Text = DateTimeFunc.FormatDate(11, DateTimeFunc.Now(CurrentDate))

            'スケジュールの時間を設定
            TimeRepeater.DataSource = GetScheduleTimeList()
            TimeRepeater.DataBind()

            '30分ごとのメモリ線
            TimeLineBorderRepeater.DataSource = Enumerable.Range(1, 48)
            TimeLineBorderRepeater.DataBind()

            'CalDav連携エラー時のメッセージ
            CaldavRegistErrorMessage.Value = WebWordUtility.GetWord(CALDAV_ERROR_MESSAGEID)
            CaldavSelectErrorMessage.Value = WebWordUtility.GetWord(CALDAV_SELECT_ERROR_MESSAGEID)
            Me.isSwipeLockHidden.Value = "0"
            Me.isToDoChipDrop.Value = "0"
            Me.isContactHistoryTransfer.Value = "0"
            Me.isToDoBox.Value = "0"
            Me.toDoButtom.Value = "0"

            'セグメントボタン表示
            With ToDoDispSegmentedButton
                .Items.Add(New ListItem(HttpUtility.HtmlEncode(WebWordUtility.GetWord(20)), "1"))
                .Items.Add(New ListItem(HttpUtility.HtmlEncode(WebWordUtility.GetWord(21)), "2"))
            End With

            '来店実績チップカラー取得
            Dim sysEnv As New SystemEnvSetting
            Me.visitSalesTipColor.Value = sysEnv.GetSystemEnvSetting(VISIT_SALES_TIP_COLOR).PARAMVALUE

            Me.slash.Value = WebWordUtility.GetWord(24)

        Else
            If Not (Me.toDoButtom.Value = "1") Then
                NextDisplayDate = isDisplayDate.Value
                DateChange()
            End If
            Me.toDoButtom.Value = "0"
        End If

        'フッターの制御
        InitFooterEvent()

        '初期選択
        ToDoDispSegmentedButton.SelectedValue = "1"

        opeCD.Value = StaffContext.Current.OpeCD


        '来店実績用プロパティ設定
        Me.SC3100302.targetDay = NextDisplayDate
        Me.SC3100302.ToDoBoxMode = Me.isToDoBox.Value

        ' ======================== ログ出力 開始 ========================
        Logger.Info(String.Format(CultureInfo.InvariantCulture, " {0}_End", MethodBase.GetCurrentMethod.Name))
        ' ======================== ログ出力 終了 ========================

    End Sub

    ''' <summary>
    ''' PreRender時の処理
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub Page_PreRender(sender As Object, e As System.EventArgs) Handles Me.PreRender

        ' ======================== ログ出力 開始 ========================
        Logger.Info(String.Format(CultureInfo.InvariantCulture, " {0}_Start", MethodBase.GetCurrentMethod.Name))
        ' ======================== ログ出力 終了 ========================

        TcvSettingButtonSetting()

        ' ======================== ログ出力 開始 ========================
        Logger.Info(String.Format(CultureInfo.InvariantCulture, " {0}_End", MethodBase.GetCurrentMethod.Name))
        ' ======================== ログ出力 終了 ========================

    End Sub

#End Region


#Region " CalDavデータ取得処理 "
    ''' <summary>
    ''' スケジュール表示の為の24H分の時間テキストを取得します。
    ''' </summary>
    ''' <returns>24H分の時：分文字列の配列</returns>
    ''' <remarks></remarks>
    Private Function GetScheduleTimeList() As List(Of String)

        ' ======================== ログ出力 開始 ========================
        Logger.Info(String.Format(CultureInfo.InvariantCulture, " {0}_Start", MethodBase.GetCurrentMethod.Name))
        ' ======================== ログ出力 終了 ========================

        Dim listTime As New List(Of String)
        Dim timeValue As Date = DateTimeFunc.Now(StaffContext.Current.DlrCD)
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

        ' ======================== ログ出力 開始 ========================
        Logger.Info(String.Format(CultureInfo.InvariantCulture, " {0}_End", MethodBase.GetCurrentMethod.Name))
        ' ======================== ログ出力 終了 ========================

        '処理結果返却
        Return listTime

    End Function

    ''' <summary>
    ''' スケジュール情報取得処理。
    ''' </summary>
    ''' <returns>XML</returns>
    ''' <remarks></remarks>
    <WebMethod(EnableSession:=True)> _
    Public Shared Function GetScheduleXmlText(ByVal isDisplayDate As String) As String

        ' ======================== ログ出力 開始 ========================
        Logger.Info(String.Format(CultureInfo.InvariantCulture, " {0}_Start", MethodBase.GetCurrentMethod.Name))
        ' ======================== ログ出力 終了 ========================

        Dim staffInfo As StaffContext = StaffContext.Current()
        Dim bizLogic As SC3010203BusinessLogic = New SC3010203BusinessLogic(staffInfo.DlrCD, staffInfo.BrnCD, staffInfo.Account, staffInfo.OpeCD)

        ' ======================== ログ出力 開始 ========================
        Logger.Info(String.Format(CultureInfo.InvariantCulture, " {0}_End", MethodBase.GetCurrentMethod.Name))
        ' ======================== ログ出力 終了 ========================

        Return bizLogic.ReadMySchedule(isDisplayDate)

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

        ' ======================== ログ出力 開始 ========================
        Logger.Info(String.Format(CultureInfo.InvariantCulture, " {0}_Start", MethodBase.GetCurrentMethod.Name))
        ' ======================== ログ出力 終了 ========================

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

        ' ======================== ログ出力 開始 ========================
        Logger.Info(String.Format(CultureInfo.InvariantCulture, " {0}_End", MethodBase.GetCurrentMethod.Name))
        ' ======================== ログ出力 終了 ========================

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
    ''' ショールーム
    ''' </summary>
    Private Const SHOW_ROOM As Integer = 1200


    'TCV設定
    Private Const TCV_SETTING As Integer = FooterMenuCategory.TCVSetting

    '2013/10/31 TCS 山田 ADD i-CROP再構築後の新車納車システムに追加したリンク対応 START
    '新車納車システム連携メニュー
    Private Const LINK_MENU As Integer = FooterMenuCategory.LinkMenu
    'リンク先URL
    Private Const C_LINK_MENU_URL As String = "LINK_MENU_URL"
    '2013/10/31 TCS 山田 ADD i-CROP再構築後の新車納車システムに追加したリンク対応 END

    ''' <summary>
    ''' フッター制御
    ''' </summary>
    ''' <param name="commonMaster">マスターページ</param>
    ''' <param name="category">カテゴリ</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Overrides Function DeclareCommonMasterFooter(commonMaster As Toyota.eCRB.SystemFrameworks.Web.CommonMasterPage, ByRef category As FooterMenuCategory) As Integer()

        ' ======================== ログ出力 開始 ========================
        Logger.Info(String.Format(CultureInfo.InvariantCulture, " {0}_Start", MethodBase.GetCurrentMethod.Name))
        ' ======================== ログ出力 終了 ========================

        'カテゴリ 
        category = FooterMenuCategory.MainMenu
        Return {SUBMENU_SCHEDULE, SUBMENU_CONT}

        ' ======================== ログ出力 開始 ========================
        Logger.Info(String.Format(CultureInfo.InvariantCulture, " {0}_End", MethodBase.GetCurrentMethod.Name))
        ' ======================== ログ出力 終了 ========================

    End Function


    ''' <summary>
    ''' フッターボタンの制御
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub InitFooterEvent()

        ' ======================== ログ出力 開始 ========================
        Logger.Info(String.Format(CultureInfo.InvariantCulture, " {0}_Start", MethodBase.GetCurrentMethod.Name))
        ' ======================== ログ出力 終了 ========================

        'スケジュールアプリ起動
        Dim scheduleButton As CommonMasterFooterButton = CType(Me.Master.Master, CommonMasterPage).GetFooterButton(SUBMENU_SCHEDULE)
        ' 2019/05/28 TS 舩橋 PostUAT-3098 カレンダーアプリ呼び出し変更 START 
        scheduleButton.OnClientClick = "return CallToiOSSchedule();"
        ' 2019/05/28 TS 舩橋 PostUAT-3098 カレンダーアプリ呼び出し変更 END 
        '連絡先アプリ起動
        Dim contButton As CommonMasterFooterButton = CType(Me.Master.Master, CommonMasterPage).GetFooterButton(SUBMENU_CONT)
        contButton.OnClientClick = "return schedule.appExecute.executeCont();"
        '2019/06/04 TS 重松 (FS)納車時オペレーションCS向上にむけた評価 Contactsボタン蓋閉め（UAT-0030） START
        contButton.Visible = False
        contButton.Enabled = False
        '2019/06/04 TS 重松 (FS)納車時オペレーションCS向上にむけた評価 Contactsボタン蓋閉め（UAT-0030） END

        'メニュー
        CType(Me.Master.Master, CommonMasterPage).GetFooterButton(MAIN_MENU).OnClientClick = "return false;"

        '顧客詳細
        Dim custSearch As CommonMasterFooterButton = CType(Me.Master.Master, CommonMasterPage).GetFooterButton(CUSTOMER_SEARCH)
        AddHandler custSearch.Click, _
            Sub()
                '顧客詳細に遷移
                Me.RedirectNextScreen("SC3080201")
            End Sub

        'ショールーム
        Dim ssvButton As CommonMasterFooterButton = CType(Me.Master.Master, CommonMasterPage).GetFooterButton(SHOW_ROOM)
        If ssvButton IsNot Nothing Then
            AddHandler ssvButton.Click, _
            Sub()
                '受付メインに遷移
                Me.RedirectNextScreen("SC3100101")
            End Sub
        End If

        Dim tcvButton As CommonMasterFooterButton = CType(Me.Master.Master, CommonMasterPage).GetFooterButton(FooterMenuCategory.TCV)
        AddHandler tcvButton.Click, AddressOf tcvButton_Click

        'TCV設定
        Dim tcvSettingButton As CommonMasterFooterButton = CType(Me.Master.Master, CommonMasterPage).GetFooterButton(TCV_SETTING)
        If tcvSettingButton IsNot Nothing Then
            AddHandler tcvSettingButton.Click, _
            Sub()
                'TCV設定に遷移
                Me.RedirectNextScreen("SC3050704")
            End Sub
        End If

        '2013/10/31 TCS 山田 ADD i-CROP再構築後の新車納車システムに追加したリンク対応 START
        '新車納車システム連携メニュー
        Dim linkMenuButton As CommonMasterFooterButton = CType(Me.Master.Master, CommonMasterPage).GetFooterButton(LINK_MENU)
        ''リンク先URLを販売店環境設定TBLより取得
        Dim dlrenvdt As New DealerEnvSetting
        Dim dlrenvrw As DlrEnvSettingDataSet.DLRENVSETTINGRow
        dlrenvrw = dlrenvdt.GetEnvSetting(StaffContext.Current.DlrCD, C_LINK_MENU_URL)
        If dlrenvrw IsNot Nothing Then
            If Not String.IsNullOrWhiteSpace(dlrenvrw.PARAMVALUE) Then
                ''URLを取得できた場合、新車納車システム連携メニューを表示。
                If linkMenuButton IsNot Nothing Then
                    linkMenuButton.Visible = True
                    ''システム環境設定より別ブラウザのURLスキーム取得。
                    Dim sysenv As New SystemEnvSetting
                    Dim rw1 As SystemEnvSettingDataSet.SYSTEMENVSETTINGRow = sysenv.GetSystemEnvSetting(URL_SCHEME)
                    Dim rw2 As SystemEnvSettingDataSet.SYSTEMENVSETTINGRow = sysenv.GetSystemEnvSetting(URL_SCHEMES)
                    ''新車納車システムへのリンクURL作成。(URLスキーム置き換え)
                    Dim linkUrl As String = dlrenvrw.PARAMVALUE
                    linkUrl = linkUrl.Replace("http://", rw1.PARAMVALUE + "://")
                    linkUrl = linkUrl.Replace("https://", rw2.PARAMVALUE + "://")
                    linkUrl = linkUrl.Replace("$1", HttpUtility.UrlEncode(StaffContext.Current.DlrCD))
                    linkUrl = linkUrl.Replace("$2", HttpUtility.UrlEncode(StaffContext.Current.BrnCD))
                    linkUrl = linkUrl.Replace("$3", HttpUtility.UrlEncode(StaffContext.Current.Account))
                    linkUrl = linkUrl.Replace("$4", String.Empty)
                    linkUrl = linkUrl.Replace("$5", String.Empty)

                    ''メニューをタップしたときに実行されるJavaScript。
                    linkMenuButton.OnClientClick = BindParameters("return schedule.appExecute.linkMenu('{0}');", {linkUrl})
                End If
            End If
        End If
        '2013/10/31 TCS 山田 ADD i-CROP再構築後の新車納車システムに追加したリンク対応 END

        'ログイン権限がセールスマネージャ、ブランチマネージャ権限場合、顧客ボタンを非表示
        Dim OpeCD As Integer = StaffContext.Current.OpeCD
        Dim SSM As Integer = Operation.SSM
        Dim BM As Integer = Operation.BM
        If OpeCD = SSM Or OpeCD = BM Then
            CType(Me.Master.Master, CommonMasterPage).GetFooterButton(CUSTOMER_SEARCH).Visible = False
        End If

        ' ======================== ログ出力 開始 ========================
        Logger.Info(String.Format(CultureInfo.InvariantCulture, " {0}_End", MethodBase.GetCurrentMethod.Name))
        ' ======================== ログ出力 終了 ========================

    End Sub

    '2013/10/31 TCS 山田 ADD i-CROP再構築後の新車納車システムに追加したリンク対応 START
    ''' <summary>
    ''' 文字列にパラメータをバインドします。
    ''' </summary>
    ''' <param name="value">文字列</param>
    ''' <param name="parameters">パラメータ</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Protected Function BindParameters(ByVal value As String, ByVal parameters As Object()) As String
        Return String.Format(CultureInfo.InvariantCulture, value, parameters)
    End Function
    '2013/10/31 TCS 山田 ADD i-CROP再構築後の新車納車システムに追加したリンク対応 END

    ''' <summary>
    ''' TCVとの連携ボタン
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub tcvButton_Click(sender As Object, e As CommonMasterFooterButtonClickEventArgs)

        ' ======================== ログ出力 開始 ========================
        Logger.Info(String.Format(CultureInfo.InvariantCulture, " {0}_Start", MethodBase.GetCurrentMethod.Name))
        ' ======================== ログ出力 終了 ========================

        Dim context As StaffContext = StaffContext.Current

        'TCV機能に渡す引数を設定
        e.Parameters.Add("DataSource", "none")
        e.Parameters.Add("MenuLockFlag", False)
        e.Parameters.Add("Account", context.Account)
        e.Parameters.Add("AccountStrCd", context.BrnCD)
        e.Parameters.Add("DlrCd", context.DlrCD)
        e.Parameters.Add("StrCd", String.Empty)
        e.Parameters.Add("FollowupBox_SeqNo", String.Empty)
        e.Parameters.Add("CstKind", String.Empty)
        e.Parameters.Add("CustomerClass", String.Empty)
        e.Parameters.Add("CRCustId", String.Empty)
        e.Parameters.Add("OperationCode", context.OpeCD)
        e.Parameters.Add("BusinessFlg", False)
        e.Parameters.Add("ReadOnlyFlg", False)

        ' ======================== ログ出力 開始 ========================
        Logger.Info(String.Format(CultureInfo.InvariantCulture, " {0}_End", MethodBase.GetCurrentMethod.Name))
        ' ======================== ログ出力 終了 ========================

    End Sub


    ''' <summary>
    ''' TCV設定ボタン
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub TcvSettingButtonSetting()

        ' ======================== ログ出力 開始 ========================
        Logger.Info(String.Format(CultureInfo.InvariantCulture, " {0}_Start", MethodBase.GetCurrentMethod.Name))
        ' ======================== ログ出力 終了 ========================

        ' 2019/06/21 TS  舩橋 PostUAT-3044 マネージャーメイン画面の「TCV Setting」ボタンを非表示にする START
        Dim OpeCD As Integer = StaffContext.Current.OpeCD
        Dim SSM As Integer = Operation.SSM
        'ログイン権限がセールスマネージャの場合、TCV設定画面ボタンを非表示にする
        If OpeCD = SSM Then
            CType(Me.Master.Master, CommonMasterPage).GetFooterButton(TCV_SETTING).Visible = False
        End If
        ' 2019/06/21 TS  舩橋 PostUAT-3044 マネージャーメイン画面の「TCV Setting」ボタンを非表示にする END

        ' ======================== ログ出力 開始 ========================
        Logger.Info(String.Format(CultureInfo.InvariantCulture, " {0}_End", MethodBase.GetCurrentMethod.Name))
        ' ======================== ログ出力 終了 ========================

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
    ''' <summary>セールスステータス</summary>
    Private Const SESSION_KEY_SALES_STATUS As String = "SearchKey.SALES_STATUS"

    ''' <summary>
    ''' 顧客詳細に遷移する為のダミーボタンクリック
    ''' </summary>
    ''' <param name="sender">イベント発生元</param>
    ''' <param name="e">イベント引数</param>
    ''' <remarks></remarks>
    Protected Sub CustDetailDummyButton_Click(sender As Object, e As System.EventArgs) Handles CustDetailDummyButton.Click

        ' ======================== ログ出力 開始 ========================
        Logger.Info(String.Format(CultureInfo.InvariantCulture, " {0}_Start", MethodBase.GetCurrentMethod.Name))
        ' ======================== ログ出力 終了 ========================

        '来店実績以外から画面遷移するか？
        If String.Equals(isContactHistoryTransfer.Value.ToString, "0") Then

            Dim cuctInfoDt As SC3010203CustInfoDataTable

            Using paramDt As New SC3010203CustInfoDataTable
                Dim dataRow As SC3010203CustInfoRow = paramDt.NewRow()

                '検索条件設定
                dataRow.FLLWUPBOX_SEQNO = Decimal.Parse(selectFOLLOWUPBOXSEQNO.Value, CultureInfo.InvariantCulture)
                paramDt.Rows.Add(dataRow)

                '活動先検索
                cuctInfoDt = SC3010203BusinessLogic.GetCustInfo(paramDt)
            End Using

            '次画面遷移パラメータ設定(TODO画面からSALESSTATUSを商談開始などの判定は必要ないためパラメータから除外)
            Me.SetValue(ScreenPos.Next, SESSION_KEY_CSTKIND, cuctInfoDt(0).CUSTSEGMENT)
            Me.SetValue(ScreenPos.Next, SESSION_KEY_CUSTOMERCLASS, cuctInfoDt(0).CUSTOMERCLASS)
            Me.SetValue(ScreenPos.Next, SESSION_KEY_CRCUSTID, cuctInfoDt(0).CRCUSTID)
            Me.SetValue(ScreenPos.Next, CONST_FLLWUPBOX_STRCD, cuctInfoDt(0).STRCD)
            Me.SetValue(ScreenPos.Next, SESSION_KEY_FOLLOW_UP_BOX, cuctInfoDt(0).FLLWUPBOX_SEQNO.ToString(CultureInfo.InvariantCulture))
        Else
            '次画面遷移パラメータ設定(SALESSTAFFCDについては顧客画面側で取得しているためパラメータから除外）
            Me.SetValue(ScreenPos.Next, SESSION_KEY_CSTKIND, Me.selectCSTKIND.Value)
            Me.SetValue(ScreenPos.Next, SESSION_KEY_CUSTOMERCLASS, Me.selectCUSTOMERCLASS.Value)
            Me.SetValue(ScreenPos.Next, SESSION_KEY_CRCUSTID, Trim(Me.selectCRCUSTID.Value))
            Me.SetValue(ScreenPos.Next, CONST_FLLWUPBOX_STRCD, Me.selectSTRCD.Value)
            Me.SetValue(ScreenPos.Next, SESSION_KEY_FOLLOW_UP_BOX, selectFOLLOWUPBOXSEQNO.Value.ToString(CultureInfo.InvariantCulture))
            '値が指定されているか？
            If String.IsNullOrEmpty(Me.selectSALESSTATUS.Value) = False Then
                Me.SetValue(ScreenPos.Next, SESSION_KEY_SALES_STATUS, Me.selectSALESSTATUS.Value)
            End If
        End If

        ' ======================== ログ出力 開始 ========================
        Logger.Info(String.Format(CultureInfo.InvariantCulture, " {0}_End", MethodBase.GetCurrentMethod.Name))
        ' ======================== ログ出力 終了 ========================

        '遷移処理
        Me.RedirectNextScreen("SC3080201")
    End Sub

#End Region


#Region "ToDo一覧に遷移するための処理"

    ''' <summary>
    ''' ToDo一覧に遷移する為のダミーボタンクリック
    ''' </summary>
    ''' <param name="sender">イベント発生元</param>
    ''' <param name="e">イベント引数</param>
    ''' <remarks></remarks>
    Protected Sub ToDoTitleButton_Click(sender As Object, e As System.EventArgs) Handles toDoTitleButton.Click

        ' ======================== ログ出力 開始 ========================
        Logger.Info(String.Format(CultureInfo.InvariantCulture, " {0}_Start", MethodBase.GetCurrentMethod.Name))
        ' ======================== ログ出力 終了 ========================

        '遷移処理
        Me.RedirectNextScreen("SC3010401")

        ' ======================== ログ出力 開始 ========================
        Logger.Info(String.Format(CultureInfo.InvariantCulture, " {0}_End", MethodBase.GetCurrentMethod.Name))
        ' ======================== ログ出力 終了 ========================

    End Sub

#End Region


#Region "ネットワーク切断時・再表示処理"
    ''' <summary>
    ''' 再表示ボタン(隠しボタン)押下時
    ''' </summary>
    ''' <param name="sender">ページオブジェクト</param>
    ''' <param name="e">イベント引数</param>
    ''' <remarks></remarks>
    Protected Sub refreshButton_Click(sender As Object, e As System.EventArgs) Handles refreshButton.Click

        ' ======================== ログ出力 開始 ========================
        Logger.Info(String.Format(CultureInfo.InvariantCulture, " {0}_Start", MethodBase.GetCurrentMethod.Name))
        ' ======================== ログ出力 終了 ========================

        If (Me.ContainsKey(ScreenPos.Current, SESSION_KEY_CRCUSTID) = True) Then

            '活動先顧客コードが存在すれば、すでに、顧客詳細に遷移してる場合
            Toyota.eCRB.SystemFrameworks.Core.Logger.Info("Me.RedirectPrevScreen()")

            '前画面へ戻る
            Me.RedirectPrevScreen()
        Else

            '活動先顧客コードが存在すれば、まだ、顧客詳細に遷移していない
            Toyota.eCRB.SystemFrameworks.Core.Logger.Info("Me.RedirectNextScreen(SC3010203)")

            '遷移処理　(再度メニューに遷移する)
            Me.RedirectNextScreen("SC3010203")

        End If

        ' ======================== ログ出力 開始 ========================
        Logger.Info(String.Format(CultureInfo.InvariantCulture, " {0}_End", MethodBase.GetCurrentMethod.Name))
        ' ======================== ログ出力 終了 ========================]

    End Sub
#End Region


#Region "TodoPrev 一覧に遷移するための処理"

    ''' <summary>
    ''' Prevボタンイベント
    ''' </summary>
    ''' <param name="sender">ページオブジェクト</param>
    ''' <param name="e">イベント引数</param>
    ''' <remarks></remarks>
    Protected Sub ToDoPrevButtom_Click(sender As Object, e As System.EventArgs) Handles toDoPrevButtom.Click

        ' ======================== ログ出力 開始 ========================
        Logger.Info(String.Format(CultureInfo.InvariantCulture, " {0}_Start", MethodBase.GetCurrentMethod.Name))
        ' ======================== ログ出力 終了 ========================

        'Todo Prev
        DisplayDate = isDisplayDate.Value
        NextDisplayDate = DateAdd(DateInterval.Day, -1, DisplayDate)
        Yearhidden.Value = NextDisplayDate.Year
        Monthhidden.Value = NextDisplayDate.Month
        Dayhidden.Value = NextDisplayDate.Day
        DateChange()
        GetScheduleXmlText(NextDisplayDate)

        ' ======================== ログ出力 開始 ========================
        Logger.Info(String.Format(CultureInfo.InvariantCulture, " {0}_End", MethodBase.GetCurrentMethod.Name))
        ' ======================== ログ出力 終了 ========================

    End Sub

#End Region


#Region "TodoToday 一覧に遷移するための処理"

    ''' <summary>
    ''' Todayボタンイベント
    ''' </summary>
    ''' <param name="sender">ページオブジェクト</param>
    ''' <param name="e">イベント引数</param>
    ''' <remarks></remarks>
    Protected Sub ToDoTodayButtom_Click(sender As Object, e As System.EventArgs) Handles toDoTodayButtom.Click

        ' ======================== ログ出力 開始 ========================
        Logger.Info(String.Format(CultureInfo.InvariantCulture, " {0}_Start", MethodBase.GetCurrentMethod.Name))
        ' ======================== ログ出力 終了 ========================

        'Todo Today
        NextDisplayDate = CurrentDate
        NowDateLiteral.Text = DateTimeFunc.FormatDate(11, DateTimeFunc.Now(NextDisplayDate))
        isDisplayDate.Value = NextDisplayDate
        Yearhidden.Value = NextDisplayDate.Year
        Monthhidden.Value = NextDisplayDate.Month
        Dayhidden.Value = NextDisplayDate.Day
        Me.isToDoBox.Value = "0"
        GetScheduleXmlText(NextDisplayDate)

        ' ======================== ログ出力 開始 ========================
        Logger.Info(String.Format(CultureInfo.InvariantCulture, " {0}_End", MethodBase.GetCurrentMethod.Name))
        ' ======================== ログ出力 終了 ========================

    End Sub

#End Region


#Region "TodoNext 一覧に遷移するための処理"

    ''' <summary>
    ''' Nextボタンイベント
    ''' </summary>
    ''' <param name="sender">ページオブジェクト</param>
    ''' <param name="e">イベント引数</param>
    ''' <remarks></remarks>
    Protected Sub ToDoNextButtom_Click(sender As Object, e As System.EventArgs) Handles toDoNextButtom.Click

        ' ======================== ログ出力 開始 ========================
        Logger.Info(String.Format(CultureInfo.InvariantCulture, " {0}_Start", MethodBase.GetCurrentMethod.Name))
        ' ======================== ログ出力 終了 ========================

        'Todo Next
        DisplayDate = isDisplayDate.Value
        NextDisplayDate = DateAdd(DateInterval.Day, 1, DisplayDate)
        Yearhidden.Value = NextDisplayDate.Year
        Monthhidden.Value = NextDisplayDate.Month
        Dayhidden.Value = NextDisplayDate.Day
        DateChange()
        GetScheduleXmlText(NextDisplayDate)

        ' ======================== ログ出力 開始 ========================
        Logger.Info(String.Format(CultureInfo.InvariantCulture, " {0}_End", MethodBase.GetCurrentMethod.Name))
        ' ======================== ログ出力 終了 ========================

    End Sub

#End Region


#Region "日付表示方法編集(ﾍｯﾀﾞｰ部)"

    ''' <summary>
    ''' 日付変更
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub DateChange()

        ' ======================== ログ出力 開始 ========================
        Logger.Info(String.Format(CultureInfo.InvariantCulture, " {0}_Start", MethodBase.GetCurrentMethod.Name))
        ' ======================== ログ出力 終了 ========================

        '年比較(同じ場合、年の非表示。違う場合、年の表示)
        If NextDisplayDate.Year = CurrentDate.Year Then
            NowDateLiteral.Text = DateTimeFunc.FormatDate(11, NextDisplayDate)
        Else
            NowDateLiteral.Text = DateTimeFunc.FormatDate(3, NextDisplayDate)
        End If
        isDisplayDate.Value = NextDisplayDate

        'ボタンToDo及びALLの非表示(当日は表示)
        If (NextDisplayDate = CurrentDate) Then
            '当日の場合
            Me.isToDoBox.Value = "0"
        ElseIf NextDisplayDate > CurrentDate Then
            '未来日の場合
            Me.isToDoBox.Value = "1"
        Else
            '過去日の場合
            Me.isToDoBox.Value = "2"
        End If

        ' ======================== ログ出力 開始 ========================
        Logger.Info(String.Format(CultureInfo.InvariantCulture, " {0}_End", MethodBase.GetCurrentMethod.Name))
        ' ======================== ログ出力 終了 ========================

    End Sub

#End Region


    '#Region "来店実績"


    '    ''' <summary>
    '    ''' 来店実績取得
    '    ''' </summary>
    '    ''' <param name="sender"></param>
    '    ''' <param name="e"></param>
    '    ''' <remarks></remarks>
    '    Protected Sub GetVisitActualList(sender As Object, e As System.EventArgs) Handles VisitSalesTrigger.Click

    '        ' ======================== ログ出力 開始 ========================
    '        Logger.Info(String.Format(CultureInfo.InvariantCulture, " {0}_Start", MethodBase.GetCurrentMethod.Name))
    '        ' ======================== ログ出力 終了 ========================

    '        Dim staffInfo As StaffContext = StaffContext.Current()
    '        Dim bizLogic As SC3010203BusinessLogic = New SC3010203BusinessLogic(staffInfo.DlrCD, staffInfo.BrnCD, staffInfo.Account, staffInfo.OpeCD)
    '        Dim visitActualList As SC3010203DataSet.SC3010203VisitActualDataTable
    '        Dim script As New StringBuilder

    '        If String.Equals(Me.isToDoBox.Value, "0") Then
    '            '来店実績一覧取得
    '            visitActualList = bizLogic.SelectVisitActualList("1", Nothing, Nothing)
    '        ElseIf String.Equals(Me.isToDoBox.Value, "2") Then

    '            Dim startDatetime = NextDisplayDate
    '            Dim endDatetime = New Date(NextDisplayDate.Year, NextDisplayDate.Month, NextDisplayDate.Day, 23, 59, 59)

    '            visitActualList = bizLogic.SelectVisitActualList("2", startDatetime, endDatetime)
    '        Else
    '            visitActualList = New SC3010203DataSet.SC3010203VisitActualDataTable
    '        End If

    '        Dim VisitSalesTotalCount As Integer
    '        Dim VisitSalesDueCount As Integer

    '        VisitSalesTotalCount = visitActualList.Count

    '        For Each dr As SC3010203DataSet.SC3010203VisitActualRow In visitActualList
    '            '1: 商談、2: 納車作業
    '            If String.Equals(dr.CST_SERVICE_TYPE, "1") Then
    '                dr.CST_SERVICE_NAME = HttpUtility.HtmlEncode(WebWordUtility.GetWord(22))
    '            ElseIf String.Equals(dr.CST_SERVICE_TYPE, "2") Then
    '                dr.CST_SERVICE_NAME = HttpUtility.HtmlEncode(WebWordUtility.GetWord(23))
    '            Else
    '                dr.CST_SERVICE_NAME = HttpUtility.HtmlEncode(WebWordUtility.GetWord(25))
    '            End If

    '            '遅れの件数をカウント
    '            If String.Equals(dr.DELAY_STATUS, SC3010203BusinessLogic.DELAY_STATUS_DELAY) Or String.Equals(dr.DELAY_STATUS, SC3010203BusinessLogic.DELAY_STATUS_DUE) Then
    '                VisitSalesDueCount = VisitSalesDueCount + 1
    '            End If
    '        Next

    '        '件数を設定
    '        If String.Equals(Me.isToDoBox.Value, "0") Then
    '            Dim Count As New StringBuilder
    '            Count.Append(VisitSalesDueCount)
    '            Count.Append(WebWordUtility.GetWord(24))
    '            Count.Append(VisitSalesTotalCount)
    '            Me.VisitSalesCount.Text = Count.ToString
    '        ElseIf String.Equals(Me.isToDoBox.Value, "2") Then
    '            Me.VisitSalesCount.Text = VisitSalesTotalCount
    '        Else
    '            Me.VisitSalesCount.Text = "0"
    '        End If

    '        Me.VisitSalesCount.Visible = True

    '        'データを設定
    '        Me.ActualVisitRepeater.DataSource = visitActualList
    '        Me.ActualVisitRepeater.DataBind()

    '        'スクロールバーを表示
    '        script.AppendLine("$('#VisitBoxIn').fingerScroll();")
    '        script.AppendLine("$('#VisitBoxIn #VisitActualRow:last-child').css('padding-bottom','10px');")
    '        script.AppendLine("$('.colorDue .SCMainChip').css('background',getVisitSalesTipColor());")
    '        script.AppendLine("ToDoDispChange();")
    '        script.AppendLine("$('.clearboth').removeClass('loadingVisitActual')")

    '        ScriptManager.RegisterClientScriptBlock(Me.VisitSales, Me.GetType, "", script.ToString, True)

    '        ' ======================== ログ出力 開始 ========================
    '        Logger.Info(String.Format(CultureInfo.InvariantCulture, " {0}_End", MethodBase.GetCurrentMethod.Name))
    '        ' ======================== ログ出力 終了 ========================

    '    End Sub


    '    ''' <summary>
    '    ''' 来店実績データ設定
    '    ''' </summary>
    '    ''' <param name="sender"></param>
    '    ''' <param name="e"></param>
    '    ''' <remarks></remarks>
    '    Protected Sub ActualVisitRepeater_ItemDataBound(sender As Object, e As System.Web.UI.WebControls.RepeaterItemEventArgs) Handles ActualVisitRepeater.ItemDataBound

    '        ' ======================== ログ出力 開始 ========================
    '        Logger.Info(String.Format(CultureInfo.InvariantCulture, " {0}_Start", MethodBase.GetCurrentMethod.Name))
    '        ' ======================== ログ出力 終了 ========================

    '        If e.Item.ItemType = ListItemType.Item _
    '                 OrElse e.Item.ItemType = ListItemType.AlternatingItem Then

    '            Dim view As Data.DataView = DirectCast(e.Item.DataItem.DataView, Data.DataView)
    '            Dim row As SC3010203DataSet.SC3010203VisitActualRow = DirectCast(e.Item.DataItem.row, SC3010203DataSet.SC3010203VisitActualRow)
    '            Dim onlineStatusIconArea As HtmlGenericControl = DirectCast(e.Item.FindControl("OnlineStatusIconArea"), HtmlGenericControl)
    '            Dim VisitActualRow As HtmlGenericControl = DirectCast(e.Item.FindControl("VisitActualRow"), HtmlGenericControl)
    '            Dim TempStaffOperationIcon As HtmlImage = DirectCast(e.Item.FindControl("TempStaffOperationIcon"), HtmlImage)
    '            Dim NextActivityIcon As HtmlImage = DirectCast(e.Item.FindControl("NextActivityIcon"), HtmlImage)

    '            VisitActualRow.Attributes("Class") = ""

    '            'チップの色を指定
    '            If String.Equals(row.REGISTFLG, "0") Then
    '                Select Case row.DELAY_STATUS
    '                    Case SC3010203BusinessLogic.DELAY_STATUS_DELAY
    '                        '遅れ
    '                        AddCssClass(VisitActualRow, "colorDelay")
    '                    Case SC3010203BusinessLogic.DELAY_STATUS_DUE
    '                        '当日(活動結果未登録)
    '                        AddCssClass(VisitActualRow, "colorDue")
    '                    Case Else
    '                        '当日(活動結果登録済)
    '                        AddCssClass(VisitActualRow, "colorComplete")
    '                        AddCssClass(VisitActualRow, "completion")
    '                End Select
    '            Else
    '                '活動結果登録済
    '                AddCssClass(VisitActualRow, "colorComplete")
    '                AddCssClass(VisitActualRow, "completion")
    '            End If

    '            If String.IsNullOrEmpty(row.TEMP_STAFF_OPERATIONCODE_ICON) Then
    '                '一次対応者の権限アイコンを非表示
    '                AddCssClass(TempStaffOperationIcon, "imageHidden")
    '            End If

    '        End If

    '        ' ======================== ログ出力 開始 ========================
    '        Logger.Info(String.Format(CultureInfo.InvariantCulture, " {0}_End", MethodBase.GetCurrentMethod.Name))
    '        ' ======================== ログ出力 終了 ========================

    '    End Sub


    '    ''' <summary>
    '    ''' クラス名を付与
    '    ''' </summary>
    '    ''' <param name="element"></param>
    '    ''' <param name="cssClass"></param>
    '    ''' <remarks></remarks>
    '    Private Sub AddCssClass(ByVal element As HtmlControl, ByVal cssClass As String)

    '        ' ======================== ログ出力 開始 ========================
    '        Logger.Info(String.Format(CultureInfo.InvariantCulture, " {0}_Start", MethodBase.GetCurrentMethod.Name))
    '        ' ======================== ログ出力 終了 ========================

    '        If String.IsNullOrEmpty(element.Attributes("Class")) Then
    '            element.Attributes("Class") = cssClass
    '        Else
    '            element.Attributes("Class") = element.Attributes("Class") & " " & cssClass
    '        End If

    '        ' ======================== ログ出力 開始 ========================
    '        Logger.Info(String.Format(CultureInfo.InvariantCulture, " {0}_End", MethodBase.GetCurrentMethod.Name))
    '        ' ======================== ログ出力 終了 ========================

    '    End Sub


    '    ''' <summary>
    '    ''' クラス名を除去
    '    ''' </summary>
    '    ''' <param name="element"></param>
    '    ''' <param name="cssClass"></param>
    '    ''' <remarks></remarks>
    '    Private Sub RemoveCssClass(ByVal element As HtmlControl, ByVal cssClass As String)

    '        ' ======================== ログ出力 開始 ========================
    '        Logger.Info(String.Format(CultureInfo.InvariantCulture, " {0}_Start", MethodBase.GetCurrentMethod.Name))
    '        ' ======================== ログ出力 終了 ========================

    '        element.Attributes("Class") = element.Attributes("Class").Replace(cssClass, "")

    '        ' ======================== ログ出力 開始 ========================
    '        Logger.Info(String.Format(CultureInfo.InvariantCulture, " {0}_End", MethodBase.GetCurrentMethod.Name))
    '        ' ======================== ログ出力 終了 ========================

    '    End Sub


    '#End Region

#Region "画面遷移用処理"

    '2014/05/20 TCS 河原 マネージャー機能 Start

    Private Const IRREGULAR_CLASS_CD As String = "IRREGULAR_CLASS_CD"
    Private Const IRREGULAR_ITEM_CD As String = "IRREGULAR_ITEM_CD"

    ''' <summary>
    ''' メインフレーム画面遷移
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub moveMainFrame(ByVal sender As Object, ByVal e As System.EventArgs) Handles moveMainFrameDummyButton.Click

        'セッションの設定
        Me.SetValue(ScreenPos.Next, IRREGULAR_CLASS_CD, Me.abnormalClassCD.Value)
        Me.SetValue(ScreenPos.Next, IRREGULAR_ITEM_CD, Me.abnormalItemCD.Value)

        Dim transitionsDiv As String = Me.transitionsDiv.Value

        If String.Equals(transitionsDiv, "1") Then
            '異常詳細画面に遷移
            Me.RedirectNextScreen("SC3290103")
        ElseIf String.Equals(transitionsDiv, "2") Then
            'SPMフレーム画面に遷移
            Me.RedirectNextScreen("SC3120201")
        End If

    End Sub

    '2014/05/20 TCS 河原 マネージャー機能 End

#End Region

End Class