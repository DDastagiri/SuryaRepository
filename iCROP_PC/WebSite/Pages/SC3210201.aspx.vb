'━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
'SC3210202.aspx.vb
'──────────────────────────────────
'機能： ショールームステータスビジュアライゼーション
'補足： 
'作成： 2012/02/06 KN m.okamura
'更新： 2013/01/18 TMEJ m.asano  【問連】GTMC121225110 対応 $01
'──────────────────────────────────

Imports Toyota.eCRB.SystemFrameworks.Web
Imports Toyota.eCRB.SystemFrameworks.Core
Imports Toyota.eCRB.SystemFrameworks.Core.iCROP.BizLogic
Imports Toyota.eCRB.SystemFrameworks.Core.iCROP.DataAccess
Imports Toyota.eCRB.SystemFrameworks.Web.Controls
Imports Toyota.eCRB.Visit.Api.BizLogic
Imports Toyota.eCRB.Visit.Api.DataAccess.VisitUtilityDataSet
Imports Toyota.eCRB.Visit.Api.DataAccess.VisitReceptionDataSet
Imports Toyota.eCRB.CommonUtility.BizLogic
Imports Toyota.eCRB.CommonUtility.DataAccess
Imports System.Data
Imports System.Globalization
Imports System.Web.Services
Imports System.Web.Script.Serialization

''' <summary>
''' ショールームステータスビジュアライゼーション(メインエリア)
''' </summary>
''' <remarks></remarks>
Partial Class PagesSC3210201
    Inherits BasePage

#Region "非公開定数"

    ''' <summary>
    ''' デバッグフラグ
    ''' </summary>
    ''' <remarks></remarks>
    Private Const DebugFlag As Boolean = False

    ''' <summary>
    ''' システム環境設定パラメータ(敬称前後)
    ''' </summary>
    ''' <remarks></remarks>
    Private Const NameTitlePotision As String = "KEISYO_ZENGO"

    ''' <summary>
    ''' システム環境設定パラメータ(顧客写真取得パス)
    ''' </summary>
    ''' <remarks></remarks>
    Private Const FacePictureUploadUrl As String = "FACEPIC_UPLOADURL"

    ''' <summary>
    ''' システム環境設定パラメータ(スタッフ写真取得パス)
    ''' </summary>
    ''' <remarks></remarks>
    Private Const FilePathStaffphoto As String = "URI_STAFFPHOTO"

    ''' <summary>
    ''' 苦情情報日数(N日)
    ''' </summary>
    ''' <remarks></remarks>
    Private Const ComplaintDisplayDate As String = "COMPLAINT_DISPLAYDATE"

    ''' <summary>
    ''' 販売店環境設定パラメータ(画面更新ロック解除時間)
    ''' </summary>
    ''' <remarks></remarks>
    Private Const LockResetTime As String = "LOCK_RESET_INTERVAL"

    ''' <summary>
    ''' 販売店環境設定パラメータ(来店状況未対応警告時間)
    ''' </summary>
    ''' <remarks></remarks>
    Private Const VisitAlertSpan As String = "VISIT_TIME_ALERT_SPAN"

    ''' <summary>
    ''' 販売店環境設定パラメータ(待ち状況未対応警告時間)
    ''' </summary>
    ''' <remarks></remarks>
    Private Const WaitAlertSpan As String = "WAIT_TIME_ALERT_SPAN"

    ''' <summary>
    ''' 販売店環境設定パラメータ（受付通知警告音出力権限コードリスト)
    ''' </summary>
    ''' <remarks></remarks>
    Private NoticeAlarmCodeList As String = "RECEPTION_NOTICE_ALARM_CODE_LIST"

    ''' <summary>
    ''' 販売店環境設定パラメータ(査定警告時間)
    ''' </summary>
    ''' <remarks></remarks>
    Private Const AssessmentSpan As String = "ASSESSMENT_ALERT_SPAN"

    ''' <summary>
    ''' 販売店環境設定パラメータ(価格相談警告時間)
    ''' </summary>
    ''' <remarks></remarks>
    Private Const PriceSpan As String = "PRICE_ALERT_SPAN"

    ''' <summary>
    ''' 販売店環境設定パラメータ(ヘルプ警告時間)
    ''' </summary>
    ''' <remarks></remarks>
    Private Const HelpSpan As String = "HELP_ALERT_SPAN"

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
    ''' メッセージID(900:再描画)
    ''' </summary>
    ''' <remarks></remarks>
    Private Const MessageReloadView As Integer = 900

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
    ''' 文言の数
    ''' </summary>
    ''' <remarks></remarks>
    Private Const WordDictionaryCount As Integer = 33

    ''' <summary>
    ''' 操作ステータス(更新)
    ''' </summary>
    ''' <remarks></remarks>
    Private Const StatusUpdate As String = "1"

    ''' <summary>
    ''' 操作ステータス(読み取り専用)
    ''' </summary>
    ''' <remarks></remarks>
    Private Const StatusReadOnly As String = "2"

    ''' <summary>
    ''' 敬称位置(前)
    ''' </summary>
    ''' <remarks></remarks>
    Private Const NameTitlePositionFront As String = "1"

    ''' <summary>
    ''' 警告音種別：総数
    ''' </summary>
    ''' <remarks></remarks>
    Private Const AlarmKindCount As String = "1"

    ''' <summary>
    ''' 警告音種別：異常件数
    ''' </summary>
    ''' <remarks></remarks>
    Private Const AlarmKindAlert As String = "2"

    ''' <summary>
    ''' お客様情報入力画面のお客様名の文字数
    ''' </summary>
    ''' <remarks></remarks>
    Private Const CustomerDialogCustomerNameSize As Integer = 18

    ''' <summary>
    ''' 商談中詳細 ステータス 1:Hot
    ''' </summary>
    ''' <remarks></remarks>
    Private Const StatusHot As String = "1"

    ''' <summary>
    ''' 商談中詳細 ステータス 2:Warm
    ''' </summary>
    ''' <remarks></remarks>
    Private Const StatusWarm As String = "2"

    ''' <summary>
    ''' 商談中詳細 ステータス 3:Success
    ''' </summary>
    ''' <remarks></remarks>
    Private Const StatusSuccess As String = "3"

    ''' <summary>
    ''' 商談中詳細 ステータス 4:Cold
    ''' </summary>
    ''' <remarks></remarks>
    Private Const StatusCold As String = "4"

    ''' <summary>
    ''' 商談中詳細 ステータス 5:GiveUp
    ''' </summary>
    ''' <remarks></remarks>
    Private Const StatusGiveUp As String = "5"

    ''' <summary>
    ''' 文字列あふれ時対応種類(「...」表示)
    ''' </summary>
    ''' <remarks></remarks>
    Private Const StringAdd As String = "A"

    ''' <summary>
    ''' 文字列あふれ時対応種類(強制カット)
    ''' </summary>
    ''' <remarks></remarks>
    Private Const StringCut As String = "C"

    ''' <summary>
    ''' 値がない場合の設定値
    ''' </summary>
    ''' <remarks></remarks>
    Private Const DataNull As String = "-"

    ''' <summary>
    ''' SSV画面ID
    ''' </summary>
    ''' <remarks></remarks>
    Private Const ReceptionistId As String = "SC3210201"

    ''' <summary>
    ''' メッセージID(正常)
    ''' </summary>
    ''' <remarks></remarks>
    Private Const MessageIdNormal As Integer = 0

    ''' <summary>
    ''' 来店実績ステータス（商談中）
    ''' </summary>
    ''' <remarks></remarks>
    Private Const VisitStatusNegotiate As String = "07"

#Region "セッションキー"

    ''' <summary>
    ''' セッションキー(文言管理)
    ''' </summary>
    ''' <remarks></remarks>
    Private Const SessionKeyWordDictionary As String = "wordDictionary"

    ''' <summary>
    ''' セッションキー(敬称の前後位置)
    ''' </summary>
    ''' <remarks></remarks>
    Private Const SessionKeyNameTitlePos As String = "nameTitlePos"

    ''' <summary>
    ''' セッションキー(顧客写真用パス)
    ''' </summary>
    ''' <remarks></remarks>
    Private Const SessionKeyFacePicPath As String = "facePicPath"

    ''' <summary>
    ''' セッションキー(スタッフ写真用パス)
    ''' </summary>
    ''' <remarks></remarks>
    Private Const SessionKeyStaffPhotoPath As String = "staffPhotoPath"

    ''' <summary>
    ''' セッションキー(遷移元メニュー)
    ''' </summary>
    ''' <remarks></remarks>
    Private Const SessionKeyBeforeFooterId As String = "beforeFooterId"

    ''' <summary>
    ''' セッションキー(苦情情報日数(N日))
    ''' </summary>
    ''' <remarks></remarks>
    Private Const SessionKeyComplaintDateCount As String = "complaintDateCount"

