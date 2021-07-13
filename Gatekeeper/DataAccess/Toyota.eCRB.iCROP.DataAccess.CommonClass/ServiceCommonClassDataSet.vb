'-------------------------------------------------------------------------
'Partial Class ServiceCommonClassDataSet.vb
'-------------------------------------------------------------------------
'機能：サービス共通関数API
'補足：
'作成：2019/02/28 NSK 河谷 REQ-SVT-TMT-20180601-001 Gate Keeper機能の視認性操作性改善
'更新：
'─────────────────────────────────────

Imports System.Text
Imports System.Globalization
Imports Oracle.DataAccess.Client
Imports Toyota.eCRB.SystemFrameworks.Core
Imports Toyota.eCRB.SystemFrameworks.Core.iCROP.BizLogic
Imports Toyota.eCRB.SystemFrameworks.Core.iCROP.DataAccess

Namespace ServiceCommonClassDataSetTableAdapters

    ''' <summary>
    ''' サービス共通関数APIデータアクセスクラスです。
    ''' </summary>
    ''' <remarks></remarks>
    Public Class ServiceCommonClassTableAdapter
        Inherits Global.System.ComponentModel.Component

        ''' <summary>
        ''' デフォルトコンストラクタ
        ''' </summary>
        ''' <remarks></remarks>
        Public Sub New()
            '処理なし
        End Sub

#Region "メソッド"

        ''' <summary>
        ''' ServiceCommonClass_003:システム設定から設定値を取得する
        ''' </summary>
        ''' <param name="settingName">システム設定名</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function GetSystemSettingValue(ByVal settingName As String) As ServiceCommonClassDataSet.SystemSettingDataTable

            Logger.Info(String.Format(CultureInfo.CurrentCulture, _
                                      "{0}.{1} P1:{2} ", _
                                      Me.GetType.ToString, _
                                      System.Reflection.MethodBase.GetCurrentMethod.Name, _
                                      settingName))

            Dim sql As New StringBuilder
            With sql
                .AppendLine(" SELECT /* ServiceCommonClass_003 */ ")
                .AppendLine(" 		 SETTING_VAL ")
                .AppendLine("   FROM ")
                .AppendLine(" 		 TB_M_SYSTEM_SETTING ")
                .AppendLine("  WHERE ")
                .AppendLine(" 		 SETTING_NAME = :SETTING_NAME ")
            End With

            Dim dt As ServiceCommonClassDataSet.SystemSettingDataTable = Nothing

            Using query As New DBSelectQuery(Of ServiceCommonClassDataSet.SystemSettingDataTable)("ServiceCommonClass_003")
                query.CommandText = sql.ToString()
                query.AddParameterWithTypeValue("SETTING_NAME", OracleDbType.NVarchar2, settingName)

                dt = query.GetData()
            End Using

            Logger.Info(String.Format(CultureInfo.CurrentCulture, _
                                      "{0}.{1} QUERY:COUNT = {2}", _
                                      Me.GetType.ToString, _
                                      System.Reflection.MethodBase.GetCurrentMethod.Name, _
                                      dt.Count))

            Return dt

        End Function

        ''' <summary>
        ''' ServiceCommonClass_004:販売店システム設定から設定値を取得する
        ''' </summary>
        ''' <param name="dealerCode">販売店コード</param>
        ''' <param name="branchCode">店舗コード</param>
        ''' <param name="allDealerCode">全店舗を示す販売店コード</param>
        ''' <param name="allBranchCode">全店舗を示す店舗コード</param>
        ''' <param name="settingName">販売店システム設定名</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function GetDlrSystemSettingValue(ByVal dealerCode As String, _
                                                 ByVal branchCode As String, _
                                                 ByVal allDealerCode As String, _
                                                 ByVal allBranchCode As String, _
                                                 ByVal settingName As String) As ServiceCommonClassDataSet.SystemSettingDataTable

            Logger.Info(String.Format(CultureInfo.CurrentCulture, _
                                      "{0}.{1} P1:{2} P2:{3} P3:{4} P4:{5} P5:{6} ", _
                                      Me.GetType.ToString, _
                                      System.Reflection.MethodBase.GetCurrentMethod.Name, _
                                      dealerCode, _
                                      branchCode, _
                                      allDealerCode, _
                                      allBranchCode, _
                                      settingName))

            Dim sql As New StringBuilder
            With sql
                .AppendLine("   SELECT /* ServiceCommonClass_004 */ ")
                .AppendLine(" 		   SETTING_VAL ")
                .AppendLine("     FROM ")
                .AppendLine(" 		   TB_M_SYSTEM_SETTING_DLR ")
                .AppendLine("    WHERE ")
                .AppendLine(" 		   DLR_CD IN (:DLR_CD, :ALL_DLR_CD) ")
                .AppendLine(" 	   AND BRN_CD IN (:BRN_CD, :ALL_BRN_CD) ")
                .AppendLine("      AND SETTING_NAME = :SETTING_NAME ")
                .AppendLine(" ORDER BY ")
                .AppendLine("          DLR_CD ASC, BRN_CD ASC ")
            End With

            Dim dt As ServiceCommonClassDataSet.SystemSettingDataTable = Nothing

            Using query As New DBSelectQuery(Of ServiceCommonClassDataSet.SystemSettingDataTable)("ServiceCommonClass_004")
                query.CommandText = sql.ToString()
                query.AddParameterWithTypeValue("DLR_CD", OracleDbType.NVarchar2, dealerCode)
                query.AddParameterWithTypeValue("BRN_CD", OracleDbType.NVarchar2, branchCode)
                query.AddParameterWithTypeValue("ALL_DLR_CD", OracleDbType.NVarchar2, allDealerCode)
                query.AddParameterWithTypeValue("ALL_BRN_CD", OracleDbType.NVarchar2, allBranchCode)
                query.AddParameterWithTypeValue("SETTING_NAME", OracleDbType.NVarchar2, settingName)

                dt = query.GetData()
            End Using

            Logger.Info(String.Format(CultureInfo.CurrentCulture, _
                                      "{0}.{1} QUERY:COUNT = {2}", _
                                      Me.GetType.ToString, _
                                      System.Reflection.MethodBase.GetCurrentMethod.Name, _
                                      dt.Count))

            Return dt

        End Function

#End Region

    End Class
End Namespace

Partial Public Class ServiceCommonClassDataSet
End Class
