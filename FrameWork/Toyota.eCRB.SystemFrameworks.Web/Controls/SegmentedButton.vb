Imports System.Globalization
Imports System.Web.UI.WebControls
Imports System.Web.UI

Namespace Toyota.eCRB.SystemFrameworks.Web.Controls
    Public Class SegmentedButton
        Inherits RadioButtonList

        Public Overrides Property RepeatLayout As System.Web.UI.WebControls.RepeatLayout
            Get
                Return RepeatLayout.UnorderedList
            End Get
            Set(value As System.Web.UI.WebControls.RepeatLayout)
                MyBase.RepeatLayout = value
            End Set
        End Property

        Public Property OnClientSelect As String
            Get
                If ViewState("OnClientSelect") Is Nothing Then
                    Return ""
                Else
                    Return CStr(ViewState("OnClientSelect"))
                End If
            End Get
            Set(value As String)
                ViewState("OnClientSelect") = value
            End Set
        End Property

        Protected Overrides Sub OnPreRender(e As System.EventArgs)
            MyBase.OnPreRender(e)

            'jquery plugin binding
            Dim options As New Dictionary(Of String, String)
            options.Add("formName", Me.UniqueID)
            If (Me.OnClientSelect <> Nothing) Then
                If (Me.AutoPostBack) Then
                    options.Add("@select", String.Format(CultureInfo.InvariantCulture, "function(value) {{ {0}(value); {1}; }}", Me.OnClientSelect, Page.ClientScript.GetPostBackEventReference(Me, "")))
                Else
                    options.Add("@select", String.Format(CultureInfo.InvariantCulture, "function(value) {{ {0}(value); }}", Me.OnClientSelect))
                End If
            End If
            Page.ClientScript.RegisterStartupScript(Me.GetType(), Me.ClientID, Common.GetJqueryPluginBinding(Me.ClientID, "SegmentedButton", options, Me.Enabled), True)
        End Sub
    End Class
End Namespace

