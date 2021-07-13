'━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
'SC3100101BusinessLogic.vb
'──────────────────────────────────
'機能： 受付メイン
'補足： 
'作成： 2011/12/12 KN t.mizumoto
'更新： 2012/08/23 TMEJ m.okamura 新車受付機能改善 $01
'更新： 2012/12/27 TMEJ t.shimamura 新車タブレットショールーム管理機能開発 $02
'更新： 2013/02/27 TMEJ t.shimamura 新車タブレット受付画面管理指標変更対応 $03
'更新： 2013/05/29 TMEJ t.shimamura 【A.STEP2】次世代e-CRB新車タブレット　新DB適応に向けた機能開発 $04
'更新： 2014/03/05 TMEJ m.asano 受注後フォロー機能開発（スタッフ活動KPI）に向けたシステム設計 $05
'更新： 2019/11/12 NSK 鈴木 (トライ店システム評価)次世代セールス基盤：ログ出力機能における保守性向上検証
'更新： 2020/02/05 NSK s.natsume TKM Change request development for Next Gen e-CRB (CR008) $06
'更新： 2020/02/05 NSK s.natsume TKM Change request development for Next Gen e-CRB (CR075) $07
'更新： 2020/03/12 NSK s.natsume TKM Change request development for Next Gen e-CRB (CR060) $08
'──────────────────────────────────

Imports System.Text
'$08 start NSK s.natsume TKM Change request development for Next Gen e-CRB (CR060)
Imports System.Text.RegularExpressions
'$08 end NSK s.natsume TKM Change request development for Next Gen e-CRB (CR060)
Imports System.Net
Imports System.IO
Imports Toyota.eCRB.SystemFrameworks.Core
Imports Toyota.eCRB.SystemFrameworks.Web
Imports Toyota.eCRB.Visit.ReceptionistMain.DataAccess
Imports Toyota.eCRB.Visit.ReceptionistMain.DataAccess.SC3100101DataSetTableAdapters
Imports Toyota.eCRB.Visit.ReceptionistMain.DataAccess.SC3100101DataSet
Imports Toyota.eCRB.SystemFrameworks.Core.iCROP.DataAccess
Imports Toyota.eCRB.SystemFrameworks.Core.iCROP.BizLogic
Imports Toyota.eCRB.Visit.Api.BizLogic
Imports Toyota.eCRB.Visit.Api.DataAccess.VisitUtilityDataSet
Imports Toyota.eCRB.Visit.Api.DataAccess.VisitReceptionDataSet
Imports Toyota.eCRB.Visit.Api.DataAccess.VisitReceptionDataSetTableAdapters

''' <summary>
''' SC3100101
''' 受付メインのビジネスロジッククラス
''' </summary>
''' <remarks></remarks>
Public Class SC3100101BusinessLogic
    Inherits BaseBusinessComponent
    Implements ISC3100101BusinessLogic

#Region "定数"

    ''' <summary>
    ''' 来店実績ステータス（フリー）
    ''' </summary>
    ''' <remarks></remarks>
    Private Const VisitStatusFree As String = "01"

    ''' <summary>
    ''' 来店実績ステータス（フリー（ブロードキャスト））
    ''' </summary>
    ''' <remarks></remarks>
    Private Const VisitStatusFreeBroadcast As String = "02"

    ''' <summary>
    ''' 来店実績ステータス（調整中）
    ''' </summary>
    ''' <remarks></remarks>
    Private Const VisitStatusAdjustment As String = "03"

    ''' <summary>
    ''' 来店実績ステータス（確定（ブロードキャスト））
    ''' </summary>
    ''' <remarks></remarks>
    Private Const VisitStatusDecisionBroadcast As String = "04"

    ''' <summary>
    ''' 来店実績ステータス（確定）
    ''' </summary>
    ''' <remarks></remarks>
    Private Const VisitStatusDecision As String = "05"

    ''' <summary>
    ''' 来店実績ステータス（待ち）
    ''' </summary>
    ''' <remarks></remarks>
    Private Const VisitStatusWating As String = "06"

    ''' <summary>
    ''' 来店実績ステータス（商談中）
    ''' </summary>
    ''' <remarks></remarks>
    Private Const VisitStatusNegotiate As String = "07"

    ''' <summary>
    ''' 来店実績ステータス（商談終了）
    ''' </summary>
    ''' <remarks></remarks>
    Private Const VisitStatusNegotiateEnd As String = "08"

    ''' <summary>
    ''' 来店実績ステータス（来店キャンセル）
    ''' </summary>
    ''' <remarks></remarks>
    Private Const VisitStatusCancel As String = "99"

    ''' <summary>
    ''' スタッフステータス（商談中）
    ''' </summary>
    ''' <remarks></remarks>
    Private Const StaffStatusNegotiate As String = "2"

    ''' <summary>
    ''' スタッフステータス（スタンバイ）
    ''' </summary>
    ''' <remarks></remarks>
    Private Const StaffStatusStandby As String = "1"

    ''' <summary>
    ''' スタッフステータス（一時退席）
    ''' </summary>
    ''' <remarks></remarks>
    Private Const StaffStatusLeaving As String = "3"

    ''' <summary>
    ''' スタッフステータス（オフライン）
    ''' </summary>
    ''' <remarks></remarks>
    Private Const StaffStatusOffline As String = "4"

    ' $02 start
    ''' <summary>
    ''' スタッフステータス（納車作業中）
    ''' </summary>
    ''' <remarks></remarks>
    Private Const StaffStatusDeliverly As String = "5"

    ' $02 end

    ''' <summary>
    ''' 削除フラグ（未削除）
    ''' </summary>
    ''' <remarks></remarks>
    Private Const DeleteFlagNotDelete As String = "0"

    ''' <summary>
    ''' Push送信用操作番号
    ''' </summary>
    ''' <remarks></remarks>
    Private Const OperationNo As String = "01"

    ''' <summary>
    ''' Push送信用機能番号
    ''' </summary>
    ''' <remarks></remarks>
    Private Const FuncNo As String = "99"

    ''' <summary>
    ''' 正常
    ''' </summary>
    ''' <remarks></remarks>
    Private Const MessageIdNormal As Integer = 0

    ''' <summary>
    ''' オラクルエラータイムアウト
    ''' </summary>
    ''' <remarks></remarks>
    Private Const OracleErrorTimeOut As Integer = 2049

    ''' <summary>
    ''' タイムアウト
    ''' </summary>
    ''' <remarks></remarks>
    Private Const MessageIdTimeOut As Integer = 901

    ''' <summary>
    ''' 排他処理
    ''' </summary>
    ''' <remarks></remarks>
    Private Const MessageIdExclusionFailed As Integer = 902

    ''' <summary>
    ''' 操作権限コード（セールスマネージャ）
    ''' </summary>
    ''' <remarks>未決定</remarks>
    Private Const OperationCdSalesStaffManager As Decimal = 7D

    ''' <summary>
    ''' 操作権限コード（受付）
    ''' </summary>
    ''' <remarks>未決定</remarks>
    Private Const OperationCdReception As Decimal = 51D

    ''' <summary>
    ''' 敬称位置（前）
    ''' </summary>
    ''' <remarks></remarks>
    Private Const NameTitlePositionFront As String = "1"

    ''' <summary>
    ''' 敬称位置（後）
    ''' </summary>
    ''' <remarks></remarks>
    Private Const NameTitlePositionBack As String = "2"

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
    ''' システム環境設定パラメータ（敬称前後）
    ''' </summary>
    ''' <remarks></remarks>
    Private Const NameTitlePotision As String = "KEISYO_ZENGO"

    ''' <summary>
    ''' 苦情情報日数(N日)
    ''' </summary>
    ''' <remarks></remarks>
    Private Const ComplaintDisplayDate As String = "COMPLAINT_DISPLAYDATE"

    ''' <summary>
    ''' 受付メイン画面ID
    ''' </summary>
    ''' <remarks></remarks>
    Private Const ReceptionistId As String = "SC3100101"

    ''' <summary>
    ''' 正常返却ID
    ''' </summary>
    ''' <remarks></remarks>
    Private Const ReturnSuccess As Integer = 0

    ''' <summary>
    ''' 異常返却ID
    ''' </summary>
    ''' <remarks></remarks>
    Private Const ReturnDBUpdateFalse As Integer = 1

    ''' <summary>
    ''' 苦情存在フラグ（存在する）
    ''' </summary>
    ''' <remarks></remarks>
    Private Const ClaimFlagExists As String = "1"


    ' $02 start 新車タブレットショールーム管理機能開発
    ''' <summary>
    ''' 接客区分 - 振り当て待ち 
    ''' </summary>
    ''' <remarks></remarks>
    Private Const ReceptionClassWaitAssgined As String = "1"

    ''' <summary>
    ''' 接客区分 - 接客待ち 
    ''' </summary>
    ''' <remarks></remarks>
    Private Const ReceptionClassWaitService As String = "2"

    ''' <summary>
    ''' 接客区分 - 接客中
    ''' </summary>
    ''' <remarks></remarks>
    Private Const ReceptionClassNegotiation As String = "3"
    ' $02 end   新車タブレットショールーム管理機能開発

    ''' <summary>
    ''' 通知送信種別(査定)
    ''' </summary>
    ''' <remarks></remarks>
    Private Const NoticeAssessment As String = "01"

    ''' <summary>
    ''' 通知送信種別(価格相談)
    ''' </summary>
    ''' <remarks></remarks>
    Private Const NoticePriceConsultation As String = "02"

    ''' <summary>
    ''' 通知送信種別(ヘルプ)
    ''' </summary>
    ''' <remarks></remarks>
    Private Const NoticeHelp As String = "03"

    ''' <summary>
    ''' 通知ステータス(依頼)
    ''' </summary>
    ''' <remarks></remarks>
    Private Const NoticeStatus As String = "1"

    ''' <summary>
    ''' 通知ステータス(受信)
    ''' </summary>
    ''' <remarks></remarks>
    Private Const ReceiveStatus As String = "3"


    ''' <summary>
    ''' 警告音出力フラグ：あり
    ''' </summary>
    ''' <remarks></remarks>
    Private Const AlarmOutputOn As String = "1"

    ''' <summary>
    ''' 警告音出力フラグ：なし
    ''' </summary>
    ''' <remarks></remarks>
    Private Const AlarmOutputOff As String = "0"

    ''' <summary>
    ''' 販売店環境設定パラメータ（受付通知警告音出力権限コードリスト)
    ''' </summary>
    ''' <remarks></remarks>
    Private NoticeAlarmCodeList As String = "RECEPTION_NOTICE_ALARM_CODE_LIST"

    ''' <summary>
    ''' 紐付け人数0
    ''' </summary>
    ''' <remarks></remarks>
    Private Const Nolinking As Integer = 0

    '$08 end NSK s.natsume TKM Change request development for Next Gen e-CRB (CR060)
    ''' <summary>
    ''' エラーメッセージID（10901:使用可能な文字種エラー）
    ''' </summary>
    ''' <remarks></remarks>
    Private Const MessageIdCharacterTypes As Integer = 10901

    ''' <summary>
    ''' エラーメッセージID（10902:名前が3語以内でないエラー）
    ''' </summary>
    ''' <remarks></remarks>
    Private Const MessageIdName3Words As Integer = 10902

    ''' <summary>
    ''' エラーメッセージID（10903:電話番号の文字数のエラー）
    ''' </summary>
    ''' <remarks></remarks>
    Private Const MessageIdTelNumberLength As Integer = 10903
    '$08 end NSK s.natsume TKM Change request development for Next Gen e-CRB (CR060)

#End Region

#Region "非公開変数"
    ''' <summary>
    ''' POSTメッセージリスト
    ''' </summary>
    ''' <remarks></remarks>
    Private postMessageList As List(Of String) = Nothing

    ''' <summary>
    ''' POSTメッセージリスト（PC基盤用）
    ''' </summary>
    ''' <remarks></remarks>
    Private postMessageListPC As List(Of String) = Nothing
#End Region

