'━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
'SC3080218.aspx.vb
'─────────────────────────────────────
'機能： 顧客詳細(活動内容)
'補足： 
'作成： 2011/11/24 TCS 安田
'更新： 2012/03/15 TCS 高橋 【SALES_2】(Sales1A ユーザテスト No.222)
'更新： 2012/04/26 TCS 河原 HTMLエンコード対応
'更新： 2012/07/31 TCS 河原 【A STEP2】次世代e-CRB 新車商談機能改善
'更新： 2013/01/24 TCS 藤井 【A.STEP2】次世代e-CRB 新車タブレットSC活動支援機能開発
'更新： 2013/01/31 TCS 坪根 【A.STEP2】次世代e-CRB 新車タブレットSC活動支援機能開発
'更新： 2013/10/04 TCS 安田 次世代e-CRBタブレット iOS7.0 VersionUp対応（セールス）
'更新： 2014/04/02 TCS 河原 性能改善
'更新： 2014/05/13 TCS 山田 性能改善(TCVから見積作成画面遷移)
'更新： 2014/08/01 TCS 外崎 性能改善(ポップアップ高速化対応)
'更新： 2014/08/28 TCS 外崎 TMT NextStep2 UAT-BTS D-118
'─────────────────────────────────────
Option Explicit On
Option Strict On

Imports System.Globalization
Imports Toyota.eCRB.SystemFrameworks.Web
Imports Toyota.eCRB.SystemFrameworks.Core
Imports Toyota.eCRB.SystemFrameworks.Web.Controls
Imports Toyota.eCRB.SystemFrameworks.Core.iCROP.BizLogic
Imports Toyota.eCRB.SystemFrameworks.Core.iCROP.DataAccess
Imports Toyota.eCRB.CustomerInfo.Details.BizLogic
Imports Toyota.eCRB.CustomerInfo.Details.DataAccess
Imports Toyota.eCRB.CommonUtility.DataAccess
Imports Toyota.eCRB.CommonUtility.BizLogic

''' <summary>
''' 顧客詳細(活動内容)
''' Webページのプレゼンテーション層
''' </summary>
''' <remarks>顧客詳細(受注後工程フォロー)</remarks>

Partial Class Pages_SC3080218
    Inherits System.Web.UI.UserControl

#Region " 定数 "

    ''' <summary>カタログ用SEQ</summary>
    Public Const CONTENT_SEQ_CATALOG As Integer = 9

    ''' <summary>試乗用SEQ</summary>
    Public Const CONTENT_SEQ_TESTDRIVE As Integer = 16

    ''' <summary>査定用SEQ</summary>
    Public Const CONTENT_SEQ_ASSESSMENT As Integer = 18

    ''' <summary>見積り用SEQ</summary>
    Public Const CONTENT_SEQ_VALUATION As Integer = 10

    ''' <summary>Follow-upBoxのシーケンスNo.</summary>
    Private Const CONST_FLLWUPBOX_SEQNO As String = "SearchKey.FOLLOW_UP_BOX"

    ''' <summary>Follow-upBoxの店舗コード</summary>
    Private Const SESSION_KEY_FLLWUPBOX_STRCD As String = "SearchKey.FLLWUPBOX_STRCD"
    ''' <summary>Follow-upBoxの店舗コード</summary>
    Private Const SESSION_KEY_PRICE As String = "SearchKey.PRICE"

#End Region

#Region " イベント類 "

    ''' <summary>
    ''' 初期表示
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        '2014/04/02 TCS 河原 性能改善 Start

        '--デバッグログ---------------------------------------------------
        Logger.Info("Page_Load Start")
        '-----------------------------------------------------------------

        '2014/08/01 TCS 外崎 性能改善(ポップアップ高速化対応) START 
        If ScriptManager.GetCurrent(Me.Page).IsInAsyncPostBack Then
            Return
        End If
        '2014/08/01 TCS 外崎 性能改善(ポップアップ高速化対応) END 

        '2014/05/13 TCS 山田 性能改善(TCVから見積作成画面遷移) START
        If ContainsKey(ScreenPos.Current, "StartPageId") Then
            If DirectCast(GetValue(ScreenPos.Current, "StartPageId", False), String).Equals("SC3070201") Then
                Exit Sub
            End If
        End If
        '2014/05/13 TCS 山田 性能改善(TCVから見積作成画面遷移) END

        If Me.Visible Then

            If Not Page.IsCallback And Not Page.IsPostBack Then
                If (Me.ContainsKey(ScreenPos.Current, CONST_FLLWUPBOX_SEQNO)) Then
                    '初期値を設定
                    SetInit()
                End If
                '2013/01/31 TCS 坪根 【A.STEP2】次世代e-CRB 新車タブレットSC活動支援機能開発 START
            Else
                If (Me.ContainsKey(ScreenPos.Current, CONST_FLLWUPBOX_SEQNO)) Then
                    '見積りリストを再設定
                    Me.ReloadValuationList()
                End If
                '2013/01/31 TCS 坪根 【A.STEP2】次世代e-CRB 新車タブレットSC活動支援機能開発 END
            End If
            '2012/07/31 TCS 河原 【A STEP2】次世代e-CRB 新車商談機能改善 START
            InitActTimePopup()
            '2012/07/31 TCS 河原 【A STEP2】次世代e-CRB 新車商談機能改善 END

            '--デバッグログ---------------------------------------------------
            Logger.Info("Page_Load End")
            '-----------------------------------------------------------------

        End If

        '2014/04/02 TCS 河原 性能改善 End

    End Sub

    ' ''' <summary>
    ' ''' プレレンダー
    ' ''' </summary>
    ' ''' <param name="sender"></param>
    ' ''' <param name="e"></param>
    ' ''' <remarks></remarks>
    'Protected Sub Page_PreRender(sender As Object, e As System.EventArgs) Handles Me.PreRender

    '    '--デバッグログ---------------------------------------------------
    '    Logger.Info("Page_PreRender Start")
    '    '-----------------------------------------------------------------

    '    If Not Page.IsCallback And Not Page.IsPostBack Then
    '        If Me.Visible Then

    '            If (Me.ContainsKey(ScreenPos.Current, CONST_FLLWUPBOX_SEQNO)) Then
    '                '初期値を設定
    '                SetInit()

    '                'プロセスの文言設定
    '                GetContentWord()

    '                'アイコンパス取得
    '                GetContentIconPath()

    '                '選択車種を取得してプロセス用のリストを作成
    '                SetSelectedCar()

    '                '日付フォーマットのセット
    '                GetDateFormat()

    '            End If
    '        End If
    '    End If

    '    '--デバッグログ---------------------------------------------------
    '    Logger.Info("Page_PreRender End")
    '    '-----------------------------------------------------------------

    'End Sub

