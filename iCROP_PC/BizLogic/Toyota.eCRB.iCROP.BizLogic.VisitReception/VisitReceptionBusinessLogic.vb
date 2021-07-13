Imports Toyota.eCRB.SystemFrameworks.Core
Imports System.Web
Imports Toyota.eCRB.Visit.Api.DataAccess.VisitReceptionDataSet
Imports Toyota.eCRB.Visit.Api.DataAccess.VisitReceptionDataSetTableAdapters
Imports Toyota.eCRB.Visit.Api.DataAccess
Imports Toyota.eCRB.SystemFrameworks.Core.iCROP.BizLogic
Imports Toyota.eCRB.SystemFrameworks.Core.iCROP.DataAccess

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

End Class