#Region "商談テーブル情報の取得"

    ''' <summary>
    ''' 商談テーブル情報の取得
    ''' </summary>
    ''' <param name="dealerCode">販売店コード</param>
    ''' <param name="storeCode">店舗コード</param>
    ''' <returns>商談テーブル情報データセット</returns>
    ''' <remarks></remarks>
    Public Function GetSalesTableInfo(ByVal dealerCode As String, ByVal storeCode As String, _
                                      ByVal nowDate As Date) _
                                        As SC3100101DataSet.SC3100101SalesTableUseDataTable

        Logger.Info("GetSalesTableInfo_Start Param[" & dealerCode & ", " & storeCode & "]")

        '開始日時(当日の0:00:00)を格納
        Dim startDate As Date = nowDate.Date
        '終了日時(当日の23:59:59)を格納
        Dim nextDate As Date = startDate.AddDays(NextDay)
        Dim endDate As Date = nextDate.AddSeconds(BeforMillisecond)
        nextDate = Nothing

        '商談テーブル情報データセット
        Dim salesTableInfoDataSet As SC3100101DataSet.SC3100101SalesTableUseDataTable = Nothing

        Using dataAdapter As New SC3100101TableAdapter
            '商談テーブル使用有無の取得
            salesTableInfoDataSet = dataAdapter.GetSalesTableUse(dealerCode, storeCode, _
                                                                            startDate, endDate)
        End Using
        'お客様氏名・商談テーブル情報データセットを返す
        Logger.Info("GetSalesTableInfo_End Ret[" & salesTableInfoDataSet.ToString & "]")
        Return salesTableInfoDataSet
    End Function
#End Region

#Region "来店情報の削除"

    ''' <summary>
    ''' 来店情報の削除
    ''' </summary>
    ''' <param name="visitSequence">来店実績連番</param>
    ''' <param name="updateAccount">更新アカウント</param>
    ''' <returns>メッセージID</returns>
    ''' <remarks></remarks>
    ''' <history>
    ''' 2019/11/12 NSK 鈴木 (トライ店システム評価)次世代セールス基盤：ログ出力機能における保守性向上検証
    ''' </history>
    <EnableCommit()>
    Public Function DeleteVisitorRecord(ByVal visitSequence As Long, ByVal updateAccount As String) As Integer _
    Implements ISC3100101BusinessLogic.DeleteVisitorRecord

        Logger.Info("DeleteVisitorRecord_Start Param[" & visitSequence & ", " & updateAccount & "]")

        Try

            Dim result As Boolean = False

            Using dataAdapter As New SC3100101TableAdapter
                '来店実績ステータスの更新（削除）
                result = dataAdapter.UpdateVisitorCancel(visitSequence, updateAccount)
            End Using

            '来店実績ステータスの更新に失敗
            If Not result Then
                ' Logger.Debug("DeleteVisitorRecord_001" & "UpdateVisitorStatusFailed")
                Me.Rollback = True
                'メッセージIDを更新
                Logger.Error("DeleteVisitorRecord_End Ret[" & MessageIdExclusionFailed & "]")
                Return MessageIdExclusionFailed
            End If
            ' Logger.Debug("DeleteVisitorRecord_002" & "UpdateVisitorStatusSuccess")

            '受付メイン画面更新
            ReceptionUpdate(OperationNo)

            Logger.Info("DeleteVisitorRecord_End Ret[" & MessageIdNormal & "]")
            Return MessageIdNormal

        Catch oraEx As OracleExceptionEx

            If oraEx.Number = OracleErrorTimeOut Then
                ' Logger.Debug("DeleteVisitorRecord_003" & "DataBaseTimeOut")
                'ロールバックを行う。
                Me.Rollback = True

                'ログ出力
                Logger.Error(CType(MessageIdTimeOut, String), oraEx)

                'DBタイムアウトエラー時
                Logger.Error("DeleteVisitorRecord_End Ret[" & MessageIdTimeOut & "]")
                Return MessageIdTimeOut
            Else
                ' Logger.Debug("DeleteVisitorRecord_004" & "OracleException")
                '上記以外のエラーは基盤側で制御
                Throw
            End If
        End Try
    End Function
#End Region

#Region "お客様氏名・商談テーブルの登録"
    ' $08 start NSK s.natsume TKM Change request development for Next Gen e-CRB (CR060)
    ' ''' <summary>
    ' ''' お客様氏名・商談テーブルの登録
    ' ''' </summary>
    ' ''' <param name="visitSequence">来店実績連番</param>
    ' ''' <param name="customerSegment">顧客区分</param>
    ' ''' <param name="tentativeName">仮登録氏名</param>
    ' ''' <param name="oldSalesTableNo">更新前商談テーブルNo.</param>
    ' ''' <param name="newSalesTableNo">更新後商談テーブルNo.</param>
    ' ''' <param name="updateAccount">更新アカウント</param>
    ' ''' <param name="isCustomerNameEdit">仮登録氏名登録フラグ</param>
    ' ''' <returns>処理結果</returns>
    ' ''' <remarks></remarks>
    '<EnableCommit()>
    'Public Function RegistrationNameAndSalesTable(ByVal visitSequence As Long, ByVal customerSegment As String, _
    '                                              ByVal tentativeName As String, ByVal oldSalesTableNo As Integer, _
    '                                              ByVal newSalesTableNo As Integer, ByVal updateAccount As String, _
    '                                     Optional ByVal isCustomerNameEdit As Boolean = True) As Integer _
    '                                          Implements ISC3100101BusinessLogic.RegistrationNameAndSalesTable
    ''' <summary>
    ''' お客様氏名・商談テーブルの登録
    ''' </summary>
    ''' <param name="visitSequence">来店実績連番</param>
    ''' <param name="customerSegment">顧客区分</param>
    ''' <param name="tentativeName">仮登録氏名</param>
    ''' <param name="oldSalesTableNo">更新前商談テーブルNo.</param>
    ''' <param name="newSalesTableNo">更新後商談テーブルNo.</param>
    ''' <param name="updateAccount">更新アカウント</param>
    ''' <param name="telNumber">電話番号</param>
    ''' <param name="isCustomerNameEdit">仮登録氏名登録フラグ</param>
    ''' <returns>処理結果</returns>
    ''' <remarks></remarks>
    ''' <history>
    ''' 2019/11/12 NSK 鈴木 (トライ店システム評価)次世代セールス基盤：ログ出力機能における保守性向上検証
    ''' </history>
    <EnableCommit()>
    Public Function RegistrationNameAndSalesTable(ByVal visitSequence As Long, ByVal customerSegment As String, _
                                                  ByVal tentativeName As String, ByVal oldSalesTableNo As Integer, _
                                                  ByVal newSalesTableNo As Integer, ByVal updateAccount As String, _
                                                  ByVal telNumber As String, _
                                         Optional ByVal isCustomerNameEdit As Boolean = True) As Integer _
                                              Implements ISC3100101BusinessLogic.RegistrationNameAndSalesTable
        ' $08 end NSK s.natsume TKM Change request development for Next Gen e-CRB (CR060)

        Logger.Info("RegistrationNameAndSalesTable_Start Param[" &
                     visitSequence & ", " & customerSegment & ", " & tentativeName & ", " &
                     oldSalesTableNo & ", " & newSalesTableNo & ", " & updateAccount & "]")

        Try
            Dim registNameAndTableResult As Integer = MessageIdNormal
            '仮登録氏名・商談テーブルNo.の更新
            ' $08 start NSK s.natsume TKM Change request development for Next Gen e-CRB (CR060)
            'registNameAndTableResult = UpdateTentativeNameAndSalesTable(visitSequence, _
            '                                                            customerSegment, _
            '                                                            tentativeName, _
            '                                                            updateAccount, _
            '                                                            oldSalesTableNo, _
            '                                                            newSalesTableNo,
            '                                                            isCustomerNameEdit)
            registNameAndTableResult = UpdateTentativeNameAndSalesTable(visitSequence, _
                                                                        customerSegment, _
                                                                        tentativeName, _
                                                                        updateAccount, _
                                                                        oldSalesTableNo, _
                                                                        newSalesTableNo, _
                                                                        telNumber,
                                                                        isCustomerNameEdit)
            ' $08 end NSK s.natsume TKM Change request development for Next Gen e-CRB (CR060)
            '仮登録氏名更新または商談テーブルNo.更新に失敗
            If Not MessageIdNormal.Equals(registNameAndTableResult) Then
                ' Logger.Debug("RegistrationNameAndSalesTable_001" & "UpdateNameOrSalesTableFailed")
                Me.Rollback = True

                Logger.Error("RegistrationNameAndSalesTable_End Ret[" & registNameAndTableResult & "]")
                Return registNameAndTableResult
            End If
            ' Logger.Debug("RegistrationNameAndSalesTable_002" & "UpdateNameAndSalesTableSuccess")

            '受付メイン画面更新
            ReceptionUpdate(OperationNo)

            Logger.Info("RegistrationNameAndSalesTable_End Ret[" & MessageIdNormal & "]")
            Return MessageIdNormal
        Catch oraEx As OracleExceptionEx

            If oraEx.Number = OracleErrorTimeOut Then
                Logger.Error("RegistrationNameAndSalesTable_003" & "DataBaseTimeOut")
                'ロールバックを行う。
                Me.Rollback = True

                'ログ出力
                Logger.Error(CType(MessageIdTimeOut, String), oraEx)

                'DBタイムアウトエラー時
                Logger.Error("RegistrationNameAndSalesTable_End Ret[" & MessageIdTimeOut & "]")
                Return MessageIdTimeOut
            Else
                ' Logger.Debug("RegistrationNameAndSalesTable_004" & "OracleException")
                '上記以外のエラーは基盤側で制御
                Throw
            End If
        End Try
    End Function
#End Region

