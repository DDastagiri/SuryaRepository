'━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
'SC3280101TableAdapter.vb
'─────────────────────────────────────
'機能：
'補足： 
'作成：2014/04/18 NCN 跡部
'─────────────────────────────────────

Imports System.Text
Imports System.Data
Imports Oracle.DataAccess.Client
Imports Toyota.eCRB.SystemFrameworks.Core

Public NotInheritable Class SC3280101TableAdapter

    ''' <summary>
    ''' デフォルトコンストラクタ
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub New()
        'デフォルトコンストラクタ
    End Sub

    ''' <summary>
    ''' SYSTEM_SETTING取得
    ''' </summary>
    ''' <param name="settingname">設定名</param>
    ''' <returns>TB_M_SYSTEM_SETTINGDataTable</returns>
    ''' <remarks></remarks>
    Public Shared Function GetSystemSetting(ByVal settingname As String) As SC3280101DataSet.TB_M_SYSTEM_SETTINGDataTable

        'ログ出力 Start ***************************************************************************
        Toyota.eCRB.SystemFrameworks.Core.Logger.Debug("SC3280101TableAdapter.GetSystemSetting")
        'ログ出力 End *****************************************************************************

        Using query As New DBSelectQuery(Of SC3280101DataSet.TB_M_SYSTEM_SETTINGDataTable)("SC3280101_001")

            Dim sql As New StringBuilder
            With sql
                .Append("SELECT /* SC3280101_001 */")
                .Append("       SETTING_VAL")
                .Append("     , SETTING_NAME")
                .Append("  FROM TB_M_SYSTEM_SETTING ")
                .Append(" WHERE (SETTING_NAME = :SETTINGNAME)")
            End With

            query.CommandText = sql.ToString()
            query.AddParameterWithTypeValue("SETTINGNAME", OracleDbType.NVarchar2, settingname)
            'ログ出力 Start ***************************************************************************
            Toyota.eCRB.SystemFrameworks.Core.Logger.Debug("SC3280101TableAdapter.GetSystemSetting_End")
            'ログ出力 End *****************************************************************************

            Return query.GetData()

        End Using


    End Function

    ''' <summary>
    ''' ESTIMATEINFO取得
    ''' </summary>
    ''' <param name="salesId">商談ID</param>
    ''' <returns>TBL_ESTIMATEINFOGDataTable</returns>
    ''' <remarks></remarks>
    Public Shared Function GetEstimateInfo(ByVal salesId As String) As SC3280101DataSet.TBL_ESTIMATEINFODataTable

        'ログ出力 Start ***************************************************************************
        Toyota.eCRB.SystemFrameworks.Core.Logger.Debug("SC3280101TableAdapter.GetSystemSetting")
        'ログ出力 End *****************************************************************************

        Using query As New DBSelectQuery(Of SC3280101DataSet.TBL_ESTIMATEINFODataTable)("SC3280101_002")

            Dim sql As New StringBuilder
            With sql
                .Append("SELECT /* SC3280101_002 */")
                .Append("       ESTIMATEID")
                .Append("     , CONTRACTFLG")
                .Append("  FROM TBL_ESTIMATEINFO ")
                .Append(" WHERE (FLLWUPBOX_SEQNO = :FLLWUPBOX_SEQNO)")
                .Append("   AND DELFLG = 0")
                .Append(" ORDER BY ESTIMATEID")
            End With

            query.CommandText = sql.ToString()
            query.AddParameterWithTypeValue("FLLWUPBOX_SEQNO", OracleDbType.NVarchar2, salesId)
            'ログ出力 Start ***************************************************************************
            Toyota.eCRB.SystemFrameworks.Core.Logger.Debug("SC3280101TableAdapter.GetSystemSetting_End")
            'ログ出力 End *****************************************************************************

            Return query.GetData()

        End Using


    End Function


End Class
