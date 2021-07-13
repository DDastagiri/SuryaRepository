Imports System.Text
Imports Oracle.DataAccess.Client
Imports Toyota.eCRB.SystemFrameworks.Core

Namespace Toyota.eCRB.SystemFrameworks.Core.iCROP.DataAccess

    Public NotInheritable Class DateTimeFormTableAdapter

        Private Sub New()

        End Sub

        Public Shared Function GetDateTimeForm(ByVal cntCD As String) As DateTimeFormDataSet.TBL_DATETIMEFORMDataTable

            Using query As New DBSelectQuery(Of DateTimeFormDataSet.TBL_DATETIMEFORMDataTable)("DATETIMEFORM_001")

                Dim sql As New StringBuilder

                With sql
                    .Append(" SELECT /* DATETIMEFORM_001 */ ")
                    .Append("        CONVID ")
                    .Append("      , FORMAT ")
                    .Append(" FROM   TBL_DATETIMEFORM ")
                    .Append(" WHERE  CNTCD = :CNTCD ")
                    .Append(" ORDER BY CONVID ")
                End With

                query.CommandText = sql.ToString()
                query.AddParameterWithTypeValue("CNTCD", OracleDbType.Char, cntCD)

                Return query.GetData()

            End Using

        End Function
    End Class
End Namespace

