'━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
'SC3220201BusinessLogic.vb
'─────────────────────────────────────
'機能： 全体管理処理
'補足： 
'作成： 2013/02/28 TMEJ小澤	初版作成
'更新： 2013/12/10 TMEJ 河原 TMEJ次世代サービス 工程管理機能開発
'更新： 2017/09/14 NSK 竹中(悠) REQ-SVT-TMT-20170227-002 見込納車遅れ日時計算ロジック統一
'更新： 2018/02/27 NSK 山田 17PRJ01947-00_(トライ店システム評価)サービス重要顧客における、識別及び対処オペレーションの検証
'更新： 2018/06/29 NSK 可児 TKM Next Gen e-CRB Project Application development Block B-1  Pマーク、Lマークなどを表示
'更新： 
'─────────────────────────────────────

Imports System.Globalization
Imports Toyota.eCRB.SystemFrameworks.Core
Imports Toyota.eCRB.DMSLinkage.CompleteCheck.DataAccess.SC3220201DataSetTableAdapters
Imports Toyota.eCRB.SystemFrameworks.Web
Imports Toyota.eCRB.DMSLinkage.CompleteCheck.DataAccess.SC3220201DataSet
Imports Toyota.eCRB.SMBLinkage.GetServiceLT.Api.DataAccess.IC3810701DataSet
Imports Toyota.eCRB.SMBLinkage.GetServiceLT.Api.BizLogic

'2013/12/10 TMEJ 河原 TMEJ次世代サービス 工程管理機能開発 START

'Imports Toyota.eCRB.DMSLinkage.RepairOrderSearch.DataAccess.IC3801003.IC3801003DataSet
'Imports Toyota.eCRB.DMSLinkage.RepairOrderSearch.DataAccess.IC3801002.IC3801002DataSet
'Imports Toyota.eCRB.DMSLinkage.RepairOrderSearch.BizLogic.IC3801003
'Imports Toyota.eCRB.CommonUtility.SMBCommonClass.Api.BizLogic
'Imports Toyota.eCRB.DMSLinkage.AddRepair.BizLogic.IC3800804
'Imports Toyota.eCRB.DMSLinkage.AddRepair.DataAccess.IC3800804.IC3800804DataSet
'Imports Toyota.eCRB.DMSLinkage.RepairOrderSearch.DataAccess.IC3801001.IC3801001DataSet
'Imports Toyota.eCRB.DMSLinkage.CustomerInfo.DataAccess.IC3800703.IC3800703DataSet
'Imports Toyota.eCRB.DMSLinkage.CustomerInfo.BizLogic.IC3800703
'Imports Toyota.eCRB.DMSLinkage.RepairOrderSearch.BizLogic.IC3801001
'Imports Toyota.eCRB.DMSLinkage.RepairOrderSearch.BizLogic.IC3801002
'Imports Toyota.eCRB.DMSLinkage.RepairOrderCreate.DataAccess.IC3801102.IC3801102DataSet
'Imports Toyota.eCRB.DMSLinkage.RepairOrderCreate.BizLogic.IC3801102
'Imports Toyota.eCRB.iCROP.BizLogic.IC3810301
'Imports Toyota.eCRB.iCROP.DataAccess.IC3810301.IC3810301DataSet


Imports Toyota.eCRB.CommonUtility.ServiceCommonClass.Api.BizLogic
Imports Toyota.eCRB.CommonUtility.ServiceCommonClass.Api.DataAccess

'2013/12/10 TMEJ 河原 TMEJ次世代サービス 工程管理機能開発 END

'2017/09/14 NSK 竹中(悠) REQ-SVT-TMT-20170227-002 見込納車遅れ日時計算ロジック統一 START
Imports Toyota.eCRB.CommonUtility.SMBCommonClass.Api.BizLogic
'2017/09/14 NSK 竹中(悠) REQ-SVT-TMT-20170227-002 見込納車遅れ日時計算ロジック統一 END

Public Class SC3220201BusinessLogic
    Inherits BaseBusinessComponent
    Implements IDisposable

