'-------------------------------------------------------------------------
'Consts.vb
'-------------------------------------------------------------------------
'機能：タブレットSMB共通関数
'補足：定数
'作成：2013/08/14 TMEJ 張 タブレット版SMB機能開発(工程管理)
'更新：2013/11/29 TMEJ 丁 タブレット版SMB チーフテクニシャン機能開発
'更新：2013/12/02 TMEJ 張 タブレット版SMB チーフテクニシャン機能開発
'更新：2014/07/15 TMEJ 張 タブレットSMB Job Dispatch機能開発
'更新：2014/09/25 TMEJ 張 BTS-180 「洗車中に関連チップ作成すると予期せぬエラーメッセージ」対応
'更新：2014/12/08 TMEJ 丁 DMS連携版サービスタブレット JobDispatch完成検査入力制御開発
'更新：2014/12/17 TMEJ 小澤 IT9824_NextSTEPサービス 洗車用端末開発向けた評価用アプリ作成
'更新：2015/02/09 TMEJ 範 DMS連携版サービスタブレット SMB納車予定時刻通知機能開発
'更新：2015/04/07 TMEJ 小澤 BTS-XXX JOB_IDのシーケンス設定を修正
'更新：2015/04/23 TMEJ 明瀬  DMS連携版サービスタブレット強制納車機能追加開発
'更新：2015/10/08 TM 皆川 タブレットSMBのチップ移動時の連携送信メッセージ詳細化
'更新：2016/01/11 NSK 浅野 (トライ店システム評価)アカウント・組織・文言マスタの仕様変更対応
'更新：2017/09/27 NSK 竹中(悠) REQ-SVT-TMT-20170227-002 見込納車遅れ日時計算ロジック統一
'更新：2019/06/18 NSK 皆川 (トライ店システム評価)次世代サービスオペレーション効率化に向けた、業務適合性検証
'更新：2019/12/06 NSK 皆川 (トライ店システム評価)整備作業における休憩時間取得判定向上検証
'更新：
'─────────────────────────────────────

Imports System.Xml
Imports System.Net
Imports System.Web
Imports System.IO
Imports System.Globalization
Imports System.Reflection
Imports System.Xml.Serialization
Imports System.Text.RegularExpressions
Imports Toyota.eCRB.SystemFrameworks.Web
Imports Toyota.eCRB.SystemFrameworks.Core
Imports Toyota.eCRB.CommonUtility.TabletSMBCommonClass.Api.DataAccess.TabletSMBCommonClassDataSet
Imports Toyota.eCRB.CommonUtility.TabletSMBCommonClass.Api.DataAccess.TabletSMBCommonClassDataSetTableAdapters
Imports Toyota.eCRB.CommonUtility.SMBCommonClass.Api.BizLogic
Imports System.Text

Partial Class TabletSMBCommonClassBusinessLogic

    '2013/12/02 TMEJ 張 タブレット版SMB チーフテクニシャン機能開発 START
