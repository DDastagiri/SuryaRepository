'-------------------------------------------------------------------
' UnallocatedActivityTableAdapter.vb
'-------------------------------------------------------------------
' 機能：活動担当未割り当て活動件数取得API
' 補足：
' 作成：2014/05/28 TCS 水野 セールスタブレットMGR機能
'-------------------------------------------------------------------

Imports System.Text
Imports Oracle.DataAccess.Client
Imports Toyota.eCRB.SystemFrameworks.Core
Imports System.Globalization
Imports System.Reflection
Imports System.Reflection.MethodBase

Public NotInheritable Class UnallocatedActivityTableAdapter

#Region "メソッド"

    ''' <summary>
    ''' デフォルトコンストラクタ
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub New()
        'デフォルトコンストラクタ
    End Sub

    ''' <summary>
    ''' 活動担当未割り当て件数取得
    ''' </summary>
    ''' <param name="dlrcd">販売店コード</param>
    ''' <param name="brncd">店舗コード</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function GetUnallocatedActivityCount(ByVal dlrcd As String, ByVal brncd As String) As Integer
        ' ======================== ログ出力 開始 ========================
        Logger.Info(String.Format(CultureInfo.InvariantCulture, " {0}_Start", MethodBase.GetCurrentMethod.Name))
        ' ======================== ログ出力 終了 ========================

        Dim sql As New StringBuilder

        With sql
            .AppendLine("SELECT /* UnallocatedActivityClass_001 */ ")
            .AppendLine("   COUNT(1) AS CNT ")
            .AppendLine("FROM ( ")
            .AppendLine("	SELECT T1.CST_ID ")
            .AppendLine("		, T1.SCHE_DLR_CD ")
            .AppendLine("		, T1.SCHE_BRN_CD ")
            .AppendLine("		, T1.SCHE_ORGNZ_ID ")
            .AppendLine("	FROM ")
            .AppendLine("  (SELECT/*+ INDEX(ST2 TB_T_ACTIVITY_IX2)*/ ")
            .AppendLine("    ST1.CST_ID ")
            .AppendLine("   ,ST1.VCL_ID ")
            .AppendLine("   ,ST1.REC_CST_VCL_TYPE as CST_VCL_TYPE ")
            .AppendLine("   ,ST2.SCHE_DLR_CD ")
            .AppendLine("   ,ST2.SCHE_BRN_CD ")
            .AppendLine("   ,ST2.SCHE_ORGNZ_ID ")
            .AppendLine("  FROM ")
            .AppendLine("   TB_T_ACTIVITY ST2 ")
            .AppendLine("   JOIN TB_T_REQUEST  ST1 ")
            .AppendLine("    ON ST1.REQ_ID = ST2.REQ_ID ")
            .AppendLine("   AND ST2.ATT_ID = 0 ")
            .AppendLine("   AND ST2.SCHE_STF_CD IN (' ') ")
            .AppendLine("   AND ST2.SCHE_DLR_CD = :DLR_CD ")
            .AppendLine("   AND ST2.SCHE_BRN_CD = :BRN_CD ")
            .AppendLine("   AND ST2.RSLT_FLG = '0' ")
            .AppendLine("   JOIN TB_M_BUSSINES_CATEGORY ST3 ")
            .AppendLine("    ON ST3.BIZ_CAT_ID = ST1.BIZ_CAT_ID ")
            .AppendLine("   JOIN TB_M_WORD_RELATION T2 ")
            .AppendLine("    ON T2.TYPE_CD = 'BIZ_TYPE' ")
            .AppendLine("    AND T2.TYPE_VAL = ST3.BIZ_TYPE ")
            .AppendLine("   JOIN TB_M_WORD T1 ")
            .AppendLine("    ON T1.WORD_CD = T2.WORD_CD ")
            .AppendLine("  UNION ALL ")
            .AppendLine("  SELECT ")
            .AppendLine("    ST3.CST_ID ")
            .AppendLine("   ,ST3.VCL_ID ")
            .AppendLine("   ,ST3.CST_VCL_TYPE ")
            .AppendLine("   ,ST5.SCHE_DLR_CD ")
            .AppendLine("   ,ST5.SCHE_BRN_CD ")
            .AppendLine("   ,CAST(0 AS NUMBER(20,0)) AS SCHE_ORGNZ_ID ")
            .AppendLine("  FROM ")
            .AppendLine("   TB_T_ATTRACT_CALL  ST5 ")
            .AppendLine("   JOIN TB_T_ATTRACT  ST3 ")
            .AppendLine("    ON ST3.ATT_ID = ST5.ATT_ID ")
            .AppendLine("   AND ST3.FIRST_ACT_SCHE_DATE <= TRUNC(SYSDATE) ")
            .AppendLine("   JOIN TB_M_ATTPLAN  ST4 ")
            .AppendLine("    ON ST4.DLR_CD = ST3.ATTPLAN_CREATE_DLR_CD ")
            .AppendLine("    AND ST4.BRN_CD = ST3.ATTPLAN_CREATE_BRN_CD ")
            .AppendLine("    AND ST4.ATTPLAN_ID = ST3.ATTPLAN_ID ")
            .AppendLine("    AND ST4.ATTPLAN_VERSION = ST3.ATTPLAN_VERSION ")
            .AppendLine("   JOIN TB_M_BUSSINES_CATEGORY  ST9 ")
            .AppendLine("    ON ST9.BIZ_CAT_ID = ST4.BIZ_CAT_ID ")
            .AppendLine("  WHERE ")
            .AppendLine("       ST5.SCHE_STF_CD IN (' ') ")
            .AppendLine("   AND ST5.SCHE_DLR_CD = :DLR_CD ")
            .AppendLine("   AND ST5.SCHE_BRN_CD = :BRN_CD ")
            .AppendLine("   AND ST5.RSLT_FLG = '0' ")
            .AppendLine("   AND ST5.CALL_COUNT = '1' ")
            .AppendLine("   AND ST5.ATT_CC_FLG = '0' ")
            .AppendLine("   AND ST5.CONSTRAINT_STATUS = '1' ")
            .AppendLine("   AND ST4.DLR_CD IN ('XXXXX', :DLR_CD) ")
            .AppendLine("   AND ST3.DLR_CD = :DLR_CD ")
            .AppendLine("   AND ST3.BRN_CD = :BRN_CD ")
            .AppendLine("  UNION ALL ")
            .AppendLine("  SELECT /*+ INDEX(ST8 TB_T_ACTIVITY_IX2)*/ ")
            .AppendLine("    ST6.CST_ID ")
            .AppendLine("   ,ST6.VCL_ID ")
            .AppendLine("   ,ST6.CST_VCL_TYPE ")
            .AppendLine("   ,ST8.SCHE_DLR_CD ")
            .AppendLine("   ,ST8.SCHE_BRN_CD ")
            .AppendLine("   ,ST8.SCHE_ORGNZ_ID ")
            .AppendLine("  FROM ")
            .AppendLine("   TB_T_ACTIVITY  ST8 ")
            .AppendLine("   JOIN TB_T_ATTRACT  ST6 ")
            .AppendLine("    ON  ST8.ATT_ID = ST6.ATT_ID ")
            .AppendLine("    AND ST8.REQ_ID = 0 ")
            .AppendLine("    AND ST6.FIRST_ACT_SCHE_DATE <= TRUNC(SYSDATE) ")
            .AppendLine("   JOIN TB_M_ATTPLAN  ST7 ")
            .AppendLine("    ON ST7.DLR_CD = ST6.ATTPLAN_CREATE_DLR_CD ")
            .AppendLine("    AND ST7.BRN_CD = ST6.ATTPLAN_CREATE_BRN_CD ")
            .AppendLine("    AND ST7.ATTPLAN_ID = ST6.ATTPLAN_ID ")
            .AppendLine("    AND ST7.ATTPLAN_VERSION = ST6.ATTPLAN_VERSION ")
            .AppendLine("   JOIN TB_M_BUSSINES_CATEGORY  ST10 ")
            .AppendLine("    ON ST10.BIZ_CAT_ID = ST7.BIZ_CAT_ID ")
            .AppendLine("  WHERE ")
            .AppendLine("       ST8.SCHE_STF_CD IN (' ') ")
            .AppendLine("   AND ST8.SCHE_DLR_CD = :DLR_CD ")
            .AppendLine("   AND ST8.SCHE_BRN_CD = :BRN_CD ")
            .AppendLine("   AND ST8.RSLT_FLG = '0' ")
            .AppendLine("   AND ST6.DLR_CD = :DLR_CD ")
            .AppendLine("   AND ST7.DLR_CD IN ( :DLR_CD, 'XXXXX' ) ")
            .AppendLine("   AND ( ")
            .AppendLine("         EXISTS (select 0 FROM TB_T_ATTRACT_CALL ST12 ")
            .AppendLine("               WHERE  ST12.ATT_CC_FLG = '0' ")
            .AppendLine("                AND   ST12.CONSTRAINT_STATUS = '1' ")
            .AppendLine("                AND   ST6.DLR_CD = :DLR_CD ")
            .AppendLine("                AND   ST6.BRN_CD = :BRN_CD ")
            .AppendLine("                AND   ST12.ATT_ID = ST6.ATT_ID ")
            .AppendLine("                AND   ST12.ATT_ID = ST8.ATT_ID ")
            .AppendLine("                AND   ST12.CALL_COUNT = ST8.ACT_COUNT ")
            .AppendLine("                AND   ST8.SCHE_DLR_CD = :DLR_CD ")
            .AppendLine("                AND   ST8.SCHE_BRN_CD = :BRN_CD ")
            .AppendLine("                AND   ST8.RSLT_FLG =  '0' ")
            .AppendLine("                AND   ST12.SCHE_DLR_CD = :DLR_CD ")
            .AppendLine("                ) ")
            .AppendLine("         OR	 ")
            .AppendLine("         EXISTS (select 0 FROM TB_T_ACTIVITY ST13 ")
            .AppendLine("               WHERE  ST13.ATT_ID = ST6.ATT_ID ")
            .AppendLine("                AND   ST13.ACT_ID = ST8.ACT_ID ")
            .AppendLine("                AND   ST8.RSLT_FLG =  '0' ")
            .AppendLine("                AND   ST8.SCHE_DLR_CD = :DLR_CD ")
            .AppendLine("                AND   ST6.ATT_STATUS = '31' ")
            .AppendLine("                AND   ST6.CONTINUE_ACT_STATUS = '21' ")
            .AppendLine("                AND   ST6.DLR_CD = :DLR_CD ")
            .AppendLine("                ) ")
            .AppendLine("       ) ")
            .AppendLine("    ) T1 ")
            .AppendLine(" JOIN TB_M_CUSTOMER_VCL  T14 ")
            .AppendLine("  ON  T14.DLR_CD = T1.SCHE_DLR_CD   ")
            .AppendLine("  AND T14.CST_ID = T1.CST_ID ")
            .AppendLine("  AND T14.VCL_ID = T1.VCL_ID ")
            .AppendLine("  AND T14.CST_VCL_TYPE = T1.CST_VCL_TYPE ")
            .AppendLine("  AND T14.DLR_CD = :DLR_CD ")
            .AppendLine(" JOIN TB_M_CUSTOMER_DLR  T8 ")
            .AppendLine("  ON T8.CST_ID = T1.CST_ID ")
            .AppendLine("  AND T8.DLR_CD = T1.SCHE_DLR_CD ")
            .AppendLine("  AND T8.DLR_CD = :DLR_CD ")
            .AppendLine(" JOIN TB_M_CUSTOMER  T2 ")
            .AppendLine("  ON T2.CST_ID = T1.CST_ID ")
            .AppendLine(" UNION ALL ")
            .AppendLine("	SELECT  /*+ INDEX(ST15 TB_T_AFTER_ODR_IX4)*/ ")
            .AppendLine("			ST17.CST_ID, ")
            .AppendLine("			ST15.AFTER_ODR_PIC_DLR_CD, ")
            .AppendLine("			ST15.AFTER_ODR_PIC_BRN_CD, ")
            .AppendLine("			ST15.AFTER_ODR_PIC_ORGNZ_ID ")
            .AppendLine("	FROM  ")
            .AppendLine("		TB_T_AFTER_ODR ST15  ")
            .AppendLine("	JOIN TB_H_SALES ST16 ")
            .AppendLine("		ON ST15.SALES_ID = ST16.SALES_ID ")
            .AppendLine("	JOIN TB_M_CUSTOMER ST17 ")
            .AppendLine("		ON ST16.CST_ID = ST17.CST_ID ")
            .AppendLine("	WHERE   ST15.AFTER_ODR_PIC_DLR_CD = :DLR_CD ")
            .AppendLine("		AND ST15.AFTER_ODR_PIC_BRN_CD = :BRN_CD ")
            .AppendLine("		AND ST15.AFTER_ODR_PIC_STF_CD = ' ' ")
            .AppendLine(") TT1 ")
        End With

        Using query As New DBSelectQuery(Of DataTable)("UnallocatedActivityClass_001")

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
