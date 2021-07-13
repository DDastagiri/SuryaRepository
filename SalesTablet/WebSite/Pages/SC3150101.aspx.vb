'------------------------------------------------------------------------------
'SC3150101.aspx.vb
'------------------------------------------------------------------------------
'機能：メインメニュー（TC）
'補足：
'作成：2012/01/28 KN 渡辺
'更新：2012/03/02 KN上田 【SERVICE_1】課題管理番号-BMTS_0229_YW_02の不具合修正(フッタボタン制御)
'------------------------------------------------------------------------------
Option Strict On
Option Explicit On

Imports System
Imports System.Data
Imports System.Web.Script.Serialization
Imports Toyota.eCRB.SystemFrameworks.Core
Imports Toyota.eCRB.SystemFrameworks.Web
Imports Toyota.eCRB.iCROP.DataAccess.SC3150101
Imports Toyota.eCRB.iCROP.BizLogic.SC3150101
'Imports Toyota.eCRB.iCROP.DataAccess.StallInfo

''' <summary>
''' 
''' </summary>
''' <remarks></remarks>
Partial Class Pages_Default
    Inherits BasePage

#Region "定数"

    ''' <summary>
    ''' 画面ID
    ''' </summary>
    ''' <remarks></remarks>
    Private Const APPLICATION_ID As String = "SC3150101"
    ''' <summary>
    ''' R/Oプレビュー画面ID
    ''' </summary>
    ''' <remarks></remarks>
    Private Const REPAIR_ORDERE_PREVIEW_PAGE As String = "SC3160208"
    ''' <summary>
    ''' 追加作業依頼書プレビュー画面ID
    ''' </summary>
    ''' <remarks></remarks>
    Private Const ADD_REPAIR_PREVIEW_PAGE As String = "SC3170302"
    ''' <summary>
    ''' 部品連絡画面ID
    ''' </summary>
    ''' <remarks></remarks>
    Private Const PARTS_CONTACT_PAGE_ID = "SC3190303"
    ''' <summary>
    ''' 追加作業一覧ページID
    ''' </summary>
    ''' <remarks></remarks>
    Private Const ADDITION_WORK_LIST_ID As String = "SC3170101"
    ''' <summary>
    ''' 追加作業入力ページID
    ''' </summary>
    ''' <remarks></remarks>
    Private Const ADDITION_WORK_PAGE_ID As String = "SC3170203"
    ''' <summary>
    ''' 完成検査一覧画面ID
    ''' </summary>
    ''' <remarks></remarks>
    Private Const COMPLETION_CHECK_PAGE_ID As String = "SC3180101"
    ''' <summary>
    ''' 完成検査チェックシート入力画面ID
    ''' </summary>
    ''' <remarks></remarks>
    Private Const COMPLETION_CHECK_INPUT_PAGE_ID As String = "SC3180204"


    ''' <summary>
    ''' フッターコード：メインメニュー
    ''' </summary>
    ''' <remarks></remarks>
    Private Const FOOTER_MAINMENU As Integer = 100
    ''' <summary>
    ''' フッターコード：カスタマー
    ''' </summary>
    ''' <remarks></remarks>
    Private Const FOOTER_CUSTOMER As Integer = 200
    ''' <summary>
    ''' フッターコード：TCV
    ''' </summary>
    ''' <remarks></remarks>
    Private Const FOOTER_TCV As Integer = 300
    ''' <summary>
    ''' フッターコード：追加作業（サブ）
    ''' </summary>
    ''' <remarks></remarks>
    Private Const FOOTER_ADDITIONAL_WORK As Integer = 1100
    ''' <summary>
    ''' フッターコード：追加作業（サブ）
    ''' </summary>
    ''' <remarks></remarks>
    Private Const FOOTER_SUB_ADDITIONAL_WORK As Integer = 1101
    ''' <summary>
    ''' フッターコード：完成検査
    ''' </summary>
    ''' <remarks></remarks>
    Private Const FOOTER_COMP_EXAM As Integer = 1000
    ''' <summary>
    ''' フッターコード：スケジューラ
    ''' </summary>
    ''' <remarks></remarks>
    Private Const FOOTER_SCHEDULE As Integer = 400
    ''' <summary>
    ''' フッターコード：電話帳
    ''' </summary>
    ''' <remarks></remarks>
    Private Const FOOTER_TEL_DIRECTORY As Integer = 500


    ''' <summary>
    ''' 休憩による作業伸長ポップアップの表示フラグ：表示する
    ''' </summary>
    ''' <remarks></remarks>
    Private Const POPUP_BREAK_DISPLAY = "1"
    ''' <summary>
    ''' 休憩による作業伸長ポップアップの表示フラグ：表示しない
    ''' </summary>
    ''' <remarks></remarks>
    Private Const POPUP_BREAK_NONE = "0"


    ''' <summary>
    ''' 押したフッタボタンの状態：初期状態
    ''' </summary>
    ''' <remarks></remarks>
    Private Const PUSHED_FOOTER_BUTTON_INIT = "0"
    ''' <summary>
    ''' 押したフッタボタンの状態：開始処理
    ''' </summary>
    ''' <remarks></remarks>
    Private Const PUSHED_FOOTER_BUTTON_START_WORK = "1"
    ''' <summary>
    ''' 押したフッタボタンの状態：終了処理
    ''' </summary>
    ''' <remarks></remarks>
    Private Const PUSHED_FOOTER_BUTTON_END_WORK = "2"
    ''' <summary>
    ''' 押したフッタボタンの状態：当日処理
    ''' </summary>
    ''' <remarks></remarks>
    Private Const PUSHED_FOOTER_BUTTON_SUSPEND_WORK = "3"
    ''' <summary>
    ''' 押したフッタボタンの状態：検査開始
    ''' </summary>
    ''' <remarks></remarks>
    Private Const PUSHED_FOOTER_BUTTON_START_CHECK = "4"
    ''' <summary>
    ''' 押したフッタボタンの状態：部品連絡
    ''' </summary>
    ''' <remarks></remarks>
    Private Const PUSHED_FOOTER_BUTTON_CONNECT_PARTS = "5"

    ''' <summary>
    ''' 干渉バリデーション結果：作業チップと干渉するため処理不可
    ''' </summary>
    ''' <remarks></remarks>
    Private Const INTERFERENCE_FAILE As Integer = 1
    ''' <summary>
    ''' 干渉バリデーション結果：休憩をとらなければ、処理可能
    ''' </summary>
    ''' <remarks></remarks>
    Private Const INTERFERENCE_DONOT_BREAK As Integer = 2
    ''' <summary>
    ''' 干渉バリデーション結果：休憩をとっても、とらなくても処理可能
    ''' </summary>
    ''' <remarks></remarks>
    Private Const INTERFERENCE_TAKE_BREAK As Integer = 3
    ''' <summary>
    ''' 干渉バリデーション結果：作業チップとも休憩チップとも干渉なし
    ''' </summary>
    ''' <remarks></remarks>
    Private Const INTERFERENCE_SUCCESSFULL As Integer = 4

    ''' <summary>
    ''' 開始イベントのエラーコード：正常終了
    ''' </summary>
    ''' <remarks></remarks>
    Private Const ERROR_CODE_START_WORK_SUCCESSFULL As Integer = 0

    ''' <summary>
    ''' R/O作業ステータス：受付
    ''' </summary>
    ''' <remarks></remarks>
    Private Const ORDER_STATUS_RECEPTION As String = "1"
    ''' <summary>
    ''' R/O作業ステータス：見積確定待ち
    ''' </summary>
    ''' <remarks></remarks>
    Private Const ORDER_STATUS_WAITING_ESTIMATE As String = "5"
    ''' <summary>
    ''' R/O作業ステータス：部品待ち
    ''' </summary>
    ''' <remarks></remarks>
    Private Const ORDER_STATUS_WAITING_PARTS As String = "4"
    ''' <summary>
    ''' R/O作業ステータス：整備中
    ''' </summary>
    ''' <remarks></remarks>
    Private Const ORDER_STATUS_ON_WORK_ORDER As String = "2"
    ''' <summary>
    ''' R/O作業ステータス：検査完了
    ''' </summary>
    ''' <remarks></remarks>
    Private Const ORDER_STATUS_COMPLETE_INSPECTION As String = "7"
    ''' <summary>
    ''' R/O作業ステータス：整備完了
    ''' </summary>
    ''' <remarks></remarks>
    Private Const ORDER_STATUS_COMPLETE_WORK As String = "6"
    ''' <summary>
    ''' R/O作業ステータス：売上済み
    ''' </summary>
    ''' <remarks></remarks>
    Private Const ORDER_STATUS_SALES As String = "3"
    ''' <summary>
    ''' R/O作業ステータス：納車完了
    ''' </summary>
    ''' <remarks></remarks>
    Private Const ORDER_STATUS_COMPLETE_DELIVERY = "8"

    ''' <summary>
    ''' DateTimeFuncにて、"yyyy/MM/dd HH:mm"形式をコンバートするためのID
    ''' </summary>
    ''' <remarks></remarks>
    Private Const DATE_CONVERT_ID_YYYYMMDDHHMM As Integer = 2
    ''' <summary>
    ''' DateTimeFuncにて、"yyyy/MM/dd"形式をコンバートするためのID
    ''' </summary>
    ''' <remarks></remarks>
    Private Const DATE_CONVERT_ID_YYYY_MM_DD As Integer = 21

    ''' <summary>
    ''' 実績ステータス：待機中
    ''' </summary>
    ''' <remarks></remarks>
    Private Const RESULT_STATUS_WAIT As String = "1"
    ''' <summary>
    ''' 実績ステータス：作業中
    ''' </summary>
    ''' <remarks></remarks>
    Private Const RESULT_STATUS_WORKING As String = "2"
    ''' <summary>
    ''' 実績ステータス：作業完了
    ''' </summary>
    ''' <remarks></remarks>
    Private Const RESULT_STATUS_COMP As String = "3"

    ''' <summary>
    ''' ステータス：ストール本予約
    ''' </summary>
    ''' <remarks></remarks>
    Private Const STATUS_RESERVE As Integer = 1
    ''' <summary>
    ''' ステータス：ストール仮予約
    ''' </summary>
    ''' <remarks></remarks>
    Private Const STATUS_TEMP_RESERVE As Integer = 2
    ''' <summary>
    ''' ステータス：引取納車
    ''' </summary>
    ''' <remarks></remarks>
    Private Const STATUS_DELIVALY As Integer = 4
    ''' <summary>
    ''' ステータス：使用不可
    ''' </summary>
    ''' <remarks></remarks>
    Private Const STATUS_UNAVAILABLE As Integer = 3
    ''' <summary>
    ''' ステータス：休憩
    ''' </summary>
    ''' <remarks></remarks>
    Private Const STATUS_REST As Integer = 99

    ' ''' <summary>
    ' ''' ポストバックをしたことを示すフラグ
    ' ''' </summary>
    ' ''' <remarks></remarks>
    'Private Const POSTBACK_TRUE As String = "1"

    ''' <summary>
    ''' チップ選択がなされてない状態を示す
    ''' </summary>
    ''' <remarks></remarks>
    Private Const SELECTED_CHIP_OFF As String = "0"
    ''' <summary>
    ''' チップ選択がなされている状態を示す
    ''' </summary>
    ''' <remarks></remarks>
    Private Const SELECTED_CHIP_ON As String = "1"

