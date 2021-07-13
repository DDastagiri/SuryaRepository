'━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
'SC3140103BusinessLogic.vb
'─────────────────────────────────────
'機能： メインメニュー(SA) ビジネスロジック
'補足： 
'作成： 2012/01/16 KN 小林
'─────────────────────────────────────
Option Explicit On
Option Strict On

Imports System.Text
Imports Toyota.eCRB.SystemFrameworks.Core
Imports Toyota.eCRB.SystemFrameworks.Core.iCROP.BizLogic
Imports Toyota.eCRB.SystemFrameworks.Web
Imports Toyota.eCRB.iCROP.DataAccess.SC3140103.SC3140103DataSet
Imports Toyota.eCRB.iCROP.DataAccess.SC3140103.SC3140103DataSetTableAdapters

Imports Toyota.eCRB.iCROP.BizLogic.IC3810301
Imports Toyota.eCRB.iCROP.DataAccess.IC3810301
Imports Toyota.eCRB.iCROP.DataAccess.IC3810301.IC3810301DataSet
Imports Toyota.eCRB.iCROP.DataAccess.IC3810301.IC3810301DataSetTableAdapters

Imports Toyota.eCRB.DMSLinkage.RepairOrderCreate.BizLogic.IC3801102
Imports Toyota.eCRB.DMSLinkage.RepairOrderCreate.DataAccess.IC3801102
Imports Toyota.eCRB.DMSLinkage.RepairOrderCreate.DataAccess.IC3801102.IC3801102DataSet
Imports Toyota.eCRB.DMSLinkage.RepairOrderCreate.DataAccess.IC3801102.IC3801102TableAdapters

Imports Toyota.eCRB.DMSLinkage.RepairOrderSearch.BizLogic.IC3801001
Imports Toyota.eCRB.DMSLinkage.RepairOrderSearch.DataAccess.IC3801001
Imports Toyota.eCRB.DMSLinkage.RepairOrderSearch.DataAccess.IC3801001.IC3801001DataSet
Imports Toyota.eCRB.DMSLinkage.RepairOrderSearch.DataAccess.IC3801001.IC3801001TableAdapter

Imports Toyota.eCRB.DMSLinkage.RepairOrderSearch.BizLogic.IC3801002
Imports Toyota.eCRB.DMSLinkage.RepairOrderSearch.DataAccess.IC3801002
Imports Toyota.eCRB.DMSLinkage.RepairOrderSearch.DataAccess.IC3801002.IC3801002DataSet
Imports Toyota.eCRB.DMSLinkage.RepairOrderSearch.DataAccess.IC3801002.IC3801002DataSetTableAdapters.IC3801002DataSetTableAdapter

Imports Toyota.eCRB.DMSLinkage.RepairOrderSearch.BizLogic.IC3801003
Imports Toyota.eCRB.DMSLinkage.RepairOrderSearch.DataAccess.IC3801003
Imports Toyota.eCRB.DMSLinkage.RepairOrderSearch.DataAccess.IC3801003.IC3801003DataSet
Imports Toyota.eCRB.DMSLinkage.RepairOrderSearch.DataAccess.IC3801003.IC3801003TableAdapter

Imports Toyota.eCRB.DMSLinkage.CustomerInfo.BizLogic.IC3800703
Imports Toyota.eCRB.DMSLinkage.CustomerInfo.DataAccess.IC3800703
Imports Toyota.eCRB.DMSLinkage.CustomerInfo.DataAccess.IC3800703.IC3800703DataSet
Imports Toyota.eCRB.DMSLinkage.CustomerInfo.DataAccess.IC3800703.IC3800703DataSetTableAdapters

Imports System.Globalization

''' <summary>
''' SC3140103
''' </summary>
''' <remarks></remarks>
Public Class SC3140103BusinessLogic
    Inherits BaseBusinessComponent

