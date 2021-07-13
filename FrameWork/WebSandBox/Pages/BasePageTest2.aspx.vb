Imports Toyota.eCRB.SystemFrameworks.Web

Partial Class Test_BasePageTest2
    Inherits BasePage

    Protected Sub Page_Load(sender As Object, e As System.EventArgs) Handles Me.Load
        If Not Page.IsPostBack Then
            Dim hogehoge As List(Of String) = DirectCast(Session("hogehoge"), List(Of String))
            Me.Label1.Text = hogehoge.Count
        End If
    End Sub

    Protected Sub Button1_Click(sender As Object, e As System.EventArgs) Handles Button1.Click

        Dim sb As New StringBuilder
        sb.Append(GetValue(ScreenPos.Current, "Test1_Next", False) & vbCrLf)
        sb.Append(ContainsKey(ScreenPos.Current, "Test1_Next") & vbCrLf)
        'sb.Append(GetValue(ScreenPos.Current, "Test1_Next", True) & vbCrLf)
        'sb.Append(ContainsKey(ScreenPos.Current, "Test1_Next") & vbCrLf)

        Me.TextBox1.Text = sb.ToString

    End Sub

    Protected Sub Button2_Click(sender As Object, e As System.EventArgs) Handles Button2.Click

        SetValue(ScreenPos.Prev, "Test2_Prev", "Test2_Prev_Value")

    End Sub

    Protected Sub Button3_Click(sender As Object, e As System.EventArgs) Handles Button3.Click

        Dim sb As New StringBuilder
        sb.Append(GetValue(ScreenPos.Prev, "Test2_Prev", False) & vbCrLf)
        sb.Append(ContainsKey(ScreenPos.Prev, "Test2_Prev") & vbCrLf)
        'sb.Append(GetValue(ScreenPos.Prev, "Test2_Prev", True) & vbCrLf)
        'sb.Append(ContainsKey(ScreenPos.Prev, "Test2_Prev") & vbCrLf)

        Me.TextBox1.Text = sb.ToString

    End Sub

    Protected Sub Button5_Click(sender As Object, e As System.EventArgs) Handles Button5.Click
        RemoveValue(ScreenPos.Prev, "Test2_Prev")
        Me.TextBox1.Text = ContainsKey(ScreenPos.Prev, "Test2_Prev")
    End Sub

    Protected Sub Button4_Click(sender As Object, e As System.EventArgs) Handles Button4.Click

        Dim hogehoge As List(Of String) = DirectCast(Session("hogehoge"), List(Of String))
        hogehoge.RemoveAt(hogehoge.Count - 1)

        RedirectPrevScreen()
    End Sub

    Protected Sub Button7_Click(sender As Object, e As System.EventArgs) Handles Button7.Click
        Dim hogehoge As List(Of String) = DirectCast(Session("hogehoge"), List(Of String))
        hogehoge.Add("BasePageTest1")
        RedirectNextScreen("BasePageTest1")

    End Sub

    Protected Sub Button8_Click(sender As Object, e As System.EventArgs) Handles Button8.Click
        Me.TextBox1.Text = GetPrevScreenId
    End Sub

    Protected Sub Button9_Click(sender As Object, e As System.EventArgs) Handles Button9.Click
        Dim hogehoge As List(Of String) = DirectCast(Session("hogehoge"), List(Of String))
        hogehoge.RemoveRange(hogehoge.Count - 3, 3)
        RedirectPrevScreen(3)
    End Sub

End Class
