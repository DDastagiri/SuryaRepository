Imports System.Globalization
Imports System.Web.UI.WebControls
Imports System.Web.UI

Namespace Toyota.eCRB.SystemFrameworks.Web.Controls
    Public Class CheckButton
        Inherits CheckBox

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

        Public Property OffIconUrl As String
            Get
                If (ViewState("OffIconUrl") Is Nothing) Then
                    Return ""
                Else
                    Return CStr(ViewState("OffIconUrl"))
                End If
            End Get
            Set(value As String)
                ViewState("OffIconUrl") = value
            End Set
        End Property

        Public Property OnIconUrl As String
            Get
                If (ViewState("OnIconUrl") Is Nothing) Then
                    Return ""
                Else
                    Return CStr(ViewState("OnIconUrl"))
                End If
            End Get
            Set(value As String)
                ViewState("OnIconUrl") = value
            End Set
        End Property

        Protected Overrides Sub OnPreRender(e As System.EventArgs)
            MyBase.OnPreRender(e)

            Me.Text = ""

            Me.InputAttributes.CssStyle.Add("display", "inline-block")
            Me.InputAttributes.CssStyle.Add("width", Me.Width.ToString())
            Me.InputAttributes.CssStyle.Add("height", Me.Height.ToString())

            'jquery plugin binding
            Dim options As New Dictionary(Of String, String)
            If (Not Me.DesignMode AndAlso Me.TextWordNo <> Nothing) Then
                options.Add("label", WebWordUtility.GetWord(Me.TextWordNo))
            End If
            options.Add("offIconUrl", ResolveClientUrl(Me.OffIconUrl))
            options.Add("onIconUrl", ResolveClientUrl(Me.OnIconUrl))
            If (Me.AutoPostBack) Then
                options.Add("@check", String.Format(CultureInfo.InvariantCulture, "function() {{ {0} }}", Page.ClientScript.GetPostBackEventReference(Me, "")))
            End If
            Page.ClientScript.RegisterStartupScript(Me.GetType(), Me.ClientID, Common.GetJqueryPluginBinding(Me.ClientID, "CheckButton", options, Me.Enabled), True)
        End Sub

    End Class

End Namespace

