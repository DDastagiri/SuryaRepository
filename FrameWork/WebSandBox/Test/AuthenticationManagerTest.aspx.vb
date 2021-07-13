Imports Toyota.eCRB.SystemFrameworks.Web

Partial Class Test_AuthenticationManager
    Inherits System.Web.UI.Page

    Protected Sub Button1_Click(sender As Object, e As System.EventArgs) Handles Button1.Click

        Dim auth As New AuthenticationManager
        Me.Label1.Text = auth.Auth(Nothing, "icrop", "")
        Me.Label2.Text = auth.Auth("Account", Nothing, "")
        Me.Label3.Text = auth.Auth("200003@44B40", "icrop", "")
        Me.Label4.Text = auth.Auth("200003@", "icrop", "")
        Me.Label5.Text = auth.Auth("200003", "icrop", "")
        Me.Label6.Text = auth.Auth("200003@44B40", "icrop", "ABCDEFGHIJKLMN1")
        Me.Label7.Text = auth.Auth("200003@44B40", "icrop", "")
        Me.Label8.Text = auth.Auth("200003@44B40", "icrop", "")
        Me.Label9.Text = auth.Auth("200003@44B40", "icrop", "ABCDEFGHIJKLMN1")
        Me.Label10.Text = auth.Auth("200003@44B40", "icrop", "ABCDEFGHIJKLMN2")
        Me.Label11.Text = auth.Auth("200003", "icrop", "ABCDEFGHIJKLMN3")
        Me.Label12.Text = auth.Auth("200003@AAAAA", "icrop", "ABCDEFGHIJKLMN3")
        Me.Label13.Text = auth.Auth("200003@GHD", "icrop", "ABCDEFGHIJKLMN4")
        Me.Label14.Text = auth.Auth("200003@GHD", "icrop", "ABCDEFGHIJKLMN4")
        Me.Label15.Text = auth.Auth("200003", "icrop", "ABCDEFGHIJKLMN4")
        Me.Label16.Text = auth.Auth("200003@AAAAA", "icrop", "ABCDEFGHIJKLMN2")
        Me.Label17.Text = auth.Auth("200003@44B40", "icrop", "")
        Me.Label18.Text = auth.Auth("200003@44B40", "icrop", "")
        Me.Label19.Text = auth.Auth("200003@44B40", "icrop", "")
        Me.Label20.Text = auth.Auth("200003@44B40", "password", "")
        Me.Label21.Text = auth.Auth("200003@44B40", "icrop", "")
        Me.Label22.Text = auth.Auth("200003@44B40", "icrop", "")
        Me.Label23.Text = auth.Auth("200003@44B40", "icrop", "")
        Me.Label24.Text = auth.Auth("200003@44B40", "icrop", "")




    End Sub

End Class
