Imports Toyota.eCRB.SystemFrameworks.Web

Partial Class Test_BasePageTest1
    Inherits BasePage
    Implements ISafeInputForm

    Protected Sub Page_Load(sender As Object, e As System.EventArgs) Handles Me.Load
        If Not Page.IsPostBack Then
            Dim hogehoge As List(Of String) = DirectCast(Session("hogehoge"), List(Of String))
            Me.Label1.Text = hogehoge.Count
        End If
    End Sub

    Protected Sub Button1_Click(sender As Object, e As System.EventArgs) Handles Button1.Click

        SetValue(ScreenPos.Current, "Test1_Current", "Test1_Current_value")

    End Sub

    Protected Sub Button2_Click(sender As Object, e As System.EventArgs) Handles Button2.Click

        Dim sb As New StringBuilder
        sb.Append(GetValue(ScreenPos.Current, "Test1_Current", False) & vbCrLf)
        sb.Append(ContainsKey(ScreenPos.Current, "Test1_Current") & vbCrLf)
        'sb.Append(GetValue(ScreenPos.Current, "Test1_Current", True) & vbCrLf)
        'sb.Append(ContainsKey(ScreenPos.Current, "Test1_Current") & vbCrLf)

        Me.TextBox1.Text = sb.ToString
    End Sub

    Protected Sub Button10_Click(sender As Object, e As System.EventArgs) Handles Button10.Click
        RemoveValue(ScreenPos.Current, "Test1_Current")
        Me.TextBox1.Text = ContainsKey(ScreenPos.Current, "Test1_Current")
    End Sub

    Protected Sub Button3_Click(sender As Object, e As System.EventArgs) Handles Button3.Click
        SetValue(ScreenPos.Next, "Test1_Next", "Test1_Next_value")
    End Sub

    Protected Sub Button4_Click(sender As Object, e As System.EventArgs) Handles Button4.Click

        Dim sb As New StringBuilder
        sb.Append(GetValue(ScreenPos.Next, "Test1_Next", False) & vbCrLf)
        sb.Append(ContainsKey(ScreenPos.Next, "Test1_Next") & vbCrLf)
        'sb.Append(GetValue(ScreenPos.Next, "Test1_Next", True) & vbCrLf)
        'sb.Append(ContainsKey(ScreenPos.Next, "Test1_Next") & vbCrLf)

        Me.TextBox1.Text = sb.ToString

    End Sub

    Protected Sub Button11_Click(sender As Object, e As System.EventArgs) Handles Button11.Click
        RemoveValue(ScreenPos.Next, "Test1_Next")
        Me.TextBox1.Text = ContainsKey(ScreenPos.Next, "Test1_Next")
    End Sub

    Protected Sub Button5_Click(sender As Object, e As System.EventArgs) Handles Button5.Click
        Dim hogehoge As List(Of String) = DirectCast(Session("hogehoge"), List(Of String))
        hogehoge.Add("BasePageTest2")
        RedirectNextScreen("BasePageTest2")
    End Sub

    Protected Sub Button6_Click(sender As Object, e As System.EventArgs) Handles Button6.Click

        SetValue(ScreenPos.Last, "Test1_Last", "Test1_Last_Value")

    End Sub

    Protected Sub Button7_Click(sender As Object, e As System.EventArgs) Handles Button7.Click

        Dim sb As New StringBuilder
        sb.Append(GetValue(ScreenPos.Last, "Test1_Last", False) & vbCrLf)
        sb.Append(ContainsKey(ScreenPos.Last, "Test1_Last") & vbCrLf)
        'sb.Append(GetValue(ScreenPos.Last, "Test1_Last", True) & vbCrLf)
        'sb.Append(ContainsKey(ScreenPos.Last, "Test1_Last") & vbCrLf)

        Me.TextBox1.Text = sb.ToString

    End Sub

    Protected Sub Button9_Click(sender As Object, e As System.EventArgs) Handles Button9.Click

        Dim sb As New StringBuilder
        sb.Append(GetValue(ScreenPos.Current, "Test1_Last", False) & vbCrLf)
        sb.Append(ContainsKey(ScreenPos.Current, "Test1_Last") & vbCrLf)
        'sb.Append(GetValue(ScreenPos.Current, "Test1_Last", True) & vbCrLf)
        'sb.Append(ContainsKey(ScreenPos.Current, "Test1_Last") & vbCrLf)

        Me.TextBox1.Text = sb.ToString

    End Sub

    Protected Sub Button12_Click(sender As Object, e As System.EventArgs) Handles Button12.Click
        RemoveValue(ScreenPos.Last, "Test1_Last")
        Me.TextBox1.Text = ContainsKey(ScreenPos.Current, "Test1_Last")
    End Sub


    Protected Sub Button8_Click(sender As Object, e As System.EventArgs) Handles Button8.Click

        Dim hogehoge As List(Of String) = DirectCast(Session("hogehoge"), List(Of String))
        hogehoge.RemoveAt(hogehoge.Count - 1)
        RedirectPrevScreen()

    End Sub

    Protected Sub Button13_Click(sender As Object, e As System.EventArgs) Handles Button13.Click

        Dim sb As New StringBuilder
        sb.Append(GetValue(ScreenPos.Current, "Test2_Prev", False) & vbCrLf)
        sb.Append(ContainsKey(ScreenPos.Current, "Test2_Prev") & vbCrLf)

        Me.TextBox1.Text = sb.ToString

    End Sub

    Protected Sub Button14_Click(sender As Object, e As System.EventArgs) Handles Button14.Click
        Me.TextBox1.Text = GetPrevScreenId
    End Sub

    Protected Sub Button15_Click(sender As Object, e As System.EventArgs) Handles Button15.Click
        Dim hogehoge As List(Of String) = DirectCast(Session("hogehoge"), List(Of String))
        hogehoge.RemoveRange(hogehoge.Count - 3, 3)
        RedirectPrevScreen(3)
    End Sub

    Protected Sub Button16_Click(sender As Object, e As System.EventArgs) Handles Button16.Click
        OpenDialog("BasePageTest3", DialogEffect.FadeIn)
    End Sub

    Protected Sub Button17_Click(sender As Object, e As System.EventArgs) Handles Button17.Click
        OpenDialog("BasePageTest3", DialogEffect.Top)
    End Sub

    Protected Sub Button18_Click(sender As Object, e As System.EventArgs) Handles Button18.Click
        OpenDialog("BasePageTest3", DialogEffect.Left)
    End Sub

    Protected Sub Button19_Click(sender As Object, e As System.EventArgs) Handles Button19.Click
        OpenDialog("BasePageTest3", DialogEffect.Right)
    End Sub

    Protected Sub Button20_Click(sender As Object, e As System.EventArgs) Handles Button20.Click
        OpenDialog("BasePageTest3", DialogEffect.Bottom)
    End Sub

    Protected Sub Button21_Click(sender As Object, e As System.EventArgs) Handles Button21.Click
        ShowMessageBox("999", "111", 0, Nothing)
    End Sub
End Class
