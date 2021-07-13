



Partial Class SC3010203DataSet
    Partial Class SC3010203CustomerNameDataTable

        Private Sub SC3010203CustomerNameDataTable_ColumnChanging(sender As System.Object, e As System.Data.DataColumnChangeEventArgs) Handles Me.ColumnChanging
            If (e.Column.ColumnName = Me.NAMETITLEColumn.ColumnName) Then
                'ユーザー コードをここに追加してください
            End If

        End Sub

    End Class

    Partial Class SC3010203TodoColorDataTable

        Private Sub SC3010203TodoColorDataTable_ColumnChanging(sender As System.Object, e As System.Data.DataColumnChangeEventArgs) Handles Me.ColumnChanging
            If (e.Column.ColumnName = Me.SCHEDULEDVSColumn.ColumnName) Then
                'ユーザー コードをここに追加してください
            End If

        End Sub

    End Class

    Partial Class SC3010203VclInfoDataTable

        Private Sub SC3010203VclInfoDataTable_SC3010203VclInfoRowChanging(sender As System.Object, e As SC3010203VclInfoRowChangeEvent) Handles Me.SC3010203VclInfoRowChanging

        End Sub

    End Class

End Class
