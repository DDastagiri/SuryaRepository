Imports Toyota.eCRB.SystemFrameworks.Core.iCROP.DataAccess

Namespace Toyota.eCRB.SystemFrameworks.Core.iCROP.BizLogic

    ''' <summary>
    ''' TBL_FUNCTIONSETTINGからデータを取得する共通クラスです。
    ''' </summary>
    ''' <remarks></remarks>
    Public Class FunctionSetting
        Inherits BaseBusinessComponent

#Region "定数"
        Private Const C_FLG_ON As String = "1"
        Private Const C_FLG_OFF As String = "0"
#End Region

#Region "GetiCROPFunctionSetting"
        ''' <summary>
        ''' iCROPの設定取得方法で機能設定を取得します。
        ''' </summary>
        ''' <param name="dlrCd">販売店コード</param>
        ''' <param name="param">パラメータ</param>
        ''' <returns>機能設定値</returns>
        ''' <remarks>
        ''' iCROPの設定取得方法で機能設定を取得します。
        ''' TBL_SYSTEMENVSETTING⇒TBL_DLRENVSETTING(販売店)⇒TBL_DLRENVSETTING(共通)の
        ''' 順で取得していきます。
        ''' </remarks>
        Public Function GetiCROPFunctionSetting(ByVal dlrCD As String, ByVal param As String) As Integer

            Dim sysEnv As New SystemEnvSetting
            Dim sysEnvDr As SystemEnvSettingDataSet.SYSTEMENVSETTINGRow = sysEnv.GetSystemEnvSetting(param)

            If sysEnvDr IsNot Nothing Then
                If sysEnvDr.PARAMVALUE.Equals(C_FLG_OFF) Then
                    Return CInt(C_FLG_OFF)
                End If
            End If

            Dim funcSetting As New FunctionSetting
            Dim funcDr As FunctionSettingDataSet.FUNCTIONSETTINGRow = funcSetting.GetFunctionSetting(dlrCD, param)

            If funcDr IsNot Nothing Then
                Return CInt(funcDr.FUNCSTATUS)
            End If

            Return CInt(C_FLG_ON)

        End Function
#End Region

#Region "GetSelectFunctionSetting"
        ''' <summary>
        ''' 指定機能設定を取得します。
        ''' </summary>
        ''' <param name="dlrCd">販売店コード</param>
        ''' <param name="param">パラメータ</param>
        ''' <returns>FUNCTIONSETTINGRow</returns>
        ''' <remarks>
        ''' 指定機能設定を取得します。
        ''' 販売店の設定が存在しない場合、共通(XXXXX)を取得します。
        ''' データが存在しない場合Nothingを返却します。
        ''' </remarks>
        Public Function GetFunctionSetting(ByVal dlrCD As String, ByVal param As String) As FunctionSettingDataSet.FUNCTIONSETTINGRow

            If String.IsNullOrEmpty(dlrCD) OrElse String.IsNullOrEmpty(param) Then
                Return Nothing
            End If

            Dim funcDr As FunctionSettingDataSet.FUNCTIONSETTINGRow = Me.GetDelaerFunctionSetting(dlrCD, param)
            If funcDr IsNot Nothing Then
                Return funcDr
            End If

            Return Me.GetCommonFunctionSetting(param)

        End Function
#End Region

#Region "GetCommonFunctionSetting"
        ''' <summary>
        ''' 指定機能設定(共通)のみ取得します。
        ''' </summary>
        ''' <param name="param">パラメータ</param>
        ''' <returns>FUNCTIONSETTINGRow</returns>
        ''' <remarks>
        ''' 指定機能設定を取得します。
        ''' データが存在しない場合Nothingを返却します。
        ''' </remarks>
        Private Function GetCommonFunctionSetting(ByVal param As String) As FunctionSettingDataSet.FUNCTIONSETTINGRow

            Return Me.GetDelaerFunctionSetting(ConstantDealerCD.AllDealerCD, param)

        End Function
#End Region

#Region "GetDelaerFunctionSetting"
        ''' <summary>
        ''' 指定機能設定(販売店)のみ取得します。
        ''' </summary>
        ''' <param name="dlrCd">販売店コード</param>
        ''' <param name="param">パラメータ</param>
        ''' <returns>FUNCTIONSETTINGRow</returns>
        ''' <remarks>
        ''' 指定機能設定を取得します。
        ''' データが存在しない場合Nothingを返却します。
        ''' </remarks>
        Public Function GetDelaerFunctionSetting(ByVal dlrCD As String,
                                                           ByVal param As String) As FunctionSettingDataSet.FUNCTIONSETTINGRow

            If String.IsNullOrEmpty(dlrCD) OrElse String.IsNullOrEmpty(param) Then
                Return Nothing
            End If

            Dim funcDt As FunctionSettingDataSet.FUNCTIONSETTINGDataTable

            funcDt = FunctionSettingTableAdapter.GetFunctionSettingDataTable(dlrCD, param)

            If funcDt.Rows.Count = 0 Then
                Return Nothing
            End If

            Return DirectCast(funcDt.Rows(0), FunctionSettingDataSet.FUNCTIONSETTINGRow)

        End Function
#End Region
    End Class

End Namespace
