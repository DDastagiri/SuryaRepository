Imports Toyota.eCRB.SystemFrameworks.Core.iCROP.DataAccess

Namespace Toyota.eCRB.SystemFrameworks.Core.iCROP.BizLogic

    ''' <summary>
    ''' TBL_DLRENVSETTINGから販売店の設定を取得する共通クラスです。
    ''' </summary>
    ''' <remarks></remarks>
    Public Class DealerEnvSetting
        Inherits BaseBusinessComponent

        ''' <summary>
        ''' キャッシュ検索キー書式
        ''' </summary>
        ''' <remarks></remarks>
        Private ReadOnly CacheKeyFormat As String = "{0:-5}{1,-3}{2}"


#Region "GetEnvSetting"
        ''' <summary>
        ''' 販売店環境設定を取得します。
        ''' </summary>
        ''' <param name="dlrCd">販売店コード</param>
        ''' <param name="param">パラメータ</param>
        ''' <returns>DLRENVSETTINGRow</returns>
        ''' <remarks>
        ''' 販売店環境設定を取得します。
        ''' 販売店の設定が存在しない場合、共通(XXXXX)の設定を取得します。
        ''' データが存在しない場合Nothingを返却します。
        ''' </remarks>
        Public Function GetEnvSetting(ByVal dlrCD As String,
                                                    ByVal param As String) As DlrEnvSettingDataSet.DLRENVSETTINGRow

            If String.IsNullOrEmpty(dlrCD) OrElse String.IsNullOrEmpty(param) Then
                Return Nothing
            End If

            Dim branchEnv As New BranchEnvSetting
            Dim envDr As DlrEnvSettingDataSet.DLRENVSETTINGRow = branchEnv.GetSpecificEnvSetting(dlrCD, ConstantBranchCD.BranchHO, param)
            If envDr IsNot Nothing Then
                Return envDr
            End If
            Dim key1 As String = String.Format(CacheKeyFormat, dlrCD, ConstantBranchCD.BranchHO, param)

            envDr = Me.GetCommonEnvSetting(param)
            SettingDataCache.DlrEnvSettingCache.TryAdd(key1, envDr)          'Key1のキャッシュを生成する
            Return envDr

        End Function
#End Region

#Region "GetSpecificEnvSetting"
        ''' <summary>
        ''' 販売店環境設定のみ取得します。
        ''' </summary>
        ''' <param name="dlrCd">販売店コード</param>
        ''' <param name="param">パラメータ</param>
        ''' <returns>DLRENVSETTINGRow</returns>
        ''' <remarks>
        ''' 販売店環境設定のみ取得します。
        ''' データが存在しない場合Nothingを返却します。
        ''' </remarks>
        Public Function GetSpecificEnvSetting(ByVal dlrCD As String,
                                               ByVal param As String) As DlrEnvSettingDataSet.DLRENVSETTINGRow

            Dim branchEnv As New BranchEnvSetting
            Dim envDr As DlrEnvSettingDataSet.DLRENVSETTINGRow = branchEnv.GetSpecificEnvSetting(dlrCD, ConstantBranchCD.BranchHO, param)
            If envDr Is Nothing Then
                Dim key As String = String.Format(CacheKeyFormat, dlrCD, ConstantBranchCD.AllBranchCD, param)
                SettingDataCache.DlrEnvSettingCache.TryAdd(key, Nothing)
            End If
            Return envDr
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

            Dim branchEnv As New BranchEnvSetting
            Dim envDr As DlrEnvSettingDataSet.DLRENVSETTINGRow = branchEnv.GetSpecificEnvSetting(ConstantDealerCD.AllDealerCD, ConstantBranchCD.BranchHO, param)
            If envDr Is Nothing Then
                Dim key As String = String.Format(CacheKeyFormat, ConstantDealerCD.AllDealerCD, ConstantBranchCD.AllBranchCD, param)
                SettingDataCache.DlrEnvSettingCache.TryAdd(key, Nothing)
            End If
            Return envDr

        End Function
#End Region

    End Class

End Namespace