#End Region

#Region "メンバ変数"

    ''' <summary>
    ''' ユーザ情報（セッションより）
    ''' </summary>
    ''' <remarks></remarks>
    Private objStaffContext As StaffContext

    ''' <summary>
    ''' ビジネスロジック
    ''' </summary>
    ''' <remarks></remarks>
    Private businessLogic As New SC3150101BusinessLogic

    ''' <summary>
    ''' ログイン中のストールID
    ''' </summary>
    ''' <remarks></remarks>
    Private stallId As Integer
    ''' <summary>
    ''' ストールの稼動開始時間
    ''' </summary>
    ''' <remarks></remarks>
    Private stallActualStartTime As Date
    ''' <summary>
    ''' ストールの稼動終了時間
    ''' </summary>
    ''' <remarks></remarks>
    Private stallActualEndTime As Date


#End Region

#Region "初期処理"
    ''' <summary>
    ''' ページロード時の処理
    ''' </summary>
    ''' <param name="sender">イベント発生元</param>
    ''' <param name="e">イベントデータ</param>
    ''' <remarks></remarks>
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Logger.Info("Page_Load Start")

        'SESSION情報にSC3150102より先にアクセスすることで、「戻る」実施時の戻り先をSC3150101に設定する.
        MyBase.ContainsKey(ScreenPos.Current, "Redirect.ORDERNO")

        'ユーザ情報の取得.
        objStaffContext = StaffContext.Current
        'フッタボタンの初期化を行う.
        InitFooterButton()
        'ログインアカウントよりストール情報を取得する.
        SetStallInfo()
        'サーバ時間を取得し、設定する.
        SetServerCurrentTime()
        'ポストバック時の処理の継続チェックをする.


        '初回呼び出し時の処理を実施する.
        If (Not Page.IsPostBack) Then

            Logger.Info("Page_Load Not IsPostBack")

            '初回表示時にHiddenにデータを格納する.
            PageLoadInit()

            'サーバよりチップ情報を取得する.
            GetChipDataFromServer()

        End If

        Logger.Info("Page_Load End")

    End Sub


    ''' <summary>
    ''' 初回ページ読込時の処理
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub PageLoadInit()

        Logger.Info("PageLoadInit Start")

        'Javascript用
        '作業進捗の文言の取得.
        Me.HiddenStartTimeWord.Value = WebWordUtility.GetWord(7)
        Me.HiddenEndTimeWord.Value = WebWordUtility.GetWord(8)
        Me.HiddenResultStartTimeWord.Value = WebWordUtility.GetWord(25)
        Me.HiddenResultEndTimeWord.Value = WebWordUtility.GetWord(26)
        'チップの休憩・使用不可に表示する文字列.
        Me.HiddenRestText.Value = WebWordUtility.GetWord(11)
        Me.HiddenUnavailableText.Value = WebWordUtility.GetWord(20)
        '日跨ぎエラー文字列.
        Me.HiddenWarnNextDate.Value = WebWordUtility.GetWord(904)
        '部品連絡ポップアップに使用する文言.
        Me.HiddenPopupPartsCancelWord.Value = WebWordUtility.GetWord(18)
        Me.HiddenPopupPartsTitleWord.Value = WebWordUtility.GetWord(17)

        Logger.Info("PageLoadInit End")

    End Sub


    ''' <summary>
    ''' ログインアカウントよりストール情報を取得し、テキストに格納する.
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub SetStallInfo()
        Logger.Info("SetStallInfo START")

        'ストール情報の取得.
        Dim stallDataTable As SC3150101DataSet.SC3150101BelongStallInfoDataTable
        stallDataTable = businessLogic.GetBelongStallData()
        'ストール時間に初期値を格納する.
        Me.stallActualStartTime = DateTimeFunc.Now(objStaffContext.DlrCD).Date
        Me.stallActualEndTime = Me.stallActualStartTime.AddDays(1)

        'ストール情報を設定.
        Dim strStallName As String = ""
        For Each eachStallData As DataRow In stallDataTable

            Me.stallId = CType(eachStallData("STALLID"), Integer)
            Logger.Info("SetStallInfo StallInfo Roop_StallID:" + CType(Me.stallId, String))

            strStallName = CType(eachStallData("STALLNAME"), String)
            Me.stallActualStartTime = ExchangeStallHourToDate(CType(eachStallData("PSTARTTIME"), String))
            Me.stallActualEndTime = ExchangeStallHourToDate(CType(eachStallData("PENDTIME"), String))
            'ストール時間が、開始時間より終了時間が小さくなってしまう場合、終了時間に1日加算する.
            If (Me.stallActualEndTime < Me.stallActualStartTime) Then
                Logger.Info("SetStallInfo StallInfo Roop If stallActualEndTime < stallActualStartTime")
                Me.stallActualEndTime = Me.stallActualEndTime.AddDays(1)
            End If

        Next

        '取得したストール情報より、エンジニア名を取得する.
        Dim stallStaffDataTable As SC3150101DataSet.SC3150101BelongStallStaffDataTable
        stallStaffDataTable = businessLogic.GetBelongStallStaffData(Me.stallId)

        'エンジニア名を設定.
        Dim strEngineerNameText As New System.Text.StringBuilder
        For Each eachStaffName As DataRow In stallStaffDataTable

            Dim staffName As String
            staffName = CType(eachStaffName("USERNAME"), String)
            Logger.Info("SetStallInfo Roop_Engineer EngineerName:" + staffName)

            'エンジニア名が既に格納されている場合、エンジニア名の分割文字を追加する.
            If (0 < strEngineerNameText.Length) Then
                Logger.Info("SetStallInfo Roop_Engineer If Engineers")
                strEngineerNameText.Append(WebWordUtility.GetWord(3))
            End If

            strEngineerNameText.Append(staffName)
        Next

        '取得したストール情報を表示する.
        LabelStallName.Text = strStallName
        LabelEngineerName.Text = strEngineerNameText.ToString()
        HiddenStallStartTime.Value = DateTimeFunc.FormatDate(2, Me.stallActualStartTime)
        HiddenStallEndTime.Value = DateTimeFunc.FormatDate(2, Me.stallActualEndTime)

        Logger.Info("SetStallInfo END")
    End Sub


    ''' <summary>
    ''' ストール時間を取得し、Date型に変換する
    ''' </summary>
    ''' <param name="stallHour">5桁の（HH:mm）形式の時間</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function ExchangeStallHourToDate(ByVal stallHour As String) As Date

        Logger.Info("ExchangeStallHourToDate Start param1:" + stallHour)

        '返す値の初期値として、当日の0時を設定する.
        Dim stallDate As Date = DateTimeFunc.Now(objStaffContext.DlrCD).Date

        Dim stallDateString As New System.Text.StringBuilder

        '当日日付を追加
        stallDateString.Append(DateTimeFunc.FormatDate(DATE_CONVERT_ID_YYYY_MM_DD, stallDate))
        stallDateString.Append(" ")
        stallDateString.Append(stallHour.Substring(0, 5))

        '生成した文字列を使用して、日付型データを取得する.
        stallDate = DateTimeFunc.FormatString("yyyy/MM/dd HH:mm", stallDateString.ToString())

        Logger.Info("ExchangeStallHourToDate End return" + DateTimeFunc.FormatDate(DATE_CONVERT_ID_YYYYMMDDHHMM, stallDate))
        Return stallDate

    End Function


    ''' <summary>
    ''' 現在のサーバ時間をHiddenFieldにセットする.
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub SetServerCurrentTime()

        Logger.Info("SetServerCurrentTime Start")

        'サーバ時間を文字列として取得して、HiddenFieldに格納.（yyyy/MM/dd HH:mm:ss形式）
        Me.HiddenServerTime.Value = DateTimeFunc.FormatDate(1, DateTimeFunc.Now(objStaffContext.DlrCD))

        Logger.Info("SetServerCurrentTime End SetTime:" + Me.HiddenServerTime.Value)

    End Sub

#End Region

