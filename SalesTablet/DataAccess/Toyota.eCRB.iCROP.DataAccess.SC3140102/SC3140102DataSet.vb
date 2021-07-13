Option Explicit On
Option Strict On

Imports System.Text
Imports Oracle.DataAccess.Client
Imports Toyota.eCRB.SystemFrameworks.Core
Imports Toyota.eCRB.SystemFrameworks.Core.iCROP.BizLogic

Partial Class SC3140102DataSet
End Class

Namespace SC3140102DataSetTableAdapters
    Public Class SC3140102DataTableAdapter
        Inherits Global.System.ComponentModel.Component


#Region " IFテスト用"

        ''' <summary>
        ''' 目標・進捗率
        ''' </summary>
        ''' <param name="dealerCode"></param>
        ''' <param name="branchCode"></param>
        ''' <param name="account"></param>
        ''' <remarks></remarks>
        Public Function GetIFTarget(ByVal dealerCode As String, ByVal branchCode As String, ByVal account As String) As SC3140102DataSet.SC3140102TargetDataTable

            Dim query As New DBSelectQuery(Of SC3140102DataSet.SC3140102TargetDataTable)("SC3140102_001")
            Dim sql As New StringBuilder

            'SQL文作成
            With sql
                .Append("SELECT /* SC3140102_001 */")
                .Append("       DLRCD ")        ' 販売店コード
                .Append("     , STRCD ")        ' 店舗コード
                .Append("     , SACODE ")
                .Append("     , NVL(PRE_CREATE_MONTH, :MINDATE) AS PRE_CREATE_MONTH ")
                .Append("     , NVL(PRE_PLAN, 0) AS PRE_PLAN ")
                .Append("     , NVL(PRE_PLAN_CHECK, 0) AS PRE_PLAN_CHECK ")
                .Append("     , NVL(PRE_PLAN_MAINT, 0) AS PRE_PLAN_MAINT ")
                .Append("     , NVL(PRE_SALE_PLAN, 0) AS PRE_SALE_PLAN ")
                .Append("     , NVL(PRE_SALE_PLAN_CHECK, 0) AS PRE_SALE_PLAN_CHECK ")
                .Append("     , NVL(PRE_SALE_PLAN_MAINT, 0) AS PRE_SALE_PLAN_MAINT ")
                .Append("     , NVL(PRE_RESULT, 0) AS PRE_RESULT ")
                .Append("     , NVL(PRE_RESULT_CHECK, 0) AS PRE_RESULT_CHECK ")
                .Append("     , NVL(PRE_RESULT_MAINT, 0) AS PRE_RESULT_MAINT ")
                .Append("     , NVL(PRE_SALE_RESULT, 0) AS PRE_SALE_RESULT ")
                .Append("     , NVL(PRE_SALE_RESULT_CHECK, 0) AS PRE_SALE_RESULT_CHECK ")
                .Append("     , NVL(PRE_SALE_RESULT_MAINT, 0) AS PRE_SALE_RESULT_MAINT ")
                .Append("     , NVL(NOW_CREATE_MONTH, :MINDATE) AS NOW_CREATE_MONTH ")
                .Append("     , NVL(NOW_PLAN, 0) AS NOW_PLAN ")
                .Append("     , NVL(NOW_PLAN_TOTAL, 0) AS NOW_PLAN_TOTAL ")
                .Append("     , NVL(NOW_PLAN_TOTAL_CHECK, 0) AS NOW_PLAN_TOTAL_CHECK ")
                .Append("     , NVL(NOW_PLAN_TOTAL_MAINT, 0) AS NOW_PLAN_TOTAL_MAINT ")
                .Append("     , NVL(NOW_SALE_PLAN, 0) AS NOW_SALE_PLAN ")
                .Append("     , NVL(NOW_SALE_PLAN_TOTAL, 0) AS NOW_SALE_PLAN_TOTAL ")
                .Append("     , NVL(NOW_SALE_PLAN_TOTAL_CHECK, 0) AS NOW_SALE_PLAN_TOTAL_CHECK ")
                .Append("     , NVL(NOW_SALE_PLAN_TOTAL_MAINT, 0) AS NOW_SALE_PLAN_TOTAL_MAINT ")
                .Append("     , NVL(NOW_RESULT_TOTAL, 0) AS NOW_RESULT_TOTAL ")
                .Append("     , NVL(NOW_RESULT_TOTAL_CHECK, 0) AS NOW_RESULT_TOTAL_CHECK ")
                .Append("     , NVL(NOW_RESULT_TOTAL_MAINT, 0) AS NOW_RESULT_TOTAL_MAINT ")
                .Append("     , NVL(NOW_SALE_RESULT_TOTAL, 0) AS NOW_SALE_RESULT_TOTAL ")
                .Append("     , NVL(NOW_SALE_RESULT_TOTAL_CHECK, 0) AS NOW_SALE_RESULT_TOTAL_CHECK ")
                .Append("     , NVL(NOW_SALE_RESULT_TOTAL_MAINT, 0) AS NOW_SALE_RESULT_TOTAL_MAINT ")
                .Append("     , NVL(TODAY_CREATE_DATE, :MINDATE) AS TODAY_CREATE_DATE ")
                .Append("     , NVL(TODAY_CARIN_PLAN, 0) AS TODAY_CARIN_PLAN ")
                .Append("     , NVL(TODAY_SALE_PLAN, 0) AS TODAY_SALE_PLAN ")
                .Append("     , NVL(TODAY_CARIN_RESULT, 0) AS TODAY_CARIN_RESULT ")
                .Append("     , NVL(TODAY_SALE_RESULT, 0) AS TODAY_SALE_RESULT ")
                .Append("  FROM TEST_SA_IF_TARGET ")
                .Append(" WHERE DLRCD  = :DLRCD ")
                .Append("   AND STRCD  = :STRCD ")
                .Append("   AND SACODE = :SACODE ")
                .Append("   AND DELFLG = '0' ")
            End With

            query.CommandText = sql.ToString()
            'バインド変数
            query.AddParameterWithTypeValue("DLRCD", OracleDbType.Char, dealerCode)
            query.AddParameterWithTypeValue("STRCD", OracleDbType.Char, branchCode)
            query.AddParameterWithTypeValue("SACODE", OracleDbType.Char, account)
            query.AddParameterWithTypeValue("MINDATE", OracleDbType.Date, DateTime.MinValue)

            '検索結果返却
            Return query.GetData()
        End Function

#End Region

    End Class

End Namespace

Partial Class SC3140102DataSet
End Class