#End Region

#Region " メソット類 "

    ''' <summary>
    ''' 初期値設定
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub SetInit()
        '対応SC欄のデフォルト値(自分)を設定
        InitStaff()

        '今回活動分類のデフォルト値を設定
        InitActContactList()

        'セッション情報を保存
        InitSetSession()

        'エラー文言設定
        SetErrWord()

        'プロセスの文言設定
        GetContentWord()

        'アイコンパス取得
        GetContentIconPath()

        '日付フォーマットのセット
        GetDateFormat()

        'プロセス欄の初期設定
        '2014/08/28 TCS 外崎 TMT NextStep2 UAT-BTS D-118 START
        Me.Sc3080218selectActCatalog.Value = ""
        Me.Sc3080218selectActTestDrive.Value = ""
        '2014/08/28 TCS 外崎 TMT NextStep2 UAT-BTS D-118 END
        UpdateProcessList()

        '対応SCタイトル取得
        InitStaffTitle()

        '今回活動分類タイトル取得
        InitActContactListTitle()

    End Sub

    ''' <summary>
    ''' ステータス変更時の再設定処理
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub RefreshDisplay()

        '今回活動分類のデフォルト値を設定
        InitActContactList()

        '今回活動分類タイトル取得
        InitActContactListTitle()

    End Sub

    ''' <summary>
    ''' 対応SC欄のデフォルト値(自分)を設定
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub InitStaff()
        Dim userary As String()
        userary = Split(StaffContext.Current.Account, "@")
        Me.Sc3080218selectStaff.Value = userary(0)
    End Sub

    ''' <summary>
    ''' 今回活動分類のデフォルト値を設定
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub InitActContactList()
        Dim rw As ActivityInfoDataSet.ActivityInfoActContactRow = ActivityInfoBusinessLogic.GetInitActContact(Sc3080218BookedFlg.Value)
        If (Not IsNothing(rw)) Then
            Me.Sc3080218selectActContact.Value = CStr(rw.CONTACTNO)
            Me.Sc3080218ProcessFlg.Value = rw.PROCESS
            Me.Sc3080218selectActContactTitle.Value = rw.CONTACT
        End If
    End Sub

    ''' <summary>
    ''' エラー文言設定
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Protected Function SetErrWord() As Boolean
        '--デバッグログ---------------------------------------------------
        Logger.Info("SetErrWord Start")
        '-----------------------------------------------------------------
        Me.Sc3080218ErrWord1.Value = WebWordUtility.GetWord(30901)
        Me.Sc3080218ErrWord2.Value = WebWordUtility.GetWord(30903)
        '--デバッグログ---------------------------------------------------
        Logger.Info("SetErrWord End")
        '-----------------------------------------------------------------
        Return True
    End Function

    ''' <summary>
    ''' プロセスの文言設定
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Protected Function GetContentWord() As Boolean

        '--デバッグログ---------------------------------------------------
        Logger.Info("GetContentWord Start")
        '-----------------------------------------------------------------
        '2012/04/26 TCS 河原 HTMLエンコード対応 START
        Using Serchdt As New ActivityInfoDataSet.ActivityInfoSeqDataTable
            Dim Serchrw As ActivityInfoDataSet.ActivityInfoSeqRow
            Serchrw = Serchdt.NewActivityInfoSeqRow
            Serchdt.Rows.Clear()
            Serchrw.SEQNO = CONTENT_SEQ_CATALOG
            Serchdt.Rows.Add(Serchrw)
            Dim contworddt As ActivityInfoDataSet.ActivityInfoContentWordDataTable
            contworddt = ActivityInfoBusinessLogic.GetContentWord(Serchdt)
            Dim contwordrw As ActivityInfoDataSet.ActivityInfoContentWordRow
            contwordrw = contworddt.Item(0)
            Me.Sc3080218CatalogWord.Text = HttpUtility.HtmlEncode(contwordrw.ACTION)
            Me.Sc3080218CatalogTitle.Text = HttpUtility.HtmlEncode(contwordrw.ACTION)
            Serchdt.Rows.Clear()
            Serchrw.SEQNO = CONTENT_SEQ_TESTDRIVE
            Serchdt.Rows.Add(Serchrw)
            contworddt = ActivityInfoBusinessLogic.GetContentWord(Serchdt)
            contwordrw = contworddt.Item(0)
            Me.Sc3080218TestDriveWord.Text = HttpUtility.HtmlEncode(contwordrw.ACTION)
            Me.Sc3080218TestDriveTitle.Text = HttpUtility.HtmlEncode(contwordrw.ACTION)
            Serchdt.Rows.Clear()
            Serchrw.SEQNO = CONTENT_SEQ_ASSESSMENT
            Serchdt.Rows.Add(Serchrw)
            contworddt = ActivityInfoBusinessLogic.GetContentWord(Serchdt)
            contwordrw = contworddt.Item(0)
            Me.Sc3080218AssesmentWord.Text = HttpUtility.HtmlEncode(contwordrw.ACTION)
            Serchdt.Rows.Clear()
            Serchrw.SEQNO = CONTENT_SEQ_VALUATION
            Serchdt.Rows.Add(Serchrw)
            contworddt = ActivityInfoBusinessLogic.GetContentWord(Serchdt)
            contwordrw = contworddt.Item(0)
            Me.Sc3080218ValuationWord.Text = HttpUtility.HtmlEncode(contwordrw.ACTION)
            Me.Sc3080218ValuationTitle.Text = HttpUtility.HtmlEncode(contwordrw.ACTION)
        End Using
        '2012/04/26 TCS 河原 HTMLエンコード対応 END
        '--デバッグログ---------------------------------------------------
        Logger.Info("GetContentWord End")
        '-----------------------------------------------------------------
        Return True
    End Function

    ''' <summary>
    ''' アイコンパス取得
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Protected Function GetContentIconPath() As Boolean

        '--デバッグログ---------------------------------------------------
        Logger.Info("GetContentIconPath Start")
        '-----------------------------------------------------------------
        Dim ipathdt As ActivityInfoDataSet.ActivityInfoContentIconPathDataTable
        Dim ipathtw As ActivityInfoDataSet.ActivityInfoContentIconPathRow

        'カタログ
        ipathdt = ActivityInfoBusinessLogic.GetContentIconPath(CONTENT_SEQ_CATALOG)
        ipathtw = ipathdt.Item(0)
        Me.Sc3080218CatalogSelPath.Value = ipathtw.ICONPATH_RESULT_SELECTED
        Me.Sc3080218CatalogNonSelPath.Value = ipathtw.ICONPATH_RESULT_NOTSELECTED

        '試乗
        ipathdt = ActivityInfoBusinessLogic.GetContentIconPath(CONTENT_SEQ_TESTDRIVE)
        ipathtw = ipathdt.Item(0)
        Me.Sc3080218TestDriveSelPath.Value = ipathtw.ICONPATH_RESULT_SELECTED
        Me.Sc3080218TestDriveNonSelPath.Value = ipathtw.ICONPATH_RESULT_NOTSELECTED

        '査定
        ipathdt = ActivityInfoBusinessLogic.GetContentIconPath(CONTENT_SEQ_ASSESSMENT)
        ipathtw = ipathdt.Item(0)
        Me.Sc3080218AssesmentSelPath.Value = ipathtw.ICONPATH_RESULT_SELECTED
        Me.Sc3080218AssesmentNonSelPath.Value = ipathtw.ICONPATH_RESULT_NOTSELECTED

        '見積り
        ipathdt = ActivityInfoBusinessLogic.GetContentIconPath(CONTENT_SEQ_VALUATION)
        ipathtw = ipathdt.Item(0)
        Me.Sc3080218ValuationSelPath.Value = ipathtw.ICONPATH_RESULT_SELECTED
        Me.Sc3080218ValuationNonSelPath.Value = ipathtw.ICONPATH_RESULT_NOTSELECTED

        '--デバッグログ---------------------------------------------------
        Logger.Info("GetContentIconPath End")
        '-----------------------------------------------------------------
        Return True

    End Function

    ''' <summary>
    ''' 日付フォーマットのセット
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Protected Function GetDateFormat() As Boolean
        '--デバッグログ---------------------------------------------------
        Logger.Info("GetDateFormat Start")
        '-----------------------------------------------------------------
        Dim formatdt As ActivityInfoDataSet.ActivityInfoDateFormatDataTable
        Dim formatrw As ActivityInfoDataSet.ActivityInfoDateFormatRow
        formatdt = ActivityInfoBusinessLogic.GetDateFormat()
        formatrw = formatdt.Item(0)
        Me.Sc3080218dateFormt.Value = formatrw.FORMAT
        '--デバッグログ---------------------------------------------------
        Logger.Info("GetDateFormat End")
        '-----------------------------------------------------------------
        Return True
    End Function

    ''' <summary>
    ''' セッション情報を保存
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub InitSetSession()
        'Follow-upBox情報
        Me.Sc3080218fllwSeq.Value = GetValue(ScreenPos.Current, CONST_FLLWUPBOX_SEQNO, False).ToString()
        Me.Sc3080218fllwstrcd.Value = GetValue(ScreenPos.Current, SESSION_KEY_FLLWUPBOX_STRCD, False).ToString()

        '2013/01/31 TCS 坪根 【A.STEP2】次世代e-CRB 新車タブレットSC活動支援機能開発 START
        'Follow-upBox情報 販売店コード
        Me.Sc3080218fllwdlrcd.Value = StaffContext.Current.DlrCD
        '国コード
        Me.Sc3080218cntcd.Value = EnvironmentSetting.CountryCode
        '2013/01/31 TCS 坪根 【A.STEP2】次世代e-CRB 新車タブレットSC活動支援機能開発 END
    End Sub

    ''' <summary>
    ''' プロセス欄更新<br/>
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub UpdateProcessList()
        '--デバッグログ---------------------------------------------------
        Logger.Info("UpdateActivityResult Start")
        '-----------------------------------------------------------------

        'プロセス欄の車両情報を取得()
        SetSelectedCar()

        'カタログ欄更新
        Sc3080218CatalogListRepeater.DataBind()
        Sc3080218CatalogListUpdatePanel.Update()

        '試乗欄更新
        Sc3080218TestDriveListRepeater.DataBind()
        Sc3080218TestDriveListUpdatePanel.Update()

        '査定欄更新
        Me.Sc3080218selectActAssesment.Value = "0"
        Me.Sc3080218selectActAssesmentWK.Value = "0"

        '見積り欄更新
        Sc3080218ValuationListRepeater.DataBind()
        Sc3080218ValuationListUpdatePanel.Update()

        '更新情報のためのHidden項目の更新
        SC3080218HiddenFieldUpdatePanel.Update()

        '--デバッグログ---------------------------------------------------
        Logger.Info("UpdateActivityResult End")
        '-----------------------------------------------------------------
    End Sub

    ''' <summary>
    ''' プロセスリスト取得
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks>選択車種を取得してプロセス用のリストを作成</remarks>
    Protected Function SetSelectedCar() As Boolean
        '--デバッグログ---------------------------------------------------
        Logger.Info("setSelectedCar Start")
        '-----------------------------------------------------------------

        Using Serchdt As New ActivityInfoDataSet.ActivityInfoDlrStrFollwDataTable
            Dim fllseq As Long
            If ContainsKey(ScreenPos.Current, CONST_FLLWUPBOX_SEQNO) Then
                If String.IsNullOrEmpty(DirectCast(GetValue(ScreenPos.Current, CONST_FLLWUPBOX_SEQNO, False), String)) Then
                    fllseq = 0
                Else
                    fllseq = CType(GetValue(ScreenPos.Current, CONST_FLLWUPBOX_SEQNO, False), Long)
                End If
            Else
                fllseq = 0
            End If
            Dim carList As String = Nothing
            Dim i As Integer = 0
            Dim fllwseriesdt As ActivityInfoDataSet.ActivityInfoFllwSeriesDataTable
            Dim fllwseriesrw As ActivityInfoDataSet.ActivityInfoFllwSeriesRow
            '2012/03/15 TCS 高橋 【SALES_2】(Sales1A ユーザテスト No.222) START
            Dim dicSelectCar As Dictionary(Of String, String) = Nothing
            dicSelectCar = Me.GetSelectedCarDictionary(Me.Sc3080218selectActCatalog.Value)
            '2012/03/15 TCS 高橋 【SALES_2】(Sales1A ユーザテスト No.222) END
            fllwseriesdt = ActivityInfoBusinessLogic.GetFllwSeries(Me.Sc3080218fllwstrcd.Value, fllseq)
            carList = ""
            For i = 0 To fllwseriesdt.Count - 1
                fllwseriesrw = fllwseriesdt.Item(i)
                carList = carList + CStr(fllwseriesrw.SEQNO)
                '2012/03/15 TCS 高橋 【SALES_2】(Sales1A ユーザテスト No.222) START
                'carList = carList + ",0;"
                If dicSelectCar.ContainsKey(CStr(fllwseriesrw.SEQNO)) Then
                    carList = carList + ",1;"
                Else
                    carList = carList + ",0;"
                End If
                '2012/03/15 TCS 高橋 【SALES_2】(Sales1A ユーザテスト No.222) END
            Next
            Me.Sc3080218selectActCatalog.Value = carList
            Me.Sc3080218selectActCatalogWK.Value = carList
            Dim fllwmodeldt As ActivityInfoDataSet.ActivityInfoFllwModelDataTable
            Dim fllwmodelrw As ActivityInfoDataSet.ActivityInfoFllwModelRow
            fllwmodeldt = ActivityInfoBusinessLogic.GetFllwModel(Me.Sc3080218fllwstrcd.Value, fllseq)
            '2012/03/15 TCS 高橋 【SALES_2】(Sales1A ユーザテスト No.222) START
            dicSelectCar = Me.GetSelectedCarDictionary(Me.Sc3080218selectActTestDrive.Value)
            '2012/03/15 TCS 高橋 【SALES_2】(Sales1A ユーザテスト No.222) END
            carList = ""
            For i = 0 To fllwmodeldt.Count - 1
                fllwmodelrw = fllwmodeldt.Item(i)
                carList = carList + CStr(fllwmodelrw.SEQNO)
                '2012/03/15 TCS 高橋 【SALES_2】(Sales1A ユーザテスト No.222) START
                'carList = carList + ",0;"
                If dicSelectCar.ContainsKey(CStr(fllwmodelrw.SEQNO)) Then
                    carList = carList + ",1;"
                Else
                    carList = carList + ",0;"
                End If
                '2012/03/15 TCS 高橋 【SALES_2】(Sales1A ユーザテスト No.222) END
            Next
            Me.Sc3080218selectActTestDrive.Value = carList
            Me.Sc3080218selectActTestDriveWK.Value = carList
            '2013/01/31 TCS 坪根 【A.STEP2】次世代e-CRB 新車タブレットSC活動支援機能開発 START
            'Dim fllwcolordt As ActivityInfoDataSet.ActivityInfoFllwColorDataTable
            'Dim fllwcolorrw As ActivityInfoDataSet.ActivityInfoFllwColorRow
            'fllwcolordt = ActivityInfoBusinessLogic.GetFllwColor(Me.Sc3080218fllwstrcd.Value, fllseq)
            ''2012/03/15 TCS 高橋 【SALES_2】(Sales1A ユーザテスト No.222) START
            'dicSelectCar = Me.GetSelectedCarDictionary(Me.Sc3080218selectActValuation.Value)
            ''2012/03/15 TCS 高橋 【SALES_2】(Sales1A ユーザテスト No.222) END
            'carList = ""
            'For i = 0 To fllwcolordt.Count - 1
            '    fllwcolorrw = fllwcolordt.Item(i)
            '    carList = carList + CStr(fllwcolorrw.SEQNO)
            '    '2012/03/15 TCS 高橋 【SALES_2】(Sales1A ユーザテスト No.222) START
            '    'carList = carList + ",0;"
            '    If dicSelectCar.ContainsKey(CStr(fllwcolorrw.SEQNO)) Then
            '        carList = carList + ",1;"
            '    Else
            '        carList = carList + ",0;"
            '    End If
            '    '2012/03/15 TCS 高橋 【SALES_2】(Sales1A ユーザテスト No.222) END
            'Next
            'Me.Sc3080218selectActValuation.Value = carList
            'Me.Sc3080218selectActValuationWK.Value = carList
            'Me.Sc3080218selectSelSeries.Value = carList

            '見積りリストを設定
            Me.SetSelectedEstimateCar(Me.Sc3080218fllwdlrcd.Value, _
                                      Me.Sc3080218fllwstrcd.Value, _
                                      fllseq, _
                                      Me.Sc3080218cntcd.Value)
            '2013/01/31 TCS 坪根 【A.STEP2】次世代e-CRB 新車タブレットSC活動支援機能開発 END

            '2012/03/15 TCS 高橋 【SALES_2】(Sales1A ユーザテスト No.222) START
            '取得後JavaScript関数
            '2012/04/04 TCS 河原 スクリプトエラー対応
            If Me.Visible Then
                Dim PresenceCategory = StaffContext.Current.PresenceCategory
                Dim Presencedetail = StaffContext.Current.PresenceDetail

                '2013/01/24 TCS 藤井 【A.STEP2】次世代e-CRB 新車タブレットSC活動支援機能開発 Modify Start
                If (String.Equals(PresenceCategory, "1") And String.Equals(Presencedetail, "1")) Or
                    (String.Equals(PresenceCategory, "2") And ((String.Equals(Presencedetail, "0")) Or
                                                               (String.Equals(Presencedetail, "2")))) Then
                    '2013/01/24 TCS 藤井 【A.STEP2】次世代e-CRB 新車タブレットSC活動支援機能開発 Modify End

                    JavaScriptUtility.RegisterStartupFunctionCallScript(Me.Page, "SetSc3080218ProcessIcon", "startup")
                End If
            End If
            '2012/04/04 TCS 河原 スクリプトエラー対応
            '2012/03/15 TCS 高橋 【SALES_2】(Sales1A ユーザテスト No.222) END

            '--デバッグログ---------------------------------------------------
            Logger.Info("setSelectedCar End")
            '-----------------------------------------------------------------
            Return True
        End Using
    End Function

    '2012/03/15 TCS 高橋 【SALES_2】(Sales1A ユーザテスト No.222) START
    ''' <summary>
    ''' 選択されている車両のDictionaryを作成
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function GetSelectedCarDictionary(ByVal carList As String) As Dictionary(Of String, String)
        Dim dic As New Dictionary(Of String, String)
        If String.IsNullOrEmpty(carList) Then
            Return dic
        End If

        Dim carArray() As String = Split(carList, ";")  '選択車両毎に分解

        For Each carInfo As String In carArray
            Dim car() As String = Split(carInfo, ",")   'SEQNOと選択状態に分解
            If car.Length = 2 AndAlso String.IsNullOrEmpty(car(0)) = False Then
                If car(1) = "1" AndAlso dic.ContainsKey(car(0)) = False Then
                    '選択されている場合、Dictinaryに追加
                    dic.Add(car(0), car(1))
                End If
            End If
        Next
        Return dic
    End Function
    '2012/03/15 TCS 高橋 【SALES_2】(Sales1A ユーザテスト No.222) END

    '2013/01/31 TCS 坪根 【A.STEP2】次世代e-CRB 新車タブレットSC活動支援機能開発 START
    ''' <summary>
    ''' プロセス(見積り)リスト取得
    ''' </summary>
    ''' <param name="dlrcd">セッション値の販売店コード</param>
    ''' <param name="strcd">セッション値の店舗コード</param>
    ''' <param name="fllwupboxseqno">セッション値のFollow-up Box内連番</param>
    ''' <param name="cntcd">国コード</param>
    ''' <remarks>選択車種を取得してプロセス用のリストを作成</remarks>
    Private Sub SetSelectedEstimateCar(ByVal dlrcd As String, ByVal strcd As String, _
                                                ByVal fllwupboxseqno As Long, ByVal cntcd As String)

        Dim estimatedt As ActivityInfoDataSet.ActivityInfoEstimateCarDataTable
        Dim estimaterw As ActivityInfoDataSet.ActivityInfoEstimateCarRow
        Dim dicSelectCar As Dictionary(Of String, String) = Nothing
        Dim carList As String = Nothing

        '見積車種情報を取得
        estimatedt = ActivityInfoBusinessLogic.GetEstimateCar(dlrcd, _
                                                              strcd, _
                                                              fllwupboxseqno, _
                                                              cntcd)
        carList = ""

        '見積車種情報が存在している場合のみ処理
        If (Not estimatedt Is Nothing) AndAlso _
                (0 < estimatedt.Rows.Count) Then

            For i = 0 To estimatedt.Count - 1
                estimaterw = estimatedt.Item(i)

                Dim estactflg As String = estimaterw.EST_ACT_FLG
                If estactflg = "1" Then
                    carList = carList & estimaterw.KEYVALUE.ToString & ",1," & estimaterw.IS_EXISTS_SELECTED_SERIES & "," & estimaterw.IS_EXISTS_ESTIMATE & "," & estimaterw.ESTIMATEID & "," & estimaterw.DISPLAY_PRICE & ";"
                Else
                    carList = carList & estimaterw.KEYVALUE.ToString & ",0," & estimaterw.IS_EXISTS_SELECTED_SERIES & "," & estimaterw.IS_EXISTS_ESTIMATE & "," & estimaterw.ESTIMATEID & "," & estimaterw.DISPLAY_PRICE & ";"
                End If
            Next

            'プロセス(見積り)ボタンのポップオーバーを紐づけ
            Me.Sc3080218PopOver6.TriggerClientId = "Sc3080218popupTrigger6"
        Else
            'プロセス(見積り)ボタンのポップオーバーを紐づけ解除
            Me.Sc3080218PopOver6.TriggerClientId = String.Empty
        End If

        '見積ポップアップ画面に表示する見積車種をログ出力
        Logger.Info("selectActValuation = " & carList)

        Me.Sc3080218selectActValuation.Value = carList
        Me.Sc3080218selectActValuationWK.Value = carList
        Me.Sc3080218selectSelSeries.Value = carList
        ' 2013/06/30 TCS 小幡 2013/10対応版　既存流用 削除 START
        ' 2013/06/30 TCS 小幡 2013/10対応版　既存流用 削除 END
    End Sub

    ''' <summary>
    ''' 見積ポップアップ画面の再表示
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub ReloadValuationList()
        Dim fllwseq As Long = Me.GetFollowupSeq

        '見積りリストを設定
        Me.SetSelectedEstimateCar(Me.Sc3080218fllwdlrcd.Value, _
                                  Me.Sc3080218fllwstrcd.Value, _
                                  fllwseq, _
                                  Me.Sc3080218cntcd.Value)

    End Sub

    ''' <summary>
    ''' FollowUpのSEQ取得
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function GetFollowupSeq() As Long
        Dim fllseq As Long
        If (Me.ContainsKey(ScreenPos.Current, CONST_FLLWUPBOX_SEQNO)) Then
            If String.IsNullOrEmpty(DirectCast(GetValue(ScreenPos.Current, CONST_FLLWUPBOX_SEQNO, False), String)) Then
                fllseq = 0
            Else
                fllseq = CType(GetValue(ScreenPos.Current, CONST_FLLWUPBOX_SEQNO, False), Long)
            End If
        Else
            fllseq = 0
        End If

        Return fllseq
    End Function
    '2013/01/31 TCS 坪根 【A.STEP2】次世代e-CRB 新車タブレットSC活動支援機能開発 END

    ''' <summary>
    ''' 各パネルの初期設定
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub InitPanel()

        Me.Sc3080218ActTimePanel.Visible = False
        Me.Sc3080218ActTimePopupFlg.Value = "0"
        Me.Sc3080218ActTimeUpdatePanel.Update()

        Me.Sc3080218StaffListPanel.Visible = False
        Me.Sc3080218StaffListPopupFlg.Value = "0"
        Me.Sc3080218StaffListUpdatePanel.Update()

        Me.Sc3080218ActContactListPanel.Visible = False
        Me.Sc3080218ActContactListPopupFlg.Value = "0"

        Me.Sc3080218CatalogListPanel.Visible = False
        Me.Sc3080218CatalogListPopupFlg.Value = "0"

        Me.Sc3080218TestDriveListPanel.Visible = False
        Me.Sc3080218TestDriveListPopupFlg.Value = "0"

        Me.Sc3080218ValuationListPanel.Visible = False
        Me.Sc3080218ValuationListPopupFlg.Value = "0"

        Me.Sc3080218PopupFlgUpdatePanel.Update()

    End Sub

    ''' <summary>
    ''' 対応SCタイトル取得
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub InitStaffTitle()
        Dim accountary As New StringBuilder
        accountary.Append(Me.Sc3080218selectStaff.Value)
        accountary.Append("@")
        accountary.Append(StaffContext.Current.DlrCD)
        Dim account As String = accountary.ToString()
        Dim dt As ActivityInfoDataSet.ActivityInfoUsersDataTable
        Dim rw As ActivityInfoDataSet.ActivityInfoUsersRow
        dt = ActivityInfoBusinessLogic.GetStaff(account)
        If (dt.Rows.Count > 0) Then
            rw = CType(dt.Rows(0), ActivityInfoDataSet.ActivityInfoUsersRow)
            Me.Sc3080218selectStaffName.Value = rw.USERNAME
        End If
    End Sub

    ''' <summary>
    ''' 今回活動分類タイトル取得
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub InitActContactListTitle()
        Dim actContactTitle = ActivityInfoBusinessLogic.GetInitActContactTitle(Me.Sc3080218selectActContact.Value)
        Me.Sc3080218selectActContactTitle.Value = actContactTitle
    End Sub

    '2012/07/31 TCS 河原 【A STEP2】次世代e-CRB 新車商談機能改善 START
    Public Sub InitActTimePopup()
        Me.DispPage3Flg.Value = "0"
        Me.Sc3080218ActTimePanel.Visible = False
        Me.Sc3080218ActTimePopupFlg.Value = "0"
        'If Me.SC3080218UpdateRWFlg.Value = "1" Then
        '    Me.Sc3080218ActTimeToSelector.Value = Nothing
        '    Me.Sc3080218ActTimeToSelectorWK.Value = Nothing
        '    Me.Sc3080218ActTimeToSelectorWK2.Value = Nothing
        'End If
    End Sub
    '2012/07/31 TCS 河原 【A STEP2】次世代e-CRB 新車商談機能改善 END

