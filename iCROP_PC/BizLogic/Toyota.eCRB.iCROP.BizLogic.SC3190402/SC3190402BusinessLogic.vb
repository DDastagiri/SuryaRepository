'━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
'SC3190402BusinessLogic.vb
'─────────────────────────────────────
'機能： 部品庫モニター画面
'補足： 見積もり待ちエリア
'作成： 2014/XX/XX NEC 村瀬
'更新： 2014/09/09 TMEJ Y.Gotoh 部品庫B／O管理に向けた評価用アプリ作成 $01
'更新： 2014/09/14 TMEJ M.Asano サービスタブレットDMS連携追加開発(部品庫モニター在庫無し表示) $02
'       2015/03/16 TMEJ M.Asano DMS連携版サービスタブレット サービスアドバイザ情報連携追加開発 $03
'更新： 2017/03/16 NSK A.Minagawa TR-SVT-TMT-20161229-001 部品庫モニターでエラーを検知 $04
'更新： 2019/11/05 NSK M.Sakamoto (トライ店システム評価)部品出庫オペレーションにおける遅れ管理機能強化検証 $05
'─────────────────────────────────────

Imports System.Text
Imports Toyota.eCRB.SystemFrameworks.Core
Imports Toyota.eCRB.SystemFrameworks.Web
Imports Toyota.eCRB.SystemFrameworks.Core.iCROP.BizLogic
Imports Toyota.eCRB.SystemFrameworks.Core.iCROP.DataAccess
Imports Toyota.eCRB.PartsManagement.PSMonitor.BizLogic
Imports Toyota.eCRB.PartsManagement.PSMonitor.DataAccess
Imports Toyota.eCRB.DMSLinkage.PartsInfo.Api.DataAccess
Imports Toyota.eCRB.DMSLinkage.PartsInfo.Api.BizLogic
Imports System.Globalization

Public Class SC3190402BusinessLogic
    Inherits BaseBusinessComponent
    Implements IDisposable, ISC3190402BusinessLogic

#Region "定数"
#Region "ROステータス"
    ''' <summary>
    ''' 部品仮見積中
    ''' </summary>
    Public Shared ConsRoStatus_25 As String = "25"
    ''' <summary>
    ''' 部品本見積中
    ''' </summary>
    Public Shared ConsRoStatus_30 As String = "30"
    ''' <summary>
    ''' 着工指示待ち(顧客承認待ち)
    ''' </summary>
    Public Shared ConsRoStatus_50 As String = "50"
    ''' <summary>
    ''' 作業中
    ''' </summary>
    Public Shared ConsRoStatus_60 As String = "60"
    '$01 部品庫B／O管理に向けた評価用アプリ作成 START
    ''' <summary>
    ''' 完成検査完了
    ''' </summary>
    Public Shared ConsRoStatus_70 As String = "70"
    '$01 部品庫B／O管理に向けた評価用アプリ作成 END
    ''' <summary>
    ''' 納車準備待ち
    ''' </summary>
    Public Shared ConsRoStatus_80 As String = "80"
    '$01 部品庫B／O管理に向けた評価用アプリ作成 START
    ''' <summary>
    ''' 完成検査完了
    ''' </summary>
    Public Shared ConsRoStatus_85 As String = "85"
    ''' <summary>
    ''' 完成検査完了
    ''' </summary>
    Public Shared ConsRoStatus_90 As String = "90"
    ''' <summary>
    ''' 完成検査完了
    ''' </summary>
    Public Shared ConsRoStatus_99 As String = "99"
    '$01 部品庫B／O管理に向けた評価用アプリ作成 END
#End Region

#Region "ストール利用ステータス"
    ''' <summary>
    ''' '着工指示待ち
    ''' </summary>
    Public Shared ConsStallUseStatus_00 As String = "00"
    ''' <summary>
    ''' '作業開始待ち
    ''' </summary>
    Public Shared ConsStallUseStatus_01 As String = "01"
    ''' <summary>
    ''' '作業中
    ''' </summary>
    Public Shared ConsStallUseStatus_02 As String = "02"
    ''' <summary>
    ''' '完了
    ''' </summary>
    Public Shared ConsStallUseStatus_03 As String = "03"
    ''' <summary>
    ''' '一部中断
    ''' </summary>
    Public Shared ConsStallUseStatus_04 As String = "04"
    ''' <summary>
    ''' '中断
    ''' </summary>
    Public Shared ConsStallUseStatus_05 As String = "05"
    ''' <summary>
    ''' '日跨ぎ終了
    ''' </summary>
    Public Shared ConsStallUseStatus_06 As String = "06"
    ''' <summary>
    ''' '未来店客
    ''' </summary>
    Public Shared ConsStallUseStatus_07 As String = "07"
#End Region

#Region "作業指示の着工指示フラグ"
    ''' <summary>
    ''' '指示済
    ''' </summary>
    Public Shared ConsStartWorkInstructFlg_Yes As String = "1"
    ''' <summary>
    ''' '未指示
    ''' </summary>
    Public Shared ConsStartWorkInstructFlg_No As String = "0"
#End Region

#Region "追加作業対象の枝番"
    ''' <summary>
    ''' '追加作業該当連番
    ''' </summary>
    Public Shared ConsAddRepair As Integer = 1
#End Region

#Region "RO親番号判断"
    ''' <summary>
    ''' 'RO親番号判断
    ''' </summary>
    Public Shared ConsRoParent As String = "0"
#End Region

#Region "作業計画待ちエリアタイトルの文言No"
    ''' <summary>
    ''' '文言No
    ''' </summary>
    ''' <remarks>改行タグ込み文言対策</remarks>
    Public Shared ConsArea02DisplayNo As Integer = 3
#End Region

#Region "部品出庫ステータス"
    ''' <summary>部品出庫していない</summary>
    Dim ConsPartsIssueStatus_NoIssue As String = "0"
    ''' <summary>部品を部分出庫</summary>
    Dim ConsPartsIssueStatus_Partsissuing As String = "1"
    ''' <summary>部品を全数出庫</summary>
    Dim ConsPartsIssueStatus_AllPartsIssuedCompletely As String = "8"

    ' $02 Start サービスタブレットDMS連携追加開発(部品庫モニター在庫無し表示)
    ''' <summary>部品在庫なし</summary>
    Public Shared ConsPartsIssueStatus_NoStock As String = "4"
    ' $02 End   サービスタブレットDMS連携追加開発(部品庫モニター在庫無し表示)
#End Region

#Region "ログ文言"
    ''' <summary>
    ''' Log開始用文言
    ''' </summary>
    ''' <remarks></remarks>
    Public Const ConsLogStart As String = "Start"

    ''' <summary>
    ''' Log終了文言
    ''' </summary>
    ''' <remarks></remarks>
    Public Const ConsLogEnd As String = "End"
#End Region

#Region "WebService呼び出しリトライ回数"
    ''' <summary>
    ''' WebService呼び出しリトライ回数
    ''' </summary>
    ''' <remarks></remarks>
    Public Const ConsRetryWebServiceMaxCount As Integer = 3
#End Region

#Region "音声ファイル"
    ''' <summary>
    ''' '新着お知らせ時の音声ファイル
    ''' </summary>
    Public Shared ConsWhatsNewMP3 As String = "../Styles/Images/SC3190402/whatsnew.mp3"
#End Region

#Region "記号等"
    ''' <summary>
    ''' 半角スペース
    ''' </summary>
    ''' <remarks></remarks>
    Public Shared ConsSpace As String = "&nbsp;"

    ''' <summary>
    ''' キャンセルフラグ
    ''' </summary>
    ''' <remarks></remarks>
    Const ConsCancelFlgYes As String = "0"

    ''' <summary>
    ''' スラッシュ
    ''' </summary>
    Const ConsSlash As String = "／"

    ''' <summary>
    ''' カンマ
    ''' </summary>
    Const ConsComma As String = ","

    ''' <summary>
    ''' ハイフン
    ''' </summary>
    Const ConsHyphen As String = "-"
#End Region

    '$04 TR-SVT-TMT-20161229-001 部品庫モニターでエラーを検知 START
#Region "オラクルエラー（ロック解放待機時間を超過）"
    ''' <summary>
    ''' オラクルエラー（ロック解放待機時間を超過）
    ''' </summary>
    ''' <remarks></remarks>
    Private Const OracleErrorResourceBusy As Integer = 30006
#End Region
    '$04 TR-SVT-TMT-20161229-001 部品庫モニターでエラーを検知 END

#Region "販売店システム設定"
    ''' <summary>
    ''' 各エリアの最大表示明細数
    ''' </summary>
    ''' <remarks></remarks>
    Public Shared ConsKeyChipDispMaxCount As String = "CHIP_DISP_MAX_COUNT"
    Public Shared ConsValueChipDispMaxCount As Integer = 7

    ''' <summary>
    ''' 追加作業見積もり待ちエリアの赤明細を判断する時間（分）
    ''' </summary>
    ''' <remarks></remarks>
    Public Shared ConsKeyDelayPeriodMinute As String = "DELAY_PERIOD_MINUTE"
    Public Shared ConsValueDelayPeriodMinute As Integer = 10

    ''' <summary>
    ''' 追加作業見積り待ちエリア改ページ間隔(秒)
    ''' </summary>
    Public Shared ConsKeyPagingIntervalAddJob As String = "PAGING_INTERVAL_ADD_JOB"

    ''' <summary>
    ''' 作業計画待ちエリア改ページ間隔(秒)
    ''' </summary>
    Public Shared ConsKeyPagingIntervalJobInstruct As String = "PAGING_INTERVAL_JOB_INSTRUCT"

    ''' <summary>
    ''' 出庫待ちエリア改ページ間隔(秒)
    ''' </summary>
    Public Shared ConsKeyPagingIntervalShipment As String = "PAGING_INTERVAL_SHIPMENT"

    ''' <summary>
    ''' 引き取り待ちエリア改ページ間隔(秒)
    ''' </summary>
    Public Shared ConsKeyPagingIntervalPick As String = "PAGING_INTERVAL_PICK"

    ''' <summary>
    ''' 改ページ間隔デフォルト値(秒)
    ''' </summary>
    Public Shared ConsValuePagingInterval As Integer = 10

    ''' <summary>
    ''' 取得データの最大件数
    ''' </summary>
    Public Shared ConsKeyChipAcquisitionMaxCount As String = "CHIP_ACQUISITION_MAX_COUNT"
    Public Shared ConsValueChipAcquisitionMaxCount As Integer = 70

    'M.Sakamoto (トライ店システム評価)部品出庫オペレーションにおける遅れ管理機能強化検証 START $05
    ''' <summary>
    ''' 更新時間間隔(分)
    ''' </summary>
    Public Shared ConstKeyPsmonitorDelayUpdateInterval As String = "PSMONITOR_DELAY_UPDATE_INTERVAL"
    Public Shared ConstValuePsmonitorDelayUpdateInterval As Integer = 1
    'M.Sakamoto (トライ店システム評価)部品出庫オペレーションにおける遅れ管理機能強化検証 END $05

#End Region

    '$01 部品庫B／O管理に向けた評価用アプリ作成 START
#Region "エリアタイプ"
    ''' <summary>
    ''' エリアタイプ：出庫待ち
    ''' </summary>
    ''' <remarks></remarks>
    Public Shared AreaType_03 As String = "3"
    ''' <summary>
    ''' エリアタイプ：引き取り待ち
    ''' </summary>
    ''' <remarks></remarks>
    Public Shared AreaType_04 As String = "4"
#End Region
    '$01 部品庫B／O管理に向けた評価用アプリ作成 END

    ' $03 DMS連携版サービスタブレット サービスアドバイザ情報連携追加開発 START
    ''' <summary>
    ''' DB初期値：日付
    ''' </summary>
    ''' <remarks></remarks>
    Private Const DB_DEFAULT_VALUE_DATE As Date = #1/1/1900#
    ' $03 DMS連携版サービスタブレット サービスアドバイザ情報連携追加開発 END
#End Region

#Region "セッション関連"
    Public Shared SessionKeyArea01RoData As String = "Area01RoData"
    Public Shared SessionKeyArea03RoData As String = "Area03RoData"
#End Region

#Region "Node名・タグ名"
    ''' <summary>
    ''' Node名(Parts Result)
    ''' </summary>
    ''' <remarks></remarks>
    Private Const NodeResult As String = "//Parts_Result/ResultCode"

    ''' <summary>
    ''' Tag名(ResultCode)
    ''' </summary>
    ''' <remarks></remarks>
    Private Const TagResultCode As String = "ResultCode"

    ''' <summary>
    ''' Tag名(DealerCode)
    ''' </summary>
    ''' <remarks></remarks>
    Private Const TagDealerCode As String = "DealerCode"

    ''' <summary>
    ''' Tag名(BranchCode)
    ''' </summary>
    ''' <remarks></remarks>
    Private Const TagBranchCode As String = "BranchCode"

    ''' <summary>
    ''' Node名(PARTS_STATUS)
    ''' </summary>
    ''' <remarks></remarks>
    Private Const NodePARTS_STATUS As String = "//Parts_Result/PARTS_STATUS"

    ''' <summary>
    ''' Tag名(R_O)
    ''' </summary>
    ''' <remarks></remarks>
    Private Const TagR_O As String = "R_O"

    ''' <summary>
    ''' Tag名(SocialID)
    ''' </summary>
    ''' <remarks></remarks>
    Private Const TagR_O_SEQNO As String = "R_O_SEQNO"

    ''' <summary>
    ''' Tag名(PARTS_ISSUE_STATUS)
    ''' </summary>
    ''' <remarks></remarks>
    Private Const TagPARTS_ISSUE_STATUS As String = "PARTS_ISSUE_STATUS"

    ''' <summary>
    ''' Tag名(RequestedStaffID)
    ''' </summary>
    ''' <remarks></remarks>
    Private Const TagRequestedStaffID As String = "RequestedStaffID"

    ''' <summary>
    ''' Node名(BILL)
    ''' </summary>
    ''' <remarks></remarks>
    Private Const NodeBILL As String = "BILL"

    ''' <summary>
    ''' Tag名(BillNo)
    ''' </summary>
    ''' <remarks></remarks>
    Private Const TagBillNo As String = "BillNo"

    ''' <summary>
    ''' Tag名(JobID)
    ''' </summary>
    ''' <remarks></remarks>
    Private Const TagJobID As String = "JobID"

    ''' <summary>
    ''' Tag名(PartsStaffID)
    ''' </summary>
    ''' <remarks></remarks>
    Private Const TagPartsStaffName As String = "PartsStaffName"

    ''' <summary>
    ''' Tag名(CageNO)
    ''' </summary>
    ''' <remarks></remarks>
    Private Const TagCageNO As String = "CageNO"
#End Region

#Region "列挙体"
    ' ''' <summary>
    ' ''' PS01(res)のPARTS_ISSUE_STATUS値
    ' ''' </summary>
    'Enum Enum_PartsIssueStatus
    '    ''' <summary>部品出庫していない</summary>
    '    NoIssue = 0
    '    ''' <summary>部品を部分出庫</summary>
    '    Partsissuing = 1
    '    ''' <summary>部品を全数出庫</summary>
    '    AllPartsIssuedCompletely = 8
    'End Enum

    ' $03 DMS連携版サービスタブレット サービスアドバイザ情報連携追加開発 START
    ''' <summary>
    ''' 時刻の書式(DATETIMEFORMのコンバートID)
    ''' </summary>
    Public Enum Enum_DateTimeForm
        ConvID_03 = 3    'YYYY/MM/DD
        ConvID_11 = 11   'dd:MM
        ConvID_14 = 14   'HH:mm
    End Enum
    ' $03 DMS連携版サービスタブレット サービスアドバイザ情報連携追加開発 END

    ''' <summary>
    ''' '更新区分
    ''' </summary>
    Public Enum Enum_RefreshKbn
        DispInit = 0        '初回起動
        DispRefresh = 1     '更新
    End Enum

    ''' <summary>
    ''' 明細種別
    ''' </summary>
    Enum Enum_BackColor
        Red = 0         '赤
        Normal = 1      '通常
        NA = 9          '対象外
    End Enum

    ''' <summary>
    ''' 新着種別
    ''' </summary>
    Enum Enum_WhatsNew
        Yes = 0         '新着あり
        No = 1          '新着なし
    End Enum

#End Region

    ''' <summary>
    ''' 作業計画待ちDataTableのINDEX
    ''' </summary>
    Enum Enum_Area02DtColIndex
        SORT_KEY = 0
        RO_NUM = 1
        RO_SEQ = 2
        REG_NUM = 3
        MODEL_NAME = 4
        SCHE_DELI_DATETIME = 5
        DLR_CD = 6
        BRN_CD = 7
    End Enum

    ''' <summary>
    ''' 出庫待ちDataTableのINDEX
    ''' </summary>
    Enum Enum_Area03DtColIndex
        SORT_KEY = 0
        RO_NUM = 1
        RO_SEQ = 2
        REG_NUM = 3
        MODEL_NAME = 4
        GRADE_NAME = 5
        STALL_NAME_SHORT = 6
        SCHE_START_DATETIME = 7
        '$01 部品庫B／O管理に向けた評価用アプリ作成 START
        DLR_CD = 8
        BRN_CD = 9
        STALL_USE_STATUS = 10
        BILL_NO = 11
        CAGE_NO = 12
        '$01 部品庫B／O管理に向けた評価用アプリ作成 END
    End Enum

    ''' <summary>
    ''' 引き取り待ちDataTable(Res)のINDEX
    ''' </summary>
    Enum Enum_Area04DtColIndex
        RO_NUM = 0
        RO_SEQ = 1
        REG_NUM = 2
        MODEL_NAME = 3
        GRADE_NAME = 4
        STALL_NAME_SHORT = 5
        SCHE_START_DATETIME = 6
        SCHE_DELI_DATETIME = 7
        DLR_CD = 8
        BRN_CD = 9
    End Enum

    '$01 部品庫B／O管理に向けた評価用アプリ作成 START
    ''' <summary>
    ''' 部品ステータス情報のINDEX
    ''' </summary>
    Enum Enum_AreaSVItemIndex
        PARTS_ISSUE_STATUS = 0
        BILL_NO = 1
        PARTS_STAFF_NAME = 2
        CAGE_NO = 3
    End Enum
    '$01 部品庫B／O管理に向けた評価用アプリ作成 END

