Option Strict On
Option Explicit On

Imports Toyota.eCRB.SystemFrameworks.Web

Partial Class Test_WebWordUtility
    Inherits System.Web.UI.Page

    Protected Sub Button1_Click(sender As Object, e As System.EventArgs) Handles Button1.Click

        Me.Label1.Text = WebWordUtility.GetWord("MASTERPAGEMAIN", 2)
        Me.Label2.Text = WebWordUtility.GetWord(1)

        If WebWordUtility.GetWord("MASTERPAGEMAIN", 99999999).Equals(String.Empty) Then
            Me.Label3.Text = "String.Empty"
        Else
            Me.Label3.Text = WebWordUtility.GetWord("MASTERPAGEMAIN", 99999999)
        End If

        If WebWordUtility.GetWord(99999999).Equals(String.Empty) Then
            Me.Label4.Text = "String.Empty"
        Else
            Me.Label4.Text = WebWordUtility.GetWord(99999999)
        End If

    End Sub

End Class