#Region "Private定数"

    ''' <summary>
    ''' 完成検査有り(API)
    ''' </summary>
    ''' <remarks></remarks>
    Private Const C_COMP_INS_FLAG_ON As String = "1"

    ''' <summary>
    ''' フラグ無し
    ''' </summary>
    ''' <remarks></remarks>
    Private Const C_FLAG_OFF = "0"
    ''' <summary>
    ''' フラグ有り
    ''' </summary>
    ''' <remarks></remarks>
    Private Const C_FLAG_ON = "1"

    ''' <summary>
    ''' 実績有無:実績あり
    ''' </summary>
    ''' <remarks></remarks>
    Private Const C_RESULT_TYPE_TRUE As String = "1"

    ''' <summary>
    ''' 実績ステータス:未入庫
    ''' </summary>
    ''' <remarks></remarks>
    Private Const C_STALLPROSESS_NONE As String = "00"

    ''' <summary>
    ''' ストール予定；来店フラグ；予約
    ''' </summary>
    ''' <remarks></remarks>
    Private Const C_STALLREZINFO_WALKIN_REZ As String = "0"

    ''' <summary>
    ''' 日付フォーマット
    ''' </summary>
    ''' <remarks></remarks>
    Private Const DateFormateYYYYMMDDHHMM As String = "yyyyMMddHHmm"

    ''' <summary>
    ''' 成功
    ''' </summary>
    ''' <remarks></remarks>
    Private Const C_RET_SUCCESS As Long = 0
    ''' <summary>
    ''' エラー:DBタイムアウト
    ''' </summary>
    ''' <remarks></remarks>
    Private Const C_RET_DBTIMEOUT As Long = 901
    ''' <summary>
    ''' エラー:該当データなし
    ''' </summary>
    ''' <remarks></remarks>
    Private Const C_RET_NOMATCH As Long = 902
    ''' <summary>
    ''' エラー:更新失敗
    ''' </summary>
    ''' <remarks></remarks>
    Private Const C_RET_DBERROR As Long = 903
    ''' <summary>
    ''' エラー:SAコードが異なる
    ''' </summary>
    ''' <remarks></remarks>
    Private Const C_RET_DIFFSACODE As Long = 1
    ''' <summary>
    ''' エラー:整備受注No作成で異常発生
    ''' </summary>
    ''' <remarks></remarks>
    Private Const C_RET_CREATEORDERNO_ERROR As Long = 1

    ' 振当ステータス(API)
    ''' <summary>
    ''' 案内待ち
    ''' </summary>
    Private Const C_VISIT_STATUS_GUIDANCE_WAIT As String = "0"
    ''' <summary>
    ''' 受付待ち
    ''' </summary>
    Private Const C_VISIT_STATUS_RECEPTION_WAIT As String = "1"
    ''' <summary>
    ''' SA振当済
    ''' </summary>
    Private Const C_VISIT_STATUS_SA_ASSIGNMENT As String = "2"
    ''' <summary>
    ''' BP/保険
    ''' </summary>
    Private Const C_VISIT_STATUS_BP_INSURANCE As String = "3"
    ''' <summary>
    ''' 退店
    ''' </summary>
    Private Const C_VISIT_STATUS_OUT_SHOP As String = "4"
    ''' <summary>
    ''' HOLD中
    ''' </summary>
    Private Const C_VISIT_STATUS_HOLDING As String = "9"

    ' R/Oステータス(API)
    ''' <summary>
    ''' なし
    ''' </summary>
    ''' <remarks></remarks>
    Private Const C_RO_STATUS_NONE As String = "0"
    ''' <summary>
    ''' 受付
    ''' </summary>
    ''' <remarks></remarks>
    Private Const C_RO_STATUS_RECEPTION As String = "1"
    ''' <summary>
    ''' 作業中
    ''' </summary>
    ''' <remarks></remarks>
    Private Const C_RO_STATUS_WORKING As String = "2"
    ''' <summary>
    ''' 部品待ち
    ''' </summary>
    ''' <remarks></remarks>
    Private Const C_RO_STATUS_ITEM_WAIT As String = "4"
    ''' <summary>
    ''' 見積確認待ち
    ''' </summary>
    ''' <remarks></remarks>
    Private Const C_RO_STATUS_ESTI_WAIT As String = "5"
    ''' <summary>
    ''' 検査完了
    ''' </summary>
    ''' <remarks></remarks>
    Private Const C_RO_STATUS_INSP_OK As String = "7"
    ''' <summary>
    ''' 売上済み
    ''' </summary>
    ''' <remarks></remarks>
    Private Const C_RO_STATUS_SALE_OK As String = "3"
    ''' <summary>
    ''' 整備完了
    ''' </summary>
    ''' <remarks></remarks>
    Private Const C_RO_STATUS_MANT_OK As String = "6"
    ''' <summary>
    ''' 納車完了
    ''' </summary>
    ''' <remarks></remarks>
    Private Const C_RO_STATUS_FINISH As String = "8"

    ''' <summary>
    ''' 仕掛前
    ''' </summary>
    Private Const DisplayStartNone As String = "0"
    ''' <summary>
    ''' 仕掛中
    ''' </summary>
    Private Const DisplayStartStart As String = "1"

    ''' <summary>
    ''' 来店実績連番最小値
    ''' </summary>
    ''' <remarks></remarks>
    Private Const MinVisitSequence As Long = -1
    ''' <summary>
    ''' 予約ID最小値
    ''' </summary>
    ''' <remarks></remarks>
    Private Const MinReserveId As Long = -1

    ''' <summary>
    ''' Log開始用文言
    ''' </summary>
    Private Const LOG_START As String = "Start"
    ''' <summary>
    ''' Log終了文言
    ''' </summary>
    Private Const LOG_END As String = "End"

    '2013/12/10 TMEJ 河原 TMEJ次世代サービス 工程管理機能開発 START

    ''' <summary>
    '''洗車必要フラグ("0"：洗車無し)
    ''' </summary>
    Private Const WashNeedFlagFalse As String = "0"

    ''' <summary>
    '''洗車必要フラグ("1"：洗車有り)
    ''' </summary>
    Private Const WashNeedFlagTrue As String = "1"

    ''' <summary>
    ''' ROステータス（"50"：着工指示待ち）
    ''' </summary>
    Private Const StatusInstructionsWait As String = "50"

    ''' <summary>
    ''' ROステータス（"60"：作業中）
    ''' </summary>
    Private Const StatusWork As String = "60"

    ''' <summary>
    ''' ROステータス（"80"：納車準備）
    ''' </summary>
    Private Const StatusDeliveryWait As String = "80"

    ''' <summary>
    ''' ROステータス（"85"：納車作業）
    ''' </summary>
    Private Const StatusDeliveryWork As String = "85"

    ''' <summary>
    ''' 追加作業(承認中)("1"：追加作業有り)
    ''' </summary>
    ''' <remarks></remarks>
    Private Const ApprovalFlagOn As String = "1"

    ''' <summary>
    ''' サービスステータス(11：預かり中)
    ''' </summary>
    Private Const ServiceStatusDropOff As String = "11"

    ''' <summary>
    ''' サービスステータス（12：納車待ち）
    ''' </summary>
    ''' <remarks></remarks>
    Private Const ServiceStatusWaitDelivery As String = "12"

    '2013/12/10 TMEJ 河原 TMEJ次世代サービス 工程管理機能開発 END

#End Region

#Region "Public定数"

    ' 表示区分
    ''' <summary>
    ''' なし
    ''' </summary>
    ''' <remarks></remarks>
    Public Const DisplayDivNone As String = "0"
    ''' <summary>
    ''' 受付
    ''' </summary>
    ''' <remarks></remarks>
    Public Const DisplayDivReception As String = "1"
    ''' <summary>
    ''' 承認依頼
    ''' </summary>
    ''' <remarks></remarks>
    Public Const DisplayDivApproval As String = "2"
    ''' <summary>
    ''' 納車準備
    ''' </summary>
    ''' <remarks></remarks>
    Public Const DisplayDivPreparation As String = "3"
    ''' <summary>
    ''' 納車作業
    ''' </summary>
    ''' <remarks></remarks>
    Public Const DisplayDivDelivery As String = "4"
    ''' <summary>
    ''' 作業中
    ''' </summary>
    ''' <remarks></remarks>
    Public Const DisplayDivWork As String = "5"
    ''' <summary>
    ''' 事前準備
    ''' </summary>
    ''' <remarks></remarks>
    Public Const DisplayDivAdvance As String = "6"
    ''' <summary>
    ''' 予約
    ''' </summary>
    ''' <remarks></remarks>
    Public Const DisplayDivReserve As String = "7"

#End Region

#Region "変数"

    ''' <summary>
    ''' 納車準備_異常表示標準時間（分）
    ''' </summary>
    ''' <remarks></remarks>
    Private deliveryPreAbnormalLT As Long

    ''' <summary>
    ''' 表示区分
    ''' </summary>
    ''' <remarks></remarks>
    Private dispDiv As String

    ''' <summary>
    ''' 仕掛中
    ''' </summary>
    ''' <remarks></remarks>
    Private dispStart As String

    ''' <summary>
    ''' 現在時刻
    ''' </summary>
    ''' <remarks></remarks>
    Private nowDate As Date

#End Region

#Region "コンストラクタ"

    '''-------------------------------------------------------
    ''' <summary>
    ''' コンストラクタ
    ''' </summary>
    ''' <param name="lngDeliveryPreAbnormalLt">納車準備_異常表示標準時間（分）</param>
    ''' <param name="nowDate">現在日付</param>
    ''' <remarks></remarks>
    '''-------------------------------------------------------
    Public Sub New(ByVal lngDeliveryPreAbnormalLT As Long, ByVal nowDate As Date)
        Me.deliveryPreAbnormalLT = lngDeliveryPreAbnormalLT
        Me.nowDate = nowDate
    End Sub

#End Region

#Region "メイン処理"

    ''' <summary>
    ''' 来店チップ情報取得
    ''' </summary>
    ''' <returns>来店チップデータセット</returns>
    ''' <remarks></remarks>
    ''' <History>
    ''' 2013/12/10 TMEJ 河原 TMEJ次世代サービス 工程管理機能開発
    ''' </History>
    Public Function GetVisitChip() As SC3220201VisitChipDataTable
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} START" _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name))

        Dim staffInfo As StaffContext = StaffContext.Current

        Dim dt As SC3220201VisitDataTable

        '追加作業チップ情報
        Dim dtAddApprovalChipInfo As SC3220201AddApprovalChipInfoDataTable = Nothing

        Using da As New SC3220201DataTableAdapter
            Try
                '当日予約情報取得
                Dim dtRezAreainfo As SC3220201RezAreainfoDataTable = _
                    da.GetReserveAreaInformation(staffInfo.DlrCD, _
                                                 staffInfo.BrnCD, _
                                                 Me.nowDate)

                '2013/12/10 TMEJ 河原 TMEJ次世代サービス 工程管理機能開発 START

                ''IF検索処理
                '' SA別未納者R/O一覧
                'Dim dtIFGetNoDeliveryROList As IC3801003NoDeliveryRODataTable = _
                '    Me.GetIFNoDeliveryROList(staffInfo)


                ''来店情報取得
                'Dim dtService As SC3220201ServiceVisitManagementDataTable = _
                '    da.GetVisitManagement(staffInfo.DlrCD, _
                '                          staffInfo.BrnCD, _
                '                          Me.nowDate, _
                '                          dtIFGetNoDeliveryROList)

                '来店情報取得
                Dim dtService As SC3220201ServiceVisitManagementDataTable = _
                    da.GetVisitManagement(staffInfo.DlrCD, _
                                          staffInfo.BrnCD, _
                                          Me.nowDate)

                '2013/12/10 TMEJ 河原 TMEJ次世代サービス 工程管理機能開発 END

                '予約情報取得
                Dim dtRezinfo As SC3220201StallRezinfoDataTable = _
                    da.GetStallReserveInformation(staffInfo.DlrCD, _
                                                  staffInfo.BrnCD, _
                                                  dtService)

                '2013/12/10 TMEJ 河原 TMEJ次世代サービス 工程管理機能開発 START

                ''実績情報取得
                'Dim dtProcess As SC3220201StallProcessDataTable = _
                '    da.GetStallProcess(staffInfo.DlrCD, _
                '                       staffInfo.BrnCD, _
                '                       dtService)

                '2013/12/10 TMEJ 河原 TMEJ次世代サービス 工程管理機能開発 END

                '2013/12/10 TMEJ 河原 TMEJ次世代サービス 工程管理機能開発 START

                '来店情報が取得チェック
                If 0 < dtService.Count Then
                    '取得できた場合

                    '追加作業チップ情報取得
                    dtAddApprovalChipInfo = _
                        da.GetAddApprovalChipInfo(dtService, staffInfo.DlrCD, staffInfo.BrnCD)

                Else
                    '取得できなかった場合

                    dtAddApprovalChipInfo = New SC3220201AddApprovalChipInfoDataTable

                End If

                '' サービス来店実績・ストール予約実績取得・ストール予約アリア取得
                'dt = Me.SetVisit(dtService, dtRezinfo, dtProcess, dtRezAreainfo)

                '' IFマージ処理
                'dt = Me.SetVisitMargin(dt, dtIFGetNoDeliveryROList)

                ' サービス来店実績・ストール予約実績取得・ストール予約アリア取得
                dt = Me.SetVisit(dtService, dtRezinfo, dtRezAreainfo, dtAddApprovalChipInfo)

                '2013/12/10 TMEJ 河原 TMEJ次世代サービス 工程管理機能開発 END

            Catch ex As OracleExceptionEx When ex.Number = 1013
                'ORACLEのタイムアウトのみ処理
                Logger.Error(String.Format(CultureInfo.CurrentCulture _
                                         , "{0}.{1} OUT:RETURNCODE = {2} {3}" _
                                         , Me.GetType.ToString _
                                         , System.Reflection.MethodBase.GetCurrentMethod.Name _
                                         , C_RET_DBTIMEOUT _
                                         , ex.Message))
                Throw ex
            End Try
        End Using

        Dim dtmRezDeliDate As DateTime

        ' チップ情報チェック
        Using dtChip As New SC3220201VisitChipDataTable
            For Each row As SC3220201VisitRow In dt.Rows
                Dim rowChip As SC3220201VisitChipRow = DirectCast(dtChip.NewRow(), SC3220201VisitChipRow)

                ' チップ状態チェック
                Me.CheckChipStatus(row)

                If Me.dispDiv.Equals(DisplayDivNone) Then
                    ' 削除
                    Continue For
                End If

                '2013/12/10 TMEJ 河原 TMEJ次世代サービス 工程管理機能開発 START

                '' 納車予定日時日付変換
                'If Not String.IsNullOrEmpty(row.REZ_DELI_DATE.Trim) Then
                '    dtmRezDeliDate = DateTimeFunc.FormatString(DateFormateYYYYMMDDHHMM, row.REZ_DELI_DATE)
                'Else
                '    ' 納車予定日日時がない場合
                '    If Not Me.IsDateTimeNull(row.ENDTIME) Then
                '        ' 作業終了予定時刻＋納車準備_異常表示標準時間（分）
                '        dtmRezDeliDate = row.ENDTIME.AddMinutes(Me.deliveryPreAbnormalLT)
                '    Else
                '        dtmRezDeliDate = DateTime.MinValue
                '    End If
                'End If

                ' 納車予定日時日付変換
                If Not row.IsREZ_DELI_DATENull _
                    AndAlso row.REZ_DELI_DATE <> DateTime.MinValue Then

                    dtmRezDeliDate = row.REZ_DELI_DATE

                Else

                    ' 納車予定日日時がない場合
                    '2017/10/26 NSK 竹中(悠) REQ-SVT-TMT-20170227-002 見込納車遅れ日時計算ロジック統一 START
                    'If Not row.IsENDTIMENull _
                    '    AndAlso row.ENDTIME <> DateTime.MinValue Then

                    ' 作業終了予定時刻＋納車準備_異常表示標準時間（分）
                    'dtmRezDeliDate = row.ENDTIME.AddMinutes(Me.deliveryPreAbnormalLT)

                    'Else
                    dtmRezDeliDate = DateTime.MinValue
                    'End If
                    '2017/10/26 NSK 竹中(悠) REQ-SVT-TMT-20170227-002 見込納車遅れ日時計算ロジック統一 END
                End If

                '2013/12/10 TMEJ 河原 TMEJ次世代サービス 工程管理機能開発 END

                '2017/09/14 NSK 竹中(悠) REQ-SVT-TMT-20170227-002 見込納車遅れ日時計算ロジック統一 START
                '納車見込遅れ時刻
                Dim deliDelayDate As Date = Date.MaxValue

                '表示区分が作業中の場合
                If Me.dispDiv.Equals(DisplayDivWork) Then

                    Using smbCommonBiz As New SMBCommonClassBusinessLogic
                        '共通関数のbiz初期化処理
                        smbCommonBiz.InitCommon(StaffContext.Current.DlrCD, StaffContext.Current.BrnCD, nowDate)

                        Try
                            '納車見込遅れ時刻取得
                            deliDelayDate = smbCommonBiz.GetDeliveryDelayDate(SMBCommonClassBusinessLogic.DisplayType.Work, dtmRezDeliDate, row.ENDTIME, Nothing, _
                                                                          Nothing, Nothing, Nothing, row.WORK_TIME, _
                                                                          row.WASHFLG, nowDate, row.REMAINING_INSPECTION_TYPE)
                        Catch ex As Exception
                            Logger.Info(String.Format(CultureInfo.CurrentCulture _
                                    , "{0}.{1} Exception:{2}" _
                                    , Me.GetType.ToString _
                                    , System.Reflection.MethodBase.GetCurrentMethod.Name _
                                    , ex.Message))

                            'データ不整合
                            'Dateの最大値を設定
                            deliDelayDate = Date.MaxValue
                        End Try
                    End Using
                End If
                rowChip.DELAY_DELI_TIME = deliDelayDate
                '2017/09/14 NSK 竹中(悠) REQ-SVT-TMT-20170227-002 見込納車遅れ日時計算ロジック統一 END

                ' チップ情報形成
                rowChip = Me.GetRowChip(row, rowChip, dtmRezDeliDate)

                ' 行追加
                dtChip.AddSC3220201VisitChipRow(rowChip)

            Next

            '' 全チップ情報をログ出力
            Me.OutPutIFLog(dtChip, "SC3220201VisitChipDataTable")

            '最終処理
            If dtAddApprovalChipInfo IsNot Nothing Then

                dtAddApprovalChipInfo.Dispose()
                dtAddApprovalChipInfo = Nothing

            End If

            Logger.Info(String.Format(CultureInfo.CurrentCulture _
                        , "{0}.{1} END" _
                        , Me.GetType.ToString _
                        , System.Reflection.MethodBase.GetCurrentMethod.Name))

            '処理結果返却
            Return dtChip

        End Using

    End Function

#End Region

#Region "チップ情報取得"

    ''' <summary>
    ''' チップ情報マージ(メインエリアと予約エリア)
    ''' </summary>
    ''' <param name="dtService">サービス来店情報データセット</param>
    ''' <param name="dtRezinfo">ストール予約データセット</param>
    ''' <param name="inDtAddApprovalChipInfo">追加作業情報</param>
    ''' <returns>来店チップデータセット</returns>
    ''' <remarks></remarks>
    Private Function SetVisit(ByVal dtService As SC3220201ServiceVisitManagementDataTable, _
                              ByVal dtRezinfo As SC3220201StallRezinfoDataTable, _
                              ByVal dtRezAreainfo As SC3220201RezAreainfoDataTable, _
                              ByVal inDtAddApprovalChipInfo As SC3220201AddApprovalChipInfoDataTable) As SC3220201VisitDataTable

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} START" _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name))

        Using dt As New SC3220201VisitDataTable

            '来店チップの情報格納
            For Each rowService As SC3220201ServiceVisitManagementRow In dtService.Rows
                Dim rowVisit As SC3220201VisitRow = dt.NewSC3220201VisitRow()
                rowVisit = Me.SetVisitChipRow(rowVisit, rowService, dtRezinfo, inDtAddApprovalChipInfo)
                dt.AddSC3220201VisitRow(rowVisit)
            Next

            '予約エリアの情報格納
            For Each drRezAreainfo As SC3220201RezAreainfoRow In dtRezAreainfo
                Dim rowReserve As SC3220201VisitRow = dt.NewSC3220201VisitRow()
                rowReserve = Me.SetReserveChipRow(rowReserve, drRezAreainfo)
                dt.AddSC3220201VisitRow(rowReserve)
            Next

            Logger.Info(String.Format(CultureInfo.CurrentCulture _
                        , "{0}.{1} END SC3220201VisitChipDataTable:COUNT = {2}" _
                        , Me.GetType.ToString _
                        , System.Reflection.MethodBase.GetCurrentMethod.Name _
                        , dt.Count))

            '処理結果返却
            Return dt

        End Using

    End Function

    '2013/12/10 TMEJ 河原 TMEJ次世代サービス 工程管理機能開発 START

    ' ''' <summary>
    ' ''' 来店チップの情報格納
    ' ''' </summary>
    ' ''' <param name="rowVisit"></param>
    ' ''' <param name="rowService"></param>
    ' ''' <param name="dtRezinfo"></param>
    ' ''' <param name="dtProcess"></param>
    ' ''' <returns></returns>
    ' ''' <remarks></remarks>
    'Private Function SetVisitChipRow(ByVal rowVisit As SC3220201VisitRow, _
    '                                 ByVal rowService As SC3220201ServiceVisitManagementRow, _
    '                                 ByVal dtRezinfo As SC3220201StallRezinfoDataTable, _
    '                                 ByVal dtProcess As SC3220201StallProcessDataTable) As SC3220201VisitRow
    '    'Logger.Info(String.Format(CultureInfo.CurrentCulture _
    '    '            , "{0}.{1} START" _
    '    '            , Me.GetType.ToString _
    '    '            , System.Reflection.MethodBase.GetCurrentMethod.Name))

    '    ' 来店実績連番
    '    rowVisit.VISITSEQ = rowService.VISITSEQ
    '    ' 販売店コード
    '    rowVisit.DLRCD = rowService.DLRCD
    '    ' 店舗コード
    '    rowVisit.STRCD = rowService.STRCD
    '    ' 予約ID
    '    rowVisit.FREZID = rowService.FREZID
    '    ' 割振りSA
    '    rowVisit.SACODE = rowService.SACODE
    '    ' VIPマーク
    '    rowVisit.VIP_MARK = C_FLAG_OFF
    '    ' JDP調査対象客マーク
    '    rowVisit.JDP_MARK = C_FLAG_OFF
    '    ' 技術情報マーク
    '    rowVisit.SSC_MARK = C_FLAG_OFF
    '    ' 駐車場コード
    '    rowVisit.PARKINGCODE = rowService.PARKINGCODE
    '    ' 来店時刻
    '    rowVisit.VISITTIMESTAMP = rowService.VISITTIMESTAMP
    '    ' チェックシート有無
    '    rowVisit.CHECKSHEET_FLAG = C_FLAG_OFF
    '    ' 振当ステータス
    '    rowVisit.ASSIGNSTATUS = rowService.ASSIGNSTATUS
    '    ' SA割振り日時
    '    rowVisit.ASSIGNTIMESTAMP = rowService.ASSIGNTIMESTAMP
    '    ' 整備受注NO
    '    rowVisit.ORDERNO = rowService.ORDERNO
    '    ' 追加作業承認数
    '    rowVisit.APPROVAL_COUNT = 0
    '    ' 追加作業承認
    '    rowVisit.APPROVAL_STATUS = C_FLAG_OFF
    '    ' 追加作業承認印刷
    '    rowVisit.APPROVAL_OUTPUT = C_FLAG_OFF
    '    ' 追加作業承認依頼時刻
    '    rowVisit.APPROVAL_TIME = DateTime.MinValue
    '    ' 顧客区分
    '    rowVisit.CUSTSEGMENT = rowService.CUSTSEGMENT
    '    ' 顧客コード
    '    rowVisit.CUSTID = rowService.DMSID

    '    'ストール予約取得
    '    Dim fRezId As Long
    '    If Not rowService.IsFREZIDNull() Then
    '        fRezId = rowService.FREZID
    '    Else
    '        fRezId = MinReserveId
    '    End If

    '    Dim rowRezinfo As SC3220201StallRezinfoRow = Me.GetVisitRezinfo(fRezId, dtRezinfo)
    '    ' 予約マーク
    '    If rowRezinfo.WALKIN.Equals(C_STALLREZINFO_WALKIN_REZ) Then
    '        rowVisit.REZ_MARK = C_FLAG_ON
    '    Else
    '        rowVisit.REZ_MARK = C_FLAG_OFF
    '    End If
    '    ' 登録番号
    '    rowVisit.VCLREGNO = Me.SetReplaceString(rowService.VCLREGNO, rowRezinfo.VCLREGNO)
    '    ' 車種
    '    rowVisit.VEHICLENAME = rowRezinfo.VEHICLENAME
    '    '' モデル
    '    'グレード(MODELは英語表記上GRADE)
    '    rowVisit.GRADE = Me.SetReplaceString(rowService.MODELCODE, rowRezinfo.MODELCODE)
    '    ' VIN
    '    rowVisit.VIN = Me.SetReplaceString(rowService.VIN, rowRezinfo.VIN)
    '    ' 走行距離
    '    rowVisit.MILEAGE = rowRezinfo.MILEAGE
    '    ' 納車予定日時
    '    rowVisit.REZ_DELI_DATE = rowRezinfo.REZ_DELI_DATE
    '    ' 顧客名
    '    rowVisit.CUSTOMERNAME = Me.SetReplaceString(rowService.NAME, rowRezinfo.CUSTOMERNAME)
    '    ' 電話番号
    '    rowVisit.TELNO = Me.SetReplaceString(rowService.TELNO, rowRezinfo.TELNO)
    '    ' 携帯番号
    '    rowVisit.MOBILE = Me.SetReplaceString(rowService.MOBILE, rowRezinfo.MOBILE)
    '    ' 代表入庫項目
    '    rowVisit.MERCHANDISENAME = rowRezinfo.MERCHANDISENAME
    '    '作業開始予定時刻
    '    rowVisit.STARTTIME = rowRezinfo.STARTTIME
    '    ' 作業終了予定時刻
    '    rowVisit.ENDTIME = rowRezinfo.ENDTIME
    '    ' 作業開始
    '    rowVisit.ACTUAL_STIME = rowRezinfo.ACTUAL_STIME
    '    ' 作業終了
    '    rowVisit.ACTUAL_ETIME = rowRezinfo.ACTUAL_ETIME
    '    ' 完成検査有無
    '    rowVisit.COMP_INS_FLAG = C_COMP_INS_FLAG_ON
    '    ' 洗車有無
    '    rowVisit.WASHFLG = rowRezinfo.WASHFLG
    '    ' 予約ID
    '    rowVisit.REZINFO_FREZID = rowRezinfo.PREZID

    '    ' ストール実績取得
    '    Dim rezId As Long
    '    If Not rowService.IsREZIDNull() Then
    '        rezId = rowRezinfo.REZID
    '    Else
    '        rezId = MinReserveId
    '    End If
    '    Dim rowProcess As SC3220201StallProcessRow = Me.GetVisitProcess(fRezId, dtProcess)

    '    ' 実績ステータス
    '    rowVisit.RESULT_STATUS = rowProcess.RESULT_STATUS

    '    Dim rowProcessAsRezId As SC3220201StallProcessRow = Me.GetVisitProcess(rezId, dtProcess)

    '    ' 作業終了予定時刻
    '    If Not String.IsNullOrEmpty(rowProcessAsRezId.REZ_END_TIME) Then
    '        ' 予定_ストール終了日時時刻で更新
    '        rowVisit.ENDTIME = DateTimeFunc.FormatString(DateFormateYYYYMMDDHHMM, rowProcessAsRezId.REZ_END_TIME)
    '    End If

    '    ' 洗車開始
    '    rowVisit.RESULT_WASH_START = rowProcess.RESULT_WASH_START
    '    ' 洗車終了
    '    rowVisit.RESULT_WASH_END = rowProcess.RESULT_WASH_END
    '    ' 担当テクニシャン
    '    rowVisit.STAFFCD = 0
    '    ' 担当テクニシャン名
    '    rowVisit.STAFFNAME = rowProcess.STAFFNAME
    '    ' 予約ID
    '    rowVisit.PROCESS_FREZID = rowRezinfo.PREZID

    '    '作業開始日時(初回)
    '    rowVisit.FIRST_STARTTIME = rowProcess.FIRST_STARTTIME
    '    '使用終了日時(最後)
    '    rowVisit.LAST_ENDTIME = rowProcess.LAST_ENDTIME
    '    '作業時間(未実施合計)
    '    rowVisit.WORK_TIME = rowProcess.WORK_TIME
    '    '有効以外件数
    '    rowVisit.UNVALID_REZ_COUNT = rowProcess.UNVALID_REZ_COUNT

    '    'Logger.Info(String.Format(CultureInfo.CurrentCulture _
    '    '            , "{0}.{1} END" _
    '    '            , Me.GetType.ToString _
    '    '            , System.Reflection.MethodBase.GetCurrentMethod.Name))
    '    Return rowVisit
    'End Function

    ' ''' <summary>
    ' ''' 予約エリアチップの情報格納
    ' ''' </summary>
    ' ''' <param name="rowReserve"></param>
    ' ''' <param name="drRezAreainfo"></param>
    ' ''' <param name="dtProcess"></param>
    ' ''' <returns></returns>
    ' ''' <remarks></remarks>
    'Private Function SetReserveChipRow(ByVal rowReserve As SC3220201VisitRow, _
    '                                   ByVal drRezAreainfo As SC3220201RezAreainfoRow, _
    '                                   ByVal dtProcess As SC3220201StallProcessDataTable) As SC3220201VisitRow
    '    'Logger.Info(String.Format(CultureInfo.CurrentCulture _
    '    '            , "{0}.{1} START" _
    '    '            , Me.GetType.ToString _
    '    '            , System.Reflection.MethodBase.GetCurrentMethod.Name))

    '    ' 来店実績連番
    '    rowReserve.VISITSEQ = drRezAreainfo.VISITSEQ
    '    ' 販売店コード
    '    rowReserve.DLRCD = drRezAreainfo.DLRCD
    '    ' 店舗コード
    '    rowReserve.STRCD = drRezAreainfo.STRCD
    '    ' 予約ID
    '    rowReserve.FREZID = drRezAreainfo.REZID
    '    ' VIPマーク
    '    rowReserve.VIP_MARK = C_FLAG_OFF
    '    ' JDP調査対象客マーク
    '    rowReserve.JDP_MARK = C_FLAG_OFF
    '    ' 技術情報マーク
    '    rowReserve.SSC_MARK = C_FLAG_OFF
    '    ' 駐車場コード
    '    rowReserve.PARKINGCODE = drRezAreainfo.PARKINGCODE
    '    ' 来店時刻
    '    rowReserve.VISITTIMESTAMP = drRezAreainfo.VISITTIMESTAMP
    '    ' チェックシート有無
    '    rowReserve.CHECKSHEET_FLAG = C_FLAG_OFF
    '    ' 振当ステータス
    '    rowReserve.ASSIGNSTATUS = drRezAreainfo.ASSIGNSTATUS
    '    ' 整備受注NO
    '    rowReserve.ORDERNO = drRezAreainfo.ORDERNO
    '    ' 追加作業承認数
    '    rowReserve.APPROVAL_COUNT = 0
    '    ' 追加作業承認
    '    rowReserve.APPROVAL_STATUS = C_FLAG_OFF
    '    ' 追加作業承認印刷
    '    rowReserve.APPROVAL_OUTPUT = C_FLAG_OFF
    '    ' 追加作業承認依頼時刻
    '    rowReserve.APPROVAL_TIME = DateTime.MinValue
    '    ' 顧客区分
    '    rowReserve.CUSTSEGMENT = drRezAreainfo.CUSTSEGMENT
    '    ' 顧客コード
    '    rowReserve.CUSTID = drRezAreainfo.DMSID

    '    ' 予約マーク
    '    If drRezAreainfo.WALKIN.Equals(C_STALLREZINFO_WALKIN_REZ) Then
    '        rowReserve.REZ_MARK = C_FLAG_ON
    '    Else
    '        rowReserve.REZ_MARK = C_FLAG_OFF
    '    End If
    '    ' 登録番号
    '    rowReserve.VCLREGNO = drRezAreainfo.VCLREGNO
    '    ' 車種
    '    rowReserve.VEHICLENAME = drRezAreainfo.VEHICLENAME
    '    '' モデル
    '    'グレード(MODELは英語表記上GRADE)
    '    rowReserve.GRADE = drRezAreainfo.MODELCODE
    '    ' VIN
    '    rowReserve.VIN = drRezAreainfo.VIN
    '    ' 走行距離
    '    rowReserve.MILEAGE = drRezAreainfo.MILEAGE
    '    ' 納車予定日時
    '    rowReserve.REZ_DELI_DATE = drRezAreainfo.REZ_DELI_DATE
    '    ' 顧客名
    '    rowReserve.CUSTOMERNAME = drRezAreainfo.CUSTOMERNAME
    '    ' 電話番号
    '    rowReserve.TELNO = drRezAreainfo.TELNO
    '    ' 携帯番号
    '    rowReserve.MOBILE = drRezAreainfo.MOBILE
    '    ' 代表入庫項目
    '    rowReserve.MERCHANDISENAME = drRezAreainfo.MERCHANDISENAME
    '    '作業開始予定時刻
    '    rowReserve.STARTTIME = drRezAreainfo.STARTTIME
    '    ' 作業終了予定時刻
    '    rowReserve.ENDTIME = drRezAreainfo.ENDTIME
    '    ' 作業開始
    '    rowReserve.ACTUAL_STIME = drRezAreainfo.ACTUAL_STIME
    '    ' 作業終了
    '    rowReserve.ACTUAL_ETIME = drRezAreainfo.ACTUAL_ETIME
    '    ' 完成検査有無
    '    rowReserve.COMP_INS_FLAG = C_COMP_INS_FLAG_ON
    '    ' 洗車有無
    '    rowReserve.WASHFLG = drRezAreainfo.WASHFLG
    '    ' 予約ID
    '    rowReserve.REZINFO_FREZID = drRezAreainfo.PREZID
    '    ' 来店予定日時
    '    rowReserve.REZ_PICK_DATE = drRezAreainfo.REZ_PICK_DATE

    '    ' ストール実績取得
    '    Dim rezId As Long = drRezAreainfo.REZID
    '    Dim rowReserveProcess As SC3220201StallProcessRow = Me.GetVisitProcess(rezId, dtProcess)

    '    ' 実績ステータス
    '    rowReserve.RESULT_STATUS = rowReserveProcess.RESULT_STATUS

    '    Dim rowProcessAsRezId As SC3220201StallProcessRow = Me.GetVisitProcess(rezId, dtProcess)

    '    ' 作業終了予定時刻
    '    If Not String.IsNullOrEmpty(rowProcessAsRezId.REZ_END_TIME) Then
    '        ' 予定_ストール終了日時時刻で更新
    '        rowReserve.ENDTIME = DateTimeFunc.FormatString(DateFormateYYYYMMDDHHMM, rowProcessAsRezId.REZ_END_TIME)
    '    End If

    '    ' 洗車開始
    '    rowReserve.RESULT_WASH_START = rowReserveProcess.RESULT_WASH_START
    '    ' 洗車終了
    '    rowReserve.RESULT_WASH_END = rowReserveProcess.RESULT_WASH_END
    '    ' 担当テクニシャン
    '    rowReserve.STAFFCD = 0
    '    ' 担当テクニシャン名
    '    rowReserve.STAFFNAME = rowReserveProcess.STAFFNAME
    '    ' 予約ID
    '    rowReserve.PROCESS_FREZID = drRezAreainfo.PREZID

    '    '作業開始日時(初回)
    '    rowReserve.FIRST_STARTTIME = rowReserveProcess.FIRST_STARTTIME
    '    '使用終了日時(最後)
    '    rowReserve.LAST_ENDTIME = rowReserveProcess.LAST_ENDTIME
    '    '作業時間(未実施合計)
    '    rowReserve.WORK_TIME = rowReserveProcess.WORK_TIME
    '    '有効以外件数
    '    rowReserve.UNVALID_REZ_COUNT = rowReserveProcess.UNVALID_REZ_COUNT

    '    'Logger.Info(String.Format(CultureInfo.CurrentCulture _
    '    '            , "{0}.{1} END" _
    '    '            , Me.GetType.ToString _
    '    '            , System.Reflection.MethodBase.GetCurrentMethod.Name))
    '    Return rowReserve
    'End Function

    ' '''-------------------------------------------------------
    ' ''' <summary>
    ' ''' ストール予約取得
    ' ''' </summary>
    ' ''' <param name="fRezId">初回予約ID</param>
    ' ''' <param name="dtRezinfo">ストール予約データセット</param>
    ' ''' <returns>ストール予約レコード</returns>
    ' ''' <remarks></remarks>
    ' '''-------------------------------------------------------
    'Private Function GetVisitRezinfo(ByVal fRezId As Long, _
    '                                 ByVal dtRezinfo As SC3220201StallRezinfoDataTable) As SC3220201StallRezinfoRow
    '    'Logger.Info(String.Format(CultureInfo.CurrentCulture _
    '    '            , "{0}.{1} START FREZID={2}" _
    '    '            , Me.GetType.ToString _
    '    '            , System.Reflection.MethodBase.GetCurrentMethod.Name _
    '    '            , fRezId.ToString(CultureInfo.CurrentCulture)))

    '    Dim row As SC3220201StallRezinfoRow = dtRezinfo.NewSC3220201StallRezinfoRow()
    '    Dim aryDtRezInfo As DataRow() = dtRezinfo.Select(String.Format(CultureInfo.CurrentCulture, "PREZID = {0}", fRezId), " ENDTIME DESC, STARTTIME DESC")

    '    ' 件数チェック
    '    If aryDtRezInfo Is Nothing OrElse aryDtRezInfo.Length = 0 Then
    '        ' 該当行無し
    '        ' 販売店コード
    '        row.DLRCD = String.Empty
    '        ' 店舗コード
    '        row.STRCD = String.Empty
    '        ' 予約ID
    '        row.REZID = MinReserveId
    '        ' 管理予約ID
    '        row.PREZID = MinReserveId
    '        ' 使用開始日時
    '        row.STARTTIME = DateTime.MinValue
    '        ' 使用開始日時
    '        row.ENDTIME = DateTime.MinValue
    '        ' 顧客コード
    '        row.CUSTCD = String.Empty
    '        ' 氏名
    '        row.CUSTOMERNAME = String.Empty
    '        ' 電話番号
    '        row.TELNO = String.Empty
    '        ' 携帯番号
    '        row.MOBILE = String.Empty
    '        ' 車名
    '        row.VEHICLENAME = String.Empty
    '        ' 登録ナンバー
    '        row.VCLREGNO = String.Empty
    '        ' VIN
    '        row.VIN = String.Empty
    '        ' 商品コード
    '        row.MERCHANDISECD = String.Empty
    '        ' 商品名
    '        row.MERCHANDISENAME = String.Empty
    '        ' モデル
    '        row.MODELCODE = String.Empty
    '        ' 走行距離
    '        row.MILEAGE = -1
    '        ' 洗車有無
    '        row.WASHFLG = String.Empty
    '        ' 来店フラグ
    '        row.WALKIN = String.Empty
    '        ' 予約_納車_希望日時時刻
    '        row.REZ_DELI_DATE = String.Empty
    '        ' 作業開始
    '        row.ACTUAL_STIME = DateTime.MinValue
    '        ' 作業終了
    '        row.ACTUAL_ETIME = DateTime.MinValue
    '    Else
    '        Dim rowRezInfo As SC3220201StallRezinfoRow = DirectCast(aryDtRezInfo(0), SC3220201StallRezinfoRow)

    '        ' 販売店コード
    '        row.DLRCD = rowRezInfo.DLRCD
    '        ' 店舗コード
    '        row.STRCD = rowRezInfo.STRCD
    '        ' 予約ID
    '        row.REZID = rowRezInfo.REZID
    '        ' 管理予約ID
    '        row.PREZID = rowRezInfo.PREZID
    '        ' 使用開始日時
    '        row.STARTTIME = rowRezInfo.STARTTIME
    '        ' 使用開始日時
    '        row.ENDTIME = rowRezInfo.ENDTIME
    '        ' 顧客コード
    '        row.CUSTCD = rowRezInfo.CUSTCD
    '        ' 氏名
    '        row.CUSTOMERNAME = rowRezInfo.CUSTOMERNAME
    '        ' 電話番号
    '        row.TELNO = rowRezInfo.TELNO
    '        ' 携帯番号
    '        row.MOBILE = rowRezInfo.MOBILE
    '        ' 車名
    '        row.VEHICLENAME = rowRezInfo.VEHICLENAME
    '        ' 登録ナンバー
    '        row.VCLREGNO = rowRezInfo.VCLREGNO
    '        ' VIN
    '        row.VIN = rowRezInfo.VIN

    '        Dim dtParentRezInfo As DataRow() = dtRezinfo.Select(String.Format(CultureInfo.CurrentCulture, "REZID = {0}", fRezId))

    '        If dtParentRezInfo IsNot Nothing AndAlso dtParentRezInfo.Length = 0 Then
    '            ' 商品コード
    '            row.MERCHANDISECD = String.Empty
    '            ' 商品名
    '            row.MERCHANDISENAME = String.Empty
    '        Else
    '            Dim rowParentRezInfo As SC3220201StallRezinfoRow = DirectCast(dtParentRezInfo(0), SC3220201StallRezinfoRow)

    '            ' 商品コード
    '            row.MERCHANDISECD = rowParentRezInfo.MERCHANDISECD
    '            ' 商品名
    '            row.MERCHANDISENAME = rowParentRezInfo.MERCHANDISENAME
    '        End If

    '        ' モデル
    '        row.MODELCODE = rowRezInfo.MODELCODE
    '        ' 走行距離
    '        row.MILEAGE = rowRezInfo.MILEAGE
    '        ' 洗車有無
    '        row.WASHFLG = rowRezInfo.WASHFLG
    '        ' 来店フラグ
    '        row.WALKIN = rowRezInfo.WALKIN
    '        ' 予約_納車_希望日時時刻
    '        row.REZ_DELI_DATE = rowRezInfo.REZ_DELI_DATE
    '        ' 作業開始
    '        row.ACTUAL_STIME = rowRezInfo.ACTUAL_STIME
    '        ' 作業終了
    '        row.ACTUAL_ETIME = rowRezInfo.ACTUAL_ETIME

    '        ' 作業開始の取得(最小)
    '        Dim rowRezInfoMin As SC3220201StallRezinfoRow
    '        Dim aryDtRezInfoMin As DataRow() = dtRezinfo.Select(String.Format(CultureInfo.CurrentCulture, "PREZID = {0} AND ACTUAL_STIME <> '{1}'", fRezId, DateTime.MinValue), " ACTUAL_STIME")

    '        If aryDtRezInfoMin IsNot Nothing AndAlso aryDtRezInfoMin.Length > 0 Then
    '            rowRezInfoMin = DirectCast(aryDtRezInfoMin(0), SC3220201StallRezinfoRow)
    '            ' 作業開始
    '            row.ACTUAL_STIME = rowRezInfoMin.ACTUAL_STIME
    '        End If

    '    End If

    '    'Logger.Info(String.Format(CultureInfo.CurrentCulture _
    '    '            , "{0}.{1} END" _
    '    '            , Me.GetType.ToString _
    '    '            , System.Reflection.MethodBase.GetCurrentMethod.Name))
    '    Return row
    'End Function

    ' '''-------------------------------------------------------
    ' ''' <summary>
    ' ''' ストール実績取得
    ' ''' </summary>
    ' ''' <param name="fRezId">予約ID</param>
    ' ''' <param name="dtProcess">ストール実績データセット</param>
    ' ''' <returns>ストール実績レコード</returns>
    ' ''' <remarks></remarks>
    ' '''-------------------------------------------------------
    'Private Function GetVisitProcess(ByVal fRezId As Long, _
    '                                 ByVal dtProcess As SC3220201StallProcessDataTable) As SC3220201StallProcessRow
    '    'Logger.Info(String.Format(CultureInfo.CurrentCulture _
    '    '            , "{0}.{1} START FREZID={2}" _
    '    '            , Me.GetType.ToString _
    '    '            , System.Reflection.MethodBase.GetCurrentMethod.Name _
    '    '            , fRezId.ToString(CultureInfo.CurrentCulture)))

    '    Dim row As SC3220201StallProcessRow = dtProcess.NewSC3220201StallProcessRow()

    '    Dim aryProcess As DataRow() = dtProcess.Select(String.Format(CultureInfo.CurrentCulture, _
    '                                                                 "PREZID = {0}", fRezId))
    '    ' 件数チェック
    '    If aryProcess Is Nothing OrElse aryProcess.Length = 0 Then
    '        ' 洗車有無
    '        row.WASHFLG = String.Empty
    '        ' 実績_ステータス
    '        row.RESULT_STATUS = C_STALLPROSESS_NONE
    '        ' 予定_ストール終了日時時刻
    '        row.REZ_END_TIME = String.Empty
    '        ' 洗車開始
    '        row.RESULT_WASH_START = String.Empty
    '        ' 洗車終了
    '        row.RESULT_WASH_END = String.Empty
    '        ' 担当テクニシャン
    '        row.STAFFCD = String.Empty
    '        ' 担当テクニシャン名
    '        row.STAFFNAME = String.Empty
    '        '作業開始日時(初回)
    '        row.FIRST_STARTTIME = Nothing
    '        '使用終了日時(最後)
    '        row.LAST_ENDTIME = Date.MinValue
    '        '作業時間(未実施合計)
    '        row.WORK_TIME = 0
    '        '有効以外件数
    '        row.UNVALID_REZ_COUNT = 0
    '    Else
    '        Dim rowProcess As SC3220201StallProcessRow

    '        rowProcess = DirectCast(aryProcess(0), SC3220201StallProcessRow)

    '        row.WASHFLG = rowProcess.WASHFLG

    '        ' 予定_ストール終了日時時刻
    '        row.REZ_END_TIME = rowProcess.REZ_END_TIME
    '        ' 洗車開始
    '        row.RESULT_WASH_START = rowProcess.RESULT_WASH_START
    '        ' 洗車終了
    '        row.RESULT_WASH_END = rowProcess.RESULT_WASH_END

    '        If C_RESULT_TYPE_TRUE.Equals(rowProcess.RESULT_TYPE) Then
    '            '実績ありの場合

    '            ' 実績_ステータス
    '            row.RESULT_STATUS = rowProcess.RESULT_STATUS
    '            ' 担当テクニシャン
    '            row.STAFFCD = rowProcess.STAFFCD
    '            ' 担当テクニシャン名
    '            row.STAFFNAME = rowProcess.STAFFNAME
    '        Else
    '            '実績なしの場合

    '            ' 実績_ステータス
    '            row.RESULT_STATUS = String.Empty
    '            ' 担当テクニシャン
    '            row.STAFFCD = String.Empty
    '            ' 担当テクニシャン名
    '            row.STAFFNAME = String.Empty
    '        End If

    '        '作業開始日時(初回)
    '        If rowProcess.IsFIRST_STARTTIMENull Then
    '            row.FIRST_STARTTIME = Nothing
    '        Else
    '            row.FIRST_STARTTIME = rowProcess.FIRST_STARTTIME
    '        End If
    '        '使用終了日時(最後)
    '        If rowProcess.IsLAST_ENDTIMENull Then
    '            row.LAST_ENDTIME = Date.MinValue
    '        Else
    '            row.LAST_ENDTIME = rowProcess.LAST_ENDTIME
    '        End If
    '        '作業時間(未実施合計)
    '        If rowProcess.IsWORK_TIMENull Then
    '            row.WORK_TIME = 0
    '        Else
    '            row.WORK_TIME = rowProcess.WORK_TIME
    '        End If

    '        '有効以外件数
    '        If rowProcess.IsUNVALID_REZ_COUNTNull Then
    '            row.UNVALID_REZ_COUNT = 0
    '        Else
    '            row.UNVALID_REZ_COUNT = rowProcess.UNVALID_REZ_COUNT
    '        End If
    '    End If

    '    'Logger.Info(String.Format(CultureInfo.CurrentCulture _
    '    '            , "{0}.{1} END" _
    '    '            , Me.GetType.ToString _
    '    '            , System.Reflection.MethodBase.GetCurrentMethod.Name))
    '    Return row
    'End Function

    ' '''-------------------------------------------------------
    ' ''' <summary>
    ' ''' サービス来店実績マージ(チップ情報)
    ' ''' </summary>
    ' ''' <param name="dt">サービス来店情報</param>
    ' ''' <param name="dtIFNoDeliveryRO">SA別未納者R/O一覧</param>
    ' ''' <returns>来店チップデータセット</returns>
    ' ''' <remarks></remarks>
    ' '''-------------------------------------------------------
    'Private Function SetVisitMargin(ByVal dt As SC3220201VisitDataTable, _
    '                                ByVal dtIFNoDeliveryRO As IC3801003NoDeliveryRODataTable) As SC3220201VisitDataTable
    '    Logger.Info(String.Format(CultureInfo.CurrentCulture _
    '                , "{0}.{1} START" _
    '                , Me.GetType.ToString _
    '                , System.Reflection.MethodBase.GetCurrentMethod.Name))

    '    Dim rowNoDeliveryRO As IC3801003NoDeliveryRORow
    '    Dim aryRow As DataRow()

    '    For Each row As SC3220201VisitRow In dt.Rows

    '        aryRow = dtIFNoDeliveryRO.Select(String.Format(CultureInfo.CurrentCulture, "ORDERNO = '{0}'", row.ORDERNO))

    '        If aryRow IsNot Nothing AndAlso aryRow.Length > 0 Then
    '            rowNoDeliveryRO = DirectCast(aryRow(0), IC3801003NoDeliveryRORow)

    '            If Not IsDBNull(rowNoDeliveryRO.Item("ORDERNO")) Then
    '                row.ORDERNO = Me.SetReplaceString(row.ORDERNO, rowNoDeliveryRO.ORDERNO)                                'R/O No
    '            End If
    '            If Not IsDBNull(rowNoDeliveryRO.Item("ORDERSTATUS")) Then
    '                row.RO_STATUS = Me.SetReplaceString(String.Empty, rowNoDeliveryRO.ORDERSTATUS)                         'R/Oステータス
    '            End If
    '            If Not IsDBNull(rowNoDeliveryRO.Item("IFLAG")) Then
    '                row.JDP_MARK = Me.SetReplaceString(String.Empty, rowNoDeliveryRO.IFLAG)                                'JDP調査対象客フラグ
    '            End If
    '            If Not IsDBNull(rowNoDeliveryRO.Item("SFLAG")) Then
    '                row.SSC_MARK = Me.SetReplaceString(String.Empty, rowNoDeliveryRO.SFLAG)                                'SSCフラグ
    '            End If
    '            If Not IsDBNull(rowNoDeliveryRO.Item("CUSTOMERNAME")) Then
    '                row.CUSTOMERNAME = Me.SetReplaceString(row.CUSTOMERNAME, rowNoDeliveryRO.CUSTOMERNAME)                 '顧客名
    '            End If
    '            If Not IsDBNull(rowNoDeliveryRO.Item("REGISTERNO")) Then
    '                row.VCLREGNO = Me.SetReplaceString(row.VCLREGNO, rowNoDeliveryRO.REGISTERNO)                           '車両登録No.
    '            End If
    '            If Not IsDBNull(rowNoDeliveryRO.Item("ADDSRVCOUNT")) Then
    '                ' データ先空白チェック
    '                If Not String.IsNullOrEmpty(rowNoDeliveryRO.ADDSRVCOUNT) Then
    '                    ' データ先あり
    '                    row.APPROVAL_COUNT = CType(rowNoDeliveryRO.ADDSRVCOUNT, Long)                                       '追加作業数
    '                End If
    '            End If
    '            If Not IsDBNull(rowNoDeliveryRO.Item("DELIVERYHOPEDATE")) Then
    '                '納車予定時間はR/O情報を優先に表示
    '                ' データ先空白チェック
    '                If Not String.IsNullOrEmpty(rowNoDeliveryRO.DELIVERYHOPEDATE) Then
    '                    ' データ先あり
    '                    Dim deliveryHopeDate = CType(rowNoDeliveryRO.DELIVERYHOPEDATE.ToString(CultureInfo.CurrentCulture), DateTime).ToString(DateFormateYYYYMMDDHHMM, CultureInfo.CurrentCulture)
    '                    row.REZ_DELI_DATE = Me.SetReplaceString(deliveryHopeDate, row.REZ_DELI_DATE)            '納車予定時刻
    '                End If

    '            End If

    '            If Not rowNoDeliveryRO.IsCOLSINGPRINTTIMENull Then
    '                row.COLSINGPRINTTIME = rowNoDeliveryRO.COLSINGPRINTTIME             '清算書印刷時刻
    '            Else
    '                row.COLSINGPRINTTIME = Date.MinValue
    '            End If

    '            If Not rowNoDeliveryRO.IsEXAMINETIMENull Then
    '                row.EXAMINETIME = rowNoDeliveryRO.EXAMINETIME                       '完成検査完了時刻
    '            Else
    '                row.EXAMINETIME = Date.MinValue
    '            End If
    '        End If
    '    Next

    '    Logger.Info(String.Format(CultureInfo.CurrentCulture _
    '                , "{0}.{1} END" _
    '                , Me.GetType.ToString _
    '                , System.Reflection.MethodBase.GetCurrentMethod.Name))
    '    Return dt
    'End Function

    '2013/12/10 TMEJ 河原 TMEJ次世代サービス 工程管理機能開発 END

    ''' <summary>
    ''' 来店チップの情報格納
    ''' </summary>
    ''' <param name="rowVisit"></param>
    ''' <param name="rowService"></param>
    ''' <param name="dtRezinfo"></param>
    ''' <param name="inDtAddApprovalChipInfo">追加作業情報</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    ''' <history>
    ''' 2013/12/10 TMEJ 河原 TMEJ次世代サービス 工程管理機能開発
    ''' </history>
    Private Function SetVisitChipRow(ByVal rowVisit As SC3220201VisitRow, _
                                     ByVal rowService As SC3220201ServiceVisitManagementRow, _
                                     ByVal dtRezinfo As SC3220201StallRezinfoDataTable, _
                                     ByVal inDtAddApprovalChipInfo As SC3220201AddApprovalChipInfoDataTable) As SC3220201VisitRow

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} START" _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name))

        ' 来店実績連番
        rowVisit.VISITSEQ = rowService.VISITSEQ
        ' 予約ID
        rowVisit.FREZID = rowService.FREZID
        ' 予約マーク
        rowVisit.REZ_MARK = rowService.REZ_MARK
        ' JDP調査対象客マーク
        rowVisit.JDP_MARK = rowService.JDP_MARK
        '2018/06/29 NSK 可児 TKM Next Gen e-CRB Project Application development Block B-1  Pマーク、Lマークなどを表示 START
        ' スマイル年間保守フラグ
        rowVisit.SML_AMC_FLG = rowService.SML_AMC_FLG
        ' 延長保守フラグ
        rowVisit.EW_FLG = rowService.EW_FLG
        ' テレマ会員フラグ
        rowVisit.TLM_MBR_FLG = rowService.TLM_MBR_FLG
        '2018/06/29 NSK 可児 TKM Next Gen e-CRB Project Application development Block B-1  Pマーク、Lマークなどを表示 END
        ' 技術情報マーク
        rowVisit.SSC_MARK = C_FLAG_OFF
        ' 来店時刻
        rowVisit.VISITTIMESTAMP = rowService.VISITTIMESTAMP
        ' 振当ステータス
        rowVisit.ASSIGNSTATUS = rowService.ASSIGNSTATUS
        ' SA割振り日時
        rowVisit.ASSIGNTIMESTAMP = rowService.ASSIGNTIMESTAMP
        ' 整備受注NO
        rowVisit.ORDERNO = rowService.ORDERNO
        ' 顧客区分
        rowVisit.CUSTSEGMENT = rowService.CUSTSEGMENT
        ' 車両登録番号
        rowVisit.VCLREGNO = rowService.VCLREGNO
        ' 顧客名
        rowVisit.CUSTOMERNAME = rowService.NAME
        '2018/02/26 NSK 山田 17PRJ01947-00_(トライ店システム評価)サービス重要顧客における、識別及び対処オペレーションの検証 START
        ' SSC対象客マーク
        rowVisit.SSC_MARK = rowService.SSC_MARK
        '2018/02/26 NSK 山田 17PRJ01947-00_(トライ店システム評価)サービス重要顧客における、識別及び対処オペレーションの検証 END

        ' RO_INFOテーブルの来店実績連番チェック
        If Not rowService.IsVISIT_IDNull Then

            ' RO情報の有無(RO_INFOテーブルの来店実績連番)
            rowVisit.VISIT_ID = rowService.VISIT_ID

        End If

        ' ROステータス(最大)
        rowVisit.RO_STATUS = rowService.MAX_RO_STATUS
        ' ROステータス(最小)
        rowVisit.MIN_RO_STATUS = rowService.MIN_RO_STATUS


        '追加作業(現在承認中)があるかチェック
        If 0 < inDtAddApprovalChipInfo.Select(String.Format(CultureInfo.CurrentCulture, "VISIT_ID = {0}", rowVisit.VISITSEQ), "").Count Then
            '追加作業(現在承認中)が存在する場合

            '追加作業フラグをON
            rowVisit.APPROVAL_FLAG = ApprovalFlagOn

        End If


        '追加作業件数チェック
        If 0 < rowService.RO_SEQ Then
            '追加作業有り

            '追加作業件数
            rowVisit.APPROVAL_COUNT = CType(rowService.MAX_RO_SEQ, Long)

        Else
            '追加作業無し

            '追加作業件数(-1)
            rowVisit.APPROVAL_COUNT = -1

        End If

        '予約情報チェック
        If 0 < rowVisit.FREZID Then

            '予約情報取得
            Dim rowRezinfo As SC3220201StallRezinfoRow = Me.GetVisitRezinfo(rowVisit.FREZID, dtRezinfo)

            '予約情報のチェック
            If rowRezinfo IsNot Nothing Then
                '予約有り

                ' 予約マーク
                rowVisit.REZ_MARK = rowRezinfo.REZ_MARK
                ' 納車予定日時
                rowVisit.REZ_DELI_DATE = rowRezinfo.REZ_DELI_DATE
                ' 代表入庫項目
                rowVisit.MERCHANDISENAME = rowRezinfo.MERCHANDISENAME
                ' 作業開始日時
                rowVisit.ACTUAL_STIME = rowRezinfo.ACTUAL_STIME
                ' 作業終了予定時刻(最後)
                rowVisit.ENDTIME = rowRezinfo.ENDTIME
                ' 洗車有無
                rowVisit.WASHFLG = rowRezinfo.WASHFLG
                ' 洗車開始日時
                rowVisit.RESULT_WASH_START = rowRezinfo.WASH_START
                ' 洗車終了日時
                rowVisit.RESULT_WASH_END = rowRezinfo.WASH_END

                '2013/12/10 TMEJ 河原 TMEJ次世代サービス 工程管理機能開発 START

                ' サービスステータス
                rowVisit.RESULT_STATUS = rowRezinfo.RESULT_STATUS

                '2013/12/10 TMEJ 河原 TMEJ次世代サービス 工程管理機能開発 END

                '2017/09/14 NSK 竹中(悠) REQ-SVT-TMT-20170227-002 見込納車遅れ日時計算ロジック統一 START
                '残作業時間
                rowVisit.WORK_TIME = rowRezinfo.WORK_TIME

                '残完成検査区分
                rowVisit.REMAINING_INSPECTION_TYPE = rowRezinfo.REMAINING_INSPECTION_TYPE
                '2017/09/14 NSK 竹中(悠) REQ-SVT-TMT-20170227-002 見込納車遅れ日時計算ロジック統一 END

            Else
                '上記以外

                '不整合データのため

                'RO情報のクリア
                rowVisit.RO_STATUS = String.Empty

                '振当ステータスクリア
                rowVisit.ASSIGNSTATUS = C_VISIT_STATUS_OUT_SHOP

            End If

        Else
            '予約無しの場合

            '整備受注番号のチェック
            If Not String.IsNullOrEmpty(rowVisit.ORDERNO) Then
                '整備受注番号が存在する場合

                '予約無しでROが顧客承認されている場合は
                '不整合データのため

                'RO情報のクリア
                rowVisit.RO_STATUS = String.Empty

                '振当ステータスクリア
                rowVisit.ASSIGNSTATUS = C_VISIT_STATUS_OUT_SHOP

            End If


        End If


        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} END" _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name))

        Return rowVisit
    End Function

    ''' <summary>
    ''' 予約エリアチップの情報格納
    ''' </summary>
    ''' <param name="rowReserve"></param>
    ''' <param name="drRezAreainfo"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function SetReserveChipRow(ByVal rowReserve As SC3220201VisitRow, _
                                       ByVal drRezAreainfo As SC3220201RezAreainfoRow) As SC3220201VisitRow

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} START" _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name))

        ' 来店実績連番
        rowReserve.VISITSEQ = drRezAreainfo.VISITSEQ
        ' 販売店コード
        rowReserve.DLRCD = drRezAreainfo.DLRCD
        ' 店舗コード
        rowReserve.STRCD = drRezAreainfo.STRCD
        ' 予約ID
        rowReserve.FREZID = drRezAreainfo.REZID
        ' JDP調査対象客マーク
        rowReserve.JDP_MARK = drRezAreainfo.JDP_MARK
        '2018/06/29 NSK 可児 TKM Next Gen e-CRB Project Application development Block B-1  Pマーク、Lマークなどを表示 START
        ' スマイル年間保守フラグ
        rowReserve.SML_AMC_FLG = drRezAreainfo.SML_AMC_FLG
        ' 延長保守フラグ
        rowReserve.EW_FLG = drRezAreainfo.EW_FLG
        ' テレマ会員フラグ
        rowReserve.TLM_MBR_FLG = drRezAreainfo.TLM_MBR_FLG
        '2018/06/29 NSK 可児 TKM Next Gen e-CRB Project Application development Block B-1  Pマーク、Lマークなどを表示 END
        ' 技術情報マーク
        rowReserve.SSC_MARK = C_FLAG_OFF
        ' 来店時刻
        rowReserve.VISITTIMESTAMP = drRezAreainfo.VISITTIMESTAMP
        ' 振当ステータス
        rowReserve.ASSIGNSTATUS = drRezAreainfo.ASSIGNSTATUS
        ' 整備受注NO
        rowReserve.ORDERNO = drRezAreainfo.ORDERNO
        ' 顧客区分
        rowReserve.CUSTSEGMENT = drRezAreainfo.CUSTSEGMENT

        ' 予約マーク
        If drRezAreainfo.WALKIN.Equals(C_STALLREZINFO_WALKIN_REZ) Then
            rowReserve.REZ_MARK = C_FLAG_ON
        Else
            rowReserve.REZ_MARK = C_FLAG_OFF
        End If

        ' 登録番号
        rowReserve.VCLREGNO = drRezAreainfo.VCLREGNO
        ' 納車予定日時
        rowReserve.REZ_DELI_DATE = drRezAreainfo.REZ_DELI_DATE
        ' 顧客名
        rowReserve.CUSTOMERNAME = drRezAreainfo.CUSTOMERNAME
        ' 代表入庫項目
        rowReserve.MERCHANDISENAME = drRezAreainfo.MERCHANDISENAME
        ' 作業開始予定時刻
        rowReserve.STARTTIME = drRezAreainfo.STARTTIME
        ' 作業終了予定時刻(最終)
        rowReserve.ENDTIME = drRezAreainfo.ENDTIME
        ' 洗車有無
        rowReserve.WASHFLG = drRezAreainfo.WASHFLG
        ' 来店予定日時
        rowReserve.REZ_PICK_DATE = drRezAreainfo.REZ_PICK_DATE
        '追加作業件数(-1)
        rowReserve.APPROVAL_COUNT = -1
        '2018/02/26 NSK 山田 17PRJ01947-00_(トライ店システム評価)サービス重要顧客における、識別及び対処オペレーションの検証 START
        ' SSC対象客マーク
        rowReserve.SSC_MARK = drRezAreainfo.SSC_MARK
        '2018/02/26 NSK 山田 17PRJ01947-00_(トライ店システム評価)サービス重要顧客における、識別及び対処オペレーションの検証 END

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} END" _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name))

        Return rowReserve

    End Function

    ''' <summary>
    ''' 予約情報取得
    ''' </summary>
    ''' <param name="fRezId">初回予約ID</param>
    ''' <param name="dtRezinfo">ストール予約データセット</param>
    ''' <returns>ストール予約レコード</returns>
    ''' <remarks></remarks>
    ''' <history>
    ''' 2013/12/10 TMEJ 河原 TMEJ次世代サービス 工程管理機能開発
    ''' </history>
    Private Function GetVisitRezinfo(ByVal fRezId As Decimal, _
                                     ByVal dtRezinfo As SC3220201StallRezinfoDataTable) As SC3220201StallRezinfoRow

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} START FREZID={2}" _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name _
                    , fRezId.ToString(CultureInfo.CurrentCulture)))

        Dim row As SC3220201StallRezinfoRow = dtRezinfo.NewSC3220201StallRezinfoRow()
        Dim aryDtRezInfo As DataRow() = dtRezinfo.Select(String.Format(CultureInfo.CurrentCulture, "PREZID = {0}", fRezId), " ENDTIME DESC, STARTTIME DESC")

        ' 件数チェック
        If aryDtRezInfo Is Nothing OrElse aryDtRezInfo.Length = 0 Then
            ' 該当行無し

            Return Nothing

        Else
            Dim rowRezInfo As SC3220201StallRezinfoRow = DirectCast(aryDtRezInfo(0), SC3220201StallRezinfoRow)

            ' 管理予約ID
            row.PREZID = rowRezInfo.PREZID
            ' 作業開始日時
            row.ACTUAL_STIME = rowRezInfo.ACTUAL_STIME
            ' 作業終了予定日時(最終)
            row.ENDTIME = rowRezInfo.ENDTIME
            ' 商品名
            row.MERCHANDISENAME = rowRezInfo.MERCHANDISENAME
            ' 洗車有無
            row.WASHFLG = rowRezInfo.WASHFLG
            ' 予約フラグ
            row.REZ_MARK = rowRezInfo.REZ_MARK
            ' 納車予定日時
            row.REZ_DELI_DATE = rowRezInfo.REZ_DELI_DATE
            ' 洗車開始日時
            row.WASH_START = rowRezInfo.WASH_START
            ' 洗車終了日時
            row.WASH_END = rowRezInfo.WASH_END

            '2013/12/10 TMEJ 河原 TMEJ次世代サービス 工程管理機能開発 START

            ' サービスステータス
            row.RESULT_STATUS = rowRezInfo.RESULT_STATUS

            '2013/12/10 TMEJ 河原 TMEJ次世代サービス 工程管理機能開発 END

            '2017/09/14 NSK 竹中(悠) REQ-SVT-TMT-20170227-002 見込納車遅れ日時計算ロジック統一 START
            '残作業時間
            row.WORK_TIME = rowRezInfo.WORK_TIME

            '残完成検査区分
            row.REMAINING_INSPECTION_TYPE = rowRezInfo.REMAINING_INSPECTION_TYPE
            '2017/09/14 NSK 竹中(悠) REQ-SVT-TMT-20170227-002 見込納車遅れ日時計算ロジック統一 END

        End If

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} END" _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name))

        Return row

    End Function

    ''' <summary>
    ''' チップ状態チェック
    ''' </summary>
    ''' <param name="row">来店チップレコード</param>
    ''' <remarks></remarks>
    ''' <history>
    ''' 2012/11/29 TMEJ 丁   【SERVICE_2】SAストール予約受付機能開発（仕分けNo.72）
    ''' 2013/01/10 TMEJ 小澤  【SERVICE_2】次世代サービスROステータス切り離し対応
    ''' 2013/12/10 TMEJ 河原 TMEJ次世代サービス 工程管理機能開発
    ''' </history>
    Private Sub CheckChipStatus(ByVal row As SC3220201VisitRow)

        '2013/12/10 TMEJ 河原 TMEJ次世代サービス 工程管理機能開発 START

        'Private Sub CheckChipStatus(ByVal row As SC3220201VisitRow)

        '2013/12/10 TMEJ 河原 TMEJ次世代サービス 工程管理機能開発 END

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} START" _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name))

        '★★★表示エリア判定★★★
        '■■仕掛中判定■■


        ' 0 : 予約チェック
        ' 振当ステータスチェック
        If String.IsNullOrEmpty(row.ASSIGNSTATUS) Then
            '「データ無し」の場合
            Me.dispDiv = DisplayDivReserve    ' ★★★予約★★★
            Me.dispStart = DisplayStartNone   ' ■■仕掛前■■
            Return
        ElseIf New String() {C_VISIT_STATUS_GUIDANCE_WAIT, C_VISIT_STATUS_RECEPTION_WAIT, C_VISIT_STATUS_HOLDING}.Contains(row.ASSIGNSTATUS) Then
            '「0：案内待ち、1：受付待ち、9：HOLD中」の場合
            Me.dispDiv = DisplayDivReserve    ' ★★★予約★★★
            Me.dispStart = DisplayStartStart  ' ■■仕掛中■■
            Return
        End If

        '2013/12/10 TMEJ 河原 TMEJ次世代サービス 工程管理機能開発 START

        '' 1 : 受付チェック
        '' 整備受注NOチェック
        'If String.IsNullOrEmpty(row.ORDERNO) Then
        '    ' 整備受注NOなし
        '    Me.dispDiv = DisplayDivReception  ' 受付
        '    Me.dispStart = DisplayStartNone   ' 仕掛前
        '    Return
        'End If

        '' R/Oステータスチェック
        'If row.RO_STATUS.Equals(C_RO_STATUS_RECEPTION) Then
        '    ' R/Oステータス：受付
        '    Me.dispDiv = DisplayDivReception  ' 受付
        '    Me.dispStart = DisplayStartStart  ' 仕掛中
        '    Return

        'ElseIf row.RO_STATUS.Equals(C_RO_STATUS_ESTI_WAIT) Then
        '    ' R/Oステータス：見積確認待ち
        '    Me.dispDiv = DisplayDivReception  ' 受付
        '    Me.dispStart = DisplayStartStart  ' 仕掛中
        '    Return
        'End If

        '' 2 : 作業中チェック

        '' R/Oステータスチェック
        'If row.RO_STATUS.Equals(C_RO_STATUS_WORKING) Or
        '   row.RO_STATUS.Equals(C_RO_STATUS_ITEM_WAIT) Then
        '    ' R/Oステータス：作業中、部品待ち、検査完了

        '    ' ストール予約取得チェック
        '    If row.REZINFO_FREZID > MinReserveId Then
        '        ' ストール予約あり

        '        '作業中エリアに設定
        '        Me.dispDiv = DisplayDivWork       ' 作業中
        '        ' 作業開始時間チェック
        '        ' 作業終了時間チェック
        '        If Not IsDateTimeNull(row.ACTUAL_STIME) And _
        '            Not IsDateTimeNull(row.ACTUAL_ETIME) Then
        '            ' 作業開始時間あり
        '            ' 作業終了時間あり
        '            Me.dispStart = DisplayStartStart  ' 仕掛中
        '            Return

        '        ElseIf IsDateTimeNull(row.ACTUAL_STIME) And _
        '                IsDateTimeNull(row.ACTUAL_ETIME) Then
        '            ' 作業開始時間なし
        '            ' 作業終了時間なし
        '            Me.dispStart = DisplayStartNone   ' 仕掛前
        '            Return

        '        ElseIf Not IsDateTimeNull(row.ACTUAL_STIME) And _
        '                    IsDateTimeNull(row.ACTUAL_ETIME) Then
        '            ' 作業開始時間あり
        '            ' 作業終了時間なし
        '            Me.dispStart = DisplayStartStart  ' 仕掛中
        '            Return
        '        End If
        '    End If
        'End If

        '' 3 : 納車準備チェック

        '' R/Oステータスチェック
        'If row.RO_STATUS.Equals(C_RO_STATUS_INSP_OK) Then
        '    ' R/Oステータス：検査完了

        '    ' ストール予約・実績取得チェック
        '    If row.REZINFO_FREZID > MinReserveId And
        '        row.PROCESS_FREZID > MinReserveId Then
        '        ' ストール予約・実績あり
        '        Dim deliveryCheck As Boolean
        '        deliveryCheck = Me.CheckChipStatusDelivery(row)
        '        If deliveryCheck = True Then
        '            Return
        '        End If
        '    End If
        'End If

        '' 4 : 納車作業チェック

        '' R/Oステータスチェック
        'If row.RO_STATUS.Equals(C_RO_STATUS_SALE_OK) Then
        '    ' R/Oステータス：売上済み
        '    Me.dispDiv = DisplayDivDelivery       ' 納車作業
        '    Me.dispStart = DisplayStartStart      ' 仕掛中
        '    Return

        'ElseIf row.RO_STATUS.Equals(C_RO_STATUS_MANT_OK) Then
        '    ' R/Oステータス：整備完了
        '    Me.dispDiv = DisplayDivDelivery       ' 納車作業
        '    Me.dispStart = DisplayStartStart      ' 仕掛中
        '    Return

        'End If

        '' 5 : 完了チェック 

        '' R/Oステータスチェック
        'If row.RO_STATUS.Equals(C_RO_STATUS_FINISH) Then
        '    ' R/Oステータス：納車完了
        '    Me.dispDiv = DisplayDivNone       ' なし
        '    Me.dispStart = DisplayStartNone   ' 仕掛前
        '    Return
        'End If

        ' 1 : 受付チェック
        ' 整備受注NOチェック
        If String.IsNullOrEmpty(row.ORDERNO) Then
            ' 整備受注NOなし

            Me.dispDiv = DisplayDivReception  ' ★★★受付エリア★★★

            'ROが発行されているかチェック
            If row.IsVISIT_IDNull Then
                '発行されていない

                Me.dispStart = DisplayStartNone   ' ■■仕掛前■■

            Else
                '発行されている

                Me.dispStart = DisplayStartStart  ' ■■仕掛中■■

            End If

            ' 処理終了
            Return

        End If


        'ROステータス(1番進んでいるステータス)で判定する
        Select Case row.RO_STATUS

            Case StatusInstructionsWait,
                 StatusWork                 '▲▲50：着工指示待ち、60：作業中▲▲

                Me.dispDiv = DisplayDivWork       ' ★★★作業中★★★

                '作業開始しているかチェック
                If row.ACTUAL_STIME = Date.MinValue Then
                    '作業開始前

                    Me.dispStart = DisplayStartNone   ' ■■仕掛前■■

                Else
                    '作業開始後

                    Me.dispStart = DisplayStartStart  ' ■■仕掛中■■

                End If

                ' 処理終了
                Return

            Case StatusDeliveryWait         '▲▲80：納車準備待ち▲▲

                'ROステータスが80で納車準備状態でも追加作業が存在すれば作業中へ
                '追加作業(現在承認中)の存在チェック
                If ApprovalFlagOn.Equals(row.APPROVAL_FLAG) Then
                    '追加作業(現在承認中)が存在する場合

                    Me.dispDiv = DisplayDivWork       ' ★★★作業中★★★
                    Me.dispStart = DisplayStartStart  ' ■■仕掛中■■

                Else
                    '追加作業(現在承認中)が存在しない場合

                    '最小のROステータスチェック
                    If StatusDeliveryWait.Equals(row.MIN_RO_STATUS) Then
                        '全てのROステータスが80：納車準備待ちの場合

                        Me.dispDiv = DisplayDivPreparation       ' ★★★納車準備★★★

                        'サービスステータスチェック
                        If ServiceStatusDropOff.Equals(row.RESULT_STATUS) OrElse _
                           ServiceStatusWaitDelivery.Equals(row.RESULT_STATUS) Then
                            '「11：預かり中」、「12：納車待ち」の場合

                            Me.dispStart = DisplayStartStart        ' ■■仕掛中■■

                        Else
                            '上記以外の場合
                            '洗車有りかつ洗車開始していないかチェック
                            If WashNeedFlagTrue.Equals(row.WASHFLG) _
                                AndAlso row.RESULT_WASH_START = Date.MinValue Then
                                '洗車有りかつ洗車開始していない場合

                                Me.dispStart = DisplayStartNone   ' ■■仕掛前■■

                            Else
                                '上記以外の場合

                                Me.dispStart = DisplayStartStart  ' ■■仕掛中■■

                            End If


                        End If

                    Else
                        'ROステータスが80：納車準備待ち未満があるがある場合
                        '全てのチップが終了していない場合

                        Me.dispDiv = DisplayDivWork       ' ★★★作業中★★★
                        Me.dispStart = DisplayStartStart  ' ■■仕掛中■■

                    End If

                End If

                ' 処理終了
                Return

            Case StatusDeliveryWork         '▲▲85：納車作業中▲▲

                'サービスステータスチェック
                If ServiceStatusDropOff.Equals(row.RESULT_STATUS) OrElse _
                   ServiceStatusWaitDelivery.Equals(row.RESULT_STATUS) Then
                    '「11：預かり中」、「12：納車待ち」の場合

                    '★★★納車★★★

                    Me.dispDiv = DisplayDivDelivery         ' ★★★納車★★★
                    Me.dispStart = DisplayStartStart        ' ■■仕掛中■■


                Else
                    '上記以外
                    'ROステータスが85でも洗車が終わっていなければ納車準備へ
                    '洗車有りかつ洗車完了実績が入ってない場合
                    If WashNeedFlagTrue.Equals(row.WASHFLG) _
                        AndAlso row.RESULT_WASH_END = Date.MinValue Then

                        '洗車が終了していない

                        Me.dispDiv = DisplayDivPreparation       ' ★★★納車準備★★★
                        Me.dispStart = DisplayStartStart         ' ■■仕掛中■■

                    Else
                        '上記以外

                        '★★★納車★★★

                        Me.dispDiv = DisplayDivDelivery         ' ★★★納車★★★
                        Me.dispStart = DisplayStartStart        ' ■■仕掛中■■

                    End If

                End If


                ' 処理終了
                Return

            Case Else

                ' その他 
                Me.dispDiv = DisplayDivNone       ' なし
                Me.dispStart = DisplayStartNone   ' 仕掛前

                ' 処理終了
                Return

        End Select

        '2013/12/10 TMEJ 河原 TMEJ次世代サービス 工程管理機能開発 END

        ' その他 
        Me.dispDiv = DisplayDivNone       ' なし
        Me.dispStart = DisplayStartNone   ' 仕掛前


        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} END" _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name))

    End Sub

    '2013/12/10 TMEJ 河原 TMEJ次世代サービス 工程管理機能開発 START

    ' ''' <summary>
    ' ''' 納車準備チェック
    ' ''' </summary>
    ' ''' <param name="row">来店チップレコード</param>
    ' ''' <remarks></remarks>
    'Private Function CheckChipStatusDelivery(ByVal row As SC3220201VisitRow) As Boolean
    '    Logger.Info(String.Format(CultureInfo.CurrentCulture _
    '                , "{0}.{1} START" _
    '                , Me.GetType.ToString _
    '                , System.Reflection.MethodBase.GetCurrentMethod.Name))

    '    Dim bizAddRepairStatusList As New IC3800804BusinessLogic
    '    Dim dtAddRepairStatus As IC3800804AddRepairStatusDataTableDataTable = _
    '        DirectCast(bizAddRepairStatusList.GetAddRepairStatusList(row.DLRCD, row.ORDERNO),  _
    '            IC3800804AddRepairStatusDataTableDataTable)

    '    If Not IsNothing(dtAddRepairStatus) Or 0 < dtAddRepairStatus.Count Then
    '        '追加作業が存在する場合
    '        Dim rowAddList As IC3800804AddRepairStatusDataTableRow() = _
    '            (From col In dtAddRepairStatus Where col.STATUS <> "9" Select col).ToArray
    '        If 0 < rowAddList.Count Then
    '            '「追加作業ステータス≠9」が存在する場合
    '            Me.dispDiv = DisplayDivWork       ' 作業中
    '            Me.dispStart = DisplayStartStart  ' 仕掛中

    '            Logger.Info(String.Format(CultureInfo.CurrentCulture _
    '                        , "{0}.{1} END" _
    '                        , Me.GetType.ToString _
    '                        , System.Reflection.MethodBase.GetCurrentMethod.Name))
    '            Return True
    '        Else
    '            '「追加作業ステータス≠9」が存在しない場合
    '            If "1".Equals(row.WASHFLG) AndAlso _
    '               (row.IsRESULT_WASH_STARTNull OrElse _
    '               String.IsNullOrEmpty(row.RESULT_WASH_START) OrElse _
    '               row.RESULT_WASH_START.Count = 0) Then
    '                '「洗車フラグ＝1:有 AndAlso 洗車開始時刻＝データ無し」の場合
    '                Me.dispDiv = DisplayDivPreparation    ' 納車準備
    '                Me.dispStart = DisplayStartNone       ' 仕掛前

    '                Logger.Info(String.Format(CultureInfo.CurrentCulture _
    '                            , "{0}.{1} END" _
    '                            , Me.GetType.ToString _
    '                            , System.Reflection.MethodBase.GetCurrentMethod.Name))
    '                Return True
    '            Else
    '                Me.dispDiv = DisplayDivPreparation    ' 納車準備
    '                Me.dispStart = DisplayStartStart      ' 仕掛中

    '                Logger.Info(String.Format(CultureInfo.CurrentCulture _
    '                            , "{0}.{1} END" _
    '                            , Me.GetType.ToString _
    '                            , System.Reflection.MethodBase.GetCurrentMethod.Name))
    '                Return True
    '            End If
    '        End If
    '    Else
    '        '追加作業が存在しない場合
    '        If "1".Equals(row.WASHFLG) AndAlso _
    '           (row.IsRESULT_WASH_STARTNull OrElse _
    '           String.IsNullOrEmpty(row.RESULT_WASH_START) OrElse _
    '           row.RESULT_WASH_START.Count = 0) Then
    '            '「洗車フラグ＝1:有 AndAlso 洗車開始時刻＝データ無し」の場合
    '            Me.dispDiv = DisplayDivPreparation    ' 納車準備
    '            Me.dispStart = DisplayStartNone       ' 仕掛前

    '            Logger.Info(String.Format(CultureInfo.CurrentCulture _
    '                        , "{0}.{1} END" _
    '                        , Me.GetType.ToString _
    '                        , System.Reflection.MethodBase.GetCurrentMethod.Name))
    '            Return True
    '        Else
    '            Me.dispDiv = DisplayDivPreparation    ' 納車準備
    '            Me.dispStart = DisplayStartStart      ' 仕掛中

    '            Logger.Info(String.Format(CultureInfo.CurrentCulture _
    '                        , "{0}.{1} END" _
    '                        , Me.GetType.ToString _
    '                        , System.Reflection.MethodBase.GetCurrentMethod.Name))
    '            Return True
    '        End If
    '    End If

    '    Logger.Info(String.Format(CultureInfo.CurrentCulture _
    '                , "{0}.{1} END" _
    '                , Me.GetType.ToString _
    '                , System.Reflection.MethodBase.GetCurrentMethod.Name))
    '    Return False
    'End Function


    '2013/12/10 TMEJ 河原 TMEJ次世代サービス 工程管理機能開発 END

    ''' <summary>
    ''' チップ情報形成
    ''' </summary>
    ''' <param name="row">来店チップレコード</param>
    ''' <param name="rowChip">チップ情報設定レコード</param>
    ''' <param name="dtmRezDeliDate">納車予定日時</param>
    ''' <returns>チップ情報設定レコード</returns>
    ''' <remarks></remarks>
    ''' <history>
    ''' 2013/12/10 TMEJ 河原 TMEJ次世代サービス 工程管理機能開発
    ''' </history>
    Private Function GetRowChip(ByVal row As SC3220201VisitRow _
                              , ByVal rowChip As SC3220201VisitChipRow _
                              , ByVal dtmRezDeliDate As DateTime) As SC3220201VisitChipRow

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} START" _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name))

        '2013/12/10 TMEJ 河原 TMEJ次世代サービス 工程管理機能開発 START

        '' 来店実績連番
        'rowChip.VISITSEQ = row.VISITSEQ
        '' 販売店コード
        'rowChip.DLRCD = row.DLRCD
        '' 店舗コード
        'rowChip.STRCD = row.STRCD
        '' 予約ID
        'rowChip.FREZID = row.FREZID
        '' 表示区分
        'rowChip.DISP_DIV = Me.dispDiv
        '' 仕掛中
        'rowChip.DISP_START = Me.dispStart
        '' VIPマーク
        'rowChip.VIP_MARK = row.VIP_MARK
        '' 予約マーク
        'rowChip.REZ_MARK = row.REZ_MARK
        '' JDP調査対象客マーク
        'rowChip.JDP_MARK = row.JDP_MARK
        '' 技術情報マーク
        'rowChip.SSC_MARK = row.SSC_MARK
        '' 顧客名
        'rowChip.CUSTOMERNAME = row.CUSTOMERNAME
        '' R/O発行確認
        'If String.IsNullOrEmpty(row.ORDERNO) AndAlso (Me.dispDiv.Equals(DisplayDivReception) OrElse Me.dispDiv.Equals(DisplayDivReserve)) Then
        '    'R/O未発行かつ受付エリアのチップの場合、JDP調査対象客マーク、技術情報マークがないので顧客情報から取得            
        '    Dim dtIFSrvCustomerDataTable As IC3800703SrvCustomerDataTable
        '    'VINがなければ取得しない
        '    If (Not String.IsNullOrEmpty(row.VIN.Trim())) OrElse _
        '        (Not String.IsNullOrEmpty(row.VCLREGNO.Trim())) Then
        '        'IF-顧客参照処理
        '        dtIFSrvCustomerDataTable = Me.GetIFCustomerInformation(row)
        '        Dim rowIFSrvCustomer As IC3800703SrvCustomerFRow

        '        If dtIFSrvCustomerDataTable IsNot Nothing _
        '            AndAlso dtIFSrvCustomerDataTable.Rows.Count > 0 Then

        '            rowIFSrvCustomer = _
        '                DirectCast(dtIFSrvCustomerDataTable.Rows(0), IC3800703SrvCustomerFRow)
        '            'JDP調査対象客マーク
        '            If Not IsDBNull(rowIFSrvCustomer.Item("JDPFLAG")) Then
        '                rowChip.JDP_MARK = rowIFSrvCustomer.JDPFLAG    'JDP調査対象客マーク
        '            End If
        '            '技術情報マーク
        '            If Not IsDBNull(rowIFSrvCustomer.Item("SSCFLAG")) Then
        '                rowChip.SSC_MARK = rowIFSrvCustomer.SSCFLAG    '技術情報マーク
        '            End If

        '            '顧客名
        '            If Not IsDBNull(rowIFSrvCustomer.Item("BUYERNAME")) Then
        '                rowChip.CUSTOMERNAME = rowIFSrvCustomer.BUYERNAME
        '            End If
        '        End If
        '    End If
        'End If
        '' 登録番号
        'rowChip.VCLREGNO = row.VCLREGNO
        '' 代表入庫項目
        'rowChip.MERCHANDISENAME = row.MERCHANDISENAME
        '' 駐車場コード
        'rowChip.PARKINGCODE = row.PARKINGCODE
        '' 担当テクニシャン名
        'rowChip.STAFFNAME = row.STAFFNAME
        '' 追加作業承認数
        'rowChip.APPROVAL_COUNT = row.APPROVAL_COUNT
        '' 整備受注NO
        'rowChip.ORDERNO = row.ORDERNO
        '' 追加承認待ちID
        'rowChip.APPROVAL_ID = String.Empty
        ''納車予定時刻
        'rowChip.REZ_DELI_DATE = dtmRezDeliDate
        ''洗車フラグ
        'rowChip.WASHFLG = row.WASHFLG

        ' 来店実績連番
        rowChip.VISITSEQ = row.VISITSEQ
        ' 予約ID
        rowChip.FREZID = row.FREZID
        ' 表示区分
        rowChip.DISP_DIV = Me.dispDiv
        ' 仕掛中
        rowChip.DISP_START = Me.dispStart
        ' 予約マーク
        rowChip.REZ_MARK = row.REZ_MARK
        ' JDP調査対象客マーク
        rowChip.JDP_MARK = row.JDP_MARK
        '2018/06/29 NSK 可児 TKM Next Gen e-CRB Project Application development Block B-1  Pマーク、Lマークなどを表示 START
        ' スマイル年間保守フラグ
        rowChip.SML_AMC_FLG = row.SML_AMC_FLG
        ' 延長保守フラグ
        rowChip.EW_FLG = row.EW_FLG
        ' テレマ会員フラグ
        rowChip.TLM_MBR_FLG = row.TLM_MBR_FLG
        '2018/06/29 NSK 可児 TKM Next Gen e-CRB Project Application development Block B-1  Pマーク、Lマークなどを表示 END
        ' 技術情報マーク
        rowChip.SSC_MARK = row.SSC_MARK
        ' 顧客名
        rowChip.CUSTOMERNAME = row.CUSTOMERNAME
        ' 登録番号
        rowChip.VCLREGNO = row.VCLREGNO
        ' 代表入庫項目
        rowChip.MERCHANDISENAME = row.MERCHANDISENAME
        ' 整備受注NO
        rowChip.ORDERNO = row.ORDERNO
        ' 納車予定時刻
        rowChip.REZ_DELI_DATE = dtmRezDeliDate
        ' 洗車フラグ
        rowChip.WASHFLG = row.WASHFLG
        ' 追加作業件数
        rowChip.APPROVAL_COUNT = row.APPROVAL_COUNT

        ' RO_INFOテーブルの来店実績連番チェック
        If Not row.IsVISIT_IDNull Then

            ' RO情報の有無(RO_INFOテーブルの来店実績連番)
            rowChip.VISIT_ID = row.VISIT_ID

        End If

        ' ROステータス(最大)
        rowChip.MAX_RO_STATUS = row.RO_STATUS
        ' 顧客名ROステータス(最小)
        rowChip.MIN_RO_STATUS = row.MIN_RO_STATUS

        '2013/12/10 TMEJ 河原 TMEJ次世代サービス 工程管理機能開発 END

        'チップに表示する日付などのデータを格納
        rowChip = SetVariousDate(row, rowChip, dtmRezDeliDate)

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} END" _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name))

        Return rowChip

    End Function

    ''' <summary>
    ''' 各エリアの日付情報格納
    ''' </summary>
    ''' <param name="row">来店チップレコード</param>
    ''' <param name="rowChip">チップ情報設定レコード</param>
    ''' <param name="dtmRezDeliDate">納車予定日時</param>
    ''' <returns>チップ情報設定レコード</returns>
    ''' <remarks></remarks>
    Private Function SetVariousDate(ByVal row As SC3220201VisitRow, _
                                    ByVal rowChip As SC3220201VisitChipRow, _
                                    ByVal dtmRezDeliDate As DateTime) As SC3220201VisitChipRow
        'Logger.Info(String.Format(CultureInfo.CurrentCulture _
        '            , "{0}.{1} START" _
        '            , Me.GetType.ToString _
        '            , System.Reflection.MethodBase.GetCurrentMethod.Name))
        ' チップ情報チェック
        Select Case Me.dispDiv
            Case DisplayDivReserve       ' 予約
                ' 表示日時
                If Not (row.VISITTIMESTAMP = Date.MinValue) Then
                    rowChip.ITEM_DATE = row.VISITTIMESTAMP          ' 来店実績日時
                ElseIf Not (row.REZ_PICK_DATE = Date.MinValue) Then
                    rowChip.ITEM_DATE = row.REZ_PICK_DATE           ' 来店予定日時
                ElseIf Not (row.STARTTIME = Date.MinValue) Then
                    rowChip.ITEM_DATE = row.STARTTIME               ' 作業開始予定日時
                End If

                '来店実績日時の設定
                rowChip.VISITTIMESTAMP = row.VISITTIMESTAMP          ' 来店実績日時

                ' 遅れ条件のデータ
                If row.VISITTIMESTAMP = Date.MinValue Then
                    If Not (row.REZ_PICK_DATE = Date.MinValue) Then
                        rowChip.PROC_DATE = row.REZ_PICK_DATE           ' 来店予定日時
                    ElseIf Not (row.STARTTIME = Date.MinValue) Then
                        rowChip.PROC_DATE = row.STARTTIME               ' 作業開始予定日時
                    End If
                Else
                    rowChip.PROC_DATE = row.VISITTIMESTAMP              ' 来店実績日時
                End If

                ' 表示順
                rowChip.DISP_SORT = String.Format(CultureInfo.CurrentCulture, "{0}{1}", _
                                                  rowChip.ITEM_DATE.ToString("yyyyMMddHHmmss", CultureInfo.CurrentCulture), _
                                                  row.VCLREGNO)

            Case DisplayDivReception       ' 受付
                ' 表示日時
                rowChip.ITEM_DATE = dtmRezDeliDate          ' 納車予定日時

                ' 遅れ条件のデータ
                rowChip.PROC_DATE = row.ASSIGNTIMESTAMP     ' SA割振り日時

                ' 表示順
                rowChip.DISP_SORT = _
                    row.ASSIGNTIMESTAMP.ToString("yyyyMMddHHmmss", CultureInfo.CurrentCulture)  ' SA割振り日時

            Case DisplayDivWork            ' 作業中
                ' 表示日時
                rowChip.ITEM_DATE = dtmRezDeliDate          ' 納車予定日時

                ' 遅れ条件のデータ
                rowChip.PROC_DATE = dtmRezDeliDate          ' 納車予定日時

                ' 表示順  (作業終了予定時刻＋SA割振り日時)
                rowChip.DISP_SORT = String.Format(CultureInfo.CurrentCulture, "{0}{1}",
                        row.ENDTIME.ToString("yyyyMMddHHmmss", CultureInfo.CurrentCulture),
                        row.ASSIGNTIMESTAMP.ToString("yyyyMMddHHmmss", CultureInfo.CurrentCulture))

            Case DisplayDivPreparation     ' 洗車/精算
                ' 表示日時
                rowChip.ITEM_DATE = dtmRezDeliDate          ' 納車予定日時

                ' 遅れ条件のデータ
                rowChip.PROC_DATE = dtmRezDeliDate          ' 納車予定日時

                ' 表示順 (納車予定日時＋ SA割振り日時)
                rowChip.DISP_SORT = String.Format(CultureInfo.CurrentCulture, "{0}{1}",
                       dtmRezDeliDate.ToString("yyyyMMddHHmmss", CultureInfo.CurrentCulture),
                       row.ASSIGNTIMESTAMP.ToString("yyyyMMddHHmmss", CultureInfo.CurrentCulture))

            Case DisplayDivDelivery        ' 納車作業
                ' 表示日時
                rowChip.ITEM_DATE = dtmRezDeliDate          ' 納車予定日時

                ' 遅れ条件のデータ
                rowChip.PROC_DATE = dtmRezDeliDate          ' 納車予定日時

                ' 表示順  (納車予定日時＋ SA割振り日時)
                rowChip.DISP_SORT = String.Format(CultureInfo.CurrentCulture, "{0}{1}",
                            dtmRezDeliDate.ToString("yyyyMMddHHmmss", CultureInfo.CurrentCulture),
                            row.ASSIGNTIMESTAMP.ToString("yyyyMMddHHmmss", CultureInfo.CurrentCulture))

            Case Else
                ' 表示日時
                rowChip.ITEM_DATE = DateTime.MinValue
                ' 遅れ条件のデータ
                rowChip.PROC_DATE = DateTime.MinValue
                ' 表示順
                rowChip.DISP_SORT = String.Empty
        End Select

        'Logger.Info(String.Format(CultureInfo.CurrentCulture _
        '            , "{0}.{1} END" _
        '            , Me.GetType.ToString _
        '            , System.Reflection.MethodBase.GetCurrentMethod.Name))
        Return rowChip
    End Function

#End Region

    '    '2013/12/10 TMEJ 河原 TMEJ次世代サービス 工程管理機能開発 START

    '#Region "共通"

    '    '''-------------------------------------------------------
    '    ''' <summary>
    '    ''' 時間チェック
    '    ''' </summary>
    '    ''' <param name="time">対象時間</param>
    '    ''' <returns>True:正常値 False:エラー値</returns>
    '    ''' <remarks></remarks>
    '    '''-------------------------------------------------------
    '    Private Function IsDateTimeNull(ByVal time As DateTime) As Boolean

    '        ' 日付チェック
    '        If time.Equals(DateTime.MinValue) Then
    '            Return True
    '        End If

    '        Return False

    '    End Function

    '    '''-------------------------------------------------------
    '    ''' <summary>
    '    ''' データ置換
    '    ''' </summary>
    '    ''' <param name="valBefore">データ元</param>
    '    ''' <param name="valAfter">データ先</param>
    '    ''' <returns>置換データ</returns>
    '    ''' <remarks></remarks>
    '    '''-------------------------------------------------------
    '    Private Function SetReplaceString(ByVal valBefore As String, ByVal valAfter As String) As String

    '        ' データ元存在チェック
    '        If Not String.IsNullOrEmpty(valBefore.Trim()) Then
    '            ' データ元あり
    '            Return valBefore
    '        End If

    '        ' データ先空白チェック
    '        If String.IsNullOrEmpty(valAfter.Trim()) Then
    '            ' データ先なし
    '            Return valBefore
    '        End If

    '        ' データ先で置換
    '        Return valAfter

    '    End Function

    '#End Region

    '    '2013/12/10 TMEJ 河原 TMEJ次世代サービス 工程管理機能開発 END

