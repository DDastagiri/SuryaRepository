Partial Class SC3080205DataSet
    Partial Class SC3080205OmitreasonDataTable

        Private Sub SC3080205OmitreasonDataTable_ColumnChanging(ByVal sender As System.Object, ByVal e As System.Data.DataColumnChangeEventArgs) Handles Me.ColumnChanging
            If (e.Column.ColumnName = Me.REASONIDColumn.ColumnName) Then
                'ユーザー コードをここに追加してください
            End If

        End Sub

    End Class

End Class
