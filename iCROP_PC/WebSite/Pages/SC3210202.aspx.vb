'━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
'SC3210202.aspx.vb
'──────────────────────────────────
'機能： ショールームステータスビジュアライゼーション
'補足： 
'作成： 2012/02/06 KN m.okamura
'更新： 2012/08/28 TMEJ m.okamura 新車受付機能改善 $01
'更新： 2013/01/18 TMEJ m.asano  【問連】GTMC121225110 対応 $02
'──────────────────────────────────

Imports Toyota.eCRB.SystemFrameworks.Web
Imports Toyota.eCRB.SystemFrameworks.Core
Imports Toyota.eCRB.SystemFrameworks.Core.iCROP.BizLogic
Imports Toyota.eCRB.SystemFrameworks.Core.iCROP.DataAccess
Imports Toyota.eCRB.SystemFrameworks.Web.Controls
Imports Toyota.eCRB.Visit.Api.BizLogic
Imports Toyota.eCRB.Visit.Api.DataAccess.VisitReceptionDataSet
Imports System.Data
Imports System.Globalization
Imports System.Web.Services
Imports System.Web.Script.Serialization

''' <summary>
''' ショールームステータスビジュアライゼーション(サブエリア)
''' </summary>
''' <remarks></remarks>
Partial Class PagesSC3210202
    Inherits BasePage

#Region "非公開定数"

    ''' <summary>
    ''' スタッフステータス(商談中)
    ''' </summary>
    ''' <remarks></remarks>
    Private Const StaffStatusNego As String = "2"

    ''' <summary>
    ''' スタッフステータス(スタンバイ)
    ''' </summary>
    ''' <remarks></remarks>
    Private Const StaffStatusStanby As String = "1"

    ''' <summary>
    ''' スタッフステータス(一時退席)
    ''' </summary>
    ''' <remarks></remarks>
    Private Const StaffStatusReave As String = "3"

    ''' <summary>
    ''' スタッフステータス(オフライン)
    ''' </summary>
    ''' <remarks></remarks>
    Private Const StaffStatusOffLine As String = "4"

    ''' <summary>
    ''' デフォルトアイコン(顧客、スタッフ)のファイルパス
    ''' </summary>
    ''' <remarks></remarks>
    Private Const DefaultIcon As String = "../Styles/Images/VisitCommon/silhouette_person01.png"

    ''' <summary>
    ''' スタッフ写真用パスの先頭に設定する文字列
    ''' </summary>
    ''' <remarks></remarks>
    Private Const StaffPhotoPathPrefix As String = "~/"

    ''' <summary>
    ''' 来店状況プレフィックス
    ''' </summary>
    ''' <remarks></remarks>
    Private Const IdPrefixVisit As String = "Vis"

    ''' <summary>
    ''' 待ち状況プレフィックス
    ''' </summary>
    ''' <remarks></remarks>
    Private Const IdPrefixWait As String = "Wait"

    ''' <summary>
    ''' 来店手段（車）
    ''' </summary>
    ''' <remarks></remarks>
    Private Const VisitMeansCar As String = "1"

    ''' <summary>
    ''' 来店手段（歩き）
    ''' </summary>
    ''' <remarks></remarks>
    Private Const VisitMeansWalk As String = "2"

    ''' <summary>
    ''' 来店実績ステータス(フリー)
    ''' </summary>
    ''' <remarks></remarks>
    Private Const VisitStatusFree As String = "01"

    ''' <summary>
    ''' 来店実績ステータス(フリーブロードキャスト)
    ''' </summary>
    ''' <remarks></remarks>
    Private Const VisitStatusFreeBroadcast As String = "02"

    ''' <summary>
    ''' 来店実績ステータス(調整中)
    ''' </summary>
    ''' <remarks></remarks>
    Private Const VisitStatusAdjust As String = "03"

    ''' <summary>
    ''' 来店実績ステータス(確定ブロードキャスト)
    ''' </summary>
    ''' <remarks></remarks>
    Private Const VisitStatusCommitBroadcast As String = "04"

    ''' <summary>
    ''' 来店実績ステータス(確定)
    ''' </summary>
    ''' <remarks></remarks>
    Private Const VisitStatusCommit As String = "05"

    ''' <summary>
    ''' 来店実績ステータス(待ち)
    ''' </summary>
    ''' <remarks></remarks>
    Private Const VisitStatusWait As String = "06"

    ' $01 start 複数顧客に対する商談平行対応
    ''' <summary>
    ''' 来店実績ステータス（商談中断）
    ''' </summary>
    ''' <remarks></remarks>
    Private Const VisitStatusNegotiateStop As String = "09"
    ' $01 end   複数顧客に対する商談平行対応

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
    ''' 正常
    ''' </summary>
    ''' <remarks></remarks>
    Private Const MessageIdNormal As Integer = 0

    ''' <summary>
    ''' 敬称位置(前)
    ''' </summary>
    ''' <remarks></remarks>
    Private Const NameTitlePositionFront As String = "1"

    ''' <summary>
    ''' 敬称位置(後)
    ''' </summary>
    ''' <remarks></remarks>
    Private Const NameTitlePositionBack As String = "2"

    ''' <summary>
    ''' 受付メイン画面ID
    ''' </summary>
    ''' <remarks></remarks>
    Private Const ReceptionistId As String = "SC3210201"

    ' $01 start 複数顧客に対する商談平行対応
    ''' <summary>
    ''' 時間の値が存在しない場合の設定値
    ''' </summary>
    ''' <remarks></remarks>
    Private Const NothingDate As String = "--:--"
    ' $01 end   複数顧客に対する商談平行対応

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
    ''' セッションキー(苦情情報日数)
    ''' </summary>
    ''' <remarks></remarks>
    Private Const SessionKeyComplaintDateCount As String = "complaintDateCount"

#End Region

