Imports System.Globalization
Imports System.Web.UI.WebControls
Imports System.Web.UI
Imports System.Web.Script.Serialization

Namespace Toyota.eCRB.SystemFrameworks.Web.Controls
    Public Enum PopOverFormHeaderStyle
        Text
        None
    End Enum

    Public Class PopOverForm
        Inherits WebControl
        Implements ICallbackEventHandler, IPostBackDataHandler

        Public Event ValueChanged As EventHandler

        Public Event ClientCallback As EventHandler(Of ClientCallbackEventArgs)

        Public Property HeaderStyle As PopOverFormHeaderStyle
            Get
                If ViewState("HeaderStyle") Is Nothing Then
                    Return PopOverFormHeaderStyle.Text
                Else
                    Return CType(ViewState("HeaderStyle"), PopOverFormHeaderStyle)
                End If
            End Get
            Set(value As PopOverFormHeaderStyle)
                ViewState("HeaderStyle") = value
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

        Public Property PageCapacity As Integer
            Get
                If ViewState("PageCapacity") Is Nothing Then
                    Return 5
                Else
                    Return CInt(ViewState("PageCapacity"))
                End If
            End Get
            Set(value As Integer)
                ViewState("PageCapacity") = value
            End Set
        End Property

        Public Property TriggerClientId As String
            Get
                If ViewState("TriggerClientID") Is Nothing Then
                    Return ""
                Else
                    Return CStr(ViewState("TriggerClientID"))
                End If
            End Get
            Set(value As String)
                ViewState("TriggerClientID") = value
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

        Public Property Value As String
            Get
                If ViewState("Value") Is Nothing Then
                    Return ""
                Else
                    Return CStr(ViewState("Value"))
                End If
            End Get
            Set(_value As String)
                ViewState("Value") = _value
            End Set
        End Property

        Public Property OnClientRender As String
            Get
                If ViewState("OnClientRender") Is Nothing Then
                    Return ""
                Else
                    Return CStr(ViewState("OnClientRender"))
                End If
            End Get
            Set(value As String)
                ViewState("OnClientRender") = value
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
            If (Me.HeaderTextWordNo <> Nothing AndAlso Me.DesignMode = False) Then
                Me.HeaderText = WebWordUtility.GetWord(Me.HeaderTextWordNo)
            End If

            Dim callback As String = "$.PopOverForm.getCallbackResponseFromServer"
            Dim callbackArguments As String = "$.PopOverForm.getCallbackArguments('" & Me.ClientID & "')"

            'jquery plugin binding
            Dim options As New Dictionary(Of String, String)
            options.Add("@preventTop", Me.PreventTop.ToString().ToLower(CultureInfo.InvariantCulture))
            options.Add("@preventBottom", Me.PreventBottom.ToString().ToLower(CultureInfo.InvariantCulture))
            options.Add("@preventLeft", Me.PreventLeft.ToString().ToLower(CultureInfo.InvariantCulture))
            options.Add("@preventRight", Me.PreventRight.ToString().ToLower(CultureInfo.InvariantCulture))
            options.Add("@pageCapacity", Me.PageCapacity.ToString(CultureInfo.InvariantCulture))
            options.Add("@postbackToServer", String.Format(CultureInfo.InvariantCulture, "function() {{ {0}; }}", Page.ClientScript.GetPostBackEventReference(Me, "")))
            options.Add("@callbackToServer", String.Format(CultureInfo.InvariantCulture, "function() {{ {0}; }}", Page.ClientScript.GetCallbackEventReference(Me, callbackArguments, callback, """" & Me.ClientID & """", True)))
            options.Add("@render", Me.OnClientRender)
            If Not String.IsNullOrEmpty(Me.OnClientOpen) Then
                options.Add("@open", Me.OnClientOpen)
            End If
            If Not String.IsNullOrEmpty(Me.OnClientClose) Then
                options.Add("@close", Me.OnClientClose)
            End If
            Page.ClientScript.RegisterStartupScript(Me.GetType(), Me.ClientID, Common.GetJqueryPluginBinding(Me.ClientID, "PopOverForm", options, True), True)
        End Sub

        Protected Overrides Sub AddAttributesToRender(writer As System.Web.UI.HtmlTextWriter)
            MyBase.AddAttributesToRender(writer)
            writer.AddAttribute("data-TriggerClientID", Me.TriggerClientId)
        End Sub

        Protected Overrides ReadOnly Property TagKey As System.Web.UI.HtmlTextWriterTag
            Get
                Return System.Web.UI.HtmlTextWriterTag.Div
            End Get
        End Property


        Protected Overrides Sub RenderContents(writer As System.Web.UI.HtmlTextWriter)
            'postBackData
            Common.RenderHiddenField(writer, Me.UniqueID, Me.Value)


            'header
            writer.AddAttribute("class", "icrop-PopOverForm-header")
            writer.AddStyleAttribute("width", Me.Width.ToString())
            writer.RenderBeginTag("div")

            If (Me.HeaderStyle <> PopOverFormHeaderStyle.None) Then
                writer.AddAttribute("class", "icrop-PopOverForm-header-left")
                writer.RenderBeginTag("div")
                writer.RenderEndTag()

                writer.AddAttribute("class", "icrop-PopOverForm-header-title")
                writer.RenderBeginTag("div")
                If (Me.HeaderStyle = PopOverFormHeaderStyle.Text) Then
                    writer.WriteEncodedText(Me.HeaderText)
                End If
                writer.RenderEndTag()

                writer.AddAttribute("class", "icrop-PopOverForm-header-right")
                writer.RenderBeginTag("div")
                writer.RenderEndTag()
            End If

            writer.RenderEndTag()

            'content
            writer.AddAttribute("class", "icrop-PopOverForm-content")
            writer.AddStyleAttribute("width", Me.Width.ToString())
            writer.AddStyleAttribute("height", Me.Height.ToString())
            writer.AddStyleAttribute("overflow", "hidden")
            writer.RenderBeginTag("div")

            writer.AddAttribute("class", "icrop-PopOverForm-sheet")
            writer.AddStyleAttribute("width", String.Format(CultureInfo.InvariantCulture, "{0}px", (Me.Width.Value * 7)))
            writer.RenderBeginTag("div")

            For i As Integer = 0 To Me.PageCapacity
                writer.AddAttribute("class", "icrop-PopOverForm-page")
                writer.AddStyleAttribute("width", Me.Width.ToString())
                writer.AddStyleAttribute("height", Me.Height.ToString())
                writer.AddStyleAttribute("float", "left")
                writer.RenderBeginTag("div")
                writer.RenderEndTag()
            Next

            writer.RenderEndTag()
            writer.RenderEndTag()
        End Sub

        Public Function GetCallbackResult() As String Implements System.Web.UI.ICallbackEventHandler.GetCallbackResult
             If _callbackResult IsNot Nothing Then
                'JSON形式に変換した文字列を戻り値として返却
                Dim result As String = Common.SerializeToJSON(_callbackResult)
                Return result
            Else
                'コールバック処理にてエラーが発生している場合
                Return "{errorMessege: 'Internal Server Error'}"
            End If
        End Function

        Public Sub RaiseCallbackEvent(eventArgument As String) Implements System.Web.UI.ICallbackEventHandler.RaiseCallbackEvent
            Dim serializer As New JavaScriptSerializer
            Dim args As Dictionary(Of String, Object)

            'JSON形式の文字列を変換
            args = serializer.Deserialize(Of Dictionary(Of String, Object))(eventArgument)
            If args Is Nothing Then
                Throw New ArgumentException("parameter error!", "eventArgument")
            End If

            _callbackResult = New Dictionary(Of String, Object)
            RaiseEvent ClientCallback(Me, New ClientCallbackEventArgs(args, _callbackResult))
        End Sub


        Public Function LoadPostData(postDataKey As String, postCollection As System.Collections.Specialized.NameValueCollection) As Boolean Implements System.Web.UI.IPostBackDataHandler.LoadPostData
            Dim value As String = postCollection(Me.UniqueID)
            If (Not Me.Value.Equals(value)) Then
                Me.Value = value
                Return True
            End If
            Return False
        End Function

        Public Sub RaisePostDataChangedEvent() Implements System.Web.UI.IPostBackDataHandler.RaisePostDataChangedEvent
            RaiseEvent ValueChanged(Me, EventArgs.Empty)
        End Sub

        Private _callbackResult As Dictionary(Of String, Object)

    End Class
End Namespace
