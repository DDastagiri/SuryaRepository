
'━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
'VisitReceptionBusinessLogic.vb
'──────────────────────────────────
'機能： 
'補足： 
'作成： -
'更新： 2013/09/26 TMEJ t.shimamura 新車タブレット受付画面管理指標の変更対応 $01
'更新： 2013/10/15 TMEJ m.asano   次世代e-CRBセールス機能 新DB適応に向けた機能開発 $02
'更新： 2015/11/10 TMEJ t.komure  (トライ店システム評価)SMBチップ検索の絞り込み方法変更 $03
'更新： 2019/11/12 NSK 鈴木 (トライ店システム評価)次世代セールス基盤：ログ出力機能における保守性向上検証
'──────────────────────────────────

Imports Toyota.eCRB.SystemFrameworks.Core
Imports System.Web
Imports Toyota.eCRB.Visit.Api.DataAccess.VisitReceptionDataSet
Imports Toyota.eCRB.Visit.Api.DataAccess.VisitReceptionDataSetTableAdapters
Imports Toyota.eCRB.Visit.Api.DataAccess
Imports Toyota.eCRB.SystemFrameworks.Core.iCROP.BizLogic
Imports Toyota.eCRB.SystemFrameworks.Core.iCROP.DataAccess
Imports System.Text
Imports System.Net
Imports Toyota.eCRB.Tool.Notify.Api.BizLogic
Imports Toyota.eCRB.Tool.Notify.Api.DataAccess
Imports Toyota.eCRB.Tool.Notify.Api.DataAccess.ConstCode
Imports Toyota.eCRB.Visit.Api.DataAccess.VisitUtilityDataSet
Imports Toyota.eCRB.Visit.Api.DataAccess.VisitUtilityDataSetTableAdapters
Imports Toyota.eCRB.SystemFrameworks.Web
Imports Toyota.eCRB.Visit.Api.BizLogic
Imports Toyota.eCRB.Common.VisitResult.DataAccess.UpdateSalesVisitDataSetTableAdapters
Imports System.Globalization

' 2019/11/12 NSK 鈴木 (トライ店システム評価)次世代セールス基盤：ログ出力機能における保守性向上検証 START
Imports Toyota.eCRB.SystemFrameworks.Core.iCROP.DataAccess.SystemSettingDataSet
' 2019/11/12 NSK 鈴木 (トライ店システム評価)次世代セールス基盤：ログ出力機能における保守性向上検証 END

