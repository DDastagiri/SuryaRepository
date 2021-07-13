'━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
'SC3150101BusinessLogic.vb
'─────────────────────────────────────
'機能： TCメインメニュービジネスロジック
'補足： 
'作成： 2012/01/26 KN 鶴田
'更新： 2012/02/28 KN 渡辺 【SERVICE_1】関連チップの順不同開始を抑制するように修正
'更新： 2012/02/29 KN 佐藤 【SERVICE_1】DevPartnerの指摘内容を修正
'更新： 
'─────────────────────────────────────

Option Strict On
Option Explicit On

Imports System.Web.Script.Serialization
Imports Toyota.eCRB.SystemFrameworks.Core
Imports Toyota.eCRB.SystemFrameworks.Web
Imports Toyota.eCRB.iCROP.DataAccess.SC3150101
Imports System.Globalization
'Imports Toyota.eCRB.iCROP.DataAccess.StallInfo

Imports Toyota.eCRB.DMSLinkage.RepairOrderSearch.BizLogic.IC3801001
Imports Toyota.eCRB.DMSLinkage.RepairOrderSearch.DataAccess.IC3801001

Imports Toyota.eCRB.DMSLinkage.AddRepair.BizLogic.IC3800804
Imports Toyota.eCRB.DMSLinkage.AddRepair.DataAccess.IC3800804

Imports Toyota.eCRB.DMSLinkage.AddRepair.BizLogic.IC3800805

Public Class SC3150101BusinessLogic
    Inherits BaseBusinessComponent

#Region "SMB実績ステータスの規定値"
    ''' <summary>
    ''' SMB実績ステータス：未入庫
    ''' </summary>
    ''' <remarks></remarks>
    Private Const SMB_RESULT_STATUS_OUT_SHED As String = "0"
    ''' <summary>
    ''' SMB実績ステータス：未入庫
    ''' </summary>
    ''' <remarks></remarks>
    Private Const SMB_RESULT_STATUS_OUT_SHED_00 As String = "00"
    ''' <summary>
    ''' SMB実績ステータス：入庫
    ''' </summary>
    ''' <remarks></remarks>
    Private Const SMB_RESULT_STATUS_IN_SHED As String = "10"
    ''' <summary>
    ''' SMB実績ステータス：作業中
    ''' </summary>
    ''' <remarks></remarks>
    Private Const SMB_RESULT_STATUS_WORKING As String = "20"
    ''' <summary>
    ''' SMB実績ステータス：部品欠品
    ''' </summary>
    ''' <remarks></remarks>
    Private Const SMB_RESULT_STATUS_PARTSMISS As String = "30"
    ''' <summary>
    ''' SMB実績ステータス：お客様連絡待ち
    ''' </summary>
    ''' <remarks></remarks>
    Private Const SMB_RESULT_STATUS_WAITING_CONNECTION As String = "31"
    ''' <summary>
    ''' SMB実績ステータス：仮置き
    ''' </summary>
    ''' <remarks></remarks>
    Private Const SMB_RESULT_STATUS_ARRANGEMENT As String = "32"
    ''' <summary>
    ''' SMB実績ステータス：未来店客
    ''' </summary>
    ''' <remarks></remarks>
    Private Const SMB_RESULT_STATUS_NOTCOMING_CUSTOMER As String = "33"
    ''' <summary>
    ''' SMB実績ステータス：ストール待機
    ''' </summary>
    ''' <remarks></remarks>
    Private Const SMB_RESULT_STATUS_WAITING_STALL As String = "38"
    ''' <summary>
    ''' SMB実績ステータス：その他
    ''' </summary>
    ''' <remarks></remarks>
    Private Const SMB_RESULT_STATUS_OTHER As String = "39"
    ''' <summary>
    ''' SMB実績ステータス：洗車待ち
    ''' </summary>
    ''' <remarks></remarks>
    Private Const SMB_RESULT_STATUS_WAITING_WASH As String = "40"
    ''' <summary>
    ''' SMB実績ステータス：洗車中
    ''' </summary>
    ''' <remarks></remarks>
    Private Const SMB_RESULT_STATUS_WASHING As String = "41"
    ''' <summary>
    ''' SMB実績ステータス：検査待ち
    ''' </summary>
    ''' <remarks></remarks>
    Private Const SMB_RESULT_STATUS_WAITING_INSPECTION As String = "42"
    ''' <summary>
    ''' SMB実績ステータス：検査中
    ''' </summary>
    ''' <remarks></remarks>
    Private Const SMB_RESULT_STATUS_INSPECTING As String = "43"
    ''' <summary>
    ''' SMB実績ステータス：検査不合格
    ''' </summary>
    ''' <remarks></remarks>
    Private Const SMB_RESULT_STATUS_REJECTED As String = "44"
    ''' <summary>
    ''' SMB実績ステータス：預かり中
    ''' </summary>
    ''' <remarks></remarks>
    Private Const SMB_RESULT_STATUS_TAKING_CHARGE As String = "50"
    ''' <summary>
    ''' SMB実績ステータス：納車待ち
    ''' </summary>
    ''' <remarks></remarks>
    Private Const SMB_RESULT_STATUS_WAITING_DELIVERY As String = "60"
    ''' <summary>
    ''' SMB実績ステータス：関連チップの前工程作業終了
    ''' </summary>
    ''' <remarks></remarks>
    Private Const SMB_RESULT_STATUS_FINISHED_PREVIOUS_PROCESS As String = "97"
    ''' <summary>
    ''' SMB実績ステータス：MidFinish
    ''' </summary>
    ''' <remarks></remarks>
    Private Const SMB_RESULT_STATUS_MID_FINISH As String = "98"
    ''' <summary>
    ''' SMB実績ステータス：完了
    ''' </summary>
    ''' <remarks></remarks>
    Private Const SMB_RESULT_STATUS_FINISHED As String = "99"

#End Region

#Region "実績ステータスの規定値"
    ''' <summary>
    ''' 実績ステータス：作業待ち
    ''' </summary>
    ''' <remarks></remarks>
    Private RESULT_STATUS_WAITING As String = "1"
    ''' <summary>
    ''' 実績ステータス：作業中
    ''' </summary>
    ''' <remarks></remarks>
    Private RESULT_STATUS_WORKING As String = "2"
    ''' <summary>
    ''' 実績ステータス：完了
    ''' </summary>
    ''' <remarks></remarks>
    Private RESULT_STATUS_FINISHED As String = "3"
#End Region

#Region "SMB実績ステータスの規定値"
    ''' <summary>
    ''' SMBステータス：ストール本予約
    ''' </summary>
    ''' <remarks></remarks>
    Private Const SMB_STATUS_COMMITE_RESOURCE As Integer = 1
    ''' <summary>
    ''' SMBステータス：ストール仮予約
    ''' </summary>
    ''' <remarks></remarks>
    Private Const SMB_STATUS_PROPOSED_RESOURCE As Integer = 2
    ''' <summary>
    ''' SMBステータス：Unavailable
    ''' </summary>
    ''' <remarks></remarks>
    Private Const SMB_STATUS_UNAVAILABLE As Integer = 3
    ''' <summary>
    ''' SMBステータス：取引納車
    ''' </summary>
    ''' <remarks></remarks>
    Private Const SMB_STATUS_PICK_DELIVERY As Integer = 4

#End Region

#Region "定数"

    ''' <summary>
    ''' DateTimeFuncにて、"yyyy/MM/dd HH:mm"形式をコンバートするためのID
    ''' </summary>
    ''' <remarks></remarks>
    Private Const DATE_CONVERT_ID_YYYYMMDDHHMM As Integer = 2
    ''' <summary>
    ''' DateTimeFuncにて、"yyyyMMdd"形式をコンバートするためのID
    ''' </summary>
    ''' <remarks></remarks>
    Private Const DATE_CONVERT_ID_YYYYMMDD As Integer = 9
    ''' <summary>
    ''' DateTimeFuncにて、"yyyy/MM/dd"形式をコンバートするためのID
    ''' </summary>
    ''' <remarks></remarks>
    Private Const DATE_CONVERT_ID_YYYY_MM_DD As Integer = 21

    ''' <summary>
    ''' データ更新用：Nullで上書き
    ''' </summary>
    ''' <remarks></remarks>
    Private Const OVERWRITE_NULL As Integer = 0
    ''' <summary>
    ''' データ更新用：指定値で上書き
    ''' </summary>
    ''' <remarks></remarks>
    Private Const OVERWRITE_NEW_VALUE As Integer = 1
    ''' <summary>
    ''' データ更新用：変更しない
    ''' </summary>
    ''' <remarks></remarks>
    Private Const KEEP_CURRENT As Integer = 2

    ''' <summary>
    ''' 予約履歴登録用：ストール予約 登録時
    ''' </summary>
    ''' <remarks></remarks>
    Private Const REZ_INSERT As Integer = 0
    ''' <summary>
    ''' 予約履歴登録用：ストール予約 通常更新時 / ACTUAL_TIME 更新時
    ''' </summary>
    ''' <remarks></remarks>
    Private Const REZ_UPDATE As Integer = 1
    ''' <summary>
    ''' 予約履歴登録用：ストール予約 キャンセル更新時
    ''' </summary>
    ''' <remarks></remarks>
    Private Const REZ_CANCEL As Integer = 2
    ''' <summary>
    ''' 予約履歴登録用：ストール予約 グループ更新時
    ''' </summary>
    ''' <remarks></remarks>
    Private Const REZ_GROUP As Integer = 3

    ''' <summary>
    ''' 戻り値：OK
    ''' </summary>
    ''' <remarks></remarks>
    Private Const RETURN_VALUE_OK As Integer = 0
    ''' <summary>
    ''' 戻り値：NG
    ''' </summary>
    ''' <remarks></remarks>
    Private Const RETURN_VALUE_NG As Integer = 906

    ''' <summary>
    ''' MidFinish作業終了時間の調整時間(時間)
    ''' </summary>
    ''' <remarks></remarks>
    Private Const C_SMB_DISPDATE_ADJUST As String = "SMB_DISPDATE_ADJUST"

    ''' <summary>
    ''' 稼働時間タイプ:Progressive
    ''' </summary>
    ''' <remarks></remarks>
    Private Const OPERATION_TIME_PROGRESS As Integer = 0
    ''' <summary>
    ''' 稼働時間タイプ:Reservation
    ''' </summary>
    ''' <remarks></remarks>
    Private Const OPERATION_TIME_RESERVE As Integer = 1

    ''' <summary>
    ''' 作業日付配列:配列数
    ''' </summary>
    ''' <remarks></remarks>
    Private Const WORK_DATE_ARRAY_NUMBER As Integer = 3
    ''' <summary>
    ''' 作業日付配列:開始日付の配列番号
    ''' </summary>
    ''' <remarks></remarks>
    Private Const WORK_START_DATE As Integer = 0
    ''' <summary>
    ''' 作業日付配列:開始時刻の配列番号
    ''' </summary>
    ''' <remarks></remarks>
    Private Const WORK_START_TIME As Integer = 1
    ''' <summary>
    ''' 作業日付配列:終了時刻の配列番号
    ''' </summary>
    ''' <remarks></remarks>
    Private Const WORK_END_TIME As Integer = 2

    ''' <summary>
    ''' ストール日時配列:配列数
    ''' </summary>
    ''' <remarks></remarks>
    Private Const STALL_DATE_ARRAY_NUMBER As Integer = 2
    ''' <summary>
    ''' ストール日時配列:開始日付
    ''' </summary>
    ''' <remarks></remarks>
    Private Const STALL_START_DATE As Integer = 0
    ''' <summary>
    ''' ストール日時配列:開始時刻
    ''' </summary>
    ''' <remarks></remarks>
    Private Const STALL_START_TIME As Integer = 1

    ''' <summary>
    ''' 時刻の表現タイプ:24時以降表記(25:00など)
    ''' </summary>
    ''' <remarks></remarks>
    Private Const TIME_TYPE_OVER24 As Integer = 1
    ''' <summary>
    ''' 時刻の表現タイプ:通常
    ''' </summary>
    ''' <remarks></remarks>
    Private Const TIME_TYPE_NORMAL As Integer = 0

    ''' <summary>
    ''' 作業開始時間取得用配列:配列数
    ''' </summary>
    ''' <remarks></remarks>
    Private Const START_TIME_ARRAY_NUMBER As Integer = 2
    ''' <summary>
    ''' 作業開始時間取得用配列:作業終了時間
    ''' </summary>
    ''' <remarks></remarks>
    Private Const START_TIME_START As Integer = 0
    ''' <summary>
    ''' 作業開始時間取得用配列:作業終了時間
    ''' </summary>
    ''' <remarks></remarks>
    Private Const START_TIME_END As Integer = 1

    ''' <summary>
    ''' 作業終了時間取得用配列:配列数
    ''' </summary>
    ''' <remarks></remarks>
    Private Const END_TIME_ARRAY_NUMBER As Integer = 2
    ''' <summary>
    ''' 作業終了時間取得用配列:作業終了時間
    ''' </summary>
    ''' <remarks></remarks>
    Private Const END_TIME_END As Integer = 0
    ''' <summary>
    ''' 作業終了時間取得用配列:作業終了時間
    ''' </summary>
    ''' <remarks></remarks>
    Private Const END_TIME_START As Integer = 1

    ''' <summary>
    ''' 対象日後の稼働日用配列:配列数
    ''' </summary>
    ''' <remarks></remarks>
    Private Const TARGET_DATE_ARRAY_NUMBER As Integer = 2
    ''' <summary>
    ''' 対象日後の稼働日用配列:稼働日
    ''' </summary>
    ''' <remarks></remarks>
    Private Const TARGET_DATE_DATE As Integer = 0
    ''' <summary>
    ''' 対象日後の稼働日用配列:非稼働日数
    ''' </summary>
    ''' <remarks></remarks>
    Private Const TARGET_DATE_COUNT As Integer = 1

    ''' <summary>
    ''' ログタイプ:情報
    ''' </summary>
    ''' <remarks></remarks>
    Private Const LOG_TYPE_INFO As String = "I"
    ''' <summary>
    ''' ログタイプ:エラー
    ''' </summary>
    ''' <remarks></remarks>
    Private Const LOG_TYPE_ERROR As String = "E"
    ''' <summary>
    ''' ログタイプ:警告
    ''' </summary>
    ''' <remarks></remarks>
    Private Const LOG_TYPE_WARNING As String = "W"

    ' 2012/03/02 KN 西田【SERVICE_1】START
    ''' <summary>
    ''' R/Oステータス＜7：検査完了＞
    ''' </summary>
    ''' <remarks></remarks>
    Private Const ORDER_NO_STATUS_COMPLET As String = "7"

    ''' <summary>
    ''' 追加作業＜9：完成検査完了＞
    ''' </summary>
    ''' <remarks></remarks>
    Private Const TACT_ADD_REPAIR_STATUS_COMPLET As String = "9"
    ' 2012/03/02 KN 西田【SERVICE_1】END
#End Region

#Region "日付変換用定数"

    ''' <summary>
    ''' 日付フォーマット変換用：変換前文字列の長さ
    ''' </summary>
    ''' <remarks></remarks>
    Private Const EXCHANGE_TIME_TARGET_LENGTH As Integer = 12
    ''' <summary>
    ''' 日付フォーマット変換用：西暦の開始インデックス
    ''' </summary>
    ''' <remarks></remarks>
    Private Const EXCHANGE_TIME_YEAR_START_INDEX As Integer = 0
    ''' <summary>
    ''' 日付フォーマット変換用：西暦の文字長
    ''' </summary>
    ''' <remarks></remarks>
    Private Const EXCHANGE_TIME_YEAR_LENGTH As Integer = 4
    ''' <summary>
    ''' 日付フォーマット変換用：月の開始インデックス
    ''' </summary>
    ''' <remarks></remarks>
    Private Const EXCHANGE_TIME_MONTH_START_INDEX As Integer = 4
    ''' <summary>
    ''' 日付フォーマット変換用：月の文字長
    ''' </summary>
    ''' <remarks></remarks>
    Private Const EXCHANGE_TIME_MONTH_LENGTH As Integer = 2
    ''' <summary>
    ''' 日付フォーマット変換用：日の開始インデックス
    ''' </summary>
    ''' <remarks></remarks>
    Private Const EXCHANGE_TIME_DAY_START_INDEX As Integer = 6
    ''' <summary>
    ''' 日付フォーマット変換用：日の文字長
    ''' </summary>
    ''' <remarks></remarks>
    Private Const EXCHANGE_TIME_DAY_LENGTH As Integer = 2
    ''' <summary>
    ''' 日付フォーマット変換用：時間の開始インデックス
    ''' </summary>
    ''' <remarks></remarks>
    Private Const EXCHANGE_TIME_HOUR_START_INDEX As Integer = 8
    ''' <summary>
    ''' 日付フォーマット変換用：時間の文字長
    ''' </summary>
    ''' <remarks></remarks>
    Private Const EXCHANGE_TIME_HOUR_LENGTH As Integer = 2
    ''' <summary>
    ''' 日付フォーマット変換用：分の開始インデックス
    ''' </summary>
    ''' <remarks></remarks>
    Private Const EXCHANGE_TIME_MINUTE_START_INDEX As Integer = 10
    ''' <summary>
    ''' 日付フォーマット変換用：分の文字長
    ''' </summary>
    ''' <remarks></remarks>
    Private Const EXCHANGE_TIME_MINUTE_LENGTH As Integer = 2
#End Region

#Region "JSON変換"
    ''' <summary>
    ''' 取得した開始時間（実績）、終了時間（実績）はなぜか"yyyymmddhhmm"の文字列にて格納されているため
    ''' "yyyy/mm/dd hh:mm"形式に変換して文字列として返す
    ''' </summary>
    ''' <param name="aTimeData"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function ExchangeTimeString(ByVal aTimeData As String) As String

        Dim stringDate As New System.Text.StringBuilder
        Dim inputTimeData As String
        inputTimeData = Trim(aTimeData)
        '取得した文字列が12文字でない場合、変換対象外とみなし、空文字を返す
        '文字列が12文字の場合のみ処理を実施する
        If (inputTimeData.Length() = EXCHANGE_TIME_TARGET_LENGTH) Then
            stringDate.Append(inputTimeData.Substring(EXCHANGE_TIME_YEAR_START_INDEX, EXCHANGE_TIME_YEAR_LENGTH))
            stringDate.Append("/")
            stringDate.Append(inputTimeData.Substring(EXCHANGE_TIME_MONTH_START_INDEX, EXCHANGE_TIME_MONTH_LENGTH))
            stringDate.Append("/")
            stringDate.Append(inputTimeData.Substring(EXCHANGE_TIME_DAY_START_INDEX, EXCHANGE_TIME_DAY_LENGTH))
            stringDate.Append(" ")
            stringDate.Append(inputTimeData.Substring(EXCHANGE_TIME_HOUR_START_INDEX, EXCHANGE_TIME_HOUR_LENGTH))
            stringDate.Append(":")
            stringDate.Append(inputTimeData.Substring(EXCHANGE_TIME_MINUTE_START_INDEX, EXCHANGE_TIME_MINUTE_LENGTH))
        End If

        Return stringDate.ToString()

    End Function



    ''' <summary>
    '''   DataTableをJSON文字列に変換する
    ''' </summary>
    ''' <param name="dataTable">変換対象 DataSet</param>
    ''' <returns>JSON文字列</returns>
    ''' <remarks></remarks>
    Public Function DataTableToJson(ByVal dataTable As DataTable) As String
        Dim resultMain As New Dictionary(Of String, Object)
        Dim JSerializer As New JavaScriptSerializer

        If dataTable Is Nothing Then
            Return JSerializer.Serialize(resultMain)
        End If

        For Each dr As DataRow In dataTable.Rows
            Dim result As New Dictionary(Of String, Object)

            For Each dc As DataColumn In dataTable.Columns
                result.Add(dc.ColumnName, dr(dc).ToString)
            Next
            resultMain.Add("Key" + CType(resultMain.Count + 1, String), result)
        Next

        Return JSerializer.Serialize(resultMain)
    End Function
#End Region

#Region "休憩時間の取得"

    ''' <summary>
    ''' 休憩時間データの格納文字数
    ''' </summary>
    ''' <remarks></remarks>
    Private Const BREAK_TIME_DATA_STRING_LENGTH = 4

    ''' <summary>
    ''' 休憩時間データの時間情報開始位置
    ''' </summary>
    ''' <remarks></remarks>
    Private Const BREAK_TIME_HOUR_INDEX = 0
    ''' <summary>
    ''' 休憩時間データの時間情報文字数
    ''' </summary>
    ''' <remarks></remarks>
    Private Const BREAK_TIME_HOUR_LENGTH = 2
    ''' <summary>
    ''' 休憩時間データの分情報開始位置
    ''' </summary>
    ''' <remarks></remarks>
    Private Const BREAK_TIME_MINUTE_INDEX = 2
    ''' <summary>
    ''' 休憩時間データの分情報文字数
    ''' </summary>
    ''' <remarks></remarks>
    Private Const BREAK_TIME_MINUTE_LENGTH = 2

    ''' <summary>
    ''' 休憩であることを示す、ステータス
    ''' </summary>
    ''' <remarks></remarks>
    Private Const STATUS_BLEAK As Integer = 99
    ''' <summary>
    ''' 使用不可であることを示す、ステータス
    ''' </summary>
    ''' <remarks></remarks>
    Private Const STATUS_UNAVAILABLE = 3

    ''' <summary>
    ''' 休憩時間を取得.
    ''' </summary>
    ''' <param name="stallId">ストールID</param>
    ''' <returns>休憩時間のデータセット</returns>
    ''' <remarks></remarks>
    Public Function GetBreakData(ByVal stallId As Integer) As SC3150101DataSet.SC3150101ChipInfoDataTable
        'Public Function GetBreakData(ByVal stallId As Integer) As SC3150101DataSet.SC3150101BreakChipInfoDataTable

        Logger.Info("GetBreakData Start param1:" + CType(stallId, String))

        Using adapter As New SC3150101DataSetTableAdapters.SC3150101StallInfoDataTableAdapter
            Dim dt As SC3150101DataSet.SC3150101BreakChipInfoDataTable
            Dim userContext As StaffContext = StaffContext.Current

            Using chipInfoTable As New SC3150101DataSet.SC3150101ChipInfoDataTable()

                '休憩チップデータを取得.
                dt = adapter.GetBreakChipInfo(userContext.DlrCD, userContext.BrnCD, stallId)

                Dim dataCount As Long = 0
                '取得した休憩情報は、開始時間・終了時間共にHHMMの4桁文字列で格納されている.
                'この状態では、他のチップとの選択に使用できないため、Date型に変換する.
                For Each dr As DataRow In dt.Rows

                    '開始時間と終了時間をDate型に変換する.
                    Dim startTimeDate As Date = ExchangeBreakHourToDate(userContext.DlrCD, CType(dr("STARTTIME"), String))
                    Dim endTimeDate As Date = ExchangeBreakHourToDate(userContext.DlrCD, CType(dr("ENDTIME"), String))

                    '終了時間が開始時間以下の場合、終了時間のほうが大きくなるように終了時間に1日ずつ加算していく.
                    While endTimeDate <= startTimeDate
                        endTimeDate.AddDays(1)
                    End While

                    '開始時間と終了時間を調整後、各カラムに格納する.
                    'dr("STARTTIME") = startTimeDate
                    'dr("ENDTIME") = endTimeDate
                    Dim chipInfoRow As SC3150101DataSet.SC3150101ChipInfoRow = CType(chipInfoTable.NewRow(), SC3150101DataSet.SC3150101ChipInfoRow)

                    chipInfoRow.DLRCD = userContext.DlrCD
                    chipInfoRow.STRCD = userContext.BrnCD
                    chipInfoRow.STARTTIME = startTimeDate
                    chipInfoRow.ENDTIME = endTimeDate
                    'チップステータスに休憩を示す値を格納する
                    chipInfoRow.STATUS = STATUS_BLEAK
                    'DBNull回避
                    chipInfoRow.REZID = -1
                    chipInfoRow.DSEQNO = 0
                    chipInfoRow.SEQNO = dataCount
                    'chipInfoRow.SERVICECODE_2 = "0"
                    chipInfoRow.RESULT_STALLID = stallId
                    chipInfoRow.STALLID = stallId
                    chipInfoRow.REZ_RECEPTION = ""
                    chipInfoRow.CUSTOMERNAME = ""
                    chipInfoRow.VEHICLENAME = ""
                    chipInfoRow.VCLREGNO = ""
                    chipInfoRow.INSDID = ""
                    chipInfoRow.CANCELFLG = ""
                    chipInfoRow.UPDATEACCOUNT = userContext.Account

                    chipInfoTable.Rows.Add(chipInfoRow)

                    dataCount = dataCount + 1
                Next

                Logger.Info("GetBreakData End")
                Return chipInfoTable
            End Using
        End Using

    End Function


    ''' <summary>
    ''' DBより取得した4桁の休憩時間を当日付けのDate型に変換する.
    ''' </summary>
    ''' <param name="dealerCode">販売店コード</param>
    ''' <param name="breakHour">4桁の休憩時間</param>
    ''' <returns>Date型の休憩時間</returns>
    ''' <remarks></remarks>
    Private Function ExchangeBreakHourToDate(ByVal dealerCode As String, ByVal breakHour As String) As Date

        Logger.Info("ExchangeBreakHourToDate Start param1:" + dealerCode + ",param2:" + breakHour)

        '返す値の初期値として、当日の0時を設定する.
        Dim breakDate As Date = DateTimeFunc.Now(dealerCode).Date

        '取得した引数が4桁である場合、変換処理を実施する.
        If (breakHour.Length = BREAK_TIME_DATA_STRING_LENGTH) Then

            Dim breakDateString As New System.Text.StringBuilder

            '当日日付を追加
            breakDateString.Append(DateTimeFunc.FormatDate(DATE_CONVERT_ID_YYYY_MM_DD, breakDate))
            breakDateString.Append(" ")
            breakDateString.Append(breakHour.Substring(BREAK_TIME_HOUR_INDEX, BREAK_TIME_HOUR_LENGTH))
            breakDateString.Append(":")
            breakDateString.Append(breakHour.Substring(BREAK_TIME_MINUTE_INDEX, BREAK_TIME_MINUTE_LENGTH))

            '生成した文字列を使用して、日付型データを取得する.
            breakDate = DateTimeFunc.FormatString("yyyy/MM/dd HH:mm", breakDateString.ToString())
        End If

        Logger.Info("ExchangeBreakHourToDate End return" + DateTimeFunc.FormatDate(DATE_CONVERT_ID_YYYYMMDDHHMM, breakDate))
        Return breakDate

    End Function

#End Region

#Region "使用不可時間の取得"

    ''' <summary>
    ''' 使用不可チップ情報の取得.
    ''' </summary>
    ''' <param name="stallId">ストールID</param>
    ''' <param name="fromDate">ストール稼動開始時間</param>
    ''' <param name="toDate">ストール稼動終了時間</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GetUnavailableData(ByVal stallId As Integer, ByVal fromDate As Date, _
                                               ByVal toDate As Date) As SC3150101DataSet.SC3150101ChipInfoDataTable
        'Public Function GetUnavailableData(ByVal stallId As Integer, ByVal fromDate As Date, _
        '                                       ByVal toDate As Date) As SC3150101DataSet.SC3150101UnavailableChipInfoDataTable

        Logger.Info("GetUnavailableData Start param1:" + CType(stallId, String))

        Using adapter As New SC3150101DataSetTableAdapters.SC3150101StallInfoDataTableAdapter

            Dim userContext As StaffContext = StaffContext.Current

            '使用不可チップデータを取得.
            Dim dtUnavailable As SC3150101DataSet.SC3150101UnavailableChipInfoDataTable
            dtUnavailable = adapter.GetUnavailableChipInfo(userContext.DlrCD, userContext.BrnCD, stallId, fromDate, toDate)

            '取得した使用不可チップデータをチップ情報テーブルに格納する.
            Using chipInfoTable As New SC3150101DataSet.SC3150101ChipInfoDataTable
                Dim dataCount As Long = 0
                For Each dr As SC3150101DataSet.SC3150101UnavailableChipInfoRow In dtUnavailable.Rows

                    Dim chipInfoRow As SC3150101DataSet.SC3150101ChipInfoRow = CType(chipInfoTable.NewRow(), SC3150101DataSet.SC3150101ChipInfoRow)

                    chipInfoRow.DLRCD = userContext.DlrCD
                    chipInfoRow.STRCD = userContext.BrnCD
                    chipInfoRow.STARTTIME = dr.STARTTIME
                    chipInfoRow.ENDTIME = dr.ENDTIME
                    '実績ステータスに使用不可を示す値を格納する
                    chipInfoRow.STATUS = STATUS_UNAVAILABLE
                    'DBNull回避
                    chipInfoRow.REZID = -2
                    chipInfoRow.DSEQNO = 0
                    chipInfoRow.SEQNO = dataCount
                    'chipInfoRow.SERVICECODE_2 = "0"
                    chipInfoRow.RESULT_STALLID = stallId
                    chipInfoRow.STALLID = stallId
                    chipInfoRow.REZ_RECEPTION = ""
                    chipInfoRow.CUSTOMERNAME = ""
                    chipInfoRow.VEHICLENAME = ""
                    chipInfoRow.VCLREGNO = ""
                    chipInfoRow.INSDID = ""
                    chipInfoRow.CANCELFLG = ""
                    chipInfoRow.UPDATEACCOUNT = userContext.Account

                    chipInfoTable.Rows.Add(chipInfoRow)

                    dataCount = dataCount + 1
                Next

                Logger.Info("GetUnavailableData End")
                Return chipInfoTable
            End Using
        End Using
    End Function
#End Region

