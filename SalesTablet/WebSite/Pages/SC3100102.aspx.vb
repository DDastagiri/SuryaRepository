'━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
'SC3100102.aspx.vb
'──────────────────────────────────
'機能： 受付メイン
'補足： 
'作成： 2011/12/12 KN t.mizumoto
'更新： 2012/08/27 TMEJ m.okamura 新車受付機能改善 $01
'更新： 2013/01/10 TMEJ m.asano 新車タブレットショールーム管理機能開発 $02
'更新： 2013/02/28 TMEJ t.shimamura 新車タブレット受付画面管理指標変更対応 $03
'更新： 2015/04/22 TMEJ y.gotoh FTMS全販社 BTS #331 $04
'更新： 2018/07/12 NSK a.kani TKM Next Gen e-CRB Project Application development Block B-1  Pマーク、Lマークなどを表示 START
'更新： 2020/02/05 NSK s.natsume TKM Change request development for Next Gen e-CRB (CR008) $06
'更新： 2020/02/05 NSK s.natsume TKM Change request development for Next Gen e-CRB (CR075) $07
'更新：
'──────────────────────────────────

Imports Toyota.eCRB.Visit.ReceptionistMain.BizLogic
Imports Toyota.eCRB.Visit.ReceptionistMain.DataAccess.SC3100101DataSet
Imports Toyota.eCRB.SystemFrameworks.Web.WebWordUtility
Imports Toyota.eCRB.SystemFrameworks.Core
Imports Toyota.eCRB.SystemFrameworks.Core.iCROP.BizLogic
Imports Toyota.eCRB.SystemFrameworks.Core.iCROP.DataAccess
Imports System.Data
Imports System.Web.Script.Serialization
Imports System.Web.Services
Imports System.Threading
Imports System.Web.Script.Services
Imports System.Globalization
Imports Toyota.eCRB.Visit.Api.BizLogic

''' <summary>
''' 受付メイン（サブエリア）
''' </summary>
''' <remarks></remarks>
Partial Class PagesSC3100102
    Inherits BasePage

#Region " 非公開定数 "

    ''' <summary>
    ''' スタッフステータス（商談中）
    ''' </summary>
    ''' <remarks></remarks>
    Private Const StaffStatusNego As String = "2"

    ''' <summary>
    ''' スタッフステータス（スタンバイ）
    ''' </summary>
    ''' <remarks></remarks>
    Private Const StaffStatusStanby As String = "1"

    ''' <summary>
    ''' スタッフステータス（一時退席）
    ''' </summary>
    ''' <remarks></remarks>
    Private Const StaffStatusReave As String = "3"

    ''' <summary>
    ''' スタッフステータス（オフライン）
    ''' </summary>
    ''' <remarks></remarks>
    Private Const StaffStatusOffLine As String = "4"

    ''' <summary>
    ''' デフォルトアイコン（顧客、スタッフ）のファイルパス
    ''' </summary>
    ''' <remarks></remarks>
    Private Const DefaultIcon As String = "../Styles/Images/VisitCommon/silhouette_person01.png"

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
    ''' 来店実績ステータス（フリー）
    ''' </summary>
    ''' <remarks></remarks>
    Private Const VisitStatusFree As String = "01"

    ''' <summary>
    ''' 来店実績ステータス（フリーブロードキャスト）
    ''' </summary>
    ''' <remarks></remarks>
    Private Const VisitStatusFreeBroadcast As String = "02"

    ''' <summary>
    ''' 来店実績ステータス（調整中）
    ''' </summary>
    ''' <remarks></remarks>
    Private Const VisitStatusAdjust As String = "03"

    ''' <summary>
    ''' 来店実績ステータス（確定ブロードキャスト）
    ''' </summary>
    ''' <remarks></remarks>
    Private Const VisitStatusCommitBroadcast As String = "04"

    ''' <summary>
    ''' 来店実績ステータス（確定）
    ''' </summary>
    ''' <remarks></remarks>
    Private Const VisitStatusCommit As String = "05"

    ''' <summary>
    ''' 来店実績ステータス（待ち）
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

    ' $02 start 新車タブレットショールーム管理機能開発
    ''' <summary>
    ''' 来店実績ステータス（商談中）
    ''' </summary>
    ''' <remarks></remarks>
    Private Const VisitStatusNegotiate As String = "07"

    ''' <summary>
    ''' 来店実績ステータス（接客不要）
    ''' </summary>
    ''' <remarks></remarks>
    Private Const VisitStatusUnNecessary As String = "10"
    ' $02 end   新車タブレットショールーム管理機能開発

    ' $03 start 納車作業ステータス対応
    ''' <summary>
    ''' 来店実績ステータス（接客不要）
    ''' </summary>
    ''' <remarks></remarks>
    Private Const VisitStatusDeliverlyStart As String = "11"
    ' $03 end   納車作業ステータス対応

    ''' <summary>
    ''' 文字列あふれ時対応種類（「...」表示）
    ''' </summary>
    ''' <remarks></remarks>
    Private Const StringAdd As String = "A"

    ''' <summary>
    ''' 文字列あふれ時対応種類（強制カット）
    ''' </summary>
    ''' <remarks></remarks>
    Private Const StringCut As String = "C"

    ''' <summary>
    ''' 値がない場合の設定値
    ''' </summary>
    ''' <remarks></remarks>
    Private Const DataNull As String = "-"

    ' $01 start 複数顧客に対する商談平行対応
    ''' <summary>
    ''' 時間の値が存在しない場合の設定値
    ''' </summary>
    ''' <remarks></remarks>
    Private Const NothingDate As String = "--:--"
    ' $01 end   複数顧客に対する商談平行対応

    ''' <summary>
    ''' 正常
    ''' </summary>
    ''' <remarks></remarks>
    Private Const MessageIdNormal As Integer = 0

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
    ''' 受付メイン画面ID
    ''' </summary>
    ''' <remarks></remarks>
    Private Const ReceptionistId As String = "SC3100101"

    ''' <summary>
    ''' セッションキー（文言管理）
    ''' </summary>
    ''' <remarks></remarks>
    Private Const SessionKeyWordDictionary As String = "wordDictionary"

    ''' <summary>
    ''' セッションキー（敬称の前後位置）
    ''' </summary>
    ''' <remarks></remarks>
    Private Const SessionKeyNameTitlePos As String = "nameTitlePos"

    ''' <summary>
    ''' セッションキー（顧客写真用パス）
    ''' </summary>
    ''' <remarks></remarks>
    Private Const SessionKeyFacePicPath As String = "facePicPath"

    ''' <summary>
    ''' セッションキー（スタッフ写真用パス）
    ''' </summary>
    ''' <remarks></remarks>
    Private Const SessionKeyStaffPhotoPath As String = "staffPhotoPath"

    ''' <summary>
    ''' セッションキー（苦情情報日数）
    ''' </summary>
    ''' <remarks></remarks>
    Private Const SessionKeyComplaintDateCount As String = "complaintDateCount"

    ' $01 start 複数顧客に対する商談平行対応
    ''' <summary>
    ''' セッションキー（査定警告時間）
    ''' </summary>
    ''' <remarks></remarks>
    Private Const SessionKeyAssessmentAlertSpan As String = "AssessmentAlertSpan"

    ''' <summary>
    ''' セッションキー（価格相談警告時間）
    ''' </summary>
    ''' <remarks></remarks>
    Private Const SessionKeyPriceAlertSpan As String = "PriceAlertSpan"

    ''' <summary>
    ''' セッションキー（ヘルプ警告時間）
    ''' </summary>
    ''' <remarks></remarks>
    Private Const SessionKeyHelpAlertSpan As String = "HelpAlertSpan"
    ' $02 end   新車タブレットショールーム管理機能開発

    '$07 NSK s.natsume TKM Change request development for Next Gen e-CRB (CR075) START
    ''' <summary>
    ''' セッションキー（受注後活動コード(納車))）
    ''' </summary>
    ''' <remarks></remarks>
    Private Const SessionKeyAfterActionCodeDelivery As String = "afterActionCodeDeli"
    '$07 NSK s.natsume TKM Change request development for Next Gen e-CRB (CR075) END

    ''' <summary>
    ''' スタッフ写真用パスの先頭に設定する文字列
    ''' </summary>
    ''' <remarks></remarks>
    Private Const StaffPhotoPathPrefix As String = "~/"

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
    ''' <summary>
    ''' 在席状態 - 商談中
    ''' </summary>
    ''' <remarks></remarks>
    Private Const PresenceDetailNegotiation As String = "0"

    ''' <summary>
    ''' 在席状態 - 商談中（一時対応）
    ''' </summary>
    ''' <remarks></remarks>
    Private Const PresenceDetailNegotiationTemp As String = "1"

    ''' <summary>
    ''' 在席状態 - 納車作業中
    ''' </summary>
    ''' <remarks></remarks>
    Private Const PresenceDetailDelivery As String = "2"

    ''' <summary>
    ''' 在席状態 - 納車作業中（一時対応）
    ''' </summary>
    ''' <remarks></remarks>
    Private Const PresenceDetailDeliveryTemp As String = "3"

    ''' <summary>
    ''' 振当て待ちプレフィックス
    ''' </summary>
    ''' <remarks></remarks>
    Private Const IdPrefixWaitAssgined As String = "WaitAssgined"

    ''' <summary>
    ''' 接客待ちプレフィックス
    ''' </summary>
    ''' <remarks></remarks>
    Private Const IdPrefixWaitService As String = "WaitService"

    ''' <summary>
    ''' 接客中プレフィックス
    ''' </summary>
    ''' <remarks></remarks>
    Private Const IdPrefixNegotiation As String = "Negotiation"
    ' $02 end   新車タブレットショールーム管理機能開発

    '2018/07/12 NSK a.kani TKM Next Gen e-CRB Project Application development Block B-1  Pマーク、Lマークなどを表示 START
    ''' <summary>
    ''' アイコンフラグ（1：表示）
    ''' </summary>
    ''' <remarks></remarks>
    Private Const IconFlagOn As String = "1"
    '2018/07/12 NSK a.kani TKM Next Gen e-CRB Project Application development Block B-1  Pマーク、Lマークなどを表示 END
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

    ' $02 start 新車タブレットショールーム管理機能開発
    ''' <summary>
    ''' 依頼時表示文字の文字数
    ''' </summary>
    ''' <remarks></remarks>
    Private Const RequestStringSize As Integer = 5
    ''' <summary>
    ''' 振当て待ちのヘッダー文字数
    ''' </summary>
    ''' <remarks></remarks>
    Private Const WaitAssginedHeaderSize As Integer = 10
    ''' <summary>
    ''' 接客待ちのヘッダー文字数
    ''' </summary>
    ''' <remarks></remarks>
    Private Const WaitServiceHeaderSize As Integer = 10
    ''' <summary>
    ''' 接客中のヘッダー文字数
    ''' </summary>
    ''' <remarks></remarks>
    Private Const NegotiationHeaderSize As Integer = 34
    ''' <summary>
    ''' 接客中のヘッダー文字数
    ''' </summary>
    ''' <remarks></remarks>
    Private Const ReceptionAreaUnitSize As Integer = 1
    ' $02 end   新車タブレットショールーム管理機能開発


