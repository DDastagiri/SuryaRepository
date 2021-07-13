'━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
'SC3720101BusinessLogic.vb
'─────────────────────────────────────
'機能： 受注時説明フレームビジネスロジック
'補足： 
'作成： 2014/03/16 SKFC 下元武
'更新： 
'─────────────────────────────────────

Option Strict On
Option Explicit On

Imports System.Web.Script.Serialization
Imports Toyota.eCRB.SystemFrameworks.Core
Imports Toyota.eCRB.SystemFrameworks.Web
Imports Toyota.eCRB.iCROP.DataAccess.SC3270101

Public Class SC3270101BusinessLogic
    Inherits BaseBusinessComponent

    ''' <summary>
    ''' システム設定の取得
    ''' </summary>
    ''' <param name="settingName">設定名</param>
    ''' <returns>システム設定を格納したデータテーブル</returns>
    ''' <remarks></remarks>
    Public Function GetSystemSettingData(ByVal settingName As String) As SC3270101DataSet.SC3270101SystemSettingDataTable

        Logger.Info("GetSystemSettingData Start")

        Using adapter As New SC3270101DataSetTableAdapters.SC3270101DataTableAdapter
            Dim dt As SC3270101DataSet.SC3270101SystemSettingDataTable

            dt = adapter.GetSystemSetting(settingName)

            Logger.Info("GetSystemSettingData End")
            Return dt

        End Using

    End Function

    ''' <summary>
    ''' 文言の取得
    ''' </summary>
    ''' <param name="wordCd">文言コード</param>
    ''' <returns>文言を格納したデータテーブル</returns>
    ''' <remarks></remarks>
    Public Function GetWordData(ByVal wordCd As String) As SC3270101DataSet.SC3270101WordDataTable

        Logger.Info("GetWordData Start")

        Using adapter As New SC3270101DataSetTableAdapters.SC3270101DataTableAdapter
            Dim dt As SC3270101DataSet.SC3270101WordDataTable

            dt = adapter.GetWord(wordCd)

            Logger.Info("GetWordData End")
            Return dt

        End Using

    End Function

    ''' <summary>
    ''' 見積情報の取得
    ''' </summary>
    ''' <param name="salesId">商談ID</param>
    ''' <returns>見積情報を格納したデータテーブル</returns>
    ''' <remarks></remarks>
    Public Function GetEstimateData(ByVal salesId As Decimal) As SC3270101DataSet.SC3270101EstimateInfoDataTable

        Logger.Info("GetEstimatesData Start")

        Using adapter As New SC3270101DataSetTableAdapters.SC3270101DataTableAdapter
            Dim dt As SC3270101DataSet.SC3270101EstimateInfoDataTable

            dt = adapter.GetEstimate(salesId)

            Logger.Info("GetEstimatesData")
            Return dt

        End Using

    End Function

    ''' <summary>
    ''' 商談に紐づく、用件または誘致の、最終活動を取得する
    ''' </summary>
    ''' <param name="salesId">商談ID</param>
    ''' <returns>活動を格納したデータテーブル</returns>
    ''' <remarks></remarks>
    Public Function GetLastActivity(ByVal salesId As Decimal) As SC3270101DataSet.SC3270101ActivityDataTable

        Logger.Info("GetLastActivity Start")

        Using adapter As New SC3270101DataSetTableAdapters.SC3270101DataTableAdapter
            Dim dt As SC3270101DataSet.SC3270101ActivityDataTable

            dt = adapter.GetLastActivity(salesId)

            Logger.Info("GetLastActivity")
            Return dt

        End Using

    End Function

End Class