#Region "スタンバイスタッフの取得"

    ''' <summary>
    ''' スタンバイスタッフの取得
    ''' </summary>
    ''' <param name="dealerCode">販売店コード</param>
    ''' <param name="storeCode">店舗コード</param>
    ''' <returns>スタッフ情報(スタンバイ)データテーブル</returns>
    ''' <remarks></remarks>
    Public Function GetStandbyStaff(ByVal dealerCode As String, ByVal storeCode As String) _
                                    As SC3100101DataSet.SC3100101StandbyStaffDataTable

        Logger.Info("GetStandbyStaff_Start Param[" & dealerCode & ", " & storeCode & "]")

        'スタッフ情報(スタンバイ)データテーブル
        Dim staffStatusDataTable As SC3100101DataSet.SC3100101StandbyStaffDataTable = Nothing

        Using dataAdapter As New SC3100101TableAdapter
            'スタッフ情報（指定ステータス）の取得
            staffStatusDataTable = dataAdapter.GetStaffStandby(dealerCode, storeCode)
        End Using

        'スタッフ情報(スタンバイ)データテーブルを返す
        Logger.Info("GetStandbyStaff_End Ret[" & staffStatusDataTable.ToString & "]")
        Return staffStatusDataTable
    End Function
#End Region

#Region "依頼通知のブロードキャスト"
    '$08 end NSK s.natsume TKM Change request development for Next Gen e-CRB (CR060)
    ' ''' <summary>
    ' ''' 依頼通知のブロードキャスト
    ' ''' </summary>
    ' ''' <param name="visitSequence">来店実績連番</param>
    ' ''' <param name="customerSegment">顧客区分</param>
    ' ''' <param name="tentativeName">仮登録氏名</param>
    ' ''' <param name="oldSalesTableNo">更新前商談テーブルNo.</param>
    ' ''' <param name="newSalesTableNo">更新後商談テーブルNo.</param>
    ' ''' <param name="vehicleNo">車両登録No.</param>
    ' ''' <param name="standbyStaffList">スタンバイスタッフリスト</param>
    ' ''' <param name="updateAccount">更新アカウント</param>
    ' ''' <returns>メッセージID</returns>
    ' ''' <remarks></remarks>
    '<EnableCommit()>
    'Public Function RequestNoticeBroadcast(ByVal visitSequence As Long, ByVal customerSegment As String, _
    '                                       ByVal tentativeName As String, ByVal oldSalesTableNo As Integer, _
    '                                       ByVal newSalesTableNo As Integer, ByVal vehicleNo As String, _
    '                                       ByVal standbyStaffList As List(Of String), ByVal updateAccount As String) As Integer _
    '                                   Implements ISC3100101BusinessLogic.RequestNoticeBroadcast
    ''' <summary>
    ''' 依頼通知のブロードキャスト
    ''' </summary>
    ''' <param name="visitSequence">来店実績連番</param>
    ''' <param name="customerSegment">顧客区分</param>
    ''' <param name="tentativeName">仮登録氏名</param>
    ''' <param name="oldSalesTableNo">更新前商談テーブルNo.</param>
    ''' <param name="newSalesTableNo">更新後商談テーブルNo.</param>
    ''' <param name="vehicleNo">車両登録No.</param>
    ''' <param name="standbyStaffList">スタンバイスタッフリスト</param>
    ''' <param name="updateAccount">更新アカウント</param>
    ''' <param name="telNumber">電話番号</param>
    ''' <returns>メッセージID</returns>
    ''' <remarks></remarks>
    ''' <history>
    ''' 2019/11/12 NSK 鈴木 (トライ店システム評価)次世代セールス基盤：ログ出力機能における保守性向上検証
    ''' </history>
    <EnableCommit()>
    Public Function RequestNoticeBroadcast(ByVal visitSequence As Long, ByVal customerSegment As String, _
                                           ByVal tentativeName As String, ByVal oldSalesTableNo As Integer, _
                                           ByVal newSalesTableNo As Integer, ByVal vehicleNo As String, _
                                           ByVal standbyStaffList As List(Of String), ByVal updateAccount As String, _
                                           ByVal telNumber As String) As Integer _
                                       Implements ISC3100101BusinessLogic.RequestNoticeBroadcast
        '$08 end NSK s.natsume TKM Change request development for Next Gen e-CRB (CR060)

        Logger.Info("RequestNoticeBroadcast_Start Param[" &
                     visitSequence & ", " & customerSegment & ", " & tentativeName & ", " &
                     oldSalesTableNo & ", " & newSalesTableNo & ", " & vehicleNo & ", " &
                     standbyStaffList.ToString & ", " & updateAccount & "]")

        Try
            Dim registNameAndTableResult As Integer = MessageIdNormal

            '仮登録氏名・商談テーブルNo.の更新
            '$08 start NSK s.natsume TKM Change request development for Next Gen e-CRB (CR060)
            'registNameAndTableResult = UpdateTentativeNameAndSalesTable(visitSequence, _
            '                                                            customerSegment, _
            '                                                            tentativeName, _
            '                                                            updateAccount, _
            '                                                            oldSalesTableNo, _
            '                                                            newSalesTableNo)
            registNameAndTableResult = UpdateTentativeNameAndSalesTable(visitSequence, _
                                                            customerSegment, _
                                                            tentativeName, _
                                                            updateAccount, _
                                                            oldSalesTableNo, _
                                                            newSalesTableNo, _
                                                            telNumber)
            '$08 end NSK s.natsume TKM Change request development for Next Gen e-CRB (CR060)
            '仮登録氏名更新または商談テーブルNo.更新に失敗
            If Not MessageIdNormal.Equals(registNameAndTableResult) Then
                ' Logger.Debug("RequestNoticeBroadcast_001" & "UpdateNameOrSalesTableFailed")
                Me.Rollback = True

                Logger.Error("RequestNoticeBroadcast_End Ret[" & registNameAndTableResult & "]")
                Return registNameAndTableResult
            End If
            ' Logger.Debug("RequestNoticeBroadcast_002" & "UpdateNameAndSalesTableSuccess")

            Dim broadcastResult As Boolean = False

            Using dataAdapter As New SC3100101TableAdapter
                'ブロードキャスト更新
                broadcastResult = dataAdapter.UpdateBroadcast(visitSequence, updateAccount)
            End Using

            'ブロードキャスト更新に失敗
            If Not broadcastResult Then
                ' Logger.Debug("RequestNoticeBroadcast_003" & "UpdateBroadcastFailed")
                Me.Rollback = True

                'メッセージIDを更新
                Logger.Error("RequestNoticeBroadcast_End Ret[" & MessageIdExclusionFailed & "]")
                Return MessageIdExclusionFailed
            End If
            ' Logger.Debug("RequestNoticeBroadcast_004" & "UpdateBroadcastSuccess")

            '新規顧客は敬称なし
            Const CustomerNameTitle As String = ""
            Const NameTitlePosition As String = "1"

            '対応依頼メッセージ作成
            Dim message As String = MakeRequestNoticeMessage(tentativeName, CustomerNameTitle, _
                                                             NameTitlePosition, vehicleNo, customerSegment)

            'スタンバイスタッフへ対応依頼を送信
            For Each standbyStaff As String In standbyStaffList
                ' Logger.Debug("RequestNoticeBroadcast_005" & "StandbyStaffLoop(" & standbyStaff & ")")
                Dim requestNoticeResult As Boolean = False

                Using dataAdapter As New SC3100101TableAdapter
                    '対応依頼通知更新
                    requestNoticeResult = dataAdapter.InsertRequestNotice(visitSequence, standbyStaff, updateAccount)
                End Using

                '対応依頼通知更新に失敗
                If Not requestNoticeResult Then
                    ' Logger.Debug("RequestNoticeBroadcast_006" & "InsertRequestNoticeFailed")
                    'ロールバックを行う。
                    Me.Rollback = True

                    'メッセージIDを更新
                    Logger.Error("RequestNoticeBroadcast_End Ret[" & MessageIdExclusionFailed & "]")
                    Return MessageIdExclusionFailed
                End If
                ' Logger.Debug("RequestNoticeBroadcast_007" & "InsertRequestNoticeSuccess")

                'Push送信
                PushDealRequestMessage(standbyStaff, message)
            Next
            ' Logger.Debug("RequestNoticeBroadcast_008" & "SendToStandbyStaffSuccess")

            '受付メイン画面更新
            ReceptionUpdate(OperationNo)

            Logger.Info("RequestNoticeBroadcast_End Ret[" & MessageIdNormal & "]")
            Return MessageIdNormal
        Catch oraEx As OracleExceptionEx

            If oraEx.Number = OracleErrorTimeOut Then
                ' Logger.Debug("RequestNoticeBroadcast_009" & "DataBaseTimeOut")

                'ロールバックを行う。
                Me.Rollback = True

                'ログ出力
                Logger.Error(CType(MessageIdTimeOut, String), oraEx)

                'DBタイムアウトエラー時
                Logger.Error("RequestNoticeBroadcast_End Ret[" & MessageIdTimeOut & "]")
                Return MessageIdTimeOut
            Else
                ' Logger.Debug("RequestNoticeBroadcast_010" & "OracleException")
                '上記以外のエラーは基盤側で制御
                Throw
            End If
        End Try
    End Function
#End Region

#Region "SC割り当て処理"

    ''' <summary>
    ''' SC割り当て処理
    ''' </summary>
    ''' <param name="visitSequence">来店実績連番</param>
    ''' <param name="dealAccount">対応アカウント</param>
    ''' <param name="updateAccount">更新アカウント</param>
    ''' <returns>メッセージID</returns>
    ''' <remarks></remarks>
    ''' <history>
    ''' 2019/11/12 NSK 鈴木 (トライ店システム評価)次世代セールス基盤：ログ出力機能における保守性向上検証
    ''' </history>
    <EnableCommit()>
    Public Function SalesConsultantAssignment(ByVal visitSequence As Long, ByVal dealAccount As String, _
                                              ByVal updateAccount As String) As Integer _
                                          Implements ISC3100101BusinessLogic.SalesConsultantAssignment

        Logger.Info("SalesConsultantAssignment_Start Param[" & visitSequence & ", " & updateAccount & ", " & updateAccount & "]")

        Try
            Dim updateDealStaffCdResult As Boolean = False
            Dim visitorCustomerDataTable As SC3100101DataSet.VisitReceptionVisitorCustomerDataTable = Nothing

            Using dataAdapter As New SC3100101TableAdapter
                '対応依頼通知削除
                dataAdapter.DeleteRequestNotice(visitSequence, updateAccount)

                '対応担当スタッフコード更新
                updateDealStaffCdResult = dataAdapter.UpdateDealStaffCode(visitSequence, _
                                                                                    dealAccount, _
                                                                                    updateAccount)
                '対応担当スタッフコード更新に失敗
                If Not updateDealStaffCdResult Then
                    ' Logger.Debug("SalesConsultantAssignment_001" & "UpdateDealStaffCodeFailed")
                    Me.Rollback = True

                    'メッセージIDを更新
                    Logger.Error("SalesConsultantAssignment_End Ret[" & MessageIdExclusionFailed & "]")
                    Return MessageIdExclusionFailed
                End If
                ' Logger.Debug("SalesConsultantAssignment_002" & "UpdateDealStaffCodeSuccess")
            End Using

            Using dataAdapter As New SC3100101TableAdapter
                '来店実績お客様情報取得
                visitorCustomerDataTable = dataAdapter.GetVisitorCustomer(visitSequence)
            End Using

            '敬称位置取得
            Dim sysEnvSet As New SystemEnvSetting
            Dim sysEnvSetTitleRow As SystemEnvSettingDataSet.SYSTEMENVSETTINGRow = Nothing
            Logger.Info("SalesConsultantAssignment_003" & "Call_Start GetSystemEnvSetting Param[" & NameTitlePotision & "]")
            sysEnvSetTitleRow = sysEnvSet.GetSystemEnvSetting(NameTitlePotision)
            Logger.Info("SalesConsultantAssignment_003" & "Call_End GetSystemEnvSetting Ret[" & sysEnvSetTitleRow.PARAMVALUE & "]")

            ' 苦情情報日数取得
            Dim sysEnvSetComplaintDisplayDateRow As SystemEnvSettingDataSet.SYSTEMENVSETTINGRow = Nothing
            Logger.Info("SalesConsultantAssignment_003_1" & "Call_Start GetSystemEnvSetting Param[" & ComplaintDisplayDate & "]")
            sysEnvSetComplaintDisplayDateRow = sysEnvSet.GetSystemEnvSetting(ComplaintDisplayDate)
            Logger.Info("SalesConsultantAssignment_003_1" & "Call_End GetSystemEnvSetting Ret[" & sysEnvSetComplaintDisplayDateRow.PARAMVALUE & "]")
            Dim complaintDateCount As String = sysEnvSetComplaintDisplayDateRow.PARAMVALUE

            'ログインユーザの情報を格納
            ' Logger.Debug("SalesConsultantAssignment_003_2" & "Call_Start StaffContext.Current")
            Dim context As StaffContext = StaffContext.Current
            ' Logger.Debug("SalesConsultantAssignment_003_2" & "Call_End StaffContext.Current Ret[" & context.ToString & "]")

            '現在日時 基盤より取得
            ' Logger.Debug("SalesConsultantAssignment_003_3" & "Call_Start DateTimeFunc.Now Param[" & context.DlrCD & "]")
            Dim nowDate As Date = DateTimeFunc.Now(context.DlrCD)
            ' Logger.Debug("SalesConsultantAssignment_003_3" & "Call_End DateTimeFunc.Now Ret[" & nowDate & "]")

            Dim isClaim As Boolean = False

            '苦情情報の取得
            Dim utility As New VisitUtilityBusinessLogic
            isClaim = utility.HasClaimInfo(visitorCustomerDataTable.Item(0).CUSTSEGMENT, _
                                        visitorCustomerDataTable.Item(0).CUSTID, _
                                        nowDate, _
                                        CInt(complaintDateCount))

            '対応依頼メッセージ作成
            Dim message As String = MakeRequestNoticeMessage(visitorCustomerDataTable.Item(0).CUSTNAME, _
                                                             visitorCustomerDataTable.Item(0).CUSTNAMETITLE, _
                                                             sysEnvSetTitleRow.PARAMVALUE, _
                                                             visitorCustomerDataTable.Item(0).VCLREGNO, _
                                                             visitorCustomerDataTable.Item(0).CUSTSEGMENT, _
                                                             isClaim)

            '担当スタッフへ対応依頼通知を送信
            PushDealRequestMessage(dealAccount, message)

            '受付メイン画面更新
            ReceptionUpdate(OperationNo)


            Logger.Info("SalesConsultantAssignment_End Ret[" & MessageIdNormal & "]")
            Return MessageIdNormal
        Catch oraEx As OracleExceptionEx

            If oraEx.Number = OracleErrorTimeOut Then
                ' Logger.Debug("SalesConsultantAssignment_004" & "DataBaseTimeOut")
                'ロールバックを行う。
                Me.Rollback = True

                'ログ出力
                Logger.Error(CType(MessageIdTimeOut, String), oraEx)

                'DBタイムアウトエラー時
                Logger.Error("SalesConsultantAssignment_End Ret[" & MessageIdTimeOut & "]")
                Return MessageIdTimeOut
            Else
                ' Logger.Debug("SalesConsultantAssignment_005" & "OracleException")
                '上記以外のエラーは基盤側で制御
                Throw
            End If
        End Try
    End Function
#End Region

