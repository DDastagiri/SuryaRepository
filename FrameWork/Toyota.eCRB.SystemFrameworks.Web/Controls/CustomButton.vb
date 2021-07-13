Imports System.Web.UI.WebControls
Imports System.Web.UI
Imports System.Globalization

Namespace Toyota.eCRB.SystemFrameworks.Web.Controls
    Public Class CustomButton
        Inherits WebControl
        Implements IPostBackEventHandler

        Public Event Click As EventHandler

        Public Property OnClientClick As String
            Get
                If ViewState("OnClientClick") Is Nothing Then
                    Return ""
                Else
                    Return CStr(ViewState("OnClientClick"))
                End If
            End Get
            Set(value As String)
                ViewState("OnClientClick") = value
            End Set
        End Property

        Public Property CausesPostBack As Boolean
            Get
                If ViewState("CausesPostBack") Is Nothing Then
                    Return True
                Else
                    Return CBool(ViewState("CausesPostBack"))
                End If
            End Get
            Set(value As Boolean)
                ViewState("CausesPostBack") = value
            End Set
        End Property

        Public Property Text As String
            Get
                If ViewState("Text") Is Nothing Then
                    Return ""
                Else
                    Return CStr(ViewState("Text"))
                End If
            End Get
            Set(value As String)
                ViewState("Text") = value
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

        Public Property IconUrl As String
            Get
                If (ViewState("IconUrl") Is Nothing) Then
                    Return ""
                Else
                    Return CStr(ViewState("IconUrl"))
                End If
            End Get
            Set(value As String)
                ViewState("IconUrl") = value
            End Set
        End Property

        Public Property BadgeCount As Integer
            Get
                If ViewState("BadgeCount") Is Nothing Then
                    Return Nothing
                Else
                    Return CInt(ViewState("BadgeCount"))
                End If
            End Get
            Set(value As Integer)
                ViewState("BadgeCount") = value
            End Set
        End Property

        '2012/07/06 KN 小澤 STEP2対応 START
        Public Property ButtonId As String
            Get
                If (ViewState("ButtonId") Is Nothing) Then
                    Return ""
                Else
                    Return CStr(ViewState("ButtonId"))
                End If
            End Get
            Set(value As String)
                ViewState("ButtonId") = value
            End Set
        End Property

        Public Property ArrowMarginLeft As Integer
            Get
                If (ViewState("ArrowMarginLeft") Is Nothing) Then
                    Return Nothing
                Else
                    Return CInt(ViewState("ArrowMarginLeft"))
                End If
            End Get
            Set(value As Integer)
                ViewState("ArrowMarginLeft") = value
            End Set
        End Property
        '2012/07/06 KN 小澤 STEP2対応 END

        Public Sub New()
            Me.BadgeCount = 0
        End Sub

        Protected Overrides Sub OnPreRender(e As System.EventArgs)
            MyBase.OnPreRender(e)

            If (Not Me.DesignMode AndAlso Me.TextWordNo <> Nothing) Then
                Me.Text = WebWordUtility.GetWord(Me.TextWordNo)
            End If

            'jquery plugin binding
            Dim options As New Dictionary(Of String, String)
            options.Add("label", Me.Text)
            options.Add("iconUrl", ResolveClientUrl(Me.IconUrl))
            options.Add("@badgeCount", Me.BadgeCount.ToString(CultureInfo.InvariantCulture))
            If (Me.CausesPostBack) Then
                If (Not String.IsNullOrEmpty(Me.OnClientClick)) Then
                    options.Add("@click", String.Format(CultureInfo.InvariantCulture, "function(e) {{ var ret = (function(event) {{ {0} }})(e); if (ret) {1}; }}", Me.OnClientClick, Page.ClientScript.GetPostBackEventReference(Me, "")))
                Else
                    options.Add("@click", String.Format(CultureInfo.InvariantCulture, "function(e) {{ {0} }}", Page.ClientScript.GetPostBackEventReference(Me, "")))
                End If
            End If
            '2012/07/06 KN 小澤 STEP2対応 START
            options.Add("buttonId", Me.ButtonId)
            options.Add("arrowMarginLeft", Me.ArrowMarginLeft.ToString(CultureInfo.InvariantCulture))
            '2012/07/06 KN 小澤 STEP2対応 END
            Page.ClientScript.RegisterStartupScript(Me.GetType(), Me.ClientID, Common.GetJqueryPluginBinding(Me.ClientID, "CustomButton", options, Me.Enabled), True)
        End Sub

        Protected Overrides ReadOnly Property TagKey As System.Web.UI.HtmlTextWriterTag
            Get
                Return System.Web.UI.HtmlTextWriterTag.Button
            End Get
        End Property

        Protected Overrides Sub AddAttributesToRender(writer As System.Web.UI.HtmlTextWriter)
            MyBase.AddAttributesToRender(writer)
            writer.AddAttribute("type", "button")
        End Sub

        Public Sub RaisePostBackEvent(eventArgument As String) Implements System.Web.UI.IPostBackEventHandler.RaisePostBackEvent
            RaiseEvent Click(Me, EventArgs.Empty)
        End Sub
    End Class

End Namespace