#Region "Public処理"
    '2014/07/15 改ページ機能追加により
    '           引数「MAX表示件数(chipsDispMaxCount)」を
    '               「MAX取得件数(chipAcquisitionMaxCount)」に変更
    ''' <summary>
    ''' 追加見積もり待ちデータ取得
    ''' </summary>
    ''' <param name="nowDate">現在日時</param>
    ''' <param name="delayPeriodMinute">遅れ判定時間</param>
    ''' <param name="chipAcquisitionMaxCount">MAX取得件数</param>
    ''' <returns>データテーブル</returns>
    ''' <remarks>ROステータス及びRO作業連番を条件にデータを取得する</remarks>
    Public Function GetWaitingforPartsQuotationListData(ByVal nowDate As Date, _
                                                        ByVal delayPeriodMinute As Integer, _
                                                        ByVal chipAcquisitionMaxCount As Integer, _
                                                        ByRef selectDataCount As Integer _
                                                        ) As SC3190402DataSet.AREA01DataTable

        '開始ログ
        '$04 TR-SVT-TMT-20161229-001 部品庫モニターでエラーを検知 START
        'Logger.Error(String.Format(CultureInfo.CurrentCulture _
        '   , "{0}.{1} {2} P1:{3} P2:{4} P3:{5}" _
        '   , Me.GetType.ToString _
        '   , System.Reflection.MethodBase.GetCurrentMethod.Name _
        '   , ConsLogStart _
        '   , nowDate.ToString _
        '   , delayPeriodMinute.ToString _
        '   , chipAcquisitionMaxCount.ToString _
        '   ))
        '$04 TR-SVT-TMT-20161229-001 部品庫モニターでエラーを検知 END

        '追加見積もり待ちデータを取得する
        Using dispArea01Data As SC3190402DataSet.AREA01DataTable = _
                    SC3190402DataSet.GetWaitingforPartsQuotationList( _
                                StaffContext.Current.DlrCD, _
                                StaffContext.Current.BrnCD, _
                                {ConsRoStatus_25, ConsRoStatus_30}, _
                                ConsAddRepair)
            '返却用の該当件数をセットする
            selectDataCount = dispArea01Data.Rows.Count
            '$04 TR-SVT-TMT-20161229-001 部品庫モニターでエラーを検知 START
            'Logger.Error("DEBUG:dispArea01Data.Rows.Count=" & dispArea01Data.Rows.Count.ToString)
            '$04 TR-SVT-TMT-20161229-001 部品庫モニターでエラーを検知 END
            'データがあるか？
            If dispArea01Data.Rows.Count > 0 Then
                For Each row As SC3190402DataSet.AREA01Row In dispArea01Data.Rows
                    '遅れ判定の基準となる時間を算出する(RO作成日時＋Ｎ分)
                    Dim checkDate As Date = row.RO_CREATE_DATETIME.AddMinutes(delayPeriodMinute)
                    '現在日時＞RO作成日時＋Ｎ分だったら赤明細とみなし、フラグを立てる
                    If nowDate > checkDate Then
                        row.SORT_KEY = Enum_BackColor.Red
                    Else
                        row.SORT_KEY = Enum_BackColor.Normal
                    End If
                Next

                'データビューでソートする
                Dim dv As DataView = New DataView(dispArea01Data)
                dv.Sort = "SORT_KEY, RO_CREATE_DATETIME, RO_NUM, RO_SEQ"

                Using dispArea01DataClone As SC3190402DataSet.AREA01DataTable = dispArea01Data.Clone
                    Dim cnt As Integer = 0
                    For Each drv As DataRowView In dv
                        If cnt >= chipAcquisitionMaxCount Then
                            '表示最大数に達したらループを抜ける
                            Exit For
                        Else
                            'ソートされたレコードをコピーする
                            dispArea01DataClone.ImportRow(drv.Row)
                            cnt = cnt + 1
                        End If
                    Next

                    '$04 TR-SVT-TMT-20161229-001 部品庫モニターでエラーを検知 START
                    ''終了ログ
                    'Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    '   , "{0}.{1} {2} QUERY:COUNT = {3}" _
                    '   , Me.GetType.ToString _
                    '   , System.Reflection.MethodBase.GetCurrentMethod.Name _
                    '   , ConsLogEnd _
                    '   , dispArea01DataClone.Rows.Count))
                    '$04 TR-SVT-TMT-20161229-001 部品庫モニターでエラーを検知 END

                    Return dispArea01DataClone
                End Using
            End If

            '終了ログ
            '$04 TR-SVT-TMT-20161229-001 部品庫モニターでエラーを検知 START
            'Logger.Error(String.Format(CultureInfo.CurrentCulture _
            '   , "{0}.{1} {2} QUERY:COUNT = {3}" _
            '   , Me.GetType.ToString _
            '   , System.Reflection.MethodBase.GetCurrentMethod.Name _
            '   , ConsLogEnd _
            '   , dispArea01Data.Rows.Count))
            '$04 TR-SVT-TMT-20161229-001 部品庫モニターでエラーを検知 END

            Return dispArea01Data
        End Using
    End Function

    '2014/07/15 改ページ機能追加により
    '           引数「MAX表示件数(chipsDispMaxCount)」を
    '               「MAX取得件数(chipAcquisitionMaxCount)」に変更
    ''' <summary>
    ''' 作業計画待ちデータ取得
    ''' </summary>
    ''' <param name="nowdate">現在日時</param>
    ''' <param name="chipAcquisitionMaxCount">MAX取得件数</param>
    ''' <returns>データテーブル</returns>
    ''' <remarks>ROステータス及びストール利用ステータスを条件にデータを取得する</remarks>
    Public Function GetWaitingforJobPlanningListData(ByVal nowDate As Date, _
                                                    ByVal chipAcquisitionMaxCount As Integer, _
                                                    ByRef selectDataCount As Integer _
                                                    ) As SC3190402DataSet.AREA02DataTable

        Dim dispArea02Data As SC3190402DataSet.AREA02DataTable = Nothing
        Try
            '開始ログ
            '$04 TR-SVT-TMT-20161229-001 部品庫モニターでエラーを検知 START
            'Logger.Error(String.Format(CultureInfo.CurrentCulture _
            '   , "{0}.{1} {2} P1:{3} P2:{4}" _
            '   , Me.GetType.ToString _
            '   , System.Reflection.MethodBase.GetCurrentMethod.Name _
            '   , ConsLogStart _
            '   , nowDate.ToString _
            '   , chipAcquisitionMaxCount.ToString _
            '   ))
            '$04 TR-SVT-TMT-20161229-001 部品庫モニターでエラーを検知 END

            '作業計画待ちデータを取得する
            '2014/06/13 ストール利用ステータス条件を削除
            'Using dispArea02Data As SC3190402DataSet.AREA02DataTable = _
            '        SC3190402DataSet.GetWatingforJobPlanningList( _
            '                StaffContext.Current.DlrCD, _
            '                StaffContext.Current.BrnCD, _
            '                {ConsRoStatus_50}, _
            '                {ConsStallUseStatus_00, ConsStallUseStatus_01, ConsStallUseStatus_02}, _
            '                {ConsStartWorkInstructFlg_Yes, ConsStartWorkInstructFlg_No}, _
            '                 ConsStartWorkInstructFlg_No)
            dispArea02Data = _
                    SC3190402DataSet.GetWatingforJobPlanningList( _
                            StaffContext.Current.DlrCD, _
                            StaffContext.Current.BrnCD, _
                            {ConsRoStatus_50}, _
                            {ConsStartWorkInstructFlg_Yes, ConsStartWorkInstructFlg_No}, _
                             ConsStartWorkInstructFlg_No)

            '返却用の該当件数をセットする(ここからNG分を差し引いていく)
            selectDataCount = dispArea02Data.Rows.Count
            '$04 TR-SVT-TMT-20161229-001 部品庫モニターでエラーを検知 START
            'Logger.Error("DEBUG:dispArea02Data.Rows.Count=" & dispArea02Data.Rows.Count.ToString)
            '$04 TR-SVT-TMT-20161229-001 部品庫モニターでエラーを検知 START
            'データがあるか？
            If dispArea02Data.Rows.Count > 0 Then
                Dim sbRoNum As StringBuilder = New StringBuilder 'サービス用RO番号
                Dim sbRoSeq As StringBuilder = New StringBuilder 'サービス用RO連番
                Dim dic As New SortedDictionary(Of String, String) 'キーと値の型を引数で指定する
                Dim dicKey As String = String.Empty
                Dim dicSv As New Dictionary(Of String, String) 'キーと値の型を引数で指定する
                Dim dicSvKey As String = String.Empty

                'サービスに引き渡すためRO番号及びRO連番を配列に渡す
                Dim listRoNum As New List(Of String)
                Dim listRoSeq As New List(Of String)
                For Each row As SC3190402DataSet.AREA02Row In dispArea02Data.Rows
                    '2014/06/13 ストール利用ステータス条件を削除
                    'SQLの抽出条件から外す代わりにここで判断を行う
                    Dim dataFlg As Boolean = False '対象データフラグ
                    If row.RO_SEQ.ToString.TrimEnd.Equals(ConsRoParent) Then
                        'RO親の場合、ストール利用ステータス=00,01,02のデータだけ抽出する
                        If row.STALL_USE_STATUS.ToString.TrimEnd.Equals(ConsStallUseStatus_00) OrElse
                           row.STALL_USE_STATUS.ToString.TrimEnd.Equals(ConsStallUseStatus_01) OrElse
                           row.STALL_USE_STATUS.ToString.TrimEnd.Equals(ConsStallUseStatus_02) Then
                            dataFlg = True
                        Else
                            '$04 TR-SVT-TMT-20161229-001 部品庫モニターでエラーを検知 START
                            'Logger.Info(String.Format("DEBUG:NG:RO親／ストール利用ステータス対象外:{0}:{1}:{2}", _
                            '                          row.RO_NUM.ToString.TrimEnd, _
                            '                          row.RO_SEQ.ToString.TrimEnd, _
                            '                          row.STALL_USE_STATUS.ToString.TrimEnd))
                            '$04 TR-SVT-TMT-20161229-001 部品庫モニターでエラーを検知 END
                            'NGの場合は表示件数を1減らす
                            selectDataCount = selectDataCount - 1
                        End If
                    Else
                        'RO子の場合、ストール利用ステータスに関わらず抽出対象とする
                        dataFlg = True
                    End If
                    '対象データのみDictionaryに書き込む
                    If dataFlg = True Then
                        listRoNum.Add(row.RO_NUM.ToString.TrimEnd)
                        listRoSeq.Add(row.RO_SEQ.ToString.TrimEnd)
                        'Dictionaryに要素を追加する
                        dicKey = MakeDictionaryKey(row.RO_NUM.ToString.TrimEnd, row.RO_SEQ.ToString.TrimEnd)
                        Dim dicValue As String = MakeDictionaryValueByDataRow(row)
                        dic.Add(dicKey, dicValue)
                    End If
                Next

                '部品ステータス情報取得用
                Dim retXML As String = String.Empty
                '実行カウント
                Dim RetryCount As Integer = 1
                '設定回数だけリトライを行う
                Do Until RetryCount > ConsRetryWebServiceMaxCount
                    '部品ステータス情報取得
                    retXML = GetPartsList(StaffContext.Current.DlrCD, _
                                          StaffContext.Current.BrnCD, _
                                          listRoNum.ToArray, _
                                          listRoSeq.ToArray)
                    '取得結果がブランク(エラー)だったらリトライする
                    If retXML.TrimEnd.Length > 0 Then
                        Exit Do
                    Else
                        '$04 TR-SVT-TMT-20161229-001 部品庫モニターでエラーを検知 START
                        'Logger.Error(String.Format("DEBUG:Call GetPartsList {0}回目 NG!", RetryCount.ToString))
                        '$04 TR-SVT-TMT-20161229-001 部品庫モニターでエラーを検知 END
                        RetryCount = RetryCount + 1
                    End If
                Loop

                '並び替えを行うためにDataViewを用意
                Dim dv As DataView = Nothing
                If retXML.TrimEnd.Length > 0 Then
                    '取得XMLの解析
                    Dim xml As Xml.XmlDocument = New Xml.XmlDocument
                    xml.LoadXml(retXML)
                    'ResultCodeタグの検索結果をセットする
                    Dim retXmlNode As Xml.XmlNode = xml.SelectSingleNode(NodeResult)
                    If Not IsNothing(retXmlNode) Then
                        'タグが取得できた
                        Dim retCD As String = retXmlNode.InnerText
                        '正常終了か？
                        If retCD = IC3190402BusinessLogic.ResultSuccess Then
                            dicKey = ""
                            '<PARTS_STATUS>の数だけループする
                            Dim nodes As Xml.XmlNodeList = xml.SelectNodes(NodePARTS_STATUS)
                            For i As Integer = 0 To nodes.Count - 1
                                'RO番号、RO連番をキーにして取得結果をDictionaryへ保存する
                                Dim nd As Xml.XmlNode = nodes(i)
                                dicSvKey = MakeDictionaryKey(nd.SelectSingleNode(TagR_O).InnerText, _
                                                           nd.SelectSingleNode(TagR_O_SEQNO).InnerText)
                                dicSv.Add(dicSvKey, nd.SelectSingleNode(TagPARTS_ISSUE_STATUS).InnerText)
                            Next

                            'e-CRBの取得結果とサービスの返却内容を突き合わせる
                            Using tempArea02Data As SC3190402DataSet.AREA02DataTable = dispArea02Data.Clone
                                For Each dicItem As KeyValuePair(Of String, String) In dic
                                    ' $02 Start サービスタブレットDMS連携追加開発(部品庫モニター在庫無し表示)
                                    If dicSv.ContainsKey(dicItem.Key) = True Then
                                        'サービス側にRO情報が存在したとき
                                        '全数出庫か？

                                        'サービス側の部品ステータスをチェックする
                                        If dicSv(dicItem.Key).Equals(ConsPartsIssueStatus_AllPartsIssuedCompletely) Then
                                            '全数出庫は対象外(部品の有無チェック不要)
                                            '$04 TR-SVT-TMT-20161229-001 部品庫モニターでエラーを検知 START
                                            'Logger.Info("DEBUG:NG:全数出庫:dicItem.Key=" & dicItem.Key)
                                            '$04 TR-SVT-TMT-20161229-001 部品庫モニターでエラーを検知 END
                                            'NGの場合は表示件数を1減らす
                                            selectDataCount = selectDataCount - 1
                                        Else
                                            '出庫無し・部分出庫及び在庫無しは対象
                                            Dim newRow As SC3190402DataSet.AREA02Row = _
                                                    MakeWatingforJobPlanningData(nowDate, dic.Item(dicItem.Key), tempArea02Data)

                                            ' 部品出庫ステータスを保持
                                            newRow.PARTS_ISSUE_STATUS = dicSv(dicItem.Key)

                                            '戻り用DataTableに追加
                                            tempArea02Data.Rows.Add(newRow)
                                        End If
                                    Else
                                        'サービス側にRO情報が存在しないときも対象とみなす
                                        Dim newRow As SC3190402DataSet.AREA02Row = _
                                        MakeWatingforJobPlanningData(nowDate, dic.Item(dicItem.Key), tempArea02Data)

                                        '戻り用DataTableに追加
                                        tempArea02Data.Rows.Add(newRow)
                                    End If
                                    ' $02 End   サービスタブレットDMS連携追加開発(部品庫モニター在庫無し表示)
                                Next
                                'データビューにコピーする
                                dv = New DataView(tempArea02Data.Copy)
                            End Using
                        Else
                            'Errorログ
                            '$04 TR-SVT-TMT-20161229-001 部品庫モニターでエラーを検知 START
                            'Logger.Error(String.Format(CultureInfo.CurrentCulture _
                            '   , "{0}.{1} <ResultCode> Tag Value is Not Successful." _
                            '   , Me.GetType.ToString _
                            '   , System.Reflection.MethodBase.GetCurrentMethod.Name _
                            '   ))
                            '$04 TR-SVT-TMT-20161229-001 部品庫モニターでエラーを検知 END

                            'サービスがエラーのときは全件移送する
                            'データビューにコピーする
                            dv = New DataView(GetAllWaitingforJobPlanningListData(nowDate, dispArea02Data).Copy)
                            'For Each row As SC3190402DataSet.AREA02Row In dispArea02Data.Rows
                            '    '赤明細のチェック
                            '    '現在日時＞納車予定時刻時刻だったら赤明細とみなし、フラグを立てる
                            '    row.SORT_KEY = CheckDelayOfDate(nowDate, row.SCHE_DELI_DATETIME)
                            'Next
                            ''データビューにコピーする
                            'dv = New DataView(dispArea02Data.Copy)
                        End If
                    Else
                        'Errorログ
                        '$04 TR-SVT-TMT-20161229-001 部品庫モニターでエラーを検知 START
                        'Logger.Error(String.Format(CultureInfo.CurrentCulture _
                        '   , "{0}.{1} <ResultCode> Tag is Not Found." _
                        '   , Me.GetType.ToString _
                        '   , System.Reflection.MethodBase.GetCurrentMethod.Name _
                        '   ))
                        '$04 TR-SVT-TMT-20161229-001 部品庫モニターでエラーを検知 END

                        'ResultCodeタグが取得できなかったときは全件移送する
                        'データビューにコピーする
                        dv = New DataView(GetAllWaitingforJobPlanningListData(nowDate, dispArea02Data).Copy)
                        'For Each row As SC3190402DataSet.AREA02Row In dispArea02Data.Rows
                        '    '赤明細のチェック
                        '    '現在日時＞納車予定時刻時刻だったら赤明細とみなし、フラグを立てる
                        '    row.SORT_KEY = CheckDelayOfDate(nowDate, row.SCHE_DELI_DATETIME)
                        'Next
                        ''データビューにコピーする
                        'dv = New DataView(dispArea02Data.Copy)
                    End If
                Else
                    'Errorログ
                    '$04 TR-SVT-TMT-20161229-001 部品庫モニターでエラーを検知 START
                    'Logger.Error(String.Format(CultureInfo.CurrentCulture _
                    '    , "{0}.{1} retXML is Blank." _
                    '    , Me.GetType.ToString _
                    '    , System.Reflection.MethodBase.GetCurrentMethod.Name _
                    '    ))
                    '$04 TR-SVT-TMT-20161229-001 部品庫モニターでエラーを検知 END

                    'retXMLがブランクだったときは全件移送する
                    'データビューにコピーする
                    dv = New DataView(GetAllWaitingforJobPlanningListData(nowDate, dispArea02Data).Copy)
                    'For Each row As SC3190402DataSet.AREA02Row In dispArea02Data.Rows
                    '    '赤明細のチェック
                    '    '現在日時＞納車予定時刻時刻だったら赤明細とみなし、フラグを立てる
                    '    row.SORT_KEY = CheckDelayOfDate(nowDate, row.SCHE_DELI_DATETIME)
                    'Next
                    ''データビューにコピーする
                    'dv = New DataView(dispArea02Data.Copy)
                End If

                'データビューでソートする
                dv.Sort = "SORT_KEY, SCHE_DELI_DATETIME, RO_NUM, RO_SEQ"

                '元DataTableをクリア
                dispArea02Data.Clear()
                ' ソート結果を一行ずつ格納
                Dim cnt As Integer = 0
                For Each drv As System.Data.DataRowView In dv
                    If cnt >= chipAcquisitionMaxCount Then
                        '表示最大数に達したらループを抜ける
                        Exit For
                    Else
                        ' DataRowとしてimportする
                        dispArea02Data.ImportRow(drv.Row)
                        cnt = cnt + 1
                    End If
                Next
            End If

            '画面に表示する件数を出力(抽出件数-NG件数のはず)
            '$04 TR-SVT-TMT-20161229-001 部品庫モニターでエラーを検知 START
            'Logger.Info("DEBUG:Return SelectCount=" & selectDataCount.ToString)
            '$04 TR-SVT-TMT-20161229-001 部品庫モニターでエラーを検知 END

            '終了ログ
            '$04 TR-SVT-TMT-20161229-001 部品庫モニターでエラーを検知 START
            'Logger.Error(String.Format(CultureInfo.CurrentCulture _
            '   , "{0}.{1} {2} QUERY:COUNT = {3}" _
            '   , Me.GetType.ToString _
            '   , System.Reflection.MethodBase.GetCurrentMethod.Name _
            '   , ConsLogEnd _
            '   , dispArea02Data.Rows.Count))
            '$04 TR-SVT-TMT-20161229-001 部品庫モニターでエラーを検知 END
            Return dispArea02Data

        Catch ex As Exception
            'エラー時
            Logger.Error(String.Format(CultureInfo.CurrentCulture _
                         , "{0}.{1} OUT:ErrGetWaitingforJobPlanningListData(ex) = {2}" _
                         , Me.GetType.ToString _
                         , System.Reflection.MethodBase.GetCurrentMethod.Name _
                         , ex.Message))

            'エラー時には0件データを返す
            dispArea02Data = New SC3190402DataSet.AREA02DataTable
            Return dispArea02Data

        Finally
            'オブジェクトの解放
            If Not IsNothing(dispArea02Data) Then
                dispArea02Data.Dispose()
            End If

        End Try

    End Function

    '2014/07/15 改ページ機能追加により
    '           引数「MAX表示件数(chipsDispMaxCount)」を
    '               「MAX取得件数(chipAcquisitionMaxCount)」に変更

    '$04 TR-SVT-TMT-20161229-001 部品庫モニターでエラーを検知 START
    ' ''' <summary>
    ' ''' 出庫待ちデータ取得
    ' ''' </summary>
    ' ''' <param name="nowdate">現在日時</param>
    ' ''' <param name="chipAcquisitionMaxCount">MAX取得件数</param>
    ' ''' <returns>データテーブル</returns>
    ' ''' <remarks>ROステータス及びストール利用ステータスを条件にデータを取得する</remarks>
    '<EnableCommit()> _
    'Public Function GetWaitingforPartsIssuingListData(ByVal nowDate As Date, _
    '                                             ByVal chipAcquisitionMaxCount As Integer, _
    '                                             ByRef selectDataCount As Integer _
    '                                             ) As SC3190402DataSet.AREA03DataTable _
    'Implements ISC3190402BusinessLogic.GetWaitingforPartsIssuingListData

    ''' <summary>
    ''' 出庫待ちデータ取得
    ''' </summary>
    ''' <param name="nowdate">現在日時</param>
    ''' <param name="chipAcquisitionMaxCount">MAX取得件数</param>
    ''' <returns>データテーブル</returns>
    ''' <remarks>ROステータス及びストール利用ステータスを条件にデータを取得する</remarks>
    Public Function GetWaitingforPartsIssuingListData(ByVal nowDate As Date, _
                                                     ByVal chipAcquisitionMaxCount As Integer, _
                                                     ByRef selectDataCount As Integer _
                                                     ) As SC3190402DataSet.AREA03DataTable
        '$04 TR-SVT-TMT-20161229-001 部品庫モニターでエラーを検知 END

        Dim dispArea03Data As SC3190402DataSet.AREA03DataTable = Nothing

        '$01 部品庫B／O管理に向けた評価用アプリ作成 START
        Dim tempArea03Data As SC3190402DataSet.AREA03DataTable = Nothing '一時work

        '$04 TR-SVT-TMT-20161229-001 部品庫モニターでエラーを検知 START
        'Dim tempArea03CageNoSetData As SC3190402DataSet.AREA03DataTable = Nothing
        '$04 TR-SVT-TMT-20161229-001 部品庫モニターでエラーを検知 END

        '$01 部品庫B／O管理に向けた評価用アプリ作成 END

        Try
            '開始ログ
            '$04 TR-SVT-TMT-20161229-001 部品庫モニターでエラーを検知 START
            'Logger.Error(String.Format(CultureInfo.CurrentCulture _
            '   , "{0}.{1} {2} P1:{3} P2:{4}" _
            '   , Me.GetType.ToString _
            '   , System.Reflection.MethodBase.GetCurrentMethod.Name _
            '   , ConsLogStart _
            '   , nowDate.ToString _
            '   , chipAcquisitionMaxCount.ToString _
            '   ))
            '$04 TR-SVT-TMT-20161229-001 部品庫モニターでエラーを検知 END

            '出庫待ちデータを取得する
            '2014/06/13 ストール利用ステータス条件を削除
            'Using dispArea03Data As SC3190402DataSet.AREA03DataTable = _
            'SC3190402DataSet.GetWatingforPartsIssuingList( _
            '            StaffContext.Current.DlrCD, _
            '            StaffContext.Current.BrnCD, _
            '            {ConsRoStatus_50, ConsRoStatus_60, ConsRoStatus_80}, _
            '            {ConsStallUseStatus_01, ConsStallUseStatus_02, ConsStallUseStatus_03}, _
            '            {ConsStartWorkInstructFlg_Yes, ConsStartWorkInstructFlg_No}, _
            '             ConsStartWorkInstructFlg_Yes)

            dispArea03Data = _
                SC3190402DataSet.GetWatingforPartsIssuingList( _
                            StaffContext.Current.DlrCD, _
                            StaffContext.Current.BrnCD, _
                            {ConsRoStatus_50, ConsRoStatus_60, ConsRoStatus_80}, _
                            {ConsStartWorkInstructFlg_Yes, ConsStartWorkInstructFlg_No}, _
                             ConsStartWorkInstructFlg_Yes)

            '返却用の該当件数をセットする(ここからNG分を差し引いていく)
            selectDataCount = dispArea03Data.Rows.Count
            '$04 TR-SVT-TMT-20161229-001 部品庫モニターでエラーを検知 START
            'Logger.Error("DEBUG:dispArea03Data.Rows.Count=" & dispArea03Data.Rows.Count.ToString)
            '$04 TR-SVT-TMT-20161229-001 部品庫モニターでエラーを検知 END
            'データがあるか？
            If dispArea03Data.Rows.Count > 0 Then
                Dim sbRoNum As StringBuilder = New StringBuilder 'サービス用RO番号
                Dim sbRoSeq As StringBuilder = New StringBuilder 'サービス用RO連番
                Dim dic As New Dictionary(Of String, String) 'キーと値の型を引数で指定する
                Dim dicKey As String = String.Empty
                Dim dicSv As New Dictionary(Of String, String) 'キーと値の型を引数で指定する
                Dim dicSvKey As String = String.Empty

                'サービスに引き渡すためRO番号及びRO連番を配列に渡す
                Dim listRoNum As New List(Of String)
                Dim listRoSeq As New List(Of String)
                For Each row As SC3190402DataSet.AREA03Row In dispArea03Data.Rows
                    '2014/06/13 ストール利用ステータス条件を削除
                    'SQLの抽出条件から外す代わりにここで判断を行う
                    Dim dataFlg As Boolean = False '対象データフラグ
                    '2014/07/31 部品のみ出庫対応(部品のみ出庫は枝番による処理分岐をしない)
                    If Not row.IsSCHE_START_DATETIMENull Then
                        '部品のみ出庫でない場合
                        If row.RO_SEQ.ToString.TrimEnd.Equals(ConsRoParent) Then
                            'RO親の場合、ストール利用ステータス=01,02,03のデータだけ抽出する
                            If row.STALL_USE_STATUS.ToString.TrimEnd.Equals(ConsStallUseStatus_01) OrElse
                               row.STALL_USE_STATUS.ToString.TrimEnd.Equals(ConsStallUseStatus_02) OrElse
                               row.STALL_USE_STATUS.ToString.TrimEnd.Equals(ConsStallUseStatus_03) Then
                                dataFlg = True
                            Else
                                '$04 TR-SVT-TMT-20161229-001 部品庫モニターでエラーを検知 START
                                'Logger.Info(String.Format("DEBUG:NG:RO親／ストール利用ステータス対象外:{0}:{1}:{2}", _
                                '                          row.RO_NUM.ToString.TrimEnd, _
                                '                          row.RO_SEQ.ToString.TrimEnd, _
                                '                          row.STALL_USE_STATUS.ToString.TrimEnd))
                                '$04 TR-SVT-TMT-20161229-001 部品庫モニターでエラーを検知 END
                                'NGの場合は表示件数を1減らす
                                selectDataCount = selectDataCount - 1
                            End If
                        Else
                            'RO子の場合、ストール利用ステータスに関わらず抽出対象とする
                            dataFlg = True
                        End If
                    Else
                        '部品のみ出庫の場合はストール利用ステータスチェック不要
                        '$04 TR-SVT-TMT-20161229-001 部品庫モニターでエラーを検知 START
                        'Logger.Info("DEBUG:部品のみ出庫／ストール利用ステータスチェック不要")
                        'Logger.Info(String.Format("DEBUG:OK:部品のみ出庫／ストール利用ステータスチェック不要:{0}:{1}", _
                        '                          row.RO_NUM.ToString.TrimEnd, _
                        '                          row.RO_SEQ.ToString.TrimEnd))
                        '$04 TR-SVT-TMT-20161229-001 部品庫モニターでエラーを検知 END
                        dataFlg = True
                    End If

                    '対象データのみDictionaryに書き込む
                    If dataFlg = True Then
                        listRoNum.Add(row.RO_NUM.ToString.TrimEnd)
                        listRoSeq.Add(row.RO_SEQ.ToString.TrimEnd)
                        'Dictionaryに要素を追加する
                        dicKey = MakeDictionaryKey(row.RO_NUM.ToString.TrimEnd, row.RO_SEQ.ToString.TrimEnd)
                        Dim dicValue As String = MakeDictionaryValueByDataRow(row)
                        dic.Add(dicKey, dicValue)
                    End If
                Next

                '部品ステータス情報取得用
                Dim retXML As String = String.Empty
                '実行カウント
                Dim RetryCount As Integer = 1
                '設定回数だけリトライを行う
                Do Until RetryCount > ConsRetryWebServiceMaxCount
                    '部品ステータス情報取得
                    retXML = GetPartsList(StaffContext.Current.DlrCD, _
                                          StaffContext.Current.BrnCD, _
                                          listRoNum.ToArray, _
                                          listRoSeq.ToArray)
                    '取得結果がブランク(エラー)だったらリトライする
                    If retXML.TrimEnd.Length > 0 Then
                        Exit Do
                    Else
                        '$04 TR-SVT-TMT-20161229-001 部品庫モニターでエラーを検知 START
                        'Logger.Error(String.Format("DEBUG:Call GetPartsList {0}回目 NG!", RetryCount.ToString))
                        '$04 TR-SVT-TMT-20161229-001 部品庫モニターでエラーを検知 END
                        RetryCount = RetryCount + 1
                    End If
                Loop

                '並び替えを行うためにDataViewを用意
                Dim dv As DataView = Nothing
                If retXML.TrimEnd.Length > 0 Then
                    '取得XMLの解析
                    Dim xml As Xml.XmlDocument = New Xml.XmlDocument
                    xml.LoadXml(retXML)
                    'ResultCodeタグの検索結果をセットする
                    Dim retXmlNode As Xml.XmlNode = xml.SelectSingleNode(NodeResult)
                    If Not IsNothing(retXmlNode) Then
                        'タグが取得できた
                        Dim retCD As String = retXmlNode.InnerText
                        '正常終了か？
                        If retCD = IC3190402BusinessLogic.ResultSuccess Then
                            dicKey = ""
                            '<PARTS_STATUS>の数だけループする
                            Dim nodes As Xml.XmlNodeList = xml.SelectNodes(NodePARTS_STATUS)
                            For i As Integer = 0 To nodes.Count - 1
                                'RO番号、RO連番をもとにDictionaryキーを作成する
                                Dim nd As Xml.XmlNode = nodes(i)
                                dicSvKey = MakeDictionaryKey(nd.SelectSingleNode(TagR_O).InnerText, _
                                                           nd.SelectSingleNode(TagR_O_SEQNO).InnerText)
                                dicSv.Add(dicSvKey, nd.SelectSingleNode(TagPARTS_ISSUE_STATUS).InnerText)
                            Next

                            'e-CRBの取得結果とサービスの返却内容を突き合わせる
                            tempArea03Data = dispArea03Data.Clone
                            For Each dicItem As KeyValuePair(Of String, String) In dic

                                ' $02 Start サービスタブレットDMS連携追加開発(部品庫モニター在庫無し表示)
                                If dicSv.ContainsKey(dicItem.Key) = True Then

                                    'サービス側にRO情報が存在したとき
                                    'サービス側の部品ステータスをチェックする
                                    If dicSv(dicItem.Key).Equals(ConsPartsIssueStatus_AllPartsIssuedCompletely) Then
                                        '全数出庫は対象外(部品の有無チェック不要)
                                        '$04 TR-SVT-TMT-20161229-001 部品庫モニターでエラーを検知 START
                                        'Logger.Info("DEBUG:NG:全数出庫:dicItem.Key=" & dicItem.Key)
                                        '$04 TR-SVT-TMT-20161229-001 部品庫モニターでエラーを検知 END
                                        'NGの場合は表示件数を1減らす
                                        selectDataCount = selectDataCount - 1
                                    Else
                                        '出庫無し・部分出庫及び在庫無しは対象
                                        'タブで区切って配列に格納後、データテーブルへセットする
                                        Dim newRow As SC3190402DataSet.AREA03Row = _
                                            Me.MakeWatingforPartsIssuingData(nowDate, dic.Item(dicItem.Key), tempArea03Data)

                                        ' 部品出庫ステータスを保持
                                        newRow.PARTS_ISSUE_STATUS = dicSv(dicItem.Key)

                                        '戻り用DataTableに追加
                                        tempArea03Data.Rows.Add(newRow)
                                    End If
                                Else
                                    'サービス側にRO情報が存在しないときも対象とみなす
                                    Dim newRow As SC3190402DataSet.AREA03Row = _
                                        Me.MakeWatingforPartsIssuingData(nowDate, dic.Item(dicItem.Key), tempArea03Data)

                                    ' 部品出庫ステータスを保持
                                    newRow.PARTS_ISSUE_STATUS = String.Empty

                                    '戻り用DataTableに追加
                                    tempArea03Data.Rows.Add(newRow)
                                End If
                                ' $02 End   サービスタブレットDMS連携追加開発(部品庫モニター在庫無し表示)
                            Next
                        Else
                            'Errorログ
                            '$04 TR-SVT-TMT-20161229-001 部品庫モニターでエラーを検知 START
                            'Logger.Error(String.Format(CultureInfo.CurrentCulture _
                            '   , "{0}.{1} <ResultCode> Tag Value is Not Successful." _
                            '   , Me.GetType.ToString _
                            '   , System.Reflection.MethodBase.GetCurrentMethod.Name _
                            '   ))
                            '$04 TR-SVT-TMT-20161229-001 部品庫モニターでエラーを検知 END

                            '$01 部品庫B／O管理に向けた評価用アプリ作成 START
                            'サービスがエラーのときは全件移送する
                            tempArea03Data = GetAllWaitingforPartsIssuingListData(nowDate, dispArea03Data)
                            '$01 部品庫B／O管理に向けた評価用アプリ作成 END
                        End If
                    Else
                        'Errorログ
                        '$04 TR-SVT-TMT-20161229-001 部品庫モニターでエラーを検知 START
                        'Logger.Error(String.Format(CultureInfo.CurrentCulture _
                        '    , "{0}.{1} <ResultCode> Tag is Not Found." _
                        '    , Me.GetType.ToString _
                        '    , System.Reflection.MethodBase.GetCurrentMethod.Name _
                        '    ))
                        '$04 TR-SVT-TMT-20161229-001 部品庫モニターでエラーを検知 END

                        '$01 部品庫B／O管理に向けた評価用アプリ作成 START
                        'ResultCodeタグが取得できなかったときは全件移送する
                        'サービスがエラーのときは全件移送する
                        tempArea03Data = GetAllWaitingforPartsIssuingListData(nowDate, dispArea03Data)
                        '$01 部品庫B／O管理に向けた評価用アプリ作成 END

                    End If
                Else
                    'Errorログ
                    '$04 TR-SVT-TMT-20161229-001 部品庫モニターでエラーを検知 START
                    'Logger.Error(String.Format(CultureInfo.CurrentCulture _
                    '    , "{0}.{1} retXML is Blank." _
                    '    , Me.GetType.ToString _
                    '    , System.Reflection.MethodBase.GetCurrentMethod.Name _
                    '    ))
                    '$04 TR-SVT-TMT-20161229-001 部品庫モニターでエラーを検知 END

                    '$01 部品庫B／O管理に向けた評価用アプリ作成 START
                    'retXMLがブランクだったときは全件移送する
                    tempArea03Data = GetAllWaitingforPartsIssuingListData(nowDate, dispArea03Data)
                    '$01 部品庫B／O管理に向けた評価用アプリ作成 END
                End If

                '$01 部品庫B／O管理に向けた評価用アプリ作成 START
                '$04 TR-SVT-TMT-20161229-001 部品庫モニターでエラーを検知 START
                ''かごの設定
                'tempArea03CageNoSetData = SetCageNo(StaffContext.Current.DlrCD, StaffContext.Current.BrnCD, _
                '                                    StaffContext.Current.Account, nowDate, tempArea03Data, AreaType_03)

                'dv = New DataView(tempArea03CageNoSetData.Copy)
                dv = New DataView(tempArea03Data.Copy)
                '$04 TR-SVT-TMT-20161229-001 部品庫モニターでエラーを検知 END
                '$01 部品庫B／O管理に向けた評価用アプリ作成 END

                'データビューでソートする
                dv.Sort = "SORT_KEY, SCHE_START_DATETIME, RO_NUM, RO_SEQ"

                '元DataTableをクリア
                dispArea03Data.Clear()
                ' ソート結果を一行ずつ格納
                Dim cnt As Integer = 0
                For Each drv As System.Data.DataRowView In dv
                    If cnt >= chipAcquisitionMaxCount Then
                        '表示最大数に達したらループを抜ける
                        Exit For
                    Else
                        ' DataRowとしてimportする
                        dispArea03Data.ImportRow(drv.Row)
                        cnt = cnt + 1
                    End If
                Next
            End If

            '画面に表示する件数を出力(抽出件数-NG件数のはず)
            '$04 TR-SVT-TMT-20161229-001 部品庫モニターでエラーを検知 START
            'Logger.Info("DEBUG:Return SelectCount=" & selectDataCount.ToString)
            '$04 TR-SVT-TMT-20161229-001 部品庫モニターでエラーを検知 END

            '終了ログ
            '$04 TR-SVT-TMT-20161229-001 部品庫モニターでエラーを検知 START
            'Logger.Error(String.Format(CultureInfo.CurrentCulture _
            '   , "{0}.{1} {2} QUERY:COUNT = {3}" _
            '   , Me.GetType.ToString _
            '   , System.Reflection.MethodBase.GetCurrentMethod.Name _
            '   , ConsLogEnd _
            '   , dispArea03Data.Rows.Count))
            '$04 TR-SVT-TMT-20161229-001 部品庫モニターでエラーを検知 END

            Return dispArea03Data

        Catch ex As Exception
            'エラー時
            Logger.Error(String.Format(CultureInfo.CurrentCulture _
                         , "{0}.{1} OUT:ErrGetWaitingforPartsIssuingListData(ex) = {2}" _
                         , Me.GetType.ToString _
                         , System.Reflection.MethodBase.GetCurrentMethod.Name _
                         , ex.Message))

            'エラー時には0件データを返す
            dispArea03Data = New SC3190402DataSet.AREA03DataTable
            Return dispArea03Data

        Finally
            'オブジェクトの解放
            If Not IsNothing(dispArea03Data) Then
                dispArea03Data.Dispose()
            End If
            '$01 部品庫B／O管理に向けた評価用アプリ作成 START
            If Not IsNothing(tempArea03Data) Then
                tempArea03Data.Dispose()
            End If
            '$04 TR-SVT-TMT-20161229-001 部品庫モニターでエラーを検知 START
            'If Not IsNothing(tempArea03CageNoSetData) Then
            '    tempArea03CageNoSetData.Dispose()
            'End If
            '$04 TR-SVT-TMT-20161229-001 部品庫モニターでエラーを検知 END
            '$01 部品庫B／O管理に向けた評価用アプリ作成 END
        End Try

    End Function

    '2014/07/15 改ページ機能追加により
    '           引数「MAX表示件数(chipsDispMaxCount)」を
    '               「MAX取得件数(chipAcquisitionMaxCount)」に変更

    '$04 TR-SVT-TMT-20161229-001 部品庫モニターでエラーを検知 START
    ' ''' <summary>
    ' ''' 引き取り待ちデータ取得
    ' ''' </summary>
    ' ''' <param name="nowDate">現在日時</param>
    ' ''' <param name="chipAcquisitionMaxCount">MAX取得件数</param>
    ' ''' <returns>データテーブル</returns>
    '<EnableCommit()> _
    'Public Function GetWaitingforTechnicianPickupListData(ByVal nowDate As Date, _
    '                                                  ByVal chipAcquisitionMaxCount As Integer, _
    '                                                  ByRef selectDataCount As Integer _
    '                                                  ) As SC3190402DataSet.AREA04ResDataTable _
    'Implements ISC3190402BusinessLogic.GetWaitingforTechnicianPickupListData

    ''' <summary>
    ''' 引き取り待ちデータ取得
    ''' </summary>
    ''' <param name="nowDate">現在日時</param>
    ''' <param name="chipAcquisitionMaxCount">MAX取得件数</param>
    ''' <returns>データテーブル</returns>
    Public Function GetWaitingforTechnicianPickupListData(ByVal nowDate As Date, _
                                                          ByVal chipAcquisitionMaxCount As Integer, _
                                                          ByRef selectDataCount As Integer _
                                                          ) As SC3190402DataSet.AREA04ResDataTable
        '$04 TR-SVT-TMT-20161229-001 部品庫モニターでエラーを検知 END

        Dim dispArea04ResData As SC3190402DataSet.AREA04ResDataTable = Nothing
        Dim tempArea04ResData As SC3190402DataSet.AREA04ResDataTable = Nothing '一時Work
        '$04 TR-SVT-TMT-20161229-001 部品庫モニターでエラーを検知 START
        'Dim tempArea04CageNoSetData As SC3190402DataSet.AREA04ResDataTable = Nothing
        '$04 TR-SVT-TMT-20161229-001 部品庫モニターでエラーを検知 END
        Try
            '開始ログ
            '$04 TR-SVT-TMT-20161229-001 部品庫モニターでエラーを検知 START
            'Logger.Error(String.Format(CultureInfo.CurrentCulture _
            '   , "{0}.{1} {2} P1:{3} P2:{4}" _
            '   , Me.GetType.ToString _
            '   , System.Reflection.MethodBase.GetCurrentMethod.Name _
            '   , ConsLogStart _
            '   , nowDate.ToString _
            '   , chipAcquisitionMaxCount.ToString _
            '   ))
            '$04 TR-SVT-TMT-20161229-001 部品庫モニターでエラーを検知 END

            '表示対象データの取得
            dispArea04ResData = New SC3190402DataSet.AREA04ResDataTable
            '引き取り待ちデータ取得及びDictionary作成
            Using dispArea04Data As SC3190402DataSet.AREA04DataTable = _
                    SC3190402DataSet.GetWaitingforTechnicianPickupList( _
                                StaffContext.Current.DlrCD, _
                                StaffContext.Current.BrnCD, _
                                {ConsRoStatus_50}, _
                                {ConsStallUseStatus_01})

                '返却用の該当件数をセットする(ここからNG分を差し引いていく)
                selectDataCount = dispArea04Data.Rows.Count
                '$04 TR-SVT-TMT-20161229-001 部品庫モニターでエラーを検知 START
                'Logger.Error("DEBUG:dispArea04Data.Rows.Count=" & dispArea04Data.Rows.Count.ToString)
                '$04 TR-SVT-TMT-20161229-001 部品庫モニターでエラーを検知 END
                'データがあるか？
                If dispArea04Data.Rows.Count > 0 Then
                    Dim sbRoNum As StringBuilder = New StringBuilder 'サービス用RO番号
                    Dim sbRoSeq As StringBuilder = New StringBuilder 'サービス用RO連番
                    Dim dic As New Dictionary(Of String, String)     'キーと値の型を引数で指定する
                    Dim dicKey As String = String.Empty
                    Dim dicSv As New Dictionary(Of String, String()) 'キーと値の型を引数で指定する
                    Dim dicSvKey As String = String.Empty

                    'サービスに引き渡すためRO番号及びRO連番を配列に渡す
                    Dim listRoNum As New List(Of String)
                    Dim listRoSeq As New List(Of String)
                    For Each row As SC3190402DataSet.AREA04Row In dispArea04Data.Rows
                        listRoNum.Add(row.RO_NUM.ToString.TrimEnd)
                        listRoSeq.Add(row.RO_SEQ.ToString.TrimEnd)
                        'Dictionaryに要素を追加する
                        dicKey = MakeDictionaryKey(row.RO_NUM.ToString.TrimEnd, row.RO_SEQ.ToString.TrimEnd)
                        Dim dicValue As String = MakeDictionaryValueByDataRow(row)
                        dic.Add(dicKey, dicValue)
                    Next

                    '部品ステータス情報取得用
                    Dim retXML As String = String.Empty
                    '実行カウント
                    Dim RetryCount As Integer = 1
                    '設定回数だけリトライを行う
                    Do Until RetryCount > ConsRetryWebServiceMaxCount
                        '部品ステータス情報取得
                        retXML = GetPartsList(StaffContext.Current.DlrCD, _
                                              StaffContext.Current.BrnCD, _
                                              listRoNum.ToArray, _
                                              listRoSeq.ToArray)
                        '取得結果がブランク(エラー)だったらリトライする
                        If retXML.TrimEnd.Length > 0 Then
                            Exit Do
                        Else
                            '$04 TR-SVT-TMT-20161229-001 部品庫モニターでエラーを検知 START
                            'Logger.Error(String.Format("DEBUG:Call GetPartsList {0}回目 NG!", RetryCount.ToString))
                            '$04 TR-SVT-TMT-20161229-001 部品庫モニターでエラーを検知 END
                            RetryCount = RetryCount + 1
                        End If
                    Loop

                    '並び替えを行うためにDataViewを用意
                    Dim dv As DataView = Nothing
                    If retXML.TrimEnd.Length > 0 Then
                        '取得XMLの解析
                        Dim xml As Xml.XmlDocument = New Xml.XmlDocument
                        xml.LoadXml(retXML)
                        'ResultCodeタグの検索結果をセットする
                        Dim retXmlNode As Xml.XmlNode = xml.SelectSingleNode(NodeResult)
                        If Not IsNothing(retXmlNode) Then
                            'タグが取得できた
                            Dim retCD As String = retXmlNode.InnerText
                            '正常終了か？
                            If retCD = IC3190402BusinessLogic.ResultSuccess Then
                                dicKey = ""
                                '<PARTS_STATUS>の数だけループする
                                Dim nodes As Xml.XmlNodeList = xml.SelectNodes(NodePARTS_STATUS)
                                For i As Integer = 0 To nodes.Count - 1
                                    'RO番号、RO連番をもとにDictionaryキーを作成する
                                    Dim nd As Xml.XmlNode = nodes(i)
                                    Dim nodes_bill As Xml.XmlNodeList = nd.SelectNodes(NodeBILL)

                                    Dim sbBuff As StringBuilder = New StringBuilder
                                    Dim strBuff As String = String.Empty

                                    'サービス情報保存用のキーを作成する
                                    dicSvKey = MakeDictionaryKey(nd.SelectSingleNode(TagR_O).InnerText, _
                                                                 nd.SelectSingleNode(TagR_O_SEQNO).InnerText)

                                    ' $02 Start サービスタブレットDMS連携追加開発(部品庫モニター在庫無し表示)
                                    'ステータスをチェック
                                    Select Case nd.SelectSingleNode(TagPARTS_ISSUE_STATUS).InnerText.TrimEnd
                                        Case ConsPartsIssueStatus_AllPartsIssuedCompletely, _
                                             ConsPartsIssueStatus_Partsissuing, _
                                             ConsPartsIssueStatus_NoStock

                                            '全数出庫or部分出庫or在庫無し
                                            'BILLタグは存在するか？
                                            If nd.SelectSingleNode(TagPARTS_ISSUE_STATUS).InnerText.TrimEnd.Equals(ConsPartsIssueStatus_AllPartsIssuedCompletely) AndAlso _
                                               nodes_bill.Count = 0 Then
                                                '全出庫かつBILLタグなし＝部品なしとみなす＝対象外
                                                '部品ステータス情報をDictionaryへ保存する
                                                'BILLタグがないことを判別させるためValueにはブランクをセットする)
                                                dicSv.Add(dicSvKey, {""})
                                                '$04 TR-SVT-TMT-20161229-001 部品庫モニターでエラーを検知 START
                                                'Logger.Info("DEBUG:NG:部品無し:dicSvKey=" & dicSvKey)
                                                '$04 TR-SVT-TMT-20161229-001 部品庫モニターでエラーを検知 END
                                                Continue For
                                            Else
                                                If nodes_bill.Count = 0 Then
                                                    'BILLタグ無し(データ不整合でない限りこのケースは無いはず)
                                                    '$04 TR-SVT-TMT-20161229-001 部品庫モニターでエラーを検知 START
                                                    'Logger.Info("DEBUG:NG:部分出庫or全数出庫・BILLタグ無し:dicSvKey=" & dicSvKey)
                                                    '$04 TR-SVT-TMT-20161229-001 部品庫モニターでエラーを検知 END
                                                    Continue For
                                                Else
                                                    '<BILL>の数だけループする
                                                    Dim list As New List(Of String)
                                                    For j As Integer = 0 To nodes_bill.Count - 1
                                                        'BILLタグ以下の情報取得
                                                        Dim nd_bill As Xml.XmlNode = nodes_bill(j)
                                                        'BILL情報を配列に格納するため、内容をlistに追加していく
                                                        strBuff = MakeDictionaryValueByXmlNode(nd.SelectSingleNode(TagPARTS_ISSUE_STATUS).InnerText.TrimEnd, nd_bill)
                                                        list.Add(strBuff)
                                                    Next
                                                    '部品ステータス情報をDictionaryへ保存する
                                                    dicSv.Add(dicSvKey, list.ToArray)
                                                End If

                                            End If

                                        Case ConsPartsIssueStatus_NoIssue

                                            '出庫無しは対象外(部品ステータスのみ書き込む)
                                            '※ROの戻り無しと区別するためこの処理を行う
                                            '$04 TR-SVT-TMT-20161229-001 部品庫モニターでエラーを検知 START
                                            'Logger.Info("DEBUG:NG:出庫無し:dicSvKey=" & dicSvKey)
                                            '$04 TR-SVT-TMT-20161229-001 部品庫モニターでエラーを検知 END
                                            dicSv.Add(dicSvKey, {nd.SelectSingleNode(TagPARTS_ISSUE_STATUS).InnerText.TrimEnd})

                                    End Select
                                    ' $02 End   サービスタブレットDMS連携追加開発(部品庫モニター在庫無し表示)
                                Next
                                'e-CRBの取得結果とサービスの返却内容を突き合わせる
                                tempArea04ResData = New SC3190402DataSet.AREA04ResDataTable
                                For Each dicItem As KeyValuePair(Of String, String) In dic
                                    'サービス側に該当ROデータが存在するかチェック
                                    If dicSv.ContainsKey(dicItem.Key) = True Then
                                        'サービス側にRO情報が存在、部品無し以外でBILLタグ有りのとき出力する
                                        For i As Integer = 0 To dicSv(dicItem.Key).Length - 1
                                            Dim OkFlg As Boolean = False '出力対象かチェックするためのフラグ
                                            Dim itemSv() As String = dicSv(dicItem.Key)(i).Split(ControlChars.Tab)
                                            If itemSv(Enum_AreaSVItemIndex.PARTS_ISSUE_STATUS).TrimEnd.Length = 0 Then
                                                '部品ステータスがブランク＝BILLタグなしだったら出力しない
                                                '$04 TR-SVT-TMT-20161229-001 部品庫モニターでエラーを検知 START
                                                'Logger.Info("DEBUG:NG:部品無し:dicItem.Key=" & dicItem.Key)
                                                '$04 TR-SVT-TMT-20161229-001 部品庫モニターでエラーを検知 END
                                            Else
                                                'ステータスをチェック
                                                Select Case itemSv(Enum_AreaSVItemIndex.PARTS_ISSUE_STATUS).TrimEnd
                                                    Case ConsPartsIssueStatus_AllPartsIssuedCompletely, _
                                                         ConsPartsIssueStatus_Partsissuing, _
                                                         ConsPartsIssueStatus_NoStock

                                                        '全数出庫or部分出庫or在庫無し→対象
                                                        '$04 TR-SVT-TMT-20161229-001 部品庫モニターでエラーを検知 START
                                                        'Logger.Info("DEBUG:OK:全数出庫or部分出庫:dicItem.Key=" & dicItem.Key)
                                                        '$04 TR-SVT-TMT-20161229-001 部品庫モニターでエラーを検知 END
                                                        Dim newRow As SC3190402DataSet.AREA04ResRow = _
                                                        Me.MakeWaitingforTechnicianPickupData(nowDate, dic.Item(dicItem.Key), tempArea04ResData, dicSv(dicItem.Key)(i))
                                                        tempArea04ResData.Rows.Add(newRow)
                                                        OkFlg = True 'フラグをOKにする

                                                    Case ConsPartsIssueStatus_NoIssue

                                                        '出庫無し→対象外
                                                        '$04 TR-SVT-TMT-20161229-001 部品庫モニターでエラーを検知 START
                                                        'Logger.Info("DEBUG:NG:出庫無しor在庫無し:dicItem.Key=" & dicItem.Key)
                                                        '$04 TR-SVT-TMT-20161229-001 部品庫モニターでエラーを検知 END
                                                End Select
                                            End If
                                            '表示件数の調整
                                            If OkFlg = True Then
                                                If i > 0 Then
                                                    'BILLNOが複数ある場合、表示件数を1増やす
                                                    selectDataCount = selectDataCount + 1
                                                    '$04 TR-SVT-TMT-20161229-001 部品庫モニターでエラーを検知 START
                                                    'Logger.Info("DEBUG:OK:COUNT UP!")
                                                    '$04 TR-SVT-TMT-20161229-001 部品庫モニターでエラーを検知 END
                                                End If
                                            Else
                                                If i = 0 Then
                                                    '1件目かつNGの場合は表示件数を1減らす
                                                    selectDataCount = selectDataCount - 1
                                                    '$04 TR-SVT-TMT-20161229-001 部品庫モニターでエラーを検知 START
                                                    'Logger.Info("DEBUG:OK:COUNT DOWN!")
                                                    '$04 TR-SVT-TMT-20161229-001 部品庫モニターでエラーを検知 END
                                                End If
                                            End If
                                        Next

                                    Else
                                        'サービス側にRO情報が存在しないときも出力する
                                        '(サービス項目はブランクとする)
                                        Dim newRow As SC3190402DataSet.AREA04ResRow = _
                                            Me.MakeWaitingforTechnicianPickupData(nowDate, dic.Item(dicItem.Key), tempArea04ResData)
                                        tempArea04ResData.Rows.Add(newRow)
                                        '$04 TR-SVT-TMT-20161229-001 部品庫モニターでエラーを検知 START
                                        'Logger.Info("DEBUG:OK:サービス側にRO情報無し or RO情報有りかつBILLタグ無し")
                                        '$04 TR-SVT-TMT-20161229-001 部品庫モニターでエラーを検知 END
                                    End If
                                Next
                            Else
                                'Errorログ
                                '$04 TR-SVT-TMT-20161229-001 部品庫モニターでエラーを検知 START
                                'Logger.Error(String.Format(CultureInfo.CurrentCulture _
                                '   , "{0}.{1} <ResultCode> Tag is Not Found." _
                                '   , Me.GetType.ToString _
                                '   , System.Reflection.MethodBase.GetCurrentMethod.Name _
                                '   ))
                                '$04 TR-SVT-TMT-20161229-001 部品庫モニターでエラーを検知 END

                                'サービスがエラーのときは全件移送する
                                '(サービス項目はブランクとする)
                                tempArea04ResData = GetAllWaitingforTechnicianPickupData(nowDate, dispArea04Data)
                            End If
                        Else
                            'Errorログ
                            '$04 TR-SVT-TMT-20161229-001 部品庫モニターでエラーを検知 START
                            'Logger.Error(String.Format(CultureInfo.CurrentCulture _
                            '   , "{0}.{1} <ResultCode> Tag Value is Not Successful." _
                            '   , Me.GetType.ToString _
                            '   , System.Reflection.MethodBase.GetCurrentMethod.Name _
                            '   ))
                            '$04 TR-SVT-TMT-20161229-001 部品庫モニターでエラーを検知 END

                            'サービスがエラーのときは全件移送する
                            '(サービス項目はブランクとする)
                            tempArea04ResData = GetAllWaitingforTechnicianPickupData(nowDate, dispArea04Data)
                        End If
                    Else
                        'Errorログ
                        '$04 TR-SVT-TMT-20161229-001 部品庫モニターでエラーを検知 START
                        'Logger.Error(String.Format(CultureInfo.CurrentCulture _
                        '    , "{0}.{1} retXML is Blank." _
                        '    , Me.GetType.ToString _
                        '    , System.Reflection.MethodBase.GetCurrentMethod.Name _
                        '    ))
                        '$04 TR-SVT-TMT-20161229-001 部品庫モニターでエラーを検知 END

                        'Service呼び出しの結果ブランクが返ってきたときは全件移送する
                        '(サービス項目はブランクとする)
                        tempArea04ResData = GetAllWaitingforTechnicianPickupData(nowDate, dispArea04Data)
                    End If

                    '$04 TR-SVT-TMT-20161229-001 部品庫モニターでエラーを検知 START
                    ''$01 部品庫B／O管理に向けた評価用アプリ作成 START
                    'tempArea04CageNoSetData = SetCageNo(StaffContext.Current.DlrCD, StaffContext.Current.BrnCD, _
                    '                                    StaffContext.Current.Account, nowDate, tempArea04ResData, AreaType_04)
                    ''$01 部品庫B／O管理に向けた評価用アプリ作成 END
                    '$04 TR-SVT-TMT-20161229-001 部品庫モニターでエラーを検知 END

                    'データビューにコピーする
                    '$04 TR-SVT-TMT-20161229-001 部品庫モニターでエラーを検知 START
                    'dv = New DataView(tempArea04CageNoSetData.Copy)
                    dv = New DataView(tempArea04ResData.Copy)
                    '$04 TR-SVT-TMT-20161229-001 部品庫モニターでエラーを検知 END

                    dv.Sort = "SORT_KEY, SCHE_DELI_DATETIME, RO_NUM, RO_SEQ"
                    ' ソート結果を一行ずつ格納
                    Dim cnt As Integer = 0
                    For Each drv As System.Data.DataRowView In dv
                        If cnt >= chipAcquisitionMaxCount Then
                            '表示最大数に達したらループを抜ける
                            Exit For
                        Else
                            ' DataRowとしてimportする
                            dispArea04ResData.ImportRow(drv.Row)
                            cnt = cnt + 1
                        End If
                    Next
                End If
            End Using

            '画面に表示する件数を出力(抽出件数-NG件数のはず)
            '$04 TR-SVT-TMT-20161229-001 部品庫モニターでエラーを検知 START
            'Logger.Info("DEBUG:Return SelectCount=" & selectDataCount.ToString)
            '$04 TR-SVT-TMT-20161229-001 部品庫モニターでエラーを検知 END

            '終了ログ
            '$04 TR-SVT-TMT-20161229-001 部品庫モニターでエラーを検知 START
            'Logger.Error(String.Format(CultureInfo.CurrentCulture _
            '   , "{0}.{1} {2} QUERY:COUNT = {3}" _
            '   , Me.GetType.ToString _
            '   , System.Reflection.MethodBase.GetCurrentMethod.Name _
            '   , ConsLogEnd _
            '   , dispArea04ResData.Rows.Count))
            '$04 TR-SVT-TMT-20161229-001 部品庫モニターでエラーを検知 END

            Return dispArea04ResData

        Catch ex As Exception
            'エラー時
            Logger.Error(String.Format(CultureInfo.CurrentCulture _
                         , "{0}.{1} OUT:ErrGetWaitingforTechnicianPickupListData(ex) = {2}" _
                         , Me.GetType.ToString _
                         , System.Reflection.MethodBase.GetCurrentMethod.Name _
                         , ex.Message))

            'エラー時には0件データを返す
            dispArea04ResData = New SC3190402DataSet.AREA04ResDataTable
            Return dispArea04ResData

        Finally
            'オブジェクトの解放
            If Not IsNothing(dispArea04ResData) Then
                dispArea04ResData.Dispose()
            End If
            '$01 部品庫B／O管理に向けた評価用アプリ作成 START
            If Not IsNothing(tempArea04ResData) Then
                tempArea04ResData.Dispose()
            End If
            '$04 TR-SVT-TMT-20161229-001 部品庫モニターでエラーを検知 START
            'If Not IsNothing(tempArea04CageNoSetData) Then
            '    tempArea04CageNoSetData.Dispose()
            'End If
            '$04 TR-SVT-TMT-20161229-001 部品庫モニターでエラーを検知 END
            '$01 部品庫B／O管理に向けた評価用アプリ作成 END
        End Try

    End Function

    '$04 TR-SVT-TMT-20161229-001 部品庫モニターでエラーを検知 START
    ''' <summary>
    ''' かご情報更新
    ''' </summary>
    ''' <param name="dealerCode">販売店コード</param>
    ''' <param name="branchCode">店舗コード</param>
    ''' <param name="nowDate">現在日付</param>
    ''' <param name="account">アカウント</param>
    ''' <param name="area03Data">出庫待ち表示対象データセット</param>
    ''' <param name="area04Data">引き取り待ち表示対象データセット</param>
    ''' <remarks></remarks>
    <EnableCommit()> _
    Public Sub UpdateCageInfo(ByVal dealerCode As String, _
                                        ByVal branchCode As String, _
                                        ByVal nowDate As Date, _
                                        ByVal account As String, _
                                        ByRef area03Data As SC3190402DataSet.AREA03DataTable, _
                                        ByRef area04Data As SC3190402DataSet.AREA04ResDataTable) _
        Implements ISC3190402BusinessLogic.UpdateCageInfo

        'ロック取得フラグ
        Dim hasLock As Boolean = True
        'かご件数
        Dim cageCount As Integer = 0

        Try
            'かご件数取得
            cageCount = SC3190402DataSet.GetCageCount(dealerCode, branchCode)

            'かごが0件の場合、かご解放、かご番号設定を行わない
            If cageCount = 0 Then
                Return
            End If

        Catch ex As OracleExceptionEx
            If ex.Number = OracleErrorResourceBusy Then
                'ロック取得の待機時間を超過した場合、ロック取得フラグをFalseに設定する
                hasLock = False
            Else
                'ロック取得待機時間超過以外のエラーはそのまま送出
                Throw
            End If
        End Try

        'ロック取得できた場合
        If hasLock Then
            'かご解放
            ReleaseCage(dealerCode, branchCode, nowDate, account)
        End If

        '出庫待ち表示対象データが存在する場合
        If area04Data.Rows.Count > 0 Then
            'かご番号設定（引き取り待ちデータ）
            area04Data = SetCageNo(dealerCode, branchCode, account, nowDate, area04Data, AreaType_04, hasLock)
        End If

        '引き取り待ち表示対象データが存在する場合
        If area03Data.Rows.Count > 0 Then
            'かご番号設定（出庫待ちデータ）
            area03Data = SetCageNo(dealerCode, branchCode, account, nowDate, area03Data, AreaType_03, hasLock)
        End If

    End Sub
    '$04 TR-SVT-TMT-20161229-001 部品庫モニターでエラーを検知 START

    ''' <summary>
    ''' 作業計画待ちデータを全件移送する
    ''' </summary>
    ''' <param name="nowDate">日付</param>
    ''' <param name="dt">移送元データ</param>
    Private Function GetAllWaitingforJobPlanningListData(ByVal nowDate As Date, ByRef dt As SC3190402DataSet.AREA02DataTable) As SC3190402DataSet.AREA02DataTable
        For Each row As SC3190402DataSet.AREA02Row In dt.Rows
            '赤明細のチェック
            '現在日時＞納車予定時刻時刻だったら赤明細とみなし、フラグを立てる
            row.SORT_KEY = CheckDelayOfDate(nowDate, row.SCHE_DELI_DATETIME)
        Next
        Return dt
    End Function

    ''' <summary>
    ''' 出庫待ちデータを全件移送する
    ''' </summary>
    ''' <param name="nowDate">日付</param>
    ''' <param name="dt">移送元データ</param>
    Private Function GetAllWaitingforPartsIssuingListData(ByVal nowDate As Date, ByRef dt As SC3190402DataSet.AREA03DataTable) As SC3190402DataSet.AREA03DataTable
        For Each row As SC3190402DataSet.AREA03Row In dt.Rows
            '赤明細のチェック
            '現在日時＞作業開始予定時刻だったら赤明細とみなし、フラグを立てる
            If row.IsSCHE_START_DATETIMENull Then
                row.SORT_KEY = Enum_BackColor.Normal
            Else
                row.SORT_KEY = CheckDelayOfDate(nowDate, row.SCHE_START_DATETIME)
            End If
        Next
        Return dt
    End Function

    ''' <summary>
    ''' 引き取り待ちデータを全件移送する
    ''' </summary>
    ''' <param name="nowDate">日付</param>
    ''' <param name="dt">移送元データ</param>
    Private Function GetAllWaitingforTechnicianPickupData(ByVal nowDate As Date, ByRef dt As SC3190402DataSet.AREA04DataTable) As SC3190402DataSet.AREA04ResDataTable
        Using dtRet As SC3190402DataSet.AREA04ResDataTable = New SC3190402DataSet.AREA04ResDataTable
            For Each row As SC3190402DataSet.AREA04Row In dt.Rows
                Dim newRow As SC3190402DataSet.AREA04ResRow = _
                    MakeWaitingforTechnicianPickupDataByRow(nowDate, row, dtRet)
                dtRet.Rows.Add(newRow)
            Next
            Return dtRet
        End Using
    End Function

    ''' <summary>
    ''' 現在時刻取得
    ''' </summary>
    ''' <returns>現在時刻</returns>
    Public Function GetDateTimeNow() As Date
        '$04 TR-SVT-TMT-20161229-001 部品庫モニターでエラーを検知 START
        ''開始ログ
        'Logger.Info(String.Format(CultureInfo.CurrentCulture _
        '   , "{0}.{1} {2}" _
        '   , Me.GetType.ToString _
        '   , System.Reflection.MethodBase.GetCurrentMethod.Name _
        '   , ConsLogStart))
        '$04 TR-SVT-TMT-20161229-001 部品庫モニターでエラーを検知 END

        Dim nowDate As Date = DateTimeFunc.Now(StaffContext.Current.DlrCD, StaffContext.Current.BrnCD)

        '$04 TR-SVT-TMT-20161229-001 部品庫モニターでエラーを検知 START
        ''終了ログ
        'Logger.Info(String.Format(CultureInfo.CurrentCulture _
        '   , "{0}.{1} {2} OUT:retValue = {3}" _
        '   , Me.GetType.ToString _
        '   , System.Reflection.MethodBase.GetCurrentMethod.Name _
        '   , ConsLogEnd _
        '   , nowDate.ToString))
        '$04 TR-SVT-TMT-20161229-001 部品庫モニターでエラーを検知 END

        Return nowDate
    End Function

    ''' <summary>
    ''' RO番号編集
    ''' </summary>
    ''' <param name="roNum">RO番号</param>
    ''' <param name="roSeq">RO連番</param>
    ''' <returns>RO番号＋RO連番</returns>
    Public Shared Function MakeRoNumAndSeq(ByVal roNum As String, _
                                           ByVal roSeq As String) As String
        '$04 TR-SVT-TMT-20161229-001 部品庫モニターでエラーを検知 START
        ''開始ログ
        'Logger.Info(String.Format(CultureInfo.CurrentCulture _
        '   , "{0} {1} P1:{2} P2:{3}" _
        '   , System.Reflection.MethodBase.GetCurrentMethod.Name _
        '   , ConsLogStart _
        '   , roNum _
        '   , roSeq _
        '   ))
        '$04 TR-SVT-TMT-20161229-001 部品庫モニターでエラーを検知 END

        Dim ret As String = String.Empty
        'RO番号＋RO連番編集
        If roSeq = "0" Then
            'RO作業連番が0(親番)のとき追加作業チェックを行う
            Dim maxRoSeq As Integer = GetMaxRoSeqData(roNum)
            If maxRoSeq >= ConsAddRepair Then
                '該当RO番号のMAX連番が1以上のときRO連番付きとする
                ret = roNum & ConsHyphen & roSeq
            Else
                'RO連番1以上のデータが存在しないときRO連番はブランクとする
                ret = roNum
            End If
        Else
            'RO連番が1以上のときは連番付きとする
            ret = roNum & ConsHyphen & roSeq
        End If

        '$04 TR-SVT-TMT-20161229-001 部品庫モニターでエラーを検知 START
        ''終了ログ
        'Logger.Info(String.Format(CultureInfo.CurrentCulture _
        '   , "{0} {1} OUT:retValue = {2}" _
        '   , System.Reflection.MethodBase.GetCurrentMethod.Name _
        '   , ConsLogEnd _
        '   , ret))
        '$04 TR-SVT-TMT-20161229-001 部品庫モニターでエラーを検知 END

        Return ret
    End Function

    ''' <summary>
    ''' グレード名編集
    ''' </summary>
    ''' <param name="modelName">モデル名</param>
    ''' <param name="gradeName">グレード名</param>
    ''' <returns>モデル名＋グレード名</returns>
    ''' <remarks></remarks>
    Public Shared Function MakeModelAndGradeName(ByVal modelName As String, _
                                                 ByVal gradeName As String) As String
        '$04 TR-SVT-TMT-20161229-001 部品庫モニターでエラーを検知 START
        ''開始ログ
        'Logger.Info(String.Format(CultureInfo.CurrentCulture _
        '   , "{0} {1} P1:{2} P2:{3}" _
        '   , System.Reflection.MethodBase.GetCurrentMethod.Name _
        '   , ConsLogStart _
        '   , modelName _
        '   , gradeName _
        '   ))
        '$04 TR-SVT-TMT-20161229-001 部品庫モニターでエラーを検知 END

        Dim ret As String = Nothing
        'モデル名＋グレード名編集
        If gradeName.TrimEnd.Length = 0 Then
            'グレード名称がブランクのときスラッシュを省く
            ret = modelName
        Else
            'グレード名あり
            If modelName.TrimEnd.Length = 0 Then
                'モデル名称がブランクのときスラッシュを省く
                ret = gradeName
            Else
                ret = modelName & ConsSlash & gradeName
            End If
        End If

        '$04 TR-SVT-TMT-20161229-001 部品庫モニターでエラーを検知 START
        ''終了ログ
        'Logger.Info(String.Format(CultureInfo.CurrentCulture _
        '   , "{0} {1} OUT:retValue = {2}" _
        '   , System.Reflection.MethodBase.GetCurrentMethod.Name _
        '   , ConsLogEnd _
        '   , ret))
        '$04 TR-SVT-TMT-20161229-001 部品庫モニターでエラーを検知 END

        Return ret
    End Function


    ''' <summary>
    ''' Dictionaryキー作成処理(RO番号＋RO連番単位)
    ''' </summary>
    ''' <param name="roNum">RO番号</param>
    ''' <param name="roSeq">RO連番</param>
    ''' <returns>Dictionaryキー</returns>
    ''' <remarks>各項目20桁の前スペース埋めで連結する</remarks>
    Public Shared Function MakeDictionaryKey(ByVal roNum As String, _
                                             ByVal roSeq As String) As String
        '$04 TR-SVT-TMT-20161229-001 部品庫モニターでエラーを検知 START
        ''開始ログ
        'Logger.Info(String.Format(CultureInfo.CurrentCulture _
        '   , "{0} {1} P1:{2} P2:{3}" _
        '   , System.Reflection.MethodBase.GetCurrentMethod.Name _
        '   , ConsLogStart _
        '   , roNum _
        '   , roSeq _
        '   ))
        '$04 TR-SVT-TMT-20161229-001 部品庫モニターでエラーを検知 END

        Dim str As String = String.Empty
        str = String.Format("{0:20}" & ControlChars.Tab & _
                            "{1:20}", _
                            roNum.TrimEnd, _
                            roSeq.TrimEnd)
        '$04 TR-SVT-TMT-20161229-001 部品庫モニターでエラーを検知 START
        ''終了ログ
        'Logger.Info(String.Format(CultureInfo.CurrentCulture _
        '   , "{0} {1} OUT:retValue = {2}" _
        '   , System.Reflection.MethodBase.GetCurrentMethod.Name _
        '   , ConsLogEnd _
        '   , str))
        '$04 TR-SVT-TMT-20161229-001 部品庫モニターでエラーを検知 END

        Return str
    End Function

    '$04 TR-SVT-TMT-20161229-001 部品庫モニターでエラーを検知 START
    ''$01 部品庫B／O管理に向けた評価用アプリ作成 START
    ' ''' <summary>
    ' ''' かごの解放
    ' ''' </summary>
    ' ''' <param name="dealerCode">販売店コード</param>
    ' ''' <param name="branchCode">店舗コード</param>
    ' ''' <param name="nowDate">現在日付</param>
    ' ''' <param name="account">アカウント</param>
    '<EnableCommit()> _
    'Public Sub ReleaseCage(ByVal dealerCode As String, _
    '                                    ByVal branchCode As String, _
    '                                    ByVal nowDate As Date, _
    '                                    ByVal account As String) _
    '                                Implements ISC3190402BusinessLogic.ReleaseCage

    '$01 部品庫B／O管理に向けた評価用アプリ作成 START
    ''' <summary>
    ''' かごの解放
    ''' </summary>
    ''' <param name="dealerCode">販売店コード</param>
    ''' <param name="branchCode">店舗コード</param>
    ''' <param name="nowDate">現在日付</param>
    ''' <param name="account">アカウント</param>
    Public Sub ReleaseCage(ByVal dealerCode As String, _
                                        ByVal branchCode As String, _
                                        ByVal nowDate As Date, _
                                        ByVal account As String)
        '$04 TR-SVT-TMT-20161229-001 部品庫モニターでエラーを検知 END

        '$04 TR-SVT-TMT-20161229-001 部品庫モニターでエラーを検知 START
        ''開始ログ
        'Logger.Info(String.Format(CultureInfo.CurrentCulture _
        '   , "{0}.{1} {2} P1:{3} P2:{4}" _
        '   , Me.GetType.ToString _
        '   , System.Reflection.MethodBase.GetCurrentMethod.Name _
        '   , ConsLogStart _
        '   , dealerCode.ToString _
        '   , branchCode.ToString _
        '   ))

        ''かご件数取得
        'Dim cageCount As Integer = SC3190402DataSet.GetCageCount(dealerCode, branchCode)

        ''かごが0件の場合、引数で受け取ったデータテーブルをそのまま返す
        'If cageCount = 0 Then
        '    Return
        'End If
        '$04 TR-SVT-TMT-20161229-001 部品庫モニターでエラーを検知 END

        'かごの解放
        SC3190402DataSet.ReleaseCage(dealerCode, branchCode, _
                                     {ConsRoStatus_85, ConsRoStatus_90, _
                                      ConsRoStatus_99}, _
                                      nowDate, _
                                      account)

        '$04 TR-SVT-TMT-20161229-001 部品庫モニターでエラーを検知 START
        ''終了ログ
        'Logger.Info(String.Format(CultureInfo.CurrentCulture _
        '   , "{0}.{1} {2} QUERY" _
        '   , Me.GetType.ToString _
        '   , System.Reflection.MethodBase.GetCurrentMethod.Name _
        '   , ConsLogEnd))
        '$04 TR-SVT-TMT-20161229-001 部品庫モニターでエラーを検知 END


    End Sub
    '$01 部品庫B／O管理に向けた評価用アプリ作成 END

    '$03 DMS連携版サービスタブレット サービスアドバイザ情報連携追加開発 START
    ''' <summary>
    ''' 店舗営業時間取得
    ''' </summary>
    ''' <param name="dealerCode">販売店コード</param>
    ''' <param name="branchCode">店舗コード</param>
    Public Function GetBranchWorkTime(ByVal dealerCode As String, ByVal branchCode As String) As SC3190402DataSet.BranchWorkTimeDataTable

        '$04 TR-SVT-TMT-20161229-001 部品庫モニターでエラーを検知 START
        ''開始ログ
        'Logger.Info(String.Format(CultureInfo.CurrentCulture _
        '   , "{0}.{1} {2} P1:{3} P2:{4}" _
        '   , Me.GetType.ToString _
        '   , System.Reflection.MethodBase.GetCurrentMethod.Name _
        '   , ConsLogStart _
        '   , dealerCode _
        '   , branchCode _
        '   ))
        '$04 TR-SVT-TMT-20161229-001 部品庫モニターでエラーを検知 END

        Dim branchWorkTimeDataTable As SC3190402DataSet.BranchWorkTimeDataTable = _
            SC3190402DataSet.GetBranchWorkTime(dealerCode, branchCode)

        Dim returnBranchWorkTimeDataTable As SC3190402DataSet.BranchWorkTimeDataTable = New SC3190402DataSet.BranchWorkTimeDataTable
        Dim returnBranchWorkTimeRow As SC3190402DataSet.BranchWorkTimeRow = returnBranchWorkTimeDataTable.NewBranchWorkTimeRow

        Dim nowDate As Date = DateTimeFunc.Now(dealerCode)

        ' データが取得できない場合
        ' 又は開始～終了共にDB初期値の場合
        If branchWorkTimeDataTable Is Nothing OrElse _
           branchWorkTimeDataTable.Count = 0 OrElse _
           (DB_DEFAULT_VALUE_DATE = branchWorkTimeDataTable.Item(0).SVC_JOB_START_TIME AndAlso _
            DB_DEFAULT_VALUE_DATE = branchWorkTimeDataTable.Item(0).SVC_JOB_END_TIME) Then

            ' 営業開始～終了を当日の00:00:00～00:00:00に設定する。
            returnBranchWorkTimeRow.SVC_JOB_START_TIME = New Date(nowDate.Year, nowDate.Month, nowDate.Day, 0, 0, 0)
            returnBranchWorkTimeRow.SVC_JOB_END_TIME = New Date(nowDate.Year, nowDate.Month, nowDate.Day + 1, 0, 0, 0)

        ElseIf branchWorkTimeDataTable.Item(0).SVC_JOB_END_TIME > branchWorkTimeDataTable.Item(0).SVC_JOB_START_TIME Then

            ' 営業時間が日を跨いでいない場合
            returnBranchWorkTimeRow.SVC_JOB_START_TIME = _
                New Date(nowDate.Year, _
                         nowDate.Month, _
                         nowDate.Day, _
                         branchWorkTimeDataTable.Item(0).SVC_JOB_START_TIME.Hour, _
                         branchWorkTimeDataTable.Item(0).SVC_JOB_START_TIME.Minute, _
                         branchWorkTimeDataTable.Item(0).SVC_JOB_START_TIME.Second)
            returnBranchWorkTimeRow.SVC_JOB_END_TIME = _
                New Date(nowDate.Year, _
                         nowDate.Month, _
                         nowDate.Day, _
                         branchWorkTimeDataTable.Item(0).SVC_JOB_END_TIME.Hour, _
                         branchWorkTimeDataTable.Item(0).SVC_JOB_END_TIME.Minute, _
                         branchWorkTimeDataTable.Item(0).SVC_JOB_END_TIME.Second)
        Else

            ' 営業時間が日を跨いでいる場合
            Dim nowDateValue As Date = New Date(1900, 1, 1, nowDate.Hour, nowDate.Minute, nowDate.Second)
            If nowDateValue < branchWorkTimeDataTable.Item(0).SVC_JOB_END_TIME Then
                ' 現在時刻が営業終了時刻より前の場合
                returnBranchWorkTimeRow.SVC_JOB_START_TIME = _
                    New Date(nowDate.Year, _
                             nowDate.Month, _
                             nowDate.Day - 1, _
                             branchWorkTimeDataTable.Item(0).SVC_JOB_START_TIME.Hour, _
                             branchWorkTimeDataTable.Item(0).SVC_JOB_START_TIME.Minute, _
                             branchWorkTimeDataTable.Item(0).SVC_JOB_START_TIME.Second)

                returnBranchWorkTimeRow.SVC_JOB_END_TIME = _
                    New Date(nowDate.Year, _
                             nowDate.Month, _
                             nowDate.Day, _
                             branchWorkTimeDataTable.Item(0).SVC_JOB_END_TIME.Hour, _
                             branchWorkTimeDataTable.Item(0).SVC_JOB_END_TIME.Minute, _
                             branchWorkTimeDataTable.Item(0).SVC_JOB_END_TIME.Second)
            Else
                ' 現在時刻が営業終了時刻より後の場合
                returnBranchWorkTimeRow.SVC_JOB_START_TIME = _
                    New Date(nowDate.Year, _
                             nowDate.Month, _
                             nowDate.Day, _
                             branchWorkTimeDataTable.Item(0).SVC_JOB_START_TIME.Hour, _
                             branchWorkTimeDataTable.Item(0).SVC_JOB_START_TIME.Minute, _
                             branchWorkTimeDataTable.Item(0).SVC_JOB_START_TIME.Second)

                returnBranchWorkTimeRow.SVC_JOB_END_TIME = _
                    New Date(nowDate.Year, _
                             nowDate.Month, _
                             nowDate.Day + 1, _
                             branchWorkTimeDataTable.Item(0).SVC_JOB_END_TIME.Hour, _
                             branchWorkTimeDataTable.Item(0).SVC_JOB_END_TIME.Minute, _
                             branchWorkTimeDataTable.Item(0).SVC_JOB_END_TIME.Second)
            End If
        End If

        returnBranchWorkTimeDataTable.AddBranchWorkTimeRow(returnBranchWorkTimeRow)
        branchWorkTimeDataTable = Nothing

        '$04 TR-SVT-TMT-20161229-001 部品庫モニターでエラーを検知 START
        ''終了ログ
        'Logger.Info(String.Format(CultureInfo.CurrentCulture _
        '   , "{0}.{1} {2} QUERY Count {3}" _
        '   , Me.GetType.ToString _
        '   , System.Reflection.MethodBase.GetCurrentMethod.Name _
        '   , ConsLogEnd _
        '   , returnBranchWorkTimeDataTable.Count))
        '$04 TR-SVT-TMT-20161229-001 部品庫モニターでエラーを検知 END

        Return returnBranchWorkTimeDataTable

    End Function
    '$03 DMS連携版サービスタブレット サービスアドバイザ情報連携追加開発 END

