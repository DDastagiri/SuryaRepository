
'━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
'SC3190601.aspx.vb
'──────────────────────────────────
'機能： B/O管理ボード
'補足： 
'作成： 2013/08/26 TMEJ M.Asano
'更新： 
'──────────────────────────────────

Imports System.Data
Imports System.Reflection
Imports System.Globalization
Imports System.Web.Services
Imports System.Web.Script.Serialization
Imports Toyota.eCRB.SystemFrameworks.Web
Imports Toyota.eCRB.SystemFrameworks.Core
Imports Toyota.eCRB.SystemFrameworks.Core.iCROP.BizLogic
Imports Toyota.eCRB.SystemFrameworks.Core.iCROP.DataAccess
Imports Toyota.eCRB.PartsManagement.BoMonitor.BizLogic
Imports Toyota.eCRB.PartsManagement.BoMonitor.DataAccess

''' <summary>
''' B/O管理ボード
''' </summary>
''' <remarks></remarks>
Partial Class Pages_SC3190601
    Inherits BasePage

#Region "定数"

#Region "DB関連"

    ''' <summary>
    ''' 自動ページング時間①
    ''' </summary>
    ''' <remarks></remarks>
    Private Const AutoPagingTimeFirst As String = "AUTO_PAGING_TIME_FIRST"

    ''' <summary>
    ''' 自動ページング時間②
    ''' </summary>
    ''' <remarks></remarks>
    Private Const AutoPagingTimeSecond As String = "AUTO_PAGING_TIME_SECOND"

    ''' <summary>
    ''' 直近到着予定部品判定日数
    ''' </summary>
    ''' <remarks></remarks>
    Private Const ArrivalCloseJudgementDays As String = "ARRIVAL_CLOSE_JUDGEMENT_DAYS"

    ''' <summary>
    ''' 車両預かりフラグ:顧客
    ''' </summary>
    ''' <remarks></remarks>
    Private Const VclPartakeFlgCust As String = "0"

    ''' <summary>
    ''' 車両預かりフラグ:販売店
    ''' </summary>
    ''' <remarks></remarks>
    Private Const VclPartakeFlgDlr As String = "1"

    ''' <summary>
    ''' お客様約束日遅れフラグ
    ''' </summary>
    ''' <remarks></remarks>
    Private Const DelayCustAppointmentDateFlag As String = "1"

    ''' <summary>
    ''' 直近到着予定フラグ
    ''' </summary>
    ''' <remarks></remarks>
    Private Const ArrivalCloseJudgementFlag As String = "1"

#End Region