#End Region

#Region " プロパティ類 "

    ''' <summary>
    ''' 活動開始日時
    ''' </summary>
    ''' <value>活動開始日時</value>
    ''' <remarks></remarks>
    Public Property ActTimeFrom() As Date
        Get
            'ポップアップ読み込み前はダミーから返却
            If Sc3080218ActTimePanel.Visible Then
                Return Me.Sc3080218ActTimeFromSelectorWK.Value.Value
            Else
                Return Me.Sc3080218ActTimeFromSelectorWK2.Value.Value
            End If
        End Get
        Set(ByVal value As Date)
            Me.Sc3080218ActTimeFromSelector.Value = value
            Me.Sc3080218ActTimeFromSelectorWK.Value = value
            Me.Sc3080218ActTimeFromSelectorWK2.Value = value
        End Set
    End Property

    ''' <summary>
    ''' 活動終了時間
    ''' </summary>
    ''' <value>活動終了時間</value>
    ''' <remarks></remarks>
    Public Property ActTimeTo() As Date
        Get
            'ポップアップ読み込み前はダミーから返却
            If Sc3080218ActTimePanel.Visible Then
                Return Me.Sc3080218ActTimeToSelectorWK.Value.Value
            Else
                Return Me.Sc3080218ActTimeToSelectorWK2.Value.Value
            End If
        End Get
        Set(ByVal value As Date)
            Me.Sc3080218ActTimeToSelector.Value = value
            Me.Sc3080218ActTimeToSelectorWK.Value = value
            Me.Sc3080218ActTimeToSelectorWK2.Value = value
            '2012/07/31 TCS 河原 【A STEP2】次世代e-CRB 新車商談機能改善 START
            '日付を渡されたら更新可否フラグを0にする
            Me.SC3080218UpdateRWFlg.Value = "0"
            '2012/07/31 TCS 河原 【A STEP2】次世代e-CRB 新車商談機能改善 END
        End Set
    End Property

    ''' <summary>
    ''' 対応SC
    ''' </summary>
    ''' <value>対応SC</value>
    ''' <remarks></remarks>
    Public Property SelectStaff() As String
        Get
            Return Me.Sc3080218selectStaff.Value
        End Get
        Set(ByVal value As String)
            Me.Sc3080218selectStaff.Value = value
        End Set
    End Property

    ''' <summary>
    ''' 活動方法
    ''' </summary>
    ''' <value>活動方法</value>
    ''' <remarks></remarks>
    Public Property SelectActContact() As String
        Get
            Return Me.Sc3080218selectActContact.Value
        End Get
        Set(ByVal value As String)
            Me.Sc3080218selectActContact.Value = value
        End Set
    End Property

    ''' <summary>
    ''' プロセス有無
    ''' </summary>
    ''' <value>プロセス有無</value>
    ''' <remarks></remarks>
    Public Property ProcessFlg() As String
        Get
            Return Me.Sc3080218ProcessFlg.Value
        End Get
        Set(ByVal value As String)
            Me.Sc3080218ProcessFlg.Value = value
        End Set
    End Property

    ''' <summary>
    ''' 受注後フラグ
    ''' </summary>
    ''' <value>受注後フラグ</value>
    ''' <remarks></remarks>
    Public Property BookedFlg() As String
        Get
            Return Me.Sc3080218BookedFlg.Value
        End Get
        Set(ByVal value As String)
            Me.Sc3080218BookedFlg.Value = value
        End Set
    End Property

    ''' <summary>
    ''' プロセス(カタログ)
    ''' </summary>
    ''' <value>プロセス(カタログ)</value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property selectActCatalog() As String
        Get
            Return Me.Sc3080218selectActCatalog.Value
        End Get
        Set(ByVal value As String)
            Me.Sc3080218selectActCatalog.Value = value
        End Set
    End Property

    ''' <summary>
    ''' プロセス(試乗)
    ''' </summary>
    ''' <value>プロセス(試乗)</value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property selectActTestDrive() As String
        Get
            Return Me.Sc3080218selectActTestDrive.Value
        End Get
        Set(ByVal value As String)
            Me.Sc3080218selectActTestDrive.Value = value
        End Set
    End Property

    ''' <summary>
    ''' プロセス(査定)
    ''' </summary>
    ''' <value>プロセス(査定)</value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property selectActAssesment() As String
        Get
            Return Me.Sc3080218selectActAssesment.Value
        End Get
        Set(ByVal value As String)
            Me.Sc3080218selectActAssesment.Value = value
        End Set
    End Property

    ''' <summary>
    ''' プロセス(見積り)
    ''' </summary>
    ''' <value>プロセス(見積り)</value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property selectActValuation() As String
        Get
            Return Me.Sc3080218selectActValuation.Value
        End Get
        Set(ByVal value As String)
            Me.Sc3080218selectActValuation.Value = value
        End Set
    End Property

    ''' <summary>
    ''' 選択車種
    ''' </summary>
    ''' <value>選択車種</value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property selectSelSeries() As String
        Get
            Return Me.Sc3080218selectSelSeries.Value
        End Get
        Set(ByVal value As String)
            Me.Sc3080218selectSelSeries.Value = value
        End Set
    End Property

