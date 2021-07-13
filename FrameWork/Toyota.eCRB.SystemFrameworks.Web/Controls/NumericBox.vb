Imports System.Globalization
Imports System.Web.UI.WebControls
Imports System.Web.UI
Imports System.Web.Script.Serialization

Namespace Toyota.eCRB.SystemFrameworks.Web.Controls
    Public Class NumericBox
        Inherits Label
        Implements IPostBackDataHandler

        Public Event ValueChanged As EventHandler

        Public Property Value As Nullable(Of Decimal)
            Get
                If ViewState("Value") Is Nothing Then
                    Return Nothing
                Else
                    Return CDec(ViewState("Value"))
                End If
            End Get
            Set(_value As Nullable(Of Decimal))
                If (_value.HasValue) Then
                    ViewState("Value") = _value
                Else
                    ViewState.Remove("Value")
                End If
            End Set
        End Property

        Public Property MaxDigits As Integer
            Get
                If ViewState("MaxDigits") Is Nothing Then
                    Return 12
                Else
                    Return CInt(ViewState("MaxDigits"))
                End If
            End Get
            Set(value As Integer)
                ViewState("MaxDigits") = value
            End Set
        End Property

        Public Property AcceptDecimalPoint As Boolean
            Get
                If ViewState("AcceptDecimalPoint") Is Nothing Then
                    Return False
                Else
                    Return CBool(ViewState("AcceptDecimalPoint"))
                End If
            End Get
            Set(value As Boolean)
                ViewState("AcceptDecimalPoint") = value
            End Set
        End Property

        Public Property AutoPostBack As Boolean
            Get
                If ViewState("AutoPostBack") Is Nothing Then
                    Return False
                Else
                    Return CBool(ViewState("AutoPostBack"))
                End If
            End Get
            Set(value As Boolean)
                ViewState("AutoPostBack") = value
            End Set
        End Property


        Public Property CompletionLabel As String
            Get
                If ViewState("CompletionLabel") Is Nothing Then
                    Return "OK"
                Else
                    Return CStr(ViewState("CompletionLabel"))
                End If
            End Get
            Set(value As String)
                ViewState("CompletionLabel") = value
            End Set
        End Property

        Public Property CompletionLabelWordNo As Decimal
            Get
                If ViewState("CompletionLabelWordNo") Is Nothing Then
                    Return Nothing
                Else
                    Return CDec(ViewState("CompletionLabelWordNo"))
                End If
            End Get
            Set(value As Decimal)
                ViewState("CompletionLabelWordNo") = value
            End Set
        End Property

        Public Property CancelLabel As String
            Get
                If ViewState("CancelLabel") Is Nothing Then
                    Return "Cancel"
                Else
                    Return CStr(ViewState("CancelLabel"))
                End If
            End Get
            Set(value As String)
                ViewState("CancelLabel") = value
            End Set
        End Property

        Public Property CancelLabelWordNo As Decimal
            Get
                If ViewState("CancelLabelWordNo") Is Nothing Then
                    Return Nothing
                Else
                    Return CDec(ViewState("CancelLabelWordNo"))
                End If
            End Get
            Set(value As Decimal)
                ViewState("CancelLabelWordNo") = value
            End Set
        End Property


        Protected Overrides Sub OnPreRender(e As System.EventArgs)
            MyBase.OnPreRender(e)

            If (Not Me.DesignMode AndAlso Me.CompletionLabelWordNo <> Nothing) Then
                Me.CompletionLabel = WebWordUtility.GetWord(Me.CompletionLabelWordNo)
            End If
            If (Not Me.DesignMode AndAlso Me.CancelLabelWordNo <> Nothing) Then
                Me.CancelLabel = WebWordUtility.GetWord(Me.CancelLabelWordNo)
            End If

            'jquery plugin binding
            Dim options As New Dictionary(Of String, String)
            options.Add("completionLabel", Me.CompletionLabel)
            options.Add("cancelLabel", Me.CancelLabel)
            options.Add("@maxDigits", Me.MaxDigits.ToString(CultureInfo.InvariantCulture))
            options.Add("@acceptDecimalPoint", Me.AcceptDecimalPoint.ToString().ToLower(CultureInfo.InvariantCulture))

            If (Me.Value.HasValue) Then
                Me.Text = Me.Value.Value.ToString(CultureInfo.InvariantCulture)
            Else
                Me.Text = ""
            End If
            options.Add("defaultValue", Me.Text)

            If (Not Me.Enabled) Then
                options.Add("@open", "function() { return false; }")
            End If

            If (Me.AutoPostBack) Then
                options.Add("@valueChanged", String.Format(CultureInfo.InvariantCulture, "function(num) {{ $(""input[name='{0}']"").val(num); $(""#{1}"").text(num); {2}; }}", Me.UniqueID, Me.ClientID, Page.ClientScript.GetPostBackEventReference(Me, "")))
            Else
                options.Add("@valueChanged", String.Format(CultureInfo.InvariantCulture, "function(num) {{ $(""input[name='{0}']"").val(num); $(""#{1}"").text(num); }}", Me.UniqueID, Me.ClientID))
            End If

            Page.ClientScript.RegisterStartupScript(Me.GetType(), Me.ClientID, Common.GetJqueryPluginBinding(Me.ClientID, "NumericKeypad", options, True), True)
        End Sub

        Protected Overrides Sub AddAttributesToRender(writer As System.Web.UI.HtmlTextWriter)
            MyBase.AddAttributesToRender(writer)
            writer.AddStyleAttribute("display", "inline-block")
            If (Me.Width <> Unit.Empty) Then
                writer.AddStyleAttribute("width", Me.Width.ToString())
            End If
            If (Me.Height <> Unit.Empty) Then
                writer.AddStyleAttribute("height", Me.Height.ToString())
            End If
        End Sub

        Public Overrides Sub RenderEndTag(writer As System.Web.UI.HtmlTextWriter)
            MyBase.RenderEndTag(writer)

            'hiddenタグを追加
            If (Me.Value.HasValue) Then
                Common.RenderHiddenField(writer, Me.UniqueID, Me.Value.Value.ToString(CultureInfo.InvariantCulture))
            Else
                Common.RenderHiddenField(writer, Me.UniqueID, "")
            End If

        End Sub

        Public Function LoadPostData(postDataKey As String, postCollection As System.Collections.Specialized.NameValueCollection) As Boolean Implements System.Web.UI.IPostBackDataHandler.LoadPostData
            Dim value As String = postCollection(Me.UniqueID)
            Dim typedValue As Decimal
            If (Decimal.TryParse(value, typedValue)) Then
                If (Me.Value.HasValue) Then
                    If (typedValue <> Me.Value.Value) Then
                        Me.Value = typedValue
                        Return True
                    End If
                Else
                    Me.Value = typedValue
                    Return True
                End If
            Else
                If (Me.Value.HasValue) Then
                    Me.Value = Nothing
                    Return True
                End If
            End If
            Return False
        End Function

        Public Sub RaisePostDataChangedEvent() Implements System.Web.UI.IPostBackDataHandler.RaisePostDataChangedEvent
            RaiseEvent ValueChanged(Me, EventArgs.Empty)
        End Sub
    End Class

End Namespace
