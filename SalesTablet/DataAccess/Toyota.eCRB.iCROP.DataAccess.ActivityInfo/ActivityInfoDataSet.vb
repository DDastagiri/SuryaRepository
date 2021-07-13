Partial Class ActivityInfoDataSet

    Partial Class ActivityInfoActHisFllwDataTable

        Private Sub ActivityInfoActHisFllwDataTable_ColumnChanging(ByVal sender As System.Object, ByVal e As System.Data.DataColumnChangeEventArgs) Handles Me.ColumnChanging
            If (e.Column.ColumnName = Me.INSDIDColumn.ColumnName) Then
                'ユーザー コードをここに追加してください
            End If

        End Sub

    End Class

    Partial Class ActivityInfoFllwModelDataTable

    End Class

End Class
