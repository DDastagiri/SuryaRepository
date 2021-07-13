Imports Toyota.eCRB.SystemFrameworks.Core.iCROP.BizLogic
Imports Toyota.eCRB.SystemFrameworks.Core.iCROP.DataAccess

Partial Class Test_SystemEnvSettingTest
    Inherits System.Web.UI.Page

    Protected Sub Button1_Click(sender As Object, e As System.EventArgs) Handles Button1.Click

        Dim sys As New SystemEnvSetting
        Me.GridView1.DataSource = sys.GetSystemEnvSetting("USED_FLG_MOBILE").Table
        Me.GridView1.DataBind()

    End Sub

    Protected Sub Button2_Click(sender As Object, e As System.EventArgs) Handles Button2.Click

        Dim sys As New SystemEnvSetting
        Me.GridView2.DataSource = sys.GetSystemEnvSetting(Nothing).Table
        Me.GridView2.DataBind()

    End Sub

End Class
