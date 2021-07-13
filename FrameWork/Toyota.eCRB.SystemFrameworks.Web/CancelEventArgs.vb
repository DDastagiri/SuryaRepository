Namespace Toyota.eCRB.SystemFrameworks.Web

    Public Class CancelEventArgs
        Inherits EventArgs

        Public Sub New()
            Me.Cancel = False
        End Sub

        Public Property Cancel As Boolean
    End Class

End Namespace