#Region "チップ情報の取得処理"
    ''' <summary>
    ''' チップ情報を取得し、JSON形式の文字列データを格納する処理.
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub GetChipDataFromServer()

        Logger.Info("GetChipDataFromServer Start")

        'チップ情報の最新を取得し、作業対象チップを設定する
        Dim dtChipInfo As SC3150101DataSet.SC3150101ChipInfoDataTable
        dtChipInfo = GetNewestChipInfo()
        GetCandidateChipInfo(dtChipInfo)

        '受信したデータをJSON形式に変換する
        Dim chipDataJson As String
        'chipDataJson = businessLogic.dataTableToJson(dtChipInfo)
        chipDataJson = businessLogic.DataTableToJson(dtChipInfo)
        Logger.Debug("GetChipDataFromServer ChipData:" + chipDataJson)

        '取得したJSON形式のデータをHiddenに格納する
        Me.HiddenJsonData.Value = chipDataJson

        'End If

        Logger.Info("GetChipDataFromServer End")

    End Sub

    ''' <summary>
    ''' データベースより最新のチップ情報を取得する
    ''' </summary>
    ''' <returns>StallInfoDataSet.CHIPINFODataTable:差分チップデータ</returns>
    ''' <remarks></remarks>
    Private Function GetNewestChipInfo() As SC3150101DataSet.SC3150101ChipInfoDataTable

        Logger.Info("GetNewestChipInfo Start")

        '休憩チップ情報を取得し、先のチップ情報に追加する.
        Dim dtBreakChipData As SC3150101DataSet.SC3150101ChipInfoDataTable
        dtBreakChipData = businessLogic.GetBreakData(Me.stallId)
        Logger.Info("GetNewestChipInfo BreakChipDataCount:" + CType(dtBreakChipData.Count, String))
        'For Each drBreakItem As SC3150101DataSet.SC3150101ChipInfoRow In dtBreakChipData
        '    dtNewestChipInfo.Rows.Add(drBreakItem)
        'Next

        '使用不可チップ情報を取得し、先のチップ情報に追加する.
        Dim dtUnavailableChipData As SC3150101DataSet.SC3150101ChipInfoDataTable
        dtUnavailableChipData = businessLogic.GetUnavailableData(Me.stallId, Me.stallActualStartTime, Me.stallActualEndTime)
        Logger.Info("GetNewestChipInfo UnavailableChipDataCount:" + CType(dtUnavailableChipData.Count, String))
        'For Each drUnavailableItem As SC3150101DataSet.SC3150101ChipInfoRow In dtUnavailableChipData
        '    dtNewestChipInfo.Rows.Add(drUnavailableItem)
        'Next
        dtBreakChipData.Merge(dtUnavailableChipData, False)

        '最新のチップ情報を取得する.
        Dim dtNewestChipInfo As SC3150101DataSet.SC3150101ChipInfoDataTable
        dtNewestChipInfo = businessLogic.GetStallChipInfo(Me.stallId, Me.stallActualStartTime, Me.stallActualEndTime)
        Logger.Info("GetNewestChipInfo NewestChipDataCount:" + CType(dtNewestChipInfo.Count, String))
        dtBreakChipData.Merge(dtNewestChipInfo, False)

        Logger.Info("GetNewestChipInfo End Return RowsCount:" + CType(dtBreakChipData.Count, String))
        Return dtBreakChipData

    End Function

    ''' <summary>
    ''' 作業対象チップ情報を特定し、作業対象チップ情報行を返す.
    ''' </summary>
    ''' <param name="dtData">作業チップデータセット</param>
    ''' <returns>作業対象チップ情報行</returns>
    ''' <remarks></remarks>
    Private Function GetCandidateChipInfo(dtData As SC3150101DataSet.SC3150101ChipInfoDataTable) As SC3150101DataSet.SC3150101ChipInfoRow

        Logger.Info("GetCandidateChipInfo Start")

        Dim drCandidateWorkInfo As SC3150101DataSet.SC3150101ChipInfoRow = Nothing
        Dim dtmOldestStartTime As DateTime = DateTime.MaxValue

        For Each drChipInfo As SC3150101DataSet.SC3150101ChipInfoRow In dtData.Rows
            Logger.Info("GetCandidateChipInfo Roop REZID:" + CType(drChipInfo.REZID, String))

            'チップ情報が、本予約・仮予約・引取納車のいずれかの場合、判定処理を実施する.
            If ((drChipInfo.STATUS = STATUS_RESERVE) Or (drChipInfo.STATUS = STATUS_TEMP_RESERVE) Or (drChipInfo.STATUS = STATUS_DELIVALY)) Then
                Logger.Info("GetCandidateChipInfo If STATUS=1or2or4 STATUS:" + CType(drChipInfo.STATUS, String))

                Dim dtmChipStartTime As DateTime
                'dtmChipStartTime = Date.ParseExact(drChipInfo("STARTTIME").ToString(), "yyyy/MM/dd H:mm:ss", Nothing)
                dtmChipStartTime = CType(drChipInfo("STARTTIME"), Date)

                If (IsDBNull(drChipInfo("RESULT_STATUS"))) Then
                    Logger.Info("GetCandidateChipInfo if ResultStatus is DBNull")

                    '実績ステータスがないものは、実績ステータス=待機中として扱う
                    '該当のレコードの開始時間（予定）を取得し、現在所持している時間と比較して小さい場合、更新する
                    If (dtmChipStartTime < dtmOldestStartTime) Then
                        Logger.Info("GetCandidateChipInfo ResultStats is DBNull And StartTime Smaller")

                        dtmOldestStartTime = dtmChipStartTime
                        drCandidateWorkInfo = drChipInfo
                    End If
                ElseIf RESULT_STATUS_WAIT.Equals(drChipInfo("RESULT_STATUS")) Then
                    Logger.Info("GetCandidateChipInfo if ResultStatus is waiting")

                    'チップの実績ステータスが待機中である場合、
                    '該当のレコードの開始時間（予定）を取得し、現在所持している時間と比較して小さい場合、更新する
                    If (dtmChipStartTime < dtmOldestStartTime) Then
                        Logger.Info("GetCandidateChipInfo ResultStatus is waiting And StartTime Smaller")

                        dtmOldestStartTime = dtmChipStartTime
                        drCandidateWorkInfo = drChipInfo
                    End If
                ElseIf RESULT_STATUS_WORKING.Equals(drChipInfo("RESULT_STATUS")) Then
                    Logger.Info("GetCnadidateChipInfo if ResultStatus is working")

                    'チップの実績ステータスが作業中である場合、該当のレコードを作業対象に設定し、ループを抜ける
                    drCandidateWorkInfo = drChipInfo
                    Exit For
                End If
            End If
        Next

        '取得したチップ情報をページの作業対象チップ情報に加える
        If Not IsNothing(drCandidateWorkInfo) Then
            Dim strCandidateId As New System.Text.StringBuilder

            strCandidateId.Append(drCandidateWorkInfo("REZID").ToString())
            strCandidateId.Append("_")
            strCandidateId.Append(drCandidateWorkInfo("SEQNO").ToString())
            strCandidateId.Append("_")
            strCandidateId.Append(drCandidateWorkInfo("DSEQNO").ToString())

            Me.HiddenCandidateId.Value = strCandidateId.ToString()
            Logger.Info("GetCnadidateChipInfo if Not IsNothing chipInfo CandidateId:" + strCandidateId.ToString())
        End If

        Logger.Info("GetCandidateChipInfo End")
        Return drCandidateWorkInfo

    End Function

#End Region

#Region "フッター制御"

    ''' <summary>
    ''' フッター制御
    ''' </summary>
    ''' <param name="commonMaster">マスターページ</param>
    ''' <param name="category">カテゴリ</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Overrides Function DeclareCommonMasterFooter( _
        ByVal commonMaster As Toyota.eCRB.SystemFrameworks.Web.CommonMasterPage, _
        ByRef category As Toyota.eCRB.SystemFrameworks.Web.FooterMenuCategory) As Integer()

        Logger.Info("Override DeclareCommonMasterFooter Start")

        '自ページの所属メニューを宣言
        category = FooterMenuCategory.MainMenu

        Logger.Info("Override DeclareCommonMasterFooter End")
        '表示非表示に関わらず、使用するサブメニューボタンを宣言
        Return New Integer() {}

    End Function

    ''' <summary>
    ''' フッターボタンの初期化
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub InitFooterButton()

        Logger.Info("InitFooterButton Start")

        'メインメニューボタンの設定
        Dim mainMenuButton As CommonMasterFooterButton = CType(Me.Master, CommonMasterPage).GetFooterButton(FOOTER_MAINMENU)
        AddHandler mainMenuButton.Click, AddressOf MainMenuButton_Click

        '作業追加ボタンの設定
        Dim addWorkButton As CommonMasterFooterButton = CType(Me.Master, CommonMasterPage).GetFooterButton(FOOTER_ADDITIONAL_WORK)
        AddHandler addWorkButton.Click, AddressOf AddWorkButton_Click

        '完成検査ボタンの設定
        Dim compExamButton As CommonMasterFooterButton = CType(Me.Master, CommonMasterPage).GetFooterButton(FOOTER_COMP_EXAM)
        AddHandler compExamButton.Click, AddressOf CompletionCheckButton_Click

        'スケジュールボタンのイベント設定
        Dim scheduleButton As CommonMasterFooterButton = CType(Me.Master, CommonMasterPage).GetFooterButton(FOOTER_SCHEDULE)
        scheduleButton.OnClientClick = "return schedule.appExecute.executeCaleNew();"

        '電話帳ボタンのイベント設定
        Dim telDirectoryButton As CommonMasterFooterButton = CType(Me.Master, CommonMasterPage).GetFooterButton(FOOTER_TEL_DIRECTORY)
        telDirectoryButton.OnClientClick = "return schedule.appExecute.executeCont();"

        '2012/03/02 上田 フッタボタン制御 Start
        mainMenuButton.OnClientClick = "return FooterButtonClick('mainMenuButton');"
        addWorkButton.OnClientClick = "return FooterButtonClick('addWorkButton');"
        compExamButton.OnClientClick = "return FooterButtonClick('compExamButton');"
        '2012/03/02 上田 フッタボタン制御 End

        Logger.Info("InitFooterButton End")

    End Sub
#End Region

