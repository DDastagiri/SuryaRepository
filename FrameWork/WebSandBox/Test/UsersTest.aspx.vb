Option Strict On
Option Explicit On

Imports Toyota.eCRB.SystemFrameworks.Core.iCROP.BizLogic

Partial Class Test_UsersTest
    Inherits System.Web.UI.Page

    Protected Sub Button1_Click(sender As Object, e As System.EventArgs) Handles Button1.Click

        Dim us As New Users
        Me.GridView1.DataSource = us.GetAllUser("44B40")
        Me.GridView1.DataBind()

    End Sub

    Protected Sub Button2_Click(sender As Object, e As System.EventArgs) Handles Button2.Click

        Dim us As New Users
        Me.GridView2.DataSource = us.GetAllUser("44B40", "01")
        Me.GridView2.DataBind()

    End Sub

    Protected Sub Button3_Click(sender As Object, e As System.EventArgs) Handles Button3.Click

        Dim us As New Users
        Me.GridView3.DataSource = us.GetAllUser("44B40", "999")
        Me.GridView3.DataBind()

    End Sub

    Protected Sub Button4_Click(sender As Object, e As System.EventArgs) Handles Button4.Click

        Dim us As New Users
        Dim opelist As New List(Of Decimal)({7, 8})
        Me.GridView4.DataSource = us.GetAllUser("44B40", "01", opelist)
        Me.GridView4.DataBind()

    End Sub

    Protected Sub Button5_Click(sender As Object, e As System.EventArgs) Handles Button5.Click

        Dim us As New Users
        Dim opelist As New List(Of Decimal)({99})
        Me.GridView5.DataSource = us.GetAllUser("44B40", "01", opelist)
        Me.GridView5.DataBind()

    End Sub

    Protected Sub Button6_Click(sender As Object, e As System.EventArgs) Handles Button6.Click

        Dim us As New Users
        Dim opelist As New List(Of Decimal)({7, 8})
        Me.GridView6.DataSource = us.GetAllUser("44B40", "01", opelist, "0")
        Me.GridView6.DataBind()

    End Sub

    Protected Sub Button7_Click(sender As Object, e As System.EventArgs) Handles Button7.Click

        Dim us As New Users
        Dim opelist As New List(Of Decimal)({7, 8})
        Me.GridView7.DataSource = us.GetAllUser("44B40", "01", opelist, "9")
        Me.GridView7.DataBind()

    End Sub

    Protected Sub Button8_Click(sender As Object, e As System.EventArgs) Handles Button8.Click

        Dim us As New Users
        Me.GridView8.DataSource = us.GetUser("ICROPGM@44B40").Table
        Me.GridView8.DataBind()

    End Sub

    Protected Sub Button9_Click(sender As Object, e As System.EventArgs) Handles Button9.Click

        Dim us As New Users
        Me.GridView9.DataSource = us.GetUser("ICROPGM@44B40", "0").Table
        Me.GridView9.DataBind()

    End Sub

    Protected Sub Button10_Click(sender As Object, e As System.EventArgs) Handles Button10.Click

        Dim us As New Users
        Me.GridView10.DataSource = us.GetUser("ICROPGM@44B40", "9").Table
        Me.GridView10.DataBind()

    End Sub
End Class
