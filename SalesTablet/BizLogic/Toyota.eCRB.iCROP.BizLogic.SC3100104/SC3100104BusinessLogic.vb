'━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
'SC3100104BusinessLogic.vb
'──────────────────────────────────
'機能： お客様チップ作成
'補足： 
'作成： 2013/09/04 TMEJ m.asano
'更新： 2019/11/12 NSK 鈴木 (トライ店システム評価)次世代セールス基盤：ログ出力機能における保守性向上検証
'──────────────────────────────────

Imports System.Text
Imports Toyota.eCRB.SystemFrameworks.Core
Imports Toyota.eCRB.SystemFrameworks.Core.iCROP.BizLogic
Imports Toyota.eCRB.SystemFrameworks.Core.iCROP.DataAccess
Imports Toyota.eCRB.Visit.Api.BizLogic
Imports Toyota.eCRB.Visit.Api.DataAccess.VisitUtilityDataSet
Imports Toyota.eCRB.Visit.Api.DataAccess.VisitUtilityDataSetTableAdapters
Imports Toyota.eCRB.Visit.Api.BizLogic.VisitUtilityBusinessLogic
Imports Toyota.eCRB.Visit.Api.DataAccess.VisitReceptionDataSet
Imports Toyota.eCRB.Visit.Api.DataAccess.VisitReceptionDataSetTableAdapters

''' <summary>
''' SC3100104
''' お客様チップ作成 ビジネスロジッククラス
''' </summary>
''' <remarks></remarks>
Public Class SC3100104BusinessLogic
    Inherits BaseBusinessComponent
    Implements ISC3100104BusinessLogic

#Region "メッセージID"

    ''' <summary>
    ''' メッセージID:成功
    ''' </summary>
    ''' <remarks></remarks>
    Private Const MessageIdSuccess As Integer = 0

    ''' <summary>
    ''' メッセージID:エラー[DBタイムアウト]
    ''' </summary>
    ''' <remarks></remarks>
    Private Const MessageIdErrorDbTimeOut As Integer = 900

    ''' <summary>
    ''' 文言ID：苦情文字
    ''' </summary>
    ''' <remarks></remarks>
    Private Const ClameWord As Integer = 13
#End Region

#Region "その他"

    ''' <summary>
    ''' オラクルエラーコード:タイムアウト
    ''' </summary>
    ''' <remarks></remarks>
    Private Const ErrorCodeOra2049 As Integer = 2049

    ''' <summary>
    ''' スタッフステータス：スタンバイ
    ''' </summary>
    ''' <remarks></remarks>
    Private Const StuffStatusStandby As String = "1"

    ''' <summary>
    ''' スタッフステータス：オフライン
    ''' </summary>
    ''' <remarks></remarks>
    Private Const StuffStatusOffline As String = "4"

    ''' <summary>
    ''' 送信タイプ：顧客担当SS
    ''' </summary>
    ''' <remarks></remarks>
    Private Const SendTypeCsutSs As Integer = 1

    ''' <summary>
    ''' 送信タイプ：セールスマネージャー
    ''' </summary>
    ''' <remarks></remarks>
    Private Const SendTypeSsm As Integer = 2

    ''' <summary>
    ''' 送信タイプ：受付係
    ''' </summary>
    ''' <remarks></remarks>
    Private Const SendTypeSlr As Integer = 3

    ''' <summary>
    ''' 権限コード：セールスマネージャー
    ''' </summary>
    ''' <remarks></remarks>
    Private Const OperationCodeSsm As Integer = 7

    ''' <summary>
    ''' 権限コード：セールススタッフ
    ''' </summary>
    ''' <remarks></remarks>
    Private Const OperationCodeSs As Integer = 8

    ''' <summary>
    ''' 権限コード：受付係
    ''' </summary>
    ''' <remarks></remarks>
    Private Const OperationCodeSlr As Integer = 51

    ''' <summary>
    ''' 来店実績ステータス:フリー
    ''' </summary>
    ''' <remarks></remarks>
    Private Const VisitStatusFree As String = "01"

    ''' <summary>
    ''' 来店実績ステータス:調整中
    ''' </summary>
    ''' <remarks></remarks>
    Private Const VisitStatusAdjust As String = "03"
#End Region

#Region "お客様チップ作成"

    ''' <summary>
    ''' お客様チップ作成
    ''' </summary>
    ''' <param name="insertRow">セールス来店実績データロウ</param>
    ''' <param name="isComplaint">苦情有無フラグ</param>
    ''' <returns>メッセージID</returns>
    ''' <remarks></remarks>
    ''' <history>
    ''' 2019/11/12 NSK 鈴木 (トライ店システム評価)次世代セールス基盤：ログ出力機能における保守性向上検証
    ''' </history>
    <EnableCommit()>
    Function CreateCustomerChip(ByVal insertRow As VisitReceptionVisitSalesRow, _
                                ByVal isComplaint As Boolean) As Integer _
                                Implements ISC3100104BusinessLogic.CreateCustomerChip

        Logger.Info("CreateCustomerChip_Start Param[insertRow=" & (insertRow IsNot Nothing) & _
                    ",isComplaint= " & isComplaint & "]")
        Dim messageId As Integer = MessageIdSuccess
        Dim visitReceptionBiz As New VisitReceptionBusinessLogic
        Try
            messageId = visitReceptionBiz.CreateCustomerChip(insertRow, isComplaint)

            ' DBタイムアウトエラー時
            If messageId = MessageIdErrorDbTimeOut Then
                'ロールバックを行う。
                Me.Rollback = True
                Return MessageIdErrorDbTimeOut
            End If

        Catch oraEx As OracleExceptionEx

            If oraEx.Number = ErrorCodeOra2049 Then

                'ロールバックを行う。
                Me.Rollback = True

                'ログ出力
                Logger.Error(CStr(MessageIdErrorDbTimeOut), oraEx)

                'DBタイムアウトエラー時
                Logger.Error("SendOrgOrNewCustomer_End Ret[" & MessageIdErrorDbTimeOut & "]")
                Return MessageIdErrorDbTimeOut
            Else
                '上記以外のエラーは基盤側で制御
                Throw
            End If

        End Try

        Logger.Info("CreateCustomerChip_End Ret[messageId=" & messageId & "]")
        Return messageId
    End Function

#End Region

    ''' <summary>
    ''' Push通知処理
    ''' </summary>
    ''' <param name="VisitSalesDataRow">セールス来店実績データロウ</param>
    ''' <param name="isClaimeInfo">苦情有無フラグ</param>
    ''' <remarks></remarks>
    Public Sub PushExecution(ByVal visitSalesDataRow As VisitReceptionVisitSalesRow, _
                              ByVal isClaimeInfo As Boolean)

        Logger.Info("PushExecution_Start")

        Dim visit As New VisitReceptionBusinessLogic
        visit.SendPushSales(visitSalesDataRow, isClaimeInfo)

        Logger.Info("PushExecution_End")

    End Sub

End Class
