'━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
'SC3720101.aspx.vb
'─────────────────────────────────────
'機能： 受注時説明フレーム
'補足： 
'作成： 2014/03/16 SKFC 下元武
'更新： 
'─────────────────────────────────────

Option Strict On
Option Explicit On

Imports System.Text
Imports Toyota.eCRB.SystemFrameworks.Core
Imports Oracle.DataAccess.Client



Partial Class SC3270101DataSet
End Class


Namespace SC3270101DataSetTableAdapters

    Public Class SC3270101DataTableAdapter
        Inherits Global.System.ComponentModel.Component

        ''' <summary>
        ''' システム設定を取得する
        ''' </summary>
        ''' <param name="settingName">設定名</param>
        ''' <returns>システム設定を格納したデータテーブル</returns>
        ''' <remarks></remarks>
        Public Function GetSystemSetting(ByVal settingName As String) As SC3270101DataSet.SC3270101SystemSettingDataTable

            Logger.Info(String.Format("GetSystemSetting IN:settingName={0}", settingName))

            Using query As New DBSelectQuery(Of SC3270101DataSet.SC3270101SystemSettingDataTable)("SC3270101_001")

                Dim sql As New StringBuilder

                With sql
                    .Append("SELECT /* SC3270101_001 */")
                    .Append("       SETTING_VAL")
                    .Append("     , SETTING_NAME")
                    .Append("  FROM TB_M_SYSTEM_SETTING")
                    .Append(" WHERE SETTING_NAME = :SETTINGNAME")
                End With

                query.CommandText = sql.ToString()

                query.AddParameterWithTypeValue("SETTINGNAME", OracleDbType.NVarchar2, settingName)

                Logger.Info("GetSystemSetting Return GetData()")

                Return query.GetData()

            End Using

        End Function

        ''' <summary>
        ''' 文言を取得する
        ''' </summary>
        ''' <param name="wordCd">文言コード</param>
        ''' <returns>文言を格納したデータテーブル</returns>
        ''' <remarks></remarks>
        Public Function GetWord(ByVal wordCd As String) As SC3270101DataSet.SC3270101WordDataTable

            Logger.Info(String.Format("GetWord IN:wordCd={0}", wordCd))

            Using query As New DBSelectQuery(Of SC3270101DataSet.SC3270101WordDataTable)("SC3270101_002")

                Dim sql As New StringBuilder

                With sql
                    .Append("SELECT /* SC3270101_002 */")
                    .Append("       WORD_CD")
                    .Append("     , WORD_VAL")
                    .Append("     , WORD_VAL_ENG")
                    .Append("  FROM TB_M_WORD")
                    .Append(" WHERE WORD_CD = :WORD_CD")
                End With

                query.CommandText = sql.ToString()

                query.AddParameterWithTypeValue("WORD_CD", OracleDbType.NVarchar2, wordCd)

                Logger.Info("GetWord Return GetData()")

                Return query.GetData()

            End Using

        End Function

        ''' <summary>
        ''' 見積情報を取得する
        ''' </summary>
        ''' <param name="salesId">商談ID</param>
        ''' <returns>見積情報を格納したデータテーブル</returns>
        ''' <remarks></remarks>
        Public Function GetEstimate(ByVal salesId As Decimal) As SC3270101DataSet.SC3270101EstimateInfoDataTable

            Logger.Info(String.Format("GetEstimate IN:salesId={0}", salesId))

            Using query As New DBSelectQuery(Of SC3270101DataSet.SC3270101EstimateInfoDataTable)("SC3270101_003")

                Dim sql As New StringBuilder

                With sql
                    .Append("SELECT /* SC3270101_003 */")
                    .Append("       ESTIMATEID")
                    .Append("     , CONTRACTFLG")
                    .Append("     , CONTRACT_APPROVAL_STATUS")
                    .Append("  FROM TBL_ESTIMATEINFO")
                    .Append(" WHERE FLLWUPBOX_SEQNO = :SALES_ID")
                    .Append("   AND DELFLG = 0")
                    .Append(" ORDER BY ESTIMATEID")
                End With

                query.CommandText = sql.ToString()

                query.AddParameterWithTypeValue("SALES_ID", OracleDbType.Int64, salesId)

                Logger.Info("GetEstimate Return GetData()")

                Return query.GetData()

            End Using

        End Function

        ''' <summary>
        ''' 商談に紐づく、用件または誘致の、最終活動を取得する
        ''' </summary>
        ''' <param name="salesId">商談ID</param>
        ''' <returns>活動を格納したデータテーブル</returns>
        ''' <remarks></remarks>
        Public Function GetLastActivity(ByVal salesId As Decimal) As SC3270101DataSet.SC3270101ActivityDataTable

            Logger.Info(String.Format("GetLastActivity IN:salesId={0}", salesId))

            Using query As New DBSelectQuery(Of SC3270101DataSet.SC3270101ActivityDataTable)("SC3270101_004")

                Dim sql As New StringBuilder

                With sql
                    .Append("SELECT /* SC3270101_00:SALES_ID */")
                    .Append("         ACT.ACT_ID")
                    .Append("       , ACT.ACT_STATUS")
                    .Append("    FROM TB_T_SALES SAL")
                    .Append("   INNER JOIN TB_T_REQUEST REQ")
                    .Append("      ON REQ.REQ_ID = SAL.REQ_ID")
                    .Append("   INNER JOIN TB_T_ACTIVITY ACT")
                    .Append("      ON ACT.ACT_ID = REQ.LAST_ACT_ID")
                    .Append("   WHERE SAL.SALES_ID = :SALES_ID")
                    .Append(" UNION ALL")
                    .Append("  SELECT")
                    .Append("         ACT.ACT_ID")
                    .Append("       , ACT.ACT_STATUS")
                    .Append("    FROM TB_T_SALES SAL")
                    .Append("   INNER JOIN TB_T_ATTRACT ATT")
                    .Append("      ON ATT.ATT_ID = SAL.ATT_ID")
                    .Append("   INNER JOIN TB_T_ACTIVITY ACT")
                    .Append("      ON ACT.ACT_ID = ATT.LAST_ACT_ID")
                    .Append("   WHERE SAL.SALES_ID = :SALES_ID")
                    .Append(" UNION ALL")
                    .Append("  SELECT")
                    .Append("         ACT.ACT_ID")
                    .Append("       , ACT.ACT_STATUS")
                    .Append("    FROM TB_H_SALES SAL")
                    .Append("   INNER JOIN TB_H_REQUEST REQ")
                    .Append("      ON REQ.REQ_ID = SAL.REQ_ID")
                    .Append("   INNER JOIN TB_H_ACTIVITY ACT")
                    .Append("      ON ACT.ACT_ID = REQ.LAST_ACT_ID")
                    .Append("   WHERE SAL.SALES_ID = :SALES_ID")
                    .Append(" UNION ALL")
                    .Append("  SELECT")
                    .Append("         ACT.ACT_ID")
                    .Append("       , ACT.ACT_STATUS")
                    .Append("    FROM TB_H_SALES SAL")
                    .Append("   INNER JOIN TB_H_ATTRACT ATT")
                    .Append("      ON ATT.ATT_ID = SAL.ATT_ID")
                    .Append("   INNER JOIN TB_H_ACTIVITY ACT")
                    .Append("      ON ACT.ACT_ID = ATT.LAST_ACT_ID")
                    .Append("   WHERE SAL.SALES_ID = :SALES_ID")
                End With

                query.CommandText = sql.ToString()

                query.AddParameterWithTypeValue("SALES_ID", OracleDbType.Int64, salesId)

                Logger.Info("GetLastActivity Return GetData()")

                Return query.GetData()

            End Using

        End Function

    End Class


End Namespace