#End Region

#End Region

#Region "非公開変数"
    ' $02 start 新車タブレットショールーム管理機能開発
    ''' <summary>
    ''' 依頼時の表示文言(査定)
    ''' </summary>
    ''' <remarks></remarks>
    Private requestAssessment As String = String.Empty
    ''' <summary>
    ''' 依頼時の表示文言(価格相談)
    ''' </summary>
    ''' <remarks></remarks>
    Private requestPrice As String = String.Empty
    ''' <summary>
    ''' 依頼時の表示文言(ヘルプ)
    ''' </summary>
    ''' <remarks></remarks>
    Private requestHelp As String = String.Empty

    ''' <summary>
    ''' 依頼時の表示文言(査定)
    ''' </summary>
    ''' <remarks></remarks>
    Private assessmentAlertTime As Integer = 0
    ''' <summary>
    ''' 依頼時の表示文言(価格相談)
    ''' </summary>
    ''' <remarks></remarks>
    Private priceAlertTime As Integer = 0
    ''' <summary>
    ''' 依頼時の表示文言(ヘルプ)
    ''' </summary>
    ''' <remarks></remarks>
    Private helpAlertTime As Integer = 0
    ' $02 end   新車タブレットショールーム管理機能開発
#End Region

#Region " イベント処理 "