#End Region

#End Region

#Region "イベント処理"

#Region "ページロード"

    ''' <summary>
    ''' ページロード時の処理です。
    ''' </summary>
    ''' <param name="sender">イベント発生元</param>
    ''' <param name="e">イベントデータ</param>
    ''' <remarks></remarks>
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        ' $01【問連】GTMC121225110 対応 Start
        ' 号口環境ではErrorレベルのログしか出力されない為、Errorレベルで記載
        Logger.Error("DebugLog SC3210201 Page_Load_Start Param[" & sender.ToString & "," & e.ToString & "]")
        ' $01【問連】GTMC121225110 対応 End

        If Not Me.IsPostBack Then

            ' Logger.Debug("Page_Load_001 " & "Not PostBack")

            If DebugFlag Then

                ' Logger.Debug("Page_Load_002 " & DebugFlag)

                DebugArea.Visible = True

            End If

            'ログインユーザの情報を格納
            ' Logger.Debug("Page_Load_003" & "Call_Start StaffContext.Current")
            Dim context As StaffContext = StaffContext.Current
            ' Logger.Debug("Page_Load_003" & "Call_End StaffContext.Current Ret[" & context.ToString & "]")

            Dim branchEnvSet As New BranchEnvSetting

            'ロック解除秒数 基盤の環境変数より取得する
            Dim branchEnvSetLockResetTime As DlrEnvSettingDataSet.DLRENVSETTINGRow = Nothing
            Logger.Info("Page_Load_004" & "Call_Start GetEnvSetting Param[" & context.DlrCD & "," & context.BrnCD & "," & LockResetTime & "]")
            branchEnvSetLockResetTime = branchEnvSet.GetEnvSetting(context.DlrCD, context.BrnCD, LockResetTime)
            Logger.Info("Page_Load_004" & "Call_End GetEnvSetting Ret[" & IsDBNull(branchEnvSetLockResetTime) & "]")
            LockResetInterval.Value = branchEnvSetLockResetTime.PARAMVALUE

            '各種時間警告秒数 基盤の環境変数より取得する
            Dim branchEnvSetVisitAlertSpan As DlrEnvSettingDataSet.DLRENVSETTINGRow = Nothing
            Dim branchEnvSetWaitAlertSpan As DlrEnvSettingDataSet.DLRENVSETTINGRow = Nothing
            Dim branchEnvSetAssessmentSpan As DlrEnvSettingDataSet.DLRENVSETTINGRow = Nothing
            Dim branchEnvSetPriceAlertSpan As DlrEnvSettingDataSet.DLRENVSETTINGRow = Nothing
            Dim branchEnvSetHelpAlertSpan As DlrEnvSettingDataSet.DLRENVSETTINGRow = Nothing
            Logger.Info("Page_Load_005" & "Call_Start GetEnvSetting Param[" & context.DlrCD & "," & context.BrnCD & "," & VisitAlertSpan & "]")
            branchEnvSetVisitAlertSpan = branchEnvSet.GetEnvSetting(context.DlrCD, context.BrnCD, VisitAlertSpan)
            Logger.Info("Page_Load_005" & "Call_End GetEnvSetting Ret[" & IsDBNull(branchEnvSetVisitAlertSpan) & "]")
            Logger.Info("Page_Load_006" & "Call_Start GetEnvSetting Param[" & context.DlrCD & "," & context.BrnCD & "," & WaitAlertSpan & "]")
            branchEnvSetWaitAlertSpan = branchEnvSet.GetEnvSetting(context.DlrCD, context.BrnCD, WaitAlertSpan)
            Logger.Info("Page_Load_006" & "Call_End GetEnvSetting Ret[" & IsDBNull(branchEnvSetWaitAlertSpan) & "]")
            Logger.Info("Page_Load_007" & "Call_Start GetEnvSetting Param[" & context.DlrCD & "," & context.BrnCD & "," & AssessmentSpan & "]")
            branchEnvSetAssessmentSpan = branchEnvSet.GetEnvSetting(context.DlrCD, context.BrnCD, AssessmentSpan)
            Logger.Info("Page_Load_007" & "Call_End GetEnvSetting Ret[" & IsDBNull(branchEnvSetAssessmentSpan) & "]")
            Logger.Info("Page_Load_008" & "Call_Start GetEnvSetting Param[" & context.DlrCD & "," & context.BrnCD & "," & PriceSpan & "]")
            branchEnvSetPriceAlertSpan = branchEnvSet.GetEnvSetting(context.DlrCD, context.BrnCD, PriceSpan)
            Logger.Info("Page_Load_008" & "Call_End GetEnvSetting Ret[" & IsDBNull(branchEnvSetPriceAlertSpan) & "]")
            Logger.Info("Page_Load_009" & "Call_Start GetEnvSetting Param[" & context.DlrCD & "," & context.BrnCD & "," & HelpSpan & "]")
            branchEnvSetHelpAlertSpan = branchEnvSet.GetEnvSetting(context.DlrCD, context.BrnCD, HelpSpan)
            Logger.Info("Page_Load_009" & "Call_End GetEnvSetting Ret[" & IsDBNull(branchEnvSetHelpAlertSpan) & "]")

            VisitTimeAlertSpan.Value = branchEnvSetVisitAlertSpan.PARAMVALUE
            WaitTimeAlertSpan.Value = branchEnvSetWaitAlertSpan.PARAMVALUE
            AssessmentAlertSpan.Value = branchEnvSetAssessmentSpan.PARAMVALUE
            PriceAlertSpan.Value = branchEnvSetPriceAlertSpan.PARAMVALUE
            HelpAlertSpan.Value = branchEnvSetHelpAlertSpan.PARAMVALUE

            branchEnvSetVisitAlertSpan = Nothing
            branchEnvSetWaitAlertSpan = Nothing
            branchEnvSetAssessmentSpan = Nothing
            branchEnvSetPriceAlertSpan = Nothing
            branchEnvSetHelpAlertSpan = Nothing

            'スタッフ写真用のパスを取得
            Dim staffPathRow As DlrEnvSettingDataSet.DLRENVSETTINGRow = Nothing
            Logger.Info("Page_Load_010" & "Call_Start GetSystemEnvSetting Param[" & FilePathStaffphoto & "]")
            staffPathRow = branchEnvSet.GetEnvSetting(context.DlrCD, context.BrnCD, FilePathStaffphoto)
            Logger.Info("Page_Load_010" & "Call_End GetSystemEnvSetting Ret[" & IsDBNull(staffPathRow) & "]")
            Dim staffPhotoPath As String = staffPathRow.PARAMVALUE

            staffPathRow = Nothing
            branchEnvSet = Nothing

            Logger.Info("Page_Load_011" & "Call_Start MyBase.SetValue[" & _
                         ScreenPos.Current & "," & SessionKeyStaffPhotoPath & "," & staffPhotoPath & "]")
            MyBase.SetValue(ScreenPos.Current, SessionKeyStaffPhotoPath, staffPhotoPath)

            '文言管理
            Dim wordDictionary As New Dictionary(Of Decimal, String)

            '文言取得(パフォーマンスを考慮しログ出力は行わない)
            For displayId As Decimal = 1 To WordDictionaryCount
                wordDictionary.Add(displayId, WebWordUtility.GetWord(ReceptionistId, displayId))
            Next

            InitWord(wordDictionary)

            Logger.Info("Page_Load_012" & "Call_Start MyBase.SetValue[" & _
                         ScreenPos.Current & "," & SessionKeyWordDictionary & "," & wordDictionary.ToString() & "]")
            MyBase.SetValue(ScreenPos.Current, SessionKeyWordDictionary, wordDictionary)

            Dim sysEnvSet As New SystemEnvSetting

            '敬称の前後位置を基盤の環境変数より取得する
            Dim sysEnvSetTitlePosRow As SystemEnvSettingDataSet.SYSTEMENVSETTINGRow = Nothing
            Logger.Info("Page_Load_013" & "Call_Start GetSystemEnvSetting Param[" & NameTitlePotision & "]")
            sysEnvSetTitlePosRow = sysEnvSet.GetSystemEnvSetting(NameTitlePotision)
            Logger.Info("Page_Load_013" & "Call_End GetSystemEnvSetting Ret[" & IsDBNull(sysEnvSetTitlePosRow) & "]")
            Dim nameTitlePos As String = sysEnvSetTitlePosRow.PARAMVALUE

            sysEnvSetTitlePosRow = Nothing

            Logger.Info("Page_Load_014" & "Call_Start MyBase.SetValue[" & _
                         ScreenPos.Current & "," & SessionKeyNameTitlePos & "," & nameTitlePos & "]")
            MyBase.SetValue(ScreenPos.Current, SessionKeyNameTitlePos, nameTitlePos)

            '顧客写真用のパスを取得
            Dim sysEnvSetPathRow As SystemEnvSettingDataSet.SYSTEMENVSETTINGRow = Nothing
            Logger.Info("Page_Load_015" & "Call_Start GetSystemEnvSetting Param[" & FacePictureUploadUrl & "]")
            sysEnvSetPathRow = sysEnvSet.GetSystemEnvSetting(FacePictureUploadUrl)
            Logger.Info("Page_Load_015" & "Call_End GetSystemEnvSetting Ret[" & IsDBNull(sysEnvSetPathRow) & "]")
            Dim facePicPath As String = sysEnvSetPathRow.PARAMVALUE

            sysEnvSetPathRow = Nothing

            Logger.Info("Page_Load_016" & "Call_Start MyBase.SetValue[" & _
                         ScreenPos.Current & "," & SessionKeyFacePicPath & "," & facePicPath & "]")
            MyBase.SetValue(ScreenPos.Current, SessionKeyFacePicPath, facePicPath)

            '苦情情報日数を取得
            Dim sysEnvSetComplaintDisplayDateRow As SystemEnvSettingDataSet.SYSTEMENVSETTINGRow = Nothing
            Logger.Info("Page_Load_017" & "Call_Start GetSystemEnvSetting Param[" & ComplaintDisplayDate & "]")
            sysEnvSetComplaintDisplayDateRow = sysEnvSet.GetSystemEnvSetting(ComplaintDisplayDate)
            Logger.Info("Page_Load_017" & "Call_End GetSystemEnvSetting Ret[" & IsDBNull(sysEnvSetComplaintDisplayDateRow) & "]")
            Dim complaintDateCount As String = sysEnvSetComplaintDisplayDateRow.PARAMVALUE

            sysEnvSetComplaintDisplayDateRow = Nothing
            sysEnvSet = Nothing

            Logger.Info("Page_Load_018" & "Call_Start MyBase.SetValue[" & _
                         ScreenPos.Current & "," & SessionKeyComplaintDateCount & "," & complaintDateCount & "]")
            MyBase.SetValue(ScreenPos.Current, SessionKeyComplaintDateCount, complaintDateCount)

            '現在日時 基盤より取得
            ' Logger.Debug("Page_Load_019" & "Call_Start DateTimeFunc.Now Param[" & context.DlrCD & "]")
            Dim nowDate As Date = DateTimeFunc.Now(context.DlrCD)
            ' Logger.Debug("Page_Load_019" & "Call_End DateTimeFunc.Now Ret[" & nowDate & "]")

            NowDateString.Value = nowDate.ToString("yyyyMMdd", CultureInfo.InvariantCulture)

            '警告音出力フラグを取得
            AlarmOutputStatus.Value = GetAlarmOutputFlag(context.DlrCD, context.BrnCD, CType(context.OpeCD, Decimal))

        End If

        ' $01【問連】GTMC121225110 対応 Start
        ' 号口環境ではErrorレベルのログしか出力されない為、Errorレベルで記載
        Logger.Error("DebugLog SC3210201 Page_Load_End")
        ' $01【問連】GTMC121225110 対応 End

    End Sub

