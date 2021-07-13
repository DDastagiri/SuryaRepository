Imports System.Web.UI.WebControls
Imports System.ComponentModel
Imports System.Globalization

Namespace Toyota.eCRB.SystemFrameworks.Web.Controls
    Public Class CustomTextBox
        Inherits TextBox

        ''' <summary>
        ''' プレースホルダ文字列（文言No）
        ''' </summary>
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

        Public Property UseEllipsis As Boolean
            Get
                If ViewState("UseEllipsis") Is Nothing Then
                    Return True
                Else
                    Return CBool(ViewState("UseEllipsis"))
                End If
            End Get
            Set(value As Boolean)
                ViewState("UseEllipsis") = value
            End Set
        End Property

        Public Property OnClientClear As String
            Get
                If ViewState("OnClientClear") Is Nothing Then
                    Return ""
                Else
                    Return CStr(ViewState("OnClientClear"))
                End If
            End Get
            Set(value As String)
                ViewState("OnClientClear") = value
            End Set
        End Property


        Protected Overrides Sub AddAttributesToRender(writer As System.Web.UI.HtmlTextWriter)
            MyBase.AddAttributesToRender(writer)
            If (Not Me.DesignMode AndAlso Me.PlaceHolderWordNo <> Nothing) Then
                writer.AddAttribute("placeholder", WebWordUtility.GetWord(Me.PlaceHolderWordNo))
            End If
        End Sub

        Protected Overrides Sub OnPreRender(e As System.EventArgs)
            MyBase.OnPreRender(e)

            'jquery plugin binding
            Dim options As New Dictionary(Of String, String)
            options.Add("useEllipsis", Me.UseEllipsis.ToString().ToLower(CultureInfo.InvariantCulture))
            If (Not String.IsNullOrEmpty(Me.OnClientClear)) Then
                options.Add("@clear", Me.OnClientClear)
            End If
            Page.ClientScript.RegisterStartupScript(Me.GetType(), Me.ClientID, Common.GetJqueryPluginBinding(Me.ClientID, "CustomTextBox", options, Me.Enabled), True)
        End Sub

    End Class

End Namespace