#Region "ページロード時の処理"
    ''' <summary>
    ''' ページロード時の処理です。
    ''' </summary>
    ''' <param name="sender">イベント発生元</param>
    ''' <param name="e">イベントデータ</param>
    ''' <remarks></remarks>
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Logger.Info("Page_Load_Start Param[" & sender.ToString & "," & e.ToString & "]")

        If Not Me.IsPostBack Then
            ' Logger.Debug("Page_Load_001" & "Not PostBack")

            'ログインユーザの情報を格納
            Dim context As StaffContext = StaffContext.Current

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

            ' $04 start FTMS全販社 BTS #331
            ' 通知一覧(MG)の依頼をタップし画面遷移する際、通知登録IFが呼ばれ
            ' Push通知を受信するため、リフレッシュが行われる。
            ' その場合、セッションから値が取得できずエラーとなるため、
            ' 取得前にセッションの存在チェックを行う。
            If Not MyBase.ContainsKey(ScreenPos.Current, SessionKeyWordDictionary) Then
                Logger.Info("Page_Load_Start_003 SessionKey Not Exist")
                Logger.Info("Page_Load_End Ret[]")
                Return
            End If
            ' $04 end FTMS全販社 BTS #331

            ' 文言管理
            Logger.Info("Page_Load_Start_004" & "Call_Start MyBase.GetValue Param[" & _
                         ScreenPos.Current & "," & SessionKeyWordDictionary & "," & False & "]")
            Dim wordDictionary As Dictionary(Of Decimal, String) = CType(MyBase.GetValue(ScreenPos.Current, SessionKeyWordDictionary, False), Dictionary(Of Decimal, String))
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

            Me.InitWord(wordDictionary)
            Me.InitBoard(nowDate, context)

            ' $02 start 新車タブレットショールーム管理機能開発
            ' 各種依頼の遅れ判定時間を取得
            assessmentAlertTime = CType(MyBase.GetValue(ScreenPos.Current, SessionKeyAssessmentAlertSpan, False), Integer)
            priceAlertTime = CType(MyBase.GetValue(ScreenPos.Current, SessionKeyPriceAlertSpan, False), Integer)
            helpAlertTime = CType(MyBase.GetValue(ScreenPos.Current, SessionKeyHelpAlertSpan, False), Integer)
            ' $02 end   新車タブレットショールーム管理機能開発

            '苦情情報取得
            Logger.Info("Page_Load_Start_008 " & "Call_Start MyBase.GetValue Param[" & _
                   ScreenPos.Current & "," & SessionKeyComplaintDateCount & "," & False & "]")
            Dim complaintDateCount As Integer = CType(MyBase.GetValue(ScreenPos.Current, SessionKeyComplaintDateCount, False), Integer)
            Logger.Info("Page_Load_Start_008 " & "Call_End MyBase.GetValue Ret[" & complaintDateCount & "]")

            Dim claimVisitSequenceList As List(Of Long) = Nothing
            ' $02 start 新車タブレットショールーム管理機能開発
            Dim businessLogic As New SC3100101BusinessLogic
            ' $02 end   新車タブレットショールーム管理機能開発
            claimVisitSequenceList = businessLogic.GetClaimInfo(context.DlrCD, _
                                                            context.BrnCD, _
                                                            nowDate, _
                                                            complaintDateCount)
            businessLogic = Nothing

            ' $02 start 新車タブレットショールーム管理機能開発
            Me.InitStaff(nowDate, context, staffPhotoPath)
            Me.InitReceptionInfo(IdPrefixWaitAssgined, WaitAssginedRepeater, nowDate, context, nameTitlePos, wordDictionary, staffPhotoPath, claimVisitSequenceList, ReceptionClassWaitAssgined)
            Me.InitReceptionInfo(IdPrefixWaitService, WaitServiceRepeater, nowDate, context, nameTitlePos, wordDictionary, staffPhotoPath, claimVisitSequenceList, ReceptionClassWaitService)
            Me.InitReceptionInfo(IdPrefixNegotiation, NegotiationRepeater, nowDate, context, nameTitlePos, wordDictionary, staffPhotoPath, claimVisitSequenceList, ReceptionClassNegotiation)
            ' $02 end   新車タブレットショールーム管理機能開発
        End If
        Logger.Info("Page_Load_End Ret[]")
    End Sub
#End Region

#End Region

#Region " 非公開メソッド"

#Region "文言管理"
    ''' <summary>
    ''' 文言管理にDB登録を行い文言番号より取得する
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub InitWord(ByVal wordDictionary As Dictionary(Of Decimal, String))

        ' $02 start 新車タブレットショールーム管理機能開発
        ' アンドン
        ' タイトルについて文字数でのカットは行わない
        BoardVisitLiteral.Text = Server.HtmlEncode(wordDictionary(55))
        BoardSalesLiteral.Text = Server.HtmlEncode(wordDictionary(56))
        '$06 NSK s.natsume TKM Change request development for Next Gen e-CRB (CR008) START
        'BoardAssessmentLiteral.Text = Server.HtmlEncode(wordDictionary(57))
        '$06 NSK s.natsume TKM Change request development for Next Gen e-CRB (CR008) END
        BoardEstimateLiteral.Text = Server.HtmlEncode(wordDictionary(58))
        BoardConclusionLiteral.Text = Server.HtmlEncode(wordDictionary(59))
        BoardDeliveryLiteral.Text = Server.HtmlEncode(wordDictionary(60))

        ' 接客状況
        ' タイトルについて文字数でのカットは行わない
        WaitAssginedTitleLiteral.Text = Server.HtmlEncode(wordDictionary(61))
        WaitAssginedUnitLiteral.Text = Server.HtmlEncode(wordDictionary(62))
        WaitServiceTitleLiteral.Text = Server.HtmlEncode(wordDictionary(63))
        WaitServiceUnitLiteral.Text = Server.HtmlEncode(wordDictionary(64))
        NegotiationTitleLiteral.Text = Server.HtmlEncode(wordDictionary(65))
        NegotiationUnitLiteral.Text = Server.HtmlEncode(wordDictionary(66))

        ' 依頼情報文言
        requestAssessment = "<li id=""AssessmentRequest"">" _
                & ChangeString(wordDictionary(67), RequestStringSize, StringCut) & "</li>"
        requestPrice = ("<li id=""PriceRequest"">" _
                & ChangeString(wordDictionary(68), RequestStringSize, StringCut) & "</li>")
        requestHelp = "<li id=""HelpRequest"">" _
                & ChangeString(wordDictionary(69), RequestStringSize, StringCut) & "</li>"
        ' $02 end   新車タブレットショールーム管理機能開発
    End Sub

#End Region

