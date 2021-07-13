Imports Toyota.eCRB.SystemFrameworks.Web.Controls
Imports System.Data
Imports System.Globalization

Partial Class Pages_ControlsSample
    Inherits BasePage
    Implements ICallbackEventHandler


    Protected Sub Page_Load(sender As Object, e As System.EventArgs) Handles Me.Load
        If (Not Me.IsPostBack AndAlso Not Me.IsCallback) Then
            With segmentedButton1
                .Items.Add(New ListItem(WebWordUtility.GetWord(4) & "1", "1"))
                .Items.Add(New ListItem(WebWordUtility.GetWord(4) & "2", "2"))
                .Items.Add(New ListItem(WebWordUtility.GetWord(4) & "3", "3"))
            End With
            segmentedButton1.SelectedValue = "2"

            With segmentedButton2
                .Items.Add(New ListItem(WebWordUtility.GetWord(4) & "1", "1"))
                .Items.Add(New ListItem(WebWordUtility.GetWord(4) & "2", "2"))
                .Items.Add(New ListItem(WebWordUtility.GetWord(4) & "3", "3"))
            End With

            Dim dt As New DataTable()
            dt.Columns.Add("leftItemId", GetType(Integer))
            dt.Columns.Add("leftName", GetType(String))
            dt.Columns.Add("leftHasDetail", GetType(Boolean))
            dt.Columns.Add("rightItemId", GetType(Integer))
            dt.Columns.Add("rightName", GetType(String))
            dt.Columns.Add("rightHasDetail", GetType(Boolean))

            dt.Rows.Add(New Object() {1, "項目１", False, 2, "項目２", True})
            dt.Rows.Add(New Object() {3, "項目３", False, 4, "項目４", False})
            dt.Rows.Add(New Object() {5, "項目５", False, 6, "項目６", False})
            dt.Rows.Add(New Object() {7, "項目７", True, 8, "項目８", False})

            With popOverForm2_1_repeater
                .DataSource = dt
                .DataBind()
            End With

            DateTimeSelector1.Value = New DateTime(2011, 1, 1, 15, 15, 15)
            DateTimeSelector2.Value = New DateTime(2011, 1, 1, 15, 15, 15)
            DateTimeSelector3.Value = DateTimeSelector2.Value

            customTextBox1.Text = "ABCDD"
        End If

        ClientScript.RegisterStartupScript(Me.GetType(), "Callback", String.Format(CultureInfo.InvariantCulture, "callback.beginCallback = function () {{ {0}; }};", Page.ClientScript.GetCallbackEventReference(Me, "callback.packedArgument", "callback.endCallback", "", True)), True)
    End Sub

    Protected Sub popOverForm1_ClientCallback(sender As Object, e As Toyota.eCRB.SystemFrameworks.Web.Controls.ClientCallbackEventArgs) Handles popOverForm1.ClientCallback

        'クライアントサイドからの引数を受け取る (PopOverForm.render イベントハンドラ内の、 pop.callbackServer()呼び出しの最初の引数）
        ' pop.callbackServer({ message: 'Hello from client' }, function(result) {...});
        Dim clientMessage As String = e.Arguments("message")

        'クライアントに渡す返却値を設定する（PopOverForm.render イベントハンドラ内の、 pop.callbackServer()呼び出しの２番目の引数に設定されているコールバック関数のresult引数になる）
        ' pop.callbackServer({ message: 'Hello from client' }, function(result) {...});
        e.Results.Add("response", "server response of " & clientMessage)
        e.Results.Add("number", 123)
    End Sub

    Protected Sub customButton2_Click(sender As Object, e As System.EventArgs) Handles customButton2.Click
        customButton2.BadgeCount += 1

    End Sub

    Protected Sub segmentedButton2_SelectedIndexChanged(sender As Object, e As System.EventArgs) Handles segmentedButton2.SelectedIndexChanged
        Me.ShowMessageBox(5, New String() {segmentedButton2.SelectedValue})
    End Sub

    Protected Sub dialogButton_Click(sender As Object, e As System.EventArgs) Handles dialogButton.Click
        Me.OpenDialog("ControlsSample", DialogEffect.left)
    End Sub

    '2014/11/04 TCS藤井 Add Start
    Protected Sub dialogCloseButton_Click(sender As Object, e As System.EventArgs) Handles dialogCloseButton.Click
        Me.CloseDialog()
    End Sub
    '2014/11/04 TCS藤井 Add End

    Protected Sub popOverForm2_1_repeater_ItemDataBound(sender As Object, e As System.Web.UI.WebControls.RepeaterItemEventArgs) Handles popOverForm2_1_repeater.ItemDataBound
        If (e.Item.ItemType = ListItemType.Item OrElse e.Item.ItemType = ListItemType.AlternatingItem) Then
            Dim leftCustomButton As CustomButton = e.Item.FindControl("leftCustomButton")
            Dim rightCustomButton As CustomButton = e.Item.FindControl("rightCustomButton")
            Dim data As DataRowView = CType(e.Item.DataItem, DataRowView)
            With leftCustomButton
                .Text = data("leftName")
                .Attributes.Add("data-ItemId", data("leftItemId"))
                If (CBool(data("leftHasDetail"))) Then
                    .Attributes.Add("data-HasDetail", "true")
                Else
                    .Attributes.Add("data-HasDetail", "false")
                End If
            End With
            With rightCustomButton
                .Text = data("rightName")
                .Attributes.Add("data-ItemId", data("rightItemId"))
                If (CBool(data("rightHasDetail"))) Then
                    .Attributes.Add("data-HasDetail", "true")
                Else
                    .Attributes.Add("data-HasDetail", "false")
                End If
            End With
        End If

    End Sub

    Protected Sub customRepeater1_ClientCallback(sender As Object, e As Toyota.eCRB.SystemFrameworks.Web.Controls.ClientCallbackEventArgs) Handles customRepeater1.ClientCallback
        Dim beginRowIndex As Integer = 0
        Dim criteria As String = CStr(e.Arguments("criteria"))
        If (Integer.TryParse(e.Arguments("beginRowIndex"), beginRowIndex)) Then
            Dim rows As New StringBuilder(50)
            Dim firstElement As Boolean = True
            If (e.Arguments("rewind").Equals("true")) Then
                Dim fromIndex As Integer = beginRowIndex - customRepeater1.MaxCacheRows + 1
                If (fromIndex < 0) Then
                    fromIndex = 0
                End If

                For i As Integer = fromIndex To beginRowIndex
                    If (firstElement) Then
                        firstElement = False
                    Else
                        rows.Append(",")
                    End If
                    rows.AppendFormat("{{ ""name"" : ""項目{0} ({1})"", ""value"" : {0} }}", i, criteria)
                Next
            Else
                Dim toIndex As Integer = (beginRowIndex + customRepeater1.MaxCacheRows - 1)
                If (103 < toIndex) Then
                    toIndex = 103
                End If

                For i As Integer = beginRowIndex To toIndex
                    If (firstElement) Then
                        firstElement = False
                    Else
                        rows.Append(",")
                    End If
                    rows.AppendFormat("{{ ""name"" : ""項目{0} ({1})"", ""value"" : {0} }}", i, criteria)
                Next
            End If
            e.Results("@rows") = "[" & rows.ToString() & "]"
        Else
            e.Results("@rows") = "[]"
        End If
        e.Results("@totalCount") = 2978

    End Sub

    Protected Sub customTextBox1_TextChanged(sender As Object, e As System.EventArgs) Handles customTextBox1.TextChanged

    End Sub

    Public Sub RaiseCallbackEvent(eventArgument As String) Implements System.Web.UI.ICallbackEventHandler.RaiseCallbackEvent
        Dim tokens As String() = eventArgument.Split(New Char() {","c}, 2)
        Dim method As String = tokens(0)
        Dim argument As String = tokens(1)

        If (method.Equals("GetAddress")) Then
            If (argument.Equals("4441111")) Then
                _callbackResult = "愛知県名古屋市中村区"
            Else
                _callbackResult = ""
            End If
        End If
    End Sub

    Public Function GetCallbackResult() As String Implements System.Web.UI.ICallbackEventHandler.GetCallbackResult
        Return _callbackResult
    End Function
    Private _callbackResult As String

    Protected Sub numericBox1_ValueChanged(sender As Object, e As System.EventArgs) Handles numericBox1.ValueChanged

    End Sub
End Class
