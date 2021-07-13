Imports Toyota.eCRB.SystemFrameworks.Core
Imports Toyota.eCRB.Tool.Notify.Api.DataAccess
Imports Toyota.eCRB.SystemFrameworks.Web
Imports Toyota.eCRB.SystemFrameworks.Core.iCROP.BizLogic
Imports Toyota.eCRB.SystemFrameworks.Core.iCROP.DataAccess
Imports System.Text

Public Class IC3040802BusinessLogic
    Inherits BaseBusinessComponent
    Implements IDisposable

#Region "定数"

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

        Dim unreadCount As Long = 0
        Try
            Using da As New IC3040802DataSetTableAdapters.IC3040802TableAdapters
                'ユーザー情報の取得
                Dim staffInfo As StaffContext = StaffContext.Current
                'ユーザーの権限情報を取得する
                Dim staffAuthority As Boolean = StaffOperationCode(staffInfo)
                'TBL_DLRENVSETTINGからデータを取り出す
                Dim daBranchEnvSetting As New BranchEnvSetting
                Dim paramValue As String =
                    daBranchEnvSetting.GetEnvSetting(staffInfo.DlrCD,
                                                     staffInfo.BrnCD,
                                                     ParameterName).PARAMVALUE

                Dim sendDate As Date = Date.Now.AddDays(-CInt(paramValue))
                '未読件数の取得
                unreadCount = da.SelectUnreadNotice(staffInfo.Account, sendDate, staffAuthority)
            End Using
        Finally
            Logger.Info(getReturnParam(CStr(unreadCount)))
            Logger.Info(getLogMethod(System.Reflection.MethodBase.GetCurrentMethod.Name, False))
        End Try

        Return unreadCount
    End Function

    ''' <summary>
    ''' スタッフ権限情報取得
    ''' </summary>
    ''' <param name="staffInfo">ユーザー情報</param>
    ''' <returns>TRUE:セールス、FALSE:サービス</returns>
    ''' <remarks></remarks>
    Private Function StaffOperationCode(ByVal staffInfo As StaffContext) As Boolean
        Logger.Info(getLogMethod(System.Reflection.MethodBase.GetCurrentMethod.Name, True))

        Dim staffAuthority As Boolean = False
        Select Case staffInfo.OpeCD
            Case Operation.SSM,
                 Operation.SSF
                staffAuthority = True
            Case Operation.SA,
                 Operation.SM,
                 Operation.TEC,
                 CType(52, Operation),
                 Operation.PS,
                 Operation.CT
                staffAuthority = False
        End Select

        Return staffAuthority
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

#Region "IDisposableインターフェイス"
    ''' <summary>
    ''' IDisposableインターフェイス.Dispoase
    ''' </summary>
    ''' <remarks></remarks>
    Public Overloads Sub Dispose() Implements IDisposable.Dispose
        Dispose(True)
        GC.SuppressFinalize(Me)
    End Sub
    Protected Overridable Overloads Sub Dispose(ByVal disposing As Boolean)
        If disposing Then

        End If
    End Sub
#End Region

End Class