#Region "文言ID"

    ''' <summary>
    ''' 文言ID：画面タイトル
    ''' </summary>
    ''' <remarks></remarks>
    Private Const WordIdScreenTitle As String = "1"

    ''' <summary>
    ''' 文言ID：CurrentStatus
    ''' </summary>
    ''' <remarks></remarks>
    Private Const WordIdCurrentStatus As String = "2"
    ''' <summary>
    ''' 文言ID：P/O全数
    ''' </summary>
    ''' <remarks></remarks>
    Private Const WordIdPoTotal As String = "3"

    ''' <summary>
    ''' 文言ID：部品全数
    ''' </summary>
    ''' <remarks></remarks>
    Private Const WordIdPartsTotal As String = "4"

    ''' <summary>
    ''' 文言ID：遅れ件数
    ''' </summary>
    ''' <remarks></remarks>
    Private Const WordIdDelayCount As String = "5"

    ''' <summary>
    ''' 文言ID：部品追加ボタン
    ''' </summary>
    ''' <remarks></remarks>
    Private Const WordIdPartsAddButton As String = "6"

    ''' <summary>
    ''' 文言ID：リストヘッダ(No)
    ''' </summary>
    ''' <remarks></remarks>
    Private Const WordIdListHeaderNo As String = "7"

    ''' <summary>
    ''' 文言ID：リストヘッダ(PO番号)
    ''' </summary>
    ''' <remarks></remarks>
    Private Const WordIdListHeaderPoNo As String = "8"

    ''' <summary>
    ''' 文言ID：リストヘッダ(RO番号)
    ''' </summary>
    ''' <remarks></remarks>
    Private Const WordIdListHeaderRoNo As String = "9"

    ''' <summary>
    ''' 文言ID：リストヘッダ(Operation)
    ''' </summary>
    ''' <remarks></remarks>
    Private Const WordIdListHeaderOperation As String = "10"

    ''' <summary>
    ''' 文言ID：リストヘッダ(Parts)
    ''' </summary>
    ''' <remarks></remarks>
    Private Const WordIdListHeaderParts As String = "11"

    ''' <summary>
    ''' 文言ID：リストヘッダ(Qty)
    ''' </summary>
    ''' <remarks></remarks>
    Private Const WordIdListHeaderQty As String = "12"

    ''' <summary>
    ''' 文言ID：リストヘッダ(OrderDate)
    ''' </summary>
    ''' <remarks></remarks>
    Private Const WordIdListHeaderOrderDate As String = "13"

    ''' <summary>
    ''' 文言ID：リストヘッダ(Eta)
    ''' </summary>
    ''' <remarks></remarks>
    Private Const WordIdListHeaderEta As String = "14"

    ''' <summary>
    ''' 文言ID：リストヘッダ(Vehicle)
    ''' </summary>
    ''' <remarks></remarks>
    Private Const WordIdListHeaderVehicle As String = "15"

    ''' <summary>
    ''' 文言ID：リストヘッダ(AptDate)
    ''' </summary>
    ''' <remarks></remarks>
    Private Const WordIdListHeaderAptDate As String = "16"

    ''' <summary>
    ''' 文言ID：車両預かりフラグ(顧客)
    ''' </summary>
    ''' <remarks></remarks>
    Private Const WordIdVclPartakeFlgCust As String = "17"

    ''' <summary>
    ''' 文言ID：車両預かりフラグ(販売店)
    ''' </summary>
    ''' <remarks></remarks>
    Private Const WordIdVclPartakeFlgDlr As String = "18"

    ''' <summary>
    ''' 文言ID：未登録時表示文言
    ''' </summary>
    ''' <remarks></remarks>
    Private Const WordIdUnregisteredWord As String = "19"

    ''' <summary>
    ''' 文言ID：コロン
    ''' </summary>
    ''' <remarks></remarks>
    Private Const WordIdColonWord As String = "20"
#End Region

#Region "その他"

    ''' <summary>
    ''' 画面ID
    ''' </summary>
    ''' <remarks></remarks>
    Private Const AppId As String = "SC3190601"

    ''' <summary>
    ''' 画面ID(部品庫モニター)
    ''' </summary>
    ''' <remarks></remarks>
    Private Const PartsMonitorAppId As String = "SC3190402"

    ''' <summary>
    ''' ページ最大行数
    ''' </summary>
    ''' <remarks></remarks>
    Private Const PAGE_MAX_ROW_NUNBER As Integer = 9

    ''' <summary>
    ''' ZERO
    ''' </summary>
    ''' <remarks></remarks>
    Private Const ZERO_STRING As String = "0"
#End Region

#End Region

