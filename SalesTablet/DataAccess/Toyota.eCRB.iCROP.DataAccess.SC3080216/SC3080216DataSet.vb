Partial Class SC3080216DataSet

    Partial Class SC3080216UpdAfterOdrDocumentDataTable

        Private Sub SC3080216UpdAfterOdrDocumentDataTable_SC3080216UpdAfterOdrDocumentRowChanging(sender As System.Object, e As SC3080216UpdAfterOdrDocumentRowChangeEvent) Handles Me.SC3080216UpdAfterOdrDocumentRowChanging

        End Sub

    End Class

    Partial Class SC3080216AfterOdrActCalDavDataTable

        Private Sub SC3080216AfterOdrActCalDavDataTable_SC3080216AfterOdrActCalDavRowChanging(sender As System.Object, e As SC3080216AfterOdrActCalDAVRowChangeEvent) Handles Me.SC3080216AfterOdrActCalDAVRowChanging

        End Sub

    End Class

    Partial Class SC3080216PlanDataTable

        Private Sub SC3080216PlanDataTable_ColumnChanging(ByVal sender As System.Object, ByVal e As System.Data.DataColumnChangeEventArgs) Handles Me.ColumnChanging
            If (e.Column.ColumnName = Me.ACTUALACCOUNTColumn.ColumnName) Then
                'ユーザー コードをここに追加してください
            End If

        End Sub

    End Class

    Partial Class SC3080216FllwSalesDataTable

        Private Sub SC3080216FllwSalesDataTable_SC3080216FllwSalesRowChanging(ByVal sender As System.Object, ByVal e As SC3080216FllwSalesRowChangeEvent) Handles Me.SC3080216FllwSalesRowChanging

        End Sub

    End Class

End Class