#Region "文字列の表示制限数"

    ''' <summary>
    ''' アンドンのヘッダー文字数
    ''' </summary>
    ''' <remarks></remarks>
    Private Const BoardHeaderSize As Integer = 8

    ''' <summary>
    ''' スタッフ状況の人名の文字数
    ''' </summary>
    ''' <remarks></remarks>
    Private Const StaffSituationHumanNameSize As Integer = 5

    ''' <summary>
    ''' スタッフステータスの文字数
    ''' </summary>
    ''' <remarks></remarks>
    Private Const StaffStatusSize As Integer = 9

    ''' <summary>
    ''' 来店状況項目名の文字数
    ''' </summary>
    ''' <remarks></remarks>
    Private Const VistorSituationTitleSize As Integer = 16

    ''' <summary>
    ''' 来店状況顧客名の文字数
    ''' </summary>
    ''' <remarks></remarks>
    Private Const VistorSituationCustomerNameSize As Integer = 5

    ''' <summary>
    ''' 来店状況スタッフ名の文字数
    ''' </summary>
    ''' <remarks></remarks>
    Private Const VistorSituationStaffNameSize As Integer = 3

    ''' <summary>
    ''' 来店状況顧客人数の文字数
    ''' </summary>
    ''' <remarks></remarks>
    Private Const VistorSituationCustomerNumSize As Integer = 4

    ''' <summary>
    ''' 車両登録Noの文字数
    ''' </summary>
    ''' <remarks></remarks>
    Private Const VehicleRegistrationSize As Integer = 6

    ''' <summary>
    ''' 削除ボタンの文字数
    ''' </summary>
    ''' <remarks></remarks>
    Private Const DeleteButtonSize As Integer = 4

    ''' <summary>
    ''' 来店時間の文字数
    ''' </summary>
    ''' <remarks></remarks>
    Private Const VisitTimeStampSize As Integer = 5

    ''' <summary>
    ''' 商談テーブルNoの文字数
    ''' </summary>
    ''' <remarks></remarks>
    Private Const SalesTableNoSize As Integer = 2

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
        ' $02 【問連】GTMC121225110 対応 Start
        ' 号口環境ではErrorレベルのログしか出力されない為、Errorレベルで記載
        Logger.Error("DebugLog SC3210202 Page_Load_Start Param[" & sender.ToString & "," & e.ToString & "]")
        ' $02 【問連】GTMC121225110 対応 End

        If Not Me.IsPostBack Then

            ' Logger.Debug("Page_Load_Start_001" & "Not PostBack")

            'ログインユーザの情報を格納
            ' Logger.Debug("Page_Load_Start_002" & "Call_Start StaffContext.Current")
            Dim context As StaffContext = StaffContext.Current
            ' Logger.Debug("Page_Load_Start_002" & "Call_End StaffContext.Current Ret[" & context.ToString & "]")

            '来店状況ステータスリスト
            Dim visitStatusList As List(Of String) = New List(Of String)
            visitStatusList.Add(VisitStatusFree)
            visitStatusList.Add(VisitStatusFreeBroadcast)
            visitStatusList.Add(VisitStatusAdjust)
            visitStatusList.Add(VisitStatusCommitBroadcast)
            visitStatusList.Add(VisitStatusCommit)

            '待ち状況ステータスリスト
            Dim waitStatusList As List(Of String) = New List(Of String)
            waitStatusList.Add(VisitStatusWait)
            ' $01 start 複数顧客に対する商談平行対応
            waitStatusList.Add(VisitStatusNegotiateStop)
            ' $01 end   複数顧客に対する商談平行対応

            '現在日時 基盤より取得
            ' Logger.Debug("Page_Load_003" & "Call_Start DateTimeFunc.Now Param[" & context.DlrCD & "]")
            Dim nowDate As Date = DateTimeFunc.Now(context.DlrCD)
            ' Logger.Debug("Page_Load_003" & "Call_End DateTimeFunc.Now Ret[" & nowDate & "]")

            ' 文言管理
            Logger.Info("Page_Load_Start_004" & "Call_Start MyBase.GetValue Param[" & _
                         ScreenPos.Current & "," & SessionKeyWordDictionary & "," & False & "]")
            Dim wordDictionary As Dictionary(Of Decimal, String) = _
                         CType(MyBase.GetValue(ScreenPos.Current, SessionKeyWordDictionary, False), Dictionary(Of Decimal, String))
            Logger.Info("Page_Load_Start_004" & "Call_End MyBase.GetValue Ret[" & wordDictionary.ToString() & "]")

            '敬称の前後位置
            Logger.Info("Page_Load_Start_005" & "Call_Start MyBase.GetValue Param[" & _
                         ScreenPos.Current & "," & SessionKeyNameTitlePos & "," & False & "]")
            Dim nameTitlePos As String = CType(MyBase.GetValue(ScreenPos.Current, SessionKeyNameTitlePos, False), String)
            Logger.Info("Page_Load_Start_005" & "Call_End MyBase.GetValue Ret[" & nameTitlePos & "]")

            '顧客写真用のパスを取得
            Logger.Info("Page_Load_Start_006" & "Call_Start MyBase.GetValue Param[" & _
                         ScreenPos.Current & "," & SessionKeyFacePicPath & "," & False & "]")
            Dim facePicPath As String = CType(MyBase.GetValue(ScreenPos.Current, SessionKeyFacePicPath, False), String)
            Logger.Info("Page_Load_Start_006" & "Call_End MyBase.GetValue Ret[" & facePicPath & "]")

            'スタッフ写真用のパスを取得
            Logger.Info("Page_Load_Start_007" & "Call_Start MyBase.GetValue Param[" & _
                         ScreenPos.Current & "," & SessionKeyStaffPhotoPath & "," & False & "]")
            Dim staffPhotoPath As String = CType(MyBase.GetValue(ScreenPos.Current, SessionKeyStaffPhotoPath, False), String)
            Logger.Info("Page_Load_Start_007" & "Call_End MyBase.GetValue Ret[" & staffPhotoPath & "]")

            '文言初期化処理
            Me.InitWord(wordDictionary)
            'アンドン初期表示
            Me.InitBoard(nowDate, context)

            '苦情情報取得
            Logger.Info("Page_Load_Start_008 " & "Call_Start MyBase.GetValue Param[" & _
                   ScreenPos.Current & "," & SessionKeyComplaintDateCount & "," & False & "]")
            Dim complaintDateCount As Integer = CType(MyBase.GetValue(ScreenPos.Current, SessionKeyComplaintDateCount, False), Integer)
            Logger.Info("Page_Load_Start_008 " & "Call_End MyBase.GetValue Ret[" & complaintDateCount & "]")

            Dim claimVisitSequenceList As List(Of Long) = Nothing
            Dim businessLogic As New VisitReceptionBusinessLogic
            claimVisitSequenceList = businessLogic.GetClaimInfo(context.DlrCD, _
                                                            context.BrnCD, _
                                                            nowDate, _
                                                            complaintDateCount)
            businessLogic = Nothing

            InitStaff(nowDate, context, nameTitlePos, facePicPath, wordDictionary, staffPhotoPath, claimVisitSequenceList)
            InitCustomer(IdPrefixVisit, VisitRepeater, visitStatusList, nowDate, context, nameTitlePos, wordDictionary, staffPhotoPath, claimVisitSequenceList)
            InitCustomer(IdPrefixWait, WaitRepeater, waitStatusList, nowDate, context, nameTitlePos, wordDictionary, staffPhotoPath, claimVisitSequenceList)

        End If

        ' $02 【問連】GTMC121225110 対応 Start
        ' 号口環境ではErrorレベルのログしか出力されない為、Errorレベルで記載
        Logger.Error("DebugLog SC3210202 Page_Load_End")
        ' $02 【問連】GTMC121225110 対応 End

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

        ' Logger.Debug("InitWord_Start ")

        VisitLiteral.Text = ChangeString(wordDictionary(1), BoardHeaderSize, StringCut)
        WaitLiteral.Text = ChangeString(wordDictionary(2), BoardHeaderSize, StringCut)
        AssessmentLiteral.Text = ChangeString(wordDictionary(3), BoardHeaderSize, StringCut)
        TestCarLiteral.Text = ChangeString(wordDictionary(4), BoardHeaderSize, StringCut)
        PriceConsultationLiteral.Text = ChangeString(wordDictionary(5), BoardHeaderSize, StringCut)
        HelpLiteral.Text = ChangeString(wordDictionary(6), BoardHeaderSize, StringCut)
        PersonUnitLiteral.Text = Server.HtmlEncode(wordDictionary(7))
        CarUnitLiteral.Text = Server.HtmlEncode(wordDictionary(8))
        StaffTitleLiteral.Text = Server.HtmlEncode(wordDictionary(9))
        VisitTitleLiteral.Text = ChangeString(wordDictionary(15), VistorSituationTitleSize, StringCut)
        WaitTitleLiteral.Text = ChangeString(wordDictionary(16), VistorSituationTitleSize, StringCut)

        ' Logger.Debug("InitWord_End")

    End Sub

