Imports System.Text
Imports Oracle.DataAccess.Client
Imports Toyota.eCRB.SystemFrameworks.Core
Imports Toyota.eCRB.SystemFrameworks.Core.iCROP.BizLogic

Namespace Toyota.eCRB.SystemFrameworks.Core.iCROP.DataAccess

    ''' <summary>
    ''' 店舗コード、店舗名検索用アダプタ
    ''' </summary>
    ''' <remarks></remarks>
    Public NotInheritable Class BranchListTableAdapter

        Private Sub New()

        End Sub

        ''' <summary>
        ''' 店舗コード、店舗名取得
        ''' </summary>
        ''' <param name="dlrCd">販売店コード</param>
        ''' <returns>BranchTableDataTable</returns>
        ''' <remarks></remarks>
        Public Shared Function GetBranchComboList(ByVal dlrCD As String) As BranchListDataSet.BranchTableDataTable

            Using query As New DBSelectQuery(Of BranchListDataSet.BranchTableDataTable)("BRANCHLIST_001")

                Dim sql As New StringBuilder
                ' 2013/06/30 TCS 武田 2013/10対応版　既存流用 START
                With sql
                    .Append(" SELECT /* BRANCHLIST_001 */ ")
                    .Append("        BRN_CD AS STRCD ")
                    .Append("      , BRN_NAME AS STRNM_LOCAL ")
                    .Append("   FROM TB_M_BRANCH ")
                    .Append("  WHERE INUSE_FLG = N'1' ")
                    .Append("    AND DLR_CD = :DLRCD ")
                    .Append("  ORDER BY BRN_CD ")
                End With

                query.CommandText = sql.ToString()
                query.AddParameterWithTypeValue("DLRCD", OracleDbType.Char, dlrCD)
                ' 2013/06/30 TCS 武田 2013/10対応版　既存流用 END

                Return query.GetData()

            End Using

        End Function
    End Class
End Namespace

