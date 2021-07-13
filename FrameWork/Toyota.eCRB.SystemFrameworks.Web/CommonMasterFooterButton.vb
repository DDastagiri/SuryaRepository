Imports Toyota.eCRB.SystemFrameworks.Web.Controls

Namespace Toyota.eCRB.SystemFrameworks.Web

    Public Class CommonMasterFooterButton

        Public Sub New(ByVal owner As CustomHyperLink, ByVal footerButtonId As Decimal)
            _owner = owner
            _footerButtonId = footerButtonId
        End Sub

        Public ReadOnly Property EventArgs As CommonMasterFooterButtonClickEventArgs
            Get
                Return _eventArgs
            End Get
        End Property

        Public ReadOnly Property Owner As CustomHyperLink
            Get
                Return _owner
            End Get
        End Property

        Public ReadOnly Property FooterButtonId As Decimal
            Get
                Return _footerButtonId
            End Get
        End Property

        Public Property OnClientClick As String
            Get
                Return _owner.OnClientClick
            End Get
            Set(value As String)
                _owner.OnClientClick = value
            End Set
        End Property

        Public Property Selected As Boolean
            Get
                Return _owner.CssClass.Equals("mstpg-selected")
            End Get
            Set(value As Boolean)
                If (value) Then
                    _owner.CssClass = "mstpg-selected"
                Else
                    _owner.CssClass = ""
                End If
            End Set
        End Property

        Public Property Enabled As Boolean
            Get
                Return _owner.Enabled
            End Get
            Set(value As Boolean)
                _owner.Enabled = value
            End Set
        End Property

        Public Property Visible As Boolean
            Get
                Return _owner.Visible
            End Get
            Set(value As Boolean)
                _owner.Visible = value
            End Set
        End Property

        Public Sub OnClick()
            RaiseEvent Click(Me, _eventArgs)
        End Sub

        Public Event Click As EventHandler(Of CommonMasterFooterButtonClickEventArgs)

        Private _eventArgs As New CommonMasterFooterButtonClickEventArgs
        Private _owner As CustomHyperLink
        Private _footerButtonId As Decimal

    End Class

End Namespace
