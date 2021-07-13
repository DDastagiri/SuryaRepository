Imports System.Reflection
Imports System.Web.Services
Imports System.Globalization
Imports Toyota.eCRB.SystemFrameworks.Core
Imports Toyota.eCRB.SystemFrameworks.Core.iCROP.BizLogic
Imports Toyota.eCRB.Common.MainMenu.BizLogic
Imports Toyota.eCRB.Common.MainMenu.DataAccess
Imports Toyota.eCRB.Common.MainMenu.DataAccess.SC3010203DataSet
Imports Toyota.eCRB.SystemFrameworks.Core.iCROP.DataAccess

Partial Class Pages_SC3100302
    Inherits System.Web.UI.UserControl

    Private NextDisplayDate As Date
#Region "来店実績"


    ''' <summary>
    ''' 来店実績取得
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub GetVisitActualList(ByVal sender As Object, ByVal e As System.EventArgs) Handles VisitSalesTrigger.Click

        ' ======================== ログ出力 開始 ========================
        Logger.Info(String.Format(CultureInfo.InvariantCulture, " {0}_Start", MethodBase.GetCurrentMethod.Name))
        ' ======================== ログ出力 終了 ========================

        NextDisplayDate = targetDay()

        Dim staffInfo As StaffContext = StaffContext.Current()
        Dim bizLogic As SC3010203BusinessLogic = New SC3010203BusinessLogic(staffInfo.DlrCD, staffInfo.BrnCD, staffInfo.Account, staffInfo.OpeCD)
        Dim visitActualList As SC3010203DataSet.SC3010203VisitActualDataTable
        Dim script As New StringBuilder

        If String.Equals(Me.WalkinCompFlg.Value, "0") Then
            '来店実績一覧取得
            visitActualList = bizLogic.SelectVisitActualList("1", Nothing, Nothing)
        ElseIf String.Equals(Me.WalkinCompFlg.Value, "2") Then

            Dim startDatetime = NextDisplayDate
            Dim endDatetime = New Date(NextDisplayDate.Year, NextDisplayDate.Month, NextDisplayDate.Day, 23, 59, 59)

            visitActualList = bizLogic.SelectVisitActualList("2", startDatetime, endDatetime)
        Else
            visitActualList = New SC3010203DataSet.SC3010203VisitActualDataTable
        End If

        Dim VisitSalesTotalCount As Integer
        Dim VisitSalesDueCount As Integer

        VisitSalesTotalCount = visitActualList.Count

        For Each dr As SC3010203DataSet.SC3010203VisitActualRow In visitActualList
            '1: 商談、2: 納車作業
            If String.Equals(dr.CST_SERVICE_TYPE, "1") Then
                dr.CST_SERVICE_NAME = HttpUtility.HtmlEncode(WebWordUtility.GetWord(22))
            ElseIf String.Equals(dr.CST_SERVICE_TYPE, "2") Then
                dr.CST_SERVICE_NAME = HttpUtility.HtmlEncode(WebWordUtility.GetWord(23))
            Else
                dr.CST_SERVICE_NAME = HttpUtility.HtmlEncode(WebWordUtility.GetWord(25))
            End If

            '遅れの件数をカウント
            If String.Equals(dr.DELAY_STATUS, SC3010203BusinessLogic.DELAY_STATUS_DELAY) Or String.Equals(dr.DELAY_STATUS, SC3010203BusinessLogic.DELAY_STATUS_DUE) Then
                VisitSalesDueCount = VisitSalesDueCount + 1
            End If
        Next

        '件数を設定
        If String.Equals(Me.WalkinCompFlg.Value, "0") Then
            Dim Count As New StringBuilder
            Count.Append(VisitSalesDueCount)
            Count.Append(WebWordUtility.GetWord(24))
            Count.Append(VisitSalesTotalCount)
            Me.VisitSalesCount.Text = Count.ToString
        ElseIf String.Equals(Me.WalkinCompFlg.Value, "2") Then
            Me.VisitSalesCount.Text = VisitSalesTotalCount
        Else
            Me.VisitSalesCount.Text = "0"
        End If

        Me.VisitSalesCount.Visible = True

        'データを設定
        Me.ActualVisitRepeater.DataSource = visitActualList
        Me.ActualVisitRepeater.DataBind()

        'スクロールバーを表示
        script.AppendLine("$('#VisitBoxIn').fingerScroll();")
        script.AppendLine("$('#VisitBoxIn #VisitActualRow:last-child').css('padding-bottom','10px');")
        script.AppendLine("$('.colorDue .SCMainChip').css('background',getVisitSalesTipColor());")
        script.AppendLine("ToDoDispChange();")
        script.AppendLine("$('.clearboth').removeClass('loadingVisitActual')")

        ScriptManager.RegisterClientScriptBlock(Me.VisitSales, Me.GetType, "", script.ToString, True)

        ' ======================== ログ出力 開始 ========================
        Logger.Info(String.Format(CultureInfo.InvariantCulture, " {0}_End", MethodBase.GetCurrentMethod.Name))
        ' ======================== ログ出力 終了 ========================

    End Sub


    ''' <summary>
    ''' 来店実績データ設定
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub ActualVisitRepeater_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.RepeaterItemEventArgs) Handles ActualVisitRepeater.ItemDataBound

        ' ======================== ログ出力 開始 ========================
        Logger.Info(String.Format(CultureInfo.InvariantCulture, " {0}_Start", MethodBase.GetCurrentMethod.Name))
        ' ======================== ログ出力 終了 ========================

        If e.Item.ItemType = ListItemType.Item _
                 OrElse e.Item.ItemType = ListItemType.AlternatingItem Then

            Dim view As Data.DataView = DirectCast(e.Item.DataItem.DataView, Data.DataView)
            Dim row As SC3010203DataSet.SC3010203VisitActualRow = DirectCast(e.Item.DataItem.row, SC3010203DataSet.SC3010203VisitActualRow)
            Dim onlineStatusIconArea As HtmlGenericControl = DirectCast(e.Item.FindControl("OnlineStatusIconArea"), HtmlGenericControl)
            Dim VisitActualRow As HtmlGenericControl = DirectCast(e.Item.FindControl("VisitActualRow"), HtmlGenericControl)
            Dim TempStaffOperationIcon As HtmlImage = DirectCast(e.Item.FindControl("TempStaffOperationIcon"), HtmlImage)
            Dim NextActivityIcon As HtmlImage = DirectCast(e.Item.FindControl("NextActivityIcon"), HtmlImage)

            VisitActualRow.Attributes("Class") = ""

            'チップの色を指定
            If String.Equals(row.REGISTFLG, "0") Then
                Select Case row.DELAY_STATUS
                    Case SC3010203BusinessLogic.DELAY_STATUS_DELAY
                        '遅れ
                        AddCssClass(VisitActualRow, "colorDelay")
                    Case SC3010203BusinessLogic.DELAY_STATUS_DUE
                        '当日(活動結果未登録)
                        AddCssClass(VisitActualRow, "colorDue")
                    Case Else
                        '当日(活動結果登録済)
                        AddCssClass(VisitActualRow, "colorComplete")
                        AddCssClass(VisitActualRow, "completion")
                End Select
            Else
                '活動結果登録済
                AddCssClass(VisitActualRow, "colorComplete")
                AddCssClass(VisitActualRow, "completion")
            End If

            If String.IsNullOrEmpty(row.TEMP_STAFF_OPERATIONCODE_ICON) Then
                '一次対応者の権限アイコンを非表示
                AddCssClass(TempStaffOperationIcon, "imageHidden")
            End If

        End If

        ' ======================== ログ出力 開始 ========================
        Logger.Info(String.Format(CultureInfo.InvariantCulture, " {0}_End", MethodBase.GetCurrentMethod.Name))
        ' ======================== ログ出力 終了 ========================

    End Sub


    ''' <summary>
    ''' クラス名を付与
    ''' </summary>
    ''' <param name="element"></param>
    ''' <param name="cssClass"></param>
    ''' <remarks></remarks>
    Private Sub AddCssClass(ByVal element As HtmlControl, ByVal cssClass As String)

        ' ======================== ログ出力 開始 ========================
        Logger.Info(String.Format(CultureInfo.InvariantCulture, " {0}_Start", MethodBase.GetCurrentMethod.Name))
        ' ======================== ログ出力 終了 ========================

        If String.IsNullOrEmpty(element.Attributes("Class")) Then
            element.Attributes("Class") = cssClass
        Else
            element.Attributes("Class") = element.Attributes("Class") & " " & cssClass
        End If

        ' ======================== ログ出力 開始 ========================
        Logger.Info(String.Format(CultureInfo.InvariantCulture, " {0}_End", MethodBase.GetCurrentMethod.Name))
        ' ======================== ログ出力 終了 ========================

    End Sub


    ''' <summary>
    ''' クラス名を除去
    ''' </summary>
    ''' <param name="element"></param>
    ''' <param name="cssClass"></param>
    ''' <remarks></remarks>
    Private Sub RemoveCssClass(ByVal element As HtmlControl, ByVal cssClass As String)

        ' ======================== ログ出力 開始 ========================
        Logger.Info(String.Format(CultureInfo.InvariantCulture, " {0}_Start", MethodBase.GetCurrentMethod.Name))
        ' ======================== ログ出力 終了 ========================

        element.Attributes("Class") = element.Attributes("Class").Replace(cssClass, "")

        ' ======================== ログ出力 開始 ========================
        Logger.Info(String.Format(CultureInfo.InvariantCulture, " {0}_End", MethodBase.GetCurrentMethod.Name))
        ' ======================== ログ出力 終了 ========================

    End Sub


#End Region


#Region "プロパティ"

    Public Property targetDay() As String
        Get
            Return Me.day.Value
        End Get
        Set(ByVal value As String)
            Me.day.Value = value
        End Set
    End Property

    Public Property ToDoBoxMode() As String
        Get
            Return Me.WalkinCompFlg.Value
        End Get
        Set(ByVal value As String)
            Me.WalkinCompFlg.Value = value
        End Set
    End Property

#End Region



End Class