#End Region

#Region "スタッフ詳細画面表示"

    ''' <summary>
    ''' スタッフ詳細画面表示
    ''' </summary>
    ''' <param name="sender">イベント発生元</param>
    ''' <param name="e">イベントデータ</param>
    ''' <remarks></remarks>
    Protected Sub StaffDetailDisplayButton_Click(ByVal sender As Object, ByVal e As System.EventArgs) _
        Handles StaffDetailDisplayButton.Click

        Logger.Info("StaffDetailDisplayButton_Click_Start Param[" & sender.ToString & "," & e.ToString & "]")

        'エラーメッセージ初期化
        StaffDetailPopoverErrorMessage.Value = String.Empty

        'セールス来店実績連番の取得
        If String.IsNullOrEmpty(StaffDetailDialogVisitSeq.Value) Then

            ' Logger.Debug("StaffDetailDisplayButton_Click_001 StaffDetailDialogVisitSeq.Value is NullOrEmpty")
            Logger.Info("StaffDetailDisplayButton_Click_End")

            Exit Sub

        End If

        ' Logger.Debug("StaffDetailDisplayButton_Click_002 StaffDetailDialogVisitSeq.Value = " & StaffDetailDialogVisitSeq.Value)

        Dim visitSeq As Long = CType(StaffDetailDialogVisitSeq.Value, Long)

        'お客様情報の取得
        Dim customerInfoDataTable As VisitReceptionVisitorCustomerDataTable = Nothing
        Dim businessLogic As New VisitReceptionBusinessLogic
        customerInfoDataTable = businessLogic.GetCustomerInfo(visitSeq, VisitStatusNegotiate)
        businessLogic = Nothing

        'お客様情報取得失敗時は処理を抜ける
        If customerInfoDataTable.Count <= 0 Then

            ' Logger.Debug("StaffDetailDisplayButton_Click_003" & "Call_Start WebWordUtility.GetWord Param[" _
            '            & ReceptionistId & "," & MessageReloadView & "]")
            Dim errorMessage As String = WebWordUtility.GetWord(ReceptionistId, MessageReloadView)
            ' Logger.Debug("StaffDetailDisplayButton_Click_003" & "Call_End WebWordUtility.GetWord Ret[" & errorMessage & "]")

            StaffDetailPopoverErrorMessage.Value = errorMessage

            Logger.Info("StaffDetailDisplayButton_Click_End")

            Return

        End If

        ' Logger.Debug("StaffDetailDisplayButton_Click_004")

        '先頭の情報を取得
        Dim customerRow As VisitReceptionVisitorCustomerRow = customerInfoDataTable.Rows(0)
        customerInfoDataTable = Nothing

        'ログインユーザの情報を格納
        ' Logger.Debug("StaffDetailDisplayButton_Click_005 " & "Call_Start StaffContext.Current")
        Dim context As StaffContext = StaffContext.Current
        ' Logger.Debug("StaffDetailDisplayButton_Click_005 " & "Call_End StaffContext.Current Ret[" & context.ToString & "]")

        '現在日時 基盤より取得
        ' Logger.Debug("StaffDetailDisplayButton_Click_006 " & "Call_Start DateTimeFunc.Now Param[" & context.DlrCD & "]")
        Dim nowDate As Date = DateTimeFunc.Now(context.DlrCD)
        ' Logger.Debug("StaffDetailDisplayButton_Click_006 " & "Call_End DateTimeFunc.Now Ret[" & nowDate & "]")

        '商談中詳細表示設定(依頼リスト)
        InitStaffDetailDialogNoticeListArea(visitSeq, context, nowDate)

        '商談中詳細表示設定(顧客情報)
        InitStaffDetailDialogVisitInfoArea(customerRow, context, nowDate)

        '商談中詳細表示設定(プロセス)
        InitStaffDetailDialogProcessArea(customerRow, context, nowDate)

        Logger.Info("StaffDetailDisplayButton_Click_End")

    End Sub

#End Region

#End Region

#Region "非公開メソッド"

