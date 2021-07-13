'━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
'SC3100101.aspx.vb
'──────────────────────────────────
'機能： 受付メイン
'補足： 
'作成： 2011/12/12 KN t.mizumoto
'更新： 2012/08/17 TMEJ m.okamura 新車受付機能改善 $01
'更新： 2013/01/10 TMEJ m.asano 新車タブレットショールーム管理機能開発 $02
'更新： 2013/02/28 TMEJ t.shimamura 新車タブレット受付画面管理指標変更対応 $03
'更新： 2013/05/29 TMEJ t.shimamura 【A.STEP2】次世代e-CRB新車タブレット　新DB適応に向けた機能開発 $04
'更新： 2013/09/03 TMEJ m.asano 次世代e-CRBセールス機能 新DB適応に向けた機能開発 $05
'更新： 2014/03/10 TMEJ y.nakamura 受注後フォロー機能開発 $06
'更新： 2019/06/26 NSK 鈴木 [TKM]UAT-0512 組織を超えて顧客詳細が編集できる【18PRJ02275-00 (FS)営業スタッフ納期遵守オペレーション確立に向けた試験研究】
'更新： 2019/11/12 NSK 鈴木 (トライ店システム評価)次世代セールス基盤：ログ出力機能における保守性向上検証
'更新： 2020/02/05 NSK s.natsume TKM Change request development for Next Gen e-CRB (CR075) $08
'更新： 2020/03/12 NSK s.natsume TKM Change request development for Next Gen e-CRB (CR060) $09
'──────────────────────────────────

Imports Toyota.eCRB.SystemFrameworks.Web
Imports Toyota.eCRB.SystemFrameworks.Core
Imports Toyota.eCRB.SystemFrameworks.Core.iCROP.BizLogic
Imports Toyota.eCRB.SystemFrameworks.Core.iCROP.DataAccess
Imports Toyota.eCRB.SystemFrameworks.Web.Controls
Imports Toyota.eCRB.Visit.ReceptionistMain.DataAccess.SC3100101DataSet
Imports Toyota.eCRB.Visit.ReceptionistMain.BizLogic
Imports System.Web.Services
Imports System.Web.Script.Serialization
Imports System.Globalization
Imports System.Data
Imports Toyota.eCRB.Visit.Api.BizLogic
Imports Toyota.eCRB.Visit.Api.DataAccess.VisitUtilityDataSet
Imports Toyota.eCRB.CommonUtility.BizLogic
Imports Toyota.eCRB.CommonUtility.DataAccess

''' <summary>
''' 受付メイン（メインエリア）
''' </summary>
''' <remarks></remarks>
Partial Class PagesSC3100101
    Inherits BasePage