#End Region

#Region "Protected処理"
    ''' <summary>
    ''' 部品ステータス取得処理（WebService）
    ''' </summary>
    ''' <param name="dealerCode">販売店コード</param>
    ''' <param name="branchCode">店舗コード</param>
    ''' <param name="roNum">RO番号</param>
    ''' <param name="roSeq">RO連番</param>
    Protected Function GetPartsList(ByVal dealerCode As String, _
                                  ByVal branchCode As String, _
                                  ByVal roNum() As String, _
                                  ByVal roSeq() As String) _
                                                As String

        Dim str As String = String.Empty 'サービスの取得結果
        Try
            '開始ログ
            '$04 TR-SVT-TMT-20161229-001 部品庫モニターでエラーを検知 START
            'Logger.Error(String.Format(CultureInfo.CurrentCulture _
            '   , "{0}.{1} {2} P1:{3} P2:{4} P3:{5} P4:{6}" _
            '   , Me.GetType.ToString _
            '   , System.Reflection.MethodBase.GetCurrentMethod.Name _
            '   , ConsLogStart _
            '   , dealerCode _
            '   , branchCode _
            '   , String.Join(ConsComma, roNum) _
            '   , String.Join(ConsComma, roSeq) _
            '   ))
            '$04 TR-SVT-TMT-20161229-001 部品庫モニターでエラーを検知 END

            '部品ステータス情報用DataTable
            Using biz As IC3190402BusinessLogic = New IC3190402BusinessLogic()
                '部品ステータス情報取得用パラメータ
                Dim searchXmlClass As IC3190402BusinessLogic.PartsSearchXmlDocumentClass = New IC3190402BusinessLogic.PartsSearchXmlDocumentClass
                '部品ステータス情報取得用パラメータ作成処理
                searchXmlClass = CreatePartsSearchXmlDocument(dealerCode, _
                                                                branchCode, _
                                                                roNum, _
                                                                roSeq)
                '部品ステータス情報取得
                str = biz.CallGetPartsSearchInfoWebService(searchXmlClass)
            End Using

        Catch ex As Exception
            'エラー発生時
            Logger.Error(String.Format(CultureInfo.CurrentCulture _
                         , "{0}.{1} OUT:ErrGetPartsList = {2}" _
                         , Me.GetType.ToString _
                         , System.Reflection.MethodBase.GetCurrentMethod.Name _
                         , ex.Message))
            str = ""

        Finally
            '終了ログ
            Logger.Info(String.Format(CultureInfo.CurrentCulture _
               , "{0}.{1} {2} OUT:retValue = {3}" _
               , Me.GetType.ToString _
               , System.Reflection.MethodBase.GetCurrentMethod.Name _
               , ConsLogEnd _
               , str))

        End Try
        Return str
    End Function

    ''' <summary>
    ''' 部品ステータス情報取得用パラメータ作成処理
    ''' </summary>
    ''' <param name="dealerCode">販売店コード</param>
    ''' <param name="branchCode">店舗CD</param>
    ''' <param name="roNum">RO番号</param>
    ''' <param name="roSeq">RO連番</param>
    ''' <returns>CustomerSearchXmlDocumentClass</returns>
    Protected Function CreatePartsSearchXmlDocument(ByVal dealerCode As String, _
                                                  ByVal branchCode As String, _
                                                  ByVal roNum() As String, _
                                                  ByVal roSeq() As String) _
                                                     As IC3190402BusinessLogic.PartsSearchXmlDocumentClass

        Dim searchXmlClass As IC3190402BusinessLogic.PartsSearchXmlDocumentClass = New IC3190402BusinessLogic.PartsSearchXmlDocumentClass
        Try
            '開始ログ
            '$04 TR-SVT-TMT-20161229-001 部品庫モニターでエラーを検知 START
            'Logger.Error(String.Format(CultureInfo.CurrentCulture _
            '   , "{0}.{1} {2} P1:{3} P2:{4} P3:{5} P4:{6}" _
            '   , Me.GetType.ToString _
            '   , System.Reflection.MethodBase.GetCurrentMethod.Name _
            '   , ConsLogStart _
            '   , dealerCode _
            '   , branchCode _
            '   , String.Join(ConsComma, roNum) _
            '   , String.Join(ConsComma, roSeq) _
            '   ))
            '$04 TR-SVT-TMT-20161229-001 部品庫モニターでエラーを検知 END

            'Detailタグ
            With searchXmlClass.Detail
                'Commonタグ配下
                '基幹コードへ変換処理
                Dim context As StaffContext = StaffContext.Current
                Using biz As IC3190402BusinessLogic = New IC3190402BusinessLogic
                    Dim rowDmsCodeMap As IC3190402DataSet.DmsCodeMapRow = biz.ChangeDmsCode(context)
                    'DMS販売店コードを設定
                    .Common.DealerCode = rowDmsCodeMap.CODE1
                    'DMS店舗コードを設定
                    .Common.BranchCode = rowDmsCodeMap.CODE2
                End Using
                'PartsSearchConditionタグ配下
                'RO情報をデータ数だけループして追加する
                Dim wk As IC3190402BusinessLogic.PartsSearchXmlDocumentClass.DetailTag.PartsSearchConditionTag = Nothing
                For i As Integer = 0 To roNum.Count - 1
                    wk = New IC3190402BusinessLogic.PartsSearchXmlDocumentClass.DetailTag.PartsSearchConditionTag
                    'R_O：RO番号
                    wk.R_O = roNum(i).ToString(CultureInfo.CurrentCulture)
                    'R_O_SEQNO：RO連番
                    wk.R_O_SEQNO = roSeq(i).ToString(CultureInfo.CurrentCulture)
                    .PartsSearchCondition.Add(wk)
                Next
            End With

        Catch ex As Exception
            'エラー発生時
            Logger.Error(String.Format(CultureInfo.CurrentCulture _
                         , "{0}.{1} OUT:ErrCreatePartsSearchXmlDocument = {2}" _
                         , Me.GetType.ToString _
                         , System.Reflection.MethodBase.GetCurrentMethod.Name _
                         , ex.Message))
            searchXmlClass = Nothing

        Finally
            '終了ログ
            '$04 TR-SVT-TMT-20161229-001 部品庫モニターでエラーを検知 START
            'Logger.Error(String.Format(CultureInfo.CurrentCulture _
            '   , "{0}.{1} {2} OUT:XML = {3}" _
            '   , Me.GetType.ToString _
            '   , System.Reflection.MethodBase.GetCurrentMethod.Name _
            '   , ConsLogEnd _
            '   , searchXmlClass.ToString))
            '$04 TR-SVT-TMT-20161229-001 部品庫モニターでエラーを検知 END
        End Try

        Return searchXmlClass

    End Function

    ''' <summary>
    ''' DictionaryのValue値作成処理
    ''' </summary>
    ''' <param name="row">行データ</param>
    ''' <remarks>Row項目をタブ区切りの文字列にして返す</remarks>
    Protected Function MakeDictionaryValueByDataRow(ByVal row As DataRow) As String
        '$04 TR-SVT-TMT-20161229-001 部品庫モニターでエラーを検知 START
        ''開始ログ
        'Logger.Info(String.Format(CultureInfo.CurrentCulture _
        '   , "{0}.{1} {2}" _
        '   , Me.GetType.ToString _
        '   , System.Reflection.MethodBase.GetCurrentMethod.Name _
        '   , ConsLogStart))
        '$04 TR-SVT-TMT-20161229-001 部品庫モニターでエラーを検知 END

        '各項目をセットして返す
        'カラムの数だけループする
        Dim list As New List(Of String)
        For i As Integer = 0 To row.Table.Columns.Count - 1
            list.Add(row(i).ToString.TrimEnd)
        Next
        Dim str = String.Join(ControlChars.Tab, list.ToArray)

        '$04 TR-SVT-TMT-20161229-001 部品庫モニターでエラーを検知 START
        ''終了ログ
        'Logger.Info(String.Format(CultureInfo.CurrentCulture _
        '   , "{0}.{1} {2} OUT:retValue = {3}" _
        '   , Me.GetType.ToString _
        '   , System.Reflection.MethodBase.GetCurrentMethod.Name _
        '   , ConsLogEnd _
        '   , str))
        '$04 TR-SVT-TMT-20161229-001 部品庫モニターでエラーを検知 END

        Return str
    End Function

    ''' <summary>
    ''' DictionaryのValue値作成処理
    ''' </summary>
    ''' <param name="partsIssueStatus">部品ステータス</param>
    ''' <param name="node">node</param>
    ''' <remarks>XmlNode項目をタブ区切りの文字列にして返す</remarks>
    Protected Function MakeDictionaryValueByXmlNode(ByVal partsIssueStatus As String, _
                                                    ByVal node As Xml.XmlNode) As String
        '$04 TR-SVT-TMT-20161229-001 部品庫モニターでエラーを検知 START
        ''開始ログ
        'Logger.Info(String.Format(CultureInfo.CurrentCulture _
        '   , "{0}.{1} {2} P1:{3}" _
        '   , Me.GetType.ToString _
        '   , System.Reflection.MethodBase.GetCurrentMethod.Name _
        '   , ConsLogStart _
        '   , partsIssueStatus _
        '   ))
        '$04 TR-SVT-TMT-20161229-001 部品庫モニターでエラーを検知 END

        '各項目をセットして返す
        Dim sb As StringBuilder = New StringBuilder
        '部品ステータス項目の連結
        '部品ステータスと出庫管理番号は必須項目なのでそのまま連結する
        sb.Append(partsIssueStatus).Append(ControlChars.Tab).Append(node.SelectSingleNode(TagBillNo).InnerText.TrimEnd)
        'スタッフ名(任意)
        If IsNothing(node.SelectSingleNode(TagPartsStaffName)) Then
            sb.Append(ControlChars.Tab).Append("")
        Else
            sb.Append(ControlChars.Tab).Append(node.SelectSingleNode(TagPartsStaffName).InnerText.TrimEnd)
        End If
        'カゴNo(任意)
        If IsNothing(node.SelectSingleNode(TagCageNO)) Then
            sb.Append(ControlChars.Tab).Append("")
        Else
            sb.Append(ControlChars.Tab).Append(node.SelectSingleNode(TagCageNO).InnerText.TrimEnd)
        End If

        '$04 TR-SVT-TMT-20161229-001 部品庫モニターでエラーを検知 START
        ''終了ログ
        'Logger.Info(String.Format(CultureInfo.CurrentCulture _
        '   , "{0}.{1} {2} OUT:retValue = {3}" _
        '   , Me.GetType.ToString _
        '   , System.Reflection.MethodBase.GetCurrentMethod.Name _
        '   , ConsLogEnd _
        '   , sb.ToString))
        '$04 TR-SVT-TMT-20161229-001 部品庫モニターでエラーを検知 END

        Return sb.ToString
    End Function

    ''' <summary>
    ''' 作業計画待ちデータ作成
    ''' </summary>
    ''' <param name="nowDate">現在日時</param>
    ''' <param name="itemValue">出力内容(タブ区切りで格納されている)</param>
    ''' <param name="dataTable">追加するDataTable</param>
    Protected Function MakeWatingforJobPlanningData(ByVal nowDate As Date, _
                                                    ByVal itemValue As String, _
                                                    ByVal dataTable As SC3190402DataSet.AREA02DataTable) _
                                                        As SC3190402DataSet.AREA02Row
        '$04 TR-SVT-TMT-20161229-001 部品庫モニターでエラーを検知 START
        ''開始ログ
        'Logger.Info(String.Format(CultureInfo.CurrentCulture _
        '   , "{0}.{1} {2} P1:{3} P2:{4}" _
        '   , Me.GetType.ToString _
        '   , System.Reflection.MethodBase.GetCurrentMethod.Name _
        '   , ConsLogStart _
        '   , nowDate _
        '   , itemValue _
        '   ))
        '$04 TR-SVT-TMT-20161229-001 部品庫モニターでエラーを検知 END

        'タブで区切って配列に格納後、データテーブルへセットする
        Dim item() As String = itemValue.Split(ControlChars.Tab)
        Dim row As SC3190402DataSet.AREA02Row = dataTable.NewAREA02Row

        row.RO_NUM = item(Enum_Area02DtColIndex.RO_NUM)
        row.RO_SEQ = item(Enum_Area02DtColIndex.RO_SEQ)
        row.REG_NUM = item(Enum_Area02DtColIndex.REG_NUM)
        row.MODEL_NAME = item(Enum_Area02DtColIndex.MODEL_NAME)
        row.SCHE_DELI_DATETIME = Date.Parse(item(Enum_Area02DtColIndex.SCHE_DELI_DATETIME))
        row.DLR_CD = item(Enum_Area02DtColIndex.DLR_CD)
        row.BRN_CD = item(Enum_Area02DtColIndex.BRN_CD)

        '赤明細のチェック
        '現在日時＞納車予定時刻時刻だったら赤明細とみなし、フラグを立てる
        row.SORT_KEY = CheckDelayOfDate(nowDate, row.SCHE_DELI_DATETIME)

        '$04 TR-SVT-TMT-20161229-001 部品庫モニターでエラーを検知 START
        ''終了ログ
        'Logger.Info(String.Format(CultureInfo.CurrentCulture _
        '   , "{0}.{1} {2}" _
        '   , Me.GetType.ToString _
        '   , System.Reflection.MethodBase.GetCurrentMethod.Name _
        '   , ConsLogEnd))
        '$04 TR-SVT-TMT-20161229-001 部品庫モニターでエラーを検知 END

        Return row
    End Function

    ''' <summary>
    ''' 出庫待ちデータ作成
    ''' </summary>
    ''' <param name="nowDate">現在日時</param>
    ''' <param name="itemValue">出力内容(タブ区切りで格納されている)</param>
    ''' <param name="dataTable">追加するDataTable</param>
    ''' <param name="itemSvValue"></param>
    ''' <returns>出庫待ちデータ</returns>
    Protected Function MakeWatingforPartsIssuingData(ByVal nowDate As Date, _
                                                    ByVal itemValue As String, _
                                                    ByVal dataTable As SC3190402DataSet.AREA03DataTable, _
                                                    Optional ByVal itemSvValue As String = "") _
                                                        As SC3190402DataSet.AREA03Row
        '$04 TR-SVT-TMT-20161229-001 部品庫モニターでエラーを検知 START
        ''開始ログ
        'Logger.Info(String.Format(CultureInfo.CurrentCulture _
        '   , "{0}.{1} {2} P1:{3} P2:{4}" _
        '   , Me.GetType.ToString _
        '   , System.Reflection.MethodBase.GetCurrentMethod.Name _
        '   , ConsLogStart _
        '   , nowDate _
        '   , itemValue _
        '   ))
        '$04 TR-SVT-TMT-20161229-001 部品庫モニターでエラーを検知 START

        'タブで区切って配列に格納後、データテーブルへセットする
        Dim item() As String = itemValue.Split(ControlChars.Tab)
        Dim row As SC3190402DataSet.AREA03Row = dataTable.NewAREA03Row

        row.RO_NUM = item(Enum_Area03DtColIndex.RO_NUM)
        row.RO_SEQ = item(Enum_Area03DtColIndex.RO_SEQ)
        row.REG_NUM = item(Enum_Area03DtColIndex.REG_NUM)
        row.MODEL_NAME = item(Enum_Area03DtColIndex.MODEL_NAME)
        row.GRADE_NAME = item(Enum_Area03DtColIndex.GRADE_NAME)
        row.STALL_NAME_SHORT = item(Enum_Area03DtColIndex.STALL_NAME_SHORT)
        row.DLR_CD = item(Enum_Area03DtColIndex.DLR_CD)
        row.BRN_CD = item(Enum_Area03DtColIndex.BRN_CD)

        '$01 部品庫B／O管理に向けた評価用アプリ作成 START
        '以下部品ステータス項目
        If itemSvValue.TrimEnd = "" Then
            row.BILL_NO = ""
            row.CAGE_NO = ""
        Else
            Dim itemSv() As String = itemSvValue.Split(ControlChars.Tab)
            If 1 < itemSv.Length Then
                row.BILL_NO = itemSv(Enum_AreaSVItemIndex.BILL_NO)
                row.CAGE_NO = itemSv(Enum_AreaSVItemIndex.CAGE_NO)
            Else
                row.BILL_NO = ""
                row.CAGE_NO = ""
            End If
        End If
        '$01 部品庫B／O管理に向けた評価用アプリ作成 END

        '2014/07/31 部品のみ出庫対応によるNull値の考慮(予定開始日時)
        If Not String.IsNullOrWhiteSpace(item(Enum_Area03DtColIndex.SCHE_START_DATETIME)) Then
            row.SCHE_START_DATETIME = Date.Parse(item(Enum_Area03DtColIndex.SCHE_START_DATETIME))
            '赤明細のチェック
            '現在日時＞作業開始予定時刻だったら赤明細とみなし、フラグを立てる
            row.SORT_KEY = CheckDelayOfDate(nowDate, row.SCHE_START_DATETIME)
        Else
            '出力先にNull値をセット
            row.SetSCHE_START_DATETIMENull()
            '日付がセットされていなかったら赤明細のチェックを行わずNormalとする
            row.SORT_KEY = Enum_BackColor.Normal
        End If

        '$04 TR-SVT-TMT-20161229-001 部品庫モニターでエラーを検知 START
        ''終了ログ
        'Logger.Info(String.Format(CultureInfo.CurrentCulture _
        '    , "{0}.{1} {2}" _
        '    , Me.GetType.ToString _
        '    , System.Reflection.MethodBase.GetCurrentMethod.Name _
        '    , ConsLogEnd))
        '$04 TR-SVT-TMT-20161229-001 部品庫モニターでエラーを検知 END

        Return row
    End Function

    ''' <summary>
    ''' 引き取り待ちデータ作成
    ''' </summary>
    ''' <param name="nowDate">現在日時</param>
    ''' <param name="itemValue">出力内容(タブ区切りで格納されている)</param>
    ''' <param name="dataTable">追加するDataTable</param>
    Protected Function MakeWaitingforTechnicianPickupData(ByVal nowDate As Date, _
                                                    ByVal itemValue As String, _
                                                    ByVal dataTable As SC3190402DataSet.AREA04ResDataTable, _
                                                    Optional ByVal itemSvValue As String = "") _
                                                        As SC3190402DataSet.AREA04ResRow
        '$04 TR-SVT-TMT-20161229-001 部品庫モニターでエラーを検知 START
        ''開始ログ
        'Logger.Info(String.Format(CultureInfo.CurrentCulture _
        '   , "{0}.{1} {2} P1:{3} P2:{4}" _
        '   , Me.GetType.ToString _
        '   , System.Reflection.MethodBase.GetCurrentMethod.Name _
        '   , ConsLogStart _
        '   , nowDate _
        '   , itemValue _
        '   ))
        '$04 TR-SVT-TMT-20161229-001 部品庫モニターでエラーを検知 END

        'タブで区切って配列に格納後、データテーブルへセットする
        Dim item() As String = itemValue.Split(ControlChars.Tab)
        Dim row As SC3190402DataSet.AREA04ResRow = dataTable.NewAREA04ResRow

        row.RO_NUM = item(Enum_Area04DtColIndex.RO_NUM)
        row.RO_SEQ = item(Enum_Area04DtColIndex.RO_SEQ)
        row.REG_NUM = item(Enum_Area04DtColIndex.REG_NUM)
        row.MODEL_NAME = item(Enum_Area04DtColIndex.MODEL_NAME)
        row.GRADE_NAME = item(Enum_Area04DtColIndex.GRADE_NAME)
        row.STALL_NAME_SHORT = item(Enum_Area04DtColIndex.STALL_NAME_SHORT)
        row.SCHE_START_DATETIME = Date.Parse(item(Enum_Area04DtColIndex.SCHE_START_DATETIME))
        row.SCHE_DELI_DATETIME = Date.Parse(item(Enum_Area04DtColIndex.SCHE_DELI_DATETIME))
        row.DLR_CD = item(Enum_Area04DtColIndex.DLR_CD)
        row.BRN_CD = item(Enum_Area04DtColIndex.BRN_CD)
        '以下部品ステータス項目
        If itemSvValue.TrimEnd = "" Then
            row.BILL_NO = ""
            row.PARTS_STAFF_NAME = ""
            row.CAGE_NO = ""
        Else
            Dim itemSv() As String = itemSvValue.Split(ControlChars.Tab)
            row.BILL_NO = itemSv(Enum_AreaSVItemIndex.BILL_NO)
            row.PARTS_STAFF_NAME = itemSv(Enum_AreaSVItemIndex.PARTS_STAFF_NAME)
            row.CAGE_NO = itemSv(Enum_AreaSVItemIndex.CAGE_NO)
        End If

        '赤明細のチェック
        '現在日時＞作業開始予定時刻だったら赤明細とみなし、フラグを立てる
        row.SORT_KEY = CheckDelayOfDate(nowDate, row.SCHE_START_DATETIME)

        '$04 TR-SVT-TMT-20161229-001 部品庫モニターでエラーを検知 START
        ''終了ログ 
        'Logger.Info(String.Format(CultureInfo.CurrentCulture _
        '   , "{0}.{1} {2}" _
        '   , Me.GetType.ToString _
        '   , System.Reflection.MethodBase.GetCurrentMethod.Name _
        '   , ConsLogEnd))
        '$04 TR-SVT-TMT-20161229-001 部品庫モニターでエラーを検知 END

        Return row
    End Function

    ''' <summary>
    ''' 引き取り待ちデータ作成
    ''' </summary>
    ''' <param name="nowDate">現在日時</param>
    ''' <param name="row">引き取り待ち行データ</param>
    ''' <param name="dataTable">追加するDataTable</param>
    Protected Function MakeWaitingforTechnicianPickupDataByRow(ByVal nowDate As Date, _
                                                    ByVal row As SC3190402DataSet.AREA04Row, _
                                                    ByVal dataTable As SC3190402DataSet.AREA04ResDataTable) _
                                                        As SC3190402DataSet.AREA04ResRow
        '$04 TR-SVT-TMT-20161229-001 部品庫モニターでエラーを検知 START
        ''開始ログ
        'Logger.Info(String.Format(CultureInfo.CurrentCulture _
        '   , "{0}.{1} {2} P1:{3}" _
        '   , Me.GetType.ToString _
        '   , System.Reflection.MethodBase.GetCurrentMethod.Name _
        '   , ConsLogStart _
        '   , nowDate _
        '   ))
        '$04 TR-SVT-TMT-20161229-001 部品庫モニターでエラーを検知 END

        Dim newRow As SC3190402DataSet.AREA04ResRow = dataTable.NewAREA04ResRow
        newRow.RO_NUM = row.RO_NUM
        newRow.RO_SEQ = row.RO_SEQ
        newRow.REG_NUM = row.REG_NUM
        newRow.MODEL_NAME = row.MODEL_NAME
        newRow.GRADE_NAME = row.GRADE_NAME
        newRow.STALL_NAME_SHORT = row.STALL_NAME_SHORT
        newRow.SCHE_START_DATETIME = row.SCHE_START_DATETIME
        newRow.SCHE_DELI_DATETIME = row.SCHE_DELI_DATETIME
        newRow.DLR_CD = row.DLR_CD
        newRow.BRN_CD = row.BRN_CD
        '以下部品ステータス項目
        newRow.BILL_NO = ""             '出庫管理番号(必須)
        newRow.PARTS_STAFF_NAME = ""    'スタッフ名(任意)
        newRow.CAGE_NO = ""             'カゴNo(任意)

        '赤明細のチェック
        '現在日時＞作業開始予定時刻だったら赤明細とみなし、フラグを立てる
        newRow.SORT_KEY = CheckDelayOfDate(nowDate, row.SCHE_START_DATETIME)

        '$04 TR-SVT-TMT-20161229-001 部品庫モニターでエラーを検知 START
        ''終了ログ
        'Logger.Info(String.Format(CultureInfo.CurrentCulture _
        '   , "{0}.{1} {2}" _
        '   , Me.GetType.ToString _
        '   , System.Reflection.MethodBase.GetCurrentMethod.Name _
        '   , ConsLogEnd))
        '$04 TR-SVT-TMT-20161229-001 部品庫モニターでエラーを検知 END

        Return newRow
    End Function

    ''' <summary>
    '''         'RO番号からMAX連番取得
    ''' </summary>
    ''' <param name="roNum">RO番号</param>
    ''' <returns>データテーブル</returns>
    ''' <remarks>RO条件にMAXのRO連番を取得する</remarks>
    Protected Shared Function GetMaxRoSeqData(ByVal roNum As String) As Integer
        '$04 TR-SVT-TMT-20161229-001 部品庫モニターでエラーを検知 START
        ''開始ログ
        'Logger.Info(String.Format(CultureInfo.CurrentCulture _
        '   , "{0} {1} P1:{2}" _
        '   , System.Reflection.MethodBase.GetCurrentMethod.Name _
        '   , ConsLogStart _
        '   , roNum _
        '   ))
        '$04 TR-SVT-TMT-20161229-001 部品庫モニターでエラーを検知 END

        '戻り値
        Dim ret As Integer = 0
        '追加作業チェック用RO連番MAX値データを取得する
        Using dt As SC3190402DataSet.MaxRoSeqDataTable = SC3190402DataSet.GetMaxRoSeq( _
                                                            StaffContext.Current.DlrCD, _
                                                            StaffContext.Current.BrnCD, _
                                                            roNum)
            'データがあるか？
            If dt.Rows.Count > 0 Then
                'RO連番のMAX値を返す
                ret = dt(0).MAX_RO_SEQ
            End If
        End Using

        '$04 TR-SVT-TMT-20161229-001 部品庫モニターでエラーを検知 START
        ''終了ログ
        'Logger.Info(String.Format(CultureInfo.CurrentCulture _
        '   , "{0} {1} OUT:retValue = {2}" _
        '   , System.Reflection.MethodBase.GetCurrentMethod.Name _
        '   , ConsLogEnd _
        '   , ret))
        '$04 TR-SVT-TMT-20161229-001 部品庫モニターでエラーを検知 END

        Return ret
    End Function

    ''' <summary>
    '''         '明細種別チェック
    ''' </summary>
    ''' <param name="nowDate">本日日付</param>
    ''' <param name="checkDate">チェック対象の日付</param>
    ''' <returns>データテーブル</returns>
    ''' <remarks>ROステータス及びストール利用ステータスを条件にデータを取得する</remarks>
    Protected Function CheckDelayOfDate(ByVal nowDate As Date, ByVal checkDate As Date) As Enum_BackColor
        '$04 TR-SVT-TMT-20161229-001 部品庫モニターでエラーを検知 START
        ''開始ログ
        'Logger.Info(String.Format(CultureInfo.CurrentCulture _
        '   , "{0}.{1} {2} P1:{3} P2:{4}" _
        '   , Me.GetType.ToString _
        '   , System.Reflection.MethodBase.GetCurrentMethod.Name _
        '   , ConsLogStart _
        '   , nowDate.ToString _
        '   , checkDate.ToString _
        '   ))
        '$04 TR-SVT-TMT-20161229-001 部品庫モニターでエラーを検知 END

        'M.Sakamoto (トライ店システム評価)部品出庫オペレーションにおける遅れ管理機能強化検証 START $05
        Dim nowDateSubtract As New TimeSpan(0, 0, 0, nowDate.Second)
        nowDate = nowDate.Subtract(nowDateSubtract)
        Dim checkDatenowDateSubtract As New TimeSpan(0, 0, 0, checkDate.Second)
        checkDate = checkDate.Subtract(checkDatenowDateSubtract)
        'M.Sakamoto (トライ店システム評価)部品出庫オペレーションにおける遅れ管理機能強化検証 END $05

        Dim ret As Enum_BackColor
        'checkDateがNullのときは通常明細とする
        '現在日時＞対象日時だったら赤明細
        If nowDate > checkDate Then
            ret = Enum_BackColor.Red
        Else
            ret = Enum_BackColor.Normal
        End If

        '$04 TR-SVT-TMT-20161229-001 部品庫モニターでエラーを検知 START
        ''終了ログ
        'Logger.Info(String.Format(CultureInfo.CurrentCulture _
        '   , "{0}.{1} {2} OUT:retValue = {3}" _
        '   , Me.GetType.ToString _
        '   , System.Reflection.MethodBase.GetCurrentMethod.Name _
        '   , ConsLogEnd _
        '   , ret))
        '$04 TR-SVT-TMT-20161229-001 部品庫モニターでエラーを検知 END

        Return ret
    End Function

#End Region

#Region "非公開メソッド"
    '$01 部品庫B／O管理に向けた評価用アプリ作成 START

    ' ''' <summary>
    ' ''' かご番号設定
    ' ''' </summary>
    ' ''' <param name="dealerCode">販売店コード</param>
    ' ''' <param name="branchCode">店舗コード</param>
    ' ''' <param name="account">アカウント</param>
    ' ''' <param name="nowDate">現在日付</param>
    ' ''' <param name="dispDt">表示対象データセット</param>
    ' ''' <param name="areaType">エリアタイプ(3:作業計画待ち,4:引き取り待ち)</param>
    ' ''' <returns>かご番号を設定した表示対象データセット</returns>
    ' ''' <remarks></remarks>
    'Private Function SetCageNo(ByVal dealerCode As String, _
    '                             ByVal branchCode As String, _
    '                             ByVal account As String, _
    '                             ByVal nowDate As Date, _
    '                             ByRef dispDt As DataTable, _
    '                             ByVal areaType As String) As DataTable

    ''' <summary>
    ''' かご番号設定
    ''' </summary>
    ''' <param name="dealerCode">販売店コード</param>
    ''' <param name="branchCode">店舗コード</param>
    ''' <param name="account">アカウント</param>
    ''' <param name="nowDate">現在日付</param>
    ''' <param name="dispDt">表示対象データセット</param>
    ''' <param name="areaType">エリアタイプ(3:作業計画待ち,4:引き取り待ち)</param>
    ''' <param name="hasLock">ロック取得フラグ</param>
    ''' <returns>かご番号を設定した表示対象データセット</returns>
    ''' <remarks></remarks>
    Private Function SetCageNo(ByVal dealerCode As String, _
                                 ByVal branchCode As String, _
                                 ByVal account As String, _
                                 ByVal nowDate As Date, _
                                 ByRef dispDt As DataTable, _
                                 ByVal areaType As String, _
                                 ByVal hasLock As Boolean) As DataTable
        '$04 TR-SVT-TMT-20161229-001 部品庫モニターでエラーを検知 START
        ''開始ログ
        'Logger.Info(String.Format(CultureInfo.CurrentCulture _
        '   , "{0}.{1} {2} P1:{3} P2:{4}" _
        '   , Me.GetType.ToString _
        '   , System.Reflection.MethodBase.GetCurrentMethod.Name _
        '   , ConsLogStart _
        '   , dealerCode.ToString _
        '   , branchCode.ToString _
        '   ))
        '$04 TR-SVT-TMT-20161229-001 部品庫モニターでエラーを検知 END

        Dim RoNumList As List(Of String) = New List(Of String)
        Dim RoSeqList As List(Of Integer) = New List(Of Integer)
        Dim BillNoList As List(Of String) = New List(Of String)

        '表示対象データテーブルのRO_NUM,RO_SEQ,BILL_NOのリストを作成
        For Each dr As DataRow In dispDt.Rows()
            RoNumList.Add(dr.Item("RO_NUM"))
            RoSeqList.Add(dr.Item("RO_SEQ"))
            BillNoList.Add(GetBillNo(dr))
        Next

        '$04 TR-SVT-TMT-20161229-001 部品庫モニターでエラーを検知 START
        ''かご件数取得
        'Dim cageCount As Integer = SC3190402DataSet.GetCageCount(dealerCode, branchCode)

        ''かごが0件の場合、引数で受け取ったデータテーブルをそのまま返す
        'If cageCount = 0 Then
        '    Return dispDt
        'End If
        '$04 TR-SVT-TMT-20161229-001 部品庫モニターでエラーを検知 END

        '$04 TR-SVT-TMT-20161229-001 部品庫モニターでエラーを検知 START
        'ロック取得フラグがTrueの場合
        If hasLock Then
            '$04 TR-SVT-TMT-20161229-001 部品庫モニターでエラーを検知 END

            '空きかご存在フラグ
            Dim isExistNotUseCage As Boolean = True

            '空きかごのインデックス
            Dim notUseCageIndex As Integer = 0

            '空きかご取得
            Dim notUseCageDt As SC3190402DataSet.NotUseCageDataTable = SC3190402DataSet.GetNotUseCage(dealerCode, branchCode)

            If notUseCageDt.Count = 0 Then
                isExistNotUseCage = False
            End If

            '引き取り待ちデータのときのみ出庫表番号の更新を行う
            If AreaType_04.Equals(areaType) Then

                '未紐付けの情報を取得
                Dim notAssociatedRoDt As DataTable = GetNotAssociatedRoInfo(dealerCode, branchCode, RoNumList.ToArray, RoSeqList.ToArray, BillNoList.ToArray, dispDt)

                '出庫表番号未設定かご取得
                Dim notSetShipmentNoInfoDt As SC3190402DataSet.NotSetShipmentNoInfoDataTable = _
                    SC3190402DataSet.GetNotSetShipmentNo(dealerCode, branchCode)

                '出庫表番号未設定のかごが1件以上存在する場合、設定を行う。
                If 0 < notSetShipmentNoInfoDt.Count Then

                    '処理対象の情報を取得
                    Dim targetRows As DataRow() = GetTargetRows("RO_NUM = '{0}' AND RO_SEQ = {1} AND BILL_NO <> ' '", notAssociatedRoDt, notSetShipmentNoInfoDt)

                    For Each targetRow As DataRow In targetRows

                        '出庫表番号
                        Dim billNo As String = GetBillNo(targetRow)

                        '出庫表番号更新
                        Dim updateShipmentNoCount As Integer = SC3190402DataSet.UpdateShipmentNo( _
                            targetRow.Item("DLR_CD"), _
                            targetRow.Item("BRN_CD"), _
                            targetRow.Item("RO_NUM"), _
                            targetRow.Item("RO_SEQ"), _
                            billNo, _
                            nowDate, _
                            account)

                        '出庫表番号更新結果が0件、かつ、空きかごが存在する場合
                        '(RO番号、RO番号連番に対して、出庫表番号が複数ある場合、2件目以降更新件数が0件になる)
                        If updateShipmentNoCount = 0 AndAlso isExistNotUseCage Then

                            '紐付ける空きかご
                            Dim notUseCageRow1 As SC3190402DataSet.NotUseCageRow = notUseCageDt.Rows(notUseCageIndex)

                            'かごの紐付け
                            Dim updateNotAssociatedCount = SC3190402DataSet.UpdateNotAssociatedRoInfo( _
                                targetRow.Item("DLR_CD"), _
                                targetRow.Item("BRN_CD"), _
                                notUseCageRow1.CAGE_NO, _
                                targetRow.Item("RO_NUM"), _
                                targetRow.Item("RO_SEQ"), _
                                billNo, _
                                notUseCageRow1.USE_COUNT + 1, _
                                nowDate, _
                                account)

                            '更新に成功した場合、空きかごのインデックスをカウントアップ
                            If 0 < updateNotAssociatedCount Then
                                notUseCageIndex += 1

                                '空きかごがなくなった場合、空きかご存在フラグをfalseにする
                                If notUseCageIndex = notUseCageDt.Count Then
                                    isExistNotUseCage = False
                                End If
                            End If
                        End If
                    Next
                End If
            End If

            '空きかごが存在する場合
            If isExistNotUseCage Then

                '未紐付けの情報を取得
                Dim notAssociatedRoDt As DataTable = GetNotAssociatedRoInfo(dealerCode, branchCode, RoNumList.ToArray, RoSeqList.ToArray, BillNoList.ToArray, dispDt)

                '開始予定日順にソート
                Dim targetRows As DataRow() = notAssociatedRoDt.Select(Nothing, "SCHE_START_DATETIME ASC")

                For Each targetRow As DataRow In targetRows

                    '出庫表番号
                    Dim billNo As String = GetBillNo(targetRow)

                    '紐付ける空きかご
                    Dim notUseCageRow2 As SC3190402DataSet.NotUseCageRow = notUseCageDt.Rows(notUseCageIndex)

                    'かごの紐付け
                    Dim updateNotAssociatedCount = SC3190402DataSet.UpdateNotAssociatedRoInfo( _
                        targetRow.Item("DLR_CD"), _
                        targetRow.Item("BRN_CD"), _
                        notUseCageRow2.CAGE_NO, _
                        targetRow.Item("RO_NUM"), _
                        targetRow.Item("RO_SEQ"), _
                        billNo, _
                        notUseCageRow2.USE_COUNT + 1, _
                        nowDate, _
                        account)

                    '更新に成功した場合、空きかごのインデックスをカウントアップ
                    If 0 < updateNotAssociatedCount Then
                        notUseCageIndex += 1

                        '空きかごがなくなった場合、紐付けを終了する。
                        If notUseCageIndex = notUseCageDt.Count Then
                            Exit For
                        End If
                    End If
                Next
            End If
            '$04 TR-SVT-TMT-20161229-001 部品庫モニターでエラーを検知 START
        End If
        '$04 TR-SVT-TMT-20161229-001 部品庫モニターでエラーを検知 END

        'かご番号を設定したデータテーブルを取得
        Dim cageNoSetDt As DataTable = GetCageNoSetDataTable(dealerCode, _
                                                             branchCode, _
                                                             RoNumList.ToArray, _
                                                             RoSeqList.ToArray, _
                                                             BillNoList.ToArray, _
                                                             dispDt)

        '$04 TR-SVT-TMT-20161229-001 部品庫モニターでエラーを検知 START
        ''終了ログ
        'Logger.Info(String.Format(CultureInfo.CurrentCulture _
        '   , "{0}.{1} {2} QUERY:COUNT = {3}" _
        '   , Me.GetType.ToString _
        '   , System.Reflection.MethodBase.GetCurrentMethod.Name _
        '   , ConsLogEnd _
        '   , cageNoSetDt.Rows.Count))
        '$04 TR-SVT-TMT-20161229-001 部品庫モニターでエラーを検知 END

        Return cageNoSetDt
    End Function


    ''' <summary>
    ''' 処理対象の情報を取得
    ''' </summary>
    ''' <param name="searchCondition">検索条件</param>
    ''' <param name="dispDt">表示対象データテーブル</param>
    ''' <param name="dt"></param>
    ''' <returns>処理対象の情報</returns>
    ''' <remarks></remarks>
    Private Function GetTargetRows(ByVal searchCondition As String, ByVal dispDt As DataTable, ByVal dt As DataTable) As DataRow()

        '$04 TR-SVT-TMT-20161229-001 部品庫モニターでエラーを検知 START
        ''開始ログ
        'Logger.Info(String.Format(CultureInfo.CurrentCulture _
        '   , "{0}.{1} {2} P1:{3}" _
        '   , Me.GetType.ToString _
        '   , System.Reflection.MethodBase.GetCurrentMethod.Name _
        '   , ConsLogStart _
        '   , searchCondition _
        '   ))
        '$04 TR-SVT-TMT-20161229-001 部品庫モニターでエラーを検知 END

        Dim CloneDt As DataTable = dispDt.Clone

        For Each row As DataRow In dt.Rows

            '対象データを抽出
            Dim selectedRows() As Data.DataRow = _
                dispDt.Select(String.Format(CultureInfo.CurrentCulture, _
                                        searchCondition, _
                                        row("RO_NUM"), _
                                        row("RO_SEQ")))

            For Each row2 As DataRow In selectedRows
                CloneDt.ImportRow(row2)
            Next
        Next

        '開始予定日順にソート
        Dim targetRows As DataRow() = CloneDt.Select(Nothing, "SCHE_START_DATETIME ASC")

        '$04 TR-SVT-TMT-20161229-001 部品庫モニターでエラーを検知 START
        ''終了ログ
        'Logger.Info(String.Format(CultureInfo.CurrentCulture _
        '   , "{0}.{1} {2} QUERY:COUNT = {3}" _
        '   , Me.GetType.ToString _
        '   , System.Reflection.MethodBase.GetCurrentMethod.Name _
        '   , ConsLogEnd _
        '   , targetRows.Count))
        '$04 TR-SVT-TMT-20161229-001 部品庫モニターでエラーを検知 END

        Return targetRows
    End Function


    ''' <summary>
    ''' かご番号を設定したデータテーブルを取得
    ''' </summary>
    ''' <param name="dealerCode">販売店コード</param>
    ''' <param name="branchCode">店舗コード</param>
    ''' <param name="roNum">R/O番号</param>
    ''' <param name="roSeq">R/O番号連番</param>
    ''' <param name="shipmentNo">出庫表番号</param>
    ''' <param name="dispDt">表示対象データテーブル</param>
    ''' <returns>かご番号を設定したデータテーブル</returns>
    ''' <remarks></remarks>
    Private Function GetCageNoSetDataTable(ByVal dealerCode As String, _
                                           ByVal branchCode As String, _
                                           ByVal roNum() As String, _
                                           ByVal roSeq() As Integer, _
                                           ByVal shipmentNo() As String, _
                                           ByVal dispDt As DataTable) As DataTable

        '$04 TR-SVT-TMT-20161229-001 部品庫モニターでエラーを検知 START
        ''開始ログ
        'Logger.Info(String.Format(CultureInfo.CurrentCulture _
        '   , "{0}.{1} {2} P1:{3} P2:{4} P3:{5} P4:{6} P5:{7}" _
        '   , Me.GetType.ToString _
        '   , System.Reflection.MethodBase.GetCurrentMethod.Name _
        '   , ConsLogStart _
        '   , dealerCode.ToString _
        '   , branchCode.ToString _
        '   , String.Join(ConsComma, roNum) _
        '   , String.Join(ConsComma, roSeq) _
        '   , String.Join(ConsComma, shipmentNo) _
        '   ))
        '$04 TR-SVT-TMT-20161229-001 部品庫モニターでエラーを検知 END

        Dim CopyDt As DataTable = dispDt.Copy

        'かご番号取得
        Dim cageNoDt As SC3190402DataSet.CageNoInfoDataTable = _
            SC3190402DataSet.GetCageNo(dealerCode, branchCode, roNum, roSeq, shipmentNo)

        For Each dr As DataRow In CopyDt.Rows()

            '出庫表番号
            Dim billNo As String
            If dr.IsNull("BILL_NO") OrElse String.IsNullOrEmpty(dr.Item("BILL_NO")) Then
                billNo = " "
            Else
                billNo = dr.Item("BILL_NO")
            End If

            Dim searchCondition As New StringBuilder
            With searchCondition
                .Append("RO_NUM = '")
                .Append(dr.Item("RO_NUM"))
                .Append("' AND RO_SEQ = ")
                .Append(dr.Item("RO_SEQ"))
                .Append("AND SHIPMENT_NO = '")
                .Append(billNo)
                .Append("'")
            End With

            '対象データを抽出
            Dim cageNoRows() As Data.DataRow = cageNoDt.Select(searchCondition.ToString)

            If 0 < cageNoRows.Length Then
                dr.Item("CAGE_NO") = cageNoRows(0).Item("CAGE_NO")
            Else
                dr.Item("CAGE_NO") = "-"
            End If
        Next

        '$04 TR-SVT-TMT-20161229-001 部品庫モニターでエラーを検知 START
        ''終了ログ
        'Logger.Info(String.Format(CultureInfo.CurrentCulture _
        '   , "{0}.{1} {2} QUERY:COUNT = {3}" _
        '   , Me.GetType.ToString _
        '   , System.Reflection.MethodBase.GetCurrentMethod.Name _
        '   , ConsLogEnd _
        '   , CopyDt.Rows.Count))
        '$04 TR-SVT-TMT-20161229-001 部品庫モニターでエラーを検知 END

        Return CopyDt
    End Function

    ''' <summary>
    ''' 未紐付けRO情報取得
    ''' </summary>
    ''' <param name="dealerCode">販売店コード</param>
    ''' <param name="branchCode">店舗コード</param>
    ''' <param name="roNum">R/O番号</param>
    ''' <param name="roSeq">R/O番号連番</param>
    ''' <param name="shipmentNo">出庫表番号</param>
    ''' <param name="dispDt">表示対象データテーブル</param>
    ''' <returns>かご番号を設定したデータテーブル</returns>
    ''' <remarks></remarks>
    Private Function GetNotAssociatedRoInfo(ByVal dealerCode As String, _
                                           ByVal branchCode As String, _
                                           ByVal roNum() As String, _
                                           ByVal roSeq() As Integer, _
                                           ByVal shipmentNo() As String, _
                                           ByVal dispDt As DataTable) As DataTable

        '$04 TR-SVT-TMT-20161229-001 部品庫モニターでエラーを検知 START
        ''開始ログ
        'Logger.Info(String.Format(CultureInfo.CurrentCulture _
        '   , "{0}.{1} {2} P1:{3} P2:{4} P3:{5} P4:{6} P5:{7}" _
        '   , Me.GetType.ToString _
        '   , System.Reflection.MethodBase.GetCurrentMethod.Name _
        '   , ConsLogStart _
        '   , dealerCode.ToString _
        '   , branchCode.ToString _
        '   , String.Join(ConsComma, roNum) _
        '   , String.Join(ConsComma, roSeq) _
        '   , String.Join(ConsComma, shipmentNo) _
        '   ))
        '$04 TR-SVT-TMT-20161229-001 部品庫モニターでエラーを検知 END

        '紐付け済みRO番号取得
        Dim associatedRoInfoDt As SC3190402DataSet.AssociatedRoInfoDataTable = _
            SC3190402DataSet.GetAssociatedRoInfo(dealerCode, branchCode, roNum, roSeq, shipmentNo)

        Dim CloneDt As DataTable = dispDt.Clone

        '表示対象データテーブルから、紐付け済みのデータを除く
        For Each row As DataRow In dispDt.Rows

            Dim associatedRoInfoRows() As Data.DataRow = _
                associatedRoInfoDt.Select(String.Format(CultureInfo.CurrentCulture, _
                                        "RO_NUM = '{0}' AND RO_SEQ = {1} AND SHIPMENT_NO = '{2}'", _
                                        row("RO_NUM"), _
                                        row("RO_SEQ"), _
                                        row("BILL_NO")))

            If associatedRoInfoRows.Count = 0 Then
                CloneDt.ImportRow(row)
            End If

        Next

        '$04 TR-SVT-TMT-20161229-001 部品庫モニターでエラーを検知 START
        ''終了ログ
        'Logger.Info(String.Format(CultureInfo.CurrentCulture _
        '   , "{0}.{1} {2} QUERY:COUNT = {3}" _
        '   , Me.GetType.ToString _
        '   , System.Reflection.MethodBase.GetCurrentMethod.Name _
        '   , ConsLogEnd _
        '   , CloneDt.Rows.Count))
        '$04 TR-SVT-TMT-20161229-001 部品庫モニターでエラーを検知 END

        Return CloneDt
    End Function

    ''' <summary>
    ''' 出庫表番号取得
    ''' </summary>
    ''' <param name="dr">DataRow</param>
    ''' <returns>かご番号を設定したデータテーブル</returns>
    ''' <remarks></remarks>
    Private Function GetBillNo(ByVal dr As DataRow) As String

        '$04 TR-SVT-TMT-20161229-001 部品庫モニターでエラーを検知 START
        ''開始ログ
        'Logger.Info(String.Format(CultureInfo.CurrentCulture _
        '   , "{0}.{1} {2} P1:{3}" _
        '   , Me.GetType.ToString _
        '   , System.Reflection.MethodBase.GetCurrentMethod.Name _
        '   , ConsLogStart _
        '   , dr.ToString _
        '   ))
        '$04 TR-SVT-TMT-20161229-001 部品庫モニターでエラーを検知 END

        '出庫表番号
        Dim billNo As String
        If dr.IsNull("BILL_NO") OrElse String.IsNullOrEmpty(dr.Item("BILL_NO")) Then
            billNo = " "
        Else
            billNo = dr.Item("BILL_NO")
        End If

        '$04 TR-SVT-TMT-20161229-001 部品庫モニターでエラーを検知 START
        ''終了ログ
        'Logger.Info(String.Format(CultureInfo.CurrentCulture _
        '   , "{0} {1} OUT:retValue = {2}" _
        '   , Me.GetType.ToString _
        '   , System.Reflection.MethodBase.GetCurrentMethod.Name _
        '   , ConsLogEnd _
        '   , billNo))
        '$04 TR-SVT-TMT-20161229-001 部品庫モニターでエラーを検知 END

        Return billNo
    End Function

    '$01 部品庫B／O管理に向けた評価用アプリ作成 END
#End Region

    ''' <summary>
    ''' IDisposableインターフェイス.Dispoase
    ''' </summary>
    ''' <remarks></remarks>
    Public Overloads Sub Dispose() Implements IDisposable.Dispose
        'Dispose(True)
        GC.SuppressFinalize(Me)
    End Sub

End Class
