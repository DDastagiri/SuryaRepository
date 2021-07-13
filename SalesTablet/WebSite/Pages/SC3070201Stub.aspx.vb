Imports Toyota.eCRB.SystemFrameworks.Web
Imports Toyota.eCRB.SystemFrameworks.Core

Partial Class Pages_SC3070201Input
    Inherits BasePage
    Private Sub SC3070201_Load(sender As Object, e As System.EventArgs) Handles Me.Load

        If (Not Me.IsPostBack AndAlso Not Me.IsCallback) Then
            'ログイン情報
            Dim staffInfo As StaffContext = StaffContext.Current
            Me.OperationCodeTextBox.Text = staffInfo.OpeCD
        End If

    End Sub

    Private Sub goQuotation_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles goButton.Click

        'セッション情報格納
        MyBase.SetValue(ScreenPos.Next, "EstimateId", Me.estimateIdTextBox.Text)
        ' 2012/10/29 TCS 上田 【A.STEP2】次世代e-CRB  新車タブレット横展開に向けた機能開発 START
        MyBase.SetValue(ScreenPos.Next, "SelectedEstimateIndex", Me.selectedEstimateIndexTextBox.Text)
        ' 2012/10/29 TCS 上田 【A.STEP2】次世代e-CRB  新車タブレット横展開に向けた機能開発 END
        MyBase.SetValue(ScreenPos.Next, "MenuLockFlag", Me.lockStatusTextBox.Text)
        'MyBase.SetValue(ScreenPos.Next, "NewActFlag", Me.NewActFlagTextBox.Text)

        MyBase.SetValue(ScreenPos.Next, "OperationCode", Me.OperationCodeTextBox.Text)
        MyBase.SetValue(ScreenPos.Next, "BusinessFlg", Me.BusinessFlgTextBox.Text)
        MyBase.SetValue(ScreenPos.Next, "ReadOnlyFlg", Me.ReadOnlyFlgTextBox.Text)

        If Not String.IsNullOrEmpty(Me.NoticeReqIdTextBox.Text) Then
            MyBase.SetValue(ScreenPos.Next, "NoticeReqId", Me.NoticeReqIdTextBox.Text)
        End If

        '画面遷移
        MyBase.RedirectNextScreen("SC3070201")

    End Sub

End Class
