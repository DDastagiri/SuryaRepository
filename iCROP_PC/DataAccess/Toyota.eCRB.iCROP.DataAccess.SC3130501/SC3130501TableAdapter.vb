'━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
'SC3130501TableAdapter.vb
'─────────────────────────────────────
'機能： 受付待ち画面(受付データ参照)
'補足： 
'作成：            SKFC 久代 【A. STEP1】
'更新： 2013/03/27 SKFC 久代 【A. STEP1】来店受付管理オペレーション確立に向けた評価アプリ作成
'更新： 2013/07/22 SKFC 山口 【A. STEP1】TSL自主研対応機能の再構築流用
'─────────────────────────────────────

Imports System.Text
Imports Oracle.DataAccess.Client
Imports Toyota.eCRB.SystemFrameworks.Core

Namespace SC3130501DataSetTableAdapters

    Public NotInheritable Class SC3130501TableAdapter

#Region "処理"
        ''' <summary>
        ''' デフォルトコンストラクタ
        ''' </summary>
        ''' <remarks></remarks>
        Private Sub New()
            '処理なし
        End Sub

        ''' <summary>
        ''' 001.呼出中データ取得
        ''' </summary>
        ''' <param name="dlrcd">販売店コード</param>
        ''' <param name="strcd">店舗コード</param>
        ''' <returns>データセット</returns>
        ''' <remarks></remarks>
        Public Shared Function GetCalleeList(ByVal dlrcd As String, ByVal strcd As String) As SC3130501DataSet.SC3130501DisplayDataDataTable
            Using query As New DBSelectQuery(Of SC3130501DataSet.SC3130501DisplayDataDataTable)("SC3130501_001")
                Dim sql As New StringBuilder
                With sql
                    .Append("SELECT /* SC3130501_001 */ ")
                    .Append("    B.STF_NAME, ")
                    .Append("    A.CALLNO, ")
                    .Append("    A.CALLPLACE ")
                    .Append("FROM ")
                    .Append("    TBL_SERVICE_VISIT_MANAGEMENT A, ")
                    .Append("    TB_M_STAFF B ")
                    .Append("WHERE ")
                    .Append("    A.SACODE = B.STF_CD(+) ")
                    .Append("      AND ")
                    .Append("    A.DLRCD = :DLRCD ")
                    .Append("      AND ")
                    .Append("    A.STRCD = :STRCD ")
                    .Append("      AND ")
                    .Append("    TRUNC(A.VISITTIMESTAMP) = TRUNC(SYSDATE) ")
                    .Append("      AND ")
                    .Append("    A.ASSIGNSTATUS = '2' ")
                    .Append("      AND ")
                    .Append("    A.CALLSTATUS = '1' ")
                    .Append("ORDER BY A.CALLSTARTDATE ")
                End With
                query.CommandText = sql.ToString()
                query.AddParameterWithTypeValue("DLRCD", OracleDbType.Char, dlrcd)
                query.AddParameterWithTypeValue("STRCD", OracleDbType.Char, strcd)
                Return query.GetData()
            End Using
        End Function

        ''' <summary>
        ''' 002.呼出待ち人数の問合せ
        ''' </summary>
        ''' <param name="dlrcd">販売店コード</param>
        ''' <param name="strcd">店舗コード</param>
        ''' <returns>データセット</returns>
        ''' <remarks></remarks>
        Public Shared Function GetWaitNumber(ByVal dlrcd As String, ByVal strcd As String) As SC3130501DataSet.SC3130501WaitNumberDataTable
            Using query As New DBSelectQuery(Of SC3130501DataSet.SC3130501WaitNumberDataTable)("SC3130501_002")
                Dim sql As New StringBuilder
                With sql
                    .Append("SELECT /* SC3130501_002 */ ")
                    .Append("    COUNT(1) AS WAITNUMBER ")
                    .Append("FROM ")
                    .Append("    TBL_SERVICE_VISIT_MANAGEMENT ")
                    .Append("WHERE ")
                    .Append("    DLRCD = :DLRCD ")
                    .Append("      AND ")
                    .Append("    STRCD = :STRCD ")
                    .Append("      AND ")
                    .Append("    TRUNC(VISITTIMESTAMP) = TRUNC(TO_DATE(SYSDATE)) ")
                    .Append("      AND ")
                    .Append("    CALLNO IS NOT NULL ")
                    .Append("      AND ")
                    .Append("    CALLSTATUS = '0' ")
                    .Append("      AND ")
                    .Append("    ASSIGNSTATUS IN ('0', '1', '2', '9') ")
                End With
                query.CommandText = sql.ToString()
                query.AddParameterWithTypeValue("DLRCD", OracleDbType.Char, dlrcd)
                query.AddParameterWithTypeValue("STRCD", OracleDbType.Char, strcd)
                Return query.GetData()
            End Using
        End Function

        ''' <summary>
        ''' 003.日付フォーマット取得
        ''' </summary>
        ''' <param name="cntcd">国コード</param>
        ''' <returns>データセット</returns>
        ''' <remarks></remarks>
        Public Shared Function GetDateFormat(ByVal cntcd As String) As SC3130501DataSet.SC3130501DateFormatDataTable
            Using query As New DBSelectQuery(Of SC3130501DataSet.SC3130501DateFormatDataTable)("SC3130501_003")
                Dim sql As New StringBuilder
                With sql
                    .Append("SELECT /* SC3130501_003 */ ")
                    .Append("    CONVID, ")
                    .Append("    FORMAT ")
                    .Append("FROM ")
                    .Append("    TBL_DATETIMEFORM ")
                    .Append("WHERE ")
                    .Append("    CNTCD = :CNTCD ")
                    .Append("ORDER BY CONVID ")
                End With
                query.CommandText = sql.ToString()
                query.AddParameterWithTypeValue("CNTCD", OracleDbType.Char, cntcd)
                Return query.GetData()
            End Using
        End Function

#End Region

    End Class
End Namespace