#Region "メンバー変数"
    ''' <summary>部品ステータス情報取得結果コード</summary>
    Public Property IC3802503ResultValue As Long
    ''' <summary>削除されたチップに作業指示情報</summary>
    Public Property TabletSmbCommonCancelInstructedChipInfo As TabletSmbCommonClassCanceledJobInfoDataTable

    '2014/07/15 TMEJ 張 タブレットSMB Job Dispatch機能開発 START
    ''' <summary>開始後、Push送信やる必要フラグ (True:Push送信 False:Push送信しない)</summary>
    Public Property NeedPushAfterStartSingleJob As Boolean = False
    ''' <summary>終了後、Push送信やる必要フラグ (True:Push送信 False:Push送信しない)</summary>
    Public Property NeedPushAfterFinishSingleJob As Boolean = False
    ''' <summary>中断後、Push送信やる必要フラグ (True:Push送信 False:Push送信しない)</summary>
    Public Property NeedPushAfterStopSingleJob As Boolean = False
    '2014/07/15 TMEJ 張 タブレットSMB Job Dispatch機能開発 END

    '2016/01/11 NSK 浅野 (トライ店システム評価)アカウント・組織・文言マスタの仕様変更対応 START
    ''' <summary>サブエリア更新Pushフラグ (True:Push送信 False:Push送信しない)</summary>
    Public Property NeedPushSubAreaRefresh As Boolean = False
    '2016/01/11 NSK 浅野 (トライ店システム評価)アカウント・組織・文言マスタの仕様変更対応 END

    ''' <summary>開始操作ストールロックするかどうかフラグ</summary>
    Private Property isStallLocked As Boolean

#End Region
    '2013/12/02 TMEJ 張 タブレット版SMB チーフテクニシャン機能開発 END

#Region "定数"

#Region "サービスステータス"
    ''' <summary>
    ''' 未入庫
    ''' </summary>
    ''' <remarks></remarks>
    Private Const SvcStatusNotCarin As String = "00"
    ''' <summary>
    ''' 未来店客（予定通りの入庫がないため一旦チップをNoShowエリアに移動している）
    ''' </summary>
    ''' <remarks></remarks>
    Private Const SvcStatusNoShow As String = "01"
    ''' <summary>
    ''' キャンセル
    ''' </summary>
    ''' <remarks></remarks>
    Private Const SvcStatusCanel As String = "02"
    ''' <summary>
    ''' 着工指示待ち
    ''' </summary>
    ''' <remarks></remarks>
    Private Const SvcStatusWorkOrderWait As String = "03"
    ''' <summary>
    ''' 作業開始待ち
    ''' </summary>
    ''' <remarks></remarks>
    Private Const SvcStatusStartwait As String = "04"
    ''' <summary>
    ''' 作業中
    ''' </summary>
    ''' <remarks></remarks>
    Private Const SvcStatusStart As String = "05"
    ''' <summary>
    ''' 次の作業開始待ち
    ''' </summary>
    ''' <remarks></remarks>
    Private Const SvcStatusNextStartWait As String = "06"
    ''' <summary>
    ''' 洗車待ち
    ''' </summary>
    ''' <remarks></remarks>
    Private Const SvcStatusCarWashWait As String = "07"
    ''' <summary>
    ''' 洗車中
    ''' </summary>
    ''' <remarks></remarks>
    Private Const SvcStatusCarWashStart As String = "08"
    ''' <summary>
    ''' 検査待ち
    ''' </summary>
    ''' <remarks></remarks>
    Private Const SvcStatusInspectionWait As String = "09"
    ''' <summary>
    ''' 検査中
    ''' </summary>
    ''' <remarks></remarks>
    Private Const SvcStatusInspectionStart As String = "10"
    ''' <summary>
    ''' 預かり中（DropOff）
    ''' </summary>
    ''' <remarks></remarks>
    Private Const SvcStatusDropOffCustomer As String = "11"
    ''' <summary>
    ''' 納車待ち（Waiting）
    ''' </summary>
    ''' <remarks></remarks>
    Private Const SvcStatusWaitingCustomer As String = "12"
    ''' <summary>
    ''' 納車済み
    ''' </summary>
    ''' <remarks></remarks>
    Private Const SvcStatusDelivery As String = "13"
#End Region

#Region "予約ステータス"
    ''' <summary>
    ''' 仮予約
    ''' </summary>
    ''' <remarks></remarks>
    Private Const ResvStatusTentative As String = "0"

    ''' <summary>
    ''' 本予約
    ''' </summary>
    ''' <remarks></remarks>
    Private Const ResvStatusConfirmed As String = "1"
#End Region

#Region "ストール利用ステータス"
    ''' <summary>
    ''' 着工指示待ち
    ''' </summary>
    ''' <remarks></remarks>
    Private Const StalluseStatusWorkOrderWait As String = "00"
    ''' <summary>
    ''' 作業開始待ち
    ''' </summary>
    ''' <remarks></remarks>
    Private Const StalluseStatusStartWait As String = "01"
    ''' <summary>
    ''' 作業中
    ''' </summary>
    ''' <remarks></remarks>
    Private Const StalluseStatusStart As String = "02"
    ''' <summary>
    ''' 完了
    ''' </summary>
    ''' <remarks></remarks>
    Private Const StalluseStatusFinish As String = "03"
    ''' <summary>
    ''' 作業計画の一部の作業が中断
    ''' </summary>
    ''' <remarks></remarks>
    Private Const StalluseStatusStartIncludeStopJob As String = "04"
    ''' <summary>
    ''' 中断
    ''' </summary>
    ''' <remarks></remarks>
    Private Const StalluseStatusStop As String = "05"
    ''' <summary>
    ''' 日跨ぎ終了
    ''' </summary>
    ''' <remarks></remarks>
    Private Const StalluseStatusMidfinish As String = "06"
    ''' <summary>
    ''' 未来店客（予定通りの入庫がないため一旦チップをNoShowエリアに移動している）
    ''' </summary>
    ''' <remarks></remarks>
    Private Const StalluseStatusNoshow As String = "07"
#End Region

#Region "遷移先区分"
    ''' <summary>
    ''' 検査待ち
    ''' </summary>
    ''' <remarks></remarks>
    Private Const TransitionStatusInspectionWait As String = "0"
    ''' <summary>
    ''' 洗車待ち
    ''' </summary>
    ''' <remarks></remarks>
    Private Const TransitionStatusCarWashWait As String = "1"
    ''' <summary>
    ''' 納車待ち
    ''' </summary>
    ''' <remarks></remarks>
    Private Const TransitionStatusDeliveryWait As String = "2"
    ''' <summary>
    ''' 次の作業開始待ち
    ''' </summary>
    ''' <remarks></remarks>
    Private Const TransitionStatusNextWorkWait As String = "3"
    ''' <summary>
    ''' 納車
    ''' </summary>
    ''' <remarks></remarks>
    Private Const TransitionStatusDelivery As String = "4"
    ''' <summary>
    ''' 自動判別
    ''' </summary>
    ''' <remarks></remarks>
    Private Const TransitionStatusAutomatic As String = "9"
#End Region

#Region "洗車必要フラグ"
    ''' <summary>
    ''' 不要
    ''' </summary>
    ''' <remarks></remarks>
    Private Const CarWashNeedFlgNeedless As String = "0"
    ''' <summary>
    ''' 必要
    ''' </summary>
    ''' <remarks></remarks>
    Private Const CarWashNeedFlgNeed As String = "1"
#End Region

#Region "検査必要フラグ"
    ''' <summary>
    ''' 不要
    ''' </summary>
    ''' <remarks></remarks>
    Private Const InspectionNeedFlgNeedless As String = "0"
    ''' <summary>
    ''' 必要
    ''' </summary>
    ''' <remarks></remarks>
    Private Const InspectionNeedFlgNeed As String = "1"
#End Region

#Region "仮置きフラグ"
    ''' <summary>
    ''' 仮置きではない
    ''' </summary>
    ''' <remarks></remarks>
    Private Const TempFlgNotTemp As String = "0"
    ''' <summary>
    ''' 仮置きではない
    ''' </summary>
    ''' <remarks></remarks>
    Private Const TempFlgTemp As String = "1"
#End Region

#Region "キャンセルフラグ"
    ''' <summary>
    ''' 有効
    ''' </summary>
    ''' <remarks></remarks>
    Private Const CancelFlgUsable As String = "0"
    ''' <summary>
    ''' キャンセル
    ''' </summary>
    ''' <remarks></remarks>
    Private Const CancelFlgCancel As String = "1"
#End Region

#Region "休憩取得フラグ"
    '2019/06/18 NSK 皆川 (トライ店システム評価)次世代サービスオペレーション効率化に向けた、業務適合性検証 START
    ' ''' <summary>
    ' ''' 取得しない（取得しなかった）
    ' ''' </summary>
    ' ''' <remarks></remarks>
    'Private Const RestTimeGetFlgNoGetRest As String = "0"
    ' ''' <summary>
    ' ''' 取得する（取得した）
    ' ''' </summary>
    ' ''' <remarks></remarks>
    'Private Const RestTimeGetFlgGetRest As String = "1"

    ''' <summary>
    ''' 取得しない（取得しなかった）
    ''' </summary>
    ''' <remarks></remarks>
    Public Const RestTimeGetFlgNoGetRest As String = "0"
    ''' <summary>
    ''' 取得する（取得した）
    ''' </summary>
    ''' <remarks></remarks>
    Public Const RestTimeGetFlgGetRest As String = "1"
    '2019/06/18 NSK 皆川 (トライ店システム評価)次世代サービスオペレーション効率化に向けた、業務適合性検証 END
#End Region

#Region "引取納車区分"

    ''' <summary>
    ''' 引取納車区分:Waiting
    ''' </summary>
    ''' <remarks></remarks>
    Private Const DeliTypeWaiting As String = "0"

    ''' <summary>
    ''' 引取納車区分:Drop off
    ''' </summary>
    ''' <remarks></remarks>
    Private Const DeliTypeDropOff As String = "4"
#End Region

#Region "作業区分"
    ''' <summary>
    ''' ストール作業
    ''' </summary>
    ''' <remarks></remarks>
    Private Const JobTypeStallWork As String = "0"
#End Region

