Imports Toyota.eCRB.SystemFrameworks.Core.iCROP.DataAccess
Imports System.Globalization
Imports System.Reflection

Namespace Toyota.eCRB.SystemFrameworks.Core.iCROP.BizLogic

    ''' <summary>
    ''' TBL_SYSTEMENVSETTINGのデータを取得する共通クラスです。
    ''' </summary>
    ''' <remarks></remarks>
    Public Class SystemEnvSetting
        Inherits BaseBusinessComponent

        '2013/06/30 TCS 山田 2013/10対応版 既存流用 START
        ''' <summary>
        ''' ロック待機時間格納用
        ''' </summary>
        ''' <remarks></remarks>
        Private Shared lockWaitTime As String = String.Empty

        ''' <summary>
        ''' ロック待機時間PARAMNAME
        ''' </summary>
        ''' <remarks></remarks>
        Private Const UPDATE_LOCK_TIMEOUT As String = "UPDATE_LOCK_TIMEOUT"
        '2013/06/30 TCS 山田 2013/10対応版 既存流用 END

#Region "GetSelectSystemEnvSetting"
        ''' <summary>
        ''' 指定システム環境設定のデータを取得します。
        ''' </summary>
        ''' <param name="param">パラメータ</param>
        ''' <returns>SYSTEMENVSETTINGRow</returns>
        ''' <remarks>
        ''' 指定システム環境設定のデータを取得します。
        ''' データが0件のとき、Nothingを返却します。
        ''' </remarks>
        Public Function GetSystemEnvSetting(ByVal param As String) As SystemEnvSettingDataSet.SYSTEMENVSETTINGRow

            ' 2013/06/30 TCS 山田 2013/10対応版　既存流用 START
            ' ======================== ログ出力 開始 ========================
            Logger.Info(String.Format(CultureInfo.InvariantCulture,
                                      " {0}_Start",
                                      MethodBase.GetCurrentMethod.Name))
            ' ======================== ログ出力 終了 ========================
            '2013/06/30 TCS 山田 2013/10対応版 既存流用 END

            If String.IsNullOrEmpty(param) Then
                ' 2013/06/30 TCS 山田 2013/10対応版　既存流用 START
                ' ======================== ログ出力 開始 ========================
                Logger.Info(String.Format(CultureInfo.InvariantCulture,
                                          " {0}_End, Return:[{1}]",
                                          MethodBase.GetCurrentMethod.Name, "Nothing"))
                ' ======================== ログ出力 終了 ========================
                '2013/06/30 TCS 山田 2013/10対応版 既存流用 END
                Return Nothing
            End If

            SettingDataCache.SystemEnvSettingCacheIsExpired()

            Dim sysDt As New SystemEnvSettingDataSet.SYSTEMENVSETTINGDataTable
            Dim dataRow As SystemEnvSettingDataSet.SYSTEMENVSETTINGRow = sysDt.NewSYSTEMENVSETTINGRow()

            If SettingDataCache.SystemEnvSettingCache.TryGetValue(param, dataRow) Then
                If param.Equals(UPDATE_LOCK_TIMEOUT) Then
                    lockWaitTime = CStr(dataRow.Item("PARAMVALUE"))
                End If

                Return dataRow
            Else
                sysDt = SystemEnvSettingTableAdapter.GetSystemEnvSettingDataTable(param, EnvironmentSetting.CountryCode)

                If sysDt.Rows.Count = 0 Then
                    ' 2013/06/30 TCS 山田 2013/10対応版　既存流用 START
                    ' ======================== ログ出力 開始 ========================
                    Logger.Info(String.Format(CultureInfo.InvariantCulture,
                                              " {0}_End, Return:[{1}]",
                                              MethodBase.GetCurrentMethod.Name, "Nothing"))
                    ' ======================== ログ出力 終了 ========================
                    '2013/06/30 TCS 山田 2013/10対応版 既存流用 END

                    SettingDataCache.SystemEnvSettingCache.TryAdd(param, Nothing)
                    Return Nothing
                End If

                dataRow = DirectCast(sysDt.Rows(0), SystemEnvSettingDataSet.SYSTEMENVSETTINGRow)

                '2013/06/30 TCS 山田 2013/10対応版 既存流用 START
                If param.Equals(UPDATE_LOCK_TIMEOUT) Then
                    lockWaitTime = CStr(dataRow.Item("PARAMVALUE"))
                    ' ======================== ログ出力 開始 ========================
                    Logger.Info(String.Format(CultureInfo.InvariantCulture,
                                              " {0}_End, Return:[{1}]",
                                              MethodBase.GetCurrentMethod.Name, "Nothing"))
                    ' ======================== ログ出力 終了 ========================
                    SettingDataCache.SystemEnvSettingCache.TryAdd(param, dataRow)
                    Return Nothing
                End If

                ' ======================== ログ出力 開始 ========================
                Logger.Info(String.Format(CultureInfo.InvariantCulture,
                                          " {0}_End, Return:[{1}]",
                                          MethodBase.GetCurrentMethod.Name, sysDt.Rows.Count))
                ' ======================== ログ出力 終了 ========================
                '2013/06/30 TCS 山田 2013/10対応版 既存流用 END

                SettingDataCache.SystemEnvSettingCache.TryAdd(param, dataRow)
                Return dataRow
            End If

        End Function

        '2013/06/30 TCS 山田 2013/10対応版 既存流用 START
        ''' <summary>
        ''' ロック待機時間を取得します。
        ''' </summary>
        ''' <remarks>
        ''' 変数に格納されているロック待機時間を返却します。
        ''' 変数に値が格納されていなかった場合、ロック待機時間を再取得します。
        ''' それでも取得できなかった場合には、Nothingを返します。
        ''' </remarks>
        Public Function GetLockWaitTime() As String

            ' ======================== ログ出力 開始 ========================
            Logger.Info(String.Format(CultureInfo.InvariantCulture,
                                      " {0}_Start",
                                      MethodBase.GetCurrentMethod.Name))
            ' ======================== ログ出力 終了 ========================

            ' ロック待機時間が格納されていない場合
            If String.IsNullOrEmpty(lockWaitTime) Then

                ' ロック待機時間を再取得
                GetSystemEnvSetting(UPDATE_LOCK_TIMEOUT)

                If String.IsNullOrEmpty(lockWaitTime) Then
                    ' ======================== ログ出力 開始 ========================
                    Logger.Info(String.Format(CultureInfo.InvariantCulture,
                                              " {0}_End, Return:[{1}]",
                                              MethodBase.GetCurrentMethod.Name, "Nothing"))
                    ' ======================== ログ出力 終了 ========================
                    Return Nothing
                End If

            End If

            ' ======================== ログ出力 開始 ========================
            Logger.Info(String.Format(CultureInfo.InvariantCulture,
                                      " {0}_End, Return:[{1}]",
                                      MethodBase.GetCurrentMethod.Name, lockWaitTime))
            ' ======================== ログ出力 終了 ========================
            Return lockWaitTime

        End Function
        '2013/06/30 TCS 山田 2013/10対応版 既存流用 END

#End Region

    End Class

End Namespace
