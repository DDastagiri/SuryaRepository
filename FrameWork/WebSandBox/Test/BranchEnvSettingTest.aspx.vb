Imports Toyota.eCRB.SystemFrameworks.Core.iCROP.BizLogic

Partial Class Test_BranchEnvSettingTest
    Inherits System.Web.UI.Page

    Protected Sub Button1_Click(sender As Object, e As System.EventArgs) Handles Button1.Click

        Dim bes As New BranchEnvSetting
        Me.GridView1.DataSource = bes.GetEnvSetting("44B40", "01", "CSV_PATH").Table
        Me.GridView1.DataBind()

    End Sub

    Protected Sub Button2_Click(sender As Object, e As System.EventArgs) Handles Button2.Click

        Dim bes As New BranchEnvSetting
        Me.GridView2.DataSource = bes.GetEnvSetting("44B40", "99", "CSV_PATH").Table
        Me.GridView2.DataBind()

    End Sub

    Protected Sub Button3_Click(sender As Object, e As System.EventArgs) Handles Button3.Click

        Dim bes As New BranchEnvSetting
        Me.GridView3.DataSource = bes.GetEnvSetting("44B40", Nothing, "CSV_PATH").Table
        Me.GridView3.DataBind()

    End Sub

    Protected Sub Button4_Click(sender As Object, e As System.EventArgs) Handles Button4.Click

        Dim bes As New BranchEnvSetting
        Me.GridView4.DataSource = bes.GetEnvSetting("44B40", "01", Nothing).Table
        Me.GridView4.DataBind()

    End Sub

    Protected Sub Button5_Click(sender As Object, e As System.EventArgs) Handles Button5.Click

        Dim bes As New BranchEnvSetting
        Me.GridView5.DataSource = bes.GetEnvSetting("AAAAA", "01", "CSV_PATH").Table
        Me.GridView5.DataBind()

    End Sub

    Protected Sub Button6_Click(sender As Object, e As System.EventArgs) Handles Button6.Click

        Dim bes As New BranchEnvSetting
        Me.GridView6.DataSource = bes.GetEnvSetting(Nothing, "01", "CSV_PATH").Table
        Me.GridView6.DataBind()

    End Sub

    Protected Sub Button7_Click(sender As Object, e As System.EventArgs) Handles Button7.Click

        Dim bes As New BranchEnvSetting
        Me.GridView7.DataSource = bes.GetSpecificEnvSetting("44B40", "01", "CSV_PATH").Table
        Me.GridView7.DataBind()

    End Sub

    Protected Sub Button8_Click(sender As Object, e As System.EventArgs) Handles Button8.Click

        Dim bes As New BranchEnvSetting
        Me.GridView8.DataSource = bes.GetSpecificEnvSetting("44B40", "99", "CSV_PATH").Table
        Me.GridView8.DataBind()

    End Sub

    Protected Sub Button9_Click(sender As Object, e As System.EventArgs) Handles Button9.Click

        Dim bes As New BranchEnvSetting
        Me.GridView9.DataSource = bes.GetSpecificEnvSetting("44B40", Nothing, "CSV_PATH").Table
        Me.GridView9.DataBind()

    End Sub

    Protected Sub Button10_Click(sender As Object, e As System.EventArgs) Handles Button10.Click

        Dim bes As New BranchEnvSetting
        Me.GridView10.DataSource = bes.GetSpecificEnvSetting("44B40", "01", Nothing).Table
        Me.GridView10.DataBind()

    End Sub

    Protected Sub Button11_Click(sender As Object, e As System.EventArgs) Handles Button11.Click

        Dim bes As New BranchEnvSetting
        Me.GridView11.DataSource = bes.GetSpecificEnvSetting("AAAAA", "01", "CSV_PATH").Table
        Me.GridView11.DataBind()

    End Sub

    Protected Sub Button12_Click(sender As Object, e As System.EventArgs) Handles Button12.Click

        Dim bes As New BranchEnvSetting
        Me.GridView12.DataSource = bes.GetSpecificEnvSetting(Nothing, "01", "CSV_PATH").Table
        Me.GridView12.DataBind()

    End Sub

End Class
