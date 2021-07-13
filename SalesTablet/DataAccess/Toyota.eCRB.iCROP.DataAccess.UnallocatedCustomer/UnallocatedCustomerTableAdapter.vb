'━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
'UnallocatedCustomerTableAdapter.vb
'─────────────────────────────────────
'機能： 顧客担当未割り当て件数取得API
'補足： 
'作成： 2014/05/30 TCS藤井 セールスタブレットMGR機能 
'─────────────────────────────────────

Imports System.Text
Imports Oracle.DataAccess.Client
Imports Toyota.eCRB.SystemFrameworks.Core
Imports System.Globalization
Imports System.Reflection
Imports System.Reflection.MethodBase

Public NotInheritable Class UnallocatedCustomerTableAdapter

#Region "メソッド"

    ''' <summary>
    ''' デフォルトコンストラクタ
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub New()
        'デフォルトコンストラクタ
    End Sub


    ''' <summary>
    ''' 顧客担当未割り当て件数取得
    ''' </summary>
    ''' <param name="dlrcd">販売店コード</param>
    ''' <param name="brncd">店舗コード</param>
    ''' <returns>顧客担当が割り当てられていない顧客の件数</returns>
    ''' <remarks></remarks>
    Public Shared Function GetStaffAssignToCustCount(ByVal dlrcd As String, ByVal brncd As String) As Integer
        ' ======================== ログ出力 開始 ========================
        Logger.Info(String.Format(CultureInfo.InvariantCulture, " {0}_Start", MethodBase.GetCurrentMethod.Name))
        ' ======================== ログ出力 終了 ========================

        Dim sql As New StringBuilder
        With sql
            .AppendLine("SELECT /* UnallocatedCustomerClass_001 */ ")
            .AppendLine("    COUNT(1) AS CNT ")
            .AppendLine("FROM (SELECT /*+ INDEX(T2 TB_M_CUSTOMER_VCL_PK) ")
            .AppendLine("                 INDEX(T4 TB_M_CUSTOMER_DLR_PK)*/ ")
            .AppendLine("             1 ")
            .AppendLine("      FROM TB_M_CUSTOMER_VCL T2 ")
            .AppendLine("           JOIN TB_M_CUSTOMER_DLR T4 ")
            .AppendLine("             ON T2.DLR_CD = T4.DLR_CD ")
            .AppendLine("            AND T2.CST_ID = T4.CST_ID ")
            .AppendLine("           JOIN TB_M_CUSTOMER T5 ")
            .AppendLine("             ON T2.CST_ID = T5.CST_ID ")
            .AppendLine("      WHERE T4.CST_TYPE = '1' ")
            .AppendLine("        AND T2.DLR_CD = :DLR_CD ")
            .AppendLine("        AND T2.SLS_PIC_BRN_CD = :BRN_CD ")
            .AppendLine("        AND T2.SLS_PIC_STF_CD = ' ' ")
            .AppendLine("        AND T2.CST_VCL_TYPE = '1' ")
            .AppendLine("      UNION ALL ")
            .AppendLine("      SELECT 1 ")
            .AppendLine("      FROM ")
            .AppendLine("           (SELECT /*+ INDEX(T2 TB_M_CUSTOMER_VCL_PK) ")
            .AppendLine("                       INDEX(T4 TB_M_CUSTOMER_DLR_PK)*/ ")
            .AppendLine("                  ROW_NUMBER() OVER(PARTITION BY T2.DLR_CD,T2.CST_ID ORDER BY T2.VCL_ID DESC) AS SEQNO ")
            .AppendLine("            FROM TB_M_CUSTOMER_VCL T2 ")
            .AppendLine("                 JOIN TB_M_CUSTOMER_DLR T4 ")
            .AppendLine("                   ON T2.DLR_CD = T4.DLR_CD ")
            .AppendLine("                  AND T2.CST_ID = T4.CST_ID ")
            .AppendLine("                 JOIN TB_M_CUSTOMER T5 ")
            .AppendLine("                   ON T2.CST_ID = T5.CST_ID ")
            .AppendLine("            WHERE T4.CST_TYPE = '2' ")
            .AppendLine("              AND T2.DLR_CD = :DLR_CD ")
            .AppendLine("              AND T2.SLS_PIC_BRN_CD = :BRN_CD ")
            .AppendLine("              AND T2.SLS_PIC_STF_CD = ' ' ")
            .AppendLine("              AND T2.CST_VCL_TYPE = '1') ")
            .AppendLine("      WHERE SEQNO = 1) ")
        End With

        Using query As New DBSelectQuery(Of DataTable)("UnallocatedCustomerClass_001")

            query.CommandText = sql.ToString()

            query.AddParameterWithTypeValue("DLR_CD", OracleDbType.NVarchar2, dlrcd)
            query.AddParameterWithTypeValue("BRN_CD", OracleDbType.NVarchar2, brncd)

            '検索結果返却
            Dim dt As DataTable = query.GetData()

            ' ======================== ログ出力 開始 ========================
            Logger.Info(String.Format(CultureInfo.InvariantCulture, " {0}_End, Return:[{1}]", MethodBase.GetCurrentMethod.Name, dt.Rows(0).Item("CNT")))
            ' ======================== ログ出力 終了 ========================
            Return CType(dt.Rows(0).Item("CNT"), Integer)

        End Using

    End Function

#End Region

End Class
