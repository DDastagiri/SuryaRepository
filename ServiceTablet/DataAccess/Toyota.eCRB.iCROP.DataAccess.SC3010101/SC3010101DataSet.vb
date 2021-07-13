Imports System.Text
Imports Oracle.DataAccess.Client
Imports Toyota.eCRB.SystemFrameworks.Core

Namespace SC3010101DataSetTableAdapters

    Public Class SC3010101MacDataTableTableAdapter
        Inherits Global.System.ComponentModel.Component

        Private Sub New()

        End Sub

#Region "Select"
        ''' <summary>
        ''' MacAddressに対応した販売店コードの取得処理
        ''' </summary>
        ''' <param name="macaddress">マックアドレス</param>
        ''' <returns>検索結果を格納したDatatable</returns>
        ''' <remarks></remarks>
        Public Shared Function SelDlrCD(ByVal macaddress As String) As SC3010101DataSet.SC3010101MacDataTableDataTable
            ' DbSelectQueryインスタンス生成
            Using query As New DBSelectQuery(Of SC3010101DataSet.SC3010101MacDataTableDataTable)("SC3010101")

                ' SQL組み立て
                Dim sql As New StringBuilder
                With sql
                    .Append("SELECT /* SC3010101_001 */ ")
                    .Append("    A.DLRCD AS Dlrcd ")
                    .Append("FROM TBL_CLIENT_MACADDRESS A ")
                    .Append("WHERE ")
                    .Append(" A.MACADDRESS = :Macaddress ")
                End With

                'Macアドレス
                query.AddParameterWithTypeValue("Macaddress", OracleDbType.Char, macaddress)

                ' SQL実行（結果表を返却）
                query.CommandText = sql.ToString()
                Return query.GetData()

            End Using
        End Function
#End Region

    End Class
End Namespace