#Region "文言表示初期化"

    ''' <summary>
    ''' 文言管理にDB登録を行い文言番号より取得する
    ''' </summary>
    ''' <param name="wordDictionary">文言管理</param>
    ''' <remarks></remarks>
    Private Sub InitWord(ByVal wordDictionary As Dictionary(Of Decimal, String))

        ' Logger.Debug("InitWord_Start")

        'スタッフ詳細
        'タイトル「商談中」
        StaffDetailNegotiationLiteral.Text = Server.HtmlEncode(wordDictionary(19))
        '来店回数「次来店」
        StaffDetailNowVisitLiteral.Text = Server.HtmlEncode(wordDictionary(20))
        '苦情アイコン「!」
        StaffDetailClaimIconLiteral.Text = Server.HtmlEncode(wordDictionary(10))
        '来店人数「人」
        StaffDetailVisitPersonLiteral.Text = Server.HtmlEncode(wordDictionary(22))
        'テーブル番号「No.」
        DisplayTableNo.Text = Server.HtmlEncode(wordDictionary(23))

        ' Logger.Debug("InitWord_End")

    End Sub

#End Region

#Region "警告音出力フラグ取得"

    ''' <summary>
    ''' 警告音出力フラグ取得
    ''' </summary>
    ''' <param name="dealerCode">販売店コード</param>
    ''' <param name="storeCode">店舗コード</param>
    ''' <param name="target">対象アカウントの権限</param>
    ''' <returns>1:出力あり、0:出力なし</returns>
    ''' <remarks></remarks>
    Private Function GetAlarmOutputFlag(ByVal dealerCode As String, ByVal storeCode As String, ByVal target As Decimal) As Integer
        ' Logger.Debug("GetAlarmOutputFlag_Start " & _
        '            "Param[" & target & "]")

        '通知警告音出力権限コードリストの取得
        Dim branchEnvSet As New BranchEnvSetting
        Dim sysEnvSetNoticeAlarmCodeListRow As DlrEnvSettingDataSet.DLRENVSETTINGRow = Nothing

        Logger.Info("GetAlarmOutputFlag_001" & "Call_Start GetSystemEnvSetting Param[" & NameTitlePotision & "]")
        sysEnvSetNoticeAlarmCodeListRow = branchEnvSet.GetEnvSetting(dealerCode, storeCode, NoticeAlarmCodeList)
        Logger.Info("GetAlarmOutputFlag_001" & "Call_End GetSystemEnvSetting Ret[" & IsDBNull(sysEnvSetNoticeAlarmCodeListRow) & "]")
        Dim operationListName As String = sysEnvSetNoticeAlarmCodeListRow.PARAMVALUE
        Dim operationCdList As String()

        'カンマ区切りで取得
        operationCdList = operationListName.Split(",")

        '初期状態:読み取り専用
        ' Logger.Debug("GetAlarmOutputFlag_001 operation = AlarmOutputOff ")
        Dim alarmOutputFlag As String = AlarmOutputOff

        For Each operation In operationCdList
            If CType(operation, Decimal) = target Then

                '更新に切り替えてforを抜ける
                ' Logger.Debug("GetAlarmOutputFlag_002 operation = AlarmOutputOn ")
                alarmOutputFlag = AlarmOutputOn
                Exit For
            End If
        Next

        ' Logger.Debug("GetAlarmOutputFlag_End " & _
        '            "Ret[" & alarmOutputFlag & "]")
        Return alarmOutputFlag
    End Function
#End Region

#Region "商談中詳細画面表示(依頼リスト)"

    ''' <summary>
    ''' 商談中詳細画面表示(依頼リスト)
    ''' </summary>
    ''' <param name="visitSeq">シーケンス番号</param>
    ''' <param name="context">ログイン情報</param>
    ''' <param name="nowDate">現在日時</param>
    ''' <remarks></remarks>
    Private Sub InitStaffDetailDialogNoticeListArea(ByVal visitSeq As Long, _
                                                    ByVal context As StaffContext, _
                                                    ByVal nowDate As Date)

        ' Logger.Debug("InitStaffDetailDialogNoticeListArea_Start Param[" & visitSeq & _
        '            ", " & context.ToString & ", " & nowDate & ", " & "]")

        Dim staffNoticeRequestDataTable As VisitReceptionStaffNoticeRequestDataTable = Nothing
        Dim businessLogic As New VisitReceptionBusinessLogic
        staffNoticeRequestDataTable = businessLogic.GetStaffNoticeRequest(visitSeq)
        businessLogic = Nothing

        'リピータをバインド(0件の場合はバインドしない)
        If staffNoticeRequestDataTable.Count <= 0 Then

            ' Logger.Debug("InitStaffDetailDialogNoticeListArea_001")

            NoticeListRepeater.Visible = False

            ' Logger.Debug("InitStaffDetailDialogNoticeListArea_End")

            Exit Sub

        End If

        ' Logger.Debug("InitStaffDetailDialogNoticeListArea_002")

        '依頼通知送信日時時間リストを設定
        SendDateList.Value = GetTimeSpanListString(staffNoticeRequestDataTable, "SENDDATE", nowDate)

        Logger.Info("InitStaffDetailDialogNoticeListArea_003 staffNoticeRequestDataTable.Count = " & staffNoticeRequestDataTable.Count)
        NoticeListRepeater.Visible = True
        NoticeListRepeater.DataSource = staffNoticeRequestDataTable
        NoticeListRepeater.DataBind()

        Dim maxLength As Integer = NoticeListRepeater.Items.Count - 1

        '取得データの格納
        For i = 0 To maxLength

            Dim item As Control = NoticeListRepeater.Items(i)
            Dim staffNoticeRequestDataRow As VisitReceptionStaffNoticeRequestRow = staffNoticeRequestDataTable.Item(i)

            '----------------------------------------------------------------------
            ' 依頼種別
            '----------------------------------------------------------------------
            Dim NoticeReqctg As String = staffNoticeRequestDataRow.NOTICEREQCTG
            CType(item.FindControl("NoticeReqctg"), HiddenField).Value = NoticeReqctg

            Dim NoticeListTag As New StringBuilder

            NoticeListTag.Append(CType(item.FindControl("NoticeName"), HtmlGenericControl).Attributes("class"))

            'タグの開始
            If i = 0 Then

                ' Logger.Debug("InitStaffDetailDialogNoticeListArea_004")

                NoticeListTag.Append(" bordernone")

            End If

            '依頼種別の判定
            Select Case NoticeReqctg

                Case NoticeAssessment
                    '査定
                    ' Logger.Debug("InitStaffDetailDialogNoticeListArea_005" & "NoticeReqctg: " & NoticeAssessment)
                    NoticeListTag.Append(" list3On")

                Case NoticePriceConsultation
                    ' Logger.Debug("InitStaffDetailDialogNoticeListArea_006" & "NoticeReqctg: " & NoticePriceConsultation)
                    '価格相談
                    NoticeListTag.Append(" list1On")

                Case NoticeHelp
                    'ヘルプ
                    ' Logger.Debug("InitStaffDetailDialogNoticeListArea_007" & "NoticeReqctg: " & NoticeHelp)
                    NoticeListTag.Append(" list4On")

            End Select

            ' Logger.Debug("InitStaffDetailDialogNoticeListArea_008")

            CType(item.FindControl("NoticeName"), HtmlGenericControl).Attributes("class") = NoticeListTag.ToString
            CType(item.FindControl("NoticeNameLiteral"), Literal).Visible = True

            '査定の場合
            If NoticeReqctg = NoticeAssessment Then

                ' Logger.Debug("InitStaffDetailDialogNoticeListArea_009")

                Dim wordDictionary As Dictionary(Of Decimal, String) = _
                CType(MyBase.GetValue(ScreenPos.Current, SessionKeyWordDictionary, False), Dictionary(Of Decimal, String))

                '中古車スタッフ表示
                CType(item.FindControl("NoticeNameLiteral"), Literal).Text = Server.HtmlEncode(wordDictionary(21))

            Else

                If Not String.IsNullOrEmpty(staffNoticeRequestDataRow.FROMACCOUNTNAME) Then

                    ' Logger.Debug("InitStaffDetailDialogNoticeListArea_010")
                    '送信者名が存在する場合
                    CType(item.FindControl("NoticeNameLiteral"), Literal).Text = Server.HtmlEncode(staffNoticeRequestDataRow.FROMACCOUNTNAME)

                Else

                    ' Logger.Debug("InitStaffDetailDialogNoticeListArea_011")
                    '上記以外の場合
                    CType(item.FindControl("NoticeNameLiteral"), Literal).Text = Server.HtmlEncode(staffNoticeRequestDataRow.TOACCOUNTNAME)

                End If

            End If

        Next

        ' Logger.Debug("InitStaffDetailDialogNoticeListArea_End")

    End Sub

    ''' <summary>
    ''' 経過時間のリスト作成
    ''' </summary>
    ''' <param name="dataTable">データテーブル</param>
    ''' <param name="columnName">カラム名</param>
    ''' <returns>経過時間のリスト</returns>
    ''' <remarks></remarks>
    Private Function GetTimeSpanListString(ByVal dataTable As DataTable, _
                                           ByVal columnName As String, ByVal nowDate As Date) As String

        ' Logger.Debug("GetTimeSpanListString_Start " & _
        '            "Param[" & dataTable.ToString & "," & columnName & "," & nowDate & "]")

        Dim businessLogic As New VisitReceptionBusinessLogic
        Dim timeSpanList As List(Of String) = businessLogic.GetTimeSpanListString(dataTable, columnName, nowDate)
        businessLogic = Nothing

        Dim javaScript As New JavaScriptSerializer

        ' Logger.Debug("GetTimeSpanListString_End Ret[javaScript.Serialize(timeSpanList)] timeSpanList.Count = " & timeSpanList.Count)
        Return javaScript.Serialize(timeSpanList)

    End Function

#End Region

#Region "商談中詳細画面表示(顧客詳細)"

    ''' <summary>
    ''' 商談中詳細画面表示(顧客詳細)
    ''' </summary>
    ''' <param name="customerRow">表示する顧客情報</param>
    ''' <param name="context">ログイン情報</param>
    ''' <param name="nowDate">現在日時</param>
    ''' <remarks></remarks>
    Private Sub InitStaffDetailDialogVisitInfoArea(ByVal customerRow As VisitReceptionVisitorCustomerRow, _
                                                   ByVal context As StaffContext, _
                                                   ByVal nowDate As Date)

        ' Logger.Debug("InitStaffDetailDialogVisitInfoArea_Start Pram[" & customerRow.ToString & "," & _
        '            context.ToString & "," & nowDate & "]")

        '文言管理を取得
        Logger.Info("InitStaffDetailDialogVisitInfoArea_001 " & "Call_Start MyBase.GetValue Param[" & _
                     ScreenPos.Current & "," & SessionKeyWordDictionary & "," & False & "]")
        Dim wordDictionary As Dictionary(Of Decimal, String) = _
         CType(MyBase.GetValue(ScreenPos.Current, SessionKeyWordDictionary, False), Dictionary(Of Decimal, String))
        Logger.Info("InitStaffDetailDialogVisitInfoArea_001 " & "Call_End MyBase.GetValue Ret[" & wordDictionary.ToString() & "]")

        '顧客区分
        Dim StaffDetailDialogCustomerSegment As String = If(customerRow.IsCUSTSEGMENTNull(), String.Empty, customerRow.CUSTSEGMENT)

        '顧客コード
        Dim StaffDetailDialogCustId As String = If(customerRow.IsCUSTIDNull(), String.Empty, customerRow.CUSTID)

        'スタッフ名を取得
        Dim users As New Users
        Dim userDataSetRow As UsersDataSet.USERSRow = users.GetUser(customerRow.ACCOUNT)

        'スタッフ名
        StaffDetailDialogTitleLiteral.Text = Server.HtmlEncode(userDataSetRow.USERNAME)

        users = Nothing
        userDataSetRow = Nothing

        '来店回数を取得
        Dim fllwUpVisitCount As String = If(customerRow.IsFLLOWUPBOX_SEQNONull(), "0", CType(customerRow.FLLOWUPBOX_SEQNO, String))
        Dim visitCountDataTable As VisitReceptionVisitCountDataTable = Nothing
        Dim businessLogic As New VisitReceptionBusinessLogic
        visitCountDataTable = businessLogic.GetVisitCount(context.DlrCD, context.BrnCD, CLng(fllwUpVisitCount))
        businessLogic = Nothing

        '来店回数
        VisitCountLiteral.Text = visitCountDataTable.Item(0)(0) + 1
        visitCountDataTable = Nothing

        '商談開始時間
        StaffDetailDialogSalesStartTime.Value = If(customerRow.IsSALESSTARTNull(), String.Empty, _
                                                   CType(Math.Round(nowDate.Subtract(CType(customerRow.SALESSTART, Date)).TotalSeconds), String))

        '顧客エリア
        If Not IsDBNull(customerRow.CUSTNAME) AndAlso Not String.IsNullOrEmpty(customerRow.CUSTNAME) _
            AndAlso Not String.IsNullOrEmpty(customerRow.CUSTNAME.Trim()) Then

            ' Logger.Debug("InitStaffDetailDialogVisitInfoArea_002 customerRow.CUSTNAME = " & customerRow.CUSTNAME)

            '顧客名
            Dim custName As New StringBuilder

            '敬称の前後位置取得
            Logger.Info("InitStaffDetailDialogVisitInfoArea_003 " & "Call_Start MyBase.GetValue Param[" & _
                         ScreenPos.Current & "," & SessionKeyNameTitlePos & "," & False & "]")
            Dim nameTitlePos As String = CType(MyBase.GetValue(ScreenPos.Current, SessionKeyNameTitlePos, False), String)
            Logger.Info("InitStaffDetailDialogVisitInfoArea_003 " & "Call_End MyBase.GetValue Ret[" & nameTitlePos.ToString() & "]")

            '敬称を取得
            Dim customerNameTitle As String = If(IsDBNull(customerRow.CUSTNAMETITLE) OrElse String.IsNullOrEmpty(customerRow.CUSTNAMETITLE) OrElse _
               String.IsNullOrEmpty(customerRow.CUSTNAMETITLE.Trim()), String.Empty, customerRow.CUSTNAMETITLE)

            '敬称の前後位置によって敬称付顧客名を設定
            If nameTitlePos.Equals(NameTitlePositionFront) Then

                custName.Append(customerNameTitle)
                custName.Append(" ")
                custName.Append(customerRow.CUSTNAME)

            Else

                custName.Append(customerRow.CUSTNAME)
                custName.Append(" ")
                custName.Append(customerNameTitle)

            End If

            '敬称付顧客名
            StaffDetailCustomerName.Text = ChangeString(custName.ToString, CustomerDialogCustomerNameSize, StringAdd)

        Else

            If customerRow.IsCUSTSEGMENTNull() OrElse String.IsNullOrEmpty(customerRow.CUSTSEGMENT) Then

                '新規顧客の場合(新規お客様)
                StaffDetailCustomerName.Text = Server.HtmlEncode(wordDictionary(17))

            Else

                '既存顧客の場合(Unknown)
                StaffDetailCustomerName.Text = Server.HtmlEncode(wordDictionary(18))

            End If

        End If
        ' Logger.Debug("InitStaffDetailDialogVisitInfoArea_004 StaffDetailCustomerName.Text = " & StaffDetailCustomerName.Text)

        'お客様の苦情情報有無の取得
        Logger.Info("InitStaffDetailDialogVisitInfoArea_005 " & "Call_Start MyBase.GetValue Param[" & _
                     ScreenPos.Current & "," & SessionKeyComplaintDateCount & "," & False & "]")
        Dim complaintDateCount As Integer = CType(MyBase.GetValue(ScreenPos.Current, SessionKeyComplaintDateCount, False), Integer)
        Logger.Info("InitStaffDetailDialogVisitInfoArea_005 " & "Call_End MyBase.GetValue Ret[" & complaintDateCount & "]")

        Dim utility As New VisitUtilityBusinessLogic
        ClaimIcon.Visible = utility.HasClaimInfo(StaffDetailDialogCustomerSegment, _
                                                 StaffDetailDialogCustId, _
                                                 nowDate, complaintDateCount)
        utility = Nothing

        '来店人数(取得できない場合は"-")
        If customerRow.IsVISITPERSONNUMNull() Then

            ' Logger.Debug("InitStaffDetailDialogVisitInfoArea_006")
            VisitPersonNumberLiteral.Text = DataNull
            StaffDetailVisitPersonLiteral.Visible = False

        Else

            ' Logger.Debug("InitStaffDetailDialogVisitInfoArea_007")
            VisitPersonNumberLiteral.Text = CType(customerRow.VISITPERSONNUM, String)
            StaffDetailVisitPersonLiteral.Visible = True

        End If

        'テーブルNo.(取得できない場合は"-")
        If customerRow.IsSALESTABLENONull() Then

            ' Logger.Debug("InitStaffDetailDialogVisitInfoArea_008")
            StaffDetailDialogSalesTableNo.Text = DataNull
            DisplayTableNo.Visible = False

        Else

            ' Logger.Debug("InitStaffDetailDialogVisitInfoArea_009")
            StaffDetailDialogSalesTableNo.Text = customerRow.SALESTABLENO
            DisplayTableNo.Visible = True

        End If

        ' Logger.Debug("InitStaffDetailDialogVisitInfoArea_End")

    End Sub