#Region "対応依頼メッセージ作成"

    ''' <summary>
    ''' 対応依頼メッセージ作成
    ''' </summary>
    ''' <param name="customerName">顧客名</param>
    ''' <param name="customerNameTitle">顧客敬称</param>
    ''' <param name="nameTitlePosition">顧客敬称位置</param>
    ''' <param name="vehicleNo">車両登録No</param>
    ''' <param name="custSegment">顧客区分</param>
    ''' <param name="isClaim">苦情有無</param>
    ''' <returns>対応依頼メッセージ</returns>
    ''' <remarks></remarks>
    Private Function MakeRequestNoticeMessage(ByVal customerName As String, _
                                              ByVal customerNameTitle As String, _
                                              ByVal nameTitlePosition As String, _
                                              ByVal vehicleNo As String, _
                                              ByVal custSegment As String, _
                                              Optional ByVal isClaim As Boolean = False) As String

        ' Logger.Debug("MakeRequestNoticeMessage_Start Param[" & customerName & ", " & customerNameTitle & ", " _
        '                                                     & nameTitlePosition & ", " & vehicleNo & ", " & custSegment & "]")

        Dim message As New StringBuilder
        ' Logger.Debug("MakeRequestNoticeMessage_001" & "Call_Start WebWordUtility.GetWord Param[" & ReceptionistId & ",29]")
        Dim wordRequestNotice As String = WebWordUtility.GetWord(ReceptionistId, 29)
        ' Logger.Debug("MakeRequestNoticeMessage_001" & "Call_Start WebWordUtility.GetWord Ret[" & wordRequestNotice & "]")

        '苦情がある場合「！」を先頭に追加
        If isClaim Then
            ' Logger.Debug("MakeRequestNoticeMessage_001_1" & "Call_Start WebWordUtility.GetWord Param[" & ReceptionistId & ",54]")
            Dim claimExclamation As String = WebWordUtility.GetWord(ReceptionistId, 54)
            ' Logger.Debug("MakeRequestNoticeMessage_001_1" & "Call_Start WebWordUtility.GetWord Ret[" & claimExclamation & "]")
            message.Append(claimExclamation)
        End If

        Dim customerNameData As String

        If Not IsDBNull(customerName) AndAlso Not String.IsNullOrEmpty(customerName) AndAlso Not String.IsNullOrEmpty(customerName.Trim()) Then
            customerNameData = customerName

            'メッセージの組み立て
            With message
                If nameTitlePosition.Equals(NameTitlePositionFront) Then
                    .Append(customerNameTitle)
                    .Append(customerNameData)
                Else
                    .Append(customerNameData)
                    .Append(customerNameTitle)
                End If
            End With
        Else
            If Not IsDBNull(custSegment) AndAlso Not String.IsNullOrEmpty(custSegment) Then
                ' Logger.Debug("MakeRequestNoticeMessage_002" & "Call_Start WebWordUtility.GetWord Param[" & ReceptionistId & ",31]")
                Dim wordUnknown As String = WebWordUtility.GetWord(ReceptionistId, 31)
                ' Logger.Debug("MakeRequestNoticeMessage_002" & "Call_Start WebWordUtility.GetWord Ret[" & wordUnknown & "]")
                customerNameData = wordUnknown
            Else
                ' Logger.Debug("MakeRequestNoticeMessage_003" & "Call_Start WebWordUtility.GetWord Param[" & ReceptionistId & ",32]")
                Dim wordNewCustomer As String = WebWordUtility.GetWord(ReceptionistId, 32)
                ' Logger.Debug("MakeRequestNoticeMessage_003" & "Call_Start WebWordUtility.GetWord Ret[" & wordNewCustomer & "]")
                customerNameData = wordNewCustomer
            End If

            message.Append(customerNameData)
        End If

        message.Append(" ")
        message.Append(wordRequestNotice)
        message.Append(" ")
        message.Append(vehicleNo)

        ' Logger.Debug("MakeRequestNoticeMessage_End Ret[" & message.ToString() & "]")
        Return message.ToString()
    End Function
#End Region

#Region "仮登録氏名・商談テーブルの更新"

    ' $08 start NSK s.natsume TKM Change request development for Next Gen e-CRB (CR060)
    ' ''' <summary>
    ' ''' 仮登録氏名・商談テーブルの更新
    ' ''' </summary>
    ' ''' <param name="visitSequence">来店実績連番</param>
    ' ''' <param name="customerSegment">顧客区分</param>
    ' ''' <param name="tentativeName">仮登録氏名</param>
    ' ''' <param name="updateAccount">更新アカウント</param>
    ' ''' <param name="oldSalesTableNo">更新前商談テーブルNo.</param>
    ' ''' <param name="newSalesTableNo">更新後商談テーブルNo.</param>
    ' ''' <returns>メッセージID</returns>
    ' ''' <remarks></remarks>
    'Private Function UpdateTentativeNameAndSalesTable(ByVal visitSequence As Long, _
    '                                                  ByVal customerSegment As String, _
    '                                                  ByVal tentativeName As String, _
    '                                                  ByVal updateAccount As String, _
    '                                                  ByVal oldSalesTableNo As Integer, _
    '                                                  ByVal newSalesTableNo As Integer, _
    '                                         Optional ByVal isCustomerNameEdit As Boolean = True) _
    '                                                  As Integer
    ''' <summary>
    ''' 仮登録氏名・商談テーブルの更新
    ''' </summary>
    ''' <param name="visitSequence">来店実績連番</param>
    ''' <param name="customerSegment">顧客区分</param>
    ''' <param name="tentativeName">仮登録氏名</param>
    ''' <param name="updateAccount">更新アカウント</param>
    ''' <param name="oldSalesTableNo">更新前商談テーブルNo.</param>
    ''' <param name="newSalesTableNo">更新後商談テーブルNo.</param>
    ''' <returns>メッセージID</returns>
    ''' <remarks></remarks>
    Private Function UpdateTentativeNameAndSalesTable(ByVal visitSequence As Long, _
                                                      ByVal customerSegment As String, _
                                                      ByVal tentativeName As String, _
                                                      ByVal updateAccount As String, _
                                                      ByVal oldSalesTableNo As Integer, _
                                                      ByVal newSalesTableNo As Integer, _
                                                      ByVal telNumber As String, _
                                             Optional ByVal isCustomerNameEdit As Boolean = True) _
                                                      As Integer
        ' $08 start NSK s.natsume TKM Change request development for Next Gen e-CRB (CR060)

        ' Logger.Debug("UpdateTentativeNameAndSalesTable_Start Param[" & _
        '             visitSequence & ", " & customerSegment & ", " & tentativeName & ", " & _
        '             updateAccount & ", " & oldSalesTableNo & ", " & newSalesTableNo & "]")

        '新規顧客の場合で尚且仮登録フラグがtrueの場合、仮登録氏名の登録を実施
        If String.IsNullOrEmpty(customerSegment) And isCustomerNameEdit Then
            ' Logger.Debug("UpdateTentativeNameAndSalesTable_001" & "UpdateTentativeName")
            Dim registTentativeNameResult As Boolean = False

            Using dataAdapter As New SC3100101TableAdapter
                '仮登録氏名の登録
                registTentativeNameResult = dataAdapter.UpdateTentativeName(visitSequence, _
                                                                            tentativeName, _
                                                                            updateAccount)
            End Using

            '登録に失敗
            If Not registTentativeNameResult Then
                ' Logger.Debug("UpdateTentativeNameAndSalesTable_002" & "UpdateTentativeNameFailed")

                'メッセージIDを更新
                ' Logger.Debug("UpdateTentativeNameAndSalesTable_End Ret[" & MessageIdExclusionFailed & "]")
                Return MessageIdExclusionFailed
            End If
            ' Logger.Debug("UpdateTentativeNameAndSalesTable_003" & "UpdateTentativeNameSuccess")

            '$08 start NSK s.natsume TKM Change request development for Next Gen e-CRB (CR060)
            Dim registTelNumberResult As Boolean = False

            Using dataAdapter As New SC3100101TableAdapter
                'ローカルテーブルにデータが存在するかを確認
                If (0 < dataAdapter.GetVisitSalesLocalCount(visitSequence)) Then
                    ' データが存在するなら更新を行う
                    registTelNumberResult = dataAdapter.UpdateTelNo(visitSequence, _
                                                                        telNumber, _
                                                                        updateAccount)
                Else
                    ' データが存在しないなら挿入を行う
                    registTelNumberResult = dataAdapter.InsertTelNo(visitSequence, _
                                                                        telNumber, _
                                                                        updateAccount)
                End If
            End Using

            '登録に失敗
            If Not registTentativeNameResult Then
                'メッセージIDを更新
                Return MessageIdExclusionFailed
            End If

            '$08 end NSK s.natsume TKM Change request development for Next Gen e-CRB (CR060)

        End If

        Dim registUseSalesTableResult As Boolean = False

        Using dataAdapter As New SC3100101TableAdapter
            '使用商談テーブルの登録
            registUseSalesTableResult = dataAdapter.UpdateSalesTableNo(visitSequence, _
                                                                    oldSalesTableNo, _
                                                                    newSalesTableNo, _
                                                                    updateAccount)
        End Using

        '登録に失敗
        If Not registUseSalesTableResult Then
            ' Logger.Debug("UpdateTentativeNameAndSalesTable_004" & "UpdateSalesTableNoFailed")

            'メッセージIDを更新
            ' Logger.Debug("UpdateTentativeNameAndSalesTable_End Ret[" & MessageIdExclusionFailed & "]")
            Return MessageIdExclusionFailed
        End If
        ' Logger.Debug("UpdateTentativeNameAndSalesTable_005" & "UpdateSalesTableNoSuccess")

        ' Logger.Debug("UpdateTentativeNameAndSalesTable_End Ret[" & MessageIdNormal & "]")
        Return MessageIdNormal
    End Function
#End Region

#Region "受付画面更新"

    ''' <summary>
    ''' 受付画面更新
    ''' </summary>
    ''' <param name="operationNo">操作番号</param>
    ''' <remarks></remarks>
    Private Sub ReceptionUpdate(ByVal operationNo As String)

        ' Logger.Debug("ReceptionUpdate_Start Param[" & operationNo & "]")

        'ログインユーザの情報を格納
        ' Logger.Debug("ReceptionUpdate_001" & "Call_Start StaffContext.Current")
        Dim context As StaffContext = StaffContext.Current
        ' Logger.Debug("ReceptionUpdate_001" & "Call_End StaffContext.Current Ret[" & context.ToString & "]")

        'ユーザマスタテーブルへのアクセスクラス
        Dim users As New Users

        '操作権限コードリスト
        '送信先にブランチマネージャー・セールスマネージャーは現状不要であるため、一旦除いておく
        Dim operationCdList As New List(Of Decimal)
        operationCdList.Add(OperationCdSalesStaffManager)
        operationCdList.Add(OperationCdReception)

        '画面更新対象スタッフの取得
        Logger.Info("ReceptionUpdate_002" & "Call_Start GetAllUser Param[" & _
                     context.DlrCD & "," & context.BrnCD & "," & operationCdList.ToString & "," & DeleteFlagNotDelete & "]")
        Dim updateUserDataTable As UsersDataSet.USERSDataTable = _
            users.GetAllUser(context.DlrCD, context.BrnCD, operationCdList, DeleteFlagNotDelete)
        Logger.Info("ReceptionUpdate_002" & "Call_Start GetAllUser Ret[" & updateUserDataTable.ToString & "]")
        '画面更新対象スタッフチェック
        If updateUserDataTable.Count = 0 Then
            ' Logger.Debug("ReceptionUpdate_003" & "UpdateTargetNothing")
            '対象者がいない場合は処理を抜ける
            ' Logger.Debug("ReceptionUpdate_End Ret[]")
            Return
        End If
        ' Logger.Debug("ReceptionUpdate_004" & "UpdateTargetNotNothing")

        'オンラインユーザのみにする
        Dim VisitUtility As New VisitUtility
        Dim onlineUsers As UsersDataSet.USERSDataTable = VisitUtility.GetOnlineUsers(updateUserDataTable)
        VisitUtility = Nothing

        '画面更新対象スタッフへ画面更新情報をPush送信
        For Each row As UsersDataSet.USERSRow In onlineUsers
            ' Logger.Debug("ReceptionUpdate_005" & "SendReceptionViewUpdate(" & row.ACCOUNT & ")")
            'ログインユーザへのPush送信は行わない
            If context.Account.Equals(row.ACCOUNT) Then
                ' Logger.Debug("ReceptionUpdate_006" & "NotSendToLoginUser")
                Continue For
            End If

            'Push送信
            '$04 start コード分析対応
            SendReceptionUpdatePush(row.ACCOUNT, operationNo)
            '$04 end コード分析対応

        Next

        ' Logger.Debug("ReceptionUpdate_End Ret[]")
    End Sub
#End Region

#Region "Push送信"

    ''' <summary>
    ''' Push送信
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub SendPush()

        Logger.Info("SendPush_Start Param[]")

        Dim VisitUtility As New VisitUtility

        If Not Me.postMessageList Is Nothing Then

            For Each postMessage In Me.postMessageList

                ' Push送信
                VisitUtility.SendPush(postMessage)

            Next

        End If

        If Not Me.postMessageListPC Is Nothing Then

            For Each postMessage In Me.postMessageListPC

                ' Push送信（PC基盤用）
                VisitUtility.SendPushPC(postMessage)

            Next

        End If

        VisitUtility = Nothing

        Logger.Info("SendPush_End Ret[]")
    End Sub
#End Region

#Region "受付画面更新Push送信"
    ' $04 start コード分析対応
    ''' <summary>
    ''' 受付画面更新Push送信
    ''' </summary>
    ''' <param name="sendAccount">送信先アカウント</param>
    ''' <param name="operationNo">操作番号</param>
    ''' <remarks></remarks>
    Private Sub SendReceptionUpdatePush(ByVal sendAccount As String, ByVal operationNo As String)
        Dim postMessage As New StringBuilder
        ' $04 end コード分析対応

        ' Logger.Debug("SendReceptionUpdatePush_Start Param[" & sendAccount & ", " & operationNo & "]")

        'POST送信する文字列を作成する。
        With postMessage
            .Append("cat=action")
            .Append("&type=main")
            .Append("&sub=js")
            .Append("&uid=")
            .Append(sendAccount)
            .Append("&time=0")
            .Append("&js1=SC3100101Update('")
            .Append(FuncNo)
            .Append("','")
            .Append(operationNo)
            .Append("')")
        End With

        ' PUSH送信文字列を保持する
        If Me.postMessageList Is Nothing Then
            Me.postMessageList = New List(Of String)
        End If

        Me.postMessageList.Add(postMessage.ToString)

        ' Logger.Debug("SendReceptionUpdatePush_End Ret[]")
    End Sub
#End Region

#Region "対応依頼メッセージのPUSH送信"

    ''' <summary>
    ''' 対応依頼メッセージのPUSH送信
    ''' </summary>
    ''' <param name="sendAccount">送信先アカウント</param>
    ''' <param name="message">メッセージ</param>
    ''' <remarks></remarks>
    Private Sub PushDealRequestMessage(ByVal sendAccount As String, ByVal message As String)
        Dim postMessage As New StringBuilder

        ' Logger.Debug("PushDealRequestMessage_Start Param[" & sendAccount & ", " & message & "]")

        'POST送信する文字列を作成する。
        With postMessage
            .Append("cat=popup")
            .Append("&type=header")
            .Append("&sub=text")
            .Append("&uid=" & sendAccount)
            .Append("&time=3")
            .Append("&color=F9EDBE64")
            .Append("&height=50")
            .Append("&width=1024")
            .Append("&pox=0")
            .Append("&msg=" & message)
            .Append("&js1=icropScript.ui.setVisitor()")
            .Append("&js2=icropScript.ui.openVisitorListDialog()")
        End With

        ' PUSH送信文字列を保持する
        If Me.postMessageList Is Nothing Then
            Me.postMessageList = New List(Of String)
        End If

        Me.postMessageList.Add(postMessage.ToString)

        ' Logger.Debug("PushDealRequestMessage_End Ret[]")
    End Sub