#Region "表示区分"
    ''' <summary>
    ''' 納車準備
    ''' </summary>
    ''' <remarks></remarks>
    Private Const DispTypeDeliveryPreparation As Long = 3
    ''' <summary>
    ''' 納車作業
    ''' </summary>
    ''' <remarks></remarks>
    Private Const DispTypeDeliveryWork As Long = 4
    ''' <summary>
    ''' 作業中
    ''' </summary>
    ''' <remarks></remarks>
    Private Const DispTypeWork As Long = 5
#End Region

#Region "追加作業ステータス"
    ''' <summary>
    ''' 追加作業起票中(白いプラスマーク)
    ''' </summary>
    ''' <remarks></remarks>
    Private Const AddWorkAddingWork As String = "1"
    ''' <summary>
    ''' 追加作業承認待ち(黄色プラスマーク)
    ''' </summary>
    ''' <remarks></remarks>
    Private Const AddWorkConfirmWait As String = "2"
    '2013/12/02 TMEJ 張 タブレット版SMB チーフテクニシャン機能開発 START
    ''' <summary>
    ''' 追加作業承がない(マークがない)
    ''' </summary>
    ''' <remarks></remarks>
    Private Const AddWorkNoMark As String = "0"
    ' ''' <summary>
    ' ''' 追加作業承認待ち
    ' ''' </summary>
    ' ''' <remarks></remarks>
    'Private Const AddWorkConfirmWait2 As String = "3"
    ' ''' <summary>
    ' ''' 追加作業承認待ち
    ' ''' </summary>
    ' ''' <remarks></remarks>
    'Private Const AddWorkConfirmWait3 As String = "4"
    '2013/12/02 TMEJ 張 タブレット版SMB チーフテクニシャン機能開発 END
#End Region

    '2013/12/02 TMEJ 張 タブレット版SMB チーフテクニシャン機能開発 START
#Region "ROステータス"
    ''' <summary>
    ''' ROがない
    ''' </summary>
    ''' <remarks></remarks>
    Private Const RoStatusNoRo As String = "00"
    ''' <summary>
    ''' TC承認待ち
    ''' </summary>
    ''' <remarks></remarks>
    Private Const RoStatusTcIssuing As String = "15"
    ''' <summary>
    ''' FM承認待ち
    ''' </summary>
    ''' <remarks></remarks>
    Private Const RoStatusWaitingForFmApproval As String = "20"
    ''' <summary>
    ''' 25：Creating Parts rough quotation
    ''' </summary>
    ''' <remarks></remarks>
    Private Const RoStatusCreatingPartsRoughQuotation As String = "25"
    ''' <summary>
    ''' 30：Creating Parts quotation
    ''' </summary>
    ''' <remarks></remarks>
    Private Const RoStatusCreatingPartsQuotation As String = "30"
    ''' <summary>
    ''' 35：Waiting for R/O Confirmation
    ''' </summary>
    ''' <remarks></remarks>
    Private Const RoStatusCreatingWaitingForRoConfirmation As String = "35"
    ''' <summary>
    ''' 40：Waiting for Customer Approval
    ''' </summary>
    ''' <remarks></remarks>
    Private Const RoStatusCreatingWaitingForCustomerApproval As String = "40"
    ''' <summary>
    ''' 作業開始待ち
    ''' </summary>
    ''' <remarks></remarks>
    Private Const RostatusApprovedbyCustomer As String = "50"
    ''' <summary>
    ''' 作業中
    ''' </summary>
    ''' <remarks></remarks>
    Private Const RostatusWorkinProgress As String = "60"
    ''' <summary>
    ''' 納車待ち
    ''' </summary>
    ''' <remarks></remarks>
    Private Const RostatusWaitForDelivery As String = "80"
    ''' <summary>
    ''' ClosingJob
    ''' </summary>
    ''' <remarks></remarks>
    Private Const RostatusClosingJob As String = "85"
    ''' <summary>
    ''' 納車済
    ''' </summary>
    ''' <remarks></remarks>
    Private Const RostatusDelivered As String = "90"
#End Region

#Region "作業ステータス"
    '2014/07/15 TMEJ 張 タブレットSMB Job Dispatch機能開発 START
    ''' <summary>
    ''' 作業前
    ''' </summary>
    ''' <remarks></remarks>
    Public Const JobStatusBeforeStart As String = "3"
    '2014/07/15 TMEJ 張 タブレットSMB Job Dispatch機能開発 END
    ''' <summary>
    ''' 作業中
    ''' </summary>
    ''' <remarks></remarks>
    Public Const JobStatusWorking As String = "0"
    ''' <summary>
    ''' 完了
    ''' </summary>
    ''' <remarks></remarks>
    Public Const JobStatusFinish As String = "1"
    ''' <summary>
    ''' 中断
    ''' </summary>
    ''' <remarks></remarks>
    Public Const JobStatusStop As String = "2"

    ''' <summary>
    ''' 制御用作業ステータス：開始前
    ''' </summary>
    ''' <remarks></remarks>
    Private Const JobLinkStatusBeforeWork As String = "101"
    ''' <summary>
    ''' 制御用作業ステータス：作業中
    ''' </summary>
    ''' <remarks></remarks>
    Private Const JobLinkStatusWorking As String = "102"
    ''' <summary>
    ''' 制御用作業ステータス：完了
    ''' </summary>
    ''' <remarks></remarks>
    Private Const JobLinkStatusFinish As String = "103"
    ''' <summary>
    ''' 制御用作業ステータス：中断
    ''' </summary>
    ''' <remarks></remarks>
    Private Const JobLinkStatusStop As String = "104"
#End Region

#Region "部品出庫ステータス"

    ''' <summary>
    ''' 部品出庫ステータス:出庫済み
    ''' </summary>
    ''' <remarks></remarks>
    Private Const PartsStatusFinish As String = "8"

#End Region

#Region "サービス分類区分"

    ''' <summary>
    ''' EM
    ''' </summary>
    ''' <remarks></remarks>
    Private Const SvcClassTypeEM As String = "1"

    ''' <summary>
    ''' FM
    ''' </summary>
    ''' <remarks></remarks>
    Private Const SvcClassTypeFM As String = "2"

#End Region
    '2013/12/02 TMEJ 張 タブレット版SMB チーフテクニシャン機能開発 END

    '2014/07/15 TMEJ 張 タブレットSMB Job Dispatch機能開発 START
#Region "ROステータス"
    ''' <summary>
    ''' 中断Jobがない
    ''' </summary>
    ''' <remarks></remarks>
    Private Const StoppingJobNotExist As String = "0"
    ''' <summary>
    ''' 中断Jobがある
    ''' </summary>
    ''' <remarks></remarks>
    Private Const StoppingJobExist As String = "1"
#End Region

