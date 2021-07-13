Option Strict On
Option Explicit On

Imports Toyota.eCRB.SystemFrameworks.Web

Partial Class Test_StaffContextTest
    Inherits System.Web.UI.Page

    Protected Sub Button1_Click(sender As Object, e As System.EventArgs) Handles Button1.Click

        Dim authManager As New AuthenticationManager
        authManager.Auth(Me.TextBox1.Text, Me.TextBox2.Text, Me.TextBox3.Text)

    End Sub

    Protected Sub Button2_Click(sender As Object, e As System.EventArgs) Handles Button2.Click

        Dim sc As StaffContext = StaffContext.Current()

        Me.Label1.Text = sc.Account
        Me.Label2.Text = sc.UserName
        Me.Label3.Text = sc.DlrCD
        Me.Label4.Text = sc.DlrName
        Me.Label5.Text = sc.BrnCD
        Me.Label6.Text = sc.BrnName
        Me.Label7.Text = CStr(sc.OpeCD)
        Me.Label8.Text = sc.OpeName
        Me.Label9.Text = CStr(sc.UserPermission)
        Me.Label10.Text = sc.TeamCD
        Me.Label11.Text = sc.TeamName
        Me.Label12.Text = CStr(sc.TeamLeader)
        Me.Label13.Text = CStr(sc.TimeDiff)
        Me.Label14.Text = CStr(StaffContext.IsCreated)

    End Sub

End Class