#End Region

    ' $01 start 複数顧客に対する商談平行対応
#Region "紐付け解除情報の取得"

    ' ''' <summary>
    ' ''' 紐付け解除情報の取得
    ' ''' </summary>
    ' ''' <param name="dealerCode">販売店コード</param>
    ' ''' <param name="storeCode">店舗コード</param>
    ' ''' <param name="staffCode">スタッフコード</param>
    ' ''' <param name="nowDate">現在日時</param>
    ' ''' <returns>紐付け解除データテーブル</returns>
    ' ''' <remarks></remarks>
    'Public Function GetLinkingCancel(ByVal dealerCode As String, ByVal storeCode As String, _
    '                                 ByVal staffCode As String, ByVal nowDate As Date) _
    '                                 As SC3100101LinkingCancelDataTable

    '    Logger.Info("GetLinkingCancel_Start ")
    '    Logger.Info("Param[" & dealerCode & ", " & storeCode & ", " & staffCode & ", " & nowDate & "]")

    '    '開始日時(当日の0:00:00)を格納
    '    Dim startDate As Date = nowDate.Date

    '    '終了日時(当日の23:59:59)を格納]
    '    Dim nextDate As Date = startDate.AddDays(NextDay)
    '    Dim endDate As Date = nextDate.AddSeconds(BeforMillisecond)
    '    nextDate = Nothing

    '    '返却用DataSet
    '    Dim retDataSet As SC3100101LinkingCancelDataTable = Nothing

    '    Using dataAdapter As New SC3100101TableAdapter
    '        retDataSet = dataAdapter.GetLinkingCancel(dealerCode, storeCode, staffCode, startDate, endDate)
    '    End Using

    '    Logger.Info("GetLinkingCancel_End ")
    '    Logger.Info("ret[" & retDataSet.ToString & "]")

    '    Return retDataSet
    'End Function

#End Region

#Region "紐付け解除更新"

    ' ''' <summary>
    ' ''' 紐付け解除更新
    ' ''' </summary>
    ' ''' <param name="visitSeqList">来店実績連番リスト</param>
    ' ''' <param name="dealAccount">対応アカウント</param>
    ' ''' <param name="updateAccount">対応アカウント</param>
    ' ''' <returns>メッセージID</returns>
    ' ''' <remarks></remarks>
    '<EnableCommit()>
    'Public Function LinkingCancel(ByVal visitSeqList As List(Of String), _
    '                              ByVal dealAccount As String, _
    '                              ByVal updateAccount As String) As Integer _
    '                          Implements ISC3100101BusinessLogic.LinkingCancel

    '    Try

    '        Using dataAdapter As New SC3100101TableAdapter

    '            Dim updateLinkingCancelResult As Boolean

    '            For Each visitSeqListItem As String In visitSeqList

    '                updateLinkingCancelResult = False

    '                ' 紐付け解除更新
    '                updateLinkingCancelResult = dataAdapter.UpdateLinkingCancel(CType(visitSeqListItem, Long), _
    '                                                                            dealAccount, _
    '                                                                            updateAccount)

    '                ' 紐付け解除更新に失敗
    '                If Not updateLinkingCancelResult Then
    '                    ' Logger.Debug("LinkingCancel_001" & "UpdateLinkingCancel Failed")
    '                    Me.Rollback = True

    '                    'メッセージIDを更新
    '                    Logger.Info("LinkingCancel_End Ret[" & MessageIdExclusionFailed & "]")
    '                    Return MessageIdExclusionFailed
    '                End If

    '            Next

    '            ' Logger.Debug("LinkingCancel_002" & "UpdateLinkingCancel Success")

    '        End Using

    '        ' 敬称位置取得
    '        Dim sysEnvSet As New SystemEnvSetting
    '        Dim sysEnvSetTitleRow As SystemEnvSettingDataSet.SYSTEMENVSETTINGRow = Nothing
    '        Logger.Info("LinkingCancel_003" & "Call_Start GetSystemEnvSetting Param[" & NameTitlePotision & "]")
    '        sysEnvSetTitleRow = sysEnvSet.GetSystemEnvSetting(NameTitlePotision)
    '        Logger.Info("LinkingCancel_003" & "Call_End GetSystemEnvSetting Ret[" & sysEnvSetTitleRow.PARAMVALUE & "]")

    '        ' 苦情情報日数取得
    '        Dim sysEnvSetComplaintDisplayDateRow As SystemEnvSettingDataSet.SYSTEMENVSETTINGRow = Nothing
    '        Logger.Info("LinkingCancel_003_1" & "Call_Start GetSystemEnvSetting Param[" & ComplaintDisplayDate & "]")
    '        sysEnvSetComplaintDisplayDateRow = sysEnvSet.GetSystemEnvSetting(ComplaintDisplayDate)
    '        Logger.Info("LinkingCancel_003_1" & "Call_End GetSystemEnvSetting Ret[" & sysEnvSetComplaintDisplayDateRow.PARAMVALUE & "]")
    '        Dim complaintDateCount As String = sysEnvSetComplaintDisplayDateRow.PARAMVALUE

    '        'ログインユーザの情報を格納
    '        ' Logger.Debug("LinkingCancel_003_2" & "Call_Start StaffContext.Current")
    '        Dim context As StaffContext = StaffContext.Current
    '        ' Logger.Debug("LinkingCancel_003_2" & "Call_End StaffContext.Current Ret[" & context.ToString & "]")

    '        '現在日時 基盤より取得
    '        ' Logger.Debug("LinkingCancel_003_3" & "Call_Start DateTimeFunc.Now Param[" & context.DlrCD & "]")
    '        Dim nowDate As Date = DateTimeFunc.Now(context.DlrCD)
    '        ' Logger.Debug("LinkingCancel_003_3" & "Call_End DateTimeFunc.Now Ret[" & nowDate & "]")

    '        Dim visitorCustomerDataTable As VisitReceptionVisitorCustomerDataTable = Nothing
    '        Dim message As String = Nothing
    '        Dim isClaim As Boolean = False

    '        Using dataAdapter As New VisitReceptionTableAdapter

    '            For Each visitSeqListItem As String In visitSeqList

    '                ' 来店実績お客様情報取得
    '                visitorCustomerDataTable = dataAdapter.GetVisitorCustomer(CType(visitSeqListItem, Long))

    '                '苦情情報の取得
    '                Dim utility As New VisitUtilityBusinessLogic
    '                isClaim = utility.HasClaimInfo(visitorCustomerDataTable.Item(0).CUSTSEGMENT, _
    '                                                    visitorCustomerDataTable.Item(0).CUSTID, _
    '                                                    nowDate, _
    '                                                    CInt(complaintDateCount))

    '                ' 紐付け解除メッセージ作成
    '                message = MakeLinkingCancelMessage(visitorCustomerDataTable.Item(0).CUSTNAME, _
    '                                                   visitorCustomerDataTable.Item(0).CUSTNAMETITLE, _
    '                                                   sysEnvSetTitleRow.PARAMVALUE, _
    '                                                   visitorCustomerDataTable.Item(0).CUSTSEGMENT, _
    '                                                   isClaim)

    '                ' 紐付け解除メッセージの保持
    '                PushLinkingCancelMessage(dealAccount, message)

    '            Next

    '            Logger.Info("LinkingCancel_004" & "MakeLinkingCancelMessage Success")

    '        End Using

    '        ' 受付メイン画面更新
    '        ReceptionUpdate(OperationNo)

    '        Logger.Info("LinkingCancel_End Ret[" & MessageIdNormal & "]")
    '        Return MessageIdNormal
    '    Catch oraEx As OracleExceptionEx

    '        If oraEx.Number = OracleErrorTimeOut Then
    '            ' Logger.Debug("LinkingCancel_005" & "DataBaseTimeOut")
    '            ' ロールバックを行う
    '            Me.Rollback = True

    '            ' ログ出力
    '            Logger.Error(CType(MessageIdTimeOut, String), oraEx)

    '            ' DBタイムアウトエラー時
    '            Logger.Info("LinkingCancel_End Ret[" & MessageIdTimeOut & "]")
    '            Return MessageIdTimeOut
    '        Else
    '            ' Logger.Debug("LinkingCancel_006" & "OracleException")
    '            ' 上記以外のエラーは基盤側で制御
    '            Throw
    '        End If
    '    End Try

    'End Function

#End Region

#Region "紐付け解除メッセージ作成"

    ' ''' <summary>
    ' ''' 紐付け解除メッセージ作成
    ' ''' </summary>
    ' ''' <param name="customerName">顧客名</param>
    ' ''' <param name="customerNameTitle">顧客敬称</param>
    ' ''' <param name="nameTitlePosition">顧客敬称位置</param>
    ' ''' <param name="custSegment">顧客区分</param>
    ' ''' <param name="isClaim">苦情有無</param>
    ' ''' <returns>紐付け解除メッセージ</returns>
    ' ''' <remarks></remarks>
    'Private Function MakeLinkingCancelMessage(ByVal customerName As String, _
    '                                          ByVal customerNameTitle As String, _
    '                                          ByVal nameTitlePosition As String, _
    '                                          ByVal custSegment As String, _
    '                                          ByVal isClaim As Boolean) As String

    '    ' Logger.Debug("MakeLinkingCancelMessage_Start Param[" & customerName & ", " & customerNameTitle & ", " _
    '    '                                                    & nameTitlePosition & ", " & custSegment & "]")

    '    Dim message As New StringBuilder
    '    ' Logger.Debug("MakeLinkingCancelMessage_001" & "Call_Start WebWordUtility.GetWord Param[" & ReceptionistId & ",36]")
    '    Dim wordRequestNotice As String = WebWordUtility.GetWord(ReceptionistId, 36)
    '    ' Logger.Debug("MakeLinkingCancelMessage_001" & "Call_Start WebWordUtility.GetWord Ret[" & wordRequestNotice & "]")

    '    '苦情がある場合「！」を先頭に追加
    '    If isClaim Then
    '        ' Logger.Debug("MakeLinkingCancelMessage_001_1" & "Call_Start WebWordUtility.GetWord Param[" & ReceptionistId & ",54]")
    '        Dim claimExclamation As String = WebWordUtility.GetWord(ReceptionistId, 54)
    '        ' Logger.Debug("MakeLinkingCancelMessage_001_1" & "Call_Start WebWordUtility.GetWord Ret[" & claimExclamation & "]")
    '        message.Append(claimExclamation)
    '    End If

    '    Dim customerNameData As String

    '    If Not IsDBNull(customerName) AndAlso Not String.IsNullOrEmpty(customerName) AndAlso Not String.IsNullOrEmpty(customerName.Trim()) Then
    '        customerNameData = customerName

    '        'メッセージの組み立て
    '        With message
    '            If nameTitlePosition.Equals(NameTitlePositionFront) Then
    '                .Append(customerNameTitle)
    '                .Append(customerNameData)
    '            Else
    '                .Append(customerNameData)
    '                .Append(customerNameTitle)
    '            End If
    '        End With
    '    Else
    '        If Not IsDBNull(custSegment) AndAlso Not String.IsNullOrEmpty(custSegment) Then
    '            ' Logger.Debug("MakeLinkingCancelMessage_002" & "Call_Start WebWordUtility.GetWord Param[" & ReceptionistId & ",31]")
    '            Dim wordUnknown As String = WebWordUtility.GetWord(ReceptionistId, 31)
    '            ' Logger.Debug("MakeLinkingCancelMessage_002" & "Call_Start WebWordUtility.GetWord Ret[" & wordUnknown & "]")
    '            customerNameData = wordUnknown
    '        Else
    '            ' Logger.Debug("MakeLinkingCancelMessage_003" & "Call_Start WebWordUtility.GetWord Param[" & ReceptionistId & ",32]")
    '            Dim wordNewCustomer As String = WebWordUtility.GetWord(ReceptionistId, 32)
    '            ' Logger.Debug("MakeLinkingCancelMessage_003" & "Call_Start WebWordUtility.GetWord Ret[" & wordNewCustomer & "]")
    '            customerNameData = wordNewCustomer
    '        End If

    '        message.Append(customerNameData)
    '    End If

    '    message.Append(" ")
    '    message.Append(wordRequestNotice)

    '    ' Logger.Debug("MakeLinkingCancelMessage_End Ret[" & message.ToString() & "]")
    '    Return message.ToString()
    'End Function

#End Region

