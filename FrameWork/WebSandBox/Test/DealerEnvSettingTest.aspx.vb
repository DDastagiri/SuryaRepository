Imports Toyota.eCRB.SystemFrameworks.Core.iCROP.BizLogic


Partial Class Test_DealerEnvSettingTest
    Inherits System.Web.UI.Page

    Protected Sub Button1_Click(sender As Object, e As System.EventArgs) Handles Button1.Click

        Dim des As New DealerEnvSetting
        Me.GridView1.DataSource = des.GetEnvSetting("44B40", "CSV_PATH").Table
        Me.GridView1.DataBind()

    End Sub

    Protected Sub Button2_Click(sender As Object, e As System.EventArgs) Handles Button2.Click

        Dim des As New DealerEnvSetting
        Me.GridView2.DataSource = des.GetEnvSetting("44B40", Nothing).Table
        Me.GridView2.DataBind()

    End Sub

    Protected Sub Button3_Click(sender As Object, e As System.EventArgs) Handles Button3.Click

        Dim des As New DealerEnvSetting
        Me.GridView3.DataSource = des.GetEnvSetting("AAAAA", "CSV_PATH").Table
        Me.GridView3.DataBind()

    End Sub

    Protected Sub Button4_Click(sender As Object, e As System.EventArgs) Handles Button4.Click

        Dim des As New DealerEnvSetting
        Me.GridView4.DataSource = des.GetEnvSetting(Nothing, "CSV_PATH").Table
        Me.GridView4.DataBind()

    End Sub

    Protected Sub Button5_Click(sender As Object, e As System.EventArgs) Handles Button5.Click

        Dim des As New DealerEnvSetting
        Me.GridView5.DataSource = des.GetSpecificEnvSetting("44B40", "CSV_PATH").Table
        Me.GridView5.DataBind()

    End Sub

    Protected Sub Button6_Click(sender As Object, e As System.EventArgs) Handles Button6.Click

        Dim des As New DealerEnvSetting
        Me.GridView6.DataSource = des.GetSpecificEnvSetting("44B40", Nothing).Table
        Me.GridView6.DataBind()

    End Sub

    Protected Sub Button7_Click(sender As Object, e As System.EventArgs) Handles Button7.Click

        Dim des As New DealerEnvSetting
        Me.GridView7.DataSource = des.GetSpecificEnvSetting("AAAAA", "CSV_PATH").Table
        Me.GridView7.DataBind()

    End Sub

    Protected Sub Button8_Click(sender As Object, e As System.EventArgs) Handles Button8.Click

        Dim des As New DealerEnvSetting
        Me.GridView8.DataSource = des.GetSpecificEnvSetting(Nothing, "CSV_PATH").Table
        Me.GridView8.DataBind()

    End Sub

End Class
