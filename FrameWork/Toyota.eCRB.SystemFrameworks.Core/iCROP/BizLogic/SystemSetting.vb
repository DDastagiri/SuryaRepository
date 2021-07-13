Imports Toyota.eCRB.SystemFrameworks.Core.iCROP.DataAccess
Imports System.Globalization
Imports System.Reflection

Namespace Toyota.eCRB.SystemFrameworks.Core.iCROP.BizLogic

    ''' <summary>
    ''' TB_M_SYSTEM_SETTINGのデータを取得する共通クラスです。
    ''' </summary>
    ''' <remarks></remarks>
    Public Class SystemSetting
        Inherits BaseBusinessComponent


#Region "GetSystemSetting"
        ''' <summary>
        ''' TB_M_SYSTEM_SETTINGの指定パラメータの設定値を取得します。
        ''' </summary>
        ''' <param name="param">パラメータ</param>
        ''' <returns>TB_M_SYSTEM_SETTINGRow</returns>
        ''' <remarks>
        ''' TB_M_SYSTEM_SETTINGの指定パラメータの設定値を取得します。
        ''' データが0件のとき、Nothingを返却します。
        ''' </remarks>
        Public Function GetSystemSetting(ByVal param As String) As SystemSettingDataSet.TB_M_SYSTEM_SETTINGRow

            If String.IsNullOrEmpty(param) Then
                Return Nothing
            End If

            SettingDataCache.SystemSettingCacheIsExpired()

            Dim sysDt As New SystemSettingDataSet.TB_M_SYSTEM_SETTINGDataTable
            Dim dataRow As SystemSettingDataSet.TB_M_SYSTEM_SETTINGRow = sysDt.NewTB_M_SYSTEM_SETTINGRow()

            If SettingDataCache.SystemSettingCache.TryGetValue(param, dataRow) Then
                Return dataRow
            Else

                sysDt = SystemSettingTableAdapter.GetSystemSettingDataTable(param)

                If sysDt.Rows.Count = 0 Then
                    SettingDataCache.SystemSettingCache.TryAdd(param, Nothing)
                    Return Nothing
                End If

                dataRow = DirectCast(sysDt.Rows(0), SystemSettingDataSet.TB_M_SYSTEM_SETTINGRow)

                SettingDataCache.SystemSettingCache.TryAdd(param, dataRow)
                Return dataRow
            End If

        End Function

#End Region

    End Class

End Namespace