#End Region

#Region "商談中詳細表示設定(プロセス)"

    ''' <summary>
    ''' 商談中詳細表示設定(プロセス)
    ''' </summary>
    ''' <param name="customerRow">表示する顧客情報</param>
    ''' <param name="context">スタッフコンテキスト</param>
    ''' <param name="nowDate">現在日時</param>
    ''' <remarks></remarks>
    Private Sub InitStaffDetailDialogProcessArea(ByVal customerRow As VisitReceptionVisitorCustomerRow, _
                                                   ByVal context As StaffContext, _
                                                   ByVal nowDate As Date)

        ' Logger.Debug("InitStaffDetailDialogProcessArea_Start " & _
        '    "Param[" & customerRow.ToString & "," & context.ToString & "," & nowDate & "]")

        'FollowUpBox-内連番が設定されていない場合
        If customerRow.IsFLLOWUPBOX_SEQNONull Then
            ' Logger.Debug("InitStaffDetailDialogProcessArea_000 IsFLLOWUPBOX_SEQNONull")
            Me.CarName.Text = DataNull
            Me.GradeName.Text = DataNull
            SetProcessDefaultWord(0)
            Me.StaffDetailStatus.Attributes("class") = "IcnNoStatus"
            Exit Sub
        End If

        '契約書No取得
        Dim contractNo As String = GetContractNo(context, customerRow.FLLOWUPBOX_SEQNO)

        '受注前後判定取得
        Dim receptionResult As String = CountFllwupboxRslt(context, customerRow.FLLOWUPBOX_SEQNO)

        Dim seqNo As Long = 0

        '希望車種の取得
        Using selectedSeriesDataTable As ActivityInfoDataSet.ActivityInfoGetSelectedSeriesListToDataTable = _
            GetSeriesList(context, customerRow.FLLOWUPBOX_SEQNO, receptionResult)

            If selectedSeriesDataTable.Count > 0 Then

                ' Logger.Debug("InitStaffDetailDialogProcessArea_001 selectedSeriesDataTable.Count > 0")
                Dim selectedSeriesDataRow As ActivityInfoDataSet.ActivityInfoGetSelectedSeriesListToRow = _
                    selectedSeriesDataTable.Item(0)

                CarName.Text = Server.HtmlEncode(If(selectedSeriesDataRow.IsSERIESNMNull() Or String.IsNullOrEmpty(selectedSeriesDataRow.SERIESNM), _
                                     DataNull, selectedSeriesDataRow.SERIESNM))
                GradeName.Text = Server.HtmlEncode(If(selectedSeriesDataRow.IsVCLMODEL_NAMENull() Or _
                                       String.IsNullOrEmpty(selectedSeriesDataRow.VCLMODEL_NAME), DataNull, selectedSeriesDataRow.VCLMODEL_NAME))

                If Not selectedSeriesDataRow.IsSEQNONull() Then
                    seqNo = selectedSeriesDataRow.SEQNO
                End If

            Else

                ' Logger.Debug("InitStaffDetailDialogProcessArea_002 selectedSeriesDataTable.Count <= 0")
                CarName.Text = DataNull
                GradeName.Text = DataNull

            End If

        End Using

        'お客様のプロセスデータ取得
        Using processDataTable As ActivityInfoDataSet.ActivityInfoGetProcessToDataTable = _
           GetProcess(context, customerRow.FLLOWUPBOX_SEQNO, contractNo)

            If processDataTable.Count <= 0 Then

                ' Logger.Debug("InitStaffDetailDialogProcessArea_003 processDataTable.Count <= 0(Call SetProcessDefaultWord)")
                SetProcessDefaultWord(receptionResult)

            Else

                ' Logger.Debug("InitStaffDetailDialogProcessArea_004 processDataTable.Count > 0")

                Dim processDataRow As ActivityInfoDataSet.ActivityInfoGetProcessToRow = Nothing

                ' 受注前の場合の場合は希望車種に紐づくプロセスを取得するようにする
                If String.Equals(receptionResult, "0") Then

                    For Each row In processDataTable

                        If Not row.IsSEQNONull() AndAlso seqNo = row.SEQNO Then
                            processDataRow = row
                            Exit For
                        End If

                    Next

                Else
                    processDataRow = processDataTable.Item(0)
                End If

                If processDataRow Is Nothing Then
                    ' Logger.Debug("InitStaffDetailDialogProcessArea_004_1 processDataRow Is Nothing")
                    SetProcessDefaultWord(receptionResult)
                Else

                    If String.Equals(receptionResult, "0") Then

                        ' Logger.Debug("InitStaffDetailDialogProcessArea_005 receptionResult = 0(Call SetProcessBeforeWord)")
                        SetProcessBeforeWord(receptionResult, processDataRow)

                    Else

                        ' Logger.Debug("InitStaffDetailDialogProcessArea_006 receptionResult = 1(Call SetProcessAfterWord)")
                        SetProcessAfterWord(receptionResult, processDataRow)

                    End If

                End If

            End If

        End Using

        'お客様のステータス取得
        Using statusDataTable As ActivityInfoDataSet.ActivityInfoGetStatusToDataTable = _
            GetStatus(context, customerRow.FLLOWUPBOX_SEQNO)

            '予めアイコンなしの情報を格納
            StaffDetailStatus.Attributes("class") = "IcnNoStatus"

            '情報が取得できた場合処理
            If statusDataTable.Count > 0 Then

                ' Logger.Debug("InitStaffDetailDialogProcessArea_007 statusDataTable.Count > 0")
                Dim statusDataRow As ActivityInfoDataSet.ActivityInfoGetStatusToRow = _
                   statusDataTable.Item(0)

                'ステータスによって表示アイコンを変更
                Select Case statusDataRow.CRACTRESULT

                    Case StatusHot
                        'ステータス:Hot
                        ' Logger.Debug("InitStaffDetailDialogProcessArea_008 CRACTRESULT = " & StatusHot)
                        StaffDetailStatus.Attributes("class") = "IcnHot"

                    Case StatusWarm
                        'ステータス:Warm
                        ' Logger.Debug("InitStaffDetailDialogProcessArea_009 CRACTRESULT = " & StatusWarm)
                        StaffDetailStatus.Attributes("class") = "IcnWarm"

                    Case StatusSuccess
                        'ステータス:Success
                        ' Logger.Debug("InitStaffDetailDialogProcessArea_010 CRACTRESULT = " & StatusSuccess)
                        StaffDetailStatus.Attributes("class") = "IcnSuccess"

                    Case StatusGiveUp
                        'ステータス:Hot
                        ' Logger.Debug("InitStaffDetailDialogProcessArea_012 CRACTRESULT = " & StatusGiveUp)
                        StaffDetailStatus.Attributes("class") = "IcnGiveup"

                    Case Else
                        'ステータス:Cold
                        ' Logger.Debug("InitStaffDetailDialogProcessArea_011 CRACTRESULT = " & StatusCold)
                        StaffDetailStatus.Attributes("class") = "IcnCold"

                End Select

            End If

            ' Logger.Debug("InitStaffDetailDialogProcessArea_008 statusDataTable.Count <= 0")
        End Using

        ' Logger.Debug("InitStaffDetailDialogProcessArea_End")

    End Sub

#Region "契約書Noの取得"

    ''' <summary>
    ''' 契約書Noの取得
    ''' </summary>
    ''' <param name="context">スタッフコンテキスト</param>
    ''' <param name="followUpBoxSeqNo">Follow-UpBox内連番</param>
    ''' <returns>契約書No(なければEmpty)を返却</returns>
    ''' <remarks></remarks>
    Private Function GetContractNo(ByVal context As StaffContext, ByVal followUpBoxSeqNo As String) As String
        ' Logger.Debug("GetContractNo_Start " & _
        '            "Param[" & context.ToString & "," & followUpBoxSeqNo & "]")

        Using dataTable As New ActivityInfoDataSet.ActivityInfoContractNoFromDataTable
            Dim setRow As ActivityInfoDataSet.ActivityInfoContractNoFromRow = _
                dataTable.NewActivityInfoContractNoFromRow

            '引数の情報をセット
            setRow.DLRCD = context.DlrCD
            setRow.STRCD = context.BrnCD
            setRow.FLLWUPBOX_SEQNO = followUpBoxSeqNo
            dataTable.AddActivityInfoContractNoFromRow(setRow)

            '契約書No取得
            Dim contractNo As String = ActivityInfoBusinessLogic.GetContractNo(dataTable)

            ' Logger.Debug("GetContractNo_End Ret[" & contractNo & "]")
            Return contractNo
        End Using
    End Function

