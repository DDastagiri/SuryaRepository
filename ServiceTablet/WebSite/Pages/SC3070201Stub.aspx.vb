Imports Toyota.eCRB.SystemFrameworks.Web
Imports Toyota.eCRB.SystemFrameworks.Core

Partial Class Pages_SC3070201input
    Inherits BasePage
    Private Sub SC3070201_Load(sender As Object, e As System.EventArgs) Handles Me.Load

        If (Not Me.IsPostBack AndAlso Not Me.IsCallback) Then

        End If

    End Sub

    Private Sub goQuotation_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles goButton.Click

        'セッション情報格納
        MyBase.SetValue(ScreenPos.Next, "EstimateId", Me.estimateIdTextBox.Text)
        MyBase.SetValue(ScreenPos.Next, "MenuLockFlag", Me.lockStatusTextBox.Text)
        MyBase.SetValue(ScreenPos.Next, "NewActFlag", Me.NewActFlagTextBox.Text)


        '画面遷移
        MyBase.RedirectNextScreen("SC3070201")

    End Sub

End Class