#End Region

#Region "アンドン初期表示"

    ''' <summary>
    ''' アンドン初期表示
    ''' </summary>
    ''' <param name="nowDate">現在日時</param>
    ''' <param name="context">ログイン情報</param>
    ''' <remarks></remarks>
    Private Sub InitBoard(ByVal nowDate As Date, ByVal context As StaffContext)

        ' Logger.Debug(New StringBuilder("InitBoard_Start Param[").Append(nowDate).Append( _
        '            ", ").Append(context.ToString).Append("]").ToString)

        'アンドン情報取得
        Dim boardDataTable As VisitReceptionBoardInfoDataTable = Nothing
        Dim businessLogic As New VisitReceptionBusinessLogic
        boardDataTable = businessLogic.GetBoardInfo(context.DlrCD, context.BrnCD, nowDate)
        businessLogic = Nothing
        Dim boardDataRow As VisitReceptionBoardInfoRow = Nothing
        boardDataRow = boardDataTable.Rows(0)

        ' 情報が存在しない場合は処理しない
        If boardDataTable.Rows.Count = 0 Then

            ' Logger.Debug("InitBoard_001")
            ' Logger.Debug("InitBoard_End")

            Exit Sub

        End If

        ' Logger.Debug("InitBoard_002")

        'アンドンデータ表示
        BoardResultNumber.Text = boardDataRow.RESULTCOUNT
        BoardAgreeNumber.Text = boardDataRow.CONCLUSIONCOUNT

        ' Logger.Debug("InitBoard_End")

    End Sub

#End Region

#Region "スタッフ状況初期表示"