#Region "外部IF処理"

    '2013/12/10 TMEJ 河原 TMEJ次世代サービス 工程管理機能開発 START

    ' '''-------------------------------------------------------
    ' ''' <summary>
    ' ''' SA別未納者R/O一覧
    ' ''' </summary>
    ' ''' <param name="staffInfo">スタッフ情報</param>
    ' ''' <returns>SA別未納者R/O一覧データセット</returns>
    ' ''' <remarks></remarks>
    ' '''-------------------------------------------------------
    'Private Function GetIFNoDeliveryROList(ByVal staffInfo As StaffContext) As IC3801003NoDeliveryRODataTable
    '    Logger.Info(String.Format(CultureInfo.CurrentCulture _
    '                , "{0}.{1} START" _
    '                , Me.GetType.ToString _
    '                , System.Reflection.MethodBase.GetCurrentMethod.Name))

    '    Dim bl As IC3801003BusinessLogic = New IC3801003BusinessLogic
    '    Dim dt As IC3801003NoDeliveryRODataTable

    '    '検索処理
    '    'IF用ログ
    '    Logger.Info(String.Format(CultureInfo.CurrentCulture _
    '                              , "CALL IF:IC3801003BusinessLogic.GetNoDeliveryROList IN:dlrcd={0}" _
    '                              , staffInfo.DlrCD))
    '    Try
    '        dt = bl.GetNoDeliveryROList(staffInfo.DlrCD, String.Empty, "0")
    '    Finally
    '        If bl IsNot Nothing Then
    '            bl = Nothing
    '        End If
    '    End Try

    '    ' IF戻り値をログ出力
    '    Me.OutPutIFLog(dt, "IC3801003BusinessLogic.GetNoDeliveryROList")

    '    Logger.Info(String.Format(CultureInfo.CurrentCulture _
    '                              , "CALL IF:IC3801003BusinessLogic.GetNoDeliveryROList OUT:Count = {0}" _
    '                              , dt.Rows.Count))

    '    Logger.Info(String.Format(CultureInfo.CurrentCulture _
    '                , "{0}.{1} END" _
    '                , Me.GetType.ToString _
    '                , System.Reflection.MethodBase.GetCurrentMethod.Name))
    '    Return dt
    'End Function

    ' '''-------------------------------------------------------
    ' ''' <summary>
    ' ''' 顧客参照
    ' ''' </summary>
    ' ''' <param name="serviceInfo">来店実績データロウ</param>
    ' ''' <returns>顧客情報格納データセット</returns>
    ' ''' <remarks></remarks>
    ' '''-------------------------------------------------------
    'Public Function GetIFCustomerInformation(ByVal serviceInfo As SC3220201VisitRow) As IC3800703SrvCustomerDataTable
    '    Logger.Info(String.Format(CultureInfo.CurrentCulture _
    '                , "{0}.{1} START" _
    '                , Me.GetType.ToString _
    '                , System.Reflection.MethodBase.GetCurrentMethod.Name))

    '    If serviceInfo IsNot Nothing Then
    '        Dim bl As IC3800703BusinessLogic = New IC3800703BusinessLogic
    '        Try
    '            'IF用ログ
    '            Logger.Info(String.Format(CultureInfo.CurrentCulture _
    '                                      , "CALL IF:IC3800703BusinessLogic.GetCustomerInfo IN:registerNo={0}, vinNo={1}, dealerCode={2}" _
    '                                      , serviceInfo.VCLREGNO _
    '                                      , serviceInfo.VIN _
    '                                      , serviceInfo.DLRCD))
    '            ' 顧客参照処理
    '            Dim dt As IC3800703SrvCustomerDataTable = bl.GetCustomerInfo(serviceInfo.VCLREGNO, serviceInfo.VIN, serviceInfo.DLRCD)
    '            ' IF戻り値をログ出力
    '            Me.OutPutIFLog(dt, "IC3800703BusinessLogic.GetCustomerInfo")

    '            Logger.Info(String.Format(CultureInfo.CurrentCulture _
    '                                      , "CALL IF:IC3800703BusinessLogic.GetCustomerInfo OUT:Count = {0}" _
    '                                      , dt.Rows.Count))

    '            Logger.Info(String.Format(CultureInfo.CurrentCulture _
    '                        , "{0}.{1} END" _
    '                        , Me.GetType.ToString _
    '                        , System.Reflection.MethodBase.GetCurrentMethod.Name))
    '            '処理結果返却
    '            Return dt
    '        Finally
    '            If bl IsNot Nothing Then
    '                bl = Nothing
    '            End If
    '        End Try
    '    Else

    '        Logger.Info(String.Format(CultureInfo.CurrentCulture _
    '                    , "{0}.{1} END" _
    '                    , Me.GetType.ToString _
    '                    , System.Reflection.MethodBase.GetCurrentMethod.Name))
    '        Return Nothing
    '    End If
    'End Function

    '2013/12/10 TMEJ 河原 TMEJ次世代サービス 工程管理機能開発 END

    ''' <summary>
    ''' サービス標準LT取得
    ''' </summary>
    ''' <param name="inDealerCode">販売店コード</param>
    ''' <param name="inStoreCode">店舗コード</param>
    ''' <returns>標準LT</returns>
    ''' <remarks></remarks>
    Public Function GetStandardLTList(ByVal inDealerCode As String, _
                                      ByVal inStoreCode As String) As StandardLTListDataTable
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} START" _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name))

        Dim bl As New IC3810701BusinessLogic
        Dim dt As StandardLTListDataTable

        Try
            dt = bl.GetStandardLTList(inDealerCode, inStoreCode)
        Catch ex As OracleExceptionEx When ex.Number = 1013
            Throw
        Finally
            If bl IsNot Nothing Then
                bl.Dispose()
                bl = Nothing
            End If
        End Try


        ' IF戻り値をログ出力
        Me.OutPutIFLog(dt, "IC3800703BusinessLogic.GetCustomerInfo")

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                                  , "CALL IF:IC3800703BusinessLogic.GetCustomerInfo OUT:Count = {0}" _
                                  , dt.Rows.Count))

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} END" _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name))
        Return dt
    End Function

    '2013/12/10 TMEJ 河原 次世代サービス 工程管理機能開発 START

    ''' <summary>
    ''' 基幹コードへ変換処理
    ''' 販売店コード・店舗コード・アカウントをそれぞれ
    ''' 基幹販売店コード・基幹店舗コード・基幹アカウントに変換
    ''' </summary>
    ''' <param name="inStaffInfo">スタッフ情報</param>
    ''' <remarks>基幹コード情報ROW</remarks>
    ''' <history>
    ''' </history>
    Public Function ChangeDmsCode(ByVal inStaffInfo As StaffContext) _
                                  As ServiceCommonClassDataSet.DmsCodeMapRow

        '開始ログ
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                  , "{0}.{1} {2} IN:DLRCD = {3} STRCD = {4} ACCOUNT = {5} " _
                  , Me.GetType.ToString _
                  , System.Reflection.MethodBase.GetCurrentMethod.Name _
                  , LOG_START _
                  , inStaffInfo.DlrCD, inStaffInfo.BrnCD, inStaffInfo.Account))

        'SMBCommonClassBusinessLogicのインスタンス
        Using smbCommon As New ServiceCommonClassBusinessLogic


            '基幹コードへ変換処理
            Dim dtDmsCodeMap As ServiceCommonClassDataSet.DmsCodeMapDataTable = _
                smbCommon.GetIcropToDmsCode(inStaffInfo.DlrCD, _
                                            ServiceCommonClassBusinessLogic.DmsCodeType.BranchCode, _
                                            inStaffInfo.DlrCD, _
                                            inStaffInfo.BrnCD, _
                                            String.Empty, _
                                            inStaffInfo.Account)

            '基幹コード情報Row
            Dim rowDmsCodeMap As ServiceCommonClassDataSet.DmsCodeMapRow

            '基幹コードへ変換処理結果チェック
            If dtDmsCodeMap IsNot Nothing AndAlso 0 < dtDmsCodeMap.Rows.Count Then
                '基幹コードへ変換処理成功

                'Rowに変換
                rowDmsCodeMap = CType(dtDmsCodeMap.Rows(0), ServiceCommonClassDataSet.DmsCodeMapRow)

                '基幹アカウントチェック
                If rowDmsCodeMap.IsACCOUNTNull Then
                    '値無し

                    '空文字を設定する
                    '基幹アカウント
                    rowDmsCodeMap.ACCOUNT = String.Empty

                End If

                '基幹販売店コードチェック
                If rowDmsCodeMap.IsCODE1Null Then
                    '値無し

                    '空文字を設定する
                    '基幹販売店コード
                    rowDmsCodeMap.CODE1 = String.Empty

                End If

                '基幹店舗コードチェック
                If rowDmsCodeMap.IsCODE2Null Then
                    '値無し

                    '空文字を設定する
                    '基幹店舗コード
                    rowDmsCodeMap.CODE2 = String.Empty

                End If

            Else
                '基幹コードへ変換処理成功失敗

                '新しいRowを作成
                rowDmsCodeMap = CType(dtDmsCodeMap.NewDmsCodeMapRow, ServiceCommonClassDataSet.DmsCodeMapRow)

                '空文字を設定する
                '基幹アカウント
                rowDmsCodeMap.ACCOUNT = String.Empty
                '基幹販売店コード
                rowDmsCodeMap.CODE1 = String.Empty
                '基幹店舗コード
                rowDmsCodeMap.CODE2 = String.Empty

            End If


            '終了ログ
            Logger.Info(String.Format(CultureInfo.CurrentCulture _
                       , "{0}.{1} {2} dtDmsCodeMap:COUNT = {3}" _
                       , Me.GetType.ToString _
                       , System.Reflection.MethodBase.GetCurrentMethod.Name _
                       , LOG_END _
                       , dtDmsCodeMap.Count))

            '結果返却
            Return rowDmsCodeMap

        End Using

    End Function

    '2013/12/10 TMEJ 河原 次世代サービス 工程管理機能開発 END


