'━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
'MC3100301BusinessLogic.vb
'──────────────────────────────────
'機能： 来店実績データ退避バッチ
'補足： 
'更新： 2020/03/06 NSK  s.natsume TKM Change request development for Next Gen e-CRB (CR060) $01
'──────────────────────────────────
'━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
'MC3100301BusinessLogic.vb
'──────────────────────────────────
'機能： 来店実績データ退避バッチ
'補足： 
'更新： 2020/03/06 NSK  s.natsume TKM Change request development for Next Gen e-CRB (CR060) $01
'──────────────────────────────────

Imports Toyota.eCRB.SystemFrameworks.Batch
Imports Toyota.eCRB.SystemFrameworks.Core
Imports Toyota.eCRB.SystemFrameworks.Core.iCROP.BizLogic
Imports Toyota.eCRB.SystemFrameworks.Core.iCROP.DataAccess
Imports Toyota.eCRB.Visit.VisitResult.DataAccess.MC3100301.MC3100301DataSetTableAdapters
Imports System.Globalization
Imports System.Text

''' <summary>
''' 来店実績データ退避バッチ ビジネスロジック層
''' </summary>
''' <remarks></remarks>
Public Class MC3100301BusinessLogic
    Inherits BaseBusinessComponent
    Implements IMC3100301BusinessLogic

#Region "定数"

    ''' <summary>
    ''' 機能ID
    ''' </summary>
    ''' <remarks></remarks>
    Private Const AppId As String = "MC3100301"

#Region "メッセージID"

    ''' <summary>
    ''' 正常終了(0)
    ''' </summary>
    ''' <remarks>保持期間が未設定</remarks>
    Private Const MessageIdSuccess As Integer = 0

    ''' <summary>
    ''' エラーコード(501)
    ''' </summary>
    ''' <remarks>保持期間が未設定</remarks>
    Private Const MessageIdNullOrEmpty As Integer = 501

    ''' <summary>
    ''' エラーコード(502)
    ''' </summary>
    ''' <remarks>保持期間が整数でない</remarks>
    Private Const MessageIdNotNumeric As Integer = 502

    ''' <summary>
    ''' エラーコード(503)
    ''' </summary>
    ''' <remarks>保持期間が0以下</remarks>
    Private Const MessageIdLessZero As Integer = 503

    ''' <summary>
    ''' エラーコード(504)
    ''' </summary>
    ''' <remarks>来店車両実績の退避処理失敗</remarks>
    Private Const MessageIdVisitVehicleFailed As Integer = 504

    ''' <summary>
    ''' エラーコード(505)
    ''' </summary>
    ''' <remarks>セールス来店実績の退避処理失敗</remarks>
    Private Const MessageIdVisitSalesFailed As Integer = 505

    ''' <summary>
    ''' エラーコード(506)
    ''' </summary>
    ''' <remarks>対応依頼通知の退避処理失敗</remarks>
    Private Const MessageIdVisitNoticeFailed As Integer = 506

    ''' <summary>
    ''' 退避期間
    ''' </summary>
    ''' <remarks>退避期間を取得する際のPARAMNAME</remarks>
    Private Const PastDays As String = "PAST_DAYS"

#End Region

#Region "バッチ終了コード"

    ''' <summary>
    ''' 異常終了
    ''' </summary>
    ''' <remarks></remarks>
    Private Const Failed As Integer = 10
#End Region

#End Region