#End Region

#Region "CR活動成功のデータ存在判定"

    ''' <summary>
    ''' CR活動成功のデータ存在判定
    ''' </summary>
    ''' <param name="context">スタッフコンテキスト</param>
    ''' <param name="followUpBoxSeqNo">Follow-UpBox内連番</param>
    ''' <returns>0:受注前、1:受注後</returns>
    ''' <remarks></remarks>
    Private Function CountFllwupboxRslt(ByVal context As StaffContext, ByVal followUpBoxSeqNo As String) As String
        ' Logger.Debug("CountFllwupboxRslt_Start " & _
        '      "Param[" & context.ToString & "," & followUpBoxSeqNo & "]")

        Using dataTable As New ActivityInfoDataSet.ActivityInfoCountFromDataTable
            Dim setRow As ActivityInfoDataSet.ActivityInfoCountFromRow = _
              dataTable.NewActivityInfoCountFromRow

            '引数の情報をセット
            setRow.DLRCD = context.DlrCD
            setRow.STRCD = context.BrnCD
            setRow.FLLWUPBOX_SEQNO = followUpBoxSeqNo
            dataTable.AddActivityInfoCountFromRow(setRow)

            '受注前か後かを判定
            Dim receptionResult As String = ActivityInfoBusinessLogic.CountFllwupboxRslt(dataTable)

            ' Logger.Debug("CountFllwupboxRslt_End Ret[" & receptionResult & "]")
            Return receptionResult
        End Using
    End Function

#End Region

#Region "車種情報取得"

    ''' <summary>
    ''' 車種情報取得
    ''' </summary>
    ''' <param name="context">スタッフコンテキスト</param>
    ''' <param name="followUpBoxSeqNo">Follow-UpBox内連番</param>
    ''' <param name="receptionResult">受注前後判定結果</param>
    ''' <returns>車種リストデータテーブル</returns>
    ''' <remarks></remarks>
    Private Function GetSeriesList(ByVal context As StaffContext, _
                                   ByVal followUpBoxSeqNo As String, _
                                   ByVal receptionResult As String) _
                                   As ActivityInfoDataSet.ActivityInfoGetSelectedSeriesListToDataTable
        ' Logger.Debug("GetSeriesList_Start " & _
        '"Param[" & context.ToString & "," & followUpBoxSeqNo & "," & receptionResult & "]")

        Using selectedSeriesDataTable As New ActivityInfoDataSet.ActivityInfoGetSelectedSeriesListFromDataTable

            Dim setParamRow As ActivityInfoDataSet.ActivityInfoGetSelectedSeriesListFromRow = _
                selectedSeriesDataTable.NewActivityInfoGetSelectedSeriesListFromRow

            '引数の情報をセット
            setParamRow.DLRCD = context.DlrCD
            setParamRow.STRCD = context.BrnCD
            setParamRow.FLLWUPBOX_SEQNO = followUpBoxSeqNo
            setParamRow.CNTCD = EnvironmentSetting.CountryCode
            selectedSeriesDataTable.AddActivityInfoGetSelectedSeriesListFromRow(setParamRow)

            If String.Equals(receptionResult, "0") Then
                ' Logger.Debug("GetSeriesList_001 receptionResult = 0")

                '希望車種取得
                ' Logger.Debug("GetSeriesList_End Ret[ActivityInfoBusinessLogic.GetSelectedSeriesList]")
                Return ActivityInfoBusinessLogic.GetSelectedSeriesList(selectedSeriesDataTable)

            Else
                ' Logger.Debug("GetSeriesList_002 receptionResult <> 0")

                '成約車種取得
                ' Logger.Debug("GetSeriesList_End Ret[ActivityInfoBusinessLogic.GetSuccessSeriesList]")
                Return ActivityInfoBusinessLogic.GetSuccessSeriesList(selectedSeriesDataTable)
            End If
        End Using
    End Function


#End Region

#Region "プロセス情報取得"

    ''' <summary>
    ''' プロセス情報取得
    ''' </summary>
    ''' <param name="context">スタッフコンテキスト</param>
    ''' <param name="followUpBoxSeqNo">Follow-UpBox内連番</param>
    ''' <param name="contractNo">契約書No</param>
    ''' <returns>各プロセス情報データテーブル</returns>
    ''' <remarks></remarks>
    Private Function GetProcess(ByVal context As StaffContext, _
                                ByVal followUpBoxSeqNo As String, _
                                ByVal contractNo As String) _
                                As ActivityInfoDataSet.ActivityInfoGetProcessToDataTable

        ' Logger.Debug("GetProcess_Start " & _
        '            "Param[" & context.ToString & "," & followUpBoxSeqNo & "," & contractNo & "]")

        Using processDataTable As New ActivityInfoDataSet.ActivityInfoGetProcessFromDataTable

            Dim setParamRow As ActivityInfoDataSet.ActivityInfoGetProcessFromRow = _
                processDataTable.NewActivityInfoGetProcessFromRow

            '引数の情報をセット
            setParamRow.DLRCD = context.DlrCD
            setParamRow.STRCD = context.BrnCD
            setParamRow.FLLWUPBOX_SEQNO = followUpBoxSeqNo
            setParamRow.SALESBKGNO = contractNo
            processDataTable.AddActivityInfoGetProcessFromRow(setParamRow)

            'プロセス情報返却
            ' Logger.Debug("GetProcess_End Ret[ActivityInfoBusinessLogic.GetProcess]")
            Return ActivityInfoBusinessLogic.GetProcess(processDataTable)

        End Using

    End Function

    ''' <summary>
    ''' プロセス情報設定
    ''' </summary>
    ''' <param name="receptionResult">受注前後判定</param>
    ''' <remarks>プロセス情報がなかったときに表示</remarks>
    Private Sub SetProcessDefaultWord(ByVal receptionResult As String)
        ' Logger.Debug("SetProcessDefaultWord_Start " & _
        '            "Param[" & receptionResult & "]")

        Logger.Info("SetProcessDefaultWord_001 " & "Call_Start MyBase.GetValue Param[" & _
           ScreenPos.Current & "," & SessionKeyWordDictionary & "," & False & "]")
        Dim wordDictionary As Dictionary(Of Decimal, String) = _
         CType(MyBase.GetValue(ScreenPos.Current, SessionKeyWordDictionary, False), Dictionary(Of Decimal, String))
        Logger.Info("SetProcessDefaultWord_001 " & "Call_End MyBase.GetValue Ret[" & wordDictionary.ToString() & "]")

        If String.Equals(receptionResult, "0") Then
            ' Logger.Debug("SetProcessDefaultWord_002 receptionResult = 0")

            StaffDetailProcess1.InnerText = Server.HtmlEncode(wordDictionary(24))
            StaffDetailProcess1.Attributes("class") = "Icn1"
            StaffDetailProcess2.InnerText = Server.HtmlEncode(wordDictionary(25))
            StaffDetailProcess2.Attributes("class") = "Icn2Off"
            StaffDetailProcess3.InnerText = Server.HtmlEncode(wordDictionary(26))
            StaffDetailProcess3.Attributes("class") = "Icn3"
            StaffDetailProcess4.InnerText = Server.HtmlEncode(wordDictionary(27))
            StaffDetailProcess4.Attributes("class") = "Icn4"
        Else
            ' Logger.Debug("SetProcessDefaultWord_003 receptionResult <> 0")

            StaffDetailProcess1.InnerText = Server.HtmlEncode(wordDictionary(28))
            StaffDetailProcess1.Attributes("class") = "Icn6Off"
            StaffDetailProcess2.InnerText = Server.HtmlEncode(wordDictionary(29))
            StaffDetailProcess2.Attributes("class") = "Icn7Off"
            StaffDetailProcess3.InnerText = Server.HtmlEncode(wordDictionary(30))
            StaffDetailProcess3.Attributes("class") = "Icn8Off"
            StaffDetailProcess4.InnerText = Server.HtmlEncode(wordDictionary(31))
            StaffDetailProcess4.Attributes("class") = "Icn9Off"
        End If

        ' Logger.Debug("SetProcessDefaultWord_End")

    End Sub

    ''' <summary>
    ''' プロセス情報設定
    ''' </summary>
    ''' <param name="receptionResult">受注前後判定</param>
    ''' <param name="processDataRow">プロセス情報</param>
    ''' <remarks>受注前の状態のプロセスを表示</remarks>
    Private Sub SetProcessBeforeWord(ByVal receptionResult As String, _
                                     ByVal processDataRow As ActivityInfoDataSet.ActivityInfoGetProcessToRow)

        ' Logger.Debug("SetProcessBeforeWord_Start " & _
        '           "Param[" & receptionResult & "," & processDataRow.ToString & "]")

        Logger.Info("SetProcessBeforeWord_001 " & "Call_Start MyBase.GetValue Param[" & _
           ScreenPos.Current & "," & SessionKeyWordDictionary & "," & False & "]")
        Dim wordDictionary As Dictionary(Of Decimal, String) = _
         CType(MyBase.GetValue(ScreenPos.Current, SessionKeyWordDictionary, False), Dictionary(Of Decimal, String))
        Logger.Info("SetProcessBeforeWord_001 " & "Call_End MyBase.GetValue Ret[" & wordDictionary.ToString() & "]")

        'CATALOGDATE:カタログ実施日
        StaffDetailProcess1.InnerText = If(processDataRow.IsCATALOGDATENull(), Server.HtmlEncode(wordDictionary(24)), processDataRow.CATALOGDATE)
        StaffDetailProcess1.Attributes("class") = If(processDataRow.IsCATALOGDATENull(), "Icn1", "Icn1On")

        'TESTDRIVEDATE:試乗実施日
        StaffDetailProcess2.InnerText = If(processDataRow.IsTESTDRIVEDATENull(), Server.HtmlEncode(wordDictionary(25)), processDataRow.TESTDRIVEDATE)
        StaffDetailProcess2.Attributes("class") = If(processDataRow.IsTESTDRIVEDATENull(), "Icn2Off", "Icn2")

        'EVALUATIONDATE:査定実施日
        StaffDetailProcess3.InnerText = If(processDataRow.IsEVALUATIONDATENull(), Server.HtmlEncode(wordDictionary(26)), processDataRow.EVALUATIONDATE)
        StaffDetailProcess3.Attributes("class") = If(processDataRow.IsEVALUATIONDATENull(), "Icn3", "Icn3On")

        'QUOTATIONDATE:見積実施日
        StaffDetailProcess4.InnerText = If(processDataRow.IsQUOTATIONDATENull(), Server.HtmlEncode(wordDictionary(27)), processDataRow.QUOTATIONDATE)
        StaffDetailProcess4.Attributes("class") = If(processDataRow.IsQUOTATIONDATENull(), "Icn4", "Icn4On")

        ' Logger.Debug("SetProcessBeforeWord_End")
    End Sub

    ''' <summary>
    ''' プロセス情報設定
    ''' </summary>
    ''' <param name="receptionResult">受注前後判定</param>
    ''' <param name="processDataRow">プロセス情報</param>
    ''' <remarks>受注後の状態のプロセスを表示</remarks>
    Private Sub SetProcessAfterWord(ByVal receptionResult As String, _
                                     ByVal processDataRow As ActivityInfoDataSet.ActivityInfoGetProcessToRow)

        ' Logger.Debug("SetProcessAfterWord_Start " & _
        '   "Param[" & receptionResult & "," & processDataRow.ToString & "]")

        Logger.Info("SetProcessAfterWord_001 " & "Call_Start MyBase.GetValue Param[" & _
           ScreenPos.Current & "," & SessionKeyWordDictionary & "," & False & "]")
        Dim wordDictionary As Dictionary(Of Decimal, String) = _
         CType(MyBase.GetValue(ScreenPos.Current, SessionKeyWordDictionary, False), Dictionary(Of Decimal, String))
        Logger.Info("SetProcessAfterWord_001 " & "Call_End MyBase.GetValue Ret[" & wordDictionary.ToString() & "]")

        'SALESBKGDATE:受注日
        StaffDetailProcess1.InnerText = If(IsDBNull(processDataRow.SALESBKGDATE) OrElse processDataRow.IsSALESBKGDATENull(), Server.HtmlEncode(wordDictionary(28)), processDataRow.SALESBKGDATE)
        StaffDetailProcess1.Attributes("class") = If(IsDBNull(processDataRow.SALESBKGDATE) OrElse processDataRow.IsSALESBKGDATENull(), "Icn6Off", "Icn6On")

        'VCLASIDATE:振当日
        StaffDetailProcess2.InnerText = If(processDataRow.IsVCLASIDATENull(), Server.HtmlEncode(wordDictionary(29)), processDataRow.VCLASIDATE)
        StaffDetailProcess2.Attributes("class") = If(processDataRow.IsVCLASIDATENull(), "Icn7Off", "Icn7On")

        'SALESDATE:入金日
        StaffDetailProcess3.InnerText = If(processDataRow.IsSALESDATENull(), Server.HtmlEncode(wordDictionary(30)), processDataRow.SALESDATE)
        StaffDetailProcess3.Attributes("class") = If(processDataRow.IsSALESDATENull(), "Icn8Off", "Icn8On")

        'VCLDELIDATE:納車日
        StaffDetailProcess4.InnerText = If(processDataRow.IsVCLDELIDATENull(), Server.HtmlEncode(wordDictionary(31)), processDataRow.VCLDELIDATE)
        StaffDetailProcess4.Attributes("class") = If(processDataRow.IsVCLDELIDATENull(), "Icn9Off", "Icn9On")

        ' Logger.Debug("SetProcessAfterWord_End")

    End Sub

