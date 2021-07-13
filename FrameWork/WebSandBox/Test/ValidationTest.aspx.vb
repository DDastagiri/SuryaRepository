Option Strict On
Option Explicit On

Imports Toyota.eCRB.SystemFrameworks.Core

Partial Class ValidationTest
    Inherits System.Web.UI.Page

    Protected Sub Button1_Click(sender As Object, e As System.EventArgs) Handles Button1.Click

        'Me.Label1.Text = CStr(Validation.IsCorrectDigit(String.Empty, 3))
        'Me.Label2.Text = CStr(Validation.IsCorrectDigit(Nothing, 3))
        'Me.Label3.Text = CStr(Validation.IsCorrectDigit("あいう", -1))
        'Me.Label4.Text = CStr(Validation.IsCorrectDigit("あいう", 0))
        'Me.Label5.Text = CStr(Validation.IsCorrectDigit("あいう", 2))
        'Me.Label6.Text = CStr(Validation.IsCorrectDigit("あいう", 3))
        'Me.Label7.Text = CStr(Validation.IsCorrectDigit("あいう", 4))

        'Me.Label8.Text = CStr(Validation.IsCorrectByte(String.Empty, 3))
        'Me.Label9.Text = CStr(Validation.IsCorrectByte(Nothing, 3))
        'Me.Label10.Text = CStr(Validation.IsCorrectByte("あいう", -1))
        'Me.Label11.Text = CStr(Validation.IsCorrectByte("あいう", 0))
        'Me.Label12.Text = CStr(Validation.IsCorrectByte("あいう", 8))
        'Me.Label13.Text = CStr(Validation.IsCorrectByte("あいう", 9))
        'Me.Label14.Text = CStr(Validation.IsCorrectByte("あいう", 10))

        'Me.Label15.Text = CStr(Validation.IsDate(1, String.Empty))
        'Me.Label16.Text = CStr(Validation.IsDate(1, Nothing))
        'Me.Label17.Text = CStr(Validation.IsDate(1, "28/11/2011 12:13:14"))
        'Me.Label18.Text = CStr(Validation.IsDate(1, "2011/11/28 12:13:14"))
        'Me.Label19.Text = CStr(Validation.IsDate(15, "28112011121314"))
        'Me.Label20.Text = CStr(Validation.IsDate(15, "20131128121314"))

        'Me.Label21.Text = CStr(Validation.IsMail(String.Empty))
        'Me.Label22.Text = CStr(Validation.IsMail(Nothing))
        'Me.Label23.Text = CStr(Validation.IsMail("aaaaa@bbbbb.com"))
        'Me.Label24.Text = CStr(Validation.IsMail("aaaabbbb.com"))
        'Me.Label25.Text = CStr(Validation.IsMail("aaaabbbb.com"))

        'Me.Label26.Text = CStr(Validation.IsRegNo(String.Empty))
        'Me.Label27.Text = CStr(Validation.IsRegNo(Nothing))
        'Me.Label28.Text = CStr(Validation.IsRegNo("reg00001"))
        'Me.Label29.Text = CStr(Validation.IsRegNo("00001"))
        'Me.Label30.Text = CStr(Validation.IsRegNo("00001"))

        'Me.Label31.Text = CStr(Validation.IsVin(String.Empty))
        'Me.Label32.Text = CStr(Validation.IsVin(Nothing))
        'Me.Label33.Text = CStr(Validation.IsVin("VIN00001"))
        'Me.Label34.Text = CStr(Validation.IsVin("00001"))
        'Me.Label35.Text = CStr(Validation.IsVin("00001"))

        Me.Label36.Text = CStr(Validation.IsPhoneNumber(String.Empty))
        Me.Label37.Text = CStr(Validation.IsPhoneNumber(Nothing))
        Me.Label38.Text = CStr(Validation.IsPhoneNumber("052-111-2222"))
        Me.Label39.Text = CStr(Validation.IsPhoneNumber("a90"))
        Me.Label40.Text = CStr(Validation.IsPhoneNumber("0000000000"))

        Me.Label41.Text = CStr(Validation.IsMobilePhoneNumber(String.Empty))
        Me.Label42.Text = CStr(Validation.IsMobilePhoneNumber(Nothing))
        Me.Label43.Text = CStr(Validation.IsMobilePhoneNumber("090-1111-2222"))
        Me.Label44.Text = CStr(Validation.IsMobilePhoneNumber("a090"))
        Me.Label45.Text = CStr(Validation.IsMobilePhoneNumber("00000000000"))

        'Me.Label46.Text = CStr(Validation.IsPostalCode(String.Empty))
        'Me.Label47.Text = CStr(Validation.IsPostalCode(Nothing))
        'Me.Label48.Text = CStr(Validation.IsPostalCode("111-2222"))
        'Me.Label49.Text = CStr(Validation.IsPostalCode("1112222"))
        'Me.Label50.Text = CStr(Validation.IsPostalCode("1112222"))

        'Me.Label51.Text = CStr(Validation.IsValidString(String.Empty))
        'Me.Label52.Text = CStr(Validation.IsValidString(Nothing))
        'Me.Label53.Text = CStr(Validation.IsValidString("あ"))
        'Me.Label54.Text = CStr(Validation.IsValidString("い"))
        'Me.Label55.Text = CStr(Validation.IsValidString("い"))

    End Sub

End Class
