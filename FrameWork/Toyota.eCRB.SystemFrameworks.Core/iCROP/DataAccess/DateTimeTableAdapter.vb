Imports System.Text
Imports Oracle.DataAccess.Client
Imports Toyota.eCRB.SystemFrameworks.Core
Imports System.Globalization
Imports System.Reflection

Namespace Toyota.eCRB.SystemFrameworks.Core.iCROP.DataAccess

    Public NotInheritable Class DateTimeTableAdapter

        Private Sub New()

        End Sub

#Region "定数"
        ''2013/06/30 TCS 坂井 2013/10対応版 既存流用 START ADD
        ''' <summary>
        ''' バインド用定数 VALUE(値:'000')
        ''' </summary>
        ''' <remarks></remarks>
        Private Const C_ZERO As String = "000"
        '2013/06/30 TCS 坂井 2013/10対応版 既存流用 END
#End Region

        Public Shared Function GetNow() As DateTimeDataSet.DATETIMEDataTable

            Using query As New DBSelectQuery(Of DateTimeDataSet.DATETIMEDataTable)("DATETIME_001")

                Dim sql As New StringBuilder

                With sql
                    .Append(" SELECT /* DATETIME_001 */ ")
                    .Append("  SYSDATE AS DBDATE")
                    .Append(" FROM ")
                    .Append("  DUAL ")
                End With

                query.CommandText = sql.ToString()

                Return query.GetData()

            End Using

        End Function

        Public Shared Function GetNow(ByVal dlrCD As String, ByVal strCD As String) As DateTimeDataSet.DATETIMEDataTable


            ' 2013/06/30 TCS 坂井 2013/10対応版　既存流用 START
            ' ======================== ログ出力 開始 ========================
            Logger.Info(String.Format(CultureInfo.InvariantCulture,
                                      " {0}_Start, dlrCD:[{1}], strCD:[{2}]",
                                      MethodBase.GetCurrentMethod.Name,
                                      dlrCD,
                                      strCD))
            ' ======================== ログ出力 終了 ========================
            ' 2013/06/30 TCS 坂井 2013/10対応版　既存流用 END

            Using query As New DBSelectQuery(Of DateTimeDataSet.DATETIMEDataTable)("DATETIME_002")

                Dim sql As New StringBuilder

                With sql
                    ' 2013/06/30 TCS 坂井 2013/10対応版　既存流用 START
                    .Append(" SELECT /* DATETIME_002 */ ")
                    .Append("        SYSDATE AS DBDATE ")
                    .Append("      , T1.TIME_DIFF ")
                    .Append("   FROM ")
                    .Append("        TB_M_BRANCH T1 ")
                    .Append("  WHERE T1.DLR_CD = :DLRCD ")
                    .Append("    AND T1.BRN_CD = ")
                    .Append("        ( ")
                    .Append("          SELECT ")
                    .Append("                 NVL(( ")
                    .Append("                       SELECT ")
                    .Append("                              T2.BRN_CD ")
                    .Append("                         FROM ")
                    .Append("                              TB_M_BRANCH T2 ")
                    .Append("                        WHERE ")
                    .Append("                              T2.DLR_CD = :DLRCD ")
                    .Append("                          AND T2.BRN_CD = :STRCD), :ZERO ) ")
                    .Append("                         FROM  ")
                    .Append("                              DUAL ")
                    .Append("        ) ")
                    ' 2013/06/30 TCS 坂井 2013/10対応版　既存流用 END
                End With

                query.AddParameterWithTypeValue("DLRCD", OracleDbType.Char, dlrCD)
                query.AddParameterWithTypeValue("STRCD", OracleDbType.Char, strCD)

                ' 2013/06/30 TCS 坂井 2013/10対応版　既存流用 START
                query.AddParameterWithTypeValue("ZERO", OracleDbType.Char, C_ZERO)
                ' 2013/06/30 TCS 坂井 2013/10対応版　既存流用 END

                query.CommandText = sql.ToString()

                ' 2013/06/30 TCS 坂井 2013/10対応版　既存流用 START
                Dim dt As DateTimeDataSet.DATETIMEDataTable = query.GetData()
                ' ======================== ログ出力 開始 ========================
                Logger.Info(String.Format(CultureInfo.InvariantCulture,
                                          " {0}_End, Return:[{1}]",
                                          MethodBase.GetCurrentMethod.Name, dt.Rows.Count))
                ' ======================== ログ出力 終了 ========================

                Return dt
                ' 2013/06/30 TCS 坂井 2013/10対応版　既存流用 END

            End Using

        End Function

    End Class

End Namespace


