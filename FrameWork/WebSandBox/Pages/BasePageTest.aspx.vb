Imports Toyota.eCRB.SystemFrameworks.Web

Partial Class Test_BasePageTest
    Inherits BasePage

    Private Const SESSION_TOPPAGE As String = "Toyota.eCRB.SystemFrameworks.Web.BasePage.TopPage"

    Protected Sub Button1_Click(sender As Object, e As System.EventArgs) Handles Button1.Click
        Dim hogehoge As New List(Of String)
        hogehoge.Add("BasePageTest1")
        Session("hogehoge") = hogehoge
        RedirectNextScreen("BasePageTest1")
    End Sub

End Class