''' <summary>
''' 来店機能の受付処理共通ロジックです。
''' </summary>
''' <remarks></remarks>
Public Class VisitReceptionBusinessLogic
    Inherits BaseBusinessComponent


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

    ''' <summary>
    ''' 削除フラグ（未削除）
    ''' </summary>
    ''' <remarks></remarks>
    Private Const DeleteFlagNotDelete As String = "0"

    ''' <summary>
    ''' 正常
    ''' </summary>
    ''' <remarks></remarks>
    Private Const MessageIdNormal As Integer = 0

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
    ''' 苦情存在フラグ（存在する）
    ''' </summary>
    ''' <remarks></remarks>
    Private Const ClaimFlagExists As String = "1"

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
    ''' ブロードキャストフラグ：未送信
    ''' </summary>
    ''' <remarks></remarks>
    Private Const BroudcastFlagUnsend As String = "0"

    ''' <summary>
    ''' ブロードキャストフラグ：対象外
    ''' </summary>
    ''' <remarks></remarks>
    Private Const BroudcastFlagNotTarget As String = "9"


    ''' <summary>
    ''' ゲートキーパーID
    ''' </summary>
    ''' <remarks></remarks>
    Private Const GateKeeperID As String = "SC3090301"

    ''' <summary>
    ''' 在席状態(大分類)：スタンバイ
    ''' </summary>
    ''' <remarks></remarks>
    Private Const PresenceCategoryStandby As String = "1"

    ''' <summary>
    ''' 在席状態(大分類)：商談中
    ''' </summary>
    ''' <remarks></remarks>
    Private Const PresenceCategoryNegotiate As String = "2"

    ''' <summary>
    ''' 在席状態(大分類)：退席中
    ''' </summary>
    ''' <remarks></remarks>
    Private Const PresenceCategoryLeaving As String = "3"

    ''' <summary>
    ''' 在席状態(大分類)：オフライン
    ''' </summary>
    ''' <remarks></remarks>
    Private Const PresenceCategoryOffline As String = "4"

    ''' <summary>
    ''' 削除フラグ（未削除）
    ''' </summary>
    ''' <remarks></remarks>
    Private Const DeleteFlagNone As String = "0"

    ''' <summary>
    ''' 送信タイプ：顧客担当SS
    ''' </summary>
    ''' <remarks></remarks>
    Private Const SendTypeCsutSs As Integer = 1

    ''' <summary>
    ''' 権限コード：受付係
    ''' </summary>
    ''' <remarks></remarks>
    Private Const OperationCodeSlr As Integer = 51

    ''' <summary>
    ''' 権限コード：セールスマネージャー
    ''' </summary>
    ''' <remarks></remarks>
    Private Const OperationCodeSsm As Integer = 7

    ''' <summary>
    ''' 送信タイプ：受付係
    ''' </summary>
    ''' <remarks></remarks>
    Private Const SendTypeSlr As Integer = 3

    ''' <summary>
    ''' 送信タイプ：セールスマネージャー
    ''' </summary>
    ''' <remarks></remarks>
    Private Const SendTypeSsm As Integer = 2

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
    ''' オラクルエラーコード:タイムアウト
    ''' </summary>
    ''' <remarks></remarks>
    Private Const ErrorCodeOra2049 As Integer = 2049

    ''' <summary>
    ''' 来店実績ステータス:調整中
    ''' </summary>
    ''' <remarks></remarks>
    Private Const VisitStatusAdjust As String = "03"


    ''' <summary>
    ''' Push送信タイプ：新規
    ''' </summary>
    ''' <remarks></remarks>
    Private Const PushTypeNew As Integer = 1

    ''' <summary>
    ''' Push送信タイプ：自社客・未取引客(担当スタッフあり・ステータスオフライン以外)
    ''' </summary>
    ''' <remarks></remarks>
    Private Const PushTypeOrgOrNewCustomerOffline As Integer = 2

    ''' <summary>
    ''' Push送信タイプ：自社客・未取引客(担当スタッフなし・ステータスオフライン)
    ''' </summary>
    ''' <remarks></remarks>
    Private Const PushTypeOrgOrNewCustomerNotOffline As Integer = 3

    ''' <summary>
    ''' Push送信タイプ：自社客・未取引客(担当スタッフなし)
    ''' </summary>
    ''' <remarks></remarks>
    Private Const PushTypeOrgOrNewCustomerNotStuff As Integer = 4

    ''' <summary>
    ''' Push送信タイプ：顧客情報なし
    ''' </summary>
    ''' <remarks></remarks>
    Private Const PushTypeNotCustomerInfo As Integer = 5

    ''' <summary>
    ''' システム管理マスタ.パラメータ名:敬称表示位置
    ''' </summary>
    ''' <remarks></remarks>
    Private Const KeisyoZengo As String = "KEISYO_ZENGO"

    ''' <summary>
    ''' 敬称表示位置:前
    ''' </summary>
    ''' <remarks></remarks>
    Private Const HonorificTitleMae As String = "1"

    ''' <summary>
    ''' 敬称表示位置:後
    ''' </summary>
    ''' <remarks></remarks>
    Private Const HonorificTitleUshiro As String = "2"
    ''' <summary>
    ''' 権限コード：ウェルカムボード
    ''' </summary>
    ''' <remarks></remarks>
    Private Const OperationCodeWB As Integer = 61

    ''' <summary>
    ''' Push種別：新規顧客
    ''' </summary>
    ''' <remarks></remarks>
    Private Const CustomerClassNew As String = "0"

    ''' <summary>
    ''' Push種別：既存顧客
    ''' </summary>
    ''' <remarks></remarks>
    Private Const CustomerClassOrginal As String = "1"

    ''' <summary>
    ''' 顧客種別：自社客
    ''' </summary>
    ''' <remarks></remarks>
    Private Const OriginalCustomer As String = "1"

    ''' <summary>
    ''' 顧客種別：未取引客
    ''' </summary>
    ''' <remarks></remarks>
    Private Const NewCustomer As String = "2"

    ''' <summary>
    ''' "+"を"%20"にエンコードする
    ''' </summary>
    ''' <remarks></remarks>
    Private Const DecodeCharPlus As String = "+"

    ''' <summary>
    ''' "+"を"%20"にエンコードする
    ''' </summary>
    ''' <remarks></remarks>
    Private Const EncodeCharPlus As String = "%20"

    ' $02 START 次世代e-CRBセールス機能 新DB適応に向けた機能開発
    ''' <summary>
    ''' 顧客種別:個人
    ''' </summary>
    ''' <remarks></remarks>
    Private Const CustTypePerson As String = "0"
    ' $02 END   次世代e-CRBセールス機能 新DB適応に向けた機能開発

    ''' <summary>
    ''' 文言ID：固定表示敬称
    ''' </summary>
    ''' <remarks></remarks>
    Private Const NameTitleDefault As Integer = 13

    ''' <summary>
    ''' 顧客種別：オーナー
    ''' </summary>
    ''' <remarks></remarks>
    Private Const CustomerKindOwner As String = "1"

    ''' <summary>
    ''' システム設定名（車両登録番号の区切文字）
    ''' </summary>
    ''' <remarks></remarks>
    Private Const SysRegNumDlmtr As String = "REG_NUM_DELIMITER"

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
    Private Const ClameWord As Integer = 7
#End Region
#End Region

#Region "メンバ変数"
    ' 顧客担当SCPush送信フラグ
    Dim IsSendPushCustomerStuff As Boolean = False
    ' 来店通知メッセージ
    Dim MESSAGE_VISIT As String = WebWordUtility.GetWord(GateKeeperID, 4)
    ' 対応依頼メッセージ
    Dim MESSAGE_SUPPORT As String = WebWordUtility.GetWord(GateKeeperID, 5)
    ' 新規顧客名称
    Dim NewCustomerName As String = " "
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
        Logger.Info("GetClaimInfo_Start Param[" & dealerCode & ", " & storeCode & _
                    ", " & nowDate & ", " & complaintDateCount & "]")

        '開始日時(当日の0:00:00)を格納
        Dim startDate As Date = nowDate.Date

        '終了日時(当日の23:59:59)を格納
        Dim nextDate As Date = startDate.AddDays(NextDay)
        Dim endDate As Date = nextDate.AddSeconds(BeforMillisecond)
        nextDate = Nothing

        '苦情表示期間の設定
        Dim completeDate As Date = startDate.AddDays(-complaintDateCount)

        '返却用DataSet
        Dim retDataSet As VisitReceptionClaimInfoDataTable = Nothing

        Using dataAdapter As New VisitReceptionTableAdapter

            '苦情情報の取得
            retDataSet = dataAdapter.GetClaimInfo(dealerCode, storeCode, startDate, endDate, completeDate)
        End Using

        Dim claimVisitSequenceList As New List(Of Long)

        For Each row As VisitReceptionClaimInfoRow In retDataSet
            claimVisitSequenceList.Add(row.VISITSEQ)
        Next

        Logger.Info("GetClaimInfo_End ret[" & retDataSet.ToString & "]")

        Return claimVisitSequenceList

    End Function
#End Region

#Region "アンドン情報の取得"

    ''' <summary>
    ''' アンドン情報の取得
    ''' </summary>
    ''' <param name="dealerCode">販売店コード</param>
    ''' <param name="storeCode">店舗コード</param>
    ''' <returns>アンドン情報データテーブル</returns>
    ''' <remarks></remarks>
    Public Function GetBoardInfo(ByVal dealerCode As String, ByVal storeCode As String, _
                                 ByVal nowDate As Date) _
                                 As VisitReceptionBoardInfoDataTable

        Logger.Info("GetBoardInfo_Start Param[" & dealerCode & ", " & storeCode & "]")

        '開始日時(当日の0:00:00)を格納
        Dim startDate As Date = nowDate.Date
        '終了日時(当日の23:59:59)を格納
        Dim nextDate As Date = startDate.AddDays(NextDay)
        Dim endDate As Date = nextDate.AddSeconds(BeforMillisecond)
        nextDate = Nothing

        'アンドン情報データテーブル
        Dim boardInfoDataTable As New VisitReceptionBoardInfoDataTable
        'アンドン情報データロウ
        Dim andonInfoDataRow As VisitReceptionBoardInfoRow = _
            boardInfoDataTable.NewVisitReceptionBoardInfoRow()

        Using dataAdapter As New VisitReceptionTableAdapter

            '実績件数の取得
            Using resultCountDataTable As VisitReceptionResultCountDataTable = _
                dataAdapter.GetResultCount(dealerCode, storeCode, startDate, endDate)
                'アンドン情報データロウへ実績件数を設定
                andonInfoDataRow.RESULTCOUNT = CShort(resultCountDataTable.Item(0)(0))
            End Using
            '制約件数の取得
            Using conclusionDataTable As VisitReceptionConclusionCountDataTable = _
                dataAdapter.GetConclusionCount(dealerCode, storeCode, startDate, endDate)
                'アンドン情報データロウへ制約件数を設定
                andonInfoDataRow.CONCLUSIONCOUNT = CShort(conclusionDataTable.Item(0)(0))
            End Using

        End Using
        'アンドン情報データテーブルへアンドン情報データロウを追加
        boardInfoDataTable.AddVisitReceptionBoardInfoRow(andonInfoDataRow)
        'アンドン情報データテーブルを返す

        Logger.Info("GetBoardInfo_End Ret[" & boardInfoDataTable.ToString & "]")
        Return boardInfoDataTable
    End Function
#End Region

#Region "来店状況の取得"
    ''' <summary>
    ''' 来店状況の取得
    ''' </summary>
    ''' <param name="dealerCode">販売店コード</param>
    ''' <param name="storeCode">店舗コード</param>
    ''' <param name="visitStatusList">来店実績ステータス</param>
    ''' <param name="nowDate">現在日時</param>
    ''' <param name="claimVisitSequenceList">苦情情報来店実績連番リスト</param>
    ''' <returns>来店状況の取得データテーブル</returns>
    ''' <remarks></remarks>
    Public Function GetVisitorSituationInfo(ByVal dealerCode As String, _
                                            ByVal storeCode As String, _
                                            ByVal visitStatusList As List(Of String), _
                                            ByVal nowDate As Date, _
                                            ByVal claimVisitSequenceList As List(Of Long)) _
                                            As VisitReceptionVisitorSituationDataTable

        Logger.Info("GetVisitorSituationInfo_Start Param[" & dealerCode & ", " & storeCode & ", " & visitStatusList.ToString & "]")

        '来店状況の取得データテーブル
        Dim visitorSituationDataTable As VisitReceptionVisitorSituationDataTable = Nothing
        '開始日時(当日の0:00:00)を格納
        Dim startDate As Date = nowDate.Date
        '終了日時(当日の23:59:59)を格納
        Dim nextDate As Date = startDate.AddDays(NextDay)
        Dim endDate As Date = nextDate.AddSeconds(BeforMillisecond)
        nextDate = Nothing

        Using dataAdapter As New VisitReceptionTableAdapter

            '来店状況の取得
            visitorSituationDataTable = dataAdapter.GetVisitorSituation(dealerCode, _
                                                                        storeCode, _
                                                                        startDate, _
                                                                        endDate, _
                                                                        visitStatusList)

            '来店情報の数だけループ
            For Each visitorSituationRow As VisitReceptionVisitorSituationRow In visitorSituationDataTable

                ' 苦情情報が存在する場合
                If claimVisitSequenceList.Contains(visitorSituationRow.VISITSEQ) Then

                    visitorSituationRow.CLAIMFLG = ClaimFlagExists

                End If

            Next

        End Using
        '来店状況の取得データテーブルを返す
        Logger.Info("GetVisitorSituationInfo_End Ret[" & visitorSituationDataTable.ToString & "]")
        Return visitorSituationDataTable
    End Function
