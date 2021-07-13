'━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
'SC3100201.aspx.vb
'──────────────────────────────────
'機能： 未対応来店客
'補足： 
'作成： 2011/12/12 KN  k.nagasawa
'更新： 2012/02/14 KN  y.nakamura STEP2開発 $01
'更新： 2012/08/27 TMEJ m.okamura 新車受付機能改善 $02
'更新： 2013/02/27 TMEJ t.shimamura 新車タブレット受付画面管理指標変更対応 $03
'──────────────────────────────────

'Option Strict On
'Option Explicit On

Imports Toyota.eCRB.SystemFrameworks.Core
Imports Toyota.eCRB.Visit.Api.BizLogic
Imports Toyota.eCRB.Visit.NotDealCustomer.BizLogic
Imports Toyota.eCRB.Visit.NotDealCustomer.DataAccess
Imports Toyota.eCRB.SystemFrameworks.Core.iCROP.BizLogic
Imports Toyota.eCRB.SystemFrameworks.Core.iCROP.DataAccess

Partial Class Pages_SC3100201
    Inherits BasePage

#Region "画面遷移キー"

    ''' <summary>
    ''' 顧客詳細画面へのセッションキー - 来店実績連番
    ''' </summary>
    ''' <remarks></remarks>
    Private Const SessionKeyVisitSeq As String = "SearchKey.VISITSEQ"

    ''' <summary>
    ''' 顧客詳細画面へのセッションキー - 顧客種別(区分)
    ''' </summary>
    ''' <remarks></remarks>
    Private Const SessionKeyCustomerKind As String = "SearchKey.CSTKIND"

    ''' <summary>
    ''' 顧客詳細画面へのセッションキー - 顧客分類
    ''' </summary>
    ''' <remarks></remarks>
    Private Const SessionKeyCustomerClass As String = "SearchKey.CUSTOMERCLASS"

    ''' <summary>
    ''' 顧客詳細画面へのセッションキー - 活動先顧客コード
    ''' </summary>
    ''' <remarks></remarks>
    Private Const SessionKeyCustomerId As String = "SearchKey.CRCUSTID"

#End Region