#Region "スタッフ状況初期表示(全体)"

    ''' <summary>
    ''' スタッフ状況初期表示
    ''' </summary>
    ''' <param name="nowDate">現在日時</param>
    ''' <param name="context">ログイン情報</param>
    ''' <param name="nameTitlePos">敬称位置</param>
    ''' <param name="facePicPath">画像パス</param>
    ''' <param name="wordDictionary">文言リスト</param>
    ''' <param name="staffPhotoPath">スタッフ画像パス</param>
    ''' <param name="claimVisitSequenceList">クレーム情報一覧</param>
    ''' <remarks></remarks>
    Private Sub InitStaff(ByVal nowDate As Date, ByVal context As StaffContext, _
                          ByVal nameTitlePos As String, ByVal facePicPath As String, _
                          ByVal wordDictionary As Dictionary(Of Decimal, String), _
                          ByVal staffPhotoPath As String, _
                          ByVal claimVisitSequenceList As List(Of Long))

        ' Logger.Debug("InitStaff_Start Param[" & nameTitlePos & "," & facePicPath & "]")

        'スタッフ情報取得
        Dim staffDataTable As VisitReceptionStaffSituationDataTable = Nothing
        Dim businessLogic As New VisitReceptionBusinessLogic
        staffDataTable = businessLogic.GetStaffSituationInfo(context.DlrCD, context.BrnCD, nowDate, claimVisitSequenceList)
        businessLogic = Nothing

        ' 情報が存在しない場合は処理しない
        If staffDataTable.Rows.Count = 0 Then

            ' Logger.Debug("InitStaff_001")
            ' Logger.Debug("InitStaff_End")

            Exit Sub

        End If

        ' Logger.Debug("InitStaff_002")

        '商談開始経過のリストを設定する
        SalesStartTimeList.Value = GetTimeSpanListString(staffDataTable, "SALESSTART", nowDate)
        '通知送信日時のリストを設定する(査定依頼)
        RequestAssessmentTimeDateList.Value = GetTimeSpanListString(staffDataTable, "REQUESTASSESSMENTDATE", nowDate)
        '通知送信日時のリストを設定する(価格相談依頼)
        RequestPriceConsultationTimeDateList.Value = GetTimeSpanListString(staffDataTable, "REQUESTPRICECONSULTATIONDATE", nowDate)
        '通知送信日時のリストを設定する(ヘルプ依頼)
        RequestHelpTimeDateList.Value = GetTimeSpanListString(staffDataTable, "REQUESTHELPDATE", nowDate)

        ' コントロールにバインドする
        StaffRepeater.DataSource = staffDataTable
        StaffRepeater.DataBind()

        ' アンドン情報(依頼情報)の初期化
        Dim RequestAssessmentCount = 0
        Dim RequestPriceConsultationCount = 0
        Dim RequestHelpCount = 0

        ' -----------------------------------------------------
        ' データを設定する
        ' -----------------------------------------------------
        For i = 0 To StaffRepeater.Items.Count - 1

            Dim staff As Control = StaffRepeater.Items(i)

            Dim staffStatusData As String = CType(staff.FindControl("StaffStatus"), HiddenField).Value

            '----------------------------------------------------------------------
            ' メインエリア
            '----------------------------------------------------------------------
            Select Case staffStatusData

                Case StaffStatusNego
                    ' Logger.Debug("InitStaff_003")
                    '商談中
                    CType(staff.FindControl("MainNegoLiteral"), Literal).Visible = True

                Case StaffStatusStanby
                    ' Logger.Debug("InitStaff_004")
                    'スタンバイ
                    CType(staff.FindControl("MainStandbyLiteral"), Literal).Visible = True

                Case StaffStatusReave
                    ' Logger.Debug("InitStaff_005")
                    '一時退席
                    CType(staff.FindControl("MainLeavingLiteral"), Literal).Visible = True

                Case StaffStatusOffLine
                    ' Logger.Debug("InitStaff_006")
                    'オフライン
                    CType(staff.FindControl("MainOfflineLiteral"), Literal).Visible = True
                    staff.FindControl("OfflineCoverDiv").Visible = True

            End Select

            '----------------------------------------------------------------------
            ' 上段
            '----------------------------------------------------------------------
            ' スタッフエリア
            InitStaffAboveArea(staff, staffDataTable.Rows(i), nameTitlePos, facePicPath, staffPhotoPath, wordDictionary)

            '----------------------------------------------------------------------
            ' 下段
            '----------------------------------------------------------------------
            InitStaffUnderArea(staff, staffDataTable.Rows(i), wordDictionary)

            If (i + 1) Mod 5 = 0 Then

                ' Logger.Debug("InitStaff_007")
                CType(staff.FindControl("StuffChip"), HtmlGenericControl).Attributes("Class") = "ListRight"

            End If

            '----------------------------------------------------------------------
            ' アンドン表示件数(依頼情報)のカウント
            '----------------------------------------------------------------------
            Dim staffDataRow As VisitReceptionStaffSituationRow = staffDataTable.Rows(i)
            Dim requestAssessmentDateData As String = If(staffDataRow.IsREQUESTASSESSMENTDATENull(), String.Empty, CType(staffDataRow.REQUESTASSESSMENTDATE, String))
            Dim requestPriceConsultationDateData As String = If(staffDataRow.IsREQUESTPRICECONSULTATIONDATENull(), String.Empty, CType(staffDataRow.REQUESTPRICECONSULTATIONDATE, String))
            Dim requestHelpDateData As String = If(staffDataRow.IsREQUESTHELPDATENull(), String.Empty, CType(staffDataRow.REQUESTHELPDATE, String))

            ' 査定依頼がされている場合
            If Not String.IsNullOrEmpty(requestAssessmentDateData) Then
                RequestAssessmentCount = RequestAssessmentCount + 1
            End If

            ' 価格相談依頼がされている場合
            If Not String.IsNullOrEmpty(requestPriceConsultationDateData) Then
                RequestPriceConsultationCount = RequestPriceConsultationCount + 1
            End If

            ' ヘルプ依頼がされている場合
            If Not String.IsNullOrEmpty(requestHelpDateData) Then
                RequestHelpCount = RequestHelpCount + 1
            End If

        Next

        ' アンドン情報(依頼情報)を設定する
        BoardAssessmentNumber.Text = RequestAssessmentCount
        BoardPriceConsultationNumber.Text = RequestPriceConsultationCount
        BoardHelpNumber.Text = RequestHelpCount

        ' Logger.Debug("InitStaff_End")

    End Sub

#End Region

#Region "スタッフステータス上段エリア"

#Region "スタッフステータス上段エリア(全体)"

    ''' <summary>
    ''' スタッフステータス上段エリアの表示
    ''' </summary>
    ''' <param name="staff">スタッフコントロール</param>
    ''' <param name="row">データロウ</param>
    ''' <param name="nameTitlePos">敬称位置</param>
    ''' <param name="facePicPath">画像パス</param>
    ''' <param name="staffPhotoPath">スタッフ画像パス</param>
    ''' <param name="wordDictionary">文言リスト</param>
    ''' <remarks></remarks>
    Private Sub InitStaffAboveArea(ByVal staff As Control, ByVal row As VisitReceptionStaffSituationRow, _
                                   ByVal nameTitlePos As String, _
                                   ByVal facePicPath As String, ByVal staffPhotoPath As String, _
                                   ByVal wordDictionary As Dictionary(Of Decimal, String))

        ' Logger.Debug("InitStaffAboveArea_Start " & "Param[" & "]")

        Dim visitorLinkingCountData As String _
            = If(row.IsVISITORLINKINGCOUNTNull(), String.Empty, CType(row.VISITORLINKINGCOUNT, String))

        ' 紐付け人数
        Dim visitorLinkingCount As Integer = 0

        If Not String.IsNullOrEmpty(visitorLinkingCountData) Then

            ' Logger.Debug("InitStaffAboveArea_001")
            visitorLinkingCount = CType(visitorLinkingCountData, Integer)

        End If

        '左側(顧客)エリアの表示
        InitStaffAboveLeftArea(staff, row, visitorLinkingCount, wordDictionary, nameTitlePos, facePicPath)

        '右側(スタッフ)エリアの表示
        InitStaffAboveRightArea(staff, row, visitorLinkingCount, staffPhotoPath, wordDictionary)

        ' Logger.Debug("InitStaffAboveArea_End")

    End Sub

#End Region

#Region "スタッフステータス上段エリア(左)"

    ''' <summary>
    ''' スタッフステータス上段エリアの左側表示
    ''' </summary>
    ''' <param name="staff">スタッフコントロール</param>
    ''' <param name="row">データロウ</param>
    ''' <param name="visitorLinkingCount">紐付け人数</param>
    ''' <param name="wordDictionary">文言リスト</param>
    ''' <param name="nameTitlePos">敬称位置</param>
    ''' <param name="facePicPath">顔画像パス</param>
    ''' <remarks></remarks>
    Private Sub InitStaffAboveLeftArea(ByVal staff As Control, ByVal row As VisitReceptionStaffSituationRow, _
                                       ByVal visitorLinkingCount As Integer, _
                                       ByVal wordDictionary As Dictionary(Of Decimal, String), _
                                       ByVal nameTitlePos As String, _
                                       ByVal facePicPath As String)

        ' Logger.Debug("InitStaffAboveLeftArea_Start " & "Param[" & "]")

        Dim staffStatusData As String = If(row.IsSTAFFSTATUSNull(), String.Empty, row.STAFFSTATUS)
        Dim custNameData As String = If(row.IsCUSTNAMENull(), String.Empty, row.CUSTNAME)
        Dim custNameTitleData As String = If(row.IsCUSTNAMETITLENull(), String.Empty, row.CUSTNAMETITLE)
        Dim custImageFileData As String = If(row.IsCUSTIMAGEFILENull(), String.Empty, row.CUSTIMAGEFILE)
        Dim resultNumData As String = If(row.IsRESULTCOUNTNull(), String.Empty, CType(row.RESULTCOUNT, String))
        Dim agreeNumData As String = If(row.IsCONCLUSIONCOUNTNull(), String.Empty, CType(row.CONCLUSIONCOUNT, String))
        Dim custSegmentData As String = If(row.IsCUSTSEGMENTNull(), String.Empty, row.CUSTSEGMENT)

        '-------------------------------------------
        ' 左側エリア
        '-------------------------------------------
        ' 商談中の場合、または紐付け人数が存在する場合
        If StaffStatusNego.Equals(staffStatusData) OrElse visitorLinkingCount > 0 Then

            staff.FindControl("CustmerDiv").Visible = True

            If String.IsNullOrEmpty(custImageFileData) OrElse String.IsNullOrEmpty(custImageFileData.Trim()) Then

                ' Logger.Debug("InitStaffAboveLeftArea_001")
                CType(staff.FindControl("CustImageFileImage"), Image).ImageUrl = DefaultIcon

            Else

                ' Logger.Debug("InitStaffAboveLeftArea_002")
                CType(staff.FindControl("CustImageFileImage"), Image).ImageUrl = facePicPath & custImageFileData

            End If

            Dim custName As New StringBuilder

            If Not String.IsNullOrEmpty(custNameData.Trim()) Then

                If String.IsNullOrEmpty(custNameTitleData) OrElse String.IsNullOrEmpty(custNameTitleData.Trim()) Then

                    ' Logger.Debug("InitStaffAboveLeftArea_003")
                    custNameTitleData = String.Empty

                End If

                '敬称を追加
                If NameTitlePositionFront.Equals(nameTitlePos) Then

                    ' Logger.Debug("InitStaffAboveLeftArea_004")
                    custName.Append(custNameTitleData)
                    custName.Append(custNameData)

                Else

                    ' Logger.Debug("InitStaffAboveLeftArea_005")
                    custName.Append(custNameData)
                    custName.Append(custNameTitleData)

                End If

                CType(staff.FindControl("CustNameLiteral"), Literal).Text = _
                    ChangeString(custName.ToString, StaffSituationHumanNameSize, StringAdd)

            Else

                If String.IsNullOrEmpty(custSegmentData) Then

                    ' 新規顧客の場合
                    ' Logger.Debug("InitStaffAboveLeftArea_006")
                    CType(staff.FindControl("CustNameLiteral"), Literal).Text = Server.HtmlEncode(wordDictionary(17))

                Else

                    ' 既存顧客の場合
                    ' Logger.Debug("InitStaffAboveLeftArea_007")
                    CType(staff.FindControl("CustNameLiteral"), Literal).Text = Server.HtmlEncode(wordDictionary(18))

                End If

            End If

        ElseIf StaffStatusStanby.Equals(staffStatusData) OrElse StaffStatusReave.Equals(staffStatusData) Then

            ' Logger.Debug("InitStaffAboveLeftArea_008")
            InitStaffAboveLeftStanbyOrReave(staff, staffStatusData, resultNumData, agreeNumData, wordDictionary)

        ElseIf StaffStatusOffLine.Equals(staffStatusData) Then

            ' 空エリア
            ' Logger.Debug("InitStaffAboveLeftArea_009")
            staff.FindControl("EmptyDiv").Visible = True

        End If

        ' Logger.Debug("InitStaffAboveLeftArea_End")

    End Sub

    ''' <summary>
    ''' スタッフステータス上段エリアの左側表示(オフライン、一時退席)
    ''' </summary>
    ''' <param name="staff">スタッフコントロール</param>
    ''' <param name="staffStatusData"></param>
    ''' <param name="resultNumData"></param>
    ''' <param name="agreeNumData"></param>
    ''' <remarks></remarks>
    Private Sub InitStaffAboveLeftStanbyOrReave(ByVal staff As Control, _
                                                ByVal staffStatusData As String, _
                                                ByVal resultNumData As String, _
                                                ByVal agreeNumData As String, _
                                                ByVal wordDictionary As Dictionary(Of Decimal, String))

        ' Logger.Debug("InitStaffAboveLeftStanbyOrReave_Start")

        '-------------------------------------------
        ' 実績エリア
        '-------------------------------------------
        ' 商談中以外の場合、または紐付け人数が設定されている場合
        If Not StaffStatusNego.Equals(staffStatusData) AndAlso Not staff.FindControl("LinkingCountDiv").Visible Then

            ' 実績エリア
            staff.FindControl("ResultDiv").Visible = True

            If Not String.IsNullOrEmpty(resultNumData) Then

                ' Logger.Debug("InitStaffAboveLeftStanbyOrReave_001")
                CType(staff.FindControl("ResultNumLiteral"), Literal).Text = Server.HtmlEncode(resultNumData + wordDictionary(11))

            End If

            If Not String.IsNullOrEmpty(agreeNumData) Then

                Dim agreNum As Integer = CInt(agreeNumData)

                If agreNum > 3 Then

                    ' Logger.Debug("InitStaffAboveLeftStanbyOrReave_002")
                    CType(staff.FindControl("AgreeNumLiteral"), Literal).Text = Server.HtmlEncode(agreeNumData)

                End If

                For count As Integer = 1 To agreNum

                    If count > 3 Then

                        ' Logger.Debug("InitStaffAboveLeftStanbyOrReave_003")
                        Exit For

                    End If

                    CType(staff.FindControl("Chip" & count), HtmlGenericControl).Visible = True

                Next

            End If

        End If

        ' Logger.Debug("InitStaffAboveLeftStanbyOrReave_End")

    End Sub

#End Region

#Region "スタッフステータス上段エリア(右)"

    ''' <summary>
    ''' スタッフステータス上段エリアの右側表示
    ''' </summary>
    ''' <param name="staff">スタッフコントロール</param>
    ''' <param name="row">データロウ</param>
    ''' <param name="visitorLinkingCount">紐付け人数</param>
    ''' <param name="staffPhotoPath">スタッフ画像パス</param>
    ''' <param name="wordDictionary">文言リスト</param>
    ''' <remarks></remarks>
    Private Sub InitStaffAboveRightArea(ByVal staff As Control, ByVal row As VisitReceptionStaffSituationRow, _
                                        ByVal visitorLinkingCount As Integer, _
                                        ByVal staffPhotoPath As String, ByVal wordDictionary As Dictionary(Of Decimal, String))

        ' Logger.Debug("InitStaffAboveRightArea_Start " & "Param[" & "]")

        Dim staffStatusData As String = If(row.IsSTAFFSTATUSNull(), String.Empty, row.STAFFSTATUS)
        Dim salesTableNoData As String = If(row.IsSALESTABLENONull(), String.Empty, CType(row.SALESTABLENO, String))
        Dim orgImgFileData As String = If(row.IsORG_IMGFILENull(), String.Empty, row.ORG_IMGFILE)
        Dim claimFlgData As String = If(row.IsCLAIMFLGNull(), String.Empty, row.CLAIMFLG)
        Dim requestAssessmentDateData As String _
            = If(row.IsREQUESTASSESSMENTDATENull(), String.Empty, CType(row.REQUESTASSESSMENTDATE, String))
        Dim requestPriceConsultationDateData As String _
            = If(row.IsREQUESTPRICECONSULTATIONDATENull(), String.Empty, CType(row.REQUESTPRICECONSULTATIONDATE, String))
        Dim requestHelpDateData As String = If(row.IsREQUESTHELPDATENull(), String.Empty, CType(row.REQUESTHELPDATE, String))
        Dim userNameData As String = If(row.IsUSERNAMENull(), String.Empty, CType(row.USERNAME, String))

        '-------------------------------------------
        ' スタッフエリア
        '-------------------------------------------
        If String.IsNullOrEmpty(orgImgFileData) OrElse String.IsNullOrEmpty(orgImgFileData.Trim()) Then

            ' Logger.Debug("InitStaffAboveRightArea_001")
            CType(staff.FindControl("OrgImgFileImage"), Image).ImageUrl = DefaultIcon

        Else

            ' Logger.Debug("InitStaffAboveRightArea_002")
            CType(staff.FindControl("OrgImgFileImage"), Image).ImageUrl = StaffPhotoPathPrefix & staffPhotoPath & orgImgFileData

        End If

        If Not String.IsNullOrEmpty(userNameData) Then

            ' Logger.Debug("InitStaffAboveRightArea_003")
            CType(staff.FindControl("UserNameLiteral"), Literal).Text = _
                ChangeString(userNameData, StaffSituationHumanNameSize, StringAdd)

        End If

        '-------------------------------------------
        '商談テーブルエリア
        '-------------------------------------------
        If Not String.IsNullOrEmpty(salesTableNoData) Then

            'グレーエリアも表示可にする
            ' Logger.Debug("InitStaffAboveRightArea_004")
            staff.FindControl("SalesTableNoDiv").Visible = True
            CType(staff.FindControl("SalesTableNoLiteral"), Literal).Text = _
                ChangeString(salesTableNoData, SalesTableNoSize, StringCut)

        End If

        '-------------------------------------------
        ' 苦情情報
        '-------------------------------------------
        If Not String.IsNullOrEmpty(claimFlgData) Then

            'グレーエリアも表示可とする
            ' Logger.Debug("InitStaffAboveRightArea_005")
            staff.FindControl("ClaimIcnDiv").Visible = True
            CType(staff.FindControl("ClaimChar"), Literal).Text = Server.HtmlEncode(wordDictionary(10))

        End If

        '-------------------------------------------
        ' 依頼情報
        '-------------------------------------------
        ' 商談中の場合
        If StaffStatusNego.Equals(staffStatusData) Then

            ' Logger.Debug("InitStaffAboveRightArea_006")

            '通知依頼信号表示
            '査定依頼がされている場合
            If Not String.IsNullOrEmpty(requestAssessmentDateData) Then

                ' Logger.Debug("InitStaffAboveRightArea_007")
                staff.FindControl("AssessmentIconOff").Visible = False
                staff.FindControl("AssessmentIconOn").Visible = True

            End If

            '価格相談依頼がされている場合
            If Not String.IsNullOrEmpty(requestPriceConsultationDateData) Then

                ' Logger.Debug("InitStaffAboveRightArea_008")
                staff.FindControl("PriceIconOff").Visible = False
                staff.FindControl("PriceIconOn").Visible = True

            End If

            'ヘルプ依頼がされている場合
            If Not String.IsNullOrEmpty(requestHelpDateData) Then

                ' Logger.Debug("InitStaffAboveRightArea_009")
                staff.FindControl("HelpIconOff").Visible = False
                staff.FindControl("HelpIconOn").Visible = True

            End If

        End If

        '-------------------------------------------
        ' 紐付け人数
        '-------------------------------------------
        '紐付け人数が存在する場合
        If visitorLinkingCount > 0 Then

            ' Logger.Debug("InitStaffAboveRightArea_010")
            Dim displayVisitorLinkingCount As Integer = visitorLinkingCount

            '商談中以外の場合、既に画面に表示している紐付け情報の分を除く
            If Not StaffStatusNego.Equals(staffStatusData) Then

                ' Logger.Debug("InitStaffAboveRightArea_011")
                displayVisitorLinkingCount = visitorLinkingCount - 1

            End If

            If displayVisitorLinkingCount > 0 Then

                ' Logger.Debug("InitStaffAboveRightArea_012")
                staff.FindControl("LinkingCountDiv").Visible = True
                CType(staff.FindControl("VisitorLinkingCountLiteral"), Literal).Text = displayVisitorLinkingCount

            End If

        End If

        ' Logger.Debug("InitStaffAboveRightArea_End")

    End Sub

