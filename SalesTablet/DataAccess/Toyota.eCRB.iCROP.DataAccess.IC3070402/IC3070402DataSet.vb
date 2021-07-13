

Partial Public Class IC3070402DataSet
    Partial Class IC3070402CstNameDataTable

        Private Sub IC3070402CstNameDataTable_ColumnChanging(sender As System.Object, e As System.Data.DataColumnChangeEventArgs) Handles Me.ColumnChanging
            If (e.Column.ColumnName = Me.CSTNAMEColumn.ColumnName) Then
                'ユーザー コードをここに追加してください
            End If

        End Sub

    End Class


End Class