#Region " 定数定義"

    ' 表示区分
    Public Const DisplayDivNone As String = "0"            ' なし
    Public Const DisplayDivReception As String = "1"       ' 受付
    Public Const DisplayDivApproval As String = "2"        ' 承認依頼
    Public Const DisplayDivPreparation As String = "3"     ' 納車準備
    Public Const DisplayDivDelivery As String = "4"        ' 納車作業
    Public Const DisplayDivWork As String = "5"            ' 作業中

    ' 仕掛中
    Public Const DisplayStartNone As String = "0"          ' 仕掛前
    Public Const DisplayStartStart As String = "1"         ' 仕掛中

    ' チップ詳細ボタン
    Public Const ButtonNone As String = "0"              ' なし
    Public Const ButtonCustomer As String = "1"          ' 顧客詳細ボタン
    Public Const ButtonNewCustomer As String = "2"       ' 新規顧客登録ボタン
    Public Const ButtonNewRO As String = "3"             ' R/O作成ボタン
    Public Const ButtonRODisplay As String = "4"         ' R/O参照ボタン
    Public Const ButtonWork As String = "5"              ' 追加作業登録ボタン
    Public Const ButtonApproval As String = "6"          ' 追加承認ボタン
    Public Const ButtonCheckSheet As String = "7"        ' チェックシートボタン
    Public Const ButtonSettlement As String = "8"        ' 清算入力ボタン

    ' ストール予定；洗車フラグ
    Private Const C_STALLREZINFO_WASH_ON As String = "1"    ' あり
    ' ストール予定；来店フラグ
    Private Const C_STALLREZINFO_WALKIN_REZ As String = "0"     ' 予約

    ' ストール実績：実績ステータス
    Private Const C_STALLPROSESS_NONE As String = "00"          ' 未入庫
    Private Const C_STALLPROSESS_CAR_IN As String = "10"        ' 入庫
    Private Const C_STALLPROSESS_WORKING As String = "20"       ' 作業中
    Private Const C_STALLPROSESS_ITEM_NONE As String = "30"     ' 部品欠品
    Private Const C_STALLPROSESS_CUST_WAIT As String = "31"     ' お客様連絡待ち
    Private Const C_STALLPROSESS_STALL_WAIT As String = "38"    ' ストール待機
    Private Const C_STALLPROSESS_ETC As String = "39"           ' その他
    Private Const C_STALLPROSESS_WASH_WAIT As String = "40"     ' 洗車待ち
    Private Const C_STALLPROSESS_WASH_DOING As String = "41"    ' 洗車中
    Private Const C_STALLPROSESS_INSP_WAIT As String = "42"     ' 検査待ち
    Private Const C_STALLPROSESS_INSP_DOING As String = "43"    ' 検査中
    Private Const C_STALLPROSESS_INSP_NG As String = "44"       ' 検査不合格
    Private Const C_STALLPROSESS_CARRY_WAIT As String = "50"    ' 預かり中
    Private Const C_STALLPROSESS_DELI_WAIT As String = "60"     ' 納車待ち
    Private Const C_STALLPROSESS_PFINISH As String = "97"       ' 関連チップの前工程作業終了
    Private Const C_STALLPROSESS_MFINISH As String = "98"       ' MidFinish
    Private Const C_STALLPROSESS_COMPLETE As String = "99"      ' 完了

    ' 追加作業承認
    Private Const C_APPROVAL_STATUS_ON As String = "1"      ' あり   
    ' 追加作業承認印刷
    Private Const C_APPROVAL_OUTPUT_ON As String = "1"      ' あり   

    ' R/O作成画面表示(API)
    Private Const C_RO_CREATE_STATUS_OK As String = "1"     ' 表示
    ' 完成検査有無(API)
    Private Const C_COMP_INS_FLAG_ON As String = "1"        ' あり
    ' チェックシート有無(API)
    Private Const C_CHECKSHEET_FLAG_ON As String = "1"      ' あり
    ' チェックシート印刷(API)
    Private Const C_CHECKSHET_OUTPUT_OK As String = "1"     ' 印刷
    ' 精算書発行(API)
    Private Const C_SETTLEMENT_OUTPUT_OK As String = "1"    ' 印刷
    ' 顧客区分(API)
    Public Const CustomerSegmentON As String = "1"           ' 自社客

    ' R/Oステータス(API)
    Private Const C_RO_STATUS_NONE As String = "0"          ' なし
    Private Const C_RO_STATUS_RECEPTION As String = "1"     ' 受付
    Private Const C_RO_STATUS_WORKING As String = "2"       ' 作業中
    Private Const C_RO_STATUS_ITEM_WAIT As String = "4"     ' 部品待ち
    Private Const C_RO_STATUS_ESTI_WAIT As String = "5"     ' 見積確認待ち
    Private Const C_RO_STATUS_INSP_OK As String = "7"       ' 検査完了
    Private Const C_RO_STATUS_SALE_OK As String = "3"       ' 売上済み
    Private Const C_RO_STATUS_MANT_OK As String = "6"       ' 整備完了
    Private Const C_RO_STATUS_FINISH As String = "8"        ' 納車完了

    ' フラグ無し
    Private Const C_FLAG_OFF = "0"                          ' フラグなし
    Private Const C_FLAG_ON = "1"                           ' フラグあり

    ' 作業区分
    Private Const C_WORK_NONE As String = "0"       ' 作業なし
    Private Const C_WORK_WAIT As String = "1"       ' 作業前
    Private Const C_WORK_START As String = "2"      ' 作業中
    Private Const C_WORK_END As String = "3"        ' 作業完了

    ' 画面ID
    Private Const MAINMENUID As String = "SC3140103"

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
    ''' エラー:SAコードが異なる
    ''' </summary>
    ''' <remarks></remarks>
    Private Const C_RET_DIFFSACODE As Long = 1

    ''' <summary>
    ''' Log開始用文言
    ''' </summary>
    ''' <remarks></remarks>
    Private Const LOG_START As String = "Start"

    ''' <summary>
    ''' Log終了文言
    ''' </summary>
    ''' <remarks></remarks>
    Private Const LOG_END As String = "End"

#End Region

#Region " 変数定義"

    ' 表示区分
    Private mDispDiv As String
    ' 仕掛中
    Private mDispStart As String

    ' チップ詳細ボタン関連
    Private mButtonLeft As String
    Private mButtonRight As String
    Private mButtonEnabledLeft As Boolean
    Private mButtonEnabledRight As Boolean

    ' 納車準備_異常表示標準時間（分）
    Private mlngDeliveryPreAbnormalLT As Long

    ' 現在時刻
    Private mNowDate As Date
#End Region

#Region "コンストラクタ"

    '''-------------------------------------------------------
    ''' <summary>
    ''' デフォルトコンストラクタ
    ''' </summary>
    ''' <remarks></remarks>
    '''-------------------------------------------------------
    Public Sub New()
        Me.mlngDeliveryPreAbnormalLT = 0
        Me.mNowDate = DateTime.MinValue
    End Sub

    '''-------------------------------------------------------
    ''' <summary>
    ''' コンストラクタ
    ''' </summary>
    ''' <param name="lngDeliveryPreAbnormalLt">納車準備_異常表示標準時間（分）</param>
    ''' <param name="nowDate">現在日付</param>
    ''' <remarks></remarks>
    '''-------------------------------------------------------
    Public Sub New(ByVal lngDeliveryPreAbnormalLT As Long, ByVal nowDate As Date)
        Me.mlngDeliveryPreAbnormalLT = lngDeliveryPreAbnormalLT
        Me.mNowDate = nowDate
    End Sub

#End Region

#Region " サービス来店実績取得"

    '''-------------------------------------------------------
    ''' <summary>
    ''' 来店チップ情報取得
    ''' </summary>
    ''' <returns>来店チップデータセット</returns>
    ''' <remarks></remarks>
    '''-------------------------------------------------------
    Public Function GetVisitChip() As SC3140103VisitChipDataTable

        '開始ログ
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                                , "{0}.{1} {2}" _
                                , Me.GetType.ToString _
                                , System.Reflection.MethodBase.GetCurrentMethod.Name _
                                , LOG_START))

        Dim dtChip As SC3140103VisitChipDataTable = New SC3140103VisitChipDataTable
        Dim staffInfo As StaffContext = StaffContext.Current

        Dim dt As SC3140103VisitDataTable
        '外部IF
        Dim dtIFGetNoDeliveryROList As IC3801003DataSet.IC3801003NoDeliveryRODataTable
        Dim dtIFConfirmAddList As IC3801002DataSet.ConfirmAddListDataTable

        Dim dtService As SC3140103ServiceVisitManagementDataTable
        Dim dtRezinfo As SC3140103StallRezinfoDataTable
        Dim dtProcess As SC3140103StallProcessDataTable

        Using da As New SC3140103DataTableAdapter
            'IF検索処理
            ' SA別未納者R/O一覧
            dtIFGetNoDeliveryROList = Me.GetIFNoDeliveryROList(staffInfo)
            ' 追加承認待ち情報取得
            dtIFConfirmAddList = Me.GetIFApprovalConfirmAddList(staffInfo)

            '検索処理
            dtService = da.GetVisitManagement(staffInfo.DlrCD, staffInfo.BrnCD, staffInfo.Account, dtIFGetNoDeliveryROList, Me.mNowDate)
            dtRezinfo = da.GetStallReserveInformation(staffInfo.DlrCD, staffInfo.BrnCD, dtService)
            dtProcess = da.GetStallProcess(staffInfo.DlrCD, staffInfo.BrnCD, dtService)
        End Using

        ' サービス来店実績・ストール予約実績取得
        dt = Me.SetVisit(dtService, dtRezinfo, dtProcess)
        ' IFマージ処理
        dt = Me.SetVisitMargin(dt, dtIFGetNoDeliveryROList)

        Dim dtmRezDeliDate As DateTime
        Dim rowChip As SC3140103VisitChipRow
        Dim rowChipApproval As SC3140103VisitChipRow

        ' チップ情報チェック
        For Each row As SC3140103VisitRow In dt.Rows
            rowChip = DirectCast(dtChip.NewRow(), SC3140103VisitChipRow)

            ' チップ状態チェック
            Me.CheckChipStatus(row)
            If Me.mDispDiv.Equals(DisplayDivNone) Then
                ' 削除
                Continue For
            End If

            ' 納車予定日時日付変換
            If Not String.IsNullOrEmpty(row.REZ_DELI_DATE) Then
                dtmRezDeliDate = DateTimeFunc.FormatString("yyyyMMddHHmm", row.REZ_DELI_DATE)
            Else
                ' 納車予定日日時がない場合
                If Not Me.IsDateTimeNull(row.ENDTIME) Then
                    ' 作業終了予定時刻＋納車準備_異常表示標準時間（分）
                    dtmRezDeliDate = row.ENDTIME.AddMinutes(Me.mlngDeliveryPreAbnormalLT)
                Else
                    dtmRezDeliDate = DateTime.MinValue
                End If
            End If

            ' チップ情報形成
            rowChip = Me.GetRowChip(row, rowChip, dtmRezDeliDate)

            ' 行追加
            dtChip.AddSC3140103VisitChipRow(rowChip)

            ' 追加承認チェック
            For Each rowIFApproval As IC3801002DataSet.ConfirmAddListRow In dtIFConfirmAddList.Select(String.Format(CultureInfo.CurrentCulture, "ORDERNO = '{0}'", row.ORDERNO))
                rowChipApproval = DirectCast(dtChip.NewRow(), SC3140103VisitChipRow)

                '追加承認待ち情報形成
                rowChipApproval = Me.GetRowChipApporoval(row, dtmRezDeliDate, rowChip, rowChipApproval, rowIFApproval)

                ' 行追加
                dtChip.AddSC3140103VisitChipRow(rowChipApproval)
            Next
        Next

        '終了ログ
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                                , "{0}.{1} {2}" _
                                , Me.GetType.ToString _
                                , System.Reflection.MethodBase.GetCurrentMethod.Name _
                                , LOG_END))

        '処理結果返却
        Return dtChip
    End Function

    '''-------------------------------------------------------
    ''' <summary>
    ''' 来店チップ詳細情報取得
    ''' </summary>
    ''' <param name="visitSeq">来店実績連番</param>
    ''' <param name="displayDiv">表示区分</param>
    ''' <returns>来店チップ詳細データセット</returns>
    ''' <remarks></remarks>
    '''-------------------------------------------------------
    Public Function GetVisitChipDetail(ByVal visitSeq As Long, ByVal displayDiv As String) As SC3140103VisitChipDetailDataTable

        '開始ログ
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                                , "{0}.{1} {2} IN:visitSeq = {3}, displayDiv = {4}" _
                                , Me.GetType.ToString _
                                , System.Reflection.MethodBase.GetCurrentMethod.Name _
                                , LOG_START _
                                , visitSeq _
                                , displayDiv))

        Dim dtChip As SC3140103VisitChipDetailDataTable = New SC3140103VisitChipDetailDataTable
        Dim staffInfo As StaffContext = StaffContext.Current

        Dim dt As SC3140103VisitDataTable
        '外部IF
        Dim dtIFOrderCommon As IC3801001OrderCommDataTable
        ' 顧客参照
        Dim dtIFSrvCustomerDataTable As IC3800703SrvCustomerDataTable

        Dim dtService As SC3140103ServiceVisitManagementDataTable
        Dim dtRezinfo As SC3140103StallRezinfoDataTable
        Dim dtProcess As SC3140103StallProcessDataTable

        Using da As New SC3140103DataTableAdapter
            '検索処理
            dtService = da.GetVisitManagement(visitSeq)
            dtRezinfo = da.GetStallReserveInformation(staffInfo.DlrCD, staffInfo.BrnCD, dtService)
            dtProcess = da.GetStallProcessDetail(staffInfo.DlrCD, staffInfo.BrnCD, dtService)
        End Using

        ' サービス来店実績・ストール予約実績取得
        dt = Me.SetVisit(dtService, dtRezinfo, dtProcess)

        ' チップ情報詳細チェック
        If dt IsNot Nothing AndAlso dt.Rows.Count > 0 Then

            Dim row As SC3140103VisitRow = DirectCast(dt.Rows(0), SC3140103VisitRow)
            Dim rowChip As SC3140103VisitChipDetailRow = DirectCast(dtChip.NewRow(), SC3140103VisitChipDetailRow)

            ' IF処理確認
            If Not String.IsNullOrEmpty(row.ORDERNO) Then
                ' IF-R/O基本情報参照処理
                dtIFOrderCommon = Me.GetIFROBaseInformationList(staffInfo, row.ORDERNO)
                ' IFマージ処理
                row = Me.SetVisitDetailOrderMargin(row, dtIFOrderCommon)
            ElseIf String.IsNullOrEmpty(row.ORDERNO) And Not String.IsNullOrEmpty(row.VIN) Then
                ' IF-顧客参照処理
                dtIFSrvCustomerDataTable = Me.GetIFCustomerInformation(row)
                ' IFマージ処理
                row = Me.SetVisitDetailCustomerMargin(row, dtIFSrvCustomerDataTable)
            End If

            ' 来店実績連番
            rowChip.VISITSEQ = row.VISITSEQ
            ' 販売店コード
            rowChip.DLRCD = row.DLRCD
            ' 店舗コード
            rowChip.STRCD = row.STRCD
            ' 予約ID
            rowChip.FREZID = row.FREZID
            ' 表示区分
            rowChip.DISP_DIV = displayDiv
            ' VIPマーク
            rowChip.VIP_MARK = row.VIP_MARK
            ' 予約マーク
            rowChip.REZ_MARK = row.REZ_MARK
            ' JDP調査対象客マーク
            rowChip.JDP_MARK = row.JDP_MARK
            ' 技術情報マーク
            rowChip.SSC_MARK = row.SSC_MARK
            ' 登録番号
            rowChip.VCLREGNO = row.VCLREGNO
            ' 車種
            rowChip.VEHICLENAME = row.VEHICLENAME
            ' モデル
            rowChip.MODELCODE = row.MODELCODE
            ' VIN
            rowChip.VIN = row.VIN
            ' 走行距離
            rowChip.MILEAGE = row.MILEAGE
            ' 顧客名
            rowChip.CUSTOMERNAME = row.CUSTOMERNAME
            ' 電話番号
            rowChip.TELNO = row.TELNO
            ' 携帯番号
            rowChip.MOBILE = row.MOBILE
            ' 代表入庫項目
            rowChip.MERCHANDISENAME = row.MERCHANDISENAME
            ' 来店時刻
            rowChip.VISITTIMESTAMP = row.VISITTIMESTAMP
            ' 作業開始
            rowChip.ACTUAL_STIME = row.ACTUAL_STIME
            ' 作業終了予定時刻
            rowChip.ENDTIME = row.ENDTIME

            ' 納車予定日時日付変換
            Dim dtmRezDeliDate As DateTime
            If Not String.IsNullOrEmpty(row.REZ_DELI_DATE) Then
                dtmRezDeliDate = DateTimeFunc.FormatString("yyyyMMddHHmm", row.REZ_DELI_DATE)
            Else
                ' 納車予定日日時がない場合
                If Not Me.IsDateTimeNull(row.ENDTIME) Then
                    ' 作業終了予定時刻＋納車準備_異常表示標準時間（分）
                    dtmRezDeliDate = row.ENDTIME.AddMinutes(Me.mlngDeliveryPreAbnormalLT)
                Else
                    dtmRezDeliDate = DateTime.MinValue
                End If
            End If
            ' 納車予定日時
            rowChip.REZ_DELI_DATE = dtmRezDeliDate

            ' チップ詳細ボタンチェック
            Me.CheckChipDetailButton(row, displayDiv)

            ' 左ボタン
            rowChip.BUTTON_LEFT = Me.mButtonLeft
            rowChip.BUTTON_ENABLED_LEFT = Me.mButtonEnabledLeft
            ' 右ボタン
            rowChip.BUTTON_RIGHT = Me.mButtonRight
            rowChip.BUTTON_ENABLED_RIGHT = Me.mButtonEnabledRight

            ' 行追加
            dtChip.AddSC3140103VisitChipDetailRow(rowChip)

        End If

        '終了ログ
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                                , "{0}.{1} {2}" _
                                , Me.GetType.ToString _
                                , System.Reflection.MethodBase.GetCurrentMethod.Name _
                                , LOG_END))

        '処理結果返却
        Return dtChip
    End Function

    '''-------------------------------------------------------
    ''' <summary>
    ''' サービス来店実績マージ(チップ情報)
    ''' </summary>
    ''' <param name="dt">サービス来店情報</param>
    ''' <param name="dtIFNoDeliveryRO">SA別未納者R/O一覧</param>
    ''' <returns>来店チップデータセット</returns>
    ''' <remarks></remarks>
    '''-------------------------------------------------------
    Private Function SetVisitMargin(ByVal dt As SC3140103VisitDataTable, ByVal dtIFNoDeliveryRO As IC3801003NoDeliveryRODataTable) As SC3140103VisitDataTable

        '開始ログ
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                                , "{0}.{1} {2}" _
                                , Me.GetType.ToString _
                                , System.Reflection.MethodBase.GetCurrentMethod.Name _
                                , LOG_START))

        Dim rowNoDeliveryRO As IC3801003NoDeliveryRORow
        Dim aryRow As DataRow()

        For Each row As SC3140103VisitRow In dt.Rows

            aryRow = dtIFNoDeliveryRO.Select(String.Format(CultureInfo.CurrentCulture, "ORDERNO = '{0}'", row.ORDERNO))

            If aryRow IsNot Nothing AndAlso aryRow.Length > 0 Then
                rowNoDeliveryRO = DirectCast(aryRow(0), IC3801003NoDeliveryRORow)

                If Not IsDBNull(rowNoDeliveryRO.Item("ORDERNO")) Then
                    row.ORDERNO = Me.SetReplaceString(row.ORDERNO, rowNoDeliveryRO.ORDERNO)                                'R/O No
                End If
                If Not IsDBNull(rowNoDeliveryRO.Item("ORDERSTATUS")) Then
                    row.RO_STATUS = Me.SetReplaceString(String.Empty, rowNoDeliveryRO.ORDERSTATUS)                         'R/Oステータス
                End If
                If Not IsDBNull(rowNoDeliveryRO.Item("IFLAG")) Then
                    row.JDP_MARK = Me.SetReplaceString(String.Empty, rowNoDeliveryRO.IFLAG)                                'JDP調査対象客フラグ
                End If
                If Not IsDBNull(rowNoDeliveryRO.Item("SFLAG")) Then
                    row.SSC_MARK = Me.SetReplaceString(String.Empty, rowNoDeliveryRO.SFLAG)                                'SSCフラグ
                End If
                If Not IsDBNull(rowNoDeliveryRO.Item("CUSTOMERNAME")) Then
                    row.CUSTOMERNAME = Me.SetReplaceString(row.CUSTOMERNAME, rowNoDeliveryRO.CUSTOMERNAME)                 '顧客名
                End If
                If Not IsDBNull(rowNoDeliveryRO.Item("REGISTERNO")) Then
                    row.VCLREGNO = Me.SetReplaceString(row.VCLREGNO, rowNoDeliveryRO.REGISTERNO)                           '車両登録No.
                End If
                If Not IsDBNull(rowNoDeliveryRO.Item("ADDSRVCOUNT")) Then
                    ' データ先空白チェック
                    If Not String.IsNullOrEmpty(rowNoDeliveryRO.ADDSRVCOUNT) Then
                        ' データ先あり
                        row.APPROVAL_COUNT = CType(rowNoDeliveryRO.ADDSRVCOUNT, Long)                                       '追加作業数
                    End If
                End If
            End If

        Next

        '終了ログ
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                                , "{0}.{1} {2}" _
                                , Me.GetType.ToString _
                                , System.Reflection.MethodBase.GetCurrentMethod.Name _
                                , LOG_END))

        '処理結果返却
        Return dt
    End Function

    '''-------------------------------------------------------
    ''' <summary>
    ''' サービス来店実績-R/O基本情報マージ(チップ詳細)
    ''' </summary>
    ''' <param name="row">サービス来店情報</param>
    ''' <param name="dtIFOrderCommon">R/O基本情報参照</param>
    ''' <returns>来店チップレコード</returns>
    ''' <remarks></remarks>
    '''-------------------------------------------------------
    Private Function SetVisitDetailOrderMargin(ByVal row As SC3140103VisitRow, ByVal dtIFOrderCommon As IC3801001OrderCommDataTable) As SC3140103VisitRow

        '開始ログ
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                                , "{0}.{1} {2}" _
                                , Me.GetType.ToString _
                                , System.Reflection.MethodBase.GetCurrentMethod.Name _
                                , LOG_START))

        Dim rowIFOrderCommon As IC3801001OrderCommRow

        If dtIFOrderCommon IsNot Nothing AndAlso dtIFOrderCommon.Rows.Count > 0 Then
            rowIFOrderCommon = DirectCast(dtIFOrderCommon.Rows(0), IC3801001OrderCommRow)

            If Not IsDBNull(rowIFOrderCommon.Item("OrderIFlag")) Then
                row.JDP_MARK = Me.SetReplaceString(String.Empty, rowIFOrderCommon.OrderIFlag)                           'JDP調査対象客フラグ
            End If
            If Not IsDBNull(rowIFOrderCommon.Item("OrderSFlag")) Then
                row.SSC_MARK = Me.SetReplaceString(String.Empty, rowIFOrderCommon.OrderSFlag)                           'SSCフラグ
            End If
            If Not IsDBNull(rowIFOrderCommon.Item("OrderRegisterNo")) Then
                row.VCLREGNO = Me.SetReplaceString(row.VCLREGNO, rowIFOrderCommon.OrderRegisterNo)                      '車両登録No.
            End If
            If Not IsDBNull(rowIFOrderCommon.Item("OrderVhcName")) Then
                row.VEHICLENAME = Me.SetReplaceString(row.VEHICLENAME, rowIFOrderCommon.OrderVhcName)                   '車種名称
            End If
            If Not IsDBNull(rowIFOrderCommon.Item("OrderGrade")) Then
                row.MODELCODE = Me.SetReplaceString(row.MODELCODE, rowIFOrderCommon.OrderGrade)                         'MODEL
            End If
            If Not IsDBNull(rowIFOrderCommon.Item("OrderVinNo")) Then
                row.VIN = Me.SetReplaceString(row.VIN, rowIFOrderCommon.OrderVinNo)                                     'VINNO
            End If
            If Not IsDBNull(rowIFOrderCommon.Item("OrderMileAge")) Then
                row.MILEAGE = Me.SetReplaceLong(CType(row.MILEAGE, Long), CType(rowIFOrderCommon.OrderMileAge, Long))   '走行距離
            End If
            If Not IsDBNull(rowIFOrderCommon.Item("TwcTel1")) Then
                row.TELNO = Me.SetReplaceString(row.TELNO, rowIFOrderCommon.TwcTel1)                                    'サービスフォロー顧客電話1
            End If
            If Not IsDBNull(rowIFOrderCommon.Item("TwcTel2")) Then
                row.MOBILE = Me.SetReplaceString(row.MOBILE, rowIFOrderCommon.TwcTel2)                                  'サービスフォロー顧客電話2
            End If
            If Not IsDBNull(rowIFOrderCommon.Item("printFlag")) Then
                row.CHECKSHEET_FLAG = Me.SetReplaceString(String.Empty, rowIFOrderCommon.printFlag)                     'チェックシート印刷有無
            End If

        End If

        '終了ログ
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                                , "{0}.{1} {2}" _
                                , Me.GetType.ToString _
                                , System.Reflection.MethodBase.GetCurrentMethod.Name _
                                , LOG_END))

        '処理結果返却
        Return row
    End Function

    '''-------------------------------------------------------
    ''' <summary>
    ''' サービス来店実績-顧客参照情報マージ(チップ詳細)
    ''' </summary>
    ''' <param name="row">サービス来店情報</param>
    ''' <param name="dtIFSrvCustomer">顧客参照情報</param>
    ''' <returns>来店チップデータセット</returns>
    ''' <remarks></remarks>
    '''-------------------------------------------------------
    Private Function SetVisitDetailCustomerMargin(ByVal row As SC3140103VisitRow, ByVal dtIFSrvCustomer As IC3800703SrvCustomerDataTable) As SC3140103VisitRow

        '開始ログ
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                                , "{0}.{1} {2}" _
                                , Me.GetType.ToString _
                                , System.Reflection.MethodBase.GetCurrentMethod.Name _
                                , LOG_START))

        Dim rowIFSrvCustomer As IC3800703SrvCustomerFRow

        If dtIFSrvCustomer IsNot Nothing AndAlso dtIFSrvCustomer.Rows.Count > 0 Then
            rowIFSrvCustomer = DirectCast(dtIFSrvCustomer.Rows(0), IC3800703SrvCustomerFRow)

            If Not IsDBNull(rowIFSrvCustomer.Item("REGISTERNO")) Then
                row.VCLREGNO = Me.SetReplaceString(row.VCLREGNO, rowIFSrvCustomer.REGISTERNO)           '登録NO.
            End If
            If Not IsDBNull(rowIFSrvCustomer.Item("MODEL")) Then
                row.VEHICLENAME = Me.SetReplaceString(row.VEHICLENAME, rowIFSrvCustomer.MODEL)          '型名
            End If
            If Not IsDBNull(rowIFSrvCustomer.Item("GRADE")) Then
                row.MODELCODE = Me.SetReplaceString(row.MODELCODE, rowIFSrvCustomer.GRADE)              'モデル
            End If
            If Not IsDBNull(rowIFSrvCustomer.Item("VINNO")) Then
                row.VIN = Me.SetReplaceString(row.VIN, rowIFSrvCustomer.VINNO)                          'VINNO
            End If
            If Not IsDBNull(rowIFSrvCustomer.Item("MILEAGE")) Then
                row.MILEAGE = Me.SetReplaceLong(CType(row.MILEAGE, Long), rowIFSrvCustomer.MILEAGE)     '走行距離
            End If
            If Not IsDBNull(rowIFSrvCustomer.Item("BUYERTEL1")) Then
                row.TELNO = Me.SetReplaceString(row.TELNO, rowIFSrvCustomer.BUYERTEL1)                  'サービスフォロー顧客電話1
            End If
            If Not IsDBNull(rowIFSrvCustomer.Item("BUYERTEL2")) Then
                row.MOBILE = Me.SetReplaceString(row.MOBILE, rowIFSrvCustomer.BUYERTEL2)                'サービスフォロー顧客電話2
            End If
            If Not IsDBNull(rowIFSrvCustomer.Item("BUYERNAME")) Then
                row.CUSTOMERNAME = Me.SetReplaceString(row.CUSTOMERNAME, rowIFSrvCustomer.BUYERNAME)    '顧客名
            End If

        End If

        '終了ログ
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                                , "{0}.{1} {2}" _
                                , Me.GetType.ToString _
                                , System.Reflection.MethodBase.GetCurrentMethod.Name _
                                , LOG_END))

        '処理結果返却
        Return row
    End Function

    '''-------------------------------------------------------
    ''' <summary>
    ''' サービス来店管理情報取得
    ''' </summary>
    ''' <param name="visitSeq">来店実績連番</param>
    ''' <returns>サービス来店実績データセット</returns>
    ''' <remarks></remarks>
    '''-------------------------------------------------------
    Public Function GetVisitManager(ByVal visitSeq As Long) As SC3140103ServiceVisitManagementDataTable

        '開始ログ
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                                , "{0}.{1} {2} IN:visitSeq = {3}" _
                                , Me.GetType.ToString _
                                , System.Reflection.MethodBase.GetCurrentMethod.Name _
                                , LOG_START _
                                , visitSeq))

        Dim dt As SC3140103ServiceVisitManagementDataTable

        Using da As New SC3140103DataTableAdapter
            '検索処理
            dt = da.GetVisitManagement(visitSeq)
        End Using

        '終了ログ
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                                , "{0}.{1} {2}" _
                                , Me.GetType.ToString _
                                , System.Reflection.MethodBase.GetCurrentMethod.Name _
                                , LOG_END))

        '処理結果返却
        Return dt
    End Function

    '''-------------------------------------------------------
    ''' <summary>
    ''' サービス来店実績情報取得
    ''' </summary>
    ''' <param name="visitSeq">来店実績連番</param>
    ''' <returns>来店実績データセット</returns>
    ''' <remarks></remarks>
    '''-------------------------------------------------------
    Public Function GetVisitChipDetail(ByVal visitSeq As Long) As SC3140103VisitDataTable

        '開始ログ
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                                , "{0}.{1} {2} IN:visitSeq = {3}" _
                                , Me.GetType.ToString _
                                , System.Reflection.MethodBase.GetCurrentMethod.Name _
                                , LOG_START _
                                , visitSeq))

        Dim dt As SC3140103VisitDataTable = New SC3140103VisitDataTable
        Dim staffInfo As StaffContext = StaffContext.Current

        Dim dtService As SC3140103ServiceVisitManagementDataTable
        Dim dtRezinfo As SC3140103StallRezinfoDataTable
        Dim dtProcess As SC3140103StallProcessDataTable

        Using da As New SC3140103DataTableAdapter
            '検索処理
            dtService = da.GetVisitManagement(visitSeq)
            dtRezinfo = da.GetStallReserveInformation(staffInfo.DlrCD, staffInfo.BrnCD, dtService)
            dtProcess = da.GetStallProcessDetail(staffInfo.DlrCD, staffInfo.BrnCD, dtService)
        End Using

        ' サービス来店実績・ストール予約実績取得
        dt = Me.SetVisit(dtService, dtRezinfo, dtProcess)

        '終了ログ
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                                , "{0}.{1} {2}" _
                                , Me.GetType.ToString _
                                , System.Reflection.MethodBase.GetCurrentMethod.Name _
                                , LOG_END))

        '処理結果返却
        Return dt
    End Function

