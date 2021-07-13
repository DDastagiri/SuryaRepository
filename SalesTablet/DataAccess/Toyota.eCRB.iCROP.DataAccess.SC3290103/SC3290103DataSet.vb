'━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
'SC3290103DataSet.vb
'──────────────────────────────────
'機能： 異常詳細画面
'補足： 
'作成： 2014/06/12 TMEJ y.gotoh
'更新： 
'──────────────────────────────────

Imports Oracle.DataAccess.Client
Imports Toyota.eCRB.SystemFrameworks.Core
Imports System.Text
Imports System.Globalization

Namespace SC3290103DataSetTableAdapters

    ''' <summary>
    ''' 異常詳細画面のデータアクセスクラス
    ''' </summary>
    ''' <remarks></remarks>
    Public Class SC3290103TableAdapter
        Inherits Global.System.ComponentModel.Component

        ''' <summary>
        ''' 異常詳細情報取得
        ''' </summary>
        ''' <param name="dealerCode">販売店コード</param>
        ''' <param name="branchCode">店舗コード</param>
        ''' <param name="irregClassCode">異常分類コード</param>
        ''' <param name="irregItemCode">硫黄項目コード</param>
        ''' <param name="todayDate">本日日付</param>
        ''' <returns>異常詳細情報データテーブル</returns>
        ''' <remarks></remarks>
        Public Function GetIrregularDetailList(ByVal dealerCode As String, ByVal branchCode As String, _
                                       ByVal irregClassCode As String, ByVal irregItemCode As String, _
                                       ByVal todayDate As Date) As SC3290103DataSet.IrregularDetailInfoDataTable

            Dim dt As SC3290103DataSet.IrregularDetailInfoDataTable = Nothing

            Using query As New DBSelectQuery(Of SC3290103DataSet.IrregularDetailInfoDataTable)("SC3290103_001")
                Dim sql As New StringBuilder

                'SQL文作成
                With sql
                    .Append(" SELECT /* SC3290103_001 */ ")
                    .Append("        FLLTGT.IRREG_CLASS_CD ")
                    .Append("      , FLLTGT.IRREG_ITEM_CD ")
                    .Append("      , ORG.ORGNZ_NAME ")
                    .Append("      , FLLTGT.STF_CD ")
                    .Append("      , STF.STF_NAME ")
                    .Append("      , FLLTGT.MONTH_TARGET ")
                    .Append("      , FLLTGT.PROGRESS_TARGET ")
                    .Append("      , FLLTGT.RSLT_COUNT ")
                    .Append("      , FLLTGT.ACHIEVE_RATE ")
                    .Append("      , FLL.FLLW_COMPLETE_FLG ")
                    .Append("      , CASE WHEN FLL.FLLW_COMPLETE_FLG = '1' THEN NULL ELSE FLL.FLLW_EXPR_DATE END AS FLLW_EXPR_DATE ")
                    .Append("      , CASE WHEN FLL.FLLW_COMPLETE_FLG = '1' THEN NULL ELSE FLL.FLLW_PIC_STF_CD END AS FLLW_PIC_STF_CD ")
                    .Append("   FROM ")
                    .Append("        TB_T_IRREG_FLLW_TGT FLLTGT ")
                    .Append("      , TB_T_IRREG_FLLW FLL ")
                    .Append("      , TB_M_STAFF STF ")
                    .Append("      , TB_M_ORGANIZATION ORG ")
                    .Append("  WHERE ")
                    .Append("        FLLTGT.IRREG_CLASS_CD = FLL.IRREG_CLASS_CD(+) ")
                    .Append("    AND FLLTGT.IRREG_ITEM_CD =FLL.IRREG_ITEM_CD(+) ")
                    .Append("    AND FLLTGT.STF_CD =FLL.STF_CD(+) ")
                    .Append("    AND FLLTGT.STF_CD = STF.STF_CD (+) ")
                    .Append("    AND STF.ORGNZ_ID=ORG.ORGNZ_ID(+) ")
                    .Append("    AND FLLTGT.IRREG_CLASS_CD = :IRREG_CLASS_CD ")
                    .Append("    AND FLLTGT.IRREG_ITEM_CD = :IRREG_ITEM_CD ")
                    .Append("    AND STF.DLR_CD = :DLR_CD ")
                    .Append("    AND STF.BRN_CD = :BRN_CD ")
                    .Append("    AND ( ")
                    .Append("            FLL.FLLW_COMPLETE_FLG = '0' ")
                    .Append("         OR FLL.FLLW_COMPLETE_FLG IS NULL ")
                    .Append("         OR ( ")
                    .Append("                FLL.FLLW_COMPLETE_FLG = '1' ")
                    .Append("            AND TRUNC(FLL.FLLW_EXPR_DATE) < TRUNC(:TODAY_DATE) ")
                    .Append("            ) ")
                    .Append("        ) ")
                    .Append("    ORDER BY FLLW_EXPR_DATE NULLS FIRST , STF.STF_CD ")
                End With

                query.CommandText = sql.ToString()
                'バインド変数
                query.AddParameterWithTypeValue("DLR_CD", OracleDbType.NVarchar2, dealerCode)
                query.AddParameterWithTypeValue("BRN_CD", OracleDbType.NVarchar2, branchCode)
                query.AddParameterWithTypeValue("IRREG_CLASS_CD", OracleDbType.NVarchar2, irregClassCode)
                query.AddParameterWithTypeValue("IRREG_ITEM_CD", OracleDbType.NVarchar2, irregItemCode)
                query.AddParameterWithTypeValue("TODAY_DATE", OracleDbType.Date, todayDate)

                'クエリ実行
                dt = query.GetData()
            End Using

            Return dt
        End Function

        ''' <summary>
        ''' 異常項目名表示名称取得
        ''' </summary>
        ''' <param name="irregClassCode">異常分類コード</param>
        ''' <param name="irregItemCode">硫黄項目コード</param>
        ''' <returns>異常項目名表示名称</returns>
        ''' <remarks></remarks>
        Public Function GetIrregularItemDisplayName(ByVal irregClassCode As String, _
                                                    ByVal irregItemCode As String) As String

            Dim dt As DataTable = Nothing

            Using query As New DBSelectQuery(Of DataTable)("SC3290103_002")
                Dim sql As New StringBuilder

                'SQL文作成
                With sql
                    .Append(" SELECT /* SC3290103_002 */ ")
                    .Append("        DECODE(WORD.WORD_VAL, ' ',WORD.WORD_VAL_ENG, WORD.WORD_VAL) AS IRREG_LIST_DISP_NAME ")
                    .Append("   FROM TB_M_SLS_MANAGER_IRREG_MNG SLSMNG ")
                    .Append("      , TB_M_WORD WORD ")
                    .Append("  WHERE SLSMNG.IRREG_LIST_DISP_NAME=WORD.WORD_CD(+) ")
                    .Append("    AND SLSMNG.IRREG_CLASS_CD = :IRREG_CLASS_CD ")
                    .Append("    AND SLSMNG.IRREG_ITEM_CD = :IRREG_ITEM_CD ")
                End With

                query.CommandText = sql.ToString()
                'バインド変数
                query.AddParameterWithTypeValue("IRREG_CLASS_CD", OracleDbType.NVarchar2, irregClassCode)
                query.AddParameterWithTypeValue("IRREG_ITEM_CD", OracleDbType.NVarchar2, irregItemCode)

                'クエリ実行
                dt = query.GetData()

                If dt.Rows.Count = 0 Then
                    Logger.Info("GetIrregularItemDisplayName End Ret[" & String.Empty & "]")
                    Return String.Empty
                Else
                    Logger.Info("GetIrregularItemDisplayName End Ret:[" & dt.Rows(0).Item("IRREG_LIST_DISP_NAME").ToString() & "]")
                    Return dt.Rows(0).Item("IRREG_LIST_DISP_NAME").ToString()
                End If
            End Using

        End Function
    End Class


End Namespace
