
Imports Toyota.eCRB.SystemFrameworks.Core

Partial Class Test_DateTimeFuncTest
    Inherits System.Web.UI.Page

    Protected Sub Button1_Click(sender As Object, e As System.EventArgs) Handles Button1.Click

        Me.Label1.Text = DateTimeFunc.FormatDate(1, Date.Now)
        Me.Label2.Text = DateTimeFunc.FormatDate(2, Date.Now)
        Me.Label3.Text = DateTimeFunc.FormatDate(3, Date.Now)
        Me.Label4.Text = DateTimeFunc.FormatDate(4, Date.Now)
        Me.Label5.Text = DateTimeFunc.FormatDate(5, Date.Now)
        Me.Label6.Text = DateTimeFunc.FormatDate(6, Date.Now)
        Me.Label7.Text = DateTimeFunc.FormatDate(7, Date.Now)
        Me.Label8.Text = DateTimeFunc.FormatDate(8, Date.Now)
        Me.Label9.Text = DateTimeFunc.FormatDate(9, Date.Now)
        Me.Label10.Text = DateTimeFunc.FormatDate(10, Date.Now)
        Me.Label11.Text = DateTimeFunc.FormatDate(11, Date.Now)
        Me.Label12.Text = DateTimeFunc.FormatDate(12, Date.Now)
        Me.Label13.Text = DateTimeFunc.FormatDate(13, Date.Now)
        Me.Label14.Text = DateTimeFunc.FormatDate(14, Date.Now)
        Me.Label15.Text = DateTimeFunc.FormatDate(15, Date.Now)
        Me.Label16.Text = DateTimeFunc.FormatDate(16, Date.Now)
        Me.Label17.Text = DateTimeFunc.FormatDate(17, Date.Now)
        Me.Label18.Text = DateTimeFunc.FormatDate(18, Date.Now)
        Me.Label19.Text = DateTimeFunc.FormatDate(19, Date.Now)
        Me.Label20.Text = DateTimeFunc.FormatDate(20, Date.Now)
        Me.Label21.Text = DateTimeFunc.FormatDate(21, Date.Now)
        Me.Label22.Text = DateTimeFunc.FormatDate(22, Date.Now)

        Dim hoge1 As Date = DateTimeFunc.FormatString("yyyy/MM/dd HH:mm", "2011/12/13 14:15")
        Dim hoge2 As Date = DateTimeFunc.FormatString("yyyyMMddHHmm", "201112131415")
        Dim hoge3 As Date = DateTimeFunc.FormatString("dd/MM/yyyy HH:mm", "13/12/2011 14:15")
        Dim hoge4 As Date = DateTimeFunc.FormatString("ddMMyyyyHHmm", "131220111415")
        Dim hoge5 As Date = DateTimeFunc.FormatString("MM/dd/yyyy HH:mm", "12/13/2011 14:15")
        Dim hoge6 As Date = DateTimeFunc.FormatString("MMddyyyyHHmm", "121320111415")
        Dim hoge7 As Date = DateTimeFunc.FormatString("yy/MM/dd HH:mm", "11/12/13 14:15")
        Dim hoge8 As Date = DateTimeFunc.FormatString("yyMMddHHmm", "1112131415")
        Dim hoge9 As Date = DateTimeFunc.FormatString("mmddyyyyHHMM", "151320111412")
        Try
            Dim hoge10 As Date = DateTimeFunc.FormatString(Nothing, "2011/12/13 14:15")
        Catch ex As Exception

        End Try
        Try
            Dim hoge11 As Date = DateTimeFunc.FormatString("yyyy/MM/dd HH:mm", Nothing)
        Catch ex As Exception

        End Try
        Try
            Dim hoge12 As Date = DateTimeFunc.FormatString("yyyy/MM/dd HH:mm", "2011/13/15 14:15")
        Catch ex As Exception

        End Try
        Dim hoge13 As Date = DateTimeFunc.FormatString("yyyy/MM/dd HH:mm:ss", "2011/12/13 14:15:16")

        Dim hoge14 As Date = DateTimeFunc.Now()
        Dim hoge15 As Date = DateTimeFunc.Now("44B40", "000")
        Dim hoge16 As Date = DateTimeFunc.Now("44B40", "01")
        Dim hoge17 As Date = DateTimeFunc.Now("11A20", "01")
        Dim hoge18 As Date = DateTimeFunc.Now("", "000")
        Dim hoge19 As Date = DateTimeFunc.Now("44B40", "")



    End Sub

End Class