#Region "ストール情報取得"

    ''' <summary>
    ''' ストール情報の取得.
    ''' </summary>
    ''' <returns>ストール情報データセット</returns>
    ''' <remarks></remarks>
    Public Function GetBelongStallData() As SC3150101DataSet.SC3150101BelongStallInfoDataTable

        Logger.Info("GetBelongStallData Start")

        Using adapter As New SC3150101DataSetTableAdapters.SC3150101StallInfoDataTableAdapter
            Dim dt As SC3150101DataSet.SC3150101BelongStallInfoDataTable
            Dim userContext As StaffContext = StaffContext.Current

            'ストール情報データセットを取得.
            dt = adapter.GetBelongStallInfo(userContext.Account, DateTimeFunc.FormatDate(DATE_CONVERT_ID_YYYYMMDD, DateTimeFunc.Now(userContext.DlrCD)))

            Logger.Info("GetBelongStallData End")
            Return dt
        End Using

    End Function


    ''' <summary>
    ''' ストールに所属するエンジニア名の取得
    ''' </summary>
    ''' <param name="stallId">ストールID</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GetBelongStallStaffData(stallId As Integer) As SC3150101DataSet.SC3150101BelongStallStaffDataTable

        Logger.Info("GetBelongStallStaffData Start")

        Using adapter As New SC3150101DataSetTableAdapters.SC3150101StallInfoDataTableAdapter
            Dim dt As SC3150101DataSet.SC3150101BelongStallStaffDataTable
            Dim userContext As StaffContext = StaffContext.Current

            'スタッフ情報データセットを取得.
            dt = adapter.GetBelongStallStaff(userContext.DlrCD, userContext.BrnCD, _
                        DateTimeFunc.FormatDate(DATE_CONVERT_ID_YYYYMMDD, DateTimeFunc.Now(userContext.DlrCD)), stallId)

            Logger.Info("GetBelongStallStaffData End")
            Return dt
        End Using

    End Function
#End Region

#Region "チップ情報（予約・実績）の取得"

    ''' <summary>
    '''   ストール（チップ）情報の取得
    ''' </summary>
    ''' <param name="stallId">ストールID</param>
    ''' <param name="dateFrom">稼働時間From</param>
    ''' <param name="dateTo">稼働時間To</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GetStallChipInfo(ByVal stallId As Integer, _
                                     ByVal dateFrom As Date, _
                                     ByVal dateTo As Date) As SC3150101DataSet.SC3150101ChipInfoDataTable

        Logger.Info("GetStallChipInfo Start")

        ' 予約チップ情報を取得
        Dim reserveChipInfo As SC3150101DataSet.SC3150101ChipInfoDataTable
        reserveChipInfo = GetReserveChipData(stallId, dateFrom, dateTo)


        ' 実績チップ情報を取得
        Dim resultChipInfo As SC3150101DataSet.SC3150101ChipInfoDataTable
        resultChipInfo = GetResultChipData(stallId, dateFrom, dateTo)

        '予約・実績チップ情報
        'Dim chipInfo As New SC3150101DataSet.SC3150101ChipInfoDataTable
        '予約チップ情報と実績チップ情報を追加する.
        'chipInfo.Concat(reserveChipInfo)
        'chipInfo.Concat(resultChipInfo)
        reserveChipInfo.Merge(resultChipInfo, False)

        Logger.Info("GetStallChipInfo End")
        Return reserveChipInfo

    End Function


    ''' <summary>
    ''' SMBに格納されている、実績ステータスを本システム用に変換する.
    ''' </summary>
    ''' <param name="SmbResultStatus">SMB上の実績ステータス</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function ExchangeChipResultStatus(ByVal SmbResultStatus As String) As String

        Logger.Info("ExchangeChipResultStatus Start")

        Dim parameterStatus As String = ""

        '取得したSMB上の実績ステータスより空白を除去する.
        If (Not IsDBNull(SmbResultStatus)) Then
            parameterStatus = Trim(SmbResultStatus)
        End If

        '返り値とする実績ステータスを初期化する.
        Dim resultStatus As String = RESULT_STATUS_WAITING

        If ((SMB_RESULT_STATUS_OUT_SHED.Equals(parameterStatus)) Or _
            (SMB_RESULT_STATUS_OUT_SHED_00.Equals(parameterStatus)) Or _
            (SMB_RESULT_STATUS_IN_SHED.Equals(parameterStatus))) Then
            '未入庫の場合、待機中に設定
            resultStatus = RESULT_STATUS_WAITING
        ElseIf (SMB_RESULT_STATUS_WORKING.Equals(parameterStatus)) Then
            'SMBの実績ステータスが作業中の場合、作業中とする
            resultStatus = RESULT_STATUS_WORKING
            'ElseIf (parameterStatus = "") Then
        ElseIf String.IsNullOrEmpty(parameterStatus) = True Then
            resultStatus = RESULT_STATUS_WAITING
        Else
            '上記条件以外の場合、作業完了とする
            resultStatus = RESULT_STATUS_FINISHED
        End If

        Logger.Info("ExchangeChipResultStatus End")
        Return resultStatus

    End Function

    ''' <summary>
    ''' 予約チップ情報の取得.
    ''' </summary>
    ''' <param name="stallId">ストールID</param>
    ''' <param name="fromDate">ストール稼動開始日時</param>
    ''' <param name="toDate">ストール稼動終了日時</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function GetReserveChipData(ByVal stallId As Integer, ByVal fromDate As Date, _
                                               ByVal toDate As Date) As SC3150101DataSet.SC3150101ChipInfoDataTable

        Logger.Info("GetReserveChipData Start")

        Dim userContext As StaffContext = StaffContext.Current
        Dim returnChipInfo As SC3150101DataSet.SC3150101ChipInfoDataTable ' 戻り値用

        Dim childChip As SC3150101DataSet.SC3150101ChildChipOrderNoDataTable
        Dim childChipItem As SC3150101DataSet.SC3150101ChildChipOrderNoRow

        Using adapter As New SC3150101DataSetTableAdapters.SC3150101StallInfoDataTableAdapter
            '予約チップ情報の取得.
            ' 2012/02/29 KN 佐藤 【SERVICE_1】DevPartnerの指摘内容を修正（処理修正） START
            'Dim dtReserveData As SC3150101DataSet.SC3150101ReserveChipInfoDataTable
            Dim reserveData As SC3150101DataSet.SC3150101ReserveChipInfoDataTable
            ' 2012/02/29 KN 佐藤 【SERVICE_1】DevPartnerの指摘内容を修正（処理修正） END
            reserveData = adapter.GetReserveChipInfo(userContext.DlrCD, userContext.BrnCD, stallId, fromDate, toDate)

            ' 予約チップ情報をチップ情報データセットに格納する.
            Using dtChipInfo As New SC3150101DataSet.SC3150101ChipInfoDataTable
                Dim reserveItem As SC3150101DataSet.SC3150101ReserveChipInfoRow
                For Each reserveItem In reserveData.Rows

                    'チップの作業時間が0以下の場合、チップ情報に追加しない.
                    If reserveItem.REZ_WORK_TIME <= 0 Then
                        Continue For
                    End If

                    '予約チップ情報を格納
                    ' 2012/02/29 KN 佐藤 【SERVICE_1】DevPartnerの指摘内容を修正（処理修正） START
                    'Dim drChipInfo As SC3150101DataSet.SC3150101ChipInfoRow = CType(dtChipInfo.NewRow(), SC3150101DataSet.SC3150101ChipInfoRow)
                    Dim chipInfoItem As SC3150101DataSet.SC3150101ChipInfoRow = _
                                            DirectCast(dtChipInfo.NewRow(), SC3150101DataSet.SC3150101ChipInfoRow)
                    ' 2012/02/29 KN 佐藤 【SERVICE_1】DevPartnerの指摘内容を修正（処理修正） END

                    '予約チップの値を格納
                    chipInfoItem.DLRCD = userContext.DlrCD
                    chipInfoItem.STRCD = userContext.BrnCD
                    chipInfoItem.REZID = reserveItem.REZID
                    chipInfoItem.DSEQNO = reserveItem.DSEQNO
                    chipInfoItem.SEQNO = reserveItem.SEQNO
                    chipInfoItem.VCLREGNO = reserveItem.VCLREGNO
                    '予約チップのサービスコード_Sを、サービスコード項目に格納.
                    'drChipInfo.SERVICECODE = reserveItem.SERVICECODE
                    chipInfoItem.SERVICECODE = reserveItem.SERVICECODE_S
                    chipInfoItem.RESULT_STATUS = ExchangeChipResultStatus(reserveItem.RESULT_STATUS)
                    chipInfoItem.REZ_RECEPTION = reserveItem.REZ_RECEPTION

                    If (Not reserveItem.IsREZ_START_TIMENull()) Then
                        Dim stringStartTime As String
                        stringStartTime = ExchangeTimeString(reserveItem.REZ_START_TIME)
                        'If (stringStartTime <> "") Then
                        '    drChipInfo.REZ_START_TIME = CType(stringStartTime, Date)
                        'End If
                        If String.IsNullOrEmpty(stringStartTime) = False Then
                            chipInfoItem.REZ_START_TIME = CType(stringStartTime, Date)
                        End If
                    End If

                    If (Not reserveItem.IsREZ_END_TIMENull()) Then
                        Dim stringEndTime As String
                        stringEndTime = ExchangeTimeString(reserveItem.REZ_END_TIME)
                        'If (stringEndTime <> "") Then
                        '    drChipInfo.REZ_END_TIME = CType(stringEndTime, Date)
                        'End If
                        If String.IsNullOrEmpty(stringEndTime) = False Then
                            chipInfoItem.REZ_END_TIME = CType(stringEndTime, Date)
                        End If
                    End If

                    chipInfoItem.REZ_WORK_TIME = reserveItem.REZ_WORK_TIME

                    If (Not reserveItem.IsUPDATE_COUNTNull()) Then
                        chipInfoItem.UPDATE_COUNT = reserveItem.UPDATE_COUNT
                    Else
                        chipInfoItem.UPDATE_COUNT = 0
                    End If

                    chipInfoItem.UPDATEDATE = reserveItem.UPDATEDATE
                    chipInfoItem.STARTTIME = reserveItem.STARTTIME
                    chipInfoItem.ENDTIME = reserveItem.ENDTIME
                    chipInfoItem.VEHICLENAME = reserveItem.VEHICLENAME

                    If (Not reserveItem.IsSTATUSNull()) Then
                        chipInfoItem.STATUS = reserveItem.STATUS
                    Else
                        chipInfoItem.STATUS = 0
                    End If

                    chipInfoItem.WALKIN = reserveItem.WALKIN
                    chipInfoItem.STOPFLG = reserveItem.STOPFLG

                    If (Not reserveItem.IsPREZIDNull()) And (reserveItem.PREZID <> -1) Then
                        chipInfoItem.PREZID = reserveItem.PREZID
                        ' R/O No. を取得
                        childChip = adapter.GetChildOrderNo(chipInfoItem.DLRCD, chipInfoItem.STRCD, _
                                                            CType(chipInfoItem.PREZID, Integer))
                        childChipItem = DirectCast(childChip.Rows(0), SC3150101DataSet.SC3150101ChildChipOrderNoRow)
                        If (Not childChipItem.IsORDERNONull()) Then
                            reserveItem.ORDERNO = childChipItem.ORDERNO ' 親チップの R/O No. をセット
                        End If
                    Else
                        chipInfoItem.PREZID = 0
                    End If

                    chipInfoItem.REZCHILDNO = reserveItem.REZCHILDNO

                    If (Not reserveItem.IsCRRYINTIMENull()) Then
                        chipInfoItem.CRRYINTIME = reserveItem.CRRYINTIME
                    End If

                    If (Not reserveItem.IsCRRYOUTTIMENull()) Then
                        chipInfoItem.CRRYOUTTIME = reserveItem.CRRYOUTTIME
                    End If
                    chipInfoItem.STRDATE = reserveItem.STRDATE
                    chipInfoItem.CANCELFLG = reserveItem.CANCELFLG
                    chipInfoItem.UPDATEACCOUNT = reserveItem.UPDATEACCOUNT
                    chipInfoItem.SVCORGNMCT = reserveItem.SVCORGNMCT
                    chipInfoItem.SVCORGNMCB = reserveItem.SVCORGNMCB
                    chipInfoItem.RELATIONSTATUS = reserveItem.RELATIONSTATUS

                    If (Not reserveItem.IsRELATION_UNFINISHED_COUNTNull()) Then
                        chipInfoItem.RELATION_UNFINISHED_COUNT = reserveItem.RELATION_UNFINISHED_COUNT
                    Else
                        chipInfoItem.RELATION_UNFINISHED_COUNT = 0
                    End If

                    chipInfoItem.ORDERNO = reserveItem.ORDERNO

                    'チップの作業時間が0以下の場合、チップ情報に追加しない.
                    'If (drChipInfo.REZ_WORK_TIME > 0) Then
                    '    dtChipInfo.Rows.Add(drChipInfo)
                    'End If
                    dtChipInfo.Rows.Add(chipInfoItem)
                Next

                'Logger.Info("GetReserveChipData End")
                'Return dtChipInfo
                returnChipInfo = CType(dtChipInfo.Copy, SC3150101DataSet.SC3150101ChipInfoDataTable)
            End Using
        End Using

        Logger.Info("GetReserveChipData End")
        Return returnChipInfo

    End Function


    ''' <summary>
    ''' 実績チップ情報の取得.
    ''' </summary>
    ''' <param name="stallId">ストールID</param>
    ''' <param name="fromDate">ストール稼働開始時間</param>
    ''' <param name="toDate">ストール稼動終了時間ｎ</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function GetResultChipData(ByVal stallId As Integer, ByVal fromDate As Date, _
                                               ByVal toDate As Date) As SC3150101DataSet.SC3150101ChipInfoDataTable

        Logger.Info("GetResultChipData Start")

        Dim userContext As StaffContext = StaffContext.Current
        Dim returnChipInfo As SC3150101DataSet.SC3150101ChipInfoDataTable ' 戻り値用

        Dim childChip As SC3150101DataSet.SC3150101ChildChipOrderNoDataTable
        Dim childChipItem As SC3150101DataSet.SC3150101ChildChipOrderNoRow

        Using adapter As New SC3150101DataSetTableAdapters.SC3150101StallInfoDataTableAdapter
            '実績チップ情報の取得.
            ' 2012/02/29 KN 佐藤 【SERVICE_1】DevPartnerの指摘内容を修正（処理修正） START
            'Dim dtResultData As SC3150101DataSet.SC3150101ResultChipInfoDataTable
            Dim resultData As SC3150101DataSet.SC3150101ResultChipInfoDataTable
            ' 2012/02/29 KN 佐藤 【SERVICE_1】DevPartnerの指摘内容を修正（処理修正） END
            resultData = adapter.GetResultChipInfo(userContext.DlrCD, userContext.BrnCD, stallId, fromDate, toDate)

            '実績チップ情報をチップ情報データセットに格納する.
            ' 2012/02/29 KN 佐藤 【SERVICE_1】DevPartnerの指摘内容を修正（処理修正） START
            'Using dtChipInfo As New SC3150101DataSet.SC3150101ChipInfoDataTable
            Using chipInfo As New SC3150101DataSet.SC3150101ChipInfoDataTable
                ' 2012/02/29 KN 佐藤 【SERVICE_1】DevPartnerの指摘内容を修正（処理修正） END
                Dim resultItem As SC3150101DataSet.SC3150101ResultChipInfoRow
                For Each resultItem In resultData.Rows

                    '実績チップ情報を格納
                    ' 2012/02/29 KN 佐藤 【SERVICE_1】DevPartnerの指摘内容を修正（処理修正） START
                    'Dim drChipInfo As SC3150101DataSet.SC3150101ChipInfoRow = CType(chipInfo.NewRow(), SC3150101DataSet.SC3150101ChipInfoRow)
                    Dim chipInfoItem As SC3150101DataSet.SC3150101ChipInfoRow = _
                                            DirectCast(chipInfo.NewRow(), SC3150101DataSet.SC3150101ChipInfoRow)
                    ' 2012/02/29 KN 佐藤 【SERVICE_1】DevPartnerの指摘内容を修正（処理修正） END

                    ' 2012/02/29 KN 佐藤 【SERVICE_1】DevPartnerの指摘内容を修正（処理追加） START
                    '実績チップ情報を初期化
                    chipInfoItem = InitChipInfoItem(chipInfoItem)
                    ' 2012/02/29 KN 佐藤 【SERVICE_1】DevPartnerの指摘内容を修正（処理追加） END

                    '実績チップの値を格納
                    chipInfoItem.DLRCD = userContext.DlrCD
                    chipInfoItem.STRCD = userContext.BrnCD
                    chipInfoItem.REZID = resultItem.REZID
                    chipInfoItem.DSEQNO = resultItem.DSEQNO
                    chipInfoItem.SEQNO = resultItem.SEQNO

                    If (Not resultItem.IsVCLREGNONull()) Then
                        chipInfoItem.VCLREGNO = resultItem.VCLREGNO
                        ' 2012/02/29 KN 佐藤 【SERVICE_1】DevPartnerの指摘内容を修正（処理削除） START
                        'Else
                        '    chipInfoItem.VCLREGNO = ""
                        ' 2012/02/29 KN 佐藤 【SERVICE_1】DevPartnerの指摘内容を修正（処理削除） END
                    End If

                    If (Not resultItem.IsSERVICECODENull()) Then
                        chipInfoItem.SERVICECODE = resultItem.SERVICECODE
                        ' 2012/02/29 KN 佐藤 【SERVICE_1】DevPartnerの指摘内容を修正（処理削除） START
                        'Else
                        '    chipInfoItem.SERVICECODE = ""
                        ' 2012/02/29 KN 佐藤 【SERVICE_1】DevPartnerの指摘内容を修正（処理削除） END
                    End If

                    chipInfoItem.RESULT_STATUS = ExchangeChipResultStatus(resultItem.RESULT_STATUS)
                    chipInfoItem.REZ_RECEPTION = resultItem.REZ_RECEPTION

                    If (Not resultItem.IsREZ_START_TIMENull()) Then
                        Dim stringStartTime As String
                        stringStartTime = ExchangeTimeString(resultItem.REZ_START_TIME)
                        'If (stringStartTime <> "") Then
                        '    drChipInfo.REZ_START_TIME = CType(stringStartTime, Date)
                        'End If
                        If String.IsNullOrEmpty(stringStartTime) = False Then
                            chipInfoItem.REZ_START_TIME = CType(stringStartTime, Date)
                        End If
                    End If

                    If (Not resultItem.IsREZ_END_TIMENull()) Then
                        Dim stringEndTime As String
                        stringEndTime = ExchangeTimeString(resultItem.REZ_END_TIME)
                        'If (stringEndTime <> "") Then
                        '    drChipInfo.REZ_END_TIME = CType(stringEndTime, Date)
                        'End If
                        If String.IsNullOrEmpty(stringEndTime) = False Then
                            chipInfoItem.REZ_END_TIME = CType(stringEndTime, Date)
                        End If
                    End If

                    If (Not resultItem.IsREZ_WORK_TIMENull()) Then
                        chipInfoItem.REZ_WORK_TIME = resultItem.REZ_WORK_TIME
                        ' 2012/02/29 KN 佐藤 【SERVICE_1】DevPartnerの指摘内容を修正（処理削除） START
                        'Else
                        '    chipInfoItem.REZ_WORK_TIME = 0
                        ' 2012/02/29 KN 佐藤 【SERVICE_1】DevPartnerの指摘内容を修正（処理削除） END
                    End If

                    If (Not resultItem.IsUPDATE_COUNTNull()) Then
                        chipInfoItem.UPDATE_COUNT = resultItem.UPDATE_COUNT
                        ' 2012/02/29 KN 佐藤 【SERVICE_1】DevPartnerの指摘内容を修正（処理削除） START
                        'Else
                        '    chipInfoItem.UPDATE_COUNT = 0
                        ' 2012/02/29 KN 佐藤 【SERVICE_1】DevPartnerの指摘内容を修正（処理削除） END
                    End If

                    chipInfoItem.UPDATEDATE = resultItem.UPDATEDATE
                    chipInfoItem.STARTTIME = resultItem.STARTTIME
                    chipInfoItem.ENDTIME = resultItem.ENDTIME
                    chipInfoItem.VEHICLENAME = resultItem.VEHICLENAME

                    If (Not resultItem.IsSTATUSNull()) Then
                        chipInfoItem.STATUS = resultItem.STATUS
                        ' 2012/02/29 KN 佐藤 【SERVICE_1】DevPartnerの指摘内容を修正（処理削除） START
                        'Else
                        '    chipInfoItem.STATUS = 0
                        ' 2012/02/29 KN 佐藤 【SERVICE_1】DevPartnerの指摘内容を修正（処理削除） END
                    End If

                    chipInfoItem.WALKIN = resultItem.WALKIN
                    chipInfoItem.STOPFLG = resultItem.STOPFLG

                    If (Not resultItem.IsPREZIDNull()) And (resultItem.PREZID <> -1) Then
                        chipInfoItem.PREZID = resultItem.PREZID
                        ' R/O No. を取得
                        childChip = adapter.GetChildOrderNo(chipInfoItem.DLRCD, chipInfoItem.STRCD, _
                                                            CType(chipInfoItem.PREZID, Integer))
                        childChipItem = CType(childChip.Rows(0), SC3150101DataSet.SC3150101ChildChipOrderNoRow)
                        If (Not childChipItem.IsORDERNONull()) Then
                            resultItem.ORDERNO = childChipItem.ORDERNO ' 親チップの R/O No. をセット
                        End If
                        ' 2012/02/29 KN 佐藤 【SERVICE_1】DevPartnerの指摘内容を修正（処理削除） START
                        'Else
                        '    chipInfoItem.PREZID = 0
                        ' 2012/02/29 KN 佐藤 【SERVICE_1】DevPartnerの指摘内容を修正（処理削除） END
                    End If

                    If (Not resultItem.IsREZCHILDNONull()) Then
                        chipInfoItem.REZCHILDNO = resultItem.REZCHILDNO
                        ' 2012/02/29 KN 佐藤 【SERVICE_1】DevPartnerの指摘内容を修正（処理削除） START
                        'Else
                        '    chipInfoItem.REZCHILDNO = 0
                        ' 2012/02/29 KN 佐藤 【SERVICE_1】DevPartnerの指摘内容を修正（処理削除） END
                    End If

                    chipInfoItem.STRDATE = resultItem.STRDATE
                    chipInfoItem.CANCELFLG = resultItem.CANCELFLG
                    chipInfoItem.UPDATEACCOUNT = resultItem.UPDATEACCOUNT
                    chipInfoItem.SVCORGNMCT = resultItem.SVCORGNMCT
                    chipInfoItem.SVCORGNMCB = resultItem.SVCORGNMCB
                    chipInfoItem.RELATIONSTATUS = resultItem.RELATIONSTATUS

                    If (Not resultItem.IsRELATION_UNFINISHED_COUNTNull()) Then
                        chipInfoItem.RELATION_UNFINISHED_COUNT = resultItem.RELATION_UNFINISHED_COUNT
                        ' 2012/02/29 KN 佐藤 【SERVICE_1】DevPartnerの指摘内容を修正（処理削除） START
                        'Else
                        '    chipInfoItem.RELATION_UNFINISHED_COUNT = 0
                        ' 2012/02/29 KN 佐藤 【SERVICE_1】DevPartnerの指摘内容を修正（処理削除） END
                    End If

                    chipInfoItem.ORDERNO = resultItem.ORDERNO

                    If (Not resultItem.IsRESULT_START_TIMENull()) Then
                        Dim stringResultStartTime As String
                        stringResultStartTime = ExchangeTimeString(resultItem.RESULT_START_TIME)
                        'If (stringResultStartTime <> "") Then
                        '    drChipInfo.RESULT_START_TIME = CType(stringResultStartTime, Date)
                        'End If
                        If String.IsNullOrEmpty(stringResultStartTime) = False Then
                            chipInfoItem.RESULT_START_TIME = CType(stringResultStartTime, Date)
                        End If

                    End If

                    If (Not resultItem.IsRESULT_END_TIMENull()) Then
                        Dim stringResultEndTime As String
                        stringResultEndTime = ExchangeTimeString(resultItem.RESULT_END_TIME)
                        'If (stringResultEndTime <> "") Then
                        '    drChipInfo.RESULT_END_TIME = CType(stringResultEndTime, Date)
                        'End If
                        If String.IsNullOrEmpty(stringResultEndTime) = False Then
                            chipInfoItem.RESULT_END_TIME = CType(stringResultEndTime, Date)
                        End If
                    End If

                    chipInfoItem.RESULT_IN_TIME = resultItem.RESULT_IN_TIME
                    chipInfoItem.RESULT_WORK_TIME = resultItem.RESULT_WORK_TIME
                    chipInfoItem.REZ_PICK_DATE = resultItem.REZ_PICK_DATE

                    If (Not resultItem.IsREZ_PICK_TIMENull()) Then
                        chipInfoItem.REZ_PICK_TIME = resultItem.REZ_PICK_TIME
                        ' 2012/02/29 KN 佐藤 【SERVICE_1】DevPartnerの指摘内容を修正（処理削除） START
                        'Else
                        '    chipInfoItem.REZ_PICK_TIME = 0
                        ' 2012/02/29 KN 佐藤 【SERVICE_1】DevPartnerの指摘内容を修正（処理削除） END
                    End If

                    chipInfoItem.REZ_DELI_DATE = resultItem.REZ_DELI_DATE

                    If (Not chipInfoItem.IsREZ_DELI_TIMENull()) Then
                        chipInfoItem.REZ_DELI_TIME = resultItem.REZ_DELI_TIME
                        ' 2012/02/29 KN 佐藤 【SERVICE_1】DevPartnerの指摘内容を修正（処理削除） START
                        'Else
                        '    chipInfoItem.REZ_DELI_TIME = 0
                        ' 2012/02/29 KN 佐藤 【SERVICE_1】DevPartnerの指摘内容を修正（処理削除） END
                    End If

                    chipInfoItem.RESULT_WAIT_END = resultItem.RESULT_WAIT_END

                    chipInfo.Rows.Add(chipInfoItem)
                Next

                'Logger.Info("GetResultChipData End")
                'Return dtChipInfo
                returnChipInfo = DirectCast(chipInfo.Copy, SC3150101DataSet.SC3150101ChipInfoDataTable)

            End Using
        End Using

        Return returnChipInfo

    End Function

    ' 2012/02/29 KN 佐藤 【SERVICE_1】DevPartnerの指摘内容を修正（処理修正） START
    ''' <summary>
    ''' 実績チップ情報を初期化
    ''' </summary>
    ''' <param name="chipInfoItem">実績チップ情報</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    ''' 
    ''' <history>
    ''' </history>
    Private Function InitChipInfoItem(ByVal chipInfoItem As SC3150101DataSet.SC3150101ChipInfoRow) As SC3150101DataSet.SC3150101ChipInfoRow
        With chipInfoItem
            .VCLREGNO = ""
            .SERVICECODE = ""
            .REZ_WORK_TIME = 0
            .REZ_PICK_TIME = 0
            .REZ_DELI_TIME = 0
            .UPDATE_COUNT = 0
            .STATUS = 0
            .PREZID = 0
            .REZCHILDNO = 0
            .RELATION_UNFINISHED_COUNT = 0
        End With
        Return chipInfoItem
    End Function
    ' 2012/02/29 KN 佐藤 【SERVICE_1】DevPartnerの指摘内容を修正（処理修正） END

#End Region

#Region "実績チップの取得"
    ''' <summary>
    '''   実績チップ情報を取得する
    ''' </summary>
    ''' <param name="dealerCode">販売店コード</param>
    ''' <param name="branchCode">店舗コード</param>
    ''' <param name="stallId">ストールID</param>
    ''' <param name="dateFrom">稼働時間FROM</param>
    ''' <param name="dateTo">稼働時間TO</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GetProcessChipInfo(ByVal dealerCode As String, _
                                       ByVal branchCode As String, _
                                       ByVal stallId As Integer, _
                                       ByVal dateFrom As Date, _
                                       ByVal dateTo As Date) As SC3150101DataSet.SC3150101ResultChipInfoDataTable

        OutputLog(LOG_TYPE_INFO, "[S]GetProcessChipInfo()", "", Nothing, _
                  "DLRCD:" & dealerCode, "STRCD:" & branchCode, "STALLID:" & CType(stallId, String), _
                  "DATE_FROM:" & CType(dateFrom, String), "DATE_TO:" & CType(dateTo, String))

        ' SC3150101TableAdapterクラスのインスタンスを生成
        Using adapter As New SC3150101DataSetTableAdapters.SC3150101StallInfoDataTableAdapter
            ' ストール実績情報を取得する
            Dim processChipInfo As SC3150101DataSet.SC3150101ResultChipInfoDataTable
            processChipInfo = adapter.GetResultChipInfo(dealerCode, branchCode, stallId, dateFrom, dateTo)
            Dim drProcessChipInfo As SC3150101DataSet.SC3150101ResultChipInfoRow
            If processChipInfo Is Nothing Then
                Return Nothing
            End If
            For Each drProcessChipInfo In processChipInfo.Rows
                drProcessChipInfo.REZID = SetNumericData(drProcessChipInfo.Item("REZID"), 0)
                drProcessChipInfo.DSEQNO = SetNumericData(drProcessChipInfo.Item("DSEQNO"), 0)
                drProcessChipInfo.SEQNO = SetNumericData(drProcessChipInfo.Item("SEQNO"), 0)
                drProcessChipInfo.MODELCODE = SetStringData(drProcessChipInfo.Item("MODELCODE"), "")
                drProcessChipInfo.VCLREGNO = SetStringData(drProcessChipInfo.Item("VCLREGNO"), "")
                drProcessChipInfo.SERVICECODE = SetStringData(drProcessChipInfo.Item("SERVICECODE"), "")
                'drProcessChipInfo.SERVICECODE_MST = SetStringData(drProcessChipInfo.Item("SERVICECODE_MST"), "")
                drProcessChipInfo.RESULT_STATUS = SetStringData(drProcessChipInfo.Item("RESULT_STATUS"), "")
                drProcessChipInfo.RESULT_STALLID = SetNumericData(drProcessChipInfo.Item("RESULT_STALLID"), 0)
                drProcessChipInfo.RESULT_START_TIME = SetStringData(drProcessChipInfo.Item("RESULT_START_TIME"), "")
                drProcessChipInfo.RESULT_END_TIME = SetStringData(drProcessChipInfo.Item("RESULT_END_TIME"), "")
                drProcessChipInfo.RESULT_IN_TIME = SetStringData(drProcessChipInfo.Item("RESULT_IN_TIME"), "")
                drProcessChipInfo.RESULT_WORK_TIME = SetNumericData(drProcessChipInfo.Item("RESULT_WORK_TIME"), 0)
                drProcessChipInfo.REZ_RECEPTION = SetStringData(drProcessChipInfo.Item("REZ_RECEPTION"), "")
                drProcessChipInfo.REZ_START_TIME = SetStringData(drProcessChipInfo.Item("REZ_START_TIME"), "")
                drProcessChipInfo.REZ_END_TIME = SetStringData(drProcessChipInfo.Item("REZ_END_TIME"), "")
                drProcessChipInfo.REZ_WORK_TIME = SetNumericData(drProcessChipInfo.Item("REZ_WORK_TIME"), 0)
                drProcessChipInfo.REZ_WORK_TIME_2 = SetNumericData(drProcessChipInfo.Item("REZ_WORK_TIME_2"), 0)
                drProcessChipInfo.REZ_PICK_DATE = SetStringData(drProcessChipInfo.Item("REZ_PICK_DATE"), "")
                drProcessChipInfo.REZ_PICK_TIME = SetNumericData(drProcessChipInfo.Item("REZ_PICK_TIME"), 0)
                drProcessChipInfo.REZ_DELI_DATE = SetStringData(drProcessChipInfo.Item("REZ_DELI_DATE"), "")
                drProcessChipInfo.REZ_DELI_TIME = SetNumericData(drProcessChipInfo.Item("REZ_DELI_TIME"), 0)
                drProcessChipInfo.RESULT_WAIT_END = SetStringData(drProcessChipInfo.Item("RESULT_WAIT_END"), "")
                drProcessChipInfo.UPDATE_COUNT = SetNumericData(drProcessChipInfo.Item("UPDATE_COUNT"), 0)
                drProcessChipInfo.INPUTACCOUNT = SetStringData(drProcessChipInfo.Item("INPUTACCOUNT"), "")
                If IsDBNull(drProcessChipInfo.Item("UPDATEDATE")) Then
                    drProcessChipInfo.Item("UPDATEDATE") = ""
                End If
                If IsDBNull(drProcessChipInfo.Item("STARTTIME")) Then
                    drProcessChipInfo.Item("STARTTIME") = ""
                End If
                If IsDBNull(drProcessChipInfo.Item("ENDTIME")) Then
                    drProcessChipInfo.Item("ENDTIME") = ""
                End If
                drProcessChipInfo.CUSTOMERNAME = SetStringData(drProcessChipInfo.Item("CUSTOMERNAME"), "")
                drProcessChipInfo.VEHICLENAME = SetStringData(drProcessChipInfo.Item("VEHICLENAME"), "")
                drProcessChipInfo.STATUS = SetNumericData(drProcessChipInfo.Item("STATUS"), 0)
                drProcessChipInfo.INSDID = SetStringData(drProcessChipInfo.Item("INSDID"), "")
                drProcessChipInfo.WALKIN = SetStringData(drProcessChipInfo.Item("WALKIN"), "")
                drProcessChipInfo.STOPFLG = SetStringData(drProcessChipInfo.Item("STOPFLG"), "")
                drProcessChipInfo.PREZID = SetNumericData(drProcessChipInfo.Item("PREZID"), 0)
                drProcessChipInfo.REZCHILDNO = SetNumericData(drProcessChipInfo.Item("REZCHILDNO"), 0)
                If IsDBNull(drProcessChipInfo.Item("STRDATE")) Then
                    drProcessChipInfo.STRDATE = DateTime.MinValue
                End If
                drProcessChipInfo.ACCOUNT_PLAN = SetStringData(drProcessChipInfo.Item("ACCOUNT_PLAN"), "")
                drProcessChipInfo.CANCELFLG = SetStringData(drProcessChipInfo.Item("CANCELFLG"), "")
                drProcessChipInfo.UPDATEACCOUNT = SetStringData(drProcessChipInfo.Item("UPDATEACCOUNT"), "")
                drProcessChipInfo.SVCORGNMCT = SetStringData(drProcessChipInfo.Item("SVCORGNMCT"), "")
                drProcessChipInfo.SVCORGNMCB = SetStringData(drProcessChipInfo.Item("SVCORGNMCB"), "")
                drProcessChipInfo.RELATIONSTATUS = SetStringData(drProcessChipInfo.Item("RELATIONSTATUS"), "")
                drProcessChipInfo.RELATION_UNFINISHED_COUNT = SetNumericData(drProcessChipInfo.Item("RELATION_UNFINISHED_COUNT"), 0)
                drProcessChipInfo.ORDERNO = SetStringData(drProcessChipInfo.Item("ORDERNO"), "")
                'drChipInfo.USERNAME = reserveItem.USERNAME
            Next

            OutputLog(LOG_TYPE_INFO, "[E]GetProcessChipInfo()", "", Nothing, "RETURN_VALUE:(DataSet)")
            Return processChipInfo

        End Using

    End Function

#End Region

