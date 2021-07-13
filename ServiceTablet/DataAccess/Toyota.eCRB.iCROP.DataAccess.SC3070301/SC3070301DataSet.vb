Partial Class SC3070301DataSet
    Partial Class SeriesCodeDataTable

        Private Sub SeriesCodeDataTable_ColumnChanging(sender As System.Object, e As System.Data.DataColumnChangeEventArgs) Handles Me.ColumnChanging
            If (e.Column.ColumnName = Me.CAR_NAME_CD_AI21Column.ColumnName) Then
                'ユーザー コードをここに追加してください
            End If

        End Sub

    End Class

    Partial Class ConstractInfoDataTable

    End Class


End Class
