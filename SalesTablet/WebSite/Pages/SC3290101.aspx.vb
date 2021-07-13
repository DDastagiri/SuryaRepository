'━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
'SC3290101.aspx.vb
'─────────────────────────────────────
'機能： 異常リスト
'補足： 
'作成： 2014/06/13 TMEJ y.gotoh
'更新： 2015/01/30 TMEJ y.gotoh 異常リストのリンク表示変更対応 $01
'─────────────────────────────────────

Imports Toyota.eCRB.SystemFrameworks.Core
Imports System.Globalization
Imports Toyota.eCRB.SalesManager.IrregularControl.DataAccess
Imports Toyota.eCRB.SalesManager.IrregularControl.BizLogic
Imports Toyota.eCRB.CommonUtility.BizLogic

''' <summary>
''' 異常リスト
''' </summary>
''' <remarks></remarks>
Partial Class Pages_SC3290101
    Inherits BasePage

#Region "定数"

    ''' <summary>
    ''' 機能ID
    ''' </summary>
    ''' <remarks></remarks>
    Private Const DisplayId As String = "SC3290101"

    ''' <summary>
    ''' 文言：※
    ''' </summary>
    ''' <remarks></remarks>
    Private Const DisplayNoMarker As String = "5"

    ''' <summary>
    ''' 異常分類コード：目標未達
    ''' </summary>
    ''' <remarks></remarks>
    Private Const IrregClassCdGoalNotAchieved As String = "00"

    ''' <summary>
    ''' 異常分類コード：担当スタッフ未振当て
    ''' </summary>
    ''' <remarks></remarks>
    Private Const IrregClassCdFuriateStaffNot As String = "10"

    ''' <summary>
    ''' 異常分類コード：計画異常
    ''' </summary>
    ''' <remarks></remarks>
    Private Const IrregClassCdPlanGap As String = "20"

    ''' <summary>
    ''' 異常項目コード：顧客担当
    ''' </summary>
    ''' <remarks></remarks>
    Private Const IrregItemCdCustomerRepresentative As String = "01"

    ''' <summary>
    ''' 担当未振当て処理タイプ：顧客担当未振当てのみ
    ''' </summary>
    ''' <remarks></remarks>
    Private Const FuriateStaffNotTypeCust As String = "1"

    ''' <summary>
    ''' 担当未振当て処理タイプ：活動担当未振当てのみ
    ''' </summary>
    ''' <remarks></remarks>
    Private Const FuriateStaffNotTypeActivity As String = "2"

    ''' <summary>
    ''' 担当未振当て処理タイプ：顧客担当未振当て、活動担当未振当て両方
    ''' </summary>
    ''' <remarks></remarks>
    Private Const FuriateStaffNotTypeBoth As String = "3"

#End Region

#Region "メンバ変数"

    ''' <summary>
    ''' 処理中件数
    ''' </summary>
    ''' <remarks></remarks>
    Private processingCount As Integer

#End Region

