Imports System.Text
Imports System.Web.HttpContext
Imports System.Web.Security
Imports Toyota.eCRB.SystemFrameworks.Core.iCROP.BizLogic
Imports Toyota.eCRB.SystemFrameworks.Core
Imports System.Web.UI.WebControls
Imports System.Globalization

Namespace Toyota.eCRB.SystemFrameworks.Web

    ''' <summary>
    ''' �������G���[�����������ꍇ�ɕ\�������y�[�W���������܂��B
    ''' </summary>
    ''' <remarks></remarks>
    Partial Public Class SC3010304
        Inherits System.Web.UI.Page

        Protected errorMessagePanel As Panel
        Protected errorMessage As Label

        Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
            If (Me.IsPostBack) Then
                Return
            End If

            '�X���[���ꂽ�Ō�̗�O���폜
            Server.ClearError()

            '�N���C�A���g�����JavaScript�G���[
            If (Request.Form("ClientError") IsNot Nothing) Then
                Logger.Warn("Script error occured on client: " & CStr(Request.Form("ClientError")))
            End If

            'Global.asax����̋N���G���[
            Dim exceptionOccured As Exception = BaseHttpApplication.StartupException
            If (exceptionOccured IsNot Nothing) Then
                errorMessagePanel.Visible = True
                errorMessage.Text = Server.HtmlEncode(exceptionOccured.Message)
            End If

        End Sub

        Protected Sub BackButton_Click(ByVal sender As Object, ByVal e As System.EventArgs)
            Dim redirectScript As String = String.Format(CultureInfo.InvariantCulture, "var w = window.parent || window; w.location.href = '{0}';", ResolveClientUrl(EnvironmentSetting.LoginUrl))
            ClientScript.RegisterStartupScript(Me.GetType(), "redirect", redirectScript, True)
        End Sub

    End Class
End Namespace
