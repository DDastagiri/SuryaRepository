Imports Toyota.eCRB.SystemFrameworks.Web

Partial Class Test_BasePageTest
    Inherits System.Web.UI.Page

    Private Const SESSION_TOPPAGE As String = "Toyota.eCRB.SystemFrameworks.Web.BasePage.TopPage"

    Protected Sub Button1_Click(sender As Object, e As System.EventArgs) Handles Button1.Click
        Session(SESSION_TOPPAGE) = "BasePageTest1"
        Response.Redirect("../Pages/BasePageTest.aspx")
    End Sub

    Protected Sub Page_Load(sender As Object, e As System.EventArgs) Handles Me.Load
        Dim auth As New AuthenticationManager
        auth.Auth("200003@44B40", "icrop", "")
    End Sub

End Class