#End Region

#End Region

#Region "スタッフステータス下段エリア"

    ''' <summary>
    ''' スタッフステータス下段エリアの表示
    ''' </summary>
    ''' <param name="staff">スタッフ情報</param>
    ''' <param name="row">データロウ</param>
    ''' <param name="wordDictionary">文言リスト</param>
    ''' <remarks></remarks>
    Private Sub InitStaffUnderArea(ByVal staff As Control, ByVal row As VisitReceptionStaffSituationRow, _
                                   ByVal wordDictionary As Dictionary(Of Decimal, String))

        ' Logger.Debug("InitStaffUnderArea_Start " & "Param[" & "]")

        Dim staffStatusData As String = If(row.IsSTAFFSTATUSNull(), String.Empty, row.STAFFSTATUS)

        'スタッフステータスによって表示を変更
        Select Case staffStatusData

            Case StaffStatusNego
                ' Logger.Debug("InitStaffUnderArea_001")
                '商談中
                staff.FindControl("UnderNegoDiv").Visible = True

            Case StaffStatusStanby
                ' Logger.Debug("InitStaffUnderArea_002")
                'スタンバイ
                staff.FindControl("UnderOherDiv").Visible = True
                CType(staff.FindControl("StaffStatusLiteral"), Literal).Text = _
                    ChangeString(wordDictionary(12), StaffStatusSize, StringCut)

            Case StaffStatusReave
                ' Logger.Debug("InitStaffUnderArea_003")
                '一時退席
                staff.FindControl("UnderOherDiv").Visible = True
                CType(staff.FindControl("StaffStatusLiteral"), Literal).Text = _
                    ChangeString(wordDictionary(13), StaffStatusSize, StringCut)

            Case StaffStatusOffLine
                ' Logger.Debug("InitStaffUnderArea_004")
                'オフライン
                staff.FindControl("UnderOherDiv").Visible = True
                CType(staff.FindControl("StaffStatusLiteral"), Literal).Text = _
                    ChangeString(wordDictionary(14), StaffStatusSize, StringCut)

        End Select

        ' Logger.Debug("InitStaffUnderArea_End")

    End Sub

#End Region

#End Region

#Region "来店状況初期表示"

#Region "来店状況初期表示(全体)"

    ''' <summary>
    ''' 来店状況初期表示
    ''' </summary>
    ''' <param name="prefix">コントロールIDのプレフィックス</param>
    ''' <param name="repeater">リピーターコントロール</param>
    ''' <param name="statusList">来店実績ステータスリスト</param>
    ''' <param name="nowDate">現在日時</param>
    ''' <param name="context">ログイン情報</param>
    ''' <param name="nameTitlePos">敬称位置</param>
    ''' <param name="wordDictionary">文言リスト</param>
    ''' <param name="staffPhotoPath">スタッフ画像パス</param>
    ''' <param name="claimVisitSequenceList">クレーム情報一覧</param>
    ''' <remarks></remarks>
    Private Sub InitCustomer(ByVal prefix As String, ByRef repeater As Repeater, _
                             ByVal statusList As List(Of String), ByVal nowDate As Date, _
                             ByVal context As StaffContext, ByVal nameTitlePos As String, _
                             ByVal wordDictionary As Dictionary(Of Decimal, String), ByVal staffPhotoPath As String, _
                             ByVal claimVisitSequenceList As List(Of Long))

        ' Logger.Debug("InitCustomer_Start Param[" & _
        '             prefix & "," & repeater.ToString & "," & statusList.ToString & "," & _
        '             nameTitlePos & "," & facePicPath & "]")

        '来店情報取得
        Dim visitDataTable As VisitReceptionVisitorSituationDataTable = Nothing
        Dim businessLogic As New VisitReceptionBusinessLogic
        visitDataTable = businessLogic.GetVisitorSituationInfo(context.DlrCD, context.BrnCD, _
                                                               statusList, nowDate, claimVisitSequenceList)
        businessLogic = Nothing

        ' アンドン情報を設定する
        If (IdPrefixVisit.Equals(prefix)) Then
            BoardVisitNumber.Text = visitDataTable.Rows.Count
        Else
            BoardWaitNumber.Text = visitDataTable.Rows.Count
        End If

        '情報が存在しない場合は処理しない
        If visitDataTable.Rows.Count = 0 Then

            ' Logger.Debug("InitCustomer_001")
            ' Logger.Debug("InitCustomer_End")

            Exit Sub

        End If

        ' Logger.Debug("InitCustomer_002")

        ' $01 start 複数顧客に対する商談平行対応
        ' 来店経過時間のリストを設定する
        Dim visitTimeSpanListString As String = GetVisitTimeSpanListString(visitDataTable, nowDate)

        ' 来店状況、待ち状況それぞれに値を設定
        If (IdPrefixVisit.Equals(prefix)) Then

            ' Logger.Debug("InitCustomer_003")
            VisitVisitTimeList.Value = visitTimeSpanListString

        Else

            ' Logger.Debug("InitCustomer_004")
            WaitVisitTimeDateList.Value = visitTimeSpanListString

        End If
        ' $01 end   複数顧客に対する商談平行対応

        'コントロールにバインドする
        repeater.DataSource = visitDataTable
        repeater.DataBind()

        ' -----------------------------------------------------
        ' データを設定する
        ' -----------------------------------------------------
        For i = 0 To repeater.Items.Count - 1

            Dim customer As Control = repeater.Items(i)

            ' $01 start 複数顧客に対する商談平行対応
            Dim className As New StringBuilder

            If i < 2 Then

                '来店状況エリアの最上部のチップ
                className.Append("CassetteTop ")

            End If

            If i Mod 2 = 0 Then

                '来店状況エリアの左側のチップ
                ' Logger.Debug("InitCustomer_005")
                className.Append("CassetteLeft")

            Else

                '来店状況エリアの左側のチップ
                ' Logger.Debug("InitCustomer_006")
                className.Append("CassetteRight")

            End If

            CType(customer.FindControl("CustomerChip"), HtmlGenericControl).Attributes("Class") = className.ToString
            ' $01 end   複数顧客に対する商談平行対応

            '----------------------------------------------------------------------
            ' チップの左側
            '----------------------------------------------------------------------
            InitCustomerLeftArea(prefix, customer, visitDataTable.Rows(i), nameTitlePos, wordDictionary)

            '----------------------------------------------------------------------
            ' チップの右側
            '----------------------------------------------------------------------
            InitCustomerRightArea(prefix, customer, visitDataTable.Rows(i), staffPhotoPath, wordDictionary)

        Next

        ' Logger.Debug("InitCustomer_End")

    End Sub