#End Region

#Region "スタッフ状況情報の取得"

    ''' <summary>
    ''' スタッフ状況情報の取得
    ''' </summary>
    ''' <param name="dealerCode">販売店コード</param>
    ''' <param name="storeCode">店舗コード</param>
    ''' <param name="nowDate">現在日時</param>
    ''' <param name="claimVisitSequenceList">苦情情報来店実績連番リスト</param>
    ''' <remarks></remarks>
    Public Function GetStaffSituationInfo(ByVal dealerCode As String, _
                                          ByVal storeCode As String, _
                                          ByVal nowDate As DateTime, _
                                          ByVal claimVisitSequenceList As List(Of Long)) _
                                              As VisitReceptionStaffSituationDataTable

        Logger.Info("GetStaffSituationInfo_Start Param[" & dealerCode & ", " & storeCode & "]")

        '開始日時(当日の0:00:00)を格納
        Dim startDate As Date = nowDate.Date
        '終了日時(当日の23:59:59)を格納
        Dim nextDate As Date = startDate.AddDays(NextDay)
        Dim endDate As Date = nextDate.AddSeconds(BeforMillisecond)
        nextDate = Nothing

        'スタッフ状況データテーブル
        Dim StaffSituationDataTable As New VisitReceptionStaffSituationDataTable

        Using dataAdapter As New VisitReceptionTableAdapter
            'スタッフ情報（商談中）の取得
            Using StaffNegotiateDataTable As VisitReceptionStaffSituationDataTable = _
                dataAdapter.GetStaffNegotiate(dealerCode, storeCode, startDate, endDate)
                'スタッフ状況データテーブルにスタッフ情報（商談中）をマージ
                StaffSituationDataTable.Merge(StaffNegotiateDataTable)
            End Using
            'スタッフ情報（スタンバイ）の取得
            Using StaffStandbyDataTable As VisitReceptionStaffSituationDataTable = _
                dataAdapter.GetStaffResult(dealerCode, storeCode, startDate, _
                                            endDate, StaffStatusStandby)
                'スタッフ状況データテーブルにスタッフ情報（スタンバイ）をマージ
                StaffSituationDataTable.Merge(StaffStandbyDataTable)
            End Using
            'スタッフ情報（一時退席）の取得
            Using StaffLeavingDataTable As VisitReceptionStaffSituationDataTable = _
                dataAdapter.GetStaffResult(dealerCode, storeCode, startDate, _
                                            endDate, StaffStatusLeaving)
                'スタッフ状況データテーブルにスタッフ情報（一時退席）をマージ
                StaffSituationDataTable.Merge(StaffLeavingDataTable)
            End Using
            'スタッフ情報（オフライン）の取得
            Using StaffOffLineDataTable As VisitReceptionStaffSituationDataTable = _
                dataAdapter.GetStaffOffline(dealerCode, storeCode)
                'スタッフ状況データテーブルにスタッフ情報（オフライン）をマージ
                StaffSituationDataTable.Merge(StaffOffLineDataTable)
            End Using

        End Using

        ' スタッフ状況情報に通知依頼情報をマージする
        StaffSituationDataTable = MargeStaffNotice(StaffSituationDataTable, dealerCode, storeCode, startDate, endDate)

        ' スタッフ状況情報に紐付け情報をマージする
        StaffSituationDataTable = MargeStaffLinking(StaffSituationDataTable, dealerCode, storeCode, startDate, endDate)

        '苦情情報の取得
        For Each staffRow As VisitReceptionStaffSituationRow In StaffSituationDataTable

            ' 苦情情報が存在する場合
            If Not staffRow.IsVISITSEQNull AndAlso claimVisitSequenceList.Contains(staffRow.VISITSEQ) Then

                staffRow.CLAIMFLG = ClaimFlagExists

            End If

        Next

        'スタッフ状況データテーブルを返す
        Logger.Info("GetStaffSituationInfo_End Ret[" & StaffSituationDataTable.ToString & "]")
        Return StaffSituationDataTable
    End Function

    ''' <summary>
    ''' スタッフ状況情報に通知依頼情報をマージする
    ''' </summary>
    ''' <param name="staffSituationDataTable">スタッフ状況情報</param>
    ''' <param name="dealerCode">販売店コード</param>
    ''' <param name="storeCode">店舗コード</param>
    ''' <param name="startDate">取得開始日時</param>
    ''' <param name="endDate">取得終了日時</param>
    ''' <remarks>スタッフ状況情報</remarks>
    Private Function MargeStaffNotice(ByVal staffSituationDataTable As VisitReceptionStaffSituationDataTable, _
                                      ByVal dealerCode As String, ByVal storeCode As String, _
                                      ByVal startDate As Date, ByVal endDate As Date) _
        As VisitReceptionStaffSituationDataTable

        Using dataAdapter As New VisitReceptionTableAdapter

            Dim noticeReq As String = Nothing
            Dim lastStatusList As List(Of String) = Nothing

            '通知依頼種別（査定）を設定
            noticeReq = NoticeAssessment
            lastStatusList = New List(Of String)
            lastStatusList.Add(NoticeStatus)

            '通知依頼（査定）の取得
            Using StaffNoticeRequestsDataTable As VisitReceptionNoticeRequestsDataTable = _
                             dataAdapter.GetNoticeRequests(dealerCode, storeCode, _
                                                           startDate, endDate, _
                                                           noticeReq, lastStatusList)

                If StaffNoticeRequestsDataTable.Rows.Count > 0 Then

                    'スタッフの数だけループ
                    For Each staffRow As VisitReceptionStaffSituationRow In staffSituationDataTable

                        '通知データ情報の数だけループ
                        For Each noticeRequestsRow As VisitReceptionNoticeRequestsRow In StaffNoticeRequestsDataTable

                            '送信日時を設定
                            If (noticeRequestsRow.ACCOUNT.Equals(staffRow.ACCOUNT)) Then

                                staffRow.REQUESTASSESSMENTDATE = noticeRequestsRow.SENDDATE
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
            Using StaffNoticeRequestsDataTable As VisitReceptionNoticeRequestsDataTable = _
                             dataAdapter.GetNoticeRequests(dealerCode, storeCode, _
                                                           startDate, endDate, _
                                                           noticeReq, lastStatusList)

                If StaffNoticeRequestsDataTable.Rows.Count > 0 Then

                    'スタッフの数だけループ
                    For Each staffRow As VisitReceptionStaffSituationRow In staffSituationDataTable

                        '通知データ情報の数だけループ
                        For Each noticeRequestsRow As VisitReceptionNoticeRequestsRow In StaffNoticeRequestsDataTable

                            '送信日時を設定
                            If (noticeRequestsRow.ACCOUNT.Equals(staffRow.ACCOUNT)) Then

                                staffRow.REQUESTPRICECONSULTATIONDATE = noticeRequestsRow.SENDDATE
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
            Using StaffNoticeRequestsDataTable As VisitReceptionNoticeRequestsDataTable = _
                             dataAdapter.GetNoticeRequests(dealerCode, storeCode, _
                                                           startDate, endDate, _
                                                           noticeReq, lastStatusList)

                If StaffNoticeRequestsDataTable.Rows.Count > 0 Then

                    'スタッフの数だけループ
                    For Each staffRow As VisitReceptionStaffSituationRow In staffSituationDataTable

                        '通知データ情報の数だけループ
                        For Each noticeRequestsRow As VisitReceptionNoticeRequestsRow In StaffNoticeRequestsDataTable

                            '送信日時を設定
                            If (noticeRequestsRow.ACCOUNT.Equals(staffRow.ACCOUNT)) Then

                                staffRow.REQUESTHELPDATE = noticeRequestsRow.SENDDATE
                                Exit For

                            End If
                        Next
                    Next

                End If

            End Using

        End Using

        Return staffSituationDataTable

    End Function

    ''' <summary>
    ''' スタッフ状況情報に紐付け情報をマージする
    ''' </summary>
    ''' <param name="staffSituationDataTable">スタッフ状況情報</param>
    ''' <param name="dealerCode">販売店コード</param>
    ''' <param name="storeCode">店舗コード</param>
    ''' <param name="startDate">取得開始日時</param>
    ''' <param name="endDate">取得終了日時</param>
    ''' <remarks>スタッフ状況情報</remarks>
    Private Function MargeStaffLinking(ByVal staffSituationDataTable As VisitReceptionStaffSituationDataTable, _
                                      ByVal dealerCode As String, ByVal storeCode As String, _
                                      ByVal startDate As Date, ByVal endDate As Date) _
        As VisitReceptionStaffSituationDataTable

        Using dataAdapter As New VisitReceptionTableAdapter

            '紐付け人数の取得
            Using StaffVisitorLinkingCountDataTable As VisitReceptionVisitorLinkingCountDataTable = _
                dataAdapter.GetVisitorLinkingCount(dealerCode, storeCode, startDate, endDate)

                If StaffVisitorLinkingCountDataTable.Rows.Count > 0 Then

                    'スタッフの数だけループ
                    For Each staffRow As VisitReceptionStaffSituationRow In staffSituationDataTable

                        '通知データ情報の数だけループ
                        For Each linkingCountRow As VisitReceptionVisitorLinkingCountRow In StaffVisitorLinkingCountDataTable

                            '紐付き人数を設定
                            If (linkingCountRow.ACCOUNT.Equals(staffRow.ACCOUNT)) Then

                                staffRow.VISITORLINKINGCOUNT = linkingCountRow.VISITORLINKINGCOUNT
                                Exit For

                            End If
                        Next
                    Next

                End If

            End Using

            '紐付け情報の取得
            Using StaffVisitorLinkingDataTable As VisitReceptionVisitorLinkingDataTable = _
                dataAdapter.GetVisitorLinking(dealerCode, storeCode, startDate, endDate)

                'スタッフの数だけループ
                For Each staffRow As VisitReceptionStaffSituationRow In staffSituationDataTable

                    '紐付けされているスタッフの数だけループ
                    For Each visitorLinkingRow As VisitReceptionVisitorLinkingRow In StaffVisitorLinkingDataTable

                        'アカウントが一致していた場合はスタッフ情報データロウに紐付け情報を設定
                        If (staffRow.ACCOUNT.Equals(visitorLinkingRow.ACCOUNT)) Then

                            staffRow.VISITSEQ = visitorLinkingRow.VISITSEQ
                            If (Not visitorLinkingRow.IsSALESTABLENONull) Then
                                staffRow.SALESTABLENO = visitorLinkingRow.SALESTABLENO
                            End If
                            staffRow.CUSTNAME = visitorLinkingRow.CUSTNAME
                            staffRow.CUSTNAMETITLE = visitorLinkingRow.CUSTNAMETITLE
                            staffRow.CUSTIMAGEFILE = visitorLinkingRow.CUSTIMAGEFILE
                            staffRow.CUSTSEGMENT = visitorLinkingRow.CUSTSEGMENT

                            Exit For

                        End If
                    Next
                Next

            End Using

        End Using

        Return staffSituationDataTable

    End Function

