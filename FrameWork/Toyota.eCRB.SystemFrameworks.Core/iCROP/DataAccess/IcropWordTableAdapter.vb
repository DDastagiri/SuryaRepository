Imports System.Text
Imports Oracle.DataAccess.Client
Imports Toyota.eCRB.SystemFrameworks.Core
Imports Toyota.eCRB.SystemFrameworks.Configuration
Imports System.Globalization
Imports System.Reflection

Namespace Toyota.eCRB.SystemFrameworks.Core.iCROP.DataAccess

    Public NotInheritable Class IcropWordTableAdapter

        Private Sub New()

        End Sub

        ''' <summary>
        ''' TBL_WORD_INI,TBL_WORD_DLRテーブルから文言を取得
        ''' </summary>
        ''' <param name="displayid"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Shared Function GetDefaultWordTableByDisplayId(ByVal displayid As String) As IcropWordDataSet.IcropWordTableDataTable

            Using query As New DBSelectQuery(Of IcropWordDataSet.IcropWordTableDataTable)("ICROPWORD_001")

                Dim sql As New StringBuilder

                With sql
                    .Append(" SELECT /* ICROPWORD_001 */ ")
                    .Append("        'XXXXX' AS DLRCD ")
                    .Append("      , A.DISPLAYID ")
                    .Append("      , A.DISPLAYNO ")
                    .Append("      , A.KINDFLG ")
                    .Append("      , NVL(B.WORD, A.WORD) AS WORD ")
                    .Append(" FROM   TBL_WORD_INI A ")
                    .Append("      , TBL_WORD_DLR B ")
                    .Append(" WHERE  A.DISPLAYID = B.DISPLAYID(+) ")
                    .Append(" AND    A.DISPLAYNO = B.DISPLAYNO(+) ")
                    .Append(" AND    B.DLRCD(+) = 'XXXXX' ")
                    .Append(" AND    A.DISPLAYID = :DISPLAYID ")
                End With

                query.CommandText = sql.ToString()
                query.AddParameterWithTypeValue("DISPLAYID", OracleDbType.NVarchar2, displayid)

                Return query.GetData()

            End Using

        End Function

        ''' <summary>
        ''' TBL_WORD_INI,TBL_WORD_DLRテーブルから全販売店文言を取得
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Shared Function GetDealerWordTable() As IcropWordDataSet.IcropWordTableDataTable

            ' 2013/06/30 TCS 坂井 2013/10対応版　既存流用 START
            ' ======================== ログ出力 開始 ========================
            Logger.Info(String.Format(CultureInfo.InvariantCulture,
                                      " {0}_Start",
                                      MethodBase.GetCurrentMethod.Name))
            ' ======================== ログ出力 終了 ========================
            ' 2013/06/30 TCS 坂井 2013/10対応版　既存流用 END

            Using query As New DBSelectQuery(Of IcropWordDataSet.IcropWordTableDataTable)("ICROPWORD_002")

                Dim sql As New StringBuilder

                '2013/06/30 TCS 坂井 2013/10対応版 既存流用 START
                With sql
                    .Append(" SELECT /* ICROPWORD_002 */ ")
                    .Append("        A.DLRCD ")
                    .Append("      , A.DISPLAYID ")
                    .Append("      , A.DISPLAYNO ")
                    .Append("      , B.KINDFLG ")
                    .Append("      , A.WORD ")
                    .Append(" FROM ")
                    .Append("        TBL_WORD_DLR A ")
                    .Append("      , TBL_WORD_INI B ")
                    .Append("      , ( ")
                    .Append("          SELECT ")
                    .Append("            T1.DLR_CD AS DLRCD ")
                    .Append("          FROM ")
                    .Append("            TB_M_DEALER T1 ")
                    .Append("          WHERE ")
                    .Append("            T1.INUSE_FLG = '1' ")
                    .Append("          UNION ")
                    .Append("          SELECT ")
                    .Append("            N'00000' AS DLRCD ")
                    .Append("          FROM ")
                    .Append("            DUAL ")
                    .Append("        ) C ")
                    .Append(" WHERE ")
                    .Append("        A.DLRCD = C.DLRCD ")
                    .Append(" AND    A.DISPLAYID = B.DISPLAYID ")
                    .Append(" AND    A.DISPLAYNO = B.DISPLAYNO ")
                    .Append(" ORDER BY ")
                    .Append("        A.DISPLAYID ")
                    .Append("      , C.DLRCD ")
                End With
                '2013/06/30 TCS 坂井 2013/10対応版 既存流用 END

                query.CommandText = sql.ToString()

                ' 2013/06/30 TCS 坂井 2013/10対応版　既存流用 START
                Dim dt As IcropWordDataSet.IcropWordTableDataTable = query.GetData()

                ' ======================== ログ出力 開始 ========================
                Logger.Info(String.Format(CultureInfo.InvariantCulture,
                                          " {0}_End, Return:[{1}]",
                                          MethodBase.GetCurrentMethod.Name, dt.Rows.Count))
                ' ======================== ログ出力 終了 ========================

                Return dt
                ' 2013/06/30 TCS 坂井 2013/10対応版　既存流用 END

            End Using

        End Function
        ''' <summary>
        ''' TBL_WORD_INI,TBL_WORD_DLRテーブルから販売店文言を取得
        ''' </summary>
        ''' <param name="displayid"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Shared Function GetDealerWordTableByDisplayId(ByVal displayid As String) As IcropWordDataSet.IcropWordTableDataTable

            ' 2013/06/30 TCS 坂井 2013/10対応版　既存流用 START
            ' ======================== ログ出力 開始 ========================
            Logger.Info(String.Format(CultureInfo.InvariantCulture,
                                      " {0}_Start, displayid:[{1}]",
                                      MethodBase.GetCurrentMethod.Name,
                                      displayid))
            ' ======================== ログ出力 終了 ========================
            ' 2013/06/30 TCS 坂井 2013/10対応版　既存流用 END

            Using query As New DBSelectQuery(Of IcropWordDataSet.IcropWordTableDataTable)("ICROPWORD_003")

                Dim sql As New StringBuilder

                '2013/06/30 TCS 坂井 2013/10対応版 既存流用 START
                With sql
                    .Append(" SELECT /* ICROPWORD_003 */ ")
                    .Append("        A.DLRCD ")
                    .Append("      , A.DISPLAYID ")
                    .Append("      , A.DISPLAYNO ")
                    .Append("      , B.KINDFLG ")
                    .Append("      , A.WORD ")
                    .Append(" FROM ")
                    .Append("        TBL_WORD_DLR A ")
                    .Append("      , TBL_WORD_INI B ")
                    .Append("      , ( ")
                    .Append("        SELECT ")
                    .Append("          DLR_CD AS DLRCD ")
                    .Append("        FROM ")
                    .Append("          TB_M_DEALER ")
                    .Append("        WHERE ")
                    .Append("          INUSE_FLG = '1' ")
                    .Append("        UNION ")
                    .Append("        SELECT ")
                    .Append("          N'00000' AS DLRCD ")
                    .Append("        FROM ")
                    .Append("          DUAL ")
                    .Append("        ) C ")
                    .Append(" WHERE ")
                    .Append("        A.DLRCD = C.DLRCD ")
                    .Append(" AND    A.DISPLAYID = B.DISPLAYID ")
                    .Append(" AND    A.DISPLAYNO = B.DISPLAYNO ")
                    .Append(" AND    A.DISPLAYID = :DISPLAYID ")
                    .Append(" ORDER BY ")
                    .Append("        A.DISPLAYID ")
                    .Append("      , C.DLRCD ")
                End With
                '2013/06/30 TCS 坂井 2013/10対応版 既存流用 END

                query.CommandText = sql.ToString()
                query.AddParameterWithTypeValue("DISPLAYID", OracleDbType.NVarchar2, displayid)

                ' 2013/06/30 TCS 坂井 2013/10対応版　既存流用 START
                Dim dt As IcropWordDataSet.IcropWordTableDataTable = query.GetData()

                ' ======================== ログ出力 開始 ========================
                Logger.Info(String.Format(CultureInfo.InvariantCulture,
                                          " {0}_End, Return:[{1}]",
                                          MethodBase.GetCurrentMethod.Name, dt.Rows.Count))
                ' ======================== ログ出力 終了 ========================

                Return dt

            End Using

        End Function

        ''' <summary>
        ''' TBL_WORD_INI,TBL_WORD_DLRテーブルから全文言を取得
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Shared Function GetDefaultWordTable() As IcropWordDataSet.IcropWordTableDataTable

            Using query As New DBSelectQuery(Of IcropWordDataSet.IcropWordTableDataTable)("ICROPWORD_004")

                Dim sql As New StringBuilder

                With sql
                    .Append(" SELECT /* ICROPWORD_004 */ ")
                    .Append("        A.DISPLAYID ")
                    .Append("      , A.DISPLAYNO ")
                    .Append("      , A.KINDFLG ")
                    .Append("      , 'XXXXX' AS DLRCD ")
                    .Append("      , NVL(B.WORD, A.WORD) AS WORD ")
                    .Append(" FROM   TBL_WORD_INI A ")
                    .Append("      , TBL_WORD_DLR B ")
                    .Append(" WHERE  A.DISPLAYID = B.DISPLAYID (+) ")
                    .Append(" AND    A.DISPLAYNO = B.DISPLAYNO (+) ")
                    .Append(" AND    B.DLRCD (+) = 'XXXXX' ")
                    .Append(" ORDER BY A.DISPLAYID ")
                End With

                query.CommandText = sql.ToString()

                Return query.GetData()

            End Using

        End Function


        ''' <summary>
        ''' TBL_ICROP_WORDテーブルから文言を取得
        ''' </summary>
        ''' <param name="displayid"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Shared Function GetIcropWordTable(ByVal displayid As String) As IcropWordDataSet.IcropWordTableDataTable

            Using query As New DBSelectQuery(Of IcropWordDataSet.IcropWordTableDataTable)("ICROPWORD_005")

                Dim config As ConfigurationManager = SystemConfiguration.Current.Manager
                Dim sql As New StringBuilder

                With sql
                    .Append(" SELECT /* ICROPWORD_005 */ ")
                    .Append("        'XXXXX' AS DLRCD ")
                    .Append("      , A.DISPLAYID ")
                    .Append("      , A.DISPLAYNO ")
                    .Append("      , 0 AS KINDFLG ")
                    .Append("      , B.WORD ")
                    .Append(" FROM   TBL_ICROP_WORDMAP A ")
                    .Append("      , TBL_ICROP_WORD B ")
                    .Append(" WHERE  B.LANGCD = :LANGCD ")
                    .Append(" AND    B.WORDNO = A.WORDNO ")
                    .Append(" AND    A.CNTCD = :CNTCD ")
                    .Append(" AND    A.DISPLAYID = :DISPLAYID ")
                End With

                query.CommandText = sql.ToString()
                query.AddParameterWithTypeValue("LANGCD", OracleDbType.NVarchar2, config.EnvironmentSetting.GetSetting(String.Empty).GetValue("WordLanguageCode"))
                query.AddParameterWithTypeValue("CNTCD", OracleDbType.NVarchar2, config.EnvironmentSetting.GetSetting(String.Empty).GetValue("CountryCode"))
                query.AddParameterWithTypeValue("DISPLAYID", OracleDbType.NVarchar2, displayid)

                Return query.GetData()

            End Using

        End Function
    End Class

End Namespace
