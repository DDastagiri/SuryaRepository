'━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
'SC3280101BusinessLogic.vb
'─────────────────────────────────────
'機能： 納車時説明フレームビジネスロジック
'補足： 
'作成： 2014/04/17 NCN 跡部
'更新： 
'─────────────────────────────────────

Option Strict On
Option Explicit On

Imports System.Web.Script.Serialization
Imports Toyota.eCRB.SystemFrameworks.Core
Imports Toyota.eCRB.SystemFrameworks.Web
Imports Toyota.eCRB.iCROP.DataAccess.SC3280101

Public Class SC3280101BusinessLogic
    Inherits BaseBusinessComponent

    ''' <summary>
    ''' システム設定の取得
    ''' </summary>
    ''' <returns>システム設定データセット</returns>
    ''' <remarks></remarks>
    Public Function GetSystemSettingData(ByVal settingName As String) As SC3280101DataSet.TB_M_SYSTEM_SETTINGDataTable
        Return SC3280101TableAdapter.GetSystemSetting(settingName)
    End Function

    ''' <summary>
    ''' 見積もりIDの取得
    ''' </summary>
    ''' <returns>見積もり情報データセット</returns>
    ''' <remarks></remarks>
    Public Function GetEstimateInfoData(ByVal salesId As String) As SC3280101DataSet.TBL_ESTIMATEINFODataTable
        Return SC3280101TableAdapter.GetEstimateInfo(salesId)
    End Function


End Class