#Region "呼び出し元タイプ"
    ''' <summary>
    ''' 工程管理画面の全作業のアクション
    ''' </summary>
    ''' <remarks></remarks>
    Public Const CallerTypeSmbAllJobAction As Short = 1

    ''' <summary>
    ''' 詳細画面またはTC画面の全作業のアクション
    ''' </summary>
    ''' <remarks></remarks>
    Public Const CallerTypeDetailAllJobAction As Short = 2

    ''' <summary>
    ''' 工程管理画面の単一作業のアクション
    ''' </summary>
    ''' <remarks></remarks>
    Public Const CallerTypeDetailSingleJobAction As Short = 3
#End Region

#Region "作業終了後のチップステータス"

    ''' <summary>
    ''' 作業中
    ''' </summary>
    ''' <remarks></remarks>
    Private Const AfterFinishChipStatusWorking As Long = 1

    ''' <summary>
    ''' 中断
    ''' </summary>
    ''' <remarks></remarks>
    Private Const AfterFinishChipStatusStop As Long = 2

    ''' <summary>
    ''' 終了
    ''' </summary>
    ''' <remarks></remarks>
    Private Const AfterFinishChipStatusFinish As Long = 3

#End Region
    '2014/07/15 TMEJ 張 タブレットSMB Job Dispatch機能開発 END

#Region "既定値"
    ''' <summary>
    ''' DB数値型の既定値（0）
    ''' </summary>
    ''' <remarks></remarks>
    Private Const DefaultNumberValue As Long = 0
    ''' <summary>
    ''' DB文字列型の既定値（" ")
    ''' </summary>
    ''' <remarks></remarks>
    Private Const DefaultStringValue As String = " "
    ''' <summary>
    ''' 全販売店を意味するワイルドカード販売店コード
    ''' </summary>
    ''' <remarks></remarks>
    Private Const AllDealerCode As String = "XXXXX"
    ''' <summary>
    ''' 全店舗を意味するワイルドカード店舗コード
    ''' </summary>
    ''' <remarks></remarks>
    Private Const AllBranchCode As String = "XXX"

    '2015/02/09 TMEJ 範 DMS連携版サービスタブレット SMB納車予定時刻通知機能開発 START
    ''' <summary>
    ''' DB日付省略値
    ''' </summary>
    ''' <remarks></remarks>
    Private Const MinDate As String = "1900/01/01 00:00:00"
    '2015/02/09 TMEJ 範 DMS連携版サービスタブレット SMB納車予定時刻通知機能開発 END
#End Region

#Region "シーケンスタイプ"
    ''' <summary>
    ''' サービス入庫
    ''' </summary>
    ''' <remarks></remarks>
    Private Const SvcinIdSeq As String = "SQ_SVCIN_ID"
    ''' <summary>
    ''' 洗車実績
    ''' </summary>
    ''' <remarks></remarks>
    Private Const CarwashRsltIdSeq As String = "SQ_CARWASH_RSLT_ID"
    ''' <summary>
    ''' 作業内容
    ''' </summary>
    ''' <remarks></remarks>
    Private Const JobDetailIdSeq As String = "SQ_JOB_DTL_ID"
    ''' <summary>
    ''' ストール利用
    ''' </summary>
    ''' <remarks></remarks>
    Private Const StallUseIdSeq As String = "SQ_STALL_USE_ID"
    ''' <summary>
    ''' 非稼働テーブル
    ''' </summary>
    ''' <remarks></remarks>
    Private Const StallIdleIdSeq As String = "SQ_STALL_IDLE_ID"
    ''' <summary>
    ''' スタッフ作業
    ''' </summary>
    ''' <remarks></remarks>
    Private Const StfJobIdSeq As String = "SQ_STF_JOB_ID"
    '2013/12/02 TMEJ 張 タブレット版SMB チーフテクニシャン機能開発 START
    ''' <summary>
    ''' 作業実績
    ''' </summary>
    ''' <remarks></remarks>
    Private Const JobRsltSeq As String = "SQ_JOB_RSLT_ID"
    '2013/12/02 TMEJ 張 タブレット版SMB チーフテクニシャン機能開発 END
    '2015/04/07 TMEJ 小澤 BTS-XXX JOB_IDのシーケンス設定を修正 START
    ''' <summary>
    ''' 作業ID
    ''' </summary>
    ''' <remarks></remarks>
    Private Const JobIdSeq As String = "SQ_JOB_ID"
    '2015/04/07 TMEJ 小澤 BTS-XXX JOB_IDのシーケンス設定を修正 END
#End Region

#Region "XML送信関連"
#Region "NoShowフラグ"
    ''' <summary>
    ''' NoShowではない
    ''' </summary>
    ''' <remarks></remarks>
    Private Const NoShowFlgNotNoShow As String = "0"
    ''' <summary>
    ''' NoShow
    ''' </summary>
    ''' <remarks></remarks>
    Private Const NoShowFlgNoShow As String = "1"
#End Region

#Region "着工指示フラグ"
    ''' <summary>
    ''' 未着工
    ''' </summary>
    ''' <remarks></remarks>
    Private Const WorkOrderFlgOff As String = "0"
    ''' <summary>
    ''' 着工指示済
    ''' </summary>
    ''' <remarks></remarks>
    Private Const WorkOrderFlgOn As String = "1"
#End Region

#Region "NoChange項目"
    ''' <summary>
    ''' NoChange項目
    ''' </summary>
    ''' <remarks></remarks>
    Private Const NoChangeItem As String = "NoChangeItem"
#End Region


    ''' <summary>
    ''' エンコード(UTF-8)
    ''' </summary>
    ''' <remarks></remarks>
    Private Const EncodingUTF8 As String = "UTF-8"

    ''' <summary>
    ''' 送信方法(POST)
    ''' </summary>
    ''' <remarks></remarks>
    Private Const Post As String = "POST"

    ''' <summary>
    ''' ContentType
    ''' </summary>
    ''' <remarks></remarks>
    Private Const ContentTypeString As String = "application/x-www-form-urlencoded"

    ''' <summary>
    ''' ハイフン
    ''' </summary>
    ''' <remarks></remarks>
    Private Const Hyphen As String = "-"
#End Region

#Region "部品準備完了フラグ"
    ''' <summary>
    ''' 部品準備完了
    ''' </summary>
    ''' <remarks></remarks>
    Private Const PartsFlgOn As String = "1"
    ''' <summary>
    ''' 部品準備未完了
    ''' </summary>
    ''' <remarks></remarks>
    Private Const PartsFlgOff As String = "0"
