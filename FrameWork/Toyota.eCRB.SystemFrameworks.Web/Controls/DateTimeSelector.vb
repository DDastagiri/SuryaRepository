Imports System.Web.UI.WebControls
Imports System.Web.UI
Imports Toyota.eCRB.SystemFrameworks.Core

Namespace Toyota.eCRB.SystemFrameworks.Web.Controls
    Public Enum DateTimeSelectorFormat
        [Date]
        [Time]
        [DateTime]
    End Enum

    Public Class DateTimeSelector
        Inherits WebControl
        Implements IPostBackDataHandler

        Public Property Value As Nullable(Of DateTime)
            Get
                If ViewState("Value") Is Nothing Then
                    Return Nothing
                Else
                    Return CDate(ViewState("Value"))
                End If
            End Get
            Set(_value As Nullable(Of DateTime))
                If (_value.HasValue) Then
                    ViewState("Value") = _value
                Else
                    ViewState.Remove("Value")
                End If
            End Set
        End Property

        Public Property Format As DateTimeSelectorFormat
            Get
                If ViewState("Format") Is Nothing Then
                    Return DateTimeSelectorFormat.Date
                Else
                    Return CType(ViewState("Format"), DateTimeSelectorFormat)
                End If
            End Get
            Set(value As DateTimeSelectorFormat)
                ViewState("Format") = value
            End Set
        End Property

        Public Property PlaceHolderWordNo As Decimal
            Get
                If ViewState("PlaceHolderWordNo") Is Nothing Then
                    Return Nothing
                Else
                    Return CDec(ViewState("PlaceHolderWordNo"))
                End If
            End Get
            Set(value As Decimal)
                ViewState("PlaceHolderWordNo") = value
            End Set
        End Property

        Public Event ValueChanged As EventHandler

        Protected Overrides Sub OnPreRender(e As System.EventArgs)
            MyBase.OnPreRender(e)

            'jquery plugin binding
            Page.ClientScript.RegisterStartupScript(Me.GetType(), Me.ClientID, Common.GetJqueryPluginBinding(Me.ClientID, "DateTimeSelector", Nothing, Me.Enabled), True)
        End Sub

        Protected Overrides ReadOnly Property TagKey As System.Web.UI.HtmlTextWriterTag
            Get
                Return System.Web.UI.HtmlTextWriterTag.Input
            End Get
        End Property

        Protected Overrides Sub AddAttributesToRender(writer As System.Web.UI.HtmlTextWriter)
            MyBase.AddAttributesToRender(writer)

            Select Case Me.Format
                Case DateTimeSelectorFormat.DateTime
            '2013/10/04 TCS 安田 TCS 安田 次世代e-CRBタブレット iOS7.0 VersionUp対応（セールス） MODIFY START
                    writer.AddAttribute("type", "datetime-local")
                    If (Me.Value.HasValue) Then
                        If (Me.Value.Value.Kind = DateTimeKind.Utc) Then
                            writer.AddAttribute("value", Me.Value.Value.ToLocalTime.ToString("yyyy-MM-ddTHH:mm:ss", Nothing))
                        Else
                            writer.AddAttribute("value", Me.Value.Value.ToString("yyyy-MM-ddTHH:mm:ss", Nothing))
                        End If
                    End If
            '2013/10/04 TCS 安田 TCS 安田 次世代e-CRBタブレット iOS7.0 VersionUp対応（セールス） MODIFY END
                Case DateTimeSelectorFormat.Time
                    writer.AddAttribute("type", "time")
                    If (Me.Value.HasValue) Then
                        writer.AddAttribute("value", Me.Value.Value.ToString("HH:mm", Nothing))
                    End If
                Case Else
                    writer.AddAttribute("type", "date")
                    If (Me.Value.HasValue) Then
                        writer.AddAttribute("value", Me.Value.Value.ToString("yyyy-MM-dd", Nothing))
                    End If
            End Select

            If (Not Me.DesignMode AndAlso Me.PlaceHolderWordNo <> Nothing) Then
                writer.AddAttribute("placeholder", WebWordUtility.GetWord(Me.PlaceHolderWordNo))
            End If

            writer.AddAttribute("name", Me.UniqueID)
        End Sub

        Public Function LoadPostData(postDataKey As String, postCollection As System.Collections.Specialized.NameValueCollection) As Boolean Implements System.Web.UI.IPostBackDataHandler.LoadPostData
            Dim value As String = postCollection(Me.UniqueID)
            Dim dateValue As Nullable(Of DateTime)

            Dim parsedValue As DateTime
            If (DateTime.TryParse(value, parsedValue)) Then
            '2013/10/04 TCS 安田 TCS 安田 次世代e-CRBタブレット iOS7.0 VersionUp対応（セールス） MODIFY START
                dateValue = parsedValue
            '2013/10/04 TCS 安田 TCS 安田 次世代e-CRBタブレット iOS7.0 VersionUp対応（セールス） MODIFY END
            End If

            If (dateValue.HasValue) Then
                If (Me.Value.HasValue = False OrElse Me.Value.Value.Equals(dateValue.Value) = False) Then
                    Me.Value = dateValue
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

        Public Sub RaisePostDataChangedEvent() Implements IPostBackDataHandler.RaisePostDataChangedEvent
            RaiseEvent ValueChanged(Me, EventArgs.Empty)
        End Sub
    End Class
End Namespace

