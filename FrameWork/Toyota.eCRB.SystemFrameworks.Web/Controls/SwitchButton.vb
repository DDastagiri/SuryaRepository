Imports System.Web.UI.WebControls
Imports System.Web.UI
Imports System.Globalization

Namespace Toyota.eCRB.SystemFrameworks.Web.Controls
    Public Class SwitchButton
        Inherits CheckBox

        Public Property OnText As String
            Get
                If ViewState("OnText") Is Nothing Then
                    Return ""
                Else
                    Return CStr(ViewState("OnText"))
                End If
            End Get
            Set(value As String)
                ViewState("OnText") = value
            End Set
        End Property

        Public Property OnTextWordNo As Decimal
            Get
                If ViewState("OnTextWordNo") Is Nothing Then
                    Return Nothing
                Else
                    Return CDec(ViewState("OnTextWordNo"))
                End If
            End Get
            Set(value As Decimal)
                ViewState("OnTextWordNo") = value
            End Set
        End Property

        Public Property OffText As String
            Get
                If ViewState("OffText") Is Nothing Then
                    Return ""
                Else
                    Return CStr(ViewState("OffText"))
                End If
            End Get
            Set(value As String)
                ViewState("OffText") = value
            End Set
        End Property

        Public Property OffTextWordNo As Decimal
            Get
                If ViewState("OffTextWordNo") Is Nothing Then
                    Return Nothing
                Else
                    Return CDec(ViewState("OffTextWordNo"))
                End If
            End Get
            Set(value As Decimal)
                ViewState("OffTextWordNo") = value
            End Set
        End Property

        Protected Overrides Sub OnPreRender(e As System.EventArgs)
            MyBase.OnPreRender(e)

            Me.InputAttributes.CssStyle.Add("display", "inline-block")
            Me.InputAttributes.CssStyle.Add("width", Me.Width.ToString())
            Me.InputAttributes.CssStyle.Add("height", Me.Height.ToString())

            If (Not Me.DesignMode AndAlso Me.OffTextWordNo <> Nothing) Then
                Me.OffText = WebWordUtility.GetWord(Me.OffTextWordNo)
            End If
            If (Not Me.DesignMode AndAlso Me.OnTextWordNo <> Nothing) Then
                Me.OnText = WebWordUtility.GetWord(Me.OnTextWordNo)
            End If

            'jquery plugin binding
            Dim options As New Dictionary(Of String, String)
            options.Add("onLabel", Me.OnText)
            options.Add("offLabel", Me.OffText)
            If (Me.Width <> Unit.Empty) Then
                options.Add("@switchWidth", (Me.Width.Value / 2).ToString(CultureInfo.InvariantCulture))
            End If
            If (Me.Height <> Unit.Empty) Then
                options.Add("@switchHeight", Me.Height.Value.ToString(CultureInfo.InvariantCulture))
            End If
            If (Me.AutoPostBack) Then
                options.Add("@check", String.Format(CultureInfo.InvariantCulture, "function() {{ {0} }}", Page.ClientScript.GetPostBackEventReference(Me, "")))
            End If
            Page.ClientScript.RegisterStartupScript(Me.GetType(), Me.ClientID, Common.GetJqueryPluginBinding(Me.ClientID, "SwitchButton", options, Me.Enabled), True)

        End Sub

    End Class
End Namespace

