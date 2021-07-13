Imports System.Web.UI.WebControls
Imports System.Web.UI
Imports System.ComponentModel
Imports System.Web

Namespace Toyota.eCRB.SystemFrameworks.Web.Controls
    Public Class CustomLabel
        Inherits Label

        Public Property UseEllipsis As Boolean
            Get
                If ViewState("UseEllipsis") Is Nothing Then
                    Return False
                Else
                    Return CBool(ViewState("UseEllipsis"))
                End If
            End Get
            Set(value As Boolean)
                ViewState("UseEllipsis") = value
            End Set
        End Property

        Public Property TextWordNo As Decimal
            Get
                If ViewState("TextWordNo") Is Nothing Then
                    Return Nothing
                Else
                    Return CDec(ViewState("TextWordNo"))
                End If
            End Get
            Set(value As Decimal)
                ViewState("TextWordNo") = value
            End Set
        End Property

        Protected Overrides Sub OnPreRender(e As System.EventArgs)
            MyBase.OnPreRender(e)

            If (Not Me.DesignMode AndAlso Me.TextWordNo <> Nothing) Then
                Me.Text = HttpUtility.HtmlEncode(WebWordUtility.GetWord(Me.TextWordNo))
            End If

            If (Me.UseEllipsis AndAlso Me.Width <> Unit.Empty) Then
                'jquery plugin binding
                Dim options As New Dictionary(Of String, String)
                options.Add("useEllipsis", "true")
                Page.ClientScript.RegisterStartupScript(Me.GetType(), Me.ClientID, Common.GetJqueryPluginBinding(Me.ClientID, "CustomLabel", options, True), True)
            End If
        End Sub
    End Class
End Namespace