#End Region

#Region "ログ出力(IF戻り値用)"

    ''' <summary>
    ''' ログ出力(IF戻り値用)
    ''' </summary>
    ''' <param name="dt">戻り値(DataTable)</param>
    ''' <param name="ifName">使用IF名</param>
    ''' <remarks></remarks>
    Private Sub OutPutIFLog(ByVal dt As DataTable, ByVal ifName As String)

        If dt Is Nothing Then
            Return
        End If

        Logger.Info(ifName + " Result START " + " OutPutCount: " + CType(dt.Rows.Count, String))

        Dim log As New Text.StringBuilder

        For j = 0 To dt.Rows.Count - 1

            log = New Text.StringBuilder()
            Dim dr As DataRow = dt.Rows(j)

            log.Append("RowNum: " + CType(j + 1, String) + " -- ")

            For i = 0 To dt.Columns.Count - 1
                log.Append(dt.Columns(i).Caption)
                If IsDBNull(dr(i)) Then
                    log.Append(" IS NULL")
                Else
                    log.Append(" = ")
                    log.Append(dr(i).ToString)
                End If

                If i <= dt.Columns.Count - 2 Then
                    log.Append(", ")
                End If
            Next

            Logger.Info(log.ToString)
        Next

        Logger.Info(ifName + " Result END ")

    End Sub

#End Region

#Region "IDisposable Support"
    Private disposedValue As Boolean ' 重複する呼び出しを検出するには

    ' IDisposable
    Protected Overridable Sub Dispose(disposing As Boolean)
        If Not Me.disposedValue Then
            If disposing Then
                ' TODO: マネージ状態を破棄します (マネージ オブジェクト)。
            End If

            ' TODO: アンマネージ リソース (アンマネージ オブジェクト) を解放し、下の Finalize() をオーバーライドします。
            ' TODO: 大きなフィールドを null に設定します。
        End If
        Me.disposedValue = True
    End Sub

    ' このコードは、破棄可能なパターンを正しく実装できるように Visual Basic によって追加されました。
    Public Sub Dispose() Implements IDisposable.Dispose
        ' このコードを変更しないでください。クリーンアップ コードを上の Dispose(ByVal disposing As Boolean) に記述します。
        Dispose(True)
        GC.SuppressFinalize(Me)
    End Sub

#End Region

End Class