#Region "紐付け解除メッセージのPUSH送信"

    ' ''' <summary>
    ' ''' 紐付け解除メッセージのPUSH送信
    ' ''' </summary>
    ' ''' <param name="sendAccount">送信先アカウント</param>
    ' ''' <param name="message">メッセージ</param>
    ' ''' <remarks></remarks>
    'Private Sub PushLinkingCancelMessage(ByVal sendAccount As String, ByVal message As String)
    '    Dim postMessage As New StringBuilder

    '    ' Logger.Debug("PushLinkingCancelMessage_Start Param[" & sendAccount & ", " & message & "]")

    '    'POST送信する文字列を作成する。
    '    With postMessage
    '        .Append("cat=popup")
    '        .Append("&type=header")
    '        .Append("&sub=text")
    '        .Append("&uid=")
    '        .Append(sendAccount)
    '        .Append("&time=3")
    '        .Append("&color=C8E8FF64")
    '        .Append("&height=50")
    '        .Append("&width=1024")
    '        .Append("&pox=0")
    '        .Append("&msg=")
    '        .Append(message)
    '        .Append("&js1=icropScript.ui.setVisitor()")
    '        .Append("&js2=icropScript.ui.openVisitorListDialog()")
    '    End With

    '    ' PUSH送信文字列を保持する
    '    If Me.postMessageList Is Nothing Then
    '        Me.postMessageList = New List(Of String)
    '    End If

    '    Me.postMessageList.Add(postMessage.ToString)

    '    ' Logger.Debug("PushLinkingCancelMessage_End Ret[]")
    'End Sub

#End Region
    ' $01 end   複数顧客に対する商談平行対応

    '$02 start 新車タブレットショールーム管理機能開発
