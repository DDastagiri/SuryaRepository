

Partial Public Class SC3160218DataSet
    Partial Class TB_M_PROGRAM_SETTINGDataTable

        Private Sub TB_M_PROGRAM_SETTINGDataTable_TB_M_PROGRAM_SETTINGRowChanging(sender As System.Object, e As TB_M_PROGRAM_SETTINGRowChangeEvent) Handles Me.TB_M_PROGRAM_SETTINGRowChanging

        End Sub

    End Class

    Partial Class TB_M_RO_DAMAGE_TYPEDataTable

        Private Sub TB_M_RO_DAMAGE_TYPEDataTable_ColumnChanging(sender As System.Object, e As System.Data.DataColumnChangeEventArgs) Handles Me.ColumnChanging
            If (e.Column.ColumnName = Me.DAMAGE_TYPEColumn.ColumnName) Then
                'ユーザー コードをここに追加してください
            End If

        End Sub

    End Class

    Partial Class TB_M_DMS_CODE_MAPDataTable

        Private Sub TB_M_DMS_CODE_MAPDataTable_TB_M_DMS_CODE_MAPRowChanging(sender As System.Object, e As TB_M_DMS_CODE_MAPRowChangeEvent) Handles Me.TB_M_DMS_CODE_MAPRowChanging

        End Sub

    End Class

End Class
