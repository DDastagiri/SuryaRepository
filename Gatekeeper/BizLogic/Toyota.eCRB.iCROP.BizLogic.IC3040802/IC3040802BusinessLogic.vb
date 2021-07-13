Imports Toyota.eCRB.SystemFrameworks.Core
Imports Toyota.eCRB.Tool.Notify.Api.DataAccess
Imports Toyota.eCRB.SystemFrameworks.Web
Imports Toyota.eCRB.SystemFrameworks.Core.iCROP.BizLogic
Imports Toyota.eCRB.SystemFrameworks.Core.iCROP.DataAccess
Imports System.Text

Public Class IC3040802BusinessLogic
    Inherits BaseBusinessComponent

#Region "定数"
    ''' <summary>
    ''' 数値
    ''' </summary>
    Private Const ZeroLong As Long = 0
    ''' <summary>
    ''' パラメータ名
    ''' </summary>
    Private Const ParameterName As String = "NOTICE_DISP_DAYS"
#End Region

#Region "未読件数件数取得処理"
    ''' <summary>
    ''' 未読件数件数取得処理"
    ''' </summary>
    ''' <returns>未読件数</returns>
    ''' <remarks></remarks>
    Public Function GetUnreadNotice() As Long
        Logger.Info(getLogMethod(System.Reflection.MethodBase.GetCurrentMethod.Name, True))

        Dim unreadCount As Long = ZeroLong
        Try
            Using da As New IC3040802DataSetTableAdapters.IC3040802TableAdapters
                'TBL_DLRENVSETTINGからデータを取り出す
                'ユーザー情報の取得
                Dim staffInfo As StaffContext
                staffInfo = StaffContext.Current
                Dim daBranchEnvSetting As New BranchEnvSetting
                Dim paramValue As String = daBranchEnvSetting.GetEnvSetting(staffInfo.DlrCD, staffInfo.BrnCD, ParameterName).PARAMVALUE

                Dim sendDate As Date = Date.Now.AddDays(-CInt(paramValue))
                '未読件数の取得
                unreadCount = da.SelectUnreadNotice(staffInfo.Account, sendDate)
            End Using
        Finally
            Logger.Info(getReturnParam(CStr(unreadCount)))
            Logger.Info(getLogMethod(System.Reflection.MethodBase.GetCurrentMethod.Name, False))
        End Try

        Return unreadCount
    End Function
#End Region

#Region "ログデータ加工処理"
    ''' <summary>
    ''' ログデータ（メソッド）
    ''' </summary>
    ''' <param name="methodName">メソッド名</param>
    ''' <param name="startEndFlag">True：「method start」を表示、False：「method end」を表示</param>
    ''' <returns>加工した文字列</returns>
    ''' <remarks></remarks>
    Private Function getLogMethod(ByVal methodName As String,
                                ByVal startEndFlag As Boolean) As String

        Dim sb As New StringBuilder
        With sb
            .Append("[")
            .Append(methodName)
            .Append("]")
            If startEndFlag Then
                .Append(" method start")
            Else
                .Append(" method end")
            End If
        End With
        Return sb.ToString
    End Function

    ''' <summary>
    ''' ログデータ（戻り値）
    ''' </summary>
    ''' <param name="paramData">引数値</param>
    ''' <returns>加工した文字列</returns>
    ''' <remarks></remarks>
    Private Function getReturnParam(ByVal paramData As String) As String
        Dim sb As New StringBuilder
        With sb
            .Append("Return=")
            .Append(paramData)
        End With
        Return sb.ToString
    End Function
#End Region

End Class