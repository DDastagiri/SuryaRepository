Imports Toyota.eCRB.SystemFrameworks.Core.iCROP.BizLogic
Imports Toyota.eCRB.SystemFrameworks.Core.iCROP.DataAccess

Partial Class Test_FunctionSettingTest
    Inherits System.Web.UI.Page

    Protected Sub Button1_Click(sender As Object, e As System.EventArgs) Handles Button1.Click

        Dim func As New FunctionSetting
        Me.Label1.Text = CStr(func.GetiCROPFunctionSetting("44B40", "USED_FLG_INBOUND"))

    End Sub

    Protected Sub Button2_Click(sender As Object, e As System.EventArgs) Handles Button2.Click

        Dim func As New FunctionSetting
        Me.Label2.Text = CStr(func.GetiCROPFunctionSetting("11A10", "USED_FLG_UCAR"))

    End Sub

    Protected Sub Button3_Click(sender As Object, e As System.EventArgs) Handles Button3.Click

        Dim func As New FunctionSetting
        Me.Label3.Text = CStr(func.GetiCROPFunctionSetting("11A20", "USED_FLG_UCAR"))

    End Sub

    Protected Sub Button4_Click(sender As Object, e As System.EventArgs) Handles Button4.Click

        Dim func As New FunctionSetting
        Me.Label4.Text = CStr(func.GetiCROPFunctionSetting("11A30", "USED_FLG_UCAR"))

    End Sub

    Protected Sub Button5_Click(sender As Object, e As System.EventArgs) Handles Button5.Click

        Dim func As New FunctionSetting
        Me.Label5.Text = CStr(func.GetiCROPFunctionSetting("11A10", "BBBBBBBBBBBBB"))

    End Sub

    Protected Sub Button6_Click(sender As Object, e As System.EventArgs) Handles Button6.Click

        Dim func As New FunctionSetting
        Me.Label6.Text = CStr(func.GetiCROPFunctionSetting("11A20", "BBBBBBBBBBBBB"))

    End Sub

    Protected Sub Button7_Click(sender As Object, e As System.EventArgs) Handles Button7.Click

        Dim func As New FunctionSetting
        Me.Label7.Text = CStr(func.GetiCROPFunctionSetting("11A30", "BBBBBBBBBBBBB"))

    End Sub

    Protected Sub Button8_Click(sender As Object, e As System.EventArgs) Handles Button8.Click

        Dim func As New FunctionSetting
        Me.GridView1.DataSource = func.GetFunctionSetting("44B40", "USED_FLG_INBOUND").Table
        Me.GridView1.DataBind()

    End Sub

    Protected Sub Button9_Click(sender As Object, e As System.EventArgs) Handles Button9.Click

        Dim func As New FunctionSetting
        Me.GridView2.DataSource = func.GetFunctionSetting("44B40", Nothing).Table
        Me.GridView2.DataBind()

    End Sub

    Protected Sub Button10_Click(sender As Object, e As System.EventArgs) Handles Button10.Click

        Dim func As New FunctionSetting
        Me.GridView3.DataSource = func.GetFunctionSetting(Nothing, "USED_FLG_INBOUND").Table
        Me.GridView3.DataBind()

    End Sub

    Protected Sub Button11_Click(sender As Object, e As System.EventArgs) Handles Button11.Click

        Dim func As New FunctionSetting
        Me.GridView4.DataSource = func.GetFunctionSetting(Nothing, Nothing).Table
        Me.GridView4.DataBind()

    End Sub

    Protected Sub Button12_Click(sender As Object, e As System.EventArgs) Handles Button12.Click

        Dim func As New FunctionSetting
        Me.GridView5.DataSource = func.GetDelaerFunctionSetting("44B40", "USED_FLG_INBOUND").Table
        Me.GridView5.DataBind()

    End Sub

    Protected Sub Button13_Click(sender As Object, e As System.EventArgs) Handles Button13.Click

        Dim func As New FunctionSetting
        Me.GridView6.DataSource = func.GetDelaerFunctionSetting("44B40", Nothing).Table
        Me.GridView6.DataBind()

    End Sub

    Protected Sub Button14_Click(sender As Object, e As System.EventArgs) Handles Button14.Click

        Dim func As New FunctionSetting
        Me.GridView7.DataSource = func.GetDelaerFunctionSetting(Nothing, "USED_FLG_INBOUND").Table
        Me.GridView7.DataBind()

    End Sub

    Protected Sub Button15_Click(sender As Object, e As System.EventArgs) Handles Button15.Click

        Dim func As New FunctionSetting
        Me.GridView8.DataSource = func.GetDelaerFunctionSetting(Nothing, Nothing).Table
        Me.GridView8.DataBind()

    End Sub

    Protected Sub Button16_Click(sender As Object, e As System.EventArgs) Handles Button16.Click

        Dim func As New FunctionSetting
        Me.GridView9.DataSource = func.GetDelaerFunctionSetting("44B40", "AAAAA").Table
        Me.GridView9.DataBind()

    End Sub

End Class