#Region "イベント定義"

    ''' <summary>
    ''' ページロード時の処理
    ''' </summary>
    ''' <param name="sender">イベント発生元</param>
    ''' <param name="e">イベントデータ</param>
    ''' <remarks></remarks>
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Logger.Info("SC3290101_Page_Load_Start")

        ' PostBack時、初期表示処理は行わない。
        If Me.IsPostBack Then
            Logger.Info("SC3290101_Page_Load_End PostBack")
            Return
        End If

        'スタッフ情報を取得
        Logger.Info("Page_Load_001 " & "Call_Start StaffContext.Current")
        Dim staffInfo As StaffContext = StaffContext.Current
        Logger.Info("Page_Load_001 " & "Call_End   StaffContext.Current")

        Dim logic As SC3290101BusinessLogic = New SC3290101BusinessLogic

        '更新日時の取得
        Dim updateTime As Date = logic.GetUpdatetime(staffInfo.DlrCD, staffInfo.BrnCD)

        'DB初期値の場合、更新日時は表示しない
        If DateTimeFunc.FormatString("yyyy/MM/dd HH:mm:ss", "1900/01/01 00:00:00") <> updateTime Then
            Dim updateTimeText As New StringBuilder
            With updateTimeText
                .Append(DateTimeFunc.FormatDate(11, updateTime))
                .Append(" ")
                .Append(DateTimeFunc.FormatDate(14, updateTime))
            End With

            Me.SC3290101_TempLastUpdateTime.Value = updateTimeText.ToString()
        End If

        '異常情報一覧の取得
        Dim irregularListDt As SC3290101DataSet.IrregularInfoDataTable
        Dim nowDate As Date = DateTimeFunc.Now(staffInfo.DlrCD)
        Dim todayDate As New Date(nowDate.Year, nowDate.Month, nowDate.Day, 0, 0, 0)
        irregularListDt = logic.GetIrregularList(staffInfo.DlrCD, staffInfo.BrnCD, todayDate)

        Me.SC3290101_IrregularListRepeater.DataSource = irregularListDt
        Me.SC3290101_IrregularListRepeater.DataBind()

        'マーカーを取得
        Dim marker As String = Server.HtmlEncode(WebWordUtility.GetWord(DisplayId, DisplayNoMarker))

        ' 件数分表示する
        For i = 0 To SC3290101_IrregularListRepeater.Items.Count - 1

            Dim irregular As Control = SC3290101_IrregularListRepeater.Items(i)
            Dim irregularInfoRow As SC3290101DataSet.IrregularInfoRow = irregularListDt.Rows(i)

            '情報を表示する
            Me.ShowIrregularityList(irregular, irregularInfoRow, marker)

        Next

        Logger.Info("SC3290101_Page_Load_End")

        'スクリプトの登録
        JavaScriptUtility.RegisterStartupFunctionCallScript(Me.Page, "initDisplaySC3290101", "startup")
    End Sub

    ''' <summary>
    ''' 担当未割当件数の処理
    ''' </summary>
    ''' <param name="sender">イベント発生元</param>
    ''' <param name="e">イベントデータ</param>
    ''' <remarks></remarks>
    Protected Sub SC3290101_FuriateStaffNotUpdateButton_Click(ByVal sender As Object, ByVal e As System.EventArgs) _
        Handles SC3290101_FuriateStaffNotUpdateButton.Click

        Logger.Info("SC3290101_FuriateStaffNotUpdateButton_Click Start")

        'スタッフ情報を取得
        Logger.Info("SC3290101_FuriateStaffNotUpdateButton_Click_001 " & "Call_Start StaffContext.Current")
        Dim staffInfo As StaffContext = StaffContext.Current
        Logger.Info("SC3290101_FuriateStaffNotUpdateButton_Click_001 " & "Call_End   StaffContext.Current")

        'スレッドに渡す引数を作成
        Dim threadArgs() As Object = New Object(3) {}
        threadArgs(0) = HttpContext.Current
        threadArgs(1) = staffInfo.DlrCD
        threadArgs(2) = staffInfo.BrnCD

        Dim furiateStaffNotType As String = Me.SC3290101_FuriateStaffNotType.Value

        Dim t1 As New System.Threading.Thread(New System.Threading.ParameterizedThreadStart(AddressOf GetStaffAssignToCustCount))
        Dim t2 As New System.Threading.Thread(New System.Threading.ParameterizedThreadStart(AddressOf GetUnallocatedActivityCount))

        '処理中件数
        Me.processingCount = 0

        If FuriateStaffNotTypeCust.Equals(furiateStaffNotType) Then
            processingCount = 1
            t1.Start(threadArgs)
        ElseIf FuriateStaffNotTypeActivity.Equals(furiateStaffNotType) Then
            processingCount = 1
            t2.Start(threadArgs)
        ElseIf FuriateStaffNotTypeBoth.Equals(furiateStaffNotType) Then
            processingCount = 2
            t1.Start(threadArgs)
            t2.Start(threadArgs)
        End If

        Do Until processingCount = 0
            System.Threading.Thread.Sleep(100)
        Loop

        Logger.Info("SC3290101_FuriateStaffNotUpdateButton_Click End")

        'スクリプトの登録
        JavaScriptUtility.RegisterStartupFunctionCallScript(Me.Page, "setFuriateStaffNotCount", "startup")
    End Sub