#Region "非公開定数"
    ''' <summary>
    ''' デバッグフラグ
    ''' </summary>
    ''' <remarks></remarks>
    Private Const DebugFlag As Boolean = False

    ''' <summary>
    ''' システム環境設定パラメータ（敬称前後）
    ''' </summary>
    ''' <remarks></remarks>
    Private Const NameTitlePotision As String = "KEISYO_ZENGO"

    ''' <summary>
    ''' システム環境設定パラメータ（顧客写真取得パス）
    ''' </summary>
    ''' <remarks></remarks>
    Private Const FacePictureUploadUrl As String = "FACEPIC_UPLOADURL"

    ''' <summary>
    ''' システム環境設定パラメータ（スタッフ写真取得パス）
    ''' </summary>
    ''' <remarks></remarks>
    Private Const FilePathStaffphoto As String = "URI_STAFFPHOTO"

    ''' <summary>
    ''' システム環境設定パラメータ（受付更新権限コードリスト)
    ''' </summary>
    ''' <remarks></remarks>
    Private Const UpdateCodeList As String = "RECEPTION_UPDATE_CODE_LIST"

    ''' <summary>
    ''' 苦情情報日数(N日)
    ''' </summary>
    ''' <remarks></remarks>
    Private Const ComplaintDisplayDate As String = "COMPLAINT_DISPLAYDATE"

    ''' <summary>
    ''' 販売店環境設定パラメータ（画面更新ロック解除時間）
    ''' </summary>
    ''' <remarks></remarks>
    Private Const LockResetTime As String = "LOCK_RESET_INTERVAL"

    ''' <summary>
    ''' 販売店環境設定パラメータ（来店状況未対応警告時間）
    ''' </summary>
    ''' <remarks></remarks>
    Private Const VisitAlertSpan As String = "VISIT_TIME_ALERT_SPAN"

    ''' <summary>
    ''' 販売店環境設定パラメータ（待ち状況未対応警告時間）
    ''' </summary>
    ''' <remarks></remarks>
    Private Const WaitAlertSpan As String = "WAIT_TIME_ALERT_SPAN"

    ' $02 start 新車タブレットショールーム管理機能開発
    ''' <summary>
    ''' 販売店環境設定パラメータ（接客不要警告時間(第１段階)）
    ''' </summary>
    ''' <remarks></remarks>
    Private Const UnNecessaryFirstAlertSpan As String = "UNNECESSARY_FIRST_TIME_ALERT_SPAN"

    ''' <summary>
    ''' 販売店環境設定パラメータ（接客不要警告時間(第２段階)）
    ''' </summary>
    ''' <remarks></remarks>
    Private Const UnNecessarySecondAlertSpan As String = "UNNECESSARY_SECOND_TIME_ALERT_SPAN"

    ''' <summary>
    ''' 販売店環境設定パラメータ（商談中断警告時間）
    ''' </summary>
    ''' <remarks></remarks>
    Private Const StopAlertSpan As String = "STOP_TIME_ALERT_SPAN"
    ' $02 end   新車タブレットショールーム管理機能開発

    ''' <summary>
    ''' 販売店環境設定パラメータ（受付通知警告音出力権限コードリスト)
    ''' </summary>
    ''' <remarks></remarks>
    Private Const NoticeAlarmCodeList As String = "RECEPTION_NOTICE_ALARM_CODE_LIST"

    ''' <summary>
    ''' ブロードキャスト未送信
    ''' </summary>
    ''' <remarks></remarks>
    Private Const BroadcastFlagOff As String = "0"

    ''' <summary>
    ''' 敬称位置（前）
    ''' </summary>
    ''' <remarks></remarks>
    Private Const NameTitlePositionFront As String = "1"

    ''' <summary>
    ''' 商談テーブル使用中
    ''' </summary>
    ''' <remarks></remarks>
    Private Const UsedFlagUsed As String = "1"

    ''' <summary>
    ''' エラーフラグ：OFF
    ''' </summary>
    ''' <remarks></remarks>
    Private Const ErrorFlagOff As String = "0"

    ''' <summary>
    ''' エラーフラグ：ON
    ''' </summary>
    ''' <remarks></remarks>
    Private Const ErrorFlagOn As String = "1"

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
    ''' お客様情報入力画面のお客様名の文字数
    ''' </summary>
    ''' <remarks></remarks>
    Private Const CustomerDialogCustomerNameSize As Integer = 20

    '$09 start NSK s.natsume TKM Change request development for Next Gen e-CRB (CR060)
    ' ''' <summary>
    ' ''' お客様情報入力画面の項目名文字数
    ' ''' </summary>
    ' ''' <remarks></remarks>
    'Private Const CustomerDialogTitleSize As Integer = 20
    ''' <summary>
    ''' お客様情報入力画面の項目名文字数
    ''' </summary>
    ''' <remarks></remarks>
    Private Const CustomerDialogTitleSize As Integer = 40
    '$09 end NSK s.natsume TKM Change request development for Next Gen e-CRB (CR060)


    ''' <summary>
    ''' 「スタンバイスタッフに送信」ボタン名の文字数
    ''' </summary>
    ''' <remarks></remarks>
    Private Const BroadcastButtonNameSize As Integer = 19

    ' $02 start 新車タブレットショールーム管理機能開発
    ''' <summary>
    ''' 「スタンバイスタッフに送信」ボタン名の文字数
    ''' </summary>
    ''' <remarks></remarks>
    Private Const UnNecessaryButtonNameSize As Integer = 19
    ' $02 end   新車タブレットショールーム管理機能開発

    ''' <summary>
    ''' お客様情報入力画面の商談テーブルNoの文字数
    ''' </summary>
    ''' <remarks></remarks>
    Private Const CustomerDialogSalesTableNoSize As Integer = 6

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
    ''' セッションキー（遷移元メニュー）
    ''' </summary>
    ''' <remarks></remarks>
    Private Const SessionKeyBeforeFooterId As String = "beforeFooterId"

    ''' <summary>
    ''' セッションキー（苦情情報日数(N日)）
    ''' </summary>
    ''' <remarks></remarks>
    Private Const SessionKeyComplaintDateCount As String = "complaintDateCount"

    '$08 NSK s.natsume TKM Change request development for Next Gen e-CRB (CR075) START
    ''' <summary>
    ''' セッションキー（受注後活動コード(納車))）
    ''' </summary>
    ''' <remarks></remarks>
    Private Const SessionKeyAfterActionCodeDelivery As String = "afterActionCodeDeli"
    '$08 NSK s.natsume TKM Change request development for Next Gen e-CRB (CR075) END

    ''' <summary>
    ''' 受付メイン画面ID
    ''' </summary>
    ''' <remarks></remarks>
    Private Const ReceptionistId As String = "SC3100101"

    ''' <summary>
    ''' フッターボタンID（スケジュール）
    ''' </summary>
    ''' <remarks></remarks>
    Private Const FooterIdSubmenuSchedule As Integer = 101

    ''' <summary>
    ''' フッターボタンID（電話帳）
    ''' </summary>
    ''' <remarks></remarks>
    Private Const FooterIdSubmenuCont As Integer = 102

    ''' <summary>
    ''' フッターボタンID（ショールームステータス）
    ''' </summary>
    ''' <remarks></remarks>
    Private Const FooterIdSubmenuShowRoomStatus As Integer = FooterMenuCategory.ShowRoomStatus

    ''' <summary>
    ''' フッターボタンID（試乗車）
    ''' </summary>
    ''' <remarks></remarks>
    Private Const FooterIdSubmenuTestDrive As Integer = 201

    ' $01 start スタンバイスタッフ並び順変更対応
    ''' <summary>
    ''' フッターボタンID（スタンバイスタッフ並び順変更）
    ''' </summary>
    ''' <remarks></remarks>
    Private Const FooterIdSubmenuStandByStaff As Integer = 1201
    ' $01 end   スタンバイスタッフ並び順変更対応

    ' $05 start 次世代e-CRBセールス機能 新DB適応に向けた機能開発
    ''' <summary>
    ''' フッターボタンID（お客様チップ作成）
    ''' </summary>
    ''' <remarks></remarks>
    Private Const FooterIdSubmenuCreateCustomerChip As Integer = 1202
    ' $05 end   次世代e-CRBセールス機能 新DB適応に向けた機能開発

    ''' <summary>
    ''' メッセージID（正常）
    ''' </summary>
    ''' <remarks></remarks>
    Private Const MessageIdNormal As Integer = 0

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
    ''' 販売店環境設定パラメータ（査定警告時間）
    ''' </summary>
    ''' <remarks></remarks>
    Private Const AssessmentSpan As String = "ASSESSMENT_ALERT_SPAN"

    ''' <summary>
    ''' 販売店環境設定パラメータ（価格相談警告時間）
    ''' </summary>
    ''' <remarks></remarks>
    Private Const PriceSpan As String = "PRICE_ALERT_SPAN"

    ''' <summary>
    ''' 販売店環境設定パラメータ（ヘルプ警告時間）
    ''' </summary>
    ''' <remarks></remarks>
    Private Const HelpSpan As String = "HELP_ALERT_SPAN"

    ''' <summary>
    ''' メッセージID(902:再描画)
    ''' </summary>
    ''' <remarks></remarks>
    Private Const MessageReloadView As Integer = 902

    ''' <summary>
    ''' 来店実績ステータス（商談中）
    ''' </summary>
    ''' <remarks></remarks>
    Private Const VisitStatusNegotiate As String = "07"

    ' $02 start 新車タブレットショールーム管理機能開発
    ''' <summary>
    ''' 来店実績ステータス（フリー）
    ''' </summary>
    ''' <remarks></remarks>
    Private Const VisitStatusFree As String = "01"

    ''' <summary>
    ''' 来店実績ステータス（接客不要）
    ''' </summary>
    ''' <remarks></remarks>
    Private Const VisitStatusUnNecessary As String = "10"

    ' $03 start 納車作業ステータス対応
    ''' <summary>
    ''' 来店実績ステータス（納車作業中）
    ''' </summary>
    ''' <remarks></remarks>
    Private Const VisitStatusDeliverlyStart As String = "11"
    ' $03 end   納車作業ステータス対応

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

    ' $02 start 新車タブレットショールーム管理機能開発
    ''' <summary>
    ''' 文言の数
    ''' </summary>
    ''' <remarks></remarks>
    Private Const WordDictionaryCount As Integer = 75
    ' $02 end   新車タブレットショールーム管理機能開発

    ''' <summary>
    ''' 操作権限コード（受付）
    ''' </summary>
    ''' <remarks>未決定</remarks>
    Private Const OperationCdReception As Decimal = 51D

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
    ''' 顧客種別(1:所有者固定)
    ''' </summary>
    ''' <remarks></remarks>
    Private Const CustomerClassOwner As String = "1"

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

    ' $06 start 受注後フォロー機能開発
    ''' <summary>
    ''' 受注後プロセスアイコンスタイル
    ''' </summary>
    ''' <remarks></remarks>
    Private Const AfterOdrProcsIconStyle As String = "background:url({0}) center top no-repeat; top:{1}px; left:{2}px; width:{3}px"

    ''' <summary>
    ''' 販売店コード(XXXXX)
    ''' </summary>
    ''' <remarks></remarks>
    Private Const DealerCdX As String = "XXXXX"

    ''' <summary>
    ''' 受注後プロセス表示数(横)
    ''' </summary>
    ''' <remarks></remarks>
    Private Const AfterOdrProcsViewNum As Integer = 5

    ''' <summary>
    ''' 受注後プロセス表示幅
    ''' </summary>
    ''' <remarks></remarks>
    Private Const AfterOdrProcsViewWidth As Integer = 54

    ''' <summary>
    ''' 受注後プロセス表示高さ
    ''' </summary>
    ''' <remarks></remarks>
    Private Const AfterOdrProcsViewHeight As Integer = 48

    ''' <summary>
    ''' 受注後プロセス表示高さ(マージン)
    ''' </summary>
    ''' <remarks></remarks>
    Private Const AfterOdrProcsViewHeightMargine As Integer = 10

    ''' <summary>
    ''' 受注後プロセス初期表示位置X
    ''' </summary>
    ''' <remarks></remarks>
    Private Const AfterOdrProcsInitPosX As Integer = 1

    ''' <summary>
    ''' 受注後プロセス初期表示位置Y
    ''' </summary>
    ''' <remarks></remarks>
    Private Const AfterOdrProcsInitPosY As Integer = 11

    ' $06 end 受注後フォロー機能開発

    '$08 NSK s.natsume TKM Change request development for Next Gen e-CRB (CR075) START
    ''' <summary>
    ''' システム環境設定パラメータ(定期リフレッシュを行う間隔(秒))
    ''' </summary>
    ''' <remarks></remarks>
    Private Const ReceptionistMainRefreshInterval = "RECEPTIONIST_MAIN_REFRESH_INTERVAL"

    ''' <summary>
    ''' システム設定パラメータ（納車活動を示す受注後活動コード）
    ''' </summary>
    ''' <remarks></remarks>
    Private Const AfterOrderActionCodeDelivery = "AFTER_ODR_ACT_CD_DELI"
    '$08 NSK s.natsume TKM Change request development for Next Gen e-CRB (CR075) END

    '$09 start NSK s.natsume TKM Change request development for Next Gen e-CRB (CR060)
    ''' <summary>
    ''' ローカル文言の数
    ''' </summary>
    ''' <remarks></remarks>
    Private Const LocalWordDictionaryCount As Integer = 5

    ''' <summary>
    ''' ローカル文言の開始値
    ''' </summary>
    ''' <remarks></remarks>
    Private Const LocalWordDictionaryInitial As Integer = 10001

    ''' <summary>
    ''' システム環境設定パラメータ(顧客の仮登録氏名で使用可能な文字種)
    ''' </summary>
    ''' <remarks></remarks>
    Private Const SalesPopupNameCharacterTypes = "SALES_POPUP_NAME_CHARACTER_TYPES"

    ''' <summary>
    ''' システム環境設定パラメータ(顧客の電話番号で使用可能な文字種)
    ''' </summary>
    ''' <remarks></remarks>
    Private Const SalesPopupTelNumberCharacterTypes = "SALES_POPUP_TELNO_CHARACTER_TYPES"

    ''' <summary>
    ''' エラーメッセージID（10901:使用可能な文字種エラー）
    ''' </summary>
    ''' <remarks></remarks>
    Private Const MessageIdCharacterTypes As Integer = 10901

    ''' <summary>
    ''' エラーメッセージID（10902:名前が3語以内でないエラー）
    ''' </summary>
    ''' <remarks></remarks>
    Private Const MessageIdName3Words As Integer = 10902

    ''' <summary>
    ''' エラーメッセージID（10903:電話番号の文字数のエラー）
    ''' </summary>
    ''' <remarks></remarks>
    Private Const MessageIdTelNumberLength As Integer = 10903

    ''' <summary>
    ''' 文言コード（24:仮登録氏名）
    ''' </summary>
    ''' <remarks></remarks>
    Private Const WordIdTentativeName As Integer = 24

    ''' <summary>
    ''' 文言コード（10002:電話番号）
    ''' </summary>
    ''' <remarks></remarks>
    Private Const WordIdTelNumber As Integer = 10002

    ''' <summary>
    ''' 文言コード（10001:お客様名・電話番号入力欄タイトル）
    ''' </summary>
    ''' <remarks></remarks>
    Private Const WordIdNameTelNumberTitle As Integer = 10001
    '$09 end NSK s.natsume TKM Change request development for Next Gen e-CRB (CR060)

#End Region

#Region "非公開変数"
    ''' <summary>
    ''' ページ用マスタページ
    ''' </summary>
    ''' <remarks></remarks>
    Private commonMasterPage As CommonMasterPage
#End Region

#Region " イベント処理 "

#Region " ページロード "
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

            If DebugFlag Then
                ' Logger.Debug("Page_Load_001_1" & DebugFlag)
                DebugArea.Visible = True
            End If

            'ログインユーザの情報を格納
            Dim context As StaffContext = StaffContext.Current

            Dim branchEnvSet As New BranchEnvSetting

            'ロック解除秒数 基盤の環境変数より取得する
            Dim branchEnvSetLockResetTime As DlrEnvSettingDataSet.DLRENVSETTINGRow = Nothing
            Logger.Info("Page_Load_003" & "Call_Start GetEnvSetting Param[" & context.DlrCD & "," & context.BrnCD & "," & LockResetTime & "]")
            branchEnvSetLockResetTime = branchEnvSet.GetEnvSetting(context.DlrCD, context.BrnCD, LockResetTime)
            Logger.Info("Page_Load_003" & "Call_End GetEnvSetting Ret[" & IsDBNull(branchEnvSetLockResetTime) & "]")
            LockResetInterval.Value = branchEnvSetLockResetTime.PARAMVALUE

            ' 待ち時間警告秒数 基盤の環境変数より取得する
            Dim branchEnvSetVisitAlertSpan As DlrEnvSettingDataSet.DLRENVSETTINGRow = Nothing
            Dim branchEnvSetWaitAlertSpan As DlrEnvSettingDataSet.DLRENVSETTINGRow = Nothing
            Logger.Info("Page_Load_004" & "Call_Start GetEnvSetting Param[" & context.DlrCD & "," & context.BrnCD & "," & VisitAlertSpan & "]")
            branchEnvSetVisitAlertSpan = branchEnvSet.GetEnvSetting(context.DlrCD, context.BrnCD, VisitAlertSpan)
            Logger.Info("Page_Load_004" & "Call_End GetEnvSetting Ret[" & IsDBNull(branchEnvSetVisitAlertSpan) & "]")
            Logger.Info("Page_Load_005" & "Call_Start GetEnvSetting Param[" & context.DlrCD & "," & context.BrnCD & "," & WaitAlertSpan & "]")
            branchEnvSetWaitAlertSpan = branchEnvSet.GetEnvSetting(context.DlrCD, context.BrnCD, WaitAlertSpan)
            Logger.Info("Page_Load_005" & "Call_End GetEnvSetting Ret[" & IsDBNull(branchEnvSetWaitAlertSpan) & "]")
            VisitTimeAlertSpan.Value = branchEnvSetVisitAlertSpan.PARAMVALUE
            WaitTimeAlertSpan.Value = branchEnvSetWaitAlertSpan.PARAMVALUE

            ' $02 start 新車タブレットショールーム管理機能開発
            ' 待接客不要警告秒数(第１段階)(第２段階) 基盤の環境変数より取得する
            Dim branchEnvSetUnNecessaryFirstAlertSpan As DlrEnvSettingDataSet.DLRENVSETTINGRow = Nothing
            Dim branchEnvSetUnNecessarySecondAlertSpan As DlrEnvSettingDataSet.DLRENVSETTINGRow = Nothing
            branchEnvSetUnNecessaryFirstAlertSpan = branchEnvSet.GetEnvSetting(context.DlrCD, context.BrnCD, UnNecessaryFirstAlertSpan)
            branchEnvSetUnNecessarySecondAlertSpan = branchEnvSet.GetEnvSetting(context.DlrCD, context.BrnCD, UnNecessarySecondAlertSpan)
            UnNecessaryFirstTimeAlertSpan.Value = branchEnvSetUnNecessaryFirstAlertSpan.PARAMVALUE
            UnNecessarySecondTimeAlertSpan.Value = branchEnvSetUnNecessarySecondAlertSpan.PARAMVALUE

            ' 商談中断警告時間 基盤の環境変数より取得する
            Dim branchEnvSetStopAlertSpan As DlrEnvSettingDataSet.DLRENVSETTINGRow = Nothing
            branchEnvSetStopAlertSpan = branchEnvSet.GetEnvSetting(context.DlrCD, context.BrnCD, StopAlertSpan)
            StopTimeAlertSpan.Value = branchEnvSetStopAlertSpan.PARAMVALUE
            ' $02 end   新車タブレットショールーム管理機能開発

            '査定、価格相談、ヘルプの警告秒数 基盤の環境変数より取得する
            Dim branchEnvSetAssessmentSpan As DlrEnvSettingDataSet.DLRENVSETTINGRow = Nothing
            Dim branchEnvSetPriceAlertSpan As DlrEnvSettingDataSet.DLRENVSETTINGRow = Nothing
            Dim branchEnvSetHelpAlertSpan As DlrEnvSettingDataSet.DLRENVSETTINGRow = Nothing
            Logger.Info("Page_Load_006" & "Call_Start GetEnvSetting Param[" & context.DlrCD & "," & context.BrnCD & "," & AssessmentSpan & "]")
            branchEnvSetAssessmentSpan = branchEnvSet.GetEnvSetting(context.DlrCD, context.BrnCD, AssessmentSpan)
            Logger.Info("Page_Load_006" & "Call_End GetEnvSetting Ret[" & IsDBNull(branchEnvSetAssessmentSpan) & "]")
            Logger.Info("Page_Load_007" & "Call_Start GetEnvSetting Param[" & context.DlrCD & "," & context.BrnCD & "," & PriceSpan & "]")
            branchEnvSetPriceAlertSpan = branchEnvSet.GetEnvSetting(context.DlrCD, context.BrnCD, PriceSpan)
            Logger.Info("Page_Load_007" & "Call_End GetEnvSetting Ret[" & IsDBNull(branchEnvSetPriceAlertSpan) & "]")
            Logger.Info("Page_Load_008" & "Call_Start GetEnvSetting Param[" & context.DlrCD & "," & context.BrnCD & "," & HelpSpan & "]")
            branchEnvSetHelpAlertSpan = branchEnvSet.GetEnvSetting(context.DlrCD, context.BrnCD, HelpSpan)
            Logger.Info("Page_Load_008" & "Call_End GetEnvSetting Ret[" & IsDBNull(branchEnvSetHelpAlertSpan) & "]")
            AssessmentAlertSpan.Value = CInt(branchEnvSetAssessmentSpan.PARAMVALUE)
            PriceAlertSpan.Value = CInt(branchEnvSetPriceAlertSpan.PARAMVALUE)
            HelpAlertSpan.Value = CInt(branchEnvSetHelpAlertSpan.PARAMVALUE)
            ' $02 start 新車タブレットショールーム管理機能開発
            ' セッションに保持
            MyBase.SetValue(ScreenPos.Current, SessionKeyAssessmentAlertSpan, CInt(branchEnvSetAssessmentSpan.PARAMVALUE))
            MyBase.SetValue(ScreenPos.Current, SessionKeyPriceAlertSpan, CInt(branchEnvSetPriceAlertSpan.PARAMVALUE))
            MyBase.SetValue(ScreenPos.Current, SessionKeyHelpAlertSpan, CInt(branchEnvSetHelpAlertSpan.PARAMVALUE))
            ' $02 end   新車タブレットショールーム管理機能開発


            ' 文言管理
            Dim wordDictionary As New Dictionary(Of Decimal, String)

            ' パフォーマンスを考慮して文言取得の際はログ出力を行わないようにする
            For displayId As Decimal = 2 To WordDictionaryCount
                wordDictionary.Add(displayId, WebWordUtility.GetWord(ReceptionistId, displayId))
            Next

            '$09 start NSK s.natsume TKM Change request development for Next Gen e-CRB (CR060)
            For displayId As Decimal = 0 To LocalWordDictionaryCount
                wordDictionary.Add(displayId + LocalWordDictionaryInitial, WebWordUtility.GetWord(ReceptionistId, displayId + LocalWordDictionaryInitial))
            Next
            '$09 end NSK s.natsume TKM Change request development for Next Gen e-CRB (CR060)

            InitWord(wordDictionary)

            Logger.Info("Page_Load_009" & "Call_Start MyBase.SetValue[" & _
                         ScreenPos.Current & "," & SessionKeyWordDictionary & "," & wordDictionary.ToString() & "]")
            MyBase.SetValue(ScreenPos.Current, SessionKeyWordDictionary, wordDictionary)

            Dim sysEnvSet As New SystemEnvSetting

            '敬称の前後位置を基盤の環境変数より取得する
            Dim sysEnvSetTitlePosRow As SystemEnvSettingDataSet.SYSTEMENVSETTINGRow = Nothing
            Logger.Info("Page_Load_010" & "Call_Start GetSystemEnvSetting Param[" & NameTitlePotision & "]")
            sysEnvSetTitlePosRow = sysEnvSet.GetSystemEnvSetting(NameTitlePotision)
            Logger.Info("Page_Load_010" & "Call_End GetSystemEnvSetting Ret[" & IsDBNull(sysEnvSetTitlePosRow) & "]")
            Dim nameTitlePos As String = sysEnvSetTitlePosRow.PARAMVALUE

            Logger.Info("Page_Load_011" & "Call_Start MyBase.SetValue[" & _
                         ScreenPos.Current & "," & SessionKeyNameTitlePos & "," & nameTitlePos & "]")
            MyBase.SetValue(ScreenPos.Current, SessionKeyNameTitlePos, nameTitlePos)

            '顧客写真用のパスを取得
            Dim sysEnvSetPathRow As SystemEnvSettingDataSet.SYSTEMENVSETTINGRow = Nothing
            Logger.Info("Page_Load_012" & "Call_Start GetSystemEnvSetting Param[" & FacePictureUploadUrl & "]")
            sysEnvSetPathRow = sysEnvSet.GetSystemEnvSetting(FacePictureUploadUrl)
            Logger.Info("Page_Load_012" & "Call_End GetSystemEnvSetting Ret[" & IsDBNull(sysEnvSetPathRow) & "]")
            Dim facePicPath As String = sysEnvSetPathRow.PARAMVALUE

            Logger.Info("Page_Load_013" & "Call_Start MyBase.SetValue[" & _
                         ScreenPos.Current & "," & SessionKeyFacePicPath & "," & facePicPath & "]")
            MyBase.SetValue(ScreenPos.Current, SessionKeyFacePicPath, facePicPath)

            'スタッフ写真用のパスを取得
            Dim staffPathRow As DlrEnvSettingDataSet.DLRENVSETTINGRow = Nothing
            Logger.Info("Page_Load_014" & "Call_Start GetSystemEnvSetting Param[" & FilePathStaffphoto & "]")
            staffPathRow = branchEnvSet.GetEnvSetting(context.DlrCD, context.BrnCD, FilePathStaffphoto)
            Logger.Info("Page_Load_014" & "Call_End GetSystemEnvSetting Ret[" & IsDBNull(staffPathRow) & "]")
            Dim staffPhotoPath As String = staffPathRow.PARAMVALUE

            Logger.Info("Page_Load_015" & "Call_Start MyBase.SetValue[" & _
                         ScreenPos.Current & "," & SessionKeyStaffPhotoPath & "," & staffPhotoPath & "]")
            MyBase.SetValue(ScreenPos.Current, SessionKeyStaffPhotoPath, staffPhotoPath)

            '操作権限コードリスト
            OperationStatus.Value = GetOperationCode(CType(context.OpeCD, Decimal))

            '現在日時 基盤より取得
            ' Logger.Debug("Page_Load_016" & "Call_Start DateTimeFunc.Now Param[" & context.DlrCD & "]")
            Dim nowDate As Date = DateTimeFunc.Now(context.DlrCD)
            ' Logger.Debug("Page_Load_016" & "Call_End DateTimeFunc.Now Ret[" & nowDate & "]")

            NowDateString.Value = nowDate.ToString("yyyyMMdd", CultureInfo.InvariantCulture)

            '警告音出力フラグを取得
            ' $02 start 新車タブレットショールーム管理機能開発
            Dim businessLogic As New SC3100101BusinessLogic
            ' $02 end   新車タブレットショールーム管理機能開発
            AlarmOutputStatus.Value = businessLogic.GetAlarmOutputFlg(context.DlrCD, context.BrnCD, CType(context.OpeCD, Decimal))
            businessLogic = Nothing

            '苦情情報日数を取得
            Dim sysEnvSetComplaintDisplayDateRow As SystemEnvSettingDataSet.SYSTEMENVSETTINGRow = Nothing
            Logger.Info("Page_Load_018" & "Call_Start GetSystemEnvSetting Param[" & ComplaintDisplayDate & "]")
            sysEnvSetComplaintDisplayDateRow = sysEnvSet.GetSystemEnvSetting(ComplaintDisplayDate)
            Logger.Info("Page_Load_018" & "Call_End GetSystemEnvSetting Ret[" & IsDBNull(sysEnvSetComplaintDisplayDateRow) & "]")
            Dim complaintDateCount As String = sysEnvSetComplaintDisplayDateRow.PARAMVALUE

            Logger.Info("Page_Load_019" & "Call_Start MyBase.SetValue[" & _
                         ScreenPos.Current & "," & SessionKeyComplaintDateCount & "," & complaintDateCount & "]")
            MyBase.SetValue(ScreenPos.Current, SessionKeyComplaintDateCount, complaintDateCount)

            '$08 NSK s.natsume TKM Change request development for Next Gen e-CRB (CR075) START
            'システム環境設定から定期リフレッシュ間隔を取得
            Dim refreshTimeRow As SystemEnvSettingDataSet.SYSTEMENVSETTINGRow = Nothing
            Logger.Info("Page_Load_020" & "Call_Start GetSystemEnvSetting Param[" & ReceptionistMainRefreshInterval & "]")
            refreshTimeRow = sysEnvSet.GetSystemEnvSetting(ReceptionistMainRefreshInterval)
            Logger.Info("Page_Load_020" & "Call_End GetSystemEnvSetting Ret[" & IsDBNull(refreshTimeRow) & "]")
            RefreshInterval.Value = refreshTimeRow.PARAMVALUE

            'システム設定から受注後活動コード(納車)を取得
            Logger.Info("Page_Load_021" & "Call_Start GetSystemSetting Param[" & AfterOrderActionCodeDelivery & "]")
            Dim afterActionCodeDeriver As String = ActivityInfoBusinessLogic.GetSystemSetting(AfterOrderActionCodeDelivery)

            Logger.Info("Page_Load_022" & "Call_Start MyBase.SetValue[" & _
             ScreenPos.Current & "," & SessionKeyAfterActionCodeDelivery & "," & afterActionCodeDeriver & "]")
            MyBase.SetValue(ScreenPos.Current, SessionKeyAfterActionCodeDelivery, afterActionCodeDeriver)

            '$08 NSK s.natsume TKM Change request development for Next Gen e-CRB (CR075) END

            '$09 start NSK s.natsume TKM Change request development for Next Gen e-CRB (CR060)
            'システム環境設定から仮登録氏名で使用可能な文字種を取得
            Dim customerNameCharacterRow As SystemEnvSettingDataSet.SYSTEMENVSETTINGRow = Nothing
            Logger.Info("Page_Load_023" & "Call_Start GetSystemEnvSetting Param[" & SalesPopupNameCharacterTypes & "]")
            customerNameCharacterRow = sysEnvSet.GetSystemEnvSetting(SalesPopupNameCharacterTypes)
            Logger.Info("Page_Load_023" & "Call_End GetSystemEnvSetting Ret[" & IsDBNull(customerNameCharacterRow) & "]")
            TentativeNameCharacterType.Value = customerNameCharacterRow.PARAMVALUE


            'システム環境設定から電話番号で使用可能な文字種を取得
            Dim customertelNumberCharacterRow As SystemEnvSettingDataSet.SYSTEMENVSETTINGRow = Nothing
            Logger.Info("Page_Load_024" & "Call_Start GetSystemEnvSetting Param[" & SalesPopupTelNumberCharacterTypes & "]")
            customertelNumberCharacterRow = sysEnvSet.GetSystemEnvSetting(SalesPopupTelNumberCharacterTypes)
            Logger.Info("Page_Load_024" & "Call_End GetSystemEnvSetting Ret[" & IsDBNull(customertelNumberCharacterRow) & "]")
            TelNumberCharacterType.Value = customertelNumberCharacterRow.PARAMVALUE
            '$09 end NSK s.natsume TKM Change request development for Next Gen e-CRB (CR060)

        End If
        If Not ScriptManager.IsInAsyncPostBack Then
            Me.InitFooter()
        End If

        Logger.Info("Page_Load_End Ret[]")

    End Sub
#End Region

#Region "お客様氏名・商談テーブルNo.入力画面表示"
    ''' <summary>
    ''' お客様氏名・商談テーブルNo.入力画面表示
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub CustomerDialogDisplayButton_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles CustomerDialogDisplayButton.Click
        Logger.Info("CustomerDialogDisplayButton_Click_Start Param[" & sender.ToString & "," & e.ToString & "]")

        CustomerPopoverErrorMessage.Value = String.Empty

        If (String.IsNullOrEmpty(CustomerDialogVisitSeq.Value) OrElse
            String.IsNullOrEmpty(CustomerDialogVisitStatus.Value)) Then
            Logger.Info("CustomerDialogDisplayButton_Click End Ret[]")
            Exit Sub
        End If

        Logger.Info("CustomerDialogDisplayButton_Click_001 CustomerDialogVisitSeq = " & CustomerDialogVisitSeq.Value)
        Dim visitSeq As Long = CType(CustomerDialogVisitSeq.Value, Long)

        Logger.Info("CustomerDialogDisplayButton_Click_002 CustomerDialogVisitStatus = " & CustomerDialogVisitStatus.Value)
        Dim visitStatus As String = CType(CustomerDialogVisitStatus.Value, String)

        'お客様情報の取得
        Dim customerInfoDataTable As VisitReceptionVisitorCustomerDataTable = Nothing
        Dim businessLogic As New SC3100101BusinessLogic
        customerInfoDataTable = businessLogic.GetCustomerInfo(visitSeq, visitStatus)
        businessLogic = Nothing

        'お客様情報取得失敗時は処理を抜ける
        If customerInfoDataTable.Count <= 0 Then

            ' Logger.Debug("CustomerDialogDisplayButton_Click" & "Call_Start WebWordUtility.GetWord Param[" & ReceptionistId & ",902]")
            Dim errorMessage As String = WebWordUtility.GetWord(ReceptionistId, 902)
            ' Logger.Debug("CustomerDialogDisplayButton_Click" & "Call_End WebWordUtility.GetWord Ret[" & errorMessage & "]")

            CustomerPopoverErrorMessage.Value = errorMessage

            Logger.Info("CustomerDialogDisplayButton_Click End Ret[]")
            Return
        End If

        ' $02 start 新車タブレットショールーム管理機能開発
        Dim customerRow As VisitReceptionVisitorCustomerRow = customerInfoDataTable.Rows(0)
        ' $02 end   新車タブレットショールーム管理機能開発

        ' お客様氏名・商談テーブルNo.入力画面表示（上段）
        InitCustomerDialogAboveArea(customerRow)

        ' お客様氏名・商談テーブルNo.入力画面表示（下段）
        InitCustomerDialogUnderArea()

        Logger.Info("CustomerDialogDisplayButton_Click_End Ret[]")
    End Sub
#End Region

#Region "商談中詳細画面表示"

    ''' <summary>
    ''' 商談中詳細画面表示
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub StaffDetailDisplayButton_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles StaffDetailDisplayButton.Click
        Logger.Info("StaffDetailDisplayButton_Click_Start " & _
                    "Param[" & sender.ToString & "," & e.ToString & "]")

        'エラーメッセージ初期化
        StaffDetailPopoverErrorMessage.Value = String.Empty

        'セールス来店実績連番の取得
        If String.IsNullOrEmpty(StaffDetailDialogVisitSeq.Value) Then
            ' Logger.Debug("StaffDetailDisplayButton_Click_001 StaffDetailDialogVisitSeq.Value is NullOrEmpty")
            Logger.Info("StaffDetailDisplayButton_Click End Ret[]")
            Exit Sub
        End If
        ' Logger.Debug("StaffDetailDisplayButton_Click_002 StaffDetailDialogVisitSeq.Value = " & StaffDetailDialogVisitSeq.Value)

        Dim visitSeq As Long = CType(StaffDetailDialogVisitSeq.Value, Long)

        'お客様情報の取得
        ' $02 start 新車タブレットショールーム管理機能開発
        Dim customerInfoDataTable As VisitReceptionVisitorCustomerDataTable = Nothing
        Dim businessLogic As New SC3100101BusinessLogic
        ' $02 end   新車タブレットショールーム管理機能開発

        '$03 start 納車作業ステータス対応
        Dim visitStatus As String = CType(StaffDetailDialogVisitStatus.Value, String)
        customerInfoDataTable = businessLogic.GetCustomerInfo(visitSeq, visitStatus)

        businessLogic = Nothing

        'お客様情報取得失敗時は処理を抜ける
        'ステータスが商談中、納車作業中でなければ処理を抜ける
        If customerInfoDataTable.Count <= 0 _
            OrElse Not (visitStatus.Equals(VisitStatusNegotiate) Or visitStatus.Equals(VisitStatusDeliverlyStart)) Then
            '$03 end   納車作業ステータス対応

            ' Logger.Debug("StaffDetailDisplayButton_Click_003" & "Call_Start WebWordUtility.GetWord Param[" _
            '            & ReceptionistId & "," & MessageReloadView & "]")
            Dim errorMessage As String = WebWordUtility.GetWord(ReceptionistId, MessageReloadView)
            ' Logger.Debug("StaffDetailDisplayButton_Click_003" & "Call_End WebWordUtility.GetWord Ret[" & errorMessage & "]")

            StaffDetailPopoverErrorMessage.Value = errorMessage

            Logger.Info("StaffDetailDisplayButton_Click End Ret[]")
            Return
        End If
        ' Logger.Debug("StaffDetailDisplayButton_Click_004 customerInfoDataTable.Count = " & customerInfoDataTable.Count)

        '先頭の情報を取得
        ' $02 start 新車タブレットショールーム管理機能開発
        Dim customerRow As VisitReceptionVisitorCustomerRow = customerInfoDataTable.Rows(0)
        ' $02 end   新車タブレットショールーム管理機能開発

        'ログインユーザの情報を格納
        ' Logger.Debug("StaffDetailDisplayButton_Click_005" & "Call_Start StaffContext.Current")
        Dim context As StaffContext = StaffContext.Current
        ' Logger.Debug("StaffDetailDisplayButton_Click_005" & "Call_End StaffContext.Current Ret[" & context.ToString & "]")

        '現在日時 基盤より取得
        ' Logger.Debug("StaffDetailDisplayButton_Click_006" & "Call_Start DateTimeFunc.Now Param[" & context.DlrCD & "]")
        Dim nowDate As Date = DateTimeFunc.Now(context.DlrCD)
        ' Logger.Debug("StaffDetailDisplayButton_Click_006" & "Call_End DateTimeFunc.Now Ret[" & nowDate & "]")

        '商談中詳細表示設定(テーブル選択)
        InitStaffDetailDialogTableArea(context, nowDate)

        '商談中詳細表示設定(依頼リスト)
        InitStaffDetailDialogNoticeListArea(visitSeq, nowDate)

        '商談中詳細表示設定(顧客情報)
        InitStaffDetailDialogVisitInfoArea(customerRow, context, nowDate)

        '商談中詳細表示設定(プロセス)
        InitStaffDetailDialogProcessArea(customerRow, context)

        Logger.Info("StaffDetailDisplayButton_Click_End Ret[]")
    End Sub

#End Region

#Region "紐付け解除情報の取得"

    ' $01 start 複数顧客に対する商談平行対応
    ' ''' <summary>
    ' ''' 紐付け解除情報の取得
    ' ''' </summary>
    ' ''' <param name="sender"></param>
    ' ''' <param name="e"></param>
    ' ''' <remarks></remarks>
    'Protected Sub LinkingCancelDialogDisplayButton_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles LinkingCancelDialogDisplayButton.Click
    '    Logger.Info("LinkingCancelDialogDisplayButton_Click_Start Param[" & sender.ToString & "," & e.ToString & "]")

    '    LinkingCancelPopoverErrorMessage.Value = String.Empty

    '    ' アカウント情報が存在しない場合は処理を抜ける
    '    If (String.IsNullOrEmpty(LinkingCancelDialogAccount.Value) OrElse
    '        String.IsNullOrEmpty(LinkingCancelDialogStaffStatus.Value) OrElse
    '        String.IsNullOrEmpty(LinkingCancelDialogLinkingCount.Value)) Then
    '        Logger.Info("LinkingCancelDialogDisplayButton_Click End Ret[]")
    '        Exit Sub
    '    End If

    '    Dim account As String = CType(LinkingCancelDialogAccount.Value, String)
    '    Dim staffStatus As String = CType(LinkingCancelDialogStaffStatus.Value, String)
    '    Dim linkingCount As Integer = CType(LinkingCancelDialogLinkingCount.Value, Integer)

    '    ' 紐付き人数が存在しない場合
    '    If linkingCount <= 0 Then
    '        ' Logger.Debug("LinkingCancelDialogDisplayButton_Click End Ret[]")
    '    End If

    '    ' ログインユーザの情報を格納
    '    ' Logger.Debug("LinkingCancelDialogDisplayButton_Click" & "Call_Start StaffContext.Current")
    '    Dim context As StaffContext = StaffContext.Current
    '    ' Logger.Debug("LinkingCancelDialogDisplayButton_Click" & "Call_End StaffContext.Current Ret[" & context.ToString & "]")

    '    ' 現在日時 基盤より取得
    '    ' Logger.Debug("LinkingCancelDialogDisplayButton_Click" & "Call_Start DateTimeFunc.Now Param[" & context.DlrCD & "]")
    '    Dim nowDate As Date = DateTimeFunc.Now(context.DlrCD)
    '    ' Logger.Debug("LinkingCancelDialogDisplayButton_Click" & "Call_End DateTimeFunc.Now Ret[" & nowDate & "]")

    '    ' 紐付け解除情報の取得
    '    Dim linkingCancelDataTable As SC3100101LinkingCancelDataTable = Nothing
    '    Dim businessLogic As New SC3100101BusinessLogic
    '    linkingCancelDataTable = businessLogic.GetLinkingCancel(context.DlrCD, context.BrnCD, account, nowDate)
    '    businessLogic = Nothing

    '    If (0 < linkingCancelDataTable.Count AndAlso
    '        Not staffStatus.Equals(StaffStatusNegotiate)) Then

    '        ' 商談中以外の場合でレコードが存在する場合、先頭のデータを削除する
    '        linkingCancelDataTable.RemoveSC3100101LinkingCancelRow(linkingCancelDataTable.Item(0))
    '        linkingCount = linkingCount - 1
    '    End If

    '    ' 紐付け解除情報取得失敗時は処理を抜ける
    '    If linkingCancelDataTable.Count = 0 OrElse Not linkingCancelDataTable.Count.Equals(linkingCount) Then
    '        Logger.Info("LinkingCancelDialogDisplayButton_Click linkingCancelDataTableCount Ret[" & linkingCancelDataTable.Count & "]")
    '        Logger.Info("LinkingCancelDialogDisplayButton_Click linkingCount Ret[" & linkingCount & "]")
    '        ' Logger.Debug("LinkingCancelDialogDisplayButton_Click" & "Call_Start WebWordUtility.GetWord Param[" & ReceptionistId & ",902]")
    '        Dim errorMessage As String = WebWordUtility.GetWord(ReceptionistId, 902)
    '        ' Logger.Debug("LinkingCancelDialogDisplayButton_Click" & "Call_End WebWordUtility.GetWord Ret[" & errorMessage & "]")
    '        LinkingCancelPopoverErrorMessage.Value = errorMessage
    '        Logger.Info("LinkingCancelDialogDisplayButton_Click End Ret[]")
    '        Return
    '    End If

    '    ' 来店経過時間リストを設定
    '    VisitTimeList.Value = GetTimeSpanListString(linkingCancelDataTable, "VISITTIMESTAMP", nowDate)

    '    LinkingCancelCustomerRepeater.DataSource = linkingCancelDataTable
    '    LinkingCancelCustomerRepeater.DataBind()

    '    Dim maxLength As Integer = LinkingCancelCustomerRepeater.Items.Count - 1

    '    For i = 0 To maxLength

    '        Dim customerList As Control = LinkingCancelCustomerRepeater.Items(i)
    '        Dim customerInfoData As SC3100101LinkingCancelRow = linkingCancelDataTable.Rows(i)

    '        ' 来店実績番号が存在しない場合は処理を抜ける
    '        If customerInfoData.IsVISITSEQNull Then
    '            Continue For
    '        End If

    '        ' タグの開始と終了判定
    '        If i = 0 Then
    '            CType(customerList.FindControl("LinkingCancelCustomerAreaTop"), Literal).Visible = True
    '        ElseIf i = maxLength Then
    '            CType(customerList.FindControl("LinkingCancelCustomerAreaBottom"), Literal).Visible = True
    '        Else
    '            CType(customerList.FindControl("LinkingCancelCustomerAreaCenter"), Literal).Visible = True
    '        End If

    '        ' 顧客名設定
    '        Dim custName As New StringBuilder

    '        If Not IsDBNull(customerInfoData.CUSTNAME) AndAlso Not String.IsNullOrEmpty(customerInfoData.CUSTNAME) AndAlso _
    '            Not String.IsNullOrEmpty(customerInfoData.CUSTNAME.Trim()) Then

    '            ' 敬称の前後位置
    '            Logger.Info("LinkingCancelDialogDisplayButton_Click" & "Call_Start MyBase.GetValue Param[" & _
    '                         ScreenPos.Current & "," & SessionKeyNameTitlePos & "," & False & "]")
    '            Dim nameTitlePos As String = CType(MyBase.GetValue(ScreenPos.Current, SessionKeyNameTitlePos, False), String)
    '            Logger.Info("LinkingCancelDialogDisplayButton_Click" & "Call_End MyBase.GetValue Ret[" & nameTitlePos.ToString() & "]")

    '            Dim customerNameTitle As String = String.Empty

    '            If Not IsDBNull(customerInfoData.CUSTNAMETITLE) AndAlso Not String.IsNullOrEmpty(customerInfoData.CUSTNAMETITLE) OrElse _
    '                String.IsNullOrEmpty(customerInfoData.CUSTNAMETITLE.Trim()) Then

    '                customerNameTitle = customerInfoData.CUSTNAMETITLE

    '            End If

    '            '敬称の前後位置
    '            If nameTitlePos.Equals(NameTitlePositionFront) Then
    '                custName.Append(customerNameTitle)
    '                custName.Append(customerInfoData.CUSTNAME)
    '            Else
    '                custName.Append(customerInfoData.CUSTNAME)
    '                custName.Append(customerNameTitle)
    '            End If

    '            CType(customerList.FindControl("LinkingCancelCustomerName"), Literal).Text = ChangeString(custName.ToString, CustomerDialogCustomerNameSize, StringAdd)

    '        Else

    '            ' 文言管理
    '            Logger.Info("LinkingCancelDialogDisplayButton_Click" & "Call_Start MyBase.GetValue Param[" & _
    '                         ScreenPos.Current & "," & SessionKeyWordDictionary & "," & False & "]")
    '            Dim wordDictionary As Dictionary(Of Decimal, String) = _
    '                CType(MyBase.GetValue(ScreenPos.Current, SessionKeyWordDictionary, False), Dictionary(Of Decimal, String))
    '            Logger.Info("LinkingCancelDialogDisplayButton_Click" & "Call_End MyBase.GetValue Ret[" & wordDictionary.ToString() & "]")

    '            If Not customerInfoData.IsCUSTSEGMENTNull AndAlso Not String.IsNullOrEmpty(customerInfoData.CUSTSEGMENT) Then
    '                ' 既存顧客の場合
    '                CType(customerList.FindControl("LinkingCancelCustomerName"), Literal).Text = Server.HtmlEncode(wordDictionary(31))
    '            Else
    '                ' 新規顧客の場合
    '                CType(customerList.FindControl("LinkingCancelCustomerName"), Literal).Text = Server.HtmlEncode(wordDictionary(32))
    '            End If

    '        End If

    '    Next

    '    Logger.Info("LinkingCancelDialogDisplayButton_Click_End Ret[]")
    'End Sub
    ' $01 end   複数顧客に対する商談平行対応