#End Region

#Region "来店状況エリア"

#Region "来店状況エリア(左)"

    ''' <summary>
    ''' 来店状況左側エリアの表示
    ''' </summary>
    ''' <param name="prefix">コントロールIDのプレフィックス</param>
    ''' <param name="customer">顧客情報コントロール</param>
    ''' <param name="row">データロウ</param>
    ''' <param name="nameTitlePos">敬称位置</param>
    ''' <param name="wordDictionary">文言リスト</param>
    ''' <remarks></remarks>
    Private Sub InitCustomerLeftArea(ByVal prefix As String, ByVal customer As Control, _
                                     ByVal row As VisitReceptionVisitorSituationRow, _
                                     ByVal nameTitlePos As String, ByVal wordDictionary As Dictionary(Of Decimal, String))

        ' Logger.Debug("InitCustomerLeftArea_Start")

        Dim visitTimestampData As String = _
            If(row.IsVISITTIMESTAMPNull(), String.Empty, Format(CType(row.VISITTIMESTAMP, DateTime), "yyyy/MM/dd HH:mm:ss"))
        Dim custNameData As String = If(row.IsCUSTNAMENull(), String.Empty, row.CUSTNAME)
        Dim custNameTitleData As String = If(row.IsCUSTNAMETITLENull(), String.Empty, row.CUSTNAMETITLE)
        Dim custSegmentData As String = If(row.IsCUSTSEGMENTNull(), String.Empty, row.CUSTSEGMENT)

        ' $01 start 複数顧客に対する商談平行対応
        ' 来店時間が存在する場合は表示する
        If Not String.IsNullOrEmpty(visitTimestampData) Then
            CType(customer.FindControl(prefix + "VisitStartLiteral"), Literal).Text = _
                ChangeString(Format(CType(visitTimestampData, DateTime), "HH:mm"), VisitTimeStampSize, StringCut)
        Else
            CType(customer.FindControl(prefix + "VisitStartLiteral"), Literal).Text = NothingDate
        End If
        ' $01 end   複数顧客に対する商談平行対応

        '顧客エリア
        If Not String.IsNullOrEmpty(custNameData.Trim()) Then

            ' Logger.Debug("InitCustomerArea_001")

            Dim custName As New StringBuilder

            If String.IsNullOrEmpty(custNameTitleData) OrElse String.IsNullOrEmpty(custNameTitleData.Trim()) Then

                ' Logger.Debug("InitCustomerArea_002")

                custNameTitleData = String.Empty

            End If

            '敬称の前後位置
            If NameTitlePositionFront.Equals(nameTitlePos) Then

                ' Logger.Debug("InitCustomerArea_003")

                custName.Append(custNameTitleData)
                custName.Append(custNameData)

            Else

                ' Logger.Debug("InitCustomerArea_004")

                custName.Append(custNameData)
                custName.Append(custNameTitleData)

            End If

            CType(customer.FindControl(prefix + "CustNameLabel"), Label).Text = _
                ChangeString(custName.ToString, VistorSituationCustomerNameSize, StringAdd)

        Else

            ' Logger.Debug("InitCustomerArea_005")

            If String.IsNullOrEmpty(custSegmentData) Then

                ' Logger.Debug("InitCustomerArea_006")
                ' 新規顧客の場合
                CType(customer.FindControl(prefix + "CustNameLabel"), Label).Text = _
                    Server.HtmlEncode(wordDictionary(17))
            Else

                ' Logger.Debug("InitCustomerArea_007")
                ' 既存顧客の場合
                CType(customer.FindControl(prefix + "CustNameLabel"), Label).Text = _
                    Server.HtmlEncode(wordDictionary(18))

            End If

        End If

        ' Logger.Debug("InitCustomerLeftArea_End")

    End Sub

#End Region

#Region "来店状況エリア(右)"

    ''' <summary>
    ''' 来店状況右側エリアの表示
    ''' </summary>
    ''' <param name="prefix">コントロールIDのプレフィックス</param>
    ''' <param name="customer">顧客情報コントロール</param>
    ''' <param name="row">データロウ</param>
    ''' <param name="staffPhotoPath">スタッフ画像パス</param>
    ''' <param name="wordDictionary">文言リスト</param>
    ''' <remarks></remarks>
    Private Sub InitCustomerRightArea(ByVal prefix As String, ByVal customer As Control, _
                                      ByVal row As VisitReceptionVisitorSituationRow, ByVal staffPhotoPath As String, _
                                      ByVal wordDictionary As Dictionary(Of Decimal, String))

        ' Logger.Debug("InitCustomerRightArea_Start")

        Dim visitStatusData As String = If(row.IsVISITSTATUSNull(), String.Empty, row.VISITSTATUS)
        Dim salesTableNoData As String = If(row.IsSALESTABLENONull(), String.Empty, CType(row.SALESTABLENO, String))
        Dim orgImgfileData As String = If(row.IsORG_IMGFILENull(), String.Empty, row.ORG_IMGFILE)
        Dim custClaimFlg As String = If(row.IsCLAIMFLGNull(), String.Empty, row.CLAIMFLG)

        ' $01 start 複数顧客に対する商談平行対応
        ' 商談中断の場合は水色の枠線を設定
        If IdPrefixWait.Equals(prefix) AndAlso VisitStatusNegotiateStop.Equals(visitStatusData) Then
            CType(customer.FindControl("CustomerChip"), HtmlGenericControl).Attributes("Class") += " StopStore"
        End If
        ' $01 end   複数顧客に対する商談平行対応

        '苦情アイコン表示フラグ
        If Not String.IsNullOrEmpty(custClaimFlg) Then

            ' Logger.Debug("InitCustomerRightArea_001")
            customer.FindControl(prefix + "ClaimIcnDiv").Visible = True
            CType(customer.FindControl(prefix + "ClaimChar"), Literal).Text = Server.HtmlEncode(wordDictionary(10))

        End If

        'スタッフ写真
        If Not String.IsNullOrEmpty(visitStatusData) Then

            ' Logger.Debug("InitCustomerRightArea_002")

            Dim orgImgfileImage As Image = _
                CType(customer.FindControl(prefix + "OrgImgfileImage"), Image)

            Select Case visitStatusData

                Case VisitStatusFree

                    ' Logger.Debug("InitCustomerRightArea_003")
                    orgImgfileImage.Visible = False

                Case VisitStatusFreeBroadcast

                    ' Logger.Debug("InitCustomerRightArea_004")
                    CType(customer.FindControl(prefix + "AccountImageAreaNormal"), Control). _
                        Visible = False
                    CType(customer.FindControl(prefix + "AccountImageAreaBroadcast"), Control). _
                        Visible = True

                Case VisitStatusAdjust

                    If String.IsNullOrEmpty(orgImgfileData) OrElse String.IsNullOrEmpty(orgImgfileData.Trim()) Then

                        ' Logger.Debug("InitCustomerRightArea_005")
                        orgImgfileImage.ImageUrl = DefaultIcon

                    Else

                        ' Logger.Debug("InitCustomerRightArea_006")
                        orgImgfileImage.ImageUrl = StaffPhotoPathPrefix & staffPhotoPath & orgImgfileData

                    End If

                    '画像の点滅設定
                    orgImgfileImage.CssClass = "imageFlashing"

                    ' $01 start 複数顧客に対する商談平行対応
                Case VisitStatusCommitBroadcast, VisitStatusCommit, VisitStatusWait, VisitStatusNegotiateStop
                    ' $01 start 複数顧客に対する商談平行対応

                    If String.IsNullOrEmpty(orgImgfileData) OrElse String.IsNullOrEmpty(orgImgfileData.Trim()) Then

                        ' Logger.Debug("InitCustomerRightArea_007")
                        orgImgfileImage.ImageUrl = DefaultIcon

                    Else

                        ' Logger.Debug("InitCustomerRightArea_008")
                        orgImgfileImage.ImageUrl = StaffPhotoPathPrefix & staffPhotoPath & orgImgfileData

                    End If

            End Select

        End If

        'セールステーブルNo
        If Not String.IsNullOrEmpty(salesTableNoData) Then

            ' Logger.Debug("InitCustomerRightArea_009")
            CType(customer.FindControl(prefix + "SalesTableNoCustomer"), Control).Visible = True
            CType(customer.FindControl(prefix + "SalesTableNoLiteral"), Literal).Text = _
                ChangeString(salesTableNoData, SalesTableNoSize, StringCut)

        End If

        ' Logger.Debug("InitCustomerRightArea_End")

    End Sub