#End Region

#Region "お客様情報の取得"

    ''' <summary>
    ''' お客様情報の取得
    ''' </summary>
    ''' <param name="visitSequence">来店実績連番</param>
    ''' <returns>お客様情報データセット</returns>
    ''' <remarks></remarks>
    Public Function GetCustomerInfo(ByVal visitSequence As Long, Optional ByVal visitStatus As String = Nothing) _
        As VisitReceptionVisitorCustomerDataTable

        Logger.Info("GetCustomerInfo_Start Param[" & visitSequence & "]")

        'お客様情報データテーブル
        Dim customerInfoDataSet As VisitReceptionVisitorCustomerDataTable = Nothing

        Using dataAdapter As New VisitReceptionTableAdapter
            '来店実績お客様情報取得
            customerInfoDataSet = dataAdapter.GetVisitorCustomer(visitSequence, visitStatus)
        End Using
        'お客様情報データテーブルを返す
        Logger.Info("GetCustomerInfo_End Ret[" & customerInfoDataSet.ToString & "]")
        Return customerInfoDataSet
    End Function
#End Region

#Region "スタッフ通知依頼情報の取得"

    ''' <summary>
    ''' スタッフ通知依頼情報の取得
    ''' </summary>
    ''' <param name="visitSeq">シーケンス番号</param>
    ''' <returns>取得結果</returns>
    ''' <remarks></remarks>
    Public Function GetStaffNoticeRequest(ByVal visitSeq As Long) As VisitReceptionStaffNoticeRequestDataTable

        Logger.Info("GetStaffNoticeRequest_Start ")
        Logger.Info("Param[" & visitSeq & "]")

        'マージ用DataSet
        Using mergeDataSet As New VisitReceptionStaffNoticeRequestDataTable

            '依頼、受信両方のステータスを対応させる
            Dim statusList As New List(Of String)
            statusList.Add(NoticeStatus)


            Using dataAdapter As New VisitReceptionTableAdapter

                '査定依頼情報の取得
                Using noticeDataSet As VisitReceptionStaffNoticeRequestDataTable = _
                    dataAdapter.GetStaffNoticeRequest(visitSeq, NoticeAssessment, statusList)
                    '取得したデータをマージさせる
                    mergeDataSet.Merge(noticeDataSet)
                End Using

                '受信を追加
                statusList.Add(ReceiveStatus)

                '価格相談依頼情報の取得
                Using priceConsultationDataSet As VisitReceptionStaffNoticeRequestDataTable = _
                    dataAdapter.GetStaffNoticeRequest(visitSeq, NoticePriceConsultation, statusList)
                    '取得したデータをマージさせる
                    mergeDataSet.Merge(priceConsultationDataSet)
                End Using

                'ヘルプでは受信がないので、予め削除する
                statusList.Remove(ReceiveStatus)

                'ヘルプ依頼情報の取得
                Using helpDataSet As VisitReceptionStaffNoticeRequestDataTable = _
                    dataAdapter.GetStaffNoticeRequest(visitSeq, NoticeHelp, statusList)
                    '取得したデータをマージさせる
                    mergeDataSet.Merge(helpDataSet)
                End Using

            End Using

            Dim view As DataView = mergeDataSet.DefaultView

            view.Sort = "SENDDATE ASC"

            Dim retDataSet As New VisitReceptionStaffNoticeRequestDataTable

            For row As Integer = 0 To view.Count - 1
                retDataSet.ImportRow(view.Item(row).Row)
            Next

            Logger.Info("GetClaimInfo_End ")
            Logger.Info("ret[" & retDataSet.ToString & "]")

            Return retDataSet
        End Using
    End Function
