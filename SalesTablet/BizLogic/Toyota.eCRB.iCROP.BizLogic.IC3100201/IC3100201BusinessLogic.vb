Imports System.Text
Imports Toyota.eCRB.SystemFrameworks.Core
Imports Toyota.eCRB.SystemFrameworks.Web
Imports Toyota.eCRB.Visit.NotDealCustomer.DataAccess

''' <summary>
''' 未対応来店客件数取得用I/Fのビジネスロジッククラス
''' </summary>
''' <remarks></remarks>
Public Class IC3100201BusinessLogic
    Inherits BaseBusinessComponent

#Region "未対応来店客件数取得処理"

    ''' <summary>
    ''' 未対応来店客件数取得処理
    ''' </summary>
    ''' <returns>未対応来店客件数</returns>
    ''' <remarks></remarks>
    Public Function GetNotDealCount() As Long
        ' メソッド名を取得
        Dim methodName As String = System.Reflection.MethodBase.GetCurrentMethod.Name
        ' 開始ログの出力
        Logger.Info(getLogMethod(methodName, True))

        ' 戻り値
        Dim notDealCount As Long = 0

        Try
            ' セッションからユーザー情報を取得
            Dim staffInfo As StaffContext = StaffContext.Current

            Dim dtRet As IC3100201DataSetDataSet.IC3100201NotDealCountDataTable = Nothing

            ' セッションから取得した情報をもとに未対応来店客件数を取得
            dtRet = IC3100201TableAdapter.GetNotDealCount( _
                              staffInfo.DlrCD _
                            , staffInfo.BrnCD _
                            , staffInfo.Account _
                            , DateTimeFunc.Now(staffInfo.DlrCD) _
                    )

            ' データ取得できた場合
            If 0 < dtRet.Rows.Count Then
                ' データテーブルから件数を取得
                notDealCount = dtRet.Item(0).COUNT_NOT_DEAL_VISIT
            End If

            ' 復旧できない予測不能な例外しか発生しないので規約に従いThrowする
        Finally
            ' 戻り値と終了ログの出力
            Logger.Info(getReturnParam(CStr(notDealCount)))
            Logger.Info(getLogMethod(methodName, False))
        End Try

        Return notDealCount

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