#End Region

#End Region

#End Region

#Region "経過時間リスト作成"

    ''' <summary>
    ''' 経過時間のリスト作成
    ''' </summary>
    ''' <param name="dataTable">データテーブル</param>
    ''' <param name="columnName">カラム名</param>
    ''' <param name="nowDate">現在日時</param>
    ''' <returns>経過時間のリスト</returns>
    ''' <remarks></remarks>
    Private Function GetTimeSpanListString(ByRef dataTable As DataTable, _
                                           ByVal columnName As String, ByVal nowDate As Date) As String

        ' Logger.Debug(New StringBuilder("GetTimeSpanListString_Start Param[").Append(dataTable).Append( _
        '            ", ").Append(columnName).Append(", ").Append(nowDate).Append("]").ToString)

        Dim businessLogic As New VisitReceptionBusinessLogic
        Dim timeSpanList As List(Of String) = businessLogic.GetTimeSpanListString(dataTable, columnName, nowDate)
        businessLogic = Nothing

        Dim javaScript As New JavaScriptSerializer

        ' Logger.Debug("GetTimeSpanListString_End Ret[" & javaScript.Serialize(timeSpanList) & "]")
        Return javaScript.Serialize(timeSpanList)

    End Function

    ' $01 start 複数顧客に対する商談平行対応
    ''' <summary>
    ''' 来店状況経過時間のリスト作成
    ''' </summary>
    ''' <param name="dataTable">データテーブル</param>
    ''' <param name="nowDate">現在日時</param>
    ''' <returns>待ち経過時間のリスト</returns>
    ''' <remarks></remarks>
    Private Function GetVisitTimeSpanListString(ByRef dataTable As DataTable, _
                                               ByVal nowDate As Date) As String

        Dim businessLogic As New VisitReceptionBusinessLogic
        Dim waitTimeSpanList As List(Of String) = businessLogic.GetTimeSpanListString(dataTable, "VISITTIMESTAMP", nowDate)
        Dim stopTimeSpanList As List(Of String) = businessLogic.GetTimeSpanListString(dataTable, "STOPTIME", nowDate)
        businessLogic = Nothing

        Dim stopTimeSpanIndex As Integer = 0
        ' 商談中断の場合は、待ち時間を商談中断時間で置き換える
        For Each stopTimeSpan As String In stopTimeSpanList

            ' 値が設定されている場合
            If Not String.IsNullOrEmpty(stopTimeSpan) Then
                waitTimeSpanList.Item(stopTimeSpanIndex) = stopTimeSpanList.Item(stopTimeSpanIndex)
            End If

            stopTimeSpanIndex += 1
        Next

        Dim javaScript As New JavaScriptSerializer

        Return javaScript.Serialize(waitTimeSpanList)

    End Function
    ' $01 end   複数顧客に対する商談平行対応

#End Region

#Region "文字列表示制御"

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

        Dim resultTarget As String
        resultTarget = Server.HtmlDecode(target)

        If length < resultTarget.Length Then

            ' Logger.Debug("ChangeString_004")

            '文字列の加工
            ' 「...」表示はスタイルシートで行うため文字列カットをしない
            If StringCut.Equals(kind) Then

                ' Logger.Debug("ChangeString_004")

                resultTarget = Left(resultTarget, length)

            End If

        End If

        ' Logger.Debug("ChangeString_End Ret[" & Server.HtmlEncode(resultTarget) & "]")
        Return Server.HtmlEncode(resultTarget)

    End Function

#End Region

#End Region

End Class

