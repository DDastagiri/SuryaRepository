Partial Class SC3080203DataSet
 
    Partial Class SC3080203ActHisFllwDataTable

        Private Sub SC3080203ActHisFllwDataTable_ColumnChanging(ByVal sender As System.Object, ByVal e As System.Data.DataColumnChangeEventArgs) Handles Me.ColumnChanging
            If (e.Column.ColumnName = Me.INSDIDColumn.ColumnName) Then
                'ユーザー コードをここに追加してください
            End If

        End Sub

    End Class

    Partial Class SC3080203FllwColorDataTable

    End Class

End Class