#Region "DB関連"

    ''' <summary>
    ''' システム環境設定マスタ - パラメータ名:顧客写真の保存先フォルダ
    ''' </summary>
    ''' <remarks></remarks>
    Private Const SystemEnvFacePicUploadUrl As String = "FACEPIC_UPLOADURL"

    ''' <summary>
    ''' システム環境設定マスタ - パラメータ名:敬称表示位置
    ''' </summary>
    ''' <remarks></remarks>
    Private Const SystemEnvKeisyoZengo As String = "KEISYO_ZENGO"

    ' $01 start step2開発
    ''' <summary>
    ''' システム環境設定マスタ - パラメータ名:苦情情報日数
    ''' </summary>
    ''' <remarks></remarks>
    Private Const ComplaintDisplayDate As String = "COMPLAINT_DISPLAYDATE"
    ' $01 end   step2開発

    ''' <summary>
    ''' 販売店環境設定マスタ - パラメータ名:対応スタッフ写真の保存先フォルダ
    ''' </summary>
    ''' <remarks></remarks>
    Private Const DealerEnvFilePathStaffPhoto As String = "URI_STAFFPHOTO"

    ''' <summary>
    ''' 販売店環境設定マスタ - パラメータ名:未対応来店客の未対応警告時間
    ''' </summary>
    ''' <remarks></remarks>
    Private Const DealerEnvAlertSpan As String = "NOTDEAL_TIME_ALERT_SPAN"

    ''' <summary>
    ''' 敬称表示位置:前
    ''' </summary>
    ''' <remarks></remarks>
    Private Const SystemEnvKeisyoZengoMae As String = "1"

    ''' <summary>
    ''' 敬称表示位置:後
    ''' </summary>
    ''' <remarks></remarks>
    Private Const SystemEnvKeisyoZengoUshiro As String = "2"

    ''' <summary>
    ''' 来店手段：車
    ''' </summary>
    ''' <remarks></remarks>
    Private Const VisitMeansCar As String = "1"

    ''' <summary>
    ''' 来店実績ステータス - フリー
    ''' </summary>
    Private Const VisitStatusFree As String = "01"

    ''' <summary>
    ''' 来店実績ステータス - フリー(ブロードキャスト)
    ''' </summary>
    Private Const VisitStatusFreeBroud As String = "02"

    ''' <summary>
    ''' 来店実績ステータス - 調整中
    ''' </summary>
    Private Const VisitStatusAdjust As String = "03"

    ''' <summary>
    ''' 来店実績ステータス - 確定(ブロードキャスト)
    ''' </summary>
    Private Const VisitStatusDefinitionBroud As String = "04"

    ''' <summary>
    ''' 来店実績ステータス - 確定
    ''' </summary>
    Private Const VisitStatusDefinition As String = "05"

    ''' <summary>
    ''' 来店実績ステータス - 待ち
    ''' </summary>
    Private Const VisitStatusWait As String = "06"

    ''' <summary>
    ''' 来店実績ステータス - 商談中
    ''' </summary>
    Private Const VisitStatusSalesStart As String = "07"

    ' $02 start 複数顧客に対する商談平行対応
    ''' <summary>
    ''' 来店実績ステータス - 商談中断
    ''' </summary>
    ''' <remarks></remarks>
    Private Const VisitStatusNegotiateStop As String = "09"
    ' $02 end   複数顧客に対する商談平行対応

    '$03 start 納車作業ステータス対応
    ''' <summary>
    ''' 来店実績ステータス - 納車作業中
    ''' </summary>
    ''' <remarks></remarks>
    Private Const VisitStatusDeliverlyStart As String = "11"

    '$03 end 納車作業ステータス対応

    ''' <summary>
    ''' 在席状態（大分類）（スタンバイ）
    ''' </summary>
    ''' <remarks></remarks>
    Private Const PresenceCategoryStandby As String = "1"

    ''' <summary>
    ''' 在席状態（大分類）（商談中）
    ''' </summary>
    ''' <remarks></remarks>
    Private Const PresenceCategorySalesPending As String = "2"

    ''' <summary>
    ''' 在席状態（小分類）
    ''' </summary>
    ''' <remarks></remarks>
    Private Const PresenceDetailSub As String = "1"

#End Region

#Region "文言ID(項目)"

    ''' <summary>
    ''' 文言ID - 項目：未対応来店客
    ''' </summary>
    ''' <remarks></remarks>
    Private Const WordIdScreenTitle As Integer = 1

    ''' <summary>
    ''' 文言ID - 項目：来店時間
    ''' </summary>
    ''' <remarks></remarks>
    Private Const WordIdVisitTimestamp As Integer = 2

    ''' <summary>
    ''' 文言ID - 項目：お客様情報
    ''' </summary>
    ''' <remarks></remarks>
    Private Const WordIdCustomerInfo As Integer = 3

    ''' <summary>
    ''' 文言ID - 項目：対応状況
    ''' </summary>
    ''' <remarks></remarks>
    Private Const WordIdDealStatus As Integer = 4

    ''' <summary>
    ''' 文言ID - 項目：参考
    ''' </summary>
    ''' <remarks></remarks>
    Private Const WordIdReference As Integer = 5

    ''' <summary>
    ''' 文言ID - 項目：ご案内依頼
    ''' </summary>
    ''' <remarks></remarks>
    Private Const WordIdNotice As Integer = 6

    ''' <summary>
    ''' 文言ID - 項目：フリー
    ''' </summary>
    ''' <remarks></remarks>
    Private Const WordIdVisitStatusFree As Integer = 7

    ''' <summary>
    ''' 文言ID - 項目：調整中
    ''' </summary>
    ''' <remarks></remarks>
    Private Const WordIdVisitStatusAdjust As Integer = 8

    ''' <summary>
    ''' 文言ID - 項目：確定
    ''' </summary>
    ''' <remarks></remarks>
    Private Const WordIdVisitStatusDefinition As Integer = 9

    ''' <summary>
    ''' 文言ID - 項目：待ち
    ''' </summary>
    ''' <remarks></remarks>
    Private Const WordIdVisitStatusWait As Integer = 10

    ''' <summary>
    ''' 文言ID - 項目：商談中
    ''' </summary>
    ''' <remarks></remarks>
    Private Const WordIdVisitStatusSalesStart As Integer = 11

    ' $02 start 複数顧客に対する商談平行対応
    ''' <summary>
    ''' 文言ID - 項目：商談中断
    ''' </summary>
    ''' <remarks></remarks>
    Private Const WordIdVisitStatusNegotiateStop As Integer = 15
    ' $02 end   複数顧客に対する商談平行対応

    ' $03 start 納車作業ステータス対応
    ''' <summary>
    ''' 文言ID - 項目：納車作業中
    ''' </summary>
    ''' <remarks></remarks>
    Private Const WordIdVisitStatusDeliverlyStart As Integer = 16
    ' $03 end   納車作業ステータス対応

    ''' <summary>
    ''' 文言ID - 項目：新規お客様
    ''' </summary>
    ''' <remarks></remarks>
    Private Const WordIdNewCustomer As Integer = 12

    ''' <summary>
    ''' 文言ID - 項目：Unknown(既存お客様の氏名がない)
    ''' </summary>
    ''' <remarks></remarks>
    Private Const WordIdUnknown As Integer = 13

    ' $01 start step2開発
    ''' <summary>
    ''' 文言ID - 項目：苦情アイコン
    ''' </summary>
    ''' <remarks></remarks>
    Private Const ClaimIcon As Integer = 14
    ' $01 end   step2開発

    ''' <summary>
    ''' 文言ID - 項目：画面に表示するデータがない
    ''' </summary>
    ''' <remarks></remarks>
    Private Const WordIdNotVisit As Integer = 21

#End Region

#Region "文言ID(メッセージ)"

    ''' <summary>
    ''' 文言ID - メッセージ：受付係によって、既に他のSCに割り当てられた状態
    ''' </summary>
    ''' <remarks></remarks>
    Private Const WordIdAlreadyUpdated As Integer = 901

    ''' <summary>
    ''' 文言ID - メッセージ：SCの返答に対するデータ更新処理時のDBタイムアウト
    ''' </summary>
    ''' <remarks></remarks>
    Private Const WordIdDBTimeout As Integer = 902

    ''' <summary>
    ''' 文言ID - メッセージ：SCの返答に対するデータ更新処理時の排他エラー
    ''' </summary>
    ''' <remarks></remarks>
    Private Const WordIdConcurrencyViolation As Integer = 903

#End Region

#Region "処理定数"

    ''' <summary>
    ''' 未設定値の表示
    ''' </summary>
    ''' <remarks></remarks>
    Private Const DefaultValue As String = "-"

    ' $02 start 複数顧客に対する商談平行対応
    ''' <summary>
    ''' 時間の値が存在しない場合の設定値
    ''' </summary>
    ''' <remarks></remarks>
    Private Const NothingDate As String = "--:--"
    ' $02 end   複数顧客に対する商談平行対応

    ''' <summary>
    ''' 顧客名と敬称の間のスペース
    ''' </summary>
    ''' <remarks></remarks>
    Private Const NameTitleSpace As String = " "

    ''' <summary>
    ''' 顧客・スタッフのシルエットアイコン
    ''' </summary>
    ''' <remarks></remarks>
    Private Const SilhouettePerson As String = "../Styles/Images/SC3100201/silhouette_person.png"

    ''' <summary>
    ''' ブロードキャストアイコン
    ''' </summary>
    ''' <remarks></remarks>
    Private Const IconBroudcast As String = "../Styles/Images/SC3100201/icon_broudcast.png"

    ''' <summary>
    ''' 文字カット数 - 未対応来店客一覧：ヘッダ項目：来店時間
    ''' </summary>
    ''' <remarks></remarks>
    Private Const DispLengthHeaderVisitTime As Integer = 4

    ''' <summary>
    ''' 文字カット数 - 未対応来店客一覧：ヘッダ項目：お客様情報
    ''' </summary>
    ''' <remarks></remarks>
    Private Const DispLengthHeaderCustomerInfo As Integer = 22

    ''' <summary>
    ''' 文字カット数 - 未対応来店客一覧：ヘッダ項目：対応状況
    ''' </summary>
    ''' <remarks></remarks>
    Private Const DispLengthHeaderDealStatus As Integer = 18

    ''' <summary>
    ''' 文字カット数 - 参考情報一覧：ヘッダ項目：参考
    ''' </summary>
    ''' <remarks></remarks>
    Private Const DispLengthHeaderReference As Integer = 44

    ''' <summary>
    ''' 文字カット数 - お客様名
    ''' </summary>
    ''' <remarks></remarks>
    Private Const DispLengthCustomerName As Integer = 7

    ''' <summary>
    ''' 文字カット数 - 車両登録No.
    ''' </summary>
    ''' <remarks></remarks>
    Private Const DispLengthVclRegNo As Integer = 6

    ''' <summary>
    ''' 文字カット数 - 顧客担当スタッフ名
    ''' </summary>
    ''' <remarks></remarks>
    Private Const DispLengthCustomerStaff As Integer = 5

    ''' <summary>
    ''' 文字カット数 - 対応担当スタッフ名
    ''' </summary>
    ''' <remarks></remarks>
    Private Const DispLengthDealStaff As Integer = 3

    ''' <summary>
    ''' 文字カット数 - お客様対応状況
    ''' </summary>
    ''' <remarks></remarks>
    Private Const DispLengthReferenceStatus As Integer = 12

    ''' <summary>
    ''' スタッフ写真用パスの先頭に設定する文字列
    ''' </summary>
    ''' <remarks></remarks>
    Private Const StaffPhotoPathPrefix As String = "~/"

#End Region

#Region "初期表示"

    ''' <summary>
    ''' ロード時の処理を実施します。
    ''' </summary>
    ''' <param name="sender">イベント発生元</param>
    ''' <param name="e">イベントデータ</param>
    ''' <remarks></remarks>
    Private Sub Page_Load(sender As Object, e As System.EventArgs) Handles Me.Load

        ' 処理時はパネルを非表示
        Me.Panel_PageInit.Visible = False
        Me.Panel_Redirect.Visible = False
        Me.Panel_NotVisitorList.Visible = False
        Me.Panel_VisitorList.Visible = False

        ' PostBack時、初期表示処理は行わない。
        If Me.IsPostBack Then

            Return
        End If

        ' 初期表示処理
        Me.Panel_PageInit.Visible = True

    End Sub

    ''' <summary>
    ''' 初期表示処理を実施します。
    ''' </summary>
    ''' <param name="sender">イベント発生元</param>
    ''' <param name="e">イベントデータ</param>
    ''' <remarks></remarks>
    Protected Sub PageInitButton_Click(sender As Object, e As System.EventArgs) Handles PageInitButton.Click

        ' 初期表示処理
        PageInit()
    End Sub

#End Region

#Region "顧客詳細画面への遷移イベント"

    ''' <summary>
    ''' 顧客詳細画面への遷移処理を実施します。
    ''' </summary>
    ''' <param name="sender">イベント発生元</param>
    ''' <param name="e">イベントデータ</param>
    ''' <remarks></remarks>
    Protected Sub RedirectButton_Click(sender As Object, e As System.EventArgs) Handles RedirectButton.Click

        Logger.Info("RedirectButton_Click_Start ")

        '　顧客詳細画面(SC3080201)に遷移
        ' NEXTセッション領域に情報を設定
        ' 来店実績連番
        Logger.Info("RedirectButton_Click_001 " & "Call_Start Me.SetValue Param[ScreenPos.Next, " & SessionKeyVisitSeq & ", " & SelectedVisitSeq.Value & "]")
        Me.SetValue(ScreenPos.Next, SessionKeyVisitSeq, SelectedVisitSeq.Value)
        Logger.Info("RedirectButton_Click_001 " & "Call_End Me.SetValue")

        ' 自社客・未取引客の場合
        If Not String.IsNullOrEmpty(SelectedCustomerId.Value) Then

            ' お客様区分(顧客区分)
            ' お客様分類(顧客分類)
            ' お客様ID(顧客コード)
            Logger.Info("RedirectButton_Click_002 " & "Call_Start Me.SetValue Param[ScreenPos.Next, " & SessionKeyCustomerKind & ", " & SelectedCustomerSegment.Value & "]")
            Me.SetValue(ScreenPos.Next, SessionKeyCustomerKind, SelectedCustomerSegment.Value)
            Logger.Info("RedirectButton_Click_002 " & "Call_End Me.SetValue")

            Logger.Info("RedirectButton_Click_003 " & "Call_Start Me.SetValue Param[ScreenPos.Next, " & SessionKeyCustomerClass & ", " & SelectedCustomerClass.Value & "]")
            Me.SetValue(ScreenPos.Next, SessionKeyCustomerClass, SelectedCustomerClass.Value)
            Logger.Info("RedirectButton_Click_003 " & "Call_End Me.SetValue")

            Logger.Info("RedirectButton_Click_004 " & "Call_Start Me.SetValue Param[ScreenPos.Next, " & SessionKeyCustomerId & ", " & SelectedCustomerId.Value & "]")
            Me.SetValue(ScreenPos.Next, SessionKeyCustomerId, SelectedCustomerId.Value)
            Logger.Info("RedirectButton_Click_004 " & "Call_End Me.SetValue")

        End If

        ' 遷移処理(親フレームに遷移する)
        Logger.Info("RedirectButton_Click_005 " & "Call_Start Me.RedirectNextScreen Param[SC3080201]")
        Me.RedirectNextScreen("SC3080201")
        Logger.Info("RedirectButton_Click_005 " & "Call_End Me.RedirectNextScreen")

        Logger.Info("RedirectButton_Click_End ")
    End Sub

#End Region

#Region "顧客写真のクリックイベント"

    ''' <summary>
    ''' 顧客写真のクリック処理を実施します。
    ''' </summary>
    ''' <param name="sender">イベント発生元</param>
    ''' <param name="e">イベントデータ</param>
    ''' <remarks></remarks>
    Protected Sub ButtonCustomer_Click(sender As Object, e As System.EventArgs) Handles ButtonCustomer.Click
        Logger.Info("ButtonCustomer_Click_Start ")

        ' 選択されたRepeaterItem
        Dim visitItem As RepeaterItem = NotDealVisitList.Items(SelectedItemIndex.Value)

        ' 来店実績連番・更新日時
        Dim visitSeq As Long = CType(visitItem.FindControl("visitSeq"), HiddenField).Value

        ' 来店実績情報の取得
        Dim businessLogic As SC3100201BusinessLogic = New SC3100201BusinessLogic
        Dim visit As SC3100201DataSet.VisitSalesRow = businessLogic.GetVisit(visitSeq)

        ' 事前条件：来店実績情報は取得できる
        Logger.Info("ButtonCustomer_Click_001 " & DirectCast(IIf(visit Is Nothing, "visit Is Nothing", "visit IsNot Nothing"), String))

        ' ログイン情報管理（アカウントの取得）
        ' Logger.Debug("ButtonCustomer_Click_002 " & "Call_Start StaffContext.Current")
        Dim loginStaff As StaffContext = StaffContext.Current
        ' Logger.Debug("ButtonCustomer_Click_002 " & "Call_End   StaffContext.Current")

        ' 対応担当アカウントとログインアカウントが異なる場合
        If String.IsNullOrEmpty(visit.DEALSTAFFCD) _
            OrElse Not String.Equals(visit.DEALSTAFFCD, loginStaff.Account) Then
            ' Logger.Debug("ButtonCustomer_Click_003 " & "AlreadyUpdated MessageId[" & WordIdAlreadyUpdated & "]")

            Me.ShowMessageBox(WordIdAlreadyUpdated)

            ' 初期表示処理
            PageInit()

            Logger.Info("ButtonCustomer_Click_End ")
            Return
        End If

        ' $02 start 複数顧客に対する商談平行対応
        ' 来店実績ステータスが「調整中」、「確定(ブロードキャスト)」、「確定」、「待ち」、「商談中断」以外の場合
        Dim visitStatus As String = visit.VISITSTATUS
        If Not String.Equals(visitStatus, VisitStatusAdjust) _
            AndAlso Not String.Equals(visitStatus, VisitStatusDefinitionBroud) _
            AndAlso Not String.Equals(visitStatus, VisitStatusDefinition) _
            AndAlso Not String.Equals(visitStatus, VisitStatusWait) _
            AndAlso Not String.Equals(visitStatus, VisitStatusNegotiateStop) Then
            ' $02 start 複数顧客に対する商談平行対応
            ' Logger.Debug("ButtonCustomer_Click_004 " & "AlreadyUpdated MessageId[" & WordIdAlreadyUpdated & "]")

            Me.ShowMessageBox(WordIdAlreadyUpdated)

            ' 初期表示処理
            PageInit()

            Logger.Info("ButtonCustomer_Click_End ")
            Return
        End If


        Logger.Info("ButtonCustomer_Click_005 " & "Ready Redirect[SC3080201]")

        ' 顧客詳細画面(SC3080201)に遷移するための情報を設定する
        ' 来店実績連番
        SelectedVisitSeq.Value = visitSeq

        ' 自社客・未取引客の場合
        If Not visit.IsCUSTIDNull Then

            ' お客様区分(顧客区分)
            ' お客様分類(顧客分類)
            ' お客様ID(顧客コード)
            SelectedCustomerSegment.Value = visit.CUSTSEGMENT
            SelectedCustomerClass.Value = visit.CUSTCLASS
            SelectedCustomerId.Value = visit.CUSTID

        End If

        ' 遷移処理(親フレームに遷移するためのフォーム処理を実施)
        Me.Panel_Redirect.Visible = True

        Logger.Info("ButtonCustomer_Click_End ")
    End Sub

#End Region

#Region "了解ボタンのクリックイベント"

    ''' <summary>
    ''' 了解ボタンのクリック処理を実施します。
    ''' </summary>
    ''' <param name="sender">イベント発生元</param>
    ''' <param name="e">イベントデータ</param>
    ''' <remarks></remarks>
    Protected Sub ButtonConsent_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles ButtonConsent.Click
        Logger.Info("ButtonConsent_Click_Start ")

        ' 選択されたRepeaterItem
        Dim visitItem As RepeaterItem = NotDealVisitList.Items(SelectedItemIndex.Value)

        ' 来店実績連番・更新日時
        Dim visitSeq As Long = CType(visitItem.FindControl("visitSeq"), HiddenField).Value
        Dim updateDate As String = CType(visitItem.FindControl("updateDate"), HiddenField).Value

        ' 来店実績情報の取得
        Dim businessLogic As SC3100201BusinessLogic = New SC3100201BusinessLogic
        Dim visit As SC3100201DataSet.VisitSalesRow = businessLogic.GetVisit(visitSeq)

        ' 事前条件：来店実績情報は取得できる
        Logger.Info("ButtonConsent_Click_001 " & DirectCast(IIf(visit Is Nothing, "visit Is Nothing", "visit IsNot Nothing"), String))

        ' ログイン情報管理（アカウントの取得）
        ' Logger.Debug("ButtonConsent_Click_002 " & "Call_Start StaffContext.Current")
        Dim loginStaff As StaffContext = StaffContext.Current
        ' Logger.Debug("ButtonConsent_Click_002 " & "Call_End   StaffContext.Current")

        Dim afterVisitStatus As String = Nothing

        ' 来店客の対応処理
        Dim visitStatus As String = visit.VISITSTATUS
        If String.Equals(visitStatus, VisitStatusFreeBroud) Then
            ' 来店実績ステータスが「フリー(ブロードキャスト)」の場合
            ' Logger.Debug("ButtonConsent_Click_003 " & "VisitStatusFreeBroud")

            ' 対応依頼通知の存在有無
            If Not businessLogic.ExistsVisitDealRequestNotice(visitSeq, loginStaff.Account) Then
                ' Logger.Debug("ButtonConsent_Click_004 " & "AlreadyUpdated MessageId[" & WordIdAlreadyUpdated & "]")

                Me.ShowMessageBox(WordIdAlreadyUpdated)

                ' 初期表示処理
                PageInit()

                Logger.Info("ButtonConsent_Click_End ")
                Return
            End If

            ' Logger.Debug("ButtonConsent_Click_005 " & "DealVisit")

            ' 来店客対応処理
            afterVisitStatus = VisitStatusDefinitionBroud
            Dim messageIdBroadcast As Integer = businessLogic.UpdateVisitCustomer(visitSeq, _
                    afterVisitStatus, True, loginStaff.Account, loginStaff.Account, _
                    loginStaff.DlrCD, loginStaff.BrnCD, updateDate)

            ' 来店客対応処理に失敗
            If messageIdBroadcast <> 0 Then
                ' Logger.Debug("ButtonConsent_Click_006 " & "DBTimeout or ConcurrencyViolation MessageId[" & messageIdBroadcast & "]")

                Me.ShowMessageBox(messageIdBroadcast)

                ' 初期表示処理
                PageInit()

                Logger.Info("ButtonConsent_Click_End ")
                Return
            End If

        ElseIf String.Equals(visitStatus, VisitStatusAdjust) _
            OrElse String.Equals(visitStatus, VisitStatusWait) Then
            ' 来店実績ステータスが「調整中」、「待ち」の場合
            ' Logger.Debug("ButtonConsent_Click_007 " & "VisitStatusAdjust OR VisitStatusWait")

            ' 来店実績の「対応担当アカウント」がログインアカウントではない場合
            If String.IsNullOrEmpty(visit.DEALSTAFFCD) _
                OrElse Not String.Equals(visit.DEALSTAFFCD, loginStaff.Account) Then
                ' Logger.Debug("ButtonConsent_Click_008 " & "AlreadyUpdated MessageId[" & WordIdAlreadyUpdated & "]")

                Me.ShowMessageBox(WordIdAlreadyUpdated)

                ' 初期表示処理
                PageInit()

                Logger.Info("ButtonConsent_Click_End ")
                Return
            End If

            ' Logger.Debug("ButtonConsent_Click_009 " & "DealVisit")

            ' 来店客対応処理
            afterVisitStatus = VisitStatusDefinition
            Dim messageIdStaffSpecify As Integer = businessLogic.UpdateVisitCustomer(visitSeq, _
                    afterVisitStatus, False, Nothing, loginStaff.Account, loginStaff.DlrCD, _
                    loginStaff.BrnCD, updateDate)

            ' 来店客対応処理に失敗
            If messageIdStaffSpecify <> 0 Then
                ' Logger.Debug("ButtonConsent_Click_010 " & "DBTimeout or ConcurrencyViolation MessageId[" & messageIdStaffSpecify & "]")

                Me.ShowMessageBox(messageIdStaffSpecify)

                ' 初期表示処理
                PageInit()

                Logger.Info("ButtonConsent_Click_End ")
                Return
            End If

        Else
            ' 来店実績ステータスが「フリー(ブロードキャスト)」、「調整中」、「待ち」以外の場合
            ' Logger.Debug("ButtonConsent_Click_011 " & "AlreadyUpdated MessageId[" & WordIdAlreadyUpdated & "]")

            Me.ShowMessageBox(WordIdAlreadyUpdated)

            ' 初期表示処理
            PageInit()

            Logger.Info("ButtonConsent_Click_End ")
            Return
        End If

        Logger.Info("ButtonConsent_Click_012 " & "Ready Redirect[SC3080201]")

        ' Push送信
        businessLogic.SendPush(loginStaff.DlrCD, loginStaff.BrnCD, visitStatus, afterVisitStatus)

        ' 顧客保持中（商談中、または、営業活動中）の場合、True
        ' Logger.Info(New StringBuilder("loginStaff.PresenceCategory: ").Append( _
        '         loginStaff.PresenceCategory).ToString())
        ' Logger.Info(New StringBuilder("loginStaff.PresenceDetail: ").Append( _
        '         loginStaff.PresenceDetail).ToString())
        Dim isSalesPending As Boolean = _
            PresenceCategorySalesPending.Equals(loginStaff.PresenceCategory) _
            OrElse (PresenceCategoryStandby.Equals(loginStaff.PresenceCategory) _
                    AndAlso PresenceDetailSub.Equals(loginStaff.PresenceDetail))

        ' 商談中の場合、顧客詳細画面へ遷移しない
        If isSalesPending Then

            ' Logger.Debug("ButtonConsent_Click_013")

            ' 初期表示処理
            PageInit()

            Logger.Info("ButtonConsent_Click_End ")
            Return
        End If

        ' 顧客詳細画面(SC3080201)に遷移するための情報を設定する
        ' 来店実績連番
        SelectedVisitSeq.Value = visitSeq

        ' 自社客・未取引客の場合
        If Not visit.IsCUSTIDNull Then

            ' お客様区分(顧客区分)
            ' お客様分類(顧客分類)
            ' お客様ID(顧客コード)
            SelectedCustomerSegment.Value = visit.CUSTSEGMENT
            SelectedCustomerClass.Value = visit.CUSTCLASS
            SelectedCustomerId.Value = visit.CUSTID

        End If

        ' 遷移処理(親フレームに遷移するためのフォーム処理を実施)
        Me.Panel_Redirect.Visible = True
        Me.Panel_NotVisitorList.Visible = False
        Me.Panel_VisitorList.Visible = False

        Logger.Info("ButtonConsent_Click_End ")
    End Sub

#End Region

#Region "待ちボタンのクリックイベント"

    ''' <summary>
    ''' 待ちボタンのクリック処理を実施します。
    ''' </summary>
    ''' <param name="sender">イベント発生元</param>
    ''' <param name="e">イベントデータ</param>
    ''' <remarks></remarks>
    Protected Sub ButtonWait_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles ButtonWait.Click
        Logger.Info("ButtonWait_Click_Start ")

        ' 選択されたRepeaterItem
        Dim visitItem As RepeaterItem = NotDealVisitList.Items(SelectedItemIndex.Value)

        ' 来店実績連番・更新日時
        Dim visitSeq As Long = CType(visitItem.FindControl("visitSeq"), HiddenField).Value
        Dim updateDate As String = CType(visitItem.FindControl("updateDate"), HiddenField).Value

        ' 来店実績情報の取得
        Dim businessLogic As SC3100201BusinessLogic = New SC3100201BusinessLogic
        Dim visit As SC3100201DataSet.VisitSalesRow = businessLogic.GetVisit(visitSeq)

        ' 事前条件：来店実績情報は取得できる
        Logger.Info("ButtonWait_Click_001 " & DirectCast(IIf(visit Is Nothing, "visit Is Nothing", "visit IsNot Nothing"), String))

        ' ログイン情報管理（アカウントの取得）
        ' Logger.Debug("ButtonWait_Click_002 " & "Call_Start StaffContext.Current")
        Dim loginStaff As StaffContext = StaffContext.Current
        ' Logger.Debug("ButtonWait_Click_002 " & "Call_End   StaffContext.Current")

        ' 来店客の対応処理
        Dim visitStatus As String = visit.VISITSTATUS
        If String.Equals(visitStatus, VisitStatusAdjust) _
            OrElse String.Equals(visitStatus, VisitStatusDefinition) Then
            ' 来店実績ステータスが「調整中」、「確定」の場合
            ' Logger.Debug("ButtonWait_Click_003 " & "VisitStatusAdjust OR VisitStatusDefinition")

            ' 来店実績の「対応担当アカウント」がログインアカウントではない場合
            If String.IsNullOrEmpty(visit.DEALSTAFFCD) _
                OrElse Not String.Equals(visit.DEALSTAFFCD, loginStaff.Account) Then
                ' Logger.Debug("ButtonWait_Click_004 " & "AlreadyUpdated MessageId[" & WordIdAlreadyUpdated & "]")

                Me.ShowMessageBox(WordIdAlreadyUpdated)

                ' 初期表示処理
                PageInit()

                Logger.Info("ButtonWait_Click_End ")
                Return
            End If

            ' Logger.Debug("ButtonWait_Click_005 " & "DealVisit")

            ' 来店客対応処理
            Dim messageId As Integer = businessLogic.UpdateVisitCustomer(visitSeq, _
                    VisitStatusWait, False, Nothing, loginStaff.Account, loginStaff.DlrCD, _
                    loginStaff.BrnCD, updateDate)

            ' 来店客対応処理に失敗
            If messageId <> 0 Then
                ' Logger.Debug("ButtonWait_Click_006 " & "DBTimeout or ConcurrencyViolation MessageId[" & messageId & "]")

                Me.ShowMessageBox(messageId)

                ' 初期表示処理
                PageInit()

                Logger.Info("ButtonWait_Click_End ")
                Return
            End If

        Else
            ' 来店実績ステータスが「調整中」、「確定」以外の場合
            ' Logger.Debug("ButtonWait_Click_007 " & "AlreadyUpdated MessageId[" & WordIdAlreadyUpdated & "]")

            Me.ShowMessageBox(WordIdAlreadyUpdated)

            ' 初期表示処理
            PageInit()

            Logger.Info("ButtonWait_Click_End ")
            Return
        End If

        ' Logger.Debug("ButtonWait_Click_008")

        ' Push送信
        businessLogic.SendPush(loginStaff.DlrCD, loginStaff.BrnCD, visitStatus, VisitStatusWait)
        ' 初期表示処理
        PageInit()

        Logger.Info("ButtonWait_Click_End ")
    End Sub

#End Region

#Region "不可ボタンのクリックイベント"

    ''' <summary>
    ''' 不可ボタンのクリック処理を実施します。
    ''' </summary>
    ''' <param name="sender">イベント発生元</param>
    ''' <param name="e">イベントデータ</param>
    ''' <remarks></remarks>
    Protected Sub ButtonNotConsent_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles ButtonNotConsent.Click
        Logger.Info("ButtonNotConsent_Click_Start ")

        ' 選択されたRepeaterItem
        Dim visitItem As RepeaterItem = NotDealVisitList.Items(SelectedItemIndex.Value)

        ' 来店実績連番・更新日時
        Dim visitSeq As Long = CType(visitItem.FindControl("visitSeq"), HiddenField).Value
        Dim updateDate As String = CType(visitItem.FindControl("updateDate"), HiddenField).Value

        ' 来店実績情報の取得
        Dim businessLogic As SC3100201BusinessLogic = New SC3100201BusinessLogic
        Dim visit As SC3100201DataSet.VisitSalesRow = businessLogic.GetVisit(visitSeq)

        ' 事前条件：来店実績情報は取得できる
        Logger.Info("ButtonNotConsent_Click_001 " & DirectCast(IIf(visit Is Nothing, "visit Is Nothing", "visit IsNot Nothing"), String))

        ' ログイン情報管理（アカウントの取得）
        ' Logger.Debug("ButtonNotConsent_Click_002 " & "Call_Start StaffContext.Current")
        Dim loginStaff As StaffContext = StaffContext.Current
        ' Logger.Debug("ButtonNotConsent_Click_002 " & "Call_End   StaffContext.Current")

        ' 来店客の対応処理
        Dim visitStatus As String = visit.VISITSTATUS
        ' $02 start 複数顧客に対する商談平行対応
        If String.Equals(visitStatus, VisitStatusAdjust) _
            OrElse String.Equals(visitStatus, VisitStatusDefinitionBroud) _
            OrElse String.Equals(visitStatus, VisitStatusDefinition) _
            OrElse String.Equals(visitStatus, VisitStatusWait) _
            OrElse String.Equals(visitStatus, VisitStatusNegotiateStop) Then
            ' 来店実績ステータスが「調整中」、「確定（ブロードキャスト）」、「確定」、「待ち」、「商談中断」の場合
            ' $02 end   複数顧客に対する商談平行対応
            ' Logger.Debug("ButtonNotConsent_Click_003 " & "VisitStatusAdjust OR VisitStatusDefinitionBroud OR VisitStatusDefinition OR VisitStatusWait")

            ' 来店実績の「対応担当アカウント」がログインアカウントではない場合
            If String.IsNullOrEmpty(visit.DEALSTAFFCD) _
                OrElse Not String.Equals(visit.DEALSTAFFCD, loginStaff.Account) Then
                ' Logger.Debug("ButtonNotConsent_Click_004 " & "AlreadyUpdated MessageId[" & WordIdAlreadyUpdated & "]")

                Me.ShowMessageBox(WordIdAlreadyUpdated)

                ' 初期表示処理
                PageInit()

                Logger.Info("ButtonNotConsent_Click_End ")
                Return
            End If

            ' Logger.Debug("ButtonNotConsent_Click_005 " & "DealVisit")

            ' 来店客対応処理
            Dim messageId As Integer = businessLogic.UpdateVisitCustomer(visitSeq, _
                    VisitStatusFree, True, Nothing, loginStaff.Account, loginStaff.DlrCD, _
                    loginStaff.BrnCD, updateDate)

            ' 来店客対応処理に失敗
            If messageId <> 0 Then
                ' Logger.Debug("ButtonNotConsent_Click_006 " & "DBTimeout or ConcurrencyViolation MessageId[" & messageId & "]")

                Me.ShowMessageBox(messageId)

                ' 初期表示処理
                PageInit()

                Logger.Info("ButtonNotConsent_Click_End ")
                Return
            End If

        Else
            ' 来店実績ステータスが「調整中」、「確定（ブロードキャスト）」、「確定」、「待ち」以外の場合
            ' Logger.Debug("ButtonNotConsent_Click_007 " & "AlreadyUpdated MessageId[" & WordIdAlreadyUpdated & "]")

            Me.ShowMessageBox(WordIdAlreadyUpdated)

            ' 初期表示処理
            PageInit()

            Logger.Info("ButtonNotConsent_Click_End ")
            Return
        End If

        ' Logger.Debug("ButtonNotConsent_Click_008 ")

        ' Push送信
        businessLogic.SendPush(loginStaff.DlrCD, loginStaff.BrnCD, visitStatus, VisitStatusFree)
        ' 初期表示処理
        PageInit()

        Logger.Info("ButtonNotConsent_Click_End ")
    End Sub

#End Region

#Region "一覧のヘッダ項目設定イベント"

    ''' <summary>
    ''' 未対応来店客一覧のヘッダ項目設定を行う。
    ''' </summary>
    ''' <param name="sender">イベント発生元</param>
    ''' <param name="e">イベントデータ</param>
    ''' <remarks></remarks>
    Protected Sub NotDealVisitList_ItemDataBound(ByVal sender As Object, _
                                                 ByVal e As System.Web.UI.WebControls.RepeaterItemEventArgs _
                                                 ) Handles NotDealVisitList.ItemDataBound
        ' Logger.Info("NotDealVisitList_ItemDataBound_Start ")

        ' 引数チェック
        If e Is Nothing Then

            ' Logger.Info("NotDealVisitList_ItemDataBound_End ")
            Return
        End If

        Dim visitUtility As New VisitUtility

        ' HeaderTemplateに対するDataBoundイベントか判定
        If e.Item.ItemType = ListItemType.Header Then

            ' 来店時間
            ' Logger.Debug("NotDealVisitList_ItemDataBound_001 " & "Call_Start WebWordUtility.GetWord Param[" & WordIdVisitTimestamp & "]")
            CType(e.Item.FindControl("Header_VisitTime"), Label).Text = HttpUtility.HtmlEncode( _
                    visitUtility.CutTailString(WebWordUtility.GetWord(WordIdVisitTimestamp), _
                    DispLengthHeaderVisitTime, False))
            ' Logger.Debug("NotDealVisitList_ItemDataBound_001 " & "Call_End WebWordUtility.GetWord")

            ' お客様情報
            ' Logger.Debug("NotDealVisitList_ItemDataBound_002 " & "Call_Start WebWordUtility.GetWord Param[" & WordIdCustomerInfo & "]")
            CType(e.Item.FindControl("Header_CustInfo"), Label).Text = HttpUtility.HtmlEncode( _
                    visitUtility.CutTailString(WebWordUtility.GetWord(WordIdCustomerInfo), _
                    DispLengthHeaderCustomerInfo, False))
            ' Logger.Debug("NotDealVisitList_ItemDataBound_002 " & "Call_End WebWordUtility.GetWord")

            ' 対応状況
            ' Logger.Debug("NotDealVisitList_ItemDataBound_003 " & "Call_Start WebWordUtility.GetWord Param[" & WordIdDealStatus & "]")
            CType(e.Item.FindControl("Header_DealStatus"), Label).Text = HttpUtility.HtmlEncode( _
                    visitUtility.CutTailString(WebWordUtility.GetWord(WordIdDealStatus), _
                    DispLengthHeaderDealStatus, False))
            ' Logger.Debug("NotDealVisitList_ItemDataBound_003 " & "Call_End WebWordUtility.GetWord")

        End If

        ' Logger.Info("NotDealVisitList_ItemDataBound_End ")
    End Sub

    ''' <summary>
    ''' 参考情報一覧のヘッダ項目設定を行う。
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e">イベント</param>
    ''' <remarks></remarks>
    Protected Sub ReferenceVisitList_ItemDataBound(ByVal sender As Object, _
                                                   ByVal e As System.Web.UI.WebControls.RepeaterItemEventArgs _
                                                   ) Handles ReferenceVisitList.ItemDataBound
        ' Logger.Info("ReferenceVisitList_ItemDataBound_Start ")

        ' 引数チェック
        If e Is Nothing Then

            ' Logger.Info("ReferenceVisitList_ItemDataBound_End ")
            Return
        End If

        Dim visitUtility As New VisitUtility

        ' HeaderTemplateに対するDataBoundイベントか判定
        If e.Item.ItemType = ListItemType.Header Then

            ' 参考
            ' Logger.Debug("ReferenceVisitList_ItemDataBound_001 " & "Call_Start WebWordUtility.GetWord Param[" & WordIdReference & "]")
            CType(e.Item.FindControl("Header_Reference"), Label).Text = HttpUtility.HtmlEncode( _
                    visitUtility.CutTailString(WebWordUtility.GetWord(WordIdReference), _
                    DispLengthHeaderReference, False))
            ' Logger.Debug("ReferenceVisitList_ItemDataBound_001 " & "Call_End WebWordUtility.GetWord")

        End If

        ' Logger.Info("ReferenceVisitList_ItemDataBound_End ")
    End Sub

