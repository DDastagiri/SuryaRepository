'━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
'SC3100103BusinessLogic.vb
'──────────────────────────────────
'機能： スタンバイスタッフ並び順変更
'補足： 
'作成： 2012/08/17 TMEJ m.okamura
'更新： 2019/11/12 NSK 鈴木 (トライ店システム評価)次世代セールス基盤：ログ出力機能における保守性向上検証
'──────────────────────────────────

Imports System.Globalization
Imports Toyota.eCRB.SystemFrameworks.Core
Imports Toyota.eCRB.Visit.ReceptionistMain.DataAccess.SC3100103DataSet
Imports Toyota.eCRB.Visit.ReceptionistMain.DataAccess.SC3100103DataSetTableAdapters

''' <summary>
''' SC3100103
''' スタンバイスタッフ並び順変更 ビジネスロジッククラス
''' </summary>
''' <remarks></remarks>
Public Class SC3100103BusinessLogic
    Inherits BaseBusinessComponent

#Region "定数"

    ''' <summary>
    ''' 翌日
    ''' </summary>
    ''' <remarks></remarks>
    Private Const NextDay As Double = 1.0

    ''' <summary>
    ''' 1ミリ秒前
    ''' </summary>
    ''' <remarks></remarks>
    Private Const BeforMillisecond As Double = -1.0

    ''' <summary>
    ''' 更新ID
    ''' </summary>
    ''' <remarks></remarks>
    Private Const UpdateId As String = "SC3100103"

    ''' <summary>
    ''' オラクルエラー：タイムアウト
    ''' </summary>
    ''' <remarks>試乗車ステータスの更新時のエラー</remarks>
    Private Const OracleException2049 As Integer = 2049

#Region "メッセージID"

    ''' <summary>
    ''' 正常終了のエラーメッセージID
    ''' </summary>
    ''' <remarks></remarks>
    Private Const MessageIdSuccess As Integer = 0

    ''' <summary>
    ''' タイムアウトが発生した場合のエラーメッセージID
    ''' </summary>
    ''' <remarks>タイムアウトが発生した場合のエラー</remarks>
    Private Const MessageIdFailed As Integer = 901

#End Region

#End Region

#Region "スタッフ情報の取得"

    ''' <summary>
    ''' スタッフ情報の取得
    ''' </summary>
    ''' <param name="dealerCode">販売店コード</param>
    ''' <param name="storeCode">店舗コード</param>
    ''' <param name="nowDate">現在日時</param>
    ''' <returns>スタッフ情報の取得データテーブル</returns>
    ''' <remarks></remarks>
    Public Function GetTargetStaff(ByVal dealerCode As String, _
                                   ByVal storeCode As String, _
                                   ByVal nowDate As Date) _
                                   As SC3100103TargetStaffDataTable

        Logger.Info("GetTargetStaff_Start Param[" & dealerCode & ", " & storeCode & ", " & nowDate & "]")

        '開始日時(当日の0:00:00)を格納
        Dim startDate As Date = nowDate.Date

        '終了日時(当日の23:59:59)を格納
        Dim nextDate As Date = startDate.AddDays(NextDay)
        Dim endDate As Date = nextDate.AddSeconds(BeforMillisecond)
        nextDate = Nothing

        'スタッフ情報の取得データテーブル
        Dim targetStaffDataTable As SC3100103TargetStaffDataTable = Nothing

        Using dataAdapter As New SC3100103TableAdapter

            '来店状況の取得
            targetStaffDataTable = dataAdapter.GetTargetStaff(dealerCode, _
                                                              storeCode, _
                                                              startDate, _
                                                              endDate)

        End Using

        'スタッフ情報の取得データテーブルを返す
        Logger.Info("GetTargetStaff_End Ret[" & targetStaffDataTable.ToString & "]")
        Return targetStaffDataTable

    End Function

#End Region

#Region "スタッフ並び順の登録"

    ''' <summary>
    ''' スタッフ並び順の登録
    ''' </summary>
    ''' <param name="dealerCode">販売店コード</param>
    ''' <param name="storeCode">店舗コード</param>
    ''' <param name="account">アカウント</param>
    ''' <param name="staffSortDataTable">スタッフ並び順データテーブル</param>
    ''' <remarks></remarks>
    ''' <history>
    ''' 2019/11/12 NSK 鈴木 (トライ店システム評価)次世代セールス基盤：ログ出力機能における保守性向上検証
    ''' </history>
    Public Function RegistStaffSort(ByVal dealerCode As String, _
                                    ByVal storeCode As String, _
                                    ByVal account As String, _
                                    ByVal staffSortDataTable As SC3100103StandByStaffSortDataTable) _
                                    As Integer

        Logger.Info("RegistStaffSort_Start Param[" & dealerCode & ", " & storeCode & "]")

        Try

            Using dataAdapter As New SC3100103TableAdapter

                'チェック用変数
                Dim dataExist As Boolean = False

                'スタッフ並び順の削除
                dataExist = dataAdapter.DeleteStaffSort(dealerCode, storeCode)

                '削除できたかチェック
                If dataExist = False Then

                    Logger.Error("RegistStaffSort_001 DeleteFalse")
                    Me.Rollback = True
                    
                    Logger.Error("RegistStaffSort_End Ret[ " + MessageIdFailed.ToString(CultureInfo.InvariantCulture()) + " ]")
                    Return MessageIdFailed

                End If

                Logger.Info("RegistStaffSort_002 DeleteTrue")

                'スタッフ並び順データ件数分処理
                For Each dataRow As SC3100103StandByStaffSortRow In staffSortDataTable

                    'スタッフ並び順の登録
                    dataExist = dataAdapter.InsertStaffSort(dataRow, account, UpdateId)

                    '登録できたかチェック
                    If dataExist = False Then
                        
                        Logger.Error("RegistStaffSort_003 InsertFalse")
                        Me.Rollback = True
                        
                        Logger.Error("RegistStaffSort_End Ret[ " + MessageIdFailed.ToString(CultureInfo.InvariantCulture()) + " ]")
                        Return MessageIdFailed

                    End If

                Next

                Logger.Info("RegistStaffSort_004 InsertTrue")

            End Using

        Catch ex As OracleExceptionEx

            ' データベースの操作中に例外が発生した場合
            Logger.Error("RegistStaffSort_003 " & "Catch OracleExceptionEx")
            Logger.Error("ErrorID:" & CStr(ex.Number) & ", Exception:" & ex.Message)

            If ex.Number = OracleException2049 Then

                'DBタイムアウトエラー時
                Me.Rollback = True
                Logger.Error("RegistStaffSort_004 " & "ex.Number = MessageId_OracleException2049")
                Logger.Error("RegistStaffSort ResultId:" & CStr(OracleException2049))
                Logger.Error("RegistStaffSort_End Ret[ " + MessageIdFailed.ToString(CultureInfo.InvariantCulture()) + " ]")
                Return MessageIdFailed

            Else

                '上記以外のエラーは基盤側で制御
                Me.Rollback = True
                Logger.Error("RegistStaffSort_005 " & "ex.Number <> MessageId_OracleException2049")
                Logger.Error("RegistStaffSort_End Ret[Throw OracleExceptionEx]")
                Throw

            End If

        End Try

        Logger.Info("RegistStaffSort_End Ret[" & MessageIdSuccess & "]")
        Return MessageIdSuccess

    End Function

#End Region

End Class