#End Region

    ''' <summary>
    ''' 活動ID（未設定）
    ''' </summary>
    Private Const NoActivityId As Long = 0

#Region "Push送信関連"
    ''' <summary>
    ''' プッシュ送信で呼び出されるJSメソッド名
    ''' </summary>
    Private Const PUSH_FuntionNM As String = "CallPushEvent()"

    ''' <summary>
    ''' タブレットSMBリフレッシュ関数名
    ''' </summary>
    Private Const PUSH_FuntionTabletSMB As String = "RefreshSMB()"

    ''' <summary>
    ''' PSメインメニューリフレッシュ関数名
    ''' </summary>
    Private Const PUSH_FuntionPS As String = "MainRefresh()"

    ''' <summary>
    ''' 来店管理メインメニューリフレッシュ関数名
    ''' </summary>
    Private Const PUSH_FuntionSVR As String = "RefreshWindow()"

    ''' <summary>
    ''' SAメインメニューリフレッシュ関数名
    ''' </summary>
    Private Const PUSH_FuntionSA As String = "MainRefresh()"

    '2014/12/17 TMEJ 小澤 IT9824_NextSTEPサービス 洗車用端末開発向けた評価用アプリ作成 START

    ''' <summary>
    ''' CWメインメニューリフレッシュ関数名
    ''' </summary>
    Private Const PUSH_FuntionTabletCW As String = "MainRefresh()"

    '2014/12/17 TMEJ 小澤 IT9824_NextSTEPサービス 洗車用端末開発向けた評価用アプリ作成 END

#End Region

    '2013/12/02 TMEJ 張 タブレット版SMB チーフテクニシャン機能開発 START
#Region "JobDispatch送信関連"

#Region "作業実績送信使用フラグ"
    ''' <summary>
    ''' 作業実績送信使用フラグ
    ''' </summary>
    ''' <remarks></remarks>
    Private Const SysParaNameJobDispatchUseFlg As String = "JOBDISPATCH_USE_FLG"
#End Region

#End Region

#Region "システム設定名"

    ''' <summary>
    ''' 日付フォーマット
    ''' </summary>
    Private Const SysDateFormat = "DATE_FORMAT"

#End Region
    '2013/12/02 TMEJ 張 タブレット版SMB チーフテクニシャン機能開発 END

    '2019/12/06 NSK 皆川 (トライ店システム評価)整備作業における休憩時間取得判定向上検証 START
#Region "販売店システム設定名"
    ''' <summary>
    ''' 休憩取得自動判定フラグ
    ''' </summary>
    ''' <remarks></remarks>
    Private Const RestAutoJudgeFlg = "REST_AUTO_JUDGE_FLG"
#End Region
    '2019/12/06 NSK 皆川 (トライ店システム評価)整備作業における休憩時間取得判定向上検証 END

#Region "チップステータス"
    ''' <summary>
    ''' 未入庫(仮予約)
    ''' </summary>
    ''' <remarks></remarks>
    Private Const ChipStatusTentativeNotCarIn As String = "1"

    ''' <summary>
    ''' 未入庫(本予約)
    ''' </summary>
    ''' <remarks></remarks>
    Private Const ChipStatusConfirmedNotCarIn As String = "2"

    ''' <summary>
    ''' 作業開始待ち(仮予約)
    ''' </summary>
    ''' <remarks></remarks>
    Private Const ChipStatusTentativeWaitStart As String = "3"

    ''' <summary>
    ''' 作業開始待ち(本予約)
    ''' </summary>
    ''' <remarks></remarks>
    Private Const ChipStatusConfirmedWaitStart As String = "4"

    ''' <summary>
    ''' 仮置き
    ''' </summary>
    ''' <remarks></remarks>
    Private Const ChipStatusTemp As String = "5"

    ''' <summary>
    ''' 未来店客(本予約)
    ''' </summary>
    ''' <remarks></remarks>
    Private Const ChipStatusNoshow As String = "6"

    ''' <summary>
    ''' 飛び込み
    ''' </summary>
    ''' <remarks></remarks>
    Private Const ChipStatusWalkin As String = "7"

    ''' <summary>
    ''' 作業中
    ''' </summary>
    ''' <remarks></remarks>
    Private Const ChipStatusWorking As String = "8"

    ''' <summary>
    ''' 作業中断：部品欠品
    ''' </summary>
    Private Const ChipStatusStopForPartsStockout As String = "9"

    ''' <summary>
    ''' 作業中断：顧客連絡待ち
    ''' </summary>
    Private Const ChipStatusStopForWaitCustomer As String = "10"

    ''' <summary>
    ''' 作業中断：ストール待ち
    ''' </summary>
    Private Const ChipStatusStopForWaitStall As String = "11"

    ''' <summary>
    ''' 作業中断：その他
    ''' </summary>
    Private Const ChipStatusStopForOtherReason As String = "12"

    ''' <summary>
    ''' 作業中断：検査中断
    ''' </summary>
    Private Const ChipStatusStopForInspection As String = "13"

    ''' <summary>
    ''' 洗車待ち
    ''' </summary>
    Private Const ChipStatusWaitWash As String = "14"

    ''' <summary>
    ''' 洗車中
    ''' </summary>
    Private Const ChipStatusWashing As String = "15"

    ''' <summary>
    ''' 検査待ち
    ''' </summary>
    Private Const ChipStatusWaitInspection As String = "16"

    ''' <summary>
    ''' 検査中
    ''' </summary>
    Private Const ChipStatusInspecting As String = "17"

    ''' <summary>
    ''' 預かり中
    ''' </summary>
    Private Const ChipStatusKeeping As String = "18"

    ''' <summary>
    ''' 納車待ち
    ''' </summary>
    Private Const ChipStatusWaitDelivery As String = "19"

    ''' <summary>
    ''' 作業完了
    ''' </summary>
    Private Const ChipStatusJobFinish As String = "20"

    ''' <summary>
    ''' 日跨ぎ終了
    ''' </summary>
    Private Const ChipStatusDateCrossEnd As String = "21"

    ''' <summary>
    ''' 納車済み
    ''' </summary>
    Private Const ChipStatusDeliveryEnd As String = "22"

    ''' <summary>
    ''' 未来店客(仮予約)
    ''' </summary>
    ''' <remarks></remarks>
    Private Const ChipStatusTentativeNoShow As String = "24"

#End Region

#Region "中断理由区分"
    ''' <summary>
    ''' 部品欠品
    ''' </summary>
    Private Const StopReasonPartsStockOut As String = "01"
    ''' <summary>
    ''' お客様連絡待ち
    ''' </summary>
    Private Const StopReasonCustomerReportWaiting As String = "02"
    ''' <summary>
    ''' 検査不合格
    ''' </summary>
    Private Const StopReasonInspectionFailure As String = "03"
    ''' <summary>
    ''' その他
    ''' </summary>
    Private Const StopReasonOthers As String = "99"
#End Region