#End Region

#Region "Private関数"

    ''' <summary>
    ''' ページの初期表示処理
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub PageInit()

        ' Logger.Debug("PageInit_Start")

        ' ログイン情報管理（販売店コード、店舗コード、アカウントの取得）
        ' Logger.Debug("PageInit_001 " & "Call_Start StaffContext.Current")
        Dim loginStaff As StaffContext = StaffContext.Current
        ' Logger.Debug("PageInit_001 " & "Call_End   StaffContext.Current")

        ' $01 start step2開発
        ' システム環境設定
        ' Logger.Info("PageInit_002 " & "New SystemEnvSetting")
        Dim sysEnvSet As New SystemEnvSetting

        ' システム環境設定（苦情表示日数の取得）
        Dim sysEnvSetComplaintDisplayDateRow As SystemEnvSettingDataSet.SYSTEMENVSETTINGRow = Nothing
        Logger.Info("PageInit_003 " & "Call_Start GetSystemEnvSetting Param[" & ComplaintDisplayDate & "]")
        sysEnvSetComplaintDisplayDateRow = sysEnvSet.GetSystemEnvSetting(ComplaintDisplayDate)
        Logger.Info("PageInit_003 " & "Call_End GetSystemEnvSetting Ret[" & IsDBNull(sysEnvSetComplaintDisplayDateRow) & "]")
        Dim complaintDateCount As Integer = CType(sysEnvSetComplaintDisplayDateRow.PARAMVALUE, Integer)

        sysEnvSet = Nothing
        sysEnvSetComplaintDisplayDateRow = Nothing
        ' $01 end   step2開発

        ' 日付管理(現在日付の取得)
        ' Logger.Debug("PageInit_004 " & "Call_Start DateTimeFunc.Now Param[" & loginStaff.DlrCD & "]")
        Dim now As Date = DateTimeFunc.Now(loginStaff.DlrCD)
        ' Logger.Debug("PageInit_004 " & "Call_End   DateTimeFunc.Now Ret[" & now & "]")

        ' 画面表示情報の取得
        Dim businessLogic As SC3100201BusinessLogic = New SC3100201BusinessLogic

        ' $01 start step2開発
        ' 未対応来店客一覧の取得
        Dim notDealVisitDt As SC3100201DataSet.NotDealVisitDataTable = _
            businessLogic.GetNotDealVisitCustomer(loginStaff.DlrCD, loginStaff.BrnCD, loginStaff.Account, now, complaintDateCount)

        ' 参考情報一覧の取得
        Dim referenceVisitDt As SC3100201DataSet.NotDealVisitDataTable = _
            businessLogic.GetReferenceVisitCustomer(loginStaff.DlrCD, loginStaff.BrnCD, loginStaff.Account, now, complaintDateCount)
        ' $01 end   step2開発

        ' 未対応来店客一覧と参考情報一覧の件数チェック
        If (notDealVisitDt Is Nothing OrElse 0 >= notDealVisitDt.Count) _
            AndAlso (referenceVisitDt Is Nothing OrElse 0 >= referenceVisitDt.Count) Then

            ' Logger.Debug("PageInit_005 " & "NotVisitorList MessageId[" & WordIdNotVisit & "]")

            ' 来店客がいない旨を表示
            Me.Panel_NotVisitorList.Visible = True
            ' Logger.Debug("PageInit_006 " & "Call_Start WebWordUtility.GetWord Param[" & WordIdNotVisit & "]")
            Me.Label_NotVisit.Text = HttpUtility.HtmlEncode(WebWordUtility.GetWord(WordIdNotVisit))
            ' Logger.Debug("PageInit_006 " & "Call_End WebWordUtility.GetWord")

            ' Logger.Debug("PageInit_End ")
            Return
        End If

        ' Logger.Debug("PageInit_007 " & "VisitorList")

        ' 未対応来店客一覧と参考情報一覧の詳細設定

        ' システム環境設定
        ' Logger.Info("PageInit_008 " & "New SystemEnvSetting")
        sysEnvSet = New SystemEnvSetting

        ' 顧客写真用のパスを取得
        Logger.Info("PageInit_009 " & "Call_Start sysEnvSet.GetSystemEnvSetting Param[" & SystemEnvFacePicUploadUrl & "]")
        Dim sysEnvSetPathRow As SystemEnvSettingDataSet.SYSTEMENVSETTINGRow = _
            sysEnvSet.GetSystemEnvSetting(SystemEnvFacePicUploadUrl)
        Logger.Info("PageInit_009 " & "Call_End sysEnvSet.GetSystemEnvSetting Ret[" & (sysEnvSetPathRow IsNot Nothing) & "]")
        Dim facePicPash As String = sysEnvSetPathRow.PARAMVALUE

        ' 敬称の表示位置を取得
        Logger.Info("PageInit_010 " & "Call_Start sysEnvSet.GetSystemEnvSetting Param[" & SystemEnvKeisyoZengo & "]")
        Dim sysEnvSetZengoRow As SystemEnvSettingDataSet.SYSTEMENVSETTINGRow = _
            sysEnvSet.GetSystemEnvSetting(SystemEnvKeisyoZengo)
        Logger.Info("PageInit_010 " & "Call_End sysEnvSet.GetSystemEnvSetting Ret[" & (sysEnvSetZengoRow IsNot Nothing) & "]")
        Dim keisyoZengo As String = sysEnvSetZengoRow.PARAMVALUE

        sysEnvSetPathRow = Nothing
        sysEnvSetZengoRow = Nothing
        sysEnvSet = Nothing

        ' 販売店環境設定
        ' Logger.Info("PageInit_011 " & "New BranchEnvSetting")
        Dim branchEnvSet As New BranchEnvSetting

        ' 対応スタッフ写真用のパスを取得
        Logger.Info("PageInit_012 " & "Call_Start GetEnvSetting Param[" & loginStaff.DlrCD & ", " & loginStaff.BrnCD & ", " & DealerEnvFilePathStaffPhoto & "]")
        Dim branchEnvSetStaffPhotoPathRow As DlrEnvSettingDataSet.DLRENVSETTINGRow = _
            branchEnvSet.GetEnvSetting(loginStaff.DlrCD, loginStaff.BrnCD, DealerEnvFilePathStaffPhoto)
        Logger.Info("PageInit_012 " & "Call_End GetEnvSetting Ret[" & (branchEnvSetStaffPhotoPathRow IsNot Nothing) & "]")
        Dim staffPicPash As String = branchEnvSetStaffPhotoPathRow.PARAMVALUE

        branchEnvSet = Nothing
        branchEnvSetStaffPhotoPathRow = Nothing

        ' 顧客保持中（商談中、または、営業活動中）でない場合、True
        ' Logger.Info(New StringBuilder("loginStaff.PresenceCategory: ").Append( _
        '         loginStaff.PresenceCategory).ToString())
        ' Logger.Info(New StringBuilder("loginStaff.PresenceDetail: ").Append( _
        '         loginStaff.PresenceDetail).ToString())
        Dim isNotSalesPending As Boolean = Not ( _
            PresenceCategorySalesPending.Equals(loginStaff.PresenceCategory) _
            OrElse (PresenceCategoryStandby.Equals(loginStaff.PresenceCategory) _
                    AndAlso PresenceDetailSub.Equals(loginStaff.PresenceDetail))
            )

        ' $01 start step2開発
        ' 苦情アイコンの文言取得
        ' Logger.Debug("PageInit_013 " & "Call_Start WebWordUtility.GetWord Param[" & ClaimIcon & "]")
        Dim claimIconWord As String = HttpUtility.HtmlEncode(WebWordUtility.GetWord(ClaimIcon))
        ' Logger.Debug("PageInit_013 " & "Call_End WebWordUtility.GetWord")
        ' $01 end   step2開発

        ' 未対応来店客一覧のデータを反映
        Me.NotDealVisitList.DataSource = notDealVisitDt
        Me.NotDealVisitList.DataBind()
        For Each notDealVisit As RepeaterItem In Me.NotDealVisitList.Items
            
            ' $02 start 複数顧客に対する商談平行対応
            Dim dispClass As String = Trim(CType(notDealVisit.FindControl("DispClass"), HiddenField).Value)
            Dim timeClass As New StringBuilder("Time" & dispClass)
            Dim infoClass As New StringBuilder("Info" & dispClass)
            Dim supportClass As New StringBuilder("Support" & dispClass)
            Dim visitStatus As String = Trim(CType(notDealVisit.FindControl("visitStatus"), HiddenField).Value)

            ' 商談中断の場合
            If String.Equals(visitStatus, VisitStatusNegotiateStop) Then
                timeClass.Append(" TimeStop")
                infoClass.Append(" InfoStop")
                supportClass.Append(" SupportStop")
            End If

            CType(notDealVisit.FindControl("tdStartTime"), Literal).Text = "<td class='" & timeClass.ToString() & "'>"
            CType(notDealVisit.FindControl("tdStartInfo"), Literal).Text = "<td class='" & infoClass.ToString() & "'>"
            CType(notDealVisit.FindControl("tdStartSupport"), Literal).Text = "<td class='" & supportClass.ToString() & "'>"

            timeClass = Nothing
            infoClass = Nothing
            supportClass = Nothing
            ' $02 end   複数顧客に対する商談平行対応

            ' $01 start step2開発
            ' 表示項目の詳細設定
            SetInitDetail(notDealVisit, facePicPash, keisyoZengo, staffPicPash, claimIconWord)
            ' $01 end   step2開発

            ' 顧客写真・了解・待ち・不可ボタンの制御
            SetButtonVisible(notDealVisit, isNotSalesPending)

        Next

        ' 参考情報一覧のデータを反映
        Me.ReferenceVisitList.DataSource = referenceVisitDt
        Me.ReferenceVisitList.DataBind()
        For Each referenceVisit As RepeaterItem In Me.ReferenceVisitList.Items
            
            ' $02 start 複数顧客に対する商談平行対応
            Dim timeClass As New StringBuilder("Time02 Reference")
            Dim infoClass As New StringBuilder("Info02 Reference")
            Dim supportClass As New StringBuilder("Support02 Reference")
            Dim visitStatus As String = Trim(CType(referenceVisit.FindControl("visitStatus"), HiddenField).Value)

            ' 商談中断の場合
            If String.Equals(visitStatus, VisitStatusNegotiateStop) Then
                timeClass.Append(" TimeStop")
                infoClass.Append(" InfoStop")
                supportClass.Append(" SupportStop")
            End If

            CType(referenceVisit.FindControl("tdStartTime"), Literal).Text = "<td class='" & timeClass.ToString() & "'>"
            CType(referenceVisit.FindControl("tdStartInfo"), Literal).Text = "<td class='" & infoClass.ToString() & "'>"
            CType(referenceVisit.FindControl("tdStartSupport"), Literal).Text = "<td class='" & supportClass.ToString() & "'>"

            timeClass = Nothing
            infoClass = Nothing
            supportClass = Nothing
            ' $02 end   複数顧客に対する商談平行対応

            ' $01 start step2開発
            ' 表示項目の詳細設定
            SetInitDetail(referenceVisit, facePicPash, keisyoZengo, staffPicPash, claimIconWord)
            ' $01 end   step2開発

            ' お客様対応状況の設定
            SetReferenceStatus(referenceVisit, loginStaff.Account)

        Next

        ' 未対応警告時間の設定
        SetAlertSpan(loginStaff.DlrCD, loginStaff.BrnCD)

        ' 来店客の情報を表示
        Me.Panel_VisitorList.Visible = True

        'Hiddenタグの削除
        DeleteHiddenTag()

        ' Logger.Debug("PageInit_End ")
    End Sub

    ''' <summary>
    ''' 不要なHiddenFieldを削除する
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub DeleteHiddenTag()

        ' 未対応来店客一覧
        For Each notDealVisit As RepeaterItem In Me.NotDealVisitList.Items
            CType(notDealVisit.FindControl("custmerId"), HiddenField).Visible = False
            CType(notDealVisit.FindControl("customerImage"), HiddenField).Visible = False
            CType(notDealVisit.FindControl("visitMeans"), HiddenField).Visible = False
            CType(notDealVisit.FindControl("visitPersonNum"), HiddenField).Visible = False
            CType(notDealVisit.FindControl("salesTableNo"), HiddenField).Visible = False
            CType(notDealVisit.FindControl("dealStaffImage"), HiddenField).Visible = False
            CType(notDealVisit.FindControl("claimInfo"), HiddenField).Visible = False
            CType(notDealVisit.FindControl("visitStatus"), HiddenField).Visible = False
            CType(notDealVisit.FindControl("visitTimestamp"), HiddenField).Visible = False
        Next

        ' 参考情報一覧
        For Each referenceVisit As RepeaterItem In Me.ReferenceVisitList.Items
            CType(referenceVisit.FindControl("custmerId"), HiddenField).Visible = False
            CType(referenceVisit.FindControl("customerImage"), HiddenField).Visible = False
            CType(referenceVisit.FindControl("visitMeans"), HiddenField).Visible = False
            CType(referenceVisit.FindControl("visitPersonNum"), HiddenField).Visible = False
            CType(referenceVisit.FindControl("salesTableNo"), HiddenField).Visible = False
            CType(referenceVisit.FindControl("dealStaffImage"), HiddenField).Visible = False
            CType(referenceVisit.FindControl("claimInfo"), HiddenField).Visible = False
            CType(referenceVisit.FindControl("visitStatus"), HiddenField).Visible = False
            CType(referenceVisit.FindControl("customerStaffId"), HiddenField).Visible = False
        Next

    End Sub

    ''' <summary>
    ''' 表示項目の詳細設定を行う。
    ''' </summary>
    ''' <param name="visitItem">来店実績情報</param>
    ''' <param name="facePicPash">顧客写真用のパス</param>
    ''' <param name="keisyoZengo">敬称前後</param>
    ''' <param name="staffPicPash">スタッフ写真用のパス</param>
    ''' <param name="claimIconWord">苦情アイコン文言</param>
    ''' <remarks></remarks>
    Private Sub SetInitDetail(ByVal visitItem As RepeaterItem, _
                              ByVal facePicPash As String, _
                              ByVal keisyoZengo As String, _
                              ByVal staffPicPash As String, _
                              ByVal claimIconWord As String)
        ' $01 start step2開発
        ' Logger.Debug(New StringBuilder("SetInitDetail_Start Param[").Append(visitItem).Append( _
        '        ", ").Append(facePicPash).Append(", ").Append(keisyoZengo).Append(", ").Append( _
        '        staffPicPash).Append(", ").Append(claimIconWord).Append("]").ToString())
        ' $01 end   step2開発

        ' お客様写真、お客様名
        SetInitCustomerInfo(visitItem, facePicPash, keisyoZengo)
        ' 車両登録No.、来店手段
        SetInitVclRegNoAndVisitMeans(visitItem)

        ' $02 start 複数顧客に対する商談平行対応
        ' 来店時間
        Dim vistTimeStamp As String = Trim(CType(visitItem.FindControl("visitTimestamp"), HiddenField).Value)
        If String.IsNullOrEmpty(vistTimeStamp) Then
            CType(visitItem.FindControl("Label_VisitTimestamp"), Label).Text = NothingDate

        Else
            CType(visitItem.FindControl("Label_VisitTimestamp"), Label).Text = Format(CType(vistTimeStamp, DateTime), "HH:mm")
        End If
        ' $02 end   複数顧客に対する商談平行対応

        ' 来店人数
        Dim visitPersonNum As String = Trim(CType(visitItem.FindControl("visitPersonNum"), HiddenField).Value)
        If String.IsNullOrEmpty(visitPersonNum) OrElse String.Equals(visitPersonNum, "0") Then
            ' Logger.Debug("SetInitDetail_001")
            CType(visitItem.FindControl("Label_VisitPersonNum"), Label).Text = DefaultValue

        Else
            ' Logger.Debug("SetInitDetail_002")
            CType(visitItem.FindControl("Label_VisitPersonNum"), Label).Text = visitPersonNum
        End If

        ' 商談テーブルNo.
        Dim salesTableNo As String = Trim(CType(visitItem.FindControl("salesTableNo"), HiddenField).Value)
        If String.IsNullOrEmpty(salesTableNo) OrElse String.Equals(salesTableNo, "0") Then
            ' Logger.Debug("SetInitDetail_003")
            CType(visitItem.FindControl("Label_SalesTableNo"), Label).Text = DefaultValue

        Else
            ' Logger.Debug("SetInitDetail_004")
            CType(visitItem.FindControl("Label_SalesTableNo"), Label).Text = salesTableNo
        End If

        ' 顧客担当スタッフ
        Dim customerStaffName As String = Trim(CType(visitItem.FindControl("customerStaffName"), Label).Text)
        If String.IsNullOrEmpty(customerStaffName) Then
            ' Logger.Debug("SetInitDetail_005")
            ' $01 start step2開発
            CType(visitItem.FindControl("Label_CustStaffName"), Literal).Text = DefaultValue
            ' $01 end   step2開発
        Else
            ' Logger.Debug("SetInitDetail_006")
            ' $01 start step2開発
            CType(visitItem.FindControl("Label_CustStaffName"), Literal).Text = customerStaffName
            ' $01 end   step2開発
        End If

        ' 対応担当スタッフ名、対応担当スタッフ画像
        SetInitDealStaffInfo(visitItem, staffPicPash)

        ' $01 start step2開発
        ' 苦情アイコン
        CType(visitItem.FindControl("Claim_Icon_Word"), Label).Text = claimIconWord
        Dim claimInfo As String = CType(visitItem.FindControl("claimInfo"), HiddenField).Value

        If String.IsNullOrEmpty(claimInfo) Then
            visitItem.FindControl("Div_Claim").Visible = False
        ElseIf CType(claimInfo, Boolean) Then
            visitItem.FindControl("Div_Claim").Visible = True
        Else
            visitItem.FindControl("Div_Claim").Visible = False
        End If
        ' $01 end   step2開発

        ' Logger.Debug("SetInitDetail_End ")

    End Sub

    ''' <summary>
    ''' お客様写真、お客様名の詳細設定を行う。
    ''' </summary>
    ''' <param name="visitItem">来店実績情報</param>
    ''' <param name="facePicPash">顧客写真用のパス</param>
    ''' <param name="keisyoZengo">敬称前後</param>
    ''' <remarks></remarks>
    Private Sub SetInitCustomerInfo( _
            ByVal visitItem As RepeaterItem, ByVal facePicPash As String, _
            ByVal keisyoZengo As String)

        ' Logger.Debug("SetInitCustomerInfo_Start ")

        ' お客様写真
        Dim customerImage As String = Trim(CType(visitItem.FindControl("customerImage"), HiddenField).Value)
        If String.IsNullOrEmpty(customerImage) Then
            ' Logger.Debug("SetInitCustomerInfo_001")
            CType(visitItem.FindControl("Image_Customer"), Image).ImageUrl = SilhouettePerson

        Else
            ' Logger.Debug("SetInitCustomerInfo_002")
            CType(visitItem.FindControl("Image_Customer"), Image).ImageUrl = facePicPash & customerImage
        End If

        ' お客様名
        Dim custmerId As String = Trim(CType(visitItem.FindControl("custmerId"), HiddenField).Value)

        ' 新規顧客の場合
        If String.IsNullOrEmpty(custmerId) Then
            ' Logger.Debug("SetInitCustomerInfo_003")

            ' 仮登録氏名の取得
            Dim tentativeName = Trim(CType(visitItem.FindControl("tentativeName"), Label).Text)

            ' 仮登録氏名が設定されていない場合
            If String.IsNullOrEmpty(tentativeName) Then
                ' Logger.Debug("SetInitCustomerInfo_004")

                ' 「新規お客様」とする
                ' Logger.Debug("SetInitCustomerInfo_005 " & "Call_Start WebWordUtility.GetWord Param[" & WordIdNewCustomer & "]")
                tentativeName = HttpUtility.HtmlEncode(WebWordUtility.GetWord(WordIdNewCustomer))
                ' Logger.Debug("SetInitCustomerInfo_005 " & "Call_End WebWordUtility.GetWord")
            End If

            CType(visitItem.FindControl("Label_CustomerName"), Label).Text = tentativeName

            ' 既存顧客の場合
        Else
            ' Logger.Debug("SetInitCustomerInfo_006")
            Dim customerName As String = Trim(CType(visitItem.FindControl("customerName"), Label).Text)
            Dim customerNameTitle As String = Trim(CType(visitItem.FindControl("customerNameTitle"), Label).Text)

            ' 氏名が設定されていない場合
            If String.IsNullOrEmpty(customerName) Then
                ' Logger.Debug("SetInitCustomerInfo_007")

                ' 「Unknown」とする
                ' Logger.Debug("SetInitCustomerInfo_008 " & "Call_Start WebWordUtility.GetWord Param[" & WordIdUnknown & "]")
                CType(visitItem.FindControl("Label_CustomerName"), Label).Text = _
                        HttpUtility.HtmlEncode(WebWordUtility.GetWord(WordIdUnknown))
                ' Logger.Debug("SetInitCustomerInfo_008 " & "Call_End WebWordUtility.GetWord")

            Else
                ' Logger.Debug("SetInitCustomerInfo_009")

                '敬称表示位置が前
                If String.Equals(keisyoZengo, SystemEnvKeisyoZengoMae) Then
                    ' Logger.Debug("SetInitCustomerInfo_010")
                    CType(visitItem.FindControl("Label_CustomerName"), Label).Text = _
                        customerNameTitle & NameTitleSpace & customerName

                    '敬称表示位置が後
                Else
                    ' Logger.Debug("SetInitCustomerInfo_011")
                    CType(visitItem.FindControl("Label_CustomerName"), Label).Text = _
                        customerName & NameTitleSpace & customerNameTitle
                End If
            End If
        End If

        ' Logger.Debug("SetInitCustomerInfo_End")

    End Sub

    ''' <summary>
    ''' 車両登録No.、来店手段の詳細設定を行う。
    ''' </summary>
    ''' <param name="visitItem">来店実績情報</param>
    ''' <remarks></remarks>
    Private Sub SetInitVclRegNoAndVisitMeans(ByVal visitItem As RepeaterItem)

        ' Logger.Debug("SetInitVclRegNoAndVisitMeans_Start ")

        ' 車両登録No.
        Dim vclRegNo As String = Trim(CType(visitItem.FindControl("vclRegNo"), Label).Text)

        ' 車両登録No.がない場合
        If String.IsNullOrEmpty(vclRegNo) Then
            ' Logger.Debug("SetInitVclRegNoAndVisitMeans_001")

            ' 来店手段
            Dim visitMeans As String = Trim(CType(visitItem.FindControl("visitMeans"), HiddenField).Value)

            ' 来店手段がない場合
            If String.IsNullOrEmpty(visitMeans) Then
                ' Logger.Debug("SetInitVclRegNoAndVisitMeans_002")
                visitItem.FindControl("Div_VclRegNo").Visible = True
                visitItem.FindControl("Div_MeansCar").Visible = False
                visitItem.FindControl("Div_MeansWalk").Visible = False
                ' $01 start step2開発
                CType(visitItem.FindControl("Label_VclRegNo"), Literal).Text = DefaultValue
                ' $01 end   step2開発
                ' 来店手段が車の場合
            ElseIf String.Equals(visitMeans, VisitMeansCar) Then
                ' Logger.Debug("SetInitVclRegNoAndVisitMeans_003")

                visitItem.FindControl("Div_VclRegNo").Visible = False
                visitItem.FindControl("Div_MeansCar").Visible = True
                visitItem.FindControl("Div_MeansWalk").Visible = False

                ' 来店手段が歩きの場合
            Else
                ' Logger.Debug("SetInitVclRegNoAndVisitMeans_004")

                visitItem.FindControl("Div_VclRegNo").Visible = False
                visitItem.FindControl("Div_MeansCar").Visible = False
                visitItem.FindControl("Div_MeansWalk").Visible = True
            End If

            ' 車両登録No.がある場合
        Else
            ' Logger.Debug("SetInitVclRegNoAndVisitMeans_005")

            visitItem.FindControl("Div_VclRegNo").Visible = True
            visitItem.FindControl("Div_MeansCar").Visible = False
            visitItem.FindControl("Div_MeansWalk").Visible = False
            ' $01 start step2開発
            CType(visitItem.FindControl("Label_VclRegNo"), Literal).Text = vclRegNo
            ' $01 end   step2開発
        End If

        ' Logger.Debug("SetInitVclRegNoAndVisitMeans_End")

    End Sub

    ''' <summary>
    ''' 対応担当スタッフ名、対応担当スタッフ画像の詳細設定を行う。
    ''' </summary>
    ''' <param name="visitItem">来店実績情報</param>
    ''' <param name="staffPicPash">スタッフ写真用のパス</param>
    ''' <remarks></remarks>
    Private Sub SetInitDealStaffInfo(ByVal visitItem As RepeaterItem, ByVal staffPicPash As String)

        ' Logger.Debug("SetInitDealStaffInfo_Start ")

        ' 対応担当スタッフ
        Dim visitStatus As String = Trim(CType(visitItem.FindControl("visitStatus"), HiddenField).Value)

        ' フリーの場合
        If String.Equals(visitStatus, VisitStatusFree) Then
            ' Logger.Debug("SetInitDealStaffInfo_001")

            ' 対応担当スタッフ名を非表示
            visitItem.FindControl("Label_DealStaffName").Visible = False
            ' ブロードキャストアイコンを非表示
            CType(visitItem.FindControl("Image_DealStaff"), Image).Visible = False

            ' フリー(ブロードキャスト)の場合
        ElseIf String.Equals(visitStatus, VisitStatusFreeBroud) Then
            ' Logger.Debug("SetInitDealStaffInfo_002")

            ' 対応担当スタッフ名を非表示
            visitItem.FindControl("Label_DealStaffName").Visible = False
            ' ブロードキャストアイコンを表示
            CType(visitItem.FindControl("Image_DealStaff"), Image).ImageUrl = IconBroudcast

        Else
            ' Logger.Debug("SetInitDealStaffInfo_003")

            ' 対応担当スタッフ名
            Dim dealStaffName As String = Trim(CType(visitItem.FindControl("dealStaffName"), Label).Text)
            If String.IsNullOrEmpty(dealStaffName) Then
                ' Logger.Debug("SetInitDealStaffInfo_004")
                CType(visitItem.FindControl("Label_DealStaffName"), Label).Text = DefaultValue

            Else
                ' Logger.Debug("SetInitDealStaffInfo_005")
                CType(visitItem.FindControl("Label_DealStaffName"), Label).Text = dealStaffName
            End If

            ' 対応担当スタッフ画像
            Dim dealStaffImage As String = Trim(CType(visitItem.FindControl("dealStaffImage"), HiddenField).Value)
            If String.IsNullOrEmpty(dealStaffImage) Then
                ' Logger.Debug("SetInitDealStaffInfo_006")
                CType(visitItem.FindControl("Image_DealStaff"), Image).ImageUrl = SilhouettePerson

            Else
                ' Logger.Debug("SetInitDealStaffInfo_007")
                CType(visitItem.FindControl("Image_DealStaff"), Image).ImageUrl = StaffPhotoPathPrefix & staffPicPash & dealStaffImage
            End If

            ' 画像の点滅設定
            If String.Equals(visitStatus, VisitStatusAdjust) Then
                ' Logger.Debug("SetInitDealStaffInfo_008")
                CType(visitItem.FindControl("Image_DealStaff"), Image).CssClass = "AdjustStaff"
            End If

        End If

        ' Logger.Debug("SetInitDealStaffInfo_End")

    End Sub

    ''' <summary>
    ''' 顧客写真・了解・待ち・不可ボタンの制御を行う。
    ''' </summary>
    ''' <param name="visitItem">来店実績情報</param>
    ''' <param name="isNotSalesPending">顧客保持中（商談中）フラグ</param>
    ''' <remarks></remarks>
    Private Sub SetButtonVisible( _
            ByVal visitItem As RepeaterItem, ByVal isNotSalesPending As Boolean)

        ' Logger.Debug(New StringBuilder("SetButtonVisible_Start Param[").Append(visitItem).Append( _
        '        ", ").Append(isNotSalesPending).Append("]").ToString())

        ' ボタン制御
        Dim visitStatus As String = CType(visitItem.FindControl("visitStatus"), HiddenField).Value
        ' 顧客詳細へのリンク
        Dim isVisibleCustomerButton As Boolean = False
        ' 了解ボタン
        Dim isVisibleConsentButton As Boolean = False
        ' 待ちボタン
        Dim isVisibleWaitButton As Boolean = False
        ' 不可ボタン
        Dim isVisibleNotConsentButton As Boolean = False

        If String.Equals(visitStatus, VisitStatusFreeBroud) Then
            ' フリー(ブロードキャスト)の場合
            ' Logger.Debug("SetButtonVisible_001")

            ' 了解ボタンの活性
            isVisibleConsentButton = True

        ElseIf String.Equals(visitStatus, VisitStatusDefinitionBroud) Then
            ' 確定(ブロードキャスト)の場合
            ' Logger.Debug("SetButtonVisible_002")

            ' 顧客詳細へのリンクの活性
            isVisibleCustomerButton = True
            ' 不可ボタンの活性
            isVisibleNotConsentButton = True

        ElseIf String.Equals(visitStatus, VisitStatusAdjust) Then
            ' 調整中の場合
            ' Logger.Debug("SetButtonVisible_003")

            ' 顧客詳細へのリンクの活性
            isVisibleCustomerButton = True
            ' 了解ボタンの活性
            isVisibleConsentButton = True
            ' 待ちボタンの活性
            isVisibleWaitButton = True
            ' 不可ボタンの活性
            isVisibleNotConsentButton = True

        ElseIf String.Equals(visitStatus, VisitStatusDefinition) Then
            ' 確定の場合
            ' Logger.Debug("SetButtonVisible_004")

            ' 顧客詳細へのリンクの活性
            isVisibleCustomerButton = True
            ' 待ちボタンの活性
            isVisibleWaitButton = True
            ' 不可ボタンの活性
            isVisibleNotConsentButton = True

        ElseIf String.Equals(visitStatus, VisitStatusWait) Then
            ' 待ちの場合
            ' Logger.Debug("SetButtonVisible_005")

            ' 顧客詳細へのリンクの活性
            isVisibleCustomerButton = True
            ' 了解ボタンの活性
            isVisibleConsentButton = True
            ' 不可ボタンの活性
            isVisibleNotConsentButton = True
            
            ' $02 start 複数顧客に対する商談平行対応
        ElseIf String.Equals(visitStatus, VisitStatusNegotiateStop) Then
            ' 商談中断の場合

            ' 顧客詳細へのリンクの活性
            isVisibleCustomerButton = True
            ' 不可ボタンの活性
            isVisibleNotConsentButton = True
            ' $02 end   複数顧客に対する商談平行対応
        End If

        ' 顧客詳細へのリンクの活性
        isVisibleCustomerButton = isVisibleCustomerButton And isNotSalesPending

        ' 顧客詳細へのリンク
        If isVisibleCustomerButton Then
            ' Logger.Debug("SetButtonVisible_006")
            DirectCast(visitItem.FindControl("Panel_InfoBox"), Panel).Attributes.Add("onclick", _
                    New StringBuilder("onClickButtonCustomer(").Append( _
                    visitItem.ItemIndex).Append(");").ToString())
        End If

        ' 了解ボタン
        visitItem.FindControl("Div_BlueButton").Visible = isVisibleConsentButton
        visitItem.FindControl("Div_BlueButtonOff").Visible = Not isVisibleConsentButton

        ' 待ちボタン
        visitItem.FindControl("Div_YellowButton").Visible = isVisibleWaitButton
        visitItem.FindControl("Div_YellowButtonOff").Visible = Not isVisibleWaitButton

        ' 不可ボタン
        visitItem.FindControl("Div_RedButton").Visible = isVisibleNotConsentButton
        visitItem.FindControl("Div_RedButtonOff").Visible = Not isVisibleNotConsentButton

        ' Logger.Debug("SetButtonVisible_End")

    End Sub

    ''' <summary>
    ''' お客様対応状況の設定を行う。
    ''' </summary>
    ''' <param name="visitItem">来店実績情報</param>
    ''' <remarks></remarks>
    Private Sub SetReferenceStatus(ByVal visitItem As RepeaterItem, _
                                   ByVal loginAccount As String)

        ' Logger.Debug("SetReferenceStatus_Start ")


        ' お客様対応状況
        Dim referenceStatus As String = Nothing

        Dim customerStaffId As String = CType(visitItem.FindControl("customerStaffId"), HiddenField).Value
        If String.IsNullOrEmpty(customerStaffId) OrElse Not String.Equals(customerStaffId, loginAccount) Then
            ' 顧客担当スタッフの情報ではない場合

            ' ご案内依頼
            ' Logger.Debug("SetReferenceStatus_001 " & "Call_Start WebWordUtility.GetWord Param[" & WordIdNotice & "]")
            referenceStatus = WebWordUtility.GetWord(WordIdNotice)
            ' Logger.Debug("SetReferenceStatus_001 " & "Call_End WebWordUtility.GetWord")

        Else
            ' 顧客担当スタッフの情報の場合

            Dim visitStatus As String = CType(visitItem.FindControl("visitStatus"), HiddenField).Value
            If String.Equals(visitStatus, VisitStatusFree) Then
                ' フリーの場合

                ' Logger.Debug("SetReferenceStatus_002 " & "Call_Start WebWordUtility.GetWord Param[" & WordIdVisitStatusFree & "]")
                referenceStatus = WebWordUtility.GetWord(WordIdVisitStatusFree)
                ' Logger.Debug("SetReferenceStatus_002 " & "Call_End WebWordUtility.GetWord")

            ElseIf String.Equals(visitStatus, VisitStatusAdjust) Then
                ' 調整中の場合

                ' Logger.Debug("SetReferenceStatus_003 " & "Call_Start WebWordUtility.GetWord Param[" & WordIdVisitStatusAdjust & "]")
                referenceStatus = WebWordUtility.GetWord(WordIdVisitStatusAdjust)
                ' Logger.Debug("SetReferenceStatus_003 " & "Call_End WebWordUtility.GetWord")

            ElseIf String.Equals(visitStatus, VisitStatusDefinition) Then
                ' 確定の場合

                ' Logger.Debug("SetReferenceStatus_004 " & "Call_Start WebWordUtility.GetWord Param[" & WordIdVisitStatusDefinition & "]")
                referenceStatus = WebWordUtility.GetWord(WordIdVisitStatusDefinition)
                ' Logger.Debug("SetReferenceStatus_004 " & "Call_End WebWordUtility.GetWord")

            ElseIf String.Equals(visitStatus, VisitStatusWait) Then
                ' 待ちの場合

                ' Logger.Debug("SetReferenceStatus_005 " & "Call_Start WebWordUtility.GetWord Param[" & WordIdVisitStatusWait & "]")
                referenceStatus = WebWordUtility.GetWord(WordIdVisitStatusWait)
                ' Logger.Debug("SetReferenceStatus_005 " & "Call_End WebWordUtility.GetWord")

            ElseIf String.Equals(visitStatus, VisitStatusSalesStart) Then
                ' 商談中の場合

                ' Logger.Debug("SetReferenceStatus_006 " & "Call_Start WebWordUtility.GetWord Param[" & WordIdVisitStatusSalesStart & "]")
                referenceStatus = WebWordUtility.GetWord(WordIdVisitStatusSalesStart)
                ' Logger.Debug("SetReferenceStatus_006 " & "Call_End WebWordUtility.GetWord")
                
                ' $02 start 複数顧客に対する商談平行対応
            ElseIf String.Equals(visitStatus, VisitStatusNegotiateStop) Then
                ' 商談中断の場合

                referenceStatus = WebWordUtility.GetWord(WordIdVisitStatusNegotiateStop)
                ' $02 end   複数顧客に対する商談平行対応

                ' $03 start 納車作業ステータス対応
            ElseIf String.Equals(visitStatus, VisitStatusDeliverlyStart) Then
                ' 納車作業中の場合

                referenceStatus = WebWordUtility.GetWord(WordIdVisitStatusDeliverlyStart)
            End If
            ' $03 end   納車作業ステータス対応

        End If

        Dim visitUtility As New VisitUtility
        CType(visitItem.FindControl("Label_ReferenceStatus"), Label).Text = _
                HttpUtility.HtmlEncode(visitUtility.CutTailString(referenceStatus, _
                DispLengthReferenceStatus, False))

        ' Logger.Debug("SetReferenceStatus_End ")

    End Sub

    ''' <summary>
    ''' 経過時間の設定を行う。
    ''' </summary>
    ''' <param name="dealerCode">販売店コード</param>
    ''' <remarks></remarks>
    Private Sub SetAlertSpan(ByVal dealerCode As String, ByVal storeCode As String)

        ' Logger.Debug("SetAlertSpan_Start Param[" & dealerCode & "]")

        ' 販売店環境設定
        ' Logger.Info("SetAlertSpan_001 " & "New BranchEnvSetting")
        Dim branchEnvSet As New BranchEnvSetting

        ' 未対応警告時間を取得
        Logger.Info("SetAlertSpan_002 " & "Call_Start GetEnvSetting Param[" & dealerCode & ", " & storeCode & ", " & DealerEnvAlertSpan & "]")
        Dim branchEnvSetAlertSpanRow As DlrEnvSettingDataSet.DLRENVSETTINGRow = _
            branchEnvSet.GetEnvSetting(dealerCode, storeCode, DealerEnvAlertSpan)
        Logger.Info("SetAlertSpan_002 " & "Call_End GetEnvSetting Ret[" & (branchEnvSetAlertSpanRow IsNot Nothing) & "]")
        If branchEnvSetAlertSpanRow Is Nothing OrElse Not IsNumeric(branchEnvSetAlertSpanRow.PARAMVALUE) Then

            NotDealTimeAlertSpan.Value = "0"
        Else

            NotDealTimeAlertSpan.Value = branchEnvSetAlertSpanRow.PARAMVALUE
        End If

        ' 日付管理(現在日付の取得)
        ' Logger.Debug("SetAlertSpan_003 " & "Call_Start DateTimeFunc.Now Param[" & dealerCode & "]")
        Dim now As Date = DateTimeFunc.Now(dealerCode)
        ' Logger.Debug("SetAlertSpan_003 " & "Call_End   DateTimeFunc.Now Ret[" & now & "]")

        For Each visitItem As RepeaterItem In Me.NotDealVisitList.Items

            ' $02 start 複数顧客に対する商談平行対応
            Dim visitTimestamp As New Date
            
            If Not String.IsNullOrEmpty(CType(visitItem.FindControl("stopTime"), HiddenField).Value) Then

                visitTimestamp = CType(visitItem.FindControl("stopTime"), HiddenField).Value

            Else

                visitTimestamp = CType(visitItem.FindControl("visitTimestamp"), HiddenField).Value

            End If
            ' $02 end   複数顧客に対する商談平行対応

            ' 経過時間(秒)を設定
            CType(visitItem.FindControl("NotDealVisit_Timer_Data"), Label).Text = _
                (now - visitTimestamp).TotalSeconds

        Next

        ' Logger.Debug("SetAlertSpan_End")
    End Sub

#End Region

End Class