#End Region

#Region " 来店チップ情報取得"

    '''-------------------------------------------------------
    ''' <summary>
    ''' チップ情報形成
    ''' </summary>
    ''' <param name="row">来店チップレコード</param>
    ''' <param name="rowChip">チップ情報設定レコード</param>
    ''' <param name="dtmRezDeliDate">納車予定日時</param>
    ''' <returns>チップ情報設定レコード</returns>
    ''' <remarks></remarks>
    '''-------------------------------------------------------
    Private Function GetRowChip(ByVal row As SC3140103VisitRow _
                             , ByVal rowChip As SC3140103VisitChipRow _
                             , ByVal dtmRezDeliDate As DateTime) As SC3140103VisitChipRow

        '開始ログ
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                                , "{0}.{1} {2} IN: dtmRezDeliDate= {3}" _
                                , Me.GetType.ToString _
                                , System.Reflection.MethodBase.GetCurrentMethod.Name _
                                , LOG_START _
                                , dtmRezDeliDate))

        ' 来店実績連番
        rowChip.VISITSEQ = row.VISITSEQ
        ' 販売店コード
        rowChip.DLRCD = row.DLRCD
        ' 店舗コード
        rowChip.STRCD = row.STRCD
        ' 予約ID
        rowChip.FREZID = row.FREZID
        ' 表示区分
        rowChip.DISP_DIV = Me.mDispDiv
        ' 仕掛中
        rowChip.DISP_START = Me.mDispStart
        ' VIPマーク
        rowChip.VIP_MARK = row.VIP_MARK
        ' 予約マーク
        rowChip.REZ_MARK = row.REZ_MARK
        ' JDP調査対象客マーク
        rowChip.JDP_MARK = row.JDP_MARK
        ' 技術情報マーク
        rowChip.SSC_MARK = row.SSC_MARK
        ' 登録番号
        rowChip.VCLREGNO = row.VCLREGNO
        ' 顧客名
        rowChip.CUSTOMERNAME = row.CUSTOMERNAME
        ' 代表入庫項目
        rowChip.MERCHANDISENAME = row.MERCHANDISENAME
        ' 駐車場コード
        rowChip.PARKINGCODE = row.PARKINGCODE
        ' 担当テクニシャン名
        rowChip.STAFFNAME = row.STAFFNAME
        ' 追加作業承認数
        rowChip.APPROVAL_COUNT = row.APPROVAL_COUNT
        ' 整備受注NO
        rowChip.ORDERNO = row.ORDERNO
        ' 追加承認待ちID
        rowChip.APPROVAL_ID = String.Empty

        ' チップ情報チェック
        Select Case Me.mDispDiv
            Case DisplayDivReception       ' 受付
                ' 表示順
                rowChip.DISP_SORT = row.ASSIGNTIMESTAMP.ToString("yyyyMMddHHmmss", CultureInfo.CurrentCulture)  ' SA割振り日時
                ' 表示日時
                rowChip.ITEM_DATE = row.VISITTIMESTAMP      ' 来店時刻
                ' 残計算日時
                rowChip.PROC_DATE = row.ASSIGNTIMESTAMP     ' SA割振り日時

            Case DisplayDivPreparation     ' 納車準備
                ' 表示順
                rowChip.DISP_SORT = String.Format(CultureInfo.CurrentCulture, "{0}{1}",
                                    dtmRezDeliDate.ToString("yyyyMMddHHmmss", CultureInfo.CurrentCulture),
                                    row.ASSIGNTIMESTAMP.ToString("yyyyMMddHHmmss", CultureInfo.CurrentCulture)) ' 納車予定日時＋ SA割振り日時
                ' 表示日時
                rowChip.ITEM_DATE = dtmRezDeliDate          ' 納車予定日時
                ' 残計算日時
                rowChip.PROC_DATE = dtmRezDeliDate          ' 納車予定日時

            Case DisplayDivDelivery        ' 納車作業
                ' 表示順
                rowChip.DISP_SORT = String.Format(CultureInfo.CurrentCulture, "{0}{1}",
                                    dtmRezDeliDate.ToString("yyyyMMddHHmmss", CultureInfo.CurrentCulture),
                                    row.ASSIGNTIMESTAMP.ToString("yyyyMMddHHmmss", CultureInfo.CurrentCulture)) ' 納車予定日時＋ SA割振り日時
                ' 表示日時
                rowChip.ITEM_DATE = dtmRezDeliDate          ' 納車予定日時
                ' 残計算日時
                rowChip.PROC_DATE = dtmRezDeliDate          ' 納車予定日時

            Case DisplayDivWork            ' 作業中
                ' 表示順
                rowChip.DISP_SORT = String.Format(CultureInfo.CurrentCulture, "{0}{1}",
                                    row.ENDTIME.ToString("yyyyMMddHHmmss", CultureInfo.CurrentCulture),
                                    row.ASSIGNTIMESTAMP.ToString("yyyyMMddHHmmss", CultureInfo.CurrentCulture)) ' 作業終了予定時刻＋SA割振り日時
                ' 表示日時
                rowChip.ITEM_DATE = dtmRezDeliDate          ' 納車予定日時
                ' 残計算日時
                rowChip.PROC_DATE = row.ENDTIME             ' 作業終了予定時刻

            Case Else
                ' 表示順
                rowChip.DISP_SORT = String.Empty
                ' 表示日時
                rowChip.ITEM_DATE = DateTime.MinValue
                ' 残計算日時
                rowChip.PROC_DATE = DateTime.MinValue

        End Select

        '終了ログ
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                                , "{0}.{1} {2}" _
                                , Me.GetType.ToString _
                                , System.Reflection.MethodBase.GetCurrentMethod.Name _
                                , LOG_END))

        Return rowChip
    End Function

    '''-------------------------------------------------------
    ''' <summary>
    ''' 追加承認待ち情報形成
    ''' </summary>
    ''' <param name="row">来店チップレコード</param>
    ''' <param name="dtmRezDeliDate">納車予定日時</param>
    ''' <param name="rowChip">チップ情報設定レコード</param>
    ''' <param name="rowChipApproval">追加承認待ちチップ情報設定レコード</param>
    ''' <param name="rowIFApproval">追加承認待ちレコード</param>
    ''' <returns>追加承認待ちチップ情報設定レコード</returns>
    ''' <remarks></remarks>
    '''-------------------------------------------------------
    Private Function GetRowChipApporoval(ByVal row As SC3140103VisitRow _
                                           , ByVal dtmRezDeliDate As DateTime _
                                           , ByVal rowChip As SC3140103VisitChipRow _
                                           , ByVal rowChipApproval As SC3140103VisitChipRow _
                                           , ByVal rowIFApproval As IC3801002DataSet.ConfirmAddListRow) As SC3140103VisitChipRow

        '開始ログ
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                                , "{0}.{1} {2} IN: dtmRezDeliDate= {3}" _
                                , Me.GetType.ToString _
                                , System.Reflection.MethodBase.GetCurrentMethod.Name _
                                , LOG_START _
                                , dtmRezDeliDate))

        ' 来店実績連番
        rowChipApproval.VISITSEQ = rowChip.VISITSEQ
        ' 販売店コード
        rowChipApproval.DLRCD = rowChip.DLRCD
        ' 店舗コード
        rowChipApproval.STRCD = rowChip.STRCD
        ' 予約ID
        rowChipApproval.FREZID = rowChip.FREZID
        ' 表示区分
        rowChipApproval.DISP_DIV = DisplayDivApproval
        ' 仕掛中
        ' IFマージ処理
        If Not IsDBNull(rowIFApproval.Item("PRINTFLAG")) Then
            row.APPROVAL_OUTPUT = rowIFApproval.PRINTFLAG
        End If
        ' 追加作業承認印刷チェック
        If row.APPROVAL_OUTPUT.Equals(C_APPROVAL_OUTPUT_ON) Then
            ' 追加作業承認印刷完了
            rowChipApproval.DISP_START = DisplayStartStart  ' 仕掛中
        Else
            ' 上記以外
            rowChipApproval.DISP_START = DisplayStartNone   ' 仕掛前
        End If
        ' VIPマーク
        rowChipApproval.VIP_MARK = rowChip.VIP_MARK
        ' 予約マーク
        rowChipApproval.REZ_MARK = rowChip.REZ_MARK
        ' JDP調査対象客マーク
        rowChipApproval.JDP_MARK = rowChip.JDP_MARK
        ' 技術情報マーク
        rowChipApproval.SSC_MARK = rowChip.SSC_MARK
        ' 登録番号
        rowChipApproval.VCLREGNO = rowChip.VCLREGNO
        ' 顧客名
        rowChipApproval.CUSTOMERNAME = rowChip.CUSTOMERNAME
        ' 代表入庫項目
        rowChipApproval.MERCHANDISENAME = rowChip.MERCHANDISENAME
        ' 駐車場コード
        rowChipApproval.PARKINGCODE = rowChip.PARKINGCODE
        ' 担当テクニシャン名
        rowChipApproval.STAFFNAME = rowChip.STAFFNAME
        ' 追加作業承認数
        rowChipApproval.APPROVAL_COUNT = rowChip.APPROVAL_COUNT
        ' 整備受注NO
        rowChipApproval.ORDERNO = rowChip.ORDERNO
        ' 追加承認待ちID
        If Not IsDBNull(rowIFApproval.Item("SRVADDSEQ")) Then
            rowChipApproval.APPROVAL_ID = rowIFApproval.SRVADDSEQ
        End If

        ' 表示日時
        rowChipApproval.ITEM_DATE = dtmRezDeliDate                  ' 納車予定日時
        ' 残計算日時
        rowChipApproval.PROC_DATE = DateTime.MinValue
        If Not IsDBNull(rowIFApproval.Item("SACONFIRMRELYDATE")) Then
            If Not String.IsNullOrEmpty(rowIFApproval.SACONFIRMRELYDATE) Then
                rowChipApproval.PROC_DATE = CType(rowIFApproval.SACONFIRMRELYDATE.ToString(CultureInfo.CurrentCulture), DateTime)   ' SA承認待ち時刻
            End If
        End If

        ' 2012/02/23 KN 森下【SERVICE_1】START
        ' 表示順
        rowChipApproval.DISP_SORT = String.Format(CultureInfo.CurrentCulture, "{0}{1}",
                                    rowChipApproval.PROC_DATE.ToString("yyyyMMddHHmmss", CultureInfo.CurrentCulture),
                                    row.ASSIGNTIMESTAMP.ToString("yyyyMMddHHmmss", CultureInfo.CurrentCulture))                     ' 追加作業承認依頼時刻＋SA割振り日時
        ' 2012/02/23 KN 森下【SERVICE_1】END

        '終了ログ
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                                , "{0}.{1} {2}" _
                                , Me.GetType.ToString _
                                , System.Reflection.MethodBase.GetCurrentMethod.Name _
                                , LOG_END))

        Return rowChipApproval
    End Function