#End Region

#Region " ページクラス処理のバイパス処理 "
    Private Sub SetValue(ByVal pos As ScreenPos, ByVal key As String, ByVal value As Object)
        GetPageInterface().SetValueBypass(pos, key, value)
    End Sub

    Private Function GetValue(ByVal pos As ScreenPos, ByVal key As String, ByVal removeFlg As Boolean) As Object
        Return GetPageInterface().GetValueBypass(pos, key, removeFlg)
    End Function

    Private Sub ShowMessageBox(ByVal wordNo As Integer, ByVal ParamArray wordParam() As String)
        GetPageInterface().ShowMessageBoxBypass(wordNo, wordParam)
    End Sub

    Private Function ContainsKey(ByVal pos As Toyota.eCRB.SystemFrameworks.Web.ScreenPos, ByVal key As String) As Boolean
        Return GetPageInterface().ContainsKeyBypass(pos, key)
    End Function

    Private Function GetPageInterface() As ICustomerDetailControl
        Return CType(Me.Page, ICustomerDetailControl)
    End Function
#End Region

#Region " ポップアップ後読み処理 "

    ''' <summary>
    ''' 活動日時ダミーボタン
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub Sc3080218ActTimeButton_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles Sc3080218ActTimeButton.Click

        'パネルを表示
        Me.Sc3080218ActTimePanel.Visible = True

        '2012/07/31 TCS 河原 【A STEP2】次世代e-CRB 新車商談機能改善 START
        '初回表示時間を設定
        If Me.SC3080218UpdateRWFlg.Value = "1" Then
            Me.Sc3080218ActTimeToSelector.Value = CType(Me.FastDispTime.Value, Date?)
            Me.Sc3080218ActTimeToSelectorWK.Value = CType(Me.FastDispTime.Value, Date?)
            Me.Sc3080218ActTimeToSelectorWK2.Value = CType(Me.FastDispTime.Value, Date?)
        Else
            Me.Sc3080218ActTimeToSelector.Value = Me.Sc3080218ActTimeToSelectorWK2.Value
            '2013/10/04 TCS 安田 TCS 安田 次世代e-CRBタブレット iOS7.0 VersionUp対応（セールス） START
            Me.Sc3080218ActTimeFromSelector.Value = ActTimeFrom
            '2013/10/04 TCS 安田 TCS 安田 次世代e-CRBタブレット iOS7.0 VersionUp対応（セールス） END
        End If
        '2012/07/31 TCS 河原 【A STEP2】次世代e-CRB 新車商談機能改善 END

        '取得完了フラグ
        Me.Sc3080218ActTimePopupFlg.Value = "1"

        '反映させる為に更新
        Me.Sc3080218ActTimeUpdatePanel.Update()
        Me.Sc3080218PopupFlgUpdatePanel.Update()

        '取得後JavaScript関数
        JavaScriptUtility.RegisterStartupFunctionCallScript(Me.Page, "setSc3080218ActTimePageOpenEnd", "startup")

    End Sub

    ''' <summary>
    ''' 対応SCダミーボタン
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub Sc3080218StaffListButton_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles Sc3080218StaffListButton.Click

        'パネルを表示
        Me.Sc3080218StaffListPanel.Visible = True

        'データ取得
        Me.Sc3080218StaffListRepeater.DataBind()

        '取得完了フラグ
        Me.Sc3080218StaffListPopupFlg.Value = "1"

        '反映させる為に更新
        Me.Sc3080218StaffListUpdatePanel.Update()
        Me.Sc3080218PopupFlgUpdatePanel.Update()

        '取得後JavaScript関数
        JavaScriptUtility.RegisterStartupFunctionCallScript(Me.Page, "setSc3080218StaffListPageOpenEnd", "startup")

    End Sub

    ''' <summary>
    ''' 活動分類ダミーボタン
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub Sc3080218ActContactListButton_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles Sc3080218ActContactListButton.Click

        'パネルを表示
        Me.Sc3080218ActContactListPanel.Visible = True

        'データ取得
        Me.Sc3080218ActContactListRepeater.DataBind()

        '取得完了フラグ
        Me.Sc3080218ActContactListPopupFlg.Value = "1"

        '反映させる為に更新
        Me.Sc3080218ActContactListUpdatePanel.Update()
        Me.Sc3080218PopupFlgUpdatePanel.Update()

        '取得後JavaScript関数
        JavaScriptUtility.RegisterStartupFunctionCallScript(Me.Page, "setSc3080218ActContactListPageOpenEnd", "startup")

    End Sub

    ''' <summary>
    ''' カタログダミーボタン
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub Sc3080218CatalogListButton_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles Sc3080218CatalogListButton.Click

        'パネルを表示
        Me.Sc3080218CatalogListPanel.Visible = True

        'データ取得
        Me.Sc3080218CatalogListRepeater.DataBind()

        '取得完了フラグ
        Me.Sc3080218CatalogListPopupFlg.Value = "1"

        '反映させる為に更新
        Me.Sc3080218CatalogListUpdatePanel.Update()
        Me.Sc3080218PopupFlgUpdatePanel.Update()

        '取得後JavaScript関数
        JavaScriptUtility.RegisterStartupFunctionCallScript(Me.Page, "setSc3080218CatalogListPageOpenEnd", "startup")

    End Sub

    ''' <summary>
    ''' 試乗ダミーボタン
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub Sc3080218TestDriveListButton_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles Sc3080218TestDriveListButton.Click

        'パネルを表示
        Me.Sc3080218TestDriveListPanel.Visible = True

        'データ取得
        Me.Sc3080218TestDriveListRepeater.DataBind()

        '取得完了フラグ
        Me.Sc3080218TestDriveListPopupFlg.Value = "1"

        '反映させる為に更新
        Me.Sc3080218TestDriveListUpdatePanel.Update()
        Me.Sc3080218PopupFlgUpdatePanel.Update()

        '取得後JavaScript関数
        JavaScriptUtility.RegisterStartupFunctionCallScript(Me.Page, "setSc3080218TestDriveListPageOpenEnd", "startup")

    End Sub

    ''' <summary>
    ''' 見積りダミーボタン
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub Sc3080218ValuationListButton_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles Sc3080218ValuationListButton.Click

        'パネルを表示
        Me.Sc3080218ValuationListPanel.Visible = True

        'データ取得
        Me.Sc3080218ValuationListRepeater.DataBind()

        '取得完了フラグ
        Me.Sc3080218ValuationListPopupFlg.Value = "1"

        '反映させる為に更新
        Me.Sc3080218ValuationListUpdatePanel.Update()
        Me.Sc3080218PopupFlgUpdatePanel.Update()

        '取得後JavaScript関数
        JavaScriptUtility.RegisterStartupFunctionCallScript(Me.Page, "setSc3080218ValuationListPageOpenEnd", "startup")

    End Sub

#End Region

End Class
