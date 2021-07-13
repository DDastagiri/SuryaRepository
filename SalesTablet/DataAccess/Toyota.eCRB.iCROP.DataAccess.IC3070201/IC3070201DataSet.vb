

Partial Public Class IC3070201DataSet
    Partial Class IC3070201KatashikiPictureDataTable

    End Class

    Partial Class IC3070201NoticeRequestDataTable

        Private Sub IC3070201NoticeRequestDataTable_ColumnChanging(ByVal sender As System.Object, ByVal e As System.Data.DataColumnChangeEventArgs) Handles Me.ColumnChanging
            If (e.Column.ColumnName = Me.STATUSColumn.ColumnName) Then
                'ユーザー コードをここに追加してください
            End If

        End Sub

    End Class

End Class
