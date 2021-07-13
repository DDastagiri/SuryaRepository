Imports System.Text
Imports Oracle.DataAccess.Client
Imports Toyota.eCRB.SystemFrameworks.Core

Namespace Toyota.eCRB.SystemFrameworks.Batch.DataAccess

    Public NotInheritable Class ProgramSettingTableAdapter

        Private Sub New()

        End Sub

        Public Shared Function GetProgramSettingTableByProgramId(ByVal programid As String) As ProgramSettingDataSet.ProgramSettingTableDataTable

            Using query As New DBSelectQuery(Of ProgramSettingDataSet.ProgramSettingTableDataTable)("PROGRAMSETTING_001")

                Dim sql As New StringBuilder

                With sql
                    .Append(" SELECT /* PROGRAMSETTING_001 */ ")
                    .Append("        PROGRAMID ")
                    .Append("      , SECTION ")
                    .Append("      , KEY ")
                    .Append("      , VALUE ")
                    .Append("      , NOTE ")
                    .Append("      , CREATEDATE ")
                    .Append("      , UPDATEDATE ")
                    .Append("      , CREATEACCOUNT ")
                    .Append("      , UPDATEACCOUNT ")
                    .Append("      , CREATEID ")
                    .Append("      , UPDATEID ")
                    .Append(" FROM   TBL_PROGRAMSETTING ")
                    .Append(" WHERE  PROGRAMID IN( 'XXXXXXXXX', :PROGRAMID ) ")
                    .Append(" ORDER BY  PROGRAMID ")
                End With

                query.CommandText = sql.ToString()
                query.AddParameterWithTypeValue("PROGRAMID", OracleDbType.Char, programid)

                Return query.GetData()

            End Using

        End Function
    End Class

End Namespace