#Region "開始処理"
    ''' <summary>
    '''   開始処理を行う
    ''' </summary>
    ''' <param name="dealerCode">販売店コード</param>
    ''' <param name="branchCode">店舗コード</param>
    ''' <param name="reserveId">予約ID</param>
    ''' <param name="stallId">ストールID</param>
    ''' <param name="updateAccount">更新アカウント</param>
    ''' <param name="orderNo">R/O No.</param>
    ''' <param name="isBreak">休憩有無(とる：True、とらない：False)</param>
    ''' <returns>正常終了：0、異常終了：エラーコード、例外：-1</returns>
    ''' <remarks></remarks>
    <EnableCommit()>
    Public Function StartWork(ByVal dealerCode As String, _
                              ByVal branchCode As String, _
                              ByVal reserveId As Integer, _
                              ByVal stallId As Integer, _
                              ByVal updateAccount As String, _
                              ByVal orderNo As String, _
                              Optional ByVal isBreak As Boolean = False) As Integer

        OutputLog(LOG_TYPE_INFO, "[S]StartWork()", "", Nothing, _
                  "DLRCD:" & dealerCode, "STRCD:" & branchCode, "REZID:" & CType(reserveId, String), _
                  "STALLID:" & CType(stallId, String), "ACCOUNT:" & updateAccount, "ORDERNO:" & orderNo)

        ' 戻り値にエラーを設定
        StartWork = RETURN_VALUE_NG

        ' SC3150101TableAdapterクラスのインスタンスを生成
        Dim adapter As SC3150101DataSetTableAdapters.SC3150101StallInfoDataTableAdapter = _
                            New SC3150101DataSetTableAdapters.SC3150101StallInfoDataTableAdapter
        Try
            ' (実際の)作業開始日時を取得する
            Dim startTime As Date = DateTimeFunc.Now(dealerCode)

            ' -------------------------------------------------------------------------------------
            ' ストール予約情報を取得する
            ' -------------------------------------------------------------------------------------
            Dim reserveInfo As SC3150101DataSet.SC3150101StallReserveInfoDataTable = _
                                    adapter.GetStallReserveInfo(dealerCode, branchCode, reserveId)
            If reserveInfo Is Nothing Then
                ' ストール予約情報の取得に失敗
                OutputLog(LOG_TYPE_ERROR, "StartWork()", _
                          "It is failed by the acquisition of the stall reservation information", Nothing)
                Exit Try
            End If
            ' DBNullの項目にデフォルト値を設定する
            reserveInfo = SetStallReserveDefaultValue(reserveInfo)
            Dim drReserveInfo As SC3150101DataSet.SC3150101StallReserveInfoRow = _
                                    DirectCast(reserveInfo.Rows(0), SC3150101DataSet.SC3150101StallReserveInfoRow)

            ' 予約ステータス(仮予約)の確認
            If drReserveInfo.STATUS = SMB_STATUS_PROPOSED_RESOURCE Then
                ' ストール仮予約のチップ
                OutputLog(LOG_TYPE_ERROR, "StartWork()", "Chip of the stall tentative reservation", Nothing)
                StartWork = 902
                Exit Try
            End If

            ' 入庫確認
            If (IsDBNull(drReserveInfo.Item("STRDATE"))) _
                OrElse (drReserveInfo.STRDATE = DateTime.MinValue) Then
                ' 未入庫のチップ
                OutputLog(LOG_TYPE_ERROR, "StartWork()", "Chip of the non-store", Nothing)
                StartWork = 901
                Exit Try
            End If

            '2012/02/28 KN 渡辺 【SERVICE_1】関連チップの順不同開始を抑制するように修正 START
            ' -------------------------------------------------------------------------------------
            ' 関連チップの順不同開始であるかのチェック
            ' -------------------------------------------------------------------------------------
            Dim resultReserveChildNo As Integer = _
                CheckReserveChildNo(dealerCode, branchCode, reserveId, drReserveInfo.REZCHILDNO)

            If (resultReserveChildNo <> 0) Then
                ' 関連チップの順不同開始エラー
                OutputLog(LOG_TYPE_ERROR, "StartWork()", "childNo random order start", Nothing)
                Exit Try
            End If
            '2012/02/28 KN 渡辺 【SERVICE_1】関連チップの順不同開始を抑制するように修正 END

            '2012/03/02 KN 西田【SERVICE_1】START
            ' -------------------------------------------------------------------------------------
            ' 追加作業の場合、TACTの情報を参照し、開始できるチップか判別する。
            ' -------------------------------------------------------------------------------------
            Dim childNoCheck As Integer = 0
            If (Not drReserveInfo.IsREZCHILDNONull) Then
                childNoCheck = CType(drReserveInfo.REZCHILDNO, Integer)
            End If

            'REZCHILDNOが0または1の場合は親作業のため更新しない
            If Not childNoCheck = 0 AndAlso Not childNoCheck = 1 Then
                'IFに渡す場合は-1する。
                childNoCheck = childNoCheck - 1
                '関連チップチェック
                If Me.IsCheckRepairStatusTact(drReserveInfo.DLRCD, orderNo, childNoCheck) <> 0 Then
                    Exit Try
                End If
            End If
            '2012/03/02 KN 西田【SERVICE_1】END

            ' -------------------------------------------------------------------------------------
            ' ストール時間を取得する
            ' -------------------------------------------------------------------------------------
            Dim stallInfo As SC3150101DataSet.SC3150101StallTimeInfoDataTable = _
                                adapter.GetStallTimeInfo(dealerCode, branchCode, stallId)
            If stallInfo Is Nothing Then
                ' ストール時間情報の取得に失敗
                OutputLog(LOG_TYPE_ERROR, "StartWork()", _
                          "It is failed by the acquisition of the stall time information", Nothing)
                Exit Try
            End If
            Dim drStallInfo As SC3150101DataSet.SC3150101StallTimeInfoRow = _
                                DirectCast(stallInfo.Rows(0), SC3150101DataSet.SC3150101StallTimeInfoRow)

            ' 稼動開始時刻
            Dim startOperationTime As TimeSpan = SetStallTime(drStallInfo.PSTARTTIME).TimeOfDay
            ' 稼動終了時刻
            Dim endOperationTime As TimeSpan = SetStallTime(drStallInfo.PENDTIME).TimeOfDay

            ' -------------------------------------------------------------------------------------
            ' 稼動時間帯を確定
            ' -------------------------------------------------------------------------------------
            Dim resultOperationTime As Integer = DecisionOperationTime(reserveInfo, startTime, _
                                                                        startOperationTime, endOperationTime)
            If resultOperationTime <> 0 Then
                ' 稼働時間外開始
                OutputLog(LOG_TYPE_ERROR, "StartWork()", "Operation overtime start", Nothing)
                Exit Try
            End If

            ' ---------------------------------------------------------------------------------
            ' ストール実績情報を取得する
            ' ---------------------------------------------------------------------------------
            Dim procInfo As SC3150101DataSet.SC3150101StallProcessInfoDataTable = _
                                adapter.GetStallProcessInfo(dealerCode, branchCode, reserveId)
            If procInfo Is Nothing Then
                ' ストール実績情報の取得に失敗
                OutputLog(LOG_TYPE_ERROR, "StartWork()", _
                          "It is failed by the acquisition of the stall results information", Nothing)
                Exit Try
            End If

            ' 日跨ぎ開始放置チップの翌日2重開始チェック(TCメイン画面では開始ボタン自体が出ないはず)
            Dim drProc As SC3150101DataSet.SC3150101StallProcessInfoRow = _
                                DirectCast(procInfo.Rows(0), SC3150101DataSet.SC3150101StallProcessInfoRow)
            If Not IsDBNull(drProc.Item("RESULT_STATUS")) _
                AndAlso SMB_RESULT_STATUS_WORKING.Equals(drProc.RESULT_STATUS) Then
                ' すでに作業開始されている
                OutputLog(LOG_TYPE_ERROR, "StartWork()", "This 'himatagi' tip is already working", Nothing)
                Exit Try
            End If

            ' ---------------------------------------------------------------------------------
            ' 二重作業開始チェック
            ' CheckMultiStart()
            ' ---------------------------------------------------------------------------------
            Dim resultMultiStarts As Integer = CheckMultiStarts(stallId, startTime, _
                                                                startOperationTime, endOperationTime)
            If resultMultiStarts <> 0 Then
                ' すでに作業開始されている
                OutputLog(LOG_TYPE_ERROR, "StartWork()", "Other tips are already working", Nothing)
                Exit Try
            End If

            ' ---------------------------------------------------------------------------------
            ' ストールの作業担当者数チェック
            ' IsStallStaffCount()
            ' ---------------------------------------------------------------------------------
            ' 2012/02/27 KN 佐藤 【SERVICE_1】スタッフストール割当の抽出条件を追加（処理修正） START
            'If IsStallStaffCount(adapter, _
            '                     startTime, _
            '                     stallId) <> RETURN_VALUE_OK Then
            '    Exit Try
            'End If
            If IsStallStaffCount(adapter, _
                                 dealerCode, _
                                 branchCode, _
                                 startTime, _
                                 stallId) <> RETURN_VALUE_OK Then
                Exit Try
            End If
            ' 2012/02/27 KN 佐藤 【SERVICE_1】スタッフストール割当の抽出条件を追加（処理修正） END

            ' ---------------------------------------------------------------------------------
            ' 指定範囲内の予約情報の取得
            ' ---------------------------------------------------------------------------------
            ' ストール開始時間
            Dim stallStartTime As TimeSpan = startOperationTime
            ' ストール終了時間
            Dim stallEndTime As TimeSpan = endOperationTime
            ' ストール予約情報の取得範囲(FROM)
            Dim fromDate As Date = startTime
            ' ストール予約情報の取得範囲(TO)
            Dim toDate As Date = GetEndDateRange(fromDate, stallStartTime, stallEndTime)
            ' 指定範囲内のストール予約情報を取得
            Dim reserveList As SC3150101DataSet.SC3150101StallReserveListDataTable = _
                                        adapter.GetStallReserveList(dealerCode, branchCode, _
                                                                    stallId, reserveId, fromDate, toDate)
            ' 指定範囲内のストール実績情報を取得
            Dim processList As SC3150101DataSet.SC3150101StallProcessListDataTable = _
                                        adapter.GetStallProcessList(dealerCode, branchCode, _
                                                                    stallId, fromDate, toDate)
            ' 指定範囲内の予約情報の取得
            reserveList = GetReserveList(reserveList, processList, stallId, _
                                         reserveId, fromDate, isBreak)


            ' ---------------------------------------------------------------------------------
            ' 休憩取得有無判定
            ' CheckBreak()
            ' ---------------------------------------------------------------------------------
            ' 休憩時間帯・使用不可時間帯取得
            Dim breakInfo As SC3150101DataSet.SC3150101StallBreakInfoDataTable = _
                                            adapter.GetBreakSlot(stallId, fromDate, toDate)

            Dim reserveStartTime As Date = drReserveInfo.STARTTIME
            Dim reserveEndTime As Date = drReserveInfo.ENDTIME
            Dim reserveWorkTime As Integer = CType(drReserveInfo.REZ_WORK_TIME, Integer)
            ' 休憩取得有無判定
            Dim resultBreak As Boolean = CheckBreak(breakInfo, isBreak, reserveStartTime, _
                                                    reserveEndTime, reserveWorkTime)


            ' 予約の作業終了予定日時を算出
            ' (実際の)予定終了時刻を算出する
            Dim workTime As Integer = reserveWorkTime
            Dim dateTemp(END_TIME_ARRAY_NUMBER) As Date
            Dim startTimeTemp As Date = startTime
            dateTemp = CalculateEndTime(stallInfo, _
                                        dealerCode, branchCode, stallId, _
                                        startTimeTemp, workTime, _
                                        resultBreak)
            Dim endTime As Date = dateTemp(END_TIME_END)

            ' 時間の見直し
            Dim dateArray(2) As Date
            dateArray = RevisionTime(startTime, endTime, CType(drStallInfo.TIMEINTERVAL, Integer))
            Dim startTimeRevision As Date = dateArray(0)
            Dim endTimeRevision As Date = dateArray(1)

            ' ---------------------------------------------------------------------------------
            ' 開始処理により干渉する移動可能な後続チップを移動させる
            ' ---------------------------------------------------------------------------------
            ' 衝突有無判定
            If IsCollision(reserveList, reserveId, startTime, endTimeRevision) = True Then

                ' 衝突チップを移動する
                Dim resultMoveChip As Integer = MoveCollisionChip(reserveList, stallInfo, breakInfo, dealerCode, _
                                                                  branchCode, reserveId, stallId, _
                                                                  startTime, endTimeRevision, updateAccount)
                ' 衝突チップ移動処理の判定
                If resultMoveChip <> RETURN_VALUE_OK Then
                    StartWork = resultMoveChip
                    Exit Try
                End If
            End If

            ' ---------------------------------------------------------------------------------

            ' 使用開始日時の設定
            reserveInfo.Rows.Item(0).Item("STARTTIME") = startTimeRevision

            ' 日跨ぎの場合予約情報のendTimeは変更しない
            ' 作業開始後に日跨ぎであるか否か
            Dim isHimatagi As Boolean = IsStartAfterIsHimatagi(startTime, endTimeRevision, _
                                                startOperationTime, endOperationTime)
            If isHimatagi = False Then
                ' 日跨ぎでない場合は使用終了日時を設定
                reserveInfo.Rows.Item(0).Item("ENDTIME") = endTimeRevision
            End If

            ' ストール予約の更新情報を設定する(必要ない気もするが既存処理で行っているので一応)
            reserveInfo.Rows.Item(0).Item("DLRCD") = drProc.DLRCD ' 販売店コード
            reserveInfo.Rows.Item(0).Item("STRCD") = drProc.STRCD ' 店舗コード
            reserveInfo.Rows.Item(0).Item("STALLID") = stallId    ' ストールID


            ' ---------------------------------------------------------------------------------
            ' 子予約連番の再割振
            ' ReorderRezChildNo()
            ' ---------------------------------------------------------------------------------
            Dim childNo As Integer = 0
            If (Not drReserveInfo.IsREZCHILDNONull) Then
                childNo = CType(drReserveInfo.REZCHILDNO, Integer)
            End If
            'childNo = ReorderReserveChildNo(dealerCode, branchCode, reserveId)
            'If childNo = -99 Then
            '    ' 子予約連番の更新に失敗
            '    OutputLog(LOG_TYPE_ERROR, "StartWork()", "It is failed by update of 'REZCHILDNO'", Nothing)
            '    Exit Try
            'End If
            ' ---------------------------------------------------------------------------------

            ' ---------------------------------------------------------------------------------
            ' ストール予約情報を更新する
            ' ---------------------------------------------------------------------------------
            If (UpdateStallReserveData(adapter, _
                                  reserveInfo, _
                                  startTime, _
                                  updateAccount, _
                                  childNo, _
                                  dealerCode, _
                                  branchCode, _
                                  reserveId) <> RETURN_VALUE_OK) Then
                Exit Try
            End If

            ' ---------------------------------------------------------------------------------
            ' ストール実績情報の登録or更新
            ' ---------------------------------------------------------------------------------
            If (UpdateStallProcessData(adapter, _
                                  procInfo, _
                                  reserveInfo, _
                                  startTime, _
                                  endTime, _
                                  drProc.SEQNO, _
                                  updateAccount) <> RETURN_VALUE_OK) Then
                Exit Try
            End If

            ' ---------------------------------------------------------------------------------
            ' 作業担当者実績の登録
            ' ---------------------------------------------------------------------------------
            If (InsertStaffStallData(adapter, _
                                    stallInfo, _
                                    startTime, _
                                    stallId, _
                                    reserveId) <> RETURN_VALUE_OK) Then
                Exit Try
            End If

            ' 2012/03/02 KN 西田【SERVICE_1】START
            ' ---------------------------------------------------------------------------------
            ' TACTの情報を更新する
            ' ---------------------------------------------------------------------------------
            '追加作業のみ更新を行う。(最後にTACT側の更新を行うことにより、IF内にコミットしても大丈夫)
            Dim childNoIFUpdate As Integer = 0
            If (Not drReserveInfo.IsREZCHILDNONull) Then
                childNoIFUpdate = CType(drReserveInfo.REZCHILDNO, Integer)
            End If

            'REZCHILDNOが0または1の場合は親作業のため更新しない
            If Not childNoIFUpdate = 0 AndAlso Not childNoIFUpdate = 1 Then
                'IFに渡す場合は-1する。
                childNoIFUpdate = childNoIFUpdate - 1
                '追加作業更新処理
                If Me.UpdateAddRepairStatus(drReserveInfo.DLRCD _
                                          , orderNo _
                                          , childNoIFUpdate) <> 0 Then
                    Exit Try
                End If
            End If
            ' 2012/03/02 KN 西田【SERVICE_1】END

            ' 正常終了
            StartWork = RETURN_VALUE_OK

        Finally
            ' 正常終了以外はロールバック
            If StartWork <> RETURN_VALUE_OK Then
                Me.Rollback = True
            End If
            ' リソースを解放
            If adapter IsNot Nothing Then
                adapter.Dispose()
                adapter = Nothing
            End If
            OutputLog(LOG_TYPE_INFO, "[E]StartWork()", "", Nothing, _
                      "RETURN_VALUE:" & StartWork.ToString(CultureInfo.CurrentCulture))
        End Try

        Return (StartWork)

    End Function

    ' 2012/02/27 KN 佐藤 【SERVICE_1】スタッフストール割当の抽出条件を追加（処理修正） START
    ''' <summary>
    ''' ストールの作業担当者数をチェック
    ''' </summary>
    ''' <param name="adapter">SC3150101StallInfoDataTableAdapter</param>
    ''' <param name="dealerCode">販売店コード</param>
    ''' <param name="branchCode">店舗コード</param>
    ''' <param name="startTime">作業開始時間</param>
    ''' <param name="stallId">ストールID</param>
    ''' <returns>処理結果</returns>
    ''' <remarks></remarks>
    Private Function IsStallStaffCount(ByVal adapter As SC3150101DataSetTableAdapters.SC3150101StallInfoDataTableAdapter, _
                                       ByVal dealerCode As String, _
                                       ByVal branchCode As String, _
                                       ByVal startTime As Date, _
                                       ByVal stallId As Integer) As Integer
        'Private Function IsStallStaffCount(ByVal adapter As SC3150101DataSetTableAdapters.SC3150101StallInfoDataTableAdapter, _
        '                                   ByVal startTime As Date, _
        '                                   ByVal stallId As Integer) As Integer
        ' 2012/02/27 KN 佐藤 【SERVICE_1】スタッフストール割当の抽出条件を追加（処理修正） END

        ' 2012/02/27 KN 佐藤 【SERVICE_1】スタッフストール割当の抽出条件を追加（処理修正） START
        OutputLog(LOG_TYPE_INFO, "[S]IsStallStaffCount()", "", Nothing, _
                  "ADAPTER:(DataTableAdapter)", _
                  "DLRCD:" & dealerCode.ToString(CultureInfo.CurrentCulture), _
                  "STRCD:" & branchCode.ToString(CultureInfo.CurrentCulture), _
                  "STARTTIME:" & startTime.ToString(CultureInfo.CurrentCulture), _
                  "STALLID:" & stallId.ToString(CultureInfo.CurrentCulture))
        ' 2012/02/27 KN 佐藤 【SERVICE_1】スタッフストール割当の抽出条件を追加（処理修正） END

        ' 戻り値にエラーを設定
        IsStallStaffCount = RETURN_VALUE_NG
        Try
            ' ストールの作業担当者数の取得
            ' 2012/02/27 KN 佐藤 【SERVICE_1】スタッフストール割当の抽出条件を追加（処理修正） START
            'Dim staffInfo As SC3150101DataSet.SC3150101StallStaffCountDataTable = adapter.GetStaffCount(startTime, stallId)
            Dim staffInfo As SC3150101DataSet.SC3150101StallStaffCountDataTable = _
                                adapter.GetStaffCount(dealerCode, branchCode, startTime, stallId)
            ' 2012/02/27 KN 佐藤 【SERVICE_1】スタッフストール割当の抽出条件を追加（処理修正） END
            If staffInfo Is Nothing OrElse staffInfo.Count = 0 Then
                ' 作業担当者情報の取得に失敗
                OutputLog(LOG_TYPE_ERROR, "IsStallStaffCount()", _
                          "It is failed by the acquisition of the work person in charge information", Nothing)
                Exit Try
            Else
                Dim drStaffInfo As SC3150101DataSet.SC3150101StallStaffCountRow = _
                                    DirectCast(staffInfo.Rows(0), SC3150101DataSet.SC3150101StallStaffCountRow)
                ' 作業担当者数の確認
                If drStaffInfo.COUNT <= 0 Then
                    ' 作業担当者がいない
                    OutputLog(LOG_TYPE_ERROR, "IsStallStaffCount()", _
                              "There is not the work person in charge", Nothing)
                    Exit Try
                End If
            End If

            ' 正常終了
            IsStallStaffCount = RETURN_VALUE_OK

        Finally
            OutputLog(LOG_TYPE_INFO, "[E]IsStallStaffCount()", "", Nothing, _
                      "RETURN_VALUE:" & IsStallStaffCount.ToString(CultureInfo.CurrentCulture))
        End Try

        Return IsStallStaffCount

    End Function

    ' 2012/03/02 KN 西田【SERVICE_1】START
    ''' <summary>
    ''' TACTデータ追加作業開始チェック（子番(追加作業)のみ）
    ''' </summary>
    ''' <param name="dlrCd">販売店コード</param>
    ''' <param name="orderNo">予約ID</param>
    ''' <param name="srvAddSeq">枝番</param>
    ''' <returns>処理結果 OK:0/NG:-1</returns>
    ''' <remarks></remarks>
    Private Function IsCheckRepairStatusTact(ByVal dlrCd As String, ByVal orderNo As String, ByVal srvAddSeq As Integer) As Integer

        Me.OutputLog(LOG_TYPE_INFO, "[S]IsCheckRepairStatusTact()", "", Nothing, _
                  "dlrCd:" & dlrCd, _
                  "orderNo:" & orderNo, _
                  "srvAddSeq:" & srvAddSeq)

        Dim rtnVal As Integer = -1

        If srvAddSeq = 1 Then
            '１番目の追加作業の場合
            '親番の情報を取得し、ステータスをチェック
            Dim dt As IC3801001DataSet.IC3801001OrderCommDataTable = Me.GetRepairOrderBaseData(dlrCd, orderNo)

            If Not IsNothing(dt) AndAlso dt.Rows.Count <> 1 Then
                Dim dRow As IC3801001DataSet.IC3801001OrderCommRow = DirectCast(dt.Rows(0), IC3801001DataSet.IC3801001OrderCommRow)

                'ステータスが7：検査完了の場合、開始可能
                If Not dRow.IsOrderStatusNull _
                   AndAlso ORDER_NO_STATUS_COMPLET.Equals(dRow.OrderStatus) Then
                    rtnVal = 0
                End If
            End If
        Else
            '２番目以降の追加作業
            '追加作業API取得
            Dim dt As IC3800804DataSet.IC3800804AddRepairStatusDataTableDataTable = Me.GetAddRepairStatusList(dlrCd, orderNo)

            '枝番（追加作業番号）が取得件数以上ない場合、データ不整合
            If Not IsNothing(dt) AndAlso srvAddSeq <= dt.Rows.Count Then
                'テーブルの配列は0からのため、-1
                Dim dRow As IC3800804DataSet.IC3800804AddRepairStatusDataTableRow _
                    = DirectCast(dt.Rows(srvAddSeq - 1), IC3800804DataSet.IC3800804AddRepairStatusDataTableRow)

                'ステータスが9：完成検査完了の場合、開始可能
                If Not dRow.IsSTATUS_Null _
                   AndAlso TACT_ADD_REPAIR_STATUS_COMPLET.Equals(dRow.STATUS_) Then
                    rtnVal = 0
                End If
            End If
        End If

        Me.OutputLog(LOG_TYPE_INFO, "[E]IsCheckRepairStatusTact()", "", Nothing, _
                  "RETURN_VALUE:" & rtnVal.ToString(CultureInfo.CurrentCulture))

        Return rtnVal
    End Function

    ''' <summary>
    ''' R/O基本情報の取得処理
    ''' </summary>
    ''' <param name="dlrCd">販売店コード</param>
    ''' <param name="orderNo">オーダーNo.</param>
    ''' <returns>R/O基本情報データテーブル</returns>
    ''' <remarks></remarks>
    Private Function GetRepairOrderBaseData(ByVal dlrCd As String, ByVal orderNo As String) As IC3801001DataSet.IC3801001OrderCommDataTable

        Me.OutputLog(LOG_TYPE_INFO, "[S]GetRepairOrderBaseData()", "", Nothing, _
                  "dlrCd:" & dlrCd, _
                  "orderNo:" & orderNo)

        Dim IC3801001 As IC3801001BusinessLogic = New IC3801001BusinessLogic

        Me.OutputLog(LOG_TYPE_INFO, "CALL IC3801001BusinessLogic.GetROBaseInfoList", "CALL", Nothing _
                                         , dlrCd, orderNo)

        Dim dt As IC3801001DataSet.IC3801001OrderCommDataTable = IC3801001.GetROBaseInfoList(dlrCd, orderNo)

        Me.OutputLog(LOG_TYPE_INFO, "[E]GetRepairOrderBaseData()", "", Nothing, _
                  "RETURN_COUNT:" & dt.Rows.Count.ToString(CultureInfo.CurrentCulture))

        Return dt
    End Function

    ''' <summary>
    ''' 追加作業ステータス情報取得処理
    ''' </summary>
    ''' <param name="dlrCd">販売店コード</param>
    ''' <param name="orderNo">オーダーNo.</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function GetAddRepairStatusList(ByVal dlrCd As String, ByVal orderNo As String) As IC3800804DataSet.IC3800804AddRepairStatusDataTableDataTable

        Me.OutputLog(LOG_TYPE_INFO, "[S]GetAddRepairStatusList()", "", Nothing, _
                  "dlrCd:" & dlrCd, _
                  "orderNo:" & orderNo)

        Dim IC3800804 As New IC3800804BusinessLogic

        Me.OutputLog(LOG_TYPE_INFO, "CALL IC3800804BusinessLogic.GetAddRepairStatusList", "CALL", Nothing _
                                         , dlrCd, orderNo)

        Dim dt As DataTable = IC3800804.GetAddRepairStatusList(dlrCd, orderNo)

        Me.OutputLog(LOG_TYPE_INFO, "[E]GetAddRepairStatusList()", "", Nothing, _
                  "RETURN_COUNT:" & dt.Rows.Count.ToString(CultureInfo.CurrentCulture))

        Return DirectCast(dt, IC3800804DataSet.IC3800804AddRepairStatusDataTableDataTable)
    End Function
    ' 2012/03/02 KN 西田【SERVICE_1】END

    ' 2012/03/01 KN 西田【SERVICE_1】START
    ''' <summary>
    ''' 追加作業の更新
    ''' </summary>
    ''' <param name="dlrCd">販売店コード</param>
    ''' <param name="orderNo">予約ID</param>
    ''' <param name="srvAddSeq">枝番</param>
    ''' <returns>処理結果</returns>
    ''' <remarks></remarks>
    Private Function UpdateAddRepairStatus(ByVal dlrCd As String, ByVal orderNo As String, ByVal srvAddSeq As Integer) As Integer

        Me.OutputLog(LOG_TYPE_INFO, "[S]UpdateAddRepairStatus()", "", Nothing, _
                  "dlrCd:" & dlrCd, _
                  "orderNo:" & orderNo, _
                  "srvAddSeq:" & srvAddSeq.ToString(CultureInfo.CurrentCulture))

        Dim IC3800805 As New IC3800805BusinessLogic

        Me.OutputLog(LOG_TYPE_INFO, "CALL IC3800805BusinessLogic.UpdateAddRepairStatus", "CALL", Nothing _
                                         , dlrCd, orderNo, srvAddSeq.ToString(CultureInfo.CurrentCulture))
        '追加作業更新処理
        Dim rtnVal As Integer = IC3800805.UpdateAddRepairStatus(dlrCd, orderNo, srvAddSeq)

        Me.OutputLog(LOG_TYPE_INFO, "[E]UpdateAddRepairStatus()", "", Nothing, _
                  "RETURN_VALUE:" & rtnVal.ToString(CultureInfo.CurrentCulture))

        Return rtnVal
    End Function

    ' 2012/03/01 KN 西田【SERVICE_1】END

    ''' <summary>
    ''' 衝突チップを移動する
    ''' </summary>
    ''' <param name="reserveList">ストール予約情報</param>
    ''' <param name="stallInfo">ストール時間情報</param>
    ''' <param name="breakInfo">休憩情報</param>
    ''' <param name="dealerCode">販売店コード</param>
    ''' <param name="branchCode">店舗コード</param>
    ''' <param name="reserveID">予約ID</param>
    ''' <param name="stallId">予約ID</param>
    ''' <param name="startTime">開始日時</param>
    ''' <param name="endTimeRevision">終了日時</param>
    ''' <param name="updateAccount">更新アカウント</param>
    ''' <returns>処理結果</returns>
    ''' <remarks></remarks>
    Private Function MoveCollisionChip(ByVal reserveList As SC3150101DataSet.SC3150101StallReserveListDataTable, _
                                       ByVal stallInfo As SC3150101DataSet.SC3150101StallTimeInfoDataTable, _
                                       ByVal breakInfo As SC3150101DataSet.SC3150101StallBreakInfoDataTable, _
                                       ByVal dealerCode As String, _
                                       ByVal branchCode As String, _
                                       ByVal reserveId As Integer, _
                                       ByVal stallId As Integer, _
                                       ByVal startTime As Date, _
                                       ByVal endTimeRevision As Date, _
                                       ByVal updateAccount As String) As Integer

        OutputLog(LOG_TYPE_INFO, "[S]MoveCollisionChip()", "", Nothing, _
                  "RESERVELIST:(DataSet)", _
                  "STALLINFO:(DataSet)", _
                  "BREAKINFO:(DataSet)", _
                  "DEALERCODE:" & dealerCode, _
                  "BRANCHCODE:" & branchCode, _
                  "RESERVEID:" & reserveId.ToString(CultureInfo.CurrentCulture), _
                  "STALLID:" & stallId.ToString(CultureInfo.CurrentCulture), _
                  "STARTTIME:" & startTime.ToString(CultureInfo.CurrentCulture), _
                  "ENDTIMEREVISION:" & endTimeRevision.ToString(CultureInfo.CurrentCulture), _
                  "UPDATEACCOUNT:" & updateAccount)

        ' 戻り値にエラーを設定
        MoveCollisionChip = RETURN_VALUE_NG
        Try
            ' 指定時間への予約の移動
            Dim reserveListTemp As SC3150101DataSet.SC3150101StallReserveListDataTable = _
                                        MoveReserve(reserveList, stallInfo, breakInfo, _
                                                    dealerCode, branchCode, reserveId, _
                                                    stallId, startTime, endTimeRevision)

            If reserveListTemp Is Nothing Then
                ' 後続チップに干渉する
                OutputLog(LOG_TYPE_ERROR, "MoveCollisionChip()", _
                          "The target chip hits the next chip", Nothing)
                MoveCollisionChip = 903
                Exit Try
            End If

            ' 時間に変更のあった予約情報の更新
            Dim result As Integer = UpdateAllReserve(reserveListTemp, _
                                                        reserveId, _
                                                        dealerCode, _
                                                        branchCode, _
                                                        stallId, _
                                                        updateAccount)
            If result < 0 Then
                ' 後続チップの移動に失敗
                OutputLog(LOG_TYPE_ERROR, "MoveCollisionChip()", _
                          "It is failed by the movement of the following chip", Nothing)
                Exit Try
            End If

            ' 正常終了
            MoveCollisionChip = RETURN_VALUE_OK

        Finally
            OutputLog(LOG_TYPE_INFO, "[E]MoveCollisionChip()", "", Nothing, _
                      "RETURN_VALUE:" & MoveCollisionChip.ToString(CultureInfo.CurrentCulture))
        End Try

        Return MoveCollisionChip

    End Function

    ''' <summary>
    ''' ストール予約を更新する
    ''' </summary>
    ''' <param name="adapter">SC3150101StallInfoDataTableAdapter</param>
    ''' <param name="reserveInfo">ストール予約情報</param>
    ''' <param name="startTime">作業開始時間</param>
    ''' <param name="updateAccount">更新アカウント</param>
    ''' <param name="childNo">子予約連番</param>
    ''' <param name="dealerCode">販売店コード</param>
    ''' <param name="branchCode">店舗コード</param>
    ''' <param name="reserveId">予約ID</param>
    ''' <returns>処理結果</returns>
    ''' <remarks></remarks>
    Private Function UpdateStallReserveData(ByVal adapter As SC3150101DataSetTableAdapters.SC3150101StallInfoDataTableAdapter, _
                                            ByVal reserveInfo As SC3150101DataSet.SC3150101StallReserveInfoDataTable, _
                                            ByVal startTime As Date, _
                                            ByVal updateAccount As String, _
                                            ByVal childNo As Integer, _
                                            ByVal dealerCode As String, _
                                            ByVal branchCode As String, _
                                            ByVal reserveId As Integer) As Integer

        OutputLog(LOG_TYPE_INFO, "[S]UpdateStallReserveData()", "", Nothing, _
                  "ADAPTER:(DataTableAdapter)", _
                  "RESERVEINFO:(DataSet)", _
                  "STARTTIME:" & startTime.ToString(CultureInfo.CurrentCulture), _
                  "UPDATEACCOUNT:" & updateAccount, _
                  "CHILDNO:" & childNo.ToString(CultureInfo.CurrentCulture), _
                  "DEALERCODE:" & dealerCode, _
                  "BRANCHCODE:" & branchCode, _
                  "RESERVEID:" & reserveId.ToString(CultureInfo.CurrentCulture))

        ' 戻り値にエラーを設定
        UpdateStallReserveData = RETURN_VALUE_NG
        Try
            ' ストール予約情報を更新する
            Dim resultUpdRez As Integer = adapter.UpdateStallReserveInfo(reserveInfo, _
                                                                            startTime, _
                                                                            Date.MinValue, _
                                                                            OVERWRITE_NEW_VALUE, _
                                                                            KEEP_CURRENT, _
                                                                            updateAccount, _
                                                                            childNo)
            If (resultUpdRez <= 0) Then
                ' ストール予約情報の更新に失敗
                OutputLog(LOG_TYPE_ERROR, "UpdateStallReserveData()", _
                          "It is failed by update of the stall reservation information", Nothing)
                Exit Try
            End If

            ' ストール予約履歴を登録する
            Dim resultInsRezHis As Integer = adapter.InsertReserveHistory(dealerCode, branchCode, reserveId, 1)
            If (resultInsRezHis <= 0) Then
                ' ストール予約履歴の登録に失敗
                OutputLog(LOG_TYPE_ERROR, "UpdateStallReserveData()", _
                          "It is failed by the registration of the stall reservation history", Nothing)
                Exit Try
            End If

            ' 正常終了
            UpdateStallReserveData = RETURN_VALUE_OK

        Finally
            OutputLog(LOG_TYPE_INFO, "[E]UpdateStallReserveData()", "", Nothing, _
                      "RETURN_VALUE:" & UpdateStallReserveData.ToString(CultureInfo.CurrentCulture))
        End Try

        Return UpdateStallReserveData

    End Function

    ''' <summary>
    ''' ストール実績の登録または更新する
    ''' </summary>
    ''' <param name="adapter">SC3150101StallInfoDataTableAdapter</param>
    ''' <param name="procInfo">ストール実績情報</param>
    ''' <param name="reserveInfo">ストール予約情報</param>
    ''' <param name="startTime">作業開始時間</param>
    ''' <param name="endTime">作業終了時間</param>
    ''' <param name="updateAccount">更新アカウント</param>
    ''' <returns>処理結果</returns>
    ''' <remarks></remarks>
    Private Function UpdateStallProcessData(ByVal adapter As SC3150101DataSetTableAdapters.SC3150101StallInfoDataTableAdapter, _
                                            ByVal procInfo As SC3150101DataSet.SC3150101StallProcessInfoDataTable, _
                                            ByVal reserveInfo As SC3150101DataSet.SC3150101StallReserveInfoDataTable, _
                                            ByVal startTime As Date, _
                                            ByVal endTime As Date, _
                                            ByVal seqNo As Decimal, _
                                            ByVal updateAccount As String) As Integer

        OutputLog(LOG_TYPE_INFO, "[S]UpdateStallProcessData()", "", Nothing, _
                  "ADAPTER:(DataTableAdapter)", _
                  "PROCINFO:(DataSet)", _
                  "RESERVEINFO:(DataSet)", _
                  "STARTTIME:" & startTime.ToString(CultureInfo.CurrentCulture), _
                  "ENDTIME:" & endTime.ToString(CultureInfo.CurrentCulture), _
                  "SEQNO:" & seqNo.ToString(CultureInfo.CurrentCulture), _
                  "UPDATEACCOUNT:" & updateAccount)

        ' 戻り値にエラーを設定
        UpdateStallProcessData = RETURN_VALUE_NG
        Try
            ' ストール実績情報の設定
            procInfo.Rows.Item(0).Item("RESULT_STATUS") = SMB_RESULT_STATUS_WORKING ' 実績_ステータス（20:作業中）
            procInfo.Rows.Item(0).Item("RESULT_START_TIME") = _
                startTime.ToString("yyyyMMddHHmm", CultureInfo.InvariantCulture())  ' 実績_ストール開始日時時刻
            procInfo.Rows.Item(0).Item("RESULT_END_TIME") = _
                endTime.ToString("yyyyMMddHHmm", CultureInfo.InvariantCulture())    ' 実績_ストール終了日時時刻

            If seqNo = 0 Then
                ' ストール実績情報を登録する
                Dim resultInsProc As Integer = adapter.InsertStallProcessInfo(procInfo, updateAccount, False, False)
                If (resultInsProc <= 0) Then
                    ' ストール実績情報の登録に失敗
                    OutputLog(LOG_TYPE_ERROR, "UpdateStallProcessData()", _
                              "It is failed by registration of the stall results information", Nothing)
                    Exit Try
                End If
            Else
                ' ストール実績情報を更新する
                Dim resultUpdProc As Integer = adapter.UpdateStallProcessInfo(procInfo, reserveInfo)
                If (resultUpdProc <= 0) Then
                    ' ストール実績情報の更新に失敗
                    OutputLog(LOG_TYPE_ERROR, "UpdateStallProcessData()", _
                              "It is failed by update of the stall results information", Nothing)
                    Exit Try
                End If
            End If

            ' 正常終了
            UpdateStallProcessData = RETURN_VALUE_OK

        Finally
            OutputLog(LOG_TYPE_INFO, "[E]UpdateStallProcessData()", "", Nothing, _
                      "RETURN_VALUE:" & UpdateStallProcessData.ToString(CultureInfo.CurrentCulture))
        End Try

        Return UpdateStallProcessData

    End Function

    ''' <summary>
    ''' ストール実績の登録または更新する
    ''' </summary>
    ''' <param name="adapter">SC3150101StallInfoDataTableAdapter</param>
    ''' <param name="stallInfo">ストール時間情報</param>
    ''' <param name="startTime">作業開始時間</param>
    ''' <param name="stallId">ストールID</param>
    ''' <param name="reserveId">予約ID</param>
    ''' <returns>処理結果</returns>
    ''' <remarks></remarks>
    Private Function InsertStaffStallData(ByVal adapter As SC3150101DataSetTableAdapters.SC3150101StallInfoDataTableAdapter, _
                                          ByVal stallInfo As SC3150101DataSet.SC3150101StallTimeInfoDataTable, _
                                          ByVal startTime As Date, _
                                          ByVal stallId As Integer, _
                                          ByVal reserveId As Integer) As Integer

        OutputLog(LOG_TYPE_INFO, "[S]InsertStaffStallData()", "", Nothing, _
                  "ADAPTER:(DataTableAdapter)", _
                  "STALLINFO:(DataSet)", _
                  "STARTTIME:" & startTime.ToString(CultureInfo.CurrentCulture), _
                  "STALLID:" & stallId.ToString(CultureInfo.CurrentCulture), _
                  "RESERVEID:" & reserveId.ToString(CultureInfo.CurrentCulture))

        ' 戻り値にエラーを設定
        InsertStaffStallData = RETURN_VALUE_NG
        Try
            ' 作業日付を取得する
            Dim staffWorkTime As Date = GetWorkDate(stallInfo, startTime)

            ' 作業担当者実績の作成
            Dim resultInsStaffStall As Integer = adapter.InsertStaffStall(stallId, reserveId, staffWorkTime)
            If (resultInsStaffStall <= 0) Then
                ' 担当者実績の登録に失敗
                OutputLog(LOG_TYPE_ERROR, "InsertStaffStallData()", _
                          "It is failed by the registration of the person in charge results", Nothing)
                Exit Try
            End If

            ' 正常終了
            InsertStaffStallData = RETURN_VALUE_OK

        Finally
            OutputLog(LOG_TYPE_INFO, "[E]InsertStaffStallData()", "", Nothing, _
                      "RETURN_VALUE:" & InsertStaffStallData.ToString(CultureInfo.CurrentCulture))
        End Try

        Return InsertStaffStallData

    End Function
#End Region

#Region "当日処理"
    ''' <summary>
    '''   当日処理を行う
    ''' </summary>
    ''' <param name="dealerCode">販売店コード</param>
    ''' <param name="branchCode">店舗コード</param>
    ''' <param name="reserveId">予約ID</param>
    ''' <param name="stallId">ストールID</param>
    ''' <param name="updateAccount">更新アカウント</param>
    ''' <param name="isBreak">休憩取得有無(True：有、False：無)</param>
    ''' <returns>正常終了：0、異常終了：エラーコード</returns>
    ''' <remarks></remarks>
    <EnableCommit()>
    Public Function SuspendWork(ByVal dealerCode As String, _
                                ByVal branchCode As String, _
                                ByVal reserveId As Integer, _
                                ByVal stallId As Integer, _
                                ByVal updateAccount As String, _
                                Optional ByVal isBreak As Boolean = False) As Integer

        OutputLog(LOG_TYPE_INFO, "[S]SuspendWork()", "", Nothing, _
                  "DLRCD:" & dealerCode, "DLRCD:" & dealerCode, "STRCD:" & branchCode, _
                  "REZID:" & reserveId.ToString(CultureInfo.CurrentCulture), _
                  "STALLID:" & stallId.ToString(CultureInfo.CurrentCulture), "ACCOUNT:" & updateAccount)

        ' 戻り値にエラーを設定
        SuspendWork = RETURN_VALUE_NG

        ' SC3150101TableAdapterクラスのインスタンスを生成
        Dim adapter As SC3150101DataSetTableAdapters.SC3150101StallInfoDataTableAdapter = _
                            New SC3150101DataSetTableAdapters.SC3150101StallInfoDataTableAdapter
        Try
            ' ストール予約情報を取得する
            Dim reserveInfo As SC3150101DataSet.SC3150101StallReserveInfoDataTable = _
                                    adapter.GetStallReserveInfo(dealerCode, branchCode, reserveId)
            If reserveInfo Is Nothing OrElse reserveInfo.Count <= 0 Then
                ' ストール予約情報の取得に失敗
                OutputLog(LOG_TYPE_ERROR, "SuspendWork()", _
                          "It is failed by the acquisition of the stall reservation information", Nothing)
                Exit Try
            End If

            ' ストール実績情報を取得する
            Dim procInfo As SC3150101DataSet.SC3150101StallProcessInfoDataTable = _
                                adapter.GetStallProcessInfo(dealerCode, branchCode, reserveId)
            If procInfo Is Nothing OrElse procInfo.Count <= 0 Then
                ' ストール実績情報の取得に失敗
                OutputLog(LOG_TYPE_ERROR, "SuspendWork()", _
                          "It is failed by the acquisition of the stall results information", Nothing)
                Exit Try
            End If

            Dim drProc As SC3150101DataSet.SC3150101StallProcessInfoRow = _
                                DirectCast(procInfo.Rows(0), SC3150101DataSet.SC3150101StallProcessInfoRow)
            ' 作業中のチップであるかチェック
            If (Not IsDBNull(drProc.Item("RESULT_STATUS")) And drProc.Item("RESULT_STATUS").Equals("0")) _
                OrElse (String.Equals(drProc.RESULT_STATUS, SMB_RESULT_STATUS_WORKING) = False) _
                OrElse (IsDBNull(drProc.Item("RESULT_STATUS"))) Then
                ' まだ作業開始されていない
                OutputLog(LOG_TYPE_ERROR, "SuspendWork()", "This chip is not yet started", Nothing)
                SuspendWork = 907
                Exit Try
            End If

            ' (実際の)作業開始日時を取得する
            ' 当日の作業開始日時
            Dim resultStartTime As Date = _
                Date.ParseExact(drProc.RESULT_START_TIME, "yyyyMMddHHmm", CultureInfo.InvariantCulture)
            ' 実績の作業予定終了時間
            Dim procEndTime As Date = _
                Date.ParseExact(drProc.RESULT_END_TIME, "yyyyMMddHHmm", CultureInfo.InvariantCulture)

            ' (実際の)作業終了日時を取得する
            Dim resultEndTime As Date = DateTimeFunc.Now(dealerCode)

            ' ストール時間を取得する
            Dim stallInfo As SC3150101DataSet.SC3150101StallTimeInfoDataTable = _
                                adapter.GetStallTimeInfo(dealerCode, branchCode, stallId)
            If stallInfo Is Nothing OrElse stallInfo.Count <= 0 Then
                ' ストール時間の取得に失敗
                OutputLog(LOG_TYPE_ERROR, "SuspendWork()", _
                          "It is failed by the acquisition of the stall time information", Nothing)
                Exit Try
            End If
            ' 翌日の時間情報を取得
            Dim drStallInfo As SC3150101DataSet.SC3150101StallTimeInfoRow = _
                                DirectCast(stallInfo.Rows(0), SC3150101DataSet.SC3150101StallTimeInfoRow)
            ' 予定作業終了時間
            Dim rezEndTime As Date = Date.ParseExact(drProc.REZ_END_TIME, "yyyyMMddHHmm", CultureInfo.InvariantCulture)
            ' ストール開始時間
            Dim stallStartTime As TimeSpan = SetStallTime(drStallInfo.PSTARTTIME).TimeOfDay
            ' ストール終了時間
            Dim stallEndTime As TimeSpan = SetStallTime(drStallInfo.PENDTIME).TimeOfDay
            ' 翌日の作業開始予定日時:rezSTime
            Dim nextDayStartTime As Date = GetNextDayStartTime(rezEndTime, stallStartTime)
            ' 翌日の予定作業時間(分):rezWTime
            Dim nextDayWorkTime As Integer = GetNextDayWorkTime(rezEndTime, stallStartTime)

            ' 作業時刻終了判定
            resultEndTime = CheckEndTime(dealerCode, branchCode, stallId, resultStartTime, _
                                         resultEndTime, procEndTime)

            ' ---------------------------------------------------------------------------------
            ' 指定範囲内のストール予約情報を取得(当日分)
            ' ---------------------------------------------------------------------------------
            ' ストール予約情報の取得範囲(FROM)
            Dim fromDate As Date = resultStartTime
            ' ストール予約情報の取得範囲(TO)
            Dim toDate As Date = GetEndDateRange(fromDate, stallStartTime, stallEndTime)
            ' 指定範囲内のストール予約情報を取得
            Dim reserveList As SC3150101DataSet.SC3150101StallReserveListDataTable = _
                                adapter.GetStallReserveList(dealerCode, branchCode, stallId, _
                                                            reserveId, fromDate, toDate)
            ' 指定範囲内のストール実績情報を取得
            Dim processList As SC3150101DataSet.SC3150101StallProcessListDataTable = _
                                adapter.GetStallProcessList(dealerCode, branchCode, stallId, _
                                                            fromDate, toDate)
            ' 指定範囲内の予約情報の取得
            reserveList = GetReserveList(reserveList, processList, stallId, reserveId, _
                                         fromDate, isBreak)
            ' 休憩時間帯・使用不可時間帯取得
            Dim breakInfo As SC3150101DataSet.SC3150101StallBreakInfoDataTable = _
                                adapter.GetBreakSlot(stallId, fromDate, toDate)

            ' ---------------------------------------------------------------------------------
            ' 指定範囲内のストール予約情報を取得(翌日分)
            ' ---------------------------------------------------------------------------------
            ' ストール予約情報の取得範囲(FROM)
            Dim fromDateOfNextDay As Date = nextDayStartTime
            ' ストール予約情報の取得範囲(TO)
            Dim toDateOfNextDay As Date = GetEndDateRange(fromDateOfNextDay, stallStartTime, stallEndTime)
            ' 指定範囲内のストール予約情報を取得
            Dim nextDayReserveList As SC3150101DataSet.SC3150101StallReserveListDataTable = _
                                            adapter.GetStallReserveList(dealerCode, branchCode, stallId, _
                                                                        reserveId, fromDateOfNextDay, _
                                                                        toDateOfNextDay)
            ' 指定範囲内のストール実績情報を取得
            Dim nextDayProcessList As SC3150101DataSet.SC3150101StallProcessListDataTable = _
                                            adapter.GetStallProcessList(dealerCode, branchCode, stallId, _
                                                                        fromDateOfNextDay, toDateOfNextDay)
            ' 指定範囲内の予約情報の取得
            nextDayReserveList = GetReserveList(nextDayReserveList, nextDayProcessList, _
                                                stallId, reserveId, fromDateOfNextDay, isBreak)
            ' 休憩時間帯・使用不可時間帯取得
            Dim nextDayBreakInfo As SC3150101DataSet.SC3150101StallBreakInfoDataTable = _
                                            adapter.GetBreakSlot(stallId, fromDateOfNextDay, toDateOfNextDay)

            ' ---------------------------------------------------------------------------------

            ' 販売店環境設定値取得(調整時間(時)取得)
            Dim envSettingInfo As SC3150101DataSet.SC3150101DealerEnvironmentSettingInfoDataTable = _
                                            adapter.GetDealerEnvironmentSettingValue(dealerCode, branchCode, _
                                                                                        C_SMB_DISPDATE_ADJUST)
            If envSettingInfo Is Nothing Then
                ' 販売店環境設定値の取得に失敗
                OutputLog(LOG_TYPE_ERROR, "SuspendWork()", _
                          "It is failed by the acquisition of the store environment set point", Nothing)
                Exit Try
            End If
            ' 稼動時間外MidFinish基準時間算出
            ' 稼動時間外MidFinish基準日時
            Dim standardTime As Date = CalculateMidFinishStandardTime(envSettingInfo, resultStartTime, _
                                                                        stallStartTime, stallEndTime)

            ' MidFinishのresultEndTimeが基準時間後の場合、resultEndTimeをストール稼動終了時間とする
            ' 作業終了時間をストール稼動終了時間とするか否かを判定する
            If IsSetWorkEndTimeToStallEndTime(standardTime) Then
                ' ここに分岐する場合、ストール稼動終了時間は翌日0:00以降
                resultEndTime = resultStartTime.AddDays(1).AddMinutes(stallEndTime.TotalMinutes)
            End If

            ' タグチェックは行わない


            ' 休憩取得有無チェック
            Dim drReserveInfo As SC3150101DataSet.SC3150101StallReserveInfoRow = _
                                    DirectCast(reserveInfo.Rows(0), SC3150101DataSet.SC3150101StallReserveInfoRow)
            '休憩取得あり (MidFinish当日分)
            Dim resultBreak As Boolean = CheckBreak(breakInfo, isBreak, resultStartTime, procEndTime, _
                                                    CType(drReserveInfo.REZ_WORK_TIME, Integer))
            '休憩取得あり (MidFinish翌日分)
            Dim nextDayResultBreak As Boolean = isBreak

            ' 当日の作業時間を算出
            Dim resultWorkTime As Integer = CalculateWorkTime(breakInfo, resultStartTime, _
                                                                resultEndTime, resultBreak)

            ' 予約の作業終了予定日時を算出
            Dim dateTemp(END_TIME_ARRAY_NUMBER) As Date
            dateTemp = CalculateEndTime(stallInfo, _
                                        dealerCode, _
                                        branchCode, _
                                        stallId, _
                                        nextDayStartTime, _
                                        nextDayWorkTime, _
                                        nextDayResultBreak)
            Dim reserveEndTime As Date = dateTemp(END_TIME_END)

            ' 予約の作業終了予定日時の見直し
            Dim reserveEndTimeRevision As Date ' (TIMEINTERVAL補正)予約の作業終了予定日時 (MidFinish翌日分)
            Dim timeDiff As Integer = CType(reserveEndTime.Minute Mod drStallInfo.TIMEINTERVAL, Integer)
            If timeDiff > 0 Then
                reserveEndTimeRevision = reserveEndTime.AddMinutes(drStallInfo.TIMEINTERVAL - timeDiff)
            Else
                reserveEndTimeRevision = reserveEndTime
            End If

            ' 翌日チップの衝突判定
            If IsCollision(nextDayReserveList, reserveId, nextDayStartTime, reserveEndTimeRevision) Then

                ' 衝突チップを移動する
                Dim resultMoveChip As Integer = MoveCollisionChip(nextDayReserveList, stallInfo, nextDayBreakInfo, _
                                                                  dealerCode, branchCode, reserveId, stallId, _
                                                                  nextDayStartTime, reserveEndTimeRevision, updateAccount)
                ' 衝突チップ移動処理の判定
                If resultMoveChip <> RETURN_VALUE_OK Then
                    SuspendWork = resultMoveChip
                    Exit Try
                End If
            End If

            ' ストール予約情報を更新およびストール予約履歴情報を登録
            If UpdateStallReserveInfoData(adapter, reserveInfo, nextDayStartTime, _
                                          reserveEndTimeRevision, nextDayWorkTime, updateAccount) <> RETURN_VALUE_OK Then
                Exit Try
            End If

            ' 当日分のストール実績情報を更新
            If UpdateStallProcessInfoData(adapter, procInfo, _
                                          resultStartTime, resultEndTime, resultWorkTime) <> RETURN_VALUE_OK Then
                Exit Try
            End If

            ' 翌日のストール実績情報を更新
            If InsertStallProcessInfoData(adapter, procInfo, updateAccount) <> RETURN_VALUE_OK Then
                Exit Try
            End If

            ' 作業日付を取得
            Dim workTime As Date = GetWorkDate(stallInfo, resultStartTime)

            ' 担当者実績情報を更新
            If UpdateStaffStallData(adapter, stallId, reserveId, workTime) <> RETURN_VALUE_OK Then
                Exit Try
            End If

            ' 正常終了
            SuspendWork = RETURN_VALUE_OK

        Finally
            ' 正常終了以外はロールバック
            If SuspendWork <> RETURN_VALUE_OK Then
                Me.Rollback = True
            End If
            ' リソースを解放
            If adapter IsNot Nothing Then
                adapter.Dispose()
                adapter = Nothing
            End If
            OutputLog(LOG_TYPE_INFO, "[E]SuspendWork()", "", Nothing, _
                      "RETURN_VALUE:" & SuspendWork.ToString(CultureInfo.CurrentCulture))
        End Try

        Return (SuspendWork)

    End Function

    ''' <summary>
    ''' ストール予約情報を更新する
    ''' </summary>
    ''' <param name="adapter">SC3150101StallInfoDataTableAdapter</param>
    ''' <param name="reserveInfo">ストール予約情報</param>
    ''' <param name="nextDayStartTime">翌日の作業開始予定日時</param>
    ''' <param name="reserveEndTimeRevision">翌日の作業終了予定日時</param>
    ''' <param name="nextDayWorkTime">翌日の予定作業時間(分)</param>
    ''' <param name="updateAccount">更新アカウント</param>
    ''' <returns>処理結果</returns>
    ''' <remarks></remarks>
    Private Function UpdateStallReserveInfoData(ByVal adapter As SC3150101DataSetTableAdapters.SC3150101StallInfoDataTableAdapter, _
                                                ByVal reserveInfo As SC3150101DataSet.SC3150101StallReserveInfoDataTable, _
                                                ByVal nextDayStartTime As Date, _
                                                ByVal reserveEndTimeRevision As Date, _
                                                ByVal nextDayWorkTime As Integer, _
                                                ByVal updateAccount As String) As Integer

        OutputLog(LOG_TYPE_INFO, "[S]UpdateStallReserveInfoData()", "", Nothing, _
                  "ADAPTER:(DataTableAdapter)", _
                  "RESERVEINFO:(DataSet)", _
                  "NEXTDAYSTARTTIME:" & nextDayStartTime.ToString(CultureInfo.CurrentCulture), _
                  "RESERVEENDTIMEREVISION:" & reserveEndTimeRevision.ToString(CultureInfo.CurrentCulture), _
                  "NEXTDAYWORKTIME:" & reserveEndTimeRevision.ToString(CultureInfo.CurrentCulture), _
                  "UPDATEACCOUNT:" & updateAccount)

        ' 戻り値にエラーを設定
        UpdateStallReserveInfoData = RETURN_VALUE_NG
        Try
            ' TBL_STALLREZINFOのUPDATE、およびTBL_STALLREZHISのINSERT
            reserveInfo.Rows.Item(0).Item("STARTTIME") = nextDayStartTime       '翌日の作業開始予定日時
            reserveInfo.Rows.Item(0).Item("ENDTIME") = reserveEndTimeRevision   '翌日の作業終了予定日時
            reserveInfo.Rows.Item(0).Item("REZ_WORK_TIME") = nextDayWorkTime    '翌日の予定作業時間(分)

            ' ストール予約情報を更新する
            Dim resultUpdRez As Integer = adapter.UpdateStallReserveInfo(reserveInfo, _
                                                                         Date.MinValue, _
                                                                         Date.MaxValue, _
                                                                         KEEP_CURRENT, _
                                                                         KEEP_CURRENT, _
                                                                         updateAccount)
            If (resultUpdRez <= 0) Then
                ' ストール予約情報の更新に失敗
                OutputLog(LOG_TYPE_ERROR, "SuspendWork()", _
                          "It is failed by update of the stall reservation information", Nothing)
                Exit Try
            End If

            Dim drStallReserveInfo As SC3150101DataSet.SC3150101StallReserveInfoRow = _
                        DirectCast(reserveInfo.Rows(0), SC3150101DataSet.SC3150101StallReserveInfoRow)

            ' ストール予約履歴を登録する
            Dim resultInsRezHis As Integer = adapter.InsertReserveHistory(drStallReserveInfo.DLRCD, _
                                                                          drStallReserveInfo.STRCD, _
                                                                          CType(drStallReserveInfo.REZID, Integer), _
                                                                          1)
            If (resultInsRezHis <= 0) Then
                ' ストール予約履歴の登録に失敗
                OutputLog(LOG_TYPE_ERROR, "SuspendWork()", _
                          "It is failed by the registration of the stall reservation history", Nothing)
                Exit Try
            End If

            ' 正常終了
            UpdateStallReserveInfoData = RETURN_VALUE_OK

        Finally
            OutputLog(LOG_TYPE_INFO, "[E]UpdateStallReserveInfoData()", "", Nothing, _
                      "RETURN_VALUE:" & UpdateStallReserveInfoData.ToString(CultureInfo.CurrentCulture))
        End Try

        Return UpdateStallReserveInfoData

    End Function

    ''' <summary>
    ''' 当日分のストール実績情報を更新する
    ''' </summary>
    ''' <param name="adapter">SC3150101StallInfoDataTableAdapter</param>
    ''' <param name="procInfo">ストール実績情報</param>
    ''' <param name="resultStartTime">実績_ストール開始日時時刻</param>
    ''' <param name="resultEndTime">実績_ストール終了日時時刻</param>
    ''' <param name="resultWorkTime">実績_実績時間</param>
    ''' <returns>処理結果</returns>
    ''' <remarks></remarks>
    Private Function UpdateStallProcessInfoData(ByVal adapter As SC3150101DataSetTableAdapters.SC3150101StallInfoDataTableAdapter, _
                                                ByVal procInfo As SC3150101DataSet.SC3150101StallProcessInfoDataTable, _
                                                ByVal resultStartTime As Date, _
                                                ByVal resultEndTime As Date, _
                                                ByVal resultWorkTime As Integer) As Integer

        OutputLog(LOG_TYPE_INFO, "[S]UpdateStallProcessInfoData()", "", Nothing, _
                 "ADAPTER:(DataTableAdapter)", _
                 "PROCINFO:(DataSet)", _
                 "RESULTSTARTTIME:" & resultStartTime.ToString(CultureInfo.CurrentCulture), _
                 "RESULTENDTIME:" & resultEndTime.ToString(CultureInfo.CurrentCulture), _
                 "RESULTWORKTIME:" & resultWorkTime.ToString(CultureInfo.CurrentCulture))

        ' 戻り値にエラーを設定
        UpdateStallProcessInfoData = RETURN_VALUE_NG
        Try
            ' 当日分のストール実績情報を更新する
            ' 実績_ステータス（98:MidFinish）
            procInfo.Rows.Item(0).Item("RESULT_STATUS") = SMB_RESULT_STATUS_MID_FINISH

            ' (当日の)実績_ストール開始日時時刻
            procInfo.Rows.Item(0).Item("RESULT_START_TIME") = _
                resultStartTime.ToString("yyyyMMddHHmm", CultureInfo.InvariantCulture())

            ' (当日の)実績_ストール終了日時時刻
            procInfo.Rows.Item(0).Item("RESULT_END_TIME") = _
                resultEndTime.ToString("yyyyMMddHHmm", CultureInfo.InvariantCulture())

            ' (当日の)実績_実績時間
            procInfo.Rows.Item(0).Item("RESULT_WORK_TIME") = resultWorkTime

            ' ストール実績情報を更新する
            Dim resultUpdProc As Integer = adapter.UpdateStallProcessInfo(procInfo, Nothing)
            If (resultUpdProc <= 0) Then
                ' ストール実績情報の更新に失敗
                OutputLog(LOG_TYPE_ERROR, "SuspendWork()", _
                          "It is failed by update of the stall results information", Nothing)
                Exit Try
            End If

            ' 正常終了
            UpdateStallProcessInfoData = RETURN_VALUE_OK

        Finally
            OutputLog(LOG_TYPE_INFO, "[E]UpdateStallProcessInfoData()", "", Nothing, _
                      "RETURN_VALUE:" & UpdateStallProcessInfoData.ToString(CultureInfo.CurrentCulture))
        End Try

        Return UpdateStallProcessInfoData

    End Function

    ''' <summary>
    ''' 翌日のストール実績情報を更新する
    ''' </summary>
    ''' <param name="adapter">SC3150101StallInfoDataTableAdapter</param>
    ''' <param name="procInfo">ストール実績情報</param>
    ''' <param name="updateAccount">更新アカウント</param>
    ''' <returns>処理結果</returns>
    ''' <remarks></remarks>
    Private Function InsertStallProcessInfoData(ByVal adapter As SC3150101DataSetTableAdapters.SC3150101StallInfoDataTableAdapter, _
                                                ByVal procInfo As SC3150101DataSet.SC3150101StallProcessInfoDataTable, _
                                                ByVal updateAccount As String) As Integer

        OutputLog(LOG_TYPE_INFO, "[S]InsertStallProcessInfoData()", "", Nothing, _
                  "ADAPTER:(DataTableAdapter)", _
                  "PROCINFO:(DataSet)", _
                  "UPDATEACCOUNT:" & updateAccount)

        ' 戻り値にエラーを設定
        InsertStallProcessInfoData = RETURN_VALUE_NG
        Try
            ' 翌日のストール実績情報の設定
            Dim daySeqNo As Integer = CType(procInfo.Rows.Item(0).Item("DSEQNO"), Integer) + 1
            procInfo.Rows.Item(0).Item("DSEQNO") = daySeqNo             ' 日跨ぎシーケンス番号：MAX(DSEQNO)+1
            procInfo.Rows.Item(0).Item("SEQNO") = 1                     ' シーケンス番号：1固定
            procInfo.Rows.Item(0).Item("RESULT_STATUS") = 10            ' 実績_ステータス（当日処理すると10）
            procInfo.Rows.Item(0).Item("RESULT_START_TIME") = Nothing   ' 実績_ストール開始日時時刻（当日処理するとNULL）
            procInfo.Rows.Item(0).Item("RESULT_END_TIME") = Nothing     ' 実績_ストール終了日時時刻（当日処理するとNULL）

            ' 翌日の分のストール実績情報を登録する
            Dim resultInsProc As Integer = adapter.InsertStallProcessInfo(procInfo, _
                                                                          updateAccount, _
                                                                          True,
                                                                          False)
            If (resultInsProc <= 0) Then
                ' 翌日分のストール実績情報の登録に失敗
                OutputLog(LOG_TYPE_ERROR, "SuspendWork()", _
                          "It is failed by registration of the stall results information for the next day", Nothing)
                Exit Try
            End If

            ' 正常終了
            InsertStallProcessInfoData = RETURN_VALUE_OK

        Finally
            OutputLog(LOG_TYPE_INFO, "[E]InsertStallProcessInfoData()", "", Nothing, _
                      "RETURN_VALUE:" & InsertStallProcessInfoData.ToString(CultureInfo.CurrentCulture))
        End Try

        Return InsertStallProcessInfoData

    End Function

    ''' <summary>
    ''' 担当者実績情報を更新する
    ''' </summary>
    ''' <param name="adapter">SC3150101StallInfoDataTableAdapter</param>
    ''' <param name="stallId">ストールID</param>
    ''' <param name="reserveId">予約ID</param>
    ''' <param name="workTime">作業日付</param>
    ''' <returns>処理結果</returns>
    ''' <remarks></remarks>
    Private Function UpdateStaffStallData(ByVal adapter As SC3150101DataSetTableAdapters.SC3150101StallInfoDataTableAdapter, _
                                          ByVal stallId As Integer, _
                                          ByVal reserveId As Integer, _
                                          ByVal workTime As Date) As Integer

        OutputLog(LOG_TYPE_INFO, "[S]UpdateStaffStallData()", "", Nothing, _
                  "ADAPTER:(DataTableAdapter)", _
                  "STALLID:" & stallId.ToString(CultureInfo.CurrentCulture), _
                  "RESERVEID:" & reserveId.ToString(CultureInfo.CurrentCulture), _
                  "WORK_TIME:" & workTime.ToString(CultureInfo.CurrentCulture))

        ' 戻り値にエラーを設定
        UpdateStaffStallData = RETURN_VALUE_NG
        Try
            ' 担当者実績情報の取得
            Dim staffResultInfo As SC3150101DataSet.SC3150101StaffResultInfoDataTable = _
                                        adapter.GetStaffResultInfo(stallId, reserveId, workTime, True)
            Dim drStaffResultInfo As SC3150101DataSet.SC3150101StaffResultInfoRow
            If staffResultInfo IsNot Nothing AndAlso staffResultInfo.Count <> 0 Then
                drStaffResultInfo = DirectCast(staffResultInfo.Rows(0), SC3150101DataSet.SC3150101StaffResultInfoRow)

                ' 実績ストール終了日時時刻
                Dim endTime As String = Nothing
                If IsDBNull(drStaffResultInfo.Item("RESULT_END_TIME")) = False Then
                    endTime = drStaffResultInfo.RESULT_END_TIME
                End If

                ' 担当者実績情報の更新
                Dim staffResult As Integer
                If (SMB_RESULT_STATUS_IN_SHED.Equals(drStaffResultInfo.RESULT_STATUS)) Then
                    ' 実績ステータス：10
                    ' 担当者ストール実績データの削除
                    staffResult = adapter.DeleteStaffStall(stallId, reserveId, _
                                                           CType(drStaffResultInfo.DSEQNO, Integer), _
                                                           CType(drStaffResultInfo.SEQNO, Integer), _
                                                           workTime)

                ElseIf (SMB_RESULT_STATUS_WORKING.Equals(drStaffResultInfo.RESULT_STATUS)) Then
                    ' 実績ステータス：20
                    If String.IsNullOrWhiteSpace(endTime) Then
                        ' 値がない場合、半角スペースを設定
                        endTime = " "
                    End If
                    ' 担当者ストール実績データの更新
                    staffResult = adapter.UpdateStaffStallAtWork(stallId, reserveId, _
                                                                 CType(drStaffResultInfo.DSEQNO, Integer), _
                                                                 CType(drStaffResultInfo.SEQNO, Integer), _
                                                                 workTime)

                Else
                    ' 実績ステータス：上記以外
                    ' 担当者ストール実績データの更新
                    staffResult = adapter.UpdateStaffStall(stallId, reserveId, _
                                                           CType(drStaffResultInfo.DSEQNO, Integer), _
                                                           CType(drStaffResultInfo.SEQNO, Integer), _
                                                           workTime, endTime)

                End If
                If (staffResult <= 0) Then
                    ' 担当者実績情報の更新に失敗
                    OutputLog(LOG_TYPE_ERROR, "SuspendWork()", _
                              "It is failed by update of the person in charge results information", Nothing)
                    Exit Try
                End If
            End If

            ' 正常終了
            UpdateStaffStallData = RETURN_VALUE_OK

        Finally
            OutputLog(LOG_TYPE_INFO, "[E]UpdateStaffStallData()", "", Nothing, _
                      "RETURN_VALUE:" & UpdateStaffStallData.ToString(CultureInfo.CurrentCulture))
        End Try

        Return UpdateStaffStallData

    End Function