#Region "アンドン初期表示"
    ''' <summary>
    ''' アンドン初期表示
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub InitBoard(ByVal nowDate As Date, ByVal context As StaffContext)
        ' Logger.Debug("InitBoard_Start Param[]")

        ' アンドン情報取得
        ' $02 start 新車タブレットショールーム管理機能開発
        Dim boardDataTable As SC3100101BoardInfoDataTable = Nothing
        Dim businessLogic As New SC3100101BusinessLogic
        '$07 NSK s.natsume TKM Change request development for Next Gen e-CRB (CR075) START
        'boardDataTable = businessLogic.GetBoardInfo(context.DlrCD, context.BrnCD, nowDate)
        Dim afterActionCodeDeli As String = CType(MyBase.GetValue(ScreenPos.Current, SessionKeyAfterActionCodeDelivery, False), String)
        boardDataTable = businessLogic.GetBoardInfo(context.DlrCD, context.BrnCD, nowDate, afterActionCodeDeli)
        '$07 NSK s.natsume TKM Change request development for Next Gen e-CRB (CR075) END
        businessLogic = Nothing
        Dim boardDataRow As SC3100101BoardInfoRow = boardDataTable.Rows(0)
        ' $02 end   新車タブレットショールーム管理機能開発

        ' 情報が存在しない場合は処理しない
        If boardDataTable.Rows.Count = 0 Then
            Exit Sub
        End If

        ' 型付データテーブル
        ' $02 start 新車タブレットショールーム管理機能開発
        BoardVisitNumber.Text = boardDataRow.VISITORCOUNT
        BoardSalesNumber.Text = boardDataRow.NEGOTIATIONCOUNT
        '$06 NSK s.natsume TKM Change request development for Next Gen e-CRB (CR008) START
        'BoardAssessmentNumber.Text = boardDataRow.APPRAISALCOUNT
        '$06 NSK s.natsume TKM Change request development for Next Gen e-CRB (CR008) END
        BoardEstimateNumber.Text = boardDataRow.ESTIMATIONCOUNT
        BoardConclusionNumber.Text = boardDataRow.ACCEPTIONORDERCOUNT

        '$03 start 納車ステータス対応
        BoardDeliveryNumber.Text = boardDataRow.DELIVERLYCOUNT
        '$03 end   納車ステータス対応

        ' $02 end   新車タブレットショールーム管理機能開発
        ' Logger.Debug("InitBoard_End Ret[]")
    End Sub
#End Region

