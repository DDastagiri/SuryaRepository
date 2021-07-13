Imports System.Collections.Concurrent
Imports Toyota.eCRB.SystemFrameworks.Core.iCROP.DataAccess
Imports System.Configuration

Namespace Toyota.eCRB.SystemFrameworks.Core.iCROP.BizLogic

    ''' <summary>
    ''' 設定データのキャッシュ用変数管理モジュール
    ''' </summary>
    ''' <remarks></remarks>
    Friend Module SettingDataCache

        ''' <summary>
        ''' 期限切れ経過時刻変数を構成情報から取得するキー
        ''' </summary>
        ''' <remarks></remarks>
        Private Const EXPIRE_SECONDS_KEY As String = "SettingValueExpiration"

        ''' <summary>
        ''' 期限切れ経過時刻 デフォルト値
        ''' </summary>
        ''' <remarks></remarks>
        Private Const EXPIRE_SECONDS_DEF_VALUE As Double = 600


        ''' <summary>
        ''' 期限切れ経過時刻変数
        ''' </summary>
        Private __expireSeconds As Nullable(Of Double) = Nothing

        ''' <summary>
        ''' 期限切れ経過時刻
        ''' </summary>
        Friend ReadOnly Property ExpireSeconds As Double
            Get
                If __expireSeconds Is Nothing Then
                    Dim val As String = ConfigurationManager.AppSettings.[Get](EXPIRE_SECONDS_KEY)
                    If String.IsNullOrEmpty(val) Then
                        __expireSeconds = EXPIRE_SECONDS_DEF_VALUE
                    Else
                        __expireSeconds = CType(ComponentModel.TypeDescriptor.GetConverter(GetType(Double)).ConvertFrom(val), Double)
                    End If

                End If
                Return __expireSeconds.Value
            End Get
        End Property

        ''' <summary>
        ''' TBL_SYSTEMENVSETTINGデータキャッシュ用変数
        ''' </summary>
        Friend Property SystemEnvSettingCache As ConcurrentDictionary(Of String, SystemEnvSettingDataSet.SYSTEMENVSETTINGRow) = New ConcurrentDictionary(Of String, SystemEnvSettingDataSet.SYSTEMENVSETTINGRow)

        ''' <summary>
        ''' TBL_SYSTEMENVSETTINGデータキャッシュ有効期間管理用の日時
        ''' </summary>
        Private Property SystemEnvSettingCacheTime As DateTime

        ''' <summary>
        ''' TBL_SYSTEMENVSETTINGデータキャッシュの有効期間をチェックし、期限切れの場合はキャッシュをクリアします。
        ''' </summary>
        Friend Sub SystemEnvSettingCacheIsExpired()
            If ExpireSeconds = 0 Then Return
            If (DateTime.Now - SystemEnvSettingCacheTime).TotalSeconds > ExpireSeconds Then
                SystemEnvSettingCache.Clear()
                SystemEnvSettingCacheTime = DateTime.Now
            End If
        End Sub

        ''' <summary>
        ''' TBL_DLRENVSETTINGデータキャッシュ用変数
        ''' </summary>
        Friend Property DlrEnvSettingCache As ConcurrentDictionary(Of String, DlrEnvSettingDataSet.DLRENVSETTINGRow) = New ConcurrentDictionary(Of String, DlrEnvSettingDataSet.DLRENVSETTINGRow)

        ''' <summary>
        ''' TBL_DLRENVSETTINGデータキャッシュ有効期間管理用の日時
        ''' </summary>
        Private Property DlrEnvSettingCacheTime As DateTime

        ''' <summary>
        ''' TBL_DLRENVSETTINGデータキャッシュの有効期間をチェックし、期限切れの場合はキャッシュをクリアします。
        ''' </summary>
        Friend Sub DlrEnvSettingCacheIsExpired()
            If ExpireSeconds = 0 Then Return
            If (DateTime.Now - DlrEnvSettingCacheTime).TotalSeconds > ExpireSeconds Then
                DlrEnvSettingCache.Clear()
                DlrEnvSettingCacheTime = DateTime.Now
            End If
        End Sub


        ''' <summary>
        ''' TB_M_SYSTEM_SETTING_DLRデータキャッシュ用変数
        ''' </summary>
        Friend Property SystemSettingDlrCache As ConcurrentDictionary(Of String, SystemSettingDlrDataSet.TB_M_SYSTEM_SETTING_DLRRow) = New ConcurrentDictionary(Of String, SystemSettingDlrDataSet.TB_M_SYSTEM_SETTING_DLRRow)

        ''' <summary>
        ''' TB_M_SYSTEM_SETTING_DLRデータキャッシュ有効期間管理用の日時
        ''' </summary>
        Private Property SystemSettingDlrCacheTime As DateTime

        ''' <summary>
        ''' TB_M_SYSTEM_SETTING_DLRデータキャッシュの有効期間をチェックし、期限切れの場合はキャッシュをクリアします。
        ''' </summary>
        Friend Sub SystemSettingDlrCacheIsExpired()
            If ExpireSeconds = 0 Then Return
            If (DateTime.Now - SystemSettingDlrCacheTime).TotalSeconds > ExpireSeconds Then
                SystemSettingDlrCache.Clear()
                SystemSettingDlrCacheTime = DateTime.Now
            End If
        End Sub


        ''' <summary>
        ''' TB_M_SYSTEM_SETTINGデータキャッシュ用変数
        ''' </summary>
        Friend Property SystemSettingCache As ConcurrentDictionary(Of String, SystemSettingDataSet.TB_M_SYSTEM_SETTINGRow) = New ConcurrentDictionary(Of String, SystemSettingDataSet.TB_M_SYSTEM_SETTINGRow)

        ''' <summary>
        ''' TB_M_SYSTEM_SETTINGデータキャッシュ有効期間管理用の日時
        ''' </summary>
        Private Property SystemSettingCacheTime As DateTime

        ''' <summary>
        ''' TB_M_SYSTEM_SETTING_DLRデータキャッシュの有効期間をチェックし、期限切れの場合はキャッシュをクリアします。
        ''' </summary>
        Friend Sub SystemSettingCacheIsExpired()
            If ExpireSeconds = 0 Then Return
            If (DateTime.Now - SystemSettingCacheTime).TotalSeconds > ExpireSeconds Then
                SystemSettingCache.Clear()
                SystemSettingCacheTime = DateTime.Now
            End If
        End Sub

    End Module
End Namespace