#End Region

#Region "フッター制御"

    ''' <summary>
    ''' フッターサブメニューの宣言
    ''' </summary>
    ''' <param name="commonMaster">ページ用マスタページ</param>
    ''' <param name="category">自ページの所属メニュー</param>
    ''' <returns>フッターボタンIDの配列</returns>
    ''' <remarks></remarks>
    Public Overrides Function DeclareCommonMasterFooter(ByVal commonMaster As CommonMasterPage, _
                                                        ByRef category As FooterMenuCategory) _
                                                        As Integer()

        'ログインユーザの情報を格納
        Dim context As StaffContext = StaffContext.Current

        Me.commonMasterPage = commonMaster

        If context.OpeCD = OperationCdReception Then

            '更新権限の場合
            Dim beforeFooterId As Integer = GetBeforeFooterId()

            ' メインメニューより遷移の場合
            If beforeFooterId = FooterMenuCategory.MainMenu Then
                ' 自ページの所属メニューを宣言
                category = FooterMenuCategory.MainMenu

                'スケジュール、連絡先
                Return {FooterIdSubmenuSchedule, FooterIdSubmenuCont}
            Else
                ' 自ページの所属メニューを宣言
                category = beforeFooterId

                ' $05 start 次世代e-CRBセールス機能 新DB適応に向けた機能開発
                ' $01 start スタンバイスタッフ並び順変更対応
                '試乗, 順序変更, お客様チップ作成
                Return {FooterIdSubmenuTestDrive, FooterIdSubmenuStandByStaff, FooterIdSubmenuCreateCustomerChip}
                ' $01 end   スタンバイスタッフ並び順変更対応
                ' $05 end   次世代e-CRBセールス機能 新DB適応に向けた機能開発
            End If
        Else
            '読取権限の場合
            'SSMの場合はショールームステータスをハイライト
            category = FooterMenuCategory.ShowRoomStatus
            Return New Integer() {}

        End If

    End Function
#End Region

#Region "非同期通信時のエラー処理"

    ''' <summary>
    ''' 非同期通信時のエラー処理を行う
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <history>
    ''' 2019/11/12 NSK 鈴木 (トライ店システム評価)次世代セールス基盤：ログ出力機能における保守性向上検証
    ''' </history>
    Protected Sub SendErrorMessageButton_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles SendErrorMessageButton.Click
        Logger.Error("SendErrorMessageButton_Click Param[" & sender.ToString & "," & e.ToString & "]")

        Logger.Error("SendErrorMessageButton_Click Throw InvalidOperationException[" & ErrorMessage.Value & "]")
        Throw New InvalidOperationException(ErrorMessage.Value)

        Logger.Error("SendErrorMessageButton_Click Ret[]")
    End Sub
#End Region

#End Region

#Region " 非同期通信用メソッド"

    '$09 start NSK s.natsume TKM Change request development for Next Gen e-CRB (CR060)
    ' ''' <summary>
    ' ''' お客様氏名・商談テーブル登録ボタンタップ
    ' ''' </summary>
    ' ''' <param name="visitSeq">来店実績連番</param>
    ' ''' <param name="customerSegment">顧客区分</param>
    ' ''' <param name="tentativeName">仮登録氏名</param>
    ' ''' <param name="salesTableNoOld">商談テーブルNo. (変更前)</param>
    ' ''' <param name="salesTableNoNew">商談テーブルNo. (変更後)</param>
    ' ''' <returns>エラーの場合のメッセージ</returns>
    ' ''' <remarks></remarks>
    '<WebMethod(EnableSession:=True)> _
    'Public Shared Function RegistrationButton_Click(ByVal visitSeq As String, _
    '                                                ByVal customerSegment As String, _
    '                                                ByVal tentativeName As String, _
    '                                                ByVal salesTableNoOld As String, _
    '                                                ByVal salesTableNoNew As String) As String
    ''' <summary>
    ''' お客様氏名・商談テーブル登録ボタンタップ
    ''' </summary>
    ''' <param name="visitSeq">来店実績連番</param>
    ''' <param name="customerSegment">顧客区分</param>
    ''' <param name="tentativeName">仮登録氏名</param>
    ''' <param name="salesTableNoOld">商談テーブルNo. (変更前)</param>
    ''' <param name="salesTableNoNew">商談テーブルNo. (変更後)</param>
    ''' <param name="telNumber">電話番号</param>
    ''' <param name="tentativeNameCharacterTypes">仮登録氏名で使用可能な文字種</param>
    ''' <param name="telNumberCharacterTypes">電話番号で使用可能な文字種</param>
    ''' <returns>エラーの場合のメッセージ</returns>
    ''' <remarks></remarks>
    <WebMethod(EnableSession:=True)> _
    Public Shared Function RegistrationButton_Click(ByVal visitSeq As String, _
                                                    ByVal customerSegment As String, _
                                                    ByVal tentativeName As String, _
                                                    ByVal salesTableNoOld As String, _
                                                    ByVal salesTableNoNew As String, _
                                                    ByVal telNumber As String, _
                                                    ByVal tentativeNameCharacterTypes As String, _
                                                    ByVal telNumberCharacterTypes As String) As String
        '$09 end NSK s.natsume TKM Change request development for Next Gen e-CRB (CR060)

        Logger.Info("RegistrationButton_Click_Start Param[" & _
             visitSeq & "," & customerSegment & "," & tentativeName & "," & _
             salesTableNoOld & "," & salesTableNoNew & "]")

        ' セッション情報確認
        If Not StaffContext.IsCreated Then
            ' Logger.Debug("RegistrationButton_Click Throw[InvalidOperationException]")
            Throw New InvalidOperationException("Session timeout.")
        End If

        Try

            'ログインユーザの情報を格納
            ' Logger.Debug("RegistrationButton_Click_001" & "Call_Start StaffContext.Current")
            Dim context As StaffContext = StaffContext.Current
            ' Logger.Debug("RegistrationButton_Click_001" & "Call_End StaffContext.Current Ret[" & context.ToString & "]")

            'メッセージ
            Dim msgId As Integer = MessageIdNormal
            Dim errorMsg As String = String.Empty

            '$09 start NSK s.natsume TKM Change request development for Next Gen e-CRB (CR060)
            ''商談テーブルNo.をInteger型に変換
            'Dim integerSalesTableNoOld As Integer = 0
            'If Not Integer.TryParse(salesTableNoOld, integerSalesTableNoOld) Then
            '    integerSalesTableNoOld = -1
            'End If
            'Dim integerSalesTableNoNew As Integer = 0
            'If Not Integer.TryParse(salesTableNoNew, integerSalesTableNoNew) Then
            '    integerSalesTableNoNew = -1
            'End If

            'Dim businessLogic As New SC3100101BusinessLogic
            'msgId = businessLogic.RegistrationNameAndSalesTable(CType(visitSeq, Long), customerSegment, _
            '                                                     tentativeName, _
            '                                                     integerSalesTableNoOld, _
            '                                                     integerSalesTableNoNew, _
            '                                                     context.Account)
            Dim businessLogic As New SC3100101BusinessLogic

            '入力可能文字種以外の文字が入力されているか
            Dim isCharacterTypeError As Boolean = False

            Dim errorMessageReplace As String = String.Empty

            Dim msgIdName As Integer = businessLogic.TentativeNameValidationCheck(tentativeName, tentativeNameCharacterTypes)

            If (msgIdName = MessageIdNormal) Then

                Dim msgIdTelNumber As Integer = businessLogic.TelNumberValidationCheck(telNumber, telNumberCharacterTypes)

                If (msgIdTelNumber = MessageIdNormal) Then

                    '商談テーブルNo.をInteger型に変換
                    Dim integerSalesTableNoOld As Integer = 0
                    If Not Integer.TryParse(salesTableNoOld, integerSalesTableNoOld) Then
                        integerSalesTableNoOld = -1
                    End If
                    Dim integerSalesTableNoNew As Integer = 0
                    If Not Integer.TryParse(salesTableNoNew, integerSalesTableNoNew) Then
                        integerSalesTableNoNew = -1
                    End If

                    msgId = businessLogic.RegistrationNameAndSalesTable(CType(visitSeq, Long), customerSegment, _
                                                                         tentativeName, _
                                                                         integerSalesTableNoOld, _
                                                                         integerSalesTableNoNew, _
                                                                         context.Account, _
                                                                         telNumber)
                ElseIf (msgIdTelNumber = MessageIdCharacterTypes) Then
                    isCharacterTypeError = True
                    errorMessageReplace = WebWordUtility.GetWord(ReceptionistId, WordIdTelNumber)
                    msgId = msgIdTelNumber
                Else
                    msgId = msgIdTelNumber
                End If
            ElseIf (msgIdName = MessageIdCharacterTypes) Then
                isCharacterTypeError = True
                errorMessageReplace = WebWordUtility.GetWord(ReceptionistId, WordIdTentativeName)
                msgId = msgIdName
            Else
                msgId = msgIdName
            End If



            '$09 end NSK s.natsume TKM Change request development for Next Gen e-CRB (CR060)


            ' PUSH送信
            If msgId = MessageIdNormal Then
                businessLogic.SendPush()
            End If

            businessLogic = Nothing

            ' Logger.Debug("RegistrationButton_Click_002" & "Call_Start WebWordUtility.GetWord Param[" & ReceptionistId & "," & msgId & "]")
            errorMsg = WebWordUtility.GetWord(ReceptionistId, msgId)
            '$09 start NSK s.natsume TKM Change request development for Next Gen e-CRB (CR060)
            If (isCharacterTypeError) Then
                errorMsg = String.Format(errorMsg, errorMessageReplace)
            End If
            '$09 end NSK s.natsume TKM Change request development for Next Gen e-CRB (CR060)
            ' Logger.Debug("RegistrationButton_Click_002" & "Call_End WebWordUtility.GetWord Ret[" & errorMsg & "]")
            Logger.Info("RegistrationButton_Click End Ret[" & errorMsg & "]")
            Return errorMsg

        Catch exception As Exception

            'ログ出力
            Logger.Error("RegistrationButton_Click Error", exception)
            Throw

        End Try

    End Function

    '$09 start NSK s.natsume TKM Change request development for Next Gen e-CRB (CR060)
    ' ''' <summary>
    ' ''' スタンバイスタッフに送信ボタンタップ
    ' ''' </summary>
    ' ''' <param name="visitSeq">来店実績連番</param>
    ' ''' <param name="customerSegment">顧客区分</param>
    ' ''' <param name="tentativeName">仮登録氏名</param>
    ' ''' <param name="salesTableNoOld">商談テーブルNo. (変更前)</param>
    ' ''' <param name="salesTableNoNew">商談テーブルNo. (変更後)</param>
    ' ''' <param name="vehicleRegistrationNo">車両登録No.</param>
    ' ''' <returns>エラーの場合のメッセージ</returns>
    ' ''' <remarks></remarks>
    '<WebMethod(EnableSession:=True)> _
    'Public Shared Function BroadcastButton_Click(ByVal visitSeq As String, _
    '                                             ByVal customerSegment As String, _
    '                                             ByVal tentativeName As String, _
    '                                             ByVal salesTableNoOld As String, _
    '                                             ByVal salesTableNoNew As String, _
    '                                             ByVal vehicleRegistrationNo As String) As String
    ''' <summary>
    ''' スタンバイスタッフに送信ボタンタップ
    ''' </summary>
    ''' <param name="visitSeq">来店実績連番</param>
    ''' <param name="customerSegment">顧客区分</param>
    ''' <param name="tentativeName">仮登録氏名</param>
    ''' <param name="salesTableNoOld">商談テーブルNo. (変更前)</param>
    ''' <param name="salesTableNoNew">商談テーブルNo. (変更後)</param>
    ''' <param name="vehicleRegistrationNo">車両登録No.</param>
    ''' <param name="telNumber">電話番号</param>
    ''' <param name="tentativeNameCharacterTypes">仮登録氏名で使用可能な文字種</param>
    ''' <param name="telNumberCharacterTypes">電話番号で使用可能な文字種</param>
    ''' <returns>エラーの場合のメッセージ</returns>
    ''' <remarks></remarks>
    <WebMethod(EnableSession:=True)> _
    Public Shared Function BroadcastButton_Click(ByVal visitSeq As String, _
                                                 ByVal customerSegment As String, _
                                                 ByVal tentativeName As String, _
                                                 ByVal salesTableNoOld As String, _
                                                 ByVal salesTableNoNew As String, _
                                                 ByVal vehicleRegistrationNo As String, _
                                                 ByVal telNumber As String, _
                                                 ByVal tentativeNameCharacterTypes As String, _
                                                 ByVal telNumberCharacterTypes As String) As String
        '$09 end NSK s.natsume TKM Change request development for Next Gen e-CRB (CR060)

        Logger.Info("BroadcastButton_Click_Start Param[" & _
             visitSeq & "," & customerSegment & "," & tentativeName & "," & _
             salesTableNoOld & "," & salesTableNoNew & "," & vehicleRegistrationNo & "]")

        ' セッション情報確認
        If Not StaffContext.IsCreated Then
            ' Logger.Debug("BroadcastButton_Click Throw[InvalidOperationException]")
            Throw New InvalidOperationException("Session timeout.")
        End If

        Try
            'ログインユーザの情報を格納
            ' Logger.Debug("BroadcastButton_Click_001" & "Call_Start StaffContext.Current")
            Dim context As StaffContext = StaffContext.Current
            ' Logger.Debug("BroadcastButton_Click_001" & "Call_End StaffContext.Current Ret[" & context.ToString & "]")

            Dim resultList As New List(Of String)
            Dim javaScript As New JavaScriptSerializer

            'スタンバイスタッフ情報を取得
            Dim standbyStaffDataTable As SC3100101StandbyStaffDataTable = Nothing
            Dim businessLogic As New SC3100101BusinessLogic
            standbyStaffDataTable = businessLogic.GetStandbyStaff(context.DlrCD, context.BrnCD)

            'スタンバイスタッフがいない場合
            If standbyStaffDataTable.Count = 0 Then
                ' Logger.Debug("BroadcastButton_Click_002" & "StandbyStaffNothing")

                'スタンバイスタッフがいない旨のメッセージを表示
                resultList.Add(ErrorFlagOff)
                ' Logger.Debug("BroadcastButton_Click_003" & "Call_Start WebWordUtility.GetWord Param[" & ReceptionistId & ",900]")
                Dim standbyStaffNothingMessage As String = WebWordUtility.GetWord(ReceptionistId, 900)
                ' Logger.Debug("BroadcastButton_Click_003" & "Call_End WebWordUtility.GetWord Ret[" & standbyStaffNothingMessage & "]")
                resultList.Add(standbyStaffNothingMessage)
                Logger.Info("BroadcastButton_Click End Ret[" & javaScript.Serialize(resultList) & "]")
                Return javaScript.Serialize(resultList)
            End If
            ' Logger.Debug("BroadcastButton_Click_004" & "StandbyStaffNothing")

            'スタンバイスタッフのアカウントリストを作成
            Dim standbyStaffList As List(Of String) = New List(Of String)
            For Each row As SC3100101StandbyStaffRow In standbyStaffDataTable
                standbyStaffList.Add(row.ACCOUNT)
            Next

            'メッセージ
            Dim msgId As Integer = MessageIdNormal
            Dim errorMsg As String = String.Empty

            '$09 end NSK s.natsume TKM Change request development for Next Gen e-CRB (CR060)

            ''商談テーブルNo.をInteger型に変換
            'Dim integerSalesTableNoOld As Integer = 0
            'If Not Integer.TryParse(salesTableNoOld, integerSalesTableNoOld) Then
            '    integerSalesTableNoOld = -1
            'End If
            'Dim integerSalesTableNoNew As Integer = 0
            'If Not Integer.TryParse(salesTableNoNew, integerSalesTableNoNew) Then
            '    integerSalesTableNoNew = -1
            'End If

            'msgId = businessLogic.RequestNoticeBroadcast(CType(visitSeq, Long), customerSegment, _
            '                                                      tentativeName, _
            '                                                      integerSalesTableNoOld, _
            '                                                      integerSalesTableNoNew, _
            '                                                      vehicleRegistrationNo, standbyStaffList, _
            '                                                      context.Account)
            '入力可能文字種以外の文字が入力されているか
            Dim isCharacterTypeError As Boolean = False

            Dim errorMessageReplace As String = String.Empty

            Dim msgIdName As Integer = businessLogic.TentativeNameValidationCheck(tentativeName, tentativeNameCharacterTypes)

            If (msgIdName = MessageIdNormal) Then

                Dim msgIdTelNumber As Integer = businessLogic.TelNumberValidationCheck(telNumber, telNumberCharacterTypes)

                If (msgIdTelNumber = MessageIdNormal) Then

                    '商談テーブルNo.をInteger型に変換
                    Dim integerSalesTableNoOld As Integer = 0
                    If Not Integer.TryParse(salesTableNoOld, integerSalesTableNoOld) Then
                        integerSalesTableNoOld = -1
                    End If
                    Dim integerSalesTableNoNew As Integer = 0
                    If Not Integer.TryParse(salesTableNoNew, integerSalesTableNoNew) Then
                        integerSalesTableNoNew = -1
                    End If
                    msgId = businessLogic.RequestNoticeBroadcast(CType(visitSeq, Long), customerSegment, _
                                                                          tentativeName, _
                                                                          integerSalesTableNoOld, _
                                                                          integerSalesTableNoNew, _
                                                                          vehicleRegistrationNo, standbyStaffList, _
                                                                          context.Account, _
                                                                          telNumber)
                ElseIf (msgIdTelNumber = MessageIdCharacterTypes) Then
                    isCharacterTypeError = True
                    errorMessageReplace = WebWordUtility.GetWord(ReceptionistId, WordIdTelNumber)
                    msgId = msgIdTelNumber
                Else
                    msgId = msgIdTelNumber
                End If
            ElseIf (msgIdName = MessageIdCharacterTypes) Then
                isCharacterTypeError = True
                errorMessageReplace = WebWordUtility.GetWord(ReceptionistId, WordIdTentativeName)
                msgId = msgIdName
            Else
                msgId = msgIdName
            End If
            '$09 end NSK s.natsume TKM Change request development for Next Gen e-CRB (CR060)

            ' PUSH送信
            If msgId = MessageIdNormal Then
                businessLogic.SendPush()
            End If

            businessLogic = Nothing

            ' Logger.Debug("BroadcastButton_Click_005" & "Call_Start WebWordUtility.GetWord Param[" & ReceptionistId & "," & msgId & "]")
            errorMsg = WebWordUtility.GetWord(ReceptionistId, msgId)
            ' Logger.Debug("BroadcastButton_Click_005" & "Call_End WebWordUtility.GetWord Ret[" & errorMsg & "]")
            '$09 start NSK s.natsume TKM Change request development for Next Gen e-CRB (CR060)
            If (isCharacterTypeError) Then
                errorMsg = String.Format(errorMsg, errorMessageReplace)
            End If
            '$09 end NSK s.natsume TKM Change request development for Next Gen e-CRB (CR060)

            If String.IsNullOrEmpty(errorMsg) Then
                resultList.Add(ErrorFlagOff)
                resultList.Add(String.Empty)
            Else
                resultList.Add(ErrorFlagOn)
                resultList.Add(errorMsg)
            End If

            Logger.Info("BroadcastButton_Click End Ret[" & javaScript.Serialize(resultList) & "]")
            Return javaScript.Serialize(resultList)

        Catch exception As Exception

            'ログ出力
            Logger.Error("BroadcastButton_Click Error", exception)
            Throw

        End Try

    End Function


    ' $02 start 新車タブレットショールーム管理機能開発
    '$09 start NSK s.natsume TKM Change request development for Next Gen e-CRB (CR060)
    ' ''' <summary>
    ' ''' 接客不要ボタン押下処理
    ' ''' </summary>
    ' ''' <param name="visitSeq">来店実績連番</param>
    ' ''' <param name="customerSegment">顧客区分</param>
    ' ''' <param name="tentativeName">仮登録氏名</param>
    ' ''' <param name="salesTableNoOld">商談テーブルNo. (変更前)</param>
    ' ''' <param name="salesTableNoNew">商談テーブルNo. (変更後)</param>
    ' ''' <returns>エラー時のメッセージ</returns>
    ' ''' <remarks></remarks>
    '<WebMethod(EnableSession:=True)> _
    'Public Shared Function UnNecessaryButton_Click(ByVal visitSeq As String, _
    '                                             ByVal customerSegment As String, _
    '                                             ByVal tentativeName As String, _
    '                                             ByVal salesTableNoOld As String, _
    '                                             ByVal salesTableNoNew As String) As String
    ''' <summary>
    ''' 接客不要ボタン押下処理
    ''' </summary>
    ''' <param name="visitSeq">来店実績連番</param>
    ''' <param name="customerSegment">顧客区分</param>
    ''' <param name="tentativeName">仮登録氏名</param>
    ''' <param name="salesTableNoOld">商談テーブルNo. (変更前)</param>
    ''' <param name="salesTableNoNew">商談テーブルNo. (変更後)</param>
    ''' <param name="telNumber">電話番号</param>
    ''' <param name="tentativeNameCharacterTypes">仮登録氏名で使用可能な文字種</param>
    ''' <param name="telNumberCharacterTypes">電話番号で使用可能な文字種</param>
    ''' <returns>エラー時のメッセージ</returns>
    ''' <remarks></remarks>
    <WebMethod(EnableSession:=True)> _
    Public Shared Function UnNecessaryButton_Click(ByVal visitSeq As String, _
                                                 ByVal customerSegment As String, _
                                                 ByVal tentativeName As String, _
                                                 ByVal salesTableNoOld As String, _
                                                 ByVal salesTableNoNew As String, _
                                                 ByVal telNumber As String, _
                                                 ByVal tentativeNameCharacterTypes As String, _
                                                 ByVal telNumberCharacterTypes As String) As String
        '$09 end NSK s.natsume TKM Change request development for Next Gen e-CRB (CR060)
        Dim startLogString As New StringBuilder
        With startLogString
            .Append("UnNecessaryButton_Click_Start Param[")
            .Append(visitSeq)
            .Append(", ")
            .Append(customerSegment)
            .Append(", ")
            .Append(tentativeName)
            .Append(", ")
            .Append(salesTableNoOld)
            .Append(", ")
            .Append(salesTableNoNew)
            .Append("]")
        End With
        Logger.Info(startLogString.ToString)

        ' セッション情報確認
        If Not StaffContext.IsCreated Then
            Throw New InvalidOperationException("Session timeout.")
        End If

        Try

            'ログインユーザの情報を格納
            Dim context As StaffContext = StaffContext.Current

            'メッセージ
            Dim msgId As Integer = MessageIdNormal
            Dim errorMsg As String = String.Empty


            '$09 end NSK s.natsume TKM Change request development for Next Gen e-CRB (CR060)
            ''商談テーブルNo.をInteger型に変換
            'Dim integerSalesTableNoOld As Integer = 0
            'If Not Integer.TryParse(salesTableNoOld, integerSalesTableNoOld) Then
            '    integerSalesTableNoOld = -1
            'End If
            'Dim integerSalesTableNoNew As Integer = 0
            'If Not Integer.TryParse(salesTableNoNew, integerSalesTableNoNew) Then
            '    integerSalesTableNoNew = -1
            'End If

            ''来店実績接客不要更新
            'Dim businessLogic As New SC3100101BusinessLogic
            'msgId = businessLogic.RegistrationUnNecessary(CType(visitSeq, Long), customerSegment, _
            '                                              tentativeName, integerSalesTableNoOld, _
            '                                              integerSalesTableNoNew, context.Account)

            '入力可能文字種以外の文字が入力されているか
            Dim isCharacterTypeError As Boolean = False

            Dim errorMessageReplace As String = String.Empty

            Dim businessLogic As New SC3100101BusinessLogic

            Dim msgIdName As Integer = businessLogic.TentativeNameValidationCheck(tentativeName, tentativeNameCharacterTypes)

            If (msgIdName = MessageIdNormal) Then

                Dim msgIdTelNumber As Integer = businessLogic.TelNumberValidationCheck(telNumber, telNumberCharacterTypes)

                If (msgIdTelNumber = MessageIdNormal) Then
                    '商談テーブルNo.をInteger型に変換
                    Dim integerSalesTableNoOld As Integer = 0
                    If Not Integer.TryParse(salesTableNoOld, integerSalesTableNoOld) Then
                        integerSalesTableNoOld = -1
                    End If
                    Dim integerSalesTableNoNew As Integer = 0
                    If Not Integer.TryParse(salesTableNoNew, integerSalesTableNoNew) Then
                        integerSalesTableNoNew = -1
                    End If

                    '来店実績接客不要更新
                    msgId = businessLogic.RegistrationUnNecessary(CType(visitSeq, Long), customerSegment, _
                                              tentativeName, integerSalesTableNoOld, _
                                              integerSalesTableNoNew, context.Account, _
                                              telNumber)
                ElseIf (msgIdTelNumber = MessageIdCharacterTypes) Then
                    isCharacterTypeError = True
                    errorMessageReplace = WebWordUtility.GetWord(ReceptionistId, WordIdTelNumber)
                    msgId = msgIdTelNumber
                Else
                    msgId = msgIdTelNumber
                End If
            ElseIf (msgIdName = MessageIdCharacterTypes) Then
                isCharacterTypeError = True
                errorMessageReplace = WebWordUtility.GetWord(ReceptionistId, WordIdTentativeName)
                msgId = msgIdName
            Else
                msgId = msgIdName
            End If
            '$09 end NSK s.natsume TKM Change request development for Next Gen e-CRB (CR060)

            ' PUSH送信
            If msgId = MessageIdNormal Then
                businessLogic.SendPush()
            End If

            businessLogic = Nothing

            errorMsg = WebWordUtility.GetWord(ReceptionistId, msgId)
            '$09 start NSK s.natsume TKM Change request development for Next Gen e-CRB (CR060)
            If (isCharacterTypeError) Then
                errorMsg = String.Format(errorMsg, errorMessageReplace)
            End If
            '$09 end NSK s.natsume TKM Change request development for Next Gen e-CRB (CR060)
            Logger.Info("UnNecessaryButton_Click_End Ret[" & errorMsg & "]")

            Dim resultList As New List(Of String)

            If String.IsNullOrEmpty(errorMsg) Then
                resultList.Add(ErrorFlagOff)
                resultList.Add(String.Empty)
            Else
                resultList.Add(ErrorFlagOn)
                resultList.Add(errorMsg)
            End If

            Dim javaScript As New JavaScriptSerializer
            Return javaScript.Serialize(resultList)

        Catch exception As Exception

            'ログ出力
            Logger.Error("UnNecessaryButton_Click Error", exception)
            Throw

        End Try

    End Function
    ' $02 end   新車タブレットショールーム管理機能開発

    ''' <summary>
    ''' 来店・待ち状況削除ボタンタップ
    ''' </summary>
    ''' <param name="visitSeq">来店実績連番</param>
    ''' <returns>エラーの場合のメッセージ</returns>
    ''' <remarks></remarks>
    <WebMethod(EnableSession:=True)> _
    Public Shared Function VisitorDelButton_Click(ByVal visitSeq As String) As String

        Logger.Info("VisitorDelButton_Click_Start Param[" & visitSeq & "]")

        ' セッション情報確認
        If Not StaffContext.IsCreated Then
            ' Logger.Debug("VisitorDelButton_Click Throw[InvalidOperationException]")
            Throw New InvalidOperationException("Session timeout.")
        End If

        Try
            'ログインユーザの情報を格納
            ' Logger.Debug("VisitorDelButton_Click_001" & "Call_Start StaffContext.Current")
            Dim context As StaffContext = StaffContext.Current
            ' Logger.Debug("VisitorDelButton_Click_001" & "Call_End StaffContext.Current Ret[" & context.ToString & "]")

            'メッセージ
            Dim msgId As Integer = MessageIdNormal
            Dim errorMsg As String = String.Empty

            Dim businessLogic As New SC3100101BusinessLogic
            msgId = businessLogic.DeleteVisitorRecord(CType(visitSeq, Long), context.Account)

            ' PUSH送信
            If msgId = MessageIdNormal Then
                businessLogic.SendPush()
            End If

            businessLogic = Nothing

            ' Logger.Debug("VisitorDelButton_Click_002" & "Call_Start WebWordUtility.GetWord Param[" & ReceptionistId & "," & msgId & "]")
            errorMsg = WebWordUtility.GetWord(ReceptionistId, msgId)
            ' Logger.Debug("VisitorDelButton_Click_002" & "Call_End WebWordUtility.GetWord Ret[" & errorMsg & "]")
            Logger.Info("VisitorDelButton_Click_End Ret[" & errorMsg & "]")
            Return errorMsg

        Catch exception As Exception

            'ログ出力
            Logger.Error("VisitorDelButton_Click Error", exception)
            Throw

        End Try

    End Function

    ''' <summary>
    ''' お客様対応依頼ボタンタップ
    ''' </summary>
    ''' <param name="visitSeq">来店実績連番</param>
    ''' <param name="staffCode">スタッフコード</param>
    ''' <returns>エラーの場合のメッセージ</returns>
    ''' <remarks></remarks>
    <WebMethod(EnableSession:=True)> _
    Public Shared Function SendButton_Click(ByVal visitSeq As String, ByVal staffCode As String) As String

        Logger.Info("SendButton_Click_Start Param[" & visitSeq & "," & staffCode & "]")

        ' セッション情報確認
        If Not StaffContext.IsCreated Then
            ' Logger.Debug("SendButton_Click Throw[InvalidOperationException]")
            Throw New InvalidOperationException("Session timeout.")
        End If

        Try
            'ログインユーザの情報を格納
            ' Logger.Debug("SendButton_Click_001" & "Call_Start StaffContext.Current")
            Dim context As StaffContext = StaffContext.Current
            ' Logger.Debug("SendButton_Click_001" & "Call_End StaffContext.Current Ret[" & context.ToString & "]")

            'メッセージ
            Dim msgId As Integer = 0
            Dim errorMsg As String = String.Empty

            'SC割り当て実施
            Dim businessLogic As New SC3100101BusinessLogic
            msgId = businessLogic.SalesConsultantAssignment(CType(visitSeq, Long), staffCode, context.Account)

            ' PUSH送信
            If MessageIdNormal = 0 Then
                businessLogic.SendPush()
            End If

            businessLogic = Nothing

            ' Logger.Debug("SendButton_Click_002" & "Call_Start WebWordUtility.GetWord Param[" & ReceptionistId & "," & msgId & "]")
            errorMsg = WebWordUtility.GetWord(ReceptionistId, msgId)
            ' Logger.Debug("SendButton_Click_002" & "Call_End WebWordUtility.GetWord Ret[" & errorMsg & "]")

            Logger.Info("SendButton_Click End Ret[" & errorMsg & "]")
            Return errorMsg

        Catch exception As Exception

            'ログ出力
            Logger.Error("SendButton_Click Error", exception)
            Throw

        End Try

    End Function

    ' $01 start 複数顧客に対する商談平行対応
    ' ''' <summary>
    ' ''' 紐付け解除登録ボタンタップ
    ' ''' </summary>
    ' ''' <param name="visitSeqList">来店実績連番リスト</param>
    ' ''' <param name="dealAccount">対応アカウント</param>
    ' ''' <returns>エラーの場合のメッセージ</returns>
    ' ''' <remarks></remarks>
    '<WebMethod(EnableSession:=True)> _
    'Public Shared Function LinkingCancelButton_Click(ByVal visitSeqList As List(Of String), _
    '                                                 ByVal dealAccount As String) As String

    '    Logger.Info("LinkingCancelRegistrationButton_Click_Start Param[" & visitSeqList.Count & "," & dealAccount & "]")

    '    ' セッション情報確認
    '    If Not StaffContext.IsCreated Then
    '        ' Logger.Debug("LinkingCancelRegistrationButton_Click Throw[InvalidOperationException]")
    '        Throw New InvalidOperationException("Session timeout.")
    '    End If

    '    Try

    '        'ログインユーザの情報を格納
    '        ' Logger.Debug("LinkingCancelRegistrationButton_Click_001" & "Call_Start StaffContext.Current")
    '        Dim context As StaffContext = StaffContext.Current
    '        ' Logger.Debug("LinkingCancelRegistrationButton_Click_001" & "Call_End StaffContext.Current Ret[" & context.ToString & "]")

    '        'メッセージ
    '        Dim msgId As Integer = MessageIdNormal
    '        Dim errorMsg As String = String.Empty
    '        Dim businessLogic As New SC3100101BusinessLogic

    '        Dim visitSeq As Long = 0

    '        ' 紐付け更新処理を行う
    '        msgId = businessLogic.LinkingCancel(visitSeqList, _
    '                                            dealAccount, _
    '                                            context.Account)

    '        ' PUSH送信
    '        If msgId = MessageIdNormal Then
    '            businessLogic.SendPush()
    '        End If

    '        businessLogic = Nothing

    '        ' Logger.Debug("LinkingCancelRegistrationButton_Click_002" & "Call_Start WebWordUtility.GetWord Param[" & ReceptionistId & "," & msgId & "]")
    '        errorMsg = WebWordUtility.GetWord(ReceptionistId, msgId)
    '        ' Logger.Debug("LinkingCancelRegistrationButton_Click_002" & "Call_End WebWordUtility.GetWord Ret[" & errorMsg & "]")

    '        Logger.Info("LinkingCancelRegistrationButton_Click End Ret[" & errorMsg & "]")
    '        Return errorMsg

    '    Catch exception As Exception

    '        'ログ出力
    '        Logger.Error("LinkingCancelRegistrationButton_Click Error", exception)
    '        Throw

    '    End Try

    'End Function
    ' $01 end   複数顧客に対する商談平行対応


    ''' <summary>
    ''' 商談中詳細画面(登録ボタンタップ)
    ''' </summary>
    ''' <param name="visitSeq">来店実績連番</param>
    ''' <param name="salesTableNoOld">商談テーブルNo. (変更前)</param>
    ''' <param name="salesTableNoNew">商談テーブルNo. (変更後)</param>
    ''' <returns>エラーの場合のメッセージ</returns>
    ''' <remarks></remarks>
    <WebMethod(EnableSession:=True)> _
    Public Shared Function StaffDetailSubmitButton_Click(ByVal visitSeq As String, _
                                                        ByVal salesTableNoOld As String, _
                                                        ByVal salesTableNoNew As String) As String
        Logger.Info("StaffDetailSubmitButton_Click_Start " & _
                    "Param[" & visitSeq & "," & salesTableNoOld & "," & salesTableNoNew & "]")

        ' セッション情報確認
        If Not StaffContext.IsCreated Then
            ' Logger.Debug("StaffDetailSubmitButton_Click Throw[InvalidOperationException]")
            Throw New InvalidOperationException("Session timeout.")
        End If

        Try
            'ログインユーザの情報を格納
            ' Logger.Debug("StaffDetailSubmitButton_Click_001 " & "Call_Start StaffContext.Current")
            Dim context As StaffContext = StaffContext.Current
            ' Logger.Debug("StaffDetailSubmitButton_Click_001 " & "Call_End StaffContext.Current Ret[" & context.ToString & "]")

            Dim resultList As New List(Of String)
            Dim javaScript As New JavaScriptSerializer
            Dim businessLogic As New SC3100101BusinessLogic

            'メッセージ
            Dim msgId As Integer = MessageIdNormal
            Dim errorMsg As String = String.Empty

            '商談テーブルNo.をInteger型に変換
            Dim integerSalesTableNoOld As Integer = 0
            If Not Integer.TryParse(salesTableNoOld, integerSalesTableNoOld) Then

                ' Logger.Debug("StaffDetailSubmitButton_Click_002" & "integerSalesTableNoOld -1")
                integerSalesTableNoOld = -1
            End If
            Dim integerSalesTableNoNew As Integer = 0
            If Not Integer.TryParse(salesTableNoNew, integerSalesTableNoNew) Then

                ' Logger.Debug("StaffDetailSubmitButton_Click_003" & "integerSalesTableNoNew -1")
                integerSalesTableNoNew = -1
            End If

            '商談テーブル更新
            '$09 start NSK s.natsume TKM Change request development for Next Gen e-CRB (CR060)
            'msgId = businessLogic.RegistrationNameAndSalesTable(CType(visitSeq, Long), _
            '                                                    String.Empty, _
            '                                                    String.Empty, _
            '                                          integerSalesTableNoOld, _
            '                                          integerSalesTableNoNew, _
            '                                                 context.Account, _
            '                                                           False)
            msgId = businessLogic.RegistrationNameAndSalesTable(CType(visitSeq, Long), _
                                                                 String.Empty, _
                                                                 String.Empty, _
                                                       integerSalesTableNoOld, _
                                                       integerSalesTableNoNew, _
                                                              context.Account, _
                                                                 String.Empty, _
                                                                        False)
            '$09 end NSK s.natsume TKM Change request development for Next Gen e-CRB (CR060)

            ' PUSH送信
            If msgId = MessageIdNormal Then
                businessLogic.SendPush()
            End If

            businessLogic = Nothing

            ' Logger.Debug("StaffDetailSubmitButton_Click_004" & "Call_Start WebWordUtility.GetWord Param[" & ReceptionistId & "," & msgId & "]")
            errorMsg = WebWordUtility.GetWord(ReceptionistId, msgId)
            ' Logger.Debug("StaffDetailSubmitButton_Click_004" & "Call_End WebWordUtility.GetWord Ret[" & errorMsg & "]")

            If String.IsNullOrEmpty(errorMsg) Then
                resultList.Add(ErrorFlagOff)
                resultList.Add(String.Empty)
            Else
                resultList.Add(ErrorFlagOn)
                resultList.Add(errorMsg)
            End If

            Logger.Info("StaffDetailSubmitButton_Click End Ret[" & javaScript.Serialize(resultList) & "]")
            Return javaScript.Serialize(resultList)

        Catch exception As Exception

            'ログ出力
            Logger.Error("StaffDetailSubmitButton_Click Error", exception)
            Throw

        End Try
    End Function