#Region "スタッフ状況初期表示"
    ''' <summary>
    ''' スタッフ状況初期表示
    ''' </summary>
    ''' <param name="nowDate">現在日時</param>
    ''' <param name="context">ログイン情報</param>
    ''' <param name="staffPhotoPath">スタッフ画像パス</param>
    ''' <remarks></remarks>
    Private Sub InitStaff(ByVal nowDate As Date, ByVal context As StaffContext, _
                          ByVal staffPhotoPath As String)

        ' Logger.Debug("InitStaff_Start Param[" & nameTitlePos & "," & facePicPath & "]")

        'スタッフ情報取得
        ' $02 start 新車タブレットショールーム管理機能開発
        Dim staffDataTable As SC3100101StaffStatusDataTable = Nothing
        Dim businessLogic As New SC3100101BusinessLogic
        staffDataTable = businessLogic.GetStaffSituationInfo(context.DlrCD, context.BrnCD, nowDate)
        businessLogic = Nothing
        ' $02 end   新車タブレットショールーム管理機能開発

        ' 情報が存在しない場合は処理しない
        If staffDataTable.Rows.Count = 0 Then
            Exit Sub
        End If

        ' コントロールにバインドする
        ' $02 start 新車タブレットショールーム管理機能開発
        StaffRepeater.DataSource = staffDataTable
        StaffRepeater.DataBind()

        ' -----------------------------------------------------
        ' データを設定する
        ' -----------------------------------------------------
        For i = 0 To StaffRepeater.Items.Count - 1

            Dim staff As Control = StaffRepeater.Items(i)

            '----------------------------------------------------------------------
            ' スタッフステータスによりチップの背景色を変更
            '----------------------------------------------------------------------
            Dim staffStatus As String = If(staffDataTable(i).IsPRESENCECATEGORYNull(), String.Empty, staffDataTable(i).PRESENCECATEGORY)
            Dim staffStatusDetail As String = If(staffDataTable(i).IsPRESENCEDETAILNull(), String.Empty, staffDataTable(i).PRESENCEDETAIL)
            Select Case staffStatus
                Case StaffStatusNego

                    ' 商談中か納車作業中か判断
                    If staffStatusDetail = PresenceDetailDelivery OrElse staffStatusDetail = PresenceDetailDeliveryTemp Then
                        ' 納車作業中
                        CType(staff.FindControl("StuffChipDiv"), HtmlGenericControl).Attributes("class") = "CassetteBack BackColor_Pink"
                    Else
                        ' 商談中
                        CType(staff.FindControl("StuffChipDiv"), HtmlGenericControl).Attributes("class") = "CassetteBack BackColor_SkyBlue"
                    End If

                Case StaffStatusReave
                    ' 一時退席中
                    CType(staff.FindControl("StuffChipDiv"), HtmlGenericControl).Attributes("class") = "CassetteBack BackColor_Yellow"

                Case StaffStatusOffLine
                    ' オフライン
                    CType(staff.FindControl("InactiveDiv"), HtmlGenericControl).Visible = True
            End Select

            '-------------------------------------------
            ' スタッフ情報
            '-------------------------------------------
            Dim orgImgFileData As String = If(staffDataTable(i).IsORG_IMGFILENull(), String.Empty, staffDataTable(i).ORG_IMGFILE)
            If String.IsNullOrEmpty(orgImgFileData) OrElse String.IsNullOrEmpty(orgImgFileData.Trim()) Then
                CType(staff.FindControl("OrgImgFileImage"), Image).ImageUrl = DefaultIcon
            Else
                CType(staff.FindControl("OrgImgFileImage"), Image).ImageUrl = StaffPhotoPathPrefix & staffPhotoPath & orgImgFileData
            End If

            Dim userNameData As String = If(staffDataTable(i).IsUSERNAMENull(), String.Empty, staffDataTable(i).USERNAME)
            If Not String.IsNullOrEmpty(userNameData) Then
                CType(staff.FindControl("UserNameLiteral"), Literal).Text = _
                    ChangeString(userNameData, StaffSituationHumanNameSize, StringAdd)
            End If

            '----------------------------------------------------------------------
            ' 紐付け人数
            '----------------------------------------------------------------------
            Dim visitorLinkingCountData As String = _
                If(staffDataTable(i).IsVISITORLINKINGCOUNTNull(), String.Empty, _
                   CType(staffDataTable(i).VISITORLINKINGCOUNT, String))

            Dim visitorLinkingCount As Integer = 0
            If Not String.IsNullOrEmpty(visitorLinkingCountData) Then
                visitorLinkingCount = CType(visitorLinkingCountData, Integer)
            End If

            ' 紐付け人数が存在する場合
            If visitorLinkingCount > 0 Then

                If visitorLinkingCount > 0 Then
                    staff.FindControl("LinkingCountDiv").Visible = True
                    CType(staff.FindControl("VisitorLinkingCountLiteral"), Literal).Text = visitorLinkingCount
                End If

            End If

        Next
        ' $02 end   新車タブレットショールーム管理機能開発

        ' Logger.Debug("InitStaff_End  Ret[]")
    End Sub

#End Region

#Region "接客状況初期表示"

    ' $02 start 新車タブレットショールーム管理機能開発
    ''' <summary>
    ''' 接客状況初期表示
    ''' </summary>
    ''' <param name="prefix">コントロールIDのプレフィックス</param>
    ''' <param name="repeater">リピーターコントロール</param>
    ''' <param name="nowDate">現在日時</param>
    ''' <param name="context">ログイン情報</param>
    ''' <param name="nameTitlePos">敬称位置</param>
    ''' <param name="wordDictionary">文言リスト</param>
    ''' <param name="staffPhotoPath">スタッフ画像パス</param>
    ''' <param name="claimVisitSequenceList">クレーム情報一覧</param>
    ''' <param name="areaType">エリア種別</param>
    ''' <remarks></remarks>
    Private Sub InitReceptionInfo(ByVal prefix As String, ByRef repeater As Repeater, _
                                    ByVal nowDate As Date, ByVal context As StaffContext, _
                                    ByVal nameTitlePos As String, ByVal wordDictionary As Dictionary(Of Decimal, String), _
                                    ByVal staffPhotoPath As String, ByVal claimVisitSequenceList As List(Of Long), _
                                    ByVal areaType As String)

        '接客情報取得
        Dim receptionInfoDataTable As SC3100101ReceptionInfoDataTable = Nothing
        Dim businessLogic As New SC3100101BusinessLogic
        receptionInfoDataTable = businessLogic.GetReceptionInfo(context.DlrCD, _
                                                          context.BrnCD, _
                                                          areaType, _
                                                          nowDate, _
                                                          claimVisitSequenceList)
        businessLogic = Nothing

        ' $02 start 新車タブレットショールーム管理機能開発
        ' チップ件数
        CType(FindControl(prefix + "NumberLiteral"), Literal).Text = _
            receptionInfoDataTable.Rows.Count
        ' $02 end   新車タブレットショールーム管理機能開発

        ' 情報が存在しない場合は処理しない
        If receptionInfoDataTable.Rows.Count = 0 Then
            Exit Sub
        End If

        ' $02 start 新車タブレットショールーム管理機能開発
        ' 経過時間のリストを作成する。
        Select Case areaType

            Case ReceptionClassWaitAssgined
                ' 振当て待ち
                WaitAssginedTimeList.Value = GetWaitAssginedTimeSpanListString(receptionInfoDataTable, nowDate)

            Case ReceptionClassWaitService
                ' 接客待ち
                WaitServiceTimeList.Value = GetWaitServiceTimeSpanListString(receptionInfoDataTable, nowDate)

            Case ReceptionClassNegotiation
                ' 接客中
                NegotiationTimeList.Value = _
                    GetNegotiationTimeSpanListString(receptionInfoDataTable, nowDate)

                ' 通知送信日時のリストを設定する（査定依頼）
                RequestAssessmentTimeDateList.Value = _
                    GetRequestTimeSpanListString(receptionInfoDataTable, "REQUESTASSESSMENTDATE", nowDate)
                ' 通知送信日時のリストを設定する（価格相談依頼）
                RequestPriceConsultationTimeDateList.Value = _
                    GetRequestTimeSpanListString(receptionInfoDataTable, "REQUESTPRICECONSULTATIONDATE", nowDate)
                ' 通知送信日時のリストを設定する（ヘルプ依頼）
                RequestHelpTimeDateList.Value = _
                    GetRequestTimeSpanListString(receptionInfoDataTable, "REQUESTHELPDATE", nowDate)

        End Select
        ' $02 end   新車タブレットショールーム管理機能開発

        ' コントロールにバインドする
        repeater.DataSource = receptionInfoDataTable
        repeater.DataBind()

        ' -----------------------------------------------------
        ' データを設定する
        ' -----------------------------------------------------
        For i = 0 To repeater.Items.Count - 1
            Dim customer As Control = repeater.Items(i)

            '----------------------------------------------------------------------
            ' 左側
            '----------------------------------------------------------------------
            InitCustomerLeftArea(prefix, customer, receptionInfoDataTable.Rows(i), nameTitlePos, wordDictionary)

            '----------------------------------------------------------------------
            ' 右側
            '----------------------------------------------------------------------
            InitCustomerRightArea(prefix, customer, receptionInfoDataTable.Rows(i), staffPhotoPath, areaType, wordDictionary)

        Next

    End Sub
    ' $02 end   新車タブレットショールーム管理機能開発

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
                                     ByVal row As SC3100101ReceptionInfoRow, _
                                     ByVal nameTitlePos As String, _
                                     ByVal wordDictionary As Dictionary(Of Decimal, String))

        Dim visitTimestampData As String = _
            If(row.IsVISITTIMESTAMPNull(), String.Empty, Format(CType(row.VISITTIMESTAMP, DateTime), "yyyy/MM/dd HH:mm:ss"))
        Dim vclregNoData As String = If(row.IsVCLREGNONull(), String.Empty, row.VCLREGNO)
        Dim visitPersonNumData As String = If(row.IsVISITPERSONNUMNull(), String.Empty, CType(row.VISITPERSONNUM, String))
        Dim visitMeansData As String = If(row.IsVISITMEANSNull(), String.Empty, row.VISITMEANS)
        Dim custNameData As String = If(row.IsCUSTNAMENull(), String.Empty, row.CUSTNAME)
        Dim custNameTitleData As String = If(row.IsCUSTNAMETITLENull(), String.Empty, row.CUSTNAMETITLE)
        Dim custSegmentData As String = If(row.IsCUSTSEGMENTNull(), String.Empty, row.CUSTSEGMENT)
        Dim custClaimFlg As String = If(row.IsCLAIMFLGNull(), String.Empty, row.CLAIMFLG)
        Dim userNameData As String = If(row.IsUSERNAMENull(), String.Empty, row.USERNAME)

        '2018/07/12 NSK a.kani TKM Next Gen e-CRB Project Application development Block B-1  Pマーク、Lマークなどを表示 START
        Dim iconFlagL As String = If(row.IsICON_FLAG_LNull(), String.Empty, row.ICON_FLAG_L)
        '2018/07/12 NSK a.kani TKM Next Gen e-CRB Project Application development Block B-1  Pマーク、Lマークなどを表示 END
        ' 削除ボタン
        ' 削除ボタンについて文字数でのカットは行わない
        CType(customer.FindControl(prefix + "VisitorDeleteLiteral"), Literal).Text = _
             Server.HtmlEncode(wordDictionary(17))

        ' $01 start 複数顧客に対する商談平行対応
        ' 来店時間が存在する場合は表示する
        If Not String.IsNullOrEmpty(visitTimestampData) Then
            CType(customer.FindControl(prefix + "VisitStartLiteral"), Literal).Text = _
                ChangeString(Format(CType(visitTimestampData, DateTime), "HH:mm"), VisitTimeStampSize, StringCut)
        Else
            CType(customer.FindControl(prefix + "VisitStartLiteral"), Literal).Text = NothingDate
        End If
        ' $01 end   複数顧客に対する商談平行対応

        ' 顧客エリア
        If Not String.IsNullOrEmpty(custNameData) AndAlso Not String.IsNullOrEmpty(custNameData.Trim()) Then

            Dim custName As New StringBuilder

            If String.IsNullOrEmpty(custNameTitleData) OrElse String.IsNullOrEmpty(custNameTitleData.Trim()) Then
                custNameTitleData = String.Empty
            End If

            '敬称の前後位置
            If NameTitlePositionFront.Equals(nameTitlePos) Then
                custName.Append(custNameTitleData)
                custName.Append(custNameData)
            Else
                custName.Append(custNameData)
                custName.Append(custNameTitleData)
            End If

            CType(customer.FindControl(prefix + "CustNameLiteral"), Literal).Text = _
                ChangeString(custName.ToString, VistorSituationCustomerNameSize, StringAdd)
        Else

            If String.IsNullOrEmpty(custSegmentData) Then
                ' 新規顧客の場合
                CType(customer.FindControl(prefix + "CustNameLiteral"), Literal).Text = _
                    Server.HtmlEncode(wordDictionary(32))
            Else
                ' 既存顧客の場合
                CType(customer.FindControl(prefix + "CustNameLiteral"), Literal).Text = _
                    Server.HtmlEncode(wordDictionary(31))
            End If
        End If

        '2018/07/12 NSK a.kani TKM Next Gen e-CRB Project Application development Block B-1  Pマーク、Lマークなどを表示 START
        If Not String.IsNullOrEmpty(iconFlagL) AndAlso iconFlagL.Equals(IconFlagOn) Then
            'フラグが2のとき、Lマークを表示
            CType(customer.FindControl(prefix + "LIcon"), Control).Visible = True
        Else
            'それ以外はLマークを非表示
            CType(customer.FindControl(prefix + "LIcon"), Control).Visible = False
        End If
        '2018/07/12 NSK a.kani TKM Next Gen e-CRB Project Application development Block B-1  Pマーク、Lマークなどを表示 END

        ' 来店人数
        If Not String.IsNullOrEmpty(visitPersonNumData) Then
            CType(customer.FindControl(prefix + "VisitPersonNum"), Control).Visible = True
            CType(customer.FindControl(prefix + "VisitPersonNumLiteral"), Literal).Text = _
                ChangeString(visitPersonNumData + wordDictionary(16), VistorSituationCustomerNumSize, StringCut)
        End If

        ' 車両登録No
        If Not String.IsNullOrEmpty(vclregNoData) Then

            CType(customer.FindControl(prefix + "VclregNoLine"), Control).Visible = True
            CType(customer.FindControl(prefix + "VclregNoLiteral"), Literal).Text = _
                ChangeString(vclregNoData, VehicleRegistrationSize, StringAdd)

        Else
            ' 来店手段
            If Not String.IsNullOrEmpty(visitMeansData) Then

                If VisitMeansCar.Equals(visitMeansData) Then

                    CType(customer.FindControl(prefix + "MeansCarLine"), Control). _
                        Visible = True

                ElseIf VisitMeansWalk.Equals(visitMeansData) Then

                    CType(customer.FindControl(prefix + "MeansWalkLine"), Control). _
                        Visible = True

                End If
            End If
        End If

        ' 苦情
        If Not String.IsNullOrEmpty(custClaimFlg) Then
            customer.FindControl(prefix + "ClaimIcnDiv").Visible = True
            CType(customer.FindControl(prefix + "ClaimIconLiteral"), Literal).Text = Server.HtmlEncode(wordDictionary(42))
        End If

        ' $02 start 新車タブレットショールーム管理機能開発
        ' 顧客担当SC名
        If Not String.IsNullOrEmpty(userNameData) Then
            CType(customer.FindControl(prefix + "UserName"), Control).Visible = True
            CType(customer.FindControl(prefix + "UserNameLiteral"), Literal).Text = _
                ChangeString(userNameData, VistorSituationStaffNameSize, StringAdd)
        End If
        ' $02 end   新車タブレットショールーム管理機能開発
    End Sub

    ''' <summary>
    ''' 来店状況右側エリアの表示
    ''' </summary>
    ''' <param name="prefix">コントロールIDのプレフィックス</param>
    ''' <param name="customer">顧客情報コントロール</param>
    ''' <param name="row">データロウ</param>
    ''' <param name="staffPhotoPath">スタッフ画像パス</param>
    ''' <param name="areaType">エリア種別</param>
    ''' <param name="wordDictionary">文言リスト</param>
    ''' <remarks></remarks>
    Private Sub InitCustomerRightArea(ByVal prefix As String, ByVal customer As Control, _
                                      ByVal row As SC3100101ReceptionInfoRow, _
                                      ByVal staffPhotoPath As String, ByVal areaType As String, _
                                      ByVal wordDictionary As Dictionary(Of Decimal, String))

        Dim visitStatusData As String = If(row.IsVISITSTATUSNull(), String.Empty, row.VISITSTATUS)
        Dim salesTableNoData As String = If(row.IsSALESTABLENONull(), String.Empty, CType(row.SALESTABLENO, String))
        Dim orgImgfileData As String = If(row.IsORG_IMGFILENull(), String.Empty, row.ORG_IMGFILE)
        ' $02 start 新車タブレットショールーム管理機能開発
        Dim presenceCategoryData As String = If(row.IsPRESENCECATEGORYNull(), String.Empty, row.PRESENCECATEGORY)
        Dim presenceDetaileData As String = If(row.IsPRESENCEDETAILNull(), String.Empty, row.PRESENCEDETAIL)
        Dim unnecessaryCountData As Int16 = If(row.IsUNNECESSARYCOUNTNull(), 0, CType(row.UNNECESSARYCOUNT, Int16))
        ' $02 end   新車タブレットショールーム管理機能開発

        ' 表示エリア判断
        If ReceptionClassWaitAssgined.Equals(areaType) Then
            ' 振当て待ちエリア
            If VisitStatusUnNecessary.Equals(visitStatusData) Then
                If unnecessaryCountData > 4 Then
                    CType(customer.FindControl("UnnecessarySetMore"), HtmlGenericControl).Visible = True
                    CType(customer.FindControl("PinNumberLiteral"), Literal).Text = unnecessaryCountData
                Else
                    CType(customer.FindControl("UnnecessarySet"), HtmlGenericControl).Visible = True
                    Dim pinIcon As String = String.Empty
                    For index = 1 To unnecessaryCountData
                        pinIcon = pinIcon & "<li></li>"
                    Next
                    CType(customer.FindControl("PinIconLiteral"), Literal).Text = pinIcon
                End If
            End If
        Else
            ' 接客待ちor接客中エリア
            ' スタッフ写真
            If Not String.IsNullOrEmpty(visitStatusData) Then

                Dim orgImgfileImage As Image = _
                    CType(customer.FindControl(prefix + "OrgImgfileImage"), Image)

                Select Case visitStatusData
                    Case VisitStatusFreeBroadcast

                        CType(customer.FindControl(prefix + "AccountImageAreaNormal"), Control). _
                            Visible = False
                        CType(customer.FindControl(prefix + "AccountImageAreaBroadcast"), Control). _
                            Visible = True

                        ' $02 start 新車タブレットショールーム管理機能開発
                        ' $03 start 新車タブレット受付画面管理指標変更対応
                    Case VisitStatusAdjust, VisitStatusCommitBroadcast, VisitStatusCommit, VisitStatusWait, VisitStatusNegotiateStop, VisitStatusNegotiate, VisitStatusDeliverlyStart
                        ' $03 end   新車タブレット受付画面管理指標変更対応

                        If String.IsNullOrEmpty(orgImgfileData) OrElse String.IsNullOrEmpty(orgImgfileData.Trim()) Then
                            orgImgfileImage.ImageUrl = DefaultIcon
                        Else
                            orgImgfileImage.ImageUrl = StaffPhotoPathPrefix & staffPhotoPath & orgImgfileData
                        End If

                        ' 調整中の場合は画像の点滅設定
                        If visitStatusData = VisitStatusAdjust Then
                            orgImgfileImage.CssClass = "imageFlashing"
                        End If
                        ' $02 end   新車タブレットショールーム管理機能開発
                End Select
            End If

            ' $02 start 新車タブレットショールーム管理機能開発
            ' 接客中エリアの場合のみ依頼情報を表示
            If ReceptionClassNegotiation.Equals(areaType) Then
                ' 依頼情報の表示
                Dim request As String = CreateRequestString(row, wordDictionary)
                If Not String.IsNullOrEmpty(request) Then
                    CType(customer.FindControl("RequestInfoLiteral"), Literal).Text = request
                End If

                If VisitStatusNegotiateStop.Equals(visitStatusData) Then
                    CType(customer.FindControl("EventDiv"), HtmlGenericControl).Attributes("class") = _
                        "ReceptionChip"
                Else
                    CType(customer.FindControl("EventDiv"), HtmlGenericControl).Attributes("class") = _
                        "NegotiationChip"
                End If
            End If
            ' $02 end   新車タブレットショールーム管理機能開発
        End If

            ' セールステーブルNo
            If Not String.IsNullOrEmpty(salesTableNoData) Then
                CType(customer.FindControl(prefix + "SalesTableNoCustomer"), Control). _
                    Visible = True
                CType(customer.FindControl(prefix + "SalesTableNoLiteral"), Literal).Text = _
                    ChangeString(salesTableNoData, SalesTableNoSize, StringCut)
            End If

            ' $02 start 新車タブレットショールーム管理機能開発
            ' チップの背景色
            If VisitStatusWait.Equals(visitStatusData) OrElse VisitStatusNegotiateStop.Equals(visitStatusData) Then
                ' ステータスが"待ち"又は"商談中断中"の場合はグレーアウト
            CType(customer.FindControl(prefix + "Inactive"), Control).Visible = True
            ' $03 start 納車作業ステータス対応
        ElseIf VisitStatusDeliverlyStart.Equals(visitStatusData) Then
            ' $03 end   納車作業ステータス対応
            ' 納車作業中の場合は、ピンクの枠線を付ける。
            CType(customer.FindControl("NegotiationDelivery"), Control).Visible = True
            End If
            ' $02 end   新車タブレットショールーム管理機能開発
    End Sub

    ''' <summary>
    ''' 依頼情報表示用のHTML文字列を生成。
    ''' </summary>
    ''' <param name="row">データロウ</param>
    ''' <param name="wordDictionary">文言リスト</param>
    ''' <returns>依頼情報表示用のHTML文字列</returns>
    ''' <remarks></remarks>
    Private Function CreateRequestString(ByVal row As SC3100101ReceptionInfoRow, _
                                         ByVal wordDictionary As Dictionary(Of Decimal, String)) As String

        Dim requestAssessmentDate As DateTime = If(row.IsREQUESTASSESSMENTDATENull(), Nothing, CType(row.REQUESTASSESSMENTDATE, DateTime))
        Dim requestPriceDate As DateTime = If(row.IsREQUESTPRICECONSULTATIONDATENull(), Nothing, CType(row.REQUESTPRICECONSULTATIONDATE, DateTime))
        Dim requestHelpDate As DateTime = If(row.IsREQUESTHELPDATENull(), Nothing, CType(row.REQUESTHELPDATE, DateTime))

        Dim request As String = String.Empty

        ' 依頼情報がなければ処理しない。
        If requestAssessmentDate = Nothing AndAlso _
           requestPriceDate = Nothing AndAlso _
           requestHelpDate = Nothing Then

            Return request
        End If

        ' 各依頼送信日時に遅れ判定時間を加算する。
        ' 依頼の表示順を遅れ時間に近い順にする為。
        If Not requestAssessmentDate = Nothing Then
            requestAssessmentDate = requestAssessmentDate.AddSeconds(assessmentAlertTime)
        End If
        If Not requestPriceDate = Nothing Then
            requestPriceDate = requestPriceDate.AddSeconds(priceAlertTime)
        End If
        If Not requestHelpDate = Nothing Then
            requestHelpDate = requestHelpDate.AddSeconds(helpAlertTime)
        End If

        ' ソート用配列を作成
        Dim keyArray As ArrayList = New ArrayList
        Dim valueArray As ArrayList = New ArrayList
        keyArray.Add(requestAssessment)
        keyArray.Add(requestPrice)
        keyArray.Add(requestHelp)
        valueArray.Add(requestAssessmentDate)
        valueArray.Add(requestPriceDate)
        valueArray.Add(requestHelpDate)

        'keyArrayの値をキーにしてソート
        Dim sortArr(1)() As Object
        sortArr(0) = keyArray.ToArray
        sortArr(1) = valueArray.ToArray
        Array.Sort(sortArr(1), sortArr(0))
        Array.Sort(sortArr(1), sortArr(1))

        ' HTML生成
        For index As Integer = 0 To sortArr(0).Length - 1
            If Not sortArr(1)(index) = Nothing Then
                request = request & sortArr(0)(index)
            End If
        Next
        Return request
    End Function
#End Region

#Region "経過時間のリスト作成"

    ' $02 start 新車タブレットショールーム管理機能開発
    ''' <summary>
    ''' 経過時間のリスト作成(依頼)
    ''' </summary>
    ''' <param name="dataTable">データテーブル</param>
    ''' <param name="columnName">カラム名</param>
    ''' <param name="nowDate">現在日時</param>
    ''' <returns>経過時間のリスト</returns>
    ''' <remarks></remarks>
    Private Function GetRequestTimeSpanListString(ByRef dataTable As DataTable, _
                                           ByVal columnName As String, ByVal nowDate As Date) As String
        Logger.Info("GetRequestTimeSpanListString_Start " & _
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

        Logger.Info("GetRequestTimeSpanListString_End Ret[timeSpanList.Count = " & timeSpanList.Count & "]")
        Dim javaScript As New JavaScriptSerializer
        Return javaScript.Serialize(timeSpanList)
    End Function

    ''' <summary>
    ''' 経過時間リスト作成(振当て待ちエリア)
    ''' </summary>
    ''' <param name="dataTable">データテーブル</param>
    ''' <param name="nowDate">現在日時</param>
    ''' <returns>経過時間のリスト</returns>
    ''' <remarks></remarks>
    Private Function GetWaitAssginedTimeSpanListString(ByRef dataTable As DataTable, _
                                                       ByVal nowDate As Date) As String
        Dim timeSpanList As New List(Of String)
        For Each dataRow As DataRow In dataTable.Rows

            Dim targetDate As String = String.Empty

            ' 来店実績ステータスの判定
            If dataRow("VISITSTATUS").ToString() = VisitStatusUnNecessary Then

                ' 接客不要の場合、接客不要時間を経過時間の基準時間とする。
                targetDate = CType(dataRow("UNNECESSARYDATE").ToString(), Date)
            Else
                ' 接客不要以外の場合
                ' 商談中断時間が設定されているか判断
                If Not IsDBNull(dataRow("STOPTIME")) AndAlso Not String.IsNullOrEmpty(dataRow("STOPTIME").ToString) Then
                    ' 商談中断時間が設定されている場合、商談中断時間を経過時間の基準時間とする。
                    targetDate = CType(dataRow("STOPTIME").ToString(), Date)
                Else
                    ' 商談中断時間が設定されていない場合、来店時間を経過時間の基準時間とする。
                    targetDate = CType(dataRow("VISITTIMESTAMP").ToString(), Date)
                End If
            End If

            timeSpanList.Add(CType(Math.Round(nowDate.Subtract(targetDate).TotalSeconds), String))
        Next

        Dim javaScript As New JavaScriptSerializer
        Return javaScript.Serialize(timeSpanList)

    End Function

    ''' <summary>
    ''' 経過時間リスト作成(接客待ちエリア)
    ''' </summary>
    ''' <param name="dataTable">データテーブル</param>
    ''' <param name="nowDate">現在日時</param>
    ''' <returns>経過時間のリスト</returns>
    ''' <remarks></remarks>
    Private Function GetWaitServiceTimeSpanListString(ByRef dataTable As DataTable, _
                                                      ByVal nowDate As Date) As String

        Dim timeSpanList As New List(Of String)
        For Each dataRow As DataRow In dataTable.Rows

            Dim targetDate As String = String.Empty

            ' 来店実績ステータスの判定
            If Not IsDBNull(dataRow("SC_ASSIGNDATE")) AndAlso Not String.IsNullOrEmpty(dataRow("SC_ASSIGNDATE").ToString()) Then

                ' SC振当て日時が設定されている場合、SC振当て日時を経過時間の基準時間とする。
                targetDate = CType(dataRow("SC_ASSIGNDATE").ToString(), Date)

            ElseIf Not IsDBNull(dataRow("STOPTIME")) AndAlso Not String.IsNullOrEmpty(dataRow("STOPTIME").ToString()) Then

                ' 商談中断時間が設定されている場合、商談中断時間を経過時間の基準時間とする。
                targetDate = CType(dataRow("STOPTIME").ToString(), Date)
            Else

                ' 商談中断時間が設定されていない場合、来店時間を経過時間の基準時間とする。
                targetDate = CType(dataRow("VISITTIMESTAMP").ToString(), Date)

            End If

            timeSpanList.Add(CType(Math.Round(nowDate.Subtract(targetDate).TotalSeconds), String))
        Next

        Dim javaScript As New JavaScriptSerializer
        Return javaScript.Serialize(timeSpanList)

    End Function

    ''' <summary>
    ''' 経過時間リスト作成(接客中エリア)
    ''' </summary>
    ''' <param name="dataTable">データテーブル</param>
    ''' <param name="nowDate">現在日時</param>
    ''' <returns>経過時間のリスト</returns>
    ''' <remarks></remarks>
    Private Function GetNegotiationTimeSpanListString(ByRef dataTable As DataTable, ByVal nowDate As Date) As String

        Dim timeSpanList As New List(Of String)
        For Each dataRow As DataRow In dataTable.Rows

            Dim targetDate As String = String.Empty

            ' 来店実績ステータスの判定
            If dataRow("VISITSTATUS").ToString() = VisitStatusNegotiateStop Then

                ' 商談中断中の場合、商談中断日時を経過時間の基準時間とする。
                targetDate = CType(dataRow("STOPTIME").ToString(), Date)
            Else

                '  商談中断中(商談中・納車作業中)以外の場合商談開始時間を経過時間の基準時間とする。
                targetDate = CType(dataRow("SALESSTART").ToString(), Date)
            End If

            timeSpanList.Add(CType(Math.Round(nowDate.Subtract(targetDate).TotalSeconds), String))
        Next

        Dim javaScript As New JavaScriptSerializer
        Return javaScript.Serialize(timeSpanList)

    End Function

    ' $02 end   新車タブレットショールーム管理機能開発

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

        '空白の値は"-"を返す
        If String.IsNullOrEmpty(target) Then
            Return DataNull
        End If

        '空白のみの場合は"-"を返す
        If String.IsNullOrEmpty(target.Trim()) Then
            Return DataNull
        End If

        Dim resultTarget As String
        resultTarget = Server.HtmlDecode(target)

        If length < resultTarget.Length Then

            Dim cutLength As Integer = 0

            '文字列の加工
            ' 「...」表示はスタイルシートで行うため文字列カットをしない
            If StringCut.Equals(kind) Then
                resultTarget = Left(resultTarget, length)
            End If

        End If

        Return Server.HtmlEncode(resultTarget)

    End Function

#End Region

#End Region

End Class
