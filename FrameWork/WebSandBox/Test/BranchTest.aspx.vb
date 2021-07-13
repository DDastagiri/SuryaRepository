Option Strict On
Option Explicit On

Imports Toyota.eCRB.SystemFrameworks.Core.iCROP.BizLogic


Partial Class Test_BranchTest
    Inherits System.Web.UI.Page

    Protected Sub Button1_Click(sender As Object, e As System.EventArgs) Handles Button1.Click

        Dim br As New Branch
        Me.GridView1.DataSource = br.GetAllBranch("44B40")
        Me.GridView1.DataBind()

    End Sub

    Protected Sub Button2_Click(sender As Object, e As System.EventArgs) Handles Button2.Click

        Dim br As New Branch
        Me.GridView2.DataSource = br.GetAllBranch("44B40", "0")
        Me.GridView2.DataBind()

    End Sub

    Protected Sub Button3_Click(sender As Object, e As System.EventArgs) Handles Button3.Click

        Dim br As New Branch
        Me.GridView3.DataSource = br.GetAllBranch("44B40", "9")
        Me.GridView3.DataBind()

    End Sub

    Protected Sub Button4_Click(sender As Object, e As System.EventArgs) Handles Button4.Click

        Dim br As New Branch
        Me.GridView4.DataSource = br.GetBranch("44B40", "01").Table
        Me.GridView4.DataBind()

    End Sub

    Protected Sub Button5_Click(sender As Object, e As System.EventArgs) Handles Button5.Click

        Dim br As New Branch
        Me.GridView5.DataSource = br.GetBranch("44B40", "01", "0").Table
        Me.GridView5.DataBind()

    End Sub

    Protected Sub Button6_Click(sender As Object, e As System.EventArgs) Handles Button6.Click

        Dim br As New Branch
        Me.GridView6.DataSource = br.GetBranch("44B40", "01", "9").Table
        Me.GridView6.DataBind()

    End Sub

End Class
