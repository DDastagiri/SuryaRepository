'━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
'MC3040401.vb
'──────────────────────────────────
'機能： CalDAV連携バッチ
'補足： 
'作成： 2011/12/01 KN   梅村
'更新： 2014/06/05 TMEJ y.gotoh 受注後フォロー機能開発 $01
'更新： 2019/04/23 NSK a.ohhira TR-SLT-FTMS-20181219-001 担当スタッフが未割り当てに変更になった場合の考慮追加、行更新日時の考慮追加 $04
'──────────────────────────────────

Imports Toyota.eCRB.SystemFrameworks.Batch
Imports Toyota.eCRB.iCROP.BizLogic.MC3040401
Imports Toyota.eCRB.SystemFrameworks.Core
Imports System.Globalization


Public Class MC3040401
    Implements IBatch

#Region "定数"
    ''' <summary>
    ''' 機能ID
    ''' </summary>
    ''' <remarks></remarks>
    Private Const C_SYSTEM = "MC3040401"

    '$01 受注後フォロー機能開発 START
    ''' <summary>
    ''' 終了コード(0)
    ''' </summary>
    ''' <remarks>正常終了</remarks>
    Private Const C_MESSAGE_0 As Integer = 0

    ''' <summary>
    ''' エラーコード(100)
    ''' </summary>
    ''' <remarks>処理中にException発生</remarks>
    Private Const C_MESSAGE_100 As Integer = 100

    ''' <summary>
    ''' エラーコード(999)
    ''' </summary>
    ''' <remarks>WebExceptionエラー</remarks>
    Private Const C_MESSAGE_999 As Integer = 999

    '$01 受注後フォロー機能開発 END
#End Region

    Public Function Execute(ByVal args() As String) As Integer Implements SystemFrameworks.Batch.IBatch.Execute

        '$01 受注後フォロー機能開発 START
        '開始ログ
        Logger.Info(C_SYSTEM & " Batch Start")

        Dim resultCode As Integer = 0
        '$04 TR-SLT-FTMS-20181219-001 （トライ店システム評価）次世代e-CRBにおけるカレンダー連携機能の仕様変更 START
        Dim resultCodeGetBatchStartDateTime As Integer = 0
        Dim resultCodeGetLastProcDate As Integer = 0
        Dim resultCodeDeleteUnregistScheduleInfo As Integer = 0
        '$04 TR-SLT-FTMS-20181219-001 （トライ店システム評価）次世代e-CRBにおけるカレンダー連携機能の仕様変更 END
        Dim resultCodeBefore As Integer = 0
        Dim resultCodeAfter As Integer = 0
        '$04 TR-SLT-FTMS-20181219-001 （トライ店システム評価）次世代e-CRBにおけるカレンダー連携機能の仕様変更 START
        Dim batchStartDateTime As DateTime
        Dim lastProcDate As DateTime
        '$04 TR-SLT-FTMS-20181219-001 （トライ店システム評価）次世代e-CRBにおけるカレンダー連携機能の仕様変更 END

        Using bizClass As New MC3040401BussinessLogic

            '$04 TR-SLT-FTMS-20181219-001 （トライ店システム評価）次世代e-CRBにおけるカレンダー連携機能の仕様変更 START
            '現在日時の取得処理
            resultCodeGetBatchStartDateTime = bizClass.GetBatchStartDateTime(batchStartDateTime)
            
            '前回バッチ起動日時取得
            resultCodeGetLastProcDate = bizClass.GetLastProcDate(lastProcDate)

            If C_MESSAGE_100.Equals(resultCodeGetBatchStartDateTime) OrElse C_MESSAGE_100.Equals(resultCodeGetLastProcDate) Then
                resultCode = C_MESSAGE_100
                Logger.Error(String.Format(CultureInfo.InvariantCulture, "{0} Batch End : Return({1})", C_SYSTEM, resultCode))
                Return resultCode
            End If

            '未登録スケジュール情報の過去データ削除
            resultCodeDeleteUnregistScheduleInfo = bizClass.DeleteUnregistScheduleInfo(batchStartDateTime)

            If C_MESSAGE_100.Equals(resultCodeDeleteUnregistScheduleInfo) Then
                resultCode = C_MESSAGE_100
                Logger.Error(String.Format(CultureInfo.InvariantCulture, "{0} Batch End : Return({1})", C_SYSTEM, resultCode))
                Return resultCode
            End If
            '$04 TR-SLT-FTMS-20181219-001 （トライ店システム評価）次世代e-CRBにおけるカレンダー連携機能の仕様変更 END

            '来店予約、入庫予約
            '$04 TR-SLT-FTMS-20181219-001 （トライ店システム評価）次世代e-CRBにおけるカレンダー連携機能の仕様変更 START
            'resultCodeBefore = bizClass.SendScheduleInfo()
            resultCodeBefore = bizClass.SendScheduleInfo(batchStartDateTime, lastProcDate)
            '$04 TR-SLT-FTMS-20181219-001 （トライ店システム評価）次世代e-CRBにおけるカレンダー連携機能の仕様変更 END

            '受注後工程
            '$04 TR-SLT-FTMS-20181219-001 （トライ店システム評価）次世代e-CRBにおけるカレンダー連携機能の仕様変更 START
            'resultCodeAfter = bizClass.SendScheduleInfoAfterProcess()
            resultCodeAfter = bizClass.SendScheduleInfoAfterProcess(batchStartDateTime, lastProcDate)
            '$04 TR-SLT-FTMS-20181219-001 （トライ店システム評価）次世代e-CRBにおけるカレンダー連携機能の仕様変更 END

            If C_MESSAGE_100.Equals(resultCodeBefore) OrElse C_MESSAGE_100.Equals(resultCodeAfter) Then
                resultCode = C_MESSAGE_100
                Logger.Error(String.Format(CultureInfo.InvariantCulture, "{0} Batch End : Return({1})", C_SYSTEM, resultCode))

            ElseIf C_MESSAGE_999.Equals(resultCodeBefore) OrElse C_MESSAGE_999.Equals(resultCodeAfter) Then
                resultCode = C_MESSAGE_999
                Logger.Error(String.Format(CultureInfo.InvariantCulture, "{0} Batch End : Return({1})", C_SYSTEM, resultCode))

            Else
                '$04 TR-SLT-FTMS-20181219-001 （トライ店システム評価）次世代e-CRBにおけるカレンダー連携機能の仕様変更 START
                'resultCode = C_MESSAGE_0
                '前回バッチ起動日時更新
                resultCode = bizClass.UpdateLastProcDate(batchStartDateTime, lastProcDate)
                
	            If C_MESSAGE_100.Equals(resultCode) Then
	                Logger.Error(String.Format(CultureInfo.InvariantCulture, "{0} Batch End : Return({1})", C_SYSTEM, resultCode))
	                Return resultCode
	            End If
                '$04 TR-SLT-FTMS-20181219-001 （トライ店システム評価）次世代e-CRBにおけるカレンダー連携機能の仕様変更 END
                
                Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0} Batch End : Return({1})", C_SYSTEM, resultCode))
            End If

            Return resultCode

        End Using
        '$01 受注後フォロー機能開発 END
    End Function
End Class