#End Region

#Region " 非公開メソッド"

    ''' <summary>
    ''' 文言管理にDB登録を行い文言番号より取得する
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub InitWord(ByVal wordDictionary As Dictionary(Of Decimal, String))

        VisitDialogTitleLiteral.Text = Server.HtmlEncode(wordDictionary(20))
        '$09 start NSK s.natsume TKM Change request development for Next Gen e-CRB (CR060)
        'CustomerNameInputLiteral.Text = ChangeString(wordDictionary(23), CustomerDialogTitleSize, StringCut)
        CustomerNameInputLiteral.Text = ChangeString(wordDictionary(WordIdNameTelNumberTitle), CustomerDialogTitleSize, StringCut)
        '$09 end NSK s.natsume TKM Change request development for Next Gen e-CRB (CR060)
        CustomerDialogCancelLiteral.Text = Server.HtmlEncode(wordDictionary(21))
        CustomerDialogCompleteLiteral.Text = Server.HtmlEncode(wordDictionary(22))
        ' プレースフォルダはエンコードが不要
        CustomerNameTextBox.Attributes.Add("placeholder", wordDictionary(24))
        '$09 start NSK s.natsume TKM Change request development for Next Gen e-CRB (CR060)
        CustomerTelNumberTextBox.Attributes.Add("placeholder", wordDictionary(WordIdTelNumber))
        '$09 end NSK s.natsume TKM Change request development for Next Gen e-CRB (CR060)
        CustomerDialogBroadcastLiteral1.Text = ChangeString(wordDictionary(25), BroadcastButtonNameSize, StringCut)
        CustomerDialogBroadcastLiteral2.Text = ChangeString(wordDictionary(25), BroadcastButtonNameSize, StringCut)
        CustomerDialogTableNoInputLiteral.Text = ChangeString(wordDictionary(26), CustomerDialogTitleSize, StringCut)
        ' $02 start 新車タブレットショールーム管理機能開発
        CustomerDialogUnNecessaryLiteral1.Text = ChangeString(wordDictionary(70), UnNecessaryButtonNameSize, StringCut)
        CustomerDialogUnNecessaryLiteral2.Text = ChangeString(wordDictionary(70), UnNecessaryButtonNameSize, StringCut)
        StaffDetailDialogTitleLiteral.Text = Server.HtmlEncode(wordDictionary(71))
        StaffDetailDialogNameLiteral.Text = Server.HtmlEncode(wordDictionary(71))
        ' $02 end   新車タブレットショールーム管理機能開発

        ' 商談中詳細画面
        StaffDetailDialogCancelLiteral.Text = Server.HtmlEncode(wordDictionary(37))
        StaffDetailDialogCompleteLiteral.Text = Server.HtmlEncode(wordDictionary(38))
        StaffDetailNegoLiteral.Text = Server.HtmlEncode(wordDictionary(39))
        StaffDetailNowVisitLiteral.Text = Server.HtmlEncode(wordDictionary(40))
        StaffDetailClaimIconLiteral.Text = Server.HtmlEncode(wordDictionary(42))
        StaffDetailVisitPersonLiteral.Text = Server.HtmlEncode(wordDictionary(43))
        StaffDetailTableNoLiteral.Text = Server.HtmlEncode(wordDictionary(44))
        StaffDetailDialogTableTitleLiteral.Text = Server.HtmlEncode(wordDictionary(53))

        ' $01 start 複数顧客に対する商談平行対応
        ' 紐付け解除画面
        'LinkingCancelDialogTitleLiteral.Text = Server.HtmlEncode(wordDictionary(33))
        'LinkingCancelDialogCancelLiteral.Text = Server.HtmlEncode(wordDictionary(34))
        'LinkingCancelDialogCompleteLiteral.Text = Server.HtmlEncode(wordDictionary(35))
        ' $01 end   複数顧客に対する商談平行対応

    End Sub

    ''' <summary>
    ''' お客様氏名・商談テーブルNo.入力画面表示（上段）
    ''' </summary>
    ''' <param name="customerRow"></param>
    ''' <remarks></remarks>
    Private Sub InitCustomerDialogAboveArea(ByVal customerRow As VisitReceptionVisitorCustomerRow)

        CustomerDialogCustomerSegment.Value = If(customerRow.IsCUSTSEGMENTNull(), String.Empty, customerRow.CUSTSEGMENT)
        CustomerDialogSalesTableNoOld.Value = If(customerRow.IsSALESTABLENONull(), String.Empty, CType(customerRow.SALESTABLENO, String))
        CustomerDialogSalesTableNoNew.Value = If(customerRow.IsSALESTABLENONull(), String.Empty, CType(customerRow.SALESTABLENO, String))
        CustomerDialogVehicleRegistrationNo.Value = If(customerRow.IsVCLREGNONull(), String.Empty, Server.HtmlEncode(customerRow.VCLREGNO))

        ' コントロールの状態を初期化
        PopupContactVisitSubmitButtonOn.Visible = False
        PopupContactVisitSubmitButtonOff.Visible = True
        ' $02 start 新車タブレットショールーム管理機能開発
        PopupUnNecessarySubmitButtonOn.Visible = False
        PopupUnNecessarySubmitButtonOff.Visible = True
        ' $02 end   新車タブレットショールーム管理機能開発

        CustomerNameTextBox.Text = String.Empty
        CustomerNameTextBox.Visible = False
        CustomerNameTextBoxLiteral.Text = String.Empty
        CustomerNameTextBoxLiteral.Visible = False

        '$09 start NSK s.natsume TKM Change request development for Next Gen e-CRB (CR060)
        CustomerTelNumberTextBox.Text = String.Empty
        CustomerTelNumberTextBox.Visible = False
        CustomerTelNumberTextBoxLiteral.Text = String.Empty
        CustomerTelNumberTextBoxLiteral.Visible = False
        '$09 end NSK s.natsume TKM Change request development for Next Gen e-CRB (CR060)

        If IsDBNull(customerRow.CUSTSEGMENT) OrElse String.IsNullOrEmpty(customerRow.CUSTSEGMENT) Then

            ' 新規顧客の場合
            CustomerNameTextBoxArea.Attributes("class") = "scNscPopUpContactVisitTextArea"
            CustomerNameTextBox.Visible = True

            '$09 start NSK s.natsume TKM Change request development for Next Gen e-CRB (CR060)
            CustomerTelNumberTextBoxArea.Attributes("class") = "scNscPopUpContactVisitTextArea"
            CustomerTelNumberTextBox.Visible = True
            '$09 end NSK s.natsume TKM Change request development for Next Gen e-CRB (CR060)

            If Not IsDBNull(customerRow.CUSTNAME) AndAlso Not String.IsNullOrEmpty(customerRow.CUSTNAME) Then
                ' テキストボックスの値はエンコード不要
                CustomerNameTextBox.Text = customerRow.CUSTNAME
            End If

            '$09 start NSK s.natsume TKM Change request development for Next Gen e-CRB (CR060)
            If Not IsDBNull(customerRow.TELNUMBER) AndAlso Not String.IsNullOrEmpty(customerRow.TELNUMBER) Then
                ' テキストボックスの値はエンコード不要
                CustomerTelNumberTextBox.Text = customerRow.TELNUMBER
            End If

            '$09 end NSK s.natsume TKM Change request development for Next Gen e-CRB (CR060)

            ' ブロードキャストを行っていない場合
            If BroadcastFlagOff.Equals(customerRow.BROUDCASTFLG) Then
                PopupContactVisitSubmitButtonOn.Visible = True
                PopupContactVisitSubmitButtonOff.Visible = False

            End If

        Else

            ' 新規顧客でない場合
            CustomerNameTextBoxArea.Attributes("class") = "scNscPopUpContactVisitTextArea02 ellipsis"
            CustomerNameTextBoxLiteral.Visible = True

            '$09 start NSK s.natsume TKM Change request development for Next Gen e-CRB (CR060)
            CustomerTelNumberTextBoxArea.Attributes("class") = "scNscPopUpContactVisitTextArea02 ellipsis"
            CustomerTelNumberTextBoxLiteral.Visible = True
            '$09 end NSK s.natsume TKM Change request development for Next Gen e-CRB (CR060)

            If Not IsDBNull(customerRow.CUSTNAME) AndAlso Not String.IsNullOrEmpty(customerRow.CUSTNAME) AndAlso _
                Not String.IsNullOrEmpty(customerRow.CUSTNAME.Trim()) Then

                Dim custName As New StringBuilder

                '敬称の前後位置
                Logger.Info("InitCustomerDialogAboveArea_001" & "Call_Start MyBase.GetValue Param[" & _
                             ScreenPos.Current & "," & SessionKeyNameTitlePos & "," & False & "]")
                Dim nameTitlePos As String = CType(MyBase.GetValue(ScreenPos.Current, SessionKeyNameTitlePos, False), String)
                Logger.Info("InitCustomerDialogAboveArea_001" & "Call_End MyBase.GetValue Ret[" & nameTitlePos.ToString() & "]")

                Dim customerNameTitle As String = customerRow.CUSTNAMETITLE

                If IsDBNull(customerRow.CUSTNAMETITLE) OrElse String.IsNullOrEmpty(customerRow.CUSTNAMETITLE) OrElse _
                    String.IsNullOrEmpty(customerRow.CUSTNAMETITLE.Trim()) Then

                    customerNameTitle = String.Empty

                End If

                '敬称の前後位置
                If nameTitlePos.Equals(NameTitlePositionFront) Then
                    custName.Append(customerNameTitle)
                    custName.Append(customerRow.CUSTNAME)
                Else
                    custName.Append(customerRow.CUSTNAME)
                    custName.Append(customerNameTitle)
                End If

                CustomerNameTextBoxLiteral.Text = ChangeString(custName.ToString, CustomerDialogCustomerNameSize, StringAdd)
                '$09 start NSK s.natsume TKM Change request development for Next Gen e-CRB (CR060)
                CustomerTelNumberTextBoxLiteral.Text = customerRow.TELNUMBER
                '$09 end NSK s.natsume TKM Change request development for Next Gen e-CRB (CR060)
            Else

                ' 文言管理
                Logger.Info("InitCustomerDialogAboveArea_002" & "Call_Start MyBase.GetValue Param[" & _
                             ScreenPos.Current & "," & SessionKeyWordDictionary & "," & False & "]")
                Dim wordDictionary As Dictionary(Of Decimal, String) = _
                    CType(MyBase.GetValue(ScreenPos.Current, SessionKeyWordDictionary, False), Dictionary(Of Decimal, String))
                Logger.Info("InitCustomerDialogAboveArea_002" & "Call_End MyBase.GetValue Ret[" & wordDictionary.ToString() & "]")

                CustomerNameTextBoxLiteral.Text = Server.HtmlEncode(wordDictionary(31))

                '$09 start NSK s.natsume TKM Change request development for Next Gen e-CRB (CR060)
                CustomerTelNumberTextBoxLiteral.Text = Server.HtmlEncode(wordDictionary(WordIdTelNumber))
                '$09 end NSK s.natsume TKM Change request development for Next Gen e-CRB (CR060)
            End If
        End If

        ' $02 start 新車タブレットショールーム管理機能開発
        ' 来店実績ステータスがフリーor接客不要の場合に接客不要ボタンを活性化する
        If VisitStatusFree.Equals(customerRow.VISITSTATUS) OrElse VisitStatusUnNecessary.Equals(customerRow.VISITSTATUS) Then
            PopupUnNecessarySubmitButtonOn.Visible = True
            PopupUnNecessarySubmitButtonOff.Visible = False
        End If
        ' $02 end   新車タブレットショールーム管理機能開発

    End Sub

    ''' <summary>
    ''' お客様氏名・商談テーブルNo.入力画面表示（下段）
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub InitCustomerDialogUnderArea()

        'ログインユーザの情報を格納
        ' Logger.Debug("InitCustomerDialogUnderArea_001" & "Call_Start StaffContext.Current")
        Dim context As StaffContext = StaffContext.Current
        ' Logger.Debug("InitCustomerDialogUnderArea_001" & "Call_End StaffContext.Current Ret[" & context.ToString & "]")

        '現在日時 基盤より取得
        ' Logger.Debug("InitCustomerDialogUnderArea_002" & "Call_Start DateTimeFunc.Now Param[" & context.DlrCD & "]")
        Dim nowDate As Date = DateTimeFunc.Now(context.DlrCD)
        ' Logger.Debug("InitCustomerDialogUnderArea_002" & "Call_End DateTimeFunc.Now Ret[" & nowDate & "]")

        '商談テーブル使用有無の取得
        Dim salesTableInfoDataTable As SC3100101SalesTableUseDataTable = Nothing
        Dim businessLogic As New SC3100101BusinessLogic
        salesTableInfoDataTable = businessLogic.GetSalesTableInfo(context.DlrCD, context.BrnCD, nowDate)
        businessLogic = Nothing

        SalesTableNoRepeater.DataSource = salesTableInfoDataTable
        SalesTableNoRepeater.DataBind()

        For i = 0 To SalesTableNoRepeater.Items.Count - 1

            Dim salesTable As Control = SalesTableNoRepeater.Items(i)

            Dim salesTableNoData As String = CType(salesTable.FindControl("SelectSalesTableNo"), HiddenField).Value
            Dim salesTableDataRow As SC3100101SalesTableUseRow = salesTableInfoDataTable.Rows(i)
            Dim shiyoFlgData As String = salesTableDataRow.SHIYOFLG
            salesTableDataRow = Nothing

            If String.IsNullOrEmpty(salesTableNoData) Then
                Exit For
            End If

            ' 行の区切り
            If ((i + 1) Mod 4) = 1 Then
                CType(salesTable.FindControl("SalesTableAreaLeft"), Literal).Visible = True
            ElseIf ((i + 1) Mod 4) = 0 Then
                CType(salesTable.FindControl("SalesTableAreaRight"), Literal).Visible = True
            Else
                CType(salesTable.FindControl("SalesTableAreaCenter"), Literal).Visible = True
            End If

            If UsedFlagUsed.Equals(shiyoFlgData) Then
                CType(salesTable.FindControl("SalesTableNoSelected"), Literal).Visible = True
            Else
                CType(salesTable.FindControl("SalesTableNoOff"), Literal).Visible = True
            End If

            CType(salesTable.FindControl("SalesTableNoLiteral"), Literal).Text = _
                ChangeString(salesTableNoData, CustomerDialogSalesTableNoSize, StringCut)

        Next

    End Sub

    ''' <summary>
    ''' フッターエリアの初期化
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub InitFooter()

        Dim context As StaffContext = StaffContext.Current

        ' ----------------------------------------------------
        ' 親メニュー
        ' ----------------------------------------------------
        'メニューボタン
        Logger.Info("InitFooter_002" & "Call_Start GetFooterButton Param[" & FooterMenuCategory.MainMenu & "]")
        Dim mainManuButton As CommonMasterFooterButton = commonMasterPage.GetFooterButton(FooterMenuCategory.MainMenu)
        Logger.Info("InitFooter_002" & "Call_End GetFooterButton Ret[" & mainManuButton.ToString & "]")

        AddHandler mainManuButton.Click, _
          Sub()
              ' Logger.Debug("MainManuButton_Click Param[]")

              If context.OpeCD = OperationCdReception Then

                  Logger.Info("MainManuButton_Click_001" & "Call_Start MyBase.SetValue[" & _
                      ScreenPos.Next & "," & SessionKeyBeforeFooterId & "," & FooterMenuCategory.MainMenu & "]")
                  MyBase.SetValue(ScreenPos.Next, SessionKeyBeforeFooterId, FooterMenuCategory.MainMenu)

                  '受付は受付メインへ
                  ' 遷移処理(親フレームに遷移する)
                  Logger.Info("MainManuButton_Click_002 " & "Call_Start Me.RedirectNextScreen Param[SC3100101]")
                  Me.RedirectNextScreen("SC3100101")
                  Logger.Info("MainManuButton_Click Ret[]")
              Else

                  'SSMはSCメインへ
                  Logger.Info("MainManuButton_Click_003 " & "Call_Start Me.RedirectNextScreen Param[SC3010203]")
                  Me.RedirectNextScreen("SC3010203")
                  Logger.Info("MainManuButton_Click Ret[]")
              End If
          End Sub

        ' ショールームステータスボタン
        Logger.Info("InitFooter_003" & "Call_Start GetFooterButton Param[" & FooterIdSubmenuShowRoomStatus & "]")
        Dim submenuShowRoomLink As CommonMasterFooterButton = commonMasterPage.GetFooterButton(FooterIdSubmenuShowRoomStatus)
        Logger.Info("InitFooter_003" & "Call_End GetFooterButton Ret[" & submenuShowRoomLink.ToString & "]")

        AddHandler submenuShowRoomLink.Click, _
            Sub()
                ' Logger.Debug("SubmenuShowRoomLink_Click Param[]")

                If context.OpeCD = OperationCdReception Then

                    Logger.Info("SubmenuShowRoomLink_Click_001" & "Call_Start MyBase.SetValue[" & _
                        ScreenPos.Next & "," & SessionKeyBeforeFooterId & "," & FooterIdSubmenuShowRoomStatus & "]")
                    MyBase.SetValue(ScreenPos.Next, SessionKeyBeforeFooterId, FooterIdSubmenuShowRoomStatus)

                End If

                'メニューに遷移
                Logger.Info("SubmenuShowRoomLink_Click_002 " & "Call_Start Me.RedirectNextScreen Param[SC3100101]")
                Me.RedirectNextScreen("SC3100101")
                Logger.Info("SubmenuShowRoomLink_Click Ret[]")
            End Sub


        ' ----------------------------------------------------
        ' 子メニュー
        ' ----------------------------------------------------
        If context.OpeCD = OperationCdReception Then
            ' Logger.Debug("InitFooter_004" & "Call_End context.OpeCD = " & OperationCdReception)

            Dim beforeFooterId As String = GetBeforeFooterId()

            '更新権限の場合
            'メインメニューから遷移
            If beforeFooterId = FooterMenuCategory.MainMenu Then

                'スケジュールボタン
                Logger.Info("InitFooter_005" & "Call_Start GetFooterButton Param[" & FooterIdSubmenuSchedule & "]")
                Dim scheduleButton As CommonMasterFooterButton = CType(Me.Master, CommonMasterPage).GetFooterButton(FooterIdSubmenuSchedule)
                Logger.Info("InitFooter_005" & "Call_End GetFooterButton Ret[" & scheduleButton.ToString & "]")

                scheduleButton.OnClientClick = "return displayCale();"

                '連絡先ボタン
                Logger.Info("InitFooter_006" & "Call_Start GetFooterButton Param[" & FooterIdSubmenuCont & "]")
                Dim contButton As CommonMasterFooterButton = CType(Me.Master, CommonMasterPage).GetFooterButton(FooterIdSubmenuCont)
                Logger.Info("InitFooter_006" & "Call_End GetFooterButton Ret[" & contButton.ToString & "]")

                contButton.OnClientClick = "return displayCont();"
            Else

                ' 試乗車ボタン
                Logger.Info("InitFooter_007" & "Call_Start GetFooterButton Param[" & FooterIdSubmenuTestDrive & "]")
                Dim testDriveLink As CommonMasterFooterButton = commonMasterPage.GetFooterButton(FooterIdSubmenuTestDrive)
                Logger.Info("InitFooter_007" & "Call_End GetFooterButton Ret[" & testDriveLink.ToString & "]")

                testDriveLink.OnClientClick = "return false;"

                ' $01 start スタンバイスタッフ並び順変更対応
                ' スタンバイスタッフ並び順変更ボタン
                Logger.Info("InitFooter_008" & "Call_Start GetFooterButton Param[" & FooterIdSubmenuStandByStaff & "]")
                Dim standByStaffLink As CommonMasterFooterButton = commonMasterPage.GetFooterButton(FooterIdSubmenuStandByStaff)
                Logger.Info("InitFooter_008" & "Call_End GetFooterButton Ret[" & standByStaffLink.ToString & "]")

                standByStaffLink.OnClientClick = "return false;"
                ' $01 end   スタンバイスタッフ並び順変更対応

                ' $05 start 次世代e-CRBセールス機能 新DB適応に向けた機能開発
                ' 来店チップ作成ボタン
                Logger.Info("InitFooter_009" & "Call_Start GetFooterButton Param[" & FooterIdSubmenuCreateCustomerChip & "]")
                Dim createCustomerChipLink As CommonMasterFooterButton = commonMasterPage.GetFooterButton(FooterIdSubmenuCreateCustomerChip)
                Logger.Info("InitFooter_009" & "Call_End GetFooterButton Ret[" & createCustomerChipLink.ToString & "]")

                createCustomerChipLink.OnClientClick = "return false;"
                ' $05 end   次世代e-CRBセールス機能 新DB適応に向けた機能開発

            End If
        Else
            '読取専用権限の場合

            '顧客ボタン
            Logger.Info("InitFooter_009" & "Call_Start GetFooterButton Param[" & FooterMenuCategory.Customer & "]")
            Dim customerButton As CommonMasterFooterButton = CType(Me.Master, CommonMasterPage).GetFooterButton(FooterMenuCategory.Customer)
            Logger.Info("InitFooter_009" & "Call_End GetFooterButton Ret[" & customerButton.ToString & "]")

            '非表示にする
            customerButton.Visible = False

            'TCVボタン
            Logger.Info("InitFooter_010" & "Call_Start GetFooterButton Param[" & FooterMenuCategory.TCV & "]")
            Dim tcvButton As CommonMasterFooterButton = CType(Me.Master, CommonMasterPage).GetFooterButton(FooterMenuCategory.TCV)
            Logger.Info("InitFooter_010" & "Call_End GetFooterButton Ret[" & tcvButton.ToString & "]")
            AddHandler tcvButton.Click, AddressOf tcvButton_Click

        End If

        ' Logger.Debug("InitFooter_End Ret[]")

    End Sub

    ''' <summary>
    ''' TCSとの連携ボタン
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub tcvButton_Click(ByVal sender As Object, ByVal e As CommonMasterFooterButtonClickEventArgs)

        ' Logger.Debug("tcvButton_Click Param[]")

        ' 処理停止フラグを設定する
        LogicStopStatus.Value = "1"

        Dim context As StaffContext = StaffContext.Current

        'TCV機能に渡す引数を設定
        e.Parameters.Add("DataSource", "none")
        Logger.Info("tcvButton_Click Parameters DataSource[" & "none" & "]")
        e.Parameters.Add("MenuLockFlag", False)
        Logger.Info("tcvButton_Click Parameters MenuLockFlag[" & "False" & "]")
        e.Parameters.Add("Account", context.Account)
        Logger.Info("tcvButton_Click Parameters Account[" & context.Account & "]")
        e.Parameters.Add("AccountStrCd", context.BrnCD)
        Logger.Info("tcvButton_Click Parameters AccountStrCd[" & context.BrnCD & "]")
        e.Parameters.Add("DlrCd", context.DlrCD)
        Logger.Info("tcvButton_Click Parameters DlrCd[" & context.DlrCD & "]")
        e.Parameters.Add("StrCd", String.Empty)
        Logger.Info("tcvButton_Click Parameters StrCd[" & String.Empty & "]")
        e.Parameters.Add("FollowupBox_SeqNo", String.Empty)
        Logger.Info("tcvButton_Click Parameters FollowupBox_SeqNo[" & String.Empty & "]")
        e.Parameters.Add("CstKind", String.Empty)
        Logger.Info("tcvButton_Click Parameters CstKind[" & String.Empty & "]")
        e.Parameters.Add("CustomerClass", String.Empty)
        Logger.Info("tcvButton_Click Parameters CustomerClass[" & String.Empty & "]")
        e.Parameters.Add("CRCustId", String.Empty)
        Logger.Info("tcvButton_Click Parameters CRCustId[" & String.Empty & "]")
        e.Parameters.Add("OperationCode", context.OpeCD)
        Logger.Info("tcvButton_Click Parameters OperationCode[" & context.OpeCD & "]")
        e.Parameters.Add("BusinessFlg", False)
        Logger.Info("tcvButton_Click Parameters BusinessFlg[" & "False" & "]")
        e.Parameters.Add("ReadOnlyFlg", False)
        Logger.Info("tcvButton_Click Parameters ReadOnlyFlg[" & "False" & "]")

        ' Logger.Debug("tcvButton_Click Ret[]")
    End Sub

    ''' <summary>
    ''' 遷移元メニューのフッターボタンIDを取得する
    ''' </summary>
    ''' <remarks></remarks>
    Private Function GetBeforeFooterId() As Integer

        ' 値が設定されていない場合はメインメニューからの遷移とする
        Dim beforeFooterId As Integer = FooterMenuCategory.MainMenu

        Try

            Dim beforeFooterIdObject As Object = Nothing

            Logger.Info("GetBeforeFooterId" & "Call_Start MyBase.GetValue Param[" & _
                         ScreenPos.Current & "," & SessionKeyBeforeFooterId & "," & False & "]")
            beforeFooterIdObject = MyBase.GetValue(ScreenPos.Current, SessionKeyBeforeFooterId, False)
            Logger.Info("GetBeforeFooterId" & "Call_End MyBase.GetValue Ret[" & beforeFooterIdObject & "]")

            If beforeFooterIdObject IsNot Nothing Then

                beforeFooterId = CType(beforeFooterIdObject, Integer)

            End If

        Catch exception As KeyNotFoundException
            ' Logger.Debug("GetBeforeFooterId Catch KeyNotFoundException")
        End Try

        Return beforeFooterId

    End Function

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

#Region "操作権限コード取得"

    ''' <summary>
    ''' 操作権限の取得
    ''' </summary>
    ''' <param name="target">対象アカウントの権限</param>
    ''' <returns>1:更新権限、2:読み取り専用権限</returns>
    ''' <remarks></remarks>
    Private Function GetOperationCode(ByVal target As Decimal) As Integer

        '初期状態:読み取り専用
        Dim operationStatus As String = StatusReadOnly

        '操作権限コードリストの取得
        Dim sysEnvSet As New SystemEnvSetting
        Dim sysEnvSetOperationListRow As SystemEnvSettingDataSet.SYSTEMENVSETTINGRow = Nothing

        Logger.Info("GetOperationCode_001" & "Call_Start GetSystemEnvSetting Param[" & NameTitlePotision & "]")
        sysEnvSetOperationListRow = sysEnvSet.GetSystemEnvSetting(UpdateCodeList)
        Logger.Info("GetOperationCode_001" & "Call_End GetSystemEnvSetting Ret[" & IsDBNull(sysEnvSetOperationListRow) & "]")

        ' 環境変数が設定されていない場合は読み取り専用とする
        If sysEnvSetOperationListRow Is Nothing Then
            Return operationStatus
        End If

        Dim operationListName As String = sysEnvSetOperationListRow.PARAMVALUE
        Dim operationCdList As String()

        'カンマ区切りで取得
        operationCdList = operationListName.Split(",")

        For Each operation In operationCdList
            If CType(operation, Decimal) = target Then

                '更新に切り替えてforを抜ける
                operationStatus = StatusUpdate
                Exit For
            End If
        Next

        Return operationStatus
    End Function
#End Region

#Region "商談中詳細画面表示(依頼リスト)"

    ''' <summary>
    ''' 商談中詳細画面表示(依頼リスト)
    ''' </summary>
    ''' <param name="visitSeq">シーケンス番号</param>
    ''' <param name="nowDate">現在日時</param>
    ''' <remarks></remarks>
    Private Sub InitStaffDetailDialogNoticeListArea(ByVal visitSeq As Long, _
                                                    ByVal nowDate As Date)
        ' Logger.Debug("InitStaffDetailDialogNoticeListArea_Start " & _
        '            "Param[" & visitSeq & "," & context.ToString & "," & nowDate & "]")

        ' $02 start 新車タブレットショールーム管理機能開発
        Dim staffNoticeRequestDataTable As VisitReceptionStaffNoticeRequestDataTable = Nothing
        Dim businessLogic As New SC3100101BusinessLogic
        ' $02 end   新車タブレットショールーム管理機能開発
        staffNoticeRequestDataTable = businessLogic.GetStaffNoticeRequest(visitSeq)
        businessLogic = Nothing


        'リピータをバインド(0件の場合はバインドしない)
        If staffNoticeRequestDataTable.Count <= 0 Then

            ' Logger.Debug("InitStaffDetailDialogNoticeListArea_001 staffNoticeRequestDataTable.Count <= 0")
            Me.NoticeListRepeater.Visible = False
            ' Logger.Debug("InitStaffDetailDialogNoticeListArea_End Ret[]")
            Exit Sub
        End If

        ' $02 start 新車タブレットショールーム管理機能開発
        ' 取得した依頼情報を異常までの時間が短い順にソートする。
        Dim staffNoticeRequestDataTableSort As VisitReceptionStaffNoticeRequestDataTable = _
            SortRequestList(staffNoticeRequestDataTable)
        ' $02 end   新車タブレットショールーム管理機能開発

        ' 依頼通知送信日時時間リストを設定
        SendDateList.Value = GetTimeSpanListString(staffNoticeRequestDataTableSort, "SENDDATE", nowDate)

        Logger.Info("InitStaffDetailDialogNoticeListArea_003 staffNoticeRequestDataTable.Count = " & staffNoticeRequestDataTableSort.Count)
        Me.NoticeListRepeater.Visible = True
        Me.NoticeListRepeater.DataSource = staffNoticeRequestDataTableSort
        Me.NoticeListRepeater.DataBind()

        Dim maxLength As Integer = NoticeListRepeater.Items.Count - 1

        '取得データの格納
        For i = 0 To maxLength

            Dim item As Control = NoticeListRepeater.Items(i)
            ' $02 start 新車タブレットショールーム管理機能開発
            Dim staffNoticeRequestDataRow As VisitReceptionStaffNoticeRequestRow = staffNoticeRequestDataTableSort.Item(i)
            ' $02 end   新車タブレットショールーム管理機能開発

            '----------------------------------------------------------------------
            ' 依頼種別
            '----------------------------------------------------------------------

            Dim NoticeReqctg As String = staffNoticeRequestDataRow.NOTICEREQCTG
            CType(item.FindControl("NoticeReqctg"), HiddenField).Value = NoticeReqctg

            Dim NoticeListTag As New StringBuilder

            NoticeListTag.Append(CType(item.FindControl("NoticeName"), HtmlGenericControl).Attributes("class"))

            'タグの開始と終了判定
            If i = 0 Then
                NoticeListTag.Append(" listTop")
            ElseIf i = maxLength Then
                NoticeListTag.Append(" listBottom")
            Else
                NoticeListTag.Append(" listCenter")
            End If

            '依頼種別の判定
            Select Case NoticeReqctg

                Case NoticeAssessment
                    '査定
                    NoticeListTag.Append(" list1On")

                Case NoticePriceConsultation
                    '価格相談
                    NoticeListTag.Append(" list3On")

                Case NoticeHelp
                    'ヘルプ
                    NoticeListTag.Append(" list4On")

            End Select

            CType(item.FindControl("NoticeName"), HtmlGenericControl).Attributes("class") = NoticeListTag.ToString
            CType(item.FindControl("NoticeNameLiteral"), Literal).Visible = True

            '査定の場合
            If NoticeReqctg = NoticeAssessment Then

                ' Logger.Debug("InitStaffDetailDialogNoticeListArea_002 staffNoticeRequestDataRow.NOTICEREQCTG = " & NoticeAssessment)
                Dim wordDictionary As Dictionary(Of Decimal, String) = _
                CType(MyBase.GetValue(ScreenPos.Current, SessionKeyWordDictionary, False), Dictionary(Of Decimal, String))

                '中古車スタッフ表示
                CType(item.FindControl("NoticeNameLiteral"), Literal).Text = Server.HtmlEncode(wordDictionary(41))

            Else

                ' Logger.Debug("InitStaffDetailDialogNoticeListArea_003 staffNoticeRequestDataRow.NOTICEREQCTG <> " & NoticeAssessment)
                If Not String.IsNullOrEmpty(staffNoticeRequestDataRow.FROMACCOUNTNAME) Then

                    '送信者名が存在する場合
                    ' Logger.Debug("InitStaffDetailDialogNoticeListArea_004 staffNoticeRequestDataRow.FROMACCOUNTNAME = " & staffNoticeRequestDataRow.FROMACCOUNTNAME)
                    CType(item.FindControl("NoticeNameLiteral"), Literal).Text = Server.HtmlEncode(staffNoticeRequestDataRow.FROMACCOUNTNAME)
                Else

                    '上記以外の場合
                    ' Logger.Debug("InitStaffDetailDialogNoticeListArea_005 staffNoticeRequestDataRow.FROMACCOUNTNAME is NullOrEmpty")
                    CType(item.FindControl("NoticeNameLiteral"), Literal).Text = Server.HtmlEncode(staffNoticeRequestDataRow.TOACCOUNTNAME)
                End If
            End If
        Next

        ' Logger.Debug("InitStaffDetailDialogNoticeListArea_End Ret[]")
    End Sub

    ' $02 start 新車タブレットショールーム管理機能開発
    ''' <summary>
    ''' 依頼情報表示用の並び替えを行う。
    ''' </summary>
    ''' <param name="targetDataTable">VisitReceptionStaffNoticeRequestDataTable</param>
    ''' <returns>ソート後のVisitReceptionStaffNoticeRequestDataTable</returns>
    ''' <remarks></remarks>
    Private Function SortRequestList(ByVal targetDataTable As VisitReceptionStaffNoticeRequestDataTable) _
        As VisitReceptionStaffNoticeRequestDataTable

        Dim requestAssessmentDate As DateTime = Nothing
        Dim requestPriceDate As DateTime = Nothing
        Dim requestHelpDate As DateTime = Nothing

        ' ソート用配列を作成
        Dim keyArray As ArrayList = New ArrayList
        Dim valueArray As ArrayList = New ArrayList
        Dim addTime As Int32 = 0
        For Each row As VisitReceptionStaffNoticeRequestRow In targetDataTable.Rows
            addTime = 0
            Select Case row.NOTICEREQCTG
                Case "01"
                    ' 査定
                    addTime = CInt(AssessmentAlertSpan.Value)
                Case "02"
                    ' 価格相談
                    addTime = CInt(PriceAlertSpan.Value)
                Case "03"
                    ' ヘルプ
                    addTime = CInt(HelpAlertSpan.Value)
            End Select
            keyArray.Add(row)
            If addTime = 0 Then
                valueArray.Add(Nothing)
            Else
                valueArray.Add(CType(row.SENDDATE, DateTime).AddSeconds(addTime))
            End If
        Next

        'keyArrayの値をキーにしてソート
        Dim sortArr(1)() As Object
        sortArr(0) = keyArray.ToArray
        sortArr(1) = valueArray.ToArray
        Array.Sort(sortArr(1), sortArr(0))
        Array.Sort(sortArr(1), sortArr(1))

        Dim returnDataTable As New VisitReceptionStaffNoticeRequestDataTable
        For index As Integer = 0 To sortArr(0).Length - 1
            If Not sortArr(1)(index) = Nothing Then
                returnDataTable.ImportRow(sortArr(0)(index))
            End If
        Next

        Return returnDataTable
    End Function
    ' $02 end   新車タブレットショールーム管理機能開発

#End Region

#Region "商談中詳細画面表示(顧客詳細)"

    ''' <summary>
    ''' 商談中詳細画面表示(顧客詳細)
    ''' </summary>
    ''' <param name="customerRow">表示する顧客情報</param>
    ''' <param name="context">スタッフコンテキスト</param>
    ''' <param name="nowDate">現在日時</param>
    ''' <remarks></remarks>
    ''' <history>
    ''' 2019/06/26 NSK 鈴木 [TKM]UAT-0512 組織を超えて顧客詳細が編集できる【18PRJ02275-00 (FS)営業スタッフ納期遵守オペレーション確立に向けた試験研究】
    ''' </history>
    Private Sub InitStaffDetailDialogVisitInfoArea(ByVal customerRow As VisitReceptionVisitorCustomerRow, _
                                                   ByVal context As StaffContext, _
                                                   ByVal nowDate As Date)
        ' Logger.Debug("InitStaffDetailDialogVisitInfoArea_Start " & _
        '            "Param[" & customerRow.ToString & "," & context.ToString & "," & nowDate & "]")

        Logger.Info("InitStaffDetailDialogVisitInfoArea_000 " & "Call_Start MyBase.GetValue Param[" & _
                     ScreenPos.Current & "," & SessionKeyWordDictionary & "," & False & "]")
        Dim wordDictionary As Dictionary(Of Decimal, String) = _
         CType(MyBase.GetValue(ScreenPos.Current, SessionKeyWordDictionary, False), Dictionary(Of Decimal, String))
        Logger.Info("InitStaffDetailDialogVisitInfoArea_000 " & "Call_End MyBase.GetValue Ret[" & wordDictionary.ToString() & "]")


        '顧客区分
        StaffDetailDialogCustomerSegment.Value = If(customerRow.IsCUSTSEGMENTNull(), String.Empty, customerRow.CUSTSEGMENT)

        '顧客コード
        StaffDetailDialogCustId.Value = If(customerRow.IsCUSTIDNull(), String.Empty, customerRow.CUSTID)


        'テーブルNo.(保持用、変更用)
        StaffDetailDialogSalesTableNoOld.Value = If(customerRow.IsSALESTABLENONull(), String.Empty, CType(customerRow.SALESTABLENO, String))
        StaffDetailDialogSalesTableNoNew.Value = If(customerRow.IsSALESTABLENONull(), String.Empty, CType(customerRow.SALESTABLENO, String))

        'テーブルNo.(表示用)
        If (customerRow.IsSALESTABLENONull()) Then

            StaffDetailTableNoLiteral.Attributes("class") = "off"
            DisplayTableNo.Text = DataNull
        Else

            StaffDetailTableNoLiteral.Attributes("class") = "on"
            DisplayTableNo.Text = CType(customerRow.SALESTABLENO, String)
        End If


        '商談開始時間
        StaffDetailDialogSalesStartTime.Value = If(customerRow.IsSALESSTARTNull(), String.Empty, _
                                                   CType(Math.Round(nowDate.Subtract(CType(customerRow.SALESSTART, Date)).TotalSeconds), String))

        'ヘッダー、スタッフ名の色、テーブルNoの色の判定
        If GetOperationCode(CType(context.OpeCD, Decimal)) = StatusUpdate Then

            '受付権限の場合
            Me.scNscPopUpStaffDetailCompleteButton.Attributes("class") = "scNscPopUpStaffDetailCompleteButton"
            Me.arrowPicture.Visible = True
            Me.TableNoLink.Attributes("class") = "list3 FontBlue ellipsis"
        Else

            '受付権限以外の場合
            Me.scNscPopUpStaffDetailCompleteButton.Attributes("class") = "scNscPopUpStaffDetailCompleteButtonOff"
            Me.arrowPicture.Visible = False
            Me.CustomerNameLink.Attributes("class") = "list1 FontBlue ellipsis"
        End If

        '来店人数
        If customerRow.IsVISITPERSONNUMNull() Then
            VisitPersonNumLiteral.Text = DataNull
            StaffDetailVisitPersonLiteral.Visible = False
        Else
            VisitPersonNumLiteral.Text = CType(customerRow.VISITPERSONNUM, String)
            StaffDetailVisitPersonLiteral.Visible = True
        End If

        '来店回数
        '$04 start FllowUp-Box連番桁変更対応
        Dim fllwUpVisitCount As Decimal _
            = If(customerRow.IsFLLOWUPBOX_SEQNONull(), 0, customerRow.FLLOWUPBOX_SEQNO)
        ' $04 start FllowUp-Box連番桁変更対応
        ' $02 start 新車タブレットショールーム管理機能開発
        Dim visitCountDataTable As VisitReceptionVisitCountDataTable = Nothing
        Dim businessLogic As New SC3100101BusinessLogic
        ' $02 end   新車タブレットショールーム管理機能開発

        ' $04 start FllowUp-Box連番桁変更対応
        visitCountDataTable = businessLogic.GetVisitCount(context.DlrCD, context.BrnCD, fllwUpVisitCount)
        ' $04 start FllowUp-Box連番桁変更対応

        businessLogic = Nothing
        VisitCountLiteral.Text = visitCountDataTable.Item(0)(0) + 1

        ' 顧客エリア
        If Not IsDBNull(customerRow.CUSTNAME) AndAlso Not String.IsNullOrEmpty(customerRow.CUSTNAME) _
            AndAlso Not String.IsNullOrEmpty(customerRow.CUSTNAME.Trim()) Then

            ' Logger.Debug("InitStaffDetailDialogVisitInfoArea_001 customerRow.CUSTNAME = " & customerRow.CUSTNAME)
            '顧客名に情報(お客様名又は仮登録氏名)がある場合
            Dim custName As New StringBuilder
            Dim customerNameTitle As String

            '敬称の前後位置情報取得
            Logger.Info("InitStaffDetailDialogVisitInfoArea_002" & "Call_Start MyBase.GetValue Param[" & _
                         ScreenPos.Current & "," & SessionKeyNameTitlePos & "," & False & "]")
            Dim nameTitlePos As String = CType(MyBase.GetValue(ScreenPos.Current, SessionKeyNameTitlePos, False), String)
            Logger.Info("InitStaffDetailDialogVisitInfoArea_002" & "Call_End MyBase.GetValue Ret[" & nameTitlePos.ToString() & "]")

            '敬称があるか判定
            If IsDBNull(customerRow.CUSTNAMETITLE) OrElse String.IsNullOrEmpty(customerRow.CUSTNAMETITLE) OrElse _
               String.IsNullOrEmpty(customerRow.CUSTNAMETITLE.Trim()) Then

                customerNameTitle = String.Empty
            Else
                customerNameTitle = customerRow.CUSTNAMETITLE
            End If

            '敬称の前後位置
            If nameTitlePos.Equals(NameTitlePositionFront) Then
                custName.Append(customerNameTitle)
                custName.Append(customerRow.CUSTNAME)
            Else
                custName.Append(customerRow.CUSTNAME)
                custName.Append(customerNameTitle)
            End If

            StaffDetailCustomerName.Text = ChangeString(custName.ToString, CustomerDialogCustomerNameSize, StringAdd)

        Else

            '顧客詳細に遷移しないようにする
            Me.CustomerNameLink.Attributes("class") = "list1NoLink ellipsis"
            If customerRow.IsCUSTSEGMENTNull() OrElse String.IsNullOrEmpty(customerRow.CUSTSEGMENT) Then

                '新規顧客の場合(新規お客様)
                StaffDetailCustomerName.Text = Server.HtmlEncode(wordDictionary(32))
            Else

                '既存顧客の場合(Unknown)
                StaffDetailCustomerName.Text = Server.HtmlEncode(wordDictionary(31))
            End If
        End If
        Logger.Info("InitStaffDetailDialogVisitInfoArea_005 StaffDetailCustomerName.Text = " & StaffDetailCustomerName.Text)

        ' 2019/06/26 NSK 鈴木 [TKM]UAT-0512 組織を超えて顧客詳細が編集できる【18PRJ02275-00 (FS)営業スタッフ納期遵守オペレーション確立に向けた試験研究】 START
        ' 基盤からログイン者情報を取得する
        Dim staffOparationCode As Operation = context.OpeCD

        ' 表示する顧客情報から情報を取得する
        Dim customerAccount As String = customerRow.ACCOUNT

        ' 対応担当スタッフコードが自組織及び配下に含まれているか判定する。
        Dim IsMyTeamMemberFlg As Boolean = ActivityInfoBusinessLogic.IsMyTeamMember(customerAccount)

        ' 操作権限コードがセールスリーダ（セールススタッフ）（8）かつ、
        ' 対応担当スタッフコードが自組織及び配下に含まれていない場合
        If staffOparationCode.Equals(Operation.SL) And Not IsMyTeamMemberFlg Then
            ' 顧客詳細に遷移しないようにする
            Me.CustomerNameLink.Attributes("class") = "list1NoLink ellipsis"
        End If
        ' 2019/06/26 NSK 鈴木 [TKM]UAT-0512 組織を超えて顧客詳細が編集できる【18PRJ02275-00 (FS)営業スタッフ納期遵守オペレーション確立に向けた試験研究】 END

        ' お客様の苦情情報の件数取得
        Logger.Info("InitStaffDetailDialogVisitInfoArea_006 " & "Call_Start MyBase.GetValue Param[" & _
                     ScreenPos.Current & "," & SessionKeyComplaintDateCount & "," & False & "]")
        Dim complaintDateCount As Integer = CType(MyBase.GetValue(ScreenPos.Current, SessionKeyComplaintDateCount, False), Integer)
        Logger.Info("InitStaffDetailDialogVisitInfoArea_006 " & "Call_End MyBase.GetValue Ret[" & complaintDateCount & "]")


        Dim utility As New VisitUtilityBusinessLogic
        Me.ClaimIcon.Visible = utility.HasClaimInfo(StaffDetailDialogCustomerSegment.Value, _
                                        StaffDetailDialogCustId.Value, _
                                        nowDate, _
                                        complaintDateCount)

        ' Logger.Debug("InitStaffDetailDialogVisitInfoArea_End Ret[]")
    End Sub

#End Region

#Region "商談中詳細画面表示(テーブル選択)"

    ''' <summary>
    ''' 商談中詳細画面表示(テーブル選択)
    ''' </summary>
    ''' <param name="context">スタッフコンテキスト</param>
    ''' <param name="nowDate">現在日時</param>
    ''' <remarks></remarks>
    Private Sub InitStaffDetailDialogTableArea(ByVal context As StaffContext, ByVal nowDate As Date)
        ' Logger.Debug("InitStaffDetailDialogTableArea_Start " & _
        '            "Param[" & context.ToString & "," & nowDate & "]")

        '商談テーブル使用有無の取得
        Dim salesTableInfoDataTable As SC3100101SalesTableUseDataTable = Nothing
        Dim businessLogic As New SC3100101BusinessLogic
        salesTableInfoDataTable = businessLogic.GetSalesTableInfo(context.DlrCD, context.BrnCD, nowDate)
        businessLogic = Nothing

        StaffDetailSalesTableNoRepeater.DataSource = salesTableInfoDataTable
        StaffDetailSalesTableNoRepeater.DataBind()

        For i = 0 To StaffDetailSalesTableNoRepeater.Items.Count - 1

            Dim salesTable As Control = StaffDetailSalesTableNoRepeater.Items(i)

            Dim salesTableNoData As String = CType(salesTable.FindControl("SelectSalesTableNo"), HiddenField).Value
            Dim salesTableDataRow As SC3100101SalesTableUseRow = salesTableInfoDataTable.Rows(i)
            Dim shiyoFlgData As String = salesTableDataRow.SHIYOFLG
            salesTableDataRow = Nothing

            If String.IsNullOrEmpty(salesTableNoData) Then
                ' Logger.Debug("InitStaffDetailDialogTableArea_001 Call_Start salesTableNoData is NullOrEmpty")
                Exit For
            End If

            ' 行の区切り
            If ((i + 1) Mod 4) = 1 Then
                CType(salesTable.FindControl("SalesTableAreaLeft"), Literal).Visible = True
            ElseIf ((i + 1) Mod 4) = 0 Then
                CType(salesTable.FindControl("SalesTableAreaRight"), Literal).Visible = True
            Else
                CType(salesTable.FindControl("SalesTableAreaCenter"), Literal).Visible = True
            End If

            If UsedFlagUsed.Equals(shiyoFlgData) Then
                CType(salesTable.FindControl("SalesTableNoSelected"), Literal).Visible = True
            Else
                CType(salesTable.FindControl("SalesTableNoOff"), Literal).Visible = True
            End If

            CType(salesTable.FindControl("SalesTableNoLiteral"), Literal).Text = _
                ChangeString(salesTableNoData, CustomerDialogSalesTableNoSize, StringCut)

        Next
        ' Logger.Debug("InitStaffDetailDialogTableArea_End Ret[]")
    End Sub
#End Region

#Region "商談中詳細表示設定(プロセス)"

    ''' <summary>
    ''' 商談中詳細表示設定(プロセス)
    ''' </summary>
    ''' <param name="customerRow">表示する顧客情報</param>
    ''' <param name="context">スタッフコンテキスト</param>
    ''' <remarks></remarks>
    Private Sub InitStaffDetailDialogProcessArea(ByVal customerRow As VisitReceptionVisitorCustomerRow, _
                                                   ByVal context As StaffContext)
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

                Me.CarName.Text = If(selectedSeriesDataRow.IsSERIESNMNull() Or String.IsNullOrEmpty(selectedSeriesDataRow.SERIESNM), _
                                     DataNull, Server.HtmlEncode(selectedSeriesDataRow.SERIESNM))
                Me.GradeName.Text = If(selectedSeriesDataRow.IsVCLMODEL_NAMENull() Or _
                                       String.IsNullOrEmpty(selectedSeriesDataRow.VCLMODEL_NAME), DataNull, Server.HtmlEncode(selectedSeriesDataRow.VCLMODEL_NAME))

                If Not selectedSeriesDataRow.IsSEQNONull() Then
                    seqNo = selectedSeriesDataRow.SEQNO
                End If

            Else
                ' Logger.Debug("InitStaffDetailDialogProcessArea_002 selectedSeriesDataTable.Count <= 0")
                Me.CarName.Text = DataNull
                Me.GradeName.Text = DataNull
            End If

        End Using

        ' $06 start 受注後フォロー機能開発
        ' お客様のプロセスデータ取得
        Me.ProcessList.Attributes("style") = Nothing
        If String.Equals(receptionResult, "0") Then

            ' 受注前プロセス取得
            Using processDataTable As ActivityInfoDataSet.ActivityInfoGetProcessToDataTable = _
               GetProcess(context, customerRow.FLLOWUPBOX_SEQNO, contractNo)

                If processDataTable.Count <= 0 Then

                    ' プロセス情報設定(プロセス情報なし)
                    SetProcessDefaultWord(receptionResult)
                Else
                    Dim processDataRow As ActivityInfoDataSet.ActivityInfoGetProcessToRow = Nothing

                    ' 希望車種に紐づくプロセスを取得する
                    For Each row In processDataTable

                        If Not row.IsSEQNONull() AndAlso seqNo = row.SEQNO Then
                            processDataRow = row
                            Exit For
                        End If

                    Next

                    If processDataRow Is Nothing Then

                        ' プロセス情報設定(プロセス情報なし)
                        SetProcessDefaultWord(receptionResult)
                    Else

                        ' プロセス情報設定(受注前)
                        SetProcessBeforeWord(processDataRow)
                    End If

                End If
            End Using

            ' お客様のステイタス取得
            Using statusDataTable As ActivityInfoDataSet.ActivityInfoGetStatusToDataTable = _
                GetStatus(context, customerRow.FLLOWUPBOX_SEQNO)

                '予めアイコンなしの情報を格納
                StaffDetailStatus.Attributes("class") = "IcnNoStatus"

                '情報が取得できた場合処理
                If statusDataTable.Count > 0 Then
                    ' Logger.Debug("InitStaffDetailDialogProcessArea_007 statusDataTable.Count > 0")
                    Dim statusDataRow As ActivityInfoDataSet.ActivityInfoGetStatusToRow = _
                       statusDataTable.Item(0)

                    Select Case statusDataRow.CRACTRESULT
                        Case StatusHot
                            ' Logger.Debug("InitStaffDetailDialogProcessArea_008 CRACTRESULT = " & StatusHot)
                            StaffDetailStatus.Attributes("class") = "IcnHot"
                        Case StatusWarm
                            ' Logger.Debug("InitStaffDetailDialogProcessArea_009 CRACTRESULT = " & StatusWarm)
                            StaffDetailStatus.Attributes("class") = "IcnWarm"
                        Case StatusSuccess
                            ' Logger.Debug("InitStaffDetailDialogProcessArea_010 CRACTRESULT = " & StatusSuccess)
                            StaffDetailStatus.Attributes("class") = "IcnSuccess"
                        Case StatusGiveUp
                            ' Logger.Debug("InitStaffDetailDialogProcessArea_011 CRACTRESULT = " & StatusGiveUp)
                            StaffDetailStatus.Attributes("class") = "IcnGiveUp"
                        Case Else
                            ' Logger.Debug("InitStaffDetailDialogProcessArea_012 CRACTRESULT = " & StatusCold)
                            StaffDetailStatus.Attributes("class") = "IcnCold"
                    End Select
                End If
            End Using

        Else

            ' 受注後工程プロセスマスタの取得
            Using bookedAfterProcessMasterDataTable As ActivityInfoDataSet.ActivityInfoBookedAfterProcessMasterDataTable = _
                ActivityInfoBusinessLogic.GetBookedAfterProcessMaster()

                ' お客様のステイタス非表示
                StaffDetailStatus.Attributes("class") = Nothing

                ' 固定出力のプロセス非表示
                SetProcessDefaultWord(receptionResult)

                If bookedAfterProcessMasterDataTable.Count > 0 Then

                    ' プロセス数に応じて表示領域を設定
                    Dim processListHeight As Integer = _
                        ((Math.Floor((bookedAfterProcessMasterDataTable.Count - 1) / AfterOdrProcsViewNum) + 1 ) * AfterOdrProcsViewHeight) + AfterOdrProcsViewHeightMargine
                    Me.ProcessList.Attributes("style") = String.Format("height:{0}px",processListHeight)

                    ' 受注後工程プロセス実績の取得
                    Dim bookedAfterProcessResultDataTable As ActivityInfoDataSet.ActivityInfoBookedAfterProcessResultDataTable = _
                        ActivityInfoBusinessLogic.GetBookedAfterProcessResult(customerRow.FLLOWUPBOX_SEQNO)

                    ' 受注後プロセス情報設定
                    SetProcessAfterWord(bookedAfterProcessMasterDataTable, bookedAfterProcessResultDataTable, context)

                End If
            End Using

        End If

        ' $06 end 受注後フォロー機能開発
    End Sub
#End Region

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

            StaffDetailProcess1.InnerText = Server.HtmlEncode(wordDictionary(45))
            StaffDetailProcess1.Attributes("class") = "Icn1Off"
            StaffDetailProcess2.InnerText = Server.HtmlEncode(wordDictionary(46))
            StaffDetailProcess2.Attributes("class") = "Icn2Off"
            StaffDetailProcess3.InnerText = Server.HtmlEncode(wordDictionary(47))
            StaffDetailProcess3.Attributes("class") = "Icn3Off"
            StaffDetailProcess4.InnerText = Server.HtmlEncode(wordDictionary(48))
            StaffDetailProcess4.Attributes("class") = "Icn4Off"
        Else
            ' Logger.Debug("SetProcessDefaultWord_003 receptionResult <> 0")
            ' $06 start 受注後フォロー機能開発
            StaffDetailProcess1.InnerText = String.Empty
            StaffDetailProcess1.Attributes("class") = Nothing
            StaffDetailProcess2.InnerText = String.Empty
            StaffDetailProcess2.Attributes("class") = Nothing
            StaffDetailProcess3.InnerText = String.Empty
            StaffDetailProcess3.Attributes("class") = Nothing
            StaffDetailProcess4.InnerText = String.Empty
            StaffDetailProcess4.Attributes("class") = Nothing
            ' $06 end 受注後フォロー機能開発
        End If

        ' Logger.Debug("SetProcessDefaultWord_End")
    End Sub

    ''' <summary>
    ''' プロセス情報設定
    ''' </summary>
    ''' <param name="processDataRow">プロセス情報</param>
    ''' <remarks>受注前の状態のプロセスを表示</remarks>
    Private Sub SetProcessBeforeWord(ByVal processDataRow As ActivityInfoDataSet.ActivityInfoGetProcessToRow)
        ' Logger.Debug("SetProcessBeforeWord_Start " & _
        '           "Param[" & receptionResult & "," & processDataRow.ToString & "]")

        Logger.Info("SetProcessBeforeWord_001 " & "Call_Start MyBase.GetValue Param[" & _
           ScreenPos.Current & "," & SessionKeyWordDictionary & "," & False & "]")
        Dim wordDictionary As Dictionary(Of Decimal, String) = _
         CType(MyBase.GetValue(ScreenPos.Current, SessionKeyWordDictionary, False), Dictionary(Of Decimal, String))
        Logger.Info("SetProcessBeforeWord_001 " & "Call_End MyBase.GetValue Ret[" & wordDictionary.ToString() & "]")

        ' CATALOGDATE:カタログ実施日
        StaffDetailProcess1.InnerText = If(processDataRow.IsCATALOGDATENull(), Server.HtmlEncode(wordDictionary(45)), processDataRow.CATALOGDATE)
        StaffDetailProcess1.Attributes("class") = If(processDataRow.IsCATALOGDATENull(), "Icn1Off", "Icn1On")

        ' TESTDRIVEDATE:試乗実施日
        StaffDetailProcess2.InnerText = If(processDataRow.IsTESTDRIVEDATENull(), Server.HtmlEncode(wordDictionary(46)), processDataRow.TESTDRIVEDATE)
        StaffDetailProcess2.Attributes("class") = If(processDataRow.IsTESTDRIVEDATENull(), "Icn2Off", "Icn2On")

        ' EVALUATIONDATE:査定実施日
        StaffDetailProcess3.InnerText = If(processDataRow.IsEVALUATIONDATENull(), Server.HtmlEncode(wordDictionary(47)), processDataRow.EVALUATIONDATE)
        StaffDetailProcess3.Attributes("class") = If(processDataRow.IsEVALUATIONDATENull(), "Icn3Off", "Icn3On")

        ' QUOTATIONDATE:見積実施日
        StaffDetailProcess4.InnerText = If(processDataRow.IsQUOTATIONDATENull(), Server.HtmlEncode(wordDictionary(48)), processDataRow.QUOTATIONDATE)
        StaffDetailProcess4.Attributes("class") = If(processDataRow.IsQUOTATIONDATENull(), "Icn4Off", "Icn4On")

        ' Logger.Debug("SetProcessBeforeWord_End")
    End Sub

    ' $06 start 受注後フォロー機能開発

    ''' <summary>
    ''' プロセス情報設定
    ''' </summary>
    ''' <param name="bookedAfterProcessMasterDataTable">受注後プロセスマスタ情報</param>
    ''' <param name="bookedAfterProcessResultDataTable">受注後プロセス実績情報</param>
    ''' <param name="context">スタッフコンテキスト</param>
    ''' <remarks></remarks>
    Private Sub SetProcessAfterWord(ByVal bookedAfterProcessMasterDataTable As ActivityInfoDataSet.ActivityInfoBookedAfterProcessMasterDataTable, _
                                    ByVal bookedAfterProcessResultDataTable As ActivityInfoDataSet.ActivityInfoBookedAfterProcessResultDataTable, _
                                    ByVal context As StaffContext)
        'Logger.Debug("SetProcessAfterWord_Start " & _
        '    "Param[" & bookedAfterProcessMasterDataTable.ToString & "," & bookedAfterProcessResultDataTable.ToString & "," & context.ToString & "]")

        Me.BookedAfterProcessRepeater.Visible = True
	    Me.BookedAfterProcessRepeater.DataSource = bookedAfterProcessMasterDataTable
	    Me.BookedAfterProcessRepeater.DataBind()

        Dim businessLogic As New SC3100101BusinessLogic
        Dim maxLength As Integer = BookedAfterProcessRepeater.Items.Count - 1

        '取得データの格納
        For i = 0 To maxLength

            ' アイコンパス取得
            Dim iconInfoRow As SC3100101AfterOrderProcessIconInfoRow() = _
                CType(businessLogic.GetAfterOrderProcessIcon(context.DlrCD).Select("AFTER_ODR_PRCS_CD = '" & bookedAfterProcessMasterDataTable(i).AFTER_ODR_PRCS_CD & "'"),  _
                    SC3100101AfterOrderProcessIconInfoRow())

            Dim iconInfoRowX As SC3100101AfterOrderProcessIconInfoRow() = _
                CType(businessLogic.GetAfterOrderProcessIcon(DealerCdX).Select("AFTER_ODR_PRCS_CD = '" & bookedAfterProcessMasterDataTable(i).AFTER_ODR_PRCS_CD & "'"),  _
                    SC3100101AfterOrderProcessIconInfoRow())

            ' 表示位置の設定
            Dim posX As Integer = ((i mod AfterOdrProcsViewNum) * AfterOdrProcsViewWidth) + AfterOdrProcsInitPosX
            Dim posY As Integer = ( Math.Floor(i / AfterOdrProcsViewNum) * AfterOdrProcsViewHeight) + AfterOdrProcsInitPosY

            ' 受注後プロセスマスタ情報に対応する実績情報を取得
            Dim resultRow As ActivityInfoDataSet.ActivityInfoBookedAfterProcessResultRow() = _
                CType(bookedAfterProcessResultDataTable.Select("AFTER_ODR_PRCS_CD = '" & bookedAfterProcessMasterDataTable(i).AFTER_ODR_PRCS_CD & "'"),  _
                    ActivityInfoDataSet.ActivityInfoBookedAfterProcessResultRow())

            ' プロセス名
            Dim processName As String = String.Empty
            If resultRow.Length <= 0 OrElse resultRow(0).IsRSLT_DATENull() Then
                processName = Server.HtmlEncode(bookedAfterProcessMasterDataTable(i).AFTER_ODR_PRCS_NAME)
            Else
                processName = DateTimeFunc.FormatElapsedDate(ElapsedDateFormat.Normal, resultRow(0).RSLT_DATE, DateTimeFunc.Now, context.DlrCD, False)
            End If

            ' プロセス情報設定
            Dim item As Control = BookedAfterProcessRepeater.Items(i)
            CType(item.FindControl("StaffDetailProcess"), HtmlGenericControl).InnerText = processName
            CType(item.FindControl("StaffDetailProcess"), HtmlGenericControl).Attributes("style") = _
                GetBookedAfterProcessIconStyle(resultRow, iconInfoRow, iconInfoRowX, posY, posX)
        Next

        'Logger.Debug("SetProcessAfterWord_End")

    End Sub

    ''' <summary>
    ''' 受注後プロセスアイコンスタイル取得
    ''' </summary>
    ''' <param name="bookedAfterProcessResult">受注後プロセス実績情報</param>
    ''' <param name="iconInfoRow">アイコン情報(販売店コード)</param>
    ''' <param name="iconInfoRowX">アイコン情報(販売店コードXXXXX)</param>
    ''' <param name="posY">表示位置Y</param>
    ''' <param name="posX">表示位置X</param>
    ''' <returns>受注後プロセスアイコンスタイル</returns>
    Private Function GetBookedAfterProcessIconStyle(bookedAfterProcessResult As ActivityInfoDataSet.ActivityInfoBookedAfterProcessResultRow(), _
                                                    iconInfoRow As SC3100101AfterOrderProcessIconInfoRow(), _
                                                    iconInfoRowX As SC3100101AfterOrderProcessIconInfoRow(), _
                                                    posY As Integer, posX As Integer) As String

        ' アイコンパス
        Dim iconPath As String = GetBookedAfterProcessIconPath(bookedAfterProcessResult, iconInfoRow)
        Dim iconPathX As String = GetBookedAfterProcessIconPath(bookedAfterProcessResult, iconInfoRowX)

        If String.IsNullOrEmpty(iconPath) then

            ' アイコン情報(販売店コード)が取得できない場合はアイコン情報(販売店コードXXXXX)に置き換える
            iconPath = iconPathX
        End If

        ' アイコンスタイルを返す
        Return String.Format(AfterOdrProcsIconStyle, iconPath, posY, posX, AfterOdrProcsViewWidth)
    End Function

    ''' <summary>
    ''' 受注後プロセスアイコンパス取得
    ''' </summary>
    ''' <param name="bookedAfterProcessResult">受注後プロセス実績情報</param>
    ''' <param name="iconInfoRow">アイコン情報</param>
    ''' <returns>受注後プロセスアイコンパス</returns>
    Private Function GetBookedAfterProcessIconPath(bookedAfterProcessResult As ActivityInfoDataSet.ActivityInfoBookedAfterProcessResultRow(), _
                                                   iconInfoRow As SC3100101AfterOrderProcessIconInfoRow()) As String
        Dim iconPath As String = Nothing

        If iconInfoRow.Length <= 0 then

            ' レコードが存在しない場合
            iconPath = Nothing
        Else
            If bookedAfterProcessResult.Length <= 0 OrElse
               "0".Equals(bookedAfterProcessResult(0).CHECKFLG) Then

                ' 作業なしアイコン
                iconPath = iconInfoRow(0).ICON_PATH_NOT
            ElseIf bookedAfterProcessResult(0).IsRSLT_DATENull() Then

                ' 未完了アイコン
                iconPath = iconInfoRow(0).ICON_PATH_OFF
            Else

                ' 完了アイコン
                iconPath = iconInfoRow(0).ICON_PATH_ON
            End If

        End If

        If String.IsNullOrEmpty(iconPath) then
            Return String.Empty
        Else
            Return iconPath
        End If
    End Function

    ' $06 end 受注後フォロー機能開発

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

