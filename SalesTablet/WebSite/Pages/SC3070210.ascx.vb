'━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
'SC3070210.ascx.vb
'─────────────────────────────────────
'機能： 相談履歴
'補足： 
'作成： 2015/03/17 TCS 鈴木  次世代e-CRB 価格相談履歴参照機能開発
'─────────────────────────────────────

Option Explicit On

Imports Toyota.eCRB.SystemFrameworks.Web
Imports Toyota.eCRB.SystemFrameworks.Web.Controls
Imports Toyota.eCRB.SystemFrameworks.Core
Imports System.Globalization
Imports System.Reflection.MethodBase
Imports System.Data
Imports Toyota.eCRB.Estimate.Quotation.BizLogic
Imports Toyota.eCRB.Estimate.Quotation.DataAccess
Imports Toyota.eCRB.iCROP.BizLogic.SC3070201
Imports Toyota.eCRB.SystemFrameworks.Core.iCROP.BizLogic


Partial Class Pages_SC3070210
    Inherits System.Web.UI.UserControl

#Region "定数"
    ''' <summary>
    ''' 見積ID（カンマ区切り）
    ''' </summary>
    ''' <remarks></remarks>
    Private Const SessionKey_EstimateId As String = "EstimateId"

    ''' <summary>
    ''' 現在選択中の見積り（インデックス番号）
    ''' </summary>
    ''' <remarks></remarks>
    Private Const SessionKey_SelectedEstimateIndex As String = "SelectedEstimateIndex"

    ''' <summary>
    ''' 注文承認を表示中フラグ
    ''' </summary>
    ''' <remarks></remarks>
    Private Const SessionKey_ApprovalActive As String = "ApprovalActive"

    ''' <summary>
    ''' 価格相談回答を表示中フラグ
    ''' </summary>
    ''' <remarks>
    ''' </remarks>
    Private Const SessionKey_PriceApproval As String = "EstimateMode.PriceApproval"

#End Region

#Region "イベント"

    Protected Sub Pages_SC3070210_Load(sender As Object, e As System.EventArgs) Handles Me.Load
        Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}_Start", GetCurrentMethod.Name), True)

        SC3070210_ShowRecentLink.Text = WebWordUtility.GetWord("SC3070201", 71012)
        SC3070210_ShowAllLink.Text = WebWordUtility.GetWord("SC3070201", 71013)

        Dim showingAll As String = Request.Form("__EVENTARGUMENT")
        If showingAll <> Nothing Then
            SC3070210_IsShowingAll.Value = showingAll
        End If

        Dim estimateId As Long = GetEstimateId()
        Dim bizLogic As New SC3070210BusinessLogic

        If estimateId = 0 Then
            'システムエラー（表示しない）
            SC3070210_CommentHistoryPanel.Visible = False
        ElseIf GetPageInterface().OperationLockedBypass() Then
            'ロック中は表示しない
            SC3070210_CommentHistoryPanel.Visible = False
        ElseIf (bizLogic.IsBookedVehicleDelivered(estimateId)) Then
            '納車まで完了している場合は表示しない
            SC3070210_CommentHistoryPanel.Visible = False
        Else
            '表示する
            LoadComments()
        End If

        Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}_End", GetCurrentMethod.Name), True)
    End Sub

#End Region

#Region "内部ロジック"

    Private Function GetEstimateId() As Long
        '見積管理ID
        Dim estimateIds As String = String.Empty
        If Me.ContainsKey(ScreenPos.Current, SessionKey_EstimateId) Then
            estimateIds = CType(Me.GetValue(ScreenPos.Current, SessionKey_EstimateId, False), String)
        End If

        '選択している見積IDのIndex
        Dim selectedEstimateIndex As Long = 0
        If Me.ContainsKey(ScreenPos.Current, SessionKey_SelectedEstimateIndex) Then
            selectedEstimateIndex = CType(Me.GetValue(ScreenPos.Current, SessionKey_SelectedEstimateIndex, False), Long)
        End If

        '選択している見積ID
        Dim estimetaIdList = estimateIds.Split(","c)
        If (estimetaIdList.Count <= selectedEstimateIndex) Then
            Logger.Error("Couldn't get ESTIMATEID argument")
            Return 0
        Else
            Return CType(estimetaIdList(selectedEstimateIndex), Long)
        End If
    End Function

    Private Sub LoadComments()
        Dim estimateId As Long = GetEstimateId()
        If (estimateId = 0) Then
            Return
        End If

        Dim isManager As Boolean
        If (StaffContext.Current.OpeCD = Operation.SSM) Then
            'SCM
            isManager = True
        Else
            'SCもしくはSC(TL)
            isManager = False
            If Me.ContainsKey(ScreenPos.Current, SessionKey_PriceApproval) Then
                Dim displayFlg As String = CType(Me.GetValue(ScreenPos.Current, SessionKey_PriceApproval, False), String)
                isManager = (displayFlg = "1")
            End If
            If Me.ContainsKey(ScreenPos.Current, SessionKey_ApprovalActive) Then
                Dim displayFlg As String = CType(Me.GetValue(ScreenPos.Current, SessionKey_ApprovalActive, False), String)
                isManager = (displayFlg = "1")
            End If
        End If

        Dim requestPrefix As String = "R"
        Dim responsePrefix As String = "L"
        If (isManager) Then
            requestPrefix = "L"
            responsePrefix = "R"
        End If

        Dim data As New DataTable
        data.Columns.Add("REQUESTDATE", GetType(System.DateTime))
        data.Columns.Add("L_CSSCLASS")
        data.Columns.Add("L_USERNAME")
        data.Columns.Add("L_COMMENTTITLE")
        data.Columns.Add("L_COMMENT")
        data.Columns.Add("L_COMMENTDATE")
        data.Columns.Add("R_CSSCLASS")
        data.Columns.Add("R_USERNAME")
        data.Columns.Add("R_COMMENTTITLE")
        data.Columns.Add("R_COMMENT")
        data.Columns.Add("R_COMMENTDATE")

        data.DefaultView.Sort = "REQUESTDATE"

        Dim bizLogic As New SC3070210BusinessLogic
        For Each comment As SC3070210DataSet.SC3070210DISCOUNTAPPROVALRow In bizLogic.GetDiscountApproval(estimateId)
            Dim dataRow As DataRow = data.NewRow()

            dataRow("REQUESTDATE") = comment.REQUESTDATE

            dataRow(requestPrefix & "_CSSCLASS") = "SC3070210_Comment_Request"
            dataRow(requestPrefix & "_USERNAME") = comment.STAFFNAME
            dataRow(requestPrefix & "_COMMENTTITLE") = WebWordUtility.GetWord(71002)
            dataRow(requestPrefix & "_COMMENTDATE") = DateTimeFunc.FormatElapsedDate(ElapsedDateFormat.Normal, comment.REQUESTDATE, StaffContext.Current.DlrCD)
            dataRow(requestPrefix & "_COMMENT") = String.Format("<div>{0}{1}</div><div>{2}{3}</div><div class='memo'>{4}</div>", _
                                                 WebWordUtility.GetWord(71003), comment.REQUESTPRICE, _
                                                 WebWordUtility.GetWord(71004), Server.HtmlEncode(comment.REASON), _
                                                 Server.HtmlEncode(comment.STAFFMEMO).Replace(vbLf, vbLf & "<br>"))

            If comment.RESPONSEFLG = "1" Then
                dataRow(responsePrefix & "_CSSCLASS") = "SC3070210_Comment_Approved"
                dataRow(responsePrefix & "_USERNAME") = comment.MANAGERNAME
                dataRow(responsePrefix & "_COMMENTTITLE") = WebWordUtility.GetWord(71005)
                dataRow(responsePrefix & "_COMMENTDATE") = DateTimeFunc.FormatElapsedDate(ElapsedDateFormat.Normal, comment.APPROVEDDATE, StaffContext.Current.DlrCD)
                dataRow(responsePrefix & "_COMMENT") = String.Format("<div>{0}{1}</div><div class='memo'>{2}</div>", _
                                                     WebWordUtility.GetWord(71006), comment.APPROVEDPRICE, _
                                                     Server.HtmlEncode(comment.MANAGERMEMO).Replace(vbLf, vbLf & "<br>"))
            Else
                dataRow(responsePrefix & "_CSSCLASS") = "SC3070210_Comment_None"
                dataRow(responsePrefix & "_USERNAME") = ""
                dataRow(responsePrefix & "_COMMENTTITLE") = ""
                dataRow(responsePrefix & "_COMMENTDATE") = ""
                dataRow(responsePrefix & "_COMMENT") = ""
            End If

            data.Rows.Add(dataRow)

            If (comment.CANCELFLG = "1") Then
                Dim cancelRow As DataRow = data.NewRow()

                cancelRow("REQUESTDATE") = comment.CANCELDATE

                cancelRow(requestPrefix & "_CSSCLASS") = "SC3070210_Comment_Request"
                cancelRow(requestPrefix & "_USERNAME") = comment.STAFFNAME
                cancelRow(requestPrefix & "_COMMENTTITLE") = WebWordUtility.GetWord(71002) & WebWordUtility.GetWord(71010)
                cancelRow(requestPrefix & "_COMMENTDATE") = DateTimeFunc.FormatElapsedDate(ElapsedDateFormat.Normal, comment.CANCELDATE, StaffContext.Current.DlrCD)
                cancelRow(requestPrefix & "_COMMENT") = ""

                cancelRow(responsePrefix & "_CSSCLASS") = "SC3070210_Comment_None"
                cancelRow(responsePrefix & "_USERNAME") = ""
                cancelRow(responsePrefix & "_COMMENTTITLE") = ""
                cancelRow(responsePrefix & "_COMMENTDATE") = ""
                cancelRow(responsePrefix & "_COMMENT") = ""

                data.Rows.Add(cancelRow)
            End If
        Next

        For Each comment As SC3070210DataSet.SC3070210CONTRACTAPPROVALRow In bizLogic.GetContracatApproval(estimateId)
            Dim dataRow As DataRow = data.NewRow()

            dataRow("REQUESTDATE") = comment.REQUESTDATE

            dataRow(requestPrefix & "_CSSCLASS") = "SC3070210_Comment_Request"
            dataRow(requestPrefix & "_USERNAME") = comment.STAFFNAME
            dataRow(requestPrefix & "_COMMENTTITLE") = WebWordUtility.GetWord(71007)
            dataRow(requestPrefix & "_COMMENTDATE") = DateTimeFunc.FormatElapsedDate(ElapsedDateFormat.Normal, comment.REQUESTDATE, StaffContext.Current.DlrCD)
            dataRow(requestPrefix & "_COMMENT") = String.Format("<div class='memo'>{0}</div>", _
                                                  Server.HtmlEncode(comment.STAFFMEMO).Replace(vbLf, vbLf & "<br>"))

            If (comment.RESPONSEFLG = "1") Then
                dataRow(responsePrefix & "_CSSCLASS") = "SC3070210_Comment_Approved"
                dataRow(responsePrefix & "_USERNAME") = comment.MANAGERNAME
                dataRow(responsePrefix & "_COMMENTTITLE") = WebWordUtility.GetWord(71008) & WebWordUtility.GetWord(71011)
                dataRow(responsePrefix & "_COMMENTDATE") = DateTimeFunc.FormatElapsedDate(ElapsedDateFormat.Normal, comment.APPROVEDDATE, StaffContext.Current.DlrCD)
                dataRow(responsePrefix & "_COMMENT") = String.Format("<div class='memo'>{0}</div>", _
                                                     Server.HtmlEncode(comment.MANAGERMEMO).Replace(vbLf, vbLf & "<br>"))
            ElseIf (comment.RESPONSEFLG = "2") Then
                dataRow(responsePrefix & "_CSSCLASS") = "SC3070210_Comment_Rejected"
                dataRow(responsePrefix & "_USERNAME") = comment.MANAGERNAME
                dataRow(responsePrefix & "_COMMENTTITLE") = WebWordUtility.GetWord(71008) & WebWordUtility.GetWord(71009)
                dataRow(responsePrefix & "_COMMENTDATE") = DateTimeFunc.FormatElapsedDate(ElapsedDateFormat.Normal, comment.APPROVEDDATE, StaffContext.Current.DlrCD)
                dataRow(responsePrefix & "_COMMENT") = String.Format("<div class='memo'>{0}</div>", _
                                                     Server.HtmlEncode(comment.MANAGERMEMO).Replace(vbLf, vbLf & "<br>"))
            Else
                dataRow(responsePrefix & "_CSSCLASS") = "SC3070210_Comment_None"
                dataRow(responsePrefix & "_USERNAME") = ""
                dataRow(responsePrefix & "_COMMENTTITLE") = ""
                dataRow(responsePrefix & "_COMMENTDATE") = ""
                dataRow(responsePrefix & "_COMMENT") = ""
            End If

            data.Rows.Add(dataRow)

            If (comment.CANCELFLG = "1") Then
                Dim cancelRow As DataRow = data.NewRow()

                cancelRow("REQUESTDATE") = comment.CANCELDATE

                cancelRow(requestPrefix & "_CSSCLASS") = "SC3070210_Comment_Request"
                cancelRow(requestPrefix & "_USERNAME") = comment.STAFFNAME
                cancelRow(requestPrefix & "_COMMENTTITLE") = WebWordUtility.GetWord(71007) & WebWordUtility.GetWord(71010)
                cancelRow(requestPrefix & "_COMMENTDATE") = DateTimeFunc.FormatElapsedDate(ElapsedDateFormat.Normal, comment.CANCELDATE, StaffContext.Current.DlrCD)
                cancelRow(requestPrefix & "_COMMENT") = ""

                cancelRow(responsePrefix & "_CSSCLASS") = "SC3070210_Comment_None"
                cancelRow(responsePrefix & "_USERNAME") = ""
                cancelRow(responsePrefix & "_COMMENTTITLE") = ""
                cancelRow(responsePrefix & "_COMMENTDATE") = ""
                cancelRow(responsePrefix & "_COMMENT") = ""

                data.Rows.Add(cancelRow)
            End If
        Next

        If data.Rows.Count <= 2 Then
            SC3070210_ShowRecentLink.Visible = False
            SC3070210_ShowAllLink.Visible = False
        Else
            If (SC3070210_IsShowingAll.Value = "False") Then
                SC3070210_ShowRecentLink.Visible = False
                SC3070210_ShowAllLink.Visible = True
                While (2 < data.Rows.Count)
                    data.DefaultView(0).Delete()
                End While
            Else
                SC3070210_ShowRecentLink.Visible = True
                SC3070210_ShowAllLink.Visible = False
            End If
        End If

        data.AcceptChanges()

        CommentRepeater.DataSource = data
        CommentRepeater.DataBind()

        '履歴が１件以上存在する場合のみ表示
        SC3070210_CommentHistoryPanel.Visible = (0 < data.Rows.Count)
    End Sub

#End Region


#Region " ページクラス処理のバイパス処理 "

    ''' <summary>
    ''' GetValue関数のバイパス
    ''' </summary>
    ''' <param name="pos">ポジジョン</param>
    ''' <param name="key">検索キー</param>
    ''' <param name="removeFlg">削除フラグ</param>
    ''' <returns>値</returns>
    ''' <remarks></remarks>
    Private Function GetValue(ByVal pos As ScreenPos, ByVal key As String, ByVal removeFlg As Boolean) As Object
        Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}_Start", GetCurrentMethod.Name), True)
        Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}_End", GetCurrentMethod.Name), True)
        Return GetPageInterface().GetValueBypass(pos, key, removeFlg)
    End Function

    ''' <summary>
    ''' ContainsKey関数のバイパス
    ''' </summary>
    ''' <param name="pos">ポジジョン</param>
    ''' <param name="key">検索キー</param>
    ''' <returns>値</returns>
    ''' <remarks></remarks>
    Private Function ContainsKey(ByVal pos As ScreenPos, ByVal key As String) As Object
        Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}_Start", GetCurrentMethod.Name), True)
        Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}_End", GetCurrentMethod.Name), True)
        Return GetPageInterface().ContainsKeyBypass(pos, key)
    End Function

    ''' <summary>
    ''' 親ページのインターフェース取得
    ''' </summary>
    ''' <returns>親ページのIEstimateInfoControl</returns>
    ''' <remarks></remarks>
    Private Function GetPageInterface() As IEstimateInfoControl
        Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}_Start", GetCurrentMethod.Name), True)
        Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}_End", GetCurrentMethod.Name), True)
        Return CType(Me.Page, IEstimateInfoControl)
    End Function

#End Region

End Class
