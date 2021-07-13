Partial Class SC3180204DataSet
    Partial Class SC3180204InspectCodeDataTable

        Private Sub SC3180204InspectCodeDataTable_ColumnChanging(ByVal sender As System.Object, ByVal e As System.Data.DataColumnChangeEventArgs) Handles Me.ColumnChanging
            If (e.Column.ColumnName = Me.JOB_INSTRUCT_IDColumn.ColumnName) Then
                'ユーザー コードをここに追加してください
            End If

        End Sub

    End Class

    Partial Class PreviosReplacementMileageDataTable

        Private Sub PreviosReplacementMileageDataTable_ColumnChanging(ByVal sender As System.Object, ByVal e As System.Data.DataColumnChangeEventArgs) Handles Me.ColumnChanging
            If (e.Column.ColumnName = Me.REG_MILEColumn.ColumnName) Then
                'ユーザー コードをここに追加してください
            End If

        End Sub

    End Class

    Partial Class SC3180204AdviceJobDataTable

    End Class

End Class

