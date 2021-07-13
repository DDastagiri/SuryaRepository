Namespace Toyota.eCRB.SystemFrameworks.Web

    Public Class CommonMasterFooterButtonClickEventArgs
        Inherits EventArgs

        Public Sub New()
            Me.TCVFunction = False
        End Sub

        Public ReadOnly Property Parameters As Dictionary(Of String, Object)
            Get
                Return _parameters
            End Get
        End Property

        Public Property TCVFunction As Boolean

        Private _parameters As New Dictionary(Of String, Object)

    End Class

End Namespace