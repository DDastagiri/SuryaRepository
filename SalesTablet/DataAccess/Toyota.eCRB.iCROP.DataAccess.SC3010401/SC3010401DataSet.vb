Partial Class SC3010401DataSet

    Partial Class SC3010401GetBookingNoDataTable

        Private Sub SC3010401GetBookingNoDataTable_ColumnChanging(sender As System.Object, e As System.Data.DataColumnChangeEventArgs) Handles Me.ColumnChanging
            If (e.Column.ColumnName = Me.BOOKINGNOColumn.ColumnName) Then
                'ユーザー コードをここに追加してください
            End If

        End Sub

    End Class

End Class
