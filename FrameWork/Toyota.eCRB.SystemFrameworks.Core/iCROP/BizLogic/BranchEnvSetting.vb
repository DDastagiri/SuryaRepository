Imports Toyota.eCRB.SystemFrameworks.Core.iCROP.DataAccess

Namespace Toyota.eCRB.SystemFrameworks.Core.iCROP.BizLogic

    ''' <summary>
    ''' TBL_DLRENVSETTINGから店舗設定を取得する共通クラスです。
    ''' </summary>
    ''' <remarks></remarks>
    Public Class BranchEnvSetting
        Inherits BaseBusinessComponent

        ''' <summary>
        ''' キャッシュ検索キー書式
        ''' </summary>
        ''' <remarks></remarks>
        Private ReadOnly CacheKeyFormat As String = "{0:-5}{1,-3}{2}"

#Region "GetEnvSetting"
        ''' <summary>
        ''' 店舗環境設定を取得します。
        ''' </summary>
        ''' <param name="dlrCd">販売店コード</param>
        ''' <param name="strCd">店舗コード</param>
        ''' <param name="param">パラメータ</param>
        ''' <returns>DLRENVSETTINGRow</returns>
        ''' <remarks>
        ''' 店舗環境設定を取得します。
        ''' 店舗の設定が存在しない場合、販売店の設定、共通(XXXXX)の設定を取得します。
        ''' データが存在しない場合Nothingを返却します。
        ''' </remarks>
        Public Function GetEnvSetting(ByVal dlrCD As String,
                                                     ByVal strCD As String,
                                                     ByVal param As String) As DlrEnvSettingDataSet.DLRENVSETTINGRow

            If String.IsNullOrEmpty(dlrCD) OrElse String.IsNullOrEmpty(strCD) OrElse String.IsNullOrEmpty(param) Then
                Return Nothing
            End If

            Dim envDr As DlrEnvSettingDataSet.DLRENVSETTINGRow = Me.GetSpecificEnvSetting(dlrCD, strCD, param)
            If envDr IsNot Nothing Then
                Return envDr
            End If
            Dim key1 As String = String.Format(CacheKeyFormat, dlrCD, strCD, param)

            envDr = Me.GetSpecificEnvSetting(dlrCD, ConstantBranchCD.BranchHO, param)
            If envDr IsNot Nothing Then
                SettingDataCache.DlrEnvSettingCache.TryAdd(key1, envDr)      'Key1のキャッシュを生成する
                Return envDr
            End If
            Dim key2 As String = String.Format(CacheKeyFormat, dlrCD, ConstantBranchCD.AllBranchCD, param)

            envDr = Me.GetCommonEnvSetting(param)
            SettingDataCache.DlrEnvSettingCache.TryAdd(key1, envDr)          'Key1のキャッシュを生成する
            SettingDataCache.DlrEnvSettingCache.TryAdd(key2, envDr)          'Key2のキャッシュを生成する
            Return Me.GetCommonEnvSetting(param)

        End Function
#End Region

#Region "GetCommonEnvSetting"
        ''' <summary>
        ''' 共通(XXXXX)環境設定のみ取得します。
        ''' </summary>
        ''' <param name="param">パラメータ</param>
        ''' <returns>DLRENVSETTINGRow</returns>
        ''' <remarks>
        ''' 共通(XXXXX)環境設定のみ取得します。
        ''' データが存在しない場合Nothingを返却します。
        ''' </remarks>
        Private Function GetCommonEnvSetting(ByVal param As String) As DlrEnvSettingDataSet.DLRENVSETTINGRow

            Dim envDr As DlrEnvSettingDataSet.DLRENVSETTINGRow = Me.GetSpecificEnvSetting(ConstantDealerCD.AllDealerCD, ConstantBranchCD.BranchHO, param)
            If envDr Is Nothing Then
                Dim key As String = String.Format(CacheKeyFormat, ConstantDealerCD.AllDealerCD, ConstantBranchCD.AllBranchCD, param)
                SettingDataCache.DlrEnvSettingCache.TryAdd(key, Nothing)
            End If
            Return envDr

        End Function
#End Region

#Region "GetSpecificEnvSetting"
        ''' <summary>
        ''' 店舗環境設定のみ取得します。
        ''' </summary>
        ''' <param name="dlrCd">販売店コード</param>
        ''' <param name="strCd">店舗コード</param>
        ''' <param name="param">パラメータ</param>
        ''' <returns>DLRENVSETTINGRow</returns>
        ''' <remarks>
        ''' 店舗環境設定のみ取得します。
        ''' データが存在しない場合Nothingを返却します。
        ''' </remarks>
        Public Function GetSpecificEnvSetting(ByVal dlrCD As String,
                                              ByVal strCD As String,
                                              ByVal param As String) As DlrEnvSettingDataSet.DLRENVSETTINGRow

            If String.IsNullOrEmpty(dlrCD) OrElse String.IsNullOrEmpty(strCD) OrElse String.IsNullOrEmpty(param) Then
                Return Nothing
            End If

            SettingDataCache.DlrEnvSettingCacheIsExpired()

            Dim envDt As New DlrEnvSettingDataSet.DLRENVSETTINGDataTable
            Dim dataRow As DlrEnvSettingDataSet.DLRENVSETTINGRow = envDt.NewDLRENVSETTINGRow()
            Dim key As String
            key = String.Format(CacheKeyFormat, dlrCD, strCD, param)

            If SettingDataCache.DlrEnvSettingCache.TryGetValue(key, dataRow) Then
                Return dataRow
            Else
                envDt = DlrEnvSettingTableAdapter.GetDlrEnvSettingDataTable(dlrCD, strCD, param)
                If envDt.Rows.Count = 0 Then
                    Return Nothing
                End If
                dataRow = DirectCast(envDt.Rows(0), DlrEnvSettingDataSet.DLRENVSETTINGRow)
                SettingDataCache.DlrEnvSettingCache.TryAdd(key, dataRow)
                Return dataRow
            End If

        End Function
#End Region

    End Class

End Namespace