#Region "接客不要情報登録"
    '$08 start NSK s.natsume TKM Change request development for Next Gen e-CRB (CR060)
    ' ''' <summary>
    ' ''' 接客不要情報を登録
    ' ''' </summary>
    ' ''' <param name="visitSequence">来店実績連番</param>
    ' ''' <param name="customerSegment">顧客区分</param>
    ' ''' <param name="tentativeName">仮登録氏名</param>
    ' ''' <param name="oldSalesTableNo">更新前商談テーブルNo.</param>
    ' ''' <param name="newSalesTableNo">更新後商談テーブルNo.</param>
    ' ''' <param name="updateAccount">ユーザーアカウント</param>
    ' ''' <returns>登録結果</returns>
    ' ''' <remarks></remarks>
    '<EnableCommit()>
    'Public Function RegistrationUnNecessary(ByVal visitSequence As Long, ByVal customerSegment As String, _
    '                                       ByVal tentativeName As String, ByVal oldSalesTableNo As Integer, _
    '                                       ByVal newSalesTableNo As Integer, ByVal updateAccount As String) As Integer _
    '     Implements ISC3100101BusinessLogic.RegistrationUnNecessary
    ''' <summary>
    ''' 接客不要情報を登録
    ''' </summary>
    ''' <param name="visitSequence">来店実績連番</param>
    ''' <param name="customerSegment">顧客区分</param>
    ''' <param name="tentativeName">仮登録氏名</param>
    ''' <param name="oldSalesTableNo">更新前商談テーブルNo.</param>
    ''' <param name="newSalesTableNo">更新後商談テーブルNo.</param>
    ''' <param name="telNumber">電話番号</param>
    ''' <param name="updateAccount">ユーザーアカウント</param>
    ''' <returns>登録結果</returns>
    ''' <remarks></remarks>
    ''' <history>
    ''' 2019/11/12 NSK 鈴木 (トライ店システム評価)次世代セールス基盤：ログ出力機能における保守性向上検証
    ''' </history>
    <EnableCommit()>
    Public Function RegistrationUnNecessary(ByVal visitSequence As Long, ByVal customerSegment As String, _
                                           ByVal tentativeName As String, ByVal oldSalesTableNo As Integer, _
                                           ByVal newSalesTableNo As Integer, ByVal updateAccount As String, _
                                           ByVal telNumber As String) As Integer _
         Implements ISC3100101BusinessLogic.RegistrationUnNecessary

        '$08 end NSK s.natsume TKM Change request development for Next Gen e-CRB (CR060)

        Dim startLogString As New StringBuilder
        With startLogString
            .Append("RegistrationUnNecessary_Start Param[")
            .Append(visitSequence)
            .Append(", ")
            .Append(customerSegment)
            .Append(", ")
            .Append(tentativeName)
            .Append(", ")
            .Append(oldSalesTableNo)
            .Append(", ")
            .Append(newSalesTableNo)
            .Append(", ")
            .Append(updateAccount)
            .Append("]")
        End With
        Logger.Info(startLogString.ToString)

        Try
            Dim registUnNecessaryResult As Integer = MessageIdNormal

            '仮登録氏名・商談テーブルNo.の更新
            '$08 start NSK s.natsume TKM Change request development for Next Gen e-CRB (CR060)
            'registUnNecessaryResult = UpdateTentativeNameAndSalesTable(visitSequence, _
            '                                                            customerSegment, _
            '                                                            tentativeName, _
            '                                                            updateAccount, _
            '                                                            oldSalesTableNo, _
            '                                                            newSalesTableNo)
            registUnNecessaryResult = UpdateTentativeNameAndSalesTable(visitSequence, _
                                                            customerSegment, _
                                                            tentativeName, _
                                                            updateAccount, _
                                                            oldSalesTableNo, _
                                                            newSalesTableNo, _
                                                            telNumber)
            '$08 end NSK s.natsume TKM Change request development for Next Gen e-CRB (CR060)

            '仮登録氏名更新または商談テーブルNo.更新に失敗
            If Not MessageIdNormal.Equals(registUnNecessaryResult) Then
                Me.Rollback = True
                Logger.Error("RegistrationUnNecessary_End Ret[" & registUnNecessaryResult & "]")
                Return registUnNecessaryResult
            End If

            ' 接客不要更新
            Dim result As Boolean
            Using tableAdapter As New SC3100101TableAdapter
                result = tableAdapter.UpdateUnNecessary(visitSequence, updateAccount)
            End Using

            If Not result Then
                Me.Rollback = True
                'メッセージIDを更新
                Logger.Error("RegistrationUnNecessary_End Ret[" & MessageIdExclusionFailed & "]")
                Return MessageIdExclusionFailed
            End If

            ReceptionUpdate(OperationNo)

            Dim endLogString As New System.Text.StringBuilder
            endLogString.Append("RegistrationUnNecessary_End Ret[")
            endLogString.Append(registUnNecessaryResult)
            endLogString.Append("]")
            Logger.Info(endLogString.ToString)
            Return registUnNecessaryResult

        Catch oraEx As OracleExceptionEx

            If oraEx.Number = OracleErrorTimeOut Then
                Logger.Error(New System.Text.StringBuilder("RegistrationUnNecessary DataBaseTimeOut").ToString)
                'ロールバックを行う。
                Me.Rollback = True

                'ログ出力
                Logger.Error(CType(MessageIdTimeOut, String), oraEx)

                'DBタイムアウトエラー時
                Logger.Error(New System.Text.StringBuilder("RegistrationUnNecessary_End Ret[").Append(MessageIdTimeOut).Append("]").ToString)

                Return MessageIdTimeOut
            Else
                '上記以外のエラーは基盤側で制御
                Throw
            End If
        End Try

    End Function

#End Region

#Region "接客情報取得"
    ''' <summary>
    ''' 各接客状況エリア情報取得
    ''' </summary>
    ''' <param name="dealerCode">販売店コード</param>
    ''' <param name="storeCode">店舗コード</param>
    ''' <param name="areaCode">取得区分</param>
    ''' <param name="nowDate">現在日時</param>
    ''' <param name="claimVisitSequenceList">苦情情報来店実績連番リスト</param>
    ''' <returns>エリア毎の状況</returns>
    Public Function GetReceptionInfo(ByVal dealerCode As String, _
                                 ByVal storeCode As String, _
                                 ByVal areaCode As String,
                                 ByVal nowDate As Date, _
                                 ByVal claimVisitSequenceList As List(Of Long)) As SC3100101DataSet.SC3100101ReceptionInfoDataTable

        Dim startLogString As New System.Text.StringBuilder
        startLogString.Append("GetSalesInfo_Start Param[")
        startLogString.Append(dealerCode)
        startLogString.Append(", ")
        startLogString.Append(storeCode)
        startLogString.Append(", ")
        startLogString.Append(areaCode)
        startLogString.Append(", ")
        startLogString.Append(claimVisitSequenceList.ToString)
        startLogString.Append("]")
        Logger.Info(startLogString.ToString)

        ' 開始日時(当日の0:00:00)を格納
        Dim startDate As Date = nowDate.Date
        ' 終了日時(当日の23:59:59)を格納
        Dim nextDate As Date = startDate.AddDays(NextDay)
        Dim endDate As Date = nextDate.AddSeconds(BeforMillisecond)
        nextDate = Nothing

        Dim salesInfoDataTable As SC3100101DataSet.SC3100101ReceptionInfoDataTable = Nothing

        Using tableAdapter As New SC3100101TableAdapter
            salesInfoDataTable = tableAdapter.GetSalesInfo(dealerCode, storeCode, areaCode, startDate, endDate)
        End Using

        '苦情情報の取得
        For Each salesInfoRow As SC3100101DataSet.SC3100101ReceptionInfoRow In salesInfoDataTable

            ' 苦情情報が存在する場合
            If claimVisitSequenceList.Contains(salesInfoRow.VISITSEQ) Then

                salesInfoRow.CLAIMFLG = ClaimFlagExists

            End If

        Next

        ' 商談中エリアの場合、依頼情報を紐付ける
        If areaCode = ReceptionClassNegotiation Then

            salesInfoDataTable = MargeNoticeInfo(salesInfoDataTable, dealerCode, storeCode, startDate, endDate)

        End If

        Dim endLogString As New System.Text.StringBuilder
        endLogString.Append("GetSalesInfo_End Ret[")
        endLogString.Append(salesInfoDataTable.ToString)
        endLogString.Append("]")
        Logger.Info(endLogString.ToString)
        Return salesInfoDataTable
    End Function

    ''' <summary>
    ''' 依頼種別をマージする
    ''' </summary>
    ''' <param name="salesInfoDataTable">接客情報テーブル</param>
    ''' <param name="dealerCode">販売店コード</param>
    ''' <param name="storeCode">店舗コード</param>
    ''' <param name="startDate">開始日時</param>
    ''' <param name="endDate">終了日時</param>
    ''' <returns>マージしたDataSet</returns>
    Public Function MargeNoticeInfo(ByVal salesInfoDataTable As SC3100101DataSet.SC3100101ReceptionInfoDataTable, _
                                 ByVal dealerCode As String, _
                                 ByVal storeCode As String, _
                                 ByVal startDate As Date, _
                                 ByVal endDate As Date) As SC3100101DataSet.SC3100101ReceptionInfoDataTable

        Using dataAdapter As New SC3100101TableAdapter

            Dim noticeReq As String = Nothing
            Dim lastStatusList As List(Of String) = Nothing

            '通知依頼種別（査定）を設定
            noticeReq = NoticeAssessment
            lastStatusList = New List(Of String)
            lastStatusList.Add(NoticeStatus)


            '通知依頼（査定）の取得
            Using noticeRequestsDataTable As SC3100101DataSet.VisitReceptionNoticeRequestsDataTable = _
                             dataAdapter.GetNoticeRequests(dealerCode, storeCode, _
                                                           startDate, endDate, _
                                                           noticeReq, lastStatusList)

                If noticeRequestsDataTable.Rows.Count > 0 Then

                    '商談件数だけループ
                    For Each salesRow As SC3100101DataSet.SC3100101ReceptionInfoRow In salesInfoDataTable

                        '通知データ情報の数だけループ
                        For Each noticeRequestsRow As SC3100101DataSet.VisitReceptionNoticeRequestsRow In noticeRequestsDataTable

                            '送信日時を設定
                            If (noticeRequestsRow.ACCOUNT.Equals(salesRow.ACCOUNT)) And (noticeRequestsRow.CUSTID.Equals(salesRow.CUSTID)) Then

                                salesRow.REQUESTASSESSMENTDATE = noticeRequestsRow.SENDDATE
                                Exit For

                            End If
                        Next
                    Next

                End If

            End Using

            lastStatusList = Nothing

            '通知依頼種別（価格相談）を設定
            noticeReq = NoticePriceConsultation
            lastStatusList = New List(Of String)
            lastStatusList.Add(NoticeStatus)
            lastStatusList.Add(ReceiveStatus)

            '通知依頼（価格相談）の取得
            Using noticeRequestsDataTable As SC3100101DataSet.VisitReceptionNoticeRequestsDataTable = _
                             dataAdapter.GetNoticeRequests(dealerCode, storeCode, _
                                                           startDate, endDate, _
                                                           noticeReq, lastStatusList)

                If noticeRequestsDataTable.Rows.Count > 0 Then

                    '商談件数だけループ
                    For Each salesRow As SC3100101DataSet.SC3100101ReceptionInfoRow In salesInfoDataTable

                        '通知データ情報の数だけループ
                        For Each noticeRequestsRow As SC3100101DataSet.VisitReceptionNoticeRequestsRow In noticeRequestsDataTable

                            '送信日時を設定
                            If (noticeRequestsRow.ACCOUNT.Equals(salesRow.ACCOUNT)) And (noticeRequestsRow.CUSTID.Equals(salesRow.CUSTID)) Then

                                salesRow.REQUESTPRICECONSULTATIONDATE = noticeRequestsRow.SENDDATE
                                Exit For

                            End If
                        Next
                    Next

                End If

            End Using

            lastStatusList = Nothing

            '通知依頼種別（ヘルプ）を設定
            noticeReq = NoticeHelp
            lastStatusList = New List(Of String)
            lastStatusList.Add(NoticeStatus)

            '通知依頼（ヘルプ）の取得
            Using noticeRequestsDataTable As SC3100101DataSet.VisitReceptionNoticeRequestsDataTable = _
                             dataAdapter.GetNoticeRequests(dealerCode, storeCode, _
                                                           startDate, endDate, _
                                                           noticeReq, lastStatusList)

                If noticeRequestsDataTable.Rows.Count > 0 Then

                    '商談件数だけループ
                    For Each salesRow As SC3100101DataSet.SC3100101ReceptionInfoRow In salesInfoDataTable

                        '通知データ情報の数だけループ
                        For Each noticeRequestsRow As SC3100101DataSet.VisitReceptionNoticeRequestsRow In noticeRequestsDataTable

                            '送信日時を設定
                            If (noticeRequestsRow.ACCOUNT.Equals(salesRow.ACCOUNT)) And (noticeRequestsRow.CUSTID.Equals(salesRow.CUSTID)) Then

                                salesRow.REQUESTHELPDATE = noticeRequestsRow.SENDDATE
                                Exit For

                            End If
                        Next
                    Next

                End If

            End Using

        End Using

        Return salesInfoDataTable

    End Function

#End Region

#Region "アンドン情報の取得"
    '$07 NSK s.natsume TKM Change request development for Next Gen e-CRB (CR075) START
    ' ''' <summary>
    ' ''' アンドン件数を取得する
    ' ''' </summary>
    ' ''' <param name="dealerCode">販売店コード</param>
    ' ''' <param name="storeCode">店舗コード</param>
    ' ''' <param name="nowDate">現在日時</param>
    ' ''' <returns>アンドン情報</returns>
    ' ''' <remarks></remarks>
    'Public Function GetBoardInfo(ByVal dealerCode As String, _
    '                                    ByVal storeCode As String, _
    '                                    ByVal nowDate As Date) As SC3100101DataSet.SC3100101BoardInfoDataTable
    ''' <summary>
    ''' アンドン件数を取得する
    ''' </summary>
    ''' <param name="dealerCode">販売店コード</param>
    ''' <param name="storeCode">店舗コード</param>
    ''' <param name="nowDate">現在日時</param>
    ''' <param name="afterOrderActionCodeDelivery">納車活動を示す受注後活動コード</param>
    ''' <returns>アンドン情報</returns>
    ''' <remarks></remarks>
    Public Function GetBoardInfo(ByVal dealerCode As String, _
                                        ByVal storeCode As String, _
                                        ByVal nowDate As Date, _
                                        ByVal afterOrderActionCodeDelivery As String) As SC3100101DataSet.SC3100101BoardInfoDataTable
        '$07 NSK s.natsume TKM Change request development for Next Gen e-CRB (CR075) END

        Dim startLogString As New System.Text.StringBuilder
        startLogString.Append("GetBordIInfo_Start Param[")
        startLogString.Append(dealerCode)
        startLogString.Append(", ")
        startLogString.Append(storeCode)
        startLogString.Append("]")
        Logger.Info(startLogString.ToString)

        '開始日時(当日の0:00:00)を格納
        Dim startDate As Date = nowDate.Date
        '終了日時(当日の23:59:59)を格納
        Dim nextDate As Date = startDate.AddDays(NextDay)
        Dim endDate As Date = nextDate.AddSeconds(BeforMillisecond)
        nextDate = Nothing

        Using bordInfoDataset As New SC3100101DataSet.SC3100101BoardInfoDataTable
            Dim bordInfo As SC3100101DataSet.SC3100101BoardInfoRow = _
                bordInfoDataset.NewSC3100101BoardInfoRow()

            Using AndonAdapter As New SC3100101TableAdapter

                '$06 NSK s.natsume TKM Change request development for Next Gen e-CRB (CR008) START
                '' 査定件数
                'Using appraisalCount As SC3100101DataSet.SC3100101BordCountDataTable = _
                '    AndonAdapter.GetAssessmentCount(dealerCode, storeCode, startDate, endDate)
                '    bordInfo.APPRAISALCOUNT = CShort(appraisalCount.Item(0)(0))
                'End Using
                '$06 NSK s.natsume TKM Change request development for Next Gen e-CRB (CR008) END

                ' 来店件数
                Using visitCount As SC3100101DataSet.SC3100101BordCountDataTable = _
                AndonAdapter.GetVisitCustCount(dealerCode, storeCode, startDate, endDate)
                    bordInfo.VISITORCOUNT = CShort(visitCount.Item(0)(0))
                End Using

                ' 見積もり件数
                Using estimationCount As SC3100101DataSet.SC3100101BordCountDataTable = _
                AndonAdapter.GetEstimateCount(dealerCode, storeCode, startDate, endDate)
                    bordInfo.ESTIMATIONCOUNT = CShort(estimationCount.Item(0)(0))
                End Using

                ' 商談件数
                Using negotiationCount As SC3100101DataSet.SC3100101BordCountDataTable = _
                    AndonAdapter.GetSalesCount(dealerCode, storeCode, startDate, endDate)
                    bordInfo.NEGOTIATIONCOUNT = CShort(negotiationCount.Item(0)(0))
                End Using

                ' 受注件数
                Using acceptionOrderCount As SC3100101DataSet.SC3100101BordCountDataTable = _
                AndonAdapter.GetConclusionCount(dealerCode, storeCode, startDate, endDate)
                    bordInfo.ACCEPTIONORDERCOUNT = CShort(acceptionOrderCount.Item(0)(0))
                End Using

                ' $03 start 納車作業対応 
                ' 納車件数
                '$07 NSK s.natsume TKM Change request development for Next Gen e-CRB (CR075) START
                'Using deliverlyCount As SC3100101DataSet.SC3100101BordCountDataTable = _
                '        AndonAdapter.GetDeliverlyCount(dealerCode, storeCode, startDate, endDate)
                '    bordInfo.DELIVERLYCOUNT = CShort(deliverlyCount.Item(0)(0))
                'End Using
                Using deliverlyCount As SC3100101DataSet.SC3100101BordCountDataTable = _
                AndonAdapter.GetDeliverlyCount(dealerCode, storeCode, startDate, endDate, afterOrderActionCodeDelivery)
                    bordInfo.DELIVERLYCOUNT = CShort(deliverlyCount.Item(0)(0))
                End Using
                '$07 NSK s.natsume TKM Change request development for Next Gen e-CRB (CR075) END
                ' $03 end   納車作業対応 


            End Using

            bordInfoDataset.AddSC3100101BoardInfoRow(bordInfo)

            Dim endLogString As New System.Text.StringBuilder
            endLogString.Append("GetBordIInfo_End Ret[")
            endLogString.Append(bordInfoDataset.ToString)
            endLogString.Append("]")
            Logger.Info(endLogString.ToString)

            Return bordInfoDataset
        End Using
    End Function

#End Region

#Region "スタッフ通知依頼情報の取得"

    ''' <summary>
    ''' スタッフ通知依頼情報の取得
    ''' </summary>
    ''' <param name="visitSeq">シーケンス番号</param>
    ''' <returns>取得結果</returns>
    ''' <remarks></remarks>
    Public Function GetStaffNoticeRequest(ByVal visitSeq As Long) _
                        As SC3100101DataSet.VisitReceptionStaffNoticeRequestDataTable

        Dim startLogString As New System.Text.StringBuilder
        startLogString.Append("GetStaffNoticeRequest_Start Param[")
        startLogString.Append(visitSeq)
        startLogString.Append("]")
        Logger.Info(startLogString.ToString)

        'マージ用DataSet
        Using mergeDataSet As New SC3100101DataSet.VisitReceptionStaffNoticeRequestDataTable

            '依頼、受信両方のステータスを対応させる
            Dim statusList As New List(Of String)
            statusList.Add(NoticeStatus)


            Using dataAdapter As New SC3100101TableAdapter

                '査定依頼情報の取得
                Using noticeDataSet As SC3100101DataSet.VisitReceptionStaffNoticeRequestDataTable = _
                    dataAdapter.GetStaffNoticeRequest(visitSeq, NoticeAssessment, statusList)
                    '取得したデータをマージさせる
                    mergeDataSet.Merge(noticeDataSet)
                End Using

                '受信を追加
                statusList.Add(ReceiveStatus)

                '価格相談依頼情報の取得
                Using priceConsultationDataSet As SC3100101DataSet.VisitReceptionStaffNoticeRequestDataTable = _
                    dataAdapter.GetStaffNoticeRequest(visitSeq, NoticePriceConsultation, statusList)
                    '取得したデータをマージさせる
                    mergeDataSet.Merge(priceConsultationDataSet)
                End Using

                'ヘルプでは受信がないので、予め削除する
                statusList.Remove(ReceiveStatus)

                'ヘルプ依頼情報の取得
                Using helpDataSet As SC3100101DataSet.VisitReceptionStaffNoticeRequestDataTable = _
                    dataAdapter.GetStaffNoticeRequest(visitSeq, NoticeHelp, statusList)
                    '取得したデータをマージさせる
                    mergeDataSet.Merge(helpDataSet)
                End Using

            End Using

            Dim view As DataView = mergeDataSet.DefaultView

            view.Sort = "SENDDATE ASC"

            Using retDataSet As New SC3100101DataSet.VisitReceptionStaffNoticeRequestDataTable

                For row As Integer = 0 To view.Count - 1
                    retDataSet.ImportRow(view.Item(row).Row)
                Next

                Dim endLogString As New System.Text.StringBuilder
                endLogString.Append("GetStaffNoticeRequest_End Ret[")
                endLogString.Append(retDataSet.ToString)
                endLogString.Append("]")
                Logger.Info(endLogString.ToString)

                Return retDataSet
            End Using
        End Using
    End Function
#End Region

#Region "スタッフ状況情報取得"
    ''' <summary>
    ''' スタッフ状況情報取得
    ''' </summary>
    ''' <param name="dealerCode">販売店コード</param>
    ''' <param name="storeCode">店舗コード</param>
    ''' <param name="nowDate">日時</param>
    ''' <returns>取得結果</returns>
    ''' <remarks></remarks>
    Public Function GetStaffSituationInfo(ByVal dealerCode As String, _
                                           ByVal storeCode As String, _
                                           ByVal nowDate As Date) As SC3100101DataSet.SC3100101StaffStatusDataTable

        Dim startLogString As New System.Text.StringBuilder
        startLogString.Append("GetStaffSituationInfo_Start Param[")
        startLogString.Append(dealerCode)
        startLogString.Append(", ")
        startLogString.Append(storeCode)
        startLogString.Append("]")
        Logger.Info(startLogString.ToString)

        '開始日時(当日の0:00:00)を格納
        Dim startDate As Date = nowDate.Date
        '終了日時(当日の23:59:59)を格納
        Dim nextDate As Date = startDate.AddDays(NextDay)
        Dim endDate As Date = nextDate.AddSeconds(BeforMillisecond)
        nextDate = Nothing

        Dim staffStatusDataSet As New SC3100101DataSet.SC3100101StaffStatusDataTable

        Using tableAdapter As New SC3100101TableAdapter

            ' スタンバイ
            Using StaffStatusStandbyDataTable As SC3100101DataSet.SC3100101StaffStatusDataTable = _
              tableAdapter.GetSalesStuffInfo(dealerCode, storeCode, StaffStatusStandby, startDate, endDate)
                staffStatusDataSet.Merge(StaffStatusStandbyDataTable)
            End Using

            ' 商談中
            Using staffStatusSalesDataTable As SC3100101DataSet.SC3100101StaffStatusDataTable = _
            tableAdapter.GetSalesStuffInfo(dealerCode, storeCode, StaffStatusNegotiate, startDate, endDate)
                staffStatusDataSet.Merge(staffStatusSalesDataTable)
            End Using

            ' 納車作業中
            Using StaffStatusDeliverlyDataTable As SC3100101DataSet.SC3100101StaffStatusDataTable = _
              tableAdapter.GetSalesStuffInfo(dealerCode, storeCode, StaffStatusDeliverly, startDate, endDate)
                staffStatusDataSet.Merge(StaffStatusDeliverlyDataTable)
            End Using

            ' 一時退席中
            Using StaffStatusWalkoutDataTable As SC3100101DataSet.SC3100101StaffStatusDataTable = _
              tableAdapter.GetSalesStuffInfo(dealerCode, storeCode, StaffStatusLeaving, startDate, endDate)
                staffStatusDataSet.Merge(StaffStatusWalkoutDataTable)
            End Using

            ' オフライン
            Using StaffStatusOfflineDataTable As SC3100101DataSet.SC3100101StaffStatusDataTable = _
              tableAdapter.GetSalesStuffInfo(dealerCode, storeCode, StaffStatusOffline, startDate, endDate)
                staffStatusDataSet.Merge(StaffStatusOfflineDataTable)
            End Using


            ' 紐付け人数の初期化
            For Each staffRow As SC3100101DataSet.SC3100101StaffStatusRow In staffStatusDataSet
                staffRow.VISITORLINKINGCOUNT = Nolinking
            Next

            '紐付け人数の取得
            Using StaffVisitorLinkingCountDataTable As SC3100101DataSet.VisitReceptionVisitorLinkingCountDataTable = _
                tableAdapter.GetVisitorLinkingCount(dealerCode, storeCode, startDate, endDate)

                If StaffVisitorLinkingCountDataTable.Rows.Count > 0 Then

                    'スタッフの数だけループ
                    For Each staffRow As SC3100101DataSet.SC3100101StaffStatusRow In staffStatusDataSet

                        '通知データ情報の数だけループ
                        For Each linkingCountRow As SC3100101DataSet.VisitReceptionVisitorLinkingCountRow In StaffVisitorLinkingCountDataTable

                            '紐付き人数を設定
                            If (linkingCountRow.ACCOUNT.Equals(staffRow.ACCOUNT)) Then

                                staffRow.VISITORLINKINGCOUNT = linkingCountRow.VISITORLINKINGCOUNT
                                Exit For

                            End If
                        Next
                    Next

                End If

            End Using

            'スタッフ状況データテーブルを返す
            Dim endLogString As New System.Text.StringBuilder
            endLogString.Append("GetStaffSituationInfo_End Ret[")
            endLogString.Append(staffStatusDataSet.ToString)
            endLogString.Append("]")
            Logger.Info(endLogString.ToString)

            Return staffStatusDataSet
        End Using
    End Function

#End Region

#Region "店舗苦情情報の取得"

    ''' <summary>
    ''' 店舗苦情情報の取得
    ''' </summary>
    ''' <param name="dealerCode">販売店コード</param>
    ''' <param name="storeCode">店舗コード</param>
    ''' <param name="nowDate">現在日時</param>
    ''' <param name="complaintDateCount">苦情表示日数</param>
    ''' <returns>店舗苦情情報</returns>
    ''' <remarks></remarks>
    Public Function GetClaimInfo(ByVal dealerCode As String, ByVal storeCode As String, _
                                 ByVal nowDate As Date, ByVal complaintDateCount As Long) As List(Of Long)

        Dim startLogString As New System.Text.StringBuilder
        startLogString.Append("GetClaimInfo_Start Param[")
        startLogString.Append(dealerCode)
        startLogString.Append(", ")
        startLogString.Append(storeCode)
        startLogString.Append(", ")
        startLogString.Append(nowDate)
        startLogString.Append(", ")
        startLogString.Append(complaintDateCount)
        startLogString.Append("]")
        Logger.Info(startLogString.ToString)

        '開始日時(当日の0:00:00)を格納
        Dim startDate As Date = nowDate.Date

        '終了日時(当日の23:59:59)を格納
        Dim nextDate As Date = startDate.AddDays(NextDay)
        Dim endDate As Date = nextDate.AddSeconds(BeforMillisecond)
        nextDate = Nothing

        '苦情表示期間の設定
        Dim completeDate As Date = startDate.AddDays(-complaintDateCount)

        '返却用DataSet
        Dim retDataSet As SC3100101DataSet.VisitReceptionClaimInfoDataTable = Nothing

        Using dataAdapter As New SC3100101TableAdapter

            '苦情情報の取得
            retDataSet = dataAdapter.GetClaimInfo(dealerCode, storeCode, startDate, endDate, completeDate)
        End Using

        Dim claimVisitSequenceList As New List(Of Long)

        For Each row As SC3100101DataSet.VisitReceptionClaimInfoRow In retDataSet
            claimVisitSequenceList.Add(row.VISITSEQ)
        Next

        Dim endLogString As New System.Text.StringBuilder
        endLogString.Append("GetClaimInfo_End Ret[")
        endLogString.Append(retDataSet.ToString)
        endLogString.Append("]")
        Logger.Info(endLogString.ToString)

        Return claimVisitSequenceList

    End Function
