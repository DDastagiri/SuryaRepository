'━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━'
'MC3040904BusinessLogic.vb                                                 '            '
'─────────────────────────────────────'
'機能： ステータス変更                                                   　'
'補足：                                                                    '
'作成： 2012/02/16 TCS 小林                                                '
'更新： 2013/02/27 TCS 山田 【A.STEP2】新車タブレット受付画面の管理指標変更対応
'更新： 2013/06/30 TCS 山田 2013/10対応版 既存流用
'─────────────────────────────────────'

Imports System
Imports System.IO
Imports System.Xml
Imports System.Text
Imports Toyota.eCRB.SystemFrameworks.Batch
Imports Toyota.eCRB.SystemFrameworks.Core
Imports Toyota.eCRB.Tool.DataMaintenance.Batch.DataAccess
Imports System.Globalization

Public Class MC3040904BusinessLogic
    Inherits BaseBusinessComponent

#Region "メッセージ文言"

    Private message003 As String = BatchWordUtility.GetWord(903)
    Private message004 As String = BatchWordUtility.GetWord(904)
    Private message005 As String = BatchWordUtility.GetWord(905)
    Private message006 As String = BatchWordUtility.GetWord(906)
    Private message007 As String = BatchWordUtility.GetWord(907)
    Private message008 As String = BatchWordUtility.GetWord(908)
    Private message009 As String = BatchWordUtility.GetWord(909)
    ' 2013/02/27 TCS 山田 【A.STEP2】新車タブレット受付画面の管理指標変更対応 START
    Private message010 As String = BatchWordUtility.GetWord(910)
    Private message011 As String = BatchWordUtility.GetWord(911)
    ' 2013/02/27 TCS 山田 【A.STEP2】新車タブレット受付画面の管理指標変更対応 END

    'Private Const message003 As String = "MC3040904 UpdateStatusInfo_Start Param[{0}]"
    'Private Const message004 As String = "MC3040904 UpdateStatusInfo_End Ret[{0}]"
    'Private Const message005 As String = "MC3040904 UpdateVisitStatus_SUCCESS Count[{0}]"
    'Private Const message006 As String = "MC3040904 UpdateVisitStatus_ERROR ErrorID[{0}]"
    'Private Const message007 As String = "MC3040904 UpdatePresenceStatus_SUCCESS Count[{0}]"
    'Private Const message008 As String = "MC3040904 UpdatePresenceStatus_ERROR ErrorID[{0}]"
    'Private Const message009 As String = "MC3040904 ErrorID:{0} Exception:{1}"

#End Region

#Region "バッチ終了コード"

    ''' <summary>
    ''' 正常終了
    ''' </summary>
    ''' <remarks></remarks>
    Private Const Success As Integer = 0

    ''' <summary>
    ''' 異常終了(来店実績ステータス更新失敗)
    ''' </summary>
    ''' <remarks></remarks>
    Private Const Failed_UpdateVisitStatus As Integer = 11

    ' 2013/02/27 TCS 山田 【A.STEP2】新車タブレット受付画面の管理指標変更対応 START
    ''' <summary>
    ''' 異常終了(来店実績ステータス(納車作業中)更新失敗)
    ''' </summary>
    ''' <remarks></remarks>
    Private Const Failed_UpdateVisitStatusDelivery As Integer = 13
    ' 2013/02/27 TCS 山田 【A.STEP2】新車タブレット受付画面の管理指標変更対応 END

    ''' <summary>
    ''' 異常終了(スタッフ在席状態更新失敗)
    ''' </summary>
    ''' <remarks></remarks>
    Private Const Failed_UpdatePresenceStatus As Integer = 12

#End Region