#Region "来店実績退避日付取得"

    ''' <summary>
    ''' 来店実績退避日付取得
    ''' </summary>
    ''' <param name="messageId">メッセージID</param>
    ''' <returns>削除日</returns>
    ''' <remarks></remarks>
    ''' <history>
    ''' 2019/11/12 NSK 鈴木 (トライ店システム評価)次世代セールス基盤：ログ出力機能における保守性向上検証
    ''' </history>
    Public Function SaveVisitResult(ByRef messageId As Integer) As Date

        Logger.Info("SaveVisitResult_Start")

        '退避期間の取得
        Logger.Info("SaveVisitResult_001 Call_Start GetSystemEnvSetting param[" & PastDays & "]")
        Dim sysEnv As SystemEnvSetting = New SystemEnvSetting
        Dim pastaTargetDays As SystemEnvSettingDataSet.SYSTEMENVSETTINGRow = sysEnv.GetSystemEnvSetting(PastDays)

        Dim pastDaysLog As New StringBuilder
        With pastDaysLog
            .Append("SaveVisitResult_001 Call_End GetSystemEnvSetting Ret[")
            .Append(pastaTargetDays)
            .Append("]")
        End With
        Logger.Info(pastDaysLog.ToString)

        '保持期間の値チェック
        If pastaTargetDays Is Nothing Then

            '値が未設定
            Logger.Info("SaveVisitResult_002 IsNullOrEmpty")
            Logger.Info("MessageId:" & MessageIdNullOrEmpty & " SystemEnv is nothing")
            Logger.Info("SaveVisitResult_End Ret[" & CStr(Failed) & "]")
            messageId = Failed
            Return Nothing
        ElseIf Not IsNumeric(pastaTargetDays.PARAMVALUE) Then

            '値が整数でない
            Logger.Info("SaveVisitResult_003 NotIsNumeric")
            Logger.Info("MessageId:" & MessageIdNotNumeric & " SystemEnv is not nNumeric")
            Logger.Info("SaveVisitResult_End Ret[" & CStr(Failed) & "]")
            messageId = Failed
            Return Nothing
        ElseIf CLng(pastaTargetDays.PARAMVALUE) <= 0 Then

            '値が0以下
            Logger.Info("SaveVisitResult_004 MessageIdLessZero")
            Logger.Info("MessageId:" & MessageIdLessZero & "  SystemEnv <= zero")
            Logger.Info("SaveVisitResult_End Ret[" & CStr(Failed) & "]")
            messageId = Failed
            Return Nothing
        End If

        '日付管理機能からシステム日時を取得
        Logger.Info("SaveVisitResult_005 Call_Start Now")
        Dim today As DateTime = DateTimeFunc.Now()
        Logger.Info("SaveVisitResult_005 Call_End Now Ret[" & CStr(today) & "]")

        '削除日付(過去の日付を取得したい為、マイナス)
        Logger.Info("SaveVisitResult_006 Call_Start DateAdd ")
        Logger.Info("param1[" & CStr(DateInterval.Day) & "]")
        Logger.Info("param2[" & pastaTargetDays.PARAMVALUE & "]")
        Logger.Info("param3[" & CStr(today) & "]")
        Dim pastDate As Date = DateAdd(DateInterval.Day, -CLng(pastaTargetDays.PARAMVALUE), today)
        Logger.Info("SaveVisitResult_006 Call_End DateAdd Ret[" & CStr(pastDate) & "]")

        '削除日を返す
        Logger.Info("SaveVisitResult_End Ret[" & CStr(pastDate) & "]")
        Return pastDate
    End Function

#End Region

#Region "来店車両実績処理"

    ''' <summary>
    '''  来店車両実績処理
    ''' </summary>
    ''' <param name="delDate">過去データと判断する日付</param>
    ''' <returns>処理結果</returns>
    ''' <remarks></remarks>
    ''' <history>
    ''' 2019/11/12 NSK 鈴木 (トライ店システム評価)次世代セールス基盤：ログ出力機能における保守性向上検証
    ''' </history>
    <EnableCommit()>
    Public Function VisitVehicle(ByVal delDate As Date) As Integer Implements IMC3100301BusinessLogic.VisitVehicle

        Logger.Info("VisitVehicle_Start param[" & CStr(delDate) & "]")
        Using adapter As New MC3100301DataSetTableAdapter

            Try

                '来店車両実績移行
                adapter.CopyVisitVehicle(delDate)

                '来店車両実績削除
                adapter.DeleteVisitVehicle(delDate)

            Catch ex As OracleExceptionEx

                Me.Rollback = True
                Logger.Error("ErrorID:" & CStr(ex.Number) & " Exception:" & ex.Message)
                Logger.Error("MessageId:" & MessageIdVisitVehicleFailed & " VisitVehicleFailed")
                Logger.Error("VisitVehicle_End Ret[" & CStr(Failed) & "]")
                Return Failed
            End Try
        End Using
        '正常値返却
        Logger.Info("VisitVehicle_End Ret[" & CStr(MessageIdSuccess) & "]")
        Return MessageIdSuccess
    End Function