#Region "イベント処理"

    ''' <summary>
    ''' ページロード時の処理
    ''' </summary>
    ''' <param name="sender">イベント発生元</param>
    ''' <param name="e">イベントデータ</param>
    ''' <remarks></remarks>
    Protected Sub Page_Load(sender As Object, e As System.EventArgs) Handles Me.Load

        Logger.Info("Page_Load_Start")

        If Me.IsPostBack Then
            Logger.Info("Page_Load_End IsPostBack")
            Return
        End If

        ' 各種設定値取得
        ' 自動ページング時間①
        Me.AutoPagingTimeFirstField.Value = GetBranchEnvSetting(AutoPagingTimeFirst)

        ' 自動ページング時間②
        Me.AutoPagingTimeSecondField.Value = GetBranchEnvSetting(AutoPagingTimeSecond)

        ' 直近到着予定部品判定日数
        Me.JudgementDaysField.Value = GetBranchEnvSetting(ArrivalCloseJudgementDays)

        ' 文言設定
        Me.SetWord()

        'スクリプトの登録
        JavaScriptUtility.RegisterStartupFunctionCallScript(Me.Page, "showSC3190601", "startup")

        Logger.Info("Page_Load_End")

    End Sub

    ''' <summary>
    ''' ページングボタン押下処理
    ''' </summary>
    ''' <param name="sender">イベント発生元</param>
    ''' <param name="e">イベントデータ</param>
    ''' <remarks></remarks>
    Protected Sub PagingButton_Click(sender As Object, e As System.EventArgs) Handles PagingButton.Click

        Logger.Info("PagingButton_Click_Start")

        ' 初期表示処理
        Me.DisplayPartsInfoList(Decimal.Parse(Me.NowPageCount.Value))

        'スクリプトの登録
        JavaScriptUtility.RegisterStartupFunctionCallScript(Me.Page, "initSC3190601", "paging")

        Logger.Info("PagingButton_Click_End")
    End Sub

    ''' <summary>
    ''' ページ遷移ボタン押下処理
    ''' </summary>
    ''' <param name="sender">イベント発生元</param>
    ''' <param name="e">イベントデータ</param>
    ''' <remarks></remarks>
    Protected Sub ScreenTransitionButton_Click(sender As Object, e As System.EventArgs) Handles ScreenTransitionButton.Click

        Logger.Info("ScreenTransitionButton_Click_Start")

        Me.RedirectNextScreen(PartsMonitorAppId)

        Logger.Info("ScreenTransitionButton_Click_End")
    End Sub

#End Region

#Region "非公開メソッド"