#Region "メイン処理"

    ''' <summary>
    ''' ステータス情報更新
    ''' </summary>
    ''' <returns>処理結果(処理成功:0,エラー:10)</returns>
    ''' <remarks></remarks>
    <EnableCommit()>
    Public Function UpdateStatusInfo() As Integer

        '開始ログ出力
        Logger.Info(String.Format(CultureInfo.InvariantCulture, message003, String.Empty))

        Dim resultCode As Integer

        Using adapter As New MC3040904TableAdapter

            '2013/06/30 TCS 山田 2013/10対応版 既存流用 START
            Try
                'セールス来店実績ロック取得
                adapter.GetVisitSalesLock()

            Catch oex As OracleExceptionEx

                '来店実績ステータス更新処理失敗
                resultCode = Failed_UpdateVisitStatus

                'エラーログ出力
                Logger.Error(String.Format(CultureInfo.InvariantCulture, message009, CStr(oex.Number), CStr(oex.Message)))
                Logger.Error(String.Format(CultureInfo.InvariantCulture, message006, CStr(oex.Number)))

                '終了ログ出力
                Logger.Info(String.Format(CultureInfo.InvariantCulture, message004, CStr(resultCode)))

                Throw

            End Try

            Try
                'ユーザマスタロック取得
                adapter.GetUsersLock()

            Catch oex As OracleExceptionEx

                'スタッフ在席状態ステータス更新処理失敗
                resultCode = Failed_UpdatePresenceStatus

                'エラーログ出力
                Logger.Error(String.Format(CultureInfo.InvariantCulture, message009, CStr(oex.Number), CStr(oex.Message)))
                Logger.Error(String.Format(CultureInfo.InvariantCulture, message008, CStr(oex.Number)))

                '終了ログ出力
                Logger.Info(String.Format(CultureInfo.InvariantCulture, message004, CStr(resultCode)))

                Throw

            End Try
            '2013/06/30 TCS 山田 2013/10対応版 既存流用 END

            Try
                '来店実績ステータス更新処理
                Dim cntVisitStatus As Integer = adapter.UpdateVisitStatus()

                If cntVisitStatus >= 0 Then

                    '来店実績ステータス更新処理成功
                    resultCode = Success

                    '処理件数ログ出力
                    Logger.Info(String.Format(CultureInfo.InvariantCulture, message005, CStr(cntVisitStatus)))

                Else
                    '来店実績ステータス更新処理失敗
                    resultCode = Failed_UpdateVisitStatus

                    'エラーログ出力
                    Logger.Error(String.Format(CultureInfo.InvariantCulture, message006, CStr(cntVisitStatus)))

                    '終了ログ出力
                    Logger.Info(String.Format(CultureInfo.InvariantCulture, message004, CStr(resultCode)))

                    Return resultCode

                End If

            Catch oex As OracleExceptionEx

                '来店実績ステータス更新処理失敗
                resultCode = Failed_UpdateVisitStatus
                Me.Rollback = True

                'エラーログ出力
                Logger.Error(String.Format(CultureInfo.InvariantCulture, message009, CStr(oex.Number), CStr(oex.Message)))
                Logger.Error(String.Format(CultureInfo.InvariantCulture, message006, CStr(oex.Number)))

                '終了ログ出力
                Logger.Info(String.Format(CultureInfo.InvariantCulture, message004, CStr(resultCode)))

                Throw

            End Try

            ' 2013/02/27 TCS 山田 【A.STEP2】新車タブレット受付画面の管理指標変更対応 START
            Try
                '来店実績ステータス(納車作業中)更新処理
                Dim cntVisitStatusDelivery As Integer = adapter.UpdateVisitStatusDelivery()

                If cntVisitStatusDelivery >= 0 Then

                    '来店実績ステータス(納車作業中)更新処理成功
                    resultCode = Success

                    '処理件数ログ出力
                    Logger.Info(String.Format(CultureInfo.InvariantCulture, message010, CStr(cntVisitStatusDelivery)))

                Else
                    '来店実績ステータス(納車作業中)更新処理失敗
                    resultCode = Failed_UpdateVisitStatusDelivery

                    'エラーログ出力
                    Logger.Error(String.Format(CultureInfo.InvariantCulture, message011, CStr(cntVisitStatusDelivery)))

                    '終了ログ出力
                    Logger.Info(String.Format(CultureInfo.InvariantCulture, message004, CStr(resultCode)))

                    Return resultCode

                End If

            Catch oex As OracleExceptionEx

                '来店実績ステータス(納車作業中)更新処理失敗
                resultCode = Failed_UpdateVisitStatusDelivery
                Me.Rollback = True

                'エラーログ出力
                Logger.Error(String.Format(CultureInfo.InvariantCulture, message009, CStr(oex.Number), CStr(oex.Message)))
                Logger.Error(String.Format(CultureInfo.InvariantCulture, message011, CStr(oex.Number)))

                '終了ログ出力
                Logger.Info(String.Format(CultureInfo.InvariantCulture, message004, CStr(resultCode)))

                Throw

            End Try
            ' 2013/02/27 TCS 山田 【A.STEP2】新車タブレット受付画面の管理指標変更対応 END

            Try
                'スタッフ在席状態ステータス更新処理
                Dim cntPresenceStatus As Integer = adapter.UpdatePresenceStatus()

                If cntPresenceStatus >= 0 Then
                    'スタッフ在席状態ステータス更新処理成功
                    resultCode = Success

                    '処理件数ログ出力
                    Logger.Info(String.Format(CultureInfo.InvariantCulture, message007, cntPresenceStatus))
                Else
                    'スタッフ在席状態ステータス更新処理失敗
                    resultCode = Failed_UpdatePresenceStatus

                    'エラーログ出力
                    Logger.Error(String.Format(CultureInfo.InvariantCulture, message008, CStr(cntPresenceStatus)))

                    '終了ログ出力
                    Logger.Info(String.Format(CultureInfo.InvariantCulture, message004, CStr(resultCode)))

                    Return resultCode
                End If

            Catch oex As OracleExceptionEx

                'スタッフ在席状態ステータス更新処理失敗
                resultCode = Failed_UpdatePresenceStatus
                Me.Rollback = True

                'エラーログ出力
                Me.Rollback = True
                Logger.Error(String.Format(CultureInfo.InvariantCulture, message009, CStr(oex.Number), CStr(oex.Message)))
                Logger.Error(String.Format(CultureInfo.InvariantCulture, message008, CStr(oex.Number)))

                '終了ログ出力
                Logger.Info(String.Format(CultureInfo.InvariantCulture, message004, CStr(resultCode)))

                Throw

            End Try

        End Using

        '終了ログ出力
        Logger.Info(String.Format(CultureInfo.InvariantCulture, message004, CStr(resultCode)))

        Return resultCode

    End Function

#End Region

End Class
