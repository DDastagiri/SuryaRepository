
Option Explicit On
Option Strict On

Imports System.Globalization
Imports Toyota.eCRB.CustomerInfo.ToDoList.BizLogic
Imports Toyota.eCRB.CustomerInfo.ToDoList.DataAccess
Imports Toyota.eCRB.iCROP.BizLogic.CalenderXmlCreateClass.BizLogic
Imports Toyota.eCRB.SystemFrameworks.Core
Imports Toyota.eCRB.SystemFrameworks.Core.iCROP.BizLogic
Imports Toyota.eCRB.SystemFrameworks.Core.iCROP.DataAccess
Imports Toyota.eCRB.SystemFrameworks.Web
Imports Toyota.eCRB.SystemFrameworks.Web.Controls
Imports System.Reflection

''' <summary>
''' ToDo一覧
''' </summary>
''' <remarks>
''' </remarks>
''' <history>
''' <para>作成： 2012/02/01 TCS 竹内</para>
''' <para>更新： 2012/03/13 TCS 渡邊   $01 SalesStep2ユーザーテスト課題No.15、18、36</para>
''' <para>更新： 2012/05/29 TCS 神本   クルクル対応</para>
''' <para>更新： 2013/01/10 TCS 橋本   【A.STEP2】次世代e-CRB 新車タブレットSC活動支援機能開発</para>
''' <para>更新： 2013/06/30 TCS 武田   2013/10対応版　既存流用</para>
''' <para>更新： 2014/02/17 TCS 山田   受注後フォロー機能開発</para>
''' <para>更新： 2015/12/08 TCS 中村   (ﾄﾗｲ店ｼｽﾃﾑ評価)新車ﾀﾌﾞﾚｯﾄ(受注後)の管理機能開発</para>
''' <para>更新： 2018/08/27 TCS 佐々木 TKM Next Gen e-CRB Project Application development Block B-3</para>
''' </history>
Partial Class SC3010401
    Inherits BasePage


#Region "定数"

    ''' <summary>
    ''' 絞り込み条件
    ''' </summary>
    ''' <remarks></remarks>
    Public Const SESSION_KEY_SERCHDELAY As String = "searchDelay"
    Public Const SESSION_KEY_SERCHTODAY As String = "searchToday"
    Public Const SESSION_KEY_SERCHFUTURE As String = "searchFuture"
    Public Const SESSION_KEY_SERCHCOLD As String = "searchCold"
    Public Const SESSION_KEY_SERCHWARM As String = "searchWarm"
    Public Const SESSION_KEY_SERCHHOT As String = "searchHot"
    Public Const SESSION_KEY_SERCHORDER As String = "searchOrder"
    Public Const SESSION_KEY_SERCHALLOC As String = "searchAlloc"
    Public Const SESSION_KEY_SERCHDEPO As String = "searchDepo"
    Public Const SESSION_KEY_SERCHDELI As String = "searchDeli"
    Public Const SESSION_KEY_SERCHGIVEUP As String = "searchGiveup"

    Public Const SESSION_KEY_SORTORDER As String = "sortOder"
    Public Const SESSION_KEY_SORTTYPE As String = "sortType"
    ' 2014/02/17 TCS 山田 受注後フォロー機能開発 START
    Public Const SESSION_KEY_TODOSEARCHTYPE As String = "toDoSearchType"
    Public Const SESSION_KEY_TODOSEARCHTEXT As String = "toDoSearchText"
    Public Const SESSION_KEY_SERCHALLBEFORE As String = "searchAllBefore"
    Public Const SESSION_KEY_SERCHALLAFTER As String = "searchAllAfter"
    ' 2014/02/17 TCS 山田 受注後フォロー機能開発 END
    '''次画面遷移情報
    ''' <summary>顧客種別</summary>
    Private Const SESSION_KEY_CSTKIND As String = "SearchKey.CSTKIND"
    ''' <summary>顧客分類</summary>
    Private Const SESSION_KEY_CUSTOMERCLASS As String = "SearchKey.CUSTOMERCLASS"
    ''' <summary>活動先顧客コード</summary>
    Private Const SESSION_KEY_CRCUSTID As String = "SearchKey.CRCUSTID"
    ''' <summary>FBOX SEQNO</summary>
    Private Const SESSION_KEY_FOLLOW_UP_BOX As String = "SearchKey.FOLLOW_UP_BOX"
    ''' <summary>Follow-upBoxの店舗コード</summary>
    Private Const SESSION_KEY_FLLWUPBOX_STRCD As String = "SearchKey.FLLWUPBOX_STRCD"


    ''' <summary>ポートレート画像登録なし時のアイコン</summary>
    Private Const NO_IMAGE_ICON As String = "../Styles/Images/Nnsc05-01Portraits01.png"


    Public Const ZEROINITIAL As Integer = 0

    ' 2014/02/17 TCS 山田 受注後フォロー機能開発 START
    ''' <summary>敬称前後取得用 </summary>
    Private Const CONTENT_KEISYO_ZENGO As String = "KEISYO_ZENGO"
    ' 2014/02/17 TCS 山田 受注後フォロー機能開発 END

    ''' <summary>
    ''' ステイタスの内部コード
    ''' </summary>
    ''' <remarks></remarks>
    Public Const CDHOTT As Integer = 100
    Public Const CDWARM As Integer = 200
    Public Const CDCOLD As Integer = 300
    Public Const CDJUCH As Integer = 400
    Public Const CDHURI As Integer = 410
    Public Const CDNYUK As Integer = 420
    Public Const CDNOUS As Integer = 430
    Public Const CDGVUP As Integer = 500

    ''' <summary>
    ''' ステイタスアイコンパス
    ''' </summary>
    ''' <remarks></remarks>
    Private Const ICOCOLD As String = "../Styles/Images/SC3010401/nsc05StatusIconStar3.png"     'Cold
    Private Const ICOWARM As String = "../Styles/Images/SC3010401/nsc05StatusIconStar2.png"     'Warm
    Private Const ICOHOTT As String = "../Styles/Images/SC3010401/nsc05StatusIconStar1.png"     'Hot
    Private Const ICOJUCH As String = "../Styles/Images/SC3010401/nsc05StatusIcon04.png"        '受注
    Private Const ICOHURI As String = "../Styles/Images/SC3010401/nsc05StatusIcon05.png"        '振当
    Private Const ICONYUK As String = "../Styles/Images/SC3010401/nsc05StatusIcon07.png"        '入金
    Private Const ICONOUS As String = "../Styles/Images/SC3010401/nsc05StatusIcon06.png"        '納車
    Private Const ICOGVUP As String = "../Styles/Images/SC3010401/nsc05StatusIcon03.png"        '断念

    ''' <summary>
    ''' 客区分
    ''' </summary>
    ''' <remarks></remarks>
    Private Const HOUJIN As String = "0"
    Private Const KOJIN As String = "1"
    Private Const NEWCUST As String = "2"

    Private Const PAST As String = "1"
    Private Const NOTPAST As String = "0"

    Private Const nmCDKujo As Integer = 10
    Private Const nmCDOrgCust As Integer = 11
    Private Const nmCDNewCust As Integer = 12
    Private Const nmCDCorp As Integer = 13
    Private Const nmCDPrv As Integer = 14

    ''' <summary>
    ''' 1ページあたりの表示件数
    ''' </summary>
    ''' <remarks></remarks>
    Public Const PAGEMAXLINE As Integer = 50

    ''' <summary>
    ''' 検索結果が0件です。メッセージＩＤ
    ''' </summary>
    ''' <remarks></remarks>
    Public Const ID_ZERO_MESSAGE As Integer = 17

    ''' <summary>
    ''' 次の{0}件を読み込むメッセージＩＤ
    ''' </summary>
    ''' <remarks></remarks>
    Public Const ID_NEXTLINE_MESSAGE As Integer = 15

    ''' <summary>
    ''' 前の{0}件を読み込むメッセージＩＤ
    ''' </summary>
    ''' <remarks></remarks>
    Public Const ID_BEFORM_MESSAGE As Integer = 16

    ' フッター　(メインメニューへ)
    Private Const MAIN_MENU As Integer = 100
    ' フッター　(顧客詳細へ)
    Private Const CUSTOMER_SEARCH As Integer = 200
    ' フッター （ショールーム）
    Private Const SHOW_ROOM As Integer = 1200

    'ソートタイプ－車名
    Private Const SORT_TYPE_CARNAME As String = "1"
    'ソートタイプ－ステータス
    Private Const SORT_TYPE_STATUS As String = "2"
    'ソートタイプ－次回活動日
    Private Const SORT_TYPE_CRACT As String = "3"

    'ソートオーダー－ASC
    Private Const SORT_ORDER_ASC As String = "1"
    'ソートオーダー－DESC
    Private Const SORT_ORDER_DESC As String = "2"

    Private Const COND_ON As Integer = 1
    Private Const COND_OFF As Integer = 0

    Private Const NOTIME_FLG As String = "0"
    Private Const ALLDAY_FLG As String = "1"

    ' 2014/02/17 TCS 山田 受注後フォロー機能開発 START
    '検索条件テキストボックス(初期値：ブランク)
    Private Const SEARCHTEXT_BLANK As String = ""

    '受注後工程チェックボックスアイコンパス用2NDキー
    Private Const CHECKBOX_AFTER_ODR_PROC As String = "30"
    ' 2014/02/17 TCS 山田 受注後フォロー機能開発 END

    '2013/01/11 TCS 橋本 【A.STEP2】Add Start
    Private Const BACKGROUNDCOLOR_KANRYOU As String = "ColorGrayOut"
    Private Const BACKGROUNDCOLOR_KISUU As String = "ColorGray"
    Private Const BACKGROUNDCOLOR_GUUSUU As String = "ColorWhite"
    '2013/01/11 TCS 橋本 【A.STEP2】Add End

#End Region


    Protected Sub Page_Load(sender As Object, e As System.EventArgs) Handles Me.Load


        ' 2014/02/17 TCS 山田 受注後フォロー機能開発 START
        '顧客検索条件ラジオボタンの制御
        InitToDoSearchTypeButtonEvent()

        '受注後工程絞り込み条件チェックボックスの制御
        '2015/12/08 TCS 中村 MOD (ﾄﾗｲ店ｼｽﾃﾑ評価)新車ﾀﾌﾞﾚｯﾄ(受注後)の管理機能開発 START
        If (SC3010401BusinessLogic.GetAfterOdrProcFlg(StaffContext.Current.DlrCD, StaffContext.Current.BrnCD).Equals("1")) Then
            Me.b2dPanel.Enabled = True
            Me.b2dPanel.Visible = True
            InitAfterOdrPrcsButtonEvent()
        Else
            Me.b2dPanel.Enabled = False
            Me.b2dPanel.Visible = False
        End If
        '2015/12/08 TCS 中村 MOD (ﾄﾗｲ店ｼｽﾃﾑ評価)新車ﾀﾌﾞﾚｯﾄ(受注後)の管理機能開発 END
        ' 2014/02/17 TCS 山田 受注後フォロー機能開発 END

        If Not Page.IsPostBack Then

            ' 2014/02/17 TCS 山田 受注後フォロー機能開発 START
            '顧客検索用文言
            Me.ToDoSearchTypeWordTelHidden.Value = WebWordUtility.GetWord(20)
            Me.ToDoSearchTypeWordNameHidden.Value = WebWordUtility.GetWord(21)
            Me.ToDoSearchTypeWordVinHidden.Value = WebWordUtility.GetWord(22)
            Me.ToDoSearchTypeWordBookingNoHidden.Value = WebWordUtility.GetWord(23)
            Me.ToDoSearchTypeWordSocialIDHidden.Value = WebWordUtility.GetWord(24)
            ' 2014/02/17 TCS 山田 受注後フォロー機能開発 END

            Logger.Info("Page_Load Not Page.IsPostBack Start")

            '次の{0}件を読み込む
            Dim massageStr As New StringBuilder(1000)
            massageStr.AppendFormat(WebWordUtility.GetWord(ID_NEXTLINE_MESSAGE), PAGEMAXLINE)
            customerRepeater.ForwardPagerLabel = massageStr.ToString

            'Me.nextMessageHidden.Value = massageStr.ToString
            'Me.nextLastMessageHidden.Value = massageStr.ToString

            '前の{0}件を読み込む
            Dim massageStr2 As New StringBuilder(1000)
            massageStr2.AppendFormat(WebWordUtility.GetWord(ID_BEFORM_MESSAGE), PAGEMAXLINE)
            customerRepeater.RewindPagerLabel = massageStr2.ToString

            'Me.forwordMessageHidden.Value = massageStr2.ToString
            'Me.forwordFirstMessageHidden.Value = massageStr2.ToString

            'ソート方向 (1:昇順)
            Me.sortOrderHidden.Value = SORT_ORDER_ASC
            'ソート項目(1:車両名,2:ステイタス,3:次回活動日)
            Me.sortTypeHidden.Value = SORT_TYPE_CRACT

            '絞り込み条件パラメータ
            ' 2014/02/17 TCS 山田 受注後フォロー機能開発 START
            If (Me.ContainsKey(ScreenPos.Current, SESSION_KEY_TODOSEARCHTYPE) = False) Then
                Me.SetValue(ScreenPos.Current, SESSION_KEY_TODOSEARCHTYPE, ToDoSegmentedButton.SelectedValue)      '検索条件ラジオボタン初期先頭項目
            End If
            If (Me.ContainsKey(ScreenPos.Current, SESSION_KEY_TODOSEARCHTEXT) = False) Then
                Me.SetValue(ScreenPos.Current, SESSION_KEY_TODOSEARCHTEXT, SEARCHTEXT_BLANK)      '検索条件テキストボックス：初期ブランク
            End If
            ' 2014/02/17 TCS 山田 受注後フォロー機能開発 END
            If (Me.ContainsKey(ScreenPos.Current, SESSION_KEY_SERCHDELAY) = False) Then
                Me.SetValue(ScreenPos.Current, SESSION_KEY_SERCHDELAY, COND_ON)      '遅れ：初期１
            End If
            If (Me.ContainsKey(ScreenPos.Current, SESSION_KEY_SERCHTODAY) = False) Then
                Me.SetValue(ScreenPos.Current, SESSION_KEY_SERCHTODAY, COND_ON)      '今日：初期１
            End If
            If (Me.ContainsKey(ScreenPos.Current, SESSION_KEY_SERCHFUTURE) = False) Then
                Me.SetValue(ScreenPos.Current, SESSION_KEY_SERCHFUTURE, COND_OFF)      '未来：初期０
            End If
            ' 2014/02/17 TCS 山田 受注後フォロー機能開発 START
            If (Me.ContainsKey(ScreenPos.Current, SESSION_KEY_SERCHALLBEFORE) = False) Then
                Me.SetValue(ScreenPos.Current, SESSION_KEY_SERCHALLBEFORE, COND_ON)      '受注前一括：初期１
            End If
            If (Me.ContainsKey(ScreenPos.Current, SESSION_KEY_SERCHALLAFTER) = False) Then
                Me.SetValue(ScreenPos.Current, SESSION_KEY_SERCHALLAFTER, COND_ON)      '受注後一括：初期１
            End If
            ' 2014/02/17 TCS 山田 受注後フォロー機能開発 END
            If (Me.ContainsKey(ScreenPos.Current, SESSION_KEY_SERCHCOLD) = False) Then
                Me.SetValue(ScreenPos.Current, SESSION_KEY_SERCHCOLD, COND_ON)      'COLD：初期１
            End If
            If (Me.ContainsKey(ScreenPos.Current, SESSION_KEY_SERCHWARM) = False) Then
                Me.SetValue(ScreenPos.Current, SESSION_KEY_SERCHWARM, COND_ON)      'WARM：初期１
            End If
            If (Me.ContainsKey(ScreenPos.Current, SESSION_KEY_SERCHHOT) = False) Then
                Me.SetValue(ScreenPos.Current, SESSION_KEY_SERCHHOT, COND_ON)      'HOT：初期１
            End If
            ' 2014/02/17 TCS 山田 受注後フォロー機能開発 START
            For i = 0 To Me.customCheckBoxRepeater.Items.Count - 1
                If (Me.ContainsKey(ScreenPos.Current, "searchAfter" & i) = False) Then
                    Me.SetValue(ScreenPos.Current, "searchAfter" & i, COND_ON)      '受注後工程：初期１
                End If
            Next
            ' 2014/02/17 TCS 山田 受注後フォロー機能開発 END
            If (Me.ContainsKey(ScreenPos.Current, SESSION_KEY_SORTORDER) = False) Then
                Me.SetValue(ScreenPos.Current, SESSION_KEY_SORTORDER, SORT_ORDER_ASC)
            End If
            If (Me.ContainsKey(ScreenPos.Current, SESSION_KEY_SORTTYPE) = False) Then
                Me.SetValue(ScreenPos.Current, SESSION_KEY_SORTTYPE, SORT_TYPE_CRACT)
            End If

            If DirectCast(GetValue(ScreenPos.Current, SESSION_KEY_SERCHDELAY, False), Integer) = COND_ON Then
                Me.checkDelay.Checked = True
            Else
                Me.checkDelay.Checked = False
            End If

            If DirectCast(GetValue(ScreenPos.Current, SESSION_KEY_SERCHTODAY, False), Integer) = COND_ON Then
                Me.checkDue.Checked = True
            Else
                Me.checkDue.Checked = False
            End If

            If DirectCast(GetValue(ScreenPos.Current, SESSION_KEY_SERCHFUTURE, False), Integer) = COND_ON Then
                Me.checkFuture.Checked = True
            Else
                Me.checkFuture.Checked = False
            End If

            ' 2014/02/17 TCS 山田 受注後フォロー機能開発 START
            If DirectCast(GetValue(ScreenPos.Current, SESSION_KEY_SERCHALLBEFORE, False), Integer) = COND_ON Then
                Me.CheckAllBefore.Checked = True
            Else
                Me.CheckAllBefore.Checked = False
            End If

            If DirectCast(GetValue(ScreenPos.Current, SESSION_KEY_SERCHALLAFTER, False), Integer) = COND_ON Then
                Me.CheckAllAfter.Checked = True
            Else
                Me.CheckAllAfter.Checked = False
            End If
            ' 2014/02/17 TCS 山田 受注後フォロー機能開発 END

            If DirectCast(GetValue(ScreenPos.Current, SESSION_KEY_SERCHCOLD, False), Integer) = COND_ON Then
                Me.checkCold.Checked = True
            Else
                Me.checkCold.Checked = False
            End If

            If DirectCast(GetValue(ScreenPos.Current, SESSION_KEY_SERCHWARM, False), Integer) = COND_ON Then
                Me.checkWarm.Checked = True
            Else
                Me.checkWarm.Checked = False
            End If

            If DirectCast(GetValue(ScreenPos.Current, SESSION_KEY_SERCHHOT, False), Integer) = COND_ON Then
                Me.checkHot.Checked = True
            Else
                Me.checkHot.Checked = False
            End If

            ' 2014/02/17 TCS 山田 受注後フォロー機能開発 START
            For i = 0 To Me.customCheckBoxRepeater.Items.Count - 1
                If DirectCast(GetValue(ScreenPos.Current, "searchAfter" & i, False), Integer) = COND_ON Then
                    DirectCast(Me.customCheckBoxRepeater.Items(i).FindControl("checkAfter"), HtmlInputCheckBox).Checked = True
                Else
                    DirectCast(Me.customCheckBoxRepeater.Items(i).FindControl("checkAfter"), HtmlInputCheckBox).Checked = False
                End If
            Next
            ' 2014/02/17 TCS 山田 受注後フォロー機能開発 END
            Logger.Info("Page_Load Not Page.IsPostBack End")
        End If

        'フッターの制御
        InitFooterEvent()


    End Sub

    ''' <summary>
    ''' CALDAV(XML String)取得
    ''' </summary>
    ''' <param name="flgFuture"></param>
    ''' <returns></returns>
    ''' <remarks>flgFuture Today:"0" , Future:"1"</remarks>
    Protected Function GetSchedule(flgFuture As Integer) As String

        Logger.Info("GetSchedule Start")

        Dim TestCALDAV = New ClassLibraryBusinessLogicTest()
        Dim service As New ClassLibraryBusinessLogic

        Dim context As StaffContext = StaffContext.Current      'スタッフ情報
        Dim nowDate As Date = DateTimeFunc.Now(context.DlrCD)   '現在日時
        'Dim truncNow As Date = New Date(nowDate.Year, nowDate.Month, nowDate.Day)
        '開始日には本日の00:00:00
        Dim stDate As New Date(nowDate.Year, nowDate.Month, nowDate.Day, 0, 0, 0)
        '終了日には本日の23:59:59 or 1000年後の23:59:59
        Dim edToday As New Date(nowDate.Year, nowDate.Month, nowDate.Day, 23, 59, 59)
        Dim edFuture As New Date(nowDate.Year + 1000, nowDate.Month, nowDate.Day, 23, 59, 59)

        Dim edDate As Date
        If flgFuture = 1 Then
            edDate = edFuture
        Else
            edDate = edToday
        End If

        'スケジュール取得(取得開始日時/取得終了日時/取得スタッフCD/取得スタッフ権限区分)
        Return service.GetCalender(stDate, _
           edDate, _
           context.Account, _
           CType(context.OpeCD, String))
        '------------------------------------------tet module str
        'Return TestCALDAV.GetCalender(stDate, _
        '   edDate, _
        '   context.Account, _
        '   CType(context.OpeCD, String))
        '------------------------------------------tet module end

        Logger.Info("GetSchedule End")

    End Function

    ''' <summary>
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    ''' <para>作成： 2012/02/01 TCS 竹内</para>
    ''' <para>更新： 2012/03/13 TCS 渡邊 $01 SalesStep2ユーザーテスト課題No.15、18、36</para>
    Protected Sub customerRepeater_ClientCallback(sender As Object, e As Toyota.eCRB.SystemFrameworks.Web.Controls.ClientCallbackEventArgs) Handles customerRepeater.ClientCallback

        Logger.Info("customerRepeater_ClientCallback Start")
        Dim sw As New System.Diagnostics.Stopwatch
        sw.Start()

        Dim beginRowIndex As Integer = 0
        Dim sortType As String
        Dim sortOrder As String
        Dim pageRows As Integer = CType(Me.customerRepeater.PageRows, Integer)
        Dim isLastRowInPage As Boolean = False

        Dim bizLogic As New SC3010401BusinessLogic

        If (Integer.TryParse(CType(e.Arguments("beginRowIndex"), String), beginRowIndex)) Then

            If e.Arguments("criteria").ToString = "" Then
                '初期表示の場合
                'NONE
            Else
                'コールバック
                Dim criteria As Dictionary(Of String, Object) = DirectCast(e.Arguments("criteria"), Dictionary(Of String, Object))

                ' 2014/02/17 TCS 山田 受注後フォロー機能開発 START
                Me.SetValue(ScreenPos.Current, SESSION_KEY_TODOSEARCHTYPE, CType(criteria("toDoSearchType"), String))
                Me.SetValue(ScreenPos.Current, SESSION_KEY_TODOSEARCHTEXT, CType(criteria("toDoSearchText"), String))
                ' 2014/02/17 TCS 山田 受注後フォロー機能開発 END
                Me.SetValue(ScreenPos.Current, SESSION_KEY_SERCHDELAY, IIf(CType(criteria("isCheckDelay"), Boolean), 1, 0))
                Me.SetValue(ScreenPos.Current, SESSION_KEY_SERCHTODAY, IIf(CType(criteria("isCheckDue"), Boolean), 1, 0))
                Me.SetValue(ScreenPos.Current, SESSION_KEY_SERCHFUTURE, IIf(CType(criteria("isCheckFuture"), Boolean), 1, 0))
                Me.SetValue(ScreenPos.Current, SESSION_KEY_SERCHCOLD, IIf(CType(criteria("isCheckCold"), Boolean), 1, 0))
                Me.SetValue(ScreenPos.Current, SESSION_KEY_SERCHWARM, IIf(CType(criteria("isCheckWarm"), Boolean), 1, 0))
                Me.SetValue(ScreenPos.Current, SESSION_KEY_SERCHHOT, IIf(CType(criteria("isCheckHot"), Boolean), 1, 0))
                ' 2014/02/17 TCS 山田 受注後フォロー機能開発 START
                Dim isCheckAfter As ArrayList = CType(criteria("isCheckAfter"), ArrayList)
                For i = 0 To Me.customCheckBoxRepeater.Items.Count - 1
                    Me.SetValue(ScreenPos.Current, "searchAfter" & i, IIf(CType(isCheckAfter.Item(i), Boolean), 1, 0))
                Next
                ' 2014/02/17 TCS 山田 受注後フォロー機能開発 END

                Me.SetValue(ScreenPos.Current, SESSION_KEY_SORTTYPE, CType(criteria("sortType"), String))
                Me.SetValue(ScreenPos.Current, SESSION_KEY_SORTORDER, CType(criteria("sortOrder"), String))

            End If


            Dim scrnrows As New StringBuilder(1000)
            Dim firstElement As Boolean = True
            Dim msgID As Integer = 0
            Dim context As StaffContext = StaffContext.Current      'スタッフ情報

            'セッション情報取得
            ' 2014/02/17 TCS 山田 受注後フォロー機能開発 START
            Dim toDoSearchType As String
            toDoSearchType = DirectCast(GetValue(ScreenPos.Current, SESSION_KEY_TODOSEARCHTYPE, False), String)
            Dim toDoSearchText As String
            toDoSearchText = DirectCast(GetValue(ScreenPos.Current, SESSION_KEY_TODOSEARCHTEXT, False), String)
            ' 2014/02/17 TCS 山田 受注後フォロー機能開発 END
            Dim serchDelay As Integer = 0
            serchDelay = DirectCast(GetValue(ScreenPos.Current, SESSION_KEY_SERCHDELAY, False), Integer)
            Dim serchToday As Integer = 0
            serchToday = DirectCast(GetValue(ScreenPos.Current, SESSION_KEY_SERCHTODAY, False), Integer)
            Dim serchFuture As Integer = 0
            serchFuture = DirectCast(GetValue(ScreenPos.Current, SESSION_KEY_SERCHFUTURE, False), Integer)
            Dim serchCold As Integer = 0
            serchCold = DirectCast(GetValue(ScreenPos.Current, SESSION_KEY_SERCHCOLD, False), Integer)
            Dim serchWarm As Integer = 0
            serchWarm = DirectCast(GetValue(ScreenPos.Current, SESSION_KEY_SERCHWARM, False), Integer)
            Dim serchHot As Integer = 0
            serchHot = DirectCast(GetValue(ScreenPos.Current, SESSION_KEY_SERCHHOT, False), Integer)
            ' 2014/02/17 TCS 山田 受注後フォロー機能開発 START DEL
            ' 2014/02/17 TCS 山田 受注後フォロー機能開発 END
            sortType = DirectCast(GetValue(ScreenPos.Current, SESSION_KEY_SORTTYPE, False), String)
            sortOrder = DirectCast(GetValue(ScreenPos.Current, SESSION_KEY_SORTORDER, False), String)

            Dim CalText As String
            '未来フラグの有無：CALDAVの未来データの取得可否
            CalText = GetSchedule(serchFuture)

            Dim ds As SC3010401DataSet = bizLogic.CreateDataSet

            'CALDAVからのXMLStringより検索テーブルを作成
            bizLogic.CreateSearch(CalText, ds, serchDelay, serchToday, serchFuture)

            bizLogic.SetListData(ds)

            '画面の項目タイトルタップより、表示順を変更
            'Dim strCondition As String
            Dim strSort As New StringBuilder

            '$01 Modify Start
            'Select Case sortType                     'ソート項目
            '    Case SORT_TYPE_CARNAME
            '        strSort.Append("SERIESNM ")                     '1:車両名称でソート
            '    Case SORT_TYPE_STATUS
            '        strSort.Append("CRRESULTSORT ")                 '2:ステイタスでソート
            '    Case Else
            '        strSort.Append("CONTACTDATE ")                  '3:次回活動日でソート
            'End Select
            'If SORT_ORDER_DESC.Equals(sortOrder) Then                  'ソート方向
            '    strSort.Append(" DESC")                             '2:降順
            'Else
            '    strSort.Append(" ASC")                              '1:昇順
            'End If

            Select Case sortType                                    'ソート項目
                Case SORT_TYPE_CARNAME
                    strSort.Append("SERIESNM ")                     '1:車両名称でソート
                    If SORT_ORDER_DESC.Equals(sortOrder) Then       'ソート方向
                        strSort.Append(" DESC")                     '2:降順
                    Else
                        strSort.Append(" ASC")                      '1:昇順
                    End If
                    'デフォルトソート表示順
                    strSort.Append(" ,CONTACTDATE ASC ")            '次回活動日でソート
                    strSort.Append(" ,CRRESULTSORT ASC ")           'ステイタスでソート
                    strSort.Append(" ,TODONAME ASC ")               'ToDoタイトル
                Case SORT_TYPE_STATUS
                    strSort.Append("CRRESULTSORT ")                 '2:ステイタスでソート
                    If SORT_ORDER_DESC.Equals(sortOrder) Then       'ソート方向
                        strSort.Append(" DESC")                     '2:降順
                    Else
                        strSort.Append(" ASC")                      '1:昇順
                    End If
                    'デフォルトソート表示順
                    strSort.Append(" ,CONTACTDATE ASC ")            '次回活動日でソート
                    strSort.Append(" ,SERIESNM ASC ")               '車両名称でソート
                    strSort.Append(" ,TODONAME ASC ")               'ToDoタイトル
                Case Else
                    strSort.Append("CONTACTDATE ")                  '3:次回活動日でソート
                    If SORT_ORDER_DESC.Equals(sortOrder) Then       'ソート方向
                        strSort.Append(" DESC")                     '2:降順
                    Else
                        strSort.Append(" ASC")                      '1:昇順
                    End If
                    'デフォルトソート表示順
                    strSort.Append(" ,CRRESULTSORT ASC ")           'ステイタスでソート
                    strSort.Append(" ,SERIESNM ASC ")               '車両名称でソート
                    strSort.Append(" ,TODONAME ASC ")               'ToDoタイトル
            End Select
            '$01 Modify End

            '絞り込み条件作成
            'CRRESULTSORT(100,200,300,400,410,420,430,500)
            Dim strWhere As New StringBuilder
            Dim condFlg As Integer = 0
            strWhere.Append("CRRESULTSORT in(")
            If serchHot = 1 Then
                strWhere.Append(" 100 ")
                condFlg = 1
            End If
            If serchWarm = 1 Then
                If condFlg = 1 Then
                    strWhere.Append(",")
                End If
                strWhere.Append(" 200 ")
                condFlg = 1
            End If

            If serchCold = 1 Then
                If condFlg = 1 Then
                    strWhere.Append(",")
                End If
                strWhere.Append(" 300 ")
                condFlg = 1
            End If

            ' 2014/02/17 TCS 山田 受注後フォロー機能開発 START
            '絞り込み条件作成(受注後工程チェックボックス)
            For i = 0 To Me.customCheckBoxRepeater.Items.Count - 1
                If DirectCast(GetValue(ScreenPos.Current, "searchAfter" & i, False), Integer) = 1 Then
                    If condFlg = 1 Then
                        strWhere.Append(",")
                    End If
                    strWhere.Append(" " & 400 + i + 1 & " ")
                    condFlg = 1
                End If
            Next

            strWhere.Append(")")

            'いずれの絞り込み条件チェックボックスも選択されていない場合
            Dim statusNothingFlg As Integer = 0
            If condFlg = 0 Then
                '絞り込み条件文字列を初期化する
                strWhere.Length = 0
                statusNothingFlg = 1
            End If

            '顧客情報検索による絞り込み条件作成
            Dim customerList As SC3010401DataSet.SC3010401GetCustomerListDataTable
            Dim searchDirection As Integer = 0
            Dim customerListNothingFlg As Integer = 0
            '検索文字列が入力されている、かつ、いずれかの絞り込み条件チェックボックスが選択されている場合のみ処理する
            If Not toDoSearchText.Equals(String.Empty) And statusNothingFlg = 0 Then
                searchDirection = SC3010401DataTableTableAdapter.IdSearchDirectionAfter
                '最初１文字が*ならば、前方後方一致とする
                If toDoSearchText.Substring(0, 1).Equals("*") Then
                    searchDirection = SC3010401DataTableTableAdapter.IdSearchDirectionAll
                    toDoSearchText = toDoSearchText.Substring(1)
                End If

                '検索項目によって分岐
                If toDoSearchType = SC3010401DataTableTableAdapter.IdSearchBookingNo Then
                    '注文番号での検索
                    If condFlg = 1 Then
                        strWhere.Append(" AND ")
                    End If

                    If searchDirection = SC3010401DataTableTableAdapter.IdSearchDirectionAfter Then
                        strWhere.Append(" BOOKINGNO like '" & toDoSearchText & "%' ")
                    Else
                        strWhere.Append(" BOOKINGNO like '%" & toDoSearchText & "%' ")
                    End If

                    condFlg = 1

                Else
                    '顧客一覧作成(顧客名称、電話番号/携帯番号、国民ID、VINでの検索)
                    If (toDoSearchType = SC3010401DataTableTableAdapter.IdSearchTel) Then
                        '電話番号時は、ハイフンを取り除く
                        toDoSearchText = toDoSearchText.Replace("-", "")             '検索文字列
                    Else
                        '電話番号以外（名称・国民ID・VIN）で検索時
                        toDoSearchText = toDoSearchText.ToUpper                      '検索文字列 (大文字に変換する)
                    End If

                    customerList = bizLogic.GetCustomerList(searchDirection, toDoSearchText, toDoSearchType)

                    If customerList.Rows.Count > 0 Then
                        If condFlg = 1 Then
                            strWhere.Append(" AND ")
                        End If

                        '絞り込み条件作成(CRCUSTIDが、作成した顧客一覧の顧客IDと同一のレコードを抽出)
                        Dim cstIdCondFlg As Integer = 0
                        strWhere.Append(" CRCUSTID in(")

                        For Each customerListRow As SC3010401DataSet.SC3010401GetCustomerListRow In customerList
                            If cstIdCondFlg = 1 Then
                                strWhere.Append(",")
                            End If
                            strWhere.Append(" " & customerListRow.CRCUSTID & " ")
                            cstIdCondFlg = 1
                        Next

                        strWhere.Append(")")
                    Else
                        customerListNothingFlg = 1
                    End If
                End If
            End If

            Dim goukeiStr As New StringBuilder(1000)
            Dim count As Integer = 0

            'いずれの絞り込み条件チェックボックスも選択されていない場合、または、顧客一覧作成結果が0件の場合は、検索結果0件で画面表示
            If statusNothingFlg = 0 And customerListNothingFlg = 0 Then
                '抽出条件、ソート条件の反映(全件TBL → 反映後TBL へコピー)
                bizLogic.SetListFilter(ds, strWhere.ToString, strSort.ToString)

                count = ds.SC3010401ListFilter.Rows.Count
                ''合計件数を出力
                goukeiStr.AppendFormat(WebWordUtility.GetWord(2), count)

            End If
            ' 2014/02/17 TCS 山田 受注後フォロー機能開発 END

            If (count > 0) Then
                Me.resultListPanel.Visible = True

                '次の{0}件を読み込む
                Dim massageStr As New StringBuilder(1000)
                massageStr.AppendFormat(WebWordUtility.GetWord(ID_NEXTLINE_MESSAGE), PAGEMAXLINE)
                'customerRepeater.ForwardPagerLabel = massageStr.ToString

                Me.nextMessageHidden.Value = massageStr.ToString
                Me.nextLastMessageHidden.Value = massageStr.ToString

                '前の{0}件を読み込む
                Dim massageStr2 As New StringBuilder(1000)
                massageStr2.AppendFormat(WebWordUtility.GetWord(ID_BEFORM_MESSAGE), PAGEMAXLINE)
                'customerRepeater.RewindPagerLabel = massageStr2.ToString

                Me.forwordMessageHidden.Value = massageStr2.ToString
                Me.forwordFirstMessageHidden.Value = massageStr2.ToString


                '顔写真の保存先フォルダ(Web向け)取得
                Dim imagePath As String = bizLogic.GetImagePath()

                Dim todoList As SC3010401DataSet.SC3010401ListFilterDataTable = ds.SC3010401ListFilter
                Dim wRow As SC3010401DataSet.SC3010401ListFilterRow
                'Dim DataCount As Integer = todoList.Rows.Count

                Dim nowDatetime As Date = DateTimeFunc.Now(context.DlrCD)   '現在日時
                Dim truncNow As Date = nowDatetime.Date

                Dim word_Kujo As String = WebWordUtility.GetWord(nmCDKujo)
                Dim word_newCustomer As String = WebWordUtility.GetWord(nmCDNewCust)
                Dim word_orgCustomer As String = WebWordUtility.GetWord(nmCDOrgCust)
                Dim word_CustomerType_Prv As String = WebWordUtility.GetWord(nmCDPrv)
                Dim word_CustomerType_Corp As String = WebWordUtility.GetWord(nmCDCorp)

                ' 2014/02/17 TCS 山田 受注後フォロー機能開発 START
                '敬称位置取得
                Dim sysEnv As New SystemEnvSetting
                Dim sysEnvRow As SystemEnvSettingDataSet.SYSTEMENVSETTINGRow
                sysEnvRow = sysEnv.GetSystemEnvSetting(CONTENT_KEISYO_ZENGO)
                ' 2014/02/17 TCS 山田 受注後フォロー機能開発 END

                ''顧客一覧取得
                For i As Integer = beginRowIndex To todoList.Rows.Count - 1

                    wRow = todoList.Item(i)

                    Dim updateFlg As Integer = 1

                    If (firstElement) Then
                        firstElement = False
                    Else
                        scrnrows.Append(",")
                    End If

                    '1.顔パス
                    Dim imgpath1 As String
                    ' 2013/06/30 TCS 武田 2013/10対応版　既存流用 START
                    If (String.IsNullOrEmpty(wRow.IMAGEFILE_S) = True) Or
                        (wRow.IMAGEFILE_S.Equals(" ")) Then
                        ' 2013/06/30 TCS 武田 2013/10対応版　既存流用 END
                        imgpath1 = NO_IMAGE_ICON
                    Else
                        imgpath1 = imagePath & wRow.IMAGEFILE_S
                    End If
                    imgpath1 = Me.ResolveClientUrl(imgpath1)

                    ' 2014/02/17 TCS 山田 受注後フォロー機能開発 START
                    '2.ＴｏＤｏ名称、予定接触方法（受注後活動名称）
                    Dim todoname As String = String.Empty
                    Dim contactName As String = String.Empty
                    'Dim todonameArray() As String = wRow.TODONAME.Trim.Split(" "c)

                    ''分割されたTODONAMEから、ＴｏＤｏ名称、予定接触方法（受注後活動名称）を設定する
                    'For j As Integer = beginRowIndex To todonameArray.Count - 1

                    '    If j = 0 Then
                    '        todoname = todonameArray(j)
                    '    ElseIf j = 1 Then
                    '        contactName = todonameArray(j)
                    '    Else
                    '        contactName = contactName + " " + todonameArray(j)
                    '    End If

                    'Next
                    If String.Equals(wRow.NAMETITLE, "") Then
                        todoname = wRow.CUSTOMERNAME
                    Else
                        '敬称位置設定
                        If sysEnvRow.PARAMVALUE.Equals("1") Then
                            todoname = wRow.NAMETITLE + Space(1) + wRow.CUSTOMERNAME
                        Else
                            todoname = wRow.CUSTOMERNAME + Space(1) + wRow.NAMETITLE
                        End If
                    End If

                    '受注後の場合、受注後活動名称をセットする
                    If (wRow.CRACTRESULT = "31") Then
                        contactName = wRow.CONTACTNAME + "(" + wRow.ACTODRNAME + ")"
                    Else
                        contactName = wRow.CONTACTNAME
                    End If

                    ' 2014/02/17 TCS 山田 受注後フォロー機能開発 END

                    '3.苦情
                    Dim kujo As String = String.Empty
                    If (wRow.CLMFLG.Equals("1")) Then
                        'kujo = WebWordUtility.GetWord(nmCDKujo)
                        kujo = word_Kujo
                    Else
                        kujo = ""
                    End If

                    '4.顧客種別名称 12.顧客種別,
                    Dim kindnm As String = String.Empty
                    Dim cstkind As String = String.Empty
                    If (wRow.CUSTSEGMENT.Equals(NEWCUST)) Then
                        cstkind = "2"
                        kindnm = word_newCustomer
                        'kindnm = WebWordUtility.GetWord(nmCDNewCust)
                    Else
                        cstkind = "1"
                        kindnm = word_orgCustomer
                        'kindnm = WebWordUtility.GetWord(nmCDOrgCust)
                    End If

                    ' 2018/08/27 TCS 佐々木 TKM Next Gen e-CRB Project Application development Block B-3 START
                    '5.車両名称
                    Dim seriesname As String = wRow.SERIESNM

                    '6.モデル名称
                    Dim modelname As String = wRow.VCLMODEL_NAME

                    '7.ステイタスアイコン
                    Dim imgpath2 As String
                    Select Case wRow.CRRESULTSORT
                        Case CDHOTT
                            imgpath2 = ICOHOTT
                        Case CDWARM
                            imgpath2 = ICOWARM
                        Case CDCOLD
                            imgpath2 = ICOCOLD
                            ' 2014/02/17 TCS 山田 受注後フォロー機能開発 START
                            '受注後工程の場合、DBから取得したアイコンパスを設定
                        Case Is >= CDJUCH
                            imgpath2 = wRow.AFTERODRICONPATH
                        Case Else
                            imgpath2 = ""
                            ' 2014/02/17 TCS 山田 受注後フォロー機能開発 END
                    End Select

                    '8.次回活動アイコン
                    Dim imgpath3 As String = wRow.CONTACTICONPATH

                    '9.次回活動日
                    Dim contactDate As String
                    If wRow.TIMEFLG.Equals(NOTIME_FLG) Or wRow.ALLDAYFLG.Equals(ALLDAY_FLG) Then
                        '時間指定なし
                        contactDate = DateTimeFunc.FormatElapsedDate(ElapsedDateFormat.Normal, wRow.CONTACTDATE, nowDatetime, context.DlrCD, False)
                    Else
                        '時間指定あり
                        contactDate = DateTimeFunc.FormatElapsedDate(ElapsedDateFormat.Normal, wRow.CONTACTDATE, nowDatetime, context.DlrCD)
                    End If

                    '10.過去フラグ
                    Dim pastFlg As String = NOTPAST
                    'If Date.Compare(truncNow, wRow.CONTACTDATE.Date) > 0 Then
                    If Date.Compare(nowDatetime, wRow.CONTACTDATE) > 0 Then
                        pastFlg = PAST
                    End If

                    '11.顧客分類
                    Dim customerclass As String = wRow.CUSTOMERCLASS

                    '12.活動先顧客コード
                    Dim crcustid As String = wRow.CRCUSTID

                    '13.FollowUpBox_SEQNO
                    Dim fllwupboxseqno As String = wRow.FLLWUPBOX_SEQNO.ToString

                    '14.fllwupbox_strcd
                    Dim fllwupboxstrcd As String = wRow.STRCD
                    ' 2018/08/27 TCS 佐々木 TKM Next Gen e-CRB Project Application development Block B-3 END

                    '2013/01/11 TCS 橋本 【A.STEP2】Mod Start
                    ''偶数／奇数行判定
                    'Dim flg As Integer
                    'flg = i Mod 2

                    '偶数/奇数/完了行によって背景色を変える
                    Dim backGroundColor As String

                    '完了フラグが1:完了
                    If wRow.COMPFLG.Equals("1") Then
                        '完了行
                        backGroundColor = BACKGROUNDCOLOR_KANRYOU
                    Else
                        '偶数／奇数行判定
                        If i Mod 2 = 0 Then
                            '偶数行
                            backGroundColor = BACKGROUNDCOLOR_GUUSUU
                        Else
                            '奇数行
                            backGroundColor = BACKGROUNDCOLOR_KISUU
                        End If
                    End If
                    '2013/01/11 TCS 橋本 【A.STEP2】Mod End

                    'シーケンシャル番号
                    Dim seqno As Long = ZEROINITIAL
                    If (wRow.IsTODOSEQNull() = True) Then
                        seqno = ZEROINITIAL
                    Else
                        seqno = wRow.TODOSEQ
                    End If

                    '2013/01/11 TCS 橋本 【A.STEP2】Mod Start
                    ''一覧内容作成
                    'scrnrows.AppendFormat("{{ ""NO"" : {0}, " &
                    '  """IMAGEPATH"" : ""{1}"", " &
                    '  """TODONAME"" : ""{2}""," &
                    '  """CLM"" : ""{3}""," &
                    '  """KINDNM"" : ""{4}""," &
                    '  """CUSTYPE"" : ""{5}""," &
                    '  """SERIESNM"" : ""{6}""," &
                    '  """MODELNM"" : ""{7}""," &
                    '  """STATUSICO"" : ""{8}""," &
                    '  """CONTACTICO"" : ""{9}""," &
                    '  """CONTACTDATE"" : ""{10}""," &
                    '  """PASTFLG"" : ""{11}""," &
                    '  """CSTKIND"" : ""{12}""," &
                    '  """CUSTOMERCLASS"" : ""{13}""," &
                    '  """CRCUSTID"" : ""{14}""," &
                    '  """FOLLOWUPBOX"" : ""{15}""," &
                    '  """FLLWUPBOXSTRCD"" : ""{16}""," &
                    '  """flg"" : {17}}}",
                    '(i + 1),
                    ' HttpUtility.JavaScriptStringEncode(imgpath1),
                    ' HttpUtility.JavaScriptStringEncode(SpaceToHeifun(todoname)),
                    ' HttpUtility.JavaScriptStringEncode(kujo),
                    ' HttpUtility.JavaScriptStringEncode(kindnm),
                    ' HttpUtility.JavaScriptStringEncode(custypenm),
                    ' HttpUtility.JavaScriptStringEncode(SpaceToHeifun(seriesname.Trim)),
                    ' HttpUtility.JavaScriptStringEncode(SpaceToHeifun(modelname.Trim)),
                    ' HttpUtility.JavaScriptStringEncode(imgpath2),
                    ' HttpUtility.JavaScriptStringEncode(imgpath3),
                    ' HttpUtility.JavaScriptStringEncode(contactDate),
                    ' HttpUtility.JavaScriptStringEncode(pastFlg),
                    ' HttpUtility.JavaScriptStringEncode(cstkind),
                    ' HttpUtility.JavaScriptStringEncode(customerclass),
                    ' HttpUtility.JavaScriptStringEncode(crcustid),
                    ' HttpUtility.JavaScriptStringEncode(fllwupboxseqno),
                    ' HttpUtility.JavaScriptStringEncode(fllwupboxstrcd),
                    '  flg)
                    '一覧内容作成
                    ' 2018/08/27 TCS 佐々木 TKM Next Gen e-CRB Project Application development Block B-3 START
                    ' 2014/02/17 TCS 山田 受注後フォロー機能開発 START
                    scrnrows.AppendFormat("{{ " &
                                          """NO"" : {0}, " &
                                          """IMAGEPATH"" : ""{1}"", " &
                                          """TODONAME"" : ""{2}""," &
                                          """CLM"" : ""{3}""," &
                                          """KINDNM"" : ""{4}""," &
                                          """CONTACTNAME"" : ""{5}""," &
                                          """SERIESNM"" : ""{6}""," &
                                          """MODELNM"" : ""{7}""," &
                                          """STATUSICO"" : ""{8}""," &
                                          """CONTACTICO"" : ""{9}""," &
                                          """CONTACTDATE"" : ""{10}""," &
                                          """PASTFLG"" : ""{11}""," &
                                          """CSTKIND"" : ""{12}""," &
                                          """CUSTOMERCLASS"" : ""{13}""," &
                                          """CRCUSTID"" : ""{14}""," &
                                          """FOLLOWUPBOX"" : ""{15}""," &
                                          """FLLWUPBOXSTRCD"" : ""{16}""," &
                                          """BACKGROUNDCOLOR"" : ""{17}""," &
                                          """joinType"" : ""{18}""" &
                                          "}}",
                                          (i + 1),
                                          HttpUtility.JavaScriptStringEncode(imgpath1),
                                          HttpUtility.JavaScriptStringEncode(SpaceToHeifun(todoname)),
                                          HttpUtility.JavaScriptStringEncode(kujo),
                                          HttpUtility.JavaScriptStringEncode(kindnm),
                                          HttpUtility.JavaScriptStringEncode(contactName),
                                          HttpUtility.JavaScriptStringEncode(SpaceToHeifun(seriesname.Trim)),
                                          HttpUtility.JavaScriptStringEncode(SpaceToHeifun(modelname.Trim)),
                                          HttpUtility.JavaScriptStringEncode(imgpath2),
                                          HttpUtility.JavaScriptStringEncode(imgpath3),
                                          HttpUtility.JavaScriptStringEncode(contactDate),
                                          HttpUtility.JavaScriptStringEncode(pastFlg),
                                          HttpUtility.JavaScriptStringEncode(cstkind),
                                          HttpUtility.JavaScriptStringEncode(customerclass),
                                          HttpUtility.JavaScriptStringEncode(crcustid),
                                          HttpUtility.JavaScriptStringEncode(fllwupboxseqno),
                                          HttpUtility.JavaScriptStringEncode(fllwupboxstrcd),
                                          HttpUtility.JavaScriptStringEncode(backGroundColor),
                                          HttpUtility.JavaScriptStringEncode(wRow.CSTJOINTYPE)
                                          )
                    '2013/01/11 TCS 橋本 【A.STEP2】Mod End
                    ' 2014/02/17 TCS 山田 受注後フォロー機能開発 END
                    ' 2018/08/27 TCS 佐々木 TKM Next Gen e-CRB Project Application development Block B-3 END

                Next

                e.Results("@rows") = "[" & scrnrows.ToString() & "]"
            Else
                e.Results("@rows") = "[]"

                Me.resultListPanel.Visible = False

            End If
            'データ件数
            e.Results("@totalCount") = count
            '合計表示
            '$01 Modify start
            'e.Results("@totalCountMessage") = String.Format(CultureInfo.InvariantCulture, """" & WebWordUtility.GetWord(2) & """", count)
            e.Results("@totalCountMessage") = """" & HttpUtility.JavaScriptStringEncode(String.Format(CultureInfo.InvariantCulture, WebWordUtility.GetWord(2), count)) & """"
            '$01 Modify End
            'メッセージ
            '$01 Modify start
            'e.Results("@message") = String.Format(CultureInfo.InvariantCulture, """" & WebWordUtility.GetWord(17) & """", count)
            e.Results("@message") = """" & HttpUtility.JavaScriptStringEncode(String.Format(CultureInfo.InvariantCulture, WebWordUtility.GetWord(17), count)) & """"
            '$01 Modify End
        Else
            e.Results("@rows") = "[]"
        End If
        sw.Stop()


        'Logger.Info("TODO一覧性能測定>>>>>>>>>>>" & sw.Elapsed.ToString)
        Logger.Info("customerRepeater_ClientCallback End")

    End Sub

    ''' <summary>
    ''' 空文字の場合にハイフンを返す
    ''' </summary>
    ''' <param name="val"></param>
    ''' <remarks></remarks>
    Private Function SpaceToHeifun(ByVal val As String) As String
        If (val.Length = 0) Then
            Return "-"
        Else
            Return val
        End If
    End Function

    ''' <summary>
    ''' フッターボタンの制御
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub InitFooterEvent()

        'メニュー
        Dim menuButton As CommonMasterFooterButton = CType(Me.Master, CommonMasterPage).GetFooterButton(MAIN_MENU)
        AddHandler menuButton.Click, _
         Sub()
             'メニューに遷移
             Me.RedirectNextScreen("SC3010203")
         End Sub

        '顧客詳細
        Dim custSearchButton As CommonMasterFooterButton = CType(Me.Master, CommonMasterPage).GetFooterButton(CUSTOMER_SEARCH)
        AddHandler custSearchButton.Click, _
         Sub()
             '顧客詳細に遷移
             Me.RedirectNextScreen("SC3080201")
         End Sub

        'TCSとの連携ボタン
        Dim tcvButton As CommonMasterFooterButton = CType(Me.Master, CommonMasterPage).GetFooterButton(FooterMenuCategory.TCV)
        AddHandler tcvButton.Click, AddressOf tcvButton_Click

        'ショールームステイタス
        Dim ssvButton As CommonMasterFooterButton = CType(Me.Master, CommonMasterPage).GetFooterButton(SHOW_ROOM)
        If ssvButton IsNot Nothing Then
            AddHandler ssvButton.Click, _
            Sub()
                '受付メインに遷移
                Me.RedirectNextScreen("SC3100101")
            End Sub
        End If

    End Sub

    ' 2014/02/17 TCS 山田 受注後フォロー機能開発 START
     ''' <summary>
     ''' 顧客検索条件ラジオボタンの制御
     ''' </summary>
     ''' <remarks></remarks>
    Private Sub InitToDoSearchTypeButtonEvent()

        ' ======================== ログ出力 開始 ========================
        Logger.Info(String.Format(CultureInfo.InvariantCulture,
                                  "{0}_Start",
                                  MethodBase.GetCurrentMethod.Name))
        ' ======================== ログ出力 終了 ========================

        Dim beginRowIndex As Integer = 0

        Dim bizLogic As New SC3010401BusinessLogic

        '顧客検索条件項目取得
        Dim todoSearchTypeList As SC3010401DataSet.SC3010401GetCstSearchCondDataTable = bizLogic.GetCstSearchCond()
        Dim wRow As SC3010401DataSet.SC3010401GetCstSearchCondRow

        '顧客検索条件一覧から、画面上のラジオボタンを作成
        For i As Integer = beginRowIndex To todoSearchTypeList.Rows.Count - 1

            wRow = todoSearchTypeList.Item(i)

            If wRow.IsWORD_VALNull Then
                wRow.WORD_VAL = String.Empty
            Else
                wRow.WORD_VAL = wRow.WORD_VAL
            End If

            With ToDoSegmentedButton
                .Items.Add(New ListItem(wRow.WORD_VAL, wRow.CST_SEARCH_COND_CD))
            End With

            ToDoSegmentedButton.Items(0).Selected = True

        Next

        ' ======================== ログ出力 開始 ========================
        Logger.Info(String.Format(CultureInfo.InvariantCulture,
                                  "{0}_End",
                                  MethodBase.GetCurrentMethod.Name))
        ' ======================== ログ出力 終了 ========================

    End Sub

     ''' <summary>
     ''' 受注後工程絞り込み条件チェックボックスの制御
     ''' </summary>
     ''' <remarks></remarks>
    Private Sub InitAfterOdrPrcsButtonEvent()

        ' ======================== ログ出力 開始 ========================
        Logger.Info(String.Format(CultureInfo.InvariantCulture,
                                  "{0}_Start",
                                  MethodBase.GetCurrentMethod.Name))
        ' ======================== ログ出力 終了 ========================

        Dim beginRowIndex As Integer = 0
        Dim YCountIndex As Integer = 2
        Dim XCountIndex As Integer = 2

        Dim bizLogic As New SC3010401BusinessLogic

        '受注後工程アイコンパス取得
        Dim afterOdrPrcsIconPathList As SC3010401DataSet.SC3010401GetAfterOdrProcIconPathDataTable = bizLogic.GetAfterOdrProcIconPath(CHECKBOX_AFTER_ODR_PROC)
        Dim wRow As SC3010401DataSet.SC3010401GetAfterOdrProcIconPathRow

        '受注後工程アイコンパス一覧から、画面上の受注後工程チェックボックスを作成
        For i As Integer = beginRowIndex To afterOdrPrcsIconPathList.Rows.Count - 1

            wRow = afterOdrPrcsIconPathList.Item(i)

            XCountIndex += 1

            If (XCountIndex > 7) Then
                YCountIndex += 1
                XCountIndex = 0
            End If

            wRow.YCount = CStr(YCountIndex)
            wRow.XCount = CStr(XCountIndex)

        Next

        'データバインド
        customCheckBoxRepeater.DataSource = afterOdrPrcsIconPathList
        customCheckBoxRepeater.DataBind()

        ' ======================== ログ出力 開始 ========================
        Logger.Info(String.Format(CultureInfo.InvariantCulture,
                                  "{0}_End",
                                  MethodBase.GetCurrentMethod.Name))
        ' ======================== ログ出力 終了 ========================

    End Sub
    ' 2014/02/17 TCS 山田 受注後フォロー機能開発 END

    ''' <summary>
    ''' TCSとの連携ボタン
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub tcvButton_Click(sender As Object, e As CommonMasterFooterButtonClickEventArgs)

        Logger.Info("tcvButton_Click Start")

        Dim context As StaffContext = StaffContext.Current

        'TCV機能に渡す引数を設定
        e.Parameters.Add("DataSource", "none")
        e.Parameters.Add("MenuLockFlag", CType(False, String))
        e.Parameters.Add("CloseCallback", "closeCallbackFunction")
        e.Parameters.Add("StatusCallback", "statusCallbackFunction")
        e.Parameters.Add("Account", context.Account)
        e.Parameters.Add("AccountStrCd", context.BrnCD)
        e.Parameters.Add("DlrCd", context.DlrCD)
        e.Parameters.Add("OperationCode", context.OpeCD)
        e.Parameters.Add("BusinessFlg", False)
        e.Parameters.Add("ReadOnlyFlg", False)

        Logger.Info("tcvButton_Click End")

    End Sub


    ''' <summary>
    ''' ＴＯＤＯ一覧(CustomerRepeater)の明細選択イベント。
    ''' </summary>
    ''' <param name="sender">イベント発生元</param>
    ''' <param name="e">イベントデータ</param>
    ''' <remarks>顧客詳細画面へ遷移する。</remarks>
    Protected Sub nextButton_Click(sender As Object, e As System.EventArgs) Handles nextButton.Click
        Logger.Info("nextButton_Click Start")

        '次画面遷移パラメータ
        Me.SetValue(ScreenPos.Next, SESSION_KEY_CSTKIND, Me.cstkindHidden.Value)
        Me.SetValue(ScreenPos.Next, SESSION_KEY_CUSTOMERCLASS, Me.customerclassHidden.Value)
        Me.SetValue(ScreenPos.Next, SESSION_KEY_CRCUSTID, Me.crcustidHidden.Value)
        Me.SetValue(ScreenPos.Next, SESSION_KEY_FOLLOW_UP_BOX, Me.fllwupboxseqHidden.Value)
        Me.SetValue(ScreenPos.Next, SESSION_KEY_FLLWUPBOX_STRCD, Me.strcdHidden.Value)

        '顧客詳細画面へ遷移
        Me.RedirectNextScreen("SC3080201")

        Logger.Info("nextButton_Click End")

    End Sub

    Protected Sub sortButton_Click(sender As Object, e As System.EventArgs) Handles sortButton.Click

        '特に処理をしない
        'customerRepeater_ClientCallbackが呼び出される。

    End Sub

 '2012/05/29 TCS 神本 クルクル対応 START

 ''' <summary>
 ''' 再表示ボタン(隠しボタン)押下時
 ''' </summary>
 ''' <param name="sender">ページオブジェクト</param>
 ''' <param name="e">イベント引数</param>
 ''' <remarks></remarks>
 Protected Sub refreshButton_Click(sender As Object, e As System.EventArgs) Handles refreshButton.Click

  'ログ出力 Start ***************************************************************************
        Toyota.eCRB.SystemFrameworks.Core.Logger.Info("refreshButton_Click Start")
  'ログ出力 End *****************************************************************************

  If (Me.ContainsKey(ScreenPos.Current, SESSION_KEY_SERCHDELAY) = True) Then

   'パターン１：顧客を選択する前から、wifiが切れていたパターン
   '           →サーバーサイドの処理が実行されていない。 (ScreenPos.Currentに、セッション情報が存在する)
            Toyota.eCRB.SystemFrameworks.Core.Logger.Info("Me.PageInitialize()")

   '初期処理
   Me.Page_Load(Nothing, Nothing)

  Else

   'パターン２：顧客を選択して、サーバーサイドの処理をしてから、wifiが切れていたパターン
   '           →すでに、サーバーサイドの処理が実行されている。 (ScreenPos.Currentに、セッション情報が存在しない)
   '           →内部的には、戻るボタンを押されたときと同じ処理をする。
            Toyota.eCRB.SystemFrameworks.Core.Logger.Info("Me.RedirectPrevScreen()")

   '前画面へ戻る
   Me.RedirectPrevScreen()

  End If

  'ログ出力 Start ***************************************************************************
        Toyota.eCRB.SystemFrameworks.Core.Logger.Info("refreshButton_Click End")
  'ログ出力 End *****************************************************************************

 End Sub
 '2012/05/29 TCS 神本 クルクル対応 END
End Class
