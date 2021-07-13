Imports Toyota.eCRB.SystemFrameworks.Core
Imports Toyota.eCRB.SystemFrameworks.Web

Partial Class Test_LoggerTest
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(sender As Object, e As System.EventArgs) Handles Me.Load

        StaffContext.Clear()

        Dim auth As New AuthenticationManager
        auth.Auth("20003@44B40", "icrop", "")
    End Sub

    Protected Sub Button1_Click(sender As Object, e As System.EventArgs) Handles Button1.Click
        Logger.Error("Error")
    End Sub

    Protected Sub Button2_Click(sender As Object, e As System.EventArgs) Handles Button2.Click
        Dim ex As New Exception("Test Exception")
        Logger.Error("Error Ex", ex)
    End Sub

    Protected Sub Button3_Click(sender As Object, e As System.EventArgs) Handles Button3.Click
        Logger.Warn("Warn")
    End Sub

    Protected Sub Button4_Click(sender As Object, e As System.EventArgs) Handles Button4.Click
        Logger.Info("Info")
    End Sub

    Protected Sub Button5_Click(sender As Object, e As System.EventArgs) Handles Button5.Click
        Logger.Info("Info receiveLog", True)
    End Sub

    Protected Sub Button6_Click(sender As Object, e As System.EventArgs) Handles Button6.Click
        Logger.Debug("Debug")
    End Sub

End Class
