Imports Toyota.eCRB.SystemFrameworks.Web
Imports Toyota.eCRB.SystemFrameworks.Core

Partial Class Pages_SC3280101Input
    Inherits BasePage

    Private Sub SC3280101_Load(sender As Object, e As System.EventArgs) Handles Me.Load

        If (Not Me.IsPostBack AndAlso Not Me.IsCallback) Then
            'ログイン情報
            Dim staffInfo As StaffContext = StaffContext.Current
        End If

    End Sub

    Private Sub goQuotation_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles goButton.Click

        Dim presenceCategory As String = Me.PresenceCategory.Text
        Dim presenceDetail As String = Me.PresenceDetail.Text

        StaffContext.Current.UpdatePresence(presenceCategory, presenceDetail)

        'セッション情報格納
        MyBase.SetValue(ScreenPos.Next, "EstimateId", "EstimateId")

        MyBase.SetValue(ScreenPos.Next, "SalesId", Me.salesIdTextBox.Text)
        MyBase.SetValue(ScreenPos.Next, "CstId", Me.CstIdTextBox.Text)
        MyBase.SetValue(ScreenPos.Next, "CstType", Me.CstTypeTextBox.Text)
        MyBase.SetValue(ScreenPos.Next, "CstVclType", Me.CstVclTypeTextBox.Text)        

        '画面遷移
        MyBase.RedirectNextScreen("SC3280101")

    End Sub

End Class
