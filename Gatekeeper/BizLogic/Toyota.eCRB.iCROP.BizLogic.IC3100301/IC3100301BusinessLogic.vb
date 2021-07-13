Option Strict On
Option Explicit On

Imports System.Text
Imports Toyota.eCRB.SystemFrameworks.Core
Imports Toyota.eCRB.Common.VisitResult.BizLogic

''' <summary>
''' 来店実績更新インターフェースビジネスロジックの実装クラス
''' </summary>
''' <remarks></remarks>
Public Class IC3100301BusinessLogic
    Inherits BaseBusinessComponent
    Implements IIC3100301BusinessLogic

#Region "来店実績更新_ログイン"

    ''' <summary>
    ''' 来店実績更新_ログイン
    ''' </summary>
    ''' <param name="updateId">更新機能ID</param>
    ''' <param name="resultId">終了コード</param>
    ''' <return>更新件数</return>
    ''' <remarks>
    ''' ログイン時に必要な来店実績データの更新を行う。
    ''' 本メソッドでは、以下のメソッドも利用しているため、例外情報に関しては、下記、メソッドを参照してください。
    ''' <seealso cref="UpdateSalesVisitBusinessLogic.UpdateVisitLogin" />
    ''' </remarks>
    <EnableCommit()> _
    Public Function UpdateVisitLogin( _
            ByVal updateId As String, ByRef resultId As Integer) As Integer _
            Implements IIC3100301BusinessLogic.UpdateVisitLogin

        Logger.Info(New StringBuilder("UpdateVisitLogin_Start Param[").Append(updateId).Append( _
                ", ").Append(resultId).Append("]").ToString())

        Dim count As Integer = 0

        Try
            ' 来店実績更新_ログイン
            Logger.Info(New StringBuilder( _
                    "UpdateVisitLogin_001 Call_Start UpdateSalesVisitBusinessLogic.UpdateVisitLogin Param[").Append( _
                    updateId).Append(", ").Append(resultId).Append("]").ToString())
            count = New UpdateSalesVisitBusinessLogic().UpdateVisitLogin(updateId, resultId)
            Logger.Info(New StringBuilder( _
                    "UpdateVisitLogin_001 Call_End UpdateSalesVisitBusinessLogic.UpdateVisitLogin Ret[").Append( _
                    count).Append("]").ToString())

            ' 処理に失敗した場合
            If 0 <> resultId Then
                Logger.Info("UpdateVisitLogin_001")
                ' ロールバックを設定
                Me.Rollback = True
            End If

            ' データベースの操作中に例外が発生した場合
        Catch ex As OracleExceptionEx
            Logger.Info("UpdateVisitLogin_002")
            Logger.Error("An exception occurred during the operation of the database.", ex)
            ' ロールバックを設定
            Me.Rollback = True
            Logger.Info(New StringBuilder("UpdateVisitLogin_Ex Ret[").Append(ex).Append( _
                    ", ").Append(resultId).Append("]").ToString())
            Throw
        End Try

        Logger.Info(New StringBuilder("UpdateVisitLogin_End Ret[").Append(count).Append( _
                ", ").Append(resultId).Append("]").ToString())

        ' 戻り値に更新件数を設定
        Return count

    End Function

    ''' <summary>
    ''' 来店実績更新_ログイン時のPush送信
    ''' </summary>
    ''' <remarks>
    ''' ログインの処理が全て終了した後に呼び出され、Push送信を行う
    ''' 本メソッドでは、以下のメソッドも利用しているため、例外情報に関しては、下記、メソッドを参照してください。
    ''' <seealso cref="UpdateSalesVisitBusinessLogic.PushUpdateVisitLogin" />
    ''' </remarks>
    Public Sub PushUpdateVisitLogin()

        Logger.Info("PushUpdateVisitLogin_Start")

        Logger.Info("PushUpdateVisitLogin_001 Call_Start UpdateSalesVisitBusinessLogic.PushUpdateVisitLogin")
        Dim bl As New UpdateSalesVisitBusinessLogic()
        bl.PushUpdateVisitLogin()
        Logger.Info("PushUpdateVisitLogin_001 Call_End UpdateSalesVisitBusinessLogic.PushUpdateVisitLogin")

        Logger.Info("PushUpdateVisitLogin_End")

    End Sub

#End Region

End Class