#Region " 顧客詳細遷移"

    ''' <summary>
    ''' 顧客詳細画面に遷移
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub StaffDetailCustomerNameButton_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles StaffDetailCustomerNameButton.Click
        Logger.Info("StaffDetailCustomerNameButton_Click_Start " & _
                    "Param[" & sender.ToString & "," & e.ToString & "]")

        ' 顧客詳細画面(SC3080201)に遷移
        ' NEXTセッション領域に情報を設定
        ' お客様区分(顧客区分)
        Logger.Info("StaffDetailCustomerNameButton_Click_002 " & "Call_Start Me.SetValue Param[ScreenPos.Next, " & SessionKeyCustomerKind & ", " & StaffDetailDialogCustomerSegment.Value & "]")
        Me.SetValue(ScreenPos.Next, SessionKeyCustomerKind, StaffDetailDialogCustomerSegment.Value)

        ' お客様分類(顧客分類)
        Logger.Info("StaffDetailCustomerNameButton_Click_003 " & "Call_Start Me.SetValue Param[ScreenPos.Next, " & SessionKeyCustomerClass & ", " & CustomerClassOwner & "]")
        Me.SetValue(ScreenPos.Next, SessionKeyCustomerClass, CustomerClassOwner)

        ' お客様ID(顧客コード)
        Logger.Info("StaffDetailCustomerNameButton_Click_004 " & "Call_Start Me.SetValue Param[ScreenPos.Next, " & SessionKeyCustomerId & ", " & StaffDetailDialogCustId.Value & "]")
        Me.SetValue(ScreenPos.Next, SessionKeyCustomerId, StaffDetailDialogCustId.Value)

        ' 遷移処理(親フレームに遷移する)
        Logger.Info("StaffDetailCustomerNameButton_Click_005 " & "Call_Start Me.RedirectNextScreen Param[SC3080201]")
        Me.RedirectNextScreen("SC3080201")
        Logger.Info("StaffDetailCustomerNameButton_Click_005 " & "Call_End Me.RedirectNextScreen")



        Logger.Info("StaffDetailCustomerNameButton_End Ret[] ")
    End Sub

#End Region

    ''' <summary>
    ''' 経過時間のリスト作成
    ''' </summary>
    ''' <param name="dataTable">データテーブル</param>
    ''' <param name="columnName">カラム名</param>
    ''' <returns>経過時間のリスト</returns>
    ''' <remarks></remarks>
    Private Function GetTimeSpanListString(ByVal dataTable As DataTable, _
                                           ByVal columnName As String, ByVal nowDate As Date) As String
        ' $02 start 新車タブレットショールーム管理機能開発
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

        Dim javaScript As New JavaScriptSerializer
        Return javaScript.Serialize(timeSpanList)
        ' $02 end   新車タブレットショールーム管理機能開発
    End Function
#End Region

End Class