#End Region

#Region " チップ状態チェック"

    '''-------------------------------------------------------
    ''' <summary>
    ''' チップ状態チェック
    ''' </summary>
    ''' <param name="row">来店チップレコード</param>
    ''' <remarks></remarks>
    '''-------------------------------------------------------
    Private Sub CheckChipStatus(ByVal row As SC3140103VisitRow)

        '開始ログ
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                                , "{0}.{1} {2} IN:ORDERNO = {3}, RO_STATUS = {4}" _
                                , Me.GetType.ToString _
                                , System.Reflection.MethodBase.GetCurrentMethod.Name _
                                , LOG_START _
                                , row.ORDERNO _
                                , row.RO_STATUS))
        Try
            ' 1 : 受付チェック

            ' 整備受注NOチェック
            If String.IsNullOrEmpty(row.ORDERNO) Then
                ' 整備受注NOなし
                Me.mDispDiv = DisplayDivReception  ' 受付
                Me.mDispStart = DisplayStartNone   ' 仕掛前
                Return
            End If

            ' R/Oステータスチェック
            If row.RO_STATUS.Equals(C_RO_STATUS_RECEPTION) Then
                ' R/Oステータス：受付
                Me.mDispDiv = DisplayDivReception  ' 受付
                Me.mDispStart = DisplayStartStart  ' 仕掛中
                Return

            ElseIf row.RO_STATUS.Equals(C_RO_STATUS_ESTI_WAIT) Then
                ' R/Oステータス：見積確認待ち
                Me.mDispDiv = DisplayDivReception  ' 受付
                Me.mDispStart = DisplayStartStart  ' 仕掛中
                Return
            End If

            ' 2 : 作業中チェック

            ' R/Oステータスチェック
            If row.RO_STATUS.Equals(C_RO_STATUS_WORKING) Or
               row.RO_STATUS.Equals(C_RO_STATUS_ITEM_WAIT) Or
               row.RO_STATUS.Equals(C_RO_STATUS_INSP_OK) Then
                ' R/Oステータス：作業中、部品待ち、検査完了

                ' ストール予約取得チェック
                If row.REZINFO_FREZID > SC3140103DataTableAdapter.MinReserveId Then
                    ' ストール予約あり

                    ' 作業開始時間チェック
                    ' 作業終了時間チェック
                    If Not IsDateTimeNull(row.ACTUAL_STIME) And Not IsDateTimeNull(row.ACTUAL_ETIME) Then
                        ' 作業開始時間あり
                        ' 作業終了時間あり

                    ElseIf IsDateTimeNull(row.ACTUAL_STIME) And IsDateTimeNull(row.ACTUAL_ETIME) Then
                        ' 作業開始時間なし
                        ' 作業終了時間なし
                        Me.mDispDiv = DisplayDivWork       ' 作業中
                        Me.mDispStart = DisplayStartNone   ' 仕掛前
                        Return

                    ElseIf Not IsDateTimeNull(row.ACTUAL_STIME) And IsDateTimeNull(row.ACTUAL_ETIME) Then
                        ' 作業開始時間あり
                        ' 作業終了時間なし
                        Me.mDispDiv = DisplayDivWork       ' 作業中
                        Me.mDispStart = DisplayStartStart  ' 仕掛中
                        Return
                    End If
                End If
            End If

            ' 3 : 納車準備チェック

            ' R/Oステータスチェック
            If row.RO_STATUS.Equals(C_RO_STATUS_INSP_OK) Then
                ' R/Oステータス：検査完了

                ' ストール予約・実績取得チェック
                If row.REZINFO_FREZID > SC3140103DataTableAdapter.MinReserveId And
                    row.PROCESS_FREZID > SC3140103DataTableAdapter.MinReserveId Then
                    ' ストール予約・実績あり

                    ' 実績ステータスチェック
                    If row.RESULT_STATUS.Equals(C_STALLPROSESS_WASH_WAIT) Then
                        ' 実績ステータス：洗車待ち
                        Me.mDispDiv = DisplayDivPreparation    ' 納車準備
                        Me.mDispStart = DisplayStartNone       ' 仕掛前
                        Return

                    ElseIf row.RESULT_STATUS.Equals(C_STALLPROSESS_WASH_DOING) Or
                           row.RESULT_STATUS.Equals(C_STALLPROSESS_CARRY_WAIT) Or
                           row.RESULT_STATUS.Equals(C_STALLPROSESS_DELI_WAIT) Then
                        ' 実績ステータス：洗車中、納車待ち、預かり中
                        Me.mDispDiv = DisplayDivPreparation    ' 納車準備
                        Me.mDispStart = DisplayStartStart      ' 仕掛中
                        Return
                    End If
                End If
            End If

            ' 4 : 納車作業チェック

            ' R/Oステータスチェック
            If row.RO_STATUS.Equals(C_RO_STATUS_SALE_OK) Then
                ' R/Oステータス：売上済み
                Me.mDispDiv = DisplayDivDelivery       ' 納車作業
                Me.mDispStart = DisplayStartStart      ' 仕掛中
                Return

            ElseIf row.RO_STATUS.Equals(C_RO_STATUS_MANT_OK) Then
                ' R/Oステータス：整備完了
                Me.mDispDiv = DisplayDivDelivery       ' 納車作業
                Me.mDispStart = DisplayStartStart      ' 仕掛中
                Return

                'ElseIf row.RO_STATUS.Equals(C_RO_STATUS_ESTI_WAIT) Then
                '    ' R/Oステータス：見積確認待ち
                '    Me.mDispDiv = DisplayDivDelivery       ' 納車作業
                '    Me.mDispStart = DisplayStartStart      ' 仕掛中
                '    Return
            End If

            ' 5 : 完了チェック 

            ' R/Oステータスチェック
            If row.RO_STATUS.Equals(C_RO_STATUS_FINISH) Then
                ' R/Oステータス：納車完了
                Me.mDispDiv = DisplayDivNone       ' なし
                Me.mDispStart = DisplayStartNone   ' 仕掛前
                Return
            End If

            ' その他 
            Me.mDispDiv = DisplayDivNone       ' なし
            Me.mDispStart = DisplayStartNone   ' 仕掛前

        Finally
            ' 終了ログ
            Logger.Info(String.Format(CultureInfo.CurrentCulture _
                                    , "{0}.{1} {2}" _
                                    , Me.GetType.ToString _
                                    , System.Reflection.MethodBase.GetCurrentMethod.Name _
                                    , LOG_END))
        End Try
    End Sub

    '''-------------------------------------------------------
    ''' <summary>
    ''' チップ詳細ボタンチェック
    ''' </summary>
    ''' <param name="row">来店チップレコード</param>
    ''' <param name="dispDiv">表示区分</param>
    ''' <remarks></remarks>
    '''-------------------------------------------------------
    Private Sub CheckChipDetailButton(ByVal row As SC3140103VisitRow, ByVal dispDiv As String)

        '開始ログ
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                                , "{0}.{1} {2} IN:dispDiv = {3}" _
                                , Me.GetType.ToString _
                                , System.Reflection.MethodBase.GetCurrentMethod.Name _
                                , LOG_START _
                                , dispDiv))

        Me.mButtonLeft = ButtonNone
        Me.mButtonRight = ButtonNone
        Me.mButtonEnabledLeft = True
        Me.mButtonEnabledRight = True

        ' チップ情報チェック
        Select Case dispDiv
            Case DisplayDivReception       ' 受付
                ' 顧客区分チェック
                If row.CUSTSEGMENT.Equals(CustomerSegmentON) Then
                    ' 自社客
                    Me.mButtonLeft = ButtonCustomer

                Else
                    ' 未取引客
                    Me.mButtonLeft = ButtonNewCustomer
                    ' R/O 作成ロック
                    Me.mButtonEnabledRight = False

                End If

                Me.mButtonRight = ButtonNewRO

            Case DisplayDivApproval        ' 承認依頼
                Me.mButtonLeft = ButtonRODisplay
                Me.mButtonRight = ButtonApproval

            Case DisplayDivPreparation     ' 納車準備
                ' チェックシート有無チェック
                If Not row.CHECKSHEET_FLAG.Equals(C_CHECKSHEET_FLAG_ON) Then
                    ' チェックシート印刷なし
                    Me.mButtonEnabledLeft = False

                End If

                Me.mButtonLeft = ButtonCheckSheet
                Me.mButtonRight = ButtonSettlement

            Case DisplayDivDelivery        ' 納車作業
                ' チェックシート有無チェック
                If Not row.CHECKSHEET_FLAG.Equals(C_CHECKSHEET_FLAG_ON) Then
                    ' チェックシート印刷なし
                    Me.mButtonEnabledLeft = False

                End If

                Me.mButtonLeft = ButtonCheckSheet
                Me.mButtonRight = ButtonSettlement

            Case DisplayDivWork            ' 作業中
                Me.mButtonLeft = ButtonRODisplay
                Me.mButtonRight = ButtonWork

            Case Else

        End Select

        '終了ログ
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                                , "{0}.{1} {2}" _
                                , Me.GetType.ToString _
                                , System.Reflection.MethodBase.GetCurrentMethod.Name _
                                , LOG_END))

    End Sub

#End Region

#Region " サービス来店実績・ストール予約・実績"

    '''-------------------------------------------------------
    ''' <summary>
    ''' サービス来店実績・ストール予約実績取得
    ''' </summary>
    ''' <param name="dtService">サービス来店情報データセット</param>
    ''' <param name="dtRezinfo">ストール予約データセット</param>
    ''' <param name="dtProcess">ストール実績データセット</param>
    ''' <returns>来店チップデータセット</returns>
    ''' <remarks></remarks>
    '''-------------------------------------------------------
    Private Function SetVisit(ByVal dtService As SC3140103ServiceVisitManagementDataTable, ByVal dtRezinfo As SC3140103StallRezinfoDataTable, ByVal dtProcess As SC3140103StallProcessDataTable) As SC3140103VisitDataTable

        '開始ログ
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                                , "{0}.{1} {2}" _
                                , Me.GetType.ToString _
                                , System.Reflection.MethodBase.GetCurrentMethod.Name _
                                , LOG_START))

        Dim dt As SC3140103VisitDataTable = New SC3140103VisitDataTable
        Dim rowRezinfo As SC3140103StallRezinfoRow
        Dim rowProcess As SC3140103StallProcessRow

        For Each rowService As SC3140103ServiceVisitManagementRow In dtService.Rows
            Dim row As SC3140103VisitRow = dt.NewSC3140103VisitRow()

            ' 来店実績連番
            row.VISITSEQ = rowService.VISITSEQ
            ' 販売店コード
            row.DLRCD = rowService.DLRCD
            ' 店舗コード
            row.STRCD = rowService.STRCD
            ' 予約ID
            row.FREZID = rowService.FREZID
            ' 割振りSA
            row.SACODE = rowService.SACODE
            ' VIPマーク
            row.VIP_MARK = C_FLAG_OFF
            ' JDP調査対象客マーク
            row.JDP_MARK = C_FLAG_OFF
            ' 技術情報マーク
            row.SSC_MARK = C_FLAG_OFF
            ' 駐車場コード
            row.PARKINGCODE = rowService.PARKINGCODE
            ' 来店時刻
            row.VISITTIMESTAMP = rowService.VISITTIMESTAMP
            ' チェックシート有無
            row.CHECKSHEET_FLAG = C_FLAG_OFF
            ' SA割振り日時
            row.ASSIGNTIMESTAMP = rowService.ASSIGNTIMESTAMP
            ' 整備受注NO
            row.ORDERNO = rowService.ORDERNO
            ' 追加作業承認数
            row.APPROVAL_COUNT = 0
            ' 追加作業承認
            row.APPROVAL_STATUS = C_FLAG_OFF
            ' 追加作業承認印刷
            row.APPROVAL_OUTPUT = C_FLAG_OFF
            ' 追加作業承認依頼時刻
            row.APPROVAL_TIME = DateTime.MinValue
            ' 顧客区分
            row.CUSTSEGMENT = rowService.CUSTSEGMENT
            ' 顧客コード
            row.CUSTID = rowService.DMSID

            'ストール予約取得
            Dim fRezId As Long
            If Not rowService.IsFREZIDNull() Then
                fRezId = rowService.FREZID
            Else
                fRezId = SC3140103DataTableAdapter.MinReserveId
            End If
            rowRezinfo = Me.GetVisitRezinfo(fRezId, dtRezinfo)
            ' 予約マーク
            If rowRezinfo.WALKIN.Equals(C_STALLREZINFO_WALKIN_REZ) Then
                row.REZ_MARK = C_FLAG_ON
            Else
                row.REZ_MARK = C_FLAG_OFF
            End If
            ' 登録番号
            row.VCLREGNO = Me.SetReplaceString(rowRezinfo.VCLREGNO, rowService.VCLREGNO)
            ' 車種
            row.VEHICLENAME = rowRezinfo.VEHICLENAME
            ' モデル
            row.MODELCODE = Me.SetReplaceString(rowRezinfo.MODELCODE, rowService.MODELCODE)
            ' VIN
            row.VIN = Me.SetReplaceString(rowRezinfo.VIN, rowService.VIN)
            ' 走行距離
            row.MILEAGE = rowRezinfo.MILEAGE
            ' 納車予定日時
            row.REZ_DELI_DATE = rowRezinfo.REZ_DELI_DATE
            ' 顧客名
            row.CUSTOMERNAME = Me.SetReplaceString(rowRezinfo.CUSTOMERNAME, rowService.NAME)
            ' 電話番号
            row.TELNO = Me.SetReplaceString(rowRezinfo.TELNO, rowService.TELNO)
            ' 携帯番号
            row.MOBILE = Me.SetReplaceString(rowRezinfo.MOBILE, rowService.MOBILE)
            ' 代表入庫項目
            row.MERCHANDISENAME = rowRezinfo.MERCHANDISENAME
            ' 作業終了予定時刻
            row.ENDTIME = rowRezinfo.ENDTIME
            ' 作業開始
            row.ACTUAL_STIME = rowRezinfo.ACTUAL_STIME
            ' 作業終了
            row.ACTUAL_ETIME = rowRezinfo.ACTUAL_ETIME
            ' 完成検査有無
            row.COMP_INS_FLAG = C_COMP_INS_FLAG_ON
            ' 洗車有無
            row.WASHFLG = rowRezinfo.WASHFLG
            ' 予約ID
            row.REZINFO_FREZID = rowRezinfo.PREZID

            ' ストール実績取得
            Dim rezId As Long
            If Not rowService.IsREZIDNull() Then
                rezId = rowRezinfo.REZID
            Else
                rezId = SC3140103DataTableAdapter.MinReserveId
            End If
            rowProcess = Me.GetVisitProcess(rezId, dtProcess)
            ' 実績ステータス
            row.RESULT_STATUS = rowProcess.RESULT_STATUS
            ' 作業終了予定時刻
            If Not String.IsNullOrEmpty(rowProcess.REZ_END_TIME) Then
                ' 予定_ストール終了日時時刻で更新
                row.ENDTIME = DateTimeFunc.FormatString("yyyyMMddHHmm", rowProcess.REZ_END_TIME)
            End If
            ' 洗車開始
            row.RESULT_WASH_START = rowProcess.RESULT_WASH_START
            ' 洗車終了
            row.RESULT_WASH_END = rowProcess.RESULT_WASH_END
            ' 担当テクニシャン
            row.STAFFCD = 0
            ' 担当テクニシャン名
            row.STAFFNAME = rowProcess.STAFFNAME
            ' 予約ID
            row.PROCESS_FREZID = rowRezinfo.PREZID

            dt.AddSC3140103VisitRow(row)
        Next

        '終了ログ
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                                , "{0}.{1} {2}" _
                                , Me.GetType.ToString _
                                , System.Reflection.MethodBase.GetCurrentMethod.Name _
                                , LOG_END))

        '処理結果返却
        Return dt
    End Function

    '''-------------------------------------------------------
    ''' <summary>
    ''' ストール予約取得
    ''' </summary>
    ''' <param name="fRezId">初回予約ID</param>
    ''' <param name="dtRezinfo">ストール予約データセット</param>
    ''' <returns>ストール予約レコード</returns>
    ''' <remarks></remarks>
    '''-------------------------------------------------------
    Private Function GetVisitRezinfo(ByVal fRezId As Long, ByVal dtRezinfo As SC3140103StallRezinfoDataTable) As SC3140103StallRezinfoRow

        '開始ログ
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                                , "{0}.{1} {2} IN:fRezId ={3}" _
                                , Me.GetType.ToString _
                                , System.Reflection.MethodBase.GetCurrentMethod.Name _
                                , LOG_START _
                                , fRezId))

        Dim row As SC3140103StallRezinfoRow = dtRezinfo.NewSC3140103StallRezinfoRow()
        Dim aryDtRezInfo As DataRow() = dtRezinfo.Select(String.Format(CultureInfo.CurrentCulture, "PREZID = {0}", fRezId), " ENDTIME DESC, STARTTIME DESC")

        ' 件数チェック
        If aryDtRezInfo Is Nothing OrElse aryDtRezInfo.Length = 0 Then
            ' 該当行無し
            ' 販売店コード
            row.DLRCD = String.Empty
            ' 店舗コード
            row.STRCD = String.Empty
            ' 予約ID
            'row.REZID = fRezId
            row.REZID = SC3140103DataTableAdapter.MinReserveId
            ' 管理予約ID
            'row.PREZID = fRezId
            row.PREZID = SC3140103DataTableAdapter.MinReserveId
            ' 使用開始日時
            row.STARTTIME = DateTime.MinValue
            ' 使用開始日時
            row.ENDTIME = DateTime.MinValue
            ' 顧客コード
            row.CUSTCD = String.Empty
            ' 氏名
            row.CUSTOMERNAME = String.Empty
            ' 電話番号
            row.TELNO = String.Empty
            ' 携帯番号
            row.MOBILE = String.Empty
            ' 車名
            row.VEHICLENAME = String.Empty
            ' 登録ナンバー
            row.VCLREGNO = String.Empty
            ' VIN
            row.VIN = String.Empty
            ' 商品コード
            row.MERCHANDISECD = String.Empty
            ' 商品名
            row.MERCHANDISENAME = String.Empty
            ' モデル
            row.MODELCODE = String.Empty
            ' 走行距離
            row.MILEAGE = -1
            ' 洗車有無
            row.WASHFLG = C_FLAG_OFF
            ' 来店フラグ
            row.WALKIN = String.Empty
            ' 予約_納車_希望日時時刻
            row.REZ_DELI_DATE = String.Empty
            ' 作業開始
            row.ACTUAL_STIME = DateTime.MinValue
            ' 作業終了
            row.ACTUAL_ETIME = DateTime.MinValue
        Else
            Dim rowRezInfo As SC3140103StallRezinfoRow = DirectCast(aryDtRezInfo(0), SC3140103StallRezinfoRow)

            ' 販売店コード
            row.DLRCD = rowRezInfo.DLRCD
            ' 店舗コード
            row.STRCD = rowRezInfo.STRCD
            ' 予約ID
            row.REZID = rowRezInfo.REZID
            ' 管理予約ID
            row.PREZID = rowRezInfo.PREZID
            ' 使用開始日時
            row.STARTTIME = rowRezInfo.STARTTIME
            ' 使用開始日時
            row.ENDTIME = rowRezInfo.ENDTIME
            ' 顧客コード
            row.CUSTCD = rowRezInfo.CUSTCD
            ' 氏名
            row.CUSTOMERNAME = rowRezInfo.CUSTOMERNAME
            ' 電話番号
            row.TELNO = rowRezInfo.TELNO
            ' 携帯番号
            row.MOBILE = rowRezInfo.MOBILE
            ' 車名
            row.VEHICLENAME = rowRezInfo.VEHICLENAME
            ' 登録ナンバー
            row.VCLREGNO = rowRezInfo.VCLREGNO
            ' VIN
            row.VIN = rowRezInfo.VIN
            ' 商品コード
            row.MERCHANDISECD = rowRezInfo.MERCHANDISECD
            ' 商品名
            row.MERCHANDISENAME = rowRezInfo.MERCHANDISENAME
            ' モデル
            row.MODELCODE = rowRezInfo.MODELCODE
            ' 走行距離
            row.MILEAGE = rowRezInfo.MILEAGE
            ' 洗車有無
            row.WASHFLG = rowRezInfo.WASHFLG
            ' 来店フラグ
            row.WALKIN = rowRezInfo.WALKIN
            ' 予約_納車_希望日時時刻
            row.REZ_DELI_DATE = rowRezInfo.REZ_DELI_DATE
            ' 作業開始
            row.ACTUAL_STIME = rowRezInfo.ACTUAL_STIME
            ' 作業終了
            row.ACTUAL_ETIME = rowRezInfo.ACTUAL_ETIME

            ' 作業開始の取得(最小)
            Dim rowRezInfoMin As SC3140103StallRezinfoRow
            Dim aryDtRezInfoMin As DataRow() = dtRezinfo.Select(String.Format(CultureInfo.CurrentCulture, "PREZID = {0}", fRezId), " ACTUAL_STIME")

            If aryDtRezInfoMin IsNot Nothing AndAlso aryDtRezInfoMin.Length > 0 Then
                rowRezInfoMin = DirectCast(aryDtRezInfoMin(0), SC3140103StallRezinfoRow)
                ' 作業開始
                row.ACTUAL_STIME = rowRezInfoMin.ACTUAL_STIME
            End If
        End If

        '終了ログ
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                                , "{0}.{1} {2}" _
                                , Me.GetType.ToString _
                                , System.Reflection.MethodBase.GetCurrentMethod.Name _
                                , LOG_END))

        Return row
    End Function

    '''-------------------------------------------------------
    ''' <summary>
    ''' ストール実績取得
    ''' </summary>
    ''' <param name="rezId">予約ID</param>
    ''' <param name="dtProcess">ストール実績データセット</param>
    ''' <returns>ストール実績レコード</returns>
    ''' <remarks></remarks>
    '''-------------------------------------------------------
    Private Function GetVisitProcess(ByVal rezId As Long, ByVal dtProcess As SC3140103StallProcessDataTable) As SC3140103StallProcessRow

        '開始ログ
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                                , "{0}.{1} {2} IN:rezId ={3}" _
                                , Me.GetType.ToString _
                                , System.Reflection.MethodBase.GetCurrentMethod.Name _
                                , LOG_START _
                                , rezId.ToString(CultureInfo.CurrentCulture)))

        Dim row As SC3140103StallProcessRow = dtProcess.NewSC3140103StallProcessRow()
        Dim aryProcess As DataRow() = dtProcess.Select(String.Format(CultureInfo.CurrentCulture, "REZID = {0}", rezId), "DSEQNO DESC, SEQNO DESC, STAFFNAME ASC")

        ' 件数チェック
        If aryProcess Is Nothing OrElse aryProcess.Length = 0 Then
            '該当行無し
            ' 販売店コード
            row.DLRCD = String.Empty
            ' 店舗コード
            row.STRCD = String.Empty
            ' 予約ID
            'row.REZID = rezId
            row.REZID = SC3140103DataTableAdapter.MinReserveId
            ' 管理予約ID
            'row.PREZID = rezId
            row.PREZID = SC3140103DataTableAdapter.MinReserveId
            ' 日跨ぎシーケンス番号
            row.DSEQNO = 0
            ' シーケンス番号
            row.SEQNO = 0
            ' 洗車有無
            row.WASHFLG = C_FLAG_OFF
            ' 実績_ステータス
            row.RESULT_STATUS = C_STALLPROSESS_NONE
            ' 予定_ストール終了日時時刻
            row.REZ_END_TIME = String.Empty
            ' 洗車開始
            row.RESULT_WASH_START = String.Empty
            ' 洗車終了
            row.RESULT_WASH_END = String.Empty
            ' 担当テクニシャン
            row.STAFFCD = String.Empty
            ' 担当テクニシャン名
            row.STAFFNAME = String.Empty
        Else
            Dim rowProcess As SC3140103StallProcessRow

            rowProcess = DirectCast(aryProcess(0), SC3140103StallProcessRow)

            ' 販売店コード
            row.DLRCD = rowProcess.DLRCD
            ' 店舗コード
            row.STRCD = rowProcess.STRCD
            ' 予約ID
            row.REZID = rowProcess.REZID
            ' 管理予約ID
            row.PREZID = rowProcess.PREZID
            ' 日跨ぎシーケンス番号
            row.DSEQNO = rowProcess.DSEQNO
            ' シーケンス番号
            row.SEQNO = rowProcess.SEQNO
            ' 洗車有無
            row.WASHFLG = rowProcess.WASHFLG
            ' 実績_ステータス
            row.RESULT_STATUS = rowProcess.RESULT_STATUS
            ' 予定_ストール終了日時時刻
            row.REZ_END_TIME = rowProcess.REZ_END_TIME
            ' 洗車開始
            row.RESULT_WASH_START = rowProcess.RESULT_WASH_START
            ' 洗車終了
            row.RESULT_WASH_END = rowProcess.RESULT_WASH_END
            ' 担当テクニシャン
            row.STAFFCD = rowProcess.STAFFCD
            ' 担当テクニシャン名
            row.STAFFNAME = rowProcess.STAFFNAME
        End If

        '終了ログ
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                                , "{0}.{1} {2}" _
                                , Me.GetType.ToString _
                                , System.Reflection.MethodBase.GetCurrentMethod.Name _
                                , LOG_END))

        Return row
    End Function

#End Region

#Region " 外部IF処理"

    '''-------------------------------------------------------
    ''' <summary>
    ''' SA別未納者R/O一覧
    ''' </summary>
    ''' <param name="staffInfo">スタッフ情報</param>
    ''' <returns>SA別未納者R/O一覧データセット</returns>
    ''' <remarks></remarks>
    '''-------------------------------------------------------
    Private Function GetIFNoDeliveryROList(ByVal staffInfo As StaffContext) As IC3801003NoDeliveryRODataTable

        '開始ログ
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                                , "{0}.{1} {2}" _
                                , Me.GetType.ToString _
                                , System.Reflection.MethodBase.GetCurrentMethod.Name _
                                , LOG_START))

        Dim bl As IC3801003BusinessLogic = New IC3801003BusinessLogic
        Dim dt As IC3801003NoDeliveryRODataTable

        '検索処理
        ' IF用にSAコードの調整-「@」より前のSAコード取得
        Dim renameSACode As String = Me.SetRenameSACode(staffInfo)

        'IF用ログ
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                                  , "CALL IF:IC3801003BusinessLogic.GetNoDeliveryROList IN:dlrcd={0}, saCode={1}" _
                                  , staffInfo.DlrCD _
                                  , renameSACode))

        dt = bl.GetNoDeliveryROList(staffInfo.DlrCD, renameSACode)

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                                  , "CALL IF:IC3801003BusinessLogic.GetNoDeliveryROList OUT:Count = {0}" _
                                  , dt.Rows.Count))

        '終了ログ
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                                , "{0}.{1} {2}" _
                                , Me.GetType.ToString _
                                , System.Reflection.MethodBase.GetCurrentMethod.Name _
                                , LOG_END))

        '処理結果返却
        Return dt
    End Function

    '''-------------------------------------------------------
    ''' <summary>
    ''' R/O基本情報参照
    ''' </summary>
    ''' <param name="staffInfo">スタッフ情報</param>
    ''' <param name="orderNo">R/O番号</param>
    ''' <returns>R/O基本情報データセット</returns>
    ''' <remarks></remarks>
    '''-------------------------------------------------------
    Private Function GetIFROBaseInformationList(ByVal staffInfo As StaffContext, ByVal orderNo As String) As IC3801001OrderCommDataTable

        '開始ログ
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                                , "{0}.{1} {2}" _
                                , Me.GetType.ToString _
                                , System.Reflection.MethodBase.GetCurrentMethod.Name _
                                , LOG_START))

        Dim bl As IC3801001BusinessLogic = New IC3801001BusinessLogic
        Dim dt As IC3801001OrderCommDataTable

        'IF用ログ
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                                  , "CALL IF:IC3801001BusinessLogic.GetROBaseInfoList IN:dealercode={0}, orderNo={1}" _
                                  , staffInfo.DlrCD _
                                  , orderNo))

        ' R/O基本情報参照
        dt = bl.GetROBaseInfoList(staffInfo.DlrCD, orderNo)

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                                  , "CALL IF:IC3801001BusinessLogic.GetROBaseInfoList OUT:Count = {0}" _
                                  , dt.Rows.Count))

        '終了ログ
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                                , "{0}.{1} {2}" _
                                , Me.GetType.ToString _
                                , System.Reflection.MethodBase.GetCurrentMethod.Name _
                                , LOG_END))

        '処理結果返却
        Return dt

    End Function

    '''-------------------------------------------------------
    ''' <summary>
    ''' 追加承認待ち情報取得
    ''' </summary>
    ''' <param name="staffInfo">スタッフ情報</param>
    ''' <returns>追加承認待ち情報データセット</returns>
    ''' <remarks></remarks>
    '''-------------------------------------------------------
    Private Function GetIFApprovalConfirmAddList(ByVal staffInfo As StaffContext) As IC3801002DataSet.ConfirmAddListDataTable

        '開始ログ
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                                , "{0}.{1} {2}" _
                                , Me.GetType.ToString _
                                , System.Reflection.MethodBase.GetCurrentMethod.Name _
                                , LOG_START))

        Dim bl As IC3801002BusinessLogic = New IC3801002BusinessLogic
        Dim dt As IC3801002DataSet.ConfirmAddListDataTable

        ' IF用にSAコードの調整-「@」より前のSAコード取得
        Dim renameSACode As String = Me.SetRenameSACode(staffInfo)

        'IF用ログ
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                                  , "CALL IF:IC3801002BusinessLogic.GetConfirmAddList IN:dealerCode={0}, branchCode={1}, saCode={2}" _
                                  , staffInfo.DlrCD _
                                  , staffInfo.BrnCD _
                                  , renameSACode))

        ' 追加承認待ち情報取得
        dt = bl.GetConfirmAddList(staffInfo.DlrCD, staffInfo.BrnCD, renameSACode)

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                                  , "CALL IF:IC3801002BusinessLogic.GetConfirmAddList OUT:Count = {0}" _
                                  , dt.Rows.Count))

        '終了ログ
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                                , "{0}.{1} {2}" _
                                , Me.GetType.ToString _
                                , System.Reflection.MethodBase.GetCurrentMethod.Name _
                                , LOG_END))

        '処理結果返却
        Return dt
    End Function

    '''-------------------------------------------------------
    ''' <summary>
    ''' SAコードの調整-「@」より前のSAコード取得
    ''' </summary>
    ''' <param name="staffInfo">スタッフ情報</param>
    ''' <returns>「@」より前のSAコード</returns>
    ''' <remarks></remarks>
    '''-------------------------------------------------------
    Private Function SetRenameSACode(ByVal staffInfo As StaffContext) As String

        ' IF用にSAコードの調整-「@」より前の文字列取得
        Dim splitString() As String
        Dim renameSACode As String = staffInfo.Account
        splitString = renameSACode.Split(CType("@", Char))
        renameSACode = splitString(0)

        '処理結果返却
        Return renameSACode
    End Function

    '''-------------------------------------------------------
    ''' <summary>
    ''' SAコードの調整-「@」より前のSAコード取得
    ''' </summary>
    ''' <param name="staffInfo">スタッフ情報</param>
    ''' <returns>「@」より前のSAコード</returns>
    ''' <remarks></remarks>
    '''-------------------------------------------------------
    Private Function SetRenameSACode(ByVal staffInfo As String) As String

        ' IF用にSAコードの調整-「@」より前の文字列取得
        Dim splitString() As String
        Dim renameSACode As String
        splitString = staffInfo.Split(CType("@", Char))
        renameSACode = splitString(0)

        '処理結果返却
        Return renameSACode

    End Function

    '''-------------------------------------------------------
    ''' <summary>
    ''' 整備受注№作成
    ''' </summary>
    ''' <param name="serviceInfo">来店実績データロウ</param>
    ''' <param name="visitSeq">来店実績連番</param>
    ''' <param name="staffInfo">ログインスタッフ情報</param>
    ''' <returns>整備受注№配列 【(0)：　整備受注№、(1)： Update結果】</returns>
    ''' <remarks></remarks>
    '''-------------------------------------------------------
    Public Function GetIFCreateOrderNo(ByVal serviceInfo As SC3140103VisitRow, ByVal visitSeq As Long, ByVal staffInfo As StaffContext) As String()

        '開始ログ
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                                , "{0}.{1} {2} IN:visitSeq = {3}" _
                                , Me.GetType.ToString _
                                , System.Reflection.MethodBase.GetCurrentMethod.Name _
                                , LOG_START _
                                , visitSeq))

        ' 整備受注NO 作成情報(0-整備受注NO、1-UpDate結果)
        Dim createOrderInformation(2) As String

        Dim rowAddOrderSave As IC3810301inOrderSaveRow
        Dim dtIF As IC3801102CreateOrderStructDataTable      ' 外部インターフェースIF(BMTS) 整備受注NO
        Dim bl As IC3801102BusinessLogic = New IC3801102BusinessLogic

        Dim upDateCheck As Long = Long.MinValue
        Dim orderNo As String = String.Empty

        If serviceInfo IsNot Nothing Then

            ' IF用にSAコードの調整-「@」より前のSAコード取得
            Dim renameSACode As String = Me.SetRenameSACode(serviceInfo.SACODE)

            'IF用ログ
            Logger.Info(String.Format(CultureInfo.CurrentCulture _
                                      , "CALL IF:IC3801102BusinessLogic.AddOrderSave IN:dealerCode={0}, registerNo={1}, vinNo={2}, model={3}, customerName={4}, customTel1={5}, rezid={6}, brncd={7}, saCode={8}" _
                                      , serviceInfo.DLRCD _
                                      , serviceInfo.VCLREGNO _
                                      , serviceInfo.VIN _
                                      , serviceInfo.MODELCODE _
                                      , serviceInfo.CUSTOMERNAME _
                                      , serviceInfo.TELNO _
                                      , CType(serviceInfo.FREZID, String) _
                                      , serviceInfo.STRCD _
                                      , renameSACode))

            ' 整備受注NO作成処理
            dtIF = bl.AddOrderSave(serviceInfo.DLRCD, serviceInfo.VCLREGNO, serviceInfo.VIN, serviceInfo.MODELCODE,
                                                 serviceInfo.CUSTOMERNAME, serviceInfo.TELNO, CType(serviceInfo.FREZID, String), serviceInfo.STRCD, renameSACode)

            Logger.Info(String.Format(CultureInfo.CurrentCulture _
                                    , "CALL IF:IC3801102BusinessLogic.AddOrderSave OUT:Count = {0}" _
                                    , dtIF.Rows.Count))

            Try
                ' 外部インターフェースから取得したData確認
                If Not IsNothing(dtIF) AndAlso dtIF.Count = 1 Then
                    '初回予約IDで条件の絞込みを行っているため、複数件データが取得されることはありえない
                    Dim rowIF As IC3801102CreateOrderStructRow = DirectCast(dtIF.Rows(0), IC3801102CreateOrderStructRow)

                    ' 整備受注NO作成結果確認
                    If rowIF.ISCREATE Then

                        ' 引数設定
                        Using dtAddOrderSave As New IC3810301inOrderSaveDataTable
                            rowAddOrderSave = dtAddOrderSave.NewIC3810301inOrderSaveRow()
                        End Using

                        rowAddOrderSave.DLRCD = rowIF.CREATEDEALERCODE      '販売店コード
                        rowAddOrderSave.STRCD = serviceInfo.STRCD           '店舗コード
                        rowAddOrderSave.ORDERNO = rowIF.CREATEORDERNO       'R/O番号
                        rowAddOrderSave.VISITSEQ = visitSeq                 '来店実績連番
                        rowAddOrderSave.SACODE = serviceInfo.SACODE         'SAコード
                        rowAddOrderSave.ACCOUNT = staffInfo.Account         'ログインID
                        rowAddOrderSave.SYSTEM = MAINMENUID                 '画面ID

                        Logger.Info("CALL IF:IC3810301BusinessLogic.AddOrderSave")

                        ' 整備受注NO反映
                        Using blSC3810301 As IC3810301BusinessLogic = New IC3810301BusinessLogic
                            upDateCheck = blSC3810301.AddOrderSave(rowAddOrderSave)
                        End Using

                        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                                                , "CALL IF:IC3810301BusinessLogic.AddOrderSave OUT:RETURNCODE = {0}" _
                                                , upDateCheck))

                        ' 整備受注NO保持
                        orderNo = rowIF.CREATEORDERNO
                    End If
                End If
            Finally
                dtIF.Dispose()
            End Try
        End If

        ' 反映結果確認
        Select Case upDateCheck
            Case C_RET_SUCCESS
                createOrderInformation(0) = orderNo         ' 整備受注NO
                createOrderInformation(1) = C_RET_SUCCESS.ToString(CultureInfo.CurrentCulture)
            Case C_RET_DBTIMEOUT
                createOrderInformation(1) = C_RET_DBTIMEOUT.ToString(CultureInfo.CurrentCulture)  ' タイムアウト
            Case C_RET_NOMATCH
                createOrderInformation(1) = C_RET_NOMATCH.ToString(CultureInfo.CurrentCulture)    ' その他
            Case C_RET_DIFFSACODE
                createOrderInformation(1) = C_RET_DIFFSACODE.ToString(CultureInfo.CurrentCulture) ' 担当SA外
            Case Else
                createOrderInformation(1) = C_RET_NOMATCH.ToString(CultureInfo.CurrentCulture)    ' その他
        End Select

        '終了ログ
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                                , "{0}.{1} {2} OUT:CreateOrderInformation(0) = {3}, CreateOrderInformation(1) = {4}" _
                                , Me.GetType.ToString _
                                , System.Reflection.MethodBase.GetCurrentMethod.Name _
                                , LOG_END _
                                , createOrderInformation(0) _
                                , createOrderInformation(1)))

        '処理結果返却
        Return createOrderInformation
    End Function

    '''-------------------------------------------------------
    ''' <summary>
    ''' 顧客参照
    ''' </summary>
    ''' <param name="serviceInfo">来店実績データロウ</param>
    ''' <returns>顧客情報格納データセット</returns>
    ''' <remarks></remarks>
    '''-------------------------------------------------------
    Public Function GetIFCustomerInformation(ByVal serviceInfo As SC3140103VisitRow) As IC3800703SrvCustomerDataTable

        '開始ログ
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                                , "{0}.{1} {2}" _
                                , Me.GetType.ToString _
                                , System.Reflection.MethodBase.GetCurrentMethod.Name _
                                , LOG_START))

        Dim bl As IC3800703BusinessLogic = New IC3800703BusinessLogic
        Dim dt As IC3800703SrvCustomerDataTable = New IC3800703SrvCustomerDataTable

        If serviceInfo IsNot Nothing Then
            'IF用ログ
            Logger.Info(String.Format(CultureInfo.CurrentCulture _
                                      , "CALL IF:IC3800703BusinessLogic.GetCustomerInfo IN:registerNo={0}, vinNo={1}, dealerCode={2}" _
                                      , serviceInfo.VCLREGNO _
                                      , serviceInfo.VIN _
                                      , serviceInfo.DLRCD))

            ' 顧客参照処理
            dt = bl.GetCustomerInfo(serviceInfo.VCLREGNO, serviceInfo.VIN, serviceInfo.DLRCD)

            Logger.Info(String.Format(CultureInfo.CurrentCulture _
                                      , "CALL IF:IC3800703BusinessLogic.GetCustomerInfo OUT:Count = {0}" _
                                      , dt.Rows.Count))
        End If

        '終了ログ
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                                , "{0}.{1} {2}" _
                                , Me.GetType.ToString _
                                , System.Reflection.MethodBase.GetCurrentMethod.Name _
                                , LOG_END))

        '処理結果返却
        Return dt
    End Function