#Region "検査ステータス"
    '2013/12/02 TMEJ 張 タブレット版SMB チーフテクニシャン機能開発 START
    ''' <summary>
    ''' 完成検査未完了
    ''' </summary>
    Private Const InspectionNotFinish As String = "0"
    ''' <summary>
    ''' 完成検査完了
    ''' </summary>
    Private Const InspectionFinished As String = "2"
    '2013/12/02 TMEJ 張 タブレット版SMB チーフテクニシャン機能開発 END
    ''' <summary>
    ''' 完成検査承認まち
    ''' </summary>
    Private Const InspectionApproval As String = "1"

#End Region

#Region "受付区分"
    ''' <summary>
    ''' 予約客
    ''' </summary>
    Private Const AcceptanceTypeReserve As String = "0"

    ''' <summary>
    ''' WalkIn
    ''' </summary>
    Private Const AcceptanceTypeWalkin As String = "1"
#End Region

#Region "基幹コード区分"
    ''' <summary>
    ''' 販売店コード
    ''' </summary>
    Private Const DmsCodeTypeDealerCode As String = "1"

    ''' <summary>
    ''' 店舗コード
    ''' </summary>
    Private Const DmsCodeTypeBranchCode As String = "2"

    ''' <summary>
    ''' ストールID
    ''' </summary>
    Private Const DmsCodeTypeStallId As String = "3"

    ''' <summary>
    ''' 顧客分類
    ''' </summary>
    Private Const DmsCodeTypeCustomerType As String = "4"

    ''' <summary>
    ''' 作業ステータス
    ''' </summary>
    Private Const DmsCodeTypeJobStatus As String = "5"

    ''' <summary>
    ''' 中断理由区分
    ''' </summary>
    Private Const DmsCodeTypeStopReason As String = "6"

    ''' <summary>
    ''' チップステータス
    ''' </summary>
    Private Const DmsCodeTypeChipStatus As String = "7"

#End Region

#Region "ウェブサービスエラー"
    ''' <summary>
    ''' 最新のデータではないエラー
    ''' </summary>
    Private Const WebServiceRowLockVersionError As Long = 6014
#End Region

    '2013/11/29 TMEJ 丁 タブレット版SMB チーフテクニシャン機能開発 START
#Region "削除されていないユーザ(delflg=0)"
    ''' <summary>
    ''' 削除されていないユーザ(delflg=0)
    ''' </summary>
    ''' <remarks></remarks>
    Public Const DelFlgNone As String = "0"
#End Region

    ''' <summary>
    ''' 自社客
    ''' </summary>
    ''' <remarks></remarks>
    Private Const CustSegmentMyCustomer As String = "1"

    ''' <summary>
    ''' 未取引客
    ''' </summary>
    ''' <remarks></remarks>
    Private Const CustSegmentNewCustomer As String = "2"

    ''' <summary>
    ''' オペレーションコード「14: TC」
    ''' </summary>
    ''' <remarks></remarks>
    Private Const OperationTC As Long = 14

    ''' <summary>
    ''' オペレーションコード「52: SVR」
    ''' </summary>
    ''' <remarks></remarks>
    Private Const OperationSVR As Long = 52

    '2016/01/11 NSK 浅野 (トライ店システム評価)アカウント・組織・文言マスタの仕様変更対応 START
    ''' <summary>
    ''' オペレーションコードCT権限
    ''' </summary>
    ''' <remarks></remarks>
    Private Const OperationCT As Integer = 55

    ''' <summary>
    ''' オペレーションコードChT
    ''' </summary>
    ''' <remarks></remarks>
    Private Const OperationChT As Integer = 62
    '2016/01/11 NSK 浅野 (トライ店システム評価)アカウント・組織・文言マスタの仕様変更対応 END

#Region "通知用定数"

    ''' <summary>
    ''' 通知API用(カテゴリータイプ)
    ''' </summary>
    ''' <remarks></remarks>
    Private Const NotifyPushCategory As String = "1"

    ''' <summary>
    ''' 通知API用(表示位置)
    ''' </summary>
    ''' <remarks></remarks>
    Private Const NotifyPotisionType As String = "1"

    ''' <summary>
    ''' 通知API用(表示時間)
    ''' </summary>
    ''' <remarks></remarks>
    Private Const NotifyTime As Integer = 3

    ''' <summary>
    ''' 通知API用(表示タイプ)
    ''' </summary>
    ''' <remarks></remarks>
    Private Const NotifyDispType As String = "1"

    ''' <summary>
    ''' 通知API用(色)
    ''' </summary>
    ''' <remarks></remarks>
    Private Const NotifyColor As String = "1"

    ''' <summary>
    ''' 通知API用(呼び出し関数)
    ''' </summary>
    ''' <remarks></remarks>
    Private Const NotifyDispFunction As String = "icropScript.ui.setNotice()"

    ''' <summary>
    ''' 通知履歴のSessionValue(カンマ)
    ''' </summary>
    ''' <remarks></remarks>
    Private Const SessionValueKanma As String = ","

    ''' <summary>
    ''' 通知履歴のSessionValue(DMS販売店コード)
    ''' </summary>
    ''' <remarks></remarks>
    Private Const SessionValueDearlerCode As String = "Session.Param1,String,"

    ''' <summary>
    ''' 通知履歴のSessionValue(DMS店舗コード)
    ''' </summary>
    ''' <remarks></remarks>
    Private Const SessionValueBranchCode As String = "Session.Param2,String,"

    ''' <summary>
    ''' 通知履歴のSessionValue(アカウント)
    ''' </summary>
    ''' <remarks></remarks>
    Private Const SessionValueLoginUserID As String = "Session.Param3,String,"

    ''' <summary>
    ''' 通知履歴のSessionValue(来店実績連番)
    ''' </summary>
    ''' <remarks></remarks>
    Private Const SessionValueSAChipID As String = "Session.Param4,String,"

    ''' <summary>
    ''' 通知履歴のSessionValue(DMS予約ID)
    ''' </summary>
    ''' <remarks></remarks>
    Private Const SessionValueBASREZID As String = "Session.Param5,String,"

    ''' <summary>
    ''' 通知履歴のSessionValue(RO番号)
    ''' </summary>
    ''' <remarks></remarks>
    Private Const SessionValueR_O As String = "Session.Param6,String,"

    ''' <summary>
    ''' 通知履歴のSessionValue(RO作業連番)
    ''' </summary>
    ''' <remarks></remarks>
    Private Const SessionValueSEQ_NO As String = "Session.Param7,String,"

    ''' <summary>
    ''' 通知履歴のSessionValue(VIN)
    ''' </summary>
    ''' <remarks></remarks>
    Private Const SessionValueVIN_NO As String = "Session.Param8,String,"

    ''' <summary>
    ''' 通知履歴のSessionValue(RO作成フラグ)
    ''' </summary>
    ''' <remarks></remarks>
    Private Const SessionValueViewMode As String = "Session.Param9,String,"

    ''' <summary>
    ''' 通知履歴のSessionValue(「0：プレビュー」固定)
    ''' </summary>
    ''' <remarks></remarks>
    Private Const SessionValueFormat As String = "Session.Param10,String,"

    ''' <summary>
    ''' 通知履歴のSessionValue(入庫管理番号)
    ''' </summary>
    ''' <remarks></remarks>
    Private Const SessionValueSvcInNum As String = "Session.Param11,String,"

    ''' <summary>
    '''  通知履歴のSessionValue(入庫販売店コード)
    ''' </summary>
    ''' <remarks></remarks>
    Private Const SessionValueSvcInDlrCd As String = "Session.Param12,String,"

    ''' <summary>
    ''' 通知履歴のSessionValue(「5：R/O参照」固定)
    ''' </summary>
    ''' <remarks></remarks>
    Private Const SessionValueDisp_Num As String = "Session.DISP_NUM,String,"

    ''' <summary>
    ''' 通知履歴のSessionValue(顧客のDMSID)
    ''' </summary>
    ''' <remarks></remarks>
    Private Const SessionValueDmsCstId As String = "SessionKey.DMS_CST_ID,String,"

    ''' <summary>
    ''' 通知履歴のSessionValue(車両情報のVIN)
    ''' </summary>
    ''' <remarks></remarks>
    Private Const SessionValueVin As String = "SessionKey.VIN,String,"

    ''' <summary>
    ''' R/Oプレビューのリンク文字列
    ''' </summary>
    ''' <remarks></remarks>
    Private Const RoPreviewLink As String = "<a id='SC30105010' Class='SC3010501' href='/Website/Pages/SC3010501.aspx' onclick='return ServiceLinkClick(event)'>"

    ''' <summary>
    ''' 顧客名のリンク文字列(車両用)
    ''' </summary>
    ''' <remarks></remarks>
    Private Const CustomerVclLink As String = "<a id='SC30802251' Class='SC3080225' href='/Website/Pages/SC3080225.aspx' onclick='return ServiceLinkClick(event)'>"

    ''' <summary>
    ''' 顧客名のリンク文字列(顧客名用)
    ''' </summary>
    ''' <remarks></remarks>
    Private Const CustomerCstLink As String = "<a id='SC30802252' Class='SC3080225' href='/Website/Pages/SC3080225.aspx' onclick='return ServiceLinkClick(event)'>"

    ''' <summary>
    ''' Aタグ終了文字列
    ''' </summary>
    ''' <remarks></remarks>
    Private Const EndLikTag As String = "</a>"

    ''' <summary>
    ''' 敬称のポジション後
    ''' </summary>
    ''' <remarks></remarks>
    Private Const Position_Type_After As String = "1"

    ''' <summary>
    ''' 敬称のポジション前
    ''' </summary>
    ''' <remarks></remarks>
    Private Const Position_Type_Before As String = "2"

    ''' <summary>
    ''' サブチップボックス画面ID
    ''' </summary>
    ''' <remarks></remarks>
    Private Const ProgramId_SubChipBox As String = "SC3240301"

    ''' <summary>
    ''' SMB工程管理画面ID
    ''' </summary>
    ''' <remarks></remarks>
    Private Const ProgramId_Main As String = "SC3240101"

    '2015/02/09 TMEJ 範 DMS連携版サービスタブレット SMB納車予定時刻通知機能開発 START
    ''' <summary>プログラムID：TabletSMBCommon</summary>
    Private Const ProgramId_TabletSMBCommon As String = "TabletSMBCommon"
    '2015/02/09 TMEJ 範 DMS連携版サービスタブレット SMB納車予定時刻通知機能開発 END