#Region "フッターサブメニュー処理"

    ''' <summary>
    ''' メインメニューボタンを押した時の処理
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub MainMenuButton_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Logger.Info("MainMenuButton_Click Start")

        '最新のチップ情報を取得する.
        GetChipDataFromServer()

        Logger.Info("MainMenuButton_Click End")
    End Sub

    ''' <summary>
    ''' 追加作業ボタンを押した時の処理
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub AddWorkButton_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Logger.Info("AddWorkButton_Click Start")

        ''チップ情報の最新を取得し、作業対象チップを取得する.
        'Dim dtChipInfo As SC3150101DataSet.SC3150101ChipInfoDataTable
        'dtChipInfo = GetNewestChipInfo()
        'Dim drCandidateChipInfo As SC3150101DataSet.SC3150101ChipInfoRow
        'drCandidateChipInfo = GetCandidateChipInfo(dtChipInfo)

        ''作業対象チップのチップIDを取得する.
        'Dim chipId As String
        'chipId = CreateChipId(drCandidateChipInfo)

        '選択中のチップ情報を取得する.
        Dim selectedChipInfo As SC3150101DataSet.SC3150101ChipInfoRow
        selectedChipInfo = GetSelectedChipInfo()

        '遷移先画面IDの初期値に、追加作業一覧ページのIDを指定する.
        Dim nextScreenId As String
        nextScreenId = ADDITION_WORK_LIST_ID

        Logger.Info("AddWorkButton_Click SelectedStatus=" + Me.HiddenSelectedChip.Value)

        'いずれかの作業チップを選択している場合
        If (Me.HiddenSelectedChip.Value = SELECTED_CHIP_ON) Then
            Logger.Info("AddWorkButton_Click HiddenSelectedId=" + Me.HiddenSelectedId.Value)

            '選択中のチップが作業対象チップである場合
            'If (Me.HiddenSelectedId.Value = chipId) Then
            If (Not IsNothing(selectedChipInfo)) Then
                'R/O作業ステータスを取得する.
                Dim repairOrderStatus As String
                repairOrderStatus = CType(Me.HiddenOrderStatus.Value, String)

                Logger.Info("AddWorkButton_Click repairOrderStatus=" + repairOrderStatus)

                'R/O作業ステータスが、部品待ち・整備中・検査完了のいずれかに属する場合
                If (ORDER_STATUS_WAITING_PARTS.Equals(repairOrderStatus) Or ORDER_STATUS_ON_WORK_ORDER.Equals(repairOrderStatus) Or _
                    ORDER_STATUS_COMPLETE_INSPECTION.Equals(repairOrderStatus)) Then

                    Dim orderNumber As String = Me.HiddenFieldOrderNo.Value
                    Dim childNumber As String = Me.HiddenFieldChildNo.Value
                    Dim editValue As String = "0"

                    Logger.Info("AddWorkButton_Click Param:Redirect.ORDERNO=" + orderNumber)
                    Logger.Info("AddWorkButton_Click Param:Redirect.SRVADDSEQ=" + childNumber)
                    Logger.Info("AddWorkButton_Click Param:Redirect.EDITFLG=" + editValue)

                    '追加作業入力ページに渡す引数をセッションに格納.
                    MyBase.SetValue(ScreenPos.Next, "Redirect.ORDERNO", orderNumber)
                    MyBase.SetValue(ScreenPos.Next, "Redirect.SRVADDSEQ", childNumber)
                    MyBase.SetValue(ScreenPos.Next, "Redirect.EDITFLG", editValue)

                    '遷移画面IDを追加作業入力画面に設定する.
                    nextScreenId = ADDITION_WORK_PAGE_ID
                End If
            End If
        End If

        Logger.Info("AddWorkButton_Click End RedirectNextScreen:ID=" + nextScreenId)
        '選択されたチップのオーダー番号が指定されている場合、追加作業入力画面へ遷移.
        Me.RedirectNextScreen(nextScreenId)

    End Sub


    ''' <summary>
    ''' 完成検査ボタンを押した時の処理.
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub CompletionCheckButton_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Logger.Info("CompletionCheckButton_Click Start")

        ''チップ情報の最新を取得し、作業対象チップを取得する.
        'Dim dtChipInfo As SC3150101DataSet.SC3150101ChipInfoDataTable
        'dtChipInfo = GetNewestChipInfo()
        'Dim drCandidateChipInfo As SC3150101DataSet.SC3150101ChipInfoRow
        'drCandidateChipInfo = GetCandidateChipInfo(dtChipInfo)

        ''作業対象チップのチップIDを取得する.
        'Dim chipId As String
        'chipId = CreateChipId(drCandidateChipInfo)

        '選択中のチップ情報を取得する.
        Dim selectedChipInfo As SC3150101DataSet.SC3150101ChipInfoRow
        selectedChipInfo = GetSelectedChipInfo()

        '遷移先画面IDの初期値として、完成検査一覧ページを指定
        Dim nextScreenId As String
        nextScreenId = COMPLETION_CHECK_PAGE_ID

        Logger.Info("CompletionCheckButton_Click SelectedChip=" + Me.HiddenSelectedChip.Value)
        '作業対象チップ情報を取得している場合のみ、判定処理を実施する.
        'If (Not IsNothing(drCandidateChipInfo)) Then
        '作業チップを選択している場合、
        If (Me.HiddenSelectedChip.Value = SELECTED_CHIP_ON) Then
            '選択中のチップが作業対象チップである場合、
            'If (Me.HiddenSelectedId.Value = chipId) Then
            If (Not IsNothing(selectedChipInfo)) Then
                Logger.Info("CompletionCheckButton_Click Not IsNothing selectedChipInfo")
                Logger.Info("CompletionCheckButton_Click RESULT_STATUS=" + selectedChipInfo.RESULT_STATUS)
                '選択中チップが作業中である場合、
                'If (drCandidateChipInfo.RESULT_STATUS.Equals(RESULT_STATUS_WORKING)) Then
                If (selectedChipInfo.RESULT_STATUS.Equals(RESULT_STATUS_WORKING)) Then

                    Dim orderNumber As String = Me.HiddenFieldOrderNo.Value
                    Dim childNumber As String = Me.HiddenFieldChildNo.Value

                    Logger.Info("CompletionCheckButton_Click Param:Redirect.ORDERNO=" + orderNumber)
                    Logger.Info("CompletionCheckButton_Click Param:Redirect.SRVADDSEQ=" + childNumber)

                    '完成検査チェックシート入力ページに渡す引数をセッションに格納
                    MyBase.SetValue(ScreenPos.Next, "Redirect.ORDERNO", orderNumber)
                    MyBase.SetValue(ScreenPos.Next, "Redirect.SRVADDSEQ", childNumber)

                    nextScreenId = COMPLETION_CHECK_INPUT_PAGE_ID
                End If
            End If
        End If
        'End If

        Logger.Info("CompletionCheckButton_Click End RedirectNextScreen:ID=" + nextScreenId)
        ' 完成検査へ遷移
        Me.RedirectNextScreen(nextScreenId)

    End Sub


    ''' <summary>
    ''' 選択中のチップ情報を取得する.
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function GetSelectedChipInfo() As SC3150101DataSet.SC3150101ChipInfoRow
        Logger.Info("GetSelectedChipInfo Start")

        '返却する選択されているチップ情報を初期化する.
        Dim selectedChipInfo As SC3150101DataSet.SC3150101ChipInfoRow
        selectedChipInfo = Nothing

        '予約・実績チップデータセットを取得する.
        Dim dtChipInfo As SC3150101DataSet.SC3150101ChipInfoDataTable
        dtChipInfo = businessLogic.GetStallChipInfo(Me.stallId, Me.stallActualStartTime, Me.stallActualEndTime)

        '取得した予約・実績チップのデータセットをループ処理する.
        For Each eachData As SC3150101DataSet.SC3150101ChipInfoRow In dtChipInfo.Rows

            'チップのチップIDを取得する.
            Dim chipId As String
            chipId = CreateChipId(eachData)
            Logger.Info("GetSelectedChipInfo roop chipId=" + chipId)

            '選択中のチップと合致する場合、返却値に現在選択中のチップ情報を格納しループ処理を抜ける.
            If (Me.HiddenSelectedId.Value.Equals(chipId)) Then
                Logger.Info("GetSelectedChipInfo chipId equals selectedId")

                selectedChipInfo = eachData
                Exit For
            End If

        Next

        Logger.Info("GetSelectedChipInfo End")
        Return selectedChipInfo

    End Function


    ''' <summary>
    ''' 選択されているチップを特定するための、チップIDを作成する.
    ''' </summary>
    ''' <param name="drChipInfo">作業対象チップ情報</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function CreateChipId(ByVal drChipInfo As SC3150101DataSet.SC3150101ChipInfoRow) As String
        Logger.Info("CreateChipId Start")

        'チップIDを生成する.
        Dim chipIdStringBuilder As New System.Text.StringBuilder

        '取得した作業対象チップ情報がNothingでない場合、値を取得する.
        If Not IsNothing(drChipInfo) Then
            Logger.Info("CreateChipId if Not IsNothing ParamChipInfo")

            chipIdStringBuilder.Append(drChipInfo.REZID)
            chipIdStringBuilder.Append("_")
            chipIdStringBuilder.Append(drChipInfo.SEQNO)
            chipIdStringBuilder.Append("_")
            chipIdStringBuilder.Append(drChipInfo.DSEQNO)
        End If

        Dim chipId As String
        chipId = chipIdStringBuilder.ToString()

        Logger.Info("CreateChipId End Return=" + chipId)
        Return chipId

    End Function

#End Region

