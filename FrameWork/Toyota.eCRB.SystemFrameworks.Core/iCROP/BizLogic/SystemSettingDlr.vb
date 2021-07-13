Imports Toyota.eCRB.SystemFrameworks.Core.iCROP.DataAccess
Imports System.Web

Namespace Toyota.eCRB.SystemFrameworks.Core.iCROP.BizLogic

    ''' <summary>
    ''' TB_M_SYSTEM_SETTING_DLRから販売店システム設定を取得する共通クラスです。
    ''' </summary>
    ''' <remarks></remarks>
    Public Class SystemSettingDlr
        Inherits BaseBusinessComponent

        ''' <summary>
        ''' キャッシュ検索キー書式
        ''' </summary>
        ''' <remarks></remarks>
        Private ReadOnly CacheKeyFormat As String = "{0:-5}{1,-3}{2}"

#Region "GetEnvSetting"


        ''' <summary>
        ''' 販売店システム設定を取得します。
        ''' </summary>
        ''' <param name="dlrCd">販売店コード</param>
        ''' <param name="strCd">店舗コード</param>
        ''' <param name="param">パラメータ</param>
        ''' <returns>TB_M_SYSTEM_SETTING_DLRRow</returns>
        ''' <remarks>
        ''' 販売店システム設定を取得します。
        ''' 店舗の設定が存在しない場合、販売店の設定、共通(XXXXX)の設定を取得します。
        ''' データが存在しない場合Nothingを返却します。
        ''' </remarks>
        Public Function GetEnvSetting(ByVal dlrCD As String,
                                                     ByVal strCD As String,
                                                     ByVal param As String) As SystemSettingDlrDataSet.TB_M_SYSTEM_SETTING_DLRRow

            If String.IsNullOrEmpty(dlrCD) OrElse String.IsNullOrEmpty(strCD) OrElse String.IsNullOrEmpty(param) Then
                Return Nothing
            End If

            Dim envDr As SystemSettingDlrDataSet.TB_M_SYSTEM_SETTING_DLRRow = Me.GetSpecificEnvSetting(dlrCD, strCD, param)
            If envDr IsNot Nothing Then
                Return envDr
            End If
            Dim key1 As String = String.Format(CacheKeyFormat, dlrCD, strCD, param)

            envDr = Me.GetSpecificEnvSetting(dlrCD, ConstantBranchCD.AllBranchCD, param)
            If envDr IsNot Nothing Then
                SettingDataCache.SystemSettingDlrCache.TryAdd(key1, envDr)      'Key1のキャッシュを生成する
                Return envDr
            End If
            Dim key2 As String = String.Format(CacheKeyFormat, dlrCD, ConstantBranchCD.AllBranchCD, param)

            envDr = Me.GetCommonEnvSetting(param)
            SettingDataCache.SystemSettingDlrCache.TryAdd(key1, envDr)          'Key1のキャッシュを生成する
            SettingDataCache.SystemSettingDlrCache.TryAdd(key2, envDr)          'Key2のキャッシュを生成する
            Return envDr

        End Function
#End Region

#Region "GetCommonEnvSetting"
        ''' <summary>
        ''' 共通(XXXXX)システム設定のみ取得します。
        ''' </summary>
        ''' <param name="param">パラメータ</param>
        ''' <returns>TB_M_SYSTEM_SETTING_DLRRow</returns>
        ''' <remarks>
        ''' 共通(XXXXX)システム設定のみ取得します。
        ''' データが存在しない場合Nothingを返却します。
        ''' </remarks>
        Private Function GetCommonEnvSetting(ByVal param As String) As SystemSettingDlrDataSet.TB_M_SYSTEM_SETTING_DLRRow

            Dim envDr As SystemSettingDlrDataSet.TB_M_SYSTEM_SETTING_DLRRow = Me.GetSpecificEnvSetting(ConstantDealerCD.AllDealerCD, ConstantBranchCD.AllBranchCD, param)
            If envDr Is Nothing Then
                Dim key As String = String.Format(CacheKeyFormat, ConstantDealerCD.AllDealerCD, ConstantBranchCD.AllBranchCD, param)
                SettingDataCache.SystemSettingDlrCache.TryAdd(key, Nothing)          'Nothing のキャッシュを生成する
            End If
            Return envDr
        End Function
#End Region

#Region "GetSpecificEnvSetting"
        ''' <summary>
        ''' 販売店システム設定のみ取得します。
        ''' </summary>
        ''' <param name="dlrCd">販売店コード</param>
        ''' <param name="strCd">店舗コード</param>
        ''' <param name="param">パラメータ</param>
        ''' <returns>TB_M_SYSTEM_SETTING_DLRRow</returns>
        ''' <remarks>
        ''' 販売店システム設定のみ取得します。
        ''' データが存在しない場合Nothingを返却します。
        ''' </remarks>
        Public Function GetSpecificEnvSetting(ByVal dlrCD As String,
                                              ByVal strCD As String,
                                              ByVal param As String) As SystemSettingDlrDataSet.TB_M_SYSTEM_SETTING_DLRRow

            If String.IsNullOrEmpty(dlrCD) OrElse String.IsNullOrEmpty(strCD) OrElse String.IsNullOrEmpty(param) Then
                Return Nothing
            End If

            SettingDataCache.SystemSettingDlrCacheIsExpired()

            Dim envDt As New SystemSettingDlrDataSet.TB_M_SYSTEM_SETTING_DLRDataTable
            Dim dataRow As SystemSettingDlrDataSet.TB_M_SYSTEM_SETTING_DLRRow = envDt.NewTB_M_SYSTEM_SETTING_DLRRow()
            Dim key As String
            key = String.Format(CacheKeyFormat, dlrCD, strCD, param)

            If SettingDataCache.SystemSettingDlrCache.TryGetValue(key, dataRow) Then
                Return dataRow
            Else
                envDt = SystemSettingDlrTableAdapter.GetSystemSettingDlrDataTable(dlrCD, strCD, param)
                If envDt.Rows.Count = 0 Then
                    Return Nothing
                End If
                dataRow = DirectCast(envDt.Rows(0), SystemSettingDlrDataSet.TB_M_SYSTEM_SETTING_DLRRow)
                SettingDataCache.SystemSettingDlrCache.TryAdd(key, dataRow)
                Return dataRow
            End If

        End Function
#End Region

    End Class

End Namespace