#End Region

#Region " その他処理"

    '''-------------------------------------------------------
    ''' <summary>
    ''' 時間チェック
    ''' </summary>
    ''' <param name="time">対象時間</param>
    ''' <returns>True:正常値 False:エラー値</returns>
    ''' <remarks></remarks>
    '''-------------------------------------------------------
    Private Function IsDateTimeNull(ByVal time As DateTime) As Boolean

        ' 日付チェック
        If time.Equals(DateTime.MinValue) Then
            Return True
        End If

        Return False

    End Function

    '''-------------------------------------------------------
    ''' <summary>
    ''' データ置換
    ''' </summary>
    ''' <param name="valBefore">データ元</param>
    ''' <param name="valAfter">データ先</param>
    ''' <returns>置換データ</returns>
    ''' <remarks></remarks>
    '''-------------------------------------------------------
    Private Function SetReplaceString(ByVal valBefore As String, ByVal valAfter As String) As String

        ' データ元存在チェック
        If Not String.IsNullOrEmpty(valBefore) Then
            ' データ元あり
            Return valBefore
        End If

        ' データ先空白チェック
        If String.IsNullOrEmpty(valAfter) Then
            ' データ先なし
            Return valBefore
        End If

        ' データ先で置換
        Return valAfter

    End Function

    '''-------------------------------------------------------
    ''' <summary>
    ''' データ置換
    ''' </summary>
    ''' <param name="valBefore">データ元</param>
    ''' <param name="valAfter">データ先</param>
    ''' <returns>置換データ</returns>
    ''' <remarks></remarks>
    '''-------------------------------------------------------
    Private Function SetReplaceLong(ByVal valBefore As Long, ByVal valAfter As Long) As Long

        ' データ元存在チェック
        If valBefore > 0 Then
            ' データ元あり
            Return valBefore
        End If

        ' データ先空白チェック
        If valAfter = 0 Then
            ' データ先なし
            Return valBefore
        End If

        ' データ先で置換
        Return valAfter

    End Function

#End Region

#Region " ストール設定情報取得"

    '''-------------------------------------------------------
    ''' <summary>
    ''' ストール設定情報取得
    ''' </summary>
    ''' <returns>ストール設定データセット</returns>
    ''' <remarks></remarks>
    '''-------------------------------------------------------
    Public Function GetStallControl() As SC3140103StallCtl2DataTable

        '開始ログ
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                                , "{0}.{1} {2}" _
                                , Me.GetType.ToString _
                                , System.Reflection.MethodBase.GetCurrentMethod.Name _
                                , LOG_START))

        Dim dt As SC3140103StallCtl2DataTable
        Dim staffInfo As StaffContext = StaffContext.Current

        Using da As New SC3140103DataTableAdapter
            '検索処理
            dt = da.GetStallControl(staffInfo.DlrCD, staffInfo.BrnCD)
        End Using

        '終了ログ
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                                , "{0}.{1} {2}" _
                                , Me.GetType.ToString _
                                , System.Reflection.MethodBase.GetCurrentMethod.Name _
                                , LOG_END))

        '処理結果返却
        Return dt
    End Function

#End Region

End Class
