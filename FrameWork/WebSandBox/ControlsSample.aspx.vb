
Partial Class Pages_ControlsSample
    Inherits System.Web.UI.Page

    Protected Sub popOverForm1_ClientCallback(sender As Object, e As Toyota.eCRB.SystemFrameworks.Web.Controls.ClientCallbackEventArgs) Handles popOverForm1.ClientCallback
        e.Results.Add("result", "server message")
        e.Results.Add("number", 123)
    End Sub
End Class
