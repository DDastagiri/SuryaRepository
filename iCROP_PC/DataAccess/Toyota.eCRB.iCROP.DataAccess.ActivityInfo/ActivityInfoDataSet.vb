Partial Class ActivityInfoDataSet
    Partial Class ActivityInfoRegistDataDataTable

        Private Sub ActivityInfoRegistDataDataTable_ColumnChanging(sender As System.Object, e As System.Data.DataColumnChangeEventArgs) Handles Me.ColumnChanging
            If (e.Column.ColumnName = Me.ACTDAYFROMColumn.ColumnName) Then
                'ユーザー コードをここに追加してください
            End If

        End Sub

    End Class

End Class
