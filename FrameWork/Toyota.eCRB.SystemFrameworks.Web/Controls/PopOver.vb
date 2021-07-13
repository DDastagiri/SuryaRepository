Imports System.Web.UI.WebControls
Imports System.Web.UI
Imports System.Globalization

Namespace Toyota.eCRB.SystemFrameworks.Web.Controls

    Public Enum PopOverHeaderStyle
        Text
        None
        ClientId
    End Enum

    Public Class PopOver
        Inherits Panel

        Public Property HeaderStyle As PopOverHeaderStyle
            Get
                If ViewState("HeaderStyle") Is Nothing Then
                    Return PopOverHeaderStyle.Text
                Else
                    Return CType(ViewState("HeaderStyle"), PopOverHeaderStyle)
                End If
            End Get
            Set(value As PopOverHeaderStyle)
                ViewState("HeaderStyle") = value
            End Set
        End Property

        Public Property HeaderClientId As String
            Get
                If ViewState("HeaderClientId") Is Nothing Then
                    Return ""
                Else
                    Return CStr(ViewState("HeaderClientId"))
                End If
            End Get
            Set(value As String)
                ViewState("HeaderClientId") = value
            End Set
        End Property

        Public Property HeaderText As String
            Get
                If ViewState("HeaderText") Is Nothing Then
                    Return ""
                Else
                    Return CStr(ViewState("HeaderText"))
                End If
            End Get
            Set(value As String)
                ViewState("HeaderText") = value
            End Set
        End Property

        Public Property HeaderTextWordNo As Decimal
            Get
                If ViewState("HeaderTextWordNo") Is Nothing Then
                    Return Nothing
                Else
                    Return CDec(ViewState("HeaderTextWordNo"))
                End If
            End Get
            Set(value As Decimal)
                ViewState("HeaderTextWordNo") = value
            End Set
        End Property

        Public Property TriggerClientId As String
            Get
                If ViewState("TriggerClientId") Is Nothing Then
                    Return ""
                Else
                    Return CStr(ViewState("TriggerClientId"))
                End If
            End Get
            Set(value As String)
                ViewState("TriggerClientId") = value
            End Set
        End Property

        Public Property PreventTop As Boolean
            Get
                If ViewState("PreventTop") Is Nothing Then
                    Return False
                Else
                    Return CBool(ViewState("PreventTop"))
                End If
            End Get
            Set(value As Boolean)
                ViewState("PreventTop") = value
            End Set
        End Property

        Public Property PreventBottom As Boolean
            Get
                If ViewState("PreventBottom") Is Nothing Then
                    Return False
                Else
                    Return CBool(ViewState("PreventBottom"))
                End If
            End Get
            Set(value As Boolean)
                ViewState("PreventBottom") = value
            End Set
        End Property

        Public Property PreventLeft As Boolean
            Get
                If ViewState("PreventLeft") Is Nothing Then
                    Return False
                Else
                    Return CBool(ViewState("PreventLeft"))
                End If
            End Get
            Set(value As Boolean)
                ViewState("PreventLeft") = value
            End Set
        End Property

        Public Property PreventRight As Boolean
            Get
                If ViewState("PreventRight") Is Nothing Then
                    Return False
                Else
                    Return CBool(ViewState("PreventRight"))
                End If
            End Get
            Set(value As Boolean)
                ViewState("PreventRight") = value
            End Set
        End Property

        Public Property OnClientOpen As String
            Get
                If ViewState("OnClientOpen") Is Nothing Then
                    Return ""
                Else
                    Return CStr(ViewState("OnClientOpen"))
                End If
            End Get
            Set(value As String)
                ViewState("OnClientOpen") = value
            End Set
        End Property

        Public Property OnClientClose As String
            Get
                If ViewState("OnClientClose") Is Nothing Then
                    Return ""
                Else
                    Return CStr(ViewState("OnClientClose"))
                End If
            End Get
            Set(value As String)
                ViewState("OnClientClose") = value
            End Set
        End Property

        Protected Overrides Sub OnPreRender(e As System.EventArgs)
            MyBase.OnPreRender(e)

            If (Me.HeaderTextWordNo <> Nothing AndAlso Me.DesignMode = False) Then
                Me.HeaderText = WebWordUtility.GetWord(Me.HeaderTextWordNo)
            End If

            If (Me.TriggerClientId <> Nothing) Then
                'jquery plugin binding
                Dim options As New Dictionary(Of String, String)
                options.Add("id", Me.ClientID & "_popover")
                options.Add("@preventTop", Me.PreventTop.ToString().ToLower(CultureInfo.InvariantCulture))
                options.Add("@preventBottom", Me.PreventBottom.ToString().ToLower(CultureInfo.InvariantCulture))
                options.Add("@preventLeft", Me.PreventLeft.ToString().ToLower(CultureInfo.InvariantCulture))
                options.Add("@preventRight", Me.PreventRight.ToString().ToLower(CultureInfo.InvariantCulture))
                If (Me.HeaderStyle = PopOverHeaderStyle.ClientId) Then
                    options.Add("header", "#" & Me.HeaderClientId)
                Else
                    options.Add("header", "#" & Me.ClientID & "_header")
                End If
                options.Add("content", "#" & Me.ClientID & "_content")
                If (Me.OnClientOpen <> Nothing) Then
                    options.Add("@openEvent", Me.OnClientOpen)
                End If
                If (Me.OnClientClose <> Nothing) Then
                    options.Add("@closeEvent", Me.OnClientClose)
                End If

                Page.ClientScript.RegisterStartupScript(Me.GetType(), Me.ClientID, Common.GetJqueryPluginBinding(Me.TriggerClientId, "popover", options, True), True)
            End If
        End Sub

        Protected Overrides Sub AddAttributesToRender(writer As System.Web.UI.HtmlTextWriter)
            MyBase.AddAttributesToRender(writer)
            writer.AddAttribute("data-TriggerClientID", Me.TriggerClientId)

            If (Me.CssClass <> Nothing) Then
                writer.AddAttribute("class", "popover " & Me.CssClass)
            Else
                writer.AddAttribute("class", "popover")
            End If
        End Sub

        Protected Overrides Sub RenderContents(writer As System.Web.UI.HtmlTextWriter)

            If (Me.HeaderStyle = PopOverHeaderStyle.None OrElse Me.HeaderStyle = PopOverHeaderStyle.Text) Then
                writer.AddAttribute("id", Me.ClientID & "_header")
                writer.RenderBeginTag("div")
                If (Me.HeaderStyle = PopOverHeaderStyle.Text) Then
                    writer.WriteEncodedText(Me.HeaderText)
                End If
                writer.RenderEndTag()
            End If

            writer.AddAttribute("id", Me.ClientID & "_content")
            writer.RenderBeginTag("div")
            MyBase.RenderContents(writer)
            writer.RenderEndTag()
        End Sub

    End Class
End Namespace
