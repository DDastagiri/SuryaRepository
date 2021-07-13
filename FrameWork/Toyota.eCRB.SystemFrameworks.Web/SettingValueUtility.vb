Imports Toyota.eCRB.SystemFrameworks.Core.iCROP.BizLogic
Imports Toyota.eCRB.SystemFrameworks.Core.iCROP.DataAccess

Namespace Toyota.eCRB.SystemFrameworks.Web

    ''' <summary>
    ''' 環境設定データを取得する為のユーティリティ機能を提供します。
    ''' </summary>
    ''' <remarks></remarks>
    Public NotInheritable Class SettingValueUtility

        ''' <summary>
        ''' DealerEnvSettingクラスのインスタンス
        ''' </summary>
        Private Shared dealerEnvSettingInstance As DealerEnvSetting = New DealerEnvSetting()

        ''' <summary>
        ''' BranchEnvSettingクラスのインスタンス
        ''' </summary>
        Private Shared branchEnvSettingInstance As BranchEnvSetting = New BranchEnvSetting()

        ''' <summary>
        ''' SystemEnvSettingクラスのインスタンス
        ''' </summary>
        Private Shared systemEnvSettingInstance As SystemEnvSetting = New SystemEnvSetting()

        ''' <summary>
        ''' SystemSettingDlrクラスのインスタンス
        ''' </summary>
        Private Shared systemSettingDlrInstance As SystemSettingDlr = New SystemSettingDlr()

        ''' <summary>
        ''' SystemSettingクラスのインスタンス
        ''' </summary>
        Private Shared systemSettingInstance As SystemSetting = New SystemSetting()

        ''' <summary>
        ''' インスタンスの生成をできないようにするためのデフォルトのコンストラクタです。
        ''' </summary>
        ''' <remarks>
        ''' </remarks>
        Private Sub New()
        End Sub

        ''' <summary>
        ''' ログインスタッフの販売店コードをもとにTBL_DLRENVSETTINGから設定値を取得します
        ''' </summary>
        ''' <param name="param">パラメータ</param>
        ''' <returns>DLRENVSETTINGRow</returns>
        ''' <remarks>
        ''' 販売店環境設定を取得します。
        ''' </remarks>
        Public Shared Function GetDealerEnvSetting(ByVal param As String) As DlrEnvSettingDataSet.DLRENVSETTINGRow
            Return dealerEnvSettingInstance.GetEnvSetting(StaffContext.Current.DlrCD, param)
        End Function

        ''' <summary>
        ''' ログインスタッフの販売店コード、店舗コードをもとにTBL_DLRENVSETTINGから設定値を取得します
        ''' </summary>
        ''' <param name="param">パラメータ</param>
        ''' <returns>DLRENVSETTINGRow</returns>
        ''' <remarks>
        ''' 店舗環境設定を取得します。
        ''' </remarks>
        Public Shared Function GetBranchEnvSetting(ByVal param As String) As DlrEnvSettingDataSet.DLRENVSETTINGRow
            Return branchEnvSettingInstance.GetEnvSetting(StaffContext.Current.DlrCD, StaffContext.Current.BrnCD, param)
        End Function

        ''' <summary>
        ''' TBL_SYSTEMENVSETTINGから設定値を取得します
        ''' </summary>
        ''' <param name="param">パラメータ</param>
        ''' <returns>SYSTEMENVSETTINGRow</returns>
        ''' <remarks>
        ''' 指定システム環境設定のデータを取得します。
        ''' </remarks>
        Public Shared Function GetSystemEnvSetting(ByVal param As String) As SystemEnvSettingDataSet.SYSTEMENVSETTINGRow
            Return systemEnvSettingInstance.GetSystemEnvSetting(param)
        End Function

        ''' <summary>
        ''' ログインスタッフの販売店コード、店舗コードをもとにTB_M_SYSTEM_SETTING_DLRから設定値を取得します
        ''' </summary>
        ''' <param name="param">パラメータ</param>
        ''' <returns>TB_M_SYSTEM_SETTING_DLRRow</returns>
        ''' <remarks>
        ''' 店舗環境設定を取得します。
        ''' </remarks>
        Public Shared Function GetSystemSettingDlr(ByVal param As String) As SystemSettingDlrDataSet.TB_M_SYSTEM_SETTING_DLRRow
            Return systemSettingDlrInstance.GetEnvSetting(StaffContext.Current.DlrCD, StaffContext.Current.BrnCD, param)
        End Function

        ''' <summary>
        ''' TB_M_SYSTEM_SETTINGから設定値を取得します
        ''' </summary>
        ''' <param name="param">パラメータ</param>
        ''' <returns>TB_M_SYSTEM_SETTINGRow</returns>
        ''' <remarks>
        ''' 指定システム環境設定のデータを取得します。
        ''' </remarks>
        Public Shared Function GetSystemSetting(ByVal param As String) As SystemSettingDataSet.TB_M_SYSTEM_SETTINGRow
            Return systemSettingInstance.GetSystemSetting(param)
        End Function

    End Class

End Namespace