#End Region

#Region "セールス来店実績処理"

    ''' <summary>
    ''' セールス来店実績移行
    ''' </summary>
    ''' <param name="delDate">過去データと判断する日付</param>
    ''' <returns>処理結果</returns>
    ''' <remarks></remarks>
    ''' <history>
    ''' 2019/11/12 NSK 鈴木 (トライ店システム評価)次世代セールス基盤：ログ出力機能における保守性向上検証
    ''' </history>
    <EnableCommit()> _
    Public Function VisitSales(ByVal delDate As Date) As Integer Implements IMC3100301BusinessLogic.VisitSales

        Logger.Info("VisitSales_Start param[" & CStr(delDate) & "]")

        Using adapter As New MC3100301DataSetTableAdapter

            Try

                'セールス来店実績移行
                adapter.CopyVisitSales(delDate)
                '$01 start TKM Change request development for Next Gen e-CRB (CR060)
                'セールス来店実績ローカル移行
                adapter.CopyLcVisitSales(delDate)
                '$01 end TKM Change request development for Next Gen e-CRB (CR060)

                'セールス来店実績削除
                adapter.DeleteVisitSales(delDate)

                '$01 start TKM Change request development for Next Gen e-CRB (CR060)
                'セールス来店実績ローカル削除
                adapter.DeleteLcVisitSales(delDate)
                '$01 end TKM Change request development for Next Gen e-CRB (CR060)

            Catch ex As OracleExceptionEx

                Me.Rollback = True
                Logger.Error("ErrorID:" & CStr(ex.Number) & " Exception:" & ex.Message)
                Logger.Error("MessageId:" & MessageIdVisitSalesFailed & " VisitSalesFailed")
                Logger.Error("VisitSales_End Ret[" & CStr(Failed) & "]")
                Return Failed
            End Try
        End Using

        '正常値返却
        Logger.Info("VisitSales_End Ret[" & CStr(MessageIdSuccess) & "]")
        Return MessageIdSuccess
    End Function
#End Region

#Region "対応依頼通知処理"

    ''' <summary>
    ''' 対応依頼通知処理
    ''' </summary>
    ''' <param name="delDate">過去データと判断する日付</param>
    ''' <returns>処理結果</returns>
    ''' <remarks></remarks>
    ''' <history>
    ''' 2019/11/12 NSK 鈴木 (トライ店システム評価)次世代セールス基盤：ログ出力機能における保守性向上検証
    ''' </history>
    <EnableCommit()> _
    Public Function VisitDealNotice(ByVal delDate As Date) As Integer Implements IMC3100301BusinessLogic.VisitDealNotice

        Logger.Info("VisitDealNotice_Start param[" & CStr(delDate) & "]")

        Using adapter As New MC3100301DataSetTableAdapter

            Try

                '対応依頼通知移行
                adapter.CopyVisitDealNotice(delDate)

                '対応依頼通知削除
                adapter.DeleteVisitDealNotice(delDate)

            Catch ex As OracleExceptionEx

                Me.Rollback = True
                Logger.Error("ErrorID:" & CStr(ex.Number) & " Exception:" & ex.Message)
                Logger.Error("MessageId:" & MessageIdVisitNoticeFailed & " VisitNoticeFailed")
                Logger.Error("VisitDealNotice_End Ret[" & CStr(Failed) & "]")
                Return Failed
            End Try
        End Using

        '正常値返却
        Logger.Info("VisitDealNotice_End Ret[" & CStr(MessageIdSuccess) & "]")
        Return MessageIdSuccess
    End Function
#End Region

End Class