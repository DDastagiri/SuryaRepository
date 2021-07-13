Imports Toyota.eCRB.SystemFrameworks.Batch
Imports Toyota.eCRB.SystemFrameworks.Core
Imports Toyota.eCRB.Visit.VisitResult.Batch.BizLogic
Imports System.Text

Namespace Toyota.eCRB.iCROP.Batch

    ''' <summary>
    ''' 来店実績データ退避バッチ バッチ
    ''' </summary>
    ''' <remarks></remarks>
    Public Class MC3100301
        Implements IBatch

#Region "バッチ終了コード"

        ''' <summary>
        ''' 正常終了
        ''' </summary>
        ''' <remarks></remarks>
        Private Const Success As Integer = 0

#End Region

#Region "来店データ退避バッチ実行"

        ''' <summary>
        ''' 来店データ退避バッチ実行
        ''' </summary>
        ''' <param name="args">コマンド引数</param>
        ''' <returns>メッセージID</returns>
        ''' <remarks></remarks>
        Public Function Execute(ByVal args() As String) As Integer Implements IBatch.Execute

            Dim executeStartLog As New StringBuilder
            With executeStartLog
                .Append("MC3100301 Execute_Start param[")
                .Append(args)
                .Append("]")
            End With

            Logger.Info(executeStartLog.ToString)

            Dim bizClass As New MC3100301BusinessLogic
            Dim deleteDate As New Date
            Dim resultCode As Integer = Success

            '日付取得を行う
            deleteDate = bizClass.SaveVisitResult(resultCode)
            If resultCode <> Success Then

                Logger.Info("MC3100301 Execute_End Ret[" + CStr(resultCode) + "]")
                Return resultCode
            End If

            '来店車両実績 移行と削除
            resultCode = bizClass.VisitVehicle(deleteDate)
            If resultCode <> Success Then

                Logger.Info("MC3100301 Execute_End Ret[" + CStr(resultCode) + "]")
                Return resultCode
            End If

            'セールス来店実績 移行と削除
            resultCode = bizClass.VisitSales(deleteDate)
            If resultCode <> Success Then

                Logger.Info("MC3100301 Execute_End Ret[" + CStr(resultCode) + "]")
                Return resultCode
            End If

            '対応依頼通知 移行と削除
            resultCode = bizClass.VisitDealNotice(deleteDate)
            Logger.Info("MC3100301 Execute_End Ret[" + CStr(resultCode) + "]")
            Return resultCode
        End Function
#End Region

    End Class

End Namespace