#End Region

    '2013/11/29 TMEJ 丁 タブレット版SMB チーフテクニシャン機能開発 END

    '2017/09/27 NSK 竹中(悠) REQ-SVT-TMT-20170227-002 見込納車遅れ日時計算ロジック統一 START
#Region "各工程の有無"

    ''' <summary>
    ''' 洗車終了有無（0:洗車中）
    ''' </summary>
    ''' <remarks></remarks>
    Private Const CarWashEndTypeWashing As String = "0"
    ''' <summary>
    ''' 洗車終了有無（1:洗車終了）
    ''' </summary>
    ''' <remarks></remarks>
    Private Const CarWashEndTypeWashEnd As String = "1"

    ''' <summary>
    ''' RO情報有無（0:RO情報なし）
    ''' </summary>
    ''' <remarks></remarks>
    Private Const RepairOrderTypeNone As String = "0"
    ''' <summary>
    ''' RO情報有無（1:RO情報あり）
    ''' </summary>
    ''' <remarks></remarks>
    Private Const RepairOrderTypeExist As String = "1"

    ''' <summary>
    ''' 作業終了有無（0:作業中）
    ''' </summary>
    ''' <remarks></remarks>
    Private Const WorkEndTypeWorking As String = "0"
    ''' <summary>
    ''' 作業終了有無（1:作業終了）
    ''' </summary>
    ''' <remarks></remarks>
    Private Const WorkEndTypeWorkEnd As String = "1"
#End Region

