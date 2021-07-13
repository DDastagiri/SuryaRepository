Imports Toyota.eCRB.SystemFrameworks.Web

Partial Class ControlTest
    Inherits BasePage

 
    Protected Sub popOverForm1_ClientCallback(sender As Object, e As Toyota.eCRB.SystemFrameworks.Web.Controls.ClientCallbackEventArgs) Handles popOverForm1.ClientCallback
        e.Results.Add("return", "Hello")
        e.Results.Add("number", 192)
    End Sub

    Protected Sub popOverForm1_ValueChanged(sender As Object, e As System.EventArgs) Handles popOverForm1.ValueChanged
        Dim val As String = popOverForm1.Value
    End Sub
End Class
