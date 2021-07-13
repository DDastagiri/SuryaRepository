Partial Class SC3080202DataSet
    Partial Class SC3080202BookedAfterProcessDataTable

        Private Sub SC3080202BookedAfterProcessDataTable_SC3080202BookedAfterProcessRowChanging(sender As System.Object, e As SC3080202BookedAfterProcessRowChangeEvent) Handles Me.SC3080202BookedAfterProcessRowChanging

        End Sub

    End Class

    Partial Class SC3080202GetSalesDataTable

        Private Sub SC3080202GetSalesDataTable_ColumnChanging(sender As System.Object, e As System.Data.DataColumnChangeEventArgs) Handles Me.ColumnChanging
            If (e.Column.ColumnName = Me.ACTIVE_OR_HIS_FLGColumn.ColumnName) Then
                'ユーザー コードをここに追加してください
            End If

        End Sub

    End Class

    Partial Class SC3080202GetFollowupboxDetailDataTable

        Private Sub SC3080202GetFollowupboxDetailDataTable_SC3080202GetFollowupboxDetailRowChanging(ByVal sender As System.Object, ByVal e As SC3080202GetFollowupboxDetailRowChangeEvent) Handles Me.SC3080202GetFollowupboxDetailRowChanging

        End Sub

    End Class

    Partial Class SC3080202UpdateSelectedSeriesFromDataTable

    End Class

    Partial Class SC3080202GetCompeModelMasterToDataTable

        Private Sub SC3080202GetCompeModelMasterToDataTable_ColumnChanging(ByVal sender As System.Object, ByVal e As System.Data.DataColumnChangeEventArgs) Handles Me.ColumnChanging
            If (e.Column.ColumnName = Me.COMPETITORCDColumn.ColumnName) Then
                'ユーザー コードをここに追加してください
            End If

        End Sub

    End Class

    Partial Class SC3080202GetCompeMakerMasterDataTable

        Private Sub SC3080202GetCompeMakerMasterDataTable_ColumnChanging(ByVal sender As System.Object, ByVal e As System.Data.DataColumnChangeEventArgs) Handles Me.ColumnChanging
            If (e.Column.ColumnName = Me.COMPETITIONMAKERColumn.ColumnName) Then
                'ユーザー コードをここに追加してください
            End If

        End Sub

    End Class

    Partial Class SC3080202GetStatusToDataTable

        Private Sub SC3080202GetStatusToDataTable_ColumnChanging(ByVal sender As System.Object, ByVal e As System.Data.DataColumnChangeEventArgs) Handles Me.ColumnChanging
            If (e.Column.ColumnName = Me.CRACTRESULTColumn.ColumnName) Then
                'ユーザー コードをここに追加してください
            End If

        End Sub

    End Class

End Class
