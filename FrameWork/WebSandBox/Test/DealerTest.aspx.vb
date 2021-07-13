Imports Toyota.eCRB.SystemFrameworks.Core.iCROP.BizLogic
Imports System.Data


Partial Class Test_DealerTest
    Inherits System.Web.UI.Page

    Protected Sub Button1_Click(sender As Object, e As System.EventArgs) Handles Button1.Click

        Dim dr As New Dealer

        Me.GridView1.DataSource = dr.GetAllDealer()
        Me.GridView1.DataBind()

    End Sub

    Protected Sub Button2_Click(sender As Object, e As System.EventArgs) Handles Button2.Click

        Dim dr As New Dealer

        Me.GridView2.DataSource = dr.GetAllDealer("1")
        Me.GridView2.DataBind()

    End Sub

    Protected Sub Button3_Click(sender As Object, e As System.EventArgs) Handles Button3.Click

        Dim dr As New Dealer

        Me.GridView3.DataSource = dr.GetAllDealer("9")
        Me.GridView3.DataBind()

    End Sub

    Protected Sub Button4_Click(sender As Object, e As System.EventArgs) Handles Button4.Click

        Dim dr As New Dealer

        Me.GridView4.DataSource = dr.GetDealer("44B40").Table
        Me.GridView4.DataBind()

    End Sub


    Protected Sub Button5_Click(sender As Object, e As System.EventArgs) Handles Button5.Click

        Dim dr As New Dealer

        Me.GridView5.DataSource = dr.GetDealer("10Z11", "1").Table
        Me.GridView5.DataBind()

    End Sub

    Protected Sub Button6_Click(sender As Object, e As System.EventArgs) Handles Button6.Click

        Dim dr As New Dealer

        Me.GridView6.DataSource = dr.GetDealer("10Z11", "9").Table
        Me.GridView6.DataBind()

    End Sub

End Class