#End Region

#Region "来店回数の取得"

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
                                  ByVal followUpBoxSeqNo As Long _
                                 ) As VisitReceptionVisitCountDataTable

        Logger.Info("GetVisitCount_Start " & _
                    "Param[" & dealerCode & ", " & storeCode & ", " & followUpBoxSeqNo & "]")

        '返却用DataSet
        Dim retDataSet As New VisitReceptionVisitCountDataTable

        Using adapter As New VisitReceptionTableAdapter

            '来店回数取得
            retDataSet = adapter.GetVisitCount(dealerCode, storeCode, followUpBoxSeqNo)
        End Using

        Logger.Info("GetVisitCount_End " & _
                    "ret[" & retDataSet.ToString & "]")
        Return retDataSet
    End Function
#End Region

#Region "経過時間のリスト作成"

    ''' <summary>
    ''' 経過時間のリスト作成
    ''' </summary>
    ''' <param name="dataTable">データテーブル</param>
    ''' <param name="columnName">カラム名</param>
    ''' <returns>経過時間のリスト</returns>
    ''' <remarks></remarks>
    Public Function GetTimeSpanListString(ByVal dataTable As DataTable, _
                                           ByVal columnName As String, ByVal nowDate As Date) As List(Of String)
        Logger.Info("GetTimeSpanListString_Start " & _
                   "Param[" & dataTable.ToString & "," & columnName & "," & nowDate & "]")

        Dim timeSpanList As New List(Of String)

        For Each row As DataRow In dataTable.Rows

            Dim span As String = String.Empty

            ' 値が設定されている場合
            If Not IsDBNull(row(columnName)) AndAlso Not String.IsNullOrEmpty(row(columnName).ToString) Then

                Dim startDate As Date = CType(row(columnName).ToString(), Date)
                span = CType(Math.Round(nowDate.Subtract(startDate).TotalSeconds), String)

            End If

            timeSpanList.Add(span)

        Next

        Logger.Info("GetTimeSpanListString_End Ret[timeSpanList.Count = " & timeSpanList.Count & "]")
        Return timeSpanList
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
    Public Function GetAlarmOutputFlag(ByVal dealerCode As String, ByVal storeCode As String, ByVal operationCode As Decimal) As String
        Logger.Info("GetAlarmOutputFlag_Start " & _
                    "Param[" & operationCode & "]")

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

        Logger.Info("GetAlarmOutputFlag_End " & _
                    "Ret[" & alarmOutputFlag & "]")
        Return alarmOutputFlag
    End Function
#End Region

#Region "顧客一覧取得"

    ''' <summary>
    ''' 顧客一覧を取得する
    ''' </summary>
    ''' <param name="dealerCode">販売店コード</param>
    ''' <param name="storeCode">店舗コード</param>
    ''' <param name="searchType">検索タイプ(1：車両登録No 、2：顧客名称、 3：VIN、 4：電話番号/携帯番号、５：国民ID)</param>
    ''' <param name="searchText">検索テキスト</param>
    ''' <returns>顧客情報データテーブル</returns>
    ''' <remarks></remarks>
    Public Function GetCustomerList(ByVal dealerCode As String, _
                                    ByVal storeCode As String, _
                                    ByVal searchType As String, _
                                    ByVal searchText As List(Of String),
                                    ByVal sortType As String) As VisitReceptionCustomerListDataTable
        Dim startLog As New StringBuilder
        startLog.Append("GetCustomerList_Start Param[dealerCode=" & dealerCode & ",storeCode= " _
            & storeCode & ",searchType= " & searchType & ",searchText=")
        For Each serchString In searchText
            startLog.Append(serchString & ",")
        Next
        startLog.Append(" ]")
        Logger.Info(startLog.ToString)

        '顧客情報の取得データテーブル
        Dim customerListDataTable As VisitReceptionCustomerListDataTable = Nothing

        Using dataAdapter As New VisitReceptionTableAdapter

            '顧客情報の取得
            customerListDataTable = dataAdapter.GetCustomerList(dealerCode, _
                                                              searchType, _
                                                              searchText, sortType)
        End Using

        'スタッフ情報の取得データテーブルを返す
        Logger.Info("GetCustomerList_End Ret[GerRows = " & customerListDataTable.Rows.Count & "]")
        Return customerListDataTable

    End Function

#End Region