#Region "画面表示"

    ''' <summary>
    ''' 部品情報一覧表示処理
    ''' </summary>
    ''' <param name="pageNumber">表示対象ページ番号</param>
    ''' <remarks></remarks>
    Private Sub DisplayPartsInfoList(ByVal pageNumber As Integer)

        Logger.Info("DisplayPartsInfoList_Start ParamValue[" & pageNumber.ToString & "]")

        'スタッフ情報を取得
        Logger.Info("DisplayPartsInfoList_001 " & "Call_Start StaffContext.Current")
        Dim staffInfo As StaffContext = StaffContext.Current
        Logger.Info("DisplayPartsInfoList_001 " & "Call_End   StaffContext.Current IsNull[" & IsNothing(staffInfo) & "]")

        ' 本日日付取得
        Logger.Info("DisplayPartsInfoList_002 " & "Call_Start DateTimeFunc.Now PramValue[" & staffInfo.DlrCD & "]")
        Dim nowDate As Date = DateTimeFunc.Now(staffInfo.DlrCD)
        Logger.Info("DisplayPartsInfoList_002 " & "Call_End   DateTimeFunc.Now RetValue[" & nowDate.ToString & "]")
        Dim todayDate As New Date(nowDate.Year, nowDate.Month, nowDate.Day, 0, 0, 0)
        nowDate = nowDate.AddDays(Integer.Parse(Me.JudgementDaysField.Value))
        Dim judgementDate As New Date(nowDate.Year, nowDate.Month, nowDate.Day, 23, 59, 59)

        ' 部品情報一覧取得
        Dim businessLogic As SC3190601BusinessLogic = New SC3190601BusinessLogic
        Dim partsInfoListDataTable As SC3190601DataSet.BoPartsInfoListDataTable = _
            businessLogic.GetPartsInfoList(staffInfo.DlrCD, staffInfo.BrnCD, todayDate, judgementDate)

        ' 最大ページ数の算出
        If partsInfoListDataTable.Count = 0 Then
            Me.NowPageCount.Value = 0
            Me.MaxPageCount.Value = 0
        Else
            Me.NowPageCount.Value = pageNumber
            If (partsInfoListDataTable.Count Mod PAGE_MAX_ROW_NUNBER) = 0 Then
                Me.MaxPageCount.Value = Math.Floor(partsInfoListDataTable.Count / PAGE_MAX_ROW_NUNBER)
            Else
                Me.MaxPageCount.Value = Math.Floor(partsInfoListDataTable.Count / PAGE_MAX_ROW_NUNBER) + 1
            End If
        End If

        ' 対象のページのデータを抽出する
        Dim startRowNumber As Integer
        Dim endRowNumber As Integer
        If pageNumber = 1 Then
            startRowNumber = 0
            endRowNumber = PAGE_MAX_ROW_NUNBER - 1
        Else
            startRowNumber = (pageNumber - 1) * PAGE_MAX_ROW_NUNBER
            endRowNumber = (pageNumber * PAGE_MAX_ROW_NUNBER) - 1
        End If
        Dim targetPageBoPartsInfoList As SC3190601DataSet.BoPartsInfoListDataTable = _
            GetTargetPageData(partsInfoListDataTable, startRowNumber, endRowNumber)

        ' データバインド
        Me.SC3190601_PartsInfoListRepeater.DataSource = targetPageBoPartsInfoList
        Me.SC3190601_PartsInfoListRepeater.DataBind()

        ' リストの表示加工処理
        Me.ProcessingPartsInfoList(targetPageBoPartsInfoList)

        Logger.Info("DisplayPartsInfoList_End")

    End Sub

    ''' <summary>
    ''' 取得対象のページデータ取得処理
    ''' </summary>
    ''' <param name="partsInfoList">BoPartsInfoListDataTable(部品情報一覧)</param>
    ''' <remarks></remarks>
    Private Function GetTargetPageData(ByVal partsInfoList As SC3190601DataSet.BoPartsInfoListDataTable, _
                                       ByVal startRowNumber As Integer,
                                       ByVal endRowNumber As Integer) _
                                       As SC3190601DataSet.BoPartsInfoListDataTable

        Dim boPartsInfoList As SC3190601DataSet.BoPartsInfoListDataTable = New SC3190601DataSet.BoPartsInfoListDataTable

        ' データ無しの場合は処理しない
        If partsInfoList.Count <= 0 Then
            Return boPartsInfoList
        End If

        ' No振り
        Dim lastIndex As Integer = partsInfoList.Count() - 1
        Dim number As Integer = 1
        Dim boId As Decimal = 0

        For i As Integer = 0 To lastIndex

            ' 取得対象行を超えた場合はループを抜ける
            If endRowNumber < i Then
                Exit For
            End If

            If i = 0 Then
                partsInfoList(i).No = number
                boId = partsInfoList(i).BO_ID
            Else
                If boId = partsInfoList(i).BO_ID Then
                    partsInfoList(i).No = number
                Else
                    number = number + 1
                    boId = partsInfoList(i).BO_ID
                    partsInfoList(i).No = number
                End If
            End If

            ' 取得対象行か判定
            If startRowNumber <= i And i <= endRowNumber Then
                boPartsInfoList.ImportRow(partsInfoList(i))
            End If
        Next

        Return boPartsInfoList
    End Function

    ''' <summary>
    ''' 部品情報一覧(表示加工処理)
    ''' </summary>
    ''' <param name="partsInfoListDataTable"></param>
    ''' <remarks></remarks>
    Private Sub ProcessingPartsInfoList(ByVal partsInfoListDataTable As SC3190601DataSet.BoPartsInfoListDataTable)

        ' ヘッダ部件数
        If Not partsInfoListDataTable Is Nothing AndAlso partsInfoListDataTable.Count > 0 Then
            Me.POTotalValField.Value = Server.HtmlEncode(partsInfoListDataTable(0).PO_COUNT)
            Me.PODelayField.Value = Server.HtmlEncode(String.Format(System.Globalization.CultureInfo.CurrentCulture _
                                                    , WebWordUtility.GetWord(AppId, WordIdDelayCount) _
                                                    , partsInfoListDataTable(0).PO_DELAY_COUNT))
            Me.PSTotalValField.Value = Server.HtmlEncode(partsInfoListDataTable(0).PARTS_COUNT)
            Me.PSDelayField.Value = Server.HtmlEncode(String.Format(System.Globalization.CultureInfo.CurrentCulture _
                                                    , WebWordUtility.GetWord(AppId, WordIdDelayCount) _
                                                    , partsInfoListDataTable(0).PARTS_DELAY_COUNT))
        Else
            Me.POTotalValField.Value = Server.HtmlEncode(ZERO_STRING)
            Me.PODelayField.Value = Server.HtmlEncode(String.Format(System.Globalization.CultureInfo.CurrentCulture _
                                                    , WebWordUtility.GetWord(AppId, WordIdDelayCount) _
                                                    , ZERO_STRING))
            Me.PSTotalValField.Value = Server.HtmlEncode(ZERO_STRING)
            Me.PSDelayField.Value = Server.HtmlEncode(String.Format(System.Globalization.CultureInfo.CurrentCulture _
                                                    , WebWordUtility.GetWord(AppId, WordIdDelayCount) _
                                                    , ZERO_STRING))

            ' 該当データ無しの為以降の処理は行わない。
            Return
        End If

        Dim firstNoControl As HtmlTableCell = Nothing
        Dim firstPoControl As HtmlTableCell = Nothing
        Dim firstRoControl As HtmlTableCell = Nothing
        Dim firstOperationControl As HtmlTableCell = Nothing
        Dim firstBoId As Decimal = 0
        Dim firstOperation As String = String.Empty
        Dim poRowCount As Integer = 1
        Dim operationRowCount As Integer = 1

        ' 件数分表示する
        For i = 0 To Me.SC3190601_PartsInfoListRepeater.Items.Count - 1

            Dim partsInfoControl As Control = SC3190601_PartsInfoListRepeater.Items(i)
            Dim partsInfoRow As SC3190601DataSet.BoPartsInfoListRow = partsInfoListDataTable.Rows(i)

            If i = 0 Then
                ' 1行目の情報を保持
                firstNoControl = CType(partsInfoControl.FindControl("SC3190601_NoRow"), HtmlTableCell)
                firstPoControl = CType(partsInfoControl.FindControl("SC3190601_PoNoRow"), HtmlTableCell)
                firstRoControl = CType(partsInfoControl.FindControl("SC3190601_RoNoRow"), HtmlTableCell)
                firstOperationControl = CType(partsInfoControl.FindControl("SC3190601_OperationRow"), HtmlTableCell)
                firstBoId = partsInfoRow.BO_ID
                firstOperation = partsInfoRow.BO_JOB_ID
            Else
                If firstOperation = partsInfoRow.BO_JOB_ID Then
                    ' 作業が変わらない場合
                    operationRowCount = operationRowCount + 1
                    CType(partsInfoControl.FindControl("SC3190601_OperationRow"), HtmlTableCell).Visible = False
                Else
                    ' 作業が変わった場合
                    firstOperationControl = CType(partsInfoControl.FindControl("SC3190601_OperationRow"), HtmlTableCell)
                    firstOperation = partsInfoRow.BO_JOB_ID
                    operationRowCount = 1
                End If

                If firstBoId = partsInfoRow.BO_ID Then
                    ' B/O IDが変わらない場合
                    poRowCount = poRowCount + 1
                    CType(partsInfoControl.FindControl("SC3190601_NoRow"), HtmlTableCell).Visible = False
                    CType(partsInfoControl.FindControl("SC3190601_PoNoRow"), HtmlTableCell).Visible = False
                    CType(partsInfoControl.FindControl("SC3190601_RoNoRow"), HtmlTableCell).Visible = False
                Else
                    ' B/O IDが変わった場合
                    firstNoControl = CType(partsInfoControl.FindControl("SC3190601_NoRow"), HtmlTableCell)
                    firstPoControl = CType(partsInfoControl.FindControl("SC3190601_PoNoRow"), HtmlTableCell)
                    firstRoControl = CType(partsInfoControl.FindControl("SC3190601_RoNoRow"), HtmlTableCell)
                    firstBoId = partsInfoRow.BO_ID
                    poRowCount = 1
                End If

                firstOperationControl.Attributes("rowspan") = operationRowCount
                firstNoControl.Attributes("rowspan") = poRowCount
                firstPoControl.Attributes("rowspan") = poRowCount
                firstRoControl.Attributes("rowspan") = poRowCount
            End If

            ' 情報を表示する
            CType(partsInfoControl.FindControl("BoIdField"), HiddenField).Value = partsInfoRow.BO_ID
            CType(partsInfoControl.FindControl("SC3190601_PartsList_No"), Label).Text = ProcessingDisplayValue(partsInfoRow.No)
            CType(partsInfoControl.FindControl("SC3190601_PartsList_PoNo"), Label).Text = ProcessingDisplayValue(partsInfoRow.PO_NUM)
            CType(partsInfoControl.FindControl("SC3190601_PartsList_RoNo"), Label).Text = ProcessingDisplayValue(partsInfoRow.RO_NUM)
            CType(partsInfoControl.FindControl("SC3190601_PartsList_Operation"), Label).Text = ProcessingDisplayValue(partsInfoRow.JOB_NAME)
            CType(partsInfoControl.FindControl("SC3190601_PartsList_PartsName"), Label).Text = ProcessingDisplayValue(partsInfoRow.PARTS_NAME)
            CType(partsInfoControl.FindControl("SC3190601_PartsList_PartsCode"), Label).Text = ProcessingDisplayValue(partsInfoRow.PARTS_CD)
            CType(partsInfoControl.FindControl("SC3190601_PartsList_Qty"), Label).Text = ProcessingDisplayValue(partsInfoRow.PARTS_AMOUNT)
            CType(partsInfoControl.FindControl("SC3190601_PartsList_OdrDate"), Label).Text = ProcessingDisplayValue(partsInfoRow.ODR_DATE)
            CType(partsInfoControl.FindControl("SC3190601_PartsList_Eta"), Label).Text = ProcessingDisplayValue(partsInfoRow.ARRIVAL_SCHE_DATE)
            If String.Equals(VclPartakeFlgCust, partsInfoRow.VCL_PARTAKE_FLG) Then
                CType(partsInfoControl.FindControl("SC3190601_PartsList_Vcl"), Label).Text = Server.HtmlEncode(WebWordUtility.GetWord(AppId, WordIdVclPartakeFlgCust))
            ElseIf String.Equals(VclPartakeFlgDlr, partsInfoRow.VCL_PARTAKE_FLG) Then
                CType(partsInfoControl.FindControl("SC3190601_PartsList_Vcl"), Label).Text = Server.HtmlEncode(WebWordUtility.GetWord(AppId, WordIdVclPartakeFlgDlr))
            Else
                CType(partsInfoControl.FindControl("SC3190601_PartsList_Vcl"), Label).Text = ProcessingDisplayValue(String.Empty)
            End If
            CType(partsInfoControl.FindControl("SC3190601_PartsList_Apt"), Label).Text = ProcessingDisplayValue(partsInfoRow.CST_APPOINTMENT_DATE)

            ' 背景色判断
            If String.Equals(DelayCustAppointmentDateFlag, partsInfoRow.PO_DELAY_FLAG) Then
                CType(partsInfoControl.FindControl("SC3190601_PartsListRow"), HtmlTableRow).Attributes("class") = "BackColor_Red"
            ElseIf String.Equals(ArrivalCloseJudgementFlag, partsInfoRow.PARTS_DELAY_FLAG) Then
                CType(partsInfoControl.FindControl("SC3190601_PartsListRow"), HtmlTableRow).Attributes("class") = "BackColor_Yellow"
            End If
        Next

    End Sub

    ''' <summary>
    ''' 表示文言の加工処理(文字列)
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Overloads Function ProcessingDisplayValue(ByVal targetValue As String) As String

        ' DB初期値の場合は、未登録用文言を表示
        If String.IsNullOrEmpty(Trim(targetValue)) Then

            Return Server.HtmlEncode(WebWordUtility.GetWord(AppId, WordIdUnregisteredWord))

        End If

        ' DBに登録済みの場合はそのまま返す
        Return Server.HtmlEncode(targetValue)

    End Function

    ''' <summary>
    ''' 表示文言の加工処理(数値)
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Overloads Function ProcessingDisplayValue(ByVal targetValue As Decimal) As String

        ' DB初期値の場合は、未登録用文言を表示
        If 0 = targetValue Then

            Return Server.HtmlEncode(WebWordUtility.GetWord(AppId, WordIdUnregisteredWord))

        End If

        ' DBに登録済みの場合はそのまま返す
        Return Server.HtmlEncode(targetValue.ToString())

    End Function

    ''' <summary>
    ''' 表示文言の加工処理(日付)
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Overloads Function ProcessingDisplayValue(ByVal targetValue As Date) As String

        ' DB初期値の場合は、未登録用文言を表示
        If DateTimeFunc.FormatString("yyyy/MM/dd HH:mm:ss", "1900/01/01 00:00:00") = targetValue Then

            Return Server.HtmlEncode(WebWordUtility.GetWord(AppId, WordIdUnregisteredWord))

        End If

        ' DBに登録済みの場合は、フォーマット変換し返す
        Return Server.HtmlEncode(DateTimeFunc.FormatDate(6, targetValue))

    End Function

#End Region

#Region "文言設定"

    ''' <summary>
    ''' 文言をセットする
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub SetWord()

        Logger.Info("SetWord_Start")

        ' 文言の設定
        Me.SC3190601_Label_Title.Text = Server.HtmlEncode(WebWordUtility.GetWord(AppId, WordIdScreenTitle))
        Me.SC3190601_Label_CurStatus.Text = Server.HtmlEncode(WebWordUtility.GetWord(AppId, WordIdCurrentStatus))
        Me.SC3190601_Label_Colon.Text = Server.HtmlEncode(WebWordUtility.GetWord(AppId, WordIdColonWord))
        Me.SC3190601_Label_POTotal.Text = Server.HtmlEncode(WebWordUtility.GetWord(AppId, WordIdPoTotal))
        Me.SC3190601_Label_PSTotal.Text = Server.HtmlEncode(WebWordUtility.GetWord(AppId, WordIdPartsTotal))
        Me.SC3190601_Label_No.Text = Server.HtmlEncode(WebWordUtility.GetWord(AppId, WordIdListHeaderNo))
        Me.SC3190601_Label_PoNo.Text = Server.HtmlEncode(WebWordUtility.GetWord(AppId, WordIdListHeaderPoNo))
        Me.SC3190601_Label_RoNo.Text = Server.HtmlEncode(WebWordUtility.GetWord(AppId, WordIdListHeaderRoNo))
        Me.SC3190601_Label_Operation.Text = Server.HtmlEncode(WebWordUtility.GetWord(AppId, WordIdListHeaderOperation))
        Me.SC3190601_Label_Parts.Text = Server.HtmlEncode(WebWordUtility.GetWord(AppId, WordIdListHeaderParts))
        Me.SC3190601_Label_Qty.Text = Server.HtmlEncode(WebWordUtility.GetWord(AppId, WordIdListHeaderQty))
        Me.SC3190601_Label_OdrDate.Text = Server.HtmlEncode(WebWordUtility.GetWord(AppId, WordIdListHeaderOrderDate))
        Me.SC3190601_Label_Eta.Text = Server.HtmlEncode(WebWordUtility.GetWord(AppId, WordIdListHeaderEta))
        Me.SC3190601_Label_Vcl.Text = Server.HtmlEncode(WebWordUtility.GetWord(AppId, WordIdListHeaderVehicle))
        Me.SC3190601_Label_Apt.Text = Server.HtmlEncode(WebWordUtility.GetWord(AppId, WordIdListHeaderAptDate))

        Logger.Info("SetWord_End")

    End Sub

#End Region

#Region "設定値取得"

    ''' <summary>
    ''' 店舗設定テーブルより設定値を取得
    ''' </summary>
    ''' <param name="prarmName">パラメータ名</param>
    ''' <returns>設定値</returns>
    ''' <remarks></remarks>
    Private Function GetBranchEnvSetting(ByVal prarmName As String) As String

        Logger.Info("GetBranchEnvSetting_Start")

        ' 設定値取得
        Dim context As StaffContext = StaffContext.Current
        Dim branchEnvSet As New BranchEnvSetting

        Dim branchEnvSetRow As DlrEnvSettingDataSet.DLRENVSETTINGRow = Nothing
        Logger.Info("GetBranchEnvSetting " & "Call_Start GetEnvSetting ParamName[" & prarmName & "]")
        branchEnvSetRow = branchEnvSet.GetEnvSetting(context.DlrCD, context.BrnCD, prarmName)
        Logger.Info("GetBranchEnvSetting " & "Call_End GetEnvSetting GetParamValue[" & branchEnvSetRow.PARAMVALUE & "]")

        Logger.Info("GetBranchEnvSetting_End")

        Return branchEnvSetRow.PARAMVALUE

    End Function
#End Region

#End Region

End Class