#End Region

#Region "ステータスの取得"

    ''' <summary>
    ''' ステータスの取得
    ''' </summary>
    ''' <param name="context">スタッフコンテキスト</param>
    ''' <param name="followUpBoxSeqNo">Follow-UpBox内連番</param>
    ''' <returns>ステータス情報テーブル</returns>
    ''' <remarks></remarks>
    Private Function GetStatus(ByVal context As StaffContext, _
                                ByVal followUpBoxSeqNo As String) _
                               As ActivityInfoDataSet.ActivityInfoGetStatusToDataTable

        ' Logger.Debug("GetStatus_Start " & _
        '           "Param[" & context.ToString & "," & followUpBoxSeqNo & "]")

        Using statusDataTable As New ActivityInfoDataSet.ActivityInfoGetStatusFromDataTable

            Dim setParamRow As ActivityInfoDataSet.ActivityInfoGetStatusFromRow = _
                statusDataTable.NewActivityInfoGetStatusFromRow

            '引数の情報をセット
            setParamRow.DLRCD = context.DlrCD
            setParamRow.STRCD = context.BrnCD
            setParamRow.FLLWUPBOX_SEQNO = followUpBoxSeqNo
            statusDataTable.AddActivityInfoGetStatusFromRow(setParamRow)

            'ステータス情報返却
            ' Logger.Debug("GetStatus_End Ret[ActivityInfoBusinessLogic.GetStatus]")
            Return ActivityInfoBusinessLogic.GetStatus(statusDataTable)

        End Using

    End Function

#End Region

#End Region

#Region "文字列の加工"

    ''' <summary>
    ''' 文字列の加工
    ''' </summary>
    ''' <param name="target">対象文字列</param>
    ''' <param name="length">指定文字数</param>
    ''' <param name="kind">種類</param>
    ''' <returns>加工後文字列</returns>
    ''' <remarks></remarks>
    Private Function ChangeString(ByVal target As String, _
                                  ByVal length As Integer, _
                                  ByVal kind As String) As String

        ' Logger.Debug(New StringBuilder("ChangeString_Start Param[").Append(target).Append( _
        '            ", ").Append(length).Append(", ").Append(kind).Append("]").ToString)

        '空白の値は"-"を返す
        If String.IsNullOrEmpty(target) Then

            ' Logger.Debug("ChangeString_001")

            ' Logger.Debug("ChangeString_End Ret[" & DataNull & "]")

            Return DataNull

        End If

        ' Logger.Debug("ChangeString_002")

        '空白のみの場合は"-"を返す
        If String.IsNullOrEmpty(target.Trim()) Then

            ' Logger.Debug("ChangeString_003")

            ' Logger.Debug("ChangeString_End Ret[" & DataNull & "]")

            Return DataNull

        End If

        ' Logger.Debug("ChangeString_004")

        Dim resultTarget As String = String.Empty

        resultTarget = Server.HtmlDecode(target)

        If length < resultTarget.Length Then

            ' Logger.Debug("ChangeString_005")

            '文字列の加工
            ' 「...」表示はスタイルシートで行うため文字列カットをしない
            If StringCut.Equals(kind) Then

                ' Logger.Debug("ChangeString_006")

                resultTarget = Left(resultTarget, length)

            End If

        End If

        ' Logger.Debug("ChangeString_End Ret[" & resultTarget & "]")

        Return Server.HtmlEncode(resultTarget)

    End Function

#End Region

#End Region

End Class

