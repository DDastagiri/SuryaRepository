Imports System.Text
Imports Oracle.DataAccess.Client
Imports Toyota.eCRB.SystemFrameworks.Core

Namespace Toyota.eCRB.SystemFrameworks.Core.iCROP.DataAccess.SystemSharedDataSetTableAdapters

    Public NotInheritable Class SystemSharedTableAdapter

        Private Sub New()

        End Sub

        ''' <summary>
        ''' 日付フォーマットの取得
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Shared Function GetDateTimeForm() As SystemSharedDataSet.DateTimeFormTableDataTable

            Using query As New DBSelectQuery(Of SystemSharedDataSet.DateTimeFormTableDataTable)("SYSTEMSHARED_005")

                Dim sql As New StringBuilder

                With sql
                    .Append(" SELECT /* SYSTEMSHARED_005 */ ")
                    .Append("        CNTCD ")
                    .Append("      , CONVID ")
                    .Append("      , FORMAT ")
                    .Append(" FROM   TBL_DATETIMEFORM ")
                    .Append(" WHERE  DELFLG = '0' ")
                End With

                query.CommandText = sql.ToString()

                Return query.GetData()

            End Using

        End Function

    End Class

End Namespace