#End Region

#Region "お客様情報の取得"

    ''' <summary>
    ''' お客様情報の取得
    ''' </summary>
    ''' <param name="visitSequence">来店実績連番</param>
    ''' <returns>お客様情報データセット</returns>
    ''' <remarks></remarks>
    Public Function GetCustomerInfo(ByVal visitSequence As Long, _
                                    Optional ByVal visitStatus As String = Nothing) _
        As SC3100101DataSet.VisitReceptionVisitorCustomerDataTable

        Dim startLogString As New System.Text.StringBuilder
        startLogString.Append("GetCustomerInfo_Start Param[")
        startLogString.Append(visitSequence)
        startLogString.Append(", ")
        startLogString.Append(visitStatus)
        startLogString.Append("]")
        Logger.Info(startLogString.ToString)

        'お客様情報データテーブル
        Dim customerInfoDataSet As SC3100101DataSet.VisitReceptionVisitorCustomerDataTable = Nothing

        Using dataAdapter As New SC3100101TableAdapter

            '来店実績お客様情報取得
            customerInfoDataSet = dataAdapter.GetVisitorCustomer(visitSequence, visitStatus)
        End Using
        'お客様情報データテーブルを返す
        Dim endLogString As New System.Text.StringBuilder
        endLogString.Append("GetCustomerInfo_End Ret[")
        endLogString.Append(customerInfoDataSet)
        endLogString.Append("]")
        Logger.Info(endLogString.ToString)
        Return customerInfoDataSet
    End Function
#End Region

#Region "警告音出力フラグ取得"

    ''' <summary>
    ''' 警告音出力フラグ取得
    ''' </summary>
    ''' <param name="dealerCode">販売店コード</param>
    ''' <param name="storeCode">店舗コード</param>
    ''' <param name="operationCode">対象アカウントの権限</param>
    ''' <returns>1:出力あり、0:出力なし</returns>
    ''' <remarks></remarks>
    Public Function GetAlarmOutputFlg(ByVal dealerCode As String, _
                                       ByVal storeCode As String, _
                                       ByVal operationCode As Decimal) As String
        Dim startLogString As New System.Text.StringBuilder
        startLogString.Append("GetAlarmOutputFlag_Start Param[")
        startLogString.Append(dealerCode)
        startLogString.Append(", ")
        startLogString.Append(storeCode)
        startLogString.Append(", ")
        startLogString.Append(operationCode)
        startLogString.Append("]")
        Logger.Info(startLogString.ToString)

        '初期状態:読み取り専用
        Dim alarmOutputFlag As String = AlarmOutputOff

        '通知警告音出力権限コードリストの取得
        Dim branchEnvSet As New BranchEnvSetting
        Dim sysEnvSetNoticeAlarmCodeListRow As DlrEnvSettingDataSet.DLRENVSETTINGRow = Nothing

        Logger.Info("GetAlarmOutputFlag_002" & "Call_Start GetSystemEnvSetting Param[" & NoticeAlarmCodeList & "]")
        sysEnvSetNoticeAlarmCodeListRow = branchEnvSet.GetEnvSetting(dealerCode, storeCode, NoticeAlarmCodeList)
        Logger.Info("GetAlarmOutputFlag_002" & "Call_End GetSystemEnvSetting Ret[" & IsDBNull(sysEnvSetNoticeAlarmCodeListRow) & "]")

        ' 環境変数が設定されていない場合は警告音出力を行わない
        If sysEnvSetNoticeAlarmCodeListRow Is Nothing Then
            Return alarmOutputFlag
        End If

        Dim operationListName As String = sysEnvSetNoticeAlarmCodeListRow.PARAMVALUE
        Dim operationCdList As String()

        'カンマ区切りで取得
        operationCdList = operationListName.Split(CType(",", Char))

        For Each operation In operationCdList
            If CType(operation, Decimal) = operationCode Then

                '更新に切り替えてforを抜ける
                alarmOutputFlag = AlarmOutputOn
                Exit For
            End If
        Next

        Dim endLogString As New System.Text.StringBuilder
        endLogString.Append("GetAlarmOutputFlag_End Ret[")
        endLogString.Append(alarmOutputFlag)
        endLogString.Append("]")
        Logger.Info(endLogString.ToString)
        Return alarmOutputFlag
    End Function
#End Region

#Region "来店回数の取得"

    ' $04 start FollowUp-Box連番桁変更対応
    ''' <summary>
    ''' 来店回数の取得
    ''' </summary>
    ''' <param name="dealerCode">販売店コード</param>
    ''' <param name="storeCode">店舗コード</param>
    ''' <param name="followUpBoxSeqNo">内連番No.</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GetVisitCount(ByVal dealerCode As String, _
                                  ByVal storeCode As String, _
                                  ByVal followUpBoxSeqNo As Decimal _
                                 ) As SC3100101DataSet.VisitReceptionVisitCountDataTable
        ' $04 end FollowUp-Box連番桁変更対応

        Dim startLogString As New System.Text.StringBuilder
        startLogString.Append("GetVisitCount_Start Param[")
        startLogString.Append(dealerCode)
        startLogString.Append(", ")
        startLogString.Append(storeCode)
        startLogString.Append(", ")
        startLogString.Append(followUpBoxSeqNo)
        startLogString.Append("]")
        Logger.Info(startLogString.ToString)
        startLogString = Nothing

        '返却用DataSet
        Dim retDataSet As SC3100101DataSet.VisitReceptionVisitCountDataTable = Nothing

        Using adapter As New SC3100101TableAdapter

            '来店回数取得
            retDataSet = adapter.GetVisitCount(dealerCode, storeCode, followUpBoxSeqNo)
        End Using

        Dim endLogString As New System.Text.StringBuilder
        endLogString.Append("GetVisitCount_End ret[")
        endLogString.Append(retDataSet.ToString)
        endLogString.Append("]")

        Logger.Info(endLogString.ToString)
        Return retDataSet

    End Function

#End Region

    '$02 end 新車タブレットショールーム管理機能開発

#Region "受注後工程アイコンを取得"

    '$05 start
    ''' <summary>
    ''' 受注後工程アイコンを取得する
    ''' </summary>
    ''' <param name="dealerCode">販売店コード</param>
    ''' <returns>受注後工程アイコンテーブル</returns>
    ''' <remarks></remarks>
    Public Function GetAfterOrderProcessIcon(ByVal dealerCode As String) _
                                      As SC3100101DataSet.SC3100101AfterOrderProcessIconInfoDataTable

        Dim startLogString As New System.Text.StringBuilder
        startLogString.Append("GetAfterOrderProcessIcon_Start Param[dealerCode=")
        startLogString.Append(dealerCode)
        startLogString.Append("]")
        Logger.Info(startLogString.ToString)

        Dim retDataSet As SC3100101DataSet.SC3100101AfterOrderProcessIconInfoDataTable = Nothing
        Using adapter As New SC3100101TableAdapter
            retDataSet = adapter.GetAfterOrderProcessIcon(dealerCode)
        End Using

        Dim endLogString As New System.Text.StringBuilder
        endLogString.Append("GetAfterOrderProcessIcon_End Ret[RecordCount=")
        endLogString.Append(retDataSet.Count)
        endLogString.Append("]")
        Logger.Info(endLogString.ToString)
        Return retDataSet
    End Function
    '$05 end
#End Region

    '$08 start NSK s.natsume TKM Change request development for Next Gen e-CRB (CR060)
#Region "仮登録氏名入力チェック"

    ''' <summary>
    ''' 仮登録氏名入力チェック
    ''' </summary>
    ''' <param name="tentativeName">仮登録氏名</param>
    ''' <param name="tentativeNameCharacterTypes">名前に使用可能な文字種(正規表現)</param>
    ''' <returns>メッセージID</returns>
    ''' <remarks></remarks>
    Public Function TentativeNameValidationCheck(ByVal tentativeName As String, _
                                           ByVal tentativeNameCharacterTypes As String) As Integer

        Dim messageID As Integer = MessageIdNormal

        Dim regexString As String = "[^ " & tentativeNameCharacterTypes & "]"

        If (Regex.IsMatch(tentativeName, regexString)) Then
            '使用不可能な文字種がある場合
            messageID = MessageIdCharacterTypes
        Else
            Dim wordCount As Integer = 0
            Dim nameWordList As String() = tentativeName.Trim.Split(CChar(" "))

            ' 名前の単語数をカウントする(連続スペースは無視する)
            For Each nameWord As String In nameWordList
                If (Not String.IsNullOrEmpty(nameWord)) Then
                    wordCount = wordCount + 1
                End If
            Next

            If (3 < wordCount) Then
                '3単語より多い場合
                messageID = MessageIdName3Words
            End If
        End If

        Return messageID
    End Function
#End Region

#Region "電話番号入力チェック"

    ''' <summary>
    ''' 電話番号入力チェック
    ''' </summary>
    ''' <param name="telNumber">電話番号</param>
    ''' <param name="telNumberCharacterTypes">電話番号で使用可能な文字種(正規表現)</param>
    ''' <returns>メッセージID</returns>
    ''' <remarks></remarks>
    Public Function TelNumberValidationCheck(ByVal telNumber As String, _
                                       ByVal telNumberCharacterTypes As String) As Integer

        Dim messageID As Integer = MessageIdNormal

        Dim regexString As String = "[^" & telNumberCharacterTypes & "]"

        If (Regex.IsMatch(telNumber, regexString)) Then
            '使用不可能な文字種がある場合
            messageID = MessageIdCharacterTypes
        Else
            If (telNumber.Length < 10 OrElse 13 < telNumber.Length) Then
                '文字数が10未満もしくは13より多い場合
                messageID = MessageIdTelNumberLength
            End If
        End If

        Return messageID
    End Function
#End Region
    '$08 end NSK s.natsume TKM Change request development for Next Gen e-CRB (CR060)

End Class