#Region "Push送信"

    ''' <summary>
    ''' Push送信
    ''' </summary>
    ''' <param name="customerRow">セールス来店実績データロウ</param>
    ''' <param name="isComplaint">苦情有無フラグ</param>
    ''' <returns>メッセージID</returns>
    ''' <remarks></remarks>
    Public Function SendPushSales(ByVal customerRow As VisitReceptionVisitSalesRow, _
                      ByVal isComplaint As Boolean) As Integer

        Logger.Info("SendPush_Start Param[insertRow=" & (customerRow IsNot Nothing) & _
                    ",isComplaint= " & isComplaint & "]")

        Dim messageId As Integer = MessageIdSuccess
        Dim custName As String = CreateCustomerName(customerRow.CUSTNAME, customerRow.CUSTNAMETITLE)

        ' SLR,SSMへの受付画面更新命令の送信
        SendPushUpdateReceptionistMain(customerRow.DEALERCODE, customerRow.STORECODE)

        ' 呼び出し元がゲートキーパーの場合、ウェルカムボードへのPush通知を行う。
        If String.Equals(customerRow.FUNCTIONID, GateKeeperID) Then

            '送信処理
            SendPushWBUpdate(customerRow)
        End If

        If customerRow.ISSTAFFFLG Then
            ' 顧客担当スタッフへ対応依頼通知を送信
            Dim sendMessage As String = _
            CreateSendMessage(custName, customerRow.VEHICLEREGNO, _
                              MESSAGE_VISIT, isComplaint)

            SendVisitInfo(SendTypeCsutSs, customerRow.STAFFCODE, sendMessage)
        End If

        Logger.Info("SendPush_End Ret[messageId=" & messageId & "]")
        Return messageId
    End Function

    ''' <summary>
    ''' 受付メイン画面更新命令送信処理
    ''' </summary>
    ''' <param name="dealerCode">販売店コード</param>
    ''' <param name="storeCode">店舗コード</param>
    ''' <remarks></remarks>
    Private Sub SendPushUpdateReceptionistMain(ByVal dealerCode As String, _
                                               ByVal storeCode As String)

        Logger.Info("SendPushUpdateReceptionistMain_Start Param[dealerCode=" & dealerCode & _
                    ",storeCode= " & storeCode & "]")

        'スタッフ情報の取得(受付係、セールスマネージャー)
        Dim stuffCodeList As New List(Of Decimal)
        stuffCodeList.Add(OperationCodeSlr)
        stuffCodeList.Add(OperationCodeSsm)

        'オンラインユーザー情報の取得
        Dim utility As New VisitUtilityBusinessLogic
        Dim sendPushUsers As VisitUtilityUsersDataTable = _
            utility.GetOnlineUsers(dealerCode, storeCode, stuffCodeList)
        utility = Nothing

        '来店通知命令の送信
        For Each userRow As VisitUtilityUsersRow In sendPushUsers

            Dim sendType As Integer

            '送信種別
            Select Case userRow.OPERATIONCODE
                Case OperationCodeSlr

                    sendType = SendTypeSlr

                Case OperationCodeSsm

                    sendType = SendTypeSsm

            End Select

            '送信処理
            SendVisitInfo(sendType, userRow.ACCOUNT)
        Next


        Logger.Info("SendPushUpdateReceptionistMain_End ]")
    End Sub

    ''' <summary>
    ''' 来店通知送信命令の送信
    ''' </summary>
    ''' <param name="sendKind">通知種別</param>
    ''' <param name="stuffCode">スタッフコード</param>
    ''' <param name="message">メッセージ</param>
    ''' <remarks></remarks>
    Private Sub SendVisitInfo(ByVal sendKind As Integer, ByVal stuffCode As String, _
                                     Optional ByVal message As String = "")

        Logger.Info("SendVisitInfo_Start Param[sendKind=" & sendKind & _
                    ",stuffCode= " & stuffCode & ",message= " & message & "]")

        Dim postMsg As New StringBuilder

        'POST送信する文字列を作成する。
        Select Case sendKind
            Case SendTypeCsutSs

                '顧客担当SSの場合
                With postMsg
                    .Append("cat=popup")
                    .Append("&type=header")
                    .Append("&sub=text")
                    .Append("&uid=" & stuffCode)
                    .Append("&time=3")
                    .Append("&color=F9EDBE64")
                    .Append("&height=50")
                    .Append("&width=1024")
                    .Append("&pox=0")
                    .Append("&msg=" & message)
                    .Append("&js1=icropScript.ui.setVisitor()")
                    .Append("&js2=icropScript.ui.openVisitorListDialog()")
                End With

            Case SendTypeSlr, SendTypeSsm

                '受付係、セールスマネージャー
                With postMsg
                    .Append("cat=action")
                    .Append("&type=main")
                    .Append("&sub=js")
                    .Append("&uid=" & stuffCode)
                    .Append("&time=0")
                    .Append("&js1=SC3100101Update('01','01')")
                End With

        End Select

        Dim visitReception As New VisitUtility
        visitReception.SendPush(postMsg.ToString())

        Logger.Info("SendVisitInfo_End")

    End Sub

    ''' <summary>
    ''' セールススタッフ[スタンバイ]への通知処理
    ''' </summary>
    ''' <param name="customerRow">セールス来店実績データロウ</param>
    ''' <param name="isComplaint">苦情有無フラグ</param>
    ''' <returns>メッセージID</returns>
    ''' <remarks></remarks>
    Private Function SendStandbyStuff(ByVal customerRow As VisitReceptionVisitSalesRow, _
                                      ByVal isComplaint As Boolean) As Integer

        Logger.Info("SendStandbyStuff_Start Param[customerRow=" & (customerRow IsNot Nothing) & _
                    ",isComplaint= " & isComplaint & "]")

        ' スタンバイ中のスタッフの取得
        Dim stuffInfo As VisitUtilityUsersDataTable
        Dim operationcodeList As New List(Of Decimal)
        operationcodeList.Add(8)
        Dim presencecaterogyList As New List(Of String)
        presencecaterogyList.Add("1")
        stuffInfo = _
            VisitUtilityDataSetTableAdapter.GetUsers(customerRow.DEALERCODE, _
                                                     customerRow.STORECODE, _
                                                     operationcodeList, presencecaterogyList, "0")

        ' スタンバイ中のスタッフが存在しなければ処理を抜ける
        ' 又はスタンバイ中のスタッフが１人のみ存在しそのスタッフが顧客担当SCの場合も処理を抜ける
        If stuffInfo.Rows.Count = 0 OrElse (stuffInfo.Rows.Count = 1 AndAlso _
                                            stuffInfo(0).ACCOUNT.Equals(customerRow.STAFFCODE)) Then
            Return MessageIdSuccess
        End If

        '通知IFへ渡すクラスの生成
        Dim noticeData As Toyota.eCRB.Tool.Notify.Api.DataAccess.XmlNoticeData
        noticeData = CreateInputClass(stuffInfo, customerRow, isComplaint)

        '通知IFの呼び出し
        Dim returnXml As New XmlCommon
        Using ic3040801Biz As New IC3040801BusinessLogic
            returnXml = ic3040801Biz.NoticeDisplay(noticeData, NoticeDisposal.Peculiar)
        End Using

        ' 戻り値判断
        ' IFの戻り値は成功='0'又はDBアクセスエラー='6000'のため成功かどうかのみで判断
        ' DBアクセスエラー以外のエラーはExceptionで帰ってくるためそのまま基盤へthrow
        If String.Equals(returnXml.ResultId, "006000") Then
            Logger.Info("SendStandbyStuff_End Ret[messageId=" & MessageIdErrorDbTimeOut & "]")
            Return MessageIdErrorDbTimeOut
        End If

        Logger.Info("SendStandbyStuff_End Ret[messageId=" & MessageIdSuccess & "]")
        Return MessageIdSuccess
    End Function

    ''' <summary>
    ''' 通知IFへ渡すXmlNoticeDataクラスの作成処理
    ''' </summary>
    ''' <param name="stuffList">セールススタッフリスト</param>
    ''' <param name="customerRow">顧客情報保持データロウ</param>
    ''' <param name="isComplaint">苦情有無フラグ</param>
    ''' <returns>XmlNoticeDataクラス</returns>
    ''' <remarks></remarks>
    Private Function CreateInputClass(ByVal stuffList As VisitUtilityUsersDataTable, _
                                      ByVal customerRow As VisitReceptionVisitSalesRow, _
                                      ByVal isComplaint As Boolean) _
                                      As XmlNoticeData

        Dim returnValue As XmlNoticeData = New XmlNoticeData

        Dim custname As String = CreateCustomerName(customerRow.CUSTNAME, customerRow.CUSTNAMETITLE)
        'ヘッダー情報
        returnValue.TransmissionDate = DateTimeFunc.Now(StaffContext.Current.DlrCD)

        '来店通知命令の送信(セールススタッフ[スタンバイ])
        For Each salesStuffInfoRow As VisitUtilityUsersRow In stuffList.Rows

            '顧客担当スタッフ以外の場合、送信対象とする
            If Not String.Equals(customerRow.STAFFCODE, salesStuffInfoRow.ACCOUNT) Then
                Dim xmlAccount As XmlAccount = New XmlAccount
                xmlAccount.ToAccount = salesStuffInfoRow.ACCOUNT
                xmlAccount.ToAccountName = salesStuffInfoRow.USERNAME
                returnValue.AccountList.Add(xmlAccount)
            End If
        Next

        'Request情報
        Dim requestNotice As XmlRequestNotice = New XmlRequestNotice
        requestNotice.DealerCode = StaffContext.Current.DlrCD
        requestNotice.StoreCode = StaffContext.Current.BrnCD
        requestNotice.RequestClass = "04"
        requestNotice.Status = "4"
        requestNotice.RequestClassId = customerRow.VISITSEQUENCE
        requestNotice.FromAccount = StaffContext.Current.Account
        requestNotice.FromAccountName = StaffContext.Current.UserName
        requestNotice.CustomId = customerRow.CUSTOMERID
        requestNotice.CustomName = custname
        requestNotice.CustomerClass = CustomerKindOwner
        requestNotice.CustomerKind = customerRow.CUSTOMERSEGMENT
        returnValue.RequestNotice = requestNotice

        'Push情報
        Dim pushInfo As XmlPushInfo = New XmlPushInfo
        pushInfo.PushCategory = "1"
        pushInfo.PositionType = "1"
        pushInfo.Time = 3
        pushInfo.DisplayType = "1"
        pushInfo.DisplayContents = _
            CreateSendMessage(custname, customerRow.VEHICLEREGNO, _
                              MESSAGE_SUPPORT, isComplaint)
        pushInfo.Color = "2"
        pushInfo.PopWidth = 1024
        pushInfo.PopHeight = 50
        pushInfo.PopX = 0
        pushInfo.DisplayFunction = "icropScript.ui.setVisitor()"
        pushInfo.ActionFunction = "icropScript.ui.openVisitorListDialog()"
        returnValue.PushInfo = pushInfo

        Return returnValue

    End Function

    ''' <summary>
    ''' 送信メッセージ作成
    ''' </summary>
    ''' <param name="customerName">顧客名</param>
    ''' <param name="vehicleNo">車両登録No</param>
    ''' <param name="message">通知メッセージ</param>
    ''' <param name="claimeInfo">苦情有無</param>
    ''' <returns>送信メッセージ</returns>
    ''' <remarks></remarks>
    Private Function CreateSendMessage(ByVal customerName As String, _
                                       ByVal vehicleNo As String, _
                                       ByVal message As String, _
                                       ByVal claimeInfo As Boolean) As String

        Logger.Info("CreateSendMessage_Start Param[customerName=" & customerName & _
                    ",vehicleNo= " & vehicleNo & ",message= " & message & _
                    ",claimeInfo= " & claimeInfo & "]")

        '送信メッセージ
        Dim pushStandbyStuffMessage As New StringBuilder

        '苦情情報の有無を判定
        If claimeInfo Then
            Dim claimMessage As String = WebWordUtility.GetWord(GateKeeperID, ClameWord)
            pushStandbyStuffMessage.Append(claimMessage)
            pushStandbyStuffMessage.Append(" ")
        End If

        '送信メッセージ作成
        pushStandbyStuffMessage.Append(customerName)
        pushStandbyStuffMessage.Append(" ")
        pushStandbyStuffMessage.Append(message)
        pushStandbyStuffMessage.Append(" ")
        pushStandbyStuffMessage.Append(vehicleNo)

        Logger.Info("CreateSendMessage_End Ret[pushStandbyStuffMessage=" & pushStandbyStuffMessage.ToString & "]")
        Return pushStandbyStuffMessage.ToString

    End Function

    ''' <summary>
    ''' 送信処理（ウェルカムボード：来店通知)
    ''' </summary>
    ''' <param name="customerRow">セールス来店実績データロウ</param>
    ''' <remarks></remarks>
    Private Sub SendPushWBUpdate(ByVal customerRow As VisitReceptionVisitSalesRow)

        Logger.Debug("SendPushWBUpdate_Start")
        Dim customerClass As String
        Dim customerName As String

        ' 新規顧客
        If customerRow.CUSTOMERSEGMENT <> NewCustomer And customerRow.CUSTOMERSEGMENT <> OriginalCustomer Then
            customerClass = CustomerClassNew
            customerName = NewCustomerName

            ' 自社・未取引客
        Else
            customerClass = CustomerClassOrginal
            '$08 start ウェルカムボード仕様変更対応
            ' 敬称が設定されているか判断
            If String.IsNullOrEmpty(Trim(customerRow.CUSTNAMETITLE)) Then
                ' 顧客タイプ判断
                If CustTypePerson.Equals(customerRow.CUSTYPE) Then
                    '顧客タイプが個人の場合、固定の敬称を付与
                    customerName = CreateCustomerName(customerRow.CUSTNAME, WebWordUtility.GetWord(GateKeeperID, NameTitleDefault))
                Else
                    '顧客タイプが法人の場合、敬称なし
                    customerName = customerRow.CUSTNAME
                End If
            Else
                ' 敬称が設定されている場合は、DBの値を使う
                customerName = CreateCustomerName(customerRow.CUSTNAME, customerRow.CUSTNAMETITLE)
            End If
            '$08 start ウェルカムボード仕様変更対応
        End If

        'スタッフ情報の取得(ウェルカムボード)
        Dim stuffCodeListWB As New List(Of Decimal)
        stuffCodeListWB.Add(OperationCodeWB)
        'オンラインユーザー情報の取得
        Dim utilityWB As New VisitUtilityBusinessLogic
        Dim sendPushUsersWB As VisitUtilityUsersDataTable = _
        utilityWB.GetOnlineUsers(customerRow.DEALERCODE, customerRow.STORECODE, stuffCodeListWB)
        utilityWB = Nothing

        Dim textEncode As System.Text.Encoding = System.Text.Encoding.GetEncoding("UTF-8")

        '来店通知命令の送信
        For Each userRow As VisitUtilityUsersRow In sendPushUsersWB
            'POST送信メッセージの作成
            Dim postSendMessage As New StringBuilder
            postSendMessage.Append("cat=action")
            postSendMessage.Append("&type=main")
            postSendMessage.Append("&sub=js")
            postSendMessage.Append("&uid=" & userRow.ACCOUNT)
            postSendMessage.Append("&time=0")
            postSendMessage.Append("&js1=SC3100304Update('")
            postSendMessage.Append(customerClass)
            postSendMessage.Append("','")
            postSendMessage.Append(HttpUtility.UrlEncode(customerName, textEncode).Replace(DecodeCharPlus, EncodeCharPlus))
            postSendMessage.Append("')")

            '送信処理
            Dim visitReception As New VisitUtility
            visitReception.SendPushPC(postSendMessage.ToString)
        Next

        Logger.Debug("SendPushWBUpdate_End]")
    End Sub
#End Region

#Region "お客様チップ作成"

    ''' <summary>
    ''' お客様チップ作成
    ''' </summary>
    ''' <param name="insertRow">セールス来店実績データロウ</param>
    ''' <param name="isComplaint">苦情有無フラグ</param>
    ''' <returns>メッセージID</returns>
    ''' <remarks></remarks>
    Public Function CreateCustomerChip(ByVal insertRow As VisitReceptionVisitSalesRow, _
                                ByVal isComplaint As Boolean) As Integer

        Logger.Info("CreateCustomerChip_Start Param[insertRow=" & (insertRow IsNot Nothing) & _
                    ",isComplaint= " & isComplaint & "]")

        Dim messageId As Integer = MessageIdSuccess
        Dim isSendStandbyStuff As Boolean = False

        '来店実績連番の取得
        Using adapter As New UpdateSalesVisitTableAdapter
            insertRow.VISITSEQUENCE = adapter.GetVisitSalesSeqNextValue()
        End Using

        ' セールス来店実績の作成
        Using adapter As New VisitReceptionTableAdapter

            ' 既存顧客かつ担当SCが存在する場合、担当SCのステータスをチェックする。
            If Not String.IsNullOrEmpty(insertRow.CUSTOMERSEGMENT) Then

                ' スタンバイスタッフ送信フラグ
                isSendStandbyStuff = True
                Dim operationCodeList As New List(Of Decimal)
                operationCodeList.Add(8)
                Dim presenceCategoryList As New List(Of String)
                presenceCategoryList.Add(PresenceCategoryStandby)
                presenceCategoryList.Add(PresenceCategoryNegotiate)
                presenceCategoryList.Add(PresenceCategoryLeaving)
                presenceCategoryList.Add(PresenceCategoryOffline)
                Dim stuffInfo As New VisitUtilityUsersDataTable
                Dim stuffInfoRow As VisitUtilityUsersRow
                stuffInfo = VisitUtilityDataSetTableAdapter.GetUsers(insertRow.DEALERCODE, insertRow.STORECODE, _
                                                operationCodeList, presenceCategoryList, "0", insertRow.STAFFCODE)

                If stuffInfo.Rows.Count > 0 Then
                    stuffInfoRow = CType(stuffInfo.Rows(0), VisitUtilityUsersRow)
                Else
                    stuffInfoRow = Nothing
                End If

                ' SCが存在しない又はステータスがオフラインの場合
                If stuffInfoRow Is Nothing OrElse _
                    String.Equals(StuffStatusOffline, stuffInfoRow.PRESENCECATEGORY) Then

                    insertRow.VISITSTATUS = VisitStatusFree
                    insertRow.PHYSICSSTAFFCODE = Nothing
                    IsSendPushCustomerStuff = True
                    insertRow.ISSTAFFFLG = False
                    insertRow.BROUDCAST = BroudcastFlagNotTarget
                Else

                    ' SCが存在しステータスがオフライン以外の場合
                    insertRow.VISITSTATUS = VisitStatusAdjust
                    insertRow.PHYSICSSTAFFCODE = insertRow.STAFFCODE
                    IsSendPushCustomerStuff = True
                    insertRow.ISSTAFFFLG = True
                    insertRow.BROUDCAST = BroudcastFlagNotTarget
                End If
            Else
                insertRow.VISITSTATUS = VisitStatusFree
                insertRow.PHYSICSSTAFFCODE = Nothing
                IsSendPushCustomerStuff = False
                insertRow.ISSTAFFFLG = False
                insertRow.BROUDCAST = BroudcastFlagUnsend
            End If

            ' セールス来店実績の作成
            adapter.InsertVisitSales(insertRow)

            ' スタンバイスタッフへの来店通知
            If isSendStandbyStuff Then
                messageId = SendStandbyStuff(insertRow, isComplaint)

            End If

        End Using

        Logger.Info("CreateCustomerChip_End Ret[messageId=" & messageId & "]")
        Return messageId
    End Function

    ' $01 end   step2開発

    ' $01 start step2開発
    ''' <summary>
    ''' 敬称付きお客様名作成
    ''' </summary>
    ''' <param name="customerName">お客様名</param>
    ''' <param name="customerNameTitle">お客様敬称</param>
    ''' <returns>敬称付きお客様名</returns>
    ''' <remarks></remarks>
    Private Function CreateCustomerName(ByVal customerName As String, ByVal customerNameTitle As String) As String

        '敬称表示位置を取得
        Dim sysEnvSet As New SystemEnvSetting
        Dim sysEnvSetRow As SystemEnvSettingDataSet.SYSTEMENVSETTINGRow
        Logger.Info("CreateCustomerName_001 " & "Call_Start SystemEnvSetting.GetSystemEnvSetting Pram[" & KeisyoZengo & "]")
        sysEnvSetRow = sysEnvSet.GetSystemEnvSetting(KeisyoZengo)
        Logger.Info("CreateCustomerName_001 " & "Call_End SystemEnvSetting.GetSystemEnvSetting Ret[" & (sysEnvSetRow IsNot Nothing) & "]")

        'お客様名作成
        Dim result As String
        If String.Equals(sysEnvSetRow.PARAMVALUE, HonorificTitleMae) Then
            ' 敬称表示位置が前
            result = customerNameTitle & " " & customerName
        Else
            ' 敬称表示位置が後
            result = customerName & " " & customerNameTitle
        End If

        Return result

    End Function

#End Region

    ' $03 START (トライ店システム評価)SMBチップ検索の絞り込み方法変更
#Region "車両登録番号検索ワード変換"

    ''' <summary>
    ''' 車両登録番号検索ワード変換
    ''' </summary>
    ''' <param name="inSearchWord">検索ワード</param>
    ''' <returns>「*」と区切り文字を取り除いた検索ワード</returns>
    ''' <remarks></remarks>
    ''' <history>
    ''' 2019/11/12 NSK 鈴木 (トライ店システム評価)次世代セールス基盤：ログ出力機能における保守性向上検証
    ''' </history>
    Public Function ConvertVclRegNumWord(ByVal inSearchWord As String) As String

        Logger.Info(String.Format(CultureInfo.InvariantCulture, _
                                  "{0}.{1}_S IN:inSearchWord={2}", _
                                  Me.GetType.ToString, _
                                  System.Reflection.MethodBase.GetCurrentMethod.Name, _
                                  inSearchWord))

        ' 2019/11/12 NSK 鈴木 (トライ店システム評価)次世代セールス基盤：ログ出力機能における保守性向上検証 START
        ''区切り文字を取得
        'Dim regNumDlmtr As String = GetSystemSettingValueBySettingName(SysRegNumDlmtr)

        '区切り文字を取得
        Dim systemSetting = New SystemSetting
        Dim row As TB_M_SYSTEM_SETTINGRow = systemSetting.GetSystemSetting(SysRegNumDlmtr)
        Dim regNumDlmtr As String = String.Empty
        If row IsNot Nothing Then
            regNumDlmtr = row.SETTING_VAL
        End If
        ' 2019/11/12 NSK 鈴木 (トライ店システム評価)次世代セールス基盤：ログ出力機能における保守性向上検証 END

        '区切り文字が存在する場合
        If Not String.IsNullOrEmpty(regNumDlmtr) Then

            '文字間に入力された'*'を検索文字列より削除
            inSearchWord = inSearchWord.Replace("*", String.Empty)

            '取得された区切文字を'*'で分割
            Dim regNumDlmtrList As List(Of String) = regNumDlmtr.Split("*"c).ToList

            For Each dlmtr As String In regNumDlmtrList
                '区切り文字を'*'で分割
                inSearchWord = inSearchWord.Replace(dlmtr, String.Empty)
            Next

        End If

        Logger.Info(String.Format(CultureInfo.InvariantCulture, _
                                  "{0}.{1}_E OUT:returnValue={2}", _
                                  Me.GetType.ToString, _
                                  System.Reflection.MethodBase.GetCurrentMethod.Name, _
                                  inSearchWord))

        Return inSearchWord

    End Function

