Imports System.Text
Imports System.Web.Security
Imports Toyota.eCRB.SystemFrameworks.Core.iCROP.BizLogic
Imports System.Globalization

Namespace Toyota.eCRB.SystemFrameworks.Web

    ''' <summary>
    ''' アクセス拒否の際に表示するページを処理します。
    ''' </summary>
    ''' <remarks></remarks>
    Partial Public Class SC3010302
        Inherits System.Web.UI.Page

        Private Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

            If (Me.IsPostBack) Then
                Return
            End If

            'スローされた最後の例外を削除
            Server.ClearError()
        End Sub

        Protected Sub BackButton_Click(ByVal sender As Object, ByVal e As System.EventArgs)
            Dim redirectScript As String = String.Format(CultureInfo.InvariantCulture, "var w = window.parent || window; w.location.href = '{0}';", ResolveClientUrl(EnvironmentSetting.LoginUrl))
            ClientScript.RegisterStartupScript(Me.GetType(), "redirect", redirectScript, True)
        End Sub

    End Class

End Namespace