#End Region

#Region "非公開メソッド"

    ''' <summary>
    ''' 異常リストの表示
    ''' </summary>
    ''' <param name="irregularControl">HTMLコントロール</param>
    ''' <param name="irregularInfoRow">異常情報</param>
    ''' <param name="marker">マーカー</param>
    ''' <remarks></remarks>
    Private Sub ShowIrregularityList(ByVal irregularControl As Control, _
                                 ByVal irregularInfoRow As SC3290101DataSet.IrregularInfoRow, _
                                 ByVal marker As String)

        'For文で何度も呼ばれるため、ログ出力はしない

        Dim irregClassCode As String = irregularInfoRow.IRREG_CLASS_CD.ToString(CultureInfo.CurrentCulture)
        Dim irregItemCode As String = String.Empty
        If IrregClassCdPlanGap.Equals(irregClassCode) Then
            ' 計画異常の場合は、連携用コードをセット
            irregItemCode = irregularInfoRow.RELATION_CD.ToString(CultureInfo.CurrentCulture)
        Else
            ' 計画異常以外の場合は、異常項目コードをセット
            irregItemCode = irregularInfoRow.IRREG_ITEM_CD.ToString(CultureInfo.CurrentCulture)
        End If

        'Hiddenに異常分類コード、異常項目コードを設定
        CType(irregularControl.FindControl("IrregClassCd"), HiddenField).Value = irregClassCode
        CType(irregularControl.FindControl("IrregItemCd"), HiddenField).Value = irregItemCode

        '異常項目名表示名称を設定
        If String.IsNullOrEmpty(irregularInfoRow.IRREG_LIST_DISP_NAME) Then
            CType(irregularControl.FindControl("SC3290101_IrregularityItem"), Label).Text = Server.HtmlEncode("-")
            CType(irregularControl.FindControl("SC3290101_IrregularityItem"), Label).Attributes("style") = "text-decoration:none;"
        Else
            CType(irregularControl.FindControl("SC3290101_IrregularityItem"), Label).Text = Server.HtmlEncode(irregularInfoRow.IRREG_LIST_DISP_NAME)
        End If

        '異常分類により、異常スタッフ数、異常件数の表示非表示を切り替える
        If IrregClassCdGoalNotAchieved.Equals(irregClassCode) Then

            '"目標未達"の場合、異常スタッフ数のみ表示
            CType(irregularControl.FindControl("SC3290101_NoOfStaffs"), Label).Text = Server.HtmlEncode(irregularInfoRow.IRREG_STAFF_COUNT)
            CType(irregularControl.FindControl("SC3290101_Marker"), Label).Text = marker
            irregularControl.FindControl("SC3290101_FuriateStaffNotDiv").Visible = False

            '$01 異常リストのリンク表示変更対応 START
            '異常項目名にアンダーラインを設定
            CType(irregularControl.FindControl("SC3290101_IrregularityItem"), Label).Attributes("class") += " Underline"
            '$01 異常リストのリンク表示変更対応 END
        ElseIf IrregClassCdFuriateStaffNot.Equals(irregClassCode) Then

            '"担当スタッフ未振当て"の場合、異常件数のみ表示
            '件数は非同期で取得するため、ここでは設定しない
            CType(irregularControl.FindControl("SC3290101_Marker"), Label).Visible = False
            CType(irregularControl.FindControl("SC3290101_NoOfIrregularities"), Label).Visible = False

            If IrregItemCdCustomerRepresentative.Equals(irregItemCode) Then

                '顧客担当未振当ての場合
                irregularControl.FindControl("SC3290101_UnallocatedActivityCountPanel").Visible = False

            Else
                '活動担当未振当ての場合
                irregularControl.FindControl("SC3290101_StaffAssignToCustCountPanel").Visible = False
            End If

        Else

            '"計画異常"or"活動遅れ"の場合、異常スタッフ数、異常件数を表示
            CType(irregularControl.FindControl("SC3290101_NoOfStaffs"), Label).Text = Server.HtmlEncode(irregularInfoRow.IRREG_STAFF_COUNT)
            CType(irregularControl.FindControl("SC3290101_NoOfIrregularities"), Label).Text = Server.HtmlEncode(irregularInfoRow.IRREG_COUNT)
            CType(irregularControl.FindControl("SC3290101_Marker"), Label).Visible = False
            irregularControl.FindControl("SC3290101_FuriateStaffNotDiv").Visible = False

            '$01 異常リストのリンク表示変更対応 START
            '異常項目名にアンダーラインを設定
            CType(irregularControl.FindControl("SC3290101_IrregularityItem"), Label).Attributes("class") += " Underline"
            '$01 異常リストのリンク表示変更対応 END
        End If

    End Sub

    ''' <summary>
    ''' 顧客担当未割当件数取得の処理
    ''' </summary>
    ''' <param name="parentContext"></param>
    ''' <remarks></remarks>
    Private Sub GetStaffAssignToCustCount(ByVal parentContext As Object)

        Dim threadArgs() As Object = CType(parentContext, Object())

        'HttpContextを引き継ぐ
        HttpContext.Current = CType(threadArgs(0), HttpContext)
        Dim dealerCode As String = CType(threadArgs(1), String)
        Dim branchCode As String = CType(threadArgs(2), String)

        Logger.Info("GetStaffAssignToCustCount_Start")

        Dim logic As UnallocatedCustomerBusinessLogic = New UnallocatedCustomerBusinessLogic

        '顧客担当未割当件数取得
        Dim staffAssignToCustCount As Integer = logic.GetStaffAssignToCustCount(dealerCode, branchCode)

        If staffAssignToCustCount = 0 Then
            '0件の場合はハイフンを表示
            Me.SC3290101_TempStaffAssignToCustCount.Text = Server.HtmlEncode("-")
        Else
            Me.SC3290101_TempStaffAssignToCustCount.Text = staffAssignToCustCount.ToString(CultureInfo.CurrentCulture)
        End If


        '処理中件数を変更
        Me.processingCount -= 1
        Logger.Info("GetStaffAssignToCustCount_End")

    End Sub

    ''' <summary>
    ''' 活動担当未割当件数取得の処理
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub GetUnallocatedActivityCount(ByVal parentContext As Object)

        Dim threadArgs() As Object = CType(parentContext, Object())

        'HttpContextを引き継ぐ
        HttpContext.Current = CType(threadArgs(0), HttpContext)
        Dim dealerCode As String = CType(threadArgs(1), String)
        Dim branchCode As String = CType(threadArgs(2), String)

        Logger.Info("GetUnallocatedActivityCount_Start")

        Dim logic As UnallocatedActivityBusinessLogic = New UnallocatedActivityBusinessLogic

        '活動担当未割当件数取得
        Dim unallocatedActivityCount As Integer = logic.GetUnallocatedActivityCount(dealerCode, branchCode)

        If unallocatedActivityCount = 0 Then
            '0件の場合はハイフンを表示
            Me.SC3290101_TempUnallocatedActivityCount.Text = Server.HtmlEncode("-")
        Else
            Me.SC3290101_TempUnallocatedActivityCount.Text = unallocatedActivityCount.ToString(CultureInfo.CurrentCulture)
        End If

        '処理中件数を変更
        Me.processingCount -= 1
        Logger.Info("GetUnallocatedActivityCount_End")
    End Sub
#End Region

End Class