#End Region

#Region "システム設定値を取得する"

    ' 2019/11/12 NSK 鈴木 (トライ店システム評価)次世代セールス基盤：ログ出力機能における保守性向上検証 START
    ' ''' <summary>
    ' ''' システム設定値を設定値名を条件に取得する
    ' ''' </summary>
    ' ''' <param name="settingName">システム設定値名</param>
    ' ''' <returns></returns>
    ' ''' <remarks></remarks>
    'Public Function GetSystemSettingValueBySettingName(ByVal settingName As String) As String

    '    Logger.Info(String.Format(CultureInfo.InvariantCulture, _
    '                              "{0}_S IN:settingName={1}", _
    '                              System.Reflection.MethodBase.GetCurrentMethod.Name, _
    '                              settingName))

    '    '戻り値
    '    Dim retValue As String = String.Empty

    '    '自分のテーブルアダプタークラスインスタンスを生成
    '    Using ta As New VisitReceptionDataSetTableAdapters.VisitReceptionTableAdapter

    '        'システム設定から取得
    '        Dim dt As VisitReceptionDataSet.SystemSettingDataTable _
    '            = ta.GetSystemSettingValue(settingName)

    '        If 0 < dt.Count Then

    '            '設定値を取得
    '            retValue = dt.Item(0).SETTING_VAL

    '        End If

    '    End Using

    '    Logger.Info(String.Format(CultureInfo.InvariantCulture, _
    '                              "{0}_E OUT:{1}={2}", _
    '                              System.Reflection.MethodBase.GetCurrentMethod.Name, _
    '                              settingName, _
    '                              retValue))

    '    Return retValue

    'End Function
    ' 2019/11/12 NSK 鈴木 (トライ店システム評価)次世代セールス基盤：ログ出力機能における保守性向上検証 END

#End Region
    ' $03 END (トライ店システム評価)SMBチップ検索の絞り込み方法変更

End Class
