Imports System.Web.UI.WebControls
Imports System.Web.UI
Imports System.Web.Script.Serialization
Imports System.Globalization

Namespace Toyota.eCRB.SystemFrameworks.Web.Controls
    Public Class CustomRepeater
        Inherits WebControl
        Implements ICallbackEventHandler

        Public Event ClientCallback As EventHandler(Of ClientCallbackEventArgs)

        Public Property MaxCacheRows As Integer
            Get
                If ViewState("MaxCacheRows") Is Nothing Then
                    Return 500
                Else
                    Return CInt(ViewState("MaxCacheRows"))
                End If
            End Get
            Set(value As Integer)
                ViewState("MaxCacheRows") = value
            End Set
        End Property

        Public Property PageRows As Integer
            Get
                If ViewState("PageRows") Is Nothing Then
                    Return 30
                Else
                    Return CInt(ViewState("PageRows"))
                End If
            End Get
            Set(value As Integer)
                ViewState("PageRows") = value
            End Set
        End Property

        Public Property CurrentPage As Integer
            Get
                If ViewState("CurrentPage") Is Nothing Then
                    Return 1
                Else
                    Return CInt(ViewState("CurrentPage"))
                End If
            End Get
            Set(value As Integer)
                ViewState("CurrentPage") = value
            End Set
        End Property

        Public Property RewindPagerLabel As String
            Get
                If ViewState("RewindPagerLabel") Is Nothing Then
                    Return "Previous"
                Else
                    Return CStr(ViewState("RewindPagerLabel"))
                End If
            End Get
            Set(value As String)
                ViewState("RewindPagerLabel") = value
            End Set
        End Property

        Public Property RewindPagerLabelWordNo As Decimal
            Get
                If ViewState("RewindPagerLabelWordNo") Is Nothing Then
                    Return Nothing
                Else
                    Return CDec(ViewState("RewindPagerLabelWordNo"))
                End If
            End Get
            Set(value As Decimal)
                ViewState("RewindPagerLabelWordNo") = value
            End Set
        End Property

        Public Property ForwardPagerLabel As String
            Get
                If ViewState("ForwardPagerLabel") Is Nothing Then
                    Return "Next"
                Else
                    Return CStr(ViewState("ForwardPagerLabel"))
                End If
            End Get
            Set(value As String)
                ViewState("ForwardPagerLabel") = value
            End Set
        End Property

        Public Property ForwardPagerLabelWordNo As Decimal
            Get
                If ViewState("ForwardPagerLabelWordNo") Is Nothing Then
                    Return Nothing
                Else
                    Return CDec(ViewState("ForwardPagerLabelWordNo"))
                End If
            End Get
            Set(value As Decimal)
                ViewState("ForwardPagerLabelWordNo") = value
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

        Public Property OnClientLoadCallbackResponse As String
            Get
                If ViewState("OnClientLoadCallbackResponse") Is Nothing Then
                    Return ""
                Else
                    Return CStr(ViewState("OnClientLoadCallbackResponse"))
                End If
            End Get
            Set(value As String)
                ViewState("OnClientLoadCallbackResponse") = value
            End Set
        End Property

        Public Property PreventMoveEvent As Boolean
            Get
                If ViewState("PreventMoveEvent") Is Nothing Then
                    Return True
                Else
                    Return CBool(ViewState("PreventMoveEvent"))
                End If
            End Get
            Set(value As Boolean)
                ViewState("PreventMoveEvent") = value
            End Set
        End Property

        Protected Overrides Sub AddAttributesToRender(writer As System.Web.UI.HtmlTextWriter)
            MyBase.AddAttributesToRender(writer)
            If (Me.CssClass <> Nothing) Then
                writer.AddAttribute("class", "icrop-CustomRepeater " & Me.CssClass)
            Else
                writer.AddAttribute("class", "icrop-CustomRepeater")
            End If
        End Sub

        Protected Overrides Sub OnPreRender(e As System.EventArgs)
            Dim callback As String = "$.CustomRepeater.getCallbackResponseFromServer"
            Dim callbackArguments As String = "$.CustomRepeater.getCallbackArguments('" & Me.ClientID & "')"

            If (Not Me.DesignMode AndAlso Me.RewindPagerLabelWordNo <> Nothing) Then
                Me.RewindPagerLabel = WebWordUtility.GetWord(Me.RewindPagerLabelWordNo)
            End If
            If (Not Me.DesignMode AndAlso Me.ForwardPagerLabelWordNo <> Nothing) Then
                Me.ForwardPagerLabel = WebWordUtility.GetWord(Me.ForwardPagerLabelWordNo)
            End If

            'jquery plugin binding
            Dim options As New Dictionary(Of String, String)
            options.Add("@pageRows", Me.PageRows.ToString(CultureInfo.InvariantCulture))
            options.Add("@defaultPage", Me.CurrentPage.ToString(CultureInfo.InvariantCulture))
            options.Add("@maxCacheRows", Me.MaxCacheRows.ToString(CultureInfo.InvariantCulture))
            options.Add("rewindPagerLabel", Me.RewindPagerLabel)
            options.Add("forwardPagerLabel", Me.ForwardPagerLabel)
            options.Add("@preventMoveEvent", Me.PreventMoveEvent.ToString().ToLower(CultureInfo.InvariantCulture))
            options.Add("@callbackToServer", String.Format(CultureInfo.InvariantCulture, "function() {{ {0}; }}", Page.ClientScript.GetCallbackEventReference(Me, callbackArguments, callback, """" & Me.ClientID & """", True)))
            options.Add("@render", Me.OnClientRender)
            If (Me.OnClientLoadCallbackResponse <> Nothing) Then
                options.Add("@loadCallbackResponse", Me.OnClientLoadCallbackResponse)
            End If
            options.Add("@load", "function(repeater, rowIndex, rewind, criteria) { repeater.callbackServer({ ""beginRowIndex"": rowIndex, ""rewind"": (rewind ? ""true"" : ""false""), ""criteria"" : criteria }, function(result) { repeater.loadCallbackResponse(result, rewind); }); return false; }")
            Page.ClientScript.RegisterStartupScript(Me.GetType(), Me.ClientID, Common.GetJqueryPluginBinding(Me.ClientID, "CustomRepeater", options, True), True)
        End Sub

        Protected Overrides ReadOnly Property TagKey As System.Web.UI.HtmlTextWriterTag
            Get
                Return System.Web.UI.HtmlTextWriterTag.Div
            End Get
        End Property

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


        Private _callbackResult As Dictionary(Of String, Object)
    End Class

End Namespace