#Region "画面固有フッターボタン処理"

    ' ''' <summary>
    ' ''' 部品連絡ボタン処理
    ' ''' </summary>
    ' ''' <param name="sender"></param>
    ' ''' <param name="e"></param>
    ' ''' <remarks></remarks>
    'Protected Sub ButtonConnectParts_Click(sender As Object, e As System.EventArgs) _
    '    Handles ButtonConnectParts.Click

    '    Logger.Info("ButtonConnectParts_Click Start")

    '    '押したフッタボタンの状態を、「部品連絡」に設定する.
    '    HiddenPushedFooter.Value = PUSHED_FOOTER_BUTTON_CONNECT_PARTS

    '    '部品連絡のポップアップをコールする.
    '    'ここにコールする関数を記載すればOK.
    '    Me.RedirectNextScreen(PARTS_CONTACT_PAGE_ID)
    '    '休憩ポップアップ表示のため
    '    'HiddenBreakPopup.Value = POPUP_BREAK_DISPLAY

    '    Logger.Info("ButtonConnectParts_Click End")

    'End Sub


    ''' <summary>
    ''' 「休憩をとらない」選択時処理
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub ButtonDoNotBreak_Click(sender As Object, e As System.EventArgs) _
        Handles ButtonDoNotBreak.Click

        Logger.Info("ButtonDoNotBreak_Click Start")

        SelectedTakeBreak(False)

        Logger.Info("ButtonDoNotBreak_Click End")

    End Sub

    ''' <summary>
    ''' 「休憩をとる」選択時処理
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub ButtonTakeBreak_Click(sender As Object, e As System.EventArgs) _
        Handles ButtonTakeBreak.Click

        Logger.Info("ButtonTakeBreak_Click Start")

        SelectedTakeBreak(True)

        Logger.Info("ButtonTakeBreak_Click End")

    End Sub


    ''' <summary>
    ''' 「休憩をとる」「休憩をとらない」の選択時処理
    ''' </summary>
    ''' <param name="selectedBreak"></param>
    ''' <remarks></remarks>
    Private Sub SelectedTakeBreak(ByVal selectedBreak As Boolean)

        Logger.Info("SelectedTakeBreak Start")

        '押したフッタボタンの状態を取得する.
        Dim pushedFooterStatus = Me.HiddenPushedFooter.Value
        Logger.Info("SelectedTakeBreak pushedFooterStatus=" + pushedFooterStatus)

        '表示されている、休憩による作業伸長ポップアップの非表示フラグをセットする
        Me.HiddenBreakPopup.Value = POPUP_BREAK_NONE
        'フッタボタンの状態を初期化する.
        Me.HiddenPushedFooter.Value = PUSHED_FOOTER_BUTTON_INIT

        'フッタボタンの状態に応じて、処理を分岐する.
        If (PUSHED_FOOTER_BUTTON_START_WORK.Equals(pushedFooterStatus)) Then
            Logger.Info("SelectedTakeBreak pushedFooterStatus is Start_Work button Param:" + selectedBreak.ToString())
            StartWorkProcess(selectedBreak)

        ElseIf (PUSHED_FOOTER_BUTTON_SUSPEND_WORK.Equals(pushedFooterStatus)) Then
            Logger.Info("SelectedTakeBreak pushedFooterStatus is Suspend_Work button Param:" + selectedBreak.ToString())
            SuspendWorkProcess(selectedBreak)

        End If

        Logger.Info("SelectedTakeBreak End")

    End Sub


    ''' <summary>
    ''' 作業開始ボタン処理
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub ButtonStartWork_Click(sender As Object, e As System.EventArgs) _
        Handles ButtonStartWork.Click

        Logger.Info("ButtonStartWork_Click Start")

        '2012/03/02 上田 フッタボタン制御 Start
        Try
            '押したフッタボタンの状態を、「作業開始」に設定する.
            HiddenPushedFooter.Value = PUSHED_FOOTER_BUTTON_START_WORK

            '選択されているチップの予約IDを取得する.
            Dim selectedRezId As String
            selectedRezId = Me.HiddenSelectedReserveId.Value
            Logger.Info("ButtonStartWork_Click selectedRezId=" + selectedRezId)

            '取得した予約IDがNull値でない場合のみ
            If (Not String.IsNullOrEmpty(selectedRezId)) Then
                Logger.Info("ButtonStartWork_Click Not IsNullOrEmpty selectedRezId")

                Dim resultInterference As Integer = ValidationInterferenceBreakUnavailable()

                '休憩・使用不可に干渉する場合、作業伸長ポップアップの表示フラグをセットする.
                If (resultInterference = INTERFERENCE_FAILE) Then
                    Logger.Info("ButtonStartWork_Click resultInterference is faile")
                    Logger.Info("ButtonStartWork_Click HiddenBreakPopup=" + POPUP_BREAK_DISPLAY)
                    HiddenBreakPopup.Value = POPUP_BREAK_DISPLAY

                ElseIf (ValidationInterferenceBreakUnavailable() = INTERFERENCE_SUCCESSFULL) Then
                    Logger.Info("ButtonStartWork_Click ValidationInterferenceBreakUnavailable is successfull")
                    '登録するためのキー情報（IDと休憩を挟むフラグ）を引数として、開始登録処理関数を呼び出す.
                    StartWorkProcess(False)

                End If

            End If
        Finally
            Me.HiddenReloadFlag.Value = String.Empty
        End Try
        '2012/03/02 上田 フッタボタン制御 End

        Logger.Info("ButtonStartWork_Click End")

    End Sub


    ''' <summary>
    ''' 開始処理
    ''' </summary>
    ''' <param name="breakExtention"></param>
    ''' <remarks></remarks>
    Private Sub StartWorkProcess(ByVal breakExtention As Boolean)

        Logger.Info("StartWorkProcess Start")

        '選択されているチップの予約IDを取得する.
        Dim selectedRezId As String
        selectedRezId = Me.HiddenSelectedReserveId.Value
        Logger.Info("StartWorkProcess selectedRezId=" + selectedRezId)

        '2012/03/01 KN 西田【SERVICE_1】START
        Dim orderNo As String = Me.HiddenFieldOrderNo.Value
        Logger.Info("StartWorkProcess orderNo=" + orderNo)

        '干渉チェックをせずに開始イベントを実施する.
        Dim resultEvent As Integer
        resultEvent = businessLogic.StartWork(objStaffContext.DlrCD, objStaffContext.BrnCD, _
                                    CType(selectedRezId, Integer), Me.stallId, objStaffContext.Account, orderNo, breakExtention)
        '2012/03/01 KN 西田【SERVICE_1】START

        '正常に終了していない場合、干渉エラーのエラーメッセージを表示する.
        If (resultEvent <> ERROR_CODE_START_WORK_SUCCESSFULL) Then
            Logger.Info("StartWorkProcess The selected Chip is overlapping with next  arranged Chip")
            MyBase.ShowMessageBox(resultEvent, "The selected Chip is overlapping with next  arranged Chip")
            Exit Sub
        End If

        '再描画のため、チップ情報の最新を取得し、作業対象チップ情報を格納する.
        GetChipDataFromServer()
        '開始処理を実施すると、シーケンス番号が更新されるため、チップのIDが変更される.
        'チップの選択状態を保持するため、選択中チップのIDを変更する.
        Me.HiddenSelectedId.Value = Me.HiddenCandidateId.Value

        Logger.Info("StartWorkProcess End")

    End Sub



    ''' <summary>
    ''' 当日処理ボタン処理
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub ButtonSuspendWork_Click(sender As Object, e As System.EventArgs) Handles ButtonSuspendWork.Click

        Logger.Info("ButtonSuspendWork_Click Start")

        '2012/03/02 上田 フッタボタン制御 Start
        Try
            '押したフッタボタンの状態を、「当日処理」に設定する.
            HiddenPushedFooter.Value = PUSHED_FOOTER_BUTTON_SUSPEND_WORK
            Logger.Info("ButtonSuspendWork_Click HiddenPushedFooter=" + PUSHED_FOOTER_BUTTON_SUSPEND_WORK)

            '選択されているチップの予約IDを取得する.
            Dim selectedRezId As String
            selectedRezId = Me.HiddenSelectedReserveId.Value
            Logger.Info("ButtonSuspendWork_Click selectedRezId=" + selectedRezId)

            '取得した予約IDがNull値でない場合のみ
            If (Not String.IsNullOrEmpty(selectedRezId)) Then

                Dim resultInterference As Integer = ValidationInterferenceBreakUnavailable()
                Logger.Info("ButtonSuspendWork_Click resultInterference=" + CType(resultInterference, String))

                '休憩・使用不可に干渉する場合、作業伸長ポップアップの表示フラグをセットする.
                If (resultInterference = INTERFERENCE_FAILE) Then
                    Logger.Info("ButtonSuspendWork_Click HiddenBreakPopup=" + POPUP_BREAK_DISPLAY)
                    HiddenBreakPopup.Value = POPUP_BREAK_DISPLAY

                ElseIf (ValidationInterferenceBreakUnavailable() = INTERFERENCE_SUCCESSFULL) Then
                    Logger.Info("ButtonSuspendWork_Click ValidationInterferenceBreakUnavailable is successfull")
                    '登録するためのキー情報（IDと休憩を挟むフラグ）を引数として、当日処理関数を呼び出す.
                    SuspendWorkProcess(False)

                End If

            End If
        Finally
            Me.HiddenReloadFlag.Value = String.Empty
        End Try
        '2012/03/02 上田 フッタボタン制御 End

        Logger.Info("ButtonSuspendWork_Click End")

    End Sub


    ''' <summary>
    ''' 当日終了処理
    ''' </summary>
    ''' <param name="breakExtention"></param>
    ''' <remarks></remarks>
    Private Sub SuspendWorkProcess(ByVal breakExtention As Boolean)

        Logger.Info("SuspendWorkProcess Start")

        '選択されているチップの予約IDを取得する.
        Dim selectedRezId As String
        selectedRezId = Me.HiddenSelectedReserveId.Value
        Logger.Info("SuspendWorkProcess selectedId=" + selectedRezId)

        '当日終了処理を実施する.
        Dim resultEvent As Integer
        resultEvent = businessLogic.SuspendWork(objStaffContext.DlrCD, objStaffContext.BrnCD, _
                                    CType(selectedRezId, Integer), Me.stallId, objStaffContext.Account, breakExtention)
        Logger.Info("SuspendWorkProcess resultEvent=" + CType(resultEvent, String))

        '正常に終了していない場合、干渉エラーのエラーメッセージを表示する.
        If (resultEvent <> ERROR_CODE_START_WORK_SUCCESSFULL) Then
            Logger.Info("SuspendWorkProcess The selected Chip is overlapping with next  arranged Chip")
            MyBase.ShowMessageBox(resultEvent, "The selected Chip is overlapping with next  arranged Chip")
            Exit Sub
        End If

        '再描画のため、チップ情報の最新を取得し、作業対象チップ情報を格納する.
        GetChipDataFromServer()

        Logger.Info("SuspendWorkProcess End")

    End Sub



    ''' <summary>
    ''' 検査開始ボタン処理
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub ButtonStartCheck_Click(sender As Object, e As System.EventArgs) Handles ButtonStartCheck.Click

        Logger.Info("ButtonStartCheck_Click Start")

        '押したフッタボタンの状態を、「検査開始」に設定する.
        HiddenPushedFooter.Value = PUSHED_FOOTER_BUTTON_START_CHECK
        Logger.Info("ButtonStartCheck_Click HiddenPushedFooter=" + PUSHED_FOOTER_BUTTON_START_CHECK)

        '完成検査入力を押下したときと同様の動作
        CompletionCheckButton_Click(sender, e)

        Logger.Info("ButtonStartCheck_Click End")

    End Sub
#End Region

#Region "バリデーション"

    'BizLogicの開始処理・終了処理・当日処理内にて実施するため、処理開始前のバリデーションはコメントアウトする.

    ' ''' <summary>
    ' ''' バリデーション（入庫済み）
    ' ''' </summary>
    ' ''' <returns>成否</returns>
    ' ''' <remarks></remarks>
    'Private Function ValidationEnterTheShed(aCandidateDataInfo As DataRow) As Boolean

    '    Logger.Info("ValidationEnterTheShed Start")

    '    Dim isEnterTheShed As Boolean = False

    '    '入庫日時がDBNullでない場合、バリデーション開始
    '    If Not (IsDBNull(aCandidateDataInfo("STRDATE"))) Then

    '        'Dim dtmStrDate As DateTime = Date.ParseExact(aCandidateDataInfo("STARTTIME").ToString(), "yyyy/MM/dd H:mm:ss", Nothing)
    '        'Dim dtmStrDate As DateTime = DateTimeFunc.FormatString("yyyy/MM/dd HH:mm", aCandidateDataInfo("STRDATE").ToString)
    '        '入庫日時が設定されているため、入庫済みであると判定
    '        isEnterTheShed = True

    '    End If

    '    Logger.Info("ValidationEnterTheShed End")
    '    Return isEnterTheShed

    'End Function


    ' ''' <summary>
    ' ''' バリデーション（本予約）
    ' ''' </summary>
    ' ''' <returns>成否</returns>
    ' ''' <remarks></remarks>
    'Private Function ValidationReserve(aCandidateDataInfo As DataRow) As Boolean

    '    Logger.Info("ValidationReserve Start")

    '    Dim blnValidation As Boolean = False

    '    'ステータスがDBNullでない場合、バリデーション開始
    '    If Not (IsDBNull(aCandidateDataInfo("STATUS"))) Then

    '        Dim intStatus As Integer = CType(aCandidateDataInfo("STATUS").ToString(), Integer)
    '        'ステータスが本予約の値であるならば、チェックを通す
    '        If (intStatus = STATUS_RESERVE) Then
    '            blnValidation = True
    '        End If

    '    End If

    '    Logger.Info("ValidationReserve End")
    '    Return blnValidation

    'End Function



    ' ''' <summary>
    ' ''' バリデーション（チップ干渉）
    ' ''' </summary>
    ' ''' <param name="aChipInfo"></param>
    ' ''' <param name="aCandidateDataInfo"></param>
    ' ''' <returns>バリデーション結果</returns>
    ' ''' <remarks></remarks>
    'Private Function ValidationInterference(aChipInfo As SC3150101DataSet.SC3150101ChipInfoDataTable, _
    '                                        aCandidateDataInfo As DataRow) As Integer

    '    Logger.Info("ValidationInterference Start")

    '    Dim isInterfere As Integer = INTERFERENCE_FAILE
    '    Dim dtmEstimateEndTime As Date

    '    '該当するチップの開始時間（予定）と終了時間（予定）を取得する.
    '    If (IsDBNull(aCandidateDataInfo("STARTTIME")) Or IsDBNull(aCandidateDataInfo("ENDTIME"))) Then
    '        isInterfere = INTERFERENCE_FAILE
    '    Else
    '        Dim objRezId As Object = aCandidateDataInfo("REZID")
    '        'Dim dtmStart As Date = Date.ParseExact(aCandidateDataInfo("STARTTIME").ToString(), "yyyy/MM/dd H:mm:ss", Nothing)
    '        'Dim dtmEnd As Date = Date.ParseExact(aCandidateDataInfo("ENDTIME").ToString(), "yyyy/MM/dd H:mm:ss", Nothing)
    '        Dim dtmStart As Date = CType(aCandidateDataInfo("STARTTIME"), Date)
    '        Dim dtmEnd As Date = CType(aCandidateDataInfo("ENDTIME"), Date)
    '        Dim ts As New TimeSpan(dtmEnd.Subtract(dtmStart).Ticks)

    '        '現在時刻より作業開始するため、現在時刻を取得し、推定作業終了時刻を取得する.
    '        Dim dtmNowTime As Date = DateTimeFunc.Now(objStaffContext.DlrCD)
    '        dtmEstimateEndTime = dtmNowTime.Add(ts)

    '        '作業チップとの干渉チェックを実施する.
    '        If (ValidationInterferenceWorkChip(dtmNowTime, dtmEstimateEndTime, objRezId, aChipInfo)) Then

    '        End If

    '        '作業チップと休憩チップの干渉チェックを実施する.
    '        Dim breakInterfereEndTime As Date
    '        breakInterfereEndTime = ValidationInterferenceBreakChip(dtmNowTime, dtmEstimateEndTime)
    '        '休憩チップとの干渉チェックをした結果、返り値の時間が引数の終了時間と異なる場合、干渉が発生している.
    '        If (breakInterfereEndTime <> dtmEstimateEndTime) Then

    '        End If

    '        '使用不可チップとの干渉をチェックした結果、返り値の時間が引数の終了時間と異なる場合、干渉が発生している.
    '        Dim unavailableInterfereEndTime As Date
    '        unavailableInterfereEndTime = ValidationInterferenceUnavailableChip(dtmNowTime, dtmEstimateEndTime)
    '        '休憩チップ、使用不可チップとの干渉チェックの結果、時間が更新されていれば重複があったものする.

    '        '再度チップと干渉を確認する（休憩・使用不可チップの伸長により、作業終了予定時間が変更されている可能性があるため）.

    '    End If

    '    Logger.Info("ValidationInterference End")

    '    isInterfere = INTERFERENCE_SUCCESSFULL

    '    Return isInterfere

    'End Function



    ' ''' <summary>
    ' ''' 作業チップの干渉チェック
    ' ''' </summary>
    ' ''' <param name="aNowTime">作業開始時刻</param>
    ' ''' <param name="aEstimateEndTime">推定作業完了時刻</param>
    ' ''' <param name="aCandidateRezId">現在選択中チップ情報</param>
    ' ''' <param name="aChipInfo">作業チップ情報</param>
    ' ''' <returns>干渉結果</returns>
    ' ''' <remarks></remarks>
    'Private Function ValidationInterferenceWorkChip(aNowTime As Date, aEstimateEndTime As Date, aCandidateRezId As Object, _
    '                                                                    aChipInfo As SC3150101DataSet.SC3150101ChipInfoDataTable) As Boolean

    '    '仮予約チップと干渉した場合、その仮予約チップを動かした状態で干渉チェックを実施することになる.
    '    '選択中チップの推定作業完了時刻は変更ないが、仮予約と干渉した場合、その仮予約の推定作業完了時刻を推定作業完了時刻として以降をチェックする.
    '    '最終的に、その状態で、他の本予約チップと干渉しない＋ストール作業時間内であるという条件を満たせば、
    '    '選択チップの推定作業完了時刻でOKということになる.
    '    '前提条件として、作業チップ情報は、開始時間でソートされている必要がある.

    '    Logger.Info("ValidationInterferenceWorkChip Start")

    '    Dim isInterfere As Boolean = True

    '    Dim dtmNowTime As Date = aNowTime
    '    Dim dtmEstimateEndTime As Date = aEstimateEndTime

    '    '所持しているチップ情報をすべてループして、他のチップとの干渉をチェック
    '    'For Each dr As DataRow In aChipInfo
    '    For i As Integer = 0 To (aChipInfo.Count - 1) Step 1

    '        Dim dr As DataRow = aChipInfo(i)

    '        '対象チップ以外の場合のみ干渉をチェック
    '        If Not aCandidateRezId.Equals(dr("REZID")) Then

    '            Dim dtmSTime2 As DateTime
    '            If Not IsDBNull(dr("RESULT_START_TIME")) Then
    '                '開始時間（実績）を取得
    '                'dtmSTime2 = ExchangeTimeString(dr("RESULT_START_TIME").ToString())
    '                'dtmSTime2 = Date.ParseExact(dr("RESULT_START_TIME").ToString(), "yyyy/MM/dd H:mm:ss", Nothing)
    '                dtmSTime2 = CType(dr("RESULT_START_TIME"), Date)
    '            Else
    '                '開始時間（実績）がない場合、開始時間（予定）を取得
    '                'dtmSTime2 = Date.ParseExact(dr("STARTTIME").ToString(), "yyyy/MM/dd H:mm:ss", Nothing)
    '                dtmSTime2 = CType(dr("STARTTIME"), Date)
    '            End If

    '            Dim dtmETime2 As DateTime
    '            If Not IsDBNull(dr("RESULT_END_TIME")) Then
    '                '終了時間（実績）を取得
    '                'dtmETime2 = ExchangeTimeString(dr("RESULT_END_TIME").ToString())
    '                'dtmETime2 = Date.ParseExact(dr("RESULT_END_TIME").ToString(), "yyyy/MM/dd H:mm:ss", Nothing)
    '                dtmETime2 = CType(dr("RESULT_END_TIME"), Date)
    '            Else
    '                '終了時間（実績）がない場合、終了時間（予定）を取得
    '                'dtmETime2 = Date.ParseExact(dr("ENDTIME").ToString(), "yyyy/MM/dd H:mm:ss", Nothing)
    '                dtmETime2 = CType(dr("ENDTIME"), Date)
    '            End If

    '            '計算対象の開始時間が、干渉チェック中チップの終了時間より小さい場合、且つ、
    '            '干渉チェック中チップの開始時間が、計算対象の終了時間より小さい場合、「干渉する」と判定
    '            If ((dtmNowTime < dtmETime2) And (dtmSTime2 < dtmEstimateEndTime)) Then
    '                'チップの干渉が確認された際に、干渉チェック中のチップステータスを確認する
    '                'Dim drStatus = CType(dr("Status"), Integer)
    '                'ここで干渉後の
    '                isInterfere = False
    '                Exit For
    '            End If
    '        End If
    '    Next

    '    Logger.Info("ValidationInterferenceWorkChip End")
    '    Return isInterfere

    'End Function


    ' ''' <summary>
    ' ''' 休憩チップと作業対象チップの干渉チェック
    ' ''' </summary>
    ' ''' <param name="aNowTime">作業対象チップの開始時間</param>
    ' ''' <param name="aEstimateEndTime">作業対象チップの終了時間</param>
    ' ''' <returns></returns>
    ' ''' <remarks></remarks>
    'Private Function ValidationInterferenceBreakChip(aNowTime As Date, aEstimateEndTime As Date) As Date

    '    Logger.Info("ValidationInterferenceBreakChip Start param1:" + DateTimeFunc.FormatDate(DATE_CONVERT_ID_YYYYMMDDHHMM, aNowTime) + _
    '                " param2:" + DateTimeFunc.FormatDate(DATE_CONVERT_ID_YYYYMMDDHHMM, aEstimateEndTime))

    '    '返り値となる終了時間を初期化する.
    '    Dim checkedEndTime As Date = aEstimateEndTime

    '    '時間ソートされた休憩チップ情報を取得する.
    '    'Dim breakDataTable As SC3150101DataSet.SC3150101BreakChipInfoDataTable
    '    Dim breakDataTable As SC3150101DataSet.SC3150101ChipInfoDataTable
    '    breakDataTable = businessLogic.GetBreakData(Me.stallId)

    '    '取得した休憩チップ情報をループ処理し、作業対象チップとの干渉を検証する.
    '    '干渉が発生した場合、その干渉を加算した終了時間を計算する.
    '    For Each eachBreakData As DataRow In breakDataTable.Rows

    '        Dim eachStartTime As Date = CType(eachBreakData("STARTTIME"), Date)
    '        Dim eachEndTime As Date = CType(eachBreakData("ENDTIME"), Date)

    '        If ((aNowTime < eachEndTime) And (eachStartTime < checkedEndTime)) Then
    '            Dim breakTime = eachEndTime - eachStartTime
    '            checkedEndTime = checkedEndTime.Add(breakTime)
    '        End If
    '    Next

    '    Logger.Info("ValidationInterferenceBreakChip End Return:" + DateTimeFunc.FormatDate(DATE_CONVERT_ID_YYYYMMDDHHMM, checkedEndTime))
    '    Return checkedEndTime

    'End Function


    ' ''' <summary>
    ' ''' 使用不可チップと作業対象チップとの干渉チェック
    ' ''' </summary>
    ' ''' <param name="aTargetStartTime">作業対象チップの開始時間</param>
    ' ''' <param name="aTargetEndTime">作業対象チップの終了時間</param>
    ' ''' <returns></returns>
    ' ''' <remarks></remarks>
    'Private Function ValidationInterferenceUnavailableChip(aTargetStartTime As Date, aTargetEndTime As Date) As Date

    '    Logger.Info("ValidationInterferenceUnavailableChip Start param1:" + DateTimeFunc.FormatDate(DATE_CONVERT_ID_YYYYMMDDHHMM, aTargetStartTime) + _
    '                " param2:" + DateTimeFunc.FormatDate(DATE_CONVERT_ID_YYYYMMDDHHMM, aTargetEndTime))

    '    '返り値となる終了時間を初期化する.
    '    Dim checkedEndTime As Date = aTargetEndTime

    '    '時間ソートされた使用不可チップ情報を取得する.
    '    Dim unavailableDataTable As SC3150101DataSet.SC3150101ChipInfoDataTable
    '    unavailableDataTable = businessLogic.GetUnavailableData(Me.stallId, Me.stallActualStartTime, Me.stallActualEndTime)

    '    '取得した使用不可チップ情報をループ処理し、作業対象チップとの干渉を検証する.
    '    '干渉が発生した場合、その干渉を加算した終了時間を計算する.
    '    For Each eachUnavailableData As DataRow In unavailableDataTable.Rows

    '        Dim eachStartTime As Date = CType(eachUnavailableData("STARTTIME"), Date)
    '        Dim eachEndTime As Date = CType(eachUnavailableData("ENDTIME"), Date)

    '        If ((aTargetStartTime < eachEndTime) And (eachStartTime < checkedEndTime)) Then
    '            Dim breakTime = eachEndTime - eachStartTime
    '            checkedEndTime = checkedEndTime.Add(breakTime)
    '        End If
    '    Next

    '    Logger.Info("ValidationInterferenceUnavailableChip End Return:" + DateTimeFunc.FormatDate(DATE_CONVERT_ID_YYYYMMDDHHMM, checkedEndTime))
    '    Return checkedEndTime

    'End Function


    '↓ここから休憩・使用不可検証
    ''' <summary>
    ''' バリデーション（休憩・使用不可チップとの干渉）
    ''' </summary>
    ''' <returns>バリデーション結果</returns>
    ''' <remarks></remarks>
    Private Function ValidationInterferenceBreakUnavailable() As Integer

        Logger.Info("ValidationInterferenceBreakUnavailable Start")

        ''チップ情報の最新を取得し、作業対象チップを取得する.
        'Dim dtChipInfo As SC3150101DataSet.SC3150101ChipInfoDataTable
        'dtChipInfo = GetNewestChipInfo()
        'Dim drCandidateChipInfo As SC3150101DataSet.SC3150101ChipInfoRow
        'drCandidateChipInfo = GetCandidateChipInfo(dtChipInfo)

        '選択中のチップ情報を取得する.
        Dim selectedChipInfo As SC3150101DataSet.SC3150101ChipInfoRow
        selectedChipInfo = GetSelectedChipInfo()

        Dim isInterfere As Integer = INTERFERENCE_SUCCESSFULL
        Dim dtmEstimateEndTime As Date

        '作業対象チップが存在する場合、バリデーションを実施する.
        'If (Not IsNothing(drCandidateChipInfo)) Then
        If (Not IsNothing(selectedChipInfo)) Then
            Logger.Info("ValidationInterferenceBreakUnavailable Not IsNothing selectedChipInfo")
            '該当するチップの開始時間（予定）と終了時間（予定）を取得する.
            'If (IsDBNull(drCandidateChipInfo.STARTTIME) Or IsDBNull(drCandidateChipInfo.ENDTIME)) Then
            If (selectedChipInfo.IsSTARTTIMENull()) Or (selectedChipInfo.IsENDTIMENull()) Then
                Logger.Info("ValidationInterferenceBreakUnavailable selectedChipInfo.STARTTIME is DBNull or selectedChipInfo.ENDTIME is DBNull")
                isInterfere = INTERFERENCE_FAILE
            Else
                'Dim dtmStart As Date = drCandidateChipInfo.STARTTIME
                'Dim dtmEnd As Date = drCandidateChipInfo.ENDTIME
                Dim dtmStart As Date = selectedChipInfo.STARTTIME
                Dim dtmEnd As Date = selectedChipInfo.ENDTIME
                Dim ts As New TimeSpan(dtmEnd.Subtract(dtmStart).Ticks)
                Logger.Info("ValidationInterferenceBreakUnavailable startTime:" + DateTimeFunc.FormatDate(1, dtmStart))
                Logger.Info("ValidationInterferenceBreakUnavailable endTime:" + DateTimeFunc.FormatDate(1, dtmEnd))
                Logger.Info("ValidationInterferenceBreakUnavailable endTime-startTime=timespan:" + ts.ToString())

                '現在時刻より作業開始するため、現在時刻を取得し、推定作業終了時刻を取得する.
                Dim dtmNowTime As Date = DateTimeFunc.Now(objStaffContext.DlrCD)
                dtmEstimateEndTime = dtmNowTime.Add(ts)
                Logger.Info("ValidationInterferenceBreakUnavailable estimateEndTime:" + DateTimeFunc.FormatDate(1, dtmEstimateEndTime))

                '休憩チップとの干渉チェックし、干渉が発生する場合、干渉発生を返す.
                If (ValidationInterferenceBreakChip(dtmNowTime, dtmEstimateEndTime)) Then
                    Logger.Info("ValidationInterferenceBreakUnavailable Interference BreakChip")
                    isInterfere = INTERFERENCE_FAILE
                Else
                    Logger.Info("ValidationInterferenceBreakUnavailable ")
                    '使用不可チップとの干渉をチェックし、干渉が発生する場合、干渉発生を返す.
                    If (ValidationInterferenceUnavailableChip(dtmNowTime, dtmEstimateEndTime)) Then
                        Logger.Info("ValidationInterferenceBreakUnavailable Interference UnavailableChip")
                        isInterfere = INTERFERENCE_FAILE
                    End If

                End If
            End If
        End If

        Logger.Info("ValidationInterferenceBreakUnavailable End Return:" + CType(isInterfere, String))

        Return isInterfere

    End Function


    ''' <summary>
    ''' 休憩チップと作業対象チップの干渉チェック
    ''' </summary>
    ''' <param name="aNowTime">作業対象チップの開始時間</param>
    ''' <param name="aEstimateEndTime">作業対象チップの終了時間</param>
    ''' <returns>干渉する：true,干渉しない：false</returns>
    ''' <remarks></remarks>
    Private Function ValidationInterferenceBreakChip(aNowTime As Date, aEstimateEndTime As Date) As Boolean

        Logger.Info("ValidationInterferenceBreakChip Start startTime:" + DateTimeFunc.FormatDate(1, aNowTime) _
                      + ", endTime:" + DateTimeFunc.FormatDate(1, aEstimateEndTime))

        '返り値を初期化する.
        Dim resultCheck As Boolean = False

        '時間ソートされた休憩チップ情報を取得する.
        'Dim breakDataTable As SC3150101DataSet.SC3150101BreakChipInfoDataTable
        Dim breakDataTable As SC3150101DataSet.SC3150101ChipInfoDataTable
        breakDataTable = businessLogic.GetBreakData(Me.stallId)

        '取得した休憩チップ情報をループ処理し、作業対象チップとの干渉を検証する.
        For Each eachBreakData As DataRow In breakDataTable.Rows
            Logger.Info("ValidationInterferenceBreakChip ")

            Dim eachStartTime As Date = CType(eachBreakData("STARTTIME"), Date)
            Dim eachEndTime As Date = CType(eachBreakData("ENDTIME"), Date)
            Logger.Info("ValidationInterferenceBreakChip eachStartTime:" + DateTimeFunc.FormatDate(1, eachStartTime))
            Logger.Info("ValidationInterferenceBreakChip eachEndTime:" + DateTimeFunc.FormatDate(1, eachEndTime))

            If ((aNowTime < eachEndTime) And (eachStartTime < aEstimateEndTime)) Then
                Logger.Info("ValidationInterferenceBreakChip ((startTime < eachEndTime) AND (eachEndTime < endTime))")
                resultCheck = True
                Exit For
            End If
        Next

        Logger.Info("ValidationInterferenceBreakChip End Return:" + resultCheck.ToString())
        Return resultCheck

    End Function


    ''' <summary>
    ''' 使用不可チップと作業対象チップとの干渉チェック
    ''' </summary>
    ''' <param name="aTargetStartTime">作業対象チップの開始時間</param>
    ''' <param name="aTargetEndTime">作業対象チップの終了時間</param>
    ''' <returns>干渉する：true,干渉しない：false</returns>
    ''' <remarks></remarks>
    Private Function ValidationInterferenceUnavailableChip(aTargetStartTime As Date, aTargetEndTime As Date) As Boolean

        Logger.Info("ValidationInterferenceUnavailableChip Start startTime:" + DateTimeFunc.FormatDate(1, aTargetStartTime) _
                     + ", endTime:" + DateTimeFunc.FormatDate(1, aTargetEndTime))

        '返り値となる値を初期化する.
        Dim resultCheck As Boolean = False

        '時間ソートされた使用不可チップ情報を取得する.
        Dim unavailableDataTable As SC3150101DataSet.SC3150101ChipInfoDataTable
        unavailableDataTable = businessLogic.GetUnavailableData(Me.stallId, Me.stallActualStartTime, Me.stallActualEndTime)

        '取得した使用不可チップ情報をループ処理し、作業対象チップとの干渉を検証する.
        For Each eachUnavailableData As DataRow In unavailableDataTable.Rows
            Logger.Info("ValidationInterferenceUnavailableChip ")

            Dim eachStartTime As Date = CType(eachUnavailableData("STARTTIME"), Date)
            Dim eachEndTime As Date = CType(eachUnavailableData("ENDTIME"), Date)
            Logger.Info("ValidationInterferenceUnavailableChip eachStartTime:" + DateTimeFunc.FormatDate(1, eachStartTime))
            Logger.Info("ValidationInterferenceUnavailableChip eachEndTime:" + DateTimeFunc.FormatDate(1, eachEndTime))

            If ((aTargetStartTime < eachEndTime) And (eachStartTime < aTargetEndTime)) Then
                Logger.Info("ValidationInterferenceUnavailableChip ((startTime < eachEndTime) AND (eachEndTime < endTime))")
                resultCheck = True
                Exit For
            End If
        Next

        Logger.Info("ValidationInterferenceUnavailableChip End Return:" + resultCheck.ToString())
        Return resultCheck

    End Function


#End Region

#Region "JavaScriptのイベントよりコールされる処理"

    ''' <summary>
    ''' R/O情報欄をフリックした際のイベント処理
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub HiddenButtonFlickRepairOrder_Click(sender As Object, e As System.EventArgs) _
        Handles HiddenButtonFlickRepairOrder.Click

        Logger.Info("HiddenButtonFlickRepairOrder_Click Start")
        '完成検査入力画面へ遷移.
        'CompletionCheckButton_Click(sender, e)

        '問答無用で、チェックシート入力へ飛ばす.
        '完成検査チェックシート入力ページに渡す引数をセッションに格納
        Dim orderNumber As String = Me.HiddenFieldOrderNo.Value
        Dim childNumber As String = Me.HiddenFieldChildNo.Value

        MyBase.SetValue(ScreenPos.Next, "Redirect.ORDERNO", orderNumber)
        MyBase.SetValue(ScreenPos.Next, "Redirect.SRVADDSEQ", childNumber)

        Logger.Info("HiddenButtonFlickRepairOrder_Click SESSION_ORDERNO:" + orderNumber)
        Logger.Info("HiddenButtonFlickRepairOrder_Click SESSION_SRVADDSEQ:" + childNumber)

        Logger.Info("HiddenButtonFlickRepairOrder_Click End")
        Me.RedirectNextScreen(COMPLETION_CHECK_INPUT_PAGE_ID)

    End Sub


    ''' <summary>
    ''' R/O情報の追加作業アイコンをタップした際の処理
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub HiddenButtonRepairOrderIcon_Click(sender As Object, e As System.EventArgs) _
        Handles HiddenButtonRepairOrderIcon.Click

        Logger.Info("HiddenButtonRepairOrderIcon_Click Start")

        Dim iconNumber As Integer
        iconNumber = CType(Me.HiddenFieldRepairOrderIcon.Value, Integer)
        Logger.Info("HiddenButtonRepairOrderIcon_Click AddWorkIconNumber:" + CType(iconNumber, String))

        Dim orderNumber As String = Me.HiddenFieldOrderNo.Value
        Dim childNumber As String = CType(iconNumber, String)
        Dim editValue As String = "0"
        Dim nextPageId As String = REPAIR_ORDERE_PREVIEW_PAGE

        '追加作業ボタンが押下された場合、追加作業情報プレビュー画面へ遷移.
        If (0 < iconNumber) Then
            Logger.Info("HiddenButtonRepairOrderIcon_Click Pushed AddWorkIcon")

            Logger.Info("HiddenButtonRepairOrderIcon_Click SESSION_ORDERNO:" + orderNumber)
            Logger.Info("HiddenButtonRepairOrderIcon_Click SESSION_SRVADDSEQ:" + childNumber)
            Logger.Info("HiddenButtonRepairOrderIcon_Click SESSION_EDITFLG:" + editValue)
            MyBase.SetValue(ScreenPos.Next, "Redirect.ORDERNO", orderNumber)
            MyBase.SetValue(ScreenPos.Next, "Redirect.SRVADDSEQ", childNumber)
            MyBase.SetValue(ScreenPos.Next, "Redirect.EDITFLG", editValue)

            nextPageId = ADD_REPAIR_PREVIEW_PAGE
        Else
            'Rボタンが押下された場合、R/Oプレビュー画面へ遷移.
            Logger.Info("HiddenButtonRepairOrderIcon_Click Pushed R/O Icon")

            Logger.Info("HiddenButtonRepairOrderIcon_Click SESSION_ORDERNO:" + orderNumber)
            MyBase.SetValue(ScreenPos.Next, "OrderNo", orderNumber)

            nextPageId = REPAIR_ORDERE_PREVIEW_PAGE
        End If

        Logger.Info("HiddenButtonRepairOrderIcon_Click End RedirectNextScreenID:" + nextPageId)
        Me.RedirectNextScreen(nextPageId)

    End Sub

    ''' <summary>
    ''' 本ページに使用しているインラインフレームに渡すセッション情報を設定する.
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub HiddenButtonChipTap_Click(sender As Object, e As System.EventArgs) _
        Handles HiddenButtonChipTap.Click

        Logger.Info("HiddenButtonChipTap_Click Start")

        'セッションに格納する前に、格納する値をセッションより除去する.
        MyBase.RemoveValue(ScreenPos.Current, "Redirect.ORDERNO")
        MyBase.RemoveValue(ScreenPos.Current, "Redirect.SRVADDSEQ")
        MyBase.RemoveValue(ScreenPos.Current, "Redirect.FILTERFLG")

        Dim orderNumber As String = Me.HiddenFieldOrderNo.Value
        Dim childNumber As String = Me.HiddenFieldChildNo.Value
        Dim repairOrderFilter As String = Me.HiddenFieldRepairOrderFilter.Value

        'オーダーナンバーをセッションに格納する.
        MyBase.SetValue(ScreenPos.Current, "Redirect.ORDERNO", orderNumber)
        '枝番（SMBにおける子番号）をセッションに格納する.
        MyBase.SetValue(ScreenPos.Current, "Redirect.SRVADDSEQ", childNumber)
        'R/O情報欄のフィルターフラグをセッションに格納する.
        MyBase.SetValue(ScreenPos.Current, "Redirect.FILTERFLG", repairOrderFilter)

        Logger.Info("HiddenButtonChipTap_Click SESSION_ORDERNO:" + orderNumber)
        Logger.Info("HiddenButtonChipTap_Click SESSION_SRVADDSEQ:" + childNumber)
        Logger.Info("HiddenButtonChipTap_Click SESSION_FILTERFLG:" + repairOrderFilter)

        Logger.Info("HiddenButtonChipTap_Click End")

    End Sub


    ''' <summary>
    ''' Push通信がきたときの処理
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub HiddenButtonRefresh_Click(sender As Object, e As System.EventArgs) Handles HiddenButtonRefresh.Click

        Logger.Info("HiddenButtonReflesh_Click Start")

        'PageLoadイベント終了後、サーバよりチップ情報を取得する.
        GetChipDataFromServer()

        Logger.Info("HiddenButtonReflesh_Click End")

    End Sub


    ''' <summary>
    ''' 履歴情報がタップされたときの処理
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub HiddenButtonHistory_Click(sender As Object, e As System.EventArgs) Handles HiddenButtonHistory.Click

        Logger.Info("HiddenButtonHistory_Click Start")

        'R/O番号を取得する.
        Dim orderNumber As String
        orderNumber = Me.HiddenHistoryOrderNumber.Value

        'R/O番号が空文字でない場合、遷移処理を実施する.
        If (orderNumber.Length > 0) Then
            Logger.Info("HiddenButtonHistory_Click orderNumber is not blank")

            Logger.Info("HiddenButtonRepairOrderIcon_Click SESSION_ORDERNO:" + orderNumber)
            MyBase.SetValue(ScreenPos.Next, "OrderNo", orderNumber)

            'R/Oプレビュー画面へ遷移.
            Logger.Info("HiddenButtonHistory_Click End NextScreen:" + REPAIR_ORDERE_PREVIEW_PAGE)
            Me.RedirectNextScreen(REPAIR_ORDERE_PREVIEW_PAGE)
        End If

        Logger.Info("HiddenButtonHistory_Click End")

    End Sub

#End Region

End Class