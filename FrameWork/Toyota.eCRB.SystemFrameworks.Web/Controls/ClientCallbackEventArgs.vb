Namespace Toyota.eCRB.SystemFrameworks.Web.Controls

    Public Class ClientCallbackEventArgs
        Inherits EventArgs

        Private _arguments As Dictionary(Of String, Object)
        Private _results As Dictionary(Of String, Object)
        Public Sub New(ByVal arguments As Dictionary(Of String, Object), ByVal results As Dictionary(Of String, Object))
            _arguments = arguments
            _results = results
        End Sub

        Public ReadOnly Property Arguments As Dictionary(Of String, Object)
            Get
                Return _arguments
            End Get
        End Property

        Public ReadOnly Property Results As Dictionary(Of String, Object)
            Get
                Return _results
            End Get
        End Property
    End Class
End Namespace