#Region "予約関連"

    ''' <summary>
    '''  予約有効
    ''' </summary>
    Private Const ReserveEffective As String = "1"

#End Region
    '2017/09/27 NSK 竹中(悠) REQ-SVT-TMT-20170227-002 見込納車遅れ日時計算ロジック統一 END

    '2019/12/06 NSK 皆川 (トライ店システム評価)整備作業における休憩時間取得判定向上検証 START
#Region "休憩取得自動判定"
    ''' <summary>
    ''' 休憩自動判定する
    ''' </summary>
    ''' <remarks></remarks>
    Public Const RestAutoJudge As String = "1"
#End Region
    '2019/12/06 NSK 皆川 (トライ店システム評価)整備作業における休憩時間取得判定向上検証 END
#End Region

#Region "列挙型"
    Public Enum ActionResult As Integer
        ''' <summary>成功</summary>
        Success = 0
        ''' <summary>データが1件も取得できないエラー</summary>
        NoDataFound = 1
        ''' <summary>チップステータスチェックエラー</summary>
        CheckError = 2
        ''' <summary>追加作業確認チェックエラー</summary>
        CheckAddWorkError = 3
        ''' <summary>R/Oが紐づいていないエラー</summary>
        NotSetroNoError = 4
        ''' <summary>営業時間を超えるエラー</summary>
        OutOfWorkingTimeError = 5
        ''' <summary>予定開始日時に対する営業日が現在日時に対する営業日と異なるエラー</summary>
        NotStartDayError = 6
        ''' <summary>処理対象チップのサービス（整備内容）が未設定のエラー</summary>
        NotSetJobSvcClassIdError = 7
        ''' <summary>同一のストールに既に作業中のステータスチップが存在するエラー</summary>
        HasWorkingChipInOneStallError = 8
        ''' <summary>重複エラー(予約チップ)</summary>
        OverlapError = 9
        ''' <summary>重複エラー(使用不可チップ)</summary>
        OverlapUnavailableError = 10
        ''' <summary>チップエンティティの取得エラー</summary>
        GetChipEntityError = 11
        ''' <summary>行ロックバージョンエラー</summary>
        RowLockVersionError = 12
        ''' <summary>ストールロックエラー</summary>
        LockStallError = 13
        ''' <summary>DBタイムアウトエラー</summary>
        DBTimeOutError = 14
        ''' <summary>他システムとの連携エラー</summary>
        DmsLinkageError = 15
        ''' <summary>一意決定不可エラー</summary>
        NotUniqueDecisionError = 16
        ''' <summary>検査依頼中、終了不可エラー</summary>
        InspectionStatusFinishError = 17
        ''' <summary>検査依頼中、中断不可エラー</summary>
        InspectionStatusStopError = 18
        ''' <summary>親R/Oが作業開始されていないエラー</summary>
        ParentroNotStartedError = 19
        ''' <summary>テクニシャン未配置エラー</summary>
        NoTechnicianError = 20
        ''' <summary>1つストールにテクニシャンが4名以上(最大人数)を超えるエラー</summary>
        OverMaxTechnicianNumsError = 21
        ''' <summary>予期せぬエラー</summary>
        ExceptionError = 22

        '2013/11/29 TMEJ 丁 タブレット版SMB チーフテクニシャン機能開発 START
        ''' <summary>清算書印刷日時チェックエラー</summary>
        CheckInvoicePrintDateTimeError = 23
        '2013/11/29 TMEJ 丁 タブレット版SMB チーフテクニシャン機能開発 END

        '2013/12/02 TMEJ 張 タブレット版SMB チーフテクニシャン機能開発 START
        ''' <summary>部品ステータス情報取得タイムアウトエラー</summary>
        IC3802503ResultTimeOutError = 24
        ''' <summary>部品ステータス情報取得基幹側のエラー</summary>
        IC3802503ResultDmsError = 25
        ''' <summary>部品ステータス情報取得その他のエラー</summary>
        IC3802503ResultOtherError = 26
        ''' <summary>着工指示した作業終了時、実績データが持ってないエラー</summary>
        NoJobResultDataError = 27
        ''' <summary>関連チップ内で作業中チップが存在しているエラー</summary>
        HasStartedRelationChipError = 28
        '2013/12/02 TMEJ 張 タブレット版SMB チーフテクニシャン機能開発 END

        '2014/07/15 TMEJ 張 タブレットSMB Job Dispatch機能開発 START
        ''' <summary>作業ステータスチェックエラー</summary>
        InvalidJobStatusError = 29
        '2014/07/15 TMEJ 張 タブレットSMB Job Dispatch機能開発 END

        '2014/09/25 TMEJ 張 BTS-180 「洗車中に関連チップ作成すると予期せぬエラーメッセージ」対応 START 
        ''' <summary>洗車中、チップ変更不可エラー</summary>
        UnablePlanChipInWashingError = 30
        ''' <summary>検査中、チップ変更不可エラー</summary>
        UnablePlanChipInInspectingError = 31
        ''' <summary>納車後、チップ変更不可エラー</summary>
        UnablePlanChipAfterDeliveriedError = 32
        '2014/09/25 TMEJ 張 BTS-180 「洗車中に関連チップ作成すると予期せぬエラーメッセージ」対応 END

        '2014/12/08 TMEJ 丁 DMS連携版サービスタブレット JobDispatch完成検査入力制御開発 START
        ''' <summary>Jobの紐付き解除によるチップ終了エラー</summary>
        ChipFinishByJobUnInstructError = 33
        '2014/12/08 TMEJ 丁 DMS連携版サービスタブレット JobDispatch完成検査入力制御開発 END

        '2019/06/18 NSK 皆川 (トライ店システム評価)次世代サービスオペレーション効率化に向けた、業務適合性検証 START
        ''' <summary>
        ''' ストール使用不可と重複する配置である場合のエラー
        ''' </summary>
        ''' <remarks></remarks>
        ChipOverlapUnavailableError = 34
        '2019/06/18 NSK 皆川 (トライ店システム評価)次世代サービスオペレーション効率化に向けた、業務適合性検証 END

        '2015/04/23 TMEJ 明瀬  DMS連携版サービスタブレット強制納車機能追加開発 START

        ''' <summary>
        ''' DMS除外エラーの警告
        ''' </summary>
        ''' <remarks></remarks>
        WarningOmitDmsError = -9000

        '2015/04/23 TMEJ 明瀬  DMS連携版サービスタブレット強制納車機能追加開発 END

        '2015/10/08 TM 皆川 タブレットSMBのチップ移動時の連携送信メッセージ詳細化 START

        ''' <summary>
        ''' 予約送信IFエラーコード範囲の下限
        ''' </summary>
        ''' <remarks></remarks>
        IC3800903ResultRangeLower = 8000

        ''' <summary>
        ''' 予約送信IFエラーコード範囲の上限
        ''' </summary>
        ''' <remarks></remarks>
        IC3800903ResultRangeUpper = 8999

        '2015/10/08 TM 皆川 タブレットSMBのチップ移動時の連携送信メッセージ詳細化 END


    End Enum

    '2013/11/29 TMEJ 丁 タブレット版SMB チーフテクニシャン機能開発 START

    ''' <summary>
    ''' 作成するメッセージフラグ
    ''' </summary>
    ''' <remarks></remarks>
    Private Enum MessageType

        ''' <summary>
        ''' 顧客詳細画面リンクなし
        ''' </summary>
        ''' <remarks></remarks>
        CustomerLink_OFF = 0
        ''' <summary>
        ''' 顧客詳細画面リンクあり
        ''' </summary>
        ''' <remarks></remarks>
        CustomerLink_ON = 1

    End Enum

    '2013/11/29 TMEJ 丁 タブレット版SMB チーフテクニシャン機能開発 END

#End Region

#Region "標準時間"

    ''' <summary>
    ''' 納車準備_異常表示標準時間（分）
    ''' </summary>
    ''' <remarks></remarks>
    Private deliverypreAbnormalLt As Long = 0

#End Region
End Class