#End Region


    ''' <summary>
    ''' 翌日の作業開始予定日時の取得（当日処理用）
    ''' </summary>
    ''' <param name="reserveEndTime">予定ストール終了日時</param>
    ''' <param name="stallStartTime">ストール稼動開始時刻</param>
    ''' <returns>翌日の作業開始予定日時</returns>
    ''' <remarks></remarks>
    Private Function GetNextDayStartTime(ByVal reserveEndTime As Date, _
                                         ByVal stallStartTime As TimeSpan) As Date

        OutputLog(LOG_TYPE_INFO, "[S]GetNextDayStartTime()", "", Nothing, _
                  "END_TIME:" & reserveEndTime.ToString(CultureInfo.InvariantCulture()), _
                  "START_TIME:" & stallStartTime.ToString())

        Dim nextStartTime As Date ' 翌日の作業開始予定日時

        nextStartTime = reserveEndTime.Date.Add(stallStartTime)

        OutputLog(LOG_TYPE_INFO, "[E]GetNextDayStartTime()", "", Nothing, _
                  "RETURN_VALUE:" & nextStartTime.ToString(CultureInfo.InvariantCulture()))

        Return nextStartTime
    End Function


    ''' <summary>
    ''' 翌日の予定作業時間(分)の取得（当日処理用）
    ''' </summary>
    ''' <param name="reserveEndTime">予定ストール終了日時</param>
    ''' <param name="stallStartTime">ストール稼動開始時刻</param>
    ''' <returns>翌日の予定作業時間(分)</returns>
    ''' <remarks></remarks>
    Private Function GetNextDayWorkTime(ByVal reserveEndTime As Date, _
                                        ByVal stallStartTime As TimeSpan) As Integer

        OutputLog(LOG_TYPE_INFO, "[S]GetNextDayWorkTime()", "", Nothing, _
                  "END_TIME:" & reserveEndTime.ToString(CultureInfo.InvariantCulture()), _
                  "START_TIME:" & stallStartTime.ToString())

        Dim duration As TimeSpan
        Dim nextDayWorkTimeHour As Integer
        Dim nextDayWorkTimeMinute As Integer
        Dim nextDayWorkTime As Integer ' 翌日の予定作業時間(分)
        Dim nextDayStartTime As Date ' 翌日の作業開始予定日時

        nextDayStartTime = reserveEndTime.Date.Add(stallStartTime)
        duration = reserveEndTime.Subtract(nextDayStartTime)
        nextDayWorkTimeHour = duration.Hours
        nextDayWorkTimeMinute = duration.Minutes
        nextDayWorkTime = (nextDayWorkTimeHour * 60) + nextDayWorkTimeMinute

        OutputLog(LOG_TYPE_INFO, "[E]GetNextDayWorkTime()", "", Nothing, _
                  "RETURN_VALUE:" & CType(nextDayWorkTime, String))

        Return nextDayWorkTime
    End Function


    ''' <summary>
    ''' 作業終了時刻判定
    ''' 作業開始時刻と作業終了時刻の稼動時間帯が異なる場合、終了時刻を作業予定終了時刻にする
    ''' </summary>
    ''' <param name="dealerCode">販売店コード</param>
    ''' <param name="branchCode">店舗コード</param>
    ''' <param name="stallId">ストールID</param>
    ''' <param name="startTime">作業開始時間</param>
    ''' <param name="endTime">作業終了時間</param>
    ''' <param name="procEndTime">実績の作業予定終了時間</param>
    ''' <returns>終了時間</returns>
    ''' <remarks></remarks>
    Public Function CheckEndTime(ByVal dealerCode As String, _
                                 ByVal branchCode As String, _
                                 ByVal stallId As Integer, _
                                 ByVal startTime As Date, _
                                 ByVal endTime As Date, _
                                 ByVal procEndTime As Date) As Date

        'Logger.Info("[S]CheckEndTime()")
        OutputLog(LOG_TYPE_INFO, "[S]CheckEndTime()", "", Nothing, _
                  "DLRCD:" & dealerCode, "STRCD:" & branchCode, "STALLID:" & CType(stallId, String), _
                  "START_TIME:" & startTime.ToString(CultureInfo.CurrentCulture()), _
                  "END_TIME:" & endTime.ToString(CultureInfo.CurrentCulture()), _
                  "PROC_END_TIME:" & procEndTime.ToString(CultureInfo.CurrentCulture()))


        Dim retEndTime As Date


        Dim adapter As New SC3150101DataSetTableAdapters.SC3150101StallInfoDataTableAdapter

        Try
            ' ストール時間を取得する
            Dim stallInfo As SC3150101DataSet.SC3150101StallTimeInfoDataTable
            stallInfo = adapter.GetStallTimeInfo(dealerCode, branchCode, stallId)

            Dim drStallInfo As SC3150101DataSet.SC3150101StallTimeInfoRow
            drStallInfo = CType(stallInfo.Rows(0), SC3150101DataSet.SC3150101StallTimeInfoRow)

            Dim operationStartTime As TimeSpan ' 稼働開始時間
            Dim operationEndTime As TimeSpan   ' 稼働終了時間
            operationStartTime = SetStallTime(drStallInfo.PSTARTTIME).TimeOfDay
            operationEndTime = SetStallTime(drStallInfo.PENDTIME).TimeOfDay

            Dim sTimeKadoStart As Date ' 稼動開始時刻(開始)
            Dim eTimeKadoStart As Date ' 稼動開始時刻(終了)
            If startTime.Add(operationStartTime) < startTime.Add(operationEndTime) Then
                ' 通常稼動の場合は単に日付の差異をチェック
                If startTime.Date <> endTime.Date Then
                    endTime = procEndTime
                End If
            Else
                ' 日跨ぎ稼動の場合は、開始・終了ごとの稼動開始時刻を取得
                ' 開始時刻
                If (startTime.Date.AddDays(-1).Add(operationStartTime) <= startTime) _
                    And (startTime < startTime.Date.Add(operationEndTime)) Then
                    sTimeKadoStart = startTime.Date.AddDays(-1).Add(operationStartTime)
                Else
                    sTimeKadoStart = startTime.Date.Add(operationStartTime)
                End If
                ' 終了時刻
                If (endTime.Date.AddDays(-1).Add(operationStartTime) <= endTime) _
                    And (endTime < endTime.Date.Add(operationEndTime)) Then
                    eTimeKadoStart = endTime.Date.AddDays(-1).Add(operationStartTime)
                Else
                    eTimeKadoStart = endTime.Date.Add(operationStartTime)
                End If

                If sTimeKadoStart.Date <> eTimeKadoStart.Date Then
                    endTime = procEndTime
                End If
            End If

            retEndTime = endTime

        Finally
            ' adapterを破棄する
            If adapter IsNot Nothing Then
                adapter.Dispose()
            End If
        End Try

        'Logger.Info("[E]CheckEndTime()")
        OutputLog(LOG_TYPE_INFO, "[E]CheckEndTime()", "", Nothing, _
                  "RETURN_VALUE:" & retEndTime.ToString(CultureInfo.CurrentCulture()))
        Return (retEndTime)

    End Function


    ''' <summary>
    ''' 作業日付取得
    ''' 日跨ぎ稼動の場合は作業日付を-1日する
    ''' </summary>
    ''' <param name="stallTimeInfo">ストール時間情報</param>
    ''' <param name="procDate">作業開始時間</param>
    ''' <returns>作業日付</returns>
    ''' <remarks></remarks>
    Public Function GetWorkDate(ByVal stallTimeInfo As SC3150101DataSet.SC3150101StallTimeInfoDataTable, _
                                ByVal procDate As Date) As Date

        OutputLog(LOG_TYPE_INFO, "[S]GetWorkDate()", "", Nothing, _
                  "STALL_TIME_INFO:(DataSet)", _
                  "PROC_DATE:" & procDate.ToString(CultureInfo.CurrentCulture()))

        Dim workDate As Date

        If stallTimeInfo Is Nothing Then
            OutputLog(LOG_TYPE_INFO, "[E]GetWorkDate()", "", Nothing, _
                  "RETURN_VALUE:" & procDate.ToString(CultureInfo.CurrentCulture()))
            Return procDate
        End If

        Dim drStallTimeInfo As SC3150101DataSet.SC3150101StallTimeInfoRow
        drStallTimeInfo = CType(stallTimeInfo.Rows(0), SC3150101DataSet.SC3150101StallTimeInfoRow)

        '稼動時間帯を取得
        Dim operationStartTimet As TimeSpan
        Dim operationEndTime As TimeSpan
        operationStartTimet = SetStallTime(drStallTimeInfo.PSTARTTIME).TimeOfDay
        operationEndTime = SetStallTime(drStallTimeInfo.PENDTIME).TimeOfDay

        workDate = procDate
        'WORKDATEの値を確定
        If procDate.Date.Add(operationStartTimet) > procDate.Date.Add(operationEndTime) Then
            '日跨ぎ稼動の場合、前日か当日かどちらの稼働時間帯かを判定
            If (procDate.Date.AddDays(-1).Add(operationStartTimet) <= procDate) And _
               (procDate < procDate.Date.Add(operationEndTime)) Then
                '前日の稼動時間帯なら-1日する
                workDate = procDate.AddDays(-1)
            End If
        End If

        OutputLog(LOG_TYPE_INFO, "[E]GetWorkDate()", "", Nothing, _
                  "RETURN_VALUE:" & workDate.ToString(CultureInfo.CurrentCulture()))
        Return workDate

    End Function


    ''' <summary>
    ''' 指定範囲時間の終了時間を取得
    ''' </summary>
    ''' <param name="fromDate">範囲(FROM)</param>
    ''' <param name="procStartTime">開始時間</param>
    ''' <param name="procEndTime">終了時間</param>
    ''' <returns>範囲(TO)</returns>
    ''' <remarks></remarks>
    Private Function GetEndDateRange(ByVal fromDate As Date, _
                                     ByVal procStartTime As TimeSpan, _
                                     ByVal procEndTime As TimeSpan) As Date

        OutputLog(LOG_TYPE_INFO, "[S]GetEndDateRange()", "", Nothing, _
                  "FROM_DATE:" & fromDate.ToString(CultureInfo.CurrentCulture()), _
                  "PROC_START_TIME:" & procStartTime.ToString(), _
                  "PROC_END_TIME:" & procEndTime.ToString())

        Dim toDate As Date

        '日跨ぎ稼動の場合
        If fromDate.Date.Add(procStartTime) > fromDate.Date.Add(procEndTime) Then
            '日跨ぎ稼動の場合、前日か当日かどちらの稼働時間帯かを判定
            If fromDate.Date.AddDays(-1).Add(procStartTime) <= fromDate _
                And fromDate < fromDate.Date.Add(procEndTime) Then
                toDate = fromDate.Date.Add(procEndTime)
            Else
                toDate = fromDate.Date.AddDays(1).Add(procEndTime)
            End If
        Else
            toDate = New Date(fromDate.Year, fromDate.Month, fromDate.Day, 23, 59, 59)
        End If

        OutputLog(LOG_TYPE_INFO, "[E]GetEndDateRange()", "", Nothing, _
                  "RETURN_VALUE:" & toDate.ToString(CultureInfo.CurrentCulture()))
        Return toDate

    End Function


    ''' <summary>
    ''' 指定範囲内の予約情報の取得
    ''' ToDateを指定しない場合に本メソッドでToDateを確定する
    ''' </summary>
    ''' <param name="reserveList">予約情報</param>
    ''' <param name="processList">実績情報</param>
    ''' <param name="stallID">ストールID</param>
    ''' <param name="reserveId">予約ID</param>
    ''' <param name="fromDate">範囲時間(FROM)</param>
    ''' <returns>処理結果</returns>
    ''' <remarks></remarks>
    Public Function GetReserveList(ByVal reserveList As SC3150101DataSet.SC3150101StallReserveListDataTable, _
                                   ByVal processList As SC3150101DataSet.SC3150101StallProcessListDataTable, _
                                   ByVal stallId As Integer, _
                                   ByVal reserveId As Integer, _
                                   ByVal fromDate As Date, _
                                   ByVal isBreak As Boolean) As SC3150101DataSet.SC3150101StallReserveListDataTable

        'Logger.Info("[S]GetReserveList()")
        OutputLog(LOG_TYPE_INFO, "[S]GetReserveList()", "", Nothing, _
                  "REZ_INFO:(DataSet)", "PROC_INFO:(DataSet)", _
                  "REZID:" & CType(reserveId, String), "STALLID:" & CType(stallId, String), _
                  "FROM_DATE:" & fromDate.ToString(CultureInfo.CurrentCulture()))

        ' 引数チェック
        If reserveList Is Nothing Then
            OutputLog(LOG_TYPE_INFO, "[E]GetReserveList()", "", Nothing)
            Return (reserveList)
        End If

        Dim reserveItem As SC3150101DataSet.SC3150101StallReserveListRow

        For Each reserveItem In reserveList.Rows

            reserveItem.Movable = "1"
            If CType(reserveItem.REZ_RECEPTION, Integer) = 0 Then
                'If reserveItem.RezStatus = 1 Then
                If reserveItem.STATUS = SMB_STATUS_COMMITE_RESOURCE Then
                    reserveItem.Movable = "0"
                End If
            Else
                If IsDBNull(reserveItem.Item("REZ_PICK_DATE")) Then
                    ' このスコープに入ってきた時は基本的にデータがないことは無いはずだが、
                    '稀に存在するのでとりあえず値を入れておく
                    reserveItem.REZ_PICK_DATE = Date.MinValue.ToString("yyyyMMddHHmm", _
                                                                       CultureInfo.CurrentCulture())
                End If
                If IsDBNull(reserveItem.Item("REZ_DELI_DATE")) Then
                    ' このスコープに入ってきた時は基本的にデータがないことは無いはずだが、
                    '稀に存在するのでとりあえず値を入れておく
                    reserveItem.REZ_DELI_DATE = Date.MinValue.ToString("yyyyMMddHHmm", _
                                                                       CultureInfo.CurrentCulture())
                End If
                If reserveItem.STARTTIME < Date.ParseExact(reserveItem.REZ_PICK_DATE, "yyyyMMddHHmm", Nothing) _
                    Or reserveItem.ENDTIME > Date.ParseExact(reserveItem.REZ_DELI_DATE, "yyyyMMddHHmm", Nothing) Then
                    reserveItem.Movable = "0"
                End If
            End If
            ' 次世代で追加
            If isBreak Then
                reserveItem.InBreak = "1"
            Else
                reserveItem.InBreak = "0"
            End If
        Next

        ' DBNullの実績データにデフォルト値をセットする
        processList = SetStallProcessListDefaultValue(processList)

        Dim processItem As SC3150101DataSet.SC3150101StallProcessListRow
        Dim drRezList() As SC3150101DataSet.SC3150101StallReserveListRow
        For Each processItem In processList.Rows

            'drRezList = reserveList.Select("REZID = " & processItem.REZID)
            drRezList = CType(reserveList.Select("REZID = " & processItem.REZID),  _
                              SC3150101DataSet.SC3150101StallReserveListRow())
            'RezItem = _ReserveList.Item(processItem.REZID)

            drRezList(0).ProcStatus = processItem.RESULT_STATUS
            If CType(drRezList(0).ProcStatus, Integer) >= CType(SMB_RESULT_STATUS_WORKING, Integer) Then
                drRezList(0).STARTTIME = Date.ParseExact(processItem.RESULT_START_TIME, "yyyyMMddHHmm", Nothing)
                drRezList(0).ENDTIME = Date.ParseExact(processItem.RESULT_END_TIME, "yyyyMMddHHmm", Nothing)
                drRezList(0).Movable = "0"
            End If

        Next

        'Logger.Info("[E]GetReserveList()")
        OutputLog(LOG_TYPE_INFO, "[E]GetReserveList()", "", Nothing, _
                  "RETURN_VALUE:" & CType(reserveList.Count, String))
        Return (reserveList)

    End Function


    ''' <summary>
    ''' 作業時間の計算
    ''' </summary>
    ''' <param name="breakList">休憩時間帯・使用不可時間帯情報</param>
    ''' <param name="startTime">作業開始日時</param>
    ''' <param name="endTime">作業終了日時</param>
    ''' <param name="isBreak">休憩取得有無</param>
    ''' <returns>実作業時間</returns>
    ''' <remarks></remarks>
    Public Function CalculateWorkTime(ByVal breakList As SC3150101DataSet.SC3150101StallBreakInfoDataTable, _
                                      ByVal startTime As Date, _
                                      ByVal endTime As Date, _
                                      ByVal isBreak As Boolean) As Integer

        'Logger.Info("[S]calculateWorkTime()")
        OutputLog(LOG_TYPE_INFO, "[S]CalculateWorkTime()", "", Nothing, _
                  "BREAK_INFO:(DataSet)", _
                  "START_TIME:" & startTime.ToString(CultureInfo.CurrentCulture()), _
                  "END_TIME:" & endTime.ToString(CultureInfo.CurrentCulture()), _
                  "BREAK_FLG:" & CType(isBreak, String))

        Dim workTime As Integer
        Dim breakTime As Integer
        Dim breakStartTime As Date
        Dim breakEndTime As Date

        workTime = CType(endTime.Subtract(startTime).TotalMinutes, Integer)

        ' 引数チェック
        If breakList Is Nothing Then
            OutputLog(LOG_TYPE_INFO, "[E]CalculateWorkTime()", "", Nothing, _
                  "RETURN_VALUE:" & CType(workTime, String))
            Return workTime
        End If

        If isBreak = True Then

            Dim breakItem As SC3150101DataSet.SC3150101StallBreakInfoRow
            For Each breakItem In breakList.Rows 'For i As Integer = 1 To breakList.Count

                'Dim breakItem As SC3150101DataSet.SC3150101StallBreakInfoRow
                'breakItem = CType(breakList.Rows(i - 1), SC3150101DataSet.SC3150101StallBreakInfoRow)

                breakStartTime = ParseDate(startTime.ToString("yyyyMMdd", _
                                                              CultureInfo.CurrentCulture()) & _
                                                          breakItem.STARTTIME)
                breakEndTime = ParseDate(startTime.ToString("yyyyMMdd", _
                                                            CultureInfo.CurrentCulture()) & _
                                                        breakItem.ENDTIME)

                If breakStartTime >= endTime Then
                    Exit For
                End If

                If breakEndTime > startTime Then
                    If breakStartTime <= startTime Then
                        If breakEndTime <= endTime Then
                            breakTime = CType(breakEndTime.Subtract(startTime).TotalMinutes, Integer)
                        Else
                            breakTime = CType(endTime.Subtract(startTime).TotalMinutes, Integer)
                        End If
                    Else
                        If breakEndTime <= endTime Then
                            breakTime = CType(breakEndTime.Subtract(breakStartTime).TotalMinutes, Integer)
                        Else
                            breakTime = CType(endTime.Subtract(breakStartTime).TotalMinutes, Integer)
                        End If
                    End If
                    workTime = workTime - breakTime

                End If
            Next
        End If


        'Logger.Info("[E]calculateWorkTime()")
        OutputLog(LOG_TYPE_INFO, "[E]CalculateWorkTime()", "", Nothing, _
                  "RETURN_VALUE:" & CType(workTime, String))
        Return workTime

    End Function


    ''' <summary>
    ''' 作業開始日時の計算
    ''' (規約により参照型引数が使えないので一旦Date型配列にしてから必要な値を戻す)
    ''' </summary>
    ''' <param name="breakInfo">休憩情報</param>
    ''' <param name="endTime">作業終了日時</param>
    ''' <param name="workTime">作業予定時間</param>
    ''' <param name="isBreak">休憩取得有無</param>
    ''' <returns>作業開始日時</returns>
    ''' <remarks></remarks>
    Public Function CalculateStartTime(ByVal breakInfo As SC3150101DataSet.SC3150101StallBreakInfoDataTable, _
                                       ByVal endTime As Date, _
                                       ByVal workTime As Integer, _
                                       ByVal isBreak As Boolean) As Date()

        OutputLog(LOG_TYPE_INFO, "[S]CalculateStartTime()", "", Nothing, _
                  "BREAK_INFO:(DataSet)", _
                  "START_TIME:" & endTime.ToString(CultureInfo.CurrentCulture()), _
                  "WORK_TIME:" & CType(workTime, String), "BREAK_FLG:" & CType(isBreak, String))


        Dim dateArray(START_TIME_ARRAY_NUMBER) As Date
        Dim startTime As Date
        Dim breakTime As Integer
        Dim breakStartTime As Date
        Dim breakEndTime As Date
        Dim drBreakInfo As SC3150101DataSet.SC3150101StallBreakInfoRow

        startTime = endTime.AddMinutes(workTime * -1)

        ' 引数チェック
        If breakInfo Is Nothing Then
            dateArray(START_TIME_START) = startTime
            dateArray(START_TIME_END) = endTime

            OutputLog(LOG_TYPE_INFO, "[E]CalculateStartTime()", "", Nothing)
            Return dateArray
        End If

        If isBreak = True Then
            For Each drBreakInfo In breakInfo.Rows
                breakStartTime = HHMMTextToDateTime(StringValueOfDB(drBreakInfo.STARTTIME).Trim())
                breakEndTime = HHMMTextToDateTime(StringValueOfDB(drBreakInfo.ENDTIME).Trim())
                If breakEndTime <= startTime Then
                    Exit For
                End If
                If breakStartTime < endTime Then
                    If breakEndTime > endTime Then
                        breakTime = CType(breakStartTime.Subtract(endTime).TotalMinutes, Integer)
                        endTime = breakStartTime
                    Else
                        breakTime = CType(breakStartTime.Subtract(breakEndTime).TotalMinutes, Integer)
                    End If
                    startTime = startTime.AddMinutes(breakTime)
                End If
            Next
        End If

        dateArray(START_TIME_START) = startTime
        dateArray(START_TIME_END) = endTime

        OutputLog(LOG_TYPE_INFO, "[E]CalculateStartTime()", "", Nothing)
        Return dateArray

    End Function


    ''' <summary>
    ''' 作業終了時間の計算
    ''' (規約により参照型引数が使えないので一旦Date型配列にしてから必要な値を戻す)
    ''' </summary>
    ''' <param name="stallTimeInfo">ストール情報</param>
    ''' <param name="dealerCode">販売店コード</param>
    ''' <param name="branchCode">店舗コード</param>
    ''' <param name="stallId">ストールID</param>
    ''' <param name="startTime">作業開始日時</param>
    ''' <param name="workTime">作業時間</param>
    ''' <param name="isBreak">休憩取得有無</param>
    ''' <returns>実作業時間</returns>
    ''' <remarks></remarks>
    Public Function CalculateEndTime(ByVal stallTimeInfo As SC3150101DataSet.SC3150101StallTimeInfoDataTable, _
                                     ByVal dealerCode As String, _
                                     ByVal branchCode As String, _
                                     ByVal stallId As Integer, _
                                     ByVal startTime As Date, _
                                     ByVal workTime As Integer, _
                                     ByVal isBreak As Boolean) As Date()

        'Logger.Info("[S]calculateEndTime()")
        OutputLog(LOG_TYPE_INFO, "[S]CalculateEndTime()", "", Nothing, _
                  "STALL_TIME_INFO:(DataSet)", "CLRCD:" & dealerCode, _
                  "STRCD:" & branchCode, "STALLID:" & CType(stallId, String), _
                  "START_TIME:" & startTime.ToString(CultureInfo.CurrentCulture()), _
                  "WORK_TIME:" & CType(workTime, String), "BREAK_FLG:" & CType(isBreak, String))

        Dim dateArray(END_TIME_ARRAY_NUMBER) As Date
        Dim endTime As Date
        'Dim breakTime As Integer


        'Try
        'Call ConvertToStallDateTime(startTime, stallDate, stallTime)
        Dim stallDateTime(STALL_DATE_ARRAY_NUMBER) As Date
        stallDateTime = ConvertToStallDateTime(stallTimeInfo, startTime)
        Dim stallDate As Date
        Dim stallTime As Date
        'Dim stallDate As Date = ConvertToStallDate(stallTimeInfo, startTime)
        'Dim stallTime As Date = ConvertToStallTime(stallTimeInfo, startTime)
        stallDate = stallDateTime(STALL_START_DATE)
        stallTime = stallDateTime(STALL_START_TIME)

        Dim chipDate(WORK_DATE_ARRAY_NUMBER) As Date

        ' 
        chipDate = SimulateChipPutting(stallTimeInfo, _
                                       dealerCode, _
                                       branchCode, _
                                       stallId, _
                                       stallDate, _
                                       stallTime, _
                                       workTime, _
                                       isBreak)

        Dim chipStartDate As Date = chipDate(WORK_START_DATE)
        Dim chipStartTime As Date = chipDate(WORK_START_TIME)
        Dim chipEndTime As Date = chipDate(WORK_END_TIME)

        startTime = chipStartDate.AddTicks(chipStartTime.Ticks())
        endTime = chipStartDate.AddTicks(chipEndTime.Ticks())

        dateArray(END_TIME_END) = endTime
        dateArray(END_TIME_START) = startTime

        OutputLog(LOG_TYPE_INFO, "[E]CalculateEndTime()", "", Nothing)
        Return dateArray

        'Catch ex As Exception
        '    Throw
        'End Try

        OutputLog(LOG_TYPE_INFO, "[E]CalculateEndTime()", "", Nothing)
        Return dateArray

    End Function


    ''' <summary>
    ''' 作業終了時間をストール稼動終了時間とするか否かを算出する
    ''' </summary>
    ''' <param name="standardTime">稼動時間外MidFinish基準日時</param>
    ''' <returns>作業終了時間をストール稼動終了時間とする場合True、それ以外False</returns>
    ''' <remarks></remarks>
    Public Function IsSetWorkEndTimeToStallEndTime(ByVal standardTime As Date) As Boolean

        Dim nowTime As Date
        Dim booleanResult As Boolean = False

        'Logger.Info("[S]IsSetWorkEndTimeToStallEndTime()")
        OutputLog(LOG_TYPE_INFO, "[S]IsSetWorkEndTimeToStallEndTime()", "", Nothing, _
                  "STD_TIME:" & standardTime.ToString(CultureInfo.InvariantCulture()))

        nowTime = DateTime.Now

        If standardTime <= nowTime Then
            '現在時間が基準時間を超える場合
            booleanResult = True
        End If

        'Logger.Info("[E]IsSetWorkEndTimeToStallEndTime()")
        OutputLog(LOG_TYPE_INFO, "[E]IsSetWorkEndTimeToStallEndTime()", "", Nothing, _
                  "RETURN_VALUE:" & CType(booleanResult, String))
        Return booleanResult

    End Function


    ''' <summary>
    ''' 稼動時間外MidFinish基準時間算出
    ''' </summary>
    ''' <param name="environmentSettingInfo">環境設定情報</param>
    ''' <param name="startTime">作業開始日時</param>
    ''' <param name="stallStartTime">ストール稼動開始時間</param>
    ''' <param name="stallEndTime">ストール稼動終了時間</param>
    ''' <returns>稼動時間外MidFinish基準日時</returns>
    ''' <remarks></remarks>
    Public Function CalculateMidFinishStandardTime(ByVal environmentSettingInfo As SC3150101DataSet.SC3150101DealerEnvironmentSettingInfoDataTable, _
                                                   ByVal startTime As Date, _
                                                   ByVal stallStartTime As TimeSpan, _
                                                   ByVal stallEndTime As TimeSpan) As Date

        'Logger.Info("[S]CalculateMidFinishStandardTime()")
        OutputLog(LOG_TYPE_INFO, "[S]CalculateMidFinishStandardTime()", "", Nothing, _
                  "ENV_SET_INFO:(DataSet)", _
                  "START_TIME:" & startTime.ToString(CultureInfo.CurrentCulture()), _
                  "STALL_START_TIME:" & stallStartTime.ToString(), _
                  "STALL_END_TIME:" & stallEndTime.ToString())


        Dim midFinishStandardTime As Date
        Dim standardTimeAdjustHour As Integer
        Dim drEnvSettingInfo As SC3150101DataSet.SC3150101DealerEnvironmentSettingInfoRow
        Dim standardTimeAdjust As String

        If environmentSettingInfo Is Nothing Then
            standardTimeAdjustHour = 0
        Else
            '調整時間(時)取得
            'Dim drEnvSettingInfo As SC3150101DataSet.SC3150101DealerEnvironmentSettingInfoRow
            drEnvSettingInfo = CType(environmentSettingInfo.Rows(0),  _
                                     SC3150101DataSet.SC3150101DealerEnvironmentSettingInfoRow)
            'Dim standardTimeAdjust As String
            standardTimeAdjust = drEnvSettingInfo.PARAMVALUE

            'If standardTimeAdjust.Trim() = "" Then
            If String.IsNullOrWhiteSpace(standardTimeAdjust) Then
                standardTimeAdjustHour = 0
            ElseIf Not IsNumeric(standardTimeAdjust) Then
                standardTimeAdjustHour = 0
            Else
                standardTimeAdjustHour = CType(standardTimeAdjust, Integer) * 1
            End If
        End If

        '基準時間算出
        If stallStartTime.TotalMinutes < stallEndTime.TotalMinutes Then
            '稼動終了時間<0:00の場合
            '基準時間は当日24:00
            midFinishStandardTime = startTime.AddDays(1)
        Else
            '稼動終了時間>=0:00の場合
            midFinishStandardTime = startTime.AddDays(1).AddMinutes(stallStartTime.TotalMinutes).AddHours(standardTimeAdjustHour * -1)

            If midFinishStandardTime < startTime.AddDays(1).AddMinutes(stallEndTime.TotalMinutes) Then
                midFinishStandardTime = startTime.AddDays(1).AddMinutes(stallEndTime.TotalMinutes)
            End If
        End If

        'Logger.Info("[E]CalculateMidFinishStandardTime()")
        OutputLog(LOG_TYPE_INFO, "[E]CalculateMidFinishStandardTime()", "", Nothing, _
                  "RETURN_VALUE:" & midFinishStandardTime.ToString(CultureInfo.CurrentCulture()))
        Return midFinishStandardTime

    End Function


    ''' <summary>
    ''' 作業開始後に日跨ぎであるか否か
    ''' </summary>
    ''' <param name="startTime">実績開始時間</param>
    ''' <param name="endTime">実績開始時間から算出した終了日時</param>
    ''' <param name="operationStartTime">稼動開始時間</param>
    ''' <param name="operationEndTime">稼動終了時間</param>
    ''' <returns>日跨ぎ：true、非日跨ぎ：false</returns>
    ''' <remarks></remarks>
    Private Function IsStartAfterIsHimatagi(ByVal startTime As Date, _
                                            ByVal endTime As Date, _
                                            ByVal operationStartTime As TimeSpan, _
                                            ByVal operationEndTime As TimeSpan) As Boolean

        OutputLog(LOG_TYPE_INFO, "[S]IsStartAfterIsHimatagi()", "", Nothing, _
                  "START_TIME:" & startTime.ToString(CultureInfo.InvariantCulture()), _
                  "END_TIME:" & endTime.ToString(CultureInfo.InvariantCulture()), _
                  "KADO_START_TIME:" & operationStartTime.ToString(), _
                  "KADO_END_TIME:" & operationEndTime.ToString())

        Dim afterStartIsHimatagi As Boolean
        afterStartIsHimatagi = False


        ' 開始後、日跨ぎ
        If operationStartTime.TotalMinutes < operationEndTime.TotalMinutes Then
            ' 稼動終了時間<00:00
            If endTime > startTime.Date.AddMinutes(operationEndTime.TotalMinutes) Then
                afterStartIsHimatagi = True
            End If
        Else
            ' 稼動終了時間>=00:00
            If TimeSpan.op_GreaterThan(operationEndTime, startTime.TimeOfDay) Then
                If endTime > startTime.Date.AddMinutes(operationEndTime.TotalMinutes) Then
                    afterStartIsHimatagi = True
                End If
            Else
                If endTime > startTime.Date.AddDays(1).AddMinutes(operationEndTime.TotalMinutes) Then
                    afterStartIsHimatagi = True
                End If
            End If
        End If

        OutputLog(LOG_TYPE_INFO, "[E]IsStartAfterIsHimatagi()", "", Nothing, _
                  "RETURN_VALUE:" & CType(afterStartIsHimatagi, String))
        Return afterStartIsHimatagi

    End Function


    ''' <summary>
    ''' 休憩取得有無判定
    ''' </summary>
    ''' <param name="breakList">休憩時間帯・使用不可時間帯情報</param>
    ''' <param name="isBreak">I/Fからの休憩取得フラグ</param>
    ''' <param name="startTime">判定開始時間</param>
    ''' <param name="endTime">判定終了時間</param>
    ''' <param name="workTime">作業予定時間</param>
    ''' <returns>休憩取得有無</returns>
    ''' <remarks></remarks>
    Public Function CheckBreak(ByVal breakList As SC3150101DataSet.SC3150101StallBreakInfoDataTable, _
                               ByVal isBreak As Boolean, _
                               ByVal startTime As Date, _
                               ByVal endTime As Date, _
                               ByVal workTime As Integer) As Boolean

        OutputLog(LOG_TYPE_INFO, "[S]CheckBreak()", "", Nothing, _
                  "BREAK_INFO:(DataSet)", "BREAK_FLG:" & CType(isBreak, String), _
                  "START_TIME:" & startTime.ToString(CultureInfo.CurrentCulture()), _
                  "END_TIME:" & endTime.ToString(CultureInfo.CurrentCulture()), _
                  "WORK_TIME:" & CType(workTime, String))

        Dim iBreak As Boolean

        If isBreak = True Then
            iBreak = True
        ElseIf isBreak = False Then
            iBreak = False
        ElseIf IsBreakTime(breakList, startTime, endTime) = False Then
            iBreak = True
        ElseIf startTime.AddMinutes(workTime) = endTime Then
            iBreak = False
        Else
            iBreak = True
        End If

        OutputLog(LOG_TYPE_INFO, "[E]CheckBreak()", "", Nothing, _
                  "RETURN_VALUE:" & CType(iBreak, String))
        Return iBreak
    End Function


    ''' <summary>
    ''' 休憩時間にかかるかどうかの判定
    ''' </summary>
    ''' <param name="breakList">休憩時間帯・使用不可時間帯情報</param>
    ''' <param name="startTime">判定開始時間</param>
    ''' <param name="endTime">判定終了時間</param>
    ''' <returns>休憩にかかる場合、True</returns>
    ''' <remarks></remarks>
    Private Function IsBreakTime(ByVal breakList As SC3150101DataSet.SC3150101StallBreakInfoDataTable, _
                                ByVal startTime As Date, _
                                ByVal endTime As Date) As Boolean

        OutputLog(LOG_TYPE_INFO, "[S]IsBreakTime()", "", Nothing, _
                  "BREAK_INFO:(DataSet)", _
                  "START_TIME:" & startTime.ToString(CultureInfo.InvariantCulture()), _
                  "END_TIME:" & endTime.ToString(CultureInfo.InvariantCulture()))

        Dim breakItem As SC3150101DataSet.SC3150101StallBreakInfoRow
        Dim breakStartTime As Date
        Dim breakEndTime As Date

        For Each breakItem In breakList.Rows

            'If breakItem.STARTTIME < endTime Then _
            '    And breakItem.ENDTIME > startTime Then
            breakStartTime = ParseDate(startTime.ToString("yyyyMMdd", _
                                                          CultureInfo.InvariantCulture()) & _
                                                      breakItem.STARTTIME.Trim())
            breakEndTime = ParseDate(startTime.ToString("yyyyMMdd", _
                                                        CultureInfo.InvariantCulture()) & _
                                                    breakItem.ENDTIME.Trim())

            If breakStartTime < endTime And breakEndTime > startTime Then
                OutputLog("I", "[E]suspendWork()", "", Nothing, _
                          "RETURN_VALUE:" & CType(True, String))
                Return True
            End If
            'If breakItem.STARTTIME < endTime.ToString("HHmm") Then _
            'And breakItem.ENDTIME > startTime.ToString("HHmm") Then
            '    Return True
            'End If

        Next

        OutputLog(LOG_TYPE_INFO, "[E]IsBreakTime()", "", Nothing, _
                  "RETURN_VALUE:" & CType(False, String))
        Return False

    End Function


    ''' <summary>
    ''' 衝突有無判定
    ''' </summary>
    ''' <param name="reserveId">予約ID</param>
    ''' <param name="StartTime">開始日時</param>
    ''' <param name="EndTime">終了日時</param>
    ''' <returns>衝突が発生する場合、True</returns>
    ''' <remarks></remarks>
    Public Function IsCollision(ByVal reserveList As SC3150101DataSet.SC3150101StallReserveListDataTable, _
                                ByVal reserveId As Integer, _
                                ByVal startTime As Date, _
                                ByVal endTime As Date) As Boolean

        OutputLog(LOG_TYPE_INFO, "[S]IsCollision()", "", Nothing, _
                  "REZ_INFO:(DataSet)", "REZID:" & CType(reserveId, String), _
                  "START_TIME:" & startTime.ToString(CultureInfo.InvariantCulture()), _
                  "END_TIME:" & endTime.ToString(CultureInfo.InvariantCulture()))

        ' 引数チェック
        If reserveList Is Nothing Then
            OutputLog(LOG_TYPE_INFO, "[E]IsCollision()", "", Nothing, _
                  "RETURN_VALUE:" & CType(False, String))
            Return False
        End If

        Dim reserveItem As SC3150101DataSet.SC3150101StallReserveListRow

        For Each reserveItem In reserveList.Rows

            'If (reserveItem.REZID <> reserveId Or (reserveItem.CANCELFLG = "1" And reserveItem.STOPFLG = "1")) _
            If (reserveItem.REZID <> reserveId _
                Or (reserveItem.CANCELFLG.Equals("1") And reserveItem.STOPFLG.Equals("1"))) _
                And (reserveItem.STARTTIME < endTime) _
                And (reserveItem.ENDTIME > startTime) Then

                OutputLog(LOG_TYPE_INFO, "[E]suspendWork()", "", Nothing, _
                          "RETURN_VALUE:" & CType(True, String))
                Return True

            End If

        Next

        OutputLog(LOG_TYPE_INFO, "[E]IsCollision()", "", Nothing, _
                  "RETURN_VALUE:" & CType(False, String))
        Return False

    End Function


    ''' <summary>
    ''' 指定された日時からストール日とストール時刻に変換する
    ''' 例えば
    '''   稼働時間 02:00/23:00 の場合
    '''     2011-11-30 03:00 → 2011-11-30 03:00
    '''   稼働時間 09:00/04:00 の場合
    '''     2011-11-30 03:00 → 2011-11-29 27:00
    ''' (規約により参照型引数が使えないので一旦date型配列にしてから必要な値を戻す)
    ''' </summary>
    ''' <param name="stallTimeInfo">ストール情報</param>
    ''' <param name="sourceDateTime">処理対象日</param>
    ''' <returns>ストール日, ストール時刻</returns>
    ''' <remarks></remarks>
    Public Function ConvertToStallDateTime(ByVal stallTimeInfo As SC3150101DataSet.SC3150101StallTimeInfoDataTable, _
                                           ByVal sourceDateTime As Date) As Date()

        OutputLog(LOG_TYPE_INFO, "[S]ConvertToStallDateTime()", "", Nothing, _
                  "STALL_TIME_INFO:(DataSet)", _
                  "TARGET_DATE:" & sourceDateTime.ToString(CultureInfo.InvariantCulture()))

        Dim prevDayAvailableEnd As Date
        Dim retDateArray(STALL_DATE_ARRAY_NUMBER) As Date
        Dim stallDate As Date
        Dim stallTime As Date

        stallDate = New DateTime(sourceDateTime.Year, sourceDateTime.Month, sourceDateTime.Day, 0, 0, 0)
        stallTime = New DateTime(1, 1, 1, sourceDateTime.Hour, sourceDateTime.Minute, sourceDateTime.Second)

        Dim availableEndTimeTicks As Long
        availableEndTimeTicks = GetAvailableEndTime(stallTimeInfo, OPERATION_TIME_PROGRESS).Ticks()
        prevDayAvailableEnd = stallDate.AddDays(-1)
        'prevDayAvailableEnd = prevDayAvailableEnd.AddTicks(GetAvailableEndTime(stallTimeInfo, OPERATION_TIME_PROGRESS).Ticks()) 'PROG:0,RES:1
        prevDayAvailableEnd = prevDayAvailableEnd.AddTicks(availableEndTimeTicks) 'PROG:0,RES:1

        If sourceDateTime < prevDayAvailableEnd Then
            'srcDateTimeが前日稼働時間内の場合、ストール用日時に調整
            stallDate = stallDate.AddDays(-1)
            stallTime = stallTime.AddDays(1)
        End If

        retDateArray(STALL_START_DATE) = stallDate
        retDateArray(STALL_START_TIME) = stallTime

        OutputLog(LOG_TYPE_INFO, "[E]ConvertToStallDateTime()", "", Nothing)
        Return retDateArray

    End Function


    ''' <summary>
    ''' チップを配置した場合のチップ開始時刻・チップ終了時刻を返却する
    ''' (規約により参照型引数が使えないので一旦string型配列にしてから必要な値を戻す)
    ''' </summary>
    ''' <param name="stallTimeInfo">ストール時間情報</param>
    ''' <param name="dealerCode">販売店コード</param>
    ''' <param name="branchCode">店舗コード</param>
    ''' <param name="stallId">ストールID</param>
    ''' <param name="targetDate">処理対象日</param>
    ''' <param name="startTime">開始時刻</param>
    ''' <param name="workTimeMinutes">作業時間</param>
    ''' <param name="isBreak">休憩取得するか否か</param>
    ''' <returns>作業日時配列</returns>
    ''' <remarks></remarks>
    Public Function SimulateChipPutting(ByVal stallTimeInfo As SC3150101DataSet.SC3150101StallTimeInfoDataTable, _
                                        ByVal dealerCode As String, _
                                        ByVal branchCode As String, _
                                        ByVal stallId As Integer, _
                                        ByVal targetDate As Date, _
                                        ByVal startTime As Date, _
                                        ByVal workTimeMinutes As Integer, _
                                        ByVal isBreak As Boolean) As Date()

        OutputLog(LOG_TYPE_INFO, "[S]SimulateChipPutting()", "", Nothing, _
                  "STALL_TIME_INFO:(DataSet)", "DLRCD:" & dealerCode, _
                  "STRCD:" & branchCode, "STALLID:" & CType(stallId, String), _
                  "TARGET_DATE:" & CType(targetDate, String), _
                  "START_TIME:" & CType(startTime, String), _
                  "WORK_TIME:" & CType(workTimeMinutes, String), _
                  "BREAK_FLG:" & CType(isBreak, String))

        Dim chipStartDate As Date
        Dim chipStartTime As Date
        Dim chipEndTime As Date
        Dim dateArray(WORK_DATE_ARRAY_NUMBER) As Date


        '最初から開始時刻が稼動時間外の場合、そのまま返す
        If startTime >= GetAvailableEndTime(stallTimeInfo, OPERATION_TIME_PROGRESS) Then
            chipStartDate = targetDate
            chipStartTime = startTime
        End If

        '後で正規化するので、広いほう(tbl_stalltime.pstarttime, pendtime)で取得
        Dim targetDayStart As Date
        targetDayStart = targetDate.AddTicks(GetAvailableStartTime(stallTimeInfo, _
                                                                   OPERATION_TIME_PROGRESS).Ticks())
        Dim targetDayEnd As Date
        targetDayEnd = targetDate.AddTicks(GetAvailableEndTime(stallTimeInfo, _
                                                               OPERATION_TIME_PROGRESS).Ticks())

        '当日の休憩+Unavailableチップのリスト取得
        'Dim stallBreakListTemp As SC3150101DataSet.SC3150101StallBreakListDataTable
        Dim stallBreakListTemp As New SC3150101DataSet.SC3150101StallBreakListDataTable '20120202
        Dim stallBreakList As New SC3150101DataSet.SC3150101StallBreakListDataTable
        If isBreak Then
            stallBreakListTemp = GetStallBreakList(stallTimeInfo, dealerCode, branchCode, _
                                                   stallId, targetDayStart, targetDayEnd)
            '初日のみ、tbl_stalltime.pstarttime ～ tbl_stalltime.endtime内の休憩を取得 (2011-11時点の仕様)
            'stallBreakList = CType(stallBreakListTemp.Clone, SC3150101DataSet.SC3150101StallBreakListDataTable)
            Dim availableStartTimeA As Date
            Dim availableEndTimeA As Date
            availableStartTimeA = GetAvailableStartTime(stallTimeInfo, OPERATION_TIME_PROGRESS)
            availableEndTimeA = GetAvailableEndTime(stallTimeInfo, OPERATION_TIME_RESERVE)
            stallBreakList = Normalize(stallBreakListTemp, _
                                       stallBreakList, _
                                       availableStartTimeA, _
                                       availableEndTimeA)

        Else
            stallBreakList = Nothing
        End If

        '■1 開始時刻補正
        Dim tempStartTime As Date
        Dim drStallBreakList As SC3150101DataSet.SC3150101StallBreakListRow
        If isBreak Then
            drStallBreakList = GetOverlapBreak(stallBreakList, startTime)
            If drStallBreakList Is Nothing Then
                '開始時刻が休憩にかからない場合
                chipStartDate = targetDate
                chipStartTime = startTime
            Else
                '開始時刻が休憩にかかる場合、開始時刻を休憩終了時刻にずらす
                'tempStartTime = HHMMTextToDateTime(StringValueOfDB(drStallBreakList.ENDTIME).Trim())
                tempStartTime = drStallBreakList.ENDTIME
                If GetAvailableEndTime(stallTimeInfo, OPERATION_TIME_PROGRESS) <= tempStartTime Then
                    ' 対象日後の稼働日を取得
                    Dim dateAndCountStart(TARGET_DATE_ARRAY_NUMBER) As String
                    dateAndCountStart = GetNextWorkDate(dealerCode, branchCode, stallId, targetDate, 0)
                    Dim targetDateString As String
                    targetDateString = dateAndCountStart(TARGET_DATE_DATE)
                    Dim AvailableStartTime As Date
                    AvailableStartTime = GetAvailableStartTime(stallTimeInfo, OPERATION_TIME_PROGRESS)
                    Dim targetDateTemp As Date
                    targetDateTemp = Date.ParseExact(targetDateString, "yyyyMMdd", Nothing)
                    '開始時刻が稼動終了以降になった場合、開始時刻を翌稼働日の稼動開始時刻とする
                    dateArray = SimulateChipPutting(stallTimeInfo, _
                                                    dealerCode, _
                                                    branchCode, _
                                                    stallId, _
                                                    targetDateTemp, _
                                                    AvailableStartTime, _
                                                    workTimeMinutes, _
                                                    isBreak)
                    Return dateArray
                Else
                    chipStartDate = targetDate
                    chipStartTime = tempStartTime
                End If
            End If
        Else
            chipStartDate = targetDate
            chipStartTime = startTime
        End If
        ' ■1 開始時刻補正完了

        ' ■2 終了時刻算
        chipEndTime = GetEndTimeAfterRevison(stallTimeInfo, stallBreakListTemp, stallBreakList, _
                                             dealerCode, branchCode, stallId, workTimeMinutes, _
                                             chipStartDate, chipStartTime, isBreak)

        dateArray(WORK_START_DATE) = chipStartDate
        dateArray(WORK_START_TIME) = chipStartTime
        dateArray(WORK_END_TIME) = chipEndTime

        ' 解放
        If stallBreakListTemp IsNot Nothing Then
            stallBreakListTemp.Dispose()
            stallBreakListTemp = Nothing
        End If
        If stallBreakList IsNot Nothing Then
            stallBreakList.Dispose()
            stallBreakList = Nothing
        End If

        OutputLog(LOG_TYPE_INFO, "[E]SimulateChipPutting()", "", Nothing)
        Return dateArray

    End Function


    ''' <summary>
    ''' 開始時刻補正後の終了時刻を算出する
    ''' </summary>
    ''' <param name="stallTimeInfo">ストール情報</param>
    ''' <param name="stallBreakListTempSource">休憩情報</param>
    ''' <param name="stallBreakListSource">正規化した休憩情報</param>
    ''' <param name="dealerCode">販売店コード</param>
    ''' <param name="branchCode">店舗コード</param>
    ''' <param name="stallId">ストールID</param>
    ''' <param name="workTimeMinutes">作業時間</param>
    ''' <param name="chipStartDate">開始日</param>
    ''' <param name="chipStartTime">開始時間</param>
    ''' <param name="isBreak">休憩有無</param>
    ''' <returns>終了時刻</returns>
    ''' <remarks></remarks>
    Private Function GetEndTimeAfterRevison(ByVal stallTimeInfo As SC3150101DataSet.SC3150101StallTimeInfoDataTable, _
                                            ByVal stallBreakListTempSource As SC3150101DataSet.SC3150101StallBreakListDataTable, _
                                            ByVal stallBreakListSource As SC3150101DataSet.SC3150101StallBreakListDataTable, _
                                            ByVal dealerCode As String, _
                                            ByVal branchCode As String, _
                                            ByVal stallId As Integer, _
                                            ByVal workTimeMinutes As Integer, _
                                            ByVal chipStartDate As Date, _
                                            ByVal chipStartTime As Date, _
                                            ByVal isBreak As Boolean) As Date

        OutputLog(LOG_TYPE_INFO, "[S]GetEndTimeAfterRevison()", "", Nothing)

        Dim chipEndTime As Date

        '■2 終了時刻算出
        Dim totalEndTime As Date            ' 仮終了時刻(作業開始時刻からの通算)
        Dim tempDate As Date                ' 処理対象日
        Dim tempDateStartTime As Date       ' 対象日における開始時刻(初日は作業開始時刻、翌日以降は稼動開始時刻)
        'Dim tempDateEndTime As Date         ' 対象日における終了時刻(日跨ぎの場合、最終日以外は稼動終了時刻)
        Dim unavailableDaysCount As Integer ' 跨いだ連続非稼動日数
        'Dim tempTime As Date                ' TEMP用
        totalEndTime = chipStartTime.AddMinutes(workTimeMinutes)
        tempDate = chipStartDate
        tempDateStartTime = chipStartTime

        Dim stallBreakListTemp As SC3150101DataSet.SC3150101StallBreakListDataTable
        If stallBreakListTempSource IsNot Nothing Then
            stallBreakListTemp = CType(stallBreakListTempSource.Copy, SC3150101DataSet.SC3150101StallBreakListDataTable)
        Else
            stallBreakListTemp = Nothing
        End If
        Dim stallBreakList As SC3150101DataSet.SC3150101StallBreakListDataTable
        If stallBreakListSource IsNot Nothing Then
            stallBreakList = CType(stallBreakListSource.Copy, SC3150101DataSet.SC3150101StallBreakListDataTable)
        Else
            stallBreakList = Nothing
        End If

        ' 後で正規化するので、広いほう(tbl_stalltime.pstarttime, pendtime)で取得
        Dim availableStartTimeTicks As Long
        availableStartTimeTicks = GetAvailableStartTime(stallTimeInfo, OPERATION_TIME_PROGRESS).Ticks()
        Dim availableEndTimeTicks As Long
        availableEndTimeTicks = GetAvailableEndTime(stallTimeInfo, OPERATION_TIME_PROGRESS).Ticks()
        Dim targetDayStart As Date
        targetDayStart = tempDate.AddTicks(availableStartTimeTicks)
        Dim targetDayEnd As Date
        targetDayEnd = tempDate.AddTicks(availableEndTimeTicks)

        Dim outerLoop As Integer
        'Dim intInnerLoop As Integer

        outerLoop = 0
        Do
            If isBreak Then

                '初回は開始時間補正時に取得している
                If stallBreakListTemp Is Nothing Then
                    'stallBreakListTemp = Nothing
                    stallBreakListTemp = GetStallBreakList(stallTimeInfo, _
                                                           dealerCode, branchCode, _
                                                           stallId, targetDayStart, targetDayEnd)

                    Dim availableStartTime As Date
                    Dim availableEndTime As Date
                    availableStartTime = GetAvailableStartTime(stallTimeInfo, OPERATION_TIME_RESERVE)
                    availableEndTime = GetAvailableEndTime(stallTimeInfo, OPERATION_TIME_RESERVE)
                    '2日目以降は、tbl_stalltime.starttime ～ tbl_stalltime.endtime内の休憩を取得 (2011-11時点の仕様)
                    stallBreakList = Normalize(stallBreakListTemp, _
                                               stallBreakList, _
                                               availableStartTime, _
                                               availableEndTime)
                End If

                'チップと重なる休憩の合計時間を加算
                totalEndTime = GetTotalEndTime(stallBreakList, tempDateStartTime, totalEndTime)
            End If

            Dim endTimeTemp As Date
            endTimeTemp = GetAvailableEndTime(stallTimeInfo, OPERATION_TIME_RESERVE)
            If tempDate.AddTicks(endTimeTemp.Ticks()) < chipStartDate.AddTicks(totalEndTime.Ticks()) Then
                '翌日以降に日跨いでいる場合（(開始日+仮終了時刻) > (対象日+稼動終了時刻)）

                '仮終了時刻 = 仮終了時刻 + (非稼働時間 * 1日分)
                Dim unavailableTime As TimeSpan
                unavailableTime = GetUnavailableTimeSpan(stallTimeInfo, OPERATION_TIME_RESERVE)
                totalEndTime = totalEndTime.Add(unavailableTime)

                '翌稼働日を取得
                Dim dateAndCountEnd(TARGET_DATE_ARRAY_NUMBER) As String

                unavailableDaysCount = 0
                dateAndCountEnd = GetNextWorkDate(dealerCode, branchCode, stallId, tempDate, _
                                               unavailableDaysCount)
                unavailableDaysCount = CType(dateAndCountEnd(TARGET_DATE_COUNT), Integer) ' 非稼働日数

                '処理対象日 = 処理対象日 + 1
                tempDate = tempDate.AddDays(1)

                '(24h * 非稼動日数分)を加算
                totalEndTime = totalEndTime.AddDays(unavailableDaysCount)
                tempDate = tempDate.AddDays(unavailableDaysCount)

            Else
                chipEndTime = totalEndTime
                Exit Do
            End If

            If stallBreakList IsNot Nothing Then
                stallBreakList.Clear()
            End If
            stallBreakListTemp = Nothing
            tempDateStartTime = GetAvailableStartTime(stallTimeInfo, OPERATION_TIME_RESERVE) '2日目以降は開始時刻=稼動開始時刻

            '無限ループよけ (60日跨ぎ)
            outerLoop = outerLoop + 1
            If outerLoop > 60 Then
                OutputLog(LOG_TYPE_WARNING, "GetEndTimeAfterRevison()", "infinite loop", Nothing)
                OutputLog(LOG_TYPE_INFO, "[E]GetEndTimeAfterRevison()", "", Nothing)
                Throw New ApplicationException("Infinite loop occurred by GetEndTimeAfterRevison() function of SC3150101BusinessLogic")
            End If
        Loop
        '■2 終了時刻算出完了

        OutputLog(LOG_TYPE_INFO, "[E]GetEndTimeAfterRevison()", "", Nothing)
        Return chipEndTime

    End Function


    ''' <summary>
    ''' 休憩時間を考慮した終了時刻を算出する
    ''' </summary>
    ''' <param name="stallBreakListSoruce">正規化した休憩情報</param>
    ''' <param name="tempDateStartTime">開始時間</param>
    ''' <param name="totalEndTimeSoruce">終了時間</param>
    ''' <returns>休憩時間を考慮した終了時間</returns>
    ''' <remarks></remarks>
    Private Function GetTotalEndTime(ByVal stallBreakListSoruce As SC3150101DataSet.SC3150101StallBreakListDataTable, _
                                     ByVal tempDateStartTime As Date, _
                                     ByVal totalEndTimeSoruce As Date) As Date

        OutputLog(LOG_TYPE_INFO, "[S]GetTotalEndTime()", "", Nothing)

        Dim tempDateEndTime As Date ' 対象日における終了時刻(日跨ぎの場合、最終日以外は稼動終了時刻)
        Dim totalEndTime As Date = totalEndTimeSoruce
        Dim tempTime As Date = tempDateStartTime ' 一時格納用
        Dim stallBreakList As SC3150101DataSet.SC3150101StallBreakListDataTable
        Dim drStallBreakList As SC3150101DataSet.SC3150101StallBreakListRow
        stallBreakList = CType(stallBreakListSoruce.Copy, SC3150101DataSet.SC3150101StallBreakListDataTable)

        Dim i As Integer = 0 ' ループカウンタ
        Do
            drStallBreakList = GetNextBreak(stallBreakList, tempTime)
            If drStallBreakList Is Nothing Then
                Exit Do
            Else
                tempDateEndTime = New DateTime(1, 1, 1, totalEndTime.Hour, totalEndTime.Minute, 0)
                If tempDateEndTime < tempDateStartTime Then
                    tempDateEndTime = tempDateEndTime.AddDays(1)
                End If

                If drStallBreakList.STARTTIME < tempDateEndTime Then
                    ' 休憩開始時刻 < 処理対象日の仮終了時刻 場合

                    ' 休憩時間
                    Dim breakTime As TimeSpan
                    breakTime = DateTime.op_Subtraction(drStallBreakList.ENDTIME, drStallBreakList.STARTTIME)

                    ' 仮終了時刻 = 仮終了時刻 + 休憩時間
                    totalEndTime = totalEndTime.Add(breakTime)
                    tempTime = drStallBreakList.ENDTIME
                Else
                    Exit Do
                End If
            End If

            '無限ループよけ (休憩最大5 + 1日分のUnavailableチップ数)
            i = i + 1
            If i > 60 Then
                OutputLog(LOG_TYPE_WARNING, "GetTotalEndTime()", "infinite loop", Nothing)
                OutputLog(LOG_TYPE_INFO, "[E]GetTotalEndTime()", "", Nothing)
                Throw New ApplicationException("Infinite loop occurred by GetTotalEndTime() function of SC3150101BusinessLogic")
            End If
        Loop

        OutputLog(LOG_TYPE_INFO, "[E]GetTotalEndTime()", "", Nothing)
        Return totalEndTime

    End Function


    ''' <summary>
    ''' 処理対象日のUnavailableチップおよび休憩のリストを返却する
    ''' </summary>
    ''' <param name="stallTimeInfo">ストール情報</param>
    ''' <param name="dealerCode">販売店コード</param>
    ''' <param name="branchCode">店舗コード</param>
    ''' <param name="stallId">ストールID</param>
    ''' <param name="targetDayStart">対象開始時間</param>
    ''' <param name="targetDayEnd">対象終了時間</param>
    ''' <returns>使用不可チップ、休憩リスト</returns>
    ''' <remarks></remarks>
    Private Function GetStallBreakList(ByVal stallTimeInfo As SC3150101DataSet.SC3150101StallTimeInfoDataTable, _
                                       ByVal dealerCode As String, _
                                       ByVal branchCode As String, _
                                       ByVal stallId As Integer, _
                                       ByVal targetDayStart As Date, _
                                       ByVal targetDayEnd As Date) As SC3150101DataSet.SC3150101StallBreakListDataTable

        OutputLog(LOG_TYPE_INFO, "[S]GetStallBreakList()", "", Nothing, _
                  "STALL_TIME_INFO:(DataSet)", "DLRCD:" & dealerCode, _
                  "STRCD:" & branchCode, "STALLID:" & CType(stallId, String), _
                  "TARGET_S_DATE:" & targetDayStart.ToString(CultureInfo.InvariantCulture()), _
                  "TARGET_E_DATE:" & targetDayEnd.ToString(CultureInfo.InvariantCulture()))


        Using adapter As New SC3150101DataSetTableAdapters.SC3150101StallInfoDataTableAdapter

            '当日の休憩+Unavailableチップのリスト取得
            Dim unavailableChipInfo As SC3150101DataSet.SC3150101UnavailableChipListDataTable
            unavailableChipInfo = adapter.GetUnavailableList(dealerCode, branchCode, stallId, _
                                                             targetDayStart, targetDayEnd)

            Dim breakStartDate As Date
            Dim breakEndDate As Date
            Dim breakStartTime As Date
            Dim breakEndTime As Date

            Dim breakList As New SC3150101DataSet.SC3150101StallBreakListDataTable

            Dim drBreakItem As SC3150101DataSet.SC3150101StallBreakListRow
            Dim drUnavailableChipItem As SC3150101DataSet.SC3150101UnavailableChipListRow

            'drBreakItem = CType(breakList.NewRow(), SC3150101DataSet.SC3150101StallBreakListRow)
            'drBreakItem = CType(breakList.Rows(), SC3150101DataSet.SC3150101StallBreakListRow)

            For Each drUnavailableChipItem In unavailableChipInfo.Rows

                breakStartDate = YYYYMMDDTextToDateTime(StringValueOfDB(drUnavailableChipItem.STARTTIME_DAY).Trim())
                breakStartTime = HHMMTextToDateTime(StringValueOfDB(drUnavailableChipItem.STARTTIME_TIME).Trim())
                breakEndDate = YYYYMMDDTextToDateTime(StringValueOfDB(drUnavailableChipItem.ENDTIME_DAY).Trim())
                breakEndTime = HHMMTextToDateTime(StringValueOfDB(drUnavailableChipItem.ENDTIME_TIME).Trim())

                'Unavailable開始日時が当日稼動開始時刻より前(日跨ぎ)の場合
                If breakStartDate.AddTicks(breakStartTime.Ticks()) < targetDayStart Then
                    '当日分のみ取得
                    breakStartTime = GetAvailableStartTime(stallTimeInfo, OPERATION_TIME_PROGRESS)
                End If

                'Unavailable終了日時が当日稼動終了時刻より後の場合
                If breakEndDate.AddTicks(breakEndTime.Ticks()) > targetDayEnd Then
                    '当日分のみ取得
                    breakEndTime = GetAvailableEndTime(stallTimeInfo, OPERATION_TIME_PROGRESS)
                End If
                drBreakItem = CType(breakList.NewRow(), SC3150101DataSet.SC3150101StallBreakListRow)
                drBreakItem.STARTTIME = breakStartTime
                drBreakItem.ENDTIME = breakEndTime

                ' データセットに行を追加
                breakList.Rows.Add(drBreakItem)
            Next

            ' 次世代で追加(以下の情報のみで良い気がするが・・・)-------------------------------
            ' ※既存では、休憩情報のみを取得したもの(LoadBreakMaster())とマージしている
            ' 休憩時間帯・使用不可時間帯取得
            Dim drStallTimeInfo As SC3150101DataSet.SC3150101StallTimeInfoRow
            drStallTimeInfo = CType(stallTimeInfo.Rows(0), SC3150101DataSet.SC3150101StallTimeInfoRow)
            Dim fromDate As Date
            Dim toDate As Date
            fromDate = targetDayStart.Date.Add(SetStallTime(drStallTimeInfo.PSTARTTIME).TimeOfDay)
            toDate = targetDayEnd.Date.Add(SetStallTime(drStallTimeInfo.PENDTIME).TimeOfDay)
            Dim breakInfo As SC3150101DataSet.SC3150101StallBreakInfoDataTable
            breakInfo = adapter.GetBreakSlot(stallId, fromDate, toDate)

            Dim drBreakInfo As SC3150101DataSet.SC3150101StallBreakInfoRow
            For Each drBreakInfo In breakInfo.Rows
                breakStartTime = HHMMTextToDateTime(StringValueOfDB(drBreakInfo.STARTTIME).Trim())
                breakEndTime = HHMMTextToDateTime(StringValueOfDB(drBreakInfo.ENDTIME).Trim())

                drBreakItem = CType(breakList.NewRow(), SC3150101DataSet.SC3150101StallBreakListRow)
                drBreakItem.STARTTIME = breakStartTime
                drBreakItem.ENDTIME = breakEndTime

                ' データセットに行を追加
                breakList.Rows.Add(drBreakItem)
                'breakList.NewRow()
                'breakList.ImportRow(drBreakItem)
            Next
            ' ---------------------------------------------------------------------------------

            OutputLog(LOG_TYPE_INFO, "[E]GetStallBreakList()", "", Nothing)
            Return breakList

        End Using

    End Function


    ''' <summary>
    ''' 稼動開始時刻を返却する
    ''' </summary>
    ''' <param name="stallTimeInfo">ストール時間情報</param>
    ''' <param name="startTimeType">ProgressiveかReservationか</param>
    ''' <returns>稼動開始時刻</returns>
    ''' <remarks></remarks>
    Private Function GetAvailableStartTime(ByVal stallTimeInfo As SC3150101DataSet.SC3150101StallTimeInfoDataTable, _
                                           ByVal startTimeType As Integer) As DateTime

        OutputLog(LOG_TYPE_INFO, "[S]GetAvailableStartTime()", "", Nothing, _
                  "STALL_TIME_INFO:(DataSet)", "TYPE:" & CType(startTimeType, String))

        Dim startTime As Date
        Dim drStallTimeInfo As SC3150101DataSet.SC3150101StallTimeInfoRow

        'LoadAvailableTime()
        Dim availStartTime As Date  'Progressive稼動開始時刻(日付は持たない)
        Dim availEndTime As Date    'Progressive稼動終了時刻(日付は持たない。24時以降の場合、01:00→25:00の形で持つ)
        Dim availStartTimeR As Date 'Reservation稼動開始時刻(日付は持たない)
        Dim availEndTimeR As Date   'Reservation稼動終了時刻(日付は持たない。24時以降の場合、01:00→25:00の形で持つ)
        'Dim stallType As Integer    'ストール時間タイプ


        drStallTimeInfo = CType(stallTimeInfo.Rows(0), SC3150101DataSet.SC3150101StallTimeInfoRow)
        availStartTimeR = HHMMTextToDateTime(StringValueOfDB(drStallTimeInfo.STARTTIME.Trim()))
        availEndTimeR = HHMMTextToDateTime(StringValueOfDB(drStallTimeInfo.ENDTIME.Trim()))
        'If StringValueOfDB(drStallTimeInfo.PSTARTTIME.Trim()).Equals(String.Empty) Then
        If String.IsNullOrEmpty(StringValueOfDB(drStallTimeInfo.PSTARTTIME.Trim())) Then
            'PSTARTTIME, PENDTIMEが未登録の場合、STARTTIME, ENDTIMEを使用
            availStartTime = availStartTimeR
            availEndTime = availEndTimeR
        Else
            availStartTime = HHMMTextToDateTime(StringValueOfDB(drStallTimeInfo.PSTARTTIME).Trim())
            availEndTime = HHMMTextToDateTime(StringValueOfDB(drStallTimeInfo.PENDTIME).Trim())
        End If


        If availStartTime > availEndTime Then
            'stallType = TIME_TYPE_OVER24
            availEndTime = availEndTime.AddDays(1)
        Else
            'stallType = TIME_TYPE_NORMAL
        End If

        If availStartTimeR > availEndTimeR Then
            availEndTimeR = availEndTimeR.AddDays(1)
        End If



        If startTimeType = 0 Then
            startTime = availStartTime
        ElseIf startTimeType = 1 Then
            startTime = availStartTimeR
        End If

        OutputLog(LOG_TYPE_INFO, "[E]GetAvailableStartTime()", "", Nothing, _
                  "RETURN_VALUE:" & startTime.ToString(CultureInfo.CurrentCulture()))
        Return startTime
    End Function


    ''' <summary>
    ''' 稼動終了時刻を返却する
    ''' </summary>
    ''' <param name="stallTimeInfo">ストール時間情報</param>
    ''' <param name="endTimeType">Progressive:0かReservation:1か</param>
    ''' <returns>稼動終了時刻</returns>
    ''' <remarks></remarks>
    Private Function GetAvailableEndTime(ByVal stallTimeInfo As SC3150101DataSet.SC3150101StallTimeInfoDataTable, _
                                         ByVal endTimeType As Integer) As DateTime

        OutputLog(LOG_TYPE_INFO, "[S]GetAvailableEndTime()", "", Nothing, _
                  "STALL_TIME_INFO:(DataSet)", "TYPE:" & CType(endTimeType, String))

        Dim endTime As Date
        Dim drStallTimeInfo As SC3150101DataSet.SC3150101StallTimeInfoRow

        'LoadAvailableTime()
        Dim availStartTime As Date  'Progressive稼動開始時刻(日付は持たない)
        Dim availEndTime As Date    'Progressive稼動終了時刻(日付は持たない。24時以降の場合、01:00→25:00の形で持つ)
        Dim availStartTimeR As Date 'Reservation稼動開始時刻(日付は持たない)
        Dim availEndTimeR As Date   'Reservation稼動終了時刻(日付は持たない。24時以降の場合、01:00→25:00の形で持つ)
        'Dim stallType As Integer    'ストール時間タイプ

        drStallTimeInfo = CType(stallTimeInfo.Rows(0), SC3150101DataSet.SC3150101StallTimeInfoRow)
        availStartTimeR = HHMMTextToDateTime(StringValueOfDB(drStallTimeInfo.STARTTIME.Trim()))
        availEndTimeR = HHMMTextToDateTime(StringValueOfDB(drStallTimeInfo.ENDTIME.Trim()))
        'If StringValueOfDB(drStallTimeInfo.PSTARTTIME.Trim()).Equals(String.Empty) Then
        If String.IsNullOrEmpty(StringValueOfDB(drStallTimeInfo.PSTARTTIME.Trim())) Then
            'PSTARTTIME, PENDTIMEが未登録の場合、STARTTIME, ENDTIMEを使用
            availStartTime = availStartTimeR
            availEndTime = availEndTimeR
        Else
            availStartTime = HHMMTextToDateTime(StringValueOfDB(drStallTimeInfo.PSTARTTIME).Trim())
            availEndTime = HHMMTextToDateTime(StringValueOfDB(drStallTimeInfo.PENDTIME).Trim())
        End If


        If availStartTime > availEndTime Then
            'stallType = TIME_TYPE_OVER24 'StallTimeTpye.Over24
            availEndTime = availEndTime.AddDays(1)
        Else
            'stallType = TIME_TYPE_NORMAL 'StallTimeTpye.Normal
        End If

        If availStartTimeR > availEndTimeR Then
            availEndTimeR = availEndTimeR.AddDays(1)
        End If

        If endTimeType = 0 Then
            endTime = availEndTime
        ElseIf endTimeType = 1 Then
            endTime = availEndTimeR
        End If

        OutputLog(LOG_TYPE_INFO, "[E]GetAvailableEndTime()", "", Nothing, _
                  "RETURN_VALUE:" & endTime.ToString(CultureInfo.InvariantCulture()))
        Return endTime

    End Function


    ''' <summary>
    ''' 非稼働時間を返却する
    ''' </summary>
    ''' <param name="stallTimeInfo">ストール時間情報</param>
    ''' <param name="startEndTimeType">ProgressiveかReservationか</param>
    ''' <returns>非稼働時間</returns>
    ''' <remarks></remarks>
    Public Function GetUnavailableTimeSpan(ByVal stallTimeInfo As SC3150101DataSet.SC3150101StallTimeInfoDataTable, _
                                           ByVal startEndTimeType As Integer) As TimeSpan

        OutputLog(LOG_TYPE_INFO, "[S]GetUnavailableTimeSpan()", "", Nothing, _
                  "STALL_TIME_INFO:(DataSet)", "TYPE:" & CType(startEndTimeType, String))

        Dim notOpetationDate As TimeSpan

        notOpetationDate = DateTime.op_Subtraction(GetAvailableStartTime(stallTimeInfo, _
                                                                         startEndTimeType).AddDays(1), _
                                                   GetAvailableEndTime(stallTimeInfo, _
                                                                       startEndTimeType))

        OutputLog(LOG_TYPE_INFO, "[E]GetUnavailableTimeSpan()", "", Nothing, _
                  "RETURN_VALUE:" & notOpetationDate.ToString())
        Return notOpetationDate

    End Function


    ''' <summary>
    ''' リスト内の休憩を正規化する
    ''' ・対象時間外の休憩を削除
    ''' ・重複・隣接する休憩を結合
    ''' </summary>
    ''' <param name="stallBreakList">休憩情報</param>
    ''' <param name="retStallBreakList">戻し用休憩情報</param>
    ''' <param name="startTime">正規化対象開始時刻</param>
    ''' <param name="endTime">正規化対象終了時刻</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function Normalize(ByVal stallBreakList As SC3150101DataSet.SC3150101StallBreakListDataTable, _
                               ByVal retStallBreakList As SC3150101DataSet.SC3150101StallBreakListDataTable, _
                               ByVal startTime As Date, _
                               ByVal endTime As Date) As SC3150101DataSet.SC3150101StallBreakListDataTable

        OutputLog(LOG_TYPE_INFO, "[S]Normalize()", "", Nothing, _
                  "BREAK_INFO:(DataSet)", _
                  "START_TIME:" & startTime.ToString(CultureInfo.InvariantCulture()), _
                  "END_TIME:" & endTime.ToString(CultureInfo.InvariantCulture()))

        ' 引数チェック
        If stallBreakList Is Nothing Then
            OutputLog(LOG_TYPE_INFO, "[E]Normalize()", "", Nothing)
            Return retStallBreakList
        End If
        'Dim currBreak As StallBreak
        'Dim newList As New SortedList
        'Dim prevEnd As DateTime
        'Dim prevBreak As StallBreak
        'Dim retStallBreakList As New SC3150101DataSet.SC3150101StallBreakListDataTable

        '稼働時間外を排除する
        '・稼働時間外にはみ出している休憩、稼働時間外の休憩(休憩設定後に稼働時間変更すると存在可能性あり)
        '・稼働時間外にはみ出しているUnavailable、稼働時間外のUnavailable、日跨ぎUnavailable
        Dim stallBreakItem As SC3150101DataSet.SC3150101StallBreakListRow
        For Each stallBreakItem In stallBreakList.Rows

            'unavailableStartTime = HHMMTextToDateTime(StringValueOfDB(unavailableChipItem.STARTTIME_TIME).Trim())
            'unavailableEndTime = HHMMTextToDateTime(StringValueOfDB(unavailableChipItem.ENDTIME_TIME).Trim())

            '開始時刻が稼働時間前の場合
            If stallBreakItem.STARTTIME < startTime Then
                '稼働時間外の分を切り落とす
                stallBreakItem.STARTTIME = startTime
                If stallBreakItem.ENDTIME < startTime Then
                    '完全に稼働時間外の場合は0m扱いとする
                    stallBreakItem.ENDTIME = startTime
                End If
            End If

            '終了時刻が稼働時間後の場合
            If stallBreakItem.ENDTIME > endTime Then
                '稼働時間外の分を切り落とす
                stallBreakItem.ENDTIME = endTime
                If stallBreakItem.STARTTIME > endTime Then
                    '完全に稼働時間外の場合は0m扱いとする
                    stallBreakItem.STARTTIME = endTime
                End If
            End If
        Next stallBreakItem

        '重複する休憩を排除する
        Dim prevEnd As DateTime
        prevEnd = DateTime.MinValue
        For Each stallBreakItem In stallBreakList.Rows

            If TimeSpan.op_Equality(DateTime.op_Subtraction(stallBreakItem.ENDTIME, _
                                                            stallBreakItem.STARTTIME), _
                                    TimeSpan.Zero) <> True Then '0分は無視する
                If stallBreakItem.STARTTIME < prevEnd Then
                    '前休憩の終了時刻より自休憩の開始時刻が前の場合

                    If stallBreakItem.ENDTIME > prevEnd Then
                        '自休憩の終了時刻が前休憩の終了時刻より後の場合、自休憩の開始時刻を前休憩の終了時刻に書換
                        stallBreakItem.STARTTIME = prevEnd
                        prevEnd = stallBreakItem.ENDTIME
                    Else
                        '自休憩の終了時刻が前休憩の終了時刻以前の場合、自休憩を0分に書換
                        stallBreakItem.STARTTIME = stallBreakItem.ENDTIME
                    End If
                Else
                    prevEnd = stallBreakItem.ENDTIME
                End If
            End If
        Next stallBreakItem

        '隣接する休憩を結合する
        Dim prevBreak As SC3150101DataSet.SC3150101StallBreakListRow
        prevBreak = Nothing
        For Each stallBreakItem In stallBreakList.Rows

            If prevBreak Is Nothing Then
                prevBreak = stallBreakItem
            ElseIf TimeSpan.op_Equality(DateTime.op_Subtraction(stallBreakItem.ENDTIME, _
                                                                stallBreakItem.STARTTIME), _
                                        TimeSpan.Zero) <> True Then '0分は無視する
                If DateTime.op_Equality(stallBreakItem.STARTTIME, prevBreak.ENDTIME) Then
                    '自休憩の開始時刻=前休憩の終了時刻の場合
                    prevBreak.ENDTIME = stallBreakItem.ENDTIME
                    stallBreakItem.STARTTIME = stallBreakItem.ENDTIME
                End If

                If prevBreak.ENDTIME < stallBreakItem.ENDTIME Then
                    prevBreak = stallBreakItem
                End If
            End If
        Next stallBreakItem

        For Each stallBreakItem In stallBreakList.Rows

            Dim timeInterval As TimeSpan
            timeInterval = DateTime.op_Subtraction(stallBreakItem.ENDTIME, stallBreakItem.STARTTIME)
            If TimeSpan.op_Equality(timeInterval, TimeSpan.Zero) <> True Then
                '0分ではない休憩のみ追加
                'newList.Add(currBreak.GetStartTime().ToString("ddHHmm") & "0000", currBreak)
                'retStallBreakList.Rows.Add(stallBreakItem)
                retStallBreakList.ImportRow(stallBreakItem)
            End If
        Next stallBreakItem

        OutputLog(LOG_TYPE_INFO, "[E]Normalize()", "", Nothing)
        Return retStallBreakList
    End Function


    ''' <summary>
    ''' 指定時刻を含む休憩を返却する
    ''' </summary>
    ''' <param name="stallBreakList">休憩情報</param>
    ''' <param name="startTime">開始時刻</param>
    ''' <returns>指定時刻を含む休憩、存在しない場合Nothing</returns>
    ''' <remarks></remarks>
    Public Function GetOverlapBreak(ByVal stallBreakList As SC3150101DataSet.SC3150101StallBreakListDataTable, _
                                    ByVal startTime As DateTime) As SC3150101DataSet.SC3150101StallBreakListRow

        OutputLog(LOG_TYPE_INFO, "[S]GetOverlapBreak()", "", Nothing, _
                  "BREAK_INFO:(DataSet)", _
                  "START_TIME:" & startTime.ToString(CultureInfo.InvariantCulture()))

        '引数チェック
        If stallBreakList Is Nothing Then
            OutputLog(LOG_TYPE_INFO, "[E]GetOverlapBreak()", "", Nothing, "RETURN_VALUE:Nothing")
            Return Nothing
        End If

        Dim stallBreakItem As SC3150101DataSet.SC3150101StallBreakListRow

        '必ず正規化してから呼ぶ
        'Debug.Assert(Me.normalized)

        For Each stallBreakItem In stallBreakList.Rows

            If (stallBreakItem.STARTTIME <= startTime) _
                And (stallBreakItem.ENDTIME >= startTime) Then
                OutputLog("I", "[E]GetOverlapBreak()", "", Nothing)
                Return stallBreakItem
            End If
        Next stallBreakItem

        OutputLog(LOG_TYPE_INFO, "[E]GetOverlapBreak()", "", Nothing, "RETURN_VALUE:Nothing")
        Return Nothing

    End Function


    ''' <summary>
    ''' 指定時刻以降に開始される休憩を返却する
    ''' </summary>
    ''' <param name="stallBreakList">休憩情報</param>
    ''' <param name="targetTime">対象時間</param>
    ''' <returns>指定時刻以降の休憩、存在しない場合Nothing</returns>
    ''' <remarks></remarks>
    Public Function GetNextBreak(ByVal stallBreakList As SC3150101DataSet.SC3150101StallBreakListDataTable, _
                                 ByVal targetTime As DateTime) As SC3150101DataSet.SC3150101StallBreakListRow

        OutputLog(LOG_TYPE_INFO, "[S]GetNextBreak()", "", Nothing, _
                  "BRREAK_INFO:(DataSet)", _
                  "TARGET_TIME:" & targetTime.ToString(CultureInfo.InvariantCulture()))

        ' 引数チェック
        If stallBreakList Is Nothing Then
            OutputLog(LOG_TYPE_INFO, "[E]GetNextBreak()", "", Nothing, "RETURN_VALUE:Nothing")
            Return Nothing
        End If

        Dim stallBreakItem As SC3150101DataSet.SC3150101StallBreakListRow
        'Dim unavailableStatTime As Date
        '必ず正規化してから呼ぶ
        'Debug.Assert(Me.normalized)

        For Each stallBreakItem In stallBreakList.Rows

            'unavailableStatTime = HHMMTextToDateTime(StringValueOfDB(unavailableChipItem.STARTTIME_TIME).Trim())
            If stallBreakItem.STARTTIME >= targetTime Then
                OutputLog(LOG_TYPE_INFO, "[E]GetNextBreak()", "", Nothing, _
                          "RETURN_VALUE:" & CType(stallBreakItem.ItemArray.Count, String))
                Return stallBreakItem
            End If

        Next stallBreakItem

        OutputLog(LOG_TYPE_INFO, "[E]GetNextBreak()", "", Nothing, "RETURN_VALUE:Nothing")
        Return Nothing

    End Function


    ''' <summary>
    ''' 処理対象日より後の稼働日を返却する
    ''' (規約により参照型引数が使えないので一旦string型配列にしてから必要な値を戻す)
    ''' </summary>
    ''' <param name="dealerCode">販売店コード</param>
    ''' <param name="branchCode">店舗コード</param>
    ''' <param name="stallId">ストールID</param>
    ''' <param name="targetDate">処理対象日</param>
    ''' <param name="unavailableCount">非稼働日を跨いだ場合その日数</param>
    ''' <returns>稼働日(時刻は持たない)、非稼働日数</returns>
    ''' <remarks></remarks>
    Public Function GetNextWorkDate(ByVal dealerCode As String, _
                                    ByVal branchCode As String, _
                                    ByVal stallId As Integer, _
                                    ByVal targetDate As DateTime, _
                                    ByVal unavailableCount As Integer) As String()

        OutputLog(LOG_TYPE_INFO, "[S]GetNextWorkDate()", "", Nothing, _
                  "DLRCD:" & dealerCode, "STRCD:" & branchCode, _
                  "STALLID:" & CType(stallId, String), _
                  "TARGET_DATE:" & targetDate.ToString(CultureInfo.InvariantCulture()), _
                  "DAY_NUM:" & CType(unavailableCount, String))

        Dim returnArrayValue(TARGET_DATE_ARRAY_NUMBER) As String ' 戻り値
        Dim nextNonworkingDate As DateTime
        Dim tempDate As DateTime
        Dim intLoop As Integer

        Dim dayText As String
        Dim y As Integer
        Dim m As Integer
        Dim d As Integer

        Using adapter As New SC3150101DataSetTableAdapters.SC3150101StallInfoDataTableAdapter

            Dim stallPlanInfo As SC3150101DataSet.SC3150101NextNonworkingDateDataTable
            Dim drStallPlanInfo As SC3150101DataSet.SC3150101NextNonworkingDateRow

            unavailableCount = 0
            tempDate = targetDate

            intLoop = 0
            Do

                stallPlanInfo = adapter.GetNextNonworkingDate(dealerCode, branchCode, _
                                                              stallId, tempDate)

                If IsNothing(stallPlanInfo) Or stallPlanInfo.Count <= 0 Then
                    nextNonworkingDate = DateTime.MinValue
                Else
                    drStallPlanInfo = CType(stallPlanInfo.Rows(0),  _
                                            SC3150101DataSet.SC3150101NextNonworkingDateRow)
                    dayText = drStallPlanInfo.WORKDATE
                    y = Integer.Parse(dayText.Substring(0, 4), CultureInfo.InvariantCulture())
                    m = Integer.Parse(dayText.Substring(4, 2), CultureInfo.InvariantCulture())
                    d = Integer.Parse(dayText.Substring(6, 2), CultureInfo.InvariantCulture())
                    nextNonworkingDate = New DateTime(y, m, d, 0, 0, 0)
                End If

                tempDate = tempDate.AddDays(1)

                '非稼働日が存在しない場合
                If nextNonworkingDate = DateTime.MinValue Then
                    OutputLog(LOG_TYPE_INFO, "[E]GetNextWorkDate()", "", Nothing, _
                              "RETURN_VALUE:" & tempDate.ToString(CultureInfo.InvariantCulture()))
                    'Return tempDate
                    returnArrayValue(TARGET_DATE_DATE) = tempDate.ToString("yyyyMMdd", _
                                                                           CultureInfo.InvariantCulture())
                    returnArrayValue(TARGET_DATE_COUNT) = CType(unavailableCount, String)
                    Return returnArrayValue
                End If

                '翌日は非稼働日ではない場合
                If nextNonworkingDate <> tempDate Then
                    OutputLog(LOG_TYPE_INFO, "[E]GetNextWorkDate()", "", Nothing, _
                              "RETURN_VALUE:" & tempDate.ToString(CultureInfo.InvariantCulture()))
                    'Return tempDate
                    returnArrayValue(TARGET_DATE_DATE) = tempDate.ToString("yyyyMMdd", _
                                                                           CultureInfo.InvariantCulture())
                    returnArrayValue(TARGET_DATE_COUNT) = CType(unavailableCount, String)
                    Return returnArrayValue
                End If

                '翌日が非稼働日の場合繰り返す
                unavailableCount = unavailableCount + 1

                '無限ループよけ
                intLoop = intLoop + 1
                If intLoop > 60 Then
                    OutputLog(LOG_TYPE_WARNING, "GetNextWorkDate()", "infinite loop", Nothing)
                    'Throw New Exception("SC3150101BusinessLogic")
                    Throw New ApplicationException("Infinite loop occurred by GetNextWorkDate() function of SC3150101BusinessLogic")
                End If
            Loop

        End Using

    End Function


    ''' <summary>
    ''' 指定時間への予約の移動
    ''' </summary>
    ''' <param name="reserveList">予約情報リスト</param>
    ''' <param name="stallTimeInfo">ストール情報</param>
    ''' <param name="breakInfo">休憩情報</param>
    ''' <param name="dealerCode">販売店コード</param>
    ''' <param name="branchCode">店舗コード</param>
    ''' <param name="reserveID">予約ID</param>
    ''' <param name="stallId">予約ID</param>
    ''' <param name="startTime">開始日時</param>
    ''' <param name="endTime">終了日時</param>
    ''' <returns>予約情報リスト</returns>
    ''' <remarks></remarks>
    Public Function MoveReserve(ByVal reserveList As SC3150101DataSet.SC3150101StallReserveListDataTable, _
                                ByVal stallTimeInfo As SC3150101DataSet.SC3150101StallTimeInfoDataTable, _
                                ByVal breakInfo As SC3150101DataSet.SC3150101StallBreakInfoDataTable, _
                                ByVal dealerCode As String, _
                                ByVal branchCode As String, _
                                ByVal reserveId As Integer, _
                                ByVal stallId As Integer, _
                                ByVal startTime As DateTime, _
                                ByVal endTime As DateTime) As SC3150101DataSet.SC3150101StallReserveListDataTable

        OutputLog(LOG_TYPE_INFO, "[S]MoveReserve()", "", Nothing, _
                  "REZ_INFO:(DataSet)", "STALL_TIME_INFO:(DataSet)", _
                  "DLRCD:" & dealerCode, "STRCD:" & branchCode, _
                  "REZID:" & CType(reserveId, String), "STALLID:" & CType(stallId, String), _
                  "START_TIME:" & startTime.ToString(CultureInfo.InvariantCulture()), _
                  "END_TIME:" & endTime.ToString(CultureInfo.InvariantCulture()))

        Dim retReserveList As SC3150101DataSet.SC3150101StallReserveListDataTable

        '_StartPosition = endTime
        retReserveList = MoveReserveSub(reserveList, _
                                        stallTimeInfo, _
                                        breakInfo, _
                                        dealerCode, _
                                        branchCode, _
                                        reserveId, _
                                        stallId, _
                                        startTime, _
                                        endTime)
        If retReserveList Is Nothing Then
            OutputLog(LOG_TYPE_INFO, "[E]MoveReserve()", "", Nothing, "RETURN_VALUE:Nothing")
            Return Nothing
        End If

        OutputLog(LOG_TYPE_INFO, "[E]MoveReserve()", "", Nothing, _
                  "RETURN_VALUE:" & CType(retReserveList.Count, String))
        Return retReserveList

    End Function


    ''' <summary>
    ''' 指定時間への予約の移動(再帰呼び出し用サブルーチン)
    ''' </summary>
    ''' <param name="reserveList">予約情報</param>
    ''' <param name="stallTimeInfo">ストール情報</param>
    ''' <param name="breakInfo">休憩情報</param>
    ''' <param name="dealerCode">販売店コード</param>
    ''' <param name="branchCode">店舗コード</param>
    ''' <param name="reserveId">予約ID</param>
    ''' <param name="stallId">ストールID</param>
    ''' <param name="startTime">開始時間</param>
    ''' <param name="endTime">終了時間</param>
    ''' <returns>移動させる予約情報。異常終了した場合、Nothing</returns>
    ''' <remarks></remarks>
    Private Function MoveReserveSub(ByVal reserveList As SC3150101DataSet.SC3150101StallReserveListDataTable, _
                                    ByVal stallTimeInfo As SC3150101DataSet.SC3150101StallTimeInfoDataTable, _
                                    ByVal breakInfo As SC3150101DataSet.SC3150101StallBreakInfoDataTable, _
                                    ByVal dealerCode As String, _
                                    ByVal branchCode As String, _
                                    ByVal reserveId As Integer, _
                                    ByVal stallId As Integer, _
                                    ByVal startTime As DateTime, _
                                    ByVal endTime As DateTime) As SC3150101DataSet.SC3150101StallReserveListDataTable

        OutputLog(LOG_TYPE_INFO, "[S]MoveReserveSub()", "", Nothing, _
                  "REZ_INFO:" & CType(reserveList.Count, String), _
                  "STALL_TIME_INFO:" & CType(stallTimeInfo.Count, String), _
                  "DLRCD:" & dealerCode, "STRCD:" & branchCode, _
                  "REZID:" & CType(reserveId, String), "STALLID:" & CType(stallId, String), _
                  "START_TIME:" & startTime.ToString(CultureInfo.InvariantCulture()), _
                  "END_TIME:" & endTime.ToString(CultureInfo.InvariantCulture()))

        If reserveList.Count = 0 Then
            Return Nothing
        End If

        Dim targetList As New List(Of SC3150101DataSet.SC3150101StallReserveListRow)
        Dim chipTimeList As SC3150101DataSet.SC3150101ChipTimeDataTable
        Dim retReserveInfo As New SC3150101DataSet.SC3150101StallReserveListDataTable
        Dim drReserveInfoTemp() As SC3150101DataSet.SC3150101StallReserveListRow
        Dim drReserveInfo As SC3150101DataSet.SC3150101StallReserveListRow

        Dim kadoTime(2) As Date
        kadoTime = GetOperationTime(stallTimeInfo, startTime)

        Dim sKadoTime As Date = kadoTime(0)
        Dim eKadoTime As Date = kadoTime(1)

        ' 衝突チェック
        If IsCollision(reserveList, reserveId, startTime, endTime) = False Then
            ' 衝突なし
            drReserveInfoTemp = DirectCast(reserveList.Select("REZID = " & CType(reserveId, String)),  _
                                      SC3150101DataSet.SC3150101StallReserveListRow())
            drReserveInfo = MoveReserve(drReserveInfoTemp(0), startTime, endTime)
            retReserveInfo.ImportRow(drReserveInfo)
            OutputLog("I", "[E]MoveReserveSub()", "", Nothing, _
                      "RETURN_VALUE:" & CType(retReserveInfo.Count, String))
            Return retReserveInfo
        End If

        For Each reserveListItem As SC3150101DataSet.SC3150101StallReserveListRow In _
            From r As SC3150101DataSet.SC3150101StallReserveListRow In reserveList _
            Where (startTime < r.ENDTIME) AndAlso (r.REZID <> reserveId)
            Order By r.STARTTIME
            ' 開始時間により干渉する選択チップ以外のチップをとりあえず移動対象とする
            targetList.Add(reserveListItem)
        Next

        chipTimeList = GetChipTimeList(breakInfo, targetList, eKadoTime)
        For Each tp As SC3150101DataSet.SC3150101ChipTimeRow In chipTimeList.Rows
            If (tp.STARTTIME < endTime) And (startTime < tp.ENDTIME) Then
                ' 動かせれない
                Return Nothing
            End If
        Next

        drReserveInfoTemp = DirectCast(reserveList.Select("REZID = " & CType(reserveId, String)),  _
                                        SC3150101DataSet.SC3150101StallReserveListRow())
        drReserveInfo = MoveReserve(drReserveInfoTemp(0), startTime, endTime)

        Dim drItem2 As SC3150101DataSet.SC3150101StallReserveListRow
        Do While targetList.Count > 0
            Dim i As Integer = 0
            Dim tp As SC3150101DataSet.SC3150101ChipTimeRow
            Dim et As DateTime ' 作業変数(endtime用)

            Do While i < targetList.Count
                drItem2 = CType(targetList(i), SC3150101DataSet.SC3150101StallReserveListRow)

                Dim chipStartTime As Date
                Dim chipEndTime As Date
                Dim chipInfo(3) As String
                Dim kind As Integer = 0
                If drItem2.Movable.Equals("1") Then
                    chipInfo = assortMovableChip(chipTimeList, _
                                                 stallTimeInfo, _
                                                 drItem2, _
                                                 dealerCode, _
                                                 branchCode, _
                                                 stallId, _
                                                 endTime, _
                                                 sKadoTime)

                    chipStartTime = Date.ParseExact(chipInfo(0), "yyyyMMddHHmm", CultureInfo.InvariantCulture)
                    chipEndTime = Date.ParseExact(chipInfo(1), "yyyyMMddHHmm", CultureInfo.InvariantCulture)

                    kind = CType(chipInfo(2), Integer)

                    Dim tpTemp() As SC3150101DataSet.SC3150101ChipTimeRow
                    If kind = 2 Then

                        tpTemp = DirectCast(chipTimeList.Select("REZID = " & CType(drItem2.REZID, String)),  _
                                                SC3150101DataSet.SC3150101ChipTimeRow())
                        tp = tpTemp(0)
                        tp.STARTTIME = drItem2.STARTTIME
                        tp.ENDTIME = drItem2.ENDTIME
                        targetList.RemoveAt(i)
                        Exit Do
                    End If
                    tpTemp = DirectCast(chipTimeList.Select("REZID = " & drItem2.REZID),  _
                                                SC3150101DataSet.SC3150101ChipTimeRow())
                    tp = tpTemp(0)
                    tp.STARTTIME = chipStartTime
                    tp.ENDTIME = chipEndTime
                    If kind = 3 Then

                        drItem2 = MoveReserve(drItem2, tp.STARTTIME, tp.ENDTIME)

                        ' 行をコピー(移動対象を戻り値に追加)
                        retReserveInfo.ImportRow(drItem2)

                        targetList.RemoveAt(i)

                        Exit Do
                    End If
                    If kind < 4 Then
                        i = i + 1
                    End If
                Else
                    targetList.RemoveAt(i)
                End If
            Loop

            et = New DateTime(9999, 12, 31, 23, 59, 59)

            For Each tp In chipTimeList.Rows
                If (tp.ENDTIME < et) AndAlso (endTime < tp.ENDTIME) Then
                    et = tp.ENDTIME
                End If
            Next
            endTime = et
        Loop

        OutputLog(LOG_TYPE_INFO, "[E]MoveReserveSub()", "", Nothing, _
                  "RETURN_VALUE:" & CType(retReserveInfo.Count, String))
        Return retReserveInfo

    End Function


    ''' <summary>
    ''' 予約日時変更
    ''' </summary>
    ''' <param name="startTime">予約開始日時</param>
    ''' <param name="endTime">予約終了日時</param>
    ''' <returns>日時変更した予約情報</returns>
    ''' <remarks></remarks>
    Private Function MoveReserve(ByVal drReserveList As SC3150101DataSet.SC3150101StallReserveListRow, _
                                 ByVal startTime As Date, ByVal endTime As Date) As SC3150101DataSet.SC3150101StallReserveListRow

        OutputLog(LOG_TYPE_INFO, "[S]MoveReserve()", "", Nothing, _
                  "REZ_INFO:(DataRow)", _
                  "STRCD:" & startTime.ToString(CultureInfo.InvariantCulture()))

        drReserveList.PrevStartTime = drReserveList.STARTTIME
        drReserveList.PrevEndTime = drReserveList.ENDTIME

        If (drReserveList.STARTTIME <> startTime) Or (drReserveList.ENDTIME <> endTime) Then
            drReserveList.STARTTIME = startTime
            drReserveList.ENDTIME = endTime
            drReserveList.Moved = "1"
        End If

        OutputLog(LOG_TYPE_INFO, "[E]MoveReserve()", "", Nothing, _
                  "RETURN_VALUE:(DataSet)" & CType(drReserveList.ItemArray.Count, String))
        Return drReserveList

    End Function


    ''' <summary>
    ''' 稼働時間の確定
    ''' </summary>
    ''' <param name="stallTimeInfo">ストール情報</param>
    ''' <param name="startTime">開始時間</param>
    ''' <returns>稼動開始時間、稼動終了時間</returns>
    ''' <remarks></remarks>
    Private Function GetOperationTime(ByVal stallTimeInfo As SC3150101DataSet.SC3150101StallTimeInfoDataTable, _
                                      ByVal startTime As Date) As Date()

        OutputLog(LOG_TYPE_INFO, "[S]GetOperationTime()", "", Nothing)

        Dim retTime(2) As Date

        ' 稼動時間帯を取得
        Dim drStallTimeInfo As SC3150101DataSet.SC3150101StallTimeInfoRow
        drStallTimeInfo = CType(stallTimeInfo.Rows(0), SC3150101DataSet.SC3150101StallTimeInfoRow)
        Dim operationStartTimet As TimeSpan
        Dim operationEndTime As TimeSpan
        operationStartTimet = SetStallTime(drStallTimeInfo.PSTARTTIME).TimeOfDay
        operationEndTime = SetStallTime(drStallTimeInfo.PENDTIME).TimeOfDay

        ' 対象の営業時間帯を確定する
        Dim sKadoTime As Date
        Dim eKadoTime As Date

        If startTime.Date.Add(operationStartTimet) > startTime.Date.Add(operationEndTime) Then
            ' 日跨ぎ稼動の場合、前日か当日かどちらの稼働時間帯かを判定
            If startTime.Date.AddDays(-1).Add(operationStartTimet) <= startTime _
                And startTime < startTime.Date.Add(operationEndTime) Then
                sKadoTime = startTime.Date.AddDays(-1).Add(operationStartTimet)
                eKadoTime = startTime.Date.Add(operationEndTime)
            Else
                sKadoTime = startTime.Date.Add(operationStartTimet)
                eKadoTime = startTime.Date.AddDays(1).Add(operationEndTime)
            End If
        Else
            ' 通常稼動の場合
            sKadoTime = startTime.Date.Add(operationStartTimet)
            eKadoTime = startTime.Date.Add(operationEndTime)
        End If

        retTime(0) = sKadoTime
        retTime(1) = eKadoTime

        OutputLog(LOG_TYPE_INFO, "[E]GetOperationTime()", "", Nothing)

        Return retTime

    End Function


    ''' <summary>
    ''' 移動チップ情報を取得
    ''' </summary>
    ''' <param name="breakInfo">休憩情報</param>
    ''' <param name="targetList">移動対象チップ情報</param>
    ''' <param name="eKadoTime">稼動終了時間</param>
    ''' <returns>チップ情報</returns>
    ''' <remarks></remarks>
    Private Function GetChipTimeList(ByVal breakInfo As SC3150101DataSet.SC3150101StallBreakInfoDataTable, _
                                     ByVal targetList As List(Of SC3150101DataSet.SC3150101StallReserveListRow), _
                                     ByVal eKadoTime As Date) As SC3150101DataSet.SC3150101ChipTimeDataTable

        OutputLog(LOG_TYPE_INFO, "[S]GetChipTimeList()", "", Nothing)

        Dim chipTimeList As New SC3150101DataSet.SC3150101ChipTimeDataTable

        Dim reserveItem As SC3150101DataSet.SC3150101StallReserveListRow
        For i As Integer = targetList.Count - 1 To 0 Step -1 ' 降順に取り出す

            Dim tb As New SC3150101DataSet.SC3150101ChipTimeDataTable
            Dim chipItem As SC3150101DataSet.SC3150101ChipTimeRow
            chipItem = CType(tb.NewRow(), SC3150101DataSet.SC3150101ChipTimeRow)
            ' データコピー用
            Dim drChipTimeInfo As SC3150101DataSet.SC3150101ChipTimeRow
            drChipTimeInfo = CType(chipTimeList.NewRow(), SC3150101DataSet.SC3150101ChipTimeRow)

            'drItem = CType(TargetList.GetByIndex(i), SC3150101DataSet.SC3150101StallReserveListRow)
            reserveItem = CType(targetList(i), SC3150101DataSet.SC3150101StallReserveListRow)

            If reserveItem.Movable.Equals("1") Then

                If (Not String.Equals(reserveItem.REZ_RECEPTION, "0")) _
                    And (Not IsDBNull(reserveItem.Item("REZ_DELI_DATE"))) Then
                    chipItem.ENDTIME = Date.ParseExact(reserveItem.REZ_DELI_DATE, "yyyyMMddHHmm", CultureInfo.InvariantCulture)
                Else
                    chipItem.ENDTIME = eKadoTime
                End If

                Dim dateTemp(START_TIME_ARRAY_NUMBER) As Date ' 一時格納用date型配列

                dateTemp = CalculateStartTime(breakInfo, _
                                              chipItem.ENDTIME, _
                                              CType(reserveItem.REZ_WORK_TIME, Integer), _
                                              convertBoolean(reserveItem.InBreak))
                chipItem.STARTTIME = dateTemp(START_TIME_START)
                chipItem.ENDTIME = dateTemp(START_TIME_END) '※既存処理ではByRefで戻り引数になっている

                Dim cl As Boolean
                Do
                    cl = False

                    Dim chipTime(3) As String
                    chipTime = GetChipStartTime(chipTimeList, breakInfo, chipItem.STARTTIME, _
                                                chipItem.ENDTIME, CType(reserveItem.REZ_WORK_TIME, Integer), _
                                                reserveItem.InBreak)
                    Date.ParseExact(chipTime(0), "yyyyMMddHHmm", CultureInfo.InvariantCulture)
                    chipItem.STARTTIME = Date.ParseExact(chipTime(0), "yyyyMMddHHmm", CultureInfo.InvariantCulture)
                    chipItem.ENDTIME = Date.ParseExact(chipTime(1), "yyyyMMddHHmm", CultureInfo.InvariantCulture)
                    cl = convertBoolean(chipTime(2))

                Loop While (cl = True)
            Else
                chipItem.STARTTIME = reserveItem.STARTTIME
                chipItem.ENDTIME = reserveItem.ENDTIME
            End If
            chipItem.REZID = reserveItem.REZID

            ' チップの予約ID、開始時間、終了時間を格納
            drChipTimeInfo.REZID = chipItem.REZID
            drChipTimeInfo.STARTTIME = chipItem.STARTTIME
            drChipTimeInfo.ENDTIME = chipItem.ENDTIME
            chipTimeList.Rows.Add(drChipTimeInfo)

        Next

        OutputLog(LOG_TYPE_INFO, "[E]GetChipTimeList()", "", Nothing)

        Return chipTimeList

    End Function


    ''' <summary>
    ''' 移動対象チップを分類する
    ''' </summary>
    ''' <param name="chipTimeList">チップ情報</param>
    ''' <param name="stallTimeInfo">ストール情報</param>
    ''' <param name="drItem2">予約情報</param>
    ''' <param name="dealerCode">販売店コード</param>
    ''' <param name="branchCode">店舗コード</param>
    ''' <param name="stallId">ストールID</param>
    ''' <param name="endTime">終了時間</param>
    ''' <param name="sKadoTime">稼動開始時間</param>
    ''' <returns>開始時間、終了時間、分類</returns>
    ''' <remarks></remarks>
    Private Function assortMovableChip(ByVal chipTimeList As SC3150101DataSet.SC3150101ChipTimeDataTable, _
                                       ByVal stallTimeInfo As SC3150101DataSet.SC3150101StallTimeInfoDataTable, _
                                       ByVal drItem2 As SC3150101DataSet.SC3150101StallReserveListRow, _
                                       ByVal dealerCode As String, _
                                       ByVal branchCode As String, _
                                       ByVal stallId As Integer, _
                                       ByVal endTime As Date, _
                                       ByVal sKadoTime As Date) As String()

        OutputLog(LOG_TYPE_INFO, "[S]assortMovableChip()", "", Nothing)

        Dim retChipInfo(3) As String
        Dim chipStartTime As Date
        Dim chipEndTime As Date
        Dim kind As Integer = 0
        Dim cl As Boolean
        Dim dateTemp(END_TIME_ARRAY_NUMBER) As Date ' 一時格納用date型配列

        If drItem2.Movable.Equals("1") Then
            kind = 1

            If Not String.Equals(drItem2.REZ_RECEPTION, "0") Then
                chipStartTime = Date.ParseExact(drItem2.REZ_PICK_DATE, "yyyyMMddHHmm", CultureInfo.InvariantCulture)
            Else
                chipStartTime = sKadoTime
            End If

            If chipStartTime < endTime Then
                dateTemp = CalculateEndTime(stallTimeInfo, _
                                            dealerCode, _
                                            branchCode, _
                                            stallId, _
                                            endTime, _
                                            CType(drItem2.REZ_WORK_TIME, Integer), _
                                            convertBoolean(drItem2.InBreak))
                chipEndTime = dateTemp(END_TIME_END)
                chipStartTime = dateTemp(END_TIME_START)
                endTime = dateTemp(END_TIME_START) '※
            Else
                dateTemp = CalculateEndTime(stallTimeInfo, _
                                            dealerCode, _
                                            branchCode, _
                                            stallId, _
                                            chipStartTime, _
                                            CType(drItem2.REZ_WORK_TIME, Integer), _
                                            convertBoolean(drItem2.InBreak))
                chipEndTime = dateTemp(END_TIME_END)
                chipStartTime = dateTemp(END_TIME_START) '※
            End If

            Do
                cl = False
                Dim chipTime(3) As String
                chipTime = GetChipEndTime(chipTimeList, stallTimeInfo, dealerCode, _
                                          branchCode, stallId, _
                                          CType(drItem2.REZID, Integer), _
                                          chipStartTime, _
                                          chipEndTime, _
                                          CType(drItem2.REZ_WORK_TIME, Integer), _
                                          drItem2.InBreak)
                cl = convertBoolean(chipTime(2))
                If cl = True Then
                    chipEndTime = Date.ParseExact(chipTime(0), "yyyyMMddHHmm", CultureInfo.InvariantCulture)
                    chipStartTime = Date.ParseExact(chipTime(1), "yyyyMMddHHmm", CultureInfo.InvariantCulture)
                End If

            Loop While (cl = True)

            ' 移動対象外判定?
            cl = IsMoveingChip(chipTimeList, chipStartTime, drItem2.STARTTIME, cl)
            If (cl = False) And (chipStartTime < drItem2.STARTTIME) Then

                kind = 2

                retChipInfo(0) = chipStartTime.ToString("yyyyMMddHHmm", CultureInfo.InvariantCulture)
                retChipInfo(1) = chipEndTime.ToString("yyyyMMddHHmm", CultureInfo.InvariantCulture)
                retChipInfo(2) = CType(kind, String)

                OutputLog(LOG_TYPE_INFO, "[E]assortMovableChip()", "", Nothing)

                Return retChipInfo
            End If

            If chipStartTime = endTime Then
                kind = 3
            End If

        Else
            kind = 4
        End If


        retChipInfo(0) = chipStartTime.ToString("yyyyMMddHHmm", CultureInfo.InvariantCulture)
        retChipInfo(1) = chipEndTime.ToString("yyyyMMddHHmm", CultureInfo.InvariantCulture)
        retChipInfo(2) = CType(kind, String)

        OutputLog(LOG_TYPE_INFO, "[E]assortMovableChip()", "", Nothing)

        Return retChipInfo

    End Function

    ''' <summary>
    ''' 移動対象チップの開始(終了)時間の確定
    ''' </summary>
    ''' <param name="chipTimeList">チップ情報</param>
    ''' <param name="breakInfo">ストール情報</param>
    ''' <param name="startTime">開始時間</param>
    ''' <param name="endTime">終了時間</param>
    ''' <param name="workTime">作業時間</param>
    ''' <param name="isBreak">休憩有無</param>
    ''' <returns>開始時間、終了時間、判定</returns>
    ''' <remarks></remarks>
    Private Function GetChipStartTime(ByVal chipTimeList As SC3150101DataSet.SC3150101ChipTimeDataTable, _
                                      ByVal breakInfo As SC3150101DataSet.SC3150101StallBreakInfoDataTable, _
                                      ByVal startTime As Date, ByVal endTime As Date, _
                                      ByVal workTime As Integer, ByVal isBreak As String) As String()

        OutputLog(LOG_TYPE_INFO, "[S]GetChipStartTime()", "", Nothing)

        Dim retChipTime(3) As String
        Dim startTimeTemp As Date
        Dim endTimeTemp As Date
        Dim st As DateTime
        Dim cl As Integer

        startTimeTemp = startTime
        endTimeTemp = endTime

        cl = 0
        st = New DateTime(9999, 12, 31, 23, 59, 59)
        For Each tp As SC3150101DataSet.SC3150101ChipTimeRow In chipTimeList.Rows
            If (tp.STARTTIME < endTimeTemp) And (startTimeTemp < tp.ENDTIME) Then
                cl = 1
                If tp.STARTTIME < st Then
                    st = tp.STARTTIME
                End If
            End If
        Next

        Dim dateTemp(START_TIME_ARRAY_NUMBER) As Date

        If cl = 1 Then
            endTimeTemp = st 'drTb.ENDTIME
            dateTemp = CalculateStartTime(breakInfo, _
                                          endTimeTemp, _
                                          workTime, _
                                          convertBoolean(isBreak))
            startTimeTemp = dateTemp(START_TIME_START) 'drTb.STARTTIME
            endTimeTemp = dateTemp(START_TIME_END) '※'drTb.ENDTIME
        End If

        retChipTime(0) = startTimeTemp.ToString("yyyyMMddHHmm", CultureInfo.InvariantCulture)
        retChipTime(1) = endTimeTemp.ToString("yyyyMMddHHmm", CultureInfo.InvariantCulture)
        retChipTime(2) = CType(cl, String)

        OutputLog(LOG_TYPE_INFO, "[E]GetChipStartTime()", "", Nothing)

        Return retChipTime

    End Function


    ''' <summary>
    ''' 移動対象チップの終了(開始)時間の確定
    ''' </summary>
    ''' <param name="chipTimeList">チップ情報</param>
    ''' <param name="stallTimeInfo">ストール情報</param>
    ''' <param name="startTime">開始時間</param>
    ''' <param name="endTime">終了時間</param>
    ''' <param name="workTime">作業時間</param>
    ''' <param name="isBreak">休憩有無</param>
    ''' <returns>開始時間、終了時間、判定</returns>
    ''' <remarks></remarks>
    Private Function GetChipEndTime(ByVal chipTimeList As SC3150101DataSet.SC3150101ChipTimeDataTable, _
                                    ByVal stallTimeInfo As SC3150101DataSet.SC3150101StallTimeInfoDataTable, _
                                    ByVal dealerCode As String, _
                                    ByVal branchCode As String, _
                                    ByVal stallId As Integer, _
                                    ByVal reserveId As Integer, _
                                    ByVal startTime As Date, _
                                    ByVal endTime As Date, _
                                    ByVal workTime As Integer, _
                                    ByVal isBreak As String) As String()

        OutputLog(LOG_TYPE_INFO, "[S]GetChipEndTime()", "", Nothing)

        Dim retChipTime(3) As String
        Dim startTimeTemp As Date
        Dim endTimeTemp As Date
        Dim et As DateTime
        Dim cl As Integer

        cl = 0
        startTimeTemp = startTime
        endTimeTemp = endTime
        et = New DateTime(1, 1, 1, 0, 0, 0)
        For Each tp As SC3150101DataSet.SC3150101ChipTimeRow In chipTimeList.Rows
            If (reserveId <> tp.REZID) And (tp.STARTTIME < endTimeTemp) _
                And (startTimeTemp < tp.ENDTIME) Then
                cl = 1
                If et < tp.ENDTIME Then
                    et = tp.ENDTIME
                End If
            End If
        Next

        Dim dateTemp(END_TIME_ARRAY_NUMBER) As Date

        If cl = 1 Then
            startTimeTemp = et
            dateTemp = CalculateEndTime(stallTimeInfo, _
                                        dealerCode, _
                                        branchCode, _
                                        stallId, _
                                        startTimeTemp, _
                                        workTime, _
                                        convertBoolean(isBreak))
            endTimeTemp = dateTemp(END_TIME_END)
            startTimeTemp = dateTemp(END_TIME_START) '※
        End If

        retChipTime(0) = endTimeTemp.ToString("yyyyMMddHHmm", CultureInfo.InvariantCulture)
        retChipTime(1) = startTimeTemp.ToString("yyyyMMddHHmm", CultureInfo.InvariantCulture)
        retChipTime(2) = CType(cl, String)

        OutputLog(LOG_TYPE_INFO, "[E]GetChipEndTime()", "", Nothing)

        Return retChipTime

    End Function


    ''' <summary>
    ''' 移動対象チップ判定？
    ''' </summary>
    ''' <param name="chipTimeList">チップ情報</param>
    ''' <param name="criterionStartTime">ストール情報</param>
    ''' <param name="startTime">開始時間</param>
    ''' <returns>移動対象：True、非移動対象：False</returns>
    ''' <remarks></remarks>
    Private Function IsMoveingChip(ByVal chipTimeList As SC3150101DataSet.SC3150101ChipTimeDataTable, _
                                   ByVal criterionStartTime As Date, _
                                   ByVal startTime As Date, _
                                   ByVal cl As Boolean) As Boolean

        OutputLog(LOG_TYPE_INFO, "[S]IsMoveingChip()", "", Nothing)

        Dim isMove As Boolean = cl

        If criterionStartTime < startTime Then
            isMove = False
            For Each tp As SC3150101DataSet.SC3150101ChipTimeRow In chipTimeList.Rows
                If (tp.STARTTIME < startTime) And (startTime < tp.ENDTIME) Then
                    OutputLog(LOG_TYPE_INFO, "[E]IsMoveingChip()", "", Nothing)
                    isMove = True
                End If
            Next
        End If

        OutputLog(LOG_TYPE_INFO, "[E]IsMoveingChip()", "", Nothing)

        Return isMove

    End Function


    ''' <summary>
    ''' 時間に変更のあった予約情報の更新
    ''' </summary>
    ''' <param name="reserveList">予約情報</param>
    ''' <param name="reserveId">更新対象外の予約ID</param>
    ''' <param name="dealerCode">販売店コード</param>
    ''' <param name="branchCode">店舗コード</param>
    ''' <param name="stallID">ストールID</param>
    ''' <param name="updateAccount">アカウント</param>
    ''' <returns>エラーが発生した場合、-1</returns>
    ''' <remarks></remarks>
    Public Function UpdateAllReserve(ByVal reserveList As SC3150101DataSet.SC3150101StallReserveListDataTable, _
                                     ByVal reserveId As Integer, _
                                     ByVal dealerCode As String, _
                                     ByVal branchCode As String, _
                                     ByVal stallId As Integer, _
                                     ByVal updateAccount As String) As Integer

        OutputLog(LOG_TYPE_INFO, "[S]UpdateAllReserve()", "", Nothing, _
                  "REZ_INFO:(DataSet)", "REZID:" & CType(reserveId, String), _
                  "DLRCD:" & dealerCode, "STRCD:" & branchCode, _
                  "STALLID:" & CType(stallId, String), "ACCOUNT:" & updateAccount)

        ' 引数チェック
        If reserveList Is Nothing Then
            ' 更新対象がない
            OutputLog(LOG_TYPE_WARNING, "UpdateAllReserve()", "Argument is nothing", Nothing)
            OutputLog(LOG_TYPE_INFO, "[E]UpdateAllReserve()", "", Nothing, _
                  "RETURN_VALUE:" & CType(RETURN_VALUE_OK, String))
            Return RETURN_VALUE_OK
        End If

        'Dim itm As ReserveInfo
        Dim reserveListItem As SC3150101DataSet.SC3150101StallReserveListRow

        Dim resultUpdRez As Integer
        Dim resultInsRezHis As Integer

        ' SC3150101TableAdapterクラスのインスタンスを生成
        Using adapter As New SC3150101DataSetTableAdapters.SC3150101StallInfoDataTableAdapter
            Using reserveInfo As New SC3150101DataSet.SC3150101StallReserveInfoDataTable
                Dim drReserveInfo As SC3150101DataSet.SC3150101StallReserveInfoRow
                For Each reserveListItem In reserveList.Rows

                    'If (reserveListItem.Moved = "1") And (reserveListItem.REZID <> reserveId) Then
                    If (reserveListItem.Moved.Equals("1")) _
                        And (Not String.Equals(reserveListItem.REZID, reserveId)) Then

                        drReserveInfo = CType(reserveInfo.NewRow(),  _
                                              SC3150101DataSet.SC3150101StallReserveInfoRow)

                        ' 更新データの設定
                        drReserveInfo.DLRCD = dealerCode
                        drReserveInfo.STRCD = branchCode
                        drReserveInfo.STALLID = stallId
                        drReserveInfo.REZID = reserveListItem.REZID
                        drReserveInfo.STARTTIME = reserveListItem.STARTTIME
                        drReserveInfo.ENDTIME = reserveListItem.ENDTIME
                        drReserveInfo.REZ_WORK_TIME = reserveListItem.REZ_WORK_TIME
                        drReserveInfo.STATUS = reserveListItem.STATUS
                        If IsDBNull(reserveListItem.Item("STRDATE")) Then
                            drReserveInfo.STRDATE = DateTime.MinValue
                        Else
                            drReserveInfo.STRDATE = reserveListItem.STRDATE
                        End If
                        drReserveInfo.WASHFLG = reserveListItem.WASHFLG
                        drReserveInfo.INSPECTIONFLG = reserveListItem.INSPECTIONFLG
                        drReserveInfo.STOPFLG = "0"
                        drReserveInfo.CANCELFLG = "0"
                        'RezItem.RestFlg = reserveListItem.RestFlg

                        ' データセットに行を追加
                        reserveInfo.Rows.Add(drReserveInfo)

                        ' ストール予約情報を更新する
                        resultUpdRez = adapter.UpdateStallReserveInfo(reserveInfo, _
                                                                      Date.MinValue, _
                                                                      Date.MaxValue, _
                                                                      OVERWRITE_NULL, _
                                                                      OVERWRITE_NULL, _
                                                                      updateAccount)
                        If (resultUpdRez <= 0) Then
                            OutputLog("I", "[E]UpdateAllReserve()", "", Nothing, _
                                      "RETURN_VALUE:" & CType(RETURN_VALUE_NG, String))
                            Return (RETURN_VALUE_NG)
                        End If

                        ' ストール予約履歴を登録する
                        'resultInsRezHis = adapter.InsertRezHistory(dealerCode, branchCode, reserveId, 1)
                        ' 2012.02.01 edit 移動した予約チップの履歴をつくるため引数を対象の予約IDになるように修正
                        resultInsRezHis = adapter.InsertReserveHistory(dealerCode, branchCode, _
                                                                       CType(drReserveInfo.REZID, Integer), 1)
                        If (resultInsRezHis <= 0) Then
                            OutputLog("I", "[E]UpdateAllReserve()", "", Nothing, _
                                      "RETURN_VALUE:" & CType(RETURN_VALUE_NG, String))
                            Return (RETURN_VALUE_NG)
                        End If

                        ' データクリア
                        reserveInfo.Clear()

                    End If
                Next
            End Using
        End Using

        OutputLog(LOG_TYPE_INFO, "[E]UpdateAllReserve()", "", Nothing, _
                  "RETURN_VALUE:" & CType(RETURN_VALUE_OK, String))
        Return RETURN_VALUE_OK

    End Function


    ''' <summary>
    ''' 稼動時間帯の確定
    ''' </summary>
    ''' <param name="reserveInfo">予約情報</param>
    ''' <param name="startTime">開始時刻</param>
    ''' <param name="startOperationTime">稼動開始時刻</param>
    ''' <param name="endOperationTime">稼動終了時刻</param>
    ''' <returns>稼動時間帯：0、非稼動時間帯：-1</returns>
    ''' <remarks></remarks>
    Private Function DecisionOperationTime(ByVal reserveInfo As SC3150101DataSet.SC3150101StallReserveInfoDataTable, _
                                           ByVal startTime As Date, _
                                           ByVal startOperationTime As TimeSpan, _
                                           ByVal endOperationTime As TimeSpan) As Integer

        OutputLog(LOG_TYPE_INFO, "[S]DecisionOperationTime()", "", Nothing, _
                  "REZ_INFO:(DataSet)", "START_TIME:" & CType(startTime, String), _
                  "KADO_START_TIME:" & startOperationTime.ToString(), _
                  "KADO_END_TIME:" & endOperationTime.ToString())

        ' 戻り値にエラーを設定
        DecisionOperationTime = RETURN_VALUE_NG
        Try
            Dim workOperationStartTime As DateTime     ' 作業開始時刻の稼動時間帯の開始時刻
            Dim scheduleOperationStartTime As DateTime ' 予定開始時刻の稼動時間帯の開始時刻
            Dim drReserveInfo As SC3150101DataSet.SC3150101StallReserveInfoRow
            drReserveInfo = CType(reserveInfo.Rows(0), SC3150101DataSet.SC3150101StallReserveInfoRow)

            If startTime.Date.Add(startOperationTime) < startTime.Date.Add(endOperationTime) Then
                ' 通常稼動の場合
                'If startTime.Date > drReserveInfo.ENDTIME.Date Or startTime.Date < drReserveInfo.STARTTIME.Date Then
                If (drReserveInfo.ENDTIME.Date < startTime.Date) _
                    Or (startTime.Date < drReserveInfo.STARTTIME.Date) Then
                    ' 稼働時間外開始
                    OutputLog(LOG_TYPE_ERROR, "DecisionOperationTime()", _
                              "Operation overtime start", Nothing)
                    Exit Try
                End If
            Else
                Dim kadoStartTimeTemp As Date ' 作業用変数
                Dim kadoEndTimeTemp As Date   ' 作業用変数

                '作業開始時刻の稼動時間帯の開始時刻を取得
                kadoStartTimeTemp = startTime.Date.AddDays(-1).Add(startOperationTime)
                kadoEndTimeTemp = startTime.Date.Add(endOperationTime)
                'If startTime.Date.AddDays(-1).Add(startOperationTime) <= startTime And startTime < startTime.Date.Add(endOperationTime) Then
                If (kadoStartTimeTemp <= startTime) And (startTime < kadoEndTimeTemp) Then
                    workOperationStartTime = startTime.AddDays(-1).Date.Add(startOperationTime)
                Else
                    workOperationStartTime = startTime.Date.Add(startOperationTime)
                End If

                ' 予定開始時刻の稼動時間帯の開始時刻を取得
                kadoStartTimeTemp = drReserveInfo.STARTTIME.Date.AddDays(-1).Add(startOperationTime)
                kadoEndTimeTemp = drReserveInfo.STARTTIME.Date.Add(endOperationTime)
                'If drReserveInfo.STARTTIME.Date.AddDays(-1).Add(startOperationTime) <= drReserveInfo.STARTTIME And drReserveInfo.STARTTIME < drReserveInfo.STARTTIME.Date.Add(endOperationTime) Then
                If (kadoStartTimeTemp <= drReserveInfo.STARTTIME) _
                    And (drReserveInfo.STARTTIME < kadoEndTimeTemp) Then
                    scheduleOperationStartTime = drReserveInfo.STARTTIME.AddDays(-1).Date.Add(startOperationTime)
                Else
                    scheduleOperationStartTime = drReserveInfo.STARTTIME.Date.Add(startOperationTime)
                End If

                ' 作業開始時刻の存在する稼動時間帯の開始時刻が、予定終了時刻がより後、または
                ' 作業開始時刻が、予定開始時刻の存在する稼動時間帯の開始時刻より前の場合エラー
                'If workOperationStartTime > drReserveInfo.ENDTIME Or startTime < scheduleOperationStartTime Then
                If (drReserveInfo.ENDTIME < workOperationStartTime) _
                    Or (startTime < scheduleOperationStartTime) Then
                    ' 稼働時間外開始
                    OutputLog(LOG_TYPE_ERROR, "DecisionOperationTime()", _
                              "Operation overtime start", Nothing)
                    Exit Try
                End If
            End If

            ' 正常終了
            DecisionOperationTime = RETURN_VALUE_OK

        Finally
            OutputLog(LOG_TYPE_INFO, "[E]DecisionOperationTime()", "", Nothing, _
                      "RETURN_VALUE:" & DecisionOperationTime.ToString(CultureInfo.CurrentCulture))
        End Try

        Return DecisionOperationTime

    End Function


    ''' <summary>
    ''' 二重作業開始チェック
    ''' </summary>
    ''' <param name="stallId">ストールID</param>
    ''' <param name="startTime">開始時刻</param>
    ''' <param name="startOperationTime">稼動開始時刻</param>
    ''' <param name="endOperationTime">稼動終了時刻</param>
    ''' <returns>開始可：0、開始不可：-1</returns>
    ''' <remarks></remarks>
    Private Function CheckMultiStarts(ByVal stallId As Integer, _
                                      ByVal startTime As Date, _
                                      ByVal startOperationTime As TimeSpan, _
                                      ByVal endOperationTime As TimeSpan) As Integer

        OutputLog(LOG_TYPE_INFO, "[S]CheckMultiStarts()", "", Nothing, _
                  "STALLID:" & CType(stallId, String), "START_TIME:" & CType(startTime, String), _
                  "KADO_START_TIME:" & startOperationTime.ToString(), _
                  "KADO_END_TIME:" & endOperationTime.ToString())

        Dim operationStart As Date
        Dim operationEnd As Date
        If startTime.Date.Add(startOperationTime) < startTime.Date.Add(endOperationTime) Then
            ' 通常稼動の場合
            operationStart = startTime.Date.Add(startOperationTime)
            operationEnd = startTime.Date.Add(endOperationTime)
        Else
            Dim kadoStartTimeTemp As Date ' 作業用変数
            Dim kadoEndTimeTemp As Date   ' 作業用変数
            kadoStartTimeTemp = startTime.Date.AddDays(-1).Add(startOperationTime)
            kadoEndTimeTemp = startTime.Date.Add(endOperationTime)
            ' 日跨ぎ稼動の場合
            'If startTime.Date.AddDays(-1).Add(startOperationTime) <= startTime And startTime < startTime.Date.Add(endOperationTime) Then
            If (kadoStartTimeTemp <= startTime) And (startTime < kadoEndTimeTemp) Then
                operationStart = startTime.AddDays(-1).Date.Add(startOperationTime)
                operationEnd = startTime.Date.Add(endOperationTime)
            Else
                operationStart = startTime.Date.Add(startOperationTime)
                operationEnd = startTime.AddDays(1).Date.Add(endOperationTime)
            End If
        End If

        ' SC3150101TableAdapterクラスのインスタンスを生成
        Using adapter As New SC3150101DataSetTableAdapters.SC3150101StallInfoDataTableAdapter

            ' 作業中の数を取得
            Dim workingState As SC3150101DataSet.SC3150101WorkingStateCountDataTable
            workingState = adapter.GetWorkingStateCount(stallId, operationStart, operationEnd)
            Dim drWorkingState As SC3150101DataSet.SC3150101WorkingStateCountRow
            drWorkingState = CType(workingState.Rows(0), SC3150101DataSet.SC3150101WorkingStateCountRow)
            ' 作業開始数の確認
            If drWorkingState.COUNT > 0 Then
                ' すでに作業開始されている
                OutputLog(LOG_TYPE_ERROR, "CheckMultiStarts()", _
                          "Other tips are already working", Nothing)
                OutputLog(LOG_TYPE_INFO, "[E]CheckMultiStarts()", "", Nothing, _
                          "RETURN_VALUE:" & CType(-1, String))
                Return (-1)
            End If

            OutputLog(LOG_TYPE_INFO, "[E]CheckMultiStarts()", "", Nothing, _
                          "RETURN_VALUE:" & CType(0, String))
            Return 0

        End Using

    End Function


    '2012/02/28 KN 渡辺 【SERVICE_1】関連チップの順不同開始を抑制するように修正 START
    ''' <summary>
    ''' 子予約連番による開始可否チェック
    ''' </summary>
    ''' <param name="dealerCode">販売店コード</param>
    ''' <param name="branchCode">店舗コード</param>
    ''' <param name="reserveId">予約ID</param>
    ''' <param name="childNo">子予約連番（0,1は親チップ）</param>
    ''' <returns>開始可：0、開始不可：-1</returns>
    ''' <remarks></remarks>
    Private Function CheckReserveChildNo(ByVal dealerCode As String, _
                                           ByVal branchCode As String, _
                                           ByVal reserveId As Integer, _
                                           ByVal childNo As Long) As Integer

        OutputLog(LOG_TYPE_INFO, "[S]CheckReserveChildNo()", "", Nothing, _
                  "DLRCD:" & dealerCode, "STRCD:" & branchCode, "REZID:" & CType(reserveId, String), "CHILDNO:" & CType(childNo, String))

        ' SC3150101TableAdapterクラスのインスタンスを生成
        Using adapter As New SC3150101DataSetTableAdapters.SC3150101StallInfoDataTableAdapter

            '開始可否の返り値
            Dim checkValue As Integer = -1
            ' 管理予約ID(PREZID)を取得
            Dim parentsReserveIdInfo As SC3150101DataSet.SC3150101ParentsReserveIdDataTable
            parentsReserveIdInfo = adapter.GetParentsReserveId(dealerCode, branchCode, reserveId)

            If (parentsReserveIdInfo IsNot Nothing) And (parentsReserveIdInfo.Count <> 0) Then
                Dim drParentsReserveIdInfo As SC3150101DataSet.SC3150101ParentsReserveIdRow
                drParentsReserveIdInfo = CType(parentsReserveIdInfo.Rows(0),  _
                                               SC3150101DataSet.SC3150101ParentsReserveIdRow)
                ' 単独チップでなければ次の処理実行
                If Not IsDBNull(drParentsReserveIdInfo.Item("PREZID")) Then
                    Dim ParentsReserveId As Integer = CType(drParentsReserveIdInfo.PREZID, Integer) ' 管理予約ID
                    'リレーション内の作業終了(97)チップの最大子予約連番(REZCHILDNO)を取得
                    Dim relationLastChildNoInfo As SC3150101DataSet.SC3150101RelationLastChildNoDataTable
                    relationLastChildNoInfo = adapter.GetRelationLastChildNo(dealerCode, _
                                                                             branchCode, _
                                                                             ParentsReserveId)
                    Dim drRelationLastChildNoInfo As SC3150101DataSet.SC3150101RelationLastChildNoRow
                    drRelationLastChildNoInfo = CType(relationLastChildNoInfo.Rows(0),  _
                                                      SC3150101DataSet.SC3150101RelationLastChildNoRow)
                    Dim maxFinishedChildNo As Integer
                    '作業終了しているリレーションチップの最大子番号を取得する.
                    If IsNothing(drRelationLastChildNoInfo) _
                        Or IsDBNull(drRelationLastChildNoInfo.Item("REZCHILDNO")) Then
                        maxFinishedChildNo = 0
                    Else
                        maxFinishedChildNo = CType(drRelationLastChildNoInfo.REZCHILDNO, Integer)
                    End If

                    '作業終了しているリレーションチップの最大子番号に1インクリメントした値が、開始可能な子番号である.
                    '選択されている子番号が開始可能な子番号である場合、開始可：0を返す.
                    If (childNo = (maxFinishedChildNo + 1)) Then
                        checkValue = 0
                    Else
                        checkValue = -1
                    End If
                Else
                    '単独チップの場合、開始可：0を返す.
                    checkValue = 0
                End If
            Else
                'チップ予約情報が取得できなかった場合、開始不可：-1を返す.
                checkValue = -1
            End If

            OutputLog(LOG_TYPE_INFO, "[E]CheckReserveChildNo()", "", Nothing, _
                                      "RETURN_VALUE:" & CType(checkValue, String))
            Return checkValue

        End Using

    End Function
    '2012/02/28 KN 渡辺 【SERVICE_1】関連チップの順不同開始を抑制するように修正 END


    ''' <summary>
    ''' 子予約連番の再割振
    ''' </summary>
    ''' <param name="dealerCode">販売店コードID</param>
    ''' <param name="branchCode">店舗コード</param>
    ''' <param name="reserveId">予約ID</param>
    ''' <returns>子予約連番、エラー：-99</returns>
    ''' <remarks></remarks>
    Private Function ReorderReserveChildNo(ByVal dealerCode As String, _
                                           ByVal branchCode As String, _
                                           ByVal reserveId As Integer) As Integer

        OutputLog(LOG_TYPE_INFO, "[S]ReorderReserveChildNo()", "", Nothing, _
                  "DLRCD:" & dealerCode, "STRCD:" & branchCode, "REZID:" & CType(reserveId, String))

        ' SC3150101TableAdapterクラスのインスタンスを生成
        Using adapter As New SC3150101DataSetTableAdapters.SC3150101StallInfoDataTableAdapter

            Dim childNo As Integer = -1

            ' 管理予約ID(PREZID)を取得
            Dim parentsReserveIdInfo As SC3150101DataSet.SC3150101ParentsReserveIdDataTable
            parentsReserveIdInfo = adapter.GetParentsReserveId(dealerCode, branchCode, reserveId)

            If (parentsReserveIdInfo IsNot Nothing) And (parentsReserveIdInfo.Count <> 0) Then
                Dim drParentsReserveIdInfo As SC3150101DataSet.SC3150101ParentsReserveIdRow
                drParentsReserveIdInfo = CType(parentsReserveIdInfo.Rows(0),  _
                                               SC3150101DataSet.SC3150101ParentsReserveIdRow)
                ' 単独チップでなければ次の処理実行
                If Not IsDBNull(drParentsReserveIdInfo.Item("PREZID")) Then
                    Dim ParentsReserveId As Integer = CType(drParentsReserveIdInfo.PREZID, Integer) ' 管理予約ID
                    'リレーション内の作業終了(97)チップの最大子予約連番(REZCHILDNO)を取得
                    Dim relationLastChildNoInfo As SC3150101DataSet.SC3150101RelationLastChildNoDataTable
                    relationLastChildNoInfo = adapter.GetRelationLastChildNo(dealerCode, _
                                                                             branchCode, _
                                                                             ParentsReserveId)
                    Dim drRelationLastChildNoInfo As SC3150101DataSet.SC3150101RelationLastChildNoRow
                    drRelationLastChildNoInfo = CType(relationLastChildNoInfo.Rows(0),  _
                                                      SC3150101DataSet.SC3150101RelationLastChildNoRow)
                    Dim maxFinishedChildNo As Integer
                    ' 最大子予約連番を設定
                    If IsNothing(drRelationLastChildNoInfo) _
                        Or IsDBNull(drRelationLastChildNoInfo.Item("REZCHILDNO")) Then
                        maxFinishedChildNo = 0
                    Else
                        maxFinishedChildNo = CType(drRelationLastChildNoInfo.REZCHILDNO, Integer)
                    End If
                    ' リレーション内の子予約連番(REZCHILDNO)更新対象を取得
                    Dim childNoUpdateTarget As SC3150101DataSet.SC3150101TargetChildNoInfoDataTable
                    childNoUpdateTarget = adapter.GetChildNoUpdateTarget(dealerCode, branchCode, _
                                                                         ParentsReserveId, _
                                                                         maxFinishedChildNo, _
                                                                         reserveId)
                    Dim drChildNoUpdateTarget As SC3150101DataSet.SC3150101TargetChildNoInfoRow
                    '最初のレコードをmaxFinishedChildNo+1 で更新、以降は前レコードの+1で更新
                    Dim resultUpdateChildNo As Integer
                    Dim tempChildNo As Integer
                    tempChildNo = maxFinishedChildNo
                    tempChildNo = tempChildNo + 1
                    For Each drChildNoUpdateTarget In childNoUpdateTarget.Rows
                        'tmpChildNo = tmpChildNo + 1
                        ' 子予約連番の更新
                        resultUpdateChildNo = adapter.UpdateChildNo(dealerCode, branchCode, _
                                                                    CType(drChildNoUpdateTarget.REZID, Integer), _
                                                                    tempChildNo)

                        If resultUpdateChildNo < 0 Then
                            ' ロールバック
                            Me.Rollback = True
                            ' 子予約連番の更新に失敗
                            OutputLog(LOG_TYPE_ERROR, "ReorderReserveChildNo()", _
                                      "It is failed by update of 'REZCHILDNO'", Nothing)
                            OutputLog(LOG_TYPE_INFO, "[E]ReorderReserveChildNo()", "", Nothing, _
                                      "RETURN_VALUE:" & CType(-99, String))
                            Return -99
                        End If

                        ' 子予約連番のインクリメント
                        tempChildNo = tempChildNo + 1
                    Next

                    childNo = maxFinishedChildNo + 1
                End If
            End If

            OutputLog(LOG_TYPE_INFO, "[E]ReorderReserveChildNo()", "", Nothing, _
                                      "RETURN_VALUE:" & CType(childNo, String))
            Return childNo

        End Using

    End Function


    ''' <summary>
    ''' 時間を見直す
    ''' </summary>
    ''' <param name="startTime">開始時間</param>
    ''' <param name="endTime">終了時間</param>
    ''' <param name="interval">インターバル</param>
    ''' <returns>開始時間、終了時間</returns>
    ''' <remarks></remarks>
    Private Function RevisionTime(ByVal startTime As Date, ByVal endTime As Date, ByVal interval As Integer) As Date()

        OutputLog(LOG_TYPE_INFO, "[S]RevisionTime()", "", Nothing)

        Dim dateArray(2) As Date
        Dim timeDiff As Integer
        Dim startTimeRevision As Date
        Dim endTimeRevision As Date

        timeDiff = CType(startTime.Minute Mod interval, Integer)
        If timeDiff > 0 Then
            startTimeRevision = startTime.AddMinutes(interval - timeDiff)
        Else
            startTimeRevision = startTime
        End If
        timeDiff = CType(endTime.Minute Mod interval, Integer)
        If timeDiff > 0 Then
            endTimeRevision = endTime.AddMinutes(interval - timeDiff)
        Else
            endTimeRevision = endTime
        End If

        dateArray(0) = startTimeRevision
        dateArray(1) = endTimeRevision

        OutputLog(LOG_TYPE_INFO, "[E]RevisionTime()", "", Nothing)

        Return dateArray

    End Function


    ''' <summary>
    ''' DBNullのストール予約情報項目にデフォルト値を設定する
    ''' </summary>
    ''' <param name="reserveInfo">ストール予約情報</param>
    ''' <returns>予約情報</returns>
    ''' <remarks></remarks>
    Private Function SetStallReserveDefaultValue(ByVal reserveInfo As SC3150101DataSet.SC3150101StallReserveInfoDataTable) As SC3150101DataSet.SC3150101StallReserveInfoDataTable

        OutputLog(LOG_TYPE_INFO, "[S]SetStallReserveDefaultValue()", "", Nothing, "REZ_INFO:(DataSet)")

        Dim drReserveInfo As SC3150101DataSet.SC3150101StallReserveInfoRow
        drReserveInfo = DirectCast(reserveInfo.Rows(0), SC3150101DataSet.SC3150101StallReserveInfoRow)

        drReserveInfo.DLRCD = SetStringData(drReserveInfo.Item("DLRCD"), "")                                           ' 販売店コード
        drReserveInfo.STRCD = SetStringData(drReserveInfo.Item("STRCD"), "")                                           ' 店舗コード
        drReserveInfo.REZID = SetNumericData(drReserveInfo.Item("REZID"), 0)                                           ' 予約ID
        drReserveInfo.STALLID = SetNumericData(drReserveInfo.Item("STALLID"), 0)                                       ' ストールID
        If IsDBNull(drReserveInfo.Item("STARTTIME")) Then
            drReserveInfo.STARTTIME = DateTime.MinValue                                                                ' 使用開始日時
        End If
        If IsDBNull(drReserveInfo.Item("ENDTIME")) Then
            drReserveInfo.ENDTIME = DateTime.MinValue                                                                  ' 使用終了日時
        End If
        drReserveInfo.REZ_WORK_TIME = SetNumericData(drReserveInfo.Item("REZ_WORK_TIME"), 0)                           ' 予定_作業時間
        drReserveInfo.REZ_RECEPTION = SetStringData(drReserveInfo.Item("REZ_RECEPTION"), "0")                          ' 予約_受付納車区分
        'drReserveInfo.REZ_PICK_DATE = CType(ParseDate(SetStringData(drReserveInfo.Item("REZ_PICK_DATE"), "")), String) ' 予約_取引_希望日時時刻
        drReserveInfo.REZ_PICK_LOC = SetStringData(drReserveInfo.Item("REZ_PICK_LOC"), "")                             ' 予約_取引_場所
        drReserveInfo.REZ_PICK_TIME = SetNumericData(drReserveInfo.Item("REZ_PICK_TIME"), 0)                           ' 予約_取引_所要時間
        'drReserveInfo.REZ_DELI_DATE = CType(ParseDate(SetStringData(drReserveInfo.Item("REZ_DELI_DATE"), "")), String) ' 予約_納車_希望日時時刻
        drReserveInfo.REZ_DELI_LOC = SetStringData(drReserveInfo.Item("REZ_DELI_LOC"), "")                             ' 予約_納車_場所
        drReserveInfo.REZ_DELI_TIME = SetNumericData(drReserveInfo.Item("REZ_DELI_TIME"), 0)                           ' 予約_納車_所要時間
        drReserveInfo.STATUS = SetNumericData(drReserveInfo.Item("STATUS"), 0)                                         ' ステータス
        If IsDBNull(drReserveInfo.Item("STRDATE")) Then
            drReserveInfo.STRDATE = DateTime.MinValue                                                                  ' 入庫日時
        End If
        drReserveInfo.WASHFLG = SetStringData(drReserveInfo.Item("WASHFLG"), "0")                                      ' 洗車フラグ
        drReserveInfo.INSPECTIONFLG = SetStringData(drReserveInfo.Item("INSPECTIONFLG"), "0")                          ' 検査フラグ
        drReserveInfo.STOPFLG = SetStringData(drReserveInfo.Item("STOPFLG"), "0")                                      ' 中断フラグ
        drReserveInfo.CANCELFLG = SetStringData(drReserveInfo.Item("CANCELFLG"), "0")                                  ' キャンセルフラグ
        drReserveInfo.DELIVERY_FLG = SetStringData(drReserveInfo.Item("DELIVERY_FLG"), "0")                            ' 納車済フラグ
        '2012/02/28 KN 渡辺 【SERVICE_1】関連チップの順不同開始を抑制するように修正 START
        drReserveInfo.REZCHILDNO = SetNumericData(drReserveInfo.Item("REZCHILDNO"), 0)
        '2012/02/28 KN 渡辺 【SERVICE_1】関連チップの順不同開始を抑制するように修正 END

        OutputLog(LOG_TYPE_INFO, "[E]SetStallReserveDefaultValue()", "", Nothing, "RETURN_VALUE:REZ_INFO(DataSet)")
        Return reserveInfo

    End Function


    ''' <summary>
    ''' DBNullのストール実績リスト情報項目にデフォルト値を設定する
    ''' </summary>
    ''' <param name="ProcessInfo">ストール実績リスト情報</param>
    ''' <returns>実績情報</returns>
    ''' <remarks></remarks>
    Private Function SetStallProcessListDefaultValue(ByVal ProcessInfo As SC3150101DataSet.SC3150101StallProcessListDataTable) As SC3150101DataSet.SC3150101StallProcessListDataTable

        OutputLog(LOG_TYPE_INFO, "[S]SetStallProcessListDefaultValue()", "", Nothing, "PROC_INFO:(DataSet)")

        Dim drProcessInfo As SC3150101DataSet.SC3150101StallProcessListRow

        For Each drProcessInfo In ProcessInfo.Rows
            drProcessInfo.DLRCD = SetStringData(drProcessInfo.Item("DLRCD"), "")
            drProcessInfo.STRCD = SetStringData(drProcessInfo.Item("STRCD"), "")
            drProcessInfo.REZID = SetNumericData(drProcessInfo.Item("REZID"), 0)
            drProcessInfo.DSEQNO = SetNumericData(drProcessInfo.Item("DSEQNO"), 0)
            drProcessInfo.SEQNO = SetNumericData(drProcessInfo.Item("SEQNO"), 0)
            drProcessInfo.RESULT_STATUS = SetStringData(drProcessInfo.Item("RESULT_STATUS"), "0")
            drProcessInfo.RESULT_STALLID = SetNumericData(drProcessInfo.Item("RESULT_STALLID"), 0)
            'drProcessInfo.RESULT_START_TIME = ParseDate(SetStringData(drProcessInfo.Item("RESULT_START_TIME"), ""))
            drProcessInfo.RESULT_START_TIME = SetStringData(drProcessInfo.Item("RESULT_START_TIME"), "")
            drProcessInfo.RESULT_END_TIME = SetStringData(drProcessInfo.Item("RESULT_END_TIME"), "")
            drProcessInfo.RESULT_WORK_TIME = SetNumericData(drProcessInfo.Item("RESULT_WORK_TIME"), 0)
            drProcessInfo.RESULT_IN_TIME = SetStringData(drProcessInfo.Item("RESULT_IN_TIME"), "")
            drProcessInfo.RESULT_WASH_START = SetStringData(drProcessInfo.Item("RESULT_WASH_START"), "")
            drProcessInfo.RESULT_WASH_END = SetStringData(drProcessInfo.Item("RESULT_WASH_END"), "")
            drProcessInfo.RESULT_INSPECTION_START = SetStringData(drProcessInfo.Item("RESULT_INSPECTION_START"), "")
            drProcessInfo.RESULT_INSPECTION_END = SetStringData(drProcessInfo.Item("RESULT_INSPECTION_END"), "")
            drProcessInfo.RESULT_WAIT_START = SetStringData(drProcessInfo.Item("RESULT_WAIT_START"), "")
            drProcessInfo.RESULT_WAIT_END = SetStringData(drProcessInfo.Item("RESULT_WAIT_END"), "")
        Next

        OutputLog(LOG_TYPE_INFO, "[E]SetStallProcessListDefaultValue()", "", Nothing, _
                  "RETURN_VALUE:PROC_INFO(DataSet)")
        Return ProcessInfo

    End Function


    ''' <summary>
    ''' YYYYMMDDHHMMの形式の文字列をDateTime型に変換する
    ''' </summary>
    ''' <param name="Value">YYYYMMDDHHMMの形式の文字列</param>
    ''' <returns>変換値</returns>
    ''' <remarks></remarks>
    Private Function SetStallTime(ByVal value As String) As Date

        'Logger.Info("[S]SetStallTime()")
        OutputLog(LOG_TYPE_INFO, "[S]SetStallTime()", "", Nothing, "DATE:" & value)


        Dim ret As Date = DateTime.Now
        Dim hour As Integer
        Dim minute As Integer
        Dim retValue As Date

        If IsDBNull(value) Then 'If IsDBNull(value) = True Then
            OutputLog(LOG_TYPE_INFO, "[E]SetStallTime()", "", Nothing, _
                      "RETURN_VALUE:" & DateTime.MinValue.ToString(CultureInfo.InvariantCulture()))
            Return DateTime.MinValue
        End If

        If String.IsNullOrWhiteSpace(value) Then 'If value.Trim() = "" Then
            OutputLog(LOG_TYPE_INFO, "[E]SetStallTime()", "", Nothing, _
                      "RETURN_VALUE:" & DateTime.MinValue.ToString(CultureInfo.InvariantCulture()))
            Return DateTime.MinValue
        End If

        hour = CType(value.Substring(0, 2), Integer)
        minute = CType(value.Substring(3, 2), Integer)

        retValue = New DateTime(ret.Year, ret.Month, ret.Day, hour, minute, 0)

        'Logger.Info("[E]SetStallTime()")
        OutputLog(LOG_TYPE_INFO, "[E]SetStallTime()", "", Nothing, _
                  "RETURN_VALUE:" & retValue.ToString(CultureInfo.InvariantCulture()))
        Return retValue

    End Function


    ''' <summary>
    ''' 日時変換
    ''' </summary>
    ''' <param name="value">日付文字列</param>
    ''' <returns>変換値</returns>
    ''' <remarks></remarks>
    Private Function ParseDate(ByVal Value As String) As Date
        ' Protected

        OutputLog(LOG_TYPE_INFO, "[S]ParseDate()", "", Nothing, "DATE:" & Value)

        Dim ret As Date

        Dim year As Integer = Integer.Parse(Value.Substring(0, 4), CultureInfo.InvariantCulture())
        Dim month As Integer = Integer.Parse(Value.Substring(4, 2), CultureInfo.InvariantCulture())
        Dim day As Integer = Integer.Parse(Value.Substring(6, 2), CultureInfo.InvariantCulture())
        Dim hour As Integer = Integer.Parse(Value.Substring(8, 2), CultureInfo.InvariantCulture())
        Dim minute As Integer = Integer.Parse(Value.Substring(10, 2), CultureInfo.InvariantCulture())

        ret = New Date(year, month, day, hour, minute, 0)

        OutputLog(LOG_TYPE_INFO, "[E]ParseDate()", "", Nothing, "RETURN_VALUE:" & CType(ret, String))
        Return ret

    End Function


    ''' <summary>
    ''' 日付を表す文字列からDateTimeを生成し返却する
    ''' 引数から年・月・日として妥当な値を取得できない場合、結果は保証しない
    ''' </summary>
    ''' <param name="YYYYMMDDText">時刻を表す文字列"YYYYMMDD"</param>
    ''' <returns>日付を表すDateTime(時刻は持たない)</returns>
    ''' <remarks></remarks>
    Private Function YYYYMMDDTextToDateTime(ByVal YYYYMMDDText As String) As Date

        OutputLog(LOG_TYPE_INFO, "[S]YYYYMMDDTextToDateTime()", "", Nothing, "DATE:" & YYYYMMDDText)

        Dim ret As Date
        Dim y As Integer
        Dim m As Integer
        Dim d As Integer


        y = Integer.Parse(YYYYMMDDText.Substring(0, 4), CultureInfo.InvariantCulture())
        m = Integer.Parse(YYYYMMDDText.Substring(4, 2), CultureInfo.InvariantCulture())
        d = Integer.Parse(YYYYMMDDText.Substring(6, 2), CultureInfo.InvariantCulture())

        ret = New DateTime(y, m, d, 0, 0, 0)

        OutputLog(LOG_TYPE_INFO, "[E]YYYYMMDDTextToDateTime()", "", Nothing, _
                  "RETURN_VALUE:" & ret.ToString(CultureInfo.InvariantCulture()))
        Return ret

    End Function


    ''' <summary>
    ''' 時刻を表す文字列からDateTimeを生成し返却する
    ''' 引数から時・分として妥当な値を取得できない場合、結果は保証しない
    ''' </summary>
    ''' <param name="HHMMText">時刻を表す文字列"HHMM" or "HH:MM"</param>
    ''' <returns>時刻を表すDateTime(日は持たない)</returns>
    ''' <remarks></remarks>
    Private Function HHMMTextToDateTime(ByVal HHMMText As String) As Date

        OutputLog(LOG_TYPE_INFO, "[S]HHMMTextToDateTime()", "", Nothing, "DATE:" & HHMMText)

        Dim retDate As Date
        Dim hours As Integer
        Dim minutes As Integer


        hours = Integer.Parse(HHMMText.Substring(0, 2), CultureInfo.InvariantCulture())
        If HHMMText.Length = 4 Then
            minutes = Integer.Parse(HHMMText.Substring(2, 2), CultureInfo.InvariantCulture())
        ElseIf HHMMText.Length = 5 Then
            minutes = Integer.Parse(HHMMText.Substring(3, 2), CultureInfo.InvariantCulture())
        Else
            OutputLog(LOG_TYPE_ERROR, "HHMMTextToDateTime()", "Argument error", Nothing)
            OutputLog(LOG_TYPE_INFO, "[E]HHMMTextToDateTime()", "", Nothing)
            'Throw New Exception("SC3150101BusinessLogic")
            Throw New ArgumentException("Argument is character string except HHMM or HH:MM")
        End If

        retDate = New DateTime(1, 1, 1, hours, minutes, 0)

        OutputLog(LOG_TYPE_INFO, "[E]HHMMTextToDateTime()", "", Nothing, _
                  "RETURN_VALUE:" & retDate.ToString(CultureInfo.InvariantCulture()))
        Return retDate

    End Function

    ''' <summary>
    ''' オブジェクトの文字列値を取得し返却する
    ''' </summary>
    ''' <param name="obj">DBから取得した文字列 or DBNull</param>
    ''' <returns>文字列。DBNullの場合、空文字列</returns>
    ''' <remarks></remarks>
    Private Function StringValueOfDB(ByVal obj As Object) As String
        If Convert.IsDBNull(obj) Then
            Return String.Empty
        End If
        Return CType(obj, String)
    End Function


    ''' <summary>
    ''' DBNullのデータをデフォルト値で返す
    ''' </summary>
    ''' <param name="src"></param>
    ''' <param name="defult">デフォルト値</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function SetStringData(ByVal src As Object, ByVal defult As String) As String

        Dim returnValue As String

        If IsDBNull(src) = True Then
            returnValue = defult
        Else
            returnValue = DirectCast(src, String)
        End If

        Return returnValue

    End Function


    ''' <summary>
    ''' DBNullのデータをデフォルト値で返す
    ''' </summary>
    ''' <param name="src"></param>
    ''' <param name="defult">デフォルト値</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function SetNumericData(ByVal src As Object, ByVal defult As Integer) As Long

        Dim returnValue As Long

        If IsDBNull(src) = True Then
            returnValue = defult
        Else
            returnValue = DirectCast(src, Long)
        End If

        Return returnValue

    End Function


    ''' <summary>
    ''' boolean値に変換する
    ''' 1：true、それ以外：false
    ''' </summary>
    ''' <param name="breakFlg">フラグ</param>
    ''' <returns>変換値</returns>
    ''' <remarks></remarks>
    Private Function convertBoolean(ByVal breakFlg As String) As Boolean

        If breakFlg.Equals("1") Then 'If breakFlg = "1" Then
            Return True
        Else
            Return False
        End If

    End Function

    ''' <summary>
    ''' ログを出力する
    ''' </summary>
    ''' <param name="logLevel">ログレベル</param>
    ''' <param name="functionName">関数名</param>
    ''' <param name="message">メッセージ</param>
    ''' <param name="ex">例外</param>
    ''' <param name="values"></param>
    ''' <remarks></remarks>
    Private Sub OutputLog(ByVal logLevel As String, _
                          ByVal functionName As String, _
                          ByVal message As String, _
                          ByVal ex As Exception, _
                          ByVal ParamArray values() As String)

        Dim i As Integer
        Dim logMessage As New System.Text.StringBuilder
        logMessage.Append("")

        For i = 0 To values.Length() - 1
            logMessage.Append("[").Append(values(i)).Append("]")
        Next i

        Dim logData As New System.Text.StringBuilder
        If LOG_TYPE_INFO.Equals(logLevel) Then
            logData.Append("")
            logData.Append(functionName).Append(" ").Append(logMessage.ToString()).Append(" ").Append(message)
            Logger.Info(logData.ToString())
        ElseIf LOG_TYPE_ERROR.Equals(logLevel) Then
            logData.Append("")
            logData.Append(message).Append("[FUNC:").Append(functionName).Append("]")
            If ex Is Nothing Then
                Logger.Error(logData.ToString())
            Else
                Logger.Error(logData.ToString(), ex)
            End If
        ElseIf LOG_TYPE_WARNING.Equals(logLevel) Then
            logData.Append("")
            logData.Append(message).Append("[FUNC:").Append(functionName).Append("]")
            Logger.Warn(logData.ToString())
        End If

    End Sub

End Class
