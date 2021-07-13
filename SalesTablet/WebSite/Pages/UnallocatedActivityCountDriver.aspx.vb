Imports Toyota.eCRB.SystemFrameworks.Web
Imports Toyota.eCRB.SystemFrameworks.Core
Imports Toyota.eCRB.CommonUtility.BizLogic

Partial Class Pages_UnallocatedActivityCountDriver
    Inherits BasePage

    Private Sub UnallocatedClassDriver_Load(sender As Object, e As System.EventArgs) Handles Me.Load

        If (Not Me.IsPostBack AndAlso Not Me.IsCallback) Then
            'ログイン情報
            Dim staffInfo As StaffContext = StaffContext.Current
            Dim dlrcd As String = staffInfo.DlrCD
            Dim brncd As String = staffInfo.BrnCD

            Dim bizLogicAct As New UnallocatedActivityBusinessLogic

            Me.UnallocatedActivityCountLabel.Text = bizLogicAct.GetUnallocatedActivityCount(dlrcd, brncd)
        End If

    End Sub


End